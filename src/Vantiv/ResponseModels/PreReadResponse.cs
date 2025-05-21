using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Vantiv.ResponseModels
{
    public class PrePeadProcessor
    {
        [DataMember(Name = "cashbackAmount")]
        [XmlElement("cashbackAmount")]
        public decimal cashbackAmount { get; set; }

        [DataMember(Name = "debitSurchargeAmount")]
        [XmlElement("debitSurchargeAmount")]
        public decimal DebitSurchargeAmount { get; set; }


        [DataMember(Name = "quickChipMessage")]
        [XmlElement("quickChipMessage")]
        public decimal quickChipMessage { get; set; }


    }

    public class PreReadResponse
    {
        public int cashbackAmount { get; set; }
        public int debitSurchargeAmount { get; set; }
        public string quickChipMessage { get; set; }
        public double approvedAmount { get; set; }
        public double convenienceFeeAmount { get; set; }
        public double subTotalAmount { get; set; }
        public int tipAmount { get; set; }
        public string fsaCard { get; set; }
        public bool isCardInserted { get; set; }
        public string accountNumber { get; set; }
        public string binValue { get; set; }
        public string cardHolderName { get; set; }
        public string cardLogo { get; set; }
        public string currencyCode { get; set; }
        public string countryCode { get; set; }
        public string language { get; set; }
        public string entryMode { get; set; }
        public string expirationYear { get; set; }
        public string expirationMonth { get; set; }
        public string paymentType { get; set; }
        public bool pinVerified { get; set; }
        public Signature signature { get; set; }
        public string terminalId { get; set; }
        public double totalAmount { get; set; }
        public string preReadId { get; set; }
        public string approvalNumber { get; set; }
        public bool isApproved { get; set; }
        public Processor _processor { get; set; }
        public string statusCode { get; set; }
        public string transactionDateTime { get; set; }
        public string transactionId { get; set; }
        public string merchantId { get; set; }
        public bool isOffline { get; set; }
        public List<object> _errors { get; set; }
        public bool _hasErrors { get; set; }
        public List<object> _links { get; set; }
        public List<object> _logs { get; set; }
        public string _type { get; set; }
        public List<object> _warnings { get; set; }
    }

    public class PreReadSignature
    {
        public string statusCode { get; set; }
    }
}
