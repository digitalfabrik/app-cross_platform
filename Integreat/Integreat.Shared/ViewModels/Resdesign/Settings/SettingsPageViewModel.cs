﻿using System;
using System.Linq;
using System.Windows.Input;
using Integreat.Shared.ApplicationObjects;
using Integreat.Shared.Data.Loader;
using Integreat.Shared.Models;
using Integreat.Shared.Services;
using Integreat.Shared.Services.Tracking;
using Integreat.Shared.Utilities;
using Integreat.Shared.ViewModels.Resdesign.General;
using Integreat.Utilities;
using localization;
using Xamarin.Forms;

namespace Integreat.Shared.ViewModels.Resdesign.Settings
{
    /// <inheritdoc />
    /// <summary>
    /// Class SettingsPageViewModel
    /// </summary>
    public class SettingsPageViewModel : BaseContentViewModel
    {
        private string _settingsStatusText;
        private string _cacheSizeText;
        private static int _tapCount;
        private readonly INavigator _navigator;
        private readonly Func<string, GeneralWebViewPageViewModel> _generalWebViewFactory;
        private string _disclaimerContent; // HTML text for the disclaimer

        private readonly ContentContainerViewModel _contentContainer
            ; // content container needed to open location selection after clearing settings

        public SettingsPageViewModel(IAnalyticsService analyticsService, INavigator navigator,
            ContentContainerViewModel contentContainer, DataLoaderProvider dataLoaderProvider
            , Func<string, GeneralWebViewPageViewModel> generalWebViewFactory) : base(analyticsService,
            dataLoaderProvider)
        {
            _navigator = navigator;
            _contentContainer = contentContainer;
            _generalWebViewFactory = generalWebViewFactory;

            Title = AppResources.Settings;

            HtmlRawViewCommand = new Command(HtmlRawView);
            ClearCacheCommand = new Command(ClearCache);
            ResetSettingsCommand = new Command(ResetSettings);
            OpenDisclaimerCommand = new Command(OpenDisclaimer);

            UpdateCacheSizeText();

            _tapCount = 0;
            OnRefresh();
        }

        /// <summary>
        /// Gets the disclaimer text.
        /// </summary>
        public string DisclaimerText => AppResources.Disclaimer;

        /// <summary>
        /// Gets the clear cache text.
        /// </summary>
        public string ClearCacheText => AppResources.ClearCache;

        /// <summary>
        /// Gets the version text.
        /// </summary>
        public string VersionText => AppResources.Version;

        /// <summary>
        /// Get the current Version
        /// </summary>
        public string Version
        {
            get
            {
                // ReSharper disable once RedundantAssignment
                var version = "2.1.2";
#if __ANDROID__
                 var context = Forms.Context;
                 version = context.PackageManager.GetPackageInfo(context.PackageName, 0).VersionName;          
#elif __IOS__
                version = Foundation.NSBundle.MainBundle.InfoDictionary[new Foundation.NSString("CFBundleVersion")]
                    .ToString();
#else
                version = "2.1.2";
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
            get => _settingsStatusText;
            set => SetProperty(ref _settingsStatusText, value);
        }

        public ICommand ClearCacheCommand { get; }
        public ICommand ResetSettingsCommand { get; }
        public ICommand HtmlRawViewCommand { get; }
        public ICommand OpenDisclaimerCommand { get; }

        private async void UpdateCacheSizeText()
        {
            CacheSizeText = $"{AppResources.CacheSize} {AppResources.Calculating}";

            // count files async
            var fileSize = await DirectorySize.CalculateDirectorySizeAsync(Constants.CachedFilePath + "/");

            // parse the bytes into an readable string
            string[] sizes = {"B", "KB", "MB", "GB", "TB"};
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

            _contentContainer.OpenLocationSelection();
        }

        /// <summary>
        /// Opens the contacts page.
        /// </summary>
        private async void OpenDisclaimer()
        {
            if (IsBusy || string.IsNullOrWhiteSpace(_disclaimerContent)) return;

            var viewModel = _generalWebViewFactory(_disclaimerContent);
            //trigger load content 
            viewModel?.RefreshCommand.Execute(false);
            await _navigator.PushAsync(viewModel, Navigation);
        }

        /// <summary>
        /// Clears the cache.
        /// </summary>
        private void ClearCache()
        {
            Cache.ClearCachedResources();
            Cache.ClearCachedContent();
            UpdateCacheSizeText();
        }

        /// <summary>
        /// after 10 tabs activate or deactivate the html raw view to display the html tags
        /// </summary>
        private void HtmlRawView()
        {
            _tapCount++;
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
            _tapCount = 0;
        }

        protected override async void LoadContent(bool forced = false, Language forLanguage = null,
            Location forLocation = null)
        {
            // load the disclaimer text
            try
            {
                IsBusy = true;

                var pages = await _dataLoaderProvider.DisclaimerDataLoader.Load(true, LastLoadedLanguage,
                    LastLoadedLocation);
                _disclaimerContent = string.Join("<br><br>", pages.Select(x => x.Content));
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}