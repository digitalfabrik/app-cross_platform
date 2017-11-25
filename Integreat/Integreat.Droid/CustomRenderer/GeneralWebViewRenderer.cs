using Xamarin.Forms.Platform.Android;
using Integreat.Droid.CustomRenderer;
using Xamarin.Forms;
using System.ComponentModel;

[assembly: ExportRenderer(typeof(WebView), typeof(GeneralWebViewRenderer))]
namespace Integreat.Droid.CustomRenderer
{
    /// <summary>
    /// Custom render for WebViews with zoom possibility
    /// </summary>
    /// <seealso cref="Xamarin.Forms.Platform.Android.WebViewRenderer" />
#pragma warning disable 618
    public class GeneralWebViewRenderer : WebViewRenderer
#pragma warning restore 618
    {
        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            //check if source is external
            if (Control != null && ((WebView)sender).Source is UrlWebViewSource)
            {
                //enable Android zoom
                Control.Settings.BuiltInZoomControls = true;
                Control.Settings.DisplayZoomControls = false;
            }
            base.OnElementPropertyChanged(sender, e);
        }
    }
}