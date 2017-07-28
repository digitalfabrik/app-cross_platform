using System;
using System.Collections.Generic;
using System.Text;
using Integreat.Shared.Services.Tracking;
using Integreat.Shared.Utilities;

namespace Integreat.Shared.ViewModels.Resdesign.General
{
    /// <summary>
    /// ViewModel for GeneralWebViewPage, which is a Page with a simple WebView that can display either a URL or a HTML string directly.
    /// </summary>
    public class GeneralWebViewPageViewModel : BaseViewModel {
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

        public bool IsHtmlRawView => Preferences.GetHtmlRawViewSetting();
        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="GeneralWebViewPageViewModel"/> class.
        /// </summary>
        /// <param name="analyticsService">The analytics service.</param>
        /// <param name="source">The source for the webView, which can either be a URL or HTML.</param>
        public GeneralWebViewPageViewModel(IAnalyticsService analyticsService, string source) : base(analyticsService)
        {
            Source = source;
        }
    }
}
