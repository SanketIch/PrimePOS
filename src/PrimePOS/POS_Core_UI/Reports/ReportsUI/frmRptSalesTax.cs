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
using POS_Core.BusinessRules;
using Infragistics.Win.UltraWinGrid;
using System.Data;
using POS_Core.DataAccess;

namespace POS_Core_UI.Reports.ReportsUI
{
    /// <summary>
    /// Summary description for frmInventoryReports.
    /// </summary>
    public class frmRptSalesTax : System.Windows.Forms.Form, ICommandLIneTaskControl
    {
        #region Declaration
        TaxCodes oTaxCodes = new TaxCodes();
        TaxCodesData oTaxCodesData;
        TaxCodesRow oTaxCodesRow = null;
        #endregion

        private System.Windows.Forms.GroupBox gbInventoryReceived;
		private Infragistics.Win.UltraWinSchedule.UltraCalendarCombo dtpToDate;
		private Infragistics.Win.UltraWinSchedule.UltraCalendarCombo dtpFromDate;
		private Infragistics.Win.Misc.UltraLabel ultraLabel13;
		private Infragistics.Win.Misc.UltraLabel ultraLabel14;
		private System.Windows.Forms.GroupBox groupBox2;
		private Infragistics.Win.Misc.UltraButton btnPrint;
		private Infragistics.Win.Misc.UltraButton btnView;
		private Infragistics.Win.Misc.UltraButton btnClose;
		private Infragistics.Win.Misc.UltraLabel lblTransactionType;
        private Infragistics.Win.Misc.UltraLabel ultraLabel1;
        private Infragistics.Win.Misc.UltraPanel pnlMain;
        private Infragistics.Win.Misc.UltraButton btnPayTypeList;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor txtPaymentType;
        private Infragistics.Win.Misc.UltraLabel ultraLabel4;
        private Infragistics.Win.Misc.UltraLabel lblSubDepartment;
        private Infragistics.Win.Misc.UltraLabel ultraLabel2;
        private Infragistics.Win.Misc.UltraLabel ultraLabel12;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor txtUserID;
        public Infragistics.Win.UltraWinEditors.UltraTextEditor txtEODId;
        private Infragistics.Win.Misc.UltraLabel lblEODId;
        private Infragistics.Win.Misc.UltraLabel lblCloseStationId;
        public Infragistics.Win.UltraWinEditors.UltraTextEditor txtCloseStationId;
        public Infragistics.Win.UltraWinEditors.UltraTextEditor txtSubDepartment;
        public Infragistics.Win.UltraWinEditors.UltraTextEditor txtDepartment;
        private GroupBox grpPayTypeList;
        private CheckBox chkSelectAll;
        private DataGridView dataGridList;
        private DataGridViewCheckBoxColumn chkBox;
        public Infragistics.Win.UltraWinEditors.UltraTextEditor txtTaxCode;
        private Infragistics.Win.UltraWinEditors.UltraOptionSet optionSetForItems;

        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;
        private Infragistics.Win.Misc.UltraButton btnReset;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor txtStationID;
        private Infragistics.Win.Misc.UltraLabel ultraLabel21;
        private bool GridVisibleFlag = false;   //PRIMEPOS-2436 18-Aug-2020 JY Added

