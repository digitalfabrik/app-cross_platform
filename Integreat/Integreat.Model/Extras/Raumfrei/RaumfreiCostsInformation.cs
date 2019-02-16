using System.Collections.Generic;
using Newtonsoft.Json;

namespace Integreat.Model.Extras.Raumfrei
{
    public class RaumfreiCostsInformation
    {
        [JsonProperty("ofRunningServices")]
        public List<string> RunningServices { get; set; }
        [JsonProperty("ofRunningServicesDiff")]
        public List<string> NotRunningServices { get; set; }
        [JsonProperty("ofAdditionalServices")]
        public List<string> AdditionalServices { get; set; }
        [JsonProperty("ofAdditionalServicesDiff")]
        public List<string> NotAdditionalServices { get; set; }
        [JsonProperty("baseRent")]
        public float BaseRent { get; set; }
        [JsonProperty("runningCosts")]
        public float RunningCosts { get; set; }
        [JsonProperty("hotWaterInRunningCosts")]
        public bool HotWaterInRunningCosts { get; set; }
        [JsonProperty("additionalCosts")]
        public float AdditionalCosts { get; set; }

        public string TranslatedNotRunningServices => string.Join(", ", NotRunningServices.ConvertAll(translateKey));
        public string TranslatedRunningServices => string.Join(", ", RunningServices.ConvertAll(translateKey));
        public string TranslatedAdditionalServices => string.Join(", ", AdditionalServices.ConvertAll(translateKey));
        public string TranslatedNotAdditionalServices => string.Join(", ", NotAdditionalServices.ConvertAll(translateKey));
        private string translateKey(string key)
        {
            switch (key)
            {
                case "heating": return "Heizung";
                case "water": return "Wasser";
                case "garbage": return "Abfall";
                case "chimney": return "Kaminkehrer";
                case "other": return "Sonstiges";
                case "garage": return "Garage/Stellplatz";
                default: return key;
            }
        }
    }
}