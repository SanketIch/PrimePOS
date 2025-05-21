using System.Xml.Serialization;

namespace Vantiv.HealthCare
{
	[XmlRoot(ElementName = "Healthcare")]
	public class Healthcare
	{

		[XmlElement(ElementName = "HealthcareFlag")]
		public int HealthcareFlag { get; set; }

		[XmlElement(ElementName = "HealthcareFirstAccountType")]
		public int HealthcareFirstAccountType { get; set; }

		[XmlElement(ElementName = "HealthcareFirstAmountType")]
		public int HealthcareFirstAmountType { get; set; }

		[XmlElement(ElementName = "HealthcareFirstCurrencyCode")]
		public int HealthcareFirstCurrencyCode { get; set; }

		[XmlElement(ElementName = "HealthcareFirstAmountSign")]
		public int HealthcareFirstAmountSign { get; set; }

		[XmlElement(ElementName = "HealthcareFirstAmount")]
		public double HealthcareFirstAmount { get; set; }

		[XmlElement(ElementName = "HealthcareSecondAccountType")]
		public int HealthcareSecondAccountType { get; set; }

		[XmlElement(ElementName = "HealthcareSecondAmountType")]
		public int HealthcareSecondAmountType { get; set; }

		[XmlElement(ElementName = "HealthcareSecondCurrencyCode")]
		public int HealthcareSecondCurrencyCode { get; set; }

		[XmlElement(ElementName = "HealthcareSecondAmountSign")]
		public int HealthcareSecondAmountSign { get; set; }

		[XmlElement(ElementName = "HealthcareSecondAmount")]
		public double HealthcareSecondAmount { get; set; }
	}
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

        [XmlElement(ElementName = "PartialApprovedFlag")]
        public int PartialApprovedFlag { get; set; }
        #endregion
    }
}
