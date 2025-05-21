//Author: Manoj Kumar
//Company: Micro Merchant Systems, 2012
//Function: Base processor for transactions.
//Implementation: For HPS (Heartland Payment Systems)

using System;
using System.Collections.Generic;
using System.Text;
using Hps.Exchange.PosGateway.Client;
using MMS.PROCESSOR;
using System.Xml;
using System.Diagnostics;
using System.IO;
//using Logger = AppLogger.AppLogger;
using System.Net;
using Rebex.Net;
using PossqlData;
using NLog;
using System.Linq;//PRIMEPOS-2761

namespace MMS.HPS
{
    public abstract class Processor:IDisposable
    {
        private ILogger logger = LogManager.GetCurrentClassLogger();

        private MMSDictionary<String, MMSDictionary<String, String>> MandatoryKeys = null;
        private MMSDictionary<String, String> ValidKeys = null;
        private MMSDictionary<String, String> MessageFields = null;
        private XmlToKeys XmlToKeys = null;
        private KeysToXml Fields = null;
        private PaymentResponse response = null;
        private String errorMessage = String.Empty;
        private String txnType = String.Empty;
        private String ResponseMessage = String.Empty;
        private MerchantInfo Merchant = null;
        private Boolean Disposed = false;
        XmlDocument xmlxCharge = null;
        PosGatewayInterface client = new PosGatewayInterface();


        #region Requestconstants
        private const String MSG_HEADER = "<XML_REQUEST>";
        private const String MSG_FOOTER = "</XML_REQUEST>";
        private const String MSG_START = "<XML_FILE>";
        private const String MSG_END = "</XML_FILE>";
        private const String RESPONSE_FILTERNODE = "XML_REQUEST";
        private const String MERCH_NUM = "MERCHANTVERIFICATIONVALUE";
        private const String USERNAME = "USERNAME";
        private const String VALID_FIELDS = "ValidFields.xml";
        private const String MANDATORY_FIELDS = "MandatoryFields.xml";
        private const String XML_CONST = "TXN";
        private const String PROCESSOR_ID = "PROCESSOR_ID";
        public const String FAILED_OPRN = "FAILED";
        public const String MMS_CARD = "MMSCARD";
        public const String PCCHARGE = "PCCHARGE";
        public const string HPS = "HPS";
        public const String INVALID_PARAMETERS = "INVALID_PARAMETERS";
        private const string COMMOMPROCESSORTAG = "CommonProcessorTag.xml";
        private const string ERRORRSPONSE = "<XML_REQUEST><USER_ID>User1</USER_ID><TROUTD></TROUTD><RESULT>Error</RESULT><AUTH_CODE>Payment Server could not be contacted</AUTH_CODE><REFERENCE></REFERENCE><INTRN_SEQ_NUM></INTRN_SEQ_NUM><TOTALTRANSTIME></TOTALTRANSTIME></XML_REQUEST>";
        private const string FSA = "FSA";
        private const string FSA_PARTIAL_AUTH = "FSA_PARTIAL_AUTH";
        private const string AMOUNT_PRESCRIPTION = "AMOUNT_PRESCRIPTION";
        private const string AMOUNT_VISION = "AMOUNT_VISION";
        private const string AMOUNT_CLINIC = "AMOUNT_CLINIC";
        private const string AMOUNT_DENTAL = "AMOUNT_DENTAL";
        private const string CLOSEXMLREQUESTTAG = "</XML_REQUEST>";
        private const string OPENXMLREQUESTTAG = "<XML_REQUEST>";
        private const string OPENXMLTRANSAMOUNT = "<TRANS_AMOUNT>"; 
        private const string CLOSEXMLTRANSAMOUNT = "</TRANS_AMOUNT>"; 
        private const string PAYMENTPROCESSOR = "HPS|";
        private const string INVALIDPAYMENTPROCESSOR = "INVALID PAYMENT PROCESSOR SELECTION";
        private const string REQUESTAMOUNTFIELD = "TRANS_AMOUNT";
      
        private const string OPENINVPROCESSOR = "<INV_PROCESSOR>";
        private const string CLOSEINVPROCESSOR = "</INV_PROCESSOR>";
        private const string INV_PROCESSOR = "INV_PROCESSOR";
        private const string AMOUNT = "AMOUNT";
        private const string ACCOUNTNUM = "ACCOUNTNUM";
        private const string VERSION = "VERSION";
        private const string EXPMONTH = "EXPMONTH";
        private const string EXPYEAR = "EXPYEAR";
        private const string EXPDATE = "EXPDATE";
        private const string MANUALFLAG = "MANUALFLAG";
        private const string DIRECTMKTINVOICENBR = "DIRECTMKTINVOICENBR";
        private const string DIRECTMKTSHIPMONTH = "DIRECTMKTSHIPMONTH";
        private const string DIRECTMKTSHIPDAY = "DIRECTMKTSHIPDAY";
        private const string FIRSTADDITIONALAMTINFO = "FIRSTADDITIONALAMTINFO";
        private const string AUTOSUBSTANTIATION = "AUTOSUBSTANTIATION";
        private const string SECONDADDITIONALAMTINFO = "SECONDADDITIONALAMTINFO";
        private const string FOURTHADDITIONALAMTINFO = "FOURTHADDITIONALAMTINFO";
        private const string CARDTYPE = "CARDTYPE";
        private const string TROUTID = "TROUTID";
        private const string MMSCARD = "MMSCARD";
        private const string LICENSEID = "LICENSEID";
        private const string SITEID = "SITEID";
        private const string DEVICEID = "DEVICEID";
        private const string SITETRACE = "SITETRACE";
        private const string PASSWORD = "PASSWORD";
        private const string CARDNBR = "CARDNBR";
        private const string AMT = "AMT";
        private const string GATEWAYTXNID = "GATEWAYTXNID";
        private const string TRACKDATA = "TRACKDATA";
        private const string CARDPRESENT = "CARDPRESENT";
        private const string ISCARDSWIPE = "ISCARDSWIPE";
        private const string AUTHAMT = "AUTHAMT";
        private const string PINBLOCK = "PINBLOCK";
        private const string ALLOWDUP = "ALLOWDUP";
        private const string TRUE = "TRUE";
        private const string ENABLETRACE = "ENABLETRACE";
        private const string URL = "URL";

        #endregion

        #region constants
        private const String RESULT = "RESULT";
        private const String RESPONSETEXT = "RSPTEXT";
        private const String TRANSACTIONID = "GateWayTxnID";//"TROUTD";
        private const String AUTHCODE = "AUTHCODE";
        private const String AMOUNT_APPROVED = "AUTH_AMOUNT";
        private const String ADDITIONAL_FUNDS_REQUIRED = "AMOUNT_DUE";
        private const String RESULT_CAPTURED = "CAPTURED";
        private const String RESULT_FAILURE = "FAILURE";
        private const String RESULT_SUCCESS = "SUCCESS";
        private const String RESULT_APPROVED = "APPROVED";
        private const String RESULT_VOIDED = "VOIDED";
        private const String RESULT_PROCESSED = "PROCESSED";
        private const String RESULT_SALERECEIVED = "SALERECEIVED";
        private const String RESULT_RETURNRECOVERED = "RETURNRECOVERED";
        private const String RESULT_NOT_CAPTURED = "NOT CAPTURED";
        private const String RESULT_NOT_APPROVED = "NOT APPROVED";
        private const String RESULT_CANCELLED = "CANCELLED";
        private const String RESULT_SALE_NOT_FOUND = "SALE NOT FOUND";
        private const String TRANS_DATE = "TRANS_DATE";
        private const String MAX_AUTH_AMOUNT = "MAX_AUTH_AMOUNT";
        private const int AmountRoundDigit = 2;
        private const string INVALIDPAYMENT_PROCESSOR = "INVALID PAYMENT PROCESSOR SELECTION";
        private const string INV_MESG = "INV_MESG";
        private const string PARTIAL_APP = "PARTIAL APPROVAL";
        private const string HPS_SALE = "HPS_SALE";
        private const string HPS_VOID = "HPS_VOID";
        private const string HPS_RETURN = "HPS_RETURN";
        private const string HPS_VOIDCREDIT = "HPS_VOIDCREDIT";
        private const string HPS_PRE_AUTH = "HPS_PRE_AUTHORIZATION";
        private const string HPS_POST_AUTH = "HPS_POST_AUTHORIZATION";
        private const string HPS_REVERSE = "HPS_REVERSE";
        private const string HPS_DEBITSALE = "HPS_DEBITSALE";
        private const string HPS_DEBITREVERSE = "HPS_DEBITREVERSE";
        private const string HPS_DEBITRETURN = "HPS_DEBITRETURN";
        private const string HPS_EBTSALES = "HPS_EBTSALES";
        private const string HPS_EBTRETURN = "HPS_EBTRETURN";
        private const string MERCHANT = "MERCHANT";
        private const string CARDHOLDERZIP = "CARDHOLDERZIP";
        private const string CARDHOLDERADDR = "CARDHOLDERADDR";
        private const string CARDHOLDER = "CARDHOLDER";
        #region PRIMEPOS-2761
        private const string TICKETNUMBER = "TICKETNUMBER";
        private const string StationID = "StationID";
        private const string USERID = "USERID";
        #endregion
        #endregion


