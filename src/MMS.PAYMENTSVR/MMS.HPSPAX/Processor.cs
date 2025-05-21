//Author : Ritesh 
//Copy Right : © Micro Merchant Systems, Inc 2008
//Functionality Desciption : The purpose of this class is to make base processor for transactions.
//External functions:None   
//Known Bugs : None
//Start Date : 2 Feb 2008.
using MMS.GlobalPayments.Api.Services;//PRIMEPOS-2931
using MMS.GlobalPayments.Api.Terminals; //PRIMEPOS-2931
using MMS.GlobalPayments.Api.Terminals.PAX;//PRIMEPOS-2931
using MMS.PROCESSOR;
using NLog;
using PossqlData;
using System;
using System.Collections.Generic;
//using SecureSubmit.Terminals.PAX;//Commented by Amit on 02 Dec 2020//PRIMEPOS-2931
//using SecureSubmit.Terminals;//Commented by Amit on 02 Dec 2020 //PRIMEPOS-2931
using System.Linq;//PRIMSPO-2761
using System.Xml;

namespace MMS.HPSPAX
{
    //Author : Ritesh 
    //CopyRight: MMS 2008
    //Functionality Desciption : The purpose of this class is to make base processor for transactions.
    //External functions:None   
    //Known Bugs : None
    //Start Date : 2 Feb 2008.
    public abstract class Processor : IDisposable
    {
        ILogger logger = LogManager.GetCurrentClassLogger();

        #region variables
        public bool isPaxDeviceConnected = false; //PRIMEPOS-3090
        //private PaxDevice _Device;//Commented by Amit on 02 Dec 2020//PRIMEPOS-2931
        //private PaxDeviceResponse _DeviceResponse;//Commented by Amit on 02 Dec 2020//PRIMEPOS-2931
        private IDeviceInterface _Device;//Commented by Amit on 02 Dec 2020//PRIMEPOS-2931
        private PaxTerminalResponse _DeviceResponse;//Commented by Amit on 02 Dec 2020//PRIMEPOS-2931
        private string _DeviceRequestMessage;

        //Dictionary to validate req
        private MMSDictionary<String, MMSDictionary<String, String>> MandatoryKeys = null;
        private MMSDictionary<String, String> ValidKeys = null;
        private MMSDictionary<String, String> MessageFields = null;

        //Xml Helper
        private XmlToKeys XmlToKeys = null;
        private KeysToXml Fields = null;

        //
        private PaymentResponse response = null;
        private String errorMessage = String.Empty;
        private String txnType = String.Empty;
        private String ResponseMessage = String.Empty;
        private MerchantInfo Merchant = null;
        private Boolean Disposed = false;
        XmlDocument xmlPAX = null;

        private decimal amount = 0.00m;
        #region PRIMEPOS-2761
        private string UserId = string.Empty;
        private string StationId = string.Empty;
        private string TRANSACTIONTYPE = string.Empty;
        private long OrgTransNo = 0;
        private decimal ApprovedAmount = 0;
        #endregion


        #region SKipSignature Nilesh,Sajid PRIMEPOS-2852
        private bool isSkipEMV = false;
        private bool isSkipF10 = false;
        private bool isSkipLessThan20 = false;
        private string RecoveryFlag = string.Empty;
        private string _ErrorMessage = string.Empty;
        #endregion
        //Transaction Parameter Constructs SecureSubmit 
        private AmountRequest amountDesc = null;
        private AccountRequest accountDesc = null;
        private TraceRequest traceDesc = null;
        private AvsRequest avsDesc = null;
        private CashierSubGroup cashierDesc = null;
        private CommercialRequest commercialDesc = null;
        private EcomSubGroup eComDesc = null;
        private ExtDataSubGroup extDataDesc = null;
        //
        #endregion

        #region constants
        private readonly string PAYMENTPROCESSOR = "PAX|";

        private const String VALID_FIELDS = "ValidFields.xml";
        private const String MANDATORY_FIELDS = "MandatoryFields.xml";
        public const String FAILED_OPRN = "FAILED";
        public const String INVALID_PARAMETERS = "INVALID_PARAMETERS";
        public const String SIGNATURE_PAD_NOT_CONNECTED = "Signature pad is not connected.";//PRIMEPOS-3090

        private const string COMMONPROCESSORTAG = "CommonProcessorTag.xml";
        private const string ERROR_COMM_RESPONSE = "Communication Error";
        private const string DEVICE_RESPONSE_CODE_TIMEOUT = "100001"; //PRIMEPOS-3087
        private const string DEVICE_RESPONSE_TEXT_TIMEOUT = "TIMEOUT"; //PRIMEPOS-3087
        private const string DEVICE_RESPONSE_CODE_NOT_FOUND = "100023"; //PRIMEPOS-3262
        #region PRIMEPOS-2761
        private const string HPSPAX = "HPSPAX";
        #endregion
        private static MMSDictionary<String, String> TXN_TYPE_MAP = null;
        private static Dictionary<string, string> REQUEST_SIGNATURE_PARAM = new Dictionary<string, string>();

