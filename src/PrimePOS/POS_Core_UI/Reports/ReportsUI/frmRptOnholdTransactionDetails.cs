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
using Infragistics.Win.UltraWinGrid;
//using POS.UI;
using POS_Core_UI.Reports.Reports;
using System.Data;
using POS_Core.DataAccess;
//using POS_Core.DataAccess;

namespace POS_Core_UI.Reports.ReportsUI
{
	/// <summary>
	/// Summary description for frmViewTransaction.
	/// </summary>
	public class frmRptOnholdTransactionDetails : System.Windows.Forms.Form
	{
		private System.Windows.Forms.GroupBox groupBox1;
        private Infragistics.Win.Misc.UltraLabel ultraLabel12;
		private Infragistics.Win.Misc.UltraLabel ultraLabel21;
		private Infragistics.Win.UltraWinEditors.UltraTextEditor txtUserID;
		private Infragistics.Win.UltraWinEditors.UltraTextEditor txtStationID;
		private Infragistics.Win.Misc.UltraLabel ultraLabel1;
		private Infragistics.Win.Misc.UltraLabel ultraLabel20;
		private Infragistics.Win.Misc.UltraLabel ultraLabel19;
		private Infragistics.Win.UltraWinSchedule.UltraCalendarCombo dtpSaleEndDate;
		private Infragistics.Win.UltraWinSchedule.UltraCalendarCombo dtpSaleStartDate;
		private Infragistics.Win.UltraWinEditors.UltraComboEditor cboTransType;
		private Infragistics.Win.Misc.UltraLabel ultraLabel2;
		private System.Windows.Forms.GroupBox groupBox2;
		private Infragistics.Win.Misc.UltraButton btnPrint;
		private Infragistics.Win.Misc.UltraButton btnView;
		private Infragistics.Win.Misc.UltraButton btnClose;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor txtItemCode;
        private UltraGrid grdSearch;
        private Infragistics.Win.Misc.UltraButton btnSearch;
        private IContainer components;        
        private int CurrentX;
        private int CurrentY;
        private Infragistics.Win.Misc.UltraLabel lblOnHoldType;
        private UltraComboEditor cboOnHoldType;
        private Infragistics.Win.Misc.UltraLabel lblOnHoldTrans;
        private UltraComboEditor cboOnHoldTrans;        

