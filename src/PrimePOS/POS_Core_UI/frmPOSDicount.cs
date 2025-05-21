using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
//using POS_Core.DataAccess;
using POS_Core.BusinessRules;
using System.Data;
using NLog;
using POS_Core.Resources;

namespace POS_Core_UI
{
	/// <summary>
	/// Summary description for frmPOSDicount.
	/// </summary>
	public class frmPOSDicount : System.Windows.Forms.Form
	{
		private Infragistics.Win.Misc.UltraLabel ultraLabel2;
		private Infragistics.Win.Misc.UltraButton btnClose;
		private Infragistics.Win.Misc.UltraButton btnSave;
		private Infragistics.Win.Misc.UltraLabel ultraLabel1;
		private Infragistics.Win.Misc.UltraLabel ultraLabel3;
		public Infragistics.Win.UltraWinEditors.UltraNumericEditor numDiscAmount;
		public Infragistics.Win.UltraWinEditors.UltraNumericEditor numDiscPerc;
        private Infragistics.Win.UltraWinStatusBar.UltraStatusBar stbAmount;
        decimal SellingPrice;
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;
        private static ILogger logger = LogManager.GetCurrentClassLogger();
        string DiscountOverrideUser = string.Empty; //PRIMEPOS-2979 19-Aug-2021 JY Added
        public string strMaxDiscountLimitOverrideUser = string.Empty; //PRIMEPOS-2979 19-Aug-2021 JY Added
        public bool bCalledFromLineItem = true; //PRIMEPOS-2979 19-Aug-2021 JY Added

