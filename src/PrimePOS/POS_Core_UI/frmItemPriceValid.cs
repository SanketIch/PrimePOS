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

namespace POS_Core_UI
{
	/// <summary>
	/// Summary description for frmInvTransType.
	/// </summary>
	public class frmItemPriceValid : System.Windows.Forms.Form
	{
		public bool IsCanceled = true;
        private ItemPriceValidationData oItemPriceValidationData = new ItemPriceValidationData();
        private string sItemID = "";
        private int iDeptID = 0;
		private ItemPriceValidation oItemPriceValidation = new ItemPriceValidation();
		private Infragistics.Win.Misc.UltraLabel ultraLabel18;
		private Infragistics.Win.Misc.UltraLabel ultraLabel11;
		private Infragistics.Win.Misc.UltraLabel lblMinSellPriceAMt;
		private Infragistics.Win.Misc.UltraLabel lblTransactionType;
		private Infragistics.Win.Misc.UltraButton btnClose;
		private Infragistics.Win.Misc.UltraButton btnSave;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ToolTip toolTip1;
		private System.Windows.Forms.RadioButton optLessThanZeroYes;
		private System.Windows.Forms.RadioButton optLessThanZeroNo;
        private Infragistics.Win.Misc.UltraLabel ultraLabel1;
        private UltraNumericEditor numMinSellingPriceAmt;
        private UltraNumericEditor numMinSellingPricePerc;
        private UltraNumericEditor numMinCostPricePerc;
        private CheckBox chkIsActive;
        private Infragistics.Win.Misc.UltraLabel ultraLabel3;
        private Infragistics.Win.Misc.UltraLabel lblMinSellingPriceAmt;
		private System.ComponentModel.IContainer components;

		public frmItemPriceValid(string ItemId)
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
			sItemID = ItemId;
			//
			// TODO: Add any constructor code after InitializeComponent call
			//
		}

