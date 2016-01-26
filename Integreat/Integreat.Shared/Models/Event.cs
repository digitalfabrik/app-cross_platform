using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using SQLite.Net.Attributes;
using SQLiteNetExtensions.Attributes;
using Integreat.Shared.Utilities;

namespace Integreat.Shared.Models
{
	[Table ("Event")]
	[JsonConverter (typeof(EventConverter))]
	public class Event
	{
		[PrimaryKey, AutoIncrement]
		public int PrimaryKey { get; set; }

		[ForeignKey (typeof(Page))]
		public int PageId { get; set; }

		public int Id { get; set; }

		public long StartTime{ get; set; }

		public long EndTime{ get; set; }

		public bool AllDay{ get; set; }


		public Event (int id, long startTime, long endTime, bool allDay)
		{
			Id = id;
			StartTime = startTime;
			EndTime = endTime;
			AllDay = allDay;
		}

		public Event ()
		{
		}
	}

	internal class EventConverter : JsonConverter
	{
		public override void WriteJson (JsonWriter writer, object value, JsonSerializer serializer)
		{
			throw new NotImplementedException ();
		}

		public override bool CanConvert (Type type)
		{
			return Reflections.IsAssignableFrom (typeof(Event), type);
		}

		public override object ReadJson (JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			var dict = serializer.Deserialize<Dictionary<string, object>> (reader);
			var id = int.Parse (dict ["id"].ToString ());
			var startDate = dict ["start_date"].ToString ();
			var endDate = dict ["end_date"].ToString ();
			var startTime = dict ["start_time"].ToString ();
			var endTime = dict ["end_time"].ToString ();
			var allDay = dict ["all_day"].ToString ().IsTrue ();

			var start = (startDate + " " + startTime).DateTimeFromRestString ().Ticks;
			var end = (endDate + " " + endTime).DateTimeFromRestString ().Ticks;
			return new Event (id, start, end, allDay);
		}
	}
}

