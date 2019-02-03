using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Integreat.Localization;
using Integreat.Shared.Data.Loader;
using Integreat.Shared.Models;
using Integreat.Shared.Utilities;
using Xamarin.Forms;

namespace Integreat.Shared.ViewModels
{
    /// <inheritdoc />
    /// <summary>
    /// Provides a base class for big content pages. Features methods to load/store/reload the selected location and language.
    /// </summary>
    public abstract class BaseContentViewModel : BaseViewModel
    {
        protected readonly DataLoaderProvider DataLoaderProvider;
        private Location _lastLoadedLocation;
        private Language _lastLoadedLanguage;
        private string _errorMessage;
        private bool _showHeadline;
        private bool _isFeedbackVisible;
        private string _headline;

        /// <summary>
        /// Locks used to assure executions in order of LoadContent and LoadSettings methods and to avoid parallel executions.
        /// </summary>
        private readonly ConcurrentDictionary<string, bool> _loaderLocks;

        private const string SettingsLockName = "Settings";
        private const string ContentLockName = "Content";

        protected BaseContentViewModel(DataLoaderProvider dataLoaderProvider)
        {
            DataLoaderProvider = dataLoaderProvider;
            _loaderLocks = new ConcurrentDictionary<string, bool>();
            LoadSettings();
        }

        /// <summary> Gets or sets the last loaded location.</summary>
        /// <value> The last loaded location. </value>
        protected Location LastLoadedLocation
        {
            get => _lastLoadedLocation;
            set => SetProperty(ref _lastLoadedLocation, value);
        }
        /// <summary> Gets or sets the last loaded language. </summary>
        /// <value> The last loaded language.</value>
        protected Language LastLoadedLanguage
        {
            get => _lastLoadedLanguage;
            private set => SetProperty(ref _lastLoadedLanguage, value);
        }

        /// <summary> Gets or sets the error message that a view may display. </summary>
        // ReSharper disable once MemberCanBeProtected.Global
        public string ErrorMessage
        {
            // ReSharper disable once MemberCanBePrivate.Global
            get => _errorMessage;
            set => SetProperty(ref _errorMessage, value,
                () => OnPropertyChanged(nameof(ErrorMessageVisible)));
        }

        /// <summary>
        /// Gets the toolbar items for this page.
        /// </summary>
        public List<ToolbarItem> ToolbarItems { get; protected set; }

        /// <summary>
        /// Gets or sets indicating whether this <see cref="T:Integreat.Shared.ViewModels.BaseContentViewModel"/> show headline.
        /// </summary>
        public bool ShowHeadline
        {
            get => _showHeadline;
            set => SetProperty(ref _showHeadline, value);
        }

        /// <summary>
        /// Gets or sets the headline.
        /// </summary>
        public string Headline
        {
            get => _headline;
            set => SetProperty(ref _headline, value);
        }

        public bool IsFeedbackVisible
        {
            get => _isFeedbackVisible;
            set => SetProperty(ref _isFeedbackVisible, value);
        }

        /// <summary> Gets a value indicating whether the [error message should be visible]. </summary>
        // ReSharper disable once MemberCanBePrivate.Global is used in xaml
        public bool ErrorMessageVisible => !string.IsNullOrWhiteSpace(ErrorMessage);

        /// <summary>
        /// Loads the location and language from the settings and finally loads their models from the persistence service.
        /// </summary>
        private async void LoadSettings()
        {
            // wait until we're not busy anymore
            await GetLock(SettingsLockName);
            IsBusy = true;
            LastLoadedLocation = null;
            LastLoadedLanguage = null;
            var locationId = Preferences.Location();
            var languageId = Preferences.Language(locationId);
            LastLoadedLocation =
                (await DataLoaderProvider.LocationsDataLoader.Load(false, err => ErrorMessage = err)).FirstOrDefault(
                    x =>
                        x.Id == locationId);
            LastLoadedLanguage =
                (await DataLoaderProvider.LanguagesDataLoader.Load(false, LastLoadedLocation, err => ErrorMessage = err)
                )
                .FirstOrDefault(x => x.PrimaryKey == languageId);

            Headline = LastLoadedLocation?.Name ?? "Integreat" ;
            IsBusy = false;
            await ReleaseLock(SettingsLockName);
        }

