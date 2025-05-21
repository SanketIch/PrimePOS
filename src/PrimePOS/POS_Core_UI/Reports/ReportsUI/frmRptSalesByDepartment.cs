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
//using POS.UI;
using POS_Core_UI.Reports.Reports;
using System.Data;
using Infragistics.Win.UltraWinGrid;
using System.Collections.Generic;
using POS_Core.DataAccess;
//using POS_Core.DataAccess;
using System.Linq;
using POS_Core.Resources;
using CrystalDecisions.CrystalReports.Engine;

namespace POS_Core_UI.Reports.ReportsUI
{
    /// <summary>
    /// Summary description for frmViewTransaction.
    /// </summary>
    public class frmRptSalesByDepartment : System.Windows.Forms.Form, ICommandLIneTaskControl
    {
        private System.Windows.Forms.GroupBox groupBox1;
        private Infragistics.Win.Misc.UltraLabel ultraLabel12;
        private Infragistics.Win.Misc.UltraLabel ultraLabel20;
        private Infragistics.Win.Misc.UltraLabel ultraLabel19;
        private Infragistics.Win.UltraWinSchedule.UltraCalendarCombo dtpSaleEndDate;
        private Infragistics.Win.UltraWinSchedule.UltraCalendarCombo dtpSaleStartDate;
        private System.Windows.Forms.RadioButton optSummary;
        private System.Windows.Forms.RadioButton optDetail;
        private System.Windows.Forms.GroupBox groupBox2;
        private Infragistics.Win.Misc.UltraButton btnPrint;
        private Infragistics.Win.Misc.UltraButton btnView;
        private Infragistics.Win.Misc.UltraButton btnClose;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor txtDepartment;
        private UltraTextEditor txtSubDepartment;
        private Infragistics.Win.Misc.UltraLabel lblSubDepartment;
        public UltraTextEditor txtCustomer;
        private Infragistics.Win.Misc.UltraLabel ultraLabel1;
        private Infragistics.Win.Misc.UltraLabel lblCustomerName;
        private Infragistics.Win.Misc.UltraLabel ultraLabel5;
        private Infragistics.Win.Misc.UltraLabel ultraLabel4;
        private UltraNumericEditor txtTopProducts;
        CustomerRow oCustomerRow = null;
		private RadioButton optSummeryItem;
		private Infragistics.Win.Misc.UltraLabel ultraLabel2;
		private Infragistics.Win.Misc.UltraButton btnPayTypeList;
		private GroupBox grpPayTypeList;
		private CheckBox chkSelectAll;
		private DataGridView dataGridList;
		private DataGridViewCheckBoxColumn chkBox;
		private UltraTextEditor txtPaymentType;
        private Infragistics.Win.Misc.UltraButton btnSearch;
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;

        #region Sprint-19 - 2157 18-Feb-2015 JY Added
        private DataSet dsDept;
        private SearchSvr oSearchSvr = new SearchSvr();
        private int CurrentX;
        private UltraGrid grdDept;
        private UltraCheckEditor chkOnlyDiscountedTrans;
        private Infragistics.Win.Misc.UltraLabel ultraLabel3;
        private UltraTextEditor txtUserID;
        private int CurrentY;
        #endregion

