using System;
using Newtonsoft.Json;
using SQLite;
using SQLite.Net.Attributes;

namespace Integreat
{
	[Table("Event")]
    [JsonConverter(typeof(EventConverter))]
    public class Event
	{
		[PrimaryKey, Column("_id")]
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("start_date")]
        public long StartTime{ get; set; }

        [JsonProperty("end_date")]
        public long EndTime{ get; set; }

        [JsonProperty("available_languages")]
        public bool AllDay{ get; set; }

        [JsonProperty("available_languages")]
        public int PageId{ get; set;}

		public Event(int id, long startTime, long endTime, bool allDay, int pageId) {
			Id = id;
			StartTime = startTime;
			EndTime = endTime;
			AllDay = allDay;
			PageId = pageId;
		}
    }

    internal class EventConverter
    {
    }
}

