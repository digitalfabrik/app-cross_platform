using System.Collections.Generic;

using Xamarin.Forms;

namespace Integreat.Shared.Views
{
	public partial class EventsOverviewPage : ContentPage
    {
        public ListView ListView => eventListView;

        public EventsOverviewPage ()
		{
			InitializeComponent ();

            var pageItems = new List<EventPageListItem>
            {
                new EventPageListItem
                {
                    Title = "Title 1",
                    Description = "Description 1"
                },
                new EventPageListItem
                {
                    Title = "Title 2",
                    Description = "Description 2"
                },
                new EventPageListItem
                {
                    Title = "Title 3",
                    Description = "Description 3"
                }
            };

            eventListView.ItemsSource = pageItems;
        }


        public class EventPageListItem
        {
            public string Title { get; set; }

            public string Description { get; set; }
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
    }
}
