using System;
using Android.Content;
using Android.Webkit;
using AWebView = Android.Webkit.WebView;
using System.Threading.Tasks;
using Android.Graphics;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Integreat.Droid.CustomRenderer;

[assembly: ExportRenderer(typeof(Xamarin.Forms.WebView), typeof(GeneralWebViewRenderer))]
namespace Integreat.Droid.CustomRenderer
{
    /// <summary>
    /// Custom render for WebViews with zoom possibility
    /// </summary>
    /// <seealso cref="Xamarin.Forms.Platform.Android.WebViewRenderer" />
    public class GeneralWebViewRenderer : WebViewRenderer
    {
        IWebViewController ElementController => Element;

        public GeneralWebViewRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.WebView> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {
                Control.SetWebViewClient(new WebClient(this));
            }
        }

        class WebClient : WebViewClient
        {
            GeneralWebViewRenderer _renderer;

            public WebClient(GeneralWebViewRenderer renderer)
            {
                _renderer = renderer ?? throw new ArgumentNullException("renderer");
            }

            public override bool ShouldOverrideUrlLoading(AWebView view, string url)
            {
                if (_renderer.Element == null)
                    return true;

                var args = new WebNavigatingEventArgs(WebNavigationEvent.NewPage, new UrlWebViewSource { Url = url }, url);

                _renderer.ElementController.SendNavigating(args);

                _renderer.UpdateCanGoBackForward();
                return args.Cancel;
            }

            public override void OnPageFinished(AWebView view, string url)
            {
                base.OnPageFinished(view, url);

                var source = new UrlWebViewSource { Url = url };
                var args = new WebNavigatedEventArgs(WebNavigationEvent.NewPage, source, url, WebNavigationResult.Success);
                _renderer.ElementController.SendNavigated(args);
            }

            public override void OnPageStarted(AWebView view, string url, Bitmap favicon)
            {
                base.OnPageStarted(view, url, favicon);

                var args = new WebNavigatingEventArgs(WebNavigationEvent.NewPage, new UrlWebViewSource { Url = url }, url);
                _renderer.ElementController.SendNavigating(args);
            }
        }
    }
}