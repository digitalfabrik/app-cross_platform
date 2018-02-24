using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Integreat.Localization;
using Integreat.Shared.Data.Loader.Targets;
using Integreat.Shared.Utilities;
using Integreat.Utilities;
using Plugin.Connectivity;
using Plugin.Connectivity.Abstractions;

namespace Integreat.Shared.Data.Loader
{
    /// <summary>
    /// Class DataLoader
    /// </summary>
    public class DataLoaderProvider
    {
        private const int NoReloadTimeout = 4;
        public readonly DisclaimerDataLoader DisclaimerDataLoader;
        public readonly EventPagesDataLoader EventPagesDataLoader;
        public readonly LanguagesDataLoader LanguagesDataLoader;
        public readonly LocationsDataLoader LocationsDataLoader;
        public readonly PagesDataLoader PagesDataLoader;

        private static readonly ConcurrentDictionary<string, bool> LoaderLocks = new ConcurrentDictionary<string, bool>();

        public DataLoaderProvider(DisclaimerDataLoader disclaimerDataLoader,
            EventPagesDataLoader eventPagesDataLoader,
            LanguagesDataLoader languagesDataLoader,
            LocationsDataLoader locationsDataLoader,
            PagesDataLoader pagesDataLoader)
        {
            DisclaimerDataLoader = disclaimerDataLoader;
            EventPagesDataLoader = eventPagesDataLoader;
            LanguagesDataLoader = languagesDataLoader;
            LocationsDataLoader = locationsDataLoader;
            PagesDataLoader = pagesDataLoader;
        }


        /// <summary>
        /// Executes the load method and performs thread secure action, as well as caching.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="forceRefresh">if set to <c>true</c> [force refresh].</param>
        /// <param name="caller">The caller.</param>
        /// <param name="loadMethod">The load method.</param>
        /// <param name="errorLogAction">The string that shall receive the error message.</param>
        /// <param name="worker">A action which will be executed, with the loaded data as parameter, after the data has been loaded from the network. (It will not be invoked, when the data is loaded from a cached file)</param>
        /// <param name="persistWorker">A action which will be executed before persisting a list. This is different to the other worker, as this one will also contain cached files, when a merge is being executed.</param>
        /// <param name="finishedAction">A action which will be executed, after data has been successfully loaded.</param>
        /// <returns></returns>
        public static async Task<ICollection<T>> ExecuteLoadMethod<T>(bool forceRefresh, IDataLoader caller, Func<Task<ICollection<T>>> loadMethod, Action<string> errorLogAction, Action<ICollection<T>> worker = null, Action<ICollection<T>> persistWorker = null, Action finishedAction = null)
        {
            // lock the file 
            await GetLockForFile(caller.FileName);
            var cachedFilePath = GetFileName(caller);
            if (CheckIfFileAlreadyExistAndCanUseCachedDataInstead(forceRefresh, caller, cachedFilePath))
            {
                await ReleaseLockForFile(caller.FileName);
                return LoadCachedData<T>(cachedFilePath);
            }

            // try to load the data from network
            ICollection<T> receivedList = null;
            // task that will load the data
            var task = Task.Run(() =>
            {
                receivedList = ExecuteLoadAndGetReceivedList(loadMethod, worker);
            });

            // start the work task and a task which will complete after a timeout simultaneously. If this task will finish first, we use the cached data instead.
            const int timeout = 50000; // 50 seconds timeout
            if (await Task.WhenAny(task, Task.Delay(timeout)) != task)
            {
                // timeout logic
                Debug.WriteLine("Timeout loading data: " + caller.FileName);
                // if a cached version exists, use it instead
                if (File.Exists(cachedFilePath))
                {
                    // load cached data
                    await ReleaseLockForFile(caller.FileName);
                    return IntegreatJsonConvert.DeserializeObject<Collection<T>>(File.ReadAllText(cachedFilePath));
                }
                await ReleaseLockForFile(caller.FileName);
                errorLogAction?.Invoke(AppResources.ErrorLoading);
                return new Collection<T>();
            }
            // loading task finished first, check if it failed (received list will be null)
            if (receivedList == null)
            {
                // if a cached version exists, use it instead
                if (File.Exists(cachedFilePath))
                {
                    // load cached data
                    await ReleaseLockForFile(caller.FileName);
                    errorLogAction?.Invoke(AppResources.ErrorInternet);
                    return IntegreatJsonConvert.DeserializeObject<Collection<T>>(File.ReadAllText(cachedFilePath));
                }

                // return empty list when it failed
                await ReleaseLockForFile(caller.FileName);
                errorLogAction?.Invoke(AppResources.ErrorLoading);
                return new Collection<T>();
            }

            // cache the file as serialized JSON
            // and there is no id element given, overwrite it (we assume we get the entire list every time). OR there is no cached version present
            if (caller.Id == null || !File.Exists(cachedFilePath) || forceRefresh)
            {
                persistWorker?.Invoke(receivedList);
                WriteFile(cachedFilePath, IntegreatJsonConvert.SerializeObject(receivedList), caller);
            }
            else
            {
                // otherwise we have to merge the loaded list, with the cached list
                var cachedList = IntegreatJsonConvert.DeserializeObject<Collection<T>>(File.ReadAllText(cachedFilePath));
                cachedList.Merge(receivedList, caller.Id);

                persistWorker?.Invoke(cachedList);

                // overwrite the cached data
                WriteFile(cachedFilePath, IntegreatJsonConvert.SerializeObject(cachedList), caller);

                // return the new merged list
                await ReleaseLockForFile(caller.FileName);
                finishedAction?.Invoke();
                return cachedList;
            }

            // finally, after writing the file return the just loaded list
            await ReleaseLockForFile(caller.FileName);
            finishedAction?.Invoke();
            return receivedList;
        }

