using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Cryptography;
using RBA_SDK;
using Log.Net;
using System.Reflection;
using System.IO;
using System.Threading;
using NLog;


namespace EDevice
{
    /// <summary>
    /// Developer: Manoj Kumar
    /// </summary>
    /// 
    public class ISCDevices : IEDevice
    {
        private ILogger logger = LogManager.GetCurrentClassLogger();

        private iForm CurrentForm;

        #region Properties
        /// <summary>
        /// Application version on the device
        /// </summary>
        public string AppVersion { get; private set; }
        /// <summary>
        /// Number of Line item on the device
        /// </summary>
        public int LineItemNumber
        {
            get {
                if (LineDisplay == null)
                    return 0;
                else
                    return LineDisplay.Count;
            }
        }

        public string Processor
        {
            private get;
            set;
        }
        /// <summary>
        /// Check if Device is connected
        /// </summary>
        public bool IsConnected { get; private set; }
        /// <summary>
        /// Check is device is disconnected
        /// </summary>
        public bool IsDisconnect { get; private set; }
        /// <summary>
        /// Get the Current Form/Screen
        /// </summary>
        public string GetCurrentForm { get; private set; }
        /// <summary>
        /// RSA Security Parameters
        /// </summary>
        private RSAParameters Params { get; set; }
        /// <summary>
        /// Entry Type of the card.  Swipe, Emv or Contactless
        /// </summary>
        private TagType entryType = TagType.EMV;
        /// <summary>
        /// Payment Tags
        /// </summary>
        public PaymentTags PayTags
        {
            get { return _payTags; }
            set {
                #region StoreProfile settings
                if (value.StoreProfile != null) {
                    if (!string.IsNullOrEmpty(value.StoreProfile.UseStoredProfile.Las4Digits)
                        && !string.IsNullOrEmpty(value.StoreProfile.UseStoredProfile.TokenID)) {
                        IsUseStoredProfile = true;
                        if (value.StoreProfile.IsStoredProfile) {
                            value.StoreProfile.IsStoredProfile = false; // its already stored don't store again
                        }
                    } else {
                        IsUseStoredProfile = false;
                    }
                }
                #endregion StoreProfile settings              
                _payTags = value;
            }
        }
        private PaymentTags _payTags;
        #endregion

        #region Variable
        DataEncryption dataEnc = null;
        private FormProperties.FormSettings cForms = new FormProperties.FormSettings();
        private FormProperties.Properties formP = new FormProperties.Properties();
        private string pFormName = string.Empty;
        private string emvAID = string.Empty;
        private CommSettings.DeviceName DeviceName;
        private bool isDeviceActivated;
        private bool isSwipe;
        private bool IsUpdateLine;
        private bool IsPrevLineDisp;
        private bool isWaitingSign;
        private bool IsUseStoredProfile;
        private readonly object oDeviceEvent = new object();
        private readonly object oShow = new object();
        List<string> LineDisplay = null;
        List<Tuple<string, byte[], string>> SecureDataList = null;
        PaymentTags.StoredProfile mStoredProfile = null;

        #endregion

        #region Events
        delegate void OfflineHandler();
        event OfflineHandler OffLineEvent;

        /// <summary>
        /// Event for the Signature
        /// </summary>
        public event Action<SigFormat, SigStatus, object> SignatureEvent;
        /// <summary>
        /// Button Click Event.  Return the Form name, ID of the button click and Button State (Radio, checkbox)
        /// </summary>
        public event Action<string, string, string> ButtonEvent;
        /// <summary>
        /// Return the Transaction Result
        /// </summary>
        public event Action<Dictionary<string, string>> Result;

        public event Action<string> UserInputEvent;
        #endregion Events

        #region Constructor
        public ISCDevices(FormProperties.FormSettings fs)
        {
            logger.Trace("In COnstructor ISCDevices()");
            this.cForms = fs;
            ManageDLL.CopyFromResource();
        }
        #endregion Constructor

        #region Connection
        /// <summary>
        /// Connection of device.
        /// </summary>
        /// <param name="oCom"></param>
        /// <returns></returns>
        public int Connect(CommSettings oCom)
        {
            logger.Trace("In Connect");


            int Result = 1;
            ERROR_ID result;
            try {
                logger.Trace("Entering Connect()");
                if (!IsDisconnect && !IsConnected) {
                    result = RBA_API.Initialize();
                    if (result == ERROR_ID.RESULT_SUCCESS) {
                        logger.Trace("Initialization Succesful");
                        RBA_API.logHandler = new LogHandler(RBATracer.TraceLog);
                        RBA_API.pinpadHandler = new PinPadMessageHandler(PinPadMessageHandler);
                        RBA_API.SetNotifyRbaDisconnected(new DisconnectHandler(PadDisconnected));
                    } else {
                        logger.Error("Initialization Failed : " + result.ToString());
                    }
                }
                /* Setting up the connection timeout */
                SETTINGS_COMM_TIMEOUTS con_TimeOuts;
                con_TimeOuts.ConnectTimeout = oCom.TimeOut;
                con_TimeOuts.ReceiveTimeout = oCom.TimeOut;
                con_TimeOuts.SendTimeout = oCom.TimeOut;
                result = RBA_API.SetCommTimeouts(con_TimeOuts);

                /* Get the selected Device Name */
                DeviceName = oCom.Device_Name;

                /* Initialize the communication settings */
                SETTINGS_COMMUNICATION comm = new SETTINGS_COMMUNICATION();
                switch (oCom.DeviceInterface) {
                    case CommSettings.eInterface.SERIAL: {
                            logger.Trace("USB-Serial connection, Connecting to port " + oCom.SERIAL.ComPort.ToString());
                            comm.interface_id = (uint)oCom.DeviceInterface;
                            comm.rs232_config.ComPort = "COM" + oCom.SERIAL.ComPort;
                            comm.rs232_config.BaudRate = Convert.ToUInt32(oCom.SERIAL.BaudRate);
                            comm.rs232_config.DataBits = oCom.SERIAL.DataBits;
                            comm.rs232_config.Parity = oCom.SERIAL.Parity;
                            comm.rs232_config.StopBits = oCom.SERIAL.StopBits;
                            comm.rs232_config.FlowControl = oCom.SERIAL.FlowControl;
                            break;
                        }
                    case CommSettings.eInterface.USB: {
                            logger.Trace("USB-HID Connection Selected");
                            /* USB-HID Settings */
                            comm.interface_id = (uint)oCom.DeviceInterface;
                            /* Auto detect VID and PID */
                            if (oCom.USBHID.AutoDetection == CommSettings.UsbHid.AutoDetect.AutoDetectedOn) {
                                logger.Trace("AutoDetecting USB Settings");
                                comm.usbhid_config.autoDetect = RBA_SDK_COMM_CONST.AutoDetectON;
                            }
                            break;
                        }
                }

                /* Connect to PinPad */
                result = RBA_API.Connect(comm);
                if (result == ERROR_ID.RESULT_SUCCESS) {
                    logger.Info("Connection: Start successfull");
                    SetDeviceOnline();
                    this.AppVersion = GetVariable(TagID.REQ_VERSION);
                    logger.Trace("RBA: " + this.AppVersion);
                    SetDeviceOffLine();
                } else if (result == ERROR_ID.RESULT_ERROR_ALREADY_CONNECTED) {
                    result = ERROR_ID.RESULT_SUCCESS;
                    logger.Warn("Connection: Start successfull, Alreay connected");
                } else {
                    logger.Error("An Error Occured while connecting " + result.ToString());
                    throw new Exception(result.ToString());
                }

                /* Successful or failed */
                Result = result == ERROR_ID.RESULT_SUCCESS ? 0 : 1;
                /* Return true or false */
                IsConnected = Result == 0 ? true : false;
                logger.Trace("Exiting Connect()");
            } catch (Exception ex) {
                logger.Fatal(ex, "Device is not connected. Check port setting or cable connection. " + ex.Message);
                throw new Exception("Device is not connected. Check port setting or cable connection." + ex.Message);
            }
            return Result;
        }
        #endregion Connection 

        #region Reconnect
        /// <summary>
        /// Attempts to Reconnect to Isc Pad incase the connection gets Disconnected
        /// </summary>
        /// <returns></returns>
        public bool ReConnnect()
        {
            logger.Trace("In Reconnect()");
            bool bresult = false;
            try {
                ERROR_ID reconnect = RBA_API.Reconnect();

                if (reconnect == ERROR_ID.RESULT_SUCCESS) {
                    logger.Info("Ingenico Sig pad reconnected Sucessfully");
                    bresult = true;
                    IsConnected = true;
                } else {
                    logger.Error("Unable to reconnect " + reconnect.ToString());
                    bresult = false;
                    IsConnected = false;
                }

            } catch (Exception ex) {
                logger.Error(ex, "An Error Occured WHile Reconnecting" + ex.Message);
                bresult = false;
            }

            //RBA_API.


            return bresult;
        }

        #endregion

        #region Check If Pad is Connected
        public bool IsISCConnected()
        {
            bool bconnected = false;
            try {
                CONNECTION_STATUS curr_Status = RBA_API.GetConnectionStatus();
                if (curr_Status == CONNECTION_STATUS.CONNECTED || curr_Status == CONNECTION_STATUS.CONNECTED_NOT_READY) {
                    logger.Info("Sig pad is Connected " + curr_Status.ToString());
                    bconnected = true;
                } else {
                    logger.Error("Sig pad is not Connected " + curr_Status.ToString());
                    bconnected = false;
                }

            } catch (Exception ex) {
                logger.Error(ex, "An Error Occured WHile Checking Connection Status" + ex.Message);
                bconnected = false;
            }
            return bconnected;
        }
        #endregion

