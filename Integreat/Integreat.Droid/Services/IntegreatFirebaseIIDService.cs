using Android.App;
using Firebase.Iid;

namespace Integreat.Droid.Services
{
    [Service]
    [IntentFilter(new[] { "com.google.firebase.INSTANCE_ID_EVENT" })]
    public class IntegreatFirebaseIidService : FirebaseInstanceIdService
    {
        //ToDo is this done ?? this string is not used
        const string TAG = "IntegreatFirebaseIIDService";
        public override void OnTokenRefresh()
        {
            var refreshedToken = FirebaseInstanceId.Instance.Token;
            System.Diagnostics.Debug.WriteLine("Refreshed topic: " + refreshedToken);
            SendRegistrationToServer(refreshedToken);

        }

        private static void SendRegistrationToServer(string topic)
        {
            //send topic to cms (maybe)
            FirebasePushNotificationManager.SaveTopicToken(topic);
        }
    }
}
