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
using POS_Core.Resources;
using System.Timers;
//using POS_Core.DataAccess;


namespace POS_Core_UI.Reports.ReportsUI
{
    /// <summary>
    /// Summary description for frmViewTransaction.
    /// </summary>
    public class frmRptTimesheet : System.Windows.Forms.Form
    {
        private System.Windows.Forms.GroupBox groupBox1;
        private Infragistics.Win.Misc.UltraLabel ultraLabel12;
        private Infragistics.Win.Misc.UltraLabel lblTransactionType;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor txtUserID;
        private Infragistics.Win.Misc.UltraLabel ultraLabel20;
        private Infragistics.Win.Misc.UltraLabel ultraLabel19;
        private Infragistics.Win.UltraWinSchedule.UltraCalendarCombo dtpSaleEndDate;
        private Infragistics.Win.UltraWinSchedule.UltraCalendarCombo dtpSaleStartDate;
        private System.Windows.Forms.GroupBox groupBox2;
        private Infragistics.Win.Misc.UltraButton btnPrint;
        private Infragistics.Win.Misc.UltraButton btnView;
        private Infragistics.Win.Misc.UltraButton btnClose;
        private UltraComboEditor cboSortType;

        const string StrSortByStardDateToEndDate = "Date-Start To End";
        const string StrSortByEnddDateToStardDate = "Date-End To Start";
        const string StrSortByUserIDAsc = "UserID Ascending ";
        const string StrSortByUserIDDesc = "UserID Descending";
        const string StrSortByWorkingHrHTL = "Worked Hr-Max To Min";
        const string StrSortByWorkingHrLTH = "Worked Hr-Min To Max";
        private Infragistics.Win.Misc.UltraLabel ultraLabel1;
        private Infragistics.Win.Misc.UltraLabel lblMessage;

        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;

        #region PRIMEPOS-189 09-Aug-2021 JY Added
        int nDisplayHourlyRate = 0;
        System.Timers.Timer tmrBlinking;
        private long iBlinkCnt = 0;
        #endregion

