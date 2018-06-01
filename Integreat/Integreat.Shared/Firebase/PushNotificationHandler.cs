using System.Collections.Generic;
using System.Diagnostics;
using Integreat.Shared.Data.Loader;
using Xamarin.Forms;

namespace Integreat.Shared.Firebase
{
    public class PushNotificationHandler : IPushNotificationHandler
	{

		private readonly FirebaseHelper _firebaseHelper;

		public PushNotificationHandler(FirebaseHelper firebaseHelper)
        {
			_firebaseHelper = firebaseHelper;
        }

        public void OnError(string error)
        {
            Debug.WriteLine("Error receiving a message: " + error);
        }

        public void OnOpened(NotificationResponse response)
        {
            Debug.WriteLine("Message opened");
        }

        public void OnReceived(IDictionary<string, object> parameters)
        {
			Debug.WriteLine("Message received");
        }
    }
}
