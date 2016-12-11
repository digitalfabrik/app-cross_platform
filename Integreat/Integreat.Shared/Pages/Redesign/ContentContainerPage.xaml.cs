using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Integreat.Shared.ViewModels.Resdesign;
using Xamarin.Forms;

namespace Integreat.Shared.Pages.Redesign
{
	public partial class ContentContainerPage : TabbedPage {
		public ContentContainerPage ()
		{
			InitializeComponent ();
            BindingContextChanged += OnBindingContextChanged;
            CurrentPageChanged += OnCurrentPageChanged;
        }

	    private void OnCurrentPageChanged(object sender, EventArgs eventArgs)
	    {
	        
	    }

	    private async void OnBindingContextChanged(object sender, EventArgs eventArgs) {
            var vm = BindingContext as ContentContainerViewModel;
	        if (vm == null) return;
            // todo

	        await Task.Delay(2000);
            var navigationPage = new NavigationPage(new DetailPage() {Title = "Globo"});
            navigationPage.Icon = "globe.png";
            navigationPage.Title = "Schedule";
	        await navigationPage.PushAsync(new DetailPage() {Title = "erer"});
            
            Children.Add(navigationPage);
            await Task.Delay(500);
            navigationPage = new NavigationPage(new DetailPage() {Title = "Search"});
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
