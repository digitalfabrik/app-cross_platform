using Integreat.Shared.Models.Extras.Raumfrei;
using Newtonsoft.Json;
using System;

namespace Integreat.Shared.Models.Extras.Sprungbrett
{
    /// <summary>
    /// RaumfreiOffer class to save and manipulate the json elements from the integreat wohnraumboerse api
    /// </summary>
    public class RaumfreiOffer : JobOfferBase
    {
        [JsonProperty("email")]
        public string EmailAddress { get; set; }
        [JsonProperty("formData")]
        public RaumfreiOfferFormData FormData { get; set; }
        [JsonProperty("createdDate")]
        public DateTime CreatedDate { get; set; }

    }
}