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
using POS_Core.Resources;
//using POS_Core.DataAccess;

namespace POS_Core_UI
{
	/// <summary>
	/// Summary description for frmTimesheet.
	/// </summary>
	public class frmTimesheet : System.Windows.Forms.Form
	{
		private TimesheetData oTimesheetData= new TimesheetData();
		private TimesheetRow oTimesheetRow ;
        private Timesheet oBRTimesheet = new Timesheet();

        //private bool isManualTimeIn = false;
        //private bool isManualTimeOut = false;

        #region system variables
        private Infragistics.Win.Misc.UltraLabel ultraLabel12;
		private Infragistics.Win.Misc.UltraLabel lblTransactionType;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor txtUserID;
		private Infragistics.Win.Misc.UltraButton btnClose;
        private Infragistics.Win.Misc.UltraButton btnSave;
        private Infragistics.Win.Misc.UltraLabel lblUserDescription;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ToolTip toolTip1;
        private Infragistics.Win.Misc.UltraLabel ultraLabel4;
        private Infragistics.Win.Misc.UltraLabel ultraLabel1;
        private Infragistics.Win.UltraWinSchedule.UltraCalendarCombo dtpEndDate;
        private Infragistics.Win.UltraWinSchedule.UltraCalendarCombo dtpStartDate;
        private Infragistics.Win.Misc.UltraLabel ultraLabel5;
        private Infragistics.Win.Misc.UltraLabel ultraLabel2;
        private UltraComboEditor cboEndTime;
        private UltraComboEditor cboStartTime;
        private Infragistics.Win.Misc.UltraButton btnClear;
        private Infragistics.Win.Misc.UltraButton btnSearch;
        private Infragistics.Win.Misc.UltraLabel lblMessage;
		private System.ComponentModel.IContainer components;
        #endregion

