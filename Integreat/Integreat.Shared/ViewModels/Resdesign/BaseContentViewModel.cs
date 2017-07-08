using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;
using Integreat.Shared.Data.Loader;
using Integreat.Shared.Models;
using Integreat.Shared.Services.Tracking;
using Integreat.Shared.Utilities;

namespace Integreat.Shared.ViewModels.Resdesign
{
    /// <summary>
    /// Provides a base class for big content pages. Features methods to load/store/reload the selected location and language.
    /// </summary>
    public abstract class BaseContentViewModel : BaseViewModel
    {
        
        #region Fields

        protected readonly DataLoaderProvider _dataLoaderProvider; 
        private Location _lastLoadedLocation;
        private Language _lastLoadedLanguage;

        /// <summary>
        /// Locks used to assure executions in order of LoadContent and LoadSettings methods and to avoid parallel executions.
        /// </summary>
        private readonly ConcurrentDictionary<string, bool> _loaderLocks;

        private string _errorMessage;

        protected const string SettingsLockName = "Settings";
        protected const string ContentLockName = "Content";
        #endregion

        #region Properties

        public Location LastLoadedLocation
        {
            get { return _lastLoadedLocation; }
            set { SetProperty(ref _lastLoadedLocation, value); }
        } // the last loaded location

        public Language LastLoadedLanguage
        {
            get { return _lastLoadedLanguage; }
            set { SetProperty(ref _lastLoadedLanguage, value); }
        } // the last loaded language


        /// <summary>
        /// Gets or sets the error message that a view may display.
        /// </summary>
        public string ErrorMessage
        {
            get { return _errorMessage; }
            set
            {
                SetProperty(ref _errorMessage, value); 
                OnPropertyChanged(nameof(ErrorMessageVisible));
            }
        }

        /// <summary>
        /// Gets a value indicating whether the [error message should be visible].
        /// </summary>
        public bool ErrorMessageVisible => !string.IsNullOrWhiteSpace(ErrorMessage);

        #endregion

        protected BaseContentViewModel(IAnalyticsService analyticsService, DataLoaderProvider dataLoaderProvider) : base(analyticsService)
        {
            _dataLoaderProvider = dataLoaderProvider;
            _loaderLocks = new ConcurrentDictionary<string, bool>();
            LoadSettings();
        }

        /// <summary>
        /// Loads the location and language from the settings and finally loads their models from the persistence service.
        /// </summary>
        protected async void LoadSettings() {
            // wait until we're not busy anymore
            await GetLock(SettingsLockName);
            IsBusy = true;
            LastLoadedLocation = null;
            LastLoadedLanguage = null;
            var locationId = Preferences.Location();
            var languageId = Preferences.Language(locationId);
            LastLoadedLocation = (await _dataLoaderProvider.LocationsDataLoader.Load(false, err => ErrorMessage = err)).First(x => x.Id == locationId);
            LastLoadedLanguage = (await _dataLoaderProvider.LanguagesDataLoader.Load(false, LastLoadedLocation, err => ErrorMessage = err)).FirstOrDefault(x => x.PrimaryKey == languageId);

            IsBusy = false;
            await ReleaseLock(SettingsLockName);
        }

        /// <summary>
        /// Called when [refresh].
        /// </summary>
        /// <param name="force">if set to <c>true</c> [force].</param>
        public override async void OnRefresh(bool force = false) {
            // get locks for both settings and content, because we want to ensure that IF settings are loading right now, the content loader DOES wait for it
            await GetLock(SettingsLockName);
            await GetLock(ContentLockName);
            // reset error message
            ErrorMessage = null;
            try
            {
                LoadContent(force);
            }
            finally
            {
                await ReleaseLock(SettingsLockName);
                await ReleaseLock(ContentLockName);
            }
        }

        /// <summary>
        /// Called when [metadata changed].
        /// </summary>
        protected override void OnMetadataChanged() {
            LoadSettings();
            OnRefresh(true);
        }

        protected async Task ReleaseLock(string callerFileName)
        {
            while (!_loaderLocks.TryUpdate(callerFileName, false, true)) await Task.Delay(200);
        }

        protected async Task GetLock(string callerFileName) {
            while (true) {
                // try to get the key, if it doesn't exist, add it. Try this until the value is false(is unlocked)
                while (_loaderLocks.GetOrAdd(callerFileName, false)) {
                    // wait 500ms until the next try
                    await Task.Delay(500);
                };
                if (_loaderLocks.TryUpdate(callerFileName, true, false))
                {
                    // if the method returns true, this thread achieved to update the lock. Therefore we're done and leave the method
                    return;
                }
            }
        }

        /// <summary>
        /// Loads or reloads the content for the given language/location.
        /// </summary>
        /// <param name="forced">Whether the load is forced or not. A forced load will always result in fetching data from the server.</param>
        /// <param name="forLanguage">The language to load the content for.</param>
        /// <param name="forLocation">The location to load the content for.</param>
        protected abstract void LoadContent(bool forced = false, Language forLanguage = null, Location forLocation = null);
    }
}
