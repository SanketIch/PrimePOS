using System;
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
using POS_Core.CommonData.Rows;
using POS_Core.DataAccess;
using System.Data;
using POS_Core.Resources;
using System.Timers;

namespace POS_Core_UI.Reports.ReportsUI
{

    //Added By Abhishek(SRT) 
    public enum InventoryReceivedEnum
    {
        InventoryReceive = 0,
        ItemListByVendor = 1
    }
    //End of Added By Abhishek(SRT)

    /// <summary>
	/// Summary description for frmInventoryReports.
	/// </summary>
	public class frmRptInventoryReceived : System.Windows.Forms.Form
	{
		private System.Windows.Forms.GroupBox gbInventoryReceived;
		private Infragistics.Win.UltraWinSchedule.UltraCalendarCombo dtpToDate;
		private Infragistics.Win.UltraWinSchedule.UltraCalendarCombo dtpFromDate;
		private Infragistics.Win.UltraWinEditors.UltraTextEditor txtItemCode;
		private Infragistics.Win.UltraWinEditors.UltraTextEditor txtVendorCode;
		private Infragistics.Win.Misc.UltraLabel ultraLabel11;
		private Infragistics.Win.Misc.UltraLabel ultraLabel12;
		private Infragistics.Win.Misc.UltraLabel ultraLabel13;
		private Infragistics.Win.Misc.UltraLabel ultraLabel14;
		private System.Windows.Forms.GroupBox groupBox2;
		private Infragistics.Win.Misc.UltraButton ultraButton1;
		private Infragistics.Win.Misc.UltraButton btnView;
		private Infragistics.Win.Misc.UltraButton btnClose;
		private Infragistics.Win.Misc.UltraLabel lblTransactionType;
		private Infragistics.Win.Misc.UltraLabel ultraLabel2;
		private Infragistics.Win.UltraWinEditors.UltraComboEditor cboTransType;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
        private Infragistics.Win.UltraWinEditors.UltraComboEditor cboUsers;
        private Infragistics.Win.Misc.UltraLabel lblUser;
        private Infragistics.Win.Misc.UltraLabel lblSecSort;
        private Infragistics.Win.UltraWinEditors.UltraComboEditor cboSecondarySort;
        private Infragistics.Win.Misc.UltraLabel lblPrimarySort;
        private Infragistics.Win.UltraWinEditors.UltraComboEditor cboPrimarySort;
        private Infragistics.Win.Misc.UltraLabel ultraLabel1;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor txtRefferenceNo;
        public Infragistics.Win.Misc.UltraLabel lblMessage;

        //Abhishek
        private InventoryReceivedEnum mReportType;
        //Parameter Added By Abhishek

        #region PRIMEPOS-2464 10-Mar-2020 JY Added
        int nDisplayItemCost = 0;
        System.Timers.Timer tmrBlinking;
        private long iBlinkCnt = 0;
        #endregion

