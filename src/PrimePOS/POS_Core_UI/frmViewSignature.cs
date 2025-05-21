using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using POS_Core.CommonData.Tables;
using POS_Core.CommonData.Rows;
using POS_Core.CommonData;
using POS_Core.DataAccess;
using System.IO;
using System.Drawing.Imaging;
using POS_Core.Resources;
using System.Text;
using POS_Core.Resources.PaymentHandler;
//using POS_Core.DataAccess;
namespace POS_Core_UI
{
    /// <summary>
    /// Summary description for frmPingFtp.
    /// </summary>
    public class frmViewSignature : System.Windows.Forms.Form
    {
        private Infragistics.Win.Misc.UltraButton btnClose;
        private string strData;
        private byte[] sigData = null;
        private string sSigType;
        private GroupBox groupBox1;
        private PictureBox picSignature;
        public Label lblSigCaption;
        private Infragistics.Win.Misc.UltraButton btnApprove;
        private bool isSignatureRejected = false; // Added By Dharmendra (SRT) for signature approval
        private bool ShowApproveButton = false;
        public bool callByUI = false;
        public byte[] BSigData
        {
            get; set;
        }

        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;

        public frmViewSignature(string strSignData, string strSigType)
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();
            strData = strSignData;
            sSigType = strSigType;
            lblSigCaption.Text = "";
            //
            // TODO: Add any constructor code after InitializeComponent call
            //
        }

        public frmViewSignature(byte[] strSignData, string strSigType, string strPaymentProcesor)   //PRIMEPOS-2900 16-Sep-2020 JY Added strPaymentProcesor parameter
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();
            sigData = strSignData;
            sSigType = strSigType;
            lblSigCaption.Text = "";
            //
            // TODO: Add any constructor code after InitializeComponent call
            //

