using System.Collections.Generic;

namespace App1.Firebase
{
    public interface IPushNotificationHandler
    {
        /// <summary>
        /// triggered when error occurs.
        /// </summary>
        /// <param name="error">Error.</param>
        void OnError(string error);
        /// <summary>
        /// triggered when a notification is opened
        /// </summary>
        void OnOpened(NotificationResponse response);
        /// <summary>
        /// triggered when a notification is received
        /// </summary>
        /// <param name="parameters">Parameters.</param>
        void OnReceived(IDictionary<string, object> parameters);
    }
}
