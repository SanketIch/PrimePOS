//Author : ARVIND 
//Copy Right : © Micro Merchant Systems, Inc 2008
//Functionality Desciption : The purpose of this class is to make base processor for transactions.
//External functions:None   
//Known Bugs : None
using System;
using System.Collections.Generic;
using System.Xml;
using NLog;
using MMS.PROCESSOR;
using PossqlData;
using Vantiv;
using Vantiv.Responses;
using System.Linq;

namespace MMS.VANTIV
{
    //Author : ARVIND 
    //Functionality Desciption : The purpose of this class is to make base processor for transactions.
    //External functions:None   
    //Known Bugs : None
    public abstract class Processor : IDisposable
    {
        ILogger logger = LogManager.GetCurrentClassLogger();

        #region variables

        private VantivProcessor _Device;
        private VantivResponse _DeviceResponse;
        private string _DeviceRequestMessage;

        //Dictionary to validate req
        private MMSDictionary<String, MMSDictionary<String, String>> MandatoryKeys = null;
        private MMSDictionary<String, String> ValidKeys = null;
        private MMSDictionary<String, String> MessageFields = null;

        //Xml Helper
        private XmlToKeys XmlToKeys = null;
        private KeysToXml Fields = null;

        //
        //private PaymentResponse response = null;
        private String errorMessage = String.Empty;
        private String txnType = String.Empty;
        private String ResponseMessage = String.Empty;
        private MerchantInfo Merchant = null;
        private Boolean Disposed = false;
        XmlDocument xmlVANTIV = null;

        private decimal amount = 0.00m;
        #region PRIMEPOS-2761
        private string Ticketno = "";
        private string TransType = "";
        private string UserID = "";
        private string StationID = "";
        private string TransID = "";
        #endregion
        //
        #endregion

        #region constants
        private readonly string PAYMENTPROCESSOR = "PAX|";

        private const String VALID_FIELDS = "ValidFields.xml";
        private const String MANDATORY_FIELDS = "MandatoryFields.xml";
        public const String FAILED_OPRN = "FAILED";
        public const String INVALID_PARAMETERS = "INVALID_PARAMETERS";
        private const string COMMONPROCESSORTAG = "CommonProcessorTag.xml";
        private const string ERROR_COMM_RESPONSE = "Communication Error";
        private string _ErrorMessage = string.Empty;//PRIMEPOS-3156
        private string[] DEVICE_RESPONSE_CODES_FOR_LDR = { "NONE", "FAILED", "HOSTERROR", "TIMEOUT", "CARDERROR", "DEVICEERROR", "PINPADERROR", "PINPADTIMEOUT", "APPROVEDBYMERCHANT", "CARDREMOVED", "APPROVEDEXCEPTCASHBACK", "APPLICATIONBLOCKED" }; //PRIMEPOS-3156 //PRIMEPOS-3526
        private bool isExceptionOccured = false; //PRIMEPOS-3156
        private static MMSDictionary<String, String> TXN_TYPE_MAP = null;
        private static Dictionary<string, string> REQUEST_SIGNATURE_PARAM = new Dictionary<string, string>();

        static Processor()
        {

            TXN_TYPE_MAP = new MMSDictionary<string, string>() {
                //Credit txn Code
                { "VANTIV_CREDIT_SALE", "01" },
                { "VANTIV_CREDIT_RETURN","02" },
                { "VANTIV_CREDIT_VOID", "16" },
                { "VANTIV_CREDIT_PREAUTH", "03" },
                { "VANTIV_CREDIT_POSTAUTH", "04" },
                { "VANTIV_CREDIT_REVERSE", "99" },
                //Debit txn Code
                { "VANTIV_DEBIT_SALE", "01" },
                { "VANTIV_DEBIT_RETURN", "02" },
                { "VANTIV_DEBIT_VOID", "16" },
                { "VANTIV_DEBIT_VOID_RETURN", "18" },
                { "VANTIV_DEBIT_AUTH", "03" },
                { "VANTIV_DEBIT_REVERSE", "99" },
                //EBT txn Code
                { "VANTIV_EBT_SALE", "01" },
                { "VANTIV_EBT_RETURN", "02" },
                { "VANTIV_NBS_SALE", "01" }, //PRIMEPOS-3372
                { "VANTIV_NBS_RETURN", "02" } //PRIMEPOS-3372
            };

            //Add Signature Constants by default to get signature data
            REQUEST_SIGNATURE_PARAM = new Dictionary<string, string>() {
                { "SIGN", "1" },
                { "GETSIGN", "1" },
                { "SIGNATURECAPTURE", "1" }
            };

        }

        //
        #endregion

        /// <summary>
        /// Author : ARVIND 
        /// Functionality Desciption : This method is constructor for the CreditProcessor class
        /// External functions:None
        /// Known Bugs : None
        /// </summary>
        /// <param name="ProcessorKey"></param>
        /// <param name="merchant">MerchantInfo</param>
        public Processor(string ProcessorKey, MerchantInfo merchant)
        {


            //For e.g. ProcessorKey = "Credit"
            //Read XML file for ValidFields.xml to make a list of all the valid Keys in a credit/debit card message.
            //Store the list fo the same in the ValidKeys (MMSDictionary/Hashtable)
            ValidKeys = new MMSDictionary<String, String>();
            XmlToKeys = new XmlToKeys();
            XmlToKeys.GetFields(VALID_FIELDS, ProcessorKey, ref ValidKeys, true);

            //This will be reference for Data passed
            MessageFields = null;

            //Create instacnce of ValidKeys
            MandatoryKeys = new MMSDictionary<String, MMSDictionary<String, String>>();

            //Create instance for the KeysToXml for Converting Message.
            Fields = new KeysToXml();

            //Create object of PaymentResponse
            _DeviceResponse = new VantivResponse();
            xmlVANTIV = new XmlDocument();
            xmlVANTIV.Load(COMMONPROCESSORTAG);
        }

