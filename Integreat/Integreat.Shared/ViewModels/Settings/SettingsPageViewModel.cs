using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Integreat.Localization;
using Integreat.Shared.Data.Loader;
using Integreat.Shared.Models;
using Integreat.Shared.Services;
using Integreat.Shared.Utilities;
using Integreat.Utilities;
using Xamarin.Forms;
using MenuItem = Integreat.Shared.Models.MenuItem;

// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace Integreat.Shared.ViewModels
{
    /// <inheritdoc />
    /// <summary>
    /// Class SettingsPageViewModel contains all information and functionality for the Settings page
    /// </summary>
    public class SettingsPageViewModel : BaseContentViewModel
    {
        private string _settingsNotification;
        private string _cacheSizeText;
        private static int _tapCount;
        private readonly INavigator _navigator;
        private readonly Func<string, GeneralWebViewPageViewModel> _generalWebViewFactory;
        private readonly Func<FcmSettingsPageViewModel> _fcmSettingsPageViewModel;
        private string _disclaimerContent; // HTML text for the disclaimer

        public SettingsPageViewModel(INavigator navigator, DataLoaderProvider dataLoaderProvider,
            Func<FcmSettingsPageViewModel> fcmSettingsPageViewModel,
            Func<string, GeneralWebViewPageViewModel> generalWebViewFactory) : base(dataLoaderProvider)
        {
            _navigator = navigator;
            _generalWebViewFactory = generalWebViewFactory;
            _fcmSettingsPageViewModel = fcmSettingsPageViewModel;
            HtmlRawViewCommand = new Command(HtmlRawView);

            Title = AppResources.Settings;
            Icon = Device.RuntimePlatform == Device.Android ? null : "settings100";
            ClearCacheCommand = new Command(async () => await ClearCache());
            ResetSettingsCommand = new Command(ResetSettings);
            OpenDisclaimerCommand = new Command(async () => await OpenDisclaimerPage());
            OpenDataProtectionCommand = new Command(async () => await OpenDataProtectionPage());
            OpenFCMSettingsCommand = new Command(async () => await OpenFCMSettings());
            ChangeLocationCommand = new Command(OpenLocationSelectionPage);
            ToggleNetworkConnection = new Command(async () => await ToggleNetworkConnectionOption());
            Task.Run(async () => { await UpdateCacheSizeText(); });

            ResetTapCounter();
            OnRefresh();

            MenuItems = FillMenuItems();
            ToolbarItems = GetPrimaryToolbarItemsSettingsPage();
        }

        private ObservableCollection<MenuItem> FillMenuItems()
            => new ObservableCollection<MenuItem>
                {
                    new MenuItem
                    {
                        Id = nameof(DisclaimerText),
                        Name = DisclaimerText,
                        Command = OpenDisclaimerCommand
                    },
                    new MenuItem
                    {
                        Id = nameof(DataProtectionText),
                        Name = DataProtectionText,
                        Command = OpenDataProtectionCommand
                    },
#if __IOS__
                        new MenuItem
                        {
                            Id = nameof(ChangeLocationText),
                            Name = ChangeLocationText,
                            Command = ChangeLocationCommand
                        },
#endif
                    new MenuItem
                    {
                        Id = nameof(NetworkConnectionText),
                        Name = NetworkConnectionText,
                        Subtitle = NetworkConectionState,
                        Command = ToggleNetworkConnection
                    },
                    new MenuItem
                    {
                        Id= nameof(ClearCacheText),
                        Name = ClearCacheText,
                        Subtitle = CacheSizeText,
                        Command = ClearCacheCommand
                    },
                    new MenuItem
                    {
                        Id = nameof(ResetSettingsText),
                        Name = ResetSettingsText,
                        Command = ResetSettingsCommand
                    },
                    new MenuItem
                    {
                        Id = nameof(VersionText),
                        Name = VersionText,
                        Subtitle = Version,
                        Command = HtmlRawViewCommand
                    }
                };

        public string DisclaimerText => AppResources.Disclaimer;
        /// <summary>
        /// Gets the FCM Settings text.
        /// </summary>
        public string FCMSettingsText => AppResources.FirebaseName;
        public string DataProtectionText => AppResources.DataProtection;
        public string NetworkConnectionText => AppResources.RefreshOptions;
        public string NetworkConectionState => Preferences.WifiOnly ? AppResources.WifiOnly : AppResources.WifiMobile;

        public string ChangeLocationText => AppResources.ChangeLocation;

        public string ClearCacheText => AppResources.ClearCache;
        public string CacheSizeText
        {
            get => _cacheSizeText;
            private set => SetProperty(ref _cacheSizeText, value);
        }

        public string VersionText => AppResources.Version;
        public string Version => Helpers.Platform.GetVersion();

        public string ResetSettingsText => AppResources.ResetSettings;

        /// <summary>
        /// Gets or sets the settings status text, used to give the user feedback that the settings clearance was successful.
        /// </summary>
        public string SettingsNotification
        {
            // ReSharper disable once UnusedMember.Global
            get => _settingsNotification;
            set => SetProperty(ref _settingsNotification, value);
        }

        public ObservableCollection<MenuItem> MenuItems { get; private set; }

        public ICommand ClearCacheCommand { get; }
        public ICommand ResetSettingsCommand { get; }
        public ICommand HtmlRawViewCommand { get; }
        public ICommand OpenDisclaimerCommand { get; }
        public ICommand OpenDataProtectionCommand { get; }
        public ICommand OpenFCMSettingsCommand { get; }
        public ICommand ChangeLocationCommand { get; }
        public ICommand ToggleNetworkConnection { get; }

        private async Task UpdateCacheSizeText()
        {
            CacheSizeText = $"{AppResources.CacheSize} {AppResources.Calculating}";
            const string pathDelimiter = "/";
            // count files async
            var fileSize = await DirectorySize.CalculateDirectorySizeAsync(Constants.CachedFilePath + pathDelimiter);

            // parse the bytes into an readable string
            string[] sizes = { "B", "KB", "MB", "GB", "TB" };
            var order = 0;
            // while the size is dividable by 1024
            while (fileSize >= 1024 && order < sizes.Length - 1)
            {
                // increment the order
                order++;
                // and divide by 1024
                fileSize = fileSize / 1024;
            }

            // set the CachedSizeText with the updated value
            CacheSizeText = $"{AppResources.CacheSize} {fileSize:0.##} {sizes[order]}";
            UpdateMenuItem(nameof(ClearCacheText), null, CacheSizeText);
        }

        private void ResetSettings()
        {
            Cache.ClearSettings();
            SettingsNotification = AppResources.SettingsReseted;
            ContentContainerViewModel.Current.OpenLocationSelection();
        }

        private async Task OpenDisclaimerPage()
        {
            if (IsBusy || string.IsNullOrWhiteSpace(_disclaimerContent)) return;

            var viewModel = _generalWebViewFactory(_disclaimerContent);
            //trigger load content 
            viewModel?.RefreshCommand.Execute(false);
            await _navigator.PushAsync(viewModel, Navigation);
        }

        /// <summary>
        /// Opens the contacts page.
        /// </summary>
        private async Task OpenDataProtectionPage()
        {
            if (IsBusy) return;

            var viewModel = _generalWebViewFactory(Constants.DataProtectionUrl);
            //trigger load content 
            viewModel?.RefreshCommand.Execute(false);
            await _navigator.PushAsync(viewModel, Navigation);
        }

        private static void OpenLocationSelectionPage() => ContentContainerViewModel.Current.OpenLocationSelection();


        /// <summary>
        /// Opens the FCM Settings.
        /// </summary>
        private async Task OpenFCMSettings()
        {
            var viewModel = _fcmSettingsPageViewModel();
            await _navigator.PushAsync(viewModel, Navigation);
        }

        /// <summary>
        /// Toggles the refresh option from wifi only to wifi + mobile data and vice versa.
        /// </summary>
        private async Task ToggleNetworkConnectionOption()
        {
            Preferences.WifiOnly = !Preferences.WifiOnly;
            // notify the updated text
            await Task.Run(() => { OnPropertyChanged(nameof(NetworkConectionState)); });
            UpdateMenuItem(nameof(NetworkConnectionText), null, NetworkConectionState);
        }

        private async Task ClearCache()
        {
            Cache.ClearCachedResources();
            Cache.ClearCachedContent();
            await UpdateCacheSizeText();
            UpdateMenuItem(nameof(ClearCacheText), null, CacheSizeText);
        }

        /// <summary>
        /// after 10 tabs activate or deactivate the html raw view to display the html tags
        /// </summary>
        private void HtmlRawView()
        {
            IncreaseTapCounter();
            if (_tapCount < 10) return;
            Preferences.SetHtmlRawView(!Preferences.GetHtmlRawViewSetting());

            //pop the page on top after settings page to close the last maybe formatted open Page
            if (Navigation.NavigationStack.Count > 2)
            {
                var pageToPop = Navigation.NavigationStack.ElementAt(Navigation.NavigationStack.Count - 2);
                Navigation.RemovePage(pageToPop);
            }
            SettingsNotification = Preferences.GetHtmlRawViewSetting()
                ? AppResources.HtmlRawViewActivated
                : AppResources.HtmlRawViewDeactivated;
            ResetTapCounter();
        }

        private static void IncreaseTapCounter() => _tapCount++;

        private static void ResetTapCounter() => _tapCount = 0;

        /// <summary>
        /// this is not so nice, maybe someone has a better solution and can change this :)
        /// </summary>
        private void UpdateMenuItem(string itemIdToUpdate, string titleToUpdate = null, string subtitleToUpdate = null)
        {
            if(itemIdToUpdate.IsNullOrEmpty()) throw new ArgumentNullException(nameof(itemIdToUpdate));
            foreach (var menuItem in MenuItems)
            {
                if (menuItem.Id != itemIdToUpdate) continue;
                if (titleToUpdate != null)
                    menuItem.Name = titleToUpdate;
                if (subtitleToUpdate != null)
                    menuItem.Subtitle = subtitleToUpdate;
            }
        }

        public sealed override void OnRefresh(bool force = false)
        {
            base.OnRefresh(force);
        }

        protected override async void LoadContent(bool forced = false, Language forLanguage = null,
            Location forLocation = null)
        {
            // load the disclaimer text
            try
            {
                IsBusy = true;

                var pages = await DataLoaderProvider.DisclaimerDataLoader.Load(true, LastLoadedLanguage,
                    LastLoadedLocation);
                _disclaimerContent = string.Join("<br><br>", pages.Select(x => x.Content));
                if (string.IsNullOrEmpty(_disclaimerContent))
                    _disclaimerContent = AppResources.DisclaimerNotAvailable;
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}