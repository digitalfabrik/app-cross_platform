using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using System.Xml.Serialization;

namespace Integreat.Shared.Utilities
{
    public class Careers4RefugeesTemp
    {
        //Constructor to initialize Parser
        public Careers4RefugeesTemp() { }

        public static async void Test()
        {
            var offers = await XmlWebParser.ParseXmlFromAddressAsync<List<CareerOffer>>("http://www.careers4refugees.de/jobsearch/exports/integreat_regensburg", "anzeigen");
        }

        public Task<List<CareerOffer>> GetCareerList()
        {
            var offers = XmlWebParser.ParseXmlFromAddressAsync<List<CareerOffer>>("http://www.careers4refugees.de/jobsearch/exports/integreat_regensburg", "anzeigen");
            return offers;
        }

        //CareerOffer class to save and manipulate the xml elements
        [XmlType(TypeName = "anzeige")]
        public class CareerOffer
        {
            [System.Xml.Serialization.XmlElement("id")]
            public string ID { get; set; }
            [System.Xml.Serialization.XmlElement("interneId")]
            public string _internalID { get; set; }
            [System.Xml.Serialization.XmlElement("titel")]
            public string _jobTitle { get; set; }
            [System.Xml.Serialization.XmlElement("firma")]
            public string _companyName { get; set; }
            [XmlArray("regionen")]
            [XmlArrayItem("region", Type = typeof(Region))]
            public List<Region> regions { get; set; }
            [System.Xml.Serialization.XmlElement("beschreibungOrt")]
            public string _placeDescription { get; set; }
            [System.Xml.Serialization.XmlElement("link")]
            public string _link { get; set; }
            [System.Xml.Serialization.XmlElement("bewerbungslink")]
            public string _offerlink { get; set; }
            [System.Xml.Serialization.XmlElement("laufendesDatum")]
            public string _date { get; set; }
            [System.Xml.Serialization.XmlElement("volltext")]
            public string _text { get; set; }
        }
        //CareerOffer class to save and manipulate the region sub elements
        [XmlType(TypeName = "region")]
        public class Region
        {
            [System.Xml.Serialization.XmlAttribute("typ")]
            public string _type { get; set; }
            [System.Xml.Serialization.XmlAttribute("land")]
            public string _country { get; set; }
            [System.Xml.Serialization.XmlAttribute("plz")]
            public string _zipcode { get; set; }
            [System.Xml.Serialization.XmlText]
            public string _content { get; set; }
        }
    }
}

