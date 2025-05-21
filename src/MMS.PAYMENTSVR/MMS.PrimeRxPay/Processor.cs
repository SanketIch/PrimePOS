//Author : ARVIND 
//Copy Right : © Micro Merchant Systems, Inc 2008
//Functionality Desciption : The purpose of this class is to make base processor for transactions.
//External functions:None   
//Known Bugs : None
using MMS.PROCESSOR;
using NLog;
using PossqlData;
using PrimeRxPay;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace MMS.PrimeRxPay
{
    //Author : ARVIND 
    //Functionality Desciption : The purpose of this class is to make base processor for transactions.
    //External functions:None   
    //Known Bugs : None
    public abstract class Processor : IDisposable
    {
        ILogger logger = LogManager.GetCurrentClassLogger();

        #region variables        
        private PrimeRxPayResponse _PrimeRxPayResponse;
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
        XmlDocument xmlPrimeRxPay = null;

        #region PRIMEPOS-2761
        private string UserId = string.Empty;
        private string StationId = string.Empty;
        private string TRANSACTIONTYPE = string.Empty;
        private long OrgTransNo = 0;
        private decimal ApprovedAmount = 0;
        #endregion
        private decimal _amount = 0.00m;
        private string _ticketNumber = string.Empty;
        private string _userId = string.Empty;
        private string _transactionType = string.Empty;
        private string _stationId = string.Empty;
        private string _cashbackAmount = string.Empty;
        private string _transactionNumber = string.Empty;
        private string _taxAmount = string.Empty;
        private string _orderNumber = string.Empty;

        //
        #endregion

        #region constants
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
                { "PRIMERXPAY_CREDIT_SALE", "01" },
                { "PRIMERXPAY_CREDIT_RETURN","02" },
                { "PRIMERXPAY_CREDIT_VOID", "16" },
                { "PRIMERXPAY_CREDIT_PREAUTH", "03" },
                { "PRIMERXPAY_CREDIT_POSTAUTH", "04" },
                { "PRIMERXPAY_CREDIT_REVERSE", "99" },
                //Debit txn Code
                { "PRIMERXPAY_DEBIT_SALE", "01" },
                { "PRIMERXPAY_DEBIT_RETURN", "02" },
                { "PRIMERXPAY_DEBIT_VOID", "16" },
                { "PRIMERXPAY_DEBIT_VOID_RETURN", "18" },
                { "PRIMERXPAY_DEBIT_AUTH", "03" },
                { "PRIMERXPAY_DEBIT_REVERSE", "99" },
                //EBT txn Code
                { "PRIMERXPAY_EBT_SALE", "01" },
                { "PRIMERXPAY_EBT_RETURN", "02" }
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
            _PrimeRxPayResponse = new PrimeRxPayResponse();
            xmlPrimeRxPay = new XmlDocument();
            xmlPrimeRxPay.Load(COMMONPROCESSORTAG);
        }

        /// <summary>
        /// Author : ARVIND 
        /// Functionality Desciption : This method is used clear the fields
        /// External functions:PaymentResponse
        /// Known Bugs : None
        /// </summary>
        private void ClearFields()
        {
            if (_PrimeRxPayResponse != null)
                _PrimeRxPayResponse.ClearFields();
            if (_PrimeRxPayResponse != null) //PRIMEPOS-2546 NJ Added for sign coming in for old transaction and test response handling 
                _PrimeRxPayResponse = null;

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
            if (_PrimeRxPayResponse == null)
                _PrimeRxPayResponse = new PrimeRxPayResponse();
            //Till - NileshJ-ARVIND
            //if (transactionType == "PrimeRxPay_CANCEL")
            //{

            //    //_PrimeRxPayResponse = _Device.Cancel();

            //    //if (_PrimeRxPayResponse != null)
            //    //{
            //    //    response.ParsePAXResponse(_PrimeRxPayResponse);
            //    //}
            //    //else
            //    //{
            //    //    errorMessage = ERROR_COMM_RESPONSE;
            //    //    logger.Error("***PAX , Error: " + ERROR_COMM_RESPONSE);
            //    //}
            //    return response;
            //}

            foreach (var fields in MessageFields)
            {
                logger.Debug("Key Names" + fields.Key);
            }

            if (IsValidRequest())
            {
                logger.Trace("Sending Data to PrimeRxPay");
                _PrimeRxPayResponse = SendReceiveData();
                if (_PrimeRxPayResponse != null)
                {
                    //response.ParsePAXResponse(_PrimeRxPayResponse);
                    logger.Trace("PrimeRxPay Device Response code - " + _PrimeRxPayResponse.Result + " Description - " + _PrimeRxPayResponse.ResultDescription);
                }
                else
                {
                    errorMessage = ERROR_COMM_RESPONSE;
                    logger.Error("***PrimeRxPay , Error: " + ERROR_COMM_RESPONSE);
                    _PrimeRxPayResponse = null; //NileshJ Temp - 12-Dec-2018
                }
                logger.Trace("END PROCESSOR(PrimeRxPay ProcessTxn)");
                return _PrimeRxPayResponse;
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
        private PrimeRxPayResponse SendReceiveData()
        {
            logger.Info("SendReceivedData() Start ");

            foreach (KeyValuePair<String, String> kvp in MessageFields)
            {
                if (kvp.Key.Trim().Equals("AMOUNT"))
                {
                    try
                    {
                        if (txnType == "PRIMERXPAY_DEBIT_RETURN" || txnType == "PRIMERXPAY_CREDIT_RETURN" || txnType == "PRIMERXPAY_EBT_RETURN")
                            _amount = Math.Abs(Convert.ToDecimal(kvp.Value.ToString()));
                        else
                            _amount = Convert.ToDecimal(kvp.Value.ToString());

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
                        _ticketNumber = kvp.Value.ToString();
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
                        _transactionType = kvp.Value.ToString();
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
                        _transactionNumber = kvp.Value.ToString();
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
                        _userId = kvp.Value.ToString();
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
                        _stationId = kvp.Value.ToString();
                    }
                    catch (Exception ex)
                    {
                        logger.Trace(ex.ToString());
                    }
                }
                #endregion                
            }
            #region PRIMEPOS-2761
            long TransNo = 0;
            long OrgTransNo = 0;
            using (var db = new Possql())
            {
                CCTransmission_Log cclog = new CCTransmission_Log();
                cclog.TransDateTime = DateTime.Now;
                cclog.TransAmount = _amount;
                cclog.TicketNo = _ticketNumber;
                //cclog.TransDataStr = _DeviceRequestMessage;
                cclog.PaymentProcessor = "PRIMERXPAY";
                cclog.StationID = _stationId;
                cclog.UserID = _userId;
                cclog.TransmissionStatus = "InProgress";
                #region PRIMEPOS-3248
                if (MessageFields.ContainsKey("ISPRIMERXPAYLINKSEND") && Convert.ToBoolean(MessageFields["ISPRIMERXPAYLINKSEND"]))
                {
                    cclog.TransType = "PRIMERXPAY_LINK_SENT";
                }
                else
                {
                    cclog.TransType = _transactionType;
                }
                #endregion
                db.CCTransmission_Logs.Add(cclog);
                db.SaveChanges();
                db.Entry(cclog).GetDatabaseValues();
                TransNo = cclog.TransNo;
            }
            #endregion
            try
            {
                PrimeRxPayProcessor _processor = PrimeRxPayProcessor.GetInstance();
                switch (txnType)
                {

                    case "PRIMERXPAY_CREDIT_SALE":
                        if (MessageFields.ContainsKey("TOKEN"))
                            _PrimeRxPayResponse = _processor.TokenSale(MessageFields);
                        else if (MessageFields.ContainsKey("ISCUSTOMERDRIVEN") && Convert.ToBoolean(MessageFields["ISCUSTOMERDRIVEN"]))
                            _PrimeRxPayResponse = _processor.CustomerSale(MessageFields);
                        else
                            _PrimeRxPayResponse = _processor.Sale(MessageFields);
                        break;
                    case "PRIMERXPAY_CREDIT_VOID":
                    case "PRIMERXPAY_DEBIT_VOID":
                    case "PRIMERXPAY_DEBIT_VOID_RETURN":
                        if (MessageFields.ContainsKey("ISVOIDPAYCOMP") || MessageFields.ContainsKey("ISVOIDCDRIVEN"))
                            _PrimeRxPayResponse = _processor.Void(MessageFields);
                        else
                            _PrimeRxPayResponse = _processor.Reversal(MessageFields);
                        break;
                    case "PRIMERXPAY_CREDIT_RETURN_VOID":
                        _PrimeRxPayResponse = _processor.Void(MessageFields);
                        break;
                    case "PRIMERXPAY_CREDIT_RETURN":
                    case "PRIMERXPAY_CREDIT_PREAUTH":
                    case "PRIMERXPAY_CREDIT_POSTAUTH":
                    case "PRIMERXPAY_CREDIT_REVERSE":
                        _PrimeRxPayResponse = _processor.Return(MessageFields);
                        break;
                    default:
                        errorMessage = "INVALID_TXN_TYPE";
                        _PrimeRxPayResponse = null;
                        break;
                }
            }
            catch (Exception ex)
            {
                logger.Trace(ex.ToString());
                return _PrimeRxPayResponse;// NileshJ Temp - 20-Dec-2018
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
                //    cclog.TransDataStr = _PrimeRxPayResponse.deviceRequest;
                //    cclog.RecDataStr = _PrimeRxPayResponse.deviceResponse;
                //    db.CCTransmission_Logs.Add(cclog);
                //    db.SaveChanges();
                //}
                #endregion

                #region PRIMEPOS-2761

                using (var db = new Possql())
                {
                    CCTransmission_Log cclog1 = new CCTransmission_Log();
                    if (_transactionType.Contains("VOID") || _transactionType.Contains("VOID"))
                    {
                        cclog1 = db.CCTransmission_Logs.Where(w => w.HostTransID == _transactionNumber).SingleOrDefault();
                        cclog1.IsReversed = true;
                        OrgTransNo = cclog1.TransNo;
                        db.CCTransmission_Logs.Attach(cclog1);
                        db.Entry(cclog1).Property(p => p.IsReversed).IsModified = true;
                        db.SaveChanges();
                    }


                    CCTransmission_Log cclog = new CCTransmission_Log();
                    cclog = db.CCTransmission_Logs.Where(w => w.TransNo == TransNo).SingleOrDefault();
                    cclog.AmtApproved = _amount;
                    cclog.TransDataStr = _PrimeRxPayResponse.request;//Request Message should be saved still pending 
                    cclog.RecDataStr = _PrimeRxPayResponse.response;//Response Message saved
                    cclog.HostTransID = _PrimeRxPayResponse.TransactionNo;
                    #region PRIMEPOS-3248
                    if (MessageFields.ContainsKey("ISPRIMERXPAYLINKSEND") && Convert.ToBoolean(MessageFields["ISPRIMERXPAYLINKSEND"]))
                    {
                        cclog.TransmissionStatus = "Completed";
                    }
                    else
                    {
                        if (MessageFields.ContainsKey("ISCUSTOMERDRIVEN") && Convert.ToBoolean(MessageFields["ISCUSTOMERDRIVEN"]))
                            cclog.TransmissionStatus = "In Progress";
                        else
                            cclog.TransmissionStatus = "Completed";
                    }
                    #endregion
                    cclog.ResponseMessage = _PrimeRxPayResponse.Result;
                    #region PRIMEPOS-3383
                    if (!string.IsNullOrWhiteSpace(_PrimeRxPayResponse.AccountNo) && _PrimeRxPayResponse.AccountNo.Trim().Length >= 4)
                    {
                        cclog.last4 = _PrimeRxPayResponse.AccountNo.Trim().Substring(_PrimeRxPayResponse.AccountNo.Trim().Length - 4, 4);
                    }
                    #endregion
                    cclog.OrgTransNo = OrgTransNo;
                    db.CCTransmission_Logs.Attach(cclog);
                    db.Entry(cclog).Property(p => p.TransDataStr).IsModified = true;
                    db.Entry(cclog).Property(p => p.RecDataStr).IsModified = true;
                    db.Entry(cclog).Property(p => p.TransmissionStatus).IsModified = true;
                    db.Entry(cclog).Property(p => p.HostTransID).IsModified = true;
                    db.Entry(cclog).Property(p => p.AmtApproved).IsModified = true;
                    db.Entry(cclog).Property(p => p.OrgTransNo).IsModified = true;
                    db.Entry(cclog).Property(p => p.ResponseMessage).IsModified = true;
                    db.Entry(cclog).Property(p => p.last4).IsModified = true; //PRIMEPOS-3383
                    db.SaveChanges();
                }
                #endregion
            }
            catch (Exception exp)
            {
                logger.Trace(exp.ToString());
                return _PrimeRxPayResponse;// NileshJ Temp - 20-Dec-2018
            }

            //    // PRIMEPOS - 2532 Modified cclog.TransDataStr,cclog.RecDataStr to add 
            //    //Request and Response of device to CCTransmission_Log table 
            //    //using (var db = new Possql())
            //    //{
            //    //    CCTransmission_Log cclog = new CCTransmission_Log();
            //    //    cclog.TransDateTime = DateTime.Now;
            //    //    cclog.TransAmount = _amount;
            //    //    cclog.TransDataStr = _PrimeRxPayResponse.request;
            //    //    cclog.RecDataStr = _PrimeRxPayResponse.response;
            //    //    //cclog.
            //    //    db.CCTransmission_Logs.Add(cclog);
            //    //    db.SaveChanges();
            //    //}
            //}
            //catch (Exception exp)
            //{
            //    logger.Trace(exp.ToString());
            //    return _PrimeRxPayResponse;// NileshJ Temp - 20-Dec-2018
            //}            
            return _PrimeRxPayResponse;

        }

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
            const String PrimeRxPay = "PRIMERXPAY";
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
                isValid = this.GetProcessorTag(kvp.Key, PrimeRxPay, out orgParam);
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

            xmlList = xmlPrimeRxPay.GetElementsByTagName(xmlCommonNode);
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
                if (_PrimeRxPayResponse != null)
                {
                    _PrimeRxPayResponse.Dispose();
                    _PrimeRxPayResponse = null;
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
