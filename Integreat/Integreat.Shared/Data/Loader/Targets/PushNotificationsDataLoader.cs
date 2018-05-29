using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Integreat.Shared.Models;

namespace Integreat.Shared.Data.Loader.Targets
{
    public class PushNotificationsDataLoader : IDataLoader
    {
        public const string FileNameConst = "pushnotificationsV1";

        /// <inheritdoc />
        public string FileName => FileNameConst;
        

        //just to match the interface
        public DateTime LastUpdated
        {
            get;
            set;
        }

		/// <inheritdoc />
		public string Id => "Id";

		/// <summary> Loads the event pages. </summary>
        /// <param name="forLocation">Which location to load for.</param>
        /// <returns>Task to load the event pages.</returns>
        public Collection<EventPage> Load(Location forLocation)
        {         
			Collection<EventPage> notifications = DataLoaderProvider.GetCachedFiles<EventPage>(this).Result;
			if(notifications != null)
			{
				//check if location is right
                foreach (var notification in notifications)
                {
                    if (notification.Location.Id != forLocation.Id)
                        notifications.Remove(notification);
                }
			}

			return notifications;
        }
        /// <summary>
        /// Add the specified eventPage.
        /// </summary>
		public void Add(EventPage eventPage)
		{
			if(eventPage !=null)
			{
				Task.Run(() => DataLoaderProvider.AddObject(eventPage, this));
			}
		}
    }
}