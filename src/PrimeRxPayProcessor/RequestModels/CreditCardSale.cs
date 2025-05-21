using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrimeRxPay.RequestModels
{
    public class CreditCardSale
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
        public string LinkExpiryInMinutes { get; set; }
        public string TransactionProcessingMode { get; set; }
        public string TransactionItems { get; set; }
        public string PharmacyNPI { get; set; }
        public int PaymentProviderID { get; set; }
        public string LaneNumber { get; set; }
        public string ReferenceNumber { get; set; }
        public string TicketNumber { get; set; }
        public string ApplicationID { get; set; }
        public string UserName { get; set; }
        public string ExternalID { get; set; }
        public string PaymentAccountID { get; set; }
        public string HSAFSACard { get; set; }
    }
}