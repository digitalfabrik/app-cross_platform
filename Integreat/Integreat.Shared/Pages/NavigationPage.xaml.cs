using System.ComponentModel;
using Xamarin.Forms;

namespace Integreat.Shared.Pages
{
	public partial class NavigationPage : ContentPage
	{
		public NavigationPage ()
		{
			InitializeComponent ();
		    ListViewMenu.Header = BindingContext;
            PropertyChanged += delegate(object sender, PropertyChangedEventArgs args)
            {
                if (args.PropertyName.Equals("BindingContext"))
                {
                    ListViewMenu.Header = BindingContext;
                }
            };
        }
    }
}
