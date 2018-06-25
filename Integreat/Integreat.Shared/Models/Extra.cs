using System;
using System.Collections.Generic;
using Integreat.Shared.ViewFactory;
using Newtonsoft.Json;
using Xamarin.Forms;

namespace Integreat.Shared.Models
{
    /// <summary>
    /// Describes a extra entry, which are additional third party features.
    /// </summary>
    public class Extra
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("alias")]
        public string Alias { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("post")]
        public IDictionary<string, string> Post { get; set; }

        [JsonProperty("thumbnail")]
        public string Thumbnail { get; set; }

        public override string ToString() => Name;
        public string Title { get; set; }

        public Command OnTapCommand { get; set; }

        public Func<IViewModel> ViewModelFactory { get; set; }


    }
}
