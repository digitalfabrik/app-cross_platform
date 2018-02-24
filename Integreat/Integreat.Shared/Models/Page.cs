using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Integreat.Shared.Utilities;
using System.Security;

namespace Integreat.Shared.Models
{
    /// <summary>
    /// Describes a page in our data model. A page may contain Content and other pages as children.
    /// </summary>
    [SecuritySafeCritical]
    public class Page
    {
        [JsonProperty("parent")]
        public string ParentJsonId { get; set; }

        [JsonProperty("permalink")]
        public PagePermalinks Permalinks { get; set; }

        [JsonProperty("parentId")]
        public string ParentId { get; set; }

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("modified_gmt")]
        [JsonConverter(typeof(DateConverter))]
        public DateTime Modified { get; set; }

        [JsonProperty("excerpt")]
        public string Description { get; set; }

        [JsonProperty("content")]
        public string Content { get; set; }

        [JsonProperty("order")]
        public int Order { get; set; }

        [JsonProperty("thumbnail")]
        public string Thumbnail { get; set; }

        [JsonProperty("author")]
        public Author Author { get; set; }

        [JsonProperty("available_languages")]
        [JsonConverter(typeof(AvailableLanguageCollectionConverter))]
        public List<AvailableLanguage> AvailableLanguages { get; set; }

        public string PrimaryKey { get; set; }

        internal bool Find(string searchText)
        {
            var pageString = (Title ?? "") + (Description ?? "");
            return pageString.ToLower().Contains((searchText ?? "").ToLower());
        }

        public static string GenerateKey(object id, Location location, Language language)
        {
            if (location == null || language == null) return "";
            return id + "_" + language.Id + "_" + location.Id;
        }
    }
}