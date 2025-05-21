using Newtonsoft.Json;
using System;

namespace NBS.ResponseModels
{
    #region PRIMEPOS-3373
    public class VoidData
    {
        [JsonProperty("response")]
        public VoidResponse Response { get; set; }

        [JsonProperty("nBInternalTrace")]
        public VoidNBInternalTrace NBInternalTrace { get; set; }
    }
    public class VoidResponse
    {
        [JsonProperty("nationsBenefitsTransactionId")]
        public string NationsBenefitsTransactionId { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("code")]
        public string Code { get; set; }
    }
    public class VoidNBInternalTrace
    {
        [JsonProperty("statusCode")]
        public string StatusCode { get; set; }

        [JsonProperty("traceId")]
        public string TraceId { get; set; }

        [JsonProperty("traceIdDate")]
        public DateTime TraceIdDate { get; set; }
    }
    public class VoidApiResponse
    {
        [JsonProperty("result")]
        public string Result { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("timeStamp")]
        public DateTime TimeStamp { get; set; }

        [JsonProperty("data")]
        public string Data { get; set; }
    }
    #endregion
}
