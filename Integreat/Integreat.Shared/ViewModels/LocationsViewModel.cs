using System;
using System.Collections.ObjectModel;
using Integreat.Shared.Models;
using Integreat.Shared.Services;
using Integreat.Shared.Services.Loader;
using Integreat.Shared.Utilities;
using Xamarin.Forms;

namespace Integreat.Shared.ViewModels
{
    public class LocationsViewModel : BaseViewModel
    {
        private readonly INavigator _navigator;
        public string Description;

        private readonly LocationsLoader _locationsLoader;
        private readonly Func<Location, LanguagesViewModel> _languageFactory;

        private Location _selectedLocation;
        public Location SelectedLocation
        {
            get { return _selectedLocation; }
            set
            {
                _selectedLocation = value;
                OnPropertyChanged();
                if (_selectedLocation != null) { 
                    LocationSelected();
                }
            }
        }

        private async void LocationSelected()
        {
            Preferences.SetLocation(_selectedLocation);
            await _navigator.PushAsync(_languageFactory(_selectedLocation));
        }

        public LocationsViewModel(LocationsLoader locationsLoader, Func<Location, LanguagesViewModel> languageFactory,
            INavigator navigator)
        {
            Title = "Select Language";
            Description = "What language do you speak?";
            _navigator = navigator;
            _languageFactory = languageFactory;
            _locationsLoader = locationsLoader;
            Items = new ObservableCollection<Location>();

            ExecuteLoadLocations();
        }

        private ObservableCollection<Location> _items;

        public ObservableCollection<Location> Items
        {
            get { return _items; }
            set
            {
                _items = value;
                OnPropertyChanged();
            }
        }

        private Command _loadLocations;
        public Command LoadLocationCommand => _loadLocations ?? (_loadLocations = new Command(ExecuteLoadLocations));

        private async void ExecuteLoadLocations()
        {
            //			var loadedItems = await LocationsLoader.Load ();
            //			Items.AddRange (loadedItems);
            //			Console.WriteLine ("Locations loaded");
            
            Items.Clear();
            for (int i = 0; i < 10; i++)
            {
                Items.Add(new Location
                {
                    Name = $"Location {i}",
                    CityImage =
                        "http://vmkrcmar21.informatik.tu-muenchen.de//wordpress//wp-content//uploads//sites//10//2015//10//cropped-Regensburg.jpg"
                });
            }
        }
    }
}
