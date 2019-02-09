using Newtonsoft.Json;
using System;

namespace Integreat.Shared.Models.Extras.Raumfrei
{
    /// <summary>
    /// RaumfreiOffer class to save and manipulate the json elements from the integreat wohnraumboerse api
    /// </summary>
    public class RaumfreiOffer
    {
        [JsonProperty("email")]
        public string EmailAddress { get; set; }
        [JsonProperty("formData")]
        public RaumfreiOfferFormData FormData { get; set; }
        [JsonProperty("createdDate")]
        public DateTime CreatedDate { get; set; }
    }
}