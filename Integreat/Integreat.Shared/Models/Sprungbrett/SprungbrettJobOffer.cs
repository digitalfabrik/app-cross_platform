using Newtonsoft.Json;
using Xamarin.Forms;

namespace Integreat.Shared.Models.Sprungbrett
{

    //JobOffer class to save and manipulate the json elements
    public class SprungbrettJobOffer
    {
        [JsonProperty("title")]
        public string Title { get; set; }
        [JsonProperty("apprenticeship")]
        public string Apprenticeship { get; set; }
        [JsonProperty("employment")]
        public string Employment { get; set; }
        [JsonProperty("company")]
        public string Company { get; set; }
        [JsonProperty("street")]
        public string Street { get; set; }
        [JsonProperty("zip")]
        public string Zip { get; set; }
        [JsonProperty("city")]
        public string City { get; set; }
        [JsonProperty("lat")]
        public string Lat { get; set; }
        [JsonProperty("lon")]
        public string Lon { get; set; }
        [JsonProperty("schooltypegroup")]
        public string Schooltypegroup { get; set; }
        [JsonProperty("distance")]
        public string Distance { get; set; }
        [JsonProperty("url")]
        public string Url { get; set; }

        public Command OnTapCommand { get; set; }
    }

}