        public void InitDevice(string _Url, string _ApplicationName, string _StationID, string _triPOS)
        {

            string Url = _Url.Split('|')[0];
            string LaneID = _Url.Split('|')[1];

            this._Device = new VantivProcessor(Url, LaneID, _ApplicationName, _StationID, _triPOS);

            //if (shouldInitDevice)
            //{
            //    _Device.Initialize();
            //}
        }

        /// <summary>
        /// Author : ARVIND 
        /// Functionality Desciption : This method is used clear the fields
        /// External functions:PaymentResponse
        /// Known Bugs : None
        /// </summary>
        private void ClearFields()
        {
            if (_DeviceResponse != null)
                _DeviceResponse.ClearFields();
            if (_DeviceResponse != null) //PRIMEPOS-2546 NJ Added for sign coming in for old transaction and test response handling 
                _DeviceResponse = null;

            errorMessage = String.Empty;
            txnType = String.Empty;
        }
        /// <summary>
        /// Author : ARVIND 
        /// Functionality Desciption : This method is used to Pad spaces for the processor
        /// External functions:MMSDictionary,KesyToXml
        /// Known Bugs : None
        /// </summary>
        /// <param name="value"></param>
        /// <param name="count"></param>
        /// <returns>String</returns>
        private String PadSpaces(String value, int count)
        {
            return value.ToString().PadRight(count, ' ');
        }

        /// <summary>
        /// Author : ARVIND 
        /// Functionality Desciption : This method is used to Build request sent to PaymentServer.
        /// External functions:KesyToXml
        /// Known Bugs : None
        /// </summary>
        /// <returns>String</returns>      

        //Use this for processing the transaction of any type.
        /// <summary>
        /// Author : ARVIND 
        /// Functionality Desciption : This method is the core method that does all the work
        /// External functions:MMSDictioanary,PaymentResponse,SocketClient
        /// Known Bugs : None
        /// </summary>
        /// <param name="transactionType"></param>
        /// <param name="requestMsgKeys"></param>
        /// <returns>PaymentResponse</returns> 

        protected PaymentResponse ProcessTxn(string transactionType, ref MMSDictionary<String, String> requestMsgKeys)
        {
            logger.Trace("Start Processor (PAX ProcessTxn) ----");
            //Clear all the fields.                 
            ClearFields();
            MessageFields = requestMsgKeys;
            txnType = transactionType;
            //NileshJ-ARVIND - 12-Dec-2018
            if (_DeviceResponse == null)
                _DeviceResponse = new VantivResponse();
            //Till - NileshJ-ARVIND
            //if (transactionType == "VANTIV_CANCEL")
            //{

            //    //_DeviceResponse = _Device.Cancel();

            //    //if (_DeviceResponse != null)
            //    //{
            //    //    response.ParsePAXResponse(_DeviceResponse);
            //    //}
            //    //else
            //    //{
            //    //    errorMessage = ERROR_COMM_RESPONSE;
            //    //    logger.Error("***PAX , Error: " + ERROR_COMM_RESPONSE);
            //    //}
            //    return response;
            //}

            if (IsValidRequest())
            {
                logger.Trace("Sending Data to VANTIV");
                isExceptionOccured = false; //PRIMEPOS-3156
                _DeviceResponse = SendReceiveData();
                try
                {
                    if ((!String.IsNullOrWhiteSpace(_DeviceResponse.ExpressResponseCode) && !String.IsNullOrWhiteSpace(_DeviceResponse.HostResponseMessage))
                    &&
                    (_DeviceResponse.ExpressResponseCode == "1001"
                    || _DeviceResponse.ExpressResponseCode == "1002"
                    || _DeviceResponse.HostResponseMessage?.ToUpper() == "TIMED OUT"))
                    {
                        _DeviceResponse = _Device.CreditCardReversal(MessageFields);

                        try
                        {
                            using (var db = new Possql())
                            {
                                CCTransmission_Log cclog = new CCTransmission_Log();
                                cclog.TransDateTime = DateTime.Now;
                                cclog.TransAmount = amount;
                                cclog.TransDataStr = _DeviceResponse.deviceRequest;
                                cclog.RecDataStr = _DeviceResponse.deviceResponse;
                                db.CCTransmission_Logs.Add(cclog);
                                db.SaveChanges();
                            }
                            _DeviceResponse.Result = _DeviceResponse.ResultDescription = " The Transaction was reversed due to some issue ";
                        }
                        catch (Exception exp)
                        {
                            logger.Error(exp.ToString());
                            return _DeviceResponse;// NileshJ Temp - 20-Dec-2018
                        }
                    }
                    if (_DeviceResponse != null && !isExceptionOccured) //PRIMEPOS-3156
                    {
                        //response.ParsePAXResponse(_DeviceResponse);
                        logger.Trace("VANTIV Device Response code - " + _DeviceResponse.Result + " Description - " + _DeviceResponse.ResultDescription);
                    }
                    else
                    {
                        #region PRIMEPOS-3156
                        //errorMessage = ERROR_COMM_RESPONSE;
                        //logger.Error("***VANTIV , Error: " + ERROR_COMM_RESPONSE);
                        //_DeviceResponse = null; //NileshJ Temp - 12-Dec-2018

                        errorMessage = ERROR_COMM_RESPONSE;
                        //logger.Error("***VANTIV , Error: " + ERROR_COMM_RESPONSE);
                        logger.Error("***VANTIV , Error: " + string.Concat(errorMessage, "\n", _ErrorMessage));

                        _DeviceResponse = new VantivResponse
                        {
                            ResultDescription = string.Concat(errorMessage, "\n", _ErrorMessage),
                            Result = "FAILED"
                        };
                        #endregion
                    }
                    logger.Trace("END PROCESSOR(VANTIV ProcessTxn)");
                    return _DeviceResponse;
                }
                catch (Exception)
                {
                    //PRIMEPOS-3156
                    errorMessage = ERROR_COMM_RESPONSE;
                    //logger.Error("***VANTIV , Error: " + ERROR_COMM_RESPONSE);
                    logger.Error("***VANTIV , Error: " + string.Concat(errorMessage, "\n", _ErrorMessage));

                    _DeviceResponse = new VantivResponse
                    {
                        ResultDescription = string.Concat(errorMessage, "\n", _ErrorMessage),
                        Result = "FAILED"
                    };
                    return _DeviceResponse;
                    //PRIMEPOS-3156 tiil here
                }
            }
            return null;
        }


