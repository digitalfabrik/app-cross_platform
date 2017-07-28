using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Input;
using Integreat.Shared.Data.Loader;
using Integreat.Shared.Models;
using Integreat.Shared.Services;
using Integreat.Shared.Services.Tracking;
using Integreat.Shared.Utilities;
using localization;
using Xamarin.Forms;

namespace Integreat.Shared.ViewModels
{
    public class LocationsViewModel : BaseViewModel
    {
        private IEnumerable<Location> _locations;
        private List<Location> _foundLocations;
        public List<Location> FoundLocations
        {
            get => _foundLocations;
            set
            {
                SetProperty(ref _foundLocations, value);
                // raise property changed event for groupedLocation (as it relies on FoundLocations)
                OnPropertyChanged(nameof(GroupedLocations));
            }
        }

        public string WhereAreYouText
        {
            get => _whereAreYouText;
            set => SetProperty(ref _whereAreYouText, value);
        }

        /// <summary>
        /// Gets or sets the error message that a view may display.
        /// </summary>
        public string ErrorMessage
        {
            get => _errorMessage;
            set
            {
                SetProperty(ref _errorMessage, value);
                OnPropertyChanged(nameof(ErrorMessageVisible));
            }
        }

        public string SearchPlaceholderText { get; set; }

        /// <summary>
        /// Gets a value indicating whether the [error message should be visible].
        /// </summary>
        public bool ErrorMessageVisible => !string.IsNullOrWhiteSpace(ErrorMessage);

        /// <summary>
        /// The FoundLocations, but grouped after the GroupKey property (which is the first letter of the name).
        /// </summary>
        public List<Grouping<string, Location>> GroupedLocations => FoundLocations == null ? null : (from location in FoundLocations
                                                                                                     group location by location.GroupKey into locationGroup
                                                                                                     select new Grouping<string, Location>(locationGroup.Key, locationGroup)).ToList();

        private readonly INavigator _navigator;
        public string Description { get; set; }

        private readonly Func<Location, LanguagesViewModel> _languageFactory;

        private Location _selectedLocation;
        public Location SelectedLocation
        {
            get => _selectedLocation;
            set
            {
                if (!SetProperty(ref _selectedLocation, value)) return;
                if (_selectedLocation != null)
                {
                    LocationSelected();
                }
            }
        }

        public ICommand OnLanguageSelectedCommand
        {
            get => _onLanguageSelectedCommand;
            set => SetProperty(ref _onLanguageSelectedCommand, value);
        }

        private async void LocationSelected()
        {
            Preferences.SetLocation(_selectedLocation);
            // get the language viewModel
            var languageVm = _languageFactory(_selectedLocation);
            // set the command that'll be executed when a language was selected
            languageVm.OnLanguageSelectedCommand = OnLanguageSelectedCommand;
            // force a refresh (since the location has changed)
            languageVm.RefreshCommand.Execute(true);
            await _navigator.PushAsync(languageVm);
        }

        public LocationsViewModel(IAnalyticsService analytics, DataLoaderProvider dataLoaderProvider, Func<Location, LanguagesViewModel> languageFactory,
            INavigator navigator)
      : base(analytics)
        {
            WhereAreYouText = AppResources.WhereAreYou;
            Title = AppResources.Location;
            _navigator = navigator;
            _languageFactory = languageFactory;
            _dataLoaderProvider = dataLoaderProvider;
            SearchPlaceholderText = AppResources.Search;
        }

        public override void OnAppearing()
        {
            ExecuteLoadLocations();
            base.OnAppearing();
        }

        private async void ExecuteLoadLocations(bool forceRefresh = false)
        {
            if (IsBusy) { return; }
            try
            {
                IsBusy = true;
                // clear list (call property changed, as the FoundLocations property indirectly affects the GroupedLocations, which are the locations displayed)
                FoundLocations?.Clear();
                OnPropertyChanged(nameof(GroupedLocations));
                // put locations into list and sort them.
                var asList = new List<Location>(await _dataLoaderProvider.LocationsDataLoader.Load(forceRefresh, err => ErrorMessage = err));
                asList.Sort(CompareLocations);
                // then set the field
                _locations = asList;
                Search();
            }
            finally
            {
                IsBusy = false;
            }

            Debug.WriteLine("Locations loaded");
        }

        private static int CompareLocations(Location a, Location b)
        {
            return string.Compare(a.NameWithoutStreetPrefix, b.NameWithoutStreetPrefix, StringComparison.Ordinal);
        }

        #region View Data

        private string _searchText = string.Empty;
        public string SearchText
        {
            get => _searchText;
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

        private ICommand _forceRefreshLocationsCommand;
        private ICommand _onLanguageSelectedCommand;
        private string _whereAreYouText;
        private readonly DataLoaderProvider _dataLoaderProvider;
        private string _errorMessage;
        public ICommand ForceRefreshLocationsCommand => _forceRefreshLocationsCommand ?? (_forceRefreshLocationsCommand = new Command(() => ExecuteLoadLocations(true)));

        public void Search()
        {
            FoundLocations = _locations?.Where(x => x.Find(SearchText)).ToList();
        }
        #endregion
    }
}