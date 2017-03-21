using System;
using System.Collections.Generic;
using System.Globalization;
using SQLite.Net.Attributes;
using Newtonsoft.Json;
using SQLiteNetExtensions.Attributes;

namespace Integreat.Shared.Models
{
	[Table ("EventPage")]
	public class EventPage : Page
	{
		[JsonProperty ("event")]
		[OneToOne (CascadeOperations = CascadeOperation.All)]
		public Event Event{ get; set; }

		[JsonProperty ("location")]
		[OneToOne (CascadeOperations = CascadeOperation.All)]
		public EventLocation Location{ get; set; }

		[JsonProperty ("tags")]
		[OneToMany (CascadeOperations = CascadeOperation.All)]
		//[TextBlob("AddressesBlobbed")]
        public List<EventTag> Tags{ get; set; }

		[JsonProperty ("categories")]
		[OneToMany (CascadeOperations = CascadeOperation.All)]
		//[TextBlob("AddressesBlobbed")]
        public List<EventCategory> Categories{ get; set; }


	    public string EventDescription => new DateTime(Event.StartTime).ToString("dd.MM.yyyy HH:mm") + " - " + Description;


        public EventPage ()
		{
		}

		public EventPage (Page page, Event pEvent, EventLocation location, List<EventTag> tags, List<EventCategory> categories) :
			base (page.PrimaryKey, page.Id, page.Title, page.Type, page.Status, page.Modified, page.Description,
			     page.Content, page.ParentId, page.Order, page.Thumbnail, page.Author, page.AutoTranslated, page.AvailableLanguages)
		{
			Event = pEvent;
			Location = location;
			Tags = tags;
			Categories = categories;
		}
	}
}

