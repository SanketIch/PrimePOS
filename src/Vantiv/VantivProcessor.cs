using Newtonsoft.Json;
using NLog;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Xml;
using Vantiv.RequestModels;
using Vantiv.ResponseModels;
using Vantiv.Responses;

namespace Vantiv
{
    //PRIMEPOS-2636
    public class VantivProcessor
    {
        ILogger logger = LogManager.GetCurrentClassLogger();
        #region properties
        public string LaneID
        {
            get; set;
        }
        public string Url
        {
            get; set;
        }
        public string StationID
        {
            get; set;
        }
        public string ApplicationName
        {
            get; set;
        }
        public Version Version
        {
            get; set;
        }
        public string PaymentAccountID
        {
            get; set;
        }
        public string AccountID
        {
            get; set;
        }
        public string AcceptorID
        {
            get; set;
        }
        public string AccountToken
        {
            get; set;
        }
        public string TerminalID
        {
            get; set;
        }//PRIMEPOS-2796
        #endregion

        private bool isTitleAdded = false;
        private bool isTokenized = false;
        private bool isNBSTransaction = false; //PRIMEPOS-3528 //PRIMEPOS-3504 
        string developerKey = string.Empty;
        string developerSecret = string.Empty;

        ScrollingDisplay displayAddItem = new ScrollingDisplay();
        public VantivProcessor(string url, string laneID, string applicationName, string stationID, string triPOSConfigPath, bool IsIdleScreen = true)//PRIMEPOS-2895 Arvind
        {
            try
            {
                logger.Trace("TRACE : Entered in the Vantiv Processor Constructor");
                Assembly assembly = Assembly.GetExecutingAssembly();
                Version version = assembly.GetName().Version;

                //Setting the Properties to use in all the Methods
                this.LaneID = laneID;
                this.Url = url;
                this.ApplicationName = applicationName;
                this.StationID = stationID;
                this.Version = version;

                string path = @"C:\Program Files (x86)\Vantiv\TriPOS Service\triPOS - Copy.config";

                XmlDocument triPOSConfig = new XmlDocument();
                triPOSConfig.Load(triPOSConfigPath + @"\triPOS.config");//PRIMEPOS-2895 Arvind
                //triPOSConfig.Load(@"C:\triPOS.config");//PRIMEPOS-2895 Arvind

                //XDocument document = XDocument.Load(path);                

                //XmlNodeList serialLane = triPOSConfig.GetElementsByTagName("serialLane");

                //var items = from item in document.Descendants("serialLane")
                //            where item.Attribute("laneId").Value == "2"
                //            select item;

                //foreach (XElement itemElement in items)
                //{
                //    itemElement.SetAttributeValue("description", "Project2_Update");

                //}


                //document.Save(path);
                XmlNodeList accountID = triPOSConfig.GetElementsByTagName("accountId");
                this.AccountID = accountID[0].InnerXml;

                XmlNodeList acceptorID = triPOSConfig.GetElementsByTagName("acceptorId");
                this.AcceptorID = acceptorID[0].InnerXml;

                XmlNodeList accountToken = triPOSConfig.GetElementsByTagName("accountToken");
                this.AccountToken = accountToken[0].InnerXml;

                var developerKeys = triPOSConfig.GetElementsByTagName("developerKey");
                this.developerKey = developerKeys[0].InnerXml;

                var developerSecrets = triPOSConfig.GetElementsByTagName("developerSecret");
                this.developerSecret = developerSecrets[0].InnerXml;

                #region PRIMEPOS-2769 
                var terminalIDs = triPOSConfig.GetElementsByTagName("terminalId");

                for (int i = 0; i < terminalIDs.Count; i++)
                {
                    if (terminalIDs[i].InnerText != "0000009999")
                    {
                        this.TerminalID = terminalIDs[i].InnerText;
                    }
                }
                #endregion

                if (IsIdleScreen)
                    IdleScreen(this.Url, this.LaneID, this.ApplicationName, this.StationID);
            }
            catch (Exception ex)
            {
                logger.Debug("Error in VantivProcessor : " + ex.ToString());
                throw ex;
            }
        }
        /// <summary>
        /// THIS METHOD IS USED TO SEND REQUEST FOR XML PART TO THE EXPRESS API
        /// </summary>
        /// <param name="destinationUrl"></param>
        /// <param name="requestXml"></param>
        /// <returns></returns>
        public string postXMLData(string destinationUrl, string requestXml)
        {
            //logger.Debug("The request is :" + requestXml);
            try
            {
                //ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;    //PRIMEPOS-3179 27-Jan-2023 JY Commented
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(destinationUrl);
                byte[] bytes;
                bytes = Encoding.ASCII.GetBytes(requestXml);
                request.ContentType = "text/xml";
                request.ContentLength = bytes.Length;
                request.Method = "POST";
                Stream requestStream = request.GetRequestStream();
                requestStream.Write(bytes, 0, bytes.Length);
                requestStream.Close();
                HttpWebResponse response;
                response = (HttpWebResponse)request.GetResponse();
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    Stream responseStream = response.GetResponseStream();
                    string responseStr = new StreamReader(responseStream).ReadToEnd();
                    logger.Debug("The response is :" + responseStr);
                    return responseStr;
                }
                return null;
            }
            catch (Exception ex)
            {
                logger.Fatal("Error in POSTXMLDATA() Method ", ex.ToString());
                throw ex;
            }
        }
        /// <summary>
        /// The Idle screen is used to reset the screen to Default screen
        /// </summary>
        /// <param name="url"></param>
        /// <param name="laneID"></param>
        /// <param name="applicationName"></param>
        /// <param name="stationID"></param>
        /// <returns></returns>
        public string IdleScreen(string url, string laneID, string applicationName, string stationID)
        {
            logger.Trace(" ENTERED IN IDLESCREEN METHOD ");

            string actualResponse = string.Empty;
            string postBody = string.Empty;
            this.Url += Constant.Idle;

            try
            {
                Idle idlescreen = new Idle();
                idlescreen.LaneID = laneID;

                postBody = JsonConvert.SerializeObject(idlescreen);

                actualResponse = Constant.SendRequestPost(postBody, this.Url, this.developerKey, this.developerSecret, this.ApplicationName);

                logger.Trace($"IdleScreen Device response : {actualResponse}");//PRIMEPOS-3054
                this.Url = this.Url.Remove(28);
                this.isTitleAdded = false;//PRIMEPOS-3126
                this.displayAddItem = new ScrollingDisplay();//PRIMEPOS-3126
            }
            catch (Exception ex)
            {
                logger.Error("ERROR IN IDLESCREEN METHOD : ", ex.ToString());
                this.Url = this.Url.Remove(28);
                throw ex;
            }
            return actualResponse;
        }
        /// <summary>
        /// This is the Sale Method for Credit and Debit Transaction
        /// </summary>
        /// <param name="url"></param>
        /// <param name="laneID"></param>
        /// <param name="applicationName"></param>
        /// <param name="stationID"></param>
        /// <param name="fields"></param>
        /// <returns></returns>
        public VantivResponse Sale(Dictionary<String, String> fields)
        {
            logger.Trace(" ENTERED IN SALE METHOD ");


            string actualResponse = string.Empty;
            string postBody = string.Empty;
            this.Url += Constant.Sale;

            //For initializing the Response class 
            VantivResponse deviceResponse = new VantivResponse();

            try
            {
                if (fields.ContainsKey("TOKENREQUEST"))
                {
                    isTokenized = Convert.ToBoolean(fields["TOKENREQUEST"]);
                }
                //Initializing the Classes
                SaleRequest saleRequest = new SaleRequest();
                saleRequest.Configuration = new Configuration();
                saleRequest.Address = new Address();
                saleRequest.healthcare = new Healthcare();

                //Setting the Properties
                saleRequest.TicketNumber = fields["TICKETNUMBER"];
                saleRequest.TransactionAmount = Convert.ToDecimal(fields["AMOUNT"]);
                saleRequest.LaneId = Convert.ToInt32(this.LaneID);
                saleRequest.Configuration.AllowPartialApprovals = true;
                saleRequest.ReferenceNumber = fields["TICKETNUMBER"];//PRIMEPOS-2795

                if (fields.ContainsKey("IIASRXAMOUNT"))
                {
                    saleRequest.healthcare.prescription = fields["IIASRXAMOUNT"];
                    saleRequest.healthcare.clinic = "0";
                    saleRequest.healthcare.dental = "0";
                    saleRequest.healthcare.total = fields["IIASRXAMOUNT"];
                    saleRequest.healthcare.vision = "0";
                }
                else if (fields.ContainsKey("IIASTRANSACTION") && fields["IIASTRANSACTION"].ToString().ToLower() == "true")
                {
                    saleRequest.healthcare.prescription = fields["IIASAUTHORIZEDAMOUNT"];
                    saleRequest.healthcare.clinic = "0";
                    saleRequest.healthcare.dental = "0";
                    saleRequest.healthcare.total = fields["IIASAUTHORIZEDAMOUNT"];
                    saleRequest.healthcare.vision = "0";
                }

                if (fields["TRANSACTIONTYPE"].ToString().ToUpper().Contains("EBT"))
                {
                    saleRequest.ebtType = "FoodStamp";
                    saleRequest.Configuration.allowDebit = false;
                }
                else
                {
                    saleRequest.Configuration.allowDebit = true;
                }

                //if (Convert.ToBoolean(fields["ALLOWDUP"]) == true)
                //{
                //    saleRequest.Configuration.CheckForDuplicateTransactions = true;
                //}
                //else
                //{
                //    saleRequest.Configuration.CheckForDuplicateTransactions = false;
                //}
                //saleRequest.Configuration.CheckForDuplicateTransactions = true;
                //Serializing the Object
                postBody = JsonConvert.SerializeObject(saleRequest);

                //For Sending it in HttpClient
                actualResponse = Constant.SendRequestPost(postBody, this.Url, this.developerKey, this.developerSecret, this.ApplicationName);
                logger.Trace($"Sale Device response : {actualResponse}");//PRIMEPOS-3054
                deviceResponse.ParseSaleResponse(actualResponse);

                deviceResponse.deviceRequest = postBody;
                deviceResponse.deviceResponse = actualResponse;
                //PRIMEPOS-2793
                if (deviceResponse.EmvReceipt != null)
                {
                    deviceResponse.EmvReceipt.LaneID = this.LaneID;
                    deviceResponse.EmvReceipt.TerminalID = this.TerminalID;
                }
                //
                this.Url = this.Url.Remove(28);

                if (isTokenized && deviceResponse.ResultDescription == "SUCCESS")
                {
                    VantivResponse vantivResponse = new VantivResponse();
                    vantivResponse = PaymentAccountCreate(deviceResponse.TransactionNo, fields);
                    deviceResponse.ProfiledID = vantivResponse?.ProfiledID; //PRIMEPOS-3338
                    deviceResponse.Expiration = vantivResponse?.Expiration; //PRIMEPOS-3338
                }
            }
            catch (Exception ex)
            {
                logger.Error(" ERROR IN SALE METHOD : " + ex.ToString());
                this.Url = this.Url.Remove(28);
                throw ex;
            }

            return deviceResponse;
        }

