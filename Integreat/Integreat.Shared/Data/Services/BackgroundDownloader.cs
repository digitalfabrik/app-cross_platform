using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Integreat.Shared.Data.Factories;
using Integreat.Shared.Data.Loader.Targets;
using Integreat.Utilities;

namespace Integreat.Shared.Data.Services
{
    /// <inheritdoc />
    public class BackgroundDownloader : IBackgroundLoader
    {
        private HttpClient _client;

        public BackgroundDownloader(HttpClient client)
        {
            _client = client;
        }

        /// <inheritdoc />
        public bool IsRunning => WorkerTask != null;

        /// <inheritdoc />
        public void Start(Action refreshCommand, PagesDataLoader pagesdataLoader)
        {
            if (IsRunning)
            {
                throw new NotSupportedException("BackgroundDownloader is already running.");
            }
            PagesdataLoader = pagesdataLoader;

            WorkerTask = LoadData(refreshCommand);
        }

        private async Task LoadData(Action refreshCommand)
        {
            var pages = await PagesdataLoader.GetCachedFilesAsync();
            var eventPages = await PagesdataLoader.GetCachedFilesAsync();

            try
            {
                await Worker(pages, x => x.Content);
                await Worker(eventPages, x => x.Content);
            }
            catch (Exception)
            {
                return;
            }

            // persist pages if they have not been reloaded in the meantime
            if (PagesdataLoader.CachedFilesHaveUpdated)
               await PagesdataLoader.PersistFiles(pages);

            refreshCommand?.Invoke();
        }

        /// <inheritdoc />
        public void Stop()
        {
            if (!IsRunning)
            {
                throw new NotSupportedException("BackgroundDownloader is not running.");
            }
            CancellationTokenSource.Cancel(true);
            _client.CancelPendingRequests();
            try
            {
                WorkerTask.Wait();
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                // ignored
            }
            finally
            {
                CancellationTokenSource.Dispose();
                CancellationTokenSource = new CancellationTokenSource();
                _client = HttpClientFactory.GetHttpClient(new Uri(Constants.IntegreatReleaseUrl));
                WorkerTask = null;
            }
        }

        private static PagesDataLoader PagesdataLoader { get; set; }

        private static Task WorkerTask { get; set; }

        private static CancellationTokenSource CancellationTokenSource { get; set; } = new CancellationTokenSource();

        /// <summary>
        /// The actual performing code of the background downloader.
        /// </summary>
        private async Task Worker<T>(IEnumerable<T> targets, Expression<Func<T, string>> valueExpr)
        {
            await Task.Run(() =>
            {
                var expr = (MemberExpression) valueExpr.Body;
                var prop = (PropertyInfo) expr.Member;

                foreach (var target in targets)
                {
                    var targetContents = prop.GetValue(target)?.ToString();

                    if (targetContents == null)
                        continue;

                    // regex which will find only valid URL's for images and pdfs
                    var res = Regex.Replace(targetContents,
                        "https?:\\/\\/(www\\.)?[-a-zA-Z0-9@:%._\\+~#=]{2,256}\\.[a-z]{2,6}\\b([-a-zA-Z0-9@:%_\\+.~#?&//=]*)(jpg|png|jpeg|pdf){1}",
                        UrlReplacer);

                    // abort when cancellation is requested
                    CancellationTokenSource.Token.ThrowIfCancellationRequested();
                    prop.SetValue(target, res);
                }
            });
        }

        /// <summary> URLs the replacer. </summary>
        /// <param name="match">The match.</param>
        /// <returns></returns>
        private string UrlReplacer(Match match)
        {
            CancellationTokenSource.Token.ThrowIfCancellationRequested();
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
                CancellationTokenSource.Token.ThrowIfCancellationRequested();
                File.WriteAllBytes(localPath, bytes);
                CancellationTokenSource.Token.ThrowIfCancellationRequested();
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

    /// <summary>
    /// Class which provides functionality to go through each page and download each content piece (png's, pdf's etc.)
    /// </summary>
    public interface IBackgroundLoader
    {
        /// <summary>
        /// Gets a value indicating whether the downloader is running.
        /// </summary>
        /// <value>
        ///   <c>true</c> if downloader is running; otherwise, <c>false</c>.
        /// </value>
        bool IsRunning { get; }

        /// <summary>
        /// Starts the background downloading.
        /// </summary>
        void Start(Action refreshCommand, PagesDataLoader pagesdataLoader);

        /// <summary>
        /// Stops this background downloading.
        /// </summary>
        void Stop();
    }
}