        public frmRptSalesByDepartment()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();
            //
            // TODO: Add any constructor code after InitializeComponent call
            //
            this.customControl = new usrDateRangeParams();  //PRIMEPOS-2485 02-Apr-2021 JY Added
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
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmRptSalesByDepartment));
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance7 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance8 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance9 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance10 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinSchedule.CalendarCombo.DateButton dateButton1 = new Infragistics.Win.UltraWinSchedule.CalendarCombo.DateButton();
            Infragistics.Win.UltraWinSchedule.CalendarCombo.DateButton dateButton2 = new Infragistics.Win.UltraWinSchedule.CalendarCombo.DateButton();
            Infragistics.Win.Appearance appearance11 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance12 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance13 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinEditors.EditorButton editorButton1 = new Infragistics.Win.UltraWinEditors.EditorButton();
            Infragistics.Win.Appearance appearance14 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinEditors.EditorButton editorButton2 = new Infragistics.Win.UltraWinEditors.EditorButton();
            Infragistics.Win.Appearance appearance15 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance16 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinEditors.EditorButton editorButton3 = new Infragistics.Win.UltraWinEditors.EditorButton();
            Infragistics.Win.Appearance appearance17 = new Infragistics.Win.Appearance();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            Infragistics.Win.Appearance appearance18 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance19 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance20 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance21 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand1 = new Infragistics.Win.UltraWinGrid.UltraGridBand("Band 0", -1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn4 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Column Name");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn5 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Type");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn6 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Criteria Value");
            Infragistics.Win.Appearance appearance22 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance23 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance24 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance25 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance26 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance27 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance28 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance29 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance30 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance31 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance32 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance33 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance34 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance35 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance36 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance37 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance38 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance39 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance40 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinScrollBar.ScrollBarLook scrollBarLook1 = new Infragistics.Win.UltraWinScrollBar.ScrollBarLook();
            Infragistics.Win.Appearance appearance41 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance42 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance43 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance44 = new Infragistics.Win.Appearance();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.ultraLabel3 = new Infragistics.Win.Misc.UltraLabel();
            this.txtUserID = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.chkOnlyDiscountedTrans = new Infragistics.Win.UltraWinEditors.UltraCheckEditor();
            this.optSummeryItem = new System.Windows.Forms.RadioButton();
            this.btnSearch = new Infragistics.Win.Misc.UltraButton();
            this.optDetail = new System.Windows.Forms.RadioButton();
            this.btnPayTypeList = new Infragistics.Win.Misc.UltraButton();
            this.ultraLabel2 = new Infragistics.Win.Misc.UltraLabel();
            this.lblCustomerName = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabel5 = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabel4 = new Infragistics.Win.Misc.UltraLabel();
            this.txtTopProducts = new Infragistics.Win.UltraWinEditors.UltraNumericEditor();
            this.ultraLabel1 = new Infragistics.Win.Misc.UltraLabel();
            this.lblSubDepartment = new Infragistics.Win.Misc.UltraLabel();
            this.dtpSaleEndDate = new Infragistics.Win.UltraWinSchedule.UltraCalendarCombo();
            this.dtpSaleStartDate = new Infragistics.Win.UltraWinSchedule.UltraCalendarCombo();
            this.ultraLabel20 = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabel19 = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabel12 = new Infragistics.Win.Misc.UltraLabel();
            this.txtDepartment = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.txtSubDepartment = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.txtCustomer = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.txtPaymentType = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.optSummary = new System.Windows.Forms.RadioButton();
            this.grpPayTypeList = new System.Windows.Forms.GroupBox();
            this.chkSelectAll = new System.Windows.Forms.CheckBox();
            this.dataGridList = new System.Windows.Forms.DataGridView();
            this.chkBox = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnPrint = new Infragistics.Win.Misc.UltraButton();
            this.btnClose = new Infragistics.Win.Misc.UltraButton();
            this.btnView = new Infragistics.Win.Misc.UltraButton();
            this.grdDept = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtUserID)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkOnlyDiscountedTrans)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTopProducts)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtpSaleEndDate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtpSaleStartDate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDepartment)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSubDepartment)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCustomer)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPaymentType)).BeginInit();
            this.grpPayTypeList.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridList)).BeginInit();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdDept)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.ultraLabel3);
            this.groupBox1.Controls.Add(this.txtUserID);
            this.groupBox1.Controls.Add(this.chkOnlyDiscountedTrans);
            this.groupBox1.Controls.Add(this.optSummeryItem);
            this.groupBox1.Controls.Add(this.btnSearch);
            this.groupBox1.Controls.Add(this.optDetail);
            this.groupBox1.Controls.Add(this.btnPayTypeList);
            this.groupBox1.Controls.Add(this.ultraLabel2);
            this.groupBox1.Controls.Add(this.lblCustomerName);
            this.groupBox1.Controls.Add(this.ultraLabel5);
            this.groupBox1.Controls.Add(this.ultraLabel4);
            this.groupBox1.Controls.Add(this.txtTopProducts);
            this.groupBox1.Controls.Add(this.ultraLabel1);
            this.groupBox1.Controls.Add(this.lblSubDepartment);
            this.groupBox1.Controls.Add(this.dtpSaleEndDate);
            this.groupBox1.Controls.Add(this.dtpSaleStartDate);
            this.groupBox1.Controls.Add(this.ultraLabel20);
            this.groupBox1.Controls.Add(this.ultraLabel19);
            this.groupBox1.Controls.Add(this.ultraLabel12);
            this.groupBox1.Controls.Add(this.txtDepartment);
            this.groupBox1.Controls.Add(this.txtSubDepartment);
            this.groupBox1.Controls.Add(this.txtCustomer);
            this.groupBox1.Controls.Add(this.txtPaymentType);
            this.groupBox1.Controls.Add(this.optSummary);
            this.groupBox1.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.groupBox1.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(5, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(998, 120);
            this.groupBox1.TabIndex = 27;
            this.groupBox1.TabStop = false;
            // 
            // ultraLabel3
            // 
            appearance1.ForeColor = System.Drawing.Color.White;
            this.ultraLabel3.Appearance = appearance1;
            this.ultraLabel3.AutoSize = true;
            this.ultraLabel3.Location = new System.Drawing.Point(216, 93);
            this.ultraLabel3.Name = "ultraLabel3";
            this.ultraLabel3.Size = new System.Drawing.Size(206, 18);
            this.ultraLabel3.TabIndex = 39;
            this.ultraLabel3.Text = "User ID <Blank = Any User>";
            // 
            // txtUserID
            // 
            this.txtUserID.AutoSize = false;
            this.txtUserID.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.txtUserID.Location = new System.Drawing.Point(476, 91);
            this.txtUserID.MaxLength = 20;
            this.txtUserID.Name = "txtUserID";
            this.txtUserID.Size = new System.Drawing.Size(123, 23);
            this.txtUserID.TabIndex = 7;
            this.txtUserID.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // chkOnlyDiscountedTrans
            // 
            appearance2.ForeColor = System.Drawing.Color.White;
            this.chkOnlyDiscountedTrans.Appearance = appearance2;
            this.chkOnlyDiscountedTrans.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkOnlyDiscountedTrans.Location = new System.Drawing.Point(5, 92);
            this.chkOnlyDiscountedTrans.Name = "chkOnlyDiscountedTrans";
            this.chkOnlyDiscountedTrans.Size = new System.Drawing.Size(185, 20);
            this.chkOnlyDiscountedTrans.TabIndex = 3;
            this.chkOnlyDiscountedTrans.Text = "Only Discounted Trans";
            // 
            // optSummeryItem
            // 
            this.optSummeryItem.AutoSize = true;
            this.optSummeryItem.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.optSummeryItem.ForeColor = System.Drawing.Color.White;
            this.optSummeryItem.Location = new System.Drawing.Point(766, 68);
            this.optSummeryItem.Name = "optSummeryItem";
            this.optSummeryItem.Size = new System.Drawing.Size(152, 21);
            this.optSummeryItem.TabIndex = 33;
            this.optSummeryItem.Text = "Summery By Item";
            // 
            // btnSearch
            // 
            appearance3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(62)))), ((int)(((byte)(176)))));
            appearance3.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance3.FontData.BoldAsString = "True";
            appearance3.ForeColor = System.Drawing.Color.White;
            this.btnSearch.Appearance = appearance3;
            this.btnSearch.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            this.btnSearch.Location = new System.Drawing.Point(918, 65);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(74, 26);
            this.btnSearch.TabIndex = 12;
            this.btnSearch.Text = "&Search";
            this.btnSearch.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // optDetail
            // 
            this.optDetail.AutoSize = true;
            this.optDetail.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.optDetail.ForeColor = System.Drawing.Color.White;
            this.optDetail.Location = new System.Drawing.Point(702, 68);
            this.optDetail.Name = "optDetail";
            this.optDetail.Size = new System.Drawing.Size(64, 21);
            this.optDetail.TabIndex = 11;
            this.optDetail.Text = "Detail";
            // 
            // btnPayTypeList
            // 
            appearance4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(62)))), ((int)(((byte)(176)))));
            appearance4.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance4.FontData.BoldAsString = "True";
            appearance4.ForeColor = System.Drawing.Color.White;
            appearance4.Image = ((object)(resources.GetObject("appearance4.Image")));
            this.btnPayTypeList.Appearance = appearance4;
            this.btnPayTypeList.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            this.btnPayTypeList.Location = new System.Drawing.Point(773, 15);
            this.btnPayTypeList.Name = "btnPayTypeList";
            this.btnPayTypeList.Size = new System.Drawing.Size(144, 23);
            this.btnPayTypeList.TabIndex = 8;
            this.btnPayTypeList.Text = "Pay Types";
            this.btnPayTypeList.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnPayTypeList.Click += new System.EventHandler(this.btnPayTypeList_Click);
            this.btnPayTypeList.KeyDown += new System.Windows.Forms.KeyEventHandler(this.butPayTypeList_KeyDown);
            this.btnPayTypeList.KeyUp += new System.Windows.Forms.KeyEventHandler(this.butPayTypeList_KeyUp);
            // 
            // ultraLabel2
            // 
            appearance5.ForeColor = System.Drawing.Color.White;
            this.ultraLabel2.Appearance = appearance5;
            this.ultraLabel2.AutoSize = true;
            this.ultraLabel2.Location = new System.Drawing.Point(605, 17);
            this.ultraLabel2.Name = "ultraLabel2";
            this.ultraLabel2.Size = new System.Drawing.Size(178, 18);
            this.ultraLabel2.TabIndex = 37;
            this.ultraLabel2.Text = "Pay Types <Blank = All>";
            // 
            // lblCustomerName
            // 
            appearance6.ForeColor = System.Drawing.Color.Black;
            this.lblCustomerName.Appearance = appearance6;
            this.lblCustomerName.BackColorInternal = System.Drawing.Color.Transparent;
            this.lblCustomerName.Location = new System.Drawing.Point(274, 255);
            this.lblCustomerName.Name = "lblCustomerName";
            this.lblCustomerName.Size = new System.Drawing.Size(125, 25);
            this.lblCustomerName.TabIndex = 30;
            this.lblCustomerName.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // ultraLabel5
            // 
            appearance7.ForeColor = System.Drawing.Color.White;
            this.ultraLabel5.Appearance = appearance7;
            this.ultraLabel5.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultraLabel5.Location = new System.Drawing.Point(134, 67);
            this.ultraLabel5.Name = "ultraLabel5";
            this.ultraLabel5.Size = new System.Drawing.Size(74, 21);
            this.ultraLabel5.TabIndex = 32;
            this.ultraLabel5.Text = "Products";
            // 
            // ultraLabel4
            // 
            appearance8.ForeColor = System.Drawing.Color.White;
            this.ultraLabel4.Appearance = appearance8;
            this.ultraLabel4.AutoSize = true;
            this.ultraLabel4.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultraLabel4.Location = new System.Drawing.Point(5, 68);
            this.ultraLabel4.Name = "ultraLabel4";
            this.ultraLabel4.Size = new System.Drawing.Size(30, 18);
            this.ultraLabel4.TabIndex = 31;
            this.ultraLabel4.Text = "Top";
            // 
            // txtTopProducts
            // 
            this.txtTopProducts.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.txtTopProducts.DisplayStyle = Infragistics.Win.EmbeddableElementDisplayStyle.OfficeXP;
            this.txtTopProducts.Location = new System.Drawing.Point(85, 65);
            this.txtTopProducts.MaskInput = "nnnn";
            this.txtTopProducts.MinValue = 0;
            this.txtTopProducts.Name = "txtTopProducts";
            this.txtTopProducts.Size = new System.Drawing.Size(43, 23);
            this.txtTopProducts.TabIndex = 2;
            // 
            // ultraLabel1
            // 
            appearance9.ForeColor = System.Drawing.Color.White;
            this.ultraLabel1.Appearance = appearance9;
            this.ultraLabel1.AutoSize = true;
            this.ultraLabel1.Location = new System.Drawing.Point(216, 68);
            this.ultraLabel1.Name = "ultraLabel1";
            this.ultraLabel1.Size = new System.Drawing.Size(245, 18);
            this.ultraLabel1.TabIndex = 28;
            this.ultraLabel1.Text = "Customer Code<Blank=Any Cust>";
            // 
            // lblSubDepartment
            // 
            appearance10.ForeColor = System.Drawing.Color.White;
            this.lblSubDepartment.Appearance = appearance10;
            this.lblSubDepartment.AutoSize = true;
            this.lblSubDepartment.Location = new System.Drawing.Point(216, 42);
            this.lblSubDepartment.Name = "lblSubDepartment";
            this.lblSubDepartment.Size = new System.Drawing.Size(256, 18);
            this.lblSubDepartment.TabIndex = 23;
            this.lblSubDepartment.Text = "Sub Dept<Blank=Ignore Sub Dept>";
            // 
            // dtpSaleEndDate
            // 
            this.dtpSaleEndDate.AllowNull = false;
            this.dtpSaleEndDate.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Office2003ToolbarButton;
            this.dtpSaleEndDate.DateButtons.Add(dateButton1);
            this.dtpSaleEndDate.Location = new System.Drawing.Point(85, 40);
            this.dtpSaleEndDate.Name = "dtpSaleEndDate";
            this.dtpSaleEndDate.NonAutoSizeHeight = 10;
            this.dtpSaleEndDate.Size = new System.Drawing.Size(123, 22);
            this.dtpSaleEndDate.TabIndex = 1;
            this.dtpSaleEndDate.Tag = "To Date";
            this.dtpSaleEndDate.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.dtpSaleEndDate.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.dtpSaleEndDate.Value = new System.DateTime(2004, 5, 25, 0, 0, 0, 0);
            // 
            // dtpSaleStartDate
            // 
            this.dtpSaleStartDate.AllowNull = false;
            this.dtpSaleStartDate.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Office2003ToolbarButton;
            this.dtpSaleStartDate.DateButtons.Add(dateButton2);
            this.dtpSaleStartDate.Location = new System.Drawing.Point(85, 15);
            this.dtpSaleStartDate.Name = "dtpSaleStartDate";
            this.dtpSaleStartDate.NonAutoSizeHeight = 10;
            this.dtpSaleStartDate.Size = new System.Drawing.Size(123, 22);
            this.dtpSaleStartDate.TabIndex = 0;
            this.dtpSaleStartDate.Tag = "From Date";
            this.dtpSaleStartDate.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.dtpSaleStartDate.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.dtpSaleStartDate.Value = new System.DateTime(2004, 5, 25, 0, 0, 0, 0);
            // 
            // ultraLabel20
            // 
            appearance11.ForeColor = System.Drawing.Color.White;
            this.ultraLabel20.Appearance = appearance11;
            this.ultraLabel20.AutoSize = true;
            this.ultraLabel20.Location = new System.Drawing.Point(5, 17);
            this.ultraLabel20.Name = "ultraLabel20";
            this.ultraLabel20.Size = new System.Drawing.Size(77, 18);
            this.ultraLabel20.TabIndex = 20;
            this.ultraLabel20.Text = "Start Date";
            // 
            // ultraLabel19
            // 
            appearance12.ForeColor = System.Drawing.Color.White;
            this.ultraLabel19.Appearance = appearance12;
            this.ultraLabel19.AutoSize = true;
            this.ultraLabel19.Location = new System.Drawing.Point(5, 42);
            this.ultraLabel19.Name = "ultraLabel19";
            this.ultraLabel19.Size = new System.Drawing.Size(68, 18);
            this.ultraLabel19.TabIndex = 22;
            this.ultraLabel19.Text = "End Date";
            // 
            // ultraLabel12
            // 
            appearance13.ForeColor = System.Drawing.Color.White;
            this.ultraLabel12.Appearance = appearance13;
            this.ultraLabel12.AutoSize = true;
            this.ultraLabel12.Location = new System.Drawing.Point(216, 17);
            this.ultraLabel12.Name = "ultraLabel12";
            this.ultraLabel12.Size = new System.Drawing.Size(223, 18);
            this.ultraLabel12.TabIndex = 6;
            this.ultraLabel12.Text = "Department<Blank=Any Dept>";
            // 
            // txtDepartment
            // 
            this.txtDepartment.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance14.BackColor = System.Drawing.Color.White;
            appearance14.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(231)))), ((int)(((byte)(230)))));
            appearance14.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance14.BorderAlpha = Infragistics.Win.Alpha.Transparent;
            appearance14.Image = ((object)(resources.GetObject("appearance14.Image")));
            appearance14.ImageAlpha = Infragistics.Win.Alpha.Opaque;
            appearance14.ImageBackgroundAlpha = Infragistics.Win.Alpha.Opaque;
            appearance14.ImageBackgroundStyle = Infragistics.Win.ImageBackgroundStyle.Centered;
            editorButton1.Appearance = appearance14;
            editorButton1.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            editorButton1.Text = "";
            editorButton1.Width = 20;
            this.txtDepartment.ButtonsRight.Add(editorButton1);
            this.txtDepartment.Location = new System.Drawing.Point(476, 15);
            this.txtDepartment.Name = "txtDepartment";
            this.txtDepartment.ReadOnly = true;
            this.txtDepartment.Size = new System.Drawing.Size(123, 23);
            this.txtDepartment.TabIndex = 4;
            this.txtDepartment.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.txtDepartment.EditorButtonClick += new Infragistics.Win.UltraWinEditors.EditorButtonEventHandler(this.txtDepartment_EditorButtonClick);
            this.txtDepartment.TextChanged += new System.EventHandler(this.txtDepartment_TextChanged);
            // 
            // txtSubDepartment
            // 
            this.txtSubDepartment.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance15.BackColor = System.Drawing.Color.White;
            appearance15.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(231)))), ((int)(((byte)(230)))));
            appearance15.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance15.BorderAlpha = Infragistics.Win.Alpha.Transparent;
            appearance15.Image = ((object)(resources.GetObject("appearance15.Image")));
            appearance15.ImageAlpha = Infragistics.Win.Alpha.Opaque;
            appearance15.ImageBackgroundAlpha = Infragistics.Win.Alpha.Opaque;
            appearance15.ImageBackgroundStyle = Infragistics.Win.ImageBackgroundStyle.Centered;
            editorButton2.Appearance = appearance15;
            editorButton2.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            editorButton2.Text = "";
            editorButton2.Width = 20;
            this.txtSubDepartment.ButtonsRight.Add(editorButton2);
            this.txtSubDepartment.Location = new System.Drawing.Point(476, 40);
            this.txtSubDepartment.Name = "txtSubDepartment";
            this.txtSubDepartment.ReadOnly = true;
            this.txtSubDepartment.Size = new System.Drawing.Size(123, 23);
            this.txtSubDepartment.TabIndex = 5;
            this.txtSubDepartment.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.txtSubDepartment.EditorButtonClick += new Infragistics.Win.UltraWinEditors.EditorButtonEventHandler(this.txtSubDepartment_EditorButtonClick);
            this.txtSubDepartment.Click += new System.EventHandler(this.txtSubDepartment_Click);
            // 
            // txtCustomer
            // 
            appearance16.BorderColor = System.Drawing.Color.Lime;
            appearance16.BorderColor3DBase = System.Drawing.Color.Lime;
            this.txtCustomer.Appearance = appearance16;
            this.txtCustomer.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance17.BackColor = System.Drawing.Color.White;
            appearance17.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(231)))), ((int)(((byte)(230)))));
            appearance17.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance17.BorderAlpha = Infragistics.Win.Alpha.Transparent;
            appearance17.Image = ((object)(resources.GetObject("appearance17.Image")));
            appearance17.ImageAlpha = Infragistics.Win.Alpha.Opaque;
            appearance17.ImageBackgroundAlpha = Infragistics.Win.Alpha.Opaque;
            appearance17.ImageBackgroundStyle = Infragistics.Win.ImageBackgroundStyle.Centered;
            editorButton3.Appearance = appearance17;
            editorButton3.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            editorButton3.Text = "";
            editorButton3.Width = 20;
            this.txtCustomer.ButtonsRight.Add(editorButton3);
            this.txtCustomer.Location = new System.Drawing.Point(476, 65);
            this.txtCustomer.Name = "txtCustomer";
            this.txtCustomer.Size = new System.Drawing.Size(123, 23);
            this.txtCustomer.TabIndex = 6;
            this.txtCustomer.TabStop = false;
            this.txtCustomer.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.txtCustomer.EditorButtonClick += new Infragistics.Win.UltraWinEditors.EditorButtonEventHandler(this.txtCustomer_EditorButtonClick);
            this.txtCustomer.Leave += new System.EventHandler(this.txtCustomer_Leave);
            // 
            // txtPaymentType
            // 
            this.txtPaymentType.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.txtPaymentType.Location = new System.Drawing.Point(605, 40);
            this.txtPaymentType.Name = "txtPaymentType";
            this.txtPaymentType.ReadOnly = true;
            this.txtPaymentType.Size = new System.Drawing.Size(312, 23);
            this.txtPaymentType.TabIndex = 9;
            this.txtPaymentType.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.txtPaymentType.EditorButtonClick += new Infragistics.Win.UltraWinEditors.EditorButtonEventHandler(this.txtPaymentType_EditorButtonClick);
            // 
            // optSummary
            // 
            this.optSummary.AutoSize = true;
            this.optSummary.Checked = true;
            this.optSummary.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.optSummary.ForeColor = System.Drawing.Color.White;
            this.optSummary.Location = new System.Drawing.Point(606, 68);
            this.optSummary.Name = "optSummary";
            this.optSummary.Size = new System.Drawing.Size(92, 21);
            this.optSummary.TabIndex = 10;
            this.optSummary.TabStop = true;
            this.optSummary.Text = "Summary";
            // 
            // grpPayTypeList
            // 
            this.grpPayTypeList.Controls.Add(this.chkSelectAll);
            this.grpPayTypeList.Controls.Add(this.dataGridList);
            this.grpPayTypeList.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpPayTypeList.Location = new System.Drawing.Point(774, 32);
            this.grpPayTypeList.Name = "grpPayTypeList";
            this.grpPayTypeList.Size = new System.Drawing.Size(144, 61);
            this.grpPayTypeList.TabIndex = 35;
            this.grpPayTypeList.TabStop = false;
            this.grpPayTypeList.Visible = false;
            // 
            // chkSelectAll
            // 
            this.chkSelectAll.AutoSize = true;
            this.chkSelectAll.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkSelectAll.Location = new System.Drawing.Point(6, 38);
            this.chkSelectAll.Name = "chkSelectAll";
            this.chkSelectAll.Size = new System.Drawing.Size(87, 17);
            this.chkSelectAll.TabIndex = 1;
            this.chkSelectAll.Text = "Select All";
            this.chkSelectAll.UseVisualStyleBackColor = true;
            this.chkSelectAll.CheckedChanged += new System.EventHandler(this.chkSelectAll_CheckedChanged);
            // 
            // dataGridList
            // 
            this.dataGridList.AllowDrop = true;
            this.dataGridList.AllowUserToAddRows = false;
            this.dataGridList.AllowUserToDeleteRows = false;
            this.dataGridList.AllowUserToResizeColumns = false;
            this.dataGridList.AllowUserToResizeRows = false;
            this.dataGridList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridList.ColumnHeadersVisible = false;
            this.dataGridList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.chkBox});
            this.dataGridList.Location = new System.Drawing.Point(0, 6);
            this.dataGridList.Name = "dataGridList";
            this.dataGridList.RowHeadersVisible = false;
            this.dataGridList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridList.Size = new System.Drawing.Size(144, 21);
            this.dataGridList.TabIndex = 0;
            this.dataGridList.Visible = false;
            this.dataGridList.RowsAdded += new System.Windows.Forms.DataGridViewRowsAddedEventHandler(this.dataGridList_RowsAdded);
            // 
            // chkBox
            // 
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
            dataGridViewCellStyle1.NullValue = false;
            this.chkBox.DefaultCellStyle = dataGridViewCellStyle1;
            this.chkBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chkBox.HeaderText = " ";
            this.chkBox.Name = "chkBox";
            this.chkBox.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.chkBox.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.chkBox.Width = 20;
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.btnPrint);
            this.groupBox2.Controls.Add(this.btnClose);
            this.groupBox2.Controls.Add(this.btnView);
            this.groupBox2.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.groupBox2.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.Location = new System.Drawing.Point(5, 410);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(998, 47);
            this.groupBox2.TabIndex = 30;
            this.groupBox2.TabStop = false;
            // 
            // btnPrint
            // 
            this.btnPrint.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            appearance18.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(62)))), ((int)(((byte)(176)))));
            appearance18.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance18.FontData.BoldAsString = "True";
            appearance18.ForeColor = System.Drawing.Color.White;
            appearance18.Image = ((object)(resources.GetObject("appearance18.Image")));
            this.btnPrint.Appearance = appearance18;
            this.btnPrint.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            this.btnPrint.Location = new System.Drawing.Point(723, 13);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(85, 26);
            this.btnPrint.TabIndex = 6;
            this.btnPrint.Text = "&Print";
            this.btnPrint.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            appearance19.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(62)))), ((int)(((byte)(176)))));
            appearance19.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance19.FontData.BoldAsString = "True";
            appearance19.ForeColor = System.Drawing.Color.White;
            appearance19.Image = ((object)(resources.GetObject("appearance19.Image")));
            this.btnClose.Appearance = appearance19;
            this.btnClose.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(907, 14);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(85, 26);
            this.btnClose.TabIndex = 7;
            this.btnClose.Text = "&Close";
            this.btnClose.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnView
            // 
            this.btnView.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            appearance20.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(62)))), ((int)(((byte)(176)))));
            appearance20.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance20.FontData.BoldAsString = "True";
            appearance20.ForeColor = System.Drawing.Color.White;
            appearance20.Image = ((object)(resources.GetObject("appearance20.Image")));
            this.btnView.Appearance = appearance20;
            this.btnView.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            this.btnView.Location = new System.Drawing.Point(815, 14);
            this.btnView.Name = "btnView";
            this.btnView.Size = new System.Drawing.Size(85, 26);
            this.btnView.TabIndex = 5;
            this.btnView.Text = "&View";
            this.btnView.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnView.Click += new System.EventHandler(this.btnView_Click);
            // 
            // grdDept
            // 
            this.grdDept.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            appearance21.BackColor = System.Drawing.Color.White;
            appearance21.BackColor2 = System.Drawing.Color.White;
            appearance21.BackColorDisabled = System.Drawing.Color.White;
            appearance21.BackColorDisabled2 = System.Drawing.Color.White;
            appearance21.BackGradientStyle = Infragistics.Win.GradientStyle.None;
            this.grdDept.DisplayLayout.Appearance = appearance21;
            ultraGridColumn4.Header.VisiblePosition = 0;
            ultraGridColumn5.Header.VisiblePosition = 1;
            ultraGridColumn6.Header.VisiblePosition = 2;
            ultraGridBand1.Columns.AddRange(new object[] {
            ultraGridColumn4,
            ultraGridColumn5,
            ultraGridColumn6});
            ultraGridBand1.HeaderVisible = true;
            this.grdDept.DisplayLayout.BandsSerializer.Add(ultraGridBand1);
            this.grdDept.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.grdDept.DisplayLayout.InterBandSpacing = 10;
            this.grdDept.DisplayLayout.MaxColScrollRegions = 1;
            this.grdDept.DisplayLayout.MaxRowScrollRegions = 1;
            appearance22.BackColor = System.Drawing.Color.White;
            appearance22.BackColor2 = System.Drawing.Color.White;
            this.grdDept.DisplayLayout.Override.ActiveCardCaptionAppearance = appearance22;
            appearance23.BackColor = System.Drawing.Color.White;
            appearance23.BackColor2 = System.Drawing.Color.White;
            appearance23.BorderColor = System.Drawing.Color.Gray;
            this.grdDept.DisplayLayout.Override.ActiveRowAppearance = appearance23;
            appearance24.BackColor = System.Drawing.Color.White;
            appearance24.BackColor2 = System.Drawing.Color.White;
            appearance24.BorderColor = System.Drawing.Color.Gray;
            this.grdDept.DisplayLayout.Override.AddRowAppearance = appearance24;
            this.grdDept.DisplayLayout.Override.AllowAddNew = Infragistics.Win.UltraWinGrid.AllowAddNew.No;
            this.grdDept.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.False;
            this.grdDept.DisplayLayout.Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.False;
            appearance25.BackColor = System.Drawing.Color.Transparent;
            this.grdDept.DisplayLayout.Override.CardAreaAppearance = appearance25;
            appearance26.BackColor = System.Drawing.Color.White;
            appearance26.BackColor2 = System.Drawing.Color.White;
            appearance26.BackColorDisabled = System.Drawing.Color.White;
            appearance26.BackColorDisabled2 = System.Drawing.Color.White;
            appearance26.BorderColor = System.Drawing.Color.Black;
            appearance26.BorderColor3DBase = System.Drawing.Color.Black;
            this.grdDept.DisplayLayout.Override.CellAppearance = appearance26;
            appearance27.BackColor = System.Drawing.Color.White;
            appearance27.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(231)))), ((int)(((byte)(230)))));
            appearance27.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance27.BorderAlpha = Infragistics.Win.Alpha.Transparent;
            appearance27.BorderColor = System.Drawing.Color.Gray;
            appearance27.BorderColor3DBase = System.Drawing.Color.Gray;
            appearance27.ImageBackgroundAlpha = Infragistics.Win.Alpha.Transparent;
            appearance27.ImageBackgroundStyle = Infragistics.Win.ImageBackgroundStyle.Centered;
            this.grdDept.DisplayLayout.Override.CellButtonAppearance = appearance27;
            appearance28.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            appearance28.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.grdDept.DisplayLayout.Override.EditCellAppearance = appearance28;
            appearance29.BorderColor = System.Drawing.Color.Gray;
            this.grdDept.DisplayLayout.Override.FilteredInRowAppearance = appearance29;
            appearance30.BorderColor = System.Drawing.Color.Gray;
            this.grdDept.DisplayLayout.Override.FilteredOutRowAppearance = appearance30;
            appearance31.BackColor = System.Drawing.Color.White;
            appearance31.BackColor2 = System.Drawing.Color.White;
            appearance31.BackColorDisabled = System.Drawing.Color.White;
            appearance31.BackColorDisabled2 = System.Drawing.Color.White;
            this.grdDept.DisplayLayout.Override.FixedCellAppearance = appearance31;
            appearance32.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            appearance32.BackColor2 = System.Drawing.Color.Beige;
            this.grdDept.DisplayLayout.Override.FixedHeaderAppearance = appearance32;
            appearance33.BackColor = System.Drawing.Color.White;
            appearance33.BackColor2 = System.Drawing.SystemColors.Control;
            appearance33.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance33.FontData.BoldAsString = "True";
            appearance33.ForeColor = System.Drawing.Color.Black;
            appearance33.TextHAlignAsString = "Left";
            appearance33.ThemedElementAlpha = Infragistics.Win.Alpha.Transparent;
            this.grdDept.DisplayLayout.Override.HeaderAppearance = appearance33;
            appearance34.BorderColor = System.Drawing.Color.Gray;
            this.grdDept.DisplayLayout.Override.RowAlternateAppearance = appearance34;
            appearance35.BackColor = System.Drawing.Color.White;
            appearance35.BackColor2 = System.Drawing.Color.White;
            appearance35.BackGradientStyle = Infragistics.Win.GradientStyle.None;
            appearance35.BackHatchStyle = Infragistics.Win.BackHatchStyle.None;
            appearance35.BorderColor = System.Drawing.Color.Gray;
            this.grdDept.DisplayLayout.Override.RowAppearance = appearance35;
            appearance36.BorderColor = System.Drawing.Color.Gray;
            this.grdDept.DisplayLayout.Override.RowPreviewAppearance = appearance36;
            appearance37.BackColor = System.Drawing.Color.White;
            appearance37.BackColor2 = System.Drawing.SystemColors.Control;
            appearance37.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance37.BorderColor = System.Drawing.Color.Gray;
            this.grdDept.DisplayLayout.Override.RowSelectorAppearance = appearance37;
            this.grdDept.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            this.grdDept.DisplayLayout.Override.RowSelectorWidth = 12;
            this.grdDept.DisplayLayout.Override.RowSpacingBefore = 2;
            appearance38.BackColor = System.Drawing.Color.Navy;
            appearance38.BackColorAlpha = Infragistics.Win.Alpha.Opaque;
            this.grdDept.DisplayLayout.Override.SelectedCellAppearance = appearance38;
            appearance39.BackColor = System.Drawing.Color.Navy;
            appearance39.BackColorDisabled = System.Drawing.Color.Navy;
            appearance39.BackGradientStyle = Infragistics.Win.GradientStyle.None;
            appearance39.BackHatchStyle = Infragistics.Win.BackHatchStyle.None;
            appearance39.BorderColor = System.Drawing.Color.Gray;
            appearance39.ForeColor = System.Drawing.Color.Black;
            this.grdDept.DisplayLayout.Override.SelectedRowAppearance = appearance39;
            this.grdDept.DisplayLayout.Override.SelectTypeCell = Infragistics.Win.UltraWinGrid.SelectType.None;
            this.grdDept.DisplayLayout.Override.SelectTypeCol = Infragistics.Win.UltraWinGrid.SelectType.None;
            this.grdDept.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.Single;
            appearance40.BorderColor = System.Drawing.Color.Gray;
            this.grdDept.DisplayLayout.Override.TemplateAddRowAppearance = appearance40;
            this.grdDept.DisplayLayout.RowConnectorColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            this.grdDept.DisplayLayout.RowConnectorStyle = Infragistics.Win.UltraWinGrid.RowConnectorStyle.Solid;
            appearance41.BackColor = System.Drawing.Color.White;
            appearance41.BackColor2 = System.Drawing.SystemColors.Control;
            appearance41.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            scrollBarLook1.Appearance = appearance41;
            appearance42.BackColor = System.Drawing.Color.White;
            appearance42.BackColor2 = System.Drawing.SystemColors.Control;
            appearance42.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance42.ThemedElementAlpha = Infragistics.Win.Alpha.Transparent;
            scrollBarLook1.ButtonAppearance = appearance42;
            appearance43.BackColor = System.Drawing.Color.White;
            appearance43.BackColor2 = System.Drawing.SystemColors.Control;
            scrollBarLook1.ThumbAppearance = appearance43;
            appearance44.BackColor = System.Drawing.Color.White;
            appearance44.BackColor2 = System.Drawing.Color.White;
            scrollBarLook1.TrackAppearance = appearance44;
            this.grdDept.DisplayLayout.ScrollBarLook = scrollBarLook1;
            this.grdDept.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.grdDept.Location = new System.Drawing.Point(5, 126);
            this.grdDept.Name = "grdDept";
            this.grdDept.Size = new System.Drawing.Size(997, 289);
            this.grdDept.TabIndex = 0;
            this.grdDept.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.grdDept.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.grdDept_InitializeLayout);
            this.grdDept.BeforeRowExpanded += new Infragistics.Win.UltraWinGrid.CancelableRowEventHandler(this.grdDept_BeforeRowExpanded);
            this.grdDept.DoubleClick += new System.EventHandler(this.grdDept_DoubleClick);
            this.grdDept.MouseMove += new System.Windows.Forms.MouseEventHandler(this.grdDept_MouseMove);
            // 
            // frmRptSalesByDepartment
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(108)))), ((int)(((byte)(172)))));
            this.ClientSize = new System.Drawing.Size(1008, 462);
            this.Controls.Add(this.grpPayTypeList);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.grdDept);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.Name = "frmRptSalesByDepartment";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Sales By Department";
            this.Load += new System.EventHandler(this.frmViewTransaction_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmViewTransaction_KeyDown);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtUserID)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkOnlyDiscountedTrans)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTopProducts)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtpSaleEndDate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtpSaleStartDate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDepartment)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSubDepartment)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCustomer)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPaymentType)).EndInit();
            this.grpPayTypeList.ResumeLayout(false);
            this.grpPayTypeList.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridList)).EndInit();
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdDept)).EndInit();
            this.ResumeLayout(false);

        }
        #endregion

        private void frmViewTransaction_Load(object sender, System.EventArgs e)
        {
            this.dtpSaleStartDate.Enter += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
            this.dtpSaleStartDate.Leave += new System.EventHandler(clsUIHelper.AfterExitEditMode);

            this.dtpSaleEndDate.Enter += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
            this.dtpSaleEndDate.Leave += new System.EventHandler(clsUIHelper.AfterExitEditMode);

            this.txtDepartment.AfterEnterEditMode += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
            this.txtDepartment.AfterExitEditMode += new System.EventHandler(clsUIHelper.AfterExitEditMode);
            //Added By Shitaljit(QuicSolv) on 3 June 2011
            this.txtSubDepartment.AfterEnterEditMode += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
            this.txtSubDepartment.AfterExitEditMode += new System.EventHandler(clsUIHelper.AfterExitEditMode);
            txtDepartment.Tag = "";
            //Till Here Added By Shitaljit
            /*this.optDetail.Enter+= new System.EventHandler(clsUIHelper.AfterEnterEditMode);
			this.optDetail.Leave+= new System.EventHandler(clsUIHelper.AfterExitEditMode);

			this.optSummary.Enter+= new System.EventHandler(clsUIHelper.AfterEnterEditMode);
			this.optSummary.Leave+= new System.EventHandler(clsUIHelper.AfterExitEditMode);
*/
            this.txtUserID.AfterEnterEditMode += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
            this.txtUserID.AfterExitEditMode += new System.EventHandler(clsUIHelper.AfterExitEditMode);
            this.Left = (frmMain.getInstance().Width - frmMain.getInstance().ultraExplorerBar1.Width - this.Width) / 2;
            this.Top = (frmMain.getInstance().Height - this.Height) / 2;

            clsUIHelper.setColorSchecme(this);
            this.dtpSaleEndDate.Value = DateTime.Now;
            this.dtpSaleStartDate.Value = DateTime.Now;
            this.optSummary.Checked = true;

			FillPayType();    //Added by Ashutosh on 06/02/2014 to add new filter pay type 
            #region Sprint-19 - 2157 18-Feb-2015 JY Added
            this.Location = new Point(30, 2);
            clsUIHelper.SetAppearance(this.grdDept);
            clsUIHelper.SetReadonlyRow(this.grdDept);
            grdDept.DisplayLayout.Override.HeaderClickAction = HeaderClickAction.SortMulti;
            grdDept.DisplayLayout.AutoFitStyle = AutoFitStyle.ResizeAllColumns;
            btnSearch_Click(sender, e);
            #endregion
        }

        private void btnView_Click(object sender, System.EventArgs e)
        {
            PreviewReport(false);
        }

        private void btnClose_Click(object sender, System.EventArgs e)
        {
            this.Close();
        }

        private void frmViewTransaction_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            try
            {
                if (e.KeyData == System.Windows.Forms.Keys.Enter)
                {
                    this.SelectNextControl(this.ActiveControl, true, true, true, true);
                }
                else if (e.KeyData == Keys.Escape)
                    this.Close();
                else if (e.KeyData == System.Windows.Forms.Keys.F4)
                {
                    if (this.txtDepartment.ContainsFocus == true)
                    {
                        this.SearchDept();
                    }
                    //Added By Shitaljit(QuicSolv) on 3 june 2011
                    if (this.txtSubDepartment.ContainsFocus == true)
                    {
                        this.SearchSubDept();
                    }
                    //Added By Shitaljit(QuicSolv) on 22 May 2014 for F4 press on txtCustomerCode
                    if (this.txtCustomer.ContainsFocus == true)
                    {
                        SearchCustomer();
                    }
                    //Till here Added By Shitaljit(QuicSolv)
                }
            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }

        #region Sprint-19 - 2157 19-Feb-2015 JY commented
        //private void PreviewReport(bool blnPrint)
        //{
        //    try
        //    {
        //        this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
        //        rptSalesByDepartment oRpt = new rptSalesByDepartment();

        //        String strQuery = "";
        //        //ORIGINAL Commented By Shitaljit on 3 June 2011
        //        //strQuery = "select Dept.DeptName ,PTD.ItemID ,PTD.ItemDescription as Description ,sum(PTD.Qty) as Qty,sum(PTD.ExtendedPrice-PTD.Discount+PTD.TaxAmount) as Amount " +
        //        //" ,'" + this.dtpSaleStartDate.Text + "' as StartDate, '" + this.dtpSaleEndDate.Text + "' as EndDate " +
        //        //" from POSTransaction PT join POSTransactionDetail PTD on (pt.transid=ptd.transid ) " +
        //        //" join Item I on (I.ItemID=PTD.ITemID ) left outer join Department Dept on (Dept.DeptID=I.DepartmentID  ) " +
        //        //" where convert(datetime,PT.TransDate,109) between convert(datetime, cast('" + this.dtpSaleStartDate.Text + " 00:00:00' as datetime) ,113) and convert(datetime, cast('" + this.dtpSaleEndDate.Text + " 23:59:59' as datetime) ,113) ";

        //        //if (this.txtDepartment.Text.Trim() != "" && txtDepartment.Tag != null && txtDepartment.Tag.ToString().Trim().Length > 0)
        //        //{
        //        //    strQuery += " and Dept.DeptCode in (" + this.txtDepartment.Tag.ToString().Trim() + ")";
        //        //}
        //        //START of Modified By Shitaljit(QuicSolv) on 3 June

        //        /// Date : 07/02/2014  
        //        /// Modified by Ashutosh
        //        /// To add new filter PayType
        //        /// 
        //        string isinternalPaytype = "";
        //        string isexternalPaytype = "";
        //        string strQuerygroupby = "";

        //        if (this.txtPaymentType.Tag.ToString().Trim() == "")
        //        {
        //            isinternalPaytype = "";
        //            isexternalPaytype = "";
        //        }
        //        else
        //        {
        //            isinternalPaytype = " and PP1.TransTypeCode in (" + txtPaymentType.Tag.ToString().Trim() + ") ";
        //            isexternalPaytype = " and PP.TransTypeCode in (" + txtPaymentType.Tag.ToString().Trim() + ") ";
        //        }
        //        // End//
        //        string sTopItem = (Resources.Configuration.convertNullToInt(this.txtTopProducts.Value)) > 0 ? " TOP "+ Resources.Configuration.convertNullToString(this.txtTopProducts.Value) : "";
        //        if (string.IsNullOrEmpty(this.txtSubDepartment.Text) == true)
        //        {
        //            rptSalesByDepartment oRptDept = new rptSalesByDepartment();

        //            /// Date 03/02/2014
        //            /// MOdified by Ashutosh
        //            /// Add transdate in query and Add PayType parameter in Query
        //            /// old Code
				   
        //            /// strQuery = "select " + sTopItem + " PT.TransID,  Dept.DeptName , " + clsPOSDBConstants.Customer_Fld_CustomerName + "+', '+" + clsPOSDBConstants.Customer_Fld_FirstName + " as CustName," +
        //            ///" PTD.ItemID ,PTD.ItemDescription as Description ,sum(PTD.Qty) as Qty,sum(PTD.ExtendedPrice-ptd.Discount+PTD.TaxAmount) as Amount ,SUM(ptd.Discount) as DiscountAmt" +
        //            ///" ,'" + this.dtpSaleStartDate.Text + "' as StartDate, '" + this.dtpSaleEndDate.Text + "' as EndDate, " +
        //            ///" SUM(ptd.Discount) as DiscountAmt,TotalDiscAmount  " +
        //            ///" from Customer CT join  POSTransaction PT on (CT.CustomerID = PT.CustomerID) join POSTransactionDetail PTD on (pt.transid=ptd.transid ) " +
        //            ///" join Item I on (I.ItemID=PTD.ITemID ) left outer join Department Dept on (Dept.DeptID=I.DepartmentID  ) " +
        //            ///" where convert(datetime,PT.TransDate,109) between convert(datetime, cast('" + this.dtpSaleStartDate.Text + " 00:00:00' as datetime) ,113) and convert(datetime, cast('" + this.dtpSaleEndDate.Text + " 23:59:59' as datetime) ,113) ";
        //            /// new code
        //            if (this.optSummeryItem.Checked == true)
        //            {
        //                string InventryQuery = "";
        //                if (this.txtDepartment.Tag.ToString().Trim() == "")
        //                {
        //                    InventryQuery = "Select SUM(PT1.TotalDiscAmount) from POSTransaction PT1,  POSTransPayment PP1  where PP1.TransID = PT1.TransID and " +
        //                    " convert(datetime,PT1.TransDate,109)  between convert(datetime, cast('" + dtpSaleStartDate.Text + " 00:00:00'  as datetime) ,113) and "+
        //                    " convert(datetime, cast('" + dtpSaleEndDate.Text + " 23:59:59' as datetime) ,113)   " + isinternalPaytype +"  ";
        //                }
        //                else
        //                {
        //                    InventryQuery = "Select SUM(PT1.TotalDiscAmount) from POSTransaction PT1, Department D1, POSTransactionDetail PTD1," +
        //                   " Item I1, POSTransPayment PP1  where  PP1.TransID = PT1.TransID and  convert(datetime,PT1.TransDate,109) between "+
        //                   "convert(datetime, cast('" + dtpSaleStartDate.Text + " 00:00:00' as datetime) ,113)  and  "+
        //                   "convert(datetime, cast('" + dtpSaleEndDate.Text + " 23:59:59' as datetime) ,113) " +
        //                   " and PT1.TransID = PTD1.TransID and PTD1.ItemID = I1.ItemID and I1.DepartmentID = D1.DeptID and  " +
        //                   "  D1.DeptCode in (" + this.txtDepartment.Tag.ToString().Trim() + ")  "+isinternalPaytype+"  ";
        //                }
        //                if (string.IsNullOrEmpty(txtCustomer.Text) == true)
        //                {
        //                    strQuery = "select " + sTopItem + "  Dept.DeptName , PTD.ItemID ," +
        //                   "PTD.ItemDescription as Description, sum(PTD.Qty) as Qty,sum(PTD.ExtendedPrice-ptd.Discount+PTD.TaxAmount) as Amount ," +
        //                   "SUM(ptd.Discount) as DiscountAmt,'" + this.dtpSaleStartDate.Text + "' as StartDate, '" + this.dtpSaleEndDate.Text + "' as EndDate, " +
        //                   " SUM(ptd.Discount) as DiscountAmt,("+InventryQuery+") AS TotalDiscAmount   from  POSTransaction PT " +
        //                   " join POSTransactionDetail PTD on (pt.transid=ptd.transid ) join Item I on (I.ItemID=PTD.ITemID )" +
        //                   "  left outer join Department Dept on (Dept.DeptID=I.DepartmentID  ) , POSTransPayment PP  " +
        //                   " where convert(datetime,PT.TransDate,109) between convert(datetime, cast('" + this.dtpSaleStartDate.Text + " 00:00:00' as datetime) ,113) " +
        //                   "and convert(datetime, cast('" + this.dtpSaleEndDate.Text + " 23:59:59' as datetime) ,113) and PP.TransID = PT.TransID  "+
        //                   " "+isexternalPaytype+" ";
        //                    strQuerygroupby = " group by PTD.ItemID,PTD.ItemDescription,Dept.DeptName,Dept.DeptCode";
        //                }
        //                else 
        //                {
        //                    strQuery = "select " + sTopItem + "    Dept.DeptName ," +
        //                   " " + clsPOSDBConstants.Customer_Fld_CustomerName + "+', '+" + clsPOSDBConstants.Customer_Fld_FirstName + " as CustName,PTD.ItemID ," +
        //                   "PTD.ItemDescription as Description, sum(PTD.Qty) as Qty,sum(PTD.ExtendedPrice-ptd.Discount+PTD.TaxAmount) as Amount ," +
        //                   "SUM(ptd.Discount) as DiscountAmt,'" + this.dtpSaleStartDate.Text + " 00:00:00' as StartDate, '" + this.dtpSaleEndDate.Text + " 23:59:59' as EndDate, " +
        //                   " SUM(ptd.Discount) as DiscountAmt,("+InventryQuery+") " +
        //                   "AS TotalDiscAmount  from Customer CT join  POSTransaction PT  on (CT.CustomerID = PT.CustomerID)" +
        //                   " join POSTransactionDetail PTD on (pt.transid=ptd.transid ) join Item I on (I.ItemID=PTD.ITemID )" +
        //                   "  left outer join Department Dept on (Dept.DeptID=I.DepartmentID  ) , POSTransPayment PP  " +
        //                   " where convert(datetime,PT.TransDate,109) between convert(datetime, cast('" + this.dtpSaleStartDate.Text + " 00:00:00' as datetime) ,113) " +
        //                   "and convert(datetime, cast('" + this.dtpSaleEndDate.Text + " 23:59:59' as datetime) ,113)   and PP.TransID = PT.TransID  " +
        //                   " " + isexternalPaytype + "  ";
        //                    strQuerygroupby = " group by PTD.ItemID,PTD.ItemDescription,Dept.DeptName,Dept.DeptCode,CT.CustomerName,CT.Firstname ";
        //                }
        //            }
        //            else 
        //            {
        //                strQuery = "select " + sTopItem + " PT.TransID, convert(datetime,PT.TransDate,109) as TransDate, Dept.DeptName , "+
        //                    " " + clsPOSDBConstants.Customer_Fld_CustomerName + "+', '+" + clsPOSDBConstants.Customer_Fld_FirstName + " as CustName," +
        //               " PTD.ItemID ,PTD.ItemDescription as Description ,sum(PTD.Qty) as Qty,sum(PTD.ExtendedPrice-ptd.Discount+PTD.TaxAmount) as Amount ,SUM(ptd.Discount) as DiscountAmt" +
        //               " ,'" + this.dtpSaleStartDate.Text + " 00:00:00' as StartDate, '" + this.dtpSaleEndDate.Text + " 23:59:59' as EndDate, " +
        //               " SUM(ptd.Discount) as DiscountAmt,TotalDiscAmount  " +
        //               " from Customer CT join  POSTransaction PT on (CT.CustomerID = PT.CustomerID) join POSTransactionDetail PTD on (pt.transid=ptd.transid ) " +
        //               " join Item I on (I.ItemID=PTD.ITemID ) left outer join Department Dept on (Dept.DeptID=I.DepartmentID  ) , POSTransPayment PP  " +
        //               " where convert(datetime,PT.TransDate,109) between convert(datetime, cast('" + this.dtpSaleStartDate.Text + " 00:00:00' as datetime) ,113) "+
        //               "and convert(datetime, cast('" + this.dtpSaleEndDate.Text + " 23:59:59' as datetime) ,113)    and PP.TransID = PT.TransID  " +
        //                   " " + isexternalPaytype + "  ";
        //                strQuerygroupby = " group by PTD.ItemID,PTD.ItemDescription,Dept.DeptName,Dept.DeptCode,CT.CustomerName,CT.Firstname,TotalDiscAmount ,PT.TransID, PT.TransDate";
        //            }
        //            //End//
        //            if (string.IsNullOrEmpty(this.txtDepartment.Text.Trim()) == false)
        //            {
        //                strQuery += " and Dept.DeptCode in (" + this.txtDepartment.Tag.ToString().Trim() + ")";
        //            }
        //            if (string.IsNullOrEmpty(this.txtCustomer.Text.Trim()) == false)
        //            {
        //                strQuery += " and CT.CustomerID=" + Resources.Configuration.convertNullToInt(this.txtCustomer.Tag.ToString().Trim()).ToString() + " ";


        //            }
        //            /// Date 02/12/2014
        //            /// MOdified by Ashutosh
        //            /// Add transdate in query and modify query for new parameter Summery by ITEM
        //            /// old Code
        //            ///strQuery += " group by PTD.ItemID,PTD.ItemDescription,Dept.DeptName,Dept.DeptCode,CT.CustomerName,CT.Firstname,TotalDiscAmount ,PT.TransID";
        //            ///new code
        //            else
        //            {
        //                ((CrystalDecisions.CrystalReports.Engine.TextObject)oRptDept.ReportDefinition.ReportObjects["txtCustName"]).Text = "";
        //                ((CrystalDecisions.CrystalReports.Engine.TextObject)oRptDept.ReportDefinition.ReportObjects["txtCust"]).Text = "";
        //            }
					
        //            strQuery += strQuerygroupby;
        //            // End//

        //            if (optDetail.Checked == true)
        //            {
        //                ((CrystalDecisions.CrystalReports.Engine.TextObject)oRptDept.ReportDefinition.ReportObjects["txtCustName"]).Text = "";
        //                ((CrystalDecisions.CrystalReports.Engine.TextObject)oRptDept.ReportDefinition.ReportObjects["txtCust"]).Text = "";
        //            }
        //            else 
        //            {
        //                if (optSummary.Checked == true)
        //                {
        //                    oRptDept.DetailSection1.SectionFormat.EnableSuppress = true;
        //                    if (string.IsNullOrEmpty(this.txtCustomer.Text.Trim()) == false)
        //                    {
        //                        ((CrystalDecisions.CrystalReports.Engine.TextObject)oRptDept.ReportDefinition.ReportObjects["txtCustName"]).Text = this.lblCustomerName.Text;
        //                    }
        //                    else
        //                    {
        //                        ((CrystalDecisions.CrystalReports.Engine.TextObject)oRptDept.ReportDefinition.ReportObjects["txtCustName"]).Text = "";
        //                        ((CrystalDecisions.CrystalReports.Engine.TextObject)oRptDept.ReportDefinition.ReportObjects["txtCust"]).Text = "";
        //                        ((CrystalDecisions.CrystalReports.Engine.TextObject)oRptDept.ReportDefinition.ReportObjects["Text11"]).Text = "";
        //                    }
        //                }

        //            /// Date 03/02/2014
        //            /// MOdified by Ashutosh
        //            /// To hide transdate and Transid label for summery report
        //                ((CrystalDecisions.CrystalReports.Engine.TextObject)oRptDept.ReportDefinition.ReportObjects["txtTransID"]).Text = "";
        //                ((CrystalDecisions.CrystalReports.Engine.TextObject)oRptDept.ReportDefinition.ReportObjects["txtTransdate"]).Text = "";
        //            /// End
        //            }
        //            DataSet ds = clsReports.GetReportSource(strQuery);
        //            string sTransID = string.Empty;
        //            int Check_first_Item_calculate_or_not = 0;
        //            foreach (DataRow oRow in ds.Tables[0].Rows)
        //            {
        //                /// Date 03/02/2014
        //                /// MOdified by Ashutosh
        //                /// Added for Summery By Item report to check invoice as per item too...
        //                /// Old Code	*/
        //                /// string currTransID = Resources.Configuration.convertNullToString(oRow["TransId"]);
        //                /// New Code
        //                string currTransID = "";
        //                if (this.optSummeryItem.Checked == true)
        //                {
        //                    if (Check_first_Item_calculate_or_not == 0)
        //                    {
        //                        currTransID = Resources.Configuration.convertNullToString(oRow["ItemID"]);
        //                        Check_first_Item_calculate_or_not++;
        //                    }
        //                }
        //                else
        //                {
        //                    currTransID = Resources.Configuration.convertNullToString(oRow["TransId"]);
        //                }
        //                /// End
        //                if (sTransID.Contains(currTransID) == false)
        //                {
        //                    sTransID += currTransID +"$";
        //                }
        //                else
        //                {
        //                    oRow["TotalDiscAmount"] = 0;
        //                }
        //            }
        //            oRptDept.SetDataSource(ds.Tables[0]);
        //            clsReports.Preview(blnPrint, oRptDept);
        //            this.Cursor = System.Windows.Forms.Cursors.Default;
        //        }
        //        //Following else part is Added  By Shitaljit(QuicSolv) on 3 June 2011
        //        // Added Fields of SubDepartment table and Dept.DeptID= I.DepartmentID in where clause.
        //        else
        //        {
        //            rptSalesByDeptAndSubDept oRptSubDept = new rptSalesByDeptAndSubDept();

        //            /// Date 03/02/2014
        //            /// MOdified by Ashutosh
        //            /// Add parameter searching by subdepartment and Adding Paytype for filtering
        //            /// old Code
        //            ///strQuery = "select " + sTopItem + " PT.TransID, Dept.DeptName , " + clsPOSDBConstants.Customer_Fld_CustomerName + "+', '+" + clsPOSDBConstants.Customer_Fld_FirstName + " as CustName," +
        //            ///" SD.Description as SubDeptDescription,PTD.ItemID ,PTD.ItemDescription as Description , sum(PTD.Qty) as Qty,sum(PTD.ExtendedPrice-PTD.Discount+PTD.TaxAmount) as Amount " +
        //            ///" ,'" + this.dtpSaleStartDate.Text + "' as StartDate, '" + this.dtpSaleEndDate.Text + "' as EndDate, " +
        //            /// "SUM(ptd.Discount) as DiscountAmt, TotalDiscAmount  " +
        //            ///" from  Customer CT join  POSTransaction PT on (CT.CustomerID = PT.CustomerID) join POSTransactionDetail PTD on (pt.transid=ptd.transid ) " +
        //            ///" join Item I on (I.ItemID=PTD.ITemID ) left outer join Department Dept on (Dept.DeptID=I.DepartmentID  ) " +
        //            ///" , SubDepartment SD " +
        //            ///" where convert(datetime,PT.TransDate,109) between convert(datetime, cast('" + this.dtpSaleStartDate.Text + " 00:00:00' as datetime) ,113) and convert(datetime, cast('" + this.dtpSaleEndDate.Text + " 23:59:59' as datetime) ,113) and SD.DepartmentId= I.DepartmentID ";
        //            /// New code
        //            if (this.optSummeryItem.Checked == true)
        //            {
        //                if (txtCustomer.Text == "")
        //                {
        //                    strQuery = "select " + sTopItem + "  Dept.DeptName , SD.Description as SubDeptDescription,PTD.ItemID , "+
        //                        "PTD.ItemDescription as Description , sum(PTD.Qty) as Qty, sum(PTD.ExtendedPrice-PTD.Discount+PTD.TaxAmount) as Amount ,"+
        //                        "'" + this.dtpSaleStartDate.Text + " 00:00:00'  as StartDate, '" + this.dtpSaleEndDate.Text + " 23:59:59' as EndDate, "+
        //                        "SUM(ptd.Discount) as DiscountAmt, "+
        //                        "(Select SUM(PT1.TotalDiscAmount) from POSTransaction PT1, Department D1, POSTransactionDetail PTD1, Item I1,  POSTransPayment PP1  " +
        //                        " where convert(datetime,PT1.TransDate,109) between convert(datetime, cast('" + dtpSaleStartDate.Text + " 00:00:00'  as datetime) ,113)"+
        //                        " and convert(datetime, cast('" + dtpSaleEndDate.Text + " 23:59:59' as datetime) ,113) and PT1.TransID = PTD1.TransID "+
        //                  "and PTD1.ItemID = I1.ItemID and I1.DepartmentID = D1.DeptID and PP1.TransID = PT1.TransID and I1.Subdepartmentid in "+
        //                  "(" + this.txtSubDepartment.Tag.ToString().Trim() + ")  "+isinternalPaytype+") as TotalDiscAmount  "+
        //                        "from  POSTransaction PT  join POSTransactionDetail PTD on (pt.transid=ptd.transid )  join Item I on (I.ItemID=PTD.ITemID ) " +
        //                    "left outer join Department Dept on (Dept.DeptID=I.DepartmentID  ) , SubDepartment SD ,  POSTransPayment PP  " +
        //                    "where convert(datetime,PT.TransDate,109) between convert(datetime, cast('" + this.dtpSaleStartDate.Text + " 00:00:00' as datetime) ,113) "+
        //                    "and convert(datetime, cast('" + this.dtpSaleEndDate.Text + " 23:59:59' as datetime) ,113) and SD.DepartmentId= I.DepartmentID " +
        //                    "and SD.SubDepartmentID = I.SubDepartmentID  and PP.TransID = PT.TransID  " + isexternalPaytype + "";
        //                    strQuerygroupby = " group by PTD.ItemID,PTD.ItemDescription,Dept.DeptName,Dept.DeptCode,SD.Description";
        //                }
        //                else
        //                {
        //                    strQuery = "select " + sTopItem + " Dept.DeptName , " +
        //                    " " + clsPOSDBConstants.Customer_Fld_CustomerName + "+', '+" + clsPOSDBConstants.Customer_Fld_FirstName + " as CustName, " +
        //                    "SD.Description as SubDeptDescription,PTD.ItemID , PTD.ItemDescription as Description , sum(PTD.Qty) as Qty," +
        //                    "sum(PTD.ExtendedPrice-PTD.Discount+PTD.TaxAmount) as Amount ,'" + this.dtpSaleStartDate.Text + " 00:00:00' as StartDate, " +
        //                    "'" + this.dtpSaleEndDate.Text + " 23:59:59' as EndDate, SUM(ptd.Discount) as DiscountAmt, "+
        //                    "(Select SUM(TotalDiscAmount) from POSTransaction PT1, Department D1, POSTransactionDetail PTD1, Item I1, Customer CT1 ,  POSTransPayment PP1" +
        //                    " where convert(datetime,PT1.TransDate,109) between convert(datetime, cast('" + dtpSaleStartDate.Text + " 00:00:00' as datetime) ,113) "+
        //                    "and convert(datetime, cast('" + dtpSaleEndDate.Text + " 23:59:59' as datetime) ,113) and PT1.TransID = PTD1.TransID and PTD1.ItemID = I1.ItemID"+
        //                    " and I1.DepartmentID = D1.DeptID and PP1.TransID = PT1.TransID and I1.Subdepartmentid in (" + this.txtSubDepartment.Tag.ToString().Trim() + ")  " +
        //                    " and  CT1.CustomerID = PT1.CustomerID and CT1.CustomerID=('" + this.txtCustomer.Tag.ToString().Trim() + "')  " + isinternalPaytype + " " +
        //                    " ) as TotalDiscAmount   from Customer CT join POSTransaction PT " +
        //                    "  on (CT.CustomerID = PT.CustomerID) join POSTransactionDetail PTD on (pt.transid=ptd.transid )  join Item I on (I.ItemID=PTD.ITemID ) " +
        //                    "left outer join Department Dept on (Dept.DeptID=I.DepartmentID  ) , SubDepartment SD , POSTransPayment PP " +
        //                    "where convert(datetime,PT.TransDate,109) "+
        //                    "between convert(datetime, cast('" + this.dtpSaleStartDate.Text + " 00:00:00' as datetime) ,113) and " +
        //                    "convert(datetime, cast('" + this.dtpSaleEndDate.Text + " 23:59:59' as datetime) ,113) and SD.DepartmentId = I.DepartmentID " +
        //                    "and SD.SubDepartmentID = I.SubDepartmentID and PP.TransID = PT.TransID  " + isexternalPaytype + "";
        //                    strQuerygroupby = " group by PTD.ItemID,PTD.ItemDescription,Dept.DeptName,Dept.DeptCode,SD.Description,CT.CustomerName," +
        //                        "CT.Firstname";
        //                }
						
        //            }
        //            else
        //            {
        //                strQuery = "select " + sTopItem + " PT.TransID,  convert(datetime, PT.TransDate, 109) as TransDate, Dept.DeptName ,"+
        //                    " " + clsPOSDBConstants.Customer_Fld_CustomerName + "+', '+" + clsPOSDBConstants.Customer_Fld_FirstName + " as CustName," +
        //                " SD.Description as SubDeptDescription,PTD.ItemID ,PTD.ItemDescription as Description , sum(PTD.Qty) as Qty,"+
        //                "sum(PTD.ExtendedPrice-PTD.Discount+PTD.TaxAmount) as Amount " +
        //                " ,'" + this.dtpSaleStartDate.Text + " 00:00:00' as StartDate, '" + this.dtpSaleEndDate.Text + " 23:59:59' as EndDate, " +
        //                 "SUM(ptd.Discount) as DiscountAmt, TotalDiscAmount  " +
        //                " from  Customer CT join  POSTransaction PT on (CT.CustomerID = PT.CustomerID) join POSTransactionDetail PTD on (pt.transid=ptd.transid ) " +
        //                " join Item I on (I.ItemID=PTD.ITemID ) left outer join Department Dept on (Dept.DeptID=I.DepartmentID  ) " +
        //                " , SubDepartment SD ,  POSTransPayment PP " +
        //                " where convert(datetime,PT.TransDate,109) between convert(datetime, cast('" + this.dtpSaleStartDate.Text + " 00:00:00' as datetime) ,113)"+
        //                " and convert(datetime, cast('" + this.dtpSaleEndDate.Text + " 23:59:59' as datetime) ,113) and SD.DepartmentId= I.DepartmentID "+
        //                "and SD.SubDepartmentID = I.SubDepartmentID  and PP.TransID = PT.TransID  " + isexternalPaytype + "";
        //                strQuerygroupby = " group by PTD.ItemID,PTD.ItemDescription,Dept.DeptName,Dept.DeptCode,SD.Description,CT.CustomerName,CT.Firstname,"+
        //                    "TotalDiscAmount, PT.TransID, PT.TransDate ";
        //            }
        //            // End//


        //            //ORIGINAL Commneted By Shitaljit(QuicSolv) on 3 June 2011
        //            //if (this.txtDepartment.Text.Trim() != "" && txtDepartment.Tag != null && txtDepartment.Tag.ToString().Trim().Length > 0)
        //            //{
        //            //    strQuery += " and Dept.DeptCode in (" + this.txtDepartment.Tag.ToString().Trim() + ")";
        //            //}
        //            //Follwing code is Added By shitaljit(QuicSolv) on 3 June
        //            if (string.IsNullOrEmpty(this.txtDepartment.Text.Trim()) == false)
        //            {
        //                strQuery += " and SD.DepartmentID in (Select Department.DeptID from Department where Department.DeptCode in(" + this.txtDepartment.Tag.ToString().Trim() + ") )";
        //            }
        //            if (string.IsNullOrEmpty(this.txtSubDepartment.Text.Trim()) == false)
        //            {
        //                strQuery += " and SD.SubDepartmentID in (" + this.txtSubDepartment.Tag.ToString().Trim() + ")";
        //            }
        //            if (string.IsNullOrEmpty(this.txtCustomer.Text.Trim()) == false)
        //            {
        //                strQuery += " and CT.CustomerID=" + Resources.Configuration.convertNullToInt(this.txtCustomer.Tag.ToString().Trim()).ToString() + " ";
        //            }
        //            //Till here Added By Shitaljit(QuicSolv)
        //            //ORIGINAL Commneted By Shitaljit(QuicSolv) on 3 June 2011
        //            //strQuery += " group by PTD.ItemID,PTD.ItemDescription,Dept.DeptName,Dept.DeptCode";
        //            // Modified By Shitaljit(QuicSolv)
        //            //Added SD.Description in the Group By Clause.

        //            /// Date 02/12/2014
        //            /// MOdified by Ashutosh
        //            /// Add transdate in query and modify query for new parameter Summery by ITEM
        //            /// old Code
        //            /// strQuery += " group by PTD.ItemID,PTD.ItemDescription,Dept.DeptName,Dept.DeptCode,SD.Description,CT.CustomerName,CT.Firstname,TotalDiscAmount, PT.TransID ";
        //            ///new code
        //            else
        //            {
        //                ((CrystalDecisions.CrystalReports.Engine.TextObject)oRptSubDept.ReportDefinition.ReportObjects["txtCustName"]).Text = "";
        //                ((CrystalDecisions.CrystalReports.Engine.TextObject)oRptSubDept.ReportDefinition.ReportObjects["txtCust"]).Text = "";
        //            }
        //            strQuery += strQuerygroupby;
        //            /// end ///
        //            if (optDetail.Checked == true)
        //            {
        //                ((CrystalDecisions.CrystalReports.Engine.TextObject)oRptSubDept.ReportDefinition.ReportObjects["txtCustName"]).Text = "";
        //                ((CrystalDecisions.CrystalReports.Engine.TextObject)oRptSubDept.ReportDefinition.ReportObjects["txtCust"]).Text = "";
        //            }
        //            else
        //            {
        //                if (this.optSummary.Checked == true)
        //                {
        //                    oRptSubDept.DetailSection1.SectionFormat.EnableSuppress = true;
        //                    if (string.IsNullOrEmpty(this.txtCustomer.Text.Trim()) == false)
        //                    {
        //                        ((CrystalDecisions.CrystalReports.Engine.TextObject)oRptSubDept.ReportDefinition.ReportObjects["txtCustName"]).Text = this.lblCustomerName.Text;
        //                    }
        //                    else
        //                    {
        //                        ((CrystalDecisions.CrystalReports.Engine.TextObject)oRptSubDept.ReportDefinition.ReportObjects["txtCustName"]).Text = "";
        //                        ((CrystalDecisions.CrystalReports.Engine.TextObject)oRptSubDept.ReportDefinition.ReportObjects["txtCust"]).Text = "";
        //                    }
        //                }
        //                ((CrystalDecisions.CrystalReports.Engine.TextObject)oRptSubDept.ReportDefinition.ReportObjects["txtTransID"]).Text = "";
        //                ((CrystalDecisions.CrystalReports.Engine.TextObject)oRptSubDept.ReportDefinition.ReportObjects["txtTransDate"]).Text = "";
						
        //            }
        //            DataSet ds = clsReports.GetReportSource(strQuery);
        //            string sTransID = string.Empty;
        //            int Check_first_Item_calculate_or_not = 0;
        //            foreach (DataRow oRow in ds.Tables[0].Rows)
        //            {
        //                string currTransID="";
        //                if (this.optSummeryItem.Checked == true)
        //                {
        //                    /// Date 03/02/2014
        //                    /// MOdified by Ashutosh
        //                    /// Add parameter searching by subdepartment and Adding Paytype for filtering
        //                    /// old Code
        //                    /// string currTransID = Resources.Configuration.convertNullToString(oRow["TransId"]);
        //                    /// new code
        //                    if (Check_first_Item_calculate_or_not == 0)
        //                    {
        //                        currTransID = Resources.Configuration.convertNullToString(oRow["ItemID"]);
        //                        Check_first_Item_calculate_or_not++;
        //                    }
        //                }
        //                else
        //                {
        //                    currTransID = Resources.Configuration.convertNullToString(oRow["TransId"]);
        //                }
        //                /// end ///
        //                if (sTransID.Contains(currTransID) == false)
        //                {
        //                    sTransID += currTransID + Configuration.CInfo.CurrencySymbol.ToString();
        //                }
        //                else
        //                {
        //                    oRow["TotalDiscAmount"] = 0;
        //                }
        //            }
        //            oRptSubDept.SetDataSource(ds.Tables[0]);
        //            clsReports.Preview(blnPrint, oRptSubDept);
        //            this.Cursor = System.Windows.Forms.Cursors.Default;
        //        }
        //        //ORIGINAL Commented By Shitaljit(QuicSolv) on 3 June 2011
        //        //if (this.optSummary.Checked == true)
        //        //{
        //        //    oRpt.DetailSection1.SectionFormat.EnableSuppress = true;
        //        //}

        //        //clsReports.Preview(blnPrint, strQuery, oRpt);
        //        //this.Cursor = System.Windows.Forms.Cursors.Default;
        //        //End Of Added By Shitaljit.
        //    }
        //    catch (Exception exp)
        //    {
        //        this.Cursor = System.Windows.Forms.Cursors.Default;
        //        clsUIHelper.ShowErrorMsg(exp.Message);
        //    }

        //}
        #endregion

        private void pbtnView_Click_1(object sender, System.EventArgs e)
        {
            this.dtpSaleStartDate.Focus();
            PreviewReport(false);
        }

        private void btnPrint_Click(object sender, System.EventArgs e)
        {
            this.dtpSaleStartDate.Focus();
            PreviewReport(true);
        }
        private void SearchDept()
        {
            try
            {
                //frmSearch oSearch = new frmSearch(clsPOSDBConstants.Department_tbl);
                frmSearchMain oSearch = new frmSearchMain(true);    //20-Dec-2017 JY Added new reference
                oSearch.SearchTable = clsPOSDBConstants.Department_tbl; //20-Dec-2017 JY Added 
                oSearch.AllowMultiRowSelect = true;
                oSearch.SearchInConstructor = true;
                oSearch.ShowDialog(this);
                if (!oSearch.IsCanceled)
                {
                    string strDeptID = "";
                    string strDeptName = "";
                    foreach (UltraGridRow oRow in oSearch.grdSearch.Rows)
                    {
                        if ((bool)oRow.Cells["check"].Value == true)
                        {
                            strDeptID += ",'" + oRow.Cells["id"].Text + "'";
                            strDeptName += "," + oRow.Cells["Name"].Text;
                        }
                    }

                    if (strDeptID.StartsWith(","))
                    {
                        strDeptID = strDeptID.Substring(1);
                        strDeptName = strDeptName.Substring(1);
                    }
                    txtDepartment.Text = strDeptName;
                    txtDepartment.Tag = strDeptID;
                }
                else
                {
                    txtDepartment.Text = string.Empty;
                    txtDepartment.Tag = string.Empty;
                }

            }

            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }
        private string GetDeptName(string code, string senderName)
        {
            string returnValue = "";
            if (senderName == clsPOSDBConstants.Department_tbl)
            {
                #region Department
                try
                {
                    POS_Core.BusinessRules.Department oDept = new Department();
                    DepartmentData oDeptData;
                    DepartmentRow oDeptRow = null;
                    oDeptData = oDept.Populate(code);
                    oDeptRow = oDeptData.Department[0];
                    if (oDeptRow != null)
                    {
                        returnValue = oDeptRow.DeptCode.ToString();
                    }
                }
                catch (System.IndexOutOfRangeException)
                {
                    this.txtDepartment.Text = String.Empty;
                    SearchDept();
                }
                catch (Exception exp)
                {
                    clsUIHelper.ShowErrorMsg(exp.Message);
                    exp = null;
                    this.txtDepartment.Text = String.Empty;
                    SearchDept();
                }
                #endregion
            }
            return returnValue;
        }

        private void txtDepartment_EditorButtonClick(object sender, Infragistics.Win.UltraWinEditors.EditorButtonEventArgs e)
        {
            SearchDept();
        }

        //Added By Shitaljit(QuicSolv) on 3 June 2011
        //To add Sub Department Filter in the Table.
        private void SearchSubDept()
        {
            try
            {
                #region Sprint-24 - PRIMEPOS-2273 27-Oct-2016 JY Commented for optimization
                //POS_Core.BusinessRules.Department oDept = new Department();
                //DepartmentData oDeptData;
                //DepartmentRow oDeptRow = null;
                //List<string> listDept = new List<string>();
                //if (txtDepartment.Tag.ToString() != "")
                //{
                //    oDeptData = oDept.PopulateList(" and deptcode in (" + txtDepartment.Tag.ToString() + ")");
                //}
                //else
                //{
                //    oDeptData = oDept.PopulateList("");
                //}
                //for (int RowIndex = 0; RowIndex < oDeptData.Tables[0].Rows.Count; RowIndex++)
                //{
                //    oDeptRow = oDeptData.Department[RowIndex];
                //    listDept.Add(oDeptRow.DeptID.ToString());
                //}
                //string InQuery = string.Join("','", listDept.ToArray());
                #endregion

                #region Sprint-24 - PRIMEPOS-2273 27-Oct-2016 JY Added for optimization
                string InQuery = string.Empty;
                if (txtDepartment.Tag != null && txtDepartment.Tag.ToString().Length > 2)
                {
                    InQuery = txtDepartment.Tag.ToString().Substring(1, txtDepartment.Tag.ToString().Length - 2);
                }
                SearchSvr.SubDeptIDFlag = true;
                #endregion

                if (InQuery != "")
                {
                    SearchSvr.SubDeptIDFlag = true;
                }
                //frmSearch oSearch = new frmSearch(clsPOSDBConstants.SubDepartment_tbl, InQuery, "");
                frmSearchMain oSearch = new frmSearchMain(clsPOSDBConstants.SubDepartment_tbl, InQuery, "", true);  //20-Dec-2017 JY Added new reference
                oSearch.AllowMultiRowSelect = true;
                oSearch.SearchInConstructor = true;
                oSearch.ShowDialog(this);
                if (!oSearch.IsCanceled)
                {
                    string strSubDeptCode = "";
                    string stSubDeptName = "";
                    foreach (UltraGridRow oRow in oSearch.grdSearch.Rows)
                    {
                        if ((bool)oRow.Cells["check"].Value == true)
                        {
                            strSubDeptCode += ",'" + oRow.Cells["Code"].Text + "'";
                            stSubDeptName += "," + oRow.Cells["Sub Department Name"].Text;
                        }
                    }
                    if (strSubDeptCode.StartsWith(","))
                    {
                        strSubDeptCode = strSubDeptCode.Substring(1);
                        stSubDeptName = stSubDeptName.Substring(1);
                    }
                    txtSubDepartment.Text = stSubDeptName;
                    txtSubDepartment.Tag = strSubDeptCode;
                }
                else
                {
                    txtSubDepartment.Text = string.Empty;
                    txtSubDepartment.Tag = string.Empty;
                }
                SearchSvr.SubDeptIDFlag = false;
            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }

        private void txtSubDepartment_Click(object sender, EventArgs e)
        {
           
        }

        private void txtSubDepartment_EditorButtonClick(object sender, EditorButtonEventArgs e)
        {
            SearchSubDept();
        }

        private void txtDepartment_TextChanged(object sender, EventArgs e)
        {
            if (txtDepartment.Text != "")
            {
                txtSubDepartment.Text = "";
            }
        }

        private void SearchCustomer()
        {
            try
            {
                //frmCustomerSearch oSearch = new frmCustomerSearch(txtCustomer.Text);
                frmSearchMain oSearch = new frmSearchMain(txtCustomer.Text, true, clsPOSDBConstants.Customer_tbl);    //18-Dec-2017 JY Added new reference
                oSearch.ActiveOnly = 1;
                oSearch.ShowDialog(this);
                if (!oSearch.IsCanceled)
                {
                    string strCode = oSearch.SelectedAcctNo();
                    if (strCode == "")
                    {
                        ClearCustomer();
                        return;
                    }

                    FKEdit(strCode, clsPOSDBConstants.Customer_tbl);
                    this.txtCustomer.Focus();
                }
                else
                {
                    ClearCustomer();
                }
            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
                ClearCustomer();
            }
        }
        private void ClearCustomer()
        {
            this.txtCustomer.Text = String.Empty;
            this.lblCustomerName.Text = String.Empty;
            this.txtCustomer.Tag = String.Empty;
            this.lblCustomerName.Text = String.Empty;
        }

        private void FKEdit(string code, string senderName)
        {
            if (senderName == clsPOSDBConstants.Customer_tbl)
            {
                #region Customer
                try
                {
                    Customer oCustomer = new Customer();
                    CustomerData oCustomerData;
                   
                    oCustomerData = oCustomer.Populate(code);
                    oCustomerRow = oCustomerData.Customer[0];
                    if (oCustomerRow != null)
                    {
                        this.txtCustomer.Text = oCustomerRow.AccountNumber.ToString();
                        this.txtCustomer.Tag = oCustomerRow.CustomerId.ToString();
                        this.lblCustomerName.Text = oCustomerRow.CustomerFullName;
                    }
                }
                catch (System.IndexOutOfRangeException)
                {
                    SearchCustomer();
                }
                catch (Exception exp)
                {
                    clsUIHelper.ShowErrorMsg(exp.Message);
                    SearchCustomer();
                }
                #endregion
            }
        }

        private void txtCustomer_EditorButtonClick(object sender, EditorButtonEventArgs e)
        {
            SearchCustomer();
        }

        private void txtCustomer_Leave(object sender, EventArgs e)
        {
            try
            {
                string txtValue = this.txtCustomer.Text;
                if (txtValue.Trim() != "")
                {
                    FKEdit(txtValue, clsPOSDBConstants.Customer_tbl);
                }
                else
                {
                    ClearCustomer();
                }
            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }

        //Till Here Added By Shitaljit(QuicSolv)     












		private void txtPaymentType_EditorButtonClick(object sender, EditorButtonEventArgs e)
		{
			SearchPayType();
		}

		private void SearchPayType()
		{
			try
			{
				if (this.txtPaymentType.ButtonsRight[0].Enabled == false)
				{
					return;
				}

                //frmSearch oSearch = new frmSearch(clsPOSDBConstants.PayType_tbl);
                frmSearchMain oSearch = new frmSearchMain(true);    //20-Dec-2017 JY Added new reference
                oSearch.SearchTable = clsPOSDBConstants.PayType_tbl;    //20-Dec-2017 JY Added
                oSearch.AllowMultiRowSelect = true;
				oSearch.ShowDialog(this);
				if (!oSearch.IsCanceled)
				{
					string strPayTypeCode = "";
					string strPayTypeName = "";
					foreach (UltraGridRow oRow in oSearch.grdSearch.Rows)
					{
						if ((bool)oRow.Cells["check"].Value == true)
						{
							strPayTypeCode += ",'" + oRow.Cells["Code"].Text + "'";
							strPayTypeName += "," + oRow.Cells["Description"].Text;
						}
					}

					if (strPayTypeCode.StartsWith(","))
					{
						strPayTypeCode = strPayTypeCode.Substring(1);
						strPayTypeName = strPayTypeName.Substring(1);
					}
					txtPaymentType.Text = strPayTypeName;
					txtPaymentType.Tag = strPayTypeCode;
				}
				else
				{
					txtPaymentType.Text = string.Empty;
					txtPaymentType.Tag = string.Empty;
				}
			}
			catch (Exception exp)
			{
				clsUIHelper.ShowErrorMsg(exp.Message);
			}
		}

		private bool GridVisibleFlag = false;
		private void btnPayTypeList_Click(object sender, EventArgs e)
		{
			if (grpPayTypeList.Visible == false)
				PayTypeList_Expand(true);
			else
			{
				PayTypeList_Expand(false);
				GetSelectedPayTypes();//Added by Krishna on 23 November 2011
			}
		}

		private void PayTypeList_Expand(bool Value)
		{
			if (Value == true)
			{
				if (grpPayTypeList.Visible == false)
				{
					dataGridList.Visible = true;
					grpPayTypeList.Visible = true;
					dataGridList.Height = 230;
					grpPayTypeList.Height = dataGridList.Height + 31;
					GridVisibleFlag = true;
					chkSelectAll.Location = new Point(chkSelectAll.Location.X, dataGridList.Height + 10);
					grpPayTypeList.Focus();

					//dataGridList.Focus();
					//dataGridList.Rows[1].Selected = true;
				}
			}
			else
			{
				if (grpPayTypeList.Visible == true)
				{
					dataGridList.Visible = false;
					grpPayTypeList.Visible = false;
					GridVisibleFlag = false;
					//butVendorList.Focus();
				}
			}
		}

		private void butPayTypeList_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyData == System.Windows.Forms.Keys.Down)
			{
				dataGridList.Focus();
			}
		}

		private void butPayTypeList_KeyUp(object sender, KeyEventArgs e)
		{
			if (e.KeyData == System.Windows.Forms.Keys.Enter)
			{
				if (grpPayTypeList.Visible == true)
				{
					dataGridList.Visible = false;
					grpPayTypeList.Visible = false;
				}
			}
		}

		POS_Core.BusinessRules.Vendor vendor = new POS_Core.BusinessRules.Vendor();
		VendorData vendorData = new VendorData();
		POS_Core.DataAccess.SearchSvr SearchSvr = new POS_Core.DataAccess.SearchSvr();

		private void FillPayType()
		{
			DataSet PayTypeData = new DataSet();
			PayTypeData = SearchSvr.Search(clsPOSDBConstants.PayType_tbl, "", "", 0, -1);
			dataGridList.DataSource = PayTypeData.Tables[0];
			for (int i = 1; i <= PayTypeData.Tables[0].Columns.Count; i++)
			{
				dataGridList.Columns[i].ReadOnly = true;
				if (i == 1)
				{
					dataGridList.Columns[i].Width = 30;
					dataGridList.Columns[i].Name = "PayType";
				}
				else
					dataGridList.Columns[i].Width = 115;
			}
			//int rowsCount = dataGridList.Rows.Count;
			//if (rowsCount > 0)
			//{
			//    PayTypeList_Expand(true);
			//    for (int i = 0; i < rowsCount; i++)
			//    {
			//        dataGridList.Rows[i].Cells["chkbox"].Value = true;
			//        chkSelectAll.Text = "Unselect All";
			//    }
			//    PayTypeList_Expand(false);
			//    GetSelectedPayTypes();
			//}
		}

		private void chkSelectAll_CheckedChanged(object sender, EventArgs e)
		{
			int rowIndex = 0;
			int rowsCount = dataGridList.Rows.Count;
			if (rowsCount > 0)
			{
				for (int i = 0; i < rowsCount; i++)
				{

					if (chkSelectAll.Checked == true)
					{

						dataGridList.Rows[i].Cells[0].Value = true;
						chkSelectAll.Text = "Unselect All";
					}
					else
					{
						dataGridList.Rows[i].Cells[0].Value = false;
						chkSelectAll.Text = "Select All";
					}
				}
			}

		}

		public void GetSelectedPayTypes()
		{
			string strPayTypeCode = "";
			string strPayTypeName = "";
			foreach (DataGridViewRow oRow in dataGridList.Rows)
			{
				if (POS_Core.Resources.Configuration.convertNullToBoolean(oRow.Cells[0].Value))
				{
					strPayTypeCode += ",'" + oRow.Cells[1].Value.ToString() + "'";
					strPayTypeName += "," + oRow.Cells["Description"].Value.ToString();
				}
			}

			if (strPayTypeCode.StartsWith(","))
			{
				strPayTypeCode = strPayTypeCode.Substring(1);
				strPayTypeName = strPayTypeName.Substring(1);
			}
			txtPaymentType.Text = strPayTypeName;
			txtPaymentType.Tag = strPayTypeCode;
		}

		private void dataGridList_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
		{
			for (int i = 0; i < dataGridList.Rows.Count; i++)
			{
				dataGridList.Rows[i].Cells[dataGridList.Columns["chkBox"].Index].Value = true;
			}
			chkSelectAll.Checked = true;
			GetSelectedPayTypes();

		}

        #region Sprint-19 - 2157 18-Feb-2015 JY Added
        private void btnSearch_Click(object sender, EventArgs e)
        {
            string strSQL;
            try
            {
                dsDept = new DataSet();
                DataSet dsDeptTmp = new DataSet();

                GenerateSQL(out strSQL, false);
                dsDeptTmp.Tables.Add(oSearchSvr.Search(strSQL).Tables[0].Copy());

                #region department summry
                var SalesByDept = from Dept in dsDeptTmp.Tables[0].AsEnumerable()
                                  group Dept by new
                                  {
                                      DeptId = Dept.Field<Int32>("DeptID"),
                                      DeptName = Dept.Field<string>("DeptName")
                                  } into userg
                                   select new
                                   {
                                       DeptID = userg.Key.DeptId,
                                       DeptName = userg.Key.DeptName,
                                       Qty = userg.Sum(x => x.Field<Int32>("Qty")),
                                       Amount = userg.Sum(x => x.Field<decimal>("Amount")),
                                   };

                DataTable dt = new DataTable();
                dt.Columns.Add("DeptId", typeof(System.Int32));
                dt.Columns.Add("Department Name", typeof(System.String));
                dt.Columns.Add("Quantity", typeof(System.Int32));
                dt.Columns.Add("Amount", typeof(System.Decimal));
                DataRow dr;

                foreach (var Summary in SalesByDept)
                {
                    dr = dt.NewRow();
                    dr["DeptId"] = Summary.DeptID;
                    dr["Department Name"] = Summary.DeptName;
                    dr["Quantity"] = Summary.Qty;
                    dr["Amount"] = Summary.Amount;

                    dt.Rows.Add(dr);
                }

                dsDept.Tables.Add(dt);   //Added table for summary by department
                dsDept.Tables[0].TableName = "Summary";
                #endregion

                #region PRIMEPOS-2236 17-Aug-2018 JY Added sub-dept summary
                var SalesBySubDept = from SubDept in dsDeptTmp.Tables[0].AsEnumerable()
                                  group SubDept by new
                                  {
                                      DeptId = SubDept.Field<Int32>("DeptID"),
                                      SubDepartmentID = Configuration.convertNullToInt(SubDept.Field<Int32>("SubDepartmentID")),
                                      SubDeptName = Configuration.convertNullToString(SubDept.Field<string>("SubDeptName"))
                                  } into userg
                                  select new
                                  {
                                      DeptID = userg.Key.DeptId,
                                      SubDepartmentID = Configuration.convertNullToInt(userg.Key.SubDepartmentID),
                                      SubDeptName = Configuration.convertNullToString(userg.Key.SubDeptName),
                                      Qty = userg.Sum(x => x.Field<Int32>("Qty")),
                                      Amount = userg.Sum(x => x.Field<decimal>("Amount")),
                                  };

                dt = new DataTable();
                dt.Columns.Add("DeptId", typeof(System.Int32));                
                dt.Columns.Add("SubDeptID", typeof(System.Int32));
                dt.Columns.Add("Sub Department Name", typeof(System.String));
                dt.Columns.Add("Quantity", typeof(System.Int32));
                dt.Columns.Add("Amount", typeof(System.Decimal));

                foreach (var SDSummary in SalesBySubDept)
                {
                    dr = dt.NewRow();
                    dr["DeptId"] = SDSummary.DeptID;
                    dr["SubDeptID"] = SDSummary.SubDepartmentID;
                    dr["Sub Department Name"] = SDSummary.SubDeptName;
                    dr["Quantity"] = SDSummary.Qty;
                    dr["Amount"] = SDSummary.Amount;

                    dt.Rows.Add(dr);
                }

                dsDept.Tables.Add(dt);   //Added table for summary by sub department
                dsDept.Tables[1].TableName = "SubDeptSummary";
                dsDept.Relations.Add("SD", dsDept.Tables[0].Columns["DeptId"], dsDept.Tables[1].Columns["DeptId"]);
                #endregion

                dsDept.Tables.Add(dsDeptTmp.Tables[0].Copy());
                dsDept.Tables[2].TableName = "Details";

                DataColumn[] SDColumns;
                DataColumn[] DetailColumns;

                SDColumns = new DataColumn[] { dsDept.Tables[1].Columns["DeptId"], dsDept.Tables[1].Columns["SubDeptID"] };
                DetailColumns = new DataColumn[] { dsDept.Tables[2].Columns["DeptId"], dsDept.Tables[2].Columns["SubDepartmentID"] };

                DataRelation SD_Detail = new DataRelation("Detail", SDColumns, DetailColumns);
                dsDept.Relations.Add(SD_Detail);

                grdDept.DataSource = dsDept;
                grdDept.DisplayLayout.Bands[0].HeaderVisible = true;
                grdDept.DisplayLayout.Bands[0].Header.Appearance.FontData.SizeInPoints = 10;
                grdDept.DisplayLayout.Bands[0].Header.Appearance.TextHAlign = Infragistics.Win.HAlign.Center;
                grdDept.DisplayLayout.Bands[0].Header.Caption = "Sales Summary By Department";
                grdDept.DisplayLayout.Bands[0].Override.WrapHeaderText = Infragistics.Win.DefaultableBoolean.True;
                grdDept.DisplayLayout.Bands[0].Columns["DeptId"].Hidden = true;

                grdDept.DisplayLayout.Bands[1].HeaderVisible = false;
                //grdDept.DisplayLayout.Bands[1].Header.Appearance.FontData.SizeInPoints = 10;
                //grdDept.DisplayLayout.Bands[1].Header.Appearance.TextHAlign = Infragistics.Win.HAlign.Center;
                //grdDept.DisplayLayout.Bands[1].Header.Caption = "Sales Summary By Sub Department";
                grdDept.DisplayLayout.Bands[1].Override.WrapHeaderText = Infragistics.Win.DefaultableBoolean.True;
                grdDept.DisplayLayout.Bands[1].Columns["DeptId"].Hidden = true;
                grdDept.DisplayLayout.Bands[1].Columns["SubDeptID"].Hidden = true;

                grdDept.DisplayLayout.Bands[2].HeaderVisible = false;
                //grdDept.DisplayLayout.Bands[2].Header.Caption = "Detail";
                //grdDept.DisplayLayout.Bands[2].Header.Appearance.FontData.SizeInPoints = 10;
                //grdDept.DisplayLayout.Bands[2].Header.Appearance.TextHAlign = Infragistics.Win.HAlign.Center;
                grdDept.DisplayLayout.Bands[2].Expandable = true;
                grdDept.DisplayLayout.Bands[2].Override.WrapHeaderText = Infragistics.Win.DefaultableBoolean.True;
                grdDept.DisplayLayout.Bands[2].Columns["DeptId"].Hidden = true;
                grdDept.DisplayLayout.Bands[2].Columns["DeptName"].Hidden = true;
                grdDept.DisplayLayout.Bands[2].Columns["SubDepartmentID"].Hidden = true;
                grdDept.DisplayLayout.Bands[2].Columns["SubDeptName"].Hidden = true;
                grdDept.DisplayLayout.Bands[2].Columns["InvoiceDiscount"].Hidden = true;
                grdDept.DisplayLayout.Bands[2].Columns["StartDate"].Hidden = true;
                grdDept.DisplayLayout.Bands[2].Columns["EndDate"].Hidden = true;

                resizeColumns(grdDept);
                grdDept.PerformAction(UltraGridAction.FirstRowInGrid);
                grdDept.Refresh();
            }
            catch (Exception Ex)
            { 
            }
        }

        private void resizeColumns(Infragistics.Win.UltraWinGrid.UltraGrid grd)
        {
            try
            {
                foreach (UltraGridBand oBand in grd.DisplayLayout.Bands)
                {
                    foreach (UltraGridColumn oCol in oBand.Columns)
                    {
                        oCol.Width = oCol.CalculateAutoResizeWidth(PerformAutoSizeType.VisibleRows, true) + 10;
                        if (oCol.DataType.Equals(typeof(System.Int32)) || oCol.DataType.Equals(typeof(System.Decimal)) || oCol.DataType.Equals(typeof(System.DateTime)))
                        {
                            oCol.CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right;
                        }
                    }
                }
            }
            catch (Exception) { }
        }

        private void GenerateSQL(out string strSQL, Boolean Flag)
        {
            //flag = true means call from PreviewReport

            string strTop = string.Empty, strFilter = string.Empty, strPayType = string.Empty, strDisc = string.Empty, strUserId = string.Empty;    //Sprint-21 - 1868 20-Jul-2015 JY Added strDisc

            try
            {
                if (Convert.ToString(this.txtPaymentType.Tag).Trim() != "")
                    strPayType = " AND PT.TransID IN (SELECT TransId FROM POSTransPayment WHERE TransTypeCode in (" + txtPaymentType.Tag.ToString().Trim() + ") )"; 
                //else
                //    strPayType = " AND PT.TransID IN (SELECT TransId FROM POSTransPayment WHERE TransTypeCode in ('1 ','2 ','3 ','4 ','5 ','6 ','7 ','B ','C ','E ','H '))";

                if (Convert.ToString(this.txtDepartment.Tag).Trim() != "")
                    strFilter += " AND I.DepartmentID in (" + this.txtDepartment.Tag.ToString().Trim() + ")";

                if (Convert.ToString(this.txtSubDepartment.Tag).Trim() != "")
                    strFilter += " AND SD.SubDepartmentID in (" + this.txtSubDepartment.Tag.ToString().Trim() + ")";

                if (Convert.ToString(txtCustomer.Tag).Trim() != "")
                    strFilter += " AND CT.CustomerID =" + POS_Core.Resources.Configuration.convertNullToInt(this.txtCustomer.Tag.ToString().Trim()).ToString();

                strTop = (POS_Core.Resources.Configuration.convertNullToInt(this.txtTopProducts.Value)) > 0 ? " TOP " + POS_Core.Resources.Configuration.convertNullToString(this.txtTopProducts.Value) : "";

                if (chkOnlyDiscountedTrans.Checked) //Sprint-21 - 1868 20-Jul-2015 JY Added condition to consider only discounted transactions
                    strDisc = " AND PT.TotalDiscAmount <> 0 ";

                if (this.txtUserID.Text.Trim() != "")   //PRIMEPOS-2837 22-Apr-2020 JY Added
                    strUserId = " AND PT.UserID ='" + this.txtUserID.Text.Trim().Replace("'","''") + "' ";

                if (Flag == false || optSummeryItem.Checked == false)    //if called for grid or called from report opnSummaryItem checked
                {
                    strSQL = " SELECT " + strTop + " Dept.DeptID, Dept.DeptName, ISNULL(SD.SubDepartmentID,0) AS SubDepartmentID, " +
                        " CASE WHEN ISNULL(SD.SubDepartmentID,0) = 0 THEN 'No Sub-dept' ELSE ISNULL(SD.Description,'') END AS SubDeptName, " +
                        " PT.TransID, convert(datetime,PT.TransDate,109) as TransDate, " +
                        " CustomerName + ', ' + FirstName as CustName, PTD.ItemID, PTD.ItemDescription AS ItemName, SUM(PTD.Qty) AS Qty, SUM(PTD.ExtendedPrice-ptd.Discount+PTD.TaxAmount) AS Amount, x.InvoiceDiscount, " +
                        "'" + this.dtpSaleStartDate.Text + "' as StartDate, '" + this.dtpSaleEndDate.Text + "' as EndDate" +
                        ", SUM(PTD.Price) AS SellingPrice, SUM(PTD.ITEMCOST) AS CostPrice, SUM(PTD.ExtendedPrice) AS ExtendedPrice, SUM(PTD.TaxAmount) AS Tax, SUM(ptd.Discount) AS Discount " +
                        " FROM POSTransaction PT " +
                        " INNER JOIN POSTransactionDetail PTD on pt.transid = ptd.transid " +
                        " LEFT JOIN Customer CT on CT.CustomerID = PT.CustomerID " +
                        " INNER JOIN Item I on I.ItemID=PTD.ItemID " +
                        " LEFT JOIN Department Dept on Dept.DeptID=I.DepartmentID " +
                        " LEFT JOIN subDepartment SD on SD.DepartmentID = Dept.DeptID AND SD.SubDepartmentID = I.SUBDEPARTMENTID " +
                        " INNER JOIN (SELECT SUM(InvoiceDiscount) AS InvoiceDiscount FROM " +
                                        " (SELECT PTD.ItemID, CASE WHEN ISNULL(SUM(PT.GrossTotal),0) = 0 then 0 else SUM(ISNULL(PT.INVOICEDISCOUNT,0))*SUM(PTD.ExtendedPrice)/SUM(PT.GrossTotal) END AS InvoiceDiscount FROM POSTransaction PT " +
                                        " INNER JOIN POSTransactionDetail PTD on pt.transid = ptd.transid LEFT JOIN Customer CT on CT.CustomerID = PT.CustomerID INNER JOIN Item I on I.ItemID=PTD.ItemID LEFT JOIN Department Dept on Dept.DeptID=I.DepartmentID LEFT JOIN subDepartment SD on SD.DepartmentID = Dept.DeptID AND SD.SubDepartmentID = I.SUBDEPARTMENTID " +
                                        " WHERE convert(datetime,PT.TransDate,109) between convert(datetime, cast('" + this.dtpSaleStartDate.Text + " 00:00:00' as datetime) ,113) and convert(datetime, cast('" + this.dtpSaleEndDate.Text + " 23:59:59' as datetime) ,113) " + strFilter + strPayType + strDisc +   //Sprint-21 - 1868 20-Jul-2015 JY Added condition to consider only discounted transactions
                                        " group by Dept.DeptID, SD.SubDepartmentID, PTD.ItemID, CT.CustomerName, CT.Firstname, PT.TransID) z " +
                                    " ) x on 1=1 " + 
                        " WHERE " +
                        " convert(datetime,PT.TransDate,109) between convert(datetime, cast('" + this.dtpSaleStartDate.Text + " 00:00:00' as datetime) ,113) and convert(datetime, cast('" + this.dtpSaleEndDate.Text + " 23:59:59' as datetime) ,113) " +
                        strFilter + strPayType + strDisc +   //Sprint-21 - 1868 20-Jul-2015 JY Added condition to consider only discounted transactions
                        strUserId + //PRIMEPOS-2837 22-Apr-2020 JY Added
                        " group by x. InvoiceDiscount, Dept.DeptID, Dept.DeptName, SD.SubDepartmentID, SD.Description, PTD.ItemID, PTD.ItemDescription, Dept.DeptCode, CT.CustomerName, CT.Firstname, PT.TransID, PT.TransDate " +
                        " ORDER BY Dept.DeptName, SD.Description, PTD.ItemDescription";
                }
                else
                {
                    strSQL = " SELECT " + strTop + " Dept.DeptID, Dept.DeptName, ISNULL(SD.SubDepartmentID,0) AS SubDepartmentID, " +
                        " CASE WHEN ISNULL(SD.SubDepartmentID,0) = 0 THEN 'No Sub-dept' ELSE ISNULL(SD.Description,'') END AS SubDeptName, " +
                        " '' AS TransID, '' AS TransDate, " +
                        " CustomerName + ', ' + FirstName as CustName, PTD.ItemID, PTD.ItemDescription AS ItemName, SUM(PTD.Qty) AS Qty, SUM(PTD.ExtendedPrice-ptd.Discount+PTD.TaxAmount) AS Amount, x.InvoiceDiscount, " +
                        "'" + this.dtpSaleStartDate.Text + "' as StartDate, '" + this.dtpSaleEndDate.Text + "' as EndDate" +
                        ", SUM(PTD.Price) AS SellingPrice, SUM(PTD.ITEMCOST) AS CostPrice, SUM(PTD.ExtendedPrice) AS ExtendedPrice, SUM(PTD.TaxAmount) AS Tax, SUM(ptd.Discount) AS Discount " +
                        " FROM POSTransaction PT " +
                        " INNER JOIN POSTransactionDetail PTD on pt.transid = ptd.transid " +
                        " LEFT JOIN Customer CT on CT.CustomerID = PT.CustomerID " +
                        " INNER JOIN Item I on I.ItemID=PTD.ItemID " +
                        " LEFT JOIN Department Dept on Dept.DeptID=I.DepartmentID " +
                        " LEFT JOIN subDepartment SD on SD.DepartmentID = Dept.DeptID AND SD.SubDepartmentID = I.SUBDEPARTMENTID " +
                        " INNER JOIN (SELECT SUM(InvoiceDiscount) AS InvoiceDiscount FROM " +
                                        " (SELECT PTD.ItemID, CASE WHEN ISNULL(SUM(PT.GrossTotal),0) = 0 then 0 else SUM(ISNULL(PT.INVOICEDISCOUNT,0))*SUM(PTD.ExtendedPrice)/SUM(PT.GrossTotal) END AS InvoiceDiscount FROM POSTransaction PT " +
                                        " INNER JOIN POSTransactionDetail PTD on pt.transid = ptd.transid LEFT JOIN Customer CT on CT.CustomerID = PT.CustomerID INNER JOIN Item I on I.ItemID=PTD.ItemID LEFT JOIN Department Dept on Dept.DeptID=I.DepartmentID LEFT JOIN subDepartment SD on SD.DepartmentID = Dept.DeptID AND SD.SubDepartmentID = I.SUBDEPARTMENTID " +
                                        " WHERE convert(datetime,PT.TransDate,109) between convert(datetime, cast('" + this.dtpSaleStartDate.Text + " 00:00:00' as datetime) ,113) and convert(datetime, cast('" + this.dtpSaleEndDate.Text + " 23:59:59' as datetime) ,113) " + strFilter + strPayType + strDisc +   //Sprint-21 - 1868 20-Jul-2015 JY Added condition to consider only discounted transactions
                                        strUserId + //PRIMEPOS-2837 22-Apr-2020 JY Added
                                        " group by Dept.DeptID, SD.SubDepartmentID, PTD.ItemID, CT.CustomerName, CT.Firstname, PT.TransID) z " +
                                    " ) x on 1 = 1 " + 
                        " WHERE " +
                        " convert(datetime,PT.TransDate,109) between convert(datetime, cast('" + this.dtpSaleStartDate.Text + " 00:00:00' as datetime) ,113) and convert(datetime, cast('" + this.dtpSaleEndDate.Text + " 23:59:59' as datetime) ,113) " +
                        strFilter + strPayType + strDisc +   //Sprint-21 - 1868 20-Jul-2015 JY Added condition to consider only discounted transactions
                        strUserId + //PRIMEPOS-2837 22-Apr-2020 JY Added
                        " group by x.InvoiceDiscount, Dept.DeptID, Dept.DeptName, SD.SubDepartmentID, SD.Description, PTD.ItemID, PTD.ItemDescription, Dept.DeptCode, CT.CustomerName, CT.Firstname " +
                        " ORDER BY Dept.DeptName, SD.Description, PTD.ItemDescription";
                }
            }
            catch (Exception Ex)
            {
                strSQL = "";
            }
        }

        private void grdDept_BeforeRowExpanded(object sender, CancelableRowEventArgs e)
        {
            foreach (Infragistics.Win.UltraWinGrid.UltraGridRow orow in grdDept.DisplayLayout.Rows)
            {
                if (orow.Expanded == true)
                    orow.CollapseAll();
            }
            e.Row.Activate();
        }

        private void grdDept_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                Point point = System.Windows.Forms.Cursor.Position;
                point = this.grdDept.PointToClient(point);
                Infragistics.Win.UIElement oUI = this.grdDept.DisplayLayout.UIElement.ElementFromPoint(point);
                if (oUI == null)
                    return;

                while (oUI != null)
                {
                    if (oUI.GetType() == typeof(Infragistics.Win.UltraWinGrid.RowUIElement))
                    {
                        Infragistics.Win.UltraWinGrid.RowUIElement oRowUI = oUI as Infragistics.Win.UltraWinGrid.RowUIElement;
                        foreach (Infragistics.Win.UltraWinGrid.UltraGridRow orow in grdDept.DisplayLayout.Rows)
                        {
                            orow.CollapseAll();
                        }
                        oRowUI.Row.ExpandAll();
                    }
                    oUI = oUI.Parent;
                }
            }
            catch (Exception exp) { clsUIHelper.ShowErrorMsg(exp.Message); }

        }

        private void grdDept_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            FormatGridColumn(grdDept, 2);
        }

        private void FormatGridColumn(Infragistics.Win.UltraWinGrid.UltraGrid grd, int nBand)
        {
            for (int i = 0; i < nBand; i++)
            {
                for (int j = 0; j < grd.DisplayLayout.Bands[i].Columns.Count; j++)
                {
                    if (grd.DisplayLayout.Bands[i].Columns[j].DataType == typeof(System.Decimal))
                    {
                        grd.DisplayLayout.Bands[i].Columns[j].Format = "#######0.00";
                        grd.DisplayLayout.Bands[i].Columns[j].CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right;
                    }
                }
            }
        }

        private void grdDept_MouseMove(object sender, MouseEventArgs e)
        {
            this.CurrentX = e.X;
            this.CurrentY = e.Y;
        }

        private void PreviewReport(bool blnPrint, bool bCalledFromScheduler = false)   //PRIMEPOS-2485 02-Apr-2021 JY Added bCalledFromScheduler
        {
            String strSQL;
            try
            {
                this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
                rptSalesByDepartment oRpt = new rptSalesByDepartment();

                GenerateSQL(out strSQL, true);

                rptSalesByDepartment oRptDept = new rptSalesByDepartment();

                if (string.IsNullOrEmpty(this.txtCustomer.Text.Trim()) == true)
                {
                    ((CrystalDecisions.CrystalReports.Engine.TextObject)oRptDept.ReportDefinition.ReportObjects["txtCustName"]).Text = "";
                    ((CrystalDecisions.CrystalReports.Engine.TextObject)oRptDept.ReportDefinition.ReportObjects["txtCust"]).Text = "";
                }

                if (optDetail.Checked == true)
                {
                    ((CrystalDecisions.CrystalReports.Engine.TextObject)oRptDept.ReportDefinition.ReportObjects["txtCustName"]).Text = "";
                    ((CrystalDecisions.CrystalReports.Engine.TextObject)oRptDept.ReportDefinition.ReportObjects["txtCust"]).Text = "";
                }
                else
                {
                    if (optSummary.Checked == true)
                    {
                        oRptDept.DetailSection1.SectionFormat.EnableSuppress = true;
                        ((CrystalDecisions.CrystalReports.Engine.TextObject)oRptDept.ReportDefinition.ReportObjects["Text5"]).Text = "";
                        ((CrystalDecisions.CrystalReports.Engine.TextObject)oRptDept.ReportDefinition.ReportObjects["Text9"]).Text = "";
                        ((CrystalDecisions.CrystalReports.Engine.TextObject)oRptDept.ReportDefinition.ReportObjects["Text11"]).Text = "";
                        ((CrystalDecisions.CrystalReports.Engine.TextObject)oRptDept.ReportDefinition.ReportObjects["Text16"]).Text = "";
                        ((CrystalDecisions.CrystalReports.Engine.TextObject)oRptDept.ReportDefinition.ReportObjects["Text17"]).Text = "";

                        if (string.IsNullOrEmpty(this.txtCustomer.Text.Trim()) == false)
                        {
                            ((CrystalDecisions.CrystalReports.Engine.TextObject)oRptDept.ReportDefinition.ReportObjects["txtCustName"]).Text = this.lblCustomerName.Text;
                        }
                        else
                        {
                            ((CrystalDecisions.CrystalReports.Engine.TextObject)oRptDept.ReportDefinition.ReportObjects["txtCustName"]).Text = "";
                            ((CrystalDecisions.CrystalReports.Engine.TextObject)oRptDept.ReportDefinition.ReportObjects["txtCust"]).Text = "";
                        }
                    }

                    ((CrystalDecisions.CrystalReports.Engine.TextObject)oRptDept.ReportDefinition.ReportObjects["txtTransID"]).Text = "";
                    ((CrystalDecisions.CrystalReports.Engine.TextObject)oRptDept.ReportDefinition.ReportObjects["txtTransdate"]).Text = "";
                }
                
                DataSet ds = clsReports.GetReportSource(strSQL);
                oRptDept.SetDataSource(ds.Tables[0]);
                clsReports.DStoExport = ds; //PRIMEPOS-2471 16-Feb-2021 JY Added
                clsReports.Preview(blnPrint, oRptDept, bCalledFromScheduler); //PRIMEPOS-2485 02-Apr-2021 JY Added bCalledFromScheduler
                oReport = oRptDept; //PRIMEPOS-2485 02-Apr-2021 JY Added
                this.Cursor = System.Windows.Forms.Cursors.Default;
            }
            catch (Exception exp)
            {
                this.Cursor = System.Windows.Forms.Cursors.Default;
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }
        #endregion

        #region PRIMEPOS-2485 02-Apr-2021 JY Added
        public bool bSendPrint = true;
        private ReportClass oReport = new ReportClass();
        public usrDateRangeParams customControl;
        private const string ReportName = "SalesByDept";

        public bool CheckTags()
        {
            return true;
        }

        public bool SaveTaskParameters(DataTable dt, int ScheduledTasksID)
        {
            try
            {
                ScheduledTasks oScheduledTasks = new ScheduledTasks();
                oScheduledTasks.SaveTaskParameters(dt, ScheduledTasksID);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool SetControlParameters(int ScheduledTasksID)
        {
            ScheduledTasks oScheduledTasks = new ScheduledTasks();
            DataTable dt = oScheduledTasks.GetScheduledTasksControlsList(ScheduledTasksID);
            customControl.setControlsValues(ref dt);
            setControlsValues(ref dt);  //PRIMEPOS-3066 21-Mar-2022 JY Added
            return true;
        }

        #region PRIMEPOS-3066 21-Mar-2022 JY Added
        public void setControlsValues(ref DataTable dt)
        {
            double Num;

            foreach (DataRow odr in dt.Select("ControlsName = '" + this.dtpSaleStartDate.Tag + " ' "))
            {
                if (double.TryParse(odr["ControlsValue"].ToString().Trim(), out Num))
                {
                    dtpSaleStartDate.Value = DateTime.Now.AddDays(Convert.ToDouble(odr["ControlsValue"].ToString().Trim()) * -1);
                }
                else
                {
                    dtpSaleStartDate.Value = odr["ControlsValue"].ToString().Trim();
                }
            }

            foreach (DataRow odr in dt.Select("ControlsName = '" + this.dtpSaleEndDate.Tag + "' "))
            {
                if (double.TryParse(odr["ControlsValue"].ToString().Trim(), out Num))
                {
                    dtpSaleEndDate.Value = DateTime.Now.AddDays(Convert.ToDouble(odr["ControlsValue"].ToString().Trim()) * -1);
                }
                else
                {
                    dtpSaleEndDate.Value = odr["ControlsValue"].ToString().Trim();
                }
            }
        }
        #endregion

        public bool RunTask(int TaskId, ref string filePath, bool bsendToPrint, ref string sNoOfRecordAffect)
        {
            SetControlParameters(TaskId);
            bSendPrint = bsendToPrint;
            //dtpSaleStartDate.Value = DateTime.Now.AddDays(Left - 60);
            //dtpSaleEndDate.Value = DateTime.Now;
            PreviewReport(bSendPrint, true);
            filePath = Application.StartupPath + @"\" + ReportName + (DateTime.Now).ToString().Replace("/", "").Replace(":", "") + ".pdf";
            this.oReport.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, filePath);
            return true;
        }

        public void GetTaskParameters(ref DataTable dt, int ScheduledTasksID)
        {
            customControl.getControlsValues(ref dt);
        }

        public Control GetParameterControl()
        {
            customControl.setDateTimeControl();
            customControl.Dock = DockStyle.Fill;
            return customControl;
        }

        public bool checkValidation()
        {
            return true;
        }
        #endregion
    }
}