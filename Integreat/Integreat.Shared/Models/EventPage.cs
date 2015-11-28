using System;
using SQLite.Net.Attributes;
using System.Collections.ObjectModel;
using Newtonsoft.Json;

namespace Integreat.Models
{
	[Table("EventPage")]
	public class EventPage : Page
    {
        [JsonProperty("event")]
        public Event Event{get;set; }

        [JsonProperty("location")]
        public EventLocation Location{get;set;}
        
        [JsonProperty("tags")]
        public Collection<EventTag> Tags{get;set; }

        [JsonProperty("categories")]
        public Collection<EventCategory> Categories{get;set;}

        public EventPage() { }
		public EventPage(Page page, Event pEvent, EventLocation location, Collection<EventTag> tags,  Collection<EventCategory> categories) :
			base(page.Id, page.Title, page.Type, page.Status, page.Modified, page.Description,
			page.Content, page.ParentId, page.Order, page.Thumbnail, page.Author, page.AvailableLanguages){
			Event = pEvent;
			Location = location;
			Tags = tags;
			Categories = categories;
		}
    }
}

