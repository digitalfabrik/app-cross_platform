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
            Uri uri = new Uri(address);
            var content = await GetXmlAsString(uri);

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

        public static async Task<string> GetXmlAsString(Uri uri)
        {
            var httpclient = new HttpClient();
            var response = await httpclient.GetAsync(uri);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return content;
            }
            throw (new ArgumentException("The given Uri did not resolve to a valid address"));
        }
    }
}
