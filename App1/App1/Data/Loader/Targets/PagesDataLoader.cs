using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using App1.Data.Services;
using App1.Models;
using App1.Models.Event;
using App1.Utilities;
using App1.ViewModels;
using Xamarin.Forms;
using Page = App1.Models.Page;

namespace App1.Data.Loader.Targets
{
    /// <inheritdoc />
    /// <summary> DataLoader implementation for loading pages. </summary>
    public class PagesDataLoader : IDataLoader
    {
        /// <summary> File name used to cache pages. </summary>
        private const string FileNameConst = "pagesV3";
        /// <summary> Load service used for loading the data </summary>
        private readonly IDataLoadService _dataLoadService;
        private readonly IBackgroundLoader _backgroundLoader;
        private Location _lastLoadedLocation;
        private Language _lastLoadedLanguage;

        /// <summary> Initializes a new instance of PagesDataLoader </summary>
        /// <param name="dataLoadService">The load service used to load the data.</param>
        /// <param name="backgroundLoader"></param>
        public PagesDataLoader(IDataLoadService dataLoadService, IBackgroundLoader backgroundLoader)
        {
            _dataLoadService = dataLoadService;
            _backgroundLoader = backgroundLoader;
        }

        public string FileName { get; private set; }

        public DateTime LastUpdated
        {
            get => Preferences.LastPageUpdateTime<EventPage>(_lastLoadedLanguage, _lastLoadedLocation);
            // ReSharper disable once ValueParameterNotUsed
            set => Preferences.SetLastPageUpdateTime<EventPage>(_lastLoadedLanguage, _lastLoadedLocation, DateTime.Now);
        }

        public string Id => "Id";

        /// <summary> Whether the cached files have been updated since the last call of <c>GetCachedFiles</c> </summary>
        public bool CachedFilesHaveUpdated { get; private set; }

        /// <summary>  Returns a task to load the pages from the server (or load the cached files). </summary>
        /// <param name="forceRefresh">Whether to force refresh or not. When forced, the algorithm will always try to load from the server.</param>
        /// <param name="forLanguage">For which language the pages shall be loaded.</param>
        /// <param name="forLocation">For which location the pages shall be loaded.</param>
        /// <param name="errorLogAction">The error log action.</param>
        /// <returns></returns>
        public Task<Collection<Page>> Load(bool forceRefresh, Language forLanguage, Location forLocation,
            Action<string> errorLogAction = null)
        {
            _lastLoadedLocation = forLocation;
            _lastLoadedLanguage = forLanguage;

            FileName = $"{_lastLoadedLocation.NameWithoutStreetPrefix}_{_lastLoadedLanguage.ShortName}_{FileNameConst}.json";

            void FinishedAction()
            {
                if (_backgroundLoader.IsRunning)
                    _backgroundLoader.Stop();

                _backgroundLoader.Start(RefreshCommand, this);
            }

            // if the background download-er is not already running, start it. (this is for first time app startup)
            if (!_backgroundLoader.IsRunning)
                _backgroundLoader.Start(RefreshCommand, this);

            return DataLoaderProvider.ExecuteLoadMethod(forceRefresh, this,
                () => _dataLoadService.GetPages(forLanguage, forLocation),
                errorLogAction, null, null, FinishedAction);
        }

        /// <summary> Refresh Command used to trigger a non-forced refresh of all main pages </summary>
        private static void RefreshCommand()
            => Device.BeginInvokeOnMainThread(() => { ContentContainerViewModel.Current?.RefreshAll(); });

        /// <summary> Gets the current cached pages async. </summary>
        /// <returns>The cached pages. Null if there are none.</returns>
        public Task<Collection<Page>> GetCachedFiles()
        {
            CachedFilesHaveUpdated = false;
            return DataLoaderProvider.GetCachedFiles<Page>(this);
        }

        /// <summary> Overwrites the cached pages with the given collection. (Does not update the "LastUpdateTime" of the pages) </summary>
        /// <param name="data">Collection of pages to persist as the cached data.</param>
        public Task PersistFiles(Collection<Page> data)
        {
            // if the cached files have been
            if (CachedFilesHaveUpdated)
                throw new NotSupportedException(
                    "Files may not be persisted, after an update. Use GetCachedFiles again and persist them before the Load method gets executed again.");
            return DataLoaderProvider.PersistFiles(data, this);
        }
    }
}