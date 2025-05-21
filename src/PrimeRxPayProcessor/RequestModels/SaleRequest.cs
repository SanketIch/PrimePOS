using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrimeRxPay.RequestModels
{
    public class SaleRequest
    {
        [JsonProperty("PatientNo")]
        public string PatientNo { get; set; }

        [JsonProperty("PatientLastName")]
        public string PatientLastName { get; set; }

        [JsonProperty("PatientFirstName")]
        public string PatientFirstName { get; set; }

        [JsonProperty("PatientDOB")]
        public string PatientDob { get; set; }

        [JsonProperty("PatientMobileNo")]
        public string PatientMobileNo { get; set; }

        [JsonProperty("PatientEmail")]
        public string PatientEmail { get; set; }

        [JsonProperty("PatientStreetLine1")]
        public string PatientStreetLine1 { get; set; }

        [JsonProperty("PatientStreetLine2")]
        public string PatientStreetLine2 { get; set; }

        [JsonProperty("PatientCity")]
        public string PatientCity { get; set; }

        [JsonProperty("PatientState")]
        public string PatientState { get; set; }

        [JsonProperty("PatientZipCode")]
        public string PatientZipCode { get; set; }

        [JsonProperty("PatientAddress")]
        public string PatientAddress { get; set; }

        [JsonProperty("InsuranceCode")]
        public string InsuranceCode { get; set; }

        [JsonProperty("InsuranceName")]
        public string InsuranceName { get; set; }

        [JsonProperty("TransDate")]
        public string TransDate { get; set; }

        [JsonProperty("Amount")]
        public string Amount { get; set; }

        [JsonProperty("HealthcareAmount")]
        public string HealthcareAmount { get; set; }

        [JsonProperty("LinkExpiryInMinutes")]
        public string LinkExpiryInMinutes { get; set; }

        [JsonProperty("TransactionProcessingMode")]
        public string TransactionProcessingMode { get; set; }

        [JsonProperty("TransactionItems")]
        public string TransactionItems { get; set; }

        [JsonProperty("PharmacyNPI")]
        public string PharmacyNpi { get; set; }

        [JsonProperty("PaymentProviderID")]
        public long PaymentProviderId { get; set; }

        [JsonProperty("LaneNumber")]
        public string LaneNumber { get; set; }

        [JsonProperty("ReferenceNumber")]
        public string ReferenceNumber { get; set; }

        [JsonProperty("TicketNumber")]
        public string TicketNumber { get; set; }

        [JsonProperty("ApplicationID")]
        public string ApplicationId { get; set; }

        [JsonProperty("UserName")]
        public string UserName { get; set; }

        [JsonProperty("ExternalID")]
        public string ExternalId { get; set; }
        [JsonProperty("PaymentCardType")]
        public string PaymentCardType { get; set; }

        [JsonProperty("ClientID")]
        public string ClientID { get; set; }
        [JsonProperty("SecretKey")]
        public string SecretKey { get; set; }
        [JsonProperty("URL")]
        public string URL { get; set; }
        [JsonProperty("TransactionSetupMethod")]
        public string TransactionSetupMethod { get; set; }
        [JsonProperty("IsSecuredDevice")]
        public bool IsSecuredDevice { get; set; }
        [JsonProperty("TerminalSrNumber")]
        public string TerminalSrNumber { get; set; }
        [JsonProperty("TerminalModel")]
        public string TerminalModel { get; set; }

    }
}
