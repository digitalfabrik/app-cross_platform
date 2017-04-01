using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Integreat.Shared.Utilities;
using Integreat.Shared.ViewModels.Resdesign;
using Xamarin.Forms;

namespace Integreat.Shared.Pages.Redesign {
    public partial class ContentContainerPage : TabbedPage {
        private ContentContainerViewModel _vm;


        public ContentContainerPage() {
            InitializeComponent();
            BindingContextChanged += OnBindingContextChanged;
            //  CurrentPageChanged += OnCurrentPageChanged;
            Appearing += OnAppearing;
        }

        private void OnAppearing(object sender, EventArgs eventArgs) {
            if (_vm == null) return;
            var locationId = Preferences.Location();
            if (locationId < 0 || Preferences.Language(locationId).IsNullOrEmpty()) {
                // not language / location selected
                _vm.OpenLocationSelection();
                return;
            }

            _vm.CreateMainView(Children, ToolbarItems, Application.Current.MainPage as NavigationPage);
            CurrentPage = Children[1];
            // we don't want this to build twice, so we remove the event listener
            Appearing -= OnAppearing;
            BindingContextChanged -= OnBindingContextChanged;
        }

        /*   private void OnCurrentPageChanged(object sender, EventArgs eventArgs)
           {
               var asPage = sender as ContentContainerPage;
               var contentAsNavigationPage = asPage?.CurrentPage as NavigationPage;
               if (contentAsNavigationPage == null) return;
               ForceLayout();
               NavigationPage.SetHasBackButton(Application.Current.MainPage, true);
           }*/

        private async void OnBindingContextChanged(object sender, EventArgs eventArgs) {
            var vm = BindingContext as ContentContainerViewModel;
            if (vm == null) return;
            _vm = vm;
            return;
            // check if there is a language and location selection saved



            ToolbarItems.Add(new ToolbarItem() { Text = "testse", Icon = "globe.png" });

            // todo
            await Task.Delay(2000);
            var navigationPage = new NavigationPage(new DetailPage() { Title = "Globo", BackgroundColor = Color.Red });
            navigationPage.Icon = "globe.png";
            navigationPage.Title = "Schedule";

            await navigationPage.PushAsync(new DetailPage() { Title = "erer", BackgroundColor = Color.Black });

            Children.Add(navigationPage);
            await Task.Delay(500);
            navigationPage = new NavigationPage(new DetailPage() { Title = "Search" });
            navigationPage.Icon = "search.png";
            navigationPage.Title = "search";
            Children.Add(navigationPage);

            await Task.Delay(500);

            navigationPage = new NavigationPage(new DetailPage() { Title = "asddasd" });
            navigationPage.Icon = "search.png";
            navigationPage.Title = "asdasd";
            Children.Add(navigationPage);
        }
    }
}
