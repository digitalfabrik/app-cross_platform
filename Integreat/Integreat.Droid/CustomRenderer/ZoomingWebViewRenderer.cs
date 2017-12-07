using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

namespace Integreat.Droid.CustomRenderer
{
    /// <summary>
    /// add zooming functionality to WebViewRenderer
    /// </summary>
    public class ZoomingWebViewRenderer : WebViewRenderer
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