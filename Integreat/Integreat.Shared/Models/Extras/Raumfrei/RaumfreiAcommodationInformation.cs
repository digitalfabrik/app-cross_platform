using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Integreat.Shared.Models.Extras.Raumfrei
{
    public class RaumfreiAcommodationInformation
    {
        [JsonProperty("ofRooms")]
        public List<string> Rooms { get; set; }
        [JsonProperty("totalArea")]
        public float TotalArea { get; set; }
        [JsonProperty("totalRooms")]
        public int TotalRooms { get; set; }
        [JsonProperty("moveInDate")]
        public DateTime MoveInDate { get; set; }
    }
}