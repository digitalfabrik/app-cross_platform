using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Integreat.Shared.Data.Loader.Targets;
using Integreat.Utilities;

namespace Integreat.Shared.Data.Services
{
    /// <summary>
    /// Class which provides functionality to go through each page and download each content piece (png's, pdf's etc.)
    /// </summary>
    public static class BackgroundDownloader
    {

        private static Task _workerTask;
        private static CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
        private static HttpClient _client = new HttpClient();
        private static PagesDataLoader _pagesdataLoader;

        /// <summary>
        /// Gets a value indicating whether the downloader is running.
        /// </summary>
        /// <value>
        ///   <c>true</c> if downloader is running; otherwise, <c>false</c>.
        /// </value>
        public static bool IsRunning => _workerTask != null;


        /// <summary>
        /// Starts the background downloading.
        /// </summary>
        public static void Start(Action refreshCommand, PagesDataLoader pagesdataLoader)
        {
            if (IsRunning) throw new Exception("BackgroundDownloader is already running.");
            _pagesdataLoader = pagesdataLoader;
            _workerTask = Task.Run(() =>
            {
                try
                {
                    Worker(_cancellationTokenSource.Token, refreshCommand);
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e);
                    // ignored (will only appear, when cancellation was requested)
                }

                _cancellationTokenSource.Token.ThrowIfCancellationRequested();

            }, _cancellationTokenSource.Token); // Pass same token to StartNew.
        }

        /// <summary>
        /// Stops this background downloading.
        /// </summary>
        public static void Stop()
        {
            if (!IsRunning) throw new Exception("BackgroundDownloader is not running.");
            _cancellationTokenSource.Cancel(true);
            _client.CancelPendingRequests();
            try
            {
                _workerTask.Wait();
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                // ignored
            }
            finally
            {
                _cancellationTokenSource.Dispose();
                _cancellationTokenSource = new CancellationTokenSource();
                _client = new HttpClient();
                _workerTask = null;
            }
        }

        /// <summary>
        /// The actual performing code of the background downloader.
        /// </summary>
        /// <param name="token"></param>
        /// <param name="refreshCommand"></param>
        private static void Worker(CancellationToken token, Action refreshCommand)
        {
            var pages = _pagesdataLoader.GetCachedFiles().Result;
            foreach (var page in pages)
            {
                // regex which will find only valid URL's for images and pdfs
                //var res = Regex.Replace(page.Content, "https?:\\/\\/(www\\.)?[-a-zA-Z0-9@:%._\\+~#=]{2,256}\\.[a-z]{2,6}\\b([-a-zA-Z0-9@:%_\\+.~#?&//=]*)(jpg|png|jpeg){1}", UrlReplacer);
				var res = Regex.Replace(page.Content, "https?:\\/\\/(www\\.)?[-a-zA-Z0-9@:%._\\+~#=]{2,256}\\.[a-z]{2,6}\\b([-a-zA-Z0-9@:%_\\+.~#?&//=]*)(jpg|png|jpeg|pdf){1}", UrlReplacer);

				page.Content = res;

                // abort when cancellation is requested
                _cancellationTokenSource.Token.ThrowIfCancellationRequested();
            }
            // persist pages if they have not been reloaded in the meantime
            if (_pagesdataLoader.CachedFilesHaveUpdated) return;
            _pagesdataLoader.PersistFiles(pages);

            // cause a non forced refresh of all pages
            refreshCommand?.Invoke();
        }

        private static string UrlReplacer(Match match)
        {
            _cancellationTokenSource.Token.ThrowIfCancellationRequested();
            Debug.WriteLine(match.Value);
            var fileName = match.Value.Split('/').Last();
            var localPath = Constants.CachedFilePath + fileName;

            // check if the file is already cached. If so, already return the localPath
            if (File.Exists(localPath))
            {
                Debug.WriteLine("File already exists: " + localPath);
                return localPath;
            }

            try
            {                
                var bytes = _client.GetByteArrayAsync(new Uri(match.Value)).Result;
                _cancellationTokenSource.Token.ThrowIfCancellationRequested();
                File.WriteAllBytes(localPath, bytes);
                _cancellationTokenSource.Token.ThrowIfCancellationRequested();
            }
            catch (Exception e)
            {
                // when any error occurs, keep the old online URL
                Debug.WriteLine("An error occured while background downloading a file: " + match.Value + "\n Error: " + e);
                return match.Value;
            }
            Debug.WriteLine("SUCCESSFULLY CACHED: " + fileName);

            // when the download succeeded, replace the page with the local path
            return localPath;
        }
    }
}
