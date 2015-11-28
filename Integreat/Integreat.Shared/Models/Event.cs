using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using SQLite.Net.Attributes;

namespace Integreat.Models
{
	[Table("Event")]
    [JsonConverter(typeof(EventConverter))]
    public class Event
	{
		[PrimaryKey, Column("_id")]
        public int Id { get; set; }
        
        public long StartTime{ get; set; }
        
        public long EndTime{ get; set; }
        
        public bool AllDay{ get; set; }
        
        public int PageId{ get; set;}

		public Event(int id, long startTime, long endTime, bool allDay, int pageId) {
			Id = id;
			StartTime = startTime;
			EndTime = endTime;
			AllDay = allDay;
			PageId = pageId;
		}

        public Event() { }
    }

    internal class EventConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override bool CanConvert(Type type)
        {
            return typeof(Event).IsAssignableFrom(type);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var dict = serializer.Deserialize<Dictionary<string, object>>(reader);
            var id = int.Parse((string)dict["id"]);
            var startDate = (string)dict["start_date"];
            var endDate = (string)dict["end_date"];
            var startTime = (string)dict["start_time"];
            var endTime = (string)dict["end_time"];
            var allDay = ((string)dict["all_day"]).IsTrue();

            var start = (startDate + " " + startTime).DateTimeFromRestString().Ticks;
            var end = (endDate + " " + endTime).DateTimeFromRestString().Ticks;
            return new Event(id, start, end, allDay, -1);
        }
    }
}

