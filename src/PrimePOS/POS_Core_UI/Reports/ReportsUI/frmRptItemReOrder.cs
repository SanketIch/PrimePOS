using System;
using System.Data.SqlClient;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using CrystalDecisions.Shared;
using CrystalDecisions.ReportSource;
using CrystalDecisions.CrystalReports.Engine;
using POS_Core_UI.Reports.Reports;
//using POS.UI;
using POS_Core.CommonData;
using POS_Core.CommonData.Tables;
using System.Data;
using System.Collections.Generic;
//using POS_Core.DataAccess;
using POS_Core.BusinessRules;
using POS_Core.CommonData.Rows;
using POS_Core.DataAccess;
using Infragistics.Win.UltraWinGrid;
using POS_Core.Resources;
using Resources;

namespace POS_Core_UI.Reports.ReportsUI
{
    /// <summary>
    /// Summary description for frmInventoryReports.
    /// </summary>
    public class frmRptItemReOrder : System.Windows.Forms.Form
    {
        private Infragistics.Win.UltraWinEditors.UltraNumericEditor numDays;
        private string SQLFill = "";
        private System.Windows.Forms.GroupBox ultraGroupBox2;
        private Infragistics.Win.UltraWinEditors.UltraOptionSet optByName;
        private System.Windows.Forms.GroupBox gbItemSold;
        private Infragistics.Win.UltraWinEditors.UltraNumericEditor numNumberOfDays;
        private System.Windows.Forms.GroupBox groupBox2;
        private Infragistics.Win.Misc.UltraButton btnPrint;
        private Infragistics.Win.Misc.UltraButton btnView;
        private Infragistics.Win.Misc.UltraButton btnClose;
        private Infragistics.Win.Misc.UltraLabel lblTransactionType;
        private Infragistics.Win.UltraWinSchedule.UltraCalendarCombo dtpFromDate;
        private Infragistics.Win.UltraWinEditors.UltraOptionSet optSearchSet;
        private GroupBox groupBox1;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor txtVendorCode;
        private Infragistics.Win.UltraWinSchedule.UltraCalendarCombo dtpToDate;
        private Infragistics.Win.Misc.UltraLabel ultraLabel2;
        private Infragistics.Win.Misc.UltraLabel ultraLabel1;

        private IContainer components;
        private string vendorName = string.Empty;
        private CheckBox chkForcedVendor;
        private bool isCanceled = false;
        private GroupBox grpAutoPO;
        private Button btnDown;
        private Button btnUp;
        private Button btnRToL;
        private Button btnLToR;
        private ListBox lstSequence;
        private ListBox lstParent;
        private Label lblMessage;
        private VScrollBar vScrollBar1;
        private VScrollBar vScrollBar2;

        //private PODetailData dsPoDetails;
        PODetailData dsPoDetails = new PODetailData();
        private GroupBox groupBox3;
        private Infragistics.Win.Misc.UltraLabel ultraLabel5;
        private Infragistics.Win.Misc.UltraLabel ultraLabel4;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor txtDepartment;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor txtSubDepartment;
        private dsSalesCompare dsSalesCompare1;
        private Infragistics.Win.UltraWinEditors.UltraOptionSet optionReport;

        //Added By SRT(Ritesh Parekh) Date: 27-Jul-2009
        private DateTime oldDate;
        private GroupBox gbItemReOrder;
        private Infragistics.Win.UltraWinEditors.UltraCheckEditor chckUseTransHistoryForReOrder;
        private Infragistics.Win.Misc.UltraLabel ultraLabel3;
        private Infragistics.Win.UltraWinEditors.UltraNumericEditor numNoOfDaysForItemReOrder;
        private Infragistics.Win.Misc.UltraLabel ultraLabel6;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor txtItemLocation;

        //End Of Added By SRT(Ritesh Parekh)
        //Added By Amit Date 16 Aug 2011
        private bool isFromPO = false;
        //End
        public PODetailData PODetailDataSet
        {
            get
            {
                return (dsPoDetails);
            }
        }

        public frmRptItemReOrder(bool isFromPO)
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();

            if (isFromPO)
            {    //Commented by Amit Date 6 July 2011
                //this.btnPrint.Visible=false;

                this.Text = this.lblTransactionType.Text = "Item Auto Order Option";
                //Commented by Amit Date 6 July 2011
                //this.btnView.Text="&Fill PO";
                this.btnPrint.Text = "&Fill PO";
                this.optionReport.Visible = false;
                //End
                //Added by Amit Date 16 Aug 2011
                this.isFromPO = true;
                //End
            }
            //
            // TODO: Add any constructor code after InitializeComponent call
            //
        }

        public frmRptItemReOrder(bool isFromPO, string vendor)
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();

            if (isFromPO)
            {    //following line Commented By Amit Date 6 july 2011
                //this.btnPrint.Visible = false;

                // Added by SRT(abhishek)
                //This will show the same vendor  in the Item Order Option
                this.txtVendorCode.Text = vendor;
                //Added by SRT 
                this.Text = this.lblTransactionType.Text = "Item Auto Order Option";
                //Commented By AMit Date 6 july 2011
                //this.btnView.Text = "&Fill PO";
                this.btnPrint.Text = "&Fill PO";
                this.optionReport.Visible = false;
                //Added by Amit Date 16 Aug 2011
                this.isFromPO = true;
                //End
            }
            //
            // TODO: Add any constructor code after InitializeComponent call
            //
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
            Infragistics.Win.ValueListItem valueListItem8 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem9 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem10 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinSchedule.CalendarCombo.DateButton dateButton1 = new Infragistics.Win.UltraWinSchedule.CalendarCombo.DateButton();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinSchedule.CalendarCombo.DateButton dateButton2 = new Infragistics.Win.UltraWinSchedule.CalendarCombo.DateButton();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.ValueListItem valueListItem11 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem12 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            Infragistics.Win.ValueListItem valueListItem13 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem14 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance7 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance8 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance9 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinEditors.EditorButton editorButton1 = new Infragistics.Win.UltraWinEditors.EditorButton();
            Infragistics.Win.Appearance appearance10 = new Infragistics.Win.Appearance();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmRptItemReOrder));
            Infragistics.Win.Appearance appearance11 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinEditors.EditorButton editorButton2 = new Infragistics.Win.UltraWinEditors.EditorButton();
            Infragistics.Win.Appearance appearance12 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinEditors.EditorButton editorButton4 = new Infragistics.Win.UltraWinEditors.EditorButton();
            Infragistics.Win.Appearance appearance14 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinEditors.EditorButton editorButton3 = new Infragistics.Win.UltraWinEditors.EditorButton();
            Infragistics.Win.Appearance appearance13 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance15 = new Infragistics.Win.Appearance();
            this.numDays = new Infragistics.Win.UltraWinEditors.UltraNumericEditor();
            this.ultraGroupBox2 = new System.Windows.Forms.GroupBox();
            this.optByName = new Infragistics.Win.UltraWinEditors.UltraOptionSet();
            this.gbItemSold = new System.Windows.Forms.GroupBox();
            this.vScrollBar2 = new System.Windows.Forms.VScrollBar();
            this.vScrollBar1 = new System.Windows.Forms.VScrollBar();
            this.dtpToDate = new Infragistics.Win.UltraWinSchedule.UltraCalendarCombo();
            this.ultraLabel2 = new Infragistics.Win.Misc.UltraLabel();
            this.dtpFromDate = new Infragistics.Win.UltraWinSchedule.UltraCalendarCombo();
            this.ultraLabel1 = new Infragistics.Win.Misc.UltraLabel();
            this.numNumberOfDays = new Infragistics.Win.UltraWinEditors.UltraNumericEditor();
            this.optSearchSet = new Infragistics.Win.UltraWinEditors.UltraOptionSet();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.optionReport = new Infragistics.Win.UltraWinEditors.UltraOptionSet();
            this.btnPrint = new Infragistics.Win.Misc.UltraButton();
            this.btnClose = new Infragistics.Win.Misc.UltraButton();
            this.btnView = new Infragistics.Win.Misc.UltraButton();
            this.lblTransactionType = new Infragistics.Win.Misc.UltraLabel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.grpAutoPO = new System.Windows.Forms.GroupBox();
            this.btnDown = new System.Windows.Forms.Button();
            this.btnUp = new System.Windows.Forms.Button();
            this.btnRToL = new System.Windows.Forms.Button();
            this.btnLToR = new System.Windows.Forms.Button();
            this.lstSequence = new System.Windows.Forms.ListBox();
            this.lstParent = new System.Windows.Forms.ListBox();
            this.chkForcedVendor = new System.Windows.Forms.CheckBox();
            this.txtVendorCode = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.lblMessage = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.txtItemLocation = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.ultraLabel6 = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabel5 = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabel4 = new Infragistics.Win.Misc.UltraLabel();
            this.txtDepartment = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.txtSubDepartment = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.gbItemReOrder = new System.Windows.Forms.GroupBox();
            this.ultraLabel3 = new Infragistics.Win.Misc.UltraLabel();
            this.numNoOfDaysForItemReOrder = new Infragistics.Win.UltraWinEditors.UltraNumericEditor();
            this.chckUseTransHistoryForReOrder = new Infragistics.Win.UltraWinEditors.UltraCheckEditor();
            this.dsSalesCompare1 = new dsSalesCompare();
            ((System.ComponentModel.ISupportInitialize)(this.numDays)).BeginInit();
            this.ultraGroupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.optByName)).BeginInit();
            this.gbItemSold.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dtpToDate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtpFromDate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numNumberOfDays)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.optSearchSet)).BeginInit();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.optionReport)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.grpAutoPO.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtVendorCode)).BeginInit();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtItemLocation)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDepartment)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSubDepartment)).BeginInit();
            this.gbItemReOrder.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numNoOfDaysForItemReOrder)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chckUseTransHistoryForReOrder)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dsSalesCompare1)).BeginInit();
            this.SuspendLayout();
            // 
            // numDays
            // 
            this.numDays.Location = new System.Drawing.Point(0, 0);
            this.numDays.Name = "numDays";
            this.numDays.Size = new System.Drawing.Size(100, 21);
            this.numDays.TabIndex = 0;
            // 
            // ultraGroupBox2
            // 
            this.ultraGroupBox2.Controls.Add(this.optByName);
            this.ultraGroupBox2.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultraGroupBox2.Location = new System.Drawing.Point(14, 287);
            this.ultraGroupBox2.Name = "ultraGroupBox2";
            this.ultraGroupBox2.Size = new System.Drawing.Size(478, 49);
            this.ultraGroupBox2.TabIndex = 2;
            this.ultraGroupBox2.TabStop = false;
            this.ultraGroupBox2.Text = "Item By";
            // 
            // optByName
            // 
            this.optByName.BorderStyle = Infragistics.Win.UIElementBorderStyle.None;
            this.optByName.CheckedIndex = 0;
            appearance1.FontData.BoldAsString = "False";
            this.optByName.ItemAppearance = appearance1;
            this.optByName.ItemOrigin = new System.Drawing.Point(0, 2);
            valueListItem8.CheckState = System.Windows.Forms.CheckState.Checked;
            valueListItem8.DataValue = 1;
            valueListItem8.DisplayText = "Item Sold";
            valueListItem9.DataValue = 2;
            valueListItem9.DisplayText = "Re-Order Level";
            valueListItem10.DataValue = 3;
            valueListItem10.DisplayText = "Zero/ Negative Quantity Items";
            this.optByName.Items.AddRange(new Infragistics.Win.ValueListItem[] {
            valueListItem8,
            valueListItem9,
            valueListItem10});
            this.optByName.ItemSpacingHorizontal = 50;
            this.optByName.Location = new System.Drawing.Point(19, 20);
            this.optByName.Name = "optByName";
            this.optByName.Size = new System.Drawing.Size(441, 20);
            this.optByName.TabIndex = 0;
            this.optByName.Text = "Item Sold";
            this.optByName.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.optByName.ValueChanged += new System.EventHandler(this.optByName_ValueChanged);
            // 
            // gbItemSold
            // 
            this.gbItemSold.Controls.Add(this.vScrollBar2);
            this.gbItemSold.Controls.Add(this.vScrollBar1);
            this.gbItemSold.Controls.Add(this.dtpToDate);
            this.gbItemSold.Controls.Add(this.ultraLabel2);
            this.gbItemSold.Controls.Add(this.dtpFromDate);
            this.gbItemSold.Controls.Add(this.ultraLabel1);
            this.gbItemSold.Controls.Add(this.numNumberOfDays);
            this.gbItemSold.Controls.Add(this.optSearchSet);
            this.gbItemSold.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbItemSold.Location = new System.Drawing.Point(14, 429);
            this.gbItemSold.Name = "gbItemSold";
            this.gbItemSold.Size = new System.Drawing.Size(478, 111);
            this.gbItemSold.TabIndex = 4;
            this.gbItemSold.TabStop = false;
            // 
            // vScrollBar2
            // 
            this.vScrollBar2.Location = new System.Drawing.Point(456, 74);
            this.vScrollBar2.Name = "vScrollBar2";
            this.vScrollBar2.Size = new System.Drawing.Size(12, 21);
            this.vScrollBar2.TabIndex = 3;
            this.vScrollBar2.Scroll += new System.Windows.Forms.ScrollEventHandler(this.vScrollBar2_Scroll);
            // 
            // vScrollBar1
            // 
            this.vScrollBar1.Location = new System.Drawing.Point(213, 74);
            this.vScrollBar1.Name = "vScrollBar1";
            this.vScrollBar1.Size = new System.Drawing.Size(12, 21);
            this.vScrollBar1.TabIndex = 6;
            this.vScrollBar1.Scroll += new System.Windows.Forms.ScrollEventHandler(this.vScrollBar1_Scroll);
            // 
            // dtpToDate
            // 
            this.dtpToDate.AllowNull = false;
            appearance2.FontData.BoldAsString = "False";
            appearance2.FontData.ItalicAsString = "False";
            appearance2.FontData.StrikeoutAsString = "False";
            appearance2.FontData.UnderlineAsString = "False";
            appearance2.ForeColor = System.Drawing.Color.Black;
            appearance2.ForeColorDisabled = System.Drawing.Color.Black;
            this.dtpToDate.Appearance = appearance2;
            this.dtpToDate.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Office2003ToolbarButton;
            this.dtpToDate.DateButtons.Add(dateButton1);
            this.dtpToDate.Format = "MM/dd/yyyy hh:mm tt";
            this.dtpToDate.Location = new System.Drawing.Point(302, 74);
            this.dtpToDate.Name = "dtpToDate";
            this.dtpToDate.NonAutoSizeHeight = 10;
            this.dtpToDate.Size = new System.Drawing.Size(153, 21);
            this.dtpToDate.TabIndex = 8;
            this.dtpToDate.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.dtpToDate.Value = new System.DateTime(2008, 10, 3, 0, 0, 0, 0);
            this.dtpToDate.ValueChanged += new System.EventHandler(this.dtpToDate_ValueChanged);
            this.dtpToDate.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dtpToDate_KeyDown);
            // 
            // ultraLabel2
            // 
            this.ultraLabel2.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultraLabel2.Location = new System.Drawing.Point(265, 76);
            this.ultraLabel2.Name = "ultraLabel2";
            this.ultraLabel2.Size = new System.Drawing.Size(43, 18);
            this.ultraLabel2.TabIndex = 7;
            this.ultraLabel2.Text = "To";
            // 
            // dtpFromDate
            // 
            this.dtpFromDate.AllowNull = false;
            appearance3.FontData.BoldAsString = "False";
            appearance3.FontData.ItalicAsString = "False";
            appearance3.FontData.StrikeoutAsString = "False";
            appearance3.FontData.UnderlineAsString = "False";
            appearance3.ForeColor = System.Drawing.Color.Black;
            appearance3.ForeColorDisabled = System.Drawing.Color.Black;
            this.dtpFromDate.Appearance = appearance3;
            this.dtpFromDate.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Office2003ToolbarButton;
            this.dtpFromDate.DateButtons.Add(dateButton2);
            this.dtpFromDate.Format = "MM/dd/yyyy hh:mm tt";
            this.dtpFromDate.Location = new System.Drawing.Point(61, 74);
            this.dtpFromDate.Name = "dtpFromDate";
            this.dtpFromDate.NonAutoSizeHeight = 10;
            this.dtpFromDate.Size = new System.Drawing.Size(153, 21);
            this.dtpFromDate.TabIndex = 5;
            this.dtpFromDate.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.dtpFromDate.Value = new System.DateTime(2008, 10, 3, 0, 0, 0, 0);
            this.dtpFromDate.ValueChanged += new System.EventHandler(this.dtpFromDate_ValueChanged);
            this.dtpFromDate.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dtpFromDate_KeyDown);
            // 
            // ultraLabel1
            // 
            this.ultraLabel1.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultraLabel1.Location = new System.Drawing.Point(19, 76);
            this.ultraLabel1.Name = "ultraLabel1";
            this.ultraLabel1.Size = new System.Drawing.Size(43, 18);
            this.ultraLabel1.TabIndex = 4;
            this.ultraLabel1.Text = "From";
            // 
            // numNumberOfDays
            // 
            this.numNumberOfDays.DisplayStyle = Infragistics.Win.EmbeddableElementDisplayStyle.OfficeXP;
            this.numNumberOfDays.Location = new System.Drawing.Point(321, 18);
            this.numNumberOfDays.MaskInput = "nnnn";
            this.numNumberOfDays.Name = "numNumberOfDays";
            this.numNumberOfDays.Size = new System.Drawing.Size(146, 22);
            this.numNumberOfDays.TabIndex = 1;
            this.numNumberOfDays.Value = 1;
            // 
            // optSearchSet
            // 
            this.optSearchSet.BorderStyle = Infragistics.Win.UIElementBorderStyle.None;
            this.optSearchSet.CheckedIndex = 1;
            appearance4.FontData.BoldAsString = "False";
            this.optSearchSet.ItemAppearance = appearance4;
            this.optSearchSet.ItemOrigin = new System.Drawing.Point(0, 2);
            valueListItem11.DataValue = 1;
            valueListItem11.DisplayText = "Enter No of Days to go back to look up  item sold";
            valueListItem12.DataValue = 2;
            valueListItem12.DisplayText = "Select Date/Time to go back and lookup item sold";
            this.optSearchSet.Items.AddRange(new Infragistics.Win.ValueListItem[] {
            valueListItem11,
            valueListItem12});
            this.optSearchSet.ItemSpacingVertical = 11;
            this.optSearchSet.Location = new System.Drawing.Point(8, 14);
            this.optSearchSet.Name = "optSearchSet";
            this.optSearchSet.Size = new System.Drawing.Size(304, 53);
            this.optSearchSet.TabIndex = 0;
            this.optSearchSet.Text = "Select Date/Time to go back and lookup item sold";
            this.optSearchSet.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.optSearchSet.ValueChanged += new System.EventHandler(this.optSearchSet_ValueChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.optionReport);
            this.groupBox2.Controls.Add(this.btnPrint);
            this.groupBox2.Controls.Add(this.btnClose);
            this.groupBox2.Controls.Add(this.btnView);
            this.groupBox2.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.groupBox2.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.Location = new System.Drawing.Point(14, 541);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(478, 57);
            this.groupBox2.TabIndex = 5;
            this.groupBox2.TabStop = false;
            // 
            // optionReport
            // 
            this.optionReport.BorderStyle = Infragistics.Win.UIElementBorderStyle.None;
            this.optionReport.CheckedIndex = 0;
            appearance5.FontData.BoldAsString = "False";
            this.optionReport.ItemAppearance = appearance5;
            this.optionReport.ItemOrigin = new System.Drawing.Point(0, 2);
            valueListItem13.DataValue = 1;
            valueListItem13.DisplayText = "Summary";
            valueListItem14.DataValue = 2;
            valueListItem14.DisplayText = "Detail";
            this.optionReport.Items.AddRange(new Infragistics.Win.ValueListItem[] {
            valueListItem13,
            valueListItem14});
            this.optionReport.ItemSpacingHorizontal = 10;
            this.optionReport.Location = new System.Drawing.Point(13, 23);
            this.optionReport.Name = "optionReport";
            this.optionReport.Size = new System.Drawing.Size(160, 20);
            this.optionReport.TabIndex = 1;
            this.optionReport.Text = "Summary";
            this.optionReport.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            // 
            // btnPrint
            // 
            appearance6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(62)))), ((int)(((byte)(176)))));
            appearance6.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance6.FontData.BoldAsString = "True";
            appearance6.ForeColor = System.Drawing.Color.White;
            this.btnPrint.Appearance = appearance6;
            this.btnPrint.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            this.btnPrint.Location = new System.Drawing.Point(280, 20);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(85, 26);
            this.btnPrint.TabIndex = 10;
            this.btnPrint.Text = "&Print";
            this.btnPrint.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            // 
            // btnClose
            // 
            appearance7.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(62)))), ((int)(((byte)(176)))));
            appearance7.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance7.FontData.BoldAsString = "True";
            appearance7.ForeColor = System.Drawing.Color.White;
            this.btnClose.Appearance = appearance7;
            this.btnClose.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(375, 20);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(85, 26);
            this.btnClose.TabIndex = 11;
            this.btnClose.Text = "&Close";
            this.btnClose.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnView
            // 
            appearance8.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(62)))), ((int)(((byte)(176)))));
            appearance8.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance8.FontData.BoldAsString = "True";
            appearance8.ForeColor = System.Drawing.Color.White;
            this.btnView.Appearance = appearance8;
            this.btnView.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            this.btnView.Location = new System.Drawing.Point(185, 20);
            this.btnView.Name = "btnView";
            this.btnView.Size = new System.Drawing.Size(85, 26);
            this.btnView.TabIndex = 9;
            this.btnView.Text = "&View";
            this.btnView.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnView.Visible = false;
            this.btnView.Click += new System.EventHandler(this.btnView_Click);
            // 
            // lblTransactionType
            // 
            appearance9.ForeColor = System.Drawing.Color.White;
            appearance9.ForeColorDisabled = System.Drawing.Color.White;
            appearance9.TextHAlignAsString = "Center";
            this.lblTransactionType.Appearance = appearance9;
            this.lblTransactionType.BackColorInternal = System.Drawing.Color.Transparent;
            this.lblTransactionType.Font = new System.Drawing.Font("Arial", 18F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTransactionType.Location = new System.Drawing.Point(62, 3);
            this.lblTransactionType.Name = "lblTransactionType";
            this.lblTransactionType.Size = new System.Drawing.Size(385, 30);
            this.lblTransactionType.TabIndex = 31;
            this.lblTransactionType.Text = "Item Re-Order Report";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.grpAutoPO);
            this.groupBox1.Controls.Add(this.chkForcedVendor);
            this.groupBox1.Controls.Add(this.txtVendorCode);
            this.groupBox1.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(14, 31);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(478, 168);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            // 
            // grpAutoPO
            // 
            this.grpAutoPO.Controls.Add(this.btnDown);
            this.grpAutoPO.Controls.Add(this.btnUp);
            this.grpAutoPO.Controls.Add(this.btnRToL);
            this.grpAutoPO.Controls.Add(this.btnLToR);
            this.grpAutoPO.Controls.Add(this.lstSequence);
            this.grpAutoPO.Controls.Add(this.lstParent);
            this.grpAutoPO.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpAutoPO.Location = new System.Drawing.Point(19, 44);
            this.grpAutoPO.Name = "grpAutoPO";
            this.grpAutoPO.Size = new System.Drawing.Size(441, 111);
            this.grpAutoPO.TabIndex = 0;
            this.grpAutoPO.TabStop = false;
            this.grpAutoPO.Text = "Vendor Selection Sequence for AutoPO Generation";
            // 
            // btnDown
            // 
            this.btnDown.Font = new System.Drawing.Font("Microsoft Sans Serif", 6F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDown.Image = global::POS_Core_UI.Properties.Resources.ig_tblPimgDn;
            this.btnDown.Location = new System.Drawing.Point(389, 63);
            this.btnDown.Name = "btnDown";
            this.btnDown.Size = new System.Drawing.Size(29, 37);
            this.btnDown.TabIndex = 5;
            this.btnDown.UseVisualStyleBackColor = true;
            this.btnDown.Click += new System.EventHandler(this.btnDown_Click);
            // 
            // btnUp
            // 
            this.btnUp.Font = new System.Drawing.Font("Microsoft Sans Serif", 6F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnUp.Image = global::POS_Core_UI.Properties.Resources.ig_tblPimgUp;
            this.btnUp.Location = new System.Drawing.Point(389, 21);
            this.btnUp.Name = "btnUp";
            this.btnUp.Size = new System.Drawing.Size(29, 36);
            this.btnUp.TabIndex = 4;
            this.btnUp.UseVisualStyleBackColor = true;
            this.btnUp.Click += new System.EventHandler(this.btnUp_Click);
            // 
            // btnRToL
            // 
            this.btnRToL.Image = global::POS_Core_UI.Properties.Resources.ig_tab_scrollXP0;
            this.btnRToL.Location = new System.Drawing.Point(181, 68);
            this.btnRToL.Name = "btnRToL";
            this.btnRToL.Size = new System.Drawing.Size(48, 21);
            this.btnRToL.TabIndex = 2;
            this.btnRToL.UseVisualStyleBackColor = true;
            this.btnRToL.Click += new System.EventHandler(this.btnRToL_Click);
            // 
            // btnLToR
            // 
            this.btnLToR.Image = global::POS_Core_UI.Properties.Resources.ig_tab_scrollXP1;
            this.btnLToR.Location = new System.Drawing.Point(181, 36);
            this.btnLToR.Name = "btnLToR";
            this.btnLToR.Size = new System.Drawing.Size(48, 21);
            this.btnLToR.TabIndex = 1;
            this.btnLToR.UseVisualStyleBackColor = true;
            this.btnLToR.Click += new System.EventHandler(this.btnLToR_Click);
            // 
            // lstSequence
            // 
            this.lstSequence.FormattingEnabled = true;
            this.lstSequence.ItemHeight = 15;
            this.lstSequence.Location = new System.Drawing.Point(251, 21);
            this.lstSequence.Name = "lstSequence";
            this.lstSequence.Size = new System.Drawing.Size(123, 79);
            this.lstSequence.TabIndex = 3;
            // 
            // lstParent
            // 
            this.lstParent.FormattingEnabled = true;
            this.lstParent.ItemHeight = 15;
            this.lstParent.Location = new System.Drawing.Point(24, 21);
            this.lstParent.Name = "lstParent";
            this.lstParent.Size = new System.Drawing.Size(134, 79);
            this.lstParent.TabIndex = 0;
            // 
            // chkForcedVendor
            // 
            this.chkForcedVendor.AutoSize = true;
            this.chkForcedVendor.Location = new System.Drawing.Point(19, 18);
            this.chkForcedVendor.Name = "chkForcedVendor";
            this.chkForcedVendor.Size = new System.Drawing.Size(121, 17);
            this.chkForcedVendor.TabIndex = 0;
            this.chkForcedVendor.Text = "Forced Vendor";
            this.chkForcedVendor.UseVisualStyleBackColor = true;
            this.chkForcedVendor.CheckedChanged += new System.EventHandler(this.chkDefaultVendor_CheckedChanged);
            // 
            // txtVendorCode
            // 
            this.txtVendorCode.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance10.Image = ((object)(resources.GetObject("appearance10.Image")));
            editorButton1.Appearance = appearance10;
            editorButton1.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Office2003ToolbarButton;
            appearance11.Image = ((object)(resources.GetObject("appearance11.Image")));
            editorButton1.PressedAppearance = appearance11;
            editorButton1.Text = "";
            this.txtVendorCode.ButtonsRight.Add(editorButton1);
            this.txtVendorCode.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtVendorCode.Location = new System.Drawing.Point(202, 18);
            this.txtVendorCode.MaxLength = 20;
            this.txtVendorCode.Name = "txtVendorCode";
            this.txtVendorCode.Size = new System.Drawing.Size(256, 20);
            this.txtVendorCode.TabIndex = 1;
            this.txtVendorCode.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.txtVendorCode.EditorButtonClick += new Infragistics.Win.UltraWinEditors.EditorButtonEventHandler(this.txtVendorCode_EditorButtonClick);
            this.txtVendorCode.Enter += new System.EventHandler(this.txtVendorCode_Enter);
            this.txtVendorCode.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtVendorCode_KeyPress);
            // 
            // lblMessage
            // 
            this.lblMessage.AutoSize = true;
            this.lblMessage.Location = new System.Drawing.Point(19, 597);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.Size = new System.Drawing.Size(0, 13);
            this.lblMessage.TabIndex = 32;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.txtItemLocation);
            this.groupBox3.Controls.Add(this.txtSubDepartment);
            this.groupBox3.Controls.Add(this.txtDepartment);
            this.groupBox3.Controls.Add(this.ultraLabel6);
            this.groupBox3.Controls.Add(this.ultraLabel5);
            this.groupBox3.Controls.Add(this.ultraLabel4);
            this.groupBox3.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox3.Location = new System.Drawing.Point(14, 200);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(478, 89);
            this.groupBox3.TabIndex = 1;
            this.groupBox3.TabStop = false;
            // 
            // txtItemLocation
            // 
            this.txtItemLocation.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance12.BackColor = System.Drawing.Color.White;
            appearance12.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(231)))), ((int)(((byte)(230)))));
            appearance12.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance12.BorderAlpha = Infragistics.Win.Alpha.Transparent;
            appearance12.Image = ((object)(resources.GetObject("appearance12.Image")));
            appearance12.ImageAlpha = Infragistics.Win.Alpha.Opaque;
            appearance12.ImageBackgroundAlpha = Infragistics.Win.Alpha.Opaque;
            appearance12.ImageBackgroundStyle = Infragistics.Win.ImageBackgroundStyle.Centered;
            editorButton2.Appearance = appearance12;
            editorButton2.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            editorButton2.Text = "";
            editorButton2.Width = 20;
            this.txtItemLocation.ButtonsRight.Add(editorButton2);
            this.txtItemLocation.Location = new System.Drawing.Point(306, 61);
            this.txtItemLocation.Name = "txtItemLocation";
            this.txtItemLocation.ReadOnly = true;
            this.txtItemLocation.Size = new System.Drawing.Size(152, 23);
            this.txtItemLocation.TabIndex = 5;
            this.txtItemLocation.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.txtItemLocation.EditorButtonClick += new Infragistics.Win.UltraWinEditors.EditorButtonEventHandler(this.txtItemLocation_EditorButtonClick);
            // 
            // ultraLabel6
            // 
            this.ultraLabel6.AutoSize = true;
            this.ultraLabel6.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultraLabel6.Location = new System.Drawing.Point(18, 65);
            this.ultraLabel6.Name = "ultraLabel6";
            this.ultraLabel6.Size = new System.Drawing.Size(243, 15);
            this.ultraLabel6.TabIndex = 4;
            this.ultraLabel6.Text = "Item Location <Blank = Ignore Location>";
            // 
            // ultraLabel5
            // 
            this.ultraLabel5.AutoSize = true;
            this.ultraLabel5.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultraLabel5.Location = new System.Drawing.Point(18, 38);
            this.ultraLabel5.Name = "ultraLabel5";
            this.ultraLabel5.Size = new System.Drawing.Size(268, 15);
            this.ultraLabel5.TabIndex = 2;
            this.ultraLabel5.Text = "Sub Department <Blank = Ignore Sub  Dept>";
            // 
            // ultraLabel4
            // 
            this.ultraLabel4.AutoSize = true;
            this.ultraLabel4.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultraLabel4.Location = new System.Drawing.Point(18, 16);
            this.ultraLabel4.Name = "ultraLabel4";
            this.ultraLabel4.Size = new System.Drawing.Size(196, 15);
            this.ultraLabel4.TabIndex = 0;
            this.ultraLabel4.Text = "Department <Blank = Any Dept>";
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
            editorButton4.Appearance = appearance14;
            editorButton4.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            editorButton4.Text = "";
            editorButton4.Width = 20;
            this.txtDepartment.ButtonsRight.Add(editorButton4);
            this.txtDepartment.Location = new System.Drawing.Point(306, 12);
            this.txtDepartment.Name = "txtDepartment";
            this.txtDepartment.ReadOnly = true;
            this.txtDepartment.Size = new System.Drawing.Size(152, 23);
            this.txtDepartment.TabIndex = 1;
            this.txtDepartment.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.txtDepartment.EditorButtonClick += new Infragistics.Win.UltraWinEditors.EditorButtonEventHandler(this.txtDepartment_EditorButtonClick);
            // 
            // txtSubDepartment
            // 
            this.txtSubDepartment.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance13.BackColor = System.Drawing.Color.White;
            appearance13.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(231)))), ((int)(((byte)(230)))));
            appearance13.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance13.BorderAlpha = Infragistics.Win.Alpha.Transparent;
            appearance13.Image = ((object)(resources.GetObject("appearance13.Image")));
            appearance13.ImageAlpha = Infragistics.Win.Alpha.Opaque;
            appearance13.ImageBackgroundAlpha = Infragistics.Win.Alpha.Opaque;
            appearance13.ImageBackgroundStyle = Infragistics.Win.ImageBackgroundStyle.Centered;
            editorButton3.Appearance = appearance13;
            editorButton3.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            editorButton3.Text = "";
            editorButton3.Width = 20;
            this.txtSubDepartment.ButtonsRight.Add(editorButton3);
            this.txtSubDepartment.Location = new System.Drawing.Point(306, 35);
            this.txtSubDepartment.Name = "txtSubDepartment";
            this.txtSubDepartment.ReadOnly = true;
            this.txtSubDepartment.Size = new System.Drawing.Size(152, 23);
            this.txtSubDepartment.TabIndex = 3;
            this.txtSubDepartment.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.txtSubDepartment.EditorButtonClick += new Infragistics.Win.UltraWinEditors.EditorButtonEventHandler(this.txtSubDepartment_EditorButtonClick);
            // 
            // gbItemReOrder
            // 
            this.gbItemReOrder.Controls.Add(this.ultraLabel3);
            this.gbItemReOrder.Controls.Add(this.numNoOfDaysForItemReOrder);
            this.gbItemReOrder.Controls.Add(this.chckUseTransHistoryForReOrder);
            this.gbItemReOrder.Enabled = false;
            this.gbItemReOrder.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.gbItemReOrder.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbItemReOrder.Location = new System.Drawing.Point(14, 342);
            this.gbItemReOrder.Name = "gbItemReOrder";
            this.gbItemReOrder.Size = new System.Drawing.Size(478, 83);
            this.gbItemReOrder.TabIndex = 3;
            this.gbItemReOrder.TabStop = false;
            // 
            // ultraLabel3
            // 
            this.ultraLabel3.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultraLabel3.Location = new System.Drawing.Point(13, 55);
            this.ultraLabel3.Name = "ultraLabel3";
            this.ultraLabel3.Size = new System.Drawing.Size(302, 13);
            this.ultraLabel3.TabIndex = 2;
            this.ultraLabel3.Text = "Enter No of Days to go back to look into transaction ";
            // 
            // numNoOfDaysForItemReOrder
            // 
            this.numNoOfDaysForItemReOrder.DisplayStyle = Infragistics.Win.EmbeddableElementDisplayStyle.OfficeXP;
            this.numNoOfDaysForItemReOrder.Location = new System.Drawing.Point(321, 51);
            this.numNoOfDaysForItemReOrder.MaskInput = "nnnn";
            this.numNoOfDaysForItemReOrder.Name = "numNoOfDaysForItemReOrder";
            this.numNoOfDaysForItemReOrder.Size = new System.Drawing.Size(146, 22);
            this.numNoOfDaysForItemReOrder.TabIndex = 1;
            this.numNoOfDaysForItemReOrder.Value = 60;
            // 
            // chckUseTransHistoryForReOrder
            // 
            appearance15.FontData.BoldAsString = "False";
            appearance15.ForeColor = System.Drawing.Color.Black;
            appearance15.ForeColorDisabled = System.Drawing.Color.White;
            this.chckUseTransHistoryForReOrder.Appearance = appearance15;
            this.chckUseTransHistoryForReOrder.CheckAlign = System.Drawing.ContentAlignment.TopLeft;
            this.chckUseTransHistoryForReOrder.Checked = true;
            this.chckUseTransHistoryForReOrder.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chckUseTransHistoryForReOrder.Location = new System.Drawing.Point(13, 19);
            this.chckUseTransHistoryForReOrder.Name = "chckUseTransHistoryForReOrder";
            this.chckUseTransHistoryForReOrder.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.chckUseTransHistoryForReOrder.Size = new System.Drawing.Size(448, 19);
            this.chckUseTransHistoryForReOrder.TabIndex = 0;
            this.chckUseTransHistoryForReOrder.Text = "Look Into Transaction History";
            this.chckUseTransHistoryForReOrder.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.chckUseTransHistoryForReOrder.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.chckUseTransHistoryForReOrder.CheckedChanged += new System.EventHandler(this.chckUseTransHistoryForReOrder_CheckedChanged);
            // 
            // dsSalesCompare1
            // 
            this.dsSalesCompare1.DataSetName = "dsSalesCompare";
            this.dsSalesCompare1.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // frmRptItemReOrder
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(507, 602);
            this.Controls.Add(this.gbItemReOrder);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.lblMessage);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.lblTransactionType);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.gbItemSold);
            this.Controls.Add(this.ultraGroupBox2);
            this.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmRptItemReOrder";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Item Re-Order Report";
            this.Load += new System.EventHandler(this.frmInventoryReports_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmRptItemReOrder_KeyDown);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.frmRptItemReOrder_KeyUp);
            ((System.ComponentModel.ISupportInitialize)(this.numDays)).EndInit();
            this.ultraGroupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.optByName)).EndInit();
            this.gbItemSold.ResumeLayout(false);
            this.gbItemSold.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dtpToDate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtpFromDate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numNumberOfDays)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.optSearchSet)).EndInit();
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.optionReport)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.grpAutoPO.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.txtVendorCode)).EndInit();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtItemLocation)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDepartment)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSubDepartment)).EndInit();
            this.gbItemReOrder.ResumeLayout(false);
            this.gbItemReOrder.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numNoOfDaysForItemReOrder)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chckUseTransHistoryForReOrder)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dsSalesCompare1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        private void frmInventoryReports_Load(object sender, System.EventArgs e)
        {
            clsUIHelper.setColorSchecme(this);
            LoadAutoPOSettings();
            this.numNumberOfDays.AfterEnterEditMode += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
            this.numNumberOfDays.AfterExitEditMode += new System.EventHandler(clsUIHelper.AfterExitEditMode);
            //Start Added By Amit Date 1 July 2011
            this.txtDepartment.AfterEnterEditMode += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
            this.txtDepartment.AfterExitEditMode += new System.EventHandler(clsUIHelper.AfterExitEditMode);
            this.txtSubDepartment.AfterEnterEditMode += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
            this.txtSubDepartment.AfterExitEditMode += new System.EventHandler(clsUIHelper.AfterExitEditMode);
            this.numNumberOfDays.Enabled = false;
            //End
            DateTime datetime = DateTime.Now.Subtract(new TimeSpan(1, 0, 0, 0));
            //this.dtpToDate.Value = new DateTime(datetime.Year, datetime.Month, datetime.Day+1, 23, 59, 59);       //PRIMEPOS-2487 06-Feb-2018 JY Commented
            this.dtpToDate.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 23, 59, 59);   //PRIMEPOS-2487 06-Feb-2018 JY Added
            this.dtpFromDate.Value = new DateTime(datetime.Year, datetime.Month, datetime.Day, 01, 0, 0);
            txtDepartment.Tag = "";
            txtSubDepartment.Tag = "";
            chkForcedVendor.Checked = true;
        }

        private void btnPrint_Click(object sender, System.EventArgs e)
        {
            //Commented By Amit Date 29 june 2011
            //Preview(true);
            //Added by Amit Date 29 june 2011

            if (txtVendorCode.Text == "" && txtVendorCode.Enabled == true)
            {
                clsUIHelper.ShowErrorMsg("Please Select vendor");
                txtVendorCode.Focus();
                return;
            }

            string fieldName = string.Empty;
            Cursor.Current = Cursors.WaitCursor;
            try
            {
                if (!chkForcedVendor.Checked && !ValidateAutoPOVendors())
                {
                    clsUIHelper.ShowErrorMsg("Please select AutoPO Mode.");
                    return;
                }
                if (!validateFields(out fieldName))
                {
                    if (fieldName == "NO-OF-DAYS")
                    {
                        clsUIHelper.ShowErrorMsg("Please Enter no of days greater than zero.");
                    }
                    if (fieldName == "DATE")
                    {
                        clsUIHelper.ShowErrorMsg("From Date can not be greater than To Date.");
                    }
                    return;
                }
                this.GetVendor = this.txtVendorCode.Text;
                this.IsCanceled = false;

                if (this.btnPrint.Text == "&Print")
                {
                    this.numNumberOfDays.Focus();
                    Preview();
                }
                else
                {
                    //AutoPO(); //PRIMEPOS-3155 12-Oct-2022 JY Commented
                    AutoPONew();    //PRIMEPOS-3155 12-Oct-2022 JY Added
                    this.Close();
                }
            }
            catch (Exception ex)
            {
            }
            finally
            {
                Cursor.Current = Cursors.WaitCursor;
            }
        }

        //Modified By Amit Date 29 June 2011
        private void Preview()
        {
            try
            {
                string sSQL = "";
                //added by Amit Date 28 July 2011
                itemreorder oRpt = new itemreorder();
                itemreorderDetail oRptDetail = new itemreorderDetail();

                //CrystalDecisions.CrystalReports.Engine.TextObject txt = (CrystalDecisions.CrystalReports.Engine.TextObject)oRpt.ReportDefinition.ReportObjects["txtQuantityStock100Days"];
                CrystalDecisions.CrystalReports.Engine.TextObject txtQtySoldSince;

                if (optionReport.CheckedIndex == 0)
                {
                    txtQtySoldSince = (CrystalDecisions.CrystalReports.Engine.TextObject)oRpt.ReportDefinition.ReportObjects["txtQtySoldSince"];
                }
                else
                {
                    txtQtySoldSince = (CrystalDecisions.CrystalReports.Engine.TextObject)oRptDetail.ReportDefinition.ReportObjects["txtQtySoldSince"];
                }

                //AutoPO(); //PRIMEPOS-3155 12-Oct-2022 JY Commented
                AutoPONew();    //PRIMEPOS-3155 12-Oct-2022 JY Added

                //Start : Comment removed by Amit Date 04 April 2011  
                if (optByName.CheckedIndex == 0)
                {
                    if (this.optSearchSet.CheckedIndex == 0)
                    {
                        //txt.Text = " QTY Sold In Last " + numNumberOfDays.Value.ToString() + " Days";
                        txtQtySoldSince.Text = "Last " + numNumberOfDays.Value.ToString() + " Days";
                    }
                    else
                    {
                        //txt.Text = " QTY Sold Since " + Convert.ToDateTime(dtpFromDate.Value.ToString()).ToString("MM/dd/yyyy") + " ";
                        txtQtySoldSince.Text = Convert.ToDateTime(dtpFromDate.Value.ToString()).ToString("d") + " To " + Convert.ToDateTime(dtpToDate.Value.ToString()).ToString("d");
                    }
                }
                else
                {
                    //txtQtySoldSince.Text ="Last "+ clsPOSDBConstants.ItemReorderPeriod.ToString() + " Days";     
                    txtQtySoldSince.Text = "Last " + this.numNoOfDaysForItemReOrder.Value.ToString() + " Days";
                }

                //End

                #region Commented
                //if (optByName.CheckedIndex == 0)
                //{
                //    sSQL = " SELECT " +
                //        " itm.ItemID " +
                //        " , itm.Description " +
                //        " , itm.QtyInStock " +
                //        " , itm.ReOrderLevel " +
                //        " , Sum(Qty) QtySold100Days " +
                //        " FROM " +
                //        " Item itm " +
                //        " left outer join itemVendor on (itemVendor.ItemID=Itm.ItemID) " +
                //        " inner join vendor on (vendor.vendorid=itemvendor.vendorid) " +
                //        " , POSTransaction  Trans " +
                //        " , POSTransactionDetail TransDetail " +
                //        " WHERE " +
                //        " Trans.TransID = TransDetail.TransID " +
                //        " AND itm.ItemID = TransDetail.ItemID " +
                //        " AND isnull(itm.ExclFromAutoPO,0) = 0 ";

                //    if (optSearchSet.CheckedIndex == 0)
                //    {
                //        sSQL += " AND Trans.TransDate >= DateAdd(D,-" + numNumberOfDays.Value + ",getdate()) ";
                //    }
                //    else
                //    {
                //        sSQL += " AND Trans.TransDate between cast('" + DateTime.Parse(dtpFromDate.Value.ToString()).ToString() + "' as datetime) and cast('" + DateTime.Parse(dtpToDate.Value.ToString()).ToString() + "' as datetime) ";
                //    }

                //    if (this.txtVendorCode.Text.Trim() != "")
                //    {
                //        sSQL += " AND Vendor.VendorCode='" + txtVendorCode.Text + "'";
                //    }

                //    sSQL += " Group By " +
                //        " itm.ItemID " +
                //        " , itm.Description " +
                //        " , itm.QtyInStock " +
                //        " , itm.ReOrderLevel ";
                //}
                //else
                //{
                //    sSQL = " SELECT " +
                //        " itm.ItemID " +
                //        " , itm.Description " +
                //        " , itm.QtyInStock " +
                //        " , itm.ReOrderLevel " +
                //        " FROM " +
                //        " Item itm " +
                //        " left outer join itemVendor on (itemVendor.ItemID=Itm.ItemID) " +
                //        " inner join vendor on (vendor.vendorid=itemvendor.vendorid) " +
                //        " WHERE " +
                //        " itm.QtyInStock < itm.ReOrderLevel " +
                //        " and isnull(itm.ExclFromAutoPO,0) = 0";

                //    if (this.txtVendorCode.Text.Trim() != "")
                //    {
                //        sSQL += " AND Vendor.VendorCode='" + txtVendorCode.Text + "'";
                //    }
                //}
                #endregion

                dsPoDetails.Tables[clsPOSDBConstants.PODetail_tbl].Columns.Add("BarcodeImg", System.Type.GetType("System.Byte[]"));

                int i = 0;
                string ItemID = null;
                string BarCode = null;
                string strBarCodeOf = null;

                frmPOPrintOptions POPrintoptions = new frmPOPrintOptions();
                DialogResult Result = POPrintoptions.ShowDialog();

                strBarCodeOf = frmPOPrintOptions.strBarCodeOf;

                foreach (DataRow dr in dsPoDetails.Tables[clsPOSDBConstants.PODetail_tbl].Rows)
                {

                    if (strBarCodeOf == "VendItmCode")
                    {
                        ItemID = dr[clsPOSDBConstants.PODetail_Fld_VendorItemCode].ToString();
                        BarCode = "Vendor Item Code";
                    }
                    else if (strBarCodeOf == "ItemID")
                    {
                        ItemID = dr[clsPOSDBConstants.PODetail_Fld_ItemID].ToString();
                        BarCode = "Item ID";
                    }
                    else
                    {
                        ItemID = "";
                        BarCode = "None";
                    }

                    try
                    {
                        if (ItemID != "")
                            Configuration.PrintBarcode(ItemID, 0, 0, 20, 200, "CODE128", "H", System.IO.Path.GetTempPath() + "\\" + ItemID + ".bmp");
                    }
                    catch (Exception ex)
                    {
                    }
                    if (ItemID != "")
                        dsPoDetails.Tables[clsPOSDBConstants.PODetail_tbl].Rows[i]["BarcodeImg"] = Configuration.GetImageData(System.IO.Path.GetTempPath() + "\\" + ItemID + ".bmp");
                    else
                        dsPoDetails.Tables[clsPOSDBConstants.PODetail_tbl].Rows[i]["BarcodeImg"] = null;
                    i++;
                }

                if (Result == DialogResult.OK)
                {
                    #region Code For Print Preview PO
                    if (optionReport.CheckedIndex == 0)
                    {
                        clsReports.setCRTextObjectText("BarCode", BarCode, oRpt);
                        clsReports.Preview(false, dsPoDetails, oRpt);
                    }
                    else
                    {
                        clsReports.setCRTextObjectText("BarCode", BarCode, oRptDetail);
                        clsReports.Preview(false, dsPoDetails, oRptDetail);
                    }
                    #endregion
                }
                else if (Result == DialogResult.Yes)
                {
                    #region Code For Print PO
                    if (optionReport.CheckedIndex == 0)
                    {
                        clsReports.Preview(true, dsPoDetails, oRpt);
                    }
                    else
                    {
                        clsReports.Preview(true, dsPoDetails, oRptDetail);
                    }
                    #endregion
                }

                //clsReports.Preview(PrintIt, dsPoDetails, oRpt);
                //clsReports.Preview(PrintIt, sSQL, oRpt);
            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }

        public string GetVendor
        {
            set { vendorName = value; }
            get { return vendorName; }
        }

        public bool IsCanceled
        {
            set { isCanceled = value; }
            get { return isCanceled; }
        }

        private void btnClose_Click(object sender, System.EventArgs e)
        {
            IsCanceled = true;
            this.Close();
        }

        private void numNumberOfDays_ValueChanged(object sender, System.EventArgs e)
        {

        }

        //Folling bool zeroFlag added by Krishna on 18April 2011
        bool zeroFalg = false;
        private void optByName_ValueChanged(object sender, System.EventArgs e)
        {
            gbItemSold.Enabled = (optByName.CheckedIndex == 0);
            gbItemReOrder.Enabled = (optByName.CheckedIndex == 1 || optByName.CheckedIndex == 2);
        }

        private void frmRptItemReOrder_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            try
            {
                if (e.KeyData == System.Windows.Forms.Keys.Enter && this.txtVendorCode.Focused == false)
                {
                    this.SelectNextControl(this.ActiveControl, true, true, true, true);
                }
                else if (e.KeyData == System.Windows.Forms.Keys.F4)
                {
                    //frmSearch oSearch = null;                    
                    string VendorID = CheckIfExactVendorExists(txtVendorCode.Text.Trim());
                    if (VendorID == "")
                    {
                        //oSearch = new frmSearch(clsPOSDBConstants.Vendor_tbl, this.txtVendorCode.Text, "");                        
                        //if (vendorcode != string.Empty)
                        //  oSearch.VendorTextFromAutoPO = vendorcode;
                        using (frmSearchMain oSearch = new frmSearchMain(clsPOSDBConstants.Vendor_tbl, this.txtVendorCode.Text, "", true))  //20-Dec-2017 JY Added new reference
                        {
                            oSearch.ShowDialog();
                            if (oSearch.IsCanceled) return;
                            txtVendorCode.Text = oSearch.SelectedRowID();
                        }
                    }
                    else
                    {
                        txtVendorCode.Tag = VendorID;
                    }
                    this.SelectNextControl(this.ActiveControl, true, true, true, true);
                }
            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }

        private void ultraGroupBox2_Enter(object sender, System.EventArgs e)
        {
            optByName.Focus();
        }

        private void gbItemReOrder_Enter(object sender, System.EventArgs e)
        {
            numNumberOfDays.Focus();
        }

        //Following function Added by Krishna on 18April 2011

        public void rptFill()
        {
            // gbItemReOrder.Enabled = true;
            itemreorder objItemReorder = new itemreorder();
            PODetailData dsPoDetails = new PODetailData();
            PODetailRow podRow = null;

            ItemData oitemdata = new ItemData();
            Item oitem = new Item();
            ItemSvr oItemSvr = new ItemSvr();

            ItemVendorSvr oItemVendorSvr = new ItemVendorSvr();
            VendorSvr oVendor = new VendorSvr();
            VendorData Vd = null;
            VendorRow VendRow = null;

            ItemVendorData oItemVendorData = null;
            List<string> lstData = new List<string>();

            Vd = oVendor.Populate(txtVendorCode.Text);
            VendRow = (VendorRow)Vd.Vendor.Rows[0];
            int VendId = VendRow.VendorId;

            SqlConnection conn = new SqlConnection(Configuration.m_ConnString);
            SqlDataAdapter Da = new SqlDataAdapter("select Item.ItemId from Item,ItemVendor where Item.ItemId=ItemVendor.ItemID AND VendorId='" + VendId + "'AND Item.[QtyInStock]<=0", conn);
            SqlCommandBuilder cbd = new SqlCommandBuilder(Da);
            DataSet Ds = new DataSet();
            Da.Fill(Ds);

            foreach (DataRow Dr in Ds.Tables[0].Rows)
            {
                lstData.Add("'" + Dr[0].ToString() + "'");
                Application.DoEvents();
            }
            string SelectClause;
            try
            {
                if (lstData.Count > 0)
                {
                    oitemdata = oItemSvr.PopulateList(" where ItemId in (" + string.Join(",", lstData.ToArray()) + ") ");
                    oItemVendorData = oItemVendorSvr.PopulateList(" where ItemId in (" + string.Join(",", lstData.ToArray()) + ")");

                    if (oitemdata == null || oitemdata.Tables.Count == 0 || oitemdata.Tables[0].Rows.Count == 0)
                    {
                        throw (new Exception("There are no more items matching transactions of selected period."));
                    }

                    if (oItemVendorData == null || oItemVendorData.Tables.Count == 0 || oItemVendorData.Tables[0].Rows.Count == 0)
                    {
                        throw (new Exception("There are no more vendor items matching transactions of selected period."));
                    }
                }
                else
                {
                    throw (new Exception("There are no more vendor items matching transactions of selected period."));
                }
                int cnt = 0;
                foreach (ItemRow oItemRow in oitemdata.Item.Rows)
                {
                    cnt++;
                    lblMessage.Text = "Scanning Item #" + oItemRow.ItemID.Trim() + " For Last Vendor";
                    Application.DoEvents();

                    podRow = dsPoDetails.PODetail.NewPODetailRow();
                    podRow.ItemID = oItemRow.ItemID;
                    podRow.ItemDescription = oItemRow.Description;
                    podRow.QTY = oItemRow.ReOrderLevel;
                    podRow.QtyInStock = oItemRow.QtyInStock;
                    podRow.ReOrderLevel = oItemRow.ReOrderLevel;
                    podRow.PacketSize = oItemRow.PckSize;
                    podRow.PacketQuant = oItemRow.PckQty;
                    podRow.Packetunit = oItemRow.PckUnit;
                    dsPoDetails.PODetail.AddRow(podRow);

                    //podRow.Price = oItemVendorRow.VenorCostPrice;
                    //podRow.VendorID = oItemVendorRow.VendorID;
                    podRow.VendorItemCode = "2424";
                    //podRow.VendorName = oItemVendorRow.VendorCode;

                }
                zeroFalg = false;
            }
            catch (Exception ex)
            {
                clsUIHelper.ShowErrorMsg(ex.Message);
            }
            CrystalDecisions.CrystalReports.Engine.TextObject txt = (CrystalDecisions.CrystalReports.Engine.TextObject)objItemReorder.ReportDefinition.ReportObjects["txtQuantityStock100Days"];
            if (this.optSearchSet.CheckedIndex == 0)
                txt.Text = " QTY Sold In Last " + numNumberOfDays.Value.ToString() + " Days";
            else
                txt.Text = " QTY Sold Since " + Convert.ToDateTime(dtpFromDate.Value.ToString()).ToString("MM/dd/yyyy hh:mm tt") + " ";


            clsReports.Preview(false, dsPoDetails, objItemReorder);
        }

        //Till here Added by Krishna

        private void btnView_Click(object sender, System.EventArgs e)
        {
            //Following Code Added by Krishna on 19 April 2011
            if (txtVendorCode.Text == "" && txtVendorCode.Enabled == true)
            {
                clsUIHelper.ShowErrorMsg("Please Select vendor");
                txtVendorCode.Focus();
                return;
            }

            if (optByName.CheckedIndex == 2)
                zeroFalg = true;
            else zeroFalg = false;

            if (!chkForcedVendor.Checked && lstSequence.Items.Contains(clsPOSDBConstants.IGNOREVENDOR) && zeroFalg == false)
            {
                this.numNumberOfDays.Focus();
                Preview();
                return;
            }
            else if (zeroFalg == true && chkForcedVendor.Checked == false)
            {
                CreateAutoPOByIgnoreVendorQtyZero();
                return;

            }
            ///Till here by Krishna            

            string fieldName = string.Empty;
            Cursor.Current = Cursors.WaitCursor;
            try
            {
                if (!ValidateAutoPOVendors() && !chkForcedVendor.Checked)
                {
                    clsUIHelper.ShowErrorMsg("Please select AutoPO Mode.");
                    return;
                }
                if (!validateFields(out fieldName))
                {
                    if (fieldName == "NO-OF-DAYS")
                    {
                        clsUIHelper.ShowErrorMsg("Please Enter no of days greater than zero.");
                    }
                    if (fieldName == "DATE")
                    {
                        clsUIHelper.ShowErrorMsg("From Date can not be greater than To Date.");
                    }
                    return;
                }
                this.GetVendor = "";

                this.GetVendor = this.txtVendorCode.Text;
                this.IsCanceled = false;

                if (this.btnView.Text == "&View")
                {
                    //Following Added by Krishna on 19April 2011
                    if (zeroFalg)
                    {
                        rptFill();
                        return;
                    }
                    //till here by krishna

                    this.numNumberOfDays.Focus();
                    Preview();
                }
                else
                {
                    //AutoPO(); //PRIMEPOS-3155 12-Oct-2022 JY Commented
                    AutoPONew();    //PRIMEPOS-3155 12-Oct-2022 JY Added
                    this.Close();
                }
            }
            catch (Exception ex)
            {
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        private bool validateFields(out string fieldName)
        {
            bool isValid = true;
            string field = string.Empty;
            try
            {
                if (optByName.CheckedIndex == 0)
                {
                    if ((DateTime)dtpFromDate.Value > (DateTime)dtpToDate.Value)
                    {
                        isValid = false;
                        fieldName = "DATE";
                        return isValid;
                    }
                    if ((int)this.numNumberOfDays.Value <= 0)
                    {
                        isValid = false;
                        fieldName = "NO-OF-DAYS";
                        return isValid;
                    }
                }
            }
            catch (Exception ex)
            {
            }
            fieldName = field;
            return isValid;
        }

        private void AutoPOForcedVendor()
        {
            try
            {
                dsPoDetails = new PODetailData();
                if (optByName.CheckedIndex == 0)
                {
                    SQLFill = " SELECT " +
                        " itm.ItemID " +
                        " , itm.Description " +
                        " , Vendor.VendorCode " +
                        " , itm.QtyInStock " +
                        " , itm.ReOrderLevel " +
                        " , Sum(Qty) QtySold100Days " +
                        " , Sum(Qty) as newOrder " +
                        " FROM " +
                        " Item itm " +
                        " left outer join itemVendor on (itemVendor.ItemID=Itm.ItemID) " +
                        " inner join vendor on (vendor.vendorid=itemvendor.vendorid) " +
                        " , POSTransaction  Trans " +
                        " , POSTransactionDetail TransDetail " +
                        " WHERE " +
                        " Trans.TransID = TransDetail.TransID " +
                        " AND itm.ItemID = TransDetail.ItemID " +
                        " AND isnull(itm.ExclFromAutoPO,0) = 0 ";

                    if (optSearchSet.CheckedIndex == 0)
                    {
                        SQLFill += " AND Trans.TransDate >= DateAdd(D,-" + numNumberOfDays.Value + ",getdate()) ";
                    }
                    else
                    {
                        SQLFill += " AND Trans.TransDate >= cast('" + DateTime.Parse(dtpFromDate.Value.ToString()).ToString() + "' as datetime) ";
                    }

                    SQLFill += " AND Vendor.VendorCode='" + txtVendorCode.Text.Replace("'", "''") + "'";


                    SQLFill += " Group By " +
                        " itm.ItemID " +
                        " , itm.Description " +
                        " , Vendor.VendorCode " +
                        " , itm.QtyInStock " +
                        " , itm.ReOrderLevel ";
                }
                else
                {
                    SQLFill = " SELECT " +
                        " itm.ItemID " +
                        " , itm.Description " +
                        " , itm.QtyInStock " +
                        " , itm.ReOrderLevel " +
                        " , itm.ReOrderLevel as newOrder " +
                        " FROM " +
                        " Item itm " +
                        " left outer join itemVendor on (itemVendor.ItemID=Itm.ItemID) " +
                        " inner join vendor on (vendor.vendorid=itemvendor.vendorid) " +
                        " WHERE " +
                        " itm.QtyInStock < itm.ReOrderLevel " +
                        " and isnull(itm.ExclFromAutoPO,0) = 0" +
                        " AND itm.ReOrderLevel>0";


                    SQLFill += " AND Vendor.VendorCode='" + txtVendorCode.Text.Replace("'", "''") + "'";

                }

                //IDbConnection conn = DataFactory.CreateConnection(POS_Core.Resources.Configuration.ConnectionString);
            }
            catch (Exception Ex)
            {
            }
        }

        private void AutoPO()
        {
            #region Declear and Initialize Queries
            //SQLFill = String.Empty;
            #endregion

            dsPoDetails = new PODetailData();
            PODetailTable dtPodetail = new PODetailTable();
            PurchaseOrder po = new PurchaseOrder();
            string seqItem = string.Empty;

            try
            {
                if (chkForcedVendor.Checked)//Forced Vendor
                {
                    //#region Sprint 24 - PRIMEPOS-2343 08-Sep-2016 JY Added logic for non-EDI vendor
                    //VendorRow oVendorRow = null;
                    //VendorData oVendorData = null;
                    //VendorSvr oVendorSvr = new VendorSvr();
                    //oVendorData = oVendorSvr.Populate(txtVendorCode.Text);
                    //oVendorRow = (VendorRow)oVendorData.Vendor.Rows[0];
                    //#endregion

                    //SQLFill = CreateAutoPO(clsPOSDBConstants.FORCEDVENDOR);
                    //dsPoDetails = po.AutoPOData(SQLFill);
                    if (txtVendorCode.Text.Trim().Length > 0)
                    {
                        dsPoDetails = new PODetailData();
                        if (optByName.CheckedIndex == 0)
                        {
                            CreateAutoPOBySpecifiedVendor(txtVendorCode.Text, "Forced");  //Sprint-27 - PRIMEPOS-2472 04-Jan-2018 JY uncommented
                            //#region Sprint 24 - PRIMEPOS-2343 08-Sep-2016 JY Added logic for non-EDI vendor
                            //if (oVendorRow.USEVICForEPO == true)
                            //    CreateAutoPOBySpecifiedVendor(txtVendorCode.Text, "Forced");
                            //else
                            //    CreateAutoPO(clsPOSDBConstants.IGNOREVENDOR);
                            //#endregion
                        }
                        else if (optByName.CheckedIndex == 1)
                        {
                            CreateAutoPOBySpecifiedVendorReorder(txtVendorCode.Text.Trim(), "Forced");    //Sprint-27 - PRIMEPOS-2472 04-Jan-2018 JY uncommented
                            //#region Sprint 24 - PRIMEPOS-2343 08-Sep-2016 JY Added logic for non-EDI vendor
                            //if (oVendorRow.USEVICForEPO == true)
                            //    CreateAutoPOBySpecifiedVendorReorder(txtVendorCode.Text.Trim(), "Forced");
                            //else
                            //    CreateAutoPO(clsPOSDBConstants.IGNOREVENDOR);
                            //#endregion
                        }
                        else     //Added By Amit Date 1 July 2011
                        {
                            CreateAutoPOBySpecifiedVendorQtyZero(txtVendorCode.Text, "Forced"); //Sprint-27 - PRIMEPOS-2472 04-Jan-2018 JY uncommented
                            //#region Sprint 24 - PRIMEPOS-2343 08-Sep-2016 JY Added logic for non-EDI vendor
                            //if (oVendorRow.USEVICForEPO == true)
                            //    CreateAutoPOBySpecifiedVendorQtyZero(txtVendorCode.Text, "Forced"); 
                            //else
                            //    CreateAutoPO(clsPOSDBConstants.IGNOREVENDOR);
                            //#endregion
                        }
                    }
                }
                else if (lstSequence.Items.Contains(clsPOSDBConstants.PREFEREDVENDOR) && !lstSequence.Items.Contains(clsPOSDBConstants.LASTVENDOR) && !lstSequence.Items.Contains(clsPOSDBConstants.DEFAULTVENDOR)) //Only Prefered Vendor
                {

                    //SQLFill = CreateAutoPO(clsPOSDBConstants.PREFEREDVENDOR);
                    //dsPoDetails = po.AutoPOData(SQLFill);
                    dsPoDetails = new PODetailData();
                    if (optByName.CheckedIndex == 0)
                    {
                        CreateAutoPOByPreferedVendor();
                    }
                    else if (optByName.CheckedIndex == 1)
                    {
                        CreateAutoPOByPreferedVendorReorder();
                    }
                    else
                    {
                        CreateAutoPOByPreferredVendorQtyZero();
                    }
                }
                else if (!lstSequence.Items.Contains(clsPOSDBConstants.PREFEREDVENDOR) && lstSequence.Items.Contains(clsPOSDBConstants.LASTVENDOR) && !lstSequence.Items.Contains(clsPOSDBConstants.DEFAULTVENDOR))
                {
                    //SQLFill = CreateAutoPO(clsPOSDBConstants.LASTVENDOR);
                    //dsPoDetails = po.AutoPOData(SQLFill);
                    dsPoDetails = new PODetailData();
                    if (optByName.CheckedIndex == 0)
                    {
                        CreateAutoPOByLastVendor();
                    }
                    else if (optByName.CheckedIndex == 1)
                    {
                        CreateAutoPOByLastVendorReOrder();
                    }
                    else
                    {
                        CreateAutoPOByLastVendorQtyZero();
                    }
                }
                else if (!lstSequence.Items.Contains(clsPOSDBConstants.PREFEREDVENDOR) && !lstSequence.Items.Contains(clsPOSDBConstants.LASTVENDOR) && lstSequence.Items.Contains(clsPOSDBConstants.DEFAULTVENDOR))
                {
                    //SQLFill = CreateAutoPO(clsPOSDBConstants.DEFAULTVENDOR);
                    //dsPoDetails = po.AutoPOData(SQLFill);
                    dsPoDetails = new PODetailData();
                    if (optByName.CheckedIndex == 0)
                    {
                        CreateAutoPOBySpecifiedVendor(Configuration.CPrimeEDISetting.DefaultVendor, "Default"); //PRIMEPOS-3167 07-Nov-2022 JY Modified
                    }
                    else if (optByName.CheckedIndex == 1)
                    {
                        CreateAutoPOBySpecifiedVendorReorder(Configuration.CPrimeEDISetting.DefaultVendor, "Default");  //PRIMEPOS-3167 07-Nov-2022 JY Modified
                    }
                    else
                    {
                        CreateAutoPOBySpecifiedVendorQtyZero(Configuration.CPrimeEDISetting.DefaultVendor, "Default");  //PRIMEPOS-3167 07-Nov-2022 JY Modified
                    }
                }
                else
                {
                    PODetailData dsPoDetailtemp = new PODetailData();

                    //lstSequence.Items.Add(clsPOSDBConstants.ITEMOWNER);
                    for (int loopCnt = 0; loopCnt < lstSequence.Items.Count + 1; loopCnt++)
                    {
                        //Added by Krishna on 20April 2011
                        if (lstSequence.Items.Contains(clsPOSDBConstants.IGNOREVENDOR))
                        {
                            CreateAutoPO(clsPOSDBConstants.IGNOREVENDOR);

                            if (dsPoDetails == null) break;

                            foreach (PODetailRow pdr in dsPoDetails.PODetail)
                            {
                                if (dsPoDetailtemp == null)
                                {
                                    dsPoDetailtemp = new PODetailData();
                                }
                                dsPoDetailtemp.PODetail.AddRow(pdr.VendorID.ToString(), pdr.VendorName, Configuration.convertNullToInt(pdr.BestPrice)
                                    , pdr.BestVendor, 0, 0, (dsPoDetailtemp.PODetail.Rows.Count + 1), 0, pdr.QTY, pdr.Cost,
                                    pdr.ItemID, pdr.Comments, pdr.ItemDescription, pdr.Price, pdr.SoldItems, pdr.VendorItemCode, pdr.QtyInStock,
                                    pdr.ReOrderLevel, pdr.MinOrdQty, pdr.DeptName, pdr.SubDeptName, pdr.RetailPrice, pdr.ItemPrice, pdr.Discount, pdr.InvRecDate, pdr.Packetunit.Trim(), pdr.PacketSize, pdr.ProcessedQTY, pdr.Cost);

                            }
                            break;
                        }
                        //Till here Added by Krishna Following Else block also.....Contents were Already der...only added in else.
                        else
                        {
                            if (loopCnt == lstSequence.Items.Count)
                                seqItem = clsPOSDBConstants.ITEMOWNER;
                            else
                                seqItem = lstSequence.Items[loopCnt].ToString();

                            CreateAutoPO(seqItem);
                        }

                        if (dsPoDetails == null) break;
                        int localindex = 0;
                        foreach (PODetailRow pdr in dsPoDetails.PODetail)
                        {
                            if (dsPoDetailtemp == null)
                            {
                                dsPoDetailtemp = new PODetailData();
                            }

                            DataRow[] pdrs = dsPoDetailtemp.PODetail.Select("ItemID='" + pdr["ItemID"].ToString() + "'");
                            if (pdrs == null || pdrs.Length == 0)
                            {
                                dsPoDetailtemp.PODetail.AddRow(pdr.VendorID.ToString(), pdr.VendorName, Configuration.convertNullToInt(pdr.BestPrice)
                                    , pdr.BestVendor, 0, 0, (dsPoDetailtemp.PODetail.Rows.Count + 1), 0, pdr.QTY, pdr.Cost,
                                    pdr.ItemID, pdr.Comments, pdr.ItemDescription, pdr.Price, pdr.SoldItems, pdr.VendorItemCode,
                                    pdr.QtyInStock, pdr.ReOrderLevel, pdr.MinOrdQty, pdr.DeptName, pdr.SubDeptName, pdr.RetailPrice, pdr.ItemPrice, pdr.Discount, pdr.InvRecDate, pdr.Packetunit.Trim(), pdr.PacketSize, pdr.ProcessedQTY, pdr.Cost);

                            }
                        }
                    }
                    dsPoDetails = dsPoDetailtemp;
                    //Added By shitaljit on 28 Sept 2012
                    if (lstSequence.Items.Contains(clsPOSDBConstants.IGNOREVENDOR) == false)
                    {
                        CreateAutoPO(clsPOSDBConstants.IGNOREVENDOR);
                    }
                }

                //Added by Amit Date 4 Aug 2011
                if (optionReport.CheckedIndex == 0)
                {
                    PODetailData dsPoDetailsTemp = new PODetailData();

                    foreach (PODetailRow podRowTemp in dsPoDetails.PODetail.Rows)
                    {

                        DataRow[] row = dsPoDetailsTemp.PODetail.Select("ItemID ='" + podRowTemp["ItemId"] + "'");

                        if (row == null || row.Length == 0)    //Adds the row in temp ds for first time
                        {

                            dsPoDetailsTemp.PODetail.AddRow(podRowTemp.VendorID.ToString(), podRowTemp.VendorName, Configuration.convertNullToInt(podRowTemp.BestPrice)
                                        , podRowTemp.BestVendor, 0, 0, (dsPoDetailsTemp.PODetail.Rows.Count + 1), 0, podRowTemp.QTY, podRowTemp.Cost,
                                        podRowTemp.ItemID, podRowTemp.Comments, podRowTemp.ItemDescription, podRowTemp.Price, podRowTemp.SoldItems, podRowTemp.VendorItemCode, podRowTemp.QtyInStock,
                                       podRowTemp.ReOrderLevel, podRowTemp.MinOrdQty, podRowTemp.DeptName, podRowTemp.SubDeptName, podRowTemp.RetailPrice
                                       , podRowTemp.ItemPrice, podRowTemp.Discount, podRowTemp.InvRecDate, podRowTemp.Packetunit.Trim(), podRowTemp.PacketSize, podRowTemp.ProcessedQTY, podRowTemp.Cost);

                        }
                        else //if (!isFromPO)   // Increases the qty of existing item //Modidified by amit Date 10 20 2011 added if after else if Reorder level option is checked dont sum qty
                        {
                            int tempPODetailID = Configuration.convertNullToInt(row[0].ItemArray[0]);
                            //int tempSoldItems = Configuration.convertNullToInt(row[0]ItemArray[25]);
                            int tempSoldItems = Configuration.convertNullToInt(row[0]["SoldItems"]);//Modifeid by shitaljit change form item array[25] to col name 
                                                                                                    //Following if is aded by shitaljit to stop considering QTy of  item sold in case of the AutoPO by Re-Order level.
                            if (optByName.CheckedIndex != 1)
                            {
                                podRowTemp.QTY += tempSoldItems;
                            }
                            dsPoDetailsTemp.PODetail.Rows.Remove(row[0]);

                            dsPoDetailsTemp.PODetail.AddRow(podRowTemp.VendorID.ToString(), podRowTemp.VendorName, Configuration.convertNullToInt(podRowTemp.BestPrice)
                                        , podRowTemp.BestVendor, 0, 0, tempPODetailID, 0, podRowTemp.QTY, podRowTemp.Cost,
                                        podRowTemp.ItemID, podRowTemp.Comments, podRowTemp.ItemDescription, podRowTemp.Price, podRowTemp.SoldItems + tempSoldItems, podRowTemp.VendorItemCode, podRowTemp.QtyInStock,
                                       podRowTemp.ReOrderLevel, podRowTemp.MinOrdQty, podRowTemp.DeptName, podRowTemp.SubDeptName, podRowTemp.RetailPrice
                                       , podRowTemp.ItemPrice, podRowTemp.Discount, podRowTemp.InvRecDate, podRowTemp.Packetunit.Trim(), podRowTemp.PacketSize, podRowTemp.ProcessedQTY, podRowTemp.Cost);

                        }

                    }

                    dsPoDetails = dsPoDetailsTemp;
                    SetPacketToOrder(ref dsPoDetails);
                }
                //End
            }
            catch (Exception ex)
            {
                clsUIHelper.ShowErrorMsg(ex.Message);
            }
        }

        private PODetailTable CreateAutoPOByPreferedVendor()
        {
            bool queryResult = false;
            PODetailTable podTable = null;
            DataSet dsTransHeader = new DataSet();
            DataSet dsTransDetailData = new DataSet();
            ItemVendorData dsItemVendor = new ItemVendorData();
            ItemData dsItems = new ItemData();
            TransHeaderSvr oPOSTrans = new TransHeaderSvr();
            TransDetailSvr oPOSTransDetail = new TransDetailSvr();
            ItemSvr oItem = new ItemSvr();
            ItemVendorSvr oItemVendor = new ItemVendorSvr();
            //Added by Amit Date 30 June 2011
            DepartmentData dsDepartmentData = new DepartmentData();
            DepartmentSvr oDepartment = new DepartmentSvr();
            SubDepartmentData dsSubDepartmentData = new SubDepartmentData();
            SubDepartmentSvr oSubDepartment = new SubDepartmentSvr();
            //End
            //Added By Amit Date 29 Nov 2011
            InvRecvHeaderSvr oInvRecHeader = new InvRecvHeaderSvr();
            DataSet dsInvRecDate = new DataSet();
            //End
            string SelectClause = string.Empty;
            if (optSearchSet.CheckedIndex == 0)
            {
                //changed by SRT(Abhishek)  Date : 21 Sept. 2009
                //Added condition to check if Transaction should be of Sell Type 
                if (Configuration.CPrimeEDISetting.ConsiderReturnTrans)  //Sprint-27 - PRIMEPOS-2390 28-Sep-2017 JY Added   //PRIMEPOS-3167 07-Nov-2022 JY Modified
                    SelectClause = " Trans.TransDate >= DateAdd(D,-" + numNumberOfDays.Value + ",getdate()) ";
                else
                    SelectClause = " Trans.TransDate >= DateAdd(D,-" + numNumberOfDays.Value + ",getdate()) AND Trans.TransType =1 ";
                //End of changed by SRT(Abhishek)  Date : 21 Sept. 2009

            }
            else
            {
                //changed by SRT(Abhishek)  Date : 21 Sept. 2009
                //Added condition to check if Transaction should be of Sell Type 
                if (Configuration.CPrimeEDISetting.ConsiderReturnTrans)  //Sprint-27 - PRIMEPOS-2390 28-Sep-2017 JY Added    //PRIMEPOS-3167 07-Nov-2022 JY Modified 
                    SelectClause = " Trans.TransDate between cast('" + DateTime.Parse(dtpFromDate.Value.ToString()).ToString() + "' as datetime) AND cast('" + DateTime.Parse(dtpToDate.Value.ToString()).ToString() + "' as datetime) ";
                else
                    SelectClause = " Trans.TransDate between cast('" + DateTime.Parse(dtpFromDate.Value.ToString()).ToString() + "' as datetime) AND cast('" + DateTime.Parse(dtpToDate.Value.ToString()).ToString() + "' as datetime) AND Trans.TransType=1 ";
                //End of changed by SRT(Abhishek)  Date : 21 Sept. 2009

            }
            lblMessage.Text = "Aquiring Transactions For Prefered Vendor.";
            Application.DoEvents();
            string strSQL = string.Empty;
            //dsTransHeader = oPOSTrans.PopulateList(SelectClause);   //PRIMEPOS-3097 28-Jun-2022 JY commented
            dsTransHeader = oPOSTrans.PopulateList(SelectClause, out strSQL);   //PRIMEPOS-3097 28-Jun-2022 JY Added
            if (dsTransHeader == null || dsTransHeader.Tables.Count == 0 || dsTransHeader.Tables[0].Rows.Count == 0)
            {
                throw (new Exception("There are no more transactions in this period."));
            }
            #region PRIMEPOS-3097 28-Jun-2022 JY Commented
            //List<string> lstData = new List<string>();
            //foreach (DataRow Dr in dsTransHeader.Tables[0].Rows)
            //{
            //    lstData.Add(Dr[0].ToString());
            //}
            //dsTransDetailData = oPOSTransDetail.PopulateList(" where TransID in (" + string.Join(",", lstData.ToArray()) + ")", out queryResult);
            //lstData = new List<string>();
            //foreach (DataRow Dr in dsTransDetailData.Tables[0].Rows)
            //{
            //    lstData.Add("'" + Dr[0].ToString() + "'");
            //}
            #endregion

            string strDistItems = string.Empty;
            dsTransDetailData = oPOSTransDetail.PopulateList(" where TransID in (" + strSQL + ")", out queryResult, out strDistItems);    //PRIMEPOS-3097 28-Jun-2022 JY Added
            lblMessage.Text = "Aquiring Transaction Items For Prefered Vendor.";
            Application.DoEvents();

            #region Sprint-19 - 1883 24-Mar-2015 JY Commented
            // //Start: Added By Amit Date 30 June 2011
            // List<string> lstDeptData = new List<string>();
            // List<string> lstSubDeptData = new List<string>();

            // if (this.txtDepartment.Text.Trim() != "" && txtDepartment.Tag.ToString() != ""
            //     && this.txtSubDepartment.Text.Trim() == "" && txtSubDepartment.Tag.ToString() == "")
            // {
            //     dsDepartmentData = oDepartment.PopulateList("where DeptCode in (" + this.txtDepartment.Tag.ToString().Trim() + ")");

            //     foreach (DataRow Dr in dsDepartmentData.Tables[0].Rows)
            //     {
            //         lstDeptData.Add("'" + Dr[0].ToString() + "'");
            //         Application.DoEvents();
            //     }
            //     dsSubDepartmentData = oSubDepartment.PopulateList("where DepartmentId in (" + string.Join(",", lstDeptData.ToArray()) + ")");

            //     dsItems = oItem.PopulateList(" where ItemId in (" + string.Join(",", lstData.ToArray()) + ") and DepartmentID in (" + string.Join(",", lstDeptData.ToArray()) + ")");
            // }
            // else if (this.txtDepartment.Text.Trim() == "" && txtDepartment.Tag.ToString() == ""
            //     && this.txtSubDepartment.Text.Trim() != "" && txtSubDepartment.Tag.ToString() != "")
            // {
            //     dsSubDepartmentData = oSubDepartment.PopulateList("where SubDepartmentId in (" + this.txtSubDepartment.Tag.ToString().Trim() + ")");
            //     foreach (DataRow Dr in dsSubDepartmentData.Tables[0].Rows)
            //     {
            //         lstSubDeptData.Add("'" + Dr[0].ToString() + "'");
            //         lstDeptData.Add("'" + Dr[1].ToString() + "'");
            //         Application.DoEvents();
            //     }
            //     dsDepartmentData = oDepartment.PopulateList("where DeptID in (" + string.Join(",", lstDeptData.ToArray()) + ")");

            //     dsItems = oItem.PopulateList(" where ItemId in (" + string.Join(",", lstData.ToArray()) + ") and SubDepartmentID in (" + string.Join(",", lstSubDeptData.ToArray()) + ")");
            // }
            // else if (this.txtDepartment.Text.Trim() != "" && txtDepartment.Tag.ToString() != ""
            //&& this.txtSubDepartment.Text.Trim() != "" && txtSubDepartment.Tag.ToString() != "")
            // {
            //     dsDepartmentData = oDepartment.PopulateList("where DeptCode in (" + this.txtDepartment.Tag.ToString().Trim() + ")");

            //     foreach (DataRow Dr in dsDepartmentData.Tables[0].Rows)
            //     {
            //         lstDeptData.Add("'" + Dr[0].ToString() + "'");
            //         Application.DoEvents();
            //     }

            //     dsSubDepartmentData = oSubDepartment.PopulateList("where SubDepartmentId in (" + this.txtSubDepartment.Tag.ToString().Trim() + ")");
            //     foreach (DataRow Dr in dsSubDepartmentData.Tables[0].Rows)
            //     {
            //         lstSubDeptData.Add("'" + Dr[0].ToString() + "'");
            //         Application.DoEvents();
            //     }

            //     dsItems = oItem.PopulateList(" where ItemId in (" + string.Join(",", lstData.ToArray()) + ") and DepartmentID in (" + string.Join(",", lstDeptData.ToArray()) + ") and SubDepartmentID in (" + string.Join(",", lstSubDeptData.ToArray()) + ")");

            // }
            // else
            // {
            //     dsDepartmentData = (DepartmentData)oDepartment.PopulateList(" where 1=1 ");
            //     dsSubDepartmentData = (SubDepartmentData)oSubDepartment.PopulateList(" where 1=1 ");

            //     dsItems = oItem.PopulateList(" where ItemId in (" + string.Join(",", lstData.ToArray()) + ")");
            // }
            // //End
            #endregion

            #region Sprint-19 - 1883 23-Mar-2015 JY Added location filter and also improved the existing filter code
            string strFilter = string.Empty;
            List<string> lstDeptData = new List<string>();
            List<string> lstSubDeptData = new List<string>();

            if (this.txtDepartment.Text.Trim() != "" && txtDepartment.Tag.ToString() != "")
            {
                dsDepartmentData = oDepartment.PopulateList("where DeptCode in (" + this.txtDepartment.Tag.ToString().Trim() + ")");

                foreach (DataRow Dr in dsDepartmentData.Tables[0].Rows)
                {
                    lstDeptData.Add("'" + Dr[0].ToString() + "'");
                }
                strFilter += " and DepartmentID in (" + string.Join(",", lstDeptData.ToArray()) + ")";

                if (this.txtSubDepartment.Text.Trim() != "" && txtSubDepartment.Tag.ToString() != "")
                {
                    dsSubDepartmentData = oSubDepartment.PopulateList("where SubDepartmentId in (" + this.txtSubDepartment.Tag.ToString().Trim() + ")");
                    foreach (DataRow Dr in dsSubDepartmentData.Tables[0].Rows)
                    {
                        lstSubDeptData.Add("'" + Dr[0].ToString() + "'");
                        lstDeptData.Add("'" + Dr[1].ToString() + "'");
                        Application.DoEvents();
                    }
                    strFilter += " and SubDepartmentID in (" + string.Join(",", lstSubDeptData.ToArray()) + ")";
                }
                else
                {
                    dsSubDepartmentData = oSubDepartment.PopulateList("where DepartmentId in (" + string.Join(",", lstDeptData.ToArray()) + ")");
                }
            }
            else
            {
                if (this.txtSubDepartment.Text.Trim() != "" && txtSubDepartment.Tag.ToString() != "")
                {
                    dsSubDepartmentData = oSubDepartment.PopulateList("where SubDepartmentId in (" + this.txtSubDepartment.Tag.ToString().Trim() + ")");
                    foreach (DataRow Dr in dsSubDepartmentData.Tables[0].Rows)
                    {
                        lstSubDeptData.Add("'" + Dr[0].ToString() + "'");
                        lstDeptData.Add("'" + Dr[1].ToString() + "'");
                        Application.DoEvents();
                    }
                    strFilter += " and SubDepartmentID in (" + string.Join(",", lstSubDeptData.ToArray()) + ")";

                    dsDepartmentData = oDepartment.PopulateList("where DeptID in (" + string.Join(",", lstDeptData.ToArray()) + ")");
                }
                else
                {
                    dsDepartmentData = (DepartmentData)oDepartment.PopulateList(" where 1=1 ");
                    dsSubDepartmentData = (SubDepartmentData)oSubDepartment.PopulateList(" where 1=1 ");
                }
            }

            if (this.txtItemLocation.Text.Trim() != "" && txtItemLocation.Tag.ToString() != "")
            {
                strFilter += " and LOCATION in (" + txtItemLocation.Text + ")";
            }
            //dsItems = oItem.PopulateList(" where ItemId in (" + string.Join(",", lstData.ToArray()) + ") " + strFilter);    //PRIMEPOS-3097 28-Jun-2022 JY Commented
            dsItems = oItem.PopulateList(" where ItemId in (" + strDistItems + ") " + strFilter);   //PRIMEPOS-3097 28-Jun-2022 JY Added
            #endregion

            //dsItemVendor = oItemVendor.PopulateList(" where ItemId in (" + string.Join(",", lstData.ToArray()) + ")");    //PRIMEPOS-3097 28-Jun-2022 JY Commented
            dsItemVendor = oItemVendor.PopulateList(" where ItemId in (" + strDistItems + ")", true);  //PRIMEPOS-3097 28-Jun-2022 JY Added
            if (dsItems == null || dsItems.Tables.Count == 0 || dsItems.Tables[0].Rows.Count == 0)
            {
                throw (new Exception("There are no more items matching transactions of selected period."));
            }

            if (dsItemVendor == null || dsItemVendor.Tables.Count == 0 || dsItemVendor.Tables[0].Rows.Count == 0)
            {
                throw (new Exception("There are no more vendor items matching transactions of selected period."));
            }

            //Added By Amit Date 29 Nov 2011
            //dsInvRecDate = oInvRecHeader.ItemInvRecDate(lstData); //PRIMEPOS-3097 28-Jun-2022 JY Commented
            dsInvRecDate = oInvRecHeader.ItemInvRecDate(strDistItems);   //PRIMEPOS-3097 28-Jun-2022 JY Added
            //End

            //int count = 0;
            podTable = new PODetailTable();
            PODetailRow podRow = null;
            //Added By Amit Date 30 June 2011
            int index = 0;

            //foreach (ItemRow oItemRow in dsItems.Tables[0].Rows)
            //Added By Amit date 29 July 2011            
            foreach (DataRow podRowTemp in dsTransDetailData.Tables[0].Rows)
            {
                DataRow[] oItemRowTemp = dsItems.Tables[0].Select("ItemID='" + podRowTemp["ItemID"] + "'");
                ItemRow oItemRow = null;

                if (oItemRowTemp.Length > 0)
                    oItemRow = (ItemRow)oItemRowTemp[0];
                else
                    continue;

                if (!oItemRow.ExclFromAutoPO)
                {
                    lblMessage.Text = "Scanning Item #" + oItemRow.ItemID.Trim() + " For Prefered Vendor";
                    Application.DoEvents();

                    if (Configuration.convertNullToString(oItemRow.PreferredVendor).Trim().Length > 0)
                    {
                        DataRow[] oItemRows = dsItemVendor.Tables[0].Select(" ItemId='" + oItemRow.ItemID.Trim() + "' AND VENDORCODE='" + oItemRow.PreferredVendor.Replace("'", "''") + "'");    //Sprint-27 - 10-Oct-2017 JY to resolve issue, replaced single quote with double quote
                        if (oItemRows != null && oItemRows.Length > 0)
                        {
                            ItemVendorRow oItemVendorRow = (ItemVendorRow)oItemRows[0];
                            //DataRow[] TransRows = dsTransDetailData.Tables[0].Select(" ItemId='" + oItemRow.ItemID.Trim() + "'");

                            //if (TransRows != null && TransRows.Length > 0)
                            //{
                            //    decimal qty = 0;
                            //    foreach (DataRow dr in TransRows)
                            //    {
                            //        qty += Configuration.convertNullToDecimal(dr["Qty"].ToString());
                            //    }
                            //    count++;
                            podRow = dsPoDetails.PODetail.NewPODetailRow();
                            podRow.ItemID = oItemRow.ItemID;
                            podRow.ItemDescription = oItemRow.Description;
                            podRow.VendorID = oItemVendorRow.VendorID;
                            podRow.VendorItemCode = oItemVendorRow.VendorItemID;
                            podRow.VendorName = oItemVendorRow.VendorCode;
                            podRow.QTY = Configuration.convertNullToInt(podRowTemp["QTY"].ToString());
                            podRow.QtyInStock = oItemRow.QtyInStock;
                            podRow.ReOrderLevel = oItemRow.ReOrderLevel;
                            //Modified by shitaljit as per Fahad for those Pharmacy which are not using PO 
                            //We should pupulate item cost from Item table not from ItemVendor table.
                            podRow.SoldItems = Configuration.convertNullToInt(podRowTemp["QTY"].ToString());
                            if (isFromPO == true)
                            {
                                podRow.Price = oItemVendorRow.VenorCostPrice;
                            }
                            else
                            {
                                podRow.Price = oItemRow.LastCostPrice;
                            }

                            podRow.PacketSize = oItemRow.PckSize;
                            podRow.PacketQuant = oItemRow.PckQty;
                            podRow.Packetunit = oItemRow.PckUnit;
                            //Start : Added By Amit Date 5 jul 2011
                            podRow.MinOrdQty = oItemRow.MinOrdQty;
                            DataRow[] DeptRow = dsDepartmentData.Tables[0].Select("DeptID=" + oItemRow.DepartmentID);
                            if (DeptRow.Length != 0)
                                podRow.DeptName = DeptRow[0][2].ToString();

                            DataRow[] SubDeptRow = dsSubDepartmentData.Tables[0].Select("SubDepartmentID=" + oItemRow.SubDepartmentID);

                            if (SubDeptRow.Length != 0)
                                podRow.SubDeptName = SubDeptRow[0][2].ToString();
                            //End
                            //Added by Amit Date 27 july 2011
                            podRow.RetailPrice = Configuration.convertNullToDecimal(podRowTemp["Price"].ToString());
                            podRow.ItemPrice = Configuration.convertNullToDecimal(podRowTemp["Price"].ToString()) - Configuration.convertNullToDecimal(podRowTemp["Discount"].ToString());
                            podRow.Discount = Configuration.convertNullToDecimal(podRowTemp["Discount"].ToString());
                            //End
                            //Added By Amit Date 29 Nov 2011
                            DataRow[] InvRecDateRow = dsInvRecDate.Tables[0].Select(" ItemId='" + oItemRow.ItemID.Trim() + "'");
                            if (InvRecDateRow.Length != 0)
                                podRow.InvRecDate = (System.DateTime)InvRecDateRow[0][clsPOSDBConstants.PODetail_Fld_InvRecDate];
                            //End

                            dsPoDetails.PODetail.AddRow(podRow);

                            index++;
                            //}
                        }
                    }
                }
            }
            SetPacketToOrder(ref dsPoDetails);
            if (dsPoDetails.PODetail.Rows.Count > 0)
            {
                lblMessage.Text = podTable.Rows.Count.ToString() + " No(s) Items Acquired For Purchase Order By Prefered Vendor";
                Application.DoEvents();
            }

            return podTable;
        }

        private PODetailTable CreateAutoPOBySpecifiedVendor(string VendCode, string ForWhichCat)
        {
            bool queryResult = false;
            PODetailTable podTable = null;
            DataSet dsTransHeader = new DataSet();
            DataSet dsTransDetailData = new DataSet();
            ItemVendorData dsItemVendor = new ItemVendorData();
            ItemData dsItems = new ItemData();
            TransHeaderSvr oPOSTrans = new TransHeaderSvr();
            TransDetailSvr oPOSTransDetail = new TransDetailSvr();
            ItemSvr oItem = new ItemSvr();
            ItemVendorSvr oItemVendor = new ItemVendorSvr();
            //Added by Amit Date 30 June 2011
            DepartmentData dsDepartmentData = new DepartmentData();
            DepartmentSvr oDepartment = new DepartmentSvr();
            SubDepartmentData dsSubDepartmentData = new SubDepartmentData();
            SubDepartmentSvr oSubDepartment = new SubDepartmentSvr();
            //End
            //Added By Amit Date 16 Aug 2011
            VendorData dsVendorData = new VendorData();
            VendorSvr oVendor = new VendorSvr();
            //End
            //Added By Amit Date 29 Nov 2011
            InvRecvHeaderSvr oInvRecHeader = new InvRecvHeaderSvr();
            DataSet dsInvRecDate = new DataSet();
            //End

            string SelectClause = string.Empty;
            if (optSearchSet.CheckedIndex == 0)
            {
                //changed by SRT(Abhishek)  Date : 21 Sept. 2009
                //Added condition to check if Transaction should be of Sell Type 
                if (Configuration.CPrimeEDISetting.ConsiderReturnTrans)  //Sprint-27 - PRIMEPOS-2390 28-Sep-2017 JY Added    //PRIMEPOS-3167 07-Nov-2022 JY Modified 
                    SelectClause = " Trans.TransDate >= DateAdd(D,-" + numNumberOfDays.Value + ",getdate()) ";
                else
                    SelectClause = " Trans.TransDate >= DateAdd(D,-" + numNumberOfDays.Value + ",getdate()) AND Trans.TransType=1 ";
                //End of changed by SRT(Abhishek)  Date : 21 Sept. 2009
            }
            else
            {
                //changed by SRT(Abhishek)  Date : 21 Sept. 2009
                //Added condition to check if Transaction should be of Sell Type 

                //changed by SRT(Abhishek)  Date : 30 Sept. 2009
                if (Configuration.CPrimeEDISetting.ConsiderReturnTrans)  //Sprint-27 - PRIMEPOS-2390 28-Sep-2017 JY Added    //PRIMEPOS-3167 07-Nov-2022 JY Modified 
                    SelectClause = " Trans.TransDate between cast('" + DateTime.Parse(dtpFromDate.Value.ToString()).ToString() + "' as datetime) AND cast('" + DateTime.Parse(dtpToDate.Value.ToString()).ToString() + "' as datetime) ";
                else
                    SelectClause = " Trans.TransDate between cast('" + DateTime.Parse(dtpFromDate.Value.ToString()).ToString() + "' as datetime) AND cast('" + DateTime.Parse(dtpToDate.Value.ToString()).ToString() + "' as datetime) AND Trans.TransType=1 "; //AND Trans.TransType=1 

                //End Of changed by SRT(Abhishek)  Date : 30 Sept. 2009

                //End of changed by SRT(Abhishek)  Date : 21 Sept. 2009

            }
            lblMessage.Text = "Aquiring Transactions For " + ForWhichCat + " Vendor.";
            Application.DoEvents();
            string strSQL = string.Empty;
            //dsTransHeader = oPOSTrans.PopulateList(SelectClause); //PRIMEPOS-3097 28-Jun-2022 JY commented
            dsTransHeader = oPOSTrans.PopulateList(SelectClause, out strSQL);   //PRIMEPOS-3097 28-Jun-2022 JY Added
            if (dsTransHeader == null || dsTransHeader.Tables.Count == 0 || dsTransHeader.Tables[0].Rows.Count == 0)
            {
                throw (new Exception("There are no more transactions in this period."));
            }
            #region PRIMEPOS-3097 28-Jun-2022 JY Commented
            //List<string> lstData = new List<string>();
            //foreach (DataRow Dr in dsTransHeader.Tables[0].Rows)
            //{
            //    lstData.Add(Dr[0].ToString());
            //    Application.DoEvents();
            //}
            //dsTransDetailData = oPOSTransDetail.PopulateList(" where TransID in (" + string.Join(",", lstData.ToArray()) + ")", out queryResult);
            //lstData = new List<string>();
            //foreach (DataRow Dr in dsTransDetailData.Tables[0].Rows)
            //{
            //    lstData.Add("'" + Dr[0].ToString() + "'");
            //    Application.DoEvents();
            //}
            #endregion

            string strDistItems = string.Empty;
            dsTransDetailData = oPOSTransDetail.PopulateList(" where TransID in (" + strSQL + ")", out queryResult, out strDistItems);    //PRIMEPOS-3097 28-Jun-2022 JY Added
            lblMessage.Text = "Aquiring Transaction Items For " + ForWhichCat + " Vendor.";
            Application.DoEvents();

            #region Sprint-19 - 1883 23-Mar-2015 JY Commented
            //Start: Added By Amit Date 30 June 2011
            // List<string> lstDeptData = new List<string>();
            // List<string> lstSubDeptData = new List<string>();

            // if (this.txtDepartment.Text.Trim() != "" && txtDepartment.Tag.ToString() != ""
            //     && this.txtSubDepartment.Text.Trim() == "" && txtSubDepartment.Tag.ToString() == "")
            // {
            //     dsDepartmentData = oDepartment.PopulateList("where DeptCode in (" + this.txtDepartment.Tag.ToString().Trim() + ")");

            //     foreach (DataRow Dr in dsDepartmentData.Tables[0].Rows)
            //     {
            //         lstDeptData.Add("'" + Dr[0].ToString() + "'");
            //         Application.DoEvents();
            //     }
            //     dsSubDepartmentData = oSubDepartment.PopulateList("where DepartmentId in (" + string.Join(",", lstDeptData.ToArray()) + ")");

            //     dsItems = oItem.PopulateList(" where ItemId in (" + string.Join(",", lstData.ToArray()) + ") and DepartmentID in (" + string.Join(",", lstDeptData.ToArray()) + ")");
            // }
            // else if (this.txtDepartment.Text.Trim() == "" && txtDepartment.Tag.ToString() == ""
            //     && this.txtSubDepartment.Text.Trim() != "" && txtSubDepartment.Tag.ToString() != "")
            // {
            //     dsSubDepartmentData = oSubDepartment.PopulateList("where SubDepartmentId in (" + this.txtSubDepartment.Tag.ToString().Trim() + ")");
            //     foreach (DataRow Dr in dsSubDepartmentData.Tables[0].Rows)
            //     {
            //         lstSubDeptData.Add("'" + Dr[0].ToString() + "'");
            //         lstDeptData.Add("'" + Dr[1].ToString() + "'");
            //         Application.DoEvents();
            //     }
            //     dsDepartmentData = oDepartment.PopulateList("where DeptID in (" + string.Join(",", lstDeptData.ToArray()) + ")");

            //     dsItems = oItem.PopulateList(" where ItemId in (" + string.Join(",", lstData.ToArray()) + ") and SubDepartmentID in (" + string.Join(",", lstSubDeptData.ToArray()) + ")");
            // }
            // else if (this.txtDepartment.Text.Trim() != "" && txtDepartment.Tag.ToString() != ""
            //&& this.txtSubDepartment.Text.Trim() != "" && txtSubDepartment.Tag.ToString() != "")
            // {
            //     dsDepartmentData = oDepartment.PopulateList("where DeptCode in (" + this.txtDepartment.Tag.ToString().Trim() + ")");

            //     foreach (DataRow Dr in dsDepartmentData.Tables[0].Rows)
            //     {
            //         lstDeptData.Add("'" + Dr[0].ToString() + "'");
            //         Application.DoEvents();
            //     }

            //     dsSubDepartmentData = oSubDepartment.PopulateList("where SubDepartmentId in (" + this.txtSubDepartment.Tag.ToString().Trim() + ")");
            //     foreach (DataRow Dr in dsSubDepartmentData.Tables[0].Rows)
            //     {
            //         lstSubDeptData.Add("'" + Dr[0].ToString() + "'");
            //         Application.DoEvents();
            //     }

            //     dsItems = oItem.PopulateList(" where ItemId in (" + string.Join(",", lstData.ToArray()) + ") and DepartmentID in (" + string.Join(",", lstDeptData.ToArray()) + ") and SubDepartmentID in (" + string.Join(",", lstSubDeptData.ToArray()) + ")");

            // }
            // else
            // {
            //     dsDepartmentData = (DepartmentData)oDepartment.PopulateList(" where 1=1 ");
            //     dsSubDepartmentData = (SubDepartmentData)oSubDepartment.PopulateList(" where 1=1 ");

            //     dsItems = oItem.PopulateList(" where ItemId in (" + string.Join(",", lstData.ToArray()) + ")");
            // }
            //End
            #endregion

            #region Sprint-19 - 1883 23-Mar-2015 JY Added location filter and also improved the existing filter code
            string strFilter = string.Empty;
            List<string> lstDeptData = new List<string>();
            List<string> lstSubDeptData = new List<string>();

            if (this.txtDepartment.Text.Trim() != "" && txtDepartment.Tag.ToString() != "")
            {
                dsDepartmentData = oDepartment.PopulateList("where DeptCode in (" + this.txtDepartment.Tag.ToString().Trim() + ")");

                foreach (DataRow Dr in dsDepartmentData.Tables[0].Rows)
                {
                    lstDeptData.Add("'" + Dr[0].ToString() + "'");
                }
                strFilter += " and DepartmentID in (" + string.Join(",", lstDeptData.ToArray()) + ")";

                if (this.txtSubDepartment.Text.Trim() != "" && txtSubDepartment.Tag.ToString() != "")
                {
                    dsSubDepartmentData = oSubDepartment.PopulateList("where SubDepartmentId in (" + this.txtSubDepartment.Tag.ToString().Trim() + ")");
                    foreach (DataRow Dr in dsSubDepartmentData.Tables[0].Rows)
                    {
                        lstSubDeptData.Add("'" + Dr[0].ToString() + "'");
                        lstDeptData.Add("'" + Dr[1].ToString() + "'");
                        Application.DoEvents();
                    }
                    strFilter += " and SubDepartmentID in (" + string.Join(",", lstSubDeptData.ToArray()) + ")";
                }
                else
                {
                    dsSubDepartmentData = oSubDepartment.PopulateList("where DepartmentId in (" + string.Join(",", lstDeptData.ToArray()) + ")");
                }
            }
            else
            {
                if (this.txtSubDepartment.Text.Trim() != "" && txtSubDepartment.Tag.ToString() != "")
                {
                    dsSubDepartmentData = oSubDepartment.PopulateList("where SubDepartmentId in (" + this.txtSubDepartment.Tag.ToString().Trim() + ")");
                    foreach (DataRow Dr in dsSubDepartmentData.Tables[0].Rows)
                    {
                        lstSubDeptData.Add("'" + Dr[0].ToString() + "'");
                        lstDeptData.Add("'" + Dr[1].ToString() + "'");
                        Application.DoEvents();
                    }
                    strFilter += " and SubDepartmentID in (" + string.Join(",", lstSubDeptData.ToArray()) + ")";

                    dsDepartmentData = oDepartment.PopulateList("where DeptID in (" + string.Join(",", lstDeptData.ToArray()) + ")");
                }
                else
                {
                    dsDepartmentData = (DepartmentData)oDepartment.PopulateList(" where 1=1 ");
                    dsSubDepartmentData = (SubDepartmentData)oSubDepartment.PopulateList(" where 1=1 ");
                }
            }

            if (this.txtItemLocation.Text.Trim() != "" && txtItemLocation.Tag.ToString() != "")
            {
                strFilter += " and LOCATION in (" + txtItemLocation.Text + ")";
            }
            //dsItems = oItem.PopulateList(" where ItemId in (" + string.Join(",", lstData.ToArray()) + ") " + strFilter);  //PRIMEPOS-3097 28-Jun-2022 JY Commented
            dsItems = oItem.PopulateList(" where ItemId in (" + strDistItems + ") " + strFilter);   //PRIMEPOS-3097 28-Jun-2022 JY Added
            #endregion

            if (dsItems == null || dsItems.Tables.Count == 0 || dsItems.Tables[0].Rows.Count == 0)
            {
                throw (new Exception("There are no more items matching transactions of selected period."));
            }

            //dsItemVendor = oItemVendor.PopulateList(" where ItemId in (" + string.Join(",", lstData.ToArray()) + ")");    //PRIMEPOS-3097 28-Jun-2022 JY Commented
            dsItemVendor = oItemVendor.PopulateList(" where ItemId in (" + strDistItems + ")", true);  //PRIMEPOS-3097 28-Jun-2022 JY Added

            if (dsItemVendor == null || dsItemVendor.Tables.Count == 0 || dsItemVendor.Tables[0].Rows.Count == 0 && isFromPO)
            {
                throw (new Exception("There are no more vendor items matching transactions of selected period."));
            }
            //Added by Amit Date 16 Aug 2011
            dsVendorData = oVendor.PopulateList(" where 1=1");
            //End
            //Added by Amit Date 29 Nov 2011
            //dsInvRecDate = oInvRecHeader.ItemInvRecDate(lstData); //PRIMEPOS-3097 28-Jun-2022 JY Commented
            dsInvRecDate = oInvRecHeader.ItemInvRecDate(strDistItems);   //PRIMEPOS-3097 28-Jun-2022 JY Added
            //End

            int count = 0;
            podTable = new PODetailTable();
            PODetailRow podRow = null;
            //Added By Amit Date 30 June 2011
            int index = 0;

            //foreach (ItemRow oItemRow in dsItems.Tables[0].Rows)
            //Added By Amit Date 27 July 2011
            foreach (DataRow podRowTemp in dsTransDetailData.Tables[0].Rows)
            {
                DataRow[] oItemRowTemp = dsItems.Tables[0].Select("ItemID='" + podRowTemp["ItemID"] + "'");
                ItemRow oItemRow = null;

                if (oItemRowTemp.Length > 0)
                    oItemRow = (ItemRow)oItemRowTemp[0];
                else
                    continue;
                //End

                lblMessage.Text = "Scanning Item #" + oItemRow.ItemID.Trim() + " For " + ForWhichCat + " Vendor";
                if (!oItemRow.ExclFromAutoPO)
                {
                    Application.DoEvents();

                    DataRow[] oItemRows = dsItemVendor.Tables[0].Select(" ItemId='" + oItemRow.ItemID.Trim() + "' and vendorcode='" + VendCode.Replace("'", "''") + "'");

                    if (oItemRows != null && oItemRows.Length > 0)
                    {
                        ItemVendorRow oItemVendorRow = (ItemVendorRow)oItemRows[0];
                        //DataRow[] TransRows = dsTransDetailData.Tables[0].Select(" ItemId='" + oItemRow.ItemID.Trim() + "' ");
                        //if (TransRows != null && TransRows.Length > 0)
                        //{
                        //    decimal qty = 0;
                        //    foreach (DataRow dr in TransRows)
                        //    {
                        //        qty += Configuration.convertNullToDecimal(dr["Qty"].ToString());
                        //    }
                        //    count++;
                        if (oItemRow.ItemID == "1001650010004")
                        { }
                        podRow = dsPoDetails.PODetail.NewPODetailRow();
                        podRow.ItemID = oItemRow.ItemID;
                        podRow.ItemDescription = oItemRow.Description;
                        podRow.VendorID = oItemVendorRow.VendorID;
                        podRow.VendorItemCode = oItemVendorRow.VendorItemID;
                        podRow.VendorName = oItemVendorRow.VendorCode;
                        podRow.QTY = Configuration.convertNullToInt(podRowTemp["Qty"].ToString());
                        podRow.QtyInStock = oItemRow.QtyInStock;
                        podRow.ReOrderLevel = oItemRow.ReOrderLevel;
                        //Modified by shitaljit as per Fahad for those Pharmacy which are not using PO 
                        //We should pupulate item cost from Item table not from ItemVendor table.
                        podRow.SoldItems = Configuration.convertNullToInt(podRowTemp["QTY"].ToString());
                        if (isFromPO == true)
                        {
                            podRow.Price = oItemVendorRow.VenorCostPrice;
                        }
                        else
                        {
                            podRow.Price = oItemRow.LastCostPrice;
                        }
                        podRow.PacketSize = oItemRow.PckSize;
                        podRow.PacketQuant = oItemRow.PckQty;
                        podRow.Packetunit = oItemRow.PckUnit;
                        //Start : Added By Amit Date 5 jul 2011
                        podRow.MinOrdQty = oItemRow.MinOrdQty;
                        DataRow[] DeptRow = dsDepartmentData.Tables[0].Select("DeptID=" + oItemRow.DepartmentID);
                        if (DeptRow.Length != 0)
                            podRow.DeptName = DeptRow[0][2].ToString();

                        DataRow[] SubDeptRow = dsSubDepartmentData.Tables[0].Select("SubDepartmentID=" + oItemRow.SubDepartmentID);

                        if (SubDeptRow.Length != 0)
                            podRow.SubDeptName = SubDeptRow[0][2].ToString();
                        //End
                        //Added by Amit Date 27 july 2011
                        podRow.RetailPrice = Configuration.convertNullToDecimal(podRowTemp["Price"].ToString());
                        podRow.ItemPrice = Configuration.convertNullToDecimal(podRowTemp["Price"].ToString()) - Configuration.convertNullToDecimal(podRowTemp["Discount"].ToString());
                        podRow.Discount = Configuration.convertNullToDecimal(podRowTemp["Discount"].ToString());
                        //End

                        //29 Nov 2011
                        DataRow[] InvRecDateRow = dsInvRecDate.Tables[0].Select(" ItemId='" + oItemRow.ItemID.Trim() + "'");
                        if (InvRecDateRow.Length != 0)
                            podRow.InvRecDate = (System.DateTime)InvRecDateRow[0][clsPOSDBConstants.PODetail_Fld_InvRecDate];
                        //End

                        dsPoDetails.PODetail.AddRow(podRow);

                        index++;
                        //}
                    }
                    //Added by Amit Date 17 Aug 2011
                    else if (!isFromPO && oItemRow.LastVendor.Trim().ToUpper() == VendCode.Trim().ToUpper())
                    {
                        VendorRow[] vendorRows = (VendorRow[])dsVendorData.Tables[0].Select("VendorCode='" + oItemRow.LastVendor.Replace("'", "''") + "'");
                        if (vendorRows != null && vendorRows.Length > 0)
                        {
                            podRow = dsPoDetails.PODetail.NewPODetailRow();
                            podRow.ItemID = oItemRow.ItemID;
                            podRow.ItemDescription = oItemRow.Description;
                            podRow.VendorID = vendorRows[0].VendorId;
                            podRow.VendorItemCode = "";
                            podRow.VendorName = vendorRows[0].Vendorname;
                            podRow.QTY = Configuration.convertNullToInt(podRowTemp["Qty"].ToString());
                            podRow.QtyInStock = oItemRow.QtyInStock;
                            podRow.ReOrderLevel = oItemRow.ReOrderLevel;
                            //Modified by shitaljit as per Fahad for those Pharmacy which are not using PO 
                            //We should pupulate item cost from Item table not from ItemVendor table.
                            //podRow.Price = 0;
                            podRow.Price = oItemRow.LastCostPrice;
                            podRow.SoldItems = Configuration.convertNullToInt(podRowTemp["QTY"].ToString());

                            podRow.PacketSize = oItemRow.PckSize;
                            podRow.PacketQuant = oItemRow.PckQty;
                            podRow.Packetunit = oItemRow.PckUnit;
                            podRow.MinOrdQty = oItemRow.MinOrdQty;

                            DataRow[] DeptRow = dsDepartmentData.Tables[0].Select("DeptID=" + oItemRow.DepartmentID);
                            if (DeptRow.Length != 0)
                                podRow.DeptName = DeptRow[0][2].ToString();

                            DataRow[] SubDeptRow = dsSubDepartmentData.Tables[0].Select("SubDepartmentID=" + oItemRow.SubDepartmentID);

                            if (SubDeptRow.Length != 0)
                                podRow.SubDeptName = SubDeptRow[0][2].ToString();

                            podRow.RetailPrice = Configuration.convertNullToDecimal(podRowTemp["Price"].ToString());
                            podRow.ItemPrice = Configuration.convertNullToDecimal(podRowTemp["Price"].ToString()) - Configuration.convertNullToDecimal(podRowTemp["Discount"].ToString());
                            podRow.Discount = Configuration.convertNullToDecimal(podRowTemp["Discount"].ToString());

                            //29 Nov 2011
                            DataRow[] InvRecDateRow = dsInvRecDate.Tables[0].Select(" ItemId='" + oItemRow.ItemID.Trim() + "'");
                            if (InvRecDateRow.Length != 0)
                                podRow.InvRecDate = (System.DateTime)InvRecDateRow[0][clsPOSDBConstants.PODetail_Fld_InvRecDate];
                            //End

                            dsPoDetails.PODetail.AddRow(podRow);

                            index++;
                        }//End 17 Aug

                    }
                }
                Application.DoEvents();
            }
            SetPacketToOrder(ref dsPoDetails);
            if (dsPoDetails.PODetail.Rows.Count > 0)
            {
                lblMessage.Text = dsPoDetails.PODetail.Rows.Count.ToString() + " No(s) Items Acquired For Purchase Order By " + ForWhichCat + " Vendor";
                Application.DoEvents();
            }
            return podTable;
        }
        private void SetPacketToOrder(ref PODetailData dsPODetails)
        {
            foreach (PODetailRow poDetRow in dsPODetails.PODetail.Rows)
            {
                Decimal iQtyToOrder = 0;
                Int32 ItemSold = 0;
                if (((poDetRow.Packetunit.ToUpper().Trim() == clsPOSDBConstants.PckUnit_CS) || (poDetRow.Packetunit.ToUpper().Trim() == clsPOSDBConstants.PckUnit_CA)) && poDetRow.MinOrdQty > (MMSUtil.UtilFunc.ValorZeroDEC(poDetRow.PacketSize.ToString()) * MMSUtil.UtilFunc.ValorZeroDEC(poDetRow.PacketQuant.ToString())))    //Sprint-21 22-Feb-2016 JY Added CA for case item
                {
                    if (MMSUtil.UtilFunc.ValorZeroDEC(poDetRow.PacketSize.ToString()) > 0)
                        iQtyToOrder = Decimal.Round(Decimal.Divide(MMSUtil.UtilFunc.ValorZeroDEC(poDetRow.QTY.ToString()), MMSUtil.UtilFunc.ValorZeroDEC(poDetRow.PacketSize.ToString())), 2);
                    else
                        iQtyToOrder = 1;
                    //ItemSold = Decimal.ToInt32(poDetRow.QTY);    //Commented By Amit Date 6 Dec 2011                
                    poDetRow.QTY = Decimal.ToInt32(Math.Ceiling(iQtyToOrder));
                }
                //poDetRow.SoldItems = Decimal.Round(Decimal.Divide(MMSUtil.UtilFunc.ValorZeroDEC(poDetRow.QTY.ToString()), MMSUtil.UtilFunc.ValorZeroDEC(poDetRow.PacketSize.ToString())), 2);
                //poDetRow.SoldItems = ItemSold;    //Commented By Amit Date 6 Dec 2011  
            }
        }

        private PODetailTable CreateAutoPOByLastVendor()
        {
            bool queryResult = false;
            PODetailTable podTable = null;
            DataSet dsTransHeader = new DataSet();
            DataSet dsTransDetailData = new DataSet();
            ItemVendorData dsItemVendor = new ItemVendorData();
            ItemData dsItems = new ItemData();
            TransHeaderSvr oPOSTrans = new TransHeaderSvr();
            TransDetailSvr oPOSTransDetail = new TransDetailSvr();
            ItemSvr oItem = new ItemSvr();
            ItemVendorSvr oItemVendor = new ItemVendorSvr();
            //Added by Amit Date 30 June 2011
            DepartmentData dsDepartmentData = new DepartmentData();
            DepartmentSvr oDepartment = new DepartmentSvr();
            SubDepartmentData dsSubDepartmentData = new SubDepartmentData();
            SubDepartmentSvr oSubDepartment = new SubDepartmentSvr();
            //End
            //Added By Amit Date 29 Nov 2011
            InvRecvHeaderSvr oInvRecHeader = new InvRecvHeaderSvr();
            DataSet dsInvRecDate = new DataSet();
            //End
            string SelectClause = string.Empty;
            if (optSearchSet.CheckedIndex == 0)
            {
                //changed by SRT(Abhishek)  Date : 21 Sept. 2009
                //Added condition to check if Transaction should be of Sell Type
                if (Configuration.CPrimeEDISetting.ConsiderReturnTrans)  //Sprint-27 - PRIMEPOS-2390 28-Sep-2017 JY Added    //PRIMEPOS-3167 07-Nov-2022 JY Modified 
                    SelectClause = " Trans.TransDate >= DateAdd(D,-" + numNumberOfDays.Value + ",getdate()) ";
                else
                    SelectClause = " Trans.TransDate >= DateAdd(D,-" + numNumberOfDays.Value + ",getdate()) AND Trans.TransType=1 ";
                //End Of changed by SRT(Abhishek)  Date : 21 Sept. 2009
            }
            else
            {
                //changed by SRT(Abhishek)  Date : 21 Sept. 2009
                //Added condition to check if Transaction should be of Sell Type

                //changed by SRT(Abhishek)  Date : 30 Sept. 2009
                if (Configuration.CPrimeEDISetting.ConsiderReturnTrans)  //Sprint-27 - PRIMEPOS-2390 28-Sep-2017 JY Added    //PRIMEPOS-3167 07-Nov-2022 JY Modified 
                    SelectClause = " Trans.TransDate between cast('" + DateTime.Parse(dtpFromDate.Value.ToString()).ToString() + "' as datetime) AND cast('" + DateTime.Parse(dtpToDate.Value.ToString()).ToString() + "' as datetime) ";
                else
                    SelectClause = " Trans.TransDate between cast('" + DateTime.Parse(dtpFromDate.Value.ToString()).ToString() + "' as datetime) AND cast('" + DateTime.Parse(dtpToDate.Value.ToString()).ToString() + "' as datetime) AND Trans.TransType=1 ";
                //End Of changed by SRT(Abhishek)  Date : 30 Sept. 2009

                //End Of changed by SRT(Abhishek)  Date : 21 Sept. 2009
            }
            lblMessage.Text = "Aquiring Transactions For Last Vendor.";
            Application.DoEvents();
            string strSQL = string.Empty;
            //dsTransHeader = oPOSTrans.PopulateList(SelectClause);   //PRIMEPOS-3097 28-Jun-2022 JY commented
            dsTransHeader = oPOSTrans.PopulateList(SelectClause, out strSQL);   //PRIMEPOS-3097 28-Jun-2022 JY Added
            if (dsTransHeader == null || dsTransHeader.Tables.Count == 0 || dsTransHeader.Tables[0].Rows.Count == 0)
            {
                throw (new Exception("There are no more transactions in this period."));
            }
            #region PRIMEPOS-3097 28-Jun-2022 JY Commented
            //List<string> lstData = new List<string>();
            //foreach (DataRow Dr in dsTransHeader.Tables[0].Rows)
            //{
            //    lstData.Add(Dr[0].ToString());
            //    Application.DoEvents();
            //}
            //dsTransDetailData = oPOSTransDetail.PopulateList(" where TransID in (" + string.Join(",", lstData.ToArray()) + ")", out queryResult);
            //lstData = new List<string>();
            //foreach (DataRow Dr in dsTransDetailData.Tables[0].Rows)
            //{
            //    lstData.Add("'" + Dr[0].ToString() + "'");
            //    Application.DoEvents();
            //}
            #endregion

            string strDistItems = string.Empty;
            dsTransDetailData = oPOSTransDetail.PopulateList(" where TransID in (" + strSQL + ")", out queryResult, out strDistItems);    //PRIMEPOS-3097 28-Jun-2022 JY Added
            lblMessage.Text = "Aquiring Transaction Items For Last Vendor.";
            Application.DoEvents();

            #region Sprint-19 - 1883 24-Mar-2015 JY Commented
            ////Start: Added By Amit Date 30 June 2011
            // List<string> lstDeptData = new List<string>();
            // List<string> lstSubDeptData = new List<string>();

            // if (this.txtDepartment.Text.Trim() != "" && txtDepartment.Tag.ToString() != ""
            //     && this.txtSubDepartment.Text.Trim() == "" && txtSubDepartment.Tag.ToString() == "")
            // {
            //     dsDepartmentData = oDepartment.PopulateList("where DeptCode in (" + this.txtDepartment.Tag.ToString().Trim() + ")");

            //     foreach (DataRow Dr in dsDepartmentData.Tables[0].Rows)
            //     {
            //         lstDeptData.Add("'" + Dr[0].ToString() + "'");
            //         Application.DoEvents();
            //     }
            //     dsSubDepartmentData = oSubDepartment.PopulateList("where DepartmentId in (" + string.Join(",", lstDeptData.ToArray()) + ")");

            //     dsItems = oItem.PopulateList(" where ItemId in (" + string.Join(",", lstData.ToArray()) + ") and DepartmentID in (" + string.Join(",", lstDeptData.ToArray()) + ")");
            // }
            // else if (this.txtDepartment.Text.Trim() == "" && txtDepartment.Tag.ToString() == ""
            //     && this.txtSubDepartment.Text.Trim() != "" && txtSubDepartment.Tag.ToString() != "")
            // {
            //     dsSubDepartmentData = oSubDepartment.PopulateList("where SubDepartmentId in (" + this.txtSubDepartment.Tag.ToString().Trim() + ")");
            //     foreach (DataRow Dr in dsSubDepartmentData.Tables[0].Rows)
            //     {
            //         lstSubDeptData.Add("'" + Dr[0].ToString() + "'");
            //         lstDeptData.Add("'" + Dr[1].ToString() + "'");
            //         Application.DoEvents();
            //     }
            //     dsDepartmentData = oDepartment.PopulateList("where DeptID in (" + string.Join(",", lstDeptData.ToArray()) + ")");

            //     dsItems = oItem.PopulateList(" where ItemId in (" + string.Join(",", lstData.ToArray()) + ") and SubDepartmentID in (" + string.Join(",", lstSubDeptData.ToArray()) + ")");
            // }
            // else if (this.txtDepartment.Text.Trim() != "" && txtDepartment.Tag.ToString() != ""
            //&& this.txtSubDepartment.Text.Trim() != "" && txtSubDepartment.Tag.ToString() != "")
            // {
            //     dsDepartmentData = oDepartment.PopulateList("where DeptCode in (" + this.txtDepartment.Tag.ToString().Trim() + ")");

            //     foreach (DataRow Dr in dsDepartmentData.Tables[0].Rows)
            //     {
            //         lstDeptData.Add("'" + Dr[0].ToString() + "'");
            //         Application.DoEvents();
            //     }

            //     dsSubDepartmentData = oSubDepartment.PopulateList("where SubDepartmentId in (" + this.txtSubDepartment.Tag.ToString().Trim() + ")");
            //     foreach (DataRow Dr in dsSubDepartmentData.Tables[0].Rows)
            //     {
            //         lstSubDeptData.Add("'" + Dr[0].ToString() + "'");
            //         Application.DoEvents();
            //     }

            //     dsItems = oItem.PopulateList(" where ItemId in (" + string.Join(",", lstData.ToArray()) + ") and DepartmentID in (" + string.Join(",", lstDeptData.ToArray()) + ") and SubDepartmentID in (" + string.Join(",", lstSubDeptData.ToArray()) + ")");

            // }
            // else
            // {
            //     dsDepartmentData = (DepartmentData)oDepartment.PopulateList(" where 1=1 ");
            //     dsSubDepartmentData = (SubDepartmentData)oSubDepartment.PopulateList(" where 1=1 ");

            //     dsItems = oItem.PopulateList(" where ItemId in (" + string.Join(",", lstData.ToArray()) + ")");
            // }
            // //End
            #endregion

            #region Sprint-19 - 1883 23-Mar-2015 JY Added location filter and also improved the existing filter code
            string strFilter = string.Empty;
            List<string> lstDeptData = new List<string>();
            List<string> lstSubDeptData = new List<string>();

            if (this.txtDepartment.Text.Trim() != "" && txtDepartment.Tag.ToString() != "")
            {
                dsDepartmentData = oDepartment.PopulateList("where DeptCode in (" + this.txtDepartment.Tag.ToString().Trim() + ")");

                foreach (DataRow Dr in dsDepartmentData.Tables[0].Rows)
                {
                    lstDeptData.Add("'" + Dr[0].ToString() + "'");
                }
                strFilter += " and DepartmentID in (" + string.Join(",", lstDeptData.ToArray()) + ")";

                if (this.txtSubDepartment.Text.Trim() != "" && txtSubDepartment.Tag.ToString() != "")
                {
                    dsSubDepartmentData = oSubDepartment.PopulateList("where SubDepartmentId in (" + this.txtSubDepartment.Tag.ToString().Trim() + ")");
                    foreach (DataRow Dr in dsSubDepartmentData.Tables[0].Rows)
                    {
                        lstSubDeptData.Add("'" + Dr[0].ToString() + "'");
                        lstDeptData.Add("'" + Dr[1].ToString() + "'");
                        Application.DoEvents();
                    }
                    strFilter += " and SubDepartmentID in (" + string.Join(",", lstSubDeptData.ToArray()) + ")";
                }
                else
                {
                    dsSubDepartmentData = oSubDepartment.PopulateList("where DepartmentId in (" + string.Join(",", lstDeptData.ToArray()) + ")");
                }
            }
            else
            {
                if (this.txtSubDepartment.Text.Trim() != "" && txtSubDepartment.Tag.ToString() != "")
                {
                    dsSubDepartmentData = oSubDepartment.PopulateList("where SubDepartmentId in (" + this.txtSubDepartment.Tag.ToString().Trim() + ")");
                    foreach (DataRow Dr in dsSubDepartmentData.Tables[0].Rows)
                    {
                        lstSubDeptData.Add("'" + Dr[0].ToString() + "'");
                        lstDeptData.Add("'" + Dr[1].ToString() + "'");
                        Application.DoEvents();
                    }
                    strFilter += " and SubDepartmentID in (" + string.Join(",", lstSubDeptData.ToArray()) + ")";

                    dsDepartmentData = oDepartment.PopulateList("where DeptID in (" + string.Join(",", lstDeptData.ToArray()) + ")");
                }
                else
                {
                    dsDepartmentData = (DepartmentData)oDepartment.PopulateList(" where 1=1 ");
                    dsSubDepartmentData = (SubDepartmentData)oSubDepartment.PopulateList(" where 1=1 ");
                }
            }

            if (this.txtItemLocation.Text.Trim() != "" && txtItemLocation.Tag.ToString() != "")
            {
                strFilter += " and LOCATION in (" + txtItemLocation.Text + ")";
            }
            //dsItems = oItem.PopulateList(" where ItemId in (" + string.Join(",", lstData.ToArray()) + ") " + strFilter);  //PRIMEPOS-3097 28-Jun-2022 JY Commented
            dsItems = oItem.PopulateList(" where ItemId in (" + strDistItems + ") " + strFilter);   //PRIMEPOS-3097 28-Jun-2022 JY Added
            #endregion

            //dsItemVendor = oItemVendor.PopulateList(" where ItemId in (" + string.Join(",", lstData.ToArray()) + ")");    //PRIMEPOS-3097 28-Jun-2022 JY Commented
            dsItemVendor = oItemVendor.PopulateList(" where ItemId in (" + strDistItems + ")", true);  //PRIMEPOS-3097 28-Jun-2022 JY Added
            if (dsItems == null || dsItems.Tables.Count == 0 || dsItems.Tables[0].Rows.Count == 0)
            {
                throw (new Exception("There are no more items matching transactions of selected period."));
            }

            if (dsItemVendor == null || dsItemVendor.Tables.Count == 0 || dsItemVendor.Tables[0].Rows.Count == 0)
            {
                throw (new Exception("There are no more vendor items matching transactions of selected period."));
            }

            //Added By Amit Date 29 Nov 2011
            //dsInvRecDate = oInvRecHeader.ItemInvRecDate(lstData); //PRIMEPOS-3097 28-Jun-2022 JY Commented
            dsInvRecDate = oInvRecHeader.ItemInvRecDate(strDistItems);   //PRIMEPOS-3097 28-Jun-2022 JY Added
            //End

            int count = 0;
            podTable = new PODetailTable();
            PODetailRow podRow = null;
            //Added By Amit Date 30 June 2011
            int index = 0;

            //foreach (ItemRow oItemRow in dsItems.Tables[0].Rows)
            //Added By Amit date 29 July 2011            
            foreach (DataRow podRowTemp in dsTransDetailData.Tables[0].Rows)
            {
                DataRow[] oItemRowTemp = dsItems.Tables[0].Select("ItemID='" + podRowTemp["ItemID"] + "'");
                ItemRow oItemRow = null;

                if (oItemRowTemp.Length > 0)
                    oItemRow = (ItemRow)oItemRowTemp[0];
                else
                    continue;

                lblMessage.Text = "Scanning Item #" + oItemRow.ItemID.Trim() + " For Last Vendor";
                Application.DoEvents();
                int TEMPVEND = 0;
                //if (Configuration.convertNullToString(oItemRow.LastVendor).Trim().Length > 0 && !oItemRow.ExclFromAutoPO && int.TryParse(oItemRow.LastVendor.ToString(), out TEMPVEND))//isnull(itm.ExclFromAutoPO,0) = 0
                if (Configuration.convertNullToString(oItemRow.LastVendor).Trim().Length > 0 && !oItemRow.ExclFromAutoPO && oItemRow.LastVendor.ToString().Trim().Length > 0)
                {
                    DataRow[] oItemRows = dsItemVendor.Tables[0].Select(" ItemId='" + oItemRow.ItemID.Trim() + "' AND VENDORCODE= '" + oItemRow.LastVendor.ToString().Trim().Replace("'", "''") + "'");
                    if (oItemRows != null && oItemRows.Length > 0)
                    {
                        ItemVendorRow oItemVendorRow = (ItemVendorRow)oItemRows[0];
                        DataRow[] TransRows = dsTransDetailData.Tables[0].Select(" ItemId='" + oItemRow.ItemID.Trim() + "'");
                        if (TransRows != null && TransRows.Length > 0)
                        {
                            //decimal qty = 0;
                            //foreach (DataRow dr in TransRows)
                            //{
                            //    qty += Configuration.convertNullToDecimal(dr["Qty"].ToString());
                            //}
                            //count++;
                            podRow = dsPoDetails.PODetail.NewPODetailRow();
                            podRow.ItemID = oItemRow.ItemID;
                            podRow.ItemDescription = oItemRow.Description;
                            podRow.VendorID = oItemVendorRow.VendorID;
                            podRow.VendorItemCode = oItemVendorRow.VendorItemID;
                            podRow.VendorName = oItemVendorRow.VendorCode;
                            podRow.QTY = Configuration.convertNullToInt(podRowTemp["QTY"].ToString());
                            podRow.QtyInStock = oItemRow.QtyInStock;
                            podRow.ReOrderLevel = oItemRow.ReOrderLevel;
                            //Modified by shitaljit as per Fahad for those Pharmacy which are not using PO 
                            //We should populate item cost from Item table not from ItemVendor table.
                            podRow.SoldItems = Configuration.convertNullToInt(podRowTemp["QTY"].ToString());
                            if (isFromPO == true)
                            {
                                podRow.Price = oItemVendorRow.VenorCostPrice;
                            }
                            else
                            {
                                podRow.Price = oItemRow.LastCostPrice;
                            }

                            podRow.PacketSize = oItemRow.PckSize;
                            podRow.PacketQuant = oItemRow.PckQty;
                            podRow.Packetunit = oItemRow.PckUnit;

                            //Start : Added By Amit Date 5 jul 2011
                            podRow.MinOrdQty = oItemRow.MinOrdQty;
                            DataRow[] DeptRow = dsDepartmentData.Tables[0].Select("DeptID=" + oItemRow.DepartmentID);
                            if (DeptRow.Length != 0)
                                podRow.DeptName = DeptRow[0][2].ToString();

                            DataRow[] SubDeptRow = dsSubDepartmentData.Tables[0].Select("SubDepartmentID=" + oItemRow.SubDepartmentID);

                            if (SubDeptRow.Length != 0)
                                podRow.SubDeptName = SubDeptRow[0][2].ToString();
                            //End
                            //Added by Amit Date 27 july 2011
                            podRow.RetailPrice = Configuration.convertNullToDecimal(podRowTemp["Price"].ToString());
                            podRow.ItemPrice = Configuration.convertNullToDecimal(podRowTemp["Price"].ToString()) - Configuration.convertNullToDecimal(podRowTemp["Discount"].ToString());
                            podRow.Discount = Configuration.convertNullToDecimal(podRowTemp["Discount"].ToString());
                            //End
                            //Added by Amit Date 29 Nov 2011
                            DataRow[] InvRecDateRow = dsInvRecDate.Tables[0].Select(" ItemId='" + oItemRow.ItemID.Trim() + "'");
                            if (InvRecDateRow.Length != 0)
                                podRow.InvRecDate = (System.DateTime)InvRecDateRow[0][clsPOSDBConstants.PODetail_Fld_InvRecDate];
                            //End

                            dsPoDetails.PODetail.AddRow(podRow);

                            index++;

                        }
                    }
                }
                Application.DoEvents();
            }
            SetPacketToOrder(ref dsPoDetails);
            if (dsPoDetails.PODetail.Rows.Count > 0)
            {
                lblMessage.Text = podTable.Rows.Count.ToString() + " No(s) Items Acquired For Purchase Order By Last Vendor";
                Application.DoEvents();
            }
            return podTable;
        }
        private PODetailTable CreateAutoPOByItemOwner()
        {
            bool queryResult = false;
            PODetailTable podTable = null;
            DataSet dsTransHeader = new DataSet();
            DataSet dsTransDetailData = new DataSet();
            ItemVendorData dsItemVendor = new ItemVendorData();
            ItemData dsItems = new ItemData();
            TransHeaderSvr oPOSTrans = new TransHeaderSvr();
            TransDetailSvr oPOSTransDetail = new TransDetailSvr();
            ItemSvr oItem = new ItemSvr();
            ItemVendorSvr oItemVendor = new ItemVendorSvr();
            //Added by Amit Date 30 June 2011
            DepartmentData dsDepartmentData = new DepartmentData();
            DepartmentSvr oDepartment = new DepartmentSvr();
            SubDepartmentData dsSubDepartmentData = new SubDepartmentData();
            SubDepartmentSvr oSubDepartment = new SubDepartmentSvr();
            //End
            //Added By Amit Date 29 Nov 2011
            InvRecvHeaderSvr oInvRecHeader = new InvRecvHeaderSvr();
            DataSet dsInvRecDate = new DataSet();
            //End

            string SelectClause = string.Empty;
            if (optSearchSet.CheckedIndex == 0)
            {
                //changed by SRT(Abhishek)  Date : 21 Sept. 2009
                //Added condition to check if Transaction should be of Sell Type
                if (Configuration.CPrimeEDISetting.ConsiderReturnTrans)  //Sprint-27 - PRIMEPOS-2390 28-Sep-2017 JY Added    //PRIMEPOS-3167 07-Nov-2022 JY Modified 
                    SelectClause = " Trans.TransDate >= DateAdd(D,-" + numNumberOfDays.Value + ",getdate()) ";
                else
                    SelectClause = " Trans.TransDate >= DateAdd(D,-" + numNumberOfDays.Value + ",getdate()) AND Trans.TransType=1 ";
                //End Of changed by SRT(Abhishek)  Date : 21 Sept. 2009
            }
            else
            {
                //changed by SRT(Abhishek)  Date : 21 Sept. 2009
                //Added condition to check if Transaction should be of Sell Type

                //changed by SRT(Abhishek)  Date : 30 Sept. 2009
                if (Configuration.CPrimeEDISetting.ConsiderReturnTrans)  //Sprint-27 - PRIMEPOS-2390 28-Sep-2017 JY Added    //PRIMEPOS-3167 07-Nov-2022 JY Modified 
                    SelectClause = " Trans.TransDate between cast('" + DateTime.Parse(dtpFromDate.Value.ToString()).ToString() + "' as datetime) AND cast('" + DateTime.Parse(dtpToDate.Value.ToString()).ToString() + "' as datetime) ";
                else
                    SelectClause = " Trans.TransDate between cast('" + DateTime.Parse(dtpFromDate.Value.ToString()).ToString() + "' as datetime) AND cast('" + DateTime.Parse(dtpToDate.Value.ToString()).ToString() + "' as datetime) AND Trans.TransType=1 ";
                //End Of changed by SRT(Abhishek)  Date : 30 Sept. 2009

                //End Of changed by SRT(Abhishek)  Date : 21 Sept. 2009
            }
            lblMessage.Text = "Aquiring Transactions For Last Vendor.";
            Application.DoEvents();
            string strSQL = string.Empty;
            //dsTransHeader = oPOSTrans.PopulateList(SelectClause);   //PRIMEPOS-3097 28-Jun-2022 JY commented
            dsTransHeader = oPOSTrans.PopulateList(SelectClause, out strSQL);   //PRIMEPOS-3097 28-Jun-2022 JY Added
            if (dsTransHeader == null || dsTransHeader.Tables.Count == 0 || dsTransHeader.Tables[0].Rows.Count == 0)
            {
                throw (new Exception("There are no more transactions in this period."));
            }
            #region PRIMEPOS-3097 28-Jun-2022 JY Commented
            //List<string> lstData = new List<string>();
            //foreach (DataRow Dr in dsTransHeader.Tables[0].Rows)
            //{
            //    lstData.Add(Dr[0].ToString());
            //    Application.DoEvents();
            //}
            //dsTransDetailData = oPOSTransDetail.PopulateList(" where TransID in (" + string.Join(",", lstData.ToArray()) + ")", out queryResult);
            //lstData = new List<string>();
            //foreach (DataRow Dr in dsTransDetailData.Tables[0].Rows)
            //{
            //    lstData.Add("'" + Dr[0].ToString() + "'");
            //    Application.DoEvents();
            //}
            #endregion

            string strDistItems = string.Empty;
            dsTransDetailData = oPOSTransDetail.PopulateList(" where TransID in (" + strSQL + ")", out queryResult, out strDistItems);    //PRIMEPOS-3097 28-Jun-2022 JY Added
            lblMessage.Text = "Aquiring Transaction Items For Last Vendor.";
            Application.DoEvents();

            #region Sprint-19 - 1883 24-Mar-2015 JY Commented
            // //Start: Added By Amit Date 30 June 2011
            // List<string> lstDeptData = new List<string>();
            // List<string> lstSubDeptData = new List<string>();

            // if (this.txtDepartment.Text.Trim() != "" && txtDepartment.Tag.ToString() != ""
            //     && this.txtSubDepartment.Text.Trim() == "" && txtSubDepartment.Tag.ToString() == "")
            // {
            //     dsDepartmentData = oDepartment.PopulateList("where DeptCode in (" + this.txtDepartment.Tag.ToString().Trim() + ")");

            //     foreach (DataRow Dr in dsDepartmentData.Tables[0].Rows)
            //     {
            //         lstDeptData.Add("'" + Dr[0].ToString() + "'");
            //         Application.DoEvents();
            //     }
            //     dsSubDepartmentData = oSubDepartment.PopulateList("where DepartmentId in (" + string.Join(",", lstDeptData.ToArray()) + ")");

            //     dsItems = oItem.PopulateList(" where ItemId in (" + string.Join(",", lstData.ToArray()) + ") and DepartmentID in (" + string.Join(",", lstDeptData.ToArray()) + ")");
            // }
            // else if (this.txtDepartment.Text.Trim() == "" && txtDepartment.Tag.ToString() == ""
            //     && this.txtSubDepartment.Text.Trim() != "" && txtSubDepartment.Tag.ToString() != "")
            // {
            //     dsSubDepartmentData = oSubDepartment.PopulateList("where SubDepartmentId in (" + this.txtSubDepartment.Tag.ToString().Trim() + ")");
            //     foreach (DataRow Dr in dsSubDepartmentData.Tables[0].Rows)
            //     {
            //         lstSubDeptData.Add("'" + Dr[0].ToString() + "'");
            //         lstDeptData.Add("'" + Dr[1].ToString() + "'");
            //         Application.DoEvents();
            //     }
            //     dsDepartmentData = oDepartment.PopulateList("where DeptID in (" + string.Join(",", lstDeptData.ToArray()) + ")");

            //     dsItems = oItem.PopulateList(" where ItemId in (" + string.Join(",", lstData.ToArray()) + ") and SubDepartmentID in (" + string.Join(",", lstSubDeptData.ToArray()) + ")");
            // }
            // else if (this.txtDepartment.Text.Trim() != "" && txtDepartment.Tag.ToString() != ""
            //&& this.txtSubDepartment.Text.Trim() != "" && txtSubDepartment.Tag.ToString() != "")
            // {
            //     dsDepartmentData = oDepartment.PopulateList("where DeptCode in (" + this.txtDepartment.Tag.ToString().Trim() + ")");

            //     foreach (DataRow Dr in dsDepartmentData.Tables[0].Rows)
            //     {
            //         lstDeptData.Add("'" + Dr[0].ToString() + "'");
            //         Application.DoEvents();
            //     }

            //     dsSubDepartmentData = oSubDepartment.PopulateList("where SubDepartmentId in (" + this.txtSubDepartment.Tag.ToString().Trim() + ")");
            //     foreach (DataRow Dr in dsSubDepartmentData.Tables[0].Rows)
            //     {
            //         lstSubDeptData.Add("'" + Dr[0].ToString() + "'");
            //         Application.DoEvents();
            //     }

            //     dsItems = oItem.PopulateList(" where ItemId in (" + string.Join(",", lstData.ToArray()) + ") and DepartmentID in (" + string.Join(",", lstDeptData.ToArray()) + ") and SubDepartmentID in (" + string.Join(",", lstSubDeptData.ToArray()) + ")");

            // }
            // else
            // {
            //     dsDepartmentData = (DepartmentData)oDepartment.PopulateList(" where 1=1 ");
            //     dsSubDepartmentData = (SubDepartmentData)oSubDepartment.PopulateList(" where 1=1 ");

            //     dsItems = oItem.PopulateList(" where ItemId in (" + string.Join(",", lstData.ToArray()) + ")");
            // }
            // //End
            #endregion

            #region Sprint-19 - 1883 23-Mar-2015 JY Added location filter and also improved the existing filter code
            string strFilter = string.Empty;
            List<string> lstDeptData = new List<string>();
            List<string> lstSubDeptData = new List<string>();

            if (this.txtDepartment.Text.Trim() != "" && txtDepartment.Tag.ToString() != "")
            {
                dsDepartmentData = oDepartment.PopulateList("where DeptCode in (" + this.txtDepartment.Tag.ToString().Trim() + ")");

                foreach (DataRow Dr in dsDepartmentData.Tables[0].Rows)
                {
                    lstDeptData.Add("'" + Dr[0].ToString() + "'");
                }
                strFilter += " and DepartmentID in (" + string.Join(",", lstDeptData.ToArray()) + ")";

                if (this.txtSubDepartment.Text.Trim() != "" && txtSubDepartment.Tag.ToString() != "")
                {
                    dsSubDepartmentData = oSubDepartment.PopulateList("where SubDepartmentId in (" + this.txtSubDepartment.Tag.ToString().Trim() + ")");
                    foreach (DataRow Dr in dsSubDepartmentData.Tables[0].Rows)
                    {
                        lstSubDeptData.Add("'" + Dr[0].ToString() + "'");
                        lstDeptData.Add("'" + Dr[1].ToString() + "'");
                        Application.DoEvents();
                    }
                    strFilter += " and SubDepartmentID in (" + string.Join(",", lstSubDeptData.ToArray()) + ")";
                }
                else
                {
                    dsSubDepartmentData = oSubDepartment.PopulateList("where DepartmentId in (" + string.Join(",", lstDeptData.ToArray()) + ")");
                }
            }
            else
            {
                if (this.txtSubDepartment.Text.Trim() != "" && txtSubDepartment.Tag.ToString() != "")
                {
                    dsSubDepartmentData = oSubDepartment.PopulateList("where SubDepartmentId in (" + this.txtSubDepartment.Tag.ToString().Trim() + ")");
                    foreach (DataRow Dr in dsSubDepartmentData.Tables[0].Rows)
                    {
                        lstSubDeptData.Add("'" + Dr[0].ToString() + "'");
                        lstDeptData.Add("'" + Dr[1].ToString() + "'");
                        Application.DoEvents();
                    }
                    strFilter += " and SubDepartmentID in (" + string.Join(",", lstSubDeptData.ToArray()) + ")";

                    dsDepartmentData = oDepartment.PopulateList("where DeptID in (" + string.Join(",", lstDeptData.ToArray()) + ")");
                }
                else
                {
                    dsDepartmentData = (DepartmentData)oDepartment.PopulateList(" where 1=1 ");
                    dsSubDepartmentData = (SubDepartmentData)oSubDepartment.PopulateList(" where 1=1 ");
                }
            }

            if (this.txtItemLocation.Text.Trim() != "" && txtItemLocation.Tag.ToString() != "")
            {
                strFilter += " and LOCATION in (" + txtItemLocation.Text + ")";
            }
            //dsItems = oItem.PopulateList(" where ItemId in (" + string.Join(",", lstData.ToArray()) + ") " + strFilter);  //PRIMEPOS-3097 28-Jun-2022 JY Commented
            dsItems = oItem.PopulateList(" where ItemId in (" + strDistItems + ") " + strFilter);   //PRIMEPOS-3097 28-Jun-2022 JY Added
            #endregion

            //dsItemVendor = oItemVendor.PopulateList(" where ItemId in (" + string.Join(",", lstData.ToArray()) + ")");    //PRIMEPOS-3097 28-Jun-2022 JY Commented
            dsItemVendor = oItemVendor.PopulateList(" where ItemId in (" + strDistItems + ")", true);  //PRIMEPOS-3097 28-Jun-2022 JY Added
            if (dsItems == null || dsItems.Tables.Count == 0 || dsItems.Tables[0].Rows.Count == 0)
            {
                throw (new Exception("There are no more items matching transactions of selected period."));
            }

            if (dsItemVendor == null || dsItemVendor.Tables.Count == 0 || dsItemVendor.Tables[0].Rows.Count == 0)
            {
                throw (new Exception("There are no more vendor items matching transactions of selected period."));
            }
            //Added By Amit Date 29 Nov 2011
            //dsInvRecDate = oInvRecHeader.ItemInvRecDate(lstData); //PRIMEPOS-3097 28-Jun-2022 JY Commented
            dsInvRecDate = oInvRecHeader.ItemInvRecDate(strDistItems);   //PRIMEPOS-3097 28-Jun-2022 JY Added
            //End

            int count = 0;
            podTable = new PODetailTable();
            PODetailRow podRow = null;
            //Added By Amit Date 30 June 2011
            int index = 0;

            //foreach (ItemRow oItemRow in dsItems.Tables[0].Rows)
            //Added By Amit date 29 July 2011            
            foreach (DataRow podRowTemp in dsTransDetailData.Tables[0].Rows)
            {
                DataRow[] oItemRowTemp = dsItems.Tables[0].Select("ItemID='" + podRowTemp["ItemID"] + "'");
                ItemRow oItemRow = null;

                if (oItemRowTemp.Length > 0)
                    oItemRow = (ItemRow)oItemRowTemp[0];
                else
                    continue;

                lblMessage.Text = "Scanning Item #" + oItemRow.ItemID.Trim() + " For Owner Vendor";
                Application.DoEvents();
                int TEMPVEND = 0;
                //if (Configuration.convertNullToString(oItemRow.LastVendor).Trim().Length > 0 && !oItemRow.ExclFromAutoPO && int.TryParse(oItemRow.LastVendor.ToString(), out TEMPVEND))//isnull(itm.ExclFromAutoPO,0) = 0
                if (!oItemRow.ExclFromAutoPO)
                {
                    string prefvendor = oItemRow.PreferredVendor.ToString();
                    string lastvendor = oItemRow.LastVendor.ToString();
                    string defaultvendor = Configuration.CPrimeEDISetting.DefaultVendor;    //PRIMEPOS-3167 07-Nov-2022 JY Modified

                    string selectQuery = string.Empty;

                    selectQuery = " ItemId='" + oItemRow.ItemID.Trim() + "'";

                    if (oItemRow.LastVendor.Trim().Length > 0)
                    {
                        //if (int.TryParse(oItemRow.LastVendor.ToString(), out TEMPVEND))
                        {
                            if (oItemRow.PreferredVendor != string.Empty)
                                selectQuery = selectQuery + " And ( vendorCode not in ('" + oItemRow.LastVendor.ToString().Trim().Replace("'", "''") + "','" + oItemRow.PreferredVendor.Replace("'", "''") + "') OR vendorcode <> '" + Configuration.CPrimeEDISetting.DefaultVendor.Replace("'", "''") + "')";  //Sprint-27 - 10-Oct-2017 JY to resolve issue, replaced single quote with double quote  //PRIMEPOS-3167 07-Nov-2022 JY Modified
                            else
                                selectQuery = selectQuery + " And ( vendorCode not in ('" + oItemRow.LastVendor.ToString().Trim().Replace("'", "''") + "') OR vendorcode <> '" + Configuration.CPrimeEDISetting.DefaultVendor.Replace("'", "''") + "')";   //Sprint-27 - 10-Oct-2017 JY to resolve issue, replaced single quote with double quote   //PRIMEPOS-3167 07-Nov-2022 JY Modified
                        }
                        //else
                        //{
                        //    if (oItemRow.PreferredVendor != string.Empty)
                        //        selectQuery = selectQuery + " And ( vendorID not in ('" + oItemRow.PreferredVendor + "') OR vendorcode <> '" + Configuration.CPOSSet.DefaultVendor + "')";
                        //    else
                        //        selectQuery = selectQuery + " And ( vendorcode <> '" + Configuration.CPOSSet.DefaultVendor + "')";
                        //}
                    }
                    else
                    {
                        if (oItemRow.PreferredVendor != string.Empty)
                            selectQuery = selectQuery + " And ( vendorCode not in ('" + oItemRow.PreferredVendor.Replace("'", "''") + "') OR vendorcode <> '" + Configuration.CPrimeEDISetting.DefaultVendor.Replace("'", "''") + "')";    //Sprint-27 - 10-Oct-2017 JY to resolve issue, replaced single quote with double quote   //PRIMEPOS-3167 07-Nov-2022 JY Modified
                        else
                            selectQuery = selectQuery + " And ( vendorcode <> '" + Configuration.CPrimeEDISetting.DefaultVendor.Replace("'", "''") + "')";  //Sprint-27 - 10-Oct-2017 JY to resolve issue, replaced single quote with double quote  //PRIMEPOS-3167 07-Nov-2022 JY Modified

                    }
                    //if (oItemRow.PreferredVendor != string.Empty)
                    //    selectQuery = " ItemId='" + oItemRow.ItemID.Trim() + "' And ( vendorID not in ('" + TEMPVEND + "','" + oItemRow.PreferredVendor + "') OR vendorcode <> '" + Configuration.CPOSSet.DefaultVendor + "')";
                    //else
                    //    selectQuery = " ItemId = '" + oItemRow.ItemID.Trim() + "' And ( vendorID not in ('" + TEMPVEND + "') OR vendorcode <> '" + Configuration.CPOSSet.DefaultVendor + "')";
                    DataRow[] oItemRows = dsItemVendor.Tables[0].Select(selectQuery);
                    if (oItemRows != null && oItemRows.Length > 0)
                    {
                        ItemVendorRow oItemVendorRow = (ItemVendorRow)oItemRows[0];
                        DataRow[] TransRows = dsTransDetailData.Tables[0].Select(" ItemId='" + oItemRow.ItemID.Trim() + "'");
                        if (TransRows != null && TransRows.Length > 0 && oItemRows.Length == 1)
                        {
                            //decimal qty = 0;
                            //foreach (DataRow dr in TransRows)
                            //{
                            //    qty += Configuration.convertNullToDecimal(dr["Qty"].ToString());
                            //}
                            //count++;
                            podRow = dsPoDetails.PODetail.NewPODetailRow();
                            podRow.ItemID = oItemRow.ItemID;
                            podRow.ItemDescription = oItemRow.Description;
                            podRow.VendorID = oItemVendorRow.VendorID;
                            podRow.VendorItemCode = oItemVendorRow.VendorItemID;
                            podRow.VendorName = oItemVendorRow.VendorCode;
                            podRow.QTY = Configuration.convertNullToInt(podRowTemp["QTY"].ToString());
                            podRow.QtyInStock = oItemRow.QtyInStock;
                            podRow.ReOrderLevel = oItemRow.ReOrderLevel;
                            //Modified by shitaljit as per Fahad for those Pharmacy which are not using PO 
                            //We should populate item cost from Item table not from ItemVendor table.
                            podRow.SoldItems = Configuration.convertNullToInt(podRowTemp["QTY"].ToString());
                            if (isFromPO == true)
                            {
                                podRow.Price = oItemVendorRow.VenorCostPrice;
                            }
                            else
                            {
                                podRow.Price = oItemRow.LastCostPrice;
                            }

                            podRow.PacketSize = oItemRow.PckSize;
                            podRow.PacketQuant = oItemRow.PckQty;
                            podRow.Packetunit = oItemRow.PckUnit;
                            //Start : Added By Amit Date 5 jul 2011
                            podRow.MinOrdQty = oItemRow.MinOrdQty;
                            DataRow[] DeptRow = dsDepartmentData.Tables[0].Select("DeptID=" + oItemRow.DepartmentID);
                            if (DeptRow.Length != 0)
                                podRow.DeptName = DeptRow[0][2].ToString();

                            DataRow[] SubDeptRow = dsSubDepartmentData.Tables[0].Select("SubDepartmentID=" + oItemRow.SubDepartmentID);

                            if (SubDeptRow.Length != 0)
                                podRow.SubDeptName = SubDeptRow[0][2].ToString();
                            //End
                            //Added by Amit Date 27 july 2011
                            podRow.RetailPrice = Configuration.convertNullToDecimal(podRowTemp["Price"].ToString());
                            podRow.ItemPrice = Configuration.convertNullToDecimal(podRowTemp["Price"].ToString()) - Configuration.convertNullToDecimal(podRowTemp["Discount"].ToString());
                            podRow.Discount = Configuration.convertNullToDecimal(podRowTemp["Discount"].ToString());
                            //End
                            //29 Nov 2011
                            DataRow[] InvRecDateRow = dsInvRecDate.Tables[0].Select(" ItemId='" + oItemRow.ItemID.Trim() + "'");
                            if (InvRecDateRow.Length != 0)
                                podRow.InvRecDate = (System.DateTime)InvRecDateRow[0][clsPOSDBConstants.PODetail_Fld_InvRecDate];
                            //End

                            dsPoDetails.PODetail.AddRow(podRow);

                            index++;

                        }
                    }
                }
                Application.DoEvents();
            }
            SetPacketToOrder(ref dsPoDetails);
            if (dsPoDetails.PODetail.Rows.Count > 0)
            {
                lblMessage.Text = podTable.Rows.Count.ToString() + " No(s) Items Acquired For Purchase Order By Last Vendor";
                Application.DoEvents();
            }
            return podTable;
        }
        private PODetailTable CreateAutoPOByPreferedVendorReorder()

        {
            bool queryResult = false;

            PODetailTable podTable = null;

            DataSet dsTransHeader = new DataSet();
            DataSet dsTransDetail = new DataSet();
            ItemData dsItems = new ItemData();
            ItemVendorData dsItemVendor = new ItemVendorData();
            DepartmentData dsDepartment = new DepartmentData();
            SubDepartmentData dsSubDepartment = new SubDepartmentData();
            DataSet dsInvRecDate = new DataSet();

            GetDsForReorderLevel("Prefered Vendor", txtDepartment.Tag.ToString().Trim(), txtSubDepartment.Tag.ToString().Trim(), out dsTransHeader, out dsTransDetail, out dsItems, out dsItemVendor, out dsDepartment, out dsSubDepartment, out dsInvRecDate, out queryResult);

            #region commented

            // DataSet dsTransHeader = new DataSet();
            // DataSet dsTransDetailData = new DataSet();

            // ItemVendorData dsItemVendor = new ItemVendorData();
            // ItemData dsItems = new ItemData();

            // TransHeaderSvr oPOSTrans = new TransHeaderSvr();
            // TransDetailSvr oPOSTransDetail = new TransDetailSvr();

            // ItemSvr oItem = new ItemSvr();
            // ItemVendorSvr oItemVendor = new ItemVendorSvr();

            // //Added by Amit Date 30 June 2011
            // DepartmentData dsDepartmentData = new DepartmentData();
            // DepartmentSvr oDepartment = new DepartmentSvr();
            // SubDepartmentData dsSubDepartmentData = new SubDepartmentData();
            // SubDepartmentSvr oSubDepartment = new SubDepartmentSvr();
            // //End

            // string SelectClause = string.Empty;

            // //changed by SRT(Abhishek)  Date : 21 Sept. 2009
            // //Added condition to check if Transaction should be of Sell Type 
            // SelectClause = " Trans.TransDate >= DateAdd(D,-" + clsPOSDBConstants.ItemReorderPeriod.ToString() + ",getdate()) AND Trans.TransType=1 ";
            // //End of changed by SRT(Abhishek)  Date : 21 Sept. 2009

            // lblMessage.Text = "Aquiring Transactions For Prefered Vendor.";
            // Application.DoEvents();

            // dsTransHeader = oPOSTrans.PopulateList(SelectClause);

            // if (dsTransHeader == null || dsTransHeader.Tables.Count == 0 || dsTransHeader.Tables[0].Rows.Count == 0)
            // {
            //     throw (new Exception("There are no more transactions in this last " + clsPOSDBConstants.ItemReorderPeriod.ToString() + " day(s)."));
            // }

            // List<string> lstData = new List<string>();
            // foreach (DataRow Dr in dsTransHeader.Tables[0].Rows)
            // {
            //     lstData.Add(Dr[0].ToString());
            // }

            // dsTransDetailData = oPOSTransDetail.PopulateList(" where TransID in (" + string.Join(",", lstData.ToArray()) + ")", out queryResult);

            // lstData = new List<string>();
            // foreach (DataRow Dr in dsTransDetailData.Tables[0].Rows)
            // {
            //     lstData.Add("'" + Dr[0].ToString() + "'");
            // }
            // lblMessage.Text = "Aquiring Transaction Items For Prefered Vendor.";
            // Application.DoEvents();
            // //Start: Added By Amit Date 30 June 2011
            // List<string> lstDeptData = new List<string>();
            // List<string> lstSubDeptData = new List<string>();

            // if (this.txtDepartment.Text.Trim() != "" && txtDepartment.Tag.ToString() != ""
            //     && this.txtSubDepartment.Text.Trim() == "" && txtSubDepartment.Tag.ToString() == "")
            // {
            //     dsDepartmentData = oDepartment.PopulateList("where DeptCode in (" + this.txtDepartment.Tag.ToString().Trim() + ")");

            //     foreach (DataRow Dr in dsDepartmentData.Tables[0].Rows)
            //     {
            //         lstDeptData.Add("'" + Dr[0].ToString() + "'");
            //         Application.DoEvents();
            //     }
            //     dsSubDepartmentData = oSubDepartment.PopulateList("where DepartmentId in (" + string.Join(",", lstDeptData.ToArray()) + ")");

            //     dsItems = oItem.PopulateList(" where ItemId in (" + string.Join(",", lstData.ToArray()) + ") and DepartmentID in (" + string.Join(",", lstDeptData.ToArray()) + ")");
            // }
            // else if (this.txtDepartment.Text.Trim() == "" && txtDepartment.Tag.ToString() == ""
            //     && this.txtSubDepartment.Text.Trim() != "" && txtSubDepartment.Tag.ToString() != "")
            // {
            //     dsSubDepartmentData = oSubDepartment.PopulateList("where SubDepartmentId in (" + this.txtSubDepartment.Tag.ToString().Trim() + ")");
            //     foreach (DataRow Dr in dsSubDepartmentData.Tables[0].Rows)
            //     {
            //         lstSubDeptData.Add("'" + Dr[0].ToString() + "'");
            //         lstDeptData.Add("'" + Dr[1].ToString() + "'");
            //         Application.DoEvents();
            //     }
            //     dsDepartmentData = oDepartment.PopulateList("where DeptID in (" + string.Join(",", lstDeptData.ToArray()) + ")");

            //     dsItems = oItem.PopulateList(" where ItemId in (" + string.Join(",", lstData.ToArray()) + ") and SubDepartmentID in (" + string.Join(",", lstSubDeptData.ToArray()) + ")");
            // }
            // else if (this.txtDepartment.Text.Trim() != "" && txtDepartment.Tag.ToString() != ""
            //&& this.txtSubDepartment.Text.Trim() != "" && txtSubDepartment.Tag.ToString() != "")
            // {
            //     dsDepartmentData = oDepartment.PopulateList("where DeptCode in (" + this.txtDepartment.Tag.ToString().Trim() + ")");

            //     foreach (DataRow Dr in dsDepartmentData.Tables[0].Rows)
            //     {
            //         lstDeptData.Add("'" + Dr[0].ToString() + "'");
            //         Application.DoEvents();
            //     }

            //     dsSubDepartmentData = oSubDepartment.PopulateList("where SubDepartmentId in (" + this.txtSubDepartment.Tag.ToString().Trim() + ")");
            //     foreach (DataRow Dr in dsSubDepartmentData.Tables[0].Rows)
            //     {
            //         lstSubDeptData.Add("'" + Dr[0].ToString() + "'");
            //         Application.DoEvents();
            //     }

            //     dsItems = oItem.PopulateList(" where ItemId in (" + string.Join(",", lstData.ToArray()) + ") and DepartmentID in (" + string.Join(",", lstDeptData.ToArray()) + ") and SubDepartmentID in (" + string.Join(",", lstSubDeptData.ToArray()) + ")");

            // }
            // else
            // {
            //     dsDepartmentData = (DepartmentData)oDepartment.PopulateList(" where 1=1 ");
            //     dsSubDepartmentData = (SubDepartmentData)oSubDepartment.PopulateList(" where 1=1 ");

            //     dsItems = oItem.PopulateList(" where ItemId in (" + string.Join(",", lstData.ToArray()) + ")");
            // }
            // //End

            // dsItemVendor = oItemVendor.PopulateList(" where ItemId in (" + string.Join(",", lstData.ToArray()) + ")");
            // if (dsItems == null || dsItems.Tables.Count == 0 || dsItems.Tables[0].Rows.Count == 0)
            // {
            //     throw (new Exception("There are no more items matching transactions of selected period."));
            // }

            // if (dsItemVendor == null || dsItemVendor.Tables.Count == 0 || dsItemVendor.Tables[0].Rows.Count == 0)
            // {
            //     throw (new Exception("There are no more vendor items matching transactions of selected period."));
            // }
            #endregion Commented

            int count = 0;
            podTable = new PODetailTable();
            PODetailRow podRow = null;
            //Added By Amit Date 30 June 2011
            int index = 0;

            DataSet dtItems = null;
            if (this.chckUseTransHistoryForReOrder.Checked == true)
            {
                dtItems = dsTransDetail;
            }
            else if (dsItems != null)
            {
                if (dsItems.Tables[0].Rows.Count > 0)
                {
                    dtItems = dsItems;
                }
                else
                {
                    throw (new Exception("There are no items to "));
                }

            }

            //Added By Amit date 29 July 2011            
            foreach (DataRow podRowTemp in dtItems.Tables[0].Rows)
            {
                DataRow[] oItemRowTemp = dsItems.Tables[0].Select("ItemID='" + podRowTemp["ItemID"] + "'");
                ItemRow oItemRow = null;

                if (oItemRowTemp.Length > 0)
                    oItemRow = (ItemRow)oItemRowTemp[0];
                else
                    continue;


                //if (!oItemRow.ExclFromAutoPO && oItemRow.QtyInStock < oItemRow.ReOrderLevel && oItemRow.ReOrderLevel > 0)
                //{
                lblMessage.Text = "Scanning Item #" + oItemRow.ItemID.Trim() + " For Prefered Vendor";
                Application.DoEvents();

                if (Configuration.convertNullToString(oItemRow.PreferredVendor).Trim().Length > 0)
                {
                    DataRow[] oItemRows = dsItemVendor.Tables[0].Select(" ItemId='" + oItemRow.ItemID.Trim() + "' AND VENDORCODE='" + oItemRow.PreferredVendor.Replace("'", "''") + "'");

                    if (oItemRows != null && oItemRows.Length > 0)
                    {
                        ItemVendorRow oItemVendorRow = (ItemVendorRow)oItemRows[0];
                        DataRow[] TransRows = dtItems.Tables[0].Select(" ItemId='" + oItemRow.ItemID.Trim() + "'");

                        if (TransRows != null && TransRows.Length > 0)
                        {
                            podRow = dsPoDetails.PODetail.NewPODetailRow();
                            podRow.ItemID = oItemRow.ItemID;
                            podRow.ItemDescription = oItemRow.Description;
                            podRow.VendorID = oItemVendorRow.VendorID;
                            podRow.VendorItemCode = oItemVendorRow.VendorItemID;
                            podRow.VendorName = oItemVendorRow.VendorCode;

                            //if (!isFromPO)
                            //{
                            //    podRow.QTY = Configuration.convertNullToInt(podRowTemp["QTY"].ToString());
                            //}
                            //else
                            //{
                            //    //podRow.QTY = oItemRow.MinOrdQty;
                            //    podRow.QTY = oItemRow.MinOrdQty - oItemRow.QtyInStock;
                            //}    

                            //Added by Krishna on 19 December 2011
                            podRow.QTY = Configuration.convertNullToInt(oItemRow.MinOrdQty) - Configuration.convertNullToInt(oItemRow.QtyInStock);
                            if (this.chckUseTransHistoryForReOrder.Checked == true)
                            {
                                podRow.SoldItems = Configuration.convertNullToInt(podRowTemp["QTY"].ToString());
                                //Added by Amit Date 27 july 2011
                                podRow.RetailPrice = Configuration.convertNullToDecimal(podRowTemp["Price"].ToString());
                                podRow.ItemPrice = Configuration.convertNullToDecimal(podRowTemp["Price"].ToString()) - Configuration.convertNullToDecimal(podRowTemp["Discount"].ToString());
                                podRow.Discount = Configuration.convertNullToDecimal(podRowTemp["Discount"].ToString());
                                //End
                            }

                            podRow.QtyInStock = oItemRow.QtyInStock;
                            podRow.ReOrderLevel = oItemRow.ReOrderLevel;
                            //Modified by shitaljit as per Fahad for those Pharmacy which are not using PO 
                            //We should pupulate item cost from Item table not from ItemVendor table.
                            if (isFromPO == true)
                            {
                                podRow.Price = oItemVendorRow.VenorCostPrice;
                            }
                            else
                            {
                                podRow.Price = oItemRow.LastCostPrice;
                            }

                            podRow.PacketSize = oItemRow.PckSize;
                            podRow.PacketQuant = oItemRow.PckQty;
                            podRow.Packetunit = oItemRow.PckUnit;
                            //Start : Added By Amit Date 5 jul 2011
                            podRow.MinOrdQty = oItemRow.MinOrdQty;
                            DataRow[] DeptRow = dsDepartment.Tables[0].Select("DeptID=" + oItemRow.DepartmentID);
                            if (DeptRow.Length != 0)
                                podRow.DeptName = DeptRow[0][2].ToString();

                            DataRow[] SubDeptRow = dsSubDepartment.Tables[0].Select("SubDepartmentID=" + oItemRow.SubDepartmentID);

                            if (SubDeptRow.Length != 0)
                                podRow.SubDeptName = SubDeptRow[0][2].ToString();
                            //End

                            //29 Nov 2011
                            DataRow[] InvRecDateRow = dsInvRecDate.Tables[0].Select(" ItemId='" + oItemRow.ItemID.Trim() + "'");
                            if (InvRecDateRow.Length != 0)
                                podRow.InvRecDate = (System.DateTime)InvRecDateRow[0][clsPOSDBConstants.PODetail_Fld_InvRecDate];
                            //End

                            dsPoDetails.PODetail.AddRow(podRow);

                            index++;

                        }
                    }
                    //  }
                }
            }
            SetPacketToOrder(ref dsPoDetails);

            if (dsPoDetails.PODetail.Rows.Count > 0)
            {
                if (optionReport.CheckedIndex == 0)
                {
                    PODetailData dsTempPoDetails = new PODetailData();
                    DataTable dtTemp = dsPoDetails.PODetail.DefaultView.ToTable(true, "ItemId");

                    lblMessage.Text = dtTemp.Rows.Count.ToString() + " No(s) Items Acquired For Purchase Order By Prefered Vendor";
                    Application.DoEvents();
                }
                else
                {
                    lblMessage.Text = dsPoDetails.PODetail.Rows.Count.ToString() + " No(s) Items Acquired For Purchase Order By Prefered Vendor";
                    Application.DoEvents();
                }
            }

            return podTable;
        }
        private PODetailTable CreateAutoPOBySpecifiedVendorReorder(string VendCode, string ForWhichCat)
        {
            bool queryResult = false;

            PODetailTable podTable = null;

            VendorData dsVendorData = new VendorData();
            VendorSvr oVendor = new VendorSvr();

            DataSet dsTransHeader = new DataSet();
            DataSet dsTransDetail = new DataSet();
            ItemData dsItems = new ItemData();
            ItemVendorData dsItemVendor = new ItemVendorData();
            DepartmentData dsDepartment = new DepartmentData();
            SubDepartmentData dsSubDepartment = new SubDepartmentData();
            DataSet dsInvRecDate = new DataSet();

            //-----------------------             
            GetDsForReorderLevel(ForWhichCat, txtDepartment.Tag.ToString().Trim(), txtSubDepartment.Tag.ToString().Trim(), out dsTransHeader, out dsTransDetail, out dsItems, out dsItemVendor, out dsDepartment, out dsSubDepartment, out dsInvRecDate, out queryResult);

            #region Commented
            // string SelectClause = string.Empty;
            // //changed by SRT(Abhishek)  Date : 21 Sept. 2009
            // //Added condition to check if Transaction should be of Sell Type 
            // SelectClause = " Trans.TransDate >= DateAdd(D,-" + clsPOSDBConstants.ItemReorderPeriod.ToString() + ",getdate()) AND Trans.TransType=1 ";
            // //End of changed by SRT(Abhishek)  Date : 21 Sept. 2009

            // lblMessage.Text = "Aquiring Transactions For " + ForWhichCat + " Vendor.";
            // Application.DoEvents();
            // dsTransHeader = oPOSTrans.PopulateList(SelectClause);
            // if (dsTransHeader == null || dsTransHeader.Tables.Count == 0 || dsTransHeader.Tables[0].Rows.Count == 0)
            // {
            //     throw (new Exception("There are no more transactions in this last " + clsPOSDBConstants.ItemReorderPeriod.ToString() + " day(s)."));
            // }
            // List<string> lstData = new List<string>();
            // foreach (DataRow Dr in dsTransHeader.Tables[0].Rows)
            // {
            //     lstData.Add(Dr[0].ToString());
            //     Application.DoEvents();
            // }
            // dsTransDetail = oPOSTransDetail.PopulateList(" where TransID in (" + string.Join(",", lstData.ToArray()) + ")", out queryResult);
            // lstData = new List<string>();
            // foreach (DataRow Dr in dsTransDetail.Tables[0].Rows)
            // {
            //     lstData.Add("'" + Dr[0].ToString() + "'");
            //     Application.DoEvents();
            // }
            // lblMessage.Text = "Aquiring Transaction Items For " + ForWhichCat + " Vendor.";
            // Application.DoEvents();

            // //Start: Added By Amit Date 30 June 2011
            // List<string> lstDeptData = new List<string>();
            // List<string> lstSubDeptData = new List<string>();

            // if (this.txtDepartment.Text.Trim() != "" && txtDepartment.Tag.ToString() != ""
            //     && this.txtSubDepartment.Text.Trim() == "" && txtSubDepartment.Tag.ToString() == "")
            // {
            //     dsDepartmentData = oDepartment.PopulateList("where DeptCode in (" + this.txtDepartment.Tag.ToString().Trim() + ")");

            //     foreach (DataRow Dr in dsDepartmentData.Tables[0].Rows)
            //     {
            //         lstDeptData.Add("'" + Dr[0].ToString() + "'");
            //         Application.DoEvents();
            //     }
            //     dsSubDepartmentData = oSubDepartment.PopulateList("where DepartmentId in (" + string.Join(",", lstDeptData.ToArray()) + ")");

            //     dsItems = oItem.PopulateList(" where ItemId in (" + string.Join(",", lstData.ToArray()) + ") and DepartmentID in (" + string.Join(",", lstDeptData.ToArray()) + ")");
            // }
            // else if (this.txtDepartment.Text.Trim() == "" && txtDepartment.Tag.ToString() == ""
            //     && this.txtSubDepartment.Text.Trim() != "" && txtSubDepartment.Tag.ToString() != "")
            // {
            //     dsSubDepartmentData = oSubDepartment.PopulateList("where SubDepartmentId in (" + this.txtSubDepartment.Tag.ToString().Trim() + ")");
            //     foreach (DataRow Dr in dsSubDepartmentData.Tables[0].Rows)
            //     {
            //         lstSubDeptData.Add("'" + Dr[0].ToString() + "'");
            //         lstDeptData.Add("'" + Dr[1].ToString() + "'");
            //         Application.DoEvents();
            //     }
            //     dsDepartmentData = oDepartment.PopulateList("where DeptID in (" + string.Join(",", lstDeptData.ToArray()) + ")");

            //     dsItems = oItem.PopulateList(" where ItemId in (" + string.Join(",", lstData.ToArray()) + ") and SubDepartmentID in (" + string.Join(",", lstSubDeptData.ToArray()) + ")");
            // }
            // else if (this.txtDepartment.Text.Trim() != "" && txtDepartment.Tag.ToString() != ""
            //&& this.txtSubDepartment.Text.Trim() != "" && txtSubDepartment.Tag.ToString() != "")
            // {
            //     dsDepartmentData = oDepartment.PopulateList("where DeptCode in (" + this.txtDepartment.Tag.ToString().Trim() + ")");

            //     foreach (DataRow Dr in dsDepartmentData.Tables[0].Rows)
            //     {
            //         lstDeptData.Add("'" + Dr[0].ToString() + "'");
            //         Application.DoEvents();
            //     }

            //     dsSubDepartmentData = oSubDepartment.PopulateList("where SubDepartmentId in (" + this.txtSubDepartment.Tag.ToString().Trim() + ")");
            //     foreach (DataRow Dr in dsSubDepartmentData.Tables[0].Rows)
            //     {
            //         lstSubDeptData.Add("'" + Dr[0].ToString() + "'");
            //         Application.DoEvents();
            //     }

            //     dsItems = oItem.PopulateList(" where ItemId in (" + string.Join(",", lstData.ToArray()) + ") and DepartmentID in (" + string.Join(",", lstDeptData.ToArray()) + ") and SubDepartmentID in (" + string.Join(",", lstSubDeptData.ToArray()) + ")");

            // }
            // else
            // {
            //     dsDepartmentData = (DepartmentData)oDepartment.PopulateList(" where 1=1 ");
            //     dsSubDepartmentData = (SubDepartmentData)oSubDepartment.PopulateList(" where 1=1 ");

            //     dsItems = oItem.PopulateList(" where ItemId in (" + string.Join(",", lstData.ToArray()) + ")");
            // }
            // //End          


            //if (dsItems == null || dsItems.Tables.Count == 0 || dsItems.Tables[0].Rows.Count == 0)
            //{
            //    throw (new Exception("There are no more items matching transactions of selected period."));
            //}

            //dsItemVendor = oItemVendor.PopulateList(" where ItemId in (" + string.Join(",", lstData.ToArray()) + ")");

            //if (dsItemVendor == null || dsItemVendor.Tables.Count == 0 || dsItemVendor.Tables[0].Rows.Count == 0 && isFromPO)
            //{
            //    throw (new Exception("There are no more vendor items matching transactions of selected period."));
            //}
            #endregion

            //Added by Amit Date 16 Aug 2011
            dsVendorData = oVendor.PopulateList(" where 1=1");
            //End
            int count = 0;
            podTable = new PODetailTable();
            PODetailRow podRow = null;
            //Added By Amit Date 30 June 2011
            int index = 0;

            DataSet dtItems = null;

            if (this.chckUseTransHistoryForReOrder.Checked == true)
            {
                dtItems = dsTransDetail;
            }
            else if (dsItems != null)
            {
                if (dsItems.Tables[0].Rows.Count > 0)
                {
                    dtItems = dsItems;
                }
                else
                {
                    throw (new Exception("There are no items to "));
                }

            }
            //Added By Amit date 29 July 2011      
            Dictionary<string, bool> ItemUseddictionary = new Dictionary<string, bool>();
            foreach (DataRow podRowTemp in dtItems.Tables[0].Rows)
            {
                DataRow[] oItemRowTemp = dsItems.Tables[0].Select("ItemID='" + podRowTemp["ItemID"] + "'");
                //dsItems.Tables[0].
                ItemRow oItemRow = null;

                if (oItemRowTemp.Length > 0)
                {
                    oItemRow = (ItemRow)oItemRowTemp[0];
                    //if (!ItemUseddictionary.ContainsKey(oItemRow.ItemID))
                    //    ItemUseddictionary.Add(oItemRow.ItemID, true);
                    //else
                    //{
                    //    oItemRow = null; 
                    //    continue;
                    //}

                }

                else
                    continue;
                //End

                lblMessage.Text = "Scanning Item #" + oItemRow.ItemID.Trim() + " For " + ForWhichCat + " Vendor";

                Application.DoEvents();
                DataRow[] oItemRows = dsItemVendor.Tables[0].Select(" ItemId='" + oItemRow.ItemID.Trim() + "' and Trim(vendorcode)='" + VendCode.Replace("'", "''") + "'");

                if (oItemRows != null && oItemRows.Length > 0)
                {
                    ItemVendorRow oItemVendorRow = (ItemVendorRow)oItemRows[0];
                    DataRow[] TransRows = dtItems.Tables[0].Select(" ItemId='" + oItemRow.ItemID.Trim() + "'");


                    if (TransRows != null && TransRows.Length > 0)
                    {
                        podRow = dsPoDetails.PODetail.NewPODetailRow();
                        podRow.ItemID = oItemRow.ItemID;
                        podRow.ItemDescription = oItemRow.Description;
                        podRow.VendorID = oItemVendorRow.VendorID;
                        podRow.VendorItemCode = oItemVendorRow.VendorItemID;
                        podRow.VendorName = oItemVendorRow.VendorCode;

                        //Added By Amit Date 21 Oct 2011
                        //if (isFromPO) for sugg qty
                        //{
                        //    // podRow.QTY = oItemRow.MinOrdQty;                            
                        //    podRow.QTY = oItemRow.MinOrdQty - oItemRow.QtyInStock;
                        //}
                        //else
                        //{
                        //    podRow.QTY = Configuration.convertNullToInt(podRowTemp["QTY"].ToString());
                        //}                        
                        //End    

                        //Added By Amit Date 6 Dec 2011
                        podRow.QTY = Configuration.convertNullToInt(oItemRow.MinOrdQty) - Configuration.convertNullToInt(oItemRow.QtyInStock);
                        if (this.chckUseTransHistoryForReOrder.Checked == true)
                        {
                            podRow.SoldItems = Configuration.convertNullToInt(podRowTemp["QTY"].ToString());
                            //Added by Amit Date 27 july 2011
                            podRow.RetailPrice = Configuration.convertNullToDecimal(podRowTemp["Price"].ToString());
                            podRow.ItemPrice = Configuration.convertNullToDecimal(podRowTemp["Price"].ToString()) - Configuration.convertNullToDecimal(podRowTemp["Discount"].ToString());
                            podRow.Discount = Configuration.convertNullToDecimal(podRowTemp["Discount"].ToString());
                            //End
                        }
                        podRow.QtyInStock = oItemRow.QtyInStock;
                        podRow.ReOrderLevel = oItemRow.ReOrderLevel;
                        //Modified by shitaljit as per Fahad for those Pharmacy which are not using PO 
                        //We should populate item cost from Item table not from ItemVendor table.
                        if (isFromPO == true)
                        {
                            podRow.Price = oItemVendorRow.VenorCostPrice;
                        }
                        else
                        {
                            podRow.Price = oItemRow.LastCostPrice;
                        }

                        podRow.PacketSize = oItemRow.PckSize;
                        podRow.PacketQuant = oItemRow.PckQty;
                        podRow.Packetunit = oItemRow.PckUnit;
                        //Start : Added By Amit Date 5 jul 2011
                        podRow.MinOrdQty = oItemRow.MinOrdQty;
                        DataRow[] DeptRow = dsDepartment.Tables[0].Select("DeptID=" + oItemRow.DepartmentID);
                        if (DeptRow.Length != 0)
                            podRow.DeptName = DeptRow[0][2].ToString();

                        DataRow[] SubDeptRow = dsSubDepartment.Tables[0].Select("SubDepartmentID=" + oItemRow.SubDepartmentID);

                        if (SubDeptRow.Length != 0)
                            podRow.SubDeptName = SubDeptRow[0][2].ToString();
                        //End
                        //Added by Amit Date 27 july 2011

                        //End
                        //29 Nov 2011
                        DataRow[] InvRecDateRow = dsInvRecDate.Tables[0].Select(" ItemId='" + oItemRow.ItemID.Trim() + "'");
                        if (InvRecDateRow.Length != 0)
                            podRow.InvRecDate = (System.DateTime)InvRecDateRow[0][clsPOSDBConstants.PODetail_Fld_InvRecDate];
                        //End
                        dsPoDetails.PODetail.AddRow(podRow);

                        index++;
                    }
                }
                //Added by Amit Date 17 Aug 2011
                else if (!isFromPO && oItemRow.LastVendor.Trim().ToUpper() == VendCode.Trim().ToUpper())
                {
                    VendorRow[] vendorRows = (VendorRow[])dsVendorData.Tables[0].Select("VendorCode='" + oItemRow.LastVendor.Replace("'", "''") + "'");
                    if (vendorRows != null && vendorRows.Length > 0)
                    {
                        podRow = dsPoDetails.PODetail.NewPODetailRow();
                        podRow.ItemID = oItemRow.ItemID;
                        podRow.ItemDescription = oItemRow.Description;
                        podRow.VendorID = vendorRows[0].VendorId;
                        podRow.VendorItemCode = "";
                        podRow.VendorName = vendorRows[0].Vendorname;
                        // podRow.QTY = Configuration.convertNullToInt(podRowTemp["Qty"].ToString());
                        podRow.QTY = Configuration.convertNullToInt(oItemRow.MinOrdQty) - Configuration.convertNullToInt(oItemRow.QtyInStock);

                        podRow.QtyInStock = oItemRow.QtyInStock;
                        podRow.ReOrderLevel = oItemRow.ReOrderLevel;
                        //Modified by shitaljit as per Fahad for those Pharmacy which are not using PO 
                        //We should pupulate item cost from Item table not from ItemVendor table.
                        //podRow.Price = 0;
                        podRow.Price = oItemRow.LastCostPrice;
                        if (this.chckUseTransHistoryForReOrder.Checked == true)
                        {
                            podRow.SoldItems = Configuration.convertNullToInt(podRowTemp["QTY"].ToString());
                            //Added by Amit Date 27 july 2011
                            podRow.RetailPrice = Configuration.convertNullToDecimal(podRowTemp["Price"].ToString());
                            podRow.ItemPrice = Configuration.convertNullToDecimal(podRowTemp["Price"].ToString()) - Configuration.convertNullToDecimal(podRowTemp["Discount"].ToString());
                            podRow.Discount = Configuration.convertNullToDecimal(podRowTemp["Discount"].ToString());
                            //End
                        }

                        podRow.PacketSize = oItemRow.PckSize;
                        podRow.PacketQuant = oItemRow.PckQty;
                        podRow.Packetunit = oItemRow.PckUnit;
                        podRow.MinOrdQty = oItemRow.MinOrdQty;

                        DataRow[] DeptRow = dsDepartment.Tables[0].Select("DeptID=" + oItemRow.DepartmentID);
                        if (DeptRow.Length != 0)
                            podRow.DeptName = DeptRow[0][2].ToString();

                        DataRow[] SubDeptRow = dsSubDepartment.Tables[0].Select("SubDepartmentID=" + oItemRow.SubDepartmentID);

                        if (SubDeptRow.Length != 0)
                            podRow.SubDeptName = SubDeptRow[0][2].ToString();

                        //29 Nov 2011
                        DataRow[] InvRecDateRow = dsInvRecDate.Tables[0].Select(" ItemId='" + oItemRow.ItemID.Trim() + "'");

                        if (InvRecDateRow.Length != 0)
                            podRow.InvRecDate = (System.DateTime)InvRecDateRow[0][clsPOSDBConstants.PODetail_Fld_InvRecDate];
                        //End

                        dsPoDetails.PODetail.AddRow(podRow);

                        index++;
                    }//End 17 Aug 
                }
                Application.DoEvents();
            }

            SetPacketToOrder(ref dsPoDetails);

            if (dsPoDetails.PODetail.Rows.Count > 0)
            {
                if (optionReport.CheckedIndex == 0)
                {
                    PODetailData dsTempPoDetails = new PODetailData();
                    DataTable dtTemp = dsPoDetails.PODetail.DefaultView.ToTable(true, "ItemId");

                    lblMessage.Text = dtTemp.Rows.Count.ToString() + " No(s) Items Acquired For Purchase Order By " + ForWhichCat + " Vendor";
                    Application.DoEvents();
                }
                else
                {
                    lblMessage.Text = dsPoDetails.PODetail.Rows.Count.ToString() + " No(s) Items Acquired For Purchase Order By " + ForWhichCat + " Vendor";
                    Application.DoEvents();
                }
            }
            return podTable;
        }

        //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Added By Amit Date 1 July 2011
        /// By Specified Vendor Qty Zero
        /// </summary>
        /// <param name="VendCode"></param>
        /// <param name="ForWhichCat"></param>
        /// <returns></returns>
        private PODetailTable CreateAutoPOBySpecifiedVendorQtyZero(string VendCode, string ForWhichCat)
        {
            bool queryResult = false;
            PODetailTable podTable = null;
            //Added By Amit Date 4 Aug 2011
            DataSet dsTransDetailData = new DataSet();
            TransDetailSvr oPOSTransDetail = new TransDetailSvr();
            //End
            ItemVendorData dsItemVendor = new ItemVendorData();
            ItemData dsItems = new ItemData();
            ItemSvr oItem = new ItemSvr();
            ItemVendorSvr oItemVendor = new ItemVendorSvr();
            DepartmentData dsDepartmentData = new DepartmentData();
            DepartmentSvr oDepartment = new DepartmentSvr();
            SubDepartmentData dsSubDepartmentData = new SubDepartmentData();
            SubDepartmentSvr oSubDepartment = new SubDepartmentSvr();
            //Added By Amit Date 16 Aug 2011
            VendorData dsVendorData = new VendorData();
            VendorSvr oVendor = new VendorSvr();
            //End
            //Added By Amit Date 29 Nov 2011
            InvRecvHeaderSvr oInvRecHeader = new InvRecvHeaderSvr();
            DataSet dsInvRecDate = new DataSet();
            //End

            //List<string> lstData = new List<string>();
            List<string> lstDeptData = new List<string>();
            List<string> lstSubDeptData = new List<string>();

            lblMessage.Text = "Aquiring Transaction Items For " + ForWhichCat + " Vendor.";

            #region Sprint-19 - 1883 23-Mar-2015 JY Commented
            // if (this.txtDepartment.Text.Trim() != "" && txtDepartment.Tag.ToString() != ""
            //     && this.txtSubDepartment.Text.Trim() == "" && txtSubDepartment.Tag.ToString() == "")
            // {
            //     dsDepartmentData = oDepartment.PopulateList("where DeptCode in (" + this.txtDepartment.Tag.ToString().Trim() + ")");

            //     foreach (DataRow Dr in dsDepartmentData.Tables[0].Rows)
            //     {
            //         lstDeptData.Add("'" + Dr[0].ToString() + "'");
            //         Application.DoEvents();
            //     }
            //     dsItems = oItem.PopulateList(" where DepartmentID in (" + string.Join(",", lstDeptData.ToArray()) + ") AND " + clsPOSDBConstants.Item_Fld_QtyInStock + " <= 0");
            //     dsSubDepartmentData = oSubDepartment.PopulateList("where DepartmentId in (" + string.Join(",", lstDeptData.ToArray()) + ")");


            // }
            // else if (this.txtDepartment.Text.Trim() == "" && txtDepartment.Tag.ToString() == ""
            //     && this.txtSubDepartment.Text.Trim() != "" && txtSubDepartment.Tag.ToString() != "")
            // {
            //     dsSubDepartmentData = oSubDepartment.PopulateList("where SubDepartmentId in (" + this.txtSubDepartment.Tag.ToString().Trim() + ")");
            //     foreach (DataRow Dr in dsSubDepartmentData.Tables[0].Rows)
            //     {
            //         lstSubDeptData.Add("'" + Dr[0].ToString() + "'");
            //         lstDeptData.Add("'" + Dr[1].ToString() + "'");
            //         Application.DoEvents();
            //     }
            //     dsDepartmentData = oDepartment.PopulateList("where DeptID in (" + string.Join(",", lstDeptData.ToArray()) + ")");
            //     dsItems = oItem.PopulateList(" where DepartmentID in (" + string.Join(",", lstDeptData.ToArray()) + ") AND " + clsPOSDBConstants.Item_Fld_QtyInStock + " <= 0");

            // }
            // else if (this.txtDepartment.Text.Trim() != "" && txtDepartment.Tag.ToString() != ""
            //&& this.txtSubDepartment.Text.Trim() != "" && txtSubDepartment.Tag.ToString() != "")
            // {
            //     dsDepartmentData = oDepartment.PopulateList("where DeptCode in (" + this.txtDepartment.Tag.ToString().Trim() + ")");

            //     foreach (DataRow Dr in dsDepartmentData.Tables[0].Rows)
            //     {
            //         lstDeptData.Add("'" + Dr[0].ToString() + "'");
            //         Application.DoEvents();
            //     }

            //     dsSubDepartmentData = oSubDepartment.PopulateList("where SubDepartmentId in (" + this.txtSubDepartment.Tag.ToString().Trim() + ")");
            //     foreach (DataRow Dr in dsSubDepartmentData.Tables[0].Rows)
            //     {
            //         lstSubDeptData.Add("'" + Dr[0].ToString() + "'");
            //         Application.DoEvents();
            //     }
            //     dsItems = oItem.PopulateList(" where DepartmentID in (" + string.Join(",", lstDeptData.ToArray()) + ") and SubDepartmentId in (" + this.txtSubDepartment.Tag.ToString().Trim() + ") AND " + clsPOSDBConstants.Item_Fld_QtyInStock + " <= 0");
            // }
            #endregion

            #region Sprint-19 - 1883 23-Mar-2015 JY Added location filter and also improved the existing filter code
            string strFilter = string.Empty;

            if (this.txtDepartment.Text.Trim() != "" && txtDepartment.Tag.ToString() != "")
            {
                dsDepartmentData = oDepartment.PopulateList("where DeptCode in (" + this.txtDepartment.Tag.ToString().Trim() + ")");

                foreach (DataRow Dr in dsDepartmentData.Tables[0].Rows)
                {
                    lstDeptData.Add("'" + Dr[0].ToString() + "'");
                }
                strFilter += " and DepartmentID in (" + string.Join(",", lstDeptData.ToArray()) + ")";

                if (this.txtSubDepartment.Text.Trim() != "" && txtSubDepartment.Tag.ToString() != "")
                {
                    dsSubDepartmentData = oSubDepartment.PopulateList("where SubDepartmentId in (" + this.txtSubDepartment.Tag.ToString().Trim() + ")");
                    foreach (DataRow Dr in dsSubDepartmentData.Tables[0].Rows)
                    {
                        lstSubDeptData.Add("'" + Dr[0].ToString() + "'");
                        lstDeptData.Add("'" + Dr[1].ToString() + "'");
                        Application.DoEvents();
                    }
                    strFilter += " and SubDepartmentID in (" + string.Join(",", lstSubDeptData.ToArray()) + ")";
                }
                else
                {
                    dsSubDepartmentData = oSubDepartment.PopulateList("where DepartmentId in (" + string.Join(",", lstDeptData.ToArray()) + ")");
                }
            }
            else
            {
                if (this.txtSubDepartment.Text.Trim() != "" && txtSubDepartment.Tag.ToString() != "")
                {
                    dsSubDepartmentData = oSubDepartment.PopulateList("where SubDepartmentId in (" + this.txtSubDepartment.Tag.ToString().Trim() + ")");
                    foreach (DataRow Dr in dsSubDepartmentData.Tables[0].Rows)
                    {
                        lstSubDeptData.Add("'" + Dr[0].ToString() + "'");
                        lstDeptData.Add("'" + Dr[1].ToString() + "'");
                        Application.DoEvents();
                    }
                    strFilter += " and SubDepartmentID in (" + string.Join(",", lstSubDeptData.ToArray()) + ")";
                    dsDepartmentData = oDepartment.PopulateList("where DeptID in (" + string.Join(",", lstDeptData.ToArray()) + ")");
                }
                else
                {
                    dsDepartmentData = (DepartmentData)oDepartment.PopulateList(" where 1=1 ");
                    dsSubDepartmentData = (SubDepartmentData)oSubDepartment.PopulateList(" where 1=1 ");
                }
            }

            if (this.txtItemLocation.Text.Trim() != "" && txtItemLocation.Tag.ToString() != "")
            {
                strFilter += " and LOCATION in (" + txtItemLocation.Text + ")";
            }
            dsItems = oItem.PopulateList(" where " + clsPOSDBConstants.Item_Fld_QtyInStock + " <= 0" + strFilter);
            #endregion

            #region Consider Transaction history in the item populate for Zero or -Ve qty logic
            //added By Shitaljit to consider Transaction history in Zero or Negative Qty.
            // JIRA link -PRIMEPOS -13
            string strDistItems = string.Empty;
            if (this.chckUseTransHistoryForReOrder.Checked == true)
            {
                dsDepartmentData = (DepartmentData)oDepartment.PopulateList(" where 1=1 ");
                dsSubDepartmentData = (SubDepartmentData)oSubDepartment.PopulateList(" where 1=1 ");
                string SelectClause = string.Empty;
                if (Configuration.CPrimeEDISetting.ConsiderReturnTrans)  //Sprint-27 - PRIMEPOS-2390 28-Sep-2017 JY Added 
                    SelectClause = " Trans.TransDate >= DateAdd(D,-" + this.numNoOfDaysForItemReOrder.Value.ToString() + ",getdate()) ";
                else
                    SelectClause = " Trans.TransDate >= DateAdd(D,-" + this.numNoOfDaysForItemReOrder.Value.ToString() + ",getdate()) AND Trans.TransType=1 ";
                Application.DoEvents();
                string strSQL = string.Empty;
                TransHeaderSvr oPOSTrans = new TransHeaderSvr();
                //DataSet dsTransHeader = oPOSTrans.PopulateList(SelectClause); //PRIMEPOS-3097 28-Jun-2022 JY commented
                DataSet dsTransHeader = oPOSTrans.PopulateList(SelectClause, out strSQL);   //PRIMEPOS-3097 28-Jun-2022 JY Added
                if (dsTransHeader == null || dsTransHeader.Tables.Count == 0 || dsTransHeader.Tables[0].Rows.Count == 0)
                {
                    throw (new Exception("There are no more transactions in this last " + this.numNoOfDaysForItemReOrder.Value.ToString() + " day(s)."));
                }
                #region PRIMEPOS-3097 28-Jun-2022 JY Commented
                //foreach (DataRow Dr in dsTransHeader.Tables[0].Rows)
                //{
                //    lstData.Add(Dr[0].ToString());
                //}
                //string whereClause = " where TransID in (" + string.Join(",", lstData.ToArray()) + ")";
                //lstData.Clear();
                //foreach (DataRow Dr in dsItems.Tables[0].Rows)
                //{
                //    lstData.Add("'" + Dr[0].ToString() + "'");
                //    Application.DoEvents();
                //}
                //if (lstData.Count > 0)
                //{
                //    whereClause += "AND ItemID in (" + string.Join(",", lstData.ToArray()) + ")";
                //}
                //dsTransDetailData = oPOSTransDetail.PopulateList(whereClause);
                //lstData.Clear();
                //foreach (DataRow Dr in dsTransDetailData.Tables[0].Rows)
                //{
                //    lstData.Add("'" + Dr[0].ToString() + "'");
                //    Application.DoEvents();
                //}                
                //dsItems = oItem.PopulateList(" where ItemID in (" + string.Join(",", lstData.ToArray()) + ") AND " + clsPOSDBConstants.Item_Fld_QtyInStock + " <= 0");
                #endregion

                #region PRIMEPOS-3097 28-Jun-2022 JY Added                
                dsTransDetailData = oPOSTransDetail.PopulateList(" where TransID in (" + strSQL + ") AND ItemID in (SELECT ItemID FROM Item where QtyInStock <= 0)", out queryResult, out strDistItems);
                dsItems = oItem.PopulateList(" where ItemID in (" + strDistItems + ") AND " + clsPOSDBConstants.Item_Fld_QtyInStock + " <= 0");
                #endregion
            }
            else
            {
                dsDepartmentData = (DepartmentData)oDepartment.PopulateList(" where 1=1 ");
                dsSubDepartmentData = (SubDepartmentData)oSubDepartment.PopulateList(" where 1=1 ");
                dsItems = oItem.PopulateList(" where " + clsPOSDBConstants.Item_Fld_QtyInStock + " <= 0");

                #region PRIMEPOS-3097 28-Jun-2022 JY Commented
                //foreach (DataRow Dr in dsItems.Tables[0].Rows)
                //{
                //    lstData.Add("'" + Dr[0].ToString() + "'");
                //    Application.DoEvents();
                //}
                #endregion
                //Added By Amit Date 4 Aug 2011
                //dsTransDetailData = oPOSTransDetail.PopulateList(" where ItemID in (" + string.Join(",", lstData.ToArray()) + ")");   //PRIMEPOS-3097 28-Jun-2022 JY commented
                dsTransDetailData = oPOSTransDetail.PopulateList(" where ItemID in (SELECT ItemID FROM Item where QtyInStock <= 0)", out queryResult, out strDistItems);    //PRIMEPOS-3097 28-Jun-2022 JY Added
                if (dsTransDetailData == null || dsTransDetailData.Tables.Count == 0 || dsTransDetailData.Tables[0].Rows.Count == 0)
                {
                    throw (new Exception("There are no items found which are used in Transaction."));
                }
                //End
            }
            #endregion

            //dsItemVendor = oItemVendor.PopulateList(" where ItemId in (" + string.Join(",", lstData.ToArray()) + ")");    //PRIMEPOS-3097 28-Jun-2022 JY commented
            dsItemVendor = oItemVendor.PopulateList(" where ItemId in (" + strDistItems + ")", true);  //PRIMEPOS-3097 28-Jun-2022 JY Added
            if (dsItemVendor == null || dsItemVendor.Tables.Count == 0 || dsItemVendor.Tables[0].Rows.Count == 0 && isFromPO)
            {
                throw (new Exception("There are no more vendor items matching Specified vendor found."));
            }
            //Added by Amit Date 16 Aug 2011
            dsVendorData = oVendor.PopulateList(" where 1=1");
            //End
            //Added By Amit Date 29 Nov 2011
            //dsInvRecDate = oInvRecHeader.ItemInvRecDate(lstData);   //PRIMEPOS-3097 28-Jun-2022 JY Commented
            dsInvRecDate = oInvRecHeader.ItemInvRecDate(strDistItems);   //PRIMEPOS-3097 28-Jun-2022 JY Added
            //End

            //int count = 0;
            podTable = new PODetailTable();
            PODetailRow podRow = null;
            //Added by Amit Date 1 jul 2011
            int index = 0;

            //foreach (ItemRow oItemRow in dsItems.Tables[0].Rows)
            //Added By Amit date 29 July 2011            
            foreach (DataRow podRowTemp in dsTransDetailData.Tables[0].Rows)
            {
                DataRow[] oItemRowTemp = dsItems.Tables[0].Select("ItemID='" + podRowTemp["ItemID"] + "'");
                ItemRow oItemRow = null;

                if (oItemRowTemp.Length > 0)
                    oItemRow = (ItemRow)oItemRowTemp[0];
                else
                    continue;
                //End

                lblMessage.Text = "Scanning Item #" + oItemRow.ItemID.Trim() + " For " + ForWhichCat + " Vendor";
                Application.DoEvents();
                DataRow[] oItemRows = dsItemVendor.Tables[0].Select(" ItemId='" + oItemRow.ItemID.Trim() + "' and vendorcode='" + VendCode.Replace("'", "''") + "'");
                if (oItemRows != null && oItemRows.Length > 0)
                {
                    ItemVendorRow oItemVendorRow = (ItemVendorRow)oItemRows[0];
                    podRow = dsPoDetails.PODetail.NewPODetailRow();
                    podRow.ItemID = oItemRow.ItemID;
                    podRow.ItemDescription = oItemRow.Description;
                    podRow.VendorID = oItemVendorRow.VendorID;
                    podRow.VendorItemCode = oItemVendorRow.VendorItemID;
                    podRow.VendorName = oItemVendorRow.VendorCode;
                    podRow.QTY = Configuration.convertNullToInt(podRowTemp["Qty"].ToString());
                    podRow.QtyInStock = oItemRow.QtyInStock;
                    podRow.ReOrderLevel = oItemRow.ReOrderLevel;
                    //Modified by shitaljit as per Fahad for those Pharmacy which are not using PO 
                    //We should pupulate item cost from Item table not from ItemVendor table.
                    podRow.SoldItems = Configuration.convertNullToInt(podRowTemp["QTY"].ToString());
                    if (isFromPO == true)
                    {
                        podRow.Price = oItemVendorRow.VenorCostPrice;
                    }
                    else
                    {
                        podRow.Price = oItemRow.LastCostPrice;
                    }

                    podRow.PacketSize = oItemRow.PckSize;
                    podRow.PacketQuant = oItemRow.PckQty;
                    podRow.Packetunit = oItemRow.PckUnit;
                    //Start : Added By Amit Date 5 jul 2011
                    podRow.MinOrdQty = oItemRow.MinOrdQty;
                    DataRow[] DeptRow = dsDepartmentData.Tables[0].Select("DeptID=" + oItemRow.DepartmentID);
                    if (DeptRow.Length != 0)
                        podRow.DeptName = DeptRow[0][2].ToString();

                    DataRow[] SubDeptRow = dsSubDepartmentData.Tables[0].Select("SubDepartmentID=" + oItemRow.SubDepartmentID);

                    if (SubDeptRow.Length != 0)
                        podRow.SubDeptName = SubDeptRow[0][2].ToString();
                    //End
                    //Added by Amit Date 4 Aug 2011
                    podRow.RetailPrice = Configuration.convertNullToDecimal(podRowTemp["Price"].ToString());
                    podRow.ItemPrice = Configuration.convertNullToDecimal(podRowTemp["Price"].ToString()) - Configuration.convertNullToDecimal(podRowTemp["Discount"].ToString());
                    podRow.Discount = Configuration.convertNullToDecimal(podRowTemp["Discount"].ToString());
                    //End

                    //29 Nov 2011
                    DataRow[] InvRecDateRow = dsInvRecDate.Tables[0].Select(" ItemId='" + oItemRow.ItemID.Trim() + "'");
                    if (InvRecDateRow.Length != 0)
                        podRow.InvRecDate = (System.DateTime)InvRecDateRow[0][clsPOSDBConstants.PODetail_Fld_InvRecDate];
                    //End

                    dsPoDetails.PODetail.AddRow(podRow);

                    index++;
                }
                //Added by Amit Date 17 Aug 2011
                else if (!isFromPO && oItemRow.LastVendor.Trim().ToUpper() == VendCode.Trim().ToUpper())
                {
                    VendorRow[] vendorRows = (VendorRow[])dsVendorData.Tables[0].Select("VendorCode='" + oItemRow.LastVendor.Replace("'", "''") + "'");
                    if (vendorRows != null && vendorRows.Length > 0)
                    {
                        podRow = dsPoDetails.PODetail.NewPODetailRow();
                        podRow.ItemID = oItemRow.ItemID;
                        podRow.ItemDescription = oItemRow.Description;
                        podRow.VendorID = vendorRows[0].VendorId;
                        podRow.VendorItemCode = "";
                        podRow.VendorName = vendorRows[0].Vendorname;
                        podRow.QTY = Configuration.convertNullToInt(podRowTemp["Qty"].ToString());
                        podRow.QtyInStock = oItemRow.QtyInStock;
                        podRow.ReOrderLevel = oItemRow.ReOrderLevel;
                        //Modified by shitaljit as per Fahad for those Pharmacy which are not using PO 
                        //We should pupulate item cost from Item table not from ItemVendor table.
                        //podRow.Price = 0;
                        podRow.Price = oItemRow.LastCostPrice;
                        podRow.SoldItems = Configuration.convertNullToInt(podRowTemp["QTY"].ToString());

                        podRow.PacketSize = oItemRow.PckSize;
                        podRow.PacketQuant = oItemRow.PckQty;
                        podRow.Packetunit = oItemRow.PckUnit;
                        podRow.MinOrdQty = oItemRow.MinOrdQty;

                        DataRow[] DeptRow = dsDepartmentData.Tables[0].Select("DeptID=" + oItemRow.DepartmentID);
                        if (DeptRow.Length != 0)
                            podRow.DeptName = DeptRow[0][2].ToString();

                        DataRow[] SubDeptRow = dsSubDepartmentData.Tables[0].Select("SubDepartmentID=" + oItemRow.SubDepartmentID);

                        if (SubDeptRow.Length != 0)
                            podRow.SubDeptName = SubDeptRow[0][2].ToString();

                        podRow.RetailPrice = Configuration.convertNullToDecimal(podRowTemp["Price"].ToString());
                        podRow.ItemPrice = Configuration.convertNullToDecimal(podRowTemp["Price"].ToString()) - Configuration.convertNullToDecimal(podRowTemp["Discount"].ToString());
                        podRow.Discount = Configuration.convertNullToDecimal(podRowTemp["Discount"].ToString());

                        //29 Nov 2011
                        DataRow[] InvRecDateRow = dsInvRecDate.Tables[0].Select(" ItemId='" + oItemRow.ItemID.Trim() + "'");
                        if (InvRecDateRow.Length != 0)
                            podRow.InvRecDate = (System.DateTime)InvRecDateRow[0][clsPOSDBConstants.PODetail_Fld_InvRecDate];
                        //End

                        dsPoDetails.PODetail.AddRow(podRow);

                        index++;
                    }//End 17 Aug
                }
                Application.DoEvents();
            }
            SetPacketToOrder(ref dsPoDetails);
            if (dsPoDetails.PODetail.Rows.Count > 0)
            {
                lblMessage.Text = dsPoDetails.PODetail.Rows.Count.ToString() + " No(s) Items Acquired For Purchase Order By " + ForWhichCat + " Vendor";
                Application.DoEvents();
            }
            return podTable;
        }

        //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Added By Amit Date 1 July 2011
        /// Preferred Vendor Qty Zero
        /// </summary>        
        /// <returns></returns>
        private PODetailTable CreateAutoPOByPreferredVendorQtyZero()
        {
            bool queryResult = false;
            PODetailTable podTable = null;
            //Added By Amit Date 4 Aug 2011
            DataSet dsTransDetailData = new DataSet();
            TransDetailSvr oPOSTransDetail = new TransDetailSvr();
            //End
            ItemVendorData dsItemVendor = new ItemVendorData();
            ItemData dsItems = new ItemData();
            ItemSvr oItem = new ItemSvr();
            ItemVendorSvr oItemVendor = new ItemVendorSvr();
            DepartmentData dsDepartmentData = new DepartmentData();
            DepartmentSvr oDepartment = new DepartmentSvr();
            SubDepartmentData dsSubDepartmentData = new SubDepartmentData();
            SubDepartmentSvr oSubDepartment = new SubDepartmentSvr();
            //Added By Amit Date 29 Nov 2011
            InvRecvHeaderSvr oInvRecHeader = new InvRecvHeaderSvr();
            DataSet dsInvRecDate = new DataSet();
            //End

            //List<string> lstData = new List<string>();
            List<string> lstDeptData = new List<string>();
            List<string> lstSubDeptData = new List<string>();

            #region Sprint-19 - 1883 24-Mar-2015 JY Commented
            // if (this.txtDepartment.Text.Trim() != "" && txtDepartment.Tag.ToString() != ""
            //     && this.txtSubDepartment.Text.Trim() == "" && txtSubDepartment.Tag.ToString() == "")
            // {
            //     dsDepartmentData = oDepartment.PopulateList("where DeptCode in (" + this.txtDepartment.Tag.ToString().Trim() + ")");

            //     foreach (DataRow Dr in dsDepartmentData.Tables[0].Rows)
            //     {
            //         lstDeptData.Add("'" + Dr[0].ToString() + "'");
            //         Application.DoEvents();
            //     }
            //     dsItems = oItem.PopulateList(" where DepartmentID in (" + string.Join(",", lstDeptData.ToArray()) + ") and " + clsPOSDBConstants.Item_Fld_PreferredVendor + "!='' AND " + clsPOSDBConstants.Item_Fld_QtyInStock + " <= 0");
            //     dsSubDepartmentData = oSubDepartment.PopulateList("where DepartmentId in (" + string.Join(",", lstDeptData.ToArray()) + ")");
            // }
            // else if (this.txtDepartment.Text.Trim() == "" && txtDepartment.Tag.ToString() == ""
            //     && this.txtSubDepartment.Text.Trim() != "" && txtSubDepartment.Tag.ToString() != "")
            // {
            //     dsSubDepartmentData = oSubDepartment.PopulateList("where SubDepartmentId in (" + this.txtSubDepartment.Tag.ToString().Trim() + ")");
            //     foreach (DataRow Dr in dsSubDepartmentData.Tables[0].Rows)
            //     {
            //         lstSubDeptData.Add("'" + Dr[0].ToString() + "'");
            //         lstDeptData.Add("'" + Dr[1].ToString() + "'");
            //         Application.DoEvents();
            //     }
            //     dsDepartmentData = oDepartment.PopulateList("where DeptID in (" + string.Join(",", lstDeptData.ToArray()) + ")");
            //     dsItems = oItem.PopulateList(" where DepartmentID in (" + string.Join(",", lstDeptData.ToArray()) + ") and  " + clsPOSDBConstants.Item_Fld_PreferredVendor + "!='' AND " + clsPOSDBConstants.Item_Fld_QtyInStock + " <= 0");
            // }
            // else if (this.txtDepartment.Text.Trim() != "" && txtDepartment.Tag.ToString() != ""
            //&& this.txtSubDepartment.Text.Trim() != "" && txtSubDepartment.Tag.ToString() != "")
            // {
            //     dsDepartmentData = oDepartment.PopulateList("where DeptCode in (" + this.txtDepartment.Tag.ToString().Trim() + ")");

            //     foreach (DataRow Dr in dsDepartmentData.Tables[0].Rows)
            //     {
            //         lstDeptData.Add("'" + Dr[0].ToString() + "'");
            //         Application.DoEvents();
            //     }

            //     dsSubDepartmentData = oSubDepartment.PopulateList("where SubDepartmentId in (" + this.txtSubDepartment.Tag.ToString().Trim() + ")");
            //     foreach (DataRow Dr in dsSubDepartmentData.Tables[0].Rows)
            //     {
            //         lstSubDeptData.Add("'" + Dr[0].ToString() + "'");
            //         Application.DoEvents();
            //     }
            //     dsItems = oItem.PopulateList(" where DepartmentID in (" + string.Join(",", lstDeptData.ToArray()) + ") and SubDepartmentId in (" + this.txtSubDepartment.Tag.ToString().Trim() + ") and  " + clsPOSDBConstants.Item_Fld_PreferredVendor + "!='' AND " + clsPOSDBConstants.Item_Fld_QtyInStock + " <= 0");
            // }
            #endregion

            #region Sprint-19 - 1883 23-Mar-2015 JY Added location filter and also improved the existing filter code
            string strFilter = string.Empty;
            if (this.txtDepartment.Text.Trim() != "" && txtDepartment.Tag.ToString() != "")
            {
                dsDepartmentData = oDepartment.PopulateList("where DeptCode in (" + this.txtDepartment.Tag.ToString().Trim() + ")");

                foreach (DataRow Dr in dsDepartmentData.Tables[0].Rows)
                {
                    lstDeptData.Add("'" + Dr[0].ToString() + "'");
                }
                strFilter += " and DepartmentID in (" + string.Join(",", lstDeptData.ToArray()) + ")";

                if (this.txtSubDepartment.Text.Trim() != "" && txtSubDepartment.Tag.ToString() != "")
                {
                    dsSubDepartmentData = oSubDepartment.PopulateList("where SubDepartmentId in (" + this.txtSubDepartment.Tag.ToString().Trim() + ")");
                    foreach (DataRow Dr in dsSubDepartmentData.Tables[0].Rows)
                    {
                        lstSubDeptData.Add("'" + Dr[0].ToString() + "'");
                        lstDeptData.Add("'" + Dr[1].ToString() + "'");
                        Application.DoEvents();
                    }
                    strFilter += " and SubDepartmentID in (" + string.Join(",", lstSubDeptData.ToArray()) + ")";
                }
                else
                {
                    dsSubDepartmentData = oSubDepartment.PopulateList("where DepartmentId in (" + string.Join(",", lstDeptData.ToArray()) + ")");
                }
            }
            else
            {
                if (this.txtSubDepartment.Text.Trim() != "" && txtSubDepartment.Tag.ToString() != "")
                {
                    dsSubDepartmentData = oSubDepartment.PopulateList("where SubDepartmentId in (" + this.txtSubDepartment.Tag.ToString().Trim() + ")");
                    foreach (DataRow Dr in dsSubDepartmentData.Tables[0].Rows)
                    {
                        lstSubDeptData.Add("'" + Dr[0].ToString() + "'");
                        lstDeptData.Add("'" + Dr[1].ToString() + "'");
                        Application.DoEvents();
                    }
                    strFilter += " and SubDepartmentID in (" + string.Join(",", lstSubDeptData.ToArray()) + ")";
                    dsDepartmentData = oDepartment.PopulateList("where DeptID in (" + string.Join(",", lstDeptData.ToArray()) + ")");
                }
                else
                {
                    dsDepartmentData = (DepartmentData)oDepartment.PopulateList(" where 1=1 ");
                    dsSubDepartmentData = (SubDepartmentData)oSubDepartment.PopulateList(" where 1=1 ");
                }
            }

            if (this.txtItemLocation.Text.Trim() != "" && txtItemLocation.Tag.ToString() != "")
            {
                strFilter += " and LOCATION in (" + txtItemLocation.Text + ")";
            }
            dsItems = oItem.PopulateList(" where " + clsPOSDBConstants.Item_Fld_PreferredVendor + "!='' AND " + clsPOSDBConstants.Item_Fld_QtyInStock + " <= 0" + strFilter);
            #endregion

            #region Consider Transaction history in the item populate for Zero or -Ve qty logic
            //added By Shitaljit to consider Transaction history in Zero or Negative Qty.
            // JIRA link -PRIMEPOS -13
            string strDistItems = string.Empty;
            if (this.chckUseTransHistoryForReOrder.Checked == true)
            {
                dsDepartmentData = (DepartmentData)oDepartment.PopulateList(" where 1=1 ");
                dsSubDepartmentData = (SubDepartmentData)oSubDepartment.PopulateList(" where 1=1 ");
                string SelectClause = string.Empty;
                if (Configuration.CPrimeEDISetting.ConsiderReturnTrans)  //Sprint-27 - PRIMEPOS-2390 28-Sep-2017 JY Added    //PRIMEPOS-3167 07-Nov-2022 JY Modified 
                    SelectClause = " Trans.TransDate >= DateAdd(D,-" + this.numNoOfDaysForItemReOrder.Value.ToString() + ",getdate()) ";
                else
                    SelectClause = " Trans.TransDate >= DateAdd(D,-" + this.numNoOfDaysForItemReOrder.Value.ToString() + ",getdate()) AND Trans.TransType=1 ";
                Application.DoEvents();
                string strSQL = string.Empty;
                TransHeaderSvr oPOSTrans = new TransHeaderSvr();
                //DataSet dsTransHeader = oPOSTrans.PopulateList(SelectClause);   //PRIMEPOS-3097 28-Jun-2022 JY commented
                DataSet dsTransHeader = oPOSTrans.PopulateList(SelectClause, out strSQL);   //PRIMEPOS-3097 28-Jun-2022 JY Added
                if (dsTransHeader == null || dsTransHeader.Tables.Count == 0 || dsTransHeader.Tables[0].Rows.Count == 0)
                {
                    throw (new Exception("There are no more transactions in this last " + this.numNoOfDaysForItemReOrder.Value.ToString() + " day(s)."));
                }
                #region PRIMEPOS-3097 28-Jun-2022 JY Commented
                //foreach (DataRow Dr in dsTransHeader.Tables[0].Rows)
                //{
                //    lstData.Add(Dr[0].ToString());
                //}
                //string whereClause = " where TransID in (" + string.Join(",", lstData.ToArray()) + ")";
                //lstData.Clear();
                //foreach (DataRow Dr in dsItems.Tables[0].Rows)
                //{
                //    lstData.Add("'" + Dr[0].ToString() + "'");
                //    Application.DoEvents();
                //}
                //if (lstData.Count > 0)
                //{
                //    whereClause += "AND ItemID in (" + string.Join(",", lstData.ToArray()) + ")";
                //}
                //dsTransDetailData = oPOSTransDetail.PopulateList(whereClause);
                //lstData.Clear();
                //foreach (DataRow Dr in dsTransDetailData.Tables[0].Rows)
                //{
                //    lstData.Add("'" + Dr[0].ToString() + "'");
                //    Application.DoEvents();
                //}
                //dsItems = oItem.PopulateList(" where ItemID in (" + string.Join(",", lstData.ToArray()) + ") AND " + clsPOSDBConstants.Item_Fld_QtyInStock + " <= 0");
                #endregion

                #region PRIMEPOS-3097 28-Jun-2022 JY Added                
                dsTransDetailData = oPOSTransDetail.PopulateList(" where TransID in (" + strSQL + ") AND ItemID in (SELECT ItemID FROM Item where PreferredVendor != '' AND QtyInStock <= 0)", out queryResult, out strDistItems);
                dsItems = oItem.PopulateList(" where ItemID in (" + strDistItems + ") AND PreferredVendor != '' AND QtyInStock <= 0");
                #endregion
            }
            else
            {
                dsDepartmentData = (DepartmentData)oDepartment.PopulateList(" where 1=1 ");
                dsSubDepartmentData = (SubDepartmentData)oSubDepartment.PopulateList(" where 1=1 ");
                dsItems = oItem.PopulateList(" where " + clsPOSDBConstants.Item_Fld_QtyInStock + " <= 0");
                #region PRIMEPOS-3097 28-Jun-2022 JY Commented
                //foreach (DataRow Dr in dsItems.Tables[0].Rows)
                //{
                //    lstData.Add("'" + Dr[0].ToString() + "'");
                //    Application.DoEvents();
                //}
                #endregion
                //Added By Amit Date 4 Aug 2011
                //dsTransDetailData = oPOSTransDetail.PopulateList(" where ItemID in (" + string.Join(",", lstData.ToArray()) + ")"); //PRIMEPOS-3097 28-Jun-2022 JY commented
                dsTransDetailData = oPOSTransDetail.PopulateList(" where ItemID in (SELECT ItemID FROM Item where PreferredVendor != '' AND QtyInStock <= 0)", out queryResult, out strDistItems);    //PRIMEPOS-3097 28-Jun-2022 JY Added
                if (dsTransDetailData == null || dsTransDetailData.Tables.Count == 0 || dsTransDetailData.Tables[0].Rows.Count == 0)
                {
                    throw (new Exception("There are no items found which are used in Transaction."));
                }
                //End
            }
            #endregion
            //if (dsItems == null || dsItems.Tables.Count == 0 || dsItems.Tables[0].Rows.Count == 0)
            //{
            //    throw (new Exception("There are no items found."));
            //}
            //if (lstData.Count > 0)
            //    dsItemVendor = oItemVendor.PopulateList(" where ItemId in (" + string.Join(",", lstData.ToArray()) + ")");    //PRIMEPOS-3097 28-Jun-2022 JY commented
            dsItemVendor = oItemVendor.PopulateList(" where ItemId in (" + strDistItems + ")", true);  //PRIMEPOS-3097 28-Jun-2022 JY Added
            //if (dsItemVendor == null || dsItemVendor.Tables.Count == 0 || dsItemVendor.Tables[0].Rows.Count == 0)
            //{
            //    throw (new Exception("There are no more vendor items matching preferred vendor found."));
            //}
            //int count = 0;

            //Added By Amit Date 29 Nov 2011
            //dsInvRecDate = oInvRecHeader.ItemInvRecDate(lstData); //PRIMEPOS-3097 28-Jun-2022 JY Commented
            dsInvRecDate = oInvRecHeader.ItemInvRecDate(strDistItems);   //PRIMEPOS-3097 28-Jun-2022 JY Added
            //End
            podTable = new PODetailTable();
            PODetailRow podRow = null;
            //Added by amit date 1 jul 2011
            int index = 0;

            //foreach (ItemRow oItemRow in dsItems.Tables[0].Rows)
            //Added By Amit date 29 July 2011            
            foreach (DataRow podRowTemp in dsTransDetailData.Tables[0].Rows)
            {
                DataRow[] oItemRowTemp = dsItems.Tables[0].Select("ItemID='" + podRowTemp["ItemID"] + "'");
                ItemRow oItemRow = null;

                if (oItemRowTemp.Length > 0)
                    oItemRow = (ItemRow)oItemRowTemp[0];
                else
                    continue;
                //End

                lblMessage.Text = "Scanning Item #" + oItemRow.ItemID.Trim() + " For Preferred Vendor";
                Application.DoEvents();
                DataRow[] oItemRows = dsItemVendor.Tables[0].Select(" ItemId='" + oItemRow.ItemID.Trim() + "'");
                if (oItemRows != null && oItemRows.Length > 0)
                {
                    ItemVendorRow oItemVendorRow = (ItemVendorRow)oItemRows[0];
                    podRow = dsPoDetails.PODetail.NewPODetailRow();
                    podRow.ItemID = oItemRow.ItemID;
                    podRow.ItemDescription = oItemRow.Description;
                    podRow.VendorID = oItemVendorRow.VendorID;
                    podRow.VendorItemCode = oItemVendorRow.VendorItemID;
                    podRow.VendorName = oItemVendorRow.VendorCode;
                    podRow.QTY = podRow.QTY = Configuration.convertNullToInt(podRowTemp["Qty"].ToString());
                    podRow.QtyInStock = oItemRow.QtyInStock;
                    podRow.ReOrderLevel = oItemRow.ReOrderLevel;
                    //Modified by shitaljit as per Fahad for those Pharmacy which are not using PO 
                    //We should pupulate item cost from Item table not from ItemVendor table.
                    podRow.SoldItems = Configuration.convertNullToInt(podRowTemp["QTY"].ToString());
                    if (isFromPO == true)
                    {
                        podRow.Price = oItemVendorRow.VenorCostPrice;
                    }
                    else
                    {
                        podRow.Price = oItemRow.LastCostPrice;
                    }

                    podRow.PacketSize = oItemRow.PckSize;
                    podRow.PacketQuant = oItemRow.PckQty;
                    podRow.Packetunit = oItemRow.PckUnit;
                    podRow.MinOrdQty = oItemRow.MinOrdQty;

                    DataRow[] DeptRow = dsDepartmentData.Tables[0].Select("DeptID=" + oItemRow.DepartmentID);
                    if (DeptRow.Length != 0)
                        podRow.DeptName = DeptRow[0][2].ToString();

                    DataRow[] SubDeptRow = dsSubDepartmentData.Tables[0].Select("SubDepartmentID=" + oItemRow.SubDepartmentID);

                    if (SubDeptRow.Length != 0)
                        podRow.SubDeptName = SubDeptRow[0][2].ToString();
                    //Added by Amit Date 4 Aug 2011
                    podRow.RetailPrice = Configuration.convertNullToDecimal(podRowTemp["Price"].ToString());
                    podRow.ItemPrice = Configuration.convertNullToDecimal(podRowTemp["Price"].ToString()) - Configuration.convertNullToDecimal(podRowTemp["Discount"].ToString());
                    podRow.Discount = Configuration.convertNullToDecimal(podRowTemp["Discount"].ToString());
                    //End
                    //29 Nov 2011
                    DataRow[] InvRecDateRow = dsInvRecDate.Tables[0].Select(" ItemId='" + oItemRow.ItemID.Trim() + "'");
                    if (InvRecDateRow.Length != 0)
                        podRow.InvRecDate = (System.DateTime)InvRecDateRow[0][clsPOSDBConstants.PODetail_Fld_InvRecDate];
                    //End

                    dsPoDetails.PODetail.AddRow(podRow);

                    index++;
                }
                Application.DoEvents();
            }
            SetPacketToOrder(ref dsPoDetails);
            podTable = dsPoDetails.PODetail;
            if (dsPoDetails.PODetail.Rows.Count > 0)
            {
                //lblMessage.Text = podTable.Rows.Count.ToString() + " No(s) Items Acquired For Purchase Order By Preferred Vendor";
                try
                {
                    DataView view = new DataView(dsPoDetails.PODetail);
                    DataTable distinctValues = view.ToTable(true, "ItemID");
                    lblMessage.Text = distinctValues.Rows.Count.ToString() + " No(s) Items Acquired For Purchase Order By Preferred Vendor";
                }
                catch
                {
                    lblMessage.Text = podTable.Rows.Count.ToString() + " No(s) Items Acquired For Purchase Order By Preferred Vendor";
                }
                Application.DoEvents();
            }
            return podTable;
        }
        //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        private PODetailTable CreateAutoPOByLastVendorReOrder()
        {
            bool queryResult = false;

            PODetailTable podTable = null;

            DataSet dsTransHeader = new DataSet();
            DataSet dsTransDetail = new DataSet();
            ItemData dsItems = new ItemData();
            ItemVendorData dsItemVendor = new ItemVendorData();
            DepartmentData dsDepartment = new DepartmentData();
            SubDepartmentData dsSubDepartment = new SubDepartmentData();
            DataSet dsInvRecDate = new DataSet();

            GetDsForReorderLevel("Last Vendor", txtDepartment.Tag.ToString().Trim(), txtSubDepartment.Tag.ToString().Trim(), out dsTransHeader, out dsTransDetail, out dsItems, out dsItemVendor, out dsDepartment, out dsSubDepartment, out dsInvRecDate, out queryResult);

            #region Commented
            //bool queryResult = false;

            //PODetailTable podTable = null;

            //DataSet dsTransHeader = new DataSet();
            //DataSet dsTransDetailData = new DataSet();
            //ItemVendorData dsItemVendor = new ItemVendorData();
            //ItemData dsItems = new ItemData();

            //TransHeaderSvr oPOSTrans = new TransHeaderSvr();
            //TransDetailSvr oPOSTransDetail = new TransDetailSvr();
            //ItemSvr oItem = new ItemSvr();
            //ItemVendorSvr oItemVendor = new ItemVendorSvr();

            ////Added by Amit Date 30 June 2011
            //DepartmentData dsDepartmentData = new DepartmentData();
            //DepartmentSvr oDepartment = new DepartmentSvr();
            //SubDepartmentData dsSubDepartmentData = new SubDepartmentData();
            //SubDepartmentSvr oSubDepartment = new SubDepartmentSvr();
            ////End

            //string SelectClause = string.Empty;

            ////changed by SRT(Abhishek)  Date : 21 Sept. 2009
            ////Added condition to check if Transaction should be of Sell Type
            //SelectClause = " Trans.TransDate >= DateAdd(D,-" + clsPOSDBConstants.ItemReorderPeriod.ToString() + ",getdate()) AND Trans.TransType=1";
            ////End Of changed by SRT(Abhishek)  Date : 21 Sept. 2009

            //lblMessage.Text = "Aquiring Transactions For Last Vendor.";
            //Application.DoEvents();

            //dsTransHeader = oPOSTrans.PopulateList(SelectClause);
            //if (dsTransHeader == null || dsTransHeader.Tables.Count == 0 || dsTransHeader.Tables[0].Rows.Count == 0)
            //{
            //    throw (new Exception("There are no more transactions in this last " + clsPOSDBConstants.ItemReorderPeriod.ToString() + " day(s)."));
            //}

            //List<string> lstData = new List<string>();
            //foreach (DataRow Dr in dsTransHeader.Tables[0].Rows)
            //{
            //    lstData.Add(Dr[0].ToString());
            //    Application.DoEvents();
            //}

            //dsTransDetailData = oPOSTransDetail.PopulateList(" where TransID in (" + string.Join(",", lstData.ToArray()) + ")", out queryResult);
            //lstData = new List<string>();
            //foreach (DataRow Dr in dsTransDetailData.Tables[0].Rows)
            //{
            //    lstData.Add("'" + Dr[0].ToString() + "'");
            //    Application.DoEvents();
            //}
            //lblMessage.Text = "Aquiring Transaction Items For Last Vendor.";
            //Application.DoEvents();

            //Start: Added By Amit Date 30 June 2011
            // List<string> lstDeptData = new List<string>();
            // List<string> lstSubDeptData = new List<string>();

            // if (this.txtDepartment.Text.Trim() != "" && txtDepartment.Tag.ToString() != ""
            //     && this.txtSubDepartment.Text.Trim() == "" && txtSubDepartment.Tag.ToString() == "")
            // {
            //     dsDepartmentData = oDepartment.PopulateList("where DeptCode in (" + this.txtDepartment.Tag.ToString().Trim() + ")");

            //     foreach (DataRow Dr in dsDepartmentData.Tables[0].Rows)
            //     {
            //         lstDeptData.Add("'" + Dr[0].ToString() + "'");
            //         Application.DoEvents();
            //     }
            //     dsSubDepartmentData = oSubDepartment.PopulateList("where DepartmentId in (" + string.Join(",", lstDeptData.ToArray()) + ")");

            //     dsItems = oItem.PopulateList(" where ItemId in (" + string.Join(",", lstData.ToArray()) + ") and DepartmentID in (" + string.Join(",", lstDeptData.ToArray()) + ")");
            // }
            // else if (this.txtDepartment.Text.Trim() == "" && txtDepartment.Tag.ToString() == ""
            //     && this.txtSubDepartment.Text.Trim() != "" && txtSubDepartment.Tag.ToString() != "")
            // {
            //     dsSubDepartmentData = oSubDepartment.PopulateList("where SubDepartmentId in (" + this.txtSubDepartment.Tag.ToString().Trim() + ")");
            //     foreach (DataRow Dr in dsSubDepartmentData.Tables[0].Rows)
            //     {
            //         lstSubDeptData.Add("'" + Dr[0].ToString() + "'");
            //         lstDeptData.Add("'" + Dr[1].ToString() + "'");
            //         Application.DoEvents();
            //     }
            //     dsDepartmentData = oDepartment.PopulateList("where DeptID in (" + string.Join(",", lstDeptData.ToArray()) + ")");

            //     dsItems = oItem.PopulateList(" where ItemId in (" + string.Join(",", lstData.ToArray()) + ") and SubDepartmentID in (" + string.Join(",", lstSubDeptData.ToArray()) + ")");
            // }
            // else if (this.txtDepartment.Text.Trim() != "" && txtDepartment.Tag.ToString() != ""
            //&& this.txtSubDepartment.Text.Trim() != "" && txtSubDepartment.Tag.ToString() != "")
            // {
            //     dsDepartmentData = oDepartment.PopulateList("where DeptCode in (" + this.txtDepartment.Tag.ToString().Trim() + ")");

            //     foreach (DataRow Dr in dsDepartmentData.Tables[0].Rows)
            //     {
            //         lstDeptData.Add("'" + Dr[0].ToString() + "'");
            //         Application.DoEvents();
            //     }

            //     dsSubDepartmentData = oSubDepartment.PopulateList("where SubDepartmentId in (" + this.txtSubDepartment.Tag.ToString().Trim() + ")");
            //     foreach (DataRow Dr in dsSubDepartmentData.Tables[0].Rows)
            //     {
            //         lstSubDeptData.Add("'" + Dr[0].ToString() + "'");
            //         Application.DoEvents();
            //     }

            //     dsItems = oItem.PopulateList(" where ItemId in (" + string.Join(",", lstData.ToArray()) + ") and DepartmentID in (" + string.Join(",", lstDeptData.ToArray()) + ") and SubDepartmentID in (" + string.Join(",", lstSubDeptData.ToArray()) + ")");

            // }
            // else
            // {
            //     dsDepartmentData = (DepartmentData)oDepartment.PopulateList(" where 1=1 ");
            //     dsSubDepartmentData = (SubDepartmentData)oSubDepartment.PopulateList(" where 1=1 ");

            //     dsItems = oItem.PopulateList(" where ItemId in (" + string.Join(",", lstData.ToArray()) + ")");
            // }
            // //End

            // dsItemVendor = oItemVendor.PopulateList(" where ItemId in (" + string.Join(",", lstData.ToArray()) + ")");
            // if (dsItems == null || dsItems.Tables.Count == 0 || dsItems.Tables[0].Rows.Count == 0)
            // {
            //     throw (new Exception("There are no more items matching transactions of selected period."));
            // }

            // if (dsItemVendor == null || dsItemVendor.Tables.Count == 0 || dsItemVendor.Tables[0].Rows.Count == 0)
            // {
            //     throw (new Exception("There are no more vendor items matching transactions of selected period."));
            // }
            #endregion

            //int count = 0;
            podTable = new PODetailTable();
            PODetailRow podRow = null;
            //Added By Amit Date 30 June 2011
            int index = 0;

            DataSet dtItems = null;

            if (this.chckUseTransHistoryForReOrder.Checked == true)
            {
                dtItems = dsTransDetail;
            }
            else if (dsItems != null)
            {
                if (dsItems.Tables[0].Rows.Count > 0)
                {
                    dtItems = dsItems;
                }
                else
                {
                    throw (new Exception("There are no items to "));
                }

            }
            //Added By Amit date 29 July 2011            
            foreach (DataRow podRowTemp in dtItems.Tables[0].Rows)
            {
                DataRow[] oItemRowTemp = dsItems.Tables[0].Select("ItemID='" + podRowTemp["ItemID"] + "'");
                ItemRow oItemRow = null;

                if (oItemRowTemp.Length > 0)
                    oItemRow = (ItemRow)oItemRowTemp[0];
                else
                    continue;

                lblMessage.Text = "Scanning Item #" + oItemRow.ItemID.Trim() + " For Last Vendor";
                Application.DoEvents();
                int TEMPVEND = 0;
                //if (Configuration.convertNullToString(oItemRow.LastVendor).Trim().Length > 0 && !oItemRow.ExclFromAutoPO && oItemRow.QtyInStock < oItemRow.ReOrderLevel && oItemRow.ReOrderLevel > 0 && int.TryParse(oItemRow.LastVendor.ToString(),out TEMPVEND))//isnull(itm.ExclFromAutoPO,0) = 0
                if (Configuration.convertNullToString(oItemRow.LastVendor).Trim().Length > 0)//isnull(itm.ExclFromAutoPO,0) = 0
                {
                    DataRow[] oItemRows = dsItemVendor.Tables[0].Select(" ItemId='" + oItemRow.ItemID.Trim() + "' AND Trim(VendorCode)='" + oItemRow.LastVendor.ToString().Trim().Replace("'", "''") + "'");

                    if (oItemRows != null && oItemRows.Length > 0)
                    {
                        ItemVendorRow oItemVendorRow = (ItemVendorRow)oItemRows[0];
                        DataRow[] TransRows = dtItems.Tables[0].Select(" ItemId='" + oItemRow.ItemID.Trim() + "' ");

                        if (TransRows != null && TransRows.Length > 0)
                        {
                            podRow = dsPoDetails.PODetail.NewPODetailRow();
                            podRow.ItemID = oItemRow.ItemID;
                            podRow.ItemDescription = oItemRow.Description;
                            podRow.VendorID = oItemVendorRow.VendorID;
                            podRow.VendorItemCode = oItemVendorRow.VendorItemID;
                            podRow.VendorName = oItemVendorRow.VendorCode;

                            //if (!isFromPO)
                            //{
                            //    podRow.QTY = Configuration.convertNullToInt(podRowTemp["QTY"].ToString());
                            //}
                            //else
                            //{
                            //    //podRow.QTY = oItemRow.MinOrdQty;
                            //    podRow.QTY = oItemRow.MinOrdQty - oItemRow.QtyInStock;
                            //}

                            //Added by Krishna on 19 December 2011
                            podRow.QTY = Configuration.convertNullToInt(oItemRow.MinOrdQty) - Configuration.convertNullToInt(oItemRow.QtyInStock);
                            if (this.chckUseTransHistoryForReOrder.Checked == true)
                            {
                                podRow.SoldItems = Configuration.convertNullToInt(podRowTemp["QTY"].ToString());
                                //Added by Amit Date 27 july 2011
                                podRow.RetailPrice = Configuration.convertNullToDecimal(podRowTemp["Price"].ToString());
                                podRow.ItemPrice = Configuration.convertNullToDecimal(podRowTemp["Price"].ToString()) - Configuration.convertNullToDecimal(podRowTemp["Discount"].ToString());
                                podRow.Discount = Configuration.convertNullToDecimal(podRowTemp["Discount"].ToString());
                                //End
                            }
                            //End of Added by Krishna
                            podRow.QtyInStock = oItemRow.QtyInStock;
                            podRow.ReOrderLevel = oItemRow.ReOrderLevel;
                            //Modified by shitaljit as per Fahad for those Pharmacy which are not using PO 
                            //We should pupulate item cost from Item table not from ItemVendor table.
                            if (isFromPO == true)
                            {
                                podRow.Price = oItemVendorRow.VenorCostPrice;
                            }
                            else
                            {
                                podRow.Price = oItemRow.LastCostPrice;
                            }

                            podRow.PacketSize = oItemRow.PckSize;
                            podRow.PacketQuant = oItemRow.PckQty;
                            podRow.Packetunit = oItemRow.PckUnit;
                            //Start : Added By Amit Date 5 jul 2011
                            podRow.MinOrdQty = oItemRow.MinOrdQty;
                            DataRow[] DeptRow = dsDepartment.Tables[0].Select("DeptID=" + oItemRow.DepartmentID);
                            if (DeptRow.Length != 0)
                                podRow.DeptName = DeptRow[0][2].ToString();

                            DataRow[] SubDeptRow = dsSubDepartment.Tables[0].Select("SubDepartmentID=" + oItemRow.SubDepartmentID);

                            if (SubDeptRow.Length != 0)
                                podRow.SubDeptName = SubDeptRow[0][2].ToString();
                            //End

                            //29 Nov 2011
                            DataRow[] InvRecDateRow = dsInvRecDate.Tables[0].Select(" ItemId='" + oItemRow.ItemID.Trim() + "'");
                            if (InvRecDateRow.Length != 0)
                                podRow.InvRecDate = (System.DateTime)InvRecDateRow[0][clsPOSDBConstants.PODetail_Fld_InvRecDate];
                            //End

                            dsPoDetails.PODetail.AddRow(podRow);

                            index++;
                        }
                    }
                }
                Application.DoEvents();
            }
            SetPacketToOrder(ref dsPoDetails);

            if (dsPoDetails.PODetail.Rows.Count > 0)
            {
                if (optionReport.CheckedIndex == 0)
                {
                    PODetailData dsTempPoDetails = new PODetailData();
                    DataTable dtTemp = dsPoDetails.PODetail.DefaultView.ToTable(true, "ItemId");

                    lblMessage.Text = dtTemp.Rows.Count.ToString() + " No(s) Items Acquired For Purchase Order By Last Vendor";
                    Application.DoEvents();
                }
                else
                {
                    lblMessage.Text = dsPoDetails.PODetail.Rows.Count.ToString() + " No(s) Items Acquired For Purchase Order By Last Vendor";
                    Application.DoEvents();
                }
            }
            return podTable;
        }

        //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Added By Amit Date 1 July 2011
        /// Last Vendor Qty Zero
        /// </summary>        
        /// <returns></returns>
        private PODetailTable CreateAutoPOByLastVendorQtyZero()
        {
            bool queryResult = false;
            PODetailTable podTable = null;
            //Added By Amit Date 4 Aug 2011
            DataSet dsTransDetailData = new DataSet();
            TransDetailSvr oPOSTransDetail = new TransDetailSvr();
            //End
            ItemVendorData dsItemVendor = new ItemVendorData();
            ItemData dsItems = new ItemData();
            ItemSvr oItem = new ItemSvr();
            ItemVendorSvr oItemVendor = new ItemVendorSvr();
            DepartmentData dsDepartmentData = new DepartmentData();
            DepartmentSvr oDepartment = new DepartmentSvr();
            SubDepartmentData dsSubDepartmentData = new SubDepartmentData();
            SubDepartmentSvr oSubDepartment = new SubDepartmentSvr();
            //Added By Amit Date 29 Nov 2011
            InvRecvHeaderSvr oInvRecHeader = new InvRecvHeaderSvr();
            DataSet dsInvRecDate = new DataSet();
            //End

            //List<string> lstData = new List<string>();
            List<string> lstDeptData = new List<string>();
            List<string> lstSubDeptData = new List<string>();

            #region Sprint-19 - 1883 24-Mar-2015 JY Commented
            // if (this.txtDepartment.Text.Trim() != "" && txtDepartment.Tag.ToString() != ""
            //     && this.txtSubDepartment.Text.Trim() == "" && txtSubDepartment.Tag.ToString() == "")
            // {
            //     dsDepartmentData = oDepartment.PopulateList("where DeptCode in (" + this.txtDepartment.Tag.ToString().Trim() + ")");

            //     foreach (DataRow Dr in dsDepartmentData.Tables[0].Rows)
            //     {
            //         lstDeptData.Add("'" + Dr[0].ToString() + "'");
            //         Application.DoEvents();
            //     }
            //     dsItems = oItem.PopulateList(" where DepartmentID in (" + string.Join(",", lstDeptData.ToArray()) + ") and " + clsPOSDBConstants.Item_Fld_LastVendor + "!='' AND " + clsPOSDBConstants.Item_Fld_QtyInStock + " <= 0");
            //     dsSubDepartmentData = oSubDepartment.PopulateList("where DepartmentId in (" + string.Join(",", lstDeptData.ToArray()) + ")");


            // }
            // else if (this.txtDepartment.Text.Trim() == "" && txtDepartment.Tag.ToString() == ""
            //     && this.txtSubDepartment.Text.Trim() != "" && txtSubDepartment.Tag.ToString() != "")
            // {
            //     dsSubDepartmentData = oSubDepartment.PopulateList("where SubDepartmentId in (" + this.txtSubDepartment.Tag.ToString().Trim() + ")");
            //     foreach (DataRow Dr in dsSubDepartmentData.Tables[0].Rows)
            //     {
            //         lstSubDeptData.Add("'" + Dr[0].ToString() + "'");
            //         lstDeptData.Add("'" + Dr[1].ToString() + "'");
            //         Application.DoEvents();
            //     }
            //     dsDepartmentData = oDepartment.PopulateList("where DeptID in (" + string.Join(",", lstDeptData.ToArray()) + ")");
            //     dsItems = oItem.PopulateList(" where DepartmentID in (" + string.Join(",", lstDeptData.ToArray()) + ") and  " + clsPOSDBConstants.Item_Fld_LastVendor + "!='' AND " + clsPOSDBConstants.Item_Fld_QtyInStock + " <= 0");

            // }
            // else if (this.txtDepartment.Text.Trim() != "" && txtDepartment.Tag.ToString() != ""
            //&& this.txtSubDepartment.Text.Trim() != "" && txtSubDepartment.Tag.ToString() != "")
            // {
            //     dsDepartmentData = oDepartment.PopulateList("where DeptCode in (" + this.txtDepartment.Tag.ToString().Trim() + ")");

            //     foreach (DataRow Dr in dsDepartmentData.Tables[0].Rows)
            //     {
            //         lstDeptData.Add("'" + Dr[0].ToString() + "'");
            //         Application.DoEvents();
            //     }

            //     dsSubDepartmentData = oSubDepartment.PopulateList("where SubDepartmentId in (" + this.txtSubDepartment.Tag.ToString().Trim() + ")");
            //     foreach (DataRow Dr in dsSubDepartmentData.Tables[0].Rows)
            //     {
            //         lstSubDeptData.Add("'" + Dr[0].ToString() + "'");
            //         Application.DoEvents();
            //     }
            //     dsItems = oItem.PopulateList(" where DepartmentID in (" + string.Join(",", lstDeptData.ToArray()) + ") and SubDepartmentId in (" + this.txtSubDepartment.Tag.ToString().Trim() + ") and  " + clsPOSDBConstants.Item_Fld_LastVendor + "!='' AND " + clsPOSDBConstants.Item_Fld_QtyInStock + " <= 0");
            // }
            #endregion

            #region Sprint-19 - 1883 23-Mar-2015 JY Added location filter and also improved the existing filter code
            string strFilter = string.Empty;

            if (this.txtDepartment.Text.Trim() != "" && txtDepartment.Tag.ToString() != "")
            {
                dsDepartmentData = oDepartment.PopulateList("where DeptCode in (" + this.txtDepartment.Tag.ToString().Trim() + ")");

                foreach (DataRow Dr in dsDepartmentData.Tables[0].Rows)
                {
                    lstDeptData.Add("'" + Dr[0].ToString() + "'");
                }
                strFilter += " and DepartmentID in (" + string.Join(",", lstDeptData.ToArray()) + ")";

                if (this.txtSubDepartment.Text.Trim() != "" && txtSubDepartment.Tag.ToString() != "")
                {
                    dsSubDepartmentData = oSubDepartment.PopulateList("where SubDepartmentId in (" + this.txtSubDepartment.Tag.ToString().Trim() + ")");
                    foreach (DataRow Dr in dsSubDepartmentData.Tables[0].Rows)
                    {
                        lstSubDeptData.Add("'" + Dr[0].ToString() + "'");
                        lstDeptData.Add("'" + Dr[1].ToString() + "'");
                        Application.DoEvents();
                    }
                    strFilter += " and SubDepartmentID in (" + string.Join(",", lstSubDeptData.ToArray()) + ")";
                }
                else
                {
                    dsSubDepartmentData = oSubDepartment.PopulateList("where DepartmentId in (" + string.Join(",", lstDeptData.ToArray()) + ")");
                }
            }
            else
            {
                if (this.txtSubDepartment.Text.Trim() != "" && txtSubDepartment.Tag.ToString() != "")
                {
                    dsSubDepartmentData = oSubDepartment.PopulateList("where SubDepartmentId in (" + this.txtSubDepartment.Tag.ToString().Trim() + ")");
                    foreach (DataRow Dr in dsSubDepartmentData.Tables[0].Rows)
                    {
                        lstSubDeptData.Add("'" + Dr[0].ToString() + "'");
                        lstDeptData.Add("'" + Dr[1].ToString() + "'");
                        Application.DoEvents();
                    }
                    strFilter += " and SubDepartmentID in (" + string.Join(",", lstSubDeptData.ToArray()) + ")";
                    dsDepartmentData = oDepartment.PopulateList("where DeptID in (" + string.Join(",", lstDeptData.ToArray()) + ")");
                }
                else
                {
                    dsDepartmentData = (DepartmentData)oDepartment.PopulateList(" where 1=1 ");
                    dsSubDepartmentData = (SubDepartmentData)oSubDepartment.PopulateList(" where 1=1 ");
                }
            }

            if (this.txtItemLocation.Text.Trim() != "" && txtItemLocation.Tag.ToString() != "")
            {
                strFilter += " and LOCATION in (" + txtItemLocation.Text + ")";
            }
            dsItems = oItem.PopulateList(" where " + clsPOSDBConstants.Item_Fld_LastVendor + "!='' AND " + clsPOSDBConstants.Item_Fld_QtyInStock + " <= 0" + strFilter);
            #endregion

            #region Consider Transaction history in the item populate for Zero or -Ve qty logic
            //added By Shitaljit to consider Transaction history in Zero or Negative Qty.
            // JIRA link -PRIMEPOS -13
            string strDistItems = string.Empty;
            if (this.chckUseTransHistoryForReOrder.Checked == true)
            {
                dsDepartmentData = (DepartmentData)oDepartment.PopulateList(" where 1=1 ");
                dsSubDepartmentData = (SubDepartmentData)oSubDepartment.PopulateList(" where 1=1 ");
                string SelectClause = string.Empty;
                if (Configuration.CPrimeEDISetting.ConsiderReturnTrans)  //Sprint-27 - PRIMEPOS-2390 28-Sep-2017 JY Added    //PRIMEPOS-3167 07-Nov-2022 JY Modified 
                    SelectClause = " Trans.TransDate >= DateAdd(D,-" + this.numNoOfDaysForItemReOrder.Value.ToString() + ",getdate()) ";
                else
                    SelectClause = " Trans.TransDate >= DateAdd(D,-" + this.numNoOfDaysForItemReOrder.Value.ToString() + ",getdate()) AND Trans.TransType=1 ";
                Application.DoEvents();
                string strSQL = string.Empty;
                TransHeaderSvr oPOSTrans = new TransHeaderSvr();
                //DataSet dsTransHeader = oPOSTrans.PopulateList(SelectClause);   //PRIMEPOS-3097 28-Jun-2022 JY commented
                DataSet dsTransHeader = oPOSTrans.PopulateList(SelectClause, out strSQL);   //PRIMEPOS-3097 28-Jun-2022 JY Added
                if (dsTransHeader == null || dsTransHeader.Tables.Count == 0 || dsTransHeader.Tables[0].Rows.Count == 0)
                {
                    throw (new Exception("There are no more transactions in this last " + this.numNoOfDaysForItemReOrder.Value.ToString() + " day(s)."));
                }

                #region PRIMEPOS-3097 28-Jun-2022 JY Commented
                //foreach (DataRow Dr in dsTransHeader.Tables[0].Rows)
                //{
                //    lstData.Add(Dr[0].ToString());
                //}
                //string whereClause = " where TransID in (" + string.Join(",", lstData.ToArray()) + ")";
                //lstData.Clear();
                //foreach (DataRow Dr in dsItems.Tables[0].Rows)
                //{
                //    lstData.Add("'" + Dr[0].ToString() + "'");
                //    Application.DoEvents();
                //}
                //if (lstData.Count > 0)
                //{
                //    whereClause += "AND ItemID in (" + string.Join(",", lstData.ToArray()) + ")";
                //}
                //dsTransDetailData = oPOSTransDetail.PopulateList(whereClause);
                //lstData.Clear();
                //foreach (DataRow Dr in dsTransDetailData.Tables[0].Rows)
                //{
                //    lstData.Add("'" + Dr[0].ToString() + "'");
                //    Application.DoEvents();
                //}
                //dsItems = oItem.PopulateList(" where ItemID in (" + string.Join(",", lstData.ToArray()) + ") AND " + clsPOSDBConstants.Item_Fld_QtyInStock + " <= 0");
                #endregion
                #region PRIMEPOS-3097 28-Jun-2022 JY Added
                dsTransDetailData = oPOSTransDetail.PopulateList(" where TransID in (" + strSQL + ") AND ItemID in (SELECT ItemID FROM Item where LastVendor != '' AND QtyInStock <= 0)", out queryResult, out strDistItems);
                dsItems = oItem.PopulateList(" where ItemID in (" + strDistItems + ") AND " + clsPOSDBConstants.Item_Fld_QtyInStock + " <= 0");
                #endregion
            }
            else
            {
                dsDepartmentData = (DepartmentData)oDepartment.PopulateList(" where 1=1 ");
                dsSubDepartmentData = (SubDepartmentData)oSubDepartment.PopulateList(" where 1=1 ");
                dsItems = oItem.PopulateList(" where " + clsPOSDBConstants.Item_Fld_QtyInStock + " <= 0");
                #region PRIMEPOS-3097 28-Jun-2022 JY Commented
                //foreach (DataRow Dr in dsItems.Tables[0].Rows)
                //{
                //    lstData.Add("'" + Dr[0].ToString() + "'");
                //    Application.DoEvents();
                //}
                #endregion
                //Added By Amit Date 4 Aug 2011
                //dsTransDetailData = oPOSTransDetail.PopulateList(" where ItemID in (" + string.Join(",", lstData.ToArray()) + ")"); //PRIMEPOS-3097 28-Jun-2022 JY commented
                dsTransDetailData = oPOSTransDetail.PopulateList(" where ItemID in (SELECT ItemID FROM Item where LastVendor != '' AND QtyInStock <= 0)", out queryResult, out strDistItems);    //PRIMEPOS-3097 28-Jun-2022 JY Added
                if (dsTransDetailData == null || dsTransDetailData.Tables.Count == 0 || dsTransDetailData.Tables[0].Rows.Count == 0)
                {
                    throw (new Exception("There are no items found which are used in Transaction."));
                }
                //End
            }
            #endregion
            //dsItemVendor = oItemVendor.PopulateList(" where ItemId in (" + string.Join(",", lstData.ToArray()) + ")");  //PRIMEPOS-3097 28-Jun-2022 JY commented
            dsItemVendor = oItemVendor.PopulateList(" where ItemId in (" + strDistItems + ")", true);  //PRIMEPOS-3097 28-Jun-2022 JY Added
            if (dsItems == null || dsItems.Tables.Count == 0 || dsItems.Tables[0].Rows.Count == 0)
            {
                throw (new Exception("There are no items found."));
            }

            if (dsItemVendor == null || dsItemVendor.Tables.Count == 0 || dsItemVendor.Tables[0].Rows.Count == 0)
            {
                throw (new Exception("There are no more vendor items matching LastVendor vendor found."));
            }
            //Added By Amit Date 29 Nov 2011
            //dsInvRecDate = oInvRecHeader.ItemInvRecDate(lstData);   //PRIMEPOS-3097 28-Jun-2022 JY Commented
            dsInvRecDate = oInvRecHeader.ItemInvRecDate(strDistItems);   //PRIMEPOS-3097 28-Jun-2022 JY Added
            //End
            //int count = 0;
            podTable = new PODetailTable();
            PODetailRow podRow = null;
            int index = 0;

            //foreach (ItemRow oItemRow in dsItems.Tables[0].Rows)
            //Added By Amit date 29 July 2011            
            foreach (DataRow podRowTemp in dsTransDetailData.Tables[0].Rows)
            {
                DataRow[] oItemRowTemp = dsItems.Tables[0].Select("ItemID='" + podRowTemp["ItemID"] + "'");
                ItemRow oItemRow = null;

                if (oItemRowTemp.Length > 0)
                    oItemRow = (ItemRow)oItemRowTemp[0];
                else
                    continue;
                //End
                lblMessage.Text = "Scanning Item #" + oItemRow.ItemID.Trim() + " For Last Vendor";
                Application.DoEvents();
                DataRow[] oItemRows = dsItemVendor.Tables[0].Select(" ItemId='" + oItemRow.ItemID.Trim() + "'");
                if (oItemRows != null && oItemRows.Length > 0)
                {
                    ItemVendorRow oItemVendorRow = (ItemVendorRow)oItemRows[0];
                    podRow = dsPoDetails.PODetail.NewPODetailRow();
                    podRow.ItemID = oItemRow.ItemID;
                    podRow.ItemDescription = oItemRow.Description;
                    podRow.VendorID = oItemVendorRow.VendorID;
                    podRow.VendorItemCode = oItemVendorRow.VendorItemID;
                    podRow.VendorName = oItemVendorRow.VendorCode;
                    podRow.QTY = podRow.QTY = Configuration.convertNullToInt(podRowTemp["Qty"].ToString());
                    podRow.QtyInStock = oItemRow.QtyInStock;
                    podRow.ReOrderLevel = oItemRow.ReOrderLevel;
                    podRow.SoldItems = Configuration.convertNullToInt(podRowTemp["QTY"].ToString());
                    //Modified by shitaljit as per Fahad for those Pharmacy which are not using PO 
                    //We should populate item cost from Item table not from ItemVendor table.
                    if (isFromPO == true)
                    {
                        podRow.Price = oItemVendorRow.VenorCostPrice;
                    }
                    else
                    {
                        podRow.Price = oItemRow.LastCostPrice;
                    }
                    podRow.PacketSize = oItemRow.PckSize;
                    podRow.PacketQuant = oItemRow.PckQty;
                    podRow.Packetunit = oItemRow.PckUnit;
                    podRow.MinOrdQty = oItemRow.MinOrdQty;

                    DataRow[] DeptRow = dsDepartmentData.Tables[0].Select("DeptID=" + oItemRow.DepartmentID);
                    if (DeptRow.Length != 0)
                        podRow.DeptName = DeptRow[0][2].ToString();

                    DataRow[] SubDeptRow = dsSubDepartmentData.Tables[0].Select("SubDepartmentID=" + oItemRow.SubDepartmentID);

                    if (SubDeptRow.Length != 0)
                        podRow.SubDeptName = SubDeptRow[0][2].ToString();
                    //Added by Amit Date 4 Aug 2011
                    podRow.RetailPrice = Configuration.convertNullToDecimal(podRowTemp["Price"].ToString());
                    podRow.ItemPrice = Configuration.convertNullToDecimal(podRowTemp["Price"].ToString()) - Configuration.convertNullToDecimal(podRowTemp["Discount"].ToString());
                    podRow.Discount = Configuration.convertNullToDecimal(podRowTemp["Discount"].ToString());
                    //End

                    //29 Nov 2011
                    DataRow[] InvRecDateRow = dsInvRecDate.Tables[0].Select(" ItemId='" + oItemRow.ItemID.Trim() + "'");
                    if (InvRecDateRow.Length != 0)
                        podRow.InvRecDate = (System.DateTime)InvRecDateRow[0][clsPOSDBConstants.PODetail_Fld_InvRecDate];
                    //End

                    dsPoDetails.PODetail.AddRow(podRow);

                    index++;
                }
                Application.DoEvents();
            }
            SetPacketToOrder(ref dsPoDetails);
            podTable = dsPoDetails.PODetail;
            if (dsPoDetails.PODetail.Rows.Count > 0)
            {
                lblMessage.Text = podTable.Rows.Count.ToString() + " No(s) Items Acquired For Purchase Order By Last Vendor";
                Application.DoEvents();
            }
            return podTable;
        }

        private PODetailTable CreateAutoPOByItemOwnerQtyZero()
        {
            bool queryResult = false;
            PODetailTable podTable = null;
            //Added By Amit Date 4 Aug 2011
            DataSet dsTransDetailData = new DataSet();
            TransDetailSvr oPOSTransDetail = new TransDetailSvr();
            //End
            ItemVendorData dsItemVendor = new ItemVendorData();
            ItemData dsItems = new ItemData();
            ItemSvr oItem = new ItemSvr();
            ItemVendorSvr oItemVendor = new ItemVendorSvr();
            DepartmentData dsDepartmentData = new DepartmentData();
            DepartmentSvr oDepartment = new DepartmentSvr();
            SubDepartmentData dsSubDepartmentData = new SubDepartmentData();
            SubDepartmentSvr oSubDepartment = new SubDepartmentSvr();

            //Added By Amit Date 29 Nov 2011
            InvRecvHeaderSvr oInvRecHeader = new InvRecvHeaderSvr();
            DataSet dsInvRecDate = new DataSet();
            //End

            //List<string> lstData = new List<string>();
            List<string> lstDeptData = new List<string>();
            List<string> lstSubDeptData = new List<string>();

            #region Sprint-19 - 1883 24-Mar-2015 JY Commented
            // if (this.txtDepartment.Text.Trim() != "" && txtDepartment.Tag.ToString() != ""
            //     && this.txtSubDepartment.Text.Trim() == "" && txtSubDepartment.Tag.ToString() == "")
            // {
            //     dsDepartmentData = oDepartment.PopulateList("where DeptCode in (" + this.txtDepartment.Tag.ToString().Trim() + ")");

            //     foreach (DataRow Dr in dsDepartmentData.Tables[0].Rows)
            //     {
            //         lstDeptData.Add("'" + Dr[0].ToString() + "'");
            //         Application.DoEvents();
            //     }
            //     dsItems = oItem.PopulateList(" where DepartmentID in (" + string.Join(",", lstDeptData.ToArray()) + ") and " + clsPOSDBConstants.Item_Fld_LastVendor + "!='' AND " + clsPOSDBConstants.Item_Fld_QtyInStock + " <= 0");
            //     dsSubDepartmentData = oSubDepartment.PopulateList("where DepartmentId in (" + string.Join(",", lstDeptData.ToArray()) + ")");


            // }
            // else if (this.txtDepartment.Text.Trim() == "" && txtDepartment.Tag.ToString() == ""
            //     && this.txtSubDepartment.Text.Trim() != "" && txtSubDepartment.Tag.ToString() != "")
            // {
            //     dsSubDepartmentData = oSubDepartment.PopulateList("where SubDepartmentId in (" + this.txtSubDepartment.Tag.ToString().Trim() + ")");
            //     foreach (DataRow Dr in dsSubDepartmentData.Tables[0].Rows)
            //     {
            //         lstSubDeptData.Add("'" + Dr[0].ToString() + "'");
            //         lstDeptData.Add("'" + Dr[1].ToString() + "'");
            //         Application.DoEvents();
            //     }
            //     dsDepartmentData = oDepartment.PopulateList("where DeptID in (" + string.Join(",", lstDeptData.ToArray()) + ")");
            //     dsItems = oItem.PopulateList(" where DepartmentID in (" + string.Join(",", lstDeptData.ToArray()) + ") and  " + clsPOSDBConstants.Item_Fld_LastVendor + "!='' AND " + clsPOSDBConstants.Item_Fld_QtyInStock + " <= 0");

            // }
            // else if (this.txtDepartment.Text.Trim() != "" && txtDepartment.Tag.ToString() != ""
            //&& this.txtSubDepartment.Text.Trim() != "" && txtSubDepartment.Tag.ToString() != "")
            // {
            //     dsDepartmentData = oDepartment.PopulateList("where DeptCode in (" + this.txtDepartment.Tag.ToString().Trim() + ")");

            //     foreach (DataRow Dr in dsDepartmentData.Tables[0].Rows)
            //     {
            //         lstDeptData.Add("'" + Dr[0].ToString() + "'");
            //         Application.DoEvents();
            //     }

            //     dsSubDepartmentData = oSubDepartment.PopulateList("where SubDepartmentId in (" + this.txtSubDepartment.Tag.ToString().Trim() + ")");
            //     foreach (DataRow Dr in dsSubDepartmentData.Tables[0].Rows)
            //     {
            //         lstSubDeptData.Add("'" + Dr[0].ToString() + "'");
            //         Application.DoEvents();
            //     }
            //     dsItems = oItem.PopulateList(" where DepartmentID in (" + string.Join(",", lstDeptData.ToArray()) + ") and SubDepartmentId in (" + this.txtSubDepartment.Tag.ToString().Trim() + ") and  " + clsPOSDBConstants.Item_Fld_LastVendor + "!='' AND " + clsPOSDBConstants.Item_Fld_QtyInStock + " <= 0");

            // }
            #endregion

            #region Sprint-19 - 1883 23-Mar-2015 JY Added location filter and also improved the existing filter code
            string strFilter = string.Empty;

            if (this.txtDepartment.Text.Trim() != "" && txtDepartment.Tag.ToString() != "")
            {
                dsDepartmentData = oDepartment.PopulateList("where DeptCode in (" + this.txtDepartment.Tag.ToString().Trim() + ")");

                foreach (DataRow Dr in dsDepartmentData.Tables[0].Rows)
                {
                    lstDeptData.Add("'" + Dr[0].ToString() + "'");
                }
                strFilter += " and DepartmentID in (" + string.Join(",", lstDeptData.ToArray()) + ")";

                if (this.txtSubDepartment.Text.Trim() != "" && txtSubDepartment.Tag.ToString() != "")
                {
                    dsSubDepartmentData = oSubDepartment.PopulateList("where SubDepartmentId in (" + this.txtSubDepartment.Tag.ToString().Trim() + ")");
                    foreach (DataRow Dr in dsSubDepartmentData.Tables[0].Rows)
                    {
                        lstSubDeptData.Add("'" + Dr[0].ToString() + "'");
                        lstDeptData.Add("'" + Dr[1].ToString() + "'");
                        Application.DoEvents();
                    }
                    strFilter += " and SubDepartmentID in (" + string.Join(",", lstSubDeptData.ToArray()) + ")";
                }
                else
                {
                    dsSubDepartmentData = oSubDepartment.PopulateList("where DepartmentId in (" + string.Join(",", lstDeptData.ToArray()) + ")");
                }
            }
            else
            {
                if (this.txtSubDepartment.Text.Trim() != "" && txtSubDepartment.Tag.ToString() != "")
                {
                    dsSubDepartmentData = oSubDepartment.PopulateList("where SubDepartmentId in (" + this.txtSubDepartment.Tag.ToString().Trim() + ")");
                    foreach (DataRow Dr in dsSubDepartmentData.Tables[0].Rows)
                    {
                        lstSubDeptData.Add("'" + Dr[0].ToString() + "'");
                        lstDeptData.Add("'" + Dr[1].ToString() + "'");
                        Application.DoEvents();
                    }
                    strFilter += " and SubDepartmentID in (" + string.Join(",", lstSubDeptData.ToArray()) + ")";
                    dsDepartmentData = oDepartment.PopulateList("where DeptID in (" + string.Join(",", lstDeptData.ToArray()) + ")");
                }
                else
                {
                    dsDepartmentData = (DepartmentData)oDepartment.PopulateList(" where 1=1 ");
                    dsSubDepartmentData = (SubDepartmentData)oSubDepartment.PopulateList(" where 1=1 ");
                }
            }

            if (this.txtItemLocation.Text.Trim() != "" && txtItemLocation.Tag.ToString() != "")
            {
                strFilter += " and LOCATION in (" + txtItemLocation.Text + ")";
            }
            dsItems = oItem.PopulateList(" where " + clsPOSDBConstants.Item_Fld_LastVendor + "!='' AND " + clsPOSDBConstants.Item_Fld_QtyInStock + " <= 0" + strFilter);
            #endregion

            #region Consider Transaction history in the item populate for Zero or -Ve qty logic
            //added By Shitaljit to consider Transaction history in Zero or Negative Qty.
            // JIRA link -PRIMEPOS -13
            string strDistItems = string.Empty;
            if (this.chckUseTransHistoryForReOrder.Checked == true)
            {
                dsDepartmentData = (DepartmentData)oDepartment.PopulateList(" where 1=1 ");
                dsSubDepartmentData = (SubDepartmentData)oSubDepartment.PopulateList(" where 1=1 ");
                string SelectClause = string.Empty;
                if (Configuration.CPrimeEDISetting.ConsiderReturnTrans)  //Sprint-27 - PRIMEPOS-2390 28-Sep-2017 JY Added    //PRIMEPOS-3167 07-Nov-2022 JY Modified 
                    SelectClause = " Trans.TransDate >= DateAdd(D,-" + this.numNoOfDaysForItemReOrder.Value.ToString() + ",getdate()) ";
                else
                    SelectClause = " Trans.TransDate >= DateAdd(D,-" + this.numNoOfDaysForItemReOrder.Value.ToString() + ",getdate()) AND Trans.TransType=1 ";
                Application.DoEvents();
                string strSQL = string.Empty;
                TransHeaderSvr oPOSTrans = new TransHeaderSvr();
                //DataSet dsTransHeader = oPOSTrans.PopulateList(SelectClause);   //PRIMEPOS-3097 28-Jun-2022 JY commented
                DataSet dsTransHeader = oPOSTrans.PopulateList(SelectClause, out strSQL);   //PRIMEPOS-3097 28-Jun-2022 JY Added
                if (dsTransHeader == null || dsTransHeader.Tables.Count == 0 || dsTransHeader.Tables[0].Rows.Count == 0)
                {
                    throw (new Exception("There are no more transactions in this last " + this.numNoOfDaysForItemReOrder.Value.ToString() + " day(s)."));
                }
                #region PRIMEPOS-3097 28-Jun-2022 JY Commented
                //foreach (DataRow Dr in dsTransHeader.Tables[0].Rows)
                //{
                //    lstData.Add(Dr[0].ToString());
                //}
                //string whereClause = " where TransID in (" + string.Join(",", lstData.ToArray()) + ")";
                //lstData.Clear();
                //foreach (DataRow Dr in dsItems.Tables[0].Rows)
                //{
                //    lstData.Add("'" + Dr[0].ToString() + "'");
                //    Application.DoEvents();
                //}
                //if (lstData.Count > 0)
                //{
                //    whereClause += "AND ItemID in (" + string.Join(",", lstData.ToArray()) + ")";
                //}
                //dsTransDetailData = oPOSTransDetail.PopulateList(whereClause);
                //lstData.Clear();
                //foreach (DataRow Dr in dsTransDetailData.Tables[0].Rows)
                //{
                //    lstData.Add("'" + Dr[0].ToString() + "'");
                //    Application.DoEvents();
                //}
                //dsItems = oItem.PopulateList(" where ItemID in (" + string.Join(",", lstData.ToArray()) + ") AND " + clsPOSDBConstants.Item_Fld_QtyInStock + " <= 0");
                #endregion
                #region PRIMEPOS-3097 28-Jun-2022 JY Added  
                dsTransDetailData = oPOSTransDetail.PopulateList(" where TransID in (" + strSQL + ") AND ItemID in (SELECT ItemID FROM Item where LastVendor != '' AND QtyInStock <= 0)", out queryResult, out strDistItems);
                dsItems = oItem.PopulateList(" where ItemID in (" + strDistItems + ") AND LastVendor != '' AND QtyInStock <= 0");
                #endregion
            }
            else
            {
                dsDepartmentData = (DepartmentData)oDepartment.PopulateList(" where 1=1 ");
                dsSubDepartmentData = (SubDepartmentData)oSubDepartment.PopulateList(" where 1=1 ");
                dsItems = oItem.PopulateList(" where " + clsPOSDBConstants.Item_Fld_QtyInStock + " <= 0");
                #region PRIMEPOS-3097 28-Jun-2022 JY Commented
                //foreach (DataRow Dr in dsItems.Tables[0].Rows)
                //{
                //    lstData.Add("'" + Dr[0].ToString() + "'");
                //    Application.DoEvents();
                //}
                #endregion
                //Added By Amit Date 4 Aug 2011
                //dsTransDetailData = oPOSTransDetail.PopulateList(" where ItemID in (" + string.Join(",", lstData.ToArray()) + ")"); //PRIMEPOS-3097 28-Jun-2022 JY commented
                dsTransDetailData = oPOSTransDetail.PopulateList(" where ItemID in (SELECT ItemID FROM Item where LastVendor != '' and QtyInStock <= 0)", out queryResult, out strDistItems);    //PRIMEPOS-3097 28-Jun-2022 JY Added
                if (dsTransDetailData == null || dsTransDetailData.Tables.Count == 0 || dsTransDetailData.Tables[0].Rows.Count == 0)
                {
                    throw (new Exception("There are no items found which are used in Transaction."));
                }
                //End
            }
            #endregion

            //if (lstData.Count > 0)
            //    dsItemVendor = oItemVendor.PopulateList(" where ItemId in (" + string.Join(",", lstData.ToArray()) + ")");    //PRIMEPOS-3097 28-Jun-2022 JY commented
            dsItemVendor = oItemVendor.PopulateList(" where ItemId in (" + strDistItems + ")", true);  //PRIMEPOS-3097 28-Jun-2022 JY Added
            //if (dsItems == null || dsItems.Tables.Count == 0 || dsItems.Tables[0].Rows.Count == 0)
            //{
            //    throw (new Exception("There are no items found."));
            //}

            //if (dsItemVendor == null || dsItemVendor.Tables.Count == 0 || dsItemVendor.Tables[0].Rows.Count == 0)
            //{
            //    throw (new Exception("There are no more vendor items matching LastVendor vendor found."));
            //}
            //int count = 0;

            //Added By Amit Date 29 Nov 2011
            //dsInvRecDate = oInvRecHeader.ItemInvRecDate(lstData);   //PRIMEPOS-3097 28-Jun-2022 JY Commented
            dsInvRecDate = oInvRecHeader.ItemInvRecDate(strDistItems);   //PRIMEPOS-3097 28-Jun-2022 JY Added
            //End
            podTable = new PODetailTable();
            PODetailRow podRow = null;
            int index = 0;

            //foreach (ItemRow oItemRow in dsItems.Tables[0].Rows)
            //Added By Amit date 29 July 2011            
            foreach (DataRow podRowTemp in dsTransDetailData.Tables[0].Rows)
            {
                DataRow[] oItemRowTemp = dsItems.Tables[0].Select("ItemID='" + podRowTemp["ItemID"] + "'");
                ItemRow oItemRow = null;

                if (oItemRowTemp.Length > 0)
                    oItemRow = (ItemRow)oItemRowTemp[0];
                else
                    continue;
                //End
                if (!oItemRow.ExclFromAutoPO)
                {
                    lblMessage.Text = "Scanning Item #" + oItemRow.ItemID.Trim() + " For Item Owner";
                    Application.DoEvents();
                    string sQuery = " ItemId ='" + oItemRow.ItemID.Trim() + "' And ( vendorCode not in ('" + oItemRow.LastVendor.ToString().Trim().Replace("'", "''") + "','" + oItemRow.PreferredVendor.Replace("'", "''") + "') OR vendorcode <> '" + Configuration.CPrimeEDISetting.DefaultVendor.Replace("'", "''") + "')"; //PRIMEPOS-3167 07-Nov-2022 JY Modified
                    DataRow[] oItemRows = dsItemVendor.Tables[0].Select(sQuery);
                    if (oItemRows != null && oItemRows.Length > 0)
                    {
                        ItemVendorRow oItemVendorRow = (ItemVendorRow)oItemRows[0];
                        podRow = dsPoDetails.PODetail.NewPODetailRow();
                        podRow.ItemID = oItemRow.ItemID;
                        podRow.ItemDescription = oItemRow.Description;
                        podRow.VendorID = oItemVendorRow.VendorID;
                        podRow.VendorItemCode = oItemVendorRow.VendorItemID;
                        podRow.VendorName = oItemVendorRow.VendorCode;
                        podRow.QTY = podRow.QTY = Configuration.convertNullToInt(podRowTemp["Qty"].ToString());
                        podRow.QtyInStock = oItemRow.QtyInStock;
                        podRow.ReOrderLevel = oItemRow.ReOrderLevel;
                        //Modified by shitaljit as per Fahad for those Pharmacy which are not using PO 
                        //We should populate item cost from Item table not from ItemVendor table.
                        podRow.SoldItems = Configuration.convertNullToInt(podRowTemp["QTY"].ToString());
                        if (isFromPO == true)
                        {
                            podRow.Price = oItemVendorRow.VenorCostPrice;
                        }
                        else
                        {
                            podRow.Price = oItemRow.LastCostPrice;
                        }

                        podRow.PacketSize = oItemRow.PckSize;
                        podRow.PacketQuant = oItemRow.PckQty;
                        podRow.Packetunit = oItemRow.PckUnit;
                        podRow.MinOrdQty = oItemRow.MinOrdQty;
                        DataRow[] DeptRow = dsDepartmentData.Tables[0].Select("DeptID=" + oItemRow.DepartmentID);
                        if (DeptRow.Length != 0)
                            podRow.DeptName = DeptRow[0][2].ToString();

                        DataRow[] SubDeptRow = dsSubDepartmentData.Tables[0].Select("SubDepartmentID=" + oItemRow.SubDepartmentID);

                        if (SubDeptRow.Length != 0)
                            podRow.SubDeptName = SubDeptRow[0][2].ToString();
                        //Added by Amit Date 4 Aug 2011
                        podRow.RetailPrice = Configuration.convertNullToDecimal(podRowTemp["Price"].ToString());
                        podRow.ItemPrice = Configuration.convertNullToDecimal(podRowTemp["Price"].ToString()) - Configuration.convertNullToDecimal(podRowTemp["Discount"].ToString());
                        podRow.Discount = Configuration.convertNullToDecimal(podRowTemp["Discount"].ToString());
                        //End
                        //29 Nov 2011
                        DataRow[] InvRecDateRow = dsInvRecDate.Tables[0].Select(" ItemId='" + oItemRow.ItemID.Trim() + "'");
                        if (InvRecDateRow.Length != 0)
                            podRow.InvRecDate = (System.DateTime)InvRecDateRow[0][clsPOSDBConstants.PODetail_Fld_InvRecDate];
                        //End

                        dsPoDetails.PODetail.AddRow(podRow);

                        index++;
                    }
                }
                Application.DoEvents();
            }
            SetPacketToOrder(ref dsPoDetails);
            podTable = dsPoDetails.PODetail;
            if (dsPoDetails.PODetail.Rows.Count > 0)
            {
                lblMessage.Text = podTable.Rows.Count.ToString() + " No(s) Items Acquired For Purchase Order By Last Vendor";
                Application.DoEvents();
            }
            return podTable;
        }
        //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

        private PODetailTable CreateAutoPOByItemOwnerReOrder()
        {
            bool queryResult = false;

            PODetailTable podTable = null;

            DataSet dsTransHeader = new DataSet();
            DataSet dsTransDetail = new DataSet();
            ItemData dsItems = new ItemData();
            ItemVendorData dsItemVendor = new ItemVendorData();
            DepartmentData dsDepartment = new DepartmentData();
            SubDepartmentData dsSubDepartment = new SubDepartmentData();
            DataSet dsInvRecDate = new DataSet();

            GetDsForReorderLevel("Item Owner", txtDepartment.Tag.ToString().Trim(), txtSubDepartment.Tag.ToString().Trim(), out dsTransHeader, out dsTransDetail, out dsItems, out dsItemVendor, out dsDepartment, out dsSubDepartment, out dsInvRecDate, out queryResult);

            #region Commented 

            // DataSet dsTransHeader = new DataSet();
            // DataSet dsTransDetailData = new DataSet();

            // ItemVendorData dsItemVendor = new ItemVendorData();
            // ItemData dsItems = new ItemData();

            // TransHeaderSvr oPOSTrans = new TransHeaderSvr();
            // TransDetailSvr oPOSTransDetail = new TransDetailSvr();

            // ItemSvr oItem = new ItemSvr();
            // ItemVendorSvr oItemVendor = new ItemVendorSvr();

            // //Added by Amit Date 30 June 2011
            // DepartmentData dsDepartmentData = new DepartmentData();
            // DepartmentSvr oDepartment = new DepartmentSvr();
            // SubDepartmentData dsSubDepartmentData = new SubDepartmentData();
            // SubDepartmentSvr oSubDepartment = new SubDepartmentSvr();
            // //End
            // string SelectClause = string.Empty;
            // //changed by SRT(Abhishek)  Date : 21 Sept. 2009
            // //Added condition to check if Transaction should be of Sell Type
            // SelectClause = " Trans.TransDate >= DateAdd(D,-" + clsPOSDBConstants.ItemReorderPeriod.ToString() + ",getdate()) AND Trans.TransType=1";
            // //End Of changed by SRT(Abhishek)  Date : 21 Sept. 2009

            // lblMessage.Text = "Aquiring Transactions For Last Vendor.";
            // Application.DoEvents();

            // dsTransHeader = oPOSTrans.PopulateList(SelectClause);
            // if (dsTransHeader == null || dsTransHeader.Tables.Count == 0 || dsTransHeader.Tables[0].Rows.Count == 0)
            // {
            //     throw (new Exception("There are no more transactions in this last " + clsPOSDBConstants.ItemReorderPeriod.ToString() + " day(s)."));
            // }
            // List<string> lstData = new List<string>();
            // foreach (DataRow Dr in dsTransHeader.Tables[0].Rows)
            // {
            //     lstData.Add(Dr[0].ToString());
            //     Application.DoEvents();
            // }
            // dsTransDetailData = oPOSTransDetail.PopulateList(" where TransID in (" + string.Join(",", lstData.ToArray()) + ")", out queryResult);
            // lstData = new List<string>();
            // foreach (DataRow Dr in dsTransDetailData.Tables[0].Rows)
            // {
            //     lstData.Add("'" + Dr[0].ToString() + "'");
            //     Application.DoEvents();
            // }
            // lblMessage.Text = "Aquiring Transaction Items For Last Vendor.";
            // Application.DoEvents();
            // //Start: Added By Amit Date 30 June 2011
            // List<string> lstDeptData = new List<string>();
            // List<string> lstSubDeptData = new List<string>();

            // if (this.txtDepartment.Text.Trim() != "" && txtDepartment.Tag.ToString() != ""
            //     && this.txtSubDepartment.Text.Trim() == "" && txtSubDepartment.Tag.ToString() == "")
            // {
            //     dsDepartmentData = oDepartment.PopulateList("where DeptCode in (" + this.txtDepartment.Tag.ToString().Trim() + ")");

            //     foreach (DataRow Dr in dsDepartmentData.Tables[0].Rows)
            //     {
            //         lstDeptData.Add("'" + Dr[0].ToString() + "'");
            //         Application.DoEvents();
            //     }
            //     dsSubDepartmentData = oSubDepartment.PopulateList("where DepartmentId in (" + string.Join(",", lstDeptData.ToArray()) + ")");

            //     dsItems = oItem.PopulateList(" where ItemId in (" + string.Join(",", lstData.ToArray()) + ") and DepartmentID in (" + string.Join(",", lstDeptData.ToArray()) + ")");
            // }
            // else if (this.txtDepartment.Text.Trim() == "" && txtDepartment.Tag.ToString() == ""
            //     && this.txtSubDepartment.Text.Trim() != "" && txtSubDepartment.Tag.ToString() != "")
            // {
            //     dsSubDepartmentData = oSubDepartment.PopulateList("where SubDepartmentId in (" + this.txtSubDepartment.Tag.ToString().Trim() + ")");
            //     foreach (DataRow Dr in dsSubDepartmentData.Tables[0].Rows)
            //     {
            //         lstSubDeptData.Add("'" + Dr[0].ToString() + "'");
            //         lstDeptData.Add("'" + Dr[1].ToString() + "'");
            //         Application.DoEvents();
            //     }
            //     dsDepartmentData = oDepartment.PopulateList("where DeptID in (" + string.Join(",", lstDeptData.ToArray()) + ")");

            //     dsItems = oItem.PopulateList(" where ItemId in (" + string.Join(",", lstData.ToArray()) + ") and SubDepartmentID in (" + string.Join(",", lstSubDeptData.ToArray()) + ")");
            // }
            // else if (this.txtDepartment.Text.Trim() != "" && txtDepartment.Tag.ToString() != ""
            //&& this.txtSubDepartment.Text.Trim() != "" && txtSubDepartment.Tag.ToString() != "")
            // {
            //     dsDepartmentData = oDepartment.PopulateList("where DeptCode in (" + this.txtDepartment.Tag.ToString().Trim() + ")");

            //     foreach (DataRow Dr in dsDepartmentData.Tables[0].Rows)
            //     {
            //         lstDeptData.Add("'" + Dr[0].ToString() + "'");
            //         Application.DoEvents();
            //     }

            //     dsSubDepartmentData = oSubDepartment.PopulateList("where SubDepartmentId in (" + this.txtSubDepartment.Tag.ToString().Trim() + ")");
            //     foreach (DataRow Dr in dsSubDepartmentData.Tables[0].Rows)
            //     {
            //         lstSubDeptData.Add("'" + Dr[0].ToString() + "'");
            //         Application.DoEvents();
            //     }

            //     dsItems = oItem.PopulateList(" where ItemId in (" + string.Join(",", lstData.ToArray()) + ") and DepartmentID in (" + string.Join(",", lstDeptData.ToArray()) + ") and SubDepartmentID in (" + string.Join(",", lstSubDeptData.ToArray()) + ")");

            // }
            // else
            // {
            //     dsDepartmentData = (DepartmentData)oDepartment.PopulateList(" where 1=1 ");
            //     dsSubDepartmentData = (SubDepartmentData)oSubDepartment.PopulateList(" where 1=1 ");

            //     dsItems = oItem.PopulateList(" where ItemId in (" + string.Join(",", lstData.ToArray()) + ")");
            // }
            // //End

            // dsItemVendor = oItemVendor.PopulateList(" where ItemId in (" + string.Join(",", lstData.ToArray()) + ")");
            // if (dsItems == null || dsItems.Tables.Count == 0 || dsItems.Tables[0].Rows.Count == 0)
            // {
            //     throw (new Exception("There are no more items matching transactions of selected period."));
            // }

            // if (dsItemVendor == null || dsItemVendor.Tables.Count == 0 || dsItemVendor.Tables[0].Rows.Count == 0)
            // {
            //     throw (new Exception("There are no more vendor items matching transactions of selected period."));
            // }
            #endregion Commented
            DataSet dtItems = null;

            if (this.chckUseTransHistoryForReOrder.Checked == true)
            {
                dtItems = dsTransDetail;
            }
            else if (dsItems != null)
            {
                if (dsItems.Tables[0].Rows.Count > 0)
                {
                    dtItems = dsItems;
                }
                else
                {
                    throw (new Exception("There are no items to "));
                }

            }
            //int count = 0;
            podTable = new PODetailTable();
            PODetailRow podRow = null;
            //Added By Amit Date 30 June 2011
            int index = 0;

            //foreach (ItemRow oItemRow in dsItems.Tables[0].Rows)
            //Added By Amit date 29 July 2011            
            foreach (DataRow podRowTemp in dtItems.Tables[0].Rows)
            {
                DataRow[] oItemRowTemp = dsItems.Tables[0].Select("ItemID='" + podRowTemp["ItemID"] + "'");
                ItemRow oItemRow = null;

                if (oItemRowTemp.Length > 0)
                    oItemRow = (ItemRow)oItemRowTemp[0];
                else
                    continue;

                lblMessage.Text = "Scanning Item #" + oItemRow.ItemID.Trim() + " For Last Vendor";
                Application.DoEvents();

                int TEMPVEND = 0;
                //if (Configuration.convertNullToString(oItemRow.LastVendor).Trim().Length > 0 && !oItemRow.ExclFromAutoPO && oItemRow.QtyInStock < oItemRow.ReOrderLevel && oItemRow.ReOrderLevel > 0 && int.TryParse(oItemRow.LastVendor.ToString(), out TEMPVEND))//isnull(itm.ExclFromAutoPO,0) = 0
                //if (!oItemRow.ExclFromAutoPO)
                //if (!oItemRow.ExclFromAutoPO && oItemRow.QtyInStock < oItemRow.ReOrderLevel && oItemRow.ReOrderLevel > 0)
                //{
                //ItemVendorRow oItemVendorRow = (ItemVendorRow)oItemRows[0];
                //DataRow[] TransRows1 = dsTransDetailData.Tables[0].Select(" ItemId='" + oItemRow.ItemID.Trim() + "' ");
                //DataRow[] oItemRows = dsItemVendor.Tables[0].Select(" ItemId='" + oItemRow.ItemID.Trim() + "' AND VENDORID=" + oItemVendorRow.VendorID.ToString());
                //DataRow[] oItemRows = dsItemVendor.Tables[0].Select(" ItemId='" + oItemRow.ItemID.Trim() + "' AND VENDORID=");

                string selectQuery = string.Empty;
                selectQuery = " ItemId='" + oItemRow.ItemID.Trim() + "'";

                if (Configuration.convertNullToString(oItemRow.LastVendor).Trim().Length > 0)
                {
                    //if (int.TryParse(oItemRow.LastVendor.ToString(), out TEMPVEND))
                    {
                        if (oItemRow.PreferredVendor != string.Empty)
                            selectQuery = selectQuery + " And ( vendorCode not in ('" + oItemRow.LastVendor.ToString().Trim().Replace("'", "''") + "','" + oItemRow.PreferredVendor.Replace("'", "''") + "') OR vendorcode <> '" + Configuration.CPrimeEDISetting.DefaultVendor.Replace("'", "''") + "')";  //PRIMEPOS-3167 07-Nov-2022 JY Modified
                        else
                            selectQuery = selectQuery + " And ( vendorCode not in ('" + oItemRow.LastVendor.ToString().Trim().Replace("'", "''") + "') OR vendorcode <> '" + Configuration.CPrimeEDISetting.DefaultVendor.Replace("'", "''") + "')";    //PRIMEPOS-3167 07-Nov-2022 JY Modified
                    }
                    //else
                    //{
                    //    if (oItemRow.PreferredVendor != string.Empty)
                    //        selectQuery = selectQuery + " And ( vendorCode not in ('" + oItemRow.PreferredVendor + "') OR vendorcode <> '" + Configuration.CPOSSet.DefaultVendor + "')";
                    //    else
                    //        selectQuery = selectQuery + " And ( vendorcode <> '" + Configuration.CPOSSet.DefaultVendor + "')";
                    //}
                }
                else
                {
                    if (oItemRow.PreferredVendor != string.Empty)
                        selectQuery = selectQuery + " And ( vendorCode not in ('" + oItemRow.PreferredVendor.Replace("'", "''") + "') OR vendorcode <> '" + Configuration.CPrimeEDISetting.DefaultVendor.Replace("'", "''") + "')"; //PRIMEPOS-3167 07-Nov-2022 JY Modified
                    else
                        selectQuery = selectQuery + " And ( vendorcode <> '" + Configuration.CPrimeEDISetting.DefaultVendor.Replace("'", "''") + "')";  //PRIMEPOS-3167 07-Nov-2022 JY Modified

                }
                DataRow[] oItemRows = dsItemVendor.Tables[0].Select(selectQuery);

                if (oItemRows != null && oItemRows.Length > 0)
                {
                    ItemVendorRow oItemVendorRow = (ItemVendorRow)oItemRows[0];
                    DataRow[] TransRows = dtItems.Tables[0].Select(" ItemId='" + oItemRow.ItemID.Trim() + "' ");
                    if (TransRows != null && TransRows.Length > 0 && oItemRows.Length == 1)
                    {
                        podRow = dsPoDetails.PODetail.NewPODetailRow();
                        podRow.ItemID = oItemRow.ItemID;
                        podRow.ItemDescription = oItemRow.Description;
                        podRow.VendorID = oItemVendorRow.VendorID;
                        podRow.VendorItemCode = oItemVendorRow.VendorItemID;
                        podRow.VendorName = oItemVendorRow.VendorCode;

                        //if (!isFromPO)
                        //{
                        //    podRow.QTY = Configuration.convertNullToInt(podRowTemp["QTY"].ToString());
                        //}
                        //else
                        //{
                        //    //podRow.QTY = oItemRow.MinOrdQty;
                        //    podRow.QTY = oItemRow.MinOrdQty - oItemRow.QtyInStock;
                        //}

                        //Added by Krishna on 19 December 2011
                        podRow.QTY = Configuration.convertNullToInt(oItemRow.MinOrdQty) - Configuration.convertNullToInt(oItemRow.QtyInStock);
                        podRow.SoldItems = Configuration.convertNullToInt(podRowTemp["QTY"].ToString());
                        //End of Added by Krishna

                        podRow.QtyInStock = oItemRow.QtyInStock;
                        podRow.ReOrderLevel = oItemRow.ReOrderLevel;
                        //Modified by shitaljit as per Fahad for those Pharmacy which are not using PO 
                        //We should populate item cost from Item table not from ItemVendor table.
                        if (isFromPO == true)
                        {
                            podRow.Price = oItemVendorRow.VenorCostPrice;
                        }
                        else
                        {
                            podRow.Price = oItemRow.LastCostPrice;
                        }

                        podRow.PacketSize = oItemRow.PckSize;
                        podRow.PacketQuant = oItemRow.PckQty;
                        podRow.Packetunit = oItemRow.PckUnit;
                        //Start : Added By Amit Date 5 jul 2011
                        podRow.MinOrdQty = oItemRow.MinOrdQty;
                        DataRow[] DeptRow = dsDepartment.Tables[0].Select("DeptID=" + oItemRow.DepartmentID);
                        if (DeptRow.Length != 0)
                            podRow.DeptName = DeptRow[0][2].ToString();

                        DataRow[] SubDeptRow = dsSubDepartment.Tables[0].Select("SubDepartmentID=" + oItemRow.SubDepartmentID);

                        if (SubDeptRow.Length != 0)
                            podRow.SubDeptName = SubDeptRow[0][2].ToString();
                        //End
                        //Added by Amit Date 27 july 2011
                        podRow.RetailPrice = Configuration.convertNullToDecimal(podRowTemp["Price"].ToString());
                        podRow.ItemPrice = Configuration.convertNullToDecimal(podRowTemp["Price"].ToString()) - Configuration.convertNullToDecimal(podRowTemp["Discount"].ToString());
                        podRow.Discount = Configuration.convertNullToDecimal(podRowTemp["Discount"].ToString());
                        //End

                        //29 Nov 2011
                        DataRow[] InvRecDateRow = dsInvRecDate.Tables[0].Select(" ItemId='" + oItemRow.ItemID.Trim() + "'");
                        if (InvRecDateRow.Length != 0)
                            podRow.InvRecDate = (System.DateTime)InvRecDateRow[0][clsPOSDBConstants.PODetail_Fld_InvRecDate];
                        //End

                        dsPoDetails.PODetail.AddRow(podRow);

                        index++;

                    }
                    //}
                }
                Application.DoEvents();
            }
            SetPacketToOrder(ref dsPoDetails);
            if (dsPoDetails.PODetail.Rows.Count > 0)
            {
                if (optionReport.CheckedIndex == 0)
                {
                    PODetailData dsTempPoDetails = new PODetailData();
                    DataTable dtTemp = dsPoDetails.PODetail.DefaultView.ToTable(true, "ItemId");

                    lblMessage.Text = dtTemp.Rows.Count.ToString() + " No(s) Items Acquired For Purchase Order By Item Owner Vendor";
                    Application.DoEvents();
                }
                else
                {
                    lblMessage.Text = dsPoDetails.PODetail.Rows.Count.ToString() + " No(s) Items Acquired For Purchase Order By Item Owner Vendor";
                    Application.DoEvents();
                }
            }
            return podTable;
        }
        private void CreateAutoPO(string POType)
        {
            bool queryResult = false;

            switch (POType)
            {
                //Following Added by Krishna on 20April2011
                case clsPOSDBConstants.IGNOREVENDOR:
                    #region IgnoreVendor

                    dsPoDetails = new PODetailData();
                    if (optByName.CheckedIndex == 0)
                    {
                        CreateAutoPOByIgnoreVendor();
                    }
                    else if (optByName.CheckedIndex == 1)
                    {
                        CreateAutoPOByIgnoreVendorReorder();
                    }
                    else
                    {
                        CreateAutoPOByIgnoreVendorQtyZero();
                    }
                    #endregion
                    break;
                //till here added by krishna

                case clsPOSDBConstants.FORCEDVENDOR:
                    #region Forced Vendor
                    if (txtVendorCode.Text.Trim().Length > 0)
                    {
                        dsPoDetails = new PODetailData();
                        if (optByName.CheckedIndex == 0)
                        {
                            CreateAutoPOBySpecifiedVendor(txtVendorCode.Text, "Forced");
                        }
                        else if (optByName.CheckedIndex == 1)
                        {
                            CreateAutoPOBySpecifiedVendorReorder(txtVendorCode.Text.Trim(), "Forced");
                        }
                        else
                        {
                            CreateAutoPOBySpecifiedVendorQtyZero(txtVendorCode.Text, "Forced");
                        }
                    }
                    #endregion
                    break;
                case clsPOSDBConstants.PREFEREDVENDOR:
                    #region PREFERED Vendor
                    dsPoDetails = new PODetailData();
                    if (optByName.CheckedIndex == 0)
                    {
                        CreateAutoPOByPreferedVendor();
                    }
                    else if (optByName.CheckedIndex == 1)
                    {
                        CreateAutoPOByPreferedVendorReorder();
                    }
                    else
                    {
                        CreateAutoPOByPreferredVendorQtyZero();
                    }
                    #endregion
                    break;
                case clsPOSDBConstants.LASTVENDOR:
                    #region LAST Vendor
                    dsPoDetails = new PODetailData();
                    if (optByName.CheckedIndex == 0)
                    {
                        CreateAutoPOByLastVendor();
                    }
                    else
                    {
                        CreateAutoPOByLastVendorReOrder();
                    }
                    #endregion
                    break;
                case clsPOSDBConstants.DEFAULTVENDOR:
                    #region Default Vendor
                    dsPoDetails = new PODetailData();
                    if (optByName.CheckedIndex == 0)
                    {
                        CreateAutoPOBySpecifiedVendor(Configuration.CPrimeEDISetting.DefaultVendor, "Default"); //PRIMEPOS-3167 07-Nov-2022 JY Modified
                    }
                    else
                    {
                        CreateAutoPOBySpecifiedVendorReorder(Configuration.CPrimeEDISetting.DefaultVendor, "Default");  //PRIMEPOS-3167 07-Nov-2022 JY Modified
                    }
                    break;
                #endregion

                case clsPOSDBConstants.ITEMOWNER:
                    #region Item Owner
                    dsPoDetails = new PODetailData();
                    if (optByName.CheckedIndex == 0)
                    {
                        CreateAutoPOByItemOwner();
                    }
                    else if (optByName.CheckedIndex == 1)
                    {
                        CreateAutoPOByItemOwnerReOrder();
                    }
                    else
                    {
                        CreateAutoPOByItemOwnerQtyZero();
                    }

                    #endregion

                    break;
            }


        }
        #region Commented
        //private string CreateAutoPO(string POType)
        //{
        //    bool queryResult = false;
        //    #region Declear and Initialize Queries
        //    SQLFill = String.Empty;
        //    string qryOptByName = " SELECT " +
        //                " itm.ItemID " +
        //                " , itm.Description " +
        //                " , Vendor.VendorCode" +
        //                " , VENDOR.VENDORID " +
        //                " , VENDOR.VENDORNAME " +
        //                " , itm.QtyInStock " +
        //                " , itm.ReOrderLevel " +
        //                " , Sum(Qty) QtySold100Days " +
        //                " , Sum(Qty) as QTY " +
        //                " , itemVendor.VENDORITEMID"+
        //                " , itemVendor.VENDORCOSTPRICE AS PRICE " +
        //                " FROM " +
        //                " Item itm " +
        //                " left outer join itemVendor on (itemVendor.ItemID=Itm.ItemID) " +
        //                " inner join vendor on (vendor.vendorid=itemvendor.vendorid) " +
        //                " , POSTransaction  Trans " +
        //                " , POSTransactionDetail TransDetail " +
        //                " WHERE " +
        //                " Trans.TransID = TransDetail.TransID " +
        //                " AND itm.ItemID = TransDetail.ItemID " +
        //                " AND isnull(itm.ExclFromAutoPO,0) = 0 ";

        //    string qryGroupByClause = " Group By " +
        //                " itm.ItemID " +
        //                " , itm.Description " +
        //                " , Vendor.VendorCode " +
        //                " , Vendor.VENDORID " +
        //                " , Vendor.VendorName " +
        //                " , itm.QtyInStock " +
        //                " , itm.ReOrderLevel,ITEMVENDOR.VendorCostPrice ,itemVendor.VENDORITEMID";

        //    string qryOrderByReorder = " SELECT " +
        //                " itm.ItemID " +
        //                " , itm.Description " +
        //                " , itm.QtyInStock " +
        //                " , itm.ReOrderLevel " +
        //                " , itm.ReOrderLevel as newOrder " +
        //                " , Vendor.VendorCode " +
        //                " , Vendor.VendorID " +
        //                " , Vendor.VendorName " +
        //                " , ItemVendor.VendorItemID " +
        //                " , ItemVendor.VendorCostPrice as Price" +
        //                " , Sum(Qty) Qty" +
        //                " FROM " +
        //                " POSTransactionDetail " +
        //                ", Item itm " +
        //                " left outer join itemVendor on (itemVendor.ItemID=Itm.ItemID) " +
        //                " inner join vendor on (vendor.vendorid=itemvendor.vendorid) " +
        //                " WHERE " +
        //                " itm.QtyInStock < itm.ReOrderLevel " +
        //                " and isnull(itm.ExclFromAutoPO,0) = 0" +
        //                " AND itm.ReOrderLevel>0" + 
        //                " AND itm.ItemID=POSTransactiondetail.ItemID";
        //    string vendorComparison = "(case when ((0)) then(case when (1) then (2) else (3) end )else ((4))end)";
        //    string preferedVendorCondition = "itm.preferredvendor=null";
        //    string preferedTrue = "itm.preferredvendor";
        //    string LastVendorCondition = "itm.lastvendor=null";
        //    string LastVendorTrue = "select vendorcode from vendor where vendorid=itm.lastvendor";
        //    string DefaultVendorCondition = "'" + Configuration.CPOSSet.DefaultVendor + "'";
        //    #endregion
        //    switch (POType)
        //    {
        //        case clsPOSDBConstants.FORCEDVENDOR:
        //            #region Forced Vendor
        //            if (optByName.CheckedIndex == 0)
        //            {
        //                SQLFill = qryOptByName;

        //                if (optSearchSet.CheckedIndex == 0)
        //                {
        //                    SQLFill += " AND Trans.TransDate >= DateAdd(D,-" + numNumberOfDays.Value + ",getdate()) ";
        //                }
        //                else
        //                {
        //                    SQLFill += " AND Trans.TransDate >= cast('" + DateTime.Parse(dtpFromDate.Value.ToString()).ToString() + "' as datetime) ";
        //                }

        //                SQLFill += " AND Vendor.VendorCode='" + txtVendorCode.Text + "'";
        //                            SQLFill += qryGroupByClause;
        //            }
        //            else
        //            {
        //                SQLFill = qryOrderByReorder;


        //                SQLFill += " AND Vendor.VendorCode='" + txtVendorCode.Text + "'";

        //            }
        //            SQLFill += qryGroupByClause;
        //            #endregion                
        //            break;
        //        case clsPOSDBConstants.PREFEREDVENDOR:
        //            #region PREFERED Vendor
        //            if (optByName.CheckedIndex == 0)
        //            {
        //                //SQLFill = qryOptByName;

        //                //if (optSearchSet.CheckedIndex == 0)
        //                //{
        //                //    SQLFill += " AND Trans.TransDate >= DateAdd(D,-" + numNumberOfDays.Value + ",getdate()) ";
        //                //}
        //                //else
        //                //{
        //                //    SQLFill += " AND Trans.TransDate >= cast('" + DateTime.Parse(dtpFromDate.Value.ToString()).ToString() + "' as datetime) ";
        //                //}

        //                //SQLFill += " AND Vendor.VendorCode=itm.PREFERREDVENDOR ";


        //                if (optSearchSet.CheckedIndex == 0)
        //                {
        //                    SQLFill = "Select Vendor.VendorCode , Vendor.VendorId  ,Vendor.VendorName, itemVendor.VendorItemId , itemVendor.VendorCostPrice AS PRICE, itm.ItemId, itm.description,itm.QtyInStock,itm.ReOrderLevel,itm.PreferredVendor,sum(OuterTransDetail.Qty) as QtySold " +
        //                             " from Item itm,  POSTransactionDetail OuterTransDetail, ItemVendor, Vendor where exists (" +
        //                              " select ItemId  from POSTransactionDetail TransDetail where TransDetail.TransId in " +
        //                                "(" +
        //                                " Select Trans.TransID from POSTransaction  Trans where Trans.TransDate >= DateAdd(D,-" + numNumberOfDays.Value + ",getdate())" +
        //                                " )  group by ItemId ) and OuterTransDetail.ItemId = itm.ItemId  and itemVendor.ItemId = itm.ItemId and Vendor.VendorId = itemVendor.VendorId " +
        //                            " group by itm.ItemId, itm.description,itm.QtyInStock,itm.ReOrderLevel,itm.PreferredVendor,Vendor.VendorCode , Vendor.VendorId  ,Vendor.VendorName, 	itemVendor.VendorItemId , itemVendor.VendorCostPrice";
        //                }
        //                else
        //                {
        //                    SQLFill = "Select Vendor.VendorCode , Vendor.VendorId  ,Vendor.VendorName, itemVendor.VendorItemId , itemVendor.VendorCostPrice AS PRICE, itm.ItemId, itm.description,itm.QtyInStock,itm.ReOrderLevel,itm.PreferredVendor,sum(OuterTransDetail.Qty) as QtySold " +
        //                            " from Item itm,  POSTransactionDetail OuterTransDetail, ItemVendor, Vendor where exists (" +
        //                             " select ItemId  from POSTransactionDetail TransDetail where TransDetail.TransId in " +
        //                               "(" +
        //                               " Select Trans.TransID from POSTransaction  Trans where Trans.TransDate >= cast('" + DateTime.Parse(dtpFromDate.Value.ToString()).ToString() + "' as datetime)" +
        //                               " )  group by ItemId ) and OuterTransDetail.ItemId = itm.ItemId  and itemVendor.ItemId = itm.ItemId and Vendor.VendorId = itemVendor.VendorId " +
        //                           " group by itm.ItemId, itm.description,itm.QtyInStock,itm.ReOrderLevel,itm.PreferredVendor,Vendor.VendorCode , Vendor.VendorId  ,Vendor.VendorName, 	itemVendor.VendorItemId , itemVendor.VendorCostPrice";
        //                }

        //            }
        //            else
        //            {
        //                SQLFill = qryOrderByReorder;


        //                SQLFill += " AND Vendor.VendorCode=itm.PREFERREDVENDOR ";
        //                SQLFill += qryGroupByClause;
        //            }
        //            #endregion
        //            break;
        //        case clsPOSDBConstants.LASTVENDOR:
        //            #region LAST Vendor
        //            if (optByName.CheckedIndex == 0)
        //            {
        //                SQLFill = qryOptByName;

        //                if (optSearchSet.CheckedIndex == 0)
        //                {
        //                    SQLFill += " AND Trans.TransDate >= DateAdd(D,-" + numNumberOfDays.Value + ",getdate()) ";
        //                }
        //                else
        //                {
        //                    SQLFill += " AND Trans.TransDate >= cast('" + DateTime.Parse(dtpFromDate.Value.ToString()).ToString() + "' as datetime) ";
        //                }

        //                SQLFill += " AND Vendor.VendorID=itm.LastVendor ";

        //            }
        //            else
        //            {
        //                SQLFill = qryOrderByReorder;


        //                SQLFill += " AND Vendor.VendorID=itm.LastVendor ";
        //            }
        //            SQLFill += qryGroupByClause;
        //            #endregion
        //            break;
        //        case clsPOSDBConstants.DEFAULTVENDOR:
        //            #region Default Vendor
        //            if (optByName.CheckedIndex == 0)
        //            {
        //                SQLFill = qryOptByName;

        //                if (optSearchSet.CheckedIndex == 0)
        //                {
        //                    SQLFill += " AND Trans.TransDate >= DateAdd(D,-" + numNumberOfDays.Value + ",getdate()) ";
        //                }
        //                else
        //                {
        //                    SQLFill += " AND Trans.TransDate >= cast('" + DateTime.Parse(dtpFromDate.Value.ToString()).ToString() + "' as datetime) ";
        //                }

        //                SQLFill += " AND Vendor.VendorCode='" + Configuration.CPOSSet.DefaultVendor + "'";

        //            }
        //            else
        //            {
        //                SQLFill = qryOrderByReorder;


        //                SQLFill += " AND Vendor.VendorCode='" + Configuration.CPOSSet.DefaultVendor + "'";
        //            }
        //            SQLFill += qryGroupByClause;
        //            #endregion
        //            break;
        //    }


        //    return (SQLFill);
        //}
        #endregion

        public System.String FillPO
        {
            get
            {
                return SQLFill;
            }
        }

        private void txtVendorCode_EditorButtonClick(object sender, Infragistics.Win.UltraWinEditors.EditorButtonEventArgs e)
        {
            Search();
            //lblDefaultVendor.Text = oSearch.Name;
        }
        private void Search()
        {
            //frmSearch oSearch = new frmSearch(clsPOSDBConstants.Vendor_tbl, this.txtVendorCode.Text, "");
            using (frmSearchMain oSearch = new frmSearchMain(clsPOSDBConstants.Vendor_tbl, this.txtVendorCode.Text, "", true))  //20-Dec-2017 JY Added new reference
            {
                oSearch.ShowDialog();
                if (oSearch.IsCanceled) return;
                txtVendorCode.Text = oSearch.SelectedRowID();
                txtVendorCode.Tag = oSearch.VendorID;   //PRIMEPOS-3155 12-Oct-2022 JY Added
            }
        }
        private void frmRptItemReOrder_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyData == System.Windows.Forms.Keys.F4)
                {
                    if (txtVendorCode.ContainsFocus)
                        txtVendorCode_EditorButtonClick(null, null);
                }
            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }

        private void chkDefaultVendor_CheckedChanged(object sender, EventArgs e)
        {
            if (chkForcedVendor.Checked)
            {
                UpdateAutoPOOptions();
                txtVendorCode.Enabled = true;
                grpAutoPO.Enabled = false;
            }
            else
            {
                txtVendorCode.Enabled = false;
                grpAutoPO.Enabled = true;
                LoadAutoPOSettings();
            }
            txtVendorCode.Text = string.Empty;
            txtVendorCode.Tag = string.Empty;   //PRIMEPOS-3155 12-Oct-2022 JY Added
            //lblDefaultVendor.Text = string.Empty;
        }

        /// <summary>
        /// Author: Gaurav
        /// Date: 18/06/2009
        /// Details: Update AutoPOSeq of UtilPOSSet for changes.
        /// </summary>
        private void UpdateAutoPOOptions()
        {
            List<string> objAutoPOSelectionList = new List<string>();
            if (ValidateAutoPOVendors())
            {

                for (int lstItmCnt = 0; lstItmCnt < lstSequence.Items.Count; lstItmCnt++)
                {
                    objAutoPOSelectionList.Add(lstSequence.Items[lstItmCnt].ToString());
                }
                Configuration.CPrimeEDISetting.AutoPOSeq = string.Join("|", objAutoPOSelectionList.ToArray());  //PRIMEPOS-3167 07-Nov-2022 JY Modified
            }
        }

        int IgnorVenorFlag = 0;//Flag aAdded by Krishna on 15 April 2011
        /// <summary>
        /// Author: Gaurav
        /// Date: 18/06/2009
        /// Details: Loads AutoPO setting sequence.
        /// </summary>
        private void LoadAutoPOSettings()
        {
            lstParent.Items.Clear();
            lstSequence.Items.Clear();
            List<string> objAutoPOSelectionList = new List<string>();
            objAutoPOSelectionList.AddRange(Configuration.CPrimeEDISetting.AutoPOSeq.Split('|'));   //PRIMEPOS-3167 07-Nov-2022 JY Modified
            if (!objAutoPOSelectionList.Contains(clsPOSDBConstants.PREFEREDVENDOR))
            {
                lstParent.Items.Add(clsPOSDBConstants.PREFEREDVENDOR);
            }
            if (!objAutoPOSelectionList.Contains(clsPOSDBConstants.LASTVENDOR))
            {
                lstParent.Items.Add(clsPOSDBConstants.LASTVENDOR);
            }
            if (!objAutoPOSelectionList.Contains(clsPOSDBConstants.DEFAULTVENDOR))
            {
                lstParent.Items.Add(clsPOSDBConstants.DEFAULTVENDOR);
            }

            //Added by Krishna on 15April2011
            if (!objAutoPOSelectionList.Contains(clsPOSDBConstants.IGNOREVENDOR))
            {
                lstParent.Items.Add(clsPOSDBConstants.IGNOREVENDOR);
            }
            //Till here by Krishna
            foreach (string str in objAutoPOSelectionList)
            {
                if (str != string.Empty)
                    lstSequence.Items.Add(str);
            }
            //chkLastVendor.Checked = objAutoPOSelectionList.Contains("LAST");
            //chkDefaultVendor.Checked = objAutoPOSelectionList.Contains("DEFAULT");
        }

        /// <summary>
        /// Author: Gaurav
        /// Date: 18-Jun-2009
        /// Validates AutoPO vendor selection sequence.
        /// </summary>
        /// <returns></returns>
        private bool ValidateAutoPOVendors()
        {
            bool AnyChecked = false;
            if (lstSequence.Items.Count > 0)
            {
                AnyChecked = true;
            }
            else
            {
                AnyChecked = false;
            }
            //if (!chkPreferedVendor.Checked && !chkLastVendor.Checked && !chkDefaultVendor.Checked)
            //{
            //    
            //}
            //else
            //{
            //    
            //}
            return (AnyChecked);
        }

        private void txtVendorCode_KeyPress(object sender, KeyPressEventArgs e)
        {
            //frmSearch oSearch = null;            
            if (e.KeyChar == (char)Keys.Enter)
            {
                string VendorID = CheckIfExactVendorExists(txtVendorCode.Text.Trim());
                if (VendorID == "")
                {
                    //oSearch = new frmSearch(clsPOSDBConstants.Vendor_tbl, this.txtVendorCode.Text, "");                    
                    //if (vendorcode != string.Empty)
                    //oSearch.VendorTextFromAutoPO = vendorcode;
                    using (frmSearchMain oSearch = new frmSearchMain(clsPOSDBConstants.Vendor_tbl, this.txtVendorCode.Text, "", true))  //20-Dec-2017 JY Added new reference
                    {
                        oSearch.ShowDialog();
                        if (oSearch.IsCanceled) return;
                        txtVendorCode.Text = oSearch.SelectedRowID();
                    }
                }
                else
                {
                    txtVendorCode.Tag = VendorID;
                }
                this.SelectNextControl(this.ActiveControl, true, true, true, true);
            }
        }

        private void chkPreferedVendor_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void chkLastVendor_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void chkDefaultVendor_CheckedChanged_1(object sender, EventArgs e)
        {

        }

        private void chkPreferedVendor_Click(object sender, EventArgs e)
        {


        }

        private void chkLastVendor_Click(object sender, EventArgs e)
        {


        }

        private void chkDefaultVendor_Click(object sender, EventArgs e)
        {


        }

        private void btnLToR_Click(object sender, EventArgs e)
        {
            if (lstParent.SelectedIndex > -1)
            {
                if (lstParent.SelectedItem.ToString() == clsPOSDBConstants.IGNOREVENDOR)
                {
                    for (int i = 0; i < lstSequence.Items.Count; i++)
                    {
                        lstParent.Items.Add(lstSequence.Items[i]);

                    }
                    lstSequence.Items.Clear();
                    lstSequence.Items.Insert(0, clsPOSDBConstants.IGNOREVENDOR);
                }
                else
                {
                    lstSequence.Items.Add(lstParent.SelectedItem);
                    if (lstSequence.Items.Contains(clsPOSDBConstants.IGNOREVENDOR))
                    {
                        lstSequence.Items.Remove(clsPOSDBConstants.IGNOREVENDOR);
                        lstParent.Items.Add(clsPOSDBConstants.IGNOREVENDOR);
                    }
                }

                lstParent.Items.Remove(lstParent.SelectedItem);
            }
        }

        private void btnRToL_Click(object sender, EventArgs e)
        {
            if (lstSequence.SelectedIndex > -1)
            {
                lstParent.Items.Add(lstSequence.SelectedItem);
                lstSequence.Items.Remove(lstSequence.SelectedItem);
                lstSequence.Refresh();
            }
        }

        private void btnUp_Click(object sender, EventArgs e)
        {
            if (lstSequence.SelectedItem != null)
            {
                string strItem = lstSequence.SelectedItem.ToString();
                int selInd = lstSequence.SelectedIndex;

                if (selInd > 0)
                {
                    lstSequence.Items.Remove(lstSequence.SelectedItem);
                    selInd--;
                    lstSequence.Items.Insert(selInd, strItem);
                    lstSequence.SelectedIndex = lstSequence.Items.IndexOf(strItem);
                }
            }
        }

        private void btnDown_Click(object sender, EventArgs e)
        {
            if (lstSequence.SelectedItem != null)
            {
                string strItem = lstSequence.SelectedItem.ToString();
                int selInd = lstSequence.SelectedIndex;

                if (selInd < lstSequence.Items.Count - 1)
                {
                    lstSequence.Items.Remove(lstSequence.SelectedItem);
                    selInd++;
                    lstSequence.Items.Insert(selInd, strItem);
                    lstSequence.SelectedIndex = lstSequence.Items.IndexOf(strItem);
                }
            }
        }

        private void dtpFromDate_ValueChanged(object sender, EventArgs e)
        {
            string fieldName = string.Empty;
            try
            {
                if (!validateFields(out fieldName))
                {
                    if (fieldName == "DATE")
                    {
                        clsUIHelper.ShowErrorMsg("From Date can not be greater than To Date.");
                        //Added By SRT(Ritesh Parekh)
                        dtpFromDate.Value = oldDate;
                        //End Of Added By SRT(Ritesh Parekh)
                    }

                    return;
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void dtpToDate_ValueChanged(object sender, EventArgs e)
        {
            string fieldName = string.Empty;
            try
            {
                if (!validateFields(out fieldName))
                {
                    if (fieldName == "DATE")
                    {
                        clsUIHelper.ShowErrorMsg("From Date can not be greater than To Date.");
                    }

                    return;
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void txtVendorCode_Enter(object sender, EventArgs e)
        {
            POSToolTip.Show(txtVendorCode, "Press F4 To Search", 200);
        }

        private string CheckIfExactVendorExists(string VendorCode)
        {
            //throw new Exception("The method or operation is not implemented.");
            string VendorID = string.Empty;
            string sqlQuery = "SELECT VendorID FROM Vendor WHERE VendorCode = '" + VendorCode.Replace("'", "''") + "'";
            using (IDataReader reader = DataHelper.ExecuteReader(sqlQuery, null))
            {
                if (reader.Read())
                {
                    VendorID = Configuration.convertNullToString(reader.GetValue(reader.GetOrdinal("VendorID")));
                }
            }
            return VendorID;
        }

        private void dtpFromDate_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == (int)Keys.Down || e.KeyValue == (int)Keys.Up)
            {
                int ChangeBy = -1;
                if (e.KeyValue == (int)Keys.Down)
                {
                    ChangeBy = -1;
                }
                else if (e.KeyValue == (int)Keys.Up)
                {
                    ChangeBy = 1;
                }

                int posInControl = dtpFromDate.SelectionStart;
                int posSelLength = dtpFromDate.SelectionLength;
                DateTime tempDate = (DateTime)dtpFromDate.Value;
                oldDate = (DateTime)dtpFromDate.Value;
                tempDate = ChangeValueDateTime(tempDate, posInControl, ChangeBy);
                dtpFromDate.Value = tempDate;
                dtpFromDate.SelectionStart = posInControl;
                dtpFromDate.SelectionLength = posSelLength;

                e.Handled = true;
            }
        }

        private void vScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {
            try
            {
                if (vScrollBar1.Value == vScrollBar1.Minimum)
                {
                    vScrollBar1.Value = Configuration.convertNullToInt(Configuration.convertNullToString(vScrollBar1.Maximum / 2));
                    e.NewValue = vScrollBar1.Value;
                }
                else
                {
                    if (e.NewValue != e.OldValue)
                    {
                        int ChangeBy = -1;
                        if (e.NewValue < e.OldValue)
                        {
                            ChangeBy = 1;
                        }
                        int posInControl = dtpFromDate.SelectionStart;
                        int posSelLength = dtpFromDate.SelectionLength;
                        DateTime tempDate = (DateTime)dtpFromDate.Value;
                        oldDate = (DateTime)dtpFromDate.Value;
                        tempDate = ChangeValueDateTime(tempDate, posInControl, ChangeBy);
                        dtpFromDate.Value = tempDate;
                        dtpFromDate.SelectionStart = posInControl;
                        dtpFromDate.SelectionLength = posSelLength;
                    }
                }
            }
            catch (Exception Ex)
            {
            }
        }

        private DateTime ChangeValueDateTime(DateTime ValToChange, int posInDateTime, int ChangeBy)
        {
            switch (posInDateTime)
            {
                case 0:
                case 1:
                    //Month To Process
                    ValToChange = ValToChange.AddMonths(ChangeBy);
                    break;
                case 3:
                case 4:
                    //Date To Process
                    ValToChange = ValToChange.AddDays(ChangeBy);
                    break;
                case 6:
                case 7:
                case 8:
                case 9:
                    //Year To Process
                    ValToChange = ValToChange.AddYears(ChangeBy);
                    break;
                case 11:
                case 12:
                    //Hour To Process
                    ValToChange = ValToChange.AddHours(ChangeBy);
                    break;
                case 14:
                case 15:
                    //Min TO Process
                    ValToChange = ValToChange.AddMinutes(ChangeBy);
                    break;
                case 17:
                case 18:
                    //AM /PM to process   
                    string str = ValToChange.ToString();
                    str = str.Trim().Substring(str.Trim().Length - 2);
                    if (str.ToUpper() == "AM")
                    {
                        str = ValToChange.ToString();
                        str = str.Substring(0, str.Length - 2) + "PM";
                    }
                    else
                    {
                        str = ValToChange.ToString();
                        str = str.Substring(0, str.Length - 2) + "AM";
                    }
                    ValToChange = DateTime.Parse(str);
                    break;
            }
            return (ValToChange);
        }

        private void vScrollBar2_Scroll(object sender, ScrollEventArgs e)
        {
            try
            {
                if (vScrollBar2.Value == vScrollBar2.Minimum)
                {
                    vScrollBar2.Value = Configuration.convertNullToInt(Configuration.convertNullToString(vScrollBar2.Maximum / 2));
                    e.NewValue = vScrollBar2.Value;
                }
                else
                {
                    if (e.NewValue != e.OldValue)
                    {
                        int ChangeBy = -1;
                        if (e.NewValue < e.OldValue)
                        {
                            ChangeBy = 1;
                        }
                        int posInControl = dtpToDate.SelectionStart;
                        int posSelLength = dtpToDate.SelectionLength;
                        DateTime tempDate = (DateTime)dtpToDate.Value;
                        tempDate = ChangeValueDateTime(tempDate, posInControl, ChangeBy);
                        dtpToDate.Value = tempDate;
                        dtpToDate.SelectionStart = posInControl;
                        dtpToDate.SelectionLength = posSelLength;
                    }
                }
            }
            catch (Exception Ex)
            {
            }
        }

        private void dtpToDate_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Down || e.KeyCode == Keys.Up)
            {
                int ChangeBy = -1;
                if (e.KeyCode == Keys.Down)
                {
                    ChangeBy = -1;
                }
                else if (e.KeyCode == Keys.Up)
                {
                    ChangeBy = 1;
                }

                int posInControl = dtpToDate.SelectionStart;
                int posSelLength = dtpToDate.SelectionLength;
                DateTime tempDate = (DateTime)dtpToDate.Value;
                tempDate = ChangeValueDateTime(tempDate, posInControl, ChangeBy);
                dtpToDate.Value = tempDate;
                dtpToDate.SelectionStart = posInControl;
                dtpToDate.SelectionLength = posSelLength;
                e.Handled = true;
            }
        }

        //Following 3  function Added by Krishna on 20 April 2011

        private PODetailTable CreateAutoPOByIgnoreVendor()
        {
            bool queryResult = false;
            PODetailTable podTable = null;

            DataSet dsTransHeader = new DataSet();

            ItemVendorData dsItemVendor = new ItemVendorData();
            ItemData dsItems = new ItemData();
            TransHeaderSvr oPOSTrans = new TransHeaderSvr();
            TransDetailSvr oPOSTransDetail = new TransDetailSvr();
            DataSet dsTransDetailData = new DataSet();
            //PODetailData dsTransDetailData = new PODetailData();

            ItemSvr oItem = new ItemSvr();
            ItemVendorSvr oItemVendor = new ItemVendorSvr();
            //Added by Amit Date 30 June 2011
            DepartmentData dsDepartmentData = new DepartmentData();
            DepartmentSvr oDepartment = new DepartmentSvr();
            SubDepartmentData dsSubDepartmentData = new SubDepartmentData();
            SubDepartmentSvr oSubDepartment = new SubDepartmentSvr();
            //End
            //Added by Amit Date 29 Nov 2011
            InvRecvHeaderSvr oInvRecHeader = new InvRecvHeaderSvr();
            DataSet dsInvRecDate = new DataSet();
            //End
            string SelectClause = string.Empty;
            if (optSearchSet.CheckedIndex == 0)
            {
                if (Configuration.CPrimeEDISetting.ConsiderReturnTrans)  //Sprint-27 - PRIMEPOS-2390 28-Sep-2017 JY Added    //PRIMEPOS-3167 07-Nov-2022 JY Modified 
                    SelectClause = " Trans.TransDate >= DateAdd(D,-" + numNumberOfDays.Value + ",getdate()) ";
                else
                    SelectClause = " Trans.TransDate >= DateAdd(D,-" + numNumberOfDays.Value + ",getdate()) AND Trans.TransType =1 ";
            }
            else
            {
                if (Configuration.CPrimeEDISetting.ConsiderReturnTrans)  //Sprint-27 - PRIMEPOS-2390 28-Sep-2017 JY Added    //PRIMEPOS-3167 07-Nov-2022 JY Modified 
                    SelectClause = " Trans.TransDate between cast('" + DateTime.Parse(dtpFromDate.Value.ToString()).ToString() + "' as datetime) AND cast('" + DateTime.Parse(dtpToDate.Value.ToString()).ToString() + "' as datetime) ";
                else
                    SelectClause = " Trans.TransDate between cast('" + DateTime.Parse(dtpFromDate.Value.ToString()).ToString() + "' as datetime) AND cast('" + DateTime.Parse(dtpToDate.Value.ToString()).ToString() + "' as datetime) AND Trans.TransType=1 ";
            }
            lblMessage.Text = "Aquiring Transactions........";
            Application.DoEvents();
            string strSQL = string.Empty;
            //dsTransHeader = oPOSTrans.PopulateList(SelectClause); //PRIMEPOS-3097 28-Jun-2022 JY commented
            dsTransHeader = oPOSTrans.PopulateList(SelectClause, out strSQL);   //PRIMEPOS-3097 28-Jun-2022 JY Added
            if (dsTransHeader == null || dsTransHeader.Tables.Count == 0 || dsTransHeader.Tables[0].Rows.Count == 0)
            {
                throw (new Exception("There are no more transactions in this period."));
            }
            #region PRIMEPOS-3097 28-Jun-2022 JY Commented
            //List<string> lstData = new List<string>();
            //foreach (DataRow Dr in dsTransHeader.Tables[0].Rows)
            //{
            //    lstData.Add(Dr[0].ToString());
            //}
            //dsTransDetailData = oPOSTransDetail.PopulateList(" where TransID in (" + string.Join(",", lstData.ToArray()) + ")", out queryResult);
            //lstData = new List<string>();
            //foreach (DataRow Dr in dsTransDetailData.Tables[0].Rows)
            //{
            //    lstData.Add("'" + Dr[0].ToString() + "'");
            //}
            #endregion

            string strDistItems = string.Empty;
            dsTransDetailData = oPOSTransDetail.PopulateList(" where TransID in (" + strSQL + ")", out queryResult, out strDistItems);    //PRIMEPOS-3097 28-Jun-2022 JY Added
            lblMessage.Text = "Aquiring Transaction Items........";
            Application.DoEvents();
            #region Sprint-19 - 1883 24-Mar-2015 JY Commented
            // //Start: Added By Amit Date 30 June 2011
            // List<string> lstDeptData = new List<string>();
            // List<string> lstSubDeptData = new List<string>();

            // if (this.txtDepartment.Text.Trim() != "" && txtDepartment.Tag.ToString() != ""
            //     && this.txtSubDepartment.Text.Trim() == "" && txtSubDepartment.Tag.ToString() == "")
            // {
            //     dsDepartmentData = oDepartment.PopulateList("where DeptCode in (" + this.txtDepartment.Tag.ToString().Trim() + ")");

            //     foreach (DataRow Dr in dsDepartmentData.Tables[0].Rows)
            //     {
            //         lstDeptData.Add("'" + Dr[0].ToString() + "'");
            //         Application.DoEvents();
            //     }
            //     dsSubDepartmentData = oSubDepartment.PopulateList("where DepartmentId in (" + string.Join(",", lstDeptData.ToArray()) + ")");

            //     dsItems = oItem.PopulateList(" where ItemId in (" + string.Join(",", lstData.ToArray()) + ") and DepartmentID in (" + string.Join(",", lstDeptData.ToArray()) + ")");
            // }
            // else if (this.txtDepartment.Text.Trim() == "" && txtDepartment.Tag.ToString() == ""
            //     && this.txtSubDepartment.Text.Trim() != "" && txtSubDepartment.Tag.ToString() != "")
            // {
            //     dsSubDepartmentData = oSubDepartment.PopulateList("where SubDepartmentId in (" + this.txtSubDepartment.Tag.ToString().Trim() + ")");
            //     foreach (DataRow Dr in dsSubDepartmentData.Tables[0].Rows)
            //     {
            //         lstSubDeptData.Add("'" + Dr[0].ToString() + "'");
            //         lstDeptData.Add("'" + Dr[1].ToString() + "'");
            //         Application.DoEvents();
            //     }
            //     dsDepartmentData = oDepartment.PopulateList("where DeptID in (" + string.Join(",", lstDeptData.ToArray()) + ")");

            //     dsItems = oItem.PopulateList(" where ItemId in (" + string.Join(",", lstData.ToArray()) + ") and SubDepartmentID in (" + string.Join(",", lstSubDeptData.ToArray()) + ")");
            // }
            // else if (this.txtDepartment.Text.Trim() != "" && txtDepartment.Tag.ToString() != ""
            //&& this.txtSubDepartment.Text.Trim() != "" && txtSubDepartment.Tag.ToString() != "")
            // {
            //     dsDepartmentData = oDepartment.PopulateList("where DeptCode in (" + this.txtDepartment.Tag.ToString().Trim() + ")");

            //     foreach (DataRow Dr in dsDepartmentData.Tables[0].Rows)
            //     {
            //         lstDeptData.Add("'" + Dr[0].ToString() + "'");
            //         Application.DoEvents();
            //     }

            //     dsSubDepartmentData = oSubDepartment.PopulateList("where SubDepartmentId in (" + this.txtSubDepartment.Tag.ToString().Trim() + ")");
            //     foreach (DataRow Dr in dsSubDepartmentData.Tables[0].Rows)
            //     {
            //         lstSubDeptData.Add("'" + Dr[0].ToString() + "'");
            //         Application.DoEvents();
            //     }

            //     dsItems = oItem.PopulateList(" where ItemId in (" + string.Join(",", lstData.ToArray()) + ") and DepartmentID in (" + string.Join(",", lstDeptData.ToArray()) + ") and SubDepartmentID in (" + string.Join(",", lstSubDeptData.ToArray()) + ")");

            // }
            // else
            // {
            //     dsDepartmentData = (DepartmentData)oDepartment.PopulateList(" where 1=1 ");
            //     dsSubDepartmentData = (SubDepartmentData)oSubDepartment.PopulateList(" where 1=1 ");

            //     dsItems = oItem.PopulateList(" where ItemId in (" + string.Join(",", lstData.ToArray()) + ")");
            // }
            // //End
            #endregion

            #region Sprint-19 - 1883 23-Mar-2015 JY Added location filter and also improved the existing filter code
            string strFilter = string.Empty;
            List<string> lstDeptData = new List<string>();
            List<string> lstSubDeptData = new List<string>();

            if (this.txtDepartment.Text.Trim() != "" && txtDepartment.Tag.ToString() != "")
            {
                dsDepartmentData = oDepartment.PopulateList("where DeptCode in (" + this.txtDepartment.Tag.ToString().Trim() + ")");

                foreach (DataRow Dr in dsDepartmentData.Tables[0].Rows)
                {
                    lstDeptData.Add("'" + Dr[0].ToString() + "'");
                }
                strFilter += " and DepartmentID in (" + string.Join(",", lstDeptData.ToArray()) + ")";

                if (this.txtSubDepartment.Text.Trim() != "" && txtSubDepartment.Tag.ToString() != "")
                {
                    dsSubDepartmentData = oSubDepartment.PopulateList("where SubDepartmentId in (" + this.txtSubDepartment.Tag.ToString().Trim() + ")");
                    foreach (DataRow Dr in dsSubDepartmentData.Tables[0].Rows)
                    {
                        lstSubDeptData.Add("'" + Dr[0].ToString() + "'");
                        lstDeptData.Add("'" + Dr[1].ToString() + "'");
                        Application.DoEvents();
                    }
                    strFilter += " and SubDepartmentID in (" + string.Join(",", lstSubDeptData.ToArray()) + ")";
                }
                else
                {
                    dsSubDepartmentData = oSubDepartment.PopulateList("where DepartmentId in (" + string.Join(",", lstDeptData.ToArray()) + ")");
                }
            }
            else
            {
                if (this.txtSubDepartment.Text.Trim() != "" && txtSubDepartment.Tag.ToString() != "")
                {
                    dsSubDepartmentData = oSubDepartment.PopulateList("where SubDepartmentId in (" + this.txtSubDepartment.Tag.ToString().Trim() + ")");
                    foreach (DataRow Dr in dsSubDepartmentData.Tables[0].Rows)
                    {
                        lstSubDeptData.Add("'" + Dr[0].ToString() + "'");
                        lstDeptData.Add("'" + Dr[1].ToString() + "'");
                        Application.DoEvents();
                    }
                    strFilter += " and SubDepartmentID in (" + string.Join(",", lstSubDeptData.ToArray()) + ")";

                    dsDepartmentData = oDepartment.PopulateList("where DeptID in (" + string.Join(",", lstDeptData.ToArray()) + ")");
                }
                else
                {
                    dsDepartmentData = (DepartmentData)oDepartment.PopulateList(" where 1=1 ");
                    dsSubDepartmentData = (SubDepartmentData)oSubDepartment.PopulateList(" where 1=1 ");
                }
            }

            if (this.txtItemLocation.Text.Trim() != "" && txtItemLocation.Tag.ToString() != "")
            {
                strFilter += " and LOCATION in (" + txtItemLocation.Text + ")";
            }
            //dsItems = oItem.PopulateList(" where ItemId in (" + string.Join(",", lstData.ToArray()) + ") " + strFilter);  //PRIMEPOS-3097 28-Jun-2022 JY Commented
            dsItems = oItem.PopulateList(" where ItemId in (" + strDistItems + ") " + strFilter);   //PRIMEPOS-3097 28-Jun-2022 JY Added
            #endregion

            //dsItemVendor = oItemVendor.PopulateList(" where ItemId in (" + string.Join(",", lstData.ToArray()) + ")");  //PRIMEPOS-3097 28-Jun-2022 JY Commented
            dsItemVendor = oItemVendor.PopulateList(" where ItemId in (" + strDistItems + ")", true);  //PRIMEPOS-3097 28-Jun-2022 JY Added
            if (dsItems == null || dsItems.Tables.Count == 0 || dsItems.Tables[0].Rows.Count == 0)
            {
                throw (new Exception("There are no more items matching transactions of selected period."));
            }
            //Added By Amit Date 29 Nov 2011
            //dsInvRecDate = oInvRecHeader.ItemInvRecDate(lstData);   //PRIMEPOS-3097 28-Jun-2022 JY Commented
            dsInvRecDate = oInvRecHeader.ItemInvRecDate(strDistItems);   //PRIMEPOS-3097 28-Jun-2022 JY Added
            //End
            //if (dsItemVendor == null || dsItemVendor.Tables.Count == 0 || dsItemVendor.Tables[0].Rows.Count == 0)
            //{
            //    throw (new Exception("There are no more vendor items matching transactions of selected period."));
            //}
            //int count = 0;

            PODetailRow podRow = null;
            //Added By Amit Date 30 June 2011
            int index = 0;
            //foreach (ItemRow oItemRow in dsItems.Tables[0].Rows)
            //Added By Amit date 29 July 2011            
            foreach (DataRow podRowTemp in dsTransDetailData.Tables[0].Rows)
            {
                DataRow[] oItemRowTemp = dsItems.Tables[0].Select("ItemID='" + podRowTemp["ItemID"] + "'");
                ItemRow oItemRow = null;

                if (oItemRowTemp.Length > 0)
                    oItemRow = (ItemRow)oItemRowTemp[0];

                else
                    continue;

                if (!oItemRow.ExclFromAutoPO)
                {
                    lblMessage.Text = "Scanning Item #" + oItemRow.ItemID.Trim() + " ";
                    Application.DoEvents();
                    DataRow[] oItemRows = dsItemVendor.Tables[0].Select(" ItemId='" + oItemRow.ItemID.Trim() + "'");

                    if (oItemRows != null && oItemRows.Length > 0)
                    {
                        ItemVendorRow oItemVendorRow = (ItemVendorRow)oItemRows[0];
                        #region Commented By Amit Date 27 July 2011
                        //DataRow[] TransRows = dsTransDetailData.Tables[0].Select(" ItemId='" + oItemRow.ItemID.Trim() + "'");

                        //if (TransRows != null && TransRows.Length > 0)
                        //{
                        //    decimal qty = 0;
                        //    foreach (DataRow dr in TransRows)
                        //    {
                        //        qty += Configuration.convertNullToDecimal(dr["Qty"].ToString());
                        //    }
                        //    count++;
                        #endregion
                        podRow = dsPoDetails.PODetail.NewPODetailRow();
                        podRow.ItemID = oItemRow.ItemID;
                        podRow.ItemDescription = oItemRow.Description;
                        podRow.VendorID = oItemVendorRow.VendorID;
                        podRow.VendorItemCode = oItemVendorRow.VendorItemID;
                        podRow.VendorName = oItemVendorRow.VendorCode;
                        podRow.QTY = Configuration.convertNullToInt(podRowTemp["QTY"].ToString());

                        podRow.QtyInStock = oItemRow.QtyInStock;
                        podRow.ReOrderLevel = oItemRow.ReOrderLevel;
                        //Modified by shitaljit as per Fahad for those Pharmacy which are not using PO 
                        //We should pupulate item cost from Item table not from ItemVendor table.
                        podRow.SoldItems = Configuration.convertNullToInt(podRowTemp["QTY"].ToString());
                        if (isFromPO == true)
                        {
                            podRow.Price = oItemVendorRow.VenorCostPrice;
                        }
                        else
                        {
                            podRow.Price = oItemRow.LastCostPrice;
                        }
                        podRow.PacketSize = oItemRow.PckSize;
                        podRow.PacketQuant = oItemRow.PckQty;
                        podRow.Packetunit = oItemRow.PckUnit;

                        //Start : Added By Amit Date 5 jul 2011
                        podRow.MinOrdQty = oItemRow.MinOrdQty;
                        DataRow[] DeptRow = dsDepartmentData.Tables[0].Select("DeptID=" + oItemRow.DepartmentID);
                        if (DeptRow.Length != 0)
                            podRow.DeptName = DeptRow[0][2].ToString();

                        DataRow[] SubDeptRow = dsSubDepartmentData.Tables[0].Select("SubDepartmentID=" + oItemRow.SubDepartmentID);

                        if (SubDeptRow.Length != 0)
                            podRow.SubDeptName = SubDeptRow[0][2].ToString();
                        //End
                        //Added by Amit Date 27 july 2011
                        podRow.RetailPrice = Configuration.convertNullToDecimal(podRowTemp["Price"].ToString());
                        podRow.ItemPrice = Configuration.convertNullToDecimal(podRowTemp["Price"].ToString()) - Configuration.convertNullToDecimal(podRowTemp["Discount"].ToString());
                        podRow.Discount = Configuration.convertNullToDecimal(podRowTemp["Discount"].ToString());
                        //End

                        //29 Nov 2011
                        DataRow[] InvRecDateRow = dsInvRecDate.Tables[0].Select(" ItemId='" + oItemRow.ItemID.Trim() + "'");
                        if (InvRecDateRow.Length != 0)
                            podRow.InvRecDate = (System.DateTime)InvRecDateRow[0][clsPOSDBConstants.PODetail_Fld_InvRecDate];
                        //End

                        dsPoDetails.PODetail.AddRow(podRow);

                        index++;

                        //}
                    }
                    else
                    {
                        #region Commented By Amit Date 27 July 2011
                        //DataRow[] TransRows = dsTransDetailData.Tables[0].Select(" ItemId='" + oItemRow.ItemID.Trim() + "'");

                        //if (TransRows != null && TransRows.Length > 0)
                        //{
                        //    decimal qty = 0;
                        //    foreach (DataRow dr in TransRows)
                        //    {
                        //        qty += Configuration.convertNullToDecimal(dr["Qty"].ToString());
                        //    }
                        //    count++;
                        #endregion
                        podRow = dsPoDetails.PODetail.NewPODetailRow();
                        podRow.ItemID = oItemRow.ItemID;
                        podRow.ItemDescription = oItemRow.Description;
                        podRow.VendorID = 0;
                        podRow.VendorItemCode = "";
                        podRow.VendorName = "";
                        podRow.QTY = Configuration.convertNullToInt(podRowTemp["Qty"].ToString());
                        podRow.QtyInStock = oItemRow.QtyInStock;
                        podRow.ReOrderLevel = oItemRow.ReOrderLevel;
                        //Modified by shitaljit as per Fahad for those Pharmacy which are not using PO 
                        //We should populate item cost from Item table not from ItemVendor table.
                        //podRow.Price = 0;
                        podRow.SoldItems = Configuration.convertNullToInt(podRowTemp["QTY"].ToString());
                        podRow.Price = oItemRow.LastCostPrice;

                        podRow.PacketSize = oItemRow.PckSize;
                        podRow.PacketQuant = oItemRow.PckQty;
                        podRow.Packetunit = oItemRow.PckUnit;
                        //Start : Added By Amit Date 5 jul 2011
                        podRow.MinOrdQty = oItemRow.MinOrdQty;

                        DataRow[] DeptRow = dsDepartmentData.Tables[0].Select("DeptID=" + oItemRow.DepartmentID);
                        if (DeptRow.Length != 0)
                            podRow.DeptName = DeptRow[0][2].ToString();
                        DataRow[] SubDeptRow = dsSubDepartmentData.Tables[0].Select("SubDepartmentID=" + oItemRow.SubDepartmentID);

                        if (SubDeptRow.Length != 0)
                            podRow.SubDeptName = SubDeptRow[0][2].ToString();
                        //End

                        //Added by Amit Date 27 july 2011
                        podRow.RetailPrice = Configuration.convertNullToDecimal(podRowTemp["Price"].ToString());
                        podRow.ItemPrice = Configuration.convertNullToDecimal(podRowTemp["Price"].ToString()) - Configuration.convertNullToDecimal(podRowTemp["Discount"].ToString());
                        podRow.Discount = Configuration.convertNullToDecimal(podRowTemp["Discount"].ToString());
                        //End

                        //29 Nov 2011
                        DataRow[] InvRecDateRow = dsInvRecDate.Tables[0].Select(" ItemId='" + oItemRow.ItemID.Trim() + "'");
                        if (InvRecDateRow.Length != 0)
                            podRow.InvRecDate = (System.DateTime)InvRecDateRow[0][clsPOSDBConstants.PODetail_Fld_InvRecDate];
                        //End
                        //Added By Shitaljit
                        if (oItemRow.PreferredVendor != "")
                        {
                            podRow.VendorName = oItemRow.PreferredVendor;
                        }
                        else if (oItemRow.LastVendor != "")
                        {
                            podRow.VendorName = oItemRow.LastVendor;
                        }
                        //End of added By Shitaljit.
                        dsPoDetails.PODetail.AddRow(podRow);

                        index++;

                        //}
                    }

                }
            }
            SetPacketToOrder(ref dsPoDetails);
            if (dsPoDetails.PODetail.Rows.Count > 0)
            {
                lblMessage.Text = dsPoDetails.PODetail.Rows.Count.ToString() + " No(s) Items Acquired For Purchase Order";
                Application.DoEvents();
            }
            return podTable;
        }

        private PODetailTable CreateAutoPOByIgnoreVendorReorder()
        {
            bool queryResult = false;

            PODetailTable podTable = null;

            DataSet dsTransHeader = new DataSet();
            DataSet dsTransDetail = new DataSet();
            ItemData dsItems = new ItemData();
            ItemVendorData dsItemVendor = new ItemVendorData();
            DepartmentData dsDepartment = new DepartmentData();
            SubDepartmentData dsSubDepartment = new SubDepartmentData();
            DataSet dsInvRecDate = new DataSet();

            GetDsForReorderLevel("Ignore Vendor", txtDepartment.Tag.ToString().Trim(), txtSubDepartment.Tag.ToString().Trim(), out dsTransHeader, out dsTransDetail, out dsItems, out dsItemVendor, out dsDepartment, out dsSubDepartment, out dsInvRecDate, out queryResult);

            //int count = 0;
            podTable = new PODetailTable();
            PODetailRow podRow = null;
            //Added By Amit Date 30 June 2011
            int index = 0;
            DataSet dtItems = null;

            if (this.chckUseTransHistoryForReOrder.Checked == true)
            {
                dtItems = dsTransDetail;
            }
            else if (dsItems != null)
            {
                if (dsItems.Tables[0].Rows.Count > 0)
                {
                    dtItems = dsItems;
                }
                else
                {
                    throw (new Exception("There are no items to "));
                }

            }
            //Added By Amit date 29 July 2011            
            foreach (DataRow podRowTemp in dtItems.Tables[0].Rows)
            {
                DataRow[] oItemRowTemp = dsItems.Tables[0].Select("ItemID='" + podRowTemp["ItemID"] + "'");
                ItemRow oItemRow = null;

                if (oItemRowTemp.Length > 0)
                    oItemRow = (ItemRow)oItemRowTemp[0];
                else
                    continue;
                //End              

                lblMessage.Text = "Scanning Item #" + oItemRow.ItemID.Trim() + "";
                Application.DoEvents();

                DataRow[] oItemRows = dsItemVendor.Tables[0].Select(" ItemId='" + oItemRow.ItemID.Trim() + "'");

                if (oItemRows != null && oItemRows.Length > 0)
                {
                    ItemVendorRow oItemVendorRow = (ItemVendorRow)oItemRows[0];
                    DataRow[] TransRows = dtItems.Tables[0].Select(" ItemId='" + oItemRow.ItemID.Trim() + "'");

                    if (TransRows != null && TransRows.Length > 0)
                    {
                        podRow = dsPoDetails.PODetail.NewPODetailRow();
                        podRow.ItemID = oItemRow.ItemID;
                        podRow.ItemDescription = oItemRow.Description;
                        podRow.VendorID = oItemVendorRow.VendorID;
                        podRow.VendorItemCode = oItemVendorRow.VendorItemID;
                        podRow.VendorName = oItemVendorRow.VendorCode;
                        ////Added By Amit Date 21 Oct 2011
                        //if (isFromPO)
                        //{
                        //   // podRow.QTY = oItemRow.MinOrdQty;
                        //    podRow.QTY = oItemRow.MinOrdQty-oItemRow.QtyInStock;  
                        //}
                        //else
                        //{
                        //    podRow.QTY = Configuration.convertNullToInt(podRowTemp["QTY"].ToString());
                        //}
                        ////End

                        //Added by Krishna on 19 December 2011
                        podRow.QTY = Configuration.convertNullToInt(oItemRow.MinOrdQty) - Configuration.convertNullToInt(oItemRow.QtyInStock);
                        if (this.chckUseTransHistoryForReOrder.Checked == true)
                        {

                            podRow.SoldItems = Configuration.convertNullToInt(podRowTemp["QTY"].ToString());
                            //Added by Amit Date 27 july 2011
                            podRow.RetailPrice = Configuration.convertNullToDecimal(podRowTemp["Price"].ToString());
                            podRow.ItemPrice = Configuration.convertNullToDecimal(podRowTemp["Price"].ToString()) - Configuration.convertNullToDecimal(podRowTemp["Discount"].ToString());
                            podRow.Discount = Configuration.convertNullToDecimal(podRowTemp["Discount"].ToString());
                            //End
                        }
                        //End of Added by Krishna

                        podRow.QtyInStock = oItemRow.QtyInStock;
                        podRow.ReOrderLevel = oItemRow.ReOrderLevel;

                        //Modified by shitaljit as per Fahad for those Pharmacy which are not using PO 
                        //We should pupulate item cost from Item table not from ItemVendor table.
                        if (isFromPO == true)
                        {
                            podRow.Price = oItemVendorRow.VenorCostPrice;
                        }
                        else
                        {
                            podRow.Price = oItemRow.LastCostPrice;
                        }

                        podRow.PacketSize = oItemRow.PckSize;
                        podRow.PacketQuant = oItemRow.PckQty;
                        podRow.Packetunit = oItemRow.PckUnit;

                        //Start : Added By Amit Date 5 jul 2011
                        podRow.MinOrdQty = oItemRow.MinOrdQty;
                        DataRow[] DeptRow = dsDepartment.Tables[0].Select("DeptID=" + oItemRow.DepartmentID);
                        if (DeptRow.Length != 0)
                            podRow.DeptName = DeptRow[0][2].ToString();

                        DataRow[] SubDeptRow = dsSubDepartment.Tables[0].Select("SubDepartmentID=" + oItemRow.SubDepartmentID);

                        if (SubDeptRow.Length != 0)
                            podRow.SubDeptName = SubDeptRow[0][2].ToString();
                        //End


                        //29 Nov 2011
                        DataRow[] InvRecDateRow = dsInvRecDate.Tables[0].Select(" ItemId='" + oItemRow.ItemID.Trim() + "'");
                        if (InvRecDateRow.Length != 0)
                            podRow.InvRecDate = (System.DateTime)InvRecDateRow[0][clsPOSDBConstants.PODetail_Fld_InvRecDate];
                        //End

                        dsPoDetails.PODetail.AddRow(podRow);

                        index++;

                    }

                }
                else if (!isFromPO)
                {
                    DataRow[] TransRows = dtItems.Tables[0].Select(" ItemId='" + oItemRow.ItemID.Trim() + "'");

                    if (TransRows != null && TransRows.Length > 0)
                    {
                        podRow = dsPoDetails.PODetail.NewPODetailRow();
                        podRow.ItemID = oItemRow.ItemID;
                        podRow.ItemDescription = oItemRow.Description;
                        podRow.VendorID = 0;
                        podRow.VendorItemCode = "";
                        podRow.VendorName = "";
                        if (this.chckUseTransHistoryForReOrder.Checked == true)
                        {

                            podRow.SoldItems = Configuration.convertNullToInt(podRowTemp["QTY"].ToString());
                            //Added by Amit Date 27 july 2011
                            podRow.RetailPrice = Configuration.convertNullToDecimal(podRowTemp["Price"].ToString());
                            podRow.ItemPrice = Configuration.convertNullToDecimal(podRowTemp["Price"].ToString()) - Configuration.convertNullToDecimal(podRowTemp["Discount"].ToString());
                            podRow.Discount = Configuration.convertNullToDecimal(podRowTemp["Discount"].ToString());
                            //End
                        }
                        podRow.QtyInStock = oItemRow.QtyInStock;
                        podRow.ReOrderLevel = oItemRow.ReOrderLevel;

                        //Modified by shitaljit as per Fahad for those Pharmacy which are not using PO 
                        //We should pupulate item cost from Item table not from ItemVendor table.
                        //podRow.Price = 0;
                        podRow.Price = oItemRow.LastCostPrice;

                        podRow.PacketSize = oItemRow.PckSize;
                        podRow.PacketQuant = oItemRow.PckQty;
                        podRow.Packetunit = oItemRow.PckUnit;

                        //Start : Added By Amit Date 5 jul 2011
                        podRow.MinOrdQty = oItemRow.MinOrdQty;
                        DataRow[] DeptRow = dsDepartment.Tables[0].Select("DeptID=" + oItemRow.DepartmentID);
                        if (DeptRow.Length != 0)
                            podRow.DeptName = DeptRow[0][2].ToString();

                        DataRow[] SubDeptRow = dsSubDepartment.Tables[0].Select("SubDepartmentID=" + oItemRow.SubDepartmentID);

                        if (SubDeptRow.Length != 0)
                            podRow.SubDeptName = SubDeptRow[0][2].ToString();
                        //End

                        //29 Nov 2011
                        DataRow[] InvRecDateRow = dsInvRecDate.Tables[0].Select(" ItemId='" + oItemRow.ItemID.Trim() + "'");
                        if (InvRecDateRow.Length != 0)
                            podRow.InvRecDate = (System.DateTime)InvRecDateRow[0][clsPOSDBConstants.PODetail_Fld_InvRecDate];
                        //End

                        dsPoDetails.PODetail.AddRow(podRow);

                        index++;

                    }

                }
            }
            SetPacketToOrder(ref dsPoDetails);

            if (dsPoDetails.PODetail.Rows.Count > 0)
            {
                if (optionReport.CheckedIndex == 0)
                {
                    PODetailData dsTempPoDetails = new PODetailData();
                    DataTable dtTemp = dsPoDetails.PODetail.DefaultView.ToTable(true, "ItemId");

                    lblMessage.Text = dtTemp.Rows.Count.ToString() + " No(s) Items Acquired For Purchase Order By Ignore Vendor";
                    Application.DoEvents();
                }
                else
                {
                    lblMessage.Text = dsPoDetails.PODetail.Rows.Count.ToString() + " No(s) Items Acquired For Purchase Order By Ignore Vendor";
                    Application.DoEvents();
                }
            }

            return podTable;
        }

        public PODetailTable CreateAutoPOByIgnoreVendorQtyZero()
        {
            #region Commented
            //Commented By Amit
            //ItemSvr oItemSvr = new ItemSvr();
            //itemreorder orptm = new itemreorder();
            //ItemData oitemData = new ItemData();
            //lblMessage.Text = " Aquiring Items...";
            //Application.DoEvents();
            //oitemData = oItemSvr.PopulateList(" Where " + clsPOSDBConstants.Item_Fld_QtyInStock + " <=0 AND SellingPrice > 0 ");
            //List<string> lstData = new List<string>();

            //lblMessage.Text = oitemData.Tables[0].Rows.Count.ToString() + " No(s) Items found having quantity Zero or Negetive";
            //Application.DoEvents();

            //clsReports.Preview(false, oitemData, orptm);
            //End
            #endregion

            PODetailTable podTable = null;
            //Added By Amit Date 4 Aug 2011
            DataSet dsTransDetailData = new DataSet();
            TransDetailSvr oPOSTransDetail = new TransDetailSvr();
            //End
            ItemVendorData dsItemVendor = new ItemVendorData();
            ItemData dsItems = new ItemData();
            ItemSvr oItem = new ItemSvr();
            ItemVendorSvr oItemVendor = new ItemVendorSvr();
            DepartmentData dsDepartmentData = new DepartmentData();
            DepartmentSvr oDepartment = new DepartmentSvr();
            SubDepartmentData dsSubDepartmentData = new SubDepartmentData();
            SubDepartmentSvr oSubDepartment = new SubDepartmentSvr();
            //Added By Amit Date 29 Nov 2011
            InvRecvHeaderSvr oInvRecHeader = new InvRecvHeaderSvr();
            DataSet dsInvRecDate = new DataSet();
            //End

            //List<string> lstData = new List<string>();
            List<string> lstDeptData = new List<string>();
            List<string> lstSubDeptData = new List<string>();

            #region Sprint-19 - 1883 24-Mar-2015 JY Commented
            // if (this.txtDepartment.Text.Trim() != "" && txtDepartment.Tag.ToString() != ""
            //     && this.txtSubDepartment.Text.Trim() == "" && txtSubDepartment.Tag.ToString() == "")
            // {
            //     dsDepartmentData = oDepartment.PopulateList("where DeptCode in (" + this.txtDepartment.Tag.ToString().Trim() + ")");

            //     foreach (DataRow Dr in dsDepartmentData.Tables[0].Rows)
            //     {
            //         lstDeptData.Add("'" + Dr[0].ToString() + "'");
            //         Application.DoEvents();
            //     }
            //     dsItems = oItem.PopulateList(" where DepartmentID in (" + string.Join(",", lstDeptData.ToArray()) + ") AND " + clsPOSDBConstants.Item_Fld_QtyInStock + " <= 0");
            //     dsSubDepartmentData = oSubDepartment.PopulateList("where DepartmentId in (" + string.Join(",", lstDeptData.ToArray()) + ")");


            // }
            // else if (this.txtDepartment.Text.Trim() == "" && txtDepartment.Tag.ToString() == ""
            //     && this.txtSubDepartment.Text.Trim() != "" && txtSubDepartment.Tag.ToString() != "")
            // {
            //     dsSubDepartmentData = oSubDepartment.PopulateList("where SubDepartmentId in (" + this.txtSubDepartment.Tag.ToString().Trim() + ")");
            //     foreach (DataRow Dr in dsSubDepartmentData.Tables[0].Rows)
            //     {
            //         lstSubDeptData.Add("'" + Dr[0].ToString() + "'");
            //         lstDeptData.Add("'" + Dr[1].ToString() + "'");
            //         Application.DoEvents();
            //     }
            //     dsDepartmentData = oDepartment.PopulateList("where DeptID in (" + string.Join(",", lstDeptData.ToArray()) + ")");
            //     dsItems = oItem.PopulateList(" where DepartmentID in (" + string.Join(",", lstDeptData.ToArray()) + ") AND " + clsPOSDBConstants.Item_Fld_QtyInStock + " <= 0");

            // }
            // else if (this.txtDepartment.Text.Trim() != "" && txtDepartment.Tag.ToString() != ""
            //&& this.txtSubDepartment.Text.Trim() != "" && txtSubDepartment.Tag.ToString() != "")
            // {
            //     dsDepartmentData = oDepartment.PopulateList("where DeptCode in (" + this.txtDepartment.Tag.ToString().Trim() + ")");

            //     foreach (DataRow Dr in dsDepartmentData.Tables[0].Rows)
            //     {
            //         lstDeptData.Add("'" + Dr[0].ToString() + "'");
            //         Application.DoEvents();
            //     }

            //     dsSubDepartmentData = oSubDepartment.PopulateList("where SubDepartmentId in (" + this.txtSubDepartment.Tag.ToString().Trim() + ")");
            //     foreach (DataRow Dr in dsSubDepartmentData.Tables[0].Rows)
            //     {
            //         lstSubDeptData.Add("'" + Dr[0].ToString() + "'");
            //         Application.DoEvents();
            //     }
            //     dsItems = oItem.PopulateList(" where DepartmentID in (" + string.Join(",", lstDeptData.ToArray()) + ") and SubDepartmentId in (" + this.txtSubDepartment.Tag.ToString().Trim() + ") AND " + clsPOSDBConstants.Item_Fld_QtyInStock + " <= 0");
            // }
            #endregion

            #region Sprint-19 - 1883 23-Mar-2015 JY Added location filter and also improved the existing filter code
            string strFilter = string.Empty;

            if (this.txtDepartment.Text.Trim() != "" && txtDepartment.Tag.ToString() != "")
            {
                dsDepartmentData = oDepartment.PopulateList("where DeptCode in (" + this.txtDepartment.Tag.ToString().Trim() + ")");

                foreach (DataRow Dr in dsDepartmentData.Tables[0].Rows)
                {
                    lstDeptData.Add("'" + Dr[0].ToString() + "'");
                }
                strFilter += " and DepartmentID in (" + string.Join(",", lstDeptData.ToArray()) + ")";

                if (this.txtSubDepartment.Text.Trim() != "" && txtSubDepartment.Tag.ToString() != "")
                {
                    dsSubDepartmentData = oSubDepartment.PopulateList("where SubDepartmentId in (" + this.txtSubDepartment.Tag.ToString().Trim() + ")");
                    foreach (DataRow Dr in dsSubDepartmentData.Tables[0].Rows)
                    {
                        lstSubDeptData.Add("'" + Dr[0].ToString() + "'");
                        lstDeptData.Add("'" + Dr[1].ToString() + "'");
                        Application.DoEvents();
                    }
                    strFilter += " and SubDepartmentID in (" + string.Join(",", lstSubDeptData.ToArray()) + ")";
                }
                else
                {
                    dsSubDepartmentData = oSubDepartment.PopulateList("where DepartmentId in (" + string.Join(",", lstDeptData.ToArray()) + ")");
                }
            }
            else
            {
                if (this.txtSubDepartment.Text.Trim() != "" && txtSubDepartment.Tag.ToString() != "")
                {
                    dsSubDepartmentData = oSubDepartment.PopulateList("where SubDepartmentId in (" + this.txtSubDepartment.Tag.ToString().Trim() + ")");
                    foreach (DataRow Dr in dsSubDepartmentData.Tables[0].Rows)
                    {
                        lstSubDeptData.Add("'" + Dr[0].ToString() + "'");
                        lstDeptData.Add("'" + Dr[1].ToString() + "'");
                        Application.DoEvents();
                    }
                    strFilter += " and SubDepartmentID in (" + string.Join(",", lstSubDeptData.ToArray()) + ")";
                    dsDepartmentData = oDepartment.PopulateList("where DeptID in (" + string.Join(",", lstDeptData.ToArray()) + ")");
                }
                else
                {
                    dsDepartmentData = (DepartmentData)oDepartment.PopulateList(" where 1=1 ");
                    dsSubDepartmentData = (SubDepartmentData)oSubDepartment.PopulateList(" where 1=1 ");
                }
            }

            if (this.txtItemLocation.Text.Trim() != "" && txtItemLocation.Tag.ToString() != "")
            {
                strFilter += " and LOCATION in (" + txtItemLocation.Text + ")";
            }
            dsItems = oItem.PopulateList(" where " + clsPOSDBConstants.Item_Fld_QtyInStock + " <= 0" + strFilter);
            #endregion

            #region Consider Transaction history in the item populate for Zero or -Ve qty logic
            //added By Shitaljit to consider Transaction history in Zero or Negative Qty.
            // JIRA link -PRIMEPOS -13
            bool queryResult = false;
            string strDistItems = string.Empty;
            if (this.chckUseTransHistoryForReOrder.Checked == true)
            {
                dsDepartmentData = (DepartmentData)oDepartment.PopulateList(" where 1=1 ");
                dsSubDepartmentData = (SubDepartmentData)oSubDepartment.PopulateList(" where 1=1 ");
                string SelectClause = string.Empty;
                if (Configuration.CPrimeEDISetting.ConsiderReturnTrans)  //Sprint-27 - PRIMEPOS-2390 28-Sep-2017 JY Added    //PRIMEPOS-3167 07-Nov-2022 JY Modified 
                    SelectClause = " Trans.TransDate >= DateAdd(D,-" + this.numNoOfDaysForItemReOrder.Value.ToString() + ",getdate()) ";
                else
                    SelectClause = " Trans.TransDate >= DateAdd(D,-" + this.numNoOfDaysForItemReOrder.Value.ToString() + ",getdate()) AND Trans.TransType=1 ";
                Application.DoEvents();
                TransHeaderSvr oPOSTrans = new TransHeaderSvr();
                string strSQL = string.Empty;
                //DataSet dsTransHeader = oPOSTrans.PopulateList(SelectClause);   //PRIMEPOS-3097 28-Jun-2022 JY commented
                DataSet dsTransHeader = oPOSTrans.PopulateList(SelectClause, out strSQL);   //PRIMEPOS-3097 28-Jun-2022 JY Added
                if (dsTransHeader == null || dsTransHeader.Tables.Count == 0 || dsTransHeader.Tables[0].Rows.Count == 0)
                {
                    throw (new Exception("There are no more transactions in this last " + this.numNoOfDaysForItemReOrder.Value.ToString() + " day(s)."));
                }

                #region PRIMEPOS-3097 28-Jun-2022 JY Commented
                //foreach (DataRow Dr in dsTransHeader.Tables[0].Rows)
                //{
                //    lstData.Add(Dr[0].ToString());
                //}
                //string whereClause = " where TransID in (" + string.Join(",", lstData.ToArray()) + ")";
                //lstData.Clear();
                //foreach (DataRow Dr in dsItems.Tables[0].Rows)
                //{
                //    lstData.Add("'" + Dr[0].ToString() + "'");
                //    Application.DoEvents();
                //}
                //if (lstData.Count > 0)
                //{
                //    whereClause += "AND ItemID in (" + string.Join(",", lstData.ToArray()) + ")";
                //}
                //dsTransDetailData = oPOSTransDetail.PopulateList(whereClause);
                //lstData.Clear();
                //foreach (DataRow Dr in dsTransDetailData.Tables[0].Rows)
                //{
                //    lstData.Add("'" + Dr[0].ToString() + "'");
                //    Application.DoEvents();
                //}
                //dsItems = oItem.PopulateList(" where ItemID in (" + string.Join(",", lstData.ToArray()) + ") AND " + clsPOSDBConstants.Item_Fld_QtyInStock + " <= 0");
                #endregion

                #region PRIMEPOS-3097 28-Jun-2022 JY Added                
                dsTransDetailData = oPOSTransDetail.PopulateList(" where TransID in (" + strSQL + ") AND ItemID in (SELECT ItemID FROM Item where QtyInStock <= 0)", out queryResult, out strDistItems);
                dsItems = oItem.PopulateList(" where ItemID in (" + strDistItems + ") AND " + clsPOSDBConstants.Item_Fld_QtyInStock + " <= 0");
                #endregion
            }
            else
            {
                dsDepartmentData = (DepartmentData)oDepartment.PopulateList(" where 1=1 ");
                dsSubDepartmentData = (SubDepartmentData)oSubDepartment.PopulateList(" where 1=1 ");
                dsItems = oItem.PopulateList(" where " + clsPOSDBConstants.Item_Fld_QtyInStock + " <= 0");
                #region PRIMEPOS-3097 28-Jun-2022 JY Commented
                //foreach (DataRow Dr in dsItems.Tables[0].Rows)
                //{
                //    lstData.Add("'" + Dr[0].ToString() + "'");
                //    Application.DoEvents();
                //}
                #endregion
                //Added By Amit Date 4 Aug 2011
                //dsTransDetailData = oPOSTransDetail.PopulateList(" where ItemID in (" + string.Join(",", lstData.ToArray()) + ")");   //PRIMEPOS-3097 28-Jun-2022 JY Commented                
                dsTransDetailData = oPOSTransDetail.PopulateList(" where ItemID in (SELECT ItemID FROM Item where QtyInStock <= 0)", out queryResult, out strDistItems);    //PRIMEPOS-3097 28-Jun-2022 JY Added
                if (dsTransDetailData == null || dsTransDetailData.Tables.Count == 0 || dsTransDetailData.Tables[0].Rows.Count == 0)
                {
                    throw (new Exception("There are no items found which are used in Transaction."));
                }
                //End
            }
            #endregion
            //dsItemVendor = oItemVendor.PopulateList(" where ItemId in (" + string.Join(",", lstData.ToArray()) + ")");  //PRIMEPOS-3097 28-Jun-2022 JY commented
            dsItemVendor = oItemVendor.PopulateList(" where ItemId in (" + strDistItems + ")", true);  //PRIMEPOS-3097 28-Jun-2022 JY Added
            if (dsItems == null || dsItems.Tables.Count == 0 || dsItems.Tables[0].Rows.Count == 0)
            {
                throw (new Exception("There are no items found."));
            }
            //Commented By Amit Date 15 july 2011
            //if (dsItemVendor == null || dsItemVendor.Tables.Count == 0 || dsItemVendor.Tables[0].Rows.Count == 0)
            //{
            //    throw (new Exception("There are no items found."));
            //}
            //int count = 0;

            //Added by Amit Date 29 Nov 2011
            //dsInvRecDate = oInvRecHeader.ItemInvRecDate(lstData);   //PRIMEPOS-3097 28-Jun-2022 JY Commented
            dsInvRecDate = oInvRecHeader.ItemInvRecDate(strDistItems);   //PRIMEPOS-3097 28-Jun-2022 JY Added
            //End
            podTable = new PODetailTable();
            PODetailRow podRow = null;
            int index = 0;
            //foreach (ItemRow oItemRow in dsItems.Tables[0].Rows)
            //Added By Amit date 29 July 2011            
            foreach (DataRow podRowTemp in dsTransDetailData.Tables[0].Rows)
            {
                DataRow[] oItemRowTemp = dsItems.Tables[0].Select("ItemID='" + podRowTemp["ItemID"] + "'");
                ItemRow oItemRow = null;

                if (oItemRowTemp.Length > 0)
                    oItemRow = (ItemRow)oItemRowTemp[0];
                else
                    continue;
                //End

                lblMessage.Text = "Scanning Item #" + oItemRow.ItemID.Trim() + " For Ignore Vendor";
                Application.DoEvents();
                DataRow[] oItemRows = dsItemVendor.Tables[0].Select(" ItemId='" + oItemRow.ItemID.Trim() + "'");

                if (oItemRows != null && oItemRows.Length > 0)
                {
                    ItemVendorRow oItemVendorRow = (ItemVendorRow)oItemRows[0];
                    podRow = dsPoDetails.PODetail.NewPODetailRow();
                    podRow.ItemID = oItemRow.ItemID;
                    podRow.ItemDescription = oItemRow.Description;
                    podRow.VendorID = oItemVendorRow.VendorID;
                    podRow.VendorItemCode = oItemVendorRow.VendorItemID;
                    podRow.VendorName = oItemVendorRow.VendorCode;
                    podRow.QTY = Configuration.convertNullToInt(podRowTemp["QTY"].ToString());
                    podRow.QtyInStock = oItemRow.QtyInStock;
                    podRow.ReOrderLevel = oItemRow.ReOrderLevel;
                    //Modified by shitaljit as per Fahad for those Pharmacy which are not using PO 
                    //We should populate item cost from Item table not from ItemVendor table.
                    podRow.SoldItems = Configuration.convertNullToInt(podRowTemp["QTY"].ToString());
                    if (isFromPO == true)
                    {
                        podRow.Price = oItemVendorRow.VenorCostPrice;
                    }
                    else
                    {
                        podRow.Price = oItemRow.LastCostPrice;
                    }

                    podRow.PacketSize = oItemRow.PckSize;
                    podRow.PacketQuant = oItemRow.PckQty;
                    podRow.Packetunit = oItemRow.PckUnit;

                    podRow.MinOrdQty = oItemRow.MinOrdQty;
                    DataRow[] DeptRow = dsDepartmentData.Tables[0].Select("DeptID=" + oItemRow.DepartmentID);
                    if (DeptRow.Length != 0)
                        podRow.DeptName = DeptRow[0][2].ToString();

                    DataRow[] SubDeptRow = dsSubDepartmentData.Tables[0].Select("SubDepartmentID=" + oItemRow.SubDepartmentID);

                    if (SubDeptRow.Length != 0)
                        podRow.SubDeptName = SubDeptRow[0][2].ToString();
                    //Added by Amit Date 4 Aug 2011
                    podRow.RetailPrice = Configuration.convertNullToDecimal(podRowTemp["Price"].ToString());
                    podRow.ItemPrice = Configuration.convertNullToDecimal(podRowTemp["Price"].ToString()) - Configuration.convertNullToDecimal(podRowTemp["Discount"].ToString());
                    podRow.Discount = Configuration.convertNullToDecimal(podRowTemp["Discount"].ToString());
                    //End
                    //29 Nov 2011
                    DataRow[] InvRecDateRow = dsInvRecDate.Tables[0].Select(" ItemId='" + oItemRow.ItemID.Trim() + "'");
                    if (InvRecDateRow.Length != 0)
                        podRow.InvRecDate = (System.DateTime)InvRecDateRow[0][clsPOSDBConstants.PODetail_Fld_InvRecDate];
                    //End

                    dsPoDetails.PODetail.AddRow(podRow);

                    index++;
                }
                else
                {
                    podRow = dsPoDetails.PODetail.NewPODetailRow();
                    podRow.ItemID = oItemRow.ItemID;
                    podRow.ItemDescription = oItemRow.Description;
                    podRow.VendorID = 0;
                    podRow.VendorItemCode = "";
                    podRow.VendorName = "";
                    podRow.QTY = podRow.QTY = Configuration.convertNullToInt(podRowTemp["QTY"].ToString());
                    podRow.SoldItems = Configuration.convertNullToInt(podRowTemp["QTY"].ToString());
                    podRow.QtyInStock = oItemRow.QtyInStock;
                    podRow.ReOrderLevel = oItemRow.ReOrderLevel;
                    //Modified by shitaljit as per Fahad for those Pharmacy which are not using PO 
                    //We should pupulate item cost from Item table not from ItemVendor table.
                    //podRow.Price = 0;
                    podRow.Price = oItemRow.LastCostPrice;

                    podRow.PacketSize = oItemRow.PckSize;
                    podRow.PacketQuant = oItemRow.PckQty;
                    podRow.Packetunit = oItemRow.PckUnit;

                    podRow.MinOrdQty = oItemRow.MinOrdQty;
                    DataRow[] DeptRow = dsDepartmentData.Tables[0].Select("DeptID=" + oItemRow.DepartmentID);
                    if (DeptRow.Length != 0)
                        podRow.DeptName = DeptRow[0][2].ToString();

                    DataRow[] SubDeptRow = dsSubDepartmentData.Tables[0].Select("SubDepartmentID=" + oItemRow.SubDepartmentID);

                    if (SubDeptRow.Length != 0)
                        podRow.SubDeptName = SubDeptRow[0][2].ToString();
                    //Added by Amit Date 4 Aug 2011
                    podRow.RetailPrice = Configuration.convertNullToDecimal(podRowTemp["Price"].ToString());
                    podRow.ItemPrice = Configuration.convertNullToDecimal(podRowTemp["Price"].ToString()) - Configuration.convertNullToDecimal(podRowTemp["Discount"].ToString());
                    podRow.Discount = Configuration.convertNullToDecimal(podRowTemp["Discount"].ToString());
                    //End
                    //29 Nov 2011
                    DataRow[] InvRecDateRow = dsInvRecDate.Tables[0].Select(" ItemId='" + oItemRow.ItemID.Trim() + "'");
                    if (InvRecDateRow.Length != 0)
                        podRow.InvRecDate = (System.DateTime)InvRecDateRow[0][clsPOSDBConstants.PODetail_Fld_InvRecDate];
                    //End

                    dsPoDetails.PODetail.AddRow(podRow);

                    index++;

                }
                Application.DoEvents();
            }
            SetPacketToOrder(ref dsPoDetails);
            podTable = dsPoDetails.PODetail;
            if (dsPoDetails.PODetail.Rows.Count > 0)
            {
                lblMessage.Text = dsPoDetails.PODetail.Rows.Count.ToString() + " No(s) Items Acquired For Purchase Order By Ignore Vendor";
                Application.DoEvents();
            }
            return podTable;
        }
        //Till here Added by Krishna on 20 April 2011

        //Added By Amit
        private void txtDepartment_EditorButtonClick(object sender, Infragistics.Win.UltraWinEditors.EditorButtonEventArgs e)
        {
            SearchDept();
        }

        //Added By Amit
        private void SearchDept()
        {
            try
            {
                //frmSearch oSearch = new frmSearch(clsPOSDBConstants.Department_tbl);
                using (frmSearchMain oSearch = new frmSearchMain(true)) //20-Dec-2017 JY Added new reference
                {
                    oSearch.SearchTable = clsPOSDBConstants.Department_tbl; //20-Dec-2017 JY Added 
                    oSearch.AllowMultiRowSelect = true;
                    oSearch.SearchInConstructor = true;
                    oSearch.ShowDialog(this);
                    if (!oSearch.IsCanceled)
                    {
                        //string strDeptCode = "";
                        string strDeptName = string.Empty;
                        string strDeptID = string.Empty;
                        foreach (UltraGridRow oRow in oSearch.grdSearch.Rows)
                        {
                            if ((bool)oRow.Cells["check"].Value == true)
                            {
                                //strDeptCode += ",'" + oRow.Cells["Code"].Text + "'";
                                strDeptName += "," + oRow.Cells["Name"].Text;
                                strDeptID += "," + oRow.Cells["Id"].Text;
                            }
                        }

                        if (strDeptID.StartsWith(","))
                        {
                            //strDeptCode = strDeptCode.Substring(1);
                            strDeptName = strDeptName.Substring(1);
                            strDeptID = strDeptID.Substring(1);
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
            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }

        //Added By Amit
        private void txtSubDepartment_EditorButtonClick(object sender, Infragistics.Win.UltraWinEditors.EditorButtonEventArgs e)
        {
            SearchSubDept();
        }
        //Added By Amit
        private void SearchSubDept()
        {
            try
            {
                POS_Core.BusinessRules.Department oDept = new Department();
                DepartmentData oDeptData = null;
                DepartmentRow oDeptRow = null;
                List<string> listDept = new List<string>();
                if (txtDepartment.Tag.ToString() != "")
                {
                    oDeptData = oDept.PopulateList(" AND DeptID IN (" + txtDepartment.Tag.ToString() + ")");
                }
                else
                {
                    oDeptData = oDept.PopulateList("");
                }
                for (int RowIndex = 0; RowIndex < oDeptData.Tables[0].Rows.Count; RowIndex++)
                {
                    oDeptRow = oDeptData.Department[RowIndex];
                    listDept.Add(oDeptRow.DeptID.ToString());
                }
                string InQuery = string.Join("','", listDept.ToArray());
                if (InQuery != "")
                {
                    SearchSvr.SubDeptIDFlag = true;
                }
                //frmSearch oSearch = new frmSearch(clsPOSDBConstants.SubDepartment_tbl, InQuery, "");
                using (frmSearchMain oSearch = new frmSearchMain(clsPOSDBConstants.SubDepartment_tbl, InQuery, "", true))   //20-Dec-2017 JY Added new reference
                {
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
                }
                SearchSvr.SubDeptIDFlag = false;
            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }

        //Added By Amit Date 1 July 2011
        private void optSearchSet_ValueChanged(object sender, EventArgs e)
        {
            if (optSearchSet.CheckedIndex == 0)
            {
                this.numNumberOfDays.Enabled = true;
                this.numNumberOfDays.Focus();
                this.dtpFromDate.Enabled = false;
                this.dtpToDate.Enabled = false;
            }
            else
            {
                this.numNumberOfDays.Enabled = false;
                this.dtpFromDate.Enabled = true;
                this.dtpFromDate.Focus();
                this.dtpToDate.Enabled = true;
            }

        }
        //Added By Amit Date 10 20 2011

        private void GetDsForReorderLevel(string ForWhichCat, string Department, string SubDepartment, out DataSet dsTransHeader,
                                            out DataSet dsTransDetail, out ItemData dsItems, out ItemVendorData dsItemVendor,
                                            out DepartmentData dsDepartmentData, out SubDepartmentData dsSubDepartmentData,
                                            out DataSet dsInvRecDate, out bool queryResult)
        {
            ItemSvr oItem = new ItemSvr();
            DepartmentSvr oDepartment = new DepartmentSvr();
            SubDepartmentSvr oSubDepartment = new SubDepartmentSvr();
            TransHeaderSvr oPOSTrans = new TransHeaderSvr();
            TransDetailSvr oPOSTransDetail = new TransDetailSvr();
            ItemVendorSvr oItemVendor = new ItemVendorSvr();
            InvRecvHeaderSvr oInvRecHeader = new InvRecvHeaderSvr();

            List<string> lstDeptData = new List<string>();
            List<string> lstSubDeptData = new List<string>();
            dsTransHeader = null;
            dsTransDetail = null;
            queryResult = false;
            string SelectClause = string.Empty;
            //List<string> lstData = new List<string>();
            string ItemIDInClause = " 1=1 ";
            string strDistItems = string.Empty;
            //following if is added by shitaljit on 4 sept 2012 to implenment ne settings.
            //UseTransHistoryForAutoPO and 
            if (this.chckUseTransHistoryForReOrder.Checked == true)
            {
                //SelectClause = " Trans.TransDate >= DateAdd(D,-" + clsPOSDBConstants.ItemReorderPeriod.ToString() + ",getdate()) AND Trans.TransType=1 ";
                if (Configuration.CPrimeEDISetting.ConsiderReturnTrans)  //Sprint-27 - PRIMEPOS-2390 28-Sep-2017 JY Added    //PRIMEPOS-3167 07-Nov-2022 JY Modified 
                    SelectClause = " Trans.TransDate >= DateAdd(D,-" + this.numNoOfDaysForItemReOrder.Value.ToString() + ",getdate()) ";
                else
                    SelectClause = " Trans.TransDate >= DateAdd(D,-" + this.numNoOfDaysForItemReOrder.Value.ToString() + ",getdate()) AND Trans.TransType=1 ";
                lblMessage.Text = "Aquiring Transactions For " + ForWhichCat + " Vendor.";
                Application.DoEvents();

                string strSQL = string.Empty;
                //dsTransHeader = oPOSTrans.PopulateList(SelectClause);   //PRIMEPOS-3097 28-Jun-2022 JY commented
                dsTransHeader = oPOSTrans.PopulateList(SelectClause, out strSQL);   //PRIMEPOS-3097 28-Jun-2022 JY Added
                if (dsTransHeader == null || dsTransHeader.Tables.Count == 0 || dsTransHeader.Tables[0].Rows.Count == 0)
                {
                    //throw (new Exception("There are no more transactions in this last " + clsPOSDBConstants.ItemReorderPeriod.ToString() + " day(s)."));
                    throw (new Exception("There are no more transactions in this last " + this.numNoOfDaysForItemReOrder.Value.ToString() + " day(s)."));
                }
                #region PRIMEPOS-3097 28-Jun-2022 JY Commented
                //foreach (DataRow Dr in dsTransHeader.Tables[0].Rows)
                //{
                //    lstData.Add(Dr[0].ToString());
                //}
                //dsTransDetail = oPOSTransDetail.PopulateList(" where TransID in (" + string.Join(",", lstData.ToArray()) + ")", out queryResult);
                //lstData = new List<string>();
                //foreach (DataRow Dr in dsTransDetail.Tables[0].Rows)
                //{
                //    lstData.Add("'" + Dr[0].ToString() + "'");
                //}
                #endregion                
                dsTransDetail = oPOSTransDetail.PopulateList(" where TransID in (" + strSQL + ")", out queryResult, out strDistItems);    //PRIMEPOS-3097 28-Jun-2022 JY Added
                lblMessage.Text = "Aquiring Transaction Items For " + ForWhichCat + " Vendor.";
                Application.DoEvents();
            }
            #region PRIMEPOS-3097 28-Jun-2022 JY Commented
            //if (lstData.Count > 0)
            //{
            //    ItemIDInClause = "ItemId in (" + string.Join(",", lstData.ToArray()) + ")";
            //}
            #endregion
            ItemIDInClause = "ItemId in (" + strDistItems + ")";    //PRIMEPOS-3097 28-Jun-2022 JY Added

            #region Sprint-19 - 1883 23-Mar-2015 JY Commented
            //if (Department != "" && SubDepartment == "")
            //{
            //    dsDepartmentData = oDepartment.PopulateList("where DeptCode in (" + Department + ")");

            //    foreach (DataRow Dr in dsDepartmentData.Tables[0].Rows)
            //    {
            //        lstDeptData.Add("'" + Dr[0].ToString() + "'");
            //        Application.DoEvents();
            //    }

            //    dsSubDepartmentData = oSubDepartment.PopulateList("where DepartmentId in (" + string.Join(",", lstDeptData.ToArray()) + ")");

            //    dsItems = oItem.PopulateList(" where " + ItemIDInClause + "  and QtyInStock <= ReOrderLevel and ReOrderLevel > 0 and QtyInStock < MinOrdQty and DepartmentID in (" + string.Join(",", lstDeptData.ToArray()) + ")");
            //}
            //else if (Department == "" && SubDepartment != "")
            //{
            //    dsSubDepartmentData = oSubDepartment.PopulateList("where SubDepartmentId in (" + SubDepartment + ")");

            //    foreach (DataRow Dr in dsSubDepartmentData.Tables[0].Rows)
            //    {
            //        lstSubDeptData.Add("'" + Dr[0].ToString() + "'");
            //        lstDeptData.Add("'" + Dr[1].ToString() + "'");
            //        Application.DoEvents();
            //    }
            //    dsDepartmentData = oDepartment.PopulateList("where DeptID in (" + string.Join(",", lstDeptData.ToArray()) + ")");

            //    dsItems = oItem.PopulateList(" where " + ItemIDInClause + " and QtyInStock <= ReOrderLevel and ReOrderLevel > 0 and QtyInStock < MinOrdQty and SubDepartmentID in (" + string.Join(",", lstSubDeptData.ToArray()) + ")");
            //}
            //else if (Department != "" && SubDepartment != "")
            //{
            //    dsDepartmentData = oDepartment.PopulateList("where DeptCode in (" + Department + ")");

            //    foreach (DataRow Dr in dsDepartmentData.Tables[0].Rows)
            //    {
            //        lstDeptData.Add("'" + Dr[0].ToString() + "'");
            //        Application.DoEvents();
            //    }

            //    dsSubDepartmentData = oSubDepartment.PopulateList("where SubDepartmentId in (" + SubDepartment + ")");
            //    foreach (DataRow Dr in dsSubDepartmentData.Tables[0].Rows)
            //    {
            //        lstSubDeptData.Add("'" + Dr[0].ToString() + "'");
            //        Application.DoEvents();
            //    }
            //    dsItems = oItem.PopulateList(" where " + ItemIDInClause + " and QtyInStock <= ReOrderLevel and ReOrderLevel > 0 and QtyInStock < MinOrdQty and DepartmentID in (" + string.Join(",", lstDeptData.ToArray()) + ") and SubDepartmentID in (" + string.Join(",", lstSubDeptData.ToArray()) + ")");
            //}
            //else
            //{
            //    dsDepartmentData = (DepartmentData)oDepartment.PopulateList(" where 1=1 ");
            //    dsSubDepartmentData = (SubDepartmentData)oSubDepartment.PopulateList(" where 1=1 ");

            //    dsItems = oItem.PopulateList(" where " + ItemIDInClause + " and QtyInStock <= ReOrderLevel and ReOrderLevel > 0 and QtyInStock < MinOrdQty and ExclFromAutoPO=0 ");
            //}
            #endregion

            #region Sprint-19 - 1883 23-Mar-2015 JY Added location filter and also improved the existing filter code
            string strFilter = string.Empty;

            if (Department != "")
            {
                dsDepartmentData = oDepartment.PopulateList("where DeptCode in (" + Department + ")");

                foreach (DataRow Dr in dsDepartmentData.Tables[0].Rows)
                {
                    lstDeptData.Add("'" + Dr[0].ToString() + "'");
                }
                strFilter += " and DepartmentID in (" + string.Join(",", lstDeptData.ToArray()) + ")";

                if (SubDepartment != "")
                {
                    dsSubDepartmentData = oSubDepartment.PopulateList("where SubDepartmentId in (" + SubDepartment + ")");
                    foreach (DataRow Dr in dsSubDepartmentData.Tables[0].Rows)
                    {
                        lstSubDeptData.Add("'" + Dr[0].ToString() + "'");
                        lstDeptData.Add("'" + Dr[1].ToString() + "'");
                        Application.DoEvents();
                    }
                    strFilter += " and SubDepartmentID in (" + string.Join(",", lstSubDeptData.ToArray()) + ")";
                }
                else
                {
                    dsSubDepartmentData = oSubDepartment.PopulateList("where DepartmentId in (" + string.Join(",", lstDeptData.ToArray()) + ")");
                }
            }
            else
            {
                if (SubDepartment != "")
                {
                    dsSubDepartmentData = oSubDepartment.PopulateList("where SubDepartmentId in (" + SubDepartment + ")");
                    foreach (DataRow Dr in dsSubDepartmentData.Tables[0].Rows)
                    {
                        lstSubDeptData.Add("'" + Dr[0].ToString() + "'");
                        lstDeptData.Add("'" + Dr[1].ToString() + "'");
                        Application.DoEvents();
                    }
                    strFilter += " and SubDepartmentID in (" + string.Join(",", lstSubDeptData.ToArray()) + ")";

                    dsDepartmentData = oDepartment.PopulateList("where DeptID in (" + string.Join(",", lstDeptData.ToArray()) + ")");
                }
                else
                {
                    dsDepartmentData = (DepartmentData)oDepartment.PopulateList(" where 1=1 ");
                    dsSubDepartmentData = (SubDepartmentData)oSubDepartment.PopulateList(" where 1=1 ");
                }
            }

            if (this.txtItemLocation.Text.Trim() != "" && txtItemLocation.Tag.ToString() != "")
            {
                strFilter += " and LOCATION in (" + txtItemLocation.Text + ")";
            }
            dsItems = oItem.PopulateList(" where " + ItemIDInClause + " and QtyInStock <= ReOrderLevel and ReOrderLevel > 0 and QtyInStock < MinOrdQty and ExclFromAutoPO=0 " + strFilter);
            #endregion

            if (dsItems == null || dsItems.Tables.Count == 0 || dsItems.Tables[0].Rows.Count == 0)
            {
                throw (new Exception("There are no more items matching transactions of selected period."));
            }

            //dsItemVendor = oItemVendor.PopulateList(" where " + ItemIDInClause);  //PRIMEPOS-3097 28-Jun-2022 JY Commented
            dsItemVendor = oItemVendor.PopulateList(" where " + ItemIDInClause, true);  //PRIMEPOS-3097 28-Jun-2022 JY Added

            if (dsItemVendor == null || dsItemVendor.Tables.Count == 0 || dsItemVendor.Tables[0].Rows.Count == 0 && ForWhichCat != "Ignore Vendor")
            {
                throw (new Exception("There are no more vendor items matching transactions of selected period."));
            }
            //------28 Nov 2011

            //dsInvRecDate = oInvRecHeader.ItemInvRecDate(lstData);   //PRIMEPOS-3097 28-Jun-2022 JY Commented
            dsInvRecDate = oInvRecHeader.ItemInvRecDate(strDistItems);   //PRIMEPOS-3097 28-Jun-2022 JY Added
        }

        private void chckUseTransHistoryForReOrder_CheckedChanged(object sender, EventArgs e)
        {
            this.numNoOfDaysForItemReOrder.Enabled = this.chckUseTransHistoryForReOrder.Checked;

        }

        #region Sprint-19 - 1883 17-Mar-2015 JY Added for Item Location
        private void txtItemLocation_EditorButtonClick(object sender, Infragistics.Win.UltraWinEditors.EditorButtonEventArgs e)
        {
            SearchItemLocation();
        }

        private void SearchItemLocation()
        {
            try
            {
                //frmSearch oSearch = new frmSearch("ITEMLOCATION");
                using (frmSearchMain oSearch = new frmSearchMain(true)) //20-Dec-2017 JY Added new reference
                {
                    oSearch.SearchTable = "ITEMLOCATION";   //20-Dec-2017 JY Added 
                    oSearch.AllowMultiRowSelect = true;
                    oSearch.SearchInConstructor = true;
                    oSearch.ShowDialog(this);
                    if (!oSearch.IsCanceled)
                    {
                        string strLocation = "";
                        foreach (UltraGridRow oRow in oSearch.grdSearch.Rows)
                        {
                            if ((bool)oRow.Cells["check"].Value == true)
                            {
                                strLocation += ",'" + oRow.Cells["Location"].Text.Replace("'","''") + "'";
                            }
                        }

                        if (strLocation.StartsWith(","))
                            strLocation = strLocation.Substring(1);

                        txtItemLocation.Text = strLocation;
                        txtItemLocation.Tag = strLocation;
                    }
                    else
                    {
                        txtItemLocation.Text = string.Empty;
                        txtItemLocation.Tag = string.Empty;
                    }
                }
            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }
        #endregion

        #region PRIMEPOS-3155 12-Oct-2022 JY Added
        private void AutoPONew()
        {
            try
            {
                if (chkForcedVendor.Checked)//Forced Vendor
                {
                    if (optByName.CheckedIndex == 0 || optByName.CheckedIndex == 2)    //Item Sold OR Zero/ Negative Quantity Items
                    {
                        dsPoDetails = CreateAutoPOBySpecifiedVendor();
                    }
                    else if (optByName.CheckedIndex == 1)   //Re-Order Level
                    {
                        dsPoDetails = CreateAutoPOBySpecifiedVendorReorder();
                    }
                }
                else
                {
                    if (lstSequence.Items.Count == 1 && lstSequence.Items.Contains(clsPOSDBConstants.PREFEREDVENDOR)) //Only Prefered Vendor
                    {
                        if (optByName.CheckedIndex == 0 || optByName.CheckedIndex == 2)    //Item Sold OR Zero/ Negative Quantity Items
                        {
                            dsPoDetails = CreateAutoPOBySpecifiedVendor();
                        }
                        else if (optByName.CheckedIndex == 1)   //Re-Order Level
                        {
                            dsPoDetails = CreateAutoPOBySpecifiedVendorReorder();
                        }
                    }
                    else if (lstSequence.Items.Count == 1 && lstSequence.Items.Contains(clsPOSDBConstants.LASTVENDOR)) //Only Last Vendor
                    {
                        if (optByName.CheckedIndex == 0 || optByName.CheckedIndex == 2)    //Item Sold OR Zero/ Negative Quantity Items
                        {
                            dsPoDetails = CreateAutoPOBySpecifiedVendor();
                        }
                        else if (optByName.CheckedIndex == 1)   //Re-Order Level
                        {
                            dsPoDetails = CreateAutoPOBySpecifiedVendorReorder();
                        }
                    }
                    else if (lstSequence.Items.Count == 1 && lstSequence.Items.Contains(clsPOSDBConstants.DEFAULTVENDOR)) //Only Default Vendor
                    {
                        if (optByName.CheckedIndex == 0 || optByName.CheckedIndex == 2)    //Item Sold OR Zero/ Negative Quantity Items
                        {
                            dsPoDetails = CreateAutoPOBySpecifiedVendor();
                        }
                        else if (optByName.CheckedIndex == 1)   //Re-Order Level
                        {
                            dsPoDetails = CreateAutoPOBySpecifiedVendorReorder();
                        }
                    }
                    else if (lstSequence.Items.Count == 1 && lstSequence.Items.Contains(clsPOSDBConstants.IGNOREVENDOR)) //Only Ignore Vendor
                    {
                        if (optByName.CheckedIndex == 0 || optByName.CheckedIndex == 2)    //Item Sold OR Zero/ Negative Quantity Items
                        {
                            dsPoDetails = CreateAutoPOForIgnoreVendor();
                        }
                        else if (optByName.CheckedIndex == 1)   //Re-Order Level
                        {
                            dsPoDetails = CreateAutoPOForIgnoreVendorReorder();
                        }
                    }
                    else if (lstSequence.Items.Count > 1)   //sequence - multiple vendor selection
                    {
                        dsPoDetails = CreateAutoPOForSequence();
                    }
                }

                if (dsPoDetails != null && dsPoDetails.Tables.Count > 0 && dsPoDetails.Tables[0].Rows.Count > 0)
                {
                    foreach (PODetailRow row in dsPoDetails.PODetail.Rows)
                    {
                        //update ItemPrice
                        if (row.Discount > 0 && row.RetailPrice > row.Discount)
                            row.ItemPrice = row.RetailPrice - row.Discount;
                        else
                            row.ItemPrice = row.RetailPrice;

                        row.SoldItems = row.QTY;    //SoldItems
                    }
                }
            }
            catch (Exception ex)
            {
                clsUIHelper.ShowErrorMsg(ex.Message);
            }
        }

        private PODetailData CreateAutoPOBySpecifiedVendor()
        {
            try
            {
                using (PODetailSvr oPODetailSvr = new PODetailSvr())
                {
                    string strWhereClause = GenerateWhereClause();
                    string strQty = " CASE WHEN ISNUMERIC(I.PCKSIZE) = 1 AND ISNUMERIC(I.PCKQTY) = 1 AND ISNULL(I.PCKUNIT,'') IN('CA', 'CS')" +
                                 " THEN CASE WHEN ISNULL(I.MinOrdQty,0) > ISNULL(CAST(LTRIM(RTRIM(I.PCKSIZE)) AS decimal), 0) * ISNULL(CAST(LTRIM(RTRIM(I.PCKQTY)) AS decimal), 0)" +
                                           " THEN CASE WHEN ISNULL(CAST(LTRIM(RTRIM(I.PCKSIZE)) AS decimal),0) > 0 THEN ABS(CEILING(SUM(PTD.Qty) / CAST(LTRIM(RTRIM(I.PCKSIZE)) AS decimal))) END" +
                                      " ELSE 1 END" +
                                " ELSE ABS(SUM(PTD.Qty)) END AS Qty";
                    string strPrice = (isFromPO == true ? " IV.VendorCostPrice " : " I.LastCostPrice ");
                    string strSQL = GenerateSqlForSpecifiedVendor(strWhereClause, strQty, strPrice);
                    dsPoDetails = oPODetailSvr.Populate(strSQL);
                }
            }
            catch (Exception ex)
            {
                clsUIHelper.ShowErrorMsg(ex.Message);
            }
            return dsPoDetails;
        }

        private PODetailData CreateAutoPOBySpecifiedVendorReorder()
        {
            try
            {
                using (PODetailSvr oPODetailSvr = new PODetailSvr())
                {
                    string strWhereClause = GenerateWhereClause();
                    string strQty = " CASE WHEN ISNUMERIC(I.PCKSIZE) = 1 AND ISNUMERIC(I.PCKQTY) = 1 AND ISNULL(I.PCKUNIT,'') IN ('CA','CS')" +
                                 " THEN CASE WHEN ISNULL(I.MinOrdQty,0) > ISNULL(CAST(LTRIM(RTRIM(I.PCKSIZE)) AS decimal), 0) * ISNULL(CAST(LTRIM(RTRIM(I.PCKQTY)) AS decimal), 0)" +
                                           " THEN CASE WHEN ISNULL(CAST(LTRIM(RTRIM(I.PCKSIZE)) AS decimal),0) > 0 THEN ABS(CEILING(ISNULL(I.MinOrdQty,0) -ISNULL(I.QtyInStock, 0) / CAST(LTRIM(RTRIM(I.PCKSIZE)) AS decimal))) END" +
                                      " ELSE 1 END" +
                            " ELSE ABS(ISNULL(I.MinOrdQty, 0) - ISNULL(I.QtyInStock, 0)) END AS Qty";
                    string strPrice = (isFromPO == true ? " IV.VendorCostPrice " : " I.LastCostPrice ");
                    string strSQL = GenerateSqlForSpecifiedVendor(strWhereClause, strQty, strPrice);
                    dsPoDetails = oPODetailSvr.Populate(strSQL);
                }
            }
            catch (Exception ex)
            {
                clsUIHelper.ShowErrorMsg(ex.Message);
            }
            return dsPoDetails;
        }

        private PODetailData CreateAutoPOForIgnoreVendor()
        {
            try
            {
                using (PODetailSvr oPODetailSvr = new PODetailSvr())
                {
                    string strWhereClause = GenerateWhereClause();
                    string strQty = " CASE WHEN ISNUMERIC(I.PCKSIZE) = 1 AND ISNUMERIC(I.PCKQTY) = 1 AND ISNULL(I.PCKUNIT,'') IN('CA', 'CS')" +
                         " THEN CASE WHEN ISNULL(I.MinOrdQty,0) > ISNULL(CAST(LTRIM(RTRIM(I.PCKSIZE)) AS decimal), 0) * ISNULL(CAST(LTRIM(RTRIM(I.PCKQTY)) AS decimal), 0)" +
                                   " THEN CASE WHEN ISNULL(CAST(LTRIM(RTRIM(I.PCKSIZE)) AS decimal),0) > 0 THEN ABS(CEILING(PTD.Qty / CAST(LTRIM(RTRIM(I.PCKSIZE)) AS decimal))) END" +
                              " ELSE 1 END" +
                        " ELSE ABS(PTD.Qty) END AS Qty";
                    string strPrice = (isFromPO == true ? " IV.VendorCostPrice " : " I.LastCostPrice ");
                    string strSQL = GenerateSqlForIgnoreVendor(strWhereClause, strQty, strPrice);
                    dsPoDetails = oPODetailSvr.Populate(strSQL);
                }
            }
            catch (Exception ex)
            {
                clsUIHelper.ShowErrorMsg(ex.Message);
            }
            return dsPoDetails;
        }

        private PODetailData CreateAutoPOForIgnoreVendorReorder()
        {
            try
            {
                using (PODetailSvr oPODetailSvr = new PODetailSvr())
                {
                    string strWhereClause = GenerateWhereClause();
                    string strQty = " CASE WHEN ISNUMERIC(I.PCKSIZE) = 1 AND ISNUMERIC(I.PCKQTY) = 1 AND ISNULL(I.PCKUNIT,'') IN ('CA','CS')" +
                                 " THEN CASE WHEN ISNULL(I.MinOrdQty,0) > ISNULL(CAST(LTRIM(RTRIM(I.PCKSIZE)) AS decimal), 0) * ISNULL(CAST(LTRIM(RTRIM(I.PCKQTY)) AS decimal), 0)" +
                                           " THEN CASE WHEN ISNULL(CAST(LTRIM(RTRIM(I.PCKSIZE)) AS decimal),0) > 0 THEN ABS(CEILING(ISNULL(I.MinOrdQty,0) -ISNULL(I.QtyInStock, 0) / CAST(LTRIM(RTRIM(I.PCKSIZE)) AS decimal))) END" +
                                      " ELSE 1 END" +
                            " ELSE ABS(ISNULL(I.MinOrdQty, 0) - ISNULL(I.QtyInStock, 0)) END AS Qty";
                    string strPrice = (isFromPO == true ? " IV.VendorCostPrice " : " I.LastCostPrice ");
                    string strSQL = GenerateSqlForIgnoreVendor(strWhereClause, strQty, strPrice);
                    dsPoDetails = oPODetailSvr.Populate(strSQL);
                }
            }
            catch (Exception ex)
            {
                clsUIHelper.ShowErrorMsg(ex.Message);
            }
            return dsPoDetails;
        }

        private PODetailData CreateAutoPOForSequence()
        {
            try
            {
                string strPrice = (isFromPO == true ? " IV.VendorCostPrice " : " I.LastCostPrice ");
                string FinalSql = string.Empty;
                for (int i = 0; i < lstSequence.Items.Count; i++)
                {
                    string strQty = string.Empty;
                    if (optByName.CheckedIndex == 0 || optByName.CheckedIndex == 2)    //Item Sold OR Zero/ Negative Quantity Items
                    {
                        strQty = " CASE WHEN ISNUMERIC(I.PCKSIZE) = 1 AND ISNUMERIC(I.PCKQTY) = 1 AND ISNULL(I.PCKUNIT,'') IN('CA', 'CS')" +
                                     " THEN CASE WHEN ISNULL(I.MinOrdQty,0) > ISNULL(CAST(LTRIM(RTRIM(I.PCKSIZE)) AS decimal), 0) * ISNULL(CAST(LTRIM(RTRIM(I.PCKQTY)) AS decimal), 0)" +
                                               " THEN CASE WHEN ISNULL(CAST(LTRIM(RTRIM(I.PCKSIZE)) AS decimal),0) > 0 THEN ABS(CEILING(SUM(PTD.Qty) / CAST(LTRIM(RTRIM(I.PCKSIZE)) AS decimal))) END" +
                                          " ELSE 1 END" +
                                    " ELSE ABS(SUM(PTD.Qty)) END AS Qty";
                    }
                    else if (optByName.CheckedIndex == 1)   //Re-Order Level
                    {
                        strQty = " CASE WHEN ISNUMERIC(I.PCKSIZE) = 1 AND ISNUMERIC(I.PCKQTY) = 1 AND ISNULL(I.PCKUNIT,'') IN ('CA','CS')" +
                                 " THEN CASE WHEN ISNULL(I.MinOrdQty,0) > ISNULL(CAST(LTRIM(RTRIM(I.PCKSIZE)) AS decimal), 0) * ISNULL(CAST(LTRIM(RTRIM(I.PCKQTY)) AS decimal), 0)" +
                                           " THEN CASE WHEN ISNULL(CAST(LTRIM(RTRIM(I.PCKSIZE)) AS decimal),0) > 0 THEN ABS(CEILING(ISNULL(I.MinOrdQty,0) -ISNULL(I.QtyInStock, 0) / CAST(LTRIM(RTRIM(I.PCKSIZE)) AS decimal))) END" +
                                      " ELSE 1 END" +
                            " ELSE ABS(ISNULL(I.MinOrdQty, 0) - ISNULL(I.QtyInStock, 0)) END AS Qty";
                    }

                    string strWhereClause = GenerateWhereClause(lstSequence.Items[i].ToString());

                    if (FinalSql == "")
                        FinalSql = GenerateSqlForSpecifiedVendorSequence(strWhereClause, strQty, strPrice, i);
                    else
                        FinalSql += " UNION " + GenerateSqlForSpecifiedVendorSequence(strWhereClause, strQty, strPrice, i);
                }

                FinalSql = "SELECT ROW_NUMBER() OVER(ORDER BY Description ASC) AS PODetailID, ItemID, Price, Qty, VendorID, Description, VendorName, VendorItemCode, QtyInStock, ReOrderLevel, [Packet Size], [Packet Quantity], [Packet Unit], SoldItems, MinOrdQty, DeptName, SubDeptName, RetailPrice, ItemPrice, Discount, InvRecDate FROM" +
                        " (" +
                        " SELECT ROW_NUMBER() OVER(PARTITION BY ItemID ORDER BY Seq, Description) AS rNum, * FROM" +
                        " (" + FinalSql +
                        " ) X" +
                        " ) Y WHERE rNum = 1";

                using (PODetailSvr oPODetailSvr = new PODetailSvr())
                {
                    dsPoDetails = oPODetailSvr.Populate(FinalSql);
                }
            }
            catch (Exception ex)
            {
                clsUIHelper.ShowErrorMsg(ex.Message);
            }
            return dsPoDetails;
        }


        private string GenerateSqlForSpecifiedVendor(string strWhereClause, string strQty, string strPrice)
        {
            string strSQL = "with cteTemp as (select ROW_NUMBER() Over(Partition by ItemID Order by ItemDetailID desc) as ItemDetailID1,* from ItemVendor ) " + //PRIMEPOS-3274
                "SELECT ROW_NUMBER() OVER(ORDER BY I.Description ASC) AS PODetailID, I.ItemID, " + strPrice + " AS Price," +
                        strQty +
                        ", V.VendorID, ISNULL(I.Description, '') AS Description, V.VendorCode AS VendorName, IV.VendorItemID AS VendorItemCode," +
                        " ISNULL(I.QtyInStock, 0) AS QtyInStock, ISNULL(I.ReOrderLevel, 0) AS ReOrderLevel, I.PCKSIZE AS[Packet Size], I.PCKQTY AS[Packet Quantity], I.PCKUNIT AS[Packet Unit]," +
                        " 0 AS SoldItems, ISNULL(I.MinOrdQty, 0) AS MinOrdQty, ISNULL(D.DeptName, '') AS DeptName, ISNULL(SD.Description, '') AS SubDeptName," +
                           " ISNULL(I.SellingPrice, 0.00) AS RetailPrice, 0.00 AS ItemPrice," +
                        " CASE WHEN ISNULL(I.isDiscountable,0) = 1 THEN" +
                            " (CASE WHEN ISNULL(I.DiscountPolicy, '') = 'I' THEN ISNULL(I.SellingPrice, 0) * ISNULL(I.Discount, 0) / 100" +
                                " WHEN ISNULL(I.DiscountPolicy, '') = 'D' THEN ISNULL(I.SellingPrice, 0) * (SELECT Discount FROM Department WHERE DeptID = I.DepartmentID) / 100" +
                                " ELSE 0 END)" +
                        " ELSE 0 END AS Discount," +
                        " IRD.InvRecDate FROM POSTransaction PT" +
                        " INNER JOIN POSTransactionDetail PTD ON PT.TransID = PTD.TransID" +
                        " INNER JOIN Customer c ON C.CustomerID = PT.CustomerID" +
                        " INNER JOIN Item I ON I.ItemID = PTD.ItemID" +
                        " INNER JOIN cteTemp IV ON iv.ItemDetailID1='1' and iv.itemid=i.ItemID" + //PRIMEPOS-3274
                        //" INNER JOIN ItemVendor IV ON IV.ItemID = I.ItemID" + //PRIMEPOS-3274
                        " INNER JOIN Vendor V ON V.VendorID = IV.VendorID" +
                        " LEFT JOIN(SELECT IRD.ItemID, Max(IR.RecieveDate) AS InvRecDate FROM InvRecievedDetail IRD INNER JOIN InventoryRecieved IR ON IRD.InvRecievedID = IR.InvRecievedID GROUP BY IRD.ItemID) IRD ON IRD.ItemID = I.ItemID" +
                        " LEFT JOIN Department D ON D.DeptID = I.DepartmentID" +
                        " LEFT JOIN SubDepartment SD ON SD.SubDepartmentID = I.SUBDEPARTMENTID" +
                        " WHERE I.ExclFromAutoPO = 0 " + strWhereClause +
                        " GROUP BY I.ItemID, IV.VendorCostPrice, I.LastCostPrice, V.VendorID, I.Description, V.VendorCode, IV.VendorItemID, I.QtyInStock, I.ReOrderLevel, I.PCKSIZE, I.PCKQTY, I.PCKUNIT, I.MinOrdQty, D.DeptName, SD.Description, I.SellingPrice, IRD.InvRecDate, I.DepartmentID, I.isDiscountable, I.DiscountPolicy, I.Discount";

            return strSQL;
        }

        private string GenerateSqlForIgnoreVendor(string strWhereClause, string strQty, string strPrice)
        {
            string strSQL = "SELECT ROW_NUMBER() OVER(ORDER BY Description ASC) AS PODetailID, x.ItemID, Price, y.Qty, VendorID, Description, VendorName, VendorItemCode, QtyInStock, ReOrderLevel, [Packet Size], [Packet Quantity], [Packet Unit], SoldItems, MinOrdQty, DeptName, SubDeptName, RetailPrice, ItemPrice, Discount, InvRecDate FROM (" +
                        " SELECT ROW_NUMBER() OVER(PARTITION BY I.ItemID ORDER BY PTD.TransDetailID DESC) AS rNum, I.ItemID, " + strPrice + " AS Price," +
                        " V.VendorID, ISNULL(I.Description, '') AS Description, V.VendorCode AS VendorName, IV.VendorItemID AS VendorItemCode," +
                        " ISNULL(I.QtyInStock, 0) AS QtyInStock, ISNULL(I.ReOrderLevel, 0) AS ReOrderLevel, I.PCKSIZE AS[Packet Size], I.PCKQTY AS[Packet Quantity], I.PCKUNIT AS[Packet Unit]," +
                        " 0 AS SoldItems, ISNULL(I.MinOrdQty, 0) AS MinOrdQty, ISNULL(D.DeptName, '') AS DeptName, ISNULL(SD.Description, '') AS SubDeptName," +
                           " ISNULL(I.SellingPrice, 0.00) AS RetailPrice, 0.00 AS ItemPrice," +
                           " CASE WHEN ISNULL(I.isDiscountable,0) = 1 THEN" +
                               " (CASE WHEN ISNULL(I.DiscountPolicy, '') = 'I' THEN ISNULL(I.SellingPrice, 0) * ISNULL(I.Discount, 0) / 100" +
                                   " WHEN ISNULL(I.DiscountPolicy, '') = 'D' THEN ISNULL(I.SellingPrice, 0) * (SELECT Discount FROM Department WHERE DeptID = I.DepartmentID) / 100" +
                                " ELSE 0 END)" +
                        " ELSE 0 END AS Discount," +
                        " IRD.InvRecDate FROM POSTransaction PT" +
                        " INNER JOIN POSTransactionDetail PTD ON PT.TransID = PTD.TransID" +
                        " INNER JOIN Customer c ON C.CustomerID = PT.CustomerID" +
                        " INNER JOIN Item I ON I.ItemID = PTD.ItemID" +
                        " LEFT JOIN ItemVendor IV ON IV.ItemID = I.ItemID" +
                        " LEFT JOIN Vendor V ON V.VendorID = IV.VendorID" +
                        " LEFT JOIN(SELECT IRD.ItemID, Max(IR.RecieveDate) AS InvRecDate FROM InvRecievedDetail IRD INNER JOIN InventoryRecieved IR ON IRD.InvRecievedID = IR.InvRecievedID GROUP BY IRD.ItemID) IRD ON IRD.ItemID = I.ItemID" +
                        " LEFT JOIN Department D ON D.DeptID = I.DepartmentID" +
                        " LEFT JOIN SubDepartment SD ON SD.SubDepartmentID = I.SUBDEPARTMENTID" +
                        " WHERE I.ExclFromAutoPO = 0 " + strWhereClause +
                        " ) X " +
                        "INNER JOIN (SELECT PTD.ItemID, sum(PTD.Qty) AS Qty from POSTransaction PT" +
                                    " INNER JOIN POSTransactionDetail PTD on PTD.TransID = PT.TransID" +
                                    " INNER JOIN Item I ON I.ItemID = PTD.ItemID" +
                                    " WHERE I.ExclFromAutoPO = 0 " + strWhereClause +
                                    " GROUP BY PTD.ItemID) y ON x.ItemID = y.ItemID" +
                        " WHERE rNum = 1" +
                        " ORDER BY Description";

            return strSQL;
        }

        private string GenerateSqlForSpecifiedVendorSequence(string strWhereClause, string strQty, string strPrice, int seq)
        {
            string strSQL = "SELECT " + seq + " AS Seq, I.ItemID, " + strPrice + " AS Price," +
                        strQty +
                        ", V.VendorID, ISNULL(I.Description, '') AS Description, V.VendorCode AS VendorName, IV.VendorItemID AS VendorItemCode, ISNULL(I.QtyInStock, 0) AS QtyInStock," +
                        " ISNULL(I.ReOrderLevel, 0) AS ReOrderLevel, I.PCKSIZE AS[Packet Size], I.PCKQTY AS[Packet Quantity], I.PCKUNIT AS[Packet Unit], 0 AS SoldItems, ISNULL(I.MinOrdQty, 0) AS MinOrdQty," +
                        " ISNULL(D.DeptName, '') AS DeptName, ISNULL(SD.Description, '') AS SubDeptName, ISNULL(I.SellingPrice, 0.00) AS RetailPrice, 0.00 AS ItemPrice," +
                        " CASE WHEN ISNULL(I.isDiscountable,0) = 1 THEN(CASE WHEN ISNULL(I.DiscountPolicy, '') = 'I' THEN ISNULL(I.SellingPrice, 0) * ISNULL(I.Discount, 0) / 100 WHEN ISNULL(I.DiscountPolicy, '') = 'D' THEN ISNULL(I.SellingPrice, 0) * (SELECT Discount FROM Department WHERE DeptID = I.DepartmentID) / 100 ELSE 0 END) ELSE 0 END AS Discount, IRD.InvRecDate" +
                        " FROM POSTransaction PT INNER JOIN POSTransactionDetail PTD ON PT.TransID = PTD.TransID INNER JOIN Customer c ON C.CustomerID = PT.CustomerID" +
                        " INNER JOIN Item I ON I.ItemID = PTD.ItemID INNER JOIN ItemVendor IV ON IV.ItemID = I.ItemID INNER JOIN Vendor V ON V.VendorID = IV.VendorID" +
                        " LEFT JOIN(SELECT IRD.ItemID, Max(IR.RecieveDate) AS InvRecDate FROM InvRecievedDetail IRD INNER JOIN InventoryRecieved IR ON IRD.InvRecievedID = IR.InvRecievedID GROUP BY IRD.ItemID) IRD ON IRD.ItemID = I.ItemID" +
                        " LEFT JOIN Department D ON D.DeptID = I.DepartmentID LEFT JOIN SubDepartment SD ON SD.SubDepartmentID = I.SUBDEPARTMENTID" +
                        " WHERE I.ExclFromAutoPO = 0 " + strWhereClause +
                        " GROUP BY I.ItemID, IV.VendorCostPrice, I.LastCostPrice, V.VendorID, I.Description, V.VendorCode, IV.VendorItemID, I.QtyInStock, I.ReOrderLevel, I.PCKSIZE, I.PCKQTY, I.PCKUNIT, I.MinOrdQty, D.DeptName, SD.Description, I.SellingPrice, IRD.InvRecDate, I.DepartmentID, I.isDiscountable, I.DiscountPolicy, I.Discount";
            return strSQL;
        }

        private string GenerateWhereClause(string VendorType = "")
        {
            string SelectClause = string.Empty;
            if (optByName.CheckedIndex == 0)    //Item Sold selected
            {
                if (optSearchSet.CheckedIndex == 0) //Enter No of Days to go back to look up  item sold
                    SelectClause = " AND PT.TransDate >= DateAdd(D,-" + numNumberOfDays.Value + ",getdate()) ";
                else //Select Date/Time to go back and lookup item sold
                    SelectClause = " AND PT.TransDate BETWEEN cast('" + DateTime.Parse(dtpFromDate.Value.ToString()).ToString() + "' as datetime) AND cast('" + DateTime.Parse(dtpToDate.Value.ToString()).ToString() + "' as datetime) ";
            }
            else if (optByName.CheckedIndex == 1 || optByName.CheckedIndex == 2)    //Re-Order Level or Zero/ Negative Quantity Items selected
            {
                SelectClause = " AND PT.TransDate >= DateAdd(D,-" + this.numNoOfDaysForItemReOrder.Value.ToString() + ",getdate()) ";
            }

            if (Configuration.CPrimeEDISetting.ConsiderReturnTrans) //PRIMEPOS-3167 07-Nov-2022 JY Modified 
                SelectClause += " AND PT.TransType IN (1,2) ";
            else
                SelectClause += " AND PT.TransType = 1 ";

            if (chkForcedVendor.Checked)
            {
                if (txtVendorCode.Text.Trim() != "")
                {
                    if (txtVendorCode.Tag.ToString().Trim() == "")
                        txtVendorCode.Tag = CheckIfExactVendorExists(txtVendorCode.Text.Trim());

                    SelectClause += " AND V.VendorID = " + txtVendorCode.Tag.ToString();
                }

                if (optByName.CheckedIndex == 1)    //Re-Order Level
                {
                    SelectClause += " AND ISNULL(I.QtyInStock,0) <= ISNULL(I.ReOrderLevel,0) AND ISNULL(I.ReOrderLevel,0) > 0 AND ISNULL(I.QtyInStock,0) < ISNULL(I.MinOrdQty,0) ";
                }
                else if (optByName.CheckedIndex == 2)   //Zero/ Negative Quantity Items
                {
                    SelectClause += " AND ISNULL(I.QtyInStock,0) <= 0 ";
                }
            }
            else
            {
                if (lstSequence.Items.Count == 1 && lstSequence.Items.Contains(clsPOSDBConstants.PREFEREDVENDOR)) //Only Prefered Vendor
                {
                    SelectClause += " AND LTRIM(RTRIM(I.PREFERREDVENDOR)) = LTRIM(RTRIM(V.VendorCode)) ";

                    if (optByName.CheckedIndex == 1)    //Re-Order Level
                    {
                        SelectClause += " AND ISNULL(I.QtyInStock,0) <= ISNULL(I.ReOrderLevel,0) AND ISNULL(I.ReOrderLevel,0) > 0 AND ISNULL(I.QtyInStock,0) < ISNULL(I.MinOrdQty,0) ";
                    }
                    else if (optByName.CheckedIndex == 2)   //Zero/ Negative Quantity Items
                    {
                        SelectClause += " AND ISNULL(I.QtyInStock,0) <= 0 ";
                    }
                }
                else if (lstSequence.Items.Count == 1 && lstSequence.Items.Contains(clsPOSDBConstants.LASTVENDOR)) //Only Last Vendor
                {
                    SelectClause += " AND LTRIM(RTRIM(I.LastVendor)) = LTRIM(RTRIM(V.VendorCode)) ";

                    if (optByName.CheckedIndex == 1)    //Re-Order Level
                    {
                        SelectClause += " AND ISNULL(I.QtyInStock,0) <= ISNULL(I.ReOrderLevel,0) AND ISNULL(I.ReOrderLevel,0) > 0 AND ISNULL(I.QtyInStock,0) < ISNULL(I.MinOrdQty,0) ";
                    }
                    else if (optByName.CheckedIndex == 2)   //Zero/ Negative Quantity Items
                    {
                        SelectClause += " AND ISNULL(I.QtyInStock,0) <= 0 ";
                    }
                }
                else if (lstSequence.Items.Count == 1 && lstSequence.Items.Contains(clsPOSDBConstants.DEFAULTVENDOR)) //Only Default Vendor
                {
                    if (Configuration.CPrimeEDISetting.DefaultVendor.Trim() != "")  //PRIMEPOS-3167 07-Nov-2022 JY Modified
                    {
                        string VendorID = CheckIfExactVendorExists(Configuration.CPrimeEDISetting.DefaultVendor);   //PRIMEPOS-3167 07-Nov-2022 JY Modified
                        SelectClause += " AND V.VendorID = " + VendorID;
                    }

                    if (optByName.CheckedIndex == 1)    //Re-Order Level
                    {
                        SelectClause += " AND ISNULL(I.QtyInStock,0) <= ISNULL(I.ReOrderLevel,0) AND ISNULL(I.ReOrderLevel,0) > 0 AND ISNULL(I.QtyInStock,0) < ISNULL(I.MinOrdQty,0) ";
                    }
                    else if (optByName.CheckedIndex == 2)   //Zero/ Negative Quantity Items
                    {
                        SelectClause += " AND ISNULL(I.QtyInStock,0) <= 0 ";
                    }
                }
                else if (lstSequence.Items.Count == 1 && lstSequence.Items.Contains(clsPOSDBConstants.IGNOREVENDOR)) //Only Ignore Vendor
                {
                    if (optByName.CheckedIndex == 1)    //Re-Order Level
                    {
                        SelectClause += " AND ISNULL(I.QtyInStock,0) <= ISNULL(I.ReOrderLevel,0) AND ISNULL(I.ReOrderLevel,0) > 0 AND ISNULL(I.QtyInStock,0) < ISNULL(I.MinOrdQty,0) ";
                    }
                    else if (optByName.CheckedIndex == 2)   //Zero/ Negative Quantity Items
                    {
                        SelectClause += " AND ISNULL(I.QtyInStock,0) <= 0 ";
                    }
                }
                else if (lstSequence.Items.Count > 1)   //sequence - multiple vendor selection
                {
                    if (VendorType.Trim().ToUpper() == clsPOSDBConstants.DEFAULTVENDOR.ToUpper())
                    {
                        if (Configuration.CPrimeEDISetting.DefaultVendor.Trim() != "")  //PRIMEPOS-3167 07-Nov-2022 JY Modified
                        {
                            string VendorID = CheckIfExactVendorExists(Configuration.CPrimeEDISetting.DefaultVendor);   //PRIMEPOS-3167 07-Nov-2022 JY Modified
                            SelectClause += " AND V.VendorID = " + VendorID;
                        }

                        if (optByName.CheckedIndex == 1)    //Re-Order Level
                        {
                            SelectClause += " AND ISNULL(I.QtyInStock,0) <= ISNULL(I.ReOrderLevel,0) AND ISNULL(I.ReOrderLevel,0) > 0 AND ISNULL(I.QtyInStock,0) < ISNULL(I.MinOrdQty,0) ";
                        }
                        else if (optByName.CheckedIndex == 2)   //Zero/ Negative Quantity Items
                        {
                            SelectClause += " AND ISNULL(I.QtyInStock,0) <= 0 ";
                        }
                    }
                    else if (VendorType.Trim().ToUpper() == clsPOSDBConstants.LASTVENDOR.ToUpper())
                    {
                        SelectClause += " AND LTRIM(RTRIM(I.LastVendor)) = LTRIM(RTRIM(V.VendorCode)) ";

                        if (optByName.CheckedIndex == 1)    //Re-Order Level
                        {
                            SelectClause += " AND ISNULL(I.QtyInStock,0) <= ISNULL(I.ReOrderLevel,0) AND ISNULL(I.ReOrderLevel,0) > 0 AND ISNULL(I.QtyInStock,0) < ISNULL(I.MinOrdQty,0) ";
                        }
                        else if (optByName.CheckedIndex == 2)   //Zero/ Negative Quantity Items
                        {
                            SelectClause += " AND ISNULL(I.QtyInStock,0) <= 0 ";
                        }
                    }
                    else if (VendorType.Trim().ToUpper() == clsPOSDBConstants.PREFEREDVENDOR.ToUpper())
                    {
                        SelectClause += " AND LTRIM(RTRIM(I.PREFERREDVENDOR)) = LTRIM(RTRIM(V.VendorCode)) ";

                        if (optByName.CheckedIndex == 1)    //Re-Order Level
                        {
                            SelectClause += " AND ISNULL(I.QtyInStock,0) <= ISNULL(I.ReOrderLevel,0) AND ISNULL(I.ReOrderLevel,0) > 0 AND ISNULL(I.QtyInStock,0) < ISNULL(I.MinOrdQty,0) ";
                        }
                        else if (optByName.CheckedIndex == 2)   //Zero/ Negative Quantity Items
                        {
                            SelectClause += " AND ISNULL(I.QtyInStock,0) <= 0 ";
                        }
                    }
                }
            }

            if (this.txtDepartment.Text.Trim() != "" && txtDepartment.Tag.ToString() != "")
            {
                SelectClause += " AND I.DepartmentID IN (" + txtDepartment.Tag.ToString() + ")";
            }

            if (this.txtSubDepartment.Text.Trim() != "" && txtSubDepartment.Tag.ToString() != "")
            {
                SelectClause += " AND I.SUBDEPARTMENTID IN (" + txtSubDepartment.Tag.ToString() + ")";
            }

            if (this.txtItemLocation.Text.Trim() != "" && txtItemLocation.Tag.ToString() != "")
            {
                SelectClause += " and I.Location IN (" + txtItemLocation.Text + ")";
            }

            return SelectClause;
        }
        #endregion
    }
}