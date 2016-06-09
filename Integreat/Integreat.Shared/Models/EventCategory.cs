using Newtonsoft.Json;
using SQLite.Net.Attributes;
using SQLiteNetExtensions.Attributes;

namespace Integreat.Shared.Models
{
	[Table ("EventCategory")]
	public class EventCategory
	{
		[PrimaryKey, AutoIncrement]
		public int PrimaryKey { get; set; }

		[JsonProperty ("id")]
		public int Id	{ get; set; }

		[JsonProperty ("name")]
		public string Name{ get; set; }

		public int EventId{ get; set; }

		[ForeignKey (typeof(Page))]
		public string PageId { get; set; }

		[ManyToOne]
		public Page Page { get; set; }

		public EventCategory ()
		{
		}

		public EventCategory (int id, string name)
		{
			Id = id;
			Name = name;
		}
	}
}

