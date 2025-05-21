using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vantiv.RequestModels
{
    public class RefundRequest
    {
        public string laneId { get; set; }
        public string transactionAmount { get; set; }
        public Address address { get; set; }
        public string cardHolderPresentCode { get; set; }
        public string clerkNumber { get; set; }
        public string commercialCardCustomerCode { get; set; }
        public Configuration configuration { get; set; }
        public string convenienceFeeAmount { get; set; }
        public string displayTransactionAmount { get; set; }
        public string invokeManualEntry { get; set; }
        public string referenceNumber { get; set; }
        public string salesTaxAmount { get; set; }
        public string shiftId { get; set; }
        public StoreCard storeCard { get; set; }
        public string ticketNumber { get; set; }
    }
}
