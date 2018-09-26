using Integreat.Shared.Data.Loader;
using Integreat.Shared.Utilities;
using Integreat.Shared.ViewModels;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

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

        public void OnError(string error) => Debug.WriteLine("Error receiving a message: " + error);

        public void OnOpened(NotificationResponse response)
        {
            Debug.WriteLine("Message opened");

            //Change location
            var lanCode = _firebaseHelper.ParamsToLanguage(response.Data);
            if (int.TryParse(_firebaseHelper.ParamsToCity(response.Data), out var cityId) && Preferences.Location() != cityId)
            {
                //get location and language
                var location = _dataLoaderProvider.LocationsDataLoader.Load(false).Result.First(l => l.Id == cityId);
                var language = _dataLoaderProvider.LanguagesDataLoader.Load(false, location).Result.First(l => l.ShortName == lanCode);

                if (location != null && language != null)
                {

                    //TODO work with current instance
                    Preferences.SetLocation(location);
                    Preferences.SetLanguage(location, language);
                }
            }

            _firebaseHelper.ShowNotificationAlert(response.Data);
        }

        public void OnReceived(IDictionary<string, object> parameters)
        {
            Debug.WriteLine("Message received");

            //show alert
            _firebaseHelper.ShowNotificationAlert(parameters);
        }
    }
}