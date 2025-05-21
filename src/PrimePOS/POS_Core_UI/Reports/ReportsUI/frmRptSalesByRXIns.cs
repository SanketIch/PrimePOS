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
	public class frmRptSalesByRXIns : System.Windows.Forms.Form
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
        private RadioButton optSortByTransDate;
        private RadioButton optRXNumber;
        private Infragistics.Win.Misc.UltraLabel ultraLabel1;
        private GroupBox groupBox3;
        public UltraTextEditor txtUser;
        private Infragistics.Win.Misc.UltraLabel ultraLabel5;
        public UltraTextEditor txtInsuranceType;
        private Infragistics.Win.Misc.UltraLabel ultraLabel3;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public frmRptSalesByRXIns()
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
            Infragistics.Win.Appearance appearance15 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance9 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance14 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance18 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance19 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance20 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinEditors.EditorButton editorButton1 = new Infragistics.Win.UltraWinEditors.EditorButton();
            Infragistics.Win.Appearance appearance21 = new Infragistics.Win.Appearance();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmRptSalesByRXIns));
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinSchedule.CalendarCombo.DateButton dateButton1 = new Infragistics.Win.UltraWinSchedule.CalendarCombo.DateButton();
            Infragistics.Win.UltraWinSchedule.CalendarCombo.DateButton dateButton2 = new Infragistics.Win.UltraWinSchedule.CalendarCombo.DateButton();
            Infragistics.Win.Appearance appearance22 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance10 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance11 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance12 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance8 = new Infragistics.Win.Appearance();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtUser = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.ultraLabel5 = new Infragistics.Win.Misc.UltraLabel();
            this.txtInsuranceType = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.ultraLabel3 = new Infragistics.Win.Misc.UltraLabel();
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
            this.optSortByTransDate = new System.Windows.Forms.RadioButton();
            this.optRXNumber = new System.Windows.Forms.RadioButton();
            this.ultraLabel1 = new Infragistics.Win.Misc.UltraLabel();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtUser)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtInsuranceType)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCustomer)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtpSaleEndDate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtpSaleStartDate)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtUser);
            this.groupBox1.Controls.Add(this.ultraLabel5);
            this.groupBox1.Controls.Add(this.txtInsuranceType);
            this.groupBox1.Controls.Add(this.ultraLabel3);
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
            this.groupBox1.Size = new System.Drawing.Size(424, 200);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            // 
            // txtUser
            // 
            appearance15.BorderColor = System.Drawing.Color.Lime;
            appearance15.BorderColor3DBase = System.Drawing.Color.Lime;
            this.txtUser.Appearance = appearance15;
            this.txtUser.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.txtUser.Location = new System.Drawing.Point(135, 154);
            this.txtUser.Name = "txtUser";
            this.txtUser.Size = new System.Drawing.Size(176, 23);
            this.txtUser.TabIndex = 4;
            this.txtUser.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.txtUser.Visible = false;
            // 
            // ultraLabel5
            // 
            appearance9.ForeColor = System.Drawing.Color.White;
            this.ultraLabel5.Appearance = appearance9;
            this.ultraLabel5.AutoSize = true;
            this.ultraLabel5.Location = new System.Drawing.Point(18, 156);
            this.ultraLabel5.Name = "ultraLabel5";
            this.ultraLabel5.Size = new System.Drawing.Size(36, 18);
            this.ultraLabel5.TabIndex = 35;
            this.ultraLabel5.Text = "User";
            this.ultraLabel5.Visible = false;
            // 
            // txtInsuranceType
            // 
            appearance14.BorderColor = System.Drawing.Color.Lime;
            appearance14.BorderColor3DBase = System.Drawing.Color.Lime;
            this.txtInsuranceType.Appearance = appearance14;
            this.txtInsuranceType.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.txtInsuranceType.Location = new System.Drawing.Point(135, 123);
            this.txtInsuranceType.Name = "txtInsuranceType";
            this.txtInsuranceType.Size = new System.Drawing.Size(176, 23);
            this.txtInsuranceType.TabIndex = 3;
            this.txtInsuranceType.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // ultraLabel3
            // 
            appearance18.ForeColor = System.Drawing.Color.White;
            this.ultraLabel3.Appearance = appearance18;
            this.ultraLabel3.AutoSize = true;
            this.ultraLabel3.Location = new System.Drawing.Point(18, 125);
            this.ultraLabel3.Name = "ultraLabel3";
            this.ultraLabel3.Size = new System.Drawing.Size(112, 18);
            this.ultraLabel3.TabIndex = 32;
            this.ultraLabel3.Text = "Insurance Type";
            // 
            // lblCustomerName
            // 
            appearance19.ForeColor = System.Drawing.Color.Black;
            this.lblCustomerName.Appearance = appearance19;
            this.lblCustomerName.BackColorInternal = System.Drawing.Color.Transparent;
            this.lblCustomerName.Location = new System.Drawing.Point(183, 94);
            this.lblCustomerName.Name = "lblCustomerName";
            this.lblCustomerName.Size = new System.Drawing.Size(235, 18);
            this.lblCustomerName.TabIndex = 28;
            this.lblCustomerName.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // txtCustomer
            // 
            appearance20.BorderColor = System.Drawing.Color.Lime;
            appearance20.BorderColor3DBase = System.Drawing.Color.Lime;
            this.txtCustomer.Appearance = appearance20;
            this.txtCustomer.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance21.BackColor = System.Drawing.Color.White;
            appearance21.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(231)))), ((int)(((byte)(230)))));
            appearance21.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance21.BorderAlpha = Infragistics.Win.Alpha.Transparent;
            appearance21.Image = ((object)(resources.GetObject("appearance21.Image")));
            appearance21.ImageAlpha = Infragistics.Win.Alpha.Opaque;
            appearance21.ImageBackgroundAlpha = Infragistics.Win.Alpha.Opaque;
            appearance21.ImageBackgroundStyle = Infragistics.Win.ImageBackgroundStyle.Centered;
            editorButton1.Appearance = appearance21;
            editorButton1.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            editorButton1.Text = "";
            editorButton1.Width = 20;
            this.txtCustomer.ButtonsRight.Add(editorButton1);
            this.txtCustomer.Location = new System.Drawing.Point(136, 92);
            this.txtCustomer.Name = "txtCustomer";
            this.txtCustomer.Size = new System.Drawing.Size(42, 23);
            this.txtCustomer.TabIndex = 2;
            this.txtCustomer.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.txtCustomer.Leave += new System.EventHandler(this.txtCustomer_Leave);
            this.txtCustomer.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtCustomer_KeyUp);
            this.txtCustomer.EditorButtonClick += new Infragistics.Win.UltraWinEditors.EditorButtonEventHandler(this.txtCustomer_EditorButtonClick);
            // 
            // ultraLabel20
            // 
            appearance1.ForeColor = System.Drawing.Color.White;
            this.ultraLabel20.Appearance = appearance1;
            this.ultraLabel20.Location = new System.Drawing.Point(19, 38);
            this.ultraLabel20.Name = "ultraLabel20";
            this.ultraLabel20.Size = new System.Drawing.Size(116, 14);
            this.ultraLabel20.TabIndex = 20;
            this.ultraLabel20.Text = "Start Date";
            // 
            // ultraLabel19
            // 
            appearance2.ForeColor = System.Drawing.Color.White;
            this.ultraLabel19.Appearance = appearance2;
            this.ultraLabel19.Location = new System.Drawing.Point(19, 67);
            this.ultraLabel19.Name = "ultraLabel19";
            this.ultraLabel19.Size = new System.Drawing.Size(106, 14);
            this.ultraLabel19.TabIndex = 22;
            this.ultraLabel19.Text = "End Date";
            // 
            // dtpSaleEndDate
            // 
            this.dtpSaleEndDate.AllowNull = false;
            this.dtpSaleEndDate.BackColor = System.Drawing.SystemColors.Window;
            this.dtpSaleEndDate.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Office2003ToolbarButton;
            this.dtpSaleEndDate.DateButtons.Add(dateButton1);
            this.dtpSaleEndDate.Format = "";
            this.dtpSaleEndDate.Location = new System.Drawing.Point(136, 63);
            this.dtpSaleEndDate.Name = "dtpSaleEndDate";
            this.dtpSaleEndDate.NonAutoSizeHeight = 10;
            this.dtpSaleEndDate.Size = new System.Drawing.Size(176, 22);
            this.dtpSaleEndDate.TabIndex = 1;
            this.dtpSaleEndDate.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.dtpSaleEndDate.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.dtpSaleEndDate.Value = new System.DateTime(2004, 5, 25, 0, 0, 0, 0);
            // 
            // dtpSaleStartDate
            // 
            this.dtpSaleStartDate.AllowNull = false;
            this.dtpSaleStartDate.BackColor = System.Drawing.SystemColors.Window;
            this.dtpSaleStartDate.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Office2003ToolbarButton;
            this.dtpSaleStartDate.DateButtons.Add(dateButton2);
            this.dtpSaleStartDate.Format = "";
            this.dtpSaleStartDate.Location = new System.Drawing.Point(136, 34);
            this.dtpSaleStartDate.Name = "dtpSaleStartDate";
            this.dtpSaleStartDate.NonAutoSizeHeight = 10;
            this.dtpSaleStartDate.Size = new System.Drawing.Size(176, 22);
            this.dtpSaleStartDate.TabIndex = 0;
            this.dtpSaleStartDate.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.dtpSaleStartDate.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.dtpSaleStartDate.Value = new System.DateTime(2004, 5, 25, 0, 0, 0, 0);
            // 
            // ultraLabel12
            // 
            appearance22.ForeColor = System.Drawing.Color.White;
            this.ultraLabel12.Appearance = appearance22;
            this.ultraLabel12.AutoSize = true;
            this.ultraLabel12.Location = new System.Drawing.Point(19, 94);
            this.ultraLabel12.Name = "ultraLabel12";
            this.ultraLabel12.Size = new System.Drawing.Size(72, 18);
            this.ultraLabel12.TabIndex = 6;
            this.ultraLabel12.Text = "Customer";
            // 
            // lblTransactionType
            // 
            appearance4.ForeColor = System.Drawing.Color.White;
            appearance4.ForeColorDisabled = System.Drawing.Color.White;
            appearance4.TextHAlignAsString = "Center";
            this.lblTransactionType.Appearance = appearance4;
            this.lblTransactionType.BackColorInternal = System.Drawing.Color.Transparent;
            this.lblTransactionType.Font = new System.Drawing.Font("Arial", 18F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTransactionType.Location = new System.Drawing.Point(16, 16);
            this.lblTransactionType.Name = "lblTransactionType";
            this.lblTransactionType.Size = new System.Drawing.Size(424, 30);
            this.lblTransactionType.TabIndex = 26;
            this.lblTransactionType.Text = "Rx Checked Out By Insurance";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnPrint);
            this.groupBox2.Controls.Add(this.btnClose);
            this.groupBox2.Controls.Add(this.btnView);
            this.groupBox2.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.groupBox2.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.Location = new System.Drawing.Point(17, 327);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(424, 57);
            this.groupBox2.TabIndex = 12;
            this.groupBox2.TabStop = false;
            // 
            // btnPrint
            // 
            appearance10.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(62)))), ((int)(((byte)(176)))));
            appearance10.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance10.FontData.BoldAsString = "True";
            appearance10.ForeColor = System.Drawing.Color.White;
            appearance10.Image = ((object)(resources.GetObject("appearance10.Image")));
            this.btnPrint.Appearance = appearance10;
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
            appearance11.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(62)))), ((int)(((byte)(176)))));
            appearance11.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance11.FontData.BoldAsString = "True";
            appearance11.ForeColor = System.Drawing.Color.White;
            appearance11.Image = ((object)(resources.GetObject("appearance11.Image")));
            this.btnClose.Appearance = appearance11;
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
            appearance12.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(62)))), ((int)(((byte)(176)))));
            appearance12.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance12.FontData.BoldAsString = "True";
            appearance12.ForeColor = System.Drawing.Color.White;
            appearance12.Image = ((object)(resources.GetObject("appearance12.Image")));
            this.btnView.Appearance = appearance12;
            this.btnView.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            this.btnView.Location = new System.Drawing.Point(239, 20);
            this.btnView.Name = "btnView";
            this.btnView.Size = new System.Drawing.Size(85, 26);
            this.btnView.TabIndex = 12;
            this.btnView.Text = "&View";
            this.btnView.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnView.Click += new System.EventHandler(this.btnView_Click_1);
            // 
            // optSortByTransDate
            // 
            this.optSortByTransDate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.optSortByTransDate.ForeColor = System.Drawing.Color.White;
            this.optSortByTransDate.Location = new System.Drawing.Point(239, 18);
            this.optSortByTransDate.Name = "optSortByTransDate";
            this.optSortByTransDate.Size = new System.Drawing.Size(145, 26);
            this.optSortByTransDate.TabIndex = 10;
            this.optSortByTransDate.Text = "Trans. Date";
            // 
            // optRXNumber
            // 
            this.optRXNumber.Checked = true;
            this.optRXNumber.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.optRXNumber.ForeColor = System.Drawing.Color.White;
            this.optRXNumber.Location = new System.Drawing.Point(135, 18);
            this.optRXNumber.Name = "optRXNumber";
            this.optRXNumber.Size = new System.Drawing.Size(147, 26);
            this.optRXNumber.TabIndex = 9;
            this.optRXNumber.TabStop = true;
            this.optRXNumber.Text = "RX #";
            // 
            // ultraLabel1
            // 
            appearance8.ForeColor = System.Drawing.Color.White;
            this.ultraLabel1.Appearance = appearance8;
            this.ultraLabel1.AutoSize = true;
            this.ultraLabel1.Location = new System.Drawing.Point(18, 23);
            this.ultraLabel1.Name = "ultraLabel1";
            this.ultraLabel1.Size = new System.Drawing.Size(67, 18);
            this.ultraLabel1.TabIndex = 35;
            this.ultraLabel1.Text = "Sort By :";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.ultraLabel1);
            this.groupBox3.Controls.Add(this.optSortByTransDate);
            this.groupBox3.Controls.Add(this.optRXNumber);
            this.groupBox3.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.groupBox3.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox3.Location = new System.Drawing.Point(17, 266);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(424, 55);
            this.groupBox3.TabIndex = 9;
            this.groupBox3.TabStop = false;
            // 
            // frmRptSalesByRXIns
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(108)))), ((int)(((byte)(172)))));
            this.ClientSize = new System.Drawing.Size(459, 408);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.lblTransactionType);
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.Name = "frmRptSalesByRXIns";
            this.Text = "Insurance Detail";
            this.Load += new System.EventHandler(this.frmRptSalesByCustomer_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmRptSalesByCustomer_KeyDown);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtUser)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtInsuranceType)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCustomer)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtpSaleEndDate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtpSaleStartDate)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);

		}
		#endregion

		private void frmRptSalesByCustomer_Load(object sender, System.EventArgs e)
		{
			this.dtpSaleStartDate.Enter += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
			this.dtpSaleStartDate.Leave += new System.EventHandler(clsUIHelper.AfterExitEditMode);

			this.dtpSaleEndDate.Enter += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
			this.dtpSaleEndDate.Leave += new System.EventHandler(clsUIHelper.AfterExitEditMode);

			this.txtCustomer.AfterEnterEditMode += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
			this.txtCustomer.AfterExitEditMode += new System.EventHandler(clsUIHelper.AfterExitEditMode);

            this.txtInsuranceType.AfterEnterEditMode += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
            this.txtInsuranceType.AfterExitEditMode += new System.EventHandler(clsUIHelper.AfterExitEditMode);

            this.txtUser.AfterEnterEditMode += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
            this.txtUser.AfterExitEditMode += new System.EventHandler(clsUIHelper.AfterExitEditMode);

			this.Left=(frmMain.getInstance().Width-frmMain.getInstance().ultraExplorerBar1.Width-this.Width)/2;
			this.Top=(frmMain.getInstance().Height-this.Height)/2;
			
			clsUIHelper.setColorSchecme(this);
			this.dtpSaleEndDate.Value=DateTime.Now;
			this.dtpSaleStartDate.Value=DateTime.Now;
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

				this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
                rptSalesByRXInsurance oRpt = new  rptSalesByRXInsurance();

				String strQuery;

                DataSet oDataSet = new DataSet();
                strQuery = " select PT.TransID, PT.TransDate,PTD.*, PRD.* From "
                    + " POSTransactionrxDetail PRD Inner Join POSTransactionDetail PTD On (PRD.TransDetailID=PTD.TransDetailID) "
                    + " Inner Join POSTransaction PT On (PT.TransID=PTD.TransID)"
                    + " WHERE PTD.ItemID='RX' "
                    + " and TransDate between cast('" + this.dtpSaleStartDate.Text + "' as datetime) and cast('" + this.dtpSaleEndDate.Text + "' as datetime) ";
                    //+ " And PT.TransType=1 ";

                if (this.txtCustomer.Tag != null && this.txtCustomer.Tag.ToString() != "")
                {
                    strQuery += " and PT.CustomerID=" + POS_Core.Resources.Configuration.convertNullToInt(this.txtCustomer.Tag.ToString().Trim()).ToString() + " ";
                    clsReports.setCRTextObjectText("CustomerName", "Customer : " + lblCustomerName.Text, oRpt);
                }
                else
                {
                    clsReports.setCRTextObjectText("CustomerName", "Customer : ALL" , oRpt);
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

                if (txtInsuranceType.Text.Trim().Length == 0)
                {
                    clsReports.setCRTextObjectText("txtStation", "Ins. : ALL", oRpt);
                }
                else
                {
                    strQuery += " and RTrim(PRD.PatType)='" + txtInsuranceType.Text + "' ";
                    if (txtInsuranceType.Text != "C")
                    {
                        strQuery += " and RTrim(PRD.InsType)<>'C'";
                    }

                    clsReports.setCRTextObjectText("txtStation", "Ins. : " + txtInsuranceType.Text, oRpt);
                }

                if (optSortByTransDate.Checked == true)
                {
                    strQuery += " order by PRD.PatType, PT.TransDate,PT.TransID ";
                }
                else if (optRXNumber.Checked == true)
                {
                    strQuery += " order by PRD.PatType, PRD.RXNo,PT.TransID ";
                }

                clsReports.setCRTextObjectText("txtFromDate", "From :" + this.dtpSaleStartDate.Text, oRpt);
                clsReports.setCRTextObjectText("txtToDate", "To :" + dtpSaleEndDate.Text, oRpt);

                DataSet ds = clsReports.GetReportSource(strQuery);
                oRpt.Database.Tables[0].SetDataSource(ds.Tables[0]);
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
                frmSearchMain oSearch = new frmSearchMain(txtCustomer.Text, true, clsPOSDBConstants.Customer_tbl);    //18-Dec-2017 JY Added new reference
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
