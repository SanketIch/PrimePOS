using System.Xml.Serialization;

namespace Vantiv.RequestModels
{
    [XmlRoot(ElementName = "Credentials")]
    public class Credentials
    {
        [XmlElement(ElementName = "AccountID")]
        public string AccountID { get; set; }
        [XmlElement(ElementName = "AccountToken")]
        public string AccountToken { get; set; }
        [XmlElement(ElementName = "AcceptorID")]
        public string AcceptorID { get; set; }
    }

    [XmlRoot(ElementName = "Application")]
    public class Application
    {
        [XmlElement(ElementName = "ApplicationID")]
        public string ApplicationID { get; set; }
        [XmlElement(ElementName = "ApplicationVersion")]
        public string ApplicationVersion { get; set; }
        [XmlElement(ElementName = "ApplicationName")]
        public string ApplicationName { get; set; }
    }

    [XmlRoot(ElementName = "Transaction")]
    public class Transaction
    {
        [XmlElement(ElementName = "TransactionAmount")]
        public string TransactionAmount { get; set; }
        [XmlElement(ElementName = "ReferenceNumber")]
        public string ReferenceNumber { get; set; }
        [XmlElement(ElementName = "TicketNumber")]
        public string TicketNumber { get; set; }
        [XmlElement(ElementName = "MarketCode")]
        public string MarketCode { get; set; }
        [XmlElement(ElementName = "TransactionID")]
        public string TransactionID { get; set; }
        [XmlElement(ElementName = "TransactionStatus")]
        public string TransactionStatus { get; set; }
        [XmlElement(ElementName = "ApprovedAmount")]
        public string ApprovedAmount { get; set; }
        [XmlElement(ElementName = "ApprovalNumber")]
        public string ApprovalNumber { get; set; }
        [XmlElement(ElementName = "PaymentType")]
        public string PaymentType { get; set; }
        [XmlElement(ElementName = "SubmissionType")]
        public string SubmissionType { get; set; }
        [XmlElement(ElementName = "NetworkTransactionID")]
        public string NetworkTransactionID { get; set; }
        #region PRIMEPOS-2796
        [XmlElement(ElementName = "ReversalType")]
        public string ReversalType { get; set; }
        #endregion
    }

    [XmlRoot(ElementName = "Terminal")]
    public class Terminal
    {
        [XmlElement(ElementName = "TerminalID")]
        public string TerminalID { get; set; }
        [XmlElement(ElementName = "CardPresentCode")]
        public string CardPresentCode { get; set; }
        [XmlElement(ElementName = "CardholderPresentCode")]
        public string CardholderPresentCode { get; set; }
        [XmlElement(ElementName = "CardInputCode")]
        public string CardInputCode { get; set; }
        [XmlElement(ElementName = "CVVPresenceCode")]
        public string CVVPresenceCode { get; set; }
        [XmlElement(ElementName = "TerminalCapabilityCode")]
        public string TerminalCapabilityCode { get; set; }
        [XmlElement(ElementName = "TerminalEnvironmentCode")]
        public string TerminalEnvironmentCode { get; set; }
        [XmlElement(ElementName = "MotoECICode")]
        public string MotoECICode { get; set; }
        #region PRIMEPOS-2769
        [XmlElement(ElementName = "TerminalType")]
        public string TerminalType { get; set; }
        #endregion
    }
    [XmlRoot(ElementName = "ExtendedParameters")]
    public class ExtendedParameters
    {

        [XmlElement(ElementName = "Healthcare")]
        public Vantiv.HealthCare.Healthcare Healthcare { get; set; }
    }
    [XmlRoot(ElementName = "PaymentAccount")]
    public class PaymentAccount
    {
        [XmlElement(ElementName = "PaymentAccountID")]
        public string PaymentAccountID { get; set; }
        [XmlElement(ElementName = "PaymentAccountType")]
        public string PaymentAccountType { get; set; }
        [XmlElement(ElementName = "PaymentAccountReferenceNumber")]
        public string PaymentAccountReferenceNumber { get; set; }
    }

    [XmlRoot(ElementName = "CreditCardSale", Namespace = "https://transaction.elementexpress.com")]
    public class CreditCardSale
    {
        [XmlElement(ElementName = "Credentials")]
        public Credentials Credentials { get; set; }
        [XmlElement(ElementName = "Application")]
        public Application Application { get; set; }
        [XmlElement(ElementName = "Transaction")]
        public HealthCare.Transaction Transaction { get; set; }
        [XmlElement(ElementName = "Terminal")]
        public Terminal Terminal { get; set; }
        [XmlElement(ElementName = "PaymentAccount")]
        public PaymentAccount PaymentAccount { get; set; }
        [XmlElement(ElementName = "ExtendedParameters")]
        public ExtendedParameters ExtendedParameters { get; set; }
        [XmlAttribute(AttributeName = "xmlns")]
        public string Xmlns { get; set; }
    }

    #region PRIMEPOS-2769 
    [XmlRoot(ElementName = "CreditCardReversal", Namespace = "https://transaction.elementexpress.com")]
    public class CreditCardReversal
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
    #endregion
    //3010
    [XmlRoot(ElementName = "EnhancedBINQuery", Namespace = "https://transaction.elementexpress.com")]
    public class EnhancedBINQuery
    {

        [XmlElement(ElementName = "Credentials")]
        public Credentials Credentials { get; set; }

        [XmlElement(ElementName = "Application")]
        public Application Application { get; set; }

        [XmlElement(ElementName = "Terminal")]
        public Terminal Terminal { get; set; }

        [XmlElement(ElementName = "PaymentAccount")]
        public PaymentAccount PaymentAccount { get; set; }

        [XmlAttribute(AttributeName = "xmlns")]
        public string Xmlns { get; set; }

        [XmlText]
        public string Text { get; set; }
    }
}
