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
using Page = Integreat.Models.Page;

namespace Integreat.Shared.Views
{
    public partial class InformationOverviewPage
    {
        public ListView ListView => pageListView;
        public PageLoader PageLoader;

        public InformationOverviewPage()
        {
            InitializeComponent();
            using (AppContainer.Container.BeginLifetimeScope())
            {
                var network = AppContainer.Container.Resolve<INetworkService>();
                var persistence = AppContainer.Container.Resolve<PersistenceService>();
                //TODO remove hardcoded data
                var language = new Language { ShortName = "de" };
                var location = new Location { Path = "/wordpress/augsburg/" };
                PageLoader = new PageLoader(language, location, persistence, network);
            }
            LoadPages();
        }

        private async void LoadPages()
        {
            Console.WriteLine("Load called");
            var pages = await PageLoader.Load();
            Console.WriteLine("Pages received:" + pages.Count);
            pageListView.ItemsSource = ConvertPagesToListItems(pages);
        }

        private IEnumerable<PageListItem> ConvertPagesToListItems(IEnumerable<Page> result)
        {
            Console.WriteLine("ConvertPagesToListItems called");
            return result.Select(page => new PageListItem
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
            ((ListView) sender).SelectedItem = null; // de-select the row
        }

        public class PageListItem
        {
            public string Title { get; set; }

            public string Description { get; set; }

            public DateTime Date { get; set; }

            public int Id { get; set; }
        }
    }
}