        /// <summary>
        /// Author : Manoj Kumar 
        /// Functionality Desciption : Send Request to HPS Gateway to get response
        /// </summary>
        /// <param name="strRequest">String</param>
        /// <returns>bool</returns> 
        private  PosResponse Process(PosGatewayInterface client, PosRequest req)
        {
            // Send To web service
            
            logger.Trace("Start HPS PROCESS() (sending data to the gateway)"); 
            PosResponse rsp = null;
            try
            {
                //Added By Rohit Nair force it to communicate with TLS1.2 Protocol
                ServicePointManager.Expect100Continue = true;
                //ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;    //PRIMEPOS-3179 27-Jan-2023 JY Commented

                rsp = client.DoTransaction(req); //send request to get process on HPS exchange
                logger.Trace("HPS PROCESS() data received");
            }
            catch (Exception ex)
            {
                //string excetion = ex.Message.ToString();
                logger.Error(ex,"***********HPS PROCESS() Error exception--\n"+ex.Message);
                return rsp;
            }
            return rsp; //return the result
        }

        public Processor()
        {
            logger.Trace("In Processor() CTOR");
        }

        /// <summary>
        /// Author : Ritesh 
        /// Functionality Desciption : This method is used to Build request sent to PaymentServer.
        /// External functions:KesyToXml
        /// Known Bugs : None
        /// Start Date : 29 JULY 2010.
        /// </summary>
        /// <returns>String</returns>
        private String BuildRequest()
        {
            logger.Trace("In BuildRequest() ");
            String reqMessage = string.Empty;
            String processor = String.Empty;
            String fsaStat = String.Empty;
            MessageFields.TryGetValue(PROCESSOR_ID, out processor);
           // bool isFSA = MessageFields.TryGetValue(FSA, out fsaStat);
            processor = PadSpaces(processor, 4);
            MessageFields.Add(PROCESSOR_ID, processor);
            //if (isFSA == true)
            //{
            //    MessageFields.Add(FSA_PARTIAL_AUTH, "1");
            //}
            reqMessage = Fields.BuildXML(ref MessageFields, MSG_HEADER, MSG_FOOTER);
            return reqMessage;
        }

        /// <summary>
        /// Author : Ritesh 
        /// Functionality Desciption : This method is used to Pad spaces for the processor
        /// External functions:MMSDictionary,KesyToXml
        /// Known Bugs : None
        /// Start Date : 28 JULY 2010.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="count"></param>
        /// <returns>String</returns>
        private String PadSpaces(String value, int count)
        {
            logger.Trace("In Padspaces() CTOR");
            return value.ToString().PadRight(count, ' ');
        }

        /// <summary>
        /// Author : Ritesh 
        /// Functionality Desciption : This method is constructor for the CreditProcessor class
        /// External functions:None
        /// Known Bugs : None
        /// Start Date :  28 JULY 2010.
        /// </summary>
        /// <param name="ProcessorKey"></param>
        /// <param name="merchant">MerchantInfo</param>
        public Processor(string ProcessorKey, MerchantInfo merchant)
        {
            logger.Trace("In Processor() CTOR");
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
            response = new HPSPaymentResponse();
            
            xmlxCharge = new XmlDocument();
            xmlxCharge.Load(COMMOMPROCESSORTAG);
            //Get reference to the Socket Client to Payment Server.
            //PaymentConn = new SocketClient("",0);
        }

        /// <summary>
        /// Author: Manoj Kumar
        /// Description: Compare Card bin with HPS FSA bin on their site
        /// </summary>
        /// <param name="URL"></param>
        /// <param name="cbin"></param>
        /// <returns>string</returns>
        private string FSACheck(string URL, string cbin)
        {
            logger.Trace("HPS FSA Card check Start FSACheck()-----");
            string AutoSub = "0";
            string FilePath = string.Empty;
            try
            {
                FilePath = AppDomain.CurrentDomain.BaseDirectory + URL;
                if(File.Exists(FilePath))
                {
                    using(var rd = XmlReader.Create(FilePath))
                    {//HPS FSA bin url get from Merchantconfig.xml
                        while(rd.Read())
                        {
                            switch(rd.NodeType)
                            {
                                case XmlNodeType.Element:
                                    while(rd.MoveToNextAttribute())
                                    {
                                        if(rd.Value == cbin.Trim()) //if the card matches then its a FSA card
                                        {
                                            AutoSub = "1";
                                            logger.Trace("HPS FSACheck() Card is FSA");
                                            rd.Close(); // close the reader
                                            logger.Trace("HPS FSA Card Check Finish");
                                            return AutoSub;
                                        }
                                    }
                                    break;
                            }
                        }
                    }
                }
                else
                {
                    throw new FileNotFoundException();
                }
                logger.Trace("HPS FSA Card Check Finish");
            }
            catch (Exception ex)
            {
                logger.Trace(ex,"****HPS FSACheck() Error: " + ex.Message);
                AutoSub = "0";
            }
            return AutoSub; // return 0 if card is not FSA or 1 if it is FSA
        }

