using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Integreat.Shared.Data.Loader;
using Integreat.Shared.Models;
using Integreat.Shared.Services.Persistence;
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

        #endregion

        protected BaseContentViewModel(IAnalyticsService analyticsService, DataLoaderProvider dataLoaderProvider) : base(analyticsService)
        {
            _dataLoaderProvider = dataLoaderProvider;
            //LoadSettings();
        }

        /// <summary>
        /// Loads the location and language from the settings and finally loads their models from the persistence service.
        /// </summary>
        protected async void LoadSettings() {
            // wait until we're not busy anymore
            await Task.Run(() => {
                while (IsBusy) ;
            });
            IsBusy = true;
            var locationId = Preferences.Location();
            var languageId = Preferences.Language(locationId);
            LastLoadedLocation = (await _dataLoaderProvider.LocationsDataLoader.Load(false)).First(x => x.Id == locationId);
            LastLoadedLanguage = (await _dataLoaderProvider.LanguagesDataLoader.Load(false, LastLoadedLocation)).FirstOrDefault(x => x.PrimaryKey == languageId);
            IsBusy = false;
        }

        /// <summary>
        /// Called when [refresh].
        /// </summary>
        /// <param name="force">if set to <c>true</c> [force].</param>
        public override async void OnRefresh(bool force = false) {
            // wait until we're not busy anymore
            await Task.Run(() => {
                while (IsBusy) ;
            });
            LoadContent(force);
        }

        /// <summary>
        /// Called when [metadata changed].
        /// </summary>
        protected override void OnMetadataChanged() {
            LoadSettings();
            OnRefresh(true);
        }



        /// <summary>
        /// Loads or reloads the content for the given language/location.
        /// </summary>
        /// <param name="forced">Whether the load is forced or not. A forced load will always result in fetching data from the server.</param>
        /// <param name="forLanguage">The language to load the content for.</param>
        /// <param name="forLocation">The location to load the content for.</param>
        public abstract void LoadContent(bool forced = false, Language forLanguage = null, Location forLocation = null);
    }
}
