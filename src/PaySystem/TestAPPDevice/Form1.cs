using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using EDevice;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace TestAPPDevice
{
    public partial class Form1 : Form
    {
        /// <summary>
        /// Declaring the Interface
        /// </summary>
        private IEDevice device;
        /// <summary>
        /// Instantiate the CommSettings
        /// </summary>
        CommSettings comm = new CommSettings();
        /// <summary>
        /// Instantiate form Properties
        /// </summary>
        FormProperties.Properties fp = new FormProperties.Properties(); 
        /// <summary>
        /// Instantiate form Setting (this will be static.  All forms and form elements with the data type is store
        /// </summary>
        FormProperties.FormSettings fs = new FormProperties.FormSettings();
        /// <summary>
        /// Instantiate the Device driver.
        /// </summary>
        DeviceAPI isc = null;

        public Form1()
        {
            InitializeComponent();
            fs.CustomForms.FormList = new List<Tuple<string, string[]>>();
            fs.CustomForms.FormList.Add(new Tuple<string, string[]>("PADITEMLIST", new string[] { "linedisplay", "IST", "DIS", "TAX", "ITOT", "lblLineItem" }));
            fs.CustomForms.FormList.Add(new Tuple<string, string[]>("PADNOPP", new string[] { "SigBox" }));
            fs.Signature.CropSignature = true;
            fs.Signature.SignReturnFormat = SigFormat.InString;
        }

        #region Enable /Disable
        private void Enable()
        {
            btnActivate.Enabled = true;
            btnCurrentS.Enabled = true;
            btnShowF1.Enabled = true;
            btnShowF2.Enabled = true;
            btnConnect.Enabled = false;
            btnShutDown.Enabled = true;
            btnCancel.Enabled = true;
            btnLineDisp.Enabled = true;
        }

        private void Disable()
        {
            btnActivate.Enabled = false;  
            btnLineDisp.Enabled = false;
            btnCurrentS.Enabled = false;
            btnShowF1.Enabled = false;
            btnShowF2.Enabled = false;
            btnShutDown.Enabled = false;
            btnCancel.Enabled = false;
            btnConnect.Enabled = true;
        }
        #endregion Enable /Disable

        #region Connect
        private void btnConnect_Click_1(object sender, EventArgs e)
        {
            Enable();

            comm.SERIAL.ComPort = 6;
            comm.Device_Name = CommSettings.DeviceName.Ingenico_isc480;
            comm.DeviceInterface = CommSettings.eInterface.SERIAL;
            comm.TimeOut = 3000;

            if (isc == null)
            {
                isc = new DeviceAPI(comm.Device_Name);
                device = isc.ISCDEVICE(fs);
                device.Connect(comm);
                //device.SignatureEvent += device_Signature;
                device.SignatureEvent += device_SignatureEvent;
                device.ButtonEvent += device_ButtonEvent;
                device.Result += device_Result;
                device.UserInputEvent += device_UserInputEvent;
            }
        }

        void device_ButtonEvent(string arg1, string arg2, string arg3)
        {
            switch (arg1)
            {
                case "PADNOPP":
                    {
                        break;
                    }
                case "PADSIGN":
                    {
                        fp.FormName = fs.CustomForms.FormList[0].Item1;
                        fp.FormElementType = FormProperties.Properties.ElementType.Text;
                        fp.FormElementID = fs.CustomForms.FormList[0].Item2[0];
                        fp.FormElementData = "Rx 10234587".PadRight(15) + "100".PadRight(10) + "$100.00".PadRight(15)
                            + "$5.00".PadRight(20) + "$100.00";
                        device.ShowForm(fp);

                        fp.FormElementType = FormProperties.Properties.ElementType.Text;
                        fp.FormElementID = fs.CustomForms.FormList[0].Item2[1];
                        fp.FormElementData = "Line 1";
                        device.ShowForm(fp);
                        break;
                    }
            }
        }

        void device_SignatureEvent(SigFormat arg1, SigStatus arg2, object arg3)
        {
            switch (arg2)
            {
                case SigStatus.IsAccepted:
                    {
                        switch (arg1)
                        {
                            case SigFormat.InString:
                                {
                                    /* String */
                                    Image img;
                                    byte[] signByte = Convert.FromBase64String((string)arg3);
                                    using (MemoryStream ms = new MemoryStream(signByte))
                                    {
                                        img = Image.FromStream(ms);
                                    }
                                    signBox.Image = img;                                  
                                    break;
                                }
                            case SigFormat.InByte:
                                {
                                    /* Byte */
                                    Bitmap bmp;
                                    using (MemoryStream ms = new MemoryStream((byte[])arg3))
                                    {
                                        bmp = new Bitmap(ms);
                                    }
                                    signBox.Image = bmp;
                                    break;
                                }
                            case SigFormat.inBmp:
                                {
                                    /* BMP */
                                    //using (FileStream FS = new FileStream(fs.Signature.fileName, FileMode.Open, FileAccess.Read))
                                    //{
                                    //    signBox.Image = Image.FromStream(FS);
                                    //}
                                    break;
                                }
                        }
                        break;
                    }
            }
        }


        //void device_SignatureEvent(SigFormat arg1, object arg2)
        //{
        //    switch (arg1)
        //    {
        //        case SigFormat.InByte:
        //            {
        //                Bitmap bmp;
        //                using (MemoryStream ms = new MemoryStream((byte)arg2))
        //                {
        //                    bmp = new Bitmap(ms);
        //                }
        //                signBox.Image = bmp;
        //                break;
        //            }
        //        case SigFormat.inBmp:
        //            {
        //                using (FileStream FS = new FileStream(fs.Signature.fileName, FileMode.Open, FileAccess.Read))
        //                {
        //                    signBox.Image = Image.FromStream(FS);
        //                }
        //                break;
        //            }
        //        case SigFormat.InString:
        //            {
        //                Image img;
        //                byte[] signByte = Convert.FromBase64String((string)arg2);
        //                using (MemoryStream ms = new MemoryStream(signByte))
        //                {
        //                    img = Image.FromStream(ms);
        //                }
        //                signBox.Image = img;
        //                break;
        //            }
        //    }
        //}

        void device_UserInputEvent(string obj)
        {
            
        }

        void device_Result(Dictionary<string, string> obj)
        {
            if (obj != null)
            {
                Form2 frm = new Form2(obj);
                frm.ShowDialog();
            }
        }
        #endregion Connect

       

       // #region Show Screen
        private void btnShowF1_Click(object sender, EventArgs e)
        {
            fp.FormName = fs.CustomForms.FormList[0].Item1;
            fp.FormElementType = FormProperties.Properties.ElementType.Text;
            fp.FormElementID = fs.CustomForms.FormList[0].Item2[0];
            fp.FormElementData = "Rx 10234587".PadRight(15) + "100".PadRight(10) + "$100.00".PadRight(15)
                + "$5.00".PadRight(20) + "$100.00";
            device.ShowForm(fp);

            fp.UpdateFormElement.Update = new List<Tuple<string[], string[]>>();
            fp.UpdateFormElement.Update.Add(new Tuple<string[], string[]>(new string[] { "IST", "Dis", "lblLineItem","Tax","ITOT" }, new string[] { "$1.0", "$0.5", "Line Item: " + device.LineItemNumber,"$0.0","$1.50" }));
            device.UpdateFormElement(fp.UpdateFormElement);
        }

        private void btnShowF2_Click(object sender, EventArgs e)
        {
           // device.UserInputForm(EDevice.ISC.InputTags.PromptTags.DOB, EDevice.ISC.InputTags.FormatTags.DOB);
           // PaymentTags pt = new PaymentTags();
        
            fp.FormName = fs.CustomForms.FormList[1].Item1;
            fp.FormElementType = FormProperties.Properties.ElementType.Text;
            fp.FormElementID = fs.CustomForms.FormList[1].Item2[0];
            fp.FormElementData = "";
            device.ShowForm(fp);
        }
        
        private void btnCurrentS_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(device.GetCurrentForm))
            {
                MessageBox.Show(device.GetCurrentForm);
            }
            else
            {
                MessageBox.Show("Please process a form to see current screen");
            }
        }

        private void btnShutDown_Click(object sender, EventArgs e)
        {
            device.ShutDown();
            Disable();
        }

        private void btnActivate_Click(object sender, EventArgs e)
        {
            GatewayParameters gt = new GatewayParameters();
            gt.AccountID = "WYWVM";
            gt.MerchantPIN = "mmsdemo";
            gt.SubID = "WLISO";

            GatewayTags gtags = new GatewayTags();
            gtags.ProcessorName = GatewayTags.Processor.WorldPay;
            gtags.AccID = "WYWVM";
            gtags.MerchantPIN = "mmsdemo";
            gtags.SubID = "WLISO";

            PaymentTags pt = new PaymentTags();
            //pt.ShowPayScreen.ShowUI = false;
          //  pt.ShowPayScreen.Amount = 100;
            //pt.ShowPayScreen.InvoiceNumber = "12456";
            pt.PaymentType = PaymentTags.payType.Credit;
            pt.TransactionType = PaymentTags.transactionType.Sale;

            PaymentTags.StoredProfile sp = new PaymentTags.StoredProfile();
            sp.IsStoredProfile = false;
            pt.StoreProfile = sp;
            
            
            switch (pt.TransactionType)
            {
                case PaymentTags.transactionType.Void:
                case PaymentTags.transactionType.Credit:
                    {
                        pt.Reversal.HistoryID = "446972724";
                        pt.Reversal.OrderID = "328291396";
                        break;
                    }
            }
            
          //  pt.FSA.AutoSubstantiation.RxAmount = 1;
           // pt.FSA.AutoSubstantiation.TotalHealthCareAmount = 1;
          //  pt.Amount.TipAmount = 1.0;
            pt.Amount.TotalAmount = 1.0;
            device.ProcessTransaction(pt);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            device.CancelTransaction();
        }

        private void btnLineDisp_Click(object sender, EventArgs e)
        {
            device.RemoveFromLineDisplay(0);

            fp.UpdateFormElement.Update = new List<Tuple<string[], string[]>>();
            fp.UpdateFormElement.Update.Add(new Tuple<string[], string[]>(new string[] { "IST", "Dis", "lblLineItem", "Tax", "ITOT" }, new string[] { "$1.0", "$0.5", "Line Item: " + device.LineItemNumber, "$0.0", "$1.50" }));
            device.UpdateFormElement(fp.UpdateFormElement);
        }

    }
}
