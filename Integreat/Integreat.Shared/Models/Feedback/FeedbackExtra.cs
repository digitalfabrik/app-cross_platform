using System;
using Newtonsoft.Json;

namespace Integreat.Shared.Models.Feedback
{
    public class FeedbackExtra : Feedback
    {
        [JsonProperty("alias")]
        public int Alias { get; set; }
    }
}
