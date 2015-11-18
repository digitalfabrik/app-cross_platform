using System;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

namespace Integreat.Droid
{
    [Activity(Label = "Integreat.Droid", Icon = "@drawable/icon", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsApplicationActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);


            App.ScreenBounds = new Xamarin.Forms.Rectangle(0, 0, (int)((int)Resources.DisplayMetrics.WidthPixels / Resources.DisplayMetrics.Density), ((int)((int)Resources.DisplayMetrics.HeightPixels / Resources.DisplayMetrics.Density) - 72));
            App.ContentBounds = new Xamarin.Forms.Rectangle(0, 0, (int)((int)Resources.DisplayMetrics.WidthPixels / Resources.DisplayMetrics.Density), ((int)((int)Resources.DisplayMetrics.HeightPixels / Resources.DisplayMetrics.Density) - 72));


            global::Xamarin.Forms.Forms.Init(this, bundle);

            LoadApplication(new App());
        }
    }
}