        static Processor()
        {

            TXN_TYPE_MAP = new MMSDictionary<string, string>() {
                //Credit txn Code
                { "HPSPAX_CREDIT_SALE", "01" },
                { "HPSPAX_CREDIT_RETURN","02" },
                { "HPSPAX_CREDIT_VOID", "16" },
                { "HPSPAX_CREDIT_PREAUTH", "03" },
                { "HPSPAX_CREDIT_POSTAUTH", "04" },
                { "HPSPAX_CREDIT_REVERSE", "99" },
                //Debit txn Code
                { "HPSPAX_DEBIT_SALE", "01" },
                { "HPSPAX_DEBIT_RETURN", "02" },
                { "HPSPAX_DEBIT_VOID", "16" },
                { "HPSPAX_DEBIT_VOID_RETURN", "18" },
                { "HPSPAX_DEBIT_AUTH", "03" },
                { "HPSPAX_DEBIT_REVERSE", "99" },
                //EBT txn Code
                { "HPSPAX_EBT_SALE", "01" },
                { "HPSPAX_EBT_RETURN", "02" }
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
        /// Author : Ritesh 
        /// Functionality Desciption : This method is constructor for the CreditProcessor class
        /// External functions:None
        /// Known Bugs : None
        /// Start Date : 14 Jan 2008.
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
            response = new PAXPaymentResponse();
            xmlPAX = new XmlDocument();
            xmlPAX.Load(COMMONPROCESSORTAG);
        }

        public void InitDevice(string _posSettings)
        {

            string hostAddress = _posSettings.Split(':')[0];
            string hostPort = _posSettings.Split(':')[1].Split('/')[0];
            #region  NileshJ Disconnection Timeout Issue - 3-July-2019 -  PRIMEPOS-2706
            string timeOutString = string.Empty;
            int timeOut = 0;
            #region PRIMEPOS-2877
            ConnectionModes ConMode = new ConnectionModes();
            try
            {
                if (_posSettings.Contains("~"))
                {
                    ConMode = (ConnectionModes)Enum.Parse(typeof(ConnectionModes), _posSettings.Split('~')[1], true);
                    try
                    {
                        timeOutString = _posSettings.Split('|')[1].Split('~')[0];
                    }
                    catch (Exception ex)
                    {
                        //timeOut = PaxDevice.DEFAULT_TIMEOUT;//Commented by Amit on 02 Dec 2020
                        timeOut = PaxInterface.DEFAULT_TIMEOUT;//PRIMEPOS-2931
                    }
                }
                else
                {
                    ConMode = ConnectionModes.TCP_IP;
                    try
                    {
                        timeOutString = _posSettings.Split('|')[1];
                    }
                    catch (Exception ex)
                    {
                        //timeOut = PaxDevice.DEFAULT_TIMEOUT;//Commented by Amit on 02 Dec 2020
                        timeOut = PaxInterface.DEFAULT_TIMEOUT;//PRIMEPOS-2931
                    }
                }
            }
            catch (Exception ex)
            {

                ConMode = ConnectionModes.TCP_IP;
                //timeOut = PaxDevice.DEFAULT_TIMEOUT;//Commented by Amit on 02 Dec 2020
                timeOut = PaxInterface.DEFAULT_TIMEOUT;//PRIMEPOS-2931
            }
            #endregion

            //try
            //{
            //    timeOutString = _posSettings.Split('|')[1];
            //}
            //catch (Exception ex)
            //{
            //    timeOut = PaxDevice.DEFAULT_TIMEOUT;
            //}


            if (timeOutString != null && timeOutString != string.Empty)
                timeOut = Convert.ToInt32(timeOutString);
            #endregion
            //this._Device = new PaxDevice(new ConnectionConfig()
            //{
            //    BaudRate = BaudRate.r19200,
            //    ConnectionMode = ConMode,// ConnectionModes.TCP_IP,
            //    //ConnectionMode = ConnectionModes.HTTP, // NileshJ added for HTTP - Temp
            //    IpAddress = hostAddress,
            //    Port = hostPort,
            //    TimeOut = timeOut //  PRIMEPOS-2706
            //});//Commented by Amit on 02 Dec 2020

            this._Device = DeviceService.Create(new ConnectionConfig
            {
                //DeviceType = DeviceType.PAX_PX7,
                ConnectionMode = ConMode,// ConnectionModes.HTTP,// ConnectionModes.TCP_IP,Amit    
                IpAddress = hostAddress,
                Port = hostPort,
                BaudRate = BaudRate.r19200,
                Timeout = timeOut,

            });//PRIMEPOS-2931
               //if (shouldInitDevice) {
               //    _Device.Initialize();
               //}

            #region PRIMEPOS-3090
            try
            {
                _DeviceResponse = _Device.Initialize();
                isPaxDeviceConnected = (_DeviceResponse != null && _DeviceResponse.DeviceResponseCode == "000000");
            }
            catch (Exception)
            {
                isPaxDeviceConnected = false;
            }
            #endregion
        }

        /// <summary>
        /// Author : Ritesh 
        /// Functionality Desciption : This method is used clear the fields
        /// External functions:PaymentResponse
        /// Known Bugs : None
        /// Start Date : 29 Jan 2008.
        /// </summary>
        private void ClearFields()
        {
            if (response != null)
                response.ClearFields();
            if (_DeviceResponse != null) //PRIMEPOS-2546 NJ Added for sign coming in for old transaction and test response handling 
                _DeviceResponse = null;

            errorMessage = String.Empty;
            txnType = String.Empty;
        }
        /// <summary>
        /// Author : Ritesh 
        /// Functionality Desciption : This method is used to Pad spaces for the processor
        /// External functions:MMSDictionary,KesyToXml
        /// Known Bugs : None
        /// Start Date : 29 Jan 2008.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="count"></param>
        /// <returns>String</returns>
        private String PadSpaces(String value, int count)
        {
            return value.ToString().PadRight(count, ' ');
        }

        /// <summary>
        /// Author : Ritesh 
        /// Functionality Desciption : This method is used to Build request sent to PaymentServer.
        /// External functions:KesyToXml
        /// Known Bugs : None
        /// Start Date : 2 Feb 2008.
        /// </summary>
        /// <returns>String</returns>
        private string BuildRequest()
        {

            String reqMessage = "";

            amountDesc = new AmountRequest();
            accountDesc = new AccountRequest();
            traceDesc = new TraceRequest();
            avsDesc = new AvsRequest();
            cashierDesc = new CashierSubGroup();
            commercialDesc = new CommercialRequest();
            eComDesc = new EcomSubGroup();
            extDataDesc = new ExtDataSubGroup();

            foreach (KeyValuePair<String, String> kvp in MessageFields)
            {

                if (kvp.Key.Trim().Equals("AMOUNT"))
                {
                    try
                    {
                        //PRIMEPOS-2579 : Added for Return Transation -ve amount Issue - NileshJ 16AUg2018
                        if (txnType == "HPSPAX_DEBIT_RETURN" || txnType == "HPSPAX_CREDIT_RETURN" || txnType == "HPSPAX_EBT_RETURN")
                            amount = Math.Abs(Convert.ToDecimal(kvp.Value.ToString()));
                        else
                            amount = Convert.ToDecimal(kvp.Value.ToString());

                        amountDesc.TransactionAmount = ToDeviceAmount(amount.ToString());
                    }
                    catch (Exception ex)
                    {
                        logger.Trace(ex.ToString());
                    }

                }
                if (kvp.Key.Trim().Equals("CASHBACK_AMOUNT"))
                {
                    try
                    {
                        amountDesc.CashBackAmount = Convert.ToString(kvp.Value);
                    }
                    catch (Exception ex)
                    {
                        logger.Trace(ex.ToString());
                    }

                }
                if (kvp.Key.Trim().Equals("TAXAMT"))
                {
                    try
                    {
                        amountDesc.TaxAmount = kvp.Value;
                    }
                    catch (Exception ex)
                    {
                        logger.Trace(ex.ToString());
                    }

                }

                if (kvp.Key.Trim().Equals("TICKETNO"))
                {
                    try
                    {
                        traceDesc.ReferenceNumber = Convert.ToString(kvp.Value);
                    }
                    catch (Exception ex)
                    {
                        logger.Trace(ex.ToString());
                    }

                }

                if (kvp.Key.Trim().Equals("TRANSID"))
                {
                    try
                    {
                        traceDesc.TransactionNumber = Convert.ToString(kvp.Value);
                    }
                    catch (Exception ex)
                    {
                        logger.Trace(ex.ToString());
                    }

                }
                // PRIMEPOS - 2530 - Added For Tokenization - Start
                if (kvp.Key.Trim().Equals("TOKENREQUEST"))
                {
                    try
                    {
                        if (!extDataDesc.Fields.ContainsKey(kvp.Key))
                        {
                            bool tokenize = false;
                            bool.TryParse(kvp.Value, out tokenize);
                            extDataDesc.Fields.Add(kvp.Key, tokenize == true ? "1" : "0");
                        }
                    }
                    catch (Exception ex)
                    {
                        logger.Trace(ex.ToString());
                    }
                }
                // PRIMEPOS - 2530 - Added For Tokenization - End
                #region PRIMEPOS-2738 ADDED BY ARVIND FOR REVERSAL
                if (kvp.Key.Trim().Equals("HREF"))
                {
                    try
                    {
                        extDataDesc.Fields.Add(kvp.Key, kvp.Value);
                    }
                    catch (Exception ex)
                    {
                        logger.Trace(ex.ToString());
                    }

                }
                if (kvp.Key.Trim().Equals("OGTRANSID"))
                {
                    try
                    {
                        eComDesc.OrderNumber = Convert.ToString(kvp.Value);
                    }
                    catch (Exception ex)
                    {
                        logger.Trace(ex.ToString());
                    }

                }
                #endregion

                #region PRIMEPOS-2761 - NileshJ
                if (kvp.Key.Trim().Equals("USERID"))
                {
                    try
                    {
                        UserId = kvp.Value.ToString();
                    }
                    catch (Exception ex)
                    {
                        logger.Error(ex.ToString());
                    }
                }
                if (kvp.Key.Trim().Equals("StationID"))
                {
                    try
                    {
                        StationId = kvp.Value.ToString();
                    }
                    catch (Exception ex)
                    {
                        logger.Error(ex.ToString());
                    }
                }
                if (kvp.Key.Trim().Equals("TRANSACTIONTYPE"))
                {
                    try
                    {
                        TRANSACTIONTYPE = kvp.Value.ToString();
                        // accountDesc.DupOverrideFlag = "1"; // temp
                    }
                    catch (Exception ex)
                    {
                        logger.Error(ex.ToString());
                    }
                }
                //PRIMEPOS-3047
                if (kvp.Key.Trim().Equals("ISALLOWDUP"))
                {
                    try
                    {
                        logger.Trace("ISALLOWDUP" + kvp.Value.ToString());
                        if (kvp.Value.ToString() == "true")
                            accountDesc.DupOverrideFlag = "1";
                    }
                    catch (Exception ex)
                    {
                        logger.Error(ex.ToString());
                    }
                }
                #endregion

                #region SkipSignature PRIMEPOS-2852

                if (kvp.Key.Trim().Equals("SkipEMVCardSign"))
                {
                    try
                    {
                        bool.TryParse(kvp.Value, out isSkipEMV);
                    }
                    catch (Exception ex)
                    {
                        logger.Trace(ex.ToString());
                    }
                }
                if (kvp.Key.Trim().Equals("SkipF10Sign"))
                {
                    try
                    {
                        bool.TryParse(kvp.Value, out isSkipF10);
                    }
                    catch (Exception ex)
                    {
                        logger.Trace(ex.ToString());
                    }
                }
                if (kvp.Key.Trim().Equals("SkipAmountSign"))
                {
                    try
                    {
                        bool.TryParse(kvp.Value, out isSkipLessThan20);
                    }
                    catch (Exception ex)
                    {
                        logger.Trace(ex.ToString());
                    }
                }
                #endregion

            }
            if (txnType != "HPSPAX_CREDIT_VOID" && txnType != "HPSPAX_DEBIT_SALE" && txnType != "HPSPAX_DEBIT_RETURN" && txnType != "HPSPAX_DEBIT_VOID")  // Add Other TXN not 
                foreach (KeyValuePair<String, String> kvp in REQUEST_SIGNATURE_PARAM)
                {
                    if (!extDataDesc.Fields.ContainsKey(kvp.Key))
                    {
                        #region SKipSign EMV F10 <20 PRIMEPOS-2852
                        //if (isSkipEMV || isSkipF10 || isSkipLessThan20)
                        if (isSkipF10 || isSkipLessThan20)
                        {
                            extDataDesc.Fields.Add(kvp.Key, "0");
                        }
                        else
                        {
                            extDataDesc.Fields.Add(kvp.Key, kvp.Value);
                        }
                        #endregion
                        // extDataDesc.Fields.Add(kvp.Key, kvp.Value);
                    }
                }
            // PRIMEPOS - 2530 - Added For Tokenization - Start
            string ccToken = null;
            MessageFields.TryGetValue("TOKEN", out ccToken);
            if (!string.IsNullOrWhiteSpace(ccToken))
            {
                extDataDesc.Fields.Remove("TOKENREQUEST");
                #region PRIMEPOS-3146
                //extDataDesc.Fields.Add("TOKEN", ccToken);
                if (!ccToken.Contains("|"))
                {
                    extDataDesc.Fields.Add("TOKEN", ccToken);
                    traceDesc.OriginalTransactionIdentifier = Guid.NewGuid().ToString("N").Length > 16 ? Guid.NewGuid().ToString("N").Substring(0, 16) : Guid.NewGuid().ToString("N");
                }
                else
                {
                    extDataDesc.Fields.Add("TOKEN", ccToken.Split('|')[0]);
                    traceDesc.OriginalTransactionIdentifier = ccToken.Split('|')[1];
                }
                #endregion  
            }
            // PRIMEPOS - 2530 - Added For Tokenization End
            string isFSA = null;
            MessageFields.TryGetValue("IIASTRANSACTION", out isFSA);
            if (!string.IsNullOrWhiteSpace(isFSA))
            {
                string FSAAmt = null;
                MessageFields.TryGetValue("IIASAUTHORIZEDAMOUNT", out FSAAmt);
                string FSAAmtNew = null;
                if (!string.IsNullOrWhiteSpace(FSAAmt))
                {
                    FSAAmtNew = FSAAmt.Replace(".", "");
                    extDataDesc.Fields.Add("PASSTHRUDATA", "FSA:HealthCare," + FSAAmtNew + "|Rx," + FSAAmtNew);
                }
                #region PRIMEPOS-3078
               

                MessageFields.TryGetValue("CARDTYPE", out string cardType);
                if (!string.IsNullOrWhiteSpace(cardType))
                {
                    extDataDesc.Fields.Add("CARDTYPE", GetCardTypeCode(cardType));//HPSPAX issues found while testing
                }
                if (extDataDesc.Fields.ContainsKey("CARDTYPE"))
                {
                    extDataDesc.Fields.Add("FORCEFSA", "1");
                }
                #endregion
            }

            if (txnType == "HPSPAX_EBT_SALE" || txnType == "HPSPAX_EBT_RETURN")
                accountDesc.EbtType = "F";



            // accountDesc.DupOverrideFlag = "1"; // Nilesh Comment Temp


            // extDataDesc.Fields.Add("DUPOVERRIDEFLAG", "1");

            //return reqMessage = "AMOUNT INFO : "+ amountDesc.GetElementString() +" | ACCOUNT INFO : "+ accountDesc.GetElementString() + 
            //    " | TRACE INFO : " + traceDesc.GetElementString() + " | AVS INFO : " + avsDesc.GetElementString() +
            //    " | CASHIER INFO : " + cashierDesc.GetElementString() + " | COMMERCIAL INFO : " + commercialDesc.GetElementString() +
            //    " | E-COMMERCE INFO : " + eComDesc.GetElementString() + " | EXT-DATA : " + extDataDesc.GetElementString();
            return reqMessage;
        }

        #region PRIMEPOS-3078
        private string GetCardTypeCode(string cardtype)
        {
            if (cardtype.ToUpper() == "VISA")
            {
                return "01";
            }
            else if (cardtype.ToUpper() == "MASTERCARD")
            {
                return "02";
            }
            else if (cardtype.ToUpper() == "AMEX")
            {
                return "03";
            }
            else if (cardtype.ToUpper() == "DISCOVER")
            {
                return "04";
            }
            else if (cardtype.ToUpper().Replace(" ", "") == "DINERCLUB")
            {
                return "05";
            }
            else if (cardtype.ToUpper() == "ENROUTE")
            {
                return "06";
            }
            else if (cardtype.ToUpper() == "JCB")
            {
                return "07";
            }
            else if (cardtype.ToUpper() == "REVOLUTIONCARD")
            {
                return "08";
            }
            else if (cardtype.ToUpper() == "VISAFLEET")
            {
                return "09";
            }
            else if (cardtype.ToUpper() == "MASTERCARDFLEET")
            {
                return "10";
            }
            else if (cardtype.ToUpper() == "FLEETONE")
            {
                return "11";
            }
            else if (cardtype.ToUpper() == "FLEETWIDE")
            {
                return "12";
            }
            else if (cardtype.ToUpper() == "FUELMAN")
            {
                return "13";
            }
            else if (cardtype.ToUpper() == "GASCARD")
            {
                return "14";
            }
            else if (cardtype.ToUpper() == "VOYAGER")
            {
                return "15";
            }
            else if (cardtype.ToUpper() == "WRIGHTEXPRESS")
            {
                return "16";
            }
            else if (cardtype.ToUpper() == "INTERAC")
            {
                return "17";
            }
            else if (cardtype.ToUpper() == "CUP")
            {
                return "18";
            }
            else
            {
                return "99";
            }
        }
        #endregion

        //Use this for processing the transaction of any type.
        /// <summary>
        /// Author : Ritesh 
        /// Functionality Desciption : This method is the core method that does all the work
        /// External functions:MMSDictioanary,PaymentResponse,SocketClient
        /// Known Bugs : None
        /// Start Date : 14 Jan 2008.
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
            //NileshJ-Ritesh - 12-Dec-2018
            if (response == null)
                response = new PAXPaymentResponse();
            //Till - NileshJ-Ritesh
            if (transactionType == "HPSPAX_CANCEL")
            {

                _DeviceResponse = _Device.Cancel();

                if (_DeviceResponse != null)
                {
                    response.ParsePAXResponse(_DeviceResponse);
                }
                else
                {
                    errorMessage = ERROR_COMM_RESPONSE;
                    logger.Error("***PAX , Error: " + ERROR_COMM_RESPONSE);
                }
                return response;
            }
            else
            {
                if (IsValidRequest())
                {
                    logger.Trace("Sending Data to PAX");
                    if (SendReceiveData())
                    {
                        if (_DeviceResponse != null)
                        {
                            response.ParsePAXResponse(_DeviceResponse);
                            #region PRIMEPOS-3146 To Handle Token Transaction not return CardType and Last Four Digit
                            MessageFields.TryGetValue("CARDTYPE", out string cardType);
                            if (!string.IsNullOrWhiteSpace(cardType))
                            {
                                response.CardType = cardType;
                            }
                            MessageFields.TryGetValue("LASTFOUR", out string maskedCardNo);
                            if (!string.IsNullOrWhiteSpace(maskedCardNo))
                            {
                                response.AccountNo = response.MaskedCardNo = maskedCardNo.Trim(); 
                            }
                            #endregion
                            logger.Trace("PAX Device Response code - " + response.Result + " Description - " + response.ResultDescription);
                        }
                        else
                        {
                            errorMessage = ERROR_COMM_RESPONSE;
                            //logger.Error("***PAX , Error: " + ERROR_COMM_RESPONSE);
                            response = new PAXPaymentResponse(); //NileshJ Temp - 12-Dec-2018
                            logger.Error("***PAX , Error: " + string.Concat(errorMessage, "\n", _ErrorMessage));
                            response.ResultDescription = string.Concat(errorMessage, "\n", _ErrorMessage);
                            response.Result = "FAILED";
                            //response = null; //NileshJ Temp - 12-Dec-2018
                        }
                    }
                    else
                    {
                        errorMessage = ERROR_COMM_RESPONSE;
                        //logger.Error("***PAX , Error: " + ERROR_COMM_RESPONSE);
                        response = new PAXPaymentResponse(); //NileshJ Temp - 12-Dec-2018
                        logger.Error("***PAX , Error: " + string.Concat(errorMessage, "\n", _ErrorMessage));
                        response.ResultDescription = string.Concat(errorMessage, "\n", _ErrorMessage);
                        response.Result = "FAILED";
                        //response = null; //NileshJ Temp - 12-Dec-2018
                    }
                    logger.Trace("END PROCESSOR(PAX ProcessTxn)");
                    return response;
                }
            }
            return null;
        }


