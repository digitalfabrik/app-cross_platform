using System;
using System.Xml.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using System.Xml.Serialization;

namespace Integreat.Shared.Utilities
{
    static class XmlWebParser
    {

        public static async Task<T> ParseXmlFromAddressAsync<T>(string address, string rootName)
        {
            // get the content from the address
            var uri = new Uri(address);
            var content = await GetResponseText(address);

            // parse it into a XDocument
            var doc = XDocument.Parse(content);
            
            // create serializer
            var xmlSerializer = new XmlSerializer(typeof(T), new XmlRootAttribute(rootName));

            // create reader from the Root, use the serializer to parse it, return the result
            using (var reader = doc.Root.CreateReader())
            {
                return (T)xmlSerializer.Deserialize(reader);
            }
        }

        /*public static async Task<string> GetXmlAsString(Source uri)
        {
            var httpclient = new HttpClient();
            var response = await httpclient.GetAsync(uri);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return content;
            }
            throw (new ArgumentException("The given Source did not resolve to a valid address"));
        }*/

        public static async Task<string> GetResponseText(string address)
        {
            using (var httpClient = new HttpClient())
                return await httpClient.GetStringAsync(address);
        }
    }
}