        /// <inheritdoc />
        /// <summary> Called when [refresh]. </summary>
        /// <param name="force">if set to <c>true</c> [force].</param>
        public override async void OnRefresh(bool force = false)
        {
            // get locks for both settings and content, because we want to ensure that 
            // IF settings are loading right now, the content loader DOES wait for it
            await GetLock(SettingsLockName);
            await GetLock(ContentLockName);
            // reset error message
            ErrorMessage = null;
            try
            {
                LoadContent(force);
            }
            finally
            {
                await ReleaseLock(SettingsLockName);
                await ReleaseLock(ContentLockName);
            }
        }

        /// <inheritdoc />
        /// <summary> Called when [metadata changed]. </summary>
        protected override void OnMetadataChanged()
        {
            LoadSettings();
            OnRefresh(true);
        }

        private async Task ReleaseLock(string callerFileName)
        {
            while (!_loaderLocks.TryUpdate(callerFileName, false, true)) await Task.Delay(200);
        }

        private async Task GetLock(string callerFileName)
        {
            while (true)
            {
                // try to get the key, if it doesn't exist, add it. Try this until the value is false(is unlocked)
                while (_loaderLocks.GetOrAdd(callerFileName, false))
                {
                    // wait 500ms until the next try
                    await Task.Delay(500);
                }
                if (_loaderLocks.TryUpdate(callerFileName, true, false))
                {
                    // if the method returns true, this thread achieved to update the lock. Therefore we're done and leave the method
                    return;
                }
            }
        }

        /// <summary>  Loads or reloads the content for the given language/location. </summary>
        /// <param name="forced">Whether the load is forced or not. A forced load will always result in fetching data from the server.</param>
        /// <param name="forLanguage">The language to load the content for.</param>
        /// <param name="forLocation">The location to load the content for.</param>
        protected abstract void LoadContent(bool forced = false, Language forLanguage = null, Location forLocation = null);

        protected static List<ToolbarItem> GetPrimaryToolbarItemsComplete(ICommand openSearchCommand, ICommand changeLanguageCommand) 
            => new List<ToolbarItem>
            {
                new ToolbarItem { Text = AppResources.Search, Icon = "search", Order = ToolbarItemOrder.Primary, Command = openSearchCommand},
                new ToolbarItem { Text = AppResources.Language, Icon = "translate", Order = ToolbarItemOrder.Primary, Command = changeLanguageCommand },
#if __ANDROID__
                new ToolbarItem { Text = AppResources.Share, Order = ToolbarItemOrder.Secondary, Icon = "share", Command = ContentContainerViewModel.Current.ShareCommand },
                new ToolbarItem { Text = AppResources.Location, Order = ToolbarItemOrder.Secondary, Command = ContentContainerViewModel.Current.OpenLocationSelectionCommand },
                new ToolbarItem { Text = AppResources.Settings, Order = ToolbarItemOrder.Secondary, Command = ContentContainerViewModel.Current.OpenSettingsCommand }
#endif
            };

        protected static List<ToolbarItem> GetPrimaryToolbarItemsTranslate(ICommand changeLanguageCommand) 
            => new List<ToolbarItem>
            {
                new ToolbarItem { Text = AppResources.Language, Icon = "translate", Order = ToolbarItemOrder.Primary, Command = changeLanguageCommand },
#if __ANDROID__
                new ToolbarItem { Text = AppResources.Share, Order = ToolbarItemOrder.Secondary, Icon = "share", Command = ContentContainerViewModel.Current.ShareCommand },
                new ToolbarItem { Text = AppResources.Location, Order = ToolbarItemOrder.Secondary, Command = ContentContainerViewModel.Current.OpenLocationSelectionCommand },
                new ToolbarItem { Text = AppResources.Settings, Order = ToolbarItemOrder.Secondary, Command = ContentContainerViewModel.Current.OpenSettingsCommand }
#endif
            };

        protected static List<ToolbarItem> GetPrimaryToolbarItemsSettingsPage() 
            => new List<ToolbarItem>
            {
#if __ANDROID__
                new ToolbarItem { Text = AppResources.Share, Order = ToolbarItemOrder.Secondary, Icon = "share", Command = ContentContainerViewModel.Current.ShareCommand },
                new ToolbarItem { Text = AppResources.Location, Order = ToolbarItemOrder.Secondary, Command = ContentContainerViewModel.Current.OpenLocationSelectionCommand },               
#endif
            };
    }
}
