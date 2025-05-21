//Author : Ritesh
//Copy Right : © Micro Merchant Systems, Inc 2008
//Functionality Desciption : The purpose of this class is to provide response of PCCharge Server on any transaction.
//External functions:MMSDictioanary,XmlToKeys
//Known Bugs : None
//Start Date : 29 January 2008.

using System;
using System.Collections.Generic;
using System.Text;
using Hps.Exchange.PosGateway.Client;
using MMS.GlobalPayments.Api.Terminals.PAX;
using NLog;

namespace MMS.PROCESSOR
{
    //Author : Ritesh
    //Functionality Desciption : The purpose of this class is to provide response of PCCharge Server on any transaction.
    //External functions:MMSDictioanary,XmlToKeys
    //Known Bugs : None
    //Start Date : 29 January 2008.
    public abstract class PaymentResponse : IDisposable
    {
        private ILogger logger = LogManager.GetCurrentClassLogger();
        #region variables

        public String Result = String.Empty;
        public String AuthNo = String.Empty;
        public String TransactionNo = String.Empty;
        public String AmountApproved = String.Empty;
        //Added By SRT(Gaurav) Date : 18 NOV 2008
        //Mantis ID: 0000112
        public String ResultDescription = String.Empty;
        public String Balance = String.Empty;  //Added by Manoj 8/19/2011
        public String MaskedCardNo = String.Empty;
        public String Expiration = String.Empty;
        public String Address = String.Empty;
        public String ZIP = String.Empty;
        public String AccountNo = String.Empty;
        public String PaymentType = String.Empty; //PRIMEPOS-3526
        public String NBSPayType = String.Empty; //PRIMEPOS-3375
        public String BinValue = String.Empty; //PRIMEPOS-3372
        public String AccountHolderName = String.Empty; //PRIMEPOS-3372
        public String PreReadId = String.Empty; //PRIMEPOS-3372
        public String CashBack = String.Empty;
        public String ProfiledID = String.Empty;
        public string EntryMethod = string.Empty;
        //End Of Added By SRT(Gaurav)
        //Added By SRT(Gaurav) Date: 01-Dec-2008
        //Mantis ID: 0000136
        public String TransDate = String.Empty;
        //End Of Added By SRT(Gaurav)
        public String IsFSATransaction = String.Empty; //Added By Dharmendra
        public String AdditionalFundsRequired = String.Empty;
        public String CardType = String.Empty;
        public String PayProviderMessage = String.Empty; //PRIMEPOS-3344
        public Double TransactionAmount = 0; //PRIMEPOS-3344
        public String TransactionID = String.Empty; //PRIMEPOS-3333

        protected MMSDictionary<String, String> ResponseMsgAllKeys = null;
        protected XmlToKeys XmlToKeys = null;
        private Boolean Disposed = false;
        public PosResponse objPosResponse;
        public String sHpsTran_Amt;
        public String sHPSTran_Type;


        // Suraj added
        public string SignatureString = string.Empty;
        public string ticketNum = null;//NileshJ
                                       //

        #endregion variables

        public string StateTax = string.Empty;
        public string MunicipalTax = string.Empty;
        public string ReduceTax = string.Empty;
        public string TotalAmount = string.Empty;
        public string BaseAmount = string.Empty;

        #region constants

        protected const int SUCCESS = 0;
        protected const int FAILURE = 1;
        protected const String FILTER_NODE = "";

        #endregion constants

        #region Property

        public PosResponse HPSResponse
        {
            get
            {
                return objPosResponse;
            }
        }

        public String HPSTranAmount
        {
            get
            {
                return sHpsTran_Amt;
            }
        }

        public String HPSTransType
        {
            get
            {
                return sHPSTran_Type;
            }
        }

        private EmvReceiptTags _emvReceipt;
        public EmvReceiptTags EmvReceipt
        {
            get { return _emvReceipt; }
            set { _emvReceipt = value; }
        }

