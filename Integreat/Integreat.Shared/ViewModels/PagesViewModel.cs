using System;
using System.Collections.Generic;
using System.Linq;
using Integreat.Shared.Models;
using Integreat.Shared.Services;
using Integreat.Shared.Services.Loader;
using Integreat.Shared.Services.Tracking;
using Xamarin.Forms;

namespace Integreat.Shared.ViewModels
{
	public class PagesViewModel : BaseViewModel
	{
	    private INavigator _navigator;
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
       
        private List<PageViewModel> _visiblePages;
	    public List<PageViewModel> VisiblePages
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


        private Command _itemTappedCommand;

        public Command ItemTappedCommand
        {
            get { return _itemTappedCommand; }
            set { SetProperty(ref _itemTappedCommand, value); }
        }

        private async void OnTap(object sender)
        {
            var elem = sender as PageViewModel;
            var subpages = LoadedPages.Where(x => x.Page.ParentId == elem.Page.PrimaryKey).ToList();
            if (subpages != null && subpages.Count > 0)
            {
                await _navigator.PushAsync(_detailedPagesViewModelFactory(elem, subpages));
            }
            else
            {
                elem.ShowPageCommand.Execute(null);
            }
        }

        private object item;
        public object LastTappedItem
        {
            get { return item; }
            set { item = value; }
        }

        private readonly Func<PageViewModel, IEnumerable<PageViewModel>, DetailedPagesViewModel> _detailedPagesViewModelFactory;
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

	    public PagesViewModel(IAnalyticsService analytics, Func<Language, Location, PageLoader> pageLoaderFactory,
            Func<Models.Page, PageViewModel> pageViewModelFactory, Func<PageViewModel, IEnumerable<PageViewModel>, DetailedPagesViewModel> detailedPagesViewModelFactory, INavigator navigator)
        : base (analytics) {
            Title = "Information";

            _pageLoaderFactory = pageLoaderFactory;
            _pageViewModelFactory = pageViewModelFactory;
	        _detailedPagesViewModelFactory = detailedPagesViewModelFactory;
            _itemTappedCommand = new Command(OnTap);
	        _navigator = navigator;
        }
       

        private void FilterPages()
        {
            var id = SelectedPage?.Page?.PrimaryKey ?? "0";
            var key = Models.Page.GenerateKey(id, _location, _language);
            VisiblePages = LoadedPages.Where(x=> x.Page.ParentId == key).OrderBy(x => x.Page.Order).ToList();
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
                //var parentPageId = _selectedPage?.Page?.PrimaryKey ?? Models.Page.GenerateKey(0, Location, Language);
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
