using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using WPD = MMS.Device.WPDevice.WPDevice;
using NLog;
using System.Data;

namespace MMS.Device
{
    /// <summary>
    /// Author: Manoj Kumar
    /// This class communicate with the Ingenico iSC480
    /// This also use the EMV Payment
    /// </summary>

    class RBA_ISC_Device
    {
        ILogger logger = LogManager.GetCurrentClassLogger();


        #region Variable
        public bool isDeviceInit { get; set; }
        private readonly object DeviceLock = new object();
        internal Queue<CommandStation> CommandQueue = new Queue<CommandStation>();
        Hashtable iData = null;
        private string PreviousScreen { get; set; }
        private bool isDeviceActivate;
        WPD isc480 = null;

        public object exceptionObj = new object();
        public bool isException = false;
    #endregion Variable


    #region Process Payment
    internal void ProcessPayment(WPDevice.WPData.Payment Pay, WPDevice.WPData.WPAccountInfo WPInfo)
        {
            isc480.ProcessPayment(Pay, WPInfo);
        }
        #endregion Process Payment


        #region Cancel Transaction WorldPAY
        internal void CancelWPTransaction()
        {
            isc480.CancelTransaction();
        }
        #endregion Cancel Transaction WorldPAY

        #region Disconnect device
        /// <summary>
        /// Disconnect from device
        /// </summary>
        /// <returns></returns>
        internal bool Disconnect()
        {
            return isc480.Disconnect();
        }
        #endregion Disconnect device


        #region WP Response Event
        public bool? IsSignValid
        {
            get { return isc480.ValidSign; }
            set { isc480.ValidSign = value; }
        }

        public byte[] GetSignature
        {
            get { return isc480.GetSignature; }
            set { isc480.GetSignature = value; }
        }

        public string GetSignatureString
        {
            get { return isc480.GetSignatureString; }
            set { isc480.GetSignatureString = value; }
        }

        public bool? IsWPResponse
        {
            get { return isc480.IsWPResponse; }
            set { isc480.IsWPResponse = value; }
        }

        public string ButtonClickID
        {
            get { return isc480.ButtonClickID; }
            set { isc480.ButtonClickID = value; }

        }

        public bool CancelCapture
        {
            get { return isc480.CancelCapture; }
            set { isc480.CancelCapture = value; }
        }

        public PatientConsent PatConsent
        {
            get { return isc480.PatConsent; }
            set { isc480.PatConsent = value; }
        }
        public Dictionary<string, string> ReturnResponse
        {
            get { return isc480.ReturnResult; }
            set { isc480.ReturnResult = value; }
        }
        public DataSet dsAutoRefillData // PRIMEPOS-2868 Added for default or other consent
        {
            get { return isc480.dsAutoRefillData; }
            set { isc480.dsAutoRefillData = value; }
        }
        #endregion WP Response Event

        //internal void 

        internal void SetUserCancelled()
        {
            WPD.isc_UserReset();
        }


