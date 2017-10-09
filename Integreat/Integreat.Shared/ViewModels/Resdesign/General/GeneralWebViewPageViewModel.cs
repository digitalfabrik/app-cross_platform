using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using Integreat.Shared.Services;
using Integreat.Shared.Services.Tracking;
using Integreat.Shared.Utilities;
using Integreat.Utilities;
using Xamarin.Forms;

namespace Integreat.Shared.ViewModels.Resdesign.General
{
    /// <summary>
    /// ViewModel for GeneralWebViewPage, which is a Page with a simple WebView that can display either a URL or a HTML string directly.
    /// </summary>
    public class GeneralWebViewPageViewModel : BaseViewModel {
        #region Fields
        private string _source;
        private Func<string, ImagePageViewModel> _imagePageFactory;
        private Func<string, PdfWebViewPageViewModel> _pdfWebViewFactory;
        private INavigator _navigator;

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
        /// Initializes a new instance of the <see cref="GeneralWebViewPageViewModel" /> class.
        /// </summary>
        /// <param name="analyticsService">The analytics service.</param>
        /// <param name="navigator">The navigator.</param>
        /// <param name="source">The source for the webView, which can either be a URL or HTML.</param>
        /// <param name="pdfWebViewFactory">The PDF web view factory.</param>
        /// <param name="imagePageFactory">The image page factory.</param>
        public GeneralWebViewPageViewModel(IAnalyticsService analyticsService, INavigator navigator, string source
            , Func<string, PdfWebViewPageViewModel> pdfWebViewFactory, Func<string, ImagePageViewModel> imagePageFactory) : base(analyticsService)
        {
            Source = source;
            _navigator = navigator;
            _pdfWebViewFactory = pdfWebViewFactory;
            _imagePageFactory = imagePageFactory;
        }

        public async void OnNavigating(WebNavigatingEventArgs eventArgs)
        {
            // CA2140 violation - transparent method accessing a critical type.  This can be fixed by any of:
            //  1. Make TransparentMethod critical
            //  2. Make TransparentMethod safe critical
            //  3. Make CriticalClass safe critical
            //  4. Make CriticalClass transparent       
            //  Warning CA2140  Transparent method 'MainContentPageViewModel.OnNavigating(object)' references security
            //  critical type 'WebNavigatingEventArgs'.In order for this reference to be allowed under the security 
            //  transparency rules, either 'MainContentPageViewModel.OnNavigating(object)' must become security critical 
            //  or safe - critical, or 'WebNavigatingEventArgs' become security safe - critical or 
            //  transparent.
            
            // check if the URL is a page URL
            if (eventArgs.Url.Contains(Constants.IntegreatReleaseUrl))
            {
                // if so, open the corresponding page instead

                // search page which has a permalink that matches
                var page = MainContentPageViewModel.Current.LoadedPages.FirstOrDefault(x =>
                    x.Page.Permalinks != null && x.Page.Permalinks.AllUrls.Contains(eventArgs.Url));
                // if we have found a corresponding page, cancel the web navigation and open it in the app instead
                if (page == null) return;

                // cancel the original navigating event
                eventArgs.Cancel = true;
                // and instead act as like the user tapped on the page
                MainContentPageViewModel.Current.OnPageTapped(page);
            }

            // check if it's a mail or telephone address
            if (eventArgs.Url.StartsWith("mailto") || eventArgs.Url.StartsWith("tel"))
            {
                // if so, open it on the device and cancel the webRequest
                Device.OpenUri(new Uri(eventArgs.Url));
                eventArgs.Cancel = true;
            }

            if (eventArgs.Url.Contains(".pdf") && Device.RuntimePlatform == Device.Android)
            {
                var view = _pdfWebViewFactory(eventArgs.Url.StartsWith("http")
                    ? eventArgs.Url
                    : eventArgs.Url.Replace("android_asset/", ""));
                view.Title = WebUtility.UrlDecode(eventArgs.Url).Split('/').Last().Split('.').First();
                eventArgs.Cancel = true;
                // push a new general webView page, which will show the URL of the offer
                await _navigator.PushAsync(view, Navigation);
            }
            if (eventArgs.Url.EndsWith(".jpg") || eventArgs.Url.EndsWith(".png"))
            {
                ImagePageViewModel view;
                if (Device.RuntimePlatform == Device.Android)
                {
                    view = _imagePageFactory(eventArgs.Url.StartsWith("http")
                        ? eventArgs.Url
                        : eventArgs.Url.Replace("android_asset/", ""));
                }
                else if (Device.RuntimePlatform == Device.iOS)
                {
                    view = _imagePageFactory(eventArgs.Url);
                }
                else
                {
                    return;
                }
                view.Title = WebUtility.UrlDecode(eventArgs.Url).Split('/').Last().Split('.').First();
                eventArgs.Cancel = true;
                // push a new general webView page, which will show the URL of the image
                await _navigator.PushAsync(view, Navigation);
            }
        }
    }
}
