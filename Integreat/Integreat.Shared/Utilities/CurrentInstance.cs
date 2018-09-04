using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Integreat.Shared.Data.Loader;
using Integreat.Shared.Models;
using Integreat.Shared.Models.Extras;
using Integreat.Utilities;
using Xamarin.Forms;
using Page = Integreat.Shared.Models.Page;

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
            private set;
        }

        public Language Language
        {
            get;
            private set;
        }

        public ICollection<Page> Pages
        {
            get => _pages;
            private set
            {
                if (value != Pages)
                {
                    _pages = value;
                    MessagingCenter.Send<CurrentInstance>(this, Constants.PagesChangedMessage);
                }

            }
        }

        public ICollection<EventPage> Events
        {
            get => _events;
            private set
            {
                if (value != Events)
                {
                    _events = value;
                    MessagingCenter.Send<CurrentInstance>(this, Constants.EventsChangedMessage);
                }

            }
        }

        public ICollection<Extra> Extras
        {
            get => _extras;
            private set
            {
                if (value != Extras)
                {
                    _extras = value;
                    MessagingCenter.Send<CurrentInstance>(this, Constants.ExtrasChangedMessage);
                }

            }
        }

        public Disclaimer Disclaimer
        {
            get => _disclaimer;
            private set
            {
                if (value != Disclaimer)
                {
                    _disclaimer = value;
                    MessagingCenter.Send<CurrentInstance>(this, Constants.DisclaimerChangedMessage);
                }

            }
        }

        public bool HasInstance
        {
            get => _hasInstance;

            private set
            {
                if (value != HasInstance){
                    _hasInstance = value;
                    MessagingCenter.Send<CurrentInstance>(this, Constants.InstanceChangedMessage);
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
            Pages = await _dataLoaderProvider.PagesDataLoader.Load(false, _crntLanguage, _location);
            Events = await _dataLoaderProvider.EventPagesDataLoader.Load(false, _crntLanguage, _location);
            Extras = await _dataLoaderProvider.ExtrasDataLoader.Load(false, _crntLanguage, _location);
            var disclaimerCollection = await _dataLoaderProvider.DisclaimerDataLoader.Load(false, _crntLanguage, _location);

            Disclaimer = disclaimerCollection.FirstOrDefault();
        }

        public async void RefreshPages()
        {
            Pages = await _dataLoaderProvider.PagesDataLoader.Load(true, _crntLanguage, _location);
        }

        public async void RefreshEvents()
        {
            Events = await _dataLoaderProvider.EventPagesDataLoader.Load(true, _crntLanguage, _location);
        }

        public async void RefreshExtras()
        {
            Extras = await _dataLoaderProvider.ExtrasDataLoader.Load(true, _crntLanguage, _location);
        }

        public async void RefreshDisclaimer()
        {
            var disclaimerCollection = await _dataLoaderProvider.DisclaimerDataLoader.Load(false, _crntLanguage, _location);

            Disclaimer = disclaimerCollection.FirstOrDefault();
        }
    }
}
