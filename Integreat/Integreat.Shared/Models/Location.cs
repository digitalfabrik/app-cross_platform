using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Integreat.Shared.Models
{
    /// <summary>
    /// Describes a location in our data model.
    /// </summary>
    public class Location
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("live")]
        public bool Live { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("icon")]
        public string Icon { get; set; }

        [JsonProperty("path")]
        public string Path { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("color")]
        public string Color { get; set; }

        [JsonProperty("cover_image")]
        public string CityImage { get; set; }

        [JsonProperty("latitude")]
        public float Latitude { get; set; }

        [JsonProperty("longitude")]
        public float Longitude { get; set; }

        [JsonProperty("ige-evts")]
        public string EventsEnabled { get; set; }

        [JsonProperty("ige-pn")]
        public string PushEnabled { get; set; }

        //
        //  Location Extras
        //

        [JsonProperty("ige-srl")]
        public string SerloEnabled { get; set; }

        [JsonProperty("ige-sbt")]
        public string SprungbrettExtras { get; set; }
        public string SprungbrettEnabled => IsEnabledSafe(SprungbrettExtras);
        public string SprungbrettUrl => UrlOrEmptyString(SprungbrettExtras);

        [JsonProperty("ige-c4r")]
        public string Careers4RefugeesExtras { get; set; }
        public string Careers4RefugeesEnabled => IsEnabledSafe(Careers4RefugeesExtras);
        public string Careers4RefugeesUrl => UrlOrEmptyString(Careers4RefugeesExtras);

        [JsonProperty("ige-ilb")]
        public string IhkApprenticeshipsExtras { get; set; }
        public string IhkApprenticeshipsEnabled => IsEnabledSafe(IhkApprenticeshipsExtras);
        public string IhkApprenticeshipsUrl => UrlOrEmptyString(IhkApprenticeshipsExtras);

        [JsonProperty("ige-ipb")]
        public string IhkInternshipsExtras { get; set; }
        public string IhkInternshipsEnabled => IsEnabledSafe(IhkInternshipsExtras);
        public string IhkInternshipsUrl => UrlOrEmptyString(IhkInternshipsExtras);

        [JsonProperty("ige-lr")]
        public string LehrstellenRadarEnabled { get; set; }

        [JsonProperty("ige-zip")]
        public string Zip { get; set; }

        /// <summary>
        /// Gets the key to group locations, which is just the first letter of the name (uppercase) however with removed prefixes.
        /// </summary>
        public string GroupKey => NameWithoutStreetPrefix.ElementAt(0).ToString().ToUpper();

        /// <summary>
        /// Removes the street prefixes from the string "Stadt ", "Landkreis ", "Kreis " & "Gemeinde ".
        /// </summary>
        public string NameWithoutStreetPrefix => Regex.Replace(Name, "(Stadt |Gemeinde |Landkreis |Kreis )", "");

        public override string ToString() => Path.Replace("/", ""); // return the path without slashes

        public bool Find(string searchText)
        {
            if (!Live)
            {
                return "wirschaffendas".Equals(searchText);
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