using System;
using System.Collections.Generic;
using Integreat.Shared.ViewModels;
using Xamarin.Forms;

namespace Integreat.Shared.Pages.General
{
    public partial class GeneralContentPage
    {
        public GeneralContentPage()
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
            ((BaseWebViewViewModel)BindingContext)?.OnNavigating(e);
        }
    }
}
