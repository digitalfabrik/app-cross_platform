using System.Windows.Input;
using Integreat.Shared.Services.Tracking;
using Integreat.Shared.Utilities;
using Integreat.Utilities;
using localization;
using Xamarin.Forms;

namespace Integreat.Shared.ViewModels.Resdesign.Settings
{
    public class SettingsPageViewModel : BaseViewModel
    {
        private string _settingsStatusText;
        private string _cacheSizeText;

        /// <summary>
        /// Gets the disclaimer text.
        /// </summary>
        public string DisclaimerText => AppResources.Disclaimer;

        /// <summary>
        /// Gets the clear cache text.
        /// </summary>
        public string ClearCacheText => AppResources.ClearCache;

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
            get { return _settingsStatusText; }
            set { SetProperty(ref _settingsStatusText, value); }
        }

        public ICommand ClearCacheCommand { get; set; }
        public ICommand ResetSettingsCommand { get; set; }
        public ICommand OpenDisclaimerCommand { get; set; }

        public SettingsPageViewModel(IAnalyticsService analyticsService) : base(analyticsService)
        {
            Title = AppResources.Settings;
            ClearCacheCommand = new Command(ClearCache);
            ResetSettingsCommand = new Command(ResetSettings);
            OpenDisclaimerCommand = new Command(OpenDisclaimer);
            UpdateCacheSizeText();
        }

        private async void UpdateCacheSizeText()
        {
            CacheSizeText = $"{AppResources.CacheSize} {AppResources.Calculating}";

            // count files async
            var fileSize = await DirectorySize.CalculateDirectorySizeAsync(Constants.CachedFilePath+"/");

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
        }

        /// <summary>
        /// Opens the contacts page.
        /// </summary>
        private async void OpenDisclaimer()
        {
            throw new System.NotImplementedException();
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
    }
}
