using Newtonsoft.Json;

namespace NBS.RequestModels
{
    #region PRIMEPOS-3373
    internal class VoidRequest
    {
        [JsonProperty("nationsBenefitsTransactionId")]
        public string NationsBenefitsTransactionId { get; set; }

        [JsonProperty("merchantDiscretionaryData")]
        public string MerchantDiscretionaryData { get; set; }
    }
    #endregion
}
