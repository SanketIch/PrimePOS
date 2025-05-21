using Newtonsoft.Json;
using System;

namespace NBS.ResponseModels
{
    public class RedeemData
    {
        [JsonProperty("response")]
        public RedeemResponse Response { get; set; }

        [JsonProperty("nBInternalTrace")]
        public RedeemNBInternalTrace NBInternalTrace { get; set; }
    }

    public class RedeemNBInternalTrace
    {
        [JsonProperty("statusCode")]
        public string StatusCode { get; set; }

        [JsonProperty("traceId")]
        public string TraceId { get; set; }

        [JsonProperty("traceIdDate")]
        public DateTime TraceIdDate { get; set; }
    }

    public class RedeemResponse
    {
        [JsonProperty("nationsBenefitsTransactionId")]
        public string NationsBenefitsTransactionId { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("code")]
        public string Code { get; set; }
    }

    public class RedeemApiResponse
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
