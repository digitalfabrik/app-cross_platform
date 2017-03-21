using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Integreat.Shared.Models;
using Integreat.Shared.Services;
using Integreat.Shared.Services.Loader;
using Integreat.Shared.Services.Tracking;
using Integreat.Shared.Utilities;
using Xamarin.Forms;

namespace Integreat.Shared.ViewModels {
    public class LocationsViewModel : BaseViewModel {
        private IEnumerable<Location> _locations;
        private List<Location> _foundLocations;
        public List<Location> FoundLocations {
            get { return _foundLocations; }
            set
            {
                SetProperty(ref _foundLocations, value);
                // raise property changed event for groupedLocation (as it relies on FoundLocations)
                OnPropertyChanged(nameof(GroupedLocations));
            }
        }

        /// <summary>
        /// The FoundLocations, but grouped after the GroupKey property (which is the first letter of the name).
        /// </summary>
        public List<Grouping<string, Location>> GroupedLocations => FoundLocations == null ? null : (from location in FoundLocations
                                                                     group location by location.GroupKey into locationGroup
                                                                     select new Grouping<string, Location>(locationGroup.Key, locationGroup)).ToList();

        private readonly INavigator _navigator;
        public string Description { get; set; }

        private readonly LocationsLoader _locationsLoader;
        private readonly Func<Location, LanguagesViewModel> _languageFactory;

        private Location _selectedLocation;
        public Location SelectedLocation {
            get { return _selectedLocation; }
            set {
                if (SetProperty(ref _selectedLocation, value)) {
                    if (_selectedLocation != null) {
                        LocationSelected();
                    }
                }
            }
        }

        public ICommand OnLanguageSelectedCommand {
            get { return _onLanguageSelectedCommand; }
            set { SetProperty(ref _onLanguageSelectedCommand, value); }
        }

        private async void LocationSelected() {
            Preferences.SetLocation(_selectedLocation);
            // get the language viewModel
            var languageVm = _languageFactory(_selectedLocation);
            // set the command that'll be executed when a language was selected
            languageVm.OnLanguageSelectedCommand = OnLanguageSelectedCommand;
            await _navigator.PushModalAsync(languageVm);
        }

        public LocationsViewModel(IAnalyticsService analytics, LocationsLoader locationsLoader, Func<Location, LanguagesViewModel> languageFactory,
            INavigator navigator)
      : base(analytics) {
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

        private async void ExecuteLoadLocations(bool forceRefresh = false) {
            if (IsBusy) {
                return;
            }
            try {
                IsBusy = true;
                // put locations into list and sort them.
                var asList = new List<Location>(await _locationsLoader.Load(forceRefresh));
                asList.Sort(CompareLocations);
                // then set the field
                _locations = asList;
                Search();
            } finally {
                IsBusy = false;
            }

            Console.WriteLine("Locations loaded");
        }

        private static int CompareLocations(Location a, Location b) {
            return string.Compare(a.NameWithoutStreetPrefix, b.NameWithoutStreetPrefix, StringComparison.Ordinal);
        }

        #region View Data

        private string _searchText = string.Empty;
        public string SearchText {
            get { return _searchText; }
            set {
                if (SetProperty(ref _searchText, value)) {
                    Search();
                }
            }
        }

        #endregion

        #region Commands

        private Command _forceRefreshLocationsCommand;
        private ICommand _onLanguageSelectedCommand;
        public Command ForceRefreshLocationsCommand => _forceRefreshLocationsCommand ?? (_forceRefreshLocationsCommand = new Command(() => ExecuteLoadLocations(true)));


        public void Search() {
            FoundLocations = _locations?.Where(x => x.Find(SearchText)).ToList();
        }
        #endregion
    }
}
