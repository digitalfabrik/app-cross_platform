using Newtonsoft.Json;
using SQLite.Net.Attributes;
using SQLiteNetExtensions.Attributes;

namespace Integreat.Shared.Models
{
	[Table ("EventTag")]
	public class EventTag
	{
		[PrimaryKey, AutoIncrement]
		public int PrimaryKey { get; set; }

		[JsonProperty ("name")]
		public string Name { get; set; }

		[JsonProperty ("id")]
		public int Id { get; set; }

		[ForeignKey (typeof(Page))]
		public string PageId { get; set; }

		[ManyToOne]
		public Page Page { get; set; }

		public EventTag ()
		{
		}

		public EventTag (int id, string name)
		{
			Id = id;
			Name = name;
		}
	}
}