        public frmPOSDicount()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
		}
        public frmPOSDicount(decimal pSellingPriice, string sUserID)    //PRIMEPOS-2979 19-Aug-2021 JY Added sUserID
        {
            SellingPrice = pSellingPriice;
            DiscountOverrideUser = sUserID; //PRIMEPOS-2979 19-Aug-2021 JY Added
            InitializeComponent();

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
            Infragistics.Win.Appearance appearance19 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance20 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance21 = new Infragistics.Win.Appearance();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmPOSDicount));
            Infragistics.Win.Appearance appearance22 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance23 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance24 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance25 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance26 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance27 = new Infragistics.Win.Appearance();
            this.numDiscPerc = new Infragistics.Win.UltraWinEditors.UltraNumericEditor();
            this.ultraLabel2 = new Infragistics.Win.Misc.UltraLabel();
            this.btnClose = new Infragistics.Win.Misc.UltraButton();
            this.btnSave = new Infragistics.Win.Misc.UltraButton();
            this.ultraLabel1 = new Infragistics.Win.Misc.UltraLabel();
            this.numDiscAmount = new Infragistics.Win.UltraWinEditors.UltraNumericEditor();
            this.ultraLabel3 = new Infragistics.Win.Misc.UltraLabel();
            this.stbAmount = new Infragistics.Win.UltraWinStatusBar.UltraStatusBar();
            ((System.ComponentModel.ISupportInitialize)(this.numDiscPerc)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numDiscAmount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.stbAmount)).BeginInit();
            this.SuspendLayout();
            // 
            // numDiscPerc
            // 
            appearance19.FontData.BoldAsString = "True";
            appearance19.FontData.SizeInPoints = 8F;
            this.numDiscPerc.Appearance = appearance19;
            this.numDiscPerc.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.numDiscPerc.Location = new System.Drawing.Point(275, 24);
            this.numDiscPerc.MaxValue = 100;
            this.numDiscPerc.MinValue = 0;
            this.numDiscPerc.Name = "numDiscPerc";
            this.numDiscPerc.NumericType = Infragistics.Win.UltraWinEditors.NumericType.Double;
            this.numDiscPerc.Size = new System.Drawing.Size(100, 19);
            this.numDiscPerc.TabIndex = 0;
            this.numDiscPerc.ValueChanged += new System.EventHandler(this.numDiscPerc_ValueChanged);
            // 
            // ultraLabel2
            // 
            appearance20.FontData.BoldAsString = "True";
            this.ultraLabel2.Appearance = appearance20;
            this.ultraLabel2.Location = new System.Drawing.Point(161, 29);
            this.ultraLabel2.Name = "ultraLabel2";
            this.ultraLabel2.Size = new System.Drawing.Size(100, 11);
            this.ultraLabel2.TabIndex = 3;
            this.ultraLabel2.Text = "Discount %";
            // 
            // btnClose
            // 
            appearance21.BorderAlpha = Infragistics.Win.Alpha.Opaque;
            appearance21.BorderColor = System.Drawing.Color.Black;
            appearance21.BorderColor3DBase = System.Drawing.Color.Black;
            appearance21.FontData.BoldAsString = "True";
            appearance21.Image = ((object)(resources.GetObject("appearance21.Image")));
            this.btnClose.Appearance = appearance21;
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(287, 109);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(85, 26);
            this.btnClose.TabIndex = 3;
            this.btnClose.Text = "&Cancel";
            this.btnClose.UseHotTracking = Infragistics.Win.DefaultableBoolean.True;
            this.btnClose.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnSave
            // 
            appearance22.FontData.BoldAsString = "True";
            appearance22.Image = ((object)(resources.GetObject("appearance22.Image")));
            this.btnSave.Appearance = appearance22;
            this.btnSave.Location = new System.Drawing.Point(195, 109);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(85, 26);
            this.btnSave.TabIndex = 2;
            this.btnSave.Text = "&Ok";
            this.btnSave.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // ultraLabel1
            // 
            appearance23.FontData.BoldAsString = "True";
            this.ultraLabel1.Appearance = appearance23;
            this.ultraLabel1.Location = new System.Drawing.Point(161, 80);
            this.ultraLabel1.Name = "ultraLabel1";
            this.ultraLabel1.Size = new System.Drawing.Size(100, 11);
            this.ultraLabel1.TabIndex = 22;
            this.ultraLabel1.Text = "Discount Amount";
            // 
            // numDiscAmount
            // 
            appearance24.FontData.BoldAsString = "True";
            appearance24.FontData.SizeInPoints = 8F;
            this.numDiscAmount.Appearance = appearance24;
            this.numDiscAmount.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.numDiscAmount.Location = new System.Drawing.Point(275, 75);
            this.numDiscAmount.MaxValue = 9999;
            this.numDiscAmount.MinValue = -9999;
            this.numDiscAmount.Name = "numDiscAmount";
            this.numDiscAmount.NumericType = Infragistics.Win.UltraWinEditors.NumericType.Double;
            this.numDiscAmount.Size = new System.Drawing.Size(100, 19);
            this.numDiscAmount.TabIndex = 1;
            this.numDiscAmount.ValueChanged += new System.EventHandler(this.numDiscAmount_ValueChanged);
            // 
            // ultraLabel3
            // 
            appearance25.FontData.BoldAsString = "True";
            this.ultraLabel3.Appearance = appearance25;
            this.ultraLabel3.Location = new System.Drawing.Point(273, 52);
            this.ultraLabel3.Name = "ultraLabel3";
            this.ultraLabel3.Size = new System.Drawing.Size(51, 11);
            this.ultraLabel3.TabIndex = 23;
            this.ultraLabel3.Text = "OR";
            // 
            // stbAmount
            // 
            appearance26.BackColor = System.Drawing.SystemColors.ControlLight;
            appearance26.BackColor2 = System.Drawing.Color.White;
            appearance26.BackGradientStyle = Infragistics.Win.GradientStyle.None;
            appearance26.BorderColor = System.Drawing.Color.Black;
            appearance26.BorderColor3DBase = System.Drawing.Color.Black;
            appearance26.ForeColor = System.Drawing.Color.Black;
            this.stbAmount.Appearance = appearance26;
            this.stbAmount.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.stbAmount.BorderStylePanel = Infragistics.Win.UIElementBorderStyle.Solid;
            this.stbAmount.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Flat;
            this.stbAmount.Dock = System.Windows.Forms.DockStyle.None;
            this.stbAmount.InterPanelSpacing = 0;
            this.stbAmount.Location = new System.Drawing.Point(2, 155);
            this.stbAmount.Name = "stbAmount";
            this.stbAmount.Padding = new Infragistics.Win.UltraWinStatusBar.UIElementMargins(-1, -1, -1, -1);
            appearance27.BackColor = System.Drawing.SystemColors.ControlLight;
            appearance27.BorderColor = System.Drawing.Color.Black;
            appearance27.BorderColor3DBase = System.Drawing.Color.Black;
            appearance27.FontData.BoldAsString = "True";
            appearance27.FontData.Name = "Arial";
            appearance27.FontData.SizeInPoints = 20F;
            appearance27.ImageHAlign = Infragistics.Win.HAlign.Center;
            appearance27.ImageVAlign = Infragistics.Win.VAlign.Middle;
            this.stbAmount.PanelAppearance = appearance27;
            this.stbAmount.ResizeStyle = Infragistics.Win.UltraWinStatusBar.ResizeStyle.None;
            this.stbAmount.ScaledImageSize = new System.Drawing.Size(24, 24);
            this.stbAmount.ScaleImages = Infragistics.Win.ScaleImage.Always;
            this.stbAmount.Size = new System.Drawing.Size(526, 87);
            this.stbAmount.SizeGripVisible = Infragistics.Win.DefaultableBoolean.False;
            this.stbAmount.TabIndex = 82;
            this.stbAmount.TabStop = true;
            this.stbAmount.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.stbAmount.ViewStyle = Infragistics.Win.UltraWinStatusBar.ViewStyle.Office2003;
            this.stbAmount.ButtonClick += new Infragistics.Win.UltraWinStatusBar.PanelEventHandler(this.stbAmount_ButtonClick_1);
            // 
            // frmPOSDicount
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(530, 247);
            this.Controls.Add(this.stbAmount);
            this.Controls.Add(this.ultraLabel3);
            this.Controls.Add(this.ultraLabel1);
            this.Controls.Add(this.numDiscAmount);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.ultraLabel2);
            this.Controls.Add(this.numDiscPerc);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Location = new System.Drawing.Point(230, 120);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmPOSDicount";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Discount";
            this.Load += new System.EventHandler(this.frmPOSDicount_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmPOSDicount_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.numDiscPerc)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numDiscAmount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.stbAmount)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion

        private void frmPOSDicount_Load(object sender, System.EventArgs e)
        {
            clsUIHelper.setColorSchecme(this);
            this.numDiscAmount.AfterEnterEditMode += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
            this.numDiscAmount.AfterExitEditMode += new System.EventHandler(clsUIHelper.AfterExitEditMode);

            this.numDiscPerc.AfterEnterEditMode += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
            this.numDiscPerc.AfterExitEditMode += new System.EventHandler(clsUIHelper.AfterExitEditMode);

            this.Location = new Point(70, 175);

            SetDiscountStrup();
        }

		private void btnClose_Click(object sender, System.EventArgs e)
		{
			this.DialogResult=DialogResult.Cancel;
            //frmDiscountOptions.CallDiscountFrm=false;
			this.Close();
		}

		private void frmPOSDicount_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
		{
            try
            {
                if (e.KeyData == System.Windows.Forms.Keys.Enter)
                {
                    this.SelectNextControl(this.ActiveControl, true, true, true, true);
                }
                else
                {
                    switch (e.KeyData)
                    {
                        case Keys.A:
                        case Keys.B:
                        case Keys.C:
                        case Keys.D:
                        case Keys.E:
                            GetDiscountPercent(Configuration.convertNullToString(e.KeyData));
                            decimal DiscToVerify = Configuration.convertNullToDecimal(this.numDiscPerc.Value);
                            decimal InvDicsValueToVerify; // NileshJ - Declare variable 
                            //if (DiscToVerify > 0 && POSTransaction.ValidateUserDicountLimit(DiscToVerify, out InvDicsValueToVerify, DiscountOverrideUser) == true) // NileshJ- Add out InvDicsValueToVerify
                            POS_Core_UI.UI.frmPOSTransaction ofrmPOSTransaction = new POS_Core_UI.UI.frmPOSTransaction();
                            if (bCalledFromLineItem == true && DiscToVerify > 0 && ofrmPOSTransaction.ValidateUserDicountLimit(DiscToVerify, out InvDicsValueToVerify, out strMaxDiscountLimitOverrideUser, DiscountOverrideUser) == true)
                            {
                                this.DialogResult = DialogResult.OK;
                                this.Close();
                            }
                            else if (DiscToVerify == 0)
                            {
                                this.DialogResult = DialogResult.OK;
                                this.Close();
                            }
                            break;
                    }

                }
                #region Commented code by shitaljit ot improve better flow of code
                
               
                //else if (e.KeyData == System.Windows.Forms.Keys.A)
                //{
                //    GetDiscountPercent("A");
                //    this.DialogResult = DialogResult.OK;
                //    this.Close();
                //}
                //else if (e.KeyData == System.Windows.Forms.Keys.B)
                //{
                //    GetDiscountPercent("B");
                //    this.DialogResult = DialogResult.OK;
                //    this.Close();
                //}
                //else if (e.KeyData == System.Windows.Forms.Keys.C)
                //{
                //    GetDiscountPercent("C");
                //    this.DialogResult = DialogResult.OK;
                //    this.Close();
                //}
                //else if (e.KeyData == System.Windows.Forms.Keys.D)
                //{
                //    GetDiscountPercent("D");
                //    this.DialogResult = DialogResult.OK;
                //    this.Close();
                //}
                //else if (e.KeyData == System.Windows.Forms.Keys.E)
                //{
                //    GetDiscountPercent("E");
                //    this.DialogResult = DialogResult.OK;
                //    this.Close();
                //}
                #endregion

            }
            catch (Exception exp)
            {
                logger.Fatal(exp, "frmPOSDicount_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)");
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
		}

        private void btnSave_Click(object sender, System.EventArgs e)
        {
            logger.Trace("btnSave_Click(object sender, System.EventArgs e)() - " + POS_Core.CommonData.clsPOSDBConstants.Log_Entering);
            System.Decimal DiscAmt = 0;
            if (frmDiscountOptions.CallDiscountFrm == true)
            {
                if (Convert.ToDecimal(numDiscPerc.Value) != 0)
                {
                    DiscAmt = Convert.ToDecimal((Convert.ToDecimal(numDiscPerc.Value) / 100 * SellingPrice));
                }
                else
                {
                    DiscAmt = Math.Round(Convert.ToDecimal(numDiscAmount.Value), 2);
                }
                if (DiscAmt > SellingPrice)
                {
                    clsUIHelper.ShowErrorMsg("Current Discount is more than Selling Price.\nPlease enter discount smaller than Selling Price");
                    numDiscPerc.Focus();
                }
                else
                {
                    this.DialogResult = DialogResult.OK;
                    frmDiscountOptions.CallDiscountFrm = false;
                    this.Close();
                }
            }
            else
            {
                if (Convert.ToDecimal(this.numDiscPerc.Value) != 0) //Invoice Discount given in Percentage
                {
                    DiscAmt = Convert.ToDecimal(this.numDiscPerc.Value);
                }
                else if (SellingPrice > 0) //Invoice Discount given in Dollar amount
                {
                    DiscAmt = ((Convert.ToDecimal(this.numDiscAmount.Value) / SellingPrice) * 100);
                }
                if (SellingPrice > 0 )
                {
                    decimal InvDicsValueToVerify; // NileshJ - Declare variable 
                    POS_Core_UI.UI.frmPOSTransaction ofrmPOSTransaction = new POS_Core_UI.UI.frmPOSTransaction();
                    if (ofrmPOSTransaction.ValidateUserDicountLimit(DiscAmt, out InvDicsValueToVerify, out strMaxDiscountLimitOverrideUser, DiscountOverrideUser) == true) // NileshJ- Add out InvDicsValueToVerify
                    {
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    }
                    else
                    {
                        if (Convert.ToDecimal(this.numDiscPerc.Value) != 0) //Invoice Discount given in Percentage
                        {
                            this.numDiscPerc.Focus();
                        }
                        else //Invoice Discount given in Dollar amount
                        {
                            this.numDiscAmount.Focus();
                        }
                    }
                }
                else if(SellingPrice ==0)
                {
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
            }
            logger.Trace("btnSave_Click(object sender, System.EventArgs e)() - " + POS_Core.CommonData.clsPOSDBConstants.Log_Exiting);
        }


		private void numDiscPerc_ValueChanged(object sender, System.EventArgs e)
		{
			if (Convert.ToDouble( this.numDiscPerc.Value)!=0)
			{
				this.numDiscAmount.Value=0;
			}
		}

		private void numDiscAmount_ValueChanged(object sender, System.EventArgs e)
		{
			if (Convert.ToDouble( this.numDiscAmount.Value)!=0)
			{
				this.numDiscPerc.Value=0;
			}
		}
       
        DataSet dtDiscountKey = new DataSet();
        private void SetDiscountStrup()        
        {            
            Prefrences pre = new Prefrences();
            dtDiscountKey = pre.PopulateGetDiscount(Configuration.StationID);
            DataRow drKey;
            drKey = dtDiscountKey.Tables[0].Rows[0];

            stbAmount.Panels.Add(Convert.ToString(drKey["KeyA"]) + "A", "Press A for (" + Convert.ToString(drKey["KeyA"])+"%)",Infragistics.Win.UltraWinStatusBar.PanelStyle.Button);
            stbAmount.Panels.Add(Convert.ToString(drKey["KeyB"]) + "B", "Press B for (" + Convert.ToString(drKey["KeyB"]) + "%)", Infragistics.Win.UltraWinStatusBar.PanelStyle.Button);
            stbAmount.Panels.Add(Convert.ToString(drKey["KeyC"]) + "C", "Press C for (" + Convert.ToString(drKey["KeyC"]) + "%)", Infragistics.Win.UltraWinStatusBar.PanelStyle.Button);
            stbAmount.Panels.Add(Convert.ToString(drKey["KeyD"]) + "D", "Press D for (" + Convert.ToString(drKey["KeyD"]) + "%)", Infragistics.Win.UltraWinStatusBar.PanelStyle.Button);
            stbAmount.Panels.Add(Convert.ToString(drKey["KeyE"]) + "E", "Press E for (" + Convert.ToString(drKey["KeyE"]) + "%)", Infragistics.Win.UltraWinStatusBar.PanelStyle.Button);
            
            stbAmount.Panels[0].Appearance.FontData.SizeInPoints = 16;
            stbAmount.Panels[1].Appearance.FontData.SizeInPoints = 16;
            stbAmount.Panels[2].Appearance.FontData.SizeInPoints = 16;
            stbAmount.Panels[3].Appearance.FontData.SizeInPoints = 16;
            stbAmount.Panels[4].Appearance.FontData.SizeInPoints = 16;
        }       

        private void stbAmount_ButtonClick_1(object sender, Infragistics.Win.UltraWinStatusBar.PanelEventArgs e)
        {
            numDiscPerc.Value = e.Panel.Key.ToString().Replace('A', ' ').Replace('B', ' ').Replace('C', ' ').Replace('D', ' ').Replace('E', ' ');
            decimal DiscToVerify = Configuration.convertNullToDecimal(this.numDiscPerc.Value);
            decimal InvDicsValueToVerify; // NileshJ - Declare variable 
            //if (DiscToVerify > 0 && POSTransaction.ValidateUserDicountLimit(DiscToVerify, out InvDicsValueToVerify, DiscountOverrideUser) == true) // NileshJ - Add out InvDicsValueToVerify
            POS_Core_UI.UI.frmPOSTransaction ofrmPOSTransaction = new POS_Core_UI.UI.frmPOSTransaction();
            if (bCalledFromLineItem == true && DiscToVerify > 0 && ofrmPOSTransaction.ValidateUserDicountLimit(DiscToVerify, out InvDicsValueToVerify, out strMaxDiscountLimitOverrideUser, DiscountOverrideUser) == true)
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else if (DiscToVerify == 0)
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private void GetDiscountPercent(string StrKey)
        {
            if (StrKey == "A")
            {
                numDiscPerc.Value = Convert.ToString(dtDiscountKey.Tables[0].Rows[0]["KeyA"]);                
            }else
            if (StrKey == "B")
            {
                numDiscPerc.Value = Convert.ToString(dtDiscountKey.Tables[0].Rows[0]["KeyB"]);             
            }else
            if (StrKey == "C")
            {
                numDiscPerc.Value = Convert.ToString(dtDiscountKey.Tables[0].Rows[0]["KeyC"]);                
            }else
            if (StrKey == "D")
            {
                numDiscPerc.Value = Convert.ToString(dtDiscountKey.Tables[0].Rows[0]["KeyD"]);             
            }else

            if (StrKey == "E")
            {
                numDiscPerc.Value = Convert.ToString(dtDiscountKey.Tables[0].Rows[0]["KeyE"]);
            }          
        }
	}
}
