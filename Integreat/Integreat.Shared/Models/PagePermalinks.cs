using System.Collections.Generic;
using Newtonsoft.Json;

namespace Integreat.Shared.Models
{
    /// <summary>
    /// Permalinks for a page. (Which are the URLs for the webview)
    /// </summary>
    public class PagePermalinks
    {
        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("url_site")]
        public string UrlSite { get; set; }

        [JsonProperty("url_page")]
        public string UrlPage { get; set; }

        [JsonProperty("url_page_id")]
        public string UrlPageId { get; set; }

        [JsonProperty("url_date_1_name")]
        public string UrlDate1Name { get; set; }

        [JsonProperty("url_date_2_name")]
        public string UrlDate2Name { get; set; }

        /// <summary>
        /// Gets all URLs in a list.
        /// </summary>
        /// <value>
        /// All permalink URLs in a list.
        /// </value>
        [JsonIgnore]
        public List<string> AllUrls => new List<string> {Url, UrlSite, UrlPage, UrlPageId, UrlDate1Name, UrlDate2Name};
    }
}
