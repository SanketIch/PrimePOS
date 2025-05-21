using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Data;
using System.ComponentModel;
using System.Threading;
using System.Globalization;
using POS.Devices;
using NLog;

namespace MMS.Device
{
    /// <Author>Author: Manoj Kumar/Rohit Nair</Author>
    /// <summary>
    /// Description: Use to communicate with the Verifone MX 870 device and the POS.
    /// This is the Main Communication channel between the Device and POS.
    /// All Data are routed thru this class.
    /// </summary>
    public class Communication : WPDevice.WPData
    {
        ILogger logger = LogManager.GetCurrentClassLogger();

        #region Variable and Property
        OPOSForm oDevice = null;
        private string _cScreen;
        //OPOSMsr OposMSR = null; //Swiper
        private string _dataEventType;
        private string _signData;
        private string _sigType;
        private readonly object locker = new object();
        CultureInfo CI = CultureInfo.CurrentCulture;

        private ArrayList _CCData = null;
        public delegate void ReturnedErrorEvent(bool b, int i, string d);
        public event ReturnedErrorEvent ReturnedErrorEvents = delegate { };

        /// <summary>
        /// Use to store RX's
        /// </summary>
        DataTable rxListTable = new DataTable();

        /// <summary>
        /// Use to Store Patient Information
        /// </summary>
        Hashtable rxPatInfo = new Hashtable();
        Hashtable padDatatoPass = null;
        Hashtable cashtxn = null;
        /// <summary>
        /// Use to Store the Sum( SubTotal, Discount, Tax, Total Amount)
        /// </summary>
        ArrayList sumStore = new ArrayList();

        private decimal prevSubTot = -0.1M;
        private bool PosAck;

        /// <summary>
        /// Get Patient Counselling
        /// </summary>
        public string PatCounsel
        {
            get { return Constant.Counsel; }
            set { Constant.Counsel = value; }
        }

        public string DeviceName
        {
            set { Constant.DeviceName = value; }
        }
        /// <summary>
        /// Get the current screen that the dll expect the device
        /// </summary>
        public string currenScreenTobe
        {
            get { return _cScreen; }
            set { _cScreen = value; }
        }
        public PatientConsent PtConent//PRIMEPOS-2867 CONSENT
        {
            get;
            set;
        }
        public void ProcessPayment(Payment Pay, WPAccountInfo WPInfo)
        {
            oDevice.WPISCPayment(Pay, WPInfo);
        }


        public void WPCancelTransaction()
        {
            oDevice.WPISCCancelTransaction();
        }

        public bool Disconnect()
        {
            return oDevice.Disconnect();
        }

        public bool? IsSignValid
        {
            get { return oDevice.IsSignValid; }
            set { oDevice.IsSignValid = value; }
        }

        public byte[] GetSignature
        {
            get { return oDevice.GetSignature; }
            set { oDevice.GetSignature = value; }
        }

        public string GetSignatureString
        {
            get { return oDevice.GetSignatureString; }
            set { oDevice.GetSignatureString = value; }
        }

        public bool? IsWPResponse
        {
            get { return oDevice.IsWPResponse; }
            set { oDevice.IsWPResponse = value; }
        }

        public string ButtonClickID
        {
            get { return oDevice.ButtonClickID; }
            set { oDevice.ButtonClickID = value; }
        }

        public bool CancelCapture
        {
            get { return oDevice.CancelCapture; }
            set { oDevice.CancelCapture = value; }
        }

        public PatientConsent PatConsent
        {
            get { return oDevice.PatConsent; }
            set { oDevice.PatConsent = value; }
        }

        public Dictionary<string, string> ReturnResponse
        {
            get { return oDevice.ReturnResponse; }
            set { oDevice.ReturnResponse = value; }
        }

        #region  PRIMEPOS-2868 - Added for default consent
        public DataSet dsAutoRefillData
        {
            get { return oDevice.dsAutoRefillData; }
            set { oDevice.dsAutoRefillData = value; }
        }
        #endregion

        /// <summary>
        /// Initialize the OPOS Library Class
        /// </summary>
        public Communication()
        {
            logger.Debug("In Communication () : Inistalizing OPOS Library Class");
            if (oDevice == null)
            {
                oDevice = new OPOSForm();
                oDevice.ReturnCodeEvent += VF_ReturnCodeEvent;
            }
            oDevice.Initialize();
        }
        /// <summary>
        /// Ingenico iSC 480, Devie Name and Connected Com Port
        /// </summary>
        /// <param name="DeviceName"></param>
        /// <param name="ComP"></param>
        public Communication(string DeviceName, int ComP)
        {
            logger.Debug("In Communication () : FOr ISc Device Name " + DeviceName);
            Constant.DeviceName = DeviceName;
            if (oDevice == null)
            {
                oDevice = new OPOSForm();
                //MMS.Device.WPDevice.Shared.ShowRx += Shared_ShowRx;//Commented by Amit on 02 Dec 2020
                Device.WPDevice.Shared.ShowRx += Shared_ShowRx;//Added by Amit on 02 Dec 2020
            }
            oDevice.iSCInitialize(ComP);
        }

        void Shared_ShowRx(bool obj)
        {
            logger.Debug("In Shared_SHowRx()");
            ShowRxDetail("1", rxListTable);
        }


        /// <summary>
        /// Disable Pin Entry 
        /// </summary>
        public void DisablePinPad()
        {
            logger.Debug("In DisablePinPad()");
            if (oDevice != null)
            {
                oDevice.DisablePinEntry();
            }
        }
        /// <summary>
        /// Display the Verifone MX return Code
        /// </summary>
        public bool DisplayVFReturnCode
        {
            get { return Constant.DisplayErrorCode; }
        }

        /// <summary>
        /// If Error Code return reinitialize device
        /// </summary>
        public void ReInitialize()
        {
            logger.Debug("In ReInitialize()");
            oDevice = null;
            if (oDevice == null)
            {
                oDevice = new OPOSForm();
                oDevice.ReturnCodeEvent += VF_ReturnCodeEvent;
            }
            oDevice.ReInitialize();
        }

        /// <summary>
        /// PRIMEPOS-2534 - Lane CLose Issue Added Suraj
        /// </summary>
        public void ReInitializeISC(int PinPadPortNo)
        {
            logger.Debug("In ReInitialize ISC device()");
            if (oDevice == null)
            {
                oDevice = new OPOSForm();
            }
            oDevice.ReInitializeISC(PinPadPortNo);
            logger.Debug("Exiting ReInitialize ISC device()");
        }

        void VF_ReturnCodeEvent(bool val, int v, string desc)
        {
            logger.Debug("In VF_returnCodeEvent(): Activating Error Event");
            // logger.Debug("Error Event Activated");
            if (ReturnedErrorEvents != null)
            {
                ReturnedErrorEvents(val, v, desc);
            }
        }

        public bool isMsrCancel
        {
            set
            {
                if (value)
                {
                    _CCData = new ArrayList();
                    Constant.CCDataSwipe = new ArrayList();
                    padDatatoPass = new Hashtable();
                    oDevice.DisableMsr();
                }
            }
        }
        /// <summary>
        /// POS ACK that it reveived the data
        /// </summary>
        public bool IsPosAck
        {
            set
            {
                PosAck = value;
                if (PosAck)
                {
                    if (ClearPosPadData())
                    {
                        logger.Debug("\t>>>POS has ack the data.<<<");
                    }
                }
                else
                {
                    logger.Debug("\t>>>POS has NOT ack the data <<<");
                    Constant.ErrorEvent += Constant_ErrorEvent;
                }
            }
        }

        /// <summary>
        /// POS will ask if the pad has all the data ready. If true it will call PADDATA() for the data
        /// </summary>
        public bool IsPadAck
        {
            get
            {
                return Constant.PadAck;
            }
        }

        /// <summary>
        /// Get the POS Tracking state. Return True if it is already
        /// in a Method. False otherwise.
        /// </summary>
        public bool IsStillWrite
        {
            get
            {
                return Constant.IsStillWrite;
            }
        }
        #endregion

        #region PRIMEPOS-2730 - Arvind
        public void ClearDeviceQueue()
        {
            oDevice.ClearDeviceQueue();
        }

        public bool isDevQueueEmpty()
        {
            return oDevice.isDevQueueEmpty();
        }
        #endregion

