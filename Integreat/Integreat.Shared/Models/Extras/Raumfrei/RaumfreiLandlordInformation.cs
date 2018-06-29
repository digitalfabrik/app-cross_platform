using Newtonsoft.Json;

namespace Integreat.Shared.Models.Extras.Raumfrei
{
    public class RaumfreiLandlordInformation
    {
        [JsonProperty("firstName")]
        public string FirstName { get; set; }
        [JsonProperty("lastName")]
        public string LastName { get; set; }
        [JsonProperty("phone")]
        public string PhoneNumber { get; set; }
        public string FullName => $"{FirstName} {LastName}";
    }
}