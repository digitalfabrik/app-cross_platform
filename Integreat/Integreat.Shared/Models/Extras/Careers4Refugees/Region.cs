using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using Integreat.Shared.Utilities;

namespace Integreat.Shared.Models.Extras.Careers4Refugees
{
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
