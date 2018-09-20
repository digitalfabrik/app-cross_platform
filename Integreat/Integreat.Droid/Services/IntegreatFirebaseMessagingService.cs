﻿using Android.App;
using Firebase.Messaging;
using System.Collections.Generic;
using System.Linq;

namespace Integreat.Droid.Services
{
    /// <summary>
    /// Integreat firebase messaging service.
    /// </summary>
    [Service]
    [IntentFilter(new[] { "com.google.firebase.MESSAGING_EVENT" })]
    public class IntegreatFirebaseMessagingService : FirebaseMessagingService
    {
        public override void OnMessageReceived(RemoteMessage message)
        {
            var parameters = new Dictionary<string, object>();
            var notification = message.GetNotification();

            if (notification != null)
            {
                if (!string.IsNullOrEmpty(notification.Body))
                    parameters.Add("body", notification.Body);

                if (!string.IsNullOrEmpty(notification.BodyLocalizationKey))
                    parameters.Add("body_loc_key", notification.BodyLocalizationKey);

                var bodyLocArgs = notification.GetBodyLocalizationArgs();
                if (bodyLocArgs != null && bodyLocArgs.Any())
                    parameters.Add("body_loc_args", bodyLocArgs);

                if (!string.IsNullOrEmpty(notification.Title))
                    parameters.Add("title", notification.Title);

                if (!string.IsNullOrEmpty(notification.TitleLocalizationKey))
                    parameters.Add("title_loc_key", notification.TitleLocalizationKey);

                var titleLocArgs = notification.GetTitleLocalizationArgs();
                if (titleLocArgs != null && titleLocArgs.Any())
                    parameters.Add("title_loc_args", titleLocArgs);

                if (!string.IsNullOrEmpty(notification.Tag))
                    parameters.Add("tag", notification.Tag);

                if (!string.IsNullOrEmpty(notification.Sound))
                    parameters.Add("sound", notification.Sound);

                if (!string.IsNullOrEmpty(notification.Icon))
                    parameters.Add("icon", notification.Icon);

                if (notification.Link != null)
                    parameters.Add("link_path", notification.Link.Path);

                if (!string.IsNullOrEmpty(notification.ClickAction))
                    parameters.Add("click_action", notification.ClickAction);

                if (!string.IsNullOrEmpty(notification.Color))
                    parameters.Add("color", notification.Color);

                if (!string.IsNullOrEmpty(message.From))
                    parameters.Add("topic", message.From);
            }

            foreach (var d in message.Data)
            {
                if (!parameters.ContainsKey(d.Key))
                    parameters.Add(d.Key, d.Value);
            }

            FirebasePushNotificationManager.ReceivedNotification(parameters);
        }
    }
}
