using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrimeRxPay.ResponseModels
{
    public class CreditCardSaleResponse
    {
        public string ExpressResponseCode { get; set; }
        public string ExpressResponseMessage { get; set; }
        public string HostResponseCode { get; set; }
        public string HostResponseMessage { get; set; }
        public string ExpressTransactionDate { get; set; }
        public string ExpressTransactionTime { get; set; }
        public string ExpressTransactionTimezone { get; set; }
        public string HostBatchID { get; set; }
        public string HostItemID { get; set; }
        public string HostBatchAmount { get; set; }
        public string CardLogo { get; set; }
        public string LastFour { get; set; }
        public string BIN { get; set; }
        public string TransactionID { get; set; }
        public string ApprovalNumber { get; set; }
        public string ReferenceNumber { get; set; }
        public string TransactionStatus { get; set; }
        public string TransactionStatusCode { get; set; }
        public string ApprovedAmount { get; set; }
        public string AVSResponseCode { get; set; }
        public object CVVResponseCode { get; set; }
        public object CommercialCardResponseCode { get; set; }
        public object BalanceAmount { get; set; }
        public object BalanceCurrencyCode { get; set; }
        public object SurchargeAmount { get; set; }
        public object NetworkLabel { get; set; }
        public object NetworkTransactionID { get; set; }
        public string PaymentAccountID { get; set; }
        public string PaymentAccountReferenceNumber { get; set; }
        public object TokenID { get; set; }
        public string TokenProvider { get; set; }
        public object TAProviderID { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }
    }
}
