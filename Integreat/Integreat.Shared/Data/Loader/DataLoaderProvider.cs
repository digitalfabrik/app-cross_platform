using System;
using System.Collections.Concurrent;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Integreat.Shared.Data.Loader.Targets;
using Integreat.Shared.Utilities;
using Integreat.Utilities;
using Newtonsoft.Json;
using Refit;

namespace Integreat.Shared.Data.Loader {
    public class DataLoaderProvider {
        private const int NoReloadTimeout = 4;
        public readonly DisclaimerDataLoader DisclaimerDataLoader;
        public readonly EventPagesDataLoader EventPagesDataLoader;
        public readonly LanguagesDataLoader LanguagesDataLoader;
        public readonly LocationsDataLoader LocationsDataLoader;
        public readonly PagesDataLoader PagesDataLoader;

        private static readonly ConcurrentDictionary<string, bool> LoaderLocks = new ConcurrentDictionary<string, bool>();

        public DataLoaderProvider(DisclaimerDataLoader disclaimerDataLoader, EventPagesDataLoader eventPagesDataLoader,LanguagesDataLoader languagesDataLoader, LocationsDataLoader locationsDataLoader, PagesDataLoader pagesDataLoader)
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
        /// <param name="worker">A action which will be executed, with the loaded data as parameter, after the data has been loaded from the network. (It will not be invoked, when the data is loaded from a cached file)</param>
        /// <param name="persistWorker">A action which will be executed before persisting a list. This is different to the other worker, as this one will also contain cached files, when a merge is being executed.</param>
        public static async Task<Collection<T>> ExecuteLoadMethod<T>(bool forceRefresh, IDataLoader caller, Func<Task<Collection<T>>> loadMethod, Action<Collection<T>> worker = null, Action<Collection<T>> persistWorker = null)
        {
            // lock the file 
            await GetLock(caller.FileName);
            // check if a cached version exists
            var cachedFilePath = Constants.DatabaseFilePath + caller.FileName;
            if (File.Exists(cachedFilePath)) {
                // if so, when we did NOT force refresh and the last time updated is no longer ago than 4 hours, use the cached data
                if (!forceRefresh && caller.LastUpdated.AddHours(NoReloadTimeout) >= DateTime.Now) {
                    // load cached data
                    await ReleaseLock(caller.FileName);
                    return JsonConvert.DeserializeObject<Collection<T>>(File.ReadAllText(cachedFilePath));
                }
            }

            // try to load the data from network
            Collection<T> receivedList;
            try
            {
                receivedList = await loadMethod();
                worker?.Invoke(receivedList);
            }
            catch (Exception e)
            {
                // return empty list when it failed
                Debug.WriteLine("Error when loading data: " + e);
                await ReleaseLock(caller.FileName);
                return new Collection<T>();
            }

            // cache the file as serialized JSON
            // and there is no id element given, overwrite it (we assume we get the entire list every time). OR there is no cached version present
            if (caller.Id == null || !File.Exists(cachedFilePath) || forceRefresh) {
                persistWorker?.Invoke(receivedList);
                WriteFile(cachedFilePath, JsonConvert.SerializeObject(receivedList), caller);
            } else {
                // otherwise we have to merge the loaded list, with the cached list
                var cachedList = JsonConvert.DeserializeObject<Collection<T>>(File.ReadAllText(cachedFilePath));
                cachedList.Merge(receivedList, caller.Id);
                
                persistWorker?.Invoke(cachedList);

                // overwrite the cached data
                WriteFile(cachedFilePath, JsonConvert.SerializeObject(cachedList), caller);

                // return the new merged list
                await ReleaseLock(caller.FileName);
                return cachedList;
            }

            // finally, after writing the file return the just loaded list
            await ReleaseLock(caller.FileName);
            return receivedList;
        }

        private static async Task ReleaseLock(string callerFileName)
        {
            while (!LoaderLocks.TryUpdate(callerFileName, false, true)) await Task.Delay(200);
        }

        private static async Task GetLock(string callerFileName) {
            while (true) {
                // try to get the key, if it doesn't exist, add it. Try this until the value is false(is unlocked)
                while (LoaderLocks.GetOrAdd(callerFileName, false)) {
                    // wait 500ms until the next try
                    await Task.Delay(500);
                };
                if (LoaderLocks.TryUpdate(callerFileName, true, false))
                {
                    // if the method returns true, this thread achieved to update the lock. Therefore we're done and leave the method
                    return;
                }
            }
        }

        private static void WriteFile(string path, string text, IDataLoader caller) {
            if (File.Exists(path)) File.Delete(path);
            File.WriteAllText(path, text);
            caller.LastUpdated = DateTime.Now;
        }
    }
}
