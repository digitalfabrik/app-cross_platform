using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.V7.App;
using Xamarin.Forms.Platform.Android;

//based on http://codeworks.it/blog/?p=294
namespace Integreat.Droid
{
    [Activity(Label= "Integreat",Icon="@mipmap/icon", Theme = "@style/SplashTheme", MainLauncher = true)] //Doesn't place it in back stack
    // ReSharper disable once UnusedMember.Global
    public class SplashActivity : FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            //System.Threading.Thread.Sleep(2000); //Let's wait awhile...
            var intent = new Intent(this, typeof(MainActivity));
            FirebasePushNotificationManager.ProcessIntent(this, Intent);
            StartActivity(intent);
            Finish();
        }
    }
}