using Newtonsoft.Json;
using System;

namespace NBS.ResponseModels
{
    public class BinRangeResponse
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

    public class BinRangeData
    {
        [JsonProperty("mId")]
        public string MId { get; set; }

        [JsonProperty("BinCode")]
        public string BinCode { get; set; }

        [JsonProperty("IsDelete")]
        public bool IsDelete { get; set; }
    }
}
