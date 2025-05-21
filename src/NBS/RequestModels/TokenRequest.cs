using Newtonsoft.Json;

namespace NBS.RequestModels
{
    public class TokenRequest
    {
        [JsonProperty("NPINo")]
        public string NpiNo { get; set; }

        [JsonProperty("AppId")]
        public string AppId { get; set; }
    }
}