        /// <summary>
        /// Desciption : This method does all the work for HPS
        /// External functions: MMSDictioanary,PaymentResponse,PosGatewayInterface. 
        /// </summary>
        /// <param name="transactionType"></param>
        /// <param name="requestMsgKeys"></param>
        /// <returns>PaymentResponse</returns>
        protected PaymentResponse ProcessTxn(string  transactionType, ref MMSDictionary<String, String> requestMsgKeys)
        {
            //Clear all the fields. 
            client = new PosGatewayInterface();
            //test;
            //client.Url = "https://posgateway.cert.secureexchange.net/Hps.Exchange.PosGateway.UAT/PosGatewayService.asmx";    
            ClearFields();
            MMSDictionary<String, String> configValue = new MMSDictionary<string, string>();
            MessageFields = requestMsgKeys;
            txnType = transactionType.ToString();
            String value = String.Empty;
            String merchantNumber = String.Empty;
            String processorId = String.Empty;
            PosRequest oPosReq=new PosRequest();
            PosResponse oPosResp;
            PosRequestVer10 reqVer1_0 = new PosRequestVer10();
            HPSPaymentResponse objPaymentRes = new HPSPaymentResponse();
            oPosReq.Item = reqVer1_0;

            #region Variables
            string amt=string.Empty;
            string cardNo = string.Empty;
            string expMonth = string.Empty;
            string expYear = string.Empty;
            string expDate=string.Empty;
            string version=string.Empty;
            string dirMktInvoiceNo=string.Empty;
            string dirMktShipMonth=string.Empty;
            string dirMktShipDay=string.Empty;
            string manualFlag=string.Empty;
            string cardType=string.Empty;
            string gateWayTxnID=string.Empty;
            string licenseID=string.Empty;
            string siteID=string.Empty;
            string deviceID=string.Empty;
            string siteTrace=string.Empty;
            string userName=string.Empty;
            string password=string.Empty;
            string year = string.Empty;
            string firstAddAmt = string.Empty;
            string pinBlock = string.Empty;
            string trackData = string.Empty;
            string autoSubst = string.Empty;
            string presAmt=string.Empty;
            string visionAmt=string.Empty;
            string sTrackData=string.Empty;
            string sAuthAmt=string.Empty;
            string sPinBlock=string.Empty;
            string sAllowDup = string.Empty;
            string sZip = string.Empty;
            string sAddr = string.Empty;
            String sHPSValue = String.Empty;
            string sCardHolName = string.Empty;
            string pUrl = string.Empty;
            int century = 1900;
            #region PRIMEPOS-2761
            string sUserID = string.Empty;
            string sStationID = string.Empty;
            string sTicketNumber = string.Empty;
            #endregion
            #endregion
            logger.Trace("Start Processor (HPS ProcessTxn) ----" + transactionType);
           /* if (!Merchant.GetMerchantInfo(value, out merchantNumber, out processorId))
            {
                return null;
            }*/

            if (Merchant.oMerchantInfo == null)
            {
                return null;
            }

            merchantNumber = Merchant.oMerchantInfo.Merchant;
            processorId = Merchant.oMerchantInfo.Processor_ID;

            client.Url = Merchant.oMerchantInfo.URL;
            licenseID = Merchant.oMerchantInfo.LicenseID;
            deviceID = Merchant.oMerchantInfo.DeviceID;
            siteID = Merchant.oMerchantInfo.SiteID;

            /*configValue= Merchant.GetMerchantInfo(); //read from Merchantconfig.xml
            if (configValue.TryGetValue(URL, out pUrl))
            {
                client.Url = pUrl;
            }
            if (configValue.TryGetValue(LICENSEID, out sHPSValue))
             {
                 licenseID = sHPSValue;
             }
             if (configValue.TryGetValue(DEVICEID, out sHPSValue))
             {
                 deviceID = sHPSValue;
             }
             if (configValue.TryGetValue(SITEID, out sHPSValue))
             {
                 siteID = sHPSValue;
             }*/
            requestMsgKeys.TryGetValue(EXPDATE, out expDate);
            expMonth = expDate;
            expYear = expDate;

            if (expDate != null)
            {
                MessageFields.Add(EXPMONTH, expMonth);
                MessageFields.Add(EXPYEAR, expYear);
                MessageFields.Remove(EXPDATE);
            }
            MessageFields.Remove(MMS_CARD);

            if (IsValidRequest()) //check the fields to make sure they are valid
            {
                requestMsgKeys.TryGetValue(USERNAME, out userName);
                requestMsgKeys.TryGetValue(PASSWORD, out password);
                reqVer1_0.Header = new PosRequestVer10Header();
                reqVer1_0.Header.LicenseId = Convert.ToInt32(licenseID);
                reqVer1_0.Header.SiteId = Convert.ToInt32(siteID);
                reqVer1_0.Header.DeviceId = Convert.ToInt32(deviceID);
                reqVer1_0.Header.UserName = userName;
                reqVer1_0.Header.Password = password;
                reqVer1_0.Header.DeveloperID = "002712";
                reqVer1_0.Header.VersionNbr = "1150";
                reqVer1_0.Header.SiteTrace = "trace0001";
                reqVer1_0.Transaction = new PosRequestVer10Transaction();
                switch (transactionType)
                {
                        #region HPS_CREDITSALE
                    case HPS_SALE:
                        requestMsgKeys.TryGetValue(AMT, out amt);
                        requestMsgKeys.TryGetValue(CARDNBR, out cardNo);
                        requestMsgKeys.TryGetValue(EXPMONTH, out expMonth);
                        requestMsgKeys.TryGetValue(EXPYEAR, out expYear);
                        requestMsgKeys.TryGetValue(ISCARDSWIPE, out manualFlag);                        
                        requestMsgKeys.TryGetValue(MMSCARD, out cardType);
                        requestMsgKeys.TryGetValue(FIRSTADDITIONALAMTINFO, out firstAddAmt);
                        requestMsgKeys.TryGetValue(AUTOSUBSTANTIATION, out autoSubst);
                        requestMsgKeys.TryGetValue(SECONDADDITIONALAMTINFO, out presAmt);
                        requestMsgKeys.TryGetValue(FOURTHADDITIONALAMTINFO, out visionAmt);
                        requestMsgKeys.TryGetValue(TRACKDATA, out trackData);
                        requestMsgKeys.TryGetValue(ALLOWDUP, out sAllowDup);
                        requestMsgKeys.TryGetValue(CARDHOLDERZIP, out sZip);
                        requestMsgKeys.TryGetValue(CARDHOLDERADDR, out sAddr);
                        requestMsgKeys.TryGetValue(CARDHOLDER, out sCardHolName);
                        #region PRIMEPOS-2761
                        requestMsgKeys.TryGetValue(DIRECTMKTINVOICENBR, out sTicketNumber);
                        requestMsgKeys.TryGetValue(StationID, out sStationID);
                        requestMsgKeys.TryGetValue(USERNAME, out sUserID);
                        #endregion
                        PosCreditSaleReqType creditSale = new PosCreditSaleReqType();
                        reqVer1_0.Transaction.Item = creditSale;
                        reqVer1_0.Transaction.ItemElementName = ItemChoiceTypePosRequestVer10Transaction.CreditSale;
                        creditSale.Block1 = new CreditSaleReqBlock1Type();
                        string FSAUrl= string.Empty;
                        string isFSA = string.Empty;

                        if (autoSubst != null) //if POS pass transaction as FSA this will be '1' else null
                        {
                            if (cardNo.StartsWith("5")) //Checking which card it is to compare with HPS FSA bin (Master card)
                            {
                                //string MCBIN = "MCBIN";
                                //if (configValue.TryGetValue(MCBIN, out FSAUrl))
                                //{
                                FSAUrl = Merchant.oMerchantInfo.MCBin;
                                isFSA = FSACheck(FSAUrl, cardNo.Substring(0, 6)); // if the check return 1 then it is a FSA card else 0
                                //}
                            }
                            else if (cardNo.StartsWith("4")) //Checking which card it is to compare with HPS FSA bin (Visa card)
                            {
                                //string VCBIN = "VCBIN";
                                //if (configValue.TryGetValue(VCBIN, out FSAUrl))
                                //{
                                    FSAUrl = Merchant.oMerchantInfo.VCBin;
                                    isFSA = FSACheck(FSAUrl, cardNo.Substring(0, 6)); // if the check return 1 then it is a FSA card else 0
                                //}
                            }
                        }
                        if (sAllowDup != null && sAllowDup != string.Empty)
                        {
                            if (sAllowDup.ToUpper() == TRUE)
                            {
                                creditSale.Block1.AllowDup = booleanType.Y;
                                creditSale.Block1.AllowDupSpecified = true;
                            }
                            else
                            {
                                creditSale.Block1.AllowDup = booleanType.N;
                                creditSale.Block1.AllowDupSpecified = false;
                            }
                        }
                            if (isFSA.Trim() == "1") 
                            {//if FSA is 1 then it is a FSA card. Now send FSA amount else just send the full amount because its not FSA
                                    AutoSubstantiationType autoSub = new AutoSubstantiationType();
                                    creditSale.Block1.AutoSubstantiation = autoSub;
                                    autoSub.MerchantVerificationValue = "";
                                    autoSub.FirstAdditionalAmtInfo = new AdditionalAmtType();
                                    autoSub.FirstAdditionalAmtInfo.Amt = Convert.ToDecimal(firstAddAmt);
                                    autoSub.FirstAdditionalAmtInfo.AmtType = amtTypeType.TOTAL_HEALTHCARE_AMT;
                                    autoSub.SecondAdditionalAmtInfo = new AdditionalAmtType();
                                    autoSub.SecondAdditionalAmtInfo.Amt = Convert.ToDecimal(presAmt);
                                    autoSub.SecondAdditionalAmtInfo.AmtType = amtTypeType.SUBTOTAL_PRESCRIPTION_AMT;
                                    autoSub.RealTimeSubstantiation = booleanType.Y;  
                            }
                            creditSale.Block1.CardData = new CardDataType();
                            creditSale.Block1.AllowPartialAuth = booleanType.N;//booleanType.Y
                            creditSale.Block1.AllowPartialAuthSpecified = false;  
                            creditSale.Block1.Amt = Convert.ToDecimal(amt);
                            response.sHpsTran_Amt = amt;
                            if (manualFlag.ToString().Trim() == "0") //Manual Flag is 0 if the track II is empty. Card number enter manually.
                            {// Submit transaction as manual
                                CardDataTypeManualEntry manualEntry = new CardDataTypeManualEntry();
                                creditSale.Block1.CardData.Item = manualEntry;
                                manualEntry.CardNbr = cardNo;
                                year = expYear.Substring(2, 2);
                                if (Convert.ToInt32(year) < 80)
                                {
                                    century = 2000;
                                }
                                DateTime dtExpyear = new DateTime(century + Convert.ToInt32(year), 1, 1);
                                int iExpYear = dtExpyear.Year;
                                manualEntry.ExpMonth = Convert.ToInt32(expMonth.Substring(0, 2).ToString());
                                manualEntry.ExpYear = iExpYear;
                                manualEntry.CardPresent = booleanType.Y;
                                manualEntry.CardPresentSpecified = true;
                                manualEntry.ReaderPresent = booleanType.Y;
                                manualEntry.ReaderPresentSpecified = true;
                                CardHolderDataType objCardHolderDdata = new CardHolderDataType();
                                if (sZip != null && sZip != string.Empty)
                                {
                                    objCardHolderDdata.CardHolderZip = sZip;
                                    creditSale.Block1.CardHolderData = objCardHolderDdata;
                                }
                                if (sAddr != null && sAddr != string.Empty)
                                {
                                    objCardHolderDdata.CardHolderAddr = sAddr;
                                    creditSale.Block1.CardHolderData = objCardHolderDdata;
                                }                  
                            }
                            else
                            {
                                CardDataTypeTrackData objReturnTrackData = new CardDataTypeTrackData();
                                objReturnTrackData.Value = trackData.Trim();
                                creditSale.Block1.CardData.Item = objReturnTrackData;
                            }                
                        break;
                #endregion
                        #region HPS_CREDITRETURN
                    case HPS_RETURN:
                        requestMsgKeys.TryGetValue(AMT, out amt);
                        requestMsgKeys.TryGetValue(CARDNBR, out cardNo);
                        requestMsgKeys.TryGetValue(EXPMONTH, out expMonth);
                        requestMsgKeys.TryGetValue(EXPYEAR, out expYear);
                        requestMsgKeys.TryGetValue(ISCARDSWIPE, out manualFlag);
                        requestMsgKeys.TryGetValue(MMSCARD, out cardType);
                        requestMsgKeys.TryGetValue(ALLOWDUP, out sAllowDup);
                        requestMsgKeys.TryGetValue(CARDHOLDERZIP, out sZip);
                        requestMsgKeys.TryGetValue(CARDHOLDERADDR, out sAddr);
                        requestMsgKeys.TryGetValue(CARDHOLDER, out sCardHolName);
                        requestMsgKeys.TryGetValue(TRACKDATA, out trackData);
                        requestMsgKeys.TryGetValue(GATEWAYTXNID, out gateWayTxnID);//PRIMEPOS-2738 ADDED BY ARVIND 
                        PosCreditReturnReqType creditReturn = new PosCreditReturnReqType();
                        reqVer1_0.Transaction.Item = creditReturn;
                        reqVer1_0.Transaction.ItemElementName = ItemChoiceTypePosRequestVer10Transaction.CreditReturn;
                        creditReturn.Block1 = new CreditReturnReqBlock1Type();
                        creditReturn.Block1.Amt = Convert.ToDecimal(amt);
                        response.sHpsTran_Amt = amt.ToString();
                        response.sHPSTran_Type = HPS_RETURN;
                        creditReturn.Block1.CardData = new CardDataType();

                        //PRIMEPOS-2738 ADDED BY ARVIND FOR STRICT RETURN
                        if (gateWayTxnID != null && gateWayTxnID != string.Empty)
                        {
                            creditReturn.Block1.GatewayTxnIdSpecified = true;
                            creditReturn.Block1.GatewayTxnId = Convert.ToInt32(gateWayTxnID);
                        }
                        //

                        if (sAllowDup != null && sAllowDup != string.Empty)
                        {
                            if (sAllowDup.ToUpper() == TRUE)
                            {
                                creditReturn.Block1.AllowDup = booleanType.Y;
                                creditReturn.Block1.AllowDupSpecified = true;
                            }
                            else
                            {
                                creditReturn.Block1.AllowDup = booleanType.N;
                                creditReturn.Block1.AllowDupSpecified = false;
                            }
                        }

                        if (manualFlag.ToString().Trim() == "0")
                        { 
                            CardDataTypeManualEntry objmanualEntry = new CardDataTypeManualEntry();
                            CardHolderDataType objCardHoldata = new CardHolderDataType();
                            creditReturn.Block1.CardData.Item = objmanualEntry;
                            objmanualEntry.CardNbr = cardNo;
                            year = expYear.Substring(2, 2);
                            century = 1900;
                            if (Convert.ToInt32(year) < 80)
                            {
                                century = 2000;
                            }
                            DateTime dExpyear = new DateTime(century + Convert.ToInt32(year), 1, 1);
                            int iEYear = dExpyear.Year;
                            objmanualEntry.ExpMonth = Convert.ToInt32(expMonth.Substring(0, 2).ToString());
                            objmanualEntry.ExpYear = iEYear;
                            objmanualEntry.CardPresent = booleanType.Y;
                            objmanualEntry.CardPresentSpecified = true;
                            objmanualEntry.ReaderPresent = booleanType.Y;
                            objmanualEntry.ReaderPresentSpecified = true;
                            if (sZip != null && sZip != string.Empty)
                            {
                                objCardHoldata.CardHolderZip = sZip;
                                creditReturn.Block1.CardHolderData = objCardHoldata;
                            }
                            if (sAddr != null && sAddr != string.Empty)
                            {
                                objCardHoldata.CardHolderAddr = sAddr;
                                creditReturn.Block1.CardHolderData = objCardHoldata;
                            }
                        }
                        else
                        {
                            CardDataTypeTrackData objReturnTrackData = new CardDataTypeTrackData();
                            objReturnTrackData.Value = trackData.Trim();
                            creditReturn.Block1.CardData.Item = objReturnTrackData;
                        } 
                        break;
                    #endregion
                        #region HPS_CREDITVOID
                    case HPS_VOID:
                        requestMsgKeys.TryGetValue(GATEWAYTXNID, out gateWayTxnID);
                        PosCreditVoidReqType creditVoid = new PosCreditVoidReqType();
                        reqVer1_0.Transaction.Item = creditVoid;
                        reqVer1_0.Transaction.ItemElementName = ItemChoiceTypePosRequestVer10Transaction.CreditVoid;
                        creditVoid.GatewayTxnId = Convert.ToInt32(gateWayTxnID);
                        #region PRIMEPOS-2761
                        requestMsgKeys.TryGetValue(DIRECTMKTINVOICENBR, out sTicketNumber);
                        requestMsgKeys.TryGetValue(StationID, out sStationID);
                        requestMsgKeys.TryGetValue(USERNAME, out sUserID);
                        requestMsgKeys.TryGetValue(AMT, out amt);
                        #endregion
                        break;
                    #endregion
                        #region HPS_CREDITREVERSE
                    case HPS_REVERSE:
                        requestMsgKeys.TryGetValue(AMT, out amt);
                        requestMsgKeys.TryGetValue(CARDNBR, out cardNo);
                        requestMsgKeys.TryGetValue(EXPYEAR, out expYear);
                        requestMsgKeys.TryGetValue(EXPMONTH, out expMonth);
                        requestMsgKeys.TryGetValue(EXPYEAR, out expYear);
                        requestMsgKeys.TryGetValue(GATEWAYTXNID, out gateWayTxnID);
                        requestMsgKeys.TryGetValue(TRACKDATA, out trackData);
                        requestMsgKeys.TryGetValue(ISCARDSWIPE, out manualFlag);
                        requestMsgKeys.TryGetValue(AUTHAMT, out sAuthAmt);
                        requestMsgKeys.TryGetValue(CARDHOLDER, out sCardHolName);
                        PosCreditReversalReqType credirReverse = new PosCreditReversalReqType();
                        reqVer1_0.Transaction.Item = credirReverse;
                        reqVer1_0.Transaction.ItemElementName = ItemChoiceTypePosRequestVer10Transaction.CreditReversal;
                        credirReverse.Block1 = new CreditReversalReqBlock1Type();
                        credirReverse.Block1.CardData = new CardDataType();

                        
                        credirReverse.Block1.Amt = Convert.ToDecimal(amt);
                        response.sHpsTran_Amt = amt;
                        if(sAuthAmt!=null && sAuthAmt!=string.Empty)
                        {
                            credirReverse.Block1.AuthAmt = Convert.ToDecimal(sAuthAmt);
                        }
                        if (gateWayTxnID != null && gateWayTxnID != string.Empty)
                        {
                            credirReverse.Block1.GatewayTxnIdSpecified = true;//PRIMEPOS-2738 ADDED BY ARVIND
                            credirReverse.Block1.GatewayTxnId =Convert.ToInt32(gateWayTxnID);
                        }

                        if (manualFlag.ToString().Trim() == "0")
                        {
                            CardDataTypeManualEntry objManualData = new CardDataTypeManualEntry();
                            credirReverse.Block1.CardData.Item = objManualData;
                            objManualData.CardNbr = cardNo;
                            year = expYear.Substring(2, 2);
                            century = 1900;
                            if (Convert.ToInt32(year) < 80)
                            {
                                century = 2000;
                            }
                            DateTime dTimeExpYear = new DateTime(century + Convert.ToInt32(year), 1, 1);
                            int iExYear = dTimeExpYear.Year;
                            objManualData.ExpMonth = Convert.ToInt32(expMonth.Substring(0, 2).ToString());
                            objManualData.ExpYear = iExYear;
                            objManualData.CardPresent = booleanType.Y;
                            objManualData.CardPresentSpecified = true;
                            objManualData.ReaderPresent = booleanType.Y;
                            objManualData.ReaderPresentSpecified = true;
                        }
                        else
                        {
                            CardDataTypeTrackData objReturnTrackData = new CardDataTypeTrackData();
                            objReturnTrackData.Value = trackData.Trim();
                            credirReverse.Block1.CardData.Item = objReturnTrackData;
                        }
                        
                        
                       
                        
                        break;
                    #endregion
                        #region HPS_DEBITSALE
                    case HPS_DEBITSALE:
                        requestMsgKeys.TryGetValue(TRACKDATA, out trackData);
                        requestMsgKeys.TryGetValue(AMT, out amt);
                        requestMsgKeys.TryGetValue(PINBLOCK, out sPinBlock);
                        requestMsgKeys.TryGetValue(ALLOWDUP, out sAllowDup);
                        PosDebitSaleReqType debitSale = new PosDebitSaleReqType();
                        reqVer1_0.Transaction.Item = debitSale;
                        reqVer1_0.Transaction.ItemElementName = ItemChoiceTypePosRequestVer10Transaction.DebitSale;
                        debitSale.Block1 = new DebitSaleReqBlock1Type();
                        debitSale.Block1.CardHolderData = new CardHolderDataType();
                        debitSale.Block1.Amt = Convert.ToDecimal(amt);
                        response.sHpsTran_Amt = amt;
                        debitSale.Block1.TrackData = trackData;
                        debitSale.Block1.PinBlock = sPinBlock;
                        if (sAllowDup != null && sAllowDup != string.Empty)
                        {
                            if (sAllowDup.ToUpper() == TRUE)
                            {
                                debitSale.Block1.AllowDup = booleanType.Y;
                                debitSale.Block1.AllowDupSpecified = true;
                            }
                            else
                            {
                                debitSale.Block1.AllowDup = booleanType.N;
                                debitSale.Block1.AllowDupSpecified = false;
                            }
                        }
                        debitSale.Block1.AllowPartialAuth = booleanType.N;//booleanType.Y
                        debitSale.Block1.AllowPartialAuthSpecified = false;//true
                        break;
                #endregion
                        #region HPS_DEBITREVERSE
                    case HPS_DEBITREVERSE:
                        requestMsgKeys.TryGetValue(AMT, out amt);
                        requestMsgKeys.TryGetValue(GATEWAYTXNID, out gateWayTxnID);
                        requestMsgKeys.TryGetValue(TRACKDATA, out trackData);
                        PosDebitReversalReqType debitReverse = new PosDebitReversalReqType();
                        reqVer1_0.Transaction.Item = debitReverse;
                        reqVer1_0.Transaction.ItemElementName = ItemChoiceTypePosRequestVer10Transaction.DebitReversal;
                        debitReverse.Block1 = new DebitReversalReqBlock1Type();
                        if (trackData != null && trackData != string.Empty)
                        {
                            debitReverse.Block1.TrackData = trackData;
                        }                        
                        debitReverse.Block1.Amt = Convert.ToDecimal(amt);
                        response.sHpsTran_Amt = amt;
                        if (gateWayTxnID != null && gateWayTxnID != string.Empty)
                        {
                            debitReverse.Block1.GatewayTxnIdSpecified = true;
                            debitReverse.Block1.GatewayTxnId =Convert.ToInt32(gateWayTxnID);
                        }
                        break;
                #endregion
                        #region HPS_DEBITRETURN
                    case HPS_DEBITRETURN:
                        requestMsgKeys.TryGetValue(TRACKDATA, out trackData);
                        requestMsgKeys.TryGetValue(GATEWAYTXNID, out gateWayTxnID);
                        requestMsgKeys.TryGetValue(AMT, out amt);
                        requestMsgKeys.TryGetValue(PINBLOCK, out sPinBlock);
                        requestMsgKeys.TryGetValue(ALLOWDUP, out sAllowDup);
                        PosDebitReturnReqType debitReturn = new PosDebitReturnReqType();
                        reqVer1_0.Transaction.Item = debitReturn;
                        reqVer1_0.Transaction.ItemElementName = ItemChoiceTypePosRequestVer10Transaction.DebitReturn;
                        debitReturn.Block1 = new DebitReturnReqBlock1Type();
                        debitReturn.Block1.CardHolderData = new CardHolderDataType();
                        debitReturn.Block1.Amt = Convert.ToDecimal(amt);
                        response.sHpsTran_Amt = amt;
                        debitReturn.Block1.TrackData = trackData;
                        debitReturn.Block1.PinBlock = sPinBlock;
                        //PRIMEPOS-2738 ADDED FOR REVERSAL
                        if (gateWayTxnID != null && gateWayTxnID != string.Empty)
                        {
                            debitReturn.Block1.GatewayTxnIdSpecified = false;
                            debitReturn.Block1.GatewayTxnId = Convert.ToInt32(gateWayTxnID);
                        }
                        //
                        if (sAllowDup != null && sAllowDup != string.Empty)
                        {
                            if (sAllowDup.ToUpper() == TRUE)
                            {
                                debitReturn.Block1.AllowDup = booleanType.Y;
                                debitReturn.Block1.AllowDupSpecified = true;
                            }
                            else
                            {
                                debitReturn.Block1.AllowDup = booleanType.N;
                                debitReturn.Block1.AllowDupSpecified = false;
                            }
                        }
                        break;
                    #endregion
                    #region HPS_EBTSALE
                    case HPS_EBTSALES:
                        requestMsgKeys.TryGetValue(CARDNBR, out cardNo);
                        requestMsgKeys.TryGetValue(EXPMONTH, out expMonth);
                        requestMsgKeys.TryGetValue(EXPYEAR, out expYear);
                        requestMsgKeys.TryGetValue(TRACKDATA, out trackData);
                        requestMsgKeys.TryGetValue(AMT, out amt);
                        requestMsgKeys.TryGetValue(PINBLOCK, out sPinBlock);
                        requestMsgKeys.TryGetValue(ALLOWDUP, out sAllowDup);
                        requestMsgKeys.TryGetValue(ISCARDSWIPE, out manualFlag);
                        PosEBTFSPurchaseReqType ebtsale = new PosEBTFSPurchaseReqType();
                        reqVer1_0.Transaction.Item = ebtsale;
                        reqVer1_0.Transaction.ItemElementName = ItemChoiceTypePosRequestVer10Transaction.EBTFSPurchase;
                        ebtsale.Block1 = new EBTFSPurchaseReqBlock1Type();
                        ebtsale.Block1.CardHolderData = new CardHolderDataType();
                        ebtsale.Block1.CardData = new CardDataType();
                        ebtsale.Block1.Amt = Convert.ToDecimal(amt);
                        response.sHpsTran_Amt = amt;
                        ebtsale.Block1.PinBlock = sPinBlock.Trim();
                        if (sAllowDup != null && sAllowDup != string.Empty)
                        {
                            if (sAllowDup.ToUpper() == TRUE)
                            {
                                ebtsale.Block1.AllowDup = booleanType.Y;
                                ebtsale.Block1.AllowDupSpecified = true;
                            }
                            else
                            {
                                ebtsale.Block1.AllowDup = booleanType.N;
                                ebtsale.Block1.AllowDupSpecified = false;
                            }
                        }
                        if (manualFlag.Trim() == "0")
                        {
                            CardDataTypeManualEntry ebtmanual = new CardDataTypeManualEntry();
                            ebtsale.Block1.CardData.Item = ebtmanual;
                            ebtmanual.CardNbr = cardNo;
                            year = expYear.Substring(2, 2);
                            if (Convert.ToInt32(year) < 80)
                            {
                                century = 2000;
                            }

                            DateTime dtExpyear = new DateTime(century + Convert.ToInt32(year), 1, 1);
                            int iExpYear = dtExpyear.Year;
                            ebtmanual.ExpMonth = Convert.ToInt32(expMonth.Substring(0, 2).ToString());
                            ebtmanual.ExpYear = iExpYear;
                            ebtmanual.CardPresent = booleanType.Y;
                            ebtmanual.CardPresentSpecified = true;
                            ebtmanual.ReaderPresent = booleanType.Y;
                            ebtmanual.ReaderPresentSpecified = true;
                            CardHolderDataType objCardHolderDdata = new CardHolderDataType();

                            if (sZip != null && sZip != string.Empty)
                            {
                                objCardHolderDdata.CardHolderZip = sZip;
                                ebtsale.Block1.CardHolderData = objCardHolderDdata;
                            }
                            if (sAddr != null && sAddr != string.Empty)
                            {
                                objCardHolderDdata.CardHolderAddr = sAddr;
                                ebtsale.Block1.CardHolderData = objCardHolderDdata;
                            }         

                        }
                        else
                        {
                            CardDataTypeTrackData objReturnTrackData = new CardDataTypeTrackData();
                            objReturnTrackData.Value = trackData.Trim();
                            ebtsale.Block1.CardData.Item = objReturnTrackData;
                        }
                        break;
                    #endregion 
                    #region HPS_EBTRETURN
                    case HPS_EBTRETURN:
                        requestMsgKeys.TryGetValue(CARDNBR, out cardNo);
                        requestMsgKeys.TryGetValue(EXPMONTH, out expMonth);
                        requestMsgKeys.TryGetValue(EXPYEAR, out expYear);
                        requestMsgKeys.TryGetValue(TRACKDATA, out trackData);
                        requestMsgKeys.TryGetValue(AMT, out amt);
                        requestMsgKeys.TryGetValue(PINBLOCK, out sPinBlock);
                        requestMsgKeys.TryGetValue(ALLOWDUP, out sAllowDup);
                        requestMsgKeys.TryGetValue(ISCARDSWIPE, out manualFlag);
                        PosEBTFSReturnReqType ebtreturn = new PosEBTFSReturnReqType();
                        reqVer1_0.Transaction.Item = ebtreturn;
                        reqVer1_0.Transaction.ItemElementName = ItemChoiceTypePosRequestVer10Transaction.EBTFSReturn;
                        ebtreturn.Block1 = new EBTFSReturnReqBlock1Type();
                        ebtreturn.Block1.CardHolderData = new CardHolderDataType();
                        ebtreturn.Block1.CardData = new CardDataType();
                        ebtreturn.Block1.Amt = Convert.ToDecimal(amt);
                        response.sHpsTran_Amt = amt;
                        ebtreturn.Block1.PinBlock = sPinBlock.Trim();
                        if (sAllowDup != null && sAllowDup != string.Empty)
                        {
                            if (sAllowDup.ToUpper() == TRUE)
                            {
                                ebtreturn.Block1.AllowDup = booleanType.Y;
                                ebtreturn.Block1.AllowDupSpecified = true;
                            }
                            else
                            {
                                ebtreturn.Block1.AllowDup = booleanType.N;
                                ebtreturn.Block1.AllowDupSpecified = false;
                            }
                        }
                        if (manualFlag.Trim() == "0")
                        {
                            CardDataTypeManualEntry ebtmanual = new CardDataTypeManualEntry();
                            ebtreturn.Block1.CardData.Item = ebtmanual;
                            ebtmanual.CardNbr = cardNo;
                            year = expYear.Substring(2, 2);
                            if (Convert.ToInt32(year) < 80)
                            {
                                century = 2000;
                            }

                            DateTime dtExpyear = new DateTime(century + Convert.ToInt32(year), 1, 1);
                            int iExpYear = dtExpyear.Year;
                            ebtmanual.ExpMonth = Convert.ToInt32(expMonth.Substring(0, 2).ToString());
                            ebtmanual.ExpYear = iExpYear;
                            ebtmanual.CardPresent = booleanType.Y;
                            ebtmanual.CardPresentSpecified = true;
                            ebtmanual.ReaderPresent = booleanType.Y;
                            ebtmanual.ReaderPresentSpecified = true;
                            CardHolderDataType objCardHolderDdata = new CardHolderDataType();

                            if (sZip != null && sZip != string.Empty)
                            {
                                objCardHolderDdata.CardHolderZip = sZip;
                                ebtreturn.Block1.CardHolderData = objCardHolderDdata;
                            }
                            if (sAddr != null && sAddr != string.Empty)
                            {
                                objCardHolderDdata.CardHolderAddr = sAddr;
                                ebtreturn.Block1.CardHolderData = objCardHolderDdata;
                            }

                        }
                        else
                        {
                            CardDataTypeTrackData objReturnTrackData = new CardDataTypeTrackData();
                            objReturnTrackData.Value = trackData.Trim();
                            ebtreturn.Block1.CardData.Item = objReturnTrackData;
                        }
                        break;
                    #endregion 
                }
                #region PRIMEPOS-2761
                long TransNo = 0;
                using (var db = new Possql())
                {
                    CCTransmission_Log cclog = new CCTransmission_Log();
                    cclog.TransDateTime = DateTime.Now;
                    //cclog.TransAmount = Convert.ToDecimal(amt);

                    cclog.TicketNo = sTicketNumber;
                    //cclog.TransDataStr = _DeviceRequestMessage;
                    cclog.PaymentProcessor = "HPS";
                    cclog.StationID = sStationID;
                    cclog.UserID = sUserID;
                    cclog.TransmissionStatus = "InProgress";
                    cclog.TransType = oPosReq.Item.Transaction.ItemElementName.ToString();


                    db.CCTransmission_Logs.Add(cclog);
                    db.SaveChanges();
                    db.Entry(cclog).GetDatabaseValues();
                    TransNo = cclog.TransNo;
                }
                #endregion
                logger.Trace("Data to send to HPS" + oPosReq.Item.Transaction.Item);
                oPosResp=  this.Process(client, oPosReq); //sending Data to HPS gateway
                try
                {
                    string requestString = Helper.SerializeRequest(oPosReq);
                    string responseString = Helper.SerializeResponse(oPosResp);
                    logger.Debug("Request: " + oPosReq);
                    logger.Debug("Response: " + oPosResp);
                    //#region COmmented  PRIMEPOS-2761
                    //using (var db = new Possql())
                    //{
                    //    CCTransmission_Log cclog = new CCTransmission_Log();
                    //    cclog.TransDateTime = DateTime.Now;
                    //    cclog.TransAmount = Convert.ToDecimal(amt);
                    //    cclog.TransDataStr = requestString;
                    //    cclog.RecDataStr = responseString;
                    //    db.CCTransmission_Logs.Add(cclog);
                    //    db.SaveChanges();
                    //}
                    //#endregion
                    #region PRIMEPOS-2761
                    long OrgTransNo = 0;
                    using (var db = new Possql())
                    {
                        CCTransmission_Log cclog1 = new CCTransmission_Log();

                        if (oPosReq.Item.Transaction.ItemElementName.ToString() != null)
                        {
                            if (oPosReq.Item.Transaction.ItemElementName.ToString().ToUpper().Contains("VOID"))
                            {
                                cclog1 = db.CCTransmission_Logs.Where(w => w.HostTransID == gateWayTxnID).SingleOrDefault();
                                cclog1.IsReversed = true;
                                OrgTransNo = cclog1.TransNo;
                                db.CCTransmission_Logs.Attach(cclog1);
                                db.Entry(cclog1).Property(p => p.IsReversed).IsModified = true;
                                db.SaveChanges();
                            }
                        }
                    }
                    using (var db = new Possql())
                    {
                        CCTransmission_Log cclog = new CCTransmission_Log();
                        cclog = db.CCTransmission_Logs.Where(w => w.TransNo == TransNo).SingleOrDefault();
                        cclog.TransDataStr = requestString;
                        cclog.RecDataStr = responseString;
                        if (amt != "")
                        {
                            cclog.TransAmount = Convert.ToDecimal(amt);
                        }
                        else
                        {
                            cclog.TransAmount = 0;
                        }
                        cclog.TransmissionStatus = "Completed";
                        cclog.HostTransID = oPosResp.Item.Header.GatewayTxnId.ToString();
                        cclog.OrgTransNo = OrgTransNo;
                        cclog.ResponseMessage = oPosResp.Item.Header.GatewayRspMsg;
                        db.CCTransmission_Logs.Attach(cclog);
                        db.Entry(cclog).Property(p => p.TransDataStr).IsModified = true;
                        db.Entry(cclog).Property(p => p.RecDataStr).IsModified = true;
                        db.Entry(cclog).Property(p => p.TransAmount).IsModified = true;
                        db.Entry(cclog).Property(p => p.TransmissionStatus).IsModified = true;
                        db.Entry(cclog).Property(p => p.HostTransID).IsModified = true;
                        db.Entry(cclog).Property(p => p.OrgTransNo).IsModified = true;
                        db.Entry(cclog).Property(p => p.ResponseMessage).IsModified = true;
                        db.SaveChanges();

                    }
                    #endregion
                }
                catch (Exception exp)
                {
                    logger.Trace(exp.ToString());

                }

                if (oPosResp != null)
                {
                    logger.Trace("GatewaytxnID:- " + oPosResp.Item.Header.GatewayTxnId + "\r\n" + "RespMsg:- " + oPosResp.Item.Header.GatewayRspMsg + "\r\n" + "RespCode:- " + oPosResp.Item.Header.GatewayRspCode+"\n");
                }
                objPaymentRes.objPosResponse = oPosResp;
                response.objPosResponse = oPosResp;
                response.ParseResponse("", ""); // send result to get parse
                if (transactionType != "HPS_VOID")// PRIMEPOS-2761 - Added if (transactionType != "HPS_VOID")
                {
                    if (trackData != null && trackData != string.Empty) //Getting the last 4 of card #
                    {
                        string cardTrackData = trackData.ToString().Replace('%', ' ').Replace('B', ' ').Trim();
                        char[] cardsplit = { '^' };
                        string[] CardNoData = cardTrackData.Split(cardsplit);
                        response.AccountNo = CardNoData[0].ToString();
                        logger.Trace("Track CardNo: " + CardNoData[0].ToString().Substring(CardNoData[0].ToString().Length - 4).PadLeft(CardNoData[0].ToString().Length, '*'));
                    }
                    else
                    {
                        response.AccountNo = cardNo;
                        logger.Trace("CardNo: " + cardNo.Substring(cardNo.Length - 4).PadLeft(cardNo.Length, '*'));
                    }
                }
                #region PRIMEPOS-2761 
                response.ticketNum = sTicketNumber;
                #endregion
                logger.Trace("End Processor (HPS ProcessTxn) ----" + transactionType);
                return response;
            }
            return null;
        }

