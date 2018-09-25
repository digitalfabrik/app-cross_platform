using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Integreat.Shared.Utilities;
using Newtonsoft.Json.Linq;
using System.Diagnostics;
using System.Security;

namespace Integreat.Shared.Models
{
    /// <summary>
    /// Describes a page in our data model. A page may contain Content and other pages as children.
    /// </summary>
    [SecuritySafeCritical]
    public class Page
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("path")]
        public string Path { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("modified_gmt")]
        [JsonConverter(typeof(DateConverter))]
        public DateTime Modified { get; set; }

        [JsonProperty("excerpt")]
        public string Description { get; set; }

        [JsonProperty("content")]
        public string Content { get; set; }

        [JsonProperty("parent")]
        public ParentPage ParentPage { get; set; }

        [JsonProperty("order")]
        public int Order { get; set; }

        [JsonProperty("available_languages")]
        public Dictionary<string, ParentPage> AvailableLanguages { get; set; }

        [JsonProperty("thumbnail")]
        public string Thumbnail { get; set; }

        [JsonProperty("hash")]
        public string Hash { get; set; }


        internal bool Find(string searchText)
        {
            var pageString = (Title ?? "") + (Description ?? "");
            return pageString.ToLower().Contains((searchText ?? "").ToLower());
        }
    }

    /// <inheritdoc />
    /// <summary>  Special converter used to convert the Date in REST format to our DateTime format and vice-versa </summary>
    [SecurityCritical]
    internal class DateConverter : JsonConverter
    {
        [SecurityCritical]
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var dt = value as DateTime? ?? new DateTime();
            writer.WriteValue(dt.ToRestAcceptableString());
        }

        [SecurityCritical]
        public override bool CanConvert(Type objectType)
        {
            return Reflections.IsAssignableFrom(typeof(DateTime), objectType);
        }

        [SecurityCritical]
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue,
            JsonSerializer serializer)
        {
            try
            {
                // try to parse the value
                var readerValue = reader.Value.ToString();
                return readerValue.DateTimeFromRestString();
            }
            catch (Exception)
            {
                // as this may fail, when the stored DateTime was in a different format than the current culture, we catch this and return null instead. 
                return null;
            }
        }
    }
}