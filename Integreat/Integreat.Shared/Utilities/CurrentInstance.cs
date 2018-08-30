using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Integreat.Shared.Data.Loader;
using Integreat.Shared.Models;
using Integreat.Shared.Models.Extras;

namespace Integreat.Shared.Utilities
{
    public class CurrentInstance
    {
        private Location _location;
        private IList<Language> _languages;
        private Language _crntLanguage;
        private ICollection<Page> _pages;
        private ICollection<EventPage> _events;
        private ICollection<Extra> _extras;
        private Disclaimer _disclaimer;

        private readonly DataLoaderProvider _dataLoaderProvider;

        private bool _hasInstance;

        public CurrentInstance(DataLoaderProvider dataLoaderProvider)
        {
            _languages = new List<Language>();
            _pages = new Collection<Page>();
            _events = new Collection<EventPage>();
            _extras = new Collection<Extra>();
            _dataLoaderProvider = dataLoaderProvider;

            _hasInstance = false;

            Initialize();
        }

        private async void Initialize()
        {
            //Load saved settings from preferences
            var locationId = Preferences.Location();
            if (locationId == -1)
                return;

            var languageId = Preferences.Language(locationId);
            if (languageId == String.Empty)
                return;

            Location = (await _dataLoaderProvider.LocationsDataLoader.Load(false)).FirstOrDefault(x => x.Id == locationId);
            Language = (await _dataLoaderProvider.LanguagesDataLoader.Load(false, Location)).FirstOrDefault(x => x.PrimaryKey == languageId);


            if (Location.Equals(null) || Language.Equals(null))
                return;

            _hasInstance = true;
        }

        #region properties

        public Location Location
        {
            get;
            set;
        }

        public Language Language
        {
            get;
            set;
        }

        public ICollection<Page> Pages => _pages;

        public bool HasInstance
        {
            get 
            {
                return _hasInstance;
            }

            private set
            {
                if (value != HasInstance)
                    _hasInstance = value;
            }
        }

        public ICollection<Language> AvailableLanguages => _languages;
        #endregion

        public void ChangeInstance(Location location, Language language)
        {
            if(HasInstance && location.Id == Location.Id)
            {
                if(language.Id != Language.Id){
                    Language = language;
                    LoadData();
                }
                return;
            }

            Location = location;
            Language = language;
            LoadData();

            HasInstance = true;
        }

        private void LoadData()
        {
            //load Pages, Events, Extras, Disclaimer

        }
    }
}
