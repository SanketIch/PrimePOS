using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Windows.Forms;
using POS_Core.BusinessRules;
using POS_Core.CommonData;
//using POS_Core.DataAccess;
using POS_Core.CommonData.Rows;
using Infragistics.Win.UltraWinGrid;
using POS_Core.DataAccess;
//using POS_Core_UI.Reports.ReportsUI;
using Infragistics.Win;
using PharmData;
using POS_Core.Resources;
using System.Linq;
using NLog;

namespace POS_Core_UI
{
    /// <summary>
    /// Summary description for frmVendorSearch.
    /// </summary>
    public class frmPatientRXSearch : System.Windows.Forms.Form
    {
        public bool IsCanceled = true;
        private string sPatientNo = "";
        private int CurrentX;
        private int CurrentY;
        private RXHeader oPatient = null;
        private string buttonStatusCheck = "C&heck All";
        private string buttonStatusUnCheck = "&UnCheck All";
        private const String isFiled = "IsFiled";
        private const string isUnBilled = "IsUnBilled";
        public static long RXNo = 0;    //PRIMEPOS-2515 17-Oct-2018 JY changed RxNo datatype from int to long
        public static int RefillNo = 0;
        public static int PartialFillNo = 0;
        public string sFamilyID = string.Empty; //Sprint-25 - PRIMEPOS-2322 01-Feb-2017 JY Added    //PRIMEPOS-3093 24-May-2022 JY modified
        public string sFamilyMember { get; set; }
        private char cType = 'P';    //PRIMEPOS-2036 22-Jan-2019 JY Added
        private ILogger logger = LogManager.GetCurrentClassLogger();    //PRIMEPOS-2688 17-May-2019 JY Added nlog
        public bool isBatchDelivery = false; // NileshJ - BatchDelivery - PrimeRx-7688 23-Sept-2019

        #region builtin var
        private Infragistics.Win.UltraWinDataSource.UltraDataSource ultraDataSource1;
        private Infragistics.Win.UltraWinGrid.UltraGrid grdSearch;
        private Infragistics.Win.UltraWinStatusBar.UltraStatusBar sbMain;
        private Infragistics.Win.UltraWinSchedule.UltraCalendarCombo clTo1;
        private Infragistics.Win.UltraWinSchedule.UltraCalendarCombo clFrom1;
        private Infragistics.Win.Misc.UltraButton btnClose;
        private Infragistics.Win.Misc.UltraButton btnEdit;
        private IContainer components;
        private CheckBox chkSelectAll;
        private Infragistics.Win.Misc.UltraButton btnCheckBoxStatus;

        public DataTable SelectedData = null;
        private Infragistics.Win.UltraWinTabControl.UltraTabControl tabMain;
        private Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage ultraTabSharedControlsPage1;
        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl1;
        private Infragistics.Win.Misc.UltraLabel lblPhone;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor txtPhone;
        private Infragistics.Win.UltraWinSchedule.UltraCalendarCombo dtpDOB;
        private Infragistics.Win.Misc.UltraLabel txtDOB;
        private Infragistics.Win.Misc.UltraLabel ultraLabel3;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor txtGender;
        private Infragistics.Win.Misc.UltraLabel ultraLabel2;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor txtAddress;
        private Infragistics.Win.Misc.UltraLabel ultraLabel1;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor txtPatient;
        private Infragistics.Win.UltraWinSchedule.UltraCalendarCombo clTo;
        private Infragistics.Win.UltraWinSchedule.UltraCalendarCombo clFrom;
        private Infragistics.Win.Misc.UltraLabel lbl2;
        private Infragistics.Win.Misc.UltraLabel lbl1;
        private Infragistics.Win.Misc.UltraButton btnSearch;
        private TableLayoutPanel tlpSymbols;
        private Infragistics.Win.Misc.UltraLabel lblOlderRxs1;
        private Infragistics.Win.Misc.UltraLabel lblOlderRxs;
        private Infragistics.Win.Misc.UltraLabel lblUnbilledRxs;
        private Infragistics.Win.Misc.UltraLabel lblUnbilledRxs1;
        private Infragistics.Win.Misc.UltraLabel lblFiledRxs;
        private Infragistics.Win.Misc.UltraLabel lblFiledRxs1;
        private Infragistics.Win.Misc.UltraLabel lblFamilyRxs;
        private Infragistics.Win.Misc.UltraLabel lblFamilyRxs1;
        private Infragistics.Win.Misc.UltraLabel lblNonRPHVRxs;
        private Infragistics.Win.Misc.UltraLabel lblNonRPHVRxs1;
        PharmBL oPharmBL = new PharmBL();//Added by Krishna on 21 August 2012
        #endregion


        public frmPatientRXSearch(RXHeader oRXHeader)
        {
            logger.Trace("frmPatientRXSearch(RXHeader oRXHeader) - " + clsPOSDBConstants.Log_Entering);
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();
            //
            // TODO: Add any constructor code after InitializeComponent call
            //
            sFamilyID = oRXHeader.FamilyID; //Sprint-25 - PRIMEPOS-2322 01-Feb-2017 JY Added
            sPatientNo = oRXHeader.PatientNo;
            this.txtPatient.Text = oRXHeader.PatientName;
            oPatient = oRXHeader;
            this.clFrom.Value = DateTime.Today.Date.Subtract(new System.TimeSpan(Configuration.CInfo.UnPickedRXSearchDays, 0, 0, 0));
            this.clTo.Value = DateTime.Today;
            //Following code added by Krishna on 21 August 2012
            //POS.ErrorLogging.Logs.Logger("frmPatientRXSearch", "frmPatientRXSearch()", "About to call PharmSQL");
            logger.Trace("Search(DateTime fromDate, DateTime toDate) - " + "About to call PharmSQL");
            DataTable dtPatient = null;
            if (oRXHeader.TblPatient != null)
                dtPatient = oRXHeader.TblPatient;
            else
                dtPatient = oPharmBL.GetPatient(sPatientNo);
            //POS.ErrorLogging.Logs.Logger("frmPatientRXSearch", "frmPatientRXSearch()", "Call PharmSQL Sucessful");
            logger.Trace("Search(DateTime fromDate, DateTime toDate) - " + "Call PharmSQL Successful");
            //Code is modified by shitaljit 0n 18Apr2013 to avoid null exception condition
            //PRIMEPOS JIRA- 801
            if (dtPatient != null)
            {
                if (dtPatient.Rows.Count > 0)
                {
                    txtGender.Text = Configuration.convertNullToString(dtPatient.Rows[0]["Sex"].ToString().Trim()) == "M" ? "Male" : "Female";
                    try
                    {
                        dtpDOB.Value = string.IsNullOrEmpty(Configuration.convertNullToString(dtPatient.Rows[0]["DOB"].ToString().Trim())) ? Configuration.MinimumDate : Convert.ToDateTime(dtPatient.Rows[0]["DOB"].ToString().Trim());
                    }
                    catch
                    {
                        dtpDOB.Value = Configuration.MinimumDate;
                    }
                    txtPhone.Text = Configuration.convertNullToString(dtPatient.Rows[0]["Phone"].ToString().Trim());
                    //End of added by Krishna
                }
            }
            logger.Trace("frmPatientRXSearch(RXHeader oRXHeader) - " + clsPOSDBConstants.Log_Exiting);
        }

