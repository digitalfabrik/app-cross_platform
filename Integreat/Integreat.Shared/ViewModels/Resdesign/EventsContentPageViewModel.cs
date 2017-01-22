using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Integreat.Shared.Models;
using Integreat.Shared.Services;
using Integreat.Shared.Services.Loader;
using Integreat.Shared.Services.Persistence;
using Integreat.Shared.Services.Tracking;
using Integreat.Shared.Utilities;
using Integreat.Shared.ViewModels.Resdesign.Events;
using Xamarin.Forms;

namespace Integreat.Shared.ViewModels.Resdesign {
    public class EventsContentPageViewModel : BaseViewModel {
        #region Fields

        private readonly Func<EventPage, EventPageViewModel> _eventPageViewModelFactory;
        private readonly Func<Language, Location, EventPageLoader> _eventPageLoaderFactory;

        private INavigator _navigator;
        private IEnumerable<EventPageViewModel> _eventPages;
        private Location _lastLoadedLocation; // the last loaded location
        private Language _lastLoadedLanguage; // the last loaded language
        private PersistenceService _persistenceService;
        private Func<PageViewModel, EventsSingleItemDetailViewModel> _singleItemDetailViewModelFactory;

        #endregion

        #region Properties

        public IEnumerable<EventPageViewModel> EventPages {
            get { return _eventPages; }
            set { SetProperty(ref _eventPages, value); }
        }
        
        #endregion

        public EventsContentPageViewModel(IAnalyticsService analytics, INavigator navigator, Func<Language, Location, EventPageLoader> eventPageLoaderFactory, Func<EventPage, 
            EventPageViewModel> eventPageViewModelFactory, PersistenceService persistenceService, Func<PageViewModel, EventsSingleItemDetailViewModel> singleItemDetailViewModelFactory)
        : base(analytics) {
            Title = "Events";
            _navigator = navigator;
            _navigator.HideToolbar(this);
            _eventPageLoaderFactory = eventPageLoaderFactory;
            _eventPageViewModelFactory = eventPageViewModelFactory;
            _singleItemDetailViewModelFactory = singleItemDetailViewModelFactory;

            _persistenceService = persistenceService;
            
            LoadSettings();
        }

        /// <summary>
        /// Called when the user [tap]'s on a item.
        /// </summary>
        /// <param name="pageViewModel">The view model of the clicked page item.</param>
        private async void OnPageTapped(object pageViewModel) {
            var pageVm = pageViewModel as PageViewModel;
            if (pageVm == null) return;
                // target page has no children, display only content
                await _navigator.PushAsync(_singleItemDetailViewModelFactory(pageVm), Navigation);

        }

        /// <summary>
        /// Loads the location and language from the settings and finally loads their models from the persistence service.
        /// </summary>
        private async void LoadSettings() {
            var locationId = Preferences.Location();
            var languageId = Preferences.Language(locationId);
            IsBusy = true;
            _lastLoadedLanguage = await _persistenceService.Get<Language>(languageId);
            _lastLoadedLocation = await _persistenceService.Get<Location>(locationId);
            IsBusy = false;
        }

        protected override void OnMetadataChanged() {
            LoadSettings();
            OnRefresh(true);
        }

        protected override async void OnRefresh(bool force = false) {
            // wait until we're not busy anymore
            await Task.Run(() => {
                while (IsBusy) ;
            });
            LoadEventPages();
        }

        /// <summary>
        /// Loads the event pages for the given location and language.
        /// </summary>
        /// <param name="forceRefresh">if set to <c>true</c> [forces a refresh] from the server.</param>
        /// <param name="forLocation">For this location.</param>
        /// <param name="forLanguage">For this language.</param>
        private async void LoadEventPages(bool forceRefresh = false, Location forLocation = null, Language forLanguage = null) {
            // if location or language is null, use the last used items
            if (forLocation == null) forLocation = _lastLoadedLocation;
            if (forLanguage == null) forLanguage = _lastLoadedLanguage;

            if (IsBusy || forLocation == null || forLanguage == null) {
                Console.WriteLine("LoadPages could not be executed");
                return;
            }

            var pageLoader = _eventPageLoaderFactory(forLanguage, forLocation);
            try {
                IsBusy = true;
                var pages = await pageLoader.Load(forceRefresh);
                
                var eventPages = pages.OrderBy(x => x.Modified).Select(page => _eventPageViewModelFactory(page)).ToList();
                foreach (var eventPageViewModel in eventPages) {
                    eventPageViewModel.OnTapCommand = new Command(OnPageTapped);
                }
                EventPages = eventPages;
            } finally {
                IsBusy = false;
            }
        }
    }
}
