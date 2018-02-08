using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Integreat.Shared.Firebase
{
    public class PushNotificationHandler:IPushNotificationHandler
    {
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
        }
    }
}
