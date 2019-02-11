using Android.App;
using Android.Widget;
using Integreat.Droid;
using Integreat.Shared.Utilities;

[assembly: Xamarin.Forms.Dependency(typeof(MessageAndroid))]
namespace Integreat.Droid
{
    public class MessageAndroid : IMessage
    {
        public void LongAlert(string message)
        {
            Toast.MakeText(Application.Context, message, ToastLength.Long).Show();
        }

        public void ShortAlert(string message)
        {
            Toast.MakeText(Application.Context, message, ToastLength.Short).Show();
        }
    }
}
