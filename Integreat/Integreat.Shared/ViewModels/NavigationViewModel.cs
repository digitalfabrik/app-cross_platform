using Integreat.Shared.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using Integreat.Shared.Models;

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

        public string Thumbnail => "http://vmkrcmar21.informatik.tu-muenchen.de/wordpress/augsburg/wp-content/uploads/sites/2/2015/11/cropped-Augsburg.jpg";

        public NavigationViewModel(INavigator navigator, Func<Language, Location, DisclaimerViewModel> disclaimerFactory, Func<LocationsViewModel> locationFactory)
        {
            Console.WriteLine("NavigationViewModel initialized");
            Title = "Navigation";
            _navigator = navigator;
            Pages = new ObservableCollection<PageViewModel>();
            _disclaimerFactory = disclaimerFactory;
            _locationsFactory = locationFactory;
        }

        private Command _openDisclaimerCommand;
        public Command OpenDisclaimerCommand => _openDisclaimerCommand ?? (_openDisclaimerCommand = new Command(OnOpenDisclaimerClicked));
        private async void OnOpenDisclaimerClicked()
        {
            await _navigator.PushModalAsync(_disclaimerFactory(_language, _location));
        }

        private Command _openStartCommand;
        public Command OpenStartCommand => _openStartCommand ?? (_openStartCommand = new Command(OnOpenStartCommand));
        private async void OnOpenStartCommand()
        {
            await _navigator.PushAsyncToTop(_locationsFactory());
        }

        internal void SetLanguage(Language language)
        {
            _language = language;
        }

        internal void SetLocation(Location location)
        {
            _location = location;
        }
    }
}
