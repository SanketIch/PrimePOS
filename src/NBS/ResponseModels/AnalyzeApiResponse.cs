using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace NBS.ResponseModels
{
    public class AnalyseAuthorizationDetail
    {
        [JsonProperty("lineItemNumber")]
        public int LineItemNumber { get; set; }

        [JsonProperty("productCode")]
        public string ProductCode { get; set; }

        [JsonProperty("productCodeType")]
        public string ProductCodeType { get; set; }

        [JsonProperty("authorizedAmountBeforeTax")]
        public double AuthorizedAmountBeforeTax { get; set; }

        [JsonProperty("authorizedTax")]
        public double AuthorizedTax { get; set; }

        [JsonProperty("authorizedFees")]
        public double AuthorizedFees { get; set; }

        [JsonProperty("authResult")]
        public string AuthResult { get; set; }

        [JsonProperty("benefitName")]
        public string BenefitName { get; set; }

        [JsonProperty("code")]
        public int Code { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }
    }

    public class AnalyseData
    {
        [JsonProperty("response")]
        public AnalyseResponse Response { get; set; }

        [JsonProperty("nBInternalTrace")]
        public AnalyseNBInternalTrace NBInternalTrace { get; set; }
    }

    public class AnalyseNBInternalTrace
    {
        [JsonProperty("statusCode")]
        public string StatusCode { get; set; }

        [JsonProperty("traceId")]
        public string TraceId { get; set; }

        [JsonProperty("traceIdDate")]
        public DateTime TraceIdDate { get; set; }
    }

    public class AnalysePurseUtilization
    {
        [JsonProperty("purseName")]
        public string PurseName { get; set; }

        [JsonProperty("amountUsed")]
        public double AmountUsed { get; set; }

        [JsonProperty("remainingBalance")]
        public double RemainingBalance { get; set; }
    }

    public class AnalyseResponse
    {
        [JsonProperty("merchantTransactionId")]
        public string MerchantTransactionId { get; set; }

        [JsonProperty("nationsBenefitsTransactionId")]
        public string NationsBenefitsTransactionId { get; set; }

        [JsonProperty("authorizedTransactionAmount")]
        public double AuthorizedTransactionAmount { get; set; }

        [JsonProperty("authorizationDetails")]
        public List<AnalyseAuthorizationDetail> AuthorizationDetails { get; set; }

        [JsonProperty("purseUtilization")]
        public List<AnalysePurseUtilization> PurseUtilization { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("code")]
        public string Code { get; set; }
    }

    public class AnalyseApiResponse
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
