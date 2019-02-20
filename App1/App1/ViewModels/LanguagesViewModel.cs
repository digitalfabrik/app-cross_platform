using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using App1.Data.Loader;
using App1.Models;
using App1.Navigator;
using App1.Utilities;
using App1.ViewFactory;
using Integreat.Localization;

namespace App1.ViewModels
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
        private readonly bool _changeInstance;

        public LanguagesViewModel(Location location, DataLoaderProvider dataLoaderProvider, INavigator navigator, IViewFactory viewFactory, bool changeInstance = true)
        {
            Title = AppResources.Language;
            navigator.HideToolbar(this);

            Items = new ObservableCollection<Language>();
            Location = location;
            _dataLoaderProvider = dataLoaderProvider;
            _viewFactory = viewFactory;

            _changeInstance = changeInstance;
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
            //check if we stay on the same location
            if (_changeInstance)
            {
                Cache.ClearCachedResources();
                ContentContainerViewModel.Current.ChangeLocation(Location);
            }
            Helpers.Platform.GetCurrentMainPage(_viewFactory);
            ContentContainerViewModel.Current.RefreshAll(true);
        }

        public override void OnAppearing()
        {
            ExecuteLoadLanguages();
            base.OnAppearing();
        }

        protected override void OnMetadataChanged()
        {
            ExecuteLoadLanguages(true);
        }

        public override void OnRefresh(bool force = false)
        {
            ExecuteLoadLanguages(force);
        }

        private async void ExecuteLoadLanguages(bool forceRefresh = false)
        {
            if (IsBusy)
                return;
            try
            {
                IsBusy = true;
                // get the languages as list, then sort them
                var asList = new List<Language>(await _dataLoaderProvider.LanguagesDataLoader.Load(forceRefresh, Location, err => ErrorMessage = err));
                asList.Sort(CompareLanguage);
                // set the loaded Languages
                Items = asList;
            }
            finally
            {
                IsBusy = false;
            }
            Console.WriteLine(AppResources.Languages_loaded);
        }

        /// <summary> Compares the language. </summary>
        /// <param name="firstLanguage">first Language.</param>
        /// <param name="secondLanguage">The second Language.</param>
        /// <returns></returns>
        private static int CompareLanguage(Language firstLanguage, Language secondLanguage)
            => string.Compare(firstLanguage.Name, secondLanguage.Name, StringComparison.Ordinal);
    }
}

