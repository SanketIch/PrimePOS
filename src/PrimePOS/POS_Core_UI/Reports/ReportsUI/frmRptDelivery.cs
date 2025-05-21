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
namespace POS_Core_UI.Reports.ReportsUI
{
	/// <summary>
	/// Summary description for frmRptSalesByCustomer.
	/// </summary>
	public class frmRptDelivery : System.Windows.Forms.Form
	{
		private System.Windows.Forms.GroupBox groupBox1;
		private Infragistics.Win.Misc.UltraLabel ultraLabel12;
        private Infragistics.Win.Misc.UltraLabel lblTransactionType;
		private Infragistics.Win.Misc.UltraLabel ultraLabel20;
		private Infragistics.Win.Misc.UltraLabel ultraLabel19;
		private Infragistics.Win.UltraWinSchedule.UltraCalendarCombo dtpSaleEndDate;
		private Infragistics.Win.UltraWinSchedule.UltraCalendarCombo dtpSaleStartDate;
		private System.Windows.Forms.GroupBox groupBox2;
		private Infragistics.Win.Misc.UltraButton btnPrint;
		private Infragistics.Win.Misc.UltraButton btnView;
		private Infragistics.Win.Misc.UltraButton btnClose;
        private Infragistics.Win.Misc.UltraLabel lblCustomerName;
        public UltraTextEditor txtCustomer;
        private RadioButton optItemsOTC;
        private RadioButton optItemsRX;
        private RadioButton optItemsAll;
        private RadioButton optSortByStreetAddress;
        private RadioButton optSortByCustomerName;
        private Infragistics.Win.Misc.UltraLabel ultraLabel1;
        private GroupBox groupBox3;
        public UltraTextEditor txtUser;
        private Infragistics.Win.Misc.UltraLabel ultraLabel5;
        public UltraTextEditor txtStation;
        private Infragistics.Win.Misc.UltraLabel ultraLabel3;
        private CheckBox chkCustomerSeperatePage;
        private RadioButton optSortByZip;
        private UltraDateTimeEditor dtpStartTime;
        private UltraDateTimeEditor dtpEndTime;

        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;

