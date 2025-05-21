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
//using POS_Core.DataAccess;

namespace POS_Core_UI.Reports.ReportsUI
{
	/// <summary>
	/// Summary description for frmViewTransaction.
	/// </summary>
	public class frmRptSSalesByUID : System.Windows.Forms.Form, ICommandLIneTaskControl
    {
		private System.Windows.Forms.GroupBox groupBox1;
        private Infragistics.Win.Misc.UltraLabel ultraLabel12;
		private Infragistics.Win.UltraWinEditors.UltraTextEditor txtUserID;
		private Infragistics.Win.Misc.UltraLabel ultraLabel20;
		private Infragistics.Win.Misc.UltraLabel ultraLabel19;
		private Infragistics.Win.UltraWinSchedule.UltraCalendarCombo dtpSaleEndDate;
		private Infragistics.Win.UltraWinSchedule.UltraCalendarCombo dtpSaleStartDate;
		private System.Windows.Forms.GroupBox groupBox2;
		private Infragistics.Win.Misc.UltraButton btnPrint;
		private Infragistics.Win.Misc.UltraButton btnView;
		private Infragistics.Win.Misc.UltraButton btnClose;
        private Infragistics.Win.Misc.UltraButton btnSearch;
        private Infragistics.Win.UltraWinGrid.UltraGrid grdSale;
        private Infragistics.Win.UltraWinGrid.UltraGrid grdReturn;

        #region Sprint-19 - 2155 19-Jan-2015 JY Added
        private DataSet dsSales;   
        private DataSet dsReturns;   
        private SearchSvr oSearchSvr = new SearchSvr(); 
        private int CurrentX; 
        private int CurrentY; 
        #endregion
        private UltraCheckEditor chkOnlyDiscountedTrans;