        #region Method That the POS call and other Method operations
        /// <summary>
        /// Clear all the data after the POS ack that it has receive the Data.
        /// </summary>
        /// <returns></returns>
        private bool ClearPosPadData()
        {
            logger.Debug("In ClearPOSPadData()");
            bool isClear = false;
            try
            {
                logger.Debug("\tClearing the POSPADDATA");
                _dataEventType = string.Empty;
                _signData = string.Empty;
                _sigType = string.Empty;

                if (_CCData != null)
                {
                    _CCData.Clear();
                    _CCData = null;
                }

                if (padDatatoPass != null)
                {
                    padDatatoPass.Clear();
                    padDatatoPass = null;
                }

                //Remove event handler
                oDevice.padSign -= VF_padSign;
                oDevice.MsrSwiper -= VF_MsrSwiper;
                oDevice.PinData -= VF_PinData;
                isClear = true;
            }
            catch (Exception ex)
            {
                isClear = false;
                logger.Error(ex, ex.Message);
            }
            return isClear;
        }


        /// <summary>
        /// This method is called by the POS to get the info from the pad
        /// </summary>
        /// <returns></returns>
        public Hashtable PadData()
        {
            logger.Debug("In PadData()");
            try
            {
                logger.Debug(">>>>POS is requesting data<<<<");
                if (padDatatoPass != null)
                {
                    padDatatoPass.Clear();
                }
                else
                {
                    padDatatoPass = new Hashtable();
                }

                padDatatoPass.Add("DataEventType", _dataEventType);
                switch (_dataEventType.ToUpper().Trim())
                {
                    case "EVENTSIGN":
                    case "EVENTNOPP":
                    case "EVENTRXAPPROVE":
                    case "EVENTOTC":
                        {
                            padDatatoPass.Add("Sign", this._signData);
                            padDatatoPass.Add("SigType", Constant.SigType);
                        }
                        break;
                    case "EVENTCC":
                        {
                            padDatatoPass.Add("CCInfo", _CCData);
                            currenScreenTobe = "EVENTCC";
                        }
                        break;
                }
                Constant.PadAck = false;
                logger.Debug(">>>>POS received the data<<<<");
            }
            catch (Exception ex)
            {
                Constant.PadAck = true;
                logger.Error(ex, "Error in PADDATA \n" + ex.Message);
            }
            return padDatatoPass;

        }
        /// <summary>
        /// Display the screen call by the POS, will take the data and pass it to the device
        /// </summary>
        /// <param name="txnId"></param>
        /// <param name="screenName"></param>
        /// <param name="sData"></param>
        /// <returns></returns>
        public bool DisplayScreen(string txnId, string screenName, string sData, DataSet dsConsentDetail = null)//PRIMEPOS-2868 - Consent
        {
            logger.Debug("In DisplayScreen()");
            bool isDisplay = false;
            string ScreenDisplay = string.Empty;
            try
            {
                logger.Debug(">>>Entering DisplayScreen<<<");
                ScreenDisplay = ScreenToDisplay(screenName.ToUpper().Trim());

                if (!string.IsNullOrEmpty(ScreenDisplay.Trim()))
                {
                    logger.Debug("Still in DisplayScreen: " + _cScreen);

                    if (Constant.DeviceName != Constant.Devices.Ingenico_ISC480.ToString() && !Constant.isErrorEventActivated)
                    {
                        Constant.ErrorEvent += Constant_ErrorEvent;
                        Constant.isErrorEventActivated = true;
                    }

                    switch (ScreenDisplay)
                    {
                        case "PADSWIPECARD":
                            {
                                logger.Debug("Still in DisplayScreen about to Queue command to Device: " + ScreenDisplay);
                                oDevice.DeviceCommand(ScreenDisplay, PassingData(ScreenDisplay, sData));

                                if (Constant.DeviceName != Constant.Devices.Ingenico_ISC480.ToString())
                                {
                                    oDevice.MsrSwiper += VF_MsrSwiper;
                                }
                                logger.Debug("Finishing from DisplayScreen " + ScreenDisplay);
                            }
                            break;
                        case "PADNOPP":
                        case "PADSIGN":
                        case "PADMSGSCREEN":
                        case "PADITEMLIST":
                        case "PADPATSIGN":
                        case "HEALTHIX":
                            {
                                logger.Debug("Still in DisplayScreen " + ScreenDisplay + " about to Queue command to Device");
                                oDevice.DeviceCommand(ScreenDisplay, PassingData(ScreenDisplay, sData));
                                logger.Debug("Finishing from DisplayScreen " + ScreenDisplay);
                            }
                            break;
                        case "XLINK":
                            {
                                logger.Debug("Still in DisplayScreen " + ScreenDisplay + " about to pass over to XCHARGE");
                                XCharge(ScreenDisplay, sData);
                                logger.Debug("Finishing DisplayScreen ");
                            }
                            break;
                        case "PADREMINDN":
                        case "PADREMINDER":
                            {
                                oDevice.DeviceCommand(ScreenDisplay, PassingData(ScreenDisplay, ""));
                                break;
                            }
                        case "PATIENTCONSENT"://PRIMEPOS-2867 CONSENT
                            {
                                Hashtable hashtable = new Hashtable();
                                if (dsConsentDetail?.Tables[0]?.Rows?.Count > 0)
                                {
                                    hashtable.Add("TITLE", dsConsentDetail.Tables["ConsentTextVersion"].Rows[0]["ConsentTextTitle"]);
                                    hashtable.Add("TEXT", dsConsentDetail.Tables["ConsentTextVersion"].Rows[0]["ConsentText"]);
                                    hashtable.Add("PATIENTNAME", dsConsentDetail.Tables["PATIENT"].Rows[0]["LNAME"] + " " + dsConsentDetail.Tables["PATIENT"].Rows[0]["FNAME"]);
                                    hashtable.Add("PATIENTADDRESS", dsConsentDetail.Tables["PATIENT"].Rows[0]["ADDRSTR"].ToString() + " "
                                        + dsConsentDetail.Tables["PATIENT"].Rows[0]["ADDRCT"].ToString() + " " +
                                        dsConsentDetail.Tables["PATIENT"].Rows[0]["ADDRST"].ToString() + " " +
                                        dsConsentDetail.Tables["PATIENT"].Rows[0]["ADDRZP"].ToString());

                                    if (dsConsentDetail.Tables["Consent_Relationship"].Rows.Count > 0)
                                        hashtable.Add("FIRSTRDBTN", dsConsentDetail.Tables["Consent_Relationship"].Rows[0]["Description"]);
                                    if (dsConsentDetail.Tables["Consent_Relationship"].Rows.Count > 1)
                                        hashtable.Add("SECONDRDBTN", dsConsentDetail.Tables["Consent_Relationship"].Rows[1]["Description"]);
                                    if (dsConsentDetail.Tables["Consent_Relationship"].Rows.Count > 2)
                                        hashtable.Add("THIRDRDBTN", dsConsentDetail.Tables["Consent_Relationship"].Rows[2]["Description"]);
                                    if (dsConsentDetail.Tables["Consent_Relationship"].Rows.Count > 3)
                                        hashtable.Add("FOURTHRDBTN", dsConsentDetail.Tables["Consent_Relationship"].Rows[3]["Description"]);

                                    if (dsConsentDetail.Tables["Consent_Status"].Rows.Count > 0)
                                        hashtable.Add("SECONDBTN", dsConsentDetail.Tables["Consent_Status"].Rows[0]["Description"]);
                                    if (dsConsentDetail.Tables["Consent_Status"].Rows.Count > 1)
                                        hashtable.Add("THIRDBTN", dsConsentDetail.Tables["Consent_Status"].Rows[1]["Description"]);

                                    oDevice.padSign += VF_padSign;
                                    oDevice.DeviceCommand(ScreenDisplay, hashtable);
                                    oDevice.oDataSet = dsConsentDetail;
                                }
                            }
                            break;
                        case "PADREFCONST": // Nilesh Default Consent set PRIMEPOS-2868
                            {
                                logger.Debug("Still in DisplayScreen " + ScreenDisplay + " about to Queue command to Device");
                                dsAutoRefillData = dsConsentDetail;
                                if (dsConsentDetail?.Tables?.Count > 0)
                                {
                                    oDevice.DeviceCommand(ScreenDisplay, DatatableToHashTable(dsConsentDetail));
                                }
                                else
                                {
                                    logger.Debug("Consent Details not avalilable " + ScreenDisplay);
                                }
                                logger.Debug("Finishing from DisplayScreen " + ScreenDisplay);
                            }
                            break;
                    }
                    logger.Debug("DisplayScreen successful: " + ScreenDisplay);
                    if (Constant.DeviceName != Constant.Devices.Ingenico_ISC480.ToString())
                    {
                        switch (ScreenDisplay)
                        {
                            case "PADNOPP":
                            case "PADSIGN":
                                {
                                    oDevice.padSign -= VF_padSign;
                                    oDevice.padSign += VF_padSign;
                                    logger.Debug("VF PadSign Event fire");
                                }
                                break;
                        }
                        isDisplay = true;
                    }
                }
                else
                {
                    isDisplay = false;
                    logger.Debug("DisplayScreen failed: " + ScreenDisplay);
                }
                logger.Debug("Exiting DisplayScreen: " + ScreenDisplay);
            }
            catch (Exception ex)
            {
                logger.Error("Error in DisplayScreen \n" + ex.ToString());
                throw new Exception(ex.Message.ToString());
            }
            return isDisplay;
        }
        #region PRIMEPOS-2868 
        public Hashtable DatatableToHashTable(DataSet ds = null)
        {
            Hashtable data = new Hashtable();

            DataTable dtPatient = new DataTable();
            dtPatient = ds.Tables["PATIENT"];

            DataTable dtConsentStatus = new DataTable();
            dtConsentStatus = ds.Tables["Consent_Status"];

            DataTable dtConsentText = new DataTable();
            dtConsentText = ds.Tables["ConsentTextVersion"];

            DataTable dtRelation = new DataTable();
            dtRelation = ds.Tables["Consent_Relationship"];

            data.Add("pTitle", dtConsentText.Rows[0]["ConsentTextTitle"].ToString());
            data.Add("pData", dtConsentText.Rows[0]["ConsentText"].ToString());

            data.Add("pName", dtPatient.Rows[0]["FNAME"].ToString() + " " + dtPatient.Rows[0]["LNAME"].ToString());
            data.Add("pAddress", dtPatient.Rows[0]["ADDRSTR"].ToString() + " ," + dtPatient.Rows[0]["ADDRCT"].ToString() + " ," + dtPatient.Rows[0]["ADDRST"].ToString() + " ," + dtPatient.Rows[0]["ADDRZP"].ToString());


            data.Add("Button1", dtConsentStatus.Rows[0]["Description"].ToString());
            data.Add("Button2", dtConsentStatus.Rows[1]["Description"].ToString());


            int j = 1;
            foreach (DataRow dr in dtRelation.Rows)
            {
                data.Add("pOptions" + j, dr["Relation"].ToString());
                j++;
            }

            return data;
        }
        #endregion
        void VF_MsrSwiper(ArrayList MsrBlock)
        {
            logger.Debug("In VF_MsrSwiper()");
            try
            {
                if (!Constant.IsMsrEvent)
                    return;

                Constant.IsMsrEvent = false;
                oDevice.MsrSwiper -= VF_MsrSwiper;

                _CCData = new ArrayList();

                if (MsrBlock != null && MsrBlock.Count > 1)
                {
                    _CCData = MsrBlock;
                    logger.Debug("MsrSwiper: " + _CCData.Count);
                }
                else
                {
                    _CCData = Constant.CCDataSwipe;
                    logger.Debug("MsrSwiper Block null but data available:  " + _CCData.Count);
                }

                if (_CCData != null && _CCData.Count > 0)
                {
                    _dataEventType = "EVENTCC";
                    Constant.PadAck = true;
                }

            }
            catch (Exception ex)
            {
                logger.Error(ex, "Error in OposMSR_msr Comm \n " + ex.Message);
            }
        }