        public frmRptSalesTax()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
            //
            // TODO: Add any constructor code after InitializeComponent call
            //
            this.customControl = new usrDateRangeParams();  //PRIMEPOS-2485 01-Apr-2021 JY Added
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            Infragistics.Win.UltraWinEditors.EditorButton editorButton1 = new Infragistics.Win.UltraWinEditors.EditorButton();
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinEditors.EditorButton editorButton2 = new Infragistics.Win.UltraWinEditors.EditorButton();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinEditors.EditorButton editorButton3 = new Infragistics.Win.UltraWinEditors.EditorButton();
            Infragistics.Win.Appearance appearance7 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance8 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance9 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinEditors.EditorButton editorButton4 = new Infragistics.Win.UltraWinEditors.EditorButton();
            Infragistics.Win.Appearance appearance10 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance11 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinSchedule.CalendarCombo.DateButton dateButton1 = new Infragistics.Win.UltraWinSchedule.CalendarCombo.DateButton();
            Infragistics.Win.Appearance appearance12 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinSchedule.CalendarCombo.DateButton dateButton2 = new Infragistics.Win.UltraWinSchedule.CalendarCombo.DateButton();
            Infragistics.Win.ValueListItem valueListItem1 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem2 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem3 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.UltraWinEditors.EditorButton editorButton5 = new Infragistics.Win.UltraWinEditors.EditorButton();
            Infragistics.Win.Appearance appearance13 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance14 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance15 = new Infragistics.Win.Appearance();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmRptSalesTax));
            Infragistics.Win.Appearance appearance16 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance17 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance18 = new Infragistics.Win.Appearance();
            this.gbInventoryReceived = new System.Windows.Forms.GroupBox();
            this.txtStationID = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.ultraLabel21 = new Infragistics.Win.Misc.UltraLabel();
            this.grpPayTypeList = new System.Windows.Forms.GroupBox();
            this.chkSelectAll = new System.Windows.Forms.CheckBox();
            this.dataGridList = new System.Windows.Forms.DataGridView();
            this.chkBox = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.ultraLabel1 = new Infragistics.Win.Misc.UltraLabel();
            this.txtSubDepartment = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.txtDepartment = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.btnPayTypeList = new Infragistics.Win.Misc.UltraButton();
            this.txtPaymentType = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.ultraLabel4 = new Infragistics.Win.Misc.UltraLabel();
            this.lblSubDepartment = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabel2 = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabel12 = new Infragistics.Win.Misc.UltraLabel();
            this.txtUserID = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.txtEODId = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.lblEODId = new Infragistics.Win.Misc.UltraLabel();
            this.lblCloseStationId = new Infragistics.Win.Misc.UltraLabel();
            this.txtCloseStationId = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.dtpToDate = new Infragistics.Win.UltraWinSchedule.UltraCalendarCombo();
            this.dtpFromDate = new Infragistics.Win.UltraWinSchedule.UltraCalendarCombo();
            this.ultraLabel13 = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabel14 = new Infragistics.Win.Misc.UltraLabel();
            this.optionSetForItems = new Infragistics.Win.UltraWinEditors.UltraOptionSet();
            this.txtTaxCode = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnReset = new Infragistics.Win.Misc.UltraButton();
            this.btnPrint = new Infragistics.Win.Misc.UltraButton();
            this.btnClose = new Infragistics.Win.Misc.UltraButton();
            this.btnView = new Infragistics.Win.Misc.UltraButton();
            this.lblTransactionType = new Infragistics.Win.Misc.UltraLabel();
            this.pnlMain = new Infragistics.Win.Misc.UltraPanel();
            this.gbInventoryReceived.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtStationID)).BeginInit();
            this.grpPayTypeList.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSubDepartment)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDepartment)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPaymentType)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtUserID)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtEODId)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCloseStationId)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtpToDate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtpFromDate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.optionSetForItems)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTaxCode)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.pnlMain.ClientArea.SuspendLayout();
            this.pnlMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbInventoryReceived
            // 
            this.gbInventoryReceived.Controls.Add(this.txtStationID);
            this.gbInventoryReceived.Controls.Add(this.ultraLabel21);
            this.gbInventoryReceived.Controls.Add(this.grpPayTypeList);
            this.gbInventoryReceived.Controls.Add(this.ultraLabel1);
            this.gbInventoryReceived.Controls.Add(this.txtSubDepartment);
            this.gbInventoryReceived.Controls.Add(this.txtDepartment);
            this.gbInventoryReceived.Controls.Add(this.btnPayTypeList);
            this.gbInventoryReceived.Controls.Add(this.txtPaymentType);
            this.gbInventoryReceived.Controls.Add(this.ultraLabel4);
            this.gbInventoryReceived.Controls.Add(this.lblSubDepartment);
            this.gbInventoryReceived.Controls.Add(this.ultraLabel2);
            this.gbInventoryReceived.Controls.Add(this.ultraLabel12);
            this.gbInventoryReceived.Controls.Add(this.txtUserID);
            this.gbInventoryReceived.Controls.Add(this.txtEODId);
            this.gbInventoryReceived.Controls.Add(this.lblEODId);
            this.gbInventoryReceived.Controls.Add(this.lblCloseStationId);
            this.gbInventoryReceived.Controls.Add(this.txtCloseStationId);
            this.gbInventoryReceived.Controls.Add(this.dtpToDate);
            this.gbInventoryReceived.Controls.Add(this.dtpFromDate);
            this.gbInventoryReceived.Controls.Add(this.ultraLabel13);
            this.gbInventoryReceived.Controls.Add(this.ultraLabel14);
            this.gbInventoryReceived.Controls.Add(this.optionSetForItems);
            this.gbInventoryReceived.Controls.Add(this.txtTaxCode);
            this.gbInventoryReceived.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbInventoryReceived.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbInventoryReceived.Location = new System.Drawing.Point(0, 30);
            this.gbInventoryReceived.Name = "gbInventoryReceived";
            this.gbInventoryReceived.Size = new System.Drawing.Size(892, 181);
            this.gbInventoryReceived.TabIndex = 0;
            this.gbInventoryReceived.TabStop = false;
            // 
            // txtStationID
            // 
            this.txtStationID.AutoSize = false;
            this.txtStationID.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.txtStationID.Location = new System.Drawing.Point(435, 24);
            this.txtStationID.MaxLength = 20;
            this.txtStationID.Name = "txtStationID";
            this.txtStationID.Size = new System.Drawing.Size(120, 20);
            this.txtStationID.TabIndex = 4;
            this.txtStationID.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // ultraLabel21
            // 
            appearance1.ForeColor = System.Drawing.Color.Black;
            this.ultraLabel21.Appearance = appearance1;
            this.ultraLabel21.AutoSize = true;
            this.ultraLabel21.Location = new System.Drawing.Point(278, 27);
            this.ultraLabel21.Name = "ultraLabel21";
            this.ultraLabel21.Size = new System.Drawing.Size(153, 15);
            this.ultraLabel21.TabIndex = 95;
            this.ultraLabel21.Text = "Station# <Blank = All>";
            // 
            // grpPayTypeList
            // 
            this.grpPayTypeList.Controls.Add(this.chkSelectAll);
            this.grpPayTypeList.Controls.Add(this.dataGridList);
            this.grpPayTypeList.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpPayTypeList.Location = new System.Drawing.Point(721, 42);
            this.grpPayTypeList.Name = "grpPayTypeList";
            this.grpPayTypeList.Size = new System.Drawing.Size(144, 110);
            this.grpPayTypeList.TabIndex = 93;
            this.grpPayTypeList.TabStop = false;
            this.grpPayTypeList.Visible = false;
            // 
            // chkSelectAll
            // 
            this.chkSelectAll.AutoSize = true;
            this.chkSelectAll.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkSelectAll.Location = new System.Drawing.Point(5, 38);
            this.chkSelectAll.Name = "chkSelectAll";
            this.chkSelectAll.Size = new System.Drawing.Size(87, 17);
            this.chkSelectAll.TabIndex = 13;
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
            this.dataGridList.Location = new System.Drawing.Point(1, 10);
            this.dataGridList.Name = "dataGridList";
            this.dataGridList.RowHeadersVisible = false;
            this.dataGridList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridList.Size = new System.Drawing.Size(144, 21);
            this.dataGridList.TabIndex = 12;
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
            // ultraLabel1
            // 
            this.ultraLabel1.AutoSize = true;
            this.ultraLabel1.Location = new System.Drawing.Point(560, 144);
            this.ultraLabel1.Name = "ultraLabel1";
            this.ultraLabel1.Size = new System.Drawing.Size(156, 15);
            this.ultraLabel1.TabIndex = 75;
            this.ultraLabel1.Text = "Tax Code <Blank = All>";
            // 
            // txtSubDepartment
            // 
            this.txtSubDepartment.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance2.BackColor = System.Drawing.Color.White;
            appearance2.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(231)))), ((int)(((byte)(230)))));
            appearance2.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance2.BorderAlpha = Infragistics.Win.Alpha.Transparent;
            appearance2.ImageAlpha = Infragistics.Win.Alpha.Opaque;
            appearance2.ImageBackgroundAlpha = Infragistics.Win.Alpha.Opaque;
            appearance2.ImageBackgroundStyle = Infragistics.Win.ImageBackgroundStyle.Centered;
            editorButton1.Appearance = appearance2;
            editorButton1.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            editorButton1.Text = "";
            editorButton1.Width = 20;
            this.txtSubDepartment.ButtonsRight.Add(editorButton1);
            this.txtSubDepartment.Location = new System.Drawing.Point(435, 141);
            this.txtSubDepartment.Name = "txtSubDepartment";
            this.txtSubDepartment.ReadOnly = true;
            this.txtSubDepartment.Size = new System.Drawing.Size(120, 20);
            this.txtSubDepartment.TabIndex = 7;
            this.txtSubDepartment.EditorButtonClick += new Infragistics.Win.UltraWinEditors.EditorButtonEventHandler(this.txtSubDepartment_EditorButtonClick);
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
            editorButton2.Appearance = appearance3;
            editorButton2.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            editorButton2.Text = "";
            editorButton2.Width = 20;
            this.txtDepartment.ButtonsRight.Add(editorButton2);
            this.txtDepartment.Location = new System.Drawing.Point(435, 102);
            this.txtDepartment.Name = "txtDepartment";
            this.txtDepartment.ReadOnly = true;
            this.txtDepartment.Size = new System.Drawing.Size(120, 20);
            this.txtDepartment.TabIndex = 6;
            this.txtDepartment.EditorButtonClick += new Infragistics.Win.UltraWinEditors.EditorButtonEventHandler(this.txtDepartment_EditorButtonClick);
            this.txtDepartment.TextChanged += new System.EventHandler(this.txtDepartment_TextChanged);
            // 
            // btnPayTypeList
            // 
            appearance4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(62)))), ((int)(((byte)(176)))));
            appearance4.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance4.FontData.BoldAsString = "True";
            appearance4.ForeColor = System.Drawing.Color.White;
            this.btnPayTypeList.Appearance = appearance4;
            this.btnPayTypeList.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            this.btnPayTypeList.Location = new System.Drawing.Point(720, 24);
            this.btnPayTypeList.Name = "btnPayTypeList";
            this.btnPayTypeList.Size = new System.Drawing.Size(144, 23);
            this.btnPayTypeList.TabIndex = 8;
            this.btnPayTypeList.Text = "Pay Types";
            this.btnPayTypeList.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnPayTypeList.Click += new System.EventHandler(this.btnPayTypeList_Click);
            this.btnPayTypeList.KeyDown += new System.Windows.Forms.KeyEventHandler(this.btnPayTypeList_KeyDown);
            this.btnPayTypeList.KeyUp += new System.Windows.Forms.KeyEventHandler(this.btnPayTypeList_KeyUp);
            // 
            // txtPaymentType
            // 
            this.txtPaymentType.AutoSize = false;
            this.txtPaymentType.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.txtPaymentType.Location = new System.Drawing.Point(560, 63);
            this.txtPaymentType.Name = "txtPaymentType";
            this.txtPaymentType.ReadOnly = true;
            this.txtPaymentType.Size = new System.Drawing.Size(319, 20);
            this.txtPaymentType.TabIndex = 87;
            this.txtPaymentType.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.txtPaymentType.EditorButtonClick += new Infragistics.Win.UltraWinEditors.EditorButtonEventHandler(this.txtPaymentType_EditorButtonClick);
            // 
            // ultraLabel4
            // 
            appearance5.ForeColor = System.Drawing.Color.Black;
            this.ultraLabel4.Appearance = appearance5;
            this.ultraLabel4.AutoSize = true;
            this.ultraLabel4.Location = new System.Drawing.Point(560, 27);
            this.ultraLabel4.Name = "ultraLabel4";
            this.ultraLabel4.Size = new System.Drawing.Size(162, 15);
            this.ultraLabel4.TabIndex = 88;
            this.ultraLabel4.Text = "Pay Types <Blank = All>";
            // 
            // lblSubDepartment
            // 
            this.lblSubDepartment.AutoSize = true;
            this.lblSubDepartment.Location = new System.Drawing.Point(279, 144);
            this.lblSubDepartment.Name = "lblSubDepartment";
            this.lblSubDepartment.Size = new System.Drawing.Size(152, 15);
            this.lblSubDepartment.TabIndex = 85;
            this.lblSubDepartment.Text = "Sub Dept<Blank = All>";
            // 
            // ultraLabel2
            // 
            this.ultraLabel2.AutoSize = true;
            this.ultraLabel2.Location = new System.Drawing.Point(261, 105);
            this.ultraLabel2.Name = "ultraLabel2";
            this.ultraLabel2.Size = new System.Drawing.Size(170, 15);
            this.ultraLabel2.TabIndex = 84;
            this.ultraLabel2.Text = "Department<Blank = All>";
            // 
            // ultraLabel12
            // 
            appearance6.ForeColor = System.Drawing.Color.Black;
            this.ultraLabel12.Appearance = appearance6;
            this.ultraLabel12.AutoSize = true;
            this.ultraLabel12.Location = new System.Drawing.Point(285, 66);
            this.ultraLabel12.Name = "ultraLabel12";
            this.ultraLabel12.Size = new System.Drawing.Size(146, 15);
            this.ultraLabel12.TabIndex = 80;
            this.ultraLabel12.Text = "User ID <Blank = All>";
            // 
            // txtUserID
            // 
            this.txtUserID.AutoSize = false;
            this.txtUserID.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.txtUserID.Location = new System.Drawing.Point(435, 63);
            this.txtUserID.MaxLength = 20;
            this.txtUserID.Name = "txtUserID";
            this.txtUserID.Size = new System.Drawing.Size(120, 20);
            this.txtUserID.TabIndex = 5;
            this.txtUserID.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // txtEODId
            // 
            this.txtEODId.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance7.BackColor = System.Drawing.Color.White;
            appearance7.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(231)))), ((int)(((byte)(230)))));
            appearance7.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance7.BorderAlpha = Infragistics.Win.Alpha.Transparent;
            appearance7.ImageAlpha = Infragistics.Win.Alpha.Opaque;
            appearance7.ImageBackgroundAlpha = Infragistics.Win.Alpha.Opaque;
            appearance7.ImageBackgroundStyle = Infragistics.Win.ImageBackgroundStyle.Centered;
            editorButton3.Appearance = appearance7;
            editorButton3.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            editorButton3.Text = "";
            editorButton3.Width = 20;
            this.txtEODId.ButtonsRight.Add(editorButton3);
            this.txtEODId.Location = new System.Drawing.Point(135, 102);
            this.txtEODId.Name = "txtEODId";
            this.txtEODId.Size = new System.Drawing.Size(120, 20);
            this.txtEODId.TabIndex = 2;
            this.txtEODId.EditorButtonClick += new Infragistics.Win.UltraWinEditors.EditorButtonEventHandler(this.txtEODId_EditorButtonClick);
            this.txtEODId.TextChanged += new System.EventHandler(this.txtEODId_TextChanged);
            this.txtEODId.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtEODId_KeyPress);
            // 
            // lblEODId
            // 
            appearance8.ForeColor = System.Drawing.Color.Black;
            this.lblEODId.Appearance = appearance8;
            this.lblEODId.AutoSize = true;
            this.lblEODId.BackColorInternal = System.Drawing.Color.Transparent;
            this.lblEODId.Location = new System.Drawing.Point(92, 105);
            this.lblEODId.Name = "lblEODId";
            this.lblEODId.Size = new System.Drawing.Size(41, 15);
            this.lblEODId.TabIndex = 79;
            this.lblEODId.Text = "EOD#";
            // 
            // lblCloseStationId
            // 
            appearance9.ForeColor = System.Drawing.Color.Black;
            appearance9.TextVAlignAsString = "Middle";
            this.lblCloseStationId.Appearance = appearance9;
            this.lblCloseStationId.AutoSize = true;
            this.lblCloseStationId.BackColorInternal = System.Drawing.Color.Transparent;
            this.lblCloseStationId.Location = new System.Drawing.Point(35, 144);
            this.lblCloseStationId.Name = "lblCloseStationId";
            this.lblCloseStationId.Size = new System.Drawing.Size(98, 15);
            this.lblCloseStationId.TabIndex = 78;
            this.lblCloseStationId.Text = "Close Station#";
            // 
            // txtCloseStationId
            // 
            this.txtCloseStationId.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance10.BackColor = System.Drawing.Color.White;
            appearance10.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(231)))), ((int)(((byte)(230)))));
            appearance10.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance10.BorderAlpha = Infragistics.Win.Alpha.Transparent;
            appearance10.ImageAlpha = Infragistics.Win.Alpha.Opaque;
            appearance10.ImageBackgroundAlpha = Infragistics.Win.Alpha.Opaque;
            appearance10.ImageBackgroundStyle = Infragistics.Win.ImageBackgroundStyle.Centered;
            editorButton4.Appearance = appearance10;
            editorButton4.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            editorButton4.Text = "";
            editorButton4.Width = 20;
            this.txtCloseStationId.ButtonsRight.Add(editorButton4);
            this.txtCloseStationId.Location = new System.Drawing.Point(135, 141);
            this.txtCloseStationId.Name = "txtCloseStationId";
            this.txtCloseStationId.Size = new System.Drawing.Size(120, 20);
            this.txtCloseStationId.TabIndex = 3;
            this.txtCloseStationId.EditorButtonClick += new Infragistics.Win.UltraWinEditors.EditorButtonEventHandler(this.txtCloseStationId_EditorButtonClick);
            this.txtCloseStationId.TextChanged += new System.EventHandler(this.txtCloseStationId_TextChanged);
            this.txtCloseStationId.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtCloseStationId_KeyPress);
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
            this.dtpToDate.AutoSize = false;
            this.dtpToDate.ButtonStyle = Infragistics.Win.UIElementButtonStyle.PopupSoft;
            this.dtpToDate.DateButtons.Add(dateButton1);
            this.dtpToDate.Location = new System.Drawing.Point(135, 63);
            this.dtpToDate.Name = "dtpToDate";
            this.dtpToDate.NonAutoSizeHeight = 10;
            this.dtpToDate.Size = new System.Drawing.Size(120, 20);
            this.dtpToDate.TabIndex = 1;
            this.dtpToDate.Tag = "To Date";
            this.dtpToDate.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.dtpToDate.Value = new System.DateTime(2004, 5, 25, 0, 0, 0, 0);
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
            this.dtpFromDate.AutoSize = false;
            this.dtpFromDate.ButtonStyle = Infragistics.Win.UIElementButtonStyle.PopupSoft;
            this.dtpFromDate.DateButtons.Add(dateButton2);
            this.dtpFromDate.Location = new System.Drawing.Point(135, 24);
            this.dtpFromDate.Name = "dtpFromDate";
            this.dtpFromDate.NonAutoSizeHeight = 10;
            this.dtpFromDate.Size = new System.Drawing.Size(120, 20);
            this.dtpFromDate.TabIndex = 0;
            this.dtpFromDate.Tag = "From Date";
            this.dtpFromDate.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.dtpFromDate.Value = new System.DateTime(2004, 5, 25, 0, 0, 0, 0);
            // 
            // ultraLabel13
            // 
            this.ultraLabel13.Location = new System.Drawing.Point(74, 66);
            this.ultraLabel13.Name = "ultraLabel13";
            this.ultraLabel13.Size = new System.Drawing.Size(59, 15);
            this.ultraLabel13.TabIndex = 2;
            this.ultraLabel13.Text = "To Date";
            // 
            // ultraLabel14
            // 
            this.ultraLabel14.Location = new System.Drawing.Point(61, 27);
            this.ultraLabel14.Name = "ultraLabel14";
            this.ultraLabel14.Size = new System.Drawing.Size(72, 15);
            this.ultraLabel14.TabIndex = 1;
            this.ultraLabel14.Text = "From Date";
            // 
            // optionSetForItems
            // 
            this.optionSetForItems.BorderStyle = Infragistics.Win.UIElementBorderStyle.None;
            valueListItem1.DataValue = "0";
            valueListItem1.DisplayText = "All Items";
            valueListItem2.DataValue = "1";
            valueListItem2.DisplayText = "Only Rx Items";
            valueListItem3.DataValue = "2";
            valueListItem3.DisplayText = "Exclude Rx Items";
            this.optionSetForItems.Items.AddRange(new Infragistics.Win.ValueListItem[] {
            valueListItem1,
            valueListItem2,
            valueListItem3});
            this.optionSetForItems.Location = new System.Drawing.Point(560, 102);
            this.optionSetForItems.Name = "optionSetForItems";
            this.optionSetForItems.Size = new System.Drawing.Size(321, 20);
            this.optionSetForItems.TabIndex = 9;
            // 
            // txtTaxCode
            // 
            this.txtTaxCode.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance13.BackColor = System.Drawing.Color.White;
            appearance13.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(231)))), ((int)(((byte)(230)))));
            appearance13.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance13.BorderAlpha = Infragistics.Win.Alpha.Transparent;
            appearance13.ImageAlpha = Infragistics.Win.Alpha.Opaque;
            appearance13.ImageBackgroundAlpha = Infragistics.Win.Alpha.Opaque;
            appearance13.ImageBackgroundStyle = Infragistics.Win.ImageBackgroundStyle.Centered;
            editorButton5.Appearance = appearance13;
            editorButton5.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            editorButton5.Text = "";
            editorButton5.Width = 20;
            this.txtTaxCode.ButtonsRight.Add(editorButton5);
            this.txtTaxCode.Location = new System.Drawing.Point(722, 141);
            this.txtTaxCode.Name = "txtTaxCode";
            this.txtTaxCode.ReadOnly = true;
            this.txtTaxCode.Size = new System.Drawing.Size(120, 20);
            this.txtTaxCode.TabIndex = 10;
            this.txtTaxCode.EditorButtonClick += new Infragistics.Win.UltraWinEditors.EditorButtonEventHandler(this.txtTaxCode_EditorButtonClick);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnReset);
            this.groupBox2.Controls.Add(this.btnPrint);
            this.groupBox2.Controls.Add(this.btnClose);
            this.groupBox2.Controls.Add(this.btnView);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.groupBox2.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.groupBox2.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.Location = new System.Drawing.Point(0, 211);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(892, 57);
            this.groupBox2.TabIndex = 30;
            this.groupBox2.TabStop = false;
            // 
            // btnReset
            // 
            appearance14.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(62)))), ((int)(((byte)(176)))));
            appearance14.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance14.FontData.BoldAsString = "True";
            appearance14.ForeColor = System.Drawing.Color.White;
            this.btnReset.Appearance = appearance14;
            this.btnReset.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            this.btnReset.Location = new System.Drawing.Point(518, 20);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(85, 26);
            this.btnReset.TabIndex = 3;
            this.btnReset.Text = "&Reset";
            this.btnReset.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
            // 
            // btnPrint
            // 
            appearance15.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(62)))), ((int)(((byte)(176)))));
            appearance15.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance15.FontData.BoldAsString = "True";
            appearance15.ForeColor = System.Drawing.Color.White;
            appearance15.Image = ((object)(resources.GetObject("appearance15.Image")));
            this.btnPrint.Appearance = appearance15;
            this.btnPrint.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            this.btnPrint.Location = new System.Drawing.Point(610, 20);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(85, 26);
            this.btnPrint.TabIndex = 2;
            this.btnPrint.Text = "&Print";
            this.btnPrint.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
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
            this.btnClose.Location = new System.Drawing.Point(794, 20);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(85, 26);
            this.btnClose.TabIndex = 1;
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
            this.btnView.Location = new System.Drawing.Point(702, 20);
            this.btnView.Name = "btnView";
            this.btnView.Size = new System.Drawing.Size(85, 26);
            this.btnView.TabIndex = 0;
            this.btnView.Text = "&View";
            this.btnView.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnView.Click += new System.EventHandler(this.btnView_Click);
            // 
            // lblTransactionType
            // 
            appearance18.ForeColor = System.Drawing.Color.White;
            appearance18.ForeColorDisabled = System.Drawing.Color.White;
            appearance18.TextHAlignAsString = "Center";
            this.lblTransactionType.Appearance = appearance18;
            this.lblTransactionType.BackColorInternal = System.Drawing.Color.Transparent;
            this.lblTransactionType.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblTransactionType.Font = new System.Drawing.Font("Arial", 18F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTransactionType.Location = new System.Drawing.Point(0, 0);
            this.lblTransactionType.Name = "lblTransactionType";
            this.lblTransactionType.Size = new System.Drawing.Size(892, 30);
            this.lblTransactionType.TabIndex = 31;
            this.lblTransactionType.Text = "Sales Tax Summary ";
            // 
            // pnlMain
            // 
            // 
            // pnlMain.ClientArea
            // 
            this.pnlMain.ClientArea.Controls.Add(this.gbInventoryReceived);
            this.pnlMain.ClientArea.Controls.Add(this.lblTransactionType);
            this.pnlMain.ClientArea.Controls.Add(this.groupBox2);
            this.pnlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlMain.Location = new System.Drawing.Point(0, 0);
            this.pnlMain.Name = "pnlMain";
            this.pnlMain.Size = new System.Drawing.Size(892, 268);
            this.pnlMain.TabIndex = 32;
            // 
            // frmRptSalesTax
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(892, 268);
            this.Controls.Add(this.pnlMain);
            this.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmRptSalesTax";
            this.Text = "Sales Tax Summary";
            this.Load += new System.EventHandler(this.frmRptSalesTax_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmRptSalesTax_KeyDown);
            this.gbInventoryReceived.ResumeLayout(false);
            this.gbInventoryReceived.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtStationID)).EndInit();
            this.grpPayTypeList.ResumeLayout(false);
            this.grpPayTypeList.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSubDepartment)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDepartment)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPaymentType)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtUserID)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtEODId)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCloseStationId)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtpToDate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtpFromDate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.optionSetForItems)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTaxCode)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.pnlMain.ClientArea.ResumeLayout(false);
            this.pnlMain.ResumeLayout(false);
            this.ResumeLayout(false);

		}
		#endregion

		private void frmRptSalesTax_Load(object sender, System.EventArgs e)
		{
			clsUIHelper.setColorSchecme(this);            

            #region PRIMEPOS-2436 18-Aug-2020 JY Added
            this.dtpFromDate.Enter += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
            this.dtpFromDate.Leave += new System.EventHandler(clsUIHelper.AfterExitEditMode);
            this.dtpToDate.Enter += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
            this.dtpToDate.Leave += new System.EventHandler(clsUIHelper.AfterExitEditMode);
            this.txtCloseStationId.AfterEnterEditMode += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
            this.txtCloseStationId.AfterExitEditMode += new System.EventHandler(clsUIHelper.AfterExitEditMode);
            this.txtEODId.AfterEnterEditMode += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
            this.txtEODId.AfterExitEditMode += new System.EventHandler(clsUIHelper.AfterExitEditMode);

            this.txtDepartment.AfterEnterEditMode += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
            this.txtDepartment.AfterExitEditMode += new System.EventHandler(clsUIHelper.AfterExitEditMode);
            this.txtSubDepartment.AfterEnterEditMode += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
            this.txtSubDepartment.AfterExitEditMode += new System.EventHandler(clsUIHelper.AfterExitEditMode);
            this.txtUserID.AfterEnterEditMode += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
            this.txtUserID.AfterExitEditMode += new System.EventHandler(clsUIHelper.AfterExitEditMode);
            this.txtStationID.AfterEnterEditMode += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
            this.txtStationID.AfterExitEditMode += new System.EventHandler(clsUIHelper.AfterExitEditMode);

            this.txtTaxCode.AfterEnterEditMode += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
            this.txtTaxCode.AfterExitEditMode += new System.EventHandler(clsUIHelper.AfterExitEditMode);
            txtDepartment.Tag = "";

            FillPayType();
            this.optionSetForItems.CheckedIndex = 0;
            #endregion

            dtpFromDate.Value = DateTime.Now;
			dtpToDate.Value = DateTime.Now;
		}

		private void Preview(bool PrintId, bool bCalledFromScheduler = false)   //PRIMEPOS-2485 22-Mar-2021 JY Added bCalledFromScheduler
        {
			try
			{
                //PRIMEPOS-2768 07-Jan-2020 JY Added InvoiceDiscount lineitemwise, changed queries accordingly
                rptSalesTaxSummary oRpt = new rptSalesTaxSummary();
//				rptTopSellingProductsPrice oRpt2= new rptTopSellingProductsPrice();

				string sSQL = "";
                string strFilterClause = FilterClause();    //PRIMEPOS-2436 18-Aug-2020 JY Added
                clsReports.setCRTextObjectText("txtFromDate",dtpFromDate.Text,oRpt);
                clsReports.setCRTextObjectText("txtToDate", dtpToDate.Text, oRpt);
                if (this.txtTaxCode.Text.Trim() != string.Empty)
                {
                    //clsReports.setCRTextObjectText("txtTaxCode", this.txtTaxCode.Text + "  (" + oTaxCodesRow.Description + ")", oRpt);
                    clsReports.setCRTextObjectText("txtTaxCode", this.txtTaxCode.Text, oRpt);
                }
                else
                    clsReports.setCRTextObjectText("txtTaxCode", "ALL", oRpt);
                //Edited By Amit Date 2 June 2011
                //Original
                //sSQL = " select 'Non Taxable' as Type,sum(PTD.ExtendedPrice) as Sale,0  AS Tax " +
                //        "	from POSTransactionDetail PTD, POSTransaction PT where PT.TransID=PTD.TransID and isnull(PTD.taxid,0)=0 " + buildCriteria() +
                //        " UNION " +
                //        " select 'Taxable' as Type,sum(PTD.ExtendedPrice) as Sale,sum(PTD.TaxAmount)  as Tax " +
                //        " from POSTransactionDetail PTD, POSTransaction PT where PT.TransID=PTD.TransID and isnull(PTD.taxid,0)<>0 " + buildCriteria();

                #region Sprint-24 - PRIMEPOS-2364 13-Jan-2017 JY Added
                string[] arrTaxCodes = txtTaxCode.Text.Split(',');
                if (string.IsNullOrEmpty(this.txtTaxCode.Text.Trim()) || (this.txtTaxCode.Text.Contains(clsPOSDBConstants.TaxCodes_NoTaxCode) && arrTaxCodes.GetLength(0) > 1))  
                {
                    sSQL = NonTaxQuery() + " UNION " + TaxableQuery();
                }
                else
                {
                    if (this.txtTaxCode.Text.Contains(clsPOSDBConstants.TaxCodes_NoTaxCode))    //Contains non taxable
                    {
                        sSQL = NonTaxQuery();
                    }
                    else  //Only taxable
                    {
                        sSQL = TaxableQuery();
                    }
                }
                #endregion

                #region Comment out by Manoj 8/6/2014 Fix report bug of not showing the correct Taxes
                //                sSQL = @"select Count(*) as Count, 'NO TAX' as TaxCode,'Non Taxable' as Type,
                //                         sum(PTD.ExtendedPrice) as [Gross Sale],sum(PTD.Discount ) as Discount ,
                //                         0 AS Tax 	from POSTransactionDetail PTD, POSTransaction PT
                //                         where PT.TransID=PTD.TransID AND
                //                         isnull(PTD.taxid,0)=0  " + buildDate() +
                //                        " UNION " +
                //                        @" select Count(*) as Count, tc.taxcode, tc.Description as Type,sum(PTD.ExtendedPrice) 
                //                         as [Gross Sale],sum(PTD.Discount ) as Discount,sum(PTDT.TaxAmount)  as Tax  
                //                         from POSTransactionDetail PTD, POSTransaction PT,taxcodes tc,POSTransactionDetailTax PTDT
                //                         where PT.TransID=PTD.TransID and PTD.TransID=PTDT.TransID AND
                //                         PTD.TransDetailID=PTDT.TransDetailID AND TC.TaxID = PTDT.Taxid
                //                         AND isnull(PTDT.taxid,0)<>0   " + buildCriteria() + 
                //                        " group by tc.TaxCode, tc.Description";
                //End
                #endregion

                string sSQLSubReport = string.Empty;
                if (string.IsNullOrEmpty(this.txtTaxCode.Text.Trim()) || (this.txtTaxCode.Text.Contains(clsPOSDBConstants.TaxCodes_NoTaxCode) && arrTaxCodes.GetLength(0) > 1))
                {
                    sSQLSubReport = @"SELECT 'Taxable' as Type, PTD.TransDetailID, PTD.TransID, PTD.TaxID, PTD.ItemID,
                                    PTD.ItemDescription, PTD.Qty, PTD.Price, PTD.Discount, PTDT.TaxAmount, PTD.ExtendedPrice, tc.taxcode,PT.TRANSDATE, PTD.InvoiceDiscount
                                    FROM POSTransactionDetail PTD
                                    INNER JOIN POSTransaction PT ON PT.TransID = PTD.TransID 
                                    INNER JOIN POSTransactionDetailTax PTDT ON PT.TransID = PTDT.TransID AND PTD.TransDetailID = PTDT.TransDetailID 
                                    INNER JOIN taxcodes tc ON tc.taxid = PTDT.taxid
                                        INNER JOIN Item I on I.ItemID=PTD.ItemID
                                        LEFT JOIN Department Dept on Dept.DeptID=I.DepartmentID
                                        LEFT JOIN subDepartment SD on SD.DepartmentID = Dept.DeptID AND SD.SubDepartmentID = I.SUBDEPARTMENTID
                                    WHERE ISNULL(PTDT.taxid,0) <> 0 " + TaxFilter() + strFilterClause +  //PRIMEPOS-2436 18-Aug-2020 JY replaced buildDate() with strFilterClause
                                    " UNION " +
                                    " SELECT 'Non Taxable' as Type, PTD.TransDetailID, PTD.TransID, PTD.TaxID, PTD.ItemID, " +
                                    " PTD.ItemDescription, PTD.Qty, PTD.Price, PTD.Discount, ISNULL(PTDT.TaxAmount,0.00) AS TaxAmount, PTD.ExtendedPrice, 'NO TAX' as TaxCode, PT.TRANSDATE, " +
                                    " PTD.InvoiceDiscount " +
                                    " FROM POSTransaction PT " +
                                    " INNER JOIN POSTransactionDetail PTD ON PT.TransID = PTD.TransID " +
                                    " LEFT JOIN POSTransactionDetailTax PTDT ON PTDT.TransDetailID = PTD.TransDetailID AND PTDT.TransID = PT.TransID " +
                                        " INNER JOIN Item I on I.ItemID = PTD.ItemID " +
                                        " LEFT JOIN Department Dept on Dept.DeptID = I.DepartmentID " +
                                        " LEFT JOIN subDepartment SD on SD.DepartmentID = Dept.DeptID AND SD.SubDepartmentID = I.SUBDEPARTMENTID " +
                                    " WHERE PTDT.TransDetailID IS NULL " + strFilterClause + //PRIMEPOS-2436 18-Aug-2020 JY replaced buildDate() with strFilterClause
                                    " order by taxcode";
                }
                #region Sprint-24 - PRIMEPOS-2364 13-Jan-2017 JY Added
                else
                {
                    if (this.txtTaxCode.Text.Contains(clsPOSDBConstants.TaxCodes_NoTaxCode))    //Contains non taxable
                    {
                        sSQLSubReport = @"SELECT 'Non Taxable' as Type, PTD.TransDetailID, PTD.TransID, PTD.TaxID, PTD.ItemID, " +
                                    " PTD.ItemDescription, PTD.Qty, PTD.Price, PTD.Discount, ISNULL(PTDT.TaxAmount,0.00) AS TaxAmount, PTD.ExtendedPrice, 'NO TAX' as TaxCode, PT.TRANSDATE, " +
                                    " PTD.InvoiceDiscount " +
                                    " FROM POSTransaction PT " +
                                    " INNER JOIN POSTransactionDetail PTD ON PT.TransID = PTD.TransID " +
                                    " LEFT JOIN POSTransactionDetailTax PTDT ON PTDT.TransDetailID = PTD.TransDetailID AND PTDT.TransID = PT.TransID " +
                                        " INNER JOIN Item I on I.ItemID = PTD.ItemID " +
                                        " LEFT JOIN Department Dept on Dept.DeptID = I.DepartmentID " +
                                        " LEFT JOIN subDepartment SD on SD.DepartmentID = Dept.DeptID AND SD.SubDepartmentID = I.SUBDEPARTMENTID " +
                                    " WHERE PTDT.TransDetailID IS NULL " + strFilterClause + //PRIMEPOS-2436 18-Aug-2020 JY replaced buildDate() with strFilterClause
                                    " order by taxcode";
                    }
                    else  //Only taxable
                    {
                        sSQLSubReport = @"SELECT 'Taxable' AS Type, PTD.TransDetailID, PTD.TransID, PTD.TaxID, PTD.ItemID,
                                    PTD.ItemDescription, PTD.Qty, PTD.Price, PTD.Discount, PTDT.TaxAmount, PTD.ExtendedPrice, tc.taxcode,PT.TRANSDATE, PTD.InvoiceDiscount
                                    FROM POSTransaction PT 
                                    INNER JOIN POSTransactionDetail PTD ON PT.TransID = PTD.TransID 
                                    INNER JOIN POSTransactionDetailTax PTDT ON PT.TransID = PTDT.TransID AND PTD.TransDetailID = PTDT.TransDetailID 
                                    INNER JOIN taxcodes tc ON tc.taxid = PTDT.taxid
                                        INNER JOIN Item I on I.ItemID=PTD.ItemID
                                        LEFT JOIN Department Dept on Dept.DeptID=I.DepartmentID
                                        LEFT JOIN subDepartment SD on SD.DepartmentID = Dept.DeptID AND SD.SubDepartmentID = I.SUBDEPARTMENTID
                                    WHERE ISNULL(PTDT.taxid,0) <> 0 " + TaxFilter() + strFilterClause +  //PRIMEPOS-2436 18-Aug-2020 JY replaced buildDate() with strFilterClause
                                    " ORDER BY taxcode";
                    }
                }
                #endregion
				
				System.Data.DataSet ds = clsReports.GetReportSource(sSQLSubReport);
				oRpt.OpenSubreport("rptTaxDetail").Database.Tables[0].SetDataSource(ds.Tables[0]);

                string sSQLSubReport1 = string.Empty;

                if (string.IsNullOrEmpty(this.txtTaxCode.Text.Trim()))
                {
                    sSQLSubReport1 = " SELECT SUM(PTD.Qty) AS TotalQty, SUM(PTD.ExtendedPrice) AS GrossSale, SUM(PTD.Discount) AS Discount, SUM(PTD.InvoiceDiscount) AS TotInvDisc, x.TransFee FROM POSTransaction PT " +
                                    " INNER JOIN POSTransactionDetail PTD ON PT.TransID=PTD.TransID " +
                                        " INNER JOIN Item I on I.ItemID = PTD.ItemID " +
                                        " LEFT JOIN Department Dept on Dept.DeptID = I.DepartmentID " +
                                        " LEFT JOIN subDepartment SD on SD.DepartmentID = Dept.DeptID AND SD.SubDepartmentID = I.SUBDEPARTMENTID " +
                                        " INNER JOIN (SELECT SUM(ISNULL(PT.TotalTransFeeAmt,0)) AS TransFee FROM POSTransaction PT INNER JOIN POSTransactionDetail PTD ON PT.TransID = PTD.TransID INNER JOIN Item I on I.ItemID = PTD.ItemID LEFT JOIN Department Dept on Dept.DeptID = I.DepartmentID LEFT JOIN subDepartment SD on SD.DepartmentID = Dept.DeptID AND SD.SubDepartmentID = I.SUBDEPARTMENTID WHERE 1=1 " + strFilterClause + ") x ON 1=1" +  //PRIMEPOS-3119 11-Aug-2022 JY Added TotalTransFeeAmt
                                    " WHERE 1 = 1 " + strFilterClause + //Sprint-24 - PRIMEPOS-2364 21-Dec-2016 JY Added   //PRIMEPOS-2436 18-Aug-2020 JY replaced buildDate() with strFilterClause
                                    " GROUP BY x.TransFee";
                    //" where PTD.TransDetailID IN (select PTDT.TransDetailID FROM POSTransactionDetailTax PTDT) " + buildDate();   //Sprint-24 - PRIMEPOS-2364 21-Dec-2016 JY Commented
                }
                else
                {
                    if (this.txtTaxCode.Text.Contains(clsPOSDBConstants.TaxCodes_NoTaxCode))    //Contains non taxable
                    {
                        if (arrTaxCodes.GetLength(0) > 1)
                        {
                            sSQLSubReport1 = " SELECT SUM(TotalQty) AS TotalQty, SUM(GrossSale) AS GrossSale, SUM(Discount) AS Discount, SUM(TotInvDisc1) AS TotInvDisc, SUM(TransFee) AS TransFee FROM " +
                                                    " (SELECT SUM(PTD.Qty) AS TotalQty, SUM(PTD.ExtendedPrice) AS GrossSale, SUM(PTD.Discount) AS Discount, SUM(PTD.InvoiceDiscount) AS TotInvDisc1, x.TransFee FROM POSTransaction PT " +
                                                    " INNER JOIN POSTransactionDetail PTD ON PT.TransID = PTD.TransID " +
                                                        " INNER JOIN Item I on I.ItemID = PTD.ItemID " +
                                                        " LEFT JOIN Department Dept on Dept.DeptID = I.DepartmentID " +
                                                        " LEFT JOIN subDepartment SD on SD.DepartmentID = Dept.DeptID AND SD.SubDepartmentID = I.SUBDEPARTMENTID " +
                                                        " INNER JOIN (SELECT SUM(ISNULL(PT.TotalTransFeeAmt,0)) AS TransFee FROM POSTransaction PT INNER JOIN POSTransactionDetail PTD ON PT.TransID = PTD.TransID INNER JOIN Item I on I.ItemID = PTD.ItemID LEFT JOIN Department Dept on Dept.DeptID = I.DepartmentID LEFT JOIN subDepartment SD on SD.DepartmentID = Dept.DeptID AND SD.SubDepartmentID = I.SUBDEPARTMENTID WHERE PTD.TransDetailID IN (SELECT TransDetailID FROM POSTransactionDetailTax PTDT INNER JOIN taxcodes tc ON TC.TaxID = PTDT.Taxid WHERE isnull(PTDT.taxid,0) <> 0 " + TaxFilter() + ")" + strFilterClause + ") x ON 1=1" +  //PRIMEPOS-3119 11-Aug-2022 JY Added TotalTransFeeAmt
                                                    " WHERE PTD.TransDetailID IN (SELECT TransDetailID FROM POSTransactionDetailTax PTDT INNER JOIN taxcodes tc ON TC.TaxID = PTDT.Taxid WHERE isnull(PTDT.taxid,0)<>0 " + TaxFilter() + ")" 
                                                    + strFilterClause +  //PRIMEPOS-2436 18-Aug-2020 JY replaced buildDate() with strFilterClause
                                                    " GROUP BY x.TransFee" +
                                                    " UNION " +
                                                    " SELECT SUM(PTD.Qty) AS TotalQty, SUM(PTD.ExtendedPrice) AS GrossSale, SUM(PTD.Discount) AS Discount, SUM(PTD.InvoiceDiscount) AS TotInvDisc1, x.TransFee FROM POSTransaction PT " +
                                                    " INNER JOIN POSTransactionDetail PTD ON PT.TransID = PTD.TransID " +
                                                    " LEFT JOIN POSTransactionDetailTax PTDT ON PTDT.TransDetailID = PTD.TransDetailID AND PTDT.TransID = PT.TransID " +
                                                        " INNER JOIN Item I on I.ItemID = PTD.ItemID " +
                                                        " LEFT JOIN Department Dept on Dept.DeptID = I.DepartmentID " +
                                                        " LEFT JOIN subDepartment SD on SD.DepartmentID = Dept.DeptID AND SD.SubDepartmentID = I.SUBDEPARTMENTID " +
                                                        " INNER JOIN (SELECT SUM(ISNULL(PT.TotalTransFeeAmt,0)) AS TransFee FROM POSTransaction PT  INNER JOIN POSTransactionDetail PTD ON PT.TransID = PTD.TransID  LEFT JOIN POSTransactionDetailTax PTDT ON PTDT.TransDetailID = PTD.TransDetailID AND PTDT.TransID = PT.TransID  INNER JOIN Item I on I.ItemID = PTD.ItemID  LEFT JOIN Department Dept on Dept.DeptID = I.DepartmentID LEFT JOIN subDepartment SD on SD.DepartmentID = Dept.DeptID AND SD.SubDepartmentID = I.SUBDEPARTMENTID   WHERE PTDT.TransDetailID IS NULL " + strFilterClause + ") x ON 1=1" +  //PRIMEPOS-3119 11-Aug-2022 JY Added TotalTransFeeAmt
                                                    " WHERE PTDT.TransDetailID IS NULL " + strFilterClause + "  GROUP BY x.TransFee) a";  //PRIMEPOS-2436 18-Aug-2020 JY replaced buildDate() with strFilterClause
                        }
                        else
                        {
                            sSQLSubReport1 = " SELECT SUM(PTD.Qty) AS TotalQty, SUM(PTD.ExtendedPrice) AS GrossSale, SUM(PTD.Discount) AS Discount, SUM(PTD.InvoiceDiscount) AS TotInvDisc, x.TransFee FROM POSTransaction PT " +
                                    " INNER JOIN POSTransactionDetail PTD ON PT.TransID = PTD.TransID " +
                                    " LEFT JOIN POSTransactionDetailTax PTDT ON PTDT.TransDetailID = PTD.TransDetailID AND PTDT.TransID = PT.TransID " +
                                        " INNER JOIN Item I on I.ItemID = PTD.ItemID " +
                                        " LEFT JOIN Department Dept on Dept.DeptID = I.DepartmentID " +
                                        " LEFT JOIN subDepartment SD on SD.DepartmentID = Dept.DeptID AND SD.SubDepartmentID = I.SUBDEPARTMENTID " +
                                        " INNER JOIN (SELECT SUM(ISNULL(PT.TotalTransFeeAmt,0)) AS TransFee FROM POSTransaction PT INNER JOIN POSTransactionDetail PTD ON PT.TransID = PTD.TransID LEFT JOIN POSTransactionDetailTax PTDT ON PTDT.TransDetailID = PTD.TransDetailID AND PTDT.TransID = PT.TransID INNER JOIN Item I on I.ItemID = PTD.ItemID LEFT JOIN Department Dept on Dept.DeptID = I.DepartmentID LEFT JOIN subDepartment SD on SD.DepartmentID = Dept.DeptID AND SD.SubDepartmentID = I.SUBDEPARTMENTID WHERE PTDT.TransDetailID IS NULL " + strFilterClause + ") x ON 1=1" +  //PRIMEPOS-3119 11-Aug-2022 JY Added TotalTransFeeAmt
                                    " WHERE PTDT.TransDetailID IS NULL " + strFilterClause +    //PRIMEPOS-2436 18-Aug-2020 JY replaced buildDate() with strFilterClause
                                    " GROUP BY x.TransFee";
                        }
                    }
                    else  //Only taxable
                    {
                        sSQLSubReport1 = " SELECT SUM(PTD.Qty) AS TotalQty, SUM(PTD.ExtendedPrice) AS GrossSale, SUM(PTD.Discount) AS Discount, SUM(PTD.InvoiceDiscount) AS TotInvDisc, x.TransFee FROM POSTransaction PT " +
                                        " INNER JOIN POSTransactionDetail PTD ON PT.TransID = PTD.TransID " +
                                            " INNER JOIN Item I on I.ItemID = PTD.ItemID " +
                                            " LEFT JOIN Department Dept on Dept.DeptID = I.DepartmentID " +
                                            " LEFT JOIN subDepartment SD on SD.DepartmentID = Dept.DeptID AND SD.SubDepartmentID = I.SUBDEPARTMENTID " +
                                            " INNER JOIN (SELECT SUM(ISNULL(PT.TotalTransFeeAmt,0)) AS TransFee FROM POSTransaction PT INNER JOIN POSTransactionDetail PTD ON PT.TransID = PTD.TransID INNER JOIN Item I on I.ItemID = PTD.ItemID LEFT JOIN Department Dept on Dept.DeptID = I.DepartmentID LEFT JOIN subDepartment SD on SD.DepartmentID = Dept.DeptID AND SD.SubDepartmentID = I.SUBDEPARTMENTID WHERE PTD.TransDetailID IN (SELECT TransDetailID FROM POSTransactionDetailTax PTDT INNER JOIN taxcodes tc ON TC.TaxID = PTDT.Taxid WHERE isnull(PTDT.taxid,0) <> 0 " + TaxFilter() + ")" + strFilterClause + ") x ON 1=1" +  //PRIMEPOS-3119 11-Aug-2022 JY Added TotalTransFeeAmt
                                        " WHERE PTD.TransDetailID IN (SELECT TransDetailID FROM POSTransactionDetailTax PTDT INNER JOIN taxcodes tc ON TC.TaxID = PTDT.Taxid WHERE isnull(PTDT.taxid,0)<>0 " + TaxFilter() + ")" +
                                        strFilterClause +   //PRIMEPOS-2436 18-Aug-2020 JY replaced buildDate() with strFilterClause
                                        "GROUP BY x.TransFee";
                    }
                }

                System.Data.DataSet ds1 = clsReports.GetReportSource(sSQLSubReport1);
                oRpt.OpenSubreport("rptTaxDetail").Database.Tables[1].SetDataSource(ds1.Tables[0]);
                //				if (optByName.CheckedIndex == 0)
                //clsReports.Preview(PrintId,sSQL,oRpt);
                string sumSQL = string.Empty;
                if (string.IsNullOrEmpty(this.txtTaxCode.Text.Trim()))
                {
                    sumSQL = "SELECT COUNT(PTD.TransDetailID) as CNT, SUM(PTD.ExtendedPrice) AS GrossSale, SUM(PTD.Discount) as Discount, SUM(PTD.InvoiceDiscount) AS TotInvDisc, x.TransFee FROM POSTransaction PT " +
                            " INNER JOIN POSTransactionDetail PTD ON PT.TransID=PTD.TransID " +
                            " INNER JOIN Item I on I.ItemID = PTD.ItemID " +
                            " LEFT JOIN Department Dept on Dept.DeptID = I.DepartmentID " +
                            " LEFT JOIN subDepartment SD on SD.DepartmentID = Dept.DeptID AND SD.SubDepartmentID = I.SUBDEPARTMENTID " +
                            " INNER JOIN (SELECT SUM(ISNULL(PT.TotalTransFeeAmt,0)) AS TransFee FROM POSTransaction PT INNER JOIN POSTransactionDetail PTD ON PT.TransID = PTD.TransID INNER JOIN Item I on I.ItemID = PTD.ItemID LEFT JOIN Department Dept on Dept.DeptID = I.DepartmentID LEFT JOIN subDepartment SD on SD.DepartmentID = Dept.DeptID AND SD.SubDepartmentID = I.SUBDEPARTMENTID WHERE 1=1 " + strFilterClause + ") x ON 1=1" +  //PRIMEPOS-3119 11-Aug-2022 JY Added TotalTransFeeAmt
                            " where 1=1 " + strFilterClause +   //PRIMEPOS-2436 18-Aug-2020 JY replaced buildDate() with strFilterClause
                            " GROUP BY x.TransFee";
                }
                else
                {
                    if (this.txtTaxCode.Text.Contains(clsPOSDBConstants.TaxCodes_NoTaxCode))    //Contains non taxable
                    {
                        if (arrTaxCodes.GetLength(0) > 1)
                        {
                            sumSQL = "SELECT SUM(CNT) AS CNT, SUM(GrossSale) AS GrossSale, SUM(Discount) AS Discount, SUM(TotInvDisc1) AS TotInvDisc, SUM(TransFee) AS TransFee FROM " +
                                " (SELECT COUNT(PTD.TransDetailID) as CNT, sum(PTD.ExtendedPrice) AS GrossSale, sum(PTD.Discount) as Discount, SUM(PTD.InvoiceDiscount) AS TotInvDisc1, x.TransFee FROM POSTransaction PT " +
                                " INNER JOIN POSTransactionDetail PTD ON PT.TransID=PTD.TransID " +
                                " INNER JOIN Item I on I.ItemID = PTD.ItemID " +
                                " LEFT JOIN Department Dept on Dept.DeptID = I.DepartmentID " +
                                " LEFT JOIN subDepartment SD on SD.DepartmentID = Dept.DeptID AND SD.SubDepartmentID = I.SUBDEPARTMENTID " +
                                " INNER JOIN (SELECT SUM(ISNULL(PT.TotalTransFeeAmt,0)) AS TransFee FROM POSTransaction PT INNER JOIN POSTransactionDetail PTD ON PT.TransID = PTD.TransID INNER JOIN Item I on I.ItemID = PTD.ItemID LEFT JOIN Department Dept on Dept.DeptID = I.DepartmentID LEFT JOIN subDepartment SD on SD.DepartmentID = Dept.DeptID AND SD.SubDepartmentID = I.SUBDEPARTMENTID WHERE PTD.TransDetailID IN (SELECT TransDetailID FROM POSTransactionDetailTax PTDT INNER JOIN taxcodes tc ON TC.TaxID = PTDT.Taxid WHERE isnull(PTDT.taxid,0) <> 0 " + TaxFilter() + ")" + strFilterClause + ") x ON 1=1" +  //PRIMEPOS-3119 11-Aug-2022 JY Added TotalTransFeeAmt
                                " WHERE PTD.TransDetailID IN (SELECT TransDetailID FROM POSTransactionDetailTax PTDT INNER JOIN taxcodes tc ON TC.TaxID = PTDT.Taxid WHERE isnull(PTDT.taxid,0)<>0 " + TaxFilter() + ")" +
                                strFilterClause +    //PRIMEPOS-2436 18-Aug-2020 JY replaced buildDate() with strFilterClause
                                " GROUP BY x.TransFee" +
                                " UNION " +
                                " SELECT COUNT(PTD.TransDetailID) as CNT, SUM(PTD.ExtendedPrice) AS GrossSale, SUM(PTD.Discount) AS Discount, SUM(PTD.InvoiceDiscount) AS TotInvDisc1, x.TransFee FROM POSTransaction PT " +
                                " INNER JOIN POSTransactionDetail PTD ON PT.TransID = PTD.TransID " +
                                " LEFT JOIN POSTransactionDetailTax PTDT ON PTDT.TransDetailID = PTD.TransDetailID AND PTDT.TransID = PT.TransID " +
                                " INNER JOIN Item I on I.ItemID = PTD.ItemID " +
                                " LEFT JOIN Department Dept on Dept.DeptID = I.DepartmentID " +
                                " LEFT JOIN subDepartment SD on SD.DepartmentID = Dept.DeptID AND SD.SubDepartmentID = I.SUBDEPARTMENTID " +
                                " INNER JOIN (SELECT SUM(ISNULL(PT.TotalTransFeeAmt,0)) AS TransFee FROM POSTransaction PT  INNER JOIN POSTransactionDetail PTD ON PT.TransID = PTD.TransID  LEFT JOIN POSTransactionDetailTax PTDT ON PTDT.TransDetailID = PTD.TransDetailID AND PTDT.TransID = PT.TransID  INNER JOIN Item I on I.ItemID = PTD.ItemID  LEFT JOIN Department Dept on Dept.DeptID = I.DepartmentID LEFT JOIN subDepartment SD on SD.DepartmentID = Dept.DeptID AND SD.SubDepartmentID = I.SUBDEPARTMENTID   WHERE PTDT.TransDetailID IS NULL " + strFilterClause + ") x ON 1=1" +  //PRIMEPOS-3119 11-Aug-2022 JY Added TotalTransFeeAmt
                                " WHERE PTDT.TransDetailID IS NULL " + strFilterClause + " GROUP BY x.TransFee) a"; //PRIMEPOS-2436 18-Aug-2020 JY replaced buildDate() with strFilterClause
                        }
                        else
                        {
                            sumSQL = " SELECT COUNT(PTD.TransDetailID) as CNT, SUM(PTD.ExtendedPrice) AS GrossSale, SUM(PTD.Discount) AS Discount, SUM(PTD.InvoiceDiscount) AS TotInvDisc, x.TransFee FROM POSTransaction PT " +
                                    " INNER JOIN POSTransactionDetail PTD ON PT.TransID = PTD.TransID " +
                                    " LEFT JOIN POSTransactionDetailTax PTDT ON PTDT.TransDetailID = PTD.TransDetailID AND PTDT.TransID = PT.TransID " +
                                    " INNER JOIN Item I on I.ItemID = PTD.ItemID " +
                                    " LEFT JOIN Department Dept on Dept.DeptID = I.DepartmentID " +
                                    " LEFT JOIN subDepartment SD on SD.DepartmentID = Dept.DeptID AND SD.SubDepartmentID = I.SUBDEPARTMENTID " +
                                    " INNER JOIN (SELECT SUM(ISNULL(PT.TotalTransFeeAmt,0)) AS TransFee FROM POSTransaction PT INNER JOIN POSTransactionDetail PTD ON PT.TransID = PTD.TransID LEFT JOIN POSTransactionDetailTax PTDT ON PTDT.TransDetailID = PTD.TransDetailID AND PTDT.TransID = PT.TransID INNER JOIN Item I on I.ItemID = PTD.ItemID LEFT JOIN Department Dept on Dept.DeptID = I.DepartmentID LEFT JOIN subDepartment SD on SD.DepartmentID = Dept.DeptID AND SD.SubDepartmentID = I.SUBDEPARTMENTID WHERE PTDT.TransDetailID IS NULL " + strFilterClause + ") x ON 1=1" +  //PRIMEPOS-3119 11-Aug-2022 JY Added TotalTransFeeAmt
                                    " WHERE PTDT.TransDetailID IS NULL " + strFilterClause +    //PRIMEPOS-2436 18-Aug-2020 JY replaced buildDate() with strFilterClause
                                    " GROUP BY x.TransFee";
                        }
                    }
                    else  //Only taxable
                    {
                        sumSQL = "SELECT COUNT(PTD.TransDetailID) AS CNT, SUM(PTD.ExtendedPrice) AS GrossSale, SUM(PTD.Discount) AS Discount, SUM(PTD.InvoiceDiscount) AS TotInvDisc, x.TransFee FROM POSTransaction PT " +
                            " INNER JOIN POSTransactionDetail PTD ON PT.TransID = PTD.TransID " +
                                " INNER JOIN Item I on I.ItemID = PTD.ItemID " +
                                " LEFT JOIN Department Dept on Dept.DeptID = I.DepartmentID " +
                                " LEFT JOIN subDepartment SD on SD.DepartmentID = Dept.DeptID AND SD.SubDepartmentID = I.SUBDEPARTMENTID " +
                                " INNER JOIN (SELECT SUM(ISNULL(PT.TotalTransFeeAmt,0)) AS TransFee FROM POSTransaction PT INNER JOIN POSTransactionDetail PTD ON PT.TransID = PTD.TransID INNER JOIN Item I on I.ItemID = PTD.ItemID LEFT JOIN Department Dept on Dept.DeptID = I.DepartmentID LEFT JOIN subDepartment SD on SD.DepartmentID = Dept.DeptID AND SD.SubDepartmentID = I.SUBDEPARTMENTID WHERE PTD.TransDetailID IN (SELECT TransDetailID FROM POSTransactionDetailTax PTDT INNER JOIN taxcodes tc ON TC.TaxID = PTDT.Taxid WHERE isnull(PTDT.taxid,0) <> 0 " + TaxFilter() + ")" + strFilterClause + ") x ON 1=1" +  //PRIMEPOS-3119 11-Aug-2022 JY Added TotalTransFeeAmt
                                " WHERE PTD.TransDetailID IN (SELECT TransDetailID FROM POSTransactionDetailTax PTDT INNER JOIN taxcodes tc ON TC.TaxID = PTDT.Taxid WHERE isnull(PTDT.taxid,0) <> 0 " + TaxFilter() + ")" +
                            strFilterClause +   //PRIMEPOS-2436 18-Aug-2020 JY replaced buildDate() with strFilterClause
                            "GROUP BY x.TransFee";
                    }
                }

                clsReports.Preview(PrintId, sSQL, sumSQL, oRpt, 1, bCalledFromScheduler);   //PRIMEPOS-2485 22-Mar-2021 JY Added bCalledFromScheduler
                oReport = oRpt; //PRIMEPOS-2485 22-Mar-2021 JY Added
            }
			catch(Exception exp)
			{
				clsUIHelper.ShowErrorMsg(exp.Message);
			}
		}

        ///<Author>Manoj 8/6/2014</Author>
        /// <summary>Build the query if a tax code is selected</summary>
        /// <returns>string</returns>
        private string TaxableQuery()
        {
            string sSQL = string.Empty;
            //return sSQL = @" select Count(*) as Count, tc.taxcode, tc.Description as Type,sum(PTD.ExtendedPrice) 
            //             as [Gross Sale],sum(PTD.Discount ) as Discount,sum(PTDT.TaxAmount)  as Tax  
            //             from POSTransactionDetail PTD, POSTransaction PT,taxcodes tc,POSTransactionDetailTax PTDT
            //             where PT.TransID=PTD.TransID and PTD.TransID=PTDT.TransID AND
            //             PTD.TransDetailID=PTDT.TransDetailID AND TC.TaxID = PTDT.Taxid
            //             AND isnull(PTDT.taxid,0)<>0   " + buildCriteria() +
            //           " group by tc.TaxCode, tc.Description";

            return sSQL = @"SELECT COUNT(*) AS Count, tc.taxcode, tc.Description as Type, SUM(PTD.ExtendedPrice) AS [Gross Sale], 
                        SUM(PTD.Discount) AS Discount, SUM(PTDT.TaxAmount) AS Tax, SUM(PTD.InvoiceDiscount) AS TotInvDisc FROM POSTransactionDetail PTD
                        INNER JOIN POSTransaction PT ON PT.TransID = PTD.TransID 
                        INNER JOIN POSTransactionDetailTax PTDT ON PTD.TransID = PTDT.TransID AND PTD.TransDetailID = PTDT.TransDetailID
                        INNER JOIN taxcodes tc ON TC.TaxID = PTDT.Taxid
                            INNER JOIN Item I on I.ItemID=PTD.ItemID
                            LEFT JOIN Department Dept on Dept.DeptID=I.DepartmentID
                            LEFT JOIN subDepartment SD on SD.DepartmentID = Dept.DeptID AND SD.SubDepartmentID = I.SUBDEPARTMENTID
                        WHERE ISNULL(PTDT.taxid,0) <> 0 " + TaxFilter() + FilterClause() +  //PRIMEPOS-2436 18-Aug-2020 JY replaced buildDate() with FilterClause()
                        " GROUP BY tc.TaxCode, tc.Description";
        }

        ///<Author>Manoj 8/6/2014</Author>
        /// <summary>Build the query is tax is N</summary>
        /// <returns>string</returns>
        private string NonTaxQuery()
        {
           string sSQL = string.Empty;

           /*return sSQL = @"select Count(*) as Count, 'NO TAX' as TaxCode,'Non Taxable' as Type,
                         sum(PTD.ExtendedPrice) as [Gross Sale],sum(PTD.Discount ) as Discount ,
                         0 AS Tax 	from POSTransactionDetail PTD, POSTransaction PT
                         where PT.TransID=PTD.TransID AND
                         isnull(PTD.taxid,0)=0  " + buildDate();*/
            //Sprint-18 17-Nov-2014 JY corrected the above sql
           return sSQL = @"SELECT Count(*) AS Count, 'NO TAX' AS TaxCode, 'Non Taxable' AS Type, SUM(PTD.ExtendedPrice) AS [Gross Sale], 
                        SUM(PTD.Discount) AS Discount, 0 AS Tax, SUM(PTD.InvoiceDiscount) AS TotInvDisc FROM POSTransaction PT
                        INNER JOIN POSTransactionDetail PTD ON PT.TransID = PTD.TransID 
                        LEFT JOIN POSTransactionDetailTax PTDT ON PTDT.TransDetailID = PTD.TransDetailID AND PTDT.TransID = PT.TransID
                            INNER JOIN Item I on I.ItemID=PTD.ItemID
                            LEFT JOIN Department Dept on Dept.DeptID=I.DepartmentID
                            LEFT JOIN subDepartment SD on SD.DepartmentID = Dept.DeptID AND SD.SubDepartmentID = I.SUBDEPARTMENTID
                        WHERE PTDT.TransDetailID IS NULL " + FilterClause();    //PRIMEPOS-2436 18-Aug-2020 JY replaced buildDate() with FilterClause()
        }

        //Sprint-23 - PRIMEPOS-N TaxType 28-Jun-2016 JY Added 
        //private string NTaxQuery()
        //{
        //    string sSQL = string.Empty;

        //    return sSQL = @"select Count(*) as Count, tc.taxcode, tc.Description as Type, sum(PTD.ExtendedPrice) 
        //                as [Gross Sale], sum(PTD.Discount) as Discount, sum(PTDT.TaxAmount) as Tax  
        //                from POSTransactionDetail PTD
        //                INNER JOIN POSTransaction PT ON PT.TransID = PTD.TransID 
        //                INNER JOIN POSTransactionDetailTax PTDT ON PTD.TransID = PTDT.TransID AND PTD.TransDetailID = PTDT.TransDetailID
        //                INNER JOIN taxcodes tc ON TC.TaxID = PTDT.Taxid
        //                where tc.taxcode = 'N' AND isnull(PTDT.taxid,0)<>0 " + buildCriteria() +
        //                " group by tc.TaxCode, tc.Description";
        //}

        ///<Author>Manoj 8/6/2014</Author>
        /// <summary>Add the date rage the user selected</summary>
        /// <returns>string</returns>
        private string buildDate()
        {
            string cDate = "";
            if(dtpFromDate.Value.ToString() != "")
            {
                cDate = cDate + " AND Convert(smalldatetime,Convert(Varchar,PT.TransDate,107)) >= '" + dtpFromDate.Text + "'";
            }
            if(dtpToDate.Value.ToString() != "")
            {
                cDate = cDate + " AND Convert(smalldatetime,Convert(Varchar,PT.TransDate,107)) <= '" + dtpToDate.Text + "'";
            }

            return cDate;
        }

        private string TaxFilter()
        {
            string sReturn = string.Empty;
            if (!string.IsNullOrEmpty(this.txtTaxCode.Text.Trim()))
            {
                sReturn = " AND PTDT.TaxID IN (" + this.txtTaxCode.Tag.ToString() + ")";
            }
            return sReturn;
        }

        ///<Author>Manoj 8/6/2014</Author>
        /// <summary>Get the selected Tax code and add it to the query</summary>
        /// <returns>string</returns>
        //private string buildCriteria()
        //{
        //	string sCriteria = string.Empty;
        //          if(!string.IsNullOrEmpty(this.txtTaxCode.Text) && this.txtTaxCode.Text != "N")
        //          {
        //              sCriteria = buildDate() + " AND PTDT.TaxID IN (" + this.txtTaxCode.Tag.ToString() + ")"; //Change by Manoj from PTDT.TaxID to PTD.TaxID. Fix error 
        //          }
        //          else
        //          {
        //              sCriteria = buildDate(); //if the tax code is not selected then just add the date range
        //          }
        //          return sCriteria;
        //}

        private void btnClose_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}

		private void frmRptSalesTax_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			try
			{
				if(e.KeyData==System.Windows.Forms.Keys.Enter)
				{
					this.SelectNextControl(this.ActiveControl,true,true,true,true);
				}
                #region PRIMEPOS-2436 18-Aug-2020 JY Added
                else if (e.KeyData == Keys.Escape)
                    this.Close();
                else if (e.KeyData == System.Windows.Forms.Keys.F4)
                {
                    if (this.txtCloseStationId.ContainsFocus == true)
                    {
                        SearchCloseStationId();
                    }
                    if (this.txtEODId.ContainsFocus == true)
                    {
                        SearchEODId();
                    }
                    if (this.txtDepartment.ContainsFocus == true)
                    {
                        this.SearchDept();
                    }
                    if (this.txtSubDepartment.ContainsFocus == true)
                    {
                        this.SearchSubDept();
                    }
                    if (this.txtTaxCode.ContainsFocus == true)
                    {
                        SearchTaxCode();
                    }
                }
                #endregion
            }
            catch (Exception exp)
			{
				clsUIHelper.ShowErrorMsg(exp.Message);
			}
		}

		private void btnView_Click(object sender, System.EventArgs e)
		{
			this.dtpFromDate.Focus();
			Preview(false);
		}

        private void btnPrint_Click(object sender, System.EventArgs e)
		{
			this.dtpFromDate.Focus();
			Preview(true);
		}

        //Added By Shitaljit 0n 17 Feb 2012
        //To ad Tax code filter in report.
        private void txtTaxCode_EditorButtonClick(object sender, Infragistics.Win.UltraWinEditors.EditorButtonEventArgs e)
        {
            SearchTaxCode();
        }
        private void SearchTaxCode()
        {
            try
            {
                //frmSearch oSearch = new frmSearch(clsPOSDBConstants.TaxCodes_With_NoTax);
                frmSearchMain oSearch = new frmSearchMain(true);    //20-Dec-2017 JY Added new reference
                oSearch.SearchTable = clsPOSDBConstants.TaxCodes_With_NoTax;    //20-Dec-2017 JY Added 
                oSearch.AllowMultiRowSelect = true;
                oSearch.SearchInConstructor = true;
                oSearch.ShowDialog(this);
                if (!oSearch.IsCanceled)
                {
                    string strTaxID = string.Empty;
                    string strTaxCode = string.Empty;
                    foreach (UltraGridRow oRow in oSearch.grdSearch.Rows)
                    {
                        if ((bool)oRow.Cells["check"].Value == true)
                        {
                            strTaxID += "," + oRow.Cells["TaxID"].Text;
                            strTaxCode += "," + oRow.Cells["Code"].Text.Trim();
                        }
                    }

                    if (strTaxID.StartsWith(","))
                    {
                        strTaxID = strTaxID.Substring(1);
                        strTaxCode = strTaxCode.Substring(1);
                    }
                    txtTaxCode.Text = strTaxCode;
                    txtTaxCode.Tag = strTaxID;
                }
                else
                {
                    txtTaxCode.Text = string.Empty;
                    txtTaxCode.Tag = string.Empty;
                }
            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }
        
        private void txtTaxCode_Validated(object sender, EventArgs e)
        {
            //if (this.txtTaxCode.Text != "")
            //{
            //    oTaxCodesData = oTaxCodes.Populate(this.txtTaxCode.Text.Trim());
            //    if (oTaxCodesData != null)
            //    {
            //        if (oTaxCodesData.Tables[0].Rows.Count > 0)
            //        {
            //            oTaxCodesRow = oTaxCodesData.TaxCodes[0];
            //            this.txtTaxCode.Text = oTaxCodesRow.TaxCode;
            //            this.txtTaxCode.Tag = oTaxCodesRow.TaxID;
            //        }
            //        else
            //        {
            //            clsUIHelper.ShowErrorMsg("Invalid Tax Code.");
            //            this.txtTaxCode.Focus();
            //        }
            //    }
            //    else
            //    {
            //        clsUIHelper.ShowErrorMsg("Invalid Tax Code.");
            //    }
            //}
        }

        #region PRIMEPOS-2436 18-Aug-2020 JY Added
        private void FillPayType()
        {
            SearchSvr SearchSvr = new POS_Core.DataAccess.SearchSvr();
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
        }

        private void txtDepartment_TextChanged(object sender, EventArgs e)
        {
            if (txtDepartment.Text != "")
            {
                txtSubDepartment.Text = "";
            }
        }

        private void txtDepartment_EditorButtonClick(object sender, Infragistics.Win.UltraWinEditors.EditorButtonEventArgs e)
        {
            SearchDept();
        }

        private void SearchDept()
        {
            try
            {
                frmSearchMain oSearch = new frmSearchMain(true);
                oSearch.SearchTable = clsPOSDBConstants.Department_tbl;
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

        private void txtSubDepartment_EditorButtonClick(object sender, Infragistics.Win.UltraWinEditors.EditorButtonEventArgs e)
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
                    InQuery = txtDepartment.Tag.ToString().Substring(1, txtDepartment.Tag.ToString().Length - 2);
                }
                SearchSvr.SubDeptIDFlag = true;

                if (InQuery != "")
                {
                    SearchSvr.SubDeptIDFlag = true;
                }
                frmSearchMain oSearch = new frmSearchMain(clsPOSDBConstants.SubDepartment_tbl, InQuery, "", true);
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
                    dataGridList.Height = 110;
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

        private void txtPaymentType_EditorButtonClick(object sender, Infragistics.Win.UltraWinEditors.EditorButtonEventArgs e)
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

                frmSearchMain oSearch = new frmSearchMain(true);
                oSearch.SearchTable = clsPOSDBConstants.PayType_tbl;
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

        private void dataGridList_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            for (int i = 0; i < dataGridList.Rows.Count; i++)
			{
                dataGridList.Rows[i].Cells[dataGridList.Columns["chkBox"].Index].Value = true;
			}
            chkSelectAll.Checked = true;
            GetSelectedPayTypes();
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

        private void txtCloseStationId_EditorButtonClick(object sender, Infragistics.Win.UltraWinEditors.EditorButtonEventArgs e)
        {
            try
            {
                SearchCloseStationId();
            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }

        private void txtCloseStationId_KeyPress(object sender, KeyPressEventArgs e)
        {
            char Delete = (char)8;
            char comma = (char)',';
            e.Handled = !Char.IsDigit(e.KeyChar) && e.KeyChar != Delete && e.KeyChar != comma;
        }

        private void SearchCloseStationId()
        {
            try
            {
                frmViewEODStation ofrmViewEODStation = new frmViewEODStation(clsPOSDBConstants.StationCloseHeader_tbl);
                ofrmViewEODStation.AllowMultiRowSelect = true;
                ofrmViewEODStation.DisplayRecordAtStartup = true;
                ofrmViewEODStation.ShowDialog(this);
                if (!ofrmViewEODStation.IsCanceled)
                {
                    string strCloseStationId = string.Empty;
                    foreach (UltraGridRow oRow in ofrmViewEODStation.grdSearch.Rows)
                    {
                        if ((bool)oRow.Cells["check"].Value == true)
                        {
                            if (strCloseStationId == string.Empty)
                                strCloseStationId = oRow.Cells[clsPOSDBConstants.StationCloseHeader_Fld_StationCloseID].Text;
                            else
                                strCloseStationId += "," + oRow.Cells[clsPOSDBConstants.StationCloseHeader_Fld_StationCloseID].Text;
                        }
                    }
                    txtCloseStationId.Text = strCloseStationId;
                }
                else
                {
                    txtCloseStationId.Text = string.Empty;
                }
            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }

        private void txtEODId_EditorButtonClick(object sender, Infragistics.Win.UltraWinEditors.EditorButtonEventArgs e)
        {
            try
            {
                SearchEODId();
            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }

        private void txtEODId_KeyPress(object sender, KeyPressEventArgs e)
        {
            char Delete = (char)8;
            char comma = (char)',';
            e.Handled = !Char.IsDigit(e.KeyChar) && e.KeyChar != Delete && e.KeyChar != comma;
        }

        private void SearchEODId()
        {
            try
            {
                frmViewEODStation ofrmViewEODStation = new frmViewEODStation(clsPOSDBConstants.EndOfDay_tbl);
                ofrmViewEODStation.AllowMultiRowSelect = true;
                ofrmViewEODStation.DisplayRecordAtStartup = true;
                ofrmViewEODStation.ShowDialog(this);
                if (!ofrmViewEODStation.IsCanceled)
                {
                    string strEODId = string.Empty;
                    foreach (UltraGridRow oRow in ofrmViewEODStation.grdSearch.Rows)
                    {
                        if ((bool)oRow.Cells["check"].Value == true)
                        {
                            if (strEODId == string.Empty)
                                strEODId = oRow.Cells[clsPOSDBConstants.StationCloseHeader_Fld_EODID].Text;
                            else
                                strEODId += "," + oRow.Cells[clsPOSDBConstants.StationCloseHeader_Fld_EODID].Text;
                        }
                    }
                    txtEODId.Text = strEODId;
                }
                else
                {
                    txtEODId.Text = string.Empty;
                }
            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }

        private string FilterClause()
        {
            string strFilter = string.Empty;

            if (string.IsNullOrWhiteSpace(this.txtCloseStationId.Text) && string.IsNullOrWhiteSpace(this.txtEODId.Text))
                strFilter += " AND convert(datetime,PT.TransDate,113) between convert(datetime, cast('" + dtpFromDate.Text + " 00:00:00 ' as datetime), 113) AND convert(datetime, cast('" + dtpToDate.Text + " 23:59:59' as datetime) ,113) ";

            if (!string.IsNullOrWhiteSpace(this.txtCloseStationId.Text))
                strFilter += " AND PT." + clsPOSDBConstants.TransHeader_Fld_StClosedID + " IN (" + this.txtCloseStationId.Text.Trim().Replace("'", "''") + ")";
            if (!string.IsNullOrWhiteSpace(this.txtEODId.Text))
                strFilter += " AND PT." + clsPOSDBConstants.TransHeader_Fld_EODID + " IN (" + this.txtEODId.Text.Trim().Replace("'", "''") + ")";
            if (Convert.ToString(this.txtDepartment.Tag).Trim() != "")
                strFilter += " AND I.DepartmentID in (" + this.txtDepartment.Tag.ToString().Trim() + ")";
            if (Convert.ToString(this.txtSubDepartment.Tag).Trim() != "")
                strFilter += " AND SD.SubDepartmentID in (" + this.txtSubDepartment.Tag.ToString().Trim() + ")";

            if (this.txtStationID.Text.Trim() != "")
                strFilter += " and PT.StationID = '" + this.txtStationID.Text.Trim().Replace("'", "''") + "' ";

            if (this.txtUserID.Text.Trim() != "")
                strFilter += " AND PT.UserID = '" + this.txtUserID.Text.Trim().Replace("'", "''") + "' ";

            if (optionSetForItems.CheckedIndex == 1) //Only Rx Items
                strFilter += " AND PTD.ItemID ='RX'";
            else if (optionSetForItems.CheckedIndex == 2) //Exclude Rx Items
                strFilter += " AND PTD.ItemID !='RX'";

            if (Convert.ToString(this.txtPaymentType.Tag).Trim() != "")
                strFilter += " AND PT.TransID IN (SELECT TransId FROM POSTransPayment WHERE TransTypeCode in (" + txtPaymentType.Tag.ToString().Trim() + ") )";
            return strFilter;
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            dtpFromDate.Value = DateTime.Now;
            dtpToDate.Value = DateTime.Now;
            txtCloseStationId.Text = "";
            txtEODId.Text = "";
            txtDepartment.Text = "";
            txtDepartment.Tag = "";
            txtSubDepartment.Text = "";
            txtSubDepartment.Tag = "";
            txtStationID.Text = "";
            txtUserID.Text = "";
            txtTaxCode.Text = "";
            txtTaxCode.Tag = "";
            txtPaymentType.Text = "";
            txtPaymentType.Tag = "";            
            optionSetForItems.CheckedIndex = 0;
            dataGridList_RowsAdded(null, null);
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

        private void txtEODId_TextChanged(object sender, EventArgs e)
        {
            if (txtEODId.Text.Trim() == "")
            {
                dtpFromDate.Enabled = dtpToDate.Enabled = true;
            }
            else
            {
                dtpFromDate.Enabled = dtpToDate.Enabled = false;
            }
        }

        private void txtCloseStationId_TextChanged(object sender, EventArgs e)
        {
            if (txtCloseStationId.Text.Trim() == "")
            {
                dtpFromDate.Enabled = dtpToDate.Enabled = true;
            }
            else
            {
                dtpFromDate.Enabled = dtpToDate.Enabled = false;
            }
        }
        #endregion

        #region PRIMEPOS-2485 22-Mar-2021 JY Added
        public bool bSendPrint = true;
        private ReportClass oReport = new ReportClass();
        public usrDateRangeParams customControl;
        private const string ReportName = "SalesTaxSummaryReport";

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

            foreach (DataRow odr in dt.Select("ControlsName = '" + this.dtpFromDate.Tag + " ' "))
            {
                if (double.TryParse(odr["ControlsValue"].ToString().Trim(), out Num))
                {
                    dtpFromDate.Value = DateTime.Now.AddDays(Convert.ToDouble(odr["ControlsValue"].ToString().Trim()) * -1);
                }
                else
                {
                    dtpFromDate.Value = odr["ControlsValue"].ToString().Trim();
                }
            }

            foreach (DataRow odr in dt.Select("ControlsName = '" + this.dtpToDate.Tag + "' "))
            {
                if (double.TryParse(odr["ControlsValue"].ToString().Trim(), out Num))
                {
                    dtpToDate.Value = DateTime.Now.AddDays(Convert.ToDouble(odr["ControlsValue"].ToString().Trim()) * -1);
                }
                else
                {
                    dtpToDate.Value = odr["ControlsValue"].ToString().Trim();
                }
            }
        }
        #endregion

        public bool RunTask(int TaskId, ref string filePath, bool bsendToPrint, ref string sNoOfRecordAffect)
        {
            SetControlParameters(TaskId);
            bSendPrint = bsendToPrint;
            //dtpFromDate.Value = DateTime.Now.AddDays(Left-60);
            //dtpToDate.Value = DateTime.Now;
            Preview(bSendPrint, true);
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