using System.Security;
using Integreat.Shared.ViewModels;
using Integreat.Utilities;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Integreat.Shared.Pages.General
{
    /// <inheritdoc />
    /// <summary>
    /// This pages displays all normal html pages with normal content
    /// </summary>
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GeneralWebViewPage
    {
        [SecurityCritical]
        public GeneralWebViewPage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Handles the OnNavigating event of the WebView control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="WebNavigatingEventArgs"/> instance containing the event data.</param>
        private void WebView_OnNavigating(object sender, WebNavigatingEventArgs e)
        {
            // try to give the OnNavigating event the ViewModel of this WebPage
#if __ANDROID__
            ((BaseWebViewViewModel)BindingContext)?.OnNavigating(e);
#elif __IOS__
            //ToDo something seems be be wired here.. I get error that BindingContext not exists in this context
            ((BaseWebViewViewModel)BindingContext)?.OnNavigating(e);
#endif

        }

        /// <summary>
        /// Handles the OnNavigated event of the WebView control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="WebNavigatedEventArgs"/> instance containing the event data.</param>
        private void WebView_OnNavigated(object sender, WebNavigatedEventArgs e)
        {
            //ihk-Lehrstellenboerse workaround

            //check if url is "invalid"
            if (e.Url.Contains(Constants.IhkLehrstellenBoerseUrl + "/?location"))
            {
                IsVisible = false;
                //change to valid url
                ((UrlWebViewSource)((WebView)sender).Source).Url = ((GeneralWebViewPageViewModel)BindingContext).Source;
            }

            if (!IsVisible && !e.Url.Contains(Constants.IhkLehrstellenBoerseUrl + "/?location") &&
                e.Url.Contains(Constants.IhkLehrstellenBoerseUrl))
            {
                IsVisible = true;
            }
        }
    }
}
