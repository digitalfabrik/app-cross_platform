using Autofac;
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
                if (_instance != null) return _instance;

                _instance = DependencyService.Get<IFirebasePushNotificationManager>();
                //just use the container in this case!!!!!
                var pushNotificationHandler = IntegreatApp.Container.Resolve<IPushNotificationHandler>();
                _instance.NotificationHandler = pushNotificationHandler;
                return _instance;
            }
        }
    }
}