        //Move to PAXResponse parse method



        /// <summary>
        /// Author : ARVIND 
        /// Functionality Desciption : This method is called to Open,Send,Receive and disconnect the Payment Server.
        /// External functions:None
        /// Known Bugs : None
        /// </summary>
        /// <param name="strRequest">String</param>
        /// <returns>bool</returns> 
        private VantivResponse SendReceiveData()
        {
            logger.Info("SendReceivedData() Start ");

            foreach (KeyValuePair<String, String> kvp in MessageFields)
            {

                if (kvp.Key.Trim().Equals("AMOUNT"))
                {
                    try
                    {
                        if (txnType == "VANTIV_DEBIT_RETURN" || txnType == "VANTIV_CREDIT_RETURN" || txnType == "VANTIV_EBT_RETURN")
                            amount = Math.Abs(Convert.ToDecimal(kvp.Value.ToString()));
                        else
                            amount = Convert.ToDecimal(kvp.Value.ToString());

                    }
                    catch (Exception ex)
                    {
                        logger.Trace(ex.ToString());
                    }

                }
                #region PRIMEPOS-2761
                if (kvp.Key.Trim().Equals("TICKETNUMBER"))
                {
                    try
                    {
                        Ticketno = kvp.Value.ToString();
                    }
                    catch (Exception ex)
                    {
                        logger.Trace(ex.ToString());
                    }
                }

                if (kvp.Key.Trim().Equals("TRANSACTIONTYPE"))
                {
                    try
                    {
                        TransType = kvp.Value.ToString();
                    }
                    catch (Exception ex)
                    {
                        logger.Trace(ex.ToString());
                    }
                }

                if (kvp.Key.Trim().Equals("TRANSACTIONID"))
                {
                    try
                    {
                        TransID = kvp.Value.ToString();
                    }
                    catch (Exception ex)
                    {
                        logger.Trace(ex.ToString());
                    }
                }

                if (kvp.Key.Trim().Equals("USERID"))
                {
                    try
                    {
                        UserID = kvp.Value.ToString();
                    }
                    catch (Exception ex)
                    {
                        logger.Trace(ex.ToString());
                    }
                }
                if (kvp.Key.Trim().Equals("StationID"))
                {
                    try
                    {
                        StationID = kvp.Value.ToString();
                    }
                    catch (Exception ex)
                    {
                        logger.Trace(ex.ToString());
                    }
                }
                #endregion

            }
            //String reqMessage = PAYMENTPROCESSOR + BuildRequest();
            // PRIMEPOS - 2532 added to get device message to get message back 
            //_Device.OnMessageSent += OnMessageSent_Handler;  

            #region PRIMEPOS-2761
            long TransNo = 0;
            long OrgTransNo = 0;
            string tempresp = string.Empty; //PRIMEPOS-3156
            string temprespdesc = string.Empty; //PRIMEPOS-3156
            bool isLocalDeatilCalled = false; //PRIMEPOS-3156
            CCTransmission_Log cclog = new CCTransmission_Log(); //PRIMEPOS-3156
            using (var db = new Possql())
            {
                //CCTransmission_Log cclog = new CCTransmission_Log(); //PRIMEPOS-3156
                cclog.TransDateTime = DateTime.Now;
                cclog.TransAmount = amount;
                cclog.TicketNo = Ticketno;
                //cclog.TransDataStr = _DeviceRequestMessage;
                cclog.PaymentProcessor = "VANTIV";
                cclog.StationID = StationID;
                cclog.UserID = UserID;
                cclog.TransmissionStatus = "InProgress";
                cclog.TransType = TransType;
                db.CCTransmission_Logs.Add(cclog);
                db.SaveChanges();
                db.Entry(cclog).GetDatabaseValues();
                TransNo = cclog.TransNo;
            }
            #endregion

            try
            {
                switch (txnType)
                {

                    case "VANTIV_CREDIT_SALE":
                    case "VANTIV_DEBIT_SALE":
                        #region PRIMEPOS-3156 LOCAL DETAILS REPORT 
                        //if (!MessageFields.ContainsKey("TOKEN"))
                        //    _DeviceResponse = _Device.Sale(MessageFields);
                        //else
                        //    _DeviceResponse = _Device.CreditCardSale(MessageFields);
                        //break;

                        bool isTransactionRecover = false;

                        bool.TryParse(MessageFields.Where(a => a.Key == "TransactionRecover").SingleOrDefault().Value, out isTransactionRecover);
                        if (isTransactionRecover)
                        {
                            logger.Trace("Entered in LocalDetailReport to get the last transaction: ");
                            _DeviceResponse = _Device.LocalDetailsReport(cclog.TicketNo, MessageFields);
                            if (_DeviceResponse != null)
                                logger.Trace("Device reponse is : " + _DeviceResponse.ToString());
                            else
                                logger.Trace("Device reponse is null: ");
                        }
                        else
                        {
                            if (!MessageFields.ContainsKey("TOKEN"))
                                _DeviceResponse = _Device.Sale(MessageFields);
                            else
                                _DeviceResponse = _Device.CreditCardSale(MessageFields);

                            if (_DeviceResponse != null)
                            {
                                logger.Trace("Sale Transaction Device reponse is : " + _DeviceResponse.ToString());
                                if (Array.Exists(DEVICE_RESPONSE_CODES_FOR_LDR, ele => ele == _DeviceResponse.Result.ToUpper()))
                                {
                                    tempresp = _DeviceResponse.Result;
                                    temprespdesc = _DeviceResponse.ResultDescription;
                                    isLocalDeatilCalled = true;
                                    logger.Trace($"Entered in LocalDetailReport to get the last Sale transaction: {cclog.TicketNo}");
                                    _DeviceResponse = _Device.LocalDetailsReport(cclog.TicketNo, MessageFields);
                                    if (_DeviceResponse != null)
                                    {
                                        if (_DeviceResponse.Result == "90")
                                        {
                                            _DeviceResponse.Result = tempresp;
                                            _DeviceResponse.ResultDescription = temprespdesc;
                                        }
                                        logger.Trace("Device reponse is : " + _DeviceResponse.ToString());
                                    }
                                    else
                                        logger.Trace("Device reponse is null: ");
                                }
                            }
                            else
                            {
                                logger.Trace("Credit Transaction Device reponse is null");
                            }

                            //if (MessageBox.Show("Do you want to use local report", "Vantive", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                            //{
                            //    _DeviceResponse = null;
                            //}

                        }

                        #endregion PRIMEPOS-3156 LOCAL DETAILS REPORT
                        break;
                    case "VANTIV_PREREAD":
                            _DeviceResponse = _Device.PreRead(MessageFields); //PRIMEPOS-3526
                        break;
                    case "VANTIV_PREREAD_SALE":
                        _DeviceResponse = _Device.PreReadSale(MessageFields); //PRIMEPOS-3526
                        break;
                    case "VANTIV_PREREAD_CANCEL":
                        _DeviceResponse = _Device.PreReadCancel(MessageFields); //PRIMEPOS-3526
                        break;
                    case "VANTIV_PREREAD_RETURN":
                        _DeviceResponse = _Device.PreReadRefund(MessageFields); //PRIMEPOS-3522
                        break;
                    case "VANTIV_CREDIT_VOID":
                    case "VANTIV_DEBIT_VOID":
                    case "VANTIV_NBS_VOID":
                    case "VANTIV_DEBIT_VOID_RETURN":
                    case "VANTIV_EBT_VOID":
                        #region  PRIMEPOS-3156 LOCAL DETAILS REPORT 
                        //_DeviceResponse = _Device.Void(MessageFields);

                        isTransactionRecover = false;

                        bool.TryParse(MessageFields.Where(a => a.Key == "TransactionRecover").SingleOrDefault().Value, out isTransactionRecover);

                        if (isTransactionRecover)
                        {
                            logger.Trace("Entered in EBT return or reverse LocalDetailReport to get the last transaction: ");
                            _DeviceResponse = _Device.LocalDetailsReport(cclog.TicketNo, MessageFields);
                            if (_DeviceResponse != null)
                                logger.Trace("Device reponse is : " + _DeviceResponse.ToString());
                            else
                                logger.Trace("Device reponse is null: ");
                        }
                        else
                        {
                            _DeviceResponse = _Device.Void(MessageFields);
                            #region PRIMEPOS-3156
                            if (_DeviceResponse != null) //PRIMEPOS-3156
                            {
                                logger.Trace("VOID Transaction Device reponse is : " + _DeviceResponse.ToString());
                                if (Array.Exists(DEVICE_RESPONSE_CODES_FOR_LDR, ele => ele == _DeviceResponse.Result.ToUpper()))
                                {
                                    tempresp = _DeviceResponse.Result;
                                    temprespdesc = _DeviceResponse.ResultDescription;
                                    isLocalDeatilCalled = true;
                                    logger.Trace($"Entered in LocalDetailReport to get the last VOID transaction: {cclog.TicketNo}");
                                    _DeviceResponse = _Device.LocalDetailsReport(cclog.TicketNo, MessageFields);
                                    if (_DeviceResponse != null)
                                    {
                                        if (_DeviceResponse.Result == "90")
                                        {
                                            _DeviceResponse.Result = tempresp;
                                            _DeviceResponse.ResultDescription = temprespdesc;
                                        }
                                        logger.Trace("Device reponse is : " + _DeviceResponse.ToString());
                                    }
                                    else
                                        logger.Trace("Device reponse is null: ");
                                }
                            }
                            else
                            {
                                logger.Trace("VOID Transaction Device reponse is null");
                            }
                            #endregion
                            //if (MessageBox.Show("Do you want to use local report", "Vantive", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                            //{
                            //    _DeviceResponse = null;
                            //}
                        }
                        #endregion PRIMEPOS-3156 LOCAL DETAILS REPORT

                        break;
                    case "VANTIV_CREDIT_RETURN":
                    case "VANTIV_CREDIT_PREAUTH":
                    case "VANTIV_CREDIT_POSTAUTH":
                    case "VANTIV_DEBIT_RETURN":
                    case "VANTIV_DEBIT_REVERSE":
                    case "VANTIV_DEBIT_Auth":
                    case "VANTIV_CREDIT_REVERSE":
                        #region PRIMEPOS-3156 LOCAL DETAILS REPORT 

                        //if (!MessageFields.ContainsKey("TRANSACTIONID"))
                        //    _DeviceResponse = _Device.Refund(MessageFields);
                        //else
                        //    //_DeviceResponse = _Device.CreditCardReturn(MessageFields);
                        //    _DeviceResponse = _Device.StrictReturn(MessageFields);
                        //// _DeviceResponse = _Device.DoDebit(TXN_TYPE_MAP[txnType], amountDesc, accountDesc, traceDesc
                        ////, cashierDesc, extDataDesc);
                        ///
                        isTransactionRecover = false;

                        bool.TryParse(MessageFields.Where(a => a.Key == "TransactionRecover").SingleOrDefault().Value, out isTransactionRecover);

                        if (isTransactionRecover)
                        {
                            logger.Trace("Entered in EBT return or reverse LocalDetailReport to get the last transaction: ");
                            _DeviceResponse = _Device.LocalDetailsReport(cclog.TicketNo, MessageFields);
                            if (_DeviceResponse != null)
                                logger.Trace("Device reponse is : " + _DeviceResponse.ToString());
                            else
                                logger.Trace("Device reponse is null: ");
                        }
                        else
                        {
                            if (!MessageFields.ContainsKey("TRANSACTIONID"))
                                _DeviceResponse = _Device.Refund(MessageFields);
                            else
                                //_DeviceResponse = _Device.CreditCardReturn(MessageFields);
                                _DeviceResponse = _Device.StrictReturn(MessageFields);
                            // _DeviceResponse = _Device.DoDebit(TXN_TYPE_MAP[txnType], amountDesc, accountDesc, traceDesc
                            //, cashierDesc, extDataDesc);
                            //if (MessageBox.Show("Do you want to use local report", "Vantive", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                            //{
                            //    _DeviceResponse = null;
                            //}
                            #region PRIMEPOS-3156-01-12
                            if (_DeviceResponse != null) //PRIMEPOS-3156
                            {
                                logger.Trace("RETURN Transaction Device reponse is : " + _DeviceResponse.ToString());
                                if (Array.Exists(DEVICE_RESPONSE_CODES_FOR_LDR, ele => ele == _DeviceResponse.Result.ToUpper()))
                                {
                                    tempresp = _DeviceResponse.Result;
                                    temprespdesc = _DeviceResponse.ResultDescription;
                                    isLocalDeatilCalled = true;
                                    logger.Trace($"Entered in LocalDetailReport to get the last RETURN transaction: {cclog.TicketNo}");
                                    _DeviceResponse = _Device.LocalDetailsReport(cclog.TicketNo, MessageFields);
                                    if (_DeviceResponse != null)
                                    {
                                        if (_DeviceResponse.Result == "90")
                                        {
                                            _DeviceResponse.Result = tempresp;
                                            _DeviceResponse.ResultDescription = temprespdesc;
                                        }
                                        logger.Trace("Device reponse is : " + _DeviceResponse.ToString());
                                    }
                                    else
                                        logger.Trace("Device reponse is null: ");
                                }
                            }
                            else
                            {
                                logger.Trace("RETURN Transaction Device reponse is null");
                            }
                            #endregion
                        }
                        #endregion PRIMEPOS-3156 LOCAL DETAILS REPORT

                        break;
                    /* EBT Transaction*/
                    case "VANTIV_EBT_SALE":
                    case "VANTIV_EBT_Auth":
                        #region PRIMEPOS-3156 LOCAL DETAILS REPORT 

                        //_DeviceResponse = _Device.Sale(MessageFields);
                        ////_DeviceResponse = _Device.EBT(MessageFields);
                        ////  _DeviceResponse = _Device.DoEBT(TXN_TYPE_MAP[txnType], amountDesc, accountDesc, traceDesc
                        ////, cashierDesc);

                        isTransactionRecover = false;

                        bool.TryParse(MessageFields.Where(a => a.Key == "TransactionRecover").SingleOrDefault().Value, out isTransactionRecover);

                        if (isTransactionRecover)
                        {
                            logger.Trace("Entered in EBT LocalDetailReport to get the last transaction: ");
                            _DeviceResponse = _Device.LocalDetailsReport(cclog.TicketNo, MessageFields);
                            if (_DeviceResponse != null)
                                logger.Trace("Device reponse is : " + _DeviceResponse.ToString());
                            else
                                logger.Trace("Device reponse is null: ");
                        }
                        else
                        {
                            _DeviceResponse = _Device.Sale(MessageFields);

                            if (_DeviceResponse != null)
                            {
                                logger.Trace("EBT Sale Transaction Device reponse is : " + _DeviceResponse.ToString());
                                if (Array.Exists(DEVICE_RESPONSE_CODES_FOR_LDR, ele => ele == _DeviceResponse.Result.ToUpper()))
                                {
                                    tempresp = _DeviceResponse.Result;
                                    temprespdesc = _DeviceResponse.ResultDescription;
                                    isLocalDeatilCalled = true;
                                    logger.Trace($"Entered in LocalDetailReport to get the last EBT Sale transaction: {cclog.TicketNo}");
                                    _DeviceResponse = _Device.LocalDetailsReport(cclog.TicketNo, MessageFields);
                                    if (_DeviceResponse != null)
                                    {
                                        if (_DeviceResponse.Result == "90")
                                        {
                                            _DeviceResponse.Result = tempresp;
                                            _DeviceResponse.ResultDescription = temprespdesc;
                                        }
                                        logger.Trace("EBT Transaction Device reponse is : " + _DeviceResponse.ToString());
                                    }
                                    else
                                        logger.Trace("EBT Transaction Device reponse is null: ");
                                }
                            }
                            else
                            {
                                logger.Trace("EBT Transaction Device reponse is null");
                            }
                            //if (MessageBox.Show("Do you want to use local report", "Vantive", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                            //{
                            //    _DeviceResponse = null;
                            //}
                        }
                        #endregion PRIMEPOS-3156 LOCAL DETAILS REPORT

                        break;
                    case "VANTIV_EBT_RETURN":
                    case "VANTIV_EBT_REVERSE":
                        #region PRIMEPOS-3156 LOCAL DETAILS REPORT 
                        //if (MessageFields.ContainsKey("TRANSACTIONID"))
                        //    _DeviceResponse = _Device.StrictReturn(MessageFields);
                        //else
                        //{
                        //    _DeviceResponse = _Device.Void(MessageFields);
                        //}
                        isTransactionRecover = false;

                        bool.TryParse(MessageFields.Where(a => a.Key == "TransactionRecover").SingleOrDefault().Value, out isTransactionRecover);

                        if (isTransactionRecover)
                        {
                            logger.Trace("Entered in EBT return or reverse LocalDetailReport to get the last transaction: ");
                            _DeviceResponse = _Device.LocalDetailsReport(cclog.TicketNo, MessageFields);
                            if (_DeviceResponse != null)
                                logger.Trace("Device reponse is : " + _DeviceResponse.ToString());
                            else
                                logger.Trace("Device reponse is null: ");
                        }
                        else
                        {
                            if (MessageFields.ContainsKey("TRANSACTIONID"))
                                _DeviceResponse = _Device.StrictReturn(MessageFields);
                            else
                            {
                                _DeviceResponse = _Device.Refund(MessageFields);//PRIMEPOS-3063
                            }

                            #region PRIMEPOS-3156-01-12
                            if (_DeviceResponse != null) //PRIMEPOS-3156-01-12
                            {
                                logger.Trace("Device reponse is : " + _DeviceResponse.ToString());
                                if (Array.Exists(DEVICE_RESPONSE_CODES_FOR_LDR, ele => ele == _DeviceResponse.Result.ToUpper()))
                                {
                                    tempresp = _DeviceResponse.Result;
                                    temprespdesc = _DeviceResponse.ResultDescription;
                                    isLocalDeatilCalled = true;
                                    logger.Trace($"Entered in LocalDetailReport to get the last EBT RETURN transaction: {cclog.TicketNo}");
                                    _DeviceResponse = _Device.LocalDetailsReport(cclog.TicketNo, MessageFields);
                                    if (_DeviceResponse != null)
                                    {
                                        if (_DeviceResponse.Result == "90")
                                        {
                                            _DeviceResponse.Result = tempresp;
                                            _DeviceResponse.ResultDescription = temprespdesc;
                                        }
                                        logger.Trace("Device reponse is : " + _DeviceResponse.ToString());
                                    }
                                    else
                                        logger.Trace("Device reponse is null: ");
                                }
                            }
                            else
                            {
                                logger.Trace("EBT RETURN Transaction Device reponse is null");
                            }
                            #endregion

                            //if (MessageBox.Show("Do you want to use local report", "Vantive", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                            //{
                            //    _DeviceResponse = null;
                            //}

                        }
                        #endregion PRIMEPOS-3156 LOCAL DETAILS REPORT

                        break;
                    case "VANTIV_NBS_SALE":
                        #region PRIMEPOS-3372
                        _DeviceResponse = _Device.NBSSale(MessageFields);
                        #endregion
                        break;
                    case "VANTIV_NBS_PREREAD":
                        #region PRIMEPOS-3372
                        _DeviceResponse = _Device.NBSPreRead(MessageFields);
                        #endregion
                        break;
                    case "VANTIV_CANCEL":
                        #region PRIMEPOS-3372
                        _DeviceResponse = _Device.NBSCancel(MessageFields);
                        break;
                        #endregion
                    case "VANTIV_NBS_RETURN":
                        #region PRIMEPOS-3375
                        if (!MessageFields.ContainsKey("TRANSACTIONID"))
                            _DeviceResponse = _Device.NBSRefund(MessageFields); //PRIMEPOS-3407
                        else
                            _DeviceResponse = _Device.NBSSaleReturn(MessageFields);
                        break;
                        #endregion
                    default:
                        errorMessage = "INVALID_TXN_TYPE";
                        _DeviceResponse = null;
                        break;
                }
            }
            catch (Exception ex)
            {
                logger.Trace(ex.ToString());

                #region PRIMEPOS-3156
                if (ex.InnerException != null)
                {
                    _ErrorMessage = ex.InnerException.Message + "|" + TransNo.ToString();
                }
                else
                {
                    _ErrorMessage = ex.Message + "|" + TransNo.ToString();
                }
                isExceptionOccured = true; //PRIMEPOS-3156
                #endregion PRIMEPOS-3156 till here

                return _DeviceResponse;// NileshJ Temp - 20-Dec-2018
            }

            try
            {

                // PRIMEPOS - 2532 Modified cclog.TransDataStr,cclog.RecDataStr to add 
                //Request and Response of device to CCTransmission_Log table 
                #region  #region PRIMEPOS-2761 - commented 
                // using (var db = new Possql())
                //{
                //    CCTransmission_Log cclog = new CCTransmission_Log();
                //    cclog.TransDateTime = DateTime.Now;
                //    cclog.TransAmount = amount;
                //    cclog.TransDataStr = _DeviceResponse.deviceRequest;
                //    cclog.RecDataStr = _DeviceResponse.deviceResponse;
                //    db.CCTransmission_Logs.Add(cclog);
                //    db.SaveChanges();
                //}
                #endregion

                #region PRIMEPOS-2761
                bool CheckIsTransactionRecover = false;//PRIMEPOS-3156
                bool.TryParse(MessageFields.Where(a => a.Key == "TransactionRecover").SingleOrDefault().Value, out CheckIsTransactionRecover);//PRIMEPOS-3156

                using (var db = new Possql())
                {
                    CCTransmission_Log cclog1 = new CCTransmission_Log();
                    if (TransType.Contains("VOID") || TransType.Contains("VOID"))
                    {
                        cclog1 = db.CCTransmission_Logs.Where(w => w.HostTransID == TransID).SingleOrDefault();
                        cclog1.IsReversed = true;
                        OrgTransNo = cclog1.TransNo;
                        db.CCTransmission_Logs.Attach(cclog1);
                        db.Entry(cclog1).Property(p => p.IsReversed).IsModified = true;
                        db.SaveChanges();
                    }


                    CCTransmission_Log ccTranslog = new CCTransmission_Log(); //PRIMEPOS-3156
                    ccTranslog = db.CCTransmission_Logs.Where(w => w.TransNo == TransNo).SingleOrDefault();
                    ccTranslog.AmtApproved = amount;
                    //ccTranslog.TransDataStr = _DeviceResponse.deviceRequest;//Request Message should be saved still pending  //PRIMEOS-3156
                    if (CheckIsTransactionRecover || isLocalDeatilCalled)//PRIMEPOS-3156
                    {
                        ccTranslog.TransDataStr = "TransactionQuery ReferenceNumber " + cclog.TicketNo;
                    }
                    else
                    {
                        ccTranslog.TransDataStr = _DeviceResponse.deviceRequest;//Request Message should be saved still pending 
                    }
                    ccTranslog.RecDataStr = _DeviceResponse.deviceResponse;//Response Message saved
                    ccTranslog.HostTransID = _DeviceResponse.TransactionNo;
                    ccTranslog.TransmissionStatus = "Completed";
                    ccTranslog.OrgTransNo = OrgTransNo;
                    #region PRIMEPOS-3182
                    if (!string.IsNullOrWhiteSpace(_DeviceResponse.AccountNo) && _DeviceResponse.AccountNo.Trim().Length >= 4)
                    {
                        ccTranslog.last4 = _DeviceResponse.AccountNo.Trim().Substring(_DeviceResponse.AccountNo.Trim().Length - 4, 4);
                    }
                    #endregion
                    db.CCTransmission_Logs.Attach(ccTranslog);
                    db.Entry(ccTranslog).Property(p => p.TransDataStr).IsModified = true;
                    db.Entry(ccTranslog).Property(p => p.RecDataStr).IsModified = true;
                    db.Entry(ccTranslog).Property(p => p.TransmissionStatus).IsModified = true;
                    db.Entry(ccTranslog).Property(p => p.HostTransID).IsModified = true;
                    db.Entry(ccTranslog).Property(p => p.AmtApproved).IsModified = true;
                    db.Entry(ccTranslog).Property(p => p.OrgTransNo).IsModified = true;
                    db.Entry(ccTranslog).Property(p => p.last4).IsModified = true;   //PRIMEPOS-3182
                    db.SaveChanges();
                }
                #endregion
            }
            catch (Exception exp)
            {
                logger.Trace(exp.ToString());

                #region PRIMEPOS-3156
                if (exp.InnerException != null)
                {
                    _ErrorMessage = exp.InnerException.Message + "|" + TransNo.ToString();
                }
                else
                {
                    _ErrorMessage = exp.Message + "|" + TransNo.ToString();
                }
                isExceptionOccured = true; //PRIMEPOS-3156
                #endregion PRIMEPOS-3156 till here

                return _DeviceResponse;// NileshJ Temp - 20-Dec-2018
            }

            //End Added
            return _DeviceResponse;

        }

