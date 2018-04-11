using System;
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
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace Integreat.Shared.ViewModels
{
    /// <summary>
    /// Class SettingsPageViewModel contains all information and functionality for the Settings page
    /// </summary>
    public class SettingsPageViewModel : BaseContentViewModel
    {
        private string _settingsStatusText;
        private string _cacheSizeText;
        private static int _tapCount;
        private readonly INavigator _navigator;
        private readonly Func<string, GeneralWebViewPageViewModel> _generalWebViewFactory;
        private string _disclaimerContent; // HTML text for the disclaimer

        public SettingsPageViewModel(INavigator navigator,
            ContentContainerViewModel contentContainer,
            DataLoaderProvider dataLoaderProvider,
            Func<string, GeneralWebViewPageViewModel> generalWebViewFactory) : base(dataLoaderProvider)
        {
            _navigator = navigator;
            _generalWebViewFactory = generalWebViewFactory;
            HtmlRawViewCommand = new Command(HtmlRawView);

            Title = AppResources.Settings;
            Icon = Device.RuntimePlatform == Device.Android ? null : "settings100";
            ClearCacheCommand = new Command(async () => await ClearCache());
            ResetSettingsCommand = new Command(ResetSettings);
            OpenDisclaimerCommand = new Command(async () => await OpenDisclaimer());
            ChangeLocationCommand = new Command(OnChangeLocation);
            SwitchRefreshOptionCommand = new Command(async () => await SwitchRefreshOption());
            Task.Run(async () => { await UpdateCacheSizeText(); });

            ResetTapCounter();
            OnRefresh();
        }

        public sealed override void OnRefresh(bool force = false)
        {
            base.OnRefresh(force);
        }

        /// <summary>
        /// Gets the disclaimer text.
        /// </summary>
        public string DisclaimerText => AppResources.Disclaimer;

        /// <summary>
        /// Gets the location text.
        /// </summary>
        public string LocationText => AppResources.ChangeLocation;

        /// <summary>
        /// Gets the clear cache text.
        /// </summary>
        public string ClearCacheText => AppResources.ClearCache;

        /// <summary>
        /// Gets the version text.
        /// </summary>
        public string VersionText => AppResources.Version;
        /// <summary>
        /// Gets the refresh text.
        /// </summary>
        public string RefreshText => AppResources.RefreshOptions;

        /// <summary>
        /// Gets the refresh option state text.
        /// </summary>
        public string RefreshState => Preferences.WifiOnly ? AppResources.WifiOnly : AppResources.WifiMobile;

        /// <summary>
        /// Get the current Version
        /// </summary>
        public string Version
        {
            get
            {
                // ReSharper disable once RedundantAssignment
#if __ANDROID__
                var context = Android.App.Application.Context;
                var version = context.PackageManager.GetPackageInfo(context.PackageName, 0).VersionName;
#elif __IOS__
                var version = Foundation.NSBundle.MainBundle.InfoDictionary[new Foundation.NSString("CFBundleVersion")]
                    .ToString();
#else
                version = "2.2.4";
#endif
                return version;
            }
        }

        /// <summary>
        /// Gets the cache size text.
        /// </summary>
        public string CacheSizeText
        {
            get => _cacheSizeText;
            private set => SetProperty(ref _cacheSizeText, value);
        }

        /// <summary>
        /// Gets the reset settings text.
        /// </summary>
        public string ResetSettingsText => AppResources.ResetSettings;

        /// <summary>
        /// Gets or sets the settings status text, used to give the user feedback that the settings clearance was successful.
        /// </summary>
        public string SettingsStatusText
        {
            // ReSharper disable once UnusedMember.Global
            get => _settingsStatusText;
            set => SetProperty(ref _settingsStatusText, value);
        }

        public ICommand ClearCacheCommand { get; }
        public ICommand ResetSettingsCommand { get; }
        public ICommand HtmlRawViewCommand { get; }
        public ICommand OpenDisclaimerCommand { get; }
        public ICommand ChangeLocationCommand { get; }
        public ICommand SwitchRefreshOptionCommand { get; }

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
        }

        /// <summary>
        /// Resets the settings.
        /// </summary>
        private void ResetSettings()
        {
            Cache.ClearSettings();
            SettingsStatusText = AppResources.SettingsReseted;
            ContentContainerViewModel.Current.OpenLocationSelection();
        }

        /// <summary>
        /// Opens the contacts page.
        /// </summary>
        private async Task OpenDisclaimer()
        {
            if (IsBusy || string.IsNullOrWhiteSpace(_disclaimerContent)) return;

            var viewModel = _generalWebViewFactory(_disclaimerContent);
            //trigger load content 
            viewModel?.RefreshCommand.Execute(false);
            await _navigator.PushAsync(viewModel, Navigation);
        }

        /// <summary>
        /// Opens the location page
        /// </summary>
        /// <param name="obj">Object.</param>
        private void OnChangeLocation()
        {
            ContentContainerViewModel.Current.OpenLocationSelection();
        }

        /// <summary>
        /// Toggles the refresh option from wifi only to wifi + mobile data and vice versa.
        /// </summary>
        private async Task SwitchRefreshOption()
        {
            Preferences.WifiOnly = !Preferences.WifiOnly;
            // notify the updated text
            await Task.Run(() => { OnPropertyChanged(nameof(RefreshState)); });
        }

        /// <summary>
        /// Clears the cache.
        /// </summary>
        private async Task ClearCache()
        {
            Cache.ClearCachedResources();
            Cache.ClearCachedContent();
            await UpdateCacheSizeText();
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
            SettingsStatusText = Preferences.GetHtmlRawViewSetting()
                ? AppResources.HtmlRawViewActivated
                : AppResources.HtmlRawViewDeactivated;
            ResetTapCounter();
        }

        private static void IncreaseTapCounter()
        {
            _tapCount++;
        }

        private static void ResetTapCounter()
        {
            _tapCount = 0;
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