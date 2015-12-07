using System;
using Xamarin.Forms;

namespace Integreat.Shared.Views
{
	public partial class RootPage
	{
		public RootPage ()
		{
			InitializeComponent ();
            MainMenu.ListView.ItemSelected += OnItemSelected;
        }

	    private void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var item = e.SelectedItem as MainMenu.MasterPageItems;
            if (item == null)
            {
                return;
            }
	        Detail = new NavigationPage((Page)Activator.CreateInstance(item.TargetType)); ;
            MainMenu.ListView.SelectedItem = null;
            IsPresented = false;
        }
    }
}