        //private void OnMessageSent_Handler(string message) {
        //    _DeviceRequestMessage = message;
        //    Console.WriteLine("PAX message Processor " + _DeviceRequestMessage);
        //}



        /// <summary>
        /// Author : ARVIND 
        /// Functionality Desciption : This method is called to validate the Valid Fields and Mandatory fields.
        /// External functions:None
        /// Known Bugs : None
        /// </summary>
        /// <returns>bool vaild</returns> 
        private bool IsValidRequest()
        {
            if (IsValidFields())
            {
                if (IsMandatoryFields())
                {
                    return true;
                }
            }
            return false;

            // we can write additional code for the invalid fields or missing fields
        }
        /// <summary>
        /// Author : ARVIND 
        /// Functionality Desciption : This method is used for checking the Valid Fields for a Processor.
        /// External functions:None
        /// Known Bugs : None
        /// </summary>
        /// <returns>bool vaild</returns>         
        private bool IsValidFields()
        {
            //compare all the fields agains list of valid fields "ValidKeys" if fine 
            foreach (KeyValuePair<String, String> kvp in MessageFields)
            {
                if (!ValidKeys.ContainsKey(kvp.Key))
                {
                    errorMessage = "INVALID FIELD:-" + kvp.Key + "   " + kvp.Value;
                    logger.Error(errorMessage);
                    return false;
                }
            }
            return true;
        }
        /// <summary>
        /// Author : ARVIND 
        /// Functionality Desciption : This method is used for checking the Mandatory Fields for a Transaction type.
        /// External functions:None
        /// Known Bugs : None
        /// </summary>
        /// <returns>bool vaild</returns>          
        private bool IsMandatoryFields()
        {
            MMSDictionary<String, String> keys = null;
            if (!MandatoryKeys.ContainsKey(txnType))
            {
                if (FetchMandatoryFields(txnType) == 0)
                {
                    errorMessage = "INVALID TRANSACTION TYPE:-" + txnType;
                    logger.Error(errorMessage);
                    return false;
                }
            }
            MandatoryKeys.TryGetValue(txnType, out keys);
            foreach (KeyValuePair<String, String> kvp in keys)
            {
                if (!MessageFields.ContainsKey(kvp.Key))
                {
                    errorMessage = "MISSING MANDATORY FIELD:-" + kvp.Key + "  " + kvp.Value;
                    logger.Error(errorMessage);
                    return false;
                }
            }
            keys = null;
            return true;
        }
        /// <summary>
        /// Author : ARVIND 
        /// Functionality Desciption : This method is used for fetching the mandatory fields of transaction from MMSDictionary or MandaotoryFields.xml file.
        /// External functions:MMSDictionary,XmlToKeys
        /// Known Bugs : None
        /// </summary>
        /// <param name="transactionType">String</param>
        /// <returns>int vaild</returns>
        private int FetchMandatoryFields(String transactionType)
        {
            int count = 0;
            String strXmlKey = transactionType.Trim();
            MMSDictionary<String, String> keys = new MMSDictionary<String, String>();
            count = XmlToKeys.GetFields(MANDATORY_FIELDS, strXmlKey, ref keys, true);
            if (count > 0)
                MandatoryKeys.Add(transactionType, keys);
            return count;
        }
        ~Processor()
        {
            System.Diagnostics.Debug.Print("Processor destructor\n");
            Dispose(false);
        }

