using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Integreat.Shared.Firebase
{
    public class PushNotificationHandler:IPushNotificationHandler
    {
        private readonly string _titleKey;
        private readonly string _messageKey;

        public PushNotificationHandler()
        {
            if(Device.RuntimePlatform.Equals(Device.Android))
            {
                _titleKey = "title";
                _messageKey = "body";
            }else if(Device.RuntimePlatform.Equals(Device.iOS))
            {
                _titleKey = "aps.alert.title";
                _messageKey = "aps.alert.body";
            }
        }

        public void OnError(string error)
        {
            Debug.WriteLine("Error receiving a message: " + error);
        }

        public void OnOpened()
        {
            Debug.WriteLine("Message opened");
        }

        public void OnReceived(IDictionary<string, object> parameters)
        {
            Debug.WriteLine("Message received");

            if(parameters.TryGetValue(_messageKey, out object body) && parameters.TryGetValue(_titleKey, out object title))
            {
                Device.BeginInvokeOnMainThread(() => 
                {
                    IntegreatApp.Current.MainPage.DisplayAlert(title.ToString(), body.ToString(), "Cancel"); 
                });
            }
        }
    }
}
