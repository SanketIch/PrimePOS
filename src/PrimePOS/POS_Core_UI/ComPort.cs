using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.IO.Ports;
using System.Threading;
//using POS_Core.DataAccess;
using NLog;
using POS_Core.Resources;

namespace POS_Core_UI
{
	/// <summary>
	/// Summary description for ComPort.
	/// </summary>
	/// 
	public class ComPort : System.Windows.Forms.Form
	{
		private AxMSCommLib.AxMSComm axMSComm1;
        private SerialPort commPort;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
        private static ILogger logger = LogManager.GetCurrentClassLogger();

		public ComPort()
		{
			//
			// Required for Windows Form Designer support
			//
			//InitializeComponent();
            SerialPortInit();
			//
			// TODO: Add any constructor code after InitializeComponent call
			//
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
        //private void InitializeComponent()
        //{
        //    System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ComPort));
        //    this.axMSComm1 = new AxMSCommLib.AxMSComm();
        //    ((System.ComponentModel.ISupportInitialize)(this.axMSComm1)).BeginInit();
        //    this.SuspendLayout();
        //    // 
        //    // axMSComm1
        //    // 
        //    this.axMSComm1.Enabled = true;
        //    this.axMSComm1.Location = new System.Drawing.Point(185, 117);
        //    this.axMSComm1.Name = "axMSComm1";
        //    this.axMSComm1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axMSComm1.OcxState")));
        //    this.axMSComm1.Size = new System.Drawing.Size(38, 38);
        //    this.axMSComm1.TabIndex = 0;
        //    // 
        //    // ComPort
        //    // 
        //    this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
        //    this.ClientSize = new System.Drawing.Size(292, 273);
        //    this.Controls.Add(this.axMSComm1);
        //    this.Name = "ComPort";
        //    this.Load += new System.EventHandler(this.ComPort_Load_1);
        //    ((System.ComponentModel.ISupportInitialize)(this.axMSComm1)).EndInit();
        //    this.ResumeLayout(false);

        //}
		#endregion

        #region .Net Serial Port by Manoj
        private void SerialPortInit()
        {
            try
            {
                if (Configuration.CPOSSet.UsePoleDisplay == true)
                {
                    commPort = new SerialPort();
                    commPort.PortName = "COM" + Configuration.CPOSSet.PD_PORT;
                    commPort.BaudRate = Convert.ToInt32(Configuration.CPOSSet.PDP_BAUD);

                    string Paritybit = string.Empty;
                    switch (Configuration.CPOSSet.PDP_PARITY.ToUpper())
                    {
                        case "N":
                            Paritybit = "None";
                            break;
                        case "E":
                            Paritybit = "Even";
                            break;
                        case "M":
                            Paritybit = "Mark";
                            break;
                        case "O":
                            Paritybit = "Odd";
                            break;
                        case "S":
                            Paritybit = "Space";
                            break;
                        default:
                            Paritybit = "None";
                            break;
                    }

                    commPort.Parity = (Parity)Enum.Parse(typeof(Parity), Paritybit);
                    commPort.DataBits = Convert.ToInt32(Configuration.CPOSSet.PDP_DBITS);
                    commPort.StopBits = (StopBits)Enum.Parse(typeof(StopBits), Configuration.CPOSSet.PDP_STOPB);
                }
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "SerialPortInit()");
                MessageBox.Show(ex.Message, "PoleDisplay Fail");
            }
        }
        #endregion

        #region Write to Pole Display by Manoj
        public void WriteToPoleDisplay(string strData)
        {
            try
            {
                if (Configuration.CPOSSet.UsePoleDisplay == true)
                {
                    if (commPort == null)
                    {
                        SerialPortInit(); //Init the PoleDisplay if it was not.
                    }

                    if (!commPort.IsOpen)
                    {
                        commPort.Open(); // Check if the port is open, Open port
                    }
                    commPort.Write(Convert.ToChar(31).ToString()); // Clear the Display
                    commPort.Write(strData); //Write to the Display
                    commPort.DiscardInBuffer(); // Discard data from serial receive buffer
                    commPort.DiscardOutBuffer();
                }
            }
            catch (UnauthorizedAccessException uex)
            {
                logger.Fatal(uex, "WriteToPoleDisplay(string strData)");
                MessageBox.Show("Pole Display Error: " + uex.Message, "Port Access");
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "WriteToPoleDisplay(string strData)");
                MessageBox.Show("Pole Display error: " + ex.Message, "Pole Display Connection");
                SerialPortInit();
            }
        }
        #endregion 


