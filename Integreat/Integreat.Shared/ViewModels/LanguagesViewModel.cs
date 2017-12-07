using Integreat.Shared.Data.Loader;
using Integreat.Shared.Models;
using Integreat.Shared.Services;
using Integreat.Shared.Utilities;
using Integreat.Shared.ViewModels;
using localization;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Integreat.Shared
{
    public class LanguagesViewModel : BaseViewModel
    {
        private readonly Location _location;

        private Language _selectedLanguage;
        private IEnumerable<Language> _items;
        private readonly DataLoaderProvider _dataLoaderProvider;
        private string _errorMessage;

        private ICommand _onLanguageSelectedCommand;

        public LanguagesViewModel(Location location, DataLoaderProvider dataLoaderProvider, INavigator navigator)
        {
            Title = AppResources.Language;
            navigator.HideToolbar(this);

            Items = new ObservableCollection<Language>();
            _location = location;
            _dataLoaderProvider = dataLoaderProvider;
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

        public ICommand OnLanguageSelectedCommand
        {
            private get => _onLanguageSelectedCommand;
            set => SetProperty(ref _onLanguageSelectedCommand, value);
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

        private void LanguageSelected()
        {
            Preferences.SetLanguage(_location, SelectedLanguage);
            OnLanguageSelectedCommand?.Execute(this);
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
            {
                return;
            }
            try
            {
                IsBusy = true;
                // get the languages as list, then sort them
                var asList = new List<Language>(await _dataLoaderProvider.LanguagesDataLoader.Load(forceRefresh, _location, err => ErrorMessage = err));
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
        {
            return string.Compare(firstLanguage.Name, secondLanguage.Name, StringComparison.Ordinal);
        }
    }
}