        public int SetGatewayParms(GatewayTags tags)
        {
            int result = 1;
            try {
                if (tags.ProcessorName == GatewayTags.Processor.WorldPay) {
                    if (tags != null) {
                        // FirstMile = new ATSSecurePostUI();
                        string strParams = "/ATSID:" + tags.AccID + "/MerchantPIN:" + tags.MerchantPIN;
                        if (!string.IsNullOrEmpty(tags.SubID)) {
                            strParams += "/ATSSubID:" + tags.SubID;
                        }
                        strParams += "/HideGeneral: 1 /HideReceipt: 1 /HideACH: 1 /HideHardware: 1 /HideCheckScanner: 1 /HideSignaturePad: 1 /HideCreditApplication: 1"
                                      + "/HideAutoUpdate: 1 /ProcessOnSwipe: 1 /SuppressAceptedSialog: 1 /AllowTokenization: 1 /MerchantIndustry: R";
                    } else {
                        throw new Exception("Gateway not set");
                    }
                }
            } catch (Exception ex) {
                logger.Fatal(ex, ex.Message);
                throw new Exception(ex.ToString());
            }
            return result;
        }

        #region Disconnection
        /// <summary>
        /// Disconnect the device but does not disconnect from system use.
        /// </summary>
        /// <returns></returns>
        public int Disconnect()
        {
            logger.Trace("In Disconnect");
            int Result = 1;
            try {
                logger.Trace("Entering Disconnect()");
                ERROR_ID result = RBA_API.Disconnect();
                if (result == ERROR_ID.RESULT_SUCCESS) {
                    IsDisconnect = true;
                    GetCurrentForm = string.Empty;
                    Result = 0;
                    logger.Trace("Disconnect Sucessful : Result: " + result.ToString());
                } else {
                    logger.Warn("Unable to DIsconnect : Result: " + result.ToString());
                }
                logger.Trace("Exiting Disconnect()");
            } catch (Exception ex) {
                logger.Fatal(ex, "An Error Occoured when Disconnecting  " + ex.Message);
                throw new Exception(ex.ToString());
            }
            return Result;
        }

        private void PadDisconnected()
        {
            logger.Trace("Ingenico Pad has been Disconnected ");
        }

        private void CallDisconnect()
        {
            logger.Trace("in CallDisconnect()");
            Disconnect();
        }
        #endregion Disconnection  

        #region Shutdown
        /// <summary>
        /// Shutdown the terminal completely
        /// </summary>
        /// <returns></returns>
        public int ShutDown()
        {
            int Result = 1;
            try {
                logger.Trace("Entering ShutDown()");
                ERROR_ID result = RBA_API.Shutdown();
                if (result == ERROR_ID.RESULT_SUCCESS) {
                    GetCurrentForm = string.Empty;
                    Result = 0;
                    isDeviceActivated = false;
                    logger.Info("Shut Down Sucessful Result: " + result.ToString());
                } else {
                    logger.Trace("Shut Down Failed Result: " + result.ToString());
                }
                logger.Trace("Exiting ShutDown()");
            } catch (Exception ex) {
                logger.Fatal(ex, "An Error Occured While Shutting down: " + ex.Message);
                throw new Exception(ex.ToString());
            }
            return Result;
        }
        #endregion Shutdown

        #region Reset
        /// <summary>
        /// This is a Hard Reset message.  The Terminal clear out the previous action.
        /// </summary>
        /// <returns></returns>
        public int Reset()
        {
            int Result = 1;
            try {
                logger.Trace("Entering Reset");
                ERROR_ID result = RBA_API.ProcessMessage(MESSAGE_ID.M10_HARD_RESET);
                if (result == ERROR_ID.RESULT_SUCCESS) {
                    Result = 0;
                    logger.Info("Reset Successful Result: " + result.ToString());
                } else {
                    logger.Warn("Rest Failed Result: " + result.ToString());
                }
                logger.Trace("Exiting Reset");
            } catch (Exception ex) {
                logger.Fatal(ex, "An error Occored while Restting  " + ex.Message);
                throw new Exception(ex.ToString());
            }
            return Result;
        }
        #endregion Reset

        #region Is Form Already Loaded
        /// <summary>
        /// Check if the form is already loaded
        /// </summary>
        /// <param name="formName"></param>
        /// <returns></returns>
        private bool IsLoadedForm(string formName)
        {
            logger.Trace("in IsloadedForm()");
            bool isLoaded = false;
            try {
                logger.Trace("Entering with Form Name: " + formName);
                this.GetCurrentForm = formName;
                if (GetCurrentForm.ToUpper() == pFormName.ToUpper()) {
                    isLoaded = true;
                } else {
                    pFormName = formName;
                    isLoaded = false;
                }
                logger.Trace(formName + " Load Result: " + isLoaded.ToString());
                logger.Trace("Exiting IsLoadedForm()");
            } catch (Exception ex) {
                logger.Fatal(ex, "An error Occoured while Checking Is the form is loaded" + ex.Message);
                throw new Exception(ex.ToString());
            }
            return isLoaded;
        }
        #endregion Is Form Already Loaded
        #region Update Form Element
        private int UpdateFormElement(FormElement element)
        {

            logger.Trace("In UpdateFormElement updating the entire form data");
            if (!IsISCConnected()) {
                ReConnnect();
            }

            int IsUpd = 1;

            try {
                ERROR_ID Result;
                logger.Trace("Setting PARAMETER_ID.P70_REQ_ID_OF_FIELD_TO_CHANGE " + element.ElementName);
                Result = RBA_API.SetParam(PARAMETER_ID.P70_REQ_ID_OF_FIELD_TO_CHANGE, element.ElementName);
                logger.Trace("Result = " + Result.ToString());

                logger.Trace("Setting PARAMETER_ID.P70_REQ_NEW_FIELD_DATA " + element.ElementData);
                Result = RBA_API.SetParam(PARAMETER_ID.P70_REQ_NEW_FIELD_DATA, element.ElementData);
                logger.Trace("Result = " + Result.ToString());

                logger.Trace("Setting MESSAGE_ID.M70_UPDATE_FROM_ELEMENT ");
                Result = RBA_API.ProcessMessage(MESSAGE_ID.M70_UPDATE_FORM_ELEMENT); //PRIMPOS - 2695 - RBA.SDK new version Integration - NileshJ
                logger.Trace("Result = " + Result.ToString());

            } catch (Exception ex) {
                logger.Fatal(ex, "Exception occured " + ex.Message);
                throw new Exception(ex.ToString());
                IsUpd = 0;
            }


            return IsUpd;


        }
        #endregion
        #region Update Multiple Element at once on a form
        /// <summary>
        /// Update all form elements at once
        /// </summary>
        /// <param name="Data"></param>
        /// <returns></returns>
        public int UpdateFormElement(FormProperties.Properties.updateForm Data)
        {
            logger.Trace("In UpdateFormElement updating the entire form data");
            if (!IsISCConnected()) {
                ReConnnect();


            }

            int IsUpd = 1;
            try {
                ERROR_ID Result;
                bool isFound = false;
                var value = cForms.CustomForms.FormList.Find(form => GetCurrentForm != null && form.Item1.ToUpper() == GetCurrentForm.ToUpper()); // Add "GetCurrentForm!=null" for error handling - NileshJ PRIMEPOS-2698 - 25-June-2019
                if (value != null && Data.Update.Count > 0) {
                    foreach (var e in Data.Update) {
                        if (e.Item1 != null) {
                            foreach (var item in e.Item1) {
                                var element = Array.Find(value.Item2, ele => ele.ToUpper().Equals(item.ToUpper()));
                                if (element != null) {
                                    var index = Array.FindIndex(e.Item1, ind => ind.ToUpper() == element.ToUpper());
                                    isFound = string.IsNullOrEmpty(element) ? false : true;

                                    if (isFound && index >= 0) {
                                        var val = e.Item2[index];
                                        logger.Trace("Updating Element " + element);
                                        if (e.Item1.Count() == 1) {

                                            Result = RBA_API.SetParam(PARAMETER_ID.P70_REQ_ID_OF_FIELD_TO_CHANGE, element);
                                            Result = RBA_API.SetParam(PARAMETER_ID.P70_REQ_NEW_FIELD_DATA, val);
                                        } else {


                                            Result = RBA_API.AddParam(PARAMETER_ID.P70_REQ_ID_OF_FIELD_TO_CHANGE, element);
                                            Result = RBA_API.AddParam(PARAMETER_ID.P70_REQ_NEW_FIELD_DATA, val);
                                        }
                                    }
                                }
                            }
                        }
                    }
                    if (isFound) {
                        logger.Trace("Upating Form");
                        Result = RBA_API.ProcessMessage(MESSAGE_ID.M70_UPDATE_FORM_ELEMENT); // PRIMPOS-2695 - RBA.SDK new version Integration - NileshJ
                    }
                }
            } catch (Exception ex) {
                logger.Fatal(ex, "An error Occoured while updating form " + ex.Message);
                throw new Exception(ex.ToString());
            }
            return IsUpd;
        }
        #endregion Update Multiple Element at once on a form

        #region Instantiate Sigbox

        public int ActivateSigBox(string FormName, string FormElementName)
        {
            int Result = 1;

            if (!IsISCConnected()) {
                ReConnnect();
            }
            logger.Trace("Activating Sigbox " + FormElementName + " in " + FormName);
            ERROR_ID result;
            try {

                FormName = FormName.Contains(".K3Z") ? FormName : FormName + ".K3Z";
                if (isWaitingSign && SignatureEvent != null) {
                    SignatureEvent(cForms.Signature.SignReturnFormat, SigStatus.IsCancel, null);
                    isWaitingSign = false;
                }



                logger.Trace("Setting " + FormElementName + " as a text req field");
                result = RBA_API.SetParam(PARAMETER_ID.P24_REQ_TEXT_ELEMENTID, FormElementName);
                logger.Trace("result = " + result.ToString());

                if (!isWaitingSign) {

                    result = RBA_API.SetParam(PARAMETER_ID.P24_REQ_PROMPT_IDX, string.Empty);
                    result = RBA_API.ProcessMessage(MESSAGE_ID.M24_FORM_ENTRY);
                    logger.Trace("Result: " + result.ToString());

                }
                logger.Trace("Sending 20.165 message to activate");
                char FSChar = (char)28;
                result = RBA_API.SendCustomMessage("20.165" + FSChar + FormName, false);
                this.isWaitingSign = true;
                logger.Trace("SigBox 20.165 Result: " + result);

                Result = result == ERROR_ID.RESULT_SUCCESS ? 0 : 1;
            } catch (Exception ex) {
                logger.Fatal(ex, "Exception occured " + ex.Message);
                throw new Exception(ex.ToString());
            }



            return Result;
        }