		public frmRptDelivery()
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
            Infragistics.Win.Appearance appearance16 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance17 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance18 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinEditors.EditorButton editorButton1 = new Infragistics.Win.UltraWinEditors.EditorButton();
            Infragistics.Win.Appearance appearance7 = new Infragistics.Win.Appearance();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmRptDelivery));
            Infragistics.Win.Appearance appearance19 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance20 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinSchedule.CalendarCombo.DateButton dateButton3 = new Infragistics.Win.UltraWinSchedule.CalendarCombo.DateButton();
            Infragistics.Win.UltraWinSchedule.CalendarCombo.DateButton dateButton4 = new Infragistics.Win.UltraWinSchedule.CalendarCombo.DateButton();
            Infragistics.Win.Appearance appearance21 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance22 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance23 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance24 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance25 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance26 = new Infragistics.Win.Appearance();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.chkCustomerSeperatePage = new System.Windows.Forms.CheckBox();
            this.txtUser = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.ultraLabel5 = new Infragistics.Win.Misc.UltraLabel();
            this.txtStation = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.ultraLabel3 = new Infragistics.Win.Misc.UltraLabel();
            this.optItemsOTC = new System.Windows.Forms.RadioButton();
            this.optItemsRX = new System.Windows.Forms.RadioButton();
            this.optItemsAll = new System.Windows.Forms.RadioButton();
            this.lblCustomerName = new Infragistics.Win.Misc.UltraLabel();
            this.txtCustomer = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.ultraLabel20 = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabel19 = new Infragistics.Win.Misc.UltraLabel();
            this.dtpSaleEndDate = new Infragistics.Win.UltraWinSchedule.UltraCalendarCombo();
            this.dtpSaleStartDate = new Infragistics.Win.UltraWinSchedule.UltraCalendarCombo();
            this.ultraLabel12 = new Infragistics.Win.Misc.UltraLabel();
            this.lblTransactionType = new Infragistics.Win.Misc.UltraLabel();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnPrint = new Infragistics.Win.Misc.UltraButton();
            this.btnClose = new Infragistics.Win.Misc.UltraButton();
            this.btnView = new Infragistics.Win.Misc.UltraButton();
            this.optSortByStreetAddress = new System.Windows.Forms.RadioButton();
            this.optSortByCustomerName = new System.Windows.Forms.RadioButton();
            this.ultraLabel1 = new Infragistics.Win.Misc.UltraLabel();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.optSortByZip = new System.Windows.Forms.RadioButton();
            this.dtpStartTime = new Infragistics.Win.UltraWinEditors.UltraDateTimeEditor();
            this.dtpEndTime = new Infragistics.Win.UltraWinEditors.UltraDateTimeEditor();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtUser)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtStation)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCustomer)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtpSaleEndDate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtpSaleStartDate)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dtpStartTime)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtpEndTime)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.dtpEndTime);
            this.groupBox1.Controls.Add(this.dtpStartTime);
            this.groupBox1.Controls.Add(this.chkCustomerSeperatePage);
            this.groupBox1.Controls.Add(this.txtUser);
            this.groupBox1.Controls.Add(this.ultraLabel5);
            this.groupBox1.Controls.Add(this.txtStation);
            this.groupBox1.Controls.Add(this.ultraLabel3);
            this.groupBox1.Controls.Add(this.optItemsOTC);
            this.groupBox1.Controls.Add(this.optItemsRX);
            this.groupBox1.Controls.Add(this.optItemsAll);
            this.groupBox1.Controls.Add(this.lblCustomerName);
            this.groupBox1.Controls.Add(this.txtCustomer);
            this.groupBox1.Controls.Add(this.ultraLabel20);
            this.groupBox1.Controls.Add(this.ultraLabel19);
            this.groupBox1.Controls.Add(this.dtpSaleEndDate);
            this.groupBox1.Controls.Add(this.dtpSaleStartDate);
            this.groupBox1.Controls.Add(this.ultraLabel12);
            this.groupBox1.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.groupBox1.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(16, 60);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(424, 246);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            // 
            // chkCustomerSeperatePage
            // 
            this.chkCustomerSeperatePage.AutoSize = true;
            this.chkCustomerSeperatePage.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chkCustomerSeperatePage.ForeColor = System.Drawing.Color.White;
            this.chkCustomerSeperatePage.Location = new System.Drawing.Point(135, 216);
            this.chkCustomerSeperatePage.Name = "chkCustomerSeperatePage";
            this.chkCustomerSeperatePage.Size = new System.Drawing.Size(270, 21);
            this.chkCustomerSeperatePage.TabIndex = 10;
            this.chkCustomerSeperatePage.Text = "Seperate Page For Each Customer ";
            this.chkCustomerSeperatePage.UseVisualStyleBackColor = true;
            // 
            // txtUser
            // 
            appearance1.BorderColor = System.Drawing.Color.Lime;
            appearance1.BorderColor3DBase = System.Drawing.Color.Lime;
            this.txtUser.Appearance = appearance1;
            this.txtUser.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.txtUser.Location = new System.Drawing.Point(135, 154);
            this.txtUser.Name = "txtUser";
            this.txtUser.Size = new System.Drawing.Size(180, 23);
            this.txtUser.TabIndex = 6;
            this.txtUser.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // ultraLabel5
            // 
            appearance16.ForeColor = System.Drawing.Color.White;
            this.ultraLabel5.Appearance = appearance16;
            this.ultraLabel5.AutoSize = true;
            this.ultraLabel5.Location = new System.Drawing.Point(18, 156);
            this.ultraLabel5.Name = "ultraLabel5";
            this.ultraLabel5.Size = new System.Drawing.Size(36, 18);
            this.ultraLabel5.TabIndex = 35;
            this.ultraLabel5.Text = "User";
            // 
            // txtStation
            // 
            appearance3.BorderColor = System.Drawing.Color.Lime;
            appearance3.BorderColor3DBase = System.Drawing.Color.Lime;
            this.txtStation.Appearance = appearance3;
            this.txtStation.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.txtStation.Location = new System.Drawing.Point(135, 123);
            this.txtStation.Name = "txtStation";
            this.txtStation.Size = new System.Drawing.Size(180, 23);
            this.txtStation.TabIndex = 5;
            this.txtStation.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // ultraLabel3
            // 
            appearance17.ForeColor = System.Drawing.Color.White;
            this.ultraLabel3.Appearance = appearance17;
            this.ultraLabel3.AutoSize = true;
            this.ultraLabel3.Location = new System.Drawing.Point(18, 125);
            this.ultraLabel3.Name = "ultraLabel3";
            this.ultraLabel3.Size = new System.Drawing.Size(54, 18);
            this.ultraLabel3.TabIndex = 32;
            this.ultraLabel3.Text = "Station";
            // 
            // optItemsOTC
            // 
            this.optItemsOTC.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.optItemsOTC.ForeColor = System.Drawing.Color.White;
            this.optItemsOTC.Location = new System.Drawing.Point(296, 187);
            this.optItemsOTC.Name = "optItemsOTC";
            this.optItemsOTC.Size = new System.Drawing.Size(122, 26);
            this.optItemsOTC.TabIndex = 9;
            this.optItemsOTC.Text = "OTC Only";
            // 
            // optItemsRX
            // 
            this.optItemsRX.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.optItemsRX.ForeColor = System.Drawing.Color.White;
            this.optItemsRX.Location = new System.Drawing.Point(190, 187);
            this.optItemsRX.Name = "optItemsRX";
            this.optItemsRX.Size = new System.Drawing.Size(122, 26);
            this.optItemsRX.TabIndex = 8;
            this.optItemsRX.Text = "RX Only";
            // 
            // optItemsAll
            // 
            this.optItemsAll.Checked = true;
            this.optItemsAll.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.optItemsAll.ForeColor = System.Drawing.Color.White;
            this.optItemsAll.Location = new System.Drawing.Point(135, 187);
            this.optItemsAll.Name = "optItemsAll";
            this.optItemsAll.Size = new System.Drawing.Size(46, 26);
            this.optItemsAll.TabIndex = 7;
            this.optItemsAll.TabStop = true;
            this.optItemsAll.Text = "All";
            // 
            // lblCustomerName
            // 
            appearance18.ForeColor = System.Drawing.Color.Black;
            this.lblCustomerName.Appearance = appearance18;
            this.lblCustomerName.BackColorInternal = System.Drawing.Color.Transparent;
            this.lblCustomerName.Location = new System.Drawing.Point(183, 94);
            this.lblCustomerName.Name = "lblCustomerName";
            this.lblCustomerName.Size = new System.Drawing.Size(235, 18);
            this.lblCustomerName.TabIndex = 28;
            this.lblCustomerName.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // txtCustomer
            // 
            appearance6.BorderColor = System.Drawing.Color.Lime;
            appearance6.BorderColor3DBase = System.Drawing.Color.Lime;
            this.txtCustomer.Appearance = appearance6;
            this.txtCustomer.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance7.BackColor = System.Drawing.Color.White;
            appearance7.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(231)))), ((int)(((byte)(230)))));
            appearance7.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance7.BorderAlpha = Infragistics.Win.Alpha.Transparent;
            appearance7.Image = ((object)(resources.GetObject("appearance7.Image")));
            appearance7.ImageAlpha = Infragistics.Win.Alpha.Opaque;
            appearance7.ImageBackgroundAlpha = Infragistics.Win.Alpha.Opaque;
            appearance7.ImageBackgroundStyle = Infragistics.Win.ImageBackgroundStyle.Centered;
            editorButton1.Appearance = appearance7;
            editorButton1.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            editorButton1.Text = "";
            editorButton1.Width = 20;
            this.txtCustomer.ButtonsRight.Add(editorButton1);
            this.txtCustomer.Location = new System.Drawing.Point(136, 92);
            this.txtCustomer.Name = "txtCustomer";
            this.txtCustomer.Size = new System.Drawing.Size(42, 23);
            this.txtCustomer.TabIndex = 4;
            this.txtCustomer.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.txtCustomer.EditorButtonClick += new Infragistics.Win.UltraWinEditors.EditorButtonEventHandler(this.txtCustomer_EditorButtonClick);
            this.txtCustomer.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtCustomer_KeyUp);
            this.txtCustomer.Leave += new System.EventHandler(this.txtCustomer_Leave);
            // 
            // ultraLabel20
            // 
            appearance19.ForeColor = System.Drawing.Color.White;
            this.ultraLabel20.Appearance = appearance19;
            this.ultraLabel20.Location = new System.Drawing.Point(19, 38);
            this.ultraLabel20.Name = "ultraLabel20";
            this.ultraLabel20.Size = new System.Drawing.Size(116, 14);
            this.ultraLabel20.TabIndex = 20;
            this.ultraLabel20.Text = "Start Date";
            // 
            // ultraLabel19
            // 
            appearance20.ForeColor = System.Drawing.Color.White;
            this.ultraLabel19.Appearance = appearance20;
            this.ultraLabel19.Location = new System.Drawing.Point(19, 67);
            this.ultraLabel19.Name = "ultraLabel19";
            this.ultraLabel19.Size = new System.Drawing.Size(106, 14);
            this.ultraLabel19.TabIndex = 22;
            this.ultraLabel19.Text = "End Date";
            // 
            // dtpSaleEndDate
            // 
            this.dtpSaleEndDate.AllowNull = false;
            this.dtpSaleEndDate.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Office2003ToolbarButton;
            this.dtpSaleEndDate.DateButtons.Add(dateButton3);
            this.dtpSaleEndDate.Format = "MM/dd/yyyy";
            this.dtpSaleEndDate.Location = new System.Drawing.Point(136, 63);
            this.dtpSaleEndDate.Name = "dtpSaleEndDate";
            this.dtpSaleEndDate.NonAutoSizeHeight = 10;
            this.dtpSaleEndDate.Size = new System.Drawing.Size(109, 22);
            this.dtpSaleEndDate.TabIndex = 2;
            this.dtpSaleEndDate.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.dtpSaleEndDate.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.dtpSaleEndDate.Value = "5/25/2004";
            // 
            // dtpSaleStartDate
            // 
            this.dtpSaleStartDate.AllowNull = false;
            this.dtpSaleStartDate.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Office2003ToolbarButton;
            this.dtpSaleStartDate.DateButtons.Add(dateButton4);
            this.dtpSaleStartDate.Format = "MM/dd/yyyy";
            this.dtpSaleStartDate.Location = new System.Drawing.Point(136, 34);
            this.dtpSaleStartDate.Name = "dtpSaleStartDate";
            this.dtpSaleStartDate.NonAutoSizeHeight = 10;
            this.dtpSaleStartDate.Size = new System.Drawing.Size(109, 22);
            this.dtpSaleStartDate.TabIndex = 0;
            this.dtpSaleStartDate.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.dtpSaleStartDate.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.dtpSaleStartDate.Value = "5/25/2004";
            // 
            // ultraLabel12
            // 
            appearance21.ForeColor = System.Drawing.Color.White;
            this.ultraLabel12.Appearance = appearance21;
            this.ultraLabel12.AutoSize = true;
            this.ultraLabel12.Location = new System.Drawing.Point(19, 94);
            this.ultraLabel12.Name = "ultraLabel12";
            this.ultraLabel12.Size = new System.Drawing.Size(72, 18);
            this.ultraLabel12.TabIndex = 6;
            this.ultraLabel12.Text = "Customer";
            // 
            // lblTransactionType
            // 
            appearance22.ForeColor = System.Drawing.Color.White;
            appearance22.ForeColorDisabled = System.Drawing.Color.White;
            appearance22.TextHAlignAsString = "Center";
            this.lblTransactionType.Appearance = appearance22;
            this.lblTransactionType.BackColorInternal = System.Drawing.Color.Transparent;
            this.lblTransactionType.Font = new System.Drawing.Font("Arial", 18F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTransactionType.Location = new System.Drawing.Point(16, 16);
            this.lblTransactionType.Name = "lblTransactionType";
            this.lblTransactionType.Size = new System.Drawing.Size(424, 30);
            this.lblTransactionType.TabIndex = 26;
            this.lblTransactionType.Text = "Delivery List";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnPrint);
            this.groupBox2.Controls.Add(this.btnClose);
            this.groupBox2.Controls.Add(this.btnView);
            this.groupBox2.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.groupBox2.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.Location = new System.Drawing.Point(17, 400);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(424, 57);
            this.groupBox2.TabIndex = 12;
            this.groupBox2.TabStop = false;
            // 
            // btnPrint
            // 
            appearance23.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(62)))), ((int)(((byte)(176)))));
            appearance23.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance23.FontData.BoldAsString = "True";
            appearance23.ForeColor = System.Drawing.Color.White;
            appearance23.Image = ((object)(resources.GetObject("appearance23.Image")));
            this.btnPrint.Appearance = appearance23;
            this.btnPrint.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            this.btnPrint.Location = new System.Drawing.Point(147, 19);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(85, 26);
            this.btnPrint.TabIndex = 13;
            this.btnPrint.Text = "&Print";
            this.btnPrint.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            // 
            // btnClose
            // 
            appearance24.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(62)))), ((int)(((byte)(176)))));
            appearance24.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance24.FontData.BoldAsString = "True";
            appearance24.ForeColor = System.Drawing.Color.White;
            appearance24.Image = ((object)(resources.GetObject("appearance24.Image")));
            this.btnClose.Appearance = appearance24;
            this.btnClose.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(331, 20);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(85, 26);
            this.btnClose.TabIndex = 14;
            this.btnClose.Text = "&Close";
            this.btnClose.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnView
            // 
            appearance25.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(62)))), ((int)(((byte)(176)))));
            appearance25.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance25.FontData.BoldAsString = "True";
            appearance25.ForeColor = System.Drawing.Color.White;
            appearance25.Image = ((object)(resources.GetObject("appearance25.Image")));
            this.btnView.Appearance = appearance25;
            this.btnView.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            this.btnView.Location = new System.Drawing.Point(239, 20);
            this.btnView.Name = "btnView";
            this.btnView.Size = new System.Drawing.Size(85, 26);
            this.btnView.TabIndex = 12;
            this.btnView.Text = "&View";
            this.btnView.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnView.Click += new System.EventHandler(this.btnView_Click_1);
            // 
            // optSortByStreetAddress
            // 
            this.optSortByStreetAddress.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.optSortByStreetAddress.ForeColor = System.Drawing.Color.White;
            this.optSortByStreetAddress.Location = new System.Drawing.Point(137, 44);
            this.optSortByStreetAddress.Name = "optSortByStreetAddress";
            this.optSortByStreetAddress.Size = new System.Drawing.Size(145, 26);
            this.optSortByStreetAddress.TabIndex = 10;
            this.optSortByStreetAddress.Text = "Street Address";
            // 
            // optSortByCustomerName
            // 
            this.optSortByCustomerName.Checked = true;
            this.optSortByCustomerName.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.optSortByCustomerName.ForeColor = System.Drawing.Color.White;
            this.optSortByCustomerName.Location = new System.Drawing.Point(135, 18);
            this.optSortByCustomerName.Name = "optSortByCustomerName";
            this.optSortByCustomerName.Size = new System.Drawing.Size(147, 26);
            this.optSortByCustomerName.TabIndex = 9;
            this.optSortByCustomerName.TabStop = true;
            this.optSortByCustomerName.Text = "Customer Name";
            // 
            // ultraLabel1
            // 
            appearance26.ForeColor = System.Drawing.Color.White;
            this.ultraLabel1.Appearance = appearance26;
            this.ultraLabel1.AutoSize = true;
            this.ultraLabel1.Location = new System.Drawing.Point(18, 23);
            this.ultraLabel1.Name = "ultraLabel1";
            this.ultraLabel1.Size = new System.Drawing.Size(67, 18);
            this.ultraLabel1.TabIndex = 35;
            this.ultraLabel1.Text = "Sort By :";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.optSortByZip);
            this.groupBox3.Controls.Add(this.ultraLabel1);
            this.groupBox3.Controls.Add(this.optSortByStreetAddress);
            this.groupBox3.Controls.Add(this.optSortByCustomerName);
            this.groupBox3.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.groupBox3.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox3.Location = new System.Drawing.Point(17, 307);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(424, 76);
            this.groupBox3.TabIndex = 9;
            this.groupBox3.TabStop = false;
            // 
            // optSortByZip
            // 
            this.optSortByZip.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.optSortByZip.ForeColor = System.Drawing.Color.White;
            this.optSortByZip.Location = new System.Drawing.Point(288, 44);
            this.optSortByZip.Name = "optSortByZip";
            this.optSortByZip.Size = new System.Drawing.Size(128, 26);
            this.optSortByZip.TabIndex = 11;
            this.optSortByZip.Text = "ZIP Code";
            // 
            // dtpStartTime
            // 
            this.dtpStartTime.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Office2003ToolbarButton;
            this.dtpStartTime.DateTime = new System.DateTime(2018, 11, 23, 12, 0, 0, 0);
            this.dtpStartTime.DropDownButtonDisplayStyle = Infragistics.Win.ButtonDisplayStyle.Never;
            this.dtpStartTime.FormatProvider = new System.Globalization.CultureInfo("bg-BG");
            this.dtpStartTime.Location = new System.Drawing.Point(246, 33);
            this.dtpStartTime.MaskInput = "{time}";
            this.dtpStartTime.Name = "dtpStartTime";
            this.dtpStartTime.Size = new System.Drawing.Size(69, 25);
            this.dtpStartTime.SpinButtonDisplayStyle = Infragistics.Win.ButtonDisplayStyle.Always;
            this.dtpStartTime.TabIndex = 1;
            this.dtpStartTime.Value = new System.DateTime(2018, 11, 23, 12, 0, 0, 0);
            // 
            // dtpEndTime
            // 
            this.dtpEndTime.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Office2003ToolbarButton;
            this.dtpEndTime.DropDownButtonDisplayStyle = Infragistics.Win.ButtonDisplayStyle.Never;
            this.dtpEndTime.FormatProvider = new System.Globalization.CultureInfo("bg-BG");
            this.dtpEndTime.Location = new System.Drawing.Point(246, 62);
            this.dtpEndTime.MaskInput = "{time}";
            this.dtpEndTime.Name = "dtpEndTime";
            this.dtpEndTime.Size = new System.Drawing.Size(69, 25);
            this.dtpEndTime.SpinButtonDisplayStyle = Infragistics.Win.ButtonDisplayStyle.Always;
            this.dtpEndTime.TabIndex = 3;
            // 
            // frmRptDelivery
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(108)))), ((int)(((byte)(172)))));
            this.ClientSize = new System.Drawing.Size(459, 473);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.lblTransactionType);
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.Name = "frmRptDelivery";
            this.Text = "Delivery List";
            this.Load += new System.EventHandler(this.frmRptSalesByCustomer_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmRptSalesByCustomer_KeyDown);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtUser)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtStation)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCustomer)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtpSaleEndDate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtpSaleStartDate)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dtpStartTime)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtpEndTime)).EndInit();
            this.ResumeLayout(false);

		}
		#endregion

		private void frmRptSalesByCustomer_Load(object sender, System.EventArgs e)
		{
			this.dtpSaleStartDate.Enter += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
			this.dtpSaleStartDate.Leave += new System.EventHandler(clsUIHelper.AfterExitEditMode);

			this.dtpSaleEndDate.Enter += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
			this.dtpSaleEndDate.Leave += new System.EventHandler(clsUIHelper.AfterExitEditMode);

            this.dtpStartTime.Enter += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
            this.dtpStartTime.Leave += new System.EventHandler(clsUIHelper.AfterExitEditMode);

            this.dtpEndTime.Enter += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
            this.dtpEndTime.Leave += new System.EventHandler(clsUIHelper.AfterExitEditMode);

            this.txtCustomer.AfterEnterEditMode += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
			this.txtCustomer.AfterExitEditMode += new System.EventHandler(clsUIHelper.AfterExitEditMode);

            this.txtStation.AfterEnterEditMode += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
            this.txtStation.AfterExitEditMode += new System.EventHandler(clsUIHelper.AfterExitEditMode);

            this.txtUser.AfterEnterEditMode += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
            this.txtUser.AfterExitEditMode += new System.EventHandler(clsUIHelper.AfterExitEditMode);

			this.Left=(frmMain.getInstance().Width-frmMain.getInstance().ultraExplorerBar1.Width-this.Width)/2;
			this.Top=(frmMain.getInstance().Height-this.Height)/2;
			
			clsUIHelper.setColorSchecme(this);
			this.dtpSaleEndDate.Value=DateTime.Now;
			this.dtpSaleStartDate.Value=DateTime.Now;
            this.dtpStartTime.Value = "00:00";
            this.dtpEndTime.Value = "23:59";
        }

		private void btnView_Click(object sender, System.EventArgs e)
		{
			PreviewReport(false);
		}

		private void btnClose_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}

		private void frmRptSalesByCustomer_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			try
			{
				if(e.KeyData==System.Windows.Forms.Keys.Enter)
				{
					this.SelectNextControl(this.ActiveControl,true,true,true,true);
				}
				else if (e.KeyData==Keys.Escape)
					this.Close();
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
                if (Convert.ToDateTime(this.dtpSaleEndDate.Value.ToString()).Date < Convert.ToDateTime(this.dtpSaleStartDate.Value.ToString()).Date)
                {
                    throw (new Exception("End date cannot be less than Start date."));
                }

                #region PRIMEPOS-2328 23-Nov-2018 JY Added 
                DateTime dtStartDate, dtEndDate;
                try
                {
                    dtStartDate = DateTime.ParseExact(Convert.ToDateTime(this.dtpSaleStartDate.Text).Date.ToString("MM/dd/yy") + " " + Convert.ToDateTime(this.dtpStartTime.Text).ToString("HH:mm"),"MM/dd/yy HH:mm", System.Globalization.CultureInfo.InvariantCulture);
                    dtEndDate = DateTime.ParseExact(Convert.ToDateTime(this.dtpSaleEndDate.Text).Date.ToString("MM/dd/yy") + " " + Convert.ToDateTime(this.dtpEndTime.Text).ToString("HH:mm"), "MM/dd/yy HH:mm", System.Globalization.CultureInfo.InvariantCulture);
                }
                catch(Exception Ex)
                {
                    dtStartDate = Convert.ToDateTime(this.dtpSaleStartDate.Text).Date;
                    dtEndDate = Convert.ToDateTime(this.dtpSaleEndDate.Text).Date;
                }
                #endregion

                this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
                rptDeliveryReport oRpt = new rptDeliveryReport();

				String strQuery;

                DataSet oDataSet = new DataSet();
                strQuery = "Select Cust.CustomerID,Cust.CustomerName + ',' + Cust.FirstName as Customer,Address1 + ' ' + Address2 + ' ' + City + ' ' + State + ' ' + Zip as Address ,PhoneOff " +
                    " ,PT.TransID,PT.TransDate ,PTD.ExtendedPrice,PTD.Qty,PTD.ItemID,PTD.ItemDescription, PTD.Discount,PTD.TaxAmount   "
                    + " FROM POSTransaction PT Inner Join POSTransactionDetail PTD On(PT.TransID=PTD.TransID) "
                    + " Inner Join Customer Cust on (cust.CustomerID=PT.CustomerID) "
                    + " Inner Join Item On (Item.ItemID=PTD.ItemID) Where IsNull(PT.IsDelivery,0)=1 "
                    + " and Cust.AcctNumber<>-1  and TransDate between cast('" + dtStartDate + "' as datetime) and cast('" + dtEndDate + "' as datetime) "
                    + " And PT.TransType=1 ";

                if (this.txtCustomer.Tag != null && this.txtCustomer.Tag!="")
                {
                    strQuery += " and PT.CustomerID=" + POS_Core.Resources.Configuration.convertNullToInt(this.txtCustomer.Tag.ToString().Trim()).ToString() + " ";
                }

                if (txtUser.Text.Trim().Length == 0)
                {
                    clsReports.setCRTextObjectText("txtUser", "User : ALL" , oRpt);
                }
                else
                {
                    strQuery += " and PT.UserID='" + txtUser.Text + "' ";
                    clsReports.setCRTextObjectText("txtUser", "User : "+txtUser.Text, oRpt);
                }

                if (txtStation.Text.Trim().Length == 0)
                {
                    clsReports.setCRTextObjectText("txtStation", "Station : ALL", oRpt);
                }
                else
                {
                    strQuery += " and PT.StationID='" + txtStation.Text + "' ";
                    clsReports.setCRTextObjectText("txtStation", "Station : " + txtStation.Text, oRpt);
                }


                if (optItemsRX.Checked == true)
                {
                    strQuery += " and PTD.ItemID='RX' ";
                }
                else if (optItemsOTC.Checked == true)
                {
                    strQuery += "  and PTD.ItemID<>'RX' ";
                }

                if (optSortByStreetAddress.Checked == true)
                {
                    strQuery += " order by Cust.Address1,PT.TransID ";
                }
                else if (optSortByCustomerName.Checked == true)
                {
                    strQuery += " order by 1,PT.TransID ";
                }
                else if (optSortByZip.Checked == true)
                {
                    strQuery += " order by Cust.Zip,PT.TransID ";
                }

                clsReports.setCRTextObjectText("txtFromDate", "From :" + this.dtpSaleStartDate.Text, oRpt);
                clsReports.setCRTextObjectText("txtToDate", "To :" + dtpSaleEndDate.Text, oRpt);

                DataSet ds = clsReports.GetReportSource(strQuery);
                oRpt.Database.Tables[0].SetDataSource(ds.Tables[0]);

                if (chkCustomerSeperatePage.Checked == true)
                {
                    clsReports.SetRepParam(oRpt, "SeperateCustomer", "1");
                }
                else
                {
                    clsReports.SetRepParam(oRpt, "SeperateCustomer", "0");
                }
                clsReports.DStoExport = ds; //PRIMEPOS-2471 16-Feb-2021 JY Added
                clsReports.Preview(blnPrint,  oRpt);
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

        private void txtCustomer_EditorButtonClick(object sender, Infragistics.Win.UltraWinEditors.EditorButtonEventArgs e)
        {
            try
            {
                SearchCustomer();
            }
            catch (Exception) { }
        }

        private void SearchCustomer()
        {
            try
            {
                //frmCustomerSearch oSearch = new frmCustomerSearch(txtCustomer.Text); 
                frmSearchMain oSearch = new frmSearchMain(txtCustomer.Text, true, clsPOSDBConstants.Customer_tbl);  //18-Dec-2017 JY Added new reference
                oSearch.ActiveOnly = 1;
                oSearch.ShowDialog(this);
                if (!oSearch.IsCanceled)
                {
                    string strCode = oSearch.SelectedRowID();
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
            //FKEdit("-1", clsPOSDBConstants.Customer_tbl);
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
                    CustomerRow oCustomerRow = null;
                    oCustomerData = oCustomer.Populate(code);
                    oCustomerRow = oCustomerData.Customer[0];
                    if (oCustomerRow != null)
                    {
                        this.txtCustomer.Text = oCustomerRow.AccountNumber.ToString();
                        this.txtCustomer.Tag = oCustomerRow.CustomerId.ToString();
                        this.lblCustomerName.Text = oCustomerRow.CustomerFullName;
                        //Added By Dharmendra(SRT) which will be required when processing
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
                    this.txtCustomer.Tag = "";
                    this.lblCustomerName.Text = "";
                }
            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }

        private void txtCustomer_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F4)
            {
                try
                {
                    SearchCustomer();
                    
                }
                catch (Exception) { }
                e.Handled = true;
            }
        }
	}
}