        /// <summary>
        /// Author : ARVIND 
        /// Functionality Desciption : This method is used for ensuring valid parameters are passed (Null check).
        /// External functions:MMSDictionary,XmlToKeys
        /// Known Bugs : None
        /// </summary>
        /// <param name="keys">MMSDictioanary</param>
        /// <returns>bool vaild</returns>
        public bool ValidateParameters(ref MMSDictionary<String, String> keys)
        {
            logger.Trace("In ValidateParameters()");
            bool flag = true;
            bool isValid = true;
            string sError = string.Empty;
            const String VANTIV = "VANTIV";
            MMSDictionary<String, String> orignalKeys = new MMSDictionary<string, string>();
            MMSDictionary<String, String> revisedKeys = new MMSDictionary<string, string>();
            string orgParam = string.Empty;
            if (keys == null)
                return false;
            foreach (KeyValuePair<String, String> kvp in keys)
            {
                //Modified By SRT(Gaurav) Date : 25-NOV-2008
                if (kvp.Value != null)
                {
                    if (kvp.Value.Trim() != "")
                    {
                        revisedKeys.Add(kvp.Key, kvp.Value);
                    }
                }
            }

            keys.Clear();
            keys = revisedKeys;

            foreach (KeyValuePair<String, String> kvp in keys)
            {
                isValid = this.GetProcessorTag(kvp.Key, VANTIV, out orgParam);
                if (isValid == true)
                {
                    orignalKeys.Add(orgParam, kvp.Value);
                }
            }
            keys = orignalKeys;

            return flag;
        }
        public bool GetProcessorTag(string xmlCommonNode, string xmlProcessorName, out string ProcessorTag)
        {
            bool isValid = true;
            string tagValue = string.Empty;

            XmlNodeList xmlList;

            xmlList = xmlVANTIV.GetElementsByTagName(xmlCommonNode);
            XmlNodeList ProcessorNode = xmlList.Item(0).ChildNodes;

            for (int iCount = 0; iCount < ProcessorNode.Count; iCount++)
            {
                if (ProcessorNode.Item(iCount).Name == xmlProcessorName)
                    tagValue = ProcessorNode.Item(iCount).InnerText;
            }

            if (tagValue != null && tagValue != string.Empty)
            {
                isValid = true;
            }
            else
            {
                isValid = false;
            }
            ProcessorTag = tagValue;
            return isValid;
        }