        #endregion

        #region Show Form
        /// <summary>
        /// Show the form and Data on the screen. 
        /// </summary>
        /// <param name="fp"></param>
        /// <returns></returns>
        public int ShowForm(FormProperties.Properties fp)
        {
            int Result = 1;
            if (!IsISCConnected()) {
                ReConnnect();
            }

            try {
                logger.Trace("Entering ShowForm()");
                string tmpForm = string.Empty;
                bool isSigBox = false;
                ERROR_ID result;
                lock (oShow) {
                    if (IsConnected) {
                        if (!string.IsNullOrWhiteSpace(fp.FormName)) {
                            string formName = fp.FormName.Contains(".K3Z") ? fp.FormName : fp.FormName + ".K3Z";
                            logger.Trace("Loading Form Name: " + formName);

                            /* Check if the form is already loaded */
                            if (!IsLoadedForm(fp.FormName.ToUpper())) {
                                ClearForm(fp.FormName);
                                /* Keep a copy of the current form Property */
                                formP = fp;
                                /* Signature form changed */
                                if (isWaitingSign && SignatureEvent != null) {
                                    SignatureEvent(cForms.Signature.SignReturnFormat, SigStatus.IsCancel, null);
                                    isWaitingSign = false;
                                }
                            }

                            tmpForm = Path.GetFileNameWithoutExtension(fp.FormName);
                            var value = cForms.CustomForms.FormList.Find(form => form.Item1.ToUpper() == tmpForm.ToUpper());
                            if (value != null) {
                                /* Signature Box exist */
                                logger.Trace("Checking If SignatureBox exists ");
                                string sigBox = Array.Find(value.Item2, element => element.ToUpper().Contains("SIGBOX"));
                                if (!string.IsNullOrWhiteSpace(sigBox) && !string.IsNullOrEmpty(fp.FormElementID) && fp.FormElementID.ToUpper().Equals("SIGBOX")) {
                                    char FSChar = (char)28;
                                    result = RBA_API.SendCustomMessage("20.165" + FSChar + formName, false);
                                    this.isWaitingSign = true;
                                    logger.Trace("SigBox 20.165 Result: " + result);
                                } else {
                                    result = RBA_API.SetParam(PARAMETER_ID.P24_REQ_FORM_NUMBER, formName);

                                    switch (fp.FormElementType) {
                                        case FormProperties.Properties.ElementType.Text: {
                                                result = RBA_API.SetParam(PARAMETER_ID.P24_REQ_TYPE_OF_ELEMENT, "T");
                                                break;
                                            }
                                        case FormProperties.Properties.ElementType.Button: {
                                                result = RBA_API.SetParam(PARAMETER_ID.P24_REQ_TYPE_OF_ELEMENT, "B");
                                                break;
                                            }
                                        default: {
                                                break;
                                            }
                                    }
                                }
                            }


                            fp.FormElementID = string.IsNullOrWhiteSpace(fp.FormElementID) ? " " : fp.FormElementID;
                            if (!fp.FormElementID.ToUpper().Equals("SIGBOX") && !string.IsNullOrEmpty(fp.FormElementID)) {
                                result = RBA_API.SetParam(PARAMETER_ID.P24_REQ_TEXT_ELEMENTID, fp.FormElementID);
                            }

                            if (fp.FormElementID.ToUpper().Contains("LINEDISPLAY")) {
                                result = RBA_API.ProcessMessage(MESSAGE_ID.M24_FORM_ENTRY);
                                /* Use to write to line display */
                                WriteToLineDisplay(fp.FormElementData);

                                logger.Trace("LineDisplay Result: " + result.ToString());
                                Result = result == ERROR_ID.RESULT_SUCCESS ? 0 : 1;
                            } else {
                                /* Write all other Properties */
                                if (!isWaitingSign && !string.IsNullOrEmpty(fp.FormElementID)) {
                                    fp.FormElementData = string.IsNullOrWhiteSpace(fp.FormElementData) ? " " : fp.FormElementData;
                                    result = RBA_API.SetParam(PARAMETER_ID.P24_REQ_PROMPT_IDX, fp.FormElementData);
                                    result = RBA_API.ProcessMessage(MESSAGE_ID.M24_FORM_ENTRY);
                                    logger.Trace("Result: " + result.ToString());
                                    Result = result == ERROR_ID.RESULT_SUCCESS ? 0 : 1;
                                }
                            }
                            logger.Trace("Exiting SHowForm()");
                        }
                    } else {
                        if (!IsConnected) {
                            logger.Error("Device is not connected, please check connection.");
                            throw new Exception("Device is not connected, please check connection.");
                        } else {
                            logger.Error("Form Properties are not set \n please set before call this method.");
                            throw new Exception("Form Properties are not set \n please set before call this method.");
                        }
                    }
                }
            } catch (Exception ex) {
                logger.Fatal(ex, "Exception occured " + ex.Message);
                throw new Exception(ex.ToString());
            }
            return Result;
        }


        public int LoadForm(string FormName)
        {
            int Result = 1;
            if (!IsISCConnected()) {
                ReConnnect();
            }

            ERROR_ID result;

            try {
                if (!string.IsNullOrWhiteSpace(FormName)) {
                    string tmpFormName = FormName.Contains(".K3Z") ? FormName : FormName + ".K3Z";
                    logger.Trace("Loading Form Name: " + FormName);

                    if (!IsLoadedForm(FormName.ToUpper())) {
                        //ClearForm(FormName);
                        /* Keep a copy of the current form Property */
                        //formP = fp;
                        /* Signature form changed */
                        if (isWaitingSign && SignatureEvent != null) {
                            SignatureEvent(cForms.Signature.SignReturnFormat, SigStatus.IsCancel, null);
                            isWaitingSign = false;
                        }
                    }

                    logger.Trace("Sending P24_REQ_FORM_NUMBER(149)");
                    result = RBA_API.SetParam(PARAMETER_ID.P24_REQ_FORM_NUMBER, tmpFormName);
                    logger.Trace("result = " + result.ToString());

                    logger.Trace("Sending MESSAGE_ID.M24_FORM_ENTRY(24)");
                    result = RBA_API.ProcessMessage(MESSAGE_ID.M24_FORM_ENTRY);
                    logger.Trace("Result: " + result.ToString());


                    Result = result == ERROR_ID.RESULT_SUCCESS ? 0 : 1;

                } else {
                    string message = "FormName is Empty";
                    logger.Fatal(message);
                    throw new Exception(message);
                }
            } catch (Exception ex) {
                logger.Fatal(ex, "Exception occured " + ex.Message);
                throw new Exception(ex.ToString());
            }
            return Result;
        }

        public int LoadForm(iForm form)
        {
            logger.Trace("In LoadForm() with iForm Interface");
            int Result = 1;
            if (!IsISCConnected()) {
                ReConnnect();
            }
            CurrentForm = form;



            try {
                if (!string.IsNullOrWhiteSpace(form.FormName)) {

                    #region PRIMEPOS-3238
                    ////Loading Form
                    //Result = LoadForm(form.FormName);

                    ////Clearing any Current Data
                    Result = ClearForm(form); //PRIMEPOS-3238T

                    //Updating line Display
                    if (form.HasLineDisplay)
                    {
                        foreach (string item in form.LineItemData)
                        {
                            Result = WriteToLineDisplay(item);
                        }
                    }
                    #endregion

                    //Upating form element
                    foreach (FormElement element in form.FormItems) {
                        Result = UpdateFormElement(element);
                    }

                    //Loading Form
                    Result = LoadForm(form.FormName); //PRIMEPOS-3238

                    ////Updating line Display
                    //if (form.HasLineDisplay) {
                    //    foreach (string item in form.LineItemData) {
                    //        Result = WriteToLineDisplay(item);
                    //    }
                    //}

                    //Instanciate Text Box
                    if (form.HasSigBox) {
                        Result = ActivateSigBox(form.FormName, form.SigBoxName);
                    }



                } else {
                    logger.Fatal("No form name detected");
                    throw new Exception("No form name detected");
                    Result = 0;
                }

            } catch (Exception ex) {
                logger.Fatal(ex, "Exception occured " + ex.Message);
                throw new Exception(ex.ToString());
                Result = 0;
            }


            return Result;
        }

        #endregion ShowForm

        #region Update Line Display
        public int UpdateLineDisplay(int index, string Data)
        {
            logger.Trace("In UpdateLineDisplay(" + index.ToString() + "," + Data + ")");
            if (!IsISCConnected()) {
                ReConnnect();
            }

            int update = 1;
            try {
                lock (oDeviceEvent) {
                    if (LineDisplay != null && LineDisplay.Count > 0) {
                        LineDisplay.RemoveAt(index);
                        LineDisplay.Insert(index, Data);
                        if (ClearDisplayMsg(false) == 0) {
                            IsUpdateLine = true;
                            foreach (var item in LineDisplay) {
                                formP.FormElementData = item;
                                ShowForm(formP);
                            }
                            IsUpdateLine = false;
                            update = 0;
                        }
                    }
                }
            } catch (Exception ex) {
                logger.Fatal(ex, "An error Occured while updating LIne Display " + ex.Message);
                throw new Exception(ex.ToString());
            }
            return update;
        }
        #endregion Update Line Display

