using System;
using System.Collections.Generic;

namespace Integreat.Shared.Firebase
{
    public delegate void FirebasePushNotificationTokenEventHandler(object source, FirebasePushNotificationTokenEventArgs e);

    public class FirebasePushNotificationTokenEventArgs : EventArgs
    {
        public string Token;

        public FirebasePushNotificationTokenEventArgs(string token)
        {
            Token = token;
        }
    }

    public delegate void FirebasePushNotificationErrorEventHandler(object source, FirebasePushNotificationErrorEventArgs e);

    public class FirebasePushNotificationErrorEventArgs : EventArgs
    {
        public string Message;

        public FirebasePushNotificationErrorEventArgs(string message)
        {
            Message = message;
        }
    }

    public delegate void FirebasePushNotificationDataEventHandler(object source, FirebasePushNotificationDataEventArgs e);

    public class FirebasePushNotificationDataEventArgs : EventArgs
    {
        public IDictionary<string, object> Data { get; }

        public FirebasePushNotificationDataEventArgs(IDictionary<string, object> data)
        {
            Data = data;
        }
    }

    public delegate void FirebasePushNotificationResponseEventHandler(object source, FirebasePushNotificationResponseEventArgs e);

    public class FirebasePushNotificationResponseEventArgs : EventArgs
    {
        public string Identifier { get; }

        public IDictionary<string, object> Data { get; }

        public FirebasePushNotificationResponseEventArgs(IDictionary<string, object> data, string identifier = "")
        {
            Data = data;
            Identifier = identifier;
        }
    }

    public interface IFirebasePushNotificationManager
    {
        /// <summary>
        /// Subscribe to specified topics.
        /// </summary>
        /// <param name="topics">Topics.</param>
        void Subscribe(string[] topics);
        /// <summary>
        /// Subscribe to one topic
        /// </summary>
        /// <param name="topic">Topic.</param>
        void Subscribe(string topic);
        /// <summary>
        /// Unsubscribe the specified topics.
        /// </summary>
        /// <param name="topics">Topics.</param>
        void Unsubscribe(string[] topics);
        /// <summary>
        /// Unsubscribe to one topic.
        /// </summary>
        /// <param name="topic">Topic.</param>
        void Unsubscribe(string topic);
        /// <summary>
        /// Unsubscribe all topics
        /// </summary>
        void UnsubscribeAll();
        /// <summary>
        /// Gets or sets the notification handler.
        /// </summary>
        IPushNotificationHandler NotificationHandler { get; set; }
        /// <summary>
        /// Push notification token
        /// </summary>
        string Token { get; }
        /// <summary>
        /// Occurs when on token refresh.
        /// </summary>
        event FirebasePushNotificationTokenEventHandler OnTokenRefresh;
        /// <summary>
        /// Occurs when on notification opened.
        /// </summary>
        event FirebasePushNotificationResponseEventHandler OnNotificationOpened;
        /// <summary>
        /// Occurs when on notification received.
        /// </summary>
        event FirebasePushNotificationDataEventHandler OnNotificationReceived;
        /// <summary>
        /// Occurs when on notification deleted.
        /// </summary>
        event FirebasePushNotificationDataEventHandler OnNotificationDeleted;
        /// <summary>
        /// Occurs when on notification error.
        /// </summary>
        event FirebasePushNotificationErrorEventHandler OnNotificationError;
    }
}
