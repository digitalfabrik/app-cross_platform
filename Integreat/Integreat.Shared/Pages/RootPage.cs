using System.Collections.Generic;
using Integreat.Shared.Services.Loader;
using Integreat.Shared.ViewModels;
using System.Threading.Tasks;
using Integreat.Shared.Controls;
using Xamarin.Forms;

namespace Integreat.Shared.Pages
{
    public class RootPage : MasterDetailPage
    {
        public PageLoader PageLoader;
        Dictionary<int, NavigationPage> Pages { get; set; }

        public RootPage()
        {
            Pages = new Dictionary<int, NavigationPage>();
            Master = new MenuPage(this);
            BindingContext = new BaseViewModel
            {
                Title = "Integreat",
                Icon = null
            };
            //setup home page
            NavigateAsync(-1);
        }

        public async Task NavigateAsync(int pageId)
        {
            if (!Pages.ContainsKey(pageId))
            {
                Pages.Add(pageId, new MyNavigationPage(new OverviewPage(pageId)));
            }

            var navigationPage = Pages[pageId];
            if (navigationPage == null)
            {
                return;
            }

            //pop to root for Windows Phone
            if (Detail != null && Device.OS == TargetPlatform.WinPhone)
            {
                await Detail.Navigation.PopToRootAsync();
            }

            Detail = navigationPage;

            if (Device.Idiom != TargetIdiom.Tablet)
            {
                IsPresented = false;
            }
        }
    }

}
