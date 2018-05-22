using System;
using Xamarin.Forms;

namespace Integreat.Shared.Firebase
{
    public class FirebaseCloudMessaging
    {

		private PushNotificationHandler _pushNotificationHandler;

		public FirebaseCloudMessaging(PushNotificationHandler pushNotificationHandler)
		{
			_pushNotificationHandler = pushNotificationHandler;
		}

        private IFirebasePushNotificationManager _instance;
        
        public IFirebasePushNotificationManager Current
        {
            get
            {
                if (_instance == null)
                {
                    _instance = DependencyService.Get<IFirebasePushNotificationManager>();
					_instance.NotificationHandler = _pushNotificationHandler;
                }
                return _instance;
            }
        }
    }
}
