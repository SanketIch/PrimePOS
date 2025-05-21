using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Xml;
using System.Diagnostics;
using System.IO;
using Microsoft.Win32;
using System.Threading.Tasks;
using System.Threading;
using System.Collections;
using VCONTROLOBJECTLib;
using NLog;

namespace MMS.Device
{
    /// <Author>Author: Manoj Kumar</Author>
    /// <summary>
    /// Description: This is the OPOS Library for the Verifone MX870 Device.
    /// All Device communication is done in this class.
    /// </summary>
    public class OPOSForm
    {
        ILogger logger = LogManager.GetCurrentClassLogger();
        #region Variable and Properties
        private FormClass OposForm = null;
        private RBA_ISC_Device iSC = null;
        private OPOSsig sigCap = null;
        private string _currentScreen;
        private string signScreen;
        OPOSPinPad Pin = null;
        OPOSMsr OposMSR = null; //Swiper
        public delegate void PinPad(ArrayList PinBlock);
        public event PinPad PinData = delegate { };
        public delegate void MsrData(ArrayList MsrBlock);
        public event MsrData MsrSwiper = delegate { };
        public delegate void VFErrorCode(bool val, int v, string desc);
        /// <summary>
        /// Event Activate if Verifone Return Error Code that is not successful
        /// </summary>
        public event VFErrorCode ReturnCodeEvent = delegate { };
        public delegate void EventHandler();
        bool isFrmInt = false;
        Queue<CommandStation> CommandQueue = new Queue<CommandStation>();
        Queue<ResendCommandStation> ResendCommandQueue = new Queue<ResendCommandStation>();
        private readonly object DeviceLock = new object();
        private readonly object ResetLock = new object();
        public delegate void padSignEvent(string s, string t, string f); //take signdata, sigType, Form Name.
        public event padSignEvent padSign = delegate { };
        delegate void OPOSErrorEventLog();
        static event OPOSErrorEventLog ErrorLog;
        delegate void CommandHandler();
        event CommandHandler CommandEvent;
        static Thread DeviceThread = null;
        Hashtable vData = null;
        object locker = new object();

        /// <summary>
        /// Get or Set the Current Screen on the Device
        /// </summary>
        public string CurrentScreen
        {
            get { return _currentScreen; }
            set { _currentScreen = value; }
        }

        public PatientConsent VerifonePatConsent//PRIMEPOS-2867 CONSENT
        {
            get;
            set;
        }
        public string radioOptionBtn = string.Empty;//PRIMEPOS-2867 Consent
        public DataSet oDataSet { get; set; } //PRIMEPOS-2867 Arvind        

        #endregion Variable and Properties

        #region Methods for the Device: Open,Close,Show, Init Form, SetProperty ...etc
        /// <summary>
        /// Initialize the device. Return True if Connected
        /// </summary>
        /// <returns></returns>
        public void Initialize()
        {
            Hashtable data = new Hashtable();
            logger.Debug("\t\tInitialize the device");
            data.Add("OPEN", "OPEN");
            DeviceCommand(Constant.WELCOME, data);
            logger.Debug("\t\tEnd Init device");
        }

        #region  PRIMEPOS-2730 - Arvind
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

        public void iSCInitialize(int ComP)
        {
            iSC = new RBA_ISC_Device();
            Hashtable data = new Hashtable();
            logger.Debug("\t\tInitialize the device");
            data.Add("INIT", ComP);
            DeviceCommand(Constant.WELCOME, data);
            logger.Debug("\t\tEnd Init device");
        }

        /// <summary>
        /// Process payment, activate the device.
        /// </summary>
        /// <param name="pay"></param>
        /// <param name="WpInfo"></param>
        public void WPISCPayment(WPDevice.WPData.Payment pay, WPDevice.WPData.WPAccountInfo WpInfo)
        {
            if (iSC != null)
            {
                iSC.ProcessPayment(pay, WpInfo);
            }
            else
            {
                return;
            }

        }

        /// <summary>
        /// Cancel Transaction
        /// </summary>
        public void WPISCCancelTransaction()
        {
            if (iSC != null)
            {
                iSC.CancelWPTransaction();
            }
        }

        /// <summary>
        /// Disconnect from Device
        /// </summary>
        /// <returns></returns>
        public bool Disconnect()
        {
            return iSC.Disconnect();
        }

        /// <summary>
        /// Check if there is a signature
        /// </summary>
        public bool? IsSignValid
        {
            get { return iSC.IsSignValid; }
            set { iSC.IsSignValid = value; }
        }

        /// <summary>
        /// Get the signature in Bytes
        /// </summary>
        public byte[] GetSignature
        {
            get { return iSC.GetSignature; }
            set { iSC.GetSignature = value; }
        }

        public string GetSignatureString
        {
            get { return iSC.GetSignatureString; }
            set { iSC.GetSignatureString = value; }
        }
        /// <summary>
        /// Check if response found
        /// </summary>
        public bool? IsWPResponse
        {
            get { return iSC.IsWPResponse; }
            set { iSC.IsWPResponse = value; }
        }

        public string ButtonClickID
        {
            get { return iSC.ButtonClickID; }
            set { iSC.ButtonClickID = value; }
        }

        public PatientConsent PatConsent
        {
            get { return iSC.PatConsent; }
            set { iSC.PatConsent = value; }
        }

        public bool CancelCapture
        {
            get { return iSC.CancelCapture; }
            set { iSC.CancelCapture = value; }
        }

        /// <summary>
        /// Return the response received
        /// </summary>
        public Dictionary<string, string> ReturnResponse
        {
            get { return iSC.ReturnResponse; }
            set { iSC.ReturnResponse = value; }
        }
        #region PRIMEPOS-2868 - Added for Default or other Consent
        public DataSet dsAutoRefillData 
        {
            get { return iSC.dsAutoRefillData; }
            set { iSC.dsAutoRefillData = value; }
        }
        #endregion
        /// <summary>
        /// ReInitialize the device after a returned error code from verifone
        /// </summary>
        public void ReInitialize()
        {
            logger.Debug("\t\t[ReInitialize Verifone MX]");
            DirectIO();
            if (CommandQueue.Count == 0)
            {
                DeviceCommand(Constant.CurrentDequeueScreen, Constant.CurrentDequeueData);
            }
        }

        /// <summary>
        /// PRIMEPOS-2534 - Lane CLose Issue Added - Suraj 
        /// ReInitialize the device Inactivity Track
        /// </summary>
        public void ReInitializeISC(int PinPadPortNo)
        {
            logger.Debug("\t\t[Enter ReInitialize ingenico ISC 480]");
            Hashtable table = new Hashtable();
            table.Add(iSCFormName.REINIT, PinPadPortNo);
            DeviceCommand(iSCFormName.REINIT, table);
            logger.Debug("\t\t[Exiting ReInitialize ingenico ISC 480]");
        }

        /// <summary>
        /// EnableDevice so it can access from other part of the dll
        /// </summary>
        /// <returns></returns>
        public bool EnableDevice()
        {
            Hashtable data = new Hashtable();
            string screen = string.Empty;
            try
            {
                if (!Constant.IsDeviceOpen)
                {
                    data.Add("OPEN", "OPEN");
                    DeviceCommand("PADMSGSCREEN", data);
                }
                logger.Debug("Xcharge Enable device?: " + Constant.IsDeviceOpen);
            }
            catch (Exception ex)
            {
                logger.Error(ex, ex.Message);
                throw new Exception(ex.ToString());
            }
            return Constant.IsDeviceOpen;

        }

        public bool DisablePinEntry()
        {
            bool isRelease = false;
            if (Pin != null)
            {
                Pin.DisablePinPad();
                isRelease = Constant.isPinRelease;
            }
            return isRelease;
        }
        /// <summary>
        /// Open the device for operation
        /// </summary>
        /// <returns></returns>
        private bool OpenDevice()
        {
            bool isOpen = false;
            int rtCode = 0;
            int count = 1;

            try
            {
                while (!isOpen)
                {
                    logger.Debug("\t\tCheck if device is avaiable");
                    if (OposForm == null)
                    {
                        OposForm = new FormClass();
                        rtCode = OposForm.Open(Constant.deviceName); //Open the device
                        logger.Debug("\t\tInitilize device status: " + rtCode);
                        if (ConvertToBoolean(rtCode))
                        {
                            logger.Debug("\t\t---Device is open----");
                            rtCode = OposForm.ClaimDevice(5000); //claim the device
                            OposForm.DeviceEnabled = Convert.ToInt32(true); //enable the device for it to work
                            OposForm.DataEventEnabled = Convert.ToInt32(true); //enable the device for touch so that it respond when touch
                            OposForm.DataEvent += OposForm_DataEvent; // dataevent for the touch
                            OposForm.ErrorEvent += OposForm_ErrorEvent; //any error from device
                            OposForm.DirectIOEvent += OposForm_DirectIOEvent;
                            OposForm.DeviceEnabled = Convert.ToInt32(true);
                            OposForm.DataEventEnabled = Convert.ToInt32(true);

                            if (ConvertToBoolean(rtCode))
                            {
                                isOpen = true;
                                Constant.IsDeviceOpen = true;
                                logger.Debug("\t\t---Device  is claimed ----");
                            }
                            else
                            {
                                isOpen = false;
                                logger.Debug("\t\t---Device failed to claimed----");
                            }
                        }
                        else
                        {
                            isOpen = false;
                            logger.Debug("\t\t---Device failed to open----");
                        }

                        if (count >= 5)
                            throw new Exception("Failed to connect Pad: " + count);
                        count += 1;
                    }
                    else if (OposForm != null && ConvertToBool(OposForm.Claimed))
                    {
                        isOpen = true;
                        Constant.IsDeviceOpen = true;
                        logger.Debug("\t\tDevice is available (T)");
                    }
                    else
                    {
                        if (ConvertToBool(OposForm.Claimed))
                        {
                            CloseDevice();
                        }
                        OposForm = null;
                        isOpen = false;
                        logger.Debug("\t\tDevice is NOT claimed");
                    }
                }
            }
            catch (Exception ex)
            {
                isOpen = false;
                logger.Error("\t\tOPOSForm OpenDevice failed. \n" + ex.ToString());
                throw new Exception(ex.Message.ToString());
            }
            return isOpen;
        }

        /// <summary>
        /// Use code 72 to reset the device - act as an override
        /// </summary>
        public void DirectIO()
        {
            int rtCode = 0;

            logger.Debug("Entering OPOSForm DirectIOEvent, Reset device. Clear all command");
            if (OposForm != null && ConvertToBool(OposForm.Claimed))
            {
                Constant.CurrentLoadedScreen = "PADWELCOME";
                rtCode = OposForm.DirectIO(0, 0, "72");
            }
            logger.Debug("Finish DirectIO: " + rtCode);
        }

        /// <summary>
        /// DirectIOEvent occur if there is any write failure on the device
        /// </summary>
        /// <param name="EventNumber"></param>
        /// <param name="pData"></param>
        /// <param name="pString"></param>
        private void OposForm_DirectIOEvent(int EventNumber, ref int pData, ref string pString)
        {
            string cScreen = string.Empty;
            bool isShow = false;
            ResetDataEvent();
            cScreen = GetLoadedForm();
            if (cScreen == null)
            {
                logger.Debug("\t\tScreen is null, getloadedForm VFDLL");
                OposForm.GetLoadedForm(out cScreen);
            }
            isShow = ConvertToBoolean(SetSignature(cScreen));
            logger.Debug("\t\tExiting OPOSForm DirectIOEvent: " + cScreen + " isShow: " + isShow);
        }