        //Move to PAXResponse parse method



        /// <summary>
        /// Author : Ritesh 
        /// Functionality Desciption : This method is called to Open,Send,Receive and disconnect the Payment Server.
        /// External functions:None
        /// Known Bugs : None
        /// Start Date : 14 Jan 2008.
        /// </summary>
        /// <param name="strRequest">String</param>
        /// <returns>bool</returns> 
        private bool SendReceiveData()
        {

            logger.Info("SendReceivedData() Start ");
            String reqMessage = PAYMENTPROCESSOR + BuildRequest();
            logger.Trace("Request message is : " + reqMessage);
            // PRIMEPOS - 2532 added to get device message to get message back 
            _Device.OnMessageSent += OnMessageSent_Handler;
            #region PRIMEPOS-2761
            long TransNo = 0;
            CCTransmission_Log cclog = new CCTransmission_Log();
            // Nilesh,Sajid PRIMEPOS-2854
            RecoveryFlag = string.Empty;
            if (!TRANSACTIONTYPE.ToString().Contains("VOID"))
            {
                long.TryParse(traceDesc.TransactionNumber, out TransNo);
                if (TransNo > 0)
                {
                    try
                    {
                        using (var db = new Possql())
                        {
                            RecoveryFlag = "R";
                            cclog = db.CCTransmission_Logs.Where(w => w.TransNo == TransNo).SingleOrDefault();
                            traceDesc.TransactionNumber = cclog.TerminalRefNumber;
                        }
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }
            }
            if (TransNo == 0 || (StationId != cclog.StationID && UserId != cclog.UserID)) // Nilesh,Sajid PRIMEPOS-2854
            {
                using (var db = new Possql())
                {
                    cclog = new CCTransmission_Log();
                    cclog.TransDateTime = DateTime.Now;
                    cclog.TransAmount = amount;
                    cclog.TicketNo = traceDesc.ReferenceNumber;
                    cclog.TransDataStr = _Device.GetDeviceRequestMessage(TXN_TYPE_MAP[txnType], amountDesc, accountDesc, traceDesc
                         , avsDesc, cashierDesc, commercialDesc, eComDesc, extDataDesc);
                    cclog.PaymentProcessor = HPSPAX;
                    cclog.StationID = StationId;
                    cclog.UserID = UserId;
                    cclog.TransmissionStatus = "InProgress";
                    cclog.TransType = TRANSACTIONTYPE;
                    db.CCTransmission_Logs.Add(cclog);
                    db.SaveChanges();
                    db.Entry(cclog).GetDatabaseValues();
                    TransNo = cclog.TransNo;
                }
            }
            #endregion
            try
            {
                switch (txnType)
                {

                    case "HPSPAX_CREDIT_SALE":
                    case "HPSPAX_CREDIT_RETURN":
                    case "HPSPAX_CREDIT_VOID":
                    case "HPSPAX_CREDIT_PREAUTH":
                    case "HPSPAX_CREDIT_POSTAUTH":
                    case "HPSPAX_CREDIT_REVERSE":
                        // SAJID LOCAL DETAILS REPORT PRIMEPOS-2862
                        bool isTransactionRecover = false;
                        bool.TryParse(MessageFields.Where(a => a.Key == "TransactionRecover").SingleOrDefault().Value, out isTransactionRecover);
                        if (isTransactionRecover)
                        {
                            logger.Trace("Entered in LocalDetailReport to get the last transaction: ");
                            _DeviceResponse = _Device.LocalDetailReport(cclog.TicketNo);
                            if (_DeviceResponse != null)
                                logger.Trace("Device reponse is : " + _DeviceResponse.ToString());
                            else
                                logger.Trace("Device reponse is null: ");
                        }
                        else
                        {
                            _DeviceResponse = _Device.DoCredit(TXN_TYPE_MAP[txnType], amountDesc, accountDesc, traceDesc, avsDesc, cashierDesc, commercialDesc, eComDesc, extDataDesc);
                            //PRIMEPOS-3047
                            //if (MessageBox.Show("Do you want to check the local detail report ", "Caption", MessageBoxButtons.YesNo) == DialogResult.Yes)
                            //{
                            //    _DeviceResponse = null;
                            //}
                            #region PRIMEPOS-3087
                            if (_DeviceResponse != null)
                            {
                                logger.Trace("Credit Transaction Device reponse is : " + _DeviceResponse.ToString());
                                if (_DeviceResponse.DeviceResponseCode == DEVICE_RESPONSE_CODE_TIMEOUT)
                                {
                                    logger.Trace($"Credit Transaction in LocalDetailReport to get the last transaction TicketNo:{cclog.TicketNo}");
                                    _DeviceResponse = _Device.LocalDetailReport(cclog.TicketNo);
                                    if (_DeviceResponse != null) 
                                    {
                                        logger.Trace("Credit Transaction Device reponse is : " + _DeviceResponse.ToString());
                                        #region PRIMEPOS-3262
                                        if (_DeviceResponse.DeviceResponseCode == DEVICE_RESPONSE_CODE_NOT_FOUND) //PRIMEPOS-3262
                                        {
                                            _DeviceResponse.DeviceResponseCode = DEVICE_RESPONSE_CODE_TIMEOUT;
                                            _DeviceResponse.DeviceResponseText = DEVICE_RESPONSE_TEXT_TIMEOUT;
                                        }
                                        #endregion
                                    }
                                    else
                                        logger.Trace("Credit Transaction Device reponse is null: ");
                                }
                            }
                            else
                            {
                                logger.Trace("Credit Transaction Device reponse is null");
                            }
                            #endregion
                        }// SAJID LOCAL DETAILS REPORT
                        break;
                    case "HPSPAX_DEBIT_SALE":
                    case "HPSPAX_DEBIT_RETURN":
                    case "HPSPAX_DEBIT_VOID":
                    case "HPSPAX_DEBIT_VOID_RETURN":
                    case "HPSPAX_DEBIT_Auth":
                    case "HPSPAX_DEBIT_REVERSE":
                        // SAJID LOCAL DETAILS REPORT PRIMEPOS-2862
                        isTransactionRecover = false;
                        bool.TryParse(MessageFields.Where(a => a.Key == "TransactionRecover").SingleOrDefault().Value, out isTransactionRecover);
                        if (isTransactionRecover)
                        {
                            logger.Trace("Entered in LocalDetailReport to get the last transaction: ");
                            _DeviceResponse = _Device.LocalDetailReport(cclog.TicketNo);
                            //PRIMEPOS-3047
                            if (_DeviceResponse != null)
                                logger.Trace("Device reponse is : " + _DeviceResponse.ToString());
                            else
                                logger.Trace("Device reponse is null: ");
                        }
                        else
                        {
                            _DeviceResponse = _Device.DoDebit(TXN_TYPE_MAP[txnType], amountDesc, accountDesc, traceDesc, cashierDesc, extDataDesc);
                            #region PRIMEPOS-3087
                            if (_DeviceResponse != null)
                            {
                                logger.Trace("Debit Transaction Device reponse is : " + _DeviceResponse.ToString());
                                if (_DeviceResponse.DeviceResponseCode == DEVICE_RESPONSE_CODE_TIMEOUT)
                                {
                                    logger.Trace($"Debit Transaction in LocalDetailReport to get the last transaction TicketNo:{cclog.TicketNo}");
                                    _DeviceResponse = _Device.LocalDetailReport(cclog.TicketNo);
                                    if (_DeviceResponse != null)
                                    {
                                        logger.Trace("Debit Transaction Device reponse is : " + _DeviceResponse.ToString());
                                        #region PRIMEPOS-3262
                                        if (_DeviceResponse.DeviceResponseCode == DEVICE_RESPONSE_CODE_NOT_FOUND) //PRIMEPOS-3262
                                        {
                                            _DeviceResponse.DeviceResponseCode = DEVICE_RESPONSE_CODE_TIMEOUT;
                                            _DeviceResponse.DeviceResponseText = DEVICE_RESPONSE_TEXT_TIMEOUT;
                                        }
                                        #endregion
                                    }
                                    else
                                        logger.Trace("Debit Transaction Device reponse is null: ");
                                }
                            }
                            else
                            {
                                logger.Trace("Debit Transaction Device reponse is null: ");
                            }
                            #endregion
                        }// SAJID LOCAL DETAILS REPORT
                        break;
                    /* EBT Transaction*/
                    case "HPSPAX_EBT_SALE":
                    case "HPSPAX_EBT_RETURN":
                    case "HPSPAX_EBT_VOID":
                    case "HPSPAX_EBT_Auth":
                    case "HPSPAX_EBT_REVERSE":
                        // SAJID LOCAL DETAILS REPORT PRIMEPOS-2862
                        isTransactionRecover = false;
                        bool.TryParse(MessageFields.Where(a => a.Key == "TransactionRecover").SingleOrDefault().Value, out isTransactionRecover);
                        if (isTransactionRecover)
                        {
                            logger.Trace("Entered in LocalDetailReport to get the last transaction: ");
                            _DeviceResponse = _Device.LocalDetailReport(cclog.TicketNo);
                            //PRIMEPOS-3047
                            if (_DeviceResponse != null)
                                logger.Trace("Device reponse is : " + _DeviceResponse.ToString());
                            else
                                logger.Trace("Device reponse is null: ");
                        }
                        else
                        {
                            _DeviceResponse = _Device.DoEBT(TXN_TYPE_MAP[txnType], amountDesc, accountDesc, traceDesc, cashierDesc);
                            #region PRIMEPOS-3087
                            if (_DeviceResponse != null)
                            {
                                logger.Trace("EBT Transaction Device reponse is : " + _DeviceResponse.ToString());
                                #region PRIMEPOS-3146
                                if (_DeviceResponse.DeviceResponseCode == DEVICE_RESPONSE_CODE_TIMEOUT)
                                {
                                    logger.Trace($"EBT Transaction in LocalDetailReport to get the last transaction TicketNo:{cclog.TicketNo}");
                                    _DeviceResponse = _Device.LocalDetailReport(cclog.TicketNo);
                                    //PRIMEPOS-3047
                                    if (_DeviceResponse != null)
                                    {
                                        logger.Trace("EBT Transaction Device reponse is : " + _DeviceResponse.ToString());
                                        #region PRIMEPOS-3262
                                        if (_DeviceResponse.DeviceResponseCode == DEVICE_RESPONSE_CODE_NOT_FOUND) //PRIMEPOS-3262
                                        {
                                            _DeviceResponse.DeviceResponseCode = DEVICE_RESPONSE_CODE_TIMEOUT;
                                            _DeviceResponse.DeviceResponseText = DEVICE_RESPONSE_TEXT_TIMEOUT;
                                        }
                                        #endregion
                                    }
                                    else
                                    {
                                        logger.Trace("EBT Transaction Device reponse is null: ");
                                    }
                                }
                                #endregion
                            }
                            else
                            {
                                logger.Trace("EBT Transaction Device reponse is null: ");
                            }
                            #endregion
                        }// SAJID LOCAL DETAILS REPORT
                        break;
                    default:
                        errorMessage = "INVALID_TXN_TYPE";
                        _DeviceResponse = null;
                        break;
                }
                #region PRIMEPOS-2761
                var db = new Possql();
                OrgTransNo = 0;
                ApprovedAmount = 0;//PRIMEPOS-3087
                if (_DeviceResponse.AmountResponse != null)
                {
                    ApprovedAmount = Convert.ToDecimal(_DeviceResponse.AmountResponse.ApprovedAmount);
                }
                CCTransmission_Log cclog1 = new CCTransmission_Log();
                if (_DeviceResponse.TransactionType != null)
                {
                    if (_DeviceResponse.TransactionType.ToString().Contains("VOID") || TRANSACTIONTYPE.ToString().Contains("VOID"))
                    {
                        cclog1 = db.CCTransmission_Logs.Where(w => w.TerminalRefNumber == traceDesc.TransactionNumber).OrderByDescending(e => e.TransDateTime).FirstOrDefault();//Added by Arvind for void issue
                        cclog1.IsReversed = true;
                        OrgTransNo = cclog1.TransNo;
                        db.CCTransmission_Logs.Attach(cclog1);
                        db.Entry(cclog1).Property(p => p.IsReversed).IsModified = true;
                        db.SaveChanges();
                    }
                }
                //PRIMEPOS-3047
                logger.Trace("Device reponse is before Completed: " + _DeviceResponse.ToString());
                logger.Trace("Device HostRefereceNumber is before Completed: " + _DeviceResponse.HostResponse?.HostRefereceNumber);
                logger.Trace("Device TerminalRefNumber is before Completed: " + _DeviceResponse.TerminalRefNumber);
                cclog = new CCTransmission_Log();
                cclog = db.CCTransmission_Logs.Where(w => w.TransNo == (TransNo != 0 ? TransNo : 0)).SingleOrDefault();
                // cclog = null;
                cclog.RecoveryFlag = RecoveryFlag;// Nilesh,Sajid PRIMEPOS-2854
                cclog.IsReversed = false;
                //cclog.TransDataStr = _DeviceRequestMessage;
                cclog.RecDataStr = _DeviceResponse.ToString();
                //cclog.TicketNo = traceDesc.ReferenceNumber;
                cclog.HostTransID = _DeviceResponse.HostResponse?.HostRefereceNumber;//Amit
                cclog.TransmissionStatus = "Completed";
                cclog.TerminalRefNumber = _DeviceResponse.TerminalRefNumber;
                cclog.AmtApproved = ApprovedAmount;
                #region PRIMEPOS-3182
                if (!string.IsNullOrWhiteSpace(_DeviceResponse.MaskedCardNumber) && _DeviceResponse.MaskedCardNumber.Trim().Length >= 4)
                {
                    cclog.last4 = _DeviceResponse.MaskedCardNumber.Trim().Substring(_DeviceResponse.MaskedCardNumber.Trim().Length - 4, 4);
                }
                #endregion
                cclog.OrgTransNo = OrgTransNo; // Orginal Transaction Numner for reverse
                if (_DeviceResponse.TransactionType != null)
                {
                    if (_DeviceResponse.TransactionType.ToString().Contains("VOID") || TRANSACTIONTYPE.ToString().Contains("VOID"))
                    {
                        cclog.TransAmount = ApprovedAmount;
                    }
                }
                cclog.ResponseMessage = _DeviceResponse.ResponseText;
                db.CCTransmission_Logs.Attach(cclog);
                //db.Entry(cclog).Property(p => p.TransDataStr).IsModified = true;
                db.Entry(cclog).Property(p => p.RecDataStr).IsModified = true;
                //db.Entry(cclog).Property(p => p.TicketNo).IsModified = true;
                db.Entry(cclog).Property(p => p.TransmissionStatus).IsModified = true;
                db.Entry(cclog).Property(p => p.HostTransID).IsModified = true;
                db.Entry(cclog).Property(p => p.TerminalRefNumber).IsModified = true;
                db.Entry(cclog).Property(p => p.AmtApproved).IsModified = true;
                db.Entry(cclog).Property(p => p.IsReversed).IsModified = true;
                db.Entry(cclog).Property(p => p.OrgTransNo).IsModified = true;
                db.Entry(cclog).Property(p => p.ResponseMessage).IsModified = true;
                db.Entry(cclog).Property(p => p.last4).IsModified = true;   //PRIMEPOS-3182
                db.Entry(cclog).Property(p => p.RecoveryFlag).IsModified = true;//Nilesh,Sajid PRIMEPOS-2854
                if (_DeviceResponse.TransactionType != null)
                {
                    if (_DeviceResponse.TransactionType.ToString().Contains("VOID") || TRANSACTIONTYPE.ToString().Contains("VOID"))
                    {
                        db.Entry(cclog).Property(p => p.TransAmount).IsModified = true;
                    }
                }
                db.SaveChanges();

                db.Dispose();
                //PRIMEPOS-3047
                logger.Trace("Device reponse is after Completed: " + _DeviceResponse.ToString());
                logger.Trace("Device HostRefereceNumber is after Completed: " + _DeviceResponse.HostResponse?.HostRefereceNumber);
                logger.Trace("Device TerminalRefNumber is after Completed: " + _DeviceResponse.TerminalRefNumber);
                #endregion


            }
            catch (Exception ex)
            {
                logger.Trace(ex.ToString());
                if (ex.InnerException != null)
                {
                    _ErrorMessage = ex.InnerException.Message + "|" + TransNo.ToString();
                }

                else
                {
                    _ErrorMessage = ex.Message + "|" + TransNo.ToString();
                }
                return false;// NileshJ Temp - 20-Dec-2018
            }
            #region PRIMEPOS-2761 - Commented
            //try {

            //    // PRIMEPOS - 2532 Modified cclog.TransDataStr,cclog.RecDataStr to add 
            //    //Request and Response of device to CCTransmission_Log table 
            //    using (var db = new Possql()) {
            //        CCTransmission_Log cclog = new CCTransmission_Log();
            //        cclog.TransDateTime = DateTime.Now;
            //        cclog.TransAmount = amount;
            //        cclog.TransDataStr = _DeviceRequestMessage;
            //        cclog.RecDataStr = _DeviceResponse.ToString();
            //        db.CCTransmission_Logs.Add(cclog);
            //        db.SaveChanges();
            //    }
            //} catch (Exception exp) {
            //    logger.Trace(exp.ToString());
            //    return false;// NileshJ Temp - 20-Dec-2018
            //}
            #endregion
            //End Added
            return true;

        }

        private void OnMessageSent_Handler(string message)
        {
            _DeviceRequestMessage = message;
            Console.WriteLine("PAX message Processor " + _DeviceRequestMessage);
        }



        /// <summary>
        /// Author : Ritesh 
        /// Functionality Desciption : This method is called to validate the Valid Fields and Mandatory fields.
        /// External functions:None
        /// Known Bugs : None
        /// Start Date : 14 Jan 2008.
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
        /// Author : Ritesh 
        /// Functionality Desciption : This method is used for checking the Valid Fields for a Processor.
        /// External functions:None
        /// Known Bugs : None
        /// Start Date : 14 Jan 2008.
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
        /// Author : Ritesh 
        /// Functionality Desciption : This method is used for checking the Mandatory Fields for a Transaction type.
        /// External functions:None
        /// Known Bugs : None
        /// Start Date : 14 Jan 2008.
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
        /// Author : Ritesh 
        /// Functionality Desciption : This method is used for fetching the mandatory fields of transaction from MMSDictionary or MandaotoryFields.xml file.
        /// External functions:MMSDictionary,XmlToKeys
        /// Known Bugs : None
        /// Start Date : 14 Jan 2008.
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
        /// Author : Ritesh 
        /// Functionality Desciption : This method is used for ensuring valid parameters are passed (Null check).
        /// External functions:MMSDictionary,XmlToKeys
        /// Known Bugs : None
        /// Start Date : 14 Jan 2008.
        /// </summary>
        /// <param name="keys">MMSDictioanary</param>
        /// <returns>bool vaild</returns>
        public bool ValidateParameters(ref MMSDictionary<String, String> keys)
        {
            logger.Trace("In ValidateParameters()");
            bool flag = true;
            bool isValid = true;
            string sError = string.Empty;
            const String PAX = "HPSPAX";
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
                isValid = this.GetProcessorTag(kvp.Key, PAX, out orgParam);
                if (isValid == true)
                {
                    orignalKeys.Add(orgParam, kvp.Value);
                }
            }
            keys = orignalKeys;
            //Commented by Ritesh unnecessary complication
            /*
            if (XLINKValidateParams.DefaultInstance.CheckXcardParams(keys, out sError) == false)
            {
                flag = false;
            }
            else
            {
                flag = true;
            }            
            */
            return flag;
        }
        public bool GetProcessorTag(string xmlCommonNode, string xmlProcessorName, out string ProcessorTag)
        {
            bool isValid = true;
            string tagValue = string.Empty;

            XmlNodeList xmlList;

            xmlList = xmlPAX.GetElementsByTagName(xmlCommonNode);
            if (xmlList.Count > 0)//PRIMEPOS-3090
            {
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
            }
            else//PRIMEPOS-3090
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
                if (response != null)
                {
                    response.Dispose();
                    response = null;
                }
                errorMessage = null;
                txnType = null;
                ResponseMessage = null;

                amountDesc = null;
                accountDesc = null;
                traceDesc = null;
                avsDesc = null;
                cashierDesc = null;
                commercialDesc = null;
                eComDesc = null;
                extDataDesc = null;

            }



            // Unmanaged cleanup code here

            Disposed = true;

        }

        #endregion
    }
}