        #region Remove item from Line Display
        /// <summary>
        /// Remove a line iteam in the Line Display by index.  Index are zero(0) base.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public int RemoveFromLineDisplay(int index)
        {
            logger.Trace("in RemoveFromLineDisplay() for index: " + index.ToString());
            if (!IsISCConnected()) {
                ReConnnect();
            }
            int remove = 1;
            try {
                List<string> tmpLineDisplay;
                logger.Trace("Entering with index: " + index);
                lock (oDeviceEvent) {
                    if (LineDisplay != null && LineDisplay.Count > 0) {
                        logger.Trace("Number of items in LineDisplay before deletation: " + LineDisplay.Count);
                        LineDisplay.RemoveAt(index);
                        tmpLineDisplay = new List<string>();
                        tmpLineDisplay = LineDisplay.ToList();
                        if (ClearDisplayMsg(false) == 0) {
                            IsUpdateLine = true;
                            foreach (var item in tmpLineDisplay) {
                                formP.FormElementData = item;
                                ShowForm(formP);
                            }
                            IsUpdateLine = false;
                            remove = 0;
                        }
                    }
                }
                logger.Trace("Exiting RemoveFromLineDisplay()");
            } catch (Exception ex) {
                logger.Fatal("An Error Occored when removing Item from Line Display" + ex.Message);
                throw new Exception(ex.ToString());
            }
            return remove;
        }
        #endregion Remove item from Line Display

        #region Write to Line Display
        public int WriteToLineDisplay(string FormData)
        {
            int Result = 1;
            if (!IsISCConnected()) {
                ReConnnect();
            }
            try {
                logger.Trace("Entering Write to LineDisplay() with data " + FormData);
                /*Strolling of Line Display */
                string VarID = "000104";
                ERROR_ID result;

                if (LineDisplay == null) {
                    LineDisplay = new List<string>();
                }
                result = RBA_API.SetParam(PARAMETER_ID.P28_REQ_RESPONSE_TYPE, "1");
                result = RBA_API.SetParam(PARAMETER_ID.P28_REQ_VARIABLE_ID, VarID);
                result = RBA_API.SetParam(PARAMETER_ID.P28_REQ_VARIABLE_DATA, FormData);
                result = RBA_API.ProcessMessage(MESSAGE_ID.M28_SET_VARIABLE);
                if (result == ERROR_ID.RESULT_SUCCESS) {
                    Result = 0;
                    if (!IsUpdateLine) {
                        LineDisplay.Add(FormData);
                    }
                    string P28Status = RBA_API.GetParam(PARAMETER_ID.P28_RES_STATUS);
                    string P28VarID = RBA_API.GetParam(PARAMETER_ID.P28_RES_VARIABLE_ID);
                    logger.Trace("LineDisplay Add Result: " + result.ToString());
                } else {
                    logger.Trace("Failed to write to device Line Display " + result.ToString());
                }
            } catch (Exception ex) {
                logger.Fatal("An error Occoured while writing to LineDisplay " + ex.Message);
                throw new Exception(ex.ToString());
            }
            return Result;
        }
        #endregion Write to Line Display

        #region Line Display
        #region Clear Line Display
        /// <summary>
        /// Clear the LineDisplay
        /// </summary>
        /// <returns></returns>
        public int ClearLineDisplay(string formName)
        {
            int Result = 1;
            if (!IsISCConnected()) {
                ReConnnect();
            }
            try {
                logger.Trace("Entering with form " + formName);
                var value = cForms.CustomForms.FormList.Find(form => form.Item1.ToUpper() == formName.ToUpper());
                if (value != null) {
                    logger.Trace(formName + " found about to clear Line Display");
                    for (int i = 0; i < value.Item2.Count(); i++) {
                        if (value.Item2[i].ToUpper().Contains("LINEDISPLAY")) {
                            Result = ClearDisplayMsg(true);
                        }
                    }
                } else {
                    logger.Trace("Form not found!");
                }
            } catch (Exception ex) {
                logger.Fatal("An error Occured while trying to clear LineDisplay " + ex.Message);
                throw new Exception(ex.ToString());
            }
            return Result;
        }
        #endregion Clear Line Display

        #region Clear Line Display Message
        /// <summary>
        /// Clear all line display entry
        /// </summary>
        /// <returns></returns>
        private int ClearDisplayMsg(bool clear)
        {
            int Result = 1;
            if (!IsISCConnected()) {
                ReConnnect();
            }
            try {
                logger.Trace("Entering ClearDisplayMsg()");
                string ClearDisplay = "15.8";
                ERROR_ID result = RBA_API.SendCustomMessage(ClearDisplay, false);
                if (result == ERROR_ID.RESULT_SUCCESS) {
                    Result = 0;
                    if (clear && LineDisplay != null) {
                        LineDisplay.Clear();
                    }
                    logger.Trace("Result: " + result.ToString());
                }
                logger.Trace("Exiting ClearDisplayMethord()");
            } catch (Exception ex) {
                logger.Fatal(ex, "An error occured while Clearing Line Display Messages " + ex.Message);
                throw new Exception(ex.ToString());
            }
            return Result;
        }
        #endregion Clear Line Display Message
        #endregion Line Display

        #region Clear Form
        /// <summary>
        /// Clear any form by using the FORMNAME
        /// </summary>
        /// <param name="formName"></param>
        /// <returns></returns>
        public int ClearForm(string formName)
        {
            int Result = 1;

            if (!IsISCConnected()) {
                ReConnnect();
            }

            ERROR_ID result;
            try {
                logger.Trace("Entering ClearForm, Form Name: " + formName);

                var value = cForms.CustomForms.FormList.Find(form => form.Item1.ToUpper() == formName.ToUpper());
                if (value != null) {
                    logger.Trace("Form found, will clear now");

                    /*Make sure the previous Line display is clear*/
                    if (IsPrevLineDisp) {
                        Result = ClearDisplayMsg(true);
                        IsPrevLineDisp = false;
                    }

                    for (int i = 0; i < value.Item2.Count(); i++) {
                        if (value.Item2[i].ToUpper().Contains("LINEDISPLAY")) {
                            Result = ClearDisplayMsg(true);
                            IsPrevLineDisp = true;
                        } else {
                            result = RBA_API.SetParam(PARAMETER_ID.P70_REQ_ID_OF_FIELD_TO_CHANGE, value.Item2[i]);
                            result = RBA_API.SetParam(PARAMETER_ID.P70_REQ_NEW_FIELD_DATA, " ");
                            result = RBA_API.ProcessMessage(MESSAGE_ID.M70_UPDATE_FORM_ELEMENT); // PRIMPOS-2695 - RBA.SDK new version Integration - NileshJ
                        }
                    }
                }
                logger.Trace("Exiting ClearForm()");
            } catch (Exception ex) {
                logger.Fatal(ex, "An error Occured While Clearing Form " + ex.Message); ;
                throw new Exception(ex.ToString());
            }
            return Result;
        }

        /// <summary>
        /// Clear any form by using the iForm Interface
        /// </summary>
        /// <param name="form">Form object inherited from iForm interface</param>
        /// <returns></returns>
        public int ClearForm(iForm form)
        {
            int Result = 1;

            if (!IsISCConnected()) {
                ReConnnect();
            }

            ERROR_ID result;
            try {
                logger.Trace("Entering ClearForm, Form Name: " + form.FormName);


                foreach (FormElement item in form.FormItems) {
                    logger.Trace("Sending P70_REQ_ID_OF_FIELD_TO_CHANGE message with Elementname: " + item.ElementName);
                    result = RBA_API.SetParam(PARAMETER_ID.P70_REQ_ID_OF_FIELD_TO_CHANGE, item.ElementName);
                    logger.Trace("Result = " + result.ToString());

                    logger.Trace("Sending P70_REQ_NEW_FIELD_DATA  " + item.ElementData);
                    result = RBA_API.SetParam(PARAMETER_ID.P70_REQ_NEW_FIELD_DATA, " ");
                    logger.Trace("Result = " + result.ToString());
                }
                logger.Trace("Sending M70_UPDATE_FROM_ELEMENT  ");
                result = RBA_API.ProcessMessage(MESSAGE_ID.M70_UPDATE_FORM_ELEMENT); // PRIMPOS-2695 - RBA.SDK new version Integration - NileshJ
                logger.Trace("Result = " + result.ToString());




                if (form.HasLineDisplay) {
                    Result = ClearDisplayMsg(true);

                }

                logger.Trace("Exiting ClearForm()");
            } catch (Exception ex) {
                logger.Fatal(ex, "An error Occured While Clearing Form " + ex.Message); ;
                throw new Exception(ex.ToString());
            }
            return Result;
        }

        #endregion Clear Form

        #region Set Device Online
        /// <summary>
        /// Set the device online to process payment. This will activate the Swiper,
        ///  EMV Tray and NFC is possible.
        /// </summary>
        /// <returns></returns>
        private int SetDeviceOnline()
        {
            int Result = 1;
            try {
                logger.Trace("Entering SetDeviceOnline()");

                ERROR_ID result;
                result = RBA_API.SetParam(PARAMETER_ID.P01_REQ_APPID, "0000");
                result = RBA_API.SetParam(PARAMETER_ID.P01_REQ_PARAMID, "0000");
                result = RBA_API.ProcessMessage(MESSAGE_ID.M01_ONLINE);
                //E2EESwipe();

                if (result == ERROR_ID.RESULT_SUCCESS) {
                    string APPID = RBA_API.GetParam(PARAMETER_ID.P01_RES_APPID);
                    string PARAMID = RBA_API.GetParam(PARAMETER_ID.P01_RES_PARAMID);
                    string AppVersion = RBA_API.GetParam(PARAMETER_ID.P07_RES_APPLICATION_VERSION);

                    Result = 0;
                    logger.Trace("Set Device Online Sicessful Result: " + result.ToString());
                    this.isDeviceActivated = true;
                    this.isSwipe = false;
                }
                logger.Trace("Exiting SetDeviceOnline()");
            } catch (Exception ex) {
                logger.Fatal(ex, "An error occured while setting device online " + ex.ToString());
                throw new Exception(ex.ToString());
            }
            return Result;
        }
        #endregion Set Device Online

