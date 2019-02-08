using System;
using Newtonsoft.Json;

namespace Integreat.Shared.Models.Feedback
{
    public class FeedbackSearch : Feedback
    {
        [JsonProperty("query")]
        public string Query { get; set; }
    }
}
