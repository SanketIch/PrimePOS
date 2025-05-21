using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using Infragistics.Win;
using POS_Core.CommonData.Rows;
using POS_Core.CommonData;
using POS_Core.BusinessRules;
using POS_Core.DataAccess;
using POS_Core.ErrorLogging;
using Infragistics.Win.UltraWinEditors;
using ToolTip = System.Windows.Forms.ToolTip;
using POS_Core.Resources;
//using POS_Core.DataAccess;

namespace POS_Core_UI
{
	/// <summary>
	/// Summary description for frmDepartment.
	/// </summary>
	public class frmDepartment : System.Windows.Forms.Form
	{
		public bool IsCanceled = true;
		private DepartmentData oDepartmentData = new DepartmentData();
		private DepartmentRow oDepartmentRow ;
		private Department oBRDepartment = new Department();
		private Infragistics.Win.Misc.UltraLabel ultraLabel21;
		private Infragistics.Win.Misc.UltraLabel ultraLabel19;
		private Infragistics.Win.Misc.UltraLabel ultraLabel20;
		private Infragistics.Win.Misc.UltraLabel ultraLabel18;
		private Infragistics.Win.Misc.UltraLabel ultraLabel12;
		private Infragistics.Win.Misc.UltraLabel ultraLabel11;
		private Infragistics.Win.Misc.UltraLabel ultraLabel14;
		private Infragistics.Win.Misc.UltraLabel lblTransactionType;
		private System.Windows.Forms.CheckBox chkIsTaxable;
		private Infragistics.Win.UltraWinEditors.UltraTextEditor txtDeptName;
		private Infragistics.Win.UltraWinEditors.UltraTextEditor txtDeptCode;
		private Infragistics.Win.Misc.UltraButton btnNew;
		private Infragistics.Win.Misc.UltraButton btnSearch;
		private Infragistics.Win.Misc.UltraButton btnClose;
		private Infragistics.Win.Misc.UltraButton btnSave;
		private Infragistics.Win.UltraWinEditors.UltraNumericEditor txtDeptSalePrice;
		private Infragistics.Win.UltraWinEditors.UltraNumericEditor txtDiscount;
		private Infragistics.Win.UltraWinSchedule.UltraCalendarCombo dtpSaleStartDate;
		private Infragistics.Win.UltraWinSchedule.UltraCalendarCombo dtpSaleEndDate;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.ToolTip toolTip1;
		private Infragistics.Win.Misc.UltraLabel ultraLabel7;
		private Infragistics.Win.Misc.UltraLabel ultraLabel1;
		private Infragistics.Win.Misc.UltraLabel ultraLabel2;
        private Infragistics.Win.Misc.UltraButton btnPriceValidation;
        private GroupBox groupBox3;
        private Infragistics.Win.Misc.UltraLabel ultraLabel3;
        private UltraNumericEditor txtSaleDiscount;
        private Infragistics.Win.Misc.UltraButton btnSubDepartments;
		private Infragistics.Win.Misc.UltraButton btnDeptNote;
		private System.ComponentModel.IContainer components;
		private ToolTip _toolTip1;
		private List<int> _selectedTaxes;
        private Infragistics.Win.Misc.UltraLabel ultraLabel4;
        private UltraNumericEditor numCLPointsPerDollar;
        private Infragistics.Win.Misc.UltraPanel pnlDeptNote;
        private Infragistics.Win.Misc.UltraLabel lblDeptNote;
        private Infragistics.Win.Misc.UltraPanel pnlClose;
        private Infragistics.Win.Misc.UltraLabel lblClose;
        private Infragistics.Win.Misc.UltraPanel pnlSubDepartments;
        private Infragistics.Win.Misc.UltraLabel lblSubDepartments;
        private Infragistics.Win.Misc.UltraPanel pnlSave;
        private Infragistics.Win.Misc.UltraLabel lblSave;
        private UltraComboEditor cboTaxCodes;

		public void Initialize()
		{
			FillTaxInformation();
			SetNew();
		}

		public frmDepartment()
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

