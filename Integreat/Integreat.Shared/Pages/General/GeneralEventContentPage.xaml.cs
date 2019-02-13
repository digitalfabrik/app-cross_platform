using Integreat.Shared.ViewModels;
using Xamarin.Forms;

namespace Integreat.Shared.Pages.General
{
    public partial class GeneralEventContentPage
    {
        public GeneralEventContentPage()
        {
            InitializeComponent();
        }

        private void WebView_OnNavigating(object sender, WebNavigatingEventArgs e)
        {
            // try to give the OnNavigating event the ViewModel of this WebPage
            ((BaseWebViewViewModel)BindingContext)?.OnNavigating(e);
        }
    }
}
