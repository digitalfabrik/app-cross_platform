using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Integreat.Shared.ApplicationObjects;
using Integreat.Shared.Data.Loader;
using Integreat.Shared.Models;
using Integreat.Shared.Pages;
using Integreat.Shared.Pages.Redesign;
using Integreat.Shared.Services;
using Integreat.Shared.Services.Tracking;
using Integreat.Shared.Utilities;
using Xamarin.Forms;
using Page = Xamarin.Forms.Page;
using localization;

namespace Integreat.Shared.ViewModels.Resdesign {
    public class ContentContainerViewModel : BaseViewModel {
        private INavigator _navigator;

        private List<ToolbarItem> _toolbarItems;
        private Func<LocationsViewModel> _locationFactory; // Location View Model factory to open a location selection page
        private Func<Location, LanguagesViewModel> _languageFactory; // Language View Model factory to open a language selection page
        private IViewFactory _viewFactory;

        private LocationsViewModel _locationsViewModel; // view model for when OpenLocationSelection is called
        private LanguagesViewModel _languageViewModel; // analog to above

        private IList<Page> _children; // children pages of this ContentContainer
        private DataLoaderProvider _dataLoaderProvider; // persistence service used to load the saved language details
        private Location _selectedLocation; // the location the user has previously selected (null if first time starting the app);

        public event EventHandler LanguageSelected;

        public List<ToolbarItem> ToolbarItems {
            get { return _toolbarItems; }
            set { SetProperty(ref _toolbarItems, value); }
        }


        public ContentContainerViewModel(IAnalyticsService analytics, INavigator navigator, Func<LocationsViewModel> locationFactory, Func<Location, LanguagesViewModel> languageFactory, IViewFactory viewFactory, DataLoaderProvider dataLoaderProvider)
        : base(analytics) {
            _navigator = navigator;
            _locationFactory = locationFactory;
            _languageFactory = languageFactory;
            _dataLoaderProvider = dataLoaderProvider;

            _viewFactory = viewFactory;

            ToolbarItems = new List<ToolbarItem>();
            //_navigator.HideToolbar(this);

            LoadLanguage();
        }


        /// <summary>
        /// Loads the location from the settings and finally loads their models from the persistence service.
        /// </summary>
        private async void LoadLanguage() {
            var locationId = Preferences.Location();
            IsBusy = true;
            _selectedLocation = (await _dataLoaderProvider.LocationsDataLoader.Load(false)).FirstOrDefault(x => x.Id == locationId);
            IsBusy = false;
        }

        /// <summary>
        /// Opens the location selection as modal page and pops them both when the language was selected.
        /// </summary>
        public async void OpenLocationSelection(bool disableBackButton = true) {
            if (_locationsViewModel != null) return; // to avoid opening multiple times

            _locationsViewModel = _locationFactory();
            _locationsViewModel.OnLanguageSelectedCommand = new Command<object>(OnLanguageSelected);
            await _navigator.PushAsync(_locationsViewModel);
            // disable back button
            if(disableBackButton)
                NavigationPage.SetHasBackButton((Application.Current.MainPage as NavigationPage)?.CurrentPage, false);
        }

        /// <summary>
        /// Opens the language selection as modal page and pops them both when the language was selected.
        /// </summary>
        public async void OpenLanguageSelection() {
            if (_languageViewModel != null) return; // to avoid opening multiple times
            _languageViewModel = _languageFactory(_selectedLocation);
            _languageViewModel.OnLanguageSelectedCommand = new Command<object>(OnLanguageSelected);
            await _navigator.PushAsync(_languageViewModel);
        }

        /// <summary>
        /// Called when [language selected].
        /// </summary>
        /// <param name="languageViewModel">The languageViewModel.</param>
        private async void OnLanguageSelected(object languageViewModel) {
            await _navigator.PopToRootAsync();

            if (_locationsViewModel != null) {
                // set the new selected location (if there is a locationsViewModel, if not there was only the language selection opened)
                _selectedLocation = _locationsViewModel.SelectedLocation;
                _locationsViewModel = null;
            }

            _languageViewModel = null;

            LanguageSelected?.Invoke(this, EventArgs.Empty);

            // refresh every page (this is for the case, that we changed the language, while the main view is already displayed. Therefore we need to update the pages, since the location or language has most likely changed)
            RefreshAll(true);
        }

        /// <summary>
        /// Creates the main pages of the App. Main, Extras, Events and Settings
        /// </summary>
        /// <param name="children">The children.</param>
        /// <param name="toolbarItems">The toolbar items.</param>
        /// <param name="navigationPage"></param>
        public void CreateMainView(IList<Page> children, NavigationPage navigationPage) {
            _children = children;

            // add the content pages to the contentContainer
            children.Add(_viewFactory.Resolve<ExtrasContentPageViewModel>());

            var newPage = _viewFactory.Resolve<MainContentPageViewModel>();

            var viewModel = (MainContentPageViewModel)newPage.BindingContext;
            viewModel.ContentContainer = this;
            navigationPage.Popped += viewModel.OnPagePopped;

            navigationPage.ToolbarItems.Add(new ToolbarItem { Text = AppResources.Search, Icon = "search.png", Command = viewModel.OpenSearchCommand });
            navigationPage.ToolbarItems.Add(new ToolbarItem { Text = AppResources.Settings, Order = ToolbarItemOrder.Secondary, Command = new Command<object>(OnOpenSettings) });
            navigationPage.ToolbarItems.Add(new ToolbarItem { Text = AppResources.Language, Order = ToolbarItemOrder.Secondary, Command = viewModel.ChangeLanguageCommand });
            children.Add(newPage);

            children.Add(_viewFactory.Resolve<EventsContentPageViewModel>());

            // Settings page is now opened via the toolbar menu
            /*
            var settingsPage = _viewFactory.Resolve<SettingsContentPageViewModel>() as SettingsContentPage;
            if (settingsPage == null) return;

            // hook the Tap events to the language/location open methods
            settingsPage.OpenLanguageSelectionCommand = new Command(OpenLanguageSelection);
            settingsPage.OpenLocationSelectionCommand = new Command(() => OpenLocationSelection());

            children.Add(settingsPage);*/


            // refresh every page
            RefreshAll();
        }

        private async void OnOpenSettings(object obj) {
            if (IsBusy) return;
            var settingsPage = _viewFactory.Resolve<SettingsContentPageViewModel>() as SettingsContentPage;
            if (settingsPage == null) return;

            // hook the Tap events to the language/location open methods
            settingsPage.OpenLanguageSelectionCommand = new Command(OpenLanguageSelection);
            settingsPage.OpenLocationSelectionCommand = new Command(() => OpenLocationSelection(false));

            // call refresh method
            (settingsPage.BindingContext as BaseContentViewModel)?.RefreshCommand?.Execute(true);

            // push the page onto the Applications root NavigationPage (there's probably a better way to get to the rootPage, but this'll do for now)
            var pushAsync = (Application.Current.MainPage as NavigationPage)?.PushAsync(settingsPage);
            if (pushAsync != null)
                await pushAsync;
        }

        /// <summary>
        /// Refreshes all content pages.
        /// </summary>0
        /// <param name="metaDataChanged">Whether meta data (that is language and/or location) has changed.</param>
        public async void RefreshAll(bool metaDataChanged = false) {
            // wait until control is no longer busy
            await Task.Run(() => {
                while (IsBusy) {
                }
            });

            if (_children == null) return;

            Title = _selectedLocation?.Name;

            foreach (var child in _children) {
                var navPage = child as BaseContentPage;

                navPage?.Refresh(metaDataChanged);

            }
        }
    }
}
