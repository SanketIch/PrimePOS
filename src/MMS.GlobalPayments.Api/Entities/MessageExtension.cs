using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace MMS.GlobalPayments.Api.Entities {
    public class MessageExtension
    {
        public string CriticalityIndicator { get; set; }
        public string MessageExtensionData { get; set; }
        public string MessageExtensionId { get; set; }
        public string MessageExtensionName { get; set; }
    }
}