        #region PRIMEPOS-3526
        /// <summary>
        /// This is the preread functionality
        /// </summary>
        /// <param name="fields"></param>
        /// <returns></returns>
        public VantivResponse PreRead(Dictionary<String, String> fields) //PRIMEPOS-3504
        {
            logger.Trace(" ENTERED IN PREREAD METHOD");
            string actualResponse = string.Empty;
            string postBody = string.Empty;
            if (fields["PREREADTYPE"].ToString().ToUpper().Contains("SALE"))
            {
                this.Url += Constant.Sale;
            }
            else if (fields["PREREADTYPE"].ToString().ToUpper().Contains("RETURN"))
            {
                this.Url += Constant.Refund;
            }
            VantivResponse deviceResponse = new VantivResponse();

            try
            {
                SaleRequest saleRequest = new SaleRequest();
                saleRequest.Configuration = new Configuration();
                saleRequest.Address = new Address();
                saleRequest.healthcare = new Healthcare();

                saleRequest.TicketNumber = fields["TICKETNUMBER"];
                //saleRequest.TransactionAmount = Convert.ToDecimal(fields["AMOUNT"]);
                saleRequest.LaneId = Convert.ToInt32(this.LaneID);
                saleRequest.Configuration.AllowPartialApprovals = true;
                saleRequest.ReferenceNumber = fields["TICKETNUMBER"];
                saleRequest.preRead = true;

                saleRequest.Configuration.allowDebit = true;
                postBody = JsonConvert.SerializeObject(saleRequest);

                actualResponse = Constant.SendRequestPost(postBody, this.Url, this.developerKey, this.developerSecret, this.ApplicationName);
                logger.Trace($"Sale Device response : {actualResponse}");
                deviceResponse.ParsePreReadResponse(actualResponse);
                deviceResponse.deviceRequest = postBody;
                deviceResponse.deviceResponse = actualResponse;
            }
            catch (Exception ex)
            {
                logger.Error(" ERROR IN PREREAD METHOD : " + ex.ToString());
                this.Url = this.Url.Remove(28);
                throw ex;
            }
            return deviceResponse;
        }

        /// <summary>
        /// This method is for sale of preread
        /// </summary>
        /// <param name="fields"></param>
        /// <returns></returns>
        public VantivResponse PreReadSale(Dictionary<String, String> fields) //PRIMEPOS-3504
        {
            logger.Trace(" ENTERED IN PREREAD METHOD");
            string actualResponse = string.Empty;
            string postBody = string.Empty;
            this.Url += Constant.Sale;
            VantivResponse deviceResponse = new VantivResponse();

            try
            {
                SaleRequest saleRequest = new SaleRequest();
                saleRequest.Configuration = new Configuration();
                if (fields.ContainsKey("TOKENREQUEST"))
                {
                    isTokenized = Convert.ToBoolean(fields["TOKENREQUEST"]);
                }

                if (fields.ContainsKey("ISNBSTRANSACTION"))
                {
                    isNBSTransaction = Convert.ToBoolean(fields["ISNBSTRANSACTION"]);
                }
                saleRequest.Address = new Address();
                //saleRequest.healthcare = new Healthcare(); //PRIMEPOS-3545

                saleRequest.TicketNumber = fields["TICKETNUMBER"]; //PRIMEPOS-3518 TENTATIVE
                saleRequest.TransactionAmount = Convert.ToDecimal(fields["AMOUNT"]);
                saleRequest.LaneId = Convert.ToInt32(this.LaneID);
                saleRequest.Configuration.AllowPartialApprovals = true;
                saleRequest.Configuration.CheckForDuplicateTransactions = false;
                saleRequest.quickChip = true;
                saleRequest.preReadId = fields["PREREADID"];
                saleRequest.ReferenceNumber = fields["TICKETNUMBER"]; //PRIMEPOS-3518 TENTATIVE
                saleRequest.Configuration.allowDebit = true;
                if(isNBSTransaction)
                {
                    saleRequest.Configuration.PromptForSignature = Constant.Never;
                }

                #region PRIMEPOS-3545
                //if (fields.ContainsKey("IIASRXAMOUNT"))
                //{
                //    saleRequest.healthcare.prescription = fields["IIASRXAMOUNT"];
                //    saleRequest.healthcare.clinic = "0";
                //    saleRequest.healthcare.dental = "0";
                //    saleRequest.healthcare.total = fields["IIASRXAMOUNT"];
                //    saleRequest.healthcare.vision = "0";
                //}
                //else if (fields.ContainsKey("IIASTRANSACTION") && fields["IIASTRANSACTION"].ToString().ToLower() == "true")
                //{
                //    saleRequest.healthcare.prescription = fields["IIASAUTHORIZEDAMOUNT"];
                //    saleRequest.healthcare.clinic = "0";
                //    saleRequest.healthcare.dental = "0";
                //    saleRequest.healthcare.total = fields["IIASAUTHORIZEDAMOUNT"];
                //    saleRequest.healthcare.vision = "0";
                //}
                #endregion

                postBody = JsonConvert.SerializeObject(saleRequest);

                actualResponse = Constant.SendRequestPost(postBody, this.Url, this.developerKey, this.developerSecret, this.ApplicationName);
                logger.Trace($"Sale Device response : {actualResponse}");
                deviceResponse.ParseSaleResponse(actualResponse);
                deviceResponse.deviceRequest = postBody;
                deviceResponse.deviceResponse = actualResponse;

                if (isTokenized && !isNBSTransaction && deviceResponse?.ResultDescription == "SUCCESS" && deviceResponse?.PaymentType?.ToUpper() == "CREDIT")
                {
                    VantivResponse vantivResponse = new VantivResponse();
                    vantivResponse = PaymentAccountCreate(deviceResponse.TransactionNo, fields);
                    deviceResponse.ProfiledID = vantivResponse?.ProfiledID; //PRIMEPOS-3338
                    deviceResponse.Expiration = vantivResponse?.Expiration; //PRIMEPOS-3338
                }
            }
            catch (Exception ex)
            {
                logger.Error(" ERROR IN PREREAD METHOD : " + ex.ToString());
                this.Url = this.Url.Remove(28);
                throw ex;
            }
            return deviceResponse;
        }
        /// <summary>
        /// This method is for cancel the preread
        /// </summary>
        /// <param name="fields"></param>
        /// <returns></returns>
        public VantivResponse PreReadCancel(Dictionary<String, String> fields) //PRIMEPOS-3504
        {
            logger.Trace("VantivProcessor==>PreReadCancel()==> ENTERED IN NBS CANCEL METHOD ");

            string actualResponse = string.Empty;
            string postBody = string.Empty;
            this.Url += Constant.Cancel;

            VantivResponse deviceResponse = new VantivResponse();

            try
            {
                SaleRequest saleRequest = new SaleRequest();
                saleRequest.Configuration = new Configuration();
                saleRequest.Address = new Address();
                saleRequest.healthcare = new Healthcare();

                saleRequest.LaneId = Convert.ToInt32(this.LaneID);

                postBody = JsonConvert.SerializeObject(saleRequest);

                actualResponse = Constant.SendRequestPost(postBody, this.Url, this.developerKey, this.developerSecret, this.ApplicationName);
                logger.Trace($"VantivProcessor==>PreReadCancel()==> NBS Cancel Device response : {actualResponse}");
                deviceResponse.ParseCancelResponse(actualResponse);

                deviceResponse.deviceRequest = postBody;
                deviceResponse.deviceResponse = actualResponse;

            }
            catch (Exception ex)
            {
                logger.Error("VantivProcessor==>PreReadCancel()==> ERROR IN SALE METHOD : " + ex.ToString());
                this.Url = this.Url.Remove(28);
                throw ex;
            }

            return deviceResponse;
        }

        public VantivResponse PreReadRefund(Dictionary<String, String> fields) //PRIMEPOS-3504
        { 
            string actualResponse = string.Empty;
            string postBody = string.Empty;
            this.Url += Constant.Refund;

            VantivResponse deviceResponse = new VantivResponse();
            try
            {
                preReadRequest preReadRequest = new preReadRequest();
                preReadRequest.Configuration = new Configuration();
                preReadRequest.Configuration.AllowPartialApprovals = true; //Have a doubt
                preReadRequest.Configuration.CheckForDuplicateTransactions = false;
                if (fields.ContainsKey("ISNBSTRANSACTION") && Convert.ToBoolean(fields["ISNBSTRANSACTION"]))
                {
                    preReadRequest.Configuration.PromptForSignature = Constant.Never;
                }
                preReadRequest.Configuration.allowDebit = true;
                preReadRequest.ticketNumber = fields["TICKETNUMBER"];
                preReadRequest.referenceNumber = fields["TICKETNUMBER"];
                if (fields["AMOUNT"].Contains("-"))
                    fields["AMOUNT"] = fields["AMOUNT"].Remove(0, 1);
                preReadRequest.TransactionAmount = Convert.ToDecimal(fields["AMOUNT"]);
                preReadRequest.LaneId = Convert.ToInt32(this.LaneID);
                preReadRequest.quickChip = true;
                preReadRequest.preReadId = fields["PREREADID"];

                postBody = JsonConvert.SerializeObject(preReadRequest);

                actualResponse = Constant.SendRequestPost(postBody, this.Url, this.developerKey, this.developerSecret, this.ApplicationName);

                logger.Trace($"Refund Device response : {actualResponse}");
                deviceResponse.ParseRefundResponse(actualResponse);

                deviceResponse.deviceRequest = postBody;
                deviceResponse.deviceResponse = actualResponse;

                if (deviceResponse.EmvReceipt != null)
                {
                    deviceResponse.EmvReceipt.LaneID = this.LaneID;
                    deviceResponse.EmvReceipt.TerminalID = this.TerminalID;
                }

                this.Url = this.Url.Remove(28);
            }
            catch (Exception ex)
            {
                logger.Error(" ERROR IN REFUND METHOD : ", ex.ToString());
                this.Url = this.Url.Remove(28);
                throw ex;
            }
            return deviceResponse;
        }
        #endregion

        #region PRIMEPOS-3372
        public VantivResponse NBSPreRead(Dictionary<String, String> fields)
        {
            logger.Trace("VantivProcessor==>NBSPreRead()==> ENTERED IN NBSPreRead METHOD ");

            string actualResponse = string.Empty;
            string postBody = string.Empty;

            if (fields["PREREADTYPE"].ToString().ToUpper().Contains("SALE"))
            {
                this.Url += Constant.Sale;
            }
            else if (fields["PREREADTYPE"].ToString().ToUpper().Contains("RETURN"))
            {
                this.Url += Constant.Refund;
            }

            //For initializing the Response class 
            VantivResponse deviceResponse = new VantivResponse();

            try
            {
                if (fields.ContainsKey("TOKENREQUEST"))
                {
                    isTokenized = Convert.ToBoolean(fields["TOKENREQUEST"]);
                }
                //Initializing the Classes
                SaleRequest saleRequest = new SaleRequest();
                saleRequest.Configuration = new Configuration();
                saleRequest.Address = new Address();
                saleRequest.healthcare = new Healthcare();
                saleRequest.TicketNumber = fields["TICKETNUMBER"];
                saleRequest.LaneId = Convert.ToInt32(this.LaneID);
                saleRequest.Configuration.AllowPartialApprovals = true;
                saleRequest.ReferenceNumber = fields["TICKETNUMBER"];//PRIMEPOS-2795
                saleRequest.preRead = true;

                postBody = JsonConvert.SerializeObject(saleRequest);

                //For Sending it in HttpClient
                actualResponse = Constant.SendRequestPost(postBody, this.Url, this.developerKey, this.developerSecret, this.ApplicationName);
                logger.Trace($"VantivProcessor==>NBSPreRead()==> NBSPreRead Device response : {actualResponse}");//PRIMEPOS-3054
                deviceResponse.ParsePreReadResponse(actualResponse);

                deviceResponse.deviceRequest = postBody;
                deviceResponse.deviceResponse = actualResponse;
            }
            catch (Exception ex)
            {
                logger.Error("VantivProcessor==>NBSPreRead()==> ERROR IN NBSPreRead METHOD : " + ex.ToString());
                this.Url = this.Url.Remove(28);
                throw ex;
            }

            return deviceResponse;
        }

