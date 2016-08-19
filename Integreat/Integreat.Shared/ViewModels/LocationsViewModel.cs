using System;
using System.Collections.Generic;
using System.Linq;
using Integreat.Shared.Models;
using Integreat.Shared.Services;
using Integreat.Shared.Services.Loader;
using Integreat.Shared.Services.Tracking;
using Integreat.Shared.Utilities;
using Xamarin.Forms;

namespace Integreat.Shared.ViewModels
{
    public class LocationsViewModel : BaseViewModel
    {
        private IEnumerable<Location> _locations;
        private List<Location> _foundLocations;
        public List<Location> FoundLocations
        {
            get { return _foundLocations; }
            set { SetProperty(ref _foundLocations, value); }
        }

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

        public LocationsViewModel(IAnalyticsService analytics, LocationsLoader locationsLoader, Func<Location, LanguagesViewModel> languageFactory,
            INavigator navigator)
      :base(analytics) {
            Title = "Select a Location";
            Description = "Where do you live?";
            _navigator = navigator;
            _navigator.HideToolbar(this);
            _languageFactory = languageFactory;
            _locationsLoader = locationsLoader;

            ExecuteLoadLocations();
        }

        private Command _loadLocations;
        public Command LoadLocationCommand => _loadLocations ?? (_loadLocations = new Command(() => ExecuteLoadLocations()));

        private async void ExecuteLoadLocations(bool forceRefresh = false)
        {
            if (IsBusy)
            {
                return;
            }
            try
            {
                IsBusy = true;
                _locations = await _locationsLoader.Load(forceRefresh);
                Search();
            }
            finally
            {
                IsBusy = false;
            }

           Console.WriteLine ("Locations loaded");
        }

        #region View Data

        private string _searchText = string.Empty;
        public string SearchText
        {
            get { return _searchText; }
            set
            {
                if (SetProperty(ref _searchText, value))
                {
                    Search();
                }
            }
        }

        #endregion

        #region Commands

        private Command _forceRefreshLocationsCommand;
        public Command ForceRefreshLocationsCommand => _forceRefreshLocationsCommand ?? (_forceRefreshLocationsCommand = new Command(() => ExecuteLoadLocations(true)));


        public void Search()
        {
            FoundLocations = _locations?.Where(x => x.Find(SearchText)).ToList();
        }
        #endregion
    }
}
