using System;
using System.Collections.Generic;
using System.Security;
using Newtonsoft.Json;

namespace Integreat.Shared.Models
{
    /// <summary>
    /// Special Page class for the content of Events.
    /// </summary>
    [SecurityCritical]
	public class EventPage : Page
	{
		public Event Event{ get; set; }

		[JsonProperty ("location")]
		public EventLocation Location{ get; set; }

		[JsonProperty ("tags")]
		//[TextBlob("AddressesBlobbed")]
        public List<EventTag> Tags{ get; set; }

		[JsonProperty ("categories")]
		//[TextBlob("AddressesBlobbed")]
        public List<EventCategory> Categories{ get; set; }

	    public string EventThumbnail => !string.IsNullOrEmpty(Thumbnail) ? Thumbnail : "CalendarBig.png"; //todo if null replace with default calender icon

	    public string EventDescription => EventDate + " - " + Location.Address + " - " + Description;

	    public string EventDate => new DateTime(Event.StartTime).ToString("dd.MM.yy HH:mm");

	}
}

