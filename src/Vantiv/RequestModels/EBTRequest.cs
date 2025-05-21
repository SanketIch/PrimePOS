using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vantiv.RequestModels
{
    public class EBTRequest
    {
        public string approvalNumber { get; set; }
        public string laneId { get; set; }
        public string transactionAmount { get; set; }
        public string voucherNumber { get; set; }
        public string cardHolderPresentCode { get; set; }
        public string clerkNumber { get; set; }
        public Configuration configuration { get; set; }
        public string referenceNumber { get; set; }
        public string ticketNumber { get; set; }
    }
}
