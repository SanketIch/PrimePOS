using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace Vantiv.RequestModels
{
    public class Healthcare
    {
        [DataMember(Name = "clinic")]
        [XmlElement("clinic")]
        public string clinic { get; set; }
        [DataMember(Name = "dental")]
        [XmlElement("dental")]
        public string dental { get; set; }
        [DataMember(Name = "prescription")]
        [XmlElement("prescription")]
        public string prescription { get; set; }
        [DataMember(Name = "total")]
        [XmlElement("total")]
        public string total { get; set; }
        [DataMember(Name = "vision")]
        [XmlElement("vision")]
        public string vision { get; set; }
    }
}
