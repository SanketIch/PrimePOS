using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;

namespace FirstMile
{
    public static class GatewaySettings
    {
        private static ILogger logger = LogManager.GetCurrentClassLogger();

        private static string _AccountId;
        private static string _MerchantPin;
        private static string _SubId;

        public static string SubId
        {
            get { return GatewaySettings._SubId; }
            set { GatewaySettings._SubId = value; }
        }



        public static string MerchantPin
        {
            get { return GatewaySettings._MerchantPin; }
            set { GatewaySettings._MerchantPin = value; }
        }


        /// <summary>
        /// Account Id of the Merchant. Required for Live Tranaction processing 
        /// </summary>
        public static string AccountId
        {
            get { return GatewaySettings._AccountId; }
            set { GatewaySettings._AccountId = value; }
        }

        internal static string GetGatewayTags()
        {
            logger.Debug("Getting Gatewat Settings Tag");
            string result = string.Empty;

            if(SubId.Length>0 && MerchantPin.Length>0 && AccountId.Length > 0)
            {
                result = GlobalConstantTags.ACCT_ID + AccountId+GlobalConstantTags.SUB_ID + SubId + GlobalConstantTags.MERCHANT_PIN + MerchantPin ;
            }
            else
            {
                string message = "Gateway Settings Parameters Missing";
                if (string.IsNullOrWhiteSpace(AccountId))
                {
                    message += " Account ID";
                }

                if (string.IsNullOrWhiteSpace(SubId))
                {
                    message += " Sub ID";
                }

                if (string.IsNullOrWhiteSpace(MerchantPin))
                {
                    message += "Merchant Pin";
                }



                logger.Fatal(message);
                throw new Exception(message);
            }

            return result;
        }

    }
}
