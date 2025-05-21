using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrimeRxPay.RequestModels
{
    public class ResendLinkReq
    {
        public string PharmacyNPI { get; set; }
        public int PrimerxPayTransId { get; set; }
        public string PatientName { get; set; }
        public string PatientMobileNo { get; set; }
        public string PatientEmail { get; set; }
        public int LinkExpiryInMinutes { get; set; }
        public int TransactionProcessingMode { get; set; }
    }
}
