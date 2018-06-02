using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Integreat.Shared.Data.Loader;
using Integreat.Shared.Models;
using Integreat.Shared.Utilities;
using Integreat.Shared.ViewModels;
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

			//Change location
			int cityId = 0;
			string lanCode = _firebaseHelper.ParamsToLanguage(response.Data);
			if (int.TryParse(_firebaseHelper.ParamsToCity(response.Data), out cityId))
			{
				if (Preferences.Location() != cityId)
				{
					//get location and language
                    var location = (_dataLoaderProvider.LocationsDataLoader.Load(false).Result).Where((Location l) => l.Id == cityId)?.First();
					var language = (_dataLoaderProvider.LanguagesDataLoader.Load(false, location).Result).Where(l => l.ShortName == lanCode)?.First();

                    if(location !=null && language !=null)
					{
						Preferences.SetLocation(location);
                        ContentContainerViewModel.Current.ChangeLocation(location);

                        Preferences.SetLanguage(location, language);

						ContentContainerViewModel.Current.RefreshAll(true);
					}               
				}
			}

			_firebaseHelper.ShowNotificationAlert(response.Data);
        }

        public void OnReceived(IDictionary<string, object> parameters)
        {
			Debug.WriteLine("Message received");

            //show 
			_firebaseHelper.ShowNotificationAlert(parameters);
        }
    }
}
