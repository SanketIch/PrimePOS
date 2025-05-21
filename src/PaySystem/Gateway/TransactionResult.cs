using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gateway
{
    /// <summary>
    /// Handles the Poperties that map the Results from a Transaction
    /// </summary>
    public class TransactionResult
    {
        #region "Private Vars"

        private ResultType _Type;

        private string statusField;

        private string resultField;

        private string historyidField;

        private string orderidField;

        private string refcodeField;

        private string authcodeField;

        private float totalField;

        private string merchantordernumberField;

        private string acctidField;

        private string subidField;

        private System.Nullable<System.DateTime> transdateField;

        private string paytypeField;

        private int duplicateField;

        private string avsresultField;

        private string cvv2resultField;

        private string batchnumberField;

        private string last4digitsField;

        private string entrymethodField;

        private int partialapprovalField;

        private string actioncodeField;

        private string additionaldataField;

        //Transaaction result

        private string balanceField;

        private string recurbillingamountField;

        private string recurnextbillingdateField;

        private string recurbillingcycleField;

        private string recurbillingmaxField;

        private string recurcanceldateField;

        private string recurlastattemptedField;

        private string recurbillingstatusField;

        private string recurtotalacceptField;

        private string achrcodeField;

        private string transactiontypeField;

              

        //Profile result members

        private string userprofileidField;

        private string paymentprofileidField;

        private string shippingprofileidField;

        //private string ccnum_decryptField;

        //private string expdate_decryptField;

        private string billaddr1Field;

        private string billaddr2Field;

        private string billcityField;

        private string billstateField;

        private string billzipField;

        private string shipaddr1Field;

        private string shipaddr2Field;

        private string shipcityField;

        private string shipstateField;

        private string shipzipField;






        #endregion

        #region "Properties"
        /// <summary>
        /// Gives the Result type
        /// Regular: for regilar payment Transaction
        /// Profile: For Stored Profile Transaction
        /// </summary>
        public ResultType Type
        {
            /*set
            {
                this._Type = value;
            }*/
            get
            {
                return this._Type;
            }
        }


        /// <summary>
        /// Returns if the transaction was Declined or Approved
        /// </summary>
        public  string Status
        {
            get
            {
                return this.statusField;
            }
            
        }

        /// <summary>
        /// Returns the colon delimitted return code
        /// Approval: string of 8 fields delimited by colon character of format Transaction_Type:Authorization_Code:Reference_Number:Batch_Number:Transaction_ID:AVS_Result_Code:Auth_Net_Message:CVV2/CVC2_result_Code:Partial_Auth
        /// Decline: String Declined followed by 2 fields delimitted by colon of format DECLINE:Decline_Code:Text_Response
        /// </summary>
        public string Result
        {
            get
            {
                return this.resultField;
            }
           
        }

        /// <summary>
        /// History Id key of the original Transaction Will be required for Voids/ refunds
        /// </summary>
        public string HistoryId
        {
            get
            {
                return this.historyidField;
            }
            
        }

        /// <summary>
        /// order ID Key of the original Transaction Will be required for voids/ refunds
        /// </summary>
        public string OrderId
        {
            get
            {
                return this.orderidField;
            }
            
        }

        /// <summary>
        /// Reference code Identical to history ID
        /// </summary>
        public string RefCode
        {
            get
            {
                return this.refcodeField;
            }
           
        }

        /// <summary>
        /// Aithorization Response The six digit authorization or approval code provided by the authorizing network
        /// </summary>
        public string AuthCode
        {
            get
            {
                return this.authcodeField;
            }
            
        }

        /// <summary>
        /// Transaction $ amount in format 0.00
        /// </summary>
        public float TotalAmt
        {
            get
            {
                return this.totalField;
            }
           
        }

        /// <summary>
        /// Customers Unique Alpha-Numeric Number
        /// </summary>
        public string MerchantOrderNumber
        {
            get
            {
                return this.merchantordernumberField;
            }
            
        }

       /// <summary>
        /// If Subid LoadBalancing is enabled, it will return the Acctid it was processed under.
       /// </summary>
        public string AccountId
        {
            get
            {
                return this.acctidField;
            }
            
        }

        /// <summary>
        /// If Subid LoadBalancing is enabled, it will return the Subid it was processed under.
        /// </summary>
        public string SubId
        {
            get
            {
                return this.subidField;
            }
            
        }

        /// <summary>
        /// Date and time of the transaction. The date/time is in ISO 8601 format:CCYY-MM-DDThh:mm:ss, with a suffix of "Z".
        /// </summary>
        public System.Nullable<System.DateTime> TransDate
        {
            get
            {
                return this.transdateField;
            }
            
        }

        /// <summary>
        /// The payment type used to process the transaction (ie, Master card, Visa, Discover, Amex, and Check).
        /// </summary>
        public string PayType
        {
            get
            {
                return this.paytypeField;
            }
            
        }

        /// <summary>
        /// "0" or "1". Default of "0" will be returned. A "1" will indicate that a duplicate transaction has been detected. The result of the original transaction will be returned.
        /// </summary>
        public int Duplicate
        {
            get
            {
                return this.duplicateField;
            }
           
        }

        /// <summary>
        /// AVS Result Code
        /// </summary>
        public string AVSResult
        {
            get
            {
                return this.avsresultField;
            }
            
        }

        /// <summary>
        /// CVV2 Result Code
        /// </summary>
        public string Cvv2Result
        {
            get
            {
                return this.cvv2resultField;
            }
            
        }

        /// <summary>
        /// Batch Number the transaction was Assigned to
        /// </summary>
        public string BatchNumber
        {
            get
            {
                return this.batchnumberField;
            }
            
        }

        /// <summary>
        /// Last 4 digits of the credit card
        /// </summary>
        public string last4digits
        {
            get
            {
                return this.last4digitsField;
            }
            
        }

        /// <summary>
        /// Indicates how a transaction was received.
        /// </summary>
        public string EntryMethod
        {
            get
            {
                return this.entrymethodField;
            }
            
        }

        /// <summary>
        /// Default = 0, 1 = A Partial amount was processed.
        /// </summary>
        public int PartialApproval
        {
            get
            {
                return this.partialapprovalField;
            }
           
        }

        /// <summary>
        /// Code identifying the partial approval amount.
        /// </summary>
        public string ActionCode
        {
            get
            {
                return this.actioncodeField;
            }
            
        }

        /// <summary>
        /// Balance Inquiry amount.
        /// </summary>
        public string Balance
        {
            get
            {
                return this.balanceField;
            }
           
        }

       
        public string RecurBillingAmount
        {
            get
            {
                return this.recurbillingamountField;
            }
           
        }

       
        public string RecurNextBillingDate
        {
            get
            {
                return this.recurnextbillingdateField;
            }
            
        }

       
        public string RecurBillingCycle
        {
            get
            {
                return this.recurbillingcycleField;
            }
            
        }

        
        public string RecurBillingMax
        {
            get
            {
                return this.recurbillingmaxField;
            }
            
        }

     
        public string RecurCancelDate
        {
            get
            {
                return this.recurcanceldateField;
            }
            
        }

        
        public string RecurLastAttempted
        {
            get
            {
                return this.recurlastattemptedField;
            }
            
        }

        
        public string RecurBillingStatus
        {
            get
            {
                return this.recurbillingstatusField;
            }
            
        }

       
        public string RecurTotalAccept
        {
            get
            {
                return this.recurtotalacceptField;
            }
            
        }

        /// <summary>
        /// ACH Return Code
        /// </summary>
        public string AchrCode
        {
            get
            {
                return this.achrcodeField;
            }
            
        }

        /// <summary>
        /// Type of Transaction
        /// </summary>
        public string TransactionType
        {
            get
            {
                return this.transactiontypeField;
            }
           
        }

        /// <summary>
        /// XML string containing additional data
        /// </summary>
        public string AdditionalData
        {
            get
            {
                return this.additionaldataField;
            }
            
        }

        /// <summary>
        /// only for profile transaction
        /// The User Profile ID (Unique ID) assigned to the Profile. Required for subsequent Profile transactions.
        /// </summary>
        public string UserProfileID
        {
            get { return this.userprofileidField; }
        }


        public string PaymentProfileID
        {
            get { return this.paymentprofileidField; }
        }


        public string ShippingProfileID
        {
            get { return this.shippingprofileidField; }
        }


        public string BillingAddr1
        {
            get { return this.billaddr1Field; }
        }


        public string BillingAddr2
        {
            get { return this.billaddr2Field; }
        }


        public string BillingAddrCity
        {
            get { return this.billcityField; }
        }

        public string BillingAddrState
        {
            get { return this.billstateField; }
        }


        public string BillingAddrZip
        {
            get { return this.billzipField; }
        }

        #endregion

        #region "Load Transmission Result"
        internal void LoadResultData(PrismPay.ProcessResult oresult)
        {
            this._Type = ResultType.Regular;
            this.statusField = oresult.status;
            this.resultField = oresult.result;
            this.historyidField = oresult.historyid;
            this.orderidField = oresult.orderid;
            this.refcodeField = oresult.refcode;
            this.authcodeField = oresult.authcode;
            this.totalField = oresult.total;
            this.merchantordernumberField = oresult.merchantordernumber;
            this.acctidField = oresult.acctid;
            this.subidField = oresult.subid;
            this.transdateField = oresult.transdate;
            this.paytypeField = oresult.paytype;
            this.duplicateField = oresult.duplicate;
            this.avsresultField = oresult.avsresult;
            this.cvv2resultField = oresult.cvv2result;
            this.batchnumberField = oresult.batchnumber;
            this.last4digitsField = oresult.last4digits;
            this.entrymethodField = oresult.entrymethod;
            this.partialapprovalField = oresult.partialapproval;
            this.actioncodeField = oresult.actioncode;
            this.balanceField = oresult.balance;
            this.recurbillingamountField = oresult.recurbillingamount;
            this.recurnextbillingdateField = oresult.recurnextbillingdate;
            this.recurbillingcycleField = oresult.recurbillingcycle;
            this.recurbillingmaxField = oresult.recurbillingmax;
            this.recurcanceldateField = oresult.recurcanceldate;
            this.recurlastattemptedField = oresult.recurlastattempted;
            this.recurbillingstatusField = oresult.recurbillingstatus;
            this.recurtotalacceptField = oresult.recurtotalaccept;
            this.achrcodeField = oresult.achrcode;
            this.transactiontypeField = oresult.transactiontype;
            this.additionaldataField = oresult.additionaldata;

        }

        internal void LoadResultData(WorldPay.ProcessResult oresult)
        {
            this._Type = ResultType.Regular;
            this.statusField = oresult.status;
            this.resultField = oresult.result;
            this.historyidField = oresult.historyid;
            this.orderidField = oresult.orderid;
            this.refcodeField = oresult.refcode;
            this.authcodeField = oresult.authcode;
            this.totalField = oresult.total;
            this.merchantordernumberField = oresult.merchantordernumber;
            this.acctidField = oresult.acctid;
            this.subidField = oresult.subid;
            this.transdateField = oresult.transdate;
            this.paytypeField = oresult.paytype;
            this.duplicateField = oresult.duplicate;
            this.avsresultField = oresult.avsresult;
            this.cvv2resultField = oresult.cvv2result;
            this.batchnumberField = oresult.batchnumber;
            this.last4digitsField = oresult.last4digits;
            this.entrymethodField = oresult.entrymethod;
            this.partialapprovalField = oresult.partialapproval;
            this.actioncodeField = oresult.actioncode;
            this.balanceField = oresult.balance;
            this.recurbillingamountField = oresult.recurbillingamount;
            this.recurnextbillingdateField = oresult.recurnextbillingdate;
            this.recurbillingcycleField = oresult.recurbillingcycle;
            this.recurbillingmaxField = oresult.recurbillingmax;
            this.recurcanceldateField = oresult.recurcanceldate;
            this.recurlastattemptedField = oresult.recurlastattempted;
            this.recurbillingstatusField = oresult.recurbillingstatus;
            this.recurtotalacceptField = oresult.recurtotalaccept;
            this.achrcodeField = oresult.achrcode;
            this.transactiontypeField = oresult.transactiontype;
            this.additionaldataField = oresult.additionaldata;
        }

        #endregion

        #region"Load Profile Result"
        internal void LoadResultData(PrismPay.ProcessProfileResult oresult)
        {
            this._Type = ResultType.Profile;
            this.statusField = oresult.status;
            this.resultField = oresult.result;
            this.historyidField = oresult.historyid;
            this.orderidField = oresult.orderid;
            this.refcodeField = oresult.refcode;
            this.authcodeField = oresult.authcode;
            this.totalField = oresult.total;
            this.merchantordernumberField = oresult.merchantordernumber;
            this.acctidField = oresult.acctid;
            this.subidField = oresult.subid;
            this.transdateField = oresult.transdate;
            this.paytypeField = oresult.paytype;
            this.duplicateField = oresult.duplicate;
            this.avsresultField = oresult.avsresult;
            this.cvv2resultField = oresult.cvv2result;
            this.batchnumberField = oresult.batchnumber;
            this.last4digitsField = oresult.last4digits;
            this.entrymethodField = oresult.entrymethod;
            this.partialapprovalField = oresult.partialapproval;
            this.actioncodeField = oresult.actioncode;
            
            this.additionaldataField = oresult.additionaldata;

            this.userprofileidField = oresult.userprofileid;
            this.paymentprofileidField = oresult.paymentprofileid;
            this.shippingprofileidField = oresult.shippingprofileid;

            this.billaddr1Field = oresult.billaddr1;
            this.billaddr2Field = oresult.billaddr2;
            this.billcityField = oresult.billcity;
            this.billstateField = oresult.billstate;
            this.billzipField = oresult.billzip;
        }

        internal void LoadResultData(WorldPay.ProcessProfileResult oresult)
        {
            this._Type = ResultType.Profile;
            this.statusField = oresult.status;
            this.resultField = oresult.result;
            this.historyidField = oresult.historyid;
            this.orderidField = oresult.orderid;
            this.refcodeField = oresult.refcode;
            this.authcodeField = oresult.authcode;
            this.totalField = oresult.total;
            this.merchantordernumberField = oresult.merchantordernumber;
            this.acctidField = oresult.acctid;
            this.subidField = oresult.subid;
            this.transdateField = oresult.transdate;
            this.paytypeField = oresult.paytype;
            this.duplicateField = oresult.duplicate;
            this.avsresultField = oresult.avsresult;
            this.cvv2resultField = oresult.cvv2result;
            this.batchnumberField = oresult.batchnumber;
            this.last4digitsField = oresult.last4digits;
            this.entrymethodField = oresult.entrymethod;
            this.partialapprovalField = oresult.partialapproval;
            this.actioncodeField = oresult.actioncode;

            this.additionaldataField = oresult.additionaldata;

            this.userprofileidField = oresult.userprofileid;
            this.paymentprofileidField = oresult.paymentprofileid;
            this.shippingprofileidField = oresult.shippingprofileid;

            this.billaddr1Field = oresult.billaddr1;
            this.billaddr2Field = oresult.billaddr2;
            this.billcityField = oresult.billcity;
            this.billstateField = oresult.billstate;
            this.billzipField = oresult.billzip;
        }

        #endregion
    }
}