        /// <summary>
        /// Author : Manoj Kumar
        /// Desciption : Clear the fields
        /// External functions: PaymentResponse
        /// </summary>
        private void ClearFields()
        {
            logger.Trace("In ClearFields()");
            if (response != null)
                response.ClearFields();
            errorMessage = String.Empty;
            txnType = String.Empty;
        }

        /// <summary>
        /// Author : Ritesh - Copy from Xlink by Manoj
        /// Desciption : Check the Valid Fields for the Processor.
        /// </summary>
        /// <returns>bool vaild</returns> 
        private bool IsValidFields()
        {
            logger.Trace("HPS IsValidFields()-- Start to check for valid fields");
            foreach (KeyValuePair<String, String> kvp in MessageFields)
            {
                if (!ValidKeys.ContainsKey(kvp.Key))
                {
                    errorMessage += "INVALID FIELD:-" + kvp.Key;
                    logger.Error("***********HPS IsValidFields()-- Invalid Field: " + errorMessage.ToString());
                    return false;
                }
            }
            logger.Trace("HPS IsValidFields()-- End checking for valid fields");
            return true;
        }

        /// <summary>
        /// Author : Manoj Kumar - copy from Xlink
        /// Desciption : This method is called to validate the Valid Fields and Mandatory fields.
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
        }

