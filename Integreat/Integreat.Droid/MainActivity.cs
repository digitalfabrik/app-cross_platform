
using Android.App;
using Android.Content.PM;
using Android.OS;
using Autofac;
using Integreat.Shared;
using Integreat.Shared.Services.Persistence;
using SQLite.Net.Platform.XamarinAndroid;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

namespace Integreat.Droid
{
	[Activity (Label = "Integreat", Icon = "@drawable/icon", ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
	public class MainActivity : FormsAppCompatActivity
    {
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
            
            Forms.Init (this, bundle);

            ToolbarResource = Resource.Layout.toolbar;
            TabLayoutResource = Resource.Layout.tabs;

		    var cb = new ContainerBuilder();
            cb.Register(c => new PersistenceService(new SQLitePlatformAndroid())).As<PersistenceService>().SingleInstance();
            LoadApplication(new IntegreatApp(cb));
		}
        
    }
}

