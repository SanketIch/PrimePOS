using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace NBS.RequestModels
{
    public class AnalyzeFee
    {
        [JsonProperty("feeType")]
        public string FeeType { get; set; }

        [JsonProperty("value")]
        public int Value { get; set; }
    }

    public class AnalyzeLineItem
    {
        [JsonProperty("lineItemNumber")]
        public int LineItemNumber { get; set; }

        [JsonProperty("productCode")]
        public string ProductCode { get; set; }

        [JsonProperty("productCodeType")]
        public string ProductCodeType { get; set; }

        [JsonProperty("unitPrice")]
        public decimal UnitPrice { get; set; }

        [JsonProperty("quantity")]
        public int Quantity { get; set; }

        [JsonProperty("units")]
        public string Units { get; set; }

        [JsonProperty("taxes")]
        public List<AnalyzeTax> Taxes { get; set; }

        [JsonProperty("fees")]
        public List<AnalyzeFee> Fees { get; set; }
    }

    public class AnalyzeMember
    {
        [JsonProperty("uidType")]
        public string UidType { get; set; }

        [JsonProperty("uid")]
        public string Uid { get; set; }
    }

    public class AnalyzeMerchant
    {
        [JsonProperty("entityId")]
        public string EntityId { get; set; }

        [JsonProperty("storeId")]
        public string StoreId { get; set; }

        [JsonProperty("terminalId")]
        public string TerminalId { get; set; }
    }

    public class AnalyseRequest
    {
        [JsonProperty("member")]
        public AnalyzeMember Member { get; set; }

        [JsonProperty("merchant")]
        public AnalyzeMerchant Merchant { get; set; }

        [JsonProperty("transaction")]
        public AnalyzeTransaction Transaction { get; set; }
    }

    public class AnalyzeTax
    {
        [JsonProperty("taxType")]
        public string TaxType { get; set; }

        [JsonProperty("value")]
        public decimal Value { get; set; }
    }

    public class AnalyzeTransaction
    {
        [JsonProperty("merchantTransactionId")]
        public string MerchantTransactionId { get; set; }

        [JsonProperty("transactionLocalDateTime")]
        public DateTime TransactionLocalDateTime { get; set; }

        [JsonProperty("transactionCurrencyCode")]
        public string TransactionCurrencyCode { get; set; }

        [JsonProperty("lineItems")]
        public List<AnalyzeLineItem> LineItems { get; set; }
    }
}
