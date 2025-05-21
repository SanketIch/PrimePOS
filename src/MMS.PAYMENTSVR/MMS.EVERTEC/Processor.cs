
using MMS.PROCESSOR;
using NLog;
using PossqlData;
//using PossqlData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Evertech;
using Evertech.Data;

namespace MMS.EVERTEC
{
    //Arvind
    public abstract class Processor : IDisposable
    {
        ILogger logger = LogManager.GetCurrentClassLogger();

        #region variables


        private PmtTxnResponse _DeviceResponse;

        //Dictionary to validate req
        private MMSDictionary<String, MMSDictionary<String, String>> MandatoryKeys = null;
        private MMSDictionary<String, String> ValidKeys = null;
        private MMSDictionary<String, String> MessageFields = null;

        //Xml Helper
        private XmlToKeys XmlToKeys = null;
        private KeysToXml Fields = null;

        private String errorMessage = String.Empty;
        private String txnType = String.Empty;
        private String ResponseMessage = String.Empty;
        private MerchantInfo Merchant = null;
        private Boolean Disposed = false;
        XmlDocument xmlEVERTEC = null;

        private decimal amount = 0.00m;
        #region PRIMEPOS-2761
        private string Ticketno = "";
        private string TransType = "";
        private string UserID = "";
        private string StationID = "";
        private string TransID = "";
        #endregion
        #endregion

        #region constants
        private readonly string PAYMENTPROCESSOR = "EVERTEC|";

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
                { "EVERTEC_CREDIT_SALE", "01" },
                { "EVERTEC_CREDIT_RETURN","02" },
                { "EVERTEC_CREDIT_VOID", "16" },
                { "EVERTEC_CREDIT_PREAUTH", "03" },
                { "EVERTEC_CREDIT_POSTAUTH", "04" },
                { "EVERTEC_CREDIT_REVERSE", "99" },
                //Debit txn Code
                { "EVERTEC_DEBIT_SALE", "01" },
                { "EVERTEC_DEBIT_RETURN", "02" },
                { "EVERTEC_DEBIT_VOID", "16" },
                { "EVERTEC_DEBIT_VOID_RETURN", "18" },
                { "EVERTEC_DEBIT_AUTH", "03" },
                { "EVERTEC_DEBIT_REVERSE", "99" },
                //EBT txn Code
                { "EVERTEC_EBT_SALE", "01" },
                { "EVERTEC_EBT_RETURN", "02" }
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
        /// Author : Arvind 
        /// Functionality Desciption : This method is constructor for the CreditProcessor class
        /// External functions:None
        /// Known Bugs : None
        /// Start Date : 5 April 2019
        /// </summary>
        /// <param name="ProcessorKey"></param>
        /// <param name="merchant">MerchantInfo</param>
        public Processor(string ProcessorKey, MerchantInfo merchant)
        {
            Merchant = merchant;
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
            _DeviceResponse = new PmtTxnResponse();
            xmlEVERTEC = new XmlDocument();
            xmlEVERTEC.Load(COMMONPROCESSORTAG);
        }


        /// <summary>
        /// Author : Arvind 
        /// Functionality Desciption : This method is used clear the fields
        /// External functions:PaymentResponse
        /// Known Bugs : None
        /// Start Date : 7 April 2019
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
        /// Author : Arvind 
        /// Functionality Desciption : This method is used to Pad spaces for the processor
        /// External functions:MMSDictionary,KesyToXml
        /// Known Bugs : None
        /// Start Date : 7 April 2019
        /// </summary>
        /// <param name="value"></param>
        /// <param name="count"></param>
        /// <returns>String</returns>
        private String PadSpaces(String value, int count)
        {
            return value.ToString().PadRight(count, ' ');
        }

        //Use this for processing the transaction of any type.
        /// <summary>
        /// Author : Arvind 
        /// Functionality Desciption : This method is the core method that does all the work
        /// External functions:MMSDictioanary,PaymentResponse,SocketClient
        /// Known Bugs : None
        /// Start Date : 5 April 2019
        /// </summary>
        /// <param name="transactionType"></param>
        /// <param name="requestMsgKeys"></param>
        /// <returns>PaymentResponse</returns> 

