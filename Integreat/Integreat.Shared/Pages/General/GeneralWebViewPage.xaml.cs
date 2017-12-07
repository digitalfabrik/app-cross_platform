using System.Security;
using Integreat.Shared.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Integreat.Shared.Pages.General
{
    /// <inheritdoc />
    /// <summary>
    /// This pages displays all normal html pages with normal content
    /// </summary>
    [XamlCompilation(XamlCompilationOptions.Compile)]
    [SecurityCritical]
    public partial class GeneralWebViewPage
    {
        /// <summary>
        /// Gets or sets the on navigating command, which will be executed when the user navigates within the webView (clicks on a link).
        /// </summary>
        // ReSharper disable once UnusedAutoPropertyAccessor.Local
        private Command OnNavigatingCommand { get; set; }

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
            OnNavigatingCommand?.Execute(e);
            // try to give the OnNavigating event the ViewModel of this WebPage
            ((BaseWebViewViewModel)BindingContext)?.OnNavigating(e);
        }
    }
}
