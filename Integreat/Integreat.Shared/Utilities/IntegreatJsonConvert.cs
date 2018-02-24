using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Security;
using Integreat.Shared.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Integreat.Shared.Utilities
{
    /// <summary>
    /// JsonConverter Abstraktion layer
    /// </summary>
    public static class IntegreatJsonConvert 
    {
        /// <summary>
        /// Deserializes the JSON to a .NET object.   
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="jsonToDeserialize">The JSON to deserialize.</param>
        /// <returns>The deserialized object from the JSON string.</returns>
        public static T DeserializeObject<T>(string jsonToDeserialize)
        {
            return JsonConvert.DeserializeObject<T>(jsonToDeserialize);
        }

        /// <summary>
        /// Serializes the specified object to a JSON string.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="receivedList">The object to serialize.</param>
        /// <returns>A JSON string representation of the object.</returns>
        public static string SerializeObject<T>(T receivedList)
        {
            return JsonConvert.SerializeObject(receivedList);
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

    /// <inheritdoc />
    /// <summary> 
    /// Converter used to resolve full page id's for the given other page id's 
    /// </summary>
    internal class AvailableLanguageCollectionConverter : JsonConverter
    {

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (!(value is List<AvailableLanguage> asList)) return;
            var props = (from lang in asList
                         select new JProperty(lang.LanguageId, lang.OtherPageId));
            var jObject = new JObject(props);
            serializer.Serialize(writer, jObject);
        }

        public override bool CanConvert(Type objectType)
        {
            return Reflections.IsAssignableFrom(typeof(List<AvailableLanguage>), objectType);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue,
            JsonSerializer serializer)
        {
            try
            {
                return !(serializer.Deserialize(reader) is JObject dict2)
                    ? new List<AvailableLanguage>()
                    : (from jToken in dict2.Properties()
                       select new AvailableLanguage(jToken.Name, jToken.Value.ToString())).ToList();
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                serializer.Deserialize(reader);
                return new List<AvailableLanguage>();
            }
        }
    }
}
