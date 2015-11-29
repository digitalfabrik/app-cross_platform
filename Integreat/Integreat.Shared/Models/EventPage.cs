using System;
using System.Collections.Generic;
using SQLite.Net.Attributes;
using System.Collections.ObjectModel;
using Newtonsoft.Json;
using SQLiteNetExtensions.Attributes;

namespace Integreat.Models
{
	[Table("EventPage")]
	public class EventPage : Page
    {
        [JsonProperty("event")]
        [OneToOne(CascadeOperations = CascadeOperation.All)]
        public Event Event{get;set; }

        [JsonProperty("location")]
        [OneToOne(CascadeOperations = CascadeOperation.All)]
        public EventLocation Location{get;set;}
        
        [JsonProperty("tags")]
        [OneToMany(CascadeOperations = CascadeOperation.All)]
        public List<EventTag> Tags{get;set; }

        [JsonProperty("categories")]
        [OneToMany(CascadeOperations = CascadeOperation.All)]
        public List<EventCategory> Categories{get;set;}

        public EventPage() { }
		public EventPage(Page page, Event pEvent, EventLocation location, List<EventTag> tags, List<EventCategory> categories) :
			base(page.Id, page.Title, page.Type, page.Status, page.Modified, page.Description,
			page.Content, page.ParentId, page.Order, page.Thumbnail, page.Author, page.AvailableLanguages){
			Event = pEvent;
			Location = location;
			Tags = tags;
			Categories = categories;
		}
    }
}

