using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrimeRxPay.ResponseModels
{
    public class VoidResponse
    {
        [JsonProperty("ExpressResponseCode")]
        public string ExpressResponseCode { get; set; }

        [JsonProperty("ExpressResponseMessage")]
        public string ExpressResponseMessage { get; set; }

        [JsonProperty("HostResponseCode")]
        public string HostResponseCode { get; set; }

        [JsonProperty("HostResponseMessage")]
        public string HostResponseMessage { get; set; }

        [JsonProperty("ExpressTransactionDate")]
        public string ExpressTransactionDate { get; set; }

        [JsonProperty("ExpressTransactionTime")]
        public string ExpressTransactionTime { get; set; }

        [JsonProperty("ExpressTransactionTimezone")]
        public string ExpressTransactionTimezone { get; set; }

        [JsonProperty("HostBatchID")]
        public string HostBatchId { get; set; }

        #region PRIMEPOS-3383
        [JsonProperty("LastFour")]
        public string LastFour { get; set; }
        #endregion

        [JsonProperty("HostItemID")]
        public string HostItemId { get; set; }

        [JsonProperty("HostBatchAmount")]
        public string HostBatchAmount { get; set; }

        [JsonProperty("CardLogo")]
        public string CardLogo { get; set; }

        [JsonProperty("BIN")]
        public string Bin { get; set; }

        [JsonProperty("TransactionID")]
        public string TransactionId { get; set; }

        [JsonProperty("ReferenceNumber")]
        public string ReferenceNumber { get; set; }

        [JsonProperty("AcquirerData")]
        public string AcquirerData { get; set; }

        [JsonProperty("ProcessorName")]
        public string ProcessorName { get; set; }

        [JsonProperty("TransactionStatus")]
        public string TransactionStatus { get; set; }

        [JsonProperty("TransactionStatusCode")]
        public string TransactionStatusCode { get; set; }

        [JsonProperty("ApprovedAmount")]
        public string ApprovedAmount { get; set; }

        [JsonProperty("TokenID")]
        public string TokenId { get; set; }

        [JsonProperty("TokenProvider")]
        public string TokenProvider { get; set; }

        [JsonProperty("TAProviderID")]
        public string TaProviderId { get; set; }

        [JsonProperty("Code")]
        public string Code { get; set; }

        [JsonProperty("Message")]
        public string Message { get; set; }
        [JsonProperty("ApprovalNumber")]
        public string ApprovalNumber { get; set; }
    }
}
