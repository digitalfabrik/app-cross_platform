using System;
using Newtonsoft.Json;

namespace Integreat.Shared.Models.Feedback
{
    public class FeedbackPage : Feedback
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("permalink")]
        public string Permalink { get; set; }
    }
}
