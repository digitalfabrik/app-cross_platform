using Integreat.Droid.CustomRenderer;
using Xamarin.Forms;

[assembly: ExportRenderer(typeof(WebView), typeof(GeneralWebViewRenderer))]
namespace Integreat.Droid.CustomRenderer
{
    /// <summary>
    /// Custom render for WebViews with zoom possibility
    /// </summary>
    /// <seealso cref="Xamarin.Forms.Platform.Android.WebViewRenderer" />
    public class GeneralWebViewRenderer : ZoomingWebViewRenderer
    {
       
    }
}