using Elavon;
using Elavon.Response;
using MMS.Device.Global;
using NLog;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;

namespace MMS.Device.Elavon
{
    //PRIMEPOS-2636
    public class Elavon8000
    {

        ILogger logger = LogManager.GetCurrentClassLogger();
        #region Variable and Properties
        ElavonProcessor device = null;
        private static Elavon8000 dDefObj = null;
        static Thread DeviceThread = null;
        private readonly object DeviceLock = new object();
        delegate void CommandHandler();
        event CommandHandler CommandEvent;
        Hashtable PData;
        delegate void VANTIVErrorEventLog();
        static event VANTIVErrorEventLog ErrorLog;
        private List<Order> deviceData = null;
        Queue<CommandStation> CommandQueue = new Queue<CommandStation>();
        private ElavonResponse deviceResponse;
        CultureInfo CI = CultureInfo.CurrentCulture;
        private List<Order> deviceDataTemp = null;

        public const string BTN_YES = "1";
        public const string BTN_NO = "0";

        // - Handling Thread Exception
        public object exceptionObj = new object();
        public bool isException = false;
        public string HIPPAAckResultId
        {
            get;
            set;
        }

        public string PatCounsel
        {
            get;
            set;
        }

        public string GetSignatureString
        {
            get;
            set;
        }

        public bool? IsSignValid
        {
            get;
            set;
        }

        public bool? IsResourceUpdated
        {
            get;
            set;
        }

        public PatientConsent PatConsent
        {
            get;
            set;
        }
        public DataSet oDataSet
        {
            get; set;
        }
        private DataTable RXList = null;
        private bool bSkipConsent = false;
        private PatientConsent tmpConsent = null;
        private int PattypeSelection = -1;
        public bool? shouldMoveNext = null;
        private int ConsentApplies = -1;
        private int ConsentStatus = -1;



        #endregion
        public static Elavon8000 DefaultInstance(string _posSettings)
        {

            if (dDefObj == null)
            {
                dDefObj = new Elavon8000(_posSettings);
                dDefObj.deviceData = new List<Order>();
            }
            return dDefObj;
        }

        private Elavon8000(string _posSettings)
        {
            try
            {
                string hostAddress = _posSettings.Split(':')[0];
                string hostPort = _posSettings.Split(':')[1].Split('/')[0];

                device = ElavonProcessor.getInstance(hostAddress, Convert.ToInt32(hostPort));
            }
            catch (Exception ex)
            {

            }
        }
        #region PRIMEPOS-3260
        public static void HeartbeatMessage(string _posSettings)
        {
            ElavonProcessor elavonProcessor = ElavonProcessor.getInstance("", 0);

            elavonProcessor.HealthMessage();
        }
        #endregion


        #region Device Public method to send commands        
        //Added by Arvind 
        public bool IsStillWrite
        {
            get
            {
                return Constant.IsStillWrite;
            }
        }
        public bool CaptureRxSignature()
        {
            Hashtable fields = new Hashtable();
            fields.Add("COMMAND", "CAPTURESIGNATURE");
            DeviceCommand("CAPTURESIGNATURE", fields);
            return true;
        }

        public void DisplayScreen(string sCurrentTransID, string screen, string strData)
        {

            Hashtable customMessage = new Hashtable();
            customMessage.Add("COMMAND", screen /*Show RX INFO Remaining*/);
            customMessage.Add("TITLE", "Message");
            customMessage.Add("MESSAGE", strData);
            DeviceCommand("SHOWMESSAGE", customMessage);
        }
        public void ResendAllItem()
        {
            Hashtable fields = new Hashtable();
            fields.Add("COMMAND", "RELOADITEMLIST");
            fields.Add("ACTION", "R");
            DeviceCommand("SHOWMESSAGE", fields);
        }
        public void SendOnHoldItems(DataTable dt, string ShowRxDesc)
        {
            logger.Debug(string.Format("In DisplayOnHoldItem()"));
            bool isDisplayOnHold = false;
            ArrayList OnHoldItem = new ArrayList();
            Hashtable data = new Hashtable();
            try
            {
                logger.Debug("Entering DisplayOnHoldItem()");
                if (dt != null && dt.Rows.Count > 0)
                {
                    if (Constant.dataStore.Count >= 0)
                    {
                        Constant.dataStore.Clear();
                    }
                    else
                    {
                        Constant.dataStore = new ArrayList();
                    }

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow dr = dt.Rows[i]; //rows
                        Hashtable fields = new Hashtable();
                        fields.Add("ItemName", dr["ItemDescription"].ToString());
                        fields.Add("ItemQty", dr["QTY"].ToString());
                        fields.Add("UnitPrice", Convert.ToDouble(dr["Price"].ToString()).ToString("F", CI).PadRight(2, '0'));
                        fields.Add("TotalPrice", Convert.ToDouble(dr["ExtendedPrice"].ToString()).ToString("F", CI).PadRight(2, '0'));
                        fields.Add("ISRX", dr["ItemID"].ToString().ToUpper() == "RX" ? 1 : 0);
                        fields.Add("TAX", Convert.ToDouble(dr["TaxAmount"].ToString()).ToString("F", CI).PadRight(2, '0'));
                        fields.Add("SHOWRXDESCRIPTION", ShowRxDesc);
                        fields.Add("DISCOUNT", Convert.ToDouble(dr["Discount"].ToString()).ToString("F", CI).PadRight(2, '0'));
                        OnHoldItem.Add(fields);
                        Constant.dataStore.Add(fields);

                    }
                    data.Add("OnHoldItems", OnHoldItem); //add the arraylist to the Hashtable for passing to pad
                    data.Add("COMMAND", "HOLDITEMS");
                    DeviceCommand("SHOWMESSAGE", data);
                }
                logger.Debug("Exiting DisplayOnHoldItem(): " + isDisplayOnHold);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Error In Comm DisplayOnHoldItem(): \n" + ex.Message);
                throw new Exception(ex.ToString());
            }

        }
        #region PRIMEPOS-2730 - NileshJ
        public void ClearDeviceQueue()
        {
            lock (CommandQueue)
            {
                CommandQueue.Clear();
                //deviceData.Clear();
            }
        }