        protected PaymentResponse ProcessTxn(string transactionType, ref MMSDictionary<String, String> requestMsgKeys)
        {
            logger.Trace("Start Processor (EVERTEC ProcessTxn) ----");
            //Clear all the fields.                 
            ClearFields();
            MessageFields = requestMsgKeys;
            txnType = transactionType;

            if (_DeviceResponse == null)
            {
                _DeviceResponse = new PmtTxnResponse();
            }

            if (IsValidRequest())
            {
                logger.Trace("Sending Data to EVERTEC");
                _DeviceResponse = SendReceiveData();
                if (_DeviceResponse != null)
                {
                    logger.Trace("EVERTEC Device Response code - " + _DeviceResponse.Result + " Description - " + _DeviceResponse.ResultDescription);
                } else
                {
                    errorMessage = ERROR_COMM_RESPONSE;
                    logger.Error("***EVERTEC , Error: " + ERROR_COMM_RESPONSE);
                    _DeviceResponse = null; //NileshJ Temp - 12-Dec-2018
                }
                logger.Trace("END PROCESSOR(EVERTEC ProcessTxn)");
                return _DeviceResponse;
            }
            return null;
        }


        //Move to EVERTECResponse parse method



        /// <summary>
        /// Author : Arvind 
        /// Functionality Desciption : This method is called to Open,Send,Receive and disconnect the Payment Server.
        /// External functions:None
        /// Known Bugs : None
        /// Start Date : 5 April 2019
        /// </summary>
        /// <param name="strRequest">String</param>
        /// <returns>bool</returns> 
        private PmtTxnResponse SendReceiveData()
        {
            foreach (KeyValuePair<String, String> kvp in MessageFields)
            {

                if (kvp.Key.Trim().Equals("AMOUNT"))
                {
                    try
                    {
                        if (txnType == "EVERTEC_DEBIT_RETURN" || txnType == "EVERTEC_CREDIT_RETURN" || txnType == "EVERTEC_EBT_RETURN")
                            amount = Math.Abs(Convert.ToDecimal(kvp.Value.ToString()));
                        else
                            amount = Convert.ToDecimal(kvp.Value.ToString());

                    } catch (Exception ex)
                    {
                        logger.Trace(ex.ToString());
                    }

                }
                #region PRIMEPOS-2761
                if (kvp.Key.Trim().Equals("TICKET_NUMBER"))
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
            logger.Info("SendReceivedData() Start ");
            EvertechProcessor device = EvertechProcessor.getInstance("192.168.1.151", 2030);
            String reqMessage = PAYMENTPROCESSOR;
            #region PRIMEPOS-2761
            long TransNo = 0;
            long OrgTransNo = 0;
            using (var db = new Possql())
            {
                CCTransmission_Log cclog = new CCTransmission_Log();
                cclog.TransDateTime = DateTime.Now;
                cclog.TransAmount = amount;
                cclog.TicketNo = Ticketno;
                //cclog.TransDataStr = otagbuilder.ToString();
                cclog.PaymentProcessor = "EVERTEC";
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

                    case "EVERTEC_CREDIT_SALE":
                    case "EVERTEC_CREDIT_PREAUTH":
                    case "EVERTEC_CREDIT_POSTAUTH":
                        _DeviceResponse = device.Sale((Dictionary<String, String>)MessageFields);
                        break;
                    case "EVERTEC_CREDIT_RETURN":
                    case "EVERTEC_CREDIT_REVERSE":
                        _DeviceResponse = device.Refund((Dictionary<String, String>)MessageFields);
                        break;
                    case "EVERTEC_CREDIT_VOID":
                        _DeviceResponse = device.Void((Dictionary<String, String>)MessageFields);
                        #region PRIMEPOS-2761
                        using (var db = new Possql())
                        {
                            CCTransmission_Log cclog = new CCTransmission_Log();
                            cclog = db.CCTransmission_Logs.Where(w => w.HostTransID == TransID).SingleOrDefault();
                            cclog.IsReversed = true;
                            OrgTransNo = cclog.TransNo;
                            db.CCTransmission_Logs.Attach(cclog);
                            db.Entry(cclog).Property(p => p.IsReversed).IsModified = true;
                            db.SaveChanges();
                        }
                        #endregion
                        break;

                    case "EVERTEC_DEBIT_SALE":
                    case "EVERTEC_DEBIT_Auth":
                        _DeviceResponse = device.Sale((Dictionary<String, String>)MessageFields);
                        break;
                    case "EVERTEC_DEBIT_RETURN":
                    case "EVERTEC_DEBIT_REVERSE":
                        _DeviceResponse = device.Refund((Dictionary<String, String>)MessageFields);
                        break;
                    case "EVERTEC_DEBIT_VOID":
                    case "EVERTEC_DEBIT_VOID_RETURN":
                        _DeviceResponse = device.Void((Dictionary<String, String>)MessageFields);
                        break;
                    /* EBT Transaction*/
                    case "EVERTEC_EBT_SALE":
                    case "EVERTEC_EBT_RETURN":
                    case "EVERTEC_EBT_VOID":
                    case "EVERTEC_EBT_Auth":
                    case "EVERTEC_EBT_REVERSE":
                        _DeviceResponse = device.EBT((Dictionary<String, String>)MessageFields);
                        break;
                    default:
                        errorMessage = "INVALID_TXN_TYPE";
                        _DeviceResponse = null;
                        break;
                }
            } catch (Exception ex)
            {
                logger.Trace(ex.ToString());
                return _DeviceResponse;
            }

            try
            {
                #region Commented PRIMEPOS-2761
                //Request and Response of device to CCTransmission_Log table 
                //using (var db = new Possql())
                //{
                //    CCTransmission_Log cclog = new CCTransmission_Log();
                //    cclog.TransDateTime = DateTime.Now;
                //    cclog.TransAmount = amount;
                //    cclog.TransDataStr = _DeviceResponse.request;//Request Message should be saved still pending 
                //    cclog.RecDataStr = _DeviceResponse.response;//Response Message saved
                //    db.CCTransmission_Logs.Add(cclog);
                //    db.SaveChanges();
                //}
                #endregion
                #region PRIMEPOS-2761
                using (var db = new Possql())
                {
                    CCTransmission_Log cclog = new CCTransmission_Log();
                    cclog = db.CCTransmission_Logs.Where(w => w.TransNo == TransNo).SingleOrDefault();
                    cclog.AmtApproved = amount;
                    cclog.TransDataStr = _DeviceResponse.request;//Request Message should be saved still pending 
                    cclog.RecDataStr = _DeviceResponse.response;//Response Message saved
                    cclog.HostTransID = _DeviceResponse.TransactionNo;
                    cclog.TransmissionStatus = "Completed";
                    cclog.OrgTransNo = OrgTransNo;
                    cclog.ResponseMessage = _DeviceResponse.ResultDescription;
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
                    db.Entry(cclog).Property(p => p.ResponseMessage).IsModified = true;
                    db.Entry(cclog).Property(p => p.last4).IsModified = true;   //PRIMEPOS-3182
                    db.SaveChanges();
                }
                #endregion
            }
            catch (Exception exp)
            {
                logger.Trace(exp.ToString());
                return _DeviceResponse;
            }

            //End Added
            return _DeviceResponse;

        }

