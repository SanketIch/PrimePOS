using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using Infragistics.Win;
using POS_Core.CommonData.Rows;
using POS_Core.CommonData;
using POS_Core.BusinessRules;
using POS_Core.ErrorLogging;
using Infragistics.Win.UltraWinEditors;

namespace POS_Core_UI
{
	/// <summary>
	/// Summary description for frmTaxCodes.
	/// </summary>
	public class frmTaxCodes : System.Windows.Forms.Form
	{
		public bool IsCanceled = true;
		private TaxCodesData oTaxCodesData = new  TaxCodesData();
		private TaxCodesRow oTaxCodesRow ;
		private TaxCodes oTaxCodes = new TaxCodes();
		private Infragistics.Win.Misc.UltraLabel ultraLabel18;
		private Infragistics.Win.Misc.UltraLabel ultraLabel11;
		private Infragistics.Win.Misc.UltraLabel ultraLabel14;
		private Infragistics.Win.Misc.UltraLabel lblTransactionType;
		private Infragistics.Win.Misc.UltraButton btnClose;
		private Infragistics.Win.Misc.UltraButton btnSave;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.ToolTip toolTip1;
		private Infragistics.Win.UltraWinEditors.UltraTextEditor txtName;
		private Infragistics.Win.UltraWinEditors.UltraTextEditor txtCode;
		private Infragistics.Win.UltraWinEditors.UltraNumericEditor txtTax;
		private UltraComboEditor cboTaxCodeType;
		private Infragistics.Win.Misc.UltraLabel ultraLabel1;
        private Infragistics.Win.Misc.UltraPanel pnlClose;
        private Infragistics.Win.Misc.UltraLabel lblClose;
        private Infragistics.Win.Misc.UltraPanel pnlSave;
        private Infragistics.Win.Misc.UltraLabel lblSave;
        private UltraCheckEditor chkActive;
        private System.ComponentModel.IContainer components;

		public void Initialize()
		{
			SetNew();
		}

