using System;
using System.Threading.Tasks;
using Xamarin.Forms;

//based on http://adventuresinxamarinforms.com/2014/12/09/creating-a-xamarin-forms-app-part-9-working-with-alerts-and-dialogs/
namespace Integreat.Shared.Pages
{
    /// <summary>
    /// Proxy class used to abstract the root Page of the Application via a pageResolver method. (Created in IntegreatModule)
    /// </summary>
    public class PageProxy : IPage
    {
        public readonly Func<Page> PageResolver;

        public PageProxy(Func<Page> pageResolver)
        {
            PageResolver = pageResolver;
        }

        public async Task DisplayAlert(string title, string message, string cancel)
        {
            await PageResolver().DisplayAlert(title, message, cancel);
        }

        public async Task<bool> DisplayAlert(string title, string message, string accept, string cancel)
        {
            return await PageResolver().DisplayAlert(title, message, accept, cancel);
        }

        public async Task<string> DisplayActionSheet(string title, string cancel, string destruction, params string[] buttons)
        {
            return await PageResolver().DisplayActionSheet(title, cancel, destruction, buttons);
        }

        public INavigation Navigation
        {
            get
            {
                var pageResolver = PageResolver();
                return pageResolver.Navigation;
            }
        }
    }
}