        #region Set Device Offline
        private void SetDeviceOffLine()
        {
            try {
                logger.Trace("Entering SetDeviceOffline()");
                ERROR_ID result = RBA_API.ProcessMessage(MESSAGE_ID.M00_OFFLINE);
                if (result == ERROR_ID.RESULT_SUCCESS) {
                    isDeviceActivated = false;
                }
                logger.Trace("Set Device Offline Result: " + result.ToString());
                logger.Trace("Exiting SetDeviceOffline()");
            } catch (Exception ex) {
                logger.Fatal(ex, "An Error Occured While Setting Device Offline " + ex.ToString(), ErrorLevel.Critical);
                throw new Exception(ex.ToString());
            }
        }
        #endregion Set Device Offline

        #region Cancel Transaction
        public int CancelTransaction()
        {
            int Result = 1;
            try {
                Reset();
                SetDeviceOffLine();
                Result = 0;
            } catch (Exception ex) {
                logger.Fatal(ex, ex.Message);
                throw new Exception(ex.ToString());
            }
            return Result;
        }
        #endregion Cancel Transaction

        #region Process Transaction
        public int ProcessTransaction(PaymentTags tags)
        {
            int Result = 1;
            try {
                if (tags != null && tags.Amount.TotalAmount > 0) {
                    this.PayTags = new PaymentTags();
                    this.PayTags = tags;
                    switch (tags.TransactionType) {
                        case PaymentTags.transactionType.Sale: {
                                #region Setup Sales                              
                                if (IsUseStoredProfile) {

                                } else {
                                    if (!isDeviceActivated) {
                                        SetDeviceOnline();
                                    }
                                    SetRBAFlowEMV(tags);
                                }
                                #endregion Setup Sales                           
                                break;
                            }
                        case PaymentTags.transactionType.Void:
                        case PaymentTags.transactionType.Credit: {
                                #region Setup Credit Sale
                                entryType = TagType.MANUAL;
                                SecureData(Tags.HISTID, tags.Reversal.HistoryID.ToByte(), entryType);
                                SecureData(Tags.ORDID, tags.Reversal.OrderID.ToByte(), entryType);
                                SendTransaction(entryType);
                                #endregion Setup Credit Sale
                                break;
                            }
                    }
                    Result = 0;
                }
            } catch (Exception ex) {
                logger.Fatal(ex, ex.Message);
                throw new Exception(ex.ToString());
            }
            return Result;
        }
        #endregion Process Transaction

        #region User Input Forms
        public bool UserInputForm(ISC.InputTags.PromptTags prompt, ISC.InputTags.FormatTags format)
        {
            bool result = false;
            ERROR_ID Result;
            switch (format) {
                case ISC.InputTags.FormatTags.Text: {
                        Result = RBA_API.SetParam(PARAMETER_ID.P27_REQ_DISPLAY_CHAR, "0");
                        Result = RBA_API.SetParam(PARAMETER_ID.P27_REQ_PROMPT_INDEX, ((int)prompt).ToString());
                        Result = RBA_API.SetParam(PARAMETER_ID.P27_REQ_MAX_INPUT_LENGTH, "50");
                        Result = RBA_API.SetParam(PARAMETER_ID.P27_REQ_MIN_INPUT_LENGTH, "00");
                        Result = RBA_API.SetParam(PARAMETER_ID.P27_REQ_FORMAT_SPECIFIER, ((int)format).ToString());
                        Result = RBA_API.SetParam(PARAMETER_ID.P27_REQ_FORM_SPECIFICID, "alpha.K3Z");
                        Result = RBA_API.ProcessMessage(MESSAGE_ID.M27_ALPHA_INPUT);
                        break;
                    }
                case ISC.InputTags.FormatTags.SSN:
                case ISC.InputTags.FormatTags.Phone:
                case ISC.InputTags.FormatTags.DOB: {
                        Result = RBA_API.SetParam(PARAMETER_ID.P21_REQ_FORMAT_SPECIFIER, ((int)format).ToString());
                        Result = RBA_API.SetParam(PARAMETER_ID.P21_REQ_PROMPT_INDEX, ((int)prompt).ToString());
                        Result = RBA_API.SetParam(PARAMETER_ID.P21_REQ_MAX_INPUT_LENGTH, "00");
                        Result = RBA_API.SetParam(PARAMETER_ID.P21_REQ_MIN_INPUT_LENGTH, "50");
                        Result = RBA_API.SetParam(PARAMETER_ID.P21_REQ_DISPLAY_CHAR, "0");
                        Result = RBA_API.SetParam(PARAMETER_ID.P21_REQ_FORM_SPECIFIC_INDEX, "INPUT.K3Z");
                        Result = RBA_API.ProcessMessage(MESSAGE_ID.M21_NUMERIC_INPUT);
                        break;
                    }
                case ISC.InputTags.FormatTags.Numbers: {
                        Result = RBA_API.SetParam(PARAMETER_ID.P21_REQ_FORMAT_SPECIFIER, ((int)format).ToString());
                        Result = RBA_API.SetParam(PARAMETER_ID.P21_REQ_PROMPT_INDEX, ((int)prompt).ToString());
                        Result = RBA_API.SetParam(PARAMETER_ID.P21_REQ_MAX_INPUT_LENGTH, "00");
                        Result = RBA_API.SetParam(PARAMETER_ID.P21_REQ_MIN_INPUT_LENGTH, "50");
                        Result = RBA_API.SetParam(PARAMETER_ID.P21_REQ_DISPLAY_CHAR, "0");
                        Result = RBA_API.SetParam(PARAMETER_ID.P21_REQ_FORM_SPECIFIC_INDEX, "INPUT.K3Z");
                        Result = RBA_API.ProcessMessage(MESSAGE_ID.M21_NUMERIC_INPUT);
                        break;
                    }
            }
            return result;
        }
        #endregion User Input 

        #region Rx Amount
        /// <summary>
        /// Get the Rx Amount if any
        /// </summary>
        /// <returns></returns>
        private double RxAmount()
        {
            double rx = 0;
            if (PayTags.FSA.IsHealthCare) {
                if (PayTags.FSA.AutoSubstantiation.RxAmount > 0 || PayTags.FSA.AutoSubstantiation.TotalHealthCareAmount > 0) {
                    rx = PayTags.FSA.AutoSubstantiation.TotalHealthCareAmount;
                }
            }
            return rx;
        }
        #endregion Rx Amount

        #region Set Stored Profile
        /// <summary>
        /// Check if card need to be stored
        /// </summary>
        private void SetStoredProfile()
        {
            string isStored = string.Empty;
            if ((mStoredProfile != null && mStoredProfile.IsStoredProfile) || (PayTags.StoreProfile != null && PayTags.StoreProfile.IsStoredProfile)) {
                isStored = Convert.ToString(true);
                SecureData(Tags.STOREDPROFILE, isStored.ToByte(), entryType);
            }
        }
        #endregion Set Stored Profile

        #region Stored Profile
        private void StoredProfileSale()
        {
            entryType = TagType.STOREDPROFILE;
            SecureData(Tags.TOKENID, PayTags.StoreProfile.UseStoredProfile.TokenID.ToByte(), entryType);
            SecureData(Tags.LAST4, PayTags.StoreProfile.UseStoredProfile.Las4Digits.ToByte(), entryType);
        }
        #endregion Stored Profile

        #region Send Transaction to Gateway
        private string SendTransaction(TagType entryType)
        {
            string status = string.Empty;
            Dictionary<string, string> result = new Dictionary<string, string>();
            try {
                SecureData(Tags.TAMT, ((float)PayTags.Amount.TotalAmount).ToString().ToByte(), entryType);
                if (RxAmount() > 0) {
                    SecureData(Tags.RXAMOUNT, RxAmount().ToString().ToByte(), entryType);
                }
                if (IsUseStoredProfile) {
                    // StoredProfileSale(); 
                } else {
                    // SetStoredProfile();
                }
                Transmission send = new Transmission();
                result = send.SendToGateway(SecureDataList, DataEncryption.DEK, PayTags).ToDictionary();
                foreach (var r in result) {
                    if (r.Key.ToUpper() == "STATUS") {
                        status = r.Value.ToString();
                    }
                }

                if (Result != null) {
                    Result(result);
                }
                #region Comment
                //CustomPayMessage(result);
                //Hashtable ht = new Hashtable();
                //if (result == "Approved")
                //{
                //    if (PayEvent != null)
                //    {
                //        PayEvent(true, ht, UITags.ReturnStatus.Approve);
                //    }
                //}
                //else
                //{
                //    if (PayEvent != null)
                //    {
                //        PayEvent(true, ht, UITags.ReturnStatus.Decline);
                //    }
                //}
                //CustomPayMessage("Approved... Authorization: " + FakeNum());

                //formP.FormName = "MFSIGN.K3Z";
                //formP.FormElementType = FormProperties.Properties.ElementType.Text;
                //formP.FormElementID = "Sigbox";
                //formP.FormElementData = "";
                //ShowForm(formP);
                //isFInish = true;
                #endregion Comment               
            } catch (Exception ex) {
                logger.Fatal(ex, ex.Message);
                throw new Exception(ex.ToString());
            }
            return status;
        }
        #endregion Send Transaction to Gateway

