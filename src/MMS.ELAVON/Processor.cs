//Author : ARVIND 
//Copy Right : © Micro Merchant Systems, Inc 2008
//Functionality Desciption : The purpose of this class is to make base processor for transactions.
//External functions:None   
//Known Bugs : None
using Elavon;
using Elavon.Response;
using MMS.PROCESSOR;
using NLog;
using PossqlData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace MMS.Elavon
{
    //Author : ARVIND 
    //Functionality Desciption : The purpose of this class is to make base processor for transactions.
    //External functions:None   
    //Known Bugs : None
    public abstract class Processor : IDisposable
    {
        ILogger logger = LogManager.GetCurrentClassLogger();

        #region variables

        private ElavonProcessor _Device;
        private ElavonResponse _DeviceResponse;
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
        XmlDocument xmlElavon = null;

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

        private static MMSDictionary<String, String> TXN_TYPE_MAP = null;
        private static Dictionary<string, string> REQUEST_SIGNATURE_PARAM = new Dictionary<string, string>();

        static Processor()
        {

            TXN_TYPE_MAP = new MMSDictionary<string, string>() {
                //Credit txn Code
                { "ELAVON_CREDIT_SALE", "01" },
                { "ELAVON_CREDIT_RETURN","02" },
                { "ELAVON_CREDIT_VOID", "16" },
                { "ELAVON_CREDIT_PREAUTH", "03" },
                { "ELAVON_CREDIT_POSTAUTH", "04" },
                { "ELAVON_CREDIT_REVERSE", "99" },
                //Debit txn Code
                { "ELAVON_DEBIT_SALE", "01" },
                { "ELAVON_DEBIT_RETURN", "02" },
                { "ELAVON_DEBIT_VOID", "16" },
                { "ELAVON_DEBIT_VOID_RETURN", "18" },
                { "ELAVON_DEBIT_AUTH", "03" },
                { "ELAVON_DEBIT_REVERSE", "99" },
                //EBT txn Code
                { "ELAVON_EBT_SALE", "01" },
                { "ELAVON_EBT_RETURN", "02" }
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
            //_DeviceResponse = new ElavonResponse();
            xmlElavon = new XmlDocument();
            xmlElavon.Load(COMMONPROCESSORTAG);
        }

        public void InitDevice(string _Url, string _ApplicationName, string _StationID)
        {

            string Url = _Url.Split('|')[0];
            string LaneID = _Url.Split('|')[1];

            //this._Device = new ElavonProcessor(Url, LaneID, _ApplicationName, _StationID,false);

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
            if (_DeviceResponse != null)
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

            if (_DeviceResponse == null)
                _DeviceResponse = new ElavonResponse();

            if (IsValidRequest())
            {
                logger.Trace("Sending Data to Elavon");
                _DeviceResponse = SendReceiveData();
                if (_DeviceResponse != null)
                {
                    logger.Trace("Elavon Device Response code - " + _DeviceResponse.Result + " Description - " + _DeviceResponse.ResultDescription);
                }
                else
                {
                    errorMessage = ERROR_COMM_RESPONSE;
                    logger.Error("***Elavon , Error: " + ERROR_COMM_RESPONSE);
                    _DeviceResponse = null; //NileshJ Temp - 12-Dec-2018
                }
                logger.Trace("END PROCESSOR(Elavon ProcessTxn)");
                return _DeviceResponse;
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
        private ElavonResponse SendReceiveData()
        {
            logger.Info("SendReceivedData() Start ");

            foreach (KeyValuePair<String, String> kvp in MessageFields)
            {

                if (kvp.Key.Trim().Equals("AMOUNT"))
                {
                    try
                    {
                        if (txnType == "ELAVON_DEBIT_RETURN" || txnType == "ELAVON_CREDIT_RETURN" || txnType == "ELAVON_EBT_RETURN")
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
            //PRIMEPOS - 2532 added to get device message to get message back
            //_Device.OnMessageSent += OnMessageSent_Handler;
            long TransNo = 0;
            long OrgTransNo = 0;
            using (var db = new Possql())
            {
                CCTransmission_Log cclog = new CCTransmission_Log();
                cclog.TransDateTime = DateTime.Now;
                cclog.TransAmount = amount;
                cclog.TicketNo = Ticketno;
                //cclog.TransDataStr = _DeviceRequestMessage;
                cclog.PaymentProcessor = "ELAVON";
                cclog.StationID = StationID;
                cclog.UserID = UserID;
                cclog.TransmissionStatus = "InProgress";
                cclog.TransType = TransType;
                db.CCTransmission_Logs.Add(cclog);
                db.SaveChanges();
                db.Entry(cclog).GetDatabaseValues();
                TransNo = cclog.TransNo;
            }
            try
            {
                _Device = ElavonProcessor.getInstance(string.Empty, 0);
                switch (txnType)
                {

                    case "ELAVON_CREDIT_SALE":
                    case "ELAVON_DEBIT_SALE":
                        _DeviceResponse = _Device.Sale(MessageFields);

                        break;
                    case "ELAVON_CREDIT_VOID":
                    case "ELAVON_DEBIT_VOID":
                    case "ELAVON_EBT_VOID":
                        _DeviceResponse = _Device.Void(MessageFields);
                        break;
                    case "ELAVON_CREDIT_VOIDRETURN":
                    case "ELAVON_DEBIT_VOID_RETURN":
                        _DeviceResponse = _Device.VoidReturn(MessageFields);
                        break;
                    case "ELAVON_CREDIT_RETURN":
                    case "ELAVON_CREDIT_PREAUTH":
                    case "ELAVON_CREDIT_POSTAUTH":
                    case "ELAVON_DEBIT_RETURN":
                    case "ELAVON_DEBIT_REVERSE":
                    //case "ELAVON_DEBIT_Auth":
                    case "ELAVON_CREDIT_REVERSE":
                        _DeviceResponse = _Device.Return(MessageFields);
                        //if (!MessageFields.ContainsKey("TRANSACTIONID"))
                        //    _DeviceResponse = _Device.Refund(MessageFields);
                        //else
                        //    //_DeviceResponse = _Device.CreditCardReturn(MessageFields);
                        //    _DeviceResponse = _Device.StrictReturn(MessageFields);
                        // _DeviceResponse = _Device.DoDebit(TXN_TYPE_MAP[txnType], amountDesc, accountDesc, traceDesc
                        //, cashierDesc, extDataDesc);

                        break;
                    /* EBT Transaction*/
                    case "ELAVON_EBT_SALE":
                    case "ELAVON_EBT_AUTH":
                    case "ELAVON_EBT_RETURN":
                    case "ELAVON_EBT_REVERSE":
                        //_DeviceResponse = _Device.Sale(MessageFields);
                        _DeviceResponse = _Device.EBT(MessageFields);
                        //  _DeviceResponse = _Device.DoEBT(TXN_TYPE_MAP[txnType], amountDesc, accountDesc, traceDesc
                        //, cashierDesc);
                        //break;
                        //if (MessageFields.ContainsKey("TRANSACTIONID"))
                        //    _DeviceResponse = _Device.StrictReturn(MessageFields);
                        //else
                        //{
                        //    _DeviceResponse = _Device.Void(MessageFields);
                        //}
                        break;
                    default:
                        errorMessage = "INVALID_TXN_TYPE";
                        _DeviceResponse = null;
                        break;
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
                return _DeviceResponse;// NileshJ Temp - 20-Dec-2018
            }

            try
            {

                // PRIMEPOS - 2532 Modified cclog.TransDataStr,cclog.RecDataStr to add 
                //Request and Response of device to CCTransmission_Log table 
                #region  #region PRIMEPOS-2761 - commented 
                //using (var db = new Possql())
                //{
                //    CCTransmission_Log cclog = new CCTransmission_Log();
                //    cclog.TransDateTime = DateTime.Now;
                //    cclog.TransAmount = amount;
                //    //cclog.TransDataStr = _DeviceResponse.deviceRequest;
                //    //cclog.RecDataStr = _DeviceResponse.deviceResponse;
                //    db.CCTransmission_Logs.Add(cclog);
                //    db.SaveChanges();
                //}
                #endregion

                #region PRIMEPOS-2761

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


                    CCTransmission_Log cclog = new CCTransmission_Log();
                    cclog = db.CCTransmission_Logs.Where(w => w.TransNo == TransNo).SingleOrDefault();
                    cclog.AmtApproved = amount;
                    cclog.TransDataStr = _DeviceResponse.deviceRequest;//Request Message should be saved still pending 
                    cclog.RecDataStr = _DeviceResponse.deviceResponse;//Response Message saved
                    cclog.HostTransID = _DeviceResponse.TransactionNo;
                    cclog.TransmissionStatus = "Completed";
                    cclog.ResponseMessage = _DeviceResponse.ResultDescription;//2943
                    cclog.OrgTransNo = OrgTransNo;
                    #region PRIMEPOS-3182
                    if (!string.IsNullOrWhiteSpace(_DeviceResponse.AccountNo) && _DeviceResponse.AccountNo.Trim().Length >= 4)
                    {
                        cclog.last4 = _DeviceResponse.AccountNo.Trim().Substring(_DeviceResponse.AccountNo.Trim().Length - 4, 4);
                    }
                    #endregion
                    db.CCTransmission_Logs.Attach(cclog);
                    db.Entry(cclog).Property(p => p.TransDataStr).IsModified = true;
                    db.Entry(cclog).Property(p => p.RecDataStr).IsModified = true;
                    db.Entry(cclog).Property(p => p.TransmissionStatus).IsModified = true;
                    db.Entry(cclog).Property(p => p.HostTransID).IsModified = true;
                    db.Entry(cclog).Property(p => p.AmtApproved).IsModified = true;
                    db.Entry(cclog).Property(p => p.OrgTransNo).IsModified = true;
                    db.Entry(cclog).Property(p => p.last4).IsModified = true;   //PRIMEPOS-3182
                    db.SaveChanges();
                }
                #endregion
            }
            catch (Exception exp)
            {
                logger.Trace(exp.ToString());
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
            const String Elavon = "ELAVON";
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
                isValid = this.GetProcessorTag(kvp.Key, Elavon, out orgParam);
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

            xmlList = xmlElavon.GetElementsByTagName(xmlCommonNode);
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
