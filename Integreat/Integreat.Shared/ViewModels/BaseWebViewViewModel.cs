using System;
using System.Linq;
using System.Net;
using Integreat.Shared.Services;
using Integreat.Shared.Services.Tracking;
using Integreat.Shared.Utilities;
using Integreat.Shared.ViewModels.Resdesign;
using Integreat.Shared.ViewModels.Resdesign.General;
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


        protected BaseWebViewViewModel(IAnalyticsService analyticsService, INavigator navigator, Func<string, ImagePageViewModel> imagePageFactory, Func<string, PdfWebViewPageViewModel> pdfWebViewFactory) : base(analyticsService)
        {
            _navigator = navigator;
            _imagePageFactory = imagePageFactory;
            _pdfWebViewFactory = pdfWebViewFactory;
        }

        public bool IsHtmlRawView => Preferences.GetHtmlRawViewSetting();

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
            if ((eventArgs.Url.Contains(Constants.IntegreatReleaseUrl) ||
                eventArgs.Url.Contains(Constants.IntegreatReleaseFallbackUrl))
                && !(eventArgs.Url.EndsWith(".pdf")|| eventArgs.Url.EndsWith(".png")|| eventArgs.Url.EndsWith(".jpg")))
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