		public frmRptOnholdTransactionDetails()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

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
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.ValueListItem valueListItem1 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem2 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem3 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.ValueListItem valueListItem4 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem5 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.UltraWinEditors.EditorButton editorButton1 = new Infragistics.Win.UltraWinEditors.EditorButton();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.ValueListItem valueListItem6 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem7 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem8 = new Infragistics.Win.ValueListItem();
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
            Infragistics.Win.Appearance appearance13 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance14 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand1 = new Infragistics.Win.UltraWinGrid.UltraGridBand("Band 0", -1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn1 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Column Name");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn2 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Type");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn3 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Criteria Value");
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
            Infragistics.Win.Appearance appearance32 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance33 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinScrollBar.ScrollBarLook scrollBarLook1 = new Infragistics.Win.UltraWinScrollBar.ScrollBarLook();
            Infragistics.Win.Appearance appearance34 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance35 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance36 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance37 = new Infragistics.Win.Appearance();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lblOnHoldType = new Infragistics.Win.Misc.UltraLabel();
            this.cboOnHoldType = new Infragistics.Win.UltraWinEditors.UltraComboEditor();
            this.lblOnHoldTrans = new Infragistics.Win.Misc.UltraLabel();
            this.cboOnHoldTrans = new Infragistics.Win.UltraWinEditors.UltraComboEditor();
            this.txtItemCode = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.ultraLabel2 = new Infragistics.Win.Misc.UltraLabel();
            this.cboTransType = new Infragistics.Win.UltraWinEditors.UltraComboEditor();
            this.ultraLabel20 = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabel19 = new Infragistics.Win.Misc.UltraLabel();
            this.dtpSaleEndDate = new Infragistics.Win.UltraWinSchedule.UltraCalendarCombo();
            this.dtpSaleStartDate = new Infragistics.Win.UltraWinSchedule.UltraCalendarCombo();
            this.ultraLabel1 = new Infragistics.Win.Misc.UltraLabel();
            this.txtStationID = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.ultraLabel21 = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabel12 = new Infragistics.Win.Misc.UltraLabel();
            this.txtUserID = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.btnSearch = new Infragistics.Win.Misc.UltraButton();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnPrint = new Infragistics.Win.Misc.UltraButton();
            this.btnClose = new Infragistics.Win.Misc.UltraButton();
            this.btnView = new Infragistics.Win.Misc.UltraButton();
            this.grdSearch = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cboOnHoldType)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboOnHoldTrans)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtItemCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboTransType)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtpSaleEndDate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtpSaleStartDate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtStationID)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtUserID)).BeginInit();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdSearch)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.groupBox1.Controls.Add(this.lblOnHoldType);
            this.groupBox1.Controls.Add(this.cboOnHoldType);
            this.groupBox1.Controls.Add(this.lblOnHoldTrans);
            this.groupBox1.Controls.Add(this.cboOnHoldTrans);
            this.groupBox1.Controls.Add(this.txtItemCode);
            this.groupBox1.Controls.Add(this.ultraLabel2);
            this.groupBox1.Controls.Add(this.cboTransType);
            this.groupBox1.Controls.Add(this.ultraLabel20);
            this.groupBox1.Controls.Add(this.ultraLabel19);
            this.groupBox1.Controls.Add(this.dtpSaleEndDate);
            this.groupBox1.Controls.Add(this.dtpSaleStartDate);
            this.groupBox1.Controls.Add(this.ultraLabel1);
            this.groupBox1.Controls.Add(this.txtStationID);
            this.groupBox1.Controls.Add(this.ultraLabel21);
            this.groupBox1.Controls.Add(this.ultraLabel12);
            this.groupBox1.Controls.Add(this.txtUserID);
            this.groupBox1.Controls.Add(this.btnSearch);
            this.groupBox1.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.4F);
            this.groupBox1.Location = new System.Drawing.Point(10, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(994, 100);
            this.groupBox1.TabIndex = 27;
            this.groupBox1.TabStop = false;
            // 
            // lblOnHoldType
            // 
            appearance1.ForeColor = System.Drawing.Color.Black;
            this.lblOnHoldType.Appearance = appearance1;
            this.lblOnHoldType.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.4F);
            this.lblOnHoldType.Location = new System.Drawing.Point(537, 72);
            this.lblOnHoldType.Name = "lblOnHoldType";
            this.lblOnHoldType.Size = new System.Drawing.Size(97, 16);
            this.lblOnHoldType.TabIndex = 29;
            this.lblOnHoldType.Text = "On-hold Type";
            // 
            // cboOnHoldType
            // 
            this.cboOnHoldType.AutoSize = false;
            this.cboOnHoldType.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.cboOnHoldType.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Office2003ToolbarButton;
            this.cboOnHoldType.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.4F);
            valueListItem1.DataValue = "0";
            valueListItem1.DisplayText = "All";
            valueListItem2.DataValue = "1";
            valueListItem2.DisplayText = "Delivery";
            valueListItem3.DataValue = "2";
            valueListItem3.DisplayText = "Non-Delivery";
            this.cboOnHoldType.Items.AddRange(new Infragistics.Win.ValueListItem[] {
            valueListItem1,
            valueListItem2,
            valueListItem3});
            this.cboOnHoldType.LimitToList = true;
            this.cboOnHoldType.Location = new System.Drawing.Point(639, 70);
            this.cboOnHoldType.Name = "cboOnHoldType";
            this.cboOnHoldType.Size = new System.Drawing.Size(144, 20);
            this.cboOnHoldType.TabIndex = 7;
            // 
            // lblOnHoldTrans
            // 
            appearance2.ForeColor = System.Drawing.Color.Black;
            this.lblOnHoldTrans.Appearance = appearance2;
            this.lblOnHoldTrans.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.4F);
            this.lblOnHoldTrans.Location = new System.Drawing.Point(537, 47);
            this.lblOnHoldTrans.Name = "lblOnHoldTrans";
            this.lblOnHoldTrans.Size = new System.Drawing.Size(97, 16);
            this.lblOnHoldTrans.TabIndex = 27;
            this.lblOnHoldTrans.Text = "On-hold Trans";
            // 
            // cboOnHoldTrans
            // 
            this.cboOnHoldTrans.AutoSize = false;
            this.cboOnHoldTrans.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.cboOnHoldTrans.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Office2003ToolbarButton;
            this.cboOnHoldTrans.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.4F);
            valueListItem4.DataValue = "0";
            valueListItem4.DisplayText = "On-Hold";
            valueListItem5.DataValue = "1";
            valueListItem5.DisplayText = "Processed";
            this.cboOnHoldTrans.Items.AddRange(new Infragistics.Win.ValueListItem[] {
            valueListItem4,
            valueListItem5});
            this.cboOnHoldTrans.LimitToList = true;
            this.cboOnHoldTrans.Location = new System.Drawing.Point(639, 45);
            this.cboOnHoldTrans.Name = "cboOnHoldTrans";
            this.cboOnHoldTrans.Size = new System.Drawing.Size(144, 20);
            this.cboOnHoldTrans.TabIndex = 6;
            // 
            // txtItemCode
            // 
            this.txtItemCode.AutoSize = false;
            this.txtItemCode.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
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
            this.txtItemCode.ButtonsRight.Add(editorButton1);
            this.txtItemCode.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.4F);
            this.txtItemCode.Location = new System.Drawing.Point(387, 70);
            this.txtItemCode.Name = "txtItemCode";
            this.txtItemCode.Size = new System.Drawing.Size(144, 20);
            this.txtItemCode.TabIndex = 4;
            this.txtItemCode.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.txtItemCode.EditorButtonClick += new Infragistics.Win.UltraWinEditors.EditorButtonEventHandler(this.txtItemCode_EditorButtonClick);
            // 
            // ultraLabel2
            // 
            appearance4.ForeColor = System.Drawing.Color.Black;
            this.ultraLabel2.Appearance = appearance4;
            this.ultraLabel2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.4F);
            this.ultraLabel2.Location = new System.Drawing.Point(537, 22);
            this.ultraLabel2.Name = "ultraLabel2";
            this.ultraLabel2.Size = new System.Drawing.Size(97, 16);
            this.ultraLabel2.TabIndex = 23;
            this.ultraLabel2.Text = "Trans Type";
            // 
            // cboTransType
            // 
            this.cboTransType.AutoSize = false;
            this.cboTransType.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.cboTransType.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Office2003ToolbarButton;
            this.cboTransType.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.4F);
            valueListItem6.DataValue = "0";
            valueListItem6.DisplayText = "All";
            valueListItem7.DataValue = "1";
            valueListItem7.DisplayText = "Sales";
            valueListItem8.DataValue = "2";
            valueListItem8.DisplayText = "Returns";
            this.cboTransType.Items.AddRange(new Infragistics.Win.ValueListItem[] {
            valueListItem6,
            valueListItem7,
            valueListItem8});
            this.cboTransType.LimitToList = true;
            this.cboTransType.Location = new System.Drawing.Point(639, 20);
            this.cboTransType.Name = "cboTransType";
            this.cboTransType.Size = new System.Drawing.Size(144, 20);
            this.cboTransType.TabIndex = 5;
            this.cboTransType.KeyDown += new System.Windows.Forms.KeyEventHandler(this.cboTransType_KeyDown);
            this.cboTransType.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.cboTransType_KeyPress);
            // 
            // ultraLabel20
            // 
            appearance5.ForeColor = System.Drawing.Color.Black;
            this.ultraLabel20.Appearance = appearance5;
            this.ultraLabel20.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.4F);
            this.ultraLabel20.Location = new System.Drawing.Point(9, 22);
            this.ultraLabel20.Name = "ultraLabel20";
            this.ultraLabel20.Size = new System.Drawing.Size(67, 16);
            this.ultraLabel20.TabIndex = 20;
            this.ultraLabel20.Text = "Start Date";
            // 
            // ultraLabel19
            // 
            appearance6.ForeColor = System.Drawing.Color.Black;
            this.ultraLabel19.Appearance = appearance6;
            this.ultraLabel19.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.4F);
            this.ultraLabel19.Location = new System.Drawing.Point(9, 47);
            this.ultraLabel19.Name = "ultraLabel19";
            this.ultraLabel19.Size = new System.Drawing.Size(67, 16);
            this.ultraLabel19.TabIndex = 22;
            this.ultraLabel19.Text = "End Date";
            // 
            // dtpSaleEndDate
            // 
            this.dtpSaleEndDate.AllowNull = false;
            this.dtpSaleEndDate.AutoSize = false;
            this.dtpSaleEndDate.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Office2003ToolbarButton;
            this.dtpSaleEndDate.DateButtons.Add(dateButton1);
            this.dtpSaleEndDate.Location = new System.Drawing.Point(81, 45);
            this.dtpSaleEndDate.Name = "dtpSaleEndDate";
            this.dtpSaleEndDate.NonAutoSizeHeight = 10;
            this.dtpSaleEndDate.Size = new System.Drawing.Size(144, 20);
            this.dtpSaleEndDate.TabIndex = 1;
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
            this.dtpSaleStartDate.Location = new System.Drawing.Point(80, 20);
            this.dtpSaleStartDate.Name = "dtpSaleStartDate";
            this.dtpSaleStartDate.NonAutoSizeHeight = 10;
            this.dtpSaleStartDate.Size = new System.Drawing.Size(144, 20);
            this.dtpSaleStartDate.TabIndex = 0;
            this.dtpSaleStartDate.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.dtpSaleStartDate.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.dtpSaleStartDate.Value = new System.DateTime(2004, 5, 25, 0, 0, 0, 0);
            // 
            // ultraLabel1
            // 
            appearance7.ForeColor = System.Drawing.Color.Black;
            this.ultraLabel1.Appearance = appearance7;
            this.ultraLabel1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.4F);
            this.ultraLabel1.Location = new System.Drawing.Point(235, 72);
            this.ultraLabel1.Name = "ultraLabel1";
            this.ultraLabel1.Size = new System.Drawing.Size(147, 16);
            this.ultraLabel1.TabIndex = 19;
            this.ultraLabel1.Text = "Item Code <Blank = All>";
            // 
            // txtStationID
            // 
            this.txtStationID.AutoSize = false;
            this.txtStationID.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.txtStationID.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.4F);
            this.txtStationID.Location = new System.Drawing.Point(387, 45);
            this.txtStationID.MaxLength = 20;
            this.txtStationID.Name = "txtStationID";
            this.txtStationID.Size = new System.Drawing.Size(144, 20);
            this.txtStationID.TabIndex = 3;
            this.txtStationID.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // ultraLabel21
            // 
            appearance8.ForeColor = System.Drawing.Color.Black;
            this.ultraLabel21.Appearance = appearance8;
            this.ultraLabel21.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.4F);
            this.ultraLabel21.Location = new System.Drawing.Point(235, 47);
            this.ultraLabel21.Name = "ultraLabel21";
            this.ultraLabel21.Size = new System.Drawing.Size(147, 16);
            this.ultraLabel21.TabIndex = 17;
            this.ultraLabel21.Text = "Station # <Blank = All>";
            // 
            // ultraLabel12
            // 
            appearance9.ForeColor = System.Drawing.Color.Black;
            this.ultraLabel12.Appearance = appearance9;
            this.ultraLabel12.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.4F);
            this.ultraLabel12.Location = new System.Drawing.Point(235, 22);
            this.ultraLabel12.Name = "ultraLabel12";
            this.ultraLabel12.Size = new System.Drawing.Size(147, 16);
            this.ultraLabel12.TabIndex = 6;
            this.ultraLabel12.Text = "User ID <Blank = All>";
            // 
            // txtUserID
            // 
            this.txtUserID.AutoSize = false;
            this.txtUserID.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.txtUserID.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.4F);
            this.txtUserID.Location = new System.Drawing.Point(387, 20);
            this.txtUserID.MaxLength = 20;
            this.txtUserID.Name = "txtUserID";
            this.txtUserID.Size = new System.Drawing.Size(144, 20);
            this.txtUserID.TabIndex = 2;
            this.txtUserID.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // btnSearch
            // 
            appearance10.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(62)))), ((int)(((byte)(176)))));
            appearance10.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance10.FontData.BoldAsString = "True";
            appearance10.ForeColor = System.Drawing.Color.White;
            this.btnSearch.Appearance = appearance10;
            this.btnSearch.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            this.btnSearch.Location = new System.Drawing.Point(810, 44);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(85, 23);
            this.btnSearch.TabIndex = 8;
            this.btnSearch.Text = "&Search";
            this.btnSearch.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
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
            this.groupBox2.Location = new System.Drawing.Point(10, 414);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(994, 45);
            this.groupBox2.TabIndex = 31;
            this.groupBox2.TabStop = false;
            // 
            // btnPrint
            // 
            this.btnPrint.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            appearance11.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(62)))), ((int)(((byte)(176)))));
            appearance11.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance11.FontData.BoldAsString = "True";
            appearance11.ForeColor = System.Drawing.Color.White;
            this.btnPrint.Appearance = appearance11;
            this.btnPrint.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            this.btnPrint.Location = new System.Drawing.Point(718, 14);
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
            appearance12.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(62)))), ((int)(((byte)(176)))));
            appearance12.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance12.FontData.BoldAsString = "True";
            appearance12.ForeColor = System.Drawing.Color.White;
            this.btnClose.Appearance = appearance12;
            this.btnClose.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(900, 14);
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
            appearance13.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(62)))), ((int)(((byte)(176)))));
            appearance13.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance13.FontData.BoldAsString = "True";
            appearance13.ForeColor = System.Drawing.Color.White;
            this.btnView.Appearance = appearance13;
            this.btnView.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            this.btnView.Location = new System.Drawing.Point(810, 14);
            this.btnView.Name = "btnView";
            this.btnView.Size = new System.Drawing.Size(85, 26);
            this.btnView.TabIndex = 1;
            this.btnView.Text = "&View";
            this.btnView.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnView.Click += new System.EventHandler(this.btnView_Click);
            // 
            // grdSearch
            // 
            this.grdSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            appearance14.BackColor = System.Drawing.Color.White;
            appearance14.BackColor2 = System.Drawing.Color.White;
            appearance14.BackColorDisabled = System.Drawing.Color.White;
            appearance14.BackColorDisabled2 = System.Drawing.Color.White;
            appearance14.BackGradientStyle = Infragistics.Win.GradientStyle.None;
            this.grdSearch.DisplayLayout.Appearance = appearance14;
            ultraGridColumn1.Header.VisiblePosition = 0;
            ultraGridColumn2.Header.VisiblePosition = 1;
            ultraGridColumn3.Header.VisiblePosition = 2;
            ultraGridBand1.Columns.AddRange(new object[] {
            ultraGridColumn1,
            ultraGridColumn2,
            ultraGridColumn3});
            ultraGridBand1.HeaderVisible = true;
            this.grdSearch.DisplayLayout.BandsSerializer.Add(ultraGridBand1);
            this.grdSearch.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.grdSearch.DisplayLayout.InterBandSpacing = 10;
            this.grdSearch.DisplayLayout.MaxColScrollRegions = 1;
            this.grdSearch.DisplayLayout.MaxRowScrollRegions = 1;
            appearance15.BackColor = System.Drawing.Color.White;
            appearance15.BackColor2 = System.Drawing.Color.White;
            this.grdSearch.DisplayLayout.Override.ActiveCardCaptionAppearance = appearance15;
            appearance16.BackColor = System.Drawing.Color.White;
            appearance16.BackColor2 = System.Drawing.Color.White;
            appearance16.BorderColor = System.Drawing.Color.Gray;
            this.grdSearch.DisplayLayout.Override.ActiveRowAppearance = appearance16;
            appearance17.BackColor = System.Drawing.Color.White;
            appearance17.BackColor2 = System.Drawing.Color.White;
            appearance17.BorderColor = System.Drawing.Color.Gray;
            this.grdSearch.DisplayLayout.Override.AddRowAppearance = appearance17;
            this.grdSearch.DisplayLayout.Override.AllowAddNew = Infragistics.Win.UltraWinGrid.AllowAddNew.No;
            this.grdSearch.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.False;
            this.grdSearch.DisplayLayout.Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.False;
            appearance18.BackColor = System.Drawing.Color.Transparent;
            this.grdSearch.DisplayLayout.Override.CardAreaAppearance = appearance18;
            appearance19.BackColor = System.Drawing.Color.White;
            appearance19.BackColor2 = System.Drawing.Color.White;
            appearance19.BackColorDisabled = System.Drawing.Color.White;
            appearance19.BackColorDisabled2 = System.Drawing.Color.White;
            appearance19.BorderColor = System.Drawing.Color.Black;
            appearance19.BorderColor3DBase = System.Drawing.Color.Black;
            this.grdSearch.DisplayLayout.Override.CellAppearance = appearance19;
            appearance20.BackColor = System.Drawing.Color.White;
            appearance20.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(231)))), ((int)(((byte)(230)))));
            appearance20.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance20.BorderAlpha = Infragistics.Win.Alpha.Transparent;
            appearance20.BorderColor = System.Drawing.Color.Gray;
            appearance20.BorderColor3DBase = System.Drawing.Color.Gray;
            appearance20.ImageBackgroundAlpha = Infragistics.Win.Alpha.Transparent;
            appearance20.ImageBackgroundStyle = Infragistics.Win.ImageBackgroundStyle.Centered;
            this.grdSearch.DisplayLayout.Override.CellButtonAppearance = appearance20;
            appearance21.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            appearance21.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.grdSearch.DisplayLayout.Override.EditCellAppearance = appearance21;
            appearance22.BorderColor = System.Drawing.Color.Gray;
            this.grdSearch.DisplayLayout.Override.FilteredInRowAppearance = appearance22;
            appearance23.BorderColor = System.Drawing.Color.Gray;
            this.grdSearch.DisplayLayout.Override.FilteredOutRowAppearance = appearance23;
            appearance24.BackColor = System.Drawing.Color.White;
            appearance24.BackColor2 = System.Drawing.Color.White;
            appearance24.BackColorDisabled = System.Drawing.Color.White;
            appearance24.BackColorDisabled2 = System.Drawing.Color.White;
            this.grdSearch.DisplayLayout.Override.FixedCellAppearance = appearance24;
            appearance25.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            appearance25.BackColor2 = System.Drawing.Color.Beige;
            this.grdSearch.DisplayLayout.Override.FixedHeaderAppearance = appearance25;
            appearance26.BackColor = System.Drawing.Color.White;
            appearance26.BackColor2 = System.Drawing.SystemColors.Control;
            appearance26.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance26.FontData.BoldAsString = "True";
            appearance26.ForeColor = System.Drawing.Color.Black;
            appearance26.TextHAlignAsString = "Left";
            appearance26.ThemedElementAlpha = Infragistics.Win.Alpha.Transparent;
            this.grdSearch.DisplayLayout.Override.HeaderAppearance = appearance26;
            appearance27.BorderColor = System.Drawing.Color.Gray;
            this.grdSearch.DisplayLayout.Override.RowAlternateAppearance = appearance27;
            appearance28.BackColor = System.Drawing.Color.White;
            appearance28.BackColor2 = System.Drawing.Color.White;
            appearance28.BackGradientStyle = Infragistics.Win.GradientStyle.None;
            appearance28.BackHatchStyle = Infragistics.Win.BackHatchStyle.None;
            appearance28.BorderColor = System.Drawing.Color.Gray;
            this.grdSearch.DisplayLayout.Override.RowAppearance = appearance28;
            appearance29.BorderColor = System.Drawing.Color.Gray;
            this.grdSearch.DisplayLayout.Override.RowPreviewAppearance = appearance29;
            appearance30.BackColor = System.Drawing.Color.White;
            appearance30.BackColor2 = System.Drawing.SystemColors.Control;
            appearance30.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance30.BorderColor = System.Drawing.Color.Gray;
            this.grdSearch.DisplayLayout.Override.RowSelectorAppearance = appearance30;
            this.grdSearch.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            this.grdSearch.DisplayLayout.Override.RowSelectorWidth = 12;
            this.grdSearch.DisplayLayout.Override.RowSpacingBefore = 2;
            appearance31.BackColor = System.Drawing.Color.Navy;
            appearance31.BackColorAlpha = Infragistics.Win.Alpha.Opaque;
            this.grdSearch.DisplayLayout.Override.SelectedCellAppearance = appearance31;
            appearance32.BackColor = System.Drawing.Color.Navy;
            appearance32.BackColorDisabled = System.Drawing.Color.Navy;
            appearance32.BackGradientStyle = Infragistics.Win.GradientStyle.None;
            appearance32.BackHatchStyle = Infragistics.Win.BackHatchStyle.None;
            appearance32.BorderColor = System.Drawing.Color.Gray;
            appearance32.ForeColor = System.Drawing.Color.Black;
            this.grdSearch.DisplayLayout.Override.SelectedRowAppearance = appearance32;
            this.grdSearch.DisplayLayout.Override.SelectTypeCell = Infragistics.Win.UltraWinGrid.SelectType.None;
            this.grdSearch.DisplayLayout.Override.SelectTypeCol = Infragistics.Win.UltraWinGrid.SelectType.None;
            this.grdSearch.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.Single;
            appearance33.BorderColor = System.Drawing.Color.Gray;
            this.grdSearch.DisplayLayout.Override.TemplateAddRowAppearance = appearance33;
            this.grdSearch.DisplayLayout.RowConnectorColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            this.grdSearch.DisplayLayout.RowConnectorStyle = Infragistics.Win.UltraWinGrid.RowConnectorStyle.Solid;
            appearance34.BackColor = System.Drawing.Color.White;
            appearance34.BackColor2 = System.Drawing.SystemColors.Control;
            appearance34.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            scrollBarLook1.Appearance = appearance34;
            appearance35.BackColor = System.Drawing.Color.White;
            appearance35.BackColor2 = System.Drawing.SystemColors.Control;
            appearance35.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance35.ThemedElementAlpha = Infragistics.Win.Alpha.Transparent;
            scrollBarLook1.ButtonAppearance = appearance35;
            appearance36.BackColor = System.Drawing.Color.White;
            appearance36.BackColor2 = System.Drawing.SystemColors.Control;
            scrollBarLook1.ThumbAppearance = appearance36;
            appearance37.BackColor = System.Drawing.Color.White;
            appearance37.BackColor2 = System.Drawing.Color.White;
            scrollBarLook1.TrackAppearance = appearance37;
            this.grdSearch.DisplayLayout.ScrollBarLook = scrollBarLook1;
            this.grdSearch.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.grdSearch.Location = new System.Drawing.Point(10, 106);
            this.grdSearch.Name = "grdSearch";
            this.grdSearch.Size = new System.Drawing.Size(994, 309);
            this.grdSearch.TabIndex = 0;
            this.grdSearch.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.grdSearch.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.grdSearch_InitializeLayout);
            this.grdSearch.InitializeRow += new Infragistics.Win.UltraWinGrid.InitializeRowEventHandler(this.grdSearch_InitializeRow);
            this.grdSearch.BeforeRowExpanded += new Infragistics.Win.UltraWinGrid.CancelableRowEventHandler(this.grdSearch_BeforeRowExpanded);
            this.grdSearch.DoubleClick += new System.EventHandler(this.grdSearch_DoubleClick);
            this.grdSearch.MouseMove += new System.Windows.Forms.MouseEventHandler(this.grdSearch_MouseMove);
            // 
            // frmRptOnholdTransactionDetails
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1008, 462);
            this.Controls.Add(this.grdSearch);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.Name = "frmRptOnholdTransactionDetails";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "On-Hold Transaction Detail Report";
            this.Load += new System.EventHandler(this.frmRptOnholdTransactionDetails_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmRptOnholdTransactionDetails_KeyDown);
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.cboOnHoldType)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboOnHoldTrans)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtItemCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboTransType)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtpSaleEndDate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtpSaleStartDate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtStationID)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtUserID)).EndInit();
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdSearch)).EndInit();
            this.ResumeLayout(false);

		}
		#endregion

		private void frmRptOnholdTransactionDetails_Load(object sender, System.EventArgs e)
		{           
            this.dtpSaleStartDate.Enter += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
            this.dtpSaleStartDate.Leave += new System.EventHandler(clsUIHelper.AfterExitEditMode);
            this.dtpSaleEndDate.Enter += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
            this.dtpSaleEndDate.Leave += new System.EventHandler(clsUIHelper.AfterExitEditMode);

            this.txtUserID.AfterEnterEditMode += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
            this.txtUserID.AfterExitEditMode += new System.EventHandler(clsUIHelper.AfterExitEditMode);
            this.txtStationID.AfterEnterEditMode += new System.EventHandler(clsUIHelper.AfterEnterEditMode);			
			this.txtStationID.AfterExitEditMode += new System.EventHandler(clsUIHelper.AfterExitEditMode);			
			this.txtItemCode.AfterEnterEditMode += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
			this.txtItemCode.AfterExitEditMode += new System.EventHandler(clsUIHelper.AfterExitEditMode);

            this.Left=(frmMain.getInstance().Width-frmMain.getInstance().ultraExplorerBar1.Width-this.Width)/2;
			this.Top=(frmMain.getInstance().Height-this.Height)/2;

            this.cboTransType.SelectedIndex = 0;
            this.cboOnHoldTrans.SelectedIndex = 0;
            this.cboOnHoldType.SelectedIndex = 0;

            clsUIHelper.setColorSchecme(this);
			this.dtpSaleEndDate.Value=DateTime.Now;
			this.dtpSaleStartDate.Value=DateTime.Now;

            this.Location = new Point(45, 2);

            clsUIHelper.SetAppearance(this.grdSearch);
            clsUIHelper.SetReadonlyRow(this.grdSearch);
            grdSearch.DisplayLayout.Override.HeaderClickAction = HeaderClickAction.SortMulti;
            grdSearch.DisplayLayout.AutoFitStyle = AutoFitStyle.ResizeAllColumns;
            btnSearch_Click(sender, e);
        }

		private void btnView_Click(object sender, System.EventArgs e)
		{
			PreviewReport(false);
		}

		private void btnClose_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}

		private void frmRptOnholdTransactionDetails_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			try
			{
				if(e.KeyData==System.Windows.Forms.Keys.Enter)
				{
					this.SelectNextControl(this.ActiveControl,true,true,true,true);
				}
				else if (e.KeyData==Keys.Escape)
					this.Close();
				else if (e.KeyData==System.Windows.Forms.Keys.F4)
				{
					if (this.txtItemCode.ContainsFocus==true)
					{
                        this.SearchItem();
                    }
				}
			}
			catch(Exception exp)
			{
				clsUIHelper.ShowErrorMsg( exp.Message);
			}
		}

		private void PreviewReport(bool blnPrint)
		{
			try
			{
				this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
				rptOnholdTransactionDetail oRpt = new rptOnholdTransactionDetail();
                string strTransSQL = string.Empty, strTransDetailsSQL = string.Empty;
                string strQuery = GenerateSQL(out strTransSQL, out strTransDetailsSQL, true);
                
                DataSet dsMainRptSource = clsReports.GetReportSource(strQuery);
                clsReports.DStoExport = dsMainRptSource;

                oRpt.Database.Tables[0].SetDataSource(dsMainRptSource.Tables[0]);
                clsReports.setCRTextObjectText("txtFromDate", "From :" + this.dtpSaleStartDate.Text, oRpt);
                clsReports.setCRTextObjectText("txtToDate", "To :" + dtpSaleEndDate.Text, oRpt);

                if (cboOnHoldTrans.SelectedIndex == 0) 
                    clsReports.setCRTextObjectText("txtHeading", "POS On-hold Transaction Details", oRpt);
                else
                    clsReports.setCRTextObjectText("txtHeading", "POS Processed Transaction Details", oRpt);

                clsReports.Preview(blnPrint,oRpt);
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
        
		private void txtItemCode_EditorButtonClick(object sender, Infragistics.Win.UltraWinEditors.EditorButtonEventArgs e)
		{
			SearchItem();		
		}

		private void SearchItem()
		{
			try
			{
                frmSearchMain oSearch = new frmSearchMain(true);
                oSearch.SearchTable = clsPOSDBConstants.Item_tbl;
                oSearch.ShowDialog(this);
				if (!oSearch.IsCanceled)
				{
					string strCode=oSearch.SelectedRowID();
					if (strCode == "") 
						return;
					
					FKEdit(strCode,clsPOSDBConstants.Item_tbl);
					this.txtItemCode.Focus();
				}
			}
			catch(Exception exp)
			{
				clsUIHelper.ShowErrorMsg(exp.Message);
			}
		}

		private void FKEdit(string code,string senderName)
		{
			if (senderName==clsPOSDBConstants.Item_tbl)
			{
				#region item
				try
				{
					POS_Core.BusinessRules.Item  oItem=new Item();
					ItemData oItemData;
					ItemRow oItemRow=null;
					oItemData = oItem.Populate(code);
					oItemRow = oItemData.Item[0];
					if (oItemRow!=null)
					{
						this.txtItemCode.Text=oItemRow.ItemID;
					}
				}
				catch(System.IndexOutOfRangeException )
				{
					this.txtItemCode.Text=String.Empty;
					SearchItem();
				}
				catch(Exception exp)
				{
					clsUIHelper.ShowErrorMsg(exp.Message);
					this.txtItemCode.Text=String.Empty;
					SearchItem();
				}
				#endregion
			}
		}

        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                DataSet oDataSet = new DataSet();
                SearchSvr oSearchSvr = new SearchSvr();
                String strTransSQL, strTransDetailsSQL;

                GenerateSQL(out strTransSQL, out strTransDetailsSQL, false);

                oDataSet.Tables.Add(oSearchSvr.Search(strTransSQL).Tables[0].Copy());
                oDataSet.Tables[0].TableName = "Master";

                oDataSet.Tables.Add(oSearchSvr.Search(strTransDetailsSQL).Tables[0].Copy());
                oDataSet.Tables[1].TableName = "Detail";

                oDataSet.Relations.Add("MasterDetail", oDataSet.Tables[0].Columns["TransID"], oDataSet.Tables[1].Columns["TransID"]);

                grdSearch.DataSource = oDataSet;

                grdSearch.DisplayLayout.Bands[0].HeaderVisible = true;
                grdSearch.DisplayLayout.Bands[0].Header.Appearance.FontData.SizeInPoints = 10;
                grdSearch.DisplayLayout.Bands[0].Header.Appearance.TextHAlign = Infragistics.Win.HAlign.Center;
                grdSearch.DisplayLayout.Bands[0].Header.Caption = "Transactions";
                grdSearch.DisplayLayout.Bands[0].Columns["ISDELIVERY"].Hidden = true;
                if (cboOnHoldType.SelectedIndex == 2)
                {
                    grdSearch.DisplayLayout.Bands[0].Columns["DeliveryAddress"].Hidden = true;
                }

                grdSearch.DisplayLayout.Bands[1].HeaderVisible = true;
                grdSearch.DisplayLayout.Bands[1].Header.Caption = "Transaction Detail";
                grdSearch.DisplayLayout.Bands[1].Header.Appearance.FontData.SizeInPoints = 10;
                grdSearch.DisplayLayout.Bands[1].Header.Appearance.TextHAlign = Infragistics.Win.HAlign.Center;
                grdSearch.DisplayLayout.Bands[1].Columns["TransID"].Hidden = true;
                grdSearch.DisplayLayout.Bands[1].Expandable = true;

                this.resizeColumns();
                grdSearch.Focus();
                grdSearch.PerformAction(UltraGridAction.FirstRowInGrid);
                grdSearch.Refresh();
            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }

        private string GenerateSQL(out String strTransSQL, out string strTransDetailsSQL, bool PreviewReport)
        {
            string strSQL = string.Empty, strFilter = string.Empty;
            strTransSQL = string.Empty;
            strTransDetailsSQL = string.Empty;

            if (PreviewReport == true)
            {
                if (cboOnHoldTrans.SelectedIndex == 0)  //On hold transactions
                {
                    strSQL = "SELECT DISTINCT PT.TransID, PT.TransDate, CASE PT.TransType WHEN 1 THEN 'Sale' WHEN 2 THEN 'Return' WHEN 3 THEN 'ROA' END AS TransType," +
                        " PT.UserID, PT.StationID, PT.ISDELIVERY, PT.DeliveryAddress, PT.TotalDiscAmount," +
                        " PTD.ItemID, PTD.ItemDescription as Description, PTD.Qty, PTD.DISCOUNT, PTD.TaxAMOUNT, PTD.Price, PTD.ExtendedPrice, ps.StatioNname" +
                        " FROM POSTransaction_OnHold PT INNER JOIN POSTransactionDetail_OnHold PTD ON PT.TransID = PTD.TransID" +
                        " INNER JOIN Item I ON I.ItemID = PTD.ItemID " +
                        " INNER JOIN util_POSSet ps ON ps.stationid = PT.stationid " +
                        " WHERE CONVERT(DATETIME, PT.TransDate,109) BETWEEN CONVERT(DATETIME, CAST('" + this.dtpSaleStartDate.Text + " 00:00:00' AS DATETIME) ,113) AND CONVERT(DATETIME, CAST('" + this.dtpSaleEndDate.Text + " 23:59:59' AS DATETIME), 113)";
                }
                else
                {
                    strSQL = "SELECT DISTINCT PT.TransID, PT.TransDate, CASE PT.TransType WHEN 1 THEN 'Sale' WHEN 2 THEN 'Return' WHEN 3 THEN 'ROA' END AS TransType," +
                        " PT.UserID, PT.StationID, PT.ISDELIVERY, PT.DeliveryAddress, PT.TotalDiscAmount," +
                        " PTD.ItemID, PTD.ItemDescription as Description, PTD.Qty, PTD.DISCOUNT, PTD.TaxAMOUNT, PTD.Price, PTD.ExtendedPrice, ps.StatioNname" +
                        " FROM postransaction PT INNER JOIN POSTransactionDetail PTD ON PT.TransID = PTD.TransID" +
                        " INNER JOIN Item I ON I.ItemID = PTD.ItemID " +
                        " INNER JOIN util_POSSet ps ON ps.stationid = PT.stationid " +
                        " WHERE PT.WasonHold = 1 AND CONVERT(DATETIME, PT.TransDate,109) BETWEEN CONVERT(DATETIME, CAST('" + this.dtpSaleStartDate.Text + " 00:00:00' AS DATETIME) ,113) AND CONVERT(DATETIME, CAST('" + this.dtpSaleEndDate.Text + " 23:59:59' AS DATETIME), 113)";
                }
            }
            else
            {
                if (cboOnHoldTrans.SelectedIndex == 0)  //On hold transactions
                {
                    strTransSQL = "SELECT DISTINCT PT.TransID, PT.TransDate, CASE PT.TransType WHEN 1 THEN 'Sale' WHEN 2 THEN 'Return' WHEN 3 THEN 'ROA' END AS TransType," +
                        " LTRIM(RTRIM(CustomerName + ' ' + FIRSTNAME)) AS [Customer Name], PT.UserID AS [User], PT.StationID AS [Station], PT.ISDELIVERY, PT.DeliveryAddress FROM POSTransaction_OnHold PT" +
                        " INNER JOIN POSTransactionDetail_OnHold PTD ON PTD.TransID = PT.TransID" +
                        " INNER JOIN Customer C ON C.CustomerID = PT.CustomerID" +
                        " WHERE CONVERT(DATETIME, PT.TransDate,109) BETWEEN CONVERT(DATETIME, CAST('" + this.dtpSaleStartDate.Text + " 00:00:00' AS DATETIME) ,113) AND CONVERT(DATETIME, CAST('" + this.dtpSaleEndDate.Text + " 23:59:59' AS DATETIME), 113)";

                    strTransDetailsSQL = "SELECT DISTINCT PT.TransID, PTD.ItemID, PTD.ItemDescription as Description, PTD.Qty, PTD.DISCOUNT AS Discount, PTD.TaxAMOUNT AS Tax, PTD.Price, PTD.ExtendedPrice" +
                        " FROM POSTransaction_OnHold PT" +
                        " INNER JOIN POSTransactionDetail_OnHold PTD ON PTD.TransID = PT.TransID" +
                        " WHERE CONVERT(DATETIME, PT.TransDate,109) BETWEEN CONVERT(DATETIME, CAST('" + this.dtpSaleStartDate.Text + " 00:00:00' AS DATETIME) ,113) AND CONVERT(DATETIME, CAST('" + this.dtpSaleEndDate.Text + " 23:59:59' AS DATETIME), 113)";
                }
                else //processed transactions
                {
                    strTransSQL = "SELECT DISTINCT PT.TransID, PT.TransDate, CASE PT.TransType WHEN 1 THEN 'Sale' WHEN 2 THEN 'Return' WHEN 3 THEN 'ROA' END AS TransType," +
                        " LTRIM(RTRIM(CustomerName + ' ' + FIRSTNAME)) AS[Customer Name], PT.UserID AS[User], PT.StationID AS[Station], PT.ISDELIVERY, PT.DeliveryAddress FROM POSTransaction PT" +
                        " INNER JOIN POSTransactionDetail PTD ON PTD.TransID = PT.TransID" +
                        " INNER JOIN Customer C ON C.CustomerID = PT.CustomerID" +
                        " WHERE PT.WasonHold = 1 AND CONVERT(DATETIME, PT.TransDate,109) BETWEEN CONVERT(DATETIME, CAST('" + this.dtpSaleStartDate.Text + " 00:00:00' AS DATETIME) ,113) AND CONVERT(DATETIME, CAST('" + this.dtpSaleEndDate.Text + " 23:59:59' AS DATETIME), 113)";

                    strTransDetailsSQL = "SELECT DISTINCT PT.TransID, PTD.ItemID, PTD.ItemDescription as Description, PTD.Qty, PTD.DISCOUNT AS Discount, PTD.TaxAMOUNT AS Tax, PTD.Price, PTD.ExtendedPrice" +
                        " FROM POSTransaction PT" +
                        " INNER JOIN POSTransactionDetail PTD ON PTD.TransID = PT.TransID" +
                        " WHERE PT.WasonHold = 1 AND CONVERT(DATETIME, PT.TransDate,109) BETWEEN CONVERT(DATETIME, CAST('" + this.dtpSaleStartDate.Text + " 00:00:00' AS DATETIME) ,113) AND CONVERT(DATETIME, CAST('" + this.dtpSaleEndDate.Text + " 23:59:59' AS DATETIME), 113)";
                }
            }

            if (this.txtUserID.Text.Trim() != "")
            {
                strFilter += " AND PT.UserID = '" + this.txtUserID.Text.Trim().Replace("'", "''") + "' ";
            }
            if (this.txtStationID.Text.Trim() != "")
            {
                strFilter += " AND PT.StationID = '" + this.txtStationID.Text.Trim().Replace("'", "''") + "' ";
            }
            if (this.txtItemCode.Text.Trim() != "")
            {
                strFilter += " AND PTD.ItemID = '" + this.txtItemCode.Text.Trim().Replace("'", "''") + "' ";
            }

            if (this.cboTransType.SelectedIndex == 1)
            {
                strFilter += " AND PT.TransType = 1";
            }
            else if (this.cboTransType.SelectedIndex == 2)
            {
                strFilter += " AND PT.TransType = 2";
            }

            if (cboOnHoldType.SelectedIndex == 1)   //delivery
                strFilter += " AND PT.ISDELIVERY = 1";
            else if (cboOnHoldType.SelectedIndex == 2)  //Non-delivery
                strFilter += " AND PT.ISDELIVERY = 0";
            
            if (strFilter != string.Empty)
            {
                strTransSQL += strFilter;
                strTransDetailsSQL += strFilter;
                if (PreviewReport == true)
                    strSQL += strFilter;
            }
            return strSQL;
        }

        private void resizeColumns()
        {
            try
            {
                foreach (UltraGridBand oBand in grdSearch.DisplayLayout.Bands)
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

        private void grdSearch_BeforeRowExpanded(object sender, CancelableRowEventArgs e)
        {
            foreach (Infragistics.Win.UltraWinGrid.UltraGridRow orow in grdSearch.DisplayLayout.Rows)
            {
                if (orow.Expanded == true)
                    orow.CollapseAll();
            }
            e.Row.Activate();
        }

        private void grdSearch_MouseMove(object sender, MouseEventArgs e)
        {
            this.CurrentX = e.X;
            this.CurrentY = e.Y;
        }

        private void grdSearch_InitializeRow(object sender, InitializeRowEventArgs e)
        {
            if (e.Row.Band.Index == 2)
            {
                e.Row.Cells["Viewsign"].Value = "View Sign";

                if ((e.Row.Cells["SigType"].Value.ToString().Trim() == clsPOSDBConstants.STRINGIMAGE))//|| e.Row.Cells["SigType"].Value.ToString().Trim() == "*") )
                {
                    if (e.Row.Cells["CustomerSign"].Value.ToString().Trim() == "")
                    {
                        e.Row.Cells["ViewSign"].Activation = Activation.Disabled;
                    }
                    else
                    {
                        e.Row.Cells["ViewSign"].Activation = Activation.ActivateOnly;
                    }
                }
                else if ((e.Row.Cells["SigType"].Value.ToString().Trim() == clsPOSDBConstants.BINARYIMAGE))//|| e.Row.Cells["SigType"].Value.ToString().Trim() == "*") )
                {
                    if (e.Row.Cells["BinarySign"].Value == System.DBNull.Value)
                    {
                        e.Row.Cells["ViewSign"].Activation = Activation.Disabled;
                    }
                    else
                    {
                        e.Row.Cells["ViewSign"].Activation = Activation.ActivateOnly;
                    }
                }
                else if (e.Row.Cells["SigType"].Value.ToString().Trim() == string.Empty)
                {
                    e.Row.Cells["ViewSign"].Activation = Activation.Disabled;
                }
                else if (e.Row.Cells["SigType"].Value.ToString().Trim() == "*")
                {
                    e.Row.Cells["ViewSign"].Activation = Activation.Disabled;
                }
            }
        }

        private void grdSearch_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            try
            {
                for (int i = 0; i < this.grdSearch.DisplayLayout.Bands[0].Columns.Count; i++)
                {
                    if (this.grdSearch.DisplayLayout.Bands[0].Columns[i].DataType == typeof(System.Decimal))
                    {
                        this.grdSearch.DisplayLayout.Bands[0].Columns[i].Format = "#######0.00";
                        this.grdSearch.DisplayLayout.Bands[0].Columns[i].CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right;
                    }
                }
            }
            catch (Exception) { }
        }

        private void cboTransType_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Tab || e.KeyCode == Keys.Up || e.KeyCode == Keys.Down)
                e.Handled = false;
            else
                e.Handled = true;
        }

        private void cboTransType_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }
    }
}
