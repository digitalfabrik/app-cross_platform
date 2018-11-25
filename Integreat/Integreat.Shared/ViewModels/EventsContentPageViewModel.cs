using Integreat.Localization;
using Integreat.Shared.Data.Loader;
using Integreat.Shared.Models;
using Integreat.Shared.Services;
using Integreat.Shared.Utilities;
using Integreat.Shared.ViewFactory;
using Integreat.Shared.ViewModels.Events;
using Integreat.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Integreat.Shared.ViewModels
{
    /// <inheritdoc />
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
        private ICommand _changeLanguageCommand;

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

        /// <summary> Gets or sets the change language command. </summary>
        /// <value> The change language command. </value>
        public ICommand ChangeLanguageCommand
        {
            get => _changeLanguageCommand;
            set => SetProperty(ref _changeLanguageCommand, value);
        }

        #endregion

        public EventsContentPageViewModel(INavigator navigator, Func<EventPage,
            EventPageViewModel> eventPageViewModelFactory, DataLoaderProvider dataLoaderProvider,
                                          Func<EventPageViewModel, EventsSingleItemDetailViewModel> singleItemDetailViewModelFactory,
                                          IViewFactory viewFactory)
        : base(dataLoaderProvider)
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

            ChangeLanguageCommand = new Command(OnChangeLanguage);

            // add toolbar items
            ToolbarItems = new List<ToolbarItem>
            {
                new ToolbarItem { Text = AppResources.Language, Icon = "translate", Order = ToolbarItemOrder.Primary, Command = ChangeLanguageCommand },
#if __ANDROID__
                new ToolbarItem { Text = AppResources.Share, Order = ToolbarItemOrder.Secondary, Icon = "share", Command = ContentContainerViewModel.Current.ShareCommand },
                new ToolbarItem { Text = AppResources.Location, Order = ToolbarItemOrder.Secondary, Command = ContentContainerViewModel.Current.OpenLocationSelectionCommand },
                new ToolbarItem { Text = AppResources.Settings, Order = ToolbarItemOrder.Secondary, Command = ContentContainerViewModel.Current.OpenSettingsCommand }
#endif
            };
        }

        /// <summary>
        /// Called when the user [tap]'s on a item.
        /// </summary>
        /// <param name="pageViewModel">The view model of the clicked page item.</param>
        private async void OnPageTapped(object pageViewModel)
        {
            if (!(pageViewModel is EventPageViewModel pageVm)) return;

            _shownPages.Push(pageVm);

            //check if meta tag already exists
            if (pageVm.HasContent && !pageVm.Content.StartsWith(HtmlTags.Doctype.GetStringValue()
                                        + Constants.MetaTagBuilderTag, StringComparison.Ordinal))
            {
                // target page has no children, display only content
                var header =
                    $"<h3>{pageVm.Title}</h3><h4>{AppResources.Date}: {pageVm.EventDate}<br/>{AppResources.Location}: {pageVm.EventLocation}</h4><br>";

                var content = header + pageVm.Content;
                var mb = new MetaTagBuilder(content);
                mb.MetaTags.ToList().AddRange(
                    new List<string>
                    {
                        "<meta name='viewport' content='width=device-width'>",
                        "<meta name='format-detection' content='telephone=no'>"
                    });
                pageVm.EventContent = mb.Build();
            }

            var viewModel = _singleItemDetailViewModelFactory(pageVm); //create new view
            var view = _viewFactory.Resolve(viewModel);
            view.Title = pageVm.Title;
            await Navigation.PushAsync(view);
            viewModel.NavigatedTo();
        }

        private async void OnChangeLanguage(object obj)
        {
            if (IsBusy) return;
            await Task.Run(() => ContentContainerViewModel.Current.OpenLanguageSelection(false));
        }

        /// <inheritdoc />
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
            NoResultText = AppResources.NoEvents;

            try
            {
                IsBusy = true;
                EventPages?.Clear();
                var ePages = await DataLoaderProvider.EventPagesDataLoader.Load(forced, forLanguage, forLocation);

                var eventPages = ePages.OrderBy(x => x.Modified).Select(page => _eventPageViewModelFactory(page)).ToList();

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

                /*
				//get notifications
				var npages = DataLoaderProvider.PushNotificationsDataLoader.Load(forLocation);

				if(npages != null)
				{
					var notificationPages = npages.OrderBy(p => p.Event.StartTime).Select(page => _eventPageViewModelFactory(page)).ToList();

                    notificationPages = (from evt in notificationPages
                                         let evtModel = (evt.Page as EventPage)?.Event
                                         where evtModel != null && new DateTime(evtModel.EndTime) > DateTime.Now
                                         orderby new DateTime(evtModel.StartTime)
                                         select evt).ToList();

                    //merge
                    eventPages.AddRange(notificationPages);
				}
                */

                EventPages = new ObservableCollection<EventPageViewModel>(eventPages);
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