        public VantivResponse NBSSale(Dictionary<String, String> fields)
        {
            logger.Trace("VantivProcessor==>NBSSale()==> ENTERED IN NBS SALE METHOD ");

            string actualResponse = string.Empty;
            string postBody = string.Empty;
            this.Url += Constant.Sale;

            //For initializing the Response class 
            VantivResponse deviceResponse = new VantivResponse();

            try
            {
                preReadRequest preReadRequest = new preReadRequest();
                preReadRequest.Configuration = new Configuration();
                preReadRequest.TransactionAmount = Convert.ToDecimal(fields["AMOUNT"]);
                preReadRequest.LaneId = Convert.ToInt32(this.LaneID);
                preReadRequest.Configuration.AllowPartialApprovals = true;
                preReadRequest.Configuration.CheckForDuplicateTransactions = false;
                preReadRequest.quickChip = true;
                preReadRequest.preReadId = fields["PREREADID"];

                preReadRequest.Configuration.allowDebit = true;

                //Serializing the Object
                postBody = JsonConvert.SerializeObject(preReadRequest);

                //For Sending it in HttpClient
                actualResponse = Constant.SendRequestPost(postBody, this.Url, this.developerKey, this.developerSecret, this.ApplicationName);
                logger.Trace($"VantivProcessor==>NBSSale()==> NBS Sale Device response : {actualResponse}");//PRIMEPOS-3054
                deviceResponse.ParseSaleResponse(actualResponse);

                deviceResponse.deviceRequest = postBody;
                deviceResponse.deviceResponse = actualResponse;

            }
            catch (Exception ex)
            {
                logger.Error("VantivProcessor==>NBSSale()==> ERROR IN NBS SALE METHOD : " + ex.ToString());
                this.Url = this.Url.Remove(28);
                throw ex;
            }

            return deviceResponse;
        }

        public VantivResponse NBSRefund(Dictionary<String, String> fields)
        {
            string actualResponse = string.Empty;
            string postBody = string.Empty;
            this.Url += Constant.Refund;

            //For initializing the Response class 
            VantivResponse deviceResponse = new VantivResponse();
            try
            {
                //Initializing the Classes         

                preReadRequest preReadRequest = new preReadRequest();
                preReadRequest.Configuration = new Configuration();
                if (fields["AMOUNT"].Contains("-"))
                    fields["AMOUNT"] = fields["AMOUNT"].Remove(0, 1);
                preReadRequest.TransactionAmount = Convert.ToDecimal(fields["AMOUNT"]);
                preReadRequest.LaneId = Convert.ToInt32(this.LaneID);
                preReadRequest.Configuration.AllowPartialApprovals = true;
                preReadRequest.Configuration.CheckForDuplicateTransactions = false;
                preReadRequest.quickChip = true;
                preReadRequest.preReadId = fields["PREREADID"];
                preReadRequest.Configuration.allowDebit = true;


                //Serializing the Object
                postBody = JsonConvert.SerializeObject(preReadRequest);

                //For Sending it in HttpClient
                actualResponse = Constant.SendRequestPost(postBody, this.Url, this.developerKey, this.developerSecret, this.ApplicationName);

                logger.Trace($"Refund Device response : {actualResponse}");//PRIMEPOS-3054
                deviceResponse.ParseRefundResponse(actualResponse);

                deviceResponse.deviceRequest = postBody;//Saving it for CC_Transmission_Log
                deviceResponse.deviceResponse = actualResponse;//Saving it for CC_Transmission_Log
                //PRIMEPOS-2793
                if (deviceResponse.EmvReceipt != null)
                {
                    deviceResponse.EmvReceipt.LaneID = this.LaneID;
                    deviceResponse.EmvReceipt.TerminalID = this.TerminalID;
                }
                //
                this.Url = this.Url.Remove(28);
            }
            catch (Exception ex)
            {
                logger.Error(" ERROR IN NBS REFUND METHOD : ", ex.ToString());
                this.Url = this.Url.Remove(28);
                throw ex;
            }
            return deviceResponse;
        }

        public VantivResponse NBSCancel(Dictionary<String, String> fields)
        {
            logger.Trace("VantivProcessor==>NBSCancel()==> ENTERED IN NBS CANCEL METHOD ");

            string actualResponse = string.Empty;
            string postBody = string.Empty;
            this.Url += Constant.Cancel;

            //For initializing the Response class 
            VantivResponse deviceResponse = new VantivResponse();

            try
            {
                SaleRequest saleRequest = new SaleRequest();
                saleRequest.Configuration = new Configuration();
                saleRequest.Address = new Address();
                saleRequest.healthcare = new Healthcare();

                saleRequest.LaneId = Convert.ToInt32(this.LaneID);

                //Serializing the Object
                postBody = JsonConvert.SerializeObject(saleRequest);

                //For Sending it in HttpClient
                actualResponse = Constant.SendRequestPost(postBody, this.Url, this.developerKey, this.developerSecret, this.ApplicationName);
                logger.Trace($"VantivProcessor==>NBSCancel()==> NBS Cancel Device response : {actualResponse}");//PRIMEPOS-3054
                deviceResponse.ParseCancelResponse(actualResponse);

                deviceResponse.deviceRequest = postBody;
                deviceResponse.deviceResponse = actualResponse;

            }
            catch (Exception ex)
            {
                logger.Error("VantivProcessor==>NBSCancel()==> ERROR IN SALE METHOD : " + ex.ToString());
                this.Url = this.Url.Remove(28);
                throw ex;
            }

            return deviceResponse;
        }

        public VantivResponse NBSSaleReturn(Dictionary<String, String> fields)
        {
            VantivResponse deviceResponse = new VantivResponse();
            try
            {
                string postBody = string.Empty;
                string transactionType = string.Empty;
                if (fields["NBSSALETYPE"].ToString().ToUpper().Contains("CREDIT"))
                {
                    transactionType = "credit";
                    this.Url += Constant.Return + "/" + fields["TRANSACTIONID"] + "/" + transactionType;
                }
                else if (fields["NBSSALETYPE"].ToString().ToUpper().Contains("DEBIT"))
                {
                    transactionType = "debit";
                    this.Url += Constant.Reversal + "/" + fields["TRANSACTIONID"] + "/" + transactionType;
                }

                StrictReturn strictReturn = new RequestModels.StrictReturn();
                strictReturn.laneId = this.LaneID;
                if (fields["TRANSACTIONTYPE"].ToString().ToUpper().Contains("EBT"))
                {
                    strictReturn.ebtType = "FoodStamp";
                }
                strictReturn.ticketNumber = fields["TICKETNUMBER"];
                strictReturn.transactionAmount = fields["AMOUNT"];
                strictReturn.configuration = new Configuration();
                //if (Convert.ToBoolean(fields["ALLOWDUP"]) == true)
                //    strictReturn.configuration.CheckForDuplicateTransactions = true;
                //else
                //    strictReturn.configuration.CheckForDuplicateTransactions = false;
                strictReturn.clerkNumber = fields["TICKETNUMBER"];
                strictReturn.referenceNumber = fields["TICKETNUMBER"];//PRIMEPOS-2795 ADDED BY ARVIND VANTIV

                postBody = JsonConvert.SerializeObject(strictReturn);

                //For Sending it in HttpClient
                string actualResponse = Constant.SendRequestPost(postBody, this.Url, this.developerKey, this.developerSecret, this.ApplicationName);


                deviceResponse.ParseReturnResponse(actualResponse);

                deviceResponse.deviceRequest = postBody;
                deviceResponse.deviceResponse = actualResponse;

                //PRIMEPOS-2793
                if (deviceResponse.EmvReceipt != null)
                {
                    deviceResponse.EmvReceipt.LaneID = this.LaneID;
                    deviceResponse.EmvReceipt.TerminalID = this.TerminalID;
                }
                //

                this.Url = this.Url.Remove(28);

            }
            catch (Exception ex)
            {
                logger.Error(" ERROR IN StrictReturn METHOD : ", ex.ToString());
                this.Url = this.Url.Remove(28);
                throw ex;
            }
            return deviceResponse;
        }

        #endregion

