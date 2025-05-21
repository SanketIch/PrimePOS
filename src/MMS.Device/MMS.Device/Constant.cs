using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Data;

namespace MMS.Device
{
    /// <summary>
    /// Constant Properties and Variables
    /// </summary>
    /// <Author>Author: Manoj Kumar</Author>
    /// 
    class Constant
    {  
        #region Properties

        public enum Devices
        {
            Mx870WithoutPinPad,
            Mx870WithPinPad,
            VerifoneMX925WithPinPad,
            Ingenico_ISC480,
            WPIngenico_ISC480
        }
       
        public static string TxnIdStored
        {
            get;
            set;
        }
        /// <summary>
        /// Get or Set when device MSR is in Event
        /// </summary>
        public static bool InMSR
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or Set if the iSC is connected or not
        /// </summary>
        public static bool iSCConnect { get; set; }
        /// <summary>
        /// Get or Set the Data from MSR
        /// </summary>
        public static ArrayList CCDataSwipe
        {
            get;
            set;
        }

        public static string DeviceName{get;set;}

        private static string _DeviceScreen;
        /// <summary>
        /// Get or Set Device Screen
        /// </summary>
        public static string DeviceScreen
        {
            get{return _DeviceScreen;}
            set{ _DeviceScreen = value;}
        }

        public static string ISC_Version { get; set; }
        private static bool _isDeviceOpen;
        /// <summary>
        /// Get or Set if Device is OPen or CLose
        /// </summary>
        public static bool IsDeviceOpen
        {
            get{return _isDeviceOpen;}
            set{_isDeviceOpen = value;}
        }
        private static string _isLoadedScreen;
        /// <summary>
        /// Get or Set isLoadedScreen
        /// </summary>
        public static string CurrentLoadedScreen
        {
            get { return _isLoadedScreen; }
            set { _isLoadedScreen = value; }
        }
        /// <summary>
        /// Get or Set the CC info to pass to the PINPAD
        /// </summary>
        public static string ccinfo
        {
            get;
            set;
        }
        private static string _itemSum;
        /// <summary>
        /// Get or Set ItemSum
        /// </summary>
        public static string ItemSum
        {
            get { return _itemSum; }
            set { _itemSum = value; }
        }

        private static bool _PadAck;
        /// <summary>
        /// Get or Set PadAck
        /// </summary>
        public static bool PadAck
        {
            get { return _PadAck; }
            set { _PadAck = value; }
        }

        private static bool _isSigCap;
        /// <summary>
        /// Get or Set IsSigCap, this is a onetime setting do not set to false
        /// </summary>
        public static bool IsSigCap
        {
            get { return _isSigCap; }
            set { _isSigCap = value; }
        }

        private static bool _isInForm;
        /// <summary>
        /// Get or Set IsInForm, use to track if the device is doing any work
        /// </summary>
        public static bool IsInForm
        {
            get { return _isInForm; }
            set { _isInForm = value; }
        }

        /// <summary>
        /// Get or Set IsInMethod, use to track which method the POS call
        /// </summary>
        public static bool IsInMethod
        {
            get;
            set;
        }

        /// <summary>
        /// Get or Set if the device is still writing
        /// </summary>
        public static bool IsStillWrite
        {
            get;
            set;
        }

        /// <summary>
        /// Get or Set the Patient Counselling
        /// </summary>
        public static string Counsel
        {
            get;
            set;
        }

        /// <summary>
        /// Store all item that is pass to the PAD
        /// </summary>
        public static ArrayList dataStore = new ArrayList();

        /// <summary>
        /// Get or Set when the Detail button is click
        /// </summary>
        public static bool DetailButtonClick
        {
            get;
            set;
        }

        /// <summary>
        /// Get or Set when PadEvent is raise
        /// </summary>
        public static bool IsPadEvent
        {
            get;
            set;
        }

        public static bool IsMsrEvent
        {
            get;
            set;
        }

        public static bool IsPinEvent
        {
            get;
            set;
        }
        /// <summary>
        /// Get or Set if the MSR is successful
        /// </summary>
        public static bool IsMsrNotOK
        {
            get;
            set;
        }
        public static bool ItemCheck { get; set; }
        public static bool isErrorEventActivated { get; set; }
        public static bool EventActivated { get; set; }

        public delegate void ResetEvent(bool v);
        public static event ResetEvent ErrorEvent = delegate { };

        public static void Reset()
        {
            if(ErrorEvent != null)
            {
                ErrorEvent(true);
            }
        }

        public static string CurrentDequeueScreen { get; set; }
        public static Hashtable CurrentDequeueData { get; set; }
        /// <summary>
        /// Display the Return code from Verifone MX
        /// </summary>
        public static bool DisplayErrorCode { get; set; }
        public static int WaitTime { get; set; }
        public static bool ShowPaymentScreen { get; set; }
        #endregion

        #region Variables
        public const string SigType = "M";