        #endregion Property

        #region public methods

        //Constructor

        /// <summary>
        /// Author : Ritesh
        /// Functionality Desciption : This method is constructor for the PaymentResponse class
        /// External functions:MMSDictioanary
        /// Known Bugs : None
        /// Start Date : 29 Jan 2008.
        /// </summary>
        public PaymentResponse()
        {
            logger.Trace("In PaymentResponse() Constructor");
            //Clear Fields
            ClearFields();
            ResponseMsgAllKeys = new MMSDictionary<String, String>();
            XmlToKeys = new XmlToKeys();
        }

        /// <summary>
        /// Author : Ritesh
        /// Functionality Desciption : This method is used to parse the response of PCCharge Payment server
        /// External functions:MMSDictioanary,XmlToKeys
        /// Known Bugs : MMSDictioanary
        /// Start Date : 14 Jan 2008.
        /// </summary>
        /// <param name="xmlMessage"></param>
        /// <param name="xmlFilterNodeName"></param>
        /// <param name="fields"></param>
        /// <param name="isFile"></param>
        /// <returns>int result</returns>
        public abstract int ParseResponse(String xmlResponse, String FilterNode);

        /// <summary>
        /// Author : Suraj
        /// Functionality Desciption : This method is used to parse the response of PAX Device
        /// External functions:SecureSubmit Specific
        /// </summary>
        /// <param name="deviceResponse"></param>
       //public virtual void ParsePAXResponse(SecureSubmit.Terminals.PAX.PaxDeviceResponse deviceResponse) {

        //}Commented by 02 Dec Amit 2020
        public virtual void ParsePAXResponse(PaxTerminalResponse deviceResponse)
        {

        }

        /// <summary>
        /// Author : Arvind
        /// Functionality Desciption : This method is used to parse the response of EVERTEC Device
        /// PRIMEPOS-2664
        /// </summary>
        /// <param name="deviceResponse"></param>
        public virtual void ParseEvertechResponse(String deviceResponse)
        {

        }//PRIMEPOS-2931
        //public abstract int ParseHPSResponse(PosResponse objPosResp);

        /// <summary>
        /// Author : Ritesh
        /// Functionality Desciption : This method is used to return all the tkeys of MMSDictioanary
        /// External functions:MMSDictioanary,XmlToKeys
        /// Known Bugs : MMSDictioanary
        /// Start Date : 29 Jan 2008.
        /// </summary>
        /// <returns>MMSDictioanary</returns>
        public MMSDictionary<String, String> GetAllKeys()
        {
            logger.Trace("In GetAllKeys()");
            return ResponseMsgAllKeys;
        }

        /// <summary>
        /// Author : Ritesh
        /// Functionality Desciption : This method is used to return all the tkeys of MMSDictioanary
        /// External functions:MMSDictioanary,XmlToKeys
        /// Known Bugs : MMSDictioanary
        /// Start Date : 29 Jan 2008.
        /// </summary>
        /// <returns>MMSDictioanary</returns>
        public void UpdateResponseMessage(MMSDictionary<String, String> MessageKeys)
        {
            logger.Trace("In UpdateResponseMessage()");
            ResponseMsgAllKeys.Clear();
            ResponseMsgAllKeys = MessageKeys;
        }

        /// <summary>
        /// Author : Ritesh
        /// Functionality Desciption : This method is used to return value of the tkeys of MMSDictioanary
        /// External functions:MMSDictioanary,XmlToKeys
        /// Known Bugs : MMSDictioanary
        /// Start Date : 29 Jan 2008.
        /// </summary>
        /// <param name="key"></param>
        /// <returns>String Value</returns>
        public String GetValue(string key)
        {
            logger.Trace("In GetValue()");
            String value = String.Empty;
            if (ResponseMsgAllKeys.TryGetValue(key, out value) == true)
                return value;
            else
                return string.Empty;
        }

