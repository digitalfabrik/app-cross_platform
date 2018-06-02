using System;
using System.Collections.Generic;
using System.Linq;
using Firebase.CloudMessaging;
using Firebase.Core;
using Foundation;
using Integreat.iOS;
using Integreat.Shared.Firebase;
using UIKit;
using UserNotifications;
using Xamarin.Forms;

[assembly: Dependency(typeof(FirebasePushNotificationManager))]
namespace Integreat.iOS
{
    public class FirebasePushNotificationManager : NSObject, IFirebasePushNotificationManager, IUNUserNotificationCenterDelegate, IMessagingDelegate
    {
        private static bool _isConnected = false;
        private static Queue<Tuple<string, bool>> _pendingTopics = new Queue<Tuple<string, bool>>();
        private static NSString _FirebaseTopicsKey = new NSString("FirebaseTopics");
        private static NSMutableArray _currentTopics = (NSUserDefaults.StandardUserDefaults.ValueForKey(_FirebaseTopicsKey) as NSArray ?? new NSArray()).MutableCopy() as NSMutableArray;
        private const string _FirebaseTokenKey = "FirebaseToken";
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

        public IPushNotificationHandler NotificationHandler { get; set; }

        public string[] SubscribedTopics
        {
            get
            {
                IList<string> topics = new List<string>();
                for (nuint i = 0; i < _currentTopics.Count; i++)
                {
                    topics.Add(_currentTopics.GetItem<NSString>(i));
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
            if(!string.IsNullOrEmpty(refreshedToken))
            {
                _onTokenRefresh?.Invoke(FirebaseCloudMessaging.Current, new FirebasePushNotificationTokenEventArgs(refreshedToken));
                Connect();
            }

            NSUserDefaults.StandardUserDefaults.SetString(fcmToken, _FirebaseTokenKey);
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
            foreach(var topic in topics)
            {
                Subscribe(topic);
            }
        }

        public void Subscribe(string topic)
        {
            if(!_isConnected)
            {
                _pendingTopics.Enqueue(new Tuple<string, bool>(topic, true));
                return;
            }

            if(!_currentTopics.Contains(new NSString(topic)))
            {
                Messaging.SharedInstance.Subscribe($"/topics/{topic}");
                _currentTopics.Add(new NSString(topic));
            }

            NSUserDefaults.StandardUserDefaults.SetValueForKey(_currentTopics, _FirebaseTopicsKey);
            NSUserDefaults.StandardUserDefaults.Synchronize();
        }

        public void Unsubscribe(string[] topics)
        {
            foreach(var topic in topics)
            {
                Unsubscribe(topic);
            }
        }

        public void Unsubscribe(string topic)
        {
            if (!_isConnected)
            {
                _pendingTopics.Enqueue(new Tuple<string, bool>(topic, true));
                return;
            }

            if(_currentTopics.Contains(new NSString(topic)))
            {
                Messaging.SharedInstance.Unsubscribe($"/topics/{topic}");
                nint idx = (nint)_currentTopics.IndexOf(new NSString(topic));
                if(idx != -1)
                {
                    _currentTopics.RemoveObject(idx);  
                }
            }

            NSUserDefaults.StandardUserDefaults.SetValueForKey(_currentTopics, _FirebaseTopicsKey);
            NSUserDefaults.StandardUserDefaults.Synchronize();
        }

        public void UnsubscribeAll()
        {
            for (nuint i = 0; i < _currentTopics.Count; i++)
            {
                Unsubscribe(_currentTopics.GetItem<NSString>(i));
            }
        }

        private static IDictionary<string, object> GetParameters(NSDictionary data)
        {
            var parameters = new Dictionary<string, object>();

            var keyAps = new NSString("aps");
            var keyAlert = new NSString("alert");

            foreach(var val in data)
            {
                if(val.Key.Equals(keyAps))
                {
                    NSDictionary aps = data.ValueForKey(keyAps) as NSDictionary;

                    if(aps != null)
                    {
                        foreach(var apsVal in aps)
                        {
                            if(apsVal.Value is NSDictionary)
                            {
                                if(apsVal.Key.Equals(keyAlert))
                                {
                                    foreach (var alertVal in apsVal.Value as NSDictionary)
                                    {
                                        parameters.Add($"aps.alert.{alertVal.Key}", $"{alertVal.Value}");
                                    }  
                                }
                            }
                            else
                            {
                                parameters.Add($"aps.{apsVal.Key}", $"{apsVal.Value}");
                            }
                        }
                    }
                }
                else
                {
                    parameters.Add($"{val.Key}", $"{val.Value}");
                }
            }
            return parameters;
        }
    }
}