        #region SetRBAFlow Amount, Transaction Type, and Payment Type
        private int SetRBAFlowEMV(PaymentTags Tags)
        {
            int Result = 1;
            try {
                //logger.Trace( "Entering");
                ERROR_ID result;
                if (Tags != null) {
                    /* Set the Total AMount 13.0*/
                    result = RBA_API.SetParam(PARAMETER_ID.P13_REQ_AMOUNT, (Tags.Amount.TotalAmount * 100).ToString());
                    result = RBA_API.ProcessMessage(MESSAGE_ID.M13_AMOUNT);
                    //logger.Trace( "Total Amount: " + Tags.Amount.TotalAmount.ToString());

                    /* Set the Transaction Type 14.0*/
                    result = RBA_API.SetParam(PARAMETER_ID.P14_REQ_TXN_TYPE, "0" + Convert.ToInt32(Tags.TransactionType).ToString());
                    result = RBA_API.ProcessMessage(MESSAGE_ID.M14_SET_TXN_TYPE);
                    // logger.Trace( "Transaction Type: " + Convert.ToInt32(Tags.TransactionType).ToString());

                    /* Force Payment Type 04.0*/
                    result = RBA_API.SetParam(PARAMETER_ID.P04_REQ_FORCE_PAYMENT_TYPE, "0");
                    result = RBA_API.SetParam(PARAMETER_ID.P04_REQ_PAYMENT_TYPE, ((char)Tags.PaymentType).ToString());
                    result = RBA_API.SetParam(PARAMETER_ID.P04_REQ_AMOUNT, (Tags.Amount.TotalAmount * 100).ToString());
                    result = RBA_API.ProcessMessage(MESSAGE_ID.M04_SET_PAYMENT_TYPE);
                    //logger.Trace( "Payment Type: " + ((char)Tags.PaymentType).ToString());

                    Result = result == ERROR_ID.RESULT_SUCCESS ? 0 : 1;
                }
                //logger.Trace( "Exiting");
            } catch (Exception ex) {
                logger.Fatal(ex, ex.Message);
                throw new Exception(ex.ToString());
            }
            return Result;
        }
        #endregion Amount, Transaction Type, and Payment Type

        #region Activate Emv Transaction
        private void ActivateEMV()
        {
            /* Force Payment Type */
            //logger.Trace( "Entering ActivateEMV()");
            ERROR_ID result;
            result = RBA_API.SetParam(PARAMETER_ID.P04_REQ_FORCE_PAYMENT_TYPE, "0");
            result = RBA_API.SetParam(PARAMETER_ID.P04_REQ_PAYMENT_TYPE, ((char)PayTags.PaymentType).ToString());
            result = RBA_API.SetParam(PARAMETER_ID.P04_REQ_AMOUNT, (PayTags.Amount.TotalAmount * 100).ToString());
            result = RBA_API.ProcessMessage(MESSAGE_ID.M04_SET_PAYMENT_TYPE);

            result = RBA_API.SetParam(PARAMETER_ID.P13_REQ_AMOUNT, (PayTags.Amount.TotalAmount * 100).ToString());
            result = RBA_API.ProcessMessage(MESSAGE_ID.M13_AMOUNT);
            //logger.Trace( "Exiting ActivateEMV()");
        }
        #endregion Activate Emv Transaction

        #region Send EMV Transaction for Authorization 33.04
        private void EMVAuthorizeResponse()
        {
            //logger.Trace( "Entering Authorizing Response()");
            ERROR_ID Result;
            Result = RBA_API.SetParam(PARAMETER_ID.P33_04_RES_STATUS, "00");
            Result = RBA_API.SetParam(PARAMETER_ID.P33_04_RES_EMVH_CURRENT_PACKET_NBR, "0");
            Result = RBA_API.SetParam(PARAMETER_ID.P33_04_RES_EMVH_PACKET_TYPE, "0");

            string status = SendTransaction(entryType);
            Result = RBA_API.AddTagParam(MESSAGE_ID.M33_04_EMV_AUTHORIZATION_RESPONSE, 0x8A, status);
            Result = RBA_API.ProcessMessage(MESSAGE_ID.M33_04_EMV_AUTHORIZATION_RESPONSE);
            //logger.Trace( "Exiting Authorizing Response()");
        }
        #endregion Send EMV Transaction for Authorization 33.04

        #region EMV Transaction Initiate Status 33.01
        private int InitiateEMVStatus()
        {
            int Result = 1;
            try {
                // logger.Trace( "Entering");
                ERROR_ID result;
                result = RBA_API.ResetParam(PARAMETER_ID.P_ALL_PARAMS);
                result = RBA_API.SetParam(PARAMETER_ID.P33_01_REQ_STATUS, "00");
                result = RBA_API.SetParam(PARAMETER_ID.P33_01_REQ_EMVH_CURRENT_PACKET_NBR, "0");
                result = RBA_API.SetParam(PARAMETER_ID.P33_01_REQ_EMVH_PACKET_TYPE, "0");
                result = RBA_API.ProcessMessage(MESSAGE_ID.M33_01_EMV_STATUS);
                Result = result == ERROR_ID.RESULT_SUCCESS ? 0 : 1;
                //logger.Trace( "Exiting, Result: " + Result);
            } catch (Exception ex) {
                //logger.Trace( ex.ToString(), ErrorLevel.Critical);
                logger.Fatal(ex, ex.Message);
                throw new Exception(ex.ToString());
            }
            return Result;
        }
        #endregion EMV Transaction Initiate Status 33.01

        #region Set Terminal Capabilities 33.07
        /// <summary>
        /// EMV 33.03 Messges
        /// </summary>
        /// <returns></returns>
        private int SetTerminalCapabilities()
        {
            int Result = 1;
            int secondByte = Convert.ToInt32("F8", 16);
            byte[] TerminalCap = new byte[] { 0xE0, 0x00, 0xC0 };
            ERROR_ID result;
            try {
                //logger.Trace( "Entering");
                result = RBA_API.SetParam(PARAMETER_ID.P33_07_RES_STATUS, "00");
                result = RBA_API.SetParam(PARAMETER_ID.P33_07_RES_EMVH_CURRENT_PACKET_NBR, "0");
                result = RBA_API.SetParam(PARAMETER_ID.P33_07_RES_EMVH_PACKET_TYPE, "0");
                TerminalCap[1] = (byte)secondByte;
                byte[] AID = emvAID.ToByte();

                result = RBA_API.AddTagParam(MESSAGE_ID.M33_07_EMV_TERMINAL_CAPABILITIES, TagID.Tag84, AID);
                result = RBA_API.AddTagParam(MESSAGE_ID.M33_07_EMV_TERMINAL_CAPABILITIES, TagID.Tag0x9F33, TerminalCap);
                result = RBA_API.ProcessMessage(MESSAGE_ID.M33_07_EMV_TERMINAL_CAPABILITIES);
                //logger.Trace( "Result: " + result.ToString());
                //logger.Trace( "Exiting");
            } catch (Exception ex) {
                //logger.Trace( ex.ToString(), ErrorLevel.Critical);
                logger.Fatal(ex, ex.Message);
                throw new Exception(ex.ToString());
            }
            return Result;
        }
        #endregion Set Terminal Capabilities 33.07      

        #region Upload Files or Package to Device
        /// <summary>
        /// Upload a single file to the device
        /// </summary>
        /// <param name="fileNamePath"></param>
        public int UploadFile(string fileNamePath)
        {
            return UploadFile(fileNamePath, false);
        }

        /// <summary>
        /// Upload a Package (.TGZ, PGZ)
        /// </summary>
        /// <param name="packageNamePath"></param>
        public int UploadPackage(string packageNamePath)
        {
            return UploadFile(packageNamePath, true);
        }

        /// <summary>
        /// Upload to device
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="isPackage"></param>
        /// <returns></returns>
        private int UploadFile(string fileName, bool isPackage)
        {
            int Result = 1;
            try {
                // logger.Trace( "Entering");
                string tmpFileName = Path.GetFileName(fileName);
                ERROR_ID result = RBA_API.SetParam(PARAMETER_ID.P62_REQ_RECORD_TYPE, "0");
                result = RBA_API.SetParam(PARAMETER_ID.P62_REQ_ENCODING_FORMAT, "8");

                if (isPackage) {
                    result = RBA_API.SetParam(PARAMETER_ID.P62_REQ_UNPACK_FLAG, "0");
                    //    logger.Trace( "Package upload");
                } else {
                    result = RBA_API.SetParam(PARAMETER_ID.P62_REQ_UNPACK_FLAG, "1");
                    //    logger.Trace( "File upload");
                }

                result = RBA_API.SetParam(PARAMETER_ID.P62_REQ_FAST_DOWNLOAD, "1");
                result = RBA_API.SetParam(PARAMETER_ID.P62_REQ_OS_FILE_NAME, fileName);
                result = RBA_API.SetParam(PARAMETER_ID.P62_REQ_FILE_NAME, tmpFileName);

                result = RBA_API.ProcessMessage(MESSAGE_ID.FILE_WRITE);
                Result = result == ERROR_ID.RESULT_SUCCESS ? 0 : 1;
                if (result != ERROR_ID.RESULT_SUCCESS) {
                    //   logger.Trace( Result.ToString(), ErrorLevel.Warning);
                } else {
                    result = RBA_API.ProcessMessage(MESSAGE_ID.M97_REBOOT);
                    CallDisconnect();
                }
                //logger.Trace( "Process Msg 62: " + result);
                //logger.Trace( "Status: " + RBA_API.GetParam(PARAMETER_ID.P62_RES_STATUS));
                //logger.Trace( "File Length: " + RBA_API.GetParam(PARAMETER_ID.P62_RES_FILE_LENGTH));
                //logger.Trace( "Exiting");
            } catch (Exception ex) {
                //logger.Trace( ex.ToString(), ErrorLevel.Critical);
                logger.Fatal(ex, ex.Message);
                throw new Exception(ex.ToString());
            }
            return Result;
        }
        #endregion Upload Files to Device

