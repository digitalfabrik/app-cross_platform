using System;
using System.Collections.Generic;

namespace Integreat.Shared.Firebase
{
    /// <inheritdoc />
    public delegate void FirebasePushNotificationTokenEventHandler(object source, FirebasePushNotificationTokenEventArgs e);

    /// <inheritdoc />
    public class FirebasePushNotificationTokenEventArgs : EventArgs
    {
        public string Token { get; set; }

        public FirebasePushNotificationTokenEventArgs(string token)
        {
            Token = token;
        }
    }

    /// <inheritdoc />
    public delegate void FirebasePushNotificationErrorEventHandler(object source, FirebasePushNotificationErrorEventArgs e);

    /// <inheritdoc />
    public class FirebasePushNotificationErrorEventArgs : EventArgs
    {
        public string Message { get; set; }

        public FirebasePushNotificationErrorEventArgs(string message)
        {
            Message = message;
        }
    }

    /// <inheritdoc />
    public delegate void FirebasePushNotificationDataEventHandler(object source, FirebasePushNotificationDataEventArgs e);

    /// <inheritdoc />
    public class FirebasePushNotificationDataEventArgs : EventArgs
    {
        public IDictionary<string, object> Data { get; }

        public FirebasePushNotificationDataEventArgs(IDictionary<string, object> data)
        {
            Data = data;
        }
    }

    /// <inheritdoc />
    public delegate void FirebasePushNotificationResponseEventHandler(object source, FirebasePushNotificationResponseEventArgs e);

    /// <inheritdoc />
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

    /// <summary>
    /// The <see cref="IFirebasePushNotificationManager"/> handles sub- and unsusbrciptions for push notification messages
    /// </summary>
    public interface IFirebasePushNotificationManager
    {
        /// <summary>  Gets all subscribed topics. </summary>
        /// <value>The subscribed topics.</value>
        IEnumerable<string> SubscribedTopics { get; }
        /// <summary> Gets or sets the notification handler. </summary>
        IPushNotificationHandler NotificationHandler { get; set; }
        /// <summary> Push notification token </summary>
        string Token { get; }

        /// <summary> Subscribe to specified topics. </summary>
        /// <param name="topics">Topics.</param>
        void Subscribe(string[] topics);
        /// <summary> Subscribe to one topic </summary>
        /// <param name="topic">Topic.</param>
        void Subscribe(string topic);
        /// <summary> Unsubscribe the specified topics. </summary>
        /// <param name="topics">Topics.</param>
        void Unsubscribe(string[] topics);
        /// <summary> Unsubscribe to one topic. </summary>
        /// <param name="topic">Topic.</param>
        void Unsubscribe(string topic);
        /// <summary> Unsubscribe all topics </summary>
        void UnsubscribeAll();

        /// <summary> Occurs when on token refresh. </summary>
        event FirebasePushNotificationTokenEventHandler OnTokenRefresh;
        /// <summary> Occurs when on notification opened. </summary>
        event FirebasePushNotificationResponseEventHandler OnNotificationOpened;
        /// <summary> Occurs when on notification received. </summary>
        event FirebasePushNotificationDataEventHandler OnNotificationReceived;
        /// <summary> Occurs when on notification deleted. </summary>
        event FirebasePushNotificationDataEventHandler OnNotificationDeleted;
        /// <summary> Occurs when on notification error. </summary>
        event FirebasePushNotificationErrorEventHandler OnNotificationError;
    }
}
