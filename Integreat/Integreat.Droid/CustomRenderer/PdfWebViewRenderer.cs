using System.Net;
using Integreat.Droid.CustomRenderer;
using Integreat.Shared.CustomRenderer;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(PdfWebView), typeof(PdfWebViewRenderer))]
namespace Integreat.Droid.CustomRenderer
{
    class PdfWebViewRenderer : WebViewRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<WebView> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement != null)
            {
                var pdfWebView = Element as PdfWebView;
                if (pdfWebView != null)
                {
                    Control.Settings.AllowUniversalAccessFromFileURLs = true;
                    Control.LoadUrl(string.Format("file:///android_asset/web/viewer.html?file={0}", WebUtility.UrlEncode(pdfWebView.Uri)));
                }
            }
        }
    }
}