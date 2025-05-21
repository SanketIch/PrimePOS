using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrimeRxPay.ResponseModels
{
    public class GetTransactionDetail
    {
        [JsonProperty("PayProviderID")]
        public string PayProviderId { get; set; }

        [JsonProperty("TransactionID")]
        public string TransactionId { get; set; }

        [JsonProperty("PayProviderTransID")]
        public string PayProviderTransId { get; set; }

        [JsonProperty("PayProviderResponseCode")]
        public string PayProviderResponseCode { get; set; }

        [JsonProperty("PayProviderResponseMessage")]
        public string PayProviderResponseMessage { get; set; }

        [JsonProperty("AVSResponseCode")]
        public string AvsResponseCode { get; set; }

        [JsonProperty("CVVResponseCode")]
        public string CvvResponseCode { get; set; }

        [JsonProperty("ApprovalNumber")]
        public string ApprovalNumber { get; set; }

        [JsonProperty("LastFour")]
        public string LastFour { get; set; }

        [JsonProperty("CardLogo")]
        public string CardLogo { get; set; }

        [JsonProperty("AmountDue")]
        public double AmountDue { get; set; }

        [JsonProperty("ApprovedAmounut")]
        public string ApprovedAmounut { get; set; }

        [JsonProperty("ServiceID")]
        public string ServiceId { get; set; }

        [JsonProperty("PaymentAccountID")]
        public string PaymentAccountId { get; set; }

        [JsonProperty("CommercialCardResponseCode")]
        public string CommercialCardResponseCode { get; set; }

        [JsonProperty("TipAmount")]
        public string TipAmount { get; set; }

        [JsonProperty("Code")]
        public string Code { get; set; }

        [JsonProperty("Message")]
        public string Message { get; set; }
        [JsonProperty("HSAFSACard")]
        public string HSAFSACard { get; set; }
        [JsonProperty("ExpirationDate")]
        public DateTime? ExpirationDate { get; set; }
        
    }
}
