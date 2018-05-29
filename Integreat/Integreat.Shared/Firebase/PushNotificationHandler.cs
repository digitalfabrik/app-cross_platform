using System.Collections.Generic;
using System.Diagnostics;
using Integreat.Shared.Data.Loader;
using Xamarin.Forms;

namespace Integreat.Shared.Firebase
{
    public class PushNotificationHandler : IPushNotificationHandler
	{

		private readonly FirebaseHelper _firebaseHelper;
		private readonly DataLoaderProvider _dataLoaderProvider;

		public PushNotificationHandler(FirebaseHelper firebaseHelper, DataLoaderProvider dataLoaderProvider)
        {
			_firebaseHelper = firebaseHelper;
			_dataLoaderProvider = dataLoaderProvider;
        }

        public void OnError(string error)
        {
            Debug.WriteLine("Error receiving a message: " + error);
        }

        public void OnOpened(NotificationResponse response)
        {
            Debug.WriteLine("Message opened");
            Device.BeginInvokeOnMainThread(() =>
            {
                IntegreatApp.Current.MainPage.DisplayAlert("opened", "opened", "Cancel");
            });
        }

        public void OnReceived(IDictionary<string, object> parameters)
        {
            Debug.WriteLine("Message received");

			//save Eventpage
			var ePage = _firebaseHelper.ParamsToEventPage(parameters);
			_dataLoaderProvider.PushNotificationsDataLoader.Add(ePage);
        }

        private void ShowNotificationAlert(IDictionary<string , object> parameters)
        {
			var title = _firebaseHelper.ParamsToTitle(parameters);
			var body = _firebaseHelper.ParamsToBody(parameters);

			if (!title.IsNullOrEmpty() && !body.IsNullOrEmpty())
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    IntegreatApp.Current.MainPage.DisplayAlert(title, body, "Cancel");
                });
            }
        }
    }
}
