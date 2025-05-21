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
using System.Linq;
using Infragistics.Win.UltraWinGrid;
using System.Collections.Generic;
using CrystalDecisions.CrystalReports.Engine;

namespace POS_Core_UI.Reports.ReportsUI
{
    /// <summary>
    /// Summary description for frmViewTransaction.
    /// </summary>
    public class frmRptSSalesByItem : System.Windows.Forms.Form, ICommandLIneTaskControl
    {
        private System.Windows.Forms.GroupBox groupBox1;
        private Infragistics.Win.Misc.UltraLabel ultraLabel12;
        private Infragistics.Win.Misc.UltraLabel ultraLabel20;
        private Infragistics.Win.Misc.UltraLabel ultraLabel19;
        private Infragistics.Win.UltraWinSchedule.UltraCalendarCombo dtpSaleEndDate;
        private Infragistics.Win.UltraWinSchedule.UltraCalendarCombo dtpSaleStartDate;
        private System.Windows.Forms.GroupBox groupBox2;
        private Infragistics.Win.Misc.UltraButton btnPrint;
        private Infragistics.Win.Misc.UltraButton btnView;
        private Infragistics.Win.Misc.UltraButton btnClose;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor txtItemID;
        private Infragistics.Win.Misc.UltraLabel ultraLabel1;
        private UltraTextEditor txtVendorCode;
        private UltraCheckEditor chkIgnoreRx;
        private Infragistics.Win.Misc.UltraLabel ultraLabel2;
        private UltraTextEditor txtUserID;
        private Infragistics.Win.Misc.UltraButton btnSearch;
        private Infragistics.Win.UltraWinGrid.UltraGrid grdSearch;
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;

        #region Sprint-19 - 2156 28-Jan-2015 JY Added
        private DataSet dsSearch;   
        private SearchSvr oSearchSvr = new SearchSvr(); 
        private int CurrentX;
        private UltraCheckEditor chkOnlyDiscountedTrans;
        private RadioButton optDetail;
        private RadioButton optSummary;
        private Infragistics.Win.Misc.UltraLabel lblSubDepartment;
        private Infragistics.Win.Misc.UltraLabel ultraLabel3;
        private UltraTextEditor txtDepartment;
        private UltraTextEditor txtSubDepartment;
        private int CurrentY; 
        #endregion

