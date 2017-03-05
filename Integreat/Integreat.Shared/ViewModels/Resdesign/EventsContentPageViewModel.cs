using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
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
    public class EventsContentPageViewModel : BaseContentViewModel {
        #region Fields

        private readonly Func<EventPage, EventPageViewModel> _eventPageViewModelFactory;
        private readonly Func<Language, Location, EventPageLoader> _eventPageLoaderFactory;

        private INavigator _navigator;
        private ObservableCollection<EventPageViewModel> _eventPages;
        private Func<PageViewModel, EventsSingleItemDetailViewModel> _singleItemDetailViewModelFactory;

        #endregion

        #region Properties

        public ObservableCollection<EventPageViewModel> EventPages {
            get { return _eventPages; }
            set
            {
                SetProperty(ref _eventPages, value); 
                OnPropertyChanged(nameof(HasNoResults));
            }
        }

        public bool HasNoResults => EventPages?.Count == 0;

        #endregion

        public EventsContentPageViewModel(IAnalyticsService analytics, INavigator navigator, Func<Language, Location, EventPageLoader> eventPageLoaderFactory, Func<EventPage,
            EventPageViewModel> eventPageViewModelFactory, PersistenceService persistenceService, Func<PageViewModel, EventsSingleItemDetailViewModel> singleItemDetailViewModelFactory)
        : base(analytics, persistenceService) {
            Title = "Events";
            _navigator = navigator;
            _navigator.HideToolbar(this);
            _eventPageLoaderFactory = eventPageLoaderFactory;
            _eventPageViewModelFactory = eventPageViewModelFactory;
            _singleItemDetailViewModelFactory = singleItemDetailViewModelFactory;
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
        /// Loads the event pages for the given location and language.
        /// </summary>
        public override async void LoadContent(bool forced = false, Language forLanguage = null, Location forLocation = null) {
            // if location or language is null, use the last used items
            if (forLocation == null) forLocation = LastLoadedLocation;
            if (forLanguage == null) forLanguage = LastLoadedLanguage;

            if (IsBusy || forLocation == null || forLanguage == null) {
                Console.WriteLine("LoadPages could not be executed");
                return;
            }

            var pageLoader = _eventPageLoaderFactory(forLanguage, forLocation);
            try {
                IsBusy = true;
                EventPages?.Clear();
                var pages = await pageLoader.Load(forced);

                var eventPages = pages.OrderBy(x => x.Modified).Select(page => _eventPageViewModelFactory(page)).ToList();

                // select only events which end times after now
                eventPages = (from evt in eventPages
                              let evtModel = (evt.Page as EventPage)?.Event
                              where evtModel != null && new DateTime(evtModel.EndTime) > DateTime.Now
                              select evt).ToList();


                foreach (var eventPageViewModel in eventPages) {
                    eventPageViewModel.OnTapCommand = new Command(OnPageTapped);
                }
                EventPages = new ObservableCollection<EventPageViewModel>(eventPages);
            } finally {
                IsBusy = false;
            }
        }
    }
}
