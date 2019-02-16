using Integreat.Localization;
using Integreat.Shared.Services;
using Integreat.Shared.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Input;
using Integreat.Data.Loader;
using Integreat.Model;
using Integreat.Utilities;
using Xamarin.Forms;
// ReSharper disable MemberCanBePrivate.Global

namespace Integreat.Shared.ViewModels
{
    /// <inheritdoc />
    /// <summary>
    /// ViewModel class for Location
    /// </summary>
    public class LocationsViewModel : BaseViewModel
    {
        private IEnumerable<Location> _locations;
        private ICollection<Location> _foundLocations;
        private readonly INavigator _navigator;

        public LocationsViewModel(DataLoaderProvider dataLoaderProvider, Func<Location, LanguagesViewModel> languageFactory,
            INavigator navigator)
        {
            WhereAreYouText = AppResources.WhereAreYou;
            Title = AppResources.Location;
            _navigator = navigator;
            _languageFactory = languageFactory;
            _dataLoaderProvider = dataLoaderProvider;
            SearchPlaceholderText = AppResources.Search;
        }

        public ICollection<Location> FoundLocations
        {
            get => _foundLocations;
            set => SetProperty(ref _foundLocations, value, () => OnPropertyChanged(nameof(GroupedLocations)));
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
            set => SetProperty(ref _errorMessage, value, () => OnPropertyChanged(nameof(ErrorMessageVisible)));
        }

        public string SearchPlaceholderText { get; set; }

        /// <summary>
        /// Gets a value indicating whether the [error message should be visible].
        /// </summary>
        public bool ErrorMessageVisible => !string.IsNullOrWhiteSpace(ErrorMessage);

        /// <summary>
        /// Gets the grouped locations.
        /// </summary>
        public ICollection<Grouping<string, Location>> GroupedLocations => GetGroupedLocations();
        /// <summary>
        /// The FoundLocations, but grouped after the GroupKey property (which is the first letter of the name).
        /// </summary>
        private List<Grouping<string, Location>> GetGroupedLocations()
            => FoundLocations == null ? null : (from location in FoundLocations
                                                group location by location.GroupKey into locationGroup
                                                select new Grouping<string, Location>(locationGroup.Key, locationGroup)).ToList();

        private readonly Func<Location, LanguagesViewModel> _languageFactory;

        private Location _selectedLocation;
        public Location SelectedLocation
        {
            get => _selectedLocation;
            set => SetProperty(ref _selectedLocation, value, HandleSelectedLocation);
        }

        private void HandleSelectedLocation()
        {
            if (_selectedLocation != null)
            {
                LocationSelected();
            }
        }

        private async void LocationSelected()
        {
            Preferences.SetLocation(_selectedLocation);
            // get the language viewModel
            var languageVm = _languageFactory(_selectedLocation);
            // force a refresh (since the location has changed)
            languageVm.RefreshCommand.Execute(true);
            await _navigator.PushAsync(languageVm);
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
            => string.Compare(a.NameWithoutStreetPrefix, b.NameWithoutStreetPrefix, StringComparison.Ordinal);

        #region View Data

        private string _searchText = string.Empty;
        public string SearchText
        {
            get => _searchText;
            set => SetProperty(ref _searchText, value, Search);
        }

        #endregion

        #region Commands

        private ICommand _forceRefreshLocationsCommand;
        private string _whereAreYouText;
        private readonly DataLoaderProvider _dataLoaderProvider;
        private string _errorMessage;
        public ICommand ForceRefreshLocationsCommand
            => _forceRefreshLocationsCommand ?? (_forceRefreshLocationsCommand = new Command(() => ExecuteLoadLocations(true)));

        public void Search()
        {
            FoundLocations = _locations?.Where(x => x.Find(SearchText)).ToList();
        }
        #endregion
    }
}