using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Vantiv.RequestModels;

namespace Vantiv.ResponseModels
{
    [XmlRoot(ElementName = "Card")]
    public class Card
    {
        [XmlElement(ElementName = "ExpirationMonth")]
        public string ExpirationMonth { get; set; }
        [XmlElement(ElementName = "ExpirationYear")]
        public string ExpirationYear { get; set; }
        [XmlElement(ElementName = "CardNumberMasked")]
        public string CardNumberMasked { get; set; }
        [XmlElement(ElementName = "CardLogo")]
        public string CardLogo { get; set; }

    }


    [XmlRoot(ElementName = "Response")]
    public class Response
    {
        [XmlElement(ElementName = "ExpressResponseCode")]
        public string ExpressResponseCode { get; set; }
        [XmlElement(ElementName = "ExpressResponseMessage")]
        public string ExpressResponseMessage { get; set; }
        [XmlElement(ElementName = "ExpressTransactionDate")]
        public string ExpressTransactionDate { get; set; }
        [XmlElement(ElementName = "ExpressTransactionTime")]
        public string ExpressTransactionTime { get; set; }
        [XmlElement(ElementName = "ExpressTransactionTimezone")]
        public string ExpressTransactionTimezone { get; set; }
        [XmlElement(ElementName = "HostResponseMessage")]
        public string HostResponseMessage { get; set; }
        [XmlElement(ElementName = "HostResponseCode")]
        public string HostResponseCode { get; set; }
        [XmlElement(ElementName = "ServicesID")]
        public string ServicesID { get; set; }
        [XmlElement(ElementName = "Card")]
        public Card Card { get; set; }
        [XmlElement(ElementName = "PaymentAccount")]
        public PaymentAccount PaymentAccount { get; set; }
        [XmlElement(ElementName = "Transaction")]
        public Transaction Transaction { get; set; }
        [XmlElement(ElementName = "Response")]
        public Response response { get; set; }

        [XmlElement(ElementName = "EnhancedBIN")]
        public EnhancedBIN EnhancedBIN { get; set; }
    }

    [XmlRoot(ElementName = "PaymentAccountCreateWithTransIDResponse", Namespace = "https://services.elementexpress.com")]
    public class PaymentAccountCreateWithTransIDResponse
    {
        [XmlElement(ElementName = "Response")]
        public Response Response { get; set; }
        [XmlAttribute(AttributeName = "xmlns")]
        public string Xmlns { get; set; }
    }
}
