using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Integreat.Shared.Data.Services;
using Integreat.Shared.Models;
using Integreat.Shared.Pages.Redesign;
using Integreat.Shared.Utilities;
using Integreat.Shared.ViewModels.Resdesign;
using Xamarin.Forms;
using Page = Integreat.Shared.Models.Page;

namespace Integreat.Shared.Data.Loader.Targets
{
    /// <summary>
    /// DataLoader implementation for loading pages.
    /// </summary>
    public class PagesDataLoader : IDataLoader
    {
        /// <summary>
        /// File name used to cache pages.
        /// </summary>
        public const string FileNameConst = "pagesV2";
        public string FileName => FileNameConst;
        public DateTime LastUpdated {
            get { return Preferences.LastPageUpdateTime<EventPage>(_lastLoadedLanguage, _lastLoadedLocation); }
            set { Preferences.SetLastPageUpdateTime<EventPage>(_lastLoadedLanguage, _lastLoadedLocation, DateTime.Now); }
        }

        public string Id => "Id";

        /// <summary>
        /// Load service used for loading the data
        /// </summary>
        private readonly IDataLoadService _dataLoadService;
        private Location _lastLoadedLocation;
        private Language _lastLoadedLanguage;
        private bool _cachedFilesHaveUpdated;

        /// <summary>
        /// Whether the cached files have been updated since the last call of <c>GetCachedFiles</c>
        /// </summary>
        public bool CachedFilesHaveUpdated => _cachedFilesHaveUpdated;

        /// <summary>
        /// Initializes a new instance of PagesDataLoader
        /// </summary>
        /// <param name="dataLoadService">The load service used to load the data.</param>
        public PagesDataLoader(IDataLoadService dataLoadService)
        {
            _dataLoadService = dataLoadService;
        }


        /// <summary>
        /// Returns a task to load the pages from the server (or load the cached files).
        /// </summary>
        /// <param name="forceRefresh">Whether to force refresh or not. When forced, the algorithm will always try to load from the server.</param>
        /// <param name="forLanguage">For which language the pages shall be loaded.</param>
        /// <param name="forLocation">For which location the pages shall be loaded.</param>
        /// <param name="errorLogAction">The error log action.</param>
        /// <returns></returns>
        public Task<Collection<Page>> Load(bool forceRefresh, Language forLanguage, Location forLocation, Action<string> errorLogAction = null)
        {
            _lastLoadedLocation = forLocation;
            _lastLoadedLanguage = forLanguage;

            // action which will be executed on newly loaded data
            Action<Collection<Page>> worker = pages =>
            {
                foreach (var page in pages)
                {
                    page.PrimaryKey = Page.GenerateKey(page.Id, forLocation, forLanguage);
                    if (!string.IsNullOrWhiteSpace(page.ParentJsonId))
                    {
                        page.ParentId = Page.GenerateKey(page.ParentJsonId, forLocation, forLanguage);
                    }

                }
            };

            // action which will be executed on the merged list of loaded and cached data
            Action<Collection<Page>> persistWorker = pages =>
            {
                // remove all pages which status is "trash"
                var itemsToRemove = pages.Where(x => x.Status == "trash").ToList();
                foreach (var page in itemsToRemove)
                {
                    pages.Remove(page);
                }

                // set flag that the cached files has been updated and a manual persist will be forbidden.
                _cachedFilesHaveUpdated = true;
            };

            Action finishedAction = () =>
            {
                if (BackgroundDownloader.IsRunning) BackgroundDownloader.Stop();
                BackgroundDownloader.Start(RefreshCommand, this);
            };


            // if the background downloader is not already running, start it. (this is for first time app startup)
            if (!BackgroundDownloader.IsRunning) BackgroundDownloader.Start(RefreshCommand, this);
            return DataLoaderProvider.ExecuteLoadMethod(forceRefresh, this, () => _dataLoadService.GetPages(forLanguage, forLocation, new UpdateTime(LastUpdated.Ticks)), errorLogAction, worker, persistWorker, finishedAction);
        }

        /// <summary>
        /// Refresh Command used to trigger a non-forced refresh of all main pages
        /// </summary>
        private void RefreshCommand()
        {
            Device.BeginInvokeOnMainThread(() => {
                ContentContainerViewModel.Current?.RefreshAll();
            });
        }

        /// <summary>
        /// Gets the current cached pages async.
        /// </summary>
        /// <returns>The cached pages. Null if there are none.</returns>
        public Task<Collection<Page>> GetCachedFiles()
        {
            _cachedFilesHaveUpdated = false;
            return DataLoaderProvider.GetCachedFiles<Page>(this);
        }

        /// <summary>
        /// Overwrites the cached pages with the given collection. (Does not update the "LastUpdateTime" of the pages)
        /// </summary>
        /// <param name="data">Collection of pages to persist as the cached data.</param>
        public Task PersistFiles(Collection<Page> data)
        {
            // if the cached files have been 
            if (CachedFilesHaveUpdated) throw new Exception("Files may not be persisted, after an update. Use GetCachedFiles again and persist them before the Load method gets executed again.");
            return DataLoaderProvider.PersistFiles(data, this);
        }
    }
}
