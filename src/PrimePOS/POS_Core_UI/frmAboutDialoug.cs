using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using POS_Core.Resources;
//using POS_Core.DataAccess;

namespace POS_Core_UI
{
	/// <summary>
	/// Summary description for frmAboutDialoug.
	/// </summary>
	public class frmAboutDialoug : System.Windows.Forms.Form
    {
		private Infragistics.Win.UltraWinEditors.UltraPictureBox ultraPictureBox2;
		private Infragistics.Win.Misc.UltraLabel lblCompanyName;
        private Infragistics.Win.Misc.UltraLabel lblProduct;
        private Infragistics.Win.Misc.UltraButton btnClose;
		private Infragistics.Win.Misc.UltraLabel ultraLabel2;
		private System.Windows.Forms.LinkLabel lnkMMS;
        private GroupBox groupBox2;
        private PictureBox pictureBox1;
        private Infragistics.Win.Misc.UltraLabel lblPrimePOS;
        private GroupBox groupBox3;
        private Infragistics.Win.Misc.UltraLabel ultraLabel4;
        private Infragistics.Win.Misc.UltraLabel ultraLabel3;
        private Infragistics.Win.Misc.UltraLabel lblPharmacy;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public frmAboutDialoug()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

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
		private void InitializeComponent()
		{
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmAboutDialoug));
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            this.ultraPictureBox2 = new Infragistics.Win.UltraWinEditors.UltraPictureBox();
            this.lblCompanyName = new Infragistics.Win.Misc.UltraLabel();
            this.lblProduct = new Infragistics.Win.Misc.UltraLabel();
            this.btnClose = new Infragistics.Win.Misc.UltraButton();
            this.ultraLabel2 = new Infragistics.Win.Misc.UltraLabel();
            this.lnkMMS = new System.Windows.Forms.LinkLabel();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.lblPrimePOS = new Infragistics.Win.Misc.UltraLabel();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.ultraLabel4 = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabel3 = new Infragistics.Win.Misc.UltraLabel();
            this.lblPharmacy = new Infragistics.Win.Misc.UltraLabel();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // ultraPictureBox2
            // 
            this.ultraPictureBox2.BorderShadowColor = System.Drawing.Color.Empty;
            this.ultraPictureBox2.Image = ((object)(resources.GetObject("ultraPictureBox2.Image")));
            this.ultraPictureBox2.Location = new System.Drawing.Point(22, 82);
            this.ultraPictureBox2.Name = "ultraPictureBox2";
            this.ultraPictureBox2.Size = new System.Drawing.Size(36, 34);
            this.ultraPictureBox2.TabIndex = 1;
            // 
            // lblCompanyName
            // 
            this.lblCompanyName.Location = new System.Drawing.Point(92, 106);
            this.lblCompanyName.Name = "lblCompanyName";
            this.lblCompanyName.Size = new System.Drawing.Size(232, 16);
            this.lblCompanyName.TabIndex = 2;
            this.lblCompanyName.Text = "Company Name";
            // 
            // lblProduct
            // 
            this.lblProduct.Location = new System.Drawing.Point(92, 82);
            this.lblProduct.Name = "lblProduct";
            this.lblProduct.Size = new System.Drawing.Size(232, 16);
            this.lblProduct.TabIndex = 3;
            this.lblProduct.Text = "Version";
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            appearance1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(62)))), ((int)(((byte)(176)))));
            appearance1.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance1.FontData.BoldAsString = "True";
            appearance1.ForeColor = System.Drawing.Color.White;
            appearance1.Image = ((object)(resources.GetObject("appearance1.Image")));
            this.btnClose.Appearance = appearance1;
            this.btnClose.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(284, 288);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(100, 26);
            this.btnClose.TabIndex = 7;
            this.btnClose.Text = "&Ok";
            this.btnClose.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // ultraLabel2
            // 
            this.ultraLabel2.Location = new System.Drawing.Point(92, 129);
            this.ultraLabel2.Name = "ultraLabel2";
            this.ultraLabel2.Size = new System.Drawing.Size(291, 28);
            this.ultraLabel2.TabIndex = 9;
            this.ultraLabel2.Text = "6800 Jericho Turnpike, Suite 203E, Syosset, NY - 11791 Phone # (516)408-3999 , Fa" +
    "x # (516)408-3992  \r\n";
            // 
            // lnkMMS
            // 
            this.lnkMMS.Location = new System.Drawing.Point(92, 164);
            this.lnkMMS.Name = "lnkMMS";
            this.lnkMMS.Size = new System.Drawing.Size(232, 15);
            this.lnkMMS.TabIndex = 10;
            this.lnkMMS.TabStop = true;
            this.lnkMMS.Text = "http://www.micromerchantsys.com";
            this.lnkMMS.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkMMS_LinkClicked);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.pictureBox1);
            this.groupBox2.Controls.Add(this.lblPrimePOS);
            this.groupBox2.Location = new System.Drawing.Point(23, 0);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(0);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(363, 77);
            this.groupBox2.TabIndex = 43;
            this.groupBox2.TabStop = false;
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.White;
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(3, 9);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(128, 64);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 41;
            this.pictureBox1.TabStop = false;
            // 
            // lblPrimePOS
            // 
            appearance2.BackColor = System.Drawing.Color.White;
            appearance2.BackColorAlpha = Infragistics.Win.Alpha.Opaque;
            appearance2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            appearance2.TextHAlignAsString = "Center";
            appearance2.TextVAlignAsString = "Middle";
            this.lblPrimePOS.Appearance = appearance2;
            this.lblPrimePOS.BackColorInternal = System.Drawing.Color.Transparent;
            this.lblPrimePOS.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPrimePOS.ForeColor = System.Drawing.Color.White;
            this.lblPrimePOS.Location = new System.Drawing.Point(131, 9);
            this.lblPrimePOS.Name = "lblPrimePOS";
            this.lblPrimePOS.Size = new System.Drawing.Size(229, 64);
            this.lblPrimePOS.TabIndex = 39;
            this.lblPrimePOS.Text = "PrimePOS";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.ultraLabel4);
            this.groupBox3.Controls.Add(this.ultraLabel3);
            this.groupBox3.Controls.Add(this.lblPharmacy);
            this.groupBox3.Location = new System.Drawing.Point(23, 180);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(363, 92);
            this.groupBox3.TabIndex = 44;
            this.groupBox3.TabStop = false;
            // 
            // ultraLabel4
            // 
            appearance3.ForeColor = System.Drawing.Color.Black;
            appearance3.TextHAlignAsString = "Center";
            this.ultraLabel4.Appearance = appearance3;
            this.ultraLabel4.BackColorInternal = System.Drawing.Color.Transparent;
            this.ultraLabel4.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultraLabel4.ForeColor = System.Drawing.Color.White;
            this.ultraLabel4.Location = new System.Drawing.Point(12, 65);
            this.ultraLabel4.Name = "ultraLabel4";
            this.ultraLabel4.Size = new System.Drawing.Size(336, 17);
            this.ultraLabel4.TabIndex = 41;
            this.ultraLabel4.Text = "All rights reserved.";
            // 
            // ultraLabel3
            // 
            appearance4.ForeColor = System.Drawing.Color.Black;
            appearance4.TextHAlignAsString = "Center";
            this.ultraLabel3.Appearance = appearance4;
            this.ultraLabel3.BackColorInternal = System.Drawing.Color.Transparent;
            this.ultraLabel3.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultraLabel3.ForeColor = System.Drawing.Color.White;
            this.ultraLabel3.Location = new System.Drawing.Point(12, 42);
            this.ultraLabel3.Name = "ultraLabel3";
            this.ultraLabel3.Size = new System.Drawing.Size(336, 17);
            this.ultraLabel3.TabIndex = 40;
            this.ultraLabel3.Text = "(c)Copyrights 2004-2022 Micro Merchant Systems, Inc.";
            // 
            // lblPharmacy
            // 
            appearance5.ForeColor = System.Drawing.Color.Black;
            appearance5.TextHAlignAsString = "Center";
            this.lblPharmacy.Appearance = appearance5;
            this.lblPharmacy.BackColorInternal = System.Drawing.Color.Transparent;
            this.lblPharmacy.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPharmacy.ForeColor = System.Drawing.Color.White;
            this.lblPharmacy.Location = new System.Drawing.Point(12, 15);
            this.lblPharmacy.Name = "lblPharmacy";
            this.lblPharmacy.Size = new System.Drawing.Size(336, 21);
            this.lblPharmacy.TabIndex = 39;
            // 
            // frmAboutDialoug
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(409, 331);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.lnkMMS);
            this.Controls.Add(this.ultraLabel2);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.lblProduct);
            this.Controls.Add(this.lblCompanyName);
            this.Controls.Add(this.ultraPictureBox2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmAboutDialoug";
            this.Load += new System.EventHandler(this.frmAboutDialoug_Load);
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.groupBox3.ResumeLayout(false);
            this.ResumeLayout(false);

		}
		#endregion

		private void frmAboutDialoug_Load(object sender, System.EventArgs e)
		{
			this.Location=new Point((frmMain.getInstance().Width-this.Width)/2,(frmMain.getInstance().Height-this.Height)/2);
			clsUIHelper.setColorSchecme(this);
			this.lblCompanyName.Text=Application.CompanyName;
            this.lblPharmacy.Text = "Licensed To : " + Configuration.CInfo.StoreName;
			this.lblProduct.Text="Product: " + Application.ProductName + "  " + Application.ProductVersion;
            this.lblPrimePOS.Appearance.BackColor = Color.White;
            this.lblPrimePOS.Appearance.ForeColor = Color.FromArgb(34, 116, 155);
		}

		private void btnClose_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}

		private void lnkMMS_LinkClicked(object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e)
		{
			System.Diagnostics.Process.Start("Http://www.MicroMerchantSystems.com");
		}
	}
}
