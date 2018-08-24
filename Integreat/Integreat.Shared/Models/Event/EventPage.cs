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
        [JsonProperty("event")]
		public Event Event{ get; set; }

		[JsonProperty ("location")]
		public EventLocation Location{ get; set; }

	    public string EventThumbnail => !string.IsNullOrEmpty(Thumbnail) ? Thumbnail : "CalendarBig.png";

	    public string EventDescription => EventDate + " - " + Location.Address + " - " + Description;

	    public string EventDate => new DateTime(Event.StartTime).ToString("dd.MM.yy HH:mm");

	}
}

