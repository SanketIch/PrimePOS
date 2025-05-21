using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vantiv.RequestModels
{
    class VoidRequest
    {
        public string laneId { get; set; }
        public string cardHolderPresentCode { get; set; }
        public string clerkNumber { get; set; }
        public Configuration configuration { get; set; }
        public string referenceNumber { get; set; }
        public string shiftId { get; set; }
        public string approvedAmount { get; set; }//PRIMEPOS-2795
        public string ticketNumber { get; set; }

    }
}
