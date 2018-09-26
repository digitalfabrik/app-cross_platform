using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
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

        private readonly Func<Location, LanguagesViewModel> _languageFactory;
        private readonly IViewFactory _viewFactory;

        private IList<Page> _children; // children pages of this ContentContainer
        private readonly DataLoaderProvider _dataLoaderProvider; // persistence service used to load the saved language details
        private Location _selectedLocation; // the location the user has previously selected (null if first time starting the app)
        private readonly Func<ContentContainerViewModel, SettingsPageViewModel> _settingsFactory; //factory used to open the settings page

        public static ContentContainerViewModel Current { get; private set; } // globally available instance of the contentContainer (to invoke refresh events)

        public ContentContainerViewModel(INavigator navigator,
                                            Func<Location, LanguagesViewModel> languageFactory,
                                            IViewFactory viewFactory,
                                            DataLoaderProvider dataLoaderProvider,
                                            Func<ContentContainerViewModel, SettingsPageViewModel> settingsFactory)
        {
            _navigator = navigator;
            _languageFactory = languageFactory;
            _dataLoaderProvider = dataLoaderProvider;
            _settingsFactory = settingsFactory;
            _viewFactory = viewFactory;

            RegisterCommands();
            LoadLanguage();

            Current = this;
        }

        private void RegisterCommands()
        {
            ShareCommand = new Command(OnShare);
            OpenLocationSelectionCommand = new Command(OpenLocationSelection);
            OpenSettingsCommand = new Command(OpenSettings);
        }

        /// <summary> Gets the share command. </summary>
        /// <value> The share command. </value>
        public ICommand ShareCommand { get; private set; }

        /// <summary>
        /// Gets the open location selection command.
        /// </summary>
        public ICommand OpenLocationSelectionCommand { get; private set; }

        /// <summary>
        /// Gets the open settings command.
        /// </summary>
        public ICommand OpenSettingsCommand { get; private set; }

        private void OnShare(object obj)
        {
            if (IsBusy) return;
            var linkToShare = GetLink();
            Debug.WriteLine(linkToShare, "Info");
            var shareMessage = new ShareMessage { Text = "Hey check this out", Title = "Integreat", Url = linkToShare };
            CrossShare.Current.Share(shareMessage);
        }

        private static string GetLink()
            => Constants.IntegreatReleaseUrl;

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
            => Application.Current.MainPage = new NavigationPage(_viewFactory.Resolve<LocationsViewModel>());

        //Opens the language selection
        public void OpenLanguageSelection()
            => Application.Current.MainPage = _viewFactory.Resolve(_languageFactory(_selectedLocation));

        public async void OpenSettings()
        {
            if ((Application.Current?.MainPage as NavigationPage)?.CurrentPage is SettingsPage) return;
            await _navigator.PushAsync(_settingsFactory(this));
        }

        /// <summary> Creates the main pages of the App. Main, Extras, Events and Settings </summary>
        /// <param name="children">The children.</param>
        public void CreateMainView(IList<Page> children)
        {
            _children = children;

            AddContentPagesToContentContainer();

            RefreshAll();
        }

        private void AddContentPagesToContentContainer()
        {
#if __ANDROID__
            SetupContentForAndroid();
#else
            SetupContentForOtherDevices();
#endif
        }

        private void GetPageAndViewModel(out Page newPage, out MainContentPageViewModel viewModel)
        {
            newPage = _viewFactory.Resolve<MainContentPageViewModel>();
            viewModel = (MainContentPageViewModel)newPage.BindingContext;
            viewModel.ContentContainer = this;
        }

        // ReSharper disable once UnusedMember.Local
#pragma warning disable S1144 // Unused private types or members should be removed
        private void SetupContentForAndroid()
        {
            _children.Add(_viewFactory.Resolve<ExtrasContentPageViewModel>());

            GetPageAndViewModel(out var newPage, out var viewModel);

            ((NavigationPage)Application.Current.MainPage).Popped += viewModel.OnPagePopped;
            _children.Add(newPage);
            _children.Add(_viewFactory.Resolve<EventsContentPageViewModel>());
        }

        private void SetupContentForOtherDevices()
        {
            // add the content pages to the contentContainer
            _children.Add(new MainNavigationPage(_viewFactory.Resolve<ExtrasContentPageViewModel>()));

            GetPageAndViewModel(out var newPage, out var viewModel);

            var navigationPage = new MainNavigationPage(newPage);
            navigationPage.Popped += viewModel.OnPagePopped;

            _children.Add(navigationPage);
            _children.Add(new MainNavigationPage(_viewFactory.Resolve<EventsContentPageViewModel>()));
            _children.Add(new MainNavigationPage(_viewFactory.Resolve<SettingsPageViewModel>()));
        }

#pragma warning restore S1144 // Unused private types or members should be removed
        /// <summary> Refreshes all content pages. </summary>
        /// <param name="metaDataChanged">Whether meta data (that is language and/or location) has changed.</param>
        public async void RefreshAll(bool metaDataChanged = false)
        {
            // wait until control is no longer busy
            await Task.Run(() => { while (IsBusy) { /*empty ignored*/ } });

            if (_children == null) return;

            Title = _selectedLocation?.Name;

            foreach (var child in _children)
            {
                RefreshPage(metaDataChanged, child);
            }
        }

        private static void RefreshPage(bool metaDataChanged, Page child)
        {
#if __ANDROID__
            (child as BaseContentPage)?.Refresh(metaDataChanged);
#else
            (((MainNavigationPage)child).CurrentPage as BaseContentPage)?.Refresh(metaDataChanged);
#endif
        }
    }
}