        /// <summary>
        /// Set the Signature Box on the device for signing
        /// </summary>
        /// <param name="cScreen"></param>
        /// <returns></returns>
        private int SetSignature(string cScreen)
        {
            int retCode = 0;
            switch (cScreen.ToUpper().Trim())
            {
                case "PADSIGN":
                    {
                        logger.Debug("About to Set Signature box for Screen: " + cScreen);
                        retCode = OposForm.BinaryConversion = Constant.BC_NONE;
                        if (Constant.DeviceName == Constant.Devices.VerifoneMX925WithPinPad.ToString())
                        {
                            retCode = OposForm.SetSigCapBoxArea(120, 40, 665, 290, 1); //sig box
                        }
                        else
                        {
                            retCode = OposForm.SetSigCapBoxArea(3, 40, 300, 140, 1); //sig box
                        }
                        retCode = OposForm.DataEventEnabled = Convert.ToInt32(true);
                        retCode = OposForm.GetSignatureData(); //sign event
                        logger.Debug("Finish Set [" + cScreen + "] with Code: " + retCode);
                    }
                    break;
                case "PADOTC":
                    {
                        logger.Debug("About to Set Signature box for Screen: " + cScreen);
                        retCode = OposForm.BinaryConversion = Constant.BC_NONE;
                        if (Constant.DeviceName == Constant.Devices.VerifoneMX925WithPinPad.ToString())
                        {
                            retCode = OposForm.SetSigCapBoxArea(53, 250, 595, 470, 1); //sig box
                        }
                        else
                        {
                            retCode = OposForm.SetSigCapBoxArea(5, 125, 240, 225, 1); //sig box
                        }
                        retCode = OposForm.DataEventEnabled = Convert.ToInt32(true);
                        retCode = OposForm.GetSignatureData(); //sign event
                        logger.Debug("Finish Set [" + cScreen + "] with Code: " + retCode);
                    }
                    break;
                case "PADNOPP":
                    {
                        logger.Debug("About to Set Signature box for Screen: " + cScreen);
                        retCode = OposForm.BinaryConversion = Constant.BC_NONE;
                        if (Constant.DeviceName == Constant.Devices.VerifoneMX925WithPinPad.ToString())
                        {
                            retCode = OposForm.SetSigCapBoxArea(30, 240, 595, 460, 1); //sig box
                        }
                        else
                        {
                            retCode = OposForm.SetSigCapBoxArea(5, 125, 240, 225, 1); //sig box
                        }
                        retCode = OposForm.DataEventEnabled = Convert.ToInt32(true);
                        retCode = OposForm.GetSignatureData(); //sign event
                        logger.Debug("Finish Set [" + cScreen + "] with Code: " + retCode);
                    }
                    break;
                case "PADRXLIST":
                    {
                        logger.Debug("About to Set Signature box for Screen: " + cScreen);
                        retCode = OposForm.BinaryConversion = Constant.BC_NONE;
                        if (Constant.DeviceName == Constant.Devices.VerifoneMX925WithPinPad.ToString())
                        {
                            retCode = OposForm.SetSigCapBoxArea(30, 285, 600, 475, 1); //sig box
                        }
                        else
                        {
                            retCode = OposForm.SetSigCapBoxArea(5, 140, 240, 230, 1); //sig box
                        }
                        retCode = OposForm.DataEventEnabled = Convert.ToInt32(true);
                        retCode = OposForm.GetSignatureData(); //sign event
                        logger.Debug("Finish Set [" + cScreen + "] with Code: " + retCode);
                    }
                    break;
                case "PADSWIPECARD":
                    {
                        logger.Debug("\t\tAbout to Activate swiper Thread");
                        /* Activate the swiper */
                        if (OposMSR == null)
                        {
                            OposMSR = new OPOSMsr();
                        }
                        OposMSR.msr += OposMSR_msr;
                        OposMSR.EnableMSR();
                        logger.Debug("\t\tSwiper Thread Activated");
                    }
                    break;
            }
            OposForm.DataEventEnabled = Convert.ToInt32(true);
            return retCode;
        }

        /// <summary>
        /// Device error event. Occur if the device encounter any error while working
        /// </summary>
        /// <param name="ResultCode"></param>
        /// <param name="ResultCodeExtended"></param>
        /// <param name="ErrorLocus"></param>
        /// <param name="pErrorResponse"></param>
        private void OposForm_ErrorEvent(int ResultCode, int ResultCodeExtended, int ErrorLocus, ref int pErrorResponse)
        {
            string cScreen = string.Empty;
            bool isShow = false;
            ResetDataEvent();
            cScreen = GetLoadedForm();
            if (cScreen == null)
            {
                OposForm.GetLoadedForm(out cScreen);
            }
            isShow = ConvertToBoolean(SetSignature(cScreen));

            logger.Debug("\t\tExiting OPOSForm ErrorEvent: " + cScreen + " isShow: " + isShow);
        }

        /// <summary>
        /// Result code from the device and it corresponding meaning
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        private string ResultText(int code)
        {
            string returnCode = string.Empty;
            switch (code)
            {
                case Constant.OPOS_SUCCESS:
                    returnCode = "OPOS_SUCCESS";
                    break;
                case Constant.OPOS_E_FAILURE:
                    returnCode = "OPOS_E_FAILURE";
                    break;
                case Constant.OPOS_E_OFFLINE:
                    returnCode = "OPOS_E_OFFLINE";
                    break;
                case Constant.OPOS_E_NOHARDWARE:
                    returnCode = "OPOS_E_NOHARDWARE";
                    break;
                case Constant.OPOS_E_TIMEOUT:
                    returnCode = "OPOS_E_TIMEOUT";
                    break;
                default:
                    returnCode = "UNKNOWN";
                    break;
            }
            logger.Debug("Result Code Return from Verifone: " + code + " [" + returnCode + "]");
            return returnCode;
        }

        /// <summary>
        /// Check the Device health e.g If it is busy.
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        private bool DeviceHealth(int code)
        {
            bool healthy = false;
            if (code == Constant.OPOS_S_BUSY)
                Thread.Sleep(Constant.WaitTime);
            else if (code == Constant.OPOS_S_CLOSED)
                OposForm = null;
            else if (code == Constant.OPOS_S_ERROR)
                OposForm.DataEventEnabled = Convert.ToInt32(true);
            else
                healthy = true;
            return healthy;
        }

        /// <summary>
        /// Call from POS to disable swipe event
        /// </summary>
        public void DisableMsr()
        {
            if (OposMSR != null)
            {
                OposMSR.DisableMsr();
            }
        }
        /// <summary>
        /// Device code and its corresponding state
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        private string StateText(int code)
        {
            string stateCode = string.Empty;
            switch (code)
            {
                case Constant.OPOS_SUCCESS:
                    stateCode = "OPOS_SUCCESS";
                    break;
                case Constant.OPOS_S_CLOSED:
                    stateCode = "OPOS_S_CLOSED";
                    break;
                case Constant.OPOS_S_IDLE:
                    stateCode = "OPOS_S_IDLE";
                    break;
                case Constant.OPOS_S_BUSY:
                    stateCode = "OPOS_S_BUSY";
                    break;
                case Constant.OPOS_S_ERROR:
                    stateCode = "OPOS_S_ERROR";
                    break;
                default:
                    stateCode = "UNKNOWN";
                    break;
            }
            return stateCode;
        }

        /// <summary>
        /// This Show form on the device
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        private bool ShowForm(string strScreen)
        {
            bool isShow = false;
            int rtCode = 0;
            int count = 1;
            string sCode = string.Empty;
            string dState = string.Empty;
            signScreen = string.Empty;
            try
            {
                logger.Debug("\t\tOPOS ShowForm(), checking if device in any other function: " + Constant.IsInForm + " and IsInMethod: " + Constant.IsInMethod);
                if (!Constant.IsInForm && Constant.IsInMethod)
                {
                    logger.Debug("\t\tEntering OPOSForm ShowForm: " + strScreen);
                    Constant.IsInForm = true;
                    OpenDevice();
                    if (OposForm != null && ConvertToBool(OposForm.Claimed))
                    {
                        if (IsLoadedForm(strScreen.ToUpper().Trim()))
                        {
                            logger.Debug("\t\tAfter IsLoadedForm()");

                            rtCode = ErrorCode(OposForm.ShowForm());
                            isShow = ConvertToBoolean(rtCode);

                            if (rtCode != Constant.OPOS_SUCCESS)
                            {
                                logger.Debug("\t\tExiting OPOSForm ShowForm is: " + isShow + " screen: " + strScreen.ToUpper() + " Return Code: " + rtCode);
                                return isShow;
                            }

                            OposForm.DataEventEnabled = Convert.ToInt32(true);
                            logger.Debug("\t\tAfter calling Verifone ShowForm(): " + isShow);
                            logger.Debug("\t\tIn ShowForm: " + isShow);

                            //Use to display the signature box
                            SetSignature(strScreen);

                            if (isShow)
                            {
                                Constant.DeviceScreen = strScreen.ToUpper().Trim(); //store the device screen that is on the device currently
                                CurrentScreen = Constant.DeviceScreen; //use to track the screen
                                logger.Debug("\t\tSetting current Screen to: " + CurrentScreen + ", DeviceScreen is: " + Constant.DeviceScreen);
                            }
                        }
                        else
                        {
                            logger.Debug("\t\tScreen was not load before show, load it again: " + strScreen);
                            isShow = false;
                        }
                    }

                    if (!isShow && (OposForm == null || !ConvertToBool(OposForm.Claimed)))
                    {
                        logger.Debug("\t\tOPOSForm ShowForm is null or OPOSForm is not claimed. Retrying");
                    }
                    logger.Debug("\t\tExiting OPOSForm ShowForm is: " + isShow + " screen: " + strScreen.ToUpper());
                }
                return isShow;
            }
            catch (Exception ex)
            {
                Constant.IsInForm = false;
                logger.Error("\t\tError in OPOSForm ShowForm \n" + ex.ToString());
                throw new Exception(ex.Message.ToString());
            }
            finally
            {
                Constant.IsInForm = false;
            }
        }

        /// <summary>
        /// Pass CC data to pos
        /// </summary>
        /// <param name="ccinfo"></param>
        void OposMSR_msr(ArrayList ccinfo)
        {
            OposMSR.msr -= OposMSR_msr;
            if (MsrSwiper != null)
            {
                MsrSwiper(ccinfo);
            }
        }

        private bool CheckReleaseStatus()
        {
            bool status = false;
            int count = 1;
            if (Constant.CurrentLoadedScreen == Constant.PinEntry)
            {
                while (count <= 10 && !Constant.isPinRelease)
                {
                    Thread.Sleep(100);
                    count++;
                }

                if (Constant.isPinRelease || count == 10)
                {
                    logger.Debug("\t\t--->>PinPad Release: " + Constant.isPinRelease);
                    Constant.isPinRelease = false;
                    status = true;
                }
                else if (!Constant.isPinRelease)
                {
                    logger.Debug("\t\t--->>PinPad Release: " + Constant.isPinRelease);
                }
            }
            else if (Constant.CurrentLoadedScreen == Constant.PadSwipeCard)
            {
                while (count <= 10 && !Constant.isMsrRelease)
                {
                    Thread.Sleep(100);
                    count++;
                }

                if (Constant.isMsrRelease || count == 10)
                {
                    logger.Debug("\t\t--->>MSR Release: " + Constant.isMsrRelease);
                    Constant.isMsrRelease = false;
                    status = true;
                }
                else if (!Constant.isMsrRelease)
                {
                    logger.Debug("\t\t--->>MSR Release: " + Constant.isMsrRelease);
                }
            }
            return status;
        }

