using System.Xml.Serialization;

namespace Vantiv.RequestModels
{
    [XmlRoot(ElementName = "PaymentAccountCreateWithTransID",Namespace = "https://services.elementexpress.com")]
    public class PaymentAccountCreateWithTransID
    {
        [XmlElement(ElementName = "Credentials")]
        public Credentials Credentials { get; set; }
        [XmlElement(ElementName = "Application")]
        public Application Application { get; set; }
        [XmlElement(ElementName = "PaymentAccount")]
        public PaymentAccount PaymentAccount { get; set; }
        [XmlElement(ElementName = "Transaction")]
        public Transaction Transaction { get; set; }
        [XmlAttribute(AttributeName = "xmlns")]
        public string Xmlns { get; set; }
        #region PRIMEPOS-2796
        [XmlElement(ElementName = "Terminal")]
        public Terminal Terminal { get; set; }
        #endregion
    }
}
