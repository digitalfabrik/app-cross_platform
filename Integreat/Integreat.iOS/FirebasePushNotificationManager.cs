using System;
using Firebase.CloudMessaging;
using Firebase.Core;
using UIKit;
using UserNotifications;

namespace Integreat.iOS
{
    public class FirebasePushNotificationManager : IUNUserNotificationCenterDelegate, IMessagingDelegate
    {
        private bool _isConnected = false;

        public IntPtr Handle => throw new NotImplementedException();

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

        private void Connect(){
            Messaging.SharedInstance.ShouldEstablishDirectChannel = true;
            _isConnected = true;
        }

        private void Disconnect(){
            Messaging.SharedInstance.ShouldEstablishDirectChannel = false;
            _isConnected = false;
        }
    }
}
