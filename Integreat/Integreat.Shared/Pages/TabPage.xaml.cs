using Integreat.Shared.ViewModels;
using Xamarin.Forms;

namespace Integreat.Shared.Pages
{
	public partial class TabPage : TabbedPage
	{
		public TabPage()
		{
			InitializeComponent ();
		}

	    protected override void OnAppearing()
	    {
	        base.OnAppearing();
            var vm = BindingContext as BaseViewModel;
            vm?.OnAppearingCommand.Execute(null);
        }
	}
}
