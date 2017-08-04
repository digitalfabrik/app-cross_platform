using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using Integreat.Shared.Data.Loader.Targets;
using Integreat.Shared.Data.Services;
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
        public static void ClearCachedContent()
        {
            // delete all files
            try
            {
                File.Delete(Constants.DatabaseFilePath + DisclaimerDataLoader.FileNameConst);
                File.Delete(Constants.DatabaseFilePath + EventPagesDataLoader.FileNameConst);
                File.Delete(Constants.DatabaseFilePath + LanguagesDataLoader.FileNameConst);
                File.Delete(Constants.DatabaseFilePath + LocationsDataLoader.FileNameConst);
                File.Delete(Constants.DatabaseFilePath + PagesDataLoader.FileNameConst);
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
