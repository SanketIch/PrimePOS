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
using MMS.PROCESSOR;
using System.Xml;
using System.Diagnostics;
using Logger = AppLogger.AppLogger;
using PossqlData;

namespace MMS.XCHARGE
{
    //Author : Ritesh 
    //CopyRight: MMS 2008
    //Functionality Desciption : The purpose of this class is to make base processor for transactions.
    //External functions:None   
    //Known Bugs : None
    //Start Date : 2 Feb 2008.
    public abstract class Processor : IDisposable
    {
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
        XmlDocument xmlxCharge = null;

        private decimal amount = 0.00m;

        #endregion

        #region constants
        private const String MSG_HEADER = "<TRANSACTION>";
        private const String MSG_FOOTER = "</TRANSACTION>";
        private const String XCTRANSACTION = "<XCTRANSACTION>";
        private const String ENDXCTRANSACTION = "</XCTRANSACTION>";
        private const String RESPONSE_FILTERNODE = "TRANSACTION";
        private const String VALID_FIELDS = "ValidFields.xml";
        private const String MANDATORY_FIELDS = "MandatoryFields.xml";
        private const String EOT = "\x04"; //EOT required for each transaction in XCharge
        public const String FAILED_OPRN = "FAILED";
        public const String INVALID_PARAMETERS = "INVALID_PARAMETERS";
        private const string COMMOMPROCESSORTAG = "CommonProcessorTag.xml";
        //Modified By Dharmendra on Jan-28-09 make correction in xmltags
        private const string ERRORRESPONSE = "<XCTRANSACTION><TRANSACTION><RESULT_SUCCESS>F</RESULT_SUCCESS><RESULT_APPROVALCODE></RESULT_APPROVALCODE><RESULT_ITEMNO></RESULT_ITEMNO><RESULT_AUTHCODE></RESULT_AUTHCODE><RESULT_BATCHNO></RESULT_BATCHNO><RESULT_AVSCODE></RESULT_AVSCODE><RESULT_TRANSID></RESULT_TRANSID><RESULT_TICKET></RESULT_TICKET><RESULT_DESCRIPTION>PAYMENT SERVER COULD NOT BE CONNECTED</RESULT_DESCRIPTION><TROUTD></TROUTD></TRANSACTION></XCTRANSACTION>";
        //Modified Till Here Jan-28-09
        //Added By Dharmendra (SRT) on Dec-09-08 for the requirement of Approed Amount with old x-charge servers        
        private const string OPENXMLTRANSAMOUNT = "<AMOUNT>";
        private const string CLOSEXMLTRANSAMOUNT = "</AMOUNT>";
        private const string MSG_START = "<XML_FILE>";
        private const string MSG_END = "</XML_FILE>";
        //Added By Dharmendra (SRT) on Dec-15-08
        private const string PAYMENTPROCESSOR = "XCHARGE|"; // Added By Dharmendra (SRT)
        private const string INVALIDPAYMENTPROCESSOR = "INVALID PAYMENT PROCESSOR SELECTION";
        private const string REQUESTAMOUNTFIELD = "AMOUNT";
        //End Added Dec-15-08
        //Added By Dharmendra (SRT) on Dec-16-08 this tag will added along with the response message
        //when we receives invalid processor name in the response
        private const string OPENINVPROCESSOR = "<INV_PROCESSOR>";
        private const string CLOSEINVPROCESSOR = "</INV_PROCESSOR>";
        private const string INV_PROCESSOR = "INV_PROCESSOR";
        //Added Til Here Dec-16-08
        //End Added
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
            response = new XChargePaymentResponse(); //Modified By SRT(Gaurav) Date: 21-NOV-2008
            xmlxCharge = new XmlDocument();
            xmlxCharge.Load(COMMOMPROCESSORTAG);
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
        private String BuildRequest()
        {
            String reqMessage = "\\";

            int count = 0;
            foreach (KeyValuePair<String, String> kvp in MessageFields)
            {
                reqMessage += kvp.Key.Trim() + "\\";
                count++;

                if (kvp.Key.Trim().Equals("AMOUNT"))
                {
                    try
                    {
                        amount = Convert.ToDecimal(kvp.Value.ToString());
                    }
                    catch (Exception ex)
                    {
                        Logger.LogWritter(ex.ToString());
                    }

                }
            }
            /*
            String reqMessage = string.Empty;*/

            // Parse the keys and build the XML message
            reqMessage = Fields.BuildXML(ref MessageFields, MSG_HEADER, MSG_FOOTER);
            reqMessage = XCTRANSACTION + reqMessage + ENDXCTRANSACTION + EOT;
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
            Logger.LogWritter("Start Processor (XCHARGE ProcessTxn) ----");
            //Clear all the fields.                 
            ClearFields();
            MessageFields = requestMsgKeys;
            txnType = transactionType;
            String value = String.Empty;
            string requestAmount = string.Empty; // Added By Dharmendra (SRT) on Dec-09-08
            string updatedResponse = string.Empty;

            if (IsValidRequest())
            {
                ResponseMessage = String.Empty;
                if (SendReceiveData())
                {
                    //Modified By Dharmendra (SRT) on  Dec-09-08 in order to send transaction amount as approved amount to old xcharge based POS application
                    if (ResponseMessage.Contains(EOT))
                    {
                        updatedResponse = ResponseMessage.Replace(EOT, "");
                    }
                    else
                    {
                        updatedResponse = ResponseMessage;
                    }
                    //Added By Dharmendra (SRT) on Dec-09-08 Adding requested <AMOUNT> tag & its value in the response tag
                    updatedResponse = updatedResponse.Replace(ENDXCTRANSACTION, ""); // detaching the outer footer
                    updatedResponse = updatedResponse.Replace(MSG_FOOTER, ""); //detaching the inner footer
                    if (updatedResponse == INVALIDPAYMENTPROCESSOR)
                    {
                        //Modified By Dharmendra (SRT) on Dec-16-08 included the response message in invalid processor tag
                        updatedResponse = XCTRANSACTION + MSG_HEADER + OPENINVPROCESSOR + updatedResponse + CLOSEINVPROCESSOR + "<RESULT_SUCCESS>INV_PROCESSOR</RESULT_SUCCESS>";
                        Logger.LogWritter("***XCHARGE Invalid Payment Processor");
                        //Modified Till Here Dec-16-08
                    }
                    if (requestMsgKeys.TryGetValue(REQUESTAMOUNTFIELD, out requestAmount)) // fetching requested amount from request parameter
                    {
                        updatedResponse += OPENXMLTRANSAMOUNT + requestAmount + CLOSEXMLTRANSAMOUNT; //creating a new tag with requested amount
                    }

                    updatedResponse += MSG_FOOTER; // reattaching the inner footer
                    updatedResponse += ENDXCTRANSACTION; //reattaching the outer footer
                    Logger.LogWritter("XCHARGE, Data string to send to PCCHARGE"); 
                    //End Added
                    //End Modified By Dharmendra

                }
                else
                {
                    //default response message 
                    updatedResponse = ERRORRESPONSE;
                    Logger.LogWritter("***XCHARGE, Error: " + ERRORRESPONSE);
                }

                response.ParseResponse(updatedResponse, RESPONSE_FILTERNODE);
                Logger.LogWritter("END Processor (PCCHARGE ProcessTxn)");
                //Modified By SRT(Gaurav) Date: 21-NOV-2008
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
        /// <param name="strRequest">String</param>
        /// <returns>bool</returns> 
        private bool SendReceiveData()
        {
            Logger.LogWritter("SendReceivedData() Start ");
            String reqMessage = PAYMENTPROCESSOR + BuildRequest();          //Added By Dharmendra (SRT) on Dec-10-08 to send the request payment processor port no.
            //String reqMessage ="<?xml version="1.0" encoding="utf-16" standalone="no">
            SocketClient PaymentConn = new SocketClient("", 0);

            if (!PaymentConn.Connect(Merchant.oMerchantInfo.Payment_Server, Convert.ToInt32(Merchant.oMerchantInfo.Port_No) ))
            {
                errorMessage += "CONNECTION FAILED:-" + PaymentConn.Error;
                PaymentConn.Dispose();
                PaymentConn = null;
                Logger.LogWritter("*** XCHARGE SendReceivedData() Error CONNECTION FAILED: " + errorMessage);
                return false;
            }

            if (PaymentConn.Send(reqMessage) == 0)
            {
                errorMessage += "SEND FAILED:-" + PaymentConn.Error;
                PaymentConn.Disconnect();
                PaymentConn.Dispose();
                PaymentConn = null;
                Logger.LogWritter("*** XCHARGE SendReceivedData() SEND FAILED: " + errorMessage);
                return false;
            }

            if (PaymentConn.Receive(ref ResponseMessage) == 0)
            {
                errorMessage += "RECEIVE FAILED:-" + PaymentConn.Error;
                PaymentConn.Disconnect();
                PaymentConn.Dispose();
                PaymentConn = null;
                Logger.LogWritter("*** XCHARGE SendReceivedData() RECEIVE FAILED: " + errorMessage);
                return false;
            };

            if (!PaymentConn.Disconnect())
            {
                errorMessage += "DISCONNECT FAILED:-" + PaymentConn.Error;
                PaymentConn.Dispose();
                PaymentConn = null;
                Logger.LogWritter("*** XCHARGE SendReceivedData() DISCONNECT FAILED: " + errorMessage);
                return false;
            };

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
                Logger.LogWritter(exp.ToString());

            }

            PaymentConn = null;
            Logger.LogWritter("SendReceivedData() END, return: true");
            return true;

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
            Logger.LogWritter("XCHARGE IsValidFields()-- Start to check for valid fields");
            //compare all the fields agains list of valid fields "ValidKeys" if fine 
            foreach (KeyValuePair<String, String> kvp in MessageFields)
            {
                if (!ValidKeys.ContainsKey(kvp.Key))
                {
                    errorMessage += "INVALID FIELD:-" + kvp.Key;
                    Logger.LogWritter("*** XCHARGE IsValidFields()-- Invalid Field: " + errorMessage.ToString());
                    return false;
                }
            }
            Logger.LogWritter("XCHARGE IsValidFields()-- End checking for valid fields, return: true");
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
            Logger.LogWritter("XCHARGE IsMandatoryFields()-- Start to check for Mandatory Fields");
            MMSDictionary<String, String> keys = null;
            if (!MandatoryKeys.ContainsKey(txnType))
            {
                if (FetchMandatoryFields(txnType) == 0)
                {
                    errorMessage += "INVALID TRANSACTION TYPE:-" + txnType;
                    Logger.LogWritter("*** XCHARGE IsMandatoryFields()-- Invalid: " + errorMessage.ToString());
                    return false;
                }
            }
            MandatoryKeys.TryGetValue(txnType, out keys);
            foreach (KeyValuePair<String, String> kvp in keys)
            {
                if (!MessageFields.ContainsKey(kvp.Key))
                {
                    errorMessage += "MISSING MANDATORY FIELD:-" + kvp.Key;
                    Logger.LogWritter("*** XCHARGE IsMandatoryFields()-- Invalid: " + errorMessage.ToString());
                    return false;
                }
            }
            Logger.LogWritter("XCHARGE IsMandatoryFields()-- End checking Mandatory Fields, return: true");
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
            bool flag = true;
            bool isValid = true;
            string sError = string.Empty;
            const String XCHARGE = "XCHARGE";
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
                isValid = this.GetProcessorTag(kvp.Key, XCHARGE, out orgParam);
                if (isValid == true)
                {
                    orignalKeys.Add(orgParam, kvp.Value);
                }
            }
            keys = orignalKeys;
            //Commented by Ritesh unnecessary complication
            /*
            if (XChargeValidateParams.DefaultInstance.CheckXcardParams(keys, out sError) == false)
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

            xmlList = xmlxCharge.GetElementsByTagName(xmlCommonNode);
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
