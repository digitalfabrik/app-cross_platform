using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Integreat.Shared.Models.Extras.Raumfrei
{
    public class RaumfreiOfferFormData
    {
        [JsonProperty("landlord")]
        public RaumfreiLandlordInformation LandlordInformation { get; set; }
        [JsonProperty("accommodation")]
        public RaumfreiAcommodationInformation AccommodationInformation { get; set; }
        [JsonProperty("costs")]
        public RaumfreiCostsInformation CostsInformation { get; set; }
    
    }
}
