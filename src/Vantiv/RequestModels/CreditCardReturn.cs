using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Vantiv.RequestModels
{
    [XmlRoot(ElementName = "CreditCardReturn", Namespace = "https://transaction.elementexpress.com")]
    public class CreditCardReturn
    {
        [XmlElement(ElementName = "Credentials")]
        public Credentials Credentials { get; set; }
        [XmlElement(ElementName = "Application")]
        public Application Application { get; set; }
        [XmlElement(ElementName = "Transaction")]
        public Transaction Transaction { get; set; }
        [XmlElement(ElementName = "Terminal")]
        public Terminal Terminal { get; set; }
        [XmlElement(ElementName = "PaymentAccount")]
        public PaymentAccount PaymentAccount { get; set; }
        [XmlAttribute(AttributeName = "xmlns")]
        public string Xmlns { get; set; }
    }
}
