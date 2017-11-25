using System.Collections.Generic;
using System.Xml.Serialization;

namespace Integreat.Shared.Models.Extras.Careers4Refugees
{
    /// <summary>
    /// CareerOffer class to save and manipulate the xml elements
    /// </summary>
    [XmlType(TypeName = "anzeige")]
    public class CareerOffer : JobOfferBase
    {
        [XmlElement("id")]
        public string Id { get; set; }
        [XmlElement("interneId")]
        public string InternalId { get; set; }
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
