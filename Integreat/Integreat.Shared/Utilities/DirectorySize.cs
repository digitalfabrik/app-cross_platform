using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace Integreat.Shared.Utilities
{
    /// <summary>
    /// Class to Calculate the used Memory size
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
                    foreach (var file in Directory.EnumerateFiles(directoryPath))
                    {
                        var info = new FileInfo(file);
                        size += info.Length;
                    }
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
