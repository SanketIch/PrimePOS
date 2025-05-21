using Newtonsoft.Json;
using System;

namespace NBS.ResponseModels
{
    public class ReversalData
    {
        [JsonProperty("response")]
        public ReversalResponse Response { get; set; }

        [JsonProperty("nBInternalTrace")]
        public ReversalNBInternalTrace NBInternalTrace { get; set; }
    }

    public class ReversalNBInternalTrace
    {
        [JsonProperty("statusCode")]
        public string StatusCode { get; set; }

        [JsonProperty("traceId")]
        public string TraceId { get; set; }

        [JsonProperty("traceIdDate")]
        public DateTime TraceIdDate { get; set; }
    }

    public class ReversalResponse
    {
        [JsonProperty("nationsBenefitsTransactionId")]
        public string NationsBenefitsTransactionId { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("code")]
        public string Code { get; set; }
    }

    public class ReversalApiResponse
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

}
