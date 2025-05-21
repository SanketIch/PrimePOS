using Newtonsoft.Json;

namespace NBS.RequestModels
{
    public class ReversalRequest
    {
        [JsonProperty("nationsBenefitsTransactionId")]
        public string NationsBenefitsTransactionId { get; set; }

        [JsonProperty("merchantDiscretionaryData")]
        public string MerchantDiscretionaryData { get; set; }
    }
}
