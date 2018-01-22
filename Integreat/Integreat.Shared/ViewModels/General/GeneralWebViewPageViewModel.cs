using System;
using Integreat.Shared.Services;

namespace Integreat.Shared.ViewModels
{
    /// <summary>
    /// ViewModel for GeneralWebViewPage, which is a Page with a simple WebView that can display either a URL or a HTML string directly.
    /// </summary>
    public class GeneralWebViewPageViewModel : BaseWebViewViewModel
    {
        private string _source;

        /// <summary>
        /// Initializes a new instance of the <see cref="GeneralWebViewPageViewModel" /> class.
        /// </summary>
        /// <param name="navigator">The navigator.</param>
        /// <param name="source">The source for the webView, which can either be a URL or HTML.</param>
        /// <param name="pdfWebViewFactory">The PDF web view factory.</param>
        /// <param name="imagePageFactory">The image page factory.</param>
        /// <param name="mainContentPageViewModel"></param>
        public GeneralWebViewPageViewModel(INavigator navigator,
            Func<string, ImagePageViewModel> imagePageFactory,
            Func<string, PdfWebViewPageViewModel> pdfWebViewFactory, string source, MainContentPageViewModel mainContentPageViewModel) :
            base(navigator, imagePageFactory, pdfWebViewFactory, mainContentPageViewModel)
        {
            Source = source;
        }

        #region Properties
        /// <summary>
        /// Gets or sets the source for the WebView, which can either be a URL or HTML, indicated by <c>SourceIsHtml</c>.
        /// </summary>
        public string Source
        {
            get => _source;
            set => SetProperty(ref _source, value);
        }
        #endregion
    }
}
