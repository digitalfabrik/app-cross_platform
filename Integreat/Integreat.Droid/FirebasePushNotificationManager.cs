using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Android.App;
using Android.Content;
using Android.OS;
using Firebase.Messaging;
using Integreat.Droid;
using Integreat.Shared.Firebase;
using Xamarin.Forms;

[assembly: Dependency(typeof(FirebasePushNotificationManager))]
namespace Integreat.Droid
{
    /// <inheritdoc />
    /// <summary>
    /// This class is the <see cref="T:Integreat.Droid.FirebasePushNotificationManager" /> native implementation.
    /// </summary>
    /// <seealso cref="T:Integreat.Shared.Firebase.IFirebasePushNotificationManager" />
    public class FirebasePushNotificationManager : IFirebasePushNotificationManager
    {
        private static NotificationResponse _delayedNotificationResponse = null;

        private const string KeyGroupName = "FirebasePushNotification";
        private const string FirebaseTopicsKey = "FirebaseTopics";
        private const string FirebaseTokenKey = "FirebaseToken";

        private readonly ICollection<string> _currentTopics;

        public FirebasePushNotificationManager()
        {
            _currentTopics = GetExistingFirebaseTopicKeys();
            Token = Android.App.Application.Context.GetSharedPreferences(KeyGroupName, FileCreationMode.Private).GetString(FirebaseTokenKey, string.Empty);
        }
       
        public string Token { get; }

        private static FirebasePushNotificationTokenEventHandler _onTokenRefresh;
        /// <inheritdoc />
        public event FirebasePushNotificationTokenEventHandler OnTokenRefresh
        {
            add => _onTokenRefresh += value;
            remove => _onTokenRefresh -= value;
        }

        private static FirebasePushNotificationResponseEventHandler _onNotificationOpened;
        /// <inheritdoc />
        public event FirebasePushNotificationResponseEventHandler OnNotificationOpened
        {
            add 
            {
                var previousVal = _onNotificationOpened;
                _onNotificationOpened += value;
                if (_delayedNotificationResponse != null && previousVal == null)
                {
                    var tmpParams = _delayedNotificationResponse;
                    _onNotificationOpened?.Invoke(FirebaseCloudMessaging.Current, new FirebasePushNotificationResponseEventArgs(tmpParams.Data, tmpParams.Identifier));
                    _delayedNotificationResponse = null;
                }
            } 
            remove
            {
                _onNotificationOpened -= value; 
            }
        }

        private static FirebasePushNotificationDataEventHandler _onNotificationReceived;

        /// <inheritdoc />
        public event FirebasePushNotificationDataEventHandler OnNotificationReceived
        {
            add => _onNotificationReceived += value;
            remove => _onNotificationReceived -= value;
        }

        private static FirebasePushNotificationDataEventHandler _onNotificationDeleted;

        /// <inheritdoc />
        public event FirebasePushNotificationDataEventHandler OnNotificationDeleted
        {
            add => _onNotificationDeleted += value;
            remove => _onNotificationDeleted -= value;
        }

        private static FirebasePushNotificationErrorEventHandler _onNotificationError;

        /// <inheritdoc />
        public event FirebasePushNotificationErrorEventHandler OnNotificationError
        {
            add => _onNotificationError += value;
            remove => _onNotificationError -= value;
        }

        public static void ProcessIntent(Activity activity, Intent intent, bool enableDelayedResponse = true)
        {
            Bundle extras = intent?.Extras;

            //check if intent has extras
            if(extras != null && !extras.IsEmpty)
            {
                var parameters = new Dictionary<string, object>();
                foreach (var key in extras.KeySet())
                {
                    if (!parameters.ContainsKey(key) && extras.Get(key) != null)
                        parameters.Add(key, $"{extras.Get(key)}");
                }

                if(parameters.Count > 0)
                {
                    var response = new NotificationResponse(parameters, extras.GetString("action_identifier", string.Empty));

                    if (_onNotificationOpened == null && enableDelayedResponse)
                        _delayedNotificationResponse = response;
                    else
                        _onNotificationOpened?.Invoke(FirebaseCloudMessaging.Current, new FirebasePushNotificationResponseEventArgs(response.Data, response.Identifier));


                    FirebaseCloudMessaging.Current.NotificationHandler?.OnOpened(response);
                }
            }
        }

        /// <inheritdoc />
        public void Subscribe(string[] topics)
        {
            foreach (var t in topics)
            {
                Subscribe(t);
            }
        }

        /// <inheritdoc />
        public void Subscribe(string topic)
        {
            if (_currentTopics.Contains(topic)) return;
            FirebaseMessaging.Instance.SubscribeToTopic(topic);
            _currentTopics.Add(topic);
            UpdateCurrentTopics();
        }

        /// <inheritdoc />
        public void Unsubscribe(string[] topics)
        {
            foreach (var t in topics)
            {
                Unsubscribe(t);
            }
        }

        /// <inheritdoc />
        public void Unsubscribe(string topic)
        {
            if (!_currentTopics.Contains(topic)) return;
            FirebaseMessaging.Instance.UnsubscribeFromTopic(topic);
            _currentTopics.Remove(topic);
            UpdateCurrentTopics();
        }

        /// <inheritdoc />
        public void UnsubscribeAll()
        {
            Unsubscribe(ListToArray(_currentTopics));
        }

        /// <inheritdoc />
        public string[] SubscribedTopics => ListToArray(_currentTopics);

        public IPushNotificationHandler NotificationHandler { get; set; }

        /// <summary> Saves the token. </summary>
        /// <param name="token">The token.</param>
        public static void SaveToken(string token)
        {
            if (token == FirebaseCloudMessaging.Current.Token) return;
            var editor = Android.App.Application.Context.GetSharedPreferences(KeyGroupName, FileCreationMode.Private).Edit();
            editor.PutString(FirebaseTokenKey, token);
            editor.Commit();

            _onTokenRefresh?.Invoke(FirebaseCloudMessaging.Current, new FirebasePushNotificationTokenEventArgs(token));
        }

        /// <summary> Receiveds the notification. </summary>
        /// <param name="parameters">The parameters.</param>
        public static void ReceivedNotification(IDictionary<string, object> parameters)
        {
            _onNotificationReceived?.Invoke(FirebaseCloudMessaging.Current, new FirebasePushNotificationDataEventArgs(parameters));
            FirebaseCloudMessaging.Current.NotificationHandler?.OnReceived(parameters);
        }

        private static string[] ListToArray(IEnumerable<string> topics)
        {
            return topics.ToArray();
        }

        private void UpdateCurrentTopics()
        {
            var editor = Android.App.Application.Context.GetSharedPreferences(KeyGroupName, FileCreationMode.Private).Edit();
            editor.PutStringSet(FirebaseTopicsKey, _currentTopics);
            editor.Commit();
        }

        private static ICollection<string> GetExistingFirebaseTopicKeys() =>
            Android.App.Application.Context.GetSharedPreferences(KeyGroupName, FileCreationMode.Private)
                .GetStringSet(FirebaseTopicsKey, new Collection<string>());
    }
}
