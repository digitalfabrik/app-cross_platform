using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using SQLite.Net.Attributes;
using SQLiteNetExtensions.Attributes;
using Integreat.Shared.Utilities;
using Newtonsoft.Json.Linq;

namespace Integreat.Shared.Models
{
	[Table ("Page")]
	public class Page
	{
		[PrimaryKey]
        [ForeignKey(typeof(Page))]
        public string PrimaryKey { get; set; }

        [JsonProperty("parent")]
        public string ParentJsonId { get; set; }
        
		public string ParentId { get; set; }

		[JsonProperty ("id")]
		public int Id { get; set; }

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

        [ManyToOne]
        public Language Language {get;set;}

		[ForeignKey (typeof(Language))]
		public string LanguageId { get; set; }

		public Page ()
		{
		}

		public Page (string primaryKey, int id, string title, string type, string status, DateTime modified, string excerpt, string content,
		                  string parentId, int order, string thumbnail, Author author, bool? autoTranslated,
		                  List<AvailableLanguage> availableLanguages)
		{
            PrimaryKey = primaryKey;
			Id = id;
			Title = title;
			Type = type;
			Status = status;
			Modified = modified;
			Description = excerpt;
			Content = content;
			Order = order;
			Thumbnail = thumbnail;
            ParentId = parentId;
			Author = author;
			AutoTranslated = autoTranslated;
			AvailableLanguages = availableLanguages;
		}
        

        internal bool Find(string searchText)
        {
            var pageString = (Title ?? "") + (Description ?? "");
            return pageString.ToLower().Contains((searchText ?? "").ToLower());
        }

        public static string GenerateKey(object id, Location location, Language language)
        {
            return id + "_" + language.Id + "_" + location.Id;
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

	    public override object ReadJson(JsonReader reader, Type objectType, object existingValue,
	        JsonSerializer serializer)
	    {
	        try
	        {
	            var dict2 = serializer.Deserialize(reader) as JObject;
	            return dict2 == null ? new List<AvailableLanguage>() : (from jToken in dict2?.Properties() select new AvailableLanguage(jToken.Name, int.Parse(jToken.Value.ToString()))).ToList();
	        }
	        catch (Exception e)
	        {
	            Console.WriteLine(e);
	            serializer.Deserialize(reader);
	            return new List<AvailableLanguage>();
	        }
	    }

	}
}

