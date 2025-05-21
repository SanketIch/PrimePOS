namespace MMS.GlobalPayments.Api.CommonData
{
    //NG SDKUPDATE 20/9/2022
    public class clsPOSDBConstants
    {
        public clsPOSDBConstants()
        {
        }

        #region PrimePOSErrorLog Constant
        public const string Log_ApplicationStarted = "Application Started";
        public const string Log_ApplicationLaunched = "Application Launched";
        public const string Log_ApplicationClosed = "Application Close";
        public const string Log_Exception_Occured = "Error Occured";
        public const string Log_Entering = "Entered";
        public const string Log_Success = "Success";
        public const string Log_Exiting = "Exited";
        public const string Log_CommunicatingRX = "Querying to PharmSQL DB";
        public const string Log_Module_Transaction = "POS Transaction ";
        public const string Log_Module_Login = "User Login";
        public const string Log_Module_PriceUpdate = "Price Update";
        public const string Log_Module_POStatusUpdate = "Purchase Order Status Update";
        public const string Log_Module_Transaction_Paymnet = "POS Transaction Payment ";
        public const string Log_Module_Sigpad = "Sig Pad ";
        public const string Log_Module_Payout = "POS Payout ";
        public const string Log_Intitialize_Object = "Intitialize Object";
        public const string Log_Dispose_Object = "Dispose Object";
        #endregion
    }
}
