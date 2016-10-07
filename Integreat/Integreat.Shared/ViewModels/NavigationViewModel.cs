using Integreat.Shared.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using Integreat.Shared.Models;
using Integreat.Shared.Services.Tracking;

namespace Integreat.Shared.ViewModels
{
    public class NavigationViewModel : BaseViewModel
    {
        private IEnumerable<PageViewModel> _pages;

        public IEnumerable<PageViewModel> Pages
        {
            get { return _pages; }
            set
            {
                SetProperty(ref _pages, value);
            }
        }

        /// <summary>
        /// Gets or sets the boolean indicating whether the Master panel is displayed or not
        /// </summary>
        public bool IsPresented
        {
            get { return _isPresented; }
            set { SetProperty(ref _isPresented, value); }
        }

        private bool _isPresented;

        private string _thumbnail;

        public string Thumbnail
        {
            get { return _thumbnail; }

            set { SetProperty(ref _thumbnail, value); }
        }

        private Language _language;
        private Location _location;
        private readonly INavigator _navigator;
        private readonly Func<Language, Location, DisclaimerViewModel> _disclaimerFactory;
        private PageViewModel _selectedPage;
        private readonly Func<LocationsViewModel> _locationsFactory;
        
        public PageViewModel SelectedPage
        {
            get { return _selectedPage; }
            set
            {
                SetProperty(ref _selectedPage, value);
            }
        }

        public NavigationViewModel(IAnalyticsService analytics, INavigator navigator, Func<Language, Location, DisclaimerViewModel> disclaimerFactory, Func<LocationsViewModel> locationFactory)
        :base(analytics) {
            Console.WriteLine("NavigationViewModel initialized");
            _navigator = navigator;
            Pages = new ObservableCollection<PageViewModel>();
            _disclaimerFactory = disclaimerFactory;
            _locationsFactory = locationFactory;
        }

        private Command _openDisclaimerCommand;
        public Command OpenDisclaimerCommand => _openDisclaimerCommand ?? (_openDisclaimerCommand = new Command(OnOpenDisclaimerClicked));
        private async void OnOpenDisclaimerClicked()
        {
            IsPresented = false; // close master page
            await _navigator.PushAsync(_disclaimerFactory(_language, _location));
        }

        private Command _openStartCommand;
        public Command OpenStartCommand => _openStartCommand ?? (_openStartCommand = new Command(OnOpenStartCommand));
        private async void OnOpenStartCommand()
        {
            await _navigator.PushAsyncToTopWithNavigation(_locationsFactory());
        }

        internal void SetLanguage(Language language)
        {
            _language = language;
        }

        internal void SetLocation(Location location)
        {
            _location = location;
            Title = location?.Name;
            Thumbnail = location?.CityImage;
        }
    }
}
