using System;
using System.Threading.Tasks;
using Xamarin.Forms;

//based on http://adventuresinxamarinforms.com/2014/12/09/creating-a-xamarin-forms-app-part-9-working-with-alerts-and-dialogs/
namespace Integreat.Shared.Pages
{
    public class PageProxy : IPage
    {
        private readonly Func<Page> _pageResolver;

        public PageProxy(Func<Page> pageResolver)
        {
            _pageResolver = pageResolver;
        }

        public async Task DisplayAlert(string title, string message, string cancel)
        {
            await _pageResolver().DisplayAlert(title, message, cancel);
        }

        public async Task<bool> DisplayAlert(string title, string message, string accept, string cancel)
        {
            return await _pageResolver().DisplayAlert(title, message, accept, cancel);
        }

        public async Task<string> DisplayActionSheet(string title, string cancel, string destruction, params string[] buttons)
        {
            return await _pageResolver().DisplayActionSheet(title, cancel, destruction, buttons);
        }

        public INavigation Navigation
        {
            get
            {
                var pageResolver = _pageResolver();
                return pageResolver.Navigation;
            }
        }
    }
}