        public bool isDevQueueEmpty()
        {
            bool isQueueEmpty = false;
            lock (CommandQueue)
            {
                if (CommandQueue.Count == 0)
                {
                    isQueueEmpty = true;
                }
            }
            return isQueueEmpty;
        }
        #endregion
        public void AddItem(Hashtable fields)
        {

            fields.Add("ACTION", "A");
            fields.Add("COMMAND", "SHOWRXLIST");
            DeviceCommand("SHOWMESSAGE", fields);
        }
        public void DeleteItem(Hashtable fields)
        {

            fields.Add("ACTION", "D");
            fields.Add("COMMAND", "DELETEITEM");
            DeviceCommand("SHOWMESSAGE", fields);
        }
        public void UpdateItem(Hashtable fields)
        {

            fields.Add("ACTION", "U");
            fields.Add("COMMAND", "SHOWRXLIST");
            DeviceCommand("SHOWMESSAGE", fields);
        }
        public bool CaptureRXSig()
        {
            Hashtable RXSignatureData = new Hashtable();
            DeviceCommand("DOSIGNATURE", null);
            return true;
        }
        public void ClearItems()
        {
            deviceData.Clear();
            DeviceCommand("CLEARMESSAGE", new Hashtable());
        }
        public void ResetToIdle()
        {
            Hashtable RXSignatureData = new Hashtable();
            DeviceCommand("CLEARMESSAGE", null);
        }
        public void EndTxn(string amount)
        {
            logger.Debug("In EndTxn()");
            int rtCode = 1;
            try
            {
                logger.Debug("Entering Comm EndTxn ");
                if (Constant.CCDataSwipe == null)
                {
                    Constant.CCDataSwipe = new ArrayList();
                }
                else
                {
                    Constant.CCDataSwipe.Clear();
                }

                //Add rx list 
                //rxListTable = null;
                Hashtable commandData = new Hashtable();
                commandData.Add("AMT", amount);
                DeviceCommand("SHOWTHANKYOU", commandData);
                deviceData.Clear();
                Constant.dataStore = null;
                rtCode = 0;
                logger.Debug("Exiting Comm EndTxn");
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Error in Comm EndTxn: \n" + ex.Message);
            }
        }
        private bool returnValue = false;//PRIMEPOS-2867 
        public void CapturePatConsel(string sCurrentTransID, string totalRx, string patientName, string patientCounceling, DataTable rxList)
        {
            //reset to null for newer button click id

            try
            {
                if (RXList != null && RXList.Rows.Count > 0)
                {
                    RXList.Clear();
                }
                else if (RXList == null)
                {
                    RXList = new DataTable();
                }

                RXList = rxList.Copy();//addrx() line 801
                logger.Debug("In Comm RxSign Total Rx's: " + RXList.Rows.Count);
                Constant.Counsel = patientCounceling; //orignial Patient counseling from POS

                //Append RXinfo To Device Message
                string RXData = prepareRXData();

                Hashtable RXPatConsel = new Hashtable();
                RXPatConsel.Add("COMMAND", "RXHEADERINFO" /*Show RX INFO Remaining*/);
                RXPatConsel.Add("HEADER", " Rx Pickup Acknowledgement");
                RXPatConsel.Add("PATIENTNAME", patientName);
                RXPatConsel.Add("RXCOUNT", totalRx);
                RXPatConsel.Add("RXDATA", RXData);
                DeviceCommand("SHOWTEXTBOX", RXPatConsel);
                //PatCounsel = "Y";

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private string prepareRXData()
        {
            string RXData = "";
            for (int i = 0; i < RXList.Rows.Count; i++)
            {
                RXInfo drugInfo = new RXInfo();

                drugInfo.RXNo = string.IsNullOrEmpty(RXList.Rows[i]["RXNO"].ToString().Trim()) ? "000000" : RXList.Rows[i]["RXNO"].ToString();
                logger.Debug("In Comm RxSign: " + (string.IsNullOrEmpty(RXList.Rows[i]["RXNO"].ToString().Trim()) ? "000000" : RXList.Rows[i]["RXNO"].ToString()));

                drugInfo.drugName = string.Empty;
                if (Convert.ToBoolean(RXList.Rows[i]["SHOWRXDESCRIPTION"].ToString()) == false)
                {
                    drugInfo.drugName = "****";
                }
                else
                {
                    drugInfo.drugName = string.IsNullOrEmpty(RXList.Rows[i]["DRUGNAME"].ToString().Trim()) ? "Not Available" : RXList.Rows[i]["DRUGNAME"].ToString();
                }
                drugInfo.date = string.IsNullOrEmpty(RXList.Rows[i]["RXDATE"].ToString().Trim()) ? "00000000" : RXList.Rows[i]["RXDATE"].ToString();

                //Append Drug to Final RXData
                if (RXData.Length > 0)
                    RXData += Environment.NewLine + drugInfo.ToString();
                else
                    RXData += drugInfo.ToString();
            }
            return RXData;
        }
        public void CaptureNOPP(string sCurrentTransID, string sPatientName, string sPatientAddress, string privacyText)
        {
            //reset to null for newer button click id

            HIPPAAckResultId = null;
            Hashtable NOPPCommandData = new Hashtable();
            NOPPCommandData.Add("COMMAND", "HIPPAACK");
            NOPPCommandData.Add("HEADER", " HIPAA Acknowledgement"); //PRIMEPOS-3212
            NOPPCommandData.Add("PATIENTNAME", sPatientName);
            NOPPCommandData.Add("PATIENTADDRESS", sPatientAddress);
            NOPPCommandData.Add("TEXT", privacyText);
            DeviceCommand("SHOWTEXTBOX", NOPPCommandData);
        }

        public bool CapturePatConsent(string patConsentProvider, string pharmacy, DataSet ds)
        {
            //Hashtable resourceFileParam = new Hashtable();
            //resourceFileParam.Add("FILE", file);
            //DeviceCommand("UPDATERESOURCEFILE", resourceFileParam);



            logger.Debug("In PerformConsentCaptureforHealthix");
            tmpConsent = new PatientConsent();
            string PharmacyName = pharmacy;

            if (patConsentProvider != Constants.CONSENT_SOURCE_HEALTHIX)
            {
                Hashtable hashtable = new Hashtable();
                if (ds?.Tables[0]?.Rows?.Count > 0)
                {
                    oDataSet = ds;
                    hashtable.Add("TITLE", ds.Tables["ConsentTextVersion"].Rows[0]["ConsentTextTitle"]);
                    hashtable.Add("TEXT", ds.Tables["ConsentTextVersion"].Rows[0]["ConsentText"]);
                    hashtable.Add("PATIENTNAME", ds.Tables["PATIENT"].Rows[0]["LNAME"] + " " + ds.Tables["PATIENT"].Rows[0]["FNAME"]);
                    hashtable.Add("PATIENTADDRESS", ds.Tables["PATIENT"].Rows[0]["ADDRSTR"].ToString() + " "
                        + ds.Tables["PATIENT"].Rows[0]["ADDRCT"].ToString() + " " +
                        ds.Tables["PATIENT"].Rows[0]["ADDRST"].ToString() + " " +
                        ds.Tables["PATIENT"].Rows[0]["ADDRZP"].ToString());

                    if (ds.Tables["Consent_Relationship"].Rows.Count > 0)
                        hashtable.Add("FIRSTRDBTN", ds.Tables["Consent_Relationship"].Rows[0]["Relation"]);
                    if (ds.Tables["Consent_Relationship"].Rows.Count > 1)
                        hashtable.Add("SECONDRDBTN", ds.Tables["Consent_Relationship"].Rows[1]["Relation"]);
                    if (ds.Tables["Consent_Relationship"].Rows.Count > 2)
                        hashtable.Add("THIRDRDBTN", ds.Tables["Consent_Relationship"].Rows[2]["Relation"]);
                    if (ds.Tables["Consent_Relationship"].Rows.Count > 3)
                        hashtable.Add("FOURTHRDBTN", ds.Tables["Consent_Relationship"].Rows[3]["Relation"]);

                    if (ds.Tables["Consent_Status"].Rows.Count > 0)
                        hashtable.Add("SECONDBTN", ds.Tables["Consent_Status"].Rows[0]["Name"]);
                    if (ds.Tables["Consent_Status"].Rows.Count > 1)
                        hashtable.Add("THIRDBTN", ds.Tables["Consent_Status"].Rows[1]["Name"]);

                    DeviceCommand("PATIENTCONSENT", hashtable);
                }
            }
            else
            {
                GetPatientType(Constants.CONSENT_SOURCE_HEALTHIX);

                if (!bSkipConsent)
                {
                    ShowHealtixForm1();
                }
                if (!bSkipConsent)
                {
                    ShowHealtixForm2(PharmacyName);
                }
                if (!bSkipConsent)
                {
                    ShowHealtixForm3();
                }

                PatConsent = tmpConsent;
                tmpConsent = null;
                returnValue = true;
            }
            return returnValue;
        }


        private void GetPatientType(string Consentsource)
        {
            logger.Debug("In GetPatientType()=> Getting Patient Type  ");
            bSkipConsent = false;
            PattypeSelection = -1;
            Hashtable PatientPickingUpRX = new Hashtable();


            PatientPickingUpRX.Add("Header", "Please select the person picking up the Prescription");
            PatientPickingUpRX.Add("TEXT1", "Patient");
            PatientPickingUpRX.Add("TEXT2", "Legal Representative");
            PatientPickingUpRX.Add("TEXT3", "Both");
            PatientPickingUpRX.Add("TEXT4", "Other");
            PatientPickingUpRX.Add("LABEL1", "Patient");
            PatientPickingUpRX.Add("LABEL2", "Legal Representative");
            PatientPickingUpRX.Add("LABEL3", "Both");
            PatientPickingUpRX.Add("LABEL4", "Other");
            PatientPickingUpRX.Add("BUTTONTYPE", 1);
            PatientPickingUpRX.Add("COMMAND", "PATCONSENTSELECTPAT");
            DeviceCommand("SHOWDIALOGFORM", PatientPickingUpRX);


            while (PattypeSelection == -1)
            {
                Thread.Sleep(100);
            }

            if (PattypeSelection >= 0 && PattypeSelection < 3)
            {
                tmpConsent.ConsentSourceName = Consentsource;
                tmpConsent.PatConsentRelationShipDescription = PatientConsent.GetRelationshipCodeForHealthix(PattypeSelection);
            }
            else
            {
                bSkipConsent = true;
                tmpConsent.IsConsentSkip = true;
            }
        }

        private void ShowHealtixForm1()
        {
            logger.Debug("In ShowHelthixForm1()=>  ");

            Hashtable _HEALTHIXFRM1 = new Hashtable();
            _HEALTHIXFRM1.Add("COMMAND", "HEALTHIXFRM1");

            /*_HEALTHIXFRM1.Add("HEADER", "Authorization for Release of Patient Information");15/03/2022
            _HEALTHIXFRM1.Add("TEXT", "I request that health information regarding my care and treatment be accessed as "
                + "set forth on this form. I can choose whether or not to allow my health care providers and health plans to "
                + "obtain access to my medical records through the health information exchange organization called Healthix. "
                + "If I give consent, my medical records from the different places where I get health care can be accessed using"
                + " a statewide computer network. I can fill out this form once and allow all Healthix Participants (including their agents) "
                + "who provide me with treatment or care management services to electronically access my information. Healthix is a"
                + " not-for-profit organization that securely shares information about people's health electronically to improve the quality"
                + " of healthcare and meets the privacy and security standards of HIPAA and New York State Law. "
                + "To learn more visit Healthix's website at www.healthix.org");*/

            #region PRIMEPOS-3068  
            //_HEALTHIXFRM1.Add("Header", "Authorization for Release of Patient Information"); 15 / 03 / 2022
            _HEALTHIXFRM1.Add("TEXT", "        Authorization for Release of Patient Information"
                + "                                                                    "
                + "I request that health information regarding my care and "
                + "treatment be accessed as set forth on this form. I can  "
                + "    choose whether or not to allow my health care providers "
                + "and health plans to obtain access to my medical records "
                + "through the health information exchange organization "
                + "called Healthix.If I give consent, my medical records "
                + "from the different   places where I get health care can "
                + "be accessed using a statewide computer network. "
                + "I can fill out this form once and    allow all Healthix "
                + "Participants (including their agents) who   provide me "
                + "with treatment or care management services to "
                + "electronically access my information. Healthix is a "
                + "not-for-profit organization that securely shares "
                + "information about  people's health electronically "
                + "to improve the quality of      healthcare and meets the "
                + "privacy and security standards of HIPAA and "
                + "New York State Law. To learn more visit          Healthix's "
                + "website at www.healthix.org");
            #endregion PRIMEPOS-3068 

            DeviceCommand("SHOWTEXTBOX", _HEALTHIXFRM1);

            while (shouldMoveNext == null)
            {
                Thread.Sleep(100);
            }

        }

        private void ShowHealtixForm2(string PharmacyName)
        {
            logger.Debug("In ShowHelthixForm1()=>  ");

            Hashtable _HEALTHIXFRM2 = new Hashtable();
            _HEALTHIXFRM2.Add("COMMAND", "HEALTHIXFRM2");

            /*_HEALTHIXFRM2.Add("HEADER", "Authorization for Release of Patient Information");
            _HEALTHIXFRM2.Add("TEXT", "The choice I make in this form will NOT affect my ability to get medical care."
                + " The choice I make in this form does NOT allow health insurers to have access to my information for the"
                + " purpose of deciding whether to provide me with health insurance coverage or pay my medical bills.");
            _HEALTHIXFRM2.Add("PHARMACYNAME", string.IsNullOrEmpty(PharmacyName) ? "" : PharmacyName);*/

            #region PRIMEPOS-3068
            //_HEALTHIXFRM2.Add("HEADER", "Authorization for Release of Patient Information"); 16 / 03 /2022
            _HEALTHIXFRM2.Add("TEXT", "  Authorization for Release of Patient Information"
                + "                                                          "
                + "  The choice I make in this form will NOT affect my      ability to get medical care."
                + " The choice I make in this form does NOT allow health insurers to have access to  my information for the"
                + " purpose of deciding whether to    provide me with health insurance coverage or pay my    medical bills.");
            _HEALTHIXFRM2.Add("PHARMACYNAME", string.IsNullOrEmpty(PharmacyName) ? "" : PharmacyName);
            #endregion  

            DeviceCommand("SHOWTEXTBOX", _HEALTHIXFRM2);
            ConsentApplies = -1;
            while (ConsentApplies == -1)
            {
                Thread.Sleep(100);
            }
            if (!bSkipConsent)
            {
                tmpConsent.ConsentTypeCode = PatientConsent.GetConsentTypeCodeForHealthix(ConsentApplies);
            }
            else
            {
                tmpConsent.IsConsentSkip = true;
            }
        }

        private void ShowHealtixForm3()
        {
            logger.Debug("In ShowHelthixForm1()=>  ");

            Hashtable _HEALTHIXFRM3 = new Hashtable();
            _HEALTHIXFRM3.Add("COMMAND", "HEALTHIXFRM3");
            _HEALTHIXFRM3.Add("HEADER", "Authorization"/* for Release of Patient Information*/);
            DeviceCommand("SHOWTEXTBOX", _HEALTHIXFRM3);

            ConsentStatus = -1;
            while (ConsentStatus == -1)
            {
                Thread.Sleep(100);
            }
            if (!bSkipConsent)
            {
                tmpConsent.ConsentStatusCode = PatientConsent.GetStatusCodeForHealthix(ConsentStatus);
            }
            else
            {
                tmpConsent.IsConsentSkip = true;
            }

        }


        public void CaptureOTCItemSignature()
        {
            Hashtable fields = new Hashtable();
            fields.Add("COMMAND", "CAPTURESIGNATURE");
            DeviceCommand("CAPTURESIGNATURE", fields);
        }


        public void CaptureOTCItemSignatureAknowledgement(ArrayList otcList)
        {
            Hashtable OTCItemsData = new Hashtable();
            OTCItemsData.Add("COMMAND", "OTCITEMDESC" /*Show RX INFO Remaining*/);
            OTCItemsData.Add("HEADER", " OTC Items Acknowledgement");
            OTCItemsData.Add("MESSAGE", "" + otcList[0]);

            ArrayList OTCItemList = new ArrayList();
            for (int i = 1; i < otcList.Count; i++)
            {
                OTCItemList.Add(otcList[i]);
            }
            OTCItemsData.Add("OTCITEMS", OTCItemList);
            DeviceCommand("SHOWTEXTBOX", OTCItemsData);
        }

        #endregion

        #region VANTIVDeviceCommandQueue 
        /// <summary>
        /// This function Queue all Commands for the Device.
        /// Each command MUST be queued in order for the device not to crash.
        /// </summary>
        /// <param n0ame="cScreenP"></param>
        /// <param name="DataP"></param>
        private void DeviceCommand(string MessageID, Hashtable DataP)
        {
            lock (CommandQueue)
            {
                logger.Debug("\t\tQueue command");
                CommandStation CommandData = new CommandStation(MessageID, DataP);
                CommandQueue.Enqueue(CommandData); //Queue the command for the device
                logger.Debug("\t\tFinish Queue command: " + CommandQueue.Count);



                if (CommandQueue.Count > 0)
                {
                    // - Handling Thread Exception
                    bool isException = false;
                    lock (exceptionObj)
                    {
                        isException = this.isException;
                    }
                    //
                    if (isException || (DeviceThread == null ? true : !DeviceThread.IsAlive))// - Handling Thread Exception - Add isException
                    {
                        // if exception and thread.isalive = true; kill the thread
                        if (DeviceThread != null)
                        {
                            if (!isException)// - 13-Dec-2018
                            {
                                DeviceThread.Abort();
                            }

                            DeviceThread = null;
                            //if (isException)
                            //{
                            //    if (DataP["COMMAND"].Equals("RELOADITEMLIST"))
                            //    {
                            //        Hashtable fields = new Hashtable();
                            //        fields.Add("COMMAND", "RELOADITEMLIST");
                            //        //DeviceCommand("SHOWMESSAGE", fields);
                            //        CommandQueue = new Queue<CommandStation>();
                            //        CommandQueue.Enqueue(new CommandStation("SHOWMESSAGE", fields));
                            //    }
                            //}
                        }

                        lock (exceptionObj)
                        {
                            isException = false;
                            this.isException = false;
                        } //  - Lock Exception

                        /* If the thread is not alive start a new thread. */
                        DeviceThread = new Thread(ExecuteDeviceCommnads);
                        DeviceThread.Start();

                    }
                }
                logger.Debug("\t\tExiting DeviceCommand");
            }
        }

        private void ExecuteDeviceCommnads()
        {
            logger.Debug("\t\t==>About to Set Data on Device");
            CommandStation command = null; //  - Add 
            try
            {
                //This is to make sure no other data is being pass from any other method while in here
                lock (DeviceLock)
                {
                    while (CommandQueue.Count > 0)
                    {
                        if (!Constant.IsInMethod)
                        {
                            Constant.IsInMethod = true; //set condition so it wait until everything finish before going in the loop again
                            Constant.IsStillWrite = true; //make sure this is set so POS can know that device is still writing to the device
                            bool isCommand = false;
                            //CommandStation command = null; //  - Commented
                            command = null; // 
                            logger.Debug("\t\tAbout to dequeue command from the Queue to the pad, Count: " + CommandQueue.Count);
                            try

                            {
                                command = CommandQueue.Dequeue(); //Deueue the Queue data                             
                                isCommand = true;
                            }
                            catch
                            {
                                isCommand = false; // if dequeue fail set this to false.
                            }

                            if (!isCommand)
                            {
                                Constant.IsInMethod = false;
                                Constant.IsStillWrite = false;
                                ErrorLog += VANTIV_ErrorLog; //Event for Error. This will call the Thread again //  - Commented
                                ErrorLog.Invoke(); //  Commented

                                lock (exceptionObj)
                                {
                                    isException = true;
                                } //  - Lock Exception
                                return;
                            }
                            string cScreen = string.Empty;

                            if (PData != null)
                                PData.Clear();
                            else
                                PData = new Hashtable();

                            PData = command.DeviceData as Hashtable; //data that was dequeue
                            cScreen = command.DeviceScreen;//data that was dequeue

                            if (!string.IsNullOrEmpty(cScreen) && PData != null)
                            {
                                Constant.CurrentDequeueScreen = string.Empty;
                                Constant.CurrentDequeueData = new Hashtable();
                                Constant.CurrentDequeueScreen = cScreen;
                                Constant.CurrentDequeueData = PData;
                            }

                            logger.Debug("\t\tEntering VANTIV ExecuteCommand ");


                            switch (cScreen.ToUpper().Trim())
                            {
                                case "GETSIGNATURE":
                                case "CAPTURESIGNATURE":
                                case "DOSIGNATURE":
                                    deviceResponse = device.GetSimpleSignature();
                                    if (deviceResponse.SignatureData != null && deviceResponse.SignatureData.Length > 0)
                                    {
                                        GetSignatureString = deviceResponse.SignatureData;
                                        IsSignValid = true;
                                    }
                                    else
                                    {
                                        IsSignValid = false;
                                    }
                                    break;
                                case "REBOOT":
                                    logger.Debug("\t\t\t[REBOOT] Begin");
                                    logger.Debug("\t\t\t[REBOOT] Finish");
                                    break;
                                case "SHOWTHANKYOU":
                                    logger.Debug("\t\t\t[SHOWTHANKYOU] Begin");
                                    string thanksMessage = @"ThankYou " + " Payment of " + PData["AMT"] + "$ Successfully done ";
                                    deviceResponse = device.DisplayMessage(thanksMessage);
                                    break;
                                case "SHOWDIALOG":
                                case "SHOWMESSAGE":
                                    logger.Debug("\t\t\t[SHOWMESSAGE] Begin");
                                    string screenCommand = (string)PData["COMMAND"];
                                    string itemAction = (string)PData["ACTION"];
                                    if (screenCommand == "SHOWRXLIST")
                                    {
                                        if (!PData.ContainsKey("ItemName"))
                                        {
                                            //device.ShowItemScreen(null, "");
                                        }
                                        else
                                        {
                                            string itemName = (string)PData["ItemName"];
                                            string placeHolderName = (string)PData["ItemName"];
                                            string showRXDesc = (string)PData["SHOWRXDESCRIPTION"];
                                            string isRX = (string)PData["ISRX"];
                                            string itemOty = (string)PData["ItemQty"];
                                            string unitPrice = (string)PData["UnitPrice"];
                                            string totalPrice = (string)PData["TotalPrice"];
                                            string withDiscAmout = (string)PData["WITHDISCAMOUNT"];
                                            string tax = (string)PData["TAX"];
                                            string discount = (string)PData["DISCOUNT"];

                                            if (isRX == "1")
                                            {
                                                string[] strArray = itemName.Split('-');
                                                if (strArray.Length > 0 && Convert.ToBoolean(showRXDesc) == false)
                                                {
                                                    placeHolderName = string.Format("{0,-10}", strArray[0] + "-****");
                                                }
                                                else
                                                {
                                                    placeHolderName = string.Format("{0,-10}", string.IsNullOrEmpty(PData["ItemName"].ToString().Trim()) ? "Not Available" : itemName);
                                                }
                                            }
                                            else
                                            {
                                                placeHolderName = string.Format("{0,-10}", string.IsNullOrEmpty(PData["ItemName"].ToString().Trim()) ? "Not Available" : itemName);
                                            }

                                            Order mOrder = new Order();
                                            var item = deviceData.Where(p => p.name.contentText.ToString().Trim() == itemName.ToString().Trim()
                                            /*&& p.price.contentText.ToString().Trim() == unitPrice.Trim()*/).SingleOrDefault();
                                            if (deviceData.Contains(item))
                                            {
                                                //update item
                                                mOrder = deviceData[deviceData.IndexOf(item)];
                                                mOrder.name.contentText = placeHolderName;
                                                mOrder.quantity.contentText = itemOty;
                                                mOrder.price.contentText = unitPrice;
                                                mOrder.discount.contentText = discount;
                                                mOrder.total.contentText = totalPrice;
                                                mOrder.tax.contentText = tax;
                                            }
                                            else
                                            {
                                                //Add item                                                
                                                mOrder.name = new Descreption(placeHolderName);
                                                mOrder.quantity = new Quantity(itemOty);
                                                mOrder.price = new UnitPrice(unitPrice);
                                                mOrder.discount = new Discount(discount);
                                                mOrder.total = new Total(totalPrice);

                                                //deviceData.Contains(item);
                                                //if (deviceData.Contains(item))
                                                //{
                                                //    mOrder = deviceData[deviceData.IndexOf(item)];
                                                //    mOrder.tax = new Tax(tax);
                                                //}
                                                //else
                                                mOrder.tax = new Tax(tax);

                                                deviceData.Add(mOrder);
                                            }
                                            Dictionary<int, Hashtable> orders = new Dictionary<int, Hashtable>();
                                            for (int i = 0; i < deviceData.Count; i++)
                                            {
                                                Hashtable sa = new Hashtable();

                                                sa.Add("Name", deviceData[i].name);
                                                sa.Add("Price", deviceData[i].price);
                                                sa.Add("Tax", deviceData[i].tax);
                                                sa.Add("Qty", deviceData[i].quantity);
                                                sa.Add("Total", deviceData[i].total);
                                                sa.Add("Discount", deviceData[i].discount);
                                                orders.Add(i, sa);
                                            }


                                            //device.ShowItemScreen(orders, itemAction);
                                            device.AddItem(orders, itemAction);
                                            logger.Debug("\t\t\t[SHOWMESSAGE] Finish");
                                        }
                                    }
                                    else if (screenCommand == "DELETEITEM")
                                    {
                                        logger.Debug("\t\t\t[DELETEITEM] Begin");
                                        if (deviceData.Count > 0)
                                        {
                                            string sCurrentTransID = (string)PData["CurrentTransID"];
                                            int nRowID = (int)PData["RowID"];
                                            Order dOrder = deviceData[nRowID];
                                            Order itemNameString = null;
                                            try
                                            {
                                                itemNameString = deviceData.Where(p => p.name.contentText == dOrder.name.contentText
                                                && p.price.contentText.Trim() == dOrder.price.contentText.Trim()).SingleOrDefault();
                                                //deviceData.RemoveAll(p => p.name.contentText == dOrder.name.contentText);
                                            }
                                            catch (Exception ex)
                                            {
                                                itemNameString = null;
                                            }
                                            Dictionary<int, Hashtable> orders = new Dictionary<int, Hashtable>();

                                            bool isremovedListItem = deviceData.Remove(itemNameString);

                                            for (int i = 0; i < deviceData.Count; i++)
                                            {
                                                Hashtable sa = new Hashtable();

                                                sa.Add("Name", deviceData[i].name);
                                                sa.Add("Price", deviceData[i].price);
                                                sa.Add("Tax", deviceData[i].tax);
                                                sa.Add("Qty", deviceData[i].quantity);
                                                sa.Add("Total", deviceData[i].total);
                                                sa.Add("Discount", deviceData[i].discount);
                                                orders.Add(i, sa);
                                            }
                                            logger.Debug("\t\t\t[DELETEITEM]  " + isremovedListItem);
                                            device.AddItem(orders, itemAction);
                                        }
                                        if (deviceData.Count == 0)
                                        {
                                            //device.IdleScreen();
                                            //device.IdleScreen(device.Url, device.LaneID, device.ApplicationName, device.StationID);
                                        }
                                        logger.Debug("\t\t\t[DELETEITEM] Finish");
                                    }
                                    else if (screenCommand == "UPDATEITEM")
                                    {
                                        logger.Debug("\t\t\t[DELETEITEM] Begin");
                                        if (deviceData.Count > 0)
                                        {
                                            string sCurrentTransID = (string)PData["CurrentTransID"];
                                            int nRowID = (int)PData["RowID"];
                                            Order dOrder = deviceData[nRowID];
                                            Order itemNameString = null;
                                            try
                                            {
                                                itemNameString = deviceData.Where(p => p.name.contentText == dOrder.name.contentText).Single();
                                            }
                                            catch (Exception ex)
                                            {
                                                itemNameString = null;
                                            }
                                            Dictionary<int, Hashtable> orders = new Dictionary<int, Hashtable>();

                                            bool isremovedListItem = deviceData.Remove(itemNameString);

                                            for (int i = 0; i < deviceData.Count; i++)
                                            {
                                                Hashtable sa = new Hashtable();

                                                sa.Add("Name", deviceData[i].name);
                                                sa.Add("Price", deviceData[i].price);
                                                sa.Add("Tax", deviceData[i].tax);
                                                sa.Add("Qty", deviceData[i].quantity);
                                                sa.Add("Total", deviceData[i].total);
                                                sa.Add("Discount", deviceData[i].discount);
                                                orders.Add(i, sa);
                                            }
                                            logger.Debug("\t\t\t[DELETEITEM]  " + isremovedListItem);
                                            device.AddItem(orders, itemAction);
                                        }
                                        logger.Debug("\t\t\t[DELETEITEM] Finish");
                                    }
                                    else if (screenCommand == "RELOADITEMLIST")
                                    {
                                        Dictionary<int, Hashtable> orders = new Dictionary<int, Hashtable>();

                                        for (int i = 0; i < deviceData.Count; i++)
                                        {
                                            Hashtable sa = new Hashtable();

                                            sa.Add("Name", deviceData[i].name);
                                            sa.Add("Price", deviceData[i].price);
                                            sa.Add("Tax", deviceData[i].tax);
                                            sa.Add("Qty", deviceData[i].quantity);
                                            sa.Add("Total", deviceData[i].total);
                                            sa.Add("Discount", deviceData[i].discount);
                                            orders.Add(i, sa);
                                        }
                                        device.AddItem(orders, itemAction);
                                    }
                                    else if (screenCommand == "HOLDITEMS")
                                    {
                                        ArrayList onholdItems = (ArrayList)PData["OnHoldItems"];
                                        for (int i = 0; i < onholdItems.Count; i++)
                                        {
                                            Hashtable item = (Hashtable)onholdItems[i];
                                            string itemName = (string)item["ItemName"];
                                            string placeHolderName = (string)item["ItemName"];
                                            string showRXDesc = (string)item["SHOWRXDESCRIPTION"];
                                            string isRX = ((int)item["ISRX"] > 0).ToString();
                                            string itemOty = (string)item["ItemQty"];
                                            string unitPrice = (string)item["UnitPrice"];
                                            string totalPrice = (string)item["TotalPrice"];
                                            string withDiscAmout = (string)item["WITHDISCAMOUNT"];
                                            string discount = (string)item["DISCOUNT"];
                                            string tax = (string)item["TAX"];


                                            if (isRX == "True")
                                            {
                                                string[] strArray = itemName.Split('-');
                                                if (strArray.Length > 0 && Convert.ToBoolean(showRXDesc) == false)
                                                {
                                                    placeHolderName = string.Format("{0,-10}", strArray[0] + "-****");
                                                }
                                                else
                                                {
                                                    placeHolderName = string.Format("{0,-10}", string.IsNullOrEmpty(item["ItemName"].ToString().Trim()) ? "Not Available" : itemName);
                                                }
                                            }
                                            else
                                            {
                                                placeHolderName = string.Format("{0,-10}", string.IsNullOrEmpty(item["ItemName"].ToString().Trim()) ? "Not Available" : itemName);
                                            }

                                            Order mOrder = new Order();
                                            mOrder.name = new Descreption(placeHolderName);
                                            mOrder.quantity = new Quantity(itemOty);
                                            mOrder.price = new UnitPrice(unitPrice);
                                            mOrder.discount = new Discount(discount);
                                            mOrder.total = new Total(totalPrice);
                                            mOrder.tax = new Tax(tax);
                                            deviceData.Add(mOrder);
                                        }

                                        Dictionary<int, Hashtable> orders = new Dictionary<int, Hashtable>();

                                        for (int i = 0; i < deviceData.Count; i++)
                                        {
                                            Hashtable sa = new Hashtable();

                                            sa.Add("Name", deviceData[i].name);
                                            sa.Add("Price", deviceData[i].price);
                                            sa.Add("Tax", deviceData[i].tax);
                                            sa.Add("Qty", deviceData[i].quantity);
                                            sa.Add("Total", deviceData[i].total);
                                            sa.Add("Discount", deviceData[i].discount);
                                            orders.Add(i, sa);
                                        }
                                        device.AddItem(orders, "R");
                                        logger.Debug("\t\t\t[SHOWMESSAGE] Finish");
                                    }
                                    else
                                    {
                                        string _CustomTitle = (string)PData["TITLE"];
                                        string _CustomMessage = (string)PData["MESSAGE"];
                                        device.DisplayMessage(_CustomMessage);
                                        logger.Debug("\t\t\t[SHOWMESSAGE] Finish");
                                    }
                                    deviceDataTemp = deviceData;
                                    break;
                                case "CLEARMESSAGE":
                                    device.IdleScreen();
                                    //Added by Arvind
                                    if (deviceData != null)
                                    {
                                        deviceData.Clear();
                                    }
                                    break;
                                case "RESET":
                                case "UPDATERESOURCEFILE":
                                case "DELETEIMAGE":
                                case "GETPINBLOCK":
                                case "INPUTACCOUNT":
                                case "RESETMSR":
                                case "INPUTTEXT":
                                case "CHECKFILE":
                                case "SHOWTEXTBOX":
                                    logger.Debug("\t\t\t[SHOWTEXTBOX] Begin");
                                    string screenDisplay = (string)PData["COMMAND"];
                                    string titleText = (string)PData["HEADER"];
                                    string patName = (string)PData["PATIENTNAME"];
                                    /*if (screenDisplay == "HIPPAACK")
                                    {
                                        string message = (string)PData["TEXT"];
                                        message = Regex.Replace(message, ".{50}", "$0" + "\u001c");
                                        string patAdd = (string)PData["PATIENTADDRESS"];
                                        string alteredMsg = message + patAdd + "\u001cProceed?";
                                        deviceResponse = device.ShowTextBox(titleText, alteredMsg, null, screenDisplay);
                                        HIPPAAckResultId = deviceResponse.buttonResponse.ToString();
                                    }*/
                                    #region PRIMEPOS-3068
                                    if (screenDisplay == "HIPPAACK")
                                    {
                                        string message = (string)PData["TEXT"];
                                        message = "                 HIPAA ACKNOWLEDGEMENT"
                                           + "                   "
                                           + "By Signing below, I hereby acknowledge that "
                                           + "I have received and reviewed the NOPP(Notice of  Privacy Practices)"
                                           + " and that I hereby adhere to all the contents mentioned in the policy. "; //PRIMEPOS-3212
                                        message = Regex.Replace(message, ".{50}", "$0" + "\u001c");
                                        string patAdd = (string)PData["PATIENTADDRESS"];
                                        string alteredMsg = message + patAdd + "\u001cProceed?";
                                        deviceResponse = device.ShowTextBox(titleText, alteredMsg, null, screenDisplay);
                                        HIPPAAckResultId = deviceResponse.buttonResponse.ToString();
                                    }
                                    #endregion 
                                    else if (screenDisplay == "RXHEADERINFO")
                                    {
                                        string RXCount = (string)PData["RXCOUNT"];
                                        string RXData = (string)PData["RXDATA"];
                                        var patientName = patName;
                                        if (patName.Length > 35)
                                        {
                                            patientName = patName.Substring(0, 34);
                                        }
                                        string alteredMsg = "  Patient : " + patientName.Trim() +
                                         "                          RxCount : " + RXCount + "\u001c" +
                                         "________________________________________" + "\u001c" +
                                       "   RxInfo :" + "\u001c   " + RXData + "\u001c" +
                                        "________________________________________" +
                                         "\u001c" + "\u001c" +
                                         "                              Patient Counseling ? ";//Append original msg to altered one

                                        deviceResponse = device.ShowTextBox(titleText, alteredMsg, null, screenDisplay);

                                        PatCounsel = deviceResponse.buttonResponse == 1 ? Constant.PatientCounselYes : Constant.PatientCounselNo;
                                    }
                                    else if (screenDisplay == "HEALTHIXFRM1")
                                    {
                                        string message = (string)PData["TEXT"];
                                        message = Regex.Replace(message, ".{60}", "$0" + "\u001c");//PRIMEPOS-3068
                                        deviceResponse = device.ShowTextBox(titleText, message, PData, screenDisplay);
                                        if (deviceResponse.buttonResponse != 0)
                                        {
                                            shouldMoveNext = true;
                                        }
                                        else
                                        {
                                            shouldMoveNext = false;
                                        }
                                    }
                                    else if (screenDisplay == "HEALTHIXFRM2")
                                    {
                                        string pharmacy = (string)PData["PHARMACYNAME"];
                                        string message = (string)PData["TEXT"];
                                        message = Regex.Replace(message, ".{55}", "$0" + "\u001c");//PRIMEPOS-3068
                                        //deviceResponse = device.ShowTextBox(titleText, message, "All Healthix Participants", pharmacy, "Skip", screenDisplay); //PRIMEPOS - 2555 Add  @"\1" for Small Font text to display on device -
                                        deviceResponse = device.ShowTextBox(titleText, message, PData, screenDisplay);
                                        ConsentApplies = deviceResponse.buttonResponse;
                                        if (ConsentApplies == 0)
                                        {
                                            //ConsentApplies = 0;
                                            tmpConsent.IsConsentSkip = true;
                                            bSkipConsent = true;
                                        }
                                    }
                                    else if (screenDisplay == "HEALTHIXFRM3")
                                    {

                                        deviceResponse = device.ShowTextBox("", "", PData, screenDisplay);
                                        ConsentStatus = deviceResponse.buttonResponse;
                                    }
                                    else if (screenDisplay == "OTCITEMDESC")
                                    {
                                        string message = (string)PData["MESSAGE"];
                                        message = Regex.Replace(message, ".{50}", "$0" + "\u001c");
                                        ArrayList OTCItems = (ArrayList)PData["OTCITEMS"];
                                        string OTCItemsStr = "";
                                        for (int i = 0; i < OTCItems.Count; i++)
                                        {
                                            OTCItemsStr += OTCItems[i] + "\n";
                                        }
                                        string alteredMsg = message + "\u001c" + "\u001c" +
                                       "________________________________________" + "\u001c" +
                                     "      Items :" + "\u001c" + OTCItemsStr + "\u001c" +
                                      "________________________________________";

                                        deviceResponse = device.ShowTextBox(titleText, alteredMsg, null, screenDisplay);
                                        if (deviceResponse.buttonResponse != 0)
                                        {
                                            shouldMoveNext = true;
                                        }
                                        else
                                        {
                                            shouldMoveNext = false;
                                        }
                                    }
                                    logger.Debug("\t\t\t[SHOWTEXTBOX] Finish");
                                    break;
                                case "AUTHORIZECARD":
                                case "COMPLETEONLINEEMV":
                                case "REMOVECARD":
                                case "GETEMVTLVDATA":
                                case "SETEMVTLVDATA":
                                case "INPUTACCOUNTWITHEMV":
                                case "COMPLETECONTACTLESSEMV":
                                case "SETSAFPARAMETERS":
                                case "REPRINT":
                                case "PRINTER":
                                case "SHOWITEM":
                                case "CARDINSERTDETECTION":
                                case "TOKENADMINISTRATIVE":
                                    break;
                                case "SHOWDIALOGFORM":
                                    string titleDialogFORM = (string)PData["TITLE"];
                                    string Label1Dialog = (string)PData["LABEL1"];
                                    string Label2Dialog = (string)PData["LABEL2"];
                                    string Label3Dialog = (string)PData["LABEL3"];
                                    string Label4Dialog = (string)PData["LABEL4"];
                                    string Text1Dialog = (string)PData["TEXT1"];
                                    string Text2Dialog = (string)PData["TEXT2"];
                                    string Text3Dialog = (string)PData["TEXT3"];
                                    string Text4Dialog = (string)PData["TEXT4"];
                                    int ButtonType = (int)PData["BUTTONTYPE"];
                                    string screenCommandDialogForm = (string)PData["COMMAND"];

                                    deviceResponse = device.ShowDialogForm(PData, "PATIENTTYPE");

                                    PattypeSelection = deviceResponse.buttonResponse;
                                    break;
                                case "PATIENTCONSENT":
                                    deviceResponse = device.PatientConsent(PData);
                                    if (deviceResponse.SecondButtonResponse != 0)
                                    {
                                        tmpConsent.ConsentTextID = Convert.ToInt32(oDataSet.Tables["ConsentTextVersion"].Rows[0]["Id"]);
                                        if (deviceResponse.SecondButtonResponse == 1)
                                        {
                                            tmpConsent.PatConsentRelationShipDescription = oDataSet.Tables["Consent_Relationship"].Rows[0]["Relation"].ToString();
                                            tmpConsent.PatConsentRelationID = Convert.ToInt32(oDataSet.Tables["Consent_Relationship"].Rows[0]["Id"]);
                                        }
                                        else if (deviceResponse.SecondButtonResponse == 2)
                                        {
                                            tmpConsent.PatConsentRelationShipDescription = oDataSet.Tables["Consent_Relationship"].Rows[1]["Relation"].ToString();
                                            tmpConsent.PatConsentRelationID = Convert.ToInt32(oDataSet.Tables["Consent_Relationship"].Rows[1]["Id"]);
                                        }
                                        else if (deviceResponse.SecondButtonResponse == 3)
                                        {
                                            tmpConsent.PatConsentRelationShipDescription = oDataSet.Tables["Consent_Relationship"].Rows[2]["Relation"].ToString();
                                            tmpConsent.PatConsentRelationID = Convert.ToInt32(oDataSet.Tables["Consent_Relationship"].Rows[2]["Id"]);
                                        }
                                    }
                                    if (deviceResponse.FirstButtonResponse == 1)
                                    {
                                        tmpConsent.ConsentStatusID = Convert.ToInt32(oDataSet.Tables["Consent_Status"].Rows[0]["ID"].ToString());
                                        tmpConsent.ConsentStatusCode = oDataSet.Tables["Consent_Status"].Rows[0]["Code"].ToString();
                                        returnValue = true;
                                    }
                                    else if (deviceResponse.FirstButtonResponse == 2)
                                    {
                                        tmpConsent.ConsentStatusID = Convert.ToInt32(oDataSet.Tables["Consent_Status"].Rows[1]["ID"].ToString());
                                        tmpConsent.ConsentStatusCode = oDataSet.Tables["Consent_Status"].Rows[1]["Code"].ToString();
                                        returnValue = true;
                                    }
                                    else
                                    {
                                        tmpConsent.IsConsentSkip = true;
                                        returnValue = false;
                                    }
                                    PatConsent = tmpConsent;
                                    tmpConsent = null;
                                    break;
                                default:
                                    break;
                            }

                        }
                        logger.Debug("\t\t ----> VANTIV ExecuteCommand finish setting");

                        Constant.IsInMethod = false;
                    }
                }
                Constant.IsStillWrite = false; //Now allow to start a new thread.            
            }
            catch (UnauthorizedAccessException uA)
            {
                logger.Error("\t\tError in VANTIV ExecuteCommand  \n" + uA.ToString());
                Constant.IsStillWrite = false;
                Constant.IsInMethod = false;
                ErrorLog += VANTIV_ErrorLog;
                ErrorLog.Invoke();

            }
            catch (Exception ex)
            {
                logger.Debug("\t\tError in VANTIV ExecuteCommand  \n" + ex.ToString());
                Constant.IsStillWrite = false;
                Constant.IsInMethod = false;
                ErrorLog += VANTIV_ErrorLog;
                ErrorLog.Invoke();
                //  - Exception Handler
                lock (exceptionObj)
                {
                    isException = true;
                }
                if (command != null)
                {
                    Hashtable CatchData = command.DeviceData as Hashtable;

                    if (command.DeviceScreen == "SHOWMESSAGE")
                    {
                        //if (CatchData.Count != 0)
                        //{
                        //CatchData["ACTION"] = "";
                        //Hashtable fields = new Hashtable();
                        //fields.Add("COMMAND", "RELOADITEMLIST");
                        //DeviceCommand("SHOWMESSAGE", fields);
                        //}
                    }
                    if (command.DeviceScreen == "DOSIGNATURE" || command.DeviceScreen == "GETSIGNATURE")
                    {

                        IsSignValid = true;
                        GetSignatureString = " ";
                    }

                }
            }
        }

        private void VANTIV_ErrorLog()
        {
            logger.Debug("\t\tError activated from VANTIV ExecuteCommand ");
            if (CommandQueue.Count > 0 && !Constant.IsStillWrite)
            {
                CommandEvent += VANTIV_CommandEvent;
                CommandEvent.Invoke();
            }
        }

        private void VANTIV_CommandEvent()
        {
            CommandEvent -= VANTIV_CommandEvent; //deactivate the event
            if (DeviceThread == null ? true : !DeviceThread.IsAlive)
            {
                //If the thread is not alive start a new thread.
                DeviceThread = new Thread(ExecuteDeviceCommnads);
                DeviceThread.Start();
            }
        }

        #endregion

    }
    #region data structure to communicate with device
    //override .TOString() in all variable of order to format & pad them accordingly
    // const int finalLength = 15; in all Order variables classes is the lenght which is given on device screen text

    public class Order
    {

        //DisplayMessage1 
        public Descreption name
        {
            get; set;
        }

        //DisplyMessage2
        public Quantity quantity
        {
            get; set;
        }
        public UnitPrice price
        {
            get; set;
        }

        public Discount discount
        {
            get; set;
        }

        public Total total
        {
            get; set;
        }

        public Tax tax
        {
            get; set;
        }

        //  
        public override string ToString()
        {
            return String.Format("{0,0} {1,0} {2,13} {3,17} {4}", this.name.ToString(), this.quantity.ToString(), this.price.ToString(), this.discount.ToString(), this.total.ToString());
            //return String.Format("{0} {1} {2} {3}", this.name.ToString(), this.quantity.ToString(), this.price.ToString(), this.discount.ToString());
        }
    }

    public class Descreption
    {
        public string contentText = "";
        public int length = 0;
        const int finalLength = 15;
        public Descreption(string content)
        {
            contentText = content;
            this.length = content.Length;
        }
        public override string ToString()
        {
            string paddedString = "";
            if (contentText.Length < finalLength)
            {
                paddedString = contentText.Trim();
                int padSize = finalLength - contentText.Length;
                if (padSize > 0)
                {
                    paddedString = contentText.PadLeft(contentText.Length + padSize);
                }
            }
            if (contentText.Length > finalLength)
            {
                paddedString = contentText.Substring(0, finalLength);
            }
            return paddedString;
        }
    }

    public class Quantity
    {
        public string contentText = "";
        public int length = 0;
        const int finalLength = 7;
        public Quantity(string content)
        {
            contentText = content;
            this.length = content.Length;
        }

        public override string ToString()
        {
            string paddedString = "";
            if (contentText.Length < finalLength)
            {
                int padSize = finalLength - contentText.Length;
                if (padSize > 0)
                {
                    paddedString = contentText.PadLeft(contentText.Length + padSize);
                    paddedString = paddedString.PadRight(contentText.Length + padSize);
                }
            }
            if (contentText.Length > finalLength)
            {
                paddedString = contentText.Substring(0, finalLength);
            }
            return paddedString;
        }
    }

    public class UnitPrice
    {
        public string contentText = "";
        public int length = 0;
        const int finalLength = 10;

        public UnitPrice(string content)
        {
            contentText = content;
            this.length = content.Length;
        }

        public override string ToString()
        {
            string paddedString = "";
            if (contentText.Length < finalLength)
            {
                int padSize = finalLength - contentText.Length;
                if (padSize > 0)
                {
                    paddedString = contentText.PadLeft(contentText.Length + padSize);
                    paddedString = paddedString.PadRight(contentText.Length + padSize);
                }
            }
            if (contentText.Length > finalLength)
            {
                paddedString = contentText.Substring(0, finalLength);
            }
            return paddedString;
        }
    }

    public class Discount
    {
        public string contentText = "";
        public int length = 0;
        const int finalLength = 5;
        public Discount(string content)
        {
            contentText = content;
            this.length = content.Length;
        }

        public override string ToString()
        {
            string paddedString = "";
            if (contentText.Length < finalLength)
            {
                int padSize = finalLength - contentText.Length;
                if (padSize > 0)
                {
                    paddedString = contentText.PadLeft(contentText.Length + padSize);
                    paddedString = paddedString.PadRight(contentText.Length + padSize);
                }
            }
            if (contentText.Length > finalLength)
            {
                paddedString = contentText.Substring(0, finalLength);
            }
            return paddedString;
        }
    }

    public class Total
    {
        public string contentText = "";
        public int length = 0;
        const int finalLength = 10;
        public Total(string content)
        {
            contentText = content;
            this.length = content.Length;
        }

        public override string ToString()
        {
            string paddedString = "";
            if (contentText.Length < finalLength)
            {
                int padSize = finalLength - contentText.Length;
                if (padSize > 0)
                {
                    paddedString = contentText.PadRight(contentText.Length + padSize);
                    paddedString = paddedString.PadLeft(paddedString.Length + padSize);
                }
            }
            if (contentText.Length > finalLength)
            {
                paddedString = contentText.Substring(0, finalLength);
            }
            return paddedString;
        }
    }
    #endregion



    #region data structure to Show RxDetails
    public class RXInfo
    {

        //DisplayMessage1 
        public string RXNo
        {
            get; set;
        }

        //DisplyMessage2
        public string drugName
        {
            get; set;
        }
        public string date
        {
            get; set;
        }


        //  
        public override string ToString()
        {
            //return String.Format("{0,0} {1,0} {2,13} {3,17} {4}", this.name.ToString(), this.quantity.ToString(), this.price.ToString(), this.discount.ToString(), this.total.ToString());
            return String.Format("{0,0} {1,10} {2,12}", this.RXNo.ToString(), this.drugName.ToString(), this.date.ToString());
        }
    }
    public class Tax
    {
        public string contentText = "";
        public int length = 0;
        const int finalLength = 15;
        public Tax(string content)
        {
            contentText = content;
            this.length = content.Length;
        }
        public override string ToString()
        {
            string paddedString = "";
            if (contentText.Length < finalLength)
            {
                int padSize = finalLength - contentText.Length;
                if (padSize > 0)
                {
                    paddedString = contentText.PadRight(contentText.Length + padSize);
                }
            }
            if (contentText.Length > finalLength)
            {
                paddedString = contentText.Substring(0, finalLength);
            }
            return paddedString;
        }
    }
    #endregion

}
