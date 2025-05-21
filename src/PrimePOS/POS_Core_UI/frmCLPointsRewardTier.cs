using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using POS_Core.CommonData.Rows;
using POS_Core.CommonData;
using POS_Core.BusinessRules;
using POS_Core.ErrorLogging;
using Infragistics.Win.UltraWinEditors;
using POS_Core.Resources;
//using POS_Core.DataAccess;

namespace POS_Core_UI
{
	/// <summary>
	/// Summary description for frmCLPointsRewardTier.
	/// </summary>
	public class frmCLPointsRewardTier : System.Windows.Forms.Form
	{
		public bool IsCanceled = true;
		private CLPointsRewardTierData oCLPointsRewardTierData = new CLPointsRewardTierData();
		private CLPointsRewardTierRow oCLPointsRewardTierRow ;
		private CLPointsRewardTier oBRCLPointsRewardTier = new CLPointsRewardTier();
        private Infragistics.Win.Misc.UltraLabel ultraLabel21;
		private Infragistics.Win.Misc.UltraLabel ultraLabel11;
		private Infragistics.Win.Misc.UltraLabel ultraLabel14;
        private Infragistics.Win.Misc.UltraLabel lblTransactionType;
		private Infragistics.Win.UltraWinEditors.UltraTextEditor txtDescription;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor txtID;
		private Infragistics.Win.Misc.UltraButton btnClose;
		private Infragistics.Win.Misc.UltraButton btnSave;
		private Infragistics.Win.UltraWinEditors.UltraNumericEditor txtPoints;
        private Infragistics.Win.UltraWinEditors.UltraNumericEditor txtDiscount;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.ToolTip toolTip1;
		private Infragistics.Win.Misc.UltraLabel ultraLabel7;
        private Infragistics.Win.Misc.UltraLabel ultraLabel1;
        private UltraNumericEditor txtRewardDays;
        private Infragistics.Win.Misc.UltraLabel ultraLabel2;
        private Infragistics.Win.Misc.UltraLabel lblDiscount;
		private System.ComponentModel.IContainer components;
        private bool isLoading;

		public void Initialize()
		{
			SetNew();
		}

