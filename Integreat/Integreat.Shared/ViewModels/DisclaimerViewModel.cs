using System;
using System.Collections.Generic;
using System.Linq;
using Integreat.Shared.Models;
using Integreat.Shared.Services.Loader;
using Integreat.Shared.Services.Tracking;
using Xamarin.Forms;

namespace Integreat.Shared.ViewModels
{
	public class DisclaimerViewModel : BaseViewModel
	{
	    private IEnumerable<PageViewModel> _pages;

	    public IEnumerable<PageViewModel> Pages
	    {
	        get { return _pages; }
	        set { SetProperty(ref _pages, value); }
	    }

        private Command _itemTappedCommand;

        public Command ItemTappedCommand
        {
            get { return _itemTappedCommand; }
            set { SetProperty(ref _itemTappedCommand, value); }
        }

        private PageViewModel _selectedPage;
        public PageViewModel SelectedPage
        {
            get { return _selectedPage; }
            set
            {
                SetProperty(ref _selectedPage, value);
            }
        }

        private async void OnTap(object sender)
        {
            var elem = sender as PageViewModel;
        }

        private readonly DisclaimerLoader _loader;
	    private readonly Func<Models.Page, PageViewModel> _pageFactory;

        public DisclaimerViewModel(IAnalyticsService analytics, Language language, Location location, Func<Models.Page, PageViewModel> pageFactory, Func<Language, Location, DisclaimerLoader> disclaimerLoaderFactory)
        :base(analytics) {
            Title = "Information";

            _pageFactory = pageFactory;
            _loader = disclaimerLoaderFactory(language, location);
            _itemTappedCommand = new Command(OnTap);
            Refresh();
        }
        private Command _loadPagesCommand;
        public Command LoadPagesCommand => _loadPagesCommand ??  (_loadPagesCommand = new Command(() => Refresh()));

        private async void Refresh(bool forceRefresh = false)
        {
            try
            {
                IsBusy = true;
                //var parentPageId = _selectedPage?.Page?.PrimaryKey ?? Models.Page.GenerateKey(0, Location, Language);
                var pages = await _loader.Load(forceRefresh);

                Pages = pages?.Select(x => _pageFactory(x)).ToList();
                Console.WriteLine("Disclaimer count: " + pages?.Count);
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
