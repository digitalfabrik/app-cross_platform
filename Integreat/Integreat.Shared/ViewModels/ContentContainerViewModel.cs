using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Integreat.Localization;
using Integreat.Shared.ViewFactory;
using Integreat.Shared.Data.Loader;
using Integreat.Shared.Models;
using Integreat.Shared.Pages;
using Integreat.Shared.Pages.Settings;
using Integreat.Shared.Services;
using Integreat.Shared.Utilities;
using Integreat.Utilities;
using Xamarin.Forms;
using Page = Xamarin.Forms.Page;
using Plugin.Share;
using Plugin.Share.Abstractions;

namespace Integreat.Shared.ViewModels
{
    /// <summary>
    /// Class ContentContainerViewModel
    /// </summary>
    public class ContentContainerViewModel : BaseViewModel
    {
        private readonly INavigator _navigator;

        private readonly Func<LocationsViewModel> _locationFactory; // Location View Model factory to open a location selection page
        private readonly Func<Location, LanguagesViewModel> _languageFactory; // Language View Model factory to open a language selection page     
        private readonly IViewFactory _viewFactory;

        private LocationsViewModel _locationsViewModel; // view model for when OpenLocationSelection is called

        private IList<Page> _children; // children pages of this ContentContainer
        private readonly DataLoaderProvider _dataLoaderProvider; // persistence service used to load the saved language details
        private Location _selectedLocation; // the location the user has previously selected (null if first time starting the app)
        private readonly Func<ContentContainerViewModel, SettingsPageViewModel> _settingsFactory; // factory used to open the settings page

        public static ContentContainerViewModel Current { get; private set; } // globally available instance of the contentContainer (to invoke refresh events)

        public event EventHandler LanguageSelected;

        public ContentContainerViewModel(INavigator navigator
                    , Func<LocationsViewModel> locationFactory, Func<Location, LanguagesViewModel> languageFactory
                    , IViewFactory viewFactory, DataLoaderProvider dataLoaderProvider, Func<ContentContainerViewModel, SettingsPageViewModel> settingsFactory)
        {
            _navigator = navigator;
            _locationFactory = locationFactory;
            _languageFactory = languageFactory;
            _dataLoaderProvider = dataLoaderProvider;
            _settingsFactory = settingsFactory;

            _viewFactory = viewFactory;

            ShareCommand = new Command(OnShare);

            LoadLanguage();
            Current = this;
        }

        /// <summary> Gets the share command. </summary>
        /// <value> The share command. </value>
        public ICommand ShareCommand { get; }

        private void OnShare(object obj)
        {
            if (IsBusy) return;
            var linkToShare = GetLink();
            Debug.WriteLine(linkToShare, "Info");
            var shareMessage = new ShareMessage { Text = "Hey check this out", Title = "Integreat", Url = linkToShare };
            CrossShare.Current.Share(shareMessage);
        }

        private string GetLink()
        {
            return Constants.IntegreatReleaseUrl;
        }

        // Loads the location from the settings and finally loads their models from the persistence service.
        private async void LoadLanguage()
        {
            var locationId = Preferences.Location();
            IsBusy = true;
            _selectedLocation = (await _dataLoaderProvider.LocationsDataLoader.Load(false)).FirstOrDefault(x => x.Id == locationId);
            IsBusy = false;
        }
       

        /// Opens the location selection as modal page and pops them both when the language was selected.
        public async void OpenLocationSelection(bool disableBackButton = true)
        {
            Application.Current.MainPage = new NavigationPage(_viewFactory.Resolve<LocationsViewModel>());
        }

        /// <summary> Creates the main pages of the App. Main, Extras, Events and Settings </summary>
        /// <param name="children">The children.</param>
        public void CreateMainView(IList<Page> children)
        {
            _children = children;

            // add the content pages to the contentContainer
            children.Add(new MainNavigationPage(_viewFactory.Resolve<ExtrasContentPageViewModel>()));

            var newPage = _viewFactory.Resolve<MainContentPageViewModel>();

            var viewModel = (MainContentPageViewModel)newPage.BindingContext;
            viewModel.ContentContainer = this;

            children.Add(new MainNavigationPage(newPage));

            children.Add(new MainNavigationPage(_viewFactory.Resolve<EventsContentPageViewModel>()));

            children.Add(new MainNavigationPage(_viewFactory.Resolve<SettingsPageViewModel>()));
            // refresh every page
            RefreshAll();
        }

        /// <summary> Refreshes all content pages. </summary>
        /// <param name="metaDataChanged">Whether meta data (that is language and/or location) has changed.</param>
        public async void RefreshAll(bool metaDataChanged = false)
        {
            // wait until control is no longer busy
            await Task.Run(() =>
            {
                while (IsBusy)
                {
                    //empty ignored 
                }
            });

            if (_children == null) return;

            Title = _selectedLocation?.Name;

            foreach (var child in _children)
            {
                var childPage = ((MainNavigationPage)child).CurrentPage as BaseContentPage;
                childPage?.Refresh(metaDataChanged);
            }
        }
    }
}