            //if (POS_Core.Resources.PaymentHandler.SigPadUtil.DefaultInstance.isISC || POS_Core.Resources.PaymentHandler.SigPadUtil.DefaultInstance.isWP)  //PRIMEPOS-2900 16-Sep-2020 JY Commented
            if (strPaymentProcesor == "XLINK" || strPaymentProcesor == "WORLDPAY")   //PRIMEPOS-2900 16-Sep-2020 JY Added
            {
                string oError = string.Empty;
                this.picSignature.Image = clsUIHelper.GetSignature(strSignData, out oError);
                this.picSignature.SizeMode = PictureBoxSizeMode.StretchImage;   //PRIMEPOS-2422 24-Feb-2021 JY Added
            }
            //PRIMEPOS-2664
            //else if (Configuration.CPOSSet.SigPadHostAddr.ToUpper().Trim().Contains("EVERTEC"))//PRIMEPOS-2664    //PRIMEPOS-2900 16-Sep-2020 JY Commented
            else if (strPaymentProcesor == "EVERTEC")   //PRIMEPOS-2900 16-Sep-2020 JY Added
            {
                //MemoryStream ms = new MemoryStream(sigData);                
                //this.picSignature.Image = bmp;
                SigDiplay.SigDisplay sigDisp = new SigDiplay.SigDisplay();
                string errorMsg = string.Empty;
                Bitmap bmp = new Bitmap(345, 245);
                sigDisp.DrawSignatureMXCrop(sigData, ref bmp, out errorMsg, new Size(0, 0));

                this.picSignature.Image = bmp;

                picSignature.SizeMode = PictureBoxSizeMode.StretchImage;   //PRIMEPOS-2422 24-Feb-2021 JY Added
            }
            //PRIMEPOS-2636
            //else if (Configuration.CPOSSet.SigPadHostAddr.ToUpper().Trim().Contains("V1"))    //PRIMEPOS-2900 16-Sep-2020 JY Commented
            else if (strPaymentProcesor == "VANTIV" || strPaymentProcesor == "NB_VANTIV")   //PRIMEPOS-2900 16-Sep-2020 JY Added //PRIMEPOS-3407 //PRIMEPOS-3482
            {
                #region PRIMEPOS-3176
                try
                {
                    Bitmap sigBitmap = clsUIHelper.ConvertPoints(sigData);
                    this.picSignature.SizeMode = PictureBoxSizeMode.StretchImage;
                    this.picSignature.Image = sigBitmap;
                }
                catch (Exception ex)
                {
                    if (ex.Message.Contains("Parameter is not valid"))
                    {
                        string oError = string.Empty;
                        this.picSignature.Image = clsUIHelper.GetSignature(strSignData, out oError);
                        this.picSignature.SizeMode = PictureBoxSizeMode.StretchImage;   
                    } 
                }
                #endregion
            }
            else if (strPaymentProcesor == "HPSPAX")   //PRIMEPOS-2952
            {
                string oError = string.Empty;
                strData = Encoding.Default.GetString(sigData);
                this.picSignature.Image = clsUIHelper.GetSignaturePAX(this.strData, out oError, strSigType, out strSignData);
                this.picSignature.SizeMode = PictureBoxSizeMode.StretchImage;
            }
            else
            {
                MemoryStream ms = new MemoryStream(strSignData);
                //ms.Position = 0;
                //ms.Seek(0, SeekOrigin.Begin);
                Bitmap myBitmap = new Bitmap(ms);

                Bitmap b = new Bitmap(myBitmap.Width, myBitmap.Height, PixelFormat.Format32bppArgb);
                //Bitmap b = new Bitmap(myBitmap.Width, myBitmap.Height, PixelFormat.Format24bppRgb);
                b = (Bitmap)myBitmap.Clone(new Rectangle(0, 0, myBitmap.Width, myBitmap.Height), PixelFormat.Format32bppArgb);
                //b = (Bitmap)myBitmap.Clone(new Rectangle(0, 0, myBitmap.Width, myBitmap.Height), PixelFormat.Format24bppRgb);
                //pictureBox1.Image = b;

                picSignature.Image = b;
                picSignature.SizeMode = PictureBoxSizeMode.StretchImage;   //PRIMEPOS-2422 24-Feb-2021 JY Added
            }
            #region comment
            //SqlConnection CN = new SqlConnection("Server=(local)\\SQLEXPRESS; Database=POSSQLOAK; uid=sa; pwd=MMSPhW110");
            ////Initialize SQL adapter.          
            //SqlDataAdapter ADAP = new SqlDataAdapter("Select * from POSTransPayment", CN);
            ////Initialize Dataset.    
            //DataSet DS = new DataSet();
            ////Fill dataset with ImagesStore table.     
            //ADAP.Fill(DS, "POSTransPayment");
            //Byte[] bt = (byte[])DS.Tables[0].Rows[DS.Tables[0].Rows.Count - 4]["BinarySign"];
            //MemoryStream ms = new MemoryStream(bt);
            //Bitmap myBitmap = new Bitmap(ms);

