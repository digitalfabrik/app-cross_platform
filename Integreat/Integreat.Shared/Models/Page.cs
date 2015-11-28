using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Newtonsoft.Json;
using SQLite.Net.Attributes;

namespace Integreat.Models
{
	[Table("Page")]
	public class Page
	{   
		[PrimaryKey, Column("_id")]
        [JsonProperty("id")]
        public int Id {get;set; }

        [JsonProperty("title")]
        public string Title {get;set; }

        [JsonProperty("type")]
        public string Type{get;set; }

        [JsonProperty("status")]
        public string Status{get;set;}

        [JsonProperty("parent")]
        public int ParentId{get;set;}

        [JsonProperty("modified_gmt")]
        [JsonConverter(typeof(DateConverter))]
        public long Modified;

        [JsonProperty("excerpt")]
        public string Description{get;set;}

        [JsonProperty("content")]
        public string Content{get;set;}

        [JsonProperty("order")]
        public int Order{get;set;}
        
        [JsonProperty("thumbnail")]
        public string Thumbnail{get;set;}

        [JsonProperty("author")]
		public Author Author{get;set;}

        [JsonIgnore]
        public Page Parent;

        [JsonIgnore]
        public Collection<Page> SubPages;

        [JsonIgnore]
        public Collection<Page> AvailablePages;

        [JsonProperty("available_languages")]
        [JsonConverter(typeof(AvailableLanguageCollectionConverter))]
        public Collection<AvailableLanguage> AvailableLanguages;

        [JsonIgnore]
        public Language Language;

        public Page() { }

		public Page(int id, string title, string type, string status, long modified, string excerpt, string content, int parentId, int order, string thumbnail, Author author, Collection<AvailableLanguage> availableLanguages) {
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
			AvailableLanguages = availableLanguages;
			AvailablePages = new Collection<Page>();
			SubPages = new Collection<Page>();
		}
	}

    internal class DateConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override bool CanConvert(Type type)
        {
            return typeof(long).IsAssignableFrom(type);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var readerValue = (string) reader.Value;
            return readerValue?.DateTimeFromRestString().Ticks ?? 0;
        }
    }

    internal class AvailableLanguageCollectionConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override bool CanConvert(Type type)
        {
            return typeof(Collection<AvailableLanguage>).IsAssignableFrom(type);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var languages = new Collection<AvailableLanguage>();
            var dict = serializer.Deserialize<Dictionary<string, int>>(reader);
            foreach (var key in dict.Keys) { 
                var value = dict[key];
                languages.Add(new AvailableLanguage(key, value));
            }
            return languages;
        }
    }
}

