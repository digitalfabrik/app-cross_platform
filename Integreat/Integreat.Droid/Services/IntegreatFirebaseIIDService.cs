using System;
using Android.App;
using Firebase.Iid;

namespace Integreat.Droid.Services
{
    [Service]
    [IntentFilter(new[] { "com.google.firebase.INSTANCE_ID_EVENT" })]
    public class IntegreatFirebaseIIDService : FirebaseInstanceIdService
    {
        const string TAG = "IntegreatFirebaseIIDService";
        public override void OnTokenRefresh()
        {
            var refreshedToken = FirebaseInstanceId.Instance.Token;
            System.Diagnostics.Debug.WriteLine("Refreshed token: "+refreshedToken);
            SendRegistrationToServer(refreshedToken);

        }

        private void SendRegistrationToServer(string token){
            
        }
    }
}
