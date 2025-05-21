using POS_Core.Resources.PaymentHandler;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace POS_Core_UI
{
    /// <summary>
    /// Summary description for frmPingFtp.
    /// </summary>
    public class frmWaitScreen : System.Windows.Forms.Form
    {
        private Infragistics.Win.Misc.UltraButton btnCancel1;
        private System.Timers.Timer timer1;
        private ProgressPanel prgPing;
        private WaitFor oWaitFor;
        private bool noTimer;
        private bool noButton;

        private const string CARDSWIPECANCEL = "6000";
        private const string NOPPCAPTURECANCEL = "6002";
        private const string NOPPUSERCANCEL = "6008";
        private const string RXCAPTURECANCEL = "6007";
        private const string SIGNATURECAPTURECANCEL = "6001";
        private const string SWIPEATTEMPTEXCEED = "6003";
        private const string INITIATESIGNATURE = "6005";
        private const string NOPPUSERCONTINUE = "6009";
        private Infragistics.Win.Misc.UltraLabel ultraLabelMsg;
        private Infragistics.Win.Misc.UltraLabel ultraLabelBtnMsg;//NileshJ - Device Cancel Button Message

        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;

        public frmWaitScreen(string strTitle, string sProgressTitle, WaitFor oWaitF)
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();
            this.WindowState = FormWindowState.Normal; // Added By Dharmendra 02-Oct-08
            this.Text = strTitle;
            this.prgPing.Title = sProgressTitle;
            oWaitFor = oWaitF;
            this.ultraLabelMsg.Text = "";
            this.Focus();
            this.Activate();
            this.prgPing.lblProgressMessage.Visible = false;
            //
            // TODO: Add any constructor code after InitializeComponent call
            //
        }

        public void SetMsgDetails(string strTitle)
        {
            this.ultraLabelMsg.Text = strTitle;
        }
        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if(disposing)
            {
                if(components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmWaitScreen));
            this.btnCancel1 = new Infragistics.Win.Misc.UltraButton();
            this.timer1 = new System.Timers.Timer();
            this.ultraLabelMsg = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabelBtnMsg = new Infragistics.Win.Misc.UltraLabel();//NileshJ - Device Cancel Button Message
            this.prgPing = new ProgressPanel();
            ((System.ComponentModel.ISupportInitialize)(this.timer1)).BeginInit();
            this.SuspendLayout();
            // 
            // btnCancel1
            // 
            appearance1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance1.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(108)))), ((int)(((byte)(172)))));
            appearance1.FontData.BoldAsString = "True";
            appearance1.ForeColor = System.Drawing.Color.White;
            appearance1.Image = ((object)(resources.GetObject("appearance1.Image")));
            this.btnCancel1.Appearance = appearance1;
            this.btnCancel1.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel1.Location = new System.Drawing.Point(94, 94);
            this.btnCancel1.Name = "btnCancel1";
            this.btnCancel1.Size = new System.Drawing.Size(104, 28);
            this.btnCancel1.TabIndex = 2;
            this.btnCancel1.Text = "&Cancel";
            this.btnCancel1.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnCancel1.Click += new System.EventHandler(this.btnCancel1_Click);
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 500D;
            this.timer1.SynchronizingObject = this;
            this.timer1.Elapsed += new System.Timers.ElapsedEventHandler(this.timer1_Elapsed);
            // 
            // ultraLabelMsg
            // 
            this.ultraLabelMsg.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultraLabelMsg.Location = new System.Drawing.Point(4, 19);
            this.ultraLabelMsg.Name = "ultraLabelMsg";
            this.ultraLabelMsg.Size = new System.Drawing.Size(300, 19);
            this.ultraLabelMsg.TabIndex = 10;
            // 
            // ultraLabelBtnMsg
            // //NileshJ - Device Cancel Button Message
            this.ultraLabelBtnMsg.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultraLabelBtnMsg.Location = new System.Drawing.Point(3, 94);
            this.ultraLabelBtnMsg.Appearance.ForeColor = System.Drawing.Color.Red;
            this.ultraLabelBtnMsg.Name = "ultraLabelBtnMsg";
            this.ultraLabelBtnMsg.Size = new System.Drawing.Size(400, 19);
            this.ultraLabelBtnMsg.TabIndex = 2;
            // 
            // prgPing
            // 
            this.prgPing.BackColor = System.Drawing.Color.WhiteSmoke;
            this.prgPing.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.prgPing.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.prgPing.Location = new System.Drawing.Point(28, 48);
            this.prgPing.Name = "prgPing";
            this.prgPing.Size = new System.Drawing.Size(248, 40);
            this.prgPing.TabIndex = 1;
            this.prgPing.Title = "Processing......";
            // 
            // frmWaitScreen
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(307, 127);
            this.ControlBox = false;
            this.Controls.Add(this.ultraLabelMsg);
            this.Controls.Add(this.prgPing);
            this.Controls.Add(this.btnCancel1);
            this.Controls.Add(this.ultraLabelBtnMsg);//NileshJ - Device Cancel Button Message
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmWaitScreen";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Processing - Please Wait";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.frmPingFtp_Load);
            ((System.ComponentModel.ISupportInitialize)(this.timer1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion Windows Form Designer generated code


        public frmWaitScreen(bool nobutton, string strTitle, string sProgressTitle)
        {
            this.noButton = nobutton;
            InitializeComponent();
            this.WindowState = FormWindowState.Normal; // Added By Dharmendra 02-Oct-08
            this.Text = strTitle;
            this.ultraLabelMsg.Text = sProgressTitle;
            //NileshJ - Device Cancel Button Message
            if (sProgressTitle == "Processing Payment Online")
            {
                ultraLabelBtnMsg.Text = "Please press the X button on the DEVICE for cancellation";
            }
            //Till
            this.Focus();
            this.Activate();
            this.prgPing.lblProgressMessage.Visible = false;
        }

        private void frmPingFtp_Load(object sender, System.EventArgs e)
        {
            clsUIHelper.setColorSchecme(this);
            if (noButton)
            {
                btnCancel1.Visible = false;
            }
            else
            {
                ultraLabelBtnMsg.Visible = false; //NileshJ - Device Cancel Button Message
            }
            //prgPing.Start();
            prgPing.BackColor = this.BackColor;
            //this.timer1.Enabled = false;
            //oWaitThread = new System.Threading.Thread(new System.Threading.ThreadStart(WaitForProcess));
            //oWaitThread.Start();
        }

        public Form CenterForm(Form child, Form parent)
        {
            child.StartPosition = FormStartPosition.Manual;
            child.Location = new Point(parent.Location.X + (parent.Width - child.Width) / 2, parent.Location.Y + (parent.Height - child.Height) / 2);
            return child;
        }

        //Commented By shitaljit(QuicSolv) on May 2011(Requestd By Manoj(MMS))
        ////private void timer1_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        ////{
        ////    if (oWaitFor == WaitFor.CreditCardProcess)
        ////    {
        ////        if (POS_Core.Resources.PaymentHandler.SigPadUtil.DefaultInstance.SigPadCardInfo != null)
        ////        {
        ////            this.timer1.Stop();
        ////            this.DialogResult = DialogResult.OK;
        ////            this.Close();
        ////        }
        ////    }
        ////    else if (POS_Core.Resources.PaymentHandler.SigPadUtil.DefaultInstance.CustomerSignature != null && POS_Core.Resources.PaymentHandler.SigPadUtil.DefaultInstance.CustomerSignature.Trim() != "")
        ////    {
        ////        if (POS_Core.Resources.PaymentHandler.SigPadUtil.DefaultInstance.CustomerSignature == NOPPUSERCONTINUE)
        ////            clsUIHelper.DefaultValue = NOPPUSERCONTINUE;
        ////        if (POS_Core.Resources.PaymentHandler.SigPadUtil.DefaultInstance.CustomerSignature == NOPPUSERCANCEL)
        ////            clsUIHelper.DefaultValue = NOPPUSERCANCEL;
        ////        if ((oWaitFor == WaitFor.CaptureRXSignature && POS_Core.Resources.PaymentHandler.SigPadUtil.DefaultInstance.CustomerSignature == RXCAPTURECANCEL) ||
        ////            (oWaitFor == WaitFor.CaptureNOPPSignature && POS_Core.Resources.PaymentHandler.SigPadUtil.DefaultInstance.CustomerSignature == NOPPCAPTURECANCEL) ||
        ////            (oWaitFor == WaitFor.CaptureNOPPSignature && POS_Core.Resources.PaymentHandler.SigPadUtil.DefaultInstance.CustomerSignature == NOPPUSERCANCEL) ||
        ////            (oWaitFor == WaitFor.CaptureNOPPSignature && POS_Core.Resources.PaymentHandler.SigPadUtil.DefaultInstance.CustomerSignature == NOPPUSERCONTINUE) ||
        ////            (oWaitFor == WaitFor.CaptureCCSignature && POS_Core.Resources.PaymentHandler.SigPadUtil.DefaultInstance.CustomerSignature == SIGNATURECAPTURECANCEL))
        ////        {
        ////            POS_Core.Resources.PaymentHandler.SigPadUtil.DefaultInstance.CustomerSignature = "";
        ////            closeScreen();
        ////        }
        ////        else if ((POS_Core.Resources.PaymentHandler.SigPadUtil.DefaultInstance.CustomerSignature == NOPPUSERCANCEL)||
        ////            (POS_Core.Resources.PaymentHandler.SigPadUtil.DefaultInstance.CustomerSignature == NOPPUSERCONTINUE))
        ////        {
        ////            POS_Core.Resources.PaymentHandler.SigPadUtil.DefaultInstance.CustomerSignature = "";
        ////        }
        ////        else
        ////        {
        ////            closeScreen();
        ////        }

        ////    }
        ////    //Application.DoEvents();

        ////}

        //This is for the Signature Pad, it pass the values from the PAD to the POS
        private void timer1_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            POS_Core.Resources.PaymentHandler.SigPadUtil.DefaultInstance.PaDResp();
            if(oWaitFor == WaitFor.CreditCardProcess)
            {
                if(POS_Core.Resources.PaymentHandler.SigPadUtil.DefaultInstance.SigPadCardInfo != null)
                {
                    this.timer1.Stop();
                    this.DialogResult = DialogResult.OK;
                    this.Close(); 
                }         
            }
            else if(oWaitFor == WaitFor.CapturePinBlock) //Added By Manoj 6/10/2012
            {
                if(POS_Core.Resources.PaymentHandler.SigPadUtil.DefaultInstance.PINBLOCK != "")
                {
                    this.timer1.Stop();
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }

            }//end add Manoj

            else if(POS_Core.Resources.PaymentHandler.SigPadUtil.DefaultInstance.CustomerSignature != null && POS_Core.Resources.PaymentHandler.SigPadUtil.DefaultInstance.CustomerSignature.Trim() != "")
            {
                clsUIHelper.DefaultValue = string.Empty; // Added by Manoj 05/04/2011
                if(POS_Core.Resources.PaymentHandler.SigPadUtil.DefaultInstance.CustomerSignature == NOPPUSERCONTINUE)
                    clsUIHelper.DefaultValue = NOPPUSERCONTINUE;
                if(POS_Core.Resources.PaymentHandler.SigPadUtil.DefaultInstance.CustomerSignature == NOPPUSERCANCEL)
                    clsUIHelper.DefaultValue = NOPPUSERCANCEL;
                if((oWaitFor == WaitFor.CaptureRXSignature && POS_Core.Resources.PaymentHandler.SigPadUtil.DefaultInstance.CustomerSignature == RXCAPTURECANCEL) ||
                (oWaitFor ==

                WaitFor.CaptureNOPPSignature && POS_Core.Resources.PaymentHandler.SigPadUtil.DefaultInstance.CustomerSignature == NOPPCAPTURECANCEL) ||
                (oWaitFor ==

                WaitFor.CaptureNOPPSignature && POS_Core.Resources.PaymentHandler.SigPadUtil.DefaultInstance.CustomerSignature == NOPPUSERCANCEL) ||
                (oWaitFor ==

                WaitFor.CaptureNOPPSignature && POS_Core.Resources.PaymentHandler.SigPadUtil.DefaultInstance.CustomerSignature == NOPPUSERCONTINUE) ||
                (oWaitFor ==

                WaitFor.CaptureCCSignature && POS_Core.Resources.PaymentHandler.SigPadUtil.DefaultInstance.CustomerSignature == SIGNATURECAPTURECANCEL))
                {
                    SigPadUtil.DefaultInstance.CustomerSignature = "";
                    closeScreen();
                }

                else if((POS_Core.Resources.PaymentHandler.SigPadUtil.DefaultInstance.CustomerSignature == NOPPUSERCANCEL) ||
                (POS_Core.Resources.PaymentHandler.SigPadUtil.DefaultInstance.CustomerSignature == NOPPUSERCONTINUE))
                {
                     SigPadUtil.DefaultInstance.CustomerSignature = "";
                }
                else
                {
                    closeScreen();
                }
            }
            //Application.DoEvents();
        }

        private void closeScreen()
        {
            this.timer1.Enabled = false;
            this.timer1.Stop();
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnCancel1_Click(object sender, EventArgs e)
        {
            this.timer1.Stop();
            this.DialogResult = DialogResult.Cancel;
            this.Close();
            if (POS_Core.Resources.Configuration.CPOSSet.SigPadHostAddr.ToUpper().Trim().Contains("VERIFONEPOSHOSTSRV"))
            {
                POS_Core.Resources.PaymentHandler.SigPadUtil.DefaultInstance.MsrCancel = true;
            }
        }
    }

    public enum WaitFor
    {
        CreditCardProcess,
        CaptureCCSignature,
        CaptureNOPPSignature,
        CaptureRXSignature,
        CaptureOTCSignature, //Added By Shitaljit 0n 16 May
        CapturePinBlock
    }
}