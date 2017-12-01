using System;
using System.Linq;
using System.Net;
using Integreat.Shared.Services;
using Integreat.Shared.Utilities;
using Integreat.Shared.ViewModels.General;
using Integreat.Utilities;
using Xamarin.Forms;

namespace Integreat.Shared.ViewModels
{
    /// <summary>
    /// This ViewModel is the BaseClass for all WebViews with shared functionality
    /// </summary>
    public abstract class BaseWebViewViewModel : BaseViewModel
    {
        private readonly INavigator _navigator;
        private readonly Func<string, ImagePageViewModel> _imagePageFactory;
        private readonly Func<string, PdfWebViewPageViewModel> _pdfWebViewFactory;


        protected BaseWebViewViewModel(INavigator navigator, Func<string, ImagePageViewModel> imagePageFactory, Func<string, PdfWebViewPageViewModel> pdfWebViewFactory)
        {
            _navigator = navigator;
            _imagePageFactory = imagePageFactory;
            _pdfWebViewFactory = pdfWebViewFactory;
        }

        /// <summary> Gets a value indicating whether this instance is HTML raw view. </summary>
        /// <value> <c>true</c> if this instance is HTML raw view; otherwise, <c>false</c>. </value>
        public bool IsHtmlRawView => Preferences.GetHtmlRawViewSetting();

        /// <summary>
        /// Raises the <see cref="E:Navigating" /> event.
        /// </summary>
        /// <param name="eventArgs">The <see cref="WebNavigatingEventArgs"/> instance containing the event data.</param>
        public async void OnNavigating(WebNavigatingEventArgs eventArgs)
        {
            // check if it's a mail or telephone address
            if (eventArgs.Url.ToLower().StartsWith("mailto") || eventArgs.Url.ToLower().StartsWith("tel"))
            {
                // if so, open it on the device and cancel the webRequest
                Device.OpenUri(new Uri(eventArgs.Url));
                eventArgs.Cancel = true;
            }

            if (Device.RuntimePlatform == Device.Android && (eventArgs.Url.ToLower().Contains(".pdf") || eventArgs.Url.ToLower().Contains("_pdf")))
            {
                var view = _pdfWebViewFactory(eventArgs.Url.ToLower().StartsWith("http")
                    ? eventArgs.Url
                    : eventArgs.Url.Replace("android_asset/", ""));
                view.Title = WebUtility.UrlDecode(eventArgs.Url).Split('/').Last().Split('.').First();
                eventArgs.Cancel = true;
                // push a new general webView page, which will show the URL of the offer
                await _navigator.PushAsync(view, Navigation);
            }
            if (eventArgs.Url.ToLower().EndsWith(".jpg") || eventArgs.Url.ToLower().EndsWith(".png"))
            {
                ImagePageViewModel view;
                if (Device.RuntimePlatform == Device.Android)
                {
                    view = _imagePageFactory(eventArgs.Url.ToLower().StartsWith("http")
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
            // check if the URL is a page URL
            if (eventArgs.Url.Contains(Constants.IntegreatReleaseUrl) ||
                eventArgs.Url.Contains(Constants.IntegreatReleaseFallbackUrl))
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
        }
    }
}
