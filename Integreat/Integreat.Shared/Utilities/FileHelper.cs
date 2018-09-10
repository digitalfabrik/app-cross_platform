using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace Integreat.Shared.Utilities
{
    public class FileHelper
    {
        /// <summary>
        /// Locks used to assure executions in order of LoadContent and LoadSettings methods and to avoid parallel executions.
        /// </summary>
        private readonly ConcurrentDictionary<string, bool> _loaderLocks;

        public FileHelper()
        {
            _loaderLocks = new ConcurrentDictionary<string, bool>();
        }

        public async Task ReleaseLock(string callerFileName)
        {
            while (!_loaderLocks.TryUpdate(callerFileName, false, true)) await Task.Delay(200);
        }

        public async Task GetLock(string callerFileName)
        {
            while (true)
            {
                // try to get the key, if it doesn't exist, add it. Try this until the value is false(is unlocked)
                while (_loaderLocks.GetOrAdd(callerFileName, false))
                {
                    // wait 500ms until the next try
                    await Task.Delay(500);
                }
                if (_loaderLocks.TryUpdate(callerFileName, true, false))
                {
                    // if the method returns true, this thread achieved to update the lock. Therefore we're done and leave the method
                    return;
                }
            }
        }
    }
}