        public frmRptInventoryReceived(InventoryReceivedEnum oReportType)
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
            mReportType = oReportType;
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
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.ValueListItem valueListItem1 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem2 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem3 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem4 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem5 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem6 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance7 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance8 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance9 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance10 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance11 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinSchedule.CalendarCombo.DateButton dateButton1 = new Infragistics.Win.UltraWinSchedule.CalendarCombo.DateButton();
            Infragistics.Win.Appearance appearance12 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinSchedule.CalendarCombo.DateButton dateButton2 = new Infragistics.Win.UltraWinSchedule.CalendarCombo.DateButton();
            Infragistics.Win.UltraWinEditors.EditorButton editorButton1 = new Infragistics.Win.UltraWinEditors.EditorButton();
            Infragistics.Win.Appearance appearance13 = new Infragistics.Win.Appearance();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmRptInventoryReceived));
            Infragistics.Win.UltraWinEditors.EditorButton editorButton2 = new Infragistics.Win.UltraWinEditors.EditorButton();
            Infragistics.Win.Appearance appearance14 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance15 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance16 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance17 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance18 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance19 = new Infragistics.Win.Appearance();
            this.gbInventoryReceived = new System.Windows.Forms.GroupBox();
            this.txtRefferenceNo = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.ultraLabel1 = new Infragistics.Win.Misc.UltraLabel();
            this.lblSecSort = new Infragistics.Win.Misc.UltraLabel();
            this.cboSecondarySort = new Infragistics.Win.UltraWinEditors.UltraComboEditor();
            this.lblPrimarySort = new Infragistics.Win.Misc.UltraLabel();
            this.cboPrimarySort = new Infragistics.Win.UltraWinEditors.UltraComboEditor();
            this.cboUsers = new Infragistics.Win.UltraWinEditors.UltraComboEditor();
            this.lblUser = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabel2 = new Infragistics.Win.Misc.UltraLabel();
            this.cboTransType = new Infragistics.Win.UltraWinEditors.UltraComboEditor();
            this.dtpToDate = new Infragistics.Win.UltraWinSchedule.UltraCalendarCombo();
            this.dtpFromDate = new Infragistics.Win.UltraWinSchedule.UltraCalendarCombo();
            this.txtItemCode = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.txtVendorCode = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.ultraLabel11 = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabel12 = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabel13 = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabel14 = new Infragistics.Win.Misc.UltraLabel();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.ultraButton1 = new Infragistics.Win.Misc.UltraButton();
            this.btnClose = new Infragistics.Win.Misc.UltraButton();
            this.btnView = new Infragistics.Win.Misc.UltraButton();
            this.lblTransactionType = new Infragistics.Win.Misc.UltraLabel();
            this.lblMessage = new Infragistics.Win.Misc.UltraLabel();
            this.gbInventoryReceived.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtRefferenceNo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboSecondarySort)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboPrimarySort)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboUsers)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboTransType)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtpToDate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtpFromDate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtItemCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtVendorCode)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbInventoryReceived
            // 
            this.gbInventoryReceived.Controls.Add(this.txtRefferenceNo);
            this.gbInventoryReceived.Controls.Add(this.ultraLabel1);
            this.gbInventoryReceived.Controls.Add(this.lblSecSort);
            this.gbInventoryReceived.Controls.Add(this.cboSecondarySort);
            this.gbInventoryReceived.Controls.Add(this.lblPrimarySort);
            this.gbInventoryReceived.Controls.Add(this.cboPrimarySort);
            this.gbInventoryReceived.Controls.Add(this.cboUsers);
            this.gbInventoryReceived.Controls.Add(this.lblUser);
            this.gbInventoryReceived.Controls.Add(this.ultraLabel2);
            this.gbInventoryReceived.Controls.Add(this.cboTransType);
            this.gbInventoryReceived.Controls.Add(this.dtpToDate);
            this.gbInventoryReceived.Controls.Add(this.dtpFromDate);
            this.gbInventoryReceived.Controls.Add(this.txtItemCode);
            this.gbInventoryReceived.Controls.Add(this.txtVendorCode);
            this.gbInventoryReceived.Controls.Add(this.ultraLabel11);
            this.gbInventoryReceived.Controls.Add(this.ultraLabel12);
            this.gbInventoryReceived.Controls.Add(this.ultraLabel13);
            this.gbInventoryReceived.Controls.Add(this.ultraLabel14);
            this.gbInventoryReceived.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbInventoryReceived.Location = new System.Drawing.Point(15, 40);
            this.gbInventoryReceived.Name = "gbInventoryReceived";
            this.gbInventoryReceived.Size = new System.Drawing.Size(395, 294);
            this.gbInventoryReceived.TabIndex = 0;
            this.gbInventoryReceived.TabStop = false;
            this.gbInventoryReceived.Text = "Inventory Received Report";
            this.gbInventoryReceived.Enter += new System.EventHandler(this.gbInventoryReceived_Enter_1);
            // 
            // txtRefferenceNo
            // 
            this.txtRefferenceNo.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.txtRefferenceNo.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtRefferenceNo.Location = new System.Drawing.Point(146, 198);
            this.txtRefferenceNo.MaxLength = 20;
            this.txtRefferenceNo.Name = "txtRefferenceNo";
            this.txtRefferenceNo.Size = new System.Drawing.Size(192, 20);
            this.txtRefferenceNo.TabIndex = 7;
            // 
            // ultraLabel1
            // 
            this.ultraLabel1.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultraLabel1.Location = new System.Drawing.Point(24, 201);
            this.ultraLabel1.Name = "ultraLabel1";
            this.ultraLabel1.Size = new System.Drawing.Size(91, 15);
            this.ultraLabel1.TabIndex = 50;
            this.ultraLabel1.Text = "Reference";
            // 
            // lblSecSort
            // 
            appearance1.FontData.BoldAsString = "False";
            appearance1.ForeColor = System.Drawing.Color.Black;
            this.lblSecSort.Appearance = appearance1;
            this.lblSecSort.AutoSize = true;
            this.lblSecSort.Location = new System.Drawing.Point(25, 262);
            this.lblSecSort.Name = "lblSecSort";
            this.lblSecSort.Size = new System.Drawing.Size(92, 15);
            this.lblSecSort.TabIndex = 48;
            this.lblSecSort.Text = "Secondary Sort";
            this.lblSecSort.Visible = false;
            // 
            // cboSecondarySort
            // 
            appearance2.BackColor = System.Drawing.Color.White;
            this.cboSecondarySort.Appearance = appearance2;
            this.cboSecondarySort.BackColor = System.Drawing.Color.White;
            this.cboSecondarySort.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.cboSecondarySort.DropDownStyle = Infragistics.Win.DropDownStyle.DropDownList;
            this.cboSecondarySort.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboSecondarySort.LimitToList = true;
            this.cboSecondarySort.Location = new System.Drawing.Point(146, 260);
            this.cboSecondarySort.Name = "cboSecondarySort";
            this.cboSecondarySort.Size = new System.Drawing.Size(192, 23);
            this.cboSecondarySort.TabIndex = 9;
            this.cboSecondarySort.Visible = false;
            // 
            // lblPrimarySort
            // 
            appearance3.FontData.BoldAsString = "False";
            appearance3.ForeColor = System.Drawing.Color.Black;
            this.lblPrimarySort.Appearance = appearance3;
            this.lblPrimarySort.AutoSize = true;
            this.lblPrimarySort.Location = new System.Drawing.Point(24, 230);
            this.lblPrimarySort.Name = "lblPrimarySort";
            this.lblPrimarySort.Size = new System.Drawing.Size(76, 15);
            this.lblPrimarySort.TabIndex = 46;
            this.lblPrimarySort.Text = "Primary Sort";
            this.lblPrimarySort.Visible = false;
            // 
            // cboPrimarySort
            // 
            appearance4.BackColor = System.Drawing.Color.White;
            this.cboPrimarySort.Appearance = appearance4;
            this.cboPrimarySort.BackColor = System.Drawing.Color.White;
            this.cboPrimarySort.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.cboPrimarySort.DropDownStyle = Infragistics.Win.DropDownStyle.DropDownList;
            this.cboPrimarySort.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            valueListItem1.DataValue = "ValueListItem0";
            valueListItem1.DisplayText = "Date";
            valueListItem1.Tag = "RecieveDate";
            valueListItem2.DataValue = "ValueListItem1";
            valueListItem2.DisplayText = "User";
            valueListItem2.Tag = "UserID";
            valueListItem3.DataValue = "ValueListItem2";
            valueListItem3.DisplayText = "Trans Type";
            valueListItem3.Tag = "InvTransTypeID";
            valueListItem4.DataValue = "ValueListItem3";
            valueListItem4.DisplayText = "Vendor Code";
            valueListItem4.Tag = "VendorCode";
            valueListItem5.DataValue = "ValueListItem4";
            valueListItem5.DisplayText = "Item Code";
            valueListItem5.Tag = "ItemID";
            valueListItem6.DataValue = "RefNo";
            valueListItem6.DisplayText = "Reference";
            valueListItem6.Tag = "RefNo";
            this.cboPrimarySort.Items.AddRange(new Infragistics.Win.ValueListItem[] {
            valueListItem1,
            valueListItem2,
            valueListItem3,
            valueListItem4,
            valueListItem5,
            valueListItem6});
            this.cboPrimarySort.LimitToList = true;
            this.cboPrimarySort.Location = new System.Drawing.Point(146, 228);
            this.cboPrimarySort.Name = "cboPrimarySort";
            this.cboPrimarySort.Size = new System.Drawing.Size(192, 23);
            this.cboPrimarySort.TabIndex = 8;
            this.cboPrimarySort.Visible = false;
            this.cboPrimarySort.ValueChanged += new System.EventHandler(this.cboPrimarySort_ValueChanged);
            this.cboPrimarySort.Leave += new System.EventHandler(this.cboPrimarySort_Leave);
            // 
            // cboUsers
            // 
            this.cboUsers.AlwaysInEditMode = true;
            appearance5.ForeColor = System.Drawing.Color.Black;
            this.cboUsers.Appearance = appearance5;
            this.cboUsers.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance6.BackColor = System.Drawing.Color.WhiteSmoke;
            appearance6.BackColor2 = System.Drawing.Color.Silver;
            appearance6.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            this.cboUsers.ButtonAppearance = appearance6;
            this.cboUsers.ButtonStyle = Infragistics.Win.UIElementButtonStyle.FlatBorderless;
            this.cboUsers.DropDownStyle = Infragistics.Win.DropDownStyle.DropDownList;
            this.cboUsers.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboUsers.LimitToList = true;
            this.cboUsers.Location = new System.Drawing.Point(146, 167);
            this.cboUsers.Name = "cboUsers";
            this.cboUsers.NullText = "Select";
            this.cboUsers.Size = new System.Drawing.Size(192, 23);
            this.cboUsers.TabIndex = 6;
            this.cboUsers.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.cboUsers.Visible = false;
            // 
            // lblUser
            // 
            appearance7.FontData.BoldAsString = "False";
            appearance7.ForeColor = System.Drawing.Color.Black;
            this.lblUser.Appearance = appearance7;
            this.lblUser.AutoSize = true;
            this.lblUser.Location = new System.Drawing.Point(24, 172);
            this.lblUser.Name = "lblUser";
            this.lblUser.Size = new System.Drawing.Size(47, 15);
            this.lblUser.TabIndex = 43;
            this.lblUser.Text = "User ID";
            this.lblUser.Visible = false;
            // 
            // ultraLabel2
            // 
            this.ultraLabel2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            appearance8.FontData.BoldAsString = "False";
            appearance8.ForeColorDisabled = System.Drawing.Color.Black;
            this.ultraLabel2.Appearance = appearance8;
            this.ultraLabel2.AutoSize = true;
            this.ultraLabel2.Location = new System.Drawing.Point(24, 137);
            this.ultraLabel2.Name = "ultraLabel2";
            this.ultraLabel2.Size = new System.Drawing.Size(97, 15);
            this.ultraLabel2.TabIndex = 42;
            this.ultraLabel2.Text = "Trans Type";
            this.ultraLabel2.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.ultraLabel2.Visible = false;
            // 
            // cboTransType
            // 
            this.cboTransType.AlwaysInEditMode = true;
            appearance9.BackColor = System.Drawing.Color.White;
            appearance9.ForeColor = System.Drawing.Color.Black;
            this.cboTransType.Appearance = appearance9;
            this.cboTransType.AutoSize = false;
            this.cboTransType.BackColor = System.Drawing.Color.White;
            this.cboTransType.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance10.BackColor = System.Drawing.Color.WhiteSmoke;
            appearance10.BackColor2 = System.Drawing.Color.Silver;
            appearance10.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            this.cboTransType.ButtonAppearance = appearance10;
            this.cboTransType.ButtonStyle = Infragistics.Win.UIElementButtonStyle.FlatBorderless;
            this.cboTransType.DropDownStyle = Infragistics.Win.DropDownStyle.DropDownList;
            this.cboTransType.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboTransType.LimitToList = true;
            this.cboTransType.Location = new System.Drawing.Point(146, 135);
            this.cboTransType.Name = "cboTransType";
            this.cboTransType.NullText = "Select";
            this.cboTransType.Size = new System.Drawing.Size(192, 23);
            this.cboTransType.TabIndex = 5;
            this.cboTransType.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.cboTransType.Visible = false;
            // 
            // dtpToDate
            // 
            this.dtpToDate.AllowNull = false;
            appearance11.FontData.BoldAsString = "False";
            appearance11.FontData.ItalicAsString = "False";
            appearance11.FontData.StrikeoutAsString = "False";
            appearance11.FontData.UnderlineAsString = "False";
            appearance11.ForeColor = System.Drawing.Color.Black;
            appearance11.ForeColorDisabled = System.Drawing.Color.Black;
            this.dtpToDate.Appearance = appearance11;
            this.dtpToDate.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Office2003ToolbarButton;
            this.dtpToDate.DateButtons.Add(dateButton1);
            this.dtpToDate.Location = new System.Drawing.Point(146, 52);
            this.dtpToDate.Name = "dtpToDate";
            this.dtpToDate.NonAutoSizeHeight = 10;
            this.dtpToDate.Size = new System.Drawing.Size(192, 21);
            this.dtpToDate.TabIndex = 2;
            this.dtpToDate.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.dtpToDate.Value = new System.DateTime(2004, 5, 25, 0, 0, 0, 0);
            this.dtpToDate.ValueChanged += new System.EventHandler(this.dtpToDate_ValueChanged);
            // 
            // dtpFromDate
            // 
            this.dtpFromDate.AllowNull = false;
            appearance12.FontData.BoldAsString = "False";
            appearance12.FontData.ItalicAsString = "False";
            appearance12.FontData.StrikeoutAsString = "False";
            appearance12.FontData.UnderlineAsString = "False";
            appearance12.ForeColor = System.Drawing.Color.Black;
            appearance12.ForeColorDisabled = System.Drawing.Color.Black;
            this.dtpFromDate.Appearance = appearance12;
            this.dtpFromDate.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Office2003ToolbarButton;
            this.dtpFromDate.DateButtons.Add(dateButton2);
            this.dtpFromDate.Location = new System.Drawing.Point(146, 24);
            this.dtpFromDate.Name = "dtpFromDate";
            this.dtpFromDate.NonAutoSizeHeight = 10;
            this.dtpFromDate.Size = new System.Drawing.Size(192, 21);
            this.dtpFromDate.TabIndex = 0;
            this.dtpFromDate.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.dtpFromDate.Value = new System.DateTime(2004, 5, 25, 0, 0, 0, 0);
            this.dtpFromDate.ValueChanged += new System.EventHandler(this.dtpFromDate_ValueChanged);
            // 
            // txtItemCode
            // 
            this.txtItemCode.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance13.Image = ((object)(resources.GetObject("appearance13.Image")));
            editorButton1.Appearance = appearance13;
            editorButton1.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Office2003ToolbarButton;
            editorButton1.Text = "";
            this.txtItemCode.ButtonsRight.Add(editorButton1);
            this.txtItemCode.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtItemCode.Location = new System.Drawing.Point(146, 108);
            this.txtItemCode.MaxLength = 20;
            this.txtItemCode.Name = "txtItemCode";
            this.txtItemCode.Size = new System.Drawing.Size(192, 20);
            this.txtItemCode.TabIndex = 4;
            this.txtItemCode.EditorButtonClick += new Infragistics.Win.UltraWinEditors.EditorButtonEventHandler(this.txtItemCode_EditorButtonClick);
            // 
            // txtVendorCode
            // 
            this.txtVendorCode.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance14.Image = ((object)(resources.GetObject("appearance14.Image")));
            editorButton2.Appearance = appearance14;
            editorButton2.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Office2003ToolbarButton;
            editorButton2.Text = "";
            this.txtVendorCode.ButtonsRight.Add(editorButton2);
            this.txtVendorCode.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtVendorCode.Location = new System.Drawing.Point(146, 80);
            this.txtVendorCode.MaxLength = 20;
            this.txtVendorCode.Name = "txtVendorCode";
            this.txtVendorCode.Size = new System.Drawing.Size(192, 20);
            this.txtVendorCode.TabIndex = 3;
            this.txtVendorCode.EditorButtonClick += new Infragistics.Win.UltraWinEditors.EditorButtonEventHandler(this.txtVendorCode_EditorButtonClick);
            // 
            // ultraLabel11
            // 
            this.ultraLabel11.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultraLabel11.Location = new System.Drawing.Point(24, 111);
            this.ultraLabel11.Name = "ultraLabel11";
            this.ultraLabel11.Size = new System.Drawing.Size(91, 15);
            this.ultraLabel11.TabIndex = 4;
            this.ultraLabel11.Text = "Item Code";
            // 
            // ultraLabel12
            // 
            this.ultraLabel12.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultraLabel12.Location = new System.Drawing.Point(24, 83);
            this.ultraLabel12.Name = "ultraLabel12";
            this.ultraLabel12.Size = new System.Drawing.Size(91, 15);
            this.ultraLabel12.TabIndex = 3;
            this.ultraLabel12.Text = "Vendor Code";
            // 
            // ultraLabel13
            // 
            this.ultraLabel13.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultraLabel13.Location = new System.Drawing.Point(24, 55);
            this.ultraLabel13.Name = "ultraLabel13";
            this.ultraLabel13.Size = new System.Drawing.Size(91, 15);
            this.ultraLabel13.TabIndex = 2;
            this.ultraLabel13.Text = "To Date";
            // 
            // ultraLabel14
            // 
            this.ultraLabel14.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultraLabel14.Location = new System.Drawing.Point(24, 27);
            this.ultraLabel14.Name = "ultraLabel14";
            this.ultraLabel14.Size = new System.Drawing.Size(91, 15);
            this.ultraLabel14.TabIndex = 1;
            this.ultraLabel14.Text = "From Date";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.ultraButton1);
            this.groupBox2.Controls.Add(this.btnClose);
            this.groupBox2.Controls.Add(this.btnView);
            this.groupBox2.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.groupBox2.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.Location = new System.Drawing.Point(15, 339);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(395, 57);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            // 
            // ultraButton1
            // 
            appearance15.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(62)))), ((int)(((byte)(176)))));
            appearance15.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance15.FontData.BoldAsString = "True";
            appearance15.ForeColor = System.Drawing.Color.White;
            appearance15.Image = ((object)(resources.GetObject("appearance15.Image")));
            this.ultraButton1.Appearance = appearance15;
            this.ultraButton1.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            this.ultraButton1.Location = new System.Drawing.Point(69, 19);
            this.ultraButton1.Name = "ultraButton1";
            this.ultraButton1.Size = new System.Drawing.Size(85, 26);
            this.ultraButton1.TabIndex = 0;
            this.ultraButton1.Text = "&Print";
            this.ultraButton1.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.ultraButton1.Click += new System.EventHandler(this.ultraButton1_Click);
            // 
            // btnClose
            // 
            appearance16.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(62)))), ((int)(((byte)(176)))));
            appearance16.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance16.FontData.BoldAsString = "True";
            appearance16.ForeColor = System.Drawing.Color.White;
            appearance16.Image = ((object)(resources.GetObject("appearance16.Image")));
            this.btnClose.Appearance = appearance16;
            this.btnClose.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(253, 20);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(85, 26);
            this.btnClose.TabIndex = 2;
            this.btnClose.Text = "&Close";
            this.btnClose.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnView
            // 
            appearance17.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(62)))), ((int)(((byte)(176)))));
            appearance17.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance17.FontData.BoldAsString = "True";
            appearance17.ForeColor = System.Drawing.Color.White;
            appearance17.Image = ((object)(resources.GetObject("appearance17.Image")));
            this.btnView.Appearance = appearance17;
            this.btnView.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            this.btnView.Location = new System.Drawing.Point(161, 20);
            this.btnView.Name = "btnView";
            this.btnView.Size = new System.Drawing.Size(85, 26);
            this.btnView.TabIndex = 1;
            this.btnView.Text = "&View";
            this.btnView.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnView.Click += new System.EventHandler(this.btnView_Click);
            // 
            // lblTransactionType
            // 
            appearance18.TextHAlignAsString = "Center";
            this.lblTransactionType.Appearance = appearance18;
            this.lblTransactionType.BackColorInternal = System.Drawing.Color.Transparent;
            this.lblTransactionType.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblTransactionType.Font = new System.Drawing.Font("Arial", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTransactionType.Location = new System.Drawing.Point(0, 0);
            this.lblTransactionType.Name = "lblTransactionType";
            this.lblTransactionType.Size = new System.Drawing.Size(424, 30);
            this.lblTransactionType.TabIndex = 31;
            this.lblTransactionType.Tag = "Header";
            this.lblTransactionType.Text = "Inventory Received Report";
            // 
            // lblMessage
            // 
            appearance19.ForeColor = System.Drawing.Color.Red;
            appearance19.TextHAlignAsString = "Center";
            this.lblMessage.Appearance = appearance19;
            this.lblMessage.BackColorInternal = System.Drawing.Color.Transparent;
            this.lblMessage.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblMessage.Font = new System.Drawing.Font("Times New Roman", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMessage.Location = new System.Drawing.Point(0, 401);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.Size = new System.Drawing.Size(424, 20);
            this.lblMessage.TabIndex = 33;
            this.lblMessage.Tag = "NOCOLOR";
            this.lblMessage.Text = "cost price is hidden due to the user does not have enough permissions";
            this.lblMessage.Visible = false;
            // 
            // frmRptInventoryReceived
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(424, 421);
            this.Controls.Add(this.lblMessage);
            this.Controls.Add(this.lblTransactionType);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.gbInventoryReceived);
            this.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmRptInventoryReceived";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Tag = "Header";
            this.Text = "Inventory Received Report";
            this.Load += new System.EventHandler(this.frmInventoryReports_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmRptInventoryReceived_KeyDown);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.frmRptInventoryReceived_KeyUp);
            this.gbInventoryReceived.ResumeLayout(false);
            this.gbInventoryReceived.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtRefferenceNo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboSecondarySort)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboPrimarySort)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboUsers)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboTransType)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtpToDate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtpFromDate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtItemCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtVendorCode)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);

		}
		#endregion

		private void frmInventoryReports_Load(object sender, System.EventArgs e)
		{
			clsUIHelper.setColorSchecme(this);
            dtpToDate.Value = DateTime.Now;
            //Following Code Added by Krishna on 11 July 2011
            DateTime dt = DateTime.SpecifyKind((DateTime)dtpToDate.Value, DateTimeKind.Local);
            dt = dt.AddDays(-1);
            //Till here Added by Krishna on 11 July 2011
            dtpFromDate.Value = dt;
            
            switch (mReportType)
            {
                case InventoryReceivedEnum.InventoryReceive:
                    this.lblTransactionType.Text = this.Text = "Inventory Received";
                    //Added By Amit Date 22 Nov 2011
                    populateTransType();
                    this.ultraLabel2.Visible = true;
                    this.cboTransType.Visible = true;
                    //End
                    //Added by krishna on 24 November 2011
                    lblUser.Visible = true;
                    cboUsers.Visible = true;
                    PopulateUsers();
                    this.ultraLabel1.Visible = true;   //Sprint-21 10-Jul-2015 JY Unhide the control as hidden for other report 
                    this.txtRefferenceNo.Visible = true;   //Sprint-21 10-Jul-2015 JY Unhide the control as hidden for other report 
                    //Below Code Added by Krishna on 30 November 2011  
                    this.lblPrimarySort.Visible = true;
                    this.lblSecSort.Visible = true;
                    this.cboPrimarySort.Visible = true;
                    this.cboSecondarySort.Visible = true;
                    this.cboPrimarySort.SelectedIndex = 0;
                    this.SetSecondarySort();
                    //End of krishna 
                    break;

                case InventoryReceivedEnum.ItemListByVendor:
                    this.gbInventoryReceived.Text = "Item List By Vendor";
                    this.lblTransactionType.Text = this.Text = "Item List By Vendor";
                    this.ultraLabel11.Visible = false;
                    this.txtItemCode.Visible = false;
                    this.ultraLabel2.Visible = false;
                    this.cboTransType.Visible = false;
                    this.ultraLabel1.Visible = false;   //Sprint-21 10-Jul-2015 JY Hide the control as not in use for this report
                    this.txtRefferenceNo.Visible = false;   //Sprint-21 10-Jul-2015 JY Hide the control as not in use for this report
                    break;
            }

			this.txtItemCode.AfterEnterEditMode += new System.EventHandler(clsUIHelper.AfterEnterEditMode);			
			this.txtItemCode.AfterExitEditMode += new System.EventHandler(clsUIHelper.AfterExitEditMode);			

			this.txtVendorCode.AfterEnterEditMode += new System.EventHandler(clsUIHelper.AfterEnterEditMode);			
			this.txtVendorCode.AfterExitEditMode += new System.EventHandler(clsUIHelper.AfterExitEditMode);			

			this.dtpFromDate.Enter+= new System.EventHandler(clsUIHelper.AfterEnterEditMode);			
			this.dtpFromDate.Leave+= new System.EventHandler(clsUIHelper.AfterExitEditMode);			
			
			this.dtpToDate.Enter+= new System.EventHandler(clsUIHelper.AfterEnterEditMode);			
			this.dtpToDate.Leave+= new System.EventHandler(clsUIHelper.AfterExitEditMode);

            this.txtRefferenceNo.Enter += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
            this.txtRefferenceNo.Leave += new System.EventHandler(clsUIHelper.AfterExitEditMode);

            //Commented by Amit Date 22 Nov 2011
            //populateTransType();

            #region PRIMEPOS-2464 10-Mar-2020 JY Added
            nDisplayItemCost = Configuration.convertBoolToInt(UserPriviliges.IsUserHasPriviliges(UserPriviliges.Modules.InventoryMgmt.ID, UserPriviliges.Screens.ItemFile.ID, UserPriviliges.Permissions.DisplayItemCost.ID));   //PRIMEPOS-2464 09-Mar-2020 JY Added
            if (nDisplayItemCost == 0)
            {
                lblMessage.Visible = true;
                tmrBlinking = new System.Timers.Timer();
                tmrBlinking.Interval = 1000;//1 seconds
                tmrBlinking.Elapsed += new ElapsedEventHandler(tmrBlinkingTimedEvent);
                tmrBlinking.Enabled = true;
            }
            else
            {
                this.Height -= 15;
            }
            #endregion
        }

        private void populateTransType()
		{
            //Uncommented By Amit Date 22 Nov 2011
			try
			{
				InvTransTypeData oInvTransTypeData=new InvTransTypeData();
				InvTransTypeSvr oInvTransType=new InvTransTypeSvr();

				oInvTransTypeData=oInvTransType.PopulateList("");

				if (oInvTransTypeData.InvTransType.Rows.Count==0)
				{
					InvTransTypeRow oNewRow= oInvTransTypeData.InvTransType.AddRow(0,"",0,"");

					oNewRow.TypeName="Inventory Received";
					oNewRow.TransType=0;
					oNewRow.UserID=POS_Core.Resources.Configuration.UserName;

					oInvTransType.Persist(oInvTransTypeData);

					oInvTransTypeData=oInvTransType.PopulateList("");
				}
				
				Infragistics.Win.ValueListItem oVl=new Infragistics.Win.ValueListItem();

				this.cboTransType.Items.Clear();

                //Added By Amit Date 22 Nov 2011
                cboTransType.Items.Insert(0,0,"All");
                //End

				foreach (InvTransTypeRow oDRow in oInvTransTypeData.InvTransType.Rows)
				{
					this.cboTransType.Items.Add(oDRow.ID.ToString(),oDRow.TypeName.ToString());
				}

				this.cboTransType.SelectedIndex=0;
			}
			catch(Exception exp) 
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
            //End
		}

		private void btnPreview_Click(object sender, System.EventArgs e)
		{
			Preview(false);
		}

		private void btnPrint_Click(object sender, System.EventArgs e)
		{
			Preview(true);
		}

		private void Preview(bool PrintId)
		{
			try
            {
                ReportClass oRpt = null;
                string sSQL = "";

                switch (mReportType)
                {
                    //inventoryreceived oRpt =new inventoryreceived();
                    case InventoryReceivedEnum.InventoryReceive:
                        oRpt = new inventoryreceived();
                        sSQL = "SELECT " +
                                    " ven.VendorCode " +
                                    " , IR.RecieveDate " +
                                    " , IR.UserID " +
                                    " , itm.ItemId " +
                                    " , itm.Description  " +
                                    " , Detail.QtyOrdered" +
                                    " , Detail.Qty  " +
                                    " , Detail.Cost  " +
                                    " , Detail.SalePrice " +
                                    " , Detail.Qty * Detail.Cost As TotalCost " +
                                    " , Detail.SalePrice * Detail.Qty As TotalRetail " +
                                    " , Detail.Comments " +
                                    " , itt.TypeName " +//itt.TypeName Added by Krishna on 1 December 2011
                                    " , IR.RefNo " +//itt.TypeName Added by Krishna on 1 December 2011
                                    ", " + nDisplayItemCost + " AS DisplayItemCost" +   //PRIMEPOS-2464 09-Mar-2020 JY Added
                                " FROM  " +
                                    " InventoryRecieved IR " +
                                    " , InvRecievedDetail Detail " +
                                    " , Vendor ven " +
                                    " , Item itm " +
                                    " , InvTransType itt"+
                                " WHERE " +
                                    "Detail.InvRecievedID = IR.InvRecievedID " +
                                    "and ven.VendorID = IR.VendorId " +
                                    "and Detail.ItemId = itm.ItemId " +
                                    "AND IR.INVTRANSTYPEID=itt.ID";//AND IR.INVTRANSTYPEID=itt.TransType added by Krishna on 1 December 2011
                        //changes IR.INVTRANSTYPEID=itt.TransType to IR.INVTRANSTYPEID=itt.ID by shitaljit on 21 Feb 2012

                        //Following Coded Added by Krishna on 1 December 2011
                        sSQL = sSQL + buildCriteria();
                        frmReportViewer oRpt1 = new frmReportViewer();
                        oRpt.Database.Tables[0].SetDataSource(clsReports.GetReportSource(sSQL).Tables[0]);
                        clsReports.SetRepParam(oRpt, "GroupByParam", cboPrimarySort.SelectedItem.Tag.ToString());
                        clsReports.DStoExport = clsReports.GetReportSource(sSQL);   //PRIMEPOS-2471 16-Feb-2021 JY Added
                        clsReports.Preview(PrintId, oRpt);
                        //End of Added by Krishna
                        break;
                    //Added By Abhishek(SRT)
                    case InventoryReceivedEnum.ItemListByVendor:
                        oRpt = new rptItemListByVendor();
                        sSQL = "SELECT " +
                               " itm.ItemID " +
                               " , itmVen.VendorItemID " +
                               " , itm.Description " +
                               " , itm.Unit  " +
                               " , itm.Location " +
                               " , itm.QtyInStock as Qty" +
                               " , itm.SellingPrice as Cost" +
                               " , itm.Discount " +
                               " , itm.ExptDeliveryDate " +
                               " , itm.ReOrderLevel  " +
                               " , itm.SaleEndDate  " +
                               " , itm.SaleStartDate " +
                               " , itm.ProductCode " +
                               " , ven.VendorCode " +
                               ", " + nDisplayItemCost + " AS DisplayItemCost" +   //PRIMEPOS-2464 09-Mar-2020 JY Added
                                // " FROM  Item itm INNER JOIN  ItemVendor itmVen ON itm.ItemID = itmVen.ItemID INNER JOIN  Vendor  ven ON itmVen.VendorID = ven.VendorID";
                               " FROM  Item itm LEFT OUTER JOIN  ItemVendor itmVen ON itm.ItemID = itmVen.ItemID " +
                               " LEFT OUTER JOIN  Vendor  ven ON itm.LastVendor = ven.VendorCode OR itm.PREFERREDVENDOR = ven.VendorCode WHERE 1=1 ";
                        break;                   
                }
                if (mReportType != InventoryReceivedEnum.InventoryReceive)//If condition  Added by Krishna on 1 December 2011
                {
                    sSQL = sSQL + buildCriteria();
                    clsReports.Preview(PrintId, sSQL, oRpt);
                }
            }
			catch(Exception exp)
			{
				clsUIHelper.ShowErrorMsg(exp.Message);
			}
		}

		private string buildCriteria()
		{
			string sCriteria = "";
            //Modified by shitaljit to fixed bug in Item List By Vendor Report.
            if (mReportType == InventoryReceivedEnum.InventoryReceive)
            {
                if (dtpFromDate.Value.ToString() != "")
                    sCriteria = sCriteria + " AND Convert(smalldatetime,Convert(Varchar,IR.RecieveDate,107)) >= '" + dtpFromDate.Text + "'";    //Sprint-21 09-Jul-2015 JY Added alias to avoid column ambigious error

                if (dtpToDate.Value.ToString() != "")
                    sCriteria = sCriteria + " AND Convert(smalldatetime,Convert(Varchar,IR.RecieveDate,107)) <= '" + dtpToDate.Text + "'";  //Sprint-21 09-Jul-2015 JY Added alias to avoid column ambigious error

                //Added by Amit Date 22 Nov 2011
                if (cboTransType.SelectedIndex > 0)
                    sCriteria = sCriteria + " AND IR.InvTransTypeID = (Select ITT.ID From InvTransType ITT Where ITT.ID =" + cboTransType.Value.ToString() + " )";
                if (cboUsers.SelectedIndex > 0)
                    sCriteria = sCriteria + " AND IR.UserId= '" + cboUsers.Value.ToString() + "' ";

                if (txtItemCode.Text.Trim().Replace("'", "''") != "")
                    sCriteria = sCriteria + " AND itm.ItemId = '" + txtItemCode.Text + "'";
                //Added By Shitaljit for PRIMEPOS-1320 Add filter to group Inventory by reference on the Inventory Received Report.
                if (txtRefferenceNo.Text.Trim().Replace("'", "''") != "")
                {
                    sCriteria = sCriteria + " AND IR.RefNo = '" + txtRefferenceNo.Text + "'";
                }
                //End
            }

            if (mReportType == InventoryReceivedEnum.ItemListByVendor)
            {
                if (dtpFromDate.Value.ToString() != "")
                    sCriteria = sCriteria + " AND Convert(smalldatetime,Convert(Varchar,itm.SaleStartDate,107)) >= '" + dtpFromDate.Text + "'"; //Sprint-21 09-Jul-2015 JY Added alias to resolve column ambigious error

                if (dtpToDate.Value.ToString() != "")
                    sCriteria = sCriteria + " AND Convert(smalldatetime,Convert(Varchar,itm.SaleEndDate,107)) <= '" + dtpToDate.Text + "'"; //Sprint-21 09-Jul-2015 JY Added alias to resolve column ambigious error

            }

			if (txtVendorCode.Text.Trim().Replace("'","''")!="")
                sCriteria = sCriteria + " AND ven.VendorCode = '" + txtVendorCode.Text + "'";   //Sprint-21 09-Jul-2015 JY Added alias to avoid column ambigious error

           
            //Following Code Added by Krishna on 30 November 2011
            if (mReportType == InventoryReceivedEnum.InventoryReceive)
            {
                String OrderByQry = String.Empty;
                if (cboSecondarySort.SelectedItem.Tag.ToString() == "RecieveDate")
                    OrderByQry += "Order by IR." + cboSecondarySort.SelectedItem.Tag.ToString() + " Desc";//Trans   //Sprint-21 10-Jul-2015 JY Added alias to avoid column ambigious error

                else if (cboSecondarySort.SelectedItem.Tag.ToString() == "UserID")
                    OrderByQry = " Order by IR." + cboSecondarySort.SelectedItem.Tag.ToString() + "";//UserID

                else if (cboSecondarySort.SelectedItem.Tag.ToString() == "InvTransTypeID")
                    OrderByQry = " Order by IR." + cboSecondarySort.SelectedItem.Tag.ToString() + "";//InvTransTypeID

                else if (cboSecondarySort.SelectedItem.Tag.ToString() == "VendorCode")
                    OrderByQry = " Order by ven." + cboSecondarySort.SelectedItem.Tag.ToString() + "";//VendorCode  //Sprint-21 10-Jul-2015 JY Added alias to avoid column ambigious error

                else if (cboSecondarySort.SelectedItem.Tag.ToString() == "ItemID")
                    OrderByQry = " Order by itm." + cboSecondarySort.SelectedItem.Tag.ToString() + "";//ItemID
                else if (cboSecondarySort.SelectedItem.Tag.ToString() == "RefNo")
                    OrderByQry = " Order by IR." + cboSecondarySort.SelectedItem.Tag.ToString() + "";//RefNo

                sCriteria = String.Concat(sCriteria, OrderByQry);
            }
            //End of Added by Krishna on 30 November 2011

			return sCriteria;
		}

		private void btnClose_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}

		private void dtpToDate_BeforeDropDown(object sender, System.ComponentModel.CancelEventArgs e)
		{
		
		}

		private void frmRptInventoryReceived_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
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

		private void gbInventoryReceived_Enter(object sender, System.EventArgs e)
		{
			dtpFromDate.Focus();
		}

		private void gbInventoryReceived_Enter_1(object sender, System.EventArgs e)
		{
		   
        }
        private bool validateFields(out string fieldName)
        {
            bool isValid = true;
            string field = string.Empty;
            try
            {
                if ((DateTime)dtpFromDate.Value > (DateTime)dtpToDate.Value)
                {
                    isValid = false;
                    fieldName = "DATE";
                    return isValid;
                }
            }
            catch (Exception ex)
            {
                clsUIHelper.ShowErrorMsg(ex.Message);
            }
            fieldName = field;
            return isValid;
        }
		private void btnView_Click(object sender, System.EventArgs e)
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
                  this.dtpFromDate.Focus();
                  Preview(false);
              }
              catch (Exception ex)
              {
                  clsUIHelper.ShowErrorMsg(ex.Message);
              }
		}

		private void ultraButton1_Click(object sender, System.EventArgs e)
		{
			this.dtpFromDate.Focus();
			Preview(true);
		}

		private void txtVendorCode_EditorButtonClick(object sender, Infragistics.Win.UltraWinEditors.EditorButtonEventArgs e)
		{
            //frmSearch oSearch = new frmSearch(clsPOSDBConstants.Vendor_tbl);
            frmSearchMain oSearch = new frmSearchMain(true);    //20-Dec-2017 JY Added new reference
            oSearch.SearchTable = clsPOSDBConstants.Vendor_tbl; //20-Dec-2017 JY Added 
            oSearch.ShowDialog();
			if (oSearch.IsCanceled) return;
			txtVendorCode.Text = oSearch.SelectedRowID();
		}

		private void txtItemCode_EditorButtonClick(object sender, Infragistics.Win.UltraWinEditors.EditorButtonEventArgs e)
		{
            //frmSearch oSearch = new frmSearch(clsPOSDBConstants.Item_tbl);
            frmSearchMain oSearch = new frmSearchMain(true);    //20-Dec-2017 JY Added new reference
            oSearch.SearchTable = clsPOSDBConstants.Item_tbl;  //20-Dec-2017 JY Added 
            oSearch.ShowDialog();
			if (oSearch.IsCanceled) return;
			txtItemCode.Text = oSearch.SelectedRowID();
		}

		private void frmRptInventoryReceived_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			try
			{
				if (e.KeyData==System.Windows.Forms.Keys.F4)
				{
					if ( txtVendorCode.ContainsFocus)
						txtVendorCode_EditorButtonClick(null,null);
					else if ( txtItemCode.ContainsFocus)
						txtItemCode_EditorButtonClick(null,null);
				}
			}
			catch(Exception exp)
			{
				clsUIHelper.ShowErrorMsg(exp.Message);
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
                    }

                    return;
                }
            }
            catch (Exception ex)
            {
                clsUIHelper.ShowErrorMsg(ex.Message);
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
                clsUIHelper.ShowErrorMsg(ex.Message);
            }
        }

        //------------------------------------------------------------------------------------------------------------------
        private void PopulateUsers()
        {
            SearchSvr oSearchSvr = new SearchSvr();

            DataSet UserDS = oSearchSvr.Search(clsPOSDBConstants.Users_tbl, "", "", 0, -1);
            this.cboUsers.Items.Add("All");
            foreach (DataRow row in UserDS.Tables[0].Rows)
            {
                cboUsers.Items.Add(row["UserId"].ToString());
            }
            this.cboUsers.SelectedIndex = 0;
        }
        //------------------------------------------------------------------------------------------------------------------
        //Following Code Added by Krishna on 30 November 2011
        public void SetSecondarySort()
        {
            try
            {
                cboSecondarySort.Items.Clear();
                int j = 0;
                string[] ItemCollection = new string[cboPrimarySort.Items.Count];
                for (int i = 0; i < cboPrimarySort.Items.Count; i++)
                {
                    if (i != cboPrimarySort.SelectedIndex)
                    {
                        ItemCollection[i] = cboPrimarySort.Items[i].ToString();
                        cboSecondarySort.Items.Add(ItemCollection[i].ToString());
                        cboSecondarySort.Items[j].Tag = cboPrimarySort.Items[i].Tag.ToString();
                        j++;
                    }
                }
                this.cboSecondarySort.SelectedIndex = 0;
                //cboSecondarySort.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void cboPrimarySort_Leave(object sender, EventArgs e)
        {
        }

        private void cboPrimarySort_ValueChanged(object sender, EventArgs e)
        {
            SetSecondarySort();
        }
        //End of Added by Krishna on 30 November 2011

        #region PRIMEPOS-2464 10-Mar-2020 JY Added
        public void tmrBlinkingTimedEvent(object source, ElapsedEventArgs e)
        {
            try
            {
                iBlinkCnt++;
                if (iBlinkCnt % 4 == 0)
                    lblMessage.Appearance.ForeColor = Color.Transparent;
                else
                    lblMessage.Appearance.ForeColor = Color.Red;
            }
            catch (Exception Ex)
            {
            }
        }
        #endregion
    }
}
