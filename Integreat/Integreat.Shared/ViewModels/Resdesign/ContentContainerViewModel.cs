using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Integreat.Shared.ApplicationObjects;
using Integreat.Shared.Models;
using Integreat.Shared.Pages;
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
        private Func<LocationsViewModel> _locationFactory;
        private IViewFactory _viewFactory;

        private LocationsViewModel _locationsViewModel; // view model for when OpenLocationSelection is called
        private IList<Page> _children; // children pages of this ContentContainer
        private PersistenceService _persistenceService; // persistence service used to load the saved language details
        private Location _selectedLocation; // the location the user has previously selected (null if first time starting the app)

        public List<ToolbarItem> ToolbarItems {
            get { return _toolbarItems; }
            set { SetProperty(ref _toolbarItems, value); }
        }


        public ContentContainerViewModel(IAnalyticsService analytics, INavigator navigator, Func<LocationsViewModel> locationFactory,  IViewFactory viewFactory, PersistenceService persistenceService)
        : base (analytics) {
            _navigator = navigator;
            _navigator.HideToolbar(this);
            _locationFactory = locationFactory;
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
            _locationsViewModel = _locationFactory();
            _locationsViewModel.OnLanguageSelectedCommand = new Command<object>(OnLanguageSelected);
             await _navigator.PushModalAsync(_locationsViewModel);
        }

        /// <summary>
        /// Called when [language selected].
        /// </summary>
        /// <param name="languageViewModel">The languageViewModel.</param>
        private async void OnLanguageSelected(object languageViewModel)
        {
            await _navigator.PopModalAsync();

            // set the new selected location
            _selectedLocation = _locationsViewModel?.SelectedLocation;

            // refresh every page (this is for the case, that we changed the language, while the main view is already displayed. Therefore we need to update the pages, since the location or language has most likely changed)
            RefreshAll();
        }

        /// <summary>
        /// Creates the main pages of the App. Main, Extras, Events and Settings
        /// </summary>
        /// <param name="children">The children.</param>
        /// <param name="toolbarItems">The toolbar items.</param>
        public async void CreateMainView(IList<Page> children, IList<ToolbarItem> toolbarItems)
        {
            _children = children;
            var navigationPage = new NavigationPage(_viewFactory.Resolve<MainContentPageViewModel>()) { Title = "Main", BarTextColor = (Color)Application.Current.Resources["textColor"], Icon = "home150.png" };
            navigationPage.ToolbarItems.Add(new ToolbarItem() { Text = "Language", Icon = "globe.png" });
            navigationPage.ToolbarItems.Add(new ToolbarItem() { Text = "Search", Icon = "search.png" });
            children.Add(navigationPage);
            
            navigationPage = new NavigationPage(_viewFactory.Resolve<ExtrasContentPageViewModel>()) { Title = "Extras", BarTextColor = (Color)Application.Current.Resources["textColor"], Icon = "extras100.png" };
            children.Add(navigationPage);


            navigationPage = new NavigationPage(_viewFactory.Resolve<EventsContentPageViewModel>()) { Title = "Events", BarTextColor = (Color)Application.Current.Resources["textColor"], Icon = "calendar159.png" };
            children.Add(navigationPage);

            navigationPage = new NavigationPage(_viewFactory.Resolve<SettingsContentPageViewModel>()) { Title = "Settings", BarTextColor = (Color)Application.Current.Resources["textColor"], Icon = "settings100.png" };
            children.Add(navigationPage);
            
            // refresh every page
            RefreshAll();
        }

        private async void RefreshAll()
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
                var page = navPage?.CurrentPage as BaseContentPage;
                if (page == null) continue;
                page.Title = _selectedLocation?.Name;
                page.Refresh();
            }
        }
    }
}
