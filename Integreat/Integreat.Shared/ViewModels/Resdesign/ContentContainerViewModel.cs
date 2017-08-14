using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Integreat.Shared.ApplicationObjects;
using Integreat.Shared.Data.Loader;
using Integreat.Shared.Models;
using Integreat.Shared.Pages;
using Integreat.Shared.Pages.Redesign.Settings;
using Integreat.Shared.Services;
using Integreat.Shared.Services.Tracking;
using Integreat.Shared.Utilities;
using Integreat.Shared.ViewModels.Resdesign.Settings;
using Xamarin.Forms;
using Page = Xamarin.Forms.Page;
using localization;

namespace Integreat.Shared.ViewModels.Resdesign
{
    public class ContentContainerViewModel : BaseViewModel
    {
        private readonly INavigator _navigator;
        
        private readonly Func<LocationsViewModel> _locationFactory; // Location View Model factory to open a location selection page
        private readonly Func<Location, LanguagesViewModel> _languageFactory; // Language View Model factory to open a language selection page
      
        private readonly IViewFactory _viewFactory;

        private LocationsViewModel _locationsViewModel; // view model for when OpenLocationSelection is called
        private LanguagesViewModel _languageViewModel; // analog to above

        private IList<Page> _children; // children pages of this ContentContainer
        private readonly DataLoaderProvider _dataLoaderProvider; // persistence service used to load the saved language details
        private Location _selectedLocation; // the location the user has previously selected (null if first time starting the app);
        private readonly Func<ContentContainerViewModel, SettingsPageViewModel> _settingsFactory; // factory used to open the settings page

        public static ContentContainerViewModel Current { get; private set; } // globally available instance of the contentContainer (to invoke refresh events)

        public event EventHandler LanguageSelected;


        public ContentContainerViewModel(IAnalyticsService analytics, INavigator navigator
                    , Func<LocationsViewModel> locationFactory, Func<Location, LanguagesViewModel> languageFactory
                    , IViewFactory viewFactory, DataLoaderProvider dataLoaderProvider, Func<ContentContainerViewModel, SettingsPageViewModel> settingsFactory)
        : base(analytics)
        {
            _navigator = navigator;
            _locationFactory = locationFactory;
            _languageFactory = languageFactory;
            _dataLoaderProvider = dataLoaderProvider;
            _settingsFactory = settingsFactory;

            _viewFactory = viewFactory;

            LoadLanguage();
            Current = this;
        }

        // Loads the location from the settings and finally loads their models from the persistence service.
        private async void LoadLanguage()
        {
            var locationId = Preferences.Location();
            IsBusy = true;
            _selectedLocation = (await _dataLoaderProvider.LocationsDataLoader.Load(false)).FirstOrDefault(x => x.Id == locationId);
            IsBusy = false;
        }


        // Opens the language selection as modal page and pops them both when the language was selected.
        public async void OpenLanguageSelection()
        {
            _languageViewModel = _languageFactory(_selectedLocation);
            _languageViewModel.OnLanguageSelectedCommand = new Command<object>(OnLanguageSelected);
            await _navigator.PushAsync(_languageViewModel);
        }

        /// <summary>
        /// Called when [language selected].
        /// </summary>
        /// <param name="languageViewModel">The languageViewModel.</param>
        private async void OnLanguageSelected(object languageViewModel)
        {
            await _navigator.PopToRootAsync();

            if (_locationsViewModel != null)
            {
                // set the new selected location (if there is a locationsViewModel, if not there was only the language selection opened)
                _selectedLocation = _locationsViewModel.SelectedLocation;
                _locationsViewModel = null;
            }

            LanguageSelected?.Invoke(this, EventArgs.Empty);

            // clear the cache after changing the language (or location in that matter)
            Cache.ClearCachedContent();
            Cache.ClearCachedResources();

            // refresh every page (this is for the case, that we changed the language, while the main view is already displayed. Therefore we need to update the pages, since the location or language has most likely changed)
            RefreshAll(true);
        }

        /// Opens the location selection as modal page and pops them both when the language was selected.
        public async void OpenLocationSelection(bool disableBackButton = true)
        {
            _locationsViewModel = _locationFactory();
            _locationsViewModel.OnLanguageSelectedCommand = new Command<object>(OnLanguageSelected);
            await _navigator.PushAsync(_locationsViewModel);
            // disable back button
            if (disableBackButton)
                NavigationPage.SetHasBackButton((Application.Current.MainPage as NavigationPage)?.CurrentPage, false);
        }

        /// <summary>
        /// Creates the main pages of the App. Main, Extras, Events and Settings
        /// </summary>
        /// <param name="children">The children.</param>
        /// <param name="navigationPage"></param>
        public void CreateMainView(IList<Page> children, NavigationPage navigationPage)
        {
            _children = children;

            // add the content pages to the contentContainer
            children.Add(_viewFactory.Resolve<ExtrasContentPageViewModel>());

            var newPage = _viewFactory.Resolve<MainContentPageViewModel>();

            var viewModel = (MainContentPageViewModel)newPage.BindingContext;
            viewModel.ContentContainer = this;
            navigationPage.Popped += viewModel.OnPagePopped;

            navigationPage.ToolbarItems.Add(new ToolbarItem { Text = AppResources.Search, Icon = "search.png", Command = viewModel.OpenSearchCommand });
            navigationPage.ToolbarItems.Add(new ToolbarItem { Text = AppResources.Language, Order = ToolbarItemOrder.Secondary, Command = viewModel.ChangeLanguageCommand });
            navigationPage.ToolbarItems.Add(new ToolbarItem { Text = AppResources.Location, Order = ToolbarItemOrder.Secondary, Command = viewModel.ChangeLocationCommand });
            navigationPage.ToolbarItems.Add(new ToolbarItem { Text = AppResources.Settings, Order = ToolbarItemOrder.Secondary, Command = new Command(OpenSettings) });

            children.Add(newPage);

            children.Add(_viewFactory.Resolve<EventsContentPageViewModel>());
            // refresh every page
            RefreshAll();
        }

        /// <summary>
        /// Opens a new SettingsPage popped unto the Application root navigation stack
        /// </summary>
        private async void OpenSettings()
        {
            // only allow the opening of the settings once by checking 
            if ((Application.Current?.MainPage as NavigationPage)?.CurrentPage is SettingsPage) return;
            await _navigator.PushAsync(_settingsFactory(this));
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

            Title = _selectedLocation?.Name;

            foreach (var child in _children)
            {
                var navPage = child as BaseContentPage;

                navPage?.Refresh(metaDataChanged);

            }
        }
    }
}
