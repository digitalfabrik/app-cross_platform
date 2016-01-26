using Integreat.Shared.Models;
using Xamarin.Forms;
using Integreat.Shared.ViewModels;
using Integreat.Shared.Views;

namespace Integreat.Shared.Pages
{
	public partial class EventsOverviewPage : ContentPage
	{
		private EventPagesViewModel ViewModel {
			get { return BindingContext as EventPagesViewModel; }
		}

		public EventsOverviewPage ()
		{
			InitializeComponent ();
			BindingContext = new EventPagesViewModel ();

			listView.ItemTapped += (sender, args) => {
				if (listView.SelectedItem == null) {
					return;
				}
				var page = listView.SelectedItem as EventPage;
				Navigation.PushAsync (new WebsiteView (page));
				listView.SelectedItem = null;
			};
		}


		protected override void OnAppearing ()
		{
			base.OnAppearing ();
			if (ViewModel == null || !ViewModel.CanLoadMore || ViewModel.IsBusy || ViewModel.EventPages.Count > 0) {
				return;
			}

			ViewModel.LoadEventPagesCommand.Execute (null);
		}
	}
}
