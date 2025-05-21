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
//using Logger = AppLogger.AppLogger;
using PossqlData;
using NLog;

namespace MMS.PCCHARGE
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
        XmlDocument xmlxCharge = null;
        #endregion

        #region constants
        private const String MSG_HEADER = "<XML_REQUEST>";
        private const String MSG_FOOTER = "</XML_REQUEST>";
        private const String MSG_START = "<XML_FILE>";
        private const String MSG_END = "</XML_FILE>";
        private const String RESPONSE_FILTERNODE = "XML_REQUEST";
        private const String MERCH_NUM = "MERCH_NUM";
        private const String USER_ID = "USER_ID";
        private const String VALID_FIELDS = "ValidFields.xml";
        private const String MANDATORY_FIELDS = "MandatoryFields.xml";
        private const String XML_CONST = "TXN";
        private const String PROCESSOR_ID = "PROCESSOR_ID";
        public const String FAILED_OPRN = "FAILED";
        public const String MMS_CARD = "MMSCARD";
        public const String PCCHARGE = "PCCHARGE";
        public const String INVALID_PARAMETERS = "INVALID_PARAMETERS";
        private const string COMMOMPROCESSORTAG = "CommonProcessorTag.xml";
        private const string ERRORRSPONSE = "<XML_REQUEST><USER_ID>User1</USER_ID><TROUTD></TROUTD><RESULT>Error</RESULT><AUTH_CODE>Payment Server could not be contacted</AUTH_CODE><REFERENCE></REFERENCE><INTRN_SEQ_NUM></INTRN_SEQ_NUM><TOTALTRANSTIME></TOTALTRANSTIME></XML_REQUEST>";
        private const string FSA = "FSA";
        private const string FSA_PARTIAL_AUTH = "FSA_PARTIAL_AUTH";
        //Added By SRT(Gaurav) Date: 01-Dec-2008        
        private const string AMOUNT_PRESCRIPTION = "AMOUNT_PRESCRIPTION";
        private const string AMOUNT_VISION = "AMOUNT_VISION";
        private const string AMOUNT_CLINIC = "AMOUNT_CLINIC";
        private const string AMOUNT_DENTAL = "AMOUNT_DENTAL";
        //End Of Added By SRT(Gaurav)
        //Added By SRT(Gaurav) Date: 04-Dec-2008
        private const string CLOSEXMLREQUESTTAG = "</XML_REQUEST>";
        private const string OPENXMLREQUESTTAG = "<XML_REQUEST>";//Added By Dharmendra (SRT)
        //Added By Dharmendra (SRT) on Dec-15-08
        private const string OPENXMLTRANSAMOUNT = "<TRANS_AMOUNT>"; // Modified By Dharmendra on Dec-15-08 replaced for tag name TransAmount
        private const string CLOSEXMLTRANSAMOUNT = "</TRANS_AMOUNT>"; // Modified By Dharmendra on Dec-15-08 replaced for tag name TransAmount
        private const string PAYMENTPROCESSOR = "PCCHARGE|"; // Added By Dharmendra (SRT)
        private const string INVALIDPAYMENTPROCESSOR = "INVALID PAYMENT PROCESSOR SELECTION"; 
        private const string REQUESTAMOUNTFIELD = "TRANS_AMOUNT";
        //Added By Dharmendra (SRT) on Dec-16-08 this tag will added along with the response message
        //when we receives invalid processor name in the response
        private const string OPENINVPROCESSOR = "<INV_PROCESSOR>";
        private const string CLOSEINVPROCESSOR = "</INV_PROCESSOR>";
        private const string INV_PROCESSOR = "INV_PROCESSOR";        
        //Added Til Here Dec-16-08
        //End Added
        
        //Added & Modified Till Here Ded-15-08
        //End Of Added By SRT(Gaurav)
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
            logger.Trace("In Processor CTOR");
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
            response = new PCChargePaymentResponse();//Modified By SRT(Gaurav) Date: 21-NOV-2008

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
            logger.Trace("ClearFields()");

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
            logger.Trace("In BuildRequest()");
            String reqMessage = string.Empty;
            String processor = String.Empty;
            String fsaStat = String.Empty;
            // Parse the keys and build the XML message
            MessageFields.TryGetValue(PROCESSOR_ID, out processor);
            bool isFSA = MessageFields.TryGetValue(FSA, out fsaStat);
            processor = PadSpaces(processor, 4);
            MessageFields.Add(PROCESSOR_ID, processor);
            //Added By SRT(Gaurav) Date: 01-Dec-2008
            //Mantis ID: 0000136
            if (isFSA == true)
            {
                //Added By SRT(Gaurav) Date: 03-Dec-2008
                //Mantis ID: 0000136
                MessageFields.Add(FSA_PARTIAL_AUTH, "1");
                //MessageFields.Add(AMOUNT_PRESCRIPTION, "0.00");
                //MessageFields.Add(AMOUNT_VISION, "0.00");
                //MessageFields.Add(AMOUNT_CLINIC, "0.00");
                //MessageFields.Add(AMOUNT_DENTAL, "0.00");

            }
            //End Of Added By SRT(Gaurav)
            reqMessage = Fields.BuildXML(ref MessageFields, MSG_HEADER, MSG_FOOTER);

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
        protected PaymentResponse ProcessTxn(int transactionType, ref MMSDictionary<String, String> requestMsgKeys)
        {
            logger.Trace("Start Processor (PCCHARGE ProcessTxn) ----");
            //Clear all the fields.                 
            ClearFields();
            MessageFields = requestMsgKeys;
            txnType = transactionType.ToString();
            String value = String.Empty;
            String merchantNumber = String.Empty;
            String processorId = String.Empty;

            //Check for existance of Card Type
            //Commented by Ritesh on 6-Dec-08 to ensure there is no dependency on card type.
            /*
            if(!MessageFields.TryGetValue(MMS_CARD, out value))
            {
                return null;
            }
             */
            //Get Merchant Info and ProcessorId
            /*if (!Merchant.GetMerchantInfo(value, out merchantNumber, out processorId))
            {
                return null;
            }*/
            if (Merchant.oMerchantInfo == null)
            {
                return null;
            }
            //Add UserId and MerchantNumber & Processor Id.
            MessageFields.Add(MERCH_NUM, Merchant.oMerchantInfo.Merchant);
            MessageFields.Add(PROCESSOR_ID, Merchant.oMerchantInfo.Processor_ID);
            MessageFields.Add(USER_ID, Merchant.oMerchantInfo.User_ID);           
            //Remove MMS Added Field            
            MessageFields.Remove(MMS_CARD);            

            /*
             * 1) Lookup for the TxnType in the MandatoryKeys (MMSDictionary/Hashtable).
             * 2) If not found look up and create a list of mandatory fields for that transaction and store it in the Collection.
             */
            if (IsValidRequest())
            {
                ResponseMessage = String.Empty;
                if (SendReceiveData())
                {
                    if (ResponseMessage == INVALIDPAYMENTPROCESSOR)
                    {
                        //Modified By Dharmendra (SRT) on Dec-16-08 included the response message in invalid processor tag
                        ResponseMessage = MSG_START + OPENXMLREQUESTTAG + OPENINVPROCESSOR + ResponseMessage + CLOSEINVPROCESSOR + "<RESULT>INV_PROCESSOR</RESULT>" + CLOSEXMLREQUESTTAG + MSG_END;
                        logger.Error("***PCCHARGE, Invalid Payment Processor");
                        //Modified By Dharmendra (SRT) on Dec-16-08 included the response message in invalid processor tag
                    }
                    else
                    {
                        //Added By SRT(Gaurav) Date: 04-Dec-2008
                        //Mantis Id : 04-Dec-2008
                        //Details: Code Added Fetching Trans Amount, FSA Amount From Request Tag and
                        //Adding It In Reponse Tags.
                        string tempValue = string.Empty;
                        ResponseMessage = ResponseMessage.Replace(CLOSEXMLREQUESTTAG, "");

                        if (requestMsgKeys.TryGetValue(REQUESTAMOUNTFIELD, out tempValue))
                        {
                            ResponseMessage += OPENXMLTRANSAMOUNT + tempValue + CLOSEXMLTRANSAMOUNT;
                        }

                        ResponseMessage += CLOSEXMLREQUESTTAG;
                        ResponseMessage = MSG_START + ResponseMessage + MSG_END;
                    }
                }
                else
                {
                    ResponseMessage = MSG_START + ERRORRSPONSE + MSG_END;
                    logger.Error("***PCCHARGE, Error: " + ResponseMessage);
                }

                response.ParseResponse(ResponseMessage, RESPONSE_FILTERNODE);
                logger.Trace("END Processor (PCCHARGE ProcessTxn)");
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
            String reqMessage = PAYMENTPROCESSOR + BuildRequest(); //Added By Dharmendra (SRT) on Dec-15-08 to send the request payment processor name.
            logger.Trace("SendReceivedData() Start ");
            SocketClient PaymentConn = new SocketClient("", 0);
            if (!PaymentConn.Connect(Merchant.oMerchantInfo.Payment_Server,Convert.ToInt32(Merchant.oMerchantInfo.Port_No)))
            {
                errorMessage += "CONNECTION FAILED:-" + PaymentConn.Error;
                PaymentConn.Dispose();
                PaymentConn = null;
                logger.Error("*** PCCHARGE SendReceivedData() Error CONNECTION FAILED: " + errorMessage);
                return false;
            }
            logger.Trace("PCCHARGE, Data string to send to PCCHARGE"); 
            if (PaymentConn.Send(reqMessage) == 0)
            {
                errorMessage += "SEND FAILED:-" + PaymentConn.Error;
                PaymentConn.Disconnect();
                PaymentConn.Dispose();
                PaymentConn = null;
                logger.Error("*** PCCHARGE SendReceivedData() SEND FAILED: "+ errorMessage);
                return false;
            }
            logger.Trace("PCCHARGE, Data string to Received from PCCHARGE"); 
            if (PaymentConn.Receive(ref ResponseMessage) == 0)
            {
                errorMessage += "RECEIVE FAILED:-" + PaymentConn.Error;
                PaymentConn.Disconnect();
                PaymentConn.Dispose();
                PaymentConn = null;
                logger.Error("*** PCCHARGE SendReceivedData() RECEIVE FAILED: " + errorMessage);
                return false;
            };
            if (!PaymentConn.Disconnect())
            {
                errorMessage += "DISCONNECT FAILED:-" + PaymentConn.Error;
                PaymentConn.Dispose();
                PaymentConn = null;
                logger.Error("*** PCCHARGE SendReceivedData() DISCONNECT FAILED: " + errorMessage);
                return false;
            };
            PaymentConn = null;
            logger.Trace("SendReceivedData() END, return: true");
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
            logger.Trace("PCCHARGE IsValidFields()-- Start to check for valid fields");
            //compare all the fields agains list of valid fields "ValidKeys" if fine 
            foreach (KeyValuePair<String, String> kvp in MessageFields)
            {
                if (!ValidKeys.ContainsKey(kvp.Key))
                {
                    errorMessage += "INVALID FIELD:-" + kvp.Key;
                    logger.Error("*** PCCHARGE IsValidFields()-- Invalid Field: " + errorMessage.ToString());
                    return false;
                }
            }
            logger.Trace("PCCHARGE IsValidFields()-- End checking for valid fields, return: true");
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
            logger.Trace("PCCHARGE IsMandatoryFields()-- Start to check for Mandatory Fields");
            //compare all the fields agains list of mandatory fields "MandatoryKeys" if fine 
            MMSDictionary<String, String> keys = null;
            if (!MandatoryKeys.ContainsKey(txnType))
            {
                if (FetchMandatoryFields(txnType) == 0)
                {
                    errorMessage += "INVALID TRANSACTION TYPE:-" + txnType;
                    logger.Error("*** PCCHARGE IsMandatoryFields()-- Invalid: " + errorMessage.ToString());
                    return false;
                }
            }
            MandatoryKeys.TryGetValue(txnType, out keys);
            foreach (KeyValuePair<String, String> kvp in keys)
            {
                if (!MessageFields.ContainsKey(kvp.Key))
                {
                    errorMessage += "MISSING MANDATORY FIELD:-" + kvp.Key;
                    logger.Error("*** PCCHARGE IsMandatoryFields()-- Invalid: " + errorMessage.ToString());
                    return false;
                }
            }
            logger.Trace("PCCHARGE IsMandatoryFields()-- End checking Mandatory Fields, return: true");
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
            String strXmlKey = XML_CONST + transactionType.Trim();
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
            MMSDictionary<String, String> orignalKeys = new MMSDictionary<string, string>();
            MMSDictionary<String, String> revisedKeys = new MMSDictionary<string, string>();
            string orgParam = string.Empty;
            if (keys == null)
                return false;
            foreach (KeyValuePair<String, String> kvp in keys)
            {
                //Modified By SRT(Gaurav) Date : 25-NOV-2008
                //Mantis ID: 0000136
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
                isValid = this.GetProcessorTag(kvp.Key, PCCHARGE, out orgParam);
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