        #region amount conversions
        private string ToDeviceAmount(string amount)
        {
            var decimalAmount = Convert.ToDecimal(amount);
            var deviceCatoredAmount = decimalAmount * 100;
            var formattedAmount = deviceCatoredAmount.ToString("0.##");
            return formattedAmount;
        }

        private string ToPOSAmount(string amount)
        {
            var decimalAmount = Convert.ToDecimal(amount);
            var POSCatoredAmount = decimalAmount / 100;
            var formattedAmount = POSCatoredAmount.ToString("0.##");
            return formattedAmount;
        }
        #endregion amount conversions


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
                if (MandatoryKeys != null)
                {
                    MandatoryKeys.Clear();
                    MandatoryKeys = null;
                }
                if (ValidKeys != null)
                {
                    ValidKeys.Clear();
                    ValidKeys = null;
                }
                //MessageFields = null;
                //PaymentConn = null;
                if (XmlToKeys != null)
                {
                    XmlToKeys.Dispose();
                    XmlToKeys = null;
                }
                if (Fields != null)
                {
                    Fields = null;
                }
                if (_DeviceResponse != null)
                {
                    _DeviceResponse.Dispose();
                    _DeviceResponse = null;
                }
                errorMessage = null;
                txnType = null;
                ResponseMessage = null;


            }



            // Unmanaged cleanup code here

            Disposed = true;

        }

        #endregion
    }
}