        /// <summary>
        /// FormInit is use before any form is shown on the device. It must be init before display
        /// </summary>
        /// <param name="frmName"></param>
        /// <returns></returns>
        private bool FormInit(string frmName)
        {
            string screen = string.Empty;
            string sCode = string.Empty;
            string dState = string.Empty;
            bool isInit = false;
            int rtCode = 0;
            int count = 1;

            try
            {
                lock (locker)
                {
                    logger.Debug("\t\tFormInit() checking if device is any other function: " + Constant.IsInForm);
                    if (!Constant.IsInForm && Constant.IsInMethod)
                    {
                        Constant.IsInForm = true;
                        CheckReleaseStatus();
                        logger.Debug("\t\tEntering OPOSForm FormInit: " + frmName);
                        OpenDevice(); //check to make sure device is open
                        logger.Debug("\t\tAfter Open Device in FormInit");
                        if (OposForm != null && ConvertToBool(OposForm.Claimed))
                        {
                            logger.Debug("\t\tIn FormInit OposForm is not NULL and device is claimed");
                            if (!IsLoadedForm(frmName.ToUpper().Trim()))
                            {
                                logger.Debug("\t\tFormInit, This form need to be loaded: " + frmName.ToUpper());

                                if (!Constant.IsSigCap) //this is a one time thing. Only when the POS is close and reopen 
                                {
                                    if (frmName.ToUpper().Trim() == "PADITEMLIST")
                                    {
                                        logger.Debug("\t\tFormInit in the IsSigCap section. Happen only once.");
                                        sigCap = new OPOSsig(); // Sometime the verifone failed to activate the signature 
                                        sigCap.SetSigCap(frmName);
                                        Constant.IsSigCap = true; //do not set to false, this is a onetime setting
                                        logger.Debug("\t\tFormInit IsSigCap: " + Constant.IsSigCap);
                                    }
                                }
                                logger.Debug("\t\tFormInit, About to go and Init the Form: " + frmName);

                                if (ConvertToBool(OposForm.Claimed))
                                {
                                    OpenDevice(); //check to make sure device is open
                                    ErrorCode(OposForm.DirectIO(0, 0, "72"));
                                    Thread.Sleep(Constant.WaitTime);
                                    rtCode = ErrorCode(OposForm.InitForm(frmName.ToUpper().Trim())); //Load form into device memory
                                    isInit = ConvertToBoolean(rtCode); //code return from verifone dll
                                    if (!isInit)
                                    {
                                        logger.Debug("\t\tExiting OPOSForm FormInit is: " + isInit + " Screen: " + Constant.CurrentLoadedScreen);
                                        return isInit;
                                    }
                                    OposForm.DataEventEnabled = Convert.ToInt32(true);
                                }
                                logger.Debug("\t\tFormInit, Is formName: " + frmName.ToUpper() + "?: " + isInit);

                                if (isInit)
                                {
                                    isFrmInt = true;
                                    if (!string.IsNullOrEmpty(Constant.CurrentLoadedScreen))
                                    {
                                        if (Constant.CurrentLoadedScreen.ToUpper().Trim() != frmName.ToUpper().Trim())
                                        {
                                            Constant.CurrentLoadedScreen = "";
                                        }
                                    }
                                    else
                                    {
                                        Constant.CurrentLoadedScreen = "";
                                    }

                                    Constant.CurrentLoadedScreen = frmName.ToUpper();
                                    logger.Debug("\t\tOPOSForm FormInit is: " + isInit + " Screen: " + Constant.CurrentLoadedScreen);
                                }
                                logger.Debug("\t\tOPOSForm FormInit and IsFormloaded:[ " + frmName + " ] is " + isInit);
                            }
                            else
                            {
                                logger.Debug("\t\t FormInit in ELSE section");
                                Thread.Sleep(170);
                                isInit = true;
                                isFrmInt = true;
                                logger.Debug("\t\tOPOSForm FormInit and IsFormloaded 2:[ " + frmName + " ] is " + isInit + " Device is Claimed: " + OposForm.Claimed);
                            }
                        }
                        logger.Debug("\t\tFormInit other checks to perform");
                        if (!isInit && (OposForm == null || !ConvertToBool(OposForm.Claimed)))
                        {
                            logger.Debug("\t\tOPOSForm FormInit OPOSForm is null or OPOSForm is not claimed. Retrying");
                        }
                        logger.Debug("\t\tExiting OPOSForm FormInit is: " + isInit + " Screen: " + Constant.CurrentLoadedScreen);
                    }
                    return isInit;
                }
            }
            catch (Exception ex)
            {
                Constant.IsInForm = false;
                logger.Debug("\t\tError in OPOSForm FormInit \n" + ex.ToString());
                throw new Exception(ex.Message.ToString());
            }
            finally
            {
                Constant.IsInForm = false;
            }
        }

        /// <summary>
        /// Use to convert the value pass back from verifone DLL 0 if operation was sucessful
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        private bool ConvertToBoolean(int i)
        {
            bool isConvert = false;
            try
            {
                isConvert = (i == 0 ? true : false);
            }
            catch (Exception) { }
            return isConvert;
        }

        /// <summary>
        /// Convert int to True or False: True = 1, False = 0. Example Claimed = 1, successful
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        private bool ConvertToBool(int i)
        {
            bool isConvert = false;
            try
            {
                isConvert = (i == 1 ? true : false);
                logger.Debug("\t\t>>Bool Status for return code. Is it True or False: " + isConvert);
            }
            catch (Exception) { }
            return isConvert;
        }

        /// <summary>
        /// Check if the form is loaded
        /// </summary>
        /// <param name="cScreen"></param>
        /// <returns></returns>
        public bool IsLoadedForm(string dScreen)
        {
            string previousLoaded = string.Empty;
            bool isLoaded = false;

            try
            {
                lock (locker)
                {
                    logger.Debug("\t\tOPOSForm IsLoadedForm() About to check if screen: [" + dScreen + "] is loaded");
                    previousLoaded = string.IsNullOrEmpty(Constant.CurrentLoadedScreen) ? "" : Constant.CurrentLoadedScreen.ToUpper().Trim();
                    if (!string.IsNullOrEmpty(previousLoaded) && dScreen.ToUpper().Trim() == previousLoaded)
                    {
                        isLoaded = true;
                    }
                    logger.Debug("\t\tOPOSForm IsLoadedForm: " + isLoaded);
                }
            }
            catch (Exception ex)
            {
                logger.Error("\t\tError in OPOSForm IsloadedForm: \n" + ex.ToString());
            }
            return isLoaded;
        }

        /// <summary>
        /// Get the Current Screen on the Device, Return True if it get the current screen
        /// </summary>
        /// <returns></returns>
        public string GetLoadedForm()
        {
            return _currentScreen;
        }

        /// <summary>
        /// CLose the Device. CLosing the device will disconnect it from the Port.
        /// </summary>
        /// <returns></returns>
        public bool CloseDevice()
        {
            DirectIO();
            Hashtable data = new Hashtable();
            data.Add("CLOSEDEVICE", "CLOSE");
            string screen = Constant.CloseDevice;
            DeviceCommand(screen, data);
            return true;
        }

        /// <summary>
        /// Close the Device, Return True if close
        /// </summary>
        /// <returns></returns>
        private bool VFCloseDevice()
        {
            string rCode = string.Empty;
            string sState = string.Empty;
            int retCode = 0;
            bool isClose = false;
            try
            {
                logger.Debug("\t\tEntering OPOSForm CloseDevice");
                if (OposForm != null && ConvertToBool(OposForm.Claimed))
                {
                    if (sigCap != null) //if for some reason this was not close
                    {
                        Constant.IsSigCap = false;
                        sigCap.CloseSigCap();
                        sigCap = null;
                    }

                    //If Verifone OPOS return true just make sure it finish then move on 
                    OposForm.DeviceEnabled = Constant.False;
                    OposForm.DataEventEnabled = Constant.False;

                    retCode = OposForm.ReleaseDevice();
                    isClose = ConvertToBoolean(retCode);
                    logger.Debug("\t\tCloseDevice Release Result: " + isClose);

                    retCode = OposForm.Close();
                    isClose = ConvertToBoolean(retCode);
                    logger.Debug("\t\tCloseDevice Close Result: " + isClose);
                    OposForm = null;

                    Constant.IsDeviceOpen = false;
                    logger.Debug("\t\tExiting OPOSForm CloseDevice: " + isClose);
                }
                else
                {
                    OposForm = null;
                    logger.Debug("\t\tSetting Opos to Null. CloseDevice()");
                }
            }
            catch (Exception ex)
            {
                logger.Error("\t\tError in OPOSForm CloseDevice \n" + ex.ToString());
                throw new Exception(ex.Message.ToString());
            }
            return isClose;
        }

        public void SetUsercancelled()
        {

        }

        /// <summary>
        /// This function Queue all Commands for the Device.
        /// Each command MUST be queued in order for the device not to crash.
        /// </summary>
        /// <param n0ame="cScreenP"></param>
        /// <param name="DataP"></param>
        public void DeviceCommand(string cScreenP, Hashtable DataP)
        {
            lock (CommandQueue)
            {
                logger.Debug("\t\tQueue command");
                CommandStation CommandData = new CommandStation(cScreenP, DataP);
                CommandQueue.Enqueue(CommandData); //Queue the command for the device
                logger.Debug("\t\tFinish Queue command: " + CommandQueue.Count);

                if (CommandQueue.Count > 0)
                {

                    if ((Constant.DeviceName == Constant.Devices.Ingenico_ISC480.ToString()) || (Constant.DeviceName == Constant.Devices.WPIngenico_ISC480.ToString())) 
                     {
                        bool isException = false;
                        lock (iSC.exceptionObj) 
                        {
                            isException = iSC.isException;
                        }
                        if (isException || (DeviceThread == null ? true : !DeviceThread.IsAlive)) {
                            // if exception and thread.isalive = true; kill the thread
                            if (DeviceThread != null) { 
                                DeviceThread.Abort();
                                DeviceThread = null;
                            }
                            iSC.CommandQueue = CommandQueue;
                            lock (iSC.exceptionObj) {iSC.isException = false; isException = false; }
                            DeviceThread = new Thread(iSC.ISC_SetDeviceProperty);
                            DeviceThread.Start();
                        }
                    }
                    else
                    {
                        if ((DeviceThread == null ? true : !DeviceThread.IsAlive)) {
                            DeviceThread = new Thread(SetDeviceProperty);
                            DeviceThread.Start();
                        }
                    }
                }
                logger.Debug("\t\tExiting DeviceCommand");
            }
        }

        private void ResendDeviceCommand(string cScreenP, Hashtable DataP)
        {
            lock (CommandQueue)
            {
                logger.Debug("\t\tReQueue command");
                ResendCommandStation CommandData = new ResendCommandStation(cScreenP, DataP);
                ResendCommandQueue.Enqueue(CommandData); //Queue the command for the device
                logger.Debug("\t\tFinish ReQueue command: " + ResendCommandQueue.Count);

                if (ResendCommandQueue.Count > 0)
                {
                    if (DeviceThread == null ? true : !DeviceThread.IsAlive)
                    {
                        //If the thread is not alive start a new thread.
                        DeviceThread = new Thread(SetDeviceProperty);
                        DeviceThread.Start();
                    }
                }
                logger.Debug("\t\tExiting DeviceCommand");
            }
        }

        /// <summary>
        /// Not use 
        /// </summary>
        void OPOSForm_EventReset()
        {
            Constant.Reset();
        }

        /// <summary>
        /// This is the Thread on which the Command will run on.
        /// </summary>
        void OPOSForm_CommandEvent()
        {
            CommandEvent -= OPOSForm_CommandEvent; //deactivate the event
            if (DeviceThread == null ? true : !DeviceThread.IsAlive)
            {
                //If the thread is not alive start a new thread.
                DeviceThread = new Thread(SetDeviceProperty);
                DeviceThread.Start();
            }
        }

        /// <summary>
        /// Return the Error Code from Verifone
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        private int ErrorCode(int code)
        {
            int rCode = 0;
            switch (code)
            {
                case Constant.OPOS_E_FAILURE:
                case Constant.OPOS_E_OFFLINE:
                case Constant.OPOS_E_NOHARDWARE:
                case Constant.OPOS_E_TIMEOUT:
                    {
                        if (ReturnCodeEvent != null)
                        {
                            ReturnCodeEvent(true, code, ResultText(code));
                        }
                    }
                    break;
                default:
                    {
                        ResultText(code);
                    }
                    break;
            }
            rCode = code;
            return rCode;
        }