        /// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public frmRptSSalesByUID()
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
            Infragistics.Win.UltraWinSchedule.CalendarCombo.DateButton dateButton1 = new Infragistics.Win.UltraWinSchedule.CalendarCombo.DateButton();
            Infragistics.Win.UltraWinSchedule.CalendarCombo.DateButton dateButton2 = new Infragistics.Win.UltraWinSchedule.CalendarCombo.DateButton();
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmRptSSalesByUID));
            Infragistics.Win.Appearance appearance7 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance8 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance9 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand1 = new Infragistics.Win.UltraWinGrid.UltraGridBand("Band 0", -1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn4 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Column Name");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn5 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Type");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn6 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Criteria Value");
            Infragistics.Win.Appearance appearance10 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance11 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance12 = new Infragistics.Win.Appearance();
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
            Infragistics.Win.UltraWinScrollBar.ScrollBarLook scrollBarLook1 = new Infragistics.Win.UltraWinScrollBar.ScrollBarLook();
            Infragistics.Win.Appearance appearance29 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance30 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance31 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance32 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance33 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand2 = new Infragistics.Win.UltraWinGrid.UltraGridBand("Band 0", -1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn1 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Column Name");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn2 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Type");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn3 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Criteria Value");
            Infragistics.Win.Appearance appearance34 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance35 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance36 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance37 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance38 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance39 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance40 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance41 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance42 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance43 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance44 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance45 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance46 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance47 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance48 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance49 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance50 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance51 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance52 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinScrollBar.ScrollBarLook scrollBarLook2 = new Infragistics.Win.UltraWinScrollBar.ScrollBarLook();
            Infragistics.Win.Appearance appearance53 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance54 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance55 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance56 = new Infragistics.Win.Appearance();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.chkOnlyDiscountedTrans = new Infragistics.Win.UltraWinEditors.UltraCheckEditor();
            this.btnSearch = new Infragistics.Win.Misc.UltraButton();
            this.ultraLabel20 = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabel19 = new Infragistics.Win.Misc.UltraLabel();
            this.dtpSaleEndDate = new Infragistics.Win.UltraWinSchedule.UltraCalendarCombo();
            this.dtpSaleStartDate = new Infragistics.Win.UltraWinSchedule.UltraCalendarCombo();
            this.ultraLabel12 = new Infragistics.Win.Misc.UltraLabel();
            this.txtUserID = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnPrint = new Infragistics.Win.Misc.UltraButton();
            this.btnClose = new Infragistics.Win.Misc.UltraButton();
            this.btnView = new Infragistics.Win.Misc.UltraButton();
            this.grdSale = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.grdReturn = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chkOnlyDiscountedTrans)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtpSaleEndDate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtpSaleStartDate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtUserID)).BeginInit();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdSale)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdReturn)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.chkOnlyDiscountedTrans);
            this.groupBox1.Controls.Add(this.btnSearch);
            this.groupBox1.Controls.Add(this.ultraLabel20);
            this.groupBox1.Controls.Add(this.ultraLabel19);
            this.groupBox1.Controls.Add(this.dtpSaleEndDate);
            this.groupBox1.Controls.Add(this.dtpSaleStartDate);
            this.groupBox1.Controls.Add(this.ultraLabel12);
            this.groupBox1.Controls.Add(this.txtUserID);
            this.groupBox1.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.4F);
            this.groupBox1.Location = new System.Drawing.Point(4, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(977, 75);
            this.groupBox1.TabIndex = 27;
            this.groupBox1.TabStop = false;
            // 
            // chkOnlyDiscountedTrans
            // 
            appearance1.ForeColor = System.Drawing.Color.White;
            this.chkOnlyDiscountedTrans.Appearance = appearance1;
            this.chkOnlyDiscountedTrans.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkOnlyDiscountedTrans.Location = new System.Drawing.Point(207, 45);
            this.chkOnlyDiscountedTrans.Name = "chkOnlyDiscountedTrans";
            this.chkOnlyDiscountedTrans.Size = new System.Drawing.Size(185, 20);
            this.chkOnlyDiscountedTrans.TabIndex = 3;
            this.chkOnlyDiscountedTrans.Text = "Only Discounted Trans";
            // 
            // btnSearch
            // 
            appearance2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(62)))), ((int)(((byte)(176)))));
            appearance2.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance2.FontData.BoldAsString = "True";
            appearance2.ForeColor = System.Drawing.Color.White;
            this.btnSearch.Appearance = appearance2;
            this.btnSearch.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            this.btnSearch.Location = new System.Drawing.Point(554, 45);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(117, 24);
            this.btnSearch.TabIndex = 4;
            this.btnSearch.Text = "&Search (F4)";
            this.btnSearch.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // ultraLabel20
            // 
            appearance3.ForeColor = System.Drawing.Color.White;
            this.ultraLabel20.Appearance = appearance3;
            this.ultraLabel20.BorderStyleInner = Infragistics.Win.UIElementBorderStyle.None;
            this.ultraLabel20.Location = new System.Drawing.Point(8, 18);
            this.ultraLabel20.Name = "ultraLabel20";
            this.ultraLabel20.Size = new System.Drawing.Size(64, 23);
            this.ultraLabel20.TabIndex = 20;
            this.ultraLabel20.Text = "Start Date";
            // 
            // ultraLabel19
            // 
            appearance4.ForeColor = System.Drawing.Color.White;
            this.ultraLabel19.Appearance = appearance4;
            this.ultraLabel19.BorderStyleInner = Infragistics.Win.UIElementBorderStyle.None;
            this.ultraLabel19.Location = new System.Drawing.Point(8, 45);
            this.ultraLabel19.Name = "ultraLabel19";
            this.ultraLabel19.Size = new System.Drawing.Size(59, 23);
            this.ultraLabel19.TabIndex = 22;
            this.ultraLabel19.Text = "End Date";
            // 
            // dtpSaleEndDate
            // 
            this.dtpSaleEndDate.AllowNull = false;
            this.dtpSaleEndDate.AutoSize = false;
            this.dtpSaleEndDate.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Office2003ToolbarButton;
            this.dtpSaleEndDate.DateButtons.Add(dateButton1);
            this.dtpSaleEndDate.Location = new System.Drawing.Point(78, 45);
            this.dtpSaleEndDate.Name = "dtpSaleEndDate";
            this.dtpSaleEndDate.NonAutoSizeHeight = 10;
            this.dtpSaleEndDate.Size = new System.Drawing.Size(123, 23);
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
            this.dtpSaleStartDate.Location = new System.Drawing.Point(78, 16);
            this.dtpSaleStartDate.Name = "dtpSaleStartDate";
            this.dtpSaleStartDate.NonAutoSizeHeight = 10;
            this.dtpSaleStartDate.Size = new System.Drawing.Size(123, 23);
            this.dtpSaleStartDate.TabIndex = 0;
            this.dtpSaleStartDate.Tag = "From Date";
            this.dtpSaleStartDate.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.dtpSaleStartDate.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.dtpSaleStartDate.Value = new System.DateTime(2004, 5, 25, 0, 0, 0, 0);
            // 
            // ultraLabel12
            // 
            appearance5.ForeColor = System.Drawing.Color.White;
            this.ultraLabel12.Appearance = appearance5;
            this.ultraLabel12.BorderStyleInner = Infragistics.Win.UIElementBorderStyle.None;
            this.ultraLabel12.Location = new System.Drawing.Point(207, 18);
            this.ultraLabel12.Name = "ultraLabel12";
            this.ultraLabel12.Size = new System.Drawing.Size(167, 23);
            this.ultraLabel12.TabIndex = 6;
            this.ultraLabel12.Text = "User ID <Blank = Any User>";
            // 
            // txtUserID
            // 
            this.txtUserID.AutoSize = false;
            this.txtUserID.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.txtUserID.Location = new System.Drawing.Point(380, 16);
            this.txtUserID.MaxLength = 20;
            this.txtUserID.Name = "txtUserID";
            this.txtUserID.Size = new System.Drawing.Size(164, 23);
            this.txtUserID.TabIndex = 2;
            this.txtUserID.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.btnPrint);
            this.groupBox2.Controls.Add(this.btnClose);
            this.groupBox2.Controls.Add(this.btnView);
            this.groupBox2.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.groupBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.4F);
            this.groupBox2.Location = new System.Drawing.Point(4, 408);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(977, 47);
            this.groupBox2.TabIndex = 31;
            this.groupBox2.TabStop = false;
            // 
            // btnPrint
            // 
            this.btnPrint.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            appearance6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(62)))), ((int)(((byte)(176)))));
            appearance6.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance6.FontData.BoldAsString = "True";
            appearance6.ForeColor = System.Drawing.Color.White;
            appearance6.Image = ((object)(resources.GetObject("appearance6.Image")));
            this.btnPrint.Appearance = appearance6;
            this.btnPrint.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            this.btnPrint.Location = new System.Drawing.Point(699, 15);
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
            appearance7.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(62)))), ((int)(((byte)(176)))));
            appearance7.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance7.FontData.BoldAsString = "True";
            appearance7.ForeColor = System.Drawing.Color.White;
            appearance7.Image = ((object)(resources.GetObject("appearance7.Image")));
            this.btnClose.Appearance = appearance7;
            this.btnClose.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(883, 15);
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
            appearance8.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(62)))), ((int)(((byte)(176)))));
            appearance8.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance8.FontData.BoldAsString = "True";
            appearance8.ForeColor = System.Drawing.Color.White;
            appearance8.Image = ((object)(resources.GetObject("appearance8.Image")));
            this.btnView.Appearance = appearance8;
            this.btnView.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            this.btnView.Location = new System.Drawing.Point(791, 15);
            this.btnView.Name = "btnView";
            this.btnView.Size = new System.Drawing.Size(85, 26);
            this.btnView.TabIndex = 1;
            this.btnView.Text = "&View";
            this.btnView.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnView.Click += new System.EventHandler(this.btnView_Click_1);
            // 
            // grdSale
            // 
            this.grdSale.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            appearance9.BackColor = System.Drawing.Color.White;
            appearance9.BackColor2 = System.Drawing.Color.White;
            appearance9.BackColorDisabled = System.Drawing.Color.White;
            appearance9.BackColorDisabled2 = System.Drawing.Color.White;
            appearance9.BackGradientStyle = Infragistics.Win.GradientStyle.None;
            this.grdSale.DisplayLayout.Appearance = appearance9;
            ultraGridColumn4.Header.VisiblePosition = 0;
            ultraGridColumn5.Header.VisiblePosition = 1;
            ultraGridColumn6.Header.VisiblePosition = 2;
            ultraGridBand1.Columns.AddRange(new object[] {
            ultraGridColumn4,
            ultraGridColumn5,
            ultraGridColumn6});
            ultraGridBand1.HeaderVisible = true;
            this.grdSale.DisplayLayout.BandsSerializer.Add(ultraGridBand1);
            this.grdSale.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.grdSale.DisplayLayout.InterBandSpacing = 10;
            this.grdSale.DisplayLayout.MaxColScrollRegions = 1;
            this.grdSale.DisplayLayout.MaxRowScrollRegions = 1;
            appearance10.BackColor = System.Drawing.Color.White;
            appearance10.BackColor2 = System.Drawing.Color.White;
            this.grdSale.DisplayLayout.Override.ActiveCardCaptionAppearance = appearance10;
            appearance11.BackColor = System.Drawing.Color.White;
            appearance11.BackColor2 = System.Drawing.Color.White;
            appearance11.BorderColor = System.Drawing.Color.Gray;
            this.grdSale.DisplayLayout.Override.ActiveRowAppearance = appearance11;
            appearance12.BackColor = System.Drawing.Color.White;
            appearance12.BackColor2 = System.Drawing.Color.White;
            appearance12.BorderColor = System.Drawing.Color.Gray;
            this.grdSale.DisplayLayout.Override.AddRowAppearance = appearance12;
            this.grdSale.DisplayLayout.Override.AllowAddNew = Infragistics.Win.UltraWinGrid.AllowAddNew.No;
            this.grdSale.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.False;
            this.grdSale.DisplayLayout.Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.False;
            appearance13.BackColor = System.Drawing.Color.Transparent;
            this.grdSale.DisplayLayout.Override.CardAreaAppearance = appearance13;
            appearance14.BackColor = System.Drawing.Color.White;
            appearance14.BackColor2 = System.Drawing.Color.White;
            appearance14.BackColorDisabled = System.Drawing.Color.White;
            appearance14.BackColorDisabled2 = System.Drawing.Color.White;
            appearance14.BorderColor = System.Drawing.Color.Black;
            appearance14.BorderColor3DBase = System.Drawing.Color.Black;
            this.grdSale.DisplayLayout.Override.CellAppearance = appearance14;
            appearance15.BackColor = System.Drawing.Color.White;
            appearance15.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(231)))), ((int)(((byte)(230)))));
            appearance15.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance15.BorderAlpha = Infragistics.Win.Alpha.Transparent;
            appearance15.BorderColor = System.Drawing.Color.Gray;
            appearance15.BorderColor3DBase = System.Drawing.Color.Gray;
            appearance15.ImageBackgroundAlpha = Infragistics.Win.Alpha.Transparent;
            appearance15.ImageBackgroundStyle = Infragistics.Win.ImageBackgroundStyle.Centered;
            this.grdSale.DisplayLayout.Override.CellButtonAppearance = appearance15;
            appearance16.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            appearance16.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.grdSale.DisplayLayout.Override.EditCellAppearance = appearance16;
            appearance17.BorderColor = System.Drawing.Color.Gray;
            this.grdSale.DisplayLayout.Override.FilteredInRowAppearance = appearance17;
            appearance18.BorderColor = System.Drawing.Color.Gray;
            this.grdSale.DisplayLayout.Override.FilteredOutRowAppearance = appearance18;
            appearance19.BackColor = System.Drawing.Color.White;
            appearance19.BackColor2 = System.Drawing.Color.White;
            appearance19.BackColorDisabled = System.Drawing.Color.White;
            appearance19.BackColorDisabled2 = System.Drawing.Color.White;
            this.grdSale.DisplayLayout.Override.FixedCellAppearance = appearance19;
            appearance20.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            appearance20.BackColor2 = System.Drawing.Color.Beige;
            this.grdSale.DisplayLayout.Override.FixedHeaderAppearance = appearance20;
            appearance21.BackColor = System.Drawing.Color.White;
            appearance21.BackColor2 = System.Drawing.SystemColors.Control;
            appearance21.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance21.FontData.BoldAsString = "True";
            appearance21.ForeColor = System.Drawing.Color.Black;
            appearance21.TextHAlignAsString = "Left";
            appearance21.ThemedElementAlpha = Infragistics.Win.Alpha.Transparent;
            this.grdSale.DisplayLayout.Override.HeaderAppearance = appearance21;
            appearance22.BorderColor = System.Drawing.Color.Gray;
            this.grdSale.DisplayLayout.Override.RowAlternateAppearance = appearance22;
            appearance23.BackColor = System.Drawing.Color.White;
            appearance23.BackColor2 = System.Drawing.Color.White;
            appearance23.BackGradientStyle = Infragistics.Win.GradientStyle.None;
            appearance23.BackHatchStyle = Infragistics.Win.BackHatchStyle.None;
            appearance23.BorderColor = System.Drawing.Color.Gray;
            this.grdSale.DisplayLayout.Override.RowAppearance = appearance23;
            appearance24.BorderColor = System.Drawing.Color.Gray;
            this.grdSale.DisplayLayout.Override.RowPreviewAppearance = appearance24;
            appearance25.BackColor = System.Drawing.Color.White;
            appearance25.BackColor2 = System.Drawing.SystemColors.Control;
            appearance25.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance25.BorderColor = System.Drawing.Color.Gray;
            this.grdSale.DisplayLayout.Override.RowSelectorAppearance = appearance25;
            this.grdSale.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            this.grdSale.DisplayLayout.Override.RowSelectorWidth = 12;
            this.grdSale.DisplayLayout.Override.RowSpacingBefore = 2;
            appearance26.BackColor = System.Drawing.Color.Navy;
            appearance26.BackColorAlpha = Infragistics.Win.Alpha.Opaque;
            this.grdSale.DisplayLayout.Override.SelectedCellAppearance = appearance26;
            appearance27.BackColor = System.Drawing.Color.Navy;
            appearance27.BackColorDisabled = System.Drawing.Color.Navy;
            appearance27.BackGradientStyle = Infragistics.Win.GradientStyle.None;
            appearance27.BackHatchStyle = Infragistics.Win.BackHatchStyle.None;
            appearance27.BorderColor = System.Drawing.Color.Gray;
            appearance27.ForeColor = System.Drawing.Color.Black;
            this.grdSale.DisplayLayout.Override.SelectedRowAppearance = appearance27;
            this.grdSale.DisplayLayout.Override.SelectTypeCell = Infragistics.Win.UltraWinGrid.SelectType.None;
            this.grdSale.DisplayLayout.Override.SelectTypeCol = Infragistics.Win.UltraWinGrid.SelectType.None;
            this.grdSale.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.Single;
            appearance28.BorderColor = System.Drawing.Color.Gray;
            this.grdSale.DisplayLayout.Override.TemplateAddRowAppearance = appearance28;
            this.grdSale.DisplayLayout.RowConnectorColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            this.grdSale.DisplayLayout.RowConnectorStyle = Infragistics.Win.UltraWinGrid.RowConnectorStyle.Solid;
            appearance29.BackColor = System.Drawing.Color.White;
            appearance29.BackColor2 = System.Drawing.SystemColors.Control;
            appearance29.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            scrollBarLook1.Appearance = appearance29;
            appearance30.BackColor = System.Drawing.Color.White;
            appearance30.BackColor2 = System.Drawing.SystemColors.Control;
            appearance30.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance30.ThemedElementAlpha = Infragistics.Win.Alpha.Transparent;
            scrollBarLook1.ButtonAppearance = appearance30;
            appearance31.BackColor = System.Drawing.Color.White;
            appearance31.BackColor2 = System.Drawing.SystemColors.Control;
            scrollBarLook1.ThumbAppearance = appearance31;
            appearance32.BackColor = System.Drawing.Color.White;
            appearance32.BackColor2 = System.Drawing.Color.White;
            scrollBarLook1.TrackAppearance = appearance32;
            this.grdSale.DisplayLayout.ScrollBarLook = scrollBarLook1;
            this.grdSale.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grdSale.Location = new System.Drawing.Point(4, 80);
            this.grdSale.Name = "grdSale";
            this.grdSale.Size = new System.Drawing.Size(485, 331);
            this.grdSale.TabIndex = 0;
            this.grdSale.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.grdSale.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.grdSale_InitializeLayout);
            this.grdSale.BeforeRowExpanded += new Infragistics.Win.UltraWinGrid.CancelableRowEventHandler(this.grdSale_BeforeRowExpanded);
            this.grdSale.DoubleClick += new System.EventHandler(this.grdSale_DoubleClick);
            this.grdSale.MouseMove += new System.Windows.Forms.MouseEventHandler(this.grdSale_MouseMove);
            // 
            // grdReturn
            // 
            this.grdReturn.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            appearance33.BackColor = System.Drawing.Color.White;
            appearance33.BackColor2 = System.Drawing.Color.White;
            appearance33.BackColorDisabled = System.Drawing.Color.White;
            appearance33.BackColorDisabled2 = System.Drawing.Color.White;
            appearance33.BackGradientStyle = Infragistics.Win.GradientStyle.None;
            this.grdReturn.DisplayLayout.Appearance = appearance33;
            ultraGridColumn1.Header.VisiblePosition = 0;
            ultraGridColumn2.Header.VisiblePosition = 1;
            ultraGridColumn3.Header.VisiblePosition = 2;
            ultraGridBand2.Columns.AddRange(new object[] {
            ultraGridColumn1,
            ultraGridColumn2,
            ultraGridColumn3});
            ultraGridBand2.HeaderVisible = true;
            this.grdReturn.DisplayLayout.BandsSerializer.Add(ultraGridBand2);
            this.grdReturn.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.grdReturn.DisplayLayout.InterBandSpacing = 10;
            this.grdReturn.DisplayLayout.MaxColScrollRegions = 1;
            this.grdReturn.DisplayLayout.MaxRowScrollRegions = 1;
            appearance34.BackColor = System.Drawing.Color.White;
            appearance34.BackColor2 = System.Drawing.Color.White;
            this.grdReturn.DisplayLayout.Override.ActiveCardCaptionAppearance = appearance34;
            appearance35.BackColor = System.Drawing.Color.White;
            appearance35.BackColor2 = System.Drawing.Color.White;
            appearance35.BorderColor = System.Drawing.Color.Gray;
            this.grdReturn.DisplayLayout.Override.ActiveRowAppearance = appearance35;
            appearance36.BackColor = System.Drawing.Color.White;
            appearance36.BackColor2 = System.Drawing.Color.White;
            appearance36.BorderColor = System.Drawing.Color.Gray;
            this.grdReturn.DisplayLayout.Override.AddRowAppearance = appearance36;
            this.grdReturn.DisplayLayout.Override.AllowAddNew = Infragistics.Win.UltraWinGrid.AllowAddNew.No;
            this.grdReturn.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.False;
            this.grdReturn.DisplayLayout.Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.False;
            appearance37.BackColor = System.Drawing.Color.Transparent;
            this.grdReturn.DisplayLayout.Override.CardAreaAppearance = appearance37;
            appearance38.BackColor = System.Drawing.Color.White;
            appearance38.BackColor2 = System.Drawing.Color.White;
            appearance38.BackColorDisabled = System.Drawing.Color.White;
            appearance38.BackColorDisabled2 = System.Drawing.Color.White;
            appearance38.BorderColor = System.Drawing.Color.Black;
            appearance38.BorderColor3DBase = System.Drawing.Color.Black;
            this.grdReturn.DisplayLayout.Override.CellAppearance = appearance38;
            appearance39.BackColor = System.Drawing.Color.White;
            appearance39.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(231)))), ((int)(((byte)(230)))));
            appearance39.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance39.BorderAlpha = Infragistics.Win.Alpha.Transparent;
            appearance39.BorderColor = System.Drawing.Color.Gray;
            appearance39.BorderColor3DBase = System.Drawing.Color.Gray;
            appearance39.ImageBackgroundAlpha = Infragistics.Win.Alpha.Transparent;
            appearance39.ImageBackgroundStyle = Infragistics.Win.ImageBackgroundStyle.Centered;
            this.grdReturn.DisplayLayout.Override.CellButtonAppearance = appearance39;
            appearance40.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            appearance40.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.grdReturn.DisplayLayout.Override.EditCellAppearance = appearance40;
            appearance41.BorderColor = System.Drawing.Color.Gray;
            this.grdReturn.DisplayLayout.Override.FilteredInRowAppearance = appearance41;
            appearance42.BorderColor = System.Drawing.Color.Gray;
            this.grdReturn.DisplayLayout.Override.FilteredOutRowAppearance = appearance42;
            appearance43.BackColor = System.Drawing.Color.White;
            appearance43.BackColor2 = System.Drawing.Color.White;
            appearance43.BackColorDisabled = System.Drawing.Color.White;
            appearance43.BackColorDisabled2 = System.Drawing.Color.White;
            this.grdReturn.DisplayLayout.Override.FixedCellAppearance = appearance43;
            appearance44.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            appearance44.BackColor2 = System.Drawing.Color.Beige;
            this.grdReturn.DisplayLayout.Override.FixedHeaderAppearance = appearance44;
            appearance45.BackColor = System.Drawing.Color.White;
            appearance45.BackColor2 = System.Drawing.SystemColors.Control;
            appearance45.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance45.FontData.BoldAsString = "True";
            appearance45.ForeColor = System.Drawing.Color.Black;
            appearance45.TextHAlignAsString = "Left";
            appearance45.ThemedElementAlpha = Infragistics.Win.Alpha.Transparent;
            this.grdReturn.DisplayLayout.Override.HeaderAppearance = appearance45;
            appearance46.BorderColor = System.Drawing.Color.Gray;
            this.grdReturn.DisplayLayout.Override.RowAlternateAppearance = appearance46;
            appearance47.BackColor = System.Drawing.Color.White;
            appearance47.BackColor2 = System.Drawing.Color.White;
            appearance47.BackGradientStyle = Infragistics.Win.GradientStyle.None;
            appearance47.BackHatchStyle = Infragistics.Win.BackHatchStyle.None;
            appearance47.BorderColor = System.Drawing.Color.Gray;
            this.grdReturn.DisplayLayout.Override.RowAppearance = appearance47;
            appearance48.BorderColor = System.Drawing.Color.Gray;
            this.grdReturn.DisplayLayout.Override.RowPreviewAppearance = appearance48;
            appearance49.BackColor = System.Drawing.Color.White;
            appearance49.BackColor2 = System.Drawing.SystemColors.Control;
            appearance49.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance49.BorderColor = System.Drawing.Color.Gray;
            this.grdReturn.DisplayLayout.Override.RowSelectorAppearance = appearance49;
            this.grdReturn.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            this.grdReturn.DisplayLayout.Override.RowSelectorWidth = 12;
            this.grdReturn.DisplayLayout.Override.RowSpacingBefore = 2;
            appearance50.BackColor = System.Drawing.Color.Navy;
            appearance50.BackColorAlpha = Infragistics.Win.Alpha.Opaque;
            this.grdReturn.DisplayLayout.Override.SelectedCellAppearance = appearance50;
            appearance51.BackColor = System.Drawing.Color.Navy;
            appearance51.BackColorDisabled = System.Drawing.Color.Navy;
            appearance51.BackGradientStyle = Infragistics.Win.GradientStyle.None;
            appearance51.BackHatchStyle = Infragistics.Win.BackHatchStyle.None;
            appearance51.BorderColor = System.Drawing.Color.Gray;
            appearance51.ForeColor = System.Drawing.Color.Black;
            this.grdReturn.DisplayLayout.Override.SelectedRowAppearance = appearance51;
            this.grdReturn.DisplayLayout.Override.SelectTypeCell = Infragistics.Win.UltraWinGrid.SelectType.None;
            this.grdReturn.DisplayLayout.Override.SelectTypeCol = Infragistics.Win.UltraWinGrid.SelectType.None;
            this.grdReturn.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.Single;
            appearance52.BorderColor = System.Drawing.Color.Gray;
            this.grdReturn.DisplayLayout.Override.TemplateAddRowAppearance = appearance52;
            this.grdReturn.DisplayLayout.RowConnectorColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            this.grdReturn.DisplayLayout.RowConnectorStyle = Infragistics.Win.UltraWinGrid.RowConnectorStyle.Solid;
            appearance53.BackColor = System.Drawing.Color.White;
            appearance53.BackColor2 = System.Drawing.SystemColors.Control;
            appearance53.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            scrollBarLook2.Appearance = appearance53;
            appearance54.BackColor = System.Drawing.Color.White;
            appearance54.BackColor2 = System.Drawing.SystemColors.Control;
            appearance54.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance54.ThemedElementAlpha = Infragistics.Win.Alpha.Transparent;
            scrollBarLook2.ButtonAppearance = appearance54;
            appearance55.BackColor = System.Drawing.Color.White;
            appearance55.BackColor2 = System.Drawing.SystemColors.Control;
            scrollBarLook2.ThumbAppearance = appearance55;
            appearance56.BackColor = System.Drawing.Color.White;
            appearance56.BackColor2 = System.Drawing.Color.White;
            scrollBarLook2.TrackAppearance = appearance56;
            this.grdReturn.DisplayLayout.ScrollBarLook = scrollBarLook2;
            this.grdReturn.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.grdReturn.Location = new System.Drawing.Point(496, 80);
            this.grdReturn.Name = "grdReturn";
            this.grdReturn.Size = new System.Drawing.Size(485, 331);
            this.grdReturn.TabIndex = 1;
            this.grdReturn.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.grdReturn.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.grdReturn_InitializeLayout);
            this.grdReturn.BeforeRowExpanded += new Infragistics.Win.UltraWinGrid.CancelableRowEventHandler(this.grdReturn_BeforeRowExpanded);
            this.grdReturn.DoubleClick += new System.EventHandler(this.grdReturn_DoubleClick);
            this.grdReturn.MouseMove += new System.Windows.Forms.MouseEventHandler(this.grdReturn_MouseMove);
            // 
            // frmRptSSalesByUID
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(108)))), ((int)(((byte)(172)))));
            this.ClientSize = new System.Drawing.Size(984, 462);
            this.Controls.Add(this.grdReturn);
            this.Controls.Add(this.grdSale);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.Name = "frmRptSSalesByUID";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Sales Summary By User";
            this.Load += new System.EventHandler(this.frmViewTransaction_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmViewTransaction_KeyDown);
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.chkOnlyDiscountedTrans)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtpSaleEndDate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtpSaleStartDate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtUserID)).EndInit();
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdSale)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdReturn)).EndInit();
            this.ResumeLayout(false);

		}
		#endregion

		private void frmViewTransaction_Load(object sender, System.EventArgs e)
		{
			this.dtpSaleStartDate.Enter += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
			this.dtpSaleStartDate.Leave += new System.EventHandler(clsUIHelper.AfterExitEditMode);

			this.dtpSaleEndDate.Enter += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
			this.dtpSaleEndDate.Leave += new System.EventHandler(clsUIHelper.AfterExitEditMode);

			this.txtUserID.AfterEnterEditMode += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
			this.txtUserID.AfterExitEditMode += new System.EventHandler(clsUIHelper.AfterExitEditMode);

			this.Left=(frmMain.getInstance().Width-frmMain.getInstance().ultraExplorerBar1.Width-this.Width)/2;
			this.Top=(frmMain.getInstance().Height-this.Height)/2;
			
			clsUIHelper.setColorSchecme(this);
			this.dtpSaleEndDate.Value=DateTime.Now;
			this.dtpSaleStartDate.Value=DateTime.Now;

            #region Sprint-19 - 2155 21-Jan-2015 JY Added
            this.Location = new Point(45, 2); 
            clsUIHelper.SetAppearance(this.grdSale);
            clsUIHelper.SetReadonlyRow(this.grdSale);
            grdSale.DisplayLayout.Override.HeaderClickAction = HeaderClickAction.SortMulti;
            grdSale.DisplayLayout.AutoFitStyle = AutoFitStyle.ResizeAllColumns;
            clsUIHelper.SetAppearance(this.grdReturn);
            clsUIHelper.SetReadonlyRow(this.grdReturn);
            grdReturn.DisplayLayout.Override.HeaderClickAction = HeaderClickAction.SortMulti;
            grdReturn.DisplayLayout.AutoFitStyle = AutoFitStyle.ResizeAllColumns;
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
                else if (e.KeyData == System.Windows.Forms.Keys.F4) //Sprint-19 - 2155 23-Jan-2015 JY Added 
                    btnSearch_Click(sender, e);
			}
			catch(Exception exp)
			{
				clsUIHelper.ShowErrorMsg( exp.Message);
			}
		}

		private void PreviewReport(bool blnPrint, bool bCalledFromScheduler = false)   //PRIMEPOS-2485 02-Apr-2021 JY Added bCalledFromScheduler
        {
			try
			{
				this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
				rptSSalesByUID oRpt = new rptSSalesByUID();

				String strQuery;
                //Modified By Amit Date 3 june 2011
                //ORIGINAL
                //strQuery="select PT.UserID, " +
                //" case PT.TransType when 1 then (GrossTotal-TotalDiscAmount+TotalTaxAmount) else 0  end as [Total Sale], " +
                //" case PT.TransType when 2 then (GrossTotal-TotalDiscAmount+TotalTaxAmount) else 0 end as [Total Returns], " +
                //" '" + this.dtpSaleStartDate.Text + "' as StartDate, '" + this.dtpSaleEndDate.Text + "' as EndDate from POSTransaction PT " +
                //" where convert(datetime,TransDate,109) between convert(datetime, cast('" + this.dtpSaleStartDate.Text  + " 00:00:00' as datetime) ,113) and convert(datetime, cast('" + this.dtpSaleEndDate.Text  + " 23:59:59' as datetime) ,113) " ;

                //strQuery = "select PT.UserID, " +
                //" case PT.TransType when 1 then (GrossTotal) else 0  end as [Amount], " +
                //" case PT.TransType when 1 then (TotalDiscAmount) else 0  end as [Discount], " +
                //" case PT.TransType when 1 then (GrossTotal-TotalDiscAmount) else 0  end as [NetSale], " +
                //" case PT.TransType when 1 then (TotalTaxAmount) else 0  end as [Tax], " +
                //" case PT.TransType when 1 then (GrossTotal-TotalDiscAmount+TotalTaxAmount) else 0  end as [Total Sale], " +
                //" case PT.TransType when 2 then (GrossTotal) else 0  end as [RAmount], " +
                //" case PT.TransType when 2 then (TotalDiscAmount) else 0  end as [RDiscount], " +
                //" case PT.TransType when 2 then (GrossTotal-TotalDiscAmount) else 0  end as [RNetSale], " +
                //" case PT.TransType when 2 then (TotalTaxAmount) else 0  end as [RTax], " +
                //" case PT.TransType when 2 then (GrossTotal-TotalDiscAmount+TotalTaxAmount) else 0 end as [Total Returns], " +
                //" '" + this.dtpSaleStartDate.Text + "' as StartDate, '" + this.dtpSaleEndDate.Text + "' as EndDate from POSTransaction PT " +
                //" where convert(datetime,TransDate,109) between convert(datetime, cast('" + this.dtpSaleStartDate.Text + " 00:00:00' as datetime) ,113) and convert(datetime, cast('" + this.dtpSaleEndDate.Text + " 23:59:59' as datetime) ,113) ";

                //End

                //Sprint-19 - 2155 23-Jan-2015 JY commented
                ////Following Query Added By Krishna on 25 October 2011
                //strQuery = "Select  PT.UserID,  case PT.TransType when 1 then (PTD.ExtendedPrice) else 0  end as [Amount],  case PT.TransType when 1 then (PTD.Discount) else 0  end as [Discount]," +
                //            "case PT.TransType when 1 then (PTD.ExtendedPrice-PTD.Discount) else 0  end as [NetSale],  case PT.TransType when 1 then (PTD.TaxAmount) else 0  end as [Tax], " +
                //            "case PT.TransType when 1 then (PTD.ExtendedPrice-PTD.Discount+PTD.TaxAmount) else 0  end as [Total Sale],  " +
                //            "case PT.TransType when 2 then (PTD.ExtendedPrice) else 0  end as [RAmount],  case PT.TransType when 2 then (PTD.Discount) else 0  end as [RDiscount],  " +
                //            "case PT.TransType when 2 then (PTD.ExtendedPrice-PTD.Discount) else 0  end as [RNetSale],  case PT.TransType when 2 then (PTD.TaxAmount) else 0  end as [RTax],  " +
                //            "case PT.TransType when 2 then (PTD.ExtendedPrice-PTD.Discount+PTD.TaxAmount) else 0 end as [Total Returns], " +
                //            "'" + this.dtpSaleStartDate.Text + "' as StartDate, '" + this.dtpSaleEndDate.Text + "' as EndDate," +
                //            "(Select SUM(TotalDiscAmount) from POSTransaction where transid in(select PT.TransID from POSTransaction PT  " +
                //            "where convert(datetime,TransDate,109) between convert(datetime, cast('" + this.dtpSaleStartDate.Text + " 00:00:00' as datetime) ,113) "+
                //            "and convert(datetime, cast('" + this.dtpSaleEndDate.Text + " 23:59:59' as datetime) ,113) )) as GrandTotDisc," +
                //            "(Select SUM(subPTD.Discount) From POSTransactionDetail subPTD where subPTD.TransDetailID=PTD.TransdetailId   ) as TotLineDisc" +
                //            " From POSTransaction PT inner join POSTransactionDetail PTD on(PT.TransID=PTD.TransID)" +
                //            "where convert(datetime,TransDate,109) between convert(datetime, cast('" + this.dtpSaleStartDate.Text + " 00:00:00' as datetime) ,113) and convert(datetime, cast('" + this.dtpSaleEndDate.Text + " 23:59:59' as datetime) ,113)";
                ////Till here added by Krishna on 25 October 2011
                //if (this.txtUserID.Text.Trim()!="")
                //    strQuery+=" and PT.UserID='" + this.txtUserID.Text.Trim() + "' ";

                #region Sprint-19 - 2155 23-Jan-2015 JY 3. Redesign the report format and added more optimized query which returns user-wise summation of data from sql instead of calculating it in crystal report.
                string strUserId = string.Empty, strDisc = string.Empty;    //Sprint-21 - 1868 17-Jul-2015 JY Added strDisc
                if (this.txtUserID.Text.Trim() != "")
                    strUserId = " and PT.UserID='" + this.txtUserID.Text.Trim() + "' ";
                if (chkOnlyDiscountedTrans.Checked) //Sprint-21 - 1868 17-Jul-2015 JY Added condition to consider only discounted transactions
                    strDisc = " AND PT.TotalDiscAmount <> 0 ";

                //PRIMEPOS-3119 11-Aug-2022 JY modified query for TransFee
                strQuery = " SELECT S.UserID, S.sGrossSale, S.sLineDiscount, S.sNetSale, S.sTax, s.sTotalAmountCollected, S.sInvoiceDiscount, S.sTransFee, S.sGrandTotal, R.rGrossSale, R.rLineDiscount, R.rNetSale, R.rTax, R.rTotalAmountCollected, R.rInvoiceDiscount, R.rTransFee, R.rGrandTotal, " +
                        "'" + this.dtpSaleStartDate.Text + "' as StartDate, '" + this.dtpSaleEndDate.Text + "' as EndDate FROM " +
                        " (SELECT UserId, SUM(GrossSale) AS sGrossSale, SUM(LineDiscount) AS sLineDiscount, SUM(NetSale) AS sNetSale, SUM(Tax) AS sTax, SUM(TotalAmountCollected) AS sTotalAmountCollected, SUM(InvoiceDiscount) AS sInvoiceDiscount, SUM(TransFee) AS sTransFee, SUM(GrandTotal) AS sGrandTotal FROM " + 
                        " (SELECT PT.UserID, SUM(PTD.ExtendedPrice) AS GrossSale, SUM(PTD.Discount) AS LineDiscount, (SUM(PTD.ExtendedPrice)-SUM(PTD.Discount)) AS NetSale, SUM(PTD.TaxAmount) AS Tax, " +
                        " (SUM(PTD.ExtendedPrice)-SUM(PTD.Discount)+SUM(PTD.TaxAmount)) as TotalAmountCollected, ISNULL(PT.INVOICEDISCOUNT,0) AS InvoiceDiscount, ISNULL(PT.TotalTransFeeAmt,0) AS TransFee, (SUM(PTD.ExtendedPrice)-SUM(PTD.Discount)+SUM(PTD.TaxAmount)-ISNULL(pt.INVOICEDISCOUNT,0)+ISNULL(PT.TotalTransFeeAmt,0)) AS GrandTotal " + 
                        " From POSTransaction PT inner join POSTransactionDetail PTD on PT.TransID=PTD.TransID " + 
                        " where PT.TransType = 1 " + strUserId + 
                        " AND convert(datetime,TransDate,109) between convert(datetime, cast('" + this.dtpSaleStartDate.Text + " 00:00:00' as datetime) ,113) and convert(datetime, cast('" + this.dtpSaleEndDate.Text + " 23:59:59' as datetime) ,113) " +
                        strDisc +   //Sprint-21 - 1868 17-Jul-2015 JY Added condition to consider only discounted transactions
                        " group by PT.UserID, PT.TransId, PT.INVOICEDISCOUNT, pt.TotalTransFeeAmt) Sale " +
                        " GROUP BY UserId) S " +
                        " LEFT JOIN " +
                        " (SELECT UserId, SUM(GrossSale) AS rGrossSale, SUM(LineDiscount) AS rLineDiscount, SUM(NetSale) AS rNetSale, SUM(Tax) AS rTax, SUM(TotalAmountCollected) AS rTotalAmountCollected, SUM(InvoiceDiscount) AS rInvoiceDiscount, SUM(TransFee) AS rTransFee, SUM(GrandTotal) AS rGrandTotal FROM " + 
                        " (SELECT PT.UserID, SUM(PTD.ExtendedPrice) AS GrossSale, SUM(PTD.Discount) AS LineDiscount, (SUM(PTD.ExtendedPrice)-SUM(PTD.Discount)) AS NetSale, SUM(PTD.TaxAmount) AS Tax, " +
                        " -(SUM(ABS(PTD.ExtendedPrice))-SUM(ABS(PTD.Discount))+SUM(ABS(PTD.TaxAmount))) as TotalAmountCollected, ISNULL(PT.INVOICEDISCOUNT,0) AS InvoiceDiscount, ISNULL(PT.TotalTransFeeAmt,0) AS TransFee, -(SUM(ABS(PTD.ExtendedPrice))-SUM(ABS(PTD.Discount))+SUM(ABS(PTD.TaxAmount))-ABS(ISNULL(PT.INVOICEDISCOUNT,0))-ABS(ISNULL(PT.TotalTransFeeAmt,0))) AS GrandTotal " + 
                        " From POSTransaction PT inner join POSTransactionDetail PTD on PT.TransID=PTD.TransID " + 
                        " where PT.TransType = 2 " + strUserId + 
                        " AND convert(datetime,TransDate,109) between convert(datetime, cast('" + this.dtpSaleStartDate.Text + " 00:00:00' as datetime) ,113) and convert(datetime, cast('" + this.dtpSaleEndDate.Text + " 23:59:59' as datetime) ,113) " +
                        strDisc +   //Sprint-21 - 1868 17-Jul-2015 JY Added condition to consider only discounted transactions
                        " group by PT.UserID, PT.TransId, PT.INVOICEDISCOUNT, pt.TotalTransFeeAmt) Ret " +
                        " GROUP BY UserId) R ON S.UserId = R.UserId Order By S.UserID";
                #endregion

                /*Search oSearch=new Search();
				DataSet ds = oSearch.Search(strQuery);
				oRpt.SetDataSource(ds);
				frmReportViewer oViewer=new frmReportViewer();
				oViewer.rvReportViewer.ReportSource=oRpt;
				oViewer.MdiParent=frmMain.getInstance();
				oViewer.WindowState=FormWindowState.Maximized;
				oViewer.Show();
				*/
                clsReports.Preview(blnPrint, strQuery, oRpt, bCalledFromScheduler); //PRIMEPOS-2485 02-Apr-2021 JY Added bCalledFromScheduler
                oReport = oRpt; //PRIMEPOS-2485 02-Apr-2021 JY Added
                this.Cursor = System.Windows.Forms.Cursors.Default;
			}
			catch(Exception exp)
			{
				this.Cursor = System.Windows.Forms.Cursors.Default;
				clsUIHelper.ShowErrorMsg(exp.Message);
			}
		}

		private void btnPrint_Click(object sender, System.EventArgs e)
		{
			PreviewReport(true);
		}

		private void btnView_Click_1(object sender, System.EventArgs e)
		{
			PreviewReport(false);
		}

        #region Sprint-19 - 2155 19-Jan-2015 JY Added 
        private void btnSearch_Click(object sender, EventArgs e)
        {
            String strSalesDetails, strReturnsDetails;
            try
            {
                dsSales = new DataSet();
                DataSet dsSalesTmp = new DataSet();
                dsReturns = new DataSet();
                DataSet dsReturnsTmp = new DataSet();

                GenerateSQL(out strSalesDetails, out strReturnsDetails);

                #region Sales
                dsSalesTmp.Tables.Add(oSearchSvr.Search(strSalesDetails).Tables[0].Copy());

                var SalesSummary = from Sales in dsSalesTmp.Tables[0].AsEnumerable()
                                   group Sales by Sales.Field<string>("UserId") into userg
                                   select new
                                   {
                                       UserId = userg.Key,
                                       GrossSale = userg.Sum(x => x.Field<decimal>("Gross Sale")),
                                       LineDiscount = userg.Sum(x => x.Field<decimal>("Line Discount")),
                                       NetSale = userg.Sum(x => x.Field<decimal>("Net Sale")),
                                       Tax = userg.Sum(x => x.Field<decimal>("Tax")),
                                       TotalAmountCollected = userg.Sum(x => x.Field<decimal>("Total Amount Collected")),
                                       InvoiceDiscount = userg.Sum(x => x.Field<decimal>("Invoice Discount")),
                                       TransFee = userg.Sum(x => x.Field<decimal>("Trans Fee")),    //PRIMEPOS-3119 11-Aug-2022 JY Added
                                       GrandTotal = userg.Sum(x => x.Field<decimal>("Grand Total"))
                                   };

                DataTable dt = new DataTable();
                dt.Columns.Add("UserId", typeof(System.String));
                dt.Columns.Add("Gross Sale", typeof(System.Decimal));
                dt.Columns.Add("Line Discount", typeof(System.Decimal));
                dt.Columns.Add("Net Sale", typeof(System.Decimal));
                dt.Columns.Add("Tax", typeof(System.Decimal));
                dt.Columns.Add("Total Amount Collected", typeof(System.Decimal));
                dt.Columns.Add("Invoice Discount", typeof(System.Decimal));
                dt.Columns.Add("Trans Fee", typeof(System.Decimal));    //PRIMEPOS-3119 11-Aug-2022 JY Added
                dt.Columns.Add("Grand Total", typeof(System.Decimal));
                DataRow dr;

                foreach (var Summary in SalesSummary)
                {
                    dr = dt.NewRow();
                    dr["UserId"] = Summary.UserId;
                    dr["Gross Sale"] = Summary.GrossSale;
                    dr["Line Discount"] = Summary.LineDiscount;
                    dr["Net Sale"] = Summary.NetSale;
                    dr["Tax"] = Summary.Tax;
                    dr["Total Amount Collected"] = Summary.TotalAmountCollected;
                    dr["Invoice Discount"] = Summary.InvoiceDiscount;
                    dr["Trans Fee"] = Summary.TransFee; //PRIMEPOS-3119 11-Aug-2022 JY Added
                    dr["Grand Total"] = Summary.GrandTotal;

                    dt.Rows.Add(dr);
                }

                dsSales.Tables.Add(dt);   //Added table for summary
                dsSales.Tables[0].TableName = "Summary";
                dsSales.Tables.Add(dsSalesTmp.Tables[0].Copy());
                dsSales.Tables[1].TableName = "Details";

                dsSales.Relations.Add("Details", dsSales.Tables[0].Columns["UserId"], dsSales.Tables[1].Columns["UserId"]);

                grdSale.DataSource = dsSales;
                grdSale.DisplayLayout.Bands[0].HeaderVisible = true;
                grdSale.DisplayLayout.Bands[0].Header.Appearance.FontData.SizeInPoints = 10;
                grdSale.DisplayLayout.Bands[0].Header.Appearance.TextHAlign = Infragistics.Win.HAlign.Center;
                grdSale.DisplayLayout.Bands[0].Header.Caption = "Sales Summary";
                grdSale.DisplayLayout.Bands[0].Override.WrapHeaderText = Infragistics.Win.DefaultableBoolean.True;
                grdSale.DisplayLayout.Bands[0].Columns[4].Width = 65;   //tax


                grdSale.DisplayLayout.Bands[1].HeaderVisible = true;
                grdSale.DisplayLayout.Bands[1].Header.Caption = "Sales Detail";
                grdSale.DisplayLayout.Bands[1].Header.Appearance.FontData.SizeInPoints = 10;
                grdSale.DisplayLayout.Bands[1].Header.Appearance.TextHAlign = Infragistics.Win.HAlign.Center;
                grdSale.DisplayLayout.Bands[1].Columns["UserId"].Hidden = true;
                grdSale.DisplayLayout.Bands[1].Expandable = true;
                grdSale.DisplayLayout.Bands[1].Override.WrapHeaderText = Infragistics.Win.DefaultableBoolean.True;
                grdSale.DisplayLayout.Bands[1].Columns[6].Width = 65;   //tax

                resizeColumns(grdSale);
                grdSale.PerformAction(UltraGridAction.FirstRowInGrid);
                grdSale.Refresh();
                #endregion

                #region Returns
                dsReturnsTmp.Tables.Add(oSearchSvr.Search(strReturnsDetails).Tables[0].Copy());

                var ReturnSummary = from Return in dsReturnsTmp.Tables[0].AsEnumerable()
                                    group Return by Return.Field<string>("UserId") into userg
                                    select new
                                    {
                                        UserId = userg.Key,
                                        GrossSale = userg.Sum(x => x.Field<decimal>("Gross Sale")),
                                        LineDiscount = userg.Sum(x => x.Field<decimal>("Line Discount")),
                                        NetSale = userg.Sum(x => x.Field<decimal>("Net Sale")),
                                        Tax = userg.Sum(x => x.Field<decimal>("Tax")),
                                        TotalAmountCollected = userg.Sum(x => x.Field<decimal>("Total Amount Collected")),
                                        InvoiceDiscount = userg.Sum(x => x.Field<decimal>("Invoice Discount")),
                                        TransFee = userg.Sum(x => x.Field<decimal>("Trans Fee")),    //PRIMEPOS-3119 11-Aug-2022 JY Added
                                        GrandTotal = userg.Sum(x => x.Field<decimal>("Grand Total"))
                                    };

                DataTable dt1 = dt.Clone();
                foreach (var Summary in ReturnSummary)
                {
                    dr = dt1.NewRow();
                    dr["UserId"] = Summary.UserId;
                    dr["Gross Sale"] = Summary.GrossSale;
                    dr["Line Discount"] = Summary.LineDiscount;
                    dr["Net Sale"] = Summary.NetSale;
                    dr["Tax"] = Summary.Tax;
                    dr["Total Amount Collected"] = Summary.TotalAmountCollected;
                    dr["Invoice Discount"] = Summary.InvoiceDiscount;
                    dr["Trans Fee"] = Summary.TransFee; //PRIMEPOS-3119 11-Aug-2022 JY Added
                    dr["Grand Total"] = Summary.GrandTotal;

                    dt1.Rows.Add(dr);
                }

                dsReturns.Tables.Add(dt1);   //Added table for summary
                dsReturns.Tables[0].TableName = "Summary";
                dsReturns.Tables.Add(dsReturnsTmp.Tables[0].Copy());
                dsReturns.Tables[1].TableName = "Details";

                dsReturns.Relations.Add("Details", dsReturns.Tables[0].Columns["UserId"], dsReturns.Tables[1].Columns["UserId"]);

                grdReturn.DataSource = dsReturns;
                grdReturn.DisplayLayout.Bands[0].HeaderVisible = true;
                grdReturn.DisplayLayout.Bands[0].Header.Appearance.FontData.SizeInPoints = 10;
                grdReturn.DisplayLayout.Bands[0].Header.Appearance.TextHAlign = Infragistics.Win.HAlign.Center;
                grdReturn.DisplayLayout.Bands[0].Header.Caption = "Return Summary";
                grdReturn.DisplayLayout.Bands[0].Override.WrapHeaderText = Infragistics.Win.DefaultableBoolean.True;
                grdReturn.DisplayLayout.Bands[0].Columns[4].Width = 65;   //tax

                grdReturn.DisplayLayout.Bands[1].HeaderVisible = true;
                grdReturn.DisplayLayout.Bands[1].Header.Caption = "Return Detail";
                grdReturn.DisplayLayout.Bands[1].Header.Appearance.FontData.SizeInPoints = 10;
                grdReturn.DisplayLayout.Bands[1].Header.Appearance.TextHAlign = Infragistics.Win.HAlign.Center;
                grdReturn.DisplayLayout.Bands[1].Columns["UserId"].Hidden = true;
                grdReturn.DisplayLayout.Bands[1].Expandable = true;
                grdReturn.DisplayLayout.Bands[1].Override.WrapHeaderText = Infragistics.Win.DefaultableBoolean.True;
                grdReturn.DisplayLayout.Bands[1].Columns[6].Width = 65;   //tax

                resizeColumns(grdReturn);
                grdReturn.PerformAction(UltraGridAction.FirstRowInGrid);
                grdReturn.Refresh();
                #endregion
            }
            catch (Exception exp) 
            { 
                clsUIHelper.ShowErrorMsg(exp.Message); 
            }
        }

        private void GenerateSQL(out string strSalesDetails, out string strReturnsDetails)
        {            
            string strUserId = string.Empty, strDisc = string.Empty;    //Sprint-21 - 1868 17-Jul-2015 JY Added strDisc
            if (this.txtUserID.Text.Trim() != "")
                strUserId = " and PT.UserID='" + this.txtUserID.Text.Trim() + "' ";
            if (chkOnlyDiscountedTrans.Checked) //Sprint-21 - 1868 17-Jul-2015 JY Added condition to consider only discounted transactions
                strDisc = " AND PT.TotalDiscAmount <> 0 ";

            strSalesDetails = "SELECT PT.TransId, PT.TransDate, UPPER(LTRIM(RTRIM(PT.UserId))) AS UserID, SUM(PTD.ExtendedPrice) AS [Gross Sale], SUM(PTD.Discount) AS [Line Discount], (SUM(PTD.ExtendedPrice)-SUM(PTD.Discount)) AS [Net Sale], SUM(PTD.TaxAmount) AS Tax, " +
                    " (SUM(PTD.ExtendedPrice)-SUM(PTD.Discount)+SUM(PTD.TaxAmount)) as [Total Amount Collected], ISNULL(PT.INVOICEDISCOUNT,0) AS [Invoice Discount], ISNULL(PT.TotalTransFeeAmt,0) AS [Trans Fee], (SUM(PTD.ExtendedPrice)-SUM(PTD.Discount)+SUM(PTD.TaxAmount)-ISNULL(PT.INVOICEDISCOUNT,0)+ISNULL(PT.TotalTransFeeAmt,0)) AS [Grand Total] " +    //PRIMEPOS-3119 11-Aug-2022 JY Added TotalTransFeeAmt
                    " From POSTransaction PT inner join POSTransactionDetail PTD on PT.TransID=PTD.TransID " +
                    " where PT.TransType = 1 " + strUserId + " AND " +
                    " convert(datetime,TransDate,109) between convert(datetime, cast('" + this.dtpSaleStartDate.Text + " 00:00:00' as datetime) ,113) and convert(datetime, cast('" + this.dtpSaleEndDate.Text + " 23:59:59' as datetime) ,113) " +
                    strDisc +   //Sprint-21 - 1868 17-Jul-2015 JY Added condition to consider only discounted transactions
                    " group by PT.TransId, PT.UserID, PT.TransDate, PT.INVOICEDISCOUNT, PT.TotalTransFeeAmt " + 
                    " order by PT.TransId desc";

            strReturnsDetails = "SELECT PT.TransId, PT.TransDate, UPPER(LTRIM(RTRIM(PT.UserId))) AS UserID, SUM(PTD.ExtendedPrice) AS [Gross Sale], SUM(PTD.Discount) AS [Line Discount], (SUM(PTD.ExtendedPrice)-SUM(PTD.Discount)) AS [Net Sale], SUM(PTD.TaxAmount) AS Tax, " +
                    " -(SUM(ABS(PTD.ExtendedPrice))-SUM(ABS(PTD.Discount))+SUM(ABS(PTD.TaxAmount))) as [Total Amount Collected], ISNULL(PT.INVOICEDISCOUNT,0) AS [Invoice Discount], ISNULL(PT.TotalTransFeeAmt,0) AS [Trans Fee], -(SUM(ABS(PTD.ExtendedPrice))-SUM(ABS(PTD.Discount))+SUM(ABS(PTD.TaxAmount))-ABS(ISNULL(PT.INVOICEDISCOUNT,0))-ABS(ISNULL(PT.TotalTransFeeAmt,0))) AS [Grand Total] " +    //PRIMEPOS-3119 11-Aug-2022 JY Added TotalTransFeeAmt
                    " From POSTransaction PT inner join POSTransactionDetail PTD on PT.TransID=PTD.TransID " +
                    " where PT.TransType = 2 " + strUserId + " AND " +
                    " convert(datetime,TransDate,109) between convert(datetime, cast('" + this.dtpSaleStartDate.Text + " 00:00:00' as datetime) ,113) and convert(datetime, cast('" + this.dtpSaleEndDate.Text + " 23:59:59' as datetime) ,113) " +
                    strDisc +   //Sprint-21 - 1868 17-Jul-2015 JY Added condition to consider only discounted transactions
                    " group by PT.TransId, PT.UserID, PT.TransDate, PT.INVOICEDISCOUNT, PT.TotalTransFeeAmt " + 
                    " order by PT.TransId desc";
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
        #endregion

        private void grdSale_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            FormatGridColumn(grdSale, 2);
        }

        private void grdReturn_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            FormatGridColumn(grdReturn, 2);
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

        private void grdSale_BeforeRowExpanded(object sender, CancelableRowEventArgs e)
        {
            foreach (Infragistics.Win.UltraWinGrid.UltraGridRow orow in grdSale.DisplayLayout.Rows)
            {
                if (orow.Expanded == true)
                    orow.CollapseAll();
            }
            e.Row.Activate();
        }

        private void grdReturn_BeforeRowExpanded(object sender, CancelableRowEventArgs e)
        {
            foreach (Infragistics.Win.UltraWinGrid.UltraGridRow orow in grdReturn.DisplayLayout.Rows)
            {
                if (orow.Expanded == true)
                    orow.CollapseAll();
            }
            e.Row.Activate();
        }

        private void grdSale_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                Point point = System.Windows.Forms.Cursor.Position;
                point = this.grdSale.PointToClient(point);
                Infragistics.Win.UIElement oUI = this.grdSale.DisplayLayout.UIElement.ElementFromPoint(point);
                if (oUI == null)
                    return;

                while (oUI != null)
                {
                    if (oUI.GetType() == typeof(Infragistics.Win.UltraWinGrid.RowUIElement))
                    {
                        Infragistics.Win.UltraWinGrid.RowUIElement oRowUI = oUI as Infragistics.Win.UltraWinGrid.RowUIElement;
                        foreach (Infragistics.Win.UltraWinGrid.UltraGridRow orow in grdSale.DisplayLayout.Rows)
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

        private void grdReturn_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                Point point = System.Windows.Forms.Cursor.Position;
                point = this.grdReturn.PointToClient(point);
                Infragistics.Win.UIElement oUI = this.grdReturn.DisplayLayout.UIElement.ElementFromPoint(point);
                if (oUI == null)
                    return;

                while (oUI != null)
                {
                    if (oUI.GetType() == typeof(Infragistics.Win.UltraWinGrid.RowUIElement))
                    {
                        Infragistics.Win.UltraWinGrid.RowUIElement oRowUI = oUI as Infragistics.Win.UltraWinGrid.RowUIElement;
                        foreach (Infragistics.Win.UltraWinGrid.UltraGridRow orow in grdReturn.DisplayLayout.Rows)
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

        private void grdSale_MouseMove(object sender, MouseEventArgs e)
        {
            this.CurrentX = e.X;
            this.CurrentY = e.Y;
        }

        private void grdReturn_MouseMove(object sender, MouseEventArgs e)
        {
            this.CurrentX = e.X;
            this.CurrentY = e.Y;
        }

        #region PRIMEPOS-2485 02-Apr-2021 JY Added
        public bool bSendPrint = true;
        private ReportClass oReport = new ReportClass();
        public usrDateRangeParams customControl;
        private const string ReportName = "SalesSummaryByUser";

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