		public frmTaxCodes()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			FillTaxTypeComboBox();

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
            Infragistics.Win.Appearance appearance19 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance20 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance21 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance22 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance23 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance24 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance25 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance26 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance27 = new Infragistics.Win.Appearance();
            this.ultraLabel18 = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabel11 = new Infragistics.Win.Misc.UltraLabel();
            this.txtName = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.ultraLabel14 = new Infragistics.Win.Misc.UltraLabel();
            this.txtCode = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.txtTax = new Infragistics.Win.UltraWinEditors.UltraNumericEditor();
            this.btnClose = new Infragistics.Win.Misc.UltraButton();
            this.btnSave = new Infragistics.Win.Misc.UltraButton();
            this.lblTransactionType = new Infragistics.Win.Misc.UltraLabel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.ultraLabel1 = new Infragistics.Win.Misc.UltraLabel();
            this.cboTaxCodeType = new Infragistics.Win.UltraWinEditors.UltraComboEditor();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.pnlSave = new Infragistics.Win.Misc.UltraPanel();
            this.lblSave = new Infragistics.Win.Misc.UltraLabel();
            this.pnlClose = new Infragistics.Win.Misc.UltraPanel();
            this.lblClose = new Infragistics.Win.Misc.UltraLabel();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.chkActive = new Infragistics.Win.UltraWinEditors.UltraCheckEditor();
            ((System.ComponentModel.ISupportInitialize)(this.txtName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTax)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cboTaxCodeType)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.pnlSave.ClientArea.SuspendLayout();
            this.pnlSave.SuspendLayout();
            this.pnlClose.ClientArea.SuspendLayout();
            this.pnlClose.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chkActive)).BeginInit();
            this.SuspendLayout();
            // 
            // ultraLabel18
            // 
            appearance19.ForeColor = System.Drawing.Color.Black;
            this.ultraLabel18.Appearance = appearance19;
            this.ultraLabel18.AutoSize = true;
            this.ultraLabel18.Location = new System.Drawing.Point(62, 86);
            this.ultraLabel18.Name = "ultraLabel18";
            this.ultraLabel18.Size = new System.Drawing.Size(49, 18);
            this.ultraLabel18.TabIndex = 15;
            this.ultraLabel18.Text = "Tax %";
            this.ultraLabel18.WrapText = false;
            // 
            // ultraLabel11
            // 
            appearance20.ForeColor = System.Drawing.Color.Black;
            this.ultraLabel11.Appearance = appearance20;
            this.ultraLabel11.Location = new System.Drawing.Point(62, 57);
            this.ultraLabel11.Name = "ultraLabel11";
            this.ultraLabel11.Size = new System.Drawing.Size(84, 14);
            this.ultraLabel11.TabIndex = 3;
            this.ultraLabel11.Text = "Tax Name";
            // 
            // txtName
            // 
            this.txtName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtName.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.txtName.Location = new System.Drawing.Point(146, 54);
            this.txtName.MaxLength = 50;
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(302, 23);
            this.txtName.TabIndex = 4;
            this.txtName.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.txtName.Validated += new System.EventHandler(this.txtBoxs_Validate);
            // 
            // ultraLabel14
            // 
            appearance21.ForeColor = System.Drawing.Color.Black;
            this.ultraLabel14.Appearance = appearance21;
            this.ultraLabel14.Location = new System.Drawing.Point(62, 28);
            this.ultraLabel14.Name = "ultraLabel14";
            this.ultraLabel14.Size = new System.Drawing.Size(82, 14);
            this.ultraLabel14.TabIndex = 1;
            this.ultraLabel14.Text = "Tax Code";
            // 
            // txtCode
            // 
            this.txtCode.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.txtCode.Location = new System.Drawing.Point(146, 24);
            this.txtCode.MaxLength = 20;
            this.txtCode.Name = "txtCode";
            this.txtCode.Size = new System.Drawing.Size(98, 23);
            this.txtCode.TabIndex = 2;
            this.toolTip1.SetToolTip(this.txtCode, "Press F4 To Search");
            this.txtCode.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.txtCode.Validated += new System.EventHandler(this.txtBoxs_Validate);
            // 
            // txtTax
            // 
            this.txtTax.AutoSize = false;
            this.txtTax.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.txtTax.ButtonStyle = Infragistics.Win.UIElementButtonStyle.VisualStudio2005Button;
            this.txtTax.FormatString = "###.####";
            this.txtTax.Location = new System.Drawing.Point(146, 83);
            this.txtTax.MaskClipMode = Infragistics.Win.UltraWinMaskedEdit.MaskMode.Raw;
            this.txtTax.MaskDisplayMode = Infragistics.Win.UltraWinMaskedEdit.MaskMode.Raw;
            this.txtTax.MaskInput = "nnn.nnnn";
            this.txtTax.MaxValue = 100;
            this.txtTax.MinValue = 0;
            this.txtTax.Name = "txtTax";
            this.txtTax.NullText = "0";
            this.txtTax.NumericType = Infragistics.Win.UltraWinEditors.NumericType.Double;
            this.txtTax.Size = new System.Drawing.Size(98, 20);
            this.txtTax.SpinButtonDisplayStyle = Infragistics.Win.ButtonDisplayStyle.Always;
            this.txtTax.TabIndex = 16;
            this.txtTax.TabNavigation = Infragistics.Win.UltraWinMaskedEdit.MaskedEditTabNavigation.NextControl;
            this.txtTax.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.txtTax.Enter += new System.EventHandler(this.txtTax_Enter);
            this.txtTax.Validated += new System.EventHandler(this.txtNumericBoxs_Validate);
            // 
            // btnClose
            // 
            appearance22.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(62)))), ((int)(((byte)(76)))));
            appearance22.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(62)))), ((int)(((byte)(76)))));
            appearance22.ForeColor = System.Drawing.Color.Black;
            this.btnClose.Appearance = appearance22;
            this.btnClose.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Flat;
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnClose.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClose.Location = new System.Drawing.Point(50, 0);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(70, 30);
            this.btnClose.TabIndex = 18;
            this.btnClose.Text = "&Cancel";
            this.btnClose.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnSave
            // 
            appearance23.BackColor = System.Drawing.Color.White;
            appearance23.ForeColor = System.Drawing.Color.Black;
            this.btnSave.Appearance = appearance23;
            this.btnSave.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Flat;
            this.btnSave.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnSave.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold);
            this.btnSave.Location = new System.Drawing.Point(50, 0);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(70, 30);
            this.btnSave.TabIndex = 17;
            this.btnSave.Text = "&Ok";
            this.btnSave.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // lblTransactionType
            // 
            this.lblTransactionType.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            appearance24.ForeColor = System.Drawing.Color.Black;
            appearance24.ForeColorDisabled = System.Drawing.Color.White;
            appearance24.TextHAlignAsString = "Center";
            this.lblTransactionType.Appearance = appearance24;
            this.lblTransactionType.BackColorInternal = System.Drawing.Color.Transparent;
            this.lblTransactionType.Font = new System.Drawing.Font("Arial", 20.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTransactionType.Location = new System.Drawing.Point(10, 10);
            this.lblTransactionType.Name = "lblTransactionType";
            this.lblTransactionType.Size = new System.Drawing.Size(456, 40);
            this.lblTransactionType.TabIndex = 23;
            this.lblTransactionType.Tag = "Header";
            this.lblTransactionType.Text = "Tax Codes Information";
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.chkActive);
            this.groupBox1.Controls.Add(this.ultraLabel1);
            this.groupBox1.Controls.Add(this.cboTaxCodeType);
            this.groupBox1.Controls.Add(this.ultraLabel11);
            this.groupBox1.Controls.Add(this.ultraLabel18);
            this.groupBox1.Controls.Add(this.ultraLabel14);
            this.groupBox1.Controls.Add(this.txtTax);
            this.groupBox1.Controls.Add(this.txtCode);
            this.groupBox1.Controls.Add(this.txtName);
            this.groupBox1.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.groupBox1.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(10, 52);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(456, 147);
            this.groupBox1.TabIndex = 24;
            this.groupBox1.TabStop = false;
            // 
            // ultraLabel1
            // 
            appearance25.ForeColor = System.Drawing.Color.Black;
            this.ultraLabel1.Appearance = appearance25;
            this.ultraLabel1.AutoSize = true;
            this.ultraLabel1.Location = new System.Drawing.Point(62, 113);
            this.ultraLabel1.Name = "ultraLabel1";
            this.ultraLabel1.Size = new System.Drawing.Size(68, 18);
            this.ultraLabel1.TabIndex = 68;
            this.ultraLabel1.Text = "Tax Type";
            this.ultraLabel1.WrapText = false;
            // 
            // cboTaxCodeType
            // 
            this.cboTaxCodeType.DropDownStyle = Infragistics.Win.DropDownStyle.DropDownList;
            this.cboTaxCodeType.Location = new System.Drawing.Point(146, 109);
            this.cboTaxCodeType.Name = "cboTaxCodeType";
            this.cboTaxCodeType.Size = new System.Drawing.Size(188, 25);
            this.cboTaxCodeType.TabIndex = 67;
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.pnlSave);
            this.groupBox2.Controls.Add(this.pnlClose);
            this.groupBox2.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.groupBox2.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.Location = new System.Drawing.Point(10, 203);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(456, 59);
            this.groupBox2.TabIndex = 25;
            this.groupBox2.TabStop = false;
            // 
            // pnlSave
            // 
            // 
            // pnlSave.ClientArea
            // 
            this.pnlSave.ClientArea.Controls.Add(this.btnSave);
            this.pnlSave.ClientArea.Controls.Add(this.lblSave);
            this.pnlSave.Location = new System.Drawing.Point(197, 18);
            this.pnlSave.Name = "pnlSave";
            this.pnlSave.Size = new System.Drawing.Size(120, 30);
            this.pnlSave.TabIndex = 73;
            // 
            // lblSave
            // 
            appearance26.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(138)))), ((int)(((byte)(31)))));
            appearance26.FontData.BoldAsString = "True";
            appearance26.ForeColor = System.Drawing.Color.White;
            appearance26.TextHAlignAsString = "Center";
            appearance26.TextVAlignAsString = "Middle";
            this.lblSave.Appearance = appearance26;
            this.lblSave.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblSave.Font = new System.Drawing.Font("Verdana", 8F, System.Drawing.FontStyle.Bold);
            this.lblSave.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblSave.Location = new System.Drawing.Point(0, 0);
            this.lblSave.Margin = new System.Windows.Forms.Padding(3, 3, 0, 3);
            this.lblSave.Name = "lblSave";
            this.lblSave.Size = new System.Drawing.Size(50, 30);
            this.lblSave.TabIndex = 0;
            this.lblSave.Tag = "NOCOLOR";
            this.lblSave.Text = "Alt + O";
            this.lblSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // pnlClose
            // 
            // 
            // pnlClose.ClientArea
            // 
            this.pnlClose.ClientArea.Controls.Add(this.btnClose);
            this.pnlClose.ClientArea.Controls.Add(this.lblClose);
            this.pnlClose.Location = new System.Drawing.Point(328, 18);
            this.pnlClose.Name = "pnlClose";
            this.pnlClose.Size = new System.Drawing.Size(120, 30);
            this.pnlClose.TabIndex = 19;
            // 
            // lblClose
            // 
            appearance27.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(138)))), ((int)(((byte)(31)))));
            appearance27.FontData.BoldAsString = "True";
            appearance27.ForeColor = System.Drawing.Color.White;
            appearance27.TextHAlignAsString = "Center";
            appearance27.TextVAlignAsString = "Middle";
            this.lblClose.Appearance = appearance27;
            this.lblClose.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblClose.Font = new System.Drawing.Font("Verdana", 8F, System.Drawing.FontStyle.Bold);
            this.lblClose.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblClose.Location = new System.Drawing.Point(0, 0);
            this.lblClose.Margin = new System.Windows.Forms.Padding(3, 3, 0, 3);
            this.lblClose.Name = "lblClose";
            this.lblClose.Size = new System.Drawing.Size(50, 30);
            this.lblClose.TabIndex = 0;
            this.lblClose.Tag = "NOCOLOR";
            this.lblClose.Text = "Alt + C";
            this.lblClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // chkActive
            // 
            this.chkActive.Location = new System.Drawing.Point(340, 112);
            this.chkActive.Name = "chkActive";
            this.chkActive.Size = new System.Drawing.Size(110, 20);
            this.chkActive.TabIndex = 69;
            this.chkActive.Text = "Active";
            this.chkActive.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.chkActive.Validated += new System.EventHandler(this.chkActive_Validated);
            // 
            // frmTaxCodes
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
            this.BackColor = System.Drawing.Color.White;
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(480, 280);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.lblTransactionType);
            this.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.Name = "frmTaxCodes";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Tax Codes";
            this.Activated += new System.EventHandler(this.frmTaxCodes_Activated);
            this.Load += new System.EventHandler(this.frmTaxCodes_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmTaxCodes_KeyDown);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.frmTaxCodes_KeyUp);
            ((System.ComponentModel.ISupportInitialize)(this.txtName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTax)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cboTaxCodeType)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.pnlSave.ClientArea.ResumeLayout(false);
            this.pnlSave.ResumeLayout(false);
            this.pnlClose.ClientArea.ResumeLayout(false);
            this.pnlClose.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.chkActive)).EndInit();
            this.ResumeLayout(false);

		}
		#endregion
		private bool Save()
		{
			try
			{
				if (!isInEditMode)
				{
					TaxCodesData taxCodesData = oTaxCodes.Populate(txtCode.Text);

					if (taxCodesData != null && taxCodesData.TaxCodes.Rows.Count > 0)
					{
						ErrorHandler.throwCustomError(POSErrorENUM.TaxCode_DuplicateTaxCode);
						return false;
					}
				}

				oTaxCodesRow.TaxCode= txtCode.Text;
				oTaxCodesRow.TaxType = cboTaxCodeType.SelectedItem == null ? 0 : (int) ((TaxTypes) cboTaxCodeType.SelectedItem.Tag);
                oTaxCodesRow.Active = chkActive.Checked;//2974
				oTaxCodes.Persist(oTaxCodesData);
				return true;
			}
			catch(POSExceptions exp)
			{
				clsUIHelper.ShowErrorMsg(exp.ErrMessage);
				switch (exp.ErrNumber)
				{
					case (long)POSErrorENUM.TaxCodes_PrimaryKeyVoilation:
						txtCode.Focus();
						break;
					case (long)POSErrorENUM.TaxCodes_DescriptionCanNotBeNull:
						txtName.Focus();
						break;
					case (long)POSErrorENUM.TaxCodes_AmountCanNotBeNull:
						txtTax.Focus();
						break;
					case (long)POSErrorENUM.TaxCode_DuplicateTaxCode:
						txtCode.Focus();
						break;

				}
				return false;
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
				if (oTaxCodesRow == null) 
					return ;
				Infragistics.Win.UltraWinEditors.UltraTextEditor txtEditor =  (Infragistics.Win.UltraWinEditors.UltraTextEditor)sender;
				switch(txtEditor.Name)
				{
					case "txtCode":
						oTaxCodesRow.TaxCode = txtCode.Text;
						break;
					case "txtName":
						oTaxCodesRow.Description= txtName.Text;
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
				if (oTaxCodesRow == null) 
					return ;
				Infragistics.Win.UltraWinEditors.UltraNumericEditor txtEditor =  (Infragistics.Win.UltraWinEditors.UltraNumericEditor)sender;
				switch(txtEditor.Name)
				{
					case "txtTax":
						oTaxCodesRow.Amount= POS_Core.Resources.Configuration.convertNullToDecimal(this.txtTax.Value.ToString());
						break;
				}
			}
			catch(Exception exp)
			{
				clsUIHelper.ShowErrorMsg(exp.Message);
			}
		}

		private void SearchTaxCode()
		{
			try
			{
                //frmSearch oSearch = new frmSearch(clsPOSDBConstants.TaxCodes_tbl);
                frmSearchMain oSearch = new frmSearchMain(true);    //20-Dec-2017 JY Added new reference
                oSearch.SearchTable = clsPOSDBConstants.TaxCodes_tbl;    //20-Dec-2017 JY Added
                oSearch.ShowDialog(this);
				if (!oSearch.IsCanceled)
				{
					string strCode=oSearch.SelectedRowID();
					if (strCode == "") 
						return;
					
					Display();
				}
			}
			catch(Exception exp)
			{
				clsUIHelper.ShowErrorMsg(exp.Message);
			}
		}

		private void Display()
		{
			txtCode.Text = oTaxCodesRow.TaxCode;
			txtName.Text = oTaxCodesRow.Description;
			txtTax.Text = oTaxCodesRow.Amount.ToString();
			cboTaxCodeType.SelectedItem = GetTaxTypeToBeSelected(oTaxCodesRow.TaxType);
            chkActive.Checked = oTaxCodesRow.Active;//2974
		}

		private ValueListItem GetTaxTypeToBeSelected(int taxType)
		{
			if (taxType <= 0)
				return null;
			else
			{
				try
				{
					return cboTaxCodeType.Items[taxType - 1];
				}
				catch (Exception)
				{
					return null;
				}
			}
		}

		private bool isInEditMode;
		public void Edit(string TaxCode)
		{
			try
			{
				isInEditMode = true;
				txtCode.Enabled = false;
				oTaxCodesData = oTaxCodes.Populate(TaxCode);
				oTaxCodesRow = oTaxCodesData.TaxCodes.GetRowByID(TaxCode);
				this.Text="Edit Tax Table";
				this.lblTransactionType.Text="Edit Tax Table";
				if (oTaxCodesRow!= null ) 
					Display();
			}
			catch(Exception exp)
			{
				clsUIHelper.ShowErrorMsg(exp.Message);
			}
		}

		private void SetNew()
		{
			oTaxCodes = new  TaxCodes();
			oTaxCodesData = new  TaxCodesData();
			this.Text="Add to Tax Table";
			this.lblTransactionType.Text="Add to Tax Table";
			Clear();
			
			oTaxCodesRow = oTaxCodesData.TaxCodes.AddRow(0, "", "", 0, "", -1,true);//2974
		}

		private void FillTaxTypeComboBox()
		{
			cboTaxCodeType.Items.Add(GetStateTypeValueListItem(TaxTypes.State.ToString(), TaxTypes.State));
			cboTaxCodeType.Items.Add(GetStateTypeValueListItem(TaxTypes.Municipality.ToString(), TaxTypes.Municipality));
			cboTaxCodeType.Items.Add(GetStateTypeValueListItem(TaxTypes.Federal.ToString(), TaxTypes.Federal));
			cboTaxCodeType.Items.Add(GetStateTypeValueListItem(TaxTypes.City.ToString(), TaxTypes.City));
			cboTaxCodeType.Items.Add(GetStateTypeValueListItem(TaxTypes.Local.ToString(), TaxTypes.Local));
			cboTaxCodeType.Items.Add(GetStateTypeValueListItem(TaxTypes.County.ToString(), TaxTypes.County));            

        }

		private static ValueListItem GetStateTypeValueListItem(string displayText, TaxTypes taxTypes)
		{
			return new ValueListItem(displayText) {Tag = taxTypes};
		}

		private void Clear()
		{
			txtName.Text = "";
			txtTax.Value = 0;
			txtCode.Text = "";
		}


		private void btnNew_Click(object sender, System.EventArgs e)
		{
			try
			{
				txtCode.Text = "";
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

		private void txtTax_Enter(object sender, System.EventArgs e)
		{
			this.txtTax.SelectionStart=0;
			this.txtTax.SelectionLength=this.txtTax.MaskInput.ToString().Length;
		}

		private void frmTaxCodes_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			try
			{
				if (e.KeyData==System.Windows.Forms.Keys.F4)
				{
					if ( txtCode.ContainsFocus)
						this.SearchTaxCode();
				}
			}
			catch(Exception exp)
			{
				clsUIHelper.ShowErrorMsg(exp.Message);
			}
		}

		private void frmTaxCodes_Load(object sender, System.EventArgs e)
		{
			this.txtCode.AfterEnterEditMode += new System.EventHandler(clsUIHelper.AfterEnterEditMode);			
			this.txtCode.AfterExitEditMode += new System.EventHandler(clsUIHelper.AfterExitEditMode);			
			this.txtName.AfterEnterEditMode += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
			this.txtName.AfterExitEditMode += new System.EventHandler(clsUIHelper.AfterExitEditMode);

			this.txtTax.AfterEnterEditMode += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
			this.txtTax.AfterExitEditMode += new System.EventHandler(clsUIHelper.AfterExitEditMode);
			
			this.txtCode.AfterEnterEditMode += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
			this.txtCode.AfterExitEditMode += new System.EventHandler(clsUIHelper.AfterExitEditMode);
			IsCanceled = true;

			clsUIHelper.setColorSchecme(this);
            btnClose.Appearance.BackColor = Color.FromArgb(255, System.Convert.ToByte(POS_Core.Resources.Configuration.arrCloseBk[0].ToString()), System.Convert.ToByte(POS_Core.Resources.Configuration.arrCloseBk[1]), System.Convert.ToByte(POS_Core.Resources.Configuration.arrCloseBk[2]));
            btnClose.Appearance.BackColor2 = Color.FromArgb(255, System.Convert.ToByte(POS_Core.Resources.Configuration.arrCloseBk[0].ToString()), System.Convert.ToByte(POS_Core.Resources.Configuration.arrCloseBk[1]), System.Convert.ToByte(POS_Core.Resources.Configuration.arrCloseBk[2]));
        }
		private void frmTaxCodes_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
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

		private void frmTaxCodes_Activated(object sender, System.EventArgs e)
		{
			clsUIHelper.CurrentForm=this;
		}

        private void chkActive_Validated(object sender, EventArgs e)
        {
            if (oTaxCodesRow == null)
                return;
            Infragistics.Win.UltraWinEditors.UltraCheckEditor txtEditor = (Infragistics.Win.UltraWinEditors.UltraCheckEditor)sender;

             string abc = txtEditor.Name;
        }
    }
}
