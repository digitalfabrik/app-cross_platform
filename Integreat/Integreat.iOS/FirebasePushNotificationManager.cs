using System;
using Firebase.CloudMessaging;
using Firebase.Core;
using Foundation;
using Integreat.iOS;
using Integreat.Shared.Firebase;
using UIKit;
using UserNotifications;

[assembly: Xamarin.Forms.Dependency(typeof(FirebasePushNotificationManager))]
namespace Integreat.iOS
{
    public class FirebasePushNotificationManager : IUNUserNotificationCenterDelegate, IMessagingDelegate, IFirebasePushNotificationManager
    {
        private static bool _isConnected = false;
        private string _FirebaseTokenKey = "FirebaseToken";
        public string Token { get { return string.IsNullOrEmpty(Messaging.SharedInstance.FcmToken) ? (NSUserDefaults.StandardUserDefaults.StringForKey(_FirebaseTokenKey) ?? string.Empty) : Messaging.SharedInstance.FcmToken; }}

        static FirebasePushNotificationTokenEventHandler _onTokenRefresh;
        public event FirebasePushNotificationTokenEventHandler OnTokenRefresh
        {
            add
            {
                _onTokenRefresh += value;
            }
            remove
            {
                _onTokenRefresh -= value;
            }
        }

        static FirebasePushNotificationResponseEventHandler _onNotificationOpened;
        public event FirebasePushNotificationResponseEventHandler OnNotificationOpened
        {
            add
            {
                _onNotificationOpened += value;
            }
            remove
            {
                _onNotificationOpened -= value;
            }
        }

        static FirebasePushNotificationDataEventHandler _onNotificationReceived;
        public event FirebasePushNotificationDataEventHandler OnNotificationReceived
        {
            add
            {
                _onNotificationReceived += value;    
            }
            remove
            {
                _onNotificationReceived -= value;
            }
        }

        static FirebasePushNotificationDataEventHandler _onNotificationDeleted;
        public event FirebasePushNotificationDataEventHandler OnNotificationDeleted
        {
            add
            {
                _onNotificationDeleted += value;
            }
            remove
            {
                _onNotificationDeleted -= value;
            }
        }

        static FirebasePushNotificationErrorEventHandler _onNotificationError;
        public event FirebasePushNotificationErrorEventHandler OnNotificationError
        {
            add
            {
                _onNotificationError += value;
            }
            remove
            {
                _onNotificationError -= value;
            }
        }

        public IntPtr Handle => throw new NotImplementedException();

        public IPushNotificationHandler NotificationHandler { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public void DidRefreshRegistrationToken(Messaging messaging, string fcmToken)
        {
            var refreshedToken = fcmToken;
            if(!string.IsNullOrEmpty(refreshedToken))
            {
                _onTokenRefresh?.Invoke(FirebaseCloudMessaging.Current, new FirebasePushNotificationTokenEventArgs(refreshedToken));
                Connect();
            }

            NSUserDefaults.StandardUserDefaults.SetString(fcmToken, _FirebaseTokenKey);
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public static void Initialize(){
            // Register your app for remote notifications.
            if (UIDevice.CurrentDevice.CheckSystemVersion(10, 0))
            {
                // iOS 10 or later
                var authOptions = UNAuthorizationOptions.Alert | UNAuthorizationOptions.Badge | UNAuthorizationOptions.Sound;
                UNUserNotificationCenter.Current.RequestAuthorization(authOptions, (granted, error) => {
                    Console.WriteLine(granted);
                });

                // For iOS 10 display notification (sent via APNS)
                UNUserNotificationCenter.Current.Delegate = FirebaseCloudMessaging.Current as IUNUserNotificationCenterDelegate;

                // For iOS 10 data message (sent via FCM)
                Messaging.SharedInstance.Delegate = FirebaseCloudMessaging.Current as IMessagingDelegate;
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

        public static void Connect(){
            Messaging.SharedInstance.ShouldEstablishDirectChannel = true;
            _isConnected = true;
        }

        public static void Disconnect(){
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