        #region Handler ISC Device
        /// <summary>
        /// Handle touch event on the device.
        /// </summary>;
        /// <param name="msgId"></param>
        private void PinPadMessageHandler(MESSAGE_ID msgId)
        {
            logger.Trace("In PinPadMessageHandler()");
            ERROR_ID result;
            lock (oDeviceEvent) {
                switch (msgId) {
                    case MESSAGE_ID.M00_OFFLINE: {
                            #region M00 Offline
                            logger.Trace("Entering M00_Offline");
                            string offline = RBA_API.GetParam(PARAMETER_ID.P00_RES_REASON_CODE);
                            this.isDeviceActivated = false;
                            logger.Trace("Device Event, Offline");
                            logger.Trace("Exiting M00_Offline");
                            #endregion M00 Offline
                            break;
                        }
                    case MESSAGE_ID.M09_SET_ALLOWED_PAYMENT: {
                            #region M09_SetAllowPayment
                            logger.Trace("Entering M09_Set Allowed Payment");
                            string CardType = RBA_API.GetParam(PARAMETER_ID.P09_RES_CARD_TYPE);
                            string CardStatus = RBA_API.GetParam(PARAMETER_ID.P09_RES_CARD_STATUS);
                            logger.Trace("Device Event, Set Allowed Payment. CardType: " + CardType + " Card Status: " + CardStatus);
                            logger.Trace("Exiting M09_Set Allowed Payment");
                            #endregion M09_SetAllowPayment
                            break;
                        }
                    case MESSAGE_ID.M20_SIGNATURE: {
                            #region M20 Signature
                            logger.Trace("Signature Accepted.");

                            string SigBlock = string.Empty;
                            string SigData = string.Empty;
                            string KeyIDPress = RBA_API.GetParam(PARAMETER_ID.P20_RES_KEY);
                            string Press = string.IsNullOrWhiteSpace(KeyIDPress) ? "Enter" : KeyIDPress;
                            logger.Trace("Form Entry on Form: [" + GetCurrentForm + "], KeyPress: " + Press);

                            result = RBA_API.SetParam(PARAMETER_ID.P29_REQ_VARIABLE_ID, TagID.REQ_BEGIN_SIGN);
                            result = RBA_API.ProcessMessage(MESSAGE_ID.M29_GET_VARIABLE);
                            int SignatureBlocks = Convert.ToInt32(RBA_API.GetParam(PARAMETER_ID.P29_RES_VARIABLE_DATA));

                            for (int i = 0; i < SignatureBlocks; i++) {
                                result = RBA_API.SetParam(PARAMETER_ID.P29_REQ_VARIABLE_ID, ("70" + Convert.ToString(i)));
                                result = RBA_API.ProcessMessage(MESSAGE_ID.M29_GET_VARIABLE);
                                SigBlock = RBA_API.GetParam(PARAMETER_ID.P29_RES_VARIABLE_DATA);
                                SigData += SigBlock;
                            }
                            if (string.IsNullOrWhiteSpace(SigData)) {
                                if (ButtonEvent != null) {
                                    ButtonEvent(GetCurrentForm, Press, string.Empty);
                                }
                                logger.Trace("Signature is blank, no data received", ErrorLevel.Warning);
                                break;
                            }

                            byte[] sigByteData = Encoding.ASCII.GetBytes(SigData);
                            using (ISCSignature sig = new ISCSignature()) {
                                sig.ProcessSignature(sigByteData, SigData.Length, true);
                                isWaitingSign = false; //no longer waiting

                                switch (cForms.Signature.SignReturnFormat) {
                                    case SigFormat.InString: {
                                            string SigString = sig.Draw(cForms.Signature.SignReturnFormat);
                                            if (SignatureEvent != null) {
                                                SignatureEvent(SigFormat.InString, SigStatus.IsAccepted, SigString);
                                            }
                                            break;
                                        }
                                }
                            }

                            if (ButtonEvent != null) {
                                ButtonEvent(GetCurrentForm, Press, string.Empty);
                            }
                            #endregion M20 Signature
                            break;
                        }
                    case MESSAGE_ID.M21_NUMERIC_INPUT: {
                            #region User Input Event
                            logger.Trace("Numeric Input");
                            string Exit = RBA_API.GetParam(PARAMETER_ID.P21_RES_EXIT_TYPE);
                            if (Exit.Equals("0")) {
                                string Input = RBA_API.GetParam(PARAMETER_ID.P21_RES_INPUT_DATA).ToString();
                                if (UserInputEvent != null) {
                                    logger.Trace("Numeric Input Entered :" + Input);

                                    UserInputEvent(Input);
                                }
                            }
                            #endregion User Input Event       
                            break;
                        }
                    case MESSAGE_ID.M27_ALPHA_INPUT: {
                            #region Alpha Input

                            logger.Trace("Alpha numeric Input");

                            string Exit = RBA_API.GetParam(PARAMETER_ID.P27_RES_EXIT_TYPE);
                            if (Exit.Equals("0")) {
                                string input = RBA_API.GetParam(PARAMETER_ID.P27_RES_DATA_INPUT);
                                if (UserInputEvent != null) {
                                    logger.Trace("Alpha numeric Input Entered :" + input);
                                    UserInputEvent(input);

                                }
                            }
                            #endregion Alpha Input
                            break;
                        }
                    case MESSAGE_ID.M23_CARD_READ: {
                            #region M23 Card Read
                            logger.Trace("Entering E2EE M23");
                            isSwipe = true;
                            entryType = TagType.SWIPE;
                            string trk1 = RBA_API.GetParam(PARAMETER_ID.P23_RES_TRACK1);
                            trk1 = trk1.Length < 5 ? "" : trk1;
                            string trk2 = RBA_API.GetParam(PARAMETER_ID.P23_RES_TRACK2);
                            string trk3 = RBA_API.GetParam(PARAMETER_ID.P23_RES_TRACK3);
                            string tmpEMSI = trk1.Trim() + trk2.Trim() + ":" + trk3.Trim();
                            SecureData(Tags.EMSI, (tmpEMSI).ToByte(), entryType);
                            /* Device Name */
                            SecureData(Tags.DNAME, DeviceName.ToString().Split('_')[0].ToByte(), entryType);
                            SendTransaction(entryType);
                            #endregion M23 Card Read
                            break;
                        }
                    case MESSAGE_ID.M24_FORM_ENTRY: {
                            #region M24
                            logger.Trace("Entering Form_Entry M24");
                            string BtnState = RBA_API.GetParam(PARAMETER_ID.P24_RES_BUTTON_STATE);
                            string KeyIDPress = RBA_API.GetParam(PARAMETER_ID.P24_RES_KEYID);
                            string ExitForm = RBA_API.GetParam(PARAMETER_ID.P24_RES_EXIT_TYPE);

                            string Press = string.IsNullOrWhiteSpace(KeyIDPress) ? "Enter" : KeyIDPress;
                            logger.Trace("Form Entry on Form: [" + GetCurrentForm + "], KeyPress: " + Press);

                            #region Logic for Line Display
                            var value = cForms.CustomForms.FormList.Find(form => form.Item1.ToUpper() == GetCurrentForm.ToUpper());
                            if (value != null) {
                                string lineD = Array.Find(value.Item2, element => element.ToUpper().Contains("LINEDISPLAY"));
                                if (!string.IsNullOrWhiteSpace(lineD) && ExitForm == "0") {
                                    ClearDisplayMsg(true);
                                }
                            }

                            if (ButtonEvent != null) {
                                ButtonEvent(GetCurrentForm, Press, BtnState);
                            }
                            #endregion Logic for Line Display

                            logger.Trace("Exiting Form_Entry M24");
                            #endregion M24
                            break;
                        }
                    case MESSAGE_ID.M33_01_EMV_STATUS: {
                            break;
                        }
                    case MESSAGE_ID.M33_02_EMV_TRANSACTION_PREPARATION_RESPONSE: {
                            #region M33_02
                            logger.Trace("Entering M33_02");
                            Emv emv = new Emv();
                            entryType = TagType.EMV;
                            string Status = RBA_API.GetParam(PARAMETER_ID.P33_02_RES_STATUS);
                            if (Status == "E") {
                                logger.Trace("Error in M33_02_EMV Transaction Response", ErrorLevel.Warning);
                            } else if (Status == "00") {
                                logger.Trace("M33_02_EMV Transaction Response Status: " + Status);
                            }

                            while (true) {
                                int TagLength = RBA_API.GetTagParamLen(msgId);
                                if (TagLength <= 0) {
                                    break;
                                } else {
                                    byte[] TagValue = new byte[1];
                                    int Tag = RBA_API.GetTagParam(msgId, out TagValue);
                                    if (Tag.ToString("X") == "57") {
                                        emv.EmvRequestData.TrkData = TagValue.ByteToString();
                                    } else if (Tag.ToString("X") == "9F1E") {
                                        string TSN = TagValue.ByteToString().HexToAscii();
                                        SecureData(Tag.ToString("X"), TSN.ToByte(), entryType);
                                    } else if (Tag.ToString("X") == "5F20") {
                                        emv.EmvRequestData.Name = TagValue.ByteToString();
                                    } else if (Tag.ToString("X") == "5F24") {
                                        emv.EmvRequestData.Expire = TagValue.ByteToString();
                                    } else if (Tag.ToString("X") == "FF1F") {
                                        emv.EmvRequestData.eTrk2Data = TagValue.ByteToString();
                                    } else if (Tag.ToString("X") == "FF21") {
                                        emv.EmvRequestData.eData = TagValue.ByteToString();
                                    } else {
                                        SecureData(Tag.ToString("X"), TagValue.ByteToString().ToByte(), entryType);
                                    }
                                }
                            }
                            if (emv != null) {
                                string EmvTrackData = emv.BuildEmvData(emv.EmvRequestData);
                                if (!string.IsNullOrEmpty(EmvTrackData)) {
                                    SecureData(Tags.EMSI, EmvTrackData.ToByte(), entryType);
                                    /* Device Name */
                                    SecureData(Tags.DNAME, DeviceName.ToString().Split('_')[0].ToByte(), entryType);
                                }
                            }
                            if (SecureDataList != null && SecureDataList.Count > 1) {
                                ActivateEMV();
                            }
                            logger.Trace("Exiting M33_02");
                            #endregion M33_02
                            break;
                        }
                    case MESSAGE_ID.M33_03_EMV_AUTHORIZATION_REQUEST: {
                            #region M33_03 EMV Request
                            bool isAuth = false;
                            string Status = RBA_API.GetParam(PARAMETER_ID.P33_03_REQ_STATUS);
                            if (Status == "E") {
                                logger.Trace("Error in M33_03_EMV Request", ErrorLevel.Warning);
                            } else if (Status == "00") {
                                logger.Trace("M33_03_EMV Request Status: " + Status);
                            }

                            while (true) {
                                int TagLength = RBA_API.GetTagParamLen(msgId);
                                if (TagLength <= 0) {
                                    break;
                                } else {
                                    byte[] TagValue = new byte[1];
                                    int Tag = RBA_API.GetTagParam(msgId, out TagValue);
                                    if (Tag.ToString("X") == "9F1E") {
                                        string TSN = TagValue.ByteToString().HexToAscii();
                                        SecureData(Tag.ToString("X"), TSN.ToByte(), entryType);
                                    } else {
                                        SecureData(Tag.ToString("X"), TagValue.ByteToString().ToByte(), TagType.EMV);
                                    }
                                    isAuth = true;
                                }
                            }
                            if (isAuth) {
                                EMVAuthorizeResponse();
                            }
                            #endregion M33_03 EMV Request
                            break;
                        }
                    case MESSAGE_ID.M33_05_EMV_AUTHORIZATION_CONFIRMATION: {
                            #region M33_05
                            logger.Trace("Entering M33_05");
                            string EncryptData = string.Empty;
                            if (RBA_API.GetParam(PARAMETER_ID.P33_05_RES_STATUS) == "E") {
                                logger.Trace("Error in M33_05_EMV Authorization Confirmation", ErrorLevel.Warning);
                            }
                            while (true) {
                                int TagLength = RBA_API.GetTagParamLen(msgId);
                                if (TagLength <= 0) {
                                    break;
                                } else {
                                    byte[] TagValue = new byte[1];
                                    int Tag = RBA_API.GetTagParam(msgId, out TagValue);
                                    SecureData(Tag.ToString("X"), TagValue.ByteToString().ToByte(), TagType.EMV);
                                    /* Device Name */
                                    SecureData(Tags.DNAME, DeviceName.ToString().Split('_')[0].ToByte(), entryType);
                                }
                            }
                            logger.Trace("Exiting M33_05");
                            #endregion M33_05
                            break;
                        }
                    case MESSAGE_ID.M33_07_EMV_TERMINAL_CAPABILITIES: {
                            #region M33_07
                            logger.Trace("Entering M33_07");
                            if (RBA_API.GetParam(PARAMETER_ID.P33_07_REQ_STATUS) == "E") {
                                logger.Trace("Error in M33_07_EMV Terminal Capabilities", ErrorLevel.Warning);
                            }
                            entryType = TagType.EMV;
                            while (true) {
                                int TagLength = RBA_API.GetTagParamLen(msgId);
                                if (TagLength <= 0) {
                                    break;
                                } else {
                                    byte[] TagValue = new byte[1];
                                    byte[] AID = new byte[1];

                                    int Tag = RBA_API.GetTagParam(msgId, out TagValue);
                                    SecureData(Tag.ToString("X"), TagValue, entryType);

                                    if (Tag == 132) {
                                        emvAID = TagValue.ByteToString().ToUpper();
                                    }
                                }
                            }

                            if (!string.IsNullOrWhiteSpace(emvAID) && this.emvAID.StartsWith("A")) {
                                SetTerminalCapabilities();
                            }
                            logger.Trace("Exiting M33_07");
                            #endregion M33_07
                            break;
                        }
                    case MESSAGE_ID.M50_AUTHORIZATION: {
                            #region M50 Authorization (Swipe)
                            logger.Trace("Entering M50_Authorization (Swipe)");
                            #region Variables
                            string tempEMSI = string.Empty;
                            string sDeviceName = string.Empty;
                            string PINB = string.Empty;
                            string PKSI = string.Empty;
                            string TSN = string.Empty;
                            #endregion Variables

                            string payType = UserSwitchPayType();
                            /* Device Source */
                            string DataSource = RBA_API.GetParam(PARAMETER_ID.P50_REQ_ACC_DATA_SOURCE); //D(Swipe), d(contactless)                           

                            switch (DataSource) {
                                case TagID.CONTACTLESS:
                                    entryType = TagType.NFC;
                                    break;
                                case TagID.MSR:
                                    entryType = TagType.SWIPE;
                                    break;
                                case TagID.MANUAL:
                                    entryType = TagType.MANUAL;
                                    break;
                                default:
                                    entryType = TagType.EMV;
                                    break;
                            }

                            /* Encrypted Magnetic swipe, Track 1 + Track 2 + Encryption */
                            tempEMSI = GetVariable(TagID.REQ_TRACK1) + GetVariable(TagID.REQ_TRACK2) + ":" + RBA_API.GetParam(PARAMETER_ID.P50_REQ_MAG_SWIPE_INFO);
                            //string[] tmpNFC = tempEMSI.Split('^');
                            //tempEMSI = tmpNFC[0] + "^" + tmpNFC[1] + "_" + TagID.SET_NFC_NAME + "^" + tmpNFC[2];
                            if (payType == "A") {
                                entryType = TagType.SWIPE;
                                /*Get the CashBack from the Device */
                                string CBAMT = ((float)Convert.ToInt32(GetVariable(TagID.REQ_CASHBACK_AMT)) / 100).ToString();
                                SecureData(Tags.CBAMT, CBAMT.ToByte(), entryType);

                                string DeviceID = RBA_API.GetParam(PARAMETER_ID.P50_REQ_PIN_DEVICE_ID);
                                string Counter = RBA_API.GetParam(PARAMETER_ID.P50_REQ_PIN_ENCRYPTION_COUNTER);
                                SecureData(Tags.TSN, (DeviceID + Counter).ToByte(), entryType);

                                /* Encrypted PinBlock {7} */
                                SecureData(Tags.PINB, RBA_API.GetParam(PARAMETER_ID.P50_REQ_PIN_ENCRYPTED_BLOCK).ToByte(), entryType);

                                /* PIN Key Set {23, 27}*/
                                SecureData(Tags.PKSI, (TagID.PKSI_CONSTANT + RBA_API.GetParam(PARAMETER_ID.P50_REQ_PIN_KEY_SET_IDENTIFIER)).ToByte(), entryType);
                            }

                            /* Name */
                            string Name = GetVariable(TagID.REQ_ACCOUNT_NAME);

                            SecureData(Tags.EMSI, tempEMSI.ToByte(), entryType);
                            /* Device Name */
                            SecureData(Tags.DNAME, DeviceName.ToString().Split('_')[0].ToByte(), entryType);

                            /* End Transaction */
                            OffLineEvent += ISCDevices_OffLineEvent;
                            OffLineEvent.Invoke();

                            if (!isSwipe) {
                                this.isSwipe = true;
                            }
                            if (PayTags.Amount.TotalAmount > 0) {
                                SendTransaction(entryType);
                            }
                            logger.Trace("Exiting M50_Authorization (Swipe)");
                            #endregion M50 Authorization (Swipe)
                            break;
                        }
                }
            }
        }
        #endregion Handler