        #region Set Device Property
        internal void ISC_SetDeviceProperty(object obj)
        {
            CommandStation command = null;
            try
            {
                logger.Debug("\t\t==>About to set Data on Device");
                /* This is to make sure no other data is being pass from any other method while in here */
                lock (DeviceLock)
                {
                    while (CommandQueue.Count > 0)
                    {
                        if (!Constant.IsInMethod)
                        {
                            Constant.IsInMethod = true; //set condition so it wait until everything finish before going in the loop again
                            Constant.IsStillWrite = true; //make sure this is set so POS can know that device is still writing to the device
                            string iScreen = string.Empty;

                            bool isCommand = true;
                            logger.Debug("\t\tAbout to dequeue command from the Queue to the pad, Count: " + CommandQueue.Count);
                            try
                            {
                                command = CommandQueue.Dequeue(); //Deueue the Queue data                             
                                isCommand = true;
                            }
                            catch (Exception ex)
                            {
                                logger.Debug("\t\tException error in dequeue command: " + ex.ToString());
                                isCommand = false;
                            }

                            if (!isCommand)
                            {
                                Constant.IsInMethod = false;
                                Constant.IsStillWrite = false;
                                lock (exceptionObj) { isException = true; }
                                return;
                            }

                            if (iData == null)
                            {
                                iData = new Hashtable();
                            }
                            else
                            {
                                iData.Clear();
                            }

                            /* Pass the screen to display */
                            iScreen = command.DeviceScreen;
                            /* Pass the data to be on the form */
                            iData = command.DeviceData as Hashtable;

                            logger.Debug("\t\t About to Set Properties on the Screen. Screen to Display: " + iScreen);

                            /* Initialize the device before Connection*/
                            if (iData.ContainsKey(iSCFormName.INIT))
                            {
                                if (Constant.DeviceName == Constant.Devices.WPIngenico_ISC480.ToString())
                                {
                                    if (isc480 == null)
                                    {
                                        isc480 = new WPD();
                                    }
                                    if (Convert.ToInt32(iData[iSCFormName.INIT]) > 0)
                                    {
                                        isc480.Connect(Convert.ToInt32(iData[iSCFormName.INIT]), "S");
                                    }
                                    else
                                    {
                                        isc480.Connect(Convert.ToInt32(iData[iSCFormName.INIT]), "U");
                                    }
                                }
                            }

                            #region Ingenico Lane Close Issue
                            //PRIMEPOS-2534 - Lane CLose Issue Added suraj
                            if (iData.ContainsKey(iSCFormName.REINIT))
                            {
                                if (Constant.DeviceName == Constant.Devices.WPIngenico_ISC480.ToString())
                                {
                                    if (isc480 == null)
                                    {
                                        isc480 = new WPD();
                                    }
                                    else
                                    {
                                        isc480.Disconnect();
                                        isc480.ShutDown();

                                        isc480 = new WPD();
                                    }
                                    if (Convert.ToInt32(iData[iSCFormName.REINIT]) > 0)
                                    {
                                        isc480.Connect(Convert.ToInt32(iData[iSCFormName.REINIT]), "S");
                                    }
                                    else
                                    {
                                        isc480.Connect(Convert.ToInt32(iData[iSCFormName.REINIT]), "U");
                                    }
                                }
                            }
                            #endregion  

                            switch (Constant.DeviceName)
                            {
                                case "WPIngenico_ISC480":
                                    {
                                        //if (iScreen.ToUpper() == "HEALTHIX")
                                        //{
                                        //    isc480.PerformConsentCaptureforHealthix(iData);
                                        //}
                                        //else
                                        //{
                                        //    if (iScreen.ToUpper() == "PADMSGSCREEN")
                                        //    {
                                        //        iScreen = "PADMSG";
                                        //    }

                                        //    isc480.ShowForm(iData, iScreen);
                                        //}

                                        //break;
                                        if (iScreen.ToUpper() == "HEALTHIX")
                                        {
                                            isc480.PerformConsentCaptureforHealthix(iData);
                                        }
                                        else if (iScreen.ToUpper() == "PADMSGSCREEN")
                                        {
                                            iScreen = "PADMSG";
                                            isc480.ShowForm(iData, iScreen); //PRIMEPOS-3297
                                        }
                                        else if (iScreen == "PADREFCONST") // PRIMEPOS-2868 - Added for Default Consent
                                        {
                                            isc480.GetDefaultConsentSelection(iData);
                                        }
                                        else
                                        {
                                            isc480.ShowForm(iData, iScreen);
                                        }
                                        break;
                                    }
                            }
                            Constant.IsInMethod = false;
                            Constant.IsStillWrite = false;
                        }
                    }
                }
                Constant.IsInMethod = false; //added temporary Suraj
                Constant.IsStillWrite = false;

            }
            catch (Exception EX)
            {
                logger.Debug("\t\tException error in ISC_SetDeviceProperty(): " + EX.ToString());
                Constant.IsInMethod = false;
                Constant.IsStillWrite = false;
                lock (exceptionObj) { isException = true; }
                if (command != null) { 
                    CommandQueue.Enqueue(command);
                }
                //throw new Exception(EX.ToString());
            }
        }
        #endregion Set Device Property
    }
}
