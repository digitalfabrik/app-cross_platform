using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Integreat.Shared.Models;
using Integreat.Shared.Services.Loader;
using Xamarin.Forms;
using Integreat.Shared.Utilities;

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
            var locationId = Preferences.Location();// new Location { Path = "/wordpress/augsburg/" };
                                                    //				var location = await persistence.Get<Location> (locationId);
                                                    //				var languageId = Preferences.Language (location); // new Language { ShortName = "de" };
                                                    //				var language = await persistence.Get<Language> (languageId);
            _eventPageLoaderFactory = eventPageLoaderFactory;
            _eventPageViewModelFactory = eventPageViewModelFactory;

            Language = new Language(0, "de", "Deutsch", "http://vmkrcmar21.informatik.tu-muenchen.de//wordpress//augsburg//wp-content//plugins//sitepress-multilingual-cms//res//flags//de.png");
            Location = new Location(0, "Augsburg",
                               "http://vmkrcmar21.informatik.tu-muenchen.de//wordpress//wp-content//uploads//sites//2//2015//10//cropped-Logo-Stadt_Augsburg-rotgruen-RGB.jpg",
                               "http://vmkrcmar21.informatik.tu-muenchen.de/wordpress/augsburg/",
                               "Es schwäbelt", "yellow", "http://vmkrcmar21.informatik.tu-muenchen.de/wordpress/augsburg/wp-content/uploads/sites/2/2015/11/cropped-Augsburg.jpg",
                               0, 0, false);
        }

		private Command _loadEventPagesCommand;

		public Command LoadEventPagesCommand => _loadEventPagesCommand ??
		                                        (_loadEventPagesCommand = new Command (LoadEventPages));

        private async void LoadEventPages()
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
                var pages = await pageLoader.Load();
                EventPages = pages.OrderBy(x => x.Modified).Select(page => _eventPageViewModelFactory(page));
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
