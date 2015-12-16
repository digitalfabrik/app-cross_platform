using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Integreat.ApplicationObject;
using Integreat.Models;
using Integreat.Services;
using Integreat.Shared.Services.Loader;
using Integreat.Shared.Services.Persistance;
using Xamarin.Forms;

namespace Integreat.Shared.ViewModels
{
    public class EventPagesViewModel : BaseViewModel
    {
        public ObservableCollection<EventPage> EventPages { get; set; }
        public EventPageLoader EventPageLoader;

        private EventPage _selectedPage;
        public EventPage SelectedPage
        {
            get { return _selectedPage; }
            set
            {
                _selectedPage = value;
                ExecuteLoadPagesCommand();
            }
        }

        public EventPagesViewModel()
        {
            Title = "Events";
            Icon = null;
            EventPages = new ObservableCollection<EventPage>();
            using (AppContainer.Container.BeginLifetimeScope())
            {
                var network = AppContainer.Container.Resolve<INetworkService>();
                var persistence = AppContainer.Container.Resolve<PersistenceService>();
                //TODO remove hardcoded data
                var language = new Language { ShortName = "de" };
                var location = new Location { Path = "/wordpress/augsburg/" };
                EventPageLoader = new EventPageLoader(language, location, persistence, network);
            }
        }

        private Command _loadEventPagesCommand;
        public Command LoadEventPagesCommand
        {
            get
            {
                return _loadEventPagesCommand ??
                  (_loadEventPagesCommand = new Command(async () =>
                  {
                      await ExecuteLoadPagesCommand();
                  }, () => !IsBusy));
            }
        }

        public async Task ExecuteLoadPagesCommand()
        {
            if (IsBusy)
            {
                return;
            }

            IsBusy = true;
            LoadEventPagesCommand.ChangeCanExecute();
            var error = false;
            try
            {
                EventPages.Clear();
                var loadedPages = await LoadEventPages();
                foreach (var page in loadedPages)
                {
                    EventPages.Add(page);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                error = true;
            }

            if (error)
            {
                var page = new ContentPage();
                await page.DisplayAlert("Error", "Unable to load pages.", "OK");
            }

            IsBusy = false;
            LoadEventPagesCommand.ChangeCanExecute();
        }

        private async Task<IEnumerable<EventPage>> LoadEventPages()
        {
            var pages = await EventPageLoader.Load();
            Console.WriteLine("EventPages received:" + pages.Count);
            var filteredPages = pages
                .OrderBy(x => x.Modified);
            return filteredPages;
        }
    }
}
