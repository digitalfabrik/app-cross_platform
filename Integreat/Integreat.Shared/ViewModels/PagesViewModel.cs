using System;
using System.Collections.Generic;
using System.Linq;
using Integreat.Shared.Models;
using Integreat.Shared.Services.Loader;
using Xamarin.Forms;

namespace Integreat.Shared.ViewModels
{
	public class PagesViewModel : BaseViewModel
	{
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

        private IEnumerable<PageViewModel> _visiblePages;
	    public IEnumerable<PageViewModel> VisiblePages
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

        private readonly Func<Models.Page, PageViewModel> _pageViewModelFactory;
        private readonly Func<Language, Location, PageLoader> _pageLoaderFactory;

        private Language _language;

	    public Language Language
        {
            get { return _language; }
            set
            {
                SetProperty(ref _language, value);
                LoadPages();
            }
        }

	    private Location _location;

        public void SetLanguageLocation(Language language, Location location) {
            _language = language;
            _location = location;
            LoadPages();
        }

	    public Location Location
	    {
	        get { return _location; }
	        set
	        {
                SetProperty(ref _location, value);
	            LoadPages();
	        }
	    }

	    public PagesViewModel(Func<Language, Location, PageLoader> pageLoaderFactory,
            Func<Models.Page, PageViewModel> pageViewModelFactory)
        {
            Title = "Information";

            _pageLoaderFactory = pageLoaderFactory;
            _pageViewModelFactory = pageViewModelFactory;
        }
        
        private void FilterPages()
        {
            VisiblePages = LoadedPages.Where(x=> SelectedPage == null || SelectedPage.Page.Id == x.Page.ParentId).OrderBy(x => x.Page.Order);
        }

	    private async void LoadPages(bool forceRefresh = false)
        {
	        if (Language == null || Location == null || IsBusy)
            {
                Console.WriteLine("LoadPages could not be executed");
                return;
            }
            var pageLoader = _pageLoaderFactory(Language, Location);
            Console.WriteLine("LoadPages called");
            try
            {
                IsBusy = true;
                var pages =  await pageLoader.Load(forceRefresh);
                LoadedPages = pages.Select(page => _pageViewModelFactory(page)).ToList();
            }
            finally
            {
                IsBusy = false;
            }
        }

        private Command _loadPagesCommand;
        public Command LoadPagesCommand => _loadPagesCommand ?? (_loadPagesCommand = new Command(() => LoadPages()));


        private Command _forceRefreshPagesCommand;
        public Command ForceRefreshPagesCommand => _forceRefreshPagesCommand ?? (_forceRefreshPagesCommand = new Command(() => LoadPages(true)));
    }
}
