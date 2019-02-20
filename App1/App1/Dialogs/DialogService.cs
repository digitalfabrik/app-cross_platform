using System.Threading.Tasks;

// based on http://adventuresinxamarinforms.com/2014/12/09/creating-a-xamarin-forms-app-part-9-working-with-alerts-and-dialogs/
namespace App1.Dialogs
{
    public class DialogService : IDialogProvider
    {
        private readonly IPage _page;

        public DialogService(IPage page)
        {
            _page = page;
        }

        public Task DisplayAlert(string title, string message, string cancel)
        {
            return _page.DisplayAlert(title, message, cancel);
        }

        public async Task<bool> DisplayAlert(string title, string message, string accept, string cancel)
        {
            return await _page.DisplayAlert(title, message, accept, cancel);
        }

        public async Task<string> DisplayActionSheet(string title, string cancel, string destruction, params string[] buttons)
        {
            return await _page.DisplayActionSheet(title, cancel, destruction, buttons);
        }
    }
}
