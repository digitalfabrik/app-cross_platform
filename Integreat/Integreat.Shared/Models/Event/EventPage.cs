using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Integreat.Shared.Models
{
    /// <summary>
    /// Special Page class for the content of Events.
    /// </summary>
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
	    public string EventDescription => new DateTime(Event.StartTime).ToString("dd.MM.yyyy HH:mm") + " - " + Description;
	}
}

