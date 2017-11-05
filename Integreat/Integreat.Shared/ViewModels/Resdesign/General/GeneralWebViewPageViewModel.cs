using System;
using Integreat.Shared.Services;
using Integreat.Shared.Services.Tracking;

namespace Integreat.Shared.ViewModels.Resdesign.General
{
    /// <summary>
    /// ViewModel for GeneralWebViewPage, which is a Page with a simple WebView that can display either a URL or a HTML string directly.
    /// </summary>
    public class GeneralWebViewPageViewModel : BaseWebViewViewModel {
        #region Fields
        private string _source;

        #endregion

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

        /// <summary>
        /// Initializes a new instance of the <see cref="GeneralWebViewPageViewModel" /> class.
        /// </summary>
        /// <param name="analyticsService">The analytics service.</param>
        /// <param name="navigator">The navigator.</param>
        /// <param name="source">The source for the webView, which can either be a URL or HTML.</param>
        /// <param name="pdfWebViewFactory">The PDF web view factory.</param>
        /// <param name="imagePageFactory">The image page factory.</param>
        public GeneralWebViewPageViewModel(IAnalyticsService analyticsService, INavigator navigator,
            Func<string, ImagePageViewModel> imagePageFactory,
            Func<string, PdfWebViewPageViewModel> pdfWebViewFactory, string source) : 
            base(analyticsService, navigator, imagePageFactory, pdfWebViewFactory)
        {
            Source = source;
        }
    }
}
