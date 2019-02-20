using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace App1.Utilities
{
    /// <summary>
    /// Class DirectorySize gets information about the storage usage of the app.
    /// </summary>
    public static class DirectorySize
    {
        /// <summary>
        /// Calculates the size of the directory.
        /// </summary>
        /// <param name="directoryPath">The directory path.</param>
        /// <returns>Size in bytes.</returns>
        public static Task<long> CalculateDirectorySizeAsync(string directoryPath)
        {
            return Task.Run(() =>
            {
                long size = 0;
                try
                {
                    size += Directory.EnumerateFiles(directoryPath).Select(file => new FileInfo(file)).Select(info => info.Length).Sum();
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e);
#if DEBUG
                    throw;
#else
                    return -1;
#endif      
                }
                return size;
            });
        }
    }
}
