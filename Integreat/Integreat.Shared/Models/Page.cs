using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using SQLite.Net.Attributes;
using SQLiteNetExtensions.Attributes;
using Integreat.Shared.Utilities;

namespace Integreat.Shared.Models
{
	[Table ("Page")]
	public class Page
	{
		[PrimaryKey, AutoIncrement]
		public int PrimaryKey { get; set; }

		[ForeignKey (typeof(Page))]
		[JsonProperty ("parent")]
		public int ParentId { get; set; }

		[ManyToOne ("ParentId", CascadeOperations = CascadeOperation.All)]
		[JsonIgnore]
		public Page Parent { get; set; }

		[JsonProperty ("id")]
		[ForeignKey (typeof(Page))]
		public int Id { get; set; }

		[JsonIgnore]
		[OneToMany ("Id", CascadeOperations = CascadeOperation.All)]
		public List<Page> SubPages { get; set; }

		[JsonProperty ("title")]
		public string Title { get; set; }

		[JsonProperty ("type")]
		public string Type { get; set; }

		[JsonProperty ("status")]
		public string Status { get; set; }

		[JsonProperty ("automatic_translation")]
		public bool? AutoTranslated { get; set; }

		[JsonProperty ("modified_gmt")]
		[JsonConverter (typeof(DateConverter))]
		public DateTime Modified { get; set; }

		[JsonProperty ("excerpt")]
		public string Description { get; set; }


        [JsonProperty ("content")]
		public string Content { get; set; }

		[JsonProperty ("order")]
		public int Order { get; set; }

		[JsonProperty ("thumbnail")]
		public string Thumbnail { get; set; }

		[ForeignKey (typeof(Author))]
		public string AuthorKey { get; set; }

		[JsonProperty ("author")]
		[ManyToOne (CascadeOperations = CascadeOperation.All)]
		public Author Author { get; set; }

		[JsonProperty ("available_languages")]
		[JsonConverter (typeof(AvailableLanguageCollectionConverter))]
		[OneToMany (CascadeOperations = CascadeOperation.All)]
		public List<AvailableLanguage> AvailableLanguages { get; set; }

		[ForeignKey (typeof(Language))]
		public int LanguageId { get; set; }

		public Page ()
		{
		}

		public Page (int id, string title, string type, string status, DateTime modified, string excerpt, string content,
		                  int parentId, int order, string thumbnail, Author author, bool? autoTranslated,
		                  List<AvailableLanguage> availableLanguages)
		{
			Id = id;
			Title = title;
			Type = type;
			Status = status;
			Modified = modified;
			Description = excerpt;
			Content = content;
			ParentId = parentId;
			Order = order;
			Thumbnail = thumbnail;
			Author = author;
			AutoTranslated = autoTranslated;
			AvailableLanguages = availableLanguages;
			SubPages = new List<Page> ();
		}
        

        internal bool find(string searchText)
        {
            string pageString = (Title ?? "") + (Description ?? "");
            return pageString.ToLower().Contains((searchText ?? "").ToLower());
        }
    }

	internal class DateConverter : JsonConverter
	{
		public override void WriteJson (JsonWriter writer, object value, JsonSerializer serializer)
		{
			throw new NotImplementedException ();
		}

		public override bool CanConvert (Type type)
		{
			return Reflections.IsAssignableFrom (typeof(DateTime), type);
		}

		public override object ReadJson (JsonReader reader, Type objectType, object existingValue,
		                                      JsonSerializer serializer)
		{
			var readerValue = reader.Value.ToString ();
			return readerValue.DateTimeFromRestString ();
		} 
	}

	internal class AvailableLanguageCollectionConverter : JsonConverter
	{
		public override void WriteJson (JsonWriter writer, object value, JsonSerializer serializer)
		{
			throw new NotImplementedException ();
		}

		public override bool CanConvert (Type type)
		{
			return Reflections.IsAssignableFrom (typeof(List<AvailableLanguage>), type);
		}

		public override object ReadJson (JsonReader reader, Type objectType, object existingValue,
		                                      JsonSerializer serializer)
		{
			try {
				var dict = serializer.Deserialize<Dictionary<string, int>> (reader);
				return (from key in dict.Keys
				                    let value = dict [key]
				                    select new AvailableLanguage (key, value)).ToList ();
			} catch (Exception) {
				serializer.Deserialize (reader);
				return new List<AvailableLanguage> ();
			}
		}

    }
}

