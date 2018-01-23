using System;
using Integreat.Droid;
using Integreat.Shared.Firebase;
using Xamarin.Forms;

[assembly: Dependency(typeof(FirebasePushNotificationManager))]
namespace Integreat.Droid
{
    public class FirebasePushNotificationManager : IFirebasePushNotificationManager
    {
        public IPushNotificationHandler NotificationHandler { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public string Token => throw new NotImplementedException();

        public event FirebasePushNotificationTokenEventHandler OnTokenRefresh;
        public event FirebasePushNotificationResponseEventHandler OnNotificationOpened;
        public event FirebasePushNotificationDataEventHandler OnNotificationReceived;
        public event FirebasePushNotificationDataEventHandler OnNotificationDeleted;
        public event FirebasePushNotificationErrorEventHandler OnNotificationError;

        public void Subscribe(string[] topics)
        {
            throw new NotImplementedException();
        }

        public void Subscribe(string topic)
        {
            throw new NotImplementedException();
        }

        public void Unsubscribe(string[] topics)
        {
            throw new NotImplementedException();
        }

        public void Unsubscribe(string topic)
        {
            throw new NotImplementedException();
        }

        public void UnsubscribeAll()
        {
            throw new NotImplementedException();
        }
    }
}
