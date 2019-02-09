using System;
using Newtonsoft.Json;

namespace Integreat.Shared.Models.Feedback
{
    public class FeedbackGeneral : IFeedback
    {
        [JsonProperty("comment")]
        public string Comment { get; set; }

        [JsonProperty("rating")]
        public string Rating { get; set; }
    }
}