        //Following Constructor Added by Krishna on 3 May 2011
        public frmDepartment(string Departmentvalue)
        {
            InitializeComponent();
            try
            {
                Initialize();
                txtDeptCode.Text = Departmentvalue;
            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }
        //Till here Added by krishna


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
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance7 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance8 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance9 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinSchedule.CalendarCombo.DateButton dateButton1 = new Infragistics.Win.UltraWinSchedule.CalendarCombo.DateButton();
            Infragistics.Win.Appearance appearance10 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinSchedule.CalendarCombo.DateButton dateButton2 = new Infragistics.Win.UltraWinSchedule.CalendarCombo.DateButton();
            Infragistics.Win.Appearance appearance11 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance12 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance13 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance16 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance17 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance14 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance18 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance19 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance20 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance23 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance21 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance26 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance27 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance25 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance15 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance24 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance22 = new Infragistics.Win.Appearance();
            this.chkIsTaxable = new System.Windows.Forms.CheckBox();
            this.ultraLabel21 = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabel19 = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabel20 = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabel18 = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabel12 = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabel11 = new Infragistics.Win.Misc.UltraLabel();
            this.txtDeptName = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.ultraLabel14 = new Infragistics.Win.Misc.UltraLabel();
            this.txtDeptCode = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.txtDeptSalePrice = new Infragistics.Win.UltraWinEditors.UltraNumericEditor();
            this.txtDiscount = new Infragistics.Win.UltraWinEditors.UltraNumericEditor();
            this.dtpSaleStartDate = new Infragistics.Win.UltraWinSchedule.UltraCalendarCombo();
            this.dtpSaleEndDate = new Infragistics.Win.UltraWinSchedule.UltraCalendarCombo();
            this.btnNew = new Infragistics.Win.Misc.UltraButton();
            this.btnSearch = new Infragistics.Win.Misc.UltraButton();
            this.btnClose = new Infragistics.Win.Misc.UltraButton();
            this.btnSave = new Infragistics.Win.Misc.UltraButton();
            this.lblTransactionType = new Infragistics.Win.Misc.UltraLabel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.ultraLabel4 = new Infragistics.Win.Misc.UltraLabel();
            this.numCLPointsPerDollar = new Infragistics.Win.UltraWinEditors.UltraNumericEditor();
            this.cboTaxCodes = new Infragistics.Win.UltraWinEditors.UltraComboEditor();
            this.btnDeptNote = new Infragistics.Win.Misc.UltraButton();
            this.ultraLabel1 = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabel7 = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabel2 = new Infragistics.Win.Misc.UltraLabel();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnSubDepartments = new Infragistics.Win.Misc.UltraButton();
            this.btnPriceValidation = new Infragistics.Win.Misc.UltraButton();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.ultraLabel3 = new Infragistics.Win.Misc.UltraLabel();
            this.txtSaleDiscount = new Infragistics.Win.UltraWinEditors.UltraNumericEditor();
            this.pnlClose = new Infragistics.Win.Misc.UltraPanel();
            this.lblClose = new Infragistics.Win.Misc.UltraLabel();
            this.pnlDeptNote = new Infragistics.Win.Misc.UltraPanel();
            this.lblDeptNote = new Infragistics.Win.Misc.UltraLabel();
            this.pnlSubDepartments = new Infragistics.Win.Misc.UltraPanel();
            this.lblSubDepartments = new Infragistics.Win.Misc.UltraLabel();
            this.pnlSave = new Infragistics.Win.Misc.UltraPanel();
            this.lblSave = new Infragistics.Win.Misc.UltraLabel();
            ((System.ComponentModel.ISupportInitialize)(this.txtDeptName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDeptCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDeptSalePrice)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDiscount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtpSaleStartDate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtpSaleEndDate)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numCLPointsPerDollar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboTaxCodes)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtSaleDiscount)).BeginInit();
            this.pnlClose.ClientArea.SuspendLayout();
            this.pnlClose.SuspendLayout();
            this.pnlDeptNote.ClientArea.SuspendLayout();
            this.pnlDeptNote.SuspendLayout();
            this.pnlSubDepartments.ClientArea.SuspendLayout();
            this.pnlSubDepartments.SuspendLayout();
            this.pnlSave.ClientArea.SuspendLayout();
            this.pnlSave.SuspendLayout();
            this.SuspendLayout();
            // 
            // chkIsTaxable
            // 
            this.chkIsTaxable.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkIsTaxable.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chkIsTaxable.ForeColor = System.Drawing.Color.White;
            this.chkIsTaxable.Location = new System.Drawing.Point(6, 78);
            this.chkIsTaxable.Name = "chkIsTaxable";
            this.chkIsTaxable.Size = new System.Drawing.Size(136, 21);
            this.chkIsTaxable.TabIndex = 1;
            this.chkIsTaxable.Text = "Taxable ?";
            this.chkIsTaxable.CheckedChanged += new System.EventHandler(this.chkIsTaxable_CheckedChanged);
            // 
            // ultraLabel21
            // 
            appearance1.FontData.BoldAsString = "False";
            appearance1.ForeColor = System.Drawing.Color.White;
            this.ultraLabel21.Appearance = appearance1;
            this.ultraLabel21.Location = new System.Drawing.Point(6, 56);
            this.ultraLabel21.Name = "ultraLabel21";
            this.ultraLabel21.Size = new System.Drawing.Size(114, 21);
            this.ultraLabel21.TabIndex = 13;
            this.ultraLabel21.Text = "Item Price";
            // 
            // ultraLabel19
            // 
            appearance2.FontData.BoldAsString = "False";
            appearance2.ForeColor = System.Drawing.Color.White;
            this.ultraLabel19.Appearance = appearance2;
            this.ultraLabel19.Location = new System.Drawing.Point(285, 27);
            this.ultraLabel19.Name = "ultraLabel19";
            this.ultraLabel19.Size = new System.Drawing.Size(103, 18);
            this.ultraLabel19.TabIndex = 11;
            this.ultraLabel19.Text = "End Date";
            // 
            // ultraLabel20
            // 
            appearance3.FontData.BoldAsString = "False";
            appearance3.ForeColor = System.Drawing.Color.White;
            this.ultraLabel20.Appearance = appearance3;
            this.ultraLabel20.Location = new System.Drawing.Point(6, 27);
            this.ultraLabel20.Name = "ultraLabel20";
            this.ultraLabel20.Size = new System.Drawing.Size(116, 21);
            this.ultraLabel20.TabIndex = 9;
            this.ultraLabel20.Text = "Start Date";
            // 
            // ultraLabel18
            // 
            this.ultraLabel18.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            appearance4.ForeColor = System.Drawing.Color.White;
            this.ultraLabel18.Appearance = appearance4;
            this.ultraLabel18.Location = new System.Drawing.Point(10, 131);
            this.ultraLabel18.Name = "ultraLabel18";
            this.ultraLabel18.Size = new System.Drawing.Size(87, 18);
            this.ultraLabel18.TabIndex = 15;
            this.ultraLabel18.Text = "Discount %";
            this.ultraLabel18.WrapText = false;
            this.ultraLabel18.Click += new System.EventHandler(this.ultraLabel18_Click);
            // 
            // ultraLabel12
            // 
            appearance5.ForeColor = System.Drawing.Color.White;
            this.ultraLabel12.Appearance = appearance5;
            this.ultraLabel12.Location = new System.Drawing.Point(9, 102);
            this.ultraLabel12.Name = "ultraLabel12";
            this.ultraLabel12.Size = new System.Drawing.Size(82, 21);
            this.ultraLabel12.TabIndex = 6;
            this.ultraLabel12.Text = "Tax Code";
            this.ultraLabel12.Click += new System.EventHandler(this.ultraLabel12_Click);
            // 
            // ultraLabel11
            // 
            appearance6.ForeColor = System.Drawing.Color.White;
            this.ultraLabel11.Appearance = appearance6;
            this.ultraLabel11.Location = new System.Drawing.Point(10, 50);
            this.ultraLabel11.Name = "ultraLabel11";
            this.ultraLabel11.Size = new System.Drawing.Size(84, 21);
            this.ultraLabel11.TabIndex = 3;
            this.ultraLabel11.Text = "Name";
            this.ultraLabel11.Click += new System.EventHandler(this.ultraLabel11_Click);
            // 
            // txtDeptName
            // 
            this.txtDeptName.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.txtDeptName.Location = new System.Drawing.Point(130, 49);
            this.txtDeptName.MaxLength = 50;
            this.txtDeptName.Name = "txtDeptName";
            this.txtDeptName.Size = new System.Drawing.Size(434, 23);
            this.txtDeptName.TabIndex = 1;
            this.txtDeptName.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.txtDeptName.Validated += new System.EventHandler(this.txtBoxs_Validate);
            // 
            // ultraLabel14
            // 
            appearance7.ForeColor = System.Drawing.Color.White;
            this.ultraLabel14.Appearance = appearance7;
            this.ultraLabel14.Location = new System.Drawing.Point(10, 20);
            this.ultraLabel14.Name = "ultraLabel14";
            this.ultraLabel14.Size = new System.Drawing.Size(82, 21);
            this.ultraLabel14.TabIndex = 1;
            this.ultraLabel14.Text = "Code";
            this.ultraLabel14.Click += new System.EventHandler(this.ultraLabel14_Click);
            // 
            // txtDeptCode
            // 
            this.txtDeptCode.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.txtDeptCode.Location = new System.Drawing.Point(130, 19);
            this.txtDeptCode.MaxLength = 20;
            this.txtDeptCode.Name = "txtDeptCode";
            this.txtDeptCode.Size = new System.Drawing.Size(148, 23);
            this.txtDeptCode.TabIndex = 0;
            this.toolTip1.SetToolTip(this.txtDeptCode, "Press F4 To Search");
            this.txtDeptCode.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.txtDeptCode.EditorButtonClick += new Infragistics.Win.UltraWinEditors.EditorButtonEventHandler(this.txtDeptCode_EditorButtonClick);
            this.txtDeptCode.Leave += new System.EventHandler(this.txtDeptCode_Leave);
            this.txtDeptCode.Validated += new System.EventHandler(this.txtBoxs_Validate);
            // 
            // txtDeptSalePrice
            // 
            appearance8.FontData.BoldAsString = "False";
            this.txtDeptSalePrice.Appearance = appearance8;
            this.txtDeptSalePrice.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.txtDeptSalePrice.ButtonStyle = Infragistics.Win.UIElementButtonStyle.VisualStudio2005Button;
            this.txtDeptSalePrice.FormatString = "##,##,###.00";
            this.txtDeptSalePrice.Location = new System.Drawing.Point(126, 56);
            this.txtDeptSalePrice.MaskClipMode = Infragistics.Win.UltraWinMaskedEdit.MaskMode.Raw;
            this.txtDeptSalePrice.MaskDisplayMode = Infragistics.Win.UltraWinMaskedEdit.MaskMode.Raw;
            this.txtDeptSalePrice.MaskInput = "nn,nn,nnn.nn";
            this.txtDeptSalePrice.MaxValue = 99999.99D;
            this.txtDeptSalePrice.MinValue = -1;
            this.txtDeptSalePrice.Name = "txtDeptSalePrice";
            this.txtDeptSalePrice.NullText = "0";
            this.txtDeptSalePrice.NumericType = Infragistics.Win.UltraWinEditors.NumericType.Double;
            this.txtDeptSalePrice.Size = new System.Drawing.Size(148, 23);
            this.txtDeptSalePrice.SpinButtonDisplayStyle = Infragistics.Win.ButtonDisplayStyle.Always;
            this.txtDeptSalePrice.TabIndex = 1;
            this.txtDeptSalePrice.TabNavigation = Infragistics.Win.UltraWinMaskedEdit.MaskedEditTabNavigation.NextControl;
            this.txtDeptSalePrice.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.txtDeptSalePrice.Enter += new System.EventHandler(this.txtDeptSalePrice_Enter);
            this.txtDeptSalePrice.Validated += new System.EventHandler(this.txtNumericBoxs_Validate);
            // 
            // txtDiscount
            // 
            this.txtDiscount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.txtDiscount.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.txtDiscount.ButtonStyle = Infragistics.Win.UIElementButtonStyle.VisualStudio2005Button;
            this.txtDiscount.FormatString = "###.00";
            this.txtDiscount.Location = new System.Drawing.Point(130, 130);
            this.txtDiscount.MaskClipMode = Infragistics.Win.UltraWinMaskedEdit.MaskMode.Raw;
            this.txtDiscount.MaskDisplayMode = Infragistics.Win.UltraWinMaskedEdit.MaskMode.Raw;
            this.txtDiscount.MaskInput = "nnn.nn";
            this.txtDiscount.MaxValue = 100;
            this.txtDiscount.MinValue = -1;
            this.txtDiscount.Name = "txtDiscount";
            this.txtDiscount.NullText = "0";
            this.txtDiscount.NumericType = Infragistics.Win.UltraWinEditors.NumericType.Double;
            this.txtDiscount.Size = new System.Drawing.Size(148, 23);
            this.txtDiscount.SpinButtonDisplayStyle = Infragistics.Win.ButtonDisplayStyle.Always;
            this.txtDiscount.TabIndex = 4;
            this.txtDiscount.TabNavigation = Infragistics.Win.UltraWinMaskedEdit.MaskedEditTabNavigation.NextControl;
            this.txtDiscount.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.txtDiscount.Enter += new System.EventHandler(this.txtDiscount_Enter);
            this.txtDiscount.Validated += new System.EventHandler(this.txtNumericBoxs_Validate);
            // 
            // dtpSaleStartDate
            // 
            this.dtpSaleStartDate.AllowNull = false;
            appearance9.FontData.BoldAsString = "False";
            this.dtpSaleStartDate.Appearance = appearance9;
            this.dtpSaleStartDate.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Office2003ToolbarButton;
            this.dtpSaleStartDate.DateButtons.Add(dateButton1);
            this.dtpSaleStartDate.Location = new System.Drawing.Point(126, 26);
            this.dtpSaleStartDate.Name = "dtpSaleStartDate";
            this.dtpSaleStartDate.NonAutoSizeHeight = 10;
            this.dtpSaleStartDate.Size = new System.Drawing.Size(148, 22);
            this.dtpSaleStartDate.TabIndex = 0;
            this.dtpSaleStartDate.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.dtpSaleStartDate.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.dtpSaleStartDate.Value = new System.DateTime(2004, 5, 25, 0, 0, 0, 0);
            this.dtpSaleStartDate.Validated += new System.EventHandler(this.txtDateBoxs_Validate);
            // 
            // dtpSaleEndDate
            // 
            this.dtpSaleEndDate.AllowNull = false;
            appearance10.FontData.BoldAsString = "False";
            this.dtpSaleEndDate.Appearance = appearance10;
            this.dtpSaleEndDate.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Office2003ToolbarButton;
            this.dtpSaleEndDate.DateButtons.Add(dateButton2);
            this.dtpSaleEndDate.Location = new System.Drawing.Point(412, 26);
            this.dtpSaleEndDate.Name = "dtpSaleEndDate";
            this.dtpSaleEndDate.NonAutoSizeHeight = 10;
            this.dtpSaleEndDate.Size = new System.Drawing.Size(148, 22);
            this.dtpSaleEndDate.TabIndex = 2;
            this.dtpSaleEndDate.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.dtpSaleEndDate.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.dtpSaleEndDate.Value = new System.DateTime(2004, 5, 25, 0, 0, 0, 0);
            this.dtpSaleEndDate.Validated += new System.EventHandler(this.txtDateBoxs_Validate);
            // 
            // btnNew
            // 
            this.btnNew.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            this.btnNew.Location = new System.Drawing.Point(28, 266);
            this.btnNew.Name = "btnNew";
            this.btnNew.Size = new System.Drawing.Size(85, 26);
            this.btnNew.TabIndex = 9;
            this.btnNew.Text = "&Clear";
            this.btnNew.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnNew.Visible = false;
            this.btnNew.Click += new System.EventHandler(this.btnNew_Click);
            // 
            // btnSearch
            // 
            this.btnSearch.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            this.btnSearch.Location = new System.Drawing.Point(120, 266);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(85, 26);
            this.btnSearch.TabIndex = 10;
            this.btnSearch.Text = "S&earch";
            this.toolTip1.SetToolTip(this.btnSearch, "Press F4 To Search");
            this.btnSearch.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnSearch.Visible = false;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // btnClose
            // 
            appearance11.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(62)))), ((int)(((byte)(76)))));
            appearance11.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(62)))), ((int)(((byte)(76)))));
            appearance11.ForeColor = System.Drawing.Color.Black;
            this.btnClose.Appearance = appearance11;
            this.btnClose.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Flat;
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnClose.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold);
            this.btnClose.Location = new System.Drawing.Point(50, 0);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(70, 30);
            this.btnClose.TabIndex = 3;
            this.btnClose.Text = "&Cancel";
            this.btnClose.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.btnClose.UseHotTracking = Infragistics.Win.DefaultableBoolean.True;
            this.btnClose.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnSave
            // 
            appearance12.BackColor = System.Drawing.Color.White;
            appearance12.FontData.BoldAsString = "True";
            appearance12.ForeColor = System.Drawing.Color.Black;
            this.btnSave.Appearance = appearance12;
            this.btnSave.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Flat;
            this.btnSave.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnSave.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold);
            this.btnSave.Location = new System.Drawing.Point(50, 0);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(70, 30);
            this.btnSave.TabIndex = 2;
            this.btnSave.Text = "&Ok";
            this.btnSave.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.btnSave.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // lblTransactionType
            // 
            appearance13.ForeColor = System.Drawing.Color.White;
            appearance13.ForeColorDisabled = System.Drawing.Color.White;
            appearance13.TextHAlignAsString = "Center";
            this.lblTransactionType.Appearance = appearance13;
            this.lblTransactionType.BackColorInternal = System.Drawing.Color.Transparent;
            this.lblTransactionType.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTransactionType.Location = new System.Drawing.Point(16, 4);
            this.lblTransactionType.Name = "lblTransactionType";
            this.lblTransactionType.Size = new System.Drawing.Size(524, 50);
            this.lblTransactionType.TabIndex = 0;
            this.lblTransactionType.Tag = "Header";
            this.lblTransactionType.Text = "Department Information";
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.pnlDeptNote);
            this.groupBox1.Controls.Add(this.ultraLabel4);
            this.groupBox1.Controls.Add(this.numCLPointsPerDollar);
            this.groupBox1.Controls.Add(this.cboTaxCodes);
            this.groupBox1.Controls.Add(this.ultraLabel1);
            this.groupBox1.Controls.Add(this.ultraLabel7);
            this.groupBox1.Controls.Add(this.ultraLabel11);
            this.groupBox1.Controls.Add(this.ultraLabel18);
            this.groupBox1.Controls.Add(this.ultraLabel12);
            this.groupBox1.Controls.Add(this.ultraLabel14);
            this.groupBox1.Controls.Add(this.txtDiscount);
            this.groupBox1.Controls.Add(this.txtDeptCode);
            this.groupBox1.Controls.Add(this.txtDeptName);
            this.groupBox1.Controls.Add(this.chkIsTaxable);
            this.groupBox1.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.groupBox1.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(14, 62);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(577, 292);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            // 
            // ultraLabel4
            // 
            this.ultraLabel4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            appearance16.ForeColor = System.Drawing.Color.White;
            this.ultraLabel4.Appearance = appearance16;
            this.ultraLabel4.Location = new System.Drawing.Point(10, 162);
            this.ultraLabel4.Name = "ultraLabel4";
            this.ultraLabel4.Size = new System.Drawing.Size(77, 18);
            this.ultraLabel4.TabIndex = 69;
            this.ultraLabel4.Text = "CL Points";
            this.ultraLabel4.WrapText = false;
            // 
            // numCLPointsPerDollar
            // 
            appearance17.FontData.BoldAsString = "False";
            appearance17.FontData.ItalicAsString = "False";
            appearance17.FontData.StrikeoutAsString = "False";
            appearance17.FontData.UnderlineAsString = "False";
            appearance17.ForeColor = System.Drawing.Color.Black;
            appearance17.ForeColorDisabled = System.Drawing.Color.Black;
            this.numCLPointsPerDollar.Appearance = appearance17;
            this.numCLPointsPerDollar.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.numCLPointsPerDollar.Location = new System.Drawing.Point(130, 157);
            this.numCLPointsPerDollar.MaskInput = "nnnnnnn";
            this.numCLPointsPerDollar.MaxValue = 100;
            this.numCLPointsPerDollar.MinValue = 0D;
            this.numCLPointsPerDollar.Name = "numCLPointsPerDollar";
            this.numCLPointsPerDollar.NumericType = Infragistics.Win.UltraWinEditors.NumericType.Double;
            this.numCLPointsPerDollar.Size = new System.Drawing.Size(148, 23);
            this.numCLPointsPerDollar.TabIndex = 3;
            this.numCLPointsPerDollar.Validated += new System.EventHandler(this.numBoxs_Validate);
            // 
            // cboTaxCodes
            // 
            this.cboTaxCodes.CheckedListSettings.CheckBoxStyle = Infragistics.Win.CheckStyle.CheckBox;
            this.cboTaxCodes.CheckedListSettings.ItemCheckArea = Infragistics.Win.ItemCheckArea.Item;
            this.cboTaxCodes.CheckedListSettings.ListSeparator = ", ";
            this.cboTaxCodes.Location = new System.Drawing.Point(130, 98);
            this.cboTaxCodes.Name = "cboTaxCodes";
            this.cboTaxCodes.Size = new System.Drawing.Size(434, 25);
            this.cboTaxCodes.TabIndex = 2;
            this.cboTaxCodes.AfterCloseUp += new System.EventHandler(this.cboTaxCodes_AfterCloseUp);
            this.cboTaxCodes.TextChanged += new System.EventHandler(this.cboTaxCodes_TextChanged);
            // 
            // btnDeptNote
            // 
            appearance14.BackColor = System.Drawing.Color.White;
            appearance14.ForeColor = System.Drawing.Color.Black;
            this.btnDeptNote.Appearance = appearance14;
            this.btnDeptNote.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Flat;
            this.btnDeptNote.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnDeptNote.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold);
            this.btnDeptNote.Location = new System.Drawing.Point(30, 0);
            this.btnDeptNote.Name = "btnDeptNote";
            this.btnDeptNote.Size = new System.Drawing.Size(150, 30);
            this.btnDeptNote.TabIndex = 5;
            this.btnDeptNote.TabStop = false;
            this.btnDeptNote.Text = "Department Note";
            this.btnDeptNote.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnDeptNote.Click += new System.EventHandler(this.btnDeptNote_Click);
            // 
            // ultraLabel1
            // 
            appearance18.ForeColor = System.Drawing.Color.White;
            appearance18.TextHAlignAsString = "Center";
            appearance18.TextVAlignAsString = "Middle";
            this.ultraLabel1.Appearance = appearance18;
            this.ultraLabel1.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultraLabel1.Location = new System.Drawing.Point(108, 53);
            this.ultraLabel1.Name = "ultraLabel1";
            this.ultraLabel1.Size = new System.Drawing.Size(11, 15);
            this.ultraLabel1.TabIndex = 38;
            this.ultraLabel1.Text = "*";
            // 
            // ultraLabel7
            // 
            appearance19.ForeColor = System.Drawing.Color.White;
            appearance19.TextHAlignAsString = "Center";
            appearance19.TextVAlignAsString = "Middle";
            this.ultraLabel7.Appearance = appearance19;
            this.ultraLabel7.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultraLabel7.Location = new System.Drawing.Point(108, 23);
            this.ultraLabel7.Name = "ultraLabel7";
            this.ultraLabel7.Size = new System.Drawing.Size(11, 15);
            this.ultraLabel7.TabIndex = 37;
            this.ultraLabel7.Text = "*";
            // 
            // ultraLabel2
            // 
            appearance20.ForeColor = System.Drawing.Color.White;
            appearance20.TextHAlignAsString = "Center";
            appearance20.TextVAlignAsString = "Middle";
            this.ultraLabel2.Appearance = appearance20;
            this.ultraLabel2.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultraLabel2.Location = new System.Drawing.Point(113, 30);
            this.ultraLabel2.Name = "ultraLabel2";
            this.ultraLabel2.Size = new System.Drawing.Size(11, 15);
            this.ultraLabel2.TabIndex = 39;
            this.ultraLabel2.Text = "*";
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.btnPriceValidation);
            this.groupBox2.Controls.Add(this.pnlSave);
            this.groupBox2.Controls.Add(this.pnlSubDepartments);
            this.groupBox2.Controls.Add(this.pnlClose);
            this.groupBox2.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.groupBox2.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.Location = new System.Drawing.Point(14, 360);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(577, 59);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            // 
            // btnSubDepartments
            // 
            appearance23.BackColor = System.Drawing.Color.White;
            appearance23.FontData.BoldAsString = "True";
            appearance23.ForeColor = System.Drawing.Color.Black;
            this.btnSubDepartments.Appearance = appearance23;
            this.btnSubDepartments.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Flat;
            this.btnSubDepartments.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnSubDepartments.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold);
            this.btnSubDepartments.Location = new System.Drawing.Point(30, 0);
            this.btnSubDepartments.Name = "btnSubDepartments";
            this.btnSubDepartments.Size = new System.Drawing.Size(130, 30);
            this.btnSubDepartments.TabIndex = 1;
            this.btnSubDepartments.TabStop = false;
            this.btnSubDepartments.Text = "Sub Department";
            this.btnSubDepartments.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnSubDepartments.Click += new System.EventHandler(this.btnSubDepartments_Click);
            // 
            // btnPriceValidation
            // 
            appearance21.BackColor = System.Drawing.Color.White;
            appearance21.FontData.BoldAsString = "True";
            appearance21.ForeColor = System.Drawing.Color.Black;
            this.btnPriceValidation.Appearance = appearance21;
            this.btnPriceValidation.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Flat;
            this.btnPriceValidation.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold);
            this.btnPriceValidation.Location = new System.Drawing.Point(11, 20);
            this.btnPriceValidation.Name = "btnPriceValidation";
            this.btnPriceValidation.Size = new System.Drawing.Size(130, 26);
            this.btnPriceValidation.TabIndex = 0;
            this.btnPriceValidation.TabStop = false;
            this.btnPriceValidation.Text = "Price Validation";
            this.btnPriceValidation.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnPriceValidation.Visible = false;
            this.btnPriceValidation.Click += new System.EventHandler(this.btnPriceValidation_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBox3.Controls.Add(this.ultraLabel3);
            this.groupBox3.Controls.Add(this.txtSaleDiscount);
            this.groupBox3.Controls.Add(this.ultraLabel2);
            this.groupBox3.Controls.Add(this.ultraLabel20);
            this.groupBox3.Controls.Add(this.dtpSaleStartDate);
            this.groupBox3.Controls.Add(this.ultraLabel21);
            this.groupBox3.Controls.Add(this.txtDeptSalePrice);
            this.groupBox3.Controls.Add(this.dtpSaleEndDate);
            this.groupBox3.Controls.Add(this.ultraLabel19);
            this.groupBox3.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.groupBox3.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox3.ForeColor = System.Drawing.Color.White;
            this.groupBox3.Location = new System.Drawing.Point(20, 242);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(565, 88);
            this.groupBox3.TabIndex = 2;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Department Sale";
            // 
            // ultraLabel3
            // 
            appearance26.FontData.BoldAsString = "False";
            appearance26.ForeColor = System.Drawing.Color.White;
            this.ultraLabel3.Appearance = appearance26;
            this.ultraLabel3.Location = new System.Drawing.Point(285, 58);
            this.ultraLabel3.Name = "ultraLabel3";
            this.ultraLabel3.Size = new System.Drawing.Size(86, 18);
            this.ultraLabel3.TabIndex = 41;
            this.ultraLabel3.Text = "Discount %";
            this.ultraLabel3.WrapText = false;
            // 
            // txtSaleDiscount
            // 
            appearance27.FontData.BoldAsString = "False";
            this.txtSaleDiscount.Appearance = appearance27;
            this.txtSaleDiscount.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.txtSaleDiscount.ButtonStyle = Infragistics.Win.UIElementButtonStyle.VisualStudio2005Button;
            this.txtSaleDiscount.FormatString = "###.00";
            this.txtSaleDiscount.Location = new System.Drawing.Point(410, 56);
            this.txtSaleDiscount.MaskClipMode = Infragistics.Win.UltraWinMaskedEdit.MaskMode.Raw;
            this.txtSaleDiscount.MaskDisplayMode = Infragistics.Win.UltraWinMaskedEdit.MaskMode.Raw;
            this.txtSaleDiscount.MaskInput = "nnn.nn";
            this.txtSaleDiscount.MaxValue = 100;
            this.txtSaleDiscount.MinValue = -1;
            this.txtSaleDiscount.Name = "txtSaleDiscount";
            this.txtSaleDiscount.NullText = "0";
            this.txtSaleDiscount.NumericType = Infragistics.Win.UltraWinEditors.NumericType.Double;
            this.txtSaleDiscount.Size = new System.Drawing.Size(148, 23);
            this.txtSaleDiscount.SpinButtonDisplayStyle = Infragistics.Win.ButtonDisplayStyle.Always;
            this.txtSaleDiscount.TabIndex = 3;
            this.txtSaleDiscount.TabNavigation = Infragistics.Win.UltraWinMaskedEdit.MaskedEditTabNavigation.NextControl;
            this.txtSaleDiscount.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.txtSaleDiscount.Enter += new System.EventHandler(this.txtSaleDiscount_Enter);
            this.txtSaleDiscount.Validated += new System.EventHandler(this.txtNumericBoxs_Validate);
            // 
            // pnlClose
            // 
            // 
            // pnlClose.ClientArea
            // 
            this.pnlClose.ClientArea.Controls.Add(this.btnClose);
            this.pnlClose.ClientArea.Controls.Add(this.lblClose);
            this.pnlClose.Location = new System.Drawing.Point(448, 18);
            this.pnlClose.Name = "pnlClose";
            this.pnlClose.Size = new System.Drawing.Size(120, 30);
            this.pnlClose.TabIndex = 13;
            // 
            // lblClose
            // 
            appearance25.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(138)))), ((int)(((byte)(31)))));
            appearance25.FontData.BoldAsString = "True";
            appearance25.ForeColor = System.Drawing.Color.White;
            appearance25.TextHAlignAsString = "Center";
            appearance25.TextVAlignAsString = "Middle";
            this.lblClose.Appearance = appearance25;
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
            // pnlDeptNote
            // 
            // 
            // pnlDeptNote.ClientArea
            // 
            this.pnlDeptNote.ClientArea.Controls.Add(this.btnDeptNote);
            this.pnlDeptNote.ClientArea.Controls.Add(this.lblDeptNote);
            this.pnlDeptNote.Location = new System.Drawing.Point(291, 126);
            this.pnlDeptNote.Name = "pnlDeptNote";
            this.pnlDeptNote.Size = new System.Drawing.Size(180, 30);
            this.pnlDeptNote.TabIndex = 70;
            this.pnlDeptNote.Visible = false;
            // 
            // lblDeptNote
            // 
            appearance15.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(138)))), ((int)(((byte)(31)))));
            appearance15.FontData.BoldAsString = "True";
            appearance15.ForeColor = System.Drawing.Color.White;
            appearance15.TextHAlignAsString = "Center";
            appearance15.TextVAlignAsString = "Middle";
            this.lblDeptNote.Appearance = appearance15;
            this.lblDeptNote.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblDeptNote.Font = new System.Drawing.Font("Verdana", 8F, System.Drawing.FontStyle.Bold);
            this.lblDeptNote.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblDeptNote.Location = new System.Drawing.Point(0, 0);
            this.lblDeptNote.Margin = new System.Windows.Forms.Padding(3, 3, 0, 3);
            this.lblDeptNote.Name = "lblDeptNote";
            this.lblDeptNote.Size = new System.Drawing.Size(30, 30);
            this.lblDeptNote.TabIndex = 0;
            this.lblDeptNote.Tag = "NOCOLOR";
            this.lblDeptNote.Text = "F6";
            this.lblDeptNote.Click += new System.EventHandler(this.btnDeptNote_Click);
            // 
            // pnlSubDepartments
            // 
            // 
            // pnlSubDepartments.ClientArea
            // 
            this.pnlSubDepartments.ClientArea.Controls.Add(this.btnSubDepartments);
            this.pnlSubDepartments.ClientArea.Controls.Add(this.lblSubDepartments);
            this.pnlSubDepartments.Location = new System.Drawing.Point(150, 18);
            this.pnlSubDepartments.Name = "pnlSubDepartments";
            this.pnlSubDepartments.Size = new System.Drawing.Size(160, 30);
            this.pnlSubDepartments.TabIndex = 71;
            this.pnlSubDepartments.Visible = false;
            // 
            // lblSubDepartments
            // 
            appearance24.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(138)))), ((int)(((byte)(31)))));
            appearance24.FontData.BoldAsString = "True";
            appearance24.ForeColor = System.Drawing.Color.White;
            appearance24.TextHAlignAsString = "Center";
            appearance24.TextVAlignAsString = "Middle";
            this.lblSubDepartments.Appearance = appearance24;
            this.lblSubDepartments.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblSubDepartments.Font = new System.Drawing.Font("Verdana", 8F, System.Drawing.FontStyle.Bold);
            this.lblSubDepartments.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblSubDepartments.Location = new System.Drawing.Point(0, 0);
            this.lblSubDepartments.Margin = new System.Windows.Forms.Padding(3, 3, 0, 3);
            this.lblSubDepartments.Name = "lblSubDepartments";
            this.lblSubDepartments.Size = new System.Drawing.Size(30, 30);
            this.lblSubDepartments.TabIndex = 0;
            this.lblSubDepartments.Tag = "NOCOLOR";
            this.lblSubDepartments.Text = "F2";
            this.lblSubDepartments.Click += new System.EventHandler(this.btnSubDepartments_Click);
            // 
            // pnlSave
            // 
            // 
            // pnlSave.ClientArea
            // 
            this.pnlSave.ClientArea.Controls.Add(this.btnSave);
            this.pnlSave.ClientArea.Controls.Add(this.lblSave);
            this.pnlSave.Location = new System.Drawing.Point(319, 18);
            this.pnlSave.Name = "pnlSave";
            this.pnlSave.Size = new System.Drawing.Size(120, 30);
            this.pnlSave.TabIndex = 72;
            // 
            // lblSave
            // 
            appearance22.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(138)))), ((int)(((byte)(31)))));
            appearance22.FontData.BoldAsString = "True";
            appearance22.ForeColor = System.Drawing.Color.White;
            appearance22.TextHAlignAsString = "Center";
            appearance22.TextVAlignAsString = "Middle";
            this.lblSave.Appearance = appearance22;
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
            // frmDepartment
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(108)))), ((int)(((byte)(172)))));
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(603, 431);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.lblTransactionType);
            this.Controls.Add(this.btnNew);
            this.Controls.Add(this.btnSearch);
            this.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.Name = "frmDepartment";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Department Information";
            this.Activated += new System.EventHandler(this.frmDepartment_Activated);
            this.Load += new System.EventHandler(this.frmDepartment_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmDepartment_KeyDown);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.frmDepartment_KeyUp);
            ((System.ComponentModel.ISupportInitialize)(this.txtDeptName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDeptCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDeptSalePrice)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDiscount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtpSaleStartDate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtpSaleEndDate)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numCLPointsPerDollar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboTaxCodes)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtSaleDiscount)).EndInit();
            this.pnlClose.ClientArea.ResumeLayout(false);
            this.pnlClose.ResumeLayout(false);
            this.pnlDeptNote.ClientArea.ResumeLayout(false);
            this.pnlDeptNote.ResumeLayout(false);
            this.pnlSubDepartments.ClientArea.ResumeLayout(false);
            this.pnlSubDepartments.ResumeLayout(false);
            this.pnlSave.ClientArea.ResumeLayout(false);
            this.pnlSave.ResumeLayout(false);
            this.ResumeLayout(false);

		}
		#endregion
		private bool Save()
		{
			try
			{
				if (oDepartmentRow.SalePrice > 0 && oDepartmentRow.SaleDiscount > 0)
                {
                    throw (new Exception("You can select either Sale Item Price or Sale Discount %."));
                }
                else
                {
                    int DeptID = 0;
                    oDepartmentRow.DeptCode = txtDeptCode.Text;

                    oBRDepartment.Persist(oDepartmentData, ref DeptID); //Sprint-22 20-Oct-2015 JY Added DeptID

                    PersistSelectedTaxCodes(DeptID);     //Sprint-22 20-Oct-2015 JY Added DeptID

                    return true;
                }
			}
			catch(POSExceptions exp)
			{
				clsUIHelper.ShowErrorMsg(exp.ErrMessage);
				switch (exp.ErrNumber)
				{
					case (long)POSErrorENUM.Department_DuplicateCode:
						txtDeptCode.Focus();
						break;
					case (long)POSErrorENUM.Department_NameCanNotBeNULL:
						txtDeptName.Focus();
						break;
					case (long)POSErrorENUM.Department_CodeCanNotBeNULL:
						txtDeptCode.Focus();
						break;
					case (long)POSErrorENUM.Department_SalePriceCanNotBeNULL:
						txtDeptName.Focus();
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

        private void PersistSelectedTaxCodes(int DeptID)     //Sprint-22 20-Oct-2015 JY Added DeptID
		{
			List<int> selectedTaxCodes = cboTaxCodes.CheckedItems.Select(checkedItem => int.Parse(checkedItem.DataValue.ToString())).ToList();
            //TaxCodeHelper.PersistDepartmentTaxCodes(selectedTaxCodes, oDepartmentData.Department[0].DeptID.ToString(CultureInfo.InvariantCulture));   //Sprint-22 20-Oct-2015 JY Commented
            if (DeptID == 0) DeptID = Configuration.convertNullToInt(oDepartmentData.Department[0].DeptID);
            TaxCodeHelper.PersistDepartmentTaxCodes(selectedTaxCodes, Configuration.convertNullToString(DeptID));  //Sprint-22 20-Oct-2015 JY Added 
		}

		private void btnSave_Click(object sender, System.EventArgs e)
		{
			if (ValidateData() && Save())
			{
				IsCanceled = false;
				this.Close();
			}
		}

		private bool ValidateData()
		{
			if (chkIsTaxable.Checked)
			{
				if (cboTaxCodes.CheckedItems.Count == 0)
				{
					clsUIHelper.ShowErrorMsg("\nPlease select Tax Code(s).");
					return false;
				}
			}

			return true;
		}

		private void txtBoxs_Validate(object sender, System.EventArgs e)
		{
			try
			{
				if (oDepartmentRow == null) 
					return ;
				Infragistics.Win.UltraWinEditors.UltraTextEditor txtEditor =  (Infragistics.Win.UltraWinEditors.UltraTextEditor)sender;
				switch(txtEditor.Name)
				{
					case "txtDeptCode":
						oDepartmentRow.DeptCode = txtDeptCode.Text;
						break;
					case "txtDeptName":
						oDepartmentRow.DeptName = txtDeptName.Text;
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
				if (oDepartmentRow == null) 
					return ;
				Infragistics.Win.UltraWinEditors.UltraNumericEditor txtEditor =  (Infragistics.Win.UltraWinEditors.UltraNumericEditor)sender;
				switch(txtEditor.Name)
				{
					case "txtDeptSalePrice":
						oDepartmentRow.SalePrice= Decimal.Parse(this.txtDeptSalePrice.Value.ToString());
						break;
					case "txtDiscount":
						oDepartmentRow.Discount= Decimal.Parse(this.txtDiscount.Value.ToString());
						break;
                    case "txtSaleDiscount":
                        oDepartmentRow.SaleDiscount = Decimal.Parse(this.txtSaleDiscount.Value.ToString());
                        break;
				}
			}
			catch(Exception exp)
			{
				clsUIHelper.ShowErrorMsg(exp.Message);
			}
		}

		private void txtDateBoxs_Validate(object sender, System.EventArgs e)
		{
			try
			{
				if (oDepartmentRow == null) 
					return ;
				Infragistics.Win.UltraWinSchedule.UltraCalendarCombo txtEditor =  (Infragistics.Win.UltraWinSchedule.UltraCalendarCombo)sender;
				switch(txtEditor.Name)
				{
					case "dtpSaleStartDate":
						oDepartmentRow.SaleStartDate= (System.DateTime)this.dtpSaleStartDate.Value;
						break;
					case "dtpSaleEndDate":
						oDepartmentRow.SaleEndDate= (System.DateTime)this.dtpSaleEndDate.Value;
						break;
				}
                //Following Edited by Krishna on 1 April 2011 for date validation
                if (oDepartmentRow.SaleStartDate.Date > oDepartmentRow.SaleEndDate.Date)
                {
                    //MessageBox.Show("Start Date cannot be greater than End date", "ERROR");
                    clsUIHelper.ShowErrorMsg("Start Date cannot be greater than End date");
                    dtpSaleStartDate.Focus();
                }
			}
			catch(Exception exp)
			{
				clsUIHelper.ShowErrorMsg(exp.Message);
			}
		}

		private void chkIsTaxable_CheckedChanged(object sender, System.EventArgs e)
		{
			try
			{
				if (oDepartmentRow == null)
					return;

				oDepartmentRow.IsTaxable = chkIsTaxable.Checked;

				if (chkIsTaxable.Checked)
				{
					cboTaxCodes.Enabled = true;
				}
				else
				{
					cboTaxCodes.Enabled = false;
				}
			}
			catch(Exception exp)
			{
				clsUIHelper.ShowErrorMsg(exp.Message);
			}
		}

		private void btnSearch_Click(object sender, System.EventArgs e)
		{
			Search();
		}

		private void Search()
		{
			try
			{
                //frmSearch oSearch = new frmSearch(clsPOSDBConstants.Department_tbl);
                frmSearchMain oSearch = new frmSearchMain(true);    //20-Dec-2017 JY Added new reference
                oSearch.SearchTable = clsPOSDBConstants.Department_tbl; //20-Dec-2017 JY Added 
                oSearch.ShowDialog(this);
				if (!oSearch.IsCanceled)
				{
					string strCode=oSearch.SelectedRowID();
					if (strCode == "") 
						return;
					
					Edit(strCode);
				}
			}
			catch(Exception exp)
			{
				clsUIHelper.ShowErrorMsg(exp.Message);
			}
		}

		private void SearchTaxCode()
		{
			//Commented by Shrikant Mali, on 04/09/2014 as searching tac code is not longer required.
			//try
			//{

			//	frmSearch oSearch = new frmSearch(clsPOSDBConstants.TaxCodes_tbl);
			//	oSearch.ShowDialog(this);
			//	if (!oSearch.IsCanceled)
			//	{
			//		string strCode=oSearch.SelectedRowID();
			//		if (strCode == "") 
			//			return;
					
			//		DisplayTaxCodes(strCode);
			//	}
			//}
			//catch(Exception exp)
			//{
			//	clsUIHelper.ShowErrorMsg(exp.Message);
			//}
		}

		private void Display()
		{
			txtDeptCode.Text = oDepartmentRow.DeptCode;
			txtDeptName.Text = oDepartmentRow.DeptName;
			txtDeptSalePrice.Text = oDepartmentRow.SalePrice.ToString();
			txtDiscount.Text = oDepartmentRow.Discount.ToString();
            txtSaleDiscount.Text = oDepartmentRow.SaleDiscount.ToString();
			//txtItemCodes.Text = oDepartmentRow.TaxCode;

			dtpSaleEndDate.Value = oDepartmentRow.SaleEndDate;
			dtpSaleStartDate.Value = oDepartmentRow.SaleStartDate;
			chkIsTaxable.Checked = oDepartmentRow.IsTaxable;

            this.btnPriceValidation.Visible = true;
            pnlSubDepartments.Visible = true;
			this.pnlDeptNote.Visible =
				UserPriviliges.IsUserHasPriviliges(UserPriviliges.Modules.Notes.ID,
					UserPriviliges.Screens.DepartmentNotes.ID);//Added by Krishna on 10 October 2011

			if (UserPriviliges.IsUserHasPriviliges(UserPriviliges.Modules.InventoryMgmt.ID,
				UserPriviliges.Screens.MinimumItemPrice.ID))
			{
				this.btnPriceValidation.Enabled = true;
			}
			else
			{
				this.btnPriceValidation.Enabled = false;
			}
			
			cboTaxCodes.Enabled = oDepartmentRow.IsTaxable;
            this.numCLPointsPerDollar.Value = oDepartmentRow.PointsPerDollar; //Sprint-18 - 2041 27-Oct-2014 JY  Added
			SetTaxCodeChecked(TaxCodeHelper.FetchDeparmentTaxInfo(oDepartmentRow.DeptID));

		}

		private void SetTaxCodeChecked(ItemTaxData itemTaxInfo)
		{
			foreach (ValueListItem item in cboTaxCodes.Items)
			{
				int value = Convert.ToInt32((item.DataValue));
				foreach (DataRow row in itemTaxInfo.ItemTaxTable.Rows)
				{
					int taxCode = int.Parse(row[clsPOSDBConstants.ItemTaxTable_TaxIdColumnName].ToString());

					if (value == taxCode)
						item.CheckState = CheckState.Checked;
				}
			}
		}

		private void DisplayTaxCodes(string TaxCode)
		{
			//try
			//{
			//	TaxCodes oBRTaxCodes = new TaxCodes();
			//	TaxCodesRow oTaxCodesRow ;
			//	TaxCodesData oTaxCodesData = oBRTaxCodes.Populate(TaxCode);
			//	if (oTaxCodesData.TaxCodes.Rows.Count>0)
			//		oTaxCodesRow = (TaxCodesRow) oTaxCodesData.TaxCodes.Rows[0];
			//	else
			//		oTaxCodesRow=null;
				
			//	if (oTaxCodesRow!= null ) 
			//	{
			//		oDepartmentRow.TaxId = oTaxCodesRow.TaxID;
			//		this.uLblTaxDescription.Text= oTaxCodesRow.Description;
			//		txtItemCodes.Text = oTaxCodesRow.TaxCode;
			//	}
			//	else
			//	{
			//		oDepartmentRow.TaxId = 0;
			//		txtItemCodes.Text = "";
			//		this.uLblTaxDescription.Text = "";
			//	}

			//}
			//catch(Exception exp)
			//{
			//	clsUIHelper.ShowErrorMsg(exp.Message);
			//}
		}
		public void Edit(string DepartmentCode)
		{
			try
			{
				txtDeptCode.Enabled = false;
				oDepartmentData = oBRDepartment.Populate(DepartmentCode);
				oDepartmentRow = oDepartmentData.Department.GetRowByID(DepartmentCode);

				if (oDepartmentRow != null)
				{
					Display();
				}
			}
			catch(Exception exp)
			{
				clsUIHelper.ShowErrorMsg(exp.Message);
			}
		}

		private void SetNew()
		{
			oBRDepartment = new Department();
			oDepartmentData = new DepartmentData();

			Clear();
			oDepartmentRow = oDepartmentData.Department.AddRow(0,"","",0,0,false,System.DateTime.Now,System.DateTime.Now,0,0,0);
		}

		private void Clear()
		{
			txtDeptName.Text = "";
			txtDeptSalePrice.Value = 0;
			//this.uLblTaxDescription.Text = "";
			txtDiscount.Value = 0;
            txtSaleDiscount.Value = 0;
			//txtItemCodes.Text = "";
			dtpSaleStartDate.Value = System.DateTime.Now;
			dtpSaleEndDate.Value = System.DateTime.Now;
			chkIsTaxable.Checked  = false;
			txtDeptCode.Enabled = true;
            pnlSubDepartments.Visible = false;
//numCLPointsPerDollar
			if (oDepartmentData != null) oDepartmentData.Department.Rows.Clear();
		}

		private void txtDeptCode_Leave(object sender, System.EventArgs e)
		{
			try
			{
				SearchDepartment();
			}
			catch(Exception exp)
			{
				clsUIHelper.ShowErrorMsg(exp.Message);
			}
		}

		private void SearchDepartment()
		{
/*			if (txtDeptCode.Text==null || txtDeptCode.Text.Trim()=="")
				return;
			try
			{
				if (Edit(txtDeptCode.Text)==false)
				{
					SetNew();
					oDepartmentRow.DeptCode =  txtDeptCode.Text ;
				}
			}
			catch(Exception exp)
			{
				clsUIHelper.ShowErrorMsg( exp.Message);
			} */
		}

		private void btnNew_Click(object sender, System.EventArgs e)
		{
			try
			{
				txtDeptCode.Text = "";
				SetNew();
			}
			catch(Exception exp)
			{
				clsUIHelper.ShowErrorMsg(exp.Message);
			}
		}
		private void txtDeptCode_EditorButtonClick(object sender, Infragistics.Win.UltraWinEditors.EditorButtonEventArgs e)
		{
			Search();
		}

		private void txtTaxCode_EditorButtonClick(object sender, Infragistics.Win.UltraWinEditors.EditorButtonEventArgs e)
		{
			// Commented by Shrikant as this control is no more used.
			//SearchTaxCode();
		}

		private void txtTaxCode_Leave(object sender, System.EventArgs e)
		{
			try
			{
				// Commented by Shrikant Mali on 04/07/2014 as this technique is no longer used to display the tax codes.
				//taxCodesSearch();
			}
			catch(Exception exp)
			{
				clsUIHelper.ShowErrorMsg(exp.Message);
			}
		}

		private void taxCodesSearch()
		{
			//if (txtItemCodes.Text != null && this.txtItemCodes.Text.Trim() != "")
			//	DisplayTaxCodes(txtItemCodes.Text);
		}

		private void btnClose_Click(object sender, System.EventArgs e)
		{
			IsCanceled = true;
			this.Close();
		}

		private void txtDeptSalePrice_Enter(object sender, System.EventArgs e)
		{
			this.txtDeptSalePrice.SelectionStart=0;
			this.txtDeptSalePrice.SelectionLength=this.txtDeptSalePrice.MaxValue.ToString().Length;
		}

		private void txtDiscount_Enter(object sender, System.EventArgs e)
		{
			this.txtDiscount.SelectionStart=0;
			this.txtDiscount.SelectionLength=this.txtDeptSalePrice.MaskInput.ToString().Length;
		}

		private void frmDepartment_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			try
			{
                if (e.KeyData == Keys.F2 && pnlSubDepartments.Visible == true)
                {
                    btnSubDepartments_Click(btnSubDepartments, new EventArgs());
                }
				else if (e.KeyData==System.Windows.Forms.Keys.F4)
				{
					if ( txtDeptCode.ContainsFocus)
						this.Search();
					//Commented by Shrikant Mali as the following control and a techniue is no longer used to fill the tax codes.
					//else if (txtTaxCode.ContainsFocus)
					//	this.SearchTaxCode();
				}
			}
			catch(Exception exp)
			{
				clsUIHelper.ShowErrorMsg(exp.Message);
			}
		}

		private void frmDepartment_Load(object sender, System.EventArgs e)
		{
			this.txtDeptCode.AfterEnterEditMode += new System.EventHandler(clsUIHelper.AfterEnterEditMode);			
			this.txtDeptCode.AfterExitEditMode += new System.EventHandler(clsUIHelper.AfterExitEditMode);			
			this.txtDeptName.AfterEnterEditMode += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
			this.txtDeptName.AfterExitEditMode += new System.EventHandler(clsUIHelper.AfterExitEditMode);
			this.txtDeptSalePrice.AfterEnterEditMode += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
			this.txtDeptSalePrice.AfterExitEditMode += new System.EventHandler(clsUIHelper.AfterExitEditMode);

			this.txtDiscount.AfterEnterEditMode += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
			this.txtDiscount.AfterExitEditMode += new System.EventHandler(clsUIHelper.AfterExitEditMode);

            this.txtSaleDiscount.AfterEnterEditMode += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
            this.txtSaleDiscount.AfterExitEditMode += new System.EventHandler(clsUIHelper.AfterExitEditMode);

			//this.txtItemCodes.AfterEnterEditMode += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
			//this.txtItemCodes.AfterExitEditMode += new System.EventHandler(clsUIHelper.AfterExitEditMode);

			this.dtpSaleStartDate.Enter += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
			this.dtpSaleStartDate.Leave += new System.EventHandler(clsUIHelper.AfterExitEditMode);

			this.dtpSaleEndDate.Enter += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
			this.dtpSaleEndDate.Leave += new System.EventHandler(clsUIHelper.AfterExitEditMode);

			//this.chkIsTaxable.Enter += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
			//this.chkIsTaxable.Leave += new System.EventHandler(clsUIHelper.AfterExitEditMode);
			IsCanceled = true;
			clsUIHelper.setColorSchecme(this);
            btnClose.Appearance.BackColor = Color.FromArgb(255, System.Convert.ToByte(POS_Core.Resources.Configuration.arrCloseBk[0].ToString()), System.Convert.ToByte(POS_Core.Resources.Configuration.arrCloseBk[1]), System.Convert.ToByte(POS_Core.Resources.Configuration.arrCloseBk[2]));
            btnClose.Appearance.BackColor2 = Color.FromArgb(255, System.Convert.ToByte(POS_Core.Resources.Configuration.arrCloseBk[0].ToString()), System.Convert.ToByte(POS_Core.Resources.Configuration.arrCloseBk[1]), System.Convert.ToByte(POS_Core.Resources.Configuration.arrCloseBk[2]));
        }

		private void FillTaxInformation()
		{
			DataTable taxCodeDataTable = TaxCodeHelper.GetTaxCodeDataTable();

			cboTaxCodes.DataSource = taxCodeDataTable;
			cboTaxCodes.ValueMember = taxCodeDataTable.Columns[0].ColumnName;
			cboTaxCodes.DisplayMember = taxCodeDataTable.Columns[1].ColumnName;
		}

		private void frmDepartment_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
		{
		
			try
			{
				if(e.KeyData==System.Windows.Forms.Keys.Enter)
				{
					this.SelectNextControl(this.ActiveControl,true,true,true,true);
				}
                //Added By Shitaljit(QuicSolv) 0n 10 oct 2011 
                if (e.KeyData == Keys.F6 && pnlDeptNote.Visible == true)
                { 
                    btnDeptNote_Click(null,null);
                }
                //Added By Shitaljit(QuicSolv) 0n 10 oct 2011 
			}
			catch(Exception exp)
			{
				clsUIHelper.ShowErrorMsg(exp.Message);
			}	
		}

		private void frmDepartment_Activated(object sender, System.EventArgs e)
		{
			clsUIHelper.CurrentForm=this;
		}

        private void btnPriceValidation_Click(object sender, EventArgs e)
        {
            try
            {
                if (oDepartmentRow.DeptID== 0)
                    return;
                frmItemPriceValid ofrm = new frmItemPriceValid(oDepartmentRow.DeptID);
                ofrm.ShowDialog(frmMain.getInstance());
            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }

        private void txtSaleDiscount_Enter(object sender, EventArgs e)
        {
            this.txtSaleDiscount.SelectionStart = 0;
            this.txtSaleDiscount.SelectionLength = this.txtSaleDiscount.MaskInput.ToString().Length;
        }

        private void btnSubDepartments_Click(object sender, EventArgs e)
        {
            if (this.oDepartmentRow.DeptID > 0)
            {
                frmSubDepartment ofrm = new frmSubDepartment(this.oDepartmentRow.DeptID);
                ofrm.Show(this);
            }
        }
        //Added By Shitaljit(QuicSolv) 0n 8 oct 2011 
        private void btnDeptNote_Click(object sender, EventArgs e)
        {
            frmCustomerNotes oFrmCustNotes = new frmCustomerNotes(this.oDepartmentRow.DeptID.ToString(), clsPOSDBConstants.Department_tbl, clsEntityType.DepartmentNote);
            oFrmCustNotes.ShowDialog();
        }

        private void ultraLabel14_Click(object sender, EventArgs e)
        {

        }

        private void ultraLabel11_Click(object sender, EventArgs e)
        {

        }

        private void ultraLabel12_Click(object sender, EventArgs e)
        {

        }

        private void ultraLabel18_Click(object sender, EventArgs e)
        {

        }

		private void cboTaxCodes_AfterCloseUp(object sender, EventArgs e)
		{
			cboTaxCodes.Text = TaxCodeHelper.GetTrimmedTaxCodes(cboTaxCodes.CheckedItems);
		}

		private void cboTaxCodes_TextChanged(object sender, EventArgs e)
		{
			cboTaxCodes.Text = TaxCodeHelper.GetTrimmedTaxCodes(cboTaxCodes.CheckedItems);
		}

        //Sprint-18 - 2041 26-Oct-2014 JY  Added
        private void numBoxs_Validate(object sender, EventArgs e)
        {
            try
            {
                System.Decimal numvalue = 0;
                if (oDepartmentRow == null)
                    return;
                Infragistics.Win.UltraWinEditors.UltraNumericEditor numEditor = (Infragistics.Win.UltraWinEditors.UltraNumericEditor)sender;
                
                if (numEditor.NumericType == Infragistics.Win.UltraWinEditors.NumericType.Double)
                    numvalue = Decimal.Parse(numEditor.Value.ToString());
                else
                    numvalue = (int)numEditor.Value;

                switch (numEditor.Name)
                {
                    case "numCLPointsPerDollar":
                       oDepartmentRow.PointsPerDollar = Configuration.convertNullToInt(numvalue);
                       break;
                }
            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }
	}
}