        void Constant_ErrorEvent(bool v)
        {
            logger.Debug("In Constant_errorEvent()");
            logger.Debug("Reopen Event Activated Begin");
            Constant.ErrorEvent -= Constant_ErrorEvent;
            oDevice.padSign -= VF_padSign;
            oDevice.padSign += VF_padSign;

            oDevice.PinData -= VF_PinData;
            oDevice.PinData += VF_PinData;

            oDevice.MsrSwiper -= VF_MsrSwiper;
            oDevice.MsrSwiper += VF_MsrSwiper;
            logger.Debug("Reopen Event Activated Exiting");
        }



        /// <summary>
        /// Display screen with more than one item at a time
        /// </summary>
        /// <param name="curScreen"></param>
        /// <param name="hData"></param>
        /// <returns></returns>
        private bool DisplayScreen(string curScreen, Hashtable hData)
        {
            logger.Debug("In DisplayScreen()");
            bool isDisplay = false;
            try
            {
                logger.Debug("Entering DisplayScreen 2: " + curScreen);

                if (!Constant.isErrorEventActivated)
                {
                    Constant.ErrorEvent -= Constant_ErrorEvent;
                    Constant.ErrorEvent += Constant_ErrorEvent;
                    Constant.isErrorEventActivated = true;
                }
                string ScreenDisplay = ScreenToDisplay(curScreen.ToUpper().Trim());
                switch (ScreenDisplay)
                {
                    case "PADSHOWCASH":
                    case "PADOTC":
                    case "PADRXLIST":
                    case "PADITEMLIST":
                        {
                            logger.Debug("Still in DisplayScreen 2: " + ScreenDisplay + " about to Queue command to Device");
                            oDevice.DeviceCommand(ScreenDisplay, hData);
                            logger.Debug("Finishing DisplayScreen 2: " + ScreenDisplay);
                        }
                        break;
                    case "FA_PINE":
                        {
                            logger.Debug("Still in DisplayScreen 2: " + ScreenDisplay + " Queue command to Device");
                            if (hData.Contains("CCinfo"))
                                Constant.ccinfo = hData["CCinfo"].ToString();
                            else
                                logger.Debug("===> No CC found to be encrypted with PINPAD");
                            oDevice.DeviceCommand(ScreenDisplay, hData);
                            logger.Debug("Still in DisplayScreen 2 about to activate PINPAD: " + ScreenDisplay);
                            oDevice.PinData -= VF_PinData;
                            oDevice.PinData += VF_PinData;

                            logger.Debug("Finishing DisplayScreen 2: " + ScreenDisplay);
                        }
                        break;
                }
                logger.Debug("Exiting DisplayScreen 2");
                switch (ScreenDisplay)
                {
                    case "PADOTC":
                    case "PADRXLIST":
                        {
                            oDevice.padSign -= VF_padSign;
                            oDevice.padSign += VF_padSign;
                            logger.Debug("VF PadSign Event fire");
                        }
                        break;
                }
                isDisplay = true;
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Error in DisplayScreen 2 " + curScreen + " \n" + ex.Message);
                throw new Exception(ex.Message.ToString());
            }
            return isDisplay;
        }

        void VF_PinData(ArrayList PinBlock)
        {
            logger.Debug("In VF_PinData()");
            try
            {
                if (!Constant.IsPinEvent)
                    return;

                oDevice.PinData -= VF_PinData;
                Constant.IsPinEvent = false;
                if (_CCData == null)
                {
                    _CCData = new ArrayList();
                }
                else
                {
                    _CCData.Clear();
                }
                _CCData = PinBlock;
                _dataEventType = "EVENTCC";
                Constant.PadAck = true;
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Error in Pin_PinBlock Event \n" + ex.Message);
            }
        }

        /// <summary>
        /// Set whatever is pass from the POS to the pad
        /// </summary>
        /// <param name="txnId"></param>
        /// <param name="curScreen"></param>
        /// <param name="aData"></param>
        /// <returns></returns>
        public bool SetMessage(string txnId, string curScreen, ArrayList aData)
        {
            logger.Debug("In SetMessage()");
            string msg = string.Empty;
            Hashtable data = new Hashtable();
            bool isSet = false;
            try
            {
                logger.Debug("Entering Comm SetMessage(POS Call)");
                switch (ScreenToDisplay(curScreen))
                {
                    case "PADOTC":
                        {
                            if (aData.Count > 1)
                            {
                                data.Add("oTcMsg", aData[0].ToString());
                                aData.RemoveAt(0);
                                data.Add("oTcItems", aData);
                                DisplayScreen(curScreen, data);
                            }
                        }
                        break;
                    case "FA_PINE":
                        {
                            if (aData.Count > 1)
                            {
                                data.Add("CCinfo", aData[1].ToString());
                                DisplayScreen(curScreen, data);
                            }
                        }
                        break;
                    default:
                        {
                            foreach (string s in aData)
                            {
                                if (!string.IsNullOrEmpty(s.Trim()))
                                {
                                    msg += s + "|";
                                }
                            }

                            if (msg.EndsWith("|"))
                            {
                                if (msg.Length > 1)
                                {
                                    msg = msg.Remove(msg.Length - 1);
                                }
                            }
                            DisplayScreen(txnId, curScreen, msg);
                        }
                        break;
                }
                logger.Debug("Exiting Comm SetMessage(POS Call), From POS: " + curScreen + " Current: " + _cScreen);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Error in SetMessage \n: " + ex.Message);
                throw new Exception(ex.Message);
            }
            return isSet;
        }

