using System;
using System.Collections.Generic;
using System.Linq;
using Integreat.Shared.Models;
using Integreat.Shared.Services.Loader;
using Xamarin.Forms;

namespace Integreat.Shared.ViewModels
{
	public class EventPagesViewModel : BaseViewModel
	{
	    private IEnumerable<EventPageViewModel> _eventPages;

	    public IEnumerable<EventPageViewModel> EventPages
	    {
	        get { return _eventPages; }
            set { SetProperty(ref _eventPages, value); }
	    } 

	    private readonly Func<EventPage, EventPageViewModel> _eventPageViewModelFactory;
        private readonly Func<Language, Location, EventPageLoader> _eventPageLoaderFactory;

        private Language _language;

        public Language Language
        {
            get { return _language; }
            set
            {
                if (SetProperty(ref _language, value))
                {
                    LoadEventPages();
                }
            }
        }

        private Location _location;

        public Location Location
        {
            get { return _location; }
            set
            {
                if (SetProperty(ref _location, value))
                {
                    LoadEventPages();
                }
            }
        }

        public EventPagesViewModel(Func<Language, Location, EventPageLoader> eventPageLoaderFactory, Func<EventPage, EventPageViewModel> eventPageViewModelFactory) //TODO page should not be included, but currently needed for dialog
        {
			Title = "Events";
            _eventPageLoaderFactory = eventPageLoaderFactory;
            _eventPageViewModelFactory = eventPageViewModelFactory;
        }

		private Command _loadEventPagesCommand;
		public Command LoadEventPagesCommand => _loadEventPagesCommand ??
		                                        (_loadEventPagesCommand = new Command (() => LoadEventPages()));

        private Command _forceRefreshEventPagesCommand;
        public Command ForceRefreshEventPagesCommand => _forceRefreshEventPagesCommand ??
                                                (_forceRefreshEventPagesCommand = new Command(() => LoadEventPages(true)));

        private async void LoadEventPages(bool forceRefresh = false)
        {
            if (Language == null || Location == null || IsBusy)
            {
                Console.WriteLine("LoadEventPages could not be executed");
                return;
            }
            var pageLoader = _eventPageLoaderFactory(Language, Location);
            Console.WriteLine("LoadPages called");
            try
            {
                IsBusy = true;
                var pages = await pageLoader.Load(forceRefresh);
                EventPages = pages.OrderBy(x => x.Modified).Select(page => _eventPageViewModelFactory(page));
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