        #region Sprint-23 - PRIMEPOS-2276 06-Jun-2016 JY Added 
        public frmPatientRXSearch(string PatientNo, char Type)
        {
            logger.Trace("frmPatientRXSearch(string PatientNo, char Type) - " + clsPOSDBConstants.Log_Entering);
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();
            //
            // TODO: Add any constructor code after InitializeComponent call
            //
            sPatientNo = PatientNo;
            cType = Type;
            this.clFrom.Value = DateTime.Today.Date.Subtract(new System.TimeSpan(Configuration.CInfo.UnPickedRXSearchDays, 0, 0, 0));
            this.clTo.Value = DateTime.Today;

            if (cType == 'P')    //PRIMEPOS-2036 22-Jan-2019 JY Added Type to distingush between Patient and FacilityCode
            {

                logger.Trace("frmPatientRXSearch(string PatientNo, char Type) - " + "About to call PharmSQL");
                //POS.ErrorLogging.Logs.Logger("frmPatientRXSearch", "frmPatientRXSearch()", "About to call PharmSQL");
                DataTable dtPatient = oPharmBL.GetPatient(sPatientNo);
                logger.Trace("frmPatientRXSearch(string PatientNo, char Type) - " + "Call PharmSQL Successful");
                //POS.ErrorLogging.Logs.Logger("frmPatientRXSearch", "frmPatientRXSearch()", "Call PharmSQL Sucessful");
                //Code is modified by shitaljit 0n 18Apr2013 to avoid null exception condition
                //PRIMEPOS JIRA- 801
                if (dtPatient != null)
                {
                    if (dtPatient.Rows.Count > 0)
                    {
                        txtGender.Text = Configuration.convertNullToString(dtPatient.Rows[0]["Sex"].ToString().Trim()) == "M" ? "Male" : "Female";
                        try
                        {
                            dtpDOB.Value = string.IsNullOrEmpty(Configuration.convertNullToString(dtPatient.Rows[0]["DOB"].ToString().Trim())) ? Configuration.MinimumDate : Convert.ToDateTime(dtPatient.Rows[0]["DOB"].ToString().Trim());
                        }
                        catch
                        {
                            dtpDOB.Value = Configuration.MinimumDate;
                        }
                        txtPhone.Text = Configuration.convertNullToString(dtPatient.Rows[0]["Phone"].ToString().Trim());
                        //End of added by Krishna
                        sFamilyID = Configuration.convertNullToInt(dtPatient.Rows[0]["FamilyID"]).ToString();
                    }
                }
            }
            logger.Trace("frmPatientRXSearch(string PatientNo, char Type) - " + clsPOSDBConstants.Log_Exiting);
        }
        #endregion

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
            this.components = new System.ComponentModel.Container();
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinSchedule.CalendarCombo.DateButton dateButton1 = new Infragistics.Win.UltraWinSchedule.CalendarCombo.DateButton();
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinSchedule.CalendarCombo.DateButton dateButton2 = new Infragistics.Win.UltraWinSchedule.CalendarCombo.DateButton();
            Infragistics.Win.UltraWinSchedule.CalendarCombo.DateButton dateButton3 = new Infragistics.Win.UltraWinSchedule.CalendarCombo.DateButton();
            Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance7 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance8 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance9 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinDataSource.UltraDataColumn ultraDataColumn1 = new Infragistics.Win.UltraWinDataSource.UltraDataColumn("Column Name");
            Infragistics.Win.UltraWinDataSource.UltraDataColumn ultraDataColumn2 = new Infragistics.Win.UltraWinDataSource.UltraDataColumn("Type");
            Infragistics.Win.UltraWinDataSource.UltraDataColumn ultraDataColumn3 = new Infragistics.Win.UltraWinDataSource.UltraDataColumn("Criteria Value");
            Infragistics.Win.Appearance appearance10 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand1 = new Infragistics.Win.UltraWinGrid.UltraGridBand("Band 0", -1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn1 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Column Name");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn2 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Type");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn3 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Criteria Value");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn4 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("IsSelected", 0);
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
            Infragistics.Win.Appearance appearance29 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinScrollBar.ScrollBarLook scrollBarLook1 = new Infragistics.Win.UltraWinScrollBar.ScrollBarLook();
            Infragistics.Win.Appearance appearance30 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance31 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance32 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance33 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance34 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance35 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinStatusBar.UltraStatusPanel ultraStatusPanel1 = new Infragistics.Win.UltraWinStatusBar.UltraStatusPanel();
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
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab1 = new Infragistics.Win.UltraWinTabControl.UltraTab();
            Infragistics.Win.Appearance appearance46 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance47 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance48 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance53 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance51 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance52 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance50 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance49 = new Infragistics.Win.Appearance();
            this.ultraTabPageControl1 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.lblPhone = new Infragistics.Win.Misc.UltraLabel();
            this.txtPhone = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.dtpDOB = new Infragistics.Win.UltraWinSchedule.UltraCalendarCombo();
            this.txtDOB = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabel3 = new Infragistics.Win.Misc.UltraLabel();
            this.txtGender = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.ultraLabel2 = new Infragistics.Win.Misc.UltraLabel();
            this.txtAddress = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.ultraLabel1 = new Infragistics.Win.Misc.UltraLabel();
            this.txtPatient = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.clTo = new Infragistics.Win.UltraWinSchedule.UltraCalendarCombo();
            this.clFrom = new Infragistics.Win.UltraWinSchedule.UltraCalendarCombo();
            this.lbl2 = new Infragistics.Win.Misc.UltraLabel();
            this.lbl1 = new Infragistics.Win.Misc.UltraLabel();
            this.btnSearch = new Infragistics.Win.Misc.UltraButton();
            this.clTo1 = new Infragistics.Win.UltraWinSchedule.UltraCalendarCombo();
            this.clFrom1 = new Infragistics.Win.UltraWinSchedule.UltraCalendarCombo();
            this.ultraDataSource1 = new Infragistics.Win.UltraWinDataSource.UltraDataSource(this.components);
            this.grdSearch = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.sbMain = new Infragistics.Win.UltraWinStatusBar.UltraStatusBar();
            this.btnClose = new Infragistics.Win.Misc.UltraButton();
            this.btnEdit = new Infragistics.Win.Misc.UltraButton();
            this.chkSelectAll = new System.Windows.Forms.CheckBox();
            this.btnCheckBoxStatus = new Infragistics.Win.Misc.UltraButton();
            this.tabMain = new Infragistics.Win.UltraWinTabControl.UltraTabControl();
            this.ultraTabSharedControlsPage1 = new Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage();
            this.tlpSymbols = new System.Windows.Forms.TableLayoutPanel();
            this.lblOlderRxs1 = new Infragistics.Win.Misc.UltraLabel();
            this.lblOlderRxs = new Infragistics.Win.Misc.UltraLabel();
            this.lblUnbilledRxs = new Infragistics.Win.Misc.UltraLabel();
            this.lblUnbilledRxs1 = new Infragistics.Win.Misc.UltraLabel();
            this.lblFiledRxs = new Infragistics.Win.Misc.UltraLabel();
            this.lblFiledRxs1 = new Infragistics.Win.Misc.UltraLabel();
            this.lblFamilyRxs = new Infragistics.Win.Misc.UltraLabel();
            this.lblFamilyRxs1 = new Infragistics.Win.Misc.UltraLabel();
            this.lblNonRPHVRxs1 = new Infragistics.Win.Misc.UltraLabel();
            this.lblNonRPHVRxs = new Infragistics.Win.Misc.UltraLabel();
            this.ultraTabPageControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtPhone)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtpDOB)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtGender)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtAddress)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPatient)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.clTo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.clFrom)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.clTo1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.clFrom1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraDataSource1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdSearch)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sbMain)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tabMain)).BeginInit();
            this.tabMain.SuspendLayout();
            this.tlpSymbols.SuspendLayout();
            this.SuspendLayout();
            // 
            // ultraTabPageControl1
            // 
            this.ultraTabPageControl1.Controls.Add(this.lblPhone);
            this.ultraTabPageControl1.Controls.Add(this.txtPhone);
            this.ultraTabPageControl1.Controls.Add(this.dtpDOB);
            this.ultraTabPageControl1.Controls.Add(this.txtDOB);
            this.ultraTabPageControl1.Controls.Add(this.ultraLabel3);
            this.ultraTabPageControl1.Controls.Add(this.txtGender);
            this.ultraTabPageControl1.Controls.Add(this.ultraLabel2);
            this.ultraTabPageControl1.Controls.Add(this.txtAddress);
            this.ultraTabPageControl1.Controls.Add(this.ultraLabel1);
            this.ultraTabPageControl1.Controls.Add(this.txtPatient);
            this.ultraTabPageControl1.Controls.Add(this.clTo);
            this.ultraTabPageControl1.Controls.Add(this.clFrom);
            this.ultraTabPageControl1.Controls.Add(this.lbl2);
            this.ultraTabPageControl1.Controls.Add(this.lbl1);
            this.ultraTabPageControl1.Controls.Add(this.btnSearch);
            this.ultraTabPageControl1.Location = new System.Drawing.Point(2, 21);
            this.ultraTabPageControl1.Name = "ultraTabPageControl1";
            this.ultraTabPageControl1.Size = new System.Drawing.Size(1061, 88);
            // 
            // lblPhone
            // 
            appearance1.ForeColor = System.Drawing.Color.Black;
            this.lblPhone.Appearance = appearance1;
            this.lblPhone.Location = new System.Drawing.Point(566, 62);
            this.lblPhone.Name = "lblPhone";
            this.lblPhone.Size = new System.Drawing.Size(63, 18);
            this.lblPhone.TabIndex = 29;
            this.lblPhone.Text = "Phone #";
            // 
            // txtPhone
            // 
            this.txtPhone.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.txtPhone.Enabled = false;
            this.txtPhone.Location = new System.Drawing.Point(640, 61);
            this.txtPhone.MaxLength = 20;
            this.txtPhone.Name = "txtPhone";
            this.txtPhone.Size = new System.Drawing.Size(132, 20);
            this.txtPhone.TabIndex = 6;
            this.txtPhone.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // dtpDOB
            // 
            this.dtpDOB.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.dtpDOB.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Office2003ToolbarButton;
            this.dtpDOB.DateButtons.Add(dateButton1);
            this.dtpDOB.Enabled = false;
            this.dtpDOB.Location = new System.Drawing.Point(640, 36);
            this.dtpDOB.Name = "dtpDOB";
            this.dtpDOB.NonAutoSizeHeight = 22;
            this.dtpDOB.Size = new System.Drawing.Size(132, 21);
            this.dtpDOB.TabIndex = 5;
            this.dtpDOB.Value = new System.DateTime(2008, 7, 11, 0, 0, 0, 0);
            // 
            // txtDOB
            // 
            appearance2.ForeColor = System.Drawing.Color.Black;
            this.txtDOB.Appearance = appearance2;
            this.txtDOB.BackColorInternal = System.Drawing.Color.Transparent;
            this.txtDOB.Location = new System.Drawing.Point(592, 37);
            this.txtDOB.Name = "txtDOB";
            this.txtDOB.Size = new System.Drawing.Size(37, 18);
            this.txtDOB.TabIndex = 27;
            this.txtDOB.Text = "DOB";
            this.txtDOB.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // ultraLabel3
            // 
            appearance3.ForeColor = System.Drawing.Color.Black;
            this.ultraLabel3.Appearance = appearance3;
            this.ultraLabel3.Location = new System.Drawing.Point(572, 11);
            this.ultraLabel3.Name = "ultraLabel3";
            this.ultraLabel3.Size = new System.Drawing.Size(57, 18);
            this.ultraLabel3.TabIndex = 25;
            this.ultraLabel3.Text = "Gender";
            // 
            // txtGender
            // 
            this.txtGender.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.txtGender.Enabled = false;
            this.txtGender.Location = new System.Drawing.Point(640, 10);
            this.txtGender.MaxLength = 20;
            this.txtGender.Name = "txtGender";
            this.txtGender.Size = new System.Drawing.Size(132, 20);
            this.txtGender.TabIndex = 4;
            this.txtGender.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // ultraLabel2
            // 
            appearance4.ForeColor = System.Drawing.Color.Black;
            this.ultraLabel2.Appearance = appearance4;
            this.ultraLabel2.Location = new System.Drawing.Point(14, 62);
            this.ultraLabel2.Name = "ultraLabel2";
            this.ultraLabel2.Size = new System.Drawing.Size(62, 18);
            this.ultraLabel2.TabIndex = 23;
            this.ultraLabel2.Text = "Address";
            // 
            // txtAddress
            // 
            this.txtAddress.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.txtAddress.Enabled = false;
            this.txtAddress.Location = new System.Drawing.Point(89, 61);
            this.txtAddress.MaxLength = 20;
            this.txtAddress.Name = "txtAddress";
            this.txtAddress.Size = new System.Drawing.Size(464, 20);
            this.txtAddress.TabIndex = 3;
            this.txtAddress.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // ultraLabel1
            // 
            appearance5.ForeColor = System.Drawing.Color.Black;
            this.ultraLabel1.Appearance = appearance5;
            this.ultraLabel1.Location = new System.Drawing.Point(14, 37);
            this.ultraLabel1.Name = "ultraLabel1";
            this.ultraLabel1.Size = new System.Drawing.Size(57, 18);
            this.ultraLabel1.TabIndex = 21;
            this.ultraLabel1.Text = "Patient";
            // 
            // txtPatient
            // 
            this.txtPatient.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.txtPatient.Enabled = false;
            this.txtPatient.Location = new System.Drawing.Point(89, 36);
            this.txtPatient.MaxLength = 20;
            this.txtPatient.Name = "txtPatient";
            this.txtPatient.Size = new System.Drawing.Size(464, 20);
            this.txtPatient.TabIndex = 2;
            this.txtPatient.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // clTo
            // 
            this.clTo.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.clTo.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Office2003ToolbarButton;
            this.clTo.DateButtons.Add(dateButton2);
            this.clTo.Location = new System.Drawing.Point(396, 10);
            this.clTo.Name = "clTo";
            this.clTo.NonAutoSizeHeight = 22;
            this.clTo.Size = new System.Drawing.Size(157, 21);
            this.clTo.TabIndex = 1;
            this.clTo.Value = new System.DateTime(2008, 7, 11, 0, 0, 0, 0);
            // 
            // clFrom
            // 
            this.clFrom.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.clFrom.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Office2003ToolbarButton;
            this.clFrom.DateButtons.Add(dateButton3);
            this.clFrom.Location = new System.Drawing.Point(89, 10);
            this.clFrom.Name = "clFrom";
            this.clFrom.NonAutoSizeHeight = 22;
            this.clFrom.Size = new System.Drawing.Size(157, 21);
            this.clFrom.TabIndex = 0;
            this.clFrom.Value = new System.DateTime(2008, 7, 11, 0, 0, 0, 0);
            // 
            // lbl2
            // 
            appearance6.ForeColor = System.Drawing.Color.Black;
            this.lbl2.Appearance = appearance6;
            this.lbl2.BackColorInternal = System.Drawing.Color.Transparent;
            this.lbl2.Location = new System.Drawing.Point(323, 11);
            this.lbl2.Name = "lbl2";
            this.lbl2.Size = new System.Drawing.Size(22, 18);
            this.lbl2.TabIndex = 7;
            this.lbl2.Text = "To ";
            this.lbl2.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // lbl1
            // 
            appearance7.ForeColor = System.Drawing.Color.Black;
            this.lbl1.Appearance = appearance7;
            this.lbl1.BackColorInternal = System.Drawing.Color.Transparent;
            this.lbl1.Location = new System.Drawing.Point(14, 11);
            this.lbl1.Name = "lbl1";
            this.lbl1.Size = new System.Drawing.Size(40, 18);
            this.lbl1.TabIndex = 5;
            this.lbl1.Text = "From ";
            this.lbl1.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // btnSearch
            // 
            this.btnSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            appearance8.BackColor = System.Drawing.Color.White;
            appearance8.BackColor2 = System.Drawing.SystemColors.Control;
            appearance8.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance8.FontData.BoldAsString = "True";
            appearance8.ForeColor = System.Drawing.Color.Black;
            this.btnSearch.Appearance = appearance8;
            this.btnSearch.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Flat;
            appearance9.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(188)))), ((int)(((byte)(193)))), ((int)(((byte)(207)))));
            appearance9.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(188)))), ((int)(((byte)(193)))), ((int)(((byte)(207)))));
            this.btnSearch.HotTrackAppearance = appearance9;
            this.btnSearch.Location = new System.Drawing.Point(929, 12);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(121, 30);
            this.btnSearch.TabIndex = 5;
            this.btnSearch.Text = "&Search (F4)";
            this.btnSearch.UseHotTracking = Infragistics.Win.DefaultableBoolean.True;
            this.btnSearch.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // clTo1
            // 
            this.clTo1.BackColor = System.Drawing.SystemColors.Window;
            this.clTo1.Location = new System.Drawing.Point(0, 0);
            this.clTo1.Name = "clTo1";
            this.clTo1.NonAutoSizeHeight = 21;
            this.clTo1.Size = new System.Drawing.Size(121, 21);
            this.clTo1.TabIndex = 0;
            this.clTo1.Value = new System.DateTime(2008, 7, 11, 0, 0, 0, 0);
            // 
            // clFrom1
            // 
            this.clFrom1.BackColor = System.Drawing.SystemColors.Window;
            this.clFrom1.Location = new System.Drawing.Point(1, 1);
            this.clFrom1.Name = "clFrom1";
            this.clFrom1.NonAutoSizeHeight = 21;
            this.clFrom1.Size = new System.Drawing.Size(121, 21);
            this.clFrom1.TabIndex = 0;
            this.clFrom1.Value = new System.DateTime(2008, 7, 11, 0, 0, 0, 0);
            // 
            // ultraDataSource1
            // 
            this.ultraDataSource1.Band.Columns.AddRange(new object[] {
            ultraDataColumn1,
            ultraDataColumn2,
            ultraDataColumn3});
            // 
            // grdSearch
            // 
            this.grdSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            appearance10.BackColor = System.Drawing.Color.White;
            appearance10.BackColor2 = System.Drawing.Color.White;
            appearance10.BackColorDisabled = System.Drawing.Color.White;
            appearance10.BackColorDisabled2 = System.Drawing.Color.White;
            appearance10.BackGradientStyle = Infragistics.Win.GradientStyle.None;
            this.grdSearch.DisplayLayout.Appearance = appearance10;
            ultraGridColumn1.Header.VisiblePosition = 0;
            ultraGridColumn2.Header.VisiblePosition = 1;
            ultraGridColumn3.Header.VisiblePosition = 2;
            ultraGridColumn4.DataType = typeof(bool);
            ultraGridColumn4.Header.VisiblePosition = 3;
            ultraGridColumn4.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.CheckBox;
            ultraGridBand1.Columns.AddRange(new object[] {
            ultraGridColumn1,
            ultraGridColumn2,
            ultraGridColumn3,
            ultraGridColumn4});
            ultraGridBand1.HeaderVisible = true;
            this.grdSearch.DisplayLayout.BandsSerializer.Add(ultraGridBand1);
            this.grdSearch.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.grdSearch.DisplayLayout.InterBandSpacing = 10;
            this.grdSearch.DisplayLayout.MaxColScrollRegions = 1;
            this.grdSearch.DisplayLayout.MaxRowScrollRegions = 1;
            appearance11.BackColor = System.Drawing.Color.White;
            appearance11.BackColor2 = System.Drawing.Color.White;
            this.grdSearch.DisplayLayout.Override.ActiveCardCaptionAppearance = appearance11;
            appearance12.BackColor = System.Drawing.Color.White;
            appearance12.BackColor2 = System.Drawing.Color.White;
            appearance12.BorderColor = System.Drawing.Color.Gray;
            this.grdSearch.DisplayLayout.Override.ActiveRowAppearance = appearance12;
            appearance13.BackColor = System.Drawing.Color.White;
            appearance13.BackColor2 = System.Drawing.Color.White;
            appearance13.BorderColor = System.Drawing.Color.Gray;
            this.grdSearch.DisplayLayout.Override.AddRowAppearance = appearance13;
            this.grdSearch.DisplayLayout.Override.AllowAddNew = Infragistics.Win.UltraWinGrid.AllowAddNew.No;
            this.grdSearch.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.False;
            this.grdSearch.DisplayLayout.Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.False;
            appearance14.BackColor = System.Drawing.Color.Transparent;
            this.grdSearch.DisplayLayout.Override.CardAreaAppearance = appearance14;
            appearance15.BackColor = System.Drawing.Color.White;
            appearance15.BackColor2 = System.Drawing.Color.White;
            appearance15.BackColorDisabled = System.Drawing.Color.White;
            appearance15.BackColorDisabled2 = System.Drawing.Color.White;
            appearance15.BorderColor = System.Drawing.Color.Black;
            appearance15.BorderColor3DBase = System.Drawing.Color.Black;
            this.grdSearch.DisplayLayout.Override.CellAppearance = appearance15;
            appearance16.BackColor = System.Drawing.Color.White;
            appearance16.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(231)))), ((int)(((byte)(230)))));
            appearance16.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance16.BorderAlpha = Infragistics.Win.Alpha.Transparent;
            appearance16.BorderColor = System.Drawing.Color.Gray;
            appearance16.BorderColor3DBase = System.Drawing.Color.Gray;
            appearance16.ImageBackgroundAlpha = Infragistics.Win.Alpha.Transparent;
            appearance16.ImageBackgroundStyle = Infragistics.Win.ImageBackgroundStyle.Centered;
            this.grdSearch.DisplayLayout.Override.CellButtonAppearance = appearance16;
            appearance17.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            appearance17.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.grdSearch.DisplayLayout.Override.EditCellAppearance = appearance17;
            appearance18.BorderColor = System.Drawing.Color.Gray;
            this.grdSearch.DisplayLayout.Override.FilteredInRowAppearance = appearance18;
            appearance19.BorderColor = System.Drawing.Color.Gray;
            this.grdSearch.DisplayLayout.Override.FilteredOutRowAppearance = appearance19;
            appearance20.BackColor = System.Drawing.Color.White;
            appearance20.BackColor2 = System.Drawing.Color.White;
            appearance20.BackColorDisabled = System.Drawing.Color.White;
            appearance20.BackColorDisabled2 = System.Drawing.Color.White;
            this.grdSearch.DisplayLayout.Override.FixedCellAppearance = appearance20;
            appearance21.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            appearance21.BackColor2 = System.Drawing.Color.Beige;
            this.grdSearch.DisplayLayout.Override.FixedHeaderAppearance = appearance21;
            appearance22.BackColor = System.Drawing.Color.White;
            appearance22.BackColor2 = System.Drawing.SystemColors.Control;
            appearance22.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance22.FontData.BoldAsString = "True";
            appearance22.ForeColor = System.Drawing.Color.Black;
            appearance22.TextHAlignAsString = "Left";
            appearance22.ThemedElementAlpha = Infragistics.Win.Alpha.Transparent;
            this.grdSearch.DisplayLayout.Override.HeaderAppearance = appearance22;
            appearance23.BorderColor = System.Drawing.Color.Gray;
            this.grdSearch.DisplayLayout.Override.RowAlternateAppearance = appearance23;
            appearance24.BackColor = System.Drawing.Color.White;
            appearance24.BackColor2 = System.Drawing.Color.White;
            appearance24.BackGradientStyle = Infragistics.Win.GradientStyle.None;
            appearance24.BackHatchStyle = Infragistics.Win.BackHatchStyle.None;
            appearance24.BorderColor = System.Drawing.Color.Gray;
            this.grdSearch.DisplayLayout.Override.RowAppearance = appearance24;
            appearance25.BorderColor = System.Drawing.Color.Gray;
            this.grdSearch.DisplayLayout.Override.RowPreviewAppearance = appearance25;
            appearance26.BackColor = System.Drawing.Color.White;
            appearance26.BackColor2 = System.Drawing.SystemColors.Control;
            appearance26.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance26.BorderColor = System.Drawing.Color.Gray;
            this.grdSearch.DisplayLayout.Override.RowSelectorAppearance = appearance26;
            this.grdSearch.DisplayLayout.Override.RowSelectorWidth = 12;
            this.grdSearch.DisplayLayout.Override.RowSpacingBefore = 2;
            appearance27.BackColor = System.Drawing.Color.Navy;
            appearance27.BackColorAlpha = Infragistics.Win.Alpha.Opaque;
            this.grdSearch.DisplayLayout.Override.SelectedCellAppearance = appearance27;
            appearance28.BackColor = System.Drawing.Color.Navy;
            appearance28.BackColorDisabled = System.Drawing.Color.Navy;
            appearance28.BackGradientStyle = Infragistics.Win.GradientStyle.None;
            appearance28.BackHatchStyle = Infragistics.Win.BackHatchStyle.None;
            appearance28.BorderColor = System.Drawing.Color.Gray;
            appearance28.ForeColor = System.Drawing.Color.Black;
            this.grdSearch.DisplayLayout.Override.SelectedRowAppearance = appearance28;
            this.grdSearch.DisplayLayout.Override.SelectTypeCell = Infragistics.Win.UltraWinGrid.SelectType.None;
            this.grdSearch.DisplayLayout.Override.SelectTypeCol = Infragistics.Win.UltraWinGrid.SelectType.None;
            this.grdSearch.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.Single;
            appearance29.BorderColor = System.Drawing.Color.Gray;
            this.grdSearch.DisplayLayout.Override.TemplateAddRowAppearance = appearance29;
            this.grdSearch.DisplayLayout.RowConnectorColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            this.grdSearch.DisplayLayout.RowConnectorStyle = Infragistics.Win.UltraWinGrid.RowConnectorStyle.Solid;
            appearance30.BackColor = System.Drawing.Color.White;
            appearance30.BackColor2 = System.Drawing.SystemColors.Control;
            appearance30.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            scrollBarLook1.Appearance = appearance30;
            appearance31.BackColor = System.Drawing.Color.White;
            appearance31.BackColor2 = System.Drawing.SystemColors.Control;
            appearance31.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance31.ThemedElementAlpha = Infragistics.Win.Alpha.Transparent;
            scrollBarLook1.ButtonAppearance = appearance31;
            appearance32.BackColor = System.Drawing.Color.White;
            appearance32.BackColor2 = System.Drawing.SystemColors.Control;
            scrollBarLook1.ThumbAppearance = appearance32;
            appearance33.BackColor = System.Drawing.Color.White;
            appearance33.BackColor2 = System.Drawing.Color.White;
            scrollBarLook1.TrackAppearance = appearance33;
            this.grdSearch.DisplayLayout.ScrollBarLook = scrollBarLook1;
            this.grdSearch.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.grdSearch.Location = new System.Drawing.Point(13, 123);
            this.grdSearch.Name = "grdSearch";
            this.grdSearch.Size = new System.Drawing.Size(1060, 372);
            this.grdSearch.TabIndex = 6;
            this.grdSearch.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.grdSearch.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.grdSearch_InitializeLayout);
            this.grdSearch.InitializeRow += new Infragistics.Win.UltraWinGrid.InitializeRowEventHandler(this.grdSearch_InitializeRow);
            this.grdSearch.BeforeRowExpanded += new Infragistics.Win.UltraWinGrid.CancelableRowEventHandler(this.grdSearch_BeforeRowExpanded);
            this.grdSearch.BeforeCellUpdate += new Infragistics.Win.UltraWinGrid.BeforeCellUpdateEventHandler(this.grdSearch_BeforeCellUpdate);
            this.grdSearch.KeyUp += new System.Windows.Forms.KeyEventHandler(this.grdSearch_KeyUp);
            this.grdSearch.Leave += new System.EventHandler(this.grdSearch_Leave);
            this.grdSearch.MouseClick += new System.Windows.Forms.MouseEventHandler(this.grdSearch_MouseClick);
            this.grdSearch.MouseMove += new System.Windows.Forms.MouseEventHandler(this.grdSearch_MouseMove);
            // 
            // sbMain
            // 
            appearance34.BackColor = System.Drawing.Color.White;
            appearance34.BackColor2 = System.Drawing.SystemColors.Control;
            appearance34.BorderColor = System.Drawing.Color.Black;
            appearance34.FontData.Name = "Verdana";
            appearance34.FontData.SizeInPoints = 10F;
            appearance34.ForeColor = System.Drawing.Color.White;
            this.sbMain.Appearance = appearance34;
            this.sbMain.Location = new System.Drawing.Point(0, 539);
            this.sbMain.Name = "sbMain";
            appearance35.BorderColor = System.Drawing.Color.Black;
            appearance35.BorderColor3DBase = System.Drawing.Color.Black;
            appearance35.ForeColor = System.Drawing.Color.Black;
            this.sbMain.PanelAppearance = appearance35;
            appearance36.BorderColor = System.Drawing.Color.White;
            ultraStatusPanel1.Appearance = appearance36;
            ultraStatusPanel1.SizingMode = Infragistics.Win.UltraWinStatusBar.PanelSizingMode.Spring;
            ultraStatusPanel1.Width = 200;
            this.sbMain.Panels.AddRange(new Infragistics.Win.UltraWinStatusBar.UltraStatusPanel[] {
            ultraStatusPanel1});
            this.sbMain.Size = new System.Drawing.Size(1084, 25);
            this.sbMain.TabIndex = 7;
            this.sbMain.ViewStyle = Infragistics.Win.UltraWinStatusBar.ViewStyle.VisualStudio2005;
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            appearance37.BackColor = System.Drawing.Color.White;
            appearance37.BackColor2 = System.Drawing.SystemColors.Control;
            appearance37.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance37.FontData.BoldAsString = "True";
            appearance37.ForeColor = System.Drawing.Color.Black;
            this.btnClose.Appearance = appearance37;
            this.btnClose.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Flat;
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            appearance38.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(188)))), ((int)(((byte)(193)))), ((int)(((byte)(207)))));
            appearance38.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(188)))), ((int)(((byte)(193)))), ((int)(((byte)(207)))));
            this.btnClose.HotTrackAppearance = appearance38;
            this.btnClose.Location = new System.Drawing.Point(971, 502);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(102, 30);
            this.btnClose.TabIndex = 8;
            this.btnClose.Text = "&Cancel";
            this.btnClose.UseHotTracking = Infragistics.Win.DefaultableBoolean.True;
            this.btnClose.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnEdit
            // 
            this.btnEdit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            appearance39.BackColor = System.Drawing.Color.White;
            appearance39.BackColor2 = System.Drawing.SystemColors.Control;
            appearance39.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance39.FontData.BoldAsString = "True";
            appearance39.ForeColor = System.Drawing.Color.Black;
            this.btnEdit.Appearance = appearance39;
            this.btnEdit.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Flat;
            appearance40.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(188)))), ((int)(((byte)(193)))), ((int)(((byte)(207)))));
            appearance40.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(188)))), ((int)(((byte)(193)))), ((int)(((byte)(207)))));
            this.btnEdit.HotTrackAppearance = appearance40;
            this.btnEdit.Location = new System.Drawing.Point(863, 502);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(102, 30);
            this.btnEdit.TabIndex = 9;
            this.btnEdit.Text = "&Ok";
            this.btnEdit.UseHotTracking = Infragistics.Win.DefaultableBoolean.True;
            this.btnEdit.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
            // 
            // chkSelectAll
            // 
            this.chkSelectAll.AutoSize = true;
            this.chkSelectAll.BackColor = System.Drawing.Color.Transparent;
            this.chkSelectAll.Checked = true;
            this.chkSelectAll.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkSelectAll.Font = new System.Drawing.Font("Verdana", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkSelectAll.Location = new System.Drawing.Point(34, 127);
            this.chkSelectAll.Name = "chkSelectAll";
            this.chkSelectAll.Size = new System.Drawing.Size(15, 14);
            this.chkSelectAll.TabIndex = 10;
            this.chkSelectAll.UseVisualStyleBackColor = false;
            this.chkSelectAll.CheckedChanged += new System.EventHandler(this.chkSelectAll_CheckedChanged);
            // 
            // btnCheckBoxStatus
            // 
            this.btnCheckBoxStatus.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            appearance41.BackColor = System.Drawing.Color.White;
            appearance41.BackColor2 = System.Drawing.SystemColors.Control;
            appearance41.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance41.FontData.BoldAsString = "True";
            appearance41.ForeColor = System.Drawing.Color.Black;
            this.btnCheckBoxStatus.Appearance = appearance41;
            this.btnCheckBoxStatus.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Flat;
            appearance42.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(188)))), ((int)(((byte)(193)))), ((int)(((byte)(207)))));
            appearance42.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(188)))), ((int)(((byte)(193)))), ((int)(((byte)(207)))));
            this.btnCheckBoxStatus.HotTrackAppearance = appearance42;
            this.btnCheckBoxStatus.Location = new System.Drawing.Point(736, 502);
            this.btnCheckBoxStatus.Name = "btnCheckBoxStatus";
            this.btnCheckBoxStatus.Size = new System.Drawing.Size(121, 30);
            this.btnCheckBoxStatus.TabIndex = 11;
            this.btnCheckBoxStatus.TabStop = false;
            this.btnCheckBoxStatus.Text = "&Uncheck All";
            this.btnCheckBoxStatus.UseHotTracking = Infragistics.Win.DefaultableBoolean.True;
            this.btnCheckBoxStatus.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnCheckBoxStatus.Click += new System.EventHandler(this.btnCheckBoxStatus_Click);
            // 
            // tabMain
            // 
            appearance43.FontData.BoldAsString = "True";
            this.tabMain.ActiveTabAppearance = appearance43;
            this.tabMain.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            appearance44.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(108)))), ((int)(((byte)(172)))));
            this.tabMain.Appearance = appearance44;
            this.tabMain.BackColorInternal = System.Drawing.Color.Transparent;
            appearance45.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(108)))), ((int)(((byte)(172)))));
            appearance45.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(108)))), ((int)(((byte)(172)))));
            this.tabMain.ClientAreaAppearance = appearance45;
            this.tabMain.Controls.Add(this.ultraTabSharedControlsPage1);
            this.tabMain.Controls.Add(this.ultraTabPageControl1);
            this.tabMain.Location = new System.Drawing.Point(9, 8);
            this.tabMain.Name = "tabMain";
            this.tabMain.SharedControlsPage = this.ultraTabSharedControlsPage1;
            this.tabMain.Size = new System.Drawing.Size(1065, 111);
            this.tabMain.Style = Infragistics.Win.UltraWinTabControl.UltraTabControlStyle.PropertyPage2003;
            this.tabMain.TabIndex = 12;
            appearance46.BackColor = System.Drawing.Color.Transparent;
            ultraTab1.Appearance = appearance46;
            appearance47.BackColor = System.Drawing.Color.Transparent;
            appearance47.BackColor2 = System.Drawing.Color.Transparent;
            appearance47.ForeColor = System.Drawing.Color.Black;
            ultraTab1.ClientAreaAppearance = appearance47;
            appearance48.BackColor = System.Drawing.Color.Transparent;
            appearance48.BackColor2 = System.Drawing.Color.Transparent;
            ultraTab1.SelectedAppearance = appearance48;
            ultraTab1.TabPage = this.ultraTabPageControl1;
            ultraTab1.Text = "Criteria";
            this.tabMain.Tabs.AddRange(new Infragistics.Win.UltraWinTabControl.UltraTab[] {
            ultraTab1});
            this.tabMain.TabStop = false;
            this.tabMain.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.tabMain.ViewStyle = Infragistics.Win.UltraWinTabControl.ViewStyle.Office2003;
            // 
            // ultraTabSharedControlsPage1
            // 
            this.ultraTabSharedControlsPage1.Location = new System.Drawing.Point(-10000, -10000);
            this.ultraTabSharedControlsPage1.Name = "ultraTabSharedControlsPage1";
            this.ultraTabSharedControlsPage1.Size = new System.Drawing.Size(1061, 88);
            // 
            // tlpSymbols
            // 
            this.tlpSymbols.ColumnCount = 10;
            this.tlpSymbols.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tlpSymbols.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tlpSymbols.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tlpSymbols.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tlpSymbols.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tlpSymbols.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tlpSymbols.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tlpSymbols.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tlpSymbols.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tlpSymbols.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tlpSymbols.Controls.Add(this.lblNonRPHVRxs, 8, 0);
            this.tlpSymbols.Controls.Add(this.lblNonRPHVRxs1, 9, 0);
            this.tlpSymbols.Controls.Add(this.lblFamilyRxs, 0, 0);
            this.tlpSymbols.Controls.Add(this.lblUnbilledRxs, 2, 0);
            this.tlpSymbols.Controls.Add(this.lblFamilyRxs1, 1, 0);
            this.tlpSymbols.Controls.Add(this.lblUnbilledRxs1, 3, 0);
            this.tlpSymbols.Controls.Add(this.lblFiledRxs, 4, 0);
            this.tlpSymbols.Controls.Add(this.lblFiledRxs1, 5, 0);
            this.tlpSymbols.Controls.Add(this.lblOlderRxs1, 7, 0);
            this.tlpSymbols.Controls.Add(this.lblOlderRxs, 6, 0);
            this.tlpSymbols.Location = new System.Drawing.Point(13, 502);
            this.tlpSymbols.Name = "tlpSymbols";
            this.tlpSymbols.RowCount = 1;
            this.tlpSymbols.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpSymbols.Size = new System.Drawing.Size(627, 25);
            this.tlpSymbols.TabIndex = 161;
            // 
            // lblOlderRxs1
            // 
            this.lblOlderRxs1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblOlderRxs1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblOlderRxs1.Location = new System.Drawing.Point(403, 3);
            this.lblOlderRxs1.Name = "lblOlderRxs1";
            this.lblOlderRxs1.Size = new System.Drawing.Size(94, 19);
            this.lblOlderRxs1.TabIndex = 11;
            this.lblOlderRxs1.Text = "Older Rxs";
            // 
            // lblOlderRxs
            // 
            appearance53.BackColor = System.Drawing.Color.LightPink;
            appearance53.BackColor2 = System.Drawing.Color.LightPink;
            appearance53.BackColorAlpha = Infragistics.Win.Alpha.UseAlphaLevel;
            appearance53.BackColorDisabled = System.Drawing.Color.LightPink;
            appearance53.BackColorDisabled2 = System.Drawing.Color.LightPink;
            appearance53.BackHatchStyle = Infragistics.Win.BackHatchStyle.Percent05;
            this.lblOlderRxs.Appearance = appearance53;
            this.lblOlderRxs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblOlderRxs.Location = new System.Drawing.Point(378, 3);
            this.lblOlderRxs.Name = "lblOlderRxs";
            this.lblOlderRxs.Size = new System.Drawing.Size(19, 19);
            this.lblOlderRxs.TabIndex = 10;
            // 
            // lblUnbilledRxs
            // 
            appearance51.BackColor = System.Drawing.Color.Red;
            appearance51.BackColor2 = System.Drawing.Color.Red;
            appearance51.BackColorAlpha = Infragistics.Win.Alpha.UseAlphaLevel;
            appearance51.BackColorDisabled = System.Drawing.Color.Red;
            appearance51.BackColorDisabled2 = System.Drawing.Color.Red;
            appearance51.BackHatchStyle = Infragistics.Win.BackHatchStyle.Percent05;
            this.lblUnbilledRxs.Appearance = appearance51;
            this.lblUnbilledRxs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblUnbilledRxs.Location = new System.Drawing.Point(128, 3);
            this.lblUnbilledRxs.Name = "lblUnbilledRxs";
            this.lblUnbilledRxs.Size = new System.Drawing.Size(19, 19);
            this.lblUnbilledRxs.TabIndex = 6;
            // 
            // lblUnbilledRxs1
            // 
            this.lblUnbilledRxs1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblUnbilledRxs1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblUnbilledRxs1.Location = new System.Drawing.Point(153, 3);
            this.lblUnbilledRxs1.Name = "lblUnbilledRxs1";
            this.lblUnbilledRxs1.Size = new System.Drawing.Size(94, 19);
            this.lblUnbilledRxs1.TabIndex = 7;
            this.lblUnbilledRxs1.Text = "Unbilled Rxs";
            // 
            // lblFiledRxs
            // 
            appearance52.BackColor = System.Drawing.Color.Blue;
            appearance52.BackColor2 = System.Drawing.Color.Blue;
            appearance52.BackColorAlpha = Infragistics.Win.Alpha.UseAlphaLevel;
            appearance52.BackColorDisabled = System.Drawing.Color.Blue;
            appearance52.BackColorDisabled2 = System.Drawing.Color.Blue;
            appearance52.BackHatchStyle = Infragistics.Win.BackHatchStyle.Percent05;
            this.lblFiledRxs.Appearance = appearance52;
            this.lblFiledRxs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblFiledRxs.Location = new System.Drawing.Point(253, 3);
            this.lblFiledRxs.Name = "lblFiledRxs";
            this.lblFiledRxs.Size = new System.Drawing.Size(19, 19);
            this.lblFiledRxs.TabIndex = 12;
            // 
            // lblFiledRxs1
            // 
            this.lblFiledRxs1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblFiledRxs1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblFiledRxs1.Location = new System.Drawing.Point(278, 3);
            this.lblFiledRxs1.Name = "lblFiledRxs1";
            this.lblFiledRxs1.Size = new System.Drawing.Size(94, 19);
            this.lblFiledRxs1.TabIndex = 13;
            this.lblFiledRxs1.Text = "Filed Rxs";
            // 
            // lblFamilyRxs
            // 
            appearance50.BackColor = System.Drawing.Color.Yellow;
            appearance50.BackColor2 = System.Drawing.Color.Yellow;
            appearance50.BackColorAlpha = Infragistics.Win.Alpha.UseAlphaLevel;
            appearance50.BackColorDisabled = System.Drawing.Color.Yellow;
            appearance50.BackColorDisabled2 = System.Drawing.Color.Yellow;
            appearance50.BackHatchStyle = Infragistics.Win.BackHatchStyle.Percent05;
            this.lblFamilyRxs.Appearance = appearance50;
            this.lblFamilyRxs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblFamilyRxs.Location = new System.Drawing.Point(3, 3);
            this.lblFamilyRxs.Name = "lblFamilyRxs";
            this.lblFamilyRxs.Size = new System.Drawing.Size(19, 19);
            this.lblFamilyRxs.TabIndex = 162;
            // 
            // lblFamilyRxs1
            // 
            this.lblFamilyRxs1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblFamilyRxs1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblFamilyRxs1.Location = new System.Drawing.Point(28, 3);
            this.lblFamilyRxs1.Name = "lblFamilyRxs1";
            this.lblFamilyRxs1.Size = new System.Drawing.Size(94, 19);
            this.lblFamilyRxs1.TabIndex = 163;
            this.lblFamilyRxs1.Text = "Family Rxs";
            // 
            // lblNonRPHVRxs1
            // 
            this.lblNonRPHVRxs1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblNonRPHVRxs1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblNonRPHVRxs1.Location = new System.Drawing.Point(528, 3);
            this.lblNonRPHVRxs1.Name = "lblNonRPHVRxs1";
            this.lblNonRPHVRxs1.Size = new System.Drawing.Size(96, 19);
            this.lblNonRPHVRxs1.TabIndex = 163;
            this.lblNonRPHVRxs1.Text = "Non-RPHV Rxs";
            // 
            // lblNonRPHVRxs
            // 
            appearance49.BackColor = System.Drawing.Color.Green;
            appearance49.BackColor2 = System.Drawing.Color.Green;
            appearance49.BackColorAlpha = Infragistics.Win.Alpha.UseAlphaLevel;
            appearance49.BackColorDisabled = System.Drawing.Color.Green;
            appearance49.BackColorDisabled2 = System.Drawing.Color.Green;
            appearance49.BackHatchStyle = Infragistics.Win.BackHatchStyle.Percent05;
            this.lblNonRPHVRxs.Appearance = appearance49;
            this.lblNonRPHVRxs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblNonRPHVRxs.Location = new System.Drawing.Point(503, 3);
            this.lblNonRPHVRxs.Name = "lblNonRPHVRxs";
            this.lblNonRPHVRxs.Size = new System.Drawing.Size(19, 19);
            this.lblNonRPHVRxs.TabIndex = 162;
            // 
            // frmPatientRXSearch
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
            this.BackColor = System.Drawing.Color.White;
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(1084, 564);
            this.Controls.Add(this.tlpSymbols);
            this.Controls.Add(this.tabMain);
            this.Controls.Add(this.btnCheckBoxStatus);
            this.Controls.Add(this.chkSelectAll);
            this.Controls.Add(this.btnEdit);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.sbMain);
            this.Controls.Add(this.grdSearch);
            this.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmPatientRXSearch";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Search Patient RX";
            this.Activated += new System.EventHandler(this.frmSearchMain_Activated);
            this.Load += new System.EventHandler(this.frmSearch_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmSearch_KeyDown);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.frmSearchMain_KeyUp);
            this.ultraTabPageControl1.ResumeLayout(false);
            this.ultraTabPageControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtPhone)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtpDOB)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtGender)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtAddress)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPatient)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.clTo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.clFrom)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.clTo1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.clFrom1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraDataSource1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdSearch)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sbMain)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tabMain)).EndInit();
            this.tabMain.ResumeLayout(false);
            this.tlpSymbols.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        private void btnSearch_Click(object sender, System.EventArgs e)
        {
            try
            {
                Search();
            }
            catch (Exception exp) { clsUIHelper.ShowErrorMsg(exp.Message); }
        }

        public void Search()
        {
            logger.Trace("Search() - " + clsPOSDBConstants.Log_Entering);
            SelectedData = Search(Convert.ToDateTime(this.clFrom.Value.ToString()), Convert.ToDateTime(this.clTo.Value.ToString()));
            //Added By Shitaljit for JIRA-920
            #region Logic to Remove Duplicate RX from the list
            DataTable dtUnpickedRX = SelectedData.Clone();
            foreach (DataRow dr in SelectedData.Rows)
            {
                if (Configuration.convertNullToInt64(dr["RXNO"]) == RXNo)
                {
                    if (Configuration.convertNullToInt(dr["NREFILL"]) != RefillNo)
                    {
                        dtUnpickedRX.ImportRow(dr);
                    }
                    else
                    {
                        if (SelectedData.Columns.Contains("PartialFillNo") && Configuration.convertNullToInt(dr["PartialFillNo"]) != PartialFillNo)
                        {
                            dtUnpickedRX.ImportRow(dr);
                        }
                    }
                }
                else
                {
                    dtUnpickedRX.ImportRow(dr);
                }
            }
            SelectedData.Clear();
            SelectedData = dtUnpickedRX;
            #endregion

            #region Sprint-25 - PRIMEPOS-2322 20-Apr-2017 JY Added
            if (Configuration.convertNullToInt(sFamilyID) > 0)  //PRIMEPOS-2816 25-Feb-2020 JY Added
            {
                if ((Configuration.CInfo.FetchFamilyRx == true) || (Configuration.CInfo.FetchFamilyRx == true && Configuration.CInfo.SearchRxsWithPatientName == true))
                {
                    DataTable dtTemp = SelectedData.Copy();
                    SelectedData.Clear();
                    if (dtTemp.Rows.Count > 0)
                    {
                        DataRow[] dr = dtTemp.Select("PATIENTNO=" + sPatientNo, "PatientName, DATEF, RXNO");
                        foreach (DataRow row in dr)
                        {
                            SelectedData.ImportRow(row);
                        }
                        dr = dtTemp.Select("PATIENTNO<>" + sPatientNo, "PatientName, DATEF, RXNO");
                        foreach (DataRow row in dr)
                        {
                            SelectedData.ImportRow(row);
                        }
                    }
                }
            }

            //if ((Configuration.CInfo.FetchFamilyRx == true || Configuration.CInfo.SearchRxsWithPatientName == true) && Configuration.convertNullToInt(sFamilyID) > 0)
            //{
            //    DataTable dtTemp = SelectedData.Copy();
            //    SelectedData.Clear();
            //    DataRow[] dr = dtTemp.Select("PATIENTNO=" + sPatientNo, "PatientName, DATEF, RXNO");
            //    foreach (DataRow row in dr)
            //    {
            //        SelectedData.ImportRow(row);
            //    }
            //    dr = dtTemp.Select("PATIENTNO<>" + sPatientNo, "PatientName, DATEF, RXNO");
            //    foreach (DataRow row in dr)
            //    {
            //        SelectedData.ImportRow(row);
            //    }
            //}
            #endregion

            grdSearch.DataSource = SelectedData;
            grdSearch.DisplayLayout.Bands[0].Columns[isUnBilled].Hidden = true;
            grdSearch.DisplayLayout.Bands[0].Columns[isFiled].Hidden = true;

            this.resizeColumns();
            grdSearch.Focus();
            grdSearch.PerformAction(UltraGridAction.FirstRowInGrid);
            grdSearch.Refresh();
            if (grdSearch.Rows.Count == 0)
            {
                this.chkSelectAll.Visible = false;
                this.btnCheckBoxStatus.Visible = false;
            }
            else
            {
                this.chkSelectAll.Visible = true;
                this.btnCheckBoxStatus.Visible = true;
                ChangeCheckButtonStatus(true);
            }
            if (Configuration.CInfo.PreventRxMaxFillDayNotifyAction > 0 && Configuration.CInfo.PreventRxMaxFillDayLimit > 0)
            {
                foreach (Infragistics.Win.UltraWinGrid.UltraGridRow orow in grdSearch.DisplayLayout.Rows)
                {
                    DateTime FillDate = DateTime.Now;
                    if (orow.Cells["DATEF"].Value != null)
                    {
                        FillDate = Convert.ToDateTime(orow.Cells["DATEF"].Value.ToString());
                    }
                    int filldayDiff = Configuration.convertNullToInt(((int)(DateTime.Now - FillDate).TotalDays).ToString());
                    if (Configuration.CInfo.PreventRxMaxFillDayLimit < filldayDiff)
                    {
                        orow.Appearance.ForeColor = Color.LightPink;    //PRIMEPOS-3158 04-Jan-2023 JY Modified
                    }
                }
            }

            #region PRIMEPOS-3158 04-Jan-2023 JY Added
            try
            {
                foreach (Infragistics.Win.UltraWinGrid.UltraGridRow orow in grdSearch.DisplayLayout.Rows)
                {
                    if (orow.Cells["Verified"].Value.ToString().ToUpper() != "V")
                    {
                        orow.Appearance.ForeColor = Color.Green;
                    }
                }
            }
            catch (Exception Ex)
            {
                logger.Fatal(Ex, "Search() - 1");
            }
            #endregion

            sbMain.Panels[0].Text = "Record(s) Count = " + grdSearch.Rows.Count;
        }

        public void SearchPrimeRx(string[] filterValues, bool isBatchDel = false)// PRIMERX-7688 - NileshJ added  bool isBatchDel=false 23-Sept-2019
        {
            isBatchDelivery = isBatchDel; // PRIMERX-7688 - NileshJ added  bool isBatchDel=false 23-Sept-2019
            SelectedData = Search(Convert.ToDateTime(this.clFrom.Value.ToString()), Convert.ToDateTime(this.clTo.Value.ToString()));
            DataRow drNew;

            filterValues = filterValues.Select(x => x.Replace("rx", "")).ToArray();
            //filterValues = filterValues.Replace("rx", "");
            DataTable tempSelectedData = new DataTable();
            tempSelectedData = SelectedData.Copy();
            var dv = new DataView(tempSelectedData);
            SelectedData.Rows.Clear();
            var filter = string.Empty;
            foreach (string Value in filterValues)
            {
                //string[] Values1 = Value.Split(',');
                //foreach (string Value1 in Values1)
                //{
                if (Value != String.Empty)
                {
                    filter = string.Join("','", Convert.ToInt32(Value));
                    dv.RowFilter = "RXNO IN (" + filter + ")";
                    SelectedData.Merge(dv.ToTable());
                    //dtPrimeRX.Merge()
                }
                //}
            }

            //var filter = string.Join("','", filterValues);
            //dv.RowFilter = "RXNO IN ('" + filter + "')";

            //dtPrimeRX = dv.ToTable();

            //Added By Shitaljit for JIRA-920
            #region Logic to Remove Duplicate RX from the list
            DataTable dtUnpickedRX = SelectedData.Clone();
            foreach (DataRow dr in SelectedData.Rows)
            {
                if (Configuration.convertNullToInt64(dr["RXNO"]) == RXNo)
                {
                    if (Configuration.convertNullToInt(dr["NREFILL"]) != RefillNo)
                    {
                        dtUnpickedRX.ImportRow(dr);
                    }
                }
                else
                {
                    dtUnpickedRX.ImportRow(dr);
                }
            }
            SelectedData.Clear();
            SelectedData = dtUnpickedRX;
            #endregion

            #region Sprint-25 - PRIMEPOS-2322 20-Apr-2017 JY Added
            if (Configuration.convertNullToInt(sFamilyID) > 0)  //PRIMEPOS-2816 25-Feb-2020 JY Added
            {
                if ((Configuration.CInfo.FetchFamilyRx == true) || (Configuration.CInfo.FetchFamilyRx == true && Configuration.CInfo.SearchRxsWithPatientName == true))
                {
                    DataTable dtTemp = SelectedData.Copy();
                    SelectedData.Clear();
                    DataRow[] dr = dtTemp.Select("PATIENTNO=" + sPatientNo, "PatientName, DATEF, RXNO");
                    foreach (DataRow row in dr)
                    {
                        SelectedData.ImportRow(row);
                    }
                    dr = dtTemp.Select("PATIENTNO<>" + sPatientNo, "PatientName, DATEF, RXNO");
                    foreach (DataRow row in dr)
                    {
                        SelectedData.ImportRow(row);
                    }
                }
            }
            //if ((Configuration.CInfo.FetchFamilyRx == true || Configuration.CInfo.SearchRxsWithPatientName == true) && Configuration.convertNullToInt(sFamilyID) > 0)
            //{
            //    DataTable dtTemp = SelectedData.Copy();
            //    SelectedData.Clear();
            //    DataRow[] dr = dtTemp.Select("PATIENTNO=" + sPatientNo, "PatientName, DATEF, RXNO");
            //    foreach (DataRow row in dr)
            //    {
            //        SelectedData.ImportRow(row);
            //    }
            //    dr = dtTemp.Select("PATIENTNO<>" + sPatientNo, "PatientName, DATEF, RXNO");
            //    foreach (DataRow row in dr)
            //    {
            //        SelectedData.ImportRow(row);
            //    }
            //}
            #endregion

            grdSearch.DataSource = SelectedData;
            grdSearch.DisplayLayout.Bands[0].Columns[isUnBilled].Hidden = true;
            grdSearch.DisplayLayout.Bands[0].Columns[isFiled].Hidden = true;

            this.resizeColumns();
            grdSearch.Focus();
            grdSearch.PerformAction(UltraGridAction.FirstRowInGrid);
            grdSearch.Refresh();
            if (grdSearch.Rows.Count == 0)
            {
                this.chkSelectAll.Visible = false;
                this.btnCheckBoxStatus.Visible = false;
            }
            else
            {
                this.chkSelectAll.Visible = true;
                this.btnCheckBoxStatus.Visible = true;
                ChangeCheckButtonStatus(true);
            }
            if (Configuration.CInfo.PreventRxMaxFillDayNotifyAction > 0 && Configuration.CInfo.PreventRxMaxFillDayLimit > 0)
            {
                foreach (Infragistics.Win.UltraWinGrid.UltraGridRow orow in grdSearch.DisplayLayout.Rows)
                {
                    DateTime FillDate = DateTime.Now;
                    if (orow.Cells["DATEF"].Value != null)
                    {
                        FillDate = Convert.ToDateTime(orow.Cells["DATEF"].Value.ToString());
                    }
                    int filldayDiff = Configuration.convertNullToInt(((int)(DateTime.Now - FillDate).TotalDays).ToString());
                    if (Configuration.CInfo.PreventRxMaxFillDayLimit < filldayDiff)
                    {
                        orow.Appearance.ForeColor = Color.LightPink;    //PRIMEPOS-3158 04-Jan-2023 JY Modified
                    }
                }
            }

            sbMain.Panels[0].Text = "Record(s) Count = " + grdSearch.Rows.Count;
            logger.Trace("Search() - " + clsPOSDBConstants.Log_Exiting);
        }

        private string GetFamilyPatientNos()
        {
            logger.Trace("GetFamilyPatientNos() - " + clsPOSDBConstants.Log_Entering);
            string sFamilyPatientNos = string.Empty;
            //Get all patient ids for selected patients family and get all unpicked Rxs for all
            DataTable oPatients = oPharmBL.GetPatientByFamilyID(Configuration.convertNullToInt(sFamilyID));
            if (oPatients != null && oPatients.Rows.Count > 0)
            {
                foreach (DataRow dr in oPatients.Rows)
                {
                    if (sFamilyPatientNos == string.Empty)
                        sFamilyPatientNos = Configuration.convertNullToString(dr["PATIENTNO"]);
                    else
                        sFamilyPatientNos += "," + Configuration.convertNullToString(dr["PATIENTNO"]);
                }
            }
            logger.Trace("GetFamilyPatientNos() - " + clsPOSDBConstants.Log_Exiting);
            return sFamilyPatientNos;
        }

        public DataTable Search(DateTime fromDate, DateTime toDate)
        {
            logger.Trace("Search(DateTime fromDate, DateTime toDate) - " + clsPOSDBConstants.Log_Entering);
            string sFamilyPatientNos = string.Empty;
            DataTable oData = new DataTable();
            //POS_Core.ErrorLogging.Logs.Logger("frmPatientRXSearch", "Search() - PatientRxSearch about to Call PHARMSQL", "");
            logger.Trace("Search(DateTime fromDate, DateTime toDate) - " + "about to Call PHARMSQL");
            PharmData.PharmBL oPharmBL = new PharmData.PharmBL();
            //if (cType == 'P' && (Configuration.CInfo.FetchFamilyRx == true || Configuration.CInfo.SearchRxsWithPatientName == true) && Configuration.convertNullToInt(sFamilyID) > 0) //PRIMEPOS-2816 25-Feb-2020 JY Commented
            //{
            //    sFamilyPatientNos = GetFamilyPatientNos();
            //}

            if (cType == 'P' && Configuration.convertNullToInt(sFamilyID) > 0)  //PRIMEPOS-2816 25-Feb-2020 JY Added
            {
                if ((Configuration.CInfo.FetchFamilyRx == true) || (Configuration.CInfo.FetchFamilyRx == true && Configuration.CInfo.SearchRxsWithPatientName == true))
                    sFamilyPatientNos = GetFamilyPatientNos();
            }
            if (sFamilyPatientNos != string.Empty)
                oData = oPharmBL.GetRxs(sFamilyPatientNos, fromDate, toDate, false, isBatchDelivery); //NileshJ - PRIMERX-7688 - Added IsBatchDelivery 23-Sept-2019
            else
            {
                if (cType == 'F')   //PRIMEPOS-2036 22-Jan-2019 JY Added for facility
                    oData = oPharmBL.GetRxs(sPatientNo, fromDate, toDate, false, cType);
                else
                    oData = oPharmBL.GetRxs(sPatientNo, fromDate, toDate, false, isBatchDelivery); //NileshJ - PRIMERX-7688 - Added IsBatchDelivery 23-Sept-2019
            }

            //POS_Core.ErrorLogging.Logs.Logger("frmPatientRXSearch", "Search() - PatientRxSearch Successfully Call PHARMSQL", "");
            logger.Trace("Search(DateTime fromDate, DateTime toDate) - " + "Successfully Call PHARMSQL");
            oData.Columns.Add("IsSelected", typeof(Boolean));
            oData.Columns.Add(isUnBilled, typeof(Boolean));
            oData.Columns.Add(isFiled, typeof(Boolean));

            foreach (DataRow oRow in oData.Rows)
            {
                oRow["IsSelected"] = true;
                oRow[isUnBilled] = false;
                oRow[isFiled] = false;
            }

            if (Configuration.CPOSSet.FetchUnbilledRx == 1 || Configuration.CPOSSet.FetchUnbilledRx == 2)   //PRIMEPOS-2398 04-Jan-2021 JY modified
            {
                DataTable dataUnbilled = new DataTable();
                if (sFamilyPatientNos != string.Empty)
                    dataUnbilled = oPharmBL.GetRxs(sFamilyPatientNos, fromDate, toDate, true);
                else
                {
                    if (cType == 'F')   //PRIMEPOS-2036 22-Jan-2019 JY Added for facility
                        dataUnbilled = oPharmBL.GetRxs(sPatientNo, fromDate, toDate, true, cType);
                    else
                        dataUnbilled = oPharmBL.GetRxs(sPatientNo, fromDate, toDate, true);
                }

                dataUnbilled.Columns.Add("IsSelected", typeof(Boolean));
                dataUnbilled.Columns.Add(isUnBilled, typeof(Boolean));
                dataUnbilled.Columns.Add(isFiled, typeof(Boolean));

                foreach (DataRow oRow in dataUnbilled.Rows)
                {
                    if (oData.Select("rxno=" + oRow["rxno"].ToString()).Length == 0)
                    {
                        oData.Rows.Add(oRow.ItemArray);
                        oData.Rows[oData.Rows.Count - 1][isUnBilled] = true;
                        oData.Rows[oData.Rows.Count - 1]["IsSelected"] = false;
                        oData.Rows[oData.Rows.Count - 1][isFiled] = false;
                    }
                }
            }

            foreach (DataRow oRow in oData.Rows)
            {

                DateTime FillDate = DateTime.Now;
                if (oRow["DATEF"] != null)
                {
                    FillDate = Convert.ToDateTime(oRow["DATEF"].ToString());
                }
                int filldayDiff = Configuration.convertNullToInt(((int)(DateTime.Now - FillDate).TotalDays).ToString());

                if (Configuration.CInfo.PreventRxMaxFillDayNotifyAction > 0 && Configuration.CInfo.PreventRxMaxFillDayLimit > 0 && Configuration.CInfo.PreventRxMaxFillDayLimit < filldayDiff)
                {
                    oRow["IsSelected"] = false; ;
                }
            }
            if (Configuration.CPOSSet.FetchFiledRx == true)
            {
                DataTable dataUnbilled = new DataTable();
                if (sFamilyPatientNos != string.Empty)
                    dataUnbilled = oPharmBL.GetRxs(sFamilyPatientNos, fromDate, toDate, "F", isBatchDelivery);//NileshJ - PRIMERX-7688 - Added IsBatchDelivery
                else
                {
                    if (cType == 'F')   //PRIMEPOS-2036 22-Jan-2019 JY Added for facility
                        dataUnbilled = oPharmBL.GetRxs(sPatientNo, fromDate, toDate, "F", cType);
                    else
                        dataUnbilled = oPharmBL.GetRxs(sPatientNo, fromDate, toDate, "F", isBatchDelivery);//NileshJ - PRIMERX-7688 - Added IsBatchDelivery
                }

                dataUnbilled.Columns.Add("IsSelected", typeof(Boolean));
                dataUnbilled.Columns.Add(isUnBilled, typeof(Boolean));
                dataUnbilled.Columns.Add(isFiled, typeof(Boolean));

                foreach (DataRow oRow in dataUnbilled.Rows)
                {
                    if (oData.Select("rxno=" + oRow["rxno"].ToString()).Length == 0)
                    {
                        oData.Rows.Add(oRow.ItemArray);
                        oData.Rows[oData.Rows.Count - 1][isUnBilled] = false;
                        oData.Rows[oData.Rows.Count - 1]["IsSelected"] = false;
                        oData.Rows[oData.Rows.Count - 1][isFiled] = true;
                    }
                }
            }
            //Commected by shitaljit on 17 Sept
            //as per the new requirement JIRA Ticket# 323. we should display the scaned RX in the list.
            RemovePickedRXs(oData);

            RemoveUnVerifiedRXs(oData);

            if (oData.Rows.Count > 0)   //Sprint-21 09-Feb-2016 JY Added
                RemoveHoldRXs(oData); //Added By SRT (SAchin) date : 08 Feb 2010
            logger.Trace("Search(DateTime fromDate, DateTime toDate) - " + clsPOSDBConstants.Log_Exiting);
            return oData;
        }

        private void RemovePickedRXs(DataTable oTable)
        {
            logger.Trace("RemovePickedRXs(DataTable oTable) - " + clsPOSDBConstants.Log_Entering);
            string sRXNo = string.Empty;
            string nRefill = string.Empty;
            for (int rowIndex = oTable.Rows.Count - 1; rowIndex >= 0; rowIndex--)
            {
                sRXNo = oTable.Rows[rowIndex]["RXNo"].ToString();
                nRefill = oTable.Rows[rowIndex]["nrefill"].ToString();
                int iPartialFillNo = 0;
                if (oTable.Columns.Contains("PartialFillNo"))
                    iPartialFillNo = Configuration.convertNullToInt(oTable.Rows[rowIndex]["PartialFillNo"]);

                if (POSTransaction.IsRXAlreadyProcessedInPOS(sRXNo, nRefill, iPartialFillNo) == true)
                {
                    oTable.Rows[rowIndex].Delete();
                }
                oTable.AcceptChanges();
            }
            string sPickedUp = string.Empty;
            if (!isBatchDelivery) // NileshJ - BatchDelivery - PRIMERX-7688 23-Sept-2019- Added just this codnition to ensure pickup condition is skipped for BatchDelivery.
            {
                if (Configuration.CPOSSet.AllowPickedUpRxToTrans == false)
                {
                    for (int rowIndex = oTable.Rows.Count - 1; rowIndex >= 0; rowIndex--)
                    {
                        sRXNo = oTable.Rows[rowIndex]["RXNo"].ToString();
                        nRefill = oTable.Rows[rowIndex]["nrefill"].ToString();
                        sPickedUp = Configuration.convertNullToString(oTable.Rows[rowIndex]["Pickedup"]);
                        if (sPickedUp.Equals("Y") == true)
                        {
                            oTable.Rows[rowIndex].Delete();
                        }
                        oTable.AcceptChanges();
                    }
                }
            }
            logger.Trace("RemovePickedRXs(DataTable oTable) - " + clsPOSDBConstants.Log_Exiting);
        }
        //Added By SRT (SAchin) date : 08 Feb 2010
        private void RemoveHoldRXs(DataTable oTable)
        {
            logger.Trace("RemoveHoldRXs(DataTable oTable) - " + clsPOSDBConstants.Log_Entering);
            POSTransaction oPOSTrans = new POSTransaction();

            #region Sprint-21 09-Feb-2016 JY commented incorrect logic
            //string StationID = string.Empty;
            //string UserID = String.Empty;
            //string TransID = string.Empty;//Added by shitaljit on 3 arpil 2012
            //foreach (RXDetail oDetail in oPatient.RXDetails)
            //{
            //    for (int i = 0; i < oTable.Rows.Count; i++)
            //    {
            //        if (oPOSTrans.RxIsOnHold(oTable.Rows[0]["RXNO"].ToString(), oTable.Rows[0]["PatientNo"].ToString(), out StationID, out UserID, out TransID))
            //        {
            //            oTable.Rows[i].Delete();
            //            //i--;
            //        }
            //        oTable.AcceptChanges();
            //    }
            //}
            #endregion

            #region Sprint-21 09-Feb-2016 JY Corrected unpicked rx logic - remove on-hold trans from it.
            DataTable dtRxOnHold = null;
            if (oPOSTrans.RxIsOnHold(oTable.Rows[0]["PatientNo"].ToString(), out dtRxOnHold))
            {
                foreach (DataRow oRow in dtRxOnHold.Rows)
                {
                    for (int i = oTable.Rows.Count - 1; i >= 0; i--)
                    {
                        if (oTable.Rows[i]["RxNo"].ToString() == oRow["RXNO"].ToString() && oTable.Rows[i]["NRefill"].ToString() == oRow["NRefill"].ToString())
                        {
                            oTable.Rows[i].Delete();
                            oTable.AcceptChanges();
                            break;
                        }
                    }
                }
            }
            #endregion

            logger.Trace("RemoveHoldRXs(DataTable oTable) - " + clsPOSDBConstants.Log_Exiting);
        }
        //end of Added By SRT (SAchin) date : 08 Feb 2010

        private void RemoveUnVerifiedRXs(DataTable oTable)
        {
            logger.Trace("RemoveUnVerifiedRXs(DataTable oTable) - " + clsPOSDBConstants.Log_Entering);
            if (Configuration.CInfo.AllowVerifiedRXOnly == 1)   //PRIMEPOS-2593 23-Jun-2020 JY modified
            {
                for (int i = oTable.Rows.Count - 1; i >= 0; i--)
                {
                    //if (oTable.Rows[i]["Verified"].ToString().ToUpper() != "V")   //PRIMEPOS-2789 04-Feb-2020 JY Commented
                    if (!(oTable.Rows[i]["Verified"].ToString().ToUpper() == "V" && Configuration.convertNullToInt(oTable.Rows[i]["VRFStage"]) == 2)) //PRIMEPOS-2789 04-Feb-2020 JY Added
                    {
                        oTable.Rows[i].Delete();
                        //i--;
                    }
                }
                oTable.AcceptChanges();
            }
            logger.Trace("RemoveUnVerifiedRXs(DataTable oTable) - " + clsPOSDBConstants.Log_Exiting);
        }

        private void btnCancel_Click(object sender, System.EventArgs e)
        {
            IsCanceled = true;
            this.Close();
        }


        private void frmSearch_Load(object sender, System.EventArgs e)
        {
            try
            {
                logger.Trace("frmSearch_Load(object sender, System.EventArgs e) - " + clsPOSDBConstants.Log_Entering);
                clsUIHelper.SetAppearance(this.grdSearch);
                clsUIHelper.SetReadonlyRow(this.grdSearch);

                this.txtPatient.AfterEnterEditMode += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
                this.txtPatient.AfterExitEditMode += new System.EventHandler(clsUIHelper.AfterExitEditMode);

                this.clFrom.Enter += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
                this.clFrom.Leave += new System.EventHandler(clsUIHelper.AfterExitEditMode);

                this.clTo.Enter += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
                this.clTo.Leave += new System.EventHandler(clsUIHelper.AfterExitEditMode);

                //Comneted By Shitaljit Search function is arleady calling from frmPOsTrancation.cs 
                //Notice search operation isd done 2 times .
                //Search();

                //Added By shitaljit on 17 Sept 2012 to directly add  unpicked RX(s) to Transaction
                if (Configuration.CInfo.AllowUnPickedRX == "2") // Allow unpicked RX(s) without verification.
                {
                    this.DialogResult = DialogResult.OK;

                    #region PRIMEPOS-2593 23-Jun-2020 JY Added
                    if (Configuration.CInfo.AllowVerifiedRXOnly == 2)
                    {
                        for (int i = 0; i < SelectedData.Rows.Count; i++)
                        {
                            if (!(SelectedData.Rows[i]["Verified"].ToString().ToUpper() == "V" && Configuration.convertNullToInt(SelectedData.Rows[i]["VRFStage"]) == 2))
                            {
                                SelectedData.Rows[i].Delete();
                            }
                        }
                        SelectedData.AcceptChanges();
                    }
                    #endregion

                    this.Close();
                    return;
                }
                if (this.grdSearch.Rows.Count == 0)
                {
                    this.clFrom.Focus();
                }
                else
                {
                    this.ActiveControl = this.grdSearch;
                }

                clsUIHelper.setColorSchecme(this);
                logger.Trace("frmSearch_Load(object sender, System.EventArgs e) - " + clsPOSDBConstants.Log_Exiting);
            }
            catch (Exception exp)
            {
                logger.Fatal(exp, "frmSearch_Load(object sender, System.EventArgs e)");
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }

        private void frmSearch_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            try
            {
                if (e.KeyData == System.Windows.Forms.Keys.Enter && this.ActiveControl.Name != "grdSearch")
                {
                    this.SelectNextControl(this.ActiveControl, true, true, true, true);
                }
            }
            catch (Exception exp)
            {
                logger.Fatal(exp, "frmSearch_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)");
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }

        private void btnClose_Click(object sender, System.EventArgs e)
        {
            this.Close();
        }

        private void grdSearch_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Space)
            {
                if (this.grdSearch.ActiveRow != null)
                {
                    this.grdSearch.ActiveRow.Cells["IsSelected"].Value = (Configuration.convertNullToBoolean(grdSearch.ActiveRow.Cells["IsSelected"].Value.ToString()) == false);
                }
            }
        }

        private void frmSearchMain_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (e.KeyData == System.Windows.Forms.Keys.F3)
            {
                btnEdit_Click(btnEdit, new EventArgs());
            }
            else if (e.KeyData == System.Windows.Forms.Keys.F4)
            {
                btnSearch_Click(btnSearch, new EventArgs());
            }
        }

        private void grdSearch_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
        {
            try
            {
                this.grdSearch.DisplayLayout.Bands[0].HeaderVisible = false;
                this.grdSearch.DisplayLayout.Bands[0].Columns["PatType"].Hidden = true;
                this.grdSearch.DisplayLayout.Bands[0].Columns["BillType"].Hidden = true;
                this.grdSearch.DisplayLayout.Bands[0].Columns["PresNo"].Hidden = true;
                //this.grdSearch.DisplayLayout.Bands[0].Columns["PatAmt"].Hidden = true;
                this.grdSearch.DisplayLayout.Bands[0].Columns["qty_ord"].Hidden = true;
                this.grdSearch.DisplayLayout.Bands[0].Columns["Days"].Hidden = true;
                this.grdSearch.DisplayLayout.Bands[0].Columns["siglines"].Hidden = true;

                this.grdSearch.DisplayLayout.Bands[0].Columns["units"].Hidden = true;
                this.grdSearch.DisplayLayout.Bands[0].Columns["patientno"].Hidden = true;
                this.grdSearch.DisplayLayout.Bands[0].Columns["Pickedup"].Hidden = true;
                this.grdSearch.DisplayLayout.Bands[0].Columns["Pickupdate"].Hidden = true;
                this.grdSearch.DisplayLayout.Bands[0].Columns["pickuptime"].Hidden = true;
                this.grdSearch.DisplayLayout.Bands[0].Columns["pickupfrom"].Hidden = true;

                this.grdSearch.DisplayLayout.Bands[0].Columns["Amount"].Hidden = true;
                this.grdSearch.DisplayLayout.Bands[0].Columns["copay"].Hidden = true;
                this.grdSearch.DisplayLayout.Bands[0].Columns["totamt"].Hidden = true;
                this.grdSearch.DisplayLayout.Bands[0].Columns["pfee"].Hidden = true;
                this.grdSearch.DisplayLayout.Bands[0].Columns["stax"].Hidden = true;
                this.grdSearch.DisplayLayout.Bands[0].Columns["discount"].Hidden = true;
                this.grdSearch.DisplayLayout.Bands[0].Columns["othfee"].Hidden = true;
                this.grdSearch.DisplayLayout.Bands[0].Columns["othamt"].Hidden = true;
                this.grdSearch.DisplayLayout.Bands[0].Columns["billedamt"].Hidden = true;
                this.grdSearch.DisplayLayout.Bands[0].Columns["UNC"].Hidden = true;
                this.grdSearch.DisplayLayout.Bands[0].Columns["DETDRGName"].Hidden = true;
                this.grdSearch.DisplayLayout.Bands[0].Columns["PickupPOS"].Hidden = true;
                this.grdSearch.DisplayLayout.Bands[0].Columns["DateO"].Hidden = true;
                this.grdSearch.DisplayLayout.Bands[0].Columns["NDC"].Hidden = true;
                //this.grdSearch.DisplayLayout.Bands[0].Columns["verified"].Hidden = true;  //PRIMEPOS-2330 17-Jan-2019 JY Commented
                this.grdSearch.DisplayLayout.Bands[0].Columns["FamilyID"].Hidden = true;    //Sprint-25 - PRIMEPOS-2322 02-Feb-2017 JY Added

                this.grdSearch.DisplayLayout.Bands[0].Columns["isSelected"].Header.SetVisiblePosition(0, false);
                this.grdSearch.DisplayLayout.Bands[0].Columns["isSelected"].Header.Caption = "";

                this.grdSearch.DisplayLayout.Bands[0].Columns["RXNo"].Header.SetVisiblePosition(1, false);
                this.grdSearch.DisplayLayout.Bands[0].Columns["RXNo"].Header.Caption = "RX No";

                this.grdSearch.DisplayLayout.Bands[0].Columns["PatientName"].Header.SetVisiblePosition(2, false);   //Sprint-25 - PRIMEPOS-2322 31-Jan-2017 JY Added patient name
                this.grdSearch.DisplayLayout.Bands[0].Columns["PatientName"].Header.Caption = "Patient Name";    //Sprint-25 - PRIMEPOS-2322 31-Jan-2017 JY Added patient name

                this.grdSearch.DisplayLayout.Bands[0].Columns["Trefills"].Header.Caption = "Auth Ref";
                this.grdSearch.DisplayLayout.Bands[0].Columns["Trefills"].Header.SetVisiblePosition(3, false);

                this.grdSearch.DisplayLayout.Bands[0].Columns["Nrefill"].Header.Caption = "Ref #";
                this.grdSearch.DisplayLayout.Bands[0].Columns["Nrefill"].Header.SetVisiblePosition(4, false);

                this.grdSearch.DisplayLayout.Bands[0].Columns["DateF"].Header.Caption = "Date Filled";
                this.grdSearch.DisplayLayout.Bands[0].Columns["DateF"].Header.SetVisiblePosition(5, false);

                //this.grdSearch.DisplayLayout.Bands[0].Columns["NDC"].Header.Caption = "Drug NDC";
                //this.grdSearch.DisplayLayout.Bands[0].Columns["NDC"].Header.SetVisiblePosition(5, false);

                this.grdSearch.DisplayLayout.Bands[0].Columns["DrgName"].Header.Caption = "Drug Name";
                this.grdSearch.DisplayLayout.Bands[0].Columns["DrgName"].Header.SetVisiblePosition(6, false);

                this.grdSearch.DisplayLayout.Bands[0].Columns["Strong"].Header.Caption = "Strength";
                this.grdSearch.DisplayLayout.Bands[0].Columns["Strong"].Header.SetVisiblePosition(7, false);

                this.grdSearch.DisplayLayout.Bands[0].Columns["Form"].Header.Caption = "Form";
                this.grdSearch.DisplayLayout.Bands[0].Columns["Form"].Header.SetVisiblePosition(8, false);

                this.grdSearch.DisplayLayout.Bands[0].Columns["Quant"].Header.Caption = "Qty";
                this.grdSearch.DisplayLayout.Bands[0].Columns["Quant"].Header.SetVisiblePosition(9, false);
                this.grdSearch.DisplayLayout.Bands[0].Columns["Quant"].Format = "######0";

                this.grdSearch.DisplayLayout.Bands[0].Columns["PatAMT"].Header.Caption = "Pat. Copay";
                this.grdSearch.DisplayLayout.Bands[0].Columns["PatAMT"].Header.SetVisiblePosition(10, false);
                this.grdSearch.DisplayLayout.Bands[0].Columns["PatAMT"].Format = "######0.00";

                this.grdSearch.DisplayLayout.Bands[0].Columns["Status"].Header.SetVisiblePosition(11, false);   //PRIMEPOS-2330 17-Jan-2019 JY Added
                this.grdSearch.DisplayLayout.Bands[0].Columns["verified"].Header.SetVisiblePosition(12, false); //PRIMEPOS-2330 17-Jan-2019 JY Added
                this.grdSearch.DisplayLayout.Bands[0].Columns["VRFStage"].Header.SetVisiblePosition(13, false); //PRIMEPOS-2789 04-Feb-2020 JY Added
            }
            catch (Exception) { }
        }

        private void frmSearchMain_Activated(object sender, System.EventArgs e)
        {
            clsUIHelper.CurrentForm = this;
        }

        private void grdSearch_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            this.CurrentX = e.X;
            this.CurrentY = e.Y;
        }

        private void TextBoxKeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            try
            {
                if (e.KeyData == Keys.Down)
                {
                    if (this.grdSearch.Rows.Count > 0)
                    {
                        this.grdSearch.Focus();
                        this.grdSearch.ActiveRow = this.grdSearch.Rows[0];
                    }
                }
            }
            catch (Exception) { }
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

        private void grdSearch_BeforeRowExpanded(object sender, Infragistics.Win.UltraWinGrid.CancelableRowEventArgs e)
        {
            foreach (Infragistics.Win.UltraWinGrid.UltraGridRow orow in grdSearch.DisplayLayout.Rows)
            {
                if (orow.Expanded == true)
                    orow.CollapseAll();
            }
            e.Row.Activate();
        }

        private void clTo_BeforeDropDown(object sender, System.ComponentModel.CancelEventArgs e)
        {

        }

        private void lbl2_Click(object sender, System.EventArgs e)
        {

        }

        private void grdSearch_InitializeRow(object sender, InitializeRowEventArgs e)
        {
            if (Configuration.convertNullToBoolean(e.Row.Cells[isUnBilled].Value) == true || Configuration.convertNullToBoolean(e.Row.Cells[isFiled].Value) == true)
            {
                e.Row.Activation = Activation.NoEdit;
                e.Row.Activation = Activation.Disabled;
                if (Configuration.convertNullToBoolean(e.Row.Cells[isUnBilled].Value) == true)
                {
                    e.Row.Appearance.ForeColor = Color.Red; //PRIMEPOS-3158 04-Jan-2023 JY Modified
                    e.Row.Appearance.ForeColorDisabled = Color.Red; //PRIMEPOS-3158 04-Jan-2023 JY Modified
                }
                else if (Configuration.convertNullToBoolean(e.Row.Cells[isFiled].Value) == true)
                {
                    e.Row.Appearance.ForeColor = Color.Blue;
                    e.Row.Appearance.ForeColorDisabled = Color.Blue;
                }
            }

            #region Sprint-25 - PRIMEPOS-2322 20-Mar-2017 JY Added
            //if ((Configuration.CInfo.FetchFamilyRx == true || Configuration.CInfo.SearchRxsWithPatientName == true) && Configuration.convertNullToInt(sFamilyID) > 0)
            //{
            //    if (sPatientNo != Configuration.convertNullToInt(e.Row.Cells["PATIENTNO"].Value).ToString())
            //        e.Row.Cells["PatientName"].Appearance.BackColor = Color.Yellow;
            //}
            if (Configuration.convertNullToInt(sFamilyID) > 0)
            {
                if ((Configuration.CInfo.FetchFamilyRx == true) || (Configuration.CInfo.FetchFamilyRx == true && Configuration.CInfo.SearchRxsWithPatientName == true))
                {
                    if (sPatientNo != Configuration.convertNullToInt(e.Row.Cells["PATIENTNO"].Value).ToString())
                        e.Row.Cells["PatientName"].Appearance.BackColor = Color.Yellow;
                }
            }
            #endregion

            #region PRIMEPOS-2593 23-Jun-2020 JY Added
            try
            {
                if (Configuration.CInfo.AllowVerifiedRXOnly == 2)
                {
                    if (!(e.Row.Cells["Verified"].Value.ToString().ToUpper() == "V" && Configuration.convertNullToInt(e.Row.Cells["VRFStage"].Value) == 2))
                    {
                        e.Row.Activation = Activation.NoEdit;
                        e.Row.Activation = Activation.Disabled;
                        e.Row.Cells["IsSelected"].Value = false;
                    }
                }
            }
            catch { }
            #endregion
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < this.SelectedData.Rows.Count; i++)
            {
                if (Configuration.convertNullToBoolean(SelectedData.Rows[i]["IsSelected"].ToString()) == false)
                {
                    SelectedData.Rows.Remove(SelectedData.Rows[i]);
                    i--;
                }
            }
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void grdSearch_MouseClick(object sender, MouseEventArgs e)
        {
            logger.Trace("grdSearch_MouseClick(object sender, MouseEventArgs e) - " + clsPOSDBConstants.Log_Entering);
            UIElement element = grdSearch.DisplayLayout.UIElement.ElementFromPoint(e.Location);
            UltraGridCell cell = element.GetContext(typeof(UltraGridCell)) as UltraGridCell;
            bool isSelected = false;
            UltraGridRow row = grdSearch.DisplayLayout.ActiveRow;
            if (row.Activation == Activation.Disabled) return;  //PRIMEPOS-2593 23-Jun-2020 JY Added
            if (row == null) return;    //PRIMEPOS-2688 17-May-2019 JY Added
            try
            {
                if (element is CheckIndicatorUIElement)
                {
                    isSelected = Configuration.convertNullToBoolean(cell.Value.ToString());
                }
                DateTime FillDate = DateTime.Now;
                if (row.Cells["DATEF"].Value != null)
                {
                    FillDate = Convert.ToDateTime(row.Cells["DATEF"].Value.ToString());
                }
                int filldayDiff = Configuration.convertNullToInt(((int)(DateTime.Now - FillDate).TotalDays).ToString());

                if (element is CheckIndicatorUIElement && Configuration.CInfo.PreventRxMaxFillDayNotifyAction > 0 && Configuration.CInfo.PreventRxMaxFillDayLimit > 0 && Configuration.CInfo.PreventRxMaxFillDayLimit < filldayDiff)
                {
                    string excepMessage = "";

                    DialogResult diaRes;
                    if (Configuration.CInfo.PreventRxMaxFillDayNotifyAction == 1 && !isSelected)
                    {
                        excepMessage = "RX " + row.Cells["RXNO"].Value.ToString() + " older than " + Configuration.CInfo.PreventRxMaxFillDayLimit + " days. Would you like to continue?";
                        diaRes = Resources.Message.Display(excepMessage, Configuration.ApplicationName, MessageBoxButtons.YesNo, MessageBoxIcon.Information, MessageBoxDefaultButton.Button2);
                        if (diaRes != DialogResult.Yes)
                        {
                            cell.Value = false;
                        }
                        else
                        {
                            cell.Value = true;
                        }
                    }
                    else if (!isSelected)
                    {
                        excepMessage = "RX " + row.Cells["RXNO"].Value.ToString() + " older than " + Configuration.CInfo.PreventRxMaxFillDayLimit + " days not permitted";
                        diaRes = Resources.Message.Display(excepMessage, Configuration.ApplicationName, MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button2);
                        cell.Value = false;
                    }
                    else
                    {
                        cell.Value = false;
                    }
                }
                else if (element is CheckIndicatorUIElement)
                {
                    cell.Value = !(Configuration.convertNullToBoolean(cell.Value.ToString()));
                }
                logger.Trace("grdSearch_MouseClick(object sender, MouseEventArgs e) - " + clsPOSDBConstants.Log_Exiting);
            }
            catch (Exception Ex)
            {
                string cellValue = string.Empty;
                try
                {
                    cellValue = cell.Value == null ? "" : cell.Value.ToString();
                }
                catch
                {
                    cellValue = "Error";
                }
                string strMessage = "Column Name :" + cell.Column.Header.Caption + ", Value : " + cellValue + ", RxNo : " + row.Cells["RxNo"].Value + ", nRefill : " + row.Cells["NREFILL"].Value;
                logger.Fatal(Ex, "grdSearch_MouseClick(object sender, MouseEventArgs e) - " + strMessage);
            }


        }

        private void chkSelectAll_CheckedChanged(object sender, EventArgs e)
        {
            ChangeGridRowSelection(chkSelectAll.Checked);
        }
        //Added By Dharmendra on Apr-29-09
        //to set the focus on OK button after leaving grid
        private void grdSearch_Leave(object sender, EventArgs e)
        {
            this.btnEdit.Focus();
        }
        //Added Till Here

        private void ChangeCheckButtonStatus(bool CheckAll)
        {
            if (CheckAll == true)
            {
                this.btnCheckBoxStatus.Text = buttonStatusUnCheck;
            }
            else
            {
                this.btnCheckBoxStatus.Text = buttonStatusCheck;
            }
        }

        private void btnCheckBoxStatus_Click(object sender, EventArgs e)
        {
            if (btnCheckBoxStatus.Text == buttonStatusUnCheck)
            {
                ChangeGridRowSelection(false);
            }
            else
            {
                ChangeGridRowSelection(true);
            }
        }

        private void ChangeGridRowSelection(bool bSelectAll)
        {
            foreach (Infragistics.Win.UltraWinGrid.UltraGridRow orow in grdSearch.DisplayLayout.Rows)
            {
                if (Configuration.convertNullToBoolean(orow.Cells[isUnBilled].Value) == false || Configuration.convertNullToBoolean(orow.Cells[isFiled].Value) == false)
                {
                    orow.Cells["IsSelected"].Value = bSelectAll;
                }
            }

            chkSelectAll.Checked = bSelectAll;
            ChangeCheckButtonStatus(bSelectAll);
        }

        private void grdSearch_BeforeCellUpdate(object sender, BeforeCellUpdateEventArgs e)
        {
            if (e.Cell.Column.Key == "IsSelected" && (Configuration.convertNullToBoolean(e.Cell.Row.Cells[isUnBilled].Value) == true || Configuration.convertNullToBoolean(e.Cell.Row.Cells[isFiled].Value) == true))
            {
                e.Cancel = true;
            }
        }
    }
}
