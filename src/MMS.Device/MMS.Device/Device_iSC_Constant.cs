using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MMS.Device
{
    class Device_iSC_Constant
    {

    }

    internal class EMVPreTag
    {
        public string Amount { get; set; }
        public string TransType { get; set; }
        public string PayType { get; set; }
    }

    /// <summary>
    /// ComPort setting for iSC 480
    /// </summary>
    internal class CONNECTIONSETTING
    {
        /// <summary>
        /// Set the ComPort for the selected device
        /// </summary>
        public uint ComPort;
        /// <summary>
        /// BaudRate for the ComPort
        /// </summary>
        public string BaudRate = "115200";
        /// <summary>
        /// Data Bits for the ComPort
        /// </summary>
        public uint DataBits = 8;
        /// <summary>
        /// Stop Bit for ComPort
        /// </summary>
        public uint StopBits = 1;
        /// <summary>
        /// Parity Bit for ComPort
        /// </summary>
        public uint Parity = 0;
        /// <summary>
        /// Flow Control
        /// </summary>
        public uint FlowControl = 0;
        /// <summary>
        /// Set Device Time Out.  Higher TimeOut will ensure that all messages are received.
        /// </summary>
        public uint TimeOut = 3000;
    }

    /// <summary>
    /// Forms for iSC480
    /// </summary>
    internal static class iSCFormName
    {
        public const string INIT = "INIT";
        public const string WELCOME = "PADWELCOME";
        public const string PADITEMLIST = "PADITEMLIST";
        public const string RXPICKUPLIST = "PADRXLIST";
        public const string NOPP = "PADNOPP";
        public const string REMINDER = "PADREMINDER";
        public const string SIGN = "PADSIGN";
        public const string SWIPE = "PADSWIPEDCARD";
        public const string MESSAGE = "PADMSGSCREEN";
        public const string OTC = "PADOTC";
        public const string FormExt = ".K3Z";
        public const string REINIT = "REINIT"; //PRIMEPOS-2534 - Lane CLose Issue
    }



    /// <summary>
    /// Calling forms from POS
    /// </summary>
    internal static class POSFormName
    {
        public const string TXTMESSAGE_SCREEN = "TXTMESSAGE_SCREEN";
        public const string ITEMLIST_SCREEN = "ITEMLIST_SCREEN";
        public const string XLINK = "XLINK";
        public const string SWIPE_SCREEN = "SWIPE_SCREEN";
        public const string SIGN_SCREEN = "SIGN_SCREEN";
        public const string OTC_SIGN_SCREEN = "OTC_SIGN_SCREEN";
        public const string PADRXLIST = "PADRXLIST";
        public const string PADNOPP = "PADNOPP";
        public const string PADSHOWCASH = "PADSHOWCASH";
    }
}
