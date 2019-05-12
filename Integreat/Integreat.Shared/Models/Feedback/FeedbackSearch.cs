using Newtonsoft.Json;

namespace Integreat.Shared.Models.Feedback
{
    public class FeedbackSearch : IFeedback
    {
        [JsonProperty("query")]
        public string Query { get; set; }

        [JsonProperty("comment")]
        public string Comment { get; set; }

        [JsonProperty("rating")]
        public string Rating { get; set; }
    }
}
