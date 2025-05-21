using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gateway
{
    /// <summary>
    /// Manages the selected gateway and sets the Account Id Used by other classes
    /// </summary>
    public static class GatewayManager
    {
        private static Gateway _SelectedGateway;//= Gateway.WorldPay;
        private static string _AccountId;
        private static string _MerchantPin;
        private static string _SubId;

        public static string SubId
        {
            get { return GatewayManager._SubId; }
            set { GatewayManager._SubId = value; }
        }



        public static string MerchantPin
        {
            get { return GatewayManager._MerchantPin; }
            set { GatewayManager._MerchantPin = value; }
        }
        

        /// <summary>
        /// Account Id of the Merchant. Required for Live Tranaction processing 
        /// </summary>
        public static string AccountId
        {
            get { return GatewayManager._AccountId; }
            set { GatewayManager._AccountId = value; }
        }


        /// <summary>
        /// Select the Gateway that will Process the transaction
        /// </summary>
        public static Gateway SelectedGateway
        {
            get { return GatewayManager._SelectedGateway; }
            set { GatewayManager._SelectedGateway = value; }
        }
    }
}
