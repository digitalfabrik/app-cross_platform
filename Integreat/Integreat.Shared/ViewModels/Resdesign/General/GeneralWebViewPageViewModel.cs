using System;
using System.Collections.Generic;
using System.Text;
using Integreat.Shared.Services.Tracking;

namespace Integreat.Shared.ViewModels.Resdesign.General
{
    /// <summary>
    /// ViewModel for GeneralWebViewPage, which is a Page with a simple WebView that can display either a URL or a HTML string directly.
    /// </summary>
    public class GeneralWebViewPageViewModel : BaseViewModel {
        #region Fields

        private bool _sourceIsHtml;
        private string _source;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the source for the WebView, which can either be a URL or HTML, indicated by <c>SourceIsHtml</c>.
        /// </summary>
        public string Source
        {
            get { return _source; }
            set { SetProperty(ref _source, value); }
        }


        /// <summary>
        /// Gets or sets a value indicating whether the [source is HTML] or not (URL).
        /// </summary>
        /// <value>
        ///   <c>true</c> if [source is HTML]; otherwise, <c>false</c>.
        /// </value>
        public bool SourceIsHtml
        {
            get { return _sourceIsHtml; }
            set
            {
                SetProperty(ref _sourceIsHtml, value);
                OnPropertyChanged(nameof(SourceIsHtml));
            }
        }

        public bool SourceIsNotHtml => !SourceIsHtml;


        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="GeneralWebViewPageViewModel"/> class.
        /// </summary>
        /// <param name="analyticsService">The analytics service.</param>
        /// <param name="source">The source for the webView, which can either be a URL or HTML.</param>
        /// <param name="sourceIsHtml">if set to <c>true</c> the [source will be treated as HTML].</param>
        public GeneralWebViewPageViewModel(IAnalyticsService analyticsService, string source, bool sourceIsHtml) : base(analyticsService)
        {
            Source = source;
            SourceIsHtml = sourceIsHtml;

        }
    }
}
