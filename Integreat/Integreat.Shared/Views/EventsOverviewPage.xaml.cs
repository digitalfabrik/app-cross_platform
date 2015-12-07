using System;
using System.Collections.Generic;
using System.Linq;
using Autofac;
using Integreat.ApplicationObject;
using Integreat.Models;
using Integreat.Services;
using Integreat.Shared.Services.Loader;
using Integreat.Shared.Services.Persistance;
using Xamarin.Forms;

namespace Integreat.Shared.Views
{
	public partial class EventsOverviewPage
    {
        public ListView ListView => eventPageListView;
        public EventPageLoader EventPageLoader;

        public EventsOverviewPage()
        {
            InitializeComponent();
            using (AppContainer.Container.BeginLifetimeScope())
            {
                var network = AppContainer.Container.Resolve<INetworkService>();
                var persistence = AppContainer.Container.Resolve<PersistenceService>();
                //TODO remove hardcoded data
                var language = new Language { ShortName = "de" };
                var location = new Location { Path = "/wordpress/augsburg/" };
                EventPageLoader = new EventPageLoader(language, location, persistence, network);
            }
            LoadEventPages();
        }

        private async void LoadEventPages()
        {
            Console.WriteLine("Load called");
            var eventPages = await EventPageLoader.Load();
            Console.WriteLine("EventPages received:" + eventPages.Count);
            eventPageListView.ItemsSource = ConvertEventPagesToListItems(eventPages);
        }

        private IEnumerable<EventPageListItem> ConvertEventPagesToListItems(IEnumerable<EventPage> result)
        {
            Console.WriteLine("ConvertEventPagesToListItems called");
            return result.Select(page => new EventPageListItem
            {
                Title = page.Title,
                Description = page.Description,
                Date = page.Modified,
                Id = page.PrimaryKey
            }).ToList();
        }

        public void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem == null)
            {
                return; // has been set to null, do not 'process' tapped event
            }
            DisplayAlert("Tapped", e.SelectedItem + " row was tapped", "OK");
            ((ListView)sender).SelectedItem = null; // de-select the row
        }

        public class EventPageListItem
        {
            public string Title { get; set; }

            public string Description { get; set; }

            public DateTime Date { get; set; }

            public int Id { get; set; }
        }
    }
}
