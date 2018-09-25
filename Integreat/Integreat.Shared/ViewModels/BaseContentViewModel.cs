using System.Collections.Generic;
using System.Windows.Input;
using Integreat.Localization;
using Integreat.Shared.Models;
using Integreat.Shared.Utilities;
using Integreat.Utilities;
using Xamarin.Forms;

namespace Integreat.Shared.ViewModels
{
    /// <inheritdoc />
    /// <summary>
    /// Provides a base class for big content pages. Features methods to load/store/reload the selected location and language.
    /// </summary>
    public abstract class BaseContentViewModel : BaseViewModel
    {
        private string _errorMessage;
        private bool _showHeadline;
        private string _headline;

        private readonly CurrentInstance _currentInstance;


        protected BaseContentViewModel(CurrentInstance currentInstance)
        {
            _currentInstance = currentInstance;

            MessagingCenter.Subscribe<CurrentInstance>(this, Constants.InstanceChangedMessage, (sender) => OnMetadataChanged());

            LoadInstance();
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

        /// <summary> Gets a value indicating whether the [error message should be visible]. </summary>
        // ReSharper disable once MemberCanBePrivate.Global is used in xaml
        public bool ErrorMessageVisible => !string.IsNullOrWhiteSpace(ErrorMessage);

        /// <summary>
        /// All that have to be done in the vm's if a Instance has changed
        /// </summary>
        private async void LoadInstance()
        {
            Headline = _currentInstance.Location?.Name ?? "Integreat";
        }

        /// <inheritdoc />
        /// <summary> Called when [refresh]. </summary>
        /// <param name="force">if set to <c>true</c> [force].</param>
        public override async void OnRefresh(bool force = false)
        {
            // reset error message
            ErrorMessage = null;
            LoadContent(force);
        }

        /// <inheritdoc />
        /// <summary> Called when [metadata changed]. </summary>
        protected override void OnMetadataChanged()
        {
            LoadInstance();
            OnRefresh(true);
        }

        /// <summary>  Loads or reloads the content for the given language/location. </summary>
        /// <param name="forced">Whether the load is forced or not. A forced load will always result in fetching data from the server.</param>
        /// <param name="forLanguage">The language to load the content for.</param>
        /// <param name="forLocation">The location to load the content for.</param>
        protected abstract void LoadContent(bool forced = false);

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