            ////Image img = Image.FromStream(ms);
            //Bitmap b = new Bitmap(myBitmap.Width, myBitmap.Height, PixelFormat.Format32bppArgb);
            //b = (Bitmap)myBitmap.Clone(new Rectangle(0, 0, myBitmap.Width, myBitmap.Height), PixelFormat.Format32bppArgb);
            //pictureBox1.Image = b;
            #endregion
        }

        public void SetMsgDetails(string strSignatureCaption)
        {
            lblSigCaption.Text = strSignatureCaption;
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
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
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            this.btnClose = new Infragistics.Win.Misc.UltraButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lblSigCaption = new System.Windows.Forms.Label();
            this.picSignature = new System.Windows.Forms.PictureBox();
            this.btnApprove = new Infragistics.Win.Misc.UltraButton();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picSignature)).BeginInit();
            this.SuspendLayout();
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            appearance1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance1.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(108)))), ((int)(((byte)(172)))));
            appearance1.FontData.BoldAsString = "True";
            appearance1.ForeColor = System.Drawing.Color.White;
            this.btnClose.Appearance = appearance1;
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnClose.Location = new System.Drawing.Point(419, 340);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(123, 29);
            this.btnClose.TabIndex = 1;
            this.btnClose.Text = "&Close";
            this.btnClose.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.lblSigCaption);
            this.groupBox1.Controls.Add(this.picSignature);
            this.groupBox1.Location = new System.Drawing.Point(10, 5);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(672, 329);
            this.groupBox1.TabIndex = 8;
            this.groupBox1.TabStop = false;
            // 
            // lblSigCaption
            // 
            this.lblSigCaption.AutoSize = true;
            this.lblSigCaption.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSigCaption.Location = new System.Drawing.Point(200, 16);
            this.lblSigCaption.Name = "lblSigCaption";
            this.lblSigCaption.Size = new System.Drawing.Size(0, 18);
            this.lblSigCaption.TabIndex = 11;
            // 
            // picSignature
            // 
            this.picSignature.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.picSignature.BackColor = System.Drawing.Color.White;
            this.picSignature.Location = new System.Drawing.Point(10, 15);
            this.picSignature.Name = "picSignature";
            this.picSignature.Size = new System.Drawing.Size(652, 305);
            this.picSignature.TabIndex = 0;
            this.picSignature.TabStop = false;
            // 
            // btnApprove
            // 
            this.btnApprove.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            appearance2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance2.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(108)))), ((int)(((byte)(172)))));
            appearance2.FontData.BoldAsString = "True";
            appearance2.ForeColor = System.Drawing.Color.White;
            this.btnApprove.Appearance = appearance2;
            this.btnApprove.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnApprove.Enabled = false;
            this.btnApprove.Location = new System.Drawing.Point(140, 340);
            this.btnApprove.Name = "btnApprove";
            this.btnApprove.Size = new System.Drawing.Size(123, 29);
            this.btnApprove.TabIndex = 0;
            this.btnApprove.Text = "&Approved";
            this.btnApprove.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnApprove.Visible = false;
            this.btnApprove.Click += new System.EventHandler(this.btnApprove_Click);
            // 
            // frmViewSignature
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(691, 373);
            this.Controls.Add(this.btnApprove);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnClose);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmViewSignature";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Customer Signature";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.frmPingFtp_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picSignature)).EndInit();
            this.ResumeLayout(false);

        }
        #endregion

        private void frmPingFtp_Load(object sender, System.EventArgs e)
        {
            clsUIHelper.setColorSchecme(this);
            if (callByUI)
            {

                if (sSigType == clsPOSDBConstants.STRINGIMAGE)
                {
                    this.picSignature.Image = clsUIHelper.GetSignature(this.strData, sSigType);
                }
                if (Configuration.CPOSSet.SigPadHostAddr.ToUpper().Trim().Contains("HPSPAX") || Configuration.CPOSSet.SigPadHostAddr.ToUpper().Trim().Contains("ELAVON"))//Added for Signature Aries8 //PRIMEPOS-2952//2943
                {
                    string oError = string.Empty;
                    this.picSignature.Image = clsUIHelper.GetSignature(sigData, out oError);
                    this.picSignature.SizeMode = PictureBoxSizeMode.StretchImage;
                }

            }
            else
            {
                if (POS_Core.Resources.PaymentHandler.SigPadUtil.DefaultInstance.isISC || POS_Core.Resources.PaymentHandler.SigPadUtil.DefaultInstance.isWP)
                {
                    string oError = string.Empty;
                    byte[] iscsign = Convert.FromBase64String(POS_Core.Resources.PaymentHandler.SigPadUtil.DefaultInstance.CustomerSignature);
                    this.picSignature.Image = clsUIHelper.GetSignature(iscsign, out oError);
                }
                else if (Configuration.CPOSSet.SigPadHostAddr.ToUpper().Trim().Contains("HPSPAX"))
                {
                    //Suraj
                    /*Added for hpspax to work even when use sigpad is false*/
                    string oError = string.Empty;
                    byte[] SigData = null;
                    //Bitmap sigBitmap = clsUIHelper.GetSignature(this.strData, out oError, sSigType, out SigData); //Commented For Aries8
                    Bitmap sigBitmap = clsUIHelper.GetSignaturePAX(this.strData, out oError, sSigType, out SigData); //PRIMEPOS-2952
                    this.BSigData = SigData;
                    //this.picSignature.Image = sigBitmap;
                    //PRIMEPOS-2998
                    //Bitmap result = new Bitmap(sigBitmap.Width, sigBitmap.Height);
                    //if (Configuration.CPOSSet.PinPadModel == "HPSPAX_ARIES8")
                    //{
                    //    using (Graphics g = Graphics.FromImage(result))
                    //    {
                    //        g.DrawImage(sigBitmap, 0, 0, 1500, 1000);
                    //    }
                    //}
                    //else
                    //{
                    //    using (Graphics g = Graphics.FromImage(result))
                    //    {
                    //        g.DrawImage(sigBitmap, 0, 0, 335, 450);
                    //    }
                    //}
                    this.picSignature.Image = sigBitmap;
                    this.picSignature.SizeMode = PictureBoxSizeMode.StretchImage;
                    MemoryStream ms = new MemoryStream();
                    this.picSignature.Image.Save(ms, ImageFormat.Png);
                    Bitmap bitmap = new Bitmap(ms);
                    this.picSignature.Image = bitmap;
                    SigPadUtil.DefaultInstance.CustomerSignature = Encoding.Default.GetString(ms.ToArray());
                }
                //PRIMEPOS-2664
                else if (Configuration.CPOSSet.SigPadHostAddr.ToUpper().Trim().Contains("EVERTEC"))
                {
                    byte[] imageData = Encoding.Default.GetBytes(strData);                    
                    MemoryStream ms = new MemoryStream(imageData);
                    ms.Position = 0L;
                    ms.Seek(0L, SeekOrigin.Begin);
                    Bitmap bitmap1 = new Bitmap((Stream)ms);
                    ms.Position = 0L;
                    ms.Seek(0L, SeekOrigin.Begin);
                    Bitmap bitmap2 = new Bitmap(bitmap1.Width, bitmap1.Height);
                    Graphics graphics = Graphics.FromImage((Image)bitmap2);
                    graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                    graphics.Clear(Color.White);
                    graphics.DrawImage((Image)bitmap1, 0, 0);                    
                    ImageConverter converter = new ImageConverter();
                    byte[] btarr = (byte[])converter.ConvertTo(bitmap2, typeof(byte[]));
                    SigPadUtil.DefaultInstance.CustomerSignature = Encoding.Default.GetString(btarr);                    
                    this.picSignature.Image = bitmap2;
                }
                //PRIMEPOS-2636
                else if (Configuration.CPOSSet.SigPadHostAddr.ToUpper().Trim().Contains("V1"))//VANTIV
                {
                    byte[] bytes = Encoding.Default.GetBytes(this.strData);

                    Bitmap sigBitmap = clsUIHelper.ConvertPoints(bytes);
                    this.picSignature.SizeMode = PictureBoxSizeMode.StretchImage;
                    this.picSignature.Image = sigBitmap;
                    MemoryStream ms = new MemoryStream();
                    this.picSignature.Image.Save(ms, ImageFormat.Png);
                    Bitmap bitmap = new Bitmap(ms);
                    this.picSignature.Image = bitmap;
                    SigPadUtil.DefaultInstance.CustomerSignature = Encoding.Default.GetString(ms.ToArray());
                }
                //
                //2943
                else if (Configuration.CPOSSet.SigPadHostAddr.ToUpper().Trim().Contains("ELAVON"))
                {
                    byte[] bytes = Encoding.Default.GetBytes(this.strData);

                    Bitmap sigBitmap = clsUIHelper.ConvertPointsFromASCII3Byte(bytes);

                    this.picSignature.SizeMode = PictureBoxSizeMode.StretchImage;
                    this.picSignature.Image = sigBitmap;
                    MemoryStream ms = new MemoryStream();
                    this.picSignature.Image.Save(ms, ImageFormat.Png);
                    Bitmap bitmap = new Bitmap(ms);
                    this.picSignature.Image = bitmap;
                    SigPadUtil.DefaultInstance.CustomerSignature = Encoding.Default.GetString(ms.ToArray());
                }
                else
                {
                    this.picSignature.Image = clsUIHelper.GetSignature(this.strData, sSigType);
                }
                this.picSignature.SizeMode = PictureBoxSizeMode.StretchImage;   //PRIMEPOS-2481 21-Apr-2021 JY Added
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            // Mantis Id : 0000119 Added By Dharmendra (SRT) on Dec-02-08
            if (ShowApproveButton == true && btnClose.Text == "Rejected")
            {
                isSignatureRejected = true; //Setting the value  to true

            }
            else
            {
                isSignatureRejected = false; //Setting the value of the property to true
            }
            this.Close();

        }
        // Mantis Id : 0000119 Added By Dharmendra (SRT) on Nov-26-08
        public frmViewSignature(string strSignData, string strSigType, bool ShowApproveButton)
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();
            strData = strSignData;
            sSigType = strSigType;
            lblSigCaption.Text = "";

            if (ShowApproveButton == true)
            {
                btnClose.Text = "Rejected"; //Modified By Dharmendra(SRT) on Dec-15-08 corrected spelling
                btnApprove.Visible = true;
                btnApprove.Enabled = true;
                this.ShowApproveButton = ShowApproveButton;
                btnApprove.Focus(); //Added By Dharmendra (SRT)on Dec-15-08 to set default focus
            }
            //2943
            if (Configuration.CPOSSet.SigPadHostAddr.ToUpper().Trim().Contains("ELAVON") && Configuration.CPOSSet.DispSigOnTrans == false)
            {
                byte[] bytes = Encoding.Default.GetBytes(this.strData);

                Bitmap sigBitmap = clsUIHelper.ConvertPointsFromASCII3Byte(bytes);

                this.picSignature.SizeMode = PictureBoxSizeMode.StretchImage;
                this.picSignature.Image = sigBitmap;
                MemoryStream ms = new MemoryStream();
                this.picSignature.Image.Save(ms, ImageFormat.Png);
                Bitmap bitmap = new Bitmap(ms);
                this.picSignature.Image = bitmap;
                SigPadUtil.DefaultInstance.CustomerSignature = Encoding.Default.GetString(ms.ToArray());
            }
            if (Configuration.CPOSSet.SigPadHostAddr.ToUpper().Trim().Contains("HPSPAX") && Configuration.CPOSSet.DispSigOnTrans == false)//PRIMEPOS-2952
            {
                string oError = string.Empty;
                byte[] SigData = null;
                Bitmap sigBitmap = clsUIHelper.GetSignaturePAX(this.strData, out oError, sSigType, out SigData);
                this.BSigData = SigData;
                //PRIMEPOS-2998
                //Bitmap result = new Bitmap(sigBitmap.Width, sigBitmap.Height);
                //if (Configuration.CPOSSet.PinPadModel == "HPSPAX_ARIES8")
                //{
                //    using (Graphics g = Graphics.FromImage(result))
                //    {
                //        g.DrawImage(sigBitmap, 0, 0, 1500, 1000);
                //    }
                //}
                //else
                //{
                //    using (Graphics g = Graphics.FromImage(result))
                //    {
                //        g.DrawImage(sigBitmap, 0, 0, 335, 450);
                //    }
                //}
                this.picSignature.Image = sigBitmap;
                this.picSignature.SizeMode = PictureBoxSizeMode.StretchImage;
                SigPadUtil.DefaultInstance.CustomerSignature = Encoding.Default.GetString(this.BSigData);
            }
            if (Configuration.CPOSSet.SigPadHostAddr.ToUpper().Trim().Contains("EVERTEC") && Configuration.CPOSSet.DispSigOnTrans == false)
            {
                byte[] imageData = Encoding.Default.GetBytes(strData);
                MemoryStream ms = new MemoryStream(imageData);
                string base64String = Convert.ToBase64String(ms.ToArray());
                ms.Position = 0L;
                ms.Seek(0L, SeekOrigin.Begin);
                Bitmap bitmap1 = new Bitmap((Stream)ms);
                ms.Position = 0L;
                ms.Seek(0L, SeekOrigin.Begin);
                Bitmap bitmap2 = new Bitmap(bitmap1.Width, bitmap1.Height);
                Graphics graphics = Graphics.FromImage((Image)bitmap2);
                graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                graphics.Clear(Color.White);
                graphics.DrawImage((Image)bitmap1, 0, 0);
                ImageConverter converter = new ImageConverter();
                byte[] btarr = (byte[])converter.ConvertTo(bitmap2, typeof(byte[]));
                SigPadUtil.DefaultInstance.CustomerSignature = Encoding.Default.GetString(btarr);

                this.picSignature.Image = bitmap2;

                picSignature.SizeMode = PictureBoxSizeMode.StretchImage;   //PRIMEPOS-2422 24-Feb-2021 JY Added
            }
        }

        private void btnApprove_Click(object sender, EventArgs e)
        {
            isSignatureRejected = false; //  Mantis Id : 0000119 Setting the value of the property to true
            this.Close();
        }
        public bool IsSignatureRejected
        {
            get
            {
                return isSignatureRejected;
            }
        }
        // Mantis Id : 0000119 End Added
    }
}
