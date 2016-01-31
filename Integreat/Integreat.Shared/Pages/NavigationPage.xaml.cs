using Xamarin.Forms;

namespace Integreat.Shared.Pages
{
	public partial class NavigationPage : ContentPage
	{
		public NavigationPage ()
		{
			InitializeComponent ();
		}
        /*readonly RootPage _root;
    public DisclaimerPresenter DisclaimerPresenter;

    public MenuPageViewModel ViewModel {
        get { return BindingContext as MenuPageViewModel; }
    }

    public MenuPage (RootPage root)
    {
        InitializeComponent ();
        _root = root;
        BindingContext = ListViewMenu.Header = new MenuPageViewModel ();

        ListViewMenu.ItemSelected += async (sender, e) => {
            if (ListViewMenu.SelectedItem == null) {
                return;
            }

            await _root.NavigateAsync (((HomeMenuItem)e.SelectedItem).PageId);
        };

        DisclaimerButton.Clicked += async (sender, e) => {
            System.Diagnostics.Debug.WriteLine ("Showing disclaimer");
            DisclaimerPresenter.Show ();
        };
    }*/
    }
}
