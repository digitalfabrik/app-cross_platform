using Integreat.Shared.Models;
using Integreat.Shared.Services.Loader;
using Integreat.Shared.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using Xamarin.Forms;

namespace Integreat.Shared.ViewModels
{
    public class MainPageViewModel : BaseViewModel
    {
        private readonly PageLoader _pageLoader;
        private readonly Func<Models.Page, PageViewModel> _pageViewModelFactory;
        public NavigationViewModel NavigationViewModel { get; }
        public TabViewModel TabViewModel { get; }
        private readonly PagesViewModel _pagesViewModel;
        private IEnumerable<PageViewModel> _pages;

        public MainPageViewModel(Func<Language, Location, PageLoader> pageLoaderFactory,
            Func<Models.Page, PageViewModel> pageViewModelFactory,
            PagesViewModel pagesViewModel, NavigationViewModel navigationViewModel, TabViewModel tabViewModel)
        {
            Title = "Information";
            var locationId = Preferences.Location(); // new Location { Path = "/wordpress/augsburg/" };
            //				var location = await persistence.Get<Location> (locationId);
            //				var languageId = Preferences.Language (location); // new Language { ShortName = "de" };
            //				var language = await persistence.Get<Language> (languageId);
            var language = new Language(0, "de", "Deutsch",
                "http://vmkrcmar21.informatik.tu-muenchen.de//wordpress//augsburg//wp-content//plugins//sitepress-multilingual-cms//res//flags//de.png");
            var location = new Location(0, "Augsburg",
                "http://vmkrcmar21.informatik.tu-muenchen.de//wordpress//wp-content//uploads//sites//2//2015//10//cropped-Logo-Stadt_Augsburg-rotgruen-RGB.jpg",
                "http://vmkrcmar21.informatik.tu-muenchen.de/wordpress/augsburg/",
                "Es schwäbelt", "yellow",
                "http://vmkrcmar21.informatik.tu-muenchen.de/wordpress/augsburg/wp-content/uploads/sites/2/2015/11/cropped-Augsburg.jpg",
                0, 0, false);

            _pageLoader = pageLoaderFactory(language, location);
            _pageViewModelFactory = pageViewModelFactory;
            _pagesViewModel = pagesViewModel;
            _pagesViewModel.LoadPagesCommand = LoadPagesCommand;

            NavigationViewModel = navigationViewModel;
            NavigationViewModel.PropertyChanged += delegate(object sender, PropertyChangedEventArgs args)
            {
                if (args.PropertyName.Equals("SelectedPage"))
                {
                    _pagesViewModel.SelectedPage = NavigationViewModel.SelectedPage;
                }
            };
            TabViewModel = tabViewModel;

            LoadPages();
        }

        public IEnumerable<PageViewModel> Pages
        {
            get { return _pages; }
            set { SetProperty(ref _pages, value); }
        }

        private async void LoadPages()
        {
            Console.WriteLine("LoadPages called");
            if (IsBusy)
            {
                return;
            }
            try
            {
                IsBusy = true;
                var pages = await _pageLoader.Load();
                Pages = pages.Select(page => _pageViewModelFactory(page)).ToList();

                _pagesViewModel.LoadedPages = new ObservableCollection<PageViewModel>(Pages);
                NavigationViewModel.Pages =
                    new ObservableCollection<PageViewModel>(Pages.Where(x => x.Page.ParentId <= 0)
                        .OrderBy(x => x.Page.Order));
            }
            finally
            {
                IsBusy = false;
            }
        }

        private Command _loadPagesCommand;
        public Command LoadPagesCommand => _loadPagesCommand ?? (_loadPagesCommand = new Command(LoadPages));
    }
}
