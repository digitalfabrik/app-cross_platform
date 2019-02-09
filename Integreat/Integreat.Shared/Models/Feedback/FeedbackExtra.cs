using System;
using Newtonsoft.Json;

namespace Integreat.Shared.Models.Feedback
{
    public class FeedbackExtra : IFeedback
    {
        [JsonProperty("alias")]
        public string Alias { get; set; }

        [JsonProperty("comment")]
        public string Comment { get; set; }

        [JsonProperty("rating")]
        public string Rating { get; set; }
    }
}