        #region Close Com Port - Manoj
        public void CloseSerialPort()
        {
            try
            {
                if (commPort != null)
                {
                    if (commPort.IsOpen)
                    {
                        commPort.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "CloseSerialPort()");
                MessageBox.Show(ex.Message, "Pole Display");
            }
        }
        #endregion


        //Commented By Shitaljit(QuicSolv) on 4 may 2011(As suggested by Manoj(MMS))

        ////public void WriteToPoleDisplay(string strData)
        ////{
        ////    try
        ////    {
        ////        if (Configuration.CPOSSet.UsePoleDisplay==true)
        ////        {
        ////            axMSComm1.CommPort = Convert.ToInt16( Configuration.CPOSSet.PD_PORT);
        ////            //If .PortOpen = False Then MsgBox("Unable to open serial port")

        ////            axMSComm1.Settings = Configuration.CPOSSet.PDP_BAUD + "," + Configuration.CPOSSet.PDP_PARITY + "," 
        ////                + Configuration.CPOSSet.PDP_DBITS + "," + Configuration.CPOSSet.PDP_STOPB;
        ////            if (axMSComm1.PortOpen == true)
        ////            {
        ////                axMSComm1.PortOpen = false;
        ////            }
        ////            axMSComm1.PortOpen = true;
        ////            axMSComm1.Output = Convert.ToChar(31).ToString() + Convert.ToChar(17).ToString();
        ////            axMSComm1.Output=strData;
        ////            axMSComm1.PortOpen = false;
        ////        }
        ////    }
        ////    catch(Exception exp)
        ////    {	clsUIHelper.ShowErrorMsg("Error using Pole Display. \n" +exp.Message); }
        ////}

        //public void WriteToPoleDisplay(string strData)
        //{   // Added by Manoj 4/20/2011 This take care of the Pole Display freezing problem
        //    try
        //    {
        //        if (Configuration.CPOSSet.UsePoleDisplay == true)
        //        {

        //            axMSComm1.CommPort = Convert.ToInt16(Configuration.CPOSSet.PD_PORT);
        //            //If .PortOpen = False Then MsgBox("Unable to open serial port")
        //            axMSComm1.Settings = Configuration.CPOSSet.PDP_BAUD + "," + Configuration.CPOSSet.PDP_PARITY + ","
        //                + Configuration.CPOSSet.PDP_DBITS + "," + Configuration.CPOSSet.PDP_STOPB;
        //            try
        //            {
        //                if (!axMSComm1.PortOpen)
        //                {
        //                    axMSComm1.PortOpen = true;
        //                }
        //                axMSComm1.Handshaking = MSCommLib.HandshakeConstants.comNone;
        //            }
        //            catch (Exception ex)
        //            {
        //                MessageBox.Show("Port error: " + ex.ToString(), "Port error");
        //            }
        //            try
        //            {
        //                axMSComm1.Output = Convert.ToChar(31).ToString() + Convert.ToChar(17).ToString();
        //                axMSComm1.Output = strData;
        //            }
        //            catch (Exception ex2)
        //            {
        //                MessageBox.Show("output error" + ex2.ToString(), "Output");
        //            }        
        //            //axMSComm1.PortOpen = false;
        //            if (axMSComm1.PortOpen)
        //            {
        //                axMSComm1.PortOpen = false;
        //            }
        //        }
        //    }
        //    catch (Exception exp)
        //    {
        //        clsUIHelper.ShowErrorMsg("Error using Pole Display. \n" + exp.Message);
        //    }
        //}

		public void ClearPoleDisplay()
		{
			string DisplayData;
			DisplayData="";
			DisplayData+=clsUIHelper.Spaces(Convert.ToInt32(Configuration.CPOSSet.PD_LINELEN)*Convert.ToInt32(Configuration.CPOSSet.PD_LINES));
			this.WriteToPoleDisplay(DisplayData);
		}

		private void ComPort_Load(object sender, System.EventArgs e)
		{
			//axMSComm1=new MSCommLib.MSCommClass();		
		}

        private void ComPort_Load_1(object sender, EventArgs e)
        {

        }

	}
}
