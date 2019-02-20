using System;
using System.Collections.Generic;
using App1.Firebase;
using App1.iOS;
using Firebase.CloudMessaging;
using Firebase.Core;
using Foundation;
using UIKit;
using UserNotifications;
using Xamarin.Forms;

[assembly: Dependency(typeof(FirebasePushNotificationManager))]
namespace App1.iOS
{
    public class FirebasePushNotificationManager : NSObject, IFirebasePushNotificationManager, IUNUserNotificationCenterDelegate, IMessagingDelegate
    {
        private static bool _isConnected;
        private static readonly Queue<Tuple<string, bool>> PendingTopics = new Queue<Tuple<string, bool>>();
        private static readonly NSString FirebaseTopicsKey = new NSString("FirebaseTopics");
        private static readonly NSMutableArray CurrentTopics = (NSUserDefaults.StandardUserDefaults.ValueForKey(FirebaseTopicsKey) as NSArray ?? new NSArray()).MutableCopy() as NSMutableArray;
        private const string FirebaseTokenKey = "FirebaseToken";
        public string Token => string.IsNullOrEmpty(Messaging.SharedInstance.FcmToken) ? (NSUserDefaults.StandardUserDefaults.StringForKey(FirebaseTokenKey) ?? string.Empty) : Messaging.SharedInstance.FcmToken;

        private static FirebasePushNotificationTokenEventHandler _onTokenRefresh;
        public event FirebasePushNotificationTokenEventHandler OnTokenRefresh
        {
            add => _onTokenRefresh += value;
            remove => _onTokenRefresh -= value;
        }

        private static FirebasePushNotificationResponseEventHandler _onNotificationOpened;
        public event FirebasePushNotificationResponseEventHandler OnNotificationOpened
        {
            add => _onNotificationOpened += value;
            remove => _onNotificationOpened -= value;
        }

        private static FirebasePushNotificationDataEventHandler _onNotificationReceived;
        public event FirebasePushNotificationDataEventHandler OnNotificationReceived
        {
            add => _onNotificationReceived += value;
            remove => _onNotificationReceived -= value;
        }

        private static FirebasePushNotificationDataEventHandler _onNotificationDeleted;
        public event FirebasePushNotificationDataEventHandler OnNotificationDeleted
        {
            add => _onNotificationDeleted += value;
            remove => _onNotificationDeleted -= value;
        }

        private static FirebasePushNotificationErrorEventHandler _onNotificationError;
        public event FirebasePushNotificationErrorEventHandler OnNotificationError
        {
            add => _onNotificationError += value;
            remove => _onNotificationError -= value;
        }

        public IPushNotificationHandler NotificationHandler { get; set; }

        public IEnumerable<string> SubscribedTopics
        {
            get
            {
                var topics = new List<string>();
                for (nuint i = 0; i < CurrentTopics.Count; i++)
                {
                    topics.Add(CurrentTopics.GetItem<NSString>(i));
                }

                return topics.ToArray();
            }
        }

        [Export("userNotificationCenter:didReceiveNotificationResponse:withCompletionHandler:")]
        public void DidReceiveNotificationResponse(UNUserNotificationCenter center, UNNotificationResponse response, Action completionHandler)
        {
            var parameters = GetParameters(response.Notification.Request.Content.UserInfo);

            if (!response.IsDismissAction)
            {
                var notificationResponse = new NotificationResponse(parameters, $"{response.ActionIdentifier}".Equals("com.apple.UNNotificationDefaultActionIdentifier", StringComparison.CurrentCultureIgnoreCase) ? string.Empty : $"{response.ActionIdentifier}");
                _onNotificationOpened?.Invoke(this, new FirebasePushNotificationResponseEventArgs(notificationResponse.Data, notificationResponse.Identifier));

                FirebaseCloudMessaging.Current.NotificationHandler?.OnOpened(notificationResponse);
            }

            completionHandler();
        }

        public void DidRefreshRegistrationToken(Messaging messaging, string fcmToken)
        {
            var refreshedToken = fcmToken;
            if (!string.IsNullOrEmpty(refreshedToken))
            {
                _onTokenRefresh?.Invoke(FirebaseCloudMessaging.Current, new FirebasePushNotificationTokenEventArgs(refreshedToken));
                Connect();
            }

            NSUserDefaults.StandardUserDefaults.SetString(fcmToken, FirebaseTokenKey);
        }

