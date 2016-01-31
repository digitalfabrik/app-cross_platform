using Integreat.Shared.Models;
using Integreat.Shared.Services.Loader;
using Integreat.Shared.Utilities;
using Integreat.Shared.ViewModels;
using System;
using System.Collections.ObjectModel;
using Integreat.Shared.Services;
using Xamarin.Forms;

namespace Integreat.Shared
{
	public class LanguagesViewModel : BaseViewModel
    {
        public string Description;
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
	            OnPropertyChanged();
	            LanguageSelected();
	        }
	    }

	    private async void LanguageSelected()
	    {
            Preferences.SetLanguage(_location, SelectedLanguage);
	        await _navigator.PushAsync(_mainPageViewModelFactory());
	    }

	    public LanguagesViewModel (Location location, Func<Location, LanguagesLoader> languageLoaderFactory, INavigator navigator,
            Func<MainPageViewModel> mainPageViewModelFactory)
        {
			Title = "Select Language";
			Description = "What language do you speak?";
		    _navigator = navigator;
	        _mainPageViewModelFactory = mainPageViewModelFactory;

            Items = new ObservableCollection<Language>();
            _location = location;
            LanguagesLoader = languageLoaderFactory(_location);
            ExecuteLoadLanguages();
        }

	    private ObservableCollection<Language> _items;

	    public ObservableCollection<Language> Items
	    {
	        get { return _items; }
	        set
	        {
	            _items = value;
	            OnPropertyChanged();
	        }
	    }

	    private Command _loadLanguages;
        public Command LoadLanguagesCommand => _loadLanguages ?? (_loadLanguages = new Command(ExecuteLoadLanguages));

        private async void ExecuteLoadLanguages()
        {           //			var loadedItems = await LocationsLoader.Load ();
                    //			Console.WriteLine ("Locations loaded");

            Items.Clear();
            //			Items.AddRange (loadedItems);
            for (int i = 0; i < 10; i++)
            {
                Items.Add(new Language()
                {
                    Name = $"Location {i}",
                    IconPath = "http://vmkrcmar21.informatik.tu-muenchen.de//wordpress//wp-content//uploads//sites//10//2015//10//cropped-Regensburg.jpg"
                });
            }
        }
	}
}

