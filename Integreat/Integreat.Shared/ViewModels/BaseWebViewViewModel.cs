using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
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
        private readonly MainContentPageViewModel _mainContentPageViewModel;


        protected BaseWebViewViewModel(INavigator navigator, Func<string, ImagePageViewModel> imagePageFactory, Func<string, PdfWebViewPageViewModel> pdfWebViewFactory, MainContentPageViewModel mainContentPageViewModel)
        {
            _navigator = navigator;
            _imagePageFactory = imagePageFactory;
            _pdfWebViewFactory = pdfWebViewFactory;
            _mainContentPageViewModel = mainContentPageViewModel;
        }

        /// <summary> Gets a value indicating whether this instance is HTML raw view. </summary>
        /// <value> <c>true</c> if this instance is HTML raw view; otherwise, <c>false</c>. </value>
        public bool IsHtmlRawView => Preferences.GetHtmlRawViewSetting();

        /// <summary>
        /// Raises the <see cref="E:Navigating" /> event.
        /// </summary>
        /// <param name="eventArgs">The <see cref="WebNavigatingEventArgs"/> instance containing the event data.</param>
        public async Task OnNavigating(WebNavigatingEventArgs eventArgs)
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
                await ShowPdfPage(eventArgs);
            }
            if (eventArgs.Url.ToLower().EndsWith(".jpg") || eventArgs.Url.ToLower().EndsWith(".png"))
            {
                switch (Device.RuntimePlatform)
                {
                    case Device.Android:
                        await ShowImagePageForAndroid(eventArgs);
                        break;
                    case Device.iOS:
                        await ShowImagePageForIOs(eventArgs);
                        break;
                    default:
                        return;
                }
            }
            // check if the URL is a page URL
            if (eventArgs.Url.Contains(Constants.IntegreatReleaseUrl) ||
                eventArgs.Url.Contains(Constants.IntegreatReleaseFallbackUrl))
            {
                // if so, open the corresponding page instead
                // search page which has a permalink that matches
                var pageViewModel = _mainContentPageViewModel.LoadedPages.FirstOrDefault(x =>
                    x.Page.Permalinks != null && x.Page.Permalinks.AllUrls.Contains(eventArgs.Url));
                // if we have found a corresponding page, cancel the web navigation and open it in the app instead
                if (pageViewModel == null) return;

                // cancel the original navigating event
                eventArgs.Cancel = true;
                // and instead act as like the user tapped on the page
                _mainContentPageViewModel.OnPageTapped(pageViewModel);
            }
        }

        private async Task ShowImagePageForIOs(WebNavigatingEventArgs eventArgs)
        {
            var view = _imagePageFactory(eventArgs.Url);
            await GetTitleAndNavigate(eventArgs, view);
        }

        private async Task ShowImagePageForAndroid(WebNavigatingEventArgs eventArgs)
        {
            var view = _imagePageFactory(eventArgs.Url.ToLower().StartsWith("http")
                ? eventArgs.Url : eventArgs.Url.Replace("android_asset/", ""));
            await GetTitleAndNavigate(eventArgs, view);
        }

        private async Task GetTitleAndNavigate(WebNavigatingEventArgs eventArgs, ImagePageViewModel view)
        {
            view.Title = WebUtility.UrlDecode(eventArgs.Url).Split('/').Last().Split('.').First();
            eventArgs.Cancel = true;
            // push a new general webView page, which will show the URL of the image
            await _navigator.PushAsync(view, Navigation);
        }

        private async Task ShowPdfPage(WebNavigatingEventArgs eventArgs)
        {
            var view = _pdfWebViewFactory(eventArgs.Url.ToLower().StartsWith("http")
                ? eventArgs.Url
                : eventArgs.Url.Replace("android_asset/", ""));
            view.Title = WebUtility.UrlDecode(eventArgs.Url).Split('/').Last().Split('.').First();
            eventArgs.Cancel = true;
            // push a new general webView page, which will show the URL of the offer
            await _navigator.PushAsync(view, Navigation);
        }
    }
}
