using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrimeRxPay.ResponseModels
{
    public class SaleResponse
    {
        [JsonProperty("TransID")]
        public long TransId { get; set; }

        [JsonProperty("TransactionSetupID")]
        public string TransactionSetupId { get; set; }

        [JsonProperty("PaymentTransactionURL")]
        public string PaymentTransactionUrl { get; set; }

        [JsonProperty("Code")]
        public string Code { get; set; }

        [JsonProperty("Message")]
        public string Message { get; set; }
    }
}
