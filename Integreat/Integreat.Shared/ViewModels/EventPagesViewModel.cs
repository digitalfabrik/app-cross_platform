using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Integreat.ApplicationObject;
using Integreat.Shared.Models;
using Integreat.Services;
using Integreat.Shared.Services.Loader;
using Integreat.Shared.Services.Persistance;
using Xamarin.Forms;
using Integreat.Shared.Pages;

namespace Integreat.Shared.ViewModels
{
	public class EventPagesViewModel : BaseViewModel
	{
		public ObservableCollection<EventPage> EventPages { get; set; }

		public EventPageLoader EventPageLoader;

		private EventPage _selectedPage;

        private Xamarin.Forms.Page page;
        private INavigation navigation;

        public EventPage SelectedPage {
			get { return _selectedPage; }
			set {
				_selectedPage = value;
				ExecuteLoadPagesCommand ();
			}
		}

		public EventPagesViewModel(INavigation navigation, Xamarin.Forms.Page page) //TODO page should not be included, but currently needed for dialog
        {
			Title = "Events";
			Icon = null;
			EventPages = new ObservableCollection<EventPage> ();
			using (AppContainer.Container.BeginLifetimeScope ()) {
				var network = AppContainer.Container.Resolve<INetworkService> ();
				var persistence = AppContainer.Container.Resolve<PersistenceService> ();
				//TODO remove hardcoded data
				var language = new Language { ShortName = "de" };
				var location = new Location { Path = "/wordpress/augsburg/" };
				EventPageLoader = new EventPageLoader (language, location, persistence, network);
			}


            if (navigation == null)
            {
                throw new ArgumentNullException("navigation");
            }
            this.navigation = navigation;

            if (page == null)
            {
                throw new ArgumentNullException("page");
            }
            this.page = page;
        }

		private Command _loadEventPagesCommand;

		public Command LoadEventPagesCommand {
			get {
				return _loadEventPagesCommand ??
				(_loadEventPagesCommand = new Command (async () => {
					await ExecuteLoadPagesCommand ();
				}, () => !IsBusy));
			}
		}

		public async Task ExecuteLoadPagesCommand ()
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
					EventPages.Add (page);
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

        private async void onChangeLanguageClicked()
        {
            var action = await page.DisplayActionSheet("ActionSheet: Send to?", "Cancel", null, "Email", "Twitter", "Facebook");
        }

        void onSearchClicked()
        {
            var search = new PageSearchList(EventPages);
            navigation.PushAsync(search);
        }

        private async Task<IEnumerable<EventPage>> LoadEventPages ()
		{
			var pages = await EventPageLoader.Load ();
			Console.WriteLine ("EventPages received:" + pages.Count);
			var filteredPages = pages
                .OrderBy (x => x.Modified);
			return filteredPages;
		}

        private Command _openSearchCommand;

        public Command OpenSearchCommand
        {
            get
            {
                return _openSearchCommand ??
                (_openSearchCommand = new Command(() => {
                    onSearchClicked();
                }));
            }
        }

        private Command _changeLanguageCommand;

        public Command ChangeLanguageCommand
        {
            get
            {
                return _changeLanguageCommand ??
                (_changeLanguageCommand = new Command(() => {
                    onChangeLanguageClicked();
                }));
            }
        }
    }
}