        /// <summary>
        /// Add the data pass from POS to a hashtable to pass to the device
        /// </summary>
        /// <param name="curScreen"></param>
        /// <param name="sData"></param>
        /// <returns></returns>
        private Hashtable PassingData(string cScreen, string sData)
        {
            logger.Debug("In PassingData()");
            Hashtable data = new Hashtable();
            bool isAlreadySet = false;

            try
            {
                string[] msg = sData.Split('|');
                switch (cScreen.ToUpper().Trim())
                {
                    case "PADMSGSCREEN":
                        {
                            data.Add("padMsg", string.IsNullOrEmpty(sData.Trim()) ? "Thank you" : sData); //add to pass to the device
                        }
                        break;
                    case "PADPATSIGN":
                        {
                            data.Add("PADPATSIGN", sData);
                        }
                        break;
                    case "PADSWIPECARD":
                        {
                            data.Add("swipeMsg", msg[0]);
                            data.Add("aCharge", msg[1]);
                        }
                        break;
                    case "PADSIGN":
                        {
                            data.Add("aCharge", sData);
                        }
                        break;
                    case "PADNOPP":
                        {
                            data.Add("pNopp", msg[0]);
                            data.Add("pName", msg[1]);
                            data.Add("pAddress", msg[2]);
                        }
                        break;
                    case "PADRXLIST":
                        {
                            data.Add("totalRx", msg[0]);
                            data.Add("pName", msg[1]);
                            data.Add("counselReq", msg[2]);
                        }
                        break;
                    case "PADITEMLIST":
                        {
                            if (msg.Length > 1 && msg[0].ToString().Contains("UpdateItem"))
                            {
                                data.Add("UpdateItem", msg[1]);
                                data.Add("Index", msg[2]);
                            }
                            else if (msg.Length > 1 && msg[0].ToString().Contains("RemoveItem"))
                                data.Add("removeCounterItem", msg[1]);
                            else if (msg.Length > 1 && msg[0].ToString().Contains("Sum"))
                            {
                                data.Add("subTotal", "$" + msg[1]);
                                data.Add("Discount", "$" + msg[2]);
                                data.Add("Tax", "$" + msg[3]);
                                data.Add("totalAmount", "$" + msg[4]);
                                isAlreadySet = true;
                            }
                            else if (string.IsNullOrEmpty(msg[0].ToString().Trim()))
                            {
                                data.Add("ClearItems", msg[0].ToString());
                                data.Add("subTotal", "$0.00");
                                data.Add("Discount", "$0.00");
                                data.Add("Tax", "$0.00");
                                data.Add("totalAmount", "$0.00");
                                if (Constant.dataStore != null)
                                    Constant.dataStore.Clear();
                                else
                                    Constant.dataStore = new ArrayList();
                            }
                            else if (msg.Length > 1 && msg[0].ToString().Contains("Add"))
                            {
                                data.Add("counterItem", msg[1]);
                            }

                            //Passing the sum(total) of each item at that time. Faster 
                            if (!string.IsNullOrEmpty(msg[0].ToString().Trim()) && !isAlreadySet)
                            {
                                string[] sum = Constant.ItemSum.Split('|');
                                if (sum[0].ToString().Contains("Sum"))
                                {
                                    data.Add("subTotal", "$" + sum[1]);
                                    data.Add("Discount", "$" + sum[2]);
                                    data.Add("Tax", "$" + sum[3]);
                                    data.Add("totalAmount", "$" + sum[4]);
                                }
                            }
                        }
                        break;
                    case "PADSHOWCASH":
                        {
                            if (msg.Length > 1)
                            {
                                data.Add("GrossAmount", string.IsNullOrEmpty(msg[0].ToString().Trim()) ? "$0.00" : msg[0].ToString());
                                data.Add("TaxAmount", string.IsNullOrEmpty(msg[1].ToString().Trim()) ? "$0.00" : msg[1].ToString());
                                data.Add("NetAmount", string.IsNullOrEmpty(msg[2].ToString().Trim()) ? "$0.00" : msg[2].ToString());
                                data.Add("PaidAmount", string.IsNullOrEmpty(msg[3].ToString().Trim()) ? "$0.00" : msg[3].ToString());
                                data.Add("ChangeDueAmount", string.IsNullOrEmpty(msg[4].ToString().Trim()) ? "$0.00" : msg[4].ToString());
                            }
                        }
                        break;
                    case "HEALTHIX":
                        {
                            data.Add("PharmacyName", sData);
                            break;
                        }
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Error in PassingData(): \n" + ex.Message);
                throw new Exception(ex.Message.ToString());
            }
            return data;
        }

        /// <summary>
        /// Start of each transaction.
        /// </summary>
        /// <param name="TxnId"></param>
        /// <returns></returns>
        public int StartTxn(string TxnId)
        {
            logger.Debug("In StartTxn()");
            int rtCode = 1;
            try
            {
                logger.Debug("Entering StartTxn New transaction");
                prevSubTot = -0.1M;
                Constant.TxnIdStored = "0";
                Constant.isErrorEventActivated = false;
                if (rxListTable != null)
                    rxListTable.Clear();
                else
                    rxListTable = new DataTable();

                if (Constant.dataStore != null)
                    Constant.dataStore.Clear(); //Clear the DataStore
                else
                    Constant.dataStore = new ArrayList();

                if (sumStore != null)
                    sumStore.Clear();
                else
                    sumStore = new ArrayList();

                if (rxPatInfo != null)
                    rxPatInfo.Clear();
                else
                    rxPatInfo = new Hashtable();

                if (DisplayScreen(TxnId, "ITEMLIST_SCREEN", ""))
                {
                    rtCode = 0;
                }
                logger.Debug("Exiting -StartTxn");
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Error in Comm StartTxn: \n" + ex.Message);
            }
            return rtCode;
        }

        /// <summary>
        /// Complete transaction.
        /// </summary>
        /// <param name="txnId"></param>
        /// <returns></returns>
        public int EndTxn(string txnId)
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

                rxListTable = null;
                Constant.dataStore = null;
                sumStore = null;
                rxPatInfo = null;
                rtCode = 0;
                logger.Debug("Exiting Comm EndTxn");
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Error in Comm EndTxn: \n" + ex.Message);
            }
            return rtCode;
        }
        /// <summary>
        /// Event for All screen with Signature and if any Error Occur
        /// s = signData, t = event type, f = screen (form)
        /// </summary>
        /// <param name="s"></param>
        /// <param name="t"></param>
        /// <param name="f"></param>
        void VF_padSign(string s, string t, string f)
        {
            logger.Debug("In VF_PadSign()");
            try
            {
                if (!Constant.IsPadEvent)
                    return; //If Pad Event is not true don't do anything

                logger.Debug("Entering Comm PadSignEvent Data Received from OPOSForm with Screen: " + f);
                Constant.IsPadEvent = false; //One event at a time
                switch (f.ToUpper().Trim())
                {
                    case "PADSIGN":
                        {
                            _dataEventType = "EVENTSIGN";
                            _signData = s.ToString();
                            Constant.PadAck = true;
                        }
                        break;
                    case "PADNOPP":
                        {
                            switch (t.ToUpper().Trim())
                            {
                                case Constant.NoppCancel:
                                    {
                                        _dataEventType = "EVENTNOPP";
                                        if (string.IsNullOrEmpty(s.ToString().Trim()))
                                        {
                                            _signData = "6008";
                                        }
                                        else
                                        {
                                            _signData = s.ToString();
                                        }
                                        Constant.PadAck = true;
                                    }
                                    break;
                                case Constant.NoppSkip:
                                    {
                                        _dataEventType = "EVENTNOPP";
                                        if (string.IsNullOrEmpty(s.ToString().Trim()))
                                        {
                                            _signData = "6009";
                                        }
                                        else
                                        {
                                            _signData = s.ToString();
                                        }
                                        Constant.PadAck = true;
                                    }
                                    break;
                                case Constant.Sign:
                                    {
                                        _dataEventType = "EVENTNOPP";
                                        _signData = s.ToString();
                                        Constant.PadAck = true;
                                    }
                                    break;
                            }

                        }
                        break;
                    case "PADRXLIST":
                        {
                            switch (t.ToUpper().Trim())
                            {
                                case Constant.RxDetail:
                                    {
                                        if (!Constant.DetailButtonClick)
                                        {
                                            Constant.DetailButtonClick = true;
                                            oDevice.padSign -= VF_padSign;
                                            ShowRxDetail("1", rxListTable);
                                        }
                                    }
                                    break;
                                case Constant.Sign:
                                    {
                                        _dataEventType = "EVENTRXAPPROVE";
                                        _signData = s.ToString();
                                        Constant.PadAck = true;
                                    }
                                    break;
                            }
                        }
                        break;
                    case "PADOTC":
                        {
                            _dataEventType = "EVENTOTC";
                            _signData = s.ToString();
                            Constant.PadAck = true;
                        }
                        break;
                    case "ERROR":
                        {
                            oDevice.padSign -= VF_padSign;
                            oDevice.padSign += VF_padSign;
                        }
                        break;
                    case "PATIENTCONSENT"://PRIMEPOS-2867 Consent
                        {
                            //Constant.PadAck = true;
                            //if (t.ToUpper() == "ACCEPT")
                            //{
                            ClearPosPadData();
                            PtConent = new PatientConsent();
                            PtConent = oDevice.VerifonePatConsent;
                            //}
                            //else
                            //{
                            //    PtConent = new PatientConsent();
                            //}
                        }
                        break;
                }
                logger.Debug("Exiting Comm PadSignEvent: " + Constant.PadAck);
            }
            catch (Exception ex)
            {
                logger.Error(ex, ex.Message);
                throw new Exception(ex.Message.ToString());
            }
        }