        private static ICollection<T> ExecuteLoadAndGetReceivedList<T>(Func<Task<ICollection<T>>> loadMethod, Action<ICollection<T>> worker)
        {
            ICollection<T> receivedList;
            try
            {
                receivedList = loadMethod().Result;
                worker?.Invoke(receivedList);
            }
            catch (Exception e)
            {
                Debug.WriteLine("Error when loading data: " + e);
                return new Collection<T>();
            }
            return receivedList;
        }

        private static bool CheckIfFileAlreadyExistAndCanUseCachedDataInstead(bool forceRefresh, IDataLoader caller, string cachedFilePath)
        {
            return CheckIfFileExists(cachedFilePath) && CheckForCacheDataUsingInsteadNewLoading(forceRefresh, caller);
        }

        private static bool CheckIfFileExists(string cachedFilePath)
        {
            return File.Exists(cachedFilePath);
        }

        private static bool CheckForCacheDataUsingInsteadNewLoading(bool forceRefresh, IDataLoader caller)
        {
            var autoRefresh = !forceRefresh; // this refresh was NOT caused by the user, but automatically
            var timePassed = caller.LastUpdated.AddHours(NoReloadTimeout) >= DateTime.Now; // 4 hours or more have passed since last update
            var notConnected = !CrossConnectivity.Current.IsConnected; // the device is not connected to the Internet
            var refreshDenied = Preferences.WifiOnly && !CrossConnectivity.Current.ConnectionTypes.Contains(ConnectionType.WiFi); // when the app shall only auto refresh to wifi and is not connected to wifi

            // use the cached data, if this is an auto refresh call and the last update is not older than 4 hours
            // OR this is an auto refresh and the refresh is denied through the current connection type and user settings
            // OR the device is simply not connected to the Internet
            return (autoRefresh && timePassed) || (autoRefresh && refreshDenied) || notConnected;
        }

        private static Collection<T> LoadCachedData<T>(string cachedFilePath)
        {
            return IntegreatJsonConvert.DeserializeObject<Collection<T>>(File.ReadAllText(cachedFilePath));
        }

        private static string GetFileName(IDataLoader caller)
        {
            // check if a cached version exists
            return Constants.DatabaseFilePath + caller.FileName;
        }

        public static async Task<ICollection<T>> GetCachedFiles<T>(IDataLoader caller)
        {
            // lock the file 
            await GetLockForFile(caller.FileName);
            // check if a cached version exists
            var cachedFilePath = Constants.DatabaseFilePath + caller.FileName;
            if (File.Exists(cachedFilePath))
            {

                // load cached data
                await ReleaseLockForFile(caller.FileName);
                return IntegreatJsonConvert.DeserializeObject<Collection<T>>(File.ReadAllText(cachedFilePath));
            }

            // finally, after writing the file return the just loaded list
            await ReleaseLockForFile(caller.FileName);
            // if there is no file saved, return null
            return null;
        }

        public static async Task PersistFiles<T>(ICollection<T> data, IDataLoader caller)
        {
            // lock the file 
            await GetLockForFile(caller.FileName);
            // check if a cached version exists
            var cachedFilePath = Constants.DatabaseFilePath + caller.FileName;
            try
            {
                WriteFile(cachedFilePath, IntegreatJsonConvert.SerializeObject(data), caller, true);
            }
            catch (Exception e)
            {
                Debug.WriteLine("PersistFilesError: " + e);
                // ignored
            }
            finally
            {
                // finally, after writing the file return the just loaded list
                await ReleaseLockForFile(caller.FileName);
            }
        }
        private static async Task ReleaseLockForFile(string callerFileName)
        {
            while (!TryToUpdateLock(callerFileName, false, true)) await WaitForNextTry();
        }
        private static async Task GetLockForFile(string callerFileName)
        {
            while (true)
            {
                await PrepareKeyToLock(callerFileName);
                if (TryToUpdateLock(callerFileName, true, false))
                {
                    // if the method returns true, this thread achieved to update the lock. Therefore we're done and leave the method
                    return;
                }
            }
        }
        private static bool TryToUpdateLock(string callerFileName, bool isNewValue, bool isComparisionValue)
        {
            return LoaderLocks.TryUpdate(callerFileName, isNewValue, isComparisionValue);
        }
        private static async Task PrepareKeyToLock(string callerFileName)
        {
            while (TryToGetOrAddLockKey(callerFileName))
            {
                await WaitForNextTry();
            }
        }
        private static async Task WaitForNextTry()
        {
            await Task.Delay(500);
        }
        private static bool TryToGetOrAddLockKey(string callerFileName)
        {
            return LoaderLocks.GetOrAdd(callerFileName, false);
        }

        private static void WriteFile(string path, string text, IDataLoader caller, bool dontSetUpdateTime = false)
        {
            if (File.Exists(path)) File.Delete(path);
            File.WriteAllText(path, text);
            if (!dontSetUpdateTime)
                caller.LastUpdated = DateTime.Now;
        }
    }
}
