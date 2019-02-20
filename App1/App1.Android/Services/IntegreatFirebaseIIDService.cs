using Android.App;
using App1.Droid.Firebase;
using Firebase.Iid;

namespace App1.Droid.Services
{
    [Service]
    [IntentFilter(new[] { "com.google.firebase.INSTANCE_ID_EVENT" })]
    public class IntegreatFirebaseIidService : FirebaseInstanceIdService
    {
        public override void OnTokenRefresh()
        {
            var refreshedToken = FirebaseInstanceId.Instance.Token;
            System.Diagnostics.Debug.WriteLine("Refreshed token: " + refreshedToken);
            SendRegistrationToServer(refreshedToken);
        }

        private static void SendRegistrationToServer(string token)
        {
            //send topic to cms (maybe)
            FirebasePushNotificationManager.SaveToken(token);
        }
    }
}