        /// <summary>
        /// This is the Refund Method for Credit and Debit Transaction
        /// </summary>
        /// <param name="fields"></param>
        /// <returns></returns>
        public VantivResponse Refund(Dictionary<String, String> fields)
        {
            string actualResponse = string.Empty;
            string postBody = string.Empty;
            this.Url += Constant.Refund;

            //For initializing the Response class 
            VantivResponse deviceResponse = new VantivResponse();
            try
            {
                //Initializing the Classes
                RefundRequest refundRequest = new RefundRequest();
                refundRequest.configuration = new Configuration();

                refundRequest.configuration.AllowPartialApprovals = false;
                refundRequest.configuration.CheckForDuplicateTransactions = true;
                refundRequest.configuration.allowDebit = true;
                refundRequest.address = new Address();
                refundRequest.ticketNumber = fields["TICKETNUMBER"];
                refundRequest.referenceNumber = fields["TICKETNUMBER"];//PRIMEPOS-2795            
                if (fields["AMOUNT"].Contains("-"))
                    fields["AMOUNT"] = fields["AMOUNT"].Remove(0, 1);
                refundRequest.transactionAmount = fields["AMOUNT"];
                refundRequest.laneId = this.LaneID;

                //Serializing the Object
                postBody = JsonConvert.SerializeObject(refundRequest);

                //For Sending it in HttpClient
                actualResponse = Constant.SendRequestPost(postBody, this.Url, this.developerKey, this.developerSecret, this.ApplicationName);

                logger.Trace($"Refund Device response : {actualResponse}");//PRIMEPOS-3054
                deviceResponse.ParseRefundResponse(actualResponse);

                deviceResponse.deviceRequest = postBody;//Saving it for CC_Transmission_Log
                deviceResponse.deviceResponse = actualResponse;//Saving it for CC_Transmission_Log
                //PRIMEPOS-2793
                if (deviceResponse.EmvReceipt != null)
                {
                    deviceResponse.EmvReceipt.LaneID = this.LaneID;
                    deviceResponse.EmvReceipt.TerminalID = this.TerminalID;
                }
                //
                this.Url = this.Url.Remove(28);
            }
            catch (Exception ex)
            {
                logger.Error(" ERROR IN REFUND METHOD : ", ex.ToString());
                this.Url = this.Url.Remove(28);
                throw ex;
            }
            return deviceResponse;
        }
        /// <summary>
        /// This is the Void Method for Credit and Debit Transaction
        /// </summary>
        /// <param name="fields"></param>
        /// <returns></returns>
        public VantivResponse Void(Dictionary<String, String> fields)
        {
            //Intitializing the Response class
            VantivResponse deviceResponse = new VantivResponse();

            try
            {

                deviceResponse = Reversal(fields);

                if (deviceResponse.ResultDescription == "SUCCESS")
                {
                    return deviceResponse;
                }

                logger.Trace(" ENTERED IN Void METHOD ");

                string actualResponse = string.Empty;
                string postBody = string.Empty;
                this.Url += Constant.Void;

                this.Url += "/" + fields["TRANSACTIONID"];

                VoidRequest voidRequest = new VoidRequest();
                voidRequest.configuration = new Configuration();
                voidRequest.ticketNumber = fields["TICKETNUMBER"];
                voidRequest.referenceNumber = fields["TICKETNUMBER"];
                #region PRIMEPOS-2795
                if (fields.ContainsKey("AMOUNT"))
                {
                    if (!fields["AMOUNT"].Contains("-"))
                        voidRequest.approvedAmount = fields["AMOUNT"];
                    else
                    {
                        fields["AMOUNT"] = fields["AMOUNT"].Remove(0, 1);
                        voidRequest.approvedAmount = fields["AMOUNT"];
                    }
                }
                #endregion
                voidRequest.laneId = this.LaneID;

                postBody = JsonConvert.SerializeObject(voidRequest);

                actualResponse = Constant.SendRequestPost(postBody, this.Url, this.developerKey, this.developerSecret, this.ApplicationName);
                logger.Trace($"VOID Device response : {actualResponse}");//PRIMEPOS-3054
                deviceResponse.ParseVoidResponse(actualResponse);

                deviceResponse.deviceRequest = postBody;//Saving it for CC_Transmission_Log
                deviceResponse.deviceResponse = actualResponse;//Saving it for CC_Transmission_Log

                //PRIMEPOS-2793
                if (deviceResponse.EmvReceipt != null)
                {
                    deviceResponse.EmvReceipt.LaneID = this.LaneID;
                    deviceResponse.EmvReceipt.TerminalID = this.TerminalID;
                }
                //
                this.Url = this.Url.Remove(28);
            }
            catch (Exception ex)
            {
                logger.Error(" ERROR IN VOID METHOD : ", ex.ToString());
                this.Url = this.Url.Remove(28);
                throw ex;
            }
            return deviceResponse;
        }
        /// <summary>
        /// This Method is used for Electronic Benefit Transfer(EBT)  Transaction
        /// </summary>
        /// <param name="fields"></param>
        /// <returns></returns>
        public VantivResponse EBT(Dictionary<String, String> fields)
        {
            logger.Trace(" ENTERED IN EBT METHOD ");

            string actualResponse = string.Empty;
            string postBody = string.Empty;
            this.Url += Constant.EBTvoucher;

            //Initiazling the Response class
            VantivResponse deviceResponse = new VantivResponse();

            try
            {
                EBTRequest ebtRequest = new EBTRequest();
                ebtRequest.configuration = new Configuration();
                ebtRequest.voucherNumber = fields["TICKETNUMBER"];
                ebtRequest.approvalNumber = fields["TICKETNUMBER"];
                ebtRequest.ticketNumber = fields["TICKETNUMBER"];
                ebtRequest.transactionAmount = fields["AMOUNT"];
                ebtRequest.laneId = this.LaneID;

                postBody = JsonConvert.SerializeObject(ebtRequest);

                actualResponse = Constant.SendRequestPost(postBody, this.Url, this.developerKey, this.developerSecret, this.ApplicationName);

                logger.Trace($"EBT Device response : {actualResponse}");//PRIMEPOS-3054
                deviceResponse.ParseEBTResponse(actualResponse);

                deviceResponse.deviceRequest = postBody;//Saving it for CC_Transmission_Log
                deviceResponse.deviceResponse = actualResponse;//Saving it for CC_Transmission_Log
                this.Url = this.Url.Remove(28);
            }
            catch (Exception ex)
            {
                logger.Error(" ERROR IN EBT METHOD : ", ex.ToString());
                this.Url = this.Url.Remove(28);
                throw ex;
            }
            return deviceResponse;
        }
        /// <summary>
        /// This MEthod is used for Capturing the Signature 
        /// </summary>
        /// <returns></returns>
        public VantivResponse GetSimpleSignature(string DelayInSecond)
        {
            logger.Trace(" ENTERED IN GetSimpleSignature METHOD ");

            string actualResponse = string.Empty;
            this.Url += Constant.Signature;
            this.Url += "/" + this.LaneID;

            //For initializing the Response class 
            VantivResponse deviceResponse = new VantivResponse();

            try
            {
                try
                {
                    int Second = Convert.ToInt32(DelayInSecond) * 1000;
                    Thread.Sleep(Second);//Arvind for Signature issue API
                }
                catch (Exception ex)
                {
                    logger.Error("Error in DelaySecond" + ex);
                    Thread.Sleep(2000);
                }
                //For Sending it in HttpClient
                actualResponse = Constant.SendRequestGet(this.Url, this.developerKey, this.developerSecret);
                logger.Trace($"GetSimpleSignature Device response : {actualResponse}");//PRIMEPOS-3054
                deviceResponse.ParseSignatureResponse(actualResponse);

                deviceResponse.SignatureData = deviceResponse.SignatureString;
                deviceResponse.deviceResponse = actualResponse;
                this.Url = this.Url.Remove(28);
            }
            catch (Exception ex)
            {
                logger.Error(" ERROR IN GetSimpleSignature METHOD : ", ex.ToString());
                this.Url = this.Url.Remove(28);
                throw ex;
            }

            return deviceResponse;
        }
        /// <summary>
        /// This Method is used for displaying the Thankyou Screen after the Transaction
        /// </summary>
        /// <param name="Message"></param>
        /// <returns></returns>
        public VantivResponse DisplayThankYouMessage(String Message)
        {
            logger.Trace(" ENTERED IN DisplayThankYouMessage METHOD ");

            string actualResponse = string.Empty;
            string postBody = string.Empty;
            this.Url += Constant.Display;

            //For initializing the Response class 
            VantivResponse deviceResponse = new VantivResponse();

            try
            {
                Display display = new Display();
                display.laneId = this.LaneID;
                display.text = Message;

                postBody = JsonConvert.SerializeObject(display);

                //For Sending it in HttpClient
                actualResponse = Constant.SendRequestPost(postBody, this.Url, this.developerKey, this.developerSecret, this.ApplicationName);

                logger.Trace($"DisplayThankYouMessage Device response : {actualResponse}");//PRIMEPOS-3054
                var response = JsonConvert.DeserializeObject<Display>(actualResponse);

                //deviceResponse.deviceRequest = postBody;//Saving it for CC_Transmission_Log
                deviceResponse.deviceResponse = actualResponse;//Saving it for CC_Transmission_Log
                this.Url = this.Url.Remove(28);

                IdleScreen(this.Url, this.LaneID, this.ApplicationName, this.StationID);
            }
            catch (Exception ex)
            {
                logger.Error(" ERROR IN DisplayThankYouMessage METHOD : ", ex.ToString());
                this.Url = this.Url.Remove(28);
                throw ex;
            }

            return deviceResponse;
        }
        /// <summary>
        /// This Screen is used to Show the Items of POS in the Device Screen 
        /// </summary>
        /// <param name="dataList"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public VantivResponse ShowItemScreen(Dictionary<int, Hashtable> dataList, string action)
        {
            logger.Trace(" ENTERED IN ShowItemScreen METHOD ");

            //var item = dataList.Where(w => w.Value["Qty"].ToString().Trim() == "2").SingleOrDefault();

            string actualResponse = string.Empty;
            string postBody = string.Empty;
            decimal price = 0;
            string name = string.Empty;
            decimal tax = 0;
            decimal discount = 0;
            decimal total = 0;
            decimal Qty = 0;
            decimal totalAmount = 0;


            //For initializing the Response class 
            VantivResponse deviceResponse = new VantivResponse();

            try
            {
                logger.Trace("Action :" + action);
                if (action == "A")
                {
                    this.Url += Constant.ScrollingDisplay;
                    if (!isTitleAdded)
                    {
                        string title = String.Format("{0,0} {1,21} {2,5} {3,15} {4,15}", Constant.Description.PadRight(15, ' '), Constant.Unit_Price, Constant.Quantity, Constant.Discount.PadLeft(10, ' '), Constant.Total.PadLeft(10, ' '));
                        ScrollingDisplay scrollingdisplaytitle = new ScrollingDisplay();
                        scrollingdisplaytitle.laneId = this.LaneID;
                        scrollingdisplaytitle.lineItem = title;
                        scrollingdisplaytitle.subtotal = "0.00";
                        scrollingdisplaytitle.tax = "0.00";
                        scrollingdisplaytitle.total = "0.00";

                        postBody = JsonConvert.SerializeObject(scrollingdisplaytitle);

                        //For Sending it in HttpClient
                        actualResponse = Constant.SendRequestPost(postBody, this.Url, this.developerKey, this.developerSecret, this.ApplicationName);


                        isTitleAdded = true;
                    }

                    Hashtable items = new Hashtable();
                    dataList.TryGetValue(dataList.Keys.Max(), out items);
                    name = items["Name"].ToString().Trim();
                    decimal prices = Convert.ToDecimal(items["Price"].ToString().Trim());
                    total = Convert.ToDecimal(items["Total"].ToString().Trim());
                    Qty = Convert.ToDecimal(items["Qty"].ToString().Trim());
                    discount = Convert.ToDecimal(items["Discount"].ToString().Trim());
                    tax = Convert.ToDecimal(items["Tax"].ToString().Trim());

                    displayAddItem.laneId = this.LaneID;
                    displayAddItem.lineItem = String.Format("{0,0} {1,21} {2,5} {3,15} {4,15}", name.PadRight(15, ' '), prices.ToString(), Qty.ToString(), discount.ToString().PadLeft(10, ' '), total.ToString().PadLeft(10, ' '));
                    if (displayAddItem.subtotal != null)
                    {
                        displayAddItem.subtotal = (Convert.ToDecimal(displayAddItem.subtotal) + prices).ToString();
                    }
                    else
                        displayAddItem.subtotal = prices.ToString();
                    if (displayAddItem.tax != null)
                        displayAddItem.tax = (Convert.ToDecimal(displayAddItem.tax) + tax).ToString();
                    else
                        displayAddItem.tax = tax.ToString();
                    #region PrimePOS-3188
                    //if (displayAddItem.total != null)
                    //    displayAddItem.total = (Convert.ToDecimal(displayAddItem.total) + prices + tax).ToString();
                    //else
                    //    displayAddItem.total = (prices + tax).ToString();

                    if (displayAddItem.total != null)
                        displayAddItem.total = (Convert.ToDecimal(displayAddItem.total) + total + tax).ToString();
                    else
                        displayAddItem.total = (total + tax).ToString();
                    #endregion


                    postBody = JsonConvert.SerializeObject(displayAddItem);

                    //For Sending it in HttpClient
                    actualResponse = Constant.SendRequestPost(postBody, this.Url, this.developerKey, this.developerSecret, this.ApplicationName);
                    logger.Trace($"ShowItemScreen Device response : {actualResponse}");//PRIMEPOS-3054
                    this.Url = this.Url.Remove(28);
                }
                else if (action == "R" || action == "D" || action == "U" && dataList.Count > 0)
                {
                    IdleScreen(this.Url, this.LaneID, this.ApplicationName, this.StationID);

                    this.Url += Constant.ScrollingDisplay;
                    string title = String.Format("{0,0} {1,21} {2,5} {3,15} {4,15}", Constant.Description.PadRight(15, ' '), Constant.Unit_Price, Constant.Quantity, Constant.Discount.PadLeft(10, ' '), Constant.Total.PadLeft(10, ' '));
                    ScrollingDisplay scrollingdisplaytitle = new ScrollingDisplay();
                    scrollingdisplaytitle.laneId = this.LaneID;
                    scrollingdisplaytitle.lineItem = title;
                    scrollingdisplaytitle.subtotal = "0.00";
                    scrollingdisplaytitle.tax = "0.00";
                    scrollingdisplaytitle.total = "0.00";


                    //display.text = Message;

                    postBody = JsonConvert.SerializeObject(scrollingdisplaytitle);

                    //For Sending it in HttpClient
                    actualResponse = Constant.SendRequestPost(postBody, this.Url, this.developerKey, this.developerSecret, this.ApplicationName);


                    isTitleAdded = true;

                    ScrollingDisplay displayReloadItem = new ScrollingDisplay();
                    decimal subtotal = 0;//PrimePOS-3188
                    foreach (KeyValuePair<int, Hashtable> data in dataList)
                    {
                        Hashtable items = data.Value;
                        tax += Convert.ToDecimal(items["Tax"].ToString().Trim());
                        price += Convert.ToDecimal(items["Price"].ToString().Trim());
                        #region PrimePOS-3188
                        //if (items["Total"].ToString().Trim() == "0.00")
                        //    totalAmount += Convert.ToDecimal(items["Price"].ToString().Trim());
                        //else
                        #endregion
                        totalAmount += Convert.ToDecimal(items["Total"].ToString().Trim());
                        name = items["Name"].ToString().Trim();
                        decimal prices = Convert.ToDecimal(items["Price"].ToString().Trim());
                        total = Convert.ToDecimal(items["Total"].ToString().Trim());
                        Qty = Convert.ToDecimal(items["Qty"].ToString().Trim());
                        discount = Convert.ToDecimal(items["Discount"].ToString().Trim());

                        displayReloadItem.laneId = this.LaneID;
                        displayReloadItem.lineItem = String.Format("{0,0} {1,21} {2,5} {3,15} {4,15}", name.PadRight(15, ' '), prices.ToString(), Qty.ToString(), discount.ToString().PadLeft(10, ' '), total.ToString().PadLeft(10, ' '));
                        //displayReloadItem.subtotal = totalAmount.ToString();//PrimePOS-3188
                        subtotal += (Qty * prices);//PrimePOS-3188
                        displayReloadItem.subtotal = subtotal.ToString();//PrimePOS-3188
                        displayReloadItem.tax = tax.ToString();
                        displayReloadItem.total = (totalAmount + tax).ToString();


                        postBody = JsonConvert.SerializeObject(displayReloadItem);

                        //For Sending it in HttpClient
                        actualResponse = Constant.SendRequestPost(postBody, this.Url, this.developerKey, this.developerSecret, this.ApplicationName);
                        logger.Trace($"ShowItemScreen Device response : {actualResponse}");//PRIMEPOS-3054
                    }
                    displayAddItem.total = displayReloadItem.total;
                    displayAddItem.subtotal = displayReloadItem.subtotal;
                    displayAddItem.tax = displayReloadItem.tax;
                    this.Url = this.Url.Remove(28);
                    isTitleAdded = true;
                }
                else
                {
                    IdleScreen(this.Url, this.LaneID, this.ApplicationName, this.StationID);
                    displayAddItem = new ScrollingDisplay();
                    isTitleAdded = false;
                    dataList.Clear();
                }
            }
            catch (Exception ex)
            {
                logger.Error(" ERROR IN ShowItemScreen METHOD : ", ex.ToString());
                this.Url = this.Url.Remove(28);
                throw ex;
            }

            return deviceResponse;
        }
        /// <summary>
        /// This Method is used for Selecting the Patient type
        /// </summary>
        /// <param name="Data"></param>
        /// <returns></returns>
        public VantivResponse ShowDialogForm(Hashtable Data)
        {
            logger.Trace(" ENTERED IN ShowDialogForm METHOD ");

            string actualResponse = string.Empty;
            this.Url += Constant.Selection;
            this.Url += "/" + this.LaneID;

            //For initializing the Response class 
            VantivResponse deviceResponse = new VantivResponse();

            try
            {
                //string a = "?form = MultiOptionTextArea & header = Header & subHeader = Subheader & text = detailed % 20text % 20line % 201 % 0Adetailed % 20text % 20line % 202 & options = one | two | three";

                var uriBuilder = new UriBuilder(Url);
                var query = System.Web.HttpUtility.ParseQueryString(uriBuilder.Query);
                query["form"] = "MultiOptionTextArea";
                query["header"] = Data["Header"].ToString();
                query["text"] = Data["TEXT1"].ToString() + "\n" + Data["TEXT2"].ToString() + "\n" + Data["TEXT3"].ToString() + "\n" + Data["TEXT4"].ToString();
                query["options"] = "Option1|Option2|Option3|Option4";
                uriBuilder.Query = query.ToString();
                Url = uriBuilder.ToString();

                //For Sending it in HttpClient
                actualResponse = Constant.SendRequestGet(this.Url, this.developerKey, this.developerSecret);
                logger.Trace($"ShowDialogForm Device response : {actualResponse}");//PRIMEPOS-3054
                var selectionResponse = JsonConvert.DeserializeObject<SelectionResponse>(actualResponse);

                deviceResponse.buttonNumber = selectionResponse.selectionIndex.ToString();

                this.Url = this.Url.Remove(28);
            }
            catch (Exception ex)
            {
                logger.Error(" ERROR IN ShowDialogForm METHOD : ", ex.ToString());
                this.Url = this.Url.Remove(28);
                throw ex;
            }


            return deviceResponse;
        }
        /// <summary>
        /// This Method is used for PatientCounselling , HealthRx ,OTC items
        /// </summary>
        /// <param name="title"></param>
        /// <param name="message"></param>
        /// <param name="button1"></param>
        /// <param name="button2"></param>
        /// <param name="button3"></param>
        /// <param name="screenDisplay"></param>
        /// <returns></returns>
        public VantivResponse ShowTextBox(string title, string message, string button1, string button2, string button3, string screenDisplay, string DelayInSecond, bool bHidePatCounseling = false) //PRIMEPOS-3302- added bHidePatCounseling
        {
            logger.Trace(" ENTERED IN ShowTextBox METHOD ");

            string actualResponse = string.Empty;
            this.Url += Constant.Selection;
            this.Url += "/" + this.LaneID;

            //For initializing the Response class 
            VantivResponse deviceResponse = new VantivResponse();

            try
            {
                var uriBuilder = new UriBuilder(Url);
                var query = System.Web.HttpUtility.ParseQueryString(uriBuilder.Query);
                try
                {
                    int Second = Convert.ToInt32(DelayInSecond) * 1000;
                    Thread.Sleep(Second);//Arvind for Signature issue API
                }
                catch (Exception ex)
                {
                    logger.Error("Error in DelaySecond" + ex);
                    Thread.Sleep(2000);
                }
                if (screenDisplay == "HEALTHIXFRM1")
                {
                    //string a = "?form = MultiOptionTextArea & header = Header & subHeader = Subheader & text = detailed % 20text % 20line % 201 % 0Adetailed % 20text % 20line % 202 & options = one | two | three";

                    query["form"] = "MultiOptionTextArea";
                    query["header"] = title;
                    query["text"] = message;
                    query["options"] = button1 != null ? button1 : "" + "|" + button2 != null ? button2 : "" + "|" + button3 != null ? button3 : "";
                }
                else if (screenDisplay == "HEALTHIXFRM2")
                {
                    query["form"] = "MultiOptionTextArea";
                    query["header"] = title;
                    query["text"] = message + "\n" + "Option 1 - " + button1 + "\nOption 2 - " + button2;//PRIMEPOS-3063
                    query["options"] = "Option1|Option2|" + button3;
                }
                else if (screenDisplay == "HEALTHIXFRM3")
                {
                    query["form"] = "MultiOptionTextArea";
                    query["header"] = title;
                    query["text"] = message + "\n" + "Option 1 - " + button1 + "\nOption 2 - " + button2 + "\nOption 3 - " + button3;//PRIMEPOS-3063
                    query["options"] = "Option1|Option2|Option3";
                }
                #region PRIMEPOS-3063
                //else if (screenDisplay == "HIPPAACK")
                //{
                //    query["form"] = "YesNoTextArea";
                //    query["header"] = title;
                //    query["text"] = message;
                //}

                else if (screenDisplay == "HIPPAACK")
                {
                    query["form"] = "MultiOptionTextArea";
                    query["header"] = title;
                    query["text"] = message;
                    query["options"] = "No|Yes";
                }
                #endregion
                #region PRIMEPOS-3054 Begin
                //else if (screenDisplay == "RXHEADERINFO")
                //{
                //    query["form"] = "YesNoTextArea";
                //    query["header"] = title;
                //    query["text"] = message;
                //}
                else if (screenDisplay == "RXHEADERINFO")
                {
                    if (!bHidePatCounseling)//PRIMEPOS-3302
                    {
                        query["form"] = "MultiOptionTextArea";
                        query["header"] = "Patient Counseling";
                        query["text"] = message;
                        query["subheader"] = "RX's being pickedup!";
                        query["options"] = "No|Yes";//PRIMEPOS-3063
                    }
                    else
                    {
                        query["form"] = "MultiOptionTextArea";
                        query["header"] = title;
                        query["text"] = message;
                        query["subheader"] = "RX's being pickedup!";
                        query["options"] = button1;
                    }
                }
                #endregion PRIMEPOS-3054 End
                else if (screenDisplay == "OTCITEMDESC")
                {
                    query["form"] = "MultiOptionTextArea";
                    query["header"] = title;
                    query["text"] = message;
                    query["options"] = button1;
                }
                uriBuilder.Query = query.ToString();
                Url = uriBuilder.ToString();

                //For Sending it in HttpClient
                actualResponse = Constant.SendRequestGet(this.Url, this.developerKey, this.developerSecret);
                logger.Trace($"ShowTextBox Device response : {actualResponse}");//PRIMEPOS-3054

                var selectionResponse = JsonConvert.DeserializeObject<SelectionResponse>(actualResponse);

                deviceResponse.buttonNumber = selectionResponse.selectionIndex.ToString();

                this.Url = this.Url.Remove(28);
            }
            catch (Exception ex)
            {
                logger.Error(" ERROR IN ShowTextBox METHOD : ", ex.ToString());
                this.Url = this.Url.Remove(28);
                throw ex;
            }

            return deviceResponse;
        }
        /// <summary>
        /// This Method is used to display the Current Message Screen in the POS
        /// </summary>
        /// <param name="Title"></param>
        /// <param name="Message"></param>
        /// <returns></returns>
        public VantivResponse DisplayMessage(String Title, String Message)
        {
            logger.Trace(" ENTERED IN DisplayMessage METHOD ");

            string actualResponse = string.Empty;
            string postBody = string.Empty;
            this.Url += Constant.Display;

            //For initializing the Response class 
            VantivResponse deviceResponse = new VantivResponse();

            try
            {
                Display display = new Display();
                display.laneId = this.LaneID;
                display.text = Message;

                postBody = JsonConvert.SerializeObject(display);

                //For Sending it in HttpClient
                actualResponse = Constant.SendRequestPost(postBody, this.Url, this.developerKey, this.developerSecret, this.ApplicationName);

                logger.Trace($"DisplayMessage Device response : {actualResponse}");//PRIMEPOS-3054
                var response = JsonConvert.DeserializeObject<Display>(actualResponse);

                //deviceResponse.deviceRequest = postBody;//Saving it for CC_Transmission_Log
                deviceResponse.deviceResponse = actualResponse;//Saving it for CC_Transmission_Log
                this.Url = this.Url.Remove(28);

                isTitleAdded = false;

                displayAddItem = new ScrollingDisplay();
            }
            catch (Exception ex)
            {
                logger.Error(" ERROR IN DisplayMessage METHOD : ", ex.ToString());
                this.Url = this.Url.Remove(28);
                throw ex;
            }

            return deviceResponse;
        }
        /// <summary>
        /// The Method is used to create a Payment Token for re using it in future for payment
        /// </summary>
        /// <param name="TransactionID"></param>
        /// <param name="fields"></param>
        /// <returns></returns>
        public VantivResponse PaymentAccountCreate(string TransactionID, Dictionary<String, String> fields)
        {
            VantivResponse deviceResponse = new VantivResponse(); //PRIMEPOS-3338
            try
            {
                //VantivResponse deviceResponse = new VantivResponse();
                //string xmlns = string.Empty;
                PaymentAccountCreateWithTransID paymentAccountCreate = new PaymentAccountCreateWithTransID();

                //if (fields.ContainsKey("VANTIVPMTACCAPI"))
                //{
                //    xmlns = fields["VANTIVPMTACCAPI"].Remove(8, 4);
                //}

                paymentAccountCreate.Xmlns = "https://services.elementexpress.com";
                paymentAccountCreate.Credentials = new Credentials();
                paymentAccountCreate.Application = new Application();
                paymentAccountCreate.PaymentAccount = new PaymentAccount();
                paymentAccountCreate.Transaction = new Transaction();
                //paymentAccountCreate.Terminal = new Terminal();//PRIMEPOS-2769

                paymentAccountCreate.Credentials.AcceptorID = this.AcceptorID;
                paymentAccountCreate.Credentials.AccountID = this.AccountID;
                paymentAccountCreate.Credentials.AccountToken = this.AccountToken;

                paymentAccountCreate.Application.ApplicationID = "10443";//PRIMEPOS-2796
                paymentAccountCreate.Application.ApplicationName = ApplicationName;
                paymentAccountCreate.Application.ApplicationVersion = Version.ToString();

                paymentAccountCreate.PaymentAccount.PaymentAccountType = "0";
                paymentAccountCreate.PaymentAccount.PaymentAccountReferenceNumber = fields["TICKETNUMBER"];

                paymentAccountCreate.Transaction.TransactionID = TransactionID;
                //paymentAccountCreate.Transaction.ReferenceNumber = fields["TICKETNUMBER"];//PRIMEPOS-2795
                #region PRIMEPOS-2769
                //paymentAccountCreate.Terminal.TerminalID = this.TerminalID;
                //paymentAccountCreate.Terminal.TerminalType = "3";
                //paymentAccountCreate.Terminal.CardholderPresentCode = "3";
                //paymentAccountCreate.Terminal.CardPresentCode = "3";
                //paymentAccountCreate.Terminal.TerminalEnvironmentCode = "2";
                #endregion
                //paymentAccountCreate.Transaction.PaymentType = "4";
                //paymentAccountCreate.Transaction.SubmissionType = "1";

                //XmlSerializerNamespaces xsn = new XmlSerializerNamespaces();
                //xsn.Add("", xmlns);//"https://services.elementexpress.com");

                string req = XmlHelper.Serialize(paymentAccountCreate/*,xsn*/);

                deviceResponse.deviceRequest = req;
                //req = req.Replace("<?xml version=\"1.0\" encoding=\"utf-16\"?>\r\n", "");

                logger.Trace("The request is :" + req.Replace(this.AcceptorID, "***").Replace(this.AccountID, "***").Replace(this.AccountToken, "***")); //PRIMEPOS-3156

                string res = postXMLData(fields.ContainsKey("ACCOUNTURL") ? fields["ACCOUNTURL"] : "", req);
                //paymentAccountCreate = XmlHelper.Deserialize<PaymentAccountCreateWithTransID>(res);
                logger.Trace($"PaymentAccountCreate Device response : {res}");//PRIMEPOS-3054
                deviceResponse.ParsePaymentCreateResponse(res);

                //return deviceResponse; //PRIMEPOS-3338
            }
            catch (Exception ex)
            {
                logger.Error("Error in PaymentAccountAPI : " + ex.ToString());
                //throw ex; //PRIMEPOS-3338
            }
            return deviceResponse; //PRIMEPOS-3338
        }
        /// <summary>
        /// This method is used for the Creditcard sale with token
        /// </summary>
        /// <param name="fields"></param>
        /// <returns></returns>
        public VantivResponse CreditCardSale(Dictionary<String, String> fields)
        {
            VantivResponse deviceResponse = new VantivResponse();

            CreditCardSale ccTokenSale = new CreditCardSale();
            try
            {
                ccTokenSale.Credentials = new Credentials();
                ccTokenSale.Application = new Application();
                ccTokenSale.Transaction = new HealthCare.Transaction();
                ccTokenSale.Terminal = new Terminal();
                ccTokenSale.PaymentAccount = new PaymentAccount();

                ccTokenSale.Xmlns = "https://transaction.elementexpress.com";
                ccTokenSale.Credentials.AcceptorID = this.AcceptorID;
                ccTokenSale.Credentials.AccountID = this.AccountID;
                ccTokenSale.Credentials.AccountToken = this.AccountToken;

                ccTokenSale.Application.ApplicationID = "10443";//PRIMEPOS-2796
                ccTokenSale.Application.ApplicationName = ApplicationName;
                ccTokenSale.Application.ApplicationVersion = Version.ToString();

                ccTokenSale.Transaction.TicketNumber = fields["TICKETNUMBER"];
                ccTokenSale.Transaction.ReferenceNumber = fields["TICKETNUMBER"];//PRIMEPOS-2795 
                ccTokenSale.Transaction.MarketCode = "2";
                ccTokenSale.Transaction.TransactionAmount = fields["AMOUNT"];
                ccTokenSale.Transaction.PartialApprovedFlag = 1;

                ccTokenSale.Terminal.TerminalID = this.TerminalID;//PRIMEPOS-2769
                ccTokenSale.Terminal.TerminalType = "3";//PRIMEPOS-2769
                ccTokenSale.Terminal.CardPresentCode = "3";//PRIMEPOS-2769
                ccTokenSale.Terminal.CardholderPresentCode = "3";//PRIMEPOS-2769
                ccTokenSale.Terminal.CardInputCode = "4";
                ccTokenSale.Terminal.CVVPresenceCode = "1";
                ccTokenSale.Terminal.TerminalCapabilityCode = "5";
                ccTokenSale.Terminal.TerminalEnvironmentCode = "2";//PRIMEPOS-2769
                ccTokenSale.Terminal.MotoECICode = "2";
                bool isFSATrans = false;
                if (fields.ContainsKey("IIASRXAMOUNT"))
                {
                    EnhancedBINQuery enhancedBINQuery = new EnhancedBINQuery();
                    enhancedBINQuery.Application = new Application();
                    enhancedBINQuery.Terminal = new Terminal();
                    enhancedBINQuery.PaymentAccount = new PaymentAccount();
                    enhancedBINQuery.Credentials = new Credentials();

                    enhancedBINQuery.Xmlns = "https://transaction.elementexpress.com";

                    enhancedBINQuery.Credentials.AcceptorID = this.AcceptorID;
                    enhancedBINQuery.Credentials.AccountID = this.AccountID;
                    enhancedBINQuery.Credentials.AccountToken = this.AccountToken;

                    enhancedBINQuery.Application.ApplicationID = "10443";//PRIMEPOS-2796
                    enhancedBINQuery.Application.ApplicationName = ApplicationName;
                    enhancedBINQuery.Application.ApplicationVersion = Version.ToString();

                    enhancedBINQuery.Terminal.TerminalID = this.TerminalID;//PRIMEPOS-2769

                    if (!fields["TOKEN"].Contains("|"))
                    {
                        enhancedBINQuery.PaymentAccount.PaymentAccountID = fields["TOKEN"];
                    }
                    else
                    {
                        enhancedBINQuery.PaymentAccount.PaymentAccountID = fields["TOKEN"].Split('|')[0];
                    }

                    string request = XmlHelper.Serialize(enhancedBINQuery/*, xsn*/);

                    deviceResponse.deviceRequest = request;

                    logger.Trace("The request is :" + request.Replace(this.AcceptorID, "***").Replace(this.AccountID, "***").Replace(this.AccountToken, "***")); //PRIMEPOS-3156

                    string response = postXMLData(fields.ContainsKey("SALEURL") ? fields["SALEURL"] : "", request);

                    EnhancedBINQueryResponse enhancedBINQueryResponse = XmlHelper.Deserialize<EnhancedBINQueryResponse>(response);


                    if (enhancedBINQueryResponse != null && enhancedBINQueryResponse.Response?.EnhancedBIN?.HSAFSACard != null && enhancedBINQueryResponse.Response?.EnhancedBIN?.HSAFSACard == "Y")
                    {
                        ccTokenSale.ExtendedParameters = new ExtendedParameters();
                        ccTokenSale.ExtendedParameters.Healthcare = new HealthCare.Healthcare();
                        //Kept HardCoded as it doesn't change
                        ccTokenSale.ExtendedParameters.Healthcare.HealthcareFlag = 1;
                        ccTokenSale.ExtendedParameters.Healthcare.HealthcareFirstAmount = Convert.ToDouble(fields["IIASRXAMOUNT"]);
                        ccTokenSale.ExtendedParameters.Healthcare.HealthcareFlag = 1;
                        ccTokenSale.ExtendedParameters.Healthcare.HealthcareFirstAccountType = 0;
                        ccTokenSale.ExtendedParameters.Healthcare.HealthcareFirstAmountType = 2;
                        ccTokenSale.ExtendedParameters.Healthcare.HealthcareFirstCurrencyCode = 840;
                        ccTokenSale.ExtendedParameters.Healthcare.HealthcareFirstAmountSign = 0;
                        ccTokenSale.ExtendedParameters.Healthcare.HealthcareSecondAccountType = 0;
                        ccTokenSale.ExtendedParameters.Healthcare.HealthcareSecondAmountType = 7;
                        ccTokenSale.ExtendedParameters.Healthcare.HealthcareSecondCurrencyCode = 840;
                        ccTokenSale.ExtendedParameters.Healthcare.HealthcareSecondAmountSign = 0;
                        ccTokenSale.ExtendedParameters.Healthcare.HealthcareSecondAmount = Convert.ToDouble(fields["IIASRXAMOUNT"]);
                    }
                }
                else if (fields.ContainsKey("IIASTRANSACTION") && fields["IIASTRANSACTION"].ToString().ToLower() == "true")
                {
                    EnhancedBINQuery enhancedBINQuery = new EnhancedBINQuery();
                    enhancedBINQuery.Application = new Application();
                    enhancedBINQuery.Terminal = new Terminal();
                    enhancedBINQuery.PaymentAccount = new PaymentAccount();
                    enhancedBINQuery.Credentials = new Credentials();

                    enhancedBINQuery.Xmlns = "https://transaction.elementexpress.com";

                    enhancedBINQuery.Credentials.AcceptorID = this.AcceptorID;
                    enhancedBINQuery.Credentials.AccountID = this.AccountID;
                    enhancedBINQuery.Credentials.AccountToken = this.AccountToken;

                    enhancedBINQuery.Application.ApplicationID = "10443";//PRIMEPOS-2796
                    enhancedBINQuery.Application.ApplicationName = ApplicationName;
                    enhancedBINQuery.Application.ApplicationVersion = Version.ToString();

                    enhancedBINQuery.Terminal.TerminalID = this.TerminalID;//PRIMEPOS-2769

                    if (!fields["TOKEN"].Contains("|"))
                    {
                        enhancedBINQuery.PaymentAccount.PaymentAccountID = fields["TOKEN"];
                    }
                    else
                    {
                        enhancedBINQuery.PaymentAccount.PaymentAccountID = fields["TOKEN"].Split('|')[0];
                    }

                    string request = XmlHelper.Serialize(enhancedBINQuery/*, xsn*/);

                    deviceResponse.deviceRequest = request;

                    logger.Trace("The request is :" + request.Replace(this.AcceptorID, "***").Replace(this.AccountID, "***").Replace(this.AccountToken, "***")); //PRIMEPOS-3156

                    string response = postXMLData(fields.ContainsKey("SALEURL") ? fields["SALEURL"] : "", request);

                    EnhancedBINQueryResponse enhancedBINQueryResponse = XmlHelper.Deserialize<EnhancedBINQueryResponse>(response);


                    if (enhancedBINQueryResponse != null && enhancedBINQueryResponse.Response?.EnhancedBIN?.HSAFSACard != null && enhancedBINQueryResponse.Response?.EnhancedBIN?.HSAFSACard == "Y")
                    {
                        ccTokenSale.ExtendedParameters = new ExtendedParameters();
                        ccTokenSale.ExtendedParameters.Healthcare = new HealthCare.Healthcare();
                        ccTokenSale.ExtendedParameters.Healthcare.HealthcareFlag = 1;
                        ccTokenSale.ExtendedParameters.Healthcare.HealthcareFirstAmount = Convert.ToDouble(fields["IIASAUTHORIZEDAMOUNT"]);
                        ccTokenSale.ExtendedParameters.Healthcare.HealthcareFlag = 1;
                        ccTokenSale.ExtendedParameters.Healthcare.HealthcareFirstAccountType = 0;
                        ccTokenSale.ExtendedParameters.Healthcare.HealthcareFirstAmountType = 2;
                        ccTokenSale.ExtendedParameters.Healthcare.HealthcareFirstCurrencyCode = 840;
                        ccTokenSale.ExtendedParameters.Healthcare.HealthcareFirstAmountSign = 0;
                        ccTokenSale.ExtendedParameters.Healthcare.HealthcareSecondAccountType = 0;
                        ccTokenSale.ExtendedParameters.Healthcare.HealthcareSecondAmountType = 7;
                        ccTokenSale.ExtendedParameters.Healthcare.HealthcareSecondCurrencyCode = 840;
                        ccTokenSale.ExtendedParameters.Healthcare.HealthcareSecondAmountSign = 0;
                        ccTokenSale.ExtendedParameters.Healthcare.HealthcareSecondAmount = Convert.ToDouble(fields["IIASAUTHORIZEDAMOUNT"]);
                    }
                }

                if (!fields["TOKEN"].Contains("|"))
                {
                    ccTokenSale.Transaction.PaymentType = "4";
                    ccTokenSale.Transaction.SubmissionType = "1";
                    ccTokenSale.Transaction.NetworkTransactionID = null;
                    ccTokenSale.PaymentAccount.PaymentAccountID = fields["TOKEN"];
                }
                else
                {
                    ccTokenSale.Transaction.PaymentType = "4";
                    ccTokenSale.Transaction.SubmissionType = "2";
                    if (fields["TOKEN"].Split('|')[1].Length > 2)//PRIMEPOS-2902
                    {
                        ccTokenSale.Transaction.NetworkTransactionID = fields["TOKEN"].Split('|')[1];
                    }
                    ccTokenSale.PaymentAccount.PaymentAccountID = fields["TOKEN"].Split('|')[0];
                }

                //XmlSerializerNamespaces xsn = new XmlSerializerNamespaces();
                //xsn.Add("", "https://transaction.elementexpress.com");

                string req = XmlHelper.Serialize(ccTokenSale/*, xsn*/);

                deviceResponse.deviceRequest = req;

                logger.Trace("The request is :" + req.Replace(this.AcceptorID, "***").Replace(this.AccountID, "***").Replace(this.AccountToken, "***")); //PRIMEPOS-3156

                string res = postXMLData(fields.ContainsKey("SALEURL") ? fields["SALEURL"] : "", req);

                deviceResponse.ParseCreditCardResponse(res);

                //PRIMEPOS-2793
                if (deviceResponse.EmvReceipt != null)
                {
                    deviceResponse.EmvReceipt.LaneID = this.LaneID;
                    deviceResponse.EmvReceipt.TerminalID = this.TerminalID;
                }
                //

            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
                throw ex;
            }
            return deviceResponse;
        }
        public VantivResponse CreditCardReturn(Dictionary<String, String> fields)
        {
            VantivResponse deviceResponse = new VantivResponse();

            CreditCardReturn ccTokenReturn = new CreditCardReturn();
            try
            {
                ccTokenReturn.Credentials = new Credentials();
                ccTokenReturn.Application = new Application();
                ccTokenReturn.Transaction = new Transaction();
                ccTokenReturn.Terminal = new Terminal();
                ccTokenReturn.PaymentAccount = new PaymentAccount();

                ccTokenReturn.Xmlns = "https://transaction.elementexpress.com";
                ccTokenReturn.Credentials.AcceptorID = this.AcceptorID;
                ccTokenReturn.Credentials.AccountID = this.AccountID;
                ccTokenReturn.Credentials.AccountToken = this.AccountToken;

                ccTokenReturn.Application.ApplicationID = "1";
                ccTokenReturn.Application.ApplicationName = ApplicationName;
                ccTokenReturn.Application.ApplicationVersion = Version.ToString();

                ccTokenReturn.Transaction.TicketNumber = fields["TICKETNUMBER"];
                ccTokenReturn.Transaction.ReferenceNumber = fields["TICKETNUMBER"];
                ccTokenReturn.Transaction.MarketCode = "7";
                ccTokenReturn.Transaction.TransactionAmount = fields["AMOUNT"];
                ccTokenReturn.Transaction.TransactionID = fields["TRANSACTIONID"];

                ccTokenReturn.Terminal.TerminalID = "123";
                ccTokenReturn.Terminal.CardPresentCode = "2";
                ccTokenReturn.Terminal.CardholderPresentCode = "2";
                ccTokenReturn.Terminal.CardInputCode = "4";
                ccTokenReturn.Terminal.CVVPresenceCode = "1";
                ccTokenReturn.Terminal.TerminalCapabilityCode = "5";
                ccTokenReturn.Terminal.TerminalEnvironmentCode = "6";
                ccTokenReturn.Terminal.MotoECICode = "2";

                if (!fields["TOKEN"].Contains("|"))
                {
                    ccTokenReturn.Transaction.PaymentType = "4";
                    ccTokenReturn.Transaction.SubmissionType = "1";
                    ccTokenReturn.Transaction.NetworkTransactionID = null;
                    ccTokenReturn.PaymentAccount.PaymentAccountID = fields["TOKEN"];
                }
                else
                {
                    ccTokenReturn.Transaction.PaymentType = "4";
                    ccTokenReturn.Transaction.SubmissionType = "2";
                    ccTokenReturn.Transaction.NetworkTransactionID = fields["TOKEN"].Split('|')[1];
                    ccTokenReturn.PaymentAccount.PaymentAccountID = fields["TOKEN"].Split('|')[0];
                }

                //XmlSerializerNamespaces xsn = new XmlSerializerNamespaces();
                //xsn.Add("", "https://transaction.elementexpress.com");

                string req = XmlHelper.Serialize(ccTokenReturn);

                deviceResponse.deviceRequest = req;

                logger.Trace("The request is :" + req.Replace(this.AcceptorID, "***").Replace(this.AccountID, "***").Replace(this.AccountToken, "***")); //PRIMEPOS-3156

                string res = postXMLData(fields.ContainsKey("SALEURL") ? fields["SALEURL"] : "", req); //PRIMEPOS-3156

                deviceResponse.ParseCreditCardResponse(res);

                //PRIMEPOS-2793
                if (deviceResponse.EmvReceipt != null)
                {
                    deviceResponse.EmvReceipt.LaneID = this.LaneID;
                    deviceResponse.EmvReceipt.TerminalID = this.TerminalID;
                }
                //
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
                throw ex;
            }
            return deviceResponse;
        }
        /// <summary>
        /// This Method is used for doing a strict return 
        /// </summary>
        /// <param name="fields"></param>
        /// <returns></returns>
        public VantivResponse StrictReturn(Dictionary<String, String> fields)
        {
            VantivResponse deviceResponse = new VantivResponse();
            try
            {
                string postBody = string.Empty;
                string transactionType = string.Empty;

                if (fields.ContainsKey("RETURNTRANSTYPE")) //PRIEMPOS-3504
                {
                    if (fields["RETURNTRANSTYPE"].ToString().ToUpper().Contains("CREDIT"))
                    {
                        transactionType = "credit";
                        this.Url += Constant.Return + "/" + fields["TRANSACTIONID"] + "/" + transactionType;
                    }
                    else if(fields["RETURNTRANSTYPE"].ToString().ToUpper().Contains("DEBIT"))
                    {
                        transactionType = "debit";
                        this.Url += Constant.Return + "/" + fields["TRANSACTIONID"] + "/" + transactionType;
                    }
                }
                else
                {
                    if (fields["TRANSACTIONTYPE"].ToString().ToUpper().Contains("CREDIT"))
                    {
                        transactionType = "credit";
                        this.Url += Constant.Return + "/" + fields["TRANSACTIONID"] + "/" + transactionType;
                    }
                    else if (fields["TRANSACTIONTYPE"].ToString().ToUpper().Contains("DEBIT"))
                    {
                        transactionType = "debit";
                        this.Url += Constant.Return + "/" + fields["TRANSACTIONID"] + "/" + transactionType;
                    }
                    else
                    {
                        transactionType = "ebt";
                        this.Url += Constant.Reversal + "/" + fields["TRANSACTIONID"] + "/" + transactionType;
                    }
                }

                StrictReturn strictReturn = new RequestModels.StrictReturn();
                strictReturn.laneId = this.LaneID;
                if (fields["TRANSACTIONTYPE"].ToString().ToUpper().Contains("EBT"))
                {
                    strictReturn.ebtType = "FoodStamp";
                }
                strictReturn.ticketNumber = fields["TICKETNUMBER"];
                if (fields["AMOUNT"].Contains("-")) //PRIMEPOS-3440
                    fields["AMOUNT"] = fields["AMOUNT"].Remove(0, 1);
                strictReturn.transactionAmount = fields["AMOUNT"];
                strictReturn.configuration = new Configuration();
                if (fields.ContainsKey("ISNBSTRANSACTION") && Convert.ToBoolean(fields["ISNBSTRANSACTION"]))
                {
                    strictReturn.configuration.PromptForSignature = Constant.Never;
                }
                //if (Convert.ToBoolean(fields["ALLOWDUP"]) == true)
                //    strictReturn.configuration.CheckForDuplicateTransactions = true;
                //else
                //    strictReturn.configuration.CheckForDuplicateTransactions = false;
                strictReturn.clerkNumber = fields["TICKETNUMBER"];
                strictReturn.referenceNumber = fields["TICKETNUMBER"];//PRIMEPOS-2795 ADDED BY ARVIND VANTIV

                postBody = JsonConvert.SerializeObject(strictReturn);

                //For Sending it in HttpClient
                string actualResponse = Constant.SendRequestPost(postBody, this.Url, this.developerKey, this.developerSecret, this.ApplicationName);


                deviceResponse.ParseReturnResponse(actualResponse);

                deviceResponse.deviceRequest = postBody;
                deviceResponse.deviceResponse = actualResponse;

                //PRIMEPOS-2793
                if (deviceResponse.EmvReceipt != null)
                {
                    deviceResponse.EmvReceipt.LaneID = this.LaneID;
                    deviceResponse.EmvReceipt.TerminalID = this.TerminalID;
                }
                //

                this.Url = this.Url.Remove(28);

            }
            catch (Exception ex)
            {
                logger.Error(" ERROR IN StrictReturn METHOD : ", ex.ToString());
                this.Url = this.Url.Remove(28);
                throw ex;
            }
            return deviceResponse;
        }
        /// <summary>
        /// This is the Reversal Method for Credit and Debit Transaction
        /// </summary>
        /// <param name="fields"></param>
        /// <returns></returns>
        public VantivResponse Reversal(Dictionary<String, String> fields)
        {
            logger.Trace(" ENTERED IN Reversal METHOD ");

            string actualResponse = string.Empty;
            string postBody = string.Empty;
            this.Url += Constant.Reversal;

            //Intitializing the Response class
            VantivResponse deviceResponse = new VantivResponse();
            try
            {
                string transactionType = string.Empty;
                if (fields["TRANSACTIONTYPE"].ToString().ToUpper().Contains("CREDIT"))
                {
                    transactionType = "credit";
                }
                else if (fields["TRANSACTIONTYPE"].ToString().ToUpper().Contains("DEBIT") )
                {
                    transactionType = "debit";
                }
                else if(fields["TRANSACTIONTYPE"].ToString().ToUpper().Contains("VANTIV_NBS_VOID"))
                {
                    if (fields["NBSSALETYPE"].ToString().ToUpper().Contains("CREDIT"))
                    {
                        transactionType = "credit"; 
                    }
                    else if(fields["NBSSALETYPE"].ToString().ToUpper().Contains("DEBIT"))
                    {
                        transactionType = "debit";
                    }
                }
                else
                {
                    transactionType = "ebt";
                }
                this.Url += "/" + fields["TRANSACTIONID"] + "/" + transactionType;

                ReversalRequest reversalRequest = new ReversalRequest();
                reversalRequest.configuration = new Configuration();
                reversalRequest.ticketNumber = fields["TICKETNUMBER"];
                reversalRequest.laneId = this.LaneID;
                reversalRequest.referenceNumber = fields["TICKETNUMBER"];//PRIMEPOS-2795 ADDED BY ARVIND

                #region PRIMEPOS-2795
                if (fields.ContainsKey("AMOUNT"))
                {
                    if (!fields["AMOUNT"].Contains("-"))
                        reversalRequest.transactionAmount = fields["AMOUNT"];
                    else
                    {
                        fields["AMOUNT"] = fields["AMOUNT"].Remove(0, 1);
                        reversalRequest.transactionAmount = fields["AMOUNT"];
                    }
                }
                #endregion

                postBody = JsonConvert.SerializeObject(reversalRequest);

                actualResponse = Constant.SendRequestPost(postBody, this.Url, this.developerKey, this.developerSecret, this.ApplicationName);

                deviceResponse.ParseReversalResponse(actualResponse);

                deviceResponse.deviceRequest = postBody;//Saving it for CC_Transmission_Log
                deviceResponse.deviceResponse = actualResponse;//Saving it for CC_Transmission_Log

                //PRIMEPOS-2793
                if (deviceResponse.EmvReceipt != null)
                {
                    deviceResponse.EmvReceipt.LaneID = this.LaneID;
                    deviceResponse.EmvReceipt.TerminalID = this.TerminalID;
                }
                //

                this.Url = this.Url.Remove(28);
            }
            catch (Exception ex)
            {
                logger.Error(" ERROR IN Reversal METHOD : ", ex.ToString());
                this.Url = this.Url.Remove(28);
                throw ex;
            }
            return deviceResponse;
        }
        #region PRIMEPOS-2769
        public VantivResponse CreditCardReversal(Dictionary<String, String> fields)
        {
            VantivResponse deviceResponse = new VantivResponse();

            CreditCardReversal ccTokenReversal = new CreditCardReversal();
            try
            {
                ccTokenReversal.Credentials = new Credentials();
                ccTokenReversal.Application = new Application();
                ccTokenReversal.Transaction = new Transaction();
                ccTokenReversal.Terminal = new Terminal();
                ccTokenReversal.PaymentAccount = new PaymentAccount();

                ccTokenReversal.Xmlns = "https://transaction.elementexpress.com";
                ccTokenReversal.Credentials.AcceptorID = this.AcceptorID;
                ccTokenReversal.Credentials.AccountID = this.AccountID;
                ccTokenReversal.Credentials.AccountToken = this.AccountToken;

                ccTokenReversal.Application.ApplicationID = "10443";//PRIMEPOS-2796
                ccTokenReversal.Application.ApplicationName = ApplicationName;
                ccTokenReversal.Application.ApplicationVersion = Version.ToString();

                ccTokenReversal.Transaction.TicketNumber = fields["TICKETNUMBER"];
                ccTokenReversal.Transaction.ReferenceNumber = fields["TICKETNUMBER"];//PRIMEPOS-2795 
                ccTokenReversal.Transaction.MarketCode = "2";
                ccTokenReversal.Transaction.TransactionAmount = fields["AMOUNT"];
                ccTokenReversal.Transaction.ReversalType = "0";//PRIMEPOS-2769

                ccTokenReversal.Terminal.TerminalID = this.TerminalID;//PRIMEPOS-2769
                ccTokenReversal.Terminal.TerminalType = "3";//PRIMEPOS-2769
                ccTokenReversal.Terminal.CardPresentCode = "3";//PRIMEPOS-2769
                ccTokenReversal.Terminal.CardholderPresentCode = "3";//PRIMEPOS-2769
                ccTokenReversal.Terminal.CardInputCode = "4";
                ccTokenReversal.Terminal.CVVPresenceCode = "1";
                ccTokenReversal.Terminal.TerminalCapabilityCode = "5";
                ccTokenReversal.Terminal.TerminalEnvironmentCode = "2";//PRIMEPOS-2769
                ccTokenReversal.Terminal.MotoECICode = "2";

                if (!fields["TOKEN"].Contains("|"))
                {
                    ccTokenReversal.Transaction.PaymentType = "4";
                    ccTokenReversal.Transaction.SubmissionType = "1";
                    ccTokenReversal.Transaction.NetworkTransactionID = null;
                    ccTokenReversal.PaymentAccount.PaymentAccountID = fields["TOKEN"];
                }
                else
                {
                    ccTokenReversal.Transaction.PaymentType = "4";
                    ccTokenReversal.Transaction.SubmissionType = "2";
                    ccTokenReversal.Transaction.NetworkTransactionID = fields["TOKEN"].Split('|')[1];
                    ccTokenReversal.PaymentAccount.PaymentAccountID = fields["TOKEN"].Split('|')[0];
                }

                //XmlSerializerNamespaces xsn = new XmlSerializerNamespaces();
                //xsn.Add("", "https://transaction.elementexpress.com");

                string req = XmlHelper.Serialize(ccTokenReversal/*, xsn*/);

                deviceResponse.deviceRequest = req;

                logger.Trace("The request is :" + req.Replace(this.AcceptorID, "***").Replace(this.AccountID, "***").Replace(this.AccountToken, "***"));//PRIMEPOS-3156

                string res = postXMLData(fields.ContainsKey("SALEURL") ? fields["SALEURL"] : "", req); //PRIMEPOS-3156

                deviceResponse.ParseCreditCardReversalResponse(res);

                deviceResponse.deviceResponse = res;

                //PRIMEPOS-2793
                if (deviceResponse.EmvReceipt != null)
                    deviceResponse.EmvReceipt.TerminalID = this.TerminalID;
                //
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
                throw ex;
            }
            return deviceResponse;
        }
        #endregion
        //PRIMEPOS-2867 Consent
        public Tuple<string, string> PatientConsent(Hashtable hashtable, string DelayInSecond)
        {
            logger.Trace(" ENTERED IN PatientConsent METHOD ");

            string actualResponse = string.Empty;
            this.Url += Constant.Selection;
            this.Url += "/" + this.LaneID;

            //For initializing the Response class 
            VantivResponse deviceResponse = new VantivResponse();
            string firstResponse = string.Empty;
            string secondResponse = string.Empty;
            try
            {
                string title = (string)hashtable["TITLE"];
                string text = (string)hashtable["TEXT"];
                string patientName = (string)hashtable["PATIENTNAME"];
                string patientAddress = (string)hashtable["PATIENTADDRESS"];
                string firstradiobtn = (string)hashtable["FIRSTRDBTN"];
                string secondradiobtn = (string)hashtable["SECONDRDBTN"];
                string thirdradiobtn = (string)hashtable["THIRDRDBTN"];
                string fourthradiobtn = (string)hashtable["FOURTHRDBTN"];//PRIMEPOS-3192
                string secondbtn = (string)hashtable["SECONDBTN"];
                string thirdbtn = (string)hashtable["THIRDBTN"];


                var uriBuilder = new UriBuilder(Url);
                var query = System.Web.HttpUtility.ParseQueryString(uriBuilder.Query);
                //string a = "?form = MultiOptionTextArea & header = Header & subHeader = Subheader & text = detailed % 20text % 20line % 201 % 0Adetailed % 20text % 20line % 202 & options = one | two | three";

                query["form"] = "MultiOptionTextArea";
                query["header"] = title;
                #region PRIMEPOS-3192
                //query["text"] = "1. " + firstradiobtn + "\n" + "2. " + secondradiobtn + "\n" + "3. " +
                //   thirdradiobtn;
                //query["options"] = "Option1" + "|" + "Option2" + "|" + "Option3";

                if (string.IsNullOrEmpty(thirdradiobtn) && string.IsNullOrEmpty(fourthradiobtn))
                {
                    query["text"] = "1. " + firstradiobtn + "\n" + "2. " + secondradiobtn; 
                    query["options"] = "Option1" + "|" + "Option2";
                }
                else if (!string.IsNullOrEmpty(thirdradiobtn) && string.IsNullOrEmpty(fourthradiobtn))
                {
                    query["text"] = "1. " + firstradiobtn + "\n" + "2. " + secondradiobtn + "\n" + "3. " + thirdradiobtn; 
                    query["options"] = "Option1" + "|" + "Option2" + "|" + "Option3"; 
                }
                else
                {
                    query["text"] = "1. " + firstradiobtn + "\n" + "2. " + secondradiobtn + "\n" + "3. " + thirdradiobtn + "\n" + "4. " + fourthradiobtn; 
                    query["options"] = "Option1" + "|" + "Option2" + "|" + "Option3" + "|" + "Option4";
                }

                #endregion
                uriBuilder.Query = query.ToString();
                Url = uriBuilder.ToString();

                try
                {
                    int Second = Convert.ToInt32(DelayInSecond) * 1000;
                    Thread.Sleep(Second);//Arvind for Signature issue API
                }
                catch (Exception ex)
                {
                    logger.Error("Error in DelaySecond" + ex);
                    Thread.Sleep(2000);
                }
                //For Sending it in HttpClient
                actualResponse = Constant.SendRequestGet(this.Url, this.developerKey, this.developerSecret);

                var btnResponse = JsonConvert.DeserializeObject<SelectionResponse>(actualResponse);
                secondResponse = deviceResponse.buttonNumber = btnResponse.selectionIndex.ToString();

                uriBuilder = new UriBuilder(Url);
                query = System.Web.HttpUtility.ParseQueryString(uriBuilder.Query);
                //string a = "?form = MultiOptionTextArea & header = Header & subHeader = Subheader & text = detailed % 20text % 20line % 201 % 0Adetailed % 20text % 20line % 202 & options = one | two | three";

                query["form"] = "MultiOptionTextArea";
                query["header"] = title;
                query["text"] = "Patient : " + patientName + "\n" + "Address : " + patientAddress + "\n" + text;
                query["options"] = secondbtn + "|" + thirdbtn; //+ "|" + "SKIP"; PRIMEPOS-3192
                uriBuilder.Query = query.ToString();
                Url = uriBuilder.ToString();

                try
                {
                    int Second = Convert.ToInt32(DelayInSecond) * 1000;
                    Thread.Sleep(Second);//Arvind for Signature issue API
                }
                catch (Exception ex)
                {
                    logger.Error("Error in DelaySecond" + ex);
                    Thread.Sleep(2000);
                }
                //For Sending it in HttpClient
                actualResponse = Constant.SendRequestGet(this.Url, this.developerKey, this.developerSecret);

                var selectionResponse = JsonConvert.DeserializeObject<SelectionResponse>(actualResponse);
                firstResponse = deviceResponse.buttonNumber = selectionResponse.selectionIndex.ToString();

                this.Url = this.Url.Remove(28);
            }
            catch (Exception ex)
            {
                logger.Error(" ERROR IN PatientConsent METHOD : ", ex.ToString());
                this.Url = this.Url.Remove(28);
                throw ex;
            }

            return Tuple.Create(firstResponse, secondResponse);
        }

