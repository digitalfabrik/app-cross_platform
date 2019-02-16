using System;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Integreat.Model
{
    /// <summary>
    /// Describes a location in our data model.
    /// </summary>
    public class Location
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("icon")]
        public string Icon { get; set; }

        [JsonProperty("cover_image")]
        public string CityImage { get; set; }

        [JsonProperty("color")]
        public string Color { get; set; }

        [JsonProperty("path")]
        public string Path { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("live")]
        public bool Live { get; set; }

        [JsonProperty("prefix")]
        public string Prefix { get; set; }

        [JsonProperty("name_without_prefix")]
        public string NameWithoutStreetPrefix { get; set; }

        [JsonProperty("plz")]
        public string Zip { get; set; }

        [JsonProperty("extras")]
        public bool ExtrasEnabled { get; set; }

        [JsonProperty("events")]
        public bool EventsEnabled { get; set; }

        [JsonProperty("ige-pn")]
        public bool PushEnabled { get; set; }

        /// <summary>
        /// Gets the key to group locations, which is just the first letter of the name (uppercase) however with removed prefixes.
        /// </summary>
        public string GroupKey => NameWithoutStreetPrefix.ElementAt(0).ToString().ToUpper();

        public override string ToString() => string.IsNullOrEmpty(Path) ? "" : Path.Replace("/", ""); // return the path without slashes

        public bool Find(string searchText)
        {
            if (!Live)
            {
#if DEBUG
                return "q".Equals(searchText.ToLower()); // in Debugging mode only enter 1 to get all instances
#else
                return "wirschaffendas".Equals(searchText);
#endif
            }
            var locationString = (Description ?? "") + (Name ?? "");
            return locationString.ToLower().Contains((searchText ?? "").ToLower());
        }

        /// <summary> URLs the or empty string. </summary>
        /// <param name="property">The property to find in the json.</param>
        /// <returns>The url or an empty string.</returns>
        private static string UrlOrEmptyString(string property)
        {
            try
            {
                var url = (string)JObject.Parse(property)["url"];
                url = url.Replace("https:/", "http:/"); //fix problem with https strings
                return url;
            }
            catch (Exception)
            {
                return "";
            }
        }

        /// <summary> Determines whether [is enabled safe] [the specified property]. </summary>
        /// <param name="property">The property to find in json.</param>
        /// <returns>"1" if is enabled, else "0"</returns>
        private static string IsEnabledSafe(string property)
        {
            try
            {
                var isEnabled = (string)JObject.Parse(property)["enabled"];
                return isEnabled;
            }
            catch (Exception)
            {
                return "0";
            }
        }
    }
}