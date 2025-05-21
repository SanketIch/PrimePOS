using FirstMile;
using MMS.PAYMENT;
using MMS.PROCESSOR;
using NLog;
using POS_Core.LabelHandler.RxLabel;
using POS_Core.Resources.DelegateHandler;
using POS_Core.Resources.PaymentHandler;
using PossqlData;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;

namespace POS_Core.Resources
{

    public class PccPaymentSvr : Form
    {
        public bool IsElavonEbtvoid = false;//2943
        private MMS.Device.WPDevice.WPData.WPAccountInfo WPAccount;
        private MMS.Device.WPDevice.WPData.Payment Payment;
        private static PccPaymentSvr oDefObj;
        private const int FAILURE = 1;
        private const int SUCCESS = 0;
        #region 'Payment Prosessing Reqest Constants'
        private const string ACCOUNTNUM = "ACCOUNTNUM";
        private const string EXPDATE = "EXPDATE";
        private const string PREREADID = "PREREADID"; //PRIMEPOS-3372
        private const string ISNBSTRANSACTION = "ISNBSTRANSACTION"; //PRIMEPOS-3526 //PRIMEPOS-3504
        private const string PREREADTYPE = "PREREADTYPE"; //PRIMEPOS-3407
        private const string NBSSALETYPE = "NBSSALETYPE"; //PRIMEPOS-3372
        private const string TRACK = "TRACK";
        private const string AMOUNT = "AMOUNT";
        private const string TOTALAMOUNT = "TOTALAMOUNT";
        private const string CARDHOLDER = "CARDHOLDER";
        private const string ADDRESS = "ADDRESS";
        private const string TICKETNO = "TICKETNO";
        private const string ZIP = "ZIP";
        private const string MANUALFLAG = "MANUALFLAG";
        private const string TIMEOUT = "TIMEOUT";
        private const string MMSCARD = "MMSCARD";
        private const string DEBITPINNO = "DEBITPINNO";
        private const string DEBITKEYNO = "DEBITKEYNO";
        private const string PASSWORD = "PASSWORD";
        private const string TERMINALID = "TERMINALID";//PrimePOS-2491 
        private const string EXPMONTH = "EXPMONTH";
        private const string EXPYEAR = "EXPYEAR";
        private const string VERSION = "VERSION";
        private const string DIRECTMKTSHIPMONTH = "DIRECTMKTSHIPMONTH";
        private const string DIRECTMKTSHIPDAY = "DIRECTMKTSHIPDAY";
        private const string ALLOWDUP = "ALLOWDUP";
        private const string CARDPRESENT = "CARDPRESENT";
        private const string CASHBACK = "CASHBACKAMT";
        //Added By SRT(Gaurav) Date: 01-Dec-2008
        //Mantis Id: 0000136
        private const string TROUTID = "TRANSID";
        //End Of Added By SRT(Gaurav)
        private const string TROUTD = "TROUTD";
        //ADDED BY ROHIT NAIR FOR XCHARGE TOKENISATION
        private const string TOKEN = "TOKEN";

        #endregion
        #region 'Payment Processing Response Constants'
        private const string RESULT = "RESULT";
        private const string AUTHORIZATION = "AUTHORIZATION";
        private const string TRANSID = "TRANSID";
        private const string APPROVALCODE = "APPROVALCODE";
        private const string RESULTDESCRIPTION = "RESULTDESCRIPTION";
        #endregion
        private MMS.PROCESSOR.MMSDictionary<String, String> cardParms;
        //Added By SRT(Ritesh Parekh) Date: 23-Jul-2009
        //Maintains the processor instances.
        public static MMS.PROCESSOR.MMSDictionary<String, PccPaymentSvr> DefaultInstances;
        //End Of Added By SRT(Ritesh Parekh)
        private PaymentServer server;
        private ICreditProcessor creditProc;
        private IDebitProcessor debitProc;
        private IEbtProcessor ebtProc;
        private INBSProcessor nbsProc; //PRIMEPOS-3372
        private PaymentResponse resp = null;
        private string txnTimeOut = string.Empty;
        string responseError = string.Empty;
        private string responseStatus = string.Empty;
        private PccResponse pccResponse;
        private string PAYMENTPROCESSOR = string.Empty;
        private string isAVSModeOn = string.Empty;
        private string logValue = string.Empty; // Added By Dharmendra (SRT) on Dec-08-08
        private String loggerString = String.Empty;
        private String sUserName = string.Empty;
        private String sPassword = string.Empty;
        private string sTerminalID = string.Empty; //PrimePOS-2491 
        private String sALLOWDUP = string.Empty;
        public string Balance = string.Empty; // Added by Manoj 8/24/2011  This is for Xcharge Balance 
        public static string CCBalance = string.Empty;
        private PrimePosLogWriter primePosLogWriter;
        private enum Transactions { CreditSale, VoidOnCreditCardSales, VoidOnCreditCardSalesReturn, CreditSalesReturn, DebitSale, EBTSale, EBTReturn, ReturnOnDebitCardSales, VoidOnDebitCardSales, VoidOnDebitCardSalesReturn };
        private static ILogger logger = LogManager.GetCurrentClassLogger();
        //Added By Dharmendra on 28-08-08
        private const string HREFNUMBER = "HREFNUMBER";//Arvind PRIMEPOS-2738 
        private const string ORIGINALTRANSID = "ORIGINALTRANSID";//Arvind PRIMEPOS-2738 
        #region PRIMEPOS-2761 
        public static string sCurrentTicket = string.Empty;
        #endregion
        #region PRIMEPOS-2841
        public static string sPharmacyNumber = string.Empty;
        #endregion
        public static PccPaymentSvr DefaultInstance
        {
            get
            {
                if (oDefObj == null)
                {
                    oDefObj = new PccPaymentSvr();
                }
                return oDefObj;
            }
        }

        //change name of this method to get processor instance
        /// <summary>
        /// Author : Ritesh Parekh
        /// Date : 23-Jul-2009
        /// This method returns the instance of pccpaymentsvr for requested server.
        /// </summary>
        /// <param name="ProcessorName"></param>
        /// <returns></returns>
        public static PccPaymentSvr GetProcessorInstance(string ProcessorName)
        {
            PccPaymentSvr objPaySvr = null;
            objPaySvr = DefaultInstances[ProcessorName];
            return (objPaySvr);
        }

        #region PRIMEPOS-2841
        public static PccPaymentSvr GetOnlinePaymentInstance(string ProcessorName)
        {
            PccPaymentSvr objPaySvr = null;
            objPaySvr = DefaultInstances[ProcessorName];
            return (objPaySvr);
        }
        #endregion

        /// <summary>
        /// Author : Ritesh Parekh
        /// Date : 23-Jul-2009
        /// This method adds current instance of perticular processor in maintained default instances,
        /// if not exist.
        /// </summary>
        /// <param name="objSvr"></param>
        /// <param name="ServerName"></param>
        /// <returns></returns>
        private static bool AddToProcessorInstances(PccPaymentSvr objSvr, string ServerName)
        {
            bool result = false;
            if (DefaultInstances == null)
            {
                DefaultInstances = new MMSDictionary<string, PccPaymentSvr>();
            }
            if (!DefaultInstances.ContainsKey(ServerName) && objSvr != null)
            {
                DefaultInstances.Add(ServerName, objSvr);
                result = true;
            }
            return result;
        }
        public string ProfiledID
        {
            get;
            set;
        }
        public string CashBack
        {
            get;
            set;
        }
        public string EntryMethod
        {
            get;
            set;
        }
        public MMS.PROCESSOR.EmvReceiptTags EmvTags
        {
            get;
            set;
        }

        public PccResponse pccRespInfo
        {
            get
            {
                return pccResponse;
            }
            set
            {
                pccResponse = value;
            }
        }
        public string IsAVSModeOn
        {
            get
            {
                return isAVSModeOn;
            }
        }

        public WPResponse WpRespInfo
        {
            get; set;
        }

        public string WP_SigString
        {
            get; set;
        }

        public string PAX_SigString
        {
            get; set;
        }// Added By Suraj
        public string EVERTEC_SigString
        {
            get; set;
        }//// Added for Evertec - PRIMEPOS-2664 Arvind
        public string VANTIV_SigString
        {
            get; set;
        }//PRIMEPOS-2636 VANTIV
        public string ELAVON_SigString
        {
            get; set;
        }//PRIMEPOS-2943

        public TransactionResult WP_TransResult
        {
            get; set;
        }
        //End Added
        public PccPaymentSvr()
        {
            cardParms = new MMS.PROCESSOR.MMSDictionary<String, String>();
            //Modified By Dharmendra On Oct-20-08 reading Payment Processor Name,Avs Mode,Transaction Timeout from POSSET instead of app.config
            PAYMENTPROCESSOR = Configuration.CPOSSet.PaymentProcessor;
            isAVSModeOn = Configuration.CPOSSet.AvsMode; // Modified & Commented By Dharmendra on Oct-20-08 System.Configuration.ConfigurationManager.AppSettings["AVSMODE"];
            server = new PaymentServer(PAYMENTPROCESSOR);

            creditProc = server.GetCreditProcessor();
            debitProc = server.GetDebitProcessor();
            ebtProc = server.GetEBTProcessor(); // Added by Manoj 8/26/2011
            nbsProc = server.GetNBSProcessor(); //PRIMEPOS-3372
            txnTimeOut = Configuration.CPOSSet.TxnTimeOut;

            logValue = System.Configuration.ConfigurationManager.AppSettings["ENABLETRACE"].ToString().Trim();
            primePosLogWriter = new PrimePosLogWriter(logValue);
            //Added By Dharmendra (SRT) on Dec-08-08 to write the log of parameters which are passed to the payment processor
            //loggerString = "-------------------Payment Transaction Logging Details----------------------\r\n";
            //loggerString += "Payment Processor Name = " + PAYMENTPROCESSOR+"\r\n";
            primePosLogWriter.AppendCommentsToLogger(loggerString);
            //End Added

            //Added By SRT(Ritesh Parekh) Date: 23-Jul-2009
            //from here adding current instance of the processor in list of default instances
            AddToProcessorInstances(this, PAYMENTPROCESSOR);
            //End Of Added By SRT(Ritesh Parekh)
            if (PAYMENTPROCESSOR.ToUpper() == "WORLDPAY")
            {
                if (string.IsNullOrEmpty(GatewaySettings.AccountId))
                {
                    GatewaySettings.AccountId = Configuration.CPOSSet.HPS_USERNAME;
                }
                if (string.IsNullOrEmpty(GatewaySettings.MerchantPin))
                {
                    GatewaySettings.MerchantPin = Configuration.CPOSSet.HPS_PASSWORD;
                }

                if (string.IsNullOrEmpty(GatewaySettings.SubId))
                {
                    GatewaySettings.SubId = Configuration.CPOSSet.WP_SubID;
                }
                /*string[] MerchantSub = Configuration.CPOSSet.HPS_PASSWORD.Split('/');
                WPAccount.AccountID = Configuration.CPOSSet.HPS_USERNAME;
                if (MerchantSub.Length > 1)
                {
                    WPAccount.MerchantPIN = MerchantSub[0];
                    WPAccount.SubID = MerchantSub[1];
                }
                else if (MerchantSub.Length == 1)
                {
                    WPAccount.MerchantPIN = MerchantSub[0];
                }*/
            }
            else
            {
                sUserName = Configuration.CPOSSet.HPS_USERNAME;
                sPassword = Configuration.CPOSSet.HPS_PASSWORD;
                sALLOWDUP = Configuration.CPOSSet.ALLOWDUP.ToString();
                sTerminalID = Configuration.CPOSSet.TerminalID; //PrimePOS-2491 
            }
        }

        #region PRIMEPOS-2841
        public PccPaymentSvr(string PaymentProcessor)
        {
            cardParms = new MMS.PROCESSOR.MMSDictionary<String, String>();
            //Modified By Dharmendra On Oct-20-08 reading Payment Processor Name,Avs Mode,Transaction Timeout from POSSET instead of app.config
            PAYMENTPROCESSOR = PaymentProcessor;
            isAVSModeOn = Configuration.CPOSSet.AvsMode; // Modified & Commented By Dharmendra on Oct-20-08 System.Configuration.ConfigurationManager.AppSettings["AVSMODE"];
            server = new PaymentServer(PAYMENTPROCESSOR);

            creditProc = server.GetCreditProcessor();
            debitProc = server.GetDebitProcessor();
            ebtProc = server.GetEBTProcessor(); // Added by Manoj 8/26/2011
            txnTimeOut = Configuration.CPOSSet.TxnTimeOut;

            logValue = System.Configuration.ConfigurationManager.AppSettings["ENABLETRACE"].ToString().Trim();
            primePosLogWriter = new PrimePosLogWriter(logValue);
            //Added By Dharmendra (SRT) on Dec-08-08 to write the log of parameters which are passed to the payment processor
            //loggerString = "-------------------Payment Transaction Logging Details----------------------\r\n";
            //loggerString += "Payment Processor Name = " + PAYMENTPROCESSOR+"\r\n";
            primePosLogWriter.AppendCommentsToLogger(loggerString);
            //End Added

            //Added By SRT(Ritesh Parekh) Date: 23-Jul-2009
            //from here adding current instance of the processor in list of default instances
            AddToProcessorInstances(this, PAYMENTPROCESSOR);
            //End Of Added By SRT(Ritesh Parekh)
            if (PAYMENTPROCESSOR.ToUpper() == "WORLDPAY")
            {
                if (string.IsNullOrEmpty(GatewaySettings.AccountId))
                {
                    GatewaySettings.AccountId = Configuration.CPOSSet.HPS_USERNAME;
                }
                if (string.IsNullOrEmpty(GatewaySettings.MerchantPin))
                {
                    GatewaySettings.MerchantPin = Configuration.CPOSSet.HPS_PASSWORD;
                }

                if (string.IsNullOrEmpty(GatewaySettings.SubId))
                {
                    GatewaySettings.SubId = Configuration.CPOSSet.WP_SubID;
                }
                /*string[] MerchantSub = Configuration.CPOSSet.HPS_PASSWORD.Split('/');
                WPAccount.AccountID = Configuration.CPOSSet.HPS_USERNAME;
                if (MerchantSub.Length > 1)
                {
                    WPAccount.MerchantPIN = MerchantSub[0];
                    WPAccount.SubID = MerchantSub[1];
                }
                else if (MerchantSub.Length == 1)
                {
                    WPAccount.MerchantPIN = MerchantSub[0];
                }*/
            }
            else
            {
                sUserName = Configuration.CPOSSet.HPS_USERNAME;
                sPassword = Configuration.CPOSSet.HPS_PASSWORD;
                sALLOWDUP = Configuration.CPOSSet.ALLOWDUP.ToString();
                sTerminalID = Configuration.CPOSSet.TerminalID; //PrimePOS-2491 
            }

            POS_Core.BusinessRules.POSTransaction pOSTransaction = new BusinessRules.POSTransaction();
            System.Data.DataSet dsPharmacyNPI = pOSTransaction.GetPharmacyNPI();

            sPharmacyNumber = dsPharmacyNPI.Tables[0].Rows[0][0].ToString();
        }
        #endregion
        /// <summary>
        /// 
        /// </summary>
        public static void CloseDefaultInstace()
        {
            if (oDefObj != null)
            {
                oDefObj.DisposeObjects();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        private void DisposeObjects()
        {
            creditProc = null;
            debitProc = null;
            resp = null;
            oDefObj = null;
        }

        //ADDED PRASHANT 5 JUN 2010

        private void IsSecurityPromptRequired(Transactions transactions, MMSDictionary<string, string> cardParms)
        {
            if (Configuration.CPOSSet.PromptForAllTrans == true)
            {
                cardParms.Add("PROMPT", "PROMPT");
            }
            else
            {
                switch (transactions)
                {
                    case Transactions.CreditSalesReturn:
                    case Transactions.VoidOnCreditCardSalesReturn:
                    case Transactions.VoidOnDebitCardSalesReturn:
                        if (Configuration.CPOSSet.PromptForReturnTrans == true)
                        {
                            cardParms.Add("PROMPT", "PROMPT");
                        }
                        break;
                }
            }
        }

        //END ADDED PRASHANT 5 JUN 2010

        #region PRIMEPOS-3526
        public bool PerformPreRead(string ticketNum, ref PccCardInfo cardInfo) //PRIMEPOS-3504
        {
            logger.Trace("public bool PerformPreRead(string ticketNum, ref PccCardInfo cardInfo) - Entered");
            bool isSale = false;

            CultureInfo ci = CultureInfo.CurrentCulture;

            Double tempAmount = !string.IsNullOrEmpty(cardInfo.transAmount) ? Convert.ToDouble(cardInfo.transAmount) : Convert.ToDouble("0.00");
            Double tempFsaAmount = !string.IsNullOrEmpty(cardInfo.transFSAAmount) ? Convert.ToDouble(cardInfo.transFSAAmount) : Convert.ToDouble("0.00");
            Double tempFsaRxAmount = !string.IsNullOrEmpty(cardInfo.transFSARxAmount) ? Convert.ToDouble(cardInfo.transFSARxAmount) : Convert.ToDouble("0.00");

            IsSecurityPromptRequired(Transactions.CreditSale, cardParms);

            cardParms.Add(ACCOUNTNUM, cardInfo.cardNumber);
            cardParms.Add(EXPDATE, cardInfo.cardExpiryDate);
            cardParms.Add(ZIP, cardInfo.zipCode);
            cardParms.Add(ADDRESS, cardInfo.customerAddress);
            cardParms.Add(AMOUNT, tempAmount.ToString("F", ci).PadRight(2, '0'));
            cardParms.Add(CARDHOLDER, cardInfo.cardHolderName);
            cardParms.Add(TICKETNO, ticketNum);
            cardParms.Add(TIMEOUT, cardInfo.txnTimeOut);
            cardParms.Add(MMSCARD, cardInfo.cardType);
            cardParms.Add(PASSWORD, sPassword);
            cardParms.Add(ALLOWDUP, sALLOWDUP);
            cardParms.Add(TERMINALID, sTerminalID);

            if (cardInfo.UseToken)
            {
                cardParms.Add("XCACCOUNTID", cardInfo.ProfileID);
            }

            AddUserID(cardParms);

            if (cardInfo.trackII.Equals(string.Empty))
            {
                cardParms.Add(MANUALFLAG, "0");
                if (cardInfo.IsCardPresent != string.Empty)
                {
                    cardParms.Add(CARDPRESENT, cardInfo.IsCardPresent);
                }
            }
            else
            {
                cardParms.Add(TRACK, cardInfo.trackII);
                cardParms.Add(MANUALFLAG, "1");
            }

            if ((cardInfo.IsFSATransaction != "0") && tempFsaAmount != 0)
            {
                cardParms.Add("FSATRANSACTION", cardInfo.IsFSATransaction);
                cardParms.Add("FSAAMOUNT", tempFsaAmount.ToString("F", ci).PadRight(2, '0'));
                if (tempFsaRxAmount != 0)
                {
                    cardParms.Add("FSARXAMOUNT", tempFsaRxAmount.ToString("F", ci).PadRight(2, '0'));
                }
            }

            #region PRIMEPOS-3517 Currently only VANTIV is implemented for PREREAD
            if (PAYMENTPROCESSOR == "HPSPAX")
            {
                cardParms.Add("HOSTADDR", Configuration.CPOSSet.SigPadHostAddr);
                cardParms.Add("TOKENREQUEST", (cardInfo.Tokenize ? true : false).ToString());
                cardParms.Add("TransactionRecover", cardInfo.isTransactionRecover.ToString());
                cardParms.Add("ISFSACARD", cardInfo.IsFsaCard.ToString());
                cardParms.Add("SITEID", Configuration.CSetting.SiteId);
                cardParms.Add("DEVICEID", Configuration.CSetting.DeviceId);
                cardParms.Add("DEVELOPERID", Configuration.CSetting.DeveloperId);
                cardParms.Add("LICENSEID", Configuration.CSetting.LicenseId);
                cardParms.Add("USERNAME", Configuration.CSetting.Username);
                cardParms.Add("PASSWORD", Configuration.CSetting.Password);
                cardParms.Add("VERSION", Configuration.CSetting.VersionNumber);
                cardParms.Add("LASTFOUR", cardInfo.Last4);
                cardParms.Add("ISALLOWDUP", cardInfo.isAllowDuplicate == true ? "true" : "false");
                if (Configuration.CPOSSet.PinPadModel.ToUpper().Trim() == "HPSPAX_ARIES8" || Configuration.CPOSSet.PinPadModel.ToUpper().Trim() == "HPSPAX_A920")
                {
                    cardParms.Add("CARDTYPE", cardInfo.cardType);
                }
            }

            if (PAYMENTPROCESSOR.ToUpper() == "EVERTEC")
            {
                cardParms.Add("ISMANUAL", cardInfo.isManual == true ? "true" : "false");
                cardParms.Add("ISALLOWDUP", cardInfo.isAllowDuplicate == true ? "true" : "false");
                cardParms.Add("TAXDETAIL", cardInfo.EvertecTaxDetails);
            }

            if (PAYMENTPROCESSOR == "XLINK")
            {
                cardParms.Add("ORDERID", ticketNum + DateTime.Now.ToString("MMddyyyyhh"));
            }
            #endregion

            if (PAYMENTPROCESSOR == "XLINK" || PAYMENTPROCESSOR == "HPSPAX" || PAYMENTPROCESSOR == "HPS" || PAYMENTPROCESSOR == "WORLDPAY" || PAYMENTPROCESSOR == "EVERTEC" || PAYMENTPROCESSOR == "VANTIV" || PAYMENTPROCESSOR == "PRIMERXPAY" || PAYMENTPROCESSOR == "ELAVON")
            {
                cardParms.Add("StationID", Configuration.StationID);
            }

            if (PAYMENTPROCESSOR == "VANTIV")
            {
                cardParms.Add("TOKENREQUEST", (cardInfo.Tokenize ? true : false).ToString());
                cardParms.Add("URL", Configuration.CPOSSet.SigPadHostAddr);
                cardParms.Add("APPLICATIONNAME", Configuration.ApplicationName);
                cardParms.Add("STATIONID", Configuration.StationID);
                cardParms.Add("ACCOUNTURL", Configuration.oMerchantConfig.VantivAccountUrl);
                cardParms.Add("SALEURL", Configuration.oMerchantConfig.VantivTokenUrl);
                cardParms.Add("TRIPOSPATH", Configuration.GetConfigFilePath());
                cardParms.Add("REPORTURL", Configuration.oMerchantConfig.VantivReportUrl);
                cardParms.Add("TransactionRecover", cardInfo.isTransactionRecover.ToString());
                cardParms.Add(PREREADTYPE, cardInfo.preReadType); //PRIMEPOS-3407
            }

            if (PAYMENTPROCESSOR == "XLINK" || PAYMENTPROCESSOR == "HPSPAX" || PAYMENTPROCESSOR == "HPS")
            {
                cardParms.Add("TRANSACTIONTYPE", cardInfo.Transtype);
                cardParms.Add("TerminalRefNumber", cardInfo.TerminalRefNumber);
            }

            try
            {
                String status = String.Empty;
                resp = null;
                pccRespInfo = null;

                primePosLogWriter.AppendPaymentRequestToLogger(cardParms);
                responseStatus = "";
                this.responseError = "";

                try
                {
                    status = creditProc.PreRead(ref cardParms, out resp);
                    PccResponse objCCResponse = new PccResponse();
                    DecipherResponse(resp, ref objCCResponse);
                    responseStatus = status;
                    responseError += objCCResponse.ResultDescription;
                    objCCResponse.ResponseStatus = status;
                    pccRespInfo = objCCResponse;
                }
                catch (Exception ex)
                {
                    logger.Fatal(ex, "PerformPreRead()");
                    responseError = ex.Message + "\n";
                    pccRespInfo = null;
                    resp = null;
                    status = "Payment Server Could Not Be Contacted. Please Contact Your Administrator";
                }
                isSale = true;
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "PerformPreRead()");
                responseError = ex.ToString();
                pccRespInfo = null;
                resp = null;
            }
            finally
            {
                cardParms.Clear();

            }
            return isSale;
        }

        public bool PerformPreReadSale(string ticketNum, ref PccCardInfo cardInfo) //PRIMEPOS-3504
        {
            logger.Trace("public bool PerformPreRead(string ticketNum, ref PccCardInfo cardInfo) - Entered");
            bool isSale = false;

            CultureInfo ci = CultureInfo.CurrentCulture;

            Double tempAmount = !string.IsNullOrEmpty(cardInfo.transAmount) ? Convert.ToDouble(cardInfo.transAmount) : Convert.ToDouble("0.00");
            Double tempFsaAmount = !string.IsNullOrEmpty(cardInfo.transFSAAmount) ? Convert.ToDouble(cardInfo.transFSAAmount) : Convert.ToDouble("0.00");
            Double tempFsaRxAmount = !string.IsNullOrEmpty(cardInfo.transFSARxAmount) ? Convert.ToDouble(cardInfo.transFSARxAmount) : Convert.ToDouble("0.00");

            IsSecurityPromptRequired(Transactions.CreditSale, cardParms);

            cardParms.Add(ACCOUNTNUM, cardInfo.cardNumber);
            cardParms.Add(EXPDATE, cardInfo.cardExpiryDate);
            cardParms.Add(ZIP, cardInfo.zipCode);
            cardParms.Add(ADDRESS, cardInfo.customerAddress);
            cardParms.Add(AMOUNT, tempAmount.ToString("F", ci).PadRight(2, '0'));
            cardParms.Add(CARDHOLDER, cardInfo.cardHolderName);
            cardParms.Add(TICKETNO, ticketNum);
            cardParms.Add(TIMEOUT, cardInfo.txnTimeOut);
            cardParms.Add(MMSCARD, cardInfo.cardType);
            cardParms.Add(PASSWORD, sPassword);
            cardParms.Add(ALLOWDUP, sALLOWDUP);
            cardParms.Add(TERMINALID, sTerminalID);

            if (cardInfo.UseToken)
            {
                cardParms.Add("XCACCOUNTID", cardInfo.ProfileID);
            }

            AddUserID(cardParms);

            if (cardInfo.trackII.Equals(string.Empty))
            {
                cardParms.Add(MANUALFLAG, "0");
                if (cardInfo.IsCardPresent != string.Empty)
                {
                    cardParms.Add(CARDPRESENT, cardInfo.IsCardPresent);
                }
            }
            else
            {
                cardParms.Add(TRACK, cardInfo.trackII);
                cardParms.Add(MANUALFLAG, "1");
            }

            if ((cardInfo.IsFSATransaction != "0") && tempFsaAmount != 0)
            {
                cardParms.Add("FSATRANSACTION", cardInfo.IsFSATransaction);
                cardParms.Add("FSAAMOUNT", tempFsaAmount.ToString("F", ci).PadRight(2, '0'));
                if (tempFsaRxAmount != 0)
                {
                    cardParms.Add("FSARXAMOUNT", tempFsaRxAmount.ToString("F", ci).PadRight(2, '0'));
                }
            }

            #region PRIMEPOS-3517 Currently only VANTIV is implemented for PREREAD
            if (PAYMENTPROCESSOR == "HPSPAX")
            {
                cardParms.Add("HOSTADDR", Configuration.CPOSSet.SigPadHostAddr);
                cardParms.Add("TOKENREQUEST", (cardInfo.Tokenize ? true : false).ToString());
                cardParms.Add("TransactionRecover", cardInfo.isTransactionRecover.ToString());
                cardParms.Add("ISFSACARD", cardInfo.IsFsaCard.ToString());
                cardParms.Add("SITEID", Configuration.CSetting.SiteId);
                cardParms.Add("DEVICEID", Configuration.CSetting.DeviceId);
                cardParms.Add("DEVELOPERID", Configuration.CSetting.DeveloperId);
                cardParms.Add("LICENSEID", Configuration.CSetting.LicenseId);
                cardParms.Add("USERNAME", Configuration.CSetting.Username);
                cardParms.Add("PASSWORD", Configuration.CSetting.Password);
                cardParms.Add("VERSION", Configuration.CSetting.VersionNumber);
                cardParms.Add("LASTFOUR", cardInfo.Last4);
                cardParms.Add("ISALLOWDUP", cardInfo.isAllowDuplicate == true ? "true" : "false");
                if (Configuration.CPOSSet.PinPadModel.ToUpper().Trim() == "HPSPAX_ARIES8" || Configuration.CPOSSet.PinPadModel.ToUpper().Trim() == "HPSPAX_A920")
                {
                    cardParms.Add("CARDTYPE", cardInfo.cardType);
                }
            }

            if (PAYMENTPROCESSOR.ToUpper() == "EVERTEC")
            {
                cardParms.Add("ISMANUAL", cardInfo.isManual == true ? "true" : "false");
                cardParms.Add("ISALLOWDUP", cardInfo.isAllowDuplicate == true ? "true" : "false");
                cardParms.Add("TAXDETAIL", cardInfo.EvertecTaxDetails);
            }

            if (PAYMENTPROCESSOR == "XLINK")
            {
                cardParms.Add("ORDERID", ticketNum + DateTime.Now.ToString("MMddyyyyhh"));
            }
            #endregion

            if (PAYMENTPROCESSOR == "XLINK" || PAYMENTPROCESSOR == "HPSPAX" || PAYMENTPROCESSOR == "HPS" || PAYMENTPROCESSOR == "WORLDPAY" || PAYMENTPROCESSOR == "EVERTEC" || PAYMENTPROCESSOR == "VANTIV" || PAYMENTPROCESSOR == "PRIMERXPAY" || PAYMENTPROCESSOR == "ELAVON")
            {
                cardParms.Add("StationID", Configuration.StationID);
            }

            if (PAYMENTPROCESSOR == "VANTIV")
            {
                cardParms.Add(ISNBSTRANSACTION, cardInfo.isNBSTransaction.ToString());
                cardParms.Add(PREREADID, cardInfo.preReadId);
                cardParms.Add("TOKENREQUEST", (cardInfo.Tokenize ? true : false).ToString());
                cardParms.Add("URL", Configuration.CPOSSet.SigPadHostAddr);
                cardParms.Add("APPLICATIONNAME", Configuration.ApplicationName);
                cardParms.Add("STATIONID", Configuration.StationID);
                cardParms.Add("ACCOUNTURL", Configuration.oMerchantConfig.VantivAccountUrl);
                cardParms.Add("SALEURL", Configuration.oMerchantConfig.VantivTokenUrl);
                cardParms.Add("TRIPOSPATH", Configuration.GetConfigFilePath());
                cardParms.Add("REPORTURL", Configuration.oMerchantConfig.VantivReportUrl);
                cardParms.Add("TransactionRecover", cardInfo.isTransactionRecover.ToString());
            }

            if (PAYMENTPROCESSOR == "XLINK" || PAYMENTPROCESSOR == "HPSPAX" || PAYMENTPROCESSOR == "HPS")
            {
                cardParms.Add("TRANSACTIONTYPE", cardInfo.Transtype);
                cardParms.Add("TerminalRefNumber", cardInfo.TerminalRefNumber);
            }

            try
            {
                String status = String.Empty;
                resp = null;
                pccRespInfo = null;

                primePosLogWriter.AppendPaymentRequestToLogger(cardParms);
                responseStatus = "";
                this.responseError = "";

                try
                {
                    status = creditProc.PreReadSale(ref cardParms, out resp);
                    PccResponse objCCResponse = new PccResponse();
                    DecipherResponse(resp, ref objCCResponse);
                    #region PRIMEPOS-3517 Currently only VANTIV is implemented for PREREAD
                    if (PAYMENTPROCESSOR == "HPSPAX")
                    {
                        if (resp != null)
                        {
                            string ccHolder = string.Empty;
                            resp.GetAllKeys().TryGetValue("CARDHOLDER", out ccHolder);
                            cardInfo.cardHolderName = ccHolder;
                        }
                    }
                    #endregion
                    responseStatus = status;
                    responseError += objCCResponse.ResultDescription;
                    objCCResponse.ResponseStatus = status;
                    pccRespInfo = objCCResponse;
                }
                catch (Exception ex)
                {
                    logger.Fatal(ex, "PerformPreRead()");
                    responseError = ex.Message + "\n";
                    pccRespInfo = null;
                    resp = null;
                    status = "Payment Server Could Not Be Contacted. Please Contact Your Administrator";
                }
                isSale = true;
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "PerformPreRead()");
                responseError = ex.ToString();
                pccRespInfo = null;
                resp = null;
            }
            finally
            {
                cardParms.Clear();
            }
            return isSale;
        }