		public frmTimesheet()
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
            this.components = new System.ComponentModel.Container();
            Infragistics.Win.UltraWinEditors.EditorButton editorButton1 = new Infragistics.Win.UltraWinEditors.EditorButton();
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmTimesheet));
            Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance14 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance16 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance28 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinSchedule.CalendarCombo.DateButton dateButton1 = new Infragistics.Win.UltraWinSchedule.CalendarCombo.DateButton();
            Infragistics.Win.Appearance appearance10 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinSchedule.CalendarCombo.DateButton dateButton2 = new Infragistics.Win.UltraWinSchedule.CalendarCombo.DateButton();
            Infragistics.Win.Appearance appearance9 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance7 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance35 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance36 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance8 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance17 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance18 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance15 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance27 = new Infragistics.Win.Appearance();
            this.lblUserDescription = new Infragistics.Win.Misc.UltraLabel();
            this.txtUserID = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.ultraLabel12 = new Infragistics.Win.Misc.UltraLabel();
            this.btnClose = new Infragistics.Win.Misc.UltraButton();
            this.btnSave = new Infragistics.Win.Misc.UltraButton();
            this.lblTransactionType = new Infragistics.Win.Misc.UltraLabel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.dtpEndDate = new Infragistics.Win.UltraWinSchedule.UltraCalendarCombo();
            this.dtpStartDate = new Infragistics.Win.UltraWinSchedule.UltraCalendarCombo();
            this.ultraLabel5 = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabel2 = new Infragistics.Win.Misc.UltraLabel();
            this.cboEndTime = new Infragistics.Win.UltraWinEditors.UltraComboEditor();
            this.cboStartTime = new Infragistics.Win.UltraWinEditors.UltraComboEditor();
            this.ultraLabel4 = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabel1 = new Infragistics.Win.Misc.UltraLabel();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnSearch = new Infragistics.Win.Misc.UltraButton();
            this.btnClear = new Infragistics.Win.Misc.UltraButton();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.lblMessage = new Infragistics.Win.Misc.UltraLabel();
            ((System.ComponentModel.ISupportInitialize)(this.txtUserID)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dtpEndDate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtpStartDate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboEndTime)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboStartTime)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblUserDescription
            // 
            this.lblUserDescription.BorderStyleInner = Infragistics.Win.UIElementBorderStyle.Solid;
            this.lblUserDescription.Location = new System.Drawing.Point(261, 34);
            this.lblUserDescription.Name = "lblUserDescription";
            this.lblUserDescription.Size = new System.Drawing.Size(303, 23);
            this.lblUserDescription.TabIndex = 8;
            // 
            // txtUserID
            // 
            this.txtUserID.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance1.BackColor = System.Drawing.Color.White;
            appearance1.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(231)))), ((int)(((byte)(230)))));
            appearance1.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance1.ImageBackground = ((System.Drawing.Image)(resources.GetObject("appearance1.ImageBackground")));
            editorButton1.Appearance = appearance1;
            editorButton1.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Office2003ToolbarButton;
            editorButton1.Text = "";
            this.txtUserID.ButtonsRight.Add(editorButton1);
            this.txtUserID.Location = new System.Drawing.Point(95, 34);
            this.txtUserID.MaxLength = 20;
            this.txtUserID.Name = "txtUserID";
            this.txtUserID.Size = new System.Drawing.Size(160, 23);
            this.txtUserID.TabIndex = 7;
            this.toolTip1.SetToolTip(this.txtUserID, "Press F4 To Search");
            this.txtUserID.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.txtUserID.Leave += new System.EventHandler(this.txtUserID_Leave);
            this.txtUserID.EditorButtonClick += new Infragistics.Win.UltraWinEditors.EditorButtonEventHandler(this.txtUserID_EditorButtonClick);
            // 
            // ultraLabel12
            // 
            appearance6.ForeColor = System.Drawing.Color.White;
            this.ultraLabel12.Appearance = appearance6;
            this.ultraLabel12.Location = new System.Drawing.Point(10, 36);
            this.ultraLabel12.Name = "ultraLabel12";
            this.ultraLabel12.Size = new System.Drawing.Size(74, 21);
            this.ultraLabel12.TabIndex = 6;
            this.ultraLabel12.Text = "Employee";
            // 
            // btnClose
            // 
            appearance14.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(62)))), ((int)(((byte)(176)))));
            appearance14.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance14.BorderAlpha = Infragistics.Win.Alpha.Opaque;
            appearance14.BorderColor = System.Drawing.Color.Black;
            appearance14.BorderColor3DBase = System.Drawing.Color.Black;
            appearance14.FontData.BoldAsString = "True";
            appearance14.ForeColor = System.Drawing.Color.White;
            appearance14.Image = ((object)(resources.GetObject("appearance14.Image")));
            this.btnClose.Appearance = appearance14;
            this.btnClose.ButtonStyle = Infragistics.Win.UIElementButtonStyle.OfficeXPToolbarButton;
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(443, 18);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(121, 30);
            this.btnClose.TabIndex = 18;
            this.btnClose.Text = "&Cancel";
            this.btnClose.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.btnClose.UseHotTracking = Infragistics.Win.DefaultableBoolean.True;
            this.btnClose.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnSave
            // 
            appearance2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(62)))), ((int)(((byte)(176)))));
            appearance2.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance2.FontData.BoldAsString = "True";
            appearance2.ForeColor = System.Drawing.Color.White;
            appearance2.Image = ((object)(resources.GetObject("appearance2.Image")));
            this.btnSave.Appearance = appearance2;
            this.btnSave.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Flat;
            this.btnSave.Location = new System.Drawing.Point(316, 18);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(121, 30);
            this.btnSave.TabIndex = 17;
            this.btnSave.Text = "&Ok";
            this.btnSave.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.btnSave.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // lblTransactionType
            // 
            appearance16.ForeColor = System.Drawing.Color.White;
            appearance16.ForeColorDisabled = System.Drawing.Color.White;
            appearance16.TextHAlignAsString = "Center";
            this.lblTransactionType.Appearance = appearance16;
            this.lblTransactionType.BackColorInternal = System.Drawing.Color.Transparent;
            this.lblTransactionType.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTransactionType.Location = new System.Drawing.Point(16, 6);
            this.lblTransactionType.Name = "lblTransactionType";
            this.lblTransactionType.Size = new System.Drawing.Size(576, 53);
            this.lblTransactionType.TabIndex = 23;
            this.lblTransactionType.Tag = "Header";
            this.lblTransactionType.Text = "Manage Employee Timesheet";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lblMessage);
            this.groupBox1.Controls.Add(this.txtUserID);
            this.groupBox1.Controls.Add(this.dtpEndDate);
            this.groupBox1.Controls.Add(this.dtpStartDate);
            this.groupBox1.Controls.Add(this.ultraLabel5);
            this.groupBox1.Controls.Add(this.ultraLabel2);
            this.groupBox1.Controls.Add(this.cboEndTime);
            this.groupBox1.Controls.Add(this.cboStartTime);
            this.groupBox1.Controls.Add(this.ultraLabel12);
            this.groupBox1.Controls.Add(this.lblUserDescription);
            this.groupBox1.Controls.Add(this.ultraLabel4);
            this.groupBox1.Controls.Add(this.ultraLabel1);
            this.groupBox1.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.groupBox1.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(14, 62);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(578, 192);
            this.groupBox1.TabIndex = 24;
            this.groupBox1.TabStop = false;
            // 
            // dtpEndDate
            // 
            appearance28.FontData.BoldAsString = "False";
            this.dtpEndDate.Appearance = appearance28;
            this.dtpEndDate.BackColor = System.Drawing.SystemColors.Window;
            this.dtpEndDate.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Office2003ToolbarButton;
            this.dtpEndDate.DateButtons.Add(dateButton1);
            this.dtpEndDate.Location = new System.Drawing.Point(95, 111);
            this.dtpEndDate.Name = "dtpEndDate";
            this.dtpEndDate.NonAutoSizeHeight = 10;
            this.dtpEndDate.NullDateLabel = "";
            this.dtpEndDate.Size = new System.Drawing.Size(160, 21);
            this.dtpEndDate.TabIndex = 10;
            this.dtpEndDate.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.dtpEndDate.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.dtpEndDate.Value = "";
            // 
            // dtpStartDate
            // 
            appearance10.FontData.BoldAsString = "False";
            this.dtpStartDate.Appearance = appearance10;
            this.dtpStartDate.BackColor = System.Drawing.SystemColors.Window;
            this.dtpStartDate.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Office2003ToolbarButton;
            this.dtpStartDate.DateButtons.Add(dateButton2);
            this.dtpStartDate.Location = new System.Drawing.Point(95, 73);
            this.dtpStartDate.Name = "dtpStartDate";
            this.dtpStartDate.NonAutoSizeHeight = 10;
            this.dtpStartDate.NullDateLabel = "";
            this.dtpStartDate.Size = new System.Drawing.Size(162, 21);
            this.dtpStartDate.TabIndex = 10;
            this.dtpStartDate.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.dtpStartDate.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.dtpStartDate.Value = "";
            // 
            // ultraLabel5
            // 
            appearance9.FontData.BoldAsString = "False";
            appearance9.ForeColor = System.Drawing.Color.White;
            this.ultraLabel5.Appearance = appearance9;
            this.ultraLabel5.Location = new System.Drawing.Point(335, 113);
            this.ultraLabel5.Name = "ultraLabel5";
            this.ultraLabel5.Size = new System.Drawing.Size(88, 21);
            this.ultraLabel5.TabIndex = 12;
            this.ultraLabel5.Text = "End Time";
            // 
            // ultraLabel2
            // 
            appearance7.FontData.BoldAsString = "False";
            appearance7.ForeColor = System.Drawing.Color.White;
            this.ultraLabel2.Appearance = appearance7;
            this.ultraLabel2.Location = new System.Drawing.Point(335, 73);
            this.ultraLabel2.Name = "ultraLabel2";
            this.ultraLabel2.Size = new System.Drawing.Size(88, 21);
            this.ultraLabel2.TabIndex = 12;
            this.ultraLabel2.Text = "Start Time";
            // 
            // cboEndTime
            // 
            appearance35.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(108)))), ((int)(((byte)(172)))));
            appearance35.ForeColor = System.Drawing.Color.White;
            this.cboEndTime.Appearance = appearance35;
            this.cboEndTime.AutoSize = false;
            this.cboEndTime.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(108)))), ((int)(((byte)(172)))));
            this.cboEndTime.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance36.BackColor = System.Drawing.Color.WhiteSmoke;
            appearance36.BackColor2 = System.Drawing.Color.Silver;
            appearance36.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            this.cboEndTime.ButtonAppearance = appearance36;
            this.cboEndTime.ButtonStyle = Infragistics.Win.UIElementButtonStyle.FlatBorderless;
            this.cboEndTime.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboEndTime.Location = new System.Drawing.Point(429, 112);
            this.cboEndTime.Name = "cboEndTime";
            this.cboEndTime.Size = new System.Drawing.Size(135, 23);
            this.cboEndTime.TabIndex = 11;
            this.cboEndTime.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            // 
            // cboStartTime
            // 
            appearance8.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(108)))), ((int)(((byte)(172)))));
            appearance8.ForeColor = System.Drawing.Color.White;
            this.cboStartTime.Appearance = appearance8;
            this.cboStartTime.AutoSize = false;
            this.cboStartTime.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(108)))), ((int)(((byte)(172)))));
            this.cboStartTime.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance17.BackColor = System.Drawing.Color.WhiteSmoke;
            appearance17.BackColor2 = System.Drawing.Color.Silver;
            appearance17.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            this.cboStartTime.ButtonAppearance = appearance17;
            this.cboStartTime.ButtonStyle = Infragistics.Win.UIElementButtonStyle.FlatBorderless;
            this.cboStartTime.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboStartTime.Location = new System.Drawing.Point(429, 72);
            this.cboStartTime.Name = "cboStartTime";
            this.cboStartTime.Size = new System.Drawing.Size(135, 23);
            this.cboStartTime.TabIndex = 11;
            this.cboStartTime.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            // 
            // ultraLabel4
            // 
            appearance18.FontData.BoldAsString = "False";
            appearance18.ForeColor = System.Drawing.Color.White;
            this.ultraLabel4.Appearance = appearance18;
            this.ultraLabel4.Location = new System.Drawing.Point(10, 112);
            this.ultraLabel4.Name = "ultraLabel4";
            this.ultraLabel4.Size = new System.Drawing.Size(79, 21);
            this.ultraLabel4.TabIndex = 9;
            this.ultraLabel4.Text = "End Date";
            // 
            // ultraLabel1
            // 
            appearance3.FontData.BoldAsString = "False";
            appearance3.ForeColor = System.Drawing.Color.White;
            this.ultraLabel1.Appearance = appearance3;
            this.ultraLabel1.Location = new System.Drawing.Point(10, 74);
            this.ultraLabel1.Name = "ultraLabel1";
            this.ultraLabel1.Size = new System.Drawing.Size(79, 21);
            this.ultraLabel1.TabIndex = 9;
            this.ultraLabel1.Text = "Start Date";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnSearch);
            this.groupBox2.Controls.Add(this.btnClear);
            this.groupBox2.Controls.Add(this.btnClose);
            this.groupBox2.Controls.Add(this.btnSave);
            this.groupBox2.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.groupBox2.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.Location = new System.Drawing.Point(14, 266);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(578, 59);
            this.groupBox2.TabIndex = 25;
            this.groupBox2.TabStop = false;
            // 
            // btnSearch
            // 
            this.btnSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            appearance4.BackColor = System.Drawing.Color.White;
            appearance4.BackColor2 = System.Drawing.SystemColors.Control;
            appearance4.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance4.FontData.BoldAsString = "True";
            appearance4.ForeColor = System.Drawing.Color.Black;
            appearance4.Image = ((object)(resources.GetObject("appearance4.Image")));
            this.btnSearch.Appearance = appearance4;
            this.btnSearch.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Flat;
            appearance5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(188)))), ((int)(((byte)(193)))), ((int)(((byte)(207)))));
            appearance5.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(188)))), ((int)(((byte)(193)))), ((int)(((byte)(207)))));
            this.btnSearch.HotTrackAppearance = appearance5;
            this.btnSearch.Location = new System.Drawing.Point(140, 18);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(121, 30);
            this.btnSearch.TabIndex = 20;
            this.btnSearch.Text = "&Search (F6)";
            this.btnSearch.UseHotTracking = Infragistics.Win.DefaultableBoolean.True;
            this.btnSearch.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // btnClear
            // 
            appearance15.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(62)))), ((int)(((byte)(176)))));
            appearance15.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance15.FontData.BoldAsString = "True";
            appearance15.ForeColor = System.Drawing.Color.White;
            this.btnClear.Appearance = appearance15;
            this.btnClear.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Flat;
            this.btnClear.Location = new System.Drawing.Point(10, 18);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(121, 30);
            this.btnClear.TabIndex = 19;
            this.btnClear.Text = "C&lear";
            this.btnClear.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.btnClear.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // lblMessage
            // 
            this.lblMessage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            appearance27.BackColorAlpha = Infragistics.Win.Alpha.Transparent;
            appearance27.FontData.BoldAsString = "False";
            appearance27.ForeColor = System.Drawing.Color.Red;
            this.lblMessage.Appearance = appearance27;
            this.lblMessage.AutoSize = true;
            this.lblMessage.Location = new System.Drawing.Point(13, 148);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.Size = new System.Drawing.Size(221, 18);
            this.lblMessage.TabIndex = 13;
            this.lblMessage.Text = "User Clocked In / Out message";
            // 
            // frmTimesheet
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(108)))), ((int)(((byte)(172)))));
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(608, 340);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.lblTransactionType);
            this.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.Name = "frmTimesheet";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Load += new System.EventHandler(this.frmTimesheet_Load);
            this.Activated += new System.EventHandler(this.frmTimesheet_Activated);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.frmTimesheet_KeyUp);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmTimesheet_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.txtUserID)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dtpEndDate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtpStartDate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboEndTime)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboStartTime)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);

		}
		#endregion
		
        private bool Save()
		{
            try
            {

                //if (oTimesheetRow.SalePrice > 0 && oTimesheetRow.SaleDiscount > 0)
                //{
                //    throw (new Exception("You can select either Sale Item Price or Sale Discount %."));
                //}
                //else
                //{
                //oTimesheetRow.DeptCode = txtDeptCode.Text;

                oBRTimesheet.Persist(oTimesheetData);
                return true;
                ///}
            }
            catch (POSExceptions exp)
            {
                clsUIHelper.ShowErrorMsg(exp.ErrMessage);
                //switch (exp.ErrNumber)
                //{
                //    case (long)POSErrorENUM.Timesheet_DuplicateCode:
                //        txtDeptCode.Focus();
                //        break;
                //    case (long)POSErrorENUM.Timesheet_NameCanNotBeNULL:
                //        txtDeptName.Focus();
                //        break;
                //    case (long)POSErrorENUM.Timesheet_CodeCanNotBeNULL:
                //        txtDeptCode.Focus();
                //        break;
                //    case (long)POSErrorENUM.Timesheet_SalePriceCanNotBeNULL:
                //        txtDeptName.Focus();
                //        break;
                //}
                return false;
            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
                return false;
            }
		}

		private void btnSave_Click(object sender, System.EventArgs e)
		{
            try
            {
                if (ValidateRow())
                {
                    if (Save())
                    {
                        SetNew();
                    }
                }
            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
            }

		}

        private bool ValidateRow()
        {
            string strMessage="";
            bool isValid = true;
            if (this.txtUserID.Text.Trim().Length == 0)
            {
                strMessage = "Please select a User.";
                isValid = false;
            }
            else if (ValidateTime(dtpStartDate.Text) == false)
            {
                strMessage = "Start date is invalid.";
                isValid = false;
            }
            else if (oTimesheetRow.IsTimeIn == true && ValidateTime(dtpEndDate.Text) == false)
            {
                strMessage = "End date is invalid.";
                isValid = false;
            }
            else if (ValidateTime(cboStartTime.Text)==false)
            {
                strMessage = "Start time is invalid.";
                isValid = false;
            }
            else if (oTimesheetRow.IsTimeIn==true && ValidateTime(cboEndTime.Text) == false)
            {
                strMessage = "End time is invalid.";
                isValid = false;
            }
            //Added by shitaljit on 7 mar 2012 to make user to enter out time in any case.
            else if (ValidateTime(cboEndTime.Text) == false)
            {
                strMessage = "End time is invalid.";
                isValid = false;
            }
            /*else if (oTimesheetRow.IsTimeIn==true && DateTime.Parse(cboEndTime.Text)<DateTime.Parse(cboStartTime.Text))
            {
                strMessage = "End time cannot be below start time.";
                isValid = false;
            }*/

            if (isValid == false)
            {
                clsUIHelper.ShowErrorMsg(strMessage);
            }
            else
            {
                oTimesheetRow.UserID = txtUserID.Text;
                string str = DateTime.Parse(dtpStartDate.Value.ToString()).Date.ToShortDateString() + " " + cboStartTime.Text;
                oTimesheetRow.TimeIn =Convert.ToDateTime( DateTime.Parse(dtpStartDate.Value.ToString()).Date.ToShortDateString() + " " + cboStartTime.Text);
                if (oTimesheetRow.IsTimeIn == true || dtpEndDate.Text.Trim() != "")
                {
                    oTimesheetRow.TimeOut = (System.Data.SqlTypes.SqlDateTime)Convert.ToDateTime(DateTime.Parse(dtpEndDate.Value.ToString()).Date.ToShortDateString() + " " + cboEndTime.Text);
                }


                oTimesheetRow.IsManualTimeIn = this.dtpStartDate.Enabled;
                oTimesheetRow.IsManualTimeOut = this.dtpEndDate.Enabled;

                if (oTimesheetRow.TimeIn.Value > oTimesheetRow.TimeOut)
                {
                    clsUIHelper.ShowErrorMsg("End time cannot be less than start time.");
                    isValid = false;
                }
            }
            return isValid;
        }

        private bool ValidateTime(string strTime)
        {
            try
            {
                DateTime dt= DateTime.Parse(strTime);
                return true;
            }
            catch
            {
                return false;
            }
        }

		private void Search()
		{
			try
			{
                //frmSearch oSearch = new frmSearch(clsPOSDBConstants.Users_tbl);
                frmSearchMain oSearch = new frmSearchMain(true);    //20-Dec-2017 JY Added new reference
                oSearch.SearchTable = clsPOSDBConstants.Users_tbl;  //20-Dec-2017 JY Added 
                oSearch.ShowDialog(this);
				if (!oSearch.IsCanceled)
				{
					string strCode=oSearch.SelectedRowID();
					if (strCode == "") 
						return;

                    this.txtUserID.Text = strCode;
                    DisplayUserName(strCode);
					//Edit(strCode);
				}
			}
			catch(Exception exp)
			{
				clsUIHelper.ShowErrorMsg(exp.Message);
			}
		}

		private void SearchUserID()
		{
			try
			{
                //frmSearch oSearch = new frmSearch(clsPOSDBConstants.Users_tbl);
                frmSearchMain oSearch = new frmSearchMain(true);    //20-Dec-2017 JY Added new reference
                oSearch.SearchTable = clsPOSDBConstants.Users_tbl;  //20-Dec-2017 JY Added 
                oSearch.ShowDialog(this);
				if (!oSearch.IsCanceled)
				{
					string strCode=oSearch.SelectedRowID();
					if (strCode == "") 
						return;
					
					DisplayUserName(strCode);
				}
			}
			catch(Exception exp)
			{
				clsUIHelper.ShowErrorMsg(exp.Message);
			}
		}

		private void Display()
		{
			//txtUserID.Text = oTimesheetRow.UserID;//Commented by Krishna on 14 October 2011
			dtpStartDate.Value= oTimesheetRow.TimeIn;
            if (oTimesheetRow.TimeOut.IsNull)
            {
                dtpEndDate.Value = null;
            }
            else
            {
                dtpEndDate.Value = oTimesheetRow.TimeOut.Value;
            }

            if (oTimesheetRow.TimeIn.HasValue)
            {
                cboStartTime.Text = oTimesheetRow.TimeIn.Value.ToString("HH:mm");
            }
            else
            {
                cboStartTime.Text = "";
            }

            if (oTimesheetRow.TimeOut.IsNull==false)
            {
                cboEndTime.Text = oTimesheetRow.TimeOut.Value.ToString("HH:mm");
            }
            else
            {
                cboEndTime.Text = "";
            }

		}

		private void DisplayUserName(string userID)
		{
			//try
			//{
                UserManagement.clsLogin login = new POS_Core_UI.UserManagement.clsLogin();
				lblUserDescription.Text= login.GetUsername(userID);
			//}
			//catch(Exception exp)
			//{
			//	clsUIHelper.ShowErrorMsg(exp.Message);
			//}
            }

            #region Commented Funtionality for Timesheet jira 99
            //public bool CheckExistingClockIn(string userID)
        //{
        //    try
        //    {
        //        //Following Code Added by Krishna on 14 October 2011
        //        System.Data.DataSet oSearchData = new System.Data.DataSet();
        //        if (dtpStartDate.Value.ToString() != "" && dtpEndDate.Value.ToString() != "")
        //        {
        //            oSearchData = oBRTimesheet.SearchDataFromEvents(this.txtUserID.Text, Convert.ToDateTime(this.dtpStartDate.Value), Convert.ToDateTime(this.dtpEndDate.Value));
        //            // oBRTimesheet.AddToTimesheet(oSearchData);
        //            if (oSearchData.Tables[0].Rows.Count > 0)
        //            {
        //                if (Resources.Message.Display("User " + userID + " has already Clocked in from Timesheet Exe.This will override the Clock In time. Do you wish to continue?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
        //                {
        //                    foreach (System.Data.DataRow row in oSearchData.Tables[0].Rows)
        //                    {
        //                        row["TimeIn"] = dtpStartDate.Value;
        //                        row["TimeOut"] = dtpEndDate.Value;
        //                    }
        //                    oBRTimesheet.AddToTimesheet(oSearchData);
        //                }
        //            }
        //            else if (dtpStartDate.Value.ToString() != "" && dtpEndDate.Value.ToString() == "")
        //            {
        //                dtpEndDate.Value = dtpStartDate.Value;
        //            }
        //            return true;
        //        }
        //        else
        //            return false;
        //        //Till here added by Krishna on 14 October 2011
        //    }
        //    catch (Exception ex)
        //    {
        //        return false;
        //    }
            //}
            #endregion


        public void DisplayLastTimeIn(string userID)
        {
            try
            {
                oTimesheetData = oBRTimesheet.GetLastTimeIn(userID);
                if (oTimesheetData.Timesheet.Rows.Count > 0)
                {
                    oTimesheetRow = oTimesheetData.Timesheet[0];
                    //oTimesheetRow.TimeOut = (System.Data.SqlTypes.SqlDateTime)DateTime.Now;//Commented by Krishna on 18 October 2011
                    //this.cboStartTime.Enabled = false;
                    //this.dtpStartDate.Enabled = false;

                    //Following Added by Krishna on 14 October 2011
                    lblMessage.Text = "User \"" + userID + "\" has not Clock - Out.\nPlease select end date and time to Clock - Out.";
                }
                else
                {
                    oTimesheetData.Timesheet.AddRow(0, this.txtUserID.Text, DateTime.Now, null, "", false, false, DateTime.Now);
                    oTimesheetRow = oTimesheetData.Timesheet[0];
                    //this.dtpEndDate.Enabled = false;
                    //this.cboEndTime.Enabled = false;
                    //Following Added by Krishna on 14 October 2011
                    lblMessage.Text = "Please select date and time for User \"" + userID + "\" to Clock - In / Out"; ;
                }
                Display();
                 this.txtUserID.Enabled = false;
                this.txtUserID.ButtonsRight[0].Enabled = false;
            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }

        public void Edit(string userID)
		{
            try
            {
                oTimesheetData = oBRTimesheet.PopulateByUserID(userID);
                if (oTimesheetData.Timesheet.Rows.Count > 0)
                {
                    oTimesheetRow = oTimesheetData.Timesheet[0];
                    if (oTimesheetRow != null)
                        Display();
                }
                else
                {
                    oTimesheetData.Timesheet.AddRow(0, "", DateTime.Now, DateTime.Now, "", false, false, DateTime.Now);
                }
            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
		}

		private void SetNew()
		{
			oBRTimesheet = new Timesheet();
			oTimesheetData = new TimesheetData();

			Clear();
			//oTimesheetRow = oTimesheetData.Timesheet.AddRow(0,Resources.Configuration.UserName,DateTime.Now,DateTime.Now,Resources.Configuration.UserName,false,false,DateTime.Now);
		}

		private void Clear()
		{
			txtUserID.Text = "";
            lblUserDescription.Text = "";
			
            dtpStartDate.Value = null;
            dtpEndDate.Value = null;

            this.cboStartTime.Text = "";
            this.cboEndTime.Text = "";

            this.txtUserID.Enabled = true;
            this.txtUserID.ButtonsRight[0].Enabled = true;

            if (UserPriviliges.IsUserHasPriviliges(UserPriviliges.Modules.Timesheet.ID, UserPriviliges.Screens.Timesheet.ID, UserPriviliges.Permissions.AllowManualTimeInOut.ID))
            {
                this.dtpEndDate.Enabled = true;
            this.dtpStartDate.Enabled = true;

            this.cboStartTime.Enabled = true;
            this.cboEndTime.Enabled = true;
            }
            else
            {
                this.dtpEndDate.Enabled = false;
                this.dtpStartDate.Enabled = false;

                this.cboStartTime.Enabled = false;
                this.cboEndTime.Enabled = false;
            }

            if (UserPriviliges.IsUserHasPriviliges(UserPriviliges.Modules.Timesheet.ID, UserPriviliges.Screens.Timesheet.ID, UserPriviliges.Permissions.AllowForLoggedInUserOnly.ID))
            {
                this.txtUserID.Text = POS_Core.Resources.Configuration.UserName;
                DisplayUserName(this.txtUserID.Text);
                DisplayLastTimeIn(this.txtUserID.Text);
                btnClear.Visible = false;
            }
            else
            {
                btnClear.Visible = true;
                this.txtUserID.Text = "";
                this.txtUserID.Focus();
            }
		}

        private string GetCurrentTime()
        {
            return DateTime.Now.ToString("HH:nn");
        }

		private void btnNew_Click(object sender, System.EventArgs e)
		{
			try
			{
				SetNew();
			}
			catch(Exception exp)
			{
				clsUIHelper.ShowErrorMsg(exp.Message);
			}
		}

		private void txtUserID_EditorButtonClick(object sender, Infragistics.Win.UltraWinEditors.EditorButtonEventArgs e)
		{
			Search();
		}

		private void txtUserID_Leave(object sender, System.EventArgs e)
		{
			try
			{
                if (txtUserID.Text.Length > 0)
                {
                    SearchUser();
                    DisplayLastTimeIn(txtUserID.Text);
                }
			}
			catch(Exception exp)
			{
				clsUIHelper.ShowErrorMsg(exp.Message);
                txtUserID.Text = "";
                txtUserID.Focus();
			}
		}

		private void SearchUser()
		{
			if (txtUserID.Text!=null || this.txtUserID.Text.Trim()!="")
				DisplayUserName(txtUserID.Text);
		}

		private void btnClose_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}

        private void frmTimesheet_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			try
			{
				if (e.KeyData==System.Windows.Forms.Keys.F4)
				{
					if ( txtUserID.ContainsFocus)
						this.SearchUserID();
				}
                else if (e.KeyData == System.Windows.Forms.Keys.F6)
                {
                    btnSearch_Click(btnSearch, new EventArgs());
                }
			}
			catch(Exception exp)
			{
				clsUIHelper.ShowErrorMsg(exp.Message);
			}
		}

		private void frmTimesheet_Load(object sender, System.EventArgs e)
		{

			this.txtUserID.AfterEnterEditMode += new System.EventHandler(clsUIHelper.AfterEnterEditMode);			
			this.txtUserID.AfterExitEditMode += new System.EventHandler(clsUIHelper.AfterExitEditMode);			
			this.dtpStartDate.Enter += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
			this.dtpStartDate.Leave += new System.EventHandler(clsUIHelper.AfterExitEditMode);
			this.dtpEndDate.Enter += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
			this.dtpEndDate.Leave += new System.EventHandler(clsUIHelper.AfterExitEditMode);

			this.cboStartTime.AfterEnterEditMode += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
			this.cboStartTime.AfterExitEditMode += new System.EventHandler(clsUIHelper.AfterExitEditMode);

            this.cboEndTime.AfterEnterEditMode += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
            this.cboEndTime.AfterExitEditMode += new System.EventHandler(clsUIHelper.AfterExitEditMode);

            PopulateTimeCombos();

            this.cboStartTime.SelectedIndex = 0;
            this.cboEndTime.SelectedIndex = 0;

			clsUIHelper.setColorSchecme(this);

            //Following Code Added by krishna on 14 October 2011
            this.lblMessage.Appearance.ForeColor = Color.Red;
            this.lblMessage.Appearance.FontData.Bold = Infragistics.Win.DefaultableBoolean.True;
            this.lblMessage.Appearance.BackColorAlpha = Infragistics.Win.Alpha.Transparent;
            //Till here Added by Krishna on 14 October 2011

            SetNew();

            if (UserPriviliges.IsUserHasPriviliges(UserPriviliges.Modules.Timesheet.ID, UserPriviliges.Screens.Timesheet.ID, UserPriviliges.Permissions.AllowForLoggedInUserOnly.ID) )
            {
                btnSearch.Visible = false;
                this.btnSearch.Enabled = false;
            }
		}

        private void PopulateTimeCombos()
        {
            for(int i=0;i<24;i++)
            {
                for (int j = 0; j < 61; j+= 15)
                {
                    if (j == 60)
                    {
                        j--;
                    }
                    string strCboTime = i.ToString().PadLeft(2, '0') + ":" + j.ToString().PadLeft(2, '0');
                    this.cboStartTime.Items.Add(strCboTime);
                    this.cboEndTime.Items.Add(strCboTime);
                }
            }
        }

        private void frmTimesheet_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
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

		private void frmTimesheet_Activated(object sender, System.EventArgs e)
		{
			clsUIHelper.CurrentForm=this;
		}

        private void btnClear_Click(object sender, EventArgs e)
        {
            try
            {
                SetNew();
            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                if (UserPriviliges.IsUserHasPriviliges(UserPriviliges.Modules.Timesheet.ID, UserPriviliges.Screens.Timesheet.ID, UserPriviliges.Permissions.AllowForLoggedInUserOnly.ID))
                {
                    return;
                }

                frmTimesheetSearch ofrm = new frmTimesheetSearch();
                if (ofrm.ShowDialog(this) == DialogResult.OK)
                {
                    Int64 TimeInOutID = 0;
                    TimeInOutID = ofrm.SelectedID;
                    oTimesheetData = oBRTimesheet.Populate(TimeInOutID);
                    if (oTimesheetData.Timesheet.Rows.Count > 0)
                    {
                        oTimesheetRow = oTimesheetData.Timesheet[0];
                        Display();
                        DisplayUserName(txtUserID.Text);
                    }
                }
            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }

	}
}
