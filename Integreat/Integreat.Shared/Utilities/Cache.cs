using System;
using System.Diagnostics;
using System.IO;
using Integreat.Shared.Data.Loader.Targets;
using Integreat.Utilities;

namespace Integreat.Shared.Utilities
{
    /// <summary>
    /// Helper class to provide a global point to clear the cached files.
    /// </summary>
    public static class Cache
    {
        /// <summary>
        /// Clears all cached pages (locations, languages, main pages, events etc.).
        /// </summary>
        public static void ClearCachedContent(bool clearInstance = false)
        {
            // delete all files
            try
            {
                var path = Helpers.Platform.GetDatabasePath(false);
                string[] files = Directory.GetFiles(path, "*.json");
                foreach(string filePath in files){
                    File.Delete(filePath);
                }
            }
            catch (Exception e)
            {
                // log the error and throw if in debug build
                Debug.WriteLine(e);
#if DEBUG
                throw;
#endif
            }
        }

        /// <summary>
        /// Clears the cached resources including all pictures and PDF's (if there are any).
        /// </summary>
        public static void ClearCachedResources()
        {
            // go to each file in the directory used to store the files
            foreach (var file in Directory.EnumerateFiles(Constants.CachedFilePath))
            {
                try
                {
                    File.Delete(file);
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e);
#if DEBUG
                    throw;
#endif
                }
            }
        }

        /// <summary>
        /// Resets the settings to all default values.
        /// </summary>
        public static void ClearSettings()
        {
            Preferences.ClearAll();
        }
    }
}
