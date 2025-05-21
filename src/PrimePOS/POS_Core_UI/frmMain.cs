using System;
using Microsoft.Win32;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Reflection;
using POS_Core.Resources;
using POS_Core.CommonData;
using POS_Core.DataAccess;
using POS_Core.BusinessRules;
using System.Threading;
using POS_Core.CommonData.Rows;
using POS_Core.CommonData.Tables;
using Infragistics.Win.UltraWinToolbars;
using System.Diagnostics; //Added by SRT
using System.Collections.Generic; //Added by SRT
using POS_Core.ErrorLogging; //Sprint-22 - PRIMEPOS-2245 27-Oct-2015 JY Added
using Xceed.Ftp;    //Sprint-24 - PRIMEPOS-2344 05-Dec-2016 JY Added
using System.IO;    //Sprint-24 - PRIMEPOS-2344 05-Dec-2016 JY Added
using System.Net;   //Sprint-24 - PRIMEPOS-2344 06-Dec-2016 JY Added
using System.Data;  //Sprint-24 - PRIMEPOS-2344 06-Dec-2016 JY Added
using System.Data.OleDb;    //Sprint-24 - PRIMEPOS-2344 06-Dec-2016 JY Added
using System.Threading.Tasks;
//using Resources;
using POS_Core.Resources.DelegateHandler;
using Resources;
using POS_Core_UI.Reports.ReportsUI;
using POS_Core.TransType;
using POS_Core_UI.UI;
using POS_Core_UI.Resources;
using POS_Core_UI.UserManagement;
using POS_Core.Resources.PaymentHandler;
using NLog; //PRIMEPOS-2641 14-Feb-2019 JY Added
using Evertech;//Added by Arvind PRIMEPOS-2664
using Evertech.Data;
using System.Data.SqlClient;
using System.Drawing.Imaging;
using PharmData;
using System.Linq;
using System.Timers;
using PossqlData;
using NBS; //PRIMEPOS-3372
using NBS.ResponseModels; //PRIMEPOS-3372

namespace POS_Core_UI
{
    /// <summary>
    /// Summary description for frmMain.
    /// </summary>
    public class frmMain : System.Windows.Forms.Form
    {
        #region variables

        public Messaging.Messaging oMessaging;
        private static frmNumericPad oNumericPad = null;
        public static frmNumericPad oPublicNumericPad = null;

        private string screenNamePOAck = string.Empty;
        private string screenNameViewPO = string.Empty;

        private static bool IsFromPanel = false;

        private InactivityMonitor.HookMonitor inactivityMonitor = null;

        private static frmRptTimesheet oRptTimesheet = new frmRptTimesheet();
        private static frmRptROATransDetails ofrmRptROA = new frmRptROATransDetails();

        private static bool m_threadStarted = false;
        private static frmRptVendor oRptVendor = new frmRptVendor();
        private static frmRptItemReOrder oRptItemReOrder = new frmRptItemReOrder(false);
        private static frmRptCostAnalysis oRptCostAnalysis = new frmRptCostAnalysis();
        private static frmRptDeadStockReport oRptDeadStockReport = new frmRptDeadStockReport();
        private static frmRptItemFileListing oRptPhysicalInventorySheet;
        private static frmCustomerNotes oFrmcustNote = new frmCustomerNotes("SYSTEM", "", clsEntityType.SystemNote);//Added by Krishna on 10 October 2011
        private static frmCustomerNotes oFrmCustomerNote = new frmCustomerNotes();//Added by Shitaljit(QuicSolv) on 10 October 2011
        private static frmRptItemFileListing oRptInventoryStatusReport;
        private static frmItemAdvSearch oFrmAdvSrchItem = new frmItemAdvSearch();//Added by Krishna on 29 November 2011

        private static frmRptItemPriceLog oRptItemPriceLog;

        //Added by SRT(Sachin) Date : 27 Nov 2009
        private static frmRptItemPriceLogLable oRptItemPriceLogLable;
        //End of Added by SRT(Sachin) Date : 27 Nov 2009

        private static frmRptItemFileListing oRptItemFileListing;

        private static frmRptInventoryReceived oRptInventoryReceived;
        private static frmRptInventoryReceived oRptItemListing;

        private static Form selectedForm = null;
        private static ComPort oComPort = new ComPort();


        private static int explorerBarWidth = 0;
        private static int MenuBarHeight = 0;
        private static int statusBarVendorHeight = 0;
        private static int statusBarHeight = 0;


        private clsLogin m_oLogin = new clsLogin();
        private Infragistics.Win.UltraWinStatusBar.UltraStatusBar ultraStatusBar1;
        public Infragistics.Win.UltraWinExplorerBar.UltraExplorerBar ultraExplorerBar1;
        public Infragistics.Win.UltraWinStatusBar.UltraStatusPanel ultraOnHoldStatusPanel;//2915
        private System.ComponentModel.IContainer components;
        public Infragistics.Win.UltraWinStatusBar.UltraStatusBar ultraStatusBar;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.ImageList imageList2;

        public LookandFeelStypeENUM Selectedstyle;
        private System.Windows.Forms.Splitter splitter1;
        public Infragistics.Win.UltraWinToolbars.UltraToolbarsManager ultMenuBar;
        private Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _frmMain_Toolbars_Dock_Area_Top;
        private Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _frmMain_Toolbars_Dock_Area_Bottom;
        private Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _frmMain_Toolbars_Dock_Area_Left;
        private Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _frmMain_Toolbars_Dock_Area_Right;
        public Infragistics.Win.UltraWinStatusBar.UltraStatusBar ultraStatusBarVendor;
        //public Infragistics.Win.UltraWinStatusBar.utr 
        private static frmMain ofrmMain;
        //Added By Dharmendra SRT on Jan-20-09
        //This variable is used to hold the value of the tag FORCE_FSA from app.config file;
        public static string sForceFSA = string.Empty;
        //public static string sForceFSA = string.Empty;
        private ContextMenuStrip cntMenuPOStatus;
        private ToolStripMenuItem Expired;
        private ToolStripMenuItem Error;
        private ToolStripMenuItem Overdue;
        private ToolStripMenuItem Acknowledged;
        private ToolStripMenuItem DeliveryReceived; // Added by atul 25-oct-2010

        //Added Till Here Jan-20-09
        //Added By Dharmendra (SRT) on Apr-11-09
        bool poBarDisplayStatus = true; //holds the display status value of po status bar
        bool usePrimePO = true;
        private List<string> POStatusList = new List<string>();
        private const string POSTQUEUED = "QUEUED";
        private const string POSTSUBMITTED = "SUBMITTED";
        private const string POSTEXPIRED = "EXPIRED";
        private const string POSTERROR = "ERROR";
        private ToolStripMenuItem maxAttemptToolStripMenuItem;
        private const string POSTOVERDUE = "OVERDUE";
        private const string POSTMAXATTEMPTS = "MAXATTEMPTS";
        private ToolStripMenuItem InComplete;
        private const string POSTACKNOWLEDGED = "ACKNOWLEDGED";
        private const string POSTINCOMPLETE = "INCOMPLETE";
        //Added Till Here Apr-11-09
        private const string POSTDELIVERYRECEIVED = "DELIVERYRECEIVED"; //added by atul 25-oct-2010
        private static frmRptPriceOverridden oRptPriceOverridden;
        private static frmRptItemDepartment oRptItemChangeDepartment;
        //Added By Shitaljit(QuicSolv) on May  2011
        private static frmSetSellingPrice oRptSetSellingPrice;
        private static frmColorSchemeForViewPOSTrans oViewPOSTransColorScheme = new frmColorSchemeForViewPOSTrans();
        private static frmPayTypes ofrmPayTyes;

        private static Thread PrimeEDIConnectionThread;
        //Till here added By Shitaljit

        private static ILogger logger = LogManager.GetCurrentClassLogger(); //PRIMEPOS-2641 14-Feb-2019 JY Added
        #endregion

        #region Sprint-20 25-May-2015 JY Added for Auto updater
        private System.Windows.Forms.Timer tmrAutuUpdate;
        private System.Timers.Timer tmrPrimeRxPayBGStatusUpdate;//PRIMEPOS-3187
        bool brunSchoudule = false;
        static string strAppName = "POS";
        List<MMSAppUpdater.clsAppUpdate> oApps = null;

        private BackgroundWorker bwCheckAlwaysRunning;
        private System.Windows.Forms.Timer tmrCheckRunningTasks;

        MMSAppUpdater.clsUpdateManager oAppUpdater = new MMSAppUpdater.clsUpdateManager(POS_Core.Resources.Configuration.CInfo.AutoUpdateServiceAddress, POS_Core.Resources.Configuration.CInfo.StoreID, POS_Core.Resources.Configuration.ConnectionString, strAppName, POS_Core.Resources.Configuration.StationID, true);
        //FOR TESTING setting last parameter "false" to download updates immediately
        //MMSAppUpdater.clsUpdateManager oAppUpdater = new MMSAppUpdater.clsUpdateManager(Configuration.CInfo.AutoUpdateServiceAddress, Configuration.CInfo.StoreID, Configuration.ConnectionString, strAppName, Configuration.StationID, false);
        #endregion

        private static int MsgCnt = 0;  //19-Nov-2015 JY Added

        private const string constIIASFileType = "IIAS";    //Sprint-25 - PRIMEPOS-2379 09-Feb-2017 JY Added
        private static bool bIIASFileDownloadComplete = false;  //Sprint-24 - PRIMEPOS-2344 05-Dec-2016 JY Added
        private static long bIIASFileSize = 0; //PRIMEPOS-3228

        private const string constPSEFileType = "PSE";  //Sprint-25 - PRIMEPOS-2379 09-Feb-2017 JY Added
        private static bool bPSEFileDownloadComplete = false;  //Sprint-25 - PRIMEPOS-2379 09-Feb-2017 JY Added
        private static long bPSEFileSize = 0; //PRIMEPOS-3228

        private static frmRptCouponReport ofrmRptCouponReport = new frmRptCouponReport();   //PRIMEPOS-2034 07-Mar-2018 JY Added
        public static int TransactionRecordCount = 0;  //PRIMEPOS-1923 08-Aug-2018 JY Added

        EvertechProcessor device = null;//PRIMEPOS-2664
        private static frmReconciliationDeliveryReport ofrmReconciliationDeliveryReport = new frmReconciliationDeliveryReport();    //PRIMERX-7688 NileshJ
        private static bool bFileUploadComplete = false;    //PRIMEPOS-2774 14-Jan-2020 JY Added

        private SettleTxnResponse EvertecSettleResponse = null;//PRIMEPOS-2664
        private static frmSetItemTax ofrmSetItemTax;    //PRIMEPOS-1633 28-Dec-2020 JY Added
        private static frmTaxOverrideReport ofrmTaxOverrideReport;  //PRIMEPOS-2391 23-Jul-2021 JY Added
        private PictureBox pictSign;
        private static frmInvShrinkage ofrmInvShrinkage = new frmInvShrinkage();    //PRIMEPOS-138 14-Feb-2021 JY Added

        protected virtual bool SaveSettings()
        {
            return true;
        }

        public static bool FromPanel
        {
            set { frmMain.IsFromPanel = value; }
            get { return frmMain.IsFromPanel; }
        }

        private void setLookFeelStyle(LookandFeelStypeENUM style)
        {

            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            ultraExplorerBar1.Tag = style;
            Selectedstyle = style;

            switch (style)
            {
                case LookandFeelStypeENUM.OfficeXP:
                    ultMenuBar.Style = Infragistics.Win.UltraWinToolbars.ToolbarStyle.OfficeXP;
                    ultMenuBar.Appearance.BackColor = System.Drawing.SystemColors.Control;
                    ultMenuBar.Appearance.BackColor2 = System.Drawing.Color.White;
                    ultraExplorerBar1.ViewStyle = Infragistics.Win.UltraWinExplorerBar.UltraExplorerBarViewStyle.XP;
                    //udmMain.WindowStyle = Infragistics.Win.UltraWinDock.WindowStyle.Office2003;
                    break;
                case LookandFeelStypeENUM.Office2000:
                    ultMenuBar.Style = Infragistics.Win.UltraWinToolbars.ToolbarStyle.Office2000;
                    ultMenuBar.Appearance.BackColor = System.Drawing.SystemColors.Control;
                    ultMenuBar.Appearance.BackColor2 = System.Drawing.Color.White;
                    ultraExplorerBar1.ViewStyle = Infragistics.Win.UltraWinExplorerBar.UltraExplorerBarViewStyle.Office2000;
                    //udmMain.WindowStyle = Infragistics.Win.UltraWinDock.WindowStyle.VC6;
                    break;
                case LookandFeelStypeENUM.Office2003:
                    ultMenuBar.Style = Infragistics.Win.UltraWinToolbars.ToolbarStyle.Office2003;
                    ultMenuBar.Appearance = appearance1;
                    ultraExplorerBar1.ViewStyle = Infragistics.Win.UltraWinExplorerBar.UltraExplorerBarViewStyle.Office2003;
                    //udmMain.WindowStyle = Infragistics.Win.UltraWinDock.WindowStyle.Office2003;
                    break;
                case LookandFeelStypeENUM.VisualStudio2005:
                    ultMenuBar.Style = Infragistics.Win.UltraWinToolbars.ToolbarStyle.VisualStudio2005;
                    ultMenuBar.Appearance = appearance1;
                    ultraExplorerBar1.ViewStyle = Infragistics.Win.UltraWinExplorerBar.UltraExplorerBarViewStyle.VisualStudio2005;
                    //udmMain.WindowStyle = Infragistics.Win.UltraWinDock.WindowStyle.VisualStudio2005;
                    break;
            }
        }

        private static void SetFormAtCenter(Form frm)
        {
            frm.MdiParent = ofrmMain;

            if (ofrmMain.ActiveMdiChild != null)
                if (ofrmMain.ActiveMdiChild.WindowState == FormWindowState.Maximized)
                    ofrmMain.ActiveMdiChild.WindowState = FormWindowState.Normal;

            int formLeft = (ofrmMain.DisplayRectangle.Width - explorerBarWidth - frm.Width) / 2;
            int formtop = 0;

            if (POS_Core.Resources.Configuration.CPrimeEDISetting.UsePrimePO)    //PRIMEPOS-3167 07-Nov-2022 JY Modified
                formtop = (ofrmMain.DisplayRectangle.Height - frm.Height - MenuBarHeight - statusBarVendorHeight - statusBarHeight) / 2;
            else
                formtop = (ofrmMain.DisplayRectangle.Height - frm.Height - MenuBarHeight - statusBarHeight) / 2;

            if (formLeft < 0) formLeft = 0;
            if (formtop < 0) formtop = 0;

            frm.StartPosition = FormStartPosition.Manual;

            frm.Left = formLeft;
            frm.Top = formtop;
            frm.BringToFront();

            //Added BY SRT(Abhishek) Date : 02/05/2009  
            if (frm.Name == POSMenuItems.FormPOLogScreen && frmMain.FromPanel == false)
                frm.Hide();
            else
                frm.Show();
            //End of Added BY SRT(Abhishek) Date : 02/05/2009 
        }

        public static frmRptVendor RptVendor
        {
            get
            {
                if (oRptVendor.IsDisposed)
                    oRptVendor = new frmRptVendor();
                return oRptVendor;
            }
        }
        //Added By shitaljit
        public static frmColorSchemeForViewPOSTrans ViewPOSTransColorScheme
        {
            get
            {
                if (oViewPOSTransColorScheme.IsDisposed)
                    oViewPOSTransColorScheme = new frmColorSchemeForViewPOSTrans();
                return oViewPOSTransColorScheme;
            }
        }

        public static ComPort PoleDisplay
        {
            get
            {
                if (oComPort.IsDisposed)
                    oComPort = new ComPort();
                return oComPort;
            }
        }


        public static frmRptCostAnalysis RptCostAnalysis
        {
            get
            {
                if (oRptCostAnalysis.IsDisposed)
                    oRptCostAnalysis = new frmRptCostAnalysis();
                return oRptCostAnalysis;
            }

        }
        public static frmRptDeadStockReport RptDeadStockReport
        {
            get
            {
                if (oRptDeadStockReport.IsDisposed)
                    oRptDeadStockReport = new frmRptDeadStockReport();
                return oRptDeadStockReport;
            }
        }

        public static frmRptItemReOrder RptItemReOrder
        {
            get
            {
                if (oRptItemReOrder.IsDisposed)
                    oRptItemReOrder = new frmRptItemReOrder(false);
                return oRptItemReOrder;
            }
        }

        public static frmRptItemFileListing RptItemFileListing
        {
            get
            {
                if (oRptItemFileListing == null || oRptItemFileListing.IsDisposed)
                    oRptItemFileListing = new frmRptItemFileListing(InventoryReportTypeENUM.ItemFileListing);
                return oRptItemFileListing;
            }
        }

        public static frmRptTimesheet RptTimesheet
        {
            get
            {
                if (oRptTimesheet.IsDisposed)
                    oRptTimesheet = new frmRptTimesheet();
                return oRptTimesheet;
            }
        }

        public static frmRptROATransDetails RptROADetails
        {
            get
            {
                if (ofrmRptROA.IsDisposed)
                    ofrmRptROA = new frmRptROATransDetails();
                return ofrmRptROA;
            }
        }

        public static frmRptItemFileListing rptPhysicalInventorySheet
        {
            get
            {
                if (oRptPhysicalInventorySheet == null || oRptPhysicalInventorySheet.IsDisposed)
                    oRptPhysicalInventorySheet = new frmRptItemFileListing(InventoryReportTypeENUM.PhysicalInventorySheet);
                return oRptPhysicalInventorySheet;
            }
        }

        public static frmRptItemFileListing rptInventoryStatusReport
        {
            get
            {
                if (oRptInventoryStatusReport == null || oRptInventoryStatusReport.IsDisposed)
                    oRptInventoryStatusReport = new frmRptItemFileListing(InventoryReportTypeENUM.InventoryStatusReport);
                return oRptInventoryStatusReport;
            }
        }

        public static frmRptItemPriceLog rptItemPriceLog
        {
            get
            {
                if (oRptItemPriceLog == null || oRptItemPriceLog.IsDisposed)
                    oRptItemPriceLog = new frmRptItemPriceLog();
                return oRptItemPriceLog;
            }
        }

        //Added By SRT(Sachin) Date : 27 Nov 2009
        public static frmRptItemPriceLogLable rptItemPriceLogLable
        {
            get
            {
                if (oRptItemPriceLogLable == null || oRptItemPriceLogLable.IsDisposed)
                    oRptItemPriceLogLable = new frmRptItemPriceLogLable();
                return oRptItemPriceLogLable;
            }
        }
        //End of Added By SRT(Sachin) Date : 27 Nov 2009


        public static frmRptInventoryReceived RptInventoryReceived
        {
            get
            {
                if (oRptInventoryReceived == null || oRptInventoryReceived.IsDisposed)
                    oRptInventoryReceived = new frmRptInventoryReceived(InventoryReceivedEnum.InventoryReceive);
                return oRptInventoryReceived;
            }
        }

        //Added By SRT(Abhishek)
        public static frmRptInventoryReceived RptItemListingByVendor
        {
            get
            {
                if (oRptItemListing == null || oRptItemListing.IsDisposed)
                    oRptItemListing = new frmRptInventoryReceived(InventoryReceivedEnum.ItemListByVendor);
                return oRptItemListing;
            }
        }


        private static frmNumericPad NumericPad
        {
            get
            {
                if (oNumericPad == null)
                    oNumericPad = new frmNumericPad(frmMain.getInstance().Handle);
                else if (oNumericPad.IsDisposed)
                    oNumericPad = new frmNumericPad(frmMain.getInstance().Handle);
                return oNumericPad;
            }
        }

        public static frmMain getInstance()
        {
            if (ofrmMain == null || ofrmMain.IsDisposed == true)
            {
                ofrmMain = new frmMain();

                //Added By SRT(Abhishek) Date : 02/05/2009 
                //ShowForm(POSMenuItems.PoLogScreen);
                //End Of Added By SRT(Abhishek) Date : 02/05/2009
                // ofrmMain.InitializePrimePOConn(); Commected By Shitaljit to run the process on seperate thread.
                //Added by shitaljit on 18 July2013 for PRIMEPOS-1228 Run Logic to establish connection to PrimeEDI in separate thread to make PrimePOS Login process faster.
                frmMain.PrimeEDIConnectionThread = new Thread(new ThreadStart(ofrmMain.InitializePrimePOConn));
                frmMain.PrimeEDIConnectionThread.Start();
            }
            return ofrmMain;
        }

        public static frmRptPriceOverridden rptPriceOverriden
        {
            get
            {
                if (oRptPriceOverridden == null || oRptPriceOverridden.IsDisposed)
                    oRptPriceOverridden = new frmRptPriceOverridden();
                return oRptPriceOverridden;
            }
        }

        #region PRIMEPOS-2391 23-Jul-2021 JY Added
        public static frmTaxOverrideReport rptfrmTaxOverrideReport
        {
            get
            {
                if (ofrmTaxOverrideReport == null || ofrmTaxOverrideReport.IsDisposed)
                    ofrmTaxOverrideReport = new frmTaxOverrideReport();
                return ofrmTaxOverrideReport;
            }
        }
        #endregion

        #region PRIMEPOS-138 14-Feb-2021 JY Added
        public static frmInvShrinkage rptfrmInvShrinkage
        {
            get
            {
                if (ofrmInvShrinkage == null || ofrmInvShrinkage.IsDisposed)
                    ofrmInvShrinkage = new frmInvShrinkage();
                return ofrmInvShrinkage;
            }
        }
        #endregion

        public static frmRptItemDepartment changeItemDepartment
        {
            get
            {
                if (oRptItemChangeDepartment == null || oRptItemChangeDepartment.IsDisposed)
                    oRptItemChangeDepartment = new frmRptItemDepartment();
                return oRptItemChangeDepartment;
            }
        }

        public static frmSetSellingPrice setSellingPrice
        {
            get
            {
                if (oRptSetSellingPrice == null || oRptSetSellingPrice.IsDisposed)
                    oRptSetSellingPrice = new frmSetSellingPrice();
                return oRptSetSellingPrice;
            }

        }

        //Following cose is  added by Krishna on 10 October 2101
        public static frmCustomerNotes FrmCustNotes
        {
            get
            {
                if (oFrmcustNote.IsDisposed)
                    oFrmcustNote = new frmCustomerNotes("SYSTEM", "", clsEntityType.SystemNote);
                return oFrmcustNote;
            }
        }
        //Till here added by Krishna on 10 October 2101

        //Added by shitaljit(QuicSolv) on 10 October 2101
        public static frmCustomerNotes oFrmCustomerNotes
        {
            get
            {
                if (oFrmCustomerNote.IsDisposed)
                    oFrmCustomerNote = new frmCustomerNotes();
                return oFrmCustomerNote;
            }
        }
        //Till here added by shitaljit(QuicSolv) on 10 October 2101

        //Added by shitaljit(QuicSolv) on 28 Jan 2013
        public static frmPayTypes FrmPaytypes
        {
            get
            {
                if (ofrmPayTyes == null || ofrmPayTyes.IsDisposed)
                    ofrmPayTyes = new frmPayTypes();
                return ofrmPayTyes;
            }
        }

        #region PRIMEPOS-1633 28-Dec-2020 JY Added
        public static frmSetItemTax setItemTax
        {
            get
            {
                if (ofrmSetItemTax == null || ofrmSetItemTax.IsDisposed)
                    ofrmSetItemTax = new frmSetItemTax();
                return ofrmSetItemTax;
            }
        }
        #endregion

        private frmMain()
        {
            //
            // Required for Windows Form Designer support
            //
            Control.CheckForIllegalCrossThreadCalls = false;
            InitializeComponent();

            /*			foreach(Control c in this.Controls) 
                        { 
                            if(c is MdiClient) 
                            { 
                                c.BackColor = this.BackColor; 
                                c.BackgroundImage = this.BackgroundImage; 
                            } 
                        }*/
            //
            // TODO: Add any constructor code after InitializeComponent call
            //         
            //Added By Dharmendra on Jan-21-09 
            //ConfigurationManager configurationManager= new ConfigurationManager(Configuration.ApplicationName);
            //sForceFSA = configurationManager.Read("FORCE_FSA", "N"); //not working
            sForceFSA = System.Configuration.ConfigurationManager.AppSettings["FORCE_FSA"];
            if (sForceFSA != "Y" && sForceFSA != "N")
            {
                sForceFSA = "N";
            }

            #region Sprint-22 - PRIMEPOS-2245 27-Oct-2015 JY Added
            try
            {
                //SystemInfoThread = new Thread(new ThreadStart(UpdateSystemInfo));
                //SystemInfoThread.Start();

                CancellationToken _cancelToken;
                Task.Factory.StartNew(() => UpdateSystemInformation(), _cancelToken, TaskCreationOptions.LongRunning, TaskScheduler.Default);
            }
            catch (Exception Ex)
            {
                Logs.Logger("UpdateSystemInformation: " + Ex.Message);
            }
            #endregion

            #region Sprint-24 - PRIMEPOS-2344 05-Dec-2016 JY Added
            Xceed.Ftp.Licenser.LicenseKey = "FTN71-TWB3A-8DTB5-CJKA";   //PRIMEPOS-3170 02-Dec-2022 JY Modified key, old key was FTN32A7BE7JWKKY04AA;
            try
            {
                if (POS_Core.Resources.Configuration.StationID == "01")
                {
                    //IIASFileUploadThread = new Thread(new ThreadStart(IIASFileUpload));
                    //IIASFileUploadThread.Start();

                    CancellationToken _cancelToken;
                    bool bUpdateNow = false;
                    Task.Factory.StartNew(() => IIASFileUpload(bUpdateNow), _cancelToken, TaskCreationOptions.LongRunning, TaskScheduler.Default);
                }
            }
            catch (Exception Ex)
            {
                Logs.Logger("IIASFileUpload: " + Ex.Message);
            }
            #endregion

            #region Sprint-25 - PRIMEPOS-2379 07-Feb-2017 JY Added
            try
            {
                if (POS_Core.Resources.Configuration.StationID == "01" && POS_Core.Resources.Configuration.CInfo.UpdatePSEItem == true)
                {
                    CancellationToken _cancelToken;
                    bool bUpdateNow = false;
                    Task.Factory.StartNew(() => PSEFileUpload(bUpdateNow), _cancelToken, TaskCreationOptions.LongRunning, TaskScheduler.Default);
                }
            }
            catch (Exception Ex)
            {
                Logs.Logger("PSEFileUpload: " + Ex.Message);
            }
            #endregion

            #region 02-Jan-2018 set menu appearance
            this.ultMenuBar.Style = Infragistics.Win.UltraWinToolbars.ToolbarStyle.Office2013;
            this.ultMenuBar.Appearance.BackColor = System.Drawing.Color.White;
            //this.ultMenuBar.Appearance.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.ultMenuBar.Appearance.BackGradientStyle = Infragistics.Win.GradientStyle.None;
            this.ultMenuBar.Appearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));

            this.ultMenuBar.MenuSettings.Appearance.BackColor = System.Drawing.Color.White;
            //this.ultMenuBar.MenuSettings.Appearance.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.ultMenuBar.MenuSettings.Appearance.BackGradientStyle = Infragistics.Win.GradientStyle.None;
            #endregion

            #region PRIMEPOS-2613 02-Jan-2019 JY Added logic for CardExpAlertLog
            try
            {
                if (POS_Core.Resources.Configuration.CInfo.CardExpAlert == true && POS_Core.Resources.Configuration.CInfo.CardExpEmail && POS_Core.Resources.Configuration.convertNullToInt(POS_Core.Resources.Configuration.CInfo.SPEmailFormatId) > 0)
                {
                    CancellationToken _cancelToken;
                    Task.Factory.StartNew(() => CardExpAlertLog(), _cancelToken, TaskCreationOptions.LongRunning, TaskScheduler.Default);
                }
            }
            catch (Exception Ex)
            {
                Logs.Logger("CardExpAlertLog: " + Ex.Message);
            }
            #endregion

            #region PRIMEPOS-2774 13-Jan-2020 JY Added
            try
            {
                if (Configuration.CInfo.S3Enable == true && Configuration.CInfo.S3Merchant != "")
                {
                    CancellationToken _cancelToken;
                    bool bUpdateNow = false;
                    Task.Factory.StartNew(() => SolutranFileUpload(bUpdateNow), _cancelToken, TaskCreationOptions.LongRunning, TaskScheduler.Default);
                }
            }
            catch (Exception Ex)
            {
                Logs.Logger("SolutranFileUpload: " + Ex.Message);
            }
            #endregion

            #region PRIMEPOS-2778 16-Jan-2020 JY Added logic to set default department for items having null/blank department
            try
            {
                if (Configuration.CInfo.DefaultDeptId > 0)
                {
                    CancellationToken _cancelToken;
                    Task.Factory.StartNew(() => SetDefaultDepartment(), _cancelToken, TaskCreationOptions.LongRunning, TaskScheduler.Default);
                }
            }
            catch (Exception Ex)
            {
                Logs.Logger("SetDefaultDepartment: " + Ex.Message);
            }
            #endregion

            #region PRIMEPOS-2572 15-Jun-2020 JY Added
            try
            {
                if (Configuration.CInfo.useNplex == true)
                {
                    CancellationToken _cancelToken;
                    NplexBL oNplexBL = new NplexBL();
                    Task.Factory.StartNew(() => oNplexBL.VoidNplexRecovery(), _cancelToken, TaskCreationOptions.LongRunning, TaskScheduler.Default);
                }
            }
            catch (Exception Ex)
            {
                Logs.Logger("NplexRecovery: " + Ex.Message);
            }
            #endregion

            #region PRIMEPOS-3088 28-Apr-2022 JY Added
            try
            {
                if (Configuration.convertNullToString(Configuration.CSetting.LastUpdateInsSigTrans) != "" && (Convert.ToDateTime(Configuration.CSetting.LastUpdateInsSigTrans) != DateTime.MinValue)) //PRIMEPOS-3243
                {
                    #region PRIMEPOS-3243
                    DateTime LastUpdatedDateInsSigTrans = Convert.ToDateTime(Configuration.CSetting.LastUpdateInsSigTrans); //Convert.ToDateTime(DateTime.Now);
                    int interval = Configuration.convertNullToInt(Configuration.CSetting.FrequencyIntervalInsSigTrans);
                    if (interval == 0)
                        interval = Configuration.CSetting.FrequencyIntervalInsSigTrans = 30;

                    int nDateMismatch = DateTime.Compare(LastUpdatedDateInsSigTrans.AddDays(interval), Convert.ToDateTime(DateTime.Now.ToShortDateString()));
                    #endregion
                    if (nDateMismatch <= 0)
                    {
                        CancellationToken _cancelToken;
                        Task.Factory.StartNew(() => UpdateInsSigTrans(), _cancelToken, TaskCreationOptions.LongRunning, TaskScheduler.Default);
                    }
                }
            }
            catch (Exception Ex)
            {
                Logs.Logger("frmMain() - UpdateInsSigTrans() - " + Ex);
            }
            #endregion

            #region PRIMEPOS-3091 06-May-2022 JY Added
            try
            {
                if (Configuration.convertNullToString(Configuration.CSetting.LastUpdatePOSTransactionRxDetail) != "" && (Convert.ToDateTime(Configuration.CSetting.LastUpdatePOSTransactionRxDetail) != DateTime.MinValue)) //PRIMEPOS-3243
                {
                    #region PRIMEPOS-3243
                    DateTime LastUpdatedDatePOSTransactionRxDetail = Convert.ToDateTime(Configuration.CSetting.LastUpdatePOSTransactionRxDetail);
                    int interval = Configuration.convertNullToInt(Configuration.CSetting.FrequencyIntervalPOSTransactionRxDetail);
                    if (interval == 0)
                        interval = Configuration.CSetting.FrequencyIntervalPOSTransactionRxDetail = 30;

                    int nDateMismatch = DateTime.Compare(LastUpdatedDatePOSTransactionRxDetail.AddDays(interval), Convert.ToDateTime(DateTime.Now.ToShortDateString()));
                    #endregion
                    if (nDateMismatch <= 0)
                    {
                        CancellationToken _cancelToken;
                        Task.Factory.StartNew(() => UpdatePOSTransactionRxDetail(), _cancelToken, TaskCreationOptions.LongRunning, TaskScheduler.Default);
                    }
                }
            }
            catch (Exception Ex)
            {
                Logs.Logger("frmMain() - UpdatePOSTransactionRxDetail() - " + Ex);
            }
            #endregion

            #region PRIMEPOS-3092 10-May-2022 JY Added
            try
            {
                if (Configuration.convertNullToString(Configuration.CSetting.LastUpdateReturnTransDetailId) != "" && (Convert.ToDateTime(Configuration.CSetting.LastUpdateReturnTransDetailId) != DateTime.MinValue)) //PRIMEPOS-3243
                {
                    #region PRIMEPOS-3243
                    DateTime LastUpdatedDateReturnTransDetailId = Convert.ToDateTime(Configuration.CSetting.LastUpdateReturnTransDetailId);
                    int interval = Configuration.convertNullToInt(Configuration.CSetting.FrequencyIntervalReturnTransDetailId);
                    if (interval == 0)
                        interval = Configuration.CSetting.FrequencyIntervalReturnTransDetailId = 30;

                    int nDateMismatch = DateTime.Compare(LastUpdatedDateReturnTransDetailId.AddDays(interval), Convert.ToDateTime(DateTime.Now.ToShortDateString()));
                    #endregion
                    if (nDateMismatch <= 0)
                    {
                        CancellationToken _cancelToken;
                        Task.Factory.StartNew(() => UpdateReturnTransDetailId(), _cancelToken, TaskCreationOptions.LongRunning, TaskScheduler.Default);
                    }
                }
            }
            catch (Exception Ex)
            {
                Logs.Logger("frmMain() - UpdateReturnTransDetailId() - " + Ex);
            }
            #endregion

            #region PRIMEPOS-3096 17-May-2022 JY Added
            try
            {
                if (Configuration.convertNullToString(Configuration.CSetting.LastUpdateTrimItemID) != "" && (Convert.ToDateTime(Configuration.CSetting.LastUpdateTrimItemID) != DateTime.MinValue)) //PRIMEPOS-3243
                {
                    #region PRIMEPOS-3243
                    DateTime LastUpdatedDateTrimItemID = Convert.ToDateTime(Configuration.CSetting.LastUpdateTrimItemID);
                    int interval = Configuration.convertNullToInt(Configuration.CSetting.FrequencyIntervalTrimItemID);
                    if (interval == 0)
                        interval = Configuration.CSetting.FrequencyIntervalTrimItemID = 30;

                    int nDateMismatch = DateTime.Compare(LastUpdatedDateTrimItemID.AddDays(interval), Convert.ToDateTime(DateTime.Now.ToShortDateString()));
                    #endregion
                    if (nDateMismatch <= 0)
                    {
                        CancellationToken _cancelToken;
                        Task.Factory.StartNew(() => TrimItemID(), _cancelToken, TaskCreationOptions.LongRunning, TaskScheduler.Default);
                    }
                }
            }
            catch (Exception Ex)
            {
                Logs.Logger("frmMain() - TrimItemID() - " + Ex);
            }
            #endregion

            #region PRIMEPOS-3060 01-Mar-2022 JY Added
            try
            {
                if (Configuration.CSetting.RunVantivSignatureFix == true && Configuration.CPOSSet.PaymentProcessor == clsPOSDBConstants.VANTIV) //PRIMEPOS-3232N
                {
                    CancellationToken _cancelToken;
                    Task.Factory.StartNew(() => UpdateSign(), _cancelToken, TaskCreationOptions.LongRunning, TaskScheduler.Default);
                }
            }
            catch (Exception Ex)
            {
                Logs.Logger("frmMain() - UpdateSign() - " + Ex);
            }
            #endregion

            #region PRIMEPOS-3094 19-May-2022 JY Added
            try
            {
                if (Configuration.convertNullToString(Configuration.CSetting.LastUpdateMissingTransDetInPrimeRx) != "" && (Convert.ToDateTime(Configuration.CSetting.LastUpdateMissingTransDetInPrimeRx) != DateTime.MinValue)) //PRIMEPOS-3243
                {
                    #region PRIMEPOS-3243
                    DateTime LastUpdatedDateMissingTransDetInPrimeRx = Convert.ToDateTime(Configuration.CSetting.LastUpdateMissingTransDetInPrimeRx);
                    int interval = Configuration.convertNullToInt(Configuration.CSetting.FrequencyIntervalMissingTransDetInPrimeRx);
                    if (interval == 0)
                        interval = Configuration.CSetting.FrequencyIntervalMissingTransDetInPrimeRx = 30;

                    int nDateMismatch = DateTime.Compare(LastUpdatedDateMissingTransDetInPrimeRx.AddDays(interval), Convert.ToDateTime(DateTime.Now.ToShortDateString()));
                    #endregion
                    if (nDateMismatch <= 0)
                    {
                        CancellationToken _cancelToken;
                        Task.Factory.StartNew(() => UpdateMissingTransDetInPrimeRx(), _cancelToken, TaskCreationOptions.LongRunning, TaskScheduler.Default);
                    }
                }
            }
            catch (Exception Ex)
            {
                Logs.Logger("frmMain() - UpdateMissingTransDetInPrimeRx() - " + Ex);
            }

            try
            {
                if (Configuration.convertNullToString(Configuration.CSetting.LastUpdateBlankSignWithSamePatientsSign) != "" && (Convert.ToDateTime(Configuration.CSetting.LastUpdateBlankSignWithSamePatientsSign) != DateTime.MinValue)) //PRIMEPOS-3243
                {
                    #region PRIMEPOS-3243
                    DateTime LastUpdatedDateBlankSignWithSamePatientsSign = Convert.ToDateTime(Configuration.CSetting.LastUpdateBlankSignWithSamePatientsSign);
                    int interval = Configuration.convertNullToInt(Configuration.CSetting.FrequencyIntervalBlankSignWithSamePatientsSign);
                    if (interval == 0)
                        interval = Configuration.CSetting.FrequencyIntervalBlankSignWithSamePatientsSign = 30;

                    int nDateMismatch = DateTime.Compare(LastUpdatedDateBlankSignWithSamePatientsSign.AddDays(interval), Convert.ToDateTime(DateTime.Now.ToShortDateString()));
                    #endregion
                    if (nDateMismatch <= 0)
                    {
                        CancellationToken _cancelToken;
                        Task.Factory.StartNew(() => UpdateBlankSignWithSamePatientsSign(), _cancelToken, TaskCreationOptions.LongRunning, TaskScheduler.Default);
                    }
                }
            }
            catch (Exception Ex)
            {
                Logs.Logger("frmMain() - UpdateBlankSignWithSamePatientsSign() - " + Ex);
            }
            #endregion

            #region PRIMEPOS-3412
            try
            {
                if (Configuration.CSetting.NBSEnable)
                {
                    CancellationToken _cancelToken;
                    Task.Factory.StartNew(() => getTokenForNBS(), _cancelToken, TaskCreationOptions.LongRunning, TaskScheduler.Default);
                }
                else
                {
                    Logs.Logger("frmMain() - getTokenForNBS() - NBS Functionality is Disable.");
                }
            }
            catch (Exception Ex)
            {
                Logs.Logger("frmMain() - getTokenForNBS() - " + Ex);
            }
            #endregion

            #region PRIMEPOS-3372
            try
            {
                if (Configuration.CSetting.NBSEnable)
                {
                    CancellationToken _cancelToken;
                    Task.Factory.StartNew(() => getBinRangeForNBS(), _cancelToken, TaskCreationOptions.LongRunning, TaskScheduler.Default);
                }
                else
                {
                    Logs.Logger("frmMain() - getBinRangeForNBS() - NBS Functionality is Disable.");
                }
            }
            catch (Exception Ex)
            {
                Logs.Logger("frmMain() - getBinRangeForNBS() - " + Ex);
            }
            #endregion

        }

        #region PRIMEPOS-3372
        private void getBinRangeForNBS()
        {
            try
            {
                if (Configuration.CSetting.NBSEnable)
                {
                    logger.Trace("frmMain()-->getBinRangeForNBS(): " + clsPOSDBConstants.Log_Entering);

                    if (!string.IsNullOrEmpty(Configuration.CSetting.NBSUrl) && !string.IsNullOrEmpty(Configuration.CSetting.NBSToken))
                    {
                        NBSProcessor nbsProcessor = new NBSProcessor(Configuration.CSetting.NBSUrl, Configuration.CSetting.NBSToken);
                        List<BinRangeData> nBSBinRanges = new List<BinRangeData>();
                        nBSBinRanges = nbsProcessor.GetBinRange();
                        if (nBSBinRanges != null)
                        {
                            foreach (var nbsBinObj in nBSBinRanges)
                            {
                                NBSBinRange nBSBin = new NBSBinRange();
                                nBSBin.binValue = Convert.ToString(nbsBinObj.BinCode);
                                nBSBin.IsDeleted = Convert.ToBoolean(nbsBinObj.IsDelete);
                                Configuration.nBSBinRange.Add(nBSBin);
                            }
                        }
                        else
                        {
                            logger.Warn("frmMain() - getBinRangeForNBS() - Failed to get BinRange for NBS");
                        }
                    }
                    else
                    {
                        logger.Warn("frmMain() - getBinRangeForNBS() - mPOS Url or Key is missing");
                    }
                    logger.Trace("frmMain() - getBinRangeForNBS(): " + clsPOSDBConstants.Log_Exiting);
                }
            }
            catch (Exception Ex)
            {
                logger.Error("frmMain() - getBinRangeForNBS() " + Ex);
            }
        }
        #endregion

        #region PRIMEPOS-3412
        private void getTokenForNBS()
        {
            try
            {
                if (Configuration.CSetting.NBSEnable)
                {
                    logger.Trace("frmMain()-->getTokenForNBS(): " + clsPOSDBConstants.Log_Entering);

                    if (!string.IsNullOrEmpty(Configuration.CSetting.NBSUrl))
                    {
                        NBSProcessor nbsProcessor = new NBSProcessor(Configuration.CSetting.NBSUrl, "");
                        TokenResponseData nbsTokenData = new TokenResponseData();
                        nbsTokenData = nbsProcessor.GetToken(Configuration.CInfo.PHNPINO);
                        if (nbsTokenData != null)
                        {
                            Configuration.CSetting.NBSToken = nbsTokenData.AccessToken;
                            Configuration.CSetting.NBSTokenExpriresAt = Convert.ToDateTime(nbsTokenData.ExpiresIn);
                        }
                        else
                        {
                            logger.Warn("frmMain() - getTokenForNBS() - Failed to get Access Token for NBS");
                        }
                    }
                    else
                    {
                        logger.Warn("frmMain() - getTokenForNBS() - CloudPOS Url is missing. Please check the configuration.");
                    }
                    logger.Trace("frmMain() - getTokenForNBS(): " + clsPOSDBConstants.Log_Exiting);
                }
            }
            catch (Exception Ex)
            {
                logger.Error("frmMain() - getTokenForNBS() " + Ex);
            }
        }
        #endregion

        #region PRIMEPOS-3091 06-May-2022 JY Added
        private void UpdatePOSTransactionRxDetail()
        {
            PharmBL oPharmBL = new PharmBL();
            if (oPharmBL.ConnectedTo_ePrimeRx())
                return;
            try
            {
                string strSQL = "SELECT a.TransDetailID, SUBSTRING(a.ItemDescription,0,CHARINDEX('-', a.ItemDescription)) as RxNo," +
                            " CASE WHEN ISNUMERIC(SUBSTRING(SUBSTRING(a.ItemDescription, CHARINDEX('-', a.ItemDescription) + 1, LEN(a.ItemDescription)), 0, CHARINDEX('-', SUBSTRING(a.ItemDescription, CHARINDEX('-', a.ItemDescription) + 1, LEN(a.ItemDescription))))) = 1" +
                            " THEN SUBSTRING(SUBSTRING(a.ItemDescription, CHARINDEX('-', a.ItemDescription)+1, LEN(a.ItemDescription)),0,CHARINDEX('-', SUBSTRING(a.ItemDescription, CHARINDEX('-', a.ItemDescription) + 1, LEN(a.ItemDescription))))" +
                            " ELSE 0 END as nRefill, 0 AS PATIENTNO, '' AS InsType, '' AS NDC, '' AS FilledDate, '' AS PatType, '' AS COUNSELLINGREQ, b.EZCAP, 0 as RETURNTRANSDETAILID, 0 AS Modified" +
                            " FROM POSTransactionDetail a WITH(NOLOCK)" +
                            " LEFT JOIN POSTransactionRxDetail b WITH(NOLOCK) ON a.TransDetailID = b.TransDetailID" +
                            " WHERE b.TransDetailID IS NULL AND a.ItemID = 'Rx' AND ISNUMERIC(SUBSTRING(a.ItemDescription,0,CHARINDEX('-', a.ItemDescription))) = 1" +
                            " ORDER BY a.TransDetailID"; //PRIMEPOS-3243 Added WITH(NOLOCK)

                DataTable dt = DataHelper.ExecuteDataTable(Configuration.ConnectionString, CommandType.Text, strSQL);
                if (dt != null && dt.Rows.Count > 0)
                {
                    dt = oPharmBL.GetPOSTransactionRxDetail(dt);
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        var rows = dt.AsEnumerable()
                                    .Where(row => row.Field<int>("Modified") == 1)
                                    .OrderBy(row => row.Field<int>("TransDetailID"));
                        dt = rows.Any() ? rows.CopyToDataTable() : dt.Clone();

                        if (dt != null && dt.Rows.Count > 0)
                        {
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                strSQL = "INSERT INTO PostransactionRxdetail(TransDetailID,RXNo,NRefill,PatientNo,InsType,DrugNDC,DateFilled,PATTYPE,COUNSELLINGREQ,EZCAP,RETURNTRANSDETAILID) VALUES(" +
                                        Configuration.convertNullToInt(dt.Rows[i]["TransDetailID"]) + "," + Configuration.convertNullToInt64(dt.Rows[i]["RXNo"]) + "," + Configuration.convertNullToInt(dt.Rows[i]["NRefill"]) + "," +
                                        Configuration.convertNullToInt(dt.Rows[i]["PatientNo"]) + ",'" + Configuration.convertNullToString(dt.Rows[i]["InsType"]) + "','" + Configuration.convertNullToString(dt.Rows[i]["NDC"]) + "','" +
                                        Convert.ToDateTime(dt.Rows[i]["FilledDate"]).ToShortDateString() + "','" + Configuration.convertNullToString(dt.Rows[i]["PatType"]) + "','','" + Configuration.convertNullToString(dt.Rows[i]["EZCAP"]) + "',0)";
                                DataHelper.ExecuteNonQuery(strSQL);
                            }
                        }
                    }
                }
                UpdateLastUpdatedDate(clsPOSDBConstants.SettingDetail_LastUpdatePOSTransactionRxDetail); //PRIMEPOS-3243
            }
            catch (Exception Ex)
            {
                Logs.Logger("UpdatePOSTransactionRxDetail() - " + Ex);
            }
        }
        #endregion

        #region PRIMEPOS-3092 10-May-2022 JY Added
        private void UpdateReturnTransDetailId()
        {
            try
            {
                string strSQL = "WITH CTE AS" +
                        " (SELECT x.TransDetailID, x.TransType, x.RXNo, x.NRefill, x.ReturnTransDetailId, y.TransDetailID AS SalesTransDetailID FROM" +
                            " (SELECT * FROM(" +
                                " SELECT Rank() OVER(PARTITION BY c.RxNo, c.nRefill ORDER BY a.TransID DESC, b.TransDetailID DESC, a.TransType DESC) AS rnk, a.TransID, a.TransType, a.TransDate, b.TransDetailID, c.RXNo, c.NRefill, b.ReturnTransDetailId FROM POSTransaction a WITH(NOLOCK)" +
                                " INNER JOIN POSTransactionDetail b WITH(NOLOCK) ON a.TransID = b.TransID" +
                                " INNER JOIN POSTransactionRXDetail c WITH(NOLOCK) on b.TransDetailID = c.TransDetailID) x1 WHERE rnk = 1 and TransType = 2) x" +
                            " INNER JOIN" +
                            " (SELECT * FROM(" +
                                " SELECT Rank() OVER(PARTITION BY c.RxNo, c.nRefill ORDER BY a.TransID DESC, b.TransDetailID DESC, a.TransType DESC) AS rnk, a.TransID, a.TransType, a.TransDate, b.TransDetailID, c.RXNo, c.NRefill, b.ReturnTransDetailId FROM POSTransaction a WITH(NOLOCK)" +
                                " INNER JOIN POSTransactionDetail b WITH(NOLOCK) ON a.TransID = b.TransID" +
                                " INNER JOIN POSTransactionRXDetail c WITH(NOLOCK) on b.TransDetailID = c.TransDetailID) y1 WHERE rnk = 2 and TransType = 1) y ON x.RxNo = y.RxNo and x.NRefill = y.NRefill" +
                        " )" +
                        " UPDATE CTE SET ReturnTransDetailId = SalesTransDetailID WHERE TransType = 2 AND ISNULL(ReturnTransDetailId,0) = 0";
                DataHelper.ExecuteNonQuery(strSQL);

                strSQL = "WITH CTE AS" +
                        " (SELECT x.TransDetailID, x.TransType, x.RXNo, x.NRefill, x.ReturnTransDetailId, y.TransDetailID AS SalesTransDetailID FROM" +
                            " (SELECT * FROM(" +
                                " SELECT Rank() OVER(PARTITION BY c.RxNo, c.nRefill ORDER BY a.TransID DESC, b.TransDetailID DESC, a.TransType DESC) AS rnk, a.TransID, a.TransType, a.TransDate, b.TransDetailID, c.RXNo, c.NRefill, b.ReturnTransDetailId FROM POSTransaction a WITH(NOLOCK)" +
                                " INNER JOIN POSTransactionDetail b WITH(NOLOCK) ON a.TransID = b.TransID" +
                                " INNER JOIN POSTransactionRXDetail c WITH(NOLOCK) on b.TransDetailID = c.TransDetailID) x1 WHERE rnk = 1 and TransType = 2) x" +
                            " INNER JOIN" +
                            " (SELECT * FROM(" +
                                " SELECT Rank() OVER(PARTITION BY c.RxNo, c.nRefill ORDER BY a.TransID DESC, b.TransDetailID DESC, a.TransType DESC) AS rnk, a.TransID, a.TransType, a.TransDate, b.TransDetailID, c.RXNo, c.NRefill, b.ReturnTransDetailId FROM POSTransaction a WITH(NOLOCK)" +
                                " INNER JOIN POSTransactionDetail b WITH(NOLOCK) ON a.TransID = b.TransID" +
                                " INNER JOIN POSTransactionRXDetail c WITH(NOLOCK) on b.TransDetailID = c.TransDetailID) y1 WHERE rnk = 2 and TransType = 1) y ON x.RxNo = y.RxNo and x.NRefill = y.NRefill" +
                        " )" +
                        " UPDATE CTE SET ReturnTransDetailId = SalesTransDetailID WHERE TransType = 2 AND ReturnTransDetailId <> SalesTransDetailID";
                DataHelper.ExecuteNonQuery(strSQL);

                strSQL = "UPDATE b SET b.ReturnTransDetailId = 0 FROM POSTransaction a WITH(NOLOCK)" +
                        " INNER JOIN POSTransactionDetail b WITH(NOLOCK) ON a.TransID = b.TransID" +
                        " WHERE a.TransType = 1 AND ISNULL(b.ReturnTransDetailId,0) <> 0";
                DataHelper.ExecuteNonQuery(strSQL);

                strSQL = "UPDATE c SET c.ReturnTransDetailId = b.ReturnTransDetailId FROM POSTransaction a WITH(NOLOCK)" +
                    " INNER JOIN POSTransactionDetail b WITH(NOLOCK) ON a.TransID = b.TransID" +
                    " INNER JOIN POSTransactionRXDetail c WITH(NOLOCK) ON b.TransDetailID = c.TransDetailID" +
                    " WHERE a.TransType = 2 AND b.ReturnTransDetailId <> c.ReturnTransDetailId";
                DataHelper.ExecuteNonQuery(strSQL);

                UpdateLastUpdatedDate(clsPOSDBConstants.SettingDetail_LastUpdateReturnTransDetailId); //PRIMEPOS-3243
            }
            catch (Exception Ex)
            {
                Logs.Logger("UpdateReturnTransDetailId() - " + Ex);
            }
        }
        #endregion

        #region PRIMEPOS-3096 17-May-2022 JY Added
        private void TrimItemID()
        {
            try
            {
                string strSQL = "SELECT ItemID FROM Item WITH(NOLOCK) WHERE ItemID like ' %' OR ItemID like '% '";
                DataTable dt = DataHelper.ExecuteDataTable(Configuration.ConnectionString, CommandType.Text, strSQL);
                if (dt != null && dt.Rows.Count > 0)
                {
                    int RowCount = dt.Rows.Count;
                    while (RowCount > 0)
                    {
                        strSQL = "UPDATE TOP (1000) Item SET ItemID = LTRIM(RTRIM(ItemID)) WHERE ItemID like ' %' OR ItemID like '% '";
                        DataHelper.ExecuteNonQuery(strSQL);
                        RowCount -= 1000;
                    }
                }
                UpdateLastUpdatedDate(clsPOSDBConstants.SettingDetail_LastUpdateTrimItemID); //PRIMEPOS-3243
            }
            catch (Exception Ex)
            {
                Logs.Logger("TrimItemID() - " + Ex);
            }
        }
        #endregion

        #region PRIMEPOS-3088 28-Apr-2022 JY Added
        private void UpdateInsSigTrans()
        {
            PharmBL oPharmBL = new PharmBL();
            if (oPharmBL.ConnectedTo_ePrimeRx())
                return;
            try
            {
                string strSQL = "SELECT b.ID, a.TransDate, b.PatientNo, b.InsType, b.TransData, b.TransSigData, b.CounselingReq, b.SigType, b.BinarySign, IsVerified FROM POSTransaction a WITH(NOLOCK)" +
                                " INNER JOIN InsSigTrans b WITH(NOLOCK) ON a.TransID = b.TransID" +
                                " WHERE ISNULL(IsVerified,0) = 0 AND ISNULL(CAST(b.TransData AS VARCHAR(MAX)),'') <> '' ORDER BY b.ID DESC"; //PRIMEPOS-3243 Added WITH(NOLOCK)

                DataTable dt = DataHelper.ExecuteDataTable(Configuration.ConnectionString, CommandType.Text, strSQL);
                if (dt != null && dt.Rows.Count > 0)
                {
                    dt = oPharmBL.UpdateInsSigTrans(dt);
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            if (Configuration.convertNullToBoolean(dt.Rows[i]["IsVerified"]) == true) //PRIMEPOS-3211
                            {
                                strSQL = "UPDATE InsSigTrans SET IsVerified = 1 WHERE ID = " + dt.Rows[i]["ID"].ToString();
                                DataHelper.ExecuteNonQuery(strSQL);
                            }
                        }
                    }
                }
                UpdateLastUpdatedDate(clsPOSDBConstants.SettingDetail_LastUpdateInsSigTrans); //PRIMEPOS-3243
            }
            catch (Exception Ex)
            {
                Logs.Logger("UpdateInsSigTrans() - " + Ex);
            }
        }
        #endregion

        #region PRIMEPOS-3085 05-Apr-2022 JY Added
        private void UpdateSign()
        {
            UpdateInvalidSignForMultiplePatientsTrans();
            UpdateInvalidSign();
        }

        private void UpdateInvalidSignForMultiplePatientsTrans()
        {
            PharmBL oPhBl = new PharmBL();
            if (oPhBl.ConnectedTo_ePrimeRx())
                return;

            try
            {
                string strSQL = "SELECT a.ID, a.PatientNo, a.TransData, b.BinarySign FROM InsSigTrans a WITH(NOLOCK)" +
                                " INNER JOIN (SELECT * FROM (SELECT ROW_NUMBER() OVER(PARTITION BY TransID ORDER BY TransID, ID DESC) AS rNum, ID, TransID, BinarySign FROM InsSigTrans WITH(NOLOCK)" +
                                            " WHERE TransID IN(SELECT TransID FROM InsSigTrans GROUP BY TransID HAVING COUNT(TransID) > 1)) X WHERE rNum = 1) b on a.TransID = b.TransID" +
                                " WHERE a.BinarySign <> b.BinarySign"; //PRIMEPOS-3243 Added WITH(NOLOCK)
                DataTable dt = DataHelper.ExecuteDataTable(Configuration.ConnectionString, CommandType.Text, strSQL);
                if (dt != null && dt.Rows.Count > 0)
                {
                    string TransData = string.Empty;
                    string PatientNo = string.Empty;
                    foreach (DataRow dr in dt.Rows)
                    {
                        TransData = Configuration.convertNullToString(dr["TransData"]).Trim();
                        PatientNo = Configuration.convertNullToString(dr["PatientNo"]).Trim();
                        bool bStatus = false;
                        if (TransData != "" && PatientNo != "" && dr["BinarySign"] != null)
                        {
                            bStatus = oPhBl.UpdateInvalidSignForMultiplePatientsTrans(TransData, PatientNo, (byte[])(dr["BinarySign"]));
                        }
                        else
                            bStatus = true;
                        if (bStatus)
                        {
                            IDbDataParameter[] sqlParams = DataFactory.CreateParameterArray(1);
                            sqlParams[0] = new SqlParameter("@BinarySign", System.Data.SqlDbType.VarBinary);
                            sqlParams[0].SourceColumn = "BinarySign";
                            if (dr["BinarySign"] != null)
                                sqlParams[0].Value = dr["BinarySign"];
                            else
                                sqlParams[0].Value = DBNull.Value;

                            strSQL = "UPDATE InsSigTrans SET BinarySign = CAST(" + sqlParams[0].ParameterName + " AS varbinary(MAX)) WHERE ID = " + dr["ID"];
                            DataHelper.ExecuteNonQuery(Configuration.ConnectionString, CommandType.Text, strSQL, sqlParams);
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                Logs.Logger("UpdateInvalidSignForMultiplePatientsTrans() - " + Ex);
            }
        }
        #endregion

        #region PRIMEPOS-3060 01-Mar-2022 JY Added
        private void UpdateInvalidSign()
        {
            PharmBL oPhBl = new PharmBL();
            if (oPhBl.ConnectedTo_ePrimeRx())
                return;

            string strSQL = string.Empty;
            try
            {
                bool bBreak = false;
                bool bExecuteFirstTimeOnly = true;
                string strFilter = "FFFFFFFF%FFFFFFFF";
                DataTable dt = new DataTable();
                while (!bBreak)
                {
                    dt = oPhBl.GetInvalidSign(strFilter);
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        int nRecCnt = Configuration.convertNullToInt(dt.Rows[0][0].ToString());
                        if (nRecCnt > 0)
                        {
                            if (bExecuteFirstTimeOnly)
                            {
                                try
                                {
                                    oPhBl.IndexReOrganizeAndRebuild();
                                }
                                catch (Exception Ex)
                                {
                                    Logs.Logger("UpdateInvalidSign() - IndexReOrganizeAndRebuild()" + Ex);
                                }
                                bExecuteFirstTimeOnly = false;
                            }
                            logger.Trace("UpdateInvalidSign() - invalid signatures to be updated: " + nRecCnt);
                            SaveData();
                        }
                        else
                        {
                            bBreak = true;
                            Configuration.CSetting.RunVantivSignatureFix = false; //PRIMEPOS-3232N
                        }
                    }
                    else
                    {
                        bBreak = true;
                        Configuration.CSetting.RunVantivSignatureFix = false; //PRIMEPOS-3232N
                        logger.Trace("UpdateInvalidSign() - invalid signature not found.");
                    }
                }
                UpdateVantivSignatureFix(); //PRIMEPOS-3232N
            }
            catch (Exception Ex)
            {
                Logs.Logger("UpdateInvalidSign() - " + Ex);
            }
        }

        private void UpdateVantivSignatureFix() //PRIMEPOS-3232N
        {
            try
            {
                string strSQL = "UPDATE SettingDetail SET FieldValue=" + Configuration.convertBoolToInt(Configuration.CSetting.RunVantivSignatureFix) + " where FieldName = '" + clsPOSDBConstants.SettingDetail_RunVantivSignatureFix + "' and SettingID = 6";
                DataHelper.ExecuteNonQuery(Configuration.ConnectionString, CommandType.Text, strSQL);
            }
            catch (Exception ex)
            {
                logger.Error("frmMain()==>UpdateVantivSignatureFix():An Error Occured" + ex.Message);
            }
        }

        #region PRIMEPOS-3243
        private void UpdateLastUpdatedDate(string FieldName)
        {
            try
            {
                string strSQL = "UPDATE SettingDetail SET FieldValue='" + DateTime.Now.ToShortDateString() + "' where FieldName = '" + FieldName + "'";
                DataHelper.ExecuteNonQuery(Configuration.ConnectionString, CommandType.Text, strSQL);
            }
            catch (Exception ex)
            {
                logger.Error("frmMain()==>UpdateLastUploadedDate():An Error Occured. " + ex.Message);
            }
        }
        #endregion

        private void SaveData()
        {
            PharmBL oPhBl = new PharmBL();
            if (oPhBl.ConnectedTo_ePrimeRx())
                return;

            try
            {
                long TRANSNO = 0;
                byte[] imgData;
                string strSQL = string.Empty;
                string strFilter = "FFFFFFFF%FFFFFFFF";
                DataTable dt = oPhBl.GetInvalidSign(strFilter, 100);
                if (dt != null && dt.Rows.Count > 0)
                {
                    logger.Trace("SaveData() - invalid signatures to be updated in blocks: " + dt.Rows.Count.ToString() + " - Start");
                    TRANSNO = 0;
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        TRANSNO = Convert.ToInt64(dt.Rows[i]["TRANSNO"]);
                        imgData = (byte[])(dt.Rows[i]["BinarySign"]);
                        MemoryStream ms = new MemoryStream(imgData);
                        ms.Position = 0;
                        try
                        {
                            pictSign.Image = Image.FromStream(ms);
                        }
                        catch (Exception Ex)
                        {
                            Bitmap sigBitmap = ConvertPoints(imgData);
                            this.pictSign.SizeMode = PictureBoxSizeMode.StretchImage;
                            this.pictSign.Image = sigBitmap;

                            #region save data
                            MemoryStream newMS = new MemoryStream();
                            pictSign.Image.Save(newMS, ImageFormat.Png);
                            imgData = new byte[newMS.Length];
                            newMS.Position = 0;
                            newMS.Read(imgData, 0, imgData.Length);
                            oPhBl.UpdateInsSigTrans(TRANSNO, imgData);
                            logger.Trace("SaveData() - invalid signatures updated for TRANSNO: " + TRANSNO.ToString());
                            #endregion
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                Logs.Logger("SaveData() - " + Ex);
            }
        }

        private Bitmap ConvertPoints(byte[] data)
        {
            SigDiplay.Signature oSigDisplay = new SigDiplay.Signature();
            oSigDisplay.SetFormat("PointsLittleEndian");
            oSigDisplay.SetData(data);

            Bitmap oBmpSig = oSigDisplay.GetSignatureBitmap(10);

            return oBmpSig;
        }
        #endregion

        #region PRIMEPOS-3094 19-May-2022 JY Added
        private void UpdateMissingTransDetInPrimeRx()
        {
            try
            {
                string strSQL = "SELECT FieldValue FROM SettingDetail WITH(NOLOCK) WHERE FieldName = 'PrimeRxInsSigTransTransNo'"; //PRIMEPOS-3243 Added WITH(NOLOCK)
                object objValue = DataHelper.ExecuteScalar(strSQL);
                if (objValue != null)
                {
                    PharmBL oPharmBL = new PharmBL();
                    string retTransNo = oPharmBL.UpdateMissingTransDet(Configuration.convertNullToInt(objValue).ToString());
                    strSQL = "UPDATE SettingDetail SET FieldValue = '" + retTransNo + "' WHERE FieldName = 'PrimeRxInsSigTransTransNo'";
                    DataHelper.ExecuteNonQuery(strSQL);
                }
                UpdateLastUpdatedDate(clsPOSDBConstants.SettingDetail_LastUpdateMissingTransDetInPrimeRx); //PRIMEPOS-3243
            }
            catch (Exception Ex)
            {
                Logs.Logger("UpdateMissingTransDetInPrimeRx() - " + Ex);
            }
        }

        private void UpdateBlankSignWithSamePatientsSign()
        {
            try
            {
                PharmBL oPharmBL = new PharmBL();
                oPharmBL.UpdateBlankSignWithSamePatientsSign();
                UpdateLastUpdatedDate(clsPOSDBConstants.SettingDetail_LastUpdateBlankSignWithSamePatientsSign); //PRIMEPOS-3243
            }
            catch (Exception Ex)
            {
                Logs.Logger("UpdateBlankSignWithSamePatientsSign() - " + Ex);
            }
        }
        #endregion

        //Sprint-22 - PRIMEPOS-2245 27-Oct-2015 JY Added
        private static void UpdateSystemInformation()
        {
            try
            {
                using (SystemInfo obj = new SystemInfo())
                {
                    obj.UpdateSystemInfo();
                    obj.UpdateApplicationInfo();
                    obj.UpdateDllInfo();
                    obj.UpdateSystemLevelSettingsLog(); //Sprint-25 - PRIMEPOS-2245 06-Mar-2017 JY Added logic to log POS settings
                    obj.UpdateStationLevelSettingsLog(); //Sprint-25 - PRIMEPOS-2245 07-Mar-2017 JY Added logic to log POS settings
                }
            }
            catch (Exception ex)
            {
                Logs.Logger("frmMain", "UpdateSystemInformation", clsPOSDBConstants.Log_Exception_Occured + ex.Message + ex.StackTrace.ToString());
            }
        }

        #region Sprint-24 - PRIMEPOS-2344 05-Dec-2016 JY Added
        public async static Task IIASFileUpload(bool bUpdateNow)
        {
            while (true)
            {
                POS_Core.ErrorLogging.Logs.Logger("frmMain", "IIASFileUpload()", clsPOSDBConstants.Log_Entering);
                logger.Trace("frmMain()-->IIASFileUpload(): "+ clsPOSDBConstants.Log_Entering);

                DateTime LastModifiedDate = DateTime.MinValue, IIASFileModifiedDateOnFTP = DateTime.MinValue;
                bool bFileDownloadStatus = false;
                int interval = 0;

                try
                {
                    bool bIsFileExists = CheckIfFileExistsOnServer(POS_Core.Resources.Configuration.CInfo.IIASFTPAddress, POS_Core.Resources.Configuration.CInfo.IIASFTPUserId, POS_Core.Resources.Configuration.CInfo.IIASFTPPassword, POS_Core.Resources.Configuration.CInfo.IIASFileName);
                    if (bIsFileExists)
                    {
                        LastModifiedDate = GetFileModifiedDateOnFTP(POS_Core.Resources.Configuration.CInfo.IIASFTPAddress, POS_Core.Resources.Configuration.CInfo.IIASFTPUserId, POS_Core.Resources.Configuration.CInfo.IIASFTPPassword, POS_Core.Resources.Configuration.CInfo.IIASFileName);

                        string strQuery = "SELECT IIASDownloadInterval, IIASFileModifiedDateOnFTp FROM Util_Company_Info";
                        DataSet ds = DataHelper.ExecuteDataset(POS_Core.Resources.Configuration.ConnectionString, CommandType.Text, strQuery);
                        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                        {
                            if (ds.Tables[0].Rows[0]["IIASFileModifiedDateOnFTP"].ToString() != string.Empty)
                            {
                                Prefrences oPref = new Prefrences();
                                DateTime? dt = oPref.GetIIASFileLastUpdatedDate();
                                if (dt != null)
                                {
                                    IIASFileModifiedDateOnFTP = Convert.ToDateTime(ds.Tables[0].Rows[0]["IIASFileModifiedDateOnFTP"]);
                                }
                                else
                                {
                                    string strSQL = "UPDATE Util_Company_Info SET IIASFileModifiedDateOnFTp = NULL";
                                    DataHelper.ExecuteNonQuery(POS_Core.Resources.Configuration.ConnectionString, CommandType.Text, strSQL);
                                }
                            }
                            interval = POS_Core.Resources.Configuration.convertNullToInt(ds.Tables[0].Rows[0]["IIASDownloadInterval"]);
                        }

                        int nDateMismatch = DateTime.Compare(Convert.ToDateTime(LastModifiedDate.ToString("MM/dd/yyyy HH:mm:ss")), Convert.ToDateTime(IIASFileModifiedDateOnFTP.ToString("MM/dd/yyyy HH:mm:ss")));
                        if (nDateMismatch != 0)
                        {
                            SetFileSizeOfFtp(POS_Core.Resources.Configuration.CInfo.IIASFTPAddress, POS_Core.Resources.Configuration.CInfo.IIASFTPUserId, POS_Core.Resources.Configuration.CInfo.IIASFTPPassword, POS_Core.Resources.Configuration.CInfo.IIASFileName, constIIASFileType); //PRIMEPOS-3228

                            bFileDownloadStatus = DownloadFile(POS_Core.Resources.Configuration.CInfo.IIASFTPAddress, POS_Core.Resources.Configuration.CInfo.IIASFTPUserId, POS_Core.Resources.Configuration.CInfo.IIASFTPPassword, POS_Core.Resources.Configuration.CInfo.IIASFileName, constIIASFileType);

                            if (bFileDownloadStatus)
                            {
                                POS_Core.ErrorLogging.Logs.Logger("frmMain", "IIASFileUpload()", "Starting Item Import");
                                logger.Trace("frmMain()-->IIASFileUpload(): Starting Item Import");

                                bool bProcessStatus = ProcessIIASItems();

                                POS_Core.ErrorLogging.Logs.Logger("frmMain", "IIASFileUpload()", "Items Import complete");
                                logger.Trace("frmMain()-->IIASFileUpload(): Items Import complete");

                                if (bProcessStatus)
                                {
                                    // save LastModifiedDate
                                    string strSQL = "UPDATE Util_Company_Info SET IIASFileModifiedDateOnFTp = '" + LastModifiedDate + "'";
                                    DataHelper.ExecuteNonQuery(POS_Core.Resources.Configuration.ConnectionString, CommandType.Text, strSQL);
                                    clsCoreUIHelper.ShowWarningMsg("IIAS file has been uploaded successfully.", "IIAS File upload status");
                                }
                            }
                        }
                    }
                    POS_Core.ErrorLogging.Logs.Logger("frmMain", "IIASFileUpload()", clsPOSDBConstants.Log_Exiting);
                    logger.Trace("frmMain()-->IIASFileUpload(): " + clsPOSDBConstants.Log_Exiting);
                }
                catch (Exception ex)
                {
                    Logs.Logger("frmMain", "IIASFileUpload", clsPOSDBConstants.Log_Exception_Occured + ex.Message + ex.StackTrace.ToString());
                    logger.Error(ex, "frmMain()-->IIASFileUpload(): An Error Occured ");
                }
                if (POS_Core.Resources.Configuration.convertNullToInt(POS_Core.Resources.Configuration.CInfo.IIASDownloadInterval) == 0)
                {
                    if (interval > 0)
                        POS_Core.Resources.Configuration.CInfo.IIASDownloadInterval = interval;
                    else
                        POS_Core.Resources.Configuration.CInfo.IIASDownloadInterval = 6;
                }
                if (!bUpdateNow)
                    await Task.Delay(new TimeSpan(POS_Core.Resources.Configuration.CInfo.IIASDownloadInterval, 0, 0));
                else
                {
                    break;
                }
                //await Task.Delay(10000);    //testing
            }
        }

        //Added to check file exists on FTP
        private static bool CheckIfFileExistsOnServer(string strFTPAddress, string strFTPUserId, string strFTPPassword, string strFileName)
        {
            POS_Core.ErrorLogging.Logs.Logger("frmMain", "CheckIfFileExistsOnServer()", clsPOSDBConstants.Log_Entering);
            logger.Trace("frmMain()-->CheckIfFileExistsOnServer(): " + clsPOSDBConstants.Log_Entering);

            #region PRIMEPOS-2689 23-May-2019 JY Added
            string requestUriString = string.Empty;
            if (strFTPAddress.Trim().EndsWith("/"))
                requestUriString = @"ftp://" + strFTPAddress + strFileName;
            else
                requestUriString = @"ftp://" + strFTPAddress + "/" + strFileName;
            #endregion

            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(requestUriString);
            request.Credentials = new NetworkCredential(strFTPUserId, strFTPPassword);
            //request.Method = WebRequestMethods.Ftp.GetFileSize;   //PRIMEPOS-2689 23-May-2019 JY Commented
            request.Method = WebRequestMethods.Ftp.ListDirectory;   //PRIMEPOS-2689 23-May-2019 JY Added

            try
            {
                FtpWebResponse response = (FtpWebResponse)request.GetResponse();
                return true;
            }
            catch (WebException ex)
            {
                POS_Core.ErrorLogging.Logs.Logger("frmMain", "CheckIfFileExistsOnServer()", ex.Message);
                logger.Error(ex, "frmMain()-->CheckIfFileExistsOnServer(): An Error Occured ");

                FtpWebResponse response = (FtpWebResponse)ex.Response;
                if (response.StatusCode == FtpStatusCode.ActionNotTakenFileUnavailable)
                    return false;
            }
            finally
            {
                request.GetResponse().Close();  //PRIMEPOS-2689 23-May-2019 JY Added
            }
            POS_Core.ErrorLogging.Logs.Logger("frmMain", "CheckIfFileExistsOnServer()", clsPOSDBConstants.Log_Exiting);
            logger.Trace("frmMain()-->CheckIfFileExistsOnServer(): " + clsPOSDBConstants.Log_Exiting);

            return false;
        }

        private static void SetFileSizeOfFtp(string strFTPAddress, string strFTPUserId, string strFTPPassword, string strFileName, string strFileType) //PRIMEPOS-3228
        {
            logger.Trace("frmMain()-->SetFileSizeOfFtp(): " + clsPOSDBConstants.Log_Entering);

            string requestUriString = string.Empty;
            if (strFTPAddress.Trim().EndsWith("/"))
                requestUriString = @"ftp://" + strFTPAddress + strFileName;
            else
                requestUriString = @"ftp://" + strFTPAddress + "/" + strFileName;
            #endregion

            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(requestUriString);
            request.Credentials = new NetworkCredential(strFTPUserId, strFTPPassword);
            request.Method = WebRequestMethods.Ftp.GetFileSize;   

            try
            {
                FtpWebResponse response = (FtpWebResponse)request.GetResponse();
                #region PRIMEPOS-3228
                if (strFileType == constIIASFileType)
                {
                    bIIASFileSize = response.ContentLength;
                }
                else if (strFileType == constPSEFileType)
                {
                    bPSEFileSize = response.ContentLength;
                }
                #endregion
            }
            catch (WebException ex)
            {
                POS_Core.ErrorLogging.Logs.Logger("frmMain", "SetFileSizeOfFtp()", ex.Message);
                logger.Error(ex, "frmMain()-->SetFileSizeOfFtp(): An Error Occured ");                
            }
            finally
            {
                request.GetResponse().Close();
            }
            POS_Core.ErrorLogging.Logs.Logger("frmMain", "SetFileSizeOfFtp()", clsPOSDBConstants.Log_Exiting);            
        }

        //Added to get file modified date on FTP
        private static DateTime GetFileModifiedDateOnFTP(string strFTPAddress, string strFTPUserId, string strFTPPassword, string strFileName)
        {
            POS_Core.ErrorLogging.Logs.Logger("frmMain", "GetFileModifiedDateOnFTP()", clsPOSDBConstants.Log_Entering);
            logger.Trace("frmMain()-->GetFileModifiedDateOnFTP(): " + clsPOSDBConstants.Log_Entering);

            DateTime LastModifiedDate = Convert.ToDateTime("1900-01-01 00:00:00.000");

            #region PRIMEPOS-2689 23-May-2019 JY Added
            string requestUriString = string.Empty;
            if (strFTPAddress.Trim().EndsWith("/"))
                requestUriString = @"ftp://" + strFTPAddress + strFileName;
            else
                requestUriString = @"ftp://" + strFTPAddress + "/" + strFileName;
            #endregion

            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(requestUriString);

            try
            {
                request.Credentials = new NetworkCredential(strFTPUserId, strFTPPassword);
                request.Method = WebRequestMethods.Ftp.GetDateTimestamp;
                FtpWebResponse respDate = (FtpWebResponse)request.GetResponse();
                LastModifiedDate = (System.DateTime)respDate.LastModified;

                if (LastModifiedDate.Year < 1900 || LastModifiedDate.Year > DateTime.Now.Year)
                    LastModifiedDate = Convert.ToDateTime("1900-01-01 00:00:00.000");

                POS_Core.ErrorLogging.Logs.Logger("frmMain", "GetFileModifiedDateOnFTP()", clsPOSDBConstants.Log_Exiting);
                logger.Trace("frmMain()-->GetFileModifiedDateOnFTP(): " + clsPOSDBConstants.Log_Exiting);
            }
            catch (Exception Ex)
            {
                POS_Core.ErrorLogging.Logs.Logger("frmMain", "GetFileModifiedDateOnFTP()", Ex.Message);
                logger.Error(Ex, "frmMain()-->GetFileModifiedDateOnFTP(): An Error Occured ");
            }
            finally
            {
                request.GetResponse().Close();  //PRIMEPOS-2689 23-May-2019 JY Added 
            }
            return LastModifiedDate;
        }

        private static bool DownloadFile(string strFTPAddress, string strFTPUserId, string strFTPPassword, string strFileName, string strFileType)
        {
            bool retVal = false;
            string RemoteFileName = strFileName;
            string LocalFileName = Path.Combine(Application.StartupPath, strFileName);

            FtpClient ftp = null;

            try
            {
                #region PRIMEPOS-2689 23-May-2019 JY Added
                string[] arrFTPAddress = strFTPAddress.Split('/');
                string strFTPFolderPath = strFTPAddress.Substring(arrFTPAddress[0].Trim().Length);
                if (strFTPFolderPath.Trim().EndsWith("/"))
                    strFTPFolderPath = strFTPFolderPath.Substring(0, strFTPFolderPath.Length - 1);
                #endregion

                ftp = new FtpClient();
                if (strFileType == constIIASFileType)
                {
                    ftp.FileTransferStatus -= new FileTransferStatusEventHandler(ftp_IIASFileTransferStatus);   //make sure that event should not get added again and again
                    ftp.FileTransferStatus += new FileTransferStatusEventHandler(ftp_IIASFileTransferStatus);
                }
                else
                {
                    ftp.FileTransferStatus -= new FileTransferStatusEventHandler(ftp_PSEFileTransferStatus);   //make sure that event should not get added again and again
                    ftp.FileTransferStatus += new FileTransferStatusEventHandler(ftp_PSEFileTransferStatus);
                }

                POS_Core.ErrorLogging.Logs.Logger("frmMain", "DownloadFile()", strFileType + ": Connecting to Ftp Server");
                logger.Trace("frmMain()-->DownloadFile()-->"+strFileType +": Connecting to Ftp Server");

                ftp.PassiveTransfer = true;//PRIMEPOS-3228
                //ftp.Connect(strFTPAddress);   //PRIMEPOS-2689 23-May-2019 JY Commented   
                ftp.Connect(arrFTPAddress[0]);  //PRIMEPOS-2689 23-May-2019 JY Added

                POS_Core.ErrorLogging.Logs.Logger("frmMain", "DownloadFile()", strFileType + ": FTP Server connected");
                logger.Trace("frmMain()-->DownloadFile()-->" + strFileType + ": FTP Server connected");

                POS_Core.ErrorLogging.Logs.Logger("frmMain", "DownloadFile()", strFileType + ": Trying to login to FTP Server");
                logger.Trace("frmMain()-->DownloadFile()-->" + strFileType + ": Trying to login to FTP Server");

                ftp.Login(strFTPUserId, strFTPPassword);

                POS_Core.ErrorLogging.Logs.Logger("frmMain", "DownloadFile()", strFileType + ": Logon Successful");
                logger.Trace("frmMain()-->DownloadFile()-->" + strFileType + ": Logon Successful");

                POS_Core.ErrorLogging.Logs.Logger("frmMain", "DownloadFile()", strFileType + ": Downloading file");
                logger.Trace("frmMain()-->DownloadFile()-->" + strFileType + ": Downloading file");

                if (strFTPFolderPath != "") ftp.ChangeCurrentFolder(strFTPFolderPath);  //PRIMEPOS-2689 23-May-2019 JY Added  
                try
                {
                    ftp.ReceiveFile(strFileName, Path.Combine(Application.StartupPath, strFileName));
                }
                catch (Exception ex2)
                {
                    logger.Error("frmMain()-->DownloadFile()-->ReceiveFile(): An Error Occured " + ex2.Message);
                }
                if (strFileType == constIIASFileType)
                {
                    while (!bIIASFileDownloadComplete)
                    {
                        Thread.Sleep(100);
                    }
                    bIIASFileDownloadComplete = false;
                }
                else
                {
                    while (!bPSEFileDownloadComplete)
                    {
                        Thread.Sleep(100);
                    }
                    bPSEFileDownloadComplete = false;
                }

                POS_Core.ErrorLogging.Logs.Logger("frmMain", "DownloadFile()", strFileType + ": File download completed");
                logger.Trace("frmMain()-->DownloadFile()-->" + strFileType + ": File download completed");

                retVal = true;
            }
            catch (System.Exception ex1)
            {
                POS_Core.ErrorLogging.Logs.Logger("frmMain", "DownloadFile(): " + strFileType, ex1.Message);
                logger.Error(ex1, "frmMain()-->DownloadFile(): An Error Occured ");
            }
            finally
            {
                ftp.Disconnect();
            }
            return retVal;
        }

        private static void ftp_IIASFileTransferStatus(object sender, FileTransferStatusEventArgs e)
        {
            if (e.AllBytesTotal > 0)
            {
                if (e.AllBytesTotal == e.AllBytesTransferred)
                {
                    bIIASFileDownloadComplete = true;
                }
            }
            else
            {
                if (bIIASFileSize != 0)
                {
                    if (bIIASFileSize == e.AllBytesTransferred)
                    {
                        bIIASFileDownloadComplete = true;
                    }
                }
            }

            //if (e.AllBytesTotal == e.AllBytesTransferred)
            //{
            //    bIIASFileDownloadComplete = true;
            //}
        }

        private static void ftp_PSEFileTransferStatus(object sender, FileTransferStatusEventArgs e)
        {
            if (e.AllBytesTotal > 0)
            {
                if (e.AllBytesTotal == e.AllBytesTransferred)
                {
                    bPSEFileDownloadComplete = true;
                }
            }
            else
            {
                if (bPSEFileSize != 0)
                {
                    if (bPSEFileSize == e.AllBytesTransferred)
                    {
                        bPSEFileDownloadComplete = true;
                    }
                }
            }

            //if (e.AllBytesTotal == e.AllBytesTransferred)
            //{
            //    bPSEFileDownloadComplete = true;
            //}
        }

        private static bool ProcessIIASItems()
        {
            bool bStatus = true;
            int cnt = 0;

            WriteSchema();
            DataSet oDS = null;
            oDS = LoadFile(POS_Core.Resources.Configuration.CInfo.IIASFileName);

            if (oDS == null) return false;

            POS_Core.ErrorLogging.Logs.Logger("frmMain", "ProcessItems()", "Changing status of existing items");
            logger.Trace("frmMain()-->ProcessItems()-->: Changing status of existing items");

            DataHelper.ExecuteNonQuery(POS_Core.Resources.Configuration.ConnectionString, CommandType.Text, "Update IIAS_Items Set IsActive=0, InActivateDate=GetDate() where IsActive=1 ");

            POS_Core.ErrorLogging.Logs.Logger("frmMain", "ProcessItems()", "Updated status of existing items");
            logger.Trace("frmMain()-->ProcessItems()-->: Updated status of existing items");

            foreach (DataRow oRow in oDS.Tables[0].Rows)
            {
                string strQuery;
                strQuery = " Select ID From IIAS_Items where UPCCOde='" + oRow["UPC"].ToString().Replace("'", "''") + "'";
                object objValue = DataHelper.ExecuteScalar(strQuery);
                if (objValue == null)
                {
                    strQuery = "INSERT INTO IIAS_Items  " +
                    " ([UPCCode], [GTIN], [Description],  " +
                    " [FineLineCode], [CategoryDescriptor], [SubCategoryDescriptor], [FinestCategoryDescriptor],  " +
                    " [ManufacturerName], [ChangeDate], [ChangeIndicator], [CreateDate],  " +
                    "[IsActive], [InActivateDate])  " +
                    " Values( '" + oRow["UPC"].ToString().Replace("'", "''") + "','" + oRow["GTIN"].ToString().Replace("'", "''") + "','" + oRow["Description"].ToString().Replace("'", "''") + "'," +
                    "'" + oRow["FLC"].ToString().Replace("'", "''") + "','" + oRow["CategoryDescription"].ToString().Replace("'", "''") + "','" + oRow["SubCategoryDescription"].ToString().Replace("'", "''") + "','" + oRow["FinestCategoryDescription"].ToString().Replace("'", "''") + "'," +
                    "'" + oRow["ManufacturerName"].ToString().Replace("'", "''") + "','" + oRow["ChangeDate"].ToString().Replace("'", "") + "','" + oRow["ChangeIndicator"].ToString().Replace("'", "''") + "',getdate() " +
                    " ,1 ,null )";
                }
                else
                {
                    strQuery = "Update IIAS_Items Set " +
                    " [GTIN]='" + oRow["GTIN"].ToString().Replace("'", "''") + "'" +
                    " ,[Description]=  '" + oRow["Description"].ToString().Replace("'", "''") + "' " +
                    " ,[FineLineCode]= '" + oRow["FLC"].ToString().Replace("'", "''") + "' " +
                    " ,[CategoryDescriptor]= '" + oRow["CategoryDescription"].ToString().Replace("'", "''") + "' " +
                    " ,[SubCategoryDescriptor]= '" + oRow["SubCategoryDescription"].ToString().Replace("'", "''") + "' " +
                    " ,[FinestCategoryDescriptor]=  '" + oRow["FinestCategoryDescription"].ToString().Replace("'", "''") + "'" +
                    " ,[ManufacturerName]= '" + oRow["ManufacturerName"].ToString().Replace("'", "''") + "' " +
                    " ,[ChangeDate]= '" + oRow["ChangeDate"].ToString().Replace("'", "") + "' " +
                    " ,[ChangeIndicator]= '" + oRow["ChangeIndicator"].ToString().Replace("'", "''") + "' " +
                    " ,[CreateDate] = getdate() " +
                    " ,[IsActive]=1, [InActivateDate]=null  " +
                    " Where[UPCCode]= '" + oRow["UPC"].ToString().Replace("'", "''") + "'";
                }
                try
                {
                    DataHelper.ExecuteNonQuery(POS_Core.Resources.Configuration.ConnectionString, CommandType.Text, strQuery);
                    cnt++;
                }
                catch (Exception exp)
                {
                    POS_Core.ErrorLogging.Logs.Logger("frmMain", "ProcessItems()", exp.Message);
                    logger.Error(exp, "frmMain()-->ProcessItems(): An Error Occured ");

                    bStatus = false;
                }
            }
            return bStatus;
        }

        private static void WriteSchema()
        {
            try
            {
                FileStream fsOutput = new FileStream(Path.Combine(Application.StartupPath, "schema.ini"), FileMode.Create, FileAccess.Write);
                StreamWriter srOutput = new StreamWriter(fsOutput);
                string s1, s2, s3, s4, s5;

                s1 = "[" + POS_Core.Resources.Configuration.CInfo.IIASFileName + "]";
                s2 = "ColNameHeader=TRUE";
                s3 = "Format=Delimited(,)";
                s4 = "MaxScanRows=25";
                s5 = "CharacterSet=ANSI";

                srOutput.WriteLine(s1.ToString() + "\r\n" + s2.ToString() + "\r\n" + s3.ToString() + "\r\n" + s4.ToString() + "\r\n" + s5.ToString());
                srOutput.Close();
                fsOutput.Close();
            }
            catch (Exception ex)
            {
                POS_Core.ErrorLogging.Logs.Logger("frmMain", "WriteSchema()", ex.Message);
            }
        }

        private static DataSet LoadFile(string strFileName)
        {
            DataSet dataSet = new DataSet();
            try
            {
                OleDbConnection selectConnection = new OleDbConnection(("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + Application.StartupPath + ";Extended Properties=\"text;HDR=YES;FMT=Delimited;\";").Trim());
                selectConnection.Open();
                new OleDbDataAdapter("select * from [" + strFileName + "]", selectConnection).Fill(dataSet, "csv");
                selectConnection.Close();
            }
            catch (Exception ex)
            {
                POS_Core.ErrorLogging.Logs.Logger("frmMain", "LoadFile()", "Error while reading from " + strFileName + " file: " + ex.Message);
            }
            return dataSet;
        }
        //#endregion

        #region Sprint-25 - PRIMEPOS-2379 07-Feb-2017 JY Added
        public async static Task PSEFileUpload(bool bUpdateNow)
        {
            while (true)
            {
                POS_Core.ErrorLogging.Logs.Logger("frmMain", "PSEFileUpload()", clsPOSDBConstants.Log_Entering);
                logger.Trace("frmMain()-->PSEFileUpload(): " + clsPOSDBConstants.Log_Entering);

                DateTime LastModifiedDate = DateTime.MinValue, PSEFileModifiedDateOnFTP = DateTime.MinValue;
                bool bFileDownloadStatus = false;
                int interval = 0;

                try
                {
                    bool bIsFileExists = CheckIfFileExistsOnServer(POS_Core.Resources.Configuration.CInfo.PSEFTPAddress, POS_Core.Resources.Configuration.CInfo.PSEFTPUserId, POS_Core.Resources.Configuration.CInfo.PSEFTPPassword, POS_Core.Resources.Configuration.CInfo.PSEFileName);
                    if (bIsFileExists)
                    {
                        LastModifiedDate = GetFileModifiedDateOnFTP(POS_Core.Resources.Configuration.CInfo.PSEFTPAddress, POS_Core.Resources.Configuration.CInfo.PSEFTPUserId, POS_Core.Resources.Configuration.CInfo.PSEFTPPassword, POS_Core.Resources.Configuration.CInfo.PSEFileName);

                        string strQuery = "SELECT PSEDownloadInterval, PSEFileModifiedDateOnFTp FROM Util_Company_Info";
                        DataSet ds = DataHelper.ExecuteDataset(POS_Core.Resources.Configuration.ConnectionString, CommandType.Text, strQuery);
                        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                        {
                            if (ds.Tables[0].Rows[0]["PSEFileModifiedDateOnFTP"].ToString() != string.Empty)
                            {
                                Prefrences oPref = new Prefrences();
                                DateTime? dt = oPref.GetPSEFileLastUpdatedDate();
                                if (dt != null)
                                {
                                    PSEFileModifiedDateOnFTP = Convert.ToDateTime(ds.Tables[0].Rows[0]["PSEFileModifiedDateOnFTP"]);
                                }
                                else
                                {
                                    string strSQL = "UPDATE Util_Company_Info SET PSEFileModifiedDateOnFTp = NULL";
                                    DataHelper.ExecuteNonQuery(POS_Core.Resources.Configuration.ConnectionString, CommandType.Text, strSQL);
                                }
                            }
                            interval = POS_Core.Resources.Configuration.convertNullToInt(ds.Tables[0].Rows[0]["PSEDownloadInterval"]);
                        }

                        int nDateMismatch = DateTime.Compare(Convert.ToDateTime(LastModifiedDate.ToString("MM/dd/yyyy HH:mm:ss")), Convert.ToDateTime(PSEFileModifiedDateOnFTP.ToString("MM/dd/yyyy HH:mm:ss")));
                        if (nDateMismatch != 0)
                        {
                            SetFileSizeOfFtp(POS_Core.Resources.Configuration.CInfo.PSEFTPAddress, POS_Core.Resources.Configuration.CInfo.PSEFTPUserId, POS_Core.Resources.Configuration.CInfo.PSEFTPPassword, POS_Core.Resources.Configuration.CInfo.PSEFileName, constPSEFileType); //PRIMEPOS-3228

                            bFileDownloadStatus = DownloadFile(POS_Core.Resources.Configuration.CInfo.PSEFTPAddress, POS_Core.Resources.Configuration.CInfo.PSEFTPUserId, POS_Core.Resources.Configuration.CInfo.PSEFTPPassword, POS_Core.Resources.Configuration.CInfo.PSEFileName, constPSEFileType);

                            if (bFileDownloadStatus)
                            {
                                POS_Core.ErrorLogging.Logs.Logger("frmMain", "PSEFileUpload()", "Starting Item Import");
                                logger.Trace("frmMain()-->PSEFileUpload(): Starting Item Import");

                                bool bProcessStatus = ProcessPSEItems();

                                POS_Core.ErrorLogging.Logs.Logger("frmMain", "PSEFileUpload()", "Items Import complete");
                                logger.Trace("frmMain()-->PSEFileUpload(): Items Import complete");

                                if (bProcessStatus)
                                {
                                    // save LastModifiedDate
                                    string strSQL = "UPDATE Util_Company_Info SET PSEFileModifiedDateOnFTp = '" + LastModifiedDate + "'";
                                    DataHelper.ExecuteNonQuery(POS_Core.Resources.Configuration.ConnectionString, CommandType.Text, strSQL);
                                    clsCoreUIHelper.ShowWarningMsg("PSE file has been uploaded successfully.", "PSE File upload status");
                                    //bReturn = true;
                                }
                            }
                        }
                    }
                    POS_Core.ErrorLogging.Logs.Logger("frmMain", "PSEFileUpload()", clsPOSDBConstants.Log_Exiting);
                    logger.Trace("frmMain()-->PSEFileUpload(): " + clsPOSDBConstants.Log_Exiting);
                }
                catch (Exception ex)
                {
                    //bReturn = false;
                    Logs.Logger("frmMain", "PSEFileUpload", clsPOSDBConstants.Log_Exception_Occured + ex.Message + ex.StackTrace.ToString());
                    logger.Error(ex, "frmMain()-->PSEFileUpload(): An Error Occured ");
                }
                if (POS_Core.Resources.Configuration.convertNullToInt(POS_Core.Resources.Configuration.CInfo.PSEDownloadInterval) == 0)
                {
                    if (interval > 0)
                        POS_Core.Resources.Configuration.CInfo.PSEDownloadInterval = interval;
                    else
                        POS_Core.Resources.Configuration.CInfo.PSEDownloadInterval = 6;
                }
                if (!bUpdateNow)
                    await Task.Delay(new TimeSpan(POS_Core.Resources.Configuration.CInfo.PSEDownloadInterval, 0, 0));
                else
                {
                    break;
                }
                //await Task.Delay(10000);    //testing
            }
        }

        private static bool ProcessPSEItems()
        {
            bool bStatus = true;
            int cnt = 0;

            DataSet oDS = LoadFile(POS_Core.Resources.Configuration.CInfo.PSEFileName);
            if (oDS == null) return false;
            PSEItemSvr oPSEItemSvr = new PSEItemSvr();
            string strQuery = string.Empty;
            foreach (DataRow oRow in oDS.Tables[0].Rows)
            {
                bool bIsRecordExists = oPSEItemSvr.IsPSEItemExists(oRow["ProductId"].ToString());
                if (bIsRecordExists == false)
                {
                    strQuery = "INSERT INTO PSE_Items ([ProductId],[ProductName],[ProductNDC],[ProductGrams],[ProductPillCnt],[CreatedBy],[CreatedOn],[UpdatedBy],[UpdatedOn],[IsActive],[RecordStatus])" +
                        " VALUES('" + oRow["ProductId"].ToString().Replace("'", "''") + "','" + oRow["ProductName"].ToString().Replace("'", "''") + "','" + oRow["ProductNDC"].ToString().Replace("'", "''").Replace("-", "")
                        + "'," + Configuration.convertNullToDecimal(oRow["ProductGrams"].ToString().Replace("'", "''")) + "," + POS_Core.Resources.Configuration.convertNullToInt(oRow["ProductPillCnt"].ToString().Replace("'", "''")) + ",'" + POS_Core.Resources.Configuration.UserName + "',GETDATE(),'" +
                       POS_Core.Resources.Configuration.UserName + "',GETDATE(),1,'E')";
                }
                else
                {
                    strQuery = "Update PSE_Items Set " +
                        " [ProductName]=  '" + oRow["ProductName"].ToString().Replace("'", "''") + "' " +
                        " ,[ProductNDC]= '" + oRow["ProductNDC"].ToString().Replace("'", "''").Replace("-", "") + "' " +
                        " ,[ProductGrams]= " + POS_Core.Resources.Configuration.convertNullToDecimal(oRow["ProductGrams"].ToString().Replace("'", "''")) +
                        " ,[ProductPillCnt]= " + POS_Core.Resources.Configuration.convertNullToInt(oRow["ProductPillCnt"].ToString().Replace("'", "''")) +
                        " ,[UpdatedBy]=  '" + POS_Core.Resources.Configuration.UserName + "'" +
                        " ,[UpdatedOn]= GETDATE(), [RecordStatus] = 'E', [IsActive] = 1" +
                        " Where SUBSTRING(ProductID,1,11) = SUBSTRING('" + oRow["ProductId"].ToString().Replace("'", "''") + "',1,11)";
                }
                try
                {
                    DataHelper.ExecuteNonQuery(Configuration.ConnectionString, CommandType.Text, strQuery);
                    cnt++;
                }
                catch (Exception exp)
                {
                    POS_Core.ErrorLogging.Logs.Logger("frmMain", "ProcessItems()", exp.Message);
                    bStatus = false;
                }
            }
            return bStatus;
        }
        #endregion

        #region PRIMEPOS-2613 02-Jan-2019 JY Added logic for CardExpAlertLog
        private void CardExpAlertLog()
        {
            POS_Core.ErrorLogging.Logs.Logger("frmMain", "CardExpAlertLog()", clsPOSDBConstants.Log_Entering);
            UpdateCardExpAlertLog();    //find new alert records and update CardExpAlertLog table
            SendCardExpAlert(); //send alert - also try MaxAttempts for non-processed records
            POS_Core.ErrorLogging.Logs.Logger("frmMain", "CardExpAlertLog()", clsPOSDBConstants.Log_Exiting);
        }

        private void UpdateCardExpAlertLog()
        {
            int nDays = POS_Core.Resources.Configuration.convertNullToInt(Configuration.CInfo.CardExpAlertDays);
            if (nDays > 0)
            {
                string strSQL = "INSERT INTO CardExpAlertLog(CustomerID, EntryID, ExpDate, AlertDate, AlertStatus, NoOfAttempt, Comments) " +
                        " SELECT a.CustomerID, a.EntryID, a.ExpDate, GETDATE(), 0, 0, '' FROM CCCustomerTokInfo a " +
                        " LEFT JOIN CardExpAlertLog b ON a.EntryID = b.EntryID" +
                        " WHERE b.EntryID IS NULL AND CONVERT(DATE, a.ExpDate) >= CONVERT(DATE, getdate() - 2) AND CONVERT(DATE, a.ExpDate) <= CONVERT(DATE, getdate() + " + nDays + " + 2)" +
                        " ORDER BY a.EntryID";

                DataHelper.ExecuteNonQuery(Configuration.ConnectionString, CommandType.Text, strSQL);
            }
        }

        private void SendCardExpAlert()
        {
            if (Configuration.convertNullToInt(Configuration.CInfo.SPEmailFormatId) > 0)
            {
                //find eligible records
                string strSQL = "SELECT a.AlertID, a.CustomerID, c.EmailAddress, " +
                                " CASE WHEN ISNULL(b.Last4,'') <> '' THEN 'XXXX XXXX XXXX ' + b.Last4 ELSE '' END AS CardNo, " +
                                " b.ExpDate, a.NoOfAttempt FROM CardExpAlertLog a " +
                                " INNER JOIN CCCustomerTokInfo b ON a.EntryID = b.EntryID " +
                                " INNER JOIN Customer c ON b.CustomerID = c.CustomerID " +
                                " WHERE a.AlertStatus = 0 AND a.NoOfAttempt < 6";

                DataSet ds = DataHelper.ExecuteDataset(Configuration.ConnectionString, CommandType.Text, strSQL);
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    //get template details
                    bool bTemplateStatus = false;
                    string strMessageSub = string.Empty, strMessageBody = string.Empty;

                    MsgTemplateData oMsgTemplateData = new MsgTemplateData();
                    MsgTemplate oMsgTemplate = new MsgTemplate();
                    oMsgTemplateData = oMsgTemplate.Populate(Configuration.convertNullToInt(Configuration.CInfo.SPEmailFormatId));
                    if (oMsgTemplateData != null && oMsgTemplateData.Tables.Count > 0 && oMsgTemplateData.Tables[0].Rows.Count > 0)
                    {
                        bTemplateStatus = true;
                        strMessageSub = POS_Core.Resources.Configuration.convertNullToString(oMsgTemplateData.MsgTemplate.Rows[0]["MessageSub"]);
                        strMessageBody = POS_Core.Resources.Configuration.convertNullToString(oMsgTemplateData.MsgTemplate.Rows[0]["Message"]);
                    }
                    //send email to customers
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        try
                        {
                            bool bStatus = false;
                            string strComments = "", strMessage;
                            if (bTemplateStatus == true)
                            {
                                string strDate = string.Empty;
                                try
                                {
                                    strDate = Convert.ToDateTime(dr["ExpDate"]).ToString("MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                                }
                                catch { }
                                strMessage = strMessageBody + "<br/>" + POS_Core.Resources.Configuration.convertNullToString(dr["CardNo"]) + " | " + strDate;
                                bStatus = POS_Core_UI.Reports.ReportsUI.clsReports.EmailReport(null, POS_Core.Resources.Configuration.convertNullToString(dr["EmailAddress"]), strMessageSub, strMessage, "File", false);
                            }
                            else
                            {
                                strComments = "Message template not assigned";
                            }
                            //update CardExpAlertLog
                            int nNoOfAttempt = POS_Core.Resources.Configuration.convertNullToInt(dr["NoOfAttempt"]) + 1;
                            strSQL = "UPDATE CardExpAlertLog SET AlertStatus = " + (bStatus == true ? 1 : 0) + ", NoOfAttempt = " + nNoOfAttempt + ", Comments = '" + strComments + "' WHERE AlertID = " + POS_Core.Resources.Configuration.convertNullToInt64(dr["AlertID"]);
                            DataHelper.ExecuteNonQuery(POS_Core.Resources.Configuration.ConnectionString, CommandType.Text, strSQL);
                        }
                        catch (Exception Ex)
                        {
                            //update CardExpAlertLog with exception
                            int nNoOfAttempt = POS_Core.Resources.Configuration.convertNullToInt(dr["NoOfAttempt"]) + 1;
                            strSQL = "UPDATE CardExpAlertLog SET AlertStatus = 0, NoOfAttempt = " + nNoOfAttempt + ", Comments = " + Ex.Message + " WHERE AlertID = " + POS_Core.Resources.Configuration.convertNullToInt64(dr["AlertID"]);
                            DataHelper.ExecuteNonQuery(Configuration.ConnectionString, CommandType.Text, strSQL);
                        }
                    }
                }
            }
        }
        #endregion

        //
        private void InitializePrimePOConn()
        {
            try
            {
                if (POS_Core.Resources.Configuration.CPrimeEDISetting.UsePrimePO == false)   //PRIMEPOS-3167 07-Nov-2022 JY Modified
                {
                    POS_Core.ErrorLogging.Logs.Logger("PrimeEDI is not Set up to Current Station# " + POS_Core.Resources.Configuration.StationID);
                    ClsUpdatePOStatus.UpdateStatusInst.UpdataConStatus("DISCONNECTED");
                }
                else if (UserPriviliges.IsUserHasPriviliges(UserPriviliges.Modules.InventoryMgmt.ID))
                {
                    POS_Core.ErrorLogging.Logs.Logger("PrimeEDI Connection Initilization Started.");
                    ClsUpdatePOStatus clsUpdateStatus = ClsUpdatePOStatus.UpdateStatusInst;
                    ClsUpdatePOStatus.UpdateConnectionStatus -= new ClsUpdatePOStatus.UpdateConnStatus(ClsUpdatePOStatus_UpdateConnectionStatus);
                    ClsUpdatePOStatus.UpdateConnectionStatus += new ClsUpdatePOStatus.UpdateConnStatus(ClsUpdatePOStatus_UpdateConnectionStatus);
                    PrimePOUtil poUtil = PrimePOUtil.DefaultInstance;
                    ClsUpdatePOStatus.POCount += new ClsUpdatePOStatus.UpdateCount(OnCountUpdated);
                    ClsUpdatePOStatus.UpdateStatusInst.UpdatePOCount();
                    //ShowForm(POSMenuItems.PoLogScreen); //Commented by shitaljit as there is no need for this code here.
                    POS_Core.ErrorLogging.Logs.Logger("PrimeEDI Connection Initilization Completed.");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                //Added by shitaljit on 18 July2013 for PRIMEPOS-1228 Run Logic to establish connection to PrimeEDI in separate thread to make PrimePOS Login process faster.
                if (frmMain.PrimeEDIConnectionThread.ThreadState != null && frmMain.PrimeEDIConnectionThread.ThreadState == System.Threading.ThreadState.Running)
                {
                    frmMain.PrimeEDIConnectionThread.Abort();
                }
            }

        }

        void ClsUpdatePOStatus_UpdateConnectionStatus(string message)
        {

            try
            {
                this.ultraStatusBarVendor.Panels[POSMenuItems.PrimePOSStatus].Text = "";
                this.ultraStatusBarVendor.Panels[POSMenuItems.PrimePOSStatus].Text = "PrimePO :";
                this.ultraStatusBarVendor.Panels[POSMenuItems.PrimePOSStatus].Text = this.ultraStatusBarVendor.Panels[POSMenuItems.PrimePOSStatus].DisplayText + message.ToString();

                //Added & Modified By Dharmendra SRT on Mar-05-09 to change the back colour
                if (message.Trim().ToUpper() == "CONNECTED")
                {
                    this.ultraStatusBarVendor.Panels[POSMenuItems.PrimePOSStatus].Appearance.BackColor = System.Drawing.Color.Green;
                }
                else
                {
                    this.ultraStatusBarVendor.Panels[POSMenuItems.PrimePOSStatus].Appearance.BackColor = System.Drawing.Color.Red;
                    if (MsgCnt == 0) { MsgCnt = 1; clsCoreUIHelper.ShowErrorMsg(" PrimePO is Disconnected "); }  //19-Nov-2015 JY Added 
                }
                //Added & Modified till here Mar-05-09   
            }
            catch (Exception ex)
            {
                //POS_Core.ErrorLogging.ErrorHandler.logException(ex, "", "");  //PRIMEPOS-2971 04-Jun-2021 JY Commented as no need to log it in errorlog
            }
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
            KillProcess(); //Added By Dharmendra (SRT) on 13-10-08 to remove the POS.exe from task Manager
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
            Infragistics.Win.UltraWinExplorerBar.UltraExplorerBarGroup ultraExplorerBarGroup1 = new Infragistics.Win.UltraWinExplorerBar.UltraExplorerBarGroup();
            Infragistics.Win.UltraWinExplorerBar.UltraExplorerBarItem ultraExplorerBarItem1 = new Infragistics.Win.UltraWinExplorerBar.UltraExplorerBarItem();
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            Infragistics.Win.UltraWinExplorerBar.UltraExplorerBarItem ultraExplorerBarItem2 = new Infragistics.Win.UltraWinExplorerBar.UltraExplorerBarItem();
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinExplorerBar.UltraExplorerBarItem ultraExplorerBarItem3 = new Infragistics.Win.UltraWinExplorerBar.UltraExplorerBarItem();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinExplorerBar.UltraExplorerBarGroup ultraExplorerBarGroup2 = new Infragistics.Win.UltraWinExplorerBar.UltraExplorerBarGroup();
            Infragistics.Win.UltraWinExplorerBar.UltraExplorerBarItem ultraExplorerBarItem4 = new Infragistics.Win.UltraWinExplorerBar.UltraExplorerBarItem();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinExplorerBar.UltraExplorerBarItem ultraExplorerBarItem5 = new Infragistics.Win.UltraWinExplorerBar.UltraExplorerBarItem();
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinExplorerBar.UltraExplorerBarItem ultraExplorerBarItem6 = new Infragistics.Win.UltraWinExplorerBar.UltraExplorerBarItem();
            Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinExplorerBar.UltraExplorerBarItem ultraExplorerBarItem7 = new Infragistics.Win.UltraWinExplorerBar.UltraExplorerBarItem();
            Infragistics.Win.Appearance appearance7 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinExplorerBar.UltraExplorerBarItem ultraExplorerBarItem8 = new Infragistics.Win.UltraWinExplorerBar.UltraExplorerBarItem();
            Infragistics.Win.Appearance appearance8 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinExplorerBar.UltraExplorerBarGroup ultraExplorerBarGroup3 = new Infragistics.Win.UltraWinExplorerBar.UltraExplorerBarGroup();
            Infragistics.Win.UltraWinExplorerBar.UltraExplorerBarItem ultraExplorerBarItem9 = new Infragistics.Win.UltraWinExplorerBar.UltraExplorerBarItem();
            Infragistics.Win.Appearance appearance9 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinExplorerBar.UltraExplorerBarItem ultraExplorerBarItem10 = new Infragistics.Win.UltraWinExplorerBar.UltraExplorerBarItem();
            Infragistics.Win.Appearance appearance10 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinExplorerBar.UltraExplorerBarItem ultraExplorerBarItem11 = new Infragistics.Win.UltraWinExplorerBar.UltraExplorerBarItem();
            Infragistics.Win.Appearance appearance11 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinExplorerBar.UltraExplorerBarItem ultraExplorerBarItem12 = new Infragistics.Win.UltraWinExplorerBar.UltraExplorerBarItem();
            Infragistics.Win.Appearance appearance12 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinExplorerBar.UltraExplorerBarItem ultraExplorerBarItem13 = new Infragistics.Win.UltraWinExplorerBar.UltraExplorerBarItem();
            Infragistics.Win.Appearance appearance13 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinExplorerBar.UltraExplorerBarItem ultraExplorerBarItem14 = new Infragistics.Win.UltraWinExplorerBar.UltraExplorerBarItem();
            Infragistics.Win.Appearance appearance14 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinExplorerBar.UltraExplorerBarItem ultraExplorerBarItem15 = new Infragistics.Win.UltraWinExplorerBar.UltraExplorerBarItem();
            Infragistics.Win.Appearance appearance15 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinExplorerBar.UltraExplorerBarItem ultraExplorerBarItem16 = new Infragistics.Win.UltraWinExplorerBar.UltraExplorerBarItem();
            Infragistics.Win.Appearance appearance16 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance17 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinStatusBar.UltraStatusPanel ultraStatusPanel1 = new Infragistics.Win.UltraWinStatusBar.UltraStatusPanel();
            Infragistics.Win.UltraWinStatusBar.UltraStatusPanel ultraStatusPanel2 = new Infragistics.Win.UltraWinStatusBar.UltraStatusPanel();
            Infragistics.Win.UltraWinStatusBar.UltraStatusPanel ultraStatusPanel3 = new Infragistics.Win.UltraWinStatusBar.UltraStatusPanel();
            Infragistics.Win.Appearance appearance18 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinStatusBar.UltraStatusPanel ultraStatusPanel4 = new Infragistics.Win.UltraWinStatusBar.UltraStatusPanel();
            Infragistics.Win.Appearance appearance19 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinStatusBar.UltraStatusPanel ultraStatusPanel5 = new Infragistics.Win.UltraWinStatusBar.UltraStatusPanel();
            Infragistics.Win.Appearance appearance20 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinStatusBar.UltraStatusPanel ultraStatusPanel6 = new Infragistics.Win.UltraWinStatusBar.UltraStatusPanel();
            Infragistics.Win.Appearance appearance21 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinStatusBar.UltraStatusPanel ultraStatusPanel7 = new Infragistics.Win.UltraWinStatusBar.UltraStatusPanel();
            Infragistics.Win.UltraWinStatusBar.UltraStatusPanel ultraStatusPanel8 = new Infragistics.Win.UltraWinStatusBar.UltraStatusPanel();
            Infragistics.Win.UltraWinStatusBar.UltraStatusPanel ultraStatusPanel9 = new Infragistics.Win.UltraWinStatusBar.UltraStatusPanel();
            Infragistics.Win.UltraWinStatusBar.UltraStatusPanel ultraStatusPanel10 = new Infragistics.Win.UltraWinStatusBar.UltraStatusPanel();
            Infragistics.Win.UltraWinStatusBar.UltraStatusPanel ultraStatusPanel11 = new Infragistics.Win.UltraWinStatusBar.UltraStatusPanel();
            Infragistics.Win.UltraWinStatusBar.UltraStatusPanel ultraStatusPanel12 = new Infragistics.Win.UltraWinStatusBar.UltraStatusPanel();
            Infragistics.Win.UltraWinStatusBar.UltraStatusPanel ultraStatusPanel13 = new Infragistics.Win.UltraWinStatusBar.UltraStatusPanel();
            Infragistics.Win.UltraWinStatusBar.UltraStatusPanel ultraStatusPanel14 = new Infragistics.Win.UltraWinStatusBar.UltraStatusPanel();
            Infragistics.Win.UltraWinStatusBar.UltraStatusPanel ultraStatusPanel15 = new Infragistics.Win.UltraWinStatusBar.UltraStatusPanel();
            Infragistics.Win.UltraWinStatusBar.UltraStatusPanel ultraStatusPanel16 = new Infragistics.Win.UltraWinStatusBar.UltraStatusPanel();
            Infragistics.Win.UltraWinStatusBar.UltraStatusPanel ultraStatusPanel17 = new Infragistics.Win.UltraWinStatusBar.UltraStatusPanel();
            Infragistics.Win.UltraWinStatusBar.UltraStatusPanel ultraStatusPanel18 = new Infragistics.Win.UltraWinStatusBar.UltraStatusPanel();
            Infragistics.Win.UltraWinStatusBar.UltraStatusPanel ultraStatusPanel19 = new Infragistics.Win.UltraWinStatusBar.UltraStatusPanel();
            Infragistics.Win.UltraWinStatusBar.UltraStatusPanel ultraStatusPanel20 = new Infragistics.Win.UltraWinStatusBar.UltraStatusPanel();
            Infragistics.Win.Appearance appearance22 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance23 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.UltraToolbar ultraToolbar1 = new Infragistics.Win.UltraWinToolbars.UltraToolbar("POS");
            Infragistics.Win.UltraWinToolbars.PopupMenuTool popupMenuTool1 = new Infragistics.Win.UltraWinToolbars.PopupMenuTool("POS_Terminal");
            Infragistics.Win.UltraWinToolbars.PopupMenuTool popupMenuTool2 = new Infragistics.Win.UltraWinToolbars.PopupMenuTool("Inventory_Management");
            Infragistics.Win.UltraWinToolbars.PopupMenuTool popupMenuTool3 = new Infragistics.Win.UltraWinToolbars.PopupMenuTool("Administrative_Functions");
            Infragistics.Win.UltraWinToolbars.PopupMenuTool popupMenuTool19 = new Infragistics.Win.UltraWinToolbars.PopupMenuTool("Time Sheet");
            Infragistics.Win.UltraWinToolbars.PopupMenuTool popupMenuTool4 = new Infragistics.Win.UltraWinToolbars.PopupMenuTool("LookandFeel");
            Infragistics.Win.UltraWinToolbars.PopupMenuTool popupMenuTool26 = new Infragistics.Win.UltraWinToolbars.PopupMenuTool("WindowCollection");
            Infragistics.Win.UltraWinToolbars.PopupMenuTool popupMenuTool21 = new Infragistics.Win.UltraWinToolbars.PopupMenuTool("Help");
            Infragistics.Win.Appearance appearance24 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.UltraToolbar ultraToolbar2 = new Infragistics.Win.UltraWinToolbars.UltraToolbar("tlbPOSTerminal");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool1 = new Infragistics.Win.UltraWinToolbars.ButtonTool("TSales");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool2 = new Infragistics.Win.UltraWinToolbars.ButtonTool("TPay_Out");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool3 = new Infragistics.Win.UltraWinToolbars.ButtonTool("TCustomers");
            Infragistics.Win.Appearance appearance25 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.UltraToolbar ultraToolbar3 = new Infragistics.Win.UltraWinToolbars.UltraToolbar("tlbInventoryManagement");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool4 = new Infragistics.Win.UltraWinToolbars.ButtonTool("TItemFile");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool5 = new Infragistics.Win.UltraWinToolbars.ButtonTool("tPhysical_Inventory");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool6 = new Infragistics.Win.UltraWinToolbars.ButtonTool("TInventory_Recieved");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool7 = new Infragistics.Win.UltraWinToolbars.ButtonTool("TPurchase_Order");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool8 = new Infragistics.Win.UltraWinToolbars.ButtonTool("TVendors");
            Infragistics.Win.Appearance appearance26 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.UltraToolbar ultraToolbar4 = new Infragistics.Win.UltraWinToolbars.UltraToolbar("tlbAdministrativeFunction");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool9 = new Infragistics.Win.UltraWinToolbars.ButtonTool("TDepartments");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool10 = new Infragistics.Win.UltraWinToolbars.ButtonTool("TTax_Codes");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool11 = new Infragistics.Win.UltraWinToolbars.ButtonTool("TFunction_Keys");
            Infragistics.Win.Appearance appearance27 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.PopupMenuTool popupMenuTool6 = new Infragistics.Win.UltraWinToolbars.PopupMenuTool("POS_Terminal");
            Infragistics.Win.Appearance appearance28 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool12 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Sales");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool13 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Pay_Out");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool14 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Customers");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool254 = new Infragistics.Win.UltraWinToolbars.ButtonTool("RecoveryTransaction");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool255 = new Infragistics.Win.UltraWinToolbars.ButtonTool("RxRecoveryUpdate");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool95 = new Infragistics.Win.UltraWinToolbars.ButtonTool("CreditCardProfiles");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool100 = new Infragistics.Win.UltraWinToolbars.ButtonTool("TransactionFee");
            Infragistics.Win.UltraWinToolbars.PopupMenuTool popupMenuTool7 = new Infragistics.Win.UltraWinToolbars.PopupMenuTool("Inventory_Management");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool15 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Inventory_Recieved");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool16 = new Infragistics.Win.UltraWinToolbars.ButtonTool("ViewInventoryRecieved");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool17 = new Infragistics.Win.UltraWinToolbars.ButtonTool("InvTransType");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool18 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Physical_Inventory");
            Infragistics.Win.UltraWinToolbars.PopupMenuTool popupMenuTool8 = new Infragistics.Win.UltraWinToolbars.PopupMenuTool("Items");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool19 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Vendors");
            Infragistics.Win.UltraWinToolbars.PopupMenuTool popupMenuTool5 = new Infragistics.Win.UltraWinToolbars.PopupMenuTool("PO");
            Infragistics.Win.UltraWinToolbars.PopupMenuTool popupMenuTool9 = new Infragistics.Win.UltraWinToolbars.PopupMenuTool("InventoryReports");
            Infragistics.Win.UltraWinToolbars.PopupMenuTool popupMenuTool10 = new Infragistics.Win.UltraWinToolbars.PopupMenuTool("Administrative_Functions");
            Infragistics.Win.Appearance appearance29 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.PopupMenuTool popupMenuTool27 = new Infragistics.Win.UltraWinToolbars.PopupMenuTool("User_Setup");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool24 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Departments");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool25 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Tax_Codes");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool26 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Function_Keys");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool27 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Close_Station");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool28 = new Infragistics.Win.UltraWinToolbars.ButtonTool("ViewStationClose");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool29 = new Infragistics.Win.UltraWinToolbars.ButtonTool("End_Of_Day");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool30 = new Infragistics.Win.UltraWinToolbars.ButtonTool("ViewEndOfDay");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool31 = new Infragistics.Win.UltraWinToolbars.ButtonTool("ViewPOSTrans");
            Infragistics.Win.UltraWinToolbars.PopupMenuTool popupMenuTool11 = new Infragistics.Win.UltraWinToolbars.PopupMenuTool("ManagementReports");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool32 = new Infragistics.Win.UltraWinToolbars.ButtonTool("MyStore");
            Infragistics.Win.UltraWinToolbars.PopupMenuTool popupMenuTool17 = new Infragistics.Win.UltraWinToolbars.PopupMenuTool("Customer Loyalty");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool206 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Manage Notes");
            Infragistics.Win.UltraWinToolbars.PopupMenuTool popupMenuTool23 = new Infragistics.Win.UltraWinToolbars.PopupMenuTool("PayoutFile");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool218 = new Infragistics.Win.UltraWinToolbars.ButtonTool("ViewPOSTransColorScheme");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool220 = new Infragistics.Win.UltraWinToolbars.ButtonTool("PayType");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool228 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Coupon");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool235 = new Infragistics.Win.UltraWinToolbars.ButtonTool("CheckForUpdates");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool236 = new Infragistics.Win.UltraWinToolbars.ButtonTool("StationSettings");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool248 = new Infragistics.Win.UltraWinToolbars.ButtonTool("ReconciliationDeliveryReport");
            Infragistics.Win.UltraWinToolbars.PopupMenuTool popupMenuTool29 = new Infragistics.Win.UltraWinToolbars.PopupMenuTool("StoreCredit");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool252 = new Infragistics.Win.UltraWinToolbars.ButtonTool("ProcessEvertecSettlement");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool258 = new Infragistics.Win.UltraWinToolbars.ButtonTool("ViewAuditTrail");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool259 = new Infragistics.Win.UltraWinToolbars.ButtonTool("ViewNoSaleTransaction");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool264 = new Infragistics.Win.UltraWinToolbars.ButtonTool("ScheduledTask");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool33 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Sales");
            Infragistics.Win.Appearance appearance30 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance31 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool34 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Sale_Returns");
            Infragistics.Win.Appearance appearance32 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool35 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Pay_Out");
            Infragistics.Win.Appearance appearance33 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool36 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Inventory_Recieved");
            Infragistics.Win.Appearance appearance34 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool37 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Purchase_Order");
            Infragistics.Win.Appearance appearance35 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool38 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Departments");
            Infragistics.Win.Appearance appearance36 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool39 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Tax_Codes");
            Infragistics.Win.Appearance appearance37 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool40 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Vendors");
            Infragistics.Win.Appearance appearance38 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool42 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Function_Keys");
            Infragistics.Win.Appearance appearance39 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool43 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Close_Station");
            Infragistics.Win.Appearance appearance40 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance41 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool44 = new Infragistics.Win.UltraWinToolbars.ButtonTool("End_Of_Day");
            Infragistics.Win.Appearance appearance42 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool45 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Lock");
            Infragistics.Win.Appearance appearance43 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool46 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Log_Off");
            Infragistics.Win.Appearance appearance44 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool47 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Customers");
            Infragistics.Win.Appearance appearance45 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool48 = new Infragistics.Win.UltraWinToolbars.ButtonTool("ItemCompanion");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool49 = new Infragistics.Win.UltraWinToolbars.ButtonTool("TSales");
            Infragistics.Win.Appearance appearance46 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance47 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool50 = new Infragistics.Win.UltraWinToolbars.ButtonTool("TSale_Returns");
            Infragistics.Win.Appearance appearance48 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool51 = new Infragistics.Win.UltraWinToolbars.ButtonTool("TPay_Out");
            Infragistics.Win.Appearance appearance49 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool52 = new Infragistics.Win.UltraWinToolbars.ButtonTool("TCustomers");
            Infragistics.Win.Appearance appearance50 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool53 = new Infragistics.Win.UltraWinToolbars.ButtonTool("TPurchase_Order");
            Infragistics.Win.Appearance appearance51 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool54 = new Infragistics.Win.UltraWinToolbars.ButtonTool("TInventory_Recieved");
            Infragistics.Win.Appearance appearance52 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool55 = new Infragistics.Win.UltraWinToolbars.ButtonTool("TItemFile");
            Infragistics.Win.Appearance appearance53 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance54 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool56 = new Infragistics.Win.UltraWinToolbars.ButtonTool("TVendors");
            Infragistics.Win.Appearance appearance55 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool57 = new Infragistics.Win.UltraWinToolbars.ButtonTool("TDepartments");
            Infragistics.Win.Appearance appearance56 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool58 = new Infragistics.Win.UltraWinToolbars.ButtonTool("TTax_Codes");
            Infragistics.Win.Appearance appearance57 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool59 = new Infragistics.Win.UltraWinToolbars.ButtonTool("TUser_Setup");
            Infragistics.Win.Appearance appearance58 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool60 = new Infragistics.Win.UltraWinToolbars.ButtonTool("TFunction_Keys");
            Infragistics.Win.Appearance appearance59 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance60 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool61 = new Infragistics.Win.UltraWinToolbars.ButtonTool("TClose_Station");
            Infragistics.Win.Appearance appearance61 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool62 = new Infragistics.Win.UltraWinToolbars.ButtonTool("TEnd_Of_Day");
            Infragistics.Win.Appearance appearance62 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool63 = new Infragistics.Win.UltraWinToolbars.ButtonTool("TLock");
            Infragistics.Win.Appearance appearance63 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool64 = new Infragistics.Win.UltraWinToolbars.ButtonTool("TLog_Off");
            Infragistics.Win.Appearance appearance64 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.TaskPaneTool taskPaneTool1 = new Infragistics.Win.UltraWinToolbars.TaskPaneTool("TaskPaneTool2");
            Infragistics.Win.UltraWinToolbars.TaskPaneTool taskPaneTool2 = new Infragistics.Win.UltraWinToolbars.TaskPaneTool("TaskPaneTool3");
            Infragistics.Win.UltraWinToolbars.TaskPaneTool taskPaneTool3 = new Infragistics.Win.UltraWinToolbars.TaskPaneTool("TaskPaneTool4");
            Infragistics.Win.UltraWinToolbars.TaskPaneTool taskPaneTool4 = new Infragistics.Win.UltraWinToolbars.TaskPaneTool("TaskPaneTool5");
            Infragistics.Win.UltraWinToolbars.TaskPaneTool taskPaneTool5 = new Infragistics.Win.UltraWinToolbars.TaskPaneTool("TaskPaneTool6");
            Infragistics.Win.UltraWinToolbars.TaskPaneTool taskPaneTool6 = new Infragistics.Win.UltraWinToolbars.TaskPaneTool("TaskPaneTool7");
            Infragistics.Win.UltraWinToolbars.PopupMenuTool popupMenuTool12 = new Infragistics.Win.UltraWinToolbars.PopupMenuTool("LookandFeel");
            Infragistics.Win.UltraWinToolbars.StateButtonTool stateButtonTool1 = new Infragistics.Win.UltraWinToolbars.StateButtonTool("Preferences", "");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool146 = new Infragistics.Win.UltraWinToolbars.ButtonTool("IIASItemFileListing");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool65 = new Infragistics.Win.UltraWinToolbars.ButtonTool("ManageMessages");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool211 = new Infragistics.Win.UltraWinToolbars.ButtonTool("PSEItemFileListing");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool66 = new Infragistics.Win.UltraWinToolbars.ButtonTool("ChangePassword");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool67 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Lock");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool204 = new Infragistics.Win.UltraWinToolbars.ButtonTool("SystemNote");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool68 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Exit");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool69 = new Infragistics.Win.UltraWinToolbars.ButtonTool("ViewPOSTrans");
            Infragistics.Win.Appearance appearance65 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.PopupMenuTool popupMenuTool13 = new Infragistics.Win.UltraWinToolbars.PopupMenuTool("InventoryReports");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool70 = new Infragistics.Win.UltraWinToolbars.ButtonTool("ItemFileListing");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool71 = new Infragistics.Win.UltraWinToolbars.ButtonTool("VendorFileListing");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool72 = new Infragistics.Win.UltraWinToolbars.ButtonTool("InventoryStatusReport");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool73 = new Infragistics.Win.UltraWinToolbars.ButtonTool("RInventoryReceived");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool74 = new Infragistics.Win.UltraWinToolbars.ButtonTool("ItemRe-Order");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool75 = new Infragistics.Win.UltraWinToolbars.ButtonTool("PurchaseOrderReport");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool76 = new Infragistics.Win.UltraWinToolbars.ButtonTool("PhysicalInventorySheet");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool77 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Physical Inv. History");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool78 = new Infragistics.Win.UltraWinToolbars.ButtonTool("ItemLabelReport");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool79 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Item Price Change Rport");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool20 = new Infragistics.Win.UltraWinToolbars.ButtonTool("ItemList By Vendor");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool156 = new Infragistics.Win.UltraWinToolbars.ButtonTool("IIASItemFileListing");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool163 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Item Price Change and Label Report");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool97 = new Infragistics.Win.UltraWinToolbars.ButtonTool("InventoryShrinkageReport");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool80 = new Infragistics.Win.UltraWinToolbars.ButtonTool("ItemFileListing");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool81 = new Infragistics.Win.UltraWinToolbars.ButtonTool("VendorFileListing");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool82 = new Infragistics.Win.UltraWinToolbars.ButtonTool("InventoryStatusReport");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool83 = new Infragistics.Win.UltraWinToolbars.ButtonTool("PhysicalInventorySheet");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool84 = new Infragistics.Win.UltraWinToolbars.ButtonTool("RInventoryReceived");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool85 = new Infragistics.Win.UltraWinToolbars.ButtonTool("ItemRe-Order");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool86 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Detail Transaction Report");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool87 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Summary Sales Report By User ID");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool88 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Sales Report By Item");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool89 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Sales Report By Department");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool90 = new Infragistics.Win.UltraWinToolbars.ButtonTool("ViewStationClose");
            Infragistics.Win.Appearance appearance66 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool91 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Sales Report By Payment");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool92 = new Infragistics.Win.UltraWinToolbars.ButtonTool("StationCloseSummary");
            Infragistics.Win.UltraWinToolbars.PopupMenuTool popupMenuTool14 = new Infragistics.Win.UltraWinToolbars.PopupMenuTool("ManagementReports");
            Infragistics.Win.UltraWinToolbars.PopupMenuTool popupMenuTool31 = new Infragistics.Win.UltraWinToolbars.PopupMenuTool("TransactionReports");
            Infragistics.Win.UltraWinToolbars.PopupMenuTool popupMenuTool33 = new Infragistics.Win.UltraWinToolbars.PopupMenuTool("SalesReports");
            Infragistics.Win.UltraWinToolbars.PopupMenuTool popupMenuTool35 = new Infragistics.Win.UltraWinToolbars.PopupMenuTool("ItemReports");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool173 = new Infragistics.Win.UltraWinToolbars.ButtonTool("PriceOverriddenReport");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool93 = new Infragistics.Win.UltraWinToolbars.ButtonTool("TaxOverrideReport");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool98 = new Infragistics.Win.UltraWinToolbars.ButtonTool("StationCloseSummary");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool239 = new Infragistics.Win.UltraWinToolbars.ButtonTool("EODSummary");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool183 = new Infragistics.Win.UltraWinToolbars.ButtonTool("StationCloseCash");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool102 = new Infragistics.Win.UltraWinToolbars.ButtonTool("PayoutDetails");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool157 = new Infragistics.Win.UltraWinToolbars.ButtonTool("CustomerList");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool165 = new Infragistics.Win.UltraWinToolbars.ButtonTool("DeliveryListReport");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool191 = new Infragistics.Win.UltraWinToolbars.ButtonTool("RxCheckout");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool216 = new Infragistics.Win.UltraWinToolbars.ButtonTool("ROADetails");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool244 = new Infragistics.Win.UltraWinToolbars.ButtonTool("CouponReport");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool105 = new Infragistics.Win.UltraWinToolbars.ButtonTool("TopSellingProducts");
            Infragistics.Win.UltraWinToolbars.StateButtonTool stateButtonTool2 = new Infragistics.Win.UltraWinToolbars.StateButtonTool("Preferences", "");
            Infragistics.Win.Appearance appearance67 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance68 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool106 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Sales Tax Summary");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool107 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Physical_Inventory");
            Infragistics.Win.Appearance appearance69 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool108 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Physical Inv. History");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool109 = new Infragistics.Win.UltraWinToolbars.ButtonTool("tPhysical_Inventory");
            Infragistics.Win.Appearance appearance70 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool110 = new Infragistics.Win.UltraWinToolbars.ButtonTool("ProductivityReport");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool111 = new Infragistics.Win.UltraWinToolbars.ButtonTool("ViewEndOfDay");
            Infragistics.Win.Appearance appearance71 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool112 = new Infragistics.Win.UltraWinToolbars.ButtonTool("PurchaseOrderReport");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool113 = new Infragistics.Win.UltraWinToolbars.ButtonTool("ItemLabelReport");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool114 = new Infragistics.Win.UltraWinToolbars.ButtonTool("ViewPurchaseOrder");
            Infragistics.Win.Appearance appearance72 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool115 = new Infragistics.Win.UltraWinToolbars.ButtonTool("ViewInventoryRecieved");
            Infragistics.Win.Appearance appearance73 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.PopupMenuTool popupMenuTool15 = new Infragistics.Win.UltraWinToolbars.PopupMenuTool("Help");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool116 = new Infragistics.Win.UltraWinToolbars.ButtonTool("About");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool200 = new Infragistics.Win.UltraWinToolbars.ButtonTool("OnlineHelp");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool232 = new Infragistics.Win.UltraWinToolbars.ButtonTool("WhatsNew");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool117 = new Infragistics.Win.UltraWinToolbars.ButtonTool("About");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool118 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Exit");
            Infragistics.Win.Appearance appearance74 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance75 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool119 = new Infragistics.Win.UltraWinToolbars.ButtonTool("POAcknowledgement");
            Infragistics.Win.UltraWinToolbars.PopupMenuTool popupMenuTool16 = new Infragistics.Win.UltraWinToolbars.PopupMenuTool("Items");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool120 = new Infragistics.Win.UltraWinToolbars.ButtonTool("ItemFile");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool121 = new Infragistics.Win.UltraWinToolbars.ButtonTool("AutoPriceUpdate");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool140 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Last Update");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool154 = new Infragistics.Win.UltraWinToolbars.ButtonTool("ItemMonitorCategory");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool161 = new Infragistics.Win.UltraWinToolbars.ButtonTool("WarningMessages");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool175 = new Infragistics.Win.UltraWinToolbars.ButtonTool("ItemDepartment");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool181 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Set Selling Price");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool241 = new Infragistics.Win.UltraWinToolbars.ButtonTool("ItemComboPricing");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool262 = new Infragistics.Win.UltraWinToolbars.ButtonTool("SetItemTax");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool123 = new Infragistics.Win.UltraWinToolbars.ButtonTool("ItemFile");
            Infragistics.Win.Appearance appearance76 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance77 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool124 = new Infragistics.Win.UltraWinToolbars.ButtonTool("AutoPriceUpdate");
            Infragistics.Win.Appearance appearance78 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance79 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool125 = new Infragistics.Win.UltraWinToolbars.ButtonTool("ManualPriceUpdate");
            Infragistics.Win.Appearance appearance80 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance81 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool126 = new Infragistics.Win.UltraWinToolbars.ButtonTool("MyStore");
            Infragistics.Win.Appearance appearance82 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance83 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool127 = new Infragistics.Win.UltraWinToolbars.ButtonTool("PayoutDetails");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool128 = new Infragistics.Win.UltraWinToolbars.ButtonTool("ChangePassword");
            Infragistics.Win.Appearance appearance84 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance85 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool129 = new Infragistics.Win.UltraWinToolbars.ButtonTool("InvTransType");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool130 = new Infragistics.Win.UltraWinToolbars.ButtonTool("ManageMessages");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool131 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Item Price Change Rport");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool132 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Sales Comparison");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool133 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Cost Analysis Report");
            Infragistics.Win.UltraWinToolbars.PopupMenuTool popupMenuTool18 = new Infragistics.Win.UltraWinToolbars.PopupMenuTool("PO");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool134 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Add/Edit Purchase Order");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool136 = new Infragistics.Win.UltraWinToolbars.ButtonTool("View Purchase Order");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool138 = new Infragistics.Win.UltraWinToolbars.ButtonTool("PO Acknowledgement");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool135 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Add/Edit Purchase Order");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool137 = new Infragistics.Win.UltraWinToolbars.ButtonTool("View Purchase Order");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool139 = new Infragistics.Win.UltraWinToolbars.ButtonTool("PO Acknowledgement");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool21 = new Infragistics.Win.UltraWinToolbars.ButtonTool("ItemList By Vendor");
            Infragistics.Win.UltraWinToolbars.PopupMenuTool popupMenuTool20 = new Infragistics.Win.UltraWinToolbars.PopupMenuTool("Time Sheet");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool22 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Manage TimeSheet");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool142 = new Infragistics.Win.UltraWinToolbars.ButtonTool("CreateTimesheet");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool144 = new Infragistics.Win.UltraWinToolbars.ButtonTool("TimesheetReport");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool122 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Manage TimeSheet");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool141 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Last Update");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool143 = new Infragistics.Win.UltraWinToolbars.ButtonTool("CreateTimesheet");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool145 = new Infragistics.Win.UltraWinToolbars.ButtonTool("TimesheetReport");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool147 = new Infragistics.Win.UltraWinToolbars.ButtonTool("IIASItemFileListing");
            Infragistics.Win.Appearance appearance86 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance87 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool149 = new Infragistics.Win.UltraWinToolbars.ButtonTool("IIASTransSummary");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool151 = new Infragistics.Win.UltraWinToolbars.ButtonTool("IIASPaymentTransaction");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool153 = new Infragistics.Win.UltraWinToolbars.ButtonTool("SalesByCustomer");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool155 = new Infragistics.Win.UltraWinToolbars.ButtonTool("ItemMonitorCategory");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool158 = new Infragistics.Win.UltraWinToolbars.ButtonTool("CustomerList");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool160 = new Infragistics.Win.UltraWinToolbars.ButtonTool("SalesByItemMonitoringCategory");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool167 = new Infragistics.Win.UltraWinToolbars.ButtonTool("SalesPseudoephedrineLogs"); //PRIMEPOS-3360
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool162 = new Infragistics.Win.UltraWinToolbars.ButtonTool("WarningMessages");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool164 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Item Price Change and Label Report");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool166 = new Infragistics.Win.UltraWinToolbars.ButtonTool("DeliveryListReport");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool168 = new Infragistics.Win.UltraWinToolbars.ButtonTool("SalesByInsurance");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool170 = new Infragistics.Win.UltraWinToolbars.ButtonTool("SaleTaxControl");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool172 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Sales Comparison By Dept.");
            Infragistics.Win.UltraWinToolbars.ComboBoxTool comboBoxTool2 = new Infragistics.Win.UltraWinToolbars.ComboBoxTool("Price Overridden Report");
            Infragistics.Win.ValueList valueList1 = new Infragistics.Win.ValueList(0);
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool174 = new Infragistics.Win.UltraWinToolbars.ButtonTool("PriceOverriddenReport");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool176 = new Infragistics.Win.UltraWinToolbars.ButtonTool("ItemDepartment");
            Infragistics.Win.UltraWinToolbars.PopupMenuTool popupMenuTool22 = new Infragistics.Win.UltraWinToolbars.PopupMenuTool("Customer Loyalty");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool177 = new Infragistics.Win.UltraWinToolbars.ButtonTool("PointsRewardTier");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool178 = new Infragistics.Win.UltraWinToolbars.ButtonTool("RegisterCLCards");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool222 = new Infragistics.Win.UltraWinToolbars.ButtonTool("ViewCLSummary");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool179 = new Infragistics.Win.UltraWinToolbars.ButtonTool("PointsRewardTier");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool180 = new Infragistics.Win.UltraWinToolbars.ButtonTool("RegisterCLCards");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool182 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Set Selling Price");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool184 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Transaction Time Report");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool186 = new Infragistics.Win.UltraWinToolbars.ButtonTool("TransactionTimeReport");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool187 = new Infragistics.Win.UltraWinToolbars.ButtonTool("TransactionTime");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool188 = new Infragistics.Win.UltraWinToolbars.ButtonTool("SalesCompare");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool190 = new Infragistics.Win.UltraWinToolbars.ButtonTool("StnCloseCash");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool192 = new Infragistics.Win.UltraWinToolbars.ButtonTool("ItemConsumtionCompare");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool193 = new Infragistics.Win.UltraWinToolbars.ButtonTool("ItemConsumptionCompare");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool185 = new Infragistics.Win.UltraWinToolbars.ButtonTool("StationCloseCash");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool194 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Item SalesPerformance");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool197 = new Infragistics.Win.UltraWinToolbars.ButtonTool("ItemSalesPerformance");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool198 = new Infragistics.Win.UltraWinToolbars.ButtonTool("ItemSaleHistoricalCompare");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool199 = new Infragistics.Win.UltraWinToolbars.ButtonTool("RxCheckout");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool201 = new Infragistics.Win.UltraWinToolbars.ButtonTool("SystemNotes");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool203 = new Infragistics.Win.UltraWinToolbars.ButtonTool("&SystemNotes");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool205 = new Infragistics.Win.UltraWinToolbars.ButtonTool("SystemNote");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool202 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Manage Notes ");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool207 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Manage Notes");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool208 = new Infragistics.Win.UltraWinToolbars.ButtonTool("AdvSearchItem");
            Infragistics.Win.UltraWinToolbars.PopupMenuTool popupMenuTool24 = new Infragistics.Win.UltraWinToolbars.PopupMenuTool("WindowCollection");
            Infragistics.Win.UltraWinToolbars.MdiWindowListTool mdiWindowListTool1 = new Infragistics.Win.UltraWinToolbars.MdiWindowListTool("WindowsList");
            Infragistics.Win.UltraWinToolbars.MdiWindowListTool mdiWindowListTool2 = new Infragistics.Win.UltraWinToolbars.MdiWindowListTool("WindowsList");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool209 = new Infragistics.Win.UltraWinToolbars.ButtonTool("OnlineHelp");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool213 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Payout");
            Infragistics.Win.UltraWinToolbars.PopupMenuTool popupMenuTool25 = new Infragistics.Win.UltraWinToolbars.PopupMenuTool("PayoutFile");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool215 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Pay_Out");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool210 = new Infragistics.Win.UltraWinToolbars.ButtonTool("PayOutCategory");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool212 = new Infragistics.Win.UltraWinToolbars.ButtonTool("PayOutCategory");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool217 = new Infragistics.Win.UltraWinToolbars.ButtonTool("ROADetails");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool219 = new Infragistics.Win.UltraWinToolbars.ButtonTool("ViewPOSTransColorScheme");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool221 = new Infragistics.Win.UltraWinToolbars.ButtonTool("PayType");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool223 = new Infragistics.Win.UltraWinToolbars.ButtonTool("ViewCLSummary");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool225 = new Infragistics.Win.UltraWinToolbars.ButtonTool("SalesSummaryByVendor");
            Infragistics.Win.UltraWinToolbars.PopupMenuTool popupMenuTool28 = new Infragistics.Win.UltraWinToolbars.PopupMenuTool("User_Setup");
            Infragistics.Win.Appearance appearance88 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance89 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool23 = new Infragistics.Win.UltraWinToolbars.ButtonTool("UserSetUp");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool41 = new Infragistics.Win.UltraWinToolbars.ButtonTool("UserGroup");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool227 = new Infragistics.Win.UltraWinToolbars.ButtonTool("UserGroup");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool226 = new Infragistics.Win.UltraWinToolbars.ButtonTool("UserSetUp");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool229 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Coupon");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool231 = new Infragistics.Win.UltraWinToolbars.ButtonTool("DeadStockReport");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool233 = new Infragistics.Win.UltraWinToolbars.ButtonTool("ProcessOnHoldPO");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool234 = new Infragistics.Win.UltraWinToolbars.ButtonTool("WhatsNew");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool237 = new Infragistics.Win.UltraWinToolbars.ButtonTool("CheckForUpdates");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool238 = new Infragistics.Win.UltraWinToolbars.ButtonTool("StationSettings");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool240 = new Infragistics.Win.UltraWinToolbars.ButtonTool("EODSummary");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool242 = new Infragistics.Win.UltraWinToolbars.ButtonTool("DepartmentList");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool214 = new Infragistics.Win.UltraWinToolbars.ButtonTool("PSEItemFileListing");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool243 = new Infragistics.Win.UltraWinToolbars.ButtonTool("ItemComboPricing");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool245 = new Infragistics.Win.UltraWinToolbars.ButtonTool("CouponReport");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool247 = new Infragistics.Win.UltraWinToolbars.ButtonTool("OnHoldTransactionDetailReport");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool249 = new Infragistics.Win.UltraWinToolbars.ButtonTool("ReconciliationDeliveryReport");
            Infragistics.Win.UltraWinToolbars.ComboBoxTool comboBoxTool3 = new Infragistics.Win.UltraWinToolbars.ComboBoxTool("Store Credit");
            Infragistics.Win.ValueList valueList2 = new Infragistics.Win.ValueList(0);
            Infragistics.Win.UltraWinToolbars.PopupMenuTool popupMenuTool30 = new Infragistics.Win.UltraWinToolbars.PopupMenuTool("StoreCredit");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool250 = new Infragistics.Win.UltraWinToolbars.ButtonTool("ViewStoreCreditSummary");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool251 = new Infragistics.Win.UltraWinToolbars.ButtonTool("ViewStoreCreditSummary");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool253 = new Infragistics.Win.UltraWinToolbars.ButtonTool("ProcessEvertecSettlement");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool256 = new Infragistics.Win.UltraWinToolbars.ButtonTool("RecoveryTransaction");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool257 = new Infragistics.Win.UltraWinToolbars.ButtonTool("RxRecoveryUpdate");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool260 = new Infragistics.Win.UltraWinToolbars.ButtonTool("ViewAuditTrail");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool261 = new Infragistics.Win.UltraWinToolbars.ButtonTool("ViewNoSaleTransaction");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool263 = new Infragistics.Win.UltraWinToolbars.ButtonTool("SetItemTax");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool265 = new Infragistics.Win.UltraWinToolbars.ButtonTool("ScheduledTask");
            Infragistics.Win.UltraWinToolbars.ComboBoxTool comboBoxTool4 = new Infragistics.Win.UltraWinToolbars.ComboBoxTool("TransactionDetail");
            Infragistics.Win.ValueList valueList3 = new Infragistics.Win.ValueList(0);
            Infragistics.Win.UltraWinToolbars.ComboBoxTool comboBoxTool6 = new Infragistics.Win.UltraWinToolbars.ComboBoxTool("ComboBoxTool1");
            Infragistics.Win.ValueList valueList4 = new Infragistics.Win.ValueList(0);
            Infragistics.Win.UltraWinToolbars.PopupMenuTool popupMenuTool32 = new Infragistics.Win.UltraWinToolbars.PopupMenuTool("TransactionReports");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool268 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Detail Transaction Report");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool269 = new Infragistics.Win.UltraWinToolbars.ButtonTool("OnHoldTransactionDetailReport");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool270 = new Infragistics.Win.UltraWinToolbars.ButtonTool("ProductivityReport");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool271 = new Infragistics.Win.UltraWinToolbars.ButtonTool("IIASTransSummary");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool272 = new Infragistics.Win.UltraWinToolbars.ButtonTool("IIASPaymentTransaction");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool273 = new Infragistics.Win.UltraWinToolbars.ButtonTool("TransactionTime");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool103 = new Infragistics.Win.UltraWinToolbars.ButtonTool("TransactionFeeReport");
            Infragistics.Win.UltraWinToolbars.PopupMenuTool popupMenuTool34 = new Infragistics.Win.UltraWinToolbars.PopupMenuTool("SalesReports");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool274 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Summary Sales Report By User ID");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool275 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Sales Report By Item");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool276 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Sales Report By Department");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool277 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Sales Report By Payment");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool278 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Sales Tax Summary");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool281 = new Infragistics.Win.UltraWinToolbars.ButtonTool("SalesByCustomer");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool282 = new Infragistics.Win.UltraWinToolbars.ButtonTool("SalesByItemMonitoringCategory");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool291 = new Infragistics.Win.UltraWinToolbars.ButtonTool("SalesPseudoephedrineLogs"); //PRIMEPOS-3360
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool280 = new Infragistics.Win.UltraWinToolbars.ButtonTool("SalesByInsurance");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool279 = new Infragistics.Win.UltraWinToolbars.ButtonTool("SaleTaxControl");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool284 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Sales Comparison By Dept.");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool283 = new Infragistics.Win.UltraWinToolbars.ButtonTool("SalesSummaryByVendor");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool285 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Sales Comparison");
            Infragistics.Win.UltraWinToolbars.PopupMenuTool popupMenuTool36 = new Infragistics.Win.UltraWinToolbars.PopupMenuTool("ItemReports");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool286 = new Infragistics.Win.UltraWinToolbars.ButtonTool("TopSellingProducts");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool287 = new Infragistics.Win.UltraWinToolbars.ButtonTool("ItemSalesPerformance");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool288 = new Infragistics.Win.UltraWinToolbars.ButtonTool("ItemSaleHistoricalCompare");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool289 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Cost Analysis Report");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool290 = new Infragistics.Win.UltraWinToolbars.ButtonTool("DeadStockReport");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool94 = new Infragistics.Win.UltraWinToolbars.ButtonTool("TaxOverrideReport");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool96 = new Infragistics.Win.UltraWinToolbars.ButtonTool("CreditCardProfiles");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool99 = new Infragistics.Win.UltraWinToolbars.ButtonTool("InventoryShrinkageReport");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool101 = new Infragistics.Win.UltraWinToolbars.ButtonTool("TransactionFee");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool104 = new Infragistics.Win.UltraWinToolbars.ButtonTool("TransactionFeeReport");
            this.ultraExplorerBar1 = new Infragistics.Win.UltraWinExplorerBar.UltraExplorerBar();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.ultraStatusBar1 = new Infragistics.Win.UltraWinStatusBar.UltraStatusBar();
            this.ultraStatusBar = new Infragistics.Win.UltraWinStatusBar.UltraStatusBar();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.imageList2 = new System.Windows.Forms.ImageList(this.components);
            this.ultraStatusBarVendor = new Infragistics.Win.UltraWinStatusBar.UltraStatusBar();
            this.cntMenuPOStatus = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.Expired = new System.Windows.Forms.ToolStripMenuItem();
            this.Error = new System.Windows.Forms.ToolStripMenuItem();
            this.Overdue = new System.Windows.Forms.ToolStripMenuItem();
            this.Acknowledged = new System.Windows.Forms.ToolStripMenuItem();
            this.DeliveryReceived = new System.Windows.Forms.ToolStripMenuItem();
            this.maxAttemptToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.InComplete = new System.Windows.Forms.ToolStripMenuItem();
            this._frmMain_Toolbars_Dock_Area_Right = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
            this.ultMenuBar = new Infragistics.Win.UltraWinToolbars.UltraToolbarsManager(this.components);
            this._frmMain_Toolbars_Dock_Area_Left = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
            this._frmMain_Toolbars_Dock_Area_Top = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
            this._frmMain_Toolbars_Dock_Area_Bottom = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
            this.bwCheckAlwaysRunning = new System.ComponentModel.BackgroundWorker();
            this.tmrCheckRunningTasks = new System.Windows.Forms.Timer(this.components);
            this.pictSign = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.ultraExplorerBar1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraStatusBar1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraStatusBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraStatusBarVendor)).BeginInit();
            this.cntMenuPOStatus.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultMenuBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictSign)).BeginInit();
            this.SuspendLayout();
            // 
            // ultraExplorerBar1
            // 
            this.ultraExplorerBar1.AcceptsFocus = Infragistics.Win.DefaultableBoolean.False;
            this.ultraExplorerBar1.AlphaBlendMode = Infragistics.Win.AlphaBlendMode.Standard;
            this.ultraExplorerBar1.AnimationSpeed = Infragistics.Win.UltraWinExplorerBar.AnimationSpeed.Fast;
            this.ultraExplorerBar1.BorderStyle = Infragistics.Win.UIElementBorderStyle.None;
            this.ultraExplorerBar1.Dock = System.Windows.Forms.DockStyle.Right;
            this.ultraExplorerBar1.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold);
            ultraExplorerBarGroup1.Expanded = false;
            ultraExplorerBarItem1.Key = "Sales";
            appearance1.Image = ((object)(resources.GetObject("appearance1.Image")));
            ultraExplorerBarItem1.Settings.AppearancesLarge.Appearance = appearance1;
            ultraExplorerBarItem1.Settings.MaxLines = 2;
            ultraExplorerBarItem1.Text = "POS Transaction (Ctrl+S)";
            ultraExplorerBarItem2.Key = "Pay_Out";
            appearance2.Image = 2;
            ultraExplorerBarItem2.Settings.AppearancesLarge.Appearance = appearance2;
            ultraExplorerBarItem2.Settings.MaxLines = 2;
            ultraExplorerBarItem2.Text = "Pay Out (Ctrl+Y)";
            ultraExplorerBarItem3.Key = "Customers";
            appearance3.Image = 3;
            ultraExplorerBarItem3.Settings.AppearancesLarge.Appearance = appearance3;
            ultraExplorerBarItem3.Settings.MaxLines = 2;
            ultraExplorerBarItem3.Text = "Customer File (Ctrl+M)";
            ultraExplorerBarGroup1.Items.AddRange(new Infragistics.Win.UltraWinExplorerBar.UltraExplorerBarItem[] {
            ultraExplorerBarItem1,
            ultraExplorerBarItem2,
            ultraExplorerBarItem3});
            ultraExplorerBarGroup1.Key = "POS_Terminal";
            ultraExplorerBarGroup1.Settings.BorderStyleItemArea = Infragistics.Win.UIElementBorderStyle.Solid;
            ultraExplorerBarGroup1.Settings.HeaderButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            ultraExplorerBarGroup1.Settings.HeaderVisible = Infragistics.Win.DefaultableBoolean.True;
            ultraExplorerBarGroup1.Settings.UseMnemonics = Infragistics.Win.DefaultableBoolean.True;
            ultraExplorerBarGroup1.Text = "POS Terminal";
            ultraExplorerBarGroup2.Expanded = false;
            ultraExplorerBarItem4.Key = "Purchase_Order";
            appearance4.Image = 4;
            ultraExplorerBarItem4.Settings.AppearancesLarge.Appearance = appearance4;
            ultraExplorerBarItem4.Settings.MaxLines = 2;
            ultraExplorerBarItem4.Text = "Purchase Order (Ctrl+P)";
            ultraExplorerBarItem5.Key = "Inventory_Recieved";
            appearance5.Image = 5;
            ultraExplorerBarItem5.Settings.AppearancesLarge.Appearance = appearance5;
            ultraExplorerBarItem5.Settings.MaxLines = 2;
            ultraExplorerBarItem5.Text = "Inventory Received (Ctrl+N)";
            ultraExplorerBarItem6.Key = "Physical_Inventory";
            appearance6.Image = 20;
            ultraExplorerBarItem6.Settings.AppearancesLarge.Appearance = appearance6;
            ultraExplorerBarItem6.Text = "Physical Inventory (Ctrl+H)";
            ultraExplorerBarItem7.Key = "ItemFile";
            appearance7.Image = 6;
            ultraExplorerBarItem7.Settings.AppearancesLarge.Appearance = appearance7;
            ultraExplorerBarItem7.Settings.MaxLines = 2;
            ultraExplorerBarItem7.Text = "Item File  (Ctrl+I)";
            ultraExplorerBarItem8.Key = "Vendors";
            appearance8.Image = 7;
            ultraExplorerBarItem8.Settings.AppearancesLarge.Appearance = appearance8;
            ultraExplorerBarItem8.Settings.MaxLines = 2;
            ultraExplorerBarItem8.Text = "Vendor File  (Ctrl+W)";
            ultraExplorerBarGroup2.Items.AddRange(new Infragistics.Win.UltraWinExplorerBar.UltraExplorerBarItem[] {
            ultraExplorerBarItem4,
            ultraExplorerBarItem5,
            ultraExplorerBarItem6,
            ultraExplorerBarItem7,
            ultraExplorerBarItem8});
            ultraExplorerBarGroup2.Key = "Inventory_Management";
            ultraExplorerBarGroup2.Settings.UseMnemonics = Infragistics.Win.DefaultableBoolean.True;
            ultraExplorerBarGroup2.Text = "Inventory Management";
            ultraExplorerBarGroup3.Expanded = false;
            ultraExplorerBarItem9.Key = "User_Setup";
            appearance9.Image = 17;
            ultraExplorerBarItem9.Settings.AppearancesLarge.Appearance = appearance9;
            ultraExplorerBarItem9.Settings.MaxLines = 2;
            ultraExplorerBarItem9.Text = "User Setup File (Ctrl+U)";
            ultraExplorerBarItem10.Key = "Departments";
            appearance10.Image = 8;
            ultraExplorerBarItem10.Settings.AppearancesLarge.Appearance = appearance10;
            ultraExplorerBarItem10.Settings.MaxLines = 2;
            ultraExplorerBarItem10.Text = "Department File (Ctrl+D)";
            ultraExplorerBarItem11.Key = "Tax_Codes";
            appearance11.Image = 9;
            ultraExplorerBarItem11.Settings.AppearancesLarge.Appearance = appearance11;
            ultraExplorerBarItem11.Settings.MaxLines = 2;
            ultraExplorerBarItem11.Text = "Tax Code File (Ctrl+T)";
            ultraExplorerBarItem12.Key = "Function_Keys";
            appearance12.Image = 11;
            ultraExplorerBarItem12.Settings.AppearancesLarge.Appearance = appearance12;
            ultraExplorerBarItem12.Settings.MaxLines = 2;
            ultraExplorerBarItem12.Text = "Function Keys (Ctrl+F)";
            ultraExplorerBarItem13.Key = "Close_Station";
            appearance13.Image = 12;
            ultraExplorerBarItem13.Settings.AppearancesLarge.Appearance = appearance13;
            ultraExplorerBarItem13.Settings.MaxLines = 2;
            ultraExplorerBarItem13.Settings.UseMnemonics = Infragistics.Win.DefaultableBoolean.True;
            ultraExplorerBarItem13.Text = "Close Station (Ctrl+G)";
            ultraExplorerBarItem14.Key = "End_Of_Day";
            appearance14.Image = 13;
            ultraExplorerBarItem14.Settings.AppearancesLarge.Appearance = appearance14;
            ultraExplorerBarItem14.Settings.MaxLines = 2;
            ultraExplorerBarItem14.Text = "End Of Day (Ctrl+E)";
            ultraExplorerBarItem15.Key = "Lock";
            appearance15.Image = 14;
            ultraExplorerBarItem15.Settings.AppearancesLarge.Appearance = appearance15;
            ultraExplorerBarItem15.Settings.MaxLines = 2;
            ultraExplorerBarItem15.Text = "Lock Station (Ctrl+L)";
            ultraExplorerBarItem16.Key = "Log_Off";
            appearance16.Image = 15;
            ultraExplorerBarItem16.Settings.AppearancesLarge.Appearance = appearance16;
            ultraExplorerBarItem16.Settings.Enabled = Infragistics.Win.DefaultableBoolean.False;
            ultraExplorerBarItem16.Settings.MaxLines = 2;
            ultraExplorerBarItem16.Text = "Log Off (Ctrl+O)";
            ultraExplorerBarItem16.Visible = false;
            ultraExplorerBarGroup3.Items.AddRange(new Infragistics.Win.UltraWinExplorerBar.UltraExplorerBarItem[] {
            ultraExplorerBarItem9,
            ultraExplorerBarItem10,
            ultraExplorerBarItem11,
            ultraExplorerBarItem12,
            ultraExplorerBarItem13,
            ultraExplorerBarItem14,
            ultraExplorerBarItem15,
            ultraExplorerBarItem16});
            ultraExplorerBarGroup3.Key = "Administrative_Function";
            ultraExplorerBarGroup3.Settings.UseMnemonics = Infragistics.Win.DefaultableBoolean.True;
            ultraExplorerBarGroup3.Text = "Administrative Function";
            this.ultraExplorerBar1.Groups.AddRange(new Infragistics.Win.UltraWinExplorerBar.UltraExplorerBarGroup[] {
            ultraExplorerBarGroup1,
            ultraExplorerBarGroup2,
            ultraExplorerBarGroup3});
            this.ultraExplorerBar1.GroupSettings.AllowEdit = Infragistics.Win.DefaultableBoolean.False;
            this.ultraExplorerBar1.GroupSettings.HeaderButtonStyle = Infragistics.Win.UIElementButtonStyle.Button;
            this.ultraExplorerBar1.GroupSettings.HeaderVisible = Infragistics.Win.DefaultableBoolean.True;
            this.ultraExplorerBar1.GroupSettings.HotTracking = Infragistics.Win.DefaultableBoolean.True;
            this.ultraExplorerBar1.GroupSettings.ItemAreaInnerMargins.Bottom = 0;
            this.ultraExplorerBar1.GroupSettings.ItemAreaInnerMargins.Left = 0;
            this.ultraExplorerBar1.GroupSettings.ItemAreaInnerMargins.Right = 0;
            this.ultraExplorerBar1.GroupSettings.ItemAreaInnerMargins.Top = 0;
            this.ultraExplorerBar1.GroupSettings.ItemAreaOuterMargins.Bottom = 0;
            this.ultraExplorerBar1.GroupSettings.ItemAreaOuterMargins.Left = 0;
            this.ultraExplorerBar1.GroupSettings.ItemAreaOuterMargins.Right = 0;
            this.ultraExplorerBar1.GroupSettings.ItemAreaOuterMargins.Top = 0;
            this.ultraExplorerBar1.GroupSettings.ItemSort = Infragistics.Win.UltraWinExplorerBar.ItemSortType.None;
            this.ultraExplorerBar1.GroupSettings.ShowExpansionIndicator = Infragistics.Win.DefaultableBoolean.True;
            this.ultraExplorerBar1.GroupSettings.ShowInkButton = Infragistics.Win.ShowInkButton.Never;
            this.ultraExplorerBar1.GroupSettings.Style = Infragistics.Win.UltraWinExplorerBar.GroupStyle.LargeImagesWithTextBelow;
            this.ultraExplorerBar1.ImageListLarge = this.imageList1;
            this.ultraExplorerBar1.ImageListSmall = this.imageList1;
            this.ultraExplorerBar1.ImageSizeLarge = new System.Drawing.Size(36, 36);
            this.ultraExplorerBar1.ImageSizeSmall = new System.Drawing.Size(36, 36);
            this.ultraExplorerBar1.ItemSettings.AllowEdit = Infragistics.Win.DefaultableBoolean.False;
            this.ultraExplorerBar1.Location = new System.Drawing.Point(600, 187);
            this.ultraExplorerBar1.Name = "ultraExplorerBar1";
            this.ultraExplorerBar1.ShowDefaultContextMenu = false;
            this.ultraExplorerBar1.Size = new System.Drawing.Size(184, 291);
            this.ultraExplorerBar1.TabIndex = 19;
            this.ultraExplorerBar1.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.ultraExplorerBar1.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.ultraExplorerBar1.Visible = false;
            this.ultraExplorerBar1.ItemClick += new Infragistics.Win.UltraWinExplorerBar.ItemClickEventHandler(this.ultraExplorerBar1_ItemClick);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "");
            this.imageList1.Images.SetKeyName(1, "");
            this.imageList1.Images.SetKeyName(2, "");
            this.imageList1.Images.SetKeyName(3, "");
            this.imageList1.Images.SetKeyName(4, "");
            this.imageList1.Images.SetKeyName(5, "");
            this.imageList1.Images.SetKeyName(6, "");
            this.imageList1.Images.SetKeyName(7, "");
            this.imageList1.Images.SetKeyName(8, "");
            this.imageList1.Images.SetKeyName(9, "");
            this.imageList1.Images.SetKeyName(10, "");
            this.imageList1.Images.SetKeyName(11, "");
            this.imageList1.Images.SetKeyName(12, "");
            this.imageList1.Images.SetKeyName(13, "");
            this.imageList1.Images.SetKeyName(14, "");
            this.imageList1.Images.SetKeyName(15, "");
            this.imageList1.Images.SetKeyName(16, "");
            this.imageList1.Images.SetKeyName(17, "");
            this.imageList1.Images.SetKeyName(18, "");
            this.imageList1.Images.SetKeyName(19, "");
            this.imageList1.Images.SetKeyName(20, "");
            this.imageList1.Images.SetKeyName(21, "");
            // 
            // ultraStatusBar1
            // 
            this.ultraStatusBar1.Dock = System.Windows.Forms.DockStyle.Left;
            this.ultraStatusBar1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.ultraStatusBar1.Location = new System.Drawing.Point(0, 0);
            this.ultraStatusBar1.Name = "ultraStatusBar1";
            this.ultraStatusBar1.Size = new System.Drawing.Size(100, 100);
            this.ultraStatusBar1.TabIndex = 0;
            this.ultraStatusBar1.Text = "ultraStatusBar1";
            // 
            // ultraStatusBar
            // 
            appearance17.FontData.BoldAsString = "True";
            appearance17.FontData.Name = "Arial";
            this.ultraStatusBar.Appearance = appearance17;
            this.ultraStatusBar.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.ultraStatusBar.Location = new System.Drawing.Point(0, 506);
            this.ultraStatusBar.Name = "ultraStatusBar";
            ultraStatusPanel1.Key = "Version";
            ultraStatusPanel1.SizingMode = Infragistics.Win.UltraWinStatusBar.PanelSizingMode.Automatic;
            ultraStatusPanel1.Tag = ((short)(9));
            ultraStatusPanel1.Text = "Version (1.0.0.0)";
            ultraStatusPanel2.Key = "Pharmacy";
            ultraStatusPanel2.SizingMode = Infragistics.Win.UltraWinStatusBar.PanelSizingMode.Adjustable;
            ultraStatusPanel2.Text = "Pharmacy";
            ultraStatusPanel2.Width = 250;
            appearance18.Image = ((object)(resources.GetObject("appearance18.Image")));
            ultraStatusPanel3.Appearance = appearance18;
            ultraStatusPanel3.Key = "Station";
            ultraStatusPanel3.SizingMode = Infragistics.Win.UltraWinStatusBar.PanelSizingMode.Automatic;
            ultraStatusPanel3.Text = "Station";
            appearance19.Image = ((object)(resources.GetObject("appearance19.Image")));
            ultraStatusPanel4.Appearance = appearance19;
            ultraStatusPanel4.Key = "User";
            ultraStatusPanel4.SizingMode = Infragistics.Win.UltraWinStatusBar.PanelSizingMode.Adjustable;
            ultraStatusPanel4.Text = "Current User";
            ultraStatusPanel4.Width = 200;
            appearance20.Image = ((object)(resources.GetObject("appearance20.Image")));
            appearance20.ImageHAlign = Infragistics.Win.HAlign.Left;
            appearance20.ImageVAlign = Infragistics.Win.VAlign.Middle;
            appearance20.TextHAlignAsString = "Right";
            ultraStatusPanel5.Appearance = appearance20;
            ultraStatusPanel5.Key = "Date ";
            ultraStatusPanel5.SizingMode = Infragistics.Win.UltraWinStatusBar.PanelSizingMode.Adjustable;
            ultraStatusPanel5.Style = Infragistics.Win.UltraWinStatusBar.PanelStyle.Date;
            ultraStatusPanel5.Text = "Date ";
            appearance21.Image = ((object)(resources.GetObject("appearance21.Image")));
            appearance21.TextHAlignAsString = "Right";
            ultraStatusPanel6.Appearance = appearance21;
            ultraStatusPanel6.Key = "Time";
            ultraStatusPanel6.Style = Infragistics.Win.UltraWinStatusBar.PanelStyle.Time;
            ultraStatusPanel6.Text = "Time";
            ultraStatusPanel7.Key = "Msg";
            ultraStatusPanel7.SizingMode = Infragistics.Win.UltraWinStatusBar.PanelSizingMode.Automatic;
            ultraStatusPanel8.Key = "Show-HidePOBar";
            ultraStatusPanel8.SizingMode = Infragistics.Win.UltraWinStatusBar.PanelSizingMode.Automatic;
            ultraStatusPanel8.Text = "Show/Hide Prime PO Status Bar";
            ultraStatusPanel8.ToolTipText = "Click Here To Show Or Hide Prime PO Status Bar";
            ultraStatusPanel9.Key = "OnHoldTrans";
            ultraStatusPanel9.Text = "On Hold Transaction : ";
            ultraStatusPanel9.ToolTipText = "Click here to see OnHold Transactions";
            this.ultraStatusBar.Panels.AddRange(new Infragistics.Win.UltraWinStatusBar.UltraStatusPanel[] {
            ultraStatusPanel1,
            ultraStatusPanel2,
            ultraStatusPanel3,
            ultraStatusPanel4,
            ultraStatusPanel5,
            ultraStatusPanel6,
            ultraStatusPanel7,
            ultraStatusPanel8,
            ultraStatusPanel9});
            this.ultraStatusBar.ScaledImageSize = new System.Drawing.Size(20, 20);
            this.ultraStatusBar.Size = new System.Drawing.Size(784, 25);
            this.ultraStatusBar.SizeGripVisible = Infragistics.Win.DefaultableBoolean.False;
            this.ultraStatusBar.TabIndex = 5;
            this.ultraStatusBar.Tag = "NOCOLOR";
            this.ultraStatusBar.UseOsThemes = Infragistics.Win.DefaultableBoolean.True;
            this.ultraStatusBar.ViewStyle = Infragistics.Win.UltraWinStatusBar.ViewStyle.VisualStudio2005;
            this.ultraStatusBar.WrapText = false;
            this.ultraStatusBar.PanelDoubleClick += new Infragistics.Win.UltraWinStatusBar.PanelClickEventHandler(this.ultraStatusBar_PanelDoubleClick);
            this.ultraStatusBar.PanelClick += new Infragistics.Win.UltraWinStatusBar.PanelClickEventHandler(this.ultraStatusBar_PanelClick);
            // 
            // splitter1
            // 
            this.splitter1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.splitter1.Location = new System.Drawing.Point(0, 187);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(3, 291);
            this.splitter1.TabIndex = 25;
            this.splitter1.TabStop = false;
            // 
            // imageList2
            // 
            this.imageList2.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList2.ImageStream")));
            this.imageList2.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList2.Images.SetKeyName(0, "");
            this.imageList2.Images.SetKeyName(1, "");
            this.imageList2.Images.SetKeyName(2, "");
            this.imageList2.Images.SetKeyName(3, "");
            this.imageList2.Images.SetKeyName(4, "");
            this.imageList2.Images.SetKeyName(5, "");
            this.imageList2.Images.SetKeyName(6, "");
            this.imageList2.Images.SetKeyName(7, "");
            this.imageList2.Images.SetKeyName(8, "");
            this.imageList2.Images.SetKeyName(9, "");
            this.imageList2.Images.SetKeyName(10, "");
            this.imageList2.Images.SetKeyName(11, "");
            this.imageList2.Images.SetKeyName(12, "");
            this.imageList2.Images.SetKeyName(13, "");
            this.imageList2.Images.SetKeyName(14, "");
            this.imageList2.Images.SetKeyName(15, "");
            this.imageList2.Images.SetKeyName(16, "");
            this.imageList2.Images.SetKeyName(17, "");
            // 
            // ultraStatusBarVendor
            // 
            this.ultraStatusBarVendor.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Office2003ToolbarButton;
            this.ultraStatusBarVendor.ContextMenuStrip = this.cntMenuPOStatus;
            this.ultraStatusBarVendor.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.ultraStatusBarVendor.Location = new System.Drawing.Point(0, 478);
            this.ultraStatusBarVendor.Name = "ultraStatusBarVendor";
            ultraStatusPanel10.Key = "Queued";
            ultraStatusPanel10.Visible = false;
            ultraStatusPanel10.Width = 120;
            ultraStatusPanel11.Key = "Submitted";
            ultraStatusPanel11.Visible = false;
            ultraStatusPanel11.Width = 120;
            ultraStatusPanel12.Key = "Expired";
            ultraStatusPanel12.Text = "Expired";
            ultraStatusPanel12.Visible = false;
            ultraStatusPanel12.Width = 120;
            ultraStatusPanel13.Key = "Error";
            ultraStatusPanel13.Text = "Error";
            ultraStatusPanel13.Visible = false;
            ultraStatusPanel13.Width = 120;
            ultraStatusPanel14.Key = "Overdue";
            ultraStatusPanel14.Text = "Overdue";
            ultraStatusPanel14.Visible = false;
            ultraStatusPanel14.Width = 120;
            ultraStatusPanel15.Key = "Acknowledged";
            ultraStatusPanel15.Visible = false;
            ultraStatusPanel15.Width = 150;
            ultraStatusPanel16.Key = "MaxAttempts";
            ultraStatusPanel16.Text = "Max Attempts";
            ultraStatusPanel16.Visible = false;
            ultraStatusPanel17.Key = "InComplete";
            ultraStatusPanel17.Text = "InComplete";
            ultraStatusPanel17.Visible = false;
            ultraStatusPanel17.Width = 120;
            ultraStatusPanel18.Key = "DeliveryReceived";
            ultraStatusPanel18.Text = "DeliveryReceived";
            ultraStatusPanel18.Visible = false;
            ultraStatusPanel18.Width = 345;
            ultraStatusPanel19.Key = "PoLogScreen";
            ultraStatusPanel19.Width = 467;
            ultraStatusPanel20.Key = "PrimePOSStatus";
            ultraStatusPanel20.Width = 345;
            this.ultraStatusBarVendor.Panels.AddRange(new Infragistics.Win.UltraWinStatusBar.UltraStatusPanel[] {
            ultraStatusPanel10,
            ultraStatusPanel11,
            ultraStatusPanel12,
            ultraStatusPanel13,
            ultraStatusPanel14,
            ultraStatusPanel15,
            ultraStatusPanel16,
            ultraStatusPanel17,
            ultraStatusPanel18,
            ultraStatusPanel19,
            ultraStatusPanel20});
            this.ultraStatusBarVendor.Size = new System.Drawing.Size(784, 28);
            this.ultraStatusBarVendor.TabIndex = 30;
            this.ultraStatusBarVendor.Tag = "NOCOLOR";
            this.ultraStatusBarVendor.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.ultraStatusBarVendor.ViewStyle = Infragistics.Win.UltraWinStatusBar.ViewStyle.VisualStudio2005;
            this.ultraStatusBarVendor.WrapText = false;
            this.ultraStatusBarVendor.PanelClick += new Infragistics.Win.UltraWinStatusBar.PanelClickEventHandler(this.ultraStatusBarVendor_PanelClick);
            // 
            // cntMenuPOStatus
            // 
            this.cntMenuPOStatus.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Expired,
            this.Error,
            this.Overdue,
            this.Acknowledged,
            this.DeliveryReceived,
            this.maxAttemptToolStripMenuItem,
            this.InComplete});
            this.cntMenuPOStatus.Name = "cntMenuPOStatus";
            this.cntMenuPOStatus.ShowCheckMargin = true;
            this.cntMenuPOStatus.Size = new System.Drawing.Size(186, 158);
            this.cntMenuPOStatus.Closed += new System.Windows.Forms.ToolStripDropDownClosedEventHandler(this.cntMenuPOStatus_Closed);
            this.cntMenuPOStatus.Opening += new System.ComponentModel.CancelEventHandler(this.cntMenuPOStatus_Opening);
            this.cntMenuPOStatus.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.cntMenuPOStatus_ItemClicked);
            // 
            // Expired
            // 
            this.Expired.CheckOnClick = true;
            this.Expired.Name = "Expired";
            this.Expired.Size = new System.Drawing.Size(185, 22);
            this.Expired.Text = "Expired";
            // 
            // Error
            // 
            this.Error.CheckOnClick = true;
            this.Error.Name = "Error";
            this.Error.Size = new System.Drawing.Size(185, 22);
            this.Error.Text = "Error";
            // 
            // Overdue
            // 
            this.Overdue.CheckOnClick = true;
            this.Overdue.Name = "Overdue";
            this.Overdue.Size = new System.Drawing.Size(185, 22);
            this.Overdue.Text = "Overdue";
            // 
            // Acknowledged
            // 
            this.Acknowledged.CheckOnClick = true;
            this.Acknowledged.Name = "Acknowledged";
            this.Acknowledged.Size = new System.Drawing.Size(185, 22);
            this.Acknowledged.Text = "Acknowledged";
            // 
            // DeliveryReceived
            // 
            this.DeliveryReceived.CheckOnClick = true;
            this.DeliveryReceived.Name = "DeliveryReceived";
            this.DeliveryReceived.Size = new System.Drawing.Size(185, 22);
            this.DeliveryReceived.Text = "DeliveryReceived";
            // 
            // maxAttemptToolStripMenuItem
            // 
            this.maxAttemptToolStripMenuItem.CheckOnClick = true;
            this.maxAttemptToolStripMenuItem.Name = "maxAttemptToolStripMenuItem";
            this.maxAttemptToolStripMenuItem.Size = new System.Drawing.Size(185, 22);
            this.maxAttemptToolStripMenuItem.Text = "MaxAttempts";
            // 
            // InComplete
            // 
            this.InComplete.CheckOnClick = true;
            this.InComplete.Name = "InComplete";
            this.InComplete.Size = new System.Drawing.Size(185, 22);
            this.InComplete.Text = "InComplete";
            // 
            // _frmMain_Toolbars_Dock_Area_Right
            // 
            this._frmMain_Toolbars_Dock_Area_Right.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._frmMain_Toolbars_Dock_Area_Right.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(158)))), ((int)(((byte)(190)))), ((int)(((byte)(245)))));
            this._frmMain_Toolbars_Dock_Area_Right.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Right;
            this._frmMain_Toolbars_Dock_Area_Right.ForeColor = System.Drawing.Color.Black;
            this._frmMain_Toolbars_Dock_Area_Right.Location = new System.Drawing.Point(784, 187);
            this._frmMain_Toolbars_Dock_Area_Right.Name = "_frmMain_Toolbars_Dock_Area_Right";
            this._frmMain_Toolbars_Dock_Area_Right.Size = new System.Drawing.Size(0, 291);
            this._frmMain_Toolbars_Dock_Area_Right.ToolbarsManager = this.ultMenuBar;
            // 
            // ultMenuBar
            // 
            appearance22.BackColor = System.Drawing.Color.White;
            appearance22.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(140)))), ((int)(((byte)(140)))), ((int)(((byte)(140)))));
            appearance22.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance22.BorderColor = System.Drawing.Color.RoyalBlue;
            appearance22.FontData.BoldAsString = "True";
            appearance22.FontData.Name = "Arial";
            appearance22.FontData.SizeInPoints = 9F;
            this.ultMenuBar.Appearance = appearance22;
            this.ultMenuBar.DesignerFlags = 1;
            this.ultMenuBar.DockWithinContainer = this;
            this.ultMenuBar.DockWithinContainerBaseType = typeof(System.Windows.Forms.Form);
            this.ultMenuBar.ImageListLarge = this.imageList1;
            this.ultMenuBar.ImageListSmall = this.imageList1;
            this.ultMenuBar.ImageSizeLarge = new System.Drawing.Size(24, 24);
            this.ultMenuBar.ImageSizeSmall = new System.Drawing.Size(24, 24);
            this.ultMenuBar.IsGlassSupported = false;
            appearance23.BackColor = System.Drawing.Color.LightCyan;
            appearance23.BackColor2 = System.Drawing.Color.PowderBlue;
            appearance23.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            this.ultMenuBar.MenuSettings.Appearance = appearance23;
            this.ultMenuBar.MenuSettings.PopupStyle = Infragistics.Win.UltraWinToolbars.PopupStyle.Menu;
            this.ultMenuBar.MenuSettings.SideStripImagePadding = 0;
            this.ultMenuBar.MenuSettings.SideStripWidth = 20;
            this.ultMenuBar.MenuSettings.ToolDisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.TextOnlyAlways;
            this.ultMenuBar.RightAlignedMenus = Infragistics.Win.DefaultableBoolean.False;
            this.ultMenuBar.RuntimeCustomizationOptions = Infragistics.Win.UltraWinToolbars.RuntimeCustomizationOptions.None;
            this.ultMenuBar.ShowFullMenusDelay = 500;
            this.ultMenuBar.ShowMenuShadows = Infragistics.Win.DefaultableBoolean.True;
            this.ultMenuBar.Style = Infragistics.Win.UltraWinToolbars.ToolbarStyle.Office2003;
            ultraToolbar1.DockedColumn = 0;
            ultraToolbar1.DockedRow = 0;
            ultraToolbar1.FloatingLocation = new System.Drawing.Point(270, 268);
            ultraToolbar1.FloatingSize = new System.Drawing.Size(386, 66);
            ultraToolbar1.IsMainMenuBar = true;
            ultraToolbar1.NonInheritedTools.AddRange(new Infragistics.Win.UltraWinToolbars.ToolBase[] {
            popupMenuTool1,
            popupMenuTool2,
            popupMenuTool3,
            popupMenuTool19,
            popupMenuTool4,
            popupMenuTool26,
            popupMenuTool21});
            ultraToolbar1.Settings.AllowCustomize = Infragistics.Win.DefaultableBoolean.False;
            ultraToolbar1.Settings.AllowDockBottom = Infragistics.Win.DefaultableBoolean.False;
            ultraToolbar1.Settings.AllowDockLeft = Infragistics.Win.DefaultableBoolean.False;
            ultraToolbar1.Settings.AllowDockRight = Infragistics.Win.DefaultableBoolean.False;
            ultraToolbar1.Settings.AllowDockTop = Infragistics.Win.DefaultableBoolean.False;
            ultraToolbar1.Settings.AllowFloating = Infragistics.Win.DefaultableBoolean.False;
            ultraToolbar1.Settings.AllowHiding = Infragistics.Win.DefaultableBoolean.False;
            appearance24.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            appearance24.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            appearance24.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance24.BorderColor = System.Drawing.Color.Black;
            appearance24.FontData.Name = "Verdana";
            ultraToolbar1.Settings.Appearance = appearance24;
            ultraToolbar1.Settings.GrabHandleStyle = Infragistics.Win.UltraWinToolbars.GrabHandleStyle.Office2003;
            ultraToolbar1.Settings.PaddingBottom = 0;
            ultraToolbar1.Settings.PaddingLeft = 0;
            ultraToolbar1.Settings.PaddingRight = 0;
            ultraToolbar1.Settings.PaddingTop = 3;
            ultraToolbar1.Settings.ToolDisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.TextOnlyInMenus;
            ultraToolbar1.Settings.ToolSpacing = 3;
            ultraToolbar1.Text = "POS";
            ultraToolbar2.DockedColumn = 0;
            ultraToolbar2.DockedRow = 1;
            ultraToolbar2.FloatingLocation = new System.Drawing.Point(36, 217);
            ultraToolbar2.FloatingSize = new System.Drawing.Size(577, 53);
            ultraToolbar2.NonInheritedTools.AddRange(new Infragistics.Win.UltraWinToolbars.ToolBase[] {
            buttonTool1,
            buttonTool2,
            buttonTool3});
            appearance25.FontData.BoldAsString = "True";
            appearance25.FontData.SizeInPoints = 9F;
            ultraToolbar2.Settings.Appearance = appearance25;
            ultraToolbar2.Text = "POS Terminal";
            ultraToolbar3.DockedColumn = 0;
            ultraToolbar3.DockedRow = 2;
            ultraToolbar3.FloatingLocation = new System.Drawing.Point(8, 220);
            ultraToolbar3.FloatingSize = new System.Drawing.Size(568, 149);
            ultraToolbar3.NonInheritedTools.AddRange(new Infragistics.Win.UltraWinToolbars.ToolBase[] {
            buttonTool4,
            buttonTool5,
            buttonTool6,
            buttonTool7,
            buttonTool8});
            appearance26.FontData.BoldAsString = "True";
            appearance26.FontData.SizeInPoints = 9F;
            ultraToolbar3.Settings.Appearance = appearance26;
            ultraToolbar3.Text = "Inventory Management";
            ultraToolbar4.DockedColumn = 0;
            ultraToolbar4.DockedRow = 3;
            ultraToolbar4.FloatingLocation = new System.Drawing.Point(33, 195);
            ultraToolbar4.FloatingSize = new System.Drawing.Size(616, 53);
            ultraToolbar4.NonInheritedTools.AddRange(new Infragistics.Win.UltraWinToolbars.ToolBase[] {
            buttonTool9,
            buttonTool10,
            buttonTool11});
            appearance27.FontData.BoldAsString = "True";
            appearance27.FontData.SizeInPoints = 9F;
            ultraToolbar4.Settings.Appearance = appearance27;
            ultraToolbar4.Text = "Administrative Function";
            this.ultMenuBar.Toolbars.AddRange(new Infragistics.Win.UltraWinToolbars.UltraToolbar[] {
            ultraToolbar1,
            ultraToolbar2,
            ultraToolbar3,
            ultraToolbar4});
            this.ultMenuBar.ToolbarSettings.AllowCustomize = Infragistics.Win.DefaultableBoolean.True;
            this.ultMenuBar.ToolbarSettings.AllowDockBottom = Infragistics.Win.DefaultableBoolean.True;
            this.ultMenuBar.ToolbarSettings.AllowDockLeft = Infragistics.Win.DefaultableBoolean.True;
            this.ultMenuBar.ToolbarSettings.AllowDockRight = Infragistics.Win.DefaultableBoolean.True;
            this.ultMenuBar.ToolbarSettings.AllowDockTop = Infragistics.Win.DefaultableBoolean.True;
            this.ultMenuBar.ToolbarSettings.AllowFloating = Infragistics.Win.DefaultableBoolean.True;
            this.ultMenuBar.ToolbarSettings.AllowHiding = Infragistics.Win.DefaultableBoolean.True;
            this.ultMenuBar.ToolbarSettings.BorderStyleDocked = Infragistics.Win.UIElementBorderStyle.Raised;
            this.ultMenuBar.ToolbarSettings.CaptionPlacement = Infragistics.Win.TextPlacement.BelowImage;
            this.ultMenuBar.ToolbarSettings.FillEntireRow = Infragistics.Win.DefaultableBoolean.False;
            this.ultMenuBar.ToolbarSettings.GrabHandleStyle = Infragistics.Win.UltraWinToolbars.GrabHandleStyle.Office2003;
            this.ultMenuBar.ToolbarSettings.ToolDisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.ImageAndText;
            popupMenuTool6.Settings.IsSideStripVisible = Infragistics.Win.DefaultableBoolean.False;
            popupMenuTool6.Settings.PopupStyle = Infragistics.Win.UltraWinToolbars.PopupStyle.Menu;
            appearance28.BackColor = System.Drawing.Color.White;
            appearance28.BackGradientStyle = Infragistics.Win.GradientStyle.None;
            popupMenuTool6.Settings.SideStripAppearance = appearance28;
            popupMenuTool6.Settings.SideStripImagePadding = 20;
            popupMenuTool6.Settings.SideStripWidth = 30;
            popupMenuTool6.Settings.ToolDisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.ImageAndText;
            popupMenuTool6.SharedPropsInternal.Caption = "&POS Terminal";
            popupMenuTool6.SharedPropsInternal.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.TextOnlyInMenus;
            popupMenuTool6.Tools.AddRange(new Infragistics.Win.UltraWinToolbars.ToolBase[] {
            buttonTool12,
            buttonTool13,
            buttonTool14,
            buttonTool254,
            buttonTool255,
            buttonTool95,
            buttonTool100});
            popupMenuTool7.Settings.IsSideStripVisible = Infragistics.Win.DefaultableBoolean.False;
            popupMenuTool7.Settings.ToolDisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.ImageAndText;
            popupMenuTool7.SharedPropsInternal.Caption = "&Inventory Management";
            popupMenuTool7.SharedPropsInternal.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.TextOnlyInMenus;
            popupMenuTool8.InstanceProps.IsFirstInGroup = true;
            popupMenuTool9.InstanceProps.IsFirstInGroup = true;
            popupMenuTool7.Tools.AddRange(new Infragistics.Win.UltraWinToolbars.ToolBase[] {
            buttonTool15,
            buttonTool16,
            buttonTool17,
            buttonTool18,
            popupMenuTool8,
            buttonTool19,
            popupMenuTool5,
            popupMenuTool9});
            appearance29.BackGradientStyle = Infragistics.Win.GradientStyle.None;
            popupMenuTool10.Settings.HotTrackAppearance = appearance29;
            popupMenuTool10.Settings.IsSideStripVisible = Infragistics.Win.DefaultableBoolean.False;
            popupMenuTool10.Settings.ToolDisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.TextOnlyAlways;
            popupMenuTool10.SharedPropsInternal.Caption = "&Administrative Functions";
            popupMenuTool10.SharedPropsInternal.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.TextOnlyInMenus;
            buttonTool27.InstanceProps.IsFirstInGroup = true;
            buttonTool29.InstanceProps.IsFirstInGroup = true;
            buttonTool31.InstanceProps.IsFirstInGroup = true;
            popupMenuTool11.InstanceProps.IsFirstInGroup = true;
            popupMenuTool17.InstanceProps.IsFirstInGroup = true;
            buttonTool235.InstanceProps.IsFirstInGroup = true;
            popupMenuTool10.Tools.AddRange(new Infragistics.Win.UltraWinToolbars.ToolBase[] {
            popupMenuTool27,
            buttonTool24,
            buttonTool25,
            buttonTool26,
            buttonTool27,
            buttonTool28,
            buttonTool29,
            buttonTool30,
            buttonTool31,
            popupMenuTool11,
            buttonTool32,
            popupMenuTool17,
            buttonTool206,
            popupMenuTool23,
            buttonTool218,
            buttonTool220,
            buttonTool228,
            buttonTool235,
            buttonTool236,
            buttonTool248,
            popupMenuTool29,
            buttonTool252,
            buttonTool258,
            buttonTool259,
            buttonTool264});
            appearance30.Image = ((object)(resources.GetObject("appearance30.Image")));
            buttonTool33.SharedPropsInternal.AppearancesLarge.Appearance = appearance30;
            appearance31.Image = ((object)(resources.GetObject("appearance31.Image")));
            buttonTool33.SharedPropsInternal.AppearancesSmall.Appearance = appearance31;
            buttonTool33.SharedPropsInternal.Caption = "&POS Transaction";
            buttonTool33.SharedPropsInternal.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.ImageAndText;
            buttonTool33.SharedPropsInternal.Shortcut = System.Windows.Forms.Shortcut.F5;
            buttonTool33.SharedPropsInternal.ToolTipText = "POS Transaction (F5)";
            appearance32.Image = ((object)(resources.GetObject("appearance32.Image")));
            buttonTool34.SharedPropsInternal.AppearancesSmall.Appearance = appearance32;
            buttonTool34.SharedPropsInternal.Caption = "Sale &Returns";
            buttonTool34.SharedPropsInternal.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.ImageAndText;
            buttonTool34.SharedPropsInternal.Shortcut = System.Windows.Forms.Shortcut.CtrlR;
            buttonTool34.SharedPropsInternal.Visible = false;
            appearance33.Image = 2;
            buttonTool35.SharedPropsInternal.AppearancesSmall.Appearance = appearance33;
            buttonTool35.SharedPropsInternal.Caption = "Pa&y Out";
            buttonTool35.SharedPropsInternal.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.ImageAndText;
            buttonTool35.SharedPropsInternal.Shortcut = System.Windows.Forms.Shortcut.CtrlY;
            appearance34.Image = 5;
            buttonTool36.SharedPropsInternal.AppearancesSmall.Appearance = appearance34;
            buttonTool36.SharedPropsInternal.Caption = "Add/Edit I&nventory Received";
            buttonTool36.SharedPropsInternal.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.ImageAndText;
            buttonTool36.SharedPropsInternal.Shortcut = System.Windows.Forms.Shortcut.CtrlN;
            appearance35.Image = 4;
            buttonTool37.SharedPropsInternal.AppearancesSmall.Appearance = appearance35;
            buttonTool37.SharedPropsInternal.Caption = "Add/Edit &Purchase Order";
            buttonTool37.SharedPropsInternal.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.ImageAndText;
            buttonTool37.SharedPropsInternal.Shortcut = System.Windows.Forms.Shortcut.CtrlP;
            appearance36.Image = 8;
            buttonTool38.SharedPropsInternal.AppearancesSmall.Appearance = appearance36;
            buttonTool38.SharedPropsInternal.Caption = "&Department File";
            buttonTool38.SharedPropsInternal.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.ImageAndText;
            buttonTool38.SharedPropsInternal.Shortcut = System.Windows.Forms.Shortcut.CtrlD;
            appearance37.Image = 9;
            buttonTool39.SharedPropsInternal.AppearancesSmall.Appearance = appearance37;
            buttonTool39.SharedPropsInternal.Caption = "Ta&x Code File";
            buttonTool39.SharedPropsInternal.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.ImageAndText;
            buttonTool39.SharedPropsInternal.Shortcut = System.Windows.Forms.Shortcut.CtrlT;
            appearance38.Image = 7;
            buttonTool40.SharedPropsInternal.AppearancesSmall.Appearance = appearance38;
            buttonTool40.SharedPropsInternal.Caption = "&Vendor File";
            buttonTool40.SharedPropsInternal.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.ImageAndText;
            buttonTool40.SharedPropsInternal.Shortcut = System.Windows.Forms.Shortcut.CtrlW;
            appearance39.Image = 11;
            buttonTool42.SharedPropsInternal.AppearancesSmall.Appearance = appearance39;
            buttonTool42.SharedPropsInternal.Caption = "&Function Keys";
            buttonTool42.SharedPropsInternal.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.ImageAndText;
            buttonTool42.SharedPropsInternal.Shortcut = System.Windows.Forms.Shortcut.CtrlF;
            appearance40.Image = ((object)(resources.GetObject("appearance40.Image")));
            buttonTool43.SharedPropsInternal.AppearancesLarge.Appearance = appearance40;
            appearance41.Image = ((object)(resources.GetObject("appearance41.Image")));
            buttonTool43.SharedPropsInternal.AppearancesSmall.Appearance = appearance41;
            buttonTool43.SharedPropsInternal.Caption = "Process &Close Station";
            buttonTool43.SharedPropsInternal.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.ImageAndText;
            buttonTool43.SharedPropsInternal.Shortcut = System.Windows.Forms.Shortcut.CtrlG;
            appearance42.Image = ((object)(resources.GetObject("appearance42.Image")));
            buttonTool44.SharedPropsInternal.AppearancesSmall.Appearance = appearance42;
            buttonTool44.SharedPropsInternal.Caption = "Process &End Of Day";
            buttonTool44.SharedPropsInternal.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.ImageAndText;
            buttonTool44.SharedPropsInternal.Shortcut = System.Windows.Forms.Shortcut.CtrlE;
            appearance43.Image = 14;
            buttonTool45.SharedPropsInternal.AppearancesSmall.Appearance = appearance43;
            buttonTool45.SharedPropsInternal.Caption = "&Lock Station";
            buttonTool45.SharedPropsInternal.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.ImageAndText;
            buttonTool45.SharedPropsInternal.Shortcut = System.Windows.Forms.Shortcut.CtrlL;
            appearance44.Image = ((object)(resources.GetObject("appearance44.Image")));
            buttonTool46.SharedPropsInternal.AppearancesSmall.Appearance = appearance44;
            buttonTool46.SharedPropsInternal.Caption = "Lo&g Off";
            buttonTool46.SharedPropsInternal.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.ImageAndText;
            buttonTool46.SharedPropsInternal.Shortcut = System.Windows.Forms.Shortcut.CtrlO;
            appearance45.Image = 3;
            buttonTool47.SharedPropsInternal.AppearancesSmall.Appearance = appearance45;
            buttonTool47.SharedPropsInternal.Caption = "&Customer File";
            buttonTool47.SharedPropsInternal.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.ImageAndText;
            buttonTool47.SharedPropsInternal.Shortcut = System.Windows.Forms.Shortcut.CtrlM;
            buttonTool48.SharedPropsInternal.Caption = "Item Companion";
            appearance46.Image = ((object)(resources.GetObject("appearance46.Image")));
            appearance46.ImageHAlign = Infragistics.Win.HAlign.Center;
            appearance46.ImageVAlign = Infragistics.Win.VAlign.Middle;
            appearance46.TextHAlignAsString = "Right";
            appearance46.TextVAlignAsString = "Middle";
            buttonTool49.SharedPropsInternal.AppearancesLarge.Appearance = appearance46;
            appearance47.Image = ((object)(resources.GetObject("appearance47.Image")));
            buttonTool49.SharedPropsInternal.AppearancesSmall.Appearance = appearance47;
            buttonTool49.SharedPropsInternal.Caption = "POS Transaction (F5)";
            buttonTool49.SharedPropsInternal.Shortcut = System.Windows.Forms.Shortcut.F5;
            buttonTool49.SharedPropsInternal.StatusText = "POS Register";
            buttonTool49.SharedPropsInternal.ToolTipText = "POS Transaction (F5)";
            appearance48.Image = 1;
            buttonTool50.SharedPropsInternal.AppearancesLarge.Appearance = appearance48;
            buttonTool50.SharedPropsInternal.Caption = "Sales Return";
            buttonTool50.SharedPropsInternal.Visible = false;
            appearance49.Image = 2;
            buttonTool51.SharedPropsInternal.AppearancesLarge.Appearance = appearance49;
            buttonTool51.SharedPropsInternal.Caption = "Pay Out";
            appearance50.Image = 3;
            buttonTool52.SharedPropsInternal.AppearancesLarge.Appearance = appearance50;
            buttonTool52.SharedPropsInternal.Caption = "Customer File";
            appearance51.Image = 4;
            buttonTool53.SharedPropsInternal.AppearancesLarge.Appearance = appearance51;
            buttonTool53.SharedPropsInternal.Caption = "Purchase Order";
            appearance52.Image = 5;
            buttonTool54.SharedPropsInternal.AppearancesLarge.Appearance = appearance52;
            buttonTool54.SharedPropsInternal.Caption = "Inventory Received";
            appearance53.Image = 6;
            buttonTool55.SharedPropsInternal.AppearancesLarge.Appearance = appearance53;
            appearance54.Image = 6;
            buttonTool55.SharedPropsInternal.AppearancesSmall.Appearance = appearance54;
            buttonTool55.SharedPropsInternal.Caption = "Item File";
            appearance55.Image = 7;
            buttonTool56.SharedPropsInternal.AppearancesLarge.Appearance = appearance55;
            buttonTool56.SharedPropsInternal.Caption = "Vendor File";
            appearance56.Image = 8;
            buttonTool57.SharedPropsInternal.AppearancesLarge.Appearance = appearance56;
            buttonTool57.SharedPropsInternal.Caption = "Department File";
            appearance57.Image = 9;
            buttonTool58.SharedPropsInternal.AppearancesLarge.Appearance = appearance57;
            buttonTool58.SharedPropsInternal.Caption = "Tax Code File";
            appearance58.Image = 10;
            buttonTool59.SharedPropsInternal.AppearancesLarge.Appearance = appearance58;
            buttonTool59.SharedPropsInternal.Caption = "User Setup File";
            buttonTool59.SharedPropsInternal.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.ImageAndText;
            appearance59.Image = 10;
            buttonTool60.SharedPropsInternal.AppearancesLarge.Appearance = appearance59;
            appearance60.Image = 10;
            buttonTool60.SharedPropsInternal.AppearancesSmall.Appearance = appearance60;
            buttonTool60.SharedPropsInternal.Caption = "Function Keys";
            appearance61.Image = 11;
            appearance61.ImageHAlign = Infragistics.Win.HAlign.Center;
            appearance61.ImageVAlign = Infragistics.Win.VAlign.Top;
            buttonTool61.SharedPropsInternal.AppearancesLarge.Appearance = appearance61;
            buttonTool61.SharedPropsInternal.Caption = "Close Station";
            appearance62.Image = 15;
            buttonTool62.SharedPropsInternal.AppearancesLarge.Appearance = appearance62;
            buttonTool62.SharedPropsInternal.Caption = "End of Day";
            appearance63.Image = 12;
            buttonTool63.SharedPropsInternal.AppearancesLarge.Appearance = appearance63;
            buttonTool63.SharedPropsInternal.Caption = "Lock";
            appearance64.Image = ((object)(resources.GetObject("appearance64.Image")));
            buttonTool64.SharedPropsInternal.AppearancesSmall.Appearance = appearance64;
            buttonTool64.SharedPropsInternal.Caption = "Log Off";
            buttonTool64.SharedPropsInternal.Enabled = false;
            buttonTool64.SharedPropsInternal.Visible = false;
            taskPaneTool1.SharedPropsInternal.Caption = "TaskPaneTool2";
            taskPaneTool2.SharedPropsInternal.Caption = "TaskPaneTool3";
            taskPaneTool3.SharedPropsInternal.Caption = "TaskPaneTool4";
            taskPaneTool4.SharedPropsInternal.Caption = "TaskPaneTool5";
            taskPaneTool5.SharedPropsInternal.Caption = "TaskPaneTool6";
            taskPaneTool6.SharedPropsInternal.Caption = "TaskPaneTool7";
            popupMenuTool12.Settings.IsSideStripVisible = Infragistics.Win.DefaultableBoolean.False;
            popupMenuTool12.SharedPropsInternal.Caption = "App&lication";
            popupMenuTool12.Tools.AddRange(new Infragistics.Win.UltraWinToolbars.ToolBase[] {
            stateButtonTool1,
            buttonTool146,
            buttonTool65,
            buttonTool211,
            buttonTool66,
            buttonTool67,
            buttonTool204,
            buttonTool68});
            appearance65.Image = ((object)(resources.GetObject("appearance65.Image")));
            buttonTool69.SharedPropsInternal.AppearancesSmall.Appearance = appearance65;
            buttonTool69.SharedPropsInternal.Caption = "View POS &Transaction";
            buttonTool69.SharedPropsInternal.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.ImageAndText;
            popupMenuTool13.Settings.IsSideStripVisible = Infragistics.Win.DefaultableBoolean.False;
            popupMenuTool13.Settings.PopupStyle = Infragistics.Win.UltraWinToolbars.PopupStyle.Menu;
            popupMenuTool13.SharedPropsInternal.Caption = "&Inventory Reports";
            popupMenuTool13.Tools.AddRange(new Infragistics.Win.UltraWinToolbars.ToolBase[] {
            buttonTool70,
            buttonTool71,
            buttonTool72,
            buttonTool73,
            buttonTool74,
            buttonTool75,
            buttonTool76,
            buttonTool77,
            buttonTool78,
            buttonTool79,
            buttonTool20,
            buttonTool156,
            buttonTool163,
            buttonTool97});
            buttonTool80.SharedPropsInternal.Caption = "&Item File Listing";
            buttonTool80.SharedPropsInternal.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.ImageAndText;
            buttonTool81.SharedPropsInternal.Caption = "&Vendor File Listing";
            buttonTool82.SharedPropsInternal.Caption = "Inventory &Status Report";
            buttonTool83.SharedPropsInternal.Caption = "&Physical Inventory Sheet";
            buttonTool84.SharedPropsInternal.Caption = "Inventory &Received";
            buttonTool85.SharedPropsInternal.Caption = "I&tems Re-Order";
            buttonTool86.SharedPropsInternal.Caption = "&Transaction Detail";
            buttonTool87.SharedPropsInternal.Caption = "Summary Sales By &User";
            buttonTool88.SharedPropsInternal.Caption = "Sales Report By &Item";
            buttonTool89.SharedPropsInternal.Caption = "Sales Report By &Department";
            appearance66.Image = ((object)(resources.GetObject("appearance66.Image")));
            buttonTool90.SharedPropsInternal.AppearancesSmall.Appearance = appearance66;
            buttonTool90.SharedPropsInternal.Caption = "View &Station Close";
            buttonTool90.SharedPropsInternal.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.ImageAndText;
            buttonTool91.SharedPropsInternal.Caption = "Sales Report By &Payment";
            buttonTool92.SharedPropsInternal.Caption = "Station &Close Summary";
            popupMenuTool14.Settings.IsSideStripVisible = Infragistics.Win.DefaultableBoolean.False;
            popupMenuTool14.SharedPropsInternal.Caption = "Management &Reports";
            popupMenuTool31.InstanceProps.IsFirstInGroup = true;
            popupMenuTool14.Tools.AddRange(new Infragistics.Win.UltraWinToolbars.ToolBase[] {
            popupMenuTool31,
            popupMenuTool33,
            popupMenuTool35,
            buttonTool173,
            buttonTool93,
            buttonTool98,
            buttonTool239,
            buttonTool183,
            buttonTool102,
            buttonTool157,
            buttonTool165,
            buttonTool191,
            buttonTool216,
            buttonTool244});
            buttonTool105.SharedPropsInternal.Caption = "Top &Selling Products";
            appearance67.Image = ((object)(resources.GetObject("appearance67.Image")));
            stateButtonTool2.SharedPropsInternal.AppearancesLarge.Appearance = appearance67;
            appearance68.Image = ((object)(resources.GetObject("appearance68.Image")));
            stateButtonTool2.SharedPropsInternal.AppearancesSmall.Appearance = appearance68;
            stateButtonTool2.SharedPropsInternal.Caption = "Preferences";
            stateButtonTool2.SharedPropsInternal.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.ImageAndText;
            buttonTool106.SharedPropsInternal.Caption = "Sales Ta&x Summary";
            appearance69.Image = 20;
            buttonTool107.SharedPropsInternal.AppearancesSmall.Appearance = appearance69;
            buttonTool107.SharedPropsInternal.Caption = "Ph&ysical Inventory";
            buttonTool107.SharedPropsInternal.Shortcut = System.Windows.Forms.Shortcut.CtrlH;
            buttonTool108.SharedPropsInternal.Caption = "Physical Inv. &History";
            appearance70.Image = 20;
            buttonTool109.SharedPropsInternal.AppearancesSmall.Appearance = appearance70;
            buttonTool109.SharedPropsInternal.Caption = "Physical Inventory";
            buttonTool110.SharedPropsInternal.Caption = "Hourly Trans. / Productivity Report";
            appearance71.Image = ((object)(resources.GetObject("appearance71.Image")));
            buttonTool111.SharedPropsInternal.AppearancesSmall.Appearance = appearance71;
            buttonTool111.SharedPropsInternal.Caption = "&View End Of Day";
            buttonTool111.SharedPropsInternal.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.ImageAndText;
            buttonTool112.SharedPropsInternal.Caption = "Purchase Order";
            buttonTool113.SharedPropsInternal.Caption = "Item Label Report";
            appearance72.Image = ((object)(resources.GetObject("appearance72.Image")));
            buttonTool114.SharedPropsInternal.AppearancesSmall.Appearance = appearance72;
            buttonTool114.SharedPropsInternal.Caption = "View Purchase &Order";
            appearance73.Image = ((object)(resources.GetObject("appearance73.Image")));
            buttonTool115.SharedPropsInternal.AppearancesSmall.Appearance = appearance73;
            buttonTool115.SharedPropsInternal.Caption = "&View Inventory Received";
            popupMenuTool15.SharedPropsInternal.Caption = "&Help";
            popupMenuTool15.SharedPropsInternal.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.TextOnlyAlways;
            popupMenuTool15.Tools.AddRange(new Infragistics.Win.UltraWinToolbars.ToolBase[] {
            buttonTool116,
            buttonTool200,
            buttonTool232});
            buttonTool117.SharedPropsInternal.Caption = "A&bout";
            appearance74.Image = 15;
            buttonTool118.SharedPropsInternal.AppearancesLarge.Appearance = appearance74;
            appearance75.Image = 15;
            buttonTool118.SharedPropsInternal.AppearancesSmall.Appearance = appearance75;
            buttonTool118.SharedPropsInternal.Caption = "E&xit";
            buttonTool118.SharedPropsInternal.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.ImageAndText;
            buttonTool119.SharedPropsInternal.Caption = "PO Ac&knowledgement";
            popupMenuTool16.SharedPropsInternal.Caption = "I&tem File";
            popupMenuTool16.SharedPropsInternal.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.ImageAndText;
            popupMenuTool16.SharedPropsInternal.Shortcut = System.Windows.Forms.Shortcut.CtrlI;
            popupMenuTool16.Tools.AddRange(new Infragistics.Win.UltraWinToolbars.ToolBase[] {
            buttonTool120,
            buttonTool121,
            buttonTool140,
            buttonTool154,
            buttonTool161,
            buttonTool175,
            buttonTool181,
            buttonTool241,
            buttonTool262});
            appearance76.Image = 6;
            buttonTool123.SharedPropsInternal.AppearancesLarge.Appearance = appearance76;
            appearance77.Image = 6;
            buttonTool123.SharedPropsInternal.AppearancesSmall.Appearance = appearance77;
            buttonTool123.SharedPropsInternal.Caption = "&Add / Edit Item";
            appearance78.Image = ((object)(resources.GetObject("appearance78.Image")));
            buttonTool124.SharedPropsInternal.AppearancesLarge.Appearance = appearance78;
            appearance79.Image = ((object)(resources.GetObject("appearance79.Image")));
            buttonTool124.SharedPropsInternal.AppearancesSmall.Appearance = appearance79;
            buttonTool124.SharedPropsInternal.Caption = "A&uto Update Price ";
            appearance80.Image = ((object)(resources.GetObject("appearance80.Image")));
            buttonTool125.SharedPropsInternal.AppearancesLarge.Appearance = appearance80;
            appearance81.Image = ((object)(resources.GetObject("appearance81.Image")));
            buttonTool125.SharedPropsInternal.AppearancesSmall.Appearance = appearance81;
            buttonTool125.SharedPropsInternal.Caption = "&Manual Update Price ";
            buttonTool125.SharedPropsInternal.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.ImageAndText;
            appearance82.Image = ((object)(resources.GetObject("appearance82.Image")));
            buttonTool126.SharedPropsInternal.AppearancesLarge.Appearance = appearance82;
            appearance83.Image = ((object)(resources.GetObject("appearance83.Image")));
            buttonTool126.SharedPropsInternal.AppearancesSmall.Appearance = appearance83;
            buttonTool126.SharedPropsInternal.Caption = "&My Store";
            buttonTool126.SharedPropsInternal.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.ImageAndText;
            buttonTool127.SharedPropsInternal.Caption = "Payout Details";
            appearance84.Image = 21;
            buttonTool128.SharedPropsInternal.AppearancesLarge.Appearance = appearance84;
            appearance85.Image = 21;
            buttonTool128.SharedPropsInternal.AppearancesSmall.Appearance = appearance85;
            buttonTool128.SharedPropsInternal.Caption = "Change Password";
            buttonTool128.SharedPropsInternal.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.ImageAndText;
            buttonTool129.SharedPropsInternal.Caption = "Inv. T&rans Type";
            buttonTool130.SharedPropsInternal.Caption = "Manage Messages";
            buttonTool130.SharedPropsInternal.Visible = false;
            buttonTool131.SharedPropsInternal.Caption = "Item Price Change Report";
            buttonTool132.SharedPropsInternal.Caption = "Sales Comparison";
            buttonTool133.SharedPropsInternal.Caption = "Cost Analysis";
            popupMenuTool18.SharedPropsInternal.Caption = "Purchase &Order";
            popupMenuTool18.Tools.AddRange(new Infragistics.Win.UltraWinToolbars.ToolBase[] {
            buttonTool134,
            buttonTool136,
            buttonTool138});
            buttonTool135.SharedPropsInternal.Caption = "Add/&Edit Purchase Order";
            buttonTool137.SharedPropsInternal.Caption = "&View Purchase Order";
            buttonTool139.SharedPropsInternal.Caption = "PO &Acknowledgement";
            buttonTool21.SharedPropsInternal.Caption = "Item List By Vendor";
            popupMenuTool20.SharedPropsInternal.Caption = "&Timesheet";
            popupMenuTool20.Tools.AddRange(new Infragistics.Win.UltraWinToolbars.ToolBase[] {
            buttonTool22,
            buttonTool142,
            buttonTool144});
            buttonTool122.SharedPropsInternal.Caption = "Clock - In / Out User";
            buttonTool141.SharedPropsInternal.Caption = "Last Update";
            buttonTool143.SharedPropsInternal.Caption = "Manage / Create Timesheet";
            buttonTool145.SharedPropsInternal.Caption = "View / Print Created Timesheet";
            appearance86.Image = ((object)(resources.GetObject("appearance86.Image")));
            buttonTool147.SharedPropsInternal.AppearancesLarge.Appearance = appearance86;
            appearance87.Image = ((object)(resources.GetObject("appearance87.Image")));
            buttonTool147.SharedPropsInternal.AppearancesSmall.Appearance = appearance87;
            buttonTool147.SharedPropsInternal.Caption = "IIAS File List";
            buttonTool147.SharedPropsInternal.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.ImageAndText;
            buttonTool149.SharedPropsInternal.Caption = "IIAS Transaction Summary";
            buttonTool151.SharedPropsInternal.Caption = "IIAS Payment Transaction";
            buttonTool153.SharedPropsInternal.Caption = "Sales By Customer";
            buttonTool155.SharedPropsInternal.Caption = "Item Monitor Category";
            buttonTool158.SharedPropsInternal.Caption = "Customer List";
            buttonTool160.SharedPropsInternal.Caption = "Sales By Item Monitoring Category";
            buttonTool162.SharedPropsInternal.Caption = "Warning Messages";
            buttonTool164.SharedPropsInternal.Caption = "Item Price Change and Label Report";
            buttonTool166.SharedPropsInternal.Caption = "Delivery List";
            buttonTool167.SharedPropsInternal.Caption = "Pseudoephedrine Sales Logs"; //PRIMEPOS-3360
            buttonTool168.SharedPropsInternal.Caption = "Sales By Insurance";
            buttonTool170.SharedPropsInternal.Caption = "Sale Tax Control";
            buttonTool172.SharedPropsInternal.Caption = "Sales Comparison By Dept.";
            comboBoxTool2.SharedPropsInternal.Caption = "Price Overridden Report";
            comboBoxTool2.ValueList = valueList1;
            buttonTool174.SharedPropsInternal.Caption = "Price Overridden Report";
            buttonTool176.SharedPropsInternal.Caption = "Item Department";
            buttonTool176.SharedPropsInternal.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.TextOnlyInMenus;
            popupMenuTool22.SharedPropsInternal.Caption = "Customer Loyalty";
            popupMenuTool22.Tools.AddRange(new Infragistics.Win.UltraWinToolbars.ToolBase[] {
            buttonTool177,
            buttonTool178,
            buttonTool222});
            buttonTool179.SharedPropsInternal.Caption = "Points Reward Tier";
            buttonTool180.SharedPropsInternal.Caption = "Register/Merge/Deactivate Cards";
            buttonTool182.SharedPropsInternal.Caption = "Set Selling Price";
            buttonTool184.SharedPropsInternal.Caption = "Transaction Time Report";
            buttonTool186.SharedPropsInternal.Caption = "Transaction Time Report";
            buttonTool187.SharedPropsInternal.Caption = "Transaction Time Report";
            buttonTool188.SharedPropsInternal.Caption = "dbh";
            buttonTool190.SharedPropsInternal.Caption = "Station Close Cash";
            buttonTool192.SharedPropsInternal.Caption = "ndfskj";
            buttonTool193.SharedPropsInternal.Caption = "Item Sale Historical Comparison";
            buttonTool185.SharedPropsInternal.Caption = "Station Close Cash";
            buttonTool194.SharedPropsInternal.Caption = "Item Sales Performance";
            buttonTool197.SharedPropsInternal.Caption = "Item Sales Performance";
            buttonTool198.SharedPropsInternal.Caption = "Item Sale Historical Comparison";
            buttonTool199.SharedPropsInternal.Caption = "Rx Checkout";
            buttonTool201.SharedPropsInternal.Caption = "System Notes";
            buttonTool203.SharedPropsInternal.Caption = "System Notes";
            buttonTool205.SharedPropsInternal.Caption = "&System Notes";
            buttonTool202.SharedPropsInternal.Caption = "Manage &Notes";
            buttonTool207.SharedPropsInternal.Caption = "Manage &Notes";
            buttonTool208.SharedPropsInternal.Caption = "Advance &Search Item";
            popupMenuTool24.SharedPropsInternal.Caption = "&Windows";
            popupMenuTool24.Tools.AddRange(new Infragistics.Win.UltraWinToolbars.ToolBase[] {
            mdiWindowListTool1});
            mdiWindowListTool2.SharedPropsInternal.Caption = "Windows";
            buttonTool209.SharedPropsInternal.Caption = "Help &Topics";
            buttonTool213.SharedPropsInternal.Caption = "&Payout";
            popupMenuTool25.SharedPropsInternal.Caption = "&Pay Out File";
            popupMenuTool25.Tools.AddRange(new Infragistics.Win.UltraWinToolbars.ToolBase[] {
            buttonTool215,
            buttonTool210});
            buttonTool212.SharedPropsInternal.Caption = "Pay &Out Category";
            buttonTool217.SharedPropsInternal.Caption = "ROA Details";
            buttonTool219.SharedPropsInternal.Caption = "View POS Trans. Co&lor Scheme";
            buttonTool221.SharedPropsInternal.Caption = "Pa&yment Type File";
            buttonTool223.SharedPropsInternal.Caption = "View CL Summary";
            buttonTool225.SharedPropsInternal.Caption = "Sales Summary By Vendor";
            appearance88.Image = ((object)(resources.GetObject("appearance88.Image")));
            popupMenuTool28.SharedPropsInternal.AppearancesLarge.Appearance = appearance88;
            appearance89.Image = ((object)(resources.GetObject("appearance89.Image")));
            popupMenuTool28.SharedPropsInternal.AppearancesSmall.Appearance = appearance89;
            popupMenuTool28.SharedPropsInternal.Caption = "&User File";
            popupMenuTool28.SharedPropsInternal.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.DefaultForToolType;
            popupMenuTool28.SharedPropsInternal.Shortcut = System.Windows.Forms.Shortcut.CtrlU;
            popupMenuTool28.Tools.AddRange(new Infragistics.Win.UltraWinToolbars.ToolBase[] {
            buttonTool23,
            buttonTool41});
            buttonTool227.SharedPropsInternal.Caption = "User &Group";
            buttonTool226.SharedPropsInternal.Caption = "&User Set Up";
            buttonTool229.SharedPropsInternal.Caption = "Coupon";
            buttonTool231.SharedPropsInternal.Caption = "Dead Stock Report";
            buttonTool233.SharedPropsInternal.Caption = "Process On Hold Purchase Orders";
            buttonTool234.SharedPropsInternal.Caption = "&What\'s New?";
            buttonTool237.SharedPropsInternal.Caption = "Check For Updates";
            buttonTool238.SharedPropsInternal.Caption = "Station Settings";
            buttonTool240.SharedPropsInternal.Caption = "End Of Day Summary";
            buttonTool242.SharedPropsInternal.Caption = "Department List";
            buttonTool214.SharedPropsInternal.Caption = "PSE Item List";
            buttonTool243.SharedPropsInternal.Caption = "Item Combo Pricing";
            buttonTool245.SharedPropsInternal.Caption = "Coupon Report";
            buttonTool247.SharedPropsInternal.Caption = "On-Hold Transaction Detail Report";
            buttonTool249.SharedPropsInternal.Caption = "Delivery Reconciliation";
            comboBoxTool3.SharedPropsInternal.Caption = "StoreCredit";
            comboBoxTool3.ValueList = valueList2;
            popupMenuTool30.SharedPropsInternal.Caption = "Store Credit";
            popupMenuTool30.Tools.AddRange(new Infragistics.Win.UltraWinToolbars.ToolBase[] {
            buttonTool250});
            buttonTool251.SharedPropsInternal.Caption = "View Store Credit Summary";
            buttonTool253.SharedPropsInternal.Caption = "Process Evertec Settlement";
            buttonTool256.SharedPropsInternal.Caption = "Credit Card Log";
            buttonTool257.SharedPropsInternal.Caption = "Failed Rx Updates";
            buttonTool260.SharedPropsInternal.Caption = "View Audit Trail";
            buttonTool261.SharedPropsInternal.Caption = "View NoSale Transaction";
            buttonTool263.SharedPropsInternal.Caption = "Set Item Tax";
            buttonTool265.SharedPropsInternal.Caption = "Scheduled Task";
            comboBoxTool4.SharedPropsInternal.Caption = "&Transaction Detail";
            comboBoxTool4.ValueList = valueList3;
            comboBoxTool6.SharedPropsInternal.Caption = "ComboBoxTool1";
            comboBoxTool6.ValueList = valueList4;
            popupMenuTool32.SharedPropsInternal.Caption = "Transaction Reports";
            popupMenuTool32.Tools.AddRange(new Infragistics.Win.UltraWinToolbars.ToolBase[] {
            buttonTool268,
            buttonTool269,
            buttonTool270,
            buttonTool271,
            buttonTool272,
            buttonTool273,
            buttonTool103});
            popupMenuTool34.SharedPropsInternal.Caption = "Sales Reports";
            popupMenuTool34.Tools.AddRange(new Infragistics.Win.UltraWinToolbars.ToolBase[] {
            buttonTool274,
            buttonTool275,
            buttonTool276,
            buttonTool277,
            buttonTool278,
            buttonTool281,
            buttonTool282,
            buttonTool280,
            buttonTool279,
            buttonTool284,
            buttonTool283,
            buttonTool285,
            buttonTool167}); //PRIMEPOS-3360
            popupMenuTool36.SharedPropsInternal.Caption = "Item Reports";
            popupMenuTool36.Tools.AddRange(new Infragistics.Win.UltraWinToolbars.ToolBase[] {
            buttonTool286,
            buttonTool287,
            buttonTool288,
            buttonTool289,
            buttonTool290});
            buttonTool94.SharedPropsInternal.Caption = "Tax Override Report";
            buttonTool96.SharedPropsInternal.Caption = "Credit Card Profiles";
            buttonTool99.SharedPropsInternal.Caption = "Inventory Shrinkage Report";
            buttonTool101.SharedPropsInternal.Caption = "Transaction Fee";
            buttonTool104.SharedPropsInternal.Caption = "Transaction Fee Report";
            buttonTool291.SharedPropsInternal.Caption = "Pseudoephedrine Sales Logs"; //PRIMEPOS-3360
            this.ultMenuBar.Tools.AddRange(new Infragistics.Win.UltraWinToolbars.ToolBase[] {
            popupMenuTool6,
            popupMenuTool7,
            popupMenuTool10,
            buttonTool33,
            buttonTool34,
            buttonTool35,
            buttonTool36,
            buttonTool37,
            buttonTool38,
            buttonTool39,
            buttonTool40,
            buttonTool42,
            buttonTool43,
            buttonTool44,
            buttonTool45,
            buttonTool46,
            buttonTool47,
            buttonTool48,
            buttonTool49,
            buttonTool50,
            buttonTool51,
            buttonTool52,
            buttonTool53,
            buttonTool54,
            buttonTool55,
            buttonTool56,
            buttonTool57,
            buttonTool58,
            buttonTool59,
            buttonTool60,
            buttonTool61,
            buttonTool62,
            buttonTool63,
            buttonTool64,
            taskPaneTool1,
            taskPaneTool2,
            taskPaneTool3,
            taskPaneTool4,
            taskPaneTool5,
            taskPaneTool6,
            popupMenuTool12,
            buttonTool69,
            popupMenuTool13,
            buttonTool80,
            buttonTool81,
            buttonTool82,
            buttonTool83,
            buttonTool84,
            buttonTool85,
            buttonTool86,
            buttonTool87,
            buttonTool88,
            buttonTool89,
            buttonTool90,
            buttonTool91,
            buttonTool92,
            popupMenuTool14,
            buttonTool105,
            stateButtonTool2,
            buttonTool106,
            buttonTool107,
            buttonTool108,
            buttonTool109,
            buttonTool110,
            buttonTool111,
            buttonTool112,
            buttonTool113,
            buttonTool114,
            buttonTool115,
            popupMenuTool15,
            buttonTool117,
            buttonTool118,
            buttonTool119,
            popupMenuTool16,
            buttonTool123,
            buttonTool124,
            buttonTool125,
            buttonTool126,
            buttonTool127,
            buttonTool128,
            buttonTool129,
            buttonTool130,
            buttonTool131,
            buttonTool132,
            buttonTool133,
            popupMenuTool18,
            buttonTool135,
            buttonTool137,
            buttonTool139,
            buttonTool21,
            popupMenuTool20,
            buttonTool122,
            buttonTool141,
            buttonTool143,
            buttonTool145,
            buttonTool147,
            buttonTool149,
            buttonTool151,
            buttonTool153,
            buttonTool155,
            buttonTool158,
            buttonTool160,
            buttonTool162,
            buttonTool164,
            buttonTool166,
            buttonTool168,
            buttonTool170,
            buttonTool172,
            comboBoxTool2,
            buttonTool174,
            buttonTool176,
            popupMenuTool22,
            buttonTool179,
            buttonTool180,
            buttonTool182,
            buttonTool184,
            buttonTool186,
            buttonTool187,
            buttonTool188,
            buttonTool190,
            buttonTool192,
            buttonTool193,
            buttonTool185,
            buttonTool194,
            buttonTool197,
            buttonTool198,
            buttonTool199,
            buttonTool201,
            buttonTool203,
            buttonTool205,
            buttonTool202,
            buttonTool207,
            buttonTool208,
            popupMenuTool24,
            mdiWindowListTool2,
            buttonTool209,
            buttonTool213,
            popupMenuTool25,
            buttonTool212,
            buttonTool217,
            buttonTool219,
            buttonTool221,
            buttonTool223,
            buttonTool225,
            popupMenuTool28,
            buttonTool227,
            buttonTool226,
            buttonTool229,
            buttonTool231,
            buttonTool233,
            buttonTool234,
            buttonTool237,
            buttonTool238,
            buttonTool240,
            buttonTool242,
            buttonTool214,
            buttonTool243,
            buttonTool245,
            buttonTool247,
            buttonTool249,
            comboBoxTool3,
            popupMenuTool30,
            buttonTool251,
            buttonTool253,
            buttonTool256,
            buttonTool257,
            buttonTool260,
            buttonTool261,
            buttonTool263,
            buttonTool265,
            comboBoxTool4,
            comboBoxTool6,
            popupMenuTool32,
            popupMenuTool34,
            popupMenuTool36,
            buttonTool94,
            buttonTool96,
            buttonTool99,
            buttonTool101,
            buttonTool104,
            buttonTool291}); //PRIMEPOS-3360
            this.ultMenuBar.UseLargeImagesOnToolbar = true;
            this.ultMenuBar.ToolClick += new Infragistics.Win.UltraWinToolbars.ToolClickEventHandler(this.ultraToolbarsManager1_ToolClick);
            this.ultMenuBar.MouseEnterElement += new Infragistics.Win.UIElementEventHandler(this.ultMenuBar_MouseEnterElement);
            // 
            // _frmMain_Toolbars_Dock_Area_Left
            // 
            this._frmMain_Toolbars_Dock_Area_Left.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._frmMain_Toolbars_Dock_Area_Left.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(158)))), ((int)(((byte)(190)))), ((int)(((byte)(245)))));
            this._frmMain_Toolbars_Dock_Area_Left.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Left;
            this._frmMain_Toolbars_Dock_Area_Left.ForeColor = System.Drawing.Color.Black;
            this._frmMain_Toolbars_Dock_Area_Left.Location = new System.Drawing.Point(0, 187);
            this._frmMain_Toolbars_Dock_Area_Left.Name = "_frmMain_Toolbars_Dock_Area_Left";
            this._frmMain_Toolbars_Dock_Area_Left.Size = new System.Drawing.Size(0, 291);
            this._frmMain_Toolbars_Dock_Area_Left.ToolbarsManager = this.ultMenuBar;
            // 
            // _frmMain_Toolbars_Dock_Area_Top
            // 
            this._frmMain_Toolbars_Dock_Area_Top.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._frmMain_Toolbars_Dock_Area_Top.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(158)))), ((int)(((byte)(190)))), ((int)(((byte)(245)))));
            this._frmMain_Toolbars_Dock_Area_Top.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Top;
            this._frmMain_Toolbars_Dock_Area_Top.ForeColor = System.Drawing.Color.Black;
            this._frmMain_Toolbars_Dock_Area_Top.Location = new System.Drawing.Point(0, 0);
            this._frmMain_Toolbars_Dock_Area_Top.Name = "_frmMain_Toolbars_Dock_Area_Top";
            this._frmMain_Toolbars_Dock_Area_Top.Size = new System.Drawing.Size(784, 187);
            this._frmMain_Toolbars_Dock_Area_Top.ToolbarsManager = this.ultMenuBar;
            // 
            // _frmMain_Toolbars_Dock_Area_Bottom
            // 
            this._frmMain_Toolbars_Dock_Area_Bottom.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._frmMain_Toolbars_Dock_Area_Bottom.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(158)))), ((int)(((byte)(190)))), ((int)(((byte)(245)))));
            this._frmMain_Toolbars_Dock_Area_Bottom.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Bottom;
            this._frmMain_Toolbars_Dock_Area_Bottom.ForeColor = System.Drawing.Color.Black;
            this._frmMain_Toolbars_Dock_Area_Bottom.Location = new System.Drawing.Point(0, 478);
            this._frmMain_Toolbars_Dock_Area_Bottom.Name = "_frmMain_Toolbars_Dock_Area_Bottom";
            this._frmMain_Toolbars_Dock_Area_Bottom.Size = new System.Drawing.Size(784, 0);
            this._frmMain_Toolbars_Dock_Area_Bottom.ToolbarsManager = this.ultMenuBar;
            // 
            // bwCheckAlwaysRunning
            // 
            this.bwCheckAlwaysRunning.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bwCheckAlwaysRunning_DoWork);
            // 
            // tmrCheckRunningTasks
            // 
            this.tmrCheckRunningTasks.Interval = 600000;
            this.tmrCheckRunningTasks.Tick += new System.EventHandler(this.tmrCheckRunningTasks_Tick);
            // 
            // pictSign
            // 
            this.pictSign.BackColor = System.Drawing.Color.White;
            this.pictSign.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictSign.Cursor = System.Windows.Forms.Cursors.Cross;
            this.pictSign.Location = new System.Drawing.Point(228, 207);
            this.pictSign.Margin = new System.Windows.Forms.Padding(2);
            this.pictSign.Name = "pictSign";
            this.pictSign.Size = new System.Drawing.Size(335, 245);
            this.pictSign.TabIndex = 33;
            this.pictSign.TabStop = false;
            this.pictSign.Visible = false;
            // 
            // frmMain
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
            this.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.ClientSize = new System.Drawing.Size(784, 531);
            this.Controls.Add(this.pictSign);
            this.Controls.Add(this.splitter1);
            this.Controls.Add(this.ultraExplorerBar1);
            this.Controls.Add(this._frmMain_Toolbars_Dock_Area_Right);
            this.Controls.Add(this._frmMain_Toolbars_Dock_Area_Left);
            this.Controls.Add(this._frmMain_Toolbars_Dock_Area_Bottom);
            this.Controls.Add(this.ultraStatusBarVendor);
            this.Controls.Add(this.ultraStatusBar);
            this.Controls.Add(this._frmMain_Toolbars_Dock_Area_Top);
            this.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.ForeColor = System.Drawing.Color.Black;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.IsMdiContainer = true;
            this.KeyPreview = true;
            this.Name = "frmMain";
            this.Text = "Prime POS";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Closing += new System.ComponentModel.CancelEventHandler(this.frmMain_Closing);
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.frmMain_Paint);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmMain_KeyDown);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.frmMain_KeyUp);
            this.Leave += new System.EventHandler(this.frmMain_Leave);
            this.Resize += new System.EventHandler(this.frmMain_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.ultraExplorerBar1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraStatusBar1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraStatusBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraStatusBarVendor)).EndInit();
            this.cntMenuPOStatus.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ultMenuBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictSign)).EndInit();
            this.ResumeLayout(false);

        }
        #endregion

        private void MenuClick(string key)
        {

            explorerBarWidth = ultraExplorerBar1.Width;

            switch (key)
            {
                case "Exit":
                    this.Close();
                    break;
                case POSMenuItems.ManageMessages:
                    if (oMessaging != null)
                    {
                        oMessaging.DisplayInbox();
                    }
                    break;
                case "About":
                    frmAboutDialoug ofrm = new frmAboutDialoug();
                    ofrm.ShowDialog(this);
                    break;
                //Following Code added by Krishna on 16 December 2011
                case "OnlineHelp":
                    try
                    {
                        string Path = Application.ExecutablePath;
                        System.IO.FileInfo FI = new System.IO.FileInfo(Path);
                        string OnlineHelpPath = FI.Directory.FullName + "\\WebHelp\\Index.htm";
                        string Subkey = @"HTTP\shell\open\command";
                        RegistryKey registrykey = Registry.ClassesRoot.OpenSubKey(Subkey, false);
                        string strDefaultBrowser = ((string)registrykey.GetValue(null, null)).Split('"')[1];
                        System.Diagnostics.ProcessStartInfo psi = new ProcessStartInfo(strDefaultBrowser, OnlineHelpPath);
                        System.Diagnostics.Process.Start(psi);
                    }
                    catch (Exception ex)
                    {
                        clsCoreUIHelper.ShowErrorMsg(ex.Message);
                    }
                    break;
                //Till here added by Krishna 

                case POSMenuItems.ChangePassword:
                    //Following if-condition is added by shitaljit on 5 April 2013
                    //For PRIMEPOS-382 Add a new security manager option that will allow/disallow access to the change password menu option
                    //if (UserPriviliges.getPermission(UserPriviliges.Modules.AdminFunc.ID, UserPriviliges.Screens.ChangeLoginUserPassword.ID, 0, UserPriviliges.Screens.ChangeLoginUserPassword.Name) == true) //PRIMEPOS-2484 04-Jun-2020 JY Commented
                    if (UserPriviliges.getPermission(UserPriviliges.Modules.AppSettings.ID, UserPriviliges.Screens.ChangeLoginUserPassword.ID, 0, UserPriviliges.Screens.ChangeLoginUserPassword.Name) == true) //PRIMEPOS-2484 04-Jun-2020 JY Added
                    {
                        frmChangePassword ofrmChngPwd = new frmChangePassword();
                        ofrmChngPwd.EnsurePasswordChange = false;
                        ofrmChngPwd.ShowDialog(this);
                    }
                    break;
                case POSMenuItems.PriceUpdateAuto:
                    frmSearchPriceUpdate ofrmPU = new frmSearchPriceUpdate();
                    //ofrmPU.ShowDialog(this);
                    break;
                case POSMenuItems.MyStore:
                    frmViewMyStore ofrmMS = new frmViewMyStore();
                    ofrmMS.ShowDialog(this);
                    break;
                case POSMenuItems.CreateTimesheet:
                    frmCreateTimesheet ofrmCreateTimesheet = new frmCreateTimesheet();
                    SetFormAtCenter(ofrmCreateTimesheet);
                    //ofrmCreateTimesheet.ShowDialog(this);
                    break;
                case "Office2000":
                    setLookFeelStyle(LookandFeelStypeENUM.Office2000);
                    break;
                case "Office2003":
                    setLookFeelStyle(LookandFeelStypeENUM.Office2003);
                    break;
                case "VisualStudio2005":
                    setLookFeelStyle(LookandFeelStypeENUM.VisualStudio2005);
                    break;
                case "OfficeXP":
                    setLookFeelStyle(LookandFeelStypeENUM.OfficeXP);
                    break;
                case POSMenuItems.ItemFileListing:
                    SetFormAtCenter(RptItemFileListing);
                    RptItemFileListing.Show();
                    break;
                case POSMenuItems.VendorFileListing:
                    SetFormAtCenter(RptVendor);
                    RptVendor.Show();
                    break;
                case POSMenuItems.InventoryStatusReport:
                    SetFormAtCenter(rptInventoryStatusReport);
                    rptInventoryStatusReport.Show();
                    break;
                case POSMenuItems.TimesheetReport:
                    SetFormAtCenter(RptTimesheet);
                    RptTimesheet.Show();
                    break;
                case POSMenuItems.PhysicalInventorySheet:
                    SetFormAtCenter(rptPhysicalInventorySheet);
                    rptPhysicalInventorySheet.Show();
                    break;
                case POSMenuItems.RInventoryReceived:
                    SetFormAtCenter(RptInventoryReceived);
                    RptInventoryReceived.Show();
                    break;
                case POSMenuItems.ItemReOrder:
                    SetFormAtCenter(RptItemReOrder);
                    RptItemReOrder.Show();
                    break;
                case POSMenuItems.CostAnalysisReport:
                    SetFormAtCenter(RptCostAnalysis);
                    RptCostAnalysis.Show();
                    break;
                case POSMenuItems.SalesSummaryByVendor:
                    frmRptSalesbyVendor ofrmSSV = new frmRptSalesbyVendor();
                    SetFormAtCenter(ofrmSSV);
                    ofrmSSV.Show();
                    break;
                case POSMenuItems.DeadStockReport:
                    SetFormAtCenter(RptDeadStockReport);
                    RptDeadStockReport.Show();
                    break;
                case POSMenuItems.SalesComparison:
                    clsYearlySalesComparison oClsYearlySalesComparison = new clsYearlySalesComparison();
                    oClsYearlySalesComparison.ShowReport();
                    break;
                case POSMenuItems.ItemListByVendor:
                    SetFormAtCenter(RptItemListingByVendor);
                    RptItemListingByVendor.Show();
                    break;
                case POSMenuItems.IIASItemFileListing:
                    frmIIASItemList oFrmIIASItemList = new frmIIASItemList();
                    SetFormAtCenter(oFrmIIASItemList);
                    oFrmIIASItemList.Show();
                    break;
                #region Sprint-25 - PRIMEPOS-2379 09-Feb-2017 JY Added
                case POSMenuItems.PSEItemFileListing:
                    frmPSEItemList ofrmPSEItemList = new frmPSEItemList();
                    SetFormAtCenter(ofrmPSEItemList);
                    ofrmPSEItemList.Show();
                    break;
                #endregion

                //Following Added By Krishna on 2 June 2011
                case POSMenuItems.TransactionTime:
                    frmRptTransactionTime oFrmTransactionTime = new frmRptTransactionTime();
                    SetFormAtCenter(oFrmTransactionTime);
                    oFrmTransactionTime.Show();
                    break;
                //Till here Added By krishna on 2 June 2011

                //Following Added By Krishna on 13 June 2011
                case POSMenuItems.ItemSaleHistoricalCompare:
                    frmRptItemConsumptionCompare oItemConsumptionCompare = new frmRptItemConsumptionCompare();
                    SetFormAtCenter(oItemConsumptionCompare);
                    oItemConsumptionCompare.Show();
                    break;
                //Till here Added By krishna on 13 June 2011

                //Following Added By Krishna on 13 June 2011
                case POSMenuItems.StnCloseCash:
                    frmRptStnCloseCash oFrmStnCloseCash = new frmRptStnCloseCash();
                    SetFormAtCenter(oFrmStnCloseCash);
                    oFrmStnCloseCash.Show();
                    break;
                //Till here Added By krishna on 15 June 2011

                //Following Added By Krishna on 10 October 2011
                case POSMenuItems.SystemNote:
                    SetFormAtCenter(FrmCustNotes);
                    FrmCustNotes.Show();
                    break;
                //Till here Added By krishna on 10 October 2011

                //Added By shitaljit(QuicSolv) on 10 oct 2011
                case POSMenuItems.ManageNotes:
                    SetFormAtCenter(oFrmCustomerNotes);
                    oFrmCustomerNotes.Show();
                    break;
                //End of Added By shitaljit(QuicSolv) on 10 oct 2011
                case POSMenuItems.IIASTransSummary:
                    frmIIASItemByTrans oFrmIIASItemByTrans = new frmIIASItemByTrans();
                    SetFormAtCenter(oFrmIIASItemByTrans);
                    oFrmIIASItemByTrans.Show();
                    break;
                case POSMenuItems.IIASPaymentTransaction:
                    frmIIASTransByPayment oFrmIIASTransByPayment = new frmIIASTransByPayment();
                    SetFormAtCenter(oFrmIIASTransByPayment);
                    oFrmIIASTransByPayment.Show();
                    break;
                case "KeyPad":
                    btnNumericKeyPad_Click(null, null);
                    break;
                case "Log_Off":
                case "Lock":
                    try
                    {
                        string oldUser = POS_Core.Resources.Configuration.UserName;
                        clsLogin m_oLogin = new clsLogin();
                        m_oLogin.ConnString = POS_Core.Resources.Configuration.ConnectionString;
                        m_oLogin.login(LoginENUM.Lock);
                        StartupSettings();
                        if (oldUser != POS_Core.Resources.Configuration.UserName)
                        {
                            foreach (Form ofrmChild in this.MdiChildren)
                            {
                                ofrmChild.Close();
                            }

                        }
                        m_oLogin = null;
                    }
                    catch (Exception exp)
                    {
                        clsCoreUIHelper.ShowErrorMsg(exp.Message);
                        m_oLogin = null;
                    }
                    break;
                default:
                    ShowForm(key);
                    break;
                case "Item Price Change Rport":
                    SetFormAtCenter(rptItemPriceLog);
                    oRptItemPriceLog.Show();
                    break;
                //Add By SRT (Sachin) Date : 27 Nov 2009
                case "Item Price Change and Label Report":
                    SetFormAtCenter(rptItemPriceLogLable);
                    oRptItemPriceLogLable.Show();
                    break;
                case "PriceOverriddenReport":
                    SetFormAtCenter(rptPriceOverriden);
                    oRptPriceOverridden.Show();
                    break;
                //Changes by Atul Joshi on 12-11-2010   
                case "ItemDepartment":
                    SetFormAtCenter(changeItemDepartment);
                    oRptItemChangeDepartment.Show();
                    break;
                //End Changes by Atul Joshi on 12-11-2010 

                //Added By Shitaljit(QuicSolv) on 9 May 2011
                case "Set Selling Price":
                    SetFormAtCenter(setSellingPrice);
                    oRptSetSellingPrice.Show();
                    break;
                //Till here Added By Shitaljit(QuicSolv)
                //Start: Added by Amit Date 18 july 2011
                case POSMenuItems.ItemSalesPerformance:
                    frmRptItemSalesPerformance oRptItemSalesPerformance = new frmRptItemSalesPerformance();
                    SetFormAtCenter(oRptItemSalesPerformance);
                    oRptItemSalesPerformance.Show();
                    break;
                //End
                //Start: Added by Amit Date 19 Aug 2011
                case POSMenuItems.RxCheckout:
                    frmRptRxCheckout oRptRxCheckout = new frmRptRxCheckout();
                    SetFormAtCenter(oRptRxCheckout);
                    oRptRxCheckout.Show();
                    break;
                //End
                //Added By shitaljit for ROADetails Report
                case POSMenuItems.ROADetails:
                    SetFormAtCenter(RptROADetails);
                    RptROADetails.Show();
                    break;

                //Added By shitaljit for Setting color Scheme for View POS Trans.
                case POSMenuItems.ViewPOSTransColorScheme:
                    SetFormAtCenter(ViewPOSTransColorScheme);
                    ViewPOSTransColorScheme.Show();
                    break;
                //Added By shitaljit for Processign on hold POs.
                case POSMenuItems.ProcessOnHoldPO:
                    frmPOOnHold ofrmPOOnHold = new frmPOOnHold();
                    SetFormAtCenter(ofrmPOOnHold);
                    ofrmPOOnHold.Show();
                    break;
                #region Sprint-19 - 2171 08-Apr-2015 JY Added 
                case "WhatsNew":
                    try
                    {
                        //string Path = Application.ExecutablePath;
                        //System.IO.FileInfo FI = new System.IO.FileInfo(Path);
                        //string strReleaseNotePath = FI.Directory.FullName + "\\ReleaseNotes.pdf";
                        //string Subkey = @"HTTP\shell\open\command";
                        //RegistryKey registrykey = Registry.ClassesRoot.OpenSubKey(Subkey, false);
                        //string strDefaultBrowser = ((string)registrykey.GetValue(null, null)).Split('"')[1];
                        //System.Diagnostics.ProcessStartInfo psi = new ProcessStartInfo(strDefaultBrowser, strReleaseNotePath);
                        //System.Diagnostics.Process.Start(psi);

                        //18-Aug-2021 JY Added
                        string pdfFileName = Application.StartupPath + "\\ReleaseNotes.pdf";
                        if (File.Exists(pdfFileName))
                        {
                            System.Diagnostics.Process.Start(pdfFileName);
                        }
                        else
                        {
                            MessageBox.Show("File not exist at this path.", "What's New", MessageBoxButtons.OK);
                        }
                    }
                    catch (Exception ex)
                    {
                        clsCoreUIHelper.ShowErrorMsg(ex.Message);
                    }
                    break;
                #endregion
                #region Sprint-20 03-Jun-2015 JY Added for auto updater
                case "CheckForUpdates":
                    checkforUpdates();
                    break;
                case "StationSettings":
                    ShowStationsSettings();
                    break;
                #endregion
                #region PRIMEPOS-2034 05-Mar-2018 JY Added
                case POSMenuItems.CouponReport:
                    SetFormAtCenter(RptCouponReport);
                    RptCouponReport.Show();
                    break;
                #endregion
                case "ReconciliationDeliveryReport":    //PRIMERX-7688 NileshJ Added
                    SetFormAtCenter(ReconciliationDeliveryReport);
                    ReconciliationDeliveryReport.Show();
                    break;
                case POSMenuItems.ProcessEvertecSettlement:
                    try
                    {
                        if (Configuration.CPOSSet.PaymentProcessor == clsPOSDBConstants.EVERTEC)
                        {
                            String displayMessage = String.Empty;
                            String _posSettings = Configuration.CPOSSet.SigPadHostAddr;

                            string hostAddress = _posSettings.Split(':')[0];
                            string hostPort = _posSettings.Split(':')[1].Split('/')[0];

                            device = EvertechProcessor.getInstance(hostAddress, Convert.ToInt32(hostPort));

                            if (!device.isLoggedOn)
                                device.Logon(Configuration.CPOSSet.TerminalID, Configuration.StationID, Configuration.CashierID);
                            EvertecSettleResponse = device.Settle();
                            displayMessage = EvertecSettleResponse.Message;

                            AuditTrail objAuditTrail = new AuditTrail();
                            objAuditTrail.InsertAuditTrail();

                            clsUIHelper.ShowOKMsg(displayMessage);
                        }
                        else
                        {
                            clsUIHelper.ShowErrorMsg("Please use the Evertec Processor");
                        }
                    }
                    catch (Exception ex)
                    {
                        Logs.Logger("frmMain", "IIASFileUpload()", "Starting Item Import");
                        throw ex;
                    }
                    break;
                #region PRIMEPOS-1633 28-Dec-2020 JY Added
                case "SetItemTax":
                    SetFormAtCenter(setItemTax);
                    ofrmSetItemTax.Show();
                    break;
                #endregion
                #region PRIMEPOS-2391 23-Jul-2021 JY Added
                case "TaxOverrideReport":
                    SetFormAtCenter(rptfrmTaxOverrideReport);
                    ofrmTaxOverrideReport.Show();
                    break;
                #endregion
                #region PRIMEPOS-138 14-Feb-2021 JY Added
                case "InventoryShrinkageReport":
                    SetFormAtCenter(rptfrmInvShrinkage);
                    ofrmInvShrinkage.Show();
                    break;
                    #endregion
            }
        }

        private void ultraToolbarsManager1_ToolClick(object sender, Infragistics.Win.UltraWinToolbars.ToolClickEventArgs e)
        {
            try
            {
                string key = "";
                if (e.Tool.OwningToolbar != null)
                    key = e.Tool.Key.Substring(1);
                else
                    key = e.Tool.Key;
                MenuClick(key);
            }
            catch (Exception exp)
            {
                clsCoreUIHelper.ShowErrorMsg(exp.Message);
            }
        }

        private void ultraExplorerBar1_ItemClick(object sender, Infragistics.Win.UltraWinExplorerBar.ItemEventArgs e)
        {
            try
            {
                MenuClick(e.Item.Key);
            }
            catch (Exception exp)
            {
                clsCoreUIHelper.ShowErrorMsg(exp.Message);
            }
        }

        public static Form ShowForm(string ItemKey)
        {
            string stableName = "";
            string sFormCaption = "";
            string sLabelText1 = "";
            string sLabelText2 = "";

            selectedForm = null;
            Type oType;
            oType = CreateFormObject(ItemKey, ref stableName, ref sFormCaption, ref sLabelText1, ref sLabelText2);

            bool isExist = false;
            if (oType != null)
            {
                if (ItemKey == POSMenuItems.Sales || ItemKey == POSMenuItems.SaleReturns)
                {
                    try
                    {
                        frmMain.HideCloseButton = true;
                        CreateParams myCp = frmMain.getInstance().CreateParams;
                        foreach (Form frm in frmMain.getInstance().MdiChildren)
                            frm.Close();
                    }
                    catch (Exception)
                    { }
                }

                foreach (Form ofrm in ofrmMain.MdiChildren)
                {
                    if (ofrm.GetType() == oType)
                    {
                        selectedForm = ofrm;
                        if (ofrm is frmSearchMain)
                        {
                            if (((frmSearchMain)ofrm).SearchTable == stableName)
                            {
                                isExist = true;
                                break;
                            }
                        }
                        else if (ofrm is frmViewEODStation) //PRIMEPOS-2494 26-Feb-2018 JY Added condition to pop up View EOD and Station Close at same time
                        {
                            if (((frmViewEODStation)ofrm).SearchTable == stableName)
                            {
                                isExist = true;
                                break;
                            }
                        }
                        else
                        {
                            isExist = true;
                            break;
                        }
                    }
                    else
                    {
                        isExist = false;
                    }
                }

                if (isExist == true)
                {
                    selectedForm.Activate();
                }
                else
                {
                    selectedForm = null;
                    selectedForm = (Form)Activator.CreateInstance(oType);
                    if (stableName == clsPOSDBConstants.StationCloseHeader_tbl || stableName == clsPOSDBConstants.EndOfDay_tbl)
                    {
                        frmViewEODStation oMainSearch;
                        oMainSearch = (frmViewEODStation)selectedForm;
                        oMainSearch.SearchTable = stableName;
                        oMainSearch.FormCaption = sFormCaption;
                        oMainSearch.LabelText1 = sLabelText1;
                        oMainSearch.LabelText2 = sLabelText2;
                        if (stableName != clsPOSDBConstants.Item_tbl)
                            oMainSearch.DisplayRecordAtStartup = true;
                    }
                    else if (stableName != "")
                    {
                        frmSearchMain oMainSearch;
                        oMainSearch = (frmSearchMain)selectedForm;
                        oMainSearch.SearchTable = stableName;
                        oMainSearch.FormCaption = sFormCaption;
                        oMainSearch.LabelText1 = sLabelText1;
                        oMainSearch.LabelText2 = sLabelText2;
                        if (stableName != clsPOSDBConstants.Item_tbl)
                            oMainSearch.DisplayRecordAtStartup = true;
                    }
                    SetFormAtCenter(selectedForm);
                }
                //Added By (SRT)Abhishek  Date : 02/05/2009   
                if (selectedForm.Name == POSMenuItems.FormPOLogScreen && frmMain.FromPanel == true)
                    selectedForm.Show();
                //End OF Added By (SRT)Abhishek  Date : 02/05/2009   

                if (ItemKey == POSMenuItems.Sales)
                    ((frmPOSTransaction)selectedForm).setTransactionType(POS_Core.TransType.POSTransactionType.Sales);//added by sandeep to run new form
                else if (ItemKey == POSMenuItems.SaleReturns)
                    ((frmPOSTransaction)selectedForm).setTransactionType(POS_Core.TransType.POSTransactionType.SalesReturn);//added by sandeep to run new form
            }
            return selectedForm;
        }

        private const int CP_NOCLOSE_BUTTON = 0x200;
        public static bool HideCloseButton = false;
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams myCp = base.CreateParams;
                if (frmMain.HideCloseButton == true)
                {
                    myCp.ClassStyle = myCp.ClassStyle | CP_NOCLOSE_BUTTON;
                }
                return myCp;
            }
        }

        private void frmMain_Load(object sender, System.EventArgs e)
        {
            try
            {
                StartupSettings();

                //Changes by Atul Joshi on 15 Feb 2011  
                //To check whether Device.ocx is register or not 
                //If register then Its ok
                //Else if will register the Device.ocx dll 
                Microsoft.Win32.RegistryKey myKey = Microsoft.Win32.Registry.ClassesRoot.OpenSubKey("Device.OCXDevice");
                string Path = System.IO.Directory.GetCurrentDirectory();
                if (myKey == null)
                {
                    Process Proc = new System.Diagnostics.Process();
                    Proc.StartInfo.FileName = "regsvr32.exe";
                    string Path2 = "C:\\Device.ocx";
                    if (!System.IO.File.Exists(Path2))
                        System.IO.File.Copy(Path + "\\Device.ocx", Path2);
                    Proc.StartInfo.Arguments = Path2 + " -s";
                    Proc.StartInfo.CreateNoWindow = true;
                    Proc.Start();
                    Proc.WaitForExit();
                    Proc.Close();
                }
                //End of Changes by Atul Joshi on 15 Feb 2011 

                /*oMessaging=new Messaging.Messaging(Configuration.ServerName,Configuration.DatabaseName,Messaging.AppType.PrimePOS,Configuration.UserName);
                oMessaging.OnMsgArrivedEvent+=new Messaging.Messaging.OnMsgArrived(OnMsgArrived);
                Messaging.Messaging.OnFormatFormEvent+=new Messaging.Messaging.OnFormatForm(OnFormatForm);
                oMessaging.RepeatTime=5000;
                oMessaging.Parent=this;
                oMessaging.StartSendRecieve();

                oMessaging.DisplayInbox();
                */
                // Added By Dharmendra (SRT) on 02-09-08

                //Modified by Dharmendra(SRT) on Nov-13-08 for removing Pin PadPOS_Core.Resources.Configuration Setting (System.Configuration.ConfigurationManager.AppSettings["USE_PINPAD"])from app.config
                // and accessing the same from POSSET  to check whether the pin pad is initialized or not

                //changed by SRT(Abhishek) Date : 21/10/2009 
                if (POS_Core.Resources.Configuration.CPOSSet.PinPadModel.Trim() != "TT8500" && POS_Core.Resources.Configuration.CPOSSet.UsePinPad != null && POS_Core.Resources.Configuration.CPOSSet.UsePinPad == true && POS_Core.Resources.Configuration.CPOSSet.PaymentProcessor != "XLINK" && POS_Core.Resources.Configuration.CPOSSet.PaymentProcessor != "HPSPAX") // this line is changed by Dharmendra(SRT)
                {
                    if (frmPinPad.DefaultInstance.InitializePinPad() == 1)
                    {

                        clsCoreUIHelper.ShowBtnErrorMsg("Pin Pad Not Initialized.", "Pin Pad Failure", MessageBoxButtons.OK);
                    }
                }
                //End Of changed by SRT(Abhishek) Date : 21/10/2009
                // Modified ended 

                //Added by Krishna on 13 July 2012
                #region 19-Nov-2015 JY Commented because it pop ups even if primepo connected
                //if (Configuration.CPOSSet.USePrimePO == true && PrimePOUtil.IsPOConnected == false && PrimePOUtil.isConnected == false)
                //{
                //    clsCoreUIHelper.ShowErrorMsg(" PrimePO is Disconnected ");
                //}                
                #endregion
                //this.ultMenuBar.Tools.Remove(buttonTool90
                //End of added by Krishna

                #region Sprint-20 25-May-2015 JY Added for Auto updater
                tmrCheckRunningTasks.Enabled = false;

                oAppUpdater.SetParentForm(this);
                if (Configuration.CInfo.AllowAutomaticUpdates == true)
                {
                    oAppUpdater.bw.RunWorkerAsync();
                    //Commented above and added below for testing purpose
                    //oAppUpdater.bws.RunWorkerAsync();

                    Random rd = new Random();
                    int schedulingSlot = 240; // mins //240 - for testing you can set it to 2
                    int randonTime = rd.Next(1, schedulingSlot);

                    int randonTimeMilliseconds = randonTime * 60000;
                    this.tmrAutuUpdate = new System.Windows.Forms.Timer();
                    this.tmrAutuUpdate.Enabled = true;
                    this.tmrAutuUpdate.Interval = randonTimeMilliseconds;
                    this.tmrAutuUpdate.Tick += new EventHandler(tmrAutuUpdate_Tick);
                    this.tmrAutuUpdate.Start();
                }

                #region PRIMEPOS-3187
                if (Configuration.CSetting.PrimeRxPayBGStatusUpdate)
                {
                    tmrPrimeRxPayBGStatusUpdate = new System.Timers.Timer();
                    tmrPrimeRxPayBGStatusUpdate.Elapsed += tmrPrimeRxPayBGStatusUpdate_Tick;
                    tmrPrimeRxPayBGStatusUpdate.Interval = Configuration.CSetting.PrimeRxPayStatusUpdateIntervalInMin * 60000;// 1 sec = 1000, 60 sec = 60000
                    tmrPrimeRxPayBGStatusUpdate.Enabled = true;
                    tmrPrimeRxPayBGStatusUpdate.Start();
                }
                #endregion

                if (POS_Core.Resources.Configuration.ArgumentEmpty == false)
                {
                    if (POS_Core.Resources.Configuration.CInfo.AllowRunningUpdates == true)
                    {
                        bwCheckAlwaysRunning.RunWorkerAsync();
                    }
                }
                ultMenuBar.Tools["CheckForUpdates"].SharedProps.Visible = POS_Core.Resources.Configuration.convertNullToBoolean(POS_Core.Resources.Configuration.CInfo.AllowAutomaticUpdates);
                //ultMenuBar.Tools["StationSettings"].SharedProps.Visible = POS_Core.Resources.Configuration.convertNullToBoolean(POS_Core.Resources.Configuration.CInfo.AllowAutomaticUpdates);    //PRIMEPOS-2859 12-Jun-2020 JY Commented
                #endregion

                #region BatchDelivery - NileshJ - PRIMERX-7688
                ultMenuBar.Tools["ReconciliationDeliveryReport"].SharedProps.Visible = POS_Core.Resources.Configuration.convertNullToBoolean(POS_Core.Resources.Configuration.CInfo.isPrimeDeliveryReconciliation);
                #endregion
            }
            catch (Exception exp)
            {
                //clsCoreUIHelper.ShowErrorMsg( exp.Message);
                throw (exp);
            }
            finally ///Sprint-20 27-May-2015 JY Added this block for auto updater
            {
                if (POS_Core.Resources.Configuration.ArgumentEmpty == false)
                    tmrCheckRunningTasks.Enabled = true;
                else
                    tmrCheckRunningTasks.Enabled = false;
            }
        }


        public delegate void dlgUpdateMessageStatus(int iMessages);

        public void UpdateMessagesStatus(int iMessages)
        {
            if (this.ultraStatusBar.InvokeRequired)
            {
                dlgUpdateMessageStatus odlg = new dlgUpdateMessageStatus(UpdateMessagesStatus);
                this.Invoke(odlg, new object[] { iMessages });
            }
            else
            {
                this.ultraStatusBar.Panels["Msg"].Text = "New Messages: " + iMessages.ToString();
                this.ultraStatusBar.Panels["Msg"].Visible = true;
                if (this.ultraStatusBar.Panels["Msg"].Appearance.BackColor == Color.Red)
                {
                    this.ultraStatusBar.Panels["Msg"].Appearance.BackColor = Color.Green;
                    this.ultraStatusBar.Panels["Msg"].Appearance.ForeColor = Color.White;
                }
                else
                {
                    this.ultraStatusBar.Panels["Msg"].Appearance.BackColor = Color.Red;
                    this.ultraStatusBar.Panels["Msg"].Appearance.ForeColor = Color.White;
                }

                if (iMessages == -1)
                {
                    this.ultraStatusBar.Panels["Msg"].Appearance = (Infragistics.Win.AppearanceBase)this.ultraStatusBar.Panels[0].Appearance.Clone();
                    this.ultraStatusBar.Panels["Msg"].Text = "";
                    this.ultraStatusBar.Panels["Msg"].Visible = false;
                }
            }
        }

        private void OnMsgArrived(int iMessage)
        {
            try
            {
                for (int i = 1; i < 50; i++)
                {
                    UpdateMessagesStatus(iMessage);
                    System.Threading.Thread.Sleep(150);
                }
                UpdateMessagesStatus(-1);
            }
            catch (Exception exp)
            {
                clsCoreUIHelper.ShowErrorMsg(exp.Message);
            }
        }

        // Added By SRT(Abhishek) Date : 04/02/2009  
        private void OnCountUpdated(int poIncomplete, int poAck, string logMsg, int expired, int overdue, int error, int maxAttempts, int deliveryReceived)
        {
            try
            {
                // this.ultraStatusBarVendor.Panels[POSMenuItems.Queued].Text = "";
                // this.ultraStatusBarVendor.Panels[POSMenuItems.Submitted].Text = "";
                this.ultraStatusBarVendor.Panels[POSMenuItems.Acknowledged].Text = "";
                this.ultraStatusBarVendor.Panels[POSMenuItems.PoLogScreen].Text = "";
                this.ultraStatusBarVendor.Panels[POSMenuItems.Expired].Text = "";
                this.ultraStatusBarVendor.Panels[POSMenuItems.Overdue].Text = "";
                this.ultraStatusBarVendor.Panels[POSMenuItems.Error].Text = "";
                this.ultraStatusBarVendor.Panels[POSMenuItems.MaxAttempts].Text = "";
                this.ultraStatusBarVendor.Panels[POSMenuItems.InComplete].Text = "";
                this.ultraStatusBarVendor.Panels[POSMenuItems.InComplete].Text = "";
                this.ultraStatusBarVendor.Panels[POSMenuItems.DeliveryReceived].Text = ""; // added by atul 25-oct-2010

                // this.ultraStatusBarVendor.Panels[POSMenuItems.Queued].Text = "Queued Orders  :";
                // this.ultraStatusBarVendor.Panels[POSMenuItems.Submitted].Text = "Submitted Orders :";
                this.ultraStatusBarVendor.Panels[POSMenuItems.Acknowledged].Text = "Acknowledged Orders :";
                this.ultraStatusBarVendor.Panels[POSMenuItems.PoLogScreen].Text = "Message :";
                this.ultraStatusBarVendor.Panels[POSMenuItems.Expired].Text = "Expired :";
                this.ultraStatusBarVendor.Panels[POSMenuItems.Overdue].Text = "Overdue :";
                this.ultraStatusBarVendor.Panels[POSMenuItems.Error].Text = "Error :";
                this.ultraStatusBarVendor.Panels[POSMenuItems.MaxAttempts].Text = "MaxAttempts :";
                this.ultraStatusBarVendor.Panels[POSMenuItems.DeliveryReceived].Text = "Delivery Received :"; // added by atul 25-oct-2010
                this.ultraStatusBarVendor.Panels[POSMenuItems.InComplete].Text = "Incomplete Orders :";

                // this.ultraStatusBarVendor.Panels[POSMenuItems.Queued].Text = this.ultraStatusBarVendor.Panels[POSMenuItems.Queued].DisplayText + poQueued.ToString();
                //  this.ultraStatusBarVendor.Panels[POSMenuItems.Submitted].Text = this.ultraStatusBarVendor.Panels[POSMenuItems.Submitted].DisplayText + poSubmitted.ToString();
                this.ultraStatusBarVendor.Panels[POSMenuItems.Acknowledged].Text = this.ultraStatusBarVendor.Panels[POSMenuItems.Acknowledged].DisplayText + poAck.ToString();
                this.ultraStatusBarVendor.Panels[POSMenuItems.PoLogScreen].Text = this.ultraStatusBarVendor.Panels[POSMenuItems.PoLogScreen].Text + logMsg.ToString();
                this.ultraStatusBarVendor.Panels[POSMenuItems.Expired].Text = this.ultraStatusBarVendor.Panels[POSMenuItems.Expired].Text + expired.ToString();
                this.ultraStatusBarVendor.Panels[POSMenuItems.Overdue].Text = this.ultraStatusBarVendor.Panels[POSMenuItems.Overdue].Text + overdue.ToString();
                this.ultraStatusBarVendor.Panels[POSMenuItems.Error].Text = this.ultraStatusBarVendor.Panels[POSMenuItems.Error].Text + error.ToString();
                this.ultraStatusBarVendor.Panels[POSMenuItems.MaxAttempts].Text = this.ultraStatusBarVendor.Panels[POSMenuItems.MaxAttempts].Text + maxAttempts.ToString();
                this.ultraStatusBarVendor.Panels[POSMenuItems.InComplete].Text = this.ultraStatusBarVendor.Panels[POSMenuItems.InComplete].Text + poIncomplete.ToString();
                // added by atul 25-oct-2010
                this.ultraStatusBarVendor.Panels[POSMenuItems.DeliveryReceived].Text = this.ultraStatusBarVendor.Panels[POSMenuItems.DeliveryReceived].Text + deliveryReceived.ToString();
            }
            catch (Exception ex)
            {
                clsCoreUIHelper.ShowErrorMsg(ex.Message);
            }
        }

        // Added By SRT(Abhishek) Date : 04/02/2009  

        public void OnFormatForm(Form ofrm)
        {
            clsCoreUIHelper.setColorSchecme(ofrm);
        }

        public void StartupSettings()
        {
            int height = Screen.GetBounds(this).Height;
            int width = Screen.GetBounds(this).Width;
            if (height < 600 || width < 800)
            {
                clsCoreUIHelper.ShowErrorMsg("Software needs higher resolution.");
            }
            else if (height == 600 && width == 800)
            {
                this.ultraExplorerBar1.Width = 90;
            }


            this.ultraStatusBar.Panels["User"].Text = "User Name: " + Configuration.UserName;
            this.ultraStatusBar.Panels["Station"].Text = "Station: " + Configuration.StationName;
            this.ultraStatusBar.Panels["Pharmacy"].Text = "Pharmacy: " + Configuration.CInfo.StoreName;
            this.ultraStatusBar.Panels["Version"].Text = Application.ProductName.Trim() + " " + Application.ProductVersion;
            //Configuration.StationID is added by shitaljit(QuicSolv) on 29 june 2011
            this.Text = Configuration.ApplicationName + " - [" + Configuration.CInfo.StoreName + "] " + Configuration.StationID;

            if (this.oMessaging != null)
            {
                oMessaging.CurrentUserID = Configuration.UserName;
            }
            try
            {
                //ultMenuBar.LoadFromBinary("MenuBarSetting");
                //ultraExplorerBar1.LoadFromBinary("ExplorerBarSetting");
            }
            catch (Exception) { }

            if (!Configuration.CPrimeEDISetting.UsePrimePO)  //PRIMEPOS-3167 07-Nov-2022 JY Modified
            {
                this.ultraStatusBarVendor.Visible = false;
                usePrimePO = false;
            }

            ApplyUserPriviliges();

            this.ultMenuBar.Toolbars["tlbAdministrativeFunction"].DockedRow = 1;
            this.ultMenuBar.Toolbars["tlbInventoryManagement"].DockedRow = 1;
            this.ultMenuBar.Toolbars["tlbPOSTerminal"].DockedRow = 1;

            this.ultMenuBar.Toolbars["tlbPOSTerminal"].DockedColumn = 0;
            this.ultMenuBar.Toolbars["tlbInventoryManagement"].DockedColumn = 1;
            this.ultMenuBar.Toolbars["tlbAdministrativeFunction"].DockedColumn = 2;

            //if (this.ultMenuBar.Toolbars["tlbPOSTerminal"].Visible) this.ultMenuBar.Toolbars["tlbPOSTerminal"].Visible = Configuration.ShowPOSTbr;
            //if (this.ultMenuBar.Toolbars["tlbInventoryManagement"].Visible) this.ultMenuBar.Toolbars["tlbInventoryManagement"].Visible = Configuration.ShowInvTbr;
            //if (this.ultMenuBar.Toolbars["tlbAdministrativeFunction"].Visible) this.ultMenuBar.Toolbars["tlbAdministrativeFunction"].Visible = Configuration.ShowAdminTbr;
            explorerBarWidth = ultraExplorerBar1.Width;

            PoleDisplay.ClearPoleDisplay();
            clsUIHelper.ShowWelcomeMessage();
            clsCoreUIHelper.setColorSchecme(this);

            this.ultraStatusBar.Appearance.ForeColor = Color.Black;

            if (UserPriviliges.IsUserHasPriviliges(UserPriviliges.Modules.InventoryMgmt.ID, UserPriviliges.Screens.PurchaseOrder.ID, UserPriviliges.Permissions.POAckProcess.ID))
            {
                /*clsEPurchaseOrder.initializePOAcknowledge();
                clsEPurchaseOrder.pingHostAck();
                clsEPurchaseOrder.pingHostPriceUpdate();*/
                //clsEPurchaseOrder.StartThread();
            }
            InstallActivityMonitor();

            //string POSTATUS = Configuration.CPOSSet.POSTATUS;
            //string[] lstPoStatus = POSTATUS.Split('|');
            //POStatusList.AddRange(lstPoStatus);
            //if (POStatusList.Count == 0)
            //{
            AddInStatusList(POSTERROR);
            AddInStatusList(POSTEXPIRED);
            AddInStatusList(POSTACKNOWLEDGED);
            AddInStatusList(POSTDELIVERYRECEIVED); // Added by atul 25-oct-2010
                                                   //}
        }

        private void AddInStatusList(string STTYPE)
        {
            if (!POStatusList.Contains(STTYPE))
            {
                POStatusList.Add(STTYPE);
            }
            if (POStatusList.Count > 3)
            {
                switch (POStatusList[0])
                {
                    case POSTINCOMPLETE:
                        this.InComplete.Checked = false;
                        break;
                    case POSTEXPIRED:
                        this.Expired.Checked = false;
                        break;
                    case POSTERROR:
                        this.Error.Checked = false;
                        break;
                    case POSTOVERDUE:
                        this.Overdue.Checked = false;
                        break;
                    case POSTMAXATTEMPTS:
                        this.maxAttemptToolStripMenuItem.Checked = false;
                        break;
                    case POSTACKNOWLEDGED:
                        this.Acknowledged.Checked = false;
                        break;
                    case POSTDELIVERYRECEIVED:
                        this.DeliveryReceived.Checked = false;
                        break;
                }
                POStatusList.RemoveAt(0);
            }
            //POStatusList.Add(STTYPE);
            //ultraStatusBarVendor.Panels["Queued"].Visible = POStatusList.Contains(POSTQUEUED);
            ultraStatusBarVendor.Panels["Expired"].Visible = POStatusList.Contains(POSTEXPIRED);
            //ultraStatusBarVendor.Panels["Submitted"].Visible = POStatusList.Contains(POSTSUBMITTED);
            ultraStatusBarVendor.Panels["Error"].Visible = POStatusList.Contains(POSTERROR);
            ultraStatusBarVendor.Panels["Overdue"].Visible = POStatusList.Contains(POSTOVERDUE);
            ultraStatusBarVendor.Panels["MaxAttempts"].Visible = POStatusList.Contains(POSTMAXATTEMPTS);
            ultraStatusBarVendor.Panels["Acknowledged"].Visible = POStatusList.Contains(POSTACKNOWLEDGED);
            ultraStatusBarVendor.Panels["InComplete"].Visible = POStatusList.Contains(POSTINCOMPLETE);
            ultraStatusBarVendor.Panels["DeliveryReceived"].Visible = POStatusList.Contains(POSTDELIVERYRECEIVED); //added by atul 25-oct-2010
        }

        private void setToolBarButtonKeys()
        {

        }

        public void ApplyUserPriviliges()
        {
            try
            {
                #region menu bar

                Infragistics.Win.UltraWinToolbars.PopupMenuTool oPopupInvMgmt = (Infragistics.Win.UltraWinToolbars.PopupMenuTool)ultMenuBar.Tools["Inventory_Management"];
                Infragistics.Win.UltraWinToolbars.PopupMenuTool oPopupAdmFun = (Infragistics.Win.UltraWinToolbars.PopupMenuTool)ultMenuBar.Tools["Administrative_Functions"];
                Infragistics.Win.UltraWinToolbars.PopupMenuTool oPopupPOSTrans = (Infragistics.Win.UltraWinToolbars.PopupMenuTool)ultMenuBar.Tools["POS_Terminal"];

                #region Disable All Toolbars/Menubars
                oPopupInvMgmt.SharedProps.ShowInCustomizer = oPopupInvMgmt.SharedProps.Visible = oPopupInvMgmt.SharedProps.Enabled = UserPriviliges.IsUserHasPriviliges(UserPriviliges.Modules.InventoryMgmt.ID);
                oPopupAdmFun.SharedProps.ShowInCustomizer = oPopupAdmFun.SharedProps.Visible = oPopupAdmFun.SharedProps.Enabled = UserPriviliges.IsUserHasPriviliges(UserPriviliges.Modules.AdminFunc.ID);
                oPopupPOSTrans.SharedProps.ShowInCustomizer = oPopupPOSTrans.SharedProps.Visible = oPopupPOSTrans.SharedProps.Enabled = UserPriviliges.IsUserHasPriviliges(UserPriviliges.Modules.POSTransaction.ID);

                //Added By SRT(Abhishek) Date : 02/05/2009
                if (Configuration.CPrimeEDISetting.UsePrimePO)  //PRIMEPOS-3167 07-Nov-2022 JY Modified
                {
                    Infragistics.Win.UltraWinStatusBar.UltraStatusBar vendorStatusBar = (Infragistics.Win.UltraWinStatusBar.UltraStatusBar)ultraStatusBarVendor;
                    vendorStatusBar.Visible = vendorStatusBar.Enabled = UserPriviliges.IsUserHasPriviliges(UserPriviliges.Modules.InventoryMgmt.ID);
                }
                //End Of Added By SRT(Abhishek) Date : 02/05/2009


                ultMenuBar.Toolbars["tlbInventoryManagement"].ShowInToolbarList = ultMenuBar.Toolbars["tlbInventoryManagement"].Visible = UserPriviliges.IsUserHasPriviliges(UserPriviliges.Modules.InventoryMgmt.ID);
                ultMenuBar.Toolbars["tlbAdministrativeFunction"].ShowInToolbarList = ultMenuBar.Toolbars["tlbAdministrativeFunction"].Visible = UserPriviliges.IsUserHasPriviliges(UserPriviliges.Modules.AdminFunc.ID);
                ultMenuBar.Toolbars["tlbPOSTerminal"].ShowInToolbarList = ultMenuBar.Toolbars["tlbPOSTerminal"].Visible = UserPriviliges.IsUserHasPriviliges(UserPriviliges.Modules.POSTransaction.ID);
                //Following Code Added by Krishna on 11 October 2011
                Infragistics.Win.UltraWinToolbars.PopupMenuTool oPopupApplicationMenu = (Infragistics.Win.UltraWinToolbars.PopupMenuTool)ultMenuBar.Tools["LookandFeel"];
                ultMenuBar.Tools["SystemNote"].SharedProps.Visible = ultMenuBar.Tools["SystemNote"].SharedProps.Enabled = UserPriviliges.IsUserHasPriviliges(UserPriviliges.Modules.Notes.ID, UserPriviliges.Screens.SystemNotes.ID);
                //Till here Added by Krishna on 11 October 2011

                foreach (Infragistics.Win.UltraWinToolbars.ToolBase oTool in oPopupInvMgmt.Tools)
                {
                    oTool.SharedProps.Visible = oPopupInvMgmt.SharedProps.Visible;
                    oTool.SharedProps.Enabled = oPopupInvMgmt.SharedProps.Enabled;
                    if (oTool.SharedProps.Visible == false)
                        oTool.SharedProps.Shortcut = new Shortcut();
                }

                foreach (Infragistics.Win.UltraWinToolbars.ToolBase oTool in oPopupAdmFun.Tools)
                {
                    oTool.SharedProps.Visible = oPopupAdmFun.SharedProps.Visible;
                    oTool.SharedProps.Enabled = oPopupAdmFun.SharedProps.Enabled;
                    if (oTool.SharedProps.Visible == false)
                        oTool.SharedProps.Shortcut = new Shortcut();
                }

                foreach (Infragistics.Win.UltraWinToolbars.ToolBase oTool in oPopupPOSTrans.Tools)
                {
                    oTool.SharedProps.Visible = oPopupPOSTrans.SharedProps.Visible;
                    oTool.SharedProps.Enabled = oPopupPOSTrans.SharedProps.Enabled;
                    if (oTool.SharedProps.Visible == false)
                        oTool.SharedProps.Shortcut = new Shortcut();
                }
                //Following Code Added by shitaljit(QuicSolv) on 11 October 2011
                oPopupAdmFun.Tools["Manage Notes"].SharedProps.Visible = ultMenuBar.Tools["Manage Notes"].SharedProps.Enabled = UserPriviliges.IsUserHasPriviliges(UserPriviliges.Modules.Notes.ID, UserPriviliges.Screens.ManageNotes.ID);
                oPopupAdmFun.Tools["PayType"].SharedProps.Visible = ultMenuBar.Tools["PayType"].SharedProps.Enabled = UserPriviliges.IsUserHasPriviliges(UserPriviliges.Modules.AdminFunc.ID, UserPriviliges.Screens.ManagePayType.ID);
                //Till here Added by shitaljit on 11 October 2011
                #endregion

                #region Disable POS Trans Menu

                if (oPopupPOSTrans.SharedProps.Visible == true)
                {
                    ultMenuBar.Tools["Sales"].SharedProps.ShowInCustomizer = ultMenuBar.Tools["Sales"].SharedProps.Visible = ultMenuBar.Tools["Sales"].SharedProps.Enabled = UserPriviliges.IsUserHasPriviliges(UserPriviliges.Modules.POSTransaction.ID, UserPriviliges.Screens.POSTransaction.ID, UserPriviliges.Screens.POSTransaction.ID);
                    ultMenuBar.Tools["Pay_Out"].SharedProps.ShowInCustomizer = ultMenuBar.Tools["Pay_Out"].SharedProps.Visible = ultMenuBar.Tools["Pay_Out"].SharedProps.Enabled = UserPriviliges.IsUserHasPriviliges(UserPriviliges.Modules.POSTransaction.ID, UserPriviliges.Screens.Payout.ID);
                    ultMenuBar.Tools["Customers"].SharedProps.ShowInCustomizer = ultMenuBar.Tools["Customers"].SharedProps.Visible = ultMenuBar.Tools["Customers"].SharedProps.Enabled = UserPriviliges.IsUserHasPriviliges(UserPriviliges.Modules.POSTransaction.ID, UserPriviliges.Screens.Customers.ID);

                    ultMenuBar.Tools["TSales"].SharedProps.ShowInCustomizer = ultMenuBar.Tools["TSales"].SharedProps.Visible = ultMenuBar.Tools["TSales"].SharedProps.Enabled = UserPriviliges.IsUserHasPriviliges(UserPriviliges.Modules.POSTransaction.ID, UserPriviliges.Screens.POSTransaction.ID, UserPriviliges.Permissions.POSTransaction.ID);
                    ultMenuBar.Tools["TPay_Out"].SharedProps.ShowInCustomizer = ultMenuBar.Tools["TPay_Out"].SharedProps.Visible = ultMenuBar.Tools["TPay_Out"].SharedProps.Enabled = UserPriviliges.IsUserHasPriviliges(UserPriviliges.Modules.POSTransaction.ID, UserPriviliges.Screens.Payout.ID);
                    ultMenuBar.Tools["TCustomers"].SharedProps.ShowInCustomizer = ultMenuBar.Tools["TCustomers"].SharedProps.Visible = ultMenuBar.Tools["TCustomers"].SharedProps.Enabled = UserPriviliges.IsUserHasPriviliges(UserPriviliges.Modules.POSTransaction.ID, UserPriviliges.Screens.Customers.ID);
                }

                #endregion

                #region Disable Inventory Management
                if (oPopupInvMgmt.SharedProps.Visible == true)
                {
                    ultMenuBar.Tools["Vendors"].SharedProps.ShowInCustomizer = ultMenuBar.Tools["Vendors"].SharedProps.Visible = ultMenuBar.Tools["Vendors"].SharedProps.Enabled = UserPriviliges.IsUserHasPriviliges(UserPriviliges.Modules.InventoryMgmt.ID, UserPriviliges.Screens.VendorFile.ID);
                    ultMenuBar.Tools["TVendors"].SharedProps.ShowInCustomizer = ultMenuBar.Tools["TVendors"].SharedProps.Visible = ultMenuBar.Tools["TVendors"].SharedProps.Enabled = UserPriviliges.IsUserHasPriviliges(UserPriviliges.Modules.InventoryMgmt.ID, UserPriviliges.Screens.VendorFile.ID);

                    ultMenuBar.Tools["Items"].SharedProps.ShowInCustomizer = ultMenuBar.Tools["Items"].SharedProps.Visible = ultMenuBar.Tools["Items"].SharedProps.Enabled = UserPriviliges.IsUserHasPriviliges(UserPriviliges.Modules.InventoryMgmt.ID, UserPriviliges.Screens.ItemFile.ID);
                    ultMenuBar.Tools["TItemFile"].SharedProps.ShowInCustomizer = ultMenuBar.Tools["TItemFile"].SharedProps.Visible = ultMenuBar.Tools["TItemFile"].SharedProps.Enabled = UserPriviliges.IsUserHasPriviliges(UserPriviliges.Modules.InventoryMgmt.ID, UserPriviliges.Screens.ItemFile.ID);

                    ultMenuBar.Tools["AutoPriceUpdate"].SharedProps.ShowInCustomizer = ultMenuBar.Tools["AutoPriceUpdate"].SharedProps.Visible = ultMenuBar.Tools["AutoPriceUpdate"].SharedProps.Enabled = UserPriviliges.IsUserHasPriviliges(UserPriviliges.Modules.InventoryMgmt.ID, UserPriviliges.Screens.ItemFile.ID, UserPriviliges.Permissions.PriceUpdate.ID);
                    ultMenuBar.Tools["ManualPriceUpdate"].SharedProps.ShowInCustomizer = ultMenuBar.Tools["ManualPriceUpdate"].SharedProps.Visible = ultMenuBar.Tools["ManualPriceUpdate"].SharedProps.Enabled = UserPriviliges.IsUserHasPriviliges(UserPriviliges.Modules.InventoryMgmt.ID, UserPriviliges.Screens.ItemFile.ID, UserPriviliges.Permissions.PriceUpdate.ID);
                    //Commented By shitaljit to include Combo Price Menu.
                    //ultMenuBar.Tools["ItemComboPricing"].SharedProps.ShowInCustomizer = ultMenuBar.Tools["ItemComboPricing"].SharedProps.Visible = ultMenuBar.Tools["ItemComboPricing"].SharedProps.Enabled = false;//Added By shitaljit to hide the tab as the functionality is incomeplete.

                    ultMenuBar.Tools["Purchase_Order"].SharedProps.ShowInCustomizer = ultMenuBar.Tools["Purchase_Order"].SharedProps.Visible = ultMenuBar.Tools["Purchase_Order"].SharedProps.Enabled = UserPriviliges.IsUserHasPriviliges(UserPriviliges.Modules.InventoryMgmt.ID, UserPriviliges.Screens.PurchaseOrder.ID);
                    ultMenuBar.Tools["TPurchase_Order"].SharedProps.ShowInCustomizer = ultMenuBar.Tools["TPurchase_Order"].SharedProps.Visible = ultMenuBar.Tools["TPurchase_Order"].SharedProps.Enabled = UserPriviliges.IsUserHasPriviliges(UserPriviliges.Modules.InventoryMgmt.ID, UserPriviliges.Screens.PurchaseOrder.ID);

                    ultMenuBar.Tools["POAcknowledgement"].SharedProps.ShowInCustomizer = ultMenuBar.Tools["POAcknowledgement"].SharedProps.Visible = ultMenuBar.Tools["POAcknowledgement"].SharedProps.Enabled = UserPriviliges.IsUserHasPriviliges(UserPriviliges.Modules.InventoryMgmt.ID, UserPriviliges.Screens.PurchaseOrder.ID, UserPriviliges.Permissions.POAckProcess.ID);
                    ultMenuBar.Tools["ViewPurchaseOrder"].SharedProps.ShowInCustomizer = ultMenuBar.Tools["ViewPurchaseOrder"].SharedProps.Visible = ultMenuBar.Tools["ViewPurchaseOrder"].SharedProps.Enabled = UserPriviliges.IsUserHasPriviliges(UserPriviliges.Modules.InventoryMgmt.ID, UserPriviliges.Screens.PurchaseOrder.ID, UserPriviliges.Permissions.POHistory.ID);

                    ultMenuBar.Tools["Physical_Inventory"].SharedProps.ShowInCustomizer = ultMenuBar.Tools["Physical_Inventory"].SharedProps.Visible = ultMenuBar.Tools["Physical_Inventory"].SharedProps.Enabled = UserPriviliges.IsUserHasPriviliges(UserPriviliges.Modules.InventoryMgmt.ID, UserPriviliges.Screens.PhysicalInventory.ID);
                    ultMenuBar.Tools["tPhysical_Inventory"].SharedProps.ShowInCustomizer = ultMenuBar.Tools["tPhysical_Inventory"].SharedProps.Visible = ultMenuBar.Tools["tPhysical_Inventory"].SharedProps.Enabled = UserPriviliges.IsUserHasPriviliges(UserPriviliges.Modules.InventoryMgmt.ID, UserPriviliges.Screens.PhysicalInventory.ID);

                    ultMenuBar.Tools["Inventory_Recieved"].SharedProps.ShowInCustomizer = ultMenuBar.Tools["Inventory_Recieved"].SharedProps.Visible = ultMenuBar.Tools["Inventory_Recieved"].SharedProps.Enabled = UserPriviliges.IsUserHasPriviliges(UserPriviliges.Modules.InventoryMgmt.ID, UserPriviliges.Screens.InventoryRecvd.ID, UserPriviliges.Permissions.InventoryReceived.ID);  //PRIMEPOS-3141 27-Oct-2022 JY Modified
                    ultMenuBar.Tools["TInventory_Recieved"].SharedProps.ShowInCustomizer = ultMenuBar.Tools["TInventory_Recieved"].SharedProps.Visible = ultMenuBar.Tools["TInventory_Recieved"].SharedProps.Enabled = UserPriviliges.IsUserHasPriviliges(UserPriviliges.Modules.InventoryMgmt.ID, UserPriviliges.Screens.InventoryRecvd.ID, UserPriviliges.Permissions.InventoryReceived.ID);  //PRIMEPOS-3141 27-Oct-2022 JY Modified
                    ultMenuBar.Tools["ViewInventoryRecieved"].SharedProps.ShowInCustomizer = ultMenuBar.Tools["ViewInventoryRecieved"].SharedProps.Visible = ultMenuBar.Tools["ViewInventoryRecieved"].SharedProps.Enabled = UserPriviliges.IsUserHasPriviliges(UserPriviliges.Modules.InventoryMgmt.ID, UserPriviliges.Screens.InventoryRecvd.ID, UserPriviliges.Permissions.ViewInventory.ID);
                    ultMenuBar.Tools[POSMenuItems.InvTransType].SharedProps.ShowInCustomizer = ultMenuBar.Tools[POSMenuItems.InvTransType].SharedProps.Visible = ultMenuBar.Tools[POSMenuItems.InvTransType].SharedProps.Enabled = UserPriviliges.IsUserHasPriviliges(UserPriviliges.Modules.InventoryMgmt.ID, UserPriviliges.Screens.InvTransType.ID);

                    ultMenuBar.Tools["InventoryReports"].SharedProps.ShowInCustomizer = ultMenuBar.Tools["InventoryReports"].SharedProps.Visible = ultMenuBar.Tools["InventoryReports"].SharedProps.Enabled = UserPriviliges.IsUserHasPriviliges(UserPriviliges.Modules.InventoryMgmt.ID, UserPriviliges.Screens.InvReports.ID);
                    ultMenuBar.Tools["ItemFileListing"].SharedProps.ShowInCustomizer = ultMenuBar.Tools["ItemFileListing"].SharedProps.Visible = ultMenuBar.Tools["ItemFileListing"].SharedProps.Enabled = UserPriviliges.IsUserHasPriviliges(UserPriviliges.Modules.InventoryMgmt.ID, UserPriviliges.Screens.InvReports.ID, UserPriviliges.Permissions.InvItemFileListing.ID);
                    ultMenuBar.Tools["VendorFileListing"].SharedProps.ShowInCustomizer = ultMenuBar.Tools["VendorFileListing"].SharedProps.Visible = ultMenuBar.Tools["VendorFileListing"].SharedProps.Enabled = UserPriviliges.IsUserHasPriviliges(UserPriviliges.Modules.InventoryMgmt.ID, UserPriviliges.Screens.InvReports.ID, UserPriviliges.Permissions.InvVendFileListing.ID);
                    ultMenuBar.Tools["InventoryStatusReport"].SharedProps.ShowInCustomizer = ultMenuBar.Tools["InventoryStatusReport"].SharedProps.Visible = ultMenuBar.Tools["InventoryStatusReport"].SharedProps.Enabled = UserPriviliges.IsUserHasPriviliges(UserPriviliges.Modules.InventoryMgmt.ID, UserPriviliges.Screens.InvReports.ID, UserPriviliges.Permissions.InvInvStatusRep.ID);
                    ultMenuBar.Tools["RInventoryReceived"].SharedProps.ShowInCustomizer = ultMenuBar.Tools["RInventoryReceived"].SharedProps.Visible = ultMenuBar.Tools["RInventoryReceived"].SharedProps.Enabled = UserPriviliges.IsUserHasPriviliges(UserPriviliges.Modules.InventoryMgmt.ID, UserPriviliges.Screens.InvReports.ID, UserPriviliges.Permissions.InvInventoryReceived.ID);
                    ultMenuBar.Tools["ItemRe-Order"].SharedProps.ShowInCustomizer = ultMenuBar.Tools["ItemRe-Order"].SharedProps.Visible = ultMenuBar.Tools["ItemRe-Order"].SharedProps.Enabled = UserPriviliges.IsUserHasPriviliges(UserPriviliges.Modules.InventoryMgmt.ID, UserPriviliges.Screens.InvReports.ID, UserPriviliges.Permissions.InvItemsReorder.ID);
                    ultMenuBar.Tools["PurchaseOrderReport"].SharedProps.ShowInCustomizer = ultMenuBar.Tools["PurchaseOrderReport"].SharedProps.Visible = ultMenuBar.Tools["PurchaseOrderReport"].SharedProps.Enabled = UserPriviliges.IsUserHasPriviliges(UserPriviliges.Modules.InventoryMgmt.ID, UserPriviliges.Screens.InvReports.ID, UserPriviliges.Permissions.InvPurchaseOrder.ID);
                    ultMenuBar.Tools["PhysicalInventorySheet"].SharedProps.ShowInCustomizer = ultMenuBar.Tools["PhysicalInventorySheet"].SharedProps.Visible = ultMenuBar.Tools["PhysicalInventorySheet"].SharedProps.Enabled = UserPriviliges.IsUserHasPriviliges(UserPriviliges.Modules.InventoryMgmt.ID, UserPriviliges.Screens.InvReports.ID, UserPriviliges.Permissions.InvPhysicalInvSheet.ID);
                    ultMenuBar.Tools["Physical Inv. History"].SharedProps.ShowInCustomizer = ultMenuBar.Tools["Physical Inv. History"].SharedProps.Visible = ultMenuBar.Tools["Physical Inv. History"].SharedProps.Enabled = UserPriviliges.IsUserHasPriviliges(UserPriviliges.Modules.InventoryMgmt.ID, UserPriviliges.Screens.InvReports.ID, UserPriviliges.Permissions.InvPhysicalInvHistory.ID);
                    ultMenuBar.Tools["ItemLabelReport"].SharedProps.ShowInCustomizer = ultMenuBar.Tools["ItemLabelReport"].SharedProps.Visible = ultMenuBar.Tools["ItemLabelReport"].SharedProps.Enabled = UserPriviliges.IsUserHasPriviliges(UserPriviliges.Modules.InventoryMgmt.ID, UserPriviliges.Screens.InvReports.ID, UserPriviliges.Permissions.InvItemLabelReport.ID);

                    ultMenuBar.Tools["IIASItemFileListing"].SharedProps.ShowInCustomizer = ultMenuBar.Tools["IIASItemFileListing"].SharedProps.Visible = ultMenuBar.Tools["IIASItemFileListing"].SharedProps.Enabled = UserPriviliges.IsUserHasPriviliges(UserPriviliges.Modules.InventoryMgmt.ID, UserPriviliges.Screens.InvReports.ID, UserPriviliges.Permissions.IIASItemFileListing.ID);

                    ultMenuBar.Tools["WarningMessages"].SharedProps.ShowInCustomizer = ultMenuBar.Tools["WarningMessages"].SharedProps.Visible = ultMenuBar.Tools["WarningMessages"].SharedProps.Enabled = UserPriviliges.IsUserHasPriviliges(UserPriviliges.Modules.InventoryMgmt.ID, UserPriviliges.Screens.WarningMessages.ID);
                    ultMenuBar.Tools["InventoryShrinkageReport"].SharedProps.ShowInCustomizer = ultMenuBar.Tools["InventoryShrinkageReport"].SharedProps.Visible = false;   //PRIMEPOS-138 14-Feb-2021 JY disabled
                }
                #endregion

                #region Disable Administrative Functions
                if (oPopupAdmFun.SharedProps.Visible == true)
                {
                    ultMenuBar.Tools[POSMenuItems.Departments].SharedProps.ShowInCustomizer = ultMenuBar.Tools[POSMenuItems.Departments].SharedProps.Visible = ultMenuBar.Tools[POSMenuItems.Departments].SharedProps.Enabled = UserPriviliges.IsUserHasPriviliges(UserPriviliges.Modules.AdminFunc.ID, UserPriviliges.Screens.Department.ID);
                    ultMenuBar.Tools["TDepartments"].SharedProps.ShowInCustomizer = ultMenuBar.Tools["TDepartments"].SharedProps.Visible = ultMenuBar.Tools["TDepartments"].SharedProps.Enabled = UserPriviliges.IsUserHasPriviliges(UserPriviliges.Modules.AdminFunc.ID, UserPriviliges.Screens.Department.ID);

                    ultMenuBar.Tools[POSMenuItems.CloseStation].SharedProps.ShowInCustomizer = ultMenuBar.Tools[POSMenuItems.CloseStation].SharedProps.Visible = ultMenuBar.Tools[POSMenuItems.CloseStation].SharedProps.Enabled = UserPriviliges.IsUserHasPriviliges(UserPriviliges.Modules.AdminFunc.ID, UserPriviliges.Screens.StationClose.ID);

                    ultMenuBar.Tools[POSMenuItems.FunctionKeys].SharedProps.ShowInCustomizer = ultMenuBar.Tools[POSMenuItems.FunctionKeys].SharedProps.Visible = ultMenuBar.Tools[POSMenuItems.FunctionKeys].SharedProps.Enabled = UserPriviliges.IsUserHasPriviliges(UserPriviliges.Modules.AdminFunc.ID, UserPriviliges.Screens.FunctionKeys.ID);
                    ultMenuBar.Tools["TFunction_Keys"].SharedProps.ShowInCustomizer = ultMenuBar.Tools["TFunction_Keys"].SharedProps.Visible = ultMenuBar.Tools["TFunction_Keys"].SharedProps.Enabled = UserPriviliges.IsUserHasPriviliges(UserPriviliges.Modules.AdminFunc.ID, UserPriviliges.Screens.FunctionKeys.ID);

                    ultMenuBar.Tools[POSMenuItems.TaxCodes].SharedProps.ShowInCustomizer = ultMenuBar.Tools[POSMenuItems.TaxCodes].SharedProps.Visible = ultMenuBar.Tools[POSMenuItems.TaxCodes].SharedProps.Enabled = UserPriviliges.IsUserHasPriviliges(UserPriviliges.Modules.AdminFunc.ID, UserPriviliges.Screens.TaxCodes.ID);
                    ultMenuBar.Tools["TTax_Codes"].SharedProps.ShowInCustomizer = ultMenuBar.Tools["TTax_Codes"].SharedProps.Visible = ultMenuBar.Tools["TTax_Codes"].SharedProps.Enabled = UserPriviliges.IsUserHasPriviliges(UserPriviliges.Modules.AdminFunc.ID, UserPriviliges.Screens.TaxCodes.ID);

                    ultMenuBar.Tools[POSMenuItems.ViewStationClose].SharedProps.ShowInCustomizer = ultMenuBar.Tools[POSMenuItems.ViewStationClose].SharedProps.Visible = ultMenuBar.Tools[POSMenuItems.ViewStationClose].SharedProps.Enabled = UserPriviliges.IsUserHasPriviliges(UserPriviliges.Modules.AdminFunc.ID, UserPriviliges.Screens.StationClose.ID);

                    ultMenuBar.Tools[POSMenuItems.EndOfDay].SharedProps.ShowInCustomizer = ultMenuBar.Tools[POSMenuItems.EndOfDay].SharedProps.Visible = ultMenuBar.Tools[POSMenuItems.EndOfDay].SharedProps.Enabled = UserPriviliges.IsUserHasPriviliges(UserPriviliges.Modules.AdminFunc.ID, UserPriviliges.Screens.EndOfDay.ID);
                    ultMenuBar.Tools[POSMenuItems.ViewEndOfDay].SharedProps.ShowInCustomizer = ultMenuBar.Tools[POSMenuItems.ViewEndOfDay].SharedProps.Visible = ultMenuBar.Tools[POSMenuItems.ViewEndOfDay].SharedProps.Enabled = UserPriviliges.IsUserHasPriviliges(UserPriviliges.Modules.AdminFunc.ID, UserPriviliges.Screens.EndOfDay.ID);

                    ultMenuBar.Tools[POSMenuItems.ViewPOSTrans].SharedProps.ShowInCustomizer = ultMenuBar.Tools[POSMenuItems.ViewPOSTrans].SharedProps.Visible = ultMenuBar.Tools[POSMenuItems.ViewPOSTrans].SharedProps.Enabled = UserPriviliges.IsUserHasPriviliges(UserPriviliges.Modules.AdminFunc.ID, UserPriviliges.Screens.ViewPOSTrans.ID);

                    ultMenuBar.Tools["ManagementReports"].SharedProps.ShowInCustomizer = ultMenuBar.Tools["ManagementReports"].SharedProps.Visible = ultMenuBar.Tools["ManagementReports"].SharedProps.Enabled = UserPriviliges.IsUserHasPriviliges(UserPriviliges.Modules.AdminFunc.ID, UserPriviliges.Screens.AdminReports.ID);
                    ultMenuBar.Tools["Detail Transaction Report"].SharedProps.ShowInCustomizer = ultMenuBar.Tools["Detail Transaction Report"].SharedProps.Visible = ultMenuBar.Tools["Detail Transaction Report"].SharedProps.Enabled = UserPriviliges.IsUserHasPriviliges(UserPriviliges.Modules.AdminFunc.ID, UserPriviliges.Screens.AdminReports.ID, UserPriviliges.Permissions.TransactionDetail.ID);
                    ultMenuBar.Tools["Summary Sales Report By User ID"].SharedProps.ShowInCustomizer = ultMenuBar.Tools["Summary Sales Report By User ID"].SharedProps.Visible = ultMenuBar.Tools["Summary Sales Report By User ID"].SharedProps.Enabled = UserPriviliges.IsUserHasPriviliges(UserPriviliges.Modules.AdminFunc.ID, UserPriviliges.Screens.AdminReports.ID, UserPriviliges.Permissions.SumSaleByU.ID);
                    ultMenuBar.Tools["Sales Report By Item"].SharedProps.ShowInCustomizer = ultMenuBar.Tools["Sales Report By Item"].SharedProps.Visible = ultMenuBar.Tools["Sales Report By Item"].SharedProps.Enabled = UserPriviliges.IsUserHasPriviliges(UserPriviliges.Modules.AdminFunc.ID, UserPriviliges.Screens.AdminReports.ID, UserPriviliges.Permissions.SumSaleByI.ID);
                    ultMenuBar.Tools["Sales Report By Department"].SharedProps.ShowInCustomizer = ultMenuBar.Tools["Sales Report By Department"].SharedProps.Visible = ultMenuBar.Tools["Sales Report By Department"].SharedProps.Enabled = UserPriviliges.IsUserHasPriviliges(UserPriviliges.Modules.AdminFunc.ID, UserPriviliges.Screens.AdminReports.ID, UserPriviliges.Permissions.SaleByDept.ID);
                    ultMenuBar.Tools["Sales Report By Payment"].SharedProps.ShowInCustomizer = ultMenuBar.Tools["Sales Report By Payment"].SharedProps.Visible = ultMenuBar.Tools["Sales Report By Payment"].SharedProps.Enabled = UserPriviliges.IsUserHasPriviliges(UserPriviliges.Modules.AdminFunc.ID, UserPriviliges.Screens.AdminReports.ID, UserPriviliges.Permissions.SaleByPayment.ID);
                    ultMenuBar.Tools["StationCloseSummary"].SharedProps.ShowInCustomizer = ultMenuBar.Tools["StationCloseSummary"].SharedProps.Visible = ultMenuBar.Tools["StationCloseSummary"].SharedProps.Enabled = UserPriviliges.IsUserHasPriviliges(UserPriviliges.Modules.AdminFunc.ID, UserPriviliges.Screens.AdminReports.ID, UserPriviliges.Permissions.StationCloseSum.ID);
                    ultMenuBar.Tools["SaleTaxControl"].SharedProps.ShowInCustomizer = ultMenuBar.Tools["SaleTaxControl"].SharedProps.Visible = ultMenuBar.Tools["SaleTaxControl"].SharedProps.Enabled = UserPriviliges.IsUserHasPriviliges(UserPriviliges.Modules.AdminFunc.ID, UserPriviliges.Screens.AdminReports.ID, UserPriviliges.Permissions.SalesTaxControl.ID);
                    ultMenuBar.Tools["PriceOverriddenReport"].SharedProps.ShowInCustomizer = ultMenuBar.Tools["PriceOverriddenReport"].SharedProps.Visible = ultMenuBar.Tools["PriceOverriddenReport"].SharedProps.Enabled = UserPriviliges.IsUserHasPriviliges(UserPriviliges.Modules.AdminFunc.ID, UserPriviliges.Screens.AdminReports.ID, UserPriviliges.Permissions.PriceOverridden.ID);
                    ultMenuBar.Tools["Sales Tax Summary"].SharedProps.ShowInCustomizer = ultMenuBar.Tools["Sales Tax Summary"].SharedProps.Visible = ultMenuBar.Tools["Sales Tax Summary"].SharedProps.Enabled = UserPriviliges.IsUserHasPriviliges(UserPriviliges.Modules.AdminFunc.ID, UserPriviliges.Screens.AdminReports.ID, UserPriviliges.Permissions.SalesTaxSum.ID);
                    ultMenuBar.Tools["TopSellingProducts"].SharedProps.ShowInCustomizer = ultMenuBar.Tools["TopSellingProducts"].SharedProps.Visible = ultMenuBar.Tools["TopSellingProducts"].SharedProps.Enabled = UserPriviliges.IsUserHasPriviliges(UserPriviliges.Modules.AdminFunc.ID, UserPriviliges.Screens.AdminReports.ID, UserPriviliges.Permissions.TopSellingProd.ID);
                    ultMenuBar.Tools["ProductivityReport"].SharedProps.ShowInCustomizer = ultMenuBar.Tools["ProductivityReport"].SharedProps.Visible = ultMenuBar.Tools["ProductivityReport"].SharedProps.Enabled = UserPriviliges.IsUserHasPriviliges(UserPriviliges.Modules.AdminFunc.ID, UserPriviliges.Screens.AdminReports.ID, UserPriviliges.Permissions.ProductivityRep.ID);
                    ultMenuBar.Tools["PayoutDetails"].SharedProps.ShowInCustomizer = ultMenuBar.Tools["PayoutDetails"].SharedProps.Visible = ultMenuBar.Tools["PayoutDetails"].SharedProps.Enabled = UserPriviliges.IsUserHasPriviliges(UserPriviliges.Modules.AdminFunc.ID, UserPriviliges.Screens.AdminReports.ID, UserPriviliges.Permissions.PayoutDetails.ID);
                    ultMenuBar.Tools[POSMenuItems.CostAnalysisReport].SharedProps.ShowInCustomizer = ultMenuBar.Tools[POSMenuItems.CostAnalysisReport].SharedProps.Visible = ultMenuBar.Tools[POSMenuItems.CostAnalysisReport].SharedProps.Enabled = UserPriviliges.IsUserHasPriviliges(UserPriviliges.Modules.AdminFunc.ID, UserPriviliges.Screens.AdminReports.ID, UserPriviliges.Permissions.CostAnalysis.ID);

                    ultMenuBar.Tools[POSMenuItems.MyStore].SharedProps.Visible = UserPriviliges.IsUserHasPriviliges(UserPriviliges.Modules.AdminFunc.ID, UserPriviliges.Screens.MyStore.ID);
                    ultMenuBar.Tools[POSMenuItems.MyStore].SharedProps.Enabled = UserPriviliges.IsUserHasPriviliges(UserPriviliges.Modules.AdminFunc.ID, UserPriviliges.Screens.MyStore.ID);

                    ultMenuBar.Tools[POSMenuItems.SalesByCustomer].SharedProps.ShowInCustomizer = ultMenuBar.Tools[POSMenuItems.SalesByCustomer].SharedProps.Visible = ultMenuBar.Tools[POSMenuItems.SalesByCustomer].SharedProps.Enabled = UserPriviliges.IsUserHasPriviliges(UserPriviliges.Modules.AdminFunc.ID, UserPriviliges.Screens.AdminReports.ID, UserPriviliges.Permissions.SalesByCustomer.ID);

                    ultMenuBar.Tools[POSMenuItems.CustomerList].SharedProps.ShowInCustomizer = ultMenuBar.Tools[POSMenuItems.CustomerList].SharedProps.Visible = ultMenuBar.Tools[POSMenuItems.CustomerList].SharedProps.Enabled = UserPriviliges.IsUserHasPriviliges(UserPriviliges.Modules.AdminFunc.ID, UserPriviliges.Screens.AdminReports.ID, UserPriviliges.Permissions.CustomerList.ID);

                    ultMenuBar.Tools[POSMenuItems.IIASPaymentTransaction].SharedProps.ShowInCustomizer = ultMenuBar.Tools[POSMenuItems.IIASPaymentTransaction].SharedProps.Visible = ultMenuBar.Tools[POSMenuItems.IIASPaymentTransaction].SharedProps.Enabled = UserPriviliges.IsUserHasPriviliges(UserPriviliges.Modules.AdminFunc.ID, UserPriviliges.Screens.AdminReports.ID, UserPriviliges.Permissions.IIASPaymentTransaction.ID);
                    ultMenuBar.Tools[POSMenuItems.IIASTransSummary].SharedProps.ShowInCustomizer = ultMenuBar.Tools[POSMenuItems.IIASTransSummary].SharedProps.Visible = ultMenuBar.Tools[POSMenuItems.IIASTransSummary].SharedProps.Enabled = UserPriviliges.IsUserHasPriviliges(UserPriviliges.Modules.AdminFunc.ID, UserPriviliges.Screens.AdminReports.ID, UserPriviliges.Permissions.IIASTransSummary.ID);

                    ultMenuBar.Tools[POSMenuItems.SalesByItemMonitoringCategory].SharedProps.ShowInCustomizer = ultMenuBar.Tools[POSMenuItems.SalesByItemMonitoringCategory].SharedProps.Visible = ultMenuBar.Tools[POSMenuItems.SalesByItemMonitoringCategory].SharedProps.Enabled = UserPriviliges.IsUserHasPriviliges(UserPriviliges.Modules.AdminFunc.ID, UserPriviliges.Screens.AdminReports.ID, UserPriviliges.Permissions.SalesByItemMonitoringCategory.ID);
                    ultMenuBar.Tools[POSMenuItems.DeliveryListReport].SharedProps.ShowInCustomizer = ultMenuBar.Tools[POSMenuItems.DeliveryListReport].SharedProps.Visible = ultMenuBar.Tools[POSMenuItems.DeliveryListReport].SharedProps.Enabled = UserPriviliges.IsUserHasPriviliges(UserPriviliges.Modules.AdminFunc.ID, UserPriviliges.Screens.AdminReports.ID, UserPriviliges.Permissions.DeliveryListReport.ID);
                    ultMenuBar.Tools[POSMenuItems.SalesByInsurance].SharedProps.ShowInCustomizer = ultMenuBar.Tools[POSMenuItems.SalesByInsurance].SharedProps.Visible = ultMenuBar.Tools[POSMenuItems.SalesByInsurance].SharedProps.Enabled = UserPriviliges.IsUserHasPriviliges(UserPriviliges.Modules.AdminFunc.ID, UserPriviliges.Screens.AdminReports.ID, UserPriviliges.Permissions.InsuranceDetialReport.ID);

                    ultMenuBar.Tools[POSMenuItems.SalesComparison].SharedProps.ShowInCustomizer = ultMenuBar.Tools[POSMenuItems.SalesComparison].SharedProps.Visible = ultMenuBar.Tools[POSMenuItems.SalesComparison].SharedProps.Enabled = UserPriviliges.IsUserHasPriviliges(UserPriviliges.Modules.AdminFunc.ID, UserPriviliges.Screens.AdminReports.ID, UserPriviliges.Permissions.SalesComparisonReport.ID);
                    ultMenuBar.Tools[POSMenuItems.SalesComparisonByDept].SharedProps.ShowInCustomizer = ultMenuBar.Tools[POSMenuItems.SalesComparisonByDept].SharedProps.Visible = ultMenuBar.Tools[POSMenuItems.SalesComparisonByDept].SharedProps.Enabled = UserPriviliges.IsUserHasPriviliges(UserPriviliges.Modules.AdminFunc.ID, UserPriviliges.Screens.AdminReports.ID, UserPriviliges.Permissions.SalesComparisonByDeptReport.ID);
                    //Added by Krishna on 23 June 2011
                    ultMenuBar.Tools[POSMenuItems.StnCloseCash].SharedProps.ShowInCustomizer = ultMenuBar.Tools[POSMenuItems.StnCloseCash].SharedProps.Visible = ultMenuBar.Tools[POSMenuItems.StnCloseCash].SharedProps.Enabled = UserPriviliges.IsUserHasPriviliges(UserPriviliges.Modules.AdminFunc.ID, UserPriviliges.Screens.AdminReports.ID, UserPriviliges.Permissions.StnCloseCash.ID);
                    //Till here added by krishna on 23 June 2011
                    ultMenuBar.Tools[POSMenuItems.PointsRewardTier].SharedProps.ShowInCustomizer = ultMenuBar.Tools[POSMenuItems.PointsRewardTier].SharedProps.Visible = ultMenuBar.Tools[POSMenuItems.PointsRewardTier].SharedProps.Enabled = UserPriviliges.IsUserHasPriviliges(UserPriviliges.Modules.AdminFunc.ID, UserPriviliges.Screens.PointsRewardTier.ID);
                    ultMenuBar.Tools[POSMenuItems.RegisterCLCards].SharedProps.ShowInCustomizer = ultMenuBar.Tools[POSMenuItems.RegisterCLCards].SharedProps.Visible = ultMenuBar.Tools[POSMenuItems.RegisterCLCards].SharedProps.Enabled = UserPriviliges.IsUserHasPriviliges(UserPriviliges.Modules.AdminFunc.ID, UserPriviliges.Screens.CustomerLoyaltyCards.ID);
                    if (ultMenuBar.Tools[POSMenuItems.RegisterCLCards].SharedProps.Visible || ultMenuBar.Tools[POSMenuItems.PointsRewardTier].SharedProps.Visible)
                    {
                        ultMenuBar.Tools["Customer Loyalty"].SharedProps.ShowInCustomizer = ultMenuBar.Tools["Customer Loyalty"].SharedProps.Visible = ultMenuBar.Tools["Customer Loyalty"].SharedProps.Enabled = true;
                    }
                    else
                    {
                        ultMenuBar.Tools["Customer Loyalty"].SharedProps.ShowInCustomizer = ultMenuBar.Tools["Customer Loyalty"].SharedProps.Visible = ultMenuBar.Tools["Customer Loyalty"].SharedProps.Enabled = false;
                    }
                    //ultMenuBar.Tools["TaxOverrideReport"].SharedProps.ShowInCustomizer = ultMenuBar.Tools["TaxOverrideReport"].SharedProps.Visible = ultMenuBar.Tools["TaxOverrideReport"].SharedProps.Enabled = UserPriviliges.IsUserHasPriviliges(UserPriviliges.Modules.AdminFunc.ID, UserPriviliges.Screens.AdminReports.ID, UserPriviliges.Permissions.PriceOverridden.ID);    //PRIMEPOS-2391 23-Jul-2021 JY Added
                }
                #endregion                            
                #endregion

                #region Explorer bar

                foreach (Infragistics.Win.UltraWinExplorerBar.UltraExplorerBarItem oItem in ultraExplorerBar1.Groups["Inventory_Management"].Items)
                {
                    oItem.Visible = false;
                    oItem.Settings.Enabled = Infragistics.Win.DefaultableBoolean.False;
                }

                foreach (Infragistics.Win.UltraWinExplorerBar.UltraExplorerBarItem oItem in ultraExplorerBar1.Groups["POS_Terminal"].Items)
                {
                    oItem.Visible = false;
                    oItem.Settings.Enabled = Infragistics.Win.DefaultableBoolean.False;
                }

                foreach (Infragistics.Win.UltraWinExplorerBar.UltraExplorerBarItem oItem in ultraExplorerBar1.Groups["Administrative_Function"].Items)
                {
                    oItem.Visible = false;
                    oItem.Settings.Enabled = Infragistics.Win.DefaultableBoolean.False;
                }

                ultraExplorerBar1.Groups["Inventory_Management"].Visible = UserPriviliges.IsUserHasPriviliges(UserPriviliges.Modules.InventoryMgmt.ID);
                if (ultraExplorerBar1.Groups["Inventory_Management"].Visible == true)
                {
                    ultraExplorerBar1.Groups["Inventory_Management"].Items["Vendors"].Visible = ultraExplorerBar1.Groups["Inventory_Management"].Items["Vendors"].Visible = UserPriviliges.IsUserHasPriviliges(UserPriviliges.Modules.InventoryMgmt.ID, UserPriviliges.Screens.VendorFile.ID);
                    ultraExplorerBar1.Groups["Inventory_Management"].Items["Vendors"].Settings.Enabled = (UserPriviliges.IsUserHasPriviliges(UserPriviliges.Modules.InventoryMgmt.ID, UserPriviliges.Screens.VendorFile.ID) == true ? Infragistics.Win.DefaultableBoolean.True : Infragistics.Win.DefaultableBoolean.False);

                    ultraExplorerBar1.Groups["Inventory_Management"].Items["ItemFile"].Visible = UserPriviliges.IsUserHasPriviliges(UserPriviliges.Modules.InventoryMgmt.ID, UserPriviliges.Screens.ItemFile.ID);
                    ultraExplorerBar1.Groups["Inventory_Management"].Items["ItemFile"].Settings.Enabled = (UserPriviliges.IsUserHasPriviliges(UserPriviliges.Modules.InventoryMgmt.ID, UserPriviliges.Screens.ItemFile.ID) == true ? Infragistics.Win.DefaultableBoolean.True : Infragistics.Win.DefaultableBoolean.False);

                    ultraExplorerBar1.Groups["Inventory_Management"].Items["Purchase_Order"].Visible = UserPriviliges.IsUserHasPriviliges(UserPriviliges.Modules.InventoryMgmt.ID, UserPriviliges.Screens.PurchaseOrder.ID);
                    ultraExplorerBar1.Groups["Inventory_Management"].Items["Purchase_Order"].Settings.Enabled = ((UserPriviliges.IsUserHasPriviliges(UserPriviliges.Modules.InventoryMgmt.ID, UserPriviliges.Screens.PurchaseOrder.ID) == true ? Infragistics.Win.DefaultableBoolean.True : Infragistics.Win.DefaultableBoolean.False));

                    ultraExplorerBar1.Groups["Inventory_Management"].Items["Inventory_Recieved"].Visible = UserPriviliges.IsUserHasPriviliges(UserPriviliges.Modules.InventoryMgmt.ID, UserPriviliges.Screens.InventoryRecvd.ID, UserPriviliges.Permissions.InventoryReceived.ID);  //PRIMEPOS-3141 27-Oct-2022 JY Modified
                    ultraExplorerBar1.Groups["Inventory_Management"].Items["Inventory_Recieved"].Settings.Enabled = ((UserPriviliges.IsUserHasPriviliges(UserPriviliges.Modules.InventoryMgmt.ID, UserPriviliges.Screens.InventoryRecvd.ID, UserPriviliges.Permissions.InventoryReceived.ID) == true ? Infragistics.Win.DefaultableBoolean.True : Infragistics.Win.DefaultableBoolean.False));

                    ultraExplorerBar1.Groups["Inventory_Management"].Items["Physical_Inventory"].Visible = UserPriviliges.IsUserHasPriviliges(UserPriviliges.Modules.InventoryMgmt.ID, UserPriviliges.Screens.PhysicalInventory.ID);
                    ultraExplorerBar1.Groups["Inventory_Management"].Items["Physical_Inventory"].Settings.Enabled = ((UserPriviliges.IsUserHasPriviliges(UserPriviliges.Modules.InventoryMgmt.ID, UserPriviliges.Screens.PhysicalInventory.ID) == true ? Infragistics.Win.DefaultableBoolean.True : Infragistics.Win.DefaultableBoolean.False));

                }

                ultraExplorerBar1.Groups["POS_Terminal"].Visible = UserPriviliges.IsUserHasPriviliges(UserPriviliges.Modules.POSTransaction.ID);
                ultraExplorerBar1.Groups["POS_Terminal"].Enabled = UserPriviliges.IsUserHasPriviliges(UserPriviliges.Modules.POSTransaction.ID);

                ultraExplorerBar1.Groups["POS_Terminal"].Items["Customers"].Visible = UserPriviliges.IsUserHasPriviliges(UserPriviliges.Modules.POSTransaction.ID, UserPriviliges.Screens.Customers.ID);
                ultraExplorerBar1.Groups["POS_Terminal"].Items["Customers"].Settings.Enabled = (UserPriviliges.IsUserHasPriviliges(UserPriviliges.Modules.POSTransaction.ID, UserPriviliges.Screens.Customers.ID) == true ? Infragistics.Win.DefaultableBoolean.True : Infragistics.Win.DefaultableBoolean.False);

                ultraExplorerBar1.Groups["POS_Terminal"].Items["Sales"].Visible = UserPriviliges.IsUserHasPriviliges(UserPriviliges.Modules.POSTransaction.ID, UserPriviliges.Screens.POSTransaction.ID);
                ultraExplorerBar1.Groups["POS_Terminal"].Items["Sales"].Settings.Enabled = (UserPriviliges.IsUserHasPriviliges(UserPriviliges.Modules.POSTransaction.ID, UserPriviliges.Screens.POSTransaction.ID) == true ? Infragistics.Win.DefaultableBoolean.True : Infragistics.Win.DefaultableBoolean.False);

                ultraExplorerBar1.Groups["POS_Terminal"].Items["Pay_Out"].Visible = UserPriviliges.IsUserHasPriviliges(UserPriviliges.Modules.POSTransaction.ID, UserPriviliges.Screens.Payout.ID);
                ultraExplorerBar1.Groups["POS_Terminal"].Items["Pay_Out"].Settings.Enabled = (UserPriviliges.IsUserHasPriviliges(UserPriviliges.Modules.POSTransaction.ID, UserPriviliges.Screens.Payout.ID) == true ? Infragistics.Win.DefaultableBoolean.True : Infragistics.Win.DefaultableBoolean.False);

                ultraExplorerBar1.Groups["Administrative_Function"].Visible = ultraExplorerBar1.Groups["Administrative_Function"].Enabled = UserPriviliges.IsUserHasPriviliges(UserPriviliges.Modules.AdminFunc.ID);
                if (ultraExplorerBar1.Groups["Administrative_Function"].Visible == true)
                {

                    ultraExplorerBar1.Groups["Administrative_Function"].Items["Departments"].Settings.Enabled = (UserPriviliges.IsUserHasPriviliges(UserPriviliges.Modules.AdminFunc.ID, UserPriviliges.Screens.Department.ID) == true ? Infragistics.Win.DefaultableBoolean.True : Infragistics.Win.DefaultableBoolean.False);
                    ultraExplorerBar1.Groups["Administrative_Function"].Items["Departments"].Visible = ultraExplorerBar1.Groups["Administrative_Function"].Items["Departments"].Visible = UserPriviliges.IsUserHasPriviliges(UserPriviliges.Modules.AdminFunc.ID, UserPriviliges.Screens.Department.ID);
                    //ultraExplorerBar1.Groups["Administrative_Function"].Items["Departments"].Settings.Enabled = (UserPriviliges.IsUserHasPriviliges(clsPOSDBConstants.Users_Fld_AllowDeptFileEdit)==true? Infragistics.Win.DefaultableBoolean.True:Infragistics.Win.DefaultableBoolean.False); 

                    ultraExplorerBar1.Groups["Administrative_Function"].Items["Close_Station"].Visible = ultraExplorerBar1.Groups["Administrative_Function"].Items["Close_Station"].Visible = UserPriviliges.IsUserHasPriviliges(UserPriviliges.Modules.AdminFunc.ID, UserPriviliges.Screens.StationClose.ID);
                    ultraExplorerBar1.Groups["Administrative_Function"].Items["Close_Station"].Settings.Enabled = (UserPriviliges.IsUserHasPriviliges(UserPriviliges.Modules.AdminFunc.ID, UserPriviliges.Screens.StationClose.ID) == true ? Infragistics.Win.DefaultableBoolean.True : Infragistics.Win.DefaultableBoolean.False);

                    ultraExplorerBar1.Groups["Administrative_Function"].Items["End_Of_Day"].Visible = ultraExplorerBar1.Groups["Administrative_Function"].Items["End_Of_Day"].Visible = UserPriviliges.IsUserHasPriviliges(UserPriviliges.Modules.AdminFunc.ID, UserPriviliges.Screens.EndOfDay.ID);
                    ultraExplorerBar1.Groups["Administrative_Function"].Items["End_Of_Day"].Settings.Enabled = (UserPriviliges.IsUserHasPriviliges(UserPriviliges.Modules.AdminFunc.ID, UserPriviliges.Screens.EndOfDay.ID) == true ? Infragistics.Win.DefaultableBoolean.True : Infragistics.Win.DefaultableBoolean.False);

                    ultraExplorerBar1.Groups["Administrative_Function"].Items["User_Setup"].Visible = UserPriviliges.IsUserHasPriviliges(UserPriviliges.Modules.AdminFunc.ID, UserPriviliges.Screens.ManageUsers.ID);
                    ultraExplorerBar1.Groups["Administrative_Function"].Items["User_Setup"].Settings.Enabled = (UserPriviliges.IsUserHasPriviliges(UserPriviliges.Modules.AdminFunc.ID, UserPriviliges.Screens.ManageUsers.ID) == true ? Infragistics.Win.DefaultableBoolean.True : Infragistics.Win.DefaultableBoolean.False);

                    ultraExplorerBar1.Groups["Administrative_Function"].Items["Tax_Codes"].Visible = UserPriviliges.IsUserHasPriviliges(UserPriviliges.Modules.AdminFunc.ID, UserPriviliges.Screens.TaxCodes.ID);
                    ultraExplorerBar1.Groups["Administrative_Function"].Items["Tax_Codes"].Settings.Enabled = (UserPriviliges.IsUserHasPriviliges(UserPriviliges.Modules.AdminFunc.ID, UserPriviliges.Screens.TaxCodes.ID) == true ? Infragistics.Win.DefaultableBoolean.True : Infragistics.Win.DefaultableBoolean.False);

                    ultraExplorerBar1.Groups["Administrative_Function"].Items["Function_Keys"].Visible = UserPriviliges.IsUserHasPriviliges(UserPriviliges.Modules.AdminFunc.ID, UserPriviliges.Screens.FunctionKeys.ID);
                    ultraExplorerBar1.Groups["Administrative_Function"].Items["Function_Keys"].Settings.Enabled = (UserPriviliges.IsUserHasPriviliges(UserPriviliges.Modules.AdminFunc.ID, UserPriviliges.Screens.FunctionKeys.ID) == true ? Infragistics.Win.DefaultableBoolean.True : Infragistics.Win.DefaultableBoolean.False);

                }

                #endregion

                ultMenuBar.Tools[POSMenuItems.UserSetup].SharedProps.Visible = UserPriviliges.IsUserHasPriviliges(UserPriviliges.Modules.AdminFunc.ID, UserPriviliges.Screens.ManageUsers.ID);
                ultMenuBar.Tools[POSMenuItems.UserSetup].SharedProps.Enabled = UserPriviliges.IsUserHasPriviliges(UserPriviliges.Modules.AdminFunc.ID, UserPriviliges.Screens.ManageUsers.ID);
                //ultMenuBar.Toolbars["tlbAdministrativeFunction"].Tools["t"+POSMenuItems.UserSetup].SharedProps.Visible=true;
                ultraExplorerBar1.Groups["Administrative_Function"].Items["User_Setup"].Visible = UserPriviliges.IsUserHasPriviliges(UserPriviliges.Modules.AdminFunc.ID, UserPriviliges.Screens.ManageUsers.ID);

                if (ultMenuBar.Tools[POSMenuItems.UserSetup].SharedProps.Visible == false)
                {
                    ultMenuBar.Tools[POSMenuItems.UserSetup].SharedProps.Shortcut = new Shortcut();
                }
                ultMenuBar.Tools[POSMenuItems.ViewStationClose].SharedProps.Visible = UserPriviliges.IsUserHasPriviliges(UserPriviliges.Modules.AdminFunc.ID, UserPriviliges.Screens.StationClose.ID, UserPriviliges.Permissions.ViewStationClose.ID);

                #region PRIMEPOS-2808
                ultMenuBar.Tools[POSMenuItems.ViewAuditTrail].SharedProps.ShowInCustomizer = ultMenuBar.Tools[POSMenuItems.ViewAuditTrail].SharedProps.Visible = ultMenuBar.Tools[POSMenuItems.ViewAuditTrail].SharedProps.Enabled = UserPriviliges.IsUserHasPriviliges(UserPriviliges.Modules.AdminFunc.ID, UserPriviliges.Screens.ViewAuditLog.ID);
                ultMenuBar.Tools[POSMenuItems.ViewNoSaleTransaction].SharedProps.ShowInCustomizer = ultMenuBar.Tools[POSMenuItems.ViewNoSaleTransaction].SharedProps.Visible = ultMenuBar.Tools[POSMenuItems.ViewNoSaleTransaction].SharedProps.Enabled = UserPriviliges.IsUserHasPriviliges(UserPriviliges.Modules.AdminFunc.ID, UserPriviliges.Screens.ViewNoSaleTransaction.ID);
                #endregion

                if (Configuration.CPOSSet.PaymentProcessor == "EVERTEC")
                {
                    ultMenuBar.Tools[POSMenuItems.ProcessEvertecSettlement].SharedProps.Visible = true;
                }
                else
                {
                    ultMenuBar.Tools[POSMenuItems.ProcessEvertecSettlement].SharedProps.Visible = false;
                }
            }
            catch (Exception exp)
            {
                Resources.Message.Display(exp.Message);
            }
        }

        private void frmMain_Resize(object sender, System.EventArgs e)
        {
        }
        private static Type CreateFormObject(string Key, ref string sTableName, ref string sFormCaption, ref string sLabelText1, ref string sLabelText2)
        {
            Type oType;
            oType = null;
            switch (Key)
            {
                case POSMenuItems.Sales:
                    POS_Core.ErrorLogging.Logs.Logger(clsPOSDBConstants.Log_Module_Transaction, "CreateFormObject", clsPOSDBConstants.Log_Intitialize_Object + "  frmPOSTransaction");
                    oType = typeof(frmPOSTransaction);//added by sandeep to run new form
                    if (oNumericPad != null)
                        if (!oNumericPad.IsDisposed)
                        {
                            oNumericPad.Close();
                            oNumericPad = null;
                        }
                    break;
                case POSMenuItems.SaleReturns:
                    POS_Core.ErrorLogging.Logs.Logger(clsPOSDBConstants.Log_Module_Transaction, "CreateFormObject", clsPOSDBConstants.Log_Intitialize_Object + "  frmPOSTransaction");
                    oType = typeof(frmPOSTransaction);//added by sandeep to run new form
                    break;
                case POSMenuItems.PayOut:
                    oType = typeof(frmPayOut);
                    break;
                //Added by shitaljit on 15 march 2012
                case POSMenuItems.PayOutCat:
                    oType = typeof(frmSearchMain);
                    sTableName = clsPOSDBConstants.PayOutCat_tbl;
                    sFormCaption = "Payout Category Type File";
                    sLabelText1 = "I&D";
                    sLabelText2 = "&Category";
                    break;

                case POSMenuItems.Coupon:
                    oType = typeof(frmSearchMain);
                    sTableName = clsPOSDBConstants.Coupon_tbl;
                    sFormCaption = "Coupon";
                    sLabelText1 = "Code";
                    sLabelText2 = "ID";
                    break;
                case POSMenuItems.Physical_Inventory:
                    oType = typeof(frmPhysicalInv);
                    break;
                case POSMenuItems.Customers:
                    oType = typeof(frmSearchMain);
                    sTableName = clsPOSDBConstants.Customer_tbl;
                    sFormCaption = "Customer File";
                    sLabelText1 = "A&cct#";
                    sLabelText2 = "&Name";
                    break;
                case POSMenuItems.ItemListByVendor:
                case POSMenuItems.InventoryRecieved:
                    oType = typeof(frmInventoryRecieved);
                    break;
                case POSMenuItems.PurchaseOrder:
                case POSMenuItems.Purchase_Order:
                    //oType=typeof(frmPurchaseOrder);
                    //break;
                    oType = typeof(frmSearchMain);
                    sTableName = clsPOSDBConstants.POHeader_tbl;
                    sFormCaption = "Purchase Order";
                    sLabelText1 = "Order";
                    sLabelText2 = "Code";
                    break;
                case POSMenuItems.ItemFile:
                case POSMenuItems.Items:
                    oType = typeof(frmSearchMain);
                    sTableName = clsPOSDBConstants.Item_tbl;
                    sFormCaption = "Item File";
                    sLabelText1 = "Item Co&de";
                    sLabelText2 = "Item &Name";
                    break;
                case POSMenuItems.ViewStationClose:
                    if (UserPriviliges.IsUserHasPriviliges(UserPriviliges.Modules.AdminFunc.ID, UserPriviliges.Screens.StationClose.ID, UserPriviliges.Permissions.ViewStationClose.ID))
                    {
                        oType = typeof(frmViewEODStation);
                        sTableName = clsPOSDBConstants.StationCloseHeader_tbl;
                        sFormCaption = "View Station Close";
                        sLabelText1 = "";
                        sLabelText2 = "";
                    }
                    break;
                case POSMenuItems.ViewEndOfDay:
                    oType = typeof(frmViewEODStation);
                    sTableName = clsPOSDBConstants.EndOfDay_tbl;
                    sFormCaption = "View End Of Day";
                    sLabelText1 = "";
                    sLabelText2 = "";
                    break;
                case POSMenuItems.Departments:
                    oType = typeof(frmSearchMain);
                    sTableName = clsPOSDBConstants.Department_tbl;
                    sFormCaption = "Department File";
                    sLabelText1 = "Dept. Co&de";
                    sLabelText2 = "&Name";
                    break;
                case POSMenuItems.PointsRewardTier:
                    oType = typeof(frmSearchMain);
                    sTableName = clsPOSDBConstants.CLPointsRewardTier_tbl;
                    sFormCaption = "Points Reward Tier File";
                    sLabelText1 = "Co&de";
                    sLabelText2 = "&Name";
                    break;
                case POSMenuItems.RegisterCLCards:
                    oType = typeof(frmSearchMain);
                    sTableName = clsPOSDBConstants.CLCards_tbl;
                    sFormCaption = "Customer Loyalty Cards File";
                    sLabelText1 = "Co&de";
                    sLabelText2 = "&Name";
                    break;
                case POSMenuItems.TaxCodes:
                    oType = typeof(frmSearchMain);
                    sTableName = clsPOSDBConstants.TaxCodes_tbl;
                    sFormCaption = "TaxCodes File";
                    sLabelText1 = "Tax Co&de";
                    sLabelText2 = "Tax &Name";
                    break;
                case POSMenuItems.WarningMessages:
                    oType = typeof(frmSearchMain);
                    sTableName = clsPOSDBConstants.WarningMessages_tbl;
                    sFormCaption = "Warning Messages File";
                    sLabelText1 = "Co&de";
                    sLabelText2 = "&Message";
                    break;
                case POSMenuItems.ItemMonitorCategory:
                    oType = typeof(frmSearchMain);
                    sTableName = clsPOSDBConstants.ItemMonitorCategory_tbl;
                    sFormCaption = "Item Monitor Category";
                    sLabelText1 = "ID";
                    sLabelText2 = "&Description";
                    break;
                case POSMenuItems.InvTransType:
                    oType = typeof(frmSearchMain);
                    sTableName = clsPOSDBConstants.InvTransType_tbl;
                    sFormCaption = "Inv. Trans. Type File";
                    sLabelText1 = "I&D";
                    sLabelText2 = "Type &Name";
                    break;
                case POSMenuItems.Vendors:
                    oType = typeof(frmSearchMain);
                    sTableName = clsPOSDBConstants.Vendor_tbl;
                    sFormCaption = "Vendor File";
                    sLabelText1 = "Vendor Co&de";
                    sLabelText2 = "&Name";
                    break;
                case POSMenuItems.UserSetup:
                case POSMenuItems.UserGroup:
                    //oType=typeof(UserManagement.frmUser);
                    //break;
                    oType = typeof(frmSearchMain);

                    if (Key == POSMenuItems.UserSetup)
                    {
                        sTableName = clsPOSDBConstants.Users_tbl;
                        sFormCaption = "Users";
                        sLabelText1 = "&User Name";
                        sLabelText2 = "Draw &No";
                    }
                    else
                    {
                        sTableName = clsPOSDBConstants.UsersGroup_tbl;
                        sFormCaption = "Users Group";
                        sLabelText1 = "&Code";
                        sLabelText2 = "&Name";
                    }
                    break;
                case POSMenuItems.FunctionKeys:
                    oType = typeof(frmFunctionKeys);
                    break;
                case POSMenuItems.CloseStation:
                    oType = typeof(frmStationClose);
                    break;
                case POSMenuItems.Preferences:
                    oType = typeof(frmPrefrences);
                    break;
                case POSMenuItems.EndOfDay:
                    oType = typeof(frmEndOfDay);
                    break;
                case POSMenuItems.Lock:
                    break;
                case POSMenuItems.Logoff:
                    break;
                case POSMenuItems.ItemCompanion:
                    oType = typeof(frmCompanionItem);
                    break;
                //Commented by Krishna on 7 July 2011

                //case POSMenuItems.ViewPOSTrans:
                //    oType = typeof(frmViewPOSTransaction);
                //    sFormCaption = "View POS Transaction";
                //    sLabelText1 = "";
                //    sLabelText2 = "";
                //    break;
                //oType=typeof(frmViewTransaction);

                //Till here Commented by Krishna on 7 July 2011
                //Following Code added by Krishna on 7 July 2011
                case POSMenuItems.ViewPOSTrans:
                    //oType = typeof(frmViewPOSTransaction);
                    frmViewPOSTransaction ofrmViewPOSTransaction = new frmViewPOSTransaction();
                    ofrmViewPOSTransaction.Show();
                    sFormCaption = "View POS Transaction";
                    sLabelText1 = "";
                    sLabelText2 = "";
                    break;
                //till here added by Krishna on 7 July 2011
                case POSMenuItems.DetailTransactionReport:
                    oType = typeof(frmRptTransactionDetail);
                    break;
                case POSMenuItems.SSalesReportByUID:
                    oType = typeof(frmRptSSalesByUID);
                    break;
                case POSMenuItems.SSalesReportByItem:
                    oType = typeof(frmRptSSalesByItem);
                    break;
                case POSMenuItems.SalesReportByDept:
                    oType = typeof(frmRptSalesByDepartment);
                    break;
                case POSMenuItems.SalesReportByPayment:
                    oType = typeof(frmRptSaleByPayType);
                    break;
                case POSMenuItems.StationCloseSummary:
                    oType = typeof(frmRptCloseStationSummary);
                    break;
                case POSMenuItems.TopSellingProducts:
                    oType = typeof(frmRptTopSellingProducts);
                    break;
                case POSMenuItems.SaleAnalysis:
                    oType = typeof(frmRptSaleAnalysisByProduct);
                    break;
                case POSMenuItems.SalesTaxSummary:
                    oType = typeof(frmRptSalesTax);
                    break;
                case POSMenuItems.Phy_Inv_History:
                    oType = typeof(frmRptPhysicalInventoryHistory);
                    break;
                case POSMenuItems.saleProductivityReport:
                    oType = typeof(frmRptProductivityReport);
                    break;
                case POSMenuItems.PurchaseOrderReport:
                    oType = typeof(frmRptPurchaseOrder);
                    break;
                case POSMenuItems.SalesByCustomer:
                    oType = typeof(frmRptSalesByCustomer);
                    break;
                case POSMenuItems.DeliveryListReport:
                    oType = typeof(frmRptDelivery);
                    break;
                case POSMenuItems.SalesByInsurance:
                    oType = typeof(frmRptSalesByRXIns);
                    break;
                case POSMenuItems.SalesComparisonByDept:
                    oType = typeof(frmRptSalesComparisonByDept);
                    break;
                case POSMenuItems.CustomerList:
                    oType = typeof(frmRptCustomerList);
                    break;
                case POSMenuItems.SalesByItemMonitoringCategory:
                    oType = typeof(frmRptSalesByItemMonitoring);
                    break;
                case POSMenuItems.SalesPseudoephedrineLogs:
                    oType = typeof(frmRptSalesPseudoephedrineLogs); //PRIMEPOS-3360
                    break;
                case POSMenuItems.ItemLabelReport:
                    oType = typeof(frmRptItemLabel);
                    break;
                case POSMenuItems.POViewPurchaseOrdPoPending:
                case POSMenuItems.POViewPurchaseOrdPoQueued:
                case POSMenuItems.Error:
                case POSMenuItems.Overdue:
                case POSMenuItems.ViewPurchaseOrder:
                    oType = typeof(frmViewPurchaseOrderStatus);
                    break;
                case POSMenuItems.ViewInventoryRecieved:
                    oType = typeof(frmViewInvRecieved);
                    break;
                // changed by Abhishek(03-02 ) 
                case POSMenuItems.POAcknowledgement:
                case POSMenuItems.POAcknowledgementPoProcessed:
                    oType = typeof(frmSearchPOAck);
                    break;
                case POSMenuItems.PoLogScreen:
                    oType = typeof(frmPOLogScreen);
                    break;
                case POSMenuItems.MyStore:
                    oType = typeof(frmViewMyStore);
                    break;
                case POSMenuItems.LastUpdate:
                    oType = typeof(frmLastUpdate);
                    break;
                case POSMenuItems.ManageTimesheet:
                    frmTimesheet ofrmTimesheet = new frmTimesheet();
                    //ofrmTimesheet.ShowDialog(); //Commented By Dharmedra
                    oType = typeof(frmTimesheet);
                    break;
                case POSMenuItems.PayoutDetails:
                    oType = typeof(frmRptPayoutDetails);
                    break;
                case POSMenuItems.SaleTaxControl:
                    oType = typeof(frmRptSalesTaxControl);
                    break;
                //added by atul 25-oct-2010
                case POSMenuItems.DeliveryReceived:
                    oType = typeof(frmSearchPOAck);
                    break;
                //End of added by atul 25-oct-2010
                case POSMenuItems.ItemComboPricing:
                    oType = typeof(frmSearchMain);
                    sTableName = clsPOSDBConstants.ItemComboPricing_tbl;
                    sFormCaption = "Item Combo Pricing";
                    sLabelText1 = "I&D";
                    sLabelText2 = "De&scription";
                    break;
                case POSMenuItems.PayType:
                    oType = typeof(frmSearchMain);
                    sTableName = clsPOSDBConstants.PayType_tbl;
                    sFormCaption = "Pay Types";
                    sLabelText1 = "Co&de";
                    sLabelText2 = "&Name";
                    break;
                case POSMenuItems.ViewCLSummary:
                    oType = typeof(frmViewCLInfo);
                    break;
                case POSMenuItems.EODSummary:   //Sprint-21 - 1861 23-Jul-2015 JY Added
                    oType = typeof(frmRptEODSummary);
                    break;
                //case POSMenuItems.DepartmentList:   //Sprint-23 - PRIMEPOS-2292 05-Jul-2016 JY Added 
                //    oType = typeof(frmRptDeptList);
                //    break;
                case POSMenuItems.OnHoldTransactionDetailReport:    //PRIMEPOS-2580 05-Sep-2018 JY Added
                    oType = typeof(frmRptOnholdTransactionDetails);
                    break;
                case POSMenuItems.ViewStoreCreditSummary: // PRIMEPOS-2747 - NileshJ - 20-Nov-2019 - StoreCredit 
                    oType = typeof(frmViewStoreCreditSummary);
                    break;

                case POSMenuItems.RecoveryTransaction: // PRIMEPOS-2761
                    oType = typeof(frmRecoveryTransaction);
                    break;
                case POSMenuItems.RxRecoveryUpdate: // PRIMEPOS-2761
                    oType = typeof(frmRxRecovery);
                    break;
                case POSMenuItems.ViewAuditTrail: // PRIMEPOS-2808
                    oType = typeof(frmViewAuditTrail);
                    break;
                case POSMenuItems.ViewNoSaleTransaction: // PRIMEPOS-2808
                    oType = typeof(frmViewNoSaleTransaction);
                    break;
                case POSMenuItems.ScheduledTask:    //PRIMEPOS-2485 16-Mar-2021 JY Added
                    oType = typeof(frmScheduledTasksView);
                    break;
                case POSMenuItems.CreditCardProfiles: //PRIMEPOS-3004 19-Oct-2021 JY Added
                    oType = typeof(frmCreditCardProfiles);
                    break;
                case POSMenuItems.TransactionFee:   //PRIMEPOS-3116 11-Jul-2022 JY Added
                    oType = typeof(frmSearchMain);
                    sTableName = clsPOSDBConstants.TransFee_tbl;
                    sFormCaption = "Transaction Fee";
                    sLabelText1 = "TransFeeID";
                    sLabelText2 = "TransFeeDesc";
                    break;
                case POSMenuItems.TransactionFeeReport: //PRIMEPOS-3136 30-Aug-2022 JY Added
                    oType = typeof(frmRptTransFee);
                    break;
            }
            return oType;
        }

        private void frmMain_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
        {
        }

        private void frmMain_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
        }

        private void btnNumericKeyPad_Click(object sender, System.EventArgs e)
        {

            if (m_threadStarted) return;
            Thread thread = new Thread(new ThreadStart(showNumericKeyPad));
            thread.Start();
            m_threadStarted = true;
            //			NumericPad.Show();

            //			frmNumericPad nkp = new frmNumericPad();
            //			Application.(nkp);
        }

        private void showNumericKeyPad()
        {
            oPublicNumericPad = NumericPad;
            Application.Run(oPublicNumericPad);
            m_threadStarted = false;
        }

        private void btnNumericKeyPad_MouseEnter(object sender, System.EventArgs e)
        {
        }

        private void btnNumericKeyPad_MouseLeave(object sender, System.EventArgs e)
        {
        }

        private void frmMain_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                // NileshJ-Clear Item on Close Button from Exe 
                if (sender.ToString().Contains("Sale Transaction"))
                    ((frmPOSTransaction)selectedForm).SetNew(false);

                #region PRIMEPOS-1923 08-Aug-2018 JY Added
                Boolean bStatus = true;
                foreach (Form frm in frmMain.getInstance().MdiChildren)
                {
                    if (frm is frmPOSTransaction)
                    {
                        if (TransactionRecordCount > 0)
                        {
                            if (!UserPriviliges.getPermission(UserPriviliges.Modules.POSTransaction.ID, UserPriviliges.Screens.POSTransaction.ID, UserPriviliges.Permissions.DeleteTrans.ID, UserPriviliges.Permissions.DeleteTrans.Name))
                            {
                                bStatus = false;
                            }
                        }
                    }
                }
                #endregion

                //if frmtrans form is active then check record, if > 0 then check access, if false, then popup override screen
                //if (Resources.Message.Display("Are you sure you want to exit?", Configuration.ApplicationName, MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)  //PRIMEPOS-1923 08-Aug-2018 JY Commented
                if (bStatus == true && Resources.Message.Display("Are you sure you want to exit?", Configuration.ApplicationName, MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes) //PRIMEPOS-1923 08-Aug-2018 JY Added
                {
                    int isPOS, isInv, isAdmin;
                    if (this.ultMenuBar.Toolbars["tlbPOSTerminal"].Visible)
                        isPOS = 1;
                    else
                        isPOS = 0;

                    if (this.ultMenuBar.Toolbars["tlbInventoryManagement"].Visible)
                        isInv = 1;
                    else
                        isInv = 0;
                    if (this.ultMenuBar.Toolbars["tlbAdministrativeFunction"].Visible)
                        isAdmin = 1;
                    else
                        isAdmin = 0;
                    //PRIMEPOS-2664 ADDED BY ARVIND
                    if (Configuration.CPOSSet.PaymentProcessor == clsPOSDBConstants.EVERTEC)
                    {
                        String displayMessage = String.Empty;
                        String _posSettings = Configuration.CPOSSet.SigPadHostAddr;

                        string hostAddress = _posSettings.Split(':')[0];
                        string hostPort = _posSettings.Split(':')[1].Split('/')[0];

                        device = EvertechProcessor.getInstance(hostAddress, Convert.ToInt32(hostPort));
                        device.Logoff();
                    }
                    Configuration.SaveToolbarSettings(isPOS, isInv, isAdmin);
                    //Added by Manoj for the POLEDISPLAY.
                    if (Configuration.CPOSSet.UsePoleDisplay)
                    {
                        PoleDisplay.CloseSerialPort();
                    }

                    //Added by SRT for Pinpad Integration - 30-8-08
                    if (Configuration.CPOSSet.UseSigPad == true)
                    {
                        SigPadUtil.CloseDefaultInstace();
                    }
                    //Modified & Added By Dharmendra(SRT) on Dec-16-08 to dispose the objects of frmPinPad & PccPaymentSvr
                    //Modified By SRT(Abhishek) Date : 21/10/2009
                    //Added condition to check condition if it is not Xlink.
                    if (Configuration.CPOSSet.UsePinPad == true && Configuration.CPOSSet.PaymentProcessor.Trim() != "XLINK")
                    {
                        frmPinPad.CloseDefaultInstace();
                    }

                    if (Configuration.CPOSSet.PaymentProcessor.Trim() == "PCCHARGE" || Configuration.CPOSSet.PaymentProcessor.Trim() == "XCHARGE" || Configuration.CPOSSet.PaymentProcessor.Trim() == "XLINK")
                    {
                        PccPaymentSvr.CloseDefaultInstace();
                    }
                    if (oNumericPad != null)
                    {
                        oNumericPad.Close();
                        oNumericPad = null;
                    }
                    //Modified & Added till here Dec-16-08
                }
                else
                {
                    e.Cancel = true;
                    foreach (Form frm in frmMain.getInstance().MdiChildren)
                    {
                        if (frm is frmPOSTransaction)
                        {
                            frm.WindowState = FormWindowState.Minimized;
                            frm.WindowState = FormWindowState.Maximized;
                            if (frm.MdiParent != null)
                            {
                                ((frmMain)frm.MdiParent).ultraExplorerBar1.Visible = false;
                                ((frmMain)frm.MdiParent).ultMenuBar.Visible = false;
                                ((frmMain)frm.MdiParent).ultMenuBar.Enabled = false;
                                ((frmMain)frm.MdiParent).ultraExplorerBar1.Enabled = false;
                                ((frmMain)frm.MdiParent).ultraStatusBarVendor.Visible = false;
                                ((frmMain)frm.MdiParent).ultraStatusBarVendor.Enabled = false;
                                if (oNumericPad != null)
                                {
                                    oNumericPad.Close();
                                    oNumericPad = null;
                                }
                                ((frmPOSTransaction)frm).btnNumericPad_Click(this, new EventArgs());
                            }
                        }
                    }
                }
                //clsEPurchaseOrder.StopThread();
            }
            catch (Exception Ex) { }
        }

        private void frmMain_Activated(object sender, System.EventArgs e)
        {
            /*	clsCoreUIHelper.CurrentForm=this.ActiveMdiChild;
                if (clsCoreUIHelper.CurrentForm!=null && clsCoreUIHelper.CurrentForm.Name=="frmPOSTransaction")
                {
                    frmPOSTransaction frm = (frmPOSTransaction)clsCoreUIHelper.CurrentForm;
                    frm.frmNumPad.Visible = true;
                } */

        }

        private void frmMain_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
        {
            MenuBarHeight = this.ultMenuBar.GetDockHeight(DockedPosition.Top);
            statusBarVendorHeight = this.ultraStatusBarVendor.Height;
            statusBarHeight = this.ultraStatusBar.Height;
        }

        private void gbNumericPad_Click(object sender, System.EventArgs e)
        {

        }
        private void gbNumericPad_Enter(object sender, System.EventArgs e)
        {
        }

        private void gbNumericPad_Leave(object sender, System.EventArgs e)
        {
        }

        private void frmMain_Deactivate(object sender, System.EventArgs e)
        {
            /*			if (clsCoreUIHelper.CurrentForm!=null && clsCoreUIHelper.CurrentForm.Name=="frmPOSTransaction")
                        {
                            frmPOSTransaction frm = (frmPOSTransaction)clsCoreUIHelper.CurrentForm;
                            frm.frmNumPad.Visible = false;
                        } */
        }

        private void frmMain_Leave(object sender, System.EventArgs e)
        {
        }

        public void InstallActivityMonitor()
        {
            try
            {
                if (Configuration.INACTIVE_INTERVAL <= 0)
                    return;
                inactivityMonitor = new InactivityMonitor.HookMonitor(false);
                inactivityMonitor.MonitorKeyboardEvents = true;
                inactivityMonitor.MonitorMouseEvents = true;
                inactivityMonitor.Interval = Configuration.INACTIVE_INTERVAL * 1000;
                inactivityMonitor.SynchronizingObject = this;
                inactivityMonitor.Elapsed += new System.Timers.ElapsedEventHandler(inactivityMonitor_Elapsed);
                inactivityMonitor.Reactivated += new EventHandler(inactivityMonitor_Reactivated);

                inactivityMonitor.Enabled = true;
            }
            catch (Exception)
            {

            }
        }

        //Fired after the time elapsed without any user activtiy 
        private void inactivityMonitor_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {

            System.Collections.IEnumerator theChildEnum = this.MdiChildren.GetEnumerator();
            System.Collections.IEnumerator theFormEnum = this.OwnedForms.GetEnumerator();

            try
            {
                inactivityMonitor.Enabled = false;
                clsUIHelper.LocakStation();
                inactivityMonitor.Enabled = true;
            }
            catch (Exception ex)
            {
                clsCoreUIHelper.ShowErrorMsg(ex.Message);
            }
        }

        //Fired after the user perform keyboard or mouse input
        private void inactivityMonitor_Reactivated(object sender, EventArgs e)
        {

        }

        private void ultraStatusBar_PanelDoubleClick(object sender, Infragistics.Win.UltraWinStatusBar.PanelClickEventArgs e)
        {
            try
            {
                if (e.Panel.Key == "Msg")
                {
                    if (oMessaging != null)
                    {
                        oMessaging.DisplayInbox();
                    }
                }

            }
            catch (Exception exp)
            {
                clsCoreUIHelper.ShowErrorMsg(exp.Message);
            }
        }

        /// <summary>
        /// Author : Dharmendra
        /// Functionality Description : This method searches for the processes named "POS.exe"
        /// if the processes is found, then the Kill method kills the process "POS.exe"
        /// Known Bugs : None
        /// Start Date : 13-Oct-08
        /// </summary>
        private void KillProcess()
        {
            POS_Core.ErrorLogging.Logs.Logger("*****************************" + clsPOSDBConstants.Log_ApplicationClosed + "*****************************");
            Process[] pArry = Process.GetProcessesByName("POS");// Modified By Dharmendra (SRT) on Nov-17-08 . Earlier the old value was POS.exe
            foreach (Process p in pArry)
            {
                p.Kill();
            }
        }

        public static string GetPOACKForm
        {
            set { ofrmMain.screenNamePOAck = value; }
            get { return ofrmMain.screenNamePOAck; }
        }

        public static string GetViewPOForm
        {
            set { ofrmMain.screenNameViewPO = value; }
            get { return ofrmMain.screenNameViewPO; }
        }

        //Added to handle event from the panels
        private void ultraStatusBarVendor_PanelClick(object sender, Infragistics.Win.UltraWinStatusBar.PanelClickEventArgs e)
        {
            try
            {
                string keyName = e.Panel.Key;

                switch (keyName)
                {
                    case POSMenuItems.Queued:
                        frmMain.FromPanel = true;
                        frmMain.GetViewPOForm = PurchseOrdStatus.Queued.ToString();
                        ShowForm(POSMenuItems.ViewPurchaseOrder);
                        break;
                    case POSMenuItems.Submitted:
                        frmMain.FromPanel = true;
                        frmMain.GetViewPOForm = PurchseOrdStatus.Submitted.ToString();
                        ShowForm(POSMenuItems.ViewPurchaseOrder);
                        break;
                    case POSMenuItems.Acknowledged:
                        frmMain.FromPanel = true;
                        frmMain.GetPOACKForm = POSMenuItems.Acknowledged;
                        ShowForm(POSMenuItems.POAcknowledgementPoProcessed);
                        break;
                    //Added By SRT(Abhishek) 
                    case POSMenuItems.Expired:
                        frmMain.FromPanel = true;
                        frmMain.GetViewPOForm = PurchseOrdStatus.Expired.ToString();
                        ShowForm(POSMenuItems.ViewPurchaseOrder);
                        break;
                    case POSMenuItems.MaxAttempts:
                        frmMain.FromPanel = true;
                        frmMain.GetViewPOForm = PurchseOrdStatus.MaxAttempt.ToString();
                        ShowForm(POSMenuItems.ViewPurchaseOrder);
                        break;
                    case POSMenuItems.PoLogScreen:
                        frmMain.FromPanel = true;
                        ShowForm(POSMenuItems.PoLogScreen);
                        break;
                    case POSMenuItems.Error:
                        frmMain.FromPanel = true;
                        frmMain.GetViewPOForm = PurchseOrdStatus.Error.ToString();
                        ShowForm(POSMenuItems.ViewPurchaseOrder);
                        break;
                    case POSMenuItems.Overdue:
                        frmMain.FromPanel = true;
                        frmMain.GetViewPOForm = PurchseOrdStatus.Overdue.ToString();
                        ShowForm(POSMenuItems.ViewPurchaseOrder);
                        break;
                    case POSMenuItems.InComplete:
                        frmMain.FromPanel = true;
                        frmMain.GetViewPOForm = PurchseOrdStatus.Incomplete.ToString();
                        ShowForm(POSMenuItems.ViewPurchaseOrder);
                        break;
                    //Changes by Atul Joshi on 3-11-2010                        
                    case POSMenuItems.DeliveryReceived:
                        ShowForm("PO Acknowledgement");
                        break;
                    //End Changes by Atul Joshi on 3-11-2010   
                    case "":
                        break;
                }
            }
            catch (Exception ex)
            {
                clsCoreUIHelper.ShowErrorMsg(ex.ToString());
            }
        }
        //Added By Dharmendra (SRT) on Apr-11-09
        //To show or hide PO Status bar on the main screen
        private void ultraStatusBar_PanelClick(object sender, Infragistics.Win.UltraWinStatusBar.PanelClickEventArgs e)
        {
            string keyName = e.Panel.Key;
            if (keyName == "Show-HidePOBar")
            {
                if (Configuration.CPrimeEDISetting.UsePrimePO)   //PRIMEPOS-3167 07-Nov-2022 JY Modified
                {
                    if (poBarDisplayStatus == true)
                    {
                        ultraStatusBarVendor.Hide();
                        poBarDisplayStatus = false;
                    }
                    else
                    {
                        ultraStatusBarVendor.Show();
                        poBarDisplayStatus = true;
                    }
                }
            }
        }
        private void cntMenuPOStatus_Opening(object sender, CancelEventArgs e)
        {
            this.InComplete.Checked = POStatusList.Contains(POSTINCOMPLETE);
            this.Acknowledged.Checked = POStatusList.Contains(POSTACKNOWLEDGED);
            this.DeliveryReceived.Checked = POStatusList.Contains(POSTDELIVERYRECEIVED); // added by atul 25-oct-2010
            this.Expired.Checked = POStatusList.Contains(POSTEXPIRED);
            this.Error.Checked = POStatusList.Contains(POSTERROR);
            this.Overdue.Checked = POStatusList.Contains(POSTOVERDUE);
            this.maxAttemptToolStripMenuItem.Checked = POStatusList.Contains(POSTMAXATTEMPTS);
        }

        private void cntMenuPOStatus_Closed(object sender, ToolStripDropDownClosedEventArgs e)
        {
            POStatusList = new List<string>();
            if (this.InComplete.Checked)
            {
                POStatusList.Add(POSTINCOMPLETE);
            }
            if (this.Expired.Checked)
            {
                POStatusList.Add(POSTEXPIRED);
            }
            if (this.Error.Checked)
            {
                POStatusList.Add(POSTERROR);
            }
            if (this.Overdue.Checked)
            {
                POStatusList.Add(POSTOVERDUE);
            }
            if (this.maxAttemptToolStripMenuItem.Checked)
            {
                POStatusList.Add(POSTMAXATTEMPTS);
            }
            if (this.Acknowledged.Checked)
            {
                POStatusList.Add(POSTACKNOWLEDGED);
            }
            if (this.DeliveryReceived.Checked) // added by atul 25-oct-2010
            {
                POStatusList.Add(POSTDELIVERYRECEIVED);
            }
            //System.Configuration.ConfigurationManager.AppSettings["POSTATUS"] = string.Join("|", POStatusList.ToArray());
        }

        private void cntMenuPOStatus_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            try
            {

                bool checkedState = true;
                String panelClicked = e.ClickedItem.Text;

                switch (panelClicked)
                {
                    case "InComplete":
                        InComplete.Checked = !InComplete.Checked;
                        if (InComplete.Checked)
                        {
                            AddInStatusList(POSTINCOMPLETE);
                        }
                        checkedState = InComplete.Checked;
                        break;
                    case "Expired":
                        Expired.Checked = !Expired.Checked;
                        if (Expired.Checked)
                        {
                            AddInStatusList(POSTEXPIRED);
                        }
                        checkedState = Expired.Checked;
                        break;
                    case "Error":
                        Error.Checked = !Error.Checked;
                        if (Error.Checked)
                        {
                            AddInStatusList(POSTERROR);
                        }
                        checkedState = Error.Checked;
                        break;
                    case "Overdue":
                        Overdue.Checked = !Overdue.Checked;
                        if (Overdue.Checked)
                        {
                            AddInStatusList(POSTOVERDUE);
                        }
                        checkedState = Overdue.Checked;
                        break;
                    case "MaxAttempts":
                        maxAttemptToolStripMenuItem.Checked = !maxAttemptToolStripMenuItem.Checked;
                        if (maxAttemptToolStripMenuItem.Checked)
                        {
                            AddInStatusList(POSTMAXATTEMPTS);
                        }
                        checkedState = maxAttemptToolStripMenuItem.Checked;
                        break;
                    case "Acknowledged":
                        Acknowledged.Checked = !Acknowledged.Checked;
                        if (Acknowledged.Checked)
                        {
                            AddInStatusList(POSTACKNOWLEDGED);
                        }
                        checkedState = Acknowledged.Checked;
                        break;
                    //added by atul 25-oct-2010
                    case "DeliveryReceived":
                        DeliveryReceived.Checked = !DeliveryReceived.Checked;
                        if (DeliveryReceived.Checked)
                        {
                            AddInStatusList(POSTDELIVERYRECEIVED);
                        }
                        checkedState = DeliveryReceived.Checked;
                        break;
                        //End of added by atul 25-oct-2010
                }
                if (checkedState == false)
                {
                    ultraStatusBarVendor.Panels[panelClicked].Visible = false;
                }
            }
            catch (Exception ex) { }
        }

        private void ultMenuBar_MouseEnterElement(object sender, Infragistics.Win.UIElementEventArgs e)
        {
            try
            {
                ultMenuBar.Tools[POSMenuItems.ViewEndOfDay].SharedProps.Visible = UserPriviliges.IsUserHasPriviliges(UserPriviliges.Modules.AdminFunc.ID, UserPriviliges.Screens.EndOfDay.ID, UserPriviliges.Permissions.ViewEOD.ID);
                ultMenuBar.Tools[POSMenuItems.EndOfDay].SharedProps.Visible = UserPriviliges.IsUserHasPriviliges(UserPriviliges.Modules.AdminFunc.ID, UserPriviliges.Screens.EndOfDay.ID, UserPriviliges.Permissions.ProcessEOD.ID);
                ultMenuBar.Tools[POSMenuItems.ViewStationClose].SharedProps.Visible = UserPriviliges.IsUserHasPriviliges(UserPriviliges.Modules.AdminFunc.ID, UserPriviliges.Screens.StationClose.ID, UserPriviliges.Permissions.ViewStationClose.ID);

                ultMenuBar.Tools["CheckForUpdates"].SharedProps.Visible = Configuration.convertNullToBoolean(Configuration.CInfo.AllowAutomaticUpdates);    //Sprint-20 03-Jun-2015 JY Added settings for auto updater
                //ultMenuBar.Tools["StationSettings"].SharedProps.Visible = Configuration.convertNullToBoolean(Configuration.CInfo.AllowAutomaticUpdates);  //PRIMEPOS-2859 12-Jun-2020 JY Commented
                ultMenuBar.Tools[POSMenuItems.EODSummary].SharedProps.Visible = UserPriviliges.IsUserHasPriviliges(UserPriviliges.Modules.AdminFunc.ID, UserPriviliges.Screens.EndOfDay.ID, UserPriviliges.Permissions.ViewEOD.ID); //Sprint-21 - 1861 23-Jul-2015 JY Added

                #region Sprint-25 - PRIMEPOS-2253 23-Mar-2017 JY Added for timesheet user rights
                //Infragistics.Win.UltraWinToolbars.PopupMenuTool oPopupTimesheet = (Infragistics.Win.UltraWinToolbars.PopupMenuTool)ultMenuBar.Tools["Time Sheet"];
                //if (oPopupTimesheet.SharedProps.Visible == true)
                //{
                ultMenuBar.Tools[POSMenuItems.ManageTimesheet].SharedProps.ShowInCustomizer = ultMenuBar.Tools[POSMenuItems.ManageTimesheet].SharedProps.Visible = ultMenuBar.Tools[POSMenuItems.ManageTimesheet].SharedProps.Enabled = UserPriviliges.IsUserHasPriviliges(UserPriviliges.Modules.Timesheet.ID, UserPriviliges.Screens.Timesheet.ID); //Clock - In / Out User
                ultMenuBar.Tools[POSMenuItems.TimesheetReport].SharedProps.ShowInCustomizer = ultMenuBar.Tools[POSMenuItems.TimesheetReport].SharedProps.Visible = ultMenuBar.Tools[POSMenuItems.TimesheetReport].SharedProps.Enabled = (UserPriviliges.IsUserHasPriviliges(UserPriviliges.Modules.Timesheet.ID, UserPriviliges.Screens.TimesheetReport.ID) == false) ? (UserPriviliges.IsUserHasPriviliges(UserPriviliges.Modules.Timesheet.ID, UserPriviliges.Screens.TimesheetReportForLoginUser.ID)) : true;    //View / Print Created Timesheet 
                ultMenuBar.Tools[POSMenuItems.CreateTimesheet].SharedProps.ShowInCustomizer = ultMenuBar.Tools[POSMenuItems.CreateTimesheet].SharedProps.Visible = ultMenuBar.Tools[POSMenuItems.CreateTimesheet].SharedProps.Enabled = UserPriviliges.IsUserHasPriviliges(UserPriviliges.Modules.Timesheet.ID, UserPriviliges.Screens.TimesheetCreate.ID); //Manage / Create Timesheet
                #endregion

                #region PRIMEPOS-2484 04-Jun-2020 JY Added
                ultMenuBar.Tools[POSMenuItems.PSEItemFileListing].SharedProps.ShowInCustomizer = ultMenuBar.Tools[POSMenuItems.PSEItemFileListing].SharedProps.Visible = ultMenuBar.Tools[POSMenuItems.PSEItemFileListing].SharedProps.Enabled = UserPriviliges.IsUserHasPriviliges(UserPriviliges.Modules.AppSettings.ID, UserPriviliges.Screens.PSEItemList.ID);
                ultMenuBar.Tools[POSMenuItems.IIASItemFileListing].SharedProps.ShowInCustomizer = ultMenuBar.Tools[POSMenuItems.IIASItemFileListing].SharedProps.Visible = ultMenuBar.Tools[POSMenuItems.IIASItemFileListing].SharedProps.Enabled = UserPriviliges.IsUserHasPriviliges(UserPriviliges.Modules.AppSettings.ID, UserPriviliges.Screens.IIASFileList.ID);
                ultMenuBar.Tools[POSMenuItems.Lock].SharedProps.ShowInCustomizer = ultMenuBar.Tools[POSMenuItems.Lock].SharedProps.Visible = ultMenuBar.Tools[POSMenuItems.Lock].SharedProps.Enabled = UserPriviliges.IsUserHasPriviliges(UserPriviliges.Modules.AppSettings.ID, UserPriviliges.Screens.LockStation.ID);
                #endregion
                ultMenuBar.Tools[POSMenuItems.TransactionFeeReport].SharedProps.ShowInCustomizer = ultMenuBar.Tools[POSMenuItems.TransactionFeeReport].SharedProps.Visible = ultMenuBar.Tools[POSMenuItems.TransactionFeeReport].SharedProps.Enabled = UserPriviliges.IsUserHasPriviliges(UserPriviliges.Modules.AdminFunc.ID, UserPriviliges.Screens.AdminReports.ID, UserPriviliges.Permissions.TransactionFeeReport.ID);    //PRIMEPOS-3136 30-Aug-2022 JY Added
                ultMenuBar.Tools["Inventory_Recieved"].SharedProps.ShowInCustomizer = ultMenuBar.Tools["Inventory_Recieved"].SharedProps.Visible = ultMenuBar.Tools["Inventory_Recieved"].SharedProps.Enabled = UserPriviliges.IsUserHasPriviliges(UserPriviliges.Modules.InventoryMgmt.ID, UserPriviliges.Screens.InventoryRecvd.ID, UserPriviliges.Permissions.InventoryReceived.ID);  //PRIMEPOS-3141 27-Oct-2022 JY Added
                ultMenuBar.Tools["ViewInventoryRecieved"].SharedProps.ShowInCustomizer = ultMenuBar.Tools["ViewInventoryRecieved"].SharedProps.Visible = ultMenuBar.Tools["ViewInventoryRecieved"].SharedProps.Enabled = UserPriviliges.IsUserHasPriviliges(UserPriviliges.Modules.InventoryMgmt.ID, UserPriviliges.Screens.InventoryRecvd.ID, UserPriviliges.Permissions.ViewInventory.ID);    //PRIMEPOS-3141 27-Oct-2022 JY Added
                ultMenuBar.Tools[POSMenuItems.SalesPseudoephedrineLogs].SharedProps.ShowInCustomizer = ultMenuBar.Tools[POSMenuItems.SalesPseudoephedrineLogs].SharedProps.Visible = ultMenuBar.Tools[POSMenuItems.SalesPseudoephedrineLogs].SharedProps.Enabled = UserPriviliges.IsUserHasPriviliges(UserPriviliges.Modules.AdminFunc.ID, UserPriviliges.Screens.AdminReports.ID, UserPriviliges.Permissions.PseudoephedrineSalesLogs.ID); //PRIMEPOS-3360
            }
            catch
            { }
        }
        //Added Til Here April-11-09

        #region Sprint-20 27-May-2015 JY Added for Auto updater
        //non urgent updates are downloaded by this timer - which comes into picture when "UrgentDownload" settings is false 
        void tmrAutuUpdate_Tick(object sender, EventArgs e)
        {
            try
            {
                if (brunSchoudule == false && Configuration.CInfo.AllowAutomaticUpdates == true)
                {
                    //last parameter denotes "UrgentDownload" settings is false 
                    MMSAppUpdater.clsUpdateManager oAppUpdater = new MMSAppUpdater.clsUpdateManager(Configuration.CInfo.AutoUpdateServiceAddress, Configuration.CInfo.StoreID, Configuration.ConnectionString, strAppName, Configuration.StationID, false);
                    oAppUpdater.SetParentForm(this);
                    oAppUpdater.bws.RunWorkerAsync();
                    brunSchoudule = true;
                    tmrAutuUpdate.Stop();
                    tmrAutuUpdate.Enabled = false;
                    tmrAutuUpdate.Dispose();
                }
            }
            catch (Exception Ex)
            {
                logger.Fatal(Ex, "tmrAutuUpdate_Tick(object sender, EventArgs e)"); //PRIMEPOS-2641 14-Feb-2019 JY Added
            }
        }

        /// <summary>
        /// PRIMEPOS-3187
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        void tmrPrimeRxPayBGStatusUpdate_Tick(object source, ElapsedEventArgs e)
        {
            DataSet oDataSet = null;
            try
            {
                logger.Info($"Start PrimeRxPay Onhold transaction status update (Paid/Unpaid/Expired)");
                tmrPrimeRxPayBGStatusUpdate.Stop();
                PrimeRxPay.PrimeRxPayResponse oPrimeRxPayResponse;
                PrimeRxPay.PrimeRxPayProcessor oPrimeRxPay = PrimeRxPay.PrimeRxPayProcessor.GetInstance();
                POSTransPaymentSvr oPosTransPaymentSvr = new POSTransPaymentSvr();
                List<PaymentTypes> objPaymentTypes;
                List<long> PrimeRxPayTransID = new List<long>(); //PRIMEPOS-3333
                List<PrimeRxPay.PrimeRxPayResponse> oPrimeRxPayResponses = new List<PrimeRxPay.PrimeRxPayResponse>();//PRIMEPOS-3333
                PaymentTypes paymentTypes;
                DateTime paymentDate = DateTime.Now; //PRIMEPOS-3453
                string cardType = string.Empty;
                string ProfiledID = string.Empty;
                bool IsFSATransaction = false;
                string[] profileIdAndFSA;
                string strSQL = @"
                    

                    SELECT PTD.Amount AS [Amount]
                            , ApprovedAmount AS[Approved Amount]
                            , Email AS[Email]
                            , Mobile AS[Mobile]
                            , TransactionProcessingMode AS[TransactionProcessingMode]
                            , PTD.TransDate AS [TransDate]
                            , TransPayID AS[TransPayId]
                            , STATUS AS[Status]
                            , PTD.TransID
                            , PTD.TransTypeCode
                            , PTD.PrimeRxPayTransID AS[PrimeRxPayTransID]
                            , PTD.ProcessorTransID AS[ProcessorTransID]
                            , PTD.PaymentProcessor AS[PaymentProcessor]
                            , PTD.TicketNumber AS[TicketNo]
                            , PTH.IsPaymentPending AS [IsCustomerDriven]
                    FROM POSTransPayment_OnHold AS PTD WITH(NOLOCK)
                    INNER JOIN POSTransaction_OnHold AS PTH WITH(NOLOCK) ON PTH.TransID = PTD.TransID
                    INNER JOIN Customer AS CTS WITH(NOLOCK) ON PTH.CustomerID = CTS.CustomerID
                    WHERE PTD.[Status]= 'In Progress' and convert(datetime,PTH.TransDate,109) between convert(datetime, cast('" + DateTime.Now.AddDays((-1) * Configuration.CSetting.PrimeRxPayStatusUpdateFromLastDays).ToString("MM/dd/yyy") + " 00:00:00 ' as datetime) ,113) and convert(datetime, cast('" + DateTime.Now.ToString("MM/dd/yyy") + " 23:59:59 ' as datetime) ,113) ";

                oDataSet = DataHelper.ExecuteDataset(Configuration.ConnectionString, CommandType.Text, strSQL);

                if (oDataSet?.Tables.Count > 0 && oDataSet?.Tables[0].Rows.Count > 0)
                {
                    using (var db = new Possql())
                    {
                        objPaymentTypes = db.PayTypes.ToList();
                    }

                    for (int i = 0; i < oDataSet.Tables[0].Rows.Count; i++)
                    {
                        if (Convert.ToBoolean(oDataSet.Tables[0].Rows[i]["IsCustomerDriven"].ToString())
                                && !string.IsNullOrWhiteSpace(Convert.ToString(oDataSet.Tables[0].Rows[i]["PrimeRxPayTransID"]))
                                   && Convert.ToString(oDataSet.Tables[0].Rows[i]["Status"]).ToUpper() != clsPOSDBConstants.PosTransPayment_Status_Completed.ToUpper())
                        {
                            PrimeRxPayTransID.Add(Convert.ToInt64(oDataSet.Tables[0].Rows[i]["PrimeRxPayTransID"])); //PRIMEPOS-3333

                            if (paymentDate > Convert.ToDateTime(oDataSet.Tables[0].Rows[i]["TransDate"])) //PRIMEPOS-3453
                            {
                                paymentDate = Convert.ToDateTime(oDataSet.Tables[0].Rows[i]["TransDate"]);
                            }
                        }
                    }

                    if (PrimeRxPayTransID != null && PrimeRxPayTransID.Count>0)
                    {
                        TimeSpan timeSpan = DateTime.Now - paymentDate; //PRIMEPOS-3453
                        int lookUpDays = Convert.ToInt32(timeSpan.Days) + 5; //PRIMEPOS-3453
                        oPrimeRxPayResponses = oPrimeRxPay.GetMultipleTransactionDetail(Configuration.CSetting.PrimeRxPayUrl + Configuration.CSetting.PrimerxPayExtensionUrl, PrimeRxPayTransID, Configuration.CSetting.PayProviderID, Configuration.CSetting.PrimeRxPayClientId, Configuration.CSetting.PrimeRxPaySecretKey, lookUpDays);
                    }

                    for (int i = 0; i < oDataSet.Tables[0].Rows.Count; i++)
                    {
                        try
                        {
                            if (Convert.ToBoolean(oDataSet.Tables[0].Rows[i]["IsCustomerDriven"].ToString())
                                && !string.IsNullOrWhiteSpace(Convert.ToString(oDataSet.Tables[0].Rows[i]["PrimeRxPayTransID"]))
                                   && Convert.ToString(oDataSet.Tables[0].Rows[i]["Status"]).ToUpper() != clsPOSDBConstants.PosTransPayment_Status_Completed.ToUpper())
                            {
                                Application.DoEvents();

                                if (oPrimeRxPayResponses != null && oPrimeRxPayResponses.Count > 0)
                                {
                                    foreach (var oPrimeRxResp in oPrimeRxPayResponses)
                                    {
                                        cardType = string.Empty;
                                        if (oPrimeRxResp?.TransactionID == Convert.ToString(oDataSet.Tables[0].Rows[i]["PrimeRxPayTransID"]))
                                        {
                                            if (oPrimeRxResp != null && !string.IsNullOrWhiteSpace(oPrimeRxResp.AmountApproved))
                                            {
                                                if (Convert.ToDouble(oPrimeRxResp.AmountApproved) > 0 && oPrimeRxResp.Result == "SUCCESS")
                                                {
                                                    cardType = PccPaymentSvr.SetActualCardType(oPrimeRxResp.CardType);
                                                    paymentTypes = objPaymentTypes.Where(a => a.PayTypeDesc == cardType).SingleOrDefault();
                                                    if (paymentTypes != null)
                                                    {
                                                        cardType = paymentTypes.PayTypeID;
                                                    }
                                                    #region PRIMEPOS-3186
                                                    ProfiledID = string.Empty;
                                                    IsFSATransaction = false;
                                                    if (oPrimeRxResp.ProfiledID.Contains("|"))
                                                    {
                                                        profileIdAndFSA = oPrimeRxResp.ProfiledID.Split('|');
                                                        if (profileIdAndFSA.Length > 0)
                                                        {
                                                            ProfiledID = profileIdAndFSA[0];
                                                        }
                                                        if (profileIdAndFSA.Length > 1)
                                                        {
                                                            IsFSATransaction = profileIdAndFSA[1] == "Y";
                                                        }
                                                    }
                                                    if (!string.IsNullOrWhiteSpace(oPrimeRxResp.Expiration))
                                                    {
                                                        oPrimeRxResp.MaskedCardNo = oPrimeRxResp.MaskedCardNo + "|" + oPrimeRxResp.Expiration;
                                                    }

                                                    oPosTransPaymentSvr.SetPrimeRxPayPartialTrans(Convert.ToString(oDataSet.Tables[0].Rows[i]["TransPayId"]),
                                                    oPrimeRxResp.AmountApproved,
                                                    oPrimeRxResp.TransactionNo,
                                                    cardType,
                                                    oPrimeRxResp.MaskedCardNo,
                                                    ProfiledID,
                                                    GetLastDateOfMonthAndYear(oPrimeRxResp.Expiration),
                                                    IsFSATransaction);

                                                    #region PRIMEPOS-3344
                                                    using (var db = new Possql())
                                                    {
                                                        CCTransmission_Log cclog = new CCTransmission_Log();
                                                        cclog.TransDateTime = DateTime.Now;
                                                        cclog.TicketNo = Convert.ToString(oDataSet.Tables[0].Rows[i]["TicketNo"]);
                                                        cclog.TransAmount = Convert.ToDecimal(oPrimeRxResp.TransactionAmount);
                                                        cclog.AmtApproved = Convert.ToDecimal(oPrimeRxResp.AmountApproved);
                                                        cclog.TransDataStr = oPrimeRxResp.request;
                                                        cclog.RecDataStr = oPrimeRxResp.response;
                                                        cclog.PaymentProcessor = "PRIMERXPAY";
                                                        cclog.UserID = Configuration.UserName;
                                                        cclog.StationID = Configuration.StationID;
                                                        cclog.TransmissionStatus = "Completed";
                                                        cclog.HostTransID = oPrimeRxResp.EmvReceipt.TransactionID;
                                                        cclog.TransType = "PRIMERXPAY_CREDIT_SALE";
                                                        cclog.ResponseMessage = oPrimeRxResp.Result;
                                                        #region PRIMEPOS-3383
                                                        if (!string.IsNullOrWhiteSpace(oPrimeRxResp.AccountNo) && oPrimeRxResp.AccountNo.Trim().Length >= 4)
                                                        {
                                                            cclog.last4 = oPrimeRxResp.AccountNo.Trim().Substring(oPrimeRxResp.AccountNo.Trim().Length - 4, 4);
                                                        }
                                                        #endregion
                                                        db.CCTransmission_Logs.Add(cclog);
                                                        db.SaveChanges();
                                                        db.Entry(cclog).GetDatabaseValues();
                                                    }
                                                    #endregion

                                                    #endregion
                                                }
                                                else if(oPrimeRxResp.Result.Contains("EXPIRED"))
                                                {
                                                    oPosTransPaymentSvr.SetPrimeRxPayExpiredTrans(Convert.ToString(oDataSet.Tables[0].Rows[i]["TransPayId"]));
                                                }
                                                else
                                                {
                                                    oPosTransPaymentSvr.SetPrimeRxPayInProgressTrans(Convert.ToString(oDataSet.Tables[0].Rows[i]["TransPayId"]));
                                                }
                                            }
                                            else
                                            {
                                                logger.Error($"tmrPrimeRxPayBGStatusUpdate_Tick()===>PrimeRxPay Onhold transaction Status GetTransactionDetail() returned is null for PrimeRxPayTransID={Convert.ToString(oDataSet.Tables[0].Rows[i]["PrimeRxPayTransID"])}");
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        catch (Exception Ex)
                        {
                            logger.Error(Ex, $"tmrPrimeRxPayBGStatusUpdate_Tick()===>Exception occured on PrimeRxPay Onhold transaction Status update for PrimeRxPayTransID={ Convert.ToString(oDataSet.Tables[0].Rows[i]["PrimeRxPayTransID"])}");
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                logger.Error(Ex, $"tmrPrimeRxPayBGStatusUpdate_Tick()===>Exception occured on PrimeRxPay Onhold transaction Status update");
            }
            finally
            {
                oDataSet = null;
                tmrPrimeRxPayBGStatusUpdate.Start();
            }
        }

        /// <summary>
        /// PRIMEPOS-3187
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        private Nullable<DateTime> GetLastDateOfMonthAndYear(string expiryDate)
        {
            int noOfDaysInMonth;
            int year = 0;
            int month = 0;

            try
            {
                if (!string.IsNullOrWhiteSpace(expiryDate) && expiryDate.Length == 4)
                {
                    month = Convert.ToInt32(expiryDate.Substring(0, 2));
                    year = Convert.ToInt32(expiryDate.Substring(2, 2));
                }
                if (month > 0 && year > 0)
                {
                    noOfDaysInMonth = DateTime.DaysInMonth(year, month);
                    return Convert.ToDateTime($"{month}/{noOfDaysInMonth}/{year}");
                }
                return null;
            }
            catch
            {
                if (month > 0 && year > 0)
                {
                    noOfDaysInMonth = 28;
                    return Convert.ToDateTime($"{month}/{noOfDaysInMonth}/{year}");
                }
                return null;
            }
        }

        void bwCheckAlwaysRunning_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            //PRIMEPOS-2993 24-Aug-2021 JY Added logic for citrix
            bool isRemoteOrCitrix = false;
            string clientMachineName = string.Empty;
            int sessionId = 0;
            TerminalSessionInfo sessionDetail = null;

            try
            {
                MMSAppUpdater.clsUpdateManager oUpdateManger = new MMSAppUpdater.clsUpdateManager(Configuration.CInfo.AutoUpdateServiceAddress, Configuration.CInfo.StoreID, Configuration.ConnectionString, strAppName, Configuration.StationID);
                sessionId = Configuration.GetSessionId();
                if (sessionId > 0)
                {
                    sessionDetail = TermServicesManager.GetSessionInfo(Environment.MachineName, sessionId); //Server Name
                    clientMachineName = sessionDetail.ClientName;
                    isRemoteOrCitrix = true;
                }
                oUpdateManger.checkAlwaysRunningandMultiInstancing(isRemoteOrCitrix, clientMachineName);
            }
            catch (Exception ex)
            {
                //clsCoreUIHelper.ShowErrorMsg(ex.ToString());  //Sprint-21 - PRIMEPOS-2260 03-Feb-2016 JY Commented as it should not throw the exception if remote server connection timed out
                //POS_Core.ErrorLogging.Logs.Logger(ex.ToString());    //Sprint-21 - PRIMEPOS-2260 03-Feb-2016 JY Added to log exception in log file
                //logger.Fatal(ex, "bwCheckAlwaysRunning_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)"); //PRIMEPOS-2641 14-Feb-2019 JY Added //31-Dec-2019 JY Commented
            }
        }

        private void tmrCheckRunningTasks_Tick(object sender, EventArgs e)
        {
            try
            {
                tmrCheckRunningTasks.Enabled = false;

                //Added logic to set check runnings tasks interval, user can set it > 10
                if (POS_Core.Resources.Configuration.convertNullToInt(POS_Core.Resources.Configuration.CInfo.RunningTasksTimerInterval) > 10)
                    tmrCheckRunningTasks.Interval = POS_Core.Resources.Configuration.CInfo.RunningTasksTimerInterval * 60 * 1000;
                else
                    tmrCheckRunningTasks.Interval = 10 * 60 * 1000;//600000

                if (POS_Core.Resources.Configuration.CInfo.AllowRunningUpdates == true)
                {
                    bwCheckAlwaysRunning.RunWorkerAsync();
                }
            }
            catch (Exception ex)
            {
                //clsCoreUIHelper.ShowErrorMsg(ex.ToString());  //Sprint-21 03-Feb-2016 JY Commented as it should not throw the exception if remote server connection timed out
                //POS_Core.ErrorLogging.Logs.Logger(ex.ToString());    //Sprint-21 03-Feb-2016 JY Added to log exception in log file
                logger.Fatal(ex, "tmrCheckRunningTasks_Tick(object sender, EventArgs e)"); //PRIMEPOS-2641 14-Feb-2019 JY Added
            }
            finally
            {
                tmrCheckRunningTasks.Enabled = true;
            }
        }

        private void checkforUpdates()
        {
            try
            {
                MMSAppUpdater.clsUpdateManager oUpdateManger = new MMSAppUpdater.clsUpdateManager(Configuration.CInfo.AutoUpdateServiceAddress, Configuration.CInfo.StoreID, Configuration.ConnectionString, strAppName, Configuration.StationID);
                oUpdateManger.CheckforUpdatesDialog(this);
            }
            catch (Exception Ex)
            {
                logger.Fatal(Ex, "checkforUpdates()"); //PRIMEPOS-2641 14-Feb-2019 JY Added
            }
        }

        //IMP Ask khaled about service address - http://xpdps/mmsupdater/service.asmx
        private void ShowStationsSettings()
        {
            try
            {
                MMSAppUpdater.clsUpdateManager oUpdateManger = new MMSAppUpdater.clsUpdateManager("http://xpdps/mmsupdater/service.asmx", Configuration.CInfo.StoreID, Configuration.ConnectionString, strAppName, Configuration.StationID);
                oUpdateManger.ShowStationSettings(this);
            }
            catch (Exception Ex)
            {
                logger.Fatal(Ex, "ShowStationsSettings()"); //PRIMEPOS-2641 14-Feb-2019 JY Added
            }
        }
        #endregion

        #region PRIMEPOS-2034 07-Mar-2018 JY Added
        public static frmRptCouponReport RptCouponReport
        {
            get
            {
                if (ofrmRptCouponReport.IsDisposed)
                    ofrmRptCouponReport = new frmRptCouponReport();
                return ofrmRptCouponReport;
            }
        }
        #endregion

        #region PRIMERX-7688 12-Jul-2019 JY Added
        public static frmReconciliationDeliveryReport ReconciliationDeliveryReport
        {
            get
            {
                if (ofrmReconciliationDeliveryReport.IsDisposed)
                    ofrmReconciliationDeliveryReport = new frmReconciliationDeliveryReport();
                return ofrmReconciliationDeliveryReport;
            }
        }
        #endregion

        #region PRIMEPOS-2774 13-Jan-2020 JY Added
        public async static Task SolutranFileUpload(bool bUpdateNow)
        {
            POS_Core.ErrorLogging.Logs.Logger("frmMain", "SolutranFileUpload()", clsPOSDBConstants.Log_Entering);

            try
            {
                if (bUpdateNow)
                {
                    if (AllowFileUpload())
                        GenerateCSVAndUpload();
                }
                else
                {
                    if (Configuration.convertNullToString(Configuration.CSetting.S3LastUploadDateOnFTP) != "" && (Convert.ToDateTime(Configuration.CSetting.S3LastUploadDateOnFTP).ToShortDateString() != "1/1/1900"))
                    {
                        DateTime LastFileUploadDate = DateTime.MinValue;
                        LastFileUploadDate = Convert.ToDateTime(Configuration.CSetting.S3LastUploadDateOnFTP);
                        int interval = Configuration.convertNullToInt(Configuration.CSetting.S3Frequency);
                        if (interval == 0)
                            interval = Configuration.CSetting.S3Frequency = 365;

                        int nDateMismatch = DateTime.Compare(LastFileUploadDate.AddDays(interval), Convert.ToDateTime(DateTime.Now.ToShortDateString()));
                        if (nDateMismatch <= 0)
                        {
                            if (AllowFileUpload())
                                GenerateCSVAndUpload();
                        }
                    }
                    else
                    {
                        if (AllowFileUpload())
                            GenerateCSVAndUpload();
                    }
                }
                POS_Core.ErrorLogging.Logs.Logger("frmMain", "SolutranFileUpload()", clsPOSDBConstants.Log_Exiting);
            }
            catch (Exception ex)
            {
                Logs.Logger("frmMain", "SolutranFileUpload", clsPOSDBConstants.Log_Exception_Occured + ex.Message + ex.StackTrace.ToString());
            }
        }

        private static bool AllowFileUpload()
        {
            bool bAllow = false;

            if (Configuration.convertNullToString(Configuration.CSetting.S3UploadProcessDate) == "" || (Convert.ToDateTime(Configuration.CSetting.S3UploadProcessDate).ToShortDateString() == "1/1/1900"))
            {
                DateTime dt = DateTime.Now;
                string strSQL = "UPDATE SettingDetail SET FieldValue = '" + dt + "' WHERE FieldName = '" + clsPOSDBConstants.SettingDetail_S3UploadProcessDate + "'";
                DataHelper.ExecuteNonQuery(POS_Core.Resources.Configuration.ConnectionString, CommandType.Text, strSQL);
                Configuration.CSetting.S3UploadProcessDate = dt;
                bAllow = true;
            }
            else
            {
                TimeSpan ts = DateTime.Now - Configuration.CSetting.S3UploadProcessDate;
                if (ts.TotalMinutes > 5)
                {
                    string strSQL = "UPDATE SettingDetail SET FieldValue = '' WHERE FieldName = '" + clsPOSDBConstants.SettingDetail_S3UploadProcessDate + "'";
                    DataHelper.ExecuteNonQuery(POS_Core.Resources.Configuration.ConnectionString, CommandType.Text, strSQL);
                    Configuration.CSetting.S3UploadProcessDate = Convert.ToDateTime("1900-01-01 00:00:00.000");
                    bAllow = true;
                }
            }
            return bAllow;
        }

        private static void GenerateCSVAndUpload()
        {
            string strCSVFilePath = string.Empty, strCSVFileName = string.Empty;
            bool bStatus = GenerateCsvFile(ref strCSVFilePath, ref strCSVFileName);
            if (bStatus)
            {
                bool bFileUploadStatus = UploadFile(Configuration.CSetting.S3FTPURL, Configuration.CSetting.S3FTPUserId, Configuration.CSetting.S3FTPPassword, Configuration.CSetting.S3FTPPort, Configuration.CSetting.S3FTP, Configuration.CSetting.S3FTPFolderPath, strCSVFilePath, strCSVFileName);
                if (bFileUploadStatus)
                {
                    string strSQL = "UPDATE SettingDetail SET FieldValue = '" + DateTime.Now.ToShortDateString() + "' WHERE FieldName = '" + clsPOSDBConstants.SettingDetail_S3LastUploadDateOnFTP + "';" +
                                    " UPDATE SettingDetail SET FieldValue = '" + strCSVFileName + "' WHERE FieldName = '" + clsPOSDBConstants.SettingDetail_S3FileName + "';" +
                                    " UPDATE SettingDetail SET FieldValue = '' WHERE FieldName = '" + clsPOSDBConstants.SettingDetail_S3UploadProcessDate + "'";

                    DataHelper.ExecuteNonQuery(POS_Core.Resources.Configuration.ConnectionString, CommandType.Text, strSQL);
                    Configuration.CSetting.S3LastUploadDateOnFTP = Convert.ToDateTime(DateTime.Now.ToString("MM/dd/yyyy"));
                    Configuration.CSetting.S3FileName = strCSVFileName;
                    Configuration.CSetting.S3UploadProcessDate = Convert.ToDateTime("1900-01-01 00:00:00.000");
                    clsCoreUIHelper.ShowWarningMsg("Item file for solutran has been uploaded successfully.", "Item File upload status");
                }
                else
                {
                    logger.Trace("SolutranFileUpload() - GenerateCSVAndUpload() - upload file issue");
                }
                try
                {
                    if (File.Exists(strCSVFilePath))
                    {
                        File.Delete(strCSVFilePath);
                    }
                }
                catch { }
            }
            else
            {
                logger.Trace("SolutranFileUpload() - GenerateCSVAndUpload() - Generate csv file issue");
            }
        }

        private static bool GenerateCsvFile(ref string strCSVFilePath, ref string strCSVFileName)
        {
            bool bStatus = false;
            strCSVFilePath = string.Empty;
            try
            {
                using (ItemSvr oItemSvr = new ItemSvr())
                {
                    DataTable dt = oItemSvr.GetItemDetailsForS3();
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        string strStoreName = Configuration.CInfo.StoreName;
                        if (Configuration.CInfo.StoreName.Length > 10)
                        {
                            strStoreName = Configuration.CInfo.StoreName.Substring(0, 10);
                        }

                        strCSVFileName = strStoreName + "_" + Configuration.CInfo.S3Merchant + "_" + DateTime.Now.ToString("MMddyyyy") + ".csv";
                        strCSVFilePath = Path.Combine(Application.StartupPath, strCSVFileName);
                        bStatus = Configuration.ExportDataToCSV(dt, strCSVFilePath);
                    }
                }
            }
            catch (Exception Ex)
            {
                logger.Fatal(Ex, "GenerateCsvFile()");
                return false;
            }
            return bStatus;
        }

        private static bool UploadFile(string strFTPAddress, string strFTPUserId, string strFTPPassword, int nPort, bool bConnectionType, string strFTPFolderPath, string strCSVFilePath, string strCSVFileName)
        {
            bool retVal = false;
            if (bConnectionType == true)    //sFTP
            {
                POS_Core.ErrorLogging.Logs.Logger("frmMain", "UploadFile() - sFTP", clsPOSDBConstants.Log_Entering);
                retVal = SFTPCommunication.Send(strFTPAddress, strFTPUserId, strFTPPassword, nPort, strFTPFolderPath, strCSVFilePath, strCSVFileName);
            }
            else //FTP
            {
                POS_Core.ErrorLogging.Logs.Logger("frmMain", "UploadFile() - FTP", clsPOSDBConstants.Log_Entering);
                FtpClient ftp = null;
                try
                {
                    string[] arrFTPAddress = strFTPAddress.Split('/');
                    string strTempFTPFolderPath = strFTPAddress.Substring(arrFTPAddress[0].Trim().Length);
                    if (strTempFTPFolderPath.Trim().EndsWith("/"))
                        strTempFTPFolderPath = strTempFTPFolderPath.Substring(0, strTempFTPFolderPath.Length - 1);

                    ftp = new FtpClient();

                    ftp.FileTransferStatus -= new FileTransferStatusEventHandler(ftp_S3FileTransferStatus);   //make sure that event should not get added again and again
                    ftp.FileTransferStatus += new FileTransferStatusEventHandler(ftp_S3FileTransferStatus);

                    ftp.PassiveTransfer = false;
                    ftp.Connect(arrFTPAddress[0]);
                    ftp.Login(strFTPUserId, strFTPPassword);
                    if (strFTPFolderPath != "")
                    {
                        if (!strFTPFolderPath.StartsWith("/")) strFTPFolderPath = "/" + strFTPFolderPath;
                        ftp.ChangeCurrentFolder(strFTPFolderPath);
                    }
                    ftp.SendFile(strCSVFilePath, strCSVFileName);

                    while (!bFileUploadComplete)
                    {
                        Thread.Sleep(500);
                    }
                    bFileUploadComplete = false;
                    retVal = true;
                }
                catch (System.Exception ex1)
                {
                    POS_Core.ErrorLogging.Logs.Logger("frmMain", "UploadFile(): ", ex1.Message);
                }
                finally
                {
                    ftp.Disconnect();
                }
            }
            POS_Core.ErrorLogging.Logs.Logger("frmMain", "UploadFile()", clsPOSDBConstants.Log_Exiting);
            return retVal;
        }

        private static void ftp_S3FileTransferStatus(object sender, FileTransferStatusEventArgs e)
        {
            if (e.AllBytesTotal == e.AllBytesTransferred)
            {
                bFileUploadComplete = true;
            }
        }
        #endregion

        #region PRIMEPOS-2778 16-Jan-2020 JY Added
        private static void SetDefaultDepartment()
        {
            try
            {
                string strSQL = "SELECT COUNT(ItemID) FROM Item WHERE ISNULL(DepartmentID,0) = 0";
                DataSet ds = DataHelper.ExecuteDataset(Configuration.ConnectionString, CommandType.Text, strSQL);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    if (Configuration.convertNullToInt64(ds.Tables[0].Rows[0][0]) > 0)
                    {
                        strSQL = "UPDATE Item SET DepartmentID = " + Configuration.CInfo.DefaultDeptId + " WHERE ISNULL(DepartmentID,'') = ''";
                        DataHelper.ExecuteNonQuery(Configuration.ConnectionString, CommandType.Text, strSQL);
                    }
                }
            }
            catch (System.Exception ex)
            {
                POS_Core.ErrorLogging.Logs.Logger("frmMain", "SetDefaultDepartment(): ", ex.Message);
            }
        }
        #endregion
    }

    public class POSMenuItems
    {
        public const string Sales = "Sales";
        public const string SaleReturns = "Sale_Returns";
        public const string PayOut = "Pay_Out";
        public const string Customers = "Customers";
        public const string ViewStationClose = "ViewStationClose";
        public const string InventoryRecieved = "Inventory_Recieved";
        //public const string PurchaseOrder="Purchase_Order";
        public const string Items = "ItemFile";
        public const string Departments = "Departments";
        public const string TaxCodes = "Tax_Codes";
        public const string Vendors = "Vendors";
        public const string UserSetup = "UserSetUp";
        public const string UserGroup = "UserGroup";
        public const string FunctionKeys = "Function_Keys";
        public const string CloseStation = "Close_Station";
        public const string InventoryReports = "InventoryReports";
        public const string Preferences = "Preferences";
        public const string EndOfDay = "End_Of_Day";
        public const string Lock = "Lock";
        public const string Logoff = "Log_Off";
        public const string ItemCompanion = "ItemCompanion";
        public const string ViewPOSTrans = "ViewPOSTrans";
        public const string ItemFileListing = "ItemFileListing";
        public const string VendorFileListing = "VendorFileListing";
        public const string InventoryStatusReport = "InventoryStatusReport";
        public const string PhysicalInventorySheet = "PhysicalInventorySheet";
        public const string RInventoryReceived = "RInventoryReceived";
        public const string ItemReOrder = "ItemRe-Order";
        public const string DetailTransactionReport = "Detail Transaction Report";
        public const string SSalesReportByUID = "Summary Sales Report By User ID";
        public const string SSalesReportByItem = "Sales Report By Item";
        public const string SalesReportByDept = "Sales Report By Department";
        public const string SalesPseudoephedrineLogs = "SalesPseudoephedrineLogs"; //PRIMEPOS-3360
        public const string SalesReportByPayment = "Sales Report By Payment";
        public const string StationCloseSummary = "StationCloseSummary";
        public const string TopSellingProducts = "TopSellingProducts";
        public const string SaleAnalysis = "SaleAnalysis";
        public const string SalesTaxSummary = "Sales Tax Summary";
        public const string Physical_Inventory = "Physical_Inventory";
        public const string Phy_Inv_History = "Physical Inv. History";
        public const string saleProductivityReport = "ProductivityReport";
        public const string ViewEndOfDay = "ViewEndOfDay";
        public const string PurchaseOrderReport = "PurchaseOrderReport";
        public const string ItemLabelReport = "ItemLabelReport";
        //public const string ViewPurchaseOrder="ViewPurchaseOrder";
        public const string ViewInventoryRecieved = "ViewInventoryRecieved";
        //public const string POAcknowledgement="POAcknowledgement";
        public const string PriceUpdateManual = "ManualPriceUpdate";
        public const string PriceUpdateAuto = "AutoPriceUpdate";
        public const string MyStore = "MyStore";
        public const string PayoutDetails = "PayoutDetails";
        public const string SaleTaxControl = "SaleTaxControl";
        public const string ChangePassword = "ChangePassword";
        public const string InvTransType = "InvTransType";
        public const string ManageMessages = "ManageMessages";
        public const string SalesComparison = "Sales Comparison";
        public const string SalesSummaryByVendor = "SalesSummaryByVendor";

        public const string DeadStockReport = "DeadStockReport";
        public const string CostAnalysisReport = "Cost Analysis Report";
        public const string LastUpdate = "Last Update";

        public const string POAcknowledgement = "PO Acknowledgement";
        public const string ViewPurchaseOrder = "View Purchase Order";
        public const string PurchaseOrder = "Add/Edit Purchase Order";
        public const string ItemFile = "Items";
        public const string ManageTimesheet = "Manage TimeSheet";
        public const string TimesheetReport = "TimesheetReport";
        public const string CreateTimesheet = "CreateTimesheet";

        public const string IIASItemFileListing = "IIASItemFileListing";
        public const string PSEItemFileListing = "PSEItemFileListing";    //Sprint-25 - PRIMEPOS-2379 09-Feb-2017 JY Added
        public const string IIASTransSummary = "IIASTransSummary";
        public const string IIASPaymentTransaction = "IIASPaymentTransaction";
        public const string SalesByCustomer = "SalesByCustomer";
        public const string ItemMonitorCategory = "ItemMonitorCategory";
        public const string CustomerList = "CustomerList";
        public const string SalesByItemMonitoringCategory = "SalesByItemMonitoringCategory";
        public const string DeliveryListReport = "DeliveryListReport";
        public const string SalesByInsurance = "SalesByInsurance";
        public const string ItmeDescription = "ItmeDescription";
        //Following Added By Krishna on 02 June 2011
        public const string TransactionTime = "TransactionTime";
        //Till here Added by Krishna on 02 June 2011
        //Following Added By Krishna on 13 June 2011
        public const string ItemSaleHistoricalCompare = "ItemSaleHistoricalCompare";
        //Till here Added by Krishna on 13 June 2011
        //Following Added By Krishna on 15 June 2011
        public const string StnCloseCash = "StationCloseCash";
        //Till here Added by Krishna on 15 June 2011
        public const string SystemNote = "SystemNote";
        public const string PayOutCat = "PayOutCategory";
        public const string Coupon = "Coupon";

        #region Added for Report purpose
        public const string ItemListByVendor = "ItemList By Vendor";
        #endregion
        #region  Addedd By (SRT)Abhishek  Date : 4/02/2009

        public const string POViewPurchaseOrdPoPending = "POViewPurchaseOrdPoPending";
        public const string POViewPurchaseOrdPoQueued = "POViewPurchaseOrdPoQueued";
        public const string POAcknowledgementPoProcessed = "POAcknowledgementPoProcessed";

        public const string Submitted = "Submitted";
        public const string Queued = "Queued";
        public const string Acknowledged = "Acknowledged";
        public const string PoLogScreen = "PoLogScreen";
        public const string FormPOLogScreen = "frmPOLogScreen";
        public const string PrimePOSStatus = "PrimePOSStatus";
        public const string Purchase_Order = "Purchase_Order";

        public const string Expired = "Expired";
        public const string Error = "Error";
        public const string Overdue = "Overdue";
        public const string MaxAttempts = "MaxAttempts";
        public const string InComplete = "InComplete";
        public const string DeliveryReceived = "DeliveryReceived"; // added by atul 25-oct-2010

        public const string WarningMessages = "WarningMessages";
        #endregion

        public const string SalesComparisonByDept = "Sales Comparison By Dept.";
        public const string PointsRewardTier = "PointsRewardTier";
        public const string RegisterCLCards = "RegisterCLCards";
        //Added By Amit Date 18 July 2011
        public const string ItemSalesPerformance = "ItemSalesPerformance";
        //Added By Amit Date 19 Aug 2011
        public const string RxCheckout = "RxCheckout";
        public const string ManageNotes = "Manage Notes";//Added By Shitaljit(QuicSolv) on 11 oct 2011
        public const string ROADetails = "ROADetails";//Added By Shitaljit(QuicSolv) on 11 oct 2012
        public const string ItemComboPricing = "ItemComboPricing";
        public const string ViewPOSTransColorScheme = "ViewPOSTransColorScheme";//Added By Shitaljit(QuicSolv) on 16 Nov 2012
        public const string PayType = "PayType";//Added By Shitaljit(QuicSolv) on 28 Jan 2013
        public const string ViewCLSummary = "ViewCLSummary";
        public const string ProcessOnHoldPO = "ProcessOnHoldPO"; //Added By Shitaljit(QuicSolv) on 28 May 2014
        public const string EODSummary = "EODSummary"; //Sprint-21 - 1861 23-Jul-2015 JY Added
        public const string DepartmentList = "DepartmentList";  //Sprint-23 - PRIMEPOS-2292 05-Jul-2016 JY Added 
        public const string CouponReport = "CouponReport"; //PRIMEPOS-2034 05-Mar-2018 JY Added
        public const string OnHoldTransactionDetailReport = "OnHoldTransactionDetailReport";    //PRIMEPOS-2580 05-Sep-2018 JY Added
        public const string ViewStoreCreditSummary = "ViewStoreCreditSummary"; // PRIMEPOS-2747 - NileshJ - 20-Nov-2019 - StoreCredit
        public const string ProcessEvertecSettlement = "ProcessEvertecSettlement";//PRIMEPOS-2664 ADDED BY ARVIND EVERTEC
        public const string RecoveryTransaction = "RecoveryTransaction"; // PRIMEPOS-2761  NileshJ
        public const string RxRecoveryUpdate = "RxRecoveryUpdate"; // PRIMEPOS-2761  NileshJ

        public const string ViewAuditTrail = "ViewAuditTrail"; // PRIMEPOS-2808 
        public const string ViewNoSaleTransaction = "ViewNoSaleTransaction"; // PRIMEPOS-2808 
        public const string ScheduledTask = "ScheduledTask";   //PRIMEPOS-2485 16-Mar-2021 JY Added
        public const string CreditCardProfiles = "CreditCardProfiles";  //PRIMEPOS-3004 19-Oct-2021 JY Added
        public const string TransactionFee = "TransactionFee";  //PRIMEPOS-3116 11-Jul-2022 JY Added
        public const string TransactionFeeReport = "TransactionFeeReport";  //PRIMEPOS-3136 30-Aug-2022 JY Added
    }

    public enum LookandFeelStypeENUM
    {
        OfficeXP = 1,
        Office2000 = 2,
        Office2003 = 3,
        VisualStudio2005 = 4
    }
}