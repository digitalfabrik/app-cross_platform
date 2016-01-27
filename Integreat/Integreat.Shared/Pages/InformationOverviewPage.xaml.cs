using Integreat.Shared.ViewModels;
using Integreat.Shared.Views;
using System;
using Xamarin.Forms;
using Page = Integreat.Shared.Models.Page;

namespace Integreat.Shared.Pages
{
	public partial class InformationOverviewPage : ContentPage
    {
        public PagesViewModel ViewModel {
			get { return BindingContext as PagesViewModel; }
		}

		public InformationOverviewPage ()
		{
			InitializeComponent ();
			BindingContext = new PagesViewModel (Navigation, this);
            
			listView.ItemTapped += (sender, args) => {
				if (listView.SelectedItem == null) { 
					return;
				}
				var page = listView.SelectedItem as Page;
				Navigation.PushAsync (new WebsiteView (page));
				listView.SelectedItem = null;
			};
        }

    }
}
