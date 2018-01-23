using System;
using Firebase.CloudMessaging;
using Firebase.Core;
using Foundation;
using Integreat.Shared.Firebase;
using UIKit;
using UserNotifications;

namespace Integreat.iOS
{
    public class FirebasePushNotificationManager : IUNUserNotificationCenterDelegate, IMessagingDelegate, IFirebasePushNotificationManager
    {
        private static FirebasePushNotificationManager _instance;
        private bool _isConnected = false;

        public event FirebasePushNotificationTokenEventHandler OnTokenRefresh;
        public event FirebasePushNotificationResponseEventHandler OnNotificationOpened;
        public event FirebasePushNotificationDataEventHandler OnNotificationReceived;
        public event FirebasePushNotificationDataEventHandler OnNotificationDeleted;
        public event FirebasePushNotificationErrorEventHandler OnNotificationError;

        public IntPtr Handle => throw new NotImplementedException();

        public static FirebasePushNotificationManager Current
        {
            get
            {
                if (_instance == null)
                    _instance = new FirebasePushNotificationManager();

                return _instance; 
            }
        }

        public IPushNotificationHandler NotificationHandler { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public string Token => throw new NotImplementedException();

        public void DidRefreshRegistrationToken(Messaging messaging, string fcmToken)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public void Initialize(){
            // Register your app for remote notifications.
            if (UIDevice.CurrentDevice.CheckSystemVersion(10, 0))
            {
                // iOS 10 or later
                var authOptions = UNAuthorizationOptions.Alert | UNAuthorizationOptions.Badge | UNAuthorizationOptions.Sound;
                UNUserNotificationCenter.Current.RequestAuthorization(authOptions, (granted, error) => {
                    Console.WriteLine(granted);
                });

                // For iOS 10 display notification (sent via APNS)
                UNUserNotificationCenter.Current.Delegate = this;

                // For iOS 10 data message (sent via FCM)
                Messaging.SharedInstance.Delegate = this;
            }
            else
            {
                // iOS 9 or before
                var allNotificationTypes = UIUserNotificationType.Alert | UIUserNotificationType.Badge | UIUserNotificationType.Sound;
                var settings = UIUserNotificationSettings.GetSettingsForTypes(allNotificationTypes, null);
                UIApplication.SharedApplication.RegisterUserNotificationSettings(settings);
            }

            UIApplication.SharedApplication.RegisterForRemoteNotifications();

            App.Configure();
        }

        // To receive notifications in foreground on iOS 10 devices.
        [Export("userNotificationCenter:willPresentNotification:withCompletionHandler:")]
        public void WillPresentNotification(UNUserNotificationCenter center, UNNotification notification, Action<UNNotificationPresentationOptions> completionHandler)
        {
            // Do your magic to handle the notification data
            System.Console.WriteLine(notification.Request.Content.UserInfo);
        }

        // Receive data message on iOS 10 devices.
        public void ApplicationReceivedRemoteMessage(RemoteMessage remoteMessage)
        {
            Console.WriteLine(remoteMessage.AppData);
        }

        public void Connect(){
            Messaging.SharedInstance.ShouldEstablishDirectChannel = true;
            _isConnected = true;
        }

        public void Disconnect(){
            Messaging.SharedInstance.ShouldEstablishDirectChannel = false;
            _isConnected = false;
        }

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