        public const string Sign = "SIGNED";
        public const string NoppCancel = "NOPPCANCELED";
        public const string NoppSkip = "NOPPSKIP";
        public const string RxDetail = "RXDETAIL";
        public const string RxCounselY = "COUNSELYES";
        public const string RxCounselN = "COUNSELNO";
        public const string PatientCounselYes = "Y";
        public const string PatientCounselNo = "N";
        /// <summary>
        /// Track if the Pin Entry is release
        /// </summary>
        public static bool isPinRelease { get; set; }
        public static bool isPinEnable { get; set; }
        public static bool isMsrEnbale {get;set;}
        public static bool isMsrRelease { get; set; }

        public const string CloseDevice = "CLOSEDEVICE";
        public const string WELCOME = "PADWELCOME";
        public const string StartScreen = "PADSTART";
        public const string PinEntry = "FA_PINE";
        public const string PadSwipeCard = "PADSWIPECARD";
        public const string maxSwipe = "6003";
        public const int itemNameLen = 14;
        public const string deviceName = "Verifone Mx";
        public const int True = 1;
        public const int False = 0;
        public const int bTrue = 0;
        public const int BC_NONE = 0;
        //Property Name
        public const int FORM_PROP_STR_CAPTION = 1;
        public const int FORM_PROP_BOOL_SELECTED = 7;
        public const int FORM_PROP_BOOL_VISIBLE = 9;

        //Data Type
        public const int FORM_PROP_TYPE_BOOL = 1;
        public const int FORM_PROP_TYPE_int = 2;
        public const int FORM_PROP_TYPE_SHORT = 3;
        public const int FORM_PROP_TYPE_STRING = 4;

        //Form PADSIGN
        public const int PADSIGN_LABEL_AMOUNT_CHARGED = 3;

        //Form PADNOPP
        public const int PADNOPP_TEXTBOX = 1;
        public const int PADNOPP_LABEL_PATIENT_NAME_TEXT = 4;
        public const int PADNOPP_LABEL_ADDRESS_TEXT = 5;

        //Form PADRXLIST
        public const int PADRXLIST_LABEL_PATIENT_NAME_TEXT = 2;
        public const int PADRXLIST_RADIOBUTTON_YES = 4;
        public const int PADRXLIST_RADIOBUTTON_NO = 5;
        public const int PADRXLIST_LABEL_RX_COUNT_TEXT = 7;
        public const int PADRXLIST_LISTBOX = 8;
        public const int PADRXLIST_BUTTON_DETAIL = 9;
        public const int PADRXLIST_PAT_COUNSELING_TEXT = 24;

        //Form PADITEMLIST
        public const int PADITEMLIST_LISTBOX = 2;
        public const int PADITEMLIST_LABEL_LINEITEM = 9;
        public const int PADITEMLIST_LABEL_SUBTOTAL = 13;
        public const int PADITEMLIST_LABEL_DISCOUNT = 15;
        public const int PADITEMLIST_LABEL_TAX = 16;
        public const int PADITEMLIST_LABEL_TOTAL_AMOUNT = 17;

        //Form PADSWIPECARD
        public const int PADSWIPECARD_LABEL_SWIPEMSG = 3;
        public const int PADSWIPECARD_LABEL_AMOUNT = 4;

        //Forn PADMSGSCREEN
        public const int PADMSGSCREEN_LABEL_MESSAGE = 1;

        //Form PADOTC
        public const int PADOTC_TEXTBOX = 6;
        public const int PADOTC_LISTBOX = 7;

        //Form PADSHOWCASH
        public const int PADSHOWCASH_LABEL_SUB_TOTAL_TEXT = 7;
        public const int PADSHOWCASH_LABEL_TAX_TEXT = 8;
        public const int PADSHOWCASH_LABEL_TOTAL_DUE_TEXT = 9;
        public const int PADSHOWCASH_LABEL_AMOUNT_TENDERED_TEXT = 10;
        public const int PADSHOWCASH_LABEL_CHANGE_DUE_TEXT = 11;

        //Device ResultCode 
        public const int OPOS_SUCCESS = 0;
        public const int OPOS_E_CLOSED = 101;
        public const int OPOS_E_CLAIMED = 102;
        public const int OPOS_E_NOTCLAIMED = 103;
        public const int OPOS_E_NOSERVICE = 104;
        public const int OPOS_E_DISABLED = 105;
        public const int OPOS_E_ILLEGAL = 106;
        public const int OPOS_E_NOHARDWARE = 107;
        public const int OPOS_E_OFFLINE = 108;
        public const int OPOS_E_NOEXIST = 109;
        public const int OPOS_E_EXISTS = 110;
        public const int OPOS_E_FAILURE = 111;
        public const int OPOS_E_TIMEOUT = 112;
        public const int OPOS_E_BUSY = 113;

        //Device State
        public const int OPOS_S_CLOSED = 1;
        public const int OPOS_S_IDLE = 2;
        public const int OPOS_S_BUSY = 3;
        public const int OPOS_S_ERROR = 4;
        #endregion
    }
}