        /// <summary>
        /// Author : Ritesh- Copy from Xlink by Manoj
        /// Functionality Desciption : This method is used for checking the Mandatory Fields for a Transaction type.
        /// External functions:None
        /// Known Bugs : None
        /// Start Date : 31 JULY 2010.
        /// </summary>
        /// <returns>bool vaild</returns> 
        private bool IsMandatoryFields()
        {
            //compare all the fields agains list of mandatory fields "MandatoryKeys" if fine 
            logger.Trace("HPS IsMandatoryFields()-- Start to check for Mandatory Fields");
            MMSDictionary<String, String> keys = null;
            if (!MandatoryKeys.ContainsKey(txnType))
            {
                if (FetchMandatoryFields(txnType) == 0)
                {
                    errorMessage += "INVALID TRANSACTION TYPE:-" + txnType;
                    logger.Error("***********HPS IsMandatoryFields()-- Invalid: " + errorMessage.ToString());
                    return false;
                }
            }
            MandatoryKeys.TryGetValue(txnType, out keys);
            foreach (KeyValuePair<String, String> kvp in keys)
            {
                if (!MessageFields.ContainsKey(kvp.Key))
                {
                    errorMessage += "MISSING MANDATORY FIELD:-" + kvp.Key;
                    logger.Error("***********HPS IsMandatoryFields()-- Invalid: " + errorMessage.ToString());
                    return false;
                }
            }
            logger.Trace("HPS IsMandatoryFields()-- End checking Mandatory Fields");
            keys = null;
            return true;
        }

