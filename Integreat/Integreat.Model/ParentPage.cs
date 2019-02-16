using Newtonsoft.Json;

namespace Integreat.Model
{
    /// <summary>
    /// This class is just for the Page Model to be parsed correctly
    /// </summary>
    public class ParentPage
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("path")]
        public string Path { get; set; }
    }
}