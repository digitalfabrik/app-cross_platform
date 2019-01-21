using Android.Content;
using Integreat.Droid.CustomRenderer;
using Xamarin.Forms;

[assembly: ExportRenderer(typeof(WebView), typeof(GeneralWebViewRenderer))]
namespace Integreat.Droid.CustomRenderer
{
    /// <inheritdoc />
    /// <summary>
    /// Custom render for WebViews with zoom possibility
    /// </summary>
    /// <seealso cref="T:Xamarin.Forms.Platform.Android.WebViewRenderer" />
    public class GeneralWebViewRenderer : ZoomingWebViewRenderer
    {
        public GeneralWebViewRenderer(Context context) : base(context)
        {

        }
    }
}