using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Xamarin.Forms.Platform.Android;
using Integreat.Droid.CustomRenderer;
using Xamarin.Forms;
using System.ComponentModel;
using Integreat.Shared.Pages.Redesign.General;

[assembly: ExportRenderer(typeof(WebView), typeof(GeneralWebViewRenderer))]
namespace Integreat.Droid.CustomRenderer
{
    class GeneralWebViewRenderer:WebViewRenderer
    {
        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            //check if source is external
            if(Control != null && ((Xamarin.Forms.WebView)sender).Source is Xamarin.Forms.UrlWebViewSource)
            {
                //enable Android zoom
                Control.Settings.BuiltInZoomControls = true;
                Control.Settings.DisplayZoomControls = false;
            }
            base.OnElementPropertyChanged(sender, e);
        }
    }
}