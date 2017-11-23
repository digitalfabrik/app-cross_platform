using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using Integreat.Shared.ApplicationObjects;
using Integreat.Shared.Data.Loader;
using Integreat.Shared.Models;
using Integreat.Shared.Services;
using Integreat.Shared.Services.Tracking;
using Integreat.Shared.Utilities;
using Integreat.Shared.ViewModels.Events;
using Integreat.Utilities;
using localization;
using Xamarin.Forms;

namespace Integreat.Shared.ViewModels
{
    /// <summary>
    /// Class EventsContentPageViewModel holds all information and functionality about Event views
    /// </summary>
    public class EventsContentPageViewModel : BaseContentViewModel
    {
        #region Fields

        private readonly Func<EventPage, EventPageViewModel> _eventPageViewModelFactory;

        private ObservableCollection<EventPageViewModel> _eventPages = new ObservableCollection<EventPageViewModel>();
        private readonly Func<EventPageViewModel, EventsSingleItemDetailViewModel> _singleItemDetailViewModelFactory;
        private string _noResultText;
        private readonly Stack<EventPageViewModel> _shownPages;
        private readonly IViewFactory _viewFactory;

        #endregion

        #region Properties

        public ObservableCollection<EventPageViewModel> EventPages
        {
            get => _eventPages;
            set
            {
                SetProperty(ref _eventPages, value);
                OnPropertyChanged(nameof(HasNoResults));
            }
        }

        public bool HasNoResults => EventPages?.Count == 0;

        public string NoResultText
        {
            get => _noResultText;
            set => SetProperty(ref _noResultText, value);
        }

        #endregion

        public EventsContentPageViewModel(IAnalyticsService analytics, INavigator navigator, Func<EventPage,
            EventPageViewModel> eventPageViewModelFactory, DataLoaderProvider dataLoaderProvider,
            Func<EventPageViewModel, EventsSingleItemDetailViewModel> singleItemDetailViewModelFactory, IViewFactory viewFactory)
        : base(analytics, dataLoaderProvider)
        {
            Title = AppResources.News;
            NoResultText = AppResources.NoEvents;
            Icon = Device.RuntimePlatform == Device.Android ? null : "calendar159";
            navigator.HideToolbar(this);
            _eventPageViewModelFactory = eventPageViewModelFactory;
            _singleItemDetailViewModelFactory = singleItemDetailViewModelFactory;
            _viewFactory = viewFactory;

            _shownPages = new Stack<EventPageViewModel>();
            EventPages = new ObservableCollection<EventPageViewModel>();
        }

        /// <summary>
        /// Called when the user [tap]'s on a item.
        /// </summary>
        /// <param name="pageViewModel">The view model of the clicked page item.</param>
        private async void OnPageTapped(object pageViewModel)
        {
            if (!(pageViewModel is EventPageViewModel pageVm)) return;

            _shownPages.Push(pageVm);

            //check if metatag already exists
            if (pageVm.HasContent && !pageVm.Content.StartsWith(HtmlTags.Doctype.GetStringValue() 
                                        + Constants.MetaTagBuilderTag, StringComparison.Ordinal))
            {
                // target page has no children, display only content
                var header = "<h3>" + pageVm.Title + "</h3>" + "<h4>" + AppResources.Date + ": " +
                             pageVm.EventDate + "<br/>" + AppResources.Location + ": " + pageVm.EventLocation + "</h4><br>";

                var content = header + pageVm.Content;
                var mb = new MetaTagBuilder(content);
                mb.MetaTags.Add("<meta name='viewport' content='width=device-width'>");
                mb.MetaTags.Add("<meta name='format-detection' content='telephone=no'>");
                pageVm.EventContent = mb.Build();
            }

            var viewModel = _singleItemDetailViewModelFactory(pageVm); //create new view
            var view = _viewFactory.Resolve(viewModel);
            view.Title = pageVm.Title;
            await Navigation.PushAsync(view);
            viewModel.NavigatedTo();
        }

        /// <summary>
        /// Loads the event pages for the given location and language.
        /// </summary>
        protected override async void LoadContent(bool forced = false, Language forLanguage = null, Location forLocation = null)
        {
            // if location or language is null, use the last used items
            if (forLocation == null) forLocation = LastLoadedLocation;
            if (forLanguage == null) forLanguage = LastLoadedLanguage;

            if (IsBusy || forLocation == null || forLanguage == null)
            {
                Debug.WriteLine("LoadPages could not be executed");
                return;
            }

            // set result text depending whether push notifications are available or not
            NoResultText = forLocation.PushEnabled == "1" ? AppResources.NoPushNotifications : AppResources.NoEvents;

            try
            {
                IsBusy = true;
                EventPages?.Clear();
                var pages = await DataLoaderProvider.EventPagesDataLoader.Load(forced, forLanguage, forLocation);

                var eventPages = pages.OrderBy(x => x.Modified).Select(page => _eventPageViewModelFactory(page)).ToList();

                // select only events which end times after now
                eventPages = (from evt in eventPages
                              let evtModel = (evt.Page as EventPage)?.Event
                              where evtModel != null && new DateTime(evtModel.EndTime) > DateTime.Now
                              orderby new DateTime(evtModel.StartTime)
                              select evt).ToList();


                foreach (var eventPageViewModel in eventPages)
                {
                    eventPageViewModel.OnTapCommand = new Command(OnPageTapped);
                }
                EventPages = new ObservableCollection<EventPageViewModel>(eventPages);
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
