using System;
using System.Collections.Generic;
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
        public string Description { get; set; }

        private readonly LocationsLoader _locationsLoader;
        private readonly Func<Location, LanguagesViewModel> _languageFactory;

        private Location _selectedLocation;
        public Location SelectedLocation
        {
            get { return _selectedLocation; }
            set
            {
                if (SetProperty(ref _selectedLocation, value))
                {
                    if (_selectedLocation != null)
                    {
                        LocationSelected();
                    }
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
            Title = "Select a Location";
            Description = "Where do you live?";
            _navigator = navigator;
            _languageFactory = languageFactory;
            _locationsLoader = locationsLoader;
            Items = new ObservableCollection<Location>();

            ExecuteLoadLocations();
        }

        private IEnumerable<Location> _items;

        public IEnumerable<Location> Items
        {
            get { return _items; }
            set
            {
                SetProperty(ref _items, value);
            }
        }

        private Command _loadLocations;
        public Command LoadLocationCommand => _loadLocations ?? (_loadLocations = new Command(ExecuteLoadLocations));

        private async void ExecuteLoadLocations()
        {
           Items = await _locationsLoader.Load ();
           Console.WriteLine ("Locations loaded");
        }
    }
}
