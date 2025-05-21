using System.Xml.Serialization;

namespace Vantiv.ResponseModels
{
    [XmlRoot(ElementName = "Item")]
    public class Item
    {
        [XmlElement(ElementName = "TransactionID")]
        public string TransactionID
        {
            get; set;
        }
        [XmlElement(ElementName = "AcceptorID")]
        public string AcceptorID
        {
            get; set;
        }
        [XmlElement(ElementName = "AccountID")]
        public string AccountID
        {
            get; set;
        }
        [XmlElement(ElementName = "Name")]
        public string Name
        {
            get; set;
        }
        [XmlElement(ElementName = "TerminalID")]
        public string TerminalID
        {
            get; set;
        }
        [XmlElement(ElementName = "ApplicationID")]
        public string ApplicationID
        {
            get; set;
        }
        [XmlElement(ElementName = "ApprovalNumber")]
        public string ApprovalNumber
        {
            get; set;
        }
        [XmlElement(ElementName = "ApprovedAmount")]
        public string ApprovedAmount
        {
            get; set;
        }
        [XmlElement(ElementName = "ExpirationMonth")]
        public string ExpirationMonth
        {
            get; set;
        }
        [XmlElement(ElementName = "ExpirationYear")]
        public string ExpirationYear
        {
            get; set;
        }
        [XmlElement(ElementName = "ExpressResponseCode")]
        public string ExpressResponseCode
        {
            get; set;
        }
        [XmlElement(ElementName = "ExpressResponseMessage")]
        public string ExpressResponseMessage
        {
            get; set;
        }
        [XmlElement(ElementName = "HostBatchID")]
        public string HostBatchID
        {
            get; set;
        }
        [XmlElement(ElementName = "HostItemID")]
        public string HostItemID
        {
            get; set;
        }
        [XmlElement(ElementName = "HostResponseCode")]
        public string HostResponseCode
        {
            get; set;
        }
        [XmlElement(ElementName = "OriginalAuthorizedAmount")]
        public string OriginalAuthorizedAmount
        {
            get; set;
        }
        [XmlElement(ElementName = "ReferenceNumber")]
        public string ReferenceNumber
        {
            get; set;
        }
        [XmlElement(ElementName = "TicketNumber")]
        public string TicketNumber
        {
            get; set;
        }
        [XmlElement(ElementName = "TrackingID")]
        public string TrackingID
        {
            get; set;
        }
        [XmlElement(ElementName = "TerminalType")]
        public string TerminalType
        {
            get; set;
        }
        [XmlElement(ElementName = "TransactionAmount")]
        public string TransactionAmount
        {
            get; set;
        }
        [XmlElement(ElementName = "TransactionStatus")]
        public string TransactionStatus
        {
            get; set;
        }
        [XmlElement(ElementName = "TransactionStatusCode")]
        public string TransactionStatusCode
        {
            get; set;
        }
        [XmlElement(ElementName = "TransactionType")]
        public string TransactionType
        {
            get; set;
        }
        [XmlElement(ElementName = "CardNumberMasked")]
        public string CardNumberMasked
        {
            get; set;
        }
        [XmlElement(ElementName = "CardLogo")]
        public string CardLogo
        {
            get; set;
        }
        [XmlElement(ElementName = "CardType")]
        public string CardType
        {
            get; set;
        }
        [XmlElement(ElementName = "TrackDataPresent")]
        public string TrackDataPresent
        {
            get; set;
        }
        [XmlElement(ElementName = "HostTransactionID")]
        public string HostTransactionID
        {
            get; set;
        }
        [XmlElement(ElementName = "ExpressTransactionDate")]
        public string ExpressTransactionDate
        {
            get; set;
        }
        [XmlElement(ElementName = "ExpressTransactionTime")]
        public string ExpressTransactionTime
        {
            get; set;
        }
        [XmlElement(ElementName = "TimeStamp")]
        public string TimeStamp
        {
            get; set;
        }
        [XmlElement(ElementName = "LaneNumber")]
        public string LaneNumber
        {
            get; set;
        }
        [XmlElement(ElementName = "IntegrationTypeID")]
        public string IntegrationTypeID
        {
            get; set;
        }
        [XmlElement(ElementName = "BatchStatus")]
        public string BatchStatus
        {
            get; set;
        }
        [XmlElement(ElementName = "BatchStatusCode")]
        public string BatchStatusCode
        {
            get; set;
        }
        [XmlElement(ElementName = "SystemTraceAuditNumber")]
        public string SystemTraceAuditNumber
        {
            get; set;
        }
        [XmlElement(ElementName = "CardLevelResults")]
        public string CardLevelResults
        {
            get; set;
        }
        [XmlElement(ElementName = "RetrievalReferenceNumber")]
        public string RetrievalReferenceNumber
        {
            get; set;
        }
        [XmlElement(ElementName = "TerminalData")]
        public string TerminalData
        {
            get; set;
        }
        [XmlElement(ElementName = "MerchantCategoryCode")]
        public string MerchantCategoryCode
        {
            get; set;
        }
        [XmlElement(ElementName = "CardInputCode")]
        public string CardInputCode
        {
            get; set;
        }
        [XmlElement(ElementName = "NetworkTransactionID")]
        public string NetworkTransactionID
        {
            get; set;
        }
    }


    [XmlRoot(ElementName = "Items")]
    public class Items
    {
        [XmlElement(ElementName = "Item")]
        public Item Item
        {
            get; set;
        }
    }

    [XmlRoot(ElementName = "ReportingData")]
    public class ReportingData
    {
        [XmlElement(ElementName = "Items")]
        public Items Items
        {
            get; set;
        }
    }



    [XmlRoot(ElementName = "TransactionQueryResponse", Namespace = "https://reporting.elementexpress.com")]
    public class TransactionQueryResponse
    {
        [XmlElement(ElementName = "Response")]
        public Response response
        {
            get; set;
        }

        [XmlAttribute(AttributeName = "xmlns")]
        public string Xmlns
        {
            get; set;
        }

        [XmlRoot(ElementName = "Response")]
        public class Response
        {
            [XmlElement(ElementName = "ExpressResponseCode")]
            public string ExpressResponseCode
            {
                get; set;
            }
            [XmlElement(ElementName = "ExpressResponseMessage")]
            public string ExpressResponseMessage
            {
                get; set;
            }
            [XmlElement(ElementName = "ExpressTransactionDate")]
            public string ExpressTransactionDate
            {
                get; set;
            }
            [XmlElement(ElementName = "ExpressTransactionTime")]
            public string ExpressTransactionTime
            {
                get; set;
            }
            [XmlElement(ElementName = "ExpressTransactionTimezone")]
            public string ExpressTransactionTimezone
            {
                get; set;
            }
            [XmlElement(ElementName = "ReportingData")]
            public ReportingData ReportingData
            {
                get; set;
            }
            [XmlElement(ElementName = "ReportingID")]
            public string ReportingID
            {
                get; set;
            }
        }
    }
}
