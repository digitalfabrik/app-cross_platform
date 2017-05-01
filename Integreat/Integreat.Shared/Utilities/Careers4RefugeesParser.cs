using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using System.Xml.Serialization;
using Xamarin.Forms;

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
            public string InternalID { get; set; }
            [System.Xml.Serialization.XmlElement("titel")]
            public string JobTitle { get; set; }
            [System.Xml.Serialization.XmlElement("firma")]
            public string CompanyName { get; set; }
            [XmlArray("regionen")]
            [XmlArrayItem("region", Type = typeof(Region))]
            public List<Region> Regions { get; set; }
            [System.Xml.Serialization.XmlElement("beschreibungOrt")]
            public string PlaceDescription { get; set; }
            [System.Xml.Serialization.XmlElement("link")]
            public string Link { get; set; }
            [System.Xml.Serialization.XmlElement("bewerbungslink")]
            public string Offerlink { get; set; }
            [System.Xml.Serialization.XmlElement("laufendesDatum")]
            public string Date { get; set; }
            [System.Xml.Serialization.XmlElement("volltext")]
            public string Text { get; set; }

            [XmlIgnore]
            public Command OnTapCommand { get; set; }
        }
        //CareerOffer class to save and manipulate the region sub elements
        [XmlType(TypeName = "region")]
        public class Region
        {
            [XmlAttribute("typ")]
            public RegionType Type { get; set; }
            [System.Xml.Serialization.XmlAttribute("land")]
            public string Country { get; set; }
            [System.Xml.Serialization.XmlAttribute("plz")]
            public string Zipcode { get; set; }
            [System.Xml.Serialization.XmlText]
            public string Content { get; set; }
        }
        public enum RegionType
        {
            [XmlEnum(Name = "")]
            Unknown = 0, // default fallback value
            [XmlEnum(Name = "PLZ")]
            PostalCode,
            [XmlEnum(Name = "ORT")]
            Place,
            [XmlEnum(Name = "GEBIET")]
            Region
        }
    }
}

