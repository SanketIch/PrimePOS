using System;
using System.Collections.Generic;
using System.Text;

namespace MMS.GlobalPayments.Api.Entities
{
    public class LodgingItems
    {       
        public string Types { get; set; } 
        public string Reference { get; set; }
        public string TotalAmount { get; set; }
        public string[] paymentMethodProgramCodes { get; set; }
    }
}
