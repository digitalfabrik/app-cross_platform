using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Android.Content;
using Android.Gms.Common;
using Firebase.Messaging;
using Integreat.Droid;
using Integreat.Shared.Firebase;
using Xamarin.Forms;

[assembly: Dependency(typeof(FirebasePushNotificationManager))]
namespace Integreat.Droid
{
    public class FirebasePushNotificationManager : IFirebasePushNotificationManager
    {
        public IPushNotificationHandler NotificationHandler { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        private static Context _context;
        private const string _KeyGroupName = "FirebasePushNotification";
        private const string _FirebaseTopicsKey = "FirebaseTopics";
        private const string _FirebaseTokenKey = "FirebaseToken";
        private static ICollection<string> _currentTopics = Android.App.Application.Context.GetSharedPreferences(_KeyGroupName, FileCreationMode.Private).GetStringSet(_FirebaseTopicsKey, new Collection<string>());

        public string Token { get { return Android.App.Application.Context.GetSharedPreferences(_KeyGroupName, FileCreationMode.Private).GetString(_FirebaseTokenKey, string.Empty); } }

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

        public static void Initialize(Context context){
            _context = context;
        }

        public void Subscribe(string[] topics)
        {
            foreach(var t in topics)
            {
                Subscribe(t);
            } 
        }

        public void Subscribe(string topic)
        {
            if (!_currentTopics.Contains(topic))
            {
                FirebaseMessaging.Instance.SubscribeToTopic(topic);
                _currentTopics.Add(topic);
            }
        }

        public void Unsubscribe(string[] topics)
        {
            foreach(var t in topics)
            {
                Unsubscribe(t);
            }
        }

        public void Unsubscribe(string topic)
        {
            if(_currentTopics.Contains(topic))
            {
                FirebaseMessaging.Instance.UnsubscribeFromTopic(topic);
                _currentTopics.Remove(topic);
            }
        }

        public void UnsubscribeAll()
        {
            foreach(var t in _currentTopics)
            {
                Unsubscribe(t);
            }

            _currentTopics.Clear();
        }

        public static void SaveToken(string token)
        {
            if(token != FirebaseCloudMessaging.Current.Token)
            {
                var editor = Android.App.Application.Context.GetSharedPreferences(FirebasePushNotificationManager._KeyGroupName, FileCreationMode.Private).Edit();
                editor.PutString(FirebasePushNotificationManager._FirebaseTokenKey, token);
                editor.Commit();

                _onTokenRefresh?.Invoke(FirebaseCloudMessaging.Current, new FirebasePushNotificationTokenEventArgs(token));
            }
        }

        public static void ReceivedNotification(IDictionary<string, object> parameters)
        {
            _onNotificationReceived?.Invoke(FirebaseCloudMessaging.Current, new FirebasePushNotificationDataEventArgs(parameters));
        }
    }
}
