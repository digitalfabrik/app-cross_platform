using Android.App;
using Firebase.Iid;

namespace Integreat.Droid.Services
{
    [Service]
    [IntentFilter(new[] { "com.google.firebase.INSTANCE_ID_EVENT" })]
    public class IntegreatFirebaseIIDService : FirebaseInstanceIdService
    {
        //ToDo is this done ?? this string is not used
        const string TAG = "IntegreatFirebaseIIDService";
        public override void OnTokenRefresh()
        {
            var refreshedToken = FirebaseInstanceId.Instance.Token;
            System.Diagnostics.Debug.WriteLine("Refreshed token: "+refreshedToken);
            SendRegistrationToServer(refreshedToken);

        }

        private void SendRegistrationToServer(string token){
            //send token to cms (maybe)
            FirebasePushNotificationManager.SaveToken(token);
        }
    }
}