        /// <summary>
        /// Author : Ritesh- Copy from Xlink by Manoj
        /// Functionality Desciption : This method is used for fetching the mandatory fields of transaction from MMSDictionary or MandaotoryFields.xml file.
        /// External functions:MMSDictionary,XmlToKeys
        /// Known Bugs : None
        /// Start Date : 31 JULY 2010.
        /// </summary>
        /// <param name="transactionType">String</param>
        /// <returns>int vaild</returns>
        private int FetchMandatoryFields(String transactionType)
        {
            int count = 0;
            String strXmlKey =  transactionType.Trim();
            MMSDictionary<String, String> keys = new MMSDictionary<String, String>();
            count = XmlToKeys.GetFields(MANDATORY_FIELDS, strXmlKey, ref keys, true);
            if (count > 0)
                MandatoryKeys.Add(transactionType, keys);
            return count;
        }

        /// <summary>
        /// Author : Ritesh - Copy from Xlink by Manoj
        /// Functionality Desciption : This method is used for ensuring valid parameters are passed (Null check).
        /// External functions:MMSDictionary,XmlToKeys
        /// Known Bugs : None
        /// Start Date : 31 JULY 2010.
        /// </summary>
        /// <param name="keys">MMSDictioanary</param>
        /// <returns>bool vaild</returns>
        public bool ValidateParameters(ref MMSDictionary<String, String> keys)
        {

            bool flag = true;
            bool isValid = true;
            string sError = string.Empty;
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
                isValid = this.GetProcessorTag(kvp.Key, HPS, out orgParam);
                if (isValid == true)
                {
                    orignalKeys.Add(orgParam, kvp.Value);
                }
            }
            keys = orignalKeys;

