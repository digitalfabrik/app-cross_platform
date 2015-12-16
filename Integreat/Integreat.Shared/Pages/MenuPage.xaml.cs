using Xamarin.Forms;
using Integreat.Shared.ViewModels;
using Integreat.Shared.Models;

namespace Integreat.Shared.Pages
{
    public partial class MenuPage : ContentPage
    {
        readonly RootPage _root;

        public MenuPageViewModel ViewModel
        {
            get { return BindingContext as MenuPageViewModel; }
        }

        public MenuPage(RootPage root)
        {
            InitializeComponent();
            _root = root;
            BindingContext = ListViewMenu.Header =  new MenuPageViewModel();

            ListViewMenu.ItemSelected += async (sender, e) =>
            {
                if (ListViewMenu.SelectedItem == null)
                {
                    return;
                }

                await _root.NavigateAsync(((HomeMenuItem) e.SelectedItem).PageId);
            };
        }

    }
}
