using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrimeRxPay.RequestModels
{
     public class VoidRequest
    {
        [JsonProperty("PharmacyNPI")]
        public string PharmacyNpi { get; set; }

        [JsonProperty("PaymentProviderID")]
        public string PaymentProviderId { get; set; }

        [JsonProperty("LaneNumber")]
        public string LaneNumber { get; set; }

        [JsonProperty("ReferenceNumber")]
        public string ReferenceNumber { get; set; }

        [JsonProperty("TicketNumber")]
        public string TicketNumber { get; set; }

        [JsonProperty("ApplicationID")]
        public string ApplicationId { get; set; }

        [JsonProperty("Amount")]
        public string Amount { get; set; }

        [JsonProperty("PaymentProviderTransID")]
        public string PaymentProviderTransId { get; set; }

        [JsonProperty("UserName")]
        public string UserName { get; set; }
        [JsonProperty("PrimerxPayTransId")]
        public int PrimerxPayTransId { get; set; }
    }
}
