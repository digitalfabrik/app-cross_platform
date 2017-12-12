using Android.App;
using Android.OS;
using NUnit.Runner.Services;

namespace Integreat.Droid.Test
{
    [Activity(Label = "Integreat.Droid.Test", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsApplicationActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            Xamarin.Forms.Forms.Init(this, bundle);

            // This will load all tests within the current project
            var nunit = new NUnit.Runner.App
            {
                Options = new TestOptions
                {
                    AutoRun = true
                }
            };

            LoadApplication(nunit);
        }       
    }
}

