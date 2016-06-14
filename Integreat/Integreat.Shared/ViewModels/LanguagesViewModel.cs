using Integreat.Shared.Models;
using Integreat.Shared.Services.Loader;
using Integreat.Shared.Utilities;
using Integreat.Shared.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Integreat.Shared.Services;
using Integreat.Shared.Services.Tracking;
using Xamarin.Forms;

namespace Integreat.Shared
{
	public class LanguagesViewModel : BaseViewModel
    {
        public string Description { get; set; }
        public LanguagesLoader LanguagesLoader;
	    private readonly INavigator _navigator;

        private readonly Location _location;
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

	    private async void LanguageSelected()
	    {
            Preferences.SetLanguage(_location, SelectedLanguage);
	        await _navigator.PushAsyncToTop(_mainPageViewModelFactory());
        }

	    public LanguagesViewModel (IAnalyticsService analytics, Location location, Func<Location, LanguagesLoader> languageLoaderFactory, INavigator navigator,
            Func<MainPageViewModel> mainPageViewModelFactory)
        : base (analytics) {
			Title = "Select Language";
			Description = "What language do you speak?";
		    _navigator = navigator;
	        _mainPageViewModelFactory = mainPageViewModelFactory;

            Items = new ObservableCollection<Language>();
            _location = location;
            LanguagesLoader = languageLoaderFactory(_location);
            ExecuteLoadLanguages();
            navigator.HideToolbar(this);
        }

	    private IEnumerable<Language> _items;

	    public IEnumerable<Language> Items
	    {
	        get { return _items; }
	        set
	        {
	            SetProperty(ref _items, value);
	        }
	    }

	    private Command _loadLanguages;
        public Command LoadLanguagesCommand => _loadLanguages ?? (_loadLanguages = new Command(() => ExecuteLoadLanguages()));

        private Command _forceRefreshLanguagesCommand;
        public Command ForceRefreshLanguagesCommand => _forceRefreshLanguagesCommand ?? (_forceRefreshLanguagesCommand = new Command(() => ExecuteLoadLanguages(true)));


        private async void ExecuteLoadLanguages(bool forceRefresh = false)
        {
            if (IsBusy)
            {
                return;
            }
            try
            {
                IsBusy = true;
                Items = await LanguagesLoader.Load(forceRefresh);
            }
            finally
            {
                IsBusy = false;
            }
            Console.WriteLine("Languages loaded");
        }
	}
}

