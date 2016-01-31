using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Integreat.Shared.Models;
using Integreat.Shared.Services.Loader;
using Xamarin.Forms;
using Integreat.Shared.Utilities;

namespace Integreat.Shared.ViewModels
{
	public class EventPagesViewModel : BaseViewModel
	{
		public ObservableCollection<EventPageViewModel> EventPages { get; set; }
	    private readonly EventPageLoader _eventPageLoader;
	    private readonly Func<EventPage, EventPageViewModel> _eventPageViewModelFactory;

		public EventPagesViewModel(Func<Language, Location, EventPageLoader> eventPageLoaderFactory, Func<EventPage, EventPageViewModel> eventPageViewModelFactory) //TODO page should not be included, but currently needed for dialog
        {
			Title = "Events";
            var locationId = Preferences.Location();// new Location { Path = "/wordpress/augsburg/" };
                                                    //				var location = await persistence.Get<Location> (locationId);
                                                    //				var languageId = Preferences.Language (location); // new Language { ShortName = "de" };
                                                    //				var language = await persistence.Get<Language> (languageId);
            var language = new Language(0, "de", "Deutsch", "http://vmkrcmar21.informatik.tu-muenchen.de//wordpress//augsburg//wp-content//plugins//sitepress-multilingual-cms//res//flags//de.png");
            var location = new Location(0, "Augsburg",
                               "http://vmkrcmar21.informatik.tu-muenchen.de//wordpress//wp-content//uploads//sites//2//2015//10//cropped-Logo-Stadt_Augsburg-rotgruen-RGB.jpg",
                               "http://vmkrcmar21.informatik.tu-muenchen.de/wordpress/augsburg/",
                               "Es schwäbelt", "yellow", "http://vmkrcmar21.informatik.tu-muenchen.de/wordpress/augsburg/wp-content/uploads/sites/2/2015/11/cropped-Augsburg.jpg",
                               0, 0, false);

            _eventPageLoader = eventPageLoaderFactory(language, location);
		    _eventPageViewModelFactory = eventPageViewModelFactory;
            EventPages = new ObservableCollection<EventPageViewModel>();
            ExecuteLoadPagesCommand();
        }

		private Command _loadEventPagesCommand;

		public Command LoadEventPagesCommand => _loadEventPagesCommand ??
		                                        (_loadEventPagesCommand = new Command (ExecuteLoadPagesCommand));

	    public async void ExecuteLoadPagesCommand ()
		{
			if (IsBusy) {
				return;
			}

			IsBusy = true;
			LoadEventPagesCommand.ChangeCanExecute ();
			var error = false;
			try {
				EventPages.Clear ();
				var loadedPages = await LoadEventPages ();
				foreach (var page in loadedPages) {
					EventPages.Add (_eventPageViewModelFactory(page));
				}
			} catch (Exception e) {
				Console.WriteLine (e.Message);
				error = true;
			}

			if (error) {
				var page = new ContentPage ();
				await page.DisplayAlert ("Error", "Unable to load pages.", "OK");
			}

			IsBusy = false;
			LoadEventPagesCommand.ChangeCanExecute ();
		}
        

        private async Task<IEnumerable<EventPage>> LoadEventPages ()
		{
			var pages = await _eventPageLoader.Load ();
			Console.WriteLine ("EventPages received:" + pages.Count);
			var filteredPages = pages
                .OrderBy (x => x.Modified);
			return filteredPages;
		}
    }
}
