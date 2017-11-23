using Integreat.Shared.Services.Tracking;

namespace Integreat.Shared.ViewModels.General
{
    /// <summary>
    /// ViewModel for GeneralWebViewPage, which is a Page with a simple WebView that can display either a URL or a HTML string directly.
    /// </summary>
    public class PdfWebViewPageViewModel : BaseViewModel
    {
        private string _uri;

        /// <summary>
        /// Initializes a new instance of the <see cref="PdfWebViewPageViewModel"/> class.
        /// </summary>
        /// <param name="analyticsService">The analytics service.</param>
        /// <param name="uri">The for the pdf file.</param>
        public PdfWebViewPageViewModel(IAnalyticsService analyticsService, string uri) : base(analyticsService)
        {
            Uri = uri;
        }

        /// <summary>
        /// Gets or sets the uri for the WebView, which can either be a URL or HTML, indicated by <c>SourceIsHtml</c>.
        /// </summary>
        public string Uri
        {
            get => _uri;
            set => SetProperty(ref _uri, value);
        }
    }
}