        public void PerformPreReadSalesReturn(string ticketNum, ref PccCardInfo cardInfo) //PRIMEPOS-3504
        {
            primePosLogWriter.AppendCommentsToLogger("Entered into PerformCreditSalesReturn() method\r\n---------------------------------------------\r\nRequest Parameters & Their Values\r\n----------------------------------\r\n");
            
            CultureInfo ci = CultureInfo.CurrentCulture;

            Double tempAmount = Convert.ToDouble(cardInfo.transAmount);
            Double tempFsaAmount = string.IsNullOrEmpty(cardInfo.transFSAAmount) ? Convert.ToDouble("0.00") : Convert.ToDouble(cardInfo.transFSAAmount);
            Double tempFsaRxAmount = string.IsNullOrEmpty(cardInfo.transFSARxAmount) ? Convert.ToDouble("0.00") : Convert.ToDouble(cardInfo.transFSARxAmount);//rx pr amt

            IsSecurityPromptRequired(Transactions.CreditSalesReturn, cardParms);

            cardParms.Add(ACCOUNTNUM, cardInfo.cardNumber);
            cardParms.Add(EXPDATE, cardInfo.cardExpiryDate);
            cardParms.Add(AMOUNT, tempAmount.ToString("F", ci).PadRight(2, '0'));
            cardParms.Add(TIMEOUT, cardInfo.txnTimeOut);
            cardParms.Add(MMSCARD, cardInfo.cardType);
            cardParms.Add(ZIP, cardInfo.zipCode);
            cardParms.Add(ADDRESS, cardInfo.customerAddress);
            cardParms.Add(CARDHOLDER, cardInfo.cardHolderName);
            cardParms.Add(TICKETNO, ticketNum);
            
            AddUserID(cardParms);
            cardParms.Add(PASSWORD, sPassword);
            cardParms.Add(ALLOWDUP, sALLOWDUP);
            cardParms.Add(TERMINALID, sTerminalID);
            
            if (cardInfo.trackII.Equals(string.Empty))
            {
                cardParms.Add(MANUALFLAG, "0");
            }
            else
            {
                cardParms.Add(TRACK, cardInfo.trackII);
                cardParms.Add(MANUALFLAG, "1");
            }
            if (tempFsaAmount != 0)
            {
                cardParms.Add("FSATRANSACTION", cardInfo.IsFSATransaction);
                cardParms.Add("FSAAMOUNT", tempFsaAmount.ToString("F", ci).PadRight(2, '0'));
                if (tempFsaRxAmount != 0)
                {
                    cardParms.Add("FSARXAMOUNT", tempFsaRxAmount.ToString("F", ci).PadRight(2, '0'));
                }
            }
            if (PAYMENTPROCESSOR == "XLINK" || PAYMENTPROCESSOR == "HPASPAX" || PAYMENTPROCESSOR == "HPS" || PAYMENTPROCESSOR == "WORLDPAY" || PAYMENTPROCESSOR == "EVERTEC" || PAYMENTPROCESSOR == "VANTIV" || PAYMENTPROCESSOR == "PRIMERXPAY" || PAYMENTPROCESSOR == "ELAVON")
            {
                cardParms.Add("StationID", Configuration.StationID);
            }

            if (PAYMENTPROCESSOR == "VANTIV")
            {
                cardParms.Add(ISNBSTRANSACTION, cardInfo.isNBSTransaction.ToString());
                cardParms.Add("URL", Configuration.CPOSSet.SigPadHostAddr);
                cardParms.Add("APPLICATIONNAME", Configuration.ApplicationName);
                cardParms.Add("STATIONID", Configuration.StationID);
                cardParms.Add(TRANSID, cardInfo.TransactionID);
                cardParms.Add("TRIPOSPATH", Configuration.GetConfigFilePath());
                cardParms.Add("TransactionRecover", cardInfo.isTransactionRecover.ToString());
                cardParms.Add("TRANSACTIONURL", Configuration.oMerchantConfig.VantivTokenUrl);
                cardParms.Add("REPORTURL", Configuration.oMerchantConfig.VantivReportUrl);
                cardParms.Add(PREREADID, cardInfo.preReadId); //PRIMEPOS-3407
            }

            try
            {
                resp = null;
                pccRespInfo = null;
                primePosLogWriter.AppendPaymentRequestToLogger(cardParms);
                String status = creditProc.PreReadReturn(ref cardParms, out resp);
                PccResponse objCCResponse = new PccResponse();
                DecipherResponse(resp, ref objCCResponse);
                responseStatus = status;
                responseError = objCCResponse.ResultDescription;
                objCCResponse.ResponseStatus = status;
                pccRespInfo = objCCResponse;
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "PerformCreditSalesReturn()");
                responseError = ex.ToString();
                pccRespInfo = null;
                resp = null;
                primePosLogWriter.AppendCommentsToLogger(responseError + "\r\n");
            }
            finally
            {
                cardParms.Clear();
            }
            primePosLogWriter.AppendCommentsToLogger("Exit From  PerformCreditSalesReturn() method\r\n------------------------------------\r\n");
        }
        #endregion

