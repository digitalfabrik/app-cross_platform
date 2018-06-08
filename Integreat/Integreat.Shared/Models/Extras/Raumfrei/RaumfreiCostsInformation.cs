using Newtonsoft.Json;
using System.Collections.Generic;

namespace Integreat.Shared.Models.Extras.Raumfrei
{
    public class RaumfreiCostsInformation
    {
        [JsonProperty("ofRunningServices")]
        public List<string> RunningServices { get; set; }
        [JsonProperty("additionalServices")]
        public List<string> AdditionalServices { get; set; }
        [JsonProperty("baseRent")]
        public float BaseRent { get; set; }
        [JsonProperty("runningCosts")]
        public float RunningCosts { get; set; }
        [JsonProperty("hotWaterInRunningCosts")]
        public bool HotWaterInRunningCosts { get; set; }
        [JsonProperty("additionalCosts")]
        public float AdditionalCosts { get; set; }
    }
}