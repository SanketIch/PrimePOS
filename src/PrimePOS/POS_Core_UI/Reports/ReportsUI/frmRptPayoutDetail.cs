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
namespace POS_Core_UI.Reports.ReportsUI
{
	/// <summary>
	/// Summary description for frmViewTransaction.
	/// </summary>
	public class frmRptPayoutDetails : System.Windows.Forms.Form
	{
		private System.Windows.Forms.GroupBox groupBox1;
		private Infragistics.Win.Misc.UltraLabel ultraLabel12;
		private Infragistics.Win.Misc.UltraLabel lblTransactionType;
		private Infragistics.Win.Misc.UltraLabel ultraLabel21;
		private Infragistics.Win.UltraWinEditors.UltraTextEditor txtUserID;
		private Infragistics.Win.UltraWinEditors.UltraTextEditor txtStationID;
		private Infragistics.Win.Misc.UltraLabel ultraLabel20;
		private Infragistics.Win.Misc.UltraLabel ultraLabel19;
		private Infragistics.Win.UltraWinSchedule.UltraCalendarCombo dtpSaleEndDate;
		private Infragistics.Win.UltraWinSchedule.UltraCalendarCombo dtpSaleStartDate;
		private System.Windows.Forms.GroupBox groupBox2;
		private Infragistics.Win.Misc.UltraButton btnPrint;
		private Infragistics.Win.Misc.UltraButton btnView;
		private Infragistics.Win.Misc.UltraButton btnClose;
		private Infragistics.Win.Misc.UltraLabel ultraLabel1;
		private Infragistics.Win.Misc.UltraLabel ultraLabel2;
		private Infragistics.Win.Misc.UltraLabel ultraLabel3;
		private Infragistics.Win.UltraWinEditors.UltraTextEditor txtDrawID;
		private Infragistics.Win.UltraWinEditors.UltraTextEditor txtClStationID;
		private Infragistics.Win.UltraWinEditors.UltraTextEditor txtEODID;
        private Infragistics.Win.Misc.UltraLabel ultraLabel4;
        private UltraComboEditor CmbCategory;
        private PayOutCatSvr oPayoutCatSvr;
        
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public frmRptPayoutDetails()
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
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance11 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance10 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance9 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance12 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinSchedule.CalendarCombo.DateButton dateButton1 = new Infragistics.Win.UltraWinSchedule.CalendarCombo.DateButton();
            Infragistics.Win.UltraWinSchedule.CalendarCombo.DateButton dateButton2 = new Infragistics.Win.UltraWinSchedule.CalendarCombo.DateButton();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmRptPayoutDetails));
            Infragistics.Win.Appearance appearance7 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance8 = new Infragistics.Win.Appearance();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.CmbCategory = new Infragistics.Win.UltraWinEditors.UltraComboEditor();
            this.ultraLabel4 = new Infragistics.Win.Misc.UltraLabel();
            this.txtEODID = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.ultraLabel3 = new Infragistics.Win.Misc.UltraLabel();
            this.txtClStationID = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.ultraLabel2 = new Infragistics.Win.Misc.UltraLabel();
            this.txtDrawID = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.ultraLabel1 = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabel20 = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabel19 = new Infragistics.Win.Misc.UltraLabel();
            this.dtpSaleEndDate = new Infragistics.Win.UltraWinSchedule.UltraCalendarCombo();
            this.dtpSaleStartDate = new Infragistics.Win.UltraWinSchedule.UltraCalendarCombo();
            this.txtStationID = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.ultraLabel21 = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabel12 = new Infragistics.Win.Misc.UltraLabel();
            this.txtUserID = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.lblTransactionType = new Infragistics.Win.Misc.UltraLabel();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnPrint = new Infragistics.Win.Misc.UltraButton();
            this.btnClose = new Infragistics.Win.Misc.UltraButton();
            this.btnView = new Infragistics.Win.Misc.UltraButton();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.CmbCategory)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtEODID)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtClStationID)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDrawID)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtpSaleEndDate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtpSaleStartDate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtStationID)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtUserID)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.CmbCategory);
            this.groupBox1.Controls.Add(this.ultraLabel4);
            this.groupBox1.Controls.Add(this.txtEODID);
            this.groupBox1.Controls.Add(this.ultraLabel3);
            this.groupBox1.Controls.Add(this.txtClStationID);
            this.groupBox1.Controls.Add(this.ultraLabel2);
            this.groupBox1.Controls.Add(this.txtDrawID);
            this.groupBox1.Controls.Add(this.ultraLabel1);
            this.groupBox1.Controls.Add(this.ultraLabel20);
            this.groupBox1.Controls.Add(this.ultraLabel19);
            this.groupBox1.Controls.Add(this.dtpSaleEndDate);
            this.groupBox1.Controls.Add(this.dtpSaleStartDate);
            this.groupBox1.Controls.Add(this.txtStationID);
            this.groupBox1.Controls.Add(this.ultraLabel21);
            this.groupBox1.Controls.Add(this.ultraLabel12);
            this.groupBox1.Controls.Add(this.txtUserID);
            this.groupBox1.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.groupBox1.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(16, 60);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(424, 296);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            // 
            // CmbCategory
            // 
            this.CmbCategory.Location = new System.Drawing.Point(278, 93);
            this.CmbCategory.Name = "CmbCategory";
            this.CmbCategory.Size = new System.Drawing.Size(125, 25);
            this.CmbCategory.TabIndex = 2;
            // 
            // ultraLabel4
            // 
            appearance2.ForeColor = System.Drawing.Color.White;
            this.ultraLabel4.Appearance = appearance2;
            this.ultraLabel4.Location = new System.Drawing.Point(22, 100);
            this.ultraLabel4.Name = "ultraLabel4";
            this.ultraLabel4.Size = new System.Drawing.Size(250, 18);
            this.ultraLabel4.TabIndex = 29;
            this.ultraLabel4.Text = "Category <Blank = All Category>";
            // 
            // txtEODID
            // 
            this.txtEODID.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.txtEODID.Location = new System.Drawing.Point(280, 249);
            this.txtEODID.MaxLength = 3;
            this.txtEODID.Name = "txtEODID";
            this.txtEODID.Size = new System.Drawing.Size(123, 23);
            this.txtEODID.TabIndex = 7;
            this.txtEODID.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // ultraLabel3
            // 
            appearance11.ForeColor = System.Drawing.Color.White;
            this.ultraLabel3.Appearance = appearance11;
            this.ultraLabel3.AutoSize = true;
            this.ultraLabel3.Location = new System.Drawing.Point(18, 251);
            this.ultraLabel3.Name = "ultraLabel3";
            this.ultraLabel3.Size = new System.Drawing.Size(189, 18);
            this.ultraLabel3.TabIndex = 28;
            this.ultraLabel3.Text = "EOD # <Blank = All EOD>";
            // 
            // txtClStationID
            // 
            this.txtClStationID.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.txtClStationID.Location = new System.Drawing.Point(280, 217);
            this.txtClStationID.MaxLength = 3;
            this.txtClStationID.Name = "txtClStationID";
            this.txtClStationID.Size = new System.Drawing.Size(123, 23);
            this.txtClStationID.TabIndex = 6;
            this.txtClStationID.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // ultraLabel2
            // 
            appearance10.ForeColor = System.Drawing.Color.White;
            this.ultraLabel2.Appearance = appearance10;
            this.ultraLabel2.AutoSize = true;
            this.ultraLabel2.Location = new System.Drawing.Point(18, 219);
            this.ultraLabel2.Name = "ultraLabel2";
            this.ultraLabel2.Size = new System.Drawing.Size(270, 18);
            this.ultraLabel2.TabIndex = 26;
            this.ultraLabel2.Text = "Close Station # <Blank = All Station>";
            // 
            // txtDrawID
            // 
            this.txtDrawID.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.txtDrawID.Location = new System.Drawing.Point(280, 185);
            this.txtDrawID.MaxLength = 3;
            this.txtDrawID.Name = "txtDrawID";
            this.txtDrawID.Size = new System.Drawing.Size(123, 23);
            this.txtDrawID.TabIndex = 5;
            this.txtDrawID.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // ultraLabel1
            // 
            appearance9.ForeColor = System.Drawing.Color.White;
            this.ultraLabel1.Appearance = appearance9;
            this.ultraLabel1.AutoSize = true;
            this.ultraLabel1.Location = new System.Drawing.Point(18, 187);
            this.ultraLabel1.Name = "ultraLabel1";
            this.ultraLabel1.Size = new System.Drawing.Size(201, 18);
            this.ultraLabel1.TabIndex = 24;
            this.ultraLabel1.Text = "Draw # <Blank = All Draw>";
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
            appearance12.ForeColor = System.Drawing.Color.White;
            this.ultraLabel19.Appearance = appearance12;
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
            this.dtpSaleEndDate.Location = new System.Drawing.Point(280, 63);
            this.dtpSaleEndDate.Name = "dtpSaleEndDate";
            this.dtpSaleEndDate.NonAutoSizeHeight = 10;
            this.dtpSaleEndDate.Size = new System.Drawing.Size(123, 22);
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
            this.dtpSaleStartDate.Location = new System.Drawing.Point(279, 34);
            this.dtpSaleStartDate.Name = "dtpSaleStartDate";
            this.dtpSaleStartDate.NonAutoSizeHeight = 10;
            this.dtpSaleStartDate.Size = new System.Drawing.Size(123, 22);
            this.dtpSaleStartDate.TabIndex = 0;
            this.dtpSaleStartDate.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.dtpSaleStartDate.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.dtpSaleStartDate.Value = new System.DateTime(2004, 5, 25, 0, 0, 0, 0);
            // 
            // txtStationID
            // 
            this.txtStationID.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.txtStationID.Location = new System.Drawing.Point(280, 153);
            this.txtStationID.MaxLength = 3;
            this.txtStationID.Name = "txtStationID";
            this.txtStationID.Size = new System.Drawing.Size(123, 23);
            this.txtStationID.TabIndex = 4;
            this.txtStationID.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // ultraLabel21
            // 
            appearance3.ForeColor = System.Drawing.Color.White;
            this.ultraLabel21.Appearance = appearance3;
            this.ultraLabel21.AutoSize = true;
            this.ultraLabel21.Location = new System.Drawing.Point(19, 155);
            this.ultraLabel21.Name = "ultraLabel21";
            this.ultraLabel21.Size = new System.Drawing.Size(228, 18);
            this.ultraLabel21.TabIndex = 17;
            this.ultraLabel21.Text = "Station # <Blank = All Station>";
            // 
            // ultraLabel12
            // 
            appearance4.ForeColor = System.Drawing.Color.White;
            this.ultraLabel12.Appearance = appearance4;
            this.ultraLabel12.AutoSize = true;
            this.ultraLabel12.Location = new System.Drawing.Point(19, 125);
            this.ultraLabel12.Name = "ultraLabel12";
            this.ultraLabel12.Size = new System.Drawing.Size(195, 18);
            this.ultraLabel12.TabIndex = 6;
            this.ultraLabel12.Text = "User ID <Blank = Any User";
            // 
            // txtUserID
            // 
            this.txtUserID.AutoSize = false;
            this.txtUserID.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.txtUserID.Location = new System.Drawing.Point(280, 123);
            this.txtUserID.MaxLength = 20;
            this.txtUserID.Name = "txtUserID";
            this.txtUserID.Size = new System.Drawing.Size(123, 23);
            this.txtUserID.TabIndex = 3;
            this.txtUserID.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // lblTransactionType
            // 
            appearance5.ForeColor = System.Drawing.Color.White;
            appearance5.ForeColorDisabled = System.Drawing.Color.White;
            appearance5.TextHAlignAsString = "Center";
            this.lblTransactionType.Appearance = appearance5;
            this.lblTransactionType.BackColorInternal = System.Drawing.Color.Transparent;
            this.lblTransactionType.Font = new System.Drawing.Font("Arial", 18F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTransactionType.Location = new System.Drawing.Point(11, 16);
            this.lblTransactionType.Name = "lblTransactionType";
            this.lblTransactionType.Size = new System.Drawing.Size(435, 30);
            this.lblTransactionType.TabIndex = 26;
            this.lblTransactionType.Text = "Payout Detail Report";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnPrint);
            this.groupBox2.Controls.Add(this.btnClose);
            this.groupBox2.Controls.Add(this.btnView);
            this.groupBox2.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.groupBox2.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.Location = new System.Drawing.Point(17, 362);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(424, 57);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            // 
            // btnPrint
            // 
            appearance6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(62)))), ((int)(((byte)(176)))));
            appearance6.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance6.FontData.BoldAsString = "True";
            appearance6.ForeColor = System.Drawing.Color.White;
            appearance6.Image = ((object)(resources.GetObject("appearance6.Image")));
            this.btnPrint.Appearance = appearance6;
            this.btnPrint.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            this.btnPrint.Location = new System.Drawing.Point(117, 19);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(85, 26);
            this.btnPrint.TabIndex = 1;
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
            appearance7.Image = ((object)(resources.GetObject("appearance7.Image")));
            this.btnClose.Appearance = appearance7;
            this.btnClose.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(301, 20);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(85, 26);
            this.btnClose.TabIndex = 2;
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
            appearance8.Image = ((object)(resources.GetObject("appearance8.Image")));
            this.btnView.Appearance = appearance8;
            this.btnView.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            this.btnView.Location = new System.Drawing.Point(209, 20);
            this.btnView.Name = "btnView";
            this.btnView.Size = new System.Drawing.Size(85, 26);
            this.btnView.TabIndex = 0;
            this.btnView.Text = "&View";
            this.btnView.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnView.Click += new System.EventHandler(this.btnView_Click_1);
            // 
            // frmRptPayoutDetails
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(108)))), ((int)(((byte)(172)))));
            this.ClientSize = new System.Drawing.Size(459, 428);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.lblTransactionType);
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.Name = "frmRptPayoutDetails";
            this.Text = "View Payout Details";
            this.Load += new System.EventHandler(this.frmViewTransaction_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmViewTransaction_KeyDown);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.CmbCategory)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtEODID)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtClStationID)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDrawID)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtpSaleEndDate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtpSaleStartDate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtStationID)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtUserID)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);

		}
		#endregion

		private void frmViewTransaction_Load(object sender, System.EventArgs e)
		{
			this.txtEODID.AfterEnterEditMode += new System.EventHandler(clsUIHelper.AfterEnterEditMode);			
			this.txtEODID.AfterExitEditMode += new System.EventHandler(clsUIHelper.AfterExitEditMode);			

			this.txtClStationID.AfterEnterEditMode += new System.EventHandler(clsUIHelper.AfterEnterEditMode);			
			this.txtClStationID.AfterExitEditMode += new System.EventHandler(clsUIHelper.AfterExitEditMode);			

			this.txtDrawID.AfterEnterEditMode += new System.EventHandler(clsUIHelper.AfterEnterEditMode);			
			this.txtDrawID.AfterExitEditMode += new System.EventHandler(clsUIHelper.AfterExitEditMode);			

			this.txtStationID.AfterEnterEditMode += new System.EventHandler(clsUIHelper.AfterEnterEditMode);			
			this.txtStationID.AfterExitEditMode += new System.EventHandler(clsUIHelper.AfterExitEditMode);			
			
			this.dtpSaleStartDate.Enter += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
			this.dtpSaleStartDate.Leave += new System.EventHandler(clsUIHelper.AfterExitEditMode);

			this.dtpSaleEndDate.Enter += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
			this.dtpSaleEndDate.Leave += new System.EventHandler(clsUIHelper.AfterExitEditMode);

			this.txtUserID.AfterEnterEditMode += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
			this.txtUserID.AfterExitEditMode += new System.EventHandler(clsUIHelper.AfterExitEditMode);

			this.Left=(frmMain.getInstance().Width-frmMain.getInstance().ultraExplorerBar1.Width-this.Width)/2;
			this.Top=(frmMain.getInstance().Height-frmMain.getInstance().ultMenuBar.Toolbars[1].Height-this.Height)/2;
			clsUIHelper.setColorSchecme(this);
			this.dtpSaleEndDate.Value=DateTime.Now;
			this.dtpSaleStartDate.Value=DateTime.Now;
            //Shitaljit Start 29Feb2012
            PayOutCatData oPayOutCatData = new PayOutCatData();
            PayOutCatSvr oPayoutCatSvr = new PayOutCatSvr();
            string Str = "Select * from " + clsPOSDBConstants.PayOutCat_tbl + "";
            oPayOutCatData = oPayoutCatSvr.Populate(Str);
            this.CmbCategory.Items.Clear();

            foreach (PayOutCatRow oRow in oPayOutCatData.Tables[0].Rows)
            {
                this.CmbCategory.Items.Add(oRow.ID.ToString(), oRow.PayoutCatType.ToString());
            }
            //Shitaljit End 29Feb2012
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
				rptPayoutDetails oRpt = new  rptPayoutDetails();
                
				String strQuery;
				
                ////strQuery="select * from payout,util_POSSet  " +  ORG Shitaljit 27Feb2012
                        ////" where payout.stationid=util_POSSet.stationid and convert(datetime,TransDate,109) between convert(datetime, cast('" + this.dtpSaleStartDate.Text  + " 00:00:00' as datetime) ,113) and convert(datetime, cast('" + this.dtpSaleEndDate.Text  + " 23:59:59' as datetime) ,113) ";
                strQuery ="select p.*,pcat.PayoutCatType from payout p,util_POSSet,PayOutCategory pcat "+
                    " where P.stationid=util_POSSet.stationid and convert(datetime,TransDate,109) between convert(datetime, cast('" + this.dtpSaleStartDate.Text + " 00:00:00' as datetime) ,113)and convert(datetime, cast('" + this.dtpSaleEndDate.Text + " 23:59:59' as datetime) ,113)AND P.PayoutCatID=pcat.ID ";
				
                if (this.txtUserID.Text.Trim()!="")
					strQuery+=" and P.UserID='" + this.txtUserID.Text.Trim() + "' ";

                if (this.CmbCategory.Text.Trim()!= "")//Shitaljit
                    strQuery += " and pcat.PayoutCatType='" + this.CmbCategory.Text.Trim() + "' ";
			
				if (this.txtStationID.Text.Trim()!="")
					strQuery+=" and stationname='" + this.txtStationID.Text.Trim() + "' ";

				if (this.txtDrawID.Text.Trim()!="")
					strQuery+=" and DrawNo='" + this.txtDrawID.Text.Trim() + "' ";

				if (this.txtClStationID.Text.Trim()!="")
					strQuery+=" and cast(stationcloseID as varchar)='" + this.txtClStationID.Text.Trim() + "' ";

				if (this.txtEODID.Text.Trim()!="")
					strQuery+=" and cast(eodID as varchar)='" + this.txtEODID.Text.Trim() + "' ";
                //Shitaljit for dates 29Feb2012
                ((CrystalDecisions.CrystalReports.Engine.TextObject)oRpt.ReportDefinition.ReportObjects["dtpstartdate"]).Text = dtpSaleStartDate.Value.ToString();
                ((CrystalDecisions.CrystalReports.Engine.TextObject)oRpt.ReportDefinition.ReportObjects["dtpEnddate"]).Text = dtpSaleEndDate.Value.ToString();
                //Shitaljit 

				clsReports.Preview(blnPrint,strQuery,oRpt);
				/*Search oSearch=new Search();
				DataSet ds = oSearch.Search(strQuery);
				oRpt.SetDataSource(ds);
				frmReportViewer oViewer=new frmReportViewer();
				oViewer.rvReportViewer.ReportSource=oRpt;
				oViewer.MdiParent=frmMain.getInstance();
				oViewer.WindowState=FormWindowState.Maximized;
				oViewer.Show();*/
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

	}
}