        public frmItemPriceValid(int DeptID)
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();
            iDeptID = DeptID;
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
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmItemPriceValid));
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance7 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance8 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance9 = new Infragistics.Win.Appearance();
            this.ultraLabel18 = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabel11 = new Infragistics.Win.Misc.UltraLabel();
            this.lblMinSellPriceAMt = new Infragistics.Win.Misc.UltraLabel();
            this.btnClose = new Infragistics.Win.Misc.UltraButton();
            this.btnSave = new Infragistics.Win.Misc.UltraButton();
            this.lblTransactionType = new Infragistics.Win.Misc.UltraLabel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.ultraLabel3 = new Infragistics.Win.Misc.UltraLabel();
            this.lblMinSellingPriceAmt = new Infragistics.Win.Misc.UltraLabel();
            this.chkIsActive = new System.Windows.Forms.CheckBox();
            this.numMinSellingPriceAmt = new Infragistics.Win.UltraWinEditors.UltraNumericEditor();
            this.numMinSellingPricePerc = new Infragistics.Win.UltraWinEditors.UltraNumericEditor();
            this.numMinCostPricePerc = new Infragistics.Win.UltraWinEditors.UltraNumericEditor();
            this.ultraLabel1 = new Infragistics.Win.Misc.UltraLabel();
            this.optLessThanZeroNo = new System.Windows.Forms.RadioButton();
            this.optLessThanZeroYes = new System.Windows.Forms.RadioButton();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numMinSellingPriceAmt)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numMinSellingPricePerc)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numMinCostPricePerc)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // ultraLabel18
            // 
            appearance1.TextHAlignAsString = "Right";
            this.ultraLabel18.Appearance = appearance1;
            this.ultraLabel18.Location = new System.Drawing.Point(8, 24);
            this.ultraLabel18.Name = "ultraLabel18";
            this.ultraLabel18.Size = new System.Drawing.Size(247, 18);
            this.ultraLabel18.TabIndex = 15;
            this.ultraLabel18.Text = "Allow Less Than Zero";
            this.ultraLabel18.WrapText = false;
            // 
            // ultraLabel11
            // 
            appearance2.TextHAlignAsString = "Right";
            this.ultraLabel11.Appearance = appearance2;
            this.ultraLabel11.Location = new System.Drawing.Point(8, 107);
            this.ultraLabel11.Name = "ultraLabel11";
            this.ultraLabel11.Size = new System.Drawing.Size(247, 18);
            this.ultraLabel11.TabIndex = 3;
            this.ultraLabel11.Text = "Maximum Percent off Selling Price";
            // 
            // lblMinSellPriceAMt
            // 
            appearance3.TextHAlignAsString = "Right";
            this.lblMinSellPriceAMt.Appearance = appearance3;
            this.lblMinSellPriceAMt.Location = new System.Drawing.Point(8, 65);
            this.lblMinSellPriceAMt.Name = "lblMinSellPriceAMt";
            this.lblMinSellPriceAMt.Size = new System.Drawing.Size(247, 18);
            this.lblMinSellPriceAMt.TabIndex = 1;
            this.lblMinSellPriceAMt.Text = "Minimum Selling Price Amount";
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            appearance4.FontData.BoldAsString = "True";
            appearance4.Image = ((object)(resources.GetObject("appearance4.Image")));
            this.btnClose.Appearance = appearance4;
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(488, 16);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(85, 35);
            this.btnClose.TabIndex = 7;
            this.btnClose.Text = "&Cancel";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            appearance5.FontData.BoldAsString = "True";
            appearance5.Image = ((object)(resources.GetObject("appearance5.Image")));
            this.btnSave.Appearance = appearance5;
            this.btnSave.Location = new System.Drawing.Point(396, 16);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(85, 35);
            this.btnSave.TabIndex = 6;
            this.btnSave.Text = "&Ok";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // lblTransactionType
            // 
            appearance6.BackColor = System.Drawing.Color.DeepSkyBlue;
            appearance6.BackColor2 = System.Drawing.Color.Azure;
            appearance6.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance6.ForeColor = System.Drawing.Color.Navy;
            appearance6.ForeColorDisabled = System.Drawing.Color.Navy;
            appearance6.TextHAlignAsString = "Center";
            this.lblTransactionType.Appearance = appearance6;
            this.lblTransactionType.BackColorInternal = System.Drawing.Color.Transparent;
            this.lblTransactionType.BorderStyleOuter = Infragistics.Win.UIElementBorderStyle.Solid;
            this.lblTransactionType.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblTransactionType.Font = new System.Drawing.Font("Verdana", 20.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTransactionType.Location = new System.Drawing.Point(0, 0);
            this.lblTransactionType.Name = "lblTransactionType";
            this.lblTransactionType.Size = new System.Drawing.Size(604, 35);
            this.lblTransactionType.TabIndex = 23;
            this.lblTransactionType.Tag = "Header";
            this.lblTransactionType.Text = "Minimum Item Price";
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.ultraLabel3);
            this.groupBox1.Controls.Add(this.lblMinSellingPriceAmt);
            this.groupBox1.Controls.Add(this.chkIsActive);
            this.groupBox1.Controls.Add(this.numMinSellingPriceAmt);
            this.groupBox1.Controls.Add(this.numMinSellingPricePerc);
            this.groupBox1.Controls.Add(this.numMinCostPricePerc);
            this.groupBox1.Controls.Add(this.ultraLabel1);
            this.groupBox1.Controls.Add(this.optLessThanZeroNo);
            this.groupBox1.Controls.Add(this.optLessThanZeroYes);
            this.groupBox1.Controls.Add(this.ultraLabel11);
            this.groupBox1.Controls.Add(this.ultraLabel18);
            this.groupBox1.Controls.Add(this.lblMinSellPriceAMt);
            this.groupBox1.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.groupBox1.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(10, 52);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(580, 205);
            this.groupBox1.TabIndex = 24;
            this.groupBox1.TabStop = false;
            // 
            // ultraLabel3
            // 
            appearance7.TextHAlignAsString = "Right";
            this.ultraLabel3.Appearance = appearance7;
            this.ultraLabel3.Location = new System.Drawing.Point(255, 128);
            this.ultraLabel3.Name = "ultraLabel3";
            this.ultraLabel3.Size = new System.Drawing.Size(28, 18);
            this.ultraLabel3.TabIndex = 20;
            this.ultraLabel3.Text = "Or";
            this.ultraLabel3.WrapText = false;
            // 
            // lblMinSellingPriceAmt
            // 
            appearance8.TextHAlignAsString = "Right";
            this.lblMinSellingPriceAmt.Appearance = appearance8;
            this.lblMinSellingPriceAmt.Location = new System.Drawing.Point(255, 86);
            this.lblMinSellingPriceAmt.Name = "lblMinSellingPriceAmt";
            this.lblMinSellingPriceAmt.Size = new System.Drawing.Size(28, 18);
            this.lblMinSellingPriceAmt.TabIndex = 19;
            this.lblMinSellingPriceAmt.Text = "Or";
            this.lblMinSellingPriceAmt.WrapText = false;
            // 
            // chkIsActive
            // 
            this.chkIsActive.BackColor = System.Drawing.Color.Transparent;
            this.chkIsActive.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkIsActive.Checked = true;
            this.chkIsActive.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkIsActive.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chkIsActive.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkIsActive.Location = new System.Drawing.Point(188, 176);
            this.chkIsActive.Name = "chkIsActive";
            this.chkIsActive.Size = new System.Drawing.Size(87, 19);
            this.chkIsActive.TabIndex = 5;
            this.chkIsActive.Text = "Is Active";
            this.chkIsActive.UseVisualStyleBackColor = false;
            // 
            // numMinSellingPriceAmt
            // 
            this.numMinSellingPriceAmt.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.numMinSellingPriceAmt.ButtonStyle = Infragistics.Win.UIElementButtonStyle.VisualStudio2005Button;
            this.numMinSellingPriceAmt.FormatString = "###.00";
            this.numMinSellingPriceAmt.Location = new System.Drawing.Point(261, 63);
            this.numMinSellingPriceAmt.MaskClipMode = Infragistics.Win.UltraWinMaskedEdit.MaskMode.Raw;
            this.numMinSellingPriceAmt.MaskDisplayMode = Infragistics.Win.UltraWinMaskedEdit.MaskMode.Raw;
            this.numMinSellingPriceAmt.MaskInput = "nnnnnn.nn";
            this.numMinSellingPriceAmt.MaxValue = 999999;
            this.numMinSellingPriceAmt.MinValue = -999999;
            this.numMinSellingPriceAmt.Name = "numMinSellingPriceAmt";
            this.numMinSellingPriceAmt.NullText = "0";
            this.numMinSellingPriceAmt.NumericType = Infragistics.Win.UltraWinEditors.NumericType.Double;
            this.numMinSellingPriceAmt.Size = new System.Drawing.Size(148, 23);
            this.numMinSellingPriceAmt.SpinButtonDisplayStyle = Infragistics.Win.ButtonDisplayStyle.Always;
            this.numMinSellingPriceAmt.TabIndex = 2;
            this.numMinSellingPriceAmt.TabNavigation = Infragistics.Win.UltraWinMaskedEdit.MaskedEditTabNavigation.NextControl;
            this.numMinSellingPriceAmt.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.numMinSellingPriceAmt.Enter += new System.EventHandler(this.numericBox_Enter);
            // 
            // numMinSellingPricePerc
            // 
            this.numMinSellingPricePerc.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.numMinSellingPricePerc.ButtonStyle = Infragistics.Win.UIElementButtonStyle.VisualStudio2005Button;
            this.numMinSellingPricePerc.FormatString = "###.00";
            this.numMinSellingPricePerc.Location = new System.Drawing.Point(261, 105);
            this.numMinSellingPricePerc.MaskClipMode = Infragistics.Win.UltraWinMaskedEdit.MaskMode.Raw;
            this.numMinSellingPricePerc.MaskDisplayMode = Infragistics.Win.UltraWinMaskedEdit.MaskMode.Raw;
            this.numMinSellingPricePerc.MaskInput = "nnn.nn";
            this.numMinSellingPricePerc.MaxValue = 100;
            this.numMinSellingPricePerc.MinValue = -1;
            this.numMinSellingPricePerc.Name = "numMinSellingPricePerc";
            this.numMinSellingPricePerc.NullText = "0";
            this.numMinSellingPricePerc.NumericType = Infragistics.Win.UltraWinEditors.NumericType.Double;
            this.numMinSellingPricePerc.Size = new System.Drawing.Size(148, 23);
            this.numMinSellingPricePerc.SpinButtonDisplayStyle = Infragistics.Win.ButtonDisplayStyle.Always;
            this.numMinSellingPricePerc.TabIndex = 3;
            this.numMinSellingPricePerc.TabNavigation = Infragistics.Win.UltraWinMaskedEdit.MaskedEditTabNavigation.NextControl;
            this.numMinSellingPricePerc.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.numMinSellingPricePerc.Enter += new System.EventHandler(this.numericBox_Enter);
            // 
            // numMinCostPricePerc
            // 
            this.numMinCostPricePerc.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.numMinCostPricePerc.ButtonStyle = Infragistics.Win.UIElementButtonStyle.VisualStudio2005Button;
            this.numMinCostPricePerc.FormatString = "###.00";
            this.numMinCostPricePerc.Location = new System.Drawing.Point(261, 147);
            this.numMinCostPricePerc.MaskClipMode = Infragistics.Win.UltraWinMaskedEdit.MaskMode.Raw;
            this.numMinCostPricePerc.MaskDisplayMode = Infragistics.Win.UltraWinMaskedEdit.MaskMode.Raw;
            this.numMinCostPricePerc.MaskInput = "nnn.nn";
            this.numMinCostPricePerc.MaxValue = 100;
            this.numMinCostPricePerc.MinValue = -1;
            this.numMinCostPricePerc.Name = "numMinCostPricePerc";
            this.numMinCostPricePerc.NullText = "0";
            this.numMinCostPricePerc.NumericType = Infragistics.Win.UltraWinEditors.NumericType.Double;
            this.numMinCostPricePerc.Size = new System.Drawing.Size(148, 23);
            this.numMinCostPricePerc.SpinButtonDisplayStyle = Infragistics.Win.ButtonDisplayStyle.Always;
            this.numMinCostPricePerc.TabIndex = 4;
            this.numMinCostPricePerc.TabNavigation = Infragistics.Win.UltraWinMaskedEdit.MaskedEditTabNavigation.NextControl;
            this.numMinCostPricePerc.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.numMinCostPricePerc.Enter += new System.EventHandler(this.numericBox_Enter);
            // 
            // ultraLabel1
            // 
            appearance9.TextHAlignAsString = "Right";
            this.ultraLabel1.Appearance = appearance9;
            this.ultraLabel1.Location = new System.Drawing.Point(8, 149);
            this.ultraLabel1.Name = "ultraLabel1";
            this.ultraLabel1.Size = new System.Drawing.Size(247, 18);
            this.ultraLabel1.TabIndex = 18;
            this.ultraLabel1.Text = "Minimum Margin on Cost(%)";
            // 
            // optLessThanZeroNo
            // 
            this.optLessThanZeroNo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.optLessThanZeroNo.Location = new System.Drawing.Point(327, 24);
            this.optLessThanZeroNo.Name = "optLessThanZeroNo";
            this.optLessThanZeroNo.Size = new System.Drawing.Size(62, 18);
            this.optLessThanZeroNo.TabIndex = 1;
            this.optLessThanZeroNo.Text = "No";
            // 
            // optLessThanZeroYes
            // 
            this.optLessThanZeroYes.Checked = true;
            this.optLessThanZeroYes.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.optLessThanZeroYes.Location = new System.Drawing.Point(261, 24);
            this.optLessThanZeroYes.Name = "optLessThanZeroYes";
            this.optLessThanZeroYes.Size = new System.Drawing.Size(60, 18);
            this.optLessThanZeroYes.TabIndex = 0;
            this.optLessThanZeroYes.TabStop = true;
            this.optLessThanZeroYes.Text = "Yes";
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.btnClose);
            this.groupBox2.Controls.Add(this.btnSave);
            this.groupBox2.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.groupBox2.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.Location = new System.Drawing.Point(10, 263);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(580, 59);
            this.groupBox2.TabIndex = 25;
            this.groupBox2.TabStop = false;
            // 
            // frmItemPriceValid
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(604, 334);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.lblTransactionType);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.Name = "frmItemPriceValid";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Tag = "";
            this.Text = "Minimum Item Price";
            this.Activated += new System.EventHandler(this.frmInvTransType_Activated);
            this.Load += new System.EventHandler(this.frmInvTransType_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmInvTransType_KeyDown);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numMinSellingPriceAmt)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numMinSellingPricePerc)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numMinCostPricePerc)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);

		}
		#endregion
		private bool Save()
		{
            bool retVal = false;
			try
			{

                if (oItemPriceValidationData.Tables[0].Rows.Count == 0)
                {
                    oItemPriceValidationData.ItemPriceValidation.AddRow(0, 0, "", false, 0, 0, false, 0);
                }
                oItemPriceValidationData.ItemPriceValidation[0].AllowNegative = this.optLessThanZeroYes.Checked;
                oItemPriceValidationData.ItemPriceValidation[0].IsActive = this.chkIsActive.Checked;
                oItemPriceValidationData.ItemPriceValidation[0].MinCostPercentage = POS_Core.Resources.Configuration.convertNullToDecimal(this.numMinCostPricePerc.Value.ToString());
                oItemPriceValidationData.ItemPriceValidation[0].MinSellingAmount = POS_Core.Resources.Configuration.convertNullToDecimal(this.numMinSellingPriceAmt.Value.ToString());
                oItemPriceValidationData.ItemPriceValidation[0].MinSellingPercentage = POS_Core.Resources.Configuration.convertNullToDecimal(this.numMinSellingPricePerc.Value.ToString());
                oItemPriceValidationData.ItemPriceValidation[0].ItemID = this.sItemID;
                oItemPriceValidationData.ItemPriceValidation[0].DeptID = this.iDeptID;

                if (this.sItemID != "")
                {
                    retVal=oItemPriceValidation.ValidateItem(this.sItemID, oItemPriceValidationData.ItemPriceValidation[0]);
                    if (retVal == false)
                    {
                        clsUIHelper.ShowErrorMsg("Current values in item conflict with validation settings.");
                    }
                    
                }
                else
                {
                   retVal= oItemPriceValidation.ValidateDept(this.iDeptID, oItemPriceValidationData.ItemPriceValidation[0]);
                   if (retVal == false)
                   {
                       clsUIHelper.ShowErrorMsg("One of the items in this department is invalid according to current validation settings.");
                   }
                }

                if (retVal == true)
                {
                    oItemPriceValidation.Persist(oItemPriceValidationData);
                    retVal=true;
                }
			}
			catch(Exception exp)
			{
				clsUIHelper.ShowErrorMsg( exp.Message);
				retVal= false;
			}
            return retVal;
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
			/*try
			{
				if (ItemPriceValidationRow == null) 
					return ;
				Infragistics.Win.UltraWinEditors.UltraTextEditor txtEditor =  (Infragistics.Win.UltraWinEditors.UltraTextEditor)sender;
				switch(txtEditor.Name)
				{
					case "txtName":
						ItemPriceValidationRow.TypeName= txtName.Text;
						break;
				}
			}
			catch(Exception exp)
			{
				clsUIHelper.ShowErrorMsg(exp.Message);
			}
            */
		}

		public void Edit()
		{
            if (sItemID.Trim() != "")
            {
                oItemPriceValidationData = oItemPriceValidation.PopulateByItem(sItemID);
            }
            else
            {
                oItemPriceValidationData = oItemPriceValidation.PopulateByDept(iDeptID);
            }
            if (oItemPriceValidationData.Tables[0].Rows.Count != 0)
            {
                this.numMinCostPricePerc.Value = oItemPriceValidationData.ItemPriceValidation[0].MinCostPercentage;
                this.numMinSellingPriceAmt.Value = oItemPriceValidationData.ItemPriceValidation[0].MinSellingAmount;
                this.numMinSellingPricePerc.Value = oItemPriceValidationData.ItemPriceValidation[0].MinSellingPercentage;
                if (oItemPriceValidationData.ItemPriceValidation[0].AllowNegative == true)
                {
                    this.optLessThanZeroYes.Checked = true;
                }
                else
                {
                    this.optLessThanZeroNo.Checked = true;
                }

                this.chkIsActive.Checked = oItemPriceValidationData.ItemPriceValidation[0].IsActive;
            }
		}

		private void btnClose_Click(object sender, System.EventArgs e)
		{
			IsCanceled = true;
			this.Close();
		}

        private void frmInvTransType_Load(object sender, System.EventArgs e)
        {
            clsUIHelper.setColorSchecme(this);
            Edit();
            if (this.sItemID.Trim() == "")
            {
                //this.lblMinSellPriceAMt.Visible = false;
                this.numMinSellingPriceAmt.Enabled = false;
            }
        }

		private void frmInvTransType_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
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

		private void frmInvTransType_Activated(object sender, System.EventArgs e)
		{
			clsUIHelper.CurrentForm=this;
		}

        private void numericBox_Enter(object sender, EventArgs e)
        {
            try
            {
                Infragistics.Win.UltraWinEditors.UltraNumericEditor txtEditor = (Infragistics.Win.UltraWinEditors.UltraNumericEditor)sender;
                txtEditor.SelectAll();
            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }

	}
}