        public frmRptTimesheet()
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
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinSchedule.CalendarCombo.DateButton dateButton1 = new Infragistics.Win.UltraWinSchedule.CalendarCombo.DateButton();
            Infragistics.Win.UltraWinSchedule.CalendarCombo.DateButton dateButton2 = new Infragistics.Win.UltraWinSchedule.CalendarCombo.DateButton();
            Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance7 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance8 = new Infragistics.Win.Appearance();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmRptTimesheet));
            Infragistics.Win.Appearance appearance9 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance10 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance11 = new Infragistics.Win.Appearance();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.ultraLabel1 = new Infragistics.Win.Misc.UltraLabel();
            this.cboSortType = new Infragistics.Win.UltraWinEditors.UltraComboEditor();
            this.ultraLabel20 = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabel19 = new Infragistics.Win.Misc.UltraLabel();
            this.dtpSaleEndDate = new Infragistics.Win.UltraWinSchedule.UltraCalendarCombo();
            this.dtpSaleStartDate = new Infragistics.Win.UltraWinSchedule.UltraCalendarCombo();
            this.ultraLabel12 = new Infragistics.Win.Misc.UltraLabel();
            this.txtUserID = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.lblTransactionType = new Infragistics.Win.Misc.UltraLabel();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnPrint = new Infragistics.Win.Misc.UltraButton();
            this.btnClose = new Infragistics.Win.Misc.UltraButton();
            this.btnView = new Infragistics.Win.Misc.UltraButton();
            this.lblMessage = new Infragistics.Win.Misc.UltraLabel();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cboSortType)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtpSaleEndDate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtpSaleStartDate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtUserID)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.ultraLabel1);
            this.groupBox1.Controls.Add(this.cboSortType);
            this.groupBox1.Controls.Add(this.ultraLabel20);
            this.groupBox1.Controls.Add(this.ultraLabel19);
            this.groupBox1.Controls.Add(this.dtpSaleEndDate);
            this.groupBox1.Controls.Add(this.dtpSaleStartDate);
            this.groupBox1.Controls.Add(this.ultraLabel12);
            this.groupBox1.Controls.Add(this.txtUserID);
            this.groupBox1.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.groupBox1.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(10, 30);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(437, 165);
            this.groupBox1.TabIndex = 27;
            this.groupBox1.TabStop = false;
            // 
            // ultraLabel1
            // 
            appearance1.ForeColor = System.Drawing.Color.White;
            this.ultraLabel1.Appearance = appearance1;
            this.ultraLabel1.AutoSize = true;
            this.ultraLabel1.Location = new System.Drawing.Point(19, 129);
            this.ultraLabel1.Name = "ultraLabel1";
            this.ultraLabel1.Size = new System.Drawing.Size(56, 18);
            this.ultraLabel1.TabIndex = 24;
            this.ultraLabel1.Text = "Sort By";
            // 
            // cboSortType
            // 
            appearance2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(108)))), ((int)(((byte)(172)))));
            appearance2.ForeColor = System.Drawing.Color.White;
            this.cboSortType.Appearance = appearance2;
            this.cboSortType.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(108)))), ((int)(((byte)(172)))));
            this.cboSortType.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance3.BackColor = System.Drawing.Color.WhiteSmoke;
            appearance3.BackColor2 = System.Drawing.Color.Silver;
            appearance3.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            this.cboSortType.ButtonAppearance = appearance3;
            this.cboSortType.ButtonStyle = Infragistics.Win.UIElementButtonStyle.FlatBorderless;
            this.cboSortType.DropDownStyle = Infragistics.Win.DropDownStyle.DropDownList;
            this.cboSortType.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboSortType.Location = new System.Drawing.Point(251, 127);
            this.cboSortType.Name = "cboSortType";
            this.cboSortType.Size = new System.Drawing.Size(132, 22);
            this.cboSortType.TabIndex = 23;
            this.cboSortType.TabStop = false;
            this.cboSortType.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            // 
            // ultraLabel20
            // 
            appearance4.ForeColor = System.Drawing.Color.White;
            this.ultraLabel20.Appearance = appearance4;
            this.ultraLabel20.Location = new System.Drawing.Point(19, 20);
            this.ultraLabel20.Name = "ultraLabel20";
            this.ultraLabel20.Size = new System.Drawing.Size(116, 14);
            this.ultraLabel20.TabIndex = 20;
            this.ultraLabel20.Text = "Start Date";
            // 
            // ultraLabel19
            // 
            appearance5.ForeColor = System.Drawing.Color.White;
            this.ultraLabel19.Appearance = appearance5;
            this.ultraLabel19.Location = new System.Drawing.Point(19, 57);
            this.ultraLabel19.Name = "ultraLabel19";
            this.ultraLabel19.Size = new System.Drawing.Size(106, 14);
            this.ultraLabel19.TabIndex = 22;
            this.ultraLabel19.Text = "End Date";
            // 
            // dtpSaleEndDate
            // 
            this.dtpSaleEndDate.AllowNull = false;
            this.dtpSaleEndDate.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Office2003ToolbarButton;
            this.dtpSaleEndDate.DateButtons.Add(dateButton1);
            this.dtpSaleEndDate.Location = new System.Drawing.Point(251, 53);
            this.dtpSaleEndDate.Name = "dtpSaleEndDate";
            this.dtpSaleEndDate.NonAutoSizeHeight = 10;
            this.dtpSaleEndDate.Size = new System.Drawing.Size(132, 22);
            this.dtpSaleEndDate.TabIndex = 1;
            this.dtpSaleEndDate.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.dtpSaleEndDate.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.dtpSaleEndDate.Value = new System.DateTime(2004, 5, 25, 0, 0, 0, 0);
            // 
            // dtpSaleStartDate
            // 
            this.dtpSaleStartDate.AllowNull = false;
            this.dtpSaleStartDate.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Office2003ToolbarButton;
            this.dtpSaleStartDate.DateButtons.Add(dateButton2);
            this.dtpSaleStartDate.Location = new System.Drawing.Point(251, 16);
            this.dtpSaleStartDate.Name = "dtpSaleStartDate";
            this.dtpSaleStartDate.NonAutoSizeHeight = 10;
            this.dtpSaleStartDate.Size = new System.Drawing.Size(132, 22);
            this.dtpSaleStartDate.TabIndex = 0;
            this.dtpSaleStartDate.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.dtpSaleStartDate.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.dtpSaleStartDate.Value = new System.DateTime(2004, 5, 25, 0, 0, 0, 0);
            // 
            // ultraLabel12
            // 
            appearance6.ForeColor = System.Drawing.Color.White;
            this.ultraLabel12.Appearance = appearance6;
            this.ultraLabel12.AutoSize = true;
            this.ultraLabel12.Location = new System.Drawing.Point(19, 92);
            this.ultraLabel12.Name = "ultraLabel12";
            this.ultraLabel12.Size = new System.Drawing.Size(195, 18);
            this.ultraLabel12.TabIndex = 6;
            this.ultraLabel12.Text = "User ID <Blank = Any User";
            // 
            // txtUserID
            // 
            this.txtUserID.AutoSize = false;
            this.txtUserID.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.txtUserID.Location = new System.Drawing.Point(251, 90);
            this.txtUserID.MaxLength = 20;
            this.txtUserID.Name = "txtUserID";
            this.txtUserID.Size = new System.Drawing.Size(132, 22);
            this.txtUserID.TabIndex = 2;
            this.txtUserID.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // lblTransactionType
            // 
            appearance7.ForeColor = System.Drawing.Color.White;
            appearance7.ForeColorDisabled = System.Drawing.Color.White;
            appearance7.TextHAlignAsString = "Center";
            this.lblTransactionType.Appearance = appearance7;
            this.lblTransactionType.BackColorInternal = System.Drawing.Color.Transparent;
            this.lblTransactionType.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblTransactionType.Font = new System.Drawing.Font("Arial", 18F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTransactionType.Location = new System.Drawing.Point(0, 0);
            this.lblTransactionType.Name = "lblTransactionType";
            this.lblTransactionType.Size = new System.Drawing.Size(459, 30);
            this.lblTransactionType.TabIndex = 26;
            this.lblTransactionType.Text = "Timesheet";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnPrint);
            this.groupBox2.Controls.Add(this.btnClose);
            this.groupBox2.Controls.Add(this.btnView);
            this.groupBox2.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.groupBox2.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.Location = new System.Drawing.Point(10, 196);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(437, 57);
            this.groupBox2.TabIndex = 31;
            this.groupBox2.TabStop = false;
            // 
            // btnPrint
            // 
            appearance8.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(62)))), ((int)(((byte)(176)))));
            appearance8.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance8.FontData.BoldAsString = "True";
            appearance8.ForeColor = System.Drawing.Color.White;
            appearance8.Image = ((object)(resources.GetObject("appearance8.Image")));
            this.btnPrint.Appearance = appearance8;
            this.btnPrint.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            this.btnPrint.Location = new System.Drawing.Point(117, 19);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(85, 26);
            this.btnPrint.TabIndex = 6;
            this.btnPrint.Text = "&Print";
            this.btnPrint.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            // 
            // btnClose
            // 
            appearance9.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(62)))), ((int)(((byte)(176)))));
            appearance9.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance9.FontData.BoldAsString = "True";
            appearance9.ForeColor = System.Drawing.Color.White;
            appearance9.Image = ((object)(resources.GetObject("appearance9.Image")));
            this.btnClose.Appearance = appearance9;
            this.btnClose.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(301, 20);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(85, 26);
            this.btnClose.TabIndex = 7;
            this.btnClose.Text = "&Close";
            this.btnClose.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnView
            // 
            appearance10.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(62)))), ((int)(((byte)(176)))));
            appearance10.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance10.FontData.BoldAsString = "True";
            appearance10.ForeColor = System.Drawing.Color.White;
            appearance10.Image = ((object)(resources.GetObject("appearance10.Image")));
            this.btnView.Appearance = appearance10;
            this.btnView.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            this.btnView.Location = new System.Drawing.Point(209, 20);
            this.btnView.Name = "btnView";
            this.btnView.Size = new System.Drawing.Size(85, 26);
            this.btnView.TabIndex = 5;
            this.btnView.Text = "&View";
            this.btnView.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnView.Click += new System.EventHandler(this.btnView_Click_1);
            // 
            // lblMessage
            // 
            appearance11.ForeColor = System.Drawing.Color.Red;
            appearance11.TextHAlignAsString = "Center";
            this.lblMessage.Appearance = appearance11;
            this.lblMessage.BackColorInternal = System.Drawing.Color.Transparent;
            this.lblMessage.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblMessage.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMessage.Location = new System.Drawing.Point(0, 261);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.Size = new System.Drawing.Size(459, 15);
            this.lblMessage.TabIndex = 75;
            this.lblMessage.Tag = "NOCOLOR";
            this.lblMessage.Text = "\"Total Earnings\" is hidden due to the logged-in user does not have enough permiss" +
    "ions.";
            this.lblMessage.Visible = false;
            // 
            // frmRptTimesheet
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(108)))), ((int)(((byte)(172)))));
            this.ClientSize = new System.Drawing.Size(459, 276);
            this.Controls.Add(this.lblMessage);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.lblTransactionType);
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.Name = "frmRptTimesheet";
            this.Text = "Timesheet";
            this.Load += new System.EventHandler(this.frmViewTransaction_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmViewTransaction_KeyDown);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cboSortType)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtpSaleEndDate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtpSaleStartDate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtUserID)).EndInit();
            this.groupBox2.ResumeLayout(false);
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

            this.Left = (frmMain.getInstance().Width - frmMain.getInstance().ultraExplorerBar1.Width - this.Width) / 2;
            this.Top = (frmMain.getInstance().Height - this.Height) / 2;

            clsUIHelper.setColorSchecme(this);
            this.dtpSaleEndDate.Value = DateTime.Now;
            this.dtpSaleStartDate.Value = DateTime.Now;
            //Added by shitaljit for PRIMEPOS-1265 Ability to view\print logged in users own clock-in\clock-out times
            if (UserPriviliges.IsUserHasPriviliges(UserPriviliges.Modules.Timesheet.ID, UserPriviliges.Screens.TimesheetReport.ID) == false)
            {

                this.txtUserID.Enabled = false;
                this.txtUserID.Text = Configuration.UserName;
            }
            loadSortType();

            #region PRIMEPOS-189 09-Aug-2021 JY Added            
            nDisplayHourlyRate = Configuration.convertBoolToInt(UserPriviliges.IsUserHasPriviliges(UserPriviliges.Modules.Timesheet.ID, UserPriviliges.Screens.DisplayHourlyRate.ID));
            if (nDisplayHourlyRate == 0)
            {
                lblMessage.Visible = true;
                tmrBlinking = new System.Timers.Timer();
                tmrBlinking.Interval = 1000;//1 seconds
                tmrBlinking.Elapsed += new ElapsedEventHandler(tmrBlinkingTimedEvent);
                tmrBlinking.Enabled = true;
            }
            else
            {
                this.Height -= 35;
            }
            #endregion
        }

        #region PRIMEPOS-189 09-Aug-2021 JY Added
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

        private void loadSortType()
        {
            try
            {
                this.cboSortType.Items.Clear();
                this.cboSortType.Items.Add("0", "--Select Sort Type--");

                this.cboSortType.Items.Add("1", StrSortByWorkingHrHTL.ToString());
                this.cboSortType.Items.Add("2", StrSortByWorkingHrLTH.ToString());

                this.cboSortType.Items.Add("3", StrSortByUserIDAsc.ToString());
                this.cboSortType.Items.Add("4", StrSortByUserIDDesc.ToString());

                this.cboSortType.Items.Add("5", StrSortByStardDateToEndDate.ToString());
                this.cboSortType.Items.Add("6", StrSortByEnddDateToStardDate.ToString());
                this.cboSortType.SelectedIndex = 0;
                //this.cboSortType.SelectedIndex = 0;
            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
            }

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
            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }

        private void PreviewReport(bool blnPrint)
        {
            try
            {
                DateTime frDate, Tdate;
                if (DateTime.TryParse(this.dtpSaleStartDate.Text.Trim(), out frDate) && DateTime.TryParse(this.dtpSaleEndDate.Text.Trim(), out Tdate))
                {
                    if (frDate > Tdate)
                    {
                        clsUIHelper.ShowErrorMsg("From Date cannot be greater than To Date ");
                        return;
                    }
                }
                else
                {
                    clsUIHelper.ShowErrorMsg("Invalid date format ");
                    return;
                }
                this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
                rptTimesheet oRpt = new rptTimesheet();

                String strQuery;

                strQuery = " Select Users.UserID, Users.LName + ', ' + Users.FName  as FullName ,TimeIn,TimeOut, " +
                    " Case When Year(TimeIn) < 1901 Then 0 When Year(TimeOut) < 1901 Then 0 Else DateDiff(MI, timein, IsNull(timeout,timein)) End as Minutes," +
                    " '" + this.dtpSaleStartDate.Text + "' as StartDate, '" + this.dtpSaleEndDate.Text + "' as EndDate," +
                    " Case When Year(TimeIn) < 1901 Then 0 When Year(TimeOut) < 1901 Then 0 Else ISNULL((DateDiff(MI, timein, IsNull(timeout, timein))) / 60.0 * Users.HourlyRate, 0.00) End AS TotalEarnings," +
                    nDisplayHourlyRate + " AS DisplayTotalEarnings" +   //PRIMEPOS-189 09-Aug-2021 JY Added
                    " From Users, Timesheet TS Where Users.UserID=TS.UserID " +
                    " and convert(datetime,TS.TimeIn,109) between convert(datetime, cast('" + this.dtpSaleStartDate.Text + " 00:00:00' as datetime) ,113) and convert(datetime, cast('" + this.dtpSaleEndDate.Text + " 23:59:59' as datetime) ,113) ";

                if (this.txtUserID.Text.Trim() != "")
                    strQuery += " and Users.UserID='" + this.txtUserID.Text.Trim() + "' ";
                if (!string.IsNullOrEmpty(cboSortType.Text))
                {
                    if (cboSortType.Text.Trim() == StrSortByEnddDateToStardDate.Trim())
                    {
                        strQuery += " Order By Users.UserID , TimeIn desc ,TimeOut ";
                    }
                    else
                        if (cboSortType.Text.Trim() == StrSortByStardDateToEndDate.Trim())
                    {
                        strQuery += " Order By Users.UserID , TimeIn , TimeOut";
                    }
                    else if (cboSortType.Text.Trim() == StrSortByUserIDAsc.Trim())
                    {
                        strQuery += " Order By Users.UserID,TimeIn, TimeOut ";
                    }
                    else if (cboSortType.Text.Trim() == StrSortByUserIDDesc.Trim())
                    {
                        strQuery += " Order By Users.UserID desc,TimeIn, TimeOut ";
                    }
                    else if (cboSortType.Text.Trim() == StrSortByWorkingHrHTL.Trim())
                    {
                        strQuery += "  Order By Users.UserID, Minutes desc,  TimeIn ";
                    }
                    else if (cboSortType.Text.Trim() == StrSortByWorkingHrLTH.Trim())
                    {
                        strQuery += " Order By  Users.UserID,Minutes, TimeIn ";
                    }
                    else
                    {
                        strQuery += " Order By Users.UserID,TimeIn, TimeOut ";
                    }
                }
                else
                {
                    strQuery += " Order By Users.UserID,TimeIn, TimeOut ";
                }

                /*Search oSearch=new Search();
				DataSet ds = oSearch.Search(strQuery);
				oRpt.SetDataSource(ds);
				frmReportViewer oViewer=new frmReportViewer();
				oViewer.rvReportViewer.ReportSource=oRpt;
				oViewer.MdiParent=frmMain.getInstance();
				oViewer.WindowState=FormWindowState.Maximized;
				oViewer.Show();
				*/
                clsReports.Preview(blnPrint, strQuery, oRpt);
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
            PreviewReport(true);
        }

        private void btnView_Click_1(object sender, System.EventArgs e)
        {
            PreviewReport(false);
        }

    }
}
