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

        private IList<Page> _children; // children pages of this ContentContainer
        private readonly DataLoaderProvider _dataLoaderProvider; // persistence service used to load the saved language details
        private Location _selectedLocation; // the location the user has previously selected (null if first time starting the app)

        public static ContentContainerViewModel Current { get; private set; } // globally available instance of the contentContainer (to invoke refresh events)

        public ContentContainerViewModel(INavigator navigator
                    , Func<LocationsViewModel> locationFactory, Func<Location, LanguagesViewModel> languageFactory
                    , IViewFactory viewFactory, DataLoaderProvider dataLoaderProvider)
        {
            _navigator = navigator;
            _locationFactory = locationFactory;
            _languageFactory = languageFactory;
            _dataLoaderProvider = dataLoaderProvider;

            _viewFactory = viewFactory;

            ShareCommand = new Command(OnShare);
            OpenLocationSelectionCommand = new Command(OpenLocationSelection);
            OpenSettingsCommand = new Command(OpenSettings);

            LoadLanguage();
            Current = this;
        }

        /// <summary> Gets the share command. </summary>
        /// <value> The share command. </value>
        public ICommand ShareCommand { get; }

        /// <summary>
        /// Gets the open location selection command.
        /// </summary>
        public ICommand OpenLocationSelectionCommand { get; }

        /// <summary>
        /// Gets the open settings command.
        /// </summary>
        public ICommand OpenSettingsCommand { get; }

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


        /// Opens the location selection as modal page.
        public void OpenLocationSelection()
        {
            Application.Current.MainPage = new NavigationPage(_viewFactory.Resolve<LocationsViewModel>());
        }

        //Opens the language selection
        public void OpenLanguageSelection()
        {
            var languageViewModel = _languageFactory(_selectedLocation);
            Application.Current.MainPage = _viewFactory.Resolve<LanguagesViewModel>(languageViewModel);
        }

        //Opens the settings page
        public async void OpenSettings()
        {
            return;
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
#if __IOS__
            children.Add(new MainNavigationPage(_viewFactory.Resolve<SettingsPageViewModel>()));

#endif
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
