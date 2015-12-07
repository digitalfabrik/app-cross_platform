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
        }

        public void SetPages(IEnumerable<Page> pages)
        {
            ListView.ItemsSource = ConvertPagesToListItems(pages);
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
