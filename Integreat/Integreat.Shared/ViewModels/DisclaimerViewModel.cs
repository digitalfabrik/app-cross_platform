using System;
using System.Collections.Generic;
using System.Linq;
using Integreat.Shared.Models;
using Integreat.Shared.Services.Loader;
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

	    private readonly DisclaimerLoader _loader;
	    private readonly Func<Models.Page, PageViewModel> _pageFactory;

        public DisclaimerViewModel(Language language, Location location, Func<Models.Page, PageViewModel> pageFactory, Func<Language, Location, DisclaimerLoader> disclaimerLoaderFactory)
        {
            Title = "Information";

            _pageFactory = pageFactory;
            _loader = disclaimerLoaderFactory(language, location);
            Refresh();
        }
        private Command _loadPagesCommand;
        public Command LoadPagesCommand => _loadPagesCommand ??  (_loadPagesCommand = new Command(Refresh));

        private async void Refresh()
        {
            var pages = await _loader.Load();
            Console.WriteLine("Disclaimer count: " + pages?.Count);
            Pages = pages?.Select(x => _pageFactory(x));
        }
    }
}
