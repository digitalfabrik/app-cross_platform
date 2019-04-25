using Integreat.Shared.Services;
using Integreat.Shared.Utilities;
using Integreat.Utilities;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
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


        protected BaseWebViewViewModel(INavigator navigator, Func<string, ImagePageViewModel> imagePageFactory,
            Func<string, PdfWebViewPageViewModel> pdfWebViewFactory, MainContentPageViewModel mainContentPageViewModel)
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
            var urlAligned = eventArgs.Url.ToLower();
            // check if it's a mail or telephone address
            if (urlAligned.StartsWith("mailto") || urlAligned.StartsWith("tel"))
            {
                // if so, open it on the device and cancel the webRequest
                Device.OpenUri(new Uri(eventArgs.Url));
                eventArgs.Cancel = true;
            }

            if (Device.RuntimePlatform == Device.Android
                && (urlAligned.Contains(".pdf")
                    || urlAligned.Contains("_pdf")))
            {
                await ShowPdfPage(eventArgs);
            }
            if (urlAligned.EndsWith(".jpg") || urlAligned.EndsWith(".png"))
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
            if (urlAligned.Contains(Constants.IntegreatReleaseUrl)|| 
                urlAligned.Contains(Constants.IntegreatUrl2) || 
                urlAligned.Contains(Constants.IntegreatUrl3))
            {
                if (urlAligned.Contains(Constants.IntegreatUrl2))
                {
                    //replace with cms.
                    urlAligned = urlAligned.Replace(Constants.IntegreatUrl2, Constants.IntegreatReleaseUrl);
                }
                else if (urlAligned.Contains(Constants.IntegreatUrl3))
                {
                    //replace with cms.
                    urlAligned = urlAligned.Replace(Constants.IntegreatUrl3, Constants.IntegreatReleaseUrl);
                }

                //add backslash if not there
                if (!urlAligned.EndsWith('/'))
                    urlAligned += '/';

                // if so, open the corresponding page instead
                // search page which has a permalink that matches
                var pageViewModel = _mainContentPageViewModel.LoadedPages.FirstOrDefault(x =>
                    x.Page.Url != null && x.Page.Url == urlAligned);
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
