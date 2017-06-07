using Integreat.Shared.Models;
using Integreat.Shared.Utilities;
using Integreat.Shared.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Integreat.Shared.Data.Loader;
using Integreat.Shared.Services;
using Integreat.Shared.Services.Tracking;
using Xamarin.Forms;
using localization;

namespace Integreat.Shared
{
    public class LanguagesViewModel : BaseViewModel
    {
        public string Description { get; set; }
        private readonly INavigator _navigator;

        private readonly Location _location;
        public Location Location => _location;


        private Language _selectedLanguage;
        public Language SelectedLanguage
        {
            get { return _selectedLanguage; }
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
            get { return _onLanguageSelectedCommand; }
            set { SetProperty(ref _onLanguageSelectedCommand, value); }
        }
        private Command _loadLanguages;
        public Command LoadLanguagesCommand => _loadLanguages ?? (_loadLanguages = new Command(() => ExecuteLoadLanguages()));

        private Command _forceRefreshLanguagesCommand;
        private ICommand _onLanguageSelectedCommand;
        public Command ForceRefreshLanguagesCommand => _forceRefreshLanguagesCommand ?? (_forceRefreshLanguagesCommand = new Command(() => ExecuteLoadLanguages(true)));


        private IEnumerable<Language> _items;
        private DataLoaderProvider _dataLoaderProvider;
        private string _errorMessage;


        /// <summary>
        /// Gets or sets the error message that a view may display.
        /// </summary>
        public string ErrorMessage {
            get { return _errorMessage; }
            set {
                SetProperty(ref _errorMessage, value);
                OnPropertyChanged(nameof(ErrorMessageVisible));
            }
        }

        /// <summary>
        /// Gets a value indicating whether the [error message should be visible].
        /// </summary>
        public bool ErrorMessageVisible => !string.IsNullOrWhiteSpace(ErrorMessage);

        public IEnumerable<Language> Items
        {
            get { return _items; }
            set
            {
                SetProperty(ref _items, value);
            }
        }


        public LanguagesViewModel(IAnalyticsService analytics, Location location, DataLoaderProvider dataLoaderProvider, INavigator navigator)
        : base(analytics)
        {
            Title = AppResources.Language;
            _navigator = navigator;
            _navigator.HideToolbar(this);

            Items = new ObservableCollection<Language>();
            _location = location;
            _dataLoaderProvider = dataLoaderProvider;
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

        private static int CompareLanguage(Language a, Language b)
        {
            return string.Compare(a.Name, b.Name, StringComparison.Ordinal);
        }
    }
}