            return flag;

        }

        /// <summary>
        /// Author : Ritesh - Copy from Xlink by Manoj
        /// Functionality Desciption : This method is used to get node value from file.
        /// External functions:XmlToKeys
        /// Known Bugs : None
        /// Start Date : 31 JULY 2010.
        /// </summary>
        /// <param name="keys">MMSDictioanary</param>
        /// <returns>bool vaild</returns>
        public bool GetProcessorTag(string xmlCommonNode, string xmlProcessorName, out string ProcessorTag)
        {
            bool isValid = true;
            string tagValue = string.Empty;

            XmlNodeList xmlList;

            xmlList = xmlxCharge.GetElementsByTagName(xmlCommonNode);
            //xmlList = xmlDoc.SelectNodes(xmlParentNode);

            
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
        

        #region IDisposable Members

        public void Dispose()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion
    }

    public class HpsFileChecker
    {
        private ILogger logger = LogManager.GetCurrentClassLogger();
        #region HPS Sftp
        Sftp sftpClient = null;
        string[] files = null;
        string[] fileDown = null;
        string hostName = string.Empty;
        string username = string.Empty;
        string password = string.Empty;
        int port = 0;
        #endregion
        /// <summary>
        /// Constructor take 4 params,
        /// </summary>
        /// <param name="Host"></param>
        /// <param name="Port"></param>
        /// <param name="User"></param>
        /// <param name="Pass"></param>
        public HpsFileChecker(string Host, int Port, string User, string Pass)
        {
            this.hostName = Host;
            this.port = Port;
            this.username = User;
            this.password = Pass;
        }

        public HpsFileChecker()
        {
        }
        #region HPS Sftp and Download
        /// <summary>
        /// If Processor is HPS check if the FSA files are current.
        /// </summary>
        public bool HPSFileCheck()
        {
            try
            {
                logger.Trace("Start checking HPS sftp for FSA file update");
                files = new string[] { "VisaHealthcareBins.xml", "MCHealthcareBins.xml" };
                if(HpsFileChecks())
                {
                    sftpClient = HpsSftpConnect(); //Connect to HPS Sftp
                    if(sftpClient != null) //is connected
                    {
                        if(fileDown != null)
                        {
                            foreach(var fileN in fileDown)
                            {
                                if(fileN != null)
                                {
                                    HpsFileDownload(sftpClient, fileN); //DownLoad file
                                }
                            }
                        }
                    }
                }
                else
                {
                    sftpClient = HpsSftpConnect(); //Connect to HPS sftp
                    if(sftpClient != null)
                    {
                        foreach(var filename in files)
                        {
                            if(filename != null)
                            {
                                HpsFileCompare(sftpClient, filename); //Compare the files
                            }
                        }
                    }
                }
                logger.Trace("Finishing checking HPS sftp for FSA files for update.");
                return true;
            }
            catch(Exception ex)
            {
                logger.Error(ex,"HPSFileCheck failed. Error: " + ex.Message);
                return false;
            }
            finally
            {
                if(sftpClient != null)
                {
                    sftpClient.Disconnect(); //disconect from HPS sftp
                    sftpClient.Dispose();
                    sftpClient = null;
                }
            }
        }

        /// <summary>
        /// Connect to the HPS sftp.
        /// </summary>
        /// <returns></returns>
        private Sftp HpsSftpConnect()
        {
            sftpClient = null;
            try
            {
                logger.Trace("Connecting to Hps Sftp");
                sftpClient = new Sftp();
                //Connect to Hps sftp
                sftpClient.Connect(this.hostName, this.port);
                //Log on 
                sftpClient.Login(this.username, this.password);
                logger.Trace("Successfully connected.");
            }
            catch(SftpException ex1)
            {
                logger.Error(ex1,"****HPSsftpConnect()..Error1: " + ex1.Message);
                throw new SftpException(ex1.ToString());
            }
            catch(Exception ex2)
            {
                logger.Error(ex2,"****HPSsftpConnect()..Error2: " + ex2.Message);
                throw new Exception(ex2.ToString());
            }
            return sftpClient;
        }

        /// <summary>
        /// Compare the HPS files and the one in the POS folder 
        /// Compare Modify date and File Size
        /// </summary>
        /// <returns></returns>
        private bool HpsFileCompare(Sftp Conn, string filename)
        {
            bool isvalid = false;
            try
            {
                logger.Trace("Start comparing file to get update");
                string LocalFilePath = AppDomain.CurrentDomain.BaseDirectory + filename; //file path
                var fLength = new FileInfo(LocalFilePath).Length; //size of file 

                if(fLength != Conn.GetFileLength(filename) || File.GetLastWriteTime(filename) != sftpClient.GetFileDateTime(filename))
                {
                    HpsFileDownload(Conn, filename); // download the file
                    isvalid = true;
                }
                logger.Trace("Finish comparing file");
            }
            catch(Exception ex)
            {
                logger.Error(ex,"HpsFile Compare() Error: " + ex.Message);
                throw new Exception(ex.ToString());
            }
            return isvalid;
        }

        /// <summary>
        /// Check if the HPS FSA files exist.
        /// </summary>
        /// <returns></returns>
        private bool HpsFileChecks()
        {
            bool notExist = false;
            try
            {
                logger.Trace("Start checking for files");
                string LocalFolder = AppDomain.CurrentDomain.BaseDirectory; //path where the file is located
                fileDown = new string[2]; //array of the 2 files
                for(int i = 0; i < files.Length; i++)
                {
                    if(!File.Exists(Path.Combine(LocalFolder, files[i].ToString())))
                    {
                        if(files[i] == files[0])
                        {
                            fileDown[i] = files[i].ToString(); //file to download
                        }
                        else
                        {
                            fileDown[i] = files[i].ToString(); //file to download
                        }
                        notExist = true;
                    }
                }
                logger.Trace("Finish checking for files");
            }
            catch(Exception ex)
            {
                logger.Error(ex,"****HpsFileCheck failed. Error: " + ex.Message);
                throw new Exception(ex.ToString());
            }
            return notExist;
        }

        /// <summary>
        /// Download the file if size or modify date is different.
        /// </summary>
        /// <param name="Conn"></param>
        /// <param name="filename"></param>
        /// <returns></returns>
        private bool HpsFileDownload(Sftp Conn, string filename)
        {
            bool isdown = false;
            try
            {
                logger.Trace("Start downloading of file: :" + filename);
                string LocalFolder = AppDomain.CurrentDomain.BaseDirectory; //location of file
                if(Conn != null)
                {
                    DateTime remoteTime = Conn.GetFileDateTime(filename); //get the date modification on the file
                    Conn.GetFiles(filename, LocalFolder, SftpBatchTransferOptions.Default, SftpActionOnExistingFiles.OverwriteAll); //DownLoad
                    File.SetLastWriteTime(LocalFolder + filename, remoteTime); //Write the date modify from the sftp to the downloaded file
                    isdown = true;
                    logger.Trace("Finish downloading File: " + filename);
                }
                else
                {
                    logger.Trace("****File downLoad fail. Error: sftp cannot connect");
                    throw new Exception("Cannot download files");
                }
            }
            catch(Exception ex)
            {
                logger.Error("****HPSFileDownload Error: " + ex.Message);
                throw new Exception(ex.ToString());
            }
            return isdown;
        }
        #endregion
    }
}