        #region Event Offline
        /// <summary>
        /// Event to offline
        /// </summary>
        void ISCDevices_OffLineEvent()
        {
            SetDeviceOffLine();
        }
        #endregion

        #region Get Variables Msg 29
        /// <summary>
        /// Get the hidden variables from the device
        /// </summary>
        /// <param name="var"></param>
        /// <returns></returns>
        private string GetVariable(string var)
        {
            string data = string.Empty;
            try {
                logger.Trace("Entering, Var ID: " + var);
                ERROR_ID Result;
                Result = RBA_API.SetParam(PARAMETER_ID.P29_REQ_VARIABLE_ID, var);
                Result = RBA_API.ProcessMessage(MESSAGE_ID.M29_GET_VARIABLE);
                data = RBA_API.GetParam(PARAMETER_ID.P29_RES_VARIABLE_DATA);
                logger.Trace("Exiting, Result: " + Result.ToString());
            } catch (Exception ex) {
                logger.Trace(ex.ToString(), ErrorLevel.Critical);
                throw new Exception(ex.ToString());
            }
            return data;
        }
        #endregion Get Variables Msg 29

        #region Switch PaymentType
        /// <summary>
        /// User Click on device to switch pay type
        /// </summary>
        private string UserSwitchPayType()
        {
            string UserPayType = string.Empty;
            try {
                logger.Trace("Entering UserSwitchPayType()");
                UserPayType = GetVariable(TagID.REQ_PAYTYPE);
                if (UserPayType != ((char)PayTags.PaymentType).ToString()) {
                    logger.Trace("User Selected a different PayType: " + UserPayType);
                    switch (UserPayType) {
                        case "B": {
                                PayTags.PaymentType = PaymentTags.payType.Credit;
                                break;
                            }
                        case "A": {
                                PayTags.PaymentType = PaymentTags.payType.Debit;
                                break;
                            }
                    }
                }
                logger.Trace("Exiting UserSwitchPayType()");
            } catch (Exception ex) {
                logger.Fatal("UserSwitchPayType" + ex.ToString(), ErrorLevel.Critical);
                throw new Exception(ex.ToString());
            }
            return UserPayType;
        }
        #endregion Switch PaymentType

        #region Secure Data to pass to gateway
        /// <summary>
        /// Securing the data
        /// </summary>
        /// <param name="code"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        private int SecureData(string code, byte[] data, TagType Type)
        {
            int result = 1;

            try {
                if (dataEnc == null) {
                    dataEnc = new DataEncryption();
                }
                if (SecureDataList == null) {
                    SecureDataList = new List<Tuple<string, byte[], string>>();
                }
                SecureDataList.Add(new Tuple<string, byte[], string>(code, dataEnc.Encryption(data), Type.ToString()));
            } catch (Exception ex) {
                logger.Fatal(ex, ex.Message);
                throw new Exception(ex.ToString());
            } finally {
                data = null;
            }
            return result;
        }
        #endregion Secure Data to pass to gateway
    }
}
