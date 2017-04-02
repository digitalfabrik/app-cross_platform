using Integreat.Shared.Models;
using Integreat.Shared.Services.Loader;
using Integreat.Shared.Utilities;
using Integreat.Shared.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Integreat.Shared.Data.Loader;
using Integreat.Shared.Data.Loader.Targets;
using Integreat.Shared.Services;
using Integreat.Shared.Services.Tracking;
using Xamarin.Forms;

namespace Integreat.Shared
{
	public class LanguagesViewModel : BaseViewModel
    {
        public string Description { get; set; }
	    private readonly INavigator _navigator;

        private readonly Location _location;
        public Location Location =>_location;

	    private readonly Func<MainPageViewModel> _mainPageViewModelFactory;

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


        public ICommand OnLanguageSelectedCommand {
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

        public IEnumerable<Language> Items
	    {
	        get { return _items; }
	        set
	        {
	            SetProperty(ref _items, value);
	        }
	    }


	    public LanguagesViewModel (IAnalyticsService analytics, Location location, DataLoaderProvider dataLoaderProvider, INavigator navigator,
            Func<MainPageViewModel> mainPageViewModelFactory)
        : base (analytics) {
			Title = "Language";
		    _navigator = navigator;
            _navigator.HideToolbar(this);
            _mainPageViewModelFactory = mainPageViewModelFactory;

            Items = new ObservableCollection<Language>();
            _location = location;
	        _dataLoaderProvider = dataLoaderProvider;
	    }
        private async void LanguageSelected()
	    {
            Preferences.SetLanguage(_location, SelectedLanguage);
            OnLanguageSelectedCommand?.Execute(this);
        }
        public override void OnAppearing() {
            ExecuteLoadLanguages();
            base.OnAppearing();
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
                var asList =  new List<Language>(await _dataLoaderProvider.LanguageDataLoader.Load(forceRefresh, _location));
                asList.Sort(CompareLanguage);
                // set the loaded Languages
                Items = asList;
            }
            finally
            {
                IsBusy = false;
            }
            Console.WriteLine("Languages loaded");
        }

        private static int CompareLanguage(Language a, Language b)
        {
            return string.Compare(a.Name, b.Name, StringComparison.Ordinal);
        }
	}
}