        public bool PerformCreditSale(string ticketNum, ref PccCardInfo cardInfo) //Changed on 28-08-08
        {
            logger.Trace("public bool PerformCreditSale(string ticketNum, ref PccCardInfo cardInfo) - Entered");
            bool isSale = false;
            // Added By Dharmendra (SRT) Dec-08-08 to store request & response in the logger file            
            primePosLogWriter.AppendCommentsToLogger("Entered into PerformCreditSale() method\r\n---------------------------------------\r\nRequest Parameters & Their Values\r\n----------------------------------\r\n");
            //Added Till Here
            if (PAYMENTPROCESSOR.ToUpper() != "WORLDPAY")
            {
                CultureInfo ci = CultureInfo.CurrentCulture;
                string tempManualFlage = string.Empty;
                string dictObj = string.Empty;
                string trnsdt = string.Empty;

                // Payment.Amount.TotalAmount = 2.0;
                // SigPadUtil.DefaultInstance.VF.ProcessPayment(Payment, WPAccount);

                Double tempAmount = !string.IsNullOrEmpty(cardInfo.transAmount) ? Convert.ToDouble(cardInfo.transAmount) : Convert.ToDouble("0.00");
                Double tempFsaAmount = !string.IsNullOrEmpty(cardInfo.transFSAAmount) ? Convert.ToDouble(cardInfo.transFSAAmount) : Convert.ToDouble("0.00"); //Healthcare amt
                Double tempFsaRxAmount = !string.IsNullOrEmpty(cardInfo.transFSARxAmount) ? Convert.ToDouble(cardInfo.transFSARxAmount) : Convert.ToDouble("0.00");//rx pr amt

                //Double otc_fsa_amt = 0;//clincamt = Healthcare amt - rx pr amt
                //ADDED PRASHANT 5 JUN 2010
                IsSecurityPromptRequired(Transactions.CreditSale, cardParms);
                //END ADDED PRASHANT 5 JUN 2010

                cardParms.Add(ACCOUNTNUM, cardInfo.cardNumber);
                cardParms.Add(EXPDATE, cardInfo.cardExpiryDate);
                cardParms.Add(ZIP, cardInfo.zipCode);
                cardParms.Add(ADDRESS, cardInfo.customerAddress);
                cardParms.Add(AMOUNT, tempAmount.ToString("F", ci).PadRight(2, '0'));
                cardParms.Add(CARDHOLDER, cardInfo.cardHolderName);
                cardParms.Add(TICKETNO, ticketNum);
                cardParms.Add(TIMEOUT, cardInfo.txnTimeOut);
                cardParms.Add(MMSCARD, cardInfo.cardType);
                cardParms.Add(PASSWORD, sPassword);
                cardParms.Add(ALLOWDUP, sALLOWDUP);
                cardParms.Add(TERMINALID, sTerminalID); //PrimePOS-2491 

                if (cardInfo.UseToken)
                {
                    cardParms.Add("XCACCOUNTID", cardInfo.ProfileID);
                }
                //Added By Dharmendra on Mar-12-09 to pass the user name in case of Payment Processor is either XCHARGE or XLINK
                AddUserID(cardParms);
                //Added Till Here
                if (cardInfo.trackII.Equals(string.Empty))
                {
                    cardParms.Add(MANUALFLAG, "0");
                    if (cardInfo.IsCardPresent != string.Empty)
                    {
                        cardParms.Add(CARDPRESENT, cardInfo.IsCardPresent);
                    }
                }
                else
                {
                    if (PAYMENTPROCESSOR == "HPS")
                    {
                        cardParms.Add(TRACK, cardInfo.Completetrack);
                    }
                    else
                    {
                        cardParms.Add(TRACK, cardInfo.trackII);
                    }
                    cardParms.Add(MANUALFLAG, "1");
                }


                //Added By SRT(Gaurav) Date : 20-NOV-2008
                if ((cardInfo.IsFSATransaction != "0") && tempFsaAmount != 0)
                {
                    //otc_fsa_amt = tempFsaAmount - tempFsaRxAmount;//clincamt = Healthcare amt - rx pr amt

                    cardParms.Add("FSATRANSACTION", cardInfo.IsFSATransaction);
                    cardParms.Add("FSAAMOUNT", tempFsaAmount.ToString("F", ci).PadRight(2, '0'));
                    //if (otc_fsa_amt != 0)
                    //{
                    //    cardParms.Add("OTC_FSA_AMT", otc_fsa_amt.ToString("F", ci).PadRight(2, '0'));
                    //}
                    //Added By sRT(Gaurav) Date: 18-Aug-2009
                    if (tempFsaRxAmount != 0)
                    {
                        cardParms.Add("FSARXAMOUNT", tempFsaRxAmount.ToString("F", ci).PadRight(2, '0'));
                    }

                    //End Of Added By SRT(Ritesh Parekh)
                }

                //PRIMEPOS-2528 (Suraj) 1-June-18 HPSPAX Communincation Params 
                if (PAYMENTPROCESSOR == "HPSPAX")
                {
                    cardParms.Add("HOSTADDR", Configuration.CPOSSet.SigPadHostAddr);
                    cardParms.Add("TOKENREQUEST", (cardInfo.Tokenize ? true : false).ToString());
                    cardParms.Add("TransactionRecover", cardInfo.isTransactionRecover.ToString()); // SAJID LOCAL DETAILS REPORT PRIMEPOS-2862
                    cardParms.Add("ISFSACARD", cardInfo.IsFsaCard.ToString());//2990
                    cardParms.Add("SITEID", Configuration.CSetting.SiteId);//2990
                    cardParms.Add("DEVICEID", Configuration.CSetting.DeviceId);//2990
                    cardParms.Add("DEVELOPERID", Configuration.CSetting.DeveloperId);//2990
                    cardParms.Add("LICENSEID", Configuration.CSetting.LicenseId);//2990
                    cardParms.Add("USERNAME", Configuration.CSetting.Username);//2990
                    cardParms.Add("PASSWORD", Configuration.CSetting.Password);//2990
                    cardParms.Add("VERSION", Configuration.CSetting.VersionNumber);//2990
                    cardParms.Add("LASTFOUR", cardInfo.Last4);//2990
                    cardParms.Add("ISALLOWDUP", cardInfo.isAllowDuplicate == true ? "true" : "false");//PRIMEPOS-3047
                    #region PRIMEPOS-3078
                    if (Configuration.CPOSSet.PinPadModel.ToUpper().Trim() == "HPSPAX_ARIES8" || Configuration.CPOSSet.PinPadModel.ToUpper().Trim() == "HPSPAX_A920")//PRIMEPOS-3146
                    {
                        cardParms.Add("CARDTYPE", cardInfo.cardType);
                    }
                    #endregion
                }

                #region PRIMEPOS-2784 EVERTEC
                if (PAYMENTPROCESSOR.ToUpper() == "EVERTEC")
                {
                    cardParms.Add("ISMANUAL", cardInfo.isManual == true ? "true" : "false");//PRIMEPOS-2805
                    cardParms.Add("ISALLOWDUP", cardInfo.isAllowDuplicate == true ? "true" : "false");
                    cardParms.Add("TAXDETAIL", cardInfo.EvertecTaxDetails);//2664
                }
                #endregion

                //PRIMEPOS-2738
                if (PAYMENTPROCESSOR == "XLINK")
                {
                    cardParms.Add("ORDERID", ticketNum + DateTime.Now.ToString("MMddyyyyhh"));
                }
                //

                //PRIMEPOS-2665 10-Apr-2019 JY Added
                if (PAYMENTPROCESSOR == "XLINK" || PAYMENTPROCESSOR == "HPSPAX" || PAYMENTPROCESSOR == "HPS" || PAYMENTPROCESSOR == "WORLDPAY" || PAYMENTPROCESSOR == "EVERTEC" || PAYMENTPROCESSOR == "VANTIV" || PAYMENTPROCESSOR == "PRIMERXPAY" || PAYMENTPROCESSOR == "ELAVON")//PRIMEPOS-2761 - Added PAYMENTPROCESSOR == "HPSPAX" and PAYMENTPROCESSOR == "HPS"  //PRIMEPOS-2841 Added by Arvind//2943
                {
                    cardParms.Add("StationID", Configuration.StationID);
                }
                //VANTIV Communincation Params PRIMEPOS-2636
                if (PAYMENTPROCESSOR == "VANTIV")
                {
                    cardParms.Add("TOKENREQUEST", (cardInfo.Tokenize ? true : false).ToString());//PRIMEPOS - 2636 - Added For Tokenization
                    cardParms.Add("URL", Configuration.CPOSSet.SigPadHostAddr);
                    cardParms.Add("APPLICATIONNAME", Configuration.ApplicationName);
                    cardParms.Add("STATIONID", Configuration.StationID);
                    cardParms.Add("ACCOUNTURL", Configuration.oMerchantConfig.VantivAccountUrl);////PRIMEPOS-TOKENURL
                    cardParms.Add("SALEURL", Configuration.oMerchantConfig.VantivTokenUrl);
                    cardParms.Add("TRIPOSPATH", Configuration.GetConfigFilePath());//PRIMEPOS-2895 Vantiv Arvind
                    cardParms.Add("REPORTURL", Configuration.oMerchantConfig.VantivReportUrl); //PRIMEPOS-3156
                    cardParms.Add("TransactionRecover", cardInfo.isTransactionRecover.ToString()); //PRIMEPOS-3156
                }
                #region PRIMEPOS-2761
                if (PAYMENTPROCESSOR == "XLINK" || PAYMENTPROCESSOR == "HPSPAX" || PAYMENTPROCESSOR == "HPS")//PRIMEPOS-2761 - Added PAYMENTPROCESSOR == "HPSPAX" and PAYMENTPROCESSOR == "HPS" 
                {
                    cardParms.Add("TRANSACTIONTYPE", cardInfo.Transtype);
                    cardParms.Add("TerminalRefNumber", cardInfo.TerminalRefNumber);
                }
                #endregion

                #region PRIMEPOS-2841
                if (PAYMENTPROCESSOR == "PRIMERXPAY")
                {
                    cardParms.Add("PHARMACYNO", sPharmacyNumber);
                    cardParms.Add("FSACARD", cardInfo.isFSACard == true ? "true" : "false");//PRIMEPOS-2841
                    cardParms.Add("URL", Configuration.CSetting.PrimeRxPayUrl + Configuration.CSetting.PrimerxPayExtensionUrl);//2956
                    cardParms.Add("PASSWORD", Configuration.CSetting.PrimeRxPaySecretKey);
                    cardParms.Add("APIKEY", Configuration.CSetting.PrimeRxPayClientId);
                    cardParms.Add("TOKENREQUEST", (cardInfo.Tokenize ? true : false).ToString());//PRIMEPOS-TOKENSALE
                    cardParms.Add("PAYPROVIDERID", Configuration.CSetting.PayProviderID.ToString());//PRIMEPOS-2902
                    cardParms.Add("EMAIL", cardInfo.Email);//PRIMEPOS-2915
                    cardParms.Add("ISEMAIL", cardInfo.IsEmail ? "true" : "false");//PRIMEPOS-2915
                    cardParms.Add("MOBILE", cardInfo.Phone);//PRIMEPOS-2915
                    cardParms.Add("ISMOBILE", cardInfo.IsPhone ? "true" : "false");//PRIMEPOS-2915
                    cardParms.Add("ISCUSTOMERDRIVEN", cardInfo.IsCustomerDriven ? "true" : "false");//PRIMEPOS-2915
                    cardParms.Add("CUSTOMERNAME", cardInfo.CustomerName);//PRIMEPOS-2915
                    cardParms.Add("DOB", cardInfo.DOB);//PRIMEPOS-2915
                    cardParms.Add("ISPRIMERXPAYLINKSEND", cardInfo.IsPrimeRxPayLinkSend ? "true" : "false"); //PRIMEPOS-3248
                    cardParms.Add("LINKEXPIRY", Configuration.CSetting.LinkExpriyInMinutes);//PRIMEPOS-2915
                    cardParms.Add("ISSECUREDEVICE", Convert.ToString(Configuration.CPOSSet.IsSecureDevice));//PRIMEPOS-3455
                    cardParms.Add("TERMINALMODEL", Configuration.CPOSSet.SecureDeviceModel);//PRIMEPOS-3455
                    cardParms.Add("TERMINALSRNUMBER", Configuration.CPOSSet.SecureDeviceSrNumber);//PRIMEPOS-3455
                }
                #endregion

                if (PAYMENTPROCESSOR == "ELAVON")//2943
                {
                    cardParms.Add("TERMINALID", Configuration.CSetting.TerminalID);
                    cardParms.Add("CHAINCODE", Configuration.CSetting.ChainCode);
                    cardParms.Add("LOCATIONNAME", Configuration.CSetting.LocationName);
                    cardParms.Add("TOKENREQUEST", (cardInfo.Tokenize ? true : false).ToString());
                    cardParms.Add("ISALLOWDUP", cardInfo.isAllowDuplicate == true ? "true" : "false");
                    cardParms.Add("ISTAX", cardInfo.IsElavonTax == true ? "true" : "false");
                    cardParms.Add("TAXAMT", cardInfo.ElavonTotalTax);//2943
                }
            }
            try
            {
                String status = String.Empty;
                resp = null;//Added by Arvind 
                pccRespInfo = null;//Added by Arvind 
                if (PAYMENTPROCESSOR.ToUpper() != "WORLDPAY")
                {
                    primePosLogWriter.AppendPaymentRequestToLogger(cardParms); //Added By Dharmendra on Dec-08-08 to append the request parameter values in logger. 
                    responseStatus = "";
                    this.responseError = "";

                    try
                    {
                        status = creditProc.Sale(ref cardParms, out resp);
                        PccResponse objCCResponse = new PccResponse();
                        DecipherResponse(resp, ref objCCResponse);
                        //#region PRIMEPOS-2841//2915
                        //if (PAYMENTPROCESSOR.ToUpper() == "PRIMERXPAY")
                        //{
                        //    if (!string.IsNullOrWhiteSpace(resp.Result))
                        //        clsCoreUIHelper.ShowOKMsg(resp.Result);
                        //}
                        //#endregion
                        //PRIMEPOS-2578 Added CardHolderName Response - Discover Specific -16AUg2018 - NILESHJ
                        if (PAYMENTPROCESSOR == "HPSPAX")
                        {
                            if (resp != null) // NileshJ - 12-Dec-2018
                            {
                                string ccHolder = string.Empty;
                                resp.GetAllKeys().TryGetValue("CARDHOLDER", out ccHolder);
                                //if (objCCResponse.CardType.ToUpper() == SecureSubmit.Terminals.PAX.CardType.DiSCOVER.ToString().ToUpper())
                                //{
                                cardInfo.cardHolderName = ccHolder;
                                //}
                            }
                        }

                        responseStatus = status;
                        responseError += objCCResponse.ResultDescription;
                        objCCResponse.ResponseStatus = status;
                        pccRespInfo = objCCResponse;
                    }
                    catch (Exception ex)
                    {
                        logger.Fatal(ex, "PerformCreditSale()");
                        responseError = ex.Message + "\n";
                        pccRespInfo = null;//Added by Arvind 
                        resp = null;//Added by Arvind
                        status = "Payment Server Could Not Be Contacted. Please Contact Your Administrator";
                    }
                    isSale = true;
                }
                else
                {
                    #region PRIMEPOS-2761
                    logger.Trace("Orphan - Start1");
                    cardInfo.StationID = Configuration.StationID;
                    cardInfo.UserId = Configuration.UserName;
                    cardInfo.TicketNo = ticketNum;
                    logger.Trace("Orphan - End1");
                    #endregion

                    TransactionResult resp = new TransactionResult();
                    logger.Trace("isSale = ProcessWpCard(TransactionType.Sale, TransactionSubType.None, cardInfo, out resp);");
                    isSale = ProcessWpCard(TransactionType.Sale, TransactionSubType.None, cardInfo, out resp);
                    WP_TransResult = resp;

                    if (WP_TransResult.Result.Equals("SUCCESS"))
                    {
                        PccPaymentSvr.DefaultInstance.EntryMethod = (string.IsNullOrEmpty(WP_TransResult.ResponseTags.EntryMethod) ? WP_TransResult.EntryMethod : WP_TransResult.ResponseTags.EntryMethod);

                        if (!string.IsNullOrEmpty(WP_TransResult.Token))
                        {
                            PccPaymentSvr.DefaultInstance.ProfiledID = WP_TransResult.Token;

                        }

                        if (WP_TransResult.ResponseTags.CashBack > 0)
                        {
                            PccPaymentSvr.DefaultInstance.CashBack = WP_TransResult.ResponseTags.CashBack.ToString();
                        }

                        if (WP_TransResult.PartialApproval)
                        {
                            cardInfo.transAmount = WP_TransResult.Amount.ToString();
                        }
                        cardInfo.OrderID = WP_TransResult.OrderID;
                        cardInfo.TransactionID = WP_TransResult.TransactionID;
                        #region PRIMEPOS-2761
                        try
                        {
                            using (var db = new Possql())
                            {
                                string Ticketnumber = cardInfo.TicketNo;
                                CCTransmission_Log cclog = new CCTransmission_Log();
                                cclog = db.CCTransmission_Logs.Where(w => w.TicketNo == Ticketnumber).SingleOrDefault();
                                cclog.HostTransID = cardInfo.OrderID + "|" + cardInfo.TransactionID;
                                cclog.ResponseMessage = WP_TransResult.Result;
                                db.CCTransmission_Logs.Attach(cclog);
                                db.Entry(cclog).Property(p => p.HostTransID).IsModified = true;
                                db.Entry(cclog).Property(p => p.ResponseMessage).IsModified = true;
                                db.SaveChanges();
                            }
                        }
                        catch (Exception exep)
                        {
                            logger.Fatal(exep, exep.Message);
                        }
                        #endregion
                    }
                    else
                    {
                        //isSale = false;
                        PccPaymentSvr.DefaultInstance.EntryMethod = "";
                        #region PRIMEPOS-2761
                        try
                        {
                            using (var db = new Possql())
                            {
                                string Ticketnumber = cardInfo.TicketNo;
                                CCTransmission_Log cclog = new CCTransmission_Log();
                                cclog = db.CCTransmission_Logs.Where(w => w.TicketNo == Ticketnumber).SingleOrDefault();
                                //cclog.HostTransID = cardInfo.OrderID + "|" + cardInfo.TransactionID;
                                cclog.ResponseMessage = WP_TransResult.Result;
                                db.CCTransmission_Logs.Attach(cclog);
                                //db.Entry(cclog).Property(p => p.HostTransID).IsModified = true;
                                db.Entry(cclog).Property(p => p.ResponseMessage).IsModified = true;
                                db.SaveChanges();
                            }
                        }
                        catch (Exception exep)
                        {
                            logger.Fatal(exep, exep.Message);
                        }
                        #endregion
                    }




                    //WPResponse owpresp = new WPResponse();
                    //LoadWPresponse(resp, out owpresp);
                    //WpRespInfo = owpresp;

                    /*if (WpProcessing(MMS.Device.WPDevice.WPData.PayTypes.Credit, MMS.Device.WPDevice.WPData.TransTypes.Sale, cardInfo))
                    {
                        isSale = true;
                    }*/

                }
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "PerformCreditSale()");
                responseError = ex.ToString();
                pccRespInfo = null;//Added by Arvind 
                resp = null;//Added by Arvind
                //Added By Dharmendra (SRT) to logg the error
                //loggerString += responseError + "\r\n"; 
                primePosLogWriter.AppendCommentsToLogger(responseError + "\r\n"); // Added By Dharmendra (SRT) on Dec-08-08 to append response error to logger
            }
            finally
            {
                cardParms.Clear();

            }
            return isSale;
            //Added By Dharmendra (SRT) on Dec-08-08           
            primePosLogWriter.AppendCommentsToLogger("Exit From PerformCreditSale() method\r\n------------------------------------\r\n");
            //Added Till Here

        }

        private void LoadWPresponse(TransactionResult result, out WPResponse resp)
        {
            /*resp = new PccResponse();

            resp.Result = result.Result;
            resp.TrouTd = result.ApprovalCode;
            resp.TransId = result.TransactionID;
            resp.ResultDescription = result.TransDetail;*/

            resp = new WPResponse();

            resp.Result = result.Result;
            resp.HistoryID = result.TransactionID;
            resp.OrderID = result.OrderID;
            if (result.Balance > 0)
            {
                resp.Balance = result.Balance.ToString();
            }
            resp.Last4Digits = result.Last4;
            resp.EntryMethod = result.EntryMethod;
            resp.PayType = result.AccountType;
            resp.Status = result.Result;
            resp.AuthCode = result.ResponseTags.AuthCode;
            if (result.Amount > 0)
            {
                resp.TotalAmt = result.Amount.ToString();
            }
            if (result.ResponseTags.CashBack > 0)
            {
                resp.CashBackAmt = result.ResponseTags.CashBack.ToString();
            }

            resp.PaymentProfileID = result.Token;






        }

        private bool ProcessWPReverse(TransactionType tType, PccCardInfo cardInfo, out TransactionResult oresult)
        {
            oresult = new TransactionResult();

            ProcessCard oWPCard = new ProcessCard();

            //removing negative value for WP 
            if (cardInfo.transAmount.Contains("-"))
            {
                cardInfo.transAmount = cardInfo.transAmount.Replace("-", "");
            }
            oWPCard.Amount = decimal.Parse(cardInfo.transAmount, System.Globalization.CultureInfo.InstalledUICulture.NumberFormat);
            oWPCard.TransType = tType;

            oWPCard.OrderID = cardInfo.OrderID;
            oWPCard.TransactionID = cardInfo.TransactionID;
            #region PRIMEPOS-2761
            oWPCard.TicketNo = cardInfo.TicketNo;
            oWPCard.StationID = cardInfo.StationID;
            oWPCard.UserId = cardInfo.UserId;
            #endregion
            //string result = oWPCard.DoTransaction(); // Comented by NileshJ - PRIMEPOS-2737 30-Sept-2019
            string result = oWPCard.DoTransaction(Configuration.CPOSSet.AllowManualFirstMiles); // PRIMEPOS-2737 - Configuration.CInfo.AllowManualFirstMiles Addded by NileshJ for allow first mile manual transaction 30-Sept-2019
            oresult.LoadResponse(result);
            responseError = string.Empty;
            ResponseStatus = oresult.Result;
            responseError += oresult.Result;
            #region PRIMEPOS-2761

            using (var db = new Possql())
            {
                CCTransmission_Log cclog = new CCTransmission_Log();
                if (oresult.Result.ToUpper().Equals("SUCCESS"))
                {
                    try
                    {
                        string OrderID = oresult.OrderID.ToString();
                        string TransID = oresult.TransactionID.ToString();
                        long OrgTransNo = 0;
                        cclog = db.CCTransmission_Logs.Where(w => w.HostTransID.Contains(OrderID + "|")).SingleOrDefault();
                        cclog.IsReversed = true;
                        OrgTransNo = cclog.TransNo;
                        //cclog.ResponseMessage = oresult.Result;
                        db.CCTransmission_Logs.Attach(cclog);
                        db.Entry(cclog).Property(p => p.IsReversed).IsModified = true;
                        //db.Entry(cclog).Property(p => p.ResponseMessage).IsModified = true;
                        db.SaveChanges();

                        cclog = db.CCTransmission_Logs.OrderByDescending(r => r.TransNo).Where(w => w.TicketNo.Contains(cardInfo.TicketNo)).Take(1).SingleOrDefault();
                        cclog.OrgTransNo = OrgTransNo;
                        cclog.ResponseMessage = oresult.Result;
                        cclog.HostTransID = OrderID + "|" + TransID;
                        db.CCTransmission_Logs.Attach(cclog);
                        db.Entry(cclog).Property(p => p.OrgTransNo).IsModified = true;
                        db.Entry(cclog).Property(p => p.ResponseMessage).IsModified = true;
                        db.Entry(cclog).Property(p => p.HostTransID).IsModified = true;
                        db.SaveChanges();
                    }
                    catch (Exception exep)
                    {
                        logger.Fatal(exep, exep.Message);
                    }

                }
                else
                {
                    #region PRIMEPOS-2761
                    try
                    {
                        string Ticketnumber = cardInfo.TicketNo;
                        cclog = db.CCTransmission_Logs.OrderByDescending(r => r.TransNo).Where(w => w.TicketNo.Contains(cardInfo.TicketNo)).Take(1).SingleOrDefault();
                        //cclog.HostTransID = cardInfo.OrderID + "|" + cardInfo.TransactionID;
                        cclog.ResponseMessage = WP_TransResult.Result;
                        db.CCTransmission_Logs.Attach(cclog);
                        //db.Entry(cclog).Property(p => p.HostTransID).IsModified = true;
                        db.Entry(cclog).Property(p => p.ResponseMessage).IsModified = true;
                        db.SaveChanges();

                    }
                    catch (Exception exep)
                    {
                        logger.Fatal(exep, exep.Message);
                    }
                    #endregion
                }
            }
            #endregion
            if (oresult.Result.ToUpper().Equals("SUCCESS") && oresult.ApprovalCode.ToUpper().Contains("DUPLICATE"))
            {
                ResponseStatus = "DECLINED";
                oresult.Result = "DECLINED";

                responseError = string.Empty;
                responseError += "DECLINED";

                responseError += "  " + "DUPLICATE TRANSACTION";
                return false;
            }

            if (oresult.Result.Contains("DECLINED"))
            {
                return false;
            }


            return true;
        }

        private bool ProcessWpCard(TransactionType tType, TransactionSubType tsType, PccCardInfo cardInfo, out TransactionResult oresult)
        {
            logger.Trace("private bool ProcessWpCard(TransactionType tType, TransactionSubType tsType, PccCardInfo cardInfo, out TransactionResult oresult) - Entered");


            oresult = new TransactionResult();

            ProcessCard oWPCard = new ProcessCard();

            //removing negative value for WP 
            if (cardInfo.transAmount.Contains("-"))
            {
                cardInfo.transAmount = cardInfo.transAmount.Replace("-", "");
            }

            oWPCard.ReturnImgType = ImageFormat.Png;

            oWPCard.Amount = decimal.Parse(cardInfo.transAmount, System.Globalization.CultureInfo.InstalledUICulture.NumberFormat);
            oWPCard.TransType = tType;
            //since tax is already included in Amount
            oWPCard.RequireTax = false;

            //if(tType==TransactionType.Void || tType == TransactionType.)
            #region PRIMEPOS-2738 ADDED BY ARVIND FOR REVERSAL
            if (tType == TransactionType.Refund && Configuration.CSetting.StrictReturn == true)
            {
                oWPCard.OrderID = cardInfo.OrderID;
                oWPCard.TransactionID = cardInfo.TransactionID;
            }
            #endregion

            oWPCard.AllowTokenization = true;

            if (cardInfo.Tokenize)
            {
                oWPCard.Tokenize = true;
            }

            if (cardInfo.UseToken)
            {
                oWPCard.Token = cardInfo.ProfileID;
                oWPCard.Last4Digits = cardInfo.Last4;
            }

            if (Configuration.CPOSSet.CaptureSigForDebit)
            {
                oWPCard.SignatureOptional = true;
            }
            else
            {
                oWPCard.SignatureOptional = false;
            }


            oWPCard.TransSubType = tsType;
            if (tsType != TransactionSubType.FSA && tsType != TransactionSubType.None)
            {
                oWPCard.IsEBT = true;
                if (tType == TransactionType.Sale)
                {
                    oWPCard.TransSubType = TransactionSubType.EBT_FoodStampSale;
                }
                else
                {
                    oWPCard.TransSubType = TransactionSubType.EBT_FoodStampReturn;
                }

                if (Configuration.CPOSSet.CaptureSigForEBT)
                {
                    oWPCard.SignatureOptional = true;
                }
                else
                {
                    oWPCard.SignatureOptional = false;
                }
            }
            if (cardInfo.IsFSATransaction == "True")
            {
                oWPCard.IsFSA = true;
                oWPCard.RXAmount = decimal.Parse(cardInfo.transFSAAmount, System.Globalization.CultureInfo.InstalledUICulture.NumberFormat);
                oWPCard.TransSubType = TransactionSubType.FSA;
            }
            #region PRIMEPOS-2761
            logger.Trace("Orphan - Start");
            oWPCard.TicketNo = cardInfo.TicketNo;
            oWPCard.StationID = cardInfo.StationID;
            oWPCard.UserId = cardInfo.UserId;
            logger.Trace("orphan - End");
            #endregion
            // oWPCard.ReturnImgEncoding = EncodingFormat.BinHex;

            //string result = oWPCard.DoTransaction() // Commented by - NileshJ - PRIMEPOS-2737 30-Sept-2019
            string result = oWPCard.DoTransaction(Configuration.CPOSSet.AllowManualFirstMiles); // PRIMEPOS-2737 - Configuration.CInfo.AllowManualFirstMiles Addded by NileshJ for allow first mile manual transaction 30-Sept-2019
            oresult.LoadResponse(result);

            if (cardInfo.IsFSATransaction == "True" && oresult.Result.ToUpper().Equals("SUCCESS"))
            {
                oresult.IsFSA = true;
            }
            responseError = string.Empty;
            if (!string.IsNullOrWhiteSpace(oresult.Result))
            {
                if (oresult.Result.ToUpper().Equals("NONE"))
                {
                    ResponseStatus = "TRANSACTION CANCELLED ";
                    responseError += "TRANSACTION CANCELLED";
                }
                else
                {
                    ResponseStatus = oresult.Result;
                    responseError += oresult.Result;
                }

            }
            else
            {
                ResponseStatus = "CANCELLED / NO RESPONSE";
                responseError += "CANCELLED / NO RESPONSE";
            }

            /* if (!string.IsNullOrWhiteSpace())
             {
                 responseError += oresult.Result;
             }
             else
             {

             }*/


            if (!string.IsNullOrWhiteSpace(oresult.Result) && oresult.Result.ToUpper().Contains("DECLINED"))
            {
                if (!string.IsNullOrWhiteSpace(oresult.ResponseTags.DeclineCode))
                {
                    responseError += "  " + oresult.ResponseTags.DeclineCode;
                }
                if (!string.IsNullOrWhiteSpace(oresult.ResponseTags.DeclineMessage))
                {
                    responseError += "  " + oresult.ResponseTags.DeclineMessage;
                }
                else
                {
                    responseError += "  " + oresult.TransDetail;
                }
            }
            if (!string.IsNullOrWhiteSpace(oresult.Result) && oresult.Result.ToUpper().Equals("SUCCESS") && oresult.ApprovalCode.ToUpper().Contains("DUPLICATE"))
            {
                ResponseStatus = "DECLINED";
                oresult.Result = "DECLINED";

                responseError = string.Empty;
                responseError += "DECLINED";

                responseError += "  " + "DUPLICATE TRANSACTION";
            }
            logger.Trace("private bool ProcessWpCard(TransactionType tType, TransactionSubType tsType, PccCardInfo cardInfo, out TransactionResult oresult) - End");
            return true;
        }

        private bool WpProcessing(MMS.Device.WPDevice.WPData.PayTypes Paytype, MMS.Device.WPDevice.WPData.TransTypes TransType, PccCardInfo cardInfo)
        {
            bool isProcess = false;
            try
            {
                Dictionary<string, string> Response = null;
                Double tempAmount = Convert.ToDouble(cardInfo.transAmount);
                if (cardInfo.IsFSATransaction.ToUpper() == "TRUE")
                {
                    Double tempFsaAmount = Configuration.convertNullToDouble(cardInfo.transFSAAmount); //Healthcare amt
                    Double tempFsaRxAmount = Configuration.convertNullToDouble(cardInfo.transFSARxAmount);//rx pr amt
                    Payment.Amount.FSA.RxAmount = tempFsaRxAmount;
                    Payment.Amount.FSA.TotalHealthCareAmount = tempFsaAmount;
                }
                Payment.Amount.TotalAmount = tempAmount;

                switch (Paytype)
                {
                    case MMS.Device.WPDevice.WPData.PayTypes.Credit:
                        {
                            Payment.PayType = MMS.Device.WPDevice.WPData.PayTypes.Credit;
                            Payment.TransType = MMS.Device.WPDevice.WPData.TransTypes.Sale;
                            break;
                        }
                    case MMS.Device.WPDevice.WPData.PayTypes.Debit:
                        {
                            Payment.PayType = MMS.Device.WPDevice.WPData.PayTypes.Debit;
                            Payment.TransType = MMS.Device.WPDevice.WPData.TransTypes.Sale;
                            break;
                        }
                }
                frmWaitScreen ofrmwait = new frmWaitScreen(false, "Activate Device", "Payment Terminal activated.  Please wait...");
                ofrmwait.Show();
                Response = new Dictionary<string, string>();
                SigPadUtil.DefaultInstance.VF.ProcessPayment(Payment, WPAccount);
                while (SigPadUtil.DefaultInstance.VF.IsWPResponse == null)
                {
                    Application.DoEvents();
                    System.Threading.Thread.Sleep(100);
                    if (ofrmwait.IsDisposed)
                    {
                        SigPadUtil.DefaultInstance.ISCCancel = true;
                        break;
                    }
                }
                if (!SigPadUtil.DefaultInstance.ISCCancel)
                {
                    ofrmwait.Close();
                    Response = SigPadUtil.DefaultInstance.VF.ReturnResponse;
                    WPResponse resp = new WPResponse();
                    if (Response != null)
                    {
                        if (Payment.Amount.FSA.RxAmount > 0 || Payment.Amount.FSA.TotalHealthCareAmount > 0)
                        {
                            resp.isFSA = true;
                        }
                        ParseWpResponse(Response, ref resp);
                        responseStatus = resp.Status;
                        responseError += resp.Status;
                        WpRespInfo = resp;
                        SigPadUtil.DefaultInstance.VF.GetSignature = null;
                        SigPadUtil.DefaultInstance.WPCaptureSignature(resp);
                        isProcess = true;
                    }
                    else
                    {
                        isProcess = false;
                    }
                }
                else
                {
                    SigPadUtil.DefaultInstance.VF.WPCancelTransaction();
                    isProcess = false;
                    SigPadUtil.DefaultInstance.ISCCancel = false;
                }

            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "WpProcessing()");
                clsCoreUIHelper.ShowErrorMsg(ex.Message);
            }
            return isProcess;
        }

        #region ParseWpResponse
        private void ParseWpResponse(Dictionary<string, string> resp, ref WPResponse wpresp)
        {
            foreach (var r in resp)
            {
                if (r.Key.ToUpper() == "AUTHCODE")
                {
                    string[] Auth = r.Value.ToString().Split(':');
                    if (Auth.Length > 1)
                    {
                        wpresp.AuthCode = Auth[1];
                    }
                }
                else if (r.Key.ToUpper() == "TOTALAMT")
                {
                    wpresp.TotalAmt = r.Value;
                }
                else if (r.Key.ToUpper() == "HISTORYID")
                {
                    wpresp.HistoryID = r.Value;
                }
                else if (r.Key.ToUpper() == "ORDERID")
                {
                    wpresp.OrderID = r.Value;
                }
                else if (r.Key.ToUpper() == "BALANCE")
                {
                    wpresp.Balance = r.Value;
                }
                else if (r.Key.ToUpper() == "LAST4DIGITS")
                {
                    wpresp.Last4Digits = r.Value;
                }
                else if (r.Key.ToUpper() == "ENTRYMETHOD")
                {
                    wpresp.EntryMethod = r.Value;
                }
                else if (r.Key.ToUpper() == "PAYTYPE")
                {
                    wpresp.PayType = r.Value;
                }
                else if (r.Key.ToUpper() == "STATUS")
                {
                    wpresp.Status = r.Value;
                }
                else if (r.Key.ToUpper() == "PARTIALAPPROVAL")
                {
                    wpresp.PartialApproval = r.Value;
                }
                else if (r.Key.ToUpper() == "PAYMENTPROFILEID")
                {
                    wpresp.PaymentProfileID = r.Value;
                }
                else if (r.Key.ToUpper() == "RESULT")
                {
                    wpresp.Result = r.Value;
                }
            }
        }
        #endregion ParseWpResponse
        //Added By Dharmendra on Mar-12-09 
        //This method will add the user name in the dictionary
        private void AddUserID(Dictionary<string, string> cardPams)
        {
            /* Change by Manoj */
            switch (PAYMENTPROCESSOR.ToUpper().Trim())
            {
                case "XLINK":
                    {
                        cardPams.Add("USERID", Configuration.UserName);
                        cardPams.Add("XUSER", sUserName);
                        break;
                    }
                case "XCHARGE":
                case "HPSPAX":// PRIMEPOS-2761
                case "WORLDPAY":// PRIMEPOS-2761
                case "VANTIV":// PRIMEPOS-2761
                case "ELAVON":// PRIMEPOS-2761//2943
                    {
                        cardPams.Add("USERID", Configuration.UserName);
                        break;
                    }
                case "HPS":
                    {
                        cardPams.Add("USERID", sUserName);
                        break;
                    }
                case "PRIMERXPAY"://PRIMEPOS-2841
                    {
                        cardPams.Add("USERID", Configuration.UserName);
                        break;
                    }
            }
        }
        //Added Till Here Mar-12-09

        /// <summary>
        /// Author: Gaurav
        /// Mantis Id: 0000118
        /// Date: 01-Dec-2008
        /// </summary>
        /// <param name="ticketNum"></param>
        /// <param name="cardInfo"></param>
        public void PerformVoidOnCreditCardSales(string ticketNum, ref PccCardInfo cardInfo)
        {
            // Added By Dharmendra (SRT) Dec-08-08 to store request & response in the logger file
            primePosLogWriter.AppendCommentsToLogger("Entered into PerformVoidOnCreditCardSales() method\r\n--------------------------------------------------\r\nRequest Parameters & Their Values\r\n----------------------------------\r\n");
            //Added Till Here

            CultureInfo ci = CultureInfo.CurrentCulture;
            string tempManualFlage = string.Empty;
            string dictObj = string.Empty;
            string trnsdt = string.Empty;

            //ADDED PRASHANT 5 JUN 2010
            IsSecurityPromptRequired(Transactions.VoidOnCreditCardSales, cardParms);
            //END ADDED PRASHANT 5 JUN 2010

            if (PAYMENTPROCESSOR == "XLINK") // NileshJ 22_Feb_2020
            {
                if (cardInfo.tRoutId.Contains("|"))
                    cardInfo.tRoutId = cardInfo.tRoutId.Split('|')[0];
            }

            cardParms.Add(TROUTID, cardInfo.tRoutId);
            cardParms.Add(MMSCARD, cardInfo.cardType);
            cardParms.Add(TICKETNO, ticketNum);// NileshJ
                                               //Added By Dharmendra on Mar-12-09 to pass the user name in case of Payment Processor is either XCHARGE or XLINK
            cardParms.Add(ALLOWDUP, sALLOWDUP);//PRIMEPOS-2664
            AddUserID(cardParms);
            //Added Till Here
            cardParms.Add(PASSWORD, sPassword);
            cardParms.Add(TERMINALID, sTerminalID); //PrimePOS-2491 

            #region Comment
            //change by srt(Sachin) on 2-feb-2010
            //Double tempFsaAmount = !string.IsNullOrEmpty(cardInfo.transFSAAmount) ? Convert.ToDouble(cardInfo.transFSAAmount : Convert.ToDouble("0.00"); 
            //Double tempFsaRxAmount = Convert.ToDouble(cardInfo.transFSARxAmount);
            //Double otc_fsa_amt = Convert.ToDouble(cardInfo.transFSAAmount) - Convert.ToDouble(cardInfo.transFSARxAmount);
            //if ((cardInfo.IsFSATransaction != "0") &&(Convert.ToDecimal(cardInfo.transFSAAmount.ToString()) != 0))
            //{
            /*
            cardParms.Add("FSATRANSACTION", cardInfo.IsFSATransaction);
            cardParms.Add("FSAAMOUNT", tempFsaAmount.ToString("F", ci).PadRight(2, '0'));
            //if (otc_fsa_amt != 0)
            //{
            //    cardParms.Add("OTC_FSA_AMT", otc_fsa_amt.ToString("F", ci).PadRight(2, '0'));
            //}
            if (tempFsaRxAmount != 0)
            {
                cardParms.Add("FSARXAMOUNT", tempFsaRxAmount.ToString("F", ci).PadRight(2, '0'));
            }
             * */

            //}
            // End change by srt(Sachin) on 2-feb-2010
            #endregion Comment

            //PRIMEPOS-2528 (Suraj) 1-June-18 HPSPAX 
            if (PAYMENTPROCESSOR == "HPSPAX")
            {
                cardParms.Add("HOSTADDR", Configuration.CPOSSet.SigPadHostAddr);
                cardParms.Add("SITEID", Configuration.CSetting.SiteId);//2990
                cardParms.Add("DEVICEID", Configuration.CSetting.DeviceId);//2990
                cardParms.Add("DEVELOPERID", Configuration.CSetting.DeveloperId);//2990
                cardParms.Add("LICENSEID", Configuration.CSetting.LicenseId);//2990
                cardParms.Add("USERNAME", Configuration.CSetting.Username);//2990
                cardParms.Add("PASSWORD", Configuration.CSetting.Password);//2990
                cardParms.Add("VERSION", Configuration.CSetting.VersionNumber);//2990
            }

            //PRIMEPOS-2665 10-Apr-2019 JY Added
            if (PAYMENTPROCESSOR == "XLINK" || PAYMENTPROCESSOR == "HPSPAX" || PAYMENTPROCESSOR == "HPS" || PAYMENTPROCESSOR == "WORLDPAY" || PAYMENTPROCESSOR == "EVERTEC" || PAYMENTPROCESSOR == "VANTIV" || PAYMENTPROCESSOR == "PRIMERXPAY" || PAYMENTPROCESSOR == "ELAVON") // PRIMEPOS-2761 added  PAYMENTPROCESSOR == "HPSPAX" || PAYMENTPROCESSOR == "HPS" || PAYMENTPROCESSOR == "WORLDPAY" || PAYMENTPROCESSOR == "EVERTEC" || PAYMENTPROCESSOR == "VANTIV"//PRIMEPOS-2841 added by Arvind//2943
            {
                cardParms.Add("StationID", Configuration.StationID);
            }

            if (PAYMENTPROCESSOR == "XLINK" || PAYMENTPROCESSOR == "HPSPAX" || PAYMENTPROCESSOR == "HPS") // PRIMEPOS-2761
            {
                cardParms.Add("TerminalRefNumber", cardInfo.TerminalRefNumber);
            }
            if (PAYMENTPROCESSOR == "ELAVON" || PAYMENTPROCESSOR == "HPS" || PAYMENTPROCESSOR == "XLINK" || PAYMENTPROCESSOR == "EVERTEC") // PRIMEPOS-2761//2943
            {
                cardParms.Add(AMOUNT, cardInfo.transAmount);
            }
            //VANTIV Communincation Params PRIMEPOS-2636
            if (PAYMENTPROCESSOR == "VANTIV")
            {
                cardParms.Add("URL", Configuration.CPOSSet.SigPadHostAddr);
                cardParms.Add("APPLICATIONNAME", Configuration.ApplicationName);
                cardParms.Add("STATIONID", Configuration.StationID);
                cardParms.Add("AMOUNT", cardInfo.transAmount);//PRIMEPOS-2795
                cardParms.Add("TRIPOSPATH", Configuration.GetConfigFilePath());//PRIMEPOS-2895 Vantiv Arvind
                cardParms.Add("TransactionRecover", cardInfo.isTransactionRecover.ToString()); //PRIMEPOS-3156
                cardParms.Add("TRANSACTIONURL", Configuration.oMerchantConfig.VantivTokenUrl);//PRIMEPOS-3156
                cardParms.Add("REPORTURL", Configuration.oMerchantConfig.VantivReportUrl);//PRIMEPOS-3156
            }
            #region PRIMEPOS-2841
            if (PAYMENTPROCESSOR == "PRIMERXPAY")
            {
                if (cardInfo.IsVoidCustomerDriven)
                {
                    if (cardInfo.IsVoidPayComplete)
                    {
                        cardParms.Add("ISVOIDPAYCOMP", cardInfo.IsVoidPayComplete ? "true" : "false");
                        if (!string.IsNullOrWhiteSpace(cardInfo.TransactionID))
                            cardParms.Add(TRANSID, cardInfo.TransactionID);
                        else
                            cardParms.Add(TRANSID, cardInfo.tRoutId);
                    }
                    else
                    {
                        cardParms.Add("ISVOIDCDRIVEN", cardInfo.IsVoidCustomerDriven ? "true" : "false");
                        if (!string.IsNullOrWhiteSpace(cardInfo.TransactionID))
                            cardParms.Add(TRANSID, cardInfo.TransactionID);
                        else
                            cardParms.Add(TRANSID, cardInfo.tRoutId);
                    }
                }
                else if (cardInfo.IsVoidPayComplete)
                {
                    cardParms.Add("ISVOIDPAYCOMP", cardInfo.IsVoidPayComplete ? "true" : "false");
                    if (!string.IsNullOrWhiteSpace(cardInfo.TransactionID))
                        cardParms.Add(TRANSID, cardInfo.TransactionID);
                    else
                        cardParms.Add(TRANSID, cardInfo.tRoutId);
                }
                else
                {
                    if (!string.IsNullOrWhiteSpace(cardInfo.TransactionID))
                        cardParms.Add(TRANSID, cardInfo.TransactionID);
                    else if (!string.IsNullOrWhiteSpace(cardInfo.tRoutId))
                        cardParms.Add(TRANSID, cardInfo.tRoutId);
                }
                cardParms.Add("PHARMACYNO", sPharmacyNumber);

                cardParms.Add("URL", Configuration.CSetting.PrimeRxPayUrl + Configuration.CSetting.PrimerxPayExtensionUrl);//2956
                cardParms.Add("PASSWORD", Configuration.CSetting.PrimeRxPaySecretKey);
                cardParms.Add("APIKEY", Configuration.CSetting.PrimeRxPayClientId);
                cardParms.Add("PAYPROVIDERID", Configuration.CSetting.PayProviderID.ToString());//PRIMEPOS-2902
            }
            #endregion
            if (PAYMENTPROCESSOR == "ELAVON")//2943
            {
                cardParms.Add("TERMINALID", Configuration.CSetting.TerminalID);
                cardParms.Add("CHAINCODE", Configuration.CSetting.ChainCode);
                cardParms.Add("LOCATIONNAME", Configuration.CSetting.LocationName);
                //cardParms.Add("USERDATA", cardInfo.UserData);
                cardParms.Add("ISALLOWDUP", cardInfo.isAllowDuplicate == true ? "true" : "false");
                cardParms.Add(EXPDATE, cardInfo.cardExpiryDate);
            }
            try
            {
                resp = null;//Added by Arvind 
                pccRespInfo = null;//Added by Arvind 
                if (PAYMENTPROCESSOR.ToUpper() != "WORLDPAY")
                {
                    primePosLogWriter.AppendPaymentRequestToLogger(cardParms); //Added By Dharmendra on Dec-08-08 to append the request parameter values in logger.                     
                    String status = creditProc.VoidSale(ref cardParms, out resp);
                    PccResponse objCCResponse = new PccResponse();
                    DecipherResponse(resp, ref objCCResponse);
                    //PRIMEPOS-2578 Added CardHolderName Response - Discover Specific -16AUg2018 - NILESHJ
                    if (PAYMENTPROCESSOR == "HPSPAX")
                    {
                        if (resp != null) // NileshJ - 12-Dec-2018
                        {
                            string ccHolder = string.Empty;
                            resp.GetAllKeys().TryGetValue("CARDHOLDER", out ccHolder);
                            //if (objCCResponse.CardType.ToUpper() == SecureSubmit.Terminals.PAX.CardType.DiSCOVER.ToString().ToUpper())
                            //{
                            cardInfo.cardHolderName = ccHolder;
                            //}
                            cardParms.Add("TransactionRecover", cardInfo.isTransactionRecover.ToString()); // SAJID LOCAL DETAILS REPORT PRIMEPOS-2862
                        }
                    }
                    PrintVoidReceipt(objCCResponse, resp);//2664
                    responseStatus = status;
                    responseError = objCCResponse.ResultDescription;
                    objCCResponse.ResponseStatus = status;
                    pccRespInfo = objCCResponse;
                }
                else
                {
                    #region PRIMEPOS-2761
                    logger.Trace("Orphan - Start1");
                    cardInfo.StationID = Configuration.StationID;
                    cardInfo.UserId = Configuration.UserName;
                    cardInfo.TicketNo = ticketNum;
                    logger.Trace("Orphan - End1");
                    #endregion
                    TransactionResult resp = new TransactionResult();
                    bool result = ProcessWPReverse(TransactionType.Void, cardInfo, out resp);
                    WP_TransResult = resp;

                    PccPaymentSvr.DefaultInstance.EntryMethod = (string.IsNullOrEmpty(WP_TransResult.ResponseTags.EntryMethod) ? WP_TransResult.EntryMethod : WP_TransResult.ResponseTags.EntryMethod);
                }

            }

            catch (Exception ex)
            {
                logger.Fatal(ex, "PerformVoidOnCreditCardSales()");
                responseError = ex.ToString();
                pccRespInfo = null;//Added by Arvind 
                resp = null;//Added by Arvind
                //Added By Dharmendra (SRT) to logg the error
                primePosLogWriter.AppendCommentsToLogger(responseError + "\r\n");
            }
            finally
            {
                cardParms.Clear();

            }
            // Added By Dharmendra (SRT) Dec-08-08
            primePosLogWriter.AppendCommentsToLogger("Exit From PerformVoidOnCreditCardSales() method\r\n----------------------------------------------\r\n");
            //Added Till Here
        }

        /// <summary>
        /// Author: Gaurav
        /// Mantis Id: 0000118
        /// Date: 01-Dec-2008
        /// </summary>
        /// <param name="ticketNum"></param>
        /// <param name="cardInfo"></param>
        public void PerformVoidOnCreditCardSalesReturn(string ticketNum, ref PccCardInfo cardInfo)
        {
            // Added By Dharmendra (SRT) Dec-08-08 to store request & response in the logger file
            primePosLogWriter.AppendCommentsToLogger("Entered into PerformVoidOnCreditCardSalesReturn() method\r\n--------------------------------------------------------\r\nRequest Parameters & Their Values\r\n----------------------------------\r\n");
            //Added Till Here
            CultureInfo ci = CultureInfo.CurrentCulture;
            string tempManualFlage = string.Empty;
            string dictObj = string.Empty;
            string trnsdt = string.Empty;

            //ADDED PRASHANT 5 JUN 2010
            IsSecurityPromptRequired(Transactions.VoidOnCreditCardSalesReturn, cardParms);
            //END ADDED PRASHANT 5 JUN 2010

            if (PAYMENTPROCESSOR == "XLINK") // NileshJ 22_Feb_2020
            {
                if (cardInfo.tRoutId.Contains("|"))
                    cardInfo.tRoutId = cardInfo.tRoutId.Split('|')[0];
            }

            cardParms.Add(TROUTID, cardInfo.tRoutId);
            cardParms.Add(MMSCARD, cardInfo.cardType);
            cardParms.Add(TICKETNO, ticketNum);// NileshJ
            //Added By Dharmendra on Mar-12-09 to pass the user name in case of Payment Processor is either XCHARGE or XLINK
            AddUserID(cardParms);
            //Added Till Here
            cardParms.Add(PASSWORD, sPassword);

            //PRIMEPOS-2528 (Suraj) 1-June-18
            if (PAYMENTPROCESSOR == "HPSPAX")
            {
                cardParms.Add("HOSTADDR", Configuration.CPOSSet.SigPadHostAddr);
            }

            //PRIMEPOS-2665 10-Apr-2019 JY Added
            if (PAYMENTPROCESSOR == "XLINK" || PAYMENTPROCESSOR == "HPSPAX" || PAYMENTPROCESSOR == "HPS" || PAYMENTPROCESSOR == "WORLDPAY" || PAYMENTPROCESSOR == "EVERTEC" || PAYMENTPROCESSOR == "VANTIV" || PAYMENTPROCESSOR == "PRIMERXPAY" || PAYMENTPROCESSOR == "ELAVON") // PRIMEPOS-2761 - 2753//PRIMEPOS-2841 Added by Arvind//2943
            {
                cardParms.Add("StationID", Configuration.StationID);
            }
            //VANTIV Communincation Params PRIMEPOS-2636
            if (PAYMENTPROCESSOR == "VANTIV")
            {
                cardParms.Add("URL", Configuration.CPOSSet.SigPadHostAddr);
                cardParms.Add("APPLICATIONNAME", Configuration.ApplicationName);
                cardParms.Add("STATIONID", Configuration.StationID);
                cardParms.Add("AMOUNT", cardInfo.transAmount);//PRIMEPOS-2795
                cardParms.Add("TRIPOSPATH", Configuration.GetConfigFilePath());//PRIMEPOS-2895 Vantiv Arvind
                //cardParms.Add("lane", Configuration.CPOSSet.SigPadHostAddr);
                cardParms.Add("TransactionRecover", cardInfo.isTransactionRecover.ToString()); //PRIMEPOS-3156
                cardParms.Add("REPORTURL", Configuration.oMerchantConfig.VantivReportUrl); //PRIMEPOS-3156
            }
            #region PRIMEPOS-2841
            if (PAYMENTPROCESSOR == "PRIMERXPAY")
            {
                if (!string.IsNullOrWhiteSpace(cardInfo.tRoutId))
                    cardParms.Add(TRANSID, cardInfo.tRoutId);
                else if (!string.IsNullOrWhiteSpace(cardInfo.TransactionID))
                    cardParms.Add(TRANSID, cardInfo.TransactionID);
                cardParms.Add("PHARMACYNO", sPharmacyNumber);

                cardParms.Add("URL", Configuration.CSetting.PrimeRxPayUrl + Configuration.CSetting.PrimerxPayExtensionUrl);//2956
                cardParms.Add("PASSWORD", Configuration.CSetting.PrimeRxPaySecretKey);
                cardParms.Add("APIKEY", Configuration.CSetting.PrimeRxPayClientId);
                cardParms.Add("PAYPROVIDERID", Configuration.CSetting.PayProviderID.ToString());//PRIMEPOS-2902
            }
            #endregion
            if (Configuration.CPOSSet.PaymentProcessor == "ELAVON")
            {
                cardParms.Add("TERMINALID", Configuration.CSetting.TerminalID);
                cardParms.Add("CHAINCODE", Configuration.CSetting.ChainCode);
                cardParms.Add("LOCATIONNAME", Configuration.CSetting.LocationName);
                //cardParms.Add("USERDATA", cardInfo.UserData);
                cardParms.Add(AMOUNT, cardInfo.transAmount);
                cardParms.Add("ISALLOWDUP", cardInfo.isAllowDuplicate == true ? "true" : "false");
                cardParms.Add(EXPDATE, cardInfo.cardExpiryDate);
            }
            try
            {
                resp = null;//Added by Arvind 
                pccRespInfo = null;//Added by Arvind 
                if (PAYMENTPROCESSOR.ToUpper() != "WORLDPAY")
                {
                    primePosLogWriter.AppendPaymentRequestToLogger(cardParms); // Added By Dharmendra on Dec-09-08 to write request parameter's value to logger                     
                    String status = creditProc.VoidCredit(ref cardParms, out resp);
                    PccResponse objCCResponse = new PccResponse();
                    DecipherResponse(resp, ref objCCResponse);
                    //PRIMEPOS-2578 Added CardHolderName Response - Discover Specific -16AUg2018 - NILESHJ
                    if (PAYMENTPROCESSOR == "HPSPAX" && resp != null)//PRIMEPOS-3090
                    {
                        string ccHolder = string.Empty;
                        resp.GetAllKeys().TryGetValue("CARDHOLDER", out ccHolder);
                        //if (objCCResponse.CardType.ToUpper() == SecureSubmit.Terminals.PAX.CardType.DiSCOVER.ToString().ToUpper())
                        //{
                        cardInfo.cardHolderName = ccHolder;
                        //}
                        cardParms.Add("TransactionRecover", cardInfo.isTransactionRecover.ToString()); // SAJID LOCAL DETAILS REPORT PRIMEPOS-2862
                    }
                    PrintVoidReceipt(objCCResponse, resp);//2664
                    responseStatus = status;
                    responseError = objCCResponse.ResultDescription;
                    objCCResponse.ResponseStatus = status;
                    pccRespInfo = objCCResponse;
                }
                else
                {
                    #region PRIMEPOS-2761
                    logger.Trace("Orphan - Start1");
                    cardInfo.StationID = Configuration.StationID;
                    cardInfo.UserId = Configuration.UserName;
                    cardInfo.TicketNo = ticketNum;
                    logger.Trace("Orphan - End1");
                    #endregion
                    TransactionResult resp = new TransactionResult();
                    bool result = ProcessWPReverse(TransactionType.Void, cardInfo, out resp);
                    WP_TransResult = resp;

                    PccPaymentSvr.DefaultInstance.EntryMethod = (string.IsNullOrEmpty(WP_TransResult.ResponseTags.EntryMethod) ? WP_TransResult.EntryMethod : WP_TransResult.ResponseTags.EntryMethod);

                    #region PRIMEPOS-2761
                    try
                    {
                        using (var db = new Possql())
                        {
                            string Ticketnumber = cardInfo.TicketNo;
                            CCTransmission_Log cclog = new CCTransmission_Log();
                            //cclog = db.CCTransmission_Logs.Where(w => w.TicketNo == Ticketnumber).SingleOrDefault();
                            cclog = db.CCTransmission_Logs.OrderByDescending(r => r.TransNo).Where(w => w.TicketNo.Contains(Ticketnumber)).Take(1).SingleOrDefault();
                            //cclog.HostTransID = cardInfo.OrderID + "|" + cardInfo.TransactionID;
                            cclog.ResponseMessage = WP_TransResult.Result;
                            db.CCTransmission_Logs.Attach(cclog);
                            //db.Entry(cclog).Property(p => p.HostTransID).IsModified = true;
                            db.Entry(cclog).Property(p => p.ResponseMessage).IsModified = true;
                            db.SaveChanges();
                        }
                    }
                    catch (Exception exep)
                    {
                        logger.Fatal(exep, exep.Message);
                    }
                    #endregion
                }
            }

            catch (Exception ex)
            {
                logger.Fatal(ex, "PerformVoidOnCreditCardSalesReturn()");
                responseError = ex.ToString();
                pccRespInfo = null;//Added by Arvind 
                resp = null;//Added by Arvind
                //Added By Dharmendra (SRT) to logg the error
                primePosLogWriter.AppendCommentsToLogger(responseError + "\r\n");
                //Logs.Logger(clsPOSDBConstants.Log_Module_Transaction, "RxIsOnHold()", ex.ToString());
            }
            finally
            {
                cardParms.Clear();

            }
            // Added By Dharmendra (SRT) Dec-08-08
            primePosLogWriter.AppendCommentsToLogger("Exit From PerformVoidOnCreditCardSales() method\r\n----------------------------------------------\r\n");
            //Added Till Here
        }


        /// <summary>
        /// Author : Dharmendra
        /// Functionality Description : This method performs credit card sales return
        /// operation
        /// Known Bugs : None
        /// Start Date : 03-09-08
        /// </summary>
        /// <param name="ticketNum"></param>
        /// <param name="transAmt"></param>

        public void PerformCreditSalesReturn(string ticketNum, ref PccCardInfo cardInfo)
        {
            // Added By Dharmendra (SRT) Dec-08-08 to store request & response in the logger file
            primePosLogWriter.AppendCommentsToLogger("Entered into PerformCreditSalesReturn() method\r\n---------------------------------------------\r\nRequest Parameters & Their Values\r\n----------------------------------\r\n");
            //Added Till Here
            CultureInfo ci = CultureInfo.CurrentCulture;
            string tempManualFlage = string.Empty;
            string dictObj = string.Empty;
            string trnsdt = string.Empty;

            Double tempAmount = Convert.ToDouble(cardInfo.transAmount);
            Double tempFsaAmount = string.IsNullOrEmpty(cardInfo.transFSAAmount) ? Convert.ToDouble("0.00") : Convert.ToDouble(cardInfo.transFSAAmount);
            Double tempFsaRxAmount = string.IsNullOrEmpty(cardInfo.transFSARxAmount) ? Convert.ToDouble("0.00") : Convert.ToDouble(cardInfo.transFSARxAmount);//rx pr amt

            //Double otc_fsa_amt = 0;

            //ADDED PRASHANT 5 JUN 2010
            IsSecurityPromptRequired(Transactions.CreditSalesReturn, cardParms);
            //END ADDED PRASHANT 5 JUN 2010

            cardParms.Add(ACCOUNTNUM, cardInfo.cardNumber);
            cardParms.Add(EXPDATE, cardInfo.cardExpiryDate);
            cardParms.Add(AMOUNT, tempAmount.ToString("F", ci).PadRight(2, '0'));
            cardParms.Add(TIMEOUT, cardInfo.txnTimeOut);
            cardParms.Add(MMSCARD, cardInfo.cardType);
            cardParms.Add(ZIP, cardInfo.zipCode);
            cardParms.Add(ADDRESS, cardInfo.customerAddress);
            cardParms.Add(CARDHOLDER, cardInfo.cardHolderName);
            cardParms.Add(TICKETNO, ticketNum);// NileshJ
            //Added By Dharmendra on Mar-12-09 to pass the user name in case of Payment Processor is either XCHARGE or XLINK
            AddUserID(cardParms);
            //Added Till Here
            //ADDED For HPS
            cardParms.Add(PASSWORD, sPassword);
            cardParms.Add(ALLOWDUP, sALLOWDUP);
            //cardParms.Add(DIRECTMKTSHIPMONTH, "09");
            //cardParms.Add(DIRECTMKTSHIPDAY, "2");
            //PrimePOS-2491 
            cardParms.Add(TERMINALID, sTerminalID);
            // Below code is already there in the function SAJID
            //if (cardInfo.UseToken && PAYMENTPROCESSOR.ToUpper().Trim() == "XLINK")
            //{
            //    cardParms.Add("XCACCOUNTID", cardInfo.ProfileID);
            //}
            //END
            #region PRIMEPOS-2738 ADDED BY ARVIND FOR REVERSAL
            if (PAYMENTPROCESSOR == "HPSPAX")
            {
                cardParms.Add(HREFNUMBER, cardInfo.HrefNumber);//Arvind PRIMEPOS-2738
                cardParms.Add(ORIGINALTRANSID, cardInfo.TransactionID);//Arvind PRIMEPOS-2738
            }
            if (PAYMENTPROCESSOR == "OPENEDGE")
            {
                cardParms.Add(ORIGINALTRANSID, cardInfo.TransactionID);//Arvind PRIMEPOS-2738
            }
            if (PAYMENTPROCESSOR == "HPS")
            {
                cardParms.Add(TRANSID, cardInfo.TransactionID);
            }
            if (PAYMENTPROCESSOR == "XLINK")
            {
                cardParms.Add("REFERENCEORDERID", cardInfo.TransactionID);
            }
            #region PRIMEPOS-2841
            if (PAYMENTPROCESSOR == "PRIMERXPAY")
            {
                cardParms.Add(TRANSID, cardInfo.TransactionID);
            }
            #endregion
            #endregion
            //if (cardInfo.UseToken && (PAYMENTPROCESSOR.ToUpper().Trim() == "XLINK" || PAYMENTPROCESSOR == "HPSPAX"))  //Strict Sales Return Tockanization issue HPS PAX //SAJID NILESH
            //{
            //    cardParms.Add("XCACCOUNTID", cardInfo.ProfileID);
            //}
            ////END

            #region //PRIMEPOS-3146
            if (cardInfo.UseToken && PAYMENTPROCESSOR.ToUpper().Trim() == "XLINK")  //Strict Sales Return Tockanization issue HPS PAX //SAJID NILESH
            {
                cardParms.Add("XCACCOUNTID", cardInfo.ProfileID);
            }

            if (cardInfo.UseToken && PAYMENTPROCESSOR == "HPSPAX")
            {
                if (string.IsNullOrWhiteSpace(cardInfo.OriginalTransactionIdentifier))
                {
                    cardParms.Add("XCACCOUNTID", cardInfo.ProfileID);
                }
                else
                {
                    cardParms.Add("XCACCOUNTID", $"{cardInfo.ProfileID}|{cardInfo.OriginalTransactionIdentifier}");
                }
            }
            #endregion
            if (cardInfo.trackII.Equals(string.Empty))
            {
                cardParms.Add(MANUALFLAG, "0");
            }
            else
            {
                if (PAYMENTPROCESSOR.ToUpper().Trim() == "HPS")
                {
                    cardParms.Add(TRACK, cardInfo.Completetrack);
                }
                else
                {
                    cardParms.Add(TRACK, cardInfo.trackII);
                }
                cardParms.Add(MANUALFLAG, "1");
            }

            //Added By SRT(Gaurav) Date : 20-NOV-2008
            if (tempFsaAmount != 0)
            {
                //otc_fsa_amt = tempFsaAmount - tempFsaRxAmount;

                cardParms.Add("FSATRANSACTION", cardInfo.IsFSATransaction);
                cardParms.Add("FSAAMOUNT", tempFsaAmount.ToString("F", ci).PadRight(2, '0'));

                /*if (otc_fsa_amt != 0)
                {
                    cardParms.Add("OTC_FSA_AMT", otc_fsa_amt.ToString("F", ci).PadRight(2, '0'));
                }
                 * */
                if (tempFsaRxAmount != 0)
                {
                    cardParms.Add("FSARXAMOUNT", tempFsaRxAmount.ToString("F", ci).PadRight(2, '0'));
                }
            }
            //End Of Added By SRT(Gaurav)

            //PRIMEPOS-2528 (Suraj) 1-June-18 HPSPAX
            if (PAYMENTPROCESSOR == "HPSPAX")
            {
                cardParms.Add("HOSTADDR", Configuration.CPOSSet.SigPadHostAddr);
                cardParms.Add("TransactionRecover", cardInfo.isTransactionRecover.ToString()); // SAJID LOCAL DETAILS REPORT PRIMEPOS-2862
            }

            #region PRIMEPOS-2784 EVERTEC
            if (PAYMENTPROCESSOR.ToUpper() == "EVERTEC")
            {
                cardParms.Add("ISMANUAL", cardInfo.isManual == true ? "true" : "false");//PRIMEPOS-2805
                cardParms.Add("ISALLOWDUP", cardInfo.isAllowDuplicate == true ? "true" : "false");
                cardParms.Add("TAXDETAIL", cardInfo.EvertecTaxDetails);//2664
            }
            #endregion

            //PRIMEPOS-2665 10-Apr-2019 JY Added
            if (PAYMENTPROCESSOR == "XLINK" || PAYMENTPROCESSOR == "HPASPAX" || PAYMENTPROCESSOR == "HPS" || PAYMENTPROCESSOR == "WORLDPAY" || PAYMENTPROCESSOR == "EVERTEC" || PAYMENTPROCESSOR == "VANTIV" || PAYMENTPROCESSOR == "PRIMERXPAY" || PAYMENTPROCESSOR == "ELAVON") // PRIMEPOS-2761 //PRIMEPOS-2841 added by Arvind//2943
            {
                cardParms.Add("StationID", Configuration.StationID);
            }
            //VANTIV Communincation Params PRIMEPOS-2636
            if (PAYMENTPROCESSOR == "VANTIV")
            {
                cardParms.Add(ISNBSTRANSACTION, cardInfo.isNBSTransaction.ToString());
                cardParms.Add("URL", Configuration.CPOSSet.SigPadHostAddr);
                cardParms.Add("APPLICATIONNAME", Configuration.ApplicationName);
                cardParms.Add("STATIONID", Configuration.StationID);
                cardParms.Add(TRANSID, cardInfo.TransactionID);
                cardParms.Add("TRIPOSPATH", Configuration.GetConfigFilePath());//PRIMEPOS-2895 Vantiv Arvind
                //cardParms.Add("lane", Configuration.CPOSSet.SigPadHostAddr);
                cardParms.Add("TransactionRecover", cardInfo.isTransactionRecover.ToString()); //PRIMEPOS-3156
                cardParms.Add("TRANSACTIONURL", Configuration.oMerchantConfig.VantivTokenUrl);//PRIMEPOS-3156
                cardParms.Add("REPORTURL", Configuration.oMerchantConfig.VantivReportUrl);//PRIMEPOS-3156
                if (Configuration.CSetting.StrictReturn) //PRIMEPOS-3521 //PRIMEPOS-3522 //PRIMEPOS-3504
                {
                    cardParms.Add("RETURNTRANSTYPE", cardInfo.returnTransType);
                }
            }

            #region primepos-2841
            if (PAYMENTPROCESSOR == "PRIMERXPAY")
            {
                cardParms.Add("PHARMACYNO", sPharmacyNumber);
                cardParms.Add("URL", Configuration.CSetting.PrimeRxPayUrl + Configuration.CSetting.PrimerxPayExtensionUrl);//2956
                cardParms.Add("PASSWORD", Configuration.CSetting.PrimeRxPaySecretKey);
                cardParms.Add("APIKEY", Configuration.CSetting.PrimeRxPayClientId);
                cardParms.Add("PAYPROVIDERID", Configuration.CSetting.PayProviderID.ToString());//PRIMEPOS-2902
            }
            #endregion
            if (PAYMENTPROCESSOR == "ELAVON")//2943
            {
                cardParms.Add("TERMINALID", Configuration.CSetting.TerminalID);
                cardParms.Add("CHAINCODE", Configuration.CSetting.ChainCode);
                cardParms.Add("LOCATIONNAME", Configuration.CSetting.LocationName);
                cardParms.Add(TRANSID, cardInfo.TransactionID);
                cardParms.Add("ISALLOWDUP", cardInfo.isAllowDuplicate == true ? "true" : "false");
                cardParms.Add("TRANSDATE", cardInfo.TransDate);
                //cardParms.Add("StationID", Configuration.StationID);
            }
            try
            {
                resp = null;//Added by Arvind 
                pccRespInfo = null;//Added by Arvind 
                if (PAYMENTPROCESSOR.ToUpper() != "WORLDPAY")
                {
                    primePosLogWriter.AppendPaymentRequestToLogger(cardParms); // Added By Dharmendra on Dec-09-08 to write request parameter's value to logger                     
                    String status = creditProc.Credit(ref cardParms, out resp);
                    PccResponse objCCResponse = new PccResponse();
                    DecipherResponse(resp, ref objCCResponse);
                    #region PRIMEPOS-2841
                    if (PAYMENTPROCESSOR.ToUpper() == "PRIMERXPAY")
                    {
                        if (!string.IsNullOrWhiteSpace(resp.Result))
                            clsCoreUIHelper.ShowOKMsg(resp.Result);
                    }
                    #endregion
                    //PRIMEPOS-2578 Added CardHolderName Response - Discover Specific -16AUg2018 - NILESHJ
                    if (PAYMENTPROCESSOR == "HPSPAX" && resp != null)//PRIMEPOS-3090
                    {
                        string ccHolder = string.Empty;
                        resp.GetAllKeys().TryGetValue("CARDHOLDER", out ccHolder);
                        //if (objCCResponse.CardType.ToUpper() == SecureSubmit.Terminals.PAX.CardType.DiSCOVER.ToString().ToUpper())
                        //{
                        cardInfo.cardHolderName = ccHolder;
                        //}
                    }
                    responseStatus = status;
                    responseError = objCCResponse.ResultDescription;
                    objCCResponse.ResponseStatus = status;
                    pccRespInfo = objCCResponse;
                }
                else
                {
                    #region PRIMEPOS-2761
                    cardInfo.StationID = Configuration.StationID;
                    cardInfo.UserId = Configuration.UserName;
                    cardInfo.TicketNo = ticketNum;
                    #endregion
                    bool isSale;
                    TransactionResult resp = new TransactionResult();
                    if (Configuration.CSetting.StrictReturn != true)
                    {
                        isSale = ProcessWpCard(TransactionType.Credit, TransactionSubType.None, cardInfo, out resp); //PRIMEPOS-2738 IF STRICT RETURN IS FALSE THAN CREDIT
                    }
                    else
                    {
                        isSale = ProcessWpCard(TransactionType.Refund, TransactionSubType.None, cardInfo, out resp); //PRIMEPOS-2738  txnType to be Refund not Credit
                    }
                    WP_TransResult = resp;
                    if (WP_TransResult.Result.Equals("SUCCESS")) // NileshJ - PRIMEPOS-2697
                    {
                        PccPaymentSvr.DefaultInstance.EntryMethod = (string.IsNullOrEmpty(WP_TransResult.ResponseTags.EntryMethod) ? WP_TransResult.EntryMethod : WP_TransResult.ResponseTags.EntryMethod);

                        if (!string.IsNullOrEmpty(WP_TransResult.Token))
                        {
                            PccPaymentSvr.DefaultInstance.ProfiledID = WP_TransResult.Token;

                        }

                        if (WP_TransResult.ResponseTags.CashBack > 0)
                        {
                            PccPaymentSvr.DefaultInstance.CashBack = WP_TransResult.ResponseTags.CashBack.ToString();
                        }
                        cardInfo.OrderID = WP_TransResult.OrderID;
                        cardInfo.TransactionID = WP_TransResult.TransactionID;
                        #region PRIMEPOS-2761
                        try
                        {
                            using (var db = new Possql())
                            {
                                string Ticketnumber = cardInfo.TicketNo;
                                CCTransmission_Log cclog = new CCTransmission_Log();
                                cclog = db.CCTransmission_Logs.Where(w => w.TicketNo == Ticketnumber).SingleOrDefault();
                                cclog.HostTransID = cardInfo.OrderID + "|" + cardInfo.TransactionID;
                                cclog.ResponseMessage = WP_TransResult.Result;
                                db.CCTransmission_Logs.Attach(cclog);
                                db.Entry(cclog).Property(p => p.HostTransID).IsModified = true;
                                db.Entry(cclog).Property(p => p.ResponseMessage).IsModified = true;
                                db.SaveChanges();
                            }
                        }
                        catch (Exception exep)
                        {
                            logger.Fatal(exep, exep.Message);
                        }
                        #endregion
                    }
                    else
                    {
                        PccPaymentSvr.DefaultInstance.EntryMethod = "";
                        #region PRIMEPOS-2761
                        try
                        {
                            using (var db = new Possql())
                            {
                                string Ticketnumber = cardInfo.TicketNo;
                                CCTransmission_Log cclog = new CCTransmission_Log();
                                //cclog = db.CCTransmission_Logs.Where(w => w.TicketNo == Ticketnumber).SingleOrDefault();
                                cclog = db.CCTransmission_Logs.OrderByDescending(r => r.TransNo).Where(w => w.TicketNo.Contains(Ticketnumber)).Take(1).SingleOrDefault();
                                //cclog.HostTransID = cardInfo.OrderID + "|" + cardInfo.TransactionID;
                                cclog.ResponseMessage = WP_TransResult.Result;
                                db.CCTransmission_Logs.Attach(cclog);
                                //db.Entry(cclog).Property(p => p.HostTransID).IsModified = true;
                                db.Entry(cclog).Property(p => p.ResponseMessage).IsModified = true;
                                db.SaveChanges();
                            }
                        }
                        catch (Exception exep)
                        {
                            logger.Fatal(exep, exep.Message);
                        }
                        #endregion
                    }
                }

            }

            catch (Exception ex)
            {
                logger.Fatal(ex, "PerformCreditSalesReturn()");
                responseError = ex.ToString();
                //Added By Dharmendra (SRT) to logg the error
                pccRespInfo = null;//Added by Arvind 
                resp = null;//Added by Arvind
                primePosLogWriter.AppendCommentsToLogger(responseError + "\r\n");
            }
            finally
            {
                cardParms.Clear();

            }
            //Added By Dharmendra (SRT) on Dec-08-08
            primePosLogWriter.AppendCommentsToLogger("Exit From  PerformCreditSalesReturn() method\r\n------------------------------------\r\n");
            //Added Till Here
        }


        /* Added By Manoj. This is for EBT Implementation - Xlink only with Verifone MX870 PinPad */
        /// <summary>
        /// Author : Manoj
        /// Functionality Description : This is for EBT(Sales) Implementation - Xlink only with Verifone MX870 PinPad 
        /// known Bugs : None
        /// Start Date : 08/24/2011
        /// </summary>
        /// <param name="ticketNum"></param>
        /// <param name="transAmt"></param>
        public void PerfomEBTSale(string ticketNum, ref PccCardInfo cardInfo)
        {
            primePosLogWriter.AppendCommentsToLogger("Entered into PerformEBTSale() method\r\n--------------------------------------\r\nRequest Parameters & Their Values\r\n----------------------------------\r\n");
            CultureInfo ci = CultureInfo.CurrentCulture;
            string tempManualFlage = string.Empty;
            string dictObj = string.Empty;
            string trnsdt = string.Empty;
            string track2CleanedData = string.Empty;
            string track2Cleaned = string.Empty;
            Double tempAmount = Convert.ToDouble(cardInfo.transAmount);

            if (PAYMENTPROCESSOR == "HPS")
            {

                if (cardInfo.trackII.Equals(string.Empty))
                {
                    cardParms.Add(MANUALFLAG, "0");
                }
                else
                {
                    cardParms.Add(MANUALFLAG, "1");
                    cardParms.Add(TRACK, cardInfo.Completetrack);
                }
                cardParms.Add(ACCOUNTNUM, cardInfo.cardNumber);
                cardParms.Add(EXPDATE, cardInfo.cardExpiryDate);
                cardParms.Add(CARDHOLDER, cardInfo.cardHolderName);
                cardParms.Add(AMOUNT, tempAmount.ToString("F", ci).PadRight(2, '0'));
                cardParms.Add(TOTALAMOUNT, tempAmount.ToString("F", ci).PadRight(2, '0'));
                cardParms.Add(DEBITPINNO, cardInfo.pinNumber);
                cardParms.Add(ALLOWDUP, sALLOWDUP);
                cardParms.Add(PASSWORD, sPassword);
                AddUserID(cardParms);
                cardParms.Add(MMSCARD, cardInfo.cardType);
            }
            else
            {

                cardParms.Add(ACCOUNTNUM, cardInfo.cardNumber);
                cardParms.Add(EXPDATE, cardInfo.cardExpiryDate);
                cardParms.Add(DEBITPINNO, cardInfo.pinNumber);
                cardParms.Add(DEBITKEYNO, cardInfo.keySerialNumber);
                cardParms.Add(AMOUNT, tempAmount.ToString("F", ci).PadRight(2, '0'));
                cardParms.Add(TRACK, cardInfo.trackII);
                if (PAYMENTPROCESSOR.ToUpper() == "EVERTEC")
                    cardParms.Add(ALLOWDUP, sALLOWDUP);//PRIMEPOS-2664 ADDED BY ARVIND EVERTEC
                cardParms.Add(CARDHOLDER, cardInfo.cardHolderName);
                cardParms.Add(TICKETNO, ticketNum);
                AddUserID(cardParms);
                cardParms.Add(PASSWORD, sPassword);
                if (cardInfo.trackII.Equals(string.Empty))
                {
                    cardParms.Add(MANUALFLAG, "0");
                }
                else
                {
                    cardParms.Add(MANUALFLAG, "1");
                }

                cardParms.Add(TIMEOUT, cardInfo.txnTimeOut);
                cardParms.Add(MMSCARD, cardInfo.cardType);
                cardParms.Add(TOTALAMOUNT, tempAmount.ToString("F", ci).PadRight(2, '0'));
                cardParms.Add(TERMINALID, sTerminalID);//PrimePOS-2491 
            }

            //PRIMEPOS-2528 (Suraj) 1-June-18
            if (PAYMENTPROCESSOR == "HPSPAX")
            {
                cardParms.Add("HOSTADDR", Configuration.CPOSSet.SigPadHostAddr);
                cardParms.Add("TransactionRecover", cardInfo.isTransactionRecover.ToString()); // SAJID LOCAL DETAILS REPORT PRIMEPOS-2862
            }

            #region PRIMEPOS-2784 EVERTEC
            if (PAYMENTPROCESSOR.ToUpper() == "EVERTEC")
            {
                cardParms.Add("ISMANUAL", cardInfo.isManual == true ? "true" : "false");//PRIMEPOS-2805
                cardParms.Add("ISALLOWDUP", cardInfo.isAllowDuplicate == true ? "true" : "false");
                cardParms.Add("EBTFOODSTAMP", cardInfo.isFood == true ? "true" : "false");//PRIMEPOS-2857
                cardParms.Add("TAXDETAIL", cardInfo.EvertecTaxDetails);//2857
                if (!string.IsNullOrEmpty(SigPadUtil.DefaultInstance.CashBack))//PRIMEPOS-2857
                    cardParms.Add(CASHBACK, SigPadUtil.DefaultInstance.CashBack);
                cardParms.Add("FONDOUNICA", cardInfo.IsFondoUnica == true ? "true" : "false");//2664
            }
            #endregion

            //PRIMEPOS-2665 10-Apr-2019 JY Added
            if (PAYMENTPROCESSOR == "XLINK" || PAYMENTPROCESSOR == "HPSPAX" || PAYMENTPROCESSOR == "HPS" || PAYMENTPROCESSOR == "WORLDPAY" || PAYMENTPROCESSOR == "EVERTEC" || PAYMENTPROCESSOR == "VANTIV" || PAYMENTPROCESSOR == "ELAVON") //PRIMEPOS-2761 //2943
            {
                cardParms.Add("StationID", Configuration.StationID);
            }

            //VANTIV Communincation Params PRIMEPOS-2636
            if (PAYMENTPROCESSOR == "VANTIV")
            {
                cardParms.Add("URL", Configuration.CPOSSet.SigPadHostAddr);
                cardParms.Add("APPLICATIONNAME", Configuration.ApplicationName);
                cardParms.Add("STATIONID", Configuration.StationID);
                cardParms.Add("TRIPOSPATH", Configuration.GetConfigFilePath());//PRIMEPOS-2895 Vantiv Arvind
                //cardParms.Add("lane", Configuration.CPOSSet.SigPadHostAddr);
                cardParms.Add("TransactionRecover", cardInfo.isTransactionRecover.ToString());//PRIMEPOS-3156
                cardParms.Add("TRANSACTIONURL", Configuration.oMerchantConfig.VantivTokenUrl);//PRIMEPOS-3156
                cardParms.Add("REPORTURL", Configuration.oMerchantConfig.VantivReportUrl);//PRIMEPOS-3156
            }

            if (PAYMENTPROCESSOR == "ELAVON")//2943
            {
                cardParms.Add("TERMINALID", Configuration.CSetting.TerminalID);
                cardParms.Add("CHAINCODE", Configuration.CSetting.ChainCode);
                cardParms.Add("LOCATIONNAME", Configuration.CSetting.LocationName);
                cardParms.Add("ISALLOWDUP", cardInfo.isAllowDuplicate == true ? "true" : "false");

            }
            try
            {
                resp = null;//Added by Arvind 
                pccRespInfo = null;//Added by Arvind 
                if (PAYMENTPROCESSOR.ToUpper() != "WORLDPAY")
                {
                    primePosLogWriter.AppendPaymentRequestToLogger(cardParms);
                    String status = ebtProc.EBTSale(ref cardParms, out resp);
                    PccResponse objDBResponse = new PccResponse();
                    DecipherResponse(resp, ref objDBResponse);
                    //PRIMEPOS-2578 Added CardHolderName Response - Discover Specific -16AUg2018 - NILESHJ
                    if (PAYMENTPROCESSOR == "HPSPAX" && resp != null)//PRIMEPOS-3090
                    {
                        string ccHolder = string.Empty;
                        resp.GetAllKeys().TryGetValue("CARDHOLDER", out ccHolder);
                        //if (objCCResponse.CardType.ToUpper() == SecureSubmit.Terminals.PAX.CardType.DiSCOVER.ToString().ToUpper())
                        //{
                        cardInfo.cardHolderName = ccHolder;
                        //}
                    }
                    responseStatus = status;
                    responseError = objDBResponse.ResultDescription;
                    objDBResponse.CardType = "EBT Card";
                    objDBResponse.ResponseStatus = status;
                    pccRespInfo = objDBResponse;
                }
                else
                {
                    #region PRIMEPOS-2761
                    logger.Trace("Orphan - Start1");
                    cardInfo.StationID = Configuration.StationID;
                    cardInfo.UserId = Configuration.UserName;
                    cardInfo.TicketNo = ticketNum;
                    logger.Trace("Orphan - End1");
                    #endregion
                    TransactionResult resp = new TransactionResult();
                    bool isSale = ProcessWpCard(TransactionType.Sale, TransactionSubType.EBT_CashBenifitSale, cardInfo, out resp);
                    WP_TransResult = resp;
                    if (WP_TransResult.Result.Equals("SUCCESS")) // NileshJ - PRIMEPOS-2697
                    {
                        PccPaymentSvr.DefaultInstance.EntryMethod = (string.IsNullOrEmpty(WP_TransResult.ResponseTags.EntryMethod) ? WP_TransResult.EntryMethod : WP_TransResult.ResponseTags.EntryMethod);

                        if (!string.IsNullOrEmpty(WP_TransResult.Token))
                        {
                            PccPaymentSvr.DefaultInstance.ProfiledID = WP_TransResult.Token;

                        }

                        if (WP_TransResult.ResponseTags.CashBack > 0)
                        {
                            PccPaymentSvr.DefaultInstance.CashBack = WP_TransResult.ResponseTags.CashBack.ToString();
                        }
                        #region PRIMEPOS-2761
                        try
                        {
                            using (var db = new Possql())
                            {
                                string Ticketnumber = cardInfo.TicketNo;
                                CCTransmission_Log cclog = new CCTransmission_Log();
                                cclog = db.CCTransmission_Logs.Where(w => w.TicketNo == Ticketnumber).SingleOrDefault();
                                cclog.HostTransID = cardInfo.OrderID + "|" + cardInfo.TransactionID;
                                cclog.ResponseMessage = WP_TransResult.Result;
                                db.CCTransmission_Logs.Attach(cclog);
                                db.Entry(cclog).Property(p => p.HostTransID).IsModified = true;
                                db.Entry(cclog).Property(p => p.ResponseMessage).IsModified = true;
                                db.SaveChanges();
                            }
                        }
                        catch (Exception exep)
                        {
                            logger.Fatal(exep, exep.Message);
                        }
                        #endregion
                        //WPResponse owpresp = new WPResponse();
                        //LoadWPresponse(resp, out owpresp);
                        //WpRespInfo = owpresp;

                        /*if (WpProcessing(MMS.Device.WPDevice.WPData.PayTypes.Credit, MMS.Device.WPDevice.WPData.TransTypes.Sale, cardInfo))
                        {
                            isSale = true;
                        }*/
                    }
                    else
                    {
                        PccPaymentSvr.DefaultInstance.EntryMethod = "";
                        #region PRIMEPOS-2761
                        try
                        {
                            using (var db = new Possql())
                            {
                                string Ticketnumber = cardInfo.TicketNo;
                                CCTransmission_Log cclog = new CCTransmission_Log();
                                cclog = db.CCTransmission_Logs.Where(w => w.TicketNo == Ticketnumber).SingleOrDefault();
                                //cclog.HostTransID = cardInfo.OrderID + "|" + cardInfo.TransactionID;
                                cclog.ResponseMessage = WP_TransResult.Result;
                                db.CCTransmission_Logs.Attach(cclog);
                                //db.Entry(cclog).Property(p => p.HostTransID).IsModified = true;
                                db.Entry(cclog).Property(p => p.ResponseMessage).IsModified = true;
                                db.SaveChanges();
                            }
                        }
                        catch (Exception exep)
                        {
                            logger.Fatal(exep, exep.Message);
                        }
                        #endregion
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "PerfomEBTSale()");
                pccRespInfo = null;//Added by Arvind 
                resp = null;//Added by Arvind
                responseError = ex.ToString();
                primePosLogWriter.AppendCommentsToLogger(responseError + "\r\n");
            }
            finally
            {
                cardParms.Clear();
            }
            primePosLogWriter.AppendCommentsToLogger("Exit From  PerformEBTSale() method\r\n-------------------------------------\r\n");
        }


        /// <summary>
        /// Author : Manoj
        /// Functionality Description : This is for EBT(Return) Implementation - Xlink only with Verifone MX870 PinPad 
        /// known Bugs : None
        /// Start Date : 08/24/2011
        /// </summary>
        /// <param name="ticketNum"></param>
        /// <param name="transAmt"></param>
        public void PerformEBTReturn(string ticketNum, ref PccCardInfo cardInfo)
        {
            primePosLogWriter.AppendCommentsToLogger("Entered into PerformEBTReturn() method\r\n--------------------------------------\r\nRequest Parameters & Their Values\r\n----------------------------------\r\n");
            CultureInfo ci = CultureInfo.CurrentCulture;
            string tempManualFlage = string.Empty;
            string dictObj = string.Empty;
            string trnsdt = string.Empty;
            string track2CleanedData = string.Empty;
            string track2Cleaned = string.Empty;
            Double tempAmount = Convert.ToDouble(cardInfo.transAmount);

            if (PAYMENTPROCESSOR == "HPS")
            {

                if (cardInfo.trackII.Equals(string.Empty))
                {
                    cardParms.Add(MANUALFLAG, "0");
                }
                else
                {
                    cardParms.Add(MANUALFLAG, "1");
                    cardParms.Add(TRACK, cardInfo.Completetrack);
                }
                cardParms.Add(ACCOUNTNUM, cardInfo.cardNumber);
                cardParms.Add(EXPDATE, cardInfo.cardExpiryDate);
                cardParms.Add(CARDHOLDER, cardInfo.cardHolderName);
                cardParms.Add(AMOUNT, tempAmount.ToString("F", ci).PadRight(2, '0'));
                cardParms.Add(TOTALAMOUNT, tempAmount.ToString("F", ci).PadRight(2, '0'));
                cardParms.Add(DEBITPINNO, cardInfo.pinNumber);
                cardParms.Add(ALLOWDUP, sALLOWDUP);
                cardParms.Add(PASSWORD, sPassword);
                AddUserID(cardParms);
                cardParms.Add(MMSCARD, cardInfo.cardType);
            }
            else
            {
                cardParms.Add(ACCOUNTNUM, cardInfo.cardNumber);
                cardParms.Add(EXPDATE, cardInfo.cardExpiryDate);
                cardParms.Add(DEBITPINNO, cardInfo.pinNumber);
                cardParms.Add(DEBITKEYNO, cardInfo.keySerialNumber);
                cardParms.Add(AMOUNT, tempAmount.ToString("F", ci).PadRight(2, '0'));
                cardParms.Add(TRACK, cardInfo.trackII);
                cardParms.Add(CARDHOLDER, cardInfo.cardHolderName);
                cardParms.Add(TICKETNO, ticketNum);
                AddUserID(cardParms);
                cardParms.Add(PASSWORD, sPassword);
                if (cardInfo.trackII.Equals(string.Empty))
                {
                    cardParms.Add(MANUALFLAG, "0");
                }
                else
                {
                    cardParms.Add(MANUALFLAG, "1");
                }

                cardParms.Add(TIMEOUT, cardInfo.txnTimeOut);
                cardParms.Add(MMSCARD, cardInfo.cardType);
                cardParms.Add(TOTALAMOUNT, tempAmount.ToString("F", ci).PadRight(2, '0'));
                cardParms.Add(TERMINALID, sTerminalID);//PrimePOS-2491 
            }

            //PRIMEPOS-2793
            if (PAYMENTPROCESSOR.ToUpper() == "VANTIV")
            {
                if (!String.IsNullOrWhiteSpace(cardInfo.tRoutId))
                    cardParms.Add(TRANSID, cardInfo.tRoutId);
                else
                    cardParms.Add(TRANSID, cardInfo.TransactionID);
            }
            else
                cardParms.Add(TRANSID, cardInfo.TransactionID);
            //

            //PRIMEPOS-2528 (Suraj) 1-June-18
            if (PAYMENTPROCESSOR == "HPSPAX")
            {
                cardParms.Add("HOSTADDR", Configuration.CPOSSet.SigPadHostAddr);
                cardParms.Add("TransactionRecover", cardInfo.isTransactionRecover.ToString()); // SAJID LOCAL DETAILS REPORT PRIMEPOS-2862
            }

            //PRIMEPOS-2665 10-Apr-2019 JY Added
            if (PAYMENTPROCESSOR == "XLINK" || PAYMENTPROCESSOR == "HPSPAX" || PAYMENTPROCESSOR == "HPS" || PAYMENTPROCESSOR == "WORLDPAY" || PAYMENTPROCESSOR == "EVERTEC" || PAYMENTPROCESSOR == "VANTIV" || PAYMENTPROCESSOR == "ELAVON") //PRIMEPOS-2761 - 2753 //2943
            {
                cardParms.Add("StationID", Configuration.StationID);
            }

            #region PRIMEPOS-2784 EVERTEC
            if (PAYMENTPROCESSOR.ToUpper() == "EVERTEC")
            {
                cardParms.Add("ISMANUAL", cardInfo.isManual == true ? "true" : "false");//PRIMEPOS-2805
                cardParms.Add("ISALLOWDUP", cardInfo.isAllowDuplicate == true ? "true" : "false");
                cardParms.Add("TAXDETAIL", cardInfo.EvertecTaxDetails);//2857
                cardParms.Add("EBTFOODSTAMP", cardInfo.isFood == true ? "true" : "false");//PRIMEPOS-2857
                cardParms.Add("FONDOUNICA", cardInfo.IsFondoUnica == true ? "true" : "false");//2664
            }
            #endregion

            //VANTIV Communincation Params PRIMEPOS-2636
            if (PAYMENTPROCESSOR == "VANTIV")
            {
                if (!cardParms.ContainsKey("AMOUNT"))
                    cardParms.Add("AMOUNT", cardInfo.transAmount);
                cardParms.Add("URL", Configuration.CPOSSet.SigPadHostAddr);
                cardParms.Add("APPLICATIONNAME", Configuration.ApplicationName);
                cardParms.Add("TRIPOSPATH", Configuration.GetConfigFilePath());//PRIMEPOS-2895 Vantiv Arvind
                cardParms.Add("STATIONID", Configuration.StationID);
                cardParms.Add("TransactionRecover", cardInfo.isTransactionRecover.ToString()); //PRIMEPOS-3156
                cardParms.Add("TRANSACTIONURL", Configuration.oMerchantConfig.VantivTokenUrl);//PRIMEPOS-3156
                cardParms.Add("REPORTURL", Configuration.oMerchantConfig.VantivReportUrl);//PRIMEPOS-3156
            }

            if (PAYMENTPROCESSOR == "ELAVON")//2943
            {
                cardParms.Add("TERMINALID", Configuration.CSetting.TerminalID);
                cardParms.Add("CHAINCODE", Configuration.CSetting.ChainCode);
                cardParms.Add("LOCATIONNAME", Configuration.CSetting.LocationName);
                cardParms.Add("ISALLOWDUP", cardInfo.isAllowDuplicate == true ? "true" : "false");
                cardParms.Add("TRANSDATE", cardInfo.TransDate);
                cardParms.Add("ISEBTVOID", this.IsElavonEbtvoid == true ? "true" : "false");
                if (!String.IsNullOrWhiteSpace(cardInfo.tRoutId))
                    cardParms.Add(TRANSID, cardInfo.tRoutId);
                else
                    cardParms.Add(TRANSID, cardInfo.TransactionID);
            }

            //PRIMEPOS-2665 10-Apr-2019 JY Added
            //if (PAYMENTPROCESSOR == "XLINK" || PAYMENTPROCESSOR == "HPSPAX" || PAYMENTPROCESSOR == "HPS" || PAYMENTPROCESSOR == "WORLDPAY" || PAYMENTPROCESSOR == "EVERTEC" || PAYMENTPROCESSOR == "VANTIV") //PRIMEPOS-2761 - 2753 
            //{
            //    cardParms.Add("StationID", Configuration.StationID);
            //}


            try
            {
                resp = null;//Added by Arvind 
                pccRespInfo = null;//Added by Arvind 
                if (PAYMENTPROCESSOR.ToUpper() != "WORLDPAY")
                {
                    //primePosLogWriter.AppendPaymentRequestToLogger(cardParms);
                    String status = ebtProc.EBTReturn(ref cardParms, out resp);
                    PccResponse objDBResponse = new PccResponse();
                    DecipherResponse(resp, ref objDBResponse);
                    //PRIMEPOS-2578 Added CardHolderName Response - Discover Specific -16AUg2018 - NILESHJ
                    if (PAYMENTPROCESSOR == "HPSPAX" && resp != null)//PRIMEPOS-3090
                    {
                        string ccHolder = string.Empty;
                        resp.GetAllKeys().TryGetValue("CARDHOLDER", out ccHolder);
                        //if (objCCResponse.CardType.ToUpper() == SecureSubmit.Terminals.PAX.CardType.DiSCOVER.ToString().ToUpper())
                        //{
                        cardInfo.cardHolderName = ccHolder;
                        //}
                    }
                    responseStatus = status;
                    responseError = objDBResponse.ResultDescription;
                    objDBResponse.CardType = "EBT Card";
                    objDBResponse.ResponseStatus = status;
                    pccRespInfo = objDBResponse;
                }
                else
                {
                    #region PRIMEPOS-2761
                    logger.Trace("Orphan - Start1");
                    cardInfo.StationID = Configuration.StationID;
                    cardInfo.UserId = Configuration.UserName;
                    cardInfo.TicketNo = ticketNum;
                    logger.Trace("Orphan - End1");
                    #endregion
                    TransactionResult resp = new TransactionResult();
                    bool isSale;
                    //ADDED BY ARVIND PRIMEPOS-2738 FOR STRICT RETURN REVERSAL...REFUND INSTEAD OF CREDIT
                    if (Configuration.CSetting.StrictReturn == true)
                    {
                        isSale = ProcessWpCard(TransactionType.Refund, TransactionSubType.EBT_CashBenifitReturn, cardInfo, out resp);
                    }
                    else
                    {
                        isSale = ProcessWpCard(TransactionType.Credit, TransactionSubType.EBT_CashBenifitReturn, cardInfo, out resp);
                    }
                    WP_TransResult = resp;
                    if (WP_TransResult.Result.Equals("SUCCESS")) // NileshJ - PRIMEPOS-2697
                    {
                        PccPaymentSvr.DefaultInstance.EntryMethod = (string.IsNullOrEmpty(WP_TransResult.ResponseTags.EntryMethod) ? WP_TransResult.EntryMethod : WP_TransResult.ResponseTags.EntryMethod);

                        if (!string.IsNullOrEmpty(WP_TransResult.Token))
                        {
                            PccPaymentSvr.DefaultInstance.ProfiledID = WP_TransResult.Token;

                        }

                        if (WP_TransResult.ResponseTags.CashBack > 0)
                        {
                            PccPaymentSvr.DefaultInstance.CashBack = WP_TransResult.ResponseTags.CashBack.ToString();
                        }
                        #region PRIMEPOS-2761
                        try
                        {
                            using (var db = new Possql())
                            {
                                string Ticketnumber = cardInfo.TicketNo;
                                CCTransmission_Log cclog = new CCTransmission_Log();
                                cclog = db.CCTransmission_Logs.Where(w => w.TicketNo == Ticketnumber).SingleOrDefault();
                                cclog.HostTransID = cardInfo.OrderID + "|" + cardInfo.TransactionID;
                                cclog.ResponseMessage = WP_TransResult.Result;
                                db.CCTransmission_Logs.Attach(cclog);
                                db.Entry(cclog).Property(p => p.HostTransID).IsModified = true;
                                db.Entry(cclog).Property(p => p.ResponseMessage).IsModified = true;
                                db.SaveChanges();
                            }
                        }
                        catch (Exception exep)
                        {
                            logger.Fatal(exep, exep.Message);
                        }
                        #endregion
                    }
                    else
                    {
                        PccPaymentSvr.DefaultInstance.EntryMethod = "";
                        #region PRIMEPOS-2761
                        try
                        {
                            using (var db = new Possql())
                            {
                                string Ticketnumber = cardInfo.TicketNo;
                                CCTransmission_Log cclog = new CCTransmission_Log();
                                cclog = db.CCTransmission_Logs.Where(w => w.TicketNo == Ticketnumber).SingleOrDefault();
                                //cclog.HostTransID = cardInfo.OrderID + "|" + cardInfo.TransactionID;
                                cclog.ResponseMessage = WP_TransResult.Result;
                                db.CCTransmission_Logs.Attach(cclog);
                                //db.Entry(cclog).Property(p => p.HostTransID).IsModified = true;
                                db.Entry(cclog).Property(p => p.ResponseMessage).IsModified = true;
                                db.SaveChanges();
                            }
                        }
                        catch (Exception exep)
                        {
                            logger.Fatal(exep, exep.Message);
                        }
                        #endregion
                    }
                }

            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "PerformEBTReturn()");
                responseError = ex.ToString();
                pccRespInfo = null;//Added by Arvind 
                resp = null;//Added by Arvind
                primePosLogWriter.AppendCommentsToLogger(responseError + "\r\n");
            }
            finally
            {
                cardParms.Clear();
            }
            primePosLogWriter.AppendCommentsToLogger("Exit From  PerformEBTReturn() method\r\n-------------------------------------\r\n");
        }

        public void PerformDebitReturnWP(string ticketNum, ref PccCardInfo cardInfo)
        {
            try
            {
                #region PRIMEPOS-2761
                logger.Trace("Orphan - Start1");
                cardInfo.StationID = Configuration.StationID;
                cardInfo.UserId = Configuration.UserName;
                cardInfo.TicketNo = ticketNum;
                logger.Trace("Orphan - End1");
                #endregion
                TransactionResult resp = new TransactionResult();
                bool isSale;
                //ADDED BY ARVIND FOR PRIMEPOS-2738 FOR STRICT RETURN ..REFUND INSTEAD OF CREDIT
                if (Configuration.CSetting.StrictReturn == true)
                {
                    isSale = ProcessWpCard(TransactionType.Refund, TransactionSubType.None, cardInfo, out resp);
                }
                else
                {
                    isSale = ProcessWpCard(TransactionType.Credit, TransactionSubType.None, cardInfo, out resp);
                }
                //
                WP_TransResult = resp;
                if (WP_TransResult.Result.Equals("SUCCESS")) // NileshJ - PRIMEPOS-2697
                {
                    PccPaymentSvr.DefaultInstance.EntryMethod = (string.IsNullOrEmpty(WP_TransResult.ResponseTags.EntryMethod) ? WP_TransResult.EntryMethod : WP_TransResult.ResponseTags.EntryMethod);

                    if (!string.IsNullOrEmpty(WP_TransResult.Token))
                    {
                        PccPaymentSvr.DefaultInstance.ProfiledID = WP_TransResult.Token;

                    }

                    if (WP_TransResult.ResponseTags.CashBack > 0)
                    {
                        PccPaymentSvr.DefaultInstance.CashBack = WP_TransResult.ResponseTags.CashBack.ToString();
                    }
                }
                else
                {
                    PccPaymentSvr.DefaultInstance.EntryMethod = "";
                }
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "PerformDebitReturnWP()");
                responseError = ex.ToString();
                primePosLogWriter.AppendCommentsToLogger(responseError + "\r\n");
            }


            primePosLogWriter.AppendCommentsToLogger("Exit From  PerformDebitSale() method\r\n-------------------------------------\r\n");
        }


        /// <summary>
        /// Author : Manoj
        /// Functionality Description : This is for Debit Card(Sale) Implementation - Xlink and HPS only with Verifone MX870 PinPad 
        /// known Bugs : None
        /// Start Date : 08/24/2011 Modify 5/31/2012
        /// </summary>
        /// <param name="ticketNum"></param>
        /// <param name="transAmt"></param>
        public bool PerformDebitSale(string ticketNum, ref PccCardInfo cardInfo)
        {

            primePosLogWriter.AppendCommentsToLogger("Entered into PerformDebitSale() method\r\n--------------------------------------\r\nRequest Parameters & Their Values\r\n----------------------------------\r\n");
            bool isProcess = false;
            CultureInfo ci = CultureInfo.CurrentCulture;
            string tempManualFlage = string.Empty;
            string dictObj = string.Empty;
            string trnsdt = string.Empty;
            string track2CleanedData = string.Empty;
            string track2Cleaned = string.Empty;
            Double tempAmount = Convert.ToDouble(cardInfo.transAmount);

            IsSecurityPromptRequired(Transactions.DebitSale, cardParms);
            if (PAYMENTPROCESSOR == "HPS")
            {
                cardParms.Add(AMOUNT, tempAmount.ToString("F", ci).PadRight(2, '0'));
                cardParms.Add(TRACK, cardInfo.Completetrack);
                cardParms.Add(DEBITPINNO, cardInfo.pinNumber);
                cardParms.Add(ALLOWDUP, sALLOWDUP);
                cardParms.Add(PASSWORD, sPassword);
                AddUserID(cardParms);
            }
            else if (PAYMENTPROCESSOR.ToUpper() != "WORLDPAY")
            {
                cardParms.Add(ACCOUNTNUM, cardInfo.cardNumber);
                cardParms.Add(EXPDATE, cardInfo.cardExpiryDate);
                cardParms.Add(DEBITPINNO, cardInfo.pinNumber);
                cardParms.Add(DEBITKEYNO, cardInfo.keySerialNumber);
                cardParms.Add(AMOUNT, tempAmount.ToString("F", ci).PadRight(2, '0'));
                cardParms.Add(TRACK, cardInfo.trackII);
                cardParms.Add(ALLOWDUP, sALLOWDUP);//PRIMEPOS-2664
                cardParms.Add(CARDHOLDER, cardInfo.cardHolderName);
                cardParms.Add(TICKETNO, ticketNum);
                if (!string.IsNullOrEmpty(SigPadUtil.DefaultInstance.CashBack))
                    cardParms.Add(CASHBACK, SigPadUtil.DefaultInstance.CashBack);
                // cardParms.Add(CASHBACKAMT, !string.IsNullOrEmpty(SigPadUtil.DefaultInstance.CashBack) ? Conver
                AddUserID(cardParms);
                cardParms.Add(PASSWORD, sPassword);
                if (cardInfo.trackII.Equals(string.Empty))
                {
                    cardParms.Add(MANUALFLAG, "0");
                }
                else
                {
                    cardParms.Add(MANUALFLAG, "1");
                }

                cardParms.Add(TIMEOUT, cardInfo.txnTimeOut);
                cardParms.Add(MMSCARD, cardInfo.cardType);
                cardParms.Add(TOTALAMOUNT, tempAmount.ToString("F", ci).PadRight(2, '0'));
                cardParms.Add(TERMINALID, sTerminalID); //PrimePOS-2491 
            }
            //Added For HPS

            //PRIMEPOS-2528 (Suraj) 1-June-18
            if (PAYMENTPROCESSOR == "HPSPAX")
            {
                cardParms.Add("HOSTADDR", Configuration.CPOSSet.SigPadHostAddr);
                cardParms.Add("TransactionRecover", cardInfo.isTransactionRecover.ToString()); // SAJID LOCAL DETAILS REPORT PRIMEPOS-2862
            }

            #region PRIMEPOS-2784 EVERTEC
            if (PAYMENTPROCESSOR.ToUpper() == "EVERTEC")
            {
                cardParms.Add("ISMANUAL", cardInfo.isManual == true ? "true" : "false");//PRIMEPOS-2805
                cardParms.Add("ISALLOWDUP", cardInfo.isAllowDuplicate == true ? "true" : "false");
                cardParms.Add("TAXDETAIL", cardInfo.EvertecTaxDetails);//2664
            }
            #endregion

            //PRIMEPOS-2665 10-Apr-2019 JY Added
            if (PAYMENTPROCESSOR == "XLINK" || PAYMENTPROCESSOR == "HPSPAX" || PAYMENTPROCESSOR == "HPS" || PAYMENTPROCESSOR == "WORLDPAY" || PAYMENTPROCESSOR == "EVERTEC" || PAYMENTPROCESSOR == "VANTIV" || PAYMENTPROCESSOR == "ELAVON") // PRIMEPOS-2761 - 2753//2943
            {
                cardParms.Add("StationID", Configuration.StationID);
            }

            //VANTIV Communincation Params PRIMEPOS-2636
            if (PAYMENTPROCESSOR == "VANTIV")
            {
                cardParms.Add("URL", Configuration.CPOSSet.SigPadHostAddr);
                cardParms.Add("APPLICATIONNAME", Configuration.ApplicationName);
                cardParms.Add("STATIONID", Configuration.StationID);
                cardParms.Add("TRIPOSPATH", Configuration.GetConfigFilePath());//PRIMEPOS-2895 Vantiv Arvind
                //cardParms.Add("lane", Configuration.CPOSSet.SigPadHostAddr);
                cardParms.Add("TransactionRecover", cardInfo.isTransactionRecover.ToString()); //PRIMEPOS-3156
                cardParms.Add("TRANSACTIONURL", Configuration.oMerchantConfig.VantivTokenUrl);//PRIMEPOS-3156
                cardParms.Add("REPORTURL", Configuration.oMerchantConfig.VantivReportUrl);//PRIMEPOS-3156
            }

            if (PAYMENTPROCESSOR == "ELAVON")//2943
            {
                cardParms.Add("TERMINALID", Configuration.CSetting.TerminalID);
                cardParms.Add("CHAINCODE", Configuration.CSetting.ChainCode);
                cardParms.Add("LOCATIONNAME", Configuration.CSetting.LocationName);
                cardParms.Add("ISALLOWDUP", cardInfo.isAllowDuplicate == true ? "true" : "false");
                cardParms.Add("ISTAX", cardInfo.IsElavonTax == true ? "true" : "false");
                cardParms.Add("TAXAMT", cardInfo.ElavonTotalTax);//2943
            }

            try
            {
                resp = null;//Added by Arvind 
                pccRespInfo = null;//Added by Arvind 
                if (PAYMENTPROCESSOR.ToUpper() != "WORLDPAY")
                {
                    primePosLogWriter.AppendPaymentRequestToLogger(cardParms);
                    String status = debitProc.Sale(ref cardParms, out resp);
                    PccResponse objDBResponse = new PccResponse();
                    DecipherResponse(resp, ref objDBResponse);
                    //PRIMEPOS-2578 Added CardHolderName Response - Discover Specific -16AUg2018 - NILESHJ
                    if (PAYMENTPROCESSOR == "HPSPAX" && resp != null)//PRIMEPOS-3090
                    {
                        string ccHolder = string.Empty;
                        resp.GetAllKeys().TryGetValue("CARDHOLDER", out ccHolder);
                        //if (objCCResponse.CardType.ToUpper() == SecureSubmit.Terminals.PAX.CardType.DiSCOVER.ToString().ToUpper())
                        //{
                        cardInfo.cardHolderName = ccHolder;
                        //}
                    }
                    responseStatus = status;
                    responseError = objDBResponse.ResultDescription;
                    if (resp != null && resp.CardType.ToUpper() != "ATH MOVIL")//PRIMEPOS-3090
                    {
                        objDBResponse.CardType = "Debit Card";
                    }
                    objDBResponse.ResponseStatus = status;
                    pccRespInfo = objDBResponse;
                    isProcess = true;
                }
                else
                {
                    #region PRIMEPOS-2761
                    logger.Trace("Orphan - Start1");
                    cardInfo.StationID = Configuration.StationID;
                    cardInfo.UserId = Configuration.UserName;
                    cardInfo.TicketNo = ticketNum;
                    logger.Trace("Orphan - End1");
                    #endregion
                    TransactionResult resp = new TransactionResult();
                    isProcess = ProcessWpCard(TransactionType.Sale, TransactionSubType.None, cardInfo, out resp);
                    WP_TransResult = resp;
                    if (WP_TransResult.Result.Equals("SUCCESS")) // NileshJ - PRIMEPOS-2697
                    {
                        PccPaymentSvr.DefaultInstance.EntryMethod = (string.IsNullOrEmpty(WP_TransResult.ResponseTags.EntryMethod) ? WP_TransResult.EntryMethod : WP_TransResult.ResponseTags.EntryMethod);

                        if (!string.IsNullOrEmpty(WP_TransResult.Token))
                        {
                            PccPaymentSvr.DefaultInstance.ProfiledID = WP_TransResult.Token;

                        }

                        if (WP_TransResult.ResponseTags.CashBack > 0)
                        {
                            PccPaymentSvr.DefaultInstance.CashBack = WP_TransResult.ResponseTags.CashBack.ToString();
                        }
                        #region PRIMEPOS-2761
                        try
                        {
                            using (var db = new Possql())
                            {
                                string Ticketnumber = cardInfo.TicketNo;
                                CCTransmission_Log cclog = new CCTransmission_Log();
                                cclog = db.CCTransmission_Logs.Where(w => w.TicketNo == Ticketnumber).SingleOrDefault();
                                cclog.HostTransID = cardInfo.OrderID + "|" + cardInfo.TransactionID;
                                cclog.ResponseMessage = WP_TransResult.Result;
                                db.CCTransmission_Logs.Attach(cclog);
                                db.Entry(cclog).Property(p => p.HostTransID).IsModified = true;
                                db.Entry(cclog).Property(p => p.ResponseMessage).IsModified = true;
                                db.SaveChanges();
                            }
                        }
                        catch (Exception exep)
                        {
                            logger.Fatal(exep, exep.Message);
                        }
                        #endregion
                        /* if (WpProcessing(MMS.Device.WPDevice.WPData.PayTypes.Debit, MMS.Device.WPDevice.WPData.TransTypes.Sale, cardInfo))
                         {
                             isProcess = true;
                         }*/
                    }
                    else
                    {
                        PccPaymentSvr.DefaultInstance.EntryMethod = "";
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "PerPerformDebitSaleformDebitReturnWP()");
                responseError = ex.ToString();
                pccRespInfo = null;//Added by Arvind 
                resp = null;//Added by Arvind
                primePosLogWriter.AppendCommentsToLogger(responseError + "\r\n");
            }
            finally
            {
                cardParms.Clear();
            }
            return isProcess;
            primePosLogWriter.AppendCommentsToLogger("Exit From  PerformDebitSale() method\r\n-------------------------------------\r\n");
        }

        #region PRIMEPOS-3372
        public bool PerformNBSSale(string ticketNum, ref PccCardInfo cardInfo)
        {

            primePosLogWriter.AppendCommentsToLogger("Entered into PerformNBSSale() method\r\n--------------------------------------\r\nRequest Parameters & Their Values\r\n----------------------------------\r\n");
            bool isProcess = false;
            CultureInfo ci = CultureInfo.CurrentCulture;
            Double tempAmount = Convert.ToDouble(cardInfo.transAmount);

            IsSecurityPromptRequired(Transactions.DebitSale, cardParms);

            cardParms.Add(PREREADID, cardInfo.preReadId);
            cardParms.Add(ACCOUNTNUM, cardInfo.cardNumber);
            cardParms.Add(EXPDATE, cardInfo.cardExpiryDate);
            cardParms.Add(DEBITPINNO, cardInfo.pinNumber);
            cardParms.Add(DEBITKEYNO, cardInfo.keySerialNumber);
            cardParms.Add(AMOUNT, tempAmount.ToString("F", ci).PadRight(2, '0'));
            cardParms.Add(TRACK, cardInfo.trackII);
            cardParms.Add(ALLOWDUP, sALLOWDUP);//PRIMEPOS-2664
            cardParms.Add(CARDHOLDER, cardInfo.cardHolderName);
            cardParms.Add(TICKETNO, ticketNum);
            if (!string.IsNullOrEmpty(SigPadUtil.DefaultInstance.CashBack))
                cardParms.Add(CASHBACK, SigPadUtil.DefaultInstance.CashBack);
            // cardParms.Add(CASHBACKAMT, !string.IsNullOrEmpty(SigPadUtil.DefaultInstance.CashBack) ? Conver
            AddUserID(cardParms);
            cardParms.Add(PASSWORD, sPassword);
            if (cardInfo.trackII.Equals(string.Empty))
            {
                cardParms.Add(MANUALFLAG, "0");
            }
            else
            {
                cardParms.Add(MANUALFLAG, "1");
            }

            cardParms.Add(TIMEOUT, cardInfo.txnTimeOut);
            cardParms.Add(MMSCARD, cardInfo.cardType);
            cardParms.Add(TOTALAMOUNT, tempAmount.ToString("F", ci).PadRight(2, '0'));
            cardParms.Add(TERMINALID, sTerminalID); //PrimePOS-2491 

            if (PAYMENTPROCESSOR == "VANTIV")
            {
                cardParms.Add("URL", Configuration.CPOSSet.SigPadHostAddr);
                cardParms.Add("APPLICATIONNAME", Configuration.ApplicationName);
                cardParms.Add("STATIONID", Configuration.StationID);
                cardParms.Add("TRIPOSPATH", Configuration.GetConfigFilePath());//PRIMEPOS-2895 Vantiv Arvind
                //cardParms.Add("lane", Configuration.CPOSSet.SigPadHostAddr);
                cardParms.Add("TransactionRecover", cardInfo.isTransactionRecover.ToString()); //PRIMEPOS-3156
                cardParms.Add("TRANSACTIONURL", Configuration.oMerchantConfig.VantivTokenUrl);//PRIMEPOS-3156
                cardParms.Add("REPORTURL", Configuration.oMerchantConfig.VantivReportUrl);//PRIMEPOS-3156
            }

            try
            {
                resp = null;//Added by Arvind 
                pccRespInfo = null;//Added by Arvind 
                if (PAYMENTPROCESSOR.ToUpper() != "WORLDPAY")
                {
                    primePosLogWriter.AppendPaymentRequestToLogger(cardParms);
                    String status = nbsProc.NBSSale(ref cardParms, out resp);
                    PccResponse objDBResponse = new PccResponse();
                    DecipherResponse(resp, ref objDBResponse);

                    responseStatus = status;
                    responseError = objDBResponse.ResultDescription;
                    //if (resp != null && resp.CardType.ToUpper() != "ATH MOVIL")
                    //{
                    //    objDBResponse.CardType = "Debit Card";
                    //}
                    objDBResponse.ResponseStatus = status;
                    pccRespInfo = objDBResponse;
                    isProcess = true;
                }
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "PerformNBSSale()");
                responseError = ex.ToString();
                pccRespInfo = null;//Added by Arvind 
                resp = null;//Added by Arvind
                primePosLogWriter.AppendCommentsToLogger(responseError + "\r\n");
            }
            finally
            {
                cardParms.Clear();
            }
            return isProcess;
            primePosLogWriter.AppendCommentsToLogger("Exit From  PerformNBSSale() method\r\n-------------------------------------\r\n");
        }

        public bool PerformNBSPreRead(string ticketNum, ref PccCardInfo cardInfo)
        {

            primePosLogWriter.AppendCommentsToLogger("Entered into PerformNBSPreRead() method\r\n--------------------------------------\r\nRequest Parameters & Their Values\r\n----------------------------------\r\n");
            bool isProcess = false;
            CultureInfo ci = CultureInfo.CurrentCulture;
            Double tempAmount = Convert.ToDouble(cardInfo.transAmount);

            IsSecurityPromptRequired(Transactions.DebitSale, cardParms);

            cardParms.Add(PREREADTYPE, cardInfo.preReadType); //PRIMEPOS-3407
            cardParms.Add(ACCOUNTNUM, cardInfo.cardNumber);
            cardParms.Add(EXPDATE, cardInfo.cardExpiryDate);
            cardParms.Add(DEBITPINNO, cardInfo.pinNumber);
            cardParms.Add(DEBITKEYNO, cardInfo.keySerialNumber);
            cardParms.Add(AMOUNT, tempAmount.ToString("F", ci).PadRight(2, '0'));
            cardParms.Add(TRACK, cardInfo.trackII);
            cardParms.Add(ALLOWDUP, sALLOWDUP);//PRIMEPOS-2664
            cardParms.Add(CARDHOLDER, cardInfo.cardHolderName);
            cardParms.Add(TICKETNO, ticketNum);
            if (!string.IsNullOrEmpty(SigPadUtil.DefaultInstance.CashBack))
                cardParms.Add(CASHBACK, SigPadUtil.DefaultInstance.CashBack);
            // cardParms.Add(CASHBACKAMT, !string.IsNullOrEmpty(SigPadUtil.DefaultInstance.CashBack) ? Conver
            AddUserID(cardParms);
            cardParms.Add(PASSWORD, sPassword);
            if (cardInfo.trackII.Equals(string.Empty))
            {
                cardParms.Add(MANUALFLAG, "0");
            }
            else
            {
                cardParms.Add(MANUALFLAG, "1");
            }

            cardParms.Add(TIMEOUT, cardInfo.txnTimeOut);
            cardParms.Add(MMSCARD, cardInfo.cardType);
            cardParms.Add(TOTALAMOUNT, tempAmount.ToString("F", ci).PadRight(2, '0'));
            cardParms.Add(TERMINALID, sTerminalID); //PrimePOS-2491 

            if (PAYMENTPROCESSOR == "VANTIV")
            {
                cardParms.Add("URL", Configuration.CPOSSet.SigPadHostAddr);
                cardParms.Add("APPLICATIONNAME", Configuration.ApplicationName);
                cardParms.Add("STATIONID", Configuration.StationID);
                cardParms.Add("TRIPOSPATH", Configuration.GetConfigFilePath());//PRIMEPOS-2895 Vantiv Arvind
                //cardParms.Add("lane", Configuration.CPOSSet.SigPadHostAddr);
                cardParms.Add("TransactionRecover", cardInfo.isTransactionRecover.ToString()); //PRIMEPOS-3156
                cardParms.Add("TRANSACTIONURL", Configuration.oMerchantConfig.VantivTokenUrl);//PRIMEPOS-3156
                cardParms.Add("REPORTURL", Configuration.oMerchantConfig.VantivReportUrl);//PRIMEPOS-3156
            }

            try
            {
                resp = null;//Added by Arvind 
                pccRespInfo = null;//Added by Arvind 
                if (PAYMENTPROCESSOR.ToUpper() != "WORLDPAY")
                {
                    primePosLogWriter.AppendPaymentRequestToLogger(cardParms);
                    String status = nbsProc.NBSPreReadSale(ref cardParms, out resp);
                    PccResponse objDBResponse = new PccResponse();
                    DecipherResponse(resp, ref objDBResponse);

                    responseStatus = status;
                    responseError = objDBResponse.ResultDescription;
                    objDBResponse.ResponseStatus = status;
                    pccRespInfo = objDBResponse;
                    isProcess = true;
                }
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "PerformNBSPreRead()");
                responseError = ex.ToString();
                pccRespInfo = null;//Added by Arvind 
                resp = null;//Added by Arvind
                primePosLogWriter.AppendCommentsToLogger(responseError + "\r\n");
            }
            finally
            {
                cardParms.Clear();
            }
            return isProcess;
            primePosLogWriter.AppendCommentsToLogger("Exit From  PerformNBSSale() method\r\n-------------------------------------\r\n");
        }

        public bool PerformPreReadCancel(string ticketNum, ref PccCardInfo cardInfo)
        {
            primePosLogWriter.AppendCommentsToLogger("Entered into PerformNBSSale() method\r\n--------------------------------------\r\nRequest Parameters & Their Values\r\n----------------------------------\r\n");
            bool isProcess = false;
            CultureInfo ci = CultureInfo.CurrentCulture;
            
            cardParms.Add(TICKETNO, ticketNum);

            if (PAYMENTPROCESSOR == "VANTIV")
            {
                cardParms.Add("URL", Configuration.CPOSSet.SigPadHostAddr);
                cardParms.Add("APPLICATIONNAME", Configuration.ApplicationName);
                cardParms.Add("STATIONID", Configuration.StationID);
                cardParms.Add("TRIPOSPATH", Configuration.GetConfigFilePath());//PRIMEPOS-2895 Vantiv Arvind
                //cardParms.Add("lane", Configuration.CPOSSet.SigPadHostAddr);
                cardParms.Add("TransactionRecover", cardInfo.isTransactionRecover.ToString()); //PRIMEPOS-3156
                cardParms.Add("TRANSACTIONURL", Configuration.oMerchantConfig.VantivTokenUrl);//PRIMEPOS-3156
                cardParms.Add("REPORTURL", Configuration.oMerchantConfig.VantivReportUrl);//PRIMEPOS-3156
            }

            try
            {
                resp = null;//Added by Arvind 
                pccRespInfo = null;//Added by Arvind 
                if (PAYMENTPROCESSOR.ToUpper() != "WORLDPAY")
                {
                    primePosLogWriter.AppendPaymentRequestToLogger(cardParms);
                    String status = nbsProc.CancelTansaction(ref cardParms, out resp);
                    PccResponse objDBResponse = new PccResponse();
                    DecipherResponse(resp, ref objDBResponse);

                    responseStatus = status;
                    responseError = objDBResponse.ResultDescription;
                    objDBResponse.ResponseStatus = status;
                    pccRespInfo = objDBResponse;
                    isProcess = true;
                }
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "PerformPreReadCancel()");
                responseError = ex.ToString();
                pccRespInfo = null;//Added by Arvind 
                resp = null;//Added by Arvind
                primePosLogWriter.AppendCommentsToLogger(responseError + "\r\n");
            }
            finally
            {
                cardParms.Clear();
            }
            return isProcess;
            primePosLogWriter.AppendCommentsToLogger("Exit From  PerformNBSSale() method\r\n-------------------------------------\r\n");
        }

        public bool PerformPreReadCancelTrans(string ticketNum, ref PccCardInfo cardInfo) //PRIMEPOS-3526 //PRIMEPOS-3504
        {
            primePosLogWriter.AppendCommentsToLogger("Entered into PerformPreReadCancelTrans() method\r\n--------------------------------------\r\nRequest Parameters & Their Values\r\n----------------------------------\r\n");
            bool isProcess = false;
            CultureInfo ci = CultureInfo.CurrentCulture;

            cardParms.Add(TICKETNO, ticketNum);

            if (PAYMENTPROCESSOR == "VANTIV")
            {
                cardParms.Add("URL", Configuration.CPOSSet.SigPadHostAddr);
                cardParms.Add("APPLICATIONNAME", Configuration.ApplicationName);
                cardParms.Add("STATIONID", Configuration.StationID);
                cardParms.Add("TRIPOSPATH", Configuration.GetConfigFilePath());//PRIMEPOS-2895 Vantiv Arvind
                cardParms.Add("TransactionRecover", cardInfo.isTransactionRecover.ToString()); //PRIMEPOS-3156
                cardParms.Add("TRANSACTIONURL", Configuration.oMerchantConfig.VantivTokenUrl);//PRIMEPOS-3156
                cardParms.Add("REPORTURL", Configuration.oMerchantConfig.VantivReportUrl);//PRIMEPOS-3156
            }

            try
            {
                resp = null;//Added by Arvind 
                pccRespInfo = null;//Added by Arvind 
                if (PAYMENTPROCESSOR.ToUpper() != "WORLDPAY")
                {
                    primePosLogWriter.AppendPaymentRequestToLogger(cardParms);
                    String status = creditProc.PreReadCancel(ref cardParms, out resp);
                    PccResponse objDBResponse = new PccResponse();
                    DecipherResponse(resp, ref objDBResponse);

                    responseStatus = status;
                    responseError = objDBResponse.ResultDescription;
                    objDBResponse.ResponseStatus = status;
                    pccRespInfo = objDBResponse;
                    isProcess = true;
                }
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "PerformPreReadCancel()");
                responseError = ex.ToString();
                pccRespInfo = null;//Added by Arvind 
                resp = null;//Added by Arvind
                primePosLogWriter.AppendCommentsToLogger(responseError + "\r\n");
            }
            finally
            {
                cardParms.Clear();
            }
            return isProcess;
            primePosLogWriter.AppendCommentsToLogger("Exit From  PerformNBSSale() method\r\n-------------------------------------\r\n");
        }

        public void PerformNBSSalesReturn(string ticketNum, ref PccCardInfo cardInfo)
        {
            // Added By Dharmendra (SRT) Dec-08-08 to store request & response in the logger file
            primePosLogWriter.AppendCommentsToLogger("Entered into PerformNBSSalesReturn() method\r\n---------------------------------------------\r\nRequest Parameters & Their Values\r\n----------------------------------\r\n");
            //Added Till Here
            CultureInfo ci = CultureInfo.CurrentCulture;
            string tempManualFlage = string.Empty;
            string dictObj = string.Empty;
            string trnsdt = string.Empty;

            Double tempAmount = Convert.ToDouble(cardInfo.transAmount);
            Double tempFsaAmount = string.IsNullOrEmpty(cardInfo.transFSAAmount) ? Convert.ToDouble("0.00") : Convert.ToDouble(cardInfo.transFSAAmount);
            Double tempFsaRxAmount = string.IsNullOrEmpty(cardInfo.transFSARxAmount) ? Convert.ToDouble("0.00") : Convert.ToDouble(cardInfo.transFSARxAmount);//rx pr amt

            //Double otc_fsa_amt = 0;

            //ADDED PRASHANT 5 JUN 2010
            IsSecurityPromptRequired(Transactions.CreditSalesReturn, cardParms);
            //END ADDED PRASHANT 5 JUN 2010

            cardParms.Add(NBSSALETYPE, cardInfo.nbsSaleType);
            cardParms.Add(PREREADID, cardInfo.preReadId); //PRIMEPOS-3407
            cardParms.Add(ACCOUNTNUM, cardInfo.cardNumber);
            cardParms.Add(EXPDATE, cardInfo.cardExpiryDate);
            cardParms.Add(AMOUNT, tempAmount.ToString("F", ci).PadRight(2, '0'));
            cardParms.Add(TIMEOUT, cardInfo.txnTimeOut);
            cardParms.Add(MMSCARD, cardInfo.cardType);
            cardParms.Add(ZIP, cardInfo.zipCode);
            cardParms.Add(ADDRESS, cardInfo.customerAddress);
            cardParms.Add(CARDHOLDER, cardInfo.cardHolderName);
            cardParms.Add(TICKETNO, ticketNum);// NileshJ
            //Added By Dharmendra on Mar-12-09 to pass the user name in case of Payment Processor is either XCHARGE or XLINK
            AddUserID(cardParms);
            //Added Till Here
            //ADDED For HPS
            cardParms.Add(PASSWORD, sPassword);
            cardParms.Add(ALLOWDUP, sALLOWDUP);
            //PrimePOS-2491 
            cardParms.Add(TERMINALID, sTerminalID);

            if (cardInfo.trackII.Equals(string.Empty))
            {
                cardParms.Add(MANUALFLAG, "0");
            }
            else
            {
                if (PAYMENTPROCESSOR.ToUpper().Trim() == "HPS")
                {
                    cardParms.Add(TRACK, cardInfo.Completetrack);
                }
                else
                {
                    cardParms.Add(TRACK, cardInfo.trackII);
                }
                cardParms.Add(MANUALFLAG, "1");
            }

            //Added By SRT(Gaurav) Date : 20-NOV-2008
            if (tempFsaAmount != 0)
            {
                //otc_fsa_amt = tempFsaAmount - tempFsaRxAmount;

                cardParms.Add("FSATRANSACTION", cardInfo.IsFSATransaction);
                cardParms.Add("FSAAMOUNT", tempFsaAmount.ToString("F", ci).PadRight(2, '0'));

                /*if (otc_fsa_amt != 0)
                {
                    cardParms.Add("OTC_FSA_AMT", otc_fsa_amt.ToString("F", ci).PadRight(2, '0'));
                }
                 * */
                if (tempFsaRxAmount != 0)
                {
                    cardParms.Add("FSARXAMOUNT", tempFsaRxAmount.ToString("F", ci).PadRight(2, '0'));
                }
            }
            //End Of Added By SRT(Gaurav)

            //PRIMEPOS-2665 10-Apr-2019 JY Added
            if (PAYMENTPROCESSOR == "XLINK" || PAYMENTPROCESSOR == "HPASPAX" || PAYMENTPROCESSOR == "HPS" || PAYMENTPROCESSOR == "WORLDPAY" || PAYMENTPROCESSOR == "EVERTEC" || PAYMENTPROCESSOR == "VANTIV" || PAYMENTPROCESSOR == "PRIMERXPAY" || PAYMENTPROCESSOR == "ELAVON") // PRIMEPOS-2761 //PRIMEPOS-2841 added by Arvind//2943
            {
                cardParms.Add("StationID", Configuration.StationID);
            }
            //VANTIV Communincation Params PRIMEPOS-2636
            if (PAYMENTPROCESSOR == "VANTIV")
            {
                cardParms.Add("URL", Configuration.CPOSSet.SigPadHostAddr);
                cardParms.Add("APPLICATIONNAME", Configuration.ApplicationName);
                cardParms.Add("STATIONID", Configuration.StationID);
                cardParms.Add(TRANSID, cardInfo.TransactionID);
                cardParms.Add("TRIPOSPATH", Configuration.GetConfigFilePath());//PRIMEPOS-2895 Vantiv Arvind
                //cardParms.Add("lane", Configuration.CPOSSet.SigPadHostAddr);
                cardParms.Add("TransactionRecover", cardInfo.isTransactionRecover.ToString()); //PRIMEPOS-3156
                cardParms.Add("TRANSACTIONURL", Configuration.oMerchantConfig.VantivTokenUrl);//PRIMEPOS-3156
                cardParms.Add("REPORTURL", Configuration.oMerchantConfig.VantivReportUrl);//PRIMEPOS-3156
            }

            try
            {
                resp = null;//Added by Arvind 
                pccRespInfo = null;//Added by Arvind 
                if (PAYMENTPROCESSOR.ToUpper() != "WORLDPAY")
                {
                    primePosLogWriter.AppendPaymentRequestToLogger(cardParms); // Added By Dharmendra on Dec-09-08 to write request parameter's value to logger                     
                    String status = nbsProc.NBSReturn(ref cardParms, out resp);
                    PccResponse objCCResponse = new PccResponse();
                    DecipherResponse(resp, ref objCCResponse);

                    responseStatus = status;
                    responseError = objCCResponse.ResultDescription;
                    objCCResponse.ResponseStatus = status;
                    pccRespInfo = objCCResponse;
                }
            }

            catch (Exception ex)
            {
                logger.Fatal(ex, "PerformCreditSalesReturn()");
                responseError = ex.ToString();
                //Added By Dharmendra (SRT) to logg the error
                pccRespInfo = null;//Added by Arvind 
                resp = null;//Added by Arvind
                primePosLogWriter.AppendCommentsToLogger(responseError + "\r\n");
            }
            finally
            {
                cardParms.Clear();

            }
            //Added By Dharmendra (SRT) on Dec-08-08
            primePosLogWriter.AppendCommentsToLogger("Exit From  PerformCreditSalesReturn() method\r\n------------------------------------\r\n");
            //Added Till Here
        }

        #endregion

        #region PRIMEPOS-3373
        public void PerformVoidOnNBSCardSales(string ticketNum, ref PccCardInfo cardInfo)
        {
            try
            {
                primePosLogWriter.AppendCommentsToLogger("Entered into PerformVoidOnNBSCardSales() method\r\n--------------------------------------------------\r\nRequest Parameters & Their Values\r\n----------------------------------\r\n");

                CultureInfo ci = CultureInfo.CurrentCulture;
                string tempManualFlage = string.Empty;
                string dictObj = string.Empty;
                string trnsdt = string.Empty;
                string track2CleanedData = string.Empty;
                string track2Cleaned = string.Empty;
                Double tempAmount = Convert.ToDouble(cardInfo.transAmount);

                IsSecurityPromptRequired(Transactions.VoidOnDebitCardSales, cardParms);

                if (PAYMENTPROCESSOR == "XLINK")
                {
                    if (cardInfo.tRoutId.Contains("|"))
                        cardInfo.tRoutId = cardInfo.tRoutId.Split('|')[0];
                }

                cardParms.Add(NBSSALETYPE, cardInfo.nbsSaleType);  //PRIMEPOS-3373
                cardParms.Add(TROUTID, cardInfo.tRoutId);
                cardParms.Add(MMSCARD, cardInfo.cardType);
                cardParms.Add(TICKETNO, ticketNum);
                cardParms.Add(ALLOWDUP, sALLOWDUP);
                AddUserID(cardParms);

                cardParms.Add(PASSWORD, sPassword);
                cardParms.Add(TERMINALID, sTerminalID);
                cardParms.Add(AMOUNT, tempAmount.ToString("F", ci).PadRight(2, '0'));

                if (PAYMENTPROCESSOR == "XLINK" || PAYMENTPROCESSOR == "HPSPAX" || PAYMENTPROCESSOR == "HPS" || PAYMENTPROCESSOR == "WORLDPAY" || PAYMENTPROCESSOR == "EVERTEC" || PAYMENTPROCESSOR == "VANTIV" || PAYMENTPROCESSOR == "ELAVON")
                {
                    cardParms.Add("StationID", Configuration.StationID);
                }

                if (PAYMENTPROCESSOR == "VANTIV")
                {
                    cardParms.Add("URL", Configuration.CPOSSet.SigPadHostAddr);
                    cardParms.Add("APPLICATIONNAME", Configuration.ApplicationName);
                    cardParms.Add("STATIONID", Configuration.StationID);
                    if (!cardParms.ContainsKey("AMOUNT"))
                        cardParms.Add("AMOUNT", cardInfo.transAmount);
                    cardParms.Add("TRIPOSPATH", Configuration.GetConfigFilePath());
                    cardParms.Add("TransactionRecover", cardInfo.isTransactionRecover.ToString());
                    cardParms.Add("TRANSACTIONURL", Configuration.oMerchantConfig.VantivTokenUrl);
                    cardParms.Add("REPORTURL", Configuration.oMerchantConfig.VantivReportUrl);
                }

                try
                {
                    resp = null;
                    pccRespInfo = null;
                    if (PAYMENTPROCESSOR.ToUpper() != "WORLDPAY")
                    {
                        primePosLogWriter.AppendPaymentRequestToLogger(cardParms);
                        String status = nbsProc.NBSVoid(ref cardParms, out resp);
                        PccResponse objDBResponse = new PccResponse();
                        DecipherResponse(resp, ref objDBResponse);
                        PrintVoidReceipt(objDBResponse, resp);
                        responseStatus = status;
                        cardInfo.status = status;
                        responseError = objDBResponse.ResultDescription;
                        //objDBResponse.CardType = "Debit Card";
                        objDBResponse.ResponseStatus = status;
                        pccRespInfo = objDBResponse;
                    }
                    else
                    {
                        logger.Trace("Orphan - Start1");
                        cardInfo.StationID = Configuration.StationID;
                        cardInfo.UserId = Configuration.UserName;
                        cardInfo.TicketNo = ticketNum;
                        logger.Trace("Orphan - End1");
                        TransactionResult resp = new TransactionResult();
                        bool result = ProcessWPReverse(TransactionType.Void, cardInfo, out resp);
                        WP_TransResult = resp;

                        PccPaymentSvr.DefaultInstance.EntryMethod = (string.IsNullOrEmpty(WP_TransResult.ResponseTags.EntryMethod) ? WP_TransResult.EntryMethod : WP_TransResult.ResponseTags.EntryMethod);
                    }

                }

                catch (Exception ex)
                {
                    logger.Fatal(ex, "PerformVoidOnNBSCardSales()");
                    responseError = ex.ToString();
                    pccRespInfo = null;
                    resp = null;
                    primePosLogWriter.AppendCommentsToLogger(responseError + "\r\n");
                }
                finally
                {
                    cardParms.Clear();

                }
                primePosLogWriter.AppendCommentsToLogger("Exit From PerformVoidOnNBSCardSales() method\r\n----------------------------------------------\r\n");
               
            }
            catch (Exception ex)
            {

            }
        }
        #endregion
        public bool PerformVoidOnWP(TransactionType type, ref PccCardInfo cardInfo)
        {
            bool result;
            try
            {
                TransactionResult resp = new TransactionResult();
                result = ProcessWPReverse(type, cardInfo, out resp);
                WP_TransResult = resp;
                if (WP_TransResult.Result.Equals("SUCCESS")) // NileshJ - PRIMEPOS-2697
                {
                    PccPaymentSvr.DefaultInstance.EntryMethod = (string.IsNullOrEmpty(WP_TransResult.ResponseTags.EntryMethod) ? WP_TransResult.EntryMethod : WP_TransResult.ResponseTags.EntryMethod);
                }
                else
                {
                    PccPaymentSvr.DefaultInstance.EntryMethod = "";
                }
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "PerformVoidOnWP()");
                responseError = ex.ToString();
                primePosLogWriter.AppendCommentsToLogger(responseError + "\r\n");
                result = false;
            }
            return result;
            primePosLogWriter.AppendCommentsToLogger("Exit From  PerformDebitSale() method\r\n-------------------------------------\r\n");
        }

        /// <summary>
        /// Author : Manoj
        /// Functionality Description : This is for Debit Card(Return) Implementation - Xlink only with Verifone MX870 PinPad 
        /// known Bugs : None
        /// Start Date : 08/24/2011
        /// </summary>
        /// <param name="ticketNum"></param>
        /// <param name="transAmt"></param>
        public void PerformReturnOnDebitCardSalesMX(string ticketNum, ref PccCardInfo cardInfo)
        {
            primePosLogWriter.AppendCommentsToLogger("Entered into PerformReturnOnDebitCardSales() method\r\n--------------------------------------------------\r\nRequest Parameters & Their Values\r\n----------------------------------\r\n");
            CultureInfo ci = CultureInfo.CurrentCulture;
            string tempManualFlage = string.Empty;
            string dictObj = string.Empty;
            string trnsdt = string.Empty;
            string track2CleanedData = string.Empty;
            string track2Cleaned = string.Empty;
            Double tempAmount = Convert.ToDouble(cardInfo.transAmount);

            IsSecurityPromptRequired(Transactions.ReturnOnDebitCardSales, cardParms);

            cardParms.Add(ACCOUNTNUM, "4444222222222222");
            cardParms.Add(EXPDATE, cardInfo.cardExpiryDate);
            cardParms.Add(DEBITPINNO, cardInfo.pinNumber);
            cardParms.Add(DEBITKEYNO, cardInfo.keySerialNumber);
            cardParms.Add(AMOUNT, tempAmount.ToString("F", ci).PadRight(2, '0'));
            cardParms.Add(TRACK, "4444222222222222=15125025432198712345?");
            cardParms.Add(CARDHOLDER, cardInfo.cardHolderName);
            cardParms.Add(TICKETNO, ticketNum);

            AddUserID(cardParms);

            if (cardInfo.trackII.Equals(string.Empty))
            {
                cardParms.Add(MANUALFLAG, "0");
            }
            else
            {
                cardParms.Add(MANUALFLAG, "1");
            }

            cardParms.Add(TIMEOUT, cardInfo.txnTimeOut);
            cardParms.Add(MMSCARD, cardInfo.cardType);
            cardParms.Add(TOTALAMOUNT, tempAmount.ToString("F", ci).PadRight(2, '0'));
            //Added For HPS
            cardParms.Add(ALLOWDUP, sALLOWDUP);
            cardParms.Add(PASSWORD, sPassword);
            try
            {
                primePosLogWriter.AppendPaymentRequestToLogger(cardParms);
                resp = null;//Added by Arvind 
                pccRespInfo = null;//Added by Arvind 
                String status = debitProc.Return(ref cardParms, out resp);
                PccResponse objDBResponse = new PccResponse();
                DecipherResponse(resp, ref objDBResponse);
                //PRIMEPOS-2578 Added CardHolderName Response - Discover Specific -16AUg2018 - NILESHJ
                if (PAYMENTPROCESSOR == "HPSPAX" && resp != null)//PRIMEPOS-3090
                {
                    string ccHolder = string.Empty;
                    resp.GetAllKeys().TryGetValue("CARDHOLDER", out ccHolder);
                    //if (objCCResponse.CardType.ToUpper() == SecureSubmit.Terminals.PAX.CardType.DiSCOVER.ToString().ToUpper())
                    //{
                    cardInfo.cardHolderName = ccHolder;
                    //}
                }
                responseStatus = status;
                responseError = objDBResponse.ResultDescription;

                objDBResponse.CardType = "Debit Card";
                objDBResponse.ResponseStatus = status;
                pccRespInfo = objDBResponse;
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "PerformReturnOnDebitCardSalesMX()");
                responseError = ex.ToString();
                pccRespInfo = null;//Added by Arvind 
                resp = null;//Added by Arvind
                primePosLogWriter.AppendCommentsToLogger(responseError + "\r\n");
            }
            finally
            {
                cardParms.Clear();
            }
            primePosLogWriter.AppendCommentsToLogger("Exit From  PerformReturnOnDebitSale() method\r\n--------------------------------------------\r\n");
        }


        /// <summary>
        /// Author : Manoj
        /// Functionality Description : This is for Debit Card(Return) Implementation - Xlink and HPS only with Verifone MX870 PinPad 
        /// known Bugs : None
        /// Start Date : 08/24/2011
        /// </summary>
        /// <param name="ticketNum"></param>
        /// <param name="transAmt"></param>
        public void PerformReturnOnDebitCardSales(string ticketNum, ref PccCardInfo cardInfo)
        {
            primePosLogWriter.AppendCommentsToLogger("Entered into PerformReturnOnDebitCardSales() method\r\n--------------------------------------------------\r\nRequest Parameters & Their Values\r\n----------------------------------\r\n");
            CultureInfo ci = CultureInfo.CurrentCulture;
            string tempManualFlage = string.Empty;
            string dictObj = string.Empty;
            string trnsdt = string.Empty;
            string track2CleanedData = string.Empty;
            string track2Cleaned = string.Empty;
            Double tempAmount = Convert.ToDouble(cardInfo.transAmount);

            IsSecurityPromptRequired(Transactions.ReturnOnDebitCardSales, cardParms);
            if (PAYMENTPROCESSOR == "HPS")
            {
                cardParms.Add(AMOUNT, tempAmount.ToString("F", ci).PadRight(2, '0'));
                cardParms.Add(TRACK, cardInfo.Completetrack);
                cardParms.Add(DEBITPINNO, cardInfo.pinNumber);
                cardParms.Add(ALLOWDUP, sALLOWDUP);
                cardParms.Add(PASSWORD, sPassword);
                cardParms.Add(TROUTID, cardInfo.tRoutId);
                cardParms.Add(TRANSID, cardInfo.TransactionID);//PRIMEPOS-2738 ADDED BY ARVIND
                AddUserID(cardParms);
            }
            else
            {
                cardParms.Add(ACCOUNTNUM, "4444222222222222");
                cardParms.Add(EXPDATE, cardInfo.cardExpiryDate);
                cardParms.Add(DEBITPINNO, cardInfo.pinNumber);
                cardParms.Add(DEBITKEYNO, cardInfo.keySerialNumber);
                cardParms.Add(AMOUNT, tempAmount.ToString("F", ci).PadRight(2, '0'));
                cardParms.Add(TRACK, "4444222222222222=15125025432198712345?");
                cardParms.Add(CARDHOLDER, cardInfo.cardHolderName);
                cardParms.Add(TICKETNO, ticketNum);
                cardParms.Add(TRANSID, cardInfo.TransactionID);//PRIMEPOS-2738 ADDED BY ARVIND

                AddUserID(cardParms);

                if (cardInfo.trackII.Equals(string.Empty))
                {
                    cardParms.Add(MANUALFLAG, "0");
                }
                else
                {
                    cardParms.Add(MANUALFLAG, "1");
                }

                cardParms.Add(TIMEOUT, cardInfo.txnTimeOut);
                cardParms.Add(MMSCARD, cardInfo.cardType);
                cardParms.Add(TOTALAMOUNT, tempAmount.ToString("F", ci).PadRight(2, '0'));
                //Added For HPS
                cardParms.Add(ALLOWDUP, sALLOWDUP);
                cardParms.Add(PASSWORD, sPassword);
                cardParms.Add(TERMINALID, sTerminalID); //PrimePOS-2491 
            }
            //PRIMEPOS-2528 (Suraj) 1-June-18
            if (PAYMENTPROCESSOR == "HPSPAX")
            {
                cardParms.Add("HOSTADDR", Configuration.CPOSSet.SigPadHostAddr);
                cardParms.Add("TransactionRecover", cardInfo.isTransactionRecover.ToString()); // SAJID LOCAL DETAILS REPORT PRIMEPOS-2862
            }

            #region PRIMEPOS-2784 EVERTEC
            if (PAYMENTPROCESSOR.ToUpper() == "EVERTEC")
            {
                cardParms.Add("ISMANUAL", cardInfo.isManual == true ? "true" : "false");//PRIMEPOS-2805
                cardParms.Add("ISALLOWDUP", cardInfo.isAllowDuplicate == true ? "true" : "false");
                cardParms.Add("TAXDETAIL", cardInfo.EvertecTaxDetails);//2664
            }
            #endregion

            //PRIMEPOS-2665 10-Apr-2019 JY Added
            if (PAYMENTPROCESSOR == "XLINK" || PAYMENTPROCESSOR == "HPSPAX" || PAYMENTPROCESSOR == "HPS" || PAYMENTPROCESSOR == "WORLDPAY" || PAYMENTPROCESSOR == "EVERTEC" || PAYMENTPROCESSOR == "VANTIV" || PAYMENTPROCESSOR == "ELAVON") //PRIMEPOS-2761//2943
            {
                cardParms.Add("StationID", Configuration.StationID);
            }

            //VANTIV Communincation Params PRIMEPOS-2636
            if (PAYMENTPROCESSOR == "VANTIV")
            {
                cardParms.Add("URL", Configuration.CPOSSet.SigPadHostAddr);
                cardParms.Add("APPLICATIONNAME", Configuration.ApplicationName);
                cardParms.Add("STATIONID", Configuration.StationID);
                cardParms.Add(TRANSID, cardInfo.TransactionID);
                cardParms.Add("TRIPOSPATH", Configuration.GetConfigFilePath());//PRIMEPOS-2895 Vantiv Arvind
                //cardParms.Add("lane", Configuration.CPOSSet.SigPadHostAddr);
                cardParms.Add("TransactionRecover", cardInfo.isTransactionRecover.ToString()); //PRIMEPOS-3156
                cardParms.Add("REPORTURL", Configuration.oMerchantConfig.VantivReportUrl); //PRIMEPOS-3156
            }

            if (PAYMENTPROCESSOR == "ELAVON")//2943
            {
                cardParms.Add("TERMINALID", Configuration.CSetting.TerminalID);
                cardParms.Add("CHAINCODE", Configuration.CSetting.ChainCode);
                cardParms.Add("LOCATIONNAME", Configuration.CSetting.LocationName);
                cardParms.Add("TRANSDATE", cardInfo.TransDate);
                cardParms.Add(TRANSID, cardInfo.TransactionID);
                cardParms.Add("ISALLOWDUP", cardInfo.isAllowDuplicate == true ? "true" : "false");
            }

            try
            {
                resp = null;//Added by Arvind 
                pccRespInfo = null;//Added by Arvind 
                if (PAYMENTPROCESSOR.ToUpper() != "WORLDPAY")
                {
                    primePosLogWriter.AppendPaymentRequestToLogger(cardParms);
                    String status = debitProc.Return(ref cardParms, out resp);
                    PccResponse objDBResponse = new PccResponse();
                    DecipherResponse(resp, ref objDBResponse);
                    //PRIMEPOS-2578 Added CardHolderName Response - Discover Specific -16AUg2018 - NILESHJ
                    if (PAYMENTPROCESSOR == "HPSPAX" && resp != null)//PRIMEPOS-3090
                    {
                        string ccHolder = string.Empty;
                        resp.GetAllKeys().TryGetValue("CARDHOLDER", out ccHolder);
                        //if (objCCResponse.CardType.ToUpper() == SecureSubmit.Terminals.PAX.CardType.DiSCOVER.ToString().ToUpper())
                        //{
                        cardInfo.cardHolderName = ccHolder;
                        //}
                    }
                    responseStatus = status;
                    responseError = objDBResponse.ResultDescription;

                    objDBResponse.CardType = "Debit Card";
                    objDBResponse.ResponseStatus = status;
                    pccRespInfo = objDBResponse;
                }
                else
                {
                    #region PRIMEPOS-2761
                    logger.Trace("Orphan - Start1");
                    cardInfo.StationID = Configuration.StationID;
                    cardInfo.UserId = Configuration.UserName;
                    cardInfo.TicketNo = ticketNum;
                    logger.Trace("Orphan - End1");
                    #endregion
                    TransactionResult resp = new TransactionResult();
                    bool result = ProcessWPReverse(TransactionType.Void, cardInfo, out resp);
                    WP_TransResult = resp;
                    if (WP_TransResult.Result.Equals("SUCCESS")) // NileshJ - PRIMEPOS-2697
                    {
                        PccPaymentSvr.DefaultInstance.EntryMethod = (string.IsNullOrEmpty(WP_TransResult.ResponseTags.EntryMethod) ? WP_TransResult.EntryMethod : WP_TransResult.ResponseTags.EntryMethod);
                    }
                    else
                    {
                        PccPaymentSvr.DefaultInstance.EntryMethod = "";
                    }
                }

            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "PerformReturnOnDebitCardSales()");
                responseError = ex.ToString();
                pccRespInfo = null;//Added by Arvind 
                resp = null;//Added by Arvind
                primePosLogWriter.AppendCommentsToLogger(responseError + "\r\n");
            }
            finally
            {
                cardParms.Clear();
            }
            primePosLogWriter.AppendCommentsToLogger("Exit From  PerformReturnOnDebitSale() method\r\n--------------------------------------------\r\n");
        }

        /// <summary>
        /// Author: Gaurav
        /// Mantis Id: 0000118
        /// Date: 01-Dec-2008
        /// </summary>
        /// <param name="objResponse"></param>
        /// <param name="objPccResponse"></param>
        public void PerformVoidOnDebitCardSales(string ticketNum, ref PccCardInfo cardInfo)
        {
            // Added By Dharmendra (SRT) Dec-08-08 to store request & response in the logger file
            primePosLogWriter.AppendCommentsToLogger("Entered into PerformVoidOnDebitCardSales() method\r\n-------------------------------------------------\r\nRequest Parameters & Their Values\r\n----------------------------------\r\n");
            //Added Till Here

            CultureInfo ci = CultureInfo.CurrentCulture;
            string tempManualFlage = string.Empty;
            string dictObj = string.Empty;
            string trnsdt = string.Empty;
            string track2CleanedData = string.Empty;
            string track2Cleaned = string.Empty;
            Double tempAmount = Convert.ToDouble(cardInfo.transAmount);
            cardParms.Add(PASSWORD, sPassword);
            AddUserID(cardParms);

            //ADDED PRASHANT 5 JUN 2010
            IsSecurityPromptRequired(Transactions.VoidOnDebitCardSales, cardParms);
            //END ADDED PRASHANT 5 JUN 2010

            if (PAYMENTPROCESSOR == "XLINK") // NileshJ 22_Feb_2020
            {
                if (cardInfo.tRoutId.Contains("|"))
                    cardInfo.tRoutId = cardInfo.tRoutId.Split('|')[0];
            }

            cardParms.Add(TROUTID, cardInfo.tRoutId);
            cardParms.Add(MMSCARD, cardInfo.cardType);
            cardParms.Add(TICKETNO, ticketNum);
            //ADDED for HPS
            cardParms.Add(AMOUNT, tempAmount.ToString("F", ci).PadRight(2, '0'));

            //PRIMEPOS-2528 (Suraj) 1-June-18
            if (PAYMENTPROCESSOR == "HPSPAX")
            {
                cardParms.Add("HOSTADDR", Configuration.CPOSSet.SigPadHostAddr);
            }

            //PRIMEPOS-2665 10-Apr-2019 JY Added
            if (PAYMENTPROCESSOR == "XLINK" || PAYMENTPROCESSOR == "HPSPAX" || PAYMENTPROCESSOR == "HPS" || PAYMENTPROCESSOR == "WORLDPAY" || PAYMENTPROCESSOR == "EVERTEC" || PAYMENTPROCESSOR == "VANTIV" || PAYMENTPROCESSOR == "ELAVON") //PRIMEPOS-2761//2943
            {
                cardParms.Add("StationID", Configuration.StationID);
            }

            //VANTIV Communincation Params PRIMEPOS-2636
            if (PAYMENTPROCESSOR == "VANTIV")
            {
                cardParms.Add("URL", Configuration.CPOSSet.SigPadHostAddr);
                cardParms.Add("APPLICATIONNAME", Configuration.ApplicationName);
                cardParms.Add("STATIONID", Configuration.StationID);
                cardParms.Add("TRIPOSPATH", Configuration.GetConfigFilePath());//PRIMEPOS-2895 Vantiv Arvind
                if (!cardParms.ContainsKey("AMOUNT"))
                    cardParms.Add("AMOUNT", cardInfo.transAmount);//PRIMEPOS-2795
                //cardParms.Add("lane", Configuration.CPOSSet.SigPadHostAddr);
                cardParms.Add("TransactionRecover", cardInfo.isTransactionRecover.ToString()); //PRIMEPOS-3156
                cardParms.Add("REPORTURL", Configuration.oMerchantConfig.VantivReportUrl); //PRIMEPOS-3156
            }

            if (PAYMENTPROCESSOR == "ELAVON")//2943
            {
                cardParms.Add("TERMINALID", Configuration.CSetting.TerminalID);
                cardParms.Add("CHAINCODE", Configuration.CSetting.ChainCode);
                cardParms.Add("LOCATIONNAME", Configuration.CSetting.LocationName);
                //cardParms.Add("USERDATA", cardInfo.UserData);
                cardParms.Add("ISALLOWDUP", cardInfo.isAllowDuplicate == true ? "true" : "false");
            }

            try
            {
                resp = null;//Added by Arvind 
                pccRespInfo = null;//Added by Arvind 
                if (PAYMENTPROCESSOR.ToUpper() != "WORLDPAY")
                {
                    primePosLogWriter.AppendPaymentRequestToLogger(cardParms);// Added By Dharmendra on Dec-09-08 to write request parameter's value to logger                     
                    String status = debitProc.Void(ref cardParms, out resp);
                    PccResponse objDBResponse = new PccResponse();
                    DecipherResponse(resp, ref objDBResponse);
                    //PRIMEPOS-2578 Added CardHolderName Response - Discover Specific -16AUg2018 - NILESHJ
                    if (PAYMENTPROCESSOR == "HPSPAX" && resp != null)//PRIMEPOS-3090
                    {
                        string ccHolder = string.Empty;
                        resp.GetAllKeys().TryGetValue("CARDHOLDER", out ccHolder);
                        //if (objCCResponse.CardType.ToUpper() == SecureSubmit.Terminals.PAX.CardType.DiSCOVER.ToString().ToUpper())
                        //{
                        cardInfo.cardHolderName = ccHolder;
                        //}
                    }
                    PrintVoidReceipt(objDBResponse, resp);//2664
                    responseStatus = status;
                    responseError = objDBResponse.ResultDescription;
                    //Added By Dharmendra on April-23-09
                    //since in debit card transaction the card type value is not send by actual processor
                    objDBResponse.CardType = "Debit Card";
                    //Added Till Here
                    objDBResponse.ResponseStatus = status;
                    pccRespInfo = objDBResponse;
                }
                else
                {
                    #region PRIMEPOS-2761
                    logger.Trace("Orphan - Start1");
                    cardInfo.StationID = Configuration.StationID;
                    cardInfo.UserId = Configuration.UserName;
                    cardInfo.TicketNo = ticketNum;
                    logger.Trace("Orphan - End1");
                    #endregion
                    TransactionResult resp = new TransactionResult();
                    bool result = ProcessWPReverse(TransactionType.Void, cardInfo, out resp);
                    WP_TransResult = resp;

                    PccPaymentSvr.DefaultInstance.EntryMethod = (string.IsNullOrEmpty(WP_TransResult.ResponseTags.EntryMethod) ? WP_TransResult.EntryMethod : WP_TransResult.ResponseTags.EntryMethod);
                }


            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "PerformVoidOnDebitCardSales()");
                responseError = ex.ToString();
                //Added By Dharmendra (SRT) to logg the error
                pccRespInfo = null;//Added by Arvind 
                resp = null;//Added by Arvind
                primePosLogWriter.AppendCommentsToLogger(responseError + "\r\n");
            }
            finally
            {
                cardParms.Clear();
            }
            // Added By Dharmendra (SRT) Dec-08-08
            primePosLogWriter.AppendCommentsToLogger("Exit From PerformVoidOnDebitCardSales() method\r\n----------------------------------------------\r\n");
            //Added Till Here
        }

        /// <summary>
        /// Author: Gaurav
        /// Mantis Id: 0000118
        /// Date: 01-Dec-2008
        /// </summary>
        /// <param name="objResponse"></param>
        /// <param name="objPccResponse"></param>
        public void PerformVoidOnDebitCardSalesReturn(string ticketNum, ref PccCardInfo cardInfo)
        {
            // Added By Dharmendra (SRT) Dec-08-08 to store request & response in the logger file
            primePosLogWriter.AppendCommentsToLogger("Entered into PerformVoidOnDebitCardSalesReturn(() method\r\n-------------------------------------------------\r\nRequest Parameters & Their Values\r\n----------------------------------\r\n");
            //Added Till Here
            CultureInfo ci = CultureInfo.CurrentCulture;
            string tempManualFlage = string.Empty;
            string dictObj = string.Empty;
            string trnsdt = string.Empty;
            string track2CleanedData = string.Empty;
            string track2Cleaned = string.Empty;
            Double tempAmount = Convert.ToDouble(cardInfo.transAmount);

            //ADDED PRASHANT 5 JUN 2010
            IsSecurityPromptRequired(Transactions.VoidOnDebitCardSalesReturn, cardParms);
            //END ADDED PRASHANT 5 JUN 2010

            if (PAYMENTPROCESSOR == "XLINK") // NileshJ 22_Feb_2020
            {
                if (cardInfo.tRoutId.Contains("|"))
                    cardInfo.tRoutId = cardInfo.tRoutId.Split('|')[0];
            }

            cardParms.Add(TROUTID, cardInfo.tRoutId);
            cardParms.Add(MMSCARD, cardInfo.cardType);
            cardParms.Add(TICKETNO, ticketNum);

            //ADDED for HPS
            cardParms.Add(AMOUNT, tempAmount.ToString("F", ci).PadRight(2, '0'));

            //PRIMEPOS-2528 (Suraj) 1-June-18
            if (PAYMENTPROCESSOR == "HPSPAX")
            {
                cardParms.Add("HOSTADDR", Configuration.CPOSSet.SigPadHostAddr);
            }

            //PRIMEPOS-2665 10-Apr-2019 JY Added
            if (PAYMENTPROCESSOR == "XLINK" || PAYMENTPROCESSOR == "HPSPAX" || PAYMENTPROCESSOR == "HPS" || PAYMENTPROCESSOR == "WORLDPAY" || PAYMENTPROCESSOR == "EVERTEC" || PAYMENTPROCESSOR == "VANTIV" || PAYMENTPROCESSOR == "ELAVON") // PRIMEPOS-2761//2943
            {
                cardParms.Add("StationID", Configuration.StationID);
            }

            //VANTIV Communincation Params PRIMEPOS-2636 
            if (PAYMENTPROCESSOR == "VANTIV")
            {
                cardParms.Add("URL", Configuration.CPOSSet.SigPadHostAddr);
                cardParms.Add("APPLICATIONNAME", Configuration.ApplicationName);
                cardParms.Add("STATIONID", Configuration.StationID);
                //PRIMEPOS-2795
                if (!cardParms.ContainsKey("AMOUNT"))
                    cardParms.Add("AMOUNT", cardInfo.transAmount);
                //
                //cardParms.Add("lane", Configuration.CPOSSet.SigPadHostAddr);
                cardParms.Add("TRIPOSPATH", Configuration.GetConfigFilePath());//PRIMEPOS-2895 Vantiv Arvind
                cardParms.Add("TransactionRecover", cardInfo.isTransactionRecover.ToString()); //PRIMEPOS-3156
                cardParms.Add("REPORTURL", Configuration.oMerchantConfig.VantivReportUrl); //PRIMEPOS-3156
            }
            if (PAYMENTPROCESSOR == "ELAVON")//2943
            {
                cardParms.Add("TERMINALID", Configuration.CSetting.TerminalID);
                cardParms.Add("CHAINCODE", Configuration.CSetting.ChainCode);
                cardParms.Add("LOCATIONNAME", Configuration.CSetting.LocationName);
                //cardParms.Add("USERDATA", cardInfo.UserData);
                cardParms.Add("ISALLOWDUP", cardInfo.isAllowDuplicate == true ? "true" : "false");
            }
            try
            {
                resp = null;//Added by Arvind 
                pccRespInfo = null;//Added by Arvind 
                if (PAYMENTPROCESSOR.ToUpper() != "WORLDPAY")
                {
                    primePosLogWriter.AppendPaymentRequestToLogger(cardParms); // Added By Dharmendra on Dec-09-08 to write request parameter's value to logger                     
                    String status = debitProc.VoidReturn(ref cardParms, out resp);
                    PccResponse objDBResponse = new PccResponse();
                    DecipherResponse(resp, ref objDBResponse);
                    //PRIMEPOS-2578 Added CardHolderName Response - Discover Specific -16AUg2018 - NILESHJ
                    if (PAYMENTPROCESSOR == "HPSPAX" && resp != null)//PRIMEPOS-3090
                    {
                        string ccHolder = string.Empty;
                        resp.GetAllKeys().TryGetValue("CARDHOLDER", out ccHolder);
                        //if (objCCResponse.CardType.ToUpper() == SecureSubmit.Terminals.PAX.CardType.DiSCOVER.ToString().ToUpper())
                        //{
                        cardInfo.cardHolderName = ccHolder;
                        //}
                    }
                    PrintVoidReceipt(objDBResponse, resp);//2664
                    responseStatus = status;
                    responseError = objDBResponse.ResultDescription;
                    //Added By Dharmendra on April-23-09
                    //since in debit card transaction the card type value is not send by actual processor
                    objDBResponse.CardType = "Debit Card";
                    //Added Till Here
                    objDBResponse.ResponseStatus = status;
                    pccRespInfo = objDBResponse;
                }
                else
                {
                    TransactionResult resp = new TransactionResult();
                    bool result = ProcessWPReverse(TransactionType.Void, cardInfo, out resp);
                    WP_TransResult = resp;

                    PccPaymentSvr.DefaultInstance.EntryMethod = (string.IsNullOrEmpty(WP_TransResult.ResponseTags.EntryMethod) ? WP_TransResult.EntryMethod : WP_TransResult.ResponseTags.EntryMethod);
                }


            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "PerformVoidOnDebitCardSalesReturn()");
                responseError = ex.ToString();
                pccRespInfo = null;//Added by Arvind 
                resp = null;//Added by Arvind
                //Added By Dharmendra (SRT) to logg the error
                primePosLogWriter.AppendCommentsToLogger(responseError + "\r\n");
            }
            finally
            {
                cardParms.Clear();
            }
            // Added By Dharmendra (SRT) Dec-08-08
            primePosLogWriter.AppendCommentsToLogger("Exit From PerformVoidOnDebitCardSalesReturn() method\r\n----------------------------------------------\r\n");
            //Added Till Here
        }

        /// <summary>
        /// Author : Ritesh
        /// Functionality Description : This method reverses the transaction perform through Credit card
        /// known Bugs : None
        /// Start Date : 2 Aug 2010
        /// </summary>
        /// <param name="ticketNum"></param>
        /// <param name="transAmt"></param>
        public void PerformReverseOnCreditCardSale(string ticketNum, ref PccCardInfo cardInfo)
        {
            primePosLogWriter.AppendCommentsToLogger("Entered into PerformReverseOnCreditCardSale() method\r\n--------------------------------------------------\r\nRequest Parameters & Their Values\r\n----------------------------------\r\n");

            CultureInfo ci = CultureInfo.CurrentCulture;
            string tempManualFlage = string.Empty;
            string dictObj = string.Empty;
            string trnsdt = string.Empty;

            Double tempAmount = Convert.ToDouble(cardInfo.transAmount);
            Double tempFsaAmount = Convert.ToDouble(cardInfo.transFSAAmount); //Healthcare amt
            Double tempFsaRxAmount = Convert.ToDouble(cardInfo.transFSARxAmount);//rx pr amt
            //Double otc_fsa_amt = 0;//clincamt = Healthcare amt - rx pr amt

            IsSecurityPromptRequired(Transactions.VoidOnCreditCardSales, cardParms);
            cardParms.Add(ACCOUNTNUM, cardInfo.cardNumber);
            cardParms.Add(EXPDATE, cardInfo.cardExpiryDate);
            cardParms.Add(TROUTID, cardInfo.tRoutId);
            cardParms.Add(MMSCARD, cardInfo.cardType);
            //cardParms.Add(TROUTD, cardInfo.tRoutId);
            cardParms.Add(AMOUNT, tempAmount.ToString("F", ci).PadRight(2, '0'));

            AddUserID(cardParms);

            cardParms.Add(PASSWORD, sPassword);

            if (cardInfo.trackII.Equals(string.Empty))
            {
                cardParms.Add(MANUALFLAG, "0");
            }
            else
            {
                if (PAYMENTPROCESSOR.ToUpper().Trim() == "HPS")
                {
                    cardParms.Add(TRACK, cardInfo.Completetrack);
                }
                else
                {
                    cardParms.Add(TRACK, cardInfo.trackII);
                }
                cardParms.Add(MANUALFLAG, "1");
            }

            if ((cardInfo.IsFSATransaction != "0") && (Convert.ToDecimal(cardInfo.transFSAAmount.ToString()) != 0))
            {
                if (Configuration.CPOSSet.PaymentProcessor != "PCCHARGE")
                {
                    cardParms.Add("FSATRANSACTION", cardInfo.IsFSATransaction);
                    cardParms.Add("FSAAMOUNT", tempFsaAmount.ToString("F", ci).PadRight(2, '0'));
                    //if (otc_fsa_amt != 0)
                    //{
                    //    cardParms.Add("OTC_FSA_AMT", otc_fsa_amt.ToString("F", ci).PadRight(2, '0'));
                    //}
                    if (tempFsaRxAmount != 0)
                    {
                        cardParms.Add("FSARXAMOUNT", tempFsaRxAmount.ToString("F", ci).PadRight(2, '0'));
                    }
                }
            }

            //PRIMEPOS-2528 (Suraj) 1-June-18
            if (PAYMENTPROCESSOR == "HPSPAX")
            {
                cardParms.Add("HOSTADDR", Configuration.CPOSSet.SigPadHostAddr);
            }

            //PRIMEPOS-2665 10-Apr-2019 JY Added
            if (PAYMENTPROCESSOR == "XLINK" || PAYMENTPROCESSOR == "HPSPAX" || PAYMENTPROCESSOR == "HPS" || PAYMENTPROCESSOR == "WORLDPAY" || PAYMENTPROCESSOR == "EVERTEC" || PAYMENTPROCESSOR == "VANTIV" || PAYMENTPROCESSOR == "ELAVON") // PRIMEPOS-2761//2943
            {
                cardParms.Add("StationID", Configuration.StationID);
            }

            if (PAYMENTPROCESSOR == "VANTIV") //PRIMEPOS-3156
            {
                cardParms.Add("TransactionRecover", cardInfo.isTransactionRecover.ToString()); //PRIMEPOS-3156
            }

            try
            {
                resp = null;//Added by Arvind 
                pccRespInfo = null;//Added by Arvind 
                if (PAYMENTPROCESSOR.ToUpper() != "WORLDPAY")
                {
                    primePosLogWriter.AppendPaymentRequestToLogger(cardParms);
                    String status = creditProc.Reverse(ref cardParms, out resp);
                    PccResponse objCCResponse = new PccResponse();
                    DecipherResponse(resp, ref objCCResponse);

                    responseStatus = status;
                    responseError = objCCResponse.ResultDescription;
                    objCCResponse.ResponseStatus = status;
                    pccRespInfo = objCCResponse;
                }
                else
                {
                    TransactionResult resp = new TransactionResult();
                    bool result = ProcessWPReverse(TransactionType.Refund, cardInfo, out resp);
                    WP_TransResult = resp;

                    PccPaymentSvr.DefaultInstance.EntryMethod = (string.IsNullOrEmpty(WP_TransResult.ResponseTags.EntryMethod) ? WP_TransResult.EntryMethod : WP_TransResult.ResponseTags.EntryMethod);
                }

            }

            catch (Exception ex)
            {
                logger.Fatal(ex, "PerformReverseOnCreditCardSale()");
                responseError = ex.ToString();
                pccRespInfo = null;//Added by Arvind 
                resp = null;//Added by Arvind
                primePosLogWriter.AppendCommentsToLogger(responseError + "\r\n");
            }
            finally
            {
                cardParms.Clear();
            }
            primePosLogWriter.AppendCommentsToLogger("Exit From PerformReverseOnCreditCardSale() method\r\n----------------------------------------------\r\n");
        }

        /// <summary>
        /// Author : Ritesh
        /// Functionality Description : This method reverses the transaction perform through Debit card
        /// known Bugs : None
        /// Start Date : 6 Aug 2010
        /// </summary>
        /// <param name="ticketNum"></param>
        /// <param name="transAmt"></param>
        public void PerformReverseOnDebitCardSale(string ticketNum, ref PccCardInfo cardInfo)
        {
            primePosLogWriter.AppendCommentsToLogger("Entered into PerformReverseOnCreditCardSale() method\r\n--------------------------------------------------\r\nRequest Parameters & Their Values\r\n----------------------------------\r\n");
            CultureInfo ci = CultureInfo.CurrentCulture;
            string tempManualFlage = string.Empty;
            string dictObj = string.Empty;
            string trnsdt = string.Empty;
            Double tempAmount = Convert.ToDouble(cardInfo.transAmount);

            IsSecurityPromptRequired(Transactions.DebitSale, cardParms);
            cardParms.Add(DEBITPINNO, cardInfo.pinNumber);
            //cardParms.Add(DEBITKEYNO, cardInfo.keySerialNumber);
            cardParms.Add(AMOUNT, tempAmount.ToString("F", ci).PadRight(2, '0'));
            cardParms.Add(TRACK, cardInfo.trackII);
            cardParms.Add(CARDHOLDER, cardInfo.cardHolderName);
            cardParms.Add(MMSCARD, cardInfo.cardType);
            cardParms.Add(ALLOWDUP, sALLOWDUP);
            cardParms.Add(PASSWORD, sPassword);

            AddUserID(cardParms);

            if (cardInfo.trackII.Equals(string.Empty))
            {
                cardParms.Add(MANUALFLAG, "0");
            }
            else
            {
                if (PAYMENTPROCESSOR.ToUpper().Trim() == "HPS")
                {
                    cardParms.Add(TRACK, cardInfo.Completetrack);
                    cardParms.Add(TROUTID, cardInfo.tRoutId);
                }
                else
                {
                    cardParms.Add(TRACK, cardInfo.trackII);
                }
                cardParms.Add(MANUALFLAG, "1");
            }

            //PRIMEPOS-2528 (Suraj) 1-June-18
            if (PAYMENTPROCESSOR == "HPSPAX")
            {
                cardParms.Add("HOSTADDR", Configuration.CPOSSet.SigPadHostAddr);
            }

            //PRIMEPOS-2665 10-Apr-2019 JY Added
            if (PAYMENTPROCESSOR == "XLINK" || PAYMENTPROCESSOR == "HPSPAX" || PAYMENTPROCESSOR == "HPS" || PAYMENTPROCESSOR == "WORLDPAY" || PAYMENTPROCESSOR == "EVERTEC" || PAYMENTPROCESSOR == "VANTIV" || PAYMENTPROCESSOR == "ELAVON") // PRIMEPOS-2761//2943
            {
                cardParms.Add("StationID", Configuration.StationID);
            }

            if (PAYMENTPROCESSOR == "VANTIV") //PRIMEPOS-3156
            {
                cardParms.Add("TransactionRecover", cardInfo.isTransactionRecover.ToString()); //PRIMEPOS-3156
            }

            try
            {
                resp = null;//Added by Arvind 
                pccRespInfo = null;//Added by Arvind 
                if (PAYMENTPROCESSOR.ToUpper() != "WORLDPAY")
                {
                    primePosLogWriter.AppendPaymentRequestToLogger(cardParms);
                    String status = debitProc.Reverse(ref cardParms, out resp);
                    PccResponse objCCResponse = new PccResponse();
                    DecipherResponse(resp, ref objCCResponse);
                    responseStatus = status;
                    responseError = objCCResponse.ResultDescription;
                    objCCResponse.ResponseStatus = status;
                    pccRespInfo = objCCResponse;
                }
                else
                {
                    TransactionResult resp = new TransactionResult();
                    bool result = ProcessWPReverse(TransactionType.Refund, cardInfo, out resp);
                    WP_TransResult = resp;

                    PccPaymentSvr.DefaultInstance.EntryMethod = (string.IsNullOrEmpty(WP_TransResult.ResponseTags.EntryMethod) ? WP_TransResult.EntryMethod : WP_TransResult.ResponseTags.EntryMethod);
                }


            }

            catch (Exception ex)
            {
                logger.Fatal(ex, "PerformReverseOnDebitCardSale()");
                responseError = ex.ToString();
                pccRespInfo = null;//Added by Arvind 
                resp = null;//Added by Arvind
                primePosLogWriter.AppendCommentsToLogger(responseError + "\r\n");
            }
            finally
            {
                cardParms.Clear();
            }
            primePosLogWriter.AppendCommentsToLogger("Exit From PerformReverseOnCreditCardSale() method\r\n----------------------------------------------\r\n");
        }

        private void DecipherResponse(PaymentResponse objResponse, ref PccResponse objPccResponse)
        {
            string value = string.Empty;
            objPccResponse.ClearPccResponseData();
            //Added By Dharmendra (SRT) on Dec-08-08 to log the response values
            primePosLogWriter.AppendCommentsToLogger("Response Parameters & Their Values\r\n----------------------------------\r\n");
            //Added Till Here
            if (resp == null)
            {
                responseStatus = "FAILURE";
                primePosLogWriter.AppendCommentsToLogger("Response is null & hence responsestatus= FAILURE\r\n");
            }
            else
            {
                objPccResponse.Result = objResponse.Result;
                objPccResponse.AuthNo = objResponse.AuthNo;
                objPccResponse.TransId = objResponse.TransactionNo;
                objPccResponse.ResultDescription = objResponse.ResultDescription;
                //Added By SRT(Gaurav) Date:18 NOV-2008
                //Mantis Id: 0000112
                objPccResponse.CardNo = objResponse.MaskedCardNo;
                objPccResponse.Expiry = objResponse.Expiration;
                objPccResponse.ZIP = objResponse.ZIP;
                objPccResponse.Address = objResponse.Address;
                //Added By SRT(Gaurav) Date: 01-Dec-2008
                //Mantis Id: 0000136
                objPccResponse.TrouTd = objResponse.ticketNum;
                //End Of Added By SRT(Gaurav)

                objPccResponse.DateCharged = DateTime.Today.ToShortDateString();
                objPccResponse.TimeCharged = DateTime.Now.ToLongTimeString();
                objPccResponse.TransDate = DateTime.Now.Date.ToString();
                //Added By Dharmendra (SRT) on Nov-26-08
                //Mantis Id: 0000136 - by Gaurav
                Double.TryParse(objResponse.AmountApproved, out objPccResponse.AmountApproved);
                Double.TryParse(objResponse.AdditionalFundsRequired, out objPccResponse.AdditionalFundsRequired);
                objPccResponse.FSATransResult = objResponse.IsFSATransaction;
                logger.Trace("DecipherResponse(PaymentResponse objResponse, ref PccResponse objPccResponse) " + Configuration.convertNullToString(objResponse.CardType));   //PRIMEPOS-2724 22-Aug-2019 JY Added
                objPccResponse.CardType = SetActualCardType(objResponse.CardType);
                objPccResponse.EntryMethod = objResponse.EntryMethod;
                objPccResponse.Balance = objResponse.Balance;  //Added by Manoj 8/24/2011 Xcharge Card Balance
                PccPaymentSvr.DefaultInstance.ProfiledID = objResponse.ProfiledID;
                PccPaymentSvr.DefaultInstance.CashBack = objResponse.CashBack;
                PccPaymentSvr.DefaultInstance.EntryMethod = objResponse.EntryMethod;
                passCCbalance(objPccResponse.Balance); // Added By Manoj
                //Added By SRT(Ritesh Parekh) Date : 23-07-2009
                objPccResponse.PaymentProcessor = Configuration.CPOSSet.PaymentProcessor;
                objPccResponse.BinValue = objResponse.BinValue; //PRIMEPOS-3372
                objPccResponse.NBSPaytype = objResponse.NBSPayType; //PRIMEPOS-3372
                objPccResponse.PaymentType = objResponse.PaymentType; //PRIMEPOS-3526 //PRIMEPOS-3504
                objPccResponse.AccountNo = objResponse.AccountNo; //PRIMEPOS-3372
                objPccResponse.AccountHolderName = objResponse.AccountHolderName; //PRIMEPOS-3372
                objPccResponse.PreReadId = objResponse.PreReadId; //PRIMEPOS-3372

                if (objResponse.EmvReceipt != null)
                {
                    objPccResponse.EmvReceipt = objResponse.EmvReceipt;
                }
                //End Of Added By SRT(Ritesh Parekh)
                primePosLogWriter.AppendPaymentResponseToLogger(objPccResponse); //Added By Dharmendra (SRT) on Dec-08-08 to add payment response data to logger 
                primePosLogWriter.AppendCommentsToLogger("------End of Response Parameters------\r\n"); //Added By Dharmendra (SRT) on Dec-08-08 to add comments in logger
                //End Added
                //Suraj
                if (!string.IsNullOrEmpty(resp.SignatureString))
                {
                    PAX_SigString = resp.SignatureString;
                }
                else
                {
                    PAX_SigString = "";
                }
                //

                //PRIMEPOS-2664
                if (!string.IsNullOrEmpty(resp.SignatureString))
                {
                    EVERTEC_SigString = resp.SignatureString;
                }
                else
                {
                    EVERTEC_SigString = "";
                }
                //

                //PRIMEPOS-2636 VANTIV ADDED BY ARVIND
                if (!string.IsNullOrEmpty(resp.SignatureString))
                {
                    VANTIV_SigString = resp.SignatureString;
                }
                else
                {
                    VANTIV_SigString = "";
                }

                //PRIMEPOS-2943 ELAVON ADDED BY ARVIND
                if (!string.IsNullOrEmpty(resp.SignatureString))
                {
                    ELAVON_SigString = resp.SignatureString;
                }
                else
                {
                    ELAVON_SigString = "";
                }
            }
        }
        private void PrintVoidReceipt(PccResponse objPccResponse, PaymentResponse Resp)//2664
        {
            if (Configuration.CPOSSet.PaymentProcessor == "EVERTEC")
            {
                RxLabel oRxlabel = new RxLabel();
                oRxlabel.isVoidReceipt = true;
                oRxlabel.CardName = objPccResponse.CardType;
                oRxlabel.EvertecDenialDate = DateTime.Now;
                oRxlabel.PaymentProcessor = Configuration.CPOSSet.PaymentProcessor;
                oRxlabel.CardNumber = objPccResponse.CardNo;
                oRxlabel.State = resp.StateTax;
                oRxlabel.Municipal = resp.MunicipalTax;
                oRxlabel.ReduceState = resp.ReduceTax;
                oRxlabel.TotalAmt = resp.TotalAmount;
                oRxlabel.BaseAmt = resp.BaseAmount;
                oRxlabel.Amount = Convert.ToString(objPccResponse.AmountApproved);
                if (objPccResponse.EmvReceipt != null)
                {
                    oRxlabel.AuthNo = objPccResponse.AuthNo;
                    oRxlabel.MerchantID = objPccResponse.EmvReceipt.MerchantID;
                    oRxlabel.InvoiceNumber = objPccResponse.EmvReceipt.InvoiceNumber;
                    oRxlabel.TerminalID = objPccResponse.EmvReceipt.TerminalID;
                    oRxlabel.ReferenceNumber = objPccResponse.EmvReceipt.ReferenceNumber;
                    oRxlabel.Batch = objPccResponse.EmvReceipt.BatchNumber;
                    //oRxlabel.ReferenceNumber = objPccResponse.EmvReceipt.ReferenceNumber;
                }
                if (Resp != null)
                {
                    //oRxlabel.State = resp.stat
                }
                oRxlabel.Print();
            }
        }
        // Added by Manoj to Pass the Credit Card Balance
        public void passCCbalance(string Cbalance)
        {
            if (Cbalance.Trim() != "")
            {
                double cardbalance = Convert.ToDouble(Cbalance);
                if (cardbalance > 0.00)
                {
                    CCBalance = Cbalance;
                }
                else
                {
                    CCBalance = "0.00";
                }
            }
            else
            {
                CCBalance = "0.00";
            }
        }
        public static string SetActualCardType(string cardType)
        {
            string newCardType = string.Empty;
            //ADDED BY ARVIND TO HANDLE CARDTYPE PRIMEPOS-2636 
            if (cardType == null)
            {
                cardType = string.Empty;
            }
            //
            switch (cardType.ToUpper().Trim())
            {
                case "MASTERCARD":
                case "MASTER":
                case "MASTER CARD":
                case "MC":
                    newCardType = "Master Card";
                    break;
                case "VISACARD":
                case "VISA":
                case "VISA CARD":
                    newCardType = "Visa";
                    break;
                case "AMERICANEXPRESSCARD":
                case "AMERICANEXPRESS":
                case "AMERICAN EXPRESS":
                case "AMEX":
                    newCardType = "American Express";
                    break;
                case "DISCOVERCARD":
                case "DISCOVER CARD":
                case "DISCOVER":
                case "DISC":
                    newCardType = "Discover";
                    break;
                case "DEBITCARD":
                case "DEBIT CARD":
                case "DEBIT":
                case "DCCB":
                    newCardType = "Debit Card";
                    break;
                case "ATH MOVIL":
                    newCardType = "ATH MOVIL";//2664
                    break;
                //PRIMEPOS-3057
                default:
                    newCardType = cardType.ToUpper().Trim();
                    break;
            }
            return newCardType;
        }

        private void CleanTrackII(string trackData, out string track2CleanedData)
        {

            if (trackData.Length > 0)
            {
                //; and the second ? are not require so take them out
                trackData.Replace(";", "");
                trackData.Replace("?", "");

            }
            track2CleanedData = trackData;
        }

        public string GetCardType(string CardNumber)
        {
            int iCardType = 0;

            //Added by (SRT)Abhishek  Date : 03/08/2009
            //Added to avoid error if CardNumber length is less than 4
            //and to avoid any parsing exception
            bool isParse = false;
            if (CardNumber.Length > 4)
                isParse = Int32.TryParse(CardNumber.Substring(0, 4), out iCardType);
            //End of Added by (SRT)Abhishek  

            string sCardType = string.Empty;
            if ((iCardType >= 2014 && iCardType <= 2015) || (iCardType >= 2140 && iCardType <= 2150))
            {
                sCardType = "ENRT";
            }
            else if ((iCardType >= 3000 && iCardType <= 3059) || (iCardType >= 3800 && iCardType <= 3899))
            {
                sCardType = "DCCB";
            }
            else if ((iCardType >= 3600 && iCardType <= 3699) || (iCardType >= 5100 && iCardType <= 5599))
            {
                sCardType = "MC";
            }
            else if ((iCardType >= 3400 && iCardType <= 3499) || (iCardType >= 3700 && iCardType <= 3799))
            {
                sCardType = "AMEX";
            }
            else if (iCardType >= 4000 && iCardType <= 4999)
            {
                sCardType = "VISA";
            }
            else if ((iCardType >= 6011) || (iCardType >= 6500 && iCardType <= 6599))
            {
                sCardType = "DISC";
            }
            else
            {
                sCardType = "VISA";
            }
            return sCardType;
        }

        public string ResponseError
        {
            get
            {
                return responseError;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public string ResponseStatus
        {
            get
            {
                return responseStatus;
            }
            set
            {
                responseStatus = value;
            }//Added Setter by SRT (Dharmendra)
        }
        public void ClearPccResponseData()
        {
            oDefObj.ClearPccResponseData();

        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // PccPaymentSvr
            // 
            this.ClientSize = new System.Drawing.Size(292, 266);
            this.Name = "PccPaymentSvr";
            this.ResumeLayout(false);

        }
        //Added By Dharmedra (SRT) on Dec-08-08
        //This method  writes the logger info to C:/PPOSTrace.log file.
        private void LoggerWrite(Object obj)
        {
            if (logValue.Equals("Y"))
            {
                //Logger.Write(obj);
            }
        }
        //Added Till Here
        //PRIMEPOS-2528 (Suraj) 1-June-18 Added For HPSPAX Payment cancellation
        public void CancelTransPAX(string transType)
        {

            switch (transType)
            {
                case "CC":
                    ((MMS.HPSPAX.CreditProcessor)creditProc).CancelTansaction(ref cardParms, out resp);
                    break;
                case "DB":
                    ((MMS.HPSPAX.DebitProcessor)debitProc).CancelTansaction(ref cardParms, out resp);
                    break;
                case "BT":
                    ((MMS.HPSPAX.EBTProcessor)ebtProc).CancelTansaction(ref cardParms, out resp);
                    break;
                default:

                    break;
            }
        }
        //PRIMEPOS-2636 ADDED BY ARVIND 
        public void CancelTransVANTIV(string transType)
        {

            switch (transType)
            {
                case "CC":
                    ((MMS.VANTIV.CreditProcessor)creditProc).CancelTansaction(ref cardParms, out resp);
                    break;
                case "DB":
                    ((MMS.VANTIV.DebitProcessor)debitProc).CancelTansaction(ref cardParms, out resp);
                    break;
                case "BT":
                    ((MMS.VANTIV.EBTProcessor)ebtProc).CancelTansaction(ref cardParms, out resp);
                    break;
                case "NB":
                    ((MMS.VANTIV.NBSProcessor)nbsProc).CancelTansaction(ref cardParms, out resp); //PRIMEPOS-3372
                    break;
                default:

                    break;
            }
        }
        //TILL HERE

    }


    #region By Manoj WP
    public class WPResponse
    {
        public string HistoryID;
        public string OrderID;
        public string Balance;
        public string Last4Digits;
        public string EntryMethod;
        public string PayType;
        public string Status;
        public string AuthCode;
        public string TotalAmt;
        public string CashBackAmt;
        public string PartialApproval;
        public string PaymentProfileID;
        public string Result;
        public bool isFSA;
        public WPResponse()
        {
            HistoryID = string.Empty;
            OrderID = string.Empty;
            Balance = string.Empty;
            Last4Digits = string.Empty;
            EntryMethod = string.Empty;
            PayType = string.Empty;
            Status = string.Empty;
            AuthCode = string.Empty;
            TotalAmt = string.Empty;
            CashBackAmt = string.Empty;
            PartialApproval = string.Empty;
            PaymentProfileID = string.Empty;
            Result = string.Empty;
            isFSA = false;
        }
    }
    #endregion By Manoj WP

    //Added By Dharmendra(SRT) on 08-08-08
    //Purpose of this class is to initialize the members of response data
    public class PccResponse
    {

        public string AuthNo; //AUTHORIZATION
        //Changed By SRT(Gaurav) Date:19 NOV 2008
        //Mantis Id: 0000136
        public string Result; //RESULT
        //End Of Changed By SRT(Gaurav)
        public string TrouTd;  //APPROVALCODE
        public string TransId;  //TRANSID
        public string ResultDescription;
        //Changed By SRT(Gaurav) Date: 19 NOV 2008
        //Change : Removed 'Res_' from member name.
        //Added By SRT(Gaurav) Date : 18 NOV 2008
        //Mantis Id: 0000112
        public string CardNo; //Masked Card No
        public string Expiry;  //Expiry
        public string Address;  //Adderess
        public string ZIP;//ZIP..
        //End Of Added By SRT(Gaurav)
        //End Of Changed By SRT(Gaurav)
        //Runtime populate
        public string TransDate;
        public string DateCharged;
        public string TimeCharged;
        //Added By Dharmendra (SRT) on Nov-26-08
        //Mantis Id: 0000136 - by Gaurav
        public double AmountApproved;
        public double AdditionalFundsRequired;
        public string FSATransResult;
        public string CardType;
        public string EntryMethod;
        public string BinValue; //PRIMEPOS-3372
        public string NBSPaytype; //PRIMEPOS-3372
        public string PaymentType; //PRIMEPOS-3526 //PRIMEPOS-3504
        public string AccountHolderName; //PRIMEPOS-3372
        public string PreReadId; //PRIMEPOS-3372
        public string AccountNo; //PRIMEPOS-3372
        //Added By SRT(Ritesh Parekh) Date : 23-Apr-2009
        public string PaymentProcessor = string.Empty;
        //End Of Added By SRT(Ritesh Parekh)
        public string Balance = string.Empty;   // Added by Manoj 8/24/2011
        public string CashBack = string.Empty;
        public bool isFSATransaction
        {
            get
            {
                bool fsa = false;
                switch (FSATransResult.Trim())
                {
                    case "T":
                        fsa = true;
                        break;
                    case "F":
                        fsa = false;
                        break;
                    default:
                        fsa = false;
                        break;

                }
                return fsa;
            }
        }        //End Added      

        public String ResponseStatus;
        public MMS.PROCESSOR.EmvReceiptTags EmvReceipt
        {
            get
            {
                return PccPaymentSvr.DefaultInstance.EmvTags;
            }
            set
            {
                PccPaymentSvr.DefaultInstance.EmvTags = value;
            }
        }

        public PccResponse()
        {
            ResponseStatus = "";
            AuthNo = "";
            //Changed By SRT(Gaurav) Date: 19 NOV 2008
            //Mantis ID: 000136
            Result = "";
            //End Of Changed By SRT(Gaurav)
            TrouTd = "";
            DateCharged = "";
            TimeCharged = "";
            TransId = "";
            TransDate = "";
            ResultDescription = "";
            //Changed By SRT(Gaurav) Date:19 NOV 2008
            //Added By SRT(Gaurav) Date: 18 NOV 2008
            //Mantis ID: 0000112
            CardNo = "";
            Expiry = "";
            Address = "";
            ZIP = "";
            //End Of Added By SRT(Gaurav)
            //End Of Changed By SRT(Gaurav)
            //Added By Dharmendra (SRT) on Nov-26-08
            //Mantis Id: 0000136 - by Gaurav
            AmountApproved = 0.0D;
            AdditionalFundsRequired = 0.0D;
            FSATransResult = "";
            CardType = "";
            CashBack = "";
            BinValue = ""; //PRIMEPOS-3372
            NBSPaytype = ""; //PRIMEPOS-3375
            AccountHolderName = ""; //PRIMEPOS-3372
            PreReadId = ""; //PRIMEPOS-3372
            AccountNo = ""; //PRIMEPOS-3372
            //End Added      
            //Added By SRT(Ritesh Parekh) Date : 23-07-2009
            PaymentProcessor = Configuration.CPOSSet.PaymentProcessor;
            //End Of Added By SRT(Ritesh Parekh)
        }
        public void ClearPccResponseData()
        {
            ResponseStatus = "";
            AuthNo = "";
            //Changed By SRT(Gaurav) Date: 19 NOV 2008
            Result = "";
            //End Of Changed By SRT(Gaurav)
            TrouTd = "";
            DateCharged = "";
            TimeCharged = "";
            TransId = "";
            TransDate = "";
            ResultDescription = "";
            //Changed Vy SRT(Gaurav) Date: 19 NOV 2008
            //Added By SRT(Gaurav) Date: 18 NOV 2008
            CardNo = "";
            Expiry = "";
            Address = "";
            ZIP = "";
            //End Of Added By SRT(Gaurav)
            //End Of Changed By SRT(Gaurav)
            //Added By Dharmendra (SRT) on Nov-26-08
            AmountApproved = 0.0D;
            AdditionalFundsRequired = 0.0D;
            FSATransResult = "";
            CardType = "";
            CashBack = "";
            //End Added      
            //Added By SRT(Ritesh Parekh) Date : 23-07-2009
            PaymentProcessor = Configuration.CPOSSet.PaymentProcessor;
            //End Of Added By SRT(Ritesh Parekh)
        }

    }

    //Added By Dharmendra(SRT) on 08-08-08
    //Purpose of this class is to initialize the members of response data
    public class PccCardInfo
    {
        public string cardHolderName;
        public string cardNumber;
        public string cardExpiryDate;
        public string cardType;
        public string status;  //PRIMEPOS-3373
        public string zipCode;
        public string trackII;
        public string Completetrack;
        public string customerAddress;
        public string transAmount;
        public string pinNumber;
        public string keySerialNumber;
        public string txnTimeOut;
        //Added By SRT(Gaurav) Date: 19-NOV-2008
        //Mantis ID: 0000136
        public string IsFSATransaction;//Values-'U'-Unknown,'F'-False,'T'-True
        public string transFSAAmount;
        //End Of Added By SRT(Gaurav)
        //Added By SRT(Gaurav) Date: 24-NOV-2008
        //Mantis ID: 0000136
        public string tRoutId;
        //End Of Added By SRT(Gaurav)
        //Added By SRT(Ritesh Parekh) Date : 23-Apr-2009
        public string PaymentProcessor = string.Empty;
        //End Of Added By SRT(Ritesh Parekh)
        //Added By SRT(Ritesh Parekh) Date: 18-Aug-2009
        //This amount holds IIASRxAmount currently required for XLINK, XCHARGE Payment processes.
        public string transFSARxAmount;
        //End Of Added By SRT(Ritesh Parekh)
        public string Balance; // Added by Manoj 8/24/2011
        public double TxnAuthAmt;
        public string IsCardPresent;
        //Added by Rohit Nair on 
        public string ProfileID;
        // Added By Rohit Nair on 05/02/2016
        public string Last4;
        public string preReadId; //PRIMEPOS-3372
        public string preReadType; //PRIMEPOS-3407
        public string nbsSaleType; //PRIMEPOS-3375
        public string returnTransType; //PRIMEPOS-3521 //PRIMEPOS-3522 //PRIMEPOS-3504
        public bool Tokenize;
        public bool UseToken;
        public bool IsFsaCard = false;//2990
        public string OrderID;
        public string TransactionID;
        public string CVVCode;  //Sprint-27 - PRIMEPOS-2301 07-Sep-2017 JY Added
        public string HrefNumber = string.Empty;//PRIMEPOS-2738 ADDED BY ARVIND
        #region PRIMEPOS-2761
        public string Transtype;
        public string TerminalRefNumber;
        public string TicketNo;
        public string StationID;
        public string UserId;
        public string CardType;
        #endregion
        public bool isAllowDuplicate = false;//PRIMEPOS-2784
        public bool isNBSTransaction = false;//PRIMEPOS-3526 //PRIMEPOS-3504
        public bool isManual = false;//PRIMEPOS-2805
        public bool isFSACard = false;//PRIMEPOS-2841
        public bool isTransactionRecover = false;// SAJID LOCAL DETAILS REPORT PRIMEPOS-2862PRIMEPOS-2862

        #region PRIMEPOS-2915
        public bool IsEmail = false;
        public bool IsPhone = false;
        public string CustomerName = "";
        public string Email = "";
        public string Phone = "";
        public string DOB = "";
        public bool IsPrimeRxPayLinkSend = false; //PRIMEPOS-3248
        public bool IsCustomerDriven = false;
        public bool IsVoidCustomerDriven = false;
        public bool IsVoidPayComplete = false;
        #endregion

        public string TransDate = string.Empty;//2943
        public bool IsElavonTax;//2943
        public string ElavonTotalTax = string.Empty;//2943

        public string EvertecTaxDetails = string.Empty;//2664
        public bool isFood = false;//PRIMEPOS-2664

        public bool IsFondoUnica = false;//PRIMEPOS-2664
        public string OriginalTransactionIdentifier = "";//PRIMEPOS-3146
        public PccCardInfo()
        {

            cardHolderName = "";
            cardNumber = "";
            cardExpiryDate = "";
            cardType = "";
            zipCode = "";
            trackII = "";
            Completetrack = "";
            customerAddress = "";
            transAmount = "";
            pinNumber = "";
            keySerialNumber = "";
            txnTimeOut = "";
            //Added By SRT(Gaurav) Date: 19-NOV-2008
            //Mantis ID: 0000136
            IsFSATransaction = "";
            transFSAAmount = "";
            //End Of Added By SRT(Gaurav)
            //Added By SRT(Gaurav) Date: 01-Dec-2008
            //Mantis Id: 0000136
            tRoutId = "";
            //End Of Added By SRT(Gaurav)
            //Added By SRT(Ritesh Parekh) Date : 23-07-2009
            PaymentProcessor = Configuration.CPOSSet.PaymentProcessor;
            //End Of Added By SRT(Ritesh Parekh)
            //Added By sRT(Ritesh Parekh) Date: 18-Aug-2009
            transFSARxAmount = "";
            //End Of Added By SRT(Ritesh Parekh)
            Balance = ""; // Added by Manoj 8/24/2011
            IsCardPresent = "";
            // Added By Rohit Nair on 05/02/2016
            ProfileID = "";
            Last4 = "";
            preReadId = ""; //PRIMEPOS-3372
            preReadType = ""; //PRIMEPOS-3407
            Tokenize = false;
            UseToken = false;

            OrderID = "";
            TransactionID = "";
            CVVCode = "";   //Sprint-27 - PRIMEPOS-2301 07-Sep-2017 JY Added
            #region PRIMEPOS-2761
            Transtype = "";
            TerminalRefNumber = "";
            TicketNo = "";
            StationID = "";
            UserId = "";
            #endregion
            OriginalTransactionIdentifier = "";//PRIMEPOS-3146
        }
        public void ClearPccCardInfo()
        {
            cardHolderName = "";
            cardNumber = "";
            cardExpiryDate = "";
            cardType = "";
            zipCode = "";
            trackII = "";
            Completetrack = "";
            customerAddress = "";
            transAmount = "";
            pinNumber = "";
            keySerialNumber = "";
            txnTimeOut = "";
            //Added By SRT(Gaurav) Date: 19-NOV-2008
            //Mantis Id: 0000136
            IsFSATransaction = "";
            transFSAAmount = "";
            //End Of Added By SRT(Gaurav)
            //Added By SRT(Gaurav) Date: 01-Dec-2008
            //Mantis Id: 0000136
            tRoutId = "";
            //End Of Added By SRT(Gaurav)
            //Added By SRT(Ritesh Parekh) Date : 23-07-2009
            PaymentProcessor = Configuration.CPOSSet.PaymentProcessor;
            //End Of Added By SRT(Ritesh Parekh)
            //Added By sRT(Ritesh Parekh) Date: 18-Aug-2009
            transFSARxAmount = "";
            //End Of Added By SRT(Ritesh Parekh)
            Balance = ""; // Added by Manoj 8/24/2011
            IsCardPresent = "";

            // Added By Rohit Nair on 05/02/2016
            ProfileID = "";
            Last4 = "";
            Tokenize = false;
            UseToken = false;

            OrderID = "";
            TransactionID = "";
            CVVCode = "";   //Sprint-27 - PRIMEPOS-2301 07-Sep-2017 JY Added
            #region PRIMEPOS-2761
            Transtype = "";
            TerminalRefNumber = "";
            TicketNo = "";
            StationID = "";
            UserId = "";
            cardType = "";//PRIMEPOS-3081
            #endregion
            OriginalTransactionIdentifier = "";//PRIMEPOS-3146
        }
        public PccCardInfo Copy()
        {
            PccCardInfo oPCCInfo = new PccCardInfo();

            oPCCInfo.cardHolderName = cardHolderName;
            oPCCInfo.cardNumber = cardNumber;
            oPCCInfo.cardExpiryDate = cardExpiryDate;
            oPCCInfo.cardType = cardType;
            oPCCInfo.zipCode = zipCode;
            oPCCInfo.trackII = trackII;
            oPCCInfo.Completetrack = Completetrack;
            oPCCInfo.customerAddress = customerAddress;
            oPCCInfo.transAmount = transAmount;
            oPCCInfo.pinNumber = pinNumber;
            oPCCInfo.keySerialNumber = keySerialNumber;
            oPCCInfo.txnTimeOut = txnTimeOut;
            //Added By SRT(Gaurav) Date: 19-NOV-2008
            //Mantis Id: 0000136
            oPCCInfo.IsFSATransaction = IsFSATransaction;
            oPCCInfo.transFSAAmount = transFSAAmount;
            oPCCInfo.Balance = Balance; // Added By Manoj 8/24/2011
            oPCCInfo.IsCardPresent = IsCardPresent;
            //End Of Added By SRT(Gaurav)
            //Added By SRT(Gaurav) Date: 01-Dec-2008
            //Mantis Id: 0000136
            oPCCInfo.tRoutId = tRoutId;
            //End Of Added By SRT(Gaurav)
            //Added By SRT(Ritesh Parekh) Date : 23-07-2009
            oPCCInfo.PaymentProcessor = PaymentProcessor;
            //End Of Added By SRT(Ritesh Parekh)
            oPCCInfo.transFSARxAmount = transFSARxAmount;

            // Added By Rohit Nair on 05/02/2016
            oPCCInfo.ProfileID = ProfileID;
            oPCCInfo.Last4 = Last4;
            oPCCInfo.Tokenize = Tokenize;
            oPCCInfo.UseToken = UseToken;
            oPCCInfo.CardType = cardType;//PRIMEPOS-3081
            oPCCInfo.TransactionID = TransactionID;
            oPCCInfo.OrderID = OrderID;
            oPCCInfo.CVVCode = CVVCode; //Sprint-27 - PRIMEPOS-2301 07-Sep-2017 JY Added
            #region PRIMEPOS-2761
            oPCCInfo.Transtype = Transtype;
            oPCCInfo.TerminalRefNumber = TerminalRefNumber;
            oPCCInfo.TicketNo = TicketNo;
            oPCCInfo.StationID = StationID;
            oPCCInfo.UserId = UserId;
            #endregion
            oPCCInfo.OriginalTransactionIdentifier = OriginalTransactionIdentifier;//PRIMEPOS-3146
            return oPCCInfo;
        }

    }
    //Added By Dharmendra on Dec-06-08
    //The purpose of this class is to provide functionality for Appending transaction related
    //parameters into PPOSTrace.log file.
    public class PrimePosLogWriter
    {

        private String loggerString = String.Empty;
        private string isTraceEnable = string.Empty;
        //This a constructor which requires the value whether tracing is enabled or not
        public PrimePosLogWriter(string isTraceEnable)
        {
            this.isTraceEnable = isTraceEnable;
        }
        public void AppendCommentsToLogger(string comments)
        {
            //LoggerWrite(comments);
        }
        public void AppendPaymentRequestToLogger(MMSDictionary<string, string> paymentRequest)
        {
            foreach (KeyValuePair<string, string> kvp in paymentRequest)
            {
                #region PRIMEPOS-3057 Start Handle null value
                if (!string.IsNullOrWhiteSpace(kvp.Key) && !string.IsNullOrWhiteSpace(kvp.Value))
                {
                    loggerString += kvp.Key.ToString().Trim() + "= " + kvp.Value.ToString().Trim() + "\r\n";
                }
                #endregion PRIMEPOS-3057 End
            }
            LoggerWrite(loggerString);
            loggerString = "";
        }
        public void AppendPaymentResponseToLogger(PccResponse objPccResponse)
        {

            loggerString += "Result= " + objPccResponse.Result + "\r\n";
            loggerString += "ResponseStatus= " + objPccResponse.ResponseStatus + "\r\n";
            loggerString += "Card Processor= " + objPccResponse.PaymentProcessor + "\r\n";
            loggerString += "CardNo= " + objPccResponse.CardNo + "\r\n";
            loggerString += "Expiry= " + objPccResponse.Expiry + "\r\n";
            loggerString += "Address= " + objPccResponse.Address + "\r\n";
            loggerString += "ZIP= " + objPccResponse.ZIP + "\r\n";
            loggerString += "AuthNo= " + objPccResponse.AuthNo + "\r\n";
            loggerString += "AmountApproved= " + objPccResponse.AmountApproved + "\r\n";
            loggerString += "AdditionalFundsRequired= " + objPccResponse.AdditionalFundsRequired + "\r\n";
            loggerString += "FSATransResult= " + objPccResponse.FSATransResult + "\r\n";
            loggerString += "TrouTd= " + objPccResponse.TrouTd + "\r\n";
            loggerString += "TransId= " + objPccResponse.TransId + "\r\n";
            loggerString += "DateCharged= " + objPccResponse.DateCharged + "\r\n";
            loggerString += "TimeCharged= " + objPccResponse.TimeCharged + "\r\n";
            loggerString += "IsFSATransaction= " + objPccResponse.isFSATransaction.ToString() + "\r\n";
            loggerString += "Card Balance= " + objPccResponse.Balance.ToString() + "\r\n";
            loggerString += "Result Description=\r\n" + objPccResponse.ResultDescription + "\r\n";
            LoggerWrite(loggerString);
            loggerString = "";
        }
        //Added By Dharmedra (SRT) on Dec-08-08
        //This method  writes the logger info to C:/PPOSTrace.log file.
        private void LoggerWrite(Object obj)
        {
            if (isTraceEnable.ToUpper().Equals("Y"))
            {
                //Logger.Write(obj);
            }
        }
        //Added Till Here
    }

}
