using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Integreat.Shared.Models;
using Integreat.Shared.Services.Loader;
using Integreat.Shared.Utilities;
using Xamarin.Forms;

namespace Integreat.Shared.ViewModels
{
	public class PagesViewModel : BaseViewModel
	{
	    private ObservableCollection<PageViewModel> _visiblePages;

	    public ObservableCollection<PageViewModel> VisiblePages
	    {
	        get { return _visiblePages; }
	        set
            {
                SetProperty(ref _visiblePages, value);
	        }
	    }

	    private PageViewModel _selectedPage;
        public PageViewModel SelectedPage { get { return _selectedPage; }
            set
            {
                if (SetProperty(ref _selectedPage, value))
                {
                    FilterPages();
                }
            } }

        private readonly PageLoader _pageLoader;
        private readonly Func<Models.Page, PageViewModel> _pageViewModelFactory;

        public PagesViewModel(Func<Language, Location, PageLoader> pageLoaderFactory,
            Func<Models.Page, PageViewModel> pageViewModelFactory)
        {
            Title = "Information";
            LoadedPages = new ObservableCollection<PageViewModel>();
            VisiblePages = new ObservableCollection<PageViewModel>();

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

            _pageViewModelFactory = pageViewModelFactory;
            _pageLoader = pageLoaderFactory(language, location);
            _pageViewModelFactory = pageViewModelFactory;
        }
        
        private void FilterPages()
        {
            VisiblePages = new ObservableCollection<PageViewModel>(LoadedPages.Where(x=> SelectedPage == null || SelectedPage.Page.Id == x.Page.ParentId).OrderBy(x => x.Page.Order));
        }

	    private IEnumerable<PageViewModel> _loadedPages; 
        public IEnumerable<PageViewModel> LoadedPages
        {
            get { return _loadedPages; }
            set
            {
                SetProperty(ref _loadedPages, value);
                FilterPages();
            }
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
                LoadedPages = pages.Select(page => _pageViewModelFactory(page)).ToList();
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
