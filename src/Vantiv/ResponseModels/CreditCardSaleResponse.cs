using System.Xml.Serialization;

namespace Vantiv.ResponseModels
{
    [XmlRoot(ElementName = "Batch")]
    public class Batch
    {
        [XmlElement(ElementName = "HostBatchID")]
        public string HostBatchID { get; set; }
        [XmlElement(ElementName = "HostItemID")]
        public string HostItemID { get; set; }
        [XmlElement(ElementName = "HostBatchAmount")]
        public string HostBatchAmount { get; set; }
    }

    [XmlRoot(ElementName = "CreditCardSaleResponse", Namespace = "https://transaction.elementexpress.com")]
    public class CreditCardSaleResponse
    {
        [XmlElement(ElementName = "Response")]
        public Response Response { get; set; }
        [XmlAttribute(AttributeName = "xmlns")]
        public string Xmlns { get; set; }
    }
    [XmlRoot(ElementName = "DebitCardSaleResponse", Namespace = "https://transaction.elementexpress.com")]
    public class DebitCardSaleResponse
    {
        [XmlElement(ElementName = "Response")]
        public Response Response { get; set; }
        [XmlAttribute(AttributeName = "xmlns")]
        public string Xmlns { get; set; }
    }
    [XmlRoot(ElementName = "CreditCardReturnResponse", Namespace = "https://transaction.elementexpress.com")]
    public class CreditCardReturnResponse
    {
        [XmlElement(ElementName = "Response")]
        public Response Response { get; set; }
        [XmlAttribute(AttributeName = "xmlns")]
        public string Xmlns { get; set; }
    }
    [XmlRoot(ElementName = "DebitCardReturnResponse", Namespace = "https://transaction.elementexpress.com")] //PRIMEPOS-3521 //PRIMEPOS-3522 //PRIMEPOS-3504
    public class DebitCardReturnResponse
    {
        [XmlElement(ElementName = "Response")]
        public Response Response { get; set; }
        [XmlAttribute(AttributeName = "xmlns")]
        public string Xmlns { get; set; }
    }
    [XmlRoot(ElementName = "EBTSaleResponse", Namespace = "https://transaction.elementexpress.com")]
    public class EBTSaleResponse
    {
        [XmlElement(ElementName = "Response")]
        public Response Response { get; set; }
        [XmlAttribute(AttributeName = "xmlns")]
        public string Xmlns { get; set; }
    }
    #region PRIMEPOS-2796
    [XmlRoot(ElementName = "CreditCardReversalResponse", Namespace = "https://transaction.elementexpress.com")]
    public class CreditCardReversalResponse
    {
        [XmlElement(ElementName = "Response")]
        public Response Response { get; set; }
        [XmlAttribute(AttributeName = "xmlns")]
        public string Xmlns { get; set; }
    }
    #endregion

    #region PRIMEPOS-3283
    [XmlRoot(ElementName = "DebitCardReversalResponse", Namespace = "https://transaction.elementexpress.com")]
    public class DebitCardReversalResponse
    {
        [XmlElement(ElementName = "Response")]
        public Response Response
        {
            get; set;
        }
        [XmlAttribute(AttributeName = "xmlns")]
        public string Xmlns
        {
            get; set;
        }
    }
    #endregion

    [XmlRoot(ElementName = "CreditCardCreditResponse", Namespace = "https://transaction.elementexpress.com")]
    public class CreditCardCreditResponse
    {
        [XmlElement(ElementName = "Response")]
        public Response Response { get; set; }
        [XmlAttribute(AttributeName = "xmlns")]
        public string Xmlns { get; set; }
    }
    [XmlRoot(ElementName = "DebitCardCreditResponse", Namespace = "https://transaction.elementexpress.com")]
    public class DebitCardCreditResponse
    {
        [XmlElement(ElementName = "Response")]
        public Response Response { get; set; }
        [XmlAttribute(AttributeName = "xmlns")]
        public string Xmlns { get; set; }
    }

    [XmlRoot(ElementName = "EnhancedBINQueryResponse", Namespace = "https://transaction.elementexpress.com")]
    public class EnhancedBINQueryResponse
    {

        [XmlElement(ElementName = "Response")]
        public Response Response { get; set; }

        [XmlAttribute(AttributeName = "xmlns")]
        public string Xmlns { get; set; }

        [XmlText]
        public string Text { get; set; }
    }
    [XmlRoot(ElementName = "EnhancedBIN")]
    public class EnhancedBIN
    {

        [XmlElement(ElementName = "Status")]
        public string Status { get; set; }

        [XmlElement(ElementName = "CreditCard")]
        public string CreditCard { get; set; }

        [XmlElement(ElementName = "HSAFSACard")]
        public string HSAFSACard { get; set; }

        [XmlElement(ElementName = "DurbinBINRegulation")]
        public int DurbinBINRegulation { get; set; }
    }
    #region PRIMEPOS-3156
    [XmlRoot(ElementName = "EBTReversalResponse", Namespace = "https://transaction.elementexpress.com")]
    public class EBTReversalResponse
    {
        [XmlElement(ElementName = "Response")]
        public Response Response
        {
            get; set;
        }

        [XmlAttribute(AttributeName = "xmlns")]
        public string Xmlns
        {
            get; set;
        }
    }
    #endregion PRIMEPOS-3156
}
