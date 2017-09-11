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

            // resolve the control as the pdfWebView
            if (e.NewElement != null)
            {
                var pdfWebView = Element as PdfWebView;
                if (pdfWebView == null) return;


                // if the target is an online pdf, use the google docs pdf viewer (there is also a online version of PDF.js, however it does not easily support cross-domain urls)
                if (pdfWebView.Uri.StartsWith("http"))
                {
                    var target = string.Format("https://docs.google.com/gview?embedded=true&url={0}", WebUtility.UrlEncode(pdfWebView.Uri));
                    Control.LoadUrl(target);
                }
                else
                {
                    // otherwise (local pdf) use the local pdf viewer (PDF.js) instead
                    Control.Settings.AllowUniversalAccessFromFileURLs = true;
                    Control.LoadUrl(string.Format("file:///android_asset/web/viewer.html?file={0}", WebUtility.UrlEncode(pdfWebView.Uri)));
                }
            }
        }
    }
}