        public frmRptSSalesByItem()
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
            Infragistics.Win.UltraWinEditors.EditorButton editorButton1 = new Infragistics.Win.UltraWinEditors.EditorButton();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinEditors.EditorButton editorButton2 = new Infragistics.Win.UltraWinEditors.EditorButton();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance7 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance8 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinEditors.EditorButton editorButton3 = new Infragistics.Win.UltraWinEditors.EditorButton();
            Infragistics.Win.Appearance appearance9 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance10 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance11 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinSchedule.CalendarCombo.DateButton dateButton1 = new Infragistics.Win.UltraWinSchedule.CalendarCombo.DateButton();
            Infragistics.Win.UltraWinSchedule.CalendarCombo.DateButton dateButton2 = new Infragistics.Win.UltraWinSchedule.CalendarCombo.DateButton();
            Infragistics.Win.Appearance appearance12 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance13 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinEditors.EditorButton editorButton4 = new Infragistics.Win.UltraWinEditors.EditorButton();
            Infragistics.Win.Appearance appearance14 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance15 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance16 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance17 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance18 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand1 = new Infragistics.Win.UltraWinGrid.UltraGridBand("Band 0", -1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn4 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Column Name");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn5 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Type");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn6 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Criteria Value");
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
            Infragistics.Win.Appearance appearance32 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance33 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance34 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance35 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance36 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance37 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinScrollBar.ScrollBarLook scrollBarLook1 = new Infragistics.Win.UltraWinScrollBar.ScrollBarLook();
            Infragistics.Win.Appearance appearance38 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance39 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance40 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance41 = new Infragistics.Win.Appearance();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lblSubDepartment = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabel3 = new Infragistics.Win.Misc.UltraLabel();
            this.txtDepartment = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.txtSubDepartment = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.optDetail = new System.Windows.Forms.RadioButton();
            this.optSummary = new System.Windows.Forms.RadioButton();
            this.chkOnlyDiscountedTrans = new Infragistics.Win.UltraWinEditors.UltraCheckEditor();
            this.btnSearch = new Infragistics.Win.Misc.UltraButton();
            this.ultraLabel2 = new Infragistics.Win.Misc.UltraLabel();
            this.txtUserID = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.chkIgnoreRx = new Infragistics.Win.UltraWinEditors.UltraCheckEditor();
            this.txtItemID = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.ultraLabel20 = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabel19 = new Infragistics.Win.Misc.UltraLabel();
            this.dtpSaleEndDate = new Infragistics.Win.UltraWinSchedule.UltraCalendarCombo();
            this.dtpSaleStartDate = new Infragistics.Win.UltraWinSchedule.UltraCalendarCombo();
            this.ultraLabel12 = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabel1 = new Infragistics.Win.Misc.UltraLabel();
            this.txtVendorCode = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnPrint = new Infragistics.Win.Misc.UltraButton();
            this.btnClose = new Infragistics.Win.Misc.UltraButton();
            this.btnView = new Infragistics.Win.Misc.UltraButton();
            this.grdSearch = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtDepartment)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSubDepartment)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkOnlyDiscountedTrans)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtUserID)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkIgnoreRx)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtItemID)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtpSaleEndDate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtpSaleStartDate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtVendorCode)).BeginInit();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdSearch)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.lblSubDepartment);
            this.groupBox1.Controls.Add(this.ultraLabel3);
            this.groupBox1.Controls.Add(this.txtDepartment);
            this.groupBox1.Controls.Add(this.txtSubDepartment);
            this.groupBox1.Controls.Add(this.optDetail);
            this.groupBox1.Controls.Add(this.optSummary);
            this.groupBox1.Controls.Add(this.chkOnlyDiscountedTrans);
            this.groupBox1.Controls.Add(this.btnSearch);
            this.groupBox1.Controls.Add(this.ultraLabel2);
            this.groupBox1.Controls.Add(this.txtUserID);
            this.groupBox1.Controls.Add(this.chkIgnoreRx);
            this.groupBox1.Controls.Add(this.txtItemID);
            this.groupBox1.Controls.Add(this.ultraLabel20);
            this.groupBox1.Controls.Add(this.ultraLabel19);
            this.groupBox1.Controls.Add(this.dtpSaleEndDate);
            this.groupBox1.Controls.Add(this.dtpSaleStartDate);
            this.groupBox1.Controls.Add(this.ultraLabel12);
            this.groupBox1.Controls.Add(this.ultraLabel1);
            this.groupBox1.Controls.Add(this.txtVendorCode);
            this.groupBox1.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.groupBox1.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(5, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(974, 92);
            this.groupBox1.TabIndex = 27;
            this.groupBox1.TabStop = false;
            // 
            // lblSubDepartment
            // 
            appearance1.ForeColor = System.Drawing.Color.White;
            this.lblSubDepartment.Appearance = appearance1;
            this.lblSubDepartment.AutoSize = true;
            this.lblSubDepartment.Location = new System.Drawing.Point(561, 40);
            this.lblSubDepartment.Name = "lblSubDepartment";
            this.lblSubDepartment.Size = new System.Drawing.Size(256, 18);
            this.lblSubDepartment.TabIndex = 33;
            this.lblSubDepartment.Text = "Sub Dept<Blank=Ignore Sub Dept>";
            // 
            // ultraLabel3
            // 
            appearance2.ForeColor = System.Drawing.Color.White;
            this.ultraLabel3.Appearance = appearance2;
            this.ultraLabel3.AutoSize = true;
            this.ultraLabel3.Location = new System.Drawing.Point(561, 16);
            this.ultraLabel3.Name = "ultraLabel3";
            this.ultraLabel3.Size = new System.Drawing.Size(223, 18);
            this.ultraLabel3.TabIndex = 32;
            this.ultraLabel3.Text = "Department<Blank=Any Dept>";
            // 
            // txtDepartment
            // 
            this.txtDepartment.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance3.BackColor = System.Drawing.Color.White;
            appearance3.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(231)))), ((int)(((byte)(230)))));
            appearance3.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance3.BorderAlpha = Infragistics.Win.Alpha.Transparent;
            appearance3.ImageAlpha = Infragistics.Win.Alpha.Opaque;
            appearance3.ImageBackgroundAlpha = Infragistics.Win.Alpha.Opaque;
            appearance3.ImageBackgroundStyle = Infragistics.Win.ImageBackgroundStyle.Centered;
            editorButton1.Appearance = appearance3;
            editorButton1.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            editorButton1.Text = "";
            editorButton1.Width = 20;
            this.txtDepartment.ButtonsRight.Add(editorButton1);
            this.txtDepartment.Location = new System.Drawing.Point(821, 14);
            this.txtDepartment.Name = "txtDepartment";
            this.txtDepartment.ReadOnly = true;
            this.txtDepartment.Size = new System.Drawing.Size(123, 23);
            this.txtDepartment.TabIndex = 4;
            this.txtDepartment.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.txtDepartment.EditorButtonClick += new Infragistics.Win.UltraWinEditors.EditorButtonEventHandler(this.txtDepartment_EditorButtonClick);
            // 
            // txtSubDepartment
            // 
            this.txtSubDepartment.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance4.BackColor = System.Drawing.Color.White;
            appearance4.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(231)))), ((int)(((byte)(230)))));
            appearance4.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance4.BorderAlpha = Infragistics.Win.Alpha.Transparent;
            appearance4.ImageAlpha = Infragistics.Win.Alpha.Opaque;
            appearance4.ImageBackgroundAlpha = Infragistics.Win.Alpha.Opaque;
            appearance4.ImageBackgroundStyle = Infragistics.Win.ImageBackgroundStyle.Centered;
            editorButton2.Appearance = appearance4;
            editorButton2.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            editorButton2.Text = "";
            editorButton2.Width = 20;
            this.txtSubDepartment.ButtonsRight.Add(editorButton2);
            this.txtSubDepartment.Location = new System.Drawing.Point(821, 38);
            this.txtSubDepartment.Name = "txtSubDepartment";
            this.txtSubDepartment.ReadOnly = true;
            this.txtSubDepartment.Size = new System.Drawing.Size(123, 23);
            this.txtSubDepartment.TabIndex = 5;
            this.txtSubDepartment.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.txtSubDepartment.EditorButtonClick += new Infragistics.Win.UltraWinEditors.EditorButtonEventHandler(this.txtSubDepartment_EditorButtonClick);
            // 
            // optDetail
            // 
            this.optDetail.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.optDetail.ForeColor = System.Drawing.Color.White;
            this.optDetail.Location = new System.Drawing.Point(660, 63);
            this.optDetail.Name = "optDetail";
            this.optDetail.Size = new System.Drawing.Size(64, 23);
            this.optDetail.TabIndex = 9;
            this.optDetail.Text = "Detail";
            // 
            // optSummary
            // 
            this.optSummary.Checked = true;
            this.optSummary.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.optSummary.ForeColor = System.Drawing.Color.White;
            this.optSummary.Location = new System.Drawing.Point(561, 63);
            this.optSummary.Name = "optSummary";
            this.optSummary.Size = new System.Drawing.Size(93, 23);
            this.optSummary.TabIndex = 8;
            this.optSummary.TabStop = true;
            this.optSummary.Text = "Summary";
            // 
            // chkOnlyDiscountedTrans
            // 
            appearance5.ForeColor = System.Drawing.Color.White;
            this.chkOnlyDiscountedTrans.Appearance = appearance5;
            this.chkOnlyDiscountedTrans.AutoSize = true;
            this.chkOnlyDiscountedTrans.Location = new System.Drawing.Point(9, 65);
            this.chkOnlyDiscountedTrans.Name = "chkOnlyDiscountedTrans";
            this.chkOnlyDiscountedTrans.Size = new System.Drawing.Size(178, 21);
            this.chkOnlyDiscountedTrans.TabIndex = 6;
            this.chkOnlyDiscountedTrans.Text = "Only Discounted Trans";
            // 
            // btnSearch
            // 
            appearance6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(62)))), ((int)(((byte)(176)))));
            appearance6.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance6.FontData.BoldAsString = "True";
            appearance6.ForeColor = System.Drawing.Color.White;
            this.btnSearch.Appearance = appearance6;
            this.btnSearch.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            this.btnSearch.Location = new System.Drawing.Point(827, 65);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(117, 23);
            this.btnSearch.TabIndex = 10;
            this.btnSearch.Text = "&Search (F4)";
            this.btnSearch.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // ultraLabel2
            // 
            appearance7.ForeColor = System.Drawing.Color.White;
            this.ultraLabel2.Appearance = appearance7;
            this.ultraLabel2.AutoSize = true;
            this.ultraLabel2.Location = new System.Drawing.Point(221, 40);
            this.ultraLabel2.Name = "ultraLabel2";
            this.ultraLabel2.Size = new System.Drawing.Size(206, 18);
            this.ultraLabel2.TabIndex = 27;
            this.ultraLabel2.Text = "User ID <Blank = Any User>";
            // 
            // txtUserID
            // 
            this.txtUserID.AutoSize = false;
            this.txtUserID.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.txtUserID.Location = new System.Drawing.Point(431, 38);
            this.txtUserID.MaxLength = 20;
            this.txtUserID.Name = "txtUserID";
            this.txtUserID.Size = new System.Drawing.Size(123, 23);
            this.txtUserID.TabIndex = 3;
            this.txtUserID.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // chkIgnoreRx
            // 
            appearance8.ForeColor = System.Drawing.Color.White;
            this.chkIgnoreRx.Appearance = appearance8;
            this.chkIgnoreRx.AutoSize = true;
            this.chkIgnoreRx.Checked = true;
            this.chkIgnoreRx.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkIgnoreRx.Location = new System.Drawing.Point(193, 65);
            this.chkIgnoreRx.Name = "chkIgnoreRx";
            this.chkIgnoreRx.Size = new System.Drawing.Size(136, 21);
            this.chkIgnoreRx.TabIndex = 7;
            this.chkIgnoreRx.Text = "Ignore RX Items";
            // 
            // txtItemID
            // 
            this.txtItemID.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance9.BackColor = System.Drawing.Color.White;
            appearance9.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(231)))), ((int)(((byte)(230)))));
            appearance9.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance9.BorderAlpha = Infragistics.Win.Alpha.Transparent;
            appearance9.ImageAlpha = Infragistics.Win.Alpha.Opaque;
            appearance9.ImageBackgroundAlpha = Infragistics.Win.Alpha.Opaque;
            appearance9.ImageBackgroundStyle = Infragistics.Win.ImageBackgroundStyle.Centered;
            editorButton3.Appearance = appearance9;
            editorButton3.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            editorButton3.Text = "";
            editorButton3.Width = 20;
            this.txtItemID.ButtonsRight.Add(editorButton3);
            this.txtItemID.Location = new System.Drawing.Point(431, 14);
            this.txtItemID.Name = "txtItemID";
            this.txtItemID.Size = new System.Drawing.Size(123, 23);
            this.txtItemID.TabIndex = 2;
            this.txtItemID.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.txtItemID.EditorButtonClick += new Infragistics.Win.UltraWinEditors.EditorButtonEventHandler(this.txtItemID_EditorButtonClick);
            // 
            // ultraLabel20
            // 
            appearance10.ForeColor = System.Drawing.Color.White;
            this.ultraLabel20.Appearance = appearance10;
            this.ultraLabel20.AutoSize = true;
            this.ultraLabel20.Location = new System.Drawing.Point(9, 16);
            this.ultraLabel20.Name = "ultraLabel20";
            this.ultraLabel20.Size = new System.Drawing.Size(77, 18);
            this.ultraLabel20.TabIndex = 20;
            this.ultraLabel20.Text = "Start Date";
            // 
            // ultraLabel19
            // 
            appearance11.ForeColor = System.Drawing.Color.White;
            this.ultraLabel19.Appearance = appearance11;
            this.ultraLabel19.AutoSize = true;
            this.ultraLabel19.Location = new System.Drawing.Point(9, 40);
            this.ultraLabel19.Name = "ultraLabel19";
            this.ultraLabel19.Size = new System.Drawing.Size(68, 18);
            this.ultraLabel19.TabIndex = 22;
            this.ultraLabel19.Text = "End Date";
            // 
            // dtpSaleEndDate
            // 
            this.dtpSaleEndDate.AllowNull = false;
            this.dtpSaleEndDate.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Office2003ToolbarButton;
            this.dtpSaleEndDate.DateButtons.Add(dateButton1);
            this.dtpSaleEndDate.Location = new System.Drawing.Point(92, 38);
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
            this.dtpSaleStartDate.Location = new System.Drawing.Point(92, 14);
            this.dtpSaleStartDate.Name = "dtpSaleStartDate";
            this.dtpSaleStartDate.NonAutoSizeHeight = 10;
            this.dtpSaleStartDate.Size = new System.Drawing.Size(123, 22);
            this.dtpSaleStartDate.TabIndex = 0;
            this.dtpSaleStartDate.Tag = "From Date";
            this.dtpSaleStartDate.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.dtpSaleStartDate.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.dtpSaleStartDate.Value = new System.DateTime(2004, 5, 25, 0, 0, 0, 0);
            // 
            // ultraLabel12
            // 
            appearance12.ForeColor = System.Drawing.Color.White;
            this.ultraLabel12.Appearance = appearance12;
            this.ultraLabel12.AutoSize = true;
            this.ultraLabel12.Location = new System.Drawing.Point(221, 16);
            this.ultraLabel12.Name = "ultraLabel12";
            this.ultraLabel12.Size = new System.Drawing.Size(209, 18);
            this.ultraLabel12.TabIndex = 6;
            this.ultraLabel12.Text = "Item ID <Blank = Any Item>";
            // 
            // ultraLabel1
            // 
            appearance13.ForeColor = System.Drawing.Color.White;
            this.ultraLabel1.Appearance = appearance13;
            this.ultraLabel1.AutoSize = true;
            this.ultraLabel1.Location = new System.Drawing.Point(153, 16);
            this.ultraLabel1.Name = "ultraLabel1";
            this.ultraLabel1.Size = new System.Drawing.Size(260, 18);
            this.ultraLabel1.TabIndex = 24;
            this.ultraLabel1.Text = "Vendor Code <Blank = Any Vendor>";
            this.ultraLabel1.Visible = false;
            // 
            // txtVendorCode
            // 
            this.txtVendorCode.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance14.BackColor = System.Drawing.Color.White;
            appearance14.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(231)))), ((int)(((byte)(230)))));
            appearance14.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance14.BorderAlpha = Infragistics.Win.Alpha.Transparent;
            appearance14.ImageAlpha = Infragistics.Win.Alpha.Opaque;
            appearance14.ImageBackgroundAlpha = Infragistics.Win.Alpha.Opaque;
            appearance14.ImageBackgroundStyle = Infragistics.Win.ImageBackgroundStyle.Centered;
            editorButton4.Appearance = appearance14;
            editorButton4.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            editorButton4.Text = "";
            editorButton4.Width = 20;
            this.txtVendorCode.ButtonsRight.Add(editorButton4);
            this.txtVendorCode.Location = new System.Drawing.Point(302, 23);
            this.txtVendorCode.Name = "txtVendorCode";
            this.txtVendorCode.Size = new System.Drawing.Size(123, 23);
            this.txtVendorCode.TabIndex = 23;
            this.txtVendorCode.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.txtVendorCode.Visible = false;
            this.txtVendorCode.EditorButtonClick += new Infragistics.Win.UltraWinEditors.EditorButtonEventHandler(this.txtVendorCode_EditorButtonClick);
            this.txtVendorCode.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtVendorCode_KeyDown);
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
            this.groupBox2.TabIndex = 4;
            this.groupBox2.TabStop = false;
            // 
            // btnPrint
            // 
            this.btnPrint.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            appearance15.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(62)))), ((int)(((byte)(176)))));
            appearance15.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance15.FontData.BoldAsString = "True";
            appearance15.ForeColor = System.Drawing.Color.White;
            this.btnPrint.Appearance = appearance15;
            this.btnPrint.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            this.btnPrint.Location = new System.Drawing.Point(698, 15);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(85, 26);
            this.btnPrint.TabIndex = 0;
            this.btnPrint.Text = "&Print";
            this.btnPrint.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            appearance16.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(62)))), ((int)(((byte)(176)))));
            appearance16.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance16.FontData.BoldAsString = "True";
            appearance16.ForeColor = System.Drawing.Color.White;
            this.btnClose.Appearance = appearance16;
            this.btnClose.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(882, 16);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(85, 26);
            this.btnClose.TabIndex = 2;
            this.btnClose.Text = "&Close";
            this.btnClose.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnView
            // 
            this.btnView.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            appearance17.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(62)))), ((int)(((byte)(176)))));
            appearance17.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance17.FontData.BoldAsString = "True";
            appearance17.ForeColor = System.Drawing.Color.White;
            this.btnView.Appearance = appearance17;
            this.btnView.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            this.btnView.Location = new System.Drawing.Point(790, 16);
            this.btnView.Name = "btnView";
            this.btnView.Size = new System.Drawing.Size(85, 26);
            this.btnView.TabIndex = 1;
            this.btnView.Text = "&View";
            this.btnView.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnView.Click += new System.EventHandler(this.btnView_Click_1);
            // 
            // grdSearch
            // 
            this.grdSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            appearance18.BackColor = System.Drawing.Color.White;
            appearance18.BackColor2 = System.Drawing.Color.White;
            appearance18.BackColorDisabled = System.Drawing.Color.White;
            appearance18.BackColorDisabled2 = System.Drawing.Color.White;
            appearance18.BackGradientStyle = Infragistics.Win.GradientStyle.None;
            this.grdSearch.DisplayLayout.Appearance = appearance18;
            ultraGridColumn4.Header.VisiblePosition = 0;
            ultraGridColumn5.Header.VisiblePosition = 1;
            ultraGridColumn6.Header.VisiblePosition = 2;
            ultraGridBand1.Columns.AddRange(new object[] {
            ultraGridColumn4,
            ultraGridColumn5,
            ultraGridColumn6});
            ultraGridBand1.HeaderVisible = true;
            this.grdSearch.DisplayLayout.BandsSerializer.Add(ultraGridBand1);
            this.grdSearch.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.grdSearch.DisplayLayout.InterBandSpacing = 10;
            this.grdSearch.DisplayLayout.MaxColScrollRegions = 1;
            this.grdSearch.DisplayLayout.MaxRowScrollRegions = 1;
            appearance19.BackColor = System.Drawing.Color.White;
            appearance19.BackColor2 = System.Drawing.Color.White;
            this.grdSearch.DisplayLayout.Override.ActiveCardCaptionAppearance = appearance19;
            appearance20.BackColor = System.Drawing.Color.White;
            appearance20.BackColor2 = System.Drawing.Color.White;
            appearance20.BorderColor = System.Drawing.Color.Gray;
            this.grdSearch.DisplayLayout.Override.ActiveRowAppearance = appearance20;
            appearance21.BackColor = System.Drawing.Color.White;
            appearance21.BackColor2 = System.Drawing.Color.White;
            appearance21.BorderColor = System.Drawing.Color.Gray;
            this.grdSearch.DisplayLayout.Override.AddRowAppearance = appearance21;
            this.grdSearch.DisplayLayout.Override.AllowAddNew = Infragistics.Win.UltraWinGrid.AllowAddNew.No;
            this.grdSearch.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.False;
            this.grdSearch.DisplayLayout.Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.False;
            appearance22.BackColor = System.Drawing.Color.Transparent;
            this.grdSearch.DisplayLayout.Override.CardAreaAppearance = appearance22;
            appearance23.BackColor = System.Drawing.Color.White;
            appearance23.BackColor2 = System.Drawing.Color.White;
            appearance23.BackColorDisabled = System.Drawing.Color.White;
            appearance23.BackColorDisabled2 = System.Drawing.Color.White;
            appearance23.BorderColor = System.Drawing.Color.Black;
            appearance23.BorderColor3DBase = System.Drawing.Color.Black;
            this.grdSearch.DisplayLayout.Override.CellAppearance = appearance23;
            appearance24.BackColor = System.Drawing.Color.White;
            appearance24.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(231)))), ((int)(((byte)(230)))));
            appearance24.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance24.BorderAlpha = Infragistics.Win.Alpha.Transparent;
            appearance24.BorderColor = System.Drawing.Color.Gray;
            appearance24.BorderColor3DBase = System.Drawing.Color.Gray;
            appearance24.ImageBackgroundAlpha = Infragistics.Win.Alpha.Transparent;
            appearance24.ImageBackgroundStyle = Infragistics.Win.ImageBackgroundStyle.Centered;
            this.grdSearch.DisplayLayout.Override.CellButtonAppearance = appearance24;
            appearance25.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            appearance25.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.grdSearch.DisplayLayout.Override.EditCellAppearance = appearance25;
            appearance26.BorderColor = System.Drawing.Color.Gray;
            this.grdSearch.DisplayLayout.Override.FilteredInRowAppearance = appearance26;
            appearance27.BorderColor = System.Drawing.Color.Gray;
            this.grdSearch.DisplayLayout.Override.FilteredOutRowAppearance = appearance27;
            appearance28.BackColor = System.Drawing.Color.White;
            appearance28.BackColor2 = System.Drawing.Color.White;
            appearance28.BackColorDisabled = System.Drawing.Color.White;
            appearance28.BackColorDisabled2 = System.Drawing.Color.White;
            this.grdSearch.DisplayLayout.Override.FixedCellAppearance = appearance28;
            appearance29.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            appearance29.BackColor2 = System.Drawing.Color.Beige;
            this.grdSearch.DisplayLayout.Override.FixedHeaderAppearance = appearance29;
            appearance30.BackColor = System.Drawing.Color.White;
            appearance30.BackColor2 = System.Drawing.SystemColors.Control;
            appearance30.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance30.FontData.BoldAsString = "True";
            appearance30.ForeColor = System.Drawing.Color.Black;
            appearance30.TextHAlignAsString = "Left";
            appearance30.ThemedElementAlpha = Infragistics.Win.Alpha.Transparent;
            this.grdSearch.DisplayLayout.Override.HeaderAppearance = appearance30;
            appearance31.BorderColor = System.Drawing.Color.Gray;
            this.grdSearch.DisplayLayout.Override.RowAlternateAppearance = appearance31;
            appearance32.BackColor = System.Drawing.Color.White;
            appearance32.BackColor2 = System.Drawing.Color.White;
            appearance32.BackGradientStyle = Infragistics.Win.GradientStyle.None;
            appearance32.BackHatchStyle = Infragistics.Win.BackHatchStyle.None;
            appearance32.BorderColor = System.Drawing.Color.Gray;
            this.grdSearch.DisplayLayout.Override.RowAppearance = appearance32;
            appearance33.BorderColor = System.Drawing.Color.Gray;
            this.grdSearch.DisplayLayout.Override.RowPreviewAppearance = appearance33;
            appearance34.BackColor = System.Drawing.Color.White;
            appearance34.BackColor2 = System.Drawing.SystemColors.Control;
            appearance34.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance34.BorderColor = System.Drawing.Color.Gray;
            this.grdSearch.DisplayLayout.Override.RowSelectorAppearance = appearance34;
            this.grdSearch.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            this.grdSearch.DisplayLayout.Override.RowSelectorWidth = 12;
            this.grdSearch.DisplayLayout.Override.RowSpacingBefore = 2;
            appearance35.BackColor = System.Drawing.Color.Navy;
            appearance35.BackColorAlpha = Infragistics.Win.Alpha.Opaque;
            this.grdSearch.DisplayLayout.Override.SelectedCellAppearance = appearance35;
            appearance36.BackColor = System.Drawing.Color.Navy;
            appearance36.BackColorDisabled = System.Drawing.Color.Navy;
            appearance36.BackGradientStyle = Infragistics.Win.GradientStyle.None;
            appearance36.BackHatchStyle = Infragistics.Win.BackHatchStyle.None;
            appearance36.BorderColor = System.Drawing.Color.Gray;
            appearance36.ForeColor = System.Drawing.Color.Black;
            this.grdSearch.DisplayLayout.Override.SelectedRowAppearance = appearance36;
            this.grdSearch.DisplayLayout.Override.SelectTypeCell = Infragistics.Win.UltraWinGrid.SelectType.None;
            this.grdSearch.DisplayLayout.Override.SelectTypeCol = Infragistics.Win.UltraWinGrid.SelectType.None;
            this.grdSearch.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.Single;
            appearance37.BorderColor = System.Drawing.Color.Gray;
            this.grdSearch.DisplayLayout.Override.TemplateAddRowAppearance = appearance37;
            this.grdSearch.DisplayLayout.RowConnectorColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            this.grdSearch.DisplayLayout.RowConnectorStyle = Infragistics.Win.UltraWinGrid.RowConnectorStyle.Solid;
            appearance38.BackColor = System.Drawing.Color.White;
            appearance38.BackColor2 = System.Drawing.SystemColors.Control;
            appearance38.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            scrollBarLook1.Appearance = appearance38;
            appearance39.BackColor = System.Drawing.Color.White;
            appearance39.BackColor2 = System.Drawing.SystemColors.Control;
            appearance39.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance39.ThemedElementAlpha = Infragistics.Win.Alpha.Transparent;
            scrollBarLook1.ButtonAppearance = appearance39;
            appearance40.BackColor = System.Drawing.Color.White;
            appearance40.BackColor2 = System.Drawing.SystemColors.Control;
            scrollBarLook1.ThumbAppearance = appearance40;
            appearance41.BackColor = System.Drawing.Color.White;
            appearance41.BackColor2 = System.Drawing.Color.White;
            scrollBarLook1.TrackAppearance = appearance41;
            this.grdSearch.DisplayLayout.ScrollBarLook = scrollBarLook1;
            this.grdSearch.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.grdSearch.Location = new System.Drawing.Point(5, 96);
            this.grdSearch.Name = "grdSearch";
            this.grdSearch.Size = new System.Drawing.Size(974, 318);
            this.grdSearch.TabIndex = 0;
            this.grdSearch.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.grdSearch.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.grdSearch_InitializeLayout);
            this.grdSearch.BeforeRowExpanded += new Infragistics.Win.UltraWinGrid.CancelableRowEventHandler(this.grdSearch_BeforeRowExpanded);
            this.grdSearch.DoubleClick += new System.EventHandler(this.grdSearch_DoubleClick);
            this.grdSearch.MouseMove += new System.Windows.Forms.MouseEventHandler(this.grdSearch_MouseMove);
            // 
            // frmRptSSalesByItem
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(108)))), ((int)(((byte)(172)))));
            this.ClientSize = new System.Drawing.Size(984, 462);
            this.Controls.Add(this.grdSearch);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.Name = "frmRptSSalesByItem";
            this.Text = "Sales Report By Item";
            this.Load += new System.EventHandler(this.frmViewTransaction_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmViewTransaction_KeyDown);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtDepartment)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSubDepartment)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkOnlyDiscountedTrans)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtUserID)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkIgnoreRx)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtItemID)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtpSaleEndDate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtpSaleStartDate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtVendorCode)).EndInit();
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdSearch)).EndInit();
            this.ResumeLayout(false);

        }
        #endregion

        private void frmViewTransaction_Load(object sender, System.EventArgs e)
        {
            this.dtpSaleStartDate.Enter += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
            this.dtpSaleStartDate.Leave += new System.EventHandler(clsUIHelper.AfterExitEditMode);

            this.dtpSaleEndDate.Enter += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
            this.dtpSaleEndDate.Leave += new System.EventHandler(clsUIHelper.AfterExitEditMode);

            this.txtItemID.AfterEnterEditMode += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
            this.txtItemID.AfterExitEditMode += new System.EventHandler(clsUIHelper.AfterExitEditMode);
            //Start: Added By Amit Date 4 May 2011
            this.txtVendorCode.AfterEnterEditMode += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
            this.txtVendorCode.AfterExitEditMode += new System.EventHandler(clsUIHelper.AfterExitEditMode);
            //End
            this.txtUserID.AfterEnterEditMode += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
            this.txtUserID.AfterExitEditMode += new System.EventHandler(clsUIHelper.AfterExitEditMode);

            this.Left = (frmMain.getInstance().Width - frmMain.getInstance().ultraExplorerBar1.Width - this.Width) / 2;
            this.Top = (frmMain.getInstance().Height - this.Height) / 2;

            clsUIHelper.setColorSchecme(this);
            this.dtpSaleEndDate.Value = DateTime.Now;
            this.dtpSaleStartDate.Value = DateTime.Now;
            this.dtpSaleStartDate.Focus();

            #region Sprint-19 - 2156 28-Jan-2015 JY Added
            this.Location = new Point(45, 2);
            clsUIHelper.SetAppearance(this.grdSearch);
            clsUIHelper.SetReadonlyRow(this.grdSearch);
            grdSearch.DisplayLayout.Override.HeaderClickAction = HeaderClickAction.SortMulti;
            grdSearch.DisplayLayout.AutoFitStyle = AutoFitStyle.ResizeAllColumns;
            btnSearch_Click(sender, e);
            #endregion
            this.optSummary.Checked = true; //Sprint-22 09-Dec-2015 JY Added
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
                    if (this.txtItemID.ContainsFocus == true)
                        this.SearchItem();
                    //Start:Added By Amit Date 4 may 2011
                    else if (this.txtVendorCode.ContainsFocus == true)
                        this.SearchVendor();
                    //End
                    else if (e.KeyData == System.Windows.Forms.Keys.F4) //Sprint-19 - 2156 23-Jan-2015 JY Added 
                        btnSearch_Click(sender, e);
                }
            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }

        private void PreviewReport(bool blnPrint, bool bCalledFromScheduler = false)   //PRIMEPOS-2485 02-Apr-2021 JY Added bCalledFromScheduler
        {
            try
            {
                this.Cursor = System.Windows.Forms.Cursors.WaitCursor;

                //rptSSalesByItem oRpt = new rptSSalesByItem(); //Sprint-21 - 2278 11-Mar-2016 JY Commented  
                CrystalDecisions.CrystalReports.Engine.ReportClass oRpt = null; //Sprint-21 - 2278 11-Mar-2016 JY Added
                String strQuery = "";

                //Sprint-21 - 2278 11-Mar-2016 JY Added
                if (this.optSummary.Checked == true)
                    oRpt = new rptSalesByItemSummary();
                else
                    oRpt = new rptSSalesByItem();

                //Sprint-21 - 2278 11-Mar-2016 JY Commented
                //if (this.optSummary.Checked == true)
                //    oRpt.DetailSection1.SectionFormat.EnableSuppress = true;    //Sprint-22 09-Dec-2015 JY Added to suppress detail section
                //else
                //    oRpt.GroupHeaderSection1.SectionFormat.EnableSuppress = true;    //Sprint-22 09-Dec-2015 JY Added to suppress detail section

                #region Commented Code
                //Start :Edited By Amit Date 1 June 2011 
                //original
                //strQuery="select PTD.ItemID,PTD.ItemDescription as Description,sum(PTD.Qty) as Qty,sum(PTD.ExtendedPrice) as Amount" +
                //" ,'" + this.dtpSaleStartDate.Text + "' as StartDate, '" + this.dtpSaleEndDate.Text + "' as EndDate from POSTransactionDetail PTD,Item I,POSTransaction PT where I.ItemID=PTD.ITemID and PT.TransID=PTD.TransID " +
                //" and  convert(datetime,PT.TransDate,109) between convert(datetime, cast('" + this.dtpSaleStartDate.Text  + " 00:00:00' as datetime) ,113) and convert(datetime, cast('" + this.dtpSaleEndDate.Text  + " 23:59:59' as datetime) ,113) ";

                //Followong Query is Commented  Shitaljit(QuicSolv) on 21 Sept 2011
                //strQuery = "select PTD.ItemID,PTD.ItemDescription as Description,IV.VendorItemID,sum(PTD.Qty)as Qty, sum(PTD.ExtendedPrice) as [Gross Sale],sum(PTD.Discount) as Discount," +
                //"sum(PTD.ExtendedPrice-PTD.Discount) as [Net Sale],sum(PTD.TaxAmount) as Tax,sum(PTD.ExtendedPrice-PTD.Discount+PTD.TaxAmount) as [Total Amount]" +
                //" ,'" + this.dtpSaleStartDate.Text + "' as StartDate, '" + this.dtpSaleEndDate.Text + "' as EndDate from POSTransactionDetail PTD,Item I,POSTransaction PT,ItemVendor IV  where I.ItemID=PTD.ITemID and PT.TransID=PTD.TransID and I.ItemID=IV.ItemID" +
                //" and  convert(datetime,PT.TransDate,109) between convert(datetime, cast('" + this.dtpSaleStartDate.Text + " 00:00:00' as datetime) ,113) and convert(datetime, cast('" + this.dtpSaleEndDate.Text + " 23:59:59' as datetime) ,113) ";
                //End

                //New Query added by Shitaljit(QuicSolv) on 21 Sept 2011
                //strQuery = "select PTD.ItemID,PTD.ItemDescription as Description,IV.VendorItemID,sum(PTD.Qty)as Qty, sum(PTD.ExtendedPrice) as [Gross Sale],sum(PTD.Discount) as Discount," +
                //        "sum(PTD.ExtendedPrice-PTD.Discount) as [Net Sale],sum(PTD.TaxAmount) as Tax,sum(PTD.ExtendedPrice-PTD.Discount+PTD.TaxAmount) as [Total Amount]" +
                //        " ,'" + this.dtpSaleStartDate.Text + "' as StartDate, '" + this.dtpSaleEndDate.Text + "' as EndDate from POSTransactionDetail PTD,Item I LEFT OUTER JOIN ItemVendor IV  ON I.ItemID=IV.ItemID,POSTransaction PT where I.ItemID=PTD.ITemID and PT.TransID=PTD.TransID " +
                //        " and  convert(datetime,PT.TransDate,109) between convert(datetime, cast('" + this.dtpSaleStartDate.Text + " 00:00:00' as datetime) ,113) and convert(datetime, cast('" + this.dtpSaleEndDate.Text + " 23:59:59' as datetime) ,113) ";
                //END of added by Shitaljit(QuicSolv) on 21 Sept 2011
                #endregion

                #region Sprint-19 - 2156 28-Jan-2015 JY Added
                ////Following query Added by Krishna on 21 October 2011
                ////Remoe Vendor and ItemVendorId by shitaljit on 4 Nov 2012
                //strQuery = "select (Select TOP 1 UserID from POSTransaction PT where  PT.TransID = PTD.TransID) as UserID, "+
                //        " PTD.ItemID,PTD.ItemDescription as Description,sum(PTD.Qty)as Qty, sum(PTD.ExtendedPrice) as [Gross Sale],sum(PTD.Discount) as Discount," +
                //        "sum(PTD.ExtendedPrice-PTD.Discount) as [Net Sale],sum(PTD.TaxAmount) as Tax,sum(PTD.ExtendedPrice-PTD.Discount+PTD.TaxAmount) as [Total Amount]," +
                //        " (Select SUM(TotalDiscAmount) from POSTransaction where transid in(select PT.TransID from POSTransaction PT where " +
                //        "  convert(datetime,PT.TransDate,109) between convert(datetime, cast('" + this.dtpSaleStartDate.Text + " 00:00:00' as datetime) ,113) and convert(datetime, cast('" + this.dtpSaleEndDate.Text + " 23:59:59' as datetime) ,113) )) as GrandTotDisc" +
                //        " ,'" + this.dtpSaleStartDate.Text + "' as StartDate, '" + this.dtpSaleEndDate.Text + "' as EndDate from POSTransactionDetail PTD,Item I, POSTransaction PT where I.ItemID=PTD.ITemID and PT.TransID=PTD.TransID " +
                //        " and  convert(datetime,PT.TransDate,109) between convert(datetime, cast('" + this.dtpSaleStartDate.Text + " 00:00:00' as datetime) ,113) and convert(datetime, cast('" + this.dtpSaleEndDate.Text + " 23:59:59' as datetime) ,113) ";
                ////Till here added by Krishna on 21 October 2011

                ////Start : Added By Amit Date 3 May 2011
                //if (string.IsNullOrEmpty(this.txtItemID.Text.Trim()) == false)
                //{
                //    strQuery += " and PTD.ItemID='" + this.txtItemID.Text.Trim() + "' ";
                //}
                //#region Commented Code
                ////if (this.txtVendorCode.Text.Trim() != "")
                ////{
                ////    strQuery += "AND V.VendorCode = '" + this.txtVendorCode.Text + "'";
                ////    ((CrystalDecisions.CrystalReports.Engine.TextObject)oRpt.ReportDefinition.ReportObjects["txtVendor"]).Text = "Vendor : " + this.txtVendorCode.Text;
                ////}
                ////else
                ////{
                ////    ((CrystalDecisions.CrystalReports.Engine.TextObject)oRpt.ReportDefinition.ReportObjects["txtVendor"]).Text = "";
                ////}
               
                ////if (this.txtItemID.Text.Trim() != "" && this.txtVendorCode.Text == "")
                ////{
                ////    strQuery += " and PTD.ItemID='" + this.txtItemID.Text.Trim() + "' ";
                ////    ((CrystalDecisions.CrystalReports.Engine.TextObject)oRpt.ReportDefinition.ReportObjects["txtVendor"]).Text = "";
                ////}
                ////else if (this.txtVendorCode.Text.Trim() != "")
                ////{
                ////    if (txtItemID.Text.Trim() == "")
                ////    {
                ////        strQuery += " and PTD.ItemID in(select IV.itemid from ItemVendor IV " +
                ////            "where IV.VendorId=(Select Vendorid from Vendor where VendorCode='" + this.txtVendorCode.Text + "'))";
                ////    }
                ////    else
                ////    {
                ////        strQuery += " and PTD.ItemID in(select IV.itemid from ItemVendor IV " +
                ////            "where IV.VendorId=(Select Vendorid from Vendor where VendorCode='" + this.txtVendorCode.Text + "')) and PTD.ItemID='" + this.txtItemID.Text + "'";
                ////    }
                ////    ((CrystalDecisions.CrystalReports.Engine.TextObject)oRpt.ReportDefinition.ReportObjects["txtVendor"]).Text = "Vendor : " + this.txtVendorCode.Text;
                ////}
                ////else
                ////{
                ////    ((CrystalDecisions.CrystalReports.Engine.TextObject)oRpt.ReportDefinition.ReportObjects["txtVendor"]).Text = "";
                ////}
                ////End
                //#endregion

                //if (chkIgnoreRx.Checked)
                //{
                //    strQuery += " and PTD.ItemID !='RX'";
                //}

                //if (string.IsNullOrEmpty(this.txtUserID.Text.Trim()) == false)
                //{
                //    strQuery += " AND PTD.TransID IN (SELECT TransID From POSTransaction WHERE UserID ='" + this.txtUserID.Text.Trim() + "')";
                //}
                //strQuery += " group by PTD.ItemID,PTD.ItemDescription, PTD.TransID ";
                //#region Commented Code
                ////Search oSearch=new Search();


                ////DataSet ds = oSearch.Search(strQuery);
                ///*oRpt.SetDataSource(ds);
                //clsUIHelper.ShowErrorMsg(ds.Tables[0].Rows.Count.ToString());
                //oRpt.Refresh();
                //frmReportViewer oViewer=new frmReportViewer();
                //oViewer.rvReportViewer.ReportSource=oRpt;
                //oViewer.MdiParent=frmMain.getInstance();
                //clsUIHelper.ShowErrorMsg(ds.Tables[0].Rows.Count.ToString());
                //oViewer.WindowState=FormWindowState.Maximized;
                //oViewer.Show();
                //*/
                //#endregion
                #endregion

                #region Sprint-19 - 2156 28-Jan-2015 JY Added
                string strUserId = string.Empty, strItem = string.Empty, strRxItem = string.Empty, strDisc = string.Empty;    //Sprint-21 - 1868 17-Jul-2015 JY Added strDisc
                string strDept = string.Empty, strSubDept = string.Empty;       //Sprint-24 - PRIMEPOS-2273 27-Oct-2016 JY Added
                if (this.txtUserID.Text.Trim() != "")
                    strUserId = " and PT.UserID='" + this.txtUserID.Text.Trim() + "' ";

                if (string.IsNullOrEmpty(this.txtItemID.Text.Trim()) == false)
                    strItem += " and PTD.ItemID='" + this.txtItemID.Text.Trim() + "' ";

                if (chkIgnoreRx.Checked)
                    strRxItem += " and PTD.ItemID !='RX'";

                if (chkOnlyDiscountedTrans.Checked) //Sprint-21 - 1868 17-Jul-2015 JY Added condition to consider only discounted transactions
                    strDisc = " AND PT.TotalDiscAmount <> 0 ";

                #region Sprint-24 - PRIMEPOS-2273 27-Oct-2016 JY Added
                if (Convert.ToString(this.txtDepartment.Tag).Trim() != "")
                    strDept = " AND I.DepartmentID in (" + this.txtDepartment.Tag.ToString().Trim() + ")";

                if (Convert.ToString(this.txtSubDepartment.Tag).Trim() != "")
                    strSubDept = " AND I.SUBDEPARTMENTID in (" + this.txtSubDepartment.Tag.ToString().Trim() + ")";
                #endregion

                strQuery = "select PT.TransID, PT.UserId, PTD.ItemID, PTD.ItemDescription as Description, sum(PTD.Qty)as Qty, sum(PTD.ExtendedPrice) as [Gross Sale], sum(PTD.Discount) as Discount, " +
                        " (SUM(PTD.ExtendedPrice)-SUM(PTD.Discount)) as [Net Sale], sum(PTD.TaxAmount) as Tax, (SUM(PTD.ExtendedPrice)-SUM(PTD.Discount)+SUM(PTD.TaxAmount)) as [Total Amount Collected], " +
                        " CASE WHEN ISNULL(SUM(PT.GrossTotal),0) = 0 then 0 else SUM(ISNULL(PT.INVOICEDISCOUNT,0))*SUM(PTD.ExtendedPrice)/SUM(PT.GrossTotal) END AS [Invoice Discount], " +
                        " '" + this.dtpSaleStartDate.Text + "' as StartDate, '" + this.dtpSaleEndDate.Text + "' as EndDate " +
                        " from POSTransactionDetail PTD " +
                        " INNER JOIN POSTransaction PT ON PT.TransID = PTD.TransID " +
                        " INNER JOIN Item I ON I.ItemID = PTD.ITemID " +
                        " LEFT JOIN Department Dept on Dept.DeptID = I.DepartmentID " +
                        " LEFT JOIN subDepartment SD on SD.DepartmentID = Dept.DeptID AND SD.SubDepartmentID = I.SUBDEPARTMENTID " +
                        " where convert(datetime,PT.TransDate,109) between convert(datetime, cast('" + this.dtpSaleStartDate.Text + " 00:00:00' as datetime) ,113) and convert(datetime, cast('" + this.dtpSaleEndDate.Text + " 23:59:59' as datetime) ,113) " +
                        strUserId + strItem + strRxItem + strDisc +  //Sprint-21 - 1868 17-Jul-2015 JY Added condition to consider only discounted transactions
                        strDept + strSubDept + //Sprint-24 - PRIMEPOS-2273 27-Oct-2016 JY Added
                        " group by PT.TransID, PTD.ItemID, PTD.ItemDescription, PT.UserId";
                #endregion
                
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
        private void SearchItem()
        {
            try
            {
                //frmSearch oSearch = new frmSearch(clsPOSDBConstants.Item_tbl);
                frmSearchMain oSearch = new frmSearchMain(true);    //20-Dec-2017 JY Added new reference
                oSearch.SearchTable = clsPOSDBConstants.Item_tbl;  //20-Dec-2017 JY Added 
                oSearch.ShowDialog(this);
                if (!oSearch.IsCanceled)
                {
                    string strCode = oSearch.SelectedRowID();
                    if (strCode == "")
                        return;

                    FKEdit(strCode, clsPOSDBConstants.Item_tbl);
                    this.txtItemID.Focus();
                }
            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }
        private void FKEdit(string code, string senderName)
        {
            if (senderName == clsPOSDBConstants.Item_tbl)
            {
                #region item
                try
                {
                    POS_Core.BusinessRules.Item oItem = new Item();
                    ItemData oItemData;
                    ItemRow oItemRow = null;
                    oItemData = oItem.Populate(code);
                    oItemRow = oItemData.Item[0];
                    if (oItemRow != null)
                    {
                        this.txtItemID.Text = oItemRow.ItemID;
                    }
                }
                catch (System.IndexOutOfRangeException)
                {
                    this.txtItemID.Text = String.Empty;
                    SearchItem();
                }
                catch (Exception exp)
                {
                    exp = null;
                    clsUIHelper.ShowErrorMsg(exp.Message);
                    this.txtItemID.Text = String.Empty;
                    SearchItem();
                }
                #endregion
            }
            else if (senderName == clsPOSDBConstants.Vendor_tbl)
            {
                try
                {
                    POS_Core.BusinessRules.Vendor oVendor = new POS_Core.BusinessRules.Vendor();
                    VendorData oVendorData;
                    VendorRow oVendorRow = null;
                    oVendorData = oVendor.Populate(code);
                    oVendorRow = oVendorData.Vendor[0];
                    if (oVendorRow != null)
                    {
                        this.txtVendorCode.Text = oVendorRow.Vendorcode;
                    }
                }
                catch (Exception exp)
                {
                    exp = null;
                    clsUIHelper.ShowErrorMsg(exp.Message);
                }
            }
        }

        private void txtItemID_EditorButtonClick(object sender, Infragistics.Win.UltraWinEditors.EditorButtonEventArgs e)
        {
            SearchItem();
        }
        //Start:Added By Amit Date 3 May 2011 
        private void txtVendorCode_EditorButtonClick(object sender, EditorButtonEventArgs e)
        {
            SearchVendor();
        }

        private void SearchVendor()
        {
            try
            {
                //frmSearch oSearch = new frmSearch(clsPOSDBConstants.Vendor_tbl);                
                //if (txtVendorCode.Text != "")
                //    oSearch.txtCode.Text = txtVendorCode.Text;
                frmSearchMain oSearch = new frmSearchMain(clsPOSDBConstants.Vendor_tbl, txtVendorCode.Text, "", true);    //20-Dec-2017 JY Added new reference
                oSearch.SearchTable = clsPOSDBConstants.Vendor_tbl; //20-Dec-2017 JY Added 
                oSearch.SearchInConstructor = true;
                oSearch.ShowDialog();
                if (oSearch.IsCanceled) return;
                txtVendorCode.Text = oSearch.SelectedRowID();
            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }

        private void txtVendorCode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter && this.txtVendorCode.Text != "")
            {
                FKEdit(this.txtVendorCode.Text, clsPOSDBConstants.Vendor_tbl);
            }
        }
        //End

        #region Sprint-19 - 2156 28-Jan-2015 JY Added
        private void btnSearch_Click(object sender, EventArgs e)
        {
            String strSearchSQL;
            try
            {
                dsSearch = new DataSet();
                DataSet dsSearchTmp = new DataSet();

                GenerateSQL(out strSearchSQL);

                dsSearchTmp.Tables.Add(oSearchSvr.Search(strSearchSQL).Tables[0].Copy());

                var varSummary = from Trans in dsSearchTmp.Tables[0].AsEnumerable()
                                 group Trans by new
                                 {
                                     user = Trans.Field<string>("UserId"),
                                     Item = Trans.Field<string>("ItemID"),
                                     ItemDesc = Trans.Field<string>("Description")
                                 } into userg
                                 select new
                                 {
                                     UserId = userg.Key.user,
                                     ItemId = userg.Key.Item,
                                     ItemName = userg.Key.ItemDesc,
                                     TotalQty = userg.Sum(x => x.Field<int>("Qty")),
                                     GrossSale = userg.Sum(x => x.Field<decimal>("Gross Sale")),
                                     Discount = userg.Sum(x => x.Field<decimal>("Discount")),
                                     NetSale = userg.Sum(x => x.Field<decimal>("Net Sale")),
                                     Tax = userg.Sum(x => x.Field<decimal>("Tax")),
                                     TotalAmountCollected = userg.Sum(x => x.Field<decimal>("Total Amount Collected"))
                                 };

                DataTable dt = new DataTable();
                dt.Columns.Add("UserId", typeof(System.String));
                dt.Columns.Add("ItemId", typeof(System.String));
                dt.Columns.Add("Item Description", typeof(System.String));
                dt.Columns.Add("Total Quantity", typeof(System.Int32));
                dt.Columns.Add("Gross Sale", typeof(System.Decimal));
                dt.Columns.Add("Discount", typeof(System.Decimal));
                dt.Columns.Add("Net Sale", typeof(System.Decimal));
                dt.Columns.Add("Tax", typeof(System.Decimal));
                dt.Columns.Add("Total Amount Collected", typeof(System.Decimal));
                DataRow dr;

                foreach (var Summary in varSummary)
                {
                    dr = dt.NewRow();
                    dr["UserId"] = Summary.UserId;
                    dr["ItemId"] = Summary.ItemId;
                    dr["Item Description"] = Summary.ItemName;
                    dr["Total Quantity"] = Summary.TotalQty;
                    dr["Gross Sale"] = Summary.GrossSale;
                    dr["Discount"] = Summary.Discount;
                    dr["Net Sale"] = Summary.NetSale;
                    dr["Tax"] = Summary.Tax;
                    dr["Total Amount Collected"] = Summary.TotalAmountCollected;
                    dt.Rows.Add(dr);
                }

                dsSearch.Tables.Add(dt);   //Added table for summary
                dsSearch.Tables[0].TableName = "Summary";
                dsSearch.Tables.Add(dsSearchTmp.Tables[0].Copy());
                dsSearch.Tables[1].TableName = "Details";

                DataColumn[] MasterCols;
                DataColumn[] DetailCols;
                MasterCols = new DataColumn[] { dsSearch.Tables[0].Columns["UserId"], dsSearch.Tables[0].Columns["ItemId"], dsSearch.Tables[0].Columns["Item Description"] };
                DetailCols = new DataColumn[] { dsSearch.Tables[1].Columns["UserId"], dsSearch.Tables[1].Columns["ItemId"], dsSearch.Tables[1].Columns["Description"] };

                DataRelation rel = new DataRelation("myDataRelation", MasterCols, DetailCols);
                dsSearch.Relations.Add(rel);

                grdSearch.DataSource = dsSearch;
                grdSearch.DisplayLayout.Bands[0].HeaderVisible = true;
                grdSearch.DisplayLayout.Bands[0].Header.Appearance.FontData.SizeInPoints = 10;
                grdSearch.DisplayLayout.Bands[0].Header.Appearance.TextHAlign = Infragistics.Win.HAlign.Center;
                grdSearch.DisplayLayout.Bands[0].Header.Caption = "Summary";
                grdSearch.DisplayLayout.Bands[0].Override.WrapHeaderText = Infragistics.Win.DefaultableBoolean.True;

                grdSearch.DisplayLayout.Bands[1].HeaderVisible = true;
                grdSearch.DisplayLayout.Bands[1].Header.Caption = "Detail";
                grdSearch.DisplayLayout.Bands[1].Header.Appearance.FontData.SizeInPoints = 10;
                grdSearch.DisplayLayout.Bands[1].Header.Appearance.TextHAlign = Infragistics.Win.HAlign.Center;
                grdSearch.DisplayLayout.Bands[1].Columns["UserId"].Hidden = true;
                grdSearch.DisplayLayout.Bands[1].Columns["ItemID"].Hidden = true;
                grdSearch.DisplayLayout.Bands[1].Columns["Description"].Hidden = true;
                grdSearch.DisplayLayout.Bands[1].Columns["Invoice Discount"].Hidden = true;
                grdSearch.DisplayLayout.Bands[1].Expandable = true;
                grdSearch.DisplayLayout.Bands[1].Override.WrapHeaderText = Infragistics.Win.DefaultableBoolean.True;

                resizeColumns(grdSearch);
                grdSearch.PerformAction(UltraGridAction.FirstRowInGrid);
                grdSearch.Refresh();
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

        private void GenerateSQL(out string strSearchSQL)
        {
            string strUserId = string.Empty, strItem = string.Empty, strRxItem = string.Empty, strDisc = string.Empty;    //Sprint-21 - 1868 17-Jul-2015 JY Added strDisc
            string strDept = string.Empty, strSubDept = string.Empty;       //Sprint-24 - PRIMEPOS-2273 27-Oct-2016 JY Added

            if (this.txtUserID.Text.Trim() != "")
                strUserId = " and PT.UserID='" + this.txtUserID.Text.Trim() + "' ";

            if (string.IsNullOrEmpty(this.txtItemID.Text.Trim()) == false)
                strItem += " and PTD.ItemID='" + this.txtItemID.Text.Trim() + "' ";
                
            if (chkIgnoreRx.Checked)
                strRxItem += " and PTD.ItemID !='RX'";

            if (chkOnlyDiscountedTrans.Checked) //Sprint-21 - 1868 17-Jul-2015 JY Added condition to consider only discounted transactions
                strDisc = " AND PT.TotalDiscAmount <> 0 ";

            #region Sprint-24 - PRIMEPOS-2273 27-Oct-2016 JY Added
            if (Convert.ToString(this.txtDepartment.Tag).Trim() != "")
                strDept = " AND I.DepartmentID in (" + this.txtDepartment.Tag.ToString().Trim() + ")";

            if (Convert.ToString(this.txtSubDepartment.Tag).Trim() != "")
                strSubDept = " AND I.SUBDEPARTMENTID in (" + this.txtSubDepartment.Tag.ToString().Trim() + ")";
            #endregion

            strSearchSQL = "select PT.TransID, PT.TransDate, UPPER(LTRIM(RTRIM(PT.UserId))) AS UserId, UPPER(LTRIM(RTRIM(PTD.ItemID))) AS ItemID, UPPER(LTRIM(RTRIM(PTD.ItemDescription))) as Description, sum(PTD.Qty)as Qty, sum(PTD.ExtendedPrice) as [Gross Sale], sum(PTD.Discount) as Discount, " +
                    " (SUM(PTD.ExtendedPrice)-SUM(PTD.Discount)) as [Net Sale], sum(PTD.TaxAmount) as Tax, (SUM(PTD.ExtendedPrice)-SUM(PTD.Discount)+SUM(PTD.TaxAmount)) as [Total Amount Collected], " +
                    " CASE WHEN ISNULL(SUM(PT.GrossTotal),0) = 0 then 0 else SUM(ISNULL(PT.INVOICEDISCOUNT,0))*SUM(PTD.ExtendedPrice)/SUM(PT.GrossTotal) END AS [Invoice Discount] " +
                    " from POSTransactionDetail PTD " +
                    " INNER JOIN POSTransaction PT ON PT.TransID = PTD.TransID " + 
                    " INNER JOIN Item I ON I.ItemID = PTD.ITemID " +
                    " LEFT JOIN Department Dept on Dept.DeptID = I.DepartmentID " +
                    " LEFT JOIN subDepartment SD on SD.DepartmentID = Dept.DeptID AND SD.SubDepartmentID = I.SUBDEPARTMENTID " +
                    " where convert(datetime,PT.TransDate,109) between convert(datetime, cast('" + this.dtpSaleStartDate.Text + " 00:00:00' as datetime) ,113) and convert(datetime, cast('" + this.dtpSaleEndDate.Text + " 23:59:59' as datetime) ,113) " +
                    strUserId + strItem + strRxItem + strDisc + //Sprint-21 - 1868 17-Jul-2015 JY Added condition to consider only discounted transactions
                    strDept + strSubDept + //Sprint-24 - PRIMEPOS-2273 27-Oct-2016 JY Added
                    " group by PT.TransID, PT.TransDate, PTD.ItemID, PTD.ItemDescription, PT.UserId ORDER BY PT.UserId, PTD.ItemID, PTD.ItemDescription";
        }

        private void grdSearch_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
        {
            FormatGridColumn(grdSearch, 2);
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

        private void grdSearch_BeforeRowExpanded(object sender, CancelableRowEventArgs e)
        {
            foreach (Infragistics.Win.UltraWinGrid.UltraGridRow orow in grdSearch.DisplayLayout.Rows)
            {
                if (orow.Expanded == true)
                    orow.CollapseAll();
            }
            e.Row.Activate();
        }

        private void grdSearch_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                Point point = System.Windows.Forms.Cursor.Position;
                point = this.grdSearch.PointToClient(point);
                Infragistics.Win.UIElement oUI = this.grdSearch.DisplayLayout.UIElement.ElementFromPoint(point);
                if (oUI == null)
                    return;

                while (oUI != null)
                {
                    if (oUI.GetType() == typeof(Infragistics.Win.UltraWinGrid.RowUIElement))
                    {
                        Infragistics.Win.UltraWinGrid.RowUIElement oRowUI = oUI as Infragistics.Win.UltraWinGrid.RowUIElement;
                        foreach (Infragistics.Win.UltraWinGrid.UltraGridRow orow in grdSearch.DisplayLayout.Rows)
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

        private void grdSearch_MouseMove(object sender, MouseEventArgs e)
        {
            this.CurrentX = e.X;
            this.CurrentY = e.Y;
        }
        #endregion

        #region Sprint-24 - PRIMEPOS-2273 27-Oct-2016 JY Added
        private void txtDepartment_EditorButtonClick(object sender, EditorButtonEventArgs e)
        {
            SearchDept();
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

        private void txtSubDepartment_EditorButtonClick(object sender, EditorButtonEventArgs e)
        {
            SearchSubDept();
        }

        private void SearchSubDept()
        {
            try
            {
                string InQuery = string.Empty;
                if (txtDepartment.Tag != null && txtDepartment.Tag.ToString().Length > 2)
                {
                    InQuery = txtDepartment.Tag.ToString().Substring(1,txtDepartment.Tag.ToString().Length-2);
                }

                SearchSvr.SubDeptIDFlag = true;
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
        #endregion

        #region PRIMEPOS-2485 02-Apr-2021 JY Added
        public bool bSendPrint = true;
        private ReportClass oReport = new ReportClass();
        public usrDateRangeParams customControl;
        private const string ReportName = "SalesReportByItem";

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

