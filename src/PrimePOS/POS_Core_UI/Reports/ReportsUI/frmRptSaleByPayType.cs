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

using POS_Core.DataAccess;
using Infragistics.Win.UltraWinGrid;
using System.Linq;
using CrystalDecisions.CrystalReports.Engine;

namespace POS_Core_UI.Reports.ReportsUI
{
	/// <summary>
	/// Summary description for frmViewTransaction.
	/// </summary>
	public class frmRptSaleByPayType : System.Windows.Forms.Form, ICommandLIneTaskControl
    {
		private System.Windows.Forms.GroupBox groupBox1;
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
		private Infragistics.Win.UltraWinEditors.UltraTextEditor txtStationID;
		private Infragistics.Win.Misc.UltraLabel ultraLabel2;
        private Infragistics.Win.Misc.UltraLabel ultraLabel1;
        private Panel panel1;
        private Infragistics.Win.Misc.UltraButton btnSearch;
        private Infragistics.Win.UltraWinGrid.UltraGrid grdPayType;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

        #region Sprint-19 - 2158 24-Feb-2015 JY Added
        private DataSet dsPayType;
        private SearchSvr oSearchSvr = new SearchSvr();
        private int CurrentX;
        private UltraCheckEditor chkOnlyDiscountedTrans;
        private UltraCheckEditor chkPrimeRxPayTrans; //PRIMEPOS-3282
        private UltraTextEditor txtPaymentType;
        private GroupBox grpPayTypeList;
        private CheckBox chkSelectAll;
        private DataGridView dataGridList;
        private DataGridViewCheckBoxColumn chkBox;
        private Infragistics.Win.Misc.UltraLabel ultraLabel3;
        private Infragistics.Win.Misc.UltraButton btnPayTypeList;
        private int CurrentY;
        #endregion
        private bool GridVisibleFlag = false;   //PRIMEPOS-2596 18-Oct-2018 JY Added
        private bool isPrimeRxPaySelected = false;

