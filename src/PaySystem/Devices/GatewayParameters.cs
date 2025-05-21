using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDevice
{
    /// <summary>
    /// Set the Gateway Parameters to process transaction
    /// </summary>
    public class GatewayParameters
    {
        /// <summary>
        /// Account ID that was given by the Processor / Gateway
        /// </summary>
        public string AccountID
        {
            set { Gateway.GatewayManager.AccountId = value; }
        }

        /// <summary>
        /// Set the subID for the gateway
        /// </summary>
        public string SubID
        {
            set { Gateway.GatewayManager.SubId = value; }
        }

        /// <summary>
        /// Set the Merchant PIN
        /// </summary>
        public string MerchantPIN
        {
            set { Gateway.GatewayManager.MerchantPin = value; }
        }
    }
}