        private void CurrectDequeueData(string cScreen, Hashtable d)
        {
            if (!string.IsNullOrEmpty(cScreen) && d != null)
            {
                Constant.CurrentDequeueScreen = string.Empty;
                Constant.CurrentDequeueData = new Hashtable();
                Constant.CurrentDequeueScreen = cScreen;
                Constant.CurrentDequeueData = d;
            }
        }

        /// <summary>
        /// This is use to set everything on the device. All Data is set from this function
        /// aCharge - Amount Charge, PName - Patient Name, PAddress - Patient Addess, PNopp - Nopp Message
        /// </summary>
        /// <param name="cScreen"></param>
        /// <param name="vData"></param>
        /// <returns></returns>
        public void SetDeviceProperty()
        {
            logger.Debug("\t\t==>About to Set Data on Device");
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
                            bool isFA_Pine = false; //pin entry
                            CommandStation command = null;
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
                                ErrorLog += OPOSForm_ErrorLog; //Event for Error. This will call the Thread again
                                ErrorLog.Invoke();
                                return;
                            }
                            string cScreen = string.Empty;
                            bool isSet = false;
                            isFrmInt = false;
                            int oCount = 0;

                            if (vData != null)
                                vData.Clear();
                            else
                                vData = new Hashtable();

                            vData = command.DeviceData as Hashtable; //data that was dequeue
                            cScreen = command.DeviceScreen;//data that was dequeue
                            CurrectDequeueData(cScreen, vData);

                            logger.Debug("\t\tEntering OPOSForm SetDeviceProperity");
                            RegisterDLLS(); //Make sure all DLL'S are registered
                            if (cScreen.ToUpper().Trim() == "CLOSEDEVICE")
                            {
                                VFCloseDevice(); //Command to close the device
                                Constant.IsInMethod = false;
                                Constant.IsStillWrite = false;
                                logger.Debug("\t\t---> Exiting OPOSForm SetDeviceProperity");
                                return;
                            }

                            if (cScreen.ToUpper().Trim() == "FA_PINE")
                            {
                                isFA_Pine = true;
                            }

                            if (!isFA_Pine)
                            {
                                if (!FormInit(cScreen.ToUpper().Trim()) && (vData != null && vData.Count > 0)) // if both fail check it one more time
                                {
                                    logger.Debug("\t\tError In OPOSForm SetDeviceProperity, Fail to load screen in memory");
                                    Constant.IsInMethod = false;
                                    Constant.IsStillWrite = false;
                                    ErrorLog += OPOSForm_ErrorLog; //Event for Error. This will call the Thread again
                                    ErrorLog.Invoke();
                                    return; // return if fail
                                }
                            }

