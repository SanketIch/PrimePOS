using System;
using System.Collections.Generic;

namespace Vantiv.ResponseModels
{
    public class EBTResponse
    {
        public string accountNumber { get; set; }
        public string binValue { get; set; }
        public string cardHolderName { get; set; }
        public string currencyCode { get; set; }
        public string entryMode { get; set; }
        public string expirationYear { get; set; }
        public string expirationMonth { get; set; }
        public string paymentType { get; set; }
        public bool pinVerified { get; set; }
        public Signature signature { get; set; }
        public string terminalId { get; set; }
        public double totalAmount { get; set; }
        public bool isApproved { get; set; }
        public Processor _processor { get; set; }
        public string statusCode { get; set; }
        public DateTime transactionDateTime { get; set; }
        public string merchantId { get; set; }
        public bool isOffline { get; set; }
        public List<ApiError> _errors { get; set; }
        public bool _hasErrors { get; set; }
        public List<ApiLink> _links { get; set; }
        public List<String> _logs { get; set; }
        public string _type { get; set; }
        public List<ApiWarning> _warnings { get; set; }
    }
}
