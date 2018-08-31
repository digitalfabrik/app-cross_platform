using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Integreat.Shared.Data.Loader;
using Integreat.Shared.Models;
using Integreat.Shared.Models.Extras;
using Xamarin.Forms;

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

        public ICollection<Event> Events
        {
            get;
            set;
        }

        public ICollection<Extra> Extras
        {
            get;
            set;
        }

        public Disclaimer Disclaimer
        {
            get;
            set;
        }

        public bool HasInstance
        {
            get 
            {
                return _hasInstance;
            }

            private set
            {
                if (value != HasInstance){
                    _hasInstance = value;
                    MessagingCenter.Send<CurrentInstance>(this, )
                }
                    
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

        private async void LoadData()
        {
            //load Pages, Events, Extras, Disclaimer
            _pages = await _dataLoaderProvider.PagesDataLoader.Load(false, _crntLanguage, _location);
            _events = await _dataLoaderProvider.EventPagesDataLoader.Load(false, _crntLanguage, _location);
            _extras = await _dataLoaderProvider.ExtrasDataLoader.Load(false, _crntLanguage, _location);
            var disclaimerCollection = await _dataLoaderProvider.DisclaimerDataLoader.Load(false, _crntLanguage, _location);

            _disclaimer = disclaimerCollection.FirstOrDefault();
        }

        public async void RefreshPages()
        {
            _pages = await _dataLoaderProvider.PagesDataLoader.Load(true, _crntLanguage, _location);
        }

        public async void RefreshEvents()
        {
            _events = await _dataLoaderProvider.EventPagesDataLoader.Load(true, _crntLanguage, _location);
        }

        public async void RefreshExtras()
        {
            _extras = await _dataLoaderProvider.ExtrasDataLoader.Load(true, _crntLanguage, _location);
        }

        public async void RefreshDisclaimer()
        {
            var disclaimerCollection = await _dataLoaderProvider.DisclaimerDataLoader.Load(false, _crntLanguage, _location);

            _disclaimer = disclaimerCollection.FirstOrDefault();
        }
    }
}