                            logger.Debug("\t\t ---> OPOSForm SetDeviceProperity screen: " + cScreen);
                            if (OposForm != null && OposForm.Claimed == Constant.True)
                            {
                                //The FORM(screen) to display by the device with all the Device Properities.
                                switch (cScreen.ToUpper().Trim())
                                {
                                    case "PADSIGN":
                                        {
                                            logger.Debug("\t\t\t[PADSIGN] Begin");
                                            if (vData != null && vData.ContainsKey("aCharge"))
                                            {
                                                logger.Debug("\t\t\t[PADSIGN] ACharge");
                                                string AmtCharge = string.IsNullOrEmpty(vData["aCharge"].ToString().Trim()) ? "0.00" : vData["aCharge"].ToString();
                                                if (ErrorCode(OposForm.SetPropValue(Constant.PADSIGN_LABEL_AMOUNT_CHARGED, Constant.FORM_PROP_STR_CAPTION, Constant.FORM_PROP_TYPE_STRING, AmtCharge)) == 0)
                                                {
                                                    isSet = true;
                                                }
                                            }
                                            logger.Debug("\t\t\t[PADSIGN] Finish");
                                        }
                                        break;
                                    case "PADNOPP":
                                        {
                                            logger.Debug("\t\t\t[PADNOPP] Begin");
                                            if (vData != null && vData.ContainsKey("pNopp") && vData.ContainsKey("pName") && vData.ContainsKey("pAddress"))
                                            {
                                                logger.Debug("\t\t\t[PADNOPP] Nopp, Name, address");
                                                if (ErrorCode(OposForm.ClearTextBoxText(Constant.PADNOPP_TEXTBOX)) == 0)
                                                {
                                                    string NoppMsg = string.IsNullOrEmpty(vData["pNopp"].ToString().Trim()) ? "Please ask for HIPAA notice. Not available at this time" : vData["pNopp"].ToString();

                                                    if (ErrorCode(OposForm.AddTextBoxText(Constant.PADNOPP_TEXTBOX, NoppMsg)) == 0)
                                                    {
                                                        string PatName = string.IsNullOrEmpty(vData["pName"].ToString().Trim()) ? "Patient Name" : vData["pName"].ToString();
                                                        if (ErrorCode(OposForm.SetPropValue(Constant.PADNOPP_LABEL_PATIENT_NAME_TEXT, Constant.FORM_PROP_STR_CAPTION, Constant.FORM_PROP_TYPE_STRING, PatName)) == 0)
                                                        {
                                                            string PatAddress = string.IsNullOrEmpty(vData["pAddress"].ToString().Trim()) ? "Patient Address" : vData["pAddress"].ToString();
                                                            if (ErrorCode(OposForm.SetPropValue(Constant.PADNOPP_LABEL_ADDRESS_TEXT, Constant.FORM_PROP_STR_CAPTION, Constant.FORM_PROP_TYPE_STRING, PatAddress)) == 0)
                                                            {
                                                                isSet = true;
                                                            }
                                                        }
                                                    }
                                                }

                                            }
                                            logger.Debug("\t\t\t[PADNOPP] Finish");
                                        }
                                        break;
                                    case "PADRXLIST":
                                        {
                                            logger.Debug("\t\t\t[PADRXLIST] Begin");
                                            if (vData.ContainsKey("showButton"))
                                            {
                                                logger.Debug("\t\t\t[PADRXLIST] ShowButton");
                                                string showButton = string.Empty;
                                                if (string.IsNullOrEmpty(vData["showButton"].ToString().Trim()))
                                                    showButton = "0";
                                                else
                                                    showButton = vData["showButton"].ToString() == "Y" ? "1" : "0";
                                                ErrorCode(OposForm.SetPropValue(Constant.PADRXLIST_BUTTON_DETAIL, Constant.FORM_PROP_BOOL_VISIBLE, Constant.FORM_PROP_BOOL_VISIBLE, showButton));
                                            }

                                            if (vData.ContainsKey("removeRxItem"))
                                            {
                                                logger.Debug("\t\t\t[PADRXLIST] RemoveRxItem");
                                                if (!string.IsNullOrEmpty(vData["index"].ToString().Trim()))
                                                {
                                                    int Index = Convert.ToInt32(vData["index"].ToString());
                                                    if (ErrorCode(OposForm.RemoveListBoxItem(Constant.PADRXLIST_LISTBOX, Index)) == 0)
                                                    {
                                                        isSet = true;
                                                    }
                                                }
                                            }

                                            if (vData.ContainsKey("pName") && vData.ContainsKey("totalRx") && vData.ContainsKey("counselReq"))
                                            {
                                                logger.Debug("\t\t\t[PADRXLIST] PName, TotalRx, CounselReq");
                                                string PatName = string.IsNullOrEmpty(vData["pName"].ToString().Trim()) ? "Patient Name" : vData["pName"].ToString();
                                                if (ErrorCode(OposForm.SetPropValue(Constant.PADRXLIST_LABEL_PATIENT_NAME_TEXT, Constant.FORM_PROP_STR_CAPTION, Constant.FORM_PROP_TYPE_STRING, PatName)) == 0)
                                                {
                                                    string TotalRx = string.IsNullOrEmpty(vData["totalRx"].ToString().Trim()) ? "0" : vData["totalRx"].ToString();
                                                    if (ErrorCode(OposForm.SetPropValue(Constant.PADRXLIST_LABEL_RX_COUNT_TEXT, Constant.FORM_PROP_STR_CAPTION, Constant.FORM_PROP_TYPE_STRING, TotalRx)) == 0)
                                                    {
                                                        int Counsel = 0;
                                                        if (!string.IsNullOrEmpty(vData["counselReq"].ToString().Trim()))
                                                        {
                                                            Counsel = vData["counselReq"].ToString() == "N" ? Constant.PADRXLIST_RADIOBUTTON_NO : Constant.PADRXLIST_RADIOBUTTON_YES;
                                                        }
                                                        if (ErrorCode(OposForm.SetPropValue(Counsel, Constant.FORM_PROP_BOOL_SELECTED, Constant.FORM_PROP_TYPE_BOOL, "1")) == 0)
                                                        {
                                                            isSet = true;
                                                        }

                                                        if (vData["counselReq"].ToString() == "NC")
                                                        {
                                                            ErrorCode(OposForm.SetPropValue(Constant.PADRXLIST_RADIOBUTTON_YES, Constant.FORM_PROP_BOOL_VISIBLE, Constant.FORM_PROP_TYPE_BOOL, "0"));
                                                            ErrorCode(OposForm.SetPropValue(Constant.PADRXLIST_RADIOBUTTON_NO, Constant.FORM_PROP_BOOL_VISIBLE, Constant.FORM_PROP_TYPE_BOOL, "0"));
                                                            if (Constant.DeviceName == Constant.Devices.VerifoneMX925WithPinPad.ToString())
                                                                ErrorCode(OposForm.SetPropValue(Constant.PADRXLIST_PAT_COUNSELING_TEXT, Constant.FORM_PROP_BOOL_VISIBLE, Constant.FORM_PROP_TYPE_BOOL, "0"));
                                                            else
                                                            {
                                                                ErrorCode(OposForm.SetPropValue(3, Constant.FORM_PROP_BOOL_VISIBLE, Constant.FORM_PROP_TYPE_BOOL, "0"));
                                                            }
                                                        }
                                                    }
                                                }
                                            }

                                            if (vData["rxItem"] != null && vData.ContainsKey("rxItem"))
                                            {
                                                logger.Debug("\t\t\t[PADRXLIST] rxItem");
                                                ArrayList rx = new ArrayList();
                                                bool isRxAdd = false;
                                                ErrorCode(OposForm.ClearListBox(Constant.PADRXLIST_LISTBOX));
                                                while (!isRxAdd)
                                                {
                                                    try
                                                    {
                                                        rx = vData["rxItem"] as ArrayList;
                                                        isRxAdd = true;
                                                    }
                                                    catch (Exception)
                                                    {
                                                        isRxAdd = false;
                                                    }
                                                }
                                                foreach (var Rxs in rx)
                                                {
                                                    string rxString = string.IsNullOrEmpty(Rxs.ToString().Trim()) ? "000000" : Rxs.ToString();
                                                    ErrorCode(OposForm.AddListBoxItem(Constant.PADRXLIST_LISTBOX, oCount, rxString, Constant.True));
                                                    oCount += 1;
                                                }
                                                isSet = true;
                                            }
                                            logger.Debug("\t\t\t[PADRXLIST] Finish");
                                        }
                                        break;
                                    case "PADITEMLIST":
                                        {
                                            int items = 0;
                                            logger.Debug("\t\t\t[PADITEMLIST] Begin");
                                            if (vData != null && vData.ContainsKey("removeCounterItem"))
                                            {
                                                logger.Debug("\t\t\t[PADITEMLIST] RemoveCounterItem");
                                                int rIndex = 0;
                                                rIndex = Convert.ToInt32(vData["removeCounterItem"].ToString());
                                                if (ErrorCode(OposForm.RemoveListBoxItem(Constant.PADITEMLIST_LISTBOX, rIndex)) == 0)
                                                {
                                                    ShowForm(cScreen);
                                                    if (Constant.dataStore != null)
                                                    {
                                                        items = Constant.dataStore.Count;
                                                    }
                                                    if (ErrorCode(OposForm.SetPropValue(Constant.PADITEMLIST_LABEL_LINEITEM, Constant.FORM_PROP_STR_CAPTION, Constant.FORM_PROP_TYPE_STRING, "Line Item: " + items.ToString())) == 0)
                                                    {
                                                        isSet = true;
                                                    }
                                                }
                                            }

                                            if (vData != null && vData.ContainsKey("ClearItems"))
                                            {
                                                logger.Debug("\t\t\t[PADITEMLIST] ClearItems");
                                                if (ErrorCode(OposForm.ClearListBox(Constant.PADITEMLIST_LISTBOX)) == 0)
                                                {
                                                    if (ErrorCode(OposForm.SetPropValue(Constant.PADITEMLIST_LABEL_LINEITEM, Constant.FORM_PROP_STR_CAPTION, Constant.FORM_PROP_TYPE_STRING, "Line Item: " + items.ToString())) == 0)
                                                    {
                                                        isSet = true;
                                                    }
                                                }
                                            }


                                            if (vData != null && vData.ContainsKey("UpdateItem") && vData.ContainsKey("Index"))
                                            {
                                                logger.Debug("\t\t\t[PADITEMLIST] UpdateItem");
                                                if (ErrorCode(OposForm.RemoveListBoxItem(Constant.PADITEMLIST_LISTBOX, Convert.ToInt32(vData["Index"].ToString()))) == 0)
                                                {
                                                    string UpdateItem = string.IsNullOrEmpty(vData["UpdateItem"].ToString().Trim()) ? "Unknown" : vData["UpdateItem"].ToString();
                                                    if (ErrorCode(OposForm.AddListBoxItem(Constant.PADITEMLIST_LISTBOX, Convert.ToInt32(vData["Index"].ToString()), UpdateItem, Constant.True)) == 0)
                                                    {
                                                        isSet = true;
                                                    }
                                                }
                                            }

                                            //Get the item from the datastore
                                            if (vData != null && vData.ContainsKey("counterItem"))
                                            {
                                                logger.Debug("\t\t\t[PADITEMLIST] counterItem");
                                                if (Constant.dataStore != null)
                                                {
                                                    items = Constant.dataStore.Count;
                                                }

                                                string CounterItem = string.IsNullOrEmpty(vData["counterItem"].ToString().Trim()) ? "Unknown" : vData["counterItem"].ToString();

                                                if (ErrorCode(OposForm.AddListBoxItem(Constant.PADITEMLIST_LISTBOX, items, CounterItem, Constant.True)) == 0)
                                                {
                                                    oCount = items;
                                                    if (ErrorCode(OposForm.SetPropValue(Constant.PADITEMLIST_LABEL_LINEITEM, Constant.FORM_PROP_STR_CAPTION, Constant.FORM_PROP_TYPE_STRING, "Line Item: " + oCount.ToString())) == 0)
                                                    {
                                                        isSet = true;
                                                    }
                                                }
                                            }

                                            if (vData != null && vData.ContainsKey("OnHoldItems"))
                                            {
                                                logger.Debug("\t\t\t[PADITEMLIST] OnHoldItem");
                                                ArrayList oitem = new ArrayList();
                                                bool isItem = false;
                                                while (!isItem)
                                                {
                                                    try
                                                    {
                                                        oitem = vData["OnHoldItems"] as ArrayList;
                                                        isItem = true;
                                                    }
                                                    catch (Exception)
                                                    {
                                                        isItem = false;
                                                    }
                                                }
                                                foreach (var cItem in oitem)
                                                {
                                                    string onHold = string.IsNullOrEmpty(cItem.ToString().Trim()) ? "Unknown" : cItem.ToString();
                                                    ErrorCode(OposForm.AddListBoxItem(Constant.PADITEMLIST_LISTBOX, oCount, onHold, Constant.True));
                                                    oCount += 1;
                                                }
                                                ErrorCode(OposForm.SetPropValue(Constant.PADITEMLIST_LABEL_LINEITEM, Constant.FORM_PROP_STR_CAPTION, Constant.FORM_PROP_TYPE_STRING, "Line Item: " + oCount.ToString()));
                                            }

                                            if (vData != null && vData.ContainsKey("resendCounter"))
                                            {
                                                logger.Debug("\t\t\t[PADITEMLIST] resendCounter");
                                                ArrayList oitem = new ArrayList();
                                                bool isItem = false;

                                                while (!isItem)
                                                {
                                                    try
                                                    {
                                                        oitem = vData["resendCounter"] as ArrayList;
                                                        isItem = true;
                                                    }
                                                    catch (Exception)
                                                    {
                                                        isItem = false;
                                                    }
                                                }
                                                foreach (var cItem in oitem)
                                                {
                                                    string ResendItem = string.IsNullOrEmpty(cItem.ToString().Trim()) ? "Unknown" : cItem.ToString();
                                                    ErrorCode(OposForm.AddListBoxItem(Constant.PADITEMLIST_LISTBOX, oCount, ResendItem, Constant.True));
                                                    oCount += 1;
                                                }
                                                ErrorCode(OposForm.SetPropValue(Constant.PADITEMLIST_LABEL_LINEITEM, Constant.FORM_PROP_STR_CAPTION, Constant.FORM_PROP_TYPE_STRING, "Line Item: " + oCount.ToString()));
                                            }

                                            if (vData != null && vData.ContainsKey("subTotal") && vData.ContainsKey("Discount") && vData.ContainsKey("Tax") && vData.ContainsKey("totalAmount"))
                                            {
                                                logger.Debug("\t\t\t[PADITEMLIST] SubTotal, Discount, Tax, TotalAmount");
                                                string subTotal = string.IsNullOrEmpty(vData["subTotal"].ToString().Trim()) ? "0.00" : vData["subTotal"].ToString();
                                                if (ErrorCode(OposForm.SetPropValue(Constant.PADITEMLIST_LABEL_SUBTOTAL, Constant.FORM_PROP_STR_CAPTION, Constant.FORM_PROP_TYPE_STRING, subTotal)) == 0)
                                                {
                                                    string Discount = string.IsNullOrEmpty(vData["Discount"].ToString().Trim()) ? "0.00" : vData["Discount"].ToString();
                                                    if (ErrorCode(OposForm.SetPropValue(Constant.PADITEMLIST_LABEL_DISCOUNT, Constant.FORM_PROP_STR_CAPTION, Constant.FORM_PROP_TYPE_STRING, Discount)) == 0)
                                                    {
                                                        string Tax = string.IsNullOrEmpty(vData["Tax"].ToString().Trim()) ? "0.00" : vData["Tax"].ToString();
                                                        if (ErrorCode(OposForm.SetPropValue(Constant.PADITEMLIST_LABEL_TAX, Constant.FORM_PROP_STR_CAPTION, Constant.FORM_PROP_TYPE_STRING, Tax)) == 0)
                                                        {
                                                            string TotalAmount = string.IsNullOrEmpty(vData["totalAmount"].ToString().Trim()) ? "0.00" : vData["totalAmount"].ToString();
                                                            if (ErrorCode(OposForm.SetPropValue(Constant.PADITEMLIST_LABEL_TOTAL_AMOUNT, Constant.FORM_PROP_STR_CAPTION, Constant.FORM_PROP_TYPE_STRING, TotalAmount)) == 0)
                                                            {
                                                                isSet = true;
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                            logger.Debug("\t\t\t[PADITEMLIST] Finish");
                                        }
                                        break;
                                    case "PADSWIPECARD":
                                        {
                                            logger.Debug("\t\t\t[PADSWIPECARD] Begin");
                                            if (vData != null && vData.ContainsKey("swipeMsg") && vData.ContainsKey("aCharge"))
                                            {
                                                logger.Debug("\t\t\t[PADSWIPECARD] SwiperMsg");
                                                string SwipeMsg = string.IsNullOrEmpty(vData["swipeMsg"].ToString().Trim()) ? "Total Charge Amount" : vData["swipeMsg"].ToString();
                                                if (ErrorCode(OposForm.SetPropValue(Constant.PADSWIPECARD_LABEL_SWIPEMSG, Constant.FORM_PROP_STR_CAPTION, Constant.FORM_PROP_TYPE_STRING, SwipeMsg)) == 0)
                                                {
                                                    string AmountCharge = string.IsNullOrEmpty(vData["aCharge"].ToString().Trim()) ? "Please ask" : vData["aCharge"].ToString().Replace(" ", "");
                                                    if (ErrorCode(OposForm.SetPropValue(Constant.PADSWIPECARD_LABEL_AMOUNT, Constant.FORM_PROP_STR_CAPTION, Constant.FORM_PROP_TYPE_STRING, AmountCharge)) == 0)
                                                    {
                                                        isSet = true;
                                                    }
                                                }
                                            }
                                            logger.Debug("\t\t\t[PADSWIPECARD] Finish");
                                        }
                                        break;
                                    case "PADMSGSCREEN":
                                        {
                                            logger.Debug("\t\t\t[PADMSGSCREEN] Begin");
                                            if (vData != null && vData.ContainsKey("padMsg"))
                                            {
                                                logger.Debug("\t\t\t[PADMSGSCREEN] PadMsg");
                                                string padMsg = string.IsNullOrEmpty(vData["padMsg"].ToString().Trim()) ? "Please wait" : vData["padMsg"].ToString();
                                                if (ErrorCode(OposForm.SetPropValue(Constant.PADMSGSCREEN_LABEL_MESSAGE, Constant.FORM_PROP_STR_CAPTION, Constant.FORM_PROP_TYPE_STRING, vData["padMsg"].ToString())) == 0)
                                                {
                                                    isSet = true;
                                                }
                                            }
                                            logger.Debug("\t\t\t[PADMSGSCREEN] Finish");
                                        }
                                        break;
                                    case "PADOTC":
                                        {
                                            logger.Debug("\t\t\t[PADOTC] Begin");
                                            if (vData != null && vData["oTcItems"] != null)
                                            {
                                                logger.Debug("\t\t\t[PADOTC] OtcItems");
                                                ArrayList OtcItems = new ArrayList();
                                                bool isOtc = false;

                                                ErrorCode(OposForm.ClearTextBoxText(Constant.PADOTC_TEXTBOX));
                                                string OTCMsg = string.IsNullOrEmpty(vData["oTcMsg"].ToString().Trim()) ? "Unknown" : vData["oTcMsg"].ToString();
                                                ErrorCode(OposForm.AddTextBoxText(Constant.PADOTC_TEXTBOX, OTCMsg));

                                                while (!isOtc)
                                                {
                                                    try
                                                    {
                                                        OtcItems = vData["oTcItems"] as ArrayList;
                                                        isOtc = true;
                                                    }
                                                    catch (Exception)
                                                    {
                                                        isOtc = false;
                                                    }
                                                }

                                                foreach (var Otc in OtcItems)
                                                {
                                                    string otcItem = string.IsNullOrEmpty(Otc.ToString().Trim()) ? "Unknown" : Otc.ToString();
                                                    ErrorCode(OposForm.AddListBoxItem(Constant.PADOTC_LISTBOX, oCount, otcItem, Constant.True));
                                                    oCount += 1;
                                                }
                                                isSet = true;
                                            }
                                            logger.Debug("\t\t\t[PADOTC] Finish");
                                        }
                                        break;
                                    case "PADSHOWCASH":
                                        {
                                            logger.Debug("\t\t\t[PADSHOWCASH] Begin");
                                            if (vData != null && vData.ContainsKey("GrossAmount") && vData.ContainsKey("TaxAmount") && vData.ContainsKey("NetAmount") && vData.ContainsKey("PaidAmount") && vData.ContainsKey("ChangeDueAmount"))
                                            {
                                                string GA = string.IsNullOrEmpty(vData["GrossAmount"].ToString().Trim()) ? "0.00" : vData["GrossAmount"].ToString();
                                                logger.Debug("\t\t\t[PADSHOWCASH] Line: GA = " + GA);
                                                if (ErrorCode(OposForm.SetPropValue(Constant.PADSHOWCASH_LABEL_SUB_TOTAL_TEXT, Constant.FORM_PROP_STR_CAPTION, Constant.FORM_PROP_TYPE_STRING, GA)) == 0)
                                                {
                                                    string TA = string.IsNullOrEmpty(vData["TaxAmount"].ToString().Trim()) ? "0.00" : vData["TaxAmount"].ToString();
                                                    logger.Debug("\t\t\t[PADSHOWCASH] Line: TA = " + TA);
                                                    if (ErrorCode(OposForm.SetPropValue(Constant.PADSHOWCASH_LABEL_TAX_TEXT, Constant.FORM_PROP_STR_CAPTION, Constant.FORM_PROP_TYPE_STRING, TA)) == 0)
                                                    {
                                                        string NA = string.IsNullOrEmpty(vData["NetAmount"].ToString().Trim()) ? "0.00" : vData["NetAmount"].ToString();
                                                        logger.Debug("\t\t\t[PADSHOWCASH] Line: NA = " + NA);
                                                        if (ErrorCode(OposForm.SetPropValue(Constant.PADSHOWCASH_LABEL_TOTAL_DUE_TEXT, Constant.FORM_PROP_STR_CAPTION, Constant.FORM_PROP_TYPE_STRING, NA)) == 0)
                                                        {
                                                            string PA = string.IsNullOrEmpty(vData["PaidAmount"].ToString().Trim()) ? "0.00" : vData["PaidAmount"].ToString();
                                                            logger.Debug("\t\t\t[PADSHOWCASH] Line: PA = " + PA);
                                                            if (ErrorCode(OposForm.SetPropValue(Constant.PADSHOWCASH_LABEL_AMOUNT_TENDERED_TEXT, Constant.FORM_PROP_STR_CAPTION, Constant.FORM_PROP_TYPE_STRING, PA)) == 0)
                                                            {
                                                                string CDA = string.IsNullOrEmpty(vData["ChangeDueAmount"].ToString().Trim()) ? "0.00" : vData["ChangeDueAmount"].ToString();
                                                                logger.Debug("\t\t\t[PADSHOWCASH] Line: CDA = " + CDA);
                                                                if (ErrorCode(OposForm.SetPropValue(Constant.PADSHOWCASH_LABEL_CHANGE_DUE_TEXT, Constant.FORM_PROP_STR_CAPTION, Constant.FORM_PROP_TYPE_STRING, CDA)) == 0)
                                                                {
                                                                    isSet = true;
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                            logger.Debug("\t\t\t[PADSHOWCASH] Finish");
                                        }
                                        break;
                                    case Constant.PinEntry:
                                        {
                                            Constant.CurrentLoadedScreen = Constant.PinEntry;
                                            if (Pin != null)
                                            {
                                                Pin = null;
                                            }
                                            Pin = new OPOSPinPad(); //PinPad 
                                            Pin.PinBlock += Pin_PinBlock;
                                            isSet = true;
                                        }
                                        break;
                                    case Constant.WELCOME:
                                        {
                                            logger.Debug("\t\t\t[PADWELCOME] Begin");
                                            if (Constant.DeviceName != Constant.Devices.VerifoneMX925WithPinPad.ToString())
                                            {
                                                GetDeviceInformation();
                                            }
                                            isSet = true;
                                            logger.Debug("\t\t\t[PADWELCOME] Finish");
                                        }
                                        break;
                                    case "PATIENTCONSENT"://PRIMEPOS-2867
                                        {
                                            try
                                            {
                                                if (vData != null)
                                                {
                                                    ErrorCode(OposForm.SetPropValue(1, Constant.FORM_PROP_STR_CAPTION, Constant.FORM_PROP_TYPE_STRING, "           " + vData["TITLE"] + ""));
                                                    ErrorCode(OposForm.SetPropValue(2, Constant.FORM_PROP_STR_CAPTION, Constant.FORM_PROP_TYPE_STRING, "PATIENT :"));
                                                    ErrorCode(OposForm.SetPropValue(3, Constant.FORM_PROP_STR_CAPTION, Constant.FORM_PROP_TYPE_STRING, "ADDRESS :"));
                                                    //ErrorCode(OposForm.SetPropValue(4, Constant.FORM_PROP_STR_CAPTION, Constant.FORM_PROP_TYPE_STRING, "\t\t\t\t\tARVIND CHAVAN"));
                                                    //ErrorCode(OposForm.SetPropValue(5, Constant.FORM_PROP_STR_CAPTION, Constant.FORM_PROP_TYPE_STRING, "\t\t\t\t\t23rd BAKER STREET"));
                                                    //ErrorCode(OposForm.SetPropValue(6, Constant.FORM_PROP_STR_CAPTION, Constant.FORM_PROP_TYPE_STRING, "\t\t\t\t\n\n\n\n\n\nHELLO MMS\n\n\n\n\n\n\n"));

                                                    ErrorCode(OposForm.ClearTextBoxText(4));
                                                    ErrorCode(OposForm.ClearTextBoxText(5));
                                                    ErrorCode(OposForm.ClearTextBoxText(6));
                                                    ErrorCode(OposForm.AddTextBoxText(4, "           " + vData["PATIENTNAME"] + ""));
                                                    ErrorCode(OposForm.AddTextBoxText(5, "          " + vData["PATIENTADDRESS"] + ""));
                                                    ErrorCode(OposForm.AddTextBoxText(6, vData["TEXT"].ToString()));

                                                    if (vData.ContainsKey("FIRSTRDBTN"))
                                                        ErrorCode(OposForm.SetPropValue(7, Constant.FORM_PROP_STR_CAPTION, Constant.FORM_PROP_TYPE_STRING, vData["FIRSTRDBTN"].ToString()));
                                                    else
                                                        ErrorCode(OposForm.SetPropValue(7, Constant.FORM_PROP_BOOL_VISIBLE, Constant.FORM_PROP_TYPE_BOOL, "0"));
                                                    if (vData.ContainsKey("SECONDRDBTN"))
                                                        ErrorCode(OposForm.SetPropValue(8, Constant.FORM_PROP_STR_CAPTION, Constant.FORM_PROP_TYPE_STRING, vData["SECONDRDBTN"].ToString()));
                                                    else
                                                        ErrorCode(OposForm.SetPropValue(8, Constant.FORM_PROP_BOOL_VISIBLE, Constant.FORM_PROP_TYPE_BOOL, "0"));
                                                    if (vData.ContainsKey("THIRDRDBTN"))
                                                        ErrorCode(OposForm.SetPropValue(9, Constant.FORM_PROP_STR_CAPTION, Constant.FORM_PROP_TYPE_STRING, vData["THIRDRDBTN"].ToString()));
                                                    else
                                                        ErrorCode(OposForm.SetPropValue(9, Constant.FORM_PROP_BOOL_VISIBLE, Constant.FORM_PROP_TYPE_BOOL, "0"));
                                                    if (vData.ContainsKey("FOURTHRDBTN"))
                                                        ErrorCode(OposForm.SetPropValue(10, Constant.FORM_PROP_STR_CAPTION, Constant.FORM_PROP_TYPE_STRING, vData["FOURTHRDBTN"].ToString()));
                                                    else
                                                        ErrorCode(OposForm.SetPropValue(10, Constant.FORM_PROP_BOOL_VISIBLE, Constant.FORM_PROP_TYPE_BOOL, "0"));
                                                    //if (vData.ContainsKey("FIRSTBTN"))
                                                    ErrorCode(OposForm.SetPropValue(11, Constant.FORM_PROP_STR_CAPTION, Constant.FORM_PROP_TYPE_STRING, "SKIP"));
                                                    //else
                                                    //    ErrorCode(OposForm.SetPropValue(11, Constant.FORM_PROP_BOOL_VISIBLE, Constant.FORM_PROP_TYPE_BOOL, "0"));
                                                    if (vData.ContainsKey("SECONDBTN"))
                                                        ErrorCode(OposForm.SetPropValue(12, Constant.FORM_PROP_STR_CAPTION, Constant.FORM_PROP_TYPE_STRING, vData["SECONDBTN"].ToString()));
                                                    else
                                                        ErrorCode(OposForm.SetPropValue(12, Constant.FORM_PROP_BOOL_VISIBLE, Constant.FORM_PROP_TYPE_BOOL, "0"));
                                                    if (vData.ContainsKey("THIRDBTN"))
                                                        ErrorCode(OposForm.SetPropValue(13, Constant.FORM_PROP_STR_CAPTION, Constant.FORM_PROP_TYPE_STRING, vData["THIRDBTN"].ToString()));
                                                    else
                                                        ErrorCode(OposForm.SetPropValue(13, Constant.FORM_PROP_BOOL_VISIBLE, Constant.FORM_PROP_TYPE_BOOL, "0"));
                                                    isSet = true;
                                                }
                                            }
                                            catch (Exception ex)
                                            {
                                                logger.Error("ERROR IS PATIENTCONSENT" + ex.Message);
                                            }
                                        }
                                        break;
                                }
                                logger.Debug("\t\t ----> OPOSForm SetDeviceProperity finish setting");

                                if (isFrmInt && !isFA_Pine)
                                {
                                    if (isSet && ShowForm(cScreen))
                                    {
                                        //Show the screen after init
                                        isSet = true;
                                    }
                                    else
                                    {
                                        isSet = false;
                                        logger.Debug("\t\tOPOSForm ShowForm Fail");
                                    }
                                }
                                else if (isFA_Pine)
                                {
                                    logger.Debug("\t\tPinPad Entry On Device Screen");
                                    isSet = true;
                                    isFA_Pine = false;
                                }
                                else
                                {
                                    isSet = false;
                                    logger.Debug("\t\tOPOSForm FormInit Fail");
                                }
                            }
                            if (OposForm == null || OposForm.Claimed == Constant.False)
                            {
                                logger.Debug("\t\tIn OPOSForm SetDeviceProperity. OPOSForm is null or OPOSForm not claimed.");
                            }
                            logger.Debug("\t\t--->||| Exiting OPOSForm SetDeviceProperity: " + isSet);
                            Constant.IsInMethod = false;
                        }
                    }
                    Constant.IsStillWrite = false; //Now allow to start a new thread.
                }
            }
            catch (UnauthorizedAccessException uA)
            {
                logger.Error("\t\tError in OPOSForm SetDeviceProperity \n" + uA.ToString());
                Constant.IsStillWrite = false;
                ErrorLog += OPOSForm_ErrorLog;
                ErrorLog.Invoke();
            }
            catch (Exception ex)
            {
                logger.Debug("\t\tError in OPOSForm SetDeviceProperity \n" + ex.ToString());
                Constant.IsStillWrite = false;
                ErrorLog += OPOSForm_ErrorLog;
                ErrorLog.Invoke();
            }
        }


        /// <summary>
        /// Register file silently
        /// </summary>
        /// <param name="dllPath"></param>
        private void registerdll(string dllPath)
        {
            try
            {
                // '/s' indicates regsvr32.exe to run silently
                string fileinfo = '"' + dllPath + '"';
                Process reg = new Process();
                reg.StartInfo.FileName = "regsvr32.exe";
                reg.StartInfo.Arguments = " /s " + fileinfo;
                reg.StartInfo.UseShellExecute = false;
                reg.StartInfo.CreateNoWindow = true;
                reg.StartInfo.RedirectStandardOutput = true;
                reg.Start();
                reg.WaitForExit();
                reg.Close();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        /// <summary>
        /// Description: Register the file if it is not already registered
        ///              This will check the registry to see if the files are register or not
        /// </summary>
        public void RegisterDLLS()
        {
            string[] Dllarray = { "OPOSLineDisplay.ocx", "OPOSMSR.ocx", "OPOSPINPad.ocx", "OPOSSigCap.ocx", "OPOSPOSKeyboard.ocx", "Opos_Constants.dll" };
            string opos64pth = "C:\\Program Files (x86)\\OPOS\\CommonCO";
            string opos32pth = "C:\\Program Files\\OPOS\\CommonCO";
            string vformpath = string.Empty;
            string[] vformdll = { "vFormControlObject.dll", "vfMxSeriesSO.dll", "vHelper.dll", "COMSigCapConvert.dll" };
            string dllpath = string.Empty;
            bool DllFound = false;

            try
            {
                lock (locker)
                {
                    RegistryKey vformobj = null, vformSO = null, vformHelp = null, vComSigCap = null, oDisp = null, oMsr = null, oPinPad = null, oSig = null, oKey = null, oConst = null;
                    if (Directory.Exists(opos64pth)) //64bit OS
                    {
                        DllFound = true;
                        vformpath = "C:\\Program Files (x86)\\VeriFone\\OPOS Install";
                        vformobj = Registry.ClassesRoot.OpenSubKey("Wow6432Node\\CLSID\\{3964D6E9-4420-4744-8092-0AFC6D95857E}");
                        vformSO = Registry.ClassesRoot.OpenSubKey("Wow6432Node\\CLSID\\{448DE762-8CDB-4F18-B8B1-D972524B766E}");
                        vformHelp = Registry.ClassesRoot.OpenSubKey("Wow6432Node\\CLSID\\{7CC49ACF-B8A8-4157-9AF7-548F362FB669}");
                        vComSigCap = Registry.ClassesRoot.OpenSubKey("Wow6432Node\\CLSID\\{3D718822-0462-4CDE-BD01-7409D935081E}");
                        oDisp = Registry.ClassesRoot.OpenSubKey("Wow6432Node\\CLSID\\{CCB90102-B81E-11D2-AB74-0040054C3719}");
                        oMsr = Registry.ClassesRoot.OpenSubKey("Wow6432Node\\CLSID\\{CCB90122-B81E-11D2-AB74-0040054C3719}");
                        oPinPad = Registry.ClassesRoot.OpenSubKey("Wow6432Node\\CLSID\\{CCB90132-B81E-11D2-AB74-0040054C3719}");
                        oSig = Registry.ClassesRoot.OpenSubKey("Wow6432Node\\CLSID\\{CCB90192-B81E-11D2-AB74-0040054C3719}");
                        oKey = Registry.ClassesRoot.OpenSubKey("Wow6432Node\\CLSID\\{CCB90142-B81E-11D2-AB74-0040054C3719}");
                        oConst = Registry.ClassesRoot.OpenSubKey("Wow6432Node\\TypeLib\\{CCB99150-B81E-11D2-AB74-0040054C3719}");
                        dllpath = opos64pth;

                    }
                    else if (Directory.Exists(opos32pth))
                    { //32Bit Os
                        DllFound = true;
                        vformpath = "C:\\Program Files\\VeriFone\\OPOS Install";
                        vformobj = Registry.ClassesRoot.OpenSubKey("CLSID\\{3964D6E9-4420-4744-8092-0AFC6D95857E}");
                        vformSO = Registry.ClassesRoot.OpenSubKey("CLSID\\{448DE762-8CDB-4F18-B8B1-D972524B766E}");
                        vformHelp = Registry.ClassesRoot.OpenSubKey("CLSID\\{7CC49ACF-B8A8-4157-9AF7-548F362FB669}");
                        vComSigCap = Registry.ClassesRoot.OpenSubKey("CLSID\\{3D718822-0462-4CDE-BD01-7409D935081E}");
                        oDisp = Registry.ClassesRoot.OpenSubKey("CLSID\\{CCB90102-B81E-11D2-AB74-0040054C3719}");
                        oMsr = Registry.ClassesRoot.OpenSubKey("CLSID\\{CCB90122-B81E-11D2-AB74-0040054C3719}");
                        oPinPad = Registry.ClassesRoot.OpenSubKey("CLSID\\{CCB90132-B81E-11D2-AB74-0040054C3719}");
                        oSig = Registry.ClassesRoot.OpenSubKey("CLSID\\{CCB90192-B81E-11D2-AB74-0040054C3719}");
                        oKey = Registry.ClassesRoot.OpenSubKey("CLSID\\{CCB90142-B81E-11D2-AB74-0040054C3719}");
                        oConst = Registry.ClassesRoot.OpenSubKey("TypeLib\\{CCB99150-B81E-11D2-AB74-0040054C3719}");
                        dllpath = opos32pth;
                    }

                    if (DllFound)
                    {
                        /* Check each dll to see if they are registered */
                        if (vformobj == null)
                        {
                            registerdll(vformpath + "\\" + vformdll[0]);
                        }
                        if (vformSO == null)
                        {
                            registerdll(vformpath + "\\" + vformdll[1]);
                        }
                        if (vformHelp == null)
                        {
                            registerdll(vformpath + "\\" + vformdll[2]);
                        }
                        if (vComSigCap == null)
                        {
                            registerdll(vformpath + "\\" + vformdll[3]);
                        }
                        if (oDisp == null)
                        {
                            registerdll(dllpath + "\\" + Dllarray[0]);
                        }
                        if (oMsr == null)
                        {
                            registerdll(dllpath + "\\" + Dllarray[1]);
                        }
                        if (oPinPad == null)
                        {
                            registerdll(dllpath + "\\" + Dllarray[2]);
                        }
                        if (oSig == null)
                        {
                            registerdll(dllpath + "\\" + Dllarray[3]);
                        }
                        if (oKey == null)
                        {
                            registerdll(dllpath + "\\" + Dllarray[4]);
                        }
                        if (oConst == null)
                        {
                            registerdll(dllpath + "\\" + Dllarray[5]);
                        }
                    }
                    else
                    {
                        throw new Exception("Cannot find DLL Folder, RegisterDLLS");
                    }
                }
            }
            catch (StackOverflowException sf)
            {
                logger.Error("\t\tError in RegisterDLLs StackOverFlow \n" + sf.ToString());
            }
            catch (Exception ex)
            {
                logger.Error("\t\tError in RegisterDLLs \n" + ex.ToString());
            }
        }

        /// <summary>
        /// Error Event this fire if any error occur when working on the device
        /// </summary>
        void OPOSForm_ErrorLog()
        {
            logger.Debug("\t\tError activated from SetDeviceProperty");
            if (CommandQueue.Count > 0 && !Constant.IsStillWrite)
            {
                CommandEvent += OPOSForm_CommandEvent;
                CommandEvent.Invoke();
            }
        }

        /// <summary>
        /// PINPAD event fire
        /// </summary>
        /// <param name="PinInfo"></param>
        void Pin_PinBlock(ArrayList PinInfo)
        {
            Pin.PinBlock -= Pin_PinBlock;
            if (PinData != null)
            {
                PinData(PinInfo);
            }
        }

        /// <summary>
        /// Get the device information. OS and Bin Information
        /// </summary>
        private void GetDeviceInformation()
        {
            DataTable Tb = new DataTable();
            string App, OS, GUI, UID, OSinfo, addon, UIDInfo;
            string[] getappinfo, getOSinfo, getGUIinfo, getUIDinfo, getOSver, getaddon;
            int appinfo, guiinfo;
            bool isAddon = false;

            try
            {
                if (OposForm != null && OposForm.Claimed == Constant.True)
                {
                    //Get the Bin Version from the PADWELCOME SCREEN
                    Tb = ReadPadXml();
                    if (Tb == null)
                    {
                        return;
                    }

                    logger.Debug("\t\t\t******************* Device Information **************************");
                    OposForm.GetVersion(out App, out OS, out GUI, out UID);
                    getappinfo = App.Split('=');
                    getOSinfo = OS.Split('=');
                    getOSver = getOSinfo[1].Split('/');
                    getaddon = getOSinfo[2].Split(';');
                    getGUIinfo = GUI.Split('=');
                    getUIDinfo = UID.Split('=');
                    addon = "";

                    if (!string.IsNullOrEmpty(getappinfo[1]))
                    {
                        appinfo = Convert.ToInt32(getappinfo[1].Substring(0, 5).Replace(".", ""));
                    }
                    else
                    {
                        appinfo = 0;
                    }

                    if (!string.IsNullOrEmpty(getGUIinfo[1]))
                    {
                        guiinfo = Convert.ToInt32(getGUIinfo[1].Replace(".", ""));
                    }
                    else
                    {
                        guiinfo = 0;
                    }

                    if (!string.IsNullOrEmpty(getOSver[1]))
                    {
                        OSinfo = getOSver[1];
                    }
                    else
                    {
                        OSinfo = "Not Available at this time";
                    }

                    if (getaddon.Length > 0)
                    {
                        for (int i = 0; i < getaddon.Length - 1; i++)
                        {
                            if (getaddon[i].ToUpper().Trim() == Tb.Rows[0]["Addon"].ToString().ToUpper().Trim())
                            {
                                addon = !string.IsNullOrEmpty(getaddon[i].ToUpper().Trim()) ? getaddon[i].ToUpper().Trim() : "Not Available at this time";
                                isAddon = true;
                            }
                        }
                    }

                    //Write to Log Device Information
                    //logger.Debug("\t\t\t**** Bin Version on Pad: " + BinVersion); //Remove
                    logger.Debug("\t\t\t**** Pad OS: " + OSinfo + "\t\tReq OS: " + Tb.Rows[0]["OS"].ToString());
                    logger.Debug("\t\t\t**** Pad App: " + appinfo + "\t\tReq App: " + Tb.Rows[0]["App"].ToString());
                    logger.Debug("\t\t\t**** Pad Gui: " + guiinfo + "\t\tReq Gui: " + Tb.Rows[0]["Gui"].ToString());

                    if (isAddon)
                    {
                        logger.Debug("\t\t\t**** Addon: " + addon);
                    }
                    else
                    {
                        logger.Debug("\t\t\t**** Addon: " + "FAILED");
                    }
                    UIDInfo = !string.IsNullOrEmpty(getUIDinfo[1]) ? getUIDinfo[1].ToString() : "Not Available at this time";
                    logger.Debug("\t\t\t**** UID: " + UIDInfo);
                    logger.Debug("\t\t\t******************* Device Information **************************");
                }
                else
                {
                    throw new Exception("Opos is null or Device Claimed: " + OposForm.Claimed);
                }
            }
            catch (Exception ex)
            {
                logger.Error("Error In GetDeviceInformation()\n: " + ex.ToString());
            }
        }

        /// <summary>
        /// Read the PadInfo.xml into a DataTable to check if the pad is Updated.
        /// </summary>
        /// <returns></returns>
        private DataTable ReadPadXml()
        {
            XmlReader xmlFile;
            xmlFile = XmlReader.Create("PadInfo.xml", new XmlReaderSettings());
            DataSet ds = new DataSet();
            ds.ReadXml(xmlFile);
            return ds.Tables[0];
        }

        /// <summary>
        /// Preparing to pass this data to the event
        /// </summary>
        /// <param name="signData"></param>
        /// <param name="sEventType"></param>
        /// <param name="sScreen"></param>
        private void PadSignData(string signData, string sEventType, string sScreen)
        {
            try
            {
                logger.Debug("\t\tEntering PadSignData");
                if (padSign != null)
                {
                    logger.Debug("\t\tAbout to send PadSign Event");
                    Constant.IsPadEvent = true;
                    padSign(signData, sEventType, sScreen); //Event to pass the data to POS
                    logger.Debug("\t\tPadsign Event successfull");
                }
                else
                {
                    logger.Debug("\t\tPadSignData is null");
                }
                logger.Debug("\t\tExiting PadSignData");
            }
            catch (Exception ex)
            {
                logger.Error("\t\tError in OPOSForm PadSignData \n" + ex.ToString());
            }
        }


        /// <summary>
        /// Reset the Event for the device
        /// </summary>
        private void ResetDataEvent()
        {
            try
            {
                if (OposForm != null && OposForm.Claimed == Constant.True)
                {
                    OposForm.DataEvent -= OposForm_DataEvent;
                    OposForm.DataEvent += OposForm_DataEvent;
                    OposForm.ErrorEvent -= OposForm_ErrorEvent;
                    OposForm.ErrorEvent += OposForm_ErrorEvent;
                    OposForm.DirectIOEvent -= OposForm_DirectIOEvent;
                    OposForm.DirectIOEvent += OposForm_DirectIOEvent;
                }
            }
            catch (Exception ex)
            {
                logger.Error("\t\tError in OPOSForm ResetDataEvent: \n" + ex.ToString());
            }
        }

        /// <summary>
        /// Event from the device itself. If any button is press on the device it will fire this Event
        /// </summary>
        /// <param name="Status"></param>
        private void OposForm_DataEvent(int Status)
        {
            Hashtable Data = new Hashtable();
            string signData = string.Empty;
            try
            {
                logger.Debug("\t\t------ Entering OPOSForm DataEvent -------");
                if (OposForm != null && OposForm.Claimed == Constant.True && OposForm.TotalPoints > 0)
                {
                    //Create a string builder for the signature Data.
                    logger.Debug("\t\tGetting OPOSForm PointArray -- Total Point is: " + OposForm.TotalPoints);
                    StringBuilder sb = new StringBuilder(OposForm.PointArray.Length);
                    sb.Append(OposForm.PointArray.ToString()); //append the points to the string builder
                    signData = sb.ToString(); // get the point and store it to a string to pass to the pos
                    logger.Debug("\t\t////// Signature is fine. ///// ");
                    sb.Clear(); // Clear the string builder.
                }
                logger.Debug("\t\tIn OPOSForm DataEvent, currentscreen: " + CurrentScreen + ", DeviceScreen: " + Constant.DeviceScreen);

                switch (Constant.DeviceScreen.ToUpper().Trim()) //Current screen where the user is clicking on
                {
                    case "PADSIGN": //Credit card signature
                        if (OposForm.TotalPoints > 0)
                        {
                            PadSignData(signData, Constant.Sign, Constant.DeviceScreen);
                        }
                        break;
                    case "PADNOPP": //NOPP Signature
                        switch (OposForm.EventControlData.ToUpper().Trim())
                        {
                            case "0PADNOPP":
                                if (OposForm.EventControlID == 10) //Refuse Button
                                {
                                    //No signature required
                                    logger.Debug("\t\tIn OPOSForm DataEvent: 10 - Refuse Button");
                                    PadSignData(signData, Constant.NoppCancel, Constant.DeviceScreen);
                                }
                                else if (OposForm.EventControlID == 12) //Continue(skip) Button
                                {
                                    //No signature required
                                    logger.Debug("\t\tIn OPOSForm DataEvent: 12 - Skip Button");
                                    PadSignData(signData, Constant.NoppSkip, Constant.DeviceScreen);
                                }
                                else
                                {
                                    if (OposForm.TotalPoints > 0)
                                    {
                                        PadSignData(signData, Constant.Sign, Constant.DeviceScreen);
                                    }
                                }
                                break;
                            default:
                                if (OposForm.TotalPoints > 0 && !string.IsNullOrEmpty(signData))
                                {
                                    PadSignData(signData, Constant.Sign, Constant.DeviceScreen);
                                }
                                break;
                        }
                        break;
                    case "PADRXLIST": //RX Signature
                        switch (OposForm.EventControlData.ToUpper().Trim())
                        {
                            case "0PADRXLIST":
                                if (OposForm.EventControlID == 9 && OposForm.TotalPoints == 0) //Detail Button
                                {
                                    signData = string.Empty; //No signature required
                                    logger.Debug("\t\tIn OPOSForm DataEvent: 9 - Detail Button");
                                    if (!Constant.DetailButtonClick)
                                        PadSignData(signData, Constant.RxDetail, Constant.DeviceScreen);
                                }
                                else if (OposForm.EventControlID == 9 && OposForm.TotalPoints > 0)
                                {
                                    PadSignData(signData, Constant.Sign, Constant.DeviceScreen);
                                }
                                break;
                            case "0YPADRXLIST":
                                if (OposForm.TotalPoints == 0) //Counsel Yes
                                {
                                    logger.Debug("\t\tIn OPOSForm DataEvent: Counseling YES");
                                    Constant.Counsel = Constant.PatientCounselYes;
                                }
                                else if (OposForm.TotalPoints > 0)
                                {
                                    logger.Debug("\t\tIn OPOSForm DataEvent: Counseling YES with signature");
                                    Constant.Counsel = Constant.PatientCounselYes;
                                    PadSignData(signData, Constant.Sign, Constant.DeviceScreen);
                                }
                                break;
                            case "0NPADRXLIST":
                                if (OposForm.TotalPoints == 0) // Counsel No
                                {
                                    logger.Debug("\t\tIn OPOSForm DataEvent: Counseling NO");
                                    Constant.Counsel = Constant.PatientCounselNo;
                                }
                                else if (OposForm.TotalPoints > 0)
                                {
                                    // Data.Add(Constant.Sign, signData);
                                    logger.Debug("\t\tIn OPOSForm DataEvent: Counseling NO with signature");
                                    Constant.Counsel = Constant.PatientCounselNo;
                                    PadSignData(signData, Constant.Sign, Constant.DeviceScreen);
                                }
                                break;
                            default:
                                if (OposForm.TotalPoints > 0 && !string.IsNullOrEmpty(signData))
                                {
                                    if (Constant.Counsel == "NC")
                                        Constant.Counsel = "Y";
                                    PadSignData(signData, Constant.Sign, Constant.DeviceScreen);
                                }
                                break;
                        }
                        break;
                    case "PATIENTCONSENT"://PRIMEPOS-2867 CONSENT
                        {
                            VerifonePatConsent = new PatientConsent();
                            if (OposForm.EventControlID == 7 || OposForm.EventControlID == 8 || OposForm.EventControlID == 9 || OposForm.EventControlID == 10)
                            {
                                radioOptionBtn = OposForm.EventControlID.ToString();
                                //PadSignData(signData, Constant.Sign, Constant.DeviceScreen);
                            }
                            else if ((OposForm.EventControlID == 11 || OposForm.EventControlID == 12 || OposForm.EventControlID == 13) && !string.IsNullOrWhiteSpace(radioOptionBtn))
                            {
                                VerifonePatConsent.ConsentTextID = Convert.ToInt32(oDataSet.Tables["ConsentTextVersion"].Rows[0]["Id"]);
                                if (OposForm.EventControlID == 11 || OposForm.EventControlID == 13)
                                {
                                    if (OposForm.EventControlID == 11)
                                    {
                                        //VerifonePatConsent.ConsentStatusCode = oDataSet.Tables["Consent_Status"].Rows[2]["Code"].ToString();
                                        VerifonePatConsent.IsConsentSkip = true;
                                    }
                                    else
                                    {
                                        logger.Trace("GONE IN REFUSE");
                                        VerifonePatConsent.ConsentStatusID = Convert.ToInt32(oDataSet.Tables["Consent_Status"].Rows[1]["ID"].ToString());
                                        VerifonePatConsent.ConsentStatusCode = oDataSet.Tables["Consent_Status"].Rows[1]["Code"].ToString();
                                    }
                                }
                                else if (OposForm.EventControlID == 12)
                                {
                                    VerifonePatConsent.ConsentStatusID = Convert.ToInt32(VerifonePatConsent.ConsentStatusCode = oDataSet.Tables["Consent_Status"].Rows[0]["ID"].ToString());
                                    VerifonePatConsent.ConsentStatusCode = oDataSet.Tables["Consent_Status"].Rows[0]["Code"].ToString();
                                }
                                if (radioOptionBtn == "7")
                                {
                                    VerifonePatConsent.PatConsentRelationShipDescription = oDataSet.Tables["Consent_Relationship"].Rows[0]["Description"].ToString();
                                    VerifonePatConsent.PatConsentRelationID = Convert.ToInt32(oDataSet.Tables["Consent_Relationship"].Rows[0]["Id"]);
                                }
                                else if (radioOptionBtn == "8")
                                {
                                    VerifonePatConsent.PatConsentRelationShipDescription = oDataSet.Tables["Consent_Relationship"].Rows[1]["Description"].ToString();
                                    VerifonePatConsent.PatConsentRelationID = Convert.ToInt32(oDataSet.Tables["Consent_Relationship"].Rows[1]["Id"]);
                                }
                                else if (radioOptionBtn == "9")
                                {
                                    VerifonePatConsent.PatConsentRelationShipDescription = oDataSet.Tables["Consent_Relationship"].Rows[2]["Description"].ToString();
                                    VerifonePatConsent.PatConsentRelationID = Convert.ToInt32(oDataSet.Tables["Consent_Relationship"].Rows[2]["Id"]);
                                }
                                else if (radioOptionBtn == "10")
                                {
                                    VerifonePatConsent.PatConsentRelationShipDescription = oDataSet.Tables["Consent_Relationship"].Rows[3]["Description"].ToString();
                                    VerifonePatConsent.PatConsentRelationID = Convert.ToInt32(oDataSet.Tables["Consent_Relationship"].Rows[3]["Id"]);
                                }

                                if (OposForm.EventControlID == 11)
                                    PadSignData(signData, "SKIP", Constant.DeviceScreen);
                                else if (OposForm.EventControlID == 13)
                                    PadSignData(signData, "REFUSE", Constant.DeviceScreen);
                                else
                                    PadSignData(signData, "ACCEPT", Constant.DeviceScreen);

                            }
                        }
                        break;
                    case "PADOTC": //OTC
                        if (OposForm.TotalPoints > 0)
                        {
                            PadSignData(signData, Constant.Sign, Constant.DeviceScreen);
                        }
                        break;
                    case "FA_PINE":
                    case "PADSHOWCASH":
                    case "PADMSGSCREEN":
                        break;
                    default:
                        {
                            logger.Debug("-->>>In OPOSForm DataEvent() Default. Unknown screen.");
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                logger.Error("\t\t Error in OPOSForm DataEvent \n" + ex.ToString());
            }
            finally
            {
                ResetDataEvent();
                if (OposForm != null && ConvertToBool(OposForm.Claimed))
                {
                    OposForm.DataEventEnabled = Convert.ToInt32(true);
                    OposForm.DeviceEnabled = Convert.ToInt32(true);
                }
                logger.Debug("\t\t------ Exiting OPOSForm DataEvent ------");
            }
        }
        #endregion
    }

    #region Queue call device command
    /// <summary>
    /// Queue all command so that the POS will not have to wait on the pad
    /// </summary>
    public class CommandStation
    {
        /// <summary>
        /// Store all the device command
        /// </summary>
        public Hashtable DeviceData = null;
        /// <summary>
        /// Store the device screen
        /// </summary>
        public string DeviceScreen { get; set; }
        /// <summary>
        /// Store all command in a Queue 
        /// </summary>
        /// <param name="Screen"></param>
        /// <param name="Data"></param>
        public CommandStation(string sScreen, Hashtable Data)
        {
            if (DeviceData != null)
                DeviceData.Clear();
            else
                DeviceData = new Hashtable();
            DeviceData = Data;
            DeviceScreen = sScreen;
        }
    }

    public class ResendCommandStation
    {
        /// <summary>
        /// Store all the device command
        /// </summary>
        public Hashtable DeviceData = null;
        /// <summary>
        /// Store the device screen
        /// </summary>
        public string DeviceScreen { get; set; }
        /// <summary>
        /// Store all command in a Queue 
        /// </summary>
        /// <param name="Screen"></param>
        /// <param name="Data"></param>
        public ResendCommandStation(string sScreen, Hashtable Data)
        {
            if (DeviceData != null)
                DeviceData.Clear();
            else
                DeviceData = new Hashtable();
            DeviceData = Data;
            DeviceScreen = sScreen;
        }
    }
    #endregion
}
