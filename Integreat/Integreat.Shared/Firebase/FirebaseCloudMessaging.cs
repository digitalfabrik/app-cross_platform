using System;
using Xamarin.Forms;

namespace Integreat.Shared.Firebase
{
    public class FirebaseCloudMessaging
    {
        private static IFirebasePushNotificationManager _instance;
        
        public static IFirebasePushNotificationManager Current
        {
            get
            {
                if (_instance == null)
                    _instance = DependencyService.Get<IFirebasePushNotificationManager>();

                return _instance;
            }
        }
    }
}