        #region PRIMEPOS-3156

        public VantivResponse LocalDetailsReport(string ticketNumber, Dictionary<String, String> fields)
        {
            try
            {
                VantivResponse deviceResponse = new VantivResponse();
                //string xmlns = string.Empty;
                TransactionQuery transactionQuery = new TransactionQuery();


                transactionQuery.Xmlns = "https://reporting.elementexpress.com";
                transactionQuery.Credentials = new Credentials();
                transactionQuery.Application = new Application();
                transactionQuery.Parameters = new Parameters();
                //transactionQuery.Terminal = new Terminal();

                //transactionQuery.Terminal.TerminalID = this.TerminalID;

                transactionQuery.Credentials.AcceptorID = this.AcceptorID;
                transactionQuery.Credentials.AccountID = this.AccountID;
                transactionQuery.Credentials.AccountToken = this.AccountToken;

                transactionQuery.Application.ApplicationID = "10443";
                transactionQuery.Application.ApplicationName = ApplicationName;
                transactionQuery.Application.ApplicationVersion = Version.ToString();
                transactionQuery.Parameters.ReferenceNumber = ticketNumber;

                string req = XmlHelper.Serialize(transactionQuery);

                deviceResponse.deviceRequest = req;

                logger.Trace("The request is :" + req.Replace(this.AcceptorID, "***").Replace(this.AccountID, "***").Replace(this.AccountToken, "***")); //PRIMEPOS-3156

                string res = postXMLData(fields.ContainsKey("REPORTURL") ? fields["REPORTURL"] : "", req); //PRIMEPOS-3156
                logger.Trace($"LocalDetailsReport Device response : {res}");
                deviceResponse.ParseLocalDetailReportResponse(res);

                return deviceResponse;
            }
            catch (Exception ex)
            {
                logger.Error("Error in LocalDetailsReport : " + ex.ToString());
                throw ex;
            }
        }
        #endregion PRIMEPOS-3156 till here
    }
}
