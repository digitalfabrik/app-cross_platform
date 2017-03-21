using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Integreat.Shared.ApplicationObjects;
using Integreat.Shared.Models;
using Integreat.Shared.Pages;
using Integreat.Shared.Pages.Redesign;
using Integreat.Shared.Services;
using Integreat.Shared.Services.Persistence;
using Integreat.Shared.Services.Tracking;
using Integreat.Shared.Utilities;
using Xamarin.Forms;
using Page = Xamarin.Forms.Page;

namespace Integreat.Shared.ViewModels.Resdesign
{
    public class ContentContainerViewModel : BaseViewModel
    {
        private INavigator _navigator;

        private List<ToolbarItem> _toolbarItems;
        private Func<LocationsViewModel> _locationFactory; // Location View Model factory to open a location selection page
        private Func<Location, LanguagesViewModel> _languageFactory; // Language View Model factory to open a language selection page
        private IViewFactory _viewFactory;

        private LocationsViewModel _locationsViewModel; // view model for when OpenLocationSelection is called
        private LanguagesViewModel _languageViewModel; // analog to above

        private IList<Page> _children; // children pages of this ContentContainer
        private PersistenceService _persistenceService; // persistence service used to load the saved language details
        private Location _selectedLocation; // the location the user has previously selected (null if first time starting the app);


        public List<ToolbarItem> ToolbarItems {
            get { return _toolbarItems; }
            set { SetProperty(ref _toolbarItems, value); }
        }


        public ContentContainerViewModel(IAnalyticsService analytics, INavigator navigator, Func<LocationsViewModel> locationFactory, Func<Location, LanguagesViewModel> languageFactory,  IViewFactory viewFactory, PersistenceService persistenceService)
        : base (analytics) {
            _navigator = navigator;
            _navigator.HideToolbar(this);
            _locationFactory = locationFactory;
            _languageFactory = languageFactory;
            _persistenceService = persistenceService;

            _viewFactory = viewFactory;

            ToolbarItems = new List<ToolbarItem>();
            _navigator.HideToolbar(this);

            LoadLanguage();
        }


        /// <summary>
        /// Loads the location from the settings and finally loads their models from the persistence service.
        /// </summary>
        private async void LoadLanguage() {
            var locationId = Preferences.Location();
            IsBusy = true;
            _selectedLocation = await _persistenceService.Get<Location>(locationId);
            IsBusy = false;
        }

        /// <summary>
        /// Opens the location selection as modal page and pops them both when the language was selected.
        /// </summary>
        public async void OpenLocationSelection()
        {
            if (_locationsViewModel != null) return; // to avoid opening multiple times

            _locationsViewModel = _locationFactory();
            _locationsViewModel.OnLanguageSelectedCommand = new Command<object>(OnLanguageSelected);
             await _navigator.PushModalAsync(_locationsViewModel);
        }

        /// <summary>
        /// Opens the language selection as modal page and pops them both when the language was selected.
        /// </summary>
        public async void OpenLanguageSelection()
        {
            if (_languageViewModel != null) return; // to avoid opening multiple times
            _languageViewModel = _languageFactory(_selectedLocation);
            _languageViewModel.OnLanguageSelectedCommand = new Command<object>(OnLanguageSelected);
            await _navigator.PushModalAsync(_languageViewModel);
        }

        /// <summary>
        /// Called when [language selected].
        /// </summary>
        /// <param name="languageViewModel">The languageViewModel.</param>
        private async void OnLanguageSelected(object languageViewModel)
        {
            if (_locationsViewModel != null)
            {
                await _navigator.PopModalAsync();

                // set the new selected location (if there is a locationsViewModel, if not there was only the language selection opened)
                _selectedLocation = _locationsViewModel.SelectedLocation;
                _locationsViewModel = null;
            }

            _languageViewModel = null;

            // refresh every page (this is for the case, that we changed the language, while the main view is already displayed. Therefore we need to update the pages, since the location or language has most likely changed)
            RefreshAll(true);
        }

        /// <summary>
        /// Creates the main pages of the App. Main, Extras, Events and Settings
        /// </summary>
        /// <param name="children">The children.</param>
        /// <param name="toolbarItems">The toolbar items.</param>
        public async void CreateMainView(IList<Page> children, IList<ToolbarItem> toolbarItems)
        {
            _children = children;
            // add the content pages to the contentContainer
            // Note: don't use icons on Android as it's not commonly used on a TabView
            var navigationPage = new NavigationPage(_viewFactory.Resolve<MainContentPageViewModel>()) { Title = "Main", BarTextColor = (Color)Application.Current.Resources["textColor"], Icon = Device.OS == TargetPlatform.Android ? null : "home150" };
            var viewModel = navigationPage.CurrentPage.BindingContext as MainContentPageViewModel;
            viewModel.ContentContainer = this;
            navigationPage.Popped += viewModel.OnPagePopped;
            navigationPage.ToolbarItems.Add(new ToolbarItem() { Text = "Language", Icon = "globe.png", Command = viewModel.ChangeLanguageCommand});
            navigationPage.ToolbarItems.Add(new ToolbarItem() { Text = "Search", Icon = "search.png", Command = viewModel.OpenSearchCommand });
            children.Add(navigationPage);
            
            navigationPage = new NavigationPage(_viewFactory.Resolve<ExtrasContentPageViewModel>()) { Title = "Extras", BarTextColor = (Color)Application.Current.Resources["textColor"], Icon = Device.OS == TargetPlatform.Android ? null : "extras100" };
            children.Add(navigationPage);

            
            navigationPage = new NavigationPage(_viewFactory.Resolve<EventsContentPageViewModel>()) { Title = "Events", BarTextColor = (Color)Application.Current.Resources["textColor"], Icon = Device.OS == TargetPlatform.Android ? null : "calendar159" };
            children.Add(navigationPage);

            var settingsPage = _viewFactory.Resolve<SettingsContentPageViewModel>() as SettingsContentPage;
            if (settingsPage == null) return;

            // hook the Tap events to the language/location open methods
            settingsPage.OpenLanguageSelectionCommand = new Command(OpenLanguageSelection);
            settingsPage.OpenLocationSelectionCommand = new Command(OpenLocationSelection);

            navigationPage = new NavigationPage(settingsPage) { Title = "Settings", BarTextColor = (Color)Application.Current.Resources["textColor"], Icon = Device.OS == TargetPlatform.Android ? null : "settings100" };
            children.Add(navigationPage);
            
            // refresh every page
            RefreshAll();
        }

        /// <summary>
        /// Refreshes all content pages.
        /// </summary>0
        /// <param name="metaDataChanged">Whether meta data (that is language and/or location) has changed.</param>
        public async void RefreshAll(bool metaDataChanged = false)
        {
            // wait until control is no longer busy
            await Task.Run(() =>
            {
                while (IsBusy)
                {
                }
            });

            if (_children == null) return;

            foreach (var child in _children) {
                var navPage = child as NavigationPage;
                if (navPage == null) continue;
                await navPage.PopToRootAsync(false);

                var page = navPage.CurrentPage as BaseContentPage;

                if (page == null) continue;
                page.Title = _selectedLocation?.Name;
                page.Refresh(metaDataChanged);


            }
        }
    }
}
