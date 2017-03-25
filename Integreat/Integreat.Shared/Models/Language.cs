using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using SQLite.Net.Attributes;
using SQLiteNetExtensions.Attributes;

namespace Integreat.Shared.Models
{
	[Table ("language")]
	public class Language
	{
		[PrimaryKey]
		public string PrimaryKey { get; set; }

		[JsonProperty ("id")]
		public int Id{ get; set; }

		public DateTime Modified { get; set; }

		[JsonProperty ("code")]
		public string ShortName{ get; set; }

		[JsonProperty ("native_name")]
		public string Name{ get; set; }

		/// <summary>
		/// Gets or sets the read direction for the language.
		/// </summary>
		[JsonProperty ("dir")]
		public string Direction { get; set; }

		[JsonProperty ("country_flag_url")]
		public string IconPath{ get; set; }

		[ForeignKey (typeof(Location))]
		public int LocationId { get; set; }

		[ManyToOne]
		public Location Location{ get; set; }

		[OneToMany]
		public List<Page> Pages { get; set; }

		public Language ()
		{
		}

		public Language (int id, string shortName, string name, string iconPath)
		{
			Id = id;
			ShortName = shortName;
			Name = name;
			IconPath = iconPath;
		}

		public override string ToString ()
		{
			return ShortName + "";
		}
	}
}

