using Newtonsoft.Json;

namespace NBS.RequestModels
{
    public class RedeemRequest
    {
        [JsonProperty("nationsBenefitsTransactionId")]
        public string NationsBenefitsTransactionId { get; set; }

        [JsonProperty("redeemedAmount")]
        public string RedeemedAmount { get; set; }

        [JsonProperty("merchantDiscretionaryData")]
        public string MerchantDiscretionaryData { get; set; }
    }
}
