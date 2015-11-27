using System.Collections.ObjectModel;
using Newtonsoft.Json;
using SQLite.Net.Attributes;

namespace Integreat
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

		public Page Parent;
		public Collection<Page> SubPages;
		public Collection<Page> AvailablePages;

        [JsonProperty("available_languages")]
        [JsonConverter(typeof(AvailableLanguageCollectionConverter))]
        public Collection<AvailableLanguage> AvailableLanguages;

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

    internal class DateConverter
    {
    }
    internal class AvailableLanguageCollectionConverter
    {
    }
}