        /// <summary>
        /// Author : Ritesh
        /// Functionality Desciption : This method is used clear the fields
        /// External functions:MMSDictioanary,XmlToKeys
        /// Known Bugs : MMSDictioanary
        /// Start Date : 29 Jan 2008.
        /// </summary>
        public void ClearFields()
        {
            logger.Trace("In ClearFields()");
            Result = String.Empty;
            AuthNo = String.Empty;
            TransactionNo = String.Empty;
            AmountApproved = String.Empty;
            ResultDescription = String.Empty;
            IsFSATransaction = String.Empty; // Added By Dharmendra (SRT)o n Nov-27-08
            AdditionalFundsRequired = String.Empty; // Added By Dharmendra (SRT) on Nov-27-08
            Balance = String.Empty; // Added by Manoj 8/19/2011
            if (ResponseMsgAllKeys != null)
                ResponseMsgAllKeys.Clear();
            //Added By Rohit Nair on Sept 15 2016 for EMV-8
            ProfiledID = string.Empty;
            EmvReceipt = null;
        }

        /// <summary>
        /// Author : Ritesh
        /// Functionality Desciption : Destructor
        /// External functions:MMSDictioanary,XmlToKeys
        /// Known Bugs : MMSDictioanary
        /// Start Date : 29 Jan 2008.
        /// </summary>
        ~PaymentResponse()
        {
            logger.Trace("PaymentResponse destructor\n");
            Dispose(false);
        }

        #endregion public methods

        #region IDisposable Members

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(Boolean disposing)
        {
            if (Disposed)
                return;

            if (disposing)
            {
                //Result = null;
                if (ResponseMsgAllKeys != null)
                {
                    ResponseMsgAllKeys.Clear();
                    ResponseMsgAllKeys = null;
                }
                XmlToKeys.Dispose();
                XmlToKeys = null;
            }
        }

        #endregion IDisposable Members
    }
    public class EmvReceiptTags
    {
        public string MerchantID { get; set; }
        public string TransID { get; set; }
        public string ApprovalCode { get; set; }
        public string Account { get; set; }
        public string AccountType { get; set; }
        public string ApprovedAmount { get; set; }
        public string TransType { get; set; }
        public string AppCrytogram { get; set; }
        public string AppTransactionCounter { get; set; }
        public string AppIndentifer { get; set; }
        public string AppPreferedName { get; set; }
        public string TerminalVerficationResult { get; set; }
        public string TransStatusInformation { get; set; }
        public string AuthorizationResposeCode { get; set; }
        public string TransRefNumber { get; set; }
        public string ValidationCode { get; set; }
        public string EntryLegend { get; set; }
        public string Verbiage { get; set; }

        public string CloudReceiptText { get; set; }

        //Added by Arvind for the Evertec Receipt 2664
        public string BatchNumber { get; set; }
        public string TraceNumber { get; set; }
        public string InvoiceNumber { get; set; }

        //Added by Arvind VANTIV PRIMEPOS-2636
        public string TerminalID { get; set; }
        public string ResponseCode { get; set; }
        public string ReferenceNumber { get; set; }
        public string TransactionID { get; set; }
        public string EbtBalance { get; set; }//PRIMEPOS-2786
        #region PRIMEPOS-2793
        public string ApplicationLabel { get; set; }
        public bool PinVerified { get; set; }
        public string LaneID { get; set; }
        public string CardLogo { get; set; }
        #endregion
        //PRIMEPOS-3000
        public bool IsEvertecForceTransaction { get; set; }

        //PRIMEPOS-3000
        public bool IsEvertecSign { get; set; }
        //PRIMEPOS-3000
        public string EvertecTaxBreakdown { get; set; }
        //PRIMEPOS-3000
        public string ControlNumber { get; set; }
        //PRIMEPOS-3000
        public string ATHMovil { get; set; }
        public string CcName { get; set; }//PRIMEPOS-2943
        public bool IsFsaCard { get; set; }//2990
    }
}