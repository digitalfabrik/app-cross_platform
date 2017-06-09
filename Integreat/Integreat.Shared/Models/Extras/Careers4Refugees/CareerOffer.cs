using System.Collections.Generic;
using System.Xml.Serialization;

using Integreat.Shared.Utilities;
using Xamarin.Forms;


namespace Integreat.Shared.Models.Extras.Careers4Refugees
{
    //CareerOffer class to save and manipulate the xml elements
    [XmlType(TypeName = "anzeige")]
    public class CareerOffer : JobOfferBase
    {
        [XmlElement("id")]
        public string ID { get; set; }
        [XmlElement("interneId")]
        public string InternalID { get; set; }
        [XmlElement("titel")]
        public string JobTitle { get; set; }
        [XmlElement("firma")]
        public string CompanyName { get; set; }
        [XmlArray("regionen")]
        [XmlArrayItem("region", Type = typeof(Region))]
        public List<Region> Regions { get; set; }
        [XmlElement("beschreibungOrt")]
        public string City { get; set; }
        [XmlElement("link")]
        public string Link { get; set; }
        [XmlElement("bewerbungslink")]
        public string Offerlink { get; set; }
        [XmlElement("laufendesDatum")]
        public string Date { get; set; }
        [XmlElement("volltext")]
        public string Text { get; set; }

    }
}