		public frmCLPointsRewardTier()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
			try
			{
				Initialize();
			}
			catch(Exception exp)
			{
				clsUIHelper.ShowErrorMsg(exp.Message);
			}

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
            this.components = new System.ComponentModel.Container();
            Infragistics.Win.Appearance appearance24 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance7 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance8 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance25 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance14 = new Infragistics.Win.Appearance();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmCLPointsRewardTier));
            Infragistics.Win.Appearance appearance15 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance16 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance17 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance18 = new Infragistics.Win.Appearance();
            this.ultraLabel21 = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabel11 = new Infragistics.Win.Misc.UltraLabel();
            this.txtDescription = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.ultraLabel14 = new Infragistics.Win.Misc.UltraLabel();
            this.txtID = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.txtPoints = new Infragistics.Win.UltraWinEditors.UltraNumericEditor();
            this.txtDiscount = new Infragistics.Win.UltraWinEditors.UltraNumericEditor();
            this.btnClose = new Infragistics.Win.Misc.UltraButton();
            this.btnSave = new Infragistics.Win.Misc.UltraButton();
            this.lblTransactionType = new Infragistics.Win.Misc.UltraLabel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lblDiscount = new Infragistics.Win.Misc.UltraLabel();
            this.txtRewardDays = new Infragistics.Win.UltraWinEditors.UltraNumericEditor();
            this.ultraLabel2 = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabel1 = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabel7 = new Infragistics.Win.Misc.UltraLabel();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.txtDescription)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtID)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPoints)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDiscount)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtRewardDays)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // ultraLabel21
            // 
            appearance24.FontData.BoldAsString = "False";
            appearance24.ForeColor = System.Drawing.Color.White;
            this.ultraLabel21.Appearance = appearance24;
            this.ultraLabel21.Location = new System.Drawing.Point(6, 79);
            this.ultraLabel21.Name = "ultraLabel21";
            this.ultraLabel21.Size = new System.Drawing.Size(114, 21);
            this.ultraLabel21.TabIndex = 13;
            this.ultraLabel21.Text = "Points";
            // 
            // ultraLabel11
            // 
            appearance7.ForeColor = System.Drawing.Color.White;
            this.ultraLabel11.Appearance = appearance7;
            this.ultraLabel11.Location = new System.Drawing.Point(6, 50);
            this.ultraLabel11.Name = "ultraLabel11";
            this.ultraLabel11.Size = new System.Drawing.Size(84, 21);
            this.ultraLabel11.TabIndex = 3;
            this.ultraLabel11.Text = "Description";
            // 
            // txtDescription
            // 
            this.txtDescription.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.txtDescription.Location = new System.Drawing.Point(138, 48);
            this.txtDescription.MaxLength = 50;
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.Size = new System.Drawing.Size(434, 23);
            this.txtDescription.TabIndex = 1;
            this.toolTip1.SetToolTip(this.txtDescription, "This is the text to describe the Tier.");
            this.txtDescription.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.txtDescription.Validated += new System.EventHandler(this.txtBoxs_Validate);
            // 
            // ultraLabel14
            // 
            appearance8.ForeColor = System.Drawing.Color.White;
            this.ultraLabel14.Appearance = appearance8;
            this.ultraLabel14.Location = new System.Drawing.Point(6, 20);
            this.ultraLabel14.Name = "ultraLabel14";
            this.ultraLabel14.Size = new System.Drawing.Size(82, 21);
            this.ultraLabel14.TabIndex = 1;
            this.ultraLabel14.Text = "ID";
            // 
            // txtID
            // 
            this.txtID.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.txtID.Enabled = false;
            this.txtID.Location = new System.Drawing.Point(138, 19);
            this.txtID.MaxLength = 20;
            this.txtID.Name = "txtID";
            this.txtID.Size = new System.Drawing.Size(148, 23);
            this.txtID.TabIndex = 0;
            this.toolTip1.SetToolTip(this.txtID, "Auto Generated ID");
            this.txtID.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.txtID.Validated += new System.EventHandler(this.txtBoxs_Validate);
            // 
            // txtPoints
            // 
            appearance25.FontData.BoldAsString = "False";
            this.txtPoints.Appearance = appearance25;
            this.txtPoints.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.txtPoints.ButtonStyle = Infragistics.Win.UIElementButtonStyle.VisualStudio2005Button;
            this.txtPoints.FormatString = "#######";
            this.txtPoints.Location = new System.Drawing.Point(138, 77);
            this.txtPoints.MaskClipMode = Infragistics.Win.UltraWinMaskedEdit.MaskMode.Raw;
            this.txtPoints.MaskDisplayMode = Infragistics.Win.UltraWinMaskedEdit.MaskMode.Raw;
            this.txtPoints.MaskInput = "nnnnnnn";
            this.txtPoints.MaxValue = 99999;
            this.txtPoints.MinValue = -1;
            this.txtPoints.Name = "txtPoints";
            this.txtPoints.NullText = "0";
            this.txtPoints.NumericType = Infragistics.Win.UltraWinEditors.NumericType.Double;
            this.txtPoints.Size = new System.Drawing.Size(148, 23);
            this.txtPoints.SpinButtonDisplayStyle = Infragistics.Win.ButtonDisplayStyle.Always;
            this.txtPoints.TabIndex = 2;
            this.txtPoints.TabNavigation = Infragistics.Win.UltraWinMaskedEdit.MaskedEditTabNavigation.NextControl;
            this.toolTip1.SetToolTip(this.txtPoints, "Points defines number of loyalty points when coupon will be generated.");
            this.txtPoints.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.txtPoints.Validated += new System.EventHandler(this.txtNumericBoxs_Validate);
            this.txtPoints.Enter += new System.EventHandler(this.txtPoints_Enter);
            // 
            // txtDiscount
            // 
            this.txtDiscount.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.txtDiscount.ButtonStyle = Infragistics.Win.UIElementButtonStyle.VisualStudio2005Button;
            this.txtDiscount.FormatString = "#####.##";
            this.txtDiscount.Location = new System.Drawing.Point(138, 106);
            this.txtDiscount.MaskClipMode = Infragistics.Win.UltraWinMaskedEdit.MaskMode.Raw;
            this.txtDiscount.MaskDisplayMode = Infragistics.Win.UltraWinMaskedEdit.MaskMode.Raw;
            this.txtDiscount.MaskInput = "nnnnn.nn";
            this.txtDiscount.MaxValue = 10000;
            this.txtDiscount.MinValue = 0;
            this.txtDiscount.Name = "txtDiscount";
            this.txtDiscount.NullText = "0";
            this.txtDiscount.NumericType = Infragistics.Win.UltraWinEditors.NumericType.Double;
            this.txtDiscount.Size = new System.Drawing.Size(148, 23);
            this.txtDiscount.SpinButtonDisplayStyle = Infragistics.Win.ButtonDisplayStyle.Always;
            this.txtDiscount.TabIndex = 4;
            this.txtDiscount.TabNavigation = Infragistics.Win.UltraWinMaskedEdit.MaskedEditTabNavigation.NextControl;
            this.txtDiscount.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.txtDiscount.Validated += new System.EventHandler(this.txtNumericBoxs_Validate);
            this.txtDiscount.Enter += new System.EventHandler(this.txtDiscount_Enter);
            // 
            // btnClose
            // 
            appearance14.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(62)))), ((int)(((byte)(176)))));
            appearance14.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance14.BorderAlpha = Infragistics.Win.Alpha.Opaque;
            appearance14.BorderColor = System.Drawing.Color.Black;
            appearance14.BorderColor3DBase = System.Drawing.Color.Black;
            appearance14.FontData.BoldAsString = "True";
            appearance14.ForeColor = System.Drawing.Color.White;
            appearance14.Image = ((object)(resources.GetObject("appearance14.Image")));
            this.btnClose.Appearance = appearance14;
            this.btnClose.ButtonStyle = Infragistics.Win.UIElementButtonStyle.OfficeXPToolbarButton;
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(479, 20);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(85, 26);
            this.btnClose.TabIndex = 18;
            this.btnClose.Text = "&Cancel";
            this.btnClose.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.btnClose.UseHotTracking = Infragistics.Win.DefaultableBoolean.True;
            this.btnClose.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnSave
            // 
            appearance15.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(62)))), ((int)(((byte)(176)))));
            appearance15.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance15.FontData.BoldAsString = "True";
            appearance15.ForeColor = System.Drawing.Color.White;
            appearance15.Image = ((object)(resources.GetObject("appearance15.Image")));
            this.btnSave.Appearance = appearance15;
            this.btnSave.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Flat;
            this.btnSave.Location = new System.Drawing.Point(385, 20);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(85, 26);
            this.btnSave.TabIndex = 17;
            this.btnSave.Text = "&Ok";
            this.btnSave.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.btnSave.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // lblTransactionType
            // 
            appearance16.ForeColor = System.Drawing.Color.White;
            appearance16.ForeColorDisabled = System.Drawing.Color.White;
            appearance16.TextHAlignAsString = "Center";
            this.lblTransactionType.Appearance = appearance16;
            this.lblTransactionType.BackColorInternal = System.Drawing.Color.Transparent;
            this.lblTransactionType.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTransactionType.Location = new System.Drawing.Point(16, 4);
            this.lblTransactionType.Name = "lblTransactionType";
            this.lblTransactionType.Size = new System.Drawing.Size(524, 50);
            this.lblTransactionType.TabIndex = 23;
            this.lblTransactionType.Tag = "Header";
            this.lblTransactionType.Text = "Points Reward Tier Information";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lblDiscount);
            this.groupBox1.Controls.Add(this.txtRewardDays);
            this.groupBox1.Controls.Add(this.ultraLabel2);
            this.groupBox1.Controls.Add(this.txtDiscount);
            this.groupBox1.Controls.Add(this.ultraLabel1);
            this.groupBox1.Controls.Add(this.ultraLabel7);
            this.groupBox1.Controls.Add(this.ultraLabel11);
            this.groupBox1.Controls.Add(this.ultraLabel14);
            this.groupBox1.Controls.Add(this.ultraLabel21);
            this.groupBox1.Controls.Add(this.txtPoints);
            this.groupBox1.Controls.Add(this.txtID);
            this.groupBox1.Controls.Add(this.txtDescription);
            this.groupBox1.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.groupBox1.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(14, 62);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(579, 184);
            this.groupBox1.TabIndex = 24;
            this.groupBox1.TabStop = false;
            // 
            // lblDiscount
            // 
            appearance1.ForeColor = System.Drawing.Color.White;
            this.lblDiscount.Appearance = appearance1;
            this.lblDiscount.Location = new System.Drawing.Point(6, 108);
            this.lblDiscount.Name = "lblDiscount";
            this.lblDiscount.Size = new System.Drawing.Size(108, 18);
            this.lblDiscount.TabIndex = 64;
            this.lblDiscount.Text = "Discount Value";
            this.lblDiscount.WrapText = false;
            // 
            // txtRewardDays
            // 
            this.txtRewardDays.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.txtRewardDays.ButtonStyle = Infragistics.Win.UIElementButtonStyle.VisualStudio2005Button;
            this.txtRewardDays.FormatString = "#####";
            this.txtRewardDays.Location = new System.Drawing.Point(138, 135);
            this.txtRewardDays.MaskClipMode = Infragistics.Win.UltraWinMaskedEdit.MaskMode.Raw;
            this.txtRewardDays.MaskDisplayMode = Infragistics.Win.UltraWinMaskedEdit.MaskMode.Raw;
            this.txtRewardDays.MaskInput = "nnnnn";
            this.txtRewardDays.MaxValue = 10000;
            this.txtRewardDays.MinValue = -1;
            this.txtRewardDays.Name = "txtRewardDays";
            this.txtRewardDays.NullText = "0";
            this.txtRewardDays.Size = new System.Drawing.Size(148, 23);
            this.txtRewardDays.SpinButtonDisplayStyle = Infragistics.Win.ButtonDisplayStyle.Always;
            this.txtRewardDays.TabIndex = 5;
            this.txtRewardDays.TabNavigation = Infragistics.Win.UltraWinMaskedEdit.MaskedEditTabNavigation.NextControl;
            this.toolTip1.SetToolTip(this.txtRewardDays, "Reward expiry days");
            this.txtRewardDays.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.txtRewardDays.ValueChanged += new System.EventHandler(this.txtNumericBoxs_Validate);
            // 
            // ultraLabel2
            // 
            appearance5.ForeColor = System.Drawing.Color.White;
            this.ultraLabel2.Appearance = appearance5;
            this.ultraLabel2.Location = new System.Drawing.Point(6, 139);
            this.ultraLabel2.Name = "ultraLabel2";
            this.ultraLabel2.Size = new System.Drawing.Size(121, 18);
            this.ultraLabel2.TabIndex = 40;
            this.ultraLabel2.Text = "Reward Days";
            this.ultraLabel2.WrapText = false;
            // 
            // ultraLabel1
            // 
            appearance17.ForeColor = System.Drawing.Color.White;
            appearance17.TextHAlignAsString = "Center";
            appearance17.TextVAlignAsString = "Middle";
            this.ultraLabel1.Appearance = appearance17;
            this.ultraLabel1.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultraLabel1.Location = new System.Drawing.Point(123, 53);
            this.ultraLabel1.Name = "ultraLabel1";
            this.ultraLabel1.Size = new System.Drawing.Size(11, 15);
            this.ultraLabel1.TabIndex = 38;
            this.ultraLabel1.Text = "*";
            // 
            // ultraLabel7
            // 
            appearance18.ForeColor = System.Drawing.Color.White;
            appearance18.TextHAlignAsString = "Center";
            appearance18.TextVAlignAsString = "Middle";
            this.ultraLabel7.Appearance = appearance18;
            this.ultraLabel7.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultraLabel7.Location = new System.Drawing.Point(123, 23);
            this.ultraLabel7.Name = "ultraLabel7";
            this.ultraLabel7.Size = new System.Drawing.Size(11, 15);
            this.ultraLabel7.TabIndex = 37;
            this.ultraLabel7.Text = "*";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnClose);
            this.groupBox2.Controls.Add(this.btnSave);
            this.groupBox2.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.groupBox2.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.Location = new System.Drawing.Point(14, 252);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(579, 59);
            this.groupBox2.TabIndex = 25;
            this.groupBox2.TabStop = false;
            // 
            // frmCLPointsRewardTier
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(108)))), ((int)(((byte)(172)))));
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(611, 334);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.lblTransactionType);
            this.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.Name = "frmCLPointsRewardTier";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Load += new System.EventHandler(this.frmCLPointsRewardTier_Load);
            this.Activated += new System.EventHandler(this.frmCLPointsRewardTier_Activated);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.frmCLPointsRewardTier_KeyUp);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmCLPointsRewardTier_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.txtDescription)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtID)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPoints)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDiscount)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtRewardDays)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);

		}
		#endregion
		private bool Save()
		{
			try
			{
                    oBRCLPointsRewardTier.Persist(oCLPointsRewardTierData);
                    return true;
			}
			catch(Exception exp)
			{
				clsUIHelper.ShowErrorMsg( exp.Message);
				return false;
			}
		}
		
        private void btnSave_Click(object sender, System.EventArgs e)
		{
			if (Save())
			{
				IsCanceled = false;
				this.Close();
			}
		}

		private void txtBoxs_Validate(object sender, System.EventArgs e)
		{
			try
			{
				if (oCLPointsRewardTierRow == null) 
					return ;
				Infragistics.Win.UltraWinEditors.UltraTextEditor txtEditor =  (Infragistics.Win.UltraWinEditors.UltraTextEditor)sender;
				switch(txtEditor.Name)
				{
					case "txtDescription":
						oCLPointsRewardTierRow.Description = txtDescription.Text;
						break;
				}
			}
			catch(Exception exp)
			{
				clsUIHelper.ShowErrorMsg(exp.Message);
			}

		}

		private void txtNumericBoxs_Validate(object sender, System.EventArgs e)
		{
			try
			{
				if (oCLPointsRewardTierRow == null) 
					return ;
				Infragistics.Win.UltraWinEditors.UltraNumericEditor txtEditor =  (Infragistics.Win.UltraWinEditors.UltraNumericEditor)sender;
				switch(txtEditor.Name)
				{
					case "txtPoints":
						oCLPointsRewardTierRow.Points= Configuration.convertNullToInt(this.txtPoints.Value.ToString());
						break;
					case "txtDiscount":
						oCLPointsRewardTierRow.Discount= Configuration.convertNullToDecimal(this.txtDiscount.Value.ToString());
						break;
                    case "txtRewardDays":
                        oCLPointsRewardTierRow.RewardPeriod = Configuration.convertNullToInt(this.txtRewardDays.Value.ToString());
                        break;
				}
			}
			catch(Exception exp)
			{
				clsUIHelper.ShowErrorMsg(exp.Message);
			}
		}

		private void Display()
		{
			txtID.Text = oCLPointsRewardTierRow.ID.ToString();
			txtDescription.Text = oCLPointsRewardTierRow.Description;
			txtPoints.Text = oCLPointsRewardTierRow.Points.ToString();
			txtDiscount.Text = oCLPointsRewardTierRow.Discount.ToString();
            txtRewardDays.Value = oCLPointsRewardTierRow.RewardPeriod;
        }

		public void Edit(int iID)
		{
            try
            {
                isLoading = true;
                txtID.Enabled = false;
                Clear();
                oCLPointsRewardTierData = oBRCLPointsRewardTier.Populate(iID);
                oCLPointsRewardTierRow = oCLPointsRewardTierData.CLPointsRewardTier.GetRowByID(iID);

                if (oCLPointsRewardTierRow != null)
                    Display();
            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
            finally
            {
                isLoading = false;
            }
		}

		private void SetNew()
		{
            isLoading = true;
			oBRCLPointsRewardTier = new CLPointsRewardTier();
			oCLPointsRewardTierData = new CLPointsRewardTierData();

			Clear();
			oCLPointsRewardTierRow = oCLPointsRewardTierData.CLPointsRewardTier.AddRow(0,"",0,0,0);
            isLoading = false;
		}

		private void Clear()
		{
			txtDescription.Text = "";
			txtPoints.Value = 0;
			txtDiscount.Value = 0;
            txtRewardDays.Value = 0;
			txtID.Text = "0";
            if (oCLPointsRewardTierData != null) oCLPointsRewardTierData.CLPointsRewardTier.Rows.Clear();
            SetupDiscountValueControl();
		}

		private void btnNew_Click(object sender, System.EventArgs e)
		{
			try
			{
				SetNew();
			}
			catch(Exception exp)
			{
				clsUIHelper.ShowErrorMsg(exp.Message);
			}
		}

		private void btnClose_Click(object sender, System.EventArgs e)
		{
			IsCanceled = true;
			this.Close();
		}

		private void txtDiscount_Enter(object sender, System.EventArgs e)
		{
			this.txtDiscount.SelectionStart=0;
			this.txtDiscount.SelectionLength=this.txtPoints.MaskInput.ToString().Length;
		}

		private void frmCLPointsRewardTier_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
		{
		}

		private void frmCLPointsRewardTier_Load(object sender, System.EventArgs e)
		{
			this.txtID.AfterEnterEditMode += new System.EventHandler(clsUIHelper.AfterEnterEditMode);			
			this.txtID.AfterExitEditMode += new System.EventHandler(clsUIHelper.AfterExitEditMode);			
			this.txtDescription.AfterEnterEditMode += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
			this.txtDescription.AfterExitEditMode += new System.EventHandler(clsUIHelper.AfterExitEditMode);
			this.txtPoints.AfterEnterEditMode += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
			this.txtPoints.AfterExitEditMode += new System.EventHandler(clsUIHelper.AfterExitEditMode);

			this.txtDiscount.AfterEnterEditMode += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
			this.txtDiscount.AfterExitEditMode += new System.EventHandler(clsUIHelper.AfterExitEditMode);

            this.txtRewardDays.AfterEnterEditMode += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
            this.txtRewardDays.AfterExitEditMode += new System.EventHandler(clsUIHelper.AfterExitEditMode);
            
			IsCanceled = true;
			clsUIHelper.setColorSchecme(this);
		}
		
        private void frmCLPointsRewardTier_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
		{
		
			try
			{
				if(e.KeyData==System.Windows.Forms.Keys.Enter)
				{
					this.SelectNextControl(this.ActiveControl,true,true,true,true);
				}
				
			}
			catch(Exception exp)
			{
				clsUIHelper.ShowErrorMsg(exp.Message);
			}	
		}

		private void frmCLPointsRewardTier_Activated(object sender, System.EventArgs e)
		{
			clsUIHelper.CurrentForm=this;
		}

        private void txtPoints_Enter(object sender, EventArgs e)
        {
            this.txtPoints.SelectionStart = 0;
            this.txtPoints.SelectionLength = this.txtPoints.MaskInput.ToString().Length;
        }

        private void SetupDiscountValueControl()
        {
            if (Configuration.CLoyaltyInfo.IsTierValueInPercent == true)
            {
                txtDiscount.FormatString = "###.##";
                txtDiscount.MaskInput = "nnn.nn";
                txtDiscount.MaxValue = 100;
                txtDiscount.Value = 0;
            }
            else
            {
                txtDiscount.FormatString = "#####.##";
                txtDiscount.MaskInput = "nnnnn.nn";
                txtDiscount.MaxValue = 10000;
            }
        }
	}
}
