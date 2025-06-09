//Copyright © Micro Merchant Sys Inc.
//Author : Atul
//Functionality Desciption :The purpose of this class is to initialize Pin Pad device
// to read the pin number & key serial number
//Known Bugs : If Pin Pad device is not attached , it will not work
//Start Date : 30 August 2008
// Importing Name Spaces
using POS_Core.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
//using POS_Core.DataAccess;

namespace POS_Core_UI
{
    public partial class frmPinPad : Form
    {
        //variable declaration
        private string txnAmount = string.Empty;
        private static frmPinPad oDefObj;
        public int iPinPadInitStatus;
        private bool pinPadInitialized = false; //Added by SRT to make it conditional.
        private const int FAILURE = 1;
        private const int SUCCESS = 0;        
        // A property to get the default instance of frmPinPad class
        public static frmPinPad DefaultInstance
        {
            get
            {
                if (oDefObj == null)
                {
                    oDefObj = new frmPinPad();
                }
                return oDefObj;
            }
        }
        public static void CloseDefaultInstace()
        {
            if (oDefObj != null)
            {
                oDefObj.DisposeObjects();               
            }
        }

        private void DisposeObjects()
        {
            this.Dispose(true); 
        }
        //A private constructor which initializes the Pin Pad & hide the form
        private frmPinPad()
        {
            InitializeComponent();
        //    InitializePinPad();
            this.Hide();
        }
        // A public property to get & set the transaction amount which is used to
        // assign to ocx control
        public string TxnAmount
        {
            get { return txnAmount; }
            set { this.txnAmount = value; }
        }
        //Added by SRT (Dharmendra)
        // A public property to check if Pinpad is initialized or not
        public bool PinPadInitialized
        {
            get { return pinPadInitialized; }
            set { this.pinPadInitialized = value; }
        }
        /// <summary>
        /// Author : Atul
        /// Functionality Description : This method reads the ocx control's default setting
        /// from configuration file and assign the same to ocx control.
        /// It also initializes the pinpad device & returns the iPinPadInitStatus
        /// Known Bugs : configuration file must contains these default settings
        /// Start Date : 30 August 2008
        /// </summary>
        /// <returns>"iPinPadInitStatus"</returns>
        public int InitializePinPad()
        {
            bool initResponse = false;
            
            //Modified by Dharmendra (SRT) on Nov-13-08. Removed Pinpad configuration reading from app.config
            string ppdBaudRate = Configuration.CPOSSet.PinPadBaudRate;
            string ppdParity =Configuration.CPOSSet.PinPadPairity.Trim();
            string ppdPort = Configuration.CPOSSet.PinPadPortNo;
            string ppdDataBits = Configuration.CPOSSet.PinPadDataBits;
            string pinDeviceName = Configuration.CPOSSet.PinPadModel;
            
            //switch (pinDeviceName.ToUpper())
            //{
            //    case "VERIFONE 1001/1000":
            //        PinPadDevice.Device = Device.DeviceType.ppdVerifone_101;
            //        break;
            //    case "IVI EN-CRYPT 2100":
            //        PinPadDevice.Device = Device.DeviceType.ppdIVICheckMate_2100;
            //       break;
            //    case "VERIFONE 2000":
            //       PinPadDevice.Device = Device.DeviceType.ppdVerifone_2000;
            //        break;
            //    case "VERIFONE EVEREST":
            //       PinPadDevice.Device = Device.DeviceType.ppdVerifone_Everest;
            //       break;
            //}
            //Modified by Dharmendra (SRT) on Nov-13-08. Removed Pinpad configuration reading from app.config
            string encryptKey = Configuration.CPOSSet.PinPadKeyEncryptionType;
            //Modified Till Here
            //switch (encryptKey)
            //{
            //    case "MS":
            //        PinPadDevice.EncryptMethod = (Device.PinPadManagement)0;
            //        break;
            //    case "DUKPT":
            //        PinPadDevice.EncryptMethod = (Device.PinPadManagement)1;
            //        break;
            //}
            //Modified by Dharmendra (SRT) on Nov-13-08. Removed Pinpad configuration reading from app.config
            //string ppdDispMsg = System.Configuration.ConfigurationManager.AppSettings["PINPAD_DISPMSG"];
            string ppdDispMsg = Configuration.CPOSSet.PinPadDispMesg;
            //Modified till here
            PinPadDevice.Baud = ppdBaudRate;
            PinPadDevice.Parity = ppdParity;
            PinPadDevice.Port = ppdPort;
            PinPadDevice.DataBits = ppdDataBits;
            PinPadDevice.DisplayString = ppdDispMsg;
            initResponse = PinPadDevice.Initialize();
            if (initResponse == true)
            {
              //  POS_Core_UI.Resources.Message.Display("Pin Pad Initialized");
                iPinPadInitStatus = SUCCESS;
                pinPadInitialized = true;
            }
            else
            {
                //POS_Core_UI.Resources.Message.Display("Pin Pad Initialization Failed");
                iPinPadInitStatus = FAILURE;
            }
            return iPinPadInitStatus;
        }
        /// <summary>
        /// Author : Atul
        /// Functionality Description : This method passs the transaction amount, card number to 
        /// pin pad device, reads pin number,key serial number from pin pad.
        /// This pin number & keserial number is assigned to the properties of
        /// SigPadUtil.DefaultInstance.SigPadCardInfo.PinNumber
        /// SigPadUtil.DefaultInstance.SigPadCardInfo.KeySerialNumber
        /// Known Bugs : None
        /// Start Date : 30 August 2008
        /// </summary>
        public void GetPinPadData(string CardNo, out string PinNumber, out string KeySerialNumber)
        {
            string pinNumber = string.Empty;
            string keySerialNumber = string.Empty;
           // PinPadDevice.EnablePin();
            PinPadDevice.Clear();
            PinPadDevice.Amount = txnAmount;
            PinPadDevice.Card = CardNo;
            pinNumber = PinPadDevice.GetPin();
            keySerialNumber = PinPadDevice.GetKeySerialNumber();
            PinNumber = pinNumber;
            KeySerialNumber = keySerialNumber;
        }
        /// <summary>
        /// Author : Dharmendra
        /// Functionality Description : This method clears the data from pin pad device
        /// and initializes amount & card number
        /// </summary>
        public void ClearPinData()
        {
            PinPadDevice.Clear();
            PinPadDevice.Amount = "";
            PinPadDevice.Card = "";            
        }
    }
}