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

        public DataLoaderProvider(DisclaimerDataLoader disclaimerDataLoader, EventPagesDataLoader eventPagesDataLoader,
                                    LanguagesDataLoader languagesDataLoader, LocationsDataLoader locationsDataLoader,
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
        /// <param name="persistWorker">A action which will be executed before persisting a list. This is different to the other LoadDataFromNetwork, as this one will also contain cached files, when a merge is being executed.</param>
        /// <param name="finishedAction">A action which will be executed, after data has been successfully loaded.</param>
        /// <returns></returns>
        public static async Task<ICollection<T>> ExecuteLoadMethod<T>(bool forceRefresh, IDataLoader caller,
            Func<Task<ICollection<T>>> loadMethod, Action<string> errorLogAction, Action<ICollection<T>> worker = null,
            Action<ICollection<T>> persistWorker = null, Action finishedAction = null)
        {
            var callerFileName = caller.FileName;
            await LockFile(callerFileName);
            var cachedFilePath = GetPathOfCachedFile(callerFileName);
            var fileExist = CheckIfFileExists(cachedFilePath);
            if (CheckIfCanUseCachedDataInstead(forceRefresh, caller, fileExist))
            {
                return await LoadCachedData<T>(cachedFilePath, callerFileName);
            }

            // try to load the data from network
            ICollection<T> receivedList = null;
            // task that will load the data
            var task = Task.Run(() =>
            {
                receivedList = LoadDataFromNetwork(loadMethod, worker);
            });

            if (await LoadingNotCompleteBeforeTimeout(task))
            {
                // timeout logic
                Debug.WriteLine($"Timeout loading data: {callerFileName}");
                // if a cached version exists, use it instead
                if (fileExist)
                {
                    return await LoadCachedData<T>(File.ReadAllText(cachedFilePath), callerFileName);
                }
                return await EmptyCollectionByError<T>(callerFileName, AppResources.ErrorLoading, errorLogAction);
            }

            if (CheckIfReceivedListIsEmpty(receivedList))
            {
                // if a cached version exists, use it instead
                if (fileExist)
                {
                    return await LoadCachedDataByError<T>(cachedFilePath, callerFileName, AppResources.ErrorInternet, errorLogAction);
                }
                return await EmptyCollectionByError<T>(callerFileName, AppResources.ErrorLoading, errorLogAction);
            }

            // cache the file as serialized JSON
            // and there is no id element given, overwrite it (we assume we get the entire list every time). OR there is no cached version present
            if (caller.Id == null || !fileExist || forceRefresh)
            {
                persistWorker?.Invoke(receivedList);
                WriteFile(cachedFilePath, IntegreatJsonConvert.SerializeObject(receivedList), caller);
            }
            else
            {
                // otherwise we have to merge the loaded list, with the cached list
                var cachedList = GetMergedCechedList(cachedFilePath, receivedList, caller.Id);

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

        private static ICollection<T> GetMergedCechedList<T>(string cachedFilePath, IEnumerable<T> receivedList, string callerId)
        {
            var cachedList = GetObjectsFromFile<T>(cachedFilePath);
            cachedList.Merge(receivedList, callerId);
            return cachedList;
        }

        private static bool CheckIfReceivedListIsEmpty<T>(ICollection<T> receivedList)
        {
            return receivedList == null || !receivedList.Any();
        }

        private static async Task<ICollection<T>> EmptyCollectionByError<T>(string callerFileName, string errorMessage, Action<string> errorLogAction)
        {
            await ReleaseLockForFile(callerFileName);
            errorLogAction?.Invoke(errorMessage);
            return EmptyCollection<T>();
        }

        private static async Task<ICollection<T>> LoadCachedDataByError<T>(string cachedFilePath, string callerFileName, string errorMessage, Action<string> errorLogAction)
        {
            errorLogAction?.Invoke(errorMessage);
            return await LoadCachedData<T>(File.ReadAllText(cachedFilePath), callerFileName);
        }

        private static ICollection<T> EmptyCollection<T>()
        {
            return new Collection<T>();
        }

        private static async Task<bool> LoadingNotCompleteBeforeTimeout(Task taskToComplete)
        {
            const int timeout = 50000;
            return await Task.WhenAny(taskToComplete, Task.Delay(timeout)) != taskToComplete;
        }

        private static ICollection<T> LoadDataFromNetwork<T>(Func<Task<ICollection<T>>> loadMethod, Action<ICollection<T>> worker)
        {
            ICollection<T> receivedList;
            try
            {
                receivedList = loadMethod().Result;
                worker?.Invoke(receivedList);
            }
            catch (Exception exception)
            {
                Debug.WriteLine($"Error when loading data: {exception.Message}");
                return EmptyCollection<T>();
            }
            return receivedList;
        }

        private static bool CheckIfCanUseCachedDataInstead(bool forceRefresh, IDataLoader caller, bool fileExist)
        {
            return fileExist && CheckForCacheDataUsingInsteadNewLoading(forceRefresh, caller);
        }

        private static bool CheckIfFileExists(string cachedFilePath)
        {
            return File.Exists(cachedFilePath);
        }

        private static bool CheckForCacheDataUsingInsteadNewLoading(bool forceRefresh, IDataLoader caller)
        {
            var timePassed = caller.LastUpdated.AddHours(NoReloadTimeout) >= DateTime.Now; // 4 hours or more have passed since last update
            var notConnected = !CrossConnectivity.Current.IsConnected; // the device is not connected to the Internet
            var refreshDenied = Preferences.WifiOnly && !CrossConnectivity.Current.ConnectionTypes.Contains(ConnectionType.WiFi); // when the app shall only auto refresh to wifi and is not connected to wifi

            // use the cached data, if this is an auto refresh call and the last update is not older than 4 hours
            // OR this is an auto refresh and the refresh is denied through the current connection type and user settings
            // OR the device is simply not connected to the Internet
            return (!forceRefresh && timePassed) || (!forceRefresh && refreshDenied) || notConnected;
        }

        private static async Task<ICollection<T>> LoadCachedData<T>(string cachedFilePath, string fileName)
        {
            await ReleaseLockForFile(fileName);
            return GetObjectsFromFile<T>(cachedFilePath);
        }

        private static ICollection<T> GetObjectsFromFile<T>(string cachedFilePath)
        {
            return IntegreatJsonConvert.DeserializeObject<ICollection<T>>(File.ReadAllText(cachedFilePath));
        }

        private static string GetPathOfCachedFile(string fileName)
        {
            return Constants.DatabaseFilePath + fileName;
        }

        public static async Task<ICollection<T>> GetCachedFiles<T>(IDataLoader caller)
        {
            // lock the file 
            await LockFile(caller.FileName);
            // check if a cached version exists
            var cachedFilePath = Constants.DatabaseFilePath + caller.FileName;
            if (File.Exists(cachedFilePath))
            {

                // load cached data
                await ReleaseLockForFile(caller.FileName);
                return GetObjectsFromFile<T>(cachedFilePath);
            }

            // finally, after writing the file return the just loaded list
            await ReleaseLockForFile(caller.FileName);
            // if there is no file saved, return null
            return EmptyCollection<T>();
        }

        public static async Task PersistFiles<T>(ICollection<T> data, IDataLoader caller)
        {
            var callerFileName = caller.FileName;
            await LockFile(callerFileName);
            // check if a cached version exists
            var cachedFilePath = GetPathOfCachedFile(callerFileName);
            try
            {
                WriteFile(cachedFilePath, IntegreatJsonConvert.SerializeObject(data), caller, true);
            }
            catch (Exception exception)
            {
                Debug.WriteLine($"PersistFilesError: {exception}");
                // ignored
            }
            finally
            {
                // finally, after writing the file return the just loaded list
                await ReleaseLockForFile(callerFileName);
            }
        }
        private static async Task ReleaseLockForFile(string callerFileName)
        {
            while (!TryToUpdateLock(callerFileName, false, true)) await WaitForNextTry();
        }
        private static async Task WaitForNextTry()
        {
            await Task.Delay(500);
        }
        private static bool TryToUpdateLock(string callerFileName, bool isNewValue, bool isComparisionValue)
        {
            return LoaderLocks.TryUpdate(callerFileName, isNewValue, isComparisionValue);
        }
        private static async Task LockFile(string callerFileName)
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
        private static async Task PrepareKeyToLock(string callerFileName)
        {
            while (TryToGetOrAddLockKey(callerFileName))
            {
                await WaitForNextTry();
            }
        } 
        private static bool TryToGetOrAddLockKey(string callerFileName)
        {
            return LoaderLocks.GetOrAdd(callerFileName, false);
        }
        private static void WriteFile(string path, string text, IDataLoader caller, bool dontSetUpdateTime = false)
        {
            if (CheckIfFileExists(path))
            {
                File.Delete(path);
            }
            File.WriteAllText(path, text);
            if (!dontSetUpdateTime)
                caller.LastUpdated = DateTime.Now;
        }
    }
}