        public static void Initialize()
        {
            // Register your app for remote notifications.
            if (UIDevice.CurrentDevice.CheckSystemVersion(10, 0))
            {
                // iOS 10 or later
                const UNAuthorizationOptions authOptions = UNAuthorizationOptions.Alert | UNAuthorizationOptions.Badge | UNAuthorizationOptions.Sound;
                UNUserNotificationCenter.Current.RequestAuthorization(authOptions, (granted, error) =>
                {
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
            Console.WriteLine(notification.Request.Content.UserInfo);
            var parameters = GetParameters(notification.Request.Content.UserInfo);
            _onNotificationReceived?.Invoke(FirebaseCloudMessaging.Current, new FirebasePushNotificationDataEventArgs(parameters));
            FirebaseCloudMessaging.Current.NotificationHandler?.OnReceived(parameters);

            completionHandler(UNNotificationPresentationOptions.None);
        }

        public static void DidReceiveMessage(NSDictionary data)
        {
            Messaging.SharedInstance.AppDidReceiveMessage(data);
            var parameters = GetParameters(data);

            _onNotificationReceived?.Invoke(FirebaseCloudMessaging.Current, new FirebasePushNotificationDataEventArgs(parameters));
            FirebaseCloudMessaging.Current.NotificationHandler?.OnReceived(parameters);

            System.Diagnostics.Debug.WriteLine("DidReceiveMessage");
        }

        // Receive data message on iOS 10 devices.
        public void ApplicationReceivedRemoteMessage(RemoteMessage remoteMessage)
        {
            Console.WriteLine(remoteMessage.AppData);
            var parameters = GetParameters(remoteMessage.AppData);
            _onNotificationReceived?.Invoke(FirebaseCloudMessaging.Current, new FirebasePushNotificationDataEventArgs(parameters));
            FirebaseCloudMessaging.Current.NotificationHandler?.OnReceived(parameters);
        }

        public static void Connect()
        {
            Messaging.SharedInstance.ShouldEstablishDirectChannel = true;
            _isConnected = true;
        }

        public static void Disconnect()
        {
            Messaging.SharedInstance.ShouldEstablishDirectChannel = false;
            _isConnected = false;
        }

        public void Subscribe(string[] topics)
        {
            foreach (var topic in topics)
            {
                Subscribe(topic);
            }
        }

        public void Subscribe(string topic)
        {
            if (!_isConnected)
            {
                PendingTopics.Enqueue(new Tuple<string, bool>(topic, true));
                return;
            }

            if (!CurrentTopics.Contains(new NSString(topic)))
            {
                Messaging.SharedInstance.Subscribe($"/topics/{topic}");
                CurrentTopics.Add(new NSString(topic));
            }

            NSUserDefaults.StandardUserDefaults.SetValueForKey(CurrentTopics, FirebaseTopicsKey);
            NSUserDefaults.StandardUserDefaults.Synchronize();
        }

        public void Unsubscribe(string[] topics)
        {
            foreach (var topic in topics)
            {
                Unsubscribe(topic);
            }
        }

        public void Unsubscribe(string topic)
        {
            if (!_isConnected)
            {
                PendingTopics.Enqueue(new Tuple<string, bool>(topic, true));
                return;
            }

            if (CurrentTopics.Contains(new NSString(topic)))
            {
                Messaging.SharedInstance.Unsubscribe($"/topics/{topic}");
                var idx = (nint)CurrentTopics.IndexOf(new NSString(topic));
                if (idx != -1)
                {
                    CurrentTopics.RemoveObject(idx);
                }
            }

            NSUserDefaults.StandardUserDefaults.SetValueForKey(CurrentTopics, FirebaseTopicsKey);
            NSUserDefaults.StandardUserDefaults.Synchronize();
        }

        public void UnsubscribeAll()
        {
            for (nuint i = 0; i < CurrentTopics.Count; i++)
            {
                Unsubscribe(CurrentTopics.GetItem<NSString>(i));
            }
        }

        private static IDictionary<string, object> GetParameters(NSDictionary data)
        {
            var parameters = new Dictionary<string, object>();

            var keyAps = new NSString("aps");
            var keyAlert = new NSString("alert");

            GetValues(data, keyAps, keyAlert, parameters);
            return parameters;
        }

        private static void GetValues(NSDictionary data, NSString keyAps, NSObject keyAlert, IDictionary<string, object> parameters)
        {
            foreach (var val in data)
            {
                if (val.Key.Equals(keyAps))
                {
                    if (!(data.ValueForKey(keyAps) is NSDictionary aps)) continue;
                    GetApsValues(keyAlert, parameters, aps);
                }
                else
                {
                    parameters.Add($"{val.Key}", $"{val.Value}");
                }
            }
        }

        private static void GetApsValues(NSObject keyAlert, IDictionary<string, object> parameters, NSDictionary aps)
        {
            foreach (var apsVal in aps)
            {
                if (apsVal.Value is NSDictionary dictionary)
                {
                    if (!apsVal.Key.Equals(keyAlert)) continue;
                    GetAlertValues(parameters, dictionary);
                }
                else
                {
                    parameters.Add($"aps.{apsVal.Key}", $"{apsVal.Value}");
                }
            }
        }

        private static void GetAlertValues(IDictionary<string, object> parameters, NSDictionary dictionary)
        {
            foreach (var alertVal in dictionary)
            {
                parameters.Add($"aps.alert.{alertVal.Key}", $"{alertVal.Value}");
            }
        }
    }
}
