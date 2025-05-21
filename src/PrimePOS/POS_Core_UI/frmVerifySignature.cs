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
using System.Text;
using POS_Core.Resources;
using POS_Core.Resources.PaymentHandler;

namespace POS_Core_UI
{

    /// <summary>
    /// /Added By Rohit Nair for  PRIMEPOS-2370 on Jan 2017
    /// Summary description for frmVerify SIgnature
    /// </summary>
    public class frmVerifySignature : System.Windows.Forms.Form
    {
        private Infragistics.Win.Misc.UltraButton btnClose;
        private string strData;
        private byte[] sigData = null;
        private string sSigType;
        private GroupBox groupBox1;
        private PictureBox picSignature;
        public Label lblSigCaption;
        private Infragistics.Win.Misc.UltraButton btnApprove;
        private bool isSignatureRejected = false; 
        private bool ShowApproveButton = false;
        public bool callByUI = false;
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;

        public frmVerifySignature(string strSignData, string strSigType)
        {
            
            InitializeComponent();
            strData = strSignData;
            sSigType = strSigType;
            lblSigCaption.Text = "";
            btnApprove.Visible = true;
            btnApprove.Enabled = true;
           
        }

        public frmVerifySignature(byte[] strSignData, string strSigType)
        {

            InitializeComponent();
            sigData = strSignData;
            sSigType = strSigType;
            lblSigCaption.Text = "";
            btnApprove.Visible = true;
            btnApprove.Enabled = true;
           
           
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
            this.btnClose.Location = new System.Drawing.Point(424, 398);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(123, 28);
            this.btnClose.TabIndex = 1;
            this.btnClose.Text = "&Rejected";
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
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(535, 380);
            this.groupBox1.TabIndex = 8;
            this.groupBox1.TabStop = false;
            // 
            // lblSigCaption
            // 
            this.lblSigCaption.AutoSize = true;
            this.lblSigCaption.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSigCaption.Location = new System.Drawing.Point(19, 16);
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
            this.picSignature.Location = new System.Drawing.Point(24, 84);
            this.picSignature.Name = "picSignature";
            this.picSignature.Size = new System.Drawing.Size(487, 275);
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
            this.btnApprove.Location = new System.Drawing.Point(14, 398);
            this.btnApprove.Name = "btnApprove";
            this.btnApprove.Size = new System.Drawing.Size(123, 28);
            this.btnApprove.TabIndex = 0;
            this.btnApprove.Text = "&Accept";
            this.btnApprove.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnApprove.Visible = false;
            this.btnApprove.Click += new System.EventHandler(this.btnApprove_Click);
            // 
            // frmVerifySignature
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(559, 438);
            this.Controls.Add(this.btnApprove);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnClose);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmVerifySignature";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Verify Signature";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.frmVerifySignature_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picSignature)).EndInit();
            this.ResumeLayout(false);

        }
        #endregion

        private void frmVerifySignature_Load(object sender, System.EventArgs e)
        {
            clsUIHelper.setColorSchecme(this);

            string message = "Hmm.. Looks like PrimePOS was unable to validate the signature" + Environment.NewLine + "Please click Accept  if the following signature is valid";
            SetMsgDetails(message);

            if (this.sSigType == clsPOSDBConstants.STRINGIMAGE)
            {
                this.picSignature.Image = clsUIHelper.GetSignature(this.strData, sSigType);
            }            
            else
            {
                if (Configuration.CPOSSet.SigPadHostAddr.ToUpper().Trim().Contains("EVERTEC"))
                {
                    //byte[] imageData = Encoding.Default.GetBytes(sigData);
                    MemoryStream ms = new MemoryStream(sigData);
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
                else
                {
                    Image img;
                    using (MemoryStream ms = new MemoryStream(sigData))
                    {
                        img = Image.FromStream(ms);
                    }
                    this.picSignature.Image = img;
                }
            }
           
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
           
                isSignatureRejected = true; //Setting the value  to true

           
            this.Close();

        }
        
     

        private void btnApprove_Click(object sender, EventArgs e)
        {
            isSignatureRejected = false; 
            this.Close();
        }
        public bool IsSignatureRejected
        {
            get { return isSignatureRejected; }            
        }
        
    }
}
