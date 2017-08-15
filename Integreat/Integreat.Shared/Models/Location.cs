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

        [JsonProperty("ige-srl")]
        public string SerloEnabled { get; set; }

        [JsonProperty("ige-sbt")]
        public string SprungbrettExtras { get; set; }


        public string SprungbrettEnabled
        {
            get
            {
                try
                {
                    var isEnabled = (string)JObject.Parse(SprungbrettExtras)["enabled"];
                    return isEnabled;
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e);
                    return "";
                }

            }
        }
        public string SprungbrettUrl
        {
            get
            {
                try
                {
                    var url = (string)JObject.Parse(SprungbrettExtras)["url"];
                    return url;
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e);
                    return "";
                }
            }
        }

        [JsonProperty("ige-evts")]
        public string EventsEnabled { get; set; }

        [JsonProperty("ige-pn")]
        public string PushEnabled { get; set; }

        [JsonProperty("ige-c4r")]
        public string Careers4RefugeesExtras { get; set; }
        public string Careers4RefugeesEnabled
        {
            get
            {
                try
                {
                    var isEnabled = (string)JObject.Parse(Careers4RefugeesExtras)["enabled"];
                    return isEnabled;
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e);
                    return "";
                }
            }
        }
        public string Careers4RefugeesUrl
        {
            get
            {
                try
                {
                    var url = (string)JObject.Parse(Careers4RefugeesExtras)["url"];
                    url = url.Replace("https:/", "http:/"); //fix problem with https strings
                    return url;
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e);
                    return "";
                }
            }
        }

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
    }
}

