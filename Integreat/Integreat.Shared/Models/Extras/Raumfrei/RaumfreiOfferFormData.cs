using Newtonsoft.Json;

namespace Integreat.Shared.Models.Extras.Raumfrei
{
    public class RaumfreiOfferFormData
    {
        [JsonProperty("landlord")]
        public RaumfreiLandlordInformation Landlord { get; set; }
        [JsonProperty("accommodation")]
        public RaumfreiAccommodationInformation Accommodation { get; set; }
        [JsonProperty("costs")]
        public RaumfreiCostsInformation Costs { get; set; }
    }
}
