using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrimeRxPay.RequestModels
{
    public class CustomerSaleRequest
    {
        public string PatientNo { get; set; }
        public string PatientLastName { get; set; }
        public string PatientFirstName { get; set; }
        public string PatientDOB { get; set; }
        public string PatientMobileNo { get; set; }
        public string PatientEmail { get; set; }
        public string PatientStreetLine1 { get; set; }
        public string PatientStreetLine2 { get; set; }
        public string PatientCity { get; set; }
        public string PatientState { get; set; }
        public string PatientZipCode { get; set; }
        public string PatientAddress { get; set; }
        public string InsuranceCode { get; set; }
        public string InsuranceName { get; set; }
        public string TransDate { get; set; }
        public string Amount { get; set; }
        public string HealthcareAmount { get; set; }
        public string ExternalID { get; set; }
        public int LinkExpiryInMinutes { get; set; }
        public int TransactionProcessingMode { get; set; }
        public int TransType { get; set; }
        public int ReturnTransID { get; set; }
        public int PaymentCardType { get; set; }
        public string TransactionSetupMethod { get; set; }
        public string PaymentAccountID { get; set; }
        public string HSAFSACard { get; set; }
        public string NetworkTransactionID { get; set; }
        public int SubmissionType { get; set; }
        public string TransactionItems { get; set; }
        public string PharmacyNPI { get; set; }
        public int PaymentProviderID { get; set; }
        public string LaneNumber { get; set; }
        public string ReferenceNumber { get; set; }
        public string TicketNumber { get; set; }
        public int ApplicationID { get; set; }
        public string UserName { get; set; }
        public int MarketCodeType { get; set; }
    }
}
