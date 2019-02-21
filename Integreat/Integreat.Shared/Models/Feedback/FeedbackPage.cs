using Newtonsoft.Json;

namespace Integreat.Shared.Models.Feedback
{
    public class FeedbackPage : IFeedback
    {
        [JsonProperty("id")]
        public int? Id { get; set; }

        [JsonProperty("permalink")]
        public string Permalink { get; set; }

        [JsonProperty("comment")]
        public string Comment { get; set; }

        [JsonProperty("rating")]
        public string Rating { get; set; }
    }
}