        public frmRptSaleByPayType()
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
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinSchedule.CalendarCombo.DateButton dateButton1 = new Infragistics.Win.UltraWinSchedule.CalendarCombo.DateButton();
            Infragistics.Win.UltraWinSchedule.CalendarCombo.DateButton dateButton2 = new Infragistics.Win.UltraWinSchedule.CalendarCombo.DateButton();
            Infragistics.Win.Appearance appearance7 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance8 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance9 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance10 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance11 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance12 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand1 = new Infragistics.Win.UltraWinGrid.UltraGridBand("Band 0", -1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn4 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Column Name");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn5 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Type");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn6 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Criteria Value");
            Infragistics.Win.Appearance appearance13 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance14 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance15 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance16 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance17 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance18 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance19 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance20 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance21 = new Infragistics.Win.Appearance();
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
            Infragistics.Win.UltraWinScrollBar.ScrollBarLook scrollBarLook1 = new Infragistics.Win.UltraWinScrollBar.ScrollBarLook();
            Infragistics.Win.Appearance appearance32 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance33 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance34 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance35 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance36 = new Infragistics.Win.Appearance(); //PRIMEPOS-3282
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnPayTypeList = new Infragistics.Win.Misc.UltraButton();
            this.ultraLabel3 = new Infragistics.Win.Misc.UltraLabel();
            this.chkOnlyDiscountedTrans = new Infragistics.Win.UltraWinEditors.UltraCheckEditor();
            this.chkPrimeRxPayTrans = new Infragistics.Win.UltraWinEditors.UltraCheckEditor(); //PRIMEPOS-3282
            this.btnSearch = new Infragistics.Win.Misc.UltraButton();
            this.panel1 = new System.Windows.Forms.Panel();
            this.ultraLabel1 = new Infragistics.Win.Misc.UltraLabel();
            this.txtStationID = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.ultraLabel2 = new Infragistics.Win.Misc.UltraLabel();
            this.optDetail = new System.Windows.Forms.RadioButton();
            this.optSummary = new System.Windows.Forms.RadioButton();
            this.dtpSaleEndDate = new Infragistics.Win.UltraWinSchedule.UltraCalendarCombo();
            this.dtpSaleStartDate = new Infragistics.Win.UltraWinSchedule.UltraCalendarCombo();
            this.ultraLabel20 = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabel19 = new Infragistics.Win.Misc.UltraLabel();
            this.txtPaymentType = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnPrint = new Infragistics.Win.Misc.UltraButton();
            this.btnClose = new Infragistics.Win.Misc.UltraButton();
            this.btnView = new Infragistics.Win.Misc.UltraButton();
            this.grdPayType = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.grpPayTypeList = new System.Windows.Forms.GroupBox();
            this.chkSelectAll = new System.Windows.Forms.CheckBox();
            this.dataGridList = new System.Windows.Forms.DataGridView();
            this.chkBox = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chkOnlyDiscountedTrans)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtStationID)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtpSaleEndDate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtpSaleStartDate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPaymentType)).BeginInit();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdPayType)).BeginInit();
            this.grpPayTypeList.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridList)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.btnPayTypeList);
            this.groupBox1.Controls.Add(this.ultraLabel3);
            this.groupBox1.Controls.Add(this.chkOnlyDiscountedTrans);
            this.groupBox1.Controls.Add(this.chkPrimeRxPayTrans); //PRIMEPOS-3282
            this.groupBox1.Controls.Add(this.btnSearch);
            this.groupBox1.Controls.Add(this.panel1);
            this.groupBox1.Controls.Add(this.txtStationID);
            this.groupBox1.Controls.Add(this.ultraLabel2);
            this.groupBox1.Controls.Add(this.optDetail);
            this.groupBox1.Controls.Add(this.optSummary);
            this.groupBox1.Controls.Add(this.dtpSaleEndDate);
            this.groupBox1.Controls.Add(this.dtpSaleStartDate);
            this.groupBox1.Controls.Add(this.ultraLabel20);
            this.groupBox1.Controls.Add(this.ultraLabel19);
            this.groupBox1.Controls.Add(this.txtPaymentType);
            this.groupBox1.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.groupBox1.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(5, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(974, 103);
            this.groupBox1.TabIndex = 27;
            this.groupBox1.TabStop = false;
            // 
            // btnPayTypeList
            // 
            appearance1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(62)))), ((int)(((byte)(176)))));
            appearance1.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance1.FontData.BoldAsString = "True";
            appearance1.ForeColor = System.Drawing.Color.White;
            this.btnPayTypeList.Appearance = appearance1;
            this.btnPayTypeList.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            this.btnPayTypeList.Location = new System.Drawing.Point(708, 15);
            this.btnPayTypeList.Name = "btnPayTypeList";
            this.btnPayTypeList.Size = new System.Drawing.Size(144, 23);
            this.btnPayTypeList.TabIndex = 4;
            this.btnPayTypeList.Text = "Pay Types";
            this.btnPayTypeList.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnPayTypeList.Click += new System.EventHandler(this.btnPayTypeList_Click);
            this.btnPayTypeList.KeyDown += new System.Windows.Forms.KeyEventHandler(this.btnPayTypeList_KeyDown);
            this.btnPayTypeList.KeyUp += new System.Windows.Forms.KeyEventHandler(this.btnPayTypeList_KeyUp);
            // 
            // ultraLabel3
            // 
            appearance2.ForeColor = System.Drawing.Color.White;
            this.ultraLabel3.Appearance = appearance2;
            this.ultraLabel3.Location = new System.Drawing.Point(535, 16);
            this.ultraLabel3.Name = "ultraLabel3";
            this.ultraLabel3.Size = new System.Drawing.Size(178, 20);
            this.ultraLabel3.TabIndex = 40;
            this.ultraLabel3.Text = "Pay Types <Blank = All>";
            // 
            // chkOnlyDiscountedTrans
            // 
            appearance3.ForeColor = System.Drawing.Color.White;
            this.chkOnlyDiscountedTrans.Appearance = appearance3;
            this.chkOnlyDiscountedTrans.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkOnlyDiscountedTrans.Location = new System.Drawing.Point(218, 44);
            this.chkOnlyDiscountedTrans.Name = "chkOnlyDiscountedTrans";
            this.chkOnlyDiscountedTrans.Size = new System.Drawing.Size(194, 20);
            this.chkOnlyDiscountedTrans.TabIndex = 3;
            this.chkOnlyDiscountedTrans.Text = "Only Discounted Trans";
            #region PRIMEPOS-3282
            // 
            // chkPrimeRxPayTrans
            // 
            appearance36.ForeColor = System.Drawing.Color.White;
            this.chkPrimeRxPayTrans.Appearance = appearance36;
            this.chkPrimeRxPayTrans.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkPrimeRxPayTrans.Location = new System.Drawing.Point(708, 70);
            this.chkPrimeRxPayTrans.Name = "chkPrimeRxPayTrans";
            this.chkPrimeRxPayTrans.Size = new System.Drawing.Size(134, 20);
            this.chkPrimeRxPayTrans.TabIndex = 63;
            this.chkPrimeRxPayTrans.Text = "PrimeRxPay";
            #endregion
            // 
            // btnSearch
            // 
            appearance4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(62)))), ((int)(((byte)(176)))));
            appearance4.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance4.FontData.BoldAsString = "True";
            appearance4.ForeColor = System.Drawing.Color.White;
            this.btnSearch.Appearance = appearance4;
            this.btnSearch.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            this.btnSearch.Location = new System.Drawing.Point(851, 69);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(117, 23);
            this.btnSearch.TabIndex = 7;
            this.btnSearch.Text = "&Search (F4)";
            this.btnSearch.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // panel1
            // 
            this.panel1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.Controls.Add(this.ultraLabel1);
            this.panel1.Location = new System.Drawing.Point(8, 72);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(515, 25);
            this.panel1.TabIndex = 30;
            // 
            // ultraLabel1
            // 
            appearance5.BackColor = System.Drawing.Color.White;
            appearance5.ForeColor = System.Drawing.Color.Red;
            this.ultraLabel1.Appearance = appearance5;
            this.ultraLabel1.AutoSize = true;
            this.ultraLabel1.Location = new System.Drawing.Point(19, 5);
            this.ultraLabel1.Name = "ultraLabel1";
            this.ultraLabel1.Size = new System.Drawing.Size(321, 18);
            this.ultraLabel1.TabIndex = 29;
            this.ultraLabel1.Text = "Auth no prefix \"M:\" denotes manual CC Trans";
            // 
            // txtStationID
            // 
            this.txtStationID.AutoSize = false;
            this.txtStationID.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.txtStationID.Location = new System.Drawing.Point(400, 16);
            this.txtStationID.MaxLength = 15;
            this.txtStationID.Name = "txtStationID";
            this.txtStationID.Size = new System.Drawing.Size(123, 20);
            this.txtStationID.TabIndex = 2;
            this.txtStationID.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // ultraLabel2
            // 
            appearance6.ForeColor = System.Drawing.Color.White;
            this.ultraLabel2.Appearance = appearance6;
            this.ultraLabel2.Location = new System.Drawing.Point(218, 16);
            this.ultraLabel2.Name = "ultraLabel2";
            this.ultraLabel2.Size = new System.Drawing.Size(175, 20);
            this.ultraLabel2.TabIndex = 28;
            this.ultraLabel2.Text = "Station # <Blank = All>";
            // 
            // optDetail
            // 
            this.optDetail.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.optDetail.ForeColor = System.Drawing.Color.White;
            this.optDetail.Location = new System.Drawing.Point(639, 70);
            this.optDetail.Name = "optDetail";
            this.optDetail.Size = new System.Drawing.Size(64, 20);
            this.optDetail.TabIndex = 6;
            this.optDetail.Text = "Detail";
            // 
            // optSummary
            // 
            this.optSummary.Checked = true;
            this.optSummary.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.optSummary.ForeColor = System.Drawing.Color.White;
            this.optSummary.Location = new System.Drawing.Point(540, 70);
            this.optSummary.Name = "optSummary";
            this.optSummary.Size = new System.Drawing.Size(93, 20);
            this.optSummary.TabIndex = 5;
            this.optSummary.TabStop = true;
            this.optSummary.Text = "Summary";
            // 
            // dtpSaleEndDate
            // 
            this.dtpSaleEndDate.AllowNull = false;
            this.dtpSaleEndDate.AutoSize = false;
            this.dtpSaleEndDate.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Office2003ToolbarButton;
            this.dtpSaleEndDate.DateButtons.Add(dateButton1);
            this.dtpSaleEndDate.Location = new System.Drawing.Point(90, 44);
            this.dtpSaleEndDate.Name = "dtpSaleEndDate";
            this.dtpSaleEndDate.NonAutoSizeHeight = 10;
            this.dtpSaleEndDate.Size = new System.Drawing.Size(123, 20);
            this.dtpSaleEndDate.TabIndex = 1;
            this.dtpSaleEndDate.Tag = "To Date";
            this.dtpSaleEndDate.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.dtpSaleEndDate.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.dtpSaleEndDate.Value = new System.DateTime(2004, 5, 25, 0, 0, 0, 0);
            // 
            // dtpSaleStartDate
            // 
            this.dtpSaleStartDate.AllowNull = false;
            this.dtpSaleStartDate.AutoSize = false;
            this.dtpSaleStartDate.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Office2003ToolbarButton;
            this.dtpSaleStartDate.DateButtons.Add(dateButton2);
            this.dtpSaleStartDate.Location = new System.Drawing.Point(90, 16);
            this.dtpSaleStartDate.Name = "dtpSaleStartDate";
            this.dtpSaleStartDate.NonAutoSizeHeight = 10;
            this.dtpSaleStartDate.Size = new System.Drawing.Size(123, 20);
            this.dtpSaleStartDate.TabIndex = 0;
            this.dtpSaleStartDate.Tag = "From Date";
            this.dtpSaleStartDate.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.dtpSaleStartDate.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.dtpSaleStartDate.Value = new System.DateTime(2004, 5, 25, 0, 0, 0, 0);
            // 
            // ultraLabel20
            // 
            appearance7.ForeColor = System.Drawing.Color.White;
            this.ultraLabel20.Appearance = appearance7;
            this.ultraLabel20.Location = new System.Drawing.Point(8, 16);
            this.ultraLabel20.Name = "ultraLabel20";
            this.ultraLabel20.Size = new System.Drawing.Size(77, 20);
            this.ultraLabel20.TabIndex = 20;
            this.ultraLabel20.Text = "Start Date";
            // 
            // ultraLabel19
            // 
            appearance8.ForeColor = System.Drawing.Color.White;
            this.ultraLabel19.Appearance = appearance8;
            this.ultraLabel19.Location = new System.Drawing.Point(8, 44);
            this.ultraLabel19.Name = "ultraLabel19";
            this.ultraLabel19.Size = new System.Drawing.Size(77, 20);
            this.ultraLabel19.TabIndex = 22;
            this.ultraLabel19.Text = "End Date";
            // 
            // txtPaymentType
            // 
            this.txtPaymentType.AutoSize = false;
            this.txtPaymentType.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.txtPaymentType.Location = new System.Drawing.Point(535, 44);
            this.txtPaymentType.Name = "txtPaymentType";
            this.txtPaymentType.ReadOnly = true;
            this.txtPaymentType.Size = new System.Drawing.Size(318, 20);
            this.txtPaymentType.TabIndex = 38;
            this.txtPaymentType.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
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
            this.groupBox2.Location = new System.Drawing.Point(5, 409);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(974, 47);
            this.groupBox2.TabIndex = 30;
            this.groupBox2.TabStop = false;
            // 
            // btnPrint
            // 
            this.btnPrint.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            appearance9.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(62)))), ((int)(((byte)(176)))));
            appearance9.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance9.FontData.BoldAsString = "True";
            appearance9.ForeColor = System.Drawing.Color.White;
            this.btnPrint.Appearance = appearance9;
            this.btnPrint.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            this.btnPrint.Location = new System.Drawing.Point(698, 14);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(85, 26);
            this.btnPrint.TabIndex = 8;
            this.btnPrint.Text = "&Print";
            this.btnPrint.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            appearance10.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(62)))), ((int)(((byte)(176)))));
            appearance10.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance10.FontData.BoldAsString = "True";
            appearance10.ForeColor = System.Drawing.Color.White;
            this.btnClose.Appearance = appearance10;
            this.btnClose.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(882, 15);
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
            appearance11.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(62)))), ((int)(((byte)(176)))));
            appearance11.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance11.FontData.BoldAsString = "True";
            appearance11.ForeColor = System.Drawing.Color.White;
            this.btnView.Appearance = appearance11;
            this.btnView.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            this.btnView.Location = new System.Drawing.Point(790, 15);
            this.btnView.Name = "btnView";
            this.btnView.Size = new System.Drawing.Size(85, 26);
            this.btnView.TabIndex = 6;
            this.btnView.Text = "&View";
            this.btnView.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnView.Click += new System.EventHandler(this.btnView_Click_1);
            // 
            // grdPayType
            // 
            this.grdPayType.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            appearance12.BackColor = System.Drawing.Color.White;
            appearance12.BackColor2 = System.Drawing.Color.White;
            appearance12.BackColorDisabled = System.Drawing.Color.White;
            appearance12.BackColorDisabled2 = System.Drawing.Color.White;
            appearance12.BackGradientStyle = Infragistics.Win.GradientStyle.None;
            this.grdPayType.DisplayLayout.Appearance = appearance12;
            ultraGridColumn4.Header.VisiblePosition = 0;
            ultraGridColumn5.Header.VisiblePosition = 1;
            ultraGridColumn6.Header.VisiblePosition = 2;
            ultraGridBand1.Columns.AddRange(new object[] {
            ultraGridColumn4,
            ultraGridColumn5,
            ultraGridColumn6});
            ultraGridBand1.HeaderVisible = true;
            this.grdPayType.DisplayLayout.BandsSerializer.Add(ultraGridBand1);
            this.grdPayType.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.grdPayType.DisplayLayout.InterBandSpacing = 10;
            this.grdPayType.DisplayLayout.MaxColScrollRegions = 1;
            this.grdPayType.DisplayLayout.MaxRowScrollRegions = 1;
            appearance13.BackColor = System.Drawing.Color.White;
            appearance13.BackColor2 = System.Drawing.Color.White;
            this.grdPayType.DisplayLayout.Override.ActiveCardCaptionAppearance = appearance13;
            appearance14.BackColor = System.Drawing.Color.White;
            appearance14.BackColor2 = System.Drawing.Color.White;
            appearance14.BorderColor = System.Drawing.Color.Gray;
            this.grdPayType.DisplayLayout.Override.ActiveRowAppearance = appearance14;
            appearance15.BackColor = System.Drawing.Color.White;
            appearance15.BackColor2 = System.Drawing.Color.White;
            appearance15.BorderColor = System.Drawing.Color.Gray;
            this.grdPayType.DisplayLayout.Override.AddRowAppearance = appearance15;
            this.grdPayType.DisplayLayout.Override.AllowAddNew = Infragistics.Win.UltraWinGrid.AllowAddNew.No;
            this.grdPayType.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.False;
            this.grdPayType.DisplayLayout.Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.False;
            appearance16.BackColor = System.Drawing.Color.Transparent;
            this.grdPayType.DisplayLayout.Override.CardAreaAppearance = appearance16;
            appearance17.BackColor = System.Drawing.Color.White;
            appearance17.BackColor2 = System.Drawing.Color.White;
            appearance17.BackColorDisabled = System.Drawing.Color.White;
            appearance17.BackColorDisabled2 = System.Drawing.Color.White;
            appearance17.BorderColor = System.Drawing.Color.Black;
            appearance17.BorderColor3DBase = System.Drawing.Color.Black;
            this.grdPayType.DisplayLayout.Override.CellAppearance = appearance17;
            appearance18.BackColor = System.Drawing.Color.White;
            appearance18.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(231)))), ((int)(((byte)(230)))));
            appearance18.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance18.BorderAlpha = Infragistics.Win.Alpha.Transparent;
            appearance18.BorderColor = System.Drawing.Color.Gray;
            appearance18.BorderColor3DBase = System.Drawing.Color.Gray;
            appearance18.ImageBackgroundAlpha = Infragistics.Win.Alpha.Transparent;
            appearance18.ImageBackgroundStyle = Infragistics.Win.ImageBackgroundStyle.Centered;
            this.grdPayType.DisplayLayout.Override.CellButtonAppearance = appearance18;
            appearance19.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            appearance19.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.grdPayType.DisplayLayout.Override.EditCellAppearance = appearance19;
            appearance20.BorderColor = System.Drawing.Color.Gray;
            this.grdPayType.DisplayLayout.Override.FilteredInRowAppearance = appearance20;
            appearance21.BorderColor = System.Drawing.Color.Gray;
            this.grdPayType.DisplayLayout.Override.FilteredOutRowAppearance = appearance21;
            appearance22.BackColor = System.Drawing.Color.White;
            appearance22.BackColor2 = System.Drawing.Color.White;
            appearance22.BackColorDisabled = System.Drawing.Color.White;
            appearance22.BackColorDisabled2 = System.Drawing.Color.White;
            this.grdPayType.DisplayLayout.Override.FixedCellAppearance = appearance22;
            appearance23.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            appearance23.BackColor2 = System.Drawing.Color.Beige;
            this.grdPayType.DisplayLayout.Override.FixedHeaderAppearance = appearance23;
            appearance24.BackColor = System.Drawing.Color.White;
            appearance24.BackColor2 = System.Drawing.SystemColors.Control;
            appearance24.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance24.FontData.BoldAsString = "True";
            appearance24.ForeColor = System.Drawing.Color.Black;
            appearance24.TextHAlignAsString = "Left";
            appearance24.ThemedElementAlpha = Infragistics.Win.Alpha.Transparent;
            this.grdPayType.DisplayLayout.Override.HeaderAppearance = appearance24;
            appearance25.BorderColor = System.Drawing.Color.Gray;
            this.grdPayType.DisplayLayout.Override.RowAlternateAppearance = appearance25;
            appearance26.BackColor = System.Drawing.Color.White;
            appearance26.BackColor2 = System.Drawing.Color.White;
            appearance26.BackGradientStyle = Infragistics.Win.GradientStyle.None;
            appearance26.BackHatchStyle = Infragistics.Win.BackHatchStyle.None;
            appearance26.BorderColor = System.Drawing.Color.Gray;
            this.grdPayType.DisplayLayout.Override.RowAppearance = appearance26;
            appearance27.BorderColor = System.Drawing.Color.Gray;
            this.grdPayType.DisplayLayout.Override.RowPreviewAppearance = appearance27;
            appearance28.BackColor = System.Drawing.Color.White;
            appearance28.BackColor2 = System.Drawing.SystemColors.Control;
            appearance28.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance28.BorderColor = System.Drawing.Color.Gray;
            this.grdPayType.DisplayLayout.Override.RowSelectorAppearance = appearance28;
            this.grdPayType.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            this.grdPayType.DisplayLayout.Override.RowSelectorWidth = 12;
            this.grdPayType.DisplayLayout.Override.RowSpacingBefore = 2;
            appearance29.BackColor = System.Drawing.Color.Navy;
            appearance29.BackColorAlpha = Infragistics.Win.Alpha.Opaque;
            this.grdPayType.DisplayLayout.Override.SelectedCellAppearance = appearance29;
            appearance30.BackColor = System.Drawing.Color.Navy;
            appearance30.BackColorDisabled = System.Drawing.Color.Navy;
            appearance30.BackGradientStyle = Infragistics.Win.GradientStyle.None;
            appearance30.BackHatchStyle = Infragistics.Win.BackHatchStyle.None;
            appearance30.BorderColor = System.Drawing.Color.Gray;
            appearance30.ForeColor = System.Drawing.Color.Black;
            this.grdPayType.DisplayLayout.Override.SelectedRowAppearance = appearance30;
            this.grdPayType.DisplayLayout.Override.SelectTypeCell = Infragistics.Win.UltraWinGrid.SelectType.None;
            this.grdPayType.DisplayLayout.Override.SelectTypeCol = Infragistics.Win.UltraWinGrid.SelectType.None;
            this.grdPayType.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.Single;
            appearance31.BorderColor = System.Drawing.Color.Gray;
            this.grdPayType.DisplayLayout.Override.TemplateAddRowAppearance = appearance31;
            this.grdPayType.DisplayLayout.RowConnectorColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            this.grdPayType.DisplayLayout.RowConnectorStyle = Infragistics.Win.UltraWinGrid.RowConnectorStyle.Solid;
            appearance32.BackColor = System.Drawing.Color.White;
            appearance32.BackColor2 = System.Drawing.SystemColors.Control;
            appearance32.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            scrollBarLook1.Appearance = appearance32;
            appearance33.BackColor = System.Drawing.Color.White;
            appearance33.BackColor2 = System.Drawing.SystemColors.Control;
            appearance33.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance33.ThemedElementAlpha = Infragistics.Win.Alpha.Transparent;
            scrollBarLook1.ButtonAppearance = appearance33;
            appearance34.BackColor = System.Drawing.Color.White;
            appearance34.BackColor2 = System.Drawing.SystemColors.Control;
            scrollBarLook1.ThumbAppearance = appearance34;
            appearance35.BackColor = System.Drawing.Color.White;
            appearance35.BackColor2 = System.Drawing.Color.White;
            scrollBarLook1.TrackAppearance = appearance35;
            this.grdPayType.DisplayLayout.ScrollBarLook = scrollBarLook1;
            this.grdPayType.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.grdPayType.Location = new System.Drawing.Point(5, 109);
            this.grdPayType.Name = "grdPayType";
            this.grdPayType.Size = new System.Drawing.Size(974, 302);
            this.grdPayType.TabIndex = 31;
            this.grdPayType.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.grdPayType.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.grdPayType_InitializeLayout);
            this.grdPayType.BeforeRowExpanded += new Infragistics.Win.UltraWinGrid.CancelableRowEventHandler(this.grdPayType_BeforeRowExpanded);
            this.grdPayType.DoubleClick += new System.EventHandler(this.grdPayType_DoubleClick);
            this.grdPayType.MouseMove += new System.Windows.Forms.MouseEventHandler(this.grdPayType_MouseMove);
            // 
            // grpPayTypeList
            // 
            this.grpPayTypeList.Controls.Add(this.chkSelectAll);
            this.grpPayTypeList.Controls.Add(this.dataGridList);
            this.grpPayTypeList.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpPayTypeList.Location = new System.Drawing.Point(712, 42);
            this.grpPayTypeList.Name = "grpPayTypeList";
            this.grpPayTypeList.Size = new System.Drawing.Size(144, 61);
            this.grpPayTypeList.TabIndex = 39;
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
            // frmRptSaleByPayType
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(108)))), ((int)(((byte)(172)))));
            this.ClientSize = new System.Drawing.Size(984, 462);
            this.Controls.Add(this.grpPayTypeList);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.grdPayType);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.Name = "frmRptSaleByPayType";
            this.Text = "Sales By Pay Type";
            this.Load += new System.EventHandler(this.frmViewTransaction_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmViewTransaction_KeyDown);
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.chkOnlyDiscountedTrans)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtStationID)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtpSaleEndDate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtpSaleStartDate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPaymentType)).EndInit();
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdPayType)).EndInit();
            this.grpPayTypeList.ResumeLayout(false);
            this.grpPayTypeList.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridList)).EndInit();
            this.ResumeLayout(false);

		}
		#endregion

		private void frmViewTransaction_Load(object sender, System.EventArgs e)
		{
            this.dtpSaleStartDate.Enter += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
			this.dtpSaleStartDate.Leave += new System.EventHandler(clsUIHelper.AfterExitEditMode);

			this.dtpSaleEndDate.Enter += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
			this.dtpSaleEndDate.Leave += new System.EventHandler(clsUIHelper.AfterExitEditMode);
			
			this.Left=(frmMain.getInstance().Width-frmMain.getInstance().ultraExplorerBar1.Width-this.Width)/2;
			this.Top=(frmMain.getInstance().Height-this.Height)/2;
			
			clsUIHelper.setColorSchecme(this);
			this.dtpSaleEndDate.Value=DateTime.Now;
			this.dtpSaleStartDate.Value=DateTime.Now;
			this.optSummary.Checked=true;

            FillPayType();  //PRIMEPOS-2596 18-Oct-2018 JY Added

            #region Sprint-19 - 2158 24-Feb-2015 JY Added
            this.Location = new Point(45, 2);
            clsUIHelper.SetAppearance(this.grdPayType);
            clsUIHelper.SetReadonlyRow(this.grdPayType);
            grdPayType.DisplayLayout.Override.HeaderClickAction = HeaderClickAction.SortMulti;
            grdPayType.DisplayLayout.AutoFitStyle = AutoFitStyle.ResizeAllColumns;
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
				if(e.KeyData==System.Windows.Forms.Keys.Enter)
				{
					this.SelectNextControl(this.ActiveControl,true,true,true,true);
				}
				else if (e.KeyData==Keys.Escape)
					this.Close();
                else if (e.KeyData == System.Windows.Forms.Keys.F4) //Sprint-19 - 2158 24-Feb-2015 JY Added 
                    btnSearch_Click(sender, e);
			}
			catch(Exception exp)
			{
				clsUIHelper.ShowErrorMsg( exp.Message);
			}
		}

        #region Sprint-19 - 2158 25-Feb-2015 JY Commented
        //private void PreviewReport(bool blnPrint)
        //{
        //    try
        //    {
        //        this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
        //        rptSalePaType oRpt = new rptSalePaType();

        //        String strQuery;

        //        //Sprint-19 - 2139 07-Jan-2015 JY Commented old query
        //        //strQuery="select Posset.stationname,PT.TransID,PTp.PayTypeDesc,PT.TransDate,Case TransType when 1 Then  Pty.amount else 0 end as Sale,Case TransType when 2 Then  Pty.amount  else 0 end as [Returns],Case TransType when 3 Then  Pty.amount  else 0 end as ROA,PTy.RefNo,PTy.AuthNo " +
        //        //" ,'" + this.dtpSaleStartDate.Text + "' as StartDate, '" + this.dtpSaleEndDate.Text + "' as EndDate from postransaction PT left join Util_POSSet POSSet  on(POSSet.StationID=PT.StationID),POSTransPayment PTy, PayType PTp where PT.TransID=PTy.TransID and PTy.TransTypeCode=PTp.PayTypeID " +
        //        //" and  convert(datetime,PT.TransDate,109) between convert(datetime, cast('" + this.dtpSaleStartDate.Text  + " 00:00:00' as datetime) ,113) and convert(datetime, cast('" + this.dtpSaleEndDate.Text  + " 23:59:59' as datetime) ,113) ";

        //        //Sprint-19 - 2139 07-Jan-2015 JY Added updated query - concat "M:" with AuthNo for manual CC trans
        //        strQuery = "select Posset.stationname,PT.TransID,PTp.PayTypeDesc,PT.TransDate,Case TransType when 1 Then  Pty.amount else 0 end as Sale,Case TransType when 2 Then  Pty.amount  else 0 end as [Returns],Case TransType when 3 Then  Pty.amount  else 0 end as ROA,PTy.RefNo, " +
        //            " CASE WHEN PTy.IsManual = 1 THEN 'M:' + PTy.AuthNo ELSE PTy.AuthNo END AuthNo " +
        //            " ,'" + this.dtpSaleStartDate.Text + "' as StartDate, '" + this.dtpSaleEndDate.Text + "' as EndDate from postransaction PT left join Util_POSSet POSSet  on(POSSet.StationID=PT.StationID),POSTransPayment PTy, PayType PTp where PT.TransID=PTy.TransID and PTy.TransTypeCode=PTp.PayTypeID " +
        //            " and  convert(datetime,PT.TransDate,109) between convert(datetime, cast('" + this.dtpSaleStartDate.Text + " 00:00:00' as datetime) ,113) and convert(datetime, cast('" + this.dtpSaleEndDate.Text + " 23:59:59' as datetime) ,113) ";

        //        if (this.cboTransType.SelectedIndex>0)
        //            strQuery+=" and PTy.TransTypeCode='" + cboTransType.SelectedItem.DataValue.ToString().Trim() + "'";
        //        /*Search oSearch=new Search();
        //        DataSet ds = oSearch.Search(strQuery);
        //        oRpt.SetDataSource(ds);
        //        */
        //        if (this.optSummary.Checked==true)
        //        {
        //            oRpt.DetailSection1.SectionFormat.EnableSuppress=true;
        //        }
				
        //        if (this.txtStationID.Text.Trim()!="")
        //        {
        //            //strQuery+=" and posset.stationname='" + this.txtStationID.Text.Replace("'","''") + "' ";//Orignal
        //            //Following Added by Krishna on 7 October 2011
        //            if (this.txtStationID.Text.Length == 1)
        //                txtStationID.Text = "0" + this.txtStationID.Text;
        //            strQuery += " and posset.STATIONID='" + this.txtStationID.Text.Replace("'", "''") + "' ";
        //            //End of Added by Krishna on 7 October 2011
        //        }

        //            /*				frmReportViewer oViewer=new frmReportViewer();
        //                            oViewer.rvReportViewer.ReportSource=oRpt;
        //                            oViewer.MdiParent=frmMain.getInstance();
        //                            oViewer.WindowState=FormWindowState.Maximized;
        //                            oViewer.Show();
        //                            */
        //        strQuery += " ORDER BY PTp.PayTypeDesc, PT.TransDate ";
        //        clsReports.Preview(blnPrint,strQuery,oRpt);
        //        this.Cursor = System.Windows.Forms.Cursors.Default;

        //    }
        //    catch(Exception exp)
        //    {
        //        this.Cursor = System.Windows.Forms.Cursors.Default;
        //        clsUIHelper.ShowErrorMsg(exp.Message);
        //    }

        //}
        #endregion

        private void btnPrint_Click(object sender, System.EventArgs e)
		{
			this.dtpSaleStartDate.Focus();
			PreviewReport(true);
		}

		private void btnView_Click_1(object sender, System.EventArgs e)
		{
			this.dtpSaleStartDate.Focus();
			PreviewReport(false);
        }

        #region Sprint-19 - 2158 25-Feb-2015 JY Added
        private void btnSearch_Click(object sender, EventArgs e)
        {
            String strPayType;
            try
            {
                dsPayType = new DataSet();
                DataSet dsPayTypeTmp = new DataSet();

                GenerateSQL(out strPayType);

                dsPayTypeTmp.Tables.Add(oSearchSvr.Search(strPayType).Tables[0].Copy());

                var PayTypeSummary = from PayType in dsPayTypeTmp.Tables[0].AsEnumerable()
                                     group PayType by PayType.Field<string>("PayTypeDesc") into userg
                                   select new
                                   {
                                       PayTypeDesc = userg.Key,
                                       Sale = userg.Sum(x => x.Field<decimal>("Sale")),
                                       Returns = userg.Sum(x => x.Field<decimal>("Returns")),
                                       ROA = userg.Sum(x => x.Field<decimal>("ROA"))
                                   };

                DataTable dt = new DataTable();
                dt.Columns.Add("Pay Type", typeof(System.String));
                dt.Columns.Add("Sale", typeof(System.Decimal));
                dt.Columns.Add("Returns", typeof(System.Decimal));
                dt.Columns.Add("ROA", typeof(System.Decimal));
                dt.Columns.Add("Net Sale", typeof(System.Decimal));
                DataRow dr;

                foreach (var Summary in PayTypeSummary)
                {
                    dr = dt.NewRow();
                    dr["Pay Type"] = Summary.PayTypeDesc;
                    dr["Sale"] = Summary.Sale;
                    dr["Returns"] = Summary.Returns;
                    dr["ROA"] = Summary.ROA;
                    dr["Net Sale"] = Summary.Sale + Summary.ROA - Math.Abs(Summary.Returns);

                    dt.Rows.Add(dr);
                }

                dsPayType.Tables.Add(dt);   //Added table for summary
                dsPayType.Tables[0].TableName = "Summary";
                dsPayType.Tables.Add(dsPayTypeTmp.Tables[0].Copy());
                dsPayType.Tables[1].TableName = "Details";

                dsPayType.Relations.Add("Details", dsPayType.Tables[0].Columns["Pay Type"], dsPayType.Tables[1].Columns["PayTypeDesc"]);

                grdPayType.DataSource = dsPayType;
                grdPayType.DisplayLayout.Bands[0].HeaderVisible = true;
                grdPayType.DisplayLayout.Bands[0].Header.Appearance.FontData.SizeInPoints = 10;
                grdPayType.DisplayLayout.Bands[0].Header.Appearance.TextHAlign = Infragistics.Win.HAlign.Center;
                grdPayType.DisplayLayout.Bands[0].Header.Caption = "PayType wise Sales Summary";
                grdPayType.DisplayLayout.Bands[0].Override.WrapHeaderText = Infragistics.Win.DefaultableBoolean.True;

                grdPayType.DisplayLayout.Bands[1].HeaderVisible = true;
                grdPayType.DisplayLayout.Bands[1].Header.Caption = "PayType wise Sales Detail";
                grdPayType.DisplayLayout.Bands[1].Header.Appearance.FontData.SizeInPoints = 10;
                grdPayType.DisplayLayout.Bands[1].Header.Appearance.TextHAlign = Infragistics.Win.HAlign.Center;
                grdPayType.DisplayLayout.Bands[1].Expandable = true;
                grdPayType.DisplayLayout.Bands[1].Override.WrapHeaderText = Infragistics.Win.DefaultableBoolean.True;
                grdPayType.DisplayLayout.Bands[1].Columns["PayTypeDesc"].Hidden = true;
                grdPayType.DisplayLayout.Bands[1].Columns["StartDate"].Hidden = true;
                grdPayType.DisplayLayout.Bands[1].Columns["EndDate"].Hidden = true;

                resizeColumns(grdPayType);
                grdPayType.PerformAction(UltraGridAction.FirstRowInGrid);
                grdPayType.Refresh();
            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
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

        private void GenerateSQL(out string strSQL)
        {
            string strFilter = string.Empty;

            try
            {
                if (Convert.ToString(this.txtPaymentType.Tag).Trim() != "")
                {
                    //strFilter += " AND PTY.TransTypeCode IN (" + txtPaymentType.Tag.ToString().Trim() + ")";
                    //if (isPrimeRxPaySelected) //PRIMEPOS-3282
                    if (chkPrimeRxPayTrans.Checked) //PRIMEPOS-3282
                    {
                        strFilter += " AND (PTY.PAYMENTPROCESSOR='PRIMERXPAY') ";
                    }
                    else
                    {
                        strFilter += " AND PTY.TransTypeCode IN (" + txtPaymentType.Tag.ToString().Trim() + ")";
                    }
                }
                else if (chkPrimeRxPayTrans.Checked) //PRIMEPOS-3282
                {
                    strFilter += " AND PTY.PAYMENTPROCESSOR='PRIMERXPAY' ";
                }

                if (this.txtStationID.Text.Trim() != "")
                {
                    if (this.txtStationID.Text.Trim().Length == 1)
                        txtStationID.Text = "0" + this.txtStationID.Text.Trim();
                    strFilter += " AND posset.STATIONID = '" + this.txtStationID.Text.Replace("'", "''") + "' ";
                }

                if (chkOnlyDiscountedTrans.Checked) //Sprint-21 - 1868 20-Jul-2015 JY Added condition to consider only discounted transactions
                    strFilter += " AND PT.TotalDiscAmount <> 0 ";

                //Sprint-19 - 2139 07-Jan-2015 JY Added updated query - concat "M:" with AuthNo for manual CC trans
                strSQL = " SELECT Posset.stationname, PT.TransID, PTp.PayTypeDesc, PT.TransDate, CASE TransType WHEN 1 THEN PTY.amount ELSE 0 END AS Sale, " +
                    " CASE TransType WHEN 2 THEN PTY.amount ELSE 0 END AS [Returns], CASE TransType WHEN 3 THEN PTY.amount ELSE 0 END AS ROA, PTY.RefNo, " + 
                    " CASE WHEN PTY.IsManual = 1 THEN 'M:' + PTY.AuthNo ELSE PTY.AuthNo END AS AuthNo, " +
                    " '" + this.dtpSaleStartDate.Text + "' as StartDate, '" + this.dtpSaleEndDate.Text + "' as EndDate FROM postransaction PT  " +
                    " LEFT JOIN Util_POSSet POSSet ON POSSet.StationID = PT.StationID " +
                    " INNER JOIN POSTransPayment PTY ON PT.TransID = PTY.TransID " + 
                    " INNER JOIN PayType PTp ON PTY.TransTypeCode = PTp.PayTypeID " +  
                    " WHERE CONVERT(DATETIME,PT.TransDate,109) BETWEEN CONVERT(DATETIME, CAST('" + this.dtpSaleStartDate.Text + " 00:00:00' AS DATETIME) ,113) AND CONVERT(DATETIME, CAST('" + this.dtpSaleEndDate.Text + " 23:59:59' AS DATETIME) ,113) " + strFilter + 
                    " ORDER BY PTp.PayTypeDesc, PT.TransDate";
            }
            catch
            {
                strSQL = string.Empty;
            }
        }

        private void PreviewReport(bool blnPrint, bool bCalledFromScheduler = false)   //PRIMEPOS-2485 02-Apr-2021 JY Added bCalledFromScheduler
        {
            String strQuery;
            try
            {
                this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
                rptSalePaType oRpt = new rptSalePaType();

                if (this.optSummary.Checked == true)
                {
                    oRpt.DetailSection1.SectionFormat.EnableSuppress = true;
                    #region Sprint-26 - PRIMEPOS-2404 10-Jul-2017 JY Added to hide Trans#, Trans Date, Station No, Reference, Auth.# in case of summary report
                    try
                    {
                        ((CrystalDecisions.CrystalReports.Engine.TextObject)oRpt.ReportDefinition.ReportObjects["Text10"]).Text = "";
                        ((CrystalDecisions.CrystalReports.Engine.TextObject)oRpt.ReportDefinition.ReportObjects["Text8"]).Text = "";
                        ((CrystalDecisions.CrystalReports.Engine.TextObject)oRpt.ReportDefinition.ReportObjects["Text11"]).Text = "";
                        ((CrystalDecisions.CrystalReports.Engine.TextObject)oRpt.ReportDefinition.ReportObjects["Text9"]).Text = "";
                        ((CrystalDecisions.CrystalReports.Engine.TextObject)oRpt.ReportDefinition.ReportObjects["Text5"]).Text = "";
                    }
                    catch (Exception Ex)
                    { }
                    #endregion
                }

                GenerateSQL(out strQuery);

                clsReports.Preview(blnPrint, strQuery, oRpt, bCalledFromScheduler); //PRIMEPOS-2485 02-Apr-2021 JY Added bCalledFromScheduler
                oReport = oRpt; //PRIMEPOS-2485 02-Apr-2021 JY Added
                this.Cursor = System.Windows.Forms.Cursors.Default;
            }
            catch (Exception exp)
            {
                this.Cursor = System.Windows.Forms.Cursors.Default;
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }

        private void grdPayType_BeforeRowExpanded(object sender, CancelableRowEventArgs e)
        {
            foreach (Infragistics.Win.UltraWinGrid.UltraGridRow orow in grdPayType.DisplayLayout.Rows)
            {
                if (orow.Expanded == true)
                    orow.CollapseAll();
            }
            e.Row.Activate();
        }

        private void grdPayType_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                Point point = System.Windows.Forms.Cursor.Position;
                point = this.grdPayType.PointToClient(point);
                Infragistics.Win.UIElement oUI = this.grdPayType.DisplayLayout.UIElement.ElementFromPoint(point);
                if (oUI == null)
                    return;

                while (oUI != null)
                {
                    if (oUI.GetType() == typeof(Infragistics.Win.UltraWinGrid.RowUIElement))
                    {
                        Infragistics.Win.UltraWinGrid.RowUIElement oRowUI = oUI as Infragistics.Win.UltraWinGrid.RowUIElement;
                        foreach (Infragistics.Win.UltraWinGrid.UltraGridRow orow in grdPayType.DisplayLayout.Rows)
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

        private void grdPayType_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            FormatGridColumn(grdPayType, 2);
        }

        private void grdPayType_MouseMove(object sender, MouseEventArgs e)
        {
            this.CurrentX = e.X;
            this.CurrentY = e.Y;
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
        #endregion
        
        #region PRIMEPOS-2596 18-Oct-2018 JY Added
        private void btnPayTypeList_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == System.Windows.Forms.Keys.Down)
            {
                dataGridList.Focus();
            }
        }

        private void btnPayTypeList_KeyUp(object sender, KeyEventArgs e)
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

        private void btnPayTypeList_Click(object sender, EventArgs e)
        {
            if (grpPayTypeList.Visible == false)
                PayTypeList_Expand(true);
            else
            {
                PayTypeList_Expand(false);
                GetSelectedPayTypes();
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
                }
            }
            else
            {
                if (grpPayTypeList.Visible == true)
                {
                    dataGridList.Visible = false;
                    grpPayTypeList.Visible = false;
                    GridVisibleFlag = false;
                }
            }
        }

        public void GetSelectedPayTypes()
        {
            string strPayTypeCode = "";
            string strPayTypeName = "";
            //isPrimeRxPaySelected = false;
            foreach (DataGridViewRow oRow in dataGridList.Rows)
            {
                if (POS_Core.Resources.Configuration.convertNullToBoolean(oRow.Cells[0].Value))
                {
                    strPayTypeCode += ",'" + oRow.Cells[1].Value.ToString() + "'";
                    strPayTypeName += "," + oRow.Cells["Description"].Value.ToString();
                    //if (oRow.Cells[1].Value.ToString().Trim() == "O" && oRow.Cells["Description"].Value.ToString().ToUpper() == "PRIMERXPAY") //PRIMEPOS-3282
                    //{
                    //    isPrimeRxPaySelected = true;
                    //}
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

        private void FillPayType()
        {
            DataSet PayTypeData = new DataSet();
            POS_Core.DataAccess.SearchSvr SearchSvr = new POS_Core.DataAccess.SearchSvr();
            PayTypeData = SearchSvr.Search(clsPOSDBConstants.PayType_tbl, "", "", 0, -1, 0, false, true); //PRIMEPOS-3282
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
        }

        private void chkSelectAll_CheckedChanged(object sender, EventArgs e)
        {
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
        #endregion

        private void dataGridList_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            for (int i = 0; i < dataGridList.Rows.Count; i++)
            {
                dataGridList.Rows[i].Cells[dataGridList.Columns["chkBox"].Index].Value = true;
            }
            chkSelectAll.Checked = true;
            GetSelectedPayTypes();
        }

        #region PRIMEPOS-2485 02-Apr-2021 JY Added
        public bool bSendPrint = true;
        private ReportClass oReport = new ReportClass();
        public usrDateRangeParams customControl;
        private const string ReportName = "SalesByPayType";

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