        private void OnMessageSent_Handler(string message)
        {
            _DeviceResponse.request = message;
            Console.WriteLine("EVERTEC message Processor " + _DeviceResponse.request);
        }



        /// <summary>
        /// Author : Arvind 
        /// Functionality Desciption : This method is called to validate the Valid Fields and Mandatory fields.
        /// External functions:None
        /// Known Bugs : None
        /// Start Date : 5 April 2019
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
        /// Author : Arvind 
        /// Functionality Desciption : This method is used for checking the Valid Fields for a Processor.
        /// External functions:None
        /// Known Bugs : None
        /// Start Date : 5 April 2019
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
        /// Author : Arvind 
        /// Functionality Desciption : This method is used for checking the Mandatory Fields for a Transaction type.
        /// External functions:None
        /// Known Bugs : None
        /// Start Date : 5 April 2019
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
        /// Author : Arvind 
        /// Functionality Desciption : This method is used for fetching the mandatory fields of transaction from MMSDictionary or MandaotoryFields.xml file.
        /// External functions:MMSDictionary,XmlToKeys
        /// Known Bugs : None
        /// Start Date : 5 April 2019
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
        /// Author : Arvind 
        /// Functionality Desciption : This method is used for ensuring valid parameters are passed (Null check).
        /// External functions:MMSDictionary,XmlToKeys
        /// Known Bugs : None
        /// Start Date : 5 April 2019
        /// </summary>
        /// <param name="keys">MMSDictioanary</param>
        /// <returns>bool vaild</returns>
        public bool ValidateParameters(ref MMSDictionary<String, String> keys)
        {
            logger.Trace("In ValidateParameters()");
            bool flag = true;
            bool isValid = true;
            string sError = string.Empty;
            const String Evertec = "EVERTEC";
            MMSDictionary<String, String> orignalKeys = new MMSDictionary<string, string>();
            MMSDictionary<String, String> revisedKeys = new MMSDictionary<string, string>();
            string orgParam = string.Empty;
            if (keys == null)
                return false;
            foreach (KeyValuePair<String, String> kvp in keys)
            {
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
                isValid = this.GetProcessorTag(kvp.Key, Evertec, out orgParam);
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

            xmlList = xmlEVERTEC.GetElementsByTagName(xmlCommonNode);
            XmlNodeList ProcessorNode = xmlList.Item(0).ChildNodes;

            for (int iCount = 0; iCount < ProcessorNode.Count; iCount++)
            {
                if (ProcessorNode.Item(iCount).Name == xmlProcessorName)
                    tagValue = ProcessorNode.Item(iCount).InnerText;
            }

            if (tagValue != null && tagValue != string.Empty)
            {
                isValid = true;
            } else
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
