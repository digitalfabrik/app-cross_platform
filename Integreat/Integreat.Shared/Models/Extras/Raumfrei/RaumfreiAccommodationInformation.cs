using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Integreat.Shared.Models.Extras.Raumfrei
{
    public class RaumfreiAccommodationInformation
    {
        [JsonProperty("ofRooms")]
        public List<string> Rooms { get; set; }
        [JsonProperty("totalArea")]
        public float TotalArea { get; set; }
        [JsonProperty("totalRooms")]
        public int TotalRooms { get; set; }
        [JsonProperty("moveInDate")]
        public DateTime MoveInDate { get; set; }

        public List<String> TranslatedRooms {
            get
            {
                return Rooms.ConvertAll(translateKey);
            }
        }

        private string translateKey(String key) {
            switch(key)
            {
                case "kitchen": return "Küche";
                case "bath": return "Bad";
                case "wc": return "WC";
                case "child1": return "Kinderzimmer 1";
                case "child2": return "Kinderzimmer 2";
                case "child3": return "Kinderzimmer 3";
                case "bed": return "Schlafzimmer";
                case "hallway": return "Diele";
                case "store": return "Abstellraum";
                case "basement": return "Kellerraum";
                case "balcony": return "Balkon/Terrasse";
                default: return key;
            }
        }
    }
}