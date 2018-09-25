using Integreat.Shared.Data.Loader;
using Integreat.Shared.Models;
using Integreat.Shared.Services;
using Integreat.Shared.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Integreat.Localization;
using Integreat.Shared.ViewFactory;
using Integreat.Utilities;
using Xamarin.Forms;
using System.Linq;

namespace Integreat.Shared.ViewModels
{
    /// <inheritdoc />
    /// <summary>
    /// Languages viewmodel instance
    /// </summary>
    public class LanguagesViewModel : BaseViewModel
    {
        private Language _selectedLanguage;
        private IEnumerable<Language> _items;
        private readonly DataLoaderProvider _dataLoaderProvider;
        private string _errorMessage;
        private readonly IViewFactory _viewFactory;
        private readonly CurrentInstance _currentInstance;

        public LanguagesViewModel(Location location, DataLoaderProvider dataLoaderProvider, INavigator navigator, IViewFactory viewFactory, CurrentInstance currentInstance)
        {
            Title = AppResources.Language;
            navigator.HideToolbar(this);

            Items = new ObservableCollection<Language>();
            Location = location;
            _dataLoaderProvider = dataLoaderProvider;
			_viewFactory = viewFactory;
            _currentInstance = currentInstance;

            MessagingCenter.Subscribe<CurrentInstance>(this, Constants.InstanceChangedMessage, (sender) => ExecuteLoadLanguages(true));
        }

        // ReSharper disable once MemberCanBePrivate.Global
        public Language SelectedLanguage
        {
            get => _selectedLanguage;
            // ReSharper disable once UnusedMember.Global
            set
            {
                _selectedLanguage = value;
                if (value != null)
                {
                    LanguageSelected();
                }
            }
        }

        /// <summary>
        /// Gets or sets the error message that a view may display.
        /// </summary>
        // ReSharper disable once MemberCanBePrivate.Global
        public string ErrorMessage
        {
            get => _errorMessage;
            set
            {
                SetProperty(ref _errorMessage, value);
                OnPropertyChanged(nameof(ErrorMessageVisible));
            }
        }

        /// <summary>
        /// Gets a value indicating whether the [error message should be visible].
        /// </summary>
        // ReSharper disable once MemberCanBePrivate.Global
        public bool ErrorMessageVisible => !string.IsNullOrWhiteSpace(ErrorMessage);

        // ReSharper disable once MemberCanBePrivate.Global
        public IEnumerable<Language> Items
        {
            get => _items;
            set => SetProperty(ref _items, value);
        }

        public Location Location { get; }

        private void LanguageSelected()
        {
            Preferences.SetLanguage(Location, SelectedLanguage);         
            ContentContainerViewModel.Current.ChangeLocation(Location);
            Helpers.Platform.GetCurrentMainPage(_viewFactory);           
            ContentContainerViewModel.Current.RefreshAll(true);
        }

        public override void OnAppearing()
        {
            ExecuteLoadLanguages();
            base.OnAppearing();
        }

        public override void OnRefresh(bool force = false)
        {
            ExecuteLoadLanguages(force);
        }

        private async void ExecuteLoadLanguages(bool forceRefresh = false)
        {
            if (IsBusy)
            {
                return;
            }
            try
            {
                IsBusy = true;
                // get the languages as list, then sort them
                var asList = _currentInstance.AvailableLanguages.OrderBy(l => l.Name).ToList();
                // set the loaded Languages
                Items = asList;
            }
            finally
            {
                IsBusy = false;
            }
            Console.WriteLine(AppResources.Languages_loaded);
        }
    }
}

