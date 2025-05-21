//Author : Ritesh 
//Copy Right : © Micro Merchant Systems, Inc 2008
//Functionality Desciption : The purpose of this class is to make base processor for transactions.
//External functions:None   
//Known Bugs : None
//Start Date : 2 Feb 2008.
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Web;
using MMS.PROCESSOR;
using System.Xml;
using System.Diagnostics;
//using Logger = AppLogger.AppLogger;
using PossqlData;
using NLog;
using System.Linq;//PRIMEPOS-2761

namespace MMS.XLINK
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
        private MMSDictionary<String, MMSDictionary<String, String>> MandatoryKeys = null;
        private MMSDictionary<String, String> ValidKeys = null;
        private MMSDictionary<String, String> MessageFields = null;
        //private SocketClient PaymentConn = null;
        private XmlToKeys XmlToKeys = null;
        private KeysToXml Fields = null;
        private PaymentResponse response = null;
        private String errorMessage = String.Empty;
        private String txnType = String.Empty;
        private String ResponseMessage = String.Empty;
        private MerchantInfo Merchant = null;
        private Boolean Disposed = false;
        XmlDocument xmlXLINK = null;
        //Added By SRT(Gaurav) Date : 08 NOV 2008
        private String AppPath = String.Empty;
        //End Of Added By SRT(Gaurav)
        //Added By SRT(Gaurav) Date : 14 NOV 2008
        private String ResultFilePath = String.Empty;

        private decimal amount = 0.00m;
        //End Of Added By SRT(Gaurav)
        #region PRIMEPOS-2761
        private string sUserID = string.Empty;
        private string sStationID = string.Empty;
        private string sTicketNumber = string.Empty;
        private string sTransType = string.Empty;
        private string sXCTRANSACTIONID = string.Empty;
        public long TransNo = 0;
        #endregion
        #endregion

        #region constants
        private const String MSG_HEADER = "<TRANSACTION>";
        private const String MSG_FOOTER = "</TRANSACTION>";
        private const String XLCTRANSACTION = "<XLCTRANSACTION>";
        private const String ENDXLCTRANSACTION = "</XLCTRANSACTION>";
        private const String RESPONSE_FILTERNODE = "XCEXPRESSLINKRESULT";
        private const String VALID_FIELDS = "ValidFields.xml";
        private const String MANDATORY_FIELDS = "MandatoryFields.xml";
        private const String EOT = "\x04"; //EOT required for each transaction in XLINK
        public const String FAILED_OPRN = "FAILED";
        public const String INVALID_PARAMETERS = "INVALID_PARAMETERS";
        private const string COMMOMPROCESSORTAG = "CommonProcessorTag.xml";
        private const string ERRORRESPONSE = "<XML_REQUEST><RESULT_SUCCESS>FAILURE</RESULT_SUCCESS><RESULT_APPROVALCODE></RESULT_APPROVALCODE><RESULT_ITEMNO></RESULT_ITEMNO><RESULT_AUTHCODE></RESULT_AUTHCODE><RESULT_BATCHNO></RESULT_BATCHNO><RESULT_AVSCODE></RESULT_AVSCODE><RESULT_TRANSID></RESULT_TRANSID><RESULT_TICKET></RESULT_TICKET><RESULT_DESCRIPTION>PAYMENT SERVER COULD NOT BE CONNECTED</RESULT_DESCRIPTION><TROUTD></TROUTD></XML_REQUEST>";
        //Added By Dharmendra (SRT) on Dec-09-08 to define tag name constants
        private const string OPENXMLTRANSAMOUNT = "<AMOUNT>";
        private const string CLOSEXMLTRANSAMOUNT = "</AMOUNT>";

        public const String CLOUD_RESP_FILTERNODE = "XLinkEMVResult";
        //The location and port of RCM
        private const string RCMURL = "https://localsystem.paygateway.com:21113/RcmService.svc/";
        //The RCM method to call
        private const string RCMMETHOD = "Initialize";
        //The start of the querystring for sending the parameters
        private const string QUERYSTRING = "?callback=xmlResponse&xl2Parameters=";
        private string sWebTerminalID = string.Empty;
        private bool IsCloudService = false;
        //
        #endregion
        public string error
        {
            //Property for error
            get
            {
                return errorMessage;
            }
            set
            {
                errorMessage = value;
            }
        }
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
            Merchant = merchant;
            AppPath = merchant.oMerchantInfo.Payment_Client;

            /*if (merchant.oMerchantInfo == null)
            {
                return;
            }*/

            //Added By SRT(Gaurav) Date : 14 NOV 2008
            ResultFilePath = merchant.oMerchantInfo.Payment_ResultFile;
            //End Of Added By SRT(Gaurav)

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
            response = new XLinkPaymentResponse();
            xmlXLINK = new XmlDocument();
            xmlXLINK.Load(COMMOMPROCESSORTAG);
            //Get reference to the Socket Client to Payment Server.
            //PaymentConn = new SocketClient("",0);
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
        private String BuildRequest(string strResultFilePath)
        {
            bool promptForPwd = false;

            String reqMessage = "";
            int count = 0;
            foreach (KeyValuePair<String, String> kvp in MessageFields)
            {
                //ADDED PRASHANT 5 JUN 2010

                if (kvp.Key.Trim() == "PROMPT")
                {
                    promptForPwd = true;
                    continue;
                }
                //END ADDED PRASHANT 5 JUN 2010

                if (kvp.Key.Trim() == kvp.Value.ToString())
                {
                    reqMessage += " /" + kvp.Key.Trim();
                }
                //Added By SRT(Gaurav ) Date: 13 NOV 2008
                //ISSUE: Fix up Parameter Pass Issue
                else if (kvp.Value.IndexOf(" ") > -1)
                {
                    reqMessage += " " + Convert.ToChar(34) + "/" + kvp.Key.Trim() + ":" + kvp.Value.ToString() + Convert.ToChar(34);
                }
                //End Of Added By SRT(Gaurav)
                //Changed By SRT(Gaurav) Date: 13 NOV 2008
                else
                {
                    reqMessage += " /" + kvp.Key.Trim() + ":" + kvp.Value.ToString();
                }
                //End Of Changed By SRT(Gaurav) 
                count++;

                if (kvp.Key.Trim().Equals("AMOUNT"))
                {
                    try
                    {
                        amount = Convert.ToDecimal(kvp.Value.ToString());
                    }
                    catch (Exception ex)
                    {
                        logger.Trace(ex.ToString());
                    }

                }
                #region PRIMEPOS-2761
                if (kvp.Key.Trim().Equals("USERID"))
                {
                    try
                    {
                        sUserID = kvp.Value.ToString();
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
                        sStationID = kvp.Value.ToString();
                    }
                    catch (Exception ex)
                    {
                        logger.Trace(ex.ToString());
                    }

                }
                if (kvp.Key.Trim().Equals("RECEIPT"))
                {
                    try
                    {
                        sTicketNumber = kvp.Value.ToString();
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
                        sTransType = kvp.Value.ToString();
                    }
                    catch (Exception ex)
                    {
                        logger.Trace(ex.ToString());
                    }

                }
                if (kvp.Key.Trim().Equals("XCTRANSACTIONID"))
                {
                    try
                    {
                        sXCTRANSACTIONID = kvp.Value.ToString();
                    }
                    catch (Exception ex)
                    {
                        logger.Trace(ex.ToString());
                    }

                }
                #endregion
            }
            //Added By SRT(Gaurav) Date : 14 NOV 2008
            if (strResultFilePath.IndexOf(" ") > -1)
            {
                reqMessage += " " + Convert.ToChar(34) + "/RESULTFILE:" + strResultFilePath + Convert.ToChar(34) + " /XMLRESULTFILE "; //PRIMEPOS-2717 24-Jul-2019 JY modified
            }
            else
            {
                reqMessage += " /RESULTFILE:" + strResultFilePath + " /XMLRESULTFILE ";
            }

            reqMessage += " /RECEIPTINRESULT /RECEIPTINRESULTFORMAT:FIELDVALUES ";
            //End Of Added By SRT(Gaurav)
            //Modified By Dharmendra (SRT) on Dec-16-08 Removed the tag /AUTOPROCESS  to prevent the display of error message window
            // on xcharge client which was showing Invalid card message            
            reqMessage += "/TITLE:\"" + Merchant.oMerchantInfo.XCClientUITitle + "\"";

            //ADDED PRASHANT 5 JUN 2010
            if (promptForPwd)
            {
                reqMessage += " /EXITWITHESCAPEKEY /USERID:POS/PASSWORD:PRIMEX";
            }
            else
            {
                reqMessage += " /EXITWITHESCAPEKEY /USERID:POS /PASSWORD:PRIMEX";
            }
            //END ADDED PRASHANT 5 JUN 2010
            // Add Tag for PartialApprovalSupport required by Xcharge as of 8/15/2011 (/PARTIALAPPROVALSUPPORT:T)
            reqMessage += " /AUTOCLOSE /PARTIALAPPROVALSUPPORT:T /SMALLWINDOW /STAYONTOP /LOCKAMOUNT /LOCKRECEIPT /LOCKTRANTYPE /LOCKCLERK ";

            //Modified By Dharmendra (SRT) on Dec-26-08 Added extra tags to prevent xcharge client from prompting for
            //passwrod,to changed window title,to lock user,to disable result dialogbox /AUTOPROCESS removed
            reqMessage += "/HIDEMAINMENU /SMARTAUTOPROCESS /TOOLBAREXITBUTTON /NORESULTDIALOG ";

            //Added By Manoj Xcharge does not process the EBT Card Auto
            if (reqMessage.Contains("EBTReturn"))
            {
                reqMessage += "/AUTOPROCESS";
            }
            //Added to ensure right behavior exists for ResultFile.txt
            //End Modified Dec-16-08
            return reqMessage;
        }

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
            logger.Trace("Start Processor (XLINK ProcessTxn) ----");
            //Clear all the fields.                 
            ClearFields();
            MessageFields = requestMsgKeys;
            txnType = transactionType;
            String value = String.Empty;
            string updatedResponse = string.Empty;
            string requestAmount = string.Empty;
            string strErrorMessage = string.Empty;  //PRIMEPOS-2717 26-Jul-2019 JY Added      

            if (IsValidRequest())
            {
                ResponseMessage = String.Empty;
                bool rtnCode = false;
                if (!IsCloudService)
                {
                    string StationID = string.Empty;
                    requestMsgKeys.TryGetValue("StationID", out StationID); //PRIMEPOS-2665 11-Apr-2019 JY Added
                    rtnCode = SendReceiveData(StationID, ref strErrorMessage);
                }
                else
                    rtnCode = CloudSendReceiveData();

                if (rtnCode)
                {

                    if (ResponseMessage.Contains(EOT))
                    {
                        updatedResponse = ResponseMessage.Replace(EOT, "");
                    }
                    else
                    {
                        updatedResponse = ResponseMessage;
                    }
                }
                else
                {
                    if (strErrorMessage == string.Empty)
                    {
                        //default response message 
                        if (!IsCloudService)
                            updatedResponse = ERRORRESPONSE;
                        else
                            updatedResponse = ResponseMessage;
                    }
                }

                if (strErrorMessage == string.Empty)
                {
                    logger.Trace("Sending Data to XLINK");
                    if (!IsCloudService)
                        response.ParseResponse(updatedResponse, RESPONSE_FILTERNODE);
                    else
                        response.ParseResponse(updatedResponse, CLOUD_RESP_FILTERNODE);
                    logger.Trace("END PROCESSOR(XLINK ProcessTxn)");
                    #region PRIMEPOS-2761

                    long OrgTransNo = 0;
                    using (var db = new Possql())
                    {
                        CCTransmission_Log cclog1 = new CCTransmission_Log();


                        if (sTransType.ToUpper().Contains("VOID"))
                        {
                            cclog1 = db.CCTransmission_Logs.Where(w => w.HostTransID.Contains(sXCTRANSACTIONID)).SingleOrDefault();
                            cclog1.IsReversed = true;
                            OrgTransNo = cclog1.TransNo;
                            db.CCTransmission_Logs.Attach(cclog1);
                            db.Entry(cclog1).Property(p => p.IsReversed).IsModified = true;
                            db.SaveChanges();
                        }
                    }

                    using (var db = new Possql())
                    {
                        CCTransmission_Log cclog = new CCTransmission_Log();
                        cclog = db.CCTransmission_Logs.Where(w => w.TransNo == TransNo).SingleOrDefault();
                        cclog.AmtApproved = amount;
                        cclog.RecDataStr = ResponseMessage;
                        cclog.HostTransID = response.TransactionNo;
                        cclog.TransmissionStatus = "Completed";
                        cclog.OrgTransNo = OrgTransNo;
                        cclog.ResponseMessage = response.Result;
                        #region PRIMEPOS-3182
                        if (!string.IsNullOrWhiteSpace(response.AccountNo) && response.AccountNo.Trim().Length >= 4)
                        {
                            cclog.last4 = response.AccountNo.Trim().Substring(response.AccountNo.Trim().Length - 4, 4);
                        }
                        #endregion
                        db.CCTransmission_Logs.Attach(cclog);
                        db.Entry(cclog).Property(p => p.AmtApproved).IsModified = true;
                        db.Entry(cclog).Property(p => p.RecDataStr).IsModified = true;
                        db.Entry(cclog).Property(p => p.HostTransID).IsModified = true;
                        db.Entry(cclog).Property(p => p.TransmissionStatus).IsModified = true;
                        db.Entry(cclog).Property(p => p.OrgTransNo).IsModified = true;
                        db.Entry(cclog).Property(p => p.ResponseMessage).IsModified = true;
                        db.Entry(cclog).Property(p => p.last4).IsModified = true;   //PRIMEPOS-3182
                        db.SaveChanges();
                    }
                    #endregion
                }
                else
                {
                    response.Result = "FAILURE";
                    response.ResultDescription = strErrorMessage;
                }
                return response;
            }
            return null;
        }

        /// <summary>
        /// Author : Ritesh 
        /// Functionality Desciption : This method is called to Open,Send,Receive and disconnect the Payment Server.
        /// External functions:None
        /// Known Bugs : None
        /// Start Date : 14 Jan 2008.
        /// </summary>
        /// <param name="StationID">String</param>  
        /// <returns>bool</returns> 
        private bool SendReceiveData(string StationID, ref string strErrorMessage)  //PRIMEPOS-2665 11-Apr-2019 JY Added StationID parameter
        {
            #region PRIMEPOS-2665 11-Apr-2019 JY Added
            string ResultFileName, strResultFilePath;
            if (StationID != "")
                ResultFileName = "ResultFile_" + StationID + ".txt";
            else
                ResultFileName = "ResultFile.txt";

            if (ResultFilePath.Trim().EndsWith(".txt", StringComparison.OrdinalIgnoreCase))
                strResultFilePath = Path.GetDirectoryName(ResultFilePath);
            else
                strResultFilePath = ResultFilePath;

            #region PRIMEPOS-2717 24-Jul-2019 JY Added
            bool bStatus = IsDirectoryExistAndWritable(strResultFilePath);
            if (bStatus == false)
            {
                //strErrorMessage = "Resultfile path: " + strResultFilePath + "\n not found or access denied, please reset it from \"Preferences -> Merchant Config -> Payment Result File\" settings and try again";
                strErrorMessage = "ResultFile.txt located in path " + strResultFilePath + "\n not found or inaccessible. Please check with your system or network administrator to ensure access to this path.";
                return false;
            }
            #endregion

            strResultFilePath += @"\" + ResultFileName;
            #endregion

            String reqMessage = BuildRequest(strResultFilePath);
            //Added By Dharmendra (SRT) on Dec-15-08
            bool isRequestProcessed = false;
            //End Added
            //String reqMessage = "";

            //Added By SRT(Gaurav) Date: 06-NOV-2008
            Process tmpProcess = new Process();
            tmpProcess.StartInfo.Arguments = reqMessage;
            tmpProcess.StartInfo.FileName = @AppPath;
            #region PRIMEPOS-2761
            //long TransNo = 0;
            using (var db = new Possql())
            {
                CCTransmission_Log cclog = new CCTransmission_Log();
                cclog.TransDateTime = DateTime.Now;
                cclog.TransAmount = amount;
                cclog.TicketNo = sTicketNumber;
                cclog.TransDataStr = reqMessage;
                cclog.PaymentProcessor = "XLINK";
                cclog.StationID = sStationID;
                cclog.UserID = sUserID;
                cclog.TransmissionStatus = "InProgress";
                cclog.TransType = sTransType;
                db.CCTransmission_Logs.Add(cclog);
                db.SaveChanges();
                db.Entry(cclog).GetDatabaseValues();
                TransNo = cclog.TransNo;
            }
            #endregion
            try
            {
                //Added By Dharmendra (SRT) on Dec-17-08 to Delete the Preveious result file
                if (File.Exists(strResultFilePath) == true)
                {
                    File.Delete(strResultFilePath);
                }
                //End Added Dec-17-08
                bool isProcessOK = tmpProcess.Start(); //Modified By Dharmendra (SRT) on Dec-17-08 to find the status of the process execution
                tmpProcess.WaitForExit();
                ResponseMessage = "Waiting For Responce";
                //Modified & Added By Dharmendra (SRT) on Dec-17-08
                if (isProcessOK == true)
                {
                    isRequestProcessed = true; //if xcharge client is able to connect to xcharge server & is able to process the transaction
                }
                else
                {
                    isRequestProcessed = false; // xcharge client is unable to connect to xcharge server & is unable to process the transaction
                }
                //End Modified & Added Dec-17-08
            }
            catch (Exception e)
            {
                ResponseMessage += "Xlink Can Not Be Contacted:" + e.Message.ToString();
                isRequestProcessed = false;
            }
            // This If Block Added By Dharmendra(SRT) on Dec-15-08 to check the value of the variable isRequestProcessed and process furthur
            if (isRequestProcessed == true)
            {
                try
                {
                    // Create an instance of StreamReader to read from a file.
                    // The using statement also closes the StreamReader.
                    //using (StreamReader sr = new StreamReader(Merchant.oMerchantInfo.Payment_ResultFile))
                    using (StreamReader sr = new StreamReader(strResultFilePath))
                    {
                        String line;
                        // Read and display lines from the file until the end of 
                        // the file is reached.
                        ResponseMessage = "<?xml version=" + Convert.ToChar(34).ToString() + "1.0" + Convert.ToChar(34).ToString() + " encoding=" + Convert.ToChar(34).ToString() + "utf-16" + Convert.ToChar(34).ToString() + " standalone=" + Convert.ToChar(34).ToString() + "no" + Convert.ToChar(34).ToString() + "?><RESULTDATA>";
                        while ((line = sr.ReadLine()) != null)
                        {
                            ResponseMessage += line.Trim();
                        }
                        ResponseMessage += "</RESULTDATA>";
                    }
                }
                catch (Exception e)
                {
                    // Let the user know what went wrong.
                    ResponseMessage += "The file could not be read:" + e.Message.ToString();
                }
                //End Of Added By SRT(Gaurav)
            } // If block till here Dec-15-08
            logger.Debug("Request Message : " + reqMessage);
            logger.Debug("Response Message : 0" + ResponseMessage);
            #region PRIMEPOS-2761 - Commented
            /*
            try
            {
                using (var db = new Possql())
                {
                    CCTransmission_Log cclog = new CCTransmission_Log();
                    cclog.TransDateTime = DateTime.Now;
                    cclog.TransAmount = amount;
                    cclog.TransDataStr = reqMessage;
                    cclog.RecDataStr = ResponseMessage;
                    db.CCTransmission_Logs.Add(cclog);
                    db.SaveChanges();
                }
            }
            catch (Exception exp)
            {
                logger.Trace(exp.ToString());

            }
            */
            #endregion
            //End Added
            return true;

        }

        #region PRIMEPOS-2717 24-Jul-2019 JY Added
        private bool IsDirectoryExistAndWritable(string dirPath)
        {
            bool bStatus = false;
            try
            {
                if (Directory.Exists(dirPath))
                {
                    using (FileStream fs = File.Create(Path.Combine(dirPath, Path.GetRandomFileName()), 1, FileOptions.DeleteOnClose))
                    {
                        bStatus = true;
                    }
                }
            }
            catch (Exception ex)
            {
                bStatus = false;
            }
            return bStatus;
        }
        #endregion

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
            const String XLINK = "XLINK";
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

            sWebTerminalID = string.Empty;
            IsCloudService = false;
            foreach (KeyValuePair<String, String> kvp in keys)
            {
                isValid = this.GetProcessorTag(kvp.Key, XLINK, out orgParam);
                if (isValid == true)
                {
                    orignalKeys.Add(orgParam, kvp.Value);
                }

                if (kvp.Key.Equals("TERMINALID"))
                {
                    sWebTerminalID = kvp.Value;
                    if (!string.IsNullOrWhiteSpace(sWebTerminalID))
                        IsCloudService = true;
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

            xmlList = xmlXLINK.GetElementsByTagName(xmlCommonNode);
            if (xmlList == null || xmlList.Count == 0)
            {
                ProcessorTag = tagValue;
                return false;
            }
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

        private string CloudBuildRequest()
        {
            string tranType = "CREDITSALE";
            string xmlInput = string.Empty;
            string webKey = string.Empty;
            string webID = string.Empty;
            string sXCTranID = string.Empty;
            string sAddressVal = string.Empty;
            string sZipVal = string.Empty;
            string sAllowDupVal = string.Empty;
            string sClerkVal = string.Empty;
            string sReceiptVal = string.Empty;
            string sTotalAmount = string.Empty;
            string sIIASTran = string.Empty;
            string sIIASAuthAmt = string.Empty;
            string sIIASRxAmt = string.Empty;
            string sAlias = string.Empty;
            
            //PRIMEPOS-2738
            string sOrderID = string.Empty;
            string sReFOrderID = string.Empty;
            foreach (KeyValuePair<String, String> kvp in MessageFields)
            {
                if (kvp.Key.Trim().Equals("TRANSACTIONTYPE"))
                {
                    string typeValue = kvp.Value.Trim().ToUpper();
                    switch (typeValue)
                    {
                        case "PURCHASE":
                            tranType = "CREDITSALE";
                            break;
                        case "RETURN":
                            tranType = "CREDITRETURN";
                            break;
                        case "DEBITPURCHASE":
                            tranType = "DEBITSALE";
                            break;
                        case "DEBITRETURN":
                            tranType = "DEBITRETURN";
                            break;
                        case "VOID":
                            tranType = "CREDITVOID";
                            break;
                        case "EBTSALE":
                            tranType = "EBTSALETRANSACTION";
                            break;
                        case "EBTRETURN":
                            tranType = "EBTRETURNTRANSACTION";
                            break;
                        default:
                            tranType = "WorkingOnIt";
                            break;
                    }
                }
                if (kvp.Key.Trim().Equals("AMOUNT"))
                {
                    try
                    {
                        amount = Convert.ToDecimal(kvp.Value);
                    }
                    catch (Exception ex)
                    {
                        logger.Trace(ex.ToString());
                    }
                }
                if (kvp.Key.Trim().Equals("PASSWORD"))
                {
                    webKey = kvp.Value;
                }
                if (kvp.Key.Trim().Equals("USERID"))
                {
                    webID = kvp.Value;
                }

                if (kvp.Key.Trim().Equals("ADDRESS"))
                {
                    sAddressVal = kvp.Value.Trim();
                }
                if (kvp.Key.Trim().Equals("ZIP"))
                {
                    sZipVal = kvp.Value.Trim();
                }
                if (kvp.Key.Trim().Equals("ALLOWDUP"))
                {
                    sAllowDupVal = kvp.Value.Trim();
                }
                if (kvp.Key.Trim().Equals("CLERK"))
                {
                    sClerkVal = kvp.Value.Trim();
                    sUserID = kvp.Value.Trim(); // PRIMEPOS-2761
                }
                if (kvp.Key.Trim().Equals("RECEIPT"))
                {
                    sReceiptVal = kvp.Value.Trim();
                    sTicketNumber = kvp.Value.Trim();// PRIMEPOS-2761
                }
                if (kvp.Key.Trim().Equals("XCTRANSACTIONID"))
                {
                    sXCTranID = kvp.Value.Trim();
                }
                if (kvp.Key.Trim().Equals("TOTALAMOUNT"))
                {
                    try
                    {
                        decimal totlAmount = Convert.ToDecimal(kvp.Value);
                        sTotalAmount = totlAmount.ToString();
                    }
                    catch (Exception ex)
                    {
                        logger.Trace(ex.ToString());
                    }
                }
                if (kvp.Key.Trim().Equals("IIASTRANSACTION"))
                {
                    sIIASTran = kvp.Value.Trim();
                }
                if (kvp.Key.Trim().Equals("IIASAUTHORIZEDAMOUNT"))
                {
                    try
                    {
                        decimal IIASAuthAmt = Convert.ToDecimal(kvp.Value);
                        sIIASAuthAmt = IIASAuthAmt.ToString();
                    }
                    catch (Exception ex)
                    {
                        logger.Trace(ex.ToString());
                    }
                }
                if (kvp.Key.Trim().Equals("IIASRXAMOUNT"))
                {
                    try
                    {
                        decimal IIASRxAmt = Convert.ToDecimal(kvp.Value);
                        sIIASRxAmt = IIASRxAmt.ToString();
                    }
                    catch (Exception ex)
                    {
                        logger.Trace(ex.ToString());
                    }
                }
                if (kvp.Key.Trim().Equals("XCACCOUNTID"))
                {
                    string profileID = kvp.Value.Trim();
                    if (profileID.Length == 13 && profileID.IndexOf("XAW") == 0)
                    {
                        sAlias = profileID.Substring(3);
                    }
                    else
                        sAlias = profileID;
                }
                #region PRIMEPOS-2761
                if (kvp.Key.Trim().Equals("StationID"))
                {
                    try
                    {
                        sStationID = kvp.Value.ToString();
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
                        sTransType = kvp.Value.ToString();
                    }
                    catch (Exception ex)
                    {
                        logger.Trace(ex.ToString());
                    }

                }
                if (kvp.Key.Trim().Equals("XCTRANSACTIONID"))
                {
                    try
                    {
                        sXCTRANSACTIONID = kvp.Value.Trim();
                    }
                    catch (Exception ex)
                    {
                        logger.Trace(ex.ToString());
                    }
                }
                #endregion
                #region  PRIMEPOS-2738 ADDED BY ARVIND
                if (kvp.Key.Trim().Equals("ORDERID"))
                {
                    sOrderID = kvp.Value.Trim();
                }
                if (kvp.Key.Trim().Equals("REFERENCEORDERID"))
                {
                    sReFOrderID = kvp.Value.Trim();
                }
                #endregion
            }

            xmlInput += "<XLINKEMVREQUEST>";

            xmlInput += "  <TRANSACTIONTYPE>" + tranType + "</TRANSACTIONTYPE>";
            xmlInput += "  <XWEBAUTHKEY>" + webKey + "</XWEBAUTHKEY>";
            xmlInput += "  <XWEBTERMINALID>" + sWebTerminalID + "</XWEBTERMINALID>";
            xmlInput += "  <XWEBID>" + webID + "</XWEBID>";
            if (!string.IsNullOrWhiteSpace(sAlias))
                xmlInput += "  <ALIAS>" + sAlias + "</ALIAS>";
            if (tranType.Equals("CREDITVOID"))
            {
                xmlInput += "  <TRANSACTIONID>" + sXCTranID + "</TRANSACTIONID>";
                xmlInput += "  <RECEIPTCOPYLABEL>MERCHANT_COPY</RECEIPTCOPYLABEL>";
            }
            else
                xmlInput += "  <AMOUNT>" + amount.ToString() + "</AMOUNT>";
            if (!string.IsNullOrWhiteSpace(sAllowDupVal))
                xmlInput += "  <ALLOWDUPLICATES>" + sAllowDupVal + "</ALLOWDUPLICATES>";
            if (!string.IsNullOrWhiteSpace(sClerkVal))
                xmlInput += "  <CLERK>" + sClerkVal + "</CLERK>";

            // for FSA/IIAS card
            if (!string.IsNullOrWhiteSpace(sIIASTran))
            {
                xmlInput += "  <IIASTRANSACTION>" + sIIASTran + "</IIASTRANSACTION>";
                xmlInput += "  <PARTIALAPPROVALSUPPORT>True</PARTIALAPPROVALSUPPORT>";
            }
            if (!string.IsNullOrWhiteSpace(sIIASAuthAmt))
                xmlInput += "  <IIASAUTHORIZEDAMOUNT>" + sIIASAuthAmt + "</IIASAUTHORIZEDAMOUNT>";
            if (!string.IsNullOrWhiteSpace(sIIASRxAmt))
                xmlInput += "  <IIASRXAMOUNT>" + sIIASRxAmt + "</IIASRXAMOUNT>";

            if (!string.IsNullOrWhiteSpace(sReceiptVal))
                xmlInput += "  <INVOICENO>" + sReceiptVal + "</INVOICENO>";
            if (!string.IsNullOrWhiteSpace(sAddressVal))
                xmlInput += "  <ADDRESS>" + sAddressVal + "</ADDRESS>";
            if (!string.IsNullOrWhiteSpace(sZipVal))
                xmlInput += "  <ZIP>" + sZipVal + "</ZIP>";
            if (!string.IsNullOrWhiteSpace(sTotalAmount))
                xmlInput += "  <TOTALAMOUNT>" + sTotalAmount + "</TOTALAMOUNT>";
            #region PRIMEPOS-2738
            if (!string.IsNullOrWhiteSpace(sOrderID))
                xmlInput += "  <ORDERID>" + sOrderID + "</ORDERID>";
            if (!string.IsNullOrWhiteSpace(sReFOrderID))
                xmlInput += "  <REFERENCEORDERID>" + sReFOrderID + "</REFERENCEORDERID>";
            #endregion

            xmlInput += "  </XLINKEMVREQUEST>";
            return xmlInput;
        }

        private bool CloudSendReceiveData()
        {
            string reqMessage = CloudBuildRequest();
            bool isRequestProcessed = true;
            string output = string.Empty;

            try
            {
                #region PRIMEPOS-2761
                //long TransNo = 0;
                using (var db = new Possql())
                {
                    CCTransmission_Log cclog = new CCTransmission_Log();
                    cclog.TransDateTime = DateTime.Now;
                    cclog.TransAmount = amount;
                    cclog.TicketNo = sTicketNumber;
                    cclog.TransDataStr = reqMessage;
                    cclog.PaymentProcessor = "XLINK";
                    cclog.StationID = sStationID;
                    cclog.UserID = sUserID;
                    cclog.TransmissionStatus = "InProgress";
                    cclog.TransType = sTransType;
                    db.CCTransmission_Logs.Add(cclog);
                    db.SaveChanges();
                    db.Entry(cclog).GetDatabaseValues();
                    TransNo = cclog.TransNo;
                }
                #endregion

                string rcmUrl = RCMURL + RCMMETHOD + QUERYSTRING + reqMessage;
                Uri rcmWebAddress = new Uri(rcmUrl);
                HttpWebRequest webRequest = WebRequest.Create(rcmWebAddress) as HttpWebRequest;
                webRequest.Method = "GET";
                webRequest.ContentType = "application/xml";
                HttpWebResponse response = (HttpWebResponse)webRequest.GetResponse();

                Stream dataStream = response.GetResponseStream();
                StreamReader responseReader = new StreamReader(dataStream, Encoding.UTF8);
                output = responseReader.ReadToEnd();
                responseReader.Close();
                dataStream.Close();

                ResponseMessage = "Waiting For Response...";
            }
            catch (Exception e)
            {
                ResponseMessage += " RCM cannot be contacted:" + e.Message.ToString();
                isRequestProcessed = false;
            }

            if (isRequestProcessed == true)
            {
                ResponseMessage = "<?xml version=" + Convert.ToChar(34).ToString() + "1.0" + Convert.ToChar(34).ToString() + " encoding=" + Convert.ToChar(34).ToString() + "utf-16" + Convert.ToChar(34).ToString() + " standalone=" + Convert.ToChar(34).ToString() + "no" + Convert.ToChar(34).ToString() + "?><RESULTDATA>";
                ResponseMessage += CloudReFormatOutput(output);
                ResponseMessage += "</RESULTDATA>";
            }

            logger.Debug("Request Message : " + reqMessage);
            logger.Debug("Response Message : 0" + ResponseMessage);
            #region PRIMEPOS-2761 - Commented
            /*
            try
            {
                using (var db = new Possql())
                {
                    CCTransmission_Log cclog = new CCTransmission_Log();
                    cclog.TransDateTime = DateTime.Now;
                    cclog.TransAmount = amount;
                    cclog.TransDataStr = reqMessage;
                    cclog.RecDataStr = ResponseMessage;
                    db.CCTransmission_Logs.Add(cclog);
                    db.SaveChanges();
                }
            }
            catch (Exception exp)
            {
                logger.Trace(exp.ToString());

            }
            */
            #endregion
            return isRequestProcessed;
        }

        private string CloudReFormatOutput(string output)
        {
            string finalOutput = output;
            string subOutput = string.Empty;
            string xmlStartTag = "<" + CLOUD_RESP_FILTERNODE + ">";
            string xmlEndTag = @"</" + CLOUD_RESP_FILTERNODE + ">";

            int idx = output.IndexOf(xmlStartTag);

            if (idx >= 0)
                subOutput = output.Substring(idx);
            else
                return finalOutput;

            idx = subOutput.IndexOf(xmlEndTag);
            if (idx >= 0)
                finalOutput = subOutput.Remove(idx + xmlEndTag.Length);

            return finalOutput;
        }

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

            }



            // Unmanaged cleanup code here

            Disposed = true;

        }

        #endregion
    }
}
