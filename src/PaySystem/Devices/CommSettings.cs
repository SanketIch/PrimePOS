using System;
using NLog;
namespace EDevice
{
	/// <summary>
	/// Setting for the Connected ComPort, USB-HID or Ethernet
	/// </summary>
	public class CommSettings
    {
        private ILogger logger = LogManager.GetCurrentClassLogger();

        #region Connection Interface
        /// <summary>
        /// Connection Interface
        /// </summary>
		public enum eInterface
		{
            /// <summary>
            /// Connection Type Serial
            /// </summary>
			SERIAL = 1,
            /// <summary>
            /// Connection Type USA-HID
            /// </summary>
            USB
		}
        /// <summary>
        /// Device interface (Serial, USB, Ethernet)
        /// </summary>
        public eInterface DeviceInterface;
        #endregion Connection Interface

        #region Device Name
        /// <summary>
        /// List of supported Devices
        /// </summary>
        public enum DeviceName
		{
			/// <summary>
			/// Ingenico ISC's with RBA are supported
			/// </summary>
			Ingenico_isc480,
			/// <summary>
			/// Ingenico ISC's with RBA are supported
			/// </summary>
			Ingenico_isc250
		}
        /// <summary>
        /// Device name that will be use
        /// </summary>
        public DeviceName Device_Name;
        #endregion Device Name

        #region Serial
        /// <summary>
		/// Serial connection
		/// </summary>
		public struct Serial
		{
			/// <summary>
			/// Set the ComPort for the selected device
			/// </summary>
			public uint ComPort;
			/// <summary>
			/// BaudRate for the ComPort
			/// </summary>
			internal string BaudRate
			{
				get
				{
					return "115200";
				}
			}
			/// <summary>
			/// Data Bits for the ComPort
			/// </summary>
			internal uint DataBits
			{
				get
				{
					return 8u;
				}
			}
			/// <summary>
			/// Stop Bit for ComPort
			/// </summary>
			internal uint StopBits
			{
				get
				{
					return 1u;
				}
			}
			/// <summary>
			/// Parity Bit for ComPort
			/// </summary>
			internal uint Parity
			{
				get
				{
					return 0u;
				}
			}
			/// <summary>
			/// Flow Control
			/// </summary>
			internal uint FlowControl
			{
				get
				{
					return 0u;
				}
			}
		}
        /// <summary>
        /// Connection Type of the device
        /// </summary>
        public Serial SERIAL;
        #endregion Serial

        #region USB-HID
        /// <summary>
        /// Only use if you are using USB HID
        /// </summary>
        public struct UsbHid
        {
            /// <summary>
            /// Auto Detection of USB
            /// </summary>
            public enum AutoDetect
            {
                /// <summary>
                /// Auto Detection set to off
                /// </summary>
                AutoDetectedOFF,
                /// <summary>
                /// Auto Detection set to on
                /// </summary>
                AutoDetectedOn
            }
            /// <summary>
            /// Set the USBHID auto detection ON or OFF.  IF OFF, You will have to provide the VID and PID
            /// </summary>
            public AutoDetect AutoDetection;
            /// <summary>
            /// Vendor ID
            /// </summary>
            public ushort VID;
            /// <summary>
            /// Product ID
            /// </summary>
            public ushort PID;
        }
        /// <summary>
        /// Connection Type of the Device
        /// </summary>
        public UsbHid USBHID;
        #endregion USB-HID

        #region Time Out
        /// <summary>
		/// Set Device Time Out.  Higher TimeOut will ensure that all messages are received.
		/// </summary>
		public uint TimeOut
		{
			get;
			set;
        }
        #endregion Time Out
    }
}
