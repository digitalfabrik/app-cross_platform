using System;
using System.Collections.Generic;
using System.Diagnostics;
using Xamarin.Forms;

namespace Integreat.Shared.Views
{
    public partial class InformationOverviewPage : ContentPage
    {
        public ListView ListView => pageListView;

        public InformationOverviewPage()
        {
            InitializeComponent();

            var pageItems = new List<PageListItem>
            {
                new PageListItem
                {
                    Title = "Title 1",
                    Description = "Description 1",
                    Date = DateTime.Now
                },
                new PageListItem
                {
                    Title = "Title 2",
                    Description = "Description 2",
                    Date = DateTime.Now
                },
                new PageListItem
                {
                    Title = "Title 3",
                    Description = "Description 3",
                    Date = DateTime.Now
                }
            };

            pageListView.ItemsSource = pageItems;
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
        }
    }
}
