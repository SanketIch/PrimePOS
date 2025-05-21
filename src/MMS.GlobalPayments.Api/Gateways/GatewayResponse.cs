using System.Net;

namespace MMS.GlobalPayments.Api.Gateways {
    internal class GatewayResponse {
        public string RawResponse { get; set; }
        public string RequestUrl { get; set; }
        public HttpStatusCode StatusCode { get; set; }
    }
}
