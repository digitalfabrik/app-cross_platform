using System;
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
                if (_instance == null)
                {
					_instance = DependencyService.Get<IFirebasePushNotificationManager>();
                    //just use the container in this case!!!!!
					IPushNotificationHandler _pushNotificationHandler = IntegreatApp.container.Resolve<IPushNotificationHandler>();
					_instance.NotificationHandler = _pushNotificationHandler;
                }
                return _instance;
            }
        }
    }
}