        /// <summary>
        /// Return the Current Screen on the device
        /// </summary>
        /// <param name="txnId"></param>
        /// <param name="curScreen"></param>
        public void GetCurrentScreen(string txnId, out string curScreen)
        {
            logger.Debug("In GetCurrentScreen()");
            string dScreen = string.Empty;
            lock (locker)
            {
                if (oDevice != null)
                {
                    dScreen = oDevice.GetLoadedForm();
                }
                curScreen = string.IsNullOrWhiteSpace(dScreen) ? _cScreen : dScreen; //PRIMEPOS-3214
                _cScreen = curScreen;
            }
        }

        public void SetUserCancelled()
        {

        }

        /// <summary>
        /// This is the NOPP function.
        /// </summary>
        /// <param name="txnId"></param>
        /// <param name="patientName"></param>
        /// <param name="patientAddress"></param>
        /// <param name="noppMsg"></param>
        /// <returns></returns>
        public bool InitiateNoppSign(string txnId, string patientName, string patientAddress, string noppMsg)
        {
            logger.Debug(string.Format("In InitiateNoppSign()"));
            bool isInit = false;

            try
            {
                lock (locker)
                {
                    logger.Debug("Entering Comm NOPPSIGN: " + patientName);
                    string noppData = noppMsg + "|" + patientName + "|" + patientAddress;
                    if (DisplayScreen(txnId, "PADNOPP", noppData))
                    {
                        isInit = true;
                    }
                    logger.Debug("Exiting Comm NOPPSIGN: " + patientName + " isDisplay? " + isInit);
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Error in Comm InitiateNoppSign \n" + ex.Message);
            }
            return isInit;
        }



        /// <summary>
        /// RX Screen with signature.
        /// </summary>
        /// <param name="txnId"></param>
        /// <param name="totalRx"></param>
        /// <param name="pName"></param>
        /// <param name="counsel"></param>
        /// <param name="rxList"></param>
        /// <returns></returns>
        public bool RxSign(string txnId, string totalRx, string pName, string counsel, DataTable rxList, bool bHidePatCounseling = false)
        {
            logger.Debug(string.Format("In RxSign()"));
            bool isRxSign = false;
            Hashtable fields = new Hashtable();
            ArrayList rxitems = new ArrayList();
            string curScreen = "PADRXLIST";





            try
            {
                lock (locker)
                {
                    logger.Debug("Entering Comm RxSign: " + pName);
                    if (rxListTable != null && rxListTable.Rows.Count > 0)
                    {
                        rxListTable.Clear();
                    }
                    else if (rxListTable == null)
                    {
                        rxListTable = new DataTable();
                    }

                    rxListTable = rxList.Copy();//addrx() line 801
                    logger.Debug("In Comm RxSign Total Rx's: " + rxListTable.Rows.Count);
                    Constant.Counsel = counsel; //orignial Patient counseling from POS
                    if (bHidePatCounseling)
                        Constant.Counsel = "NC";
                    string RxData = totalRx + "|" + pName + "|" + Constant.Counsel;

                    for (int i = 0; i < rxList.Rows.Count; i++)
                    {
                        fields.Add("RXNO", string.IsNullOrEmpty(rxList.Rows[i]["RXNO"].ToString().Trim()) ? "000000" : rxList.Rows[i]["RXNO"].ToString());
                        logger.Debug("In Comm RxSign: " + (string.IsNullOrEmpty(rxList.Rows[i]["RXNO"].ToString().Trim()) ? "000000" : rxList.Rows[i]["RXNO"].ToString()));
                        if (Convert.ToBoolean(rxList.Rows[i]["SHOWRXDESCRIPTION"].ToString()) == false)
                        {
                            fields.Add("DRUGNAME", "****");
                        }
                        else
                        {
                            fields.Add("DRUGNAME", string.IsNullOrEmpty(rxList.Rows[i]["DRUGNAME"].ToString().Trim()) ? "Not Available" : rxList.Rows[i]["DRUGNAME"].ToString());
                        }
                        fields.Add("RXDATE", string.IsNullOrEmpty(rxList.Rows[i]["RXDATE"].ToString().Trim()) ? "00000000" : rxList.Rows[i]["RXDATE"].ToString());
                        string itemRow = PrepareRxString(fields);
                        rxitems.Add(itemRow);
                        if (fields != null)
                            fields.Clear();
                    }
                    Constant.DetailButtonClick = false;
                    rxPatInfo = PassingData(curScreen, RxData);
                    fields = PassingData(curScreen, RxData);
                    fields.Add("rxItem", rxitems);

                    if (DisplayScreen(curScreen, fields))
                    {
                        isRxSign = true;
                    }
                    logger.Debug("Exiting Comm RxSign: " + pName);
                    return isRxSign;
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Error in Comm RxSign \n" + ex.Message);
                throw new Exception(ex.Message.ToString());
            }
        }

        /// <summary>
        /// Add Items from the POS into the PAD
        /// </summary>
        /// <param name="TxnID"></param>
        /// <param name="Row"></param>
        /// <param name="RowIndex"></param>
        /// <param name="ShowRxDec"></param>
        /// <returns></returns>
        public bool AddItems(string TxnID, DataRow Row, int RowIndex, string ShowRxDec)
        {
            logger.Debug(string.Format("In AddItems()"));
            bool isAdded = false;
            try
            {
                lock (locker)
                {
                    logger.Debug("Entering Comm AddItems() TransID: " + TxnID);
                    Hashtable fields = new Hashtable();

                    fields.Add("ItemName", string.IsNullOrEmpty(Row["ItemDescription"].ToString().Trim()) ? "UnKnown" : Row["ItemDescription"].ToString());
                    fields.Add("ItemQty", Row["QTY"].ToString());
                    fields.Add("UnitPrice", Convert.ToDouble(Row["Price"].ToString()).ToString("F", CI).PadRight(2, '0'));
                    fields.Add("TotalPrice", Convert.ToDouble(Row["Discount"].ToString()).ToString("F", CI).PadRight(2, '0'));
                    fields.Add("ISRX", Row["ItemID"].ToString() == "RX" ? 1 : 0);
                    fields.Add("SHOWRXDESCRIPTION", ShowRxDec);
                    double TotalAmountWithDisc = Convert.ToDouble(Row["ExtendedPrice"].ToString()) - Convert.ToDouble(Row["Discount"].ToString());
                    fields.Add("WITHDISCAMOUNT", TotalAmountWithDisc.ToString("F", CI).PadRight(2, '0'));

                    string ItemRow = "Add |" + PrepareItemString(fields);
                    logger.Debug("Item Added: " + ItemRow);

                    if (Constant.dataStore == null)
                        Constant.dataStore = new ArrayList();

                    if (Constant.dataStore != null)
                    {
                        Constant.dataStore.Add(ItemRow);
                        logger.Debug("Comm AddItem ItemCount: " + Constant.dataStore.Count + " Item: " + ItemRow);
                        if (DisplayScreen(TxnID, "ITEMLIST_SCREEN", ItemRow))
                        {
                            isAdded = true;
                        }
                    }
                    logger.Debug("Exiting Comm AddItem: " + isAdded + " ItemCOunt: " + Constant.dataStore.Count + " Item: " + ItemRow);
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Error in Comm AddItems() \n" + ex.Message);
                throw new Exception(ex.Message.ToString());
            }
            return isAdded;
        }

        /// <summary>
        /// Update the sum from the POS
        /// </summary>
        /// <param name="txnId"></param>
        /// <param name="itemRow"></param>
        /// <returns></returns>
        public bool UpdateSum(string txnId, string itemRow)
        {
            logger.Debug(string.Format("In UpdateSum()"));
            bool isSumUpd = false;
            string sum = string.Empty;
            decimal subTot = 0.0M;
            try
            {
                logger.Debug("In UpdateSum()");
                string[] total = itemRow.Split('%');
                subTot = Convert.ToDecimal(total[3].ToString());
                if (subTot == prevSubTot)
                {
                    return true;
                }
                logger.Debug("Entering Comm Update Sum");
                sum = "Sum |" + itemRow.Replace('%', '|'); //Replace the % with |
                Constant.ItemSum = sum;
                isSumUpd = true;
                prevSubTot = subTot;
                logger.Debug("Exiting Comm Update Sum: " + isSumUpd);

            }
            catch (Exception ex)
            {
                logger.Error(ex, "Error in Comm Update Sum \n" + ex.Message);
                throw new Exception(ex.Message.ToString());
            }
            return isSumUpd;
        }

        /// <summary>
        /// Update selected Item
        /// </summary>
        /// <param name="txnId"></param>
        /// <param name="index"></param>
        /// <param name="fields"></param>
        /// <returns></returns>
        //public bool UpdateItem(string txnId, int index, Hashtable fields)
        public bool UpdateItem(string txnId, DataRow UpdateRow, int index, string ShowRxDec)
        {
            logger.Debug(string.Format("In UpdateItem()"));
            string Upd = "UpdateItem";
            bool isUpdate = false;

            try
            {
                lock (locker)
                {
                    logger.Debug("Entering Comm UpdateItem: index = " + index);
                    Hashtable fields = new Hashtable();
                    fields.Add("ItemName", string.IsNullOrEmpty(UpdateRow["ItemDescription"].ToString().Trim()) ? "Not Available" : UpdateRow["ItemDescription"].ToString());
                    fields.Add("ItemQty", UpdateRow["QTY"].ToString());
                    fields.Add("UnitPrice", Convert.ToDouble(UpdateRow["Price"].ToString()).ToString("F", CI).PadRight(2, '0'));
                    fields.Add("TotalPrice", Convert.ToDouble(UpdateRow["Discount"].ToString()).ToString("F", CI).PadRight(2, '0'));
                    fields.Add("ISRX", UpdateRow["ItemID"].ToString() == "RX" ? 1 : 0);
                    fields.Add("SHOWRXDESCRIPTION", ShowRxDec);
                    double TotalAmountWithDisc = Convert.ToDouble(UpdateRow["ExtendedPrice"].ToString()) - Convert.ToDouble(UpdateRow["Discount"].ToString());
                    fields.Add("WITHDISCAMOUNT", TotalAmountWithDisc.ToString("F", CI).PadRight(2, '0'));

                    string itemRow = PrepareItemString(fields); //Prepare the string
                    logger.Debug("Item Updated: " + itemRow);
                    if (Constant.dataStore != null && Constant.dataStore.Count > 0)
                    {
                        if (Constant.dataStore.Count > index)
                        {
                            Constant.dataStore.RemoveAt(index); //remove item at this index
                            Constant.dataStore.Insert(index, itemRow); //after remove insert the new item
                            itemRow = Upd + "|" + itemRow + "|" + index;

                            if (DisplayScreen(txnId, "ITEMLIST_SCREEN", itemRow))
                            {
                                isUpdate = true;
                            }
                        }
                    }
                    logger.Debug("Exiting Comm UpdateItem: " + isUpdate + " Store count: " + Constant.dataStore.Count);
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Error in Comm UpdateItem, DataStore: " + Constant.dataStore.Count + " ,Index: " + index + "\n" + ex.Message);
                throw new Exception(ex.Message.ToString());
            }
            return isUpdate;
        }

        /// <summary>
        /// Delete the selected Item 
        /// </summary>
        /// <param name="txnId"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public bool DeleteItem(string txnId, int index)
        {
            logger.Debug(string.Format("In DeleteItem()"));
            string strIndex = string.Empty;
            string delItem = "RemoveItem";
            bool isDelete = false;

            try
            {
                lock (locker)
                {
                    logger.Debug("Entering DeleteItem(), Index to deleted is: " + index);
                    if (Constant.dataStore != null && Constant.dataStore.Count > 0)
                    {
                        if (Constant.dataStore.Count > index)
                        {
                            Constant.dataStore.RemoveAt(index); //Remove at this index
                            strIndex = delItem + "|" + Convert.ToString(index);

                            if (DisplayScreen(txnId, "ITEMLIST_SCREEN", strIndex))
                            {
                                isDelete = true;
                            }
                        }
                        else
                        {
                            logger.Debug("DeleteItem() error. Index: " + index);
                        }
                    }
                    logger.Debug("Exiting Comm DeleteItem: " + isDelete + " Index: " + index);
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Error in Comm DeleteItem \n" + ex.Message);
                throw new Exception(ex.Message.ToString());
            }
            return isDelete;
        }

        /// <summary>
        /// Resend all data to the device 
        /// </summary>
        /// <param name="txnId"></param>
        /// <param name="flag"></param>
        /// <returns></returns>
        public bool ResendAllItem(string txnId, string flag)
        {
            logger.Debug(string.Format("In ResendAllItem()"));
            Hashtable data = new Hashtable();
            ArrayList resenditem = new ArrayList();
            bool isResend = false;

            try
            {
                lock (locker)
                {
                    logger.Debug("Entering Comm ResendAllItem");
                    if (Constant.dataStore != null && Constant.dataStore.Count > 0)
                    {
                        logger.Debug("Total items to Resend: " + Constant.dataStore.Count);
                        foreach (string item in Constant.dataStore)
                        {
                            string[] itemsplit = item.Split('|');
                            if (itemsplit.Length > 1)
                                resenditem.Add(itemsplit[1]);// add each item to the arraylist
                            else
                                resenditem.Add(item);
                        }
                        data.Add("resendCounter", resenditem); // add arraylist to hashtable for quick display in the pad

                        if (data != null && data.Count > 0)
                            logger.Debug("Total Items in Resend list to send: " + data.Count);
                        else
                            logger.Debug("No Item in Resend list");

                        foreach (DictionaryEntry d in PassingData("PADITEMLIST", Constant.ItemSum))
                        {
                            data.Add(d.Key, d.Value);
                        }

                        logger.Debug("Before Display Resend items");

                        if (DisplayScreen("ITEMLIST_SCREEN", data))
                        {
                            isResend = true;
                        }
                        logger.Debug("After Display Resend items: " + isResend);
                    }
                    logger.Debug("Exiting Comm ResendAllItem: " + isResend);
                    return isResend;
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Error in Comm ResendAllItem \n" + ex.Message);
                throw new Exception(ex.Message.ToString());
            }

        }

        /// <summary>
        /// Display On Hold Items on the Pad.
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public bool DisplayOnHoldItem(DataTable dt, string ShowRxDesc)
        {
            logger.Debug(string.Format("In DisplayOnHoldItem()"));
            bool isDisplayOnHold = false;
            ArrayList OnHoldItem = new ArrayList();
            Hashtable data = new Hashtable();
            Hashtable fields = new Hashtable();

            try
            {
                lock (locker)
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
                            if (fields != null)
                            {
                                fields.Clear();
                            }
                            fields.Add("ItemName", dr["ItemDescription"].ToString());
                            fields.Add("ItemQty", dr["QTY"].ToString());
                            fields.Add("UnitPrice", Convert.ToDouble(dr["Price"].ToString()).ToString("F", CI).PadRight(2, '0'));
                            fields.Add("TotalPrice", Convert.ToDouble(dr["Discount"].ToString()).ToString("F", CI).PadRight(2, '0'));
                            fields.Add("ISRX", dr["ItemID"].ToString().ToUpper() == "RX" ? 1 : 0);
                            fields.Add("SHOWRXDESCRIPTION", ShowRxDesc);
                            double TotalWithDis = Convert.ToDouble(dr["ExtendedPrice"].ToString()) - Convert.ToDouble(dr["Discount"].ToString());
                            fields.Add("WITHDISCAMOUNT", TotalWithDis.ToString("F", CI).PadRight(2, '0'));
                            string RowItem = PrepareItemString(fields);
                            OnHoldItem.Add(RowItem); //set the item to string
                            Constant.dataStore.Add(RowItem);
                        }
                        data.Add("OnHoldItems", OnHoldItem); //add the arraylist to the Hashtable for passing to pad

                        foreach (DictionaryEntry d in PassingData("PADITEMLIST", Constant.ItemSum))
                        {
                            data.Add(d.Key, d.Value);
                        }

                        if (DisplayScreen("ITEMLIST_SCREEN", data))
                        {
                            isDisplayOnHold = true;
                        }
                    }
                    logger.Debug("Exiting DisplayOnHoldItem(): " + isDisplayOnHold);
                    return isDisplayOnHold;
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Error In Comm DisplayOnHoldItem(): \n" + ex.Message);
                throw new Exception(ex.ToString());
            }
        }

        /// <summary>
        /// Cash Transaction on the device, POS pass these values
        /// </summary>
        /// <param name="TxnId"></param>
        /// <param name="gAmount"></param>
        /// <param name="taxAmount"></param>
        /// <param name="netAmount"></param>
        /// <param name="paidAmount"></param>
        /// <param name="changeDueAmount"></param>
        /// <returns></returns>
        public bool ProcessCashTxn(string TxnId, string gAmount, string taxAmount, string netAmount, string paidAmount, string changeDueAmount)
        {
            logger.Debug(string.Format("In ProcessCashTxn()"));
            bool isProcess = false;
            string cScreen = string.Empty;
            try
            {
                if (Constant.TxnIdStored.Trim() == TxnId.Trim())
                {
                    logger.Debug("ProcessCashTxn same TxnID as PREVIOUS");
                    return isProcess;
                }
                Constant.TxnIdStored = TxnId.Trim();

                lock (locker)
                {
                    logger.Debug("Entering Comm ProcessCashTxn");
                    if (!Constant.ShowPaymentScreen)
                    {
                        cScreen = "TXTMESSAGE_SCREEN";
                        string msg = "Thank you for your payment: $" + paidAmount;

                        if (DisplayScreen(TxnId, cScreen, msg))
                        {
                            isProcess = true;
                        }
                        logger.Debug("Exiting Comm ProcessCashTxn: " + isProcess);
                    }
                    else
                    {
                        cScreen = "PADSHOWCASH";
                        if (cashtxn != null)
                        {
                            cashtxn.Clear();
                        }
                        else
                        {
                            cashtxn = new Hashtable();
                        }
                        cashtxn.Add("GrossAmount", string.IsNullOrEmpty(gAmount.Trim()) ? "0.00" : gAmount);
                        cashtxn.Add("TaxAmount", string.IsNullOrEmpty(taxAmount.Trim()) ? "0.00" : taxAmount);
                        cashtxn.Add("NetAmount", string.IsNullOrEmpty(netAmount.Trim()) ? "0.00" : netAmount);
                        cashtxn.Add("PaidAmount", string.IsNullOrEmpty(paidAmount.Trim()) ? "0.00" : paidAmount);
                        cashtxn.Add("ChangeDueAmount", string.IsNullOrEmpty(changeDueAmount.Trim()) ? "0.00" : changeDueAmount);

                        logger.Debug("Comm ProcessCashTxn With data populate: " + cashtxn.Count.ToString());
                        if (DisplayScreen(cScreen, cashtxn))
                        {
                            isProcess = true;
                        }
                        logger.Debug("Exiting Comm ProcessCashTxn: " + isProcess + " With data populate: " + cashtxn.Count.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Error in Comm ProcessCashTxn \n" + ex.Message);
                throw new Exception(ex.Message.ToString());
            }
            finally
            {
                Constant.TxnIdStored = "0";
            }
            return isProcess;
        }

        /// <summary>
        /// If the user click Detail button then it will show everything
        /// </summary>
        /// <param name="txnId"></param>
        /// <param name="rxlist"></param>
        /// <returns></returns>
        public bool ShowRxDetail(string txnId, DataTable rxList)
        {

            logger.Debug(string.Format("In ShowRxDetail()"));
            Hashtable fields = new Hashtable();
            Hashtable sFields = new Hashtable();
            ArrayList rxItems = new ArrayList();
            bool isUpdated = false;
            string curScreen = "PADRXLIST";

            try
            {
                lock (locker)
                {
                    logger.Debug("Entering Comm ShowRxDetail");
                    for (int i = 0; i < rxList.Rows.Count; i++)
                    {
                        fields.Add("RXNO", string.IsNullOrEmpty(rxList.Rows[i]["RXNO"].ToString().Trim()) ? "000000" : rxList.Rows[i]["RXNO"].ToString());
                        fields.Add("DRUGNAME", string.IsNullOrEmpty(rxList.Rows[i]["DRUGNAME"].ToString().Trim()) ? "No Available" : rxList.Rows[i]["DRUGNAME"].ToString());
                        fields.Add("RXDATE", string.IsNullOrEmpty(rxList.Rows[i]["RXDATE"].ToString().Trim()) ? "00000000" : rxList.Rows[i]["RXDATE"].ToString());

                        string itemRow = PrepareRxString(fields);
                        rxItems.Add(itemRow);
                        if (fields != null)
                            fields.Clear();
                    }

                    if (sFields != null && sFields.Count >= 0)
                    {
                        sFields.Clear();
                    }
                    else
                    {
                        sFields = new Hashtable();
                    }

                    if (rxPatInfo.ContainsKey("rxItem"))
                    {
                        rxPatInfo.Remove("rxItem");
                    }

                    sFields = rxPatInfo; //Rx patient info to show
                    sFields.Add("ShowRx", rxItems);

                    if (DisplayScreen(curScreen, sFields))
                    {
                        isUpdated = true;
                    }
                    logger.Debug("Exiting Comm ShowRxDetail: " + isUpdated);
                    return isUpdated;
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex, ex.Message);
                throw new Exception(ex.Message.ToString());
            }
        }


        /// <summary>
        /// Prepare a string to display on the device: Item
        /// </summary>
        /// <param name="fields"></param>
        /// <returns></returns>
        private string PrepareItemString(Hashtable fields)
        {
            string ItemName = string.Empty;
            string IsRX = string.Empty;
            Hashtable itemDet = null;
            try
            {
                itemDet = new Hashtable();
                ItemName = string.Format("{0,-15}", string.IsNullOrEmpty(fields["ItemName"].ToString().Trim()) ? "Not Available" : fields["ItemName"].ToString());
                if (string.IsNullOrEmpty(fields["ISRX"].ToString().Trim()))
                    IsRX = "0";
                else
                    IsRX = fields["ISRX"].ToString();

                if (IsRX == "1")
                {
                    string[] strArray = ItemName.Split('-');
                    if (strArray.Length > 0 && Convert.ToBoolean(fields["SHOWRXDESCRIPTION"].ToString()) == false)
                    {
                        itemDet["ITEMNAME"] = string.Format("{0,-15}", strArray[0] + "-****");
                    }
                    else
                    {
                        itemDet["ITEMNAME"] = string.Format("{0,-15}", string.IsNullOrEmpty(fields["ItemName"].ToString().Trim()) ? "Not Available" : fields["ItemName"].ToString());
                    }
                }
                else
                {
                    itemDet["ITEMNAME"] = string.Format("{0,-15}", string.IsNullOrEmpty(fields["ItemName"].ToString().Trim()) ? "Not Available" : fields["ItemName"].ToString());
                }

                itemDet["ITEMQTY"] = string.Format("{0,5}", fields["ItemQty"].ToString());
                itemDet["UNITPRICE"] = string.Format("{0,9}", "$" + fields["UnitPrice"].ToString());
                itemDet["TOTALPRICE"] = string.Format("{0,9}", "$" + fields["TotalPrice"].ToString());
                itemDet["WITHDISCAMOUNT"] = string.Format("{0,9}", "$" + fields["WITHDISCAMOUNT"].ToString());
                if (Constant.DeviceName == Constant.Devices.VerifoneMX925WithPinPad.ToString())
                {
                    return string.Format("{0}{1,9}{2,17}{3,16}{4,14}", itemDet["ITEMNAME"].ToString().Remove(Constant.itemNameLen), itemDet["ITEMQTY"].ToString(), itemDet["UNITPRICE"].ToString(), itemDet["TOTALPRICE"].ToString(), itemDet["WITHDISCAMOUNT"].ToString());
                }
                else if ((Constant.DeviceName == Constant.Devices.Ingenico_ISC480.ToString()) || (Constant.DeviceName == Constant.Devices.WPIngenico_ISC480.ToString()))
                {
                    return string.Format("{0}{1,3}{2,12}{3,14}{4,30}", itemDet["ITEMNAME"].ToString().Remove(Constant.itemNameLen), itemDet["ITEMQTY"].ToString(), itemDet["UNITPRICE"].ToString(), itemDet["TOTALPRICE"].ToString(), itemDet["WITHDISCAMOUNT"].ToString());
                }
                else
                {
                    return string.Format("{0}{1}{2}{3}{4}", itemDet["ITEMNAME"].ToString().Remove(Constant.itemNameLen), itemDet["ITEMQTY"].ToString(), itemDet["UNITPRICE"].ToString(), itemDet["TOTALPRICE"].ToString(), itemDet["WITHDISCAMOUNT"].ToString());
                }
            }
            catch (Exception ex)
            {
                logger.Debug("Error in Comm PrepareItemString \n" + ex.ToString());
                throw new Exception(ex.Message.ToString());
            }
        }

        /// <summary>
        /// Prepare a string to display on the device: RX
        /// </summary>
        /// <param name="fields"></param>
        /// <returns></returns>
        private string PrepareRxString(Hashtable fields)
        {
            Hashtable RxDet = null;
            try
            {
                RxDet = new Hashtable();
                RxDet["RXNO"] = string.Format("{0,-10}", fields["RXNO"].ToString());
                RxDet["DRUGNAME"] = string.Format("{0,-15}", fields["DRUGNAME"].ToString().PadRight(Constant.itemNameLen, ' '));
                RxDet["RXDATE"] = string.Format("{0,14}", fields["RXDATE"].ToString());

                if (Constant.DeviceName == Constant.Devices.VerifoneMX925WithPinPad.ToString())
                {
                    return string.Format("{0}{1,17}{2,23}", RxDet["RXNO"].ToString(), RxDet["DRUGNAME"].ToString().Remove(Constant.itemNameLen), RxDet["RXDATE"].ToString());
                }
                else if (Constant.DeviceName == Constant.Devices.Ingenico_ISC480.ToString())
                {
                    return string.Format("{0}{1,17}{2,23}", RxDet["RXNO"].ToString(), RxDet["DRUGNAME"].ToString().Remove(Constant.itemNameLen), RxDet["RXDATE"].ToString());
                }
                else
                {
                    return string.Format("{0}{1}{2}", RxDet["RXNO"].ToString(), RxDet["DRUGNAME"].ToString().Remove(Constant.itemNameLen), RxDet["RXDATE"].ToString());
                }
            }
            catch (Exception ex)
            {
                logger.Debug("Error in Comm PrepareRxString \n" + ex.ToString());
                throw new Exception(ex.Message.ToString());
            }
        }

        /// <summary>
        /// Get with PAD screen corresponding to the screen pass from POS
        /// </summary>
        /// <param name="screenName"></param>
        /// <returns></returns>
        private string ScreenToDisplay(string screenName)
        {
            string dispScreen = string.Empty;
            try
            {
                switch (screenName.ToUpper().Trim())
                {
                    case "TXTMESSAGE_SCREEN":
                        {
                            dispScreen = "PADMSGSCREEN";
                        }
                        break;
                    case "ITEMLIST_SCREEN":
                        {
                            dispScreen = "PADITEMLIST";
                        }
                        break;
                    case "XLINK":
                        {
                            dispScreen = "XLINK";
                        }
                        break;
                    case "SWIPE_SCREEN":
                        {
                            if (Constant.DeviceName == Constant.Devices.Ingenico_ISC480.ToString())
                                dispScreen = "PADSWIPED";
                            else
                                dispScreen = "PADSWIPECARD";
                        }
                        break;
                    case "SIGN_SCREEN":
                        {
                            dispScreen = "PADSIGN";
                        }
                        break;
                    case "OTC_SIGN_SCREEN":
                        {
                            dispScreen = "PADOTC";
                        }
                        break;
                    case "PADRXLIST":
                        {
                            dispScreen = "PADRXLIST";
                        }
                        break;
                    case "PADNOPP":
                        {
                            dispScreen = "PADNOPP";
                        }
                        break;
                    case "PIN_PAD":
                        {
                            dispScreen = "FA_PINE";
                        }
                        break;
                    case "PADSHOWCASH":
                        {
                            dispScreen = "PADSHOWCASH";
                        }
                        break;
                    case "PATIENTCOUNSEL":
                        {
                            dispScreen = "PADREMINDER";
                            break;
                        }
                    case "PATIENTCOUNSELN":
                        {
                            dispScreen = "PADREMINDN";
                            break;
                        }
                    case "PADPATSIGN":
                        {
                            dispScreen = "PADPATSIGN";
                            break;
                        }
                    case "HEALTHIX":
                        {
                            dispScreen = "HEALTHIX";
                            break;
                        }
                    default: // PRIMEPOS-2868 Added for defualt consent
                        {
                            // DEFAULT NAME 
                            if (Constant.DeviceName == Constant.Devices.WPIngenico_ISC480.ToString())
                            {
                                dispScreen = "PADREFCONST";
                            }
                            else if (Constant.DeviceName == Constant.Devices.VerifoneMX925WithPinPad.ToString())//PRIMEPOS-2867 ARVIND
                            {
                                dispScreen = "PATIENTCONSENT";
                            }
                        }
                        break;
                }
                _cScreen = dispScreen.ToUpper().Trim(); //current screen that should be on the pad.
            }
            catch (Exception ex) { logger.Error(ex, ex.Message); }
            return dispScreen.ToUpper().Trim();
        }

        /// <summary>
        /// Enable and Disable Devices 
        /// </summary>
        enum xSwitch
        {
            ON,
            OFF
        }

        /// <summary>
        /// Use to Release and Open Device for Xcharge only.
        /// </summary>
        /// <param name="curScreen"></param>
        /// <param name="sData"></param>
        private void XCharge(string curScreen, string sData)
        {
            bool isClose = false;
            bool isOpen = false;

            try
            {
                logger.Debug("Entering Comm Xcharge: " + sData);
                if (Constant.DeviceName == Constant.Devices.Ingenico_ISC480.ToString())
                {
                    logger.Debug("iSC Series Devices");
                    if (oDevice != null && sData.ToUpper().Trim() == xSwitch.ON.ToString())
                    {
                        //close 
                    }
                    else
                    {
                        //open
                    }
                }
                else
                {
                    logger.Debug("Mx Series Devices");
                    if (oDevice != null && sData.ToUpper().Trim() == "ON")
                    {
                        isClose = oDevice.CloseDevice(); // close the OPOS device object
                        logger.Debug("Closing Device for Xcharge");
                    }
                    else if (oDevice != null && sData.ToUpper().Trim() == "OFF")
                    {
                        if (!Constant.IsDeviceOpen)
                        {
                            isOpen = oDevice.EnableDevice(); //Open the OPOS device object
                        }
                        logger.Debug("Open Device after Xcharge close");
                    }
                }
                logger.Debug("Exiting Comm Xcharge");
            }
            catch (Exception ex)
            {
                logger.Error(ex, ex.Message);
                throw new Exception(ex.ToString());
            }
        }

        public void ClosePort()
        {
            try
            {
                lock (locker)
                {
                    logger.Debug("Entering ClosePort() - Pos Close");
                    oDevice.CloseDevice(); // close the OPOS device object
                    logger.Debug("Exiting ClosePort()");
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex, ex.Message);
                throw new Exception(ex.ToString());
            }
        }
        #endregion
    }
}
