using Integreat.Shared.Utilities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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



        [JsonProperty("available_languages", NullValueHandling = NullValueHandling.Ignore)]
        [JsonConverter(typeof(AvailableLanguageCollectionConverter))]
        public List<AvailableLanguageObject> AvailableLanguages { get; set; }

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

    public class AvailableLanguageObject
    {
        public string Id { get; set; }
        public ParentPage ParentPage { get; set; }
    }
    /// <inheritdoc />
    /// <summary>  Special converter used to convert the Date in REST format to our DateTime format and vice-versa </summary>
    internal class DateConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var dt = value as DateTime? ?? new DateTime();
            writer.WriteValue(dt.ToRestAcceptableString());
        }

        public override bool CanConvert(Type objectType)
        {
            return Reflections.IsAssignableFrom(typeof(DateTime), objectType);
        }

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

    public class AvailableLanguageCollectionConverter : JsonConverter
    {
        [SecurityCritical]
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (!(value is List<AvailableLanguageObject> asList)) return;
            try
            {
                var props = (from lang in asList
                             select new JProperty(lang.Id, (JObject)JToken.FromObject(lang.ParentPage)));
                var jObject = new JObject(props);
                serializer.Serialize(writer, jObject);
            }
            catch(Exception e){
                Debug.WriteLine(e);
            }
        }

        [SecurityCritical]
        public override bool CanConvert(Type objectType)
        {
            return Reflections.IsAssignableFrom(typeof(List<AvailableLanguageObject>), objectType);
        }

        [SecurityCritical]
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue,
            JsonSerializer serializer)
        {
            try
            {
                List<AvailableLanguageObject> availableLanguages = new List<AvailableLanguageObject>();
                foreach(JProperty jProperty in ((JObject)serializer.Deserialize(reader)).Properties()){
                    //just for debuggin purposes
                    if(jProperty.Value["id"].ToString() == "2705"){
                        Debug.WriteLine("so someting");
                    }
                    AvailableLanguageObject availableLanguageObject = new AvailableLanguageObject
                    {
                        Id = jProperty.Name
                    };
                    int id;
                    string sHelper = jProperty.Value["id"].ToString();
                    Int32.TryParse(sHelper, out id);
                    ParentPage pp = new ParentPage
                    {
                        Id = id,
                        Url = jProperty.Value["url"].ToString(),
                        Path = jProperty.Value["path"].ToString()
                    };
                    availableLanguageObject.ParentPage = pp;
                    availableLanguages.Add(availableLanguageObject);
                }
                return availableLanguages;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                serializer.Deserialize(reader);
                return new List<AvailableLanguageObject>();
            }
        }
    }
}