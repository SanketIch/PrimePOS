using Infragistics.Win;
using Infragistics.Win.UltraWinCalcManager;
using Infragistics.Win.UltraWinEditors;
using Infragistics.Win.UltraWinGrid;
using MMS.Device;
using NLog;
using PharmData;
using POS_Core.BusinessRules;
using POS_Core.CommonData;
using POS_Core.CommonData.Rows;
using POS_Core.CommonData.Tables;
using POS_Core.DataAccess;
using POS_Core.ErrorLogging;
using POS_Core_UI.Layout;
//using POS_Core_UI.Reports.ReportsUI;
//using POS_Core.DataAccess;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Timers;
using System.Windows.Forms;
using POS_Core.Resources;
using POS_Core_UI.Reports.ReportsUI;
using POS_Core.Resources.PaymentHandler;
using POS_Core.LabelHandler;
using POS_Core.LabelHandler.RxLabel;
using PrimeRx.Models;
using MMSInterfaces.Helpers;
using MMSInterfaceLib.Common;
using static MMSInterfaces.Helpers.EventHub;
using Evertech;
using Evertech.Data;
using Resources;
using System.Drawing;
using System.Collections.Generic;
using System.Threading.Tasks;
using POS_Core.Business_Tier.Hyphen;

namespace POS_Core_UI.UI
{
    public partial class frmPOSTransaction : frmTransactionLayout
    {
        private static ILogger logger = LogManager.GetCurrentClassLogger(); //m

        #region Declaration
        private POSTransPayment_CCLogList oPOSTransPayment_CCLogList = null;
        FunctionKeysRow oFKRow = null;
        private CLCardsRow oCLCardRow = null;
        signatureType oSignatureType = new signatureType();
        private const int _standardDistanceBetweenControls = 11;
        private const decimal Hundred = 100;
        public static decimal TransactionAmount = 0;
        public static decimal InvDicsValueToVerify = 0;
        private static bool DelFromGridFlag = false;
        private static bool isQtyChange = false;
        private static int ExtraQty = 0;
        //private static Thread MaxStnCloseCashLimitThread;
        frmPOSPayAuthNo ofrmPOSPayAuthNo = new frmPOSPayAuthNo();
        frmDiscountOptions oDiscountOptions;
        private POSTransaction oPOSTrans = new POSTransaction();
        //private Hashtable DrugClassInfoCapture = new Hashtable(); //PRIMEPOS-2547 20-Jul-2018 JY Commented
        private DataTable DrugClassInfoCapture = null;   //PRIMEPOS-2547 20-Jul-2018 JY Added
        ContextMenu RightClickMenu = new ContextMenu();
        frmUrlView frmUrl; //PRIMEPOS-3207
        private DataTable RxWithValidClass = null;
        private Form frmNumPad = null;
        System.Timers.Timer heartBeatTimer;
        System.Timers.Timer notRespondTimer;
        private static frmNumericPad oNumericPad = null;
        private bool isBatchWtihShippingItem = false;//2927
        private string PAX_DEVICE_ARIES8 = "HPSPAX_ARIES8"; // Amit HPSPAX_ARIES8 Add    //PRIMEPOS-2952
        private string PAX_DEVICE_PAX7 = "HPSPAX"; //PRIMEPOS-2952
        private string PAX_DEVICE_A920 = "HPSPAX_A920"; //PRIMEPOS-3146
        #region StoreCredit PRIMEPOS-2747 -NileshJ 
        private StoreCreditData oStoreCreditData = new StoreCreditData();
        #endregion
        #region PRIMEPOS-3065 10-Mar-2022 JY Added
        private bool bIsPatient = false;
        private string strDriversLicense = string.Empty;
        private DateTime DriversLicenseExpDate = DateTime.MinValue;
        #endregion

        public frmNumericPad NumericPad
        {
            get
            {
                if (oNumericPad == null)
                    oNumericPad = new frmNumericPad(this.Handle);
                else if (oNumericPad.IsDisposed)
                    oNumericPad = new frmNumericPad(this.Handle);

                return oNumericPad;
            }
        }
        private string sigType;
        public string SigType
        {
            get
            {
                return sigType;
            }
            set
            {
                sigType = value;
            }
        }
        private bool IsHundredPerInvDisc = false;
        private bool bPrintGiftRecpt = false;
        private bool isAddRow;
        private bool isAnyUnPickRx = false;
        private bool isDescriptionOverride;
        private bool bInItemCodeEvent = false;
        private bool AutoGirdfallFlag = false;
        private bool IsItemExist = false;
        private bool isItemInfoChanged = false;
        private bool isFetchRx = false;
        private bool bIsCopied = false;
        private bool bCaptureSignature = false;
        private bool aboutToOpenDrawer = false;
        private bool isItemadding = false;
        private bool m_threadStarted = false;
        private bool bItemMonitorInTrans = false;
        private bool isEditRow;
        private bool isTimerLocked = false;
        private bool isOnHoldTrans = false;
        private bool isDeliveryAndOnHoldTrans = false; //PRIMEPOS-3494
        private bool isCallofRetTrans = false;
        private bool isSearchUnPickCancel = false;
        private bool bContinuePayment = true;
        //private bool reDisplayID = false; //PRIMEPOS-2525 24-May-2018 JY Commented
        private bool IsOTCsignCancelled = false;
        private string TransStartDateTime = "";
        private string strDiscountPolicy = string.Empty;
        private string sSigPadTransID = "";
        private string CouponItemDesc = string.Empty;
        private Int64 CouponID = 0; //PRIMEPOS-2034 05-Mar-2018 JY Added
        private string sMode = string.Empty;
        private string CurrentFuncKey = string.Empty;
        private string strActionButton = string.Empty;
        private string inquiryId = string.Empty;
        private string pseTrxId = string.Empty;
        private string heartBeats;
        private string sSigPadTransIDLast = "";
        private int cntFuncKey = 24;
        private int MaxFuncKeyPosition = 24;
        private int MinFuncKeyPosition = 0;
        private int onHoldTransID = 0;
        private List<OnholdRxs> lstOnHoldRxs = new List<OnholdRxs>();   //PRIMEPOS-2639 27-Mar-2019 JY Added
        private int TransID = 0;
        private int CopyRxInReturnTrans = 0;
        private int taxoverflag = 0;
        private int lastX;
        private int lastY;
        private int heartBeatMSec;
        private int HeartBitCount = 0;
        private int tempPatNo = 0;
        private int tempCustId = -1;
        private int DefaultQTY;
        private decimal custDiscount = 0;
        private decimal trPrice;
        private decimal ChangeDue;
        private decimal GroupPrice = 0;
        float FontSize = 0;
        byte[] OTCSignDataBinary = null;

        //PRIMEPOS-2534 (Suraj) 31-may-18 Suraj - Added VFInactivityMonitor for ingenico lane close issue
        System.Timers.Timer VFInactivityTrackingTimer = null;
        //
        private Boolean bIsCustomerTokenExists = false; //PRIMEPOS-2611 13-Nov-2018 JY Added
        System.Timers.Timer tmrBlinking;    //PRIMEPOS-2611 15-Nov-2018 JY Added
        private long iBlinkCnt = 0;  //PRIMEPOS-2611 15-Nov-2018 JY Added
        DataTable dtRxOnHold = null;    //PRIMEPOS-2639 22-Feb-2019 JY Added
        DataTable dtRxOnHoldForPrimeRxPayment = null;    //PRIMEPOS-3248
        #region POSLITE -  NileshJ
        private string[] argsPass
        {
            get; set;
        }
        private bool isRxParameter = false;

        public string appName = string.Empty;
        public bool IsPOSLite = false;
        #endregion

        #region BatchDelivery - NileshJ - PRIMERX-7688 23-Sept-2019
        public bool isBatchDelivery = false; // Nilesh - BatchDelivery
        public decimal batchDelCopayAmount = 0;
        public string batchDelPatient = string.Empty;
        public string batchDelRxNo = string.Empty;
        public string batchDelNo = string.Empty;
        public DataTable dtRxDetailsData = new DataTable();
        public DataTable dtDelOrderData = new DataTable();
        private decimal alreadyPaidAmount = 0;
        public string allowUnPickedRX = string.Empty;
        public bool isLaunchedByDelivery = false;
        #endregion

        #region PRIMEPOS-3157 28-Nov-2022 JY Added
        private List<Image> loadedImages = new List<Image>();
        private int currentImageIndex;
        #endregion

        #endregion

        #region PRIMEPOS-2664 EVERTEC
        public PmtTxnResponse evertecResponse = null;
        #endregion

        AuditTrail oAuditTrail = new AuditTrail(); //PRIMEPOS-2808

        //2915{
        public bool IsCustomerDriven = false;
        public string PrimeRxPayTransID = string.Empty;
        //2915
        public decimal PendingAmount = 0;
        public bool IsRemoveOnHoldPayment = false;

        #region PRIMEPOS-2857 EVERTEC
        public string CityTaxAmount = string.Empty;
        public string StateTaxAmount = string.Empty;
        public string ReduceStateTaxAmount = string.Empty;
        public string ReduceCityTaxAmount = string.Empty;//PRIMEPOS-3099
        #endregion       

        public decimal TransFeeAmt = 0; //PRIMEPOS-3117 11-Jul-2022 JY Added

        public frmPOSTransaction()
        {
            InitializeComponent();
            SetRightClickMenu();
            PopulateFunctionKeys();
            //btnCreditCardProfile.Appearance.Image = Properties.Resources.CreditCard;//PRIMEPOS-2896
            //btnCreditCardProfile.Appearance.BackColor = Color.Black;//PRIMEPOS-2896
            this.Closing += new System.ComponentModel.CancelEventHandler(this.frmPOSSaleTransaction_Closing);
        }
        #region POSLITE -  NileshJ
        // Farman Add Paramterise Constructor
        public frmPOSTransaction(string[] args)
        {
            //System.Diagnostics.Debugger.Break();
            //System.Windows.Forms.MessageBox.Show("Hello");

            //appName = args[2].ToString();
            IsPOSLite = Convert.ToBoolean(args[1]);
            InitializeComponent();
            SetRightClickMenu();
            PopulateFunctionKeys();
            this.Closing += new System.ComponentModel.CancelEventHandler(this.frmPOSSaleTransaction_Closing);
            args = args.Where(w => w != args[0] && w != args[1] && w != args[3] && w != args[4]).ToArray();
            argsPass = args;
            isRxParameter = true;
            //if(IsPOSLite)
            //{
            //    Configuration.CInfo.AllowUnPickedRX = "1";
            //}
            //btnCreditCardProfile.Appearance.Image = Properties.Resources.CreditCard;//PRIMEPOS-2896
        }
        #endregion

        #region Batch Delivery  NileshJ - PRIMERX-7688 23-Sept-2019
        public void ReloadTransaction(string[] rxNo, bool isBatchDel, DataTable dtDeliveryRxDetails, decimal alreadyPaidAmt) //NileshJ - PRIMERX-7688 added to handle scenario when Delivery screen is loaded from the frmPOSTransaction screen
        {
            argsPass = rxNo;
            isBatchDelivery = isBatchDel;
            dtRxDetailsData = dtDeliveryRxDetails;
            alreadyPaidAmount = alreadyPaidAmt;
            frmPOSTerminal_Load(this, null);
        }

        public frmPOSTransaction(string[] rxNo, bool isBatchDel, DataTable dtDeliveryRxDetails, decimal alreadyPaidAmt) //NileshJ - PRIMERX-7688 added a new constructure which will be called if to be used in batch delivery mode.
        {
            InitializeComponent();
            SetRightClickMenu();
            PopulateFunctionKeys();
            this.Closing += new System.ComponentModel.CancelEventHandler(this.frmPOSSaleTransaction_Closing);
            argsPass = rxNo;
            isRxParameter = false;
            isBatchDelivery = isBatchDel;
            dtRxDetailsData = dtDeliveryRxDetails;
            alreadyPaidAmount = alreadyPaidAmt;
        }

        #endregion

        #region PRIMEPOS-2738 - NileshJ
        public bool isStrictReturn = Configuration.CSetting.StrictReturn;
        #endregion

        #region formEvent
        //done by sandeep
        private void frmPOSTransaction_Activated(object sender, System.EventArgs e)
        {
            try
            {
                if (frmNumPad != null)
                    frmNumPad.Visible = true;
                clsUIHelper.CurrentForm = this;
            }
            catch (Exception) { }
        }

        //done by sandeep
        private void frmPOSTransaction_Deactivate(object sender, System.EventArgs e)
        {
            try
            {
                if (frmNumPad != null)
                    frmNumPad.Visible = false;
                logger.Trace("frmPOSTransaction_Deactivate() - " + clsPOSDBConstants.Log_Exiting);
            }
            catch (Exception) { }
        }

        //done by sandeep
        private void frmPOSTransaction_Leave(object sender, System.EventArgs e)
        {
            try
            {
                if (frmNumPad != null)
                    frmNumPad.Visible = false;
            }
            catch (Exception) { }
        }

        //done by sandeep
        private void frmPOSTransaction_Shown(object sender, EventArgs e)
        {
            try
            {
                if (frmNumPad != null)
                    frmNumPad.Visible = true;
                clsUIHelper.CurrentForm = this;
                logger.Trace("frmPOSTransaction_Shown() - POSTransaction Screen Shown");
                SetFontSize(this, this.Width, this.Height, 1);
                AdjustWidthOfCell();
            }
            catch (Exception) { }
        }

        //done by sandeep
        private void frmPOSTerminal_Load(object sender, System.EventArgs e)
        {
            try
            {
                logger.Trace("frmPOSTerminal_Load() - " + clsPOSDBConstants.Log_Entering);
                InitiateTimers();
                ApplyGridFormat();

                SetNew(false);
                SetCalculation();
                oPOSTrans.countUnBilledRx = 0;
                AssignEventHandlerToTextEditor();
                this.WindowState = FormWindowState.Maximized;
                //this.pnlTransaction.Left=(this.Width-this.pnlTransaction.Width)/2;
                if (this.MdiParent != null)
                {
                    ((frmMain)this.MdiParent).ultraExplorerBar1.Visible = false;
                    ((frmMain)this.MdiParent).ultMenuBar.Visible = false;
                    ((frmMain)this.MdiParent).ultMenuBar.Enabled = false;
                    ((frmMain)this.MdiParent).ultraExplorerBar1.Enabled = false;
                    //Added by SRT(Abhshek) Date : 11/09/2009
                    //Added to hide status bar for vendor on POS transaction screen
                    ((frmMain)this.MdiParent).ultraStatusBarVendor.Visible = false;
                    ((frmMain)this.MdiParent).ultraStatusBarVendor.Enabled = false;
                    //End of Added by SRT(Abhshek) Date : 11/09/2009
                }
                if (Configuration.showNumPad == true)
                {
                    logger.Trace("frmPOSTerminal_Load - Setting Showing NumPad - " + clsPOSDBConstants.Log_Entering);
                    showNumPad();
                    clsUIHelper.SETACTIVEWINDOW(this.Handle);
                    txtItemCode.Focus();
                    m_threadStarted = true;
                    this.btnNumericPad.Text = "Hide Num Pad";
                    logger.Trace("frmPOSTerminal_Load - Setting Showing NumPad - " + clsPOSDBConstants.Log_Exiting);
                }

                //setting Item Pad
                HideFuncKeyLayout();

                if (UserPriviliges.IsUserHasPriviliges(UserPriviliges.Modules.AdminFunc.ID, UserPriviliges.Screens.ViewPOSTrans.ID) == false)
                {
                    this.txtCustomer.ButtonsRight["H"].Visible = false;
                }
                if (Configuration.CInfo.SaveCCToken == false)
                    this.txtCustomer.ButtonsRight["C"].Visible = false;   //Sprint-25 - PRIMEPOS-2371 22-Mar-2017 JY Added                
                btnCLControlPane.Visible = Configuration.CLoyaltyInfo.UseCustomerLoyalty;
                clsUIHelper.SETACTIVEWINDOW(frmMain.getInstance().Handle);
                Activate();
                #region Added for POSLITE
                if (isRxParameter == true)
                {

                    //CancelEventArgs CE = new CancelEventArgs(false);
                    //foreach (string arg in argsPass)
                    //{
                    string[] rxOrItemList = argsPass[0].Split(',');

                    #region PRIMEPOS-2949 29-Mar-2021 JY Added
                    if (rxOrItemList.Length > 0 && rxOrItemList[0].ToUpper().Contains("BT"))
                    {
                        string strPOSBatchCode = Configuration.convertNullToString(Configuration.CInfo.IntakeBatchCode).Trim().ToUpper();
                        if (strPOSBatchCode != "BT")
                        {
                            rxOrItemList[0] = rxOrItemList[0].Replace("BT", strPOSBatchCode);
                        }
                    }
                    #endregion

                    foreach (string rxOrItem in rxOrItemList)
                    {
                        if (rxOrItem != String.Empty)
                        {
                            txtItemCode.Text = rxOrItem;
                            SendKeys.Send("{ENTER}");
                            grdDetail.Focus();  //PRIMEPOS-2949 15-Mar-2021 JY Added
                            //txtItemCode_KeyDown((object)this.txtItemCode, new KeyEventArgs(Keys.Enter));
                        }
                    }
                    //}
                    isRxParameter = false;

                }
                #endregion
                #region BatchDelivery - NileshJ - PRIMERX-7688 23-Sept-2019
                if (isBatchDelivery)  //This logic is to have one by one Rx's selected to be entered ensuring all validations happen as per normal flow.
                {
                    foreach (string rxOrItem in argsPass)
                    {
                        if (rxOrItem != String.Empty)
                        {
                            txtItemCode.Text = rxOrItem;
                            SendKeys.Send("{ENTER}");
                        }
                    }
                }
                #endregion

                #region AuditTrail - NileshJ PRIMEPOS-2808 
                oAuditTrail.oAuditDataSet.Tables.Add(oAuditTrail.CreateAuditLogDatatable());
                #endregion

                logger.Trace("frmPOSTerminal_Load() - " + clsPOSDBConstants.Log_Exiting);
            }
            catch (Exception Ex)
            {
                logger.Fatal(Ex, "frmPOSTerminal_Load()");
                clsUIHelper.ShowErrorMsg(Ex.Message);
                this.Close();
            }
        }

        //done by sandeep
        private void frmPOSTransaction_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            try
            {
                logger.Trace("frmPOSTransaction_KeyDown() - " + clsPOSDBConstants.Log_Entering);

                if (e.KeyData == System.Windows.Forms.Keys.Enter && this.txtItemCode.ContainsFocus == false && this.txtDepartmentCode.ContainsFocus == false)
                {
                    #region 19-Jun-2015 JY Added
                    if (this.txtDescription.ContainsFocus == true && sMode == "S")
                        this.SelectNextControl(this.ActiveControl, true, true, true, true);
                    else if (this.txtDescription.ContainsFocus == false)
                        this.SelectNextControl(this.ActiveControl, true, true, true, true);
                    #endregion
                }

                if (e.KeyData == System.Windows.Forms.Keys.F4 && e.Control == false && e.Shift == false && e.Alt == false)
                {
                    if (this.txtCustomer.ContainsFocus == true)
                        this.SearchCustomer(false);
                    else if (this.txtItemCode.ContainsFocus == true)
                        this.SearchItem(this.txtItemCode.Text.Trim(), "");
                    else if (this.txtDescription.ContainsFocus == true)
                        this.SearchItem("", this.txtDescription.Text.Trim());
                    else if (this.cmbTaxCode.ContainsFocus == true)
                        this.SearchTaxCode();
                    else if (this.txtDepartmentCode.ContainsFocus == true)
                        this.SearchDeptCode();
                }
                if (e.KeyCode == Keys.F8 && e.Control == false && e.Shift == false && e.Alt == false)
                {
                    this.txtCustomer.Text = this.txtItemCode.Text;
                    SearchCustomer(false);//Search Costomer.
                    SetFocusBack();
                }

                // process the other function options
                bool othFuncProcessed = false;
                if (e.KeyCode == Keys.F3 && e.Control == false && e.Shift == false && e.Alt == false)
                {
                    this.tbTerminalActions("F3"); // Price/Inv. Check
                    othFuncProcessed = true;
                }
                if (e.Control && e.KeyCode == Keys.L && e.Shift == false && e.Alt == false)
                {
                    this.tbTerminalActions("Ctrl+L"); //Ctrl+L Lock Register,
                    othFuncProcessed = true;
                }
                //Ctrl+N No Sale
                if (e.Control && e.KeyCode == Keys.N && e.Shift == false && e.Alt == false)
                {
                    // no sale
                    this.tbTerminalActions("Ctrl+N");
                    othFuncProcessed = true;
                }
                if (othFuncProcessed)
                {
                    return;
                }

                #region strtbterminal

                if (this.mTerminal.Visible == true)
                {
                    //"F2=Returns, Esc Main Menu, F4=Recv. On Acct., Ctrl+O Payout";
                    if (e.KeyCode == Keys.F2 && e.Control == false && e.Shift == false && e.Alt == false)
                    {
                        this.tbTerminalActions("F2");
                    }
                    else if (e.KeyCode == Keys.F5 && e.Control == false && e.Shift == false && e.Alt == false)
                    {
                        this.tbTerminalActions("F5");
                        //recieve on account
                    }
                    else if (e.KeyCode == Keys.P && e.Control == true && e.Shift == false && e.Alt == false)
                    {
                        this.tbTerminalActions("Ctrl+P");
                        //recieve on account
                    }
                    else if (e.Control && e.KeyCode == Keys.O && e.Shift == false && e.Alt == false)
                    {
                        //payout
                        this.tbTerminalActions("Ctrl+O");
                    }
                    #region PRIMEPOS-2786 EBT BALANCE EVERTEC
                    else if (e.Control && e.KeyCode == Keys.E && e.Shift == false && e.Alt == false)
                    {
                        this.tbTerminalActions("Ctrl+E");
                    }
                    #endregion
                    else if (e.KeyCode == Keys.Escape && this.txtItemCode.ContainsFocus == true)
                    {
                        this.tbTerminalActions("Esc");
                    }
                    //Following Added by Krishna on 6 May 2011
                    else if (e.Control && e.KeyCode == Keys.C && e.Shift == false && e.Alt == false)
                    {
                        //following if is added by shitaljit to velidate user rignts on clearing grid.
                        if (this.grdDetail.Rows.Count > 0)
                        {
                            if (UserPriviliges.getPermission(UserPriviliges.Modules.POSTransaction.ID, UserPriviliges.Screens.POSTransaction.ID, UserPriviliges.Permissions.DeleteTrans.ID, UserPriviliges.Permissions.DeleteTrans.Name))
                            {
                                SetNew(false);
                            }
                        }
                    }
                    //Till here Added by Krishna
                    else if (e.KeyCode == Keys.S && e.Control == true && e.Shift == false && e.Alt == false)    //Sprint-27 - PRIMEPOS-2456 10-Oct-2017 JY Added
                    {
                        this.tbTerminalActions("Ctrl+S");
                    }
                }

                #endregion strtbterminal

                #region strtbterminalentry

                if (this.mTerminalEntry.Visible == true)
                {
                    //"F3=Price/Inv. Check, Ctrl+L = Lock Register, Ctrl+N No Sale";
                    if (e.KeyCode == Keys.F3 && e.Control == false && e.Shift == false && e.Alt == false)
                    {
                        tbTerminalEnteryActions("F3");
                    }
                    else if (e.KeyCode == Keys.F5 && e.Control == false && e.Shift == false && e.Alt == false)
                    {
                        tbTerminalEnteryActions("F5");
                    }
                    else if (e.Control && e.KeyCode == Keys.L && e.Shift == false && e.Alt == false)
                    {
                        tbTerminalEnteryActions("Ctrl+L");
                    }
                    else if (e.Control && e.KeyCode == Keys.N && e.Shift == false && e.Alt == false)
                    {
                        tbTerminalEnteryActions("Ctrl+N");
                    }
                    else if (e.KeyCode == Keys.H && e.Control == true && e.Shift == false && e.Alt == false)
                    {
                        tbTerminalEnteryActions("Ctrl+H");
                    }
                    //Added by Shitaljit on 6 May 2011
                    else if (e.KeyCode == Keys.T && e.Control == true && e.Shift == false && e.Alt == false)
                    {
                        tbTerminalEnteryActions("Ctrl+T");
                    }//Till here added by Shitaljit
                     //Following Added by Krishna on 6 May 2011
                    else if (e.KeyCode == Keys.C && e.Control == true && e.Shift == false && e.Alt == false)
                    {
                        //following if is added by shitaljit to velidate user rignts on clearing grid.
                        //Added By Shitaljit on 16 May 2012
                        if (this.grdDetail.Rows.Count > 0)
                        {
                            tbTerminalEnteryActions("Ctrl+C");
                        }
                    }
                    //Till here added by Krishna
                    //Added By shitaljit for scanning coupon
                    else if (e.KeyCode == Keys.F6 && e.Control == false && e.Shift == false && e.Alt == false)
                    {
                        if (this.grdDetail.Rows.Count > 0)
                        {
                            tbTerminalEnteryActions("F6");
                        }
                    }
                }

                #endregion strtbterminalentry

                #region Function Key Short Menu

                if (tblFunKeyMenu.Visible == true)
                {
                    if (e.KeyCode == Keys.F1 && e.Shift == false && e.Control == false && e.Alt == false)
                    {
                        BrowseFuncKey(FuncKeyBrowse.Home);
                        this.txtItemCode.Focus();
                    }
                    if (e.KeyCode == Keys.PageUp && e.Shift == false && e.Control == false && e.Alt == false && this.btnPrevious.Enabled == true)
                    {
                        BrowseFuncKey(FuncKeyBrowse.Backward);
                        this.txtItemCode.Focus();
                    }
                    if (e.KeyCode == Keys.F7 && e.Shift == false && e.Control == false && e.Alt == false)
                    {
                        FunKeyCommonOperations.AddKeys(oFKRow);
                        this.txtItemCode.Focus();
                    }
                    if (e.KeyCode == Keys.PageDown && e.Shift == false && e.Control == false && e.Alt == false && this.btnNext.Enabled == true)
                    {
                        BrowseFuncKey(FuncKeyBrowse.Forward);
                        this.txtItemCode.Focus();
                    }
                }

                #endregion Function Key Short Menu
            }
            catch (Exception Ex)
            {
                logger.Fatal(Ex, "frmPOSTransaction_KeyDown()");
            }
            logger.Trace("frmPOSTransaction_KeyDown() - " + clsPOSDBConstants.Log_Exiting);
        }

        //completed by sandeep
        private void frmPOSTransaction_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            try
            {
                logger.Trace("frmPOSTransaction_KeyUp() - " + clsPOSDBConstants.Log_Entering);
                if (e.KeyData == Keys.F10)
                {
                    if (this.txtItemCode.Text.Trim() != "")
                    {
                        if (Configuration.CPOSSet.RXCode.Trim() != "")
                        {
                            if (!this.txtItemCode.Text.ToUpper().Trim().EndsWith(Configuration.CPOSSet.RXCode.ToUpper().Trim()))
                            {
                                this.txtItemCode.Text = String.Concat(this.txtItemCode.Text, Configuration.CPOSSet.RXCode.Trim());
                            }
                        }
                        CancelEventArgs CE = new CancelEventArgs(false);
                        ItemBox_Validatiang((object)this.txtItemCode, CE);
                    }
                }
                else if (this.txtItemCode.ContainsFocus == true)
                {
                    FunctionKeys oFKeys = new FunctionKeys();
                    String FKey;

                    FKey = "";
                    if (e.Control == true)
                    {
                        FKey = "Ctrl+" + e.KeyData.ToString().Substring(0, e.KeyData.ToString().IndexOf(","));
                    }
                    else if (e.Shift == true)
                    {
                        FKey = "Shift+" + e.KeyData.ToString().Substring(0, e.KeyData.ToString().IndexOf(","));
                    }
                    else
                    {
                        FKey += e.KeyData.ToString();
                    }

                    FunctionKeysData oFKData = oFKeys.PopulateList(" where FunKey='" + FKey + "'");

                    if (oFKData.FunctionKeys.Rows.Count > 0)
                    {
                        if (oFKData.FunctionKeys.Rows[0][clsPOSDBConstants.FunctionKeys_Fld_Operation].ToString().Trim() != "")
                        {
                            if (oFKData.FunctionKeys.Rows[0][clsPOSDBConstants.FunctionKeys_Fld_FunctionType].ToString().Equals(clsPOSDBConstants.FunctionKeys_Type_Item))
                            {
                                string strKey = oFKData.FunctionKeys.Rows[0][clsPOSDBConstants.FunctionKeys_Fld_FunKey].ToString().Trim();
                                if (this.txtItemCode.Text.Trim() == "")
                                {
                                    this.txtItemCode.Text = oFKData.FunctionKeys.Rows[0][clsPOSDBConstants.FunctionKeys_Fld_Operation].ToString().Trim();
                                }
                                else if (clsUIHelper.isNumeric(this.txtItemCode.Text))
                                {
                                    this.txtItemCode.Text += "@" + oFKData.FunctionKeys.Rows[0][clsPOSDBConstants.FunctionKeys_Fld_Operation].ToString().Trim();
                                }
                                else if (strKey == "Shift+Q" || strKey == "Shift+W")//Added By shitaljit to resolve Shift Keys working when using with price.
                                {
                                    this.txtItemCode.Text = this.txtItemCode.Text.Substring(0, this.txtItemCode.Text.Length - 1);
                                    this.txtItemCode.Text += "@" + oFKData.FunctionKeys.Rows[0][clsPOSDBConstants.FunctionKeys_Fld_Operation].ToString().Trim();
                                }
                                else
                                {
                                    this.txtItemCode.Text += "@" + oFKData.FunctionKeys.Rows[0][clsPOSDBConstants.FunctionKeys_Fld_Operation].ToString().Trim();
                                }
                                CancelEventArgs CE = new CancelEventArgs(false);
                                ItemBox_Validatiang((object)this.txtItemCode, CE);

                            }
                            else
                            {
                                frmExtendedFuncKeys ofrmExtendedFuncKeys = new frmExtendedFuncKeys(FKey);
                                if (ofrmExtendedFuncKeys.LoadSubKeys(FKey))
                                {
                                    ofrmExtendedFuncKeys.parentForm = this;
                                    ofrmExtendedFuncKeys.StartPosition = FormStartPosition.CenterParent;
                                    ofrmExtendedFuncKeys.ShowDialog();
                                }
                            }
                            e.Handled = true;
                        }
                        else
                        {
                            this.txtItemCode.Text = string.Empty;
                        }
                    }
                }
                logger.Trace("frmPOSTransaction_KeyUp() - " + clsPOSDBConstants.Log_Exiting);
            }
            catch (Exception exp)
            {
                logger.Trace(exp, "frmPOSTransaction_KeyUp()");
            }
        }


        #endregion

        #region formMethod

        //completed by sandeep
        private void HideFuncKeyLayout()
        {
            logger.Trace("HideFuncKeyLayout - Setting Showing ItemPad - " + clsPOSDBConstants.Log_Entering);
            if (Configuration.showItemPad == true)
            {
                tblFuncKeyReg.Visible = true;
                btnItemPad.Text = "Hide Item Pad";
            }
            else
            {
                tblLeft.RowStyles[1].SizeType = SizeType.Absolute;
                tblLeft.RowStyles[1].Height = 0;

                tblLeft.RowStyles[0].SizeType = SizeType.Percent;
                tblLeft.RowStyles[0].Height = 100;
            }
            logger.Trace("HideFuncKeyLayout - Setting Showing ItemPad - " + clsPOSDBConstants.Log_Exiting);
        }

        //private void NewfrmPOSTransaction_Resize(object sender, EventArgs e)
        //{
        //    SetEnabledLoad(this);
        //    oPOSTrans.SetPrevScreenSize(Width);
        //}



        //completed by sandeep  check for font
        //public void SetEnabledLoad(Control control)
        //{
        //    //System.Windows.Forms.Form.siz

        //    foreach (Control child in control.Controls) {

        //        Font font = child.Font;
        //        int frmWidth = Width;

        //        if (child.Controls.Count > 0) {
        //            SetEnabledLoad(child);
        //            child.Font = new System.Drawing.Font(font.FontFamily, oPOSTrans.GetMetrics(frmWidth, font.Size),font.Style);

        //        } else {
        //            child.Font = new System.Drawing.Font(font.FontFamily, oPOSTrans.GetMetrics(frmWidth, font.Size), font.Style);
        //        }
        //        if(child.Tag == null || child.Tag.ToString() != "NOCOLOR") {

        //            if (child.GetType() == typeof(Infragistics.Win.Misc.UltraLabel)) {
        //                ((Infragistics.Win.Misc.UltraLabel)child).Appearance.ForeColor = MasterLayout.lableForeColor;
        //            }
        //            if (child.GetType() == typeof(Infragistics.Win.Misc.UltraButton)) {
        //                ((Infragistics.Win.Misc.UltraButton)child).Appearance.ForeColor = MasterLayout.btnForeColor;
        //            }
        //        }

        //    }
        //}

        //completed by sandeep
        private void AssignEventHandlerToTextEditor()
        {
            try
            {

                this.txtCustomer.AfterEnterEditMode += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
                this.txtCustomer.AfterExitEditMode += new System.EventHandler(clsUIHelper.AfterExitEditMode);

                //this.dtpTransDate.AfterEnterEditMode += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
                //this.dtpTransDate.AfterExitEditMode += new System.EventHandler(clsUIHelper.AfterExitEditMode);

                this.txtItemCode.AfterEnterEditMode += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
                this.txtItemCode.AfterExitEditMode += new System.EventHandler(clsUIHelper.AfterExitEditMode);

                this.txtDescription.AfterEnterEditMode += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
                this.txtDescription.AfterExitEditMode += new System.EventHandler(clsUIHelper.AfterExitEditMode);

                this.txtQty.AfterEnterEditMode += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
                this.txtQty.AfterExitEditMode += new System.EventHandler(clsUIHelper.AfterExitEditMode);

                this.txtUnitPrice.AfterEnterEditMode += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
                this.txtUnitPrice.AfterExitEditMode += new System.EventHandler(clsUIHelper.AfterExitEditMode);

                this.txtDiscount.AfterEnterEditMode += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
                this.txtDiscount.AfterExitEditMode += new System.EventHandler(clsUIHelper.AfterExitEditMode);

                this.cmbTaxCode.AfterEnterEditMode += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
                this.cmbTaxCode.AfterExitEditMode += new System.EventHandler(clsUIHelper.AfterExitEditMode);

                //Added by shitaljit(Quicksolv) on 3 May 2011
                this.txtDepartmentCode.AfterEnterEditMode += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
                this.txtDepartmentCode.AfterExitEditMode += new System.EventHandler(clsUIHelper.AfterExitEditMode);
                //Till here added by shitaljit
                logger.Trace("frmPOSTerminal_Load - Setting Enter/Exit Mode for controls - " + clsPOSDBConstants.Log_Exiting);
            }
            catch (Exception exp)
            {
                logger.Fatal(exp, "AssignEventHandlerToTextEditor()");
            }

        }

        //done by sandeep
        private void SetCalculation()
        {
            try
            {
                logger.Trace("frmPOSTerminal_Load - Setting calculation formula - " + clsPOSDBConstants.Log_Entering);

                CalcSettings oAutoCalc = null;
                // This was added by Manoj to set the calculation back to the old style. If the new calculation is needed use commented code below
                lblSubTotal2.Text = this.txtAmtSubTotal.Text; // SAJID PRIMEPOS-2794
                oAutoCalc = this.ultraCalcManager1.GetCalcSettings(this.txtAmtSubTotal);
                oAutoCalc.Alias = "SubTotal";
                oAutoCalc.PropertyName = "Text";
                oAutoCalc.Formula = "round(sum([//grdDetail/" + grdDetail.DisplayLayout.Bands[0].Key + "/" + clsPOSDBConstants.TransDetail_Fld_ExtendedPrice + "]),2)";
                lblTax2.Text = txtAmtTax.Text; // SAJID PRIMEPOS-2794
                oAutoCalc = this.ultraCalcManager1.GetCalcSettings(this.txtAmtTax);
                oAutoCalc.Alias = "TaxAmount";
                oAutoCalc.PropertyName = "Text";
                oAutoCalc.Formula = "Round(sum([//grdDetail/" + grdDetail.DisplayLayout.Bands[0].Key + "/" + clsPOSDBConstants.TransDetail_Fld_TaxAmount + "]),2)";

                CalcSettings calcSettings = null;

                //Set CalcSettings properties on TextBox1
                calcSettings = this.ultraCalcManager1.GetCalcSettings(this.lblInvDiscount);
                calcSettings.PropertyName = "Text";
                calcSettings.Alias = "invDiscount";
                calcSettings.TreatAsType = typeof(System.Decimal);
                calcSettings.ErrorValue = 1;

                lblDiscount2.Text = txtAmtDiscount.Text; // SAJID PRIMEPOS-2794
                oAutoCalc = this.ultraCalcManager1.GetCalcSettings(this.txtAmtDiscount);
                oAutoCalc.Alias = "Discount";
                oAutoCalc.PropertyName = "Text";
                oAutoCalc.Formula = "Round([//invDiscount]+sum([//grdDetail/" + grdDetail.DisplayLayout.Bands[0].Key + "/" + clsPOSDBConstants.TransDetail_Fld_Discount + "]),2)";

                lblTotalAmount2.Text = txtAmtTotal.Text; // SAJID PRIMEPOS-2794

                oAutoCalc = this.ultraCalcManager1.GetCalcSettings(this.txtAmtTotal);
                oAutoCalc.Alias = "Total";
                oAutoCalc.PropertyName = "Text";
                oAutoCalc.Formula = "Round(([//SubTotal]+[//TaxAmount]-[//Discount]),2)";

                logger.Trace("frmPOSTerminal_Load - Setting calculation formula - " + clsPOSDBConstants.Log_Exiting);
                logger.Trace("frmPOSTerminal_Load - Setting Enter/Exit Mode for controls - " + clsPOSDBConstants.Log_Entering);
            }
            catch (Exception exp)
            {
                logger.Fatal(exp, "SetCalculation()");

            }
        }

        //completed by sandeep
        //Change private to public for using this method for clear item from device on close button
        public void SetNew(bool showLogin)
        {
            try
            {
                logger.Trace("SetNew() - " + clsPOSDBConstants.Log_Entering);
                isStrictReturn = Configuration.CSetting.StrictReturn;//PRIMEPOS-2738 ADDED BY ARVIND
                txtLineItemCnt.Text = "Line Items = 0" + "          Total Quantity = 0";
                frmMain.TransactionRecordCount = grdDetail.Rows.Count;  //PRIMEPOS-1923 08-Aug-2018 JY Added

                MaxFuncKeyPosition = cntFuncKey;
                MinFuncKeyPosition = 0;
                TransFeeAmt = 0;//PRIMEPOS-3339
                Configuration.IsDiscOverridefromPOSTrans = false;

                heartBeatTimer.Enabled = true;
                bContinuePayment = true;
                isOnHoldTrans = false;

                IsHundredPerInvDisc = false;
                isCallofRetTrans = false;
                bPrintGiftRecpt = false;
                tempPatNo = 0;
                onHoldTransID = 0;
                lstOnHoldRxs = new List<OnholdRxs>(); //PRIMEPOS-2639 27-Mar-2019 JY Added
                TransID = 0;
                custDiscount = 0;
                OTCSignDataBinary = null;

                RxWithValidClass = null;
                DrugClassInfoCapture = null;    //PRIMEPOS-2547 23-Jul-2018 JY Added
                lstPatientNos.Clear();  //PRIMEPOS-2547 23-Jul-2018 JY Added

                oPOSTrans = new POSTransaction();

                oPOSTransPayment_CCLogList = new POSTransPayment_CCLogList();
                oSignatureType = new signatureType();

                grdDetail.DataSource = oPOSTrans.oTransDData;

                ApplyGridFormat();

                grdDetail.Refresh();

                ClearItemRow();
                EnabledDisableItemRow(true);
                changeStToolbars(TransactionStToolbars.strTBTerminal);
                setTransactionType(POS_Core.TransType.POSTransactionType.Sales);

                txtItemCode.Enabled = true;
                txtCustomer.Enabled = true;
                txtItemCode.Enabled = true;
                btnChangeQty.Enabled = true;
                txtDescription.Enabled = true;
                lblCouponDiscount.Text = string.Empty; //PRIMEPOS-2034 12-Mar-2018 JY Added
                txtItemCode.Focus();

                txtAmtTendered.Value = 0;
                txtAmtBalanceDue.Text = "0";
                txtAmtChangeDue.Text = "0";

                if (Configuration.CInfo.ShowTextPrediction == true)
                {
                    txtItemDescription.Text = "";
                    txtItemDescription.Visible = false;
                    txtDescription.Visible = true;
                }
                setDescriptionView();

                logger.Trace("SetNew() - About to Check if UseSigPad is True");
                if (Configuration.CPOSSet.UseSigPad == true)
                {
                    logger.Trace("SetNew() - Showing Transaction Screen on Sig Pad " + clsPOSDBConstants.Log_Entering);
                    sSigPadTransIDLast = sSigPadTransID;
                    sSigPadTransID = Configuration.StationID + DateTime.Now.ToString("ddMMyyyyhhssmm");
                    SigPadUtil.DefaultInstance.isFinishDevice = true;
                    SigPadUtil.DefaultInstance.StartTransaction(sSigPadTransID, sSigPadTransIDLast);
                    SigPadUtil.DefaultInstance.isFinishDevice = false;
                    logger.Trace("SetNew() - Showing Transaction Screen on Sig Pad " + clsPOSDBConstants.Log_Exiting);
                    //PRIMEPOS-2534 (Suraj) 31-may-18 Suraj - Added VFInactivityMonitor for ingenico lane close issue
                    StartVFInactivityMonitor(this);
                    //
                }
                logger.Trace("SetNew() - About to Enter Clear()");
                Clear(showLogin);
                txtCustomer.Text = "-1";
                tempCustId = -1;
                logger.Trace("SetNew() - Successfully exit Clear()");
                logger.Trace("SetNew() - About to enter SearchCustomer()");
                GetCustomer(this.txtCustomer.Text, true);
                logger.Trace("SetNew() - Successfully exit SearchCustomer()");
                EnabledDisableItemRow(false);
                this.txtDescription.Enabled = true; //PRIMEPOS-2707 05-Jul-2019 JY Added
                this.lblCouponDiscount.Text = string.Empty; //PRIMEPOS-2034 12-Mar-2018 JY Added
                bCaptureSignature = false;  //PRIMEPOS-2605 27-Oct-2018 JY Added
                #region BatchDelivery  - NileshJ - PRIMERX-7688 23-Sept-2019 
                if (Configuration.CInfo.isPrimeDeliveryReconciliation == false)
                {
                    btnBatchDel.Visible = false;
                }
                else
                {
                    btnBatchDel.Visible = true;
                }
                //reset BatchDelivery related variables 
                batchDelCopayAmount = 0;
                batchDelPatient = string.Empty;
                batchDelRxNo = string.Empty;
                batchDelNo = string.Empty;
                #endregion

                #region PRIMEPOS-2794 - NileshJ
                if (txtCustomer.Text.Trim() == "-1" && !Configuration.CSetting.EnableCustomerEngagement)
                {
                    rightTabPayCust.Tabs[1].Visible = false;
                }
                else
                {
                    rightTabPayCust.Tabs[1].Visible = true;
                }
                #endregion

                #region PRIMEPOS-3157 28-Nov-2022 JY Added
                if (Configuration.CPOSSet.UsePrimeRX == false || Configuration.CSetting.PatientsSubCategories.Trim() == "")
                {
                    rightTabPayCust.Tabs[2].Visible = false;
                }
                else
                {
                    rightTabPayCust.Tabs[2].Visible = true;
                }
                #endregion

                //2927
                isBatchWtihShippingItem = false;
                IsCustomerDriven = false;//2915
                PendingAmount = 0;//2915
                #region PRIMEPOS-3065 10-Mar-2022 JY Added
                bIsPatient = false;
                strDriversLicense = "";
                DriversLicenseExpDate = DateTime.MinValue;
                #endregion
                logger.Trace("SetNew() - " + clsPOSDBConstants.Log_Exiting);
            }
            catch (Exception exp)
            {
                logger.Fatal(exp, "SetNew()");
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }

        //done by sandeep
        public void OnNotRespontTimedEvent(object source, ElapsedEventArgs e)
        {
            try
            {
                //logger.Trace("OnNotRespontTimedEvent() - System is not responing for 60 seconds");
                if (Configuration.ProcessStatus("POS") == false)
                {
                    notRespondTimer.Enabled = false;
                    System.Data.SqlClient.SqlConnection.ClearAllPools();
                    //logger.Trace("OnNotRespontTimedEvent() - ClearAllPools as PrimePOS is not responding from last 60 seconds");
                }
            }
            catch (Exception Ex)
            {
                logger.Fatal(Ex, "OnNotRespontTimedEvent() - System is not responing for 60 seconds");
            }
        }

        //done by sandeep
        public void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            try
            {
                //logger.Trace("OnTimedEvent() - Time elapse in second :" + (HeartBitCount * heartBeatMSec) / 1000);
                if (isTimerLocked == true || notRespondTimer.Enabled == true)
                {
                    HeartBitCount++; //Added by Manoj 4-10-2015
                    return;
                }
                HeartBitCount++;
                isTimerLocked = true;
                if (Configuration.ProcessStatus("POS") == false)
                {
                    //logger.Trace("OnTimedEvent() - Time elapse in second :" + (HeartBitCount * heartBeatMSec) / 1000 + "System is not responding.");
                    isTimerLocked = false;
                    System.Data.SqlClient.SqlConnection.ClearAllPools();
                    //logger.Trace("OnTimedEvent() - ClearAllPools as PrimePOS is not responding");
                    notRespondTimer.Enabled = true;
                }
                else
                {
                    isTimerLocked = false;
                }
            }
            catch (Exception Ex)
            {
                isTimerLocked = false;
                logger.Fatal(Ex, "OnTimedEvent() - Exception Occured Time elapse in second :" + (HeartBitCount * heartBeatMSec) / 100 + Ex.StackTrace.ToString());
            }
        }

        //done by sandeep
        public void InitiateTimers()
        {
            logger.Trace("InitiateTimers() - " + clsPOSDBConstants.Log_Entering);
            //Here HeartBeat Frequency Is Converted In Milliseconds.
            heartBeats = "10";
            heartBeatMSec = Convert.ToInt32(heartBeats) * 1000;
            //Initialisation
            heartBeatTimer = new System.Timers.Timer();
            heartBeatTimer.Interval = heartBeatMSec;
            heartBeatTimer.Enabled = false;
            heartBeatTimer.Elapsed += new ElapsedEventHandler(OnTimedEvent);

            //Initialisation
            notRespondTimer = new System.Timers.Timer();
            notRespondTimer.Interval = 60 * 1000;//60 seconds timeout..
            notRespondTimer.Enabled = false;
            notRespondTimer.Elapsed += new ElapsedEventHandler(OnNotRespontTimedEvent);

            #region PRIMEPOS-2611 15-Nov-2018 JY Added
            tmrBlinking = new System.Timers.Timer();
            tmrBlinking.Interval = 1000;//1 seconds
            tmrBlinking.Elapsed += new ElapsedEventHandler(tmrBlinkingTimedEvent);
            tmrBlinking.Enabled = false;
            #endregion

            logger.Trace("InitiateTimers() - " + clsPOSDBConstants.Log_Exiting);
        }

        public void tmrBlinkingTimedEvent(object source, ElapsedEventArgs e)
        {
            try
            {
                iBlinkCnt++;
                if (iBlinkCnt % 2 == 0)
                    lblCustomerName.Appearance.ForeColor = Color.Transparent;
                else
                    lblCustomerName.Appearance.ForeColor = Color.White;
            }
            catch (Exception Ex)
            {
                logger.Fatal(Ex, "tmrBlinkingTimedEvent(object source, ElapsedEventArgs e)");
            }
        }

        //completed by sandeep
        private void SetFocusBack()
        {
            // this  method sets focus back after one of the Other Function buttons is pressed
            if (this.mTerminal.Visible)
                this.grdDetail.Focus();
            else
            {
                if (txtItemCode.Enabled)
                    this.txtItemCode.Focus();
                else
                    this.grdDetail.Focus();
            }
        }

        //completed by sandeep
        private void frmPOSSaleTransaction_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                logger.Trace("frmPOSSaleTransaction_Closing() - " + clsPOSDBConstants.Log_Entering);
                if (this.MdiParent != null)
                {
                    ((frmMain)this.MdiParent).ultMenuBar.Visible = true;
                    ((frmMain)this.MdiParent).ultMenuBar.Enabled = true;
                    ((frmMain)this.MdiParent).ultraExplorerBar1.Enabled = true;
                    //Added by SRT(Abhshek) Date : 11/09/2009
                    //Added to show status bar for vendor after POS Transaction screen is closed
                    if (Configuration.CPrimeEDISetting.UsePrimePO == true)  //PRIMEPOS-3167 07-Nov-2022 JY Modified
                    {
                        ((frmMain)this.MdiParent).ultraStatusBarVendor.Visible = true;
                        ((frmMain)this.MdiParent).ultraStatusBarVendor.Enabled = true;
                    }
                    //End of Added by SRT(Abhshek) Date : 11/09/2009
                    clsUIHelper.ShowWelcomeMessage();
                }

                //if (frmNumPad != null) frmNumPad.Close(); //PRIMEPOS-1923 13-Aug-2018 JY Commented
                m_threadStarted = false;    //PRIMEPOS-1923 13-Aug-2018 JY Added to shut numeric pad when we click on "X" to close the application, but later clicked on "Cancel"    
                if (grdDetail.Rows.Count == 0)
                    Configuration.IsWelComeScreen = true;
                else
                    Configuration.IsWelComeScreen = false;
            }
            catch (Exception exp)
            {
                logger.Fatal(exp, "frmPOSSaleTransaction_Closing()");
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
            logger.Trace("frmPOSSaleTransaction_Closing() - " + clsPOSDBConstants.Log_Exiting);
        }

        private void frmPOSTransaction_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (frmNumPad != null)
                frmNumPad.Close();   //PRIMEPOS-1923 13-Aug-2018 JY Added 
        }

        //completed by sandeep
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




        //done by sandeep
        public void ClearAll()
        {
            try
            {
                oPOSTrans = new POSTransaction();
                this.grdDetail.DataSource = oPOSTrans.oTransDData;
                ApplyGridFormat();
                this.grdDetail.Refresh();
                this.ClearItemRow();
                this.EnabledDisableItemRow(true);
                this.txtItemCode.Enabled = true;
                this.changeStToolbars(TransactionStToolbars.strTBTerminal);
                this.txtAmtTendered.Value = 0;
                this.txtAmtBalanceDue.Text = "0";
                this.txtAmtChangeDue.Text = "0";
                this.setTransactionType(POS_Core.TransType.POSTransactionType.Sales);
                this.txtItemCode.Enabled = true;
                if (Configuration.CPOSSet.UseSigPad == true)
                {
                    sSigPadTransIDLast = sSigPadTransID;
                    sSigPadTransID = Configuration.StationID + DateTime.Now.ToString("ddMMyyyyhhssmm");
                    SigPadUtil.DefaultInstance.isFinishDevice = true;
                    SigPadUtil.DefaultInstance.StartTransaction(sSigPadTransID, sSigPadTransIDLast);
                    SigPadUtil.DefaultInstance.isFinishDevice = false;
                }

                //Added By Manoj to Clear the Drug Class if the user click clear 4/3/2014
                if (RxWithValidClass != null)
                {
                    RxWithValidClass = null;
                    DrugClassInfoCapture = null;    //PRIMEPOS-2547 23-Jul-2018 JY Added
                    lstPatientNos.Clear();  //PRIMEPOS-2547 23-Jul-2018 JY Added
                }
                this.oCLCardRow = null;
                this.lblCouponDiscount.Text = string.Empty; //PRIMEPOS-2034 12-Mar-2018 JY Added
                this.txtItemCode.Focus();
                tempCustId = -1;
            }
            catch (Exception exp)
            {
                logger.Fatal(exp, "ClearAll()");
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }

        //completed by sandeep
        private void changeStToolbars(TransactionStToolbars oTbr)
        {
            try
            {
                if (oTbr == TransactionStToolbars.strTBTerminal)
                {
                    mTerminalEntry.Dock = DockStyle.None;
                    mTerminalEntry.Visible = false;
                    mGrpEditOptions.Dock = DockStyle.None;
                    mGrpEditOptions.Visible = false;
                    mTerminal.Dock = DockStyle.Fill;
                    mTerminal.Visible = true;
                    SetRowWidth(mTerminal, 2);
                    #region PRIMEPOS-2786 EVERTEC
                    if (Configuration.CPOSSet.PaymentProcessor.ToUpper() != "EVERTEC")
                    {
                        btnEBTBalance.Visible = false;
                        lblEBTBalance.Visible = false;
                    }
                    else
                    {
                        tblEbtBalance.Visible = true;
                        lblEBTBalance.Visible = true;
                        btnEBTBalance.Visible = true;
                    }
                    #endregion
                }
                else if (oTbr == TransactionStToolbars.strTBEditItem)
                {
                    mTerminal.Dock = DockStyle.None;
                    mTerminal.Visible = false;
                    mTerminalEntry.Dock = DockStyle.None;
                    mTerminalEntry.Visible = false;

                    mGrpEditOptions.Dock = DockStyle.Fill;
                    mGrpEditOptions.Visible = true;
                    SetRowWidth(mTerminal, 0);
                }
                else if (oTbr == TransactionStToolbars.strTBTerminalEntery)
                {

                    mTerminal.Dock = DockStyle.None;
                    mTerminal.Visible = false;

                    mGrpEditOptions.Dock = DockStyle.None;
                    mGrpEditOptions.Visible = false;

                    mTerminalEntry.Dock = DockStyle.Fill;
                    mTerminalEntry.Visible = true;
                    SetRowWidth(mTerminal, 1);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //completed by sandeep
        private void SetRowWidth(TableLayoutPanel tblLayout, int cellPosition)
        {

            for (int cntcol = 0; cntcol <= 2; cntcol++)
            {
                if (cntcol != cellPosition)
                {
                    tblOperationKey.RowStyles[cntcol].SizeType = SizeType.Absolute;
                    tblOperationKey.RowStyles[cntcol].Height = 0;
                }
            }
            tblOperationKey.RowStyles[cellPosition].SizeType = SizeType.Percent;
            tblOperationKey.RowStyles[cellPosition].Height = 100;

        }


        //done by sandeep
        public void setTransactionType(POS_Core.TransType.POSTransactionType oType)
        {
            try
            {
                logger.Trace("setTransactionType() - " + clsPOSDBConstants.Log_Entering);
                if (this.grdDetail.Rows.Count > 0)
                {
                    ErrorHandler.throwCustomError(POSErrorENUM.TransHeader_CanNotChangeTransactionType);
                }
                else
                {
                    oPOSTrans.CurrentTransactionType = oType;
                    if (oPOSTrans.CurrentTransactionType == POS_Core.TransType.POSTransactionType.Sales)
                    {
                        this.ultraGroupBox1.Text = "Sale Transaction";
                        ultraGroupBox1.HeaderAppearance.ForeColor = Color.Black;
                        this.Text = "Sale Transaction";
                        this.txtQty.MinValue = -9999;
                        this.txtQty.MaxValue = 9999;
                        DefaultQTY = 1;
                        btnReturn.Text = "Return";
                    }
                    else
                    {
                        if (UserPriviliges.getPermission(UserPriviliges.Modules.POSTransaction.ID, UserPriviliges.Screens.POSTransaction.ID, UserPriviliges.Permissions.ReturnTransaction.ID, UserPriviliges.Permissions.ReturnTransaction.Name))
                        {
                            this.ultraGroupBox1.Text = "Return Transaction";
                            ultraGroupBox1.HeaderAppearance.ForeColor = Color.Red;
                            this.Text = "Return Transaction";
                            this.txtQty.MinValue = -9999;
                            this.txtQty.MaxValue = 0;
                            DefaultQTY = int.Parse("-1");
                            btnReturn.Text = "Sales";

                            btnChangeQty.Enabled = false;//Added by krishna on 11 July 2011
                        }
                        else
                        {
                            this.ultraGroupBox1.Text = "Sale Transaction";
                            ultraGroupBox1.HeaderAppearance.ForeColor = Color.Black;
                            this.Text = "Sale Transaction";
                            this.txtQty.MinValue = -9999;
                            this.txtQty.MaxValue = 9999;
                            DefaultQTY = 1;
                            btnReturn.Text = "Return";
                            ClearAll();
                        }
                    }
                    changeStToolbars(TransactionStToolbars.strTBTerminal);
                    txtQty.Value = DefaultQTY;
                    txtQty.Refresh();
                }
                logger.Trace("setTransactionType() - " + clsPOSDBConstants.Log_Exiting);
            }
            catch (POSExceptions Ex)
            {
                logger.Fatal(Ex, "setTransactionType()");
                clsUIHelper.ShowErrorMsg(Ex.Message);
            }
        }

        //completed by sandeep
        private void Clear(bool showLogin)
        {
            try
            {
                logger.Trace("Clear() - " + clsPOSDBConstants.Log_Entering);
                txtCustomer.Text = String.Empty;
                dtpTransDate.Value = System.DateTime.Now;
                lblCustomerName.Text = String.Empty;
                bIsCustomerTokenExists = false; //PRIMEPOS-2611 13-Nov-2018 JY Added
                lblCustomerName.Appearance.Image = null; //PRIMEPOS-2611 13-Nov-2018 JY Added 
                oPOSTrans.oCustomerRow = null;
                oCLCardRow = null;

                txtAmtChangeDue.Text = "0";
                txtAmtDiscount.Text = "0";
                txtAmtTax.Text = "0";
                txtAmtSubTotal.Text = "0";
                txtAmtTotal.Text = "0";
                txtAmtTendered.Text = "0";
                lblInvDiscount.Text = "0";

                onHoldTransID = 0;
                lstOnHoldRxs = new List<OnholdRxs>(); //PRIMEPOS-2639 27-Mar-2019 JY Added

                if (Configuration.CPOSSet.LoginBeforeTrans == true && showLogin == true)
                {
                    logger.Trace("Clear() - About to lockstation");
                    clsUIHelper.LocakStation();
                    logger.Trace("Clear() - Finish lockstation");
                }
                // SAJID PRIMEPOS-2794
                lblDiscount2.Text = "0";
                lblTax2.Text = "0";
                lblSubTotal2.Text = "0";
                lblTotalAmount2.Text = "0";

                #region AuditTrail - NileshJ PRIMEPOS-2808 
                oAuditTrail.oAuditDataSet.Tables.Clear();
                oAuditTrail.oAuditDataSet.Tables.Add(oAuditTrail.CreateAuditLogDatatable());
                #endregion

                loadedImages.Clear();   //PRIMEPOS-3157 28-Nov-2022 JY Added
                pbImage.Image = null;   //PRIMEPOS-3157 28-Nov-2022 JY Added
                logger.Trace("Clear() - " + clsPOSDBConstants.Log_Exiting);
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "Clear() \n>>>>Error: " + ex.StackTrace.ToString(), clsPOSDBConstants.Log_Exception_Occured);
            }
        }

        #endregion

        #region gridEvent

        //done by sandeep
        private void grdDetail_Enter(object sender, System.EventArgs e)
        {
            try
            {
                if (this.grdDetail.Rows.Count > 0)
                {
                    this.changeStToolbars(TransactionStToolbars.strTBEditItem);
                    if (this.grdDetail.Selected.Rows.Count > 0)
                    {
                        this.grdDetail.ActiveRow = this.grdDetail.Selected.Rows[0];
                    }
                    else if (this.grdDetail.ActiveRow == null)
                    {
                        this.grdDetail.ActiveRow = this.grdDetail.Rows[0];
                    }
                    this.grdDetail.ActiveRow.Selected = true;
                }
                else
                    this.txtItemCode.Focus();
            }
            catch (Exception exp)
            {
                logger.Fatal(exp, "grdDetail_Enter()");
            }
        }
        //done by sandeep
        private void grdDetail_AfterRowActivate(object sender, EventArgs e)
        {
            string ItemCode = Configuration.convertNullToString(this.grdDetail.ActiveRow.Cells[clsPOSDBConstants.TransDetail_Fld_ItemID].Value);
            int RoeorderLevel = 0;
            int QtyInHand = 0;
            if (oPOSTrans.IsInventoryLow(ItemCode, out RoeorderLevel, out QtyInHand) == true)
            {
                this.grdDetail.ActiveRow.Cells[clsPOSDBConstants.TransDetail_Fld_ItemDescription].Appearance.ForeColor = Color.Red;
                this.grdDetail.ActiveRow.Cells[clsPOSDBConstants.TransDetail_Fld_ItemID].Appearance.ForeColor = Color.Red;
            }
            this.grdDetail.ActiveRow.Cells[clsPOSDBConstants.TransDetail_Fld_ItemDescriptionMasked].Value = MaskDrugName(this.grdDetail.ActiveRow.Cells[clsPOSDBConstants.ItemComboPricingDetail_Fld_ItemID].Value.ToString(), this.grdDetail.ActiveRow.Cells[clsPOSDBConstants.TransDetail_Fld_ItemDescription].Value.ToString());  //PRIMEPOS-3130
        }
        //done by sandeep
        private void grdDetail_Leave(object sender, System.EventArgs e)
        {
            try
            {
                //Application.DoEvents();
                this.grdDetail.Refresh();
                //if (this.grdDetail.Rows.Count > 0) {
                //    this.changeStToolbars(TransactionStToolbars.strTBTerminalEntery);
                //} else {
                //    this.changeStToolbars(TransactionStToolbars.strTBTerminal);
                //}

            }
            catch (Exception exp)
            {
                logger.Fatal(exp, "grdDetail_Leave()");
            }
        }
        //done by sandeep
        private void grdDetail_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            try
            {
                if (this.grdDetail.ActiveRow != null)
                {
                    if (e.KeyData == Keys.Q)
                    {
                        tbEditItemActions("Q");
                    }
                    else if (e.KeyData == Keys.N)
                    {
                        tbEditItemActions("N");
                    }
                    else if (e.KeyData == Keys.P)
                    {
                        tbEditItemActions("P");
                    }
                    else if (e.KeyData == Keys.D)
                    {
                        tbEditItemActions("D");
                    }
                    else if (e.KeyData == Keys.T)
                    {
                        tbEditItemActions("T");
                    }
                    else if (e.KeyData == Keys.U)
                    {
                        tbEditItemActions("U");
                    }
                    else if (e.KeyData == Keys.Escape || e.KeyData == Keys.Enter)
                    {
                        tbEditItemActions("Esc");
                    }

                    //Added By shitaljit(QuicSolv) on 6 May 2011
                    else if (e.Control && e.KeyCode == Keys.T && e.Shift == false && e.Alt == false)
                    {
                        taxOverrideAll();
                    }
                    else if (e.Control && e.KeyCode == Keys.C && e.Shift == false && e.Alt == false)
                    {
                        //Added By Shitaljit on 16 May 2012
                        if (grdDetail.Rows.Count > 0)
                        {
                            tbTerminalEnteryActions("Ctrl+C");
                            this.txtItemCode.Focus();
                        }
                    }
                    //Till here Added by shitaljit(QuicSolv)
                    e.Handled = false;
                }
            }
            catch (Exception) { }
        }
        //done by sandeep
        private void grdDetail_AfterRowsDeleted(object sender, System.EventArgs e)
        {
            txtLineItemCnt.Text = "Line Items = " + GetLineItemCount().ToString() + "          Total Quantity = " + GetTotalQty().ToString();    //Sprint-26 - PRIMEPOS-2414 21-Jun-2017 JY Added
            frmMain.TransactionRecordCount = grdDetail.Rows.Count;  //PRIMEPOS-1923 08-Aug-2018 JY Added

            oPOSTrans.ProcessItemsForComboPricing(oPOSTrans.oTransDData.TransDetail, oPOSTrans.oTDRow, true, oPOSTrans.oTDTaxData);
            AutoSelectFirstRxCustomer();    //Sprint-23 - PRIMEPOS-2293 31-May-2016 JY Added to auto select first rx customer in sale transaction

            #region Sprint-27 - PRIMEPOS-2413 19-Sep-2017 JY Added 
            if (Configuration.CInfo.GroupTransItems == false)
            {
                oPOSTrans.ProcessItemsForSalePrice(oPOSTrans.oTransDData.TransDetail, oPOSTrans.oTDRow, true, oPOSTrans.oTDTaxData);
            }
            #endregion

            if (this.grdDetail.Rows.Count == 0)
            {
                try
                {
                    oPOSTrans.oTransHData.TransHeader.Rows[0]["ReturnTransID"] = 0;
                }
                catch { } //Sprint-26 - PRIMEPOS-2383 09-Aug-2017 JY Added
                this.EnabledDisableItemRow(true);
                this.txtItemCode.Enabled = true;
                this.lblCouponDiscount.Text = string.Empty; //PRIMEPOS-2034 12-Mar-2018 JY Added                
                this.txtItemCode.Focus();
                #region BatchDelivery - NileshJ - PRIMERX-7688 23-Sept-2019
                isBatchDelivery = false; //If all the rows of frmPOSTransaction get removed it will convert mode back to normal one not the BatchDelivery mode.
                #endregion
                #region PRIMEPOS-2738 ARVIND - REVERSAL
                if (Configuration.CSetting.StrictReturn == true && oPOSTrans.CurrentTransactionType == POS_Core.TransType.POSTransactionType.SalesReturn)
                {
                    this.SetNew(false);
                    this.setTransactionType(POS_Core.TransType.POSTransactionType.Sales);
                }
                #endregion
            }
            else if (this.grdDetail.ActiveRow != null)
            {
                this.grdDetail.ActiveRow.Selected = true;
            }
            #region PRIMEPOS-2738 
            //NEED TO COMMENT THE CONDITION FOR SETTING RETURNTRANSID 0 FOR PARTIAL TRANSACTION NEED TO CHECK IN CODE WHEATHER THIS IMPACTS ON OTHER CONDITIONS OR NOT
            #endregion
            //#region Sprint-26 - PRIMEPOS-2383 09-Aug-2017 JY Added
            //try
            //{
            //    if (Configuration.convertNullToInt(oPOSTrans.oTransHData.TransHeader.Rows[0]["ReturnTransID"]) > 0)
            //    {
            //        bool bResetReturnTransId = oPOSTrans.CheckReturnTransactionId();
            //        if (!bResetReturnTransId) oPOSTrans.oTransHData.TransHeader.Rows[0]["ReturnTransID"] = 0;
            //    }
            //}
            //catch (Exception exp)
            //{
            //    logger.Fatal(exp, "grdDetail_AfterRowsDeleted()");
            //}
            //#endregion
        }
        //completed by sandeep
        private void grdDetail_BeforeRowsDeleted(object sender, Infragistics.Win.UltraWinGrid.BeforeRowsDeletedEventArgs e)
        {
            try
            {
                if (!DelFromGridFlag)
                {
                    if (UserPriviliges.getPermission(UserPriviliges.Modules.POSTransaction.ID, UserPriviliges.Screens.POSTransaction.ID, UserPriviliges.Permissions.DeleteItemfromPOSTrans.ID, UserPriviliges.Permissions.DeleteItemfromPOSTrans.Name))   //PRIMEPOS-1923 08-Aug-2018 JY Added to check permissions prior to delete item message
                    {
                        DelFromGridFlag = true;
                        e.Cancel = true;
                        tbEditItemActions("Del", this.grdDetail.ActiveRow);
                        DelFromGridFlag = false;    //PRIMEPOS-1923 08-Aug-2018 JY Added - delete confirmation message occurs only while deleting first items, it supposed to bringup for every item before delete
                        return;
                    }
                    else    //PRIMEPOS-1923 08-Aug-2018 JY Added
                    {
                        e.Cancel = true;
                        return;
                    }
                }

                logger.Trace("grdDetail_BeforeRowsDeleted() - " + clsPOSDBConstants.Log_Entering);
                e.DisplayPromptMsg = false;

                //if (UserPriviliges.getPermission(UserPriviliges.Modules.POSTransaction.ID, UserPriviliges.Screens.POSTransaction.ID, UserPriviliges.Permissions.DeleteItemfromPOSTrans.ID, UserPriviliges.Permissions.DeleteItemfromPOSTrans.Name))
                //{
                TransDetailRow TempRow = null;
                if (this.grdDetail.ActiveRow.Cells[clsPOSDBConstants.TransDetail_Fld_TransDetailID].Value.ToString().Trim() == "")
                {
                    TempRow = oPOSTrans.oTransDData.TransDetail[this.grdDetail.ActiveRow.ListIndex];
                    oPOSTrans.SetRowTrans(TempRow);
                }
                else
                {
                    DataRow[] dr = (oPOSTrans.oTransDData.TransDetail.Select(clsPOSDBConstants.TransDetail_Fld_TransDetailID + "=" + this.grdDetail.ActiveRow.Cells[clsPOSDBConstants.TransDetail_Fld_TransDetailID].Value.ToString()));
                    TempRow = oPOSTrans.oTransDData.TransDetail.NewTransDetailRow();
                    TempRow.ItemArray = ((TransDetailRow)dr[0]).ItemArray;
                    oPOSTrans.SetRowTrans(TempRow);
                }
                oPOSTrans.oTDRow = TempRow;

                int quantity = oPOSTrans.GetExistingQuantity(TempRow, oPOSTrans.oTransDData);
                ItemData oItemData;
                ItemRow oItemRow = null;
                oItemData = oPOSTrans.PopulateItem(TempRow.ItemID);
                if (oItemData != null)
                {
                    if (oItemData.Item.Rows.Count > 0)
                    {
                        oItemRow = oItemData.Item[0];
                    }
                    if (oItemData.Item.Rows.Count > 0 && oItemRow.SaleLimitQty > 0 && oItemRow.SaleLimitQty < quantity && TempRow.IsPriceChanged && oItemRow.isOnSale && TempRow.Price == oItemRow.OnSalePrice)
                    {
                        clsUIHelper.ShowErrorMsg(" Can not Delete Discounted Item " + Environment.NewLine + "Delete Non Discounted row of same item first");
                        e.Cancel = true;
                        return;
                    }
                }

                if (e.Rows[0].Cells[clsPOSDBConstants.TransDetail_Fld_ItemID].Text.Trim().ToUpper() == "RX")
                {
                    oPOSTrans.RemoveRX(RxWithValidClass, DrugClassInfoCapture, e.Rows[0].Cells[clsPOSDBConstants.TransDetail_Fld_TransDetailID].Value.ToString(), lstOnHoldRxs);
                }

                int i;
                i = e.Rows[0].Index;
                if (i > 0)
                {
                    this.grdDetail.ActiveRow = this.grdDetail.Rows[i - 1];
                    this.grdDetail.ActiveRow.Selected = true;
                }
                else if (i == 0 && this.grdDetail.Rows.Count > this.grdDetail.Selected.Rows.Count)
                {
                    this.grdDetail.ActiveRow = this.grdDetail.Rows[this.grdDetail.Selected.Rows.Count];
                    this.grdDetail.ActiveRow.Selected = true;
                }
                //}
                //else
                //{
                //    e.Cancel = true;
                //}
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "grdDetail_BeforeRowsDeleted()");
                clsUIHelper.ShowErrorMsg(ex.Message);
            }
            logger.Trace("grdDetail_BeforeRowsDeleted() - " + clsPOSDBConstants.Log_Exiting);
        }
        //completed by sandeep
        private void grdDetail_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            try
            {
                if (this.grdDetail.ActiveRow != null)
                {
                    if (e.Control && e.KeyCode == Keys.I)
                    {
                        EditItem(this.grdDetail.ActiveRow.Cells[clsPOSDBConstants.Item_Fld_ItemID].Text.ToString());
                    }
                }
            }
            catch (Exception exp)
            {
                logger.Trace("grdDetail_KeyUp() - " + clsPOSDBConstants.Log_Exiting);
            }
        }
        //completed by sandeep
        private void grdDetail_AfterRowInsert(object sender, Infragistics.Win.UltraWinGrid.RowEventArgs e)
        {
            e.Row.Selected = true;
        }
        //completed by sandeep
        private void grdDetail_ClickCellButton(object sender, CellEventArgs e)
        {
            if (isBatchWtihShippingItem && Convert.ToString(this.grdDetail.ActiveRow.Cells[clsPOSDBConstants.Item_Fld_ItemID].Value) != Configuration.ShippingItem)//2927
            {
                clsUIHelper.ShowErrorMsg("Cannot remove item in Batch Shipping");
                return;
            }
            if (grdDetail.Rows.Count == 1)
                grdDetail.Rows[0].Selected = true;
            DelFromGridFlag = true;
            if (UserPriviliges.getPermission(UserPriviliges.Modules.POSTransaction.ID, UserPriviliges.Screens.POSTransaction.ID, UserPriviliges.Permissions.DeleteItemfromPOSTrans.ID, UserPriviliges.Permissions.DeleteItemfromPOSTrans.Name))   //PRIMEPOS-1923 08-Aug-2018 JY Added
            {
                tbEditItemActions("Del");
            }
            DelFromGridFlag = false;
        }

        //completed by sandeep
        private void grdDetail_InitializeRow(object sender, InitializeRowEventArgs e)
        {
            txtLineItemCnt.Text = "Line Items = " + GetLineItemCount().ToString() + "          Total Quantity = " + GetTotalQty().ToString();    //Sprint-26 - PRIMEPOS-2414 21-Jun-2017 JY Added
            frmMain.TransactionRecordCount = grdDetail.Rows.Count;  //PRIMEPOS-1923 08-Aug-2018 JY Added

            //Added By shitaljit for PrimePOS -13 Inventory Low Non Prompt
            string ItemCode = Configuration.convertNullToString(e.Row.Cells[clsPOSDBConstants.TransDetail_Fld_ItemID].Value);
            int RoeorderLevel = 0;
            int QtyInHand = 0;
            if (oPOSTrans.IsInventoryLow(ItemCode, out RoeorderLevel, out QtyInHand) == true)
            {
                e.Row.Cells[clsPOSDBConstants.TransDetail_Fld_ItemDescription].Appearance.ForeColor = Color.Red;
                e.Row.Cells[clsPOSDBConstants.TransDetail_Fld_ItemID].Appearance.ForeColor = Color.Red;
                string ToolTipText = string.Empty;
                ToolTipText = "The Inventory for Item #" + ItemCode + " is low, Qty. in stock is " + QtyInHand;
                if (RoeorderLevel != 0)
                {
                    ToolTipText += " and Re-Order level is " + RoeorderLevel;
                }
                e.Row.Cells[clsPOSDBConstants.TransDetail_Fld_ItemID].ToolTipText = ToolTipText;
            }
            //Added by shitaljit on 16 August 2012 to highlight Disc. Cell
            decimal discount = Configuration.convertNullToDecimal(e.Row.Cells[clsPOSDBConstants.TransDetail_Fld_Discount].Value.ToString());
            if (discount != 0)
            {
                e.Row.Cells[clsPOSDBConstants.TransDetail_Fld_Discount].Appearance.BackColor = Color.Red;
            }
            else
            {
                e.Row.Cells[clsPOSDBConstants.TransDetail_Fld_Discount].Appearance.BackColor = Color.White;
            }
            //Added By shitaljit for JIRA-341 Ext Price Should be price after dics
            decimal UnitPrice = Configuration.convertNullToDecimal(e.Row.Cells[clsPOSDBConstants.TransDetail_Fld_Price].Value.ToString());
            decimal ExtPrice = Configuration.convertNullToDecimal(e.Row.Cells[clsPOSDBConstants.TransDetail_Fld_ExtendedPrice].Value.ToString());
            int Qty = Configuration.convertNullToInt(e.Row.Cells[clsPOSDBConstants.TransDetail_Fld_QTY].Value.ToString());
            if (Qty == 0)
            {
                Qty = DefaultQTY;
                oPOSTrans.oTDRow.QTY = Qty;
            }
            e.Row.Cells[clsPOSDBConstants.TransDetail_Fld_NetPrice].Value = "0.00";
            if (Math.Abs(UnitPrice) > 0)
            {
                if (oPOSTrans.CurrentTransactionType == POS_Core.TransType.POSTransactionType.Sales)
                {
                    if (GroupPrice <= 0)
                    {
                        e.Row.Cells[clsPOSDBConstants.TransDetail_Fld_NetPrice].Value = ((UnitPrice * Qty) - discount).ToString("##########0.00");
                    }
                    else
                    {
                        e.Row.Cells[clsPOSDBConstants.TransDetail_Fld_NetPrice].Value = (ExtPrice - discount).ToString("##########0.00");
                    }
                }
                else if (oPOSTrans.CurrentTransactionType == POS_Core.TransType.POSTransactionType.SalesReturn)
                {
                    e.Row.Cells[clsPOSDBConstants.TransDetail_Fld_NetPrice].Value = (-((Math.Abs(UnitPrice) * Math.Abs(Qty)) - Math.Abs(discount))).ToString("##########0.00");
                }
            }
            else if (Math.Abs(UnitPrice) == 0)
            {
                if (oPOSTrans.CurrentTransactionType == POS_Core.TransType.POSTransactionType.Sales)
                {
                    e.Row.Cells[clsPOSDBConstants.TransDetail_Fld_NetPrice].Value = ((ExtPrice * Qty) - discount).ToString("##########0.00");
                }
                else if (oPOSTrans.CurrentTransactionType == POS_Core.TransType.POSTransactionType.SalesReturn)
                {
                    e.Row.Cells[clsPOSDBConstants.TransDetail_Fld_NetPrice].Value = (-((Math.Abs(ExtPrice) * Math.Abs(Qty)) - Math.Abs(discount))).ToString("##########0.00");
                }
            }
            if (Configuration.convertNullToDecimal(e.Row.Cells[clsPOSDBConstants.TransDetail_Fld_NetPrice].Value) == 0)
            {
                e.Row.Cells[clsPOSDBConstants.TransDetail_Fld_NetPrice].Value = "0.00";
            }
            //END

            if (Configuration.convertNullToBoolean(e.Row.Cells[clsPOSDBConstants.TransDetail_Fld_IsIIAS].Value.ToString()) == true)
            {
                e.Row.Appearance.ForeColor = Color.Blue;
            }
            else
            {
                e.Row.Appearance.ForeColor = Color.Black;
            }
            //Added By Amit Date 6 Dec 2011
            string strCat = e.Row.Cells[clsPOSDBConstants.TransDetail_Fld_Category].Value.ToString();

            if (strCat != "")
            {
                string ToolTip = oPOSTrans.GetToolTipForGridItem(strCat);
                e.Row.Cells[clsPOSDBConstants.TransDetail_Fld_Category].ToolTipText = ToolTip;
            }
            //grdDetail.DisplayLayout.AutoFitStyle = AutoFitStyle.ResizeAllColumns;
            //End
            e.Row.Cells[clsPOSDBConstants.TransDetail_Fld_ItemID].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.URL;    //PRIMEPOS-3034 02-Dec-2021 JY Added
        }
        #endregion

        #region gridMethod
        public void AdjustWidthOfCell()
        {
            ColumnsCollection col = grdDetail.DisplayLayout.Bands[0].Columns;

            int grdWidth = grdDetail.Width;

            col[clsPOSDBConstants.TransDetail_Fld_ItemID].RowLayoutColumnInfo.PreferredCellSize = new Size((int)(grdWidth * 0.16), 0);
            col[clsPOSDBConstants.TransDetail_Fld_ItemDescriptionMasked].RowLayoutColumnInfo.PreferredCellSize = new Size((int)(grdWidth * 0.27), 0);  //PRIMEPOS-3130
            col[clsPOSDBConstants.TransDetail_Fld_QTY].RowLayoutColumnInfo.PreferredCellSize = new Size((int)(grdWidth * 0.07), 0);
            col[clsPOSDBConstants.TransDetail_Fld_Price].RowLayoutColumnInfo.PreferredCellSize = new Size((int)(grdWidth * 0.11), 0);
            col[clsPOSDBConstants.TransDetail_Fld_Discount].RowLayoutColumnInfo.PreferredCellSize = new Size((int)(grdWidth * 0.11), 0);
            col["TaxCode"].RowLayoutColumnInfo.PreferredCellSize = new Size((int)(grdWidth * 0.07), 0);
            col[clsPOSDBConstants.TransDetail_Fld_Category].RowLayoutColumnInfo.PreferredCellSize = new Size((int)(grdWidth * 0.07), 0);
            col[clsPOSDBConstants.TransDetail_Fld_NetPrice].RowLayoutColumnInfo.PreferredCellSize = new Size((int)(grdWidth * 0.12), 0);
            col["DelCol"].RowLayoutColumnInfo.PreferredCellSize = new Size((int)(grdWidth * 0.02), 0);
            grdDetail.Refresh();
        }
        //completed by sandeep
        private int GetTotalQty()
        {
            int nTotalQty = 0;
            if (grdDetail.DisplayLayout.Rows.Count > 0)
            {
                for (int i = 0; i < grdDetail.DisplayLayout.Rows.Count; i++)
                {
                    nTotalQty += Math.Abs(Configuration.convertNullToInt(grdDetail.DisplayLayout.Rows[i].Cells[clsPOSDBConstants.TransDetail_Fld_QTY].Text));
                }
            }
            else
            {
                nTotalQty = 0;
            }
            return nTotalQty;
        }
        //completed by sandeep
        private int GetLineItemCount()
        {
            int nLineItemCnt = 0;
            if (grdDetail.DisplayLayout.Rows.Count > 0)
            {
                nLineItemCnt = grdDetail.DisplayLayout.Rows.Count;
            }
            else
            {
                nLineItemCnt = 0;
            }

            return nLineItemCnt;
        }

        //completed by sandeep
        private void HideGridCol()
        {
            try
            {
                foreach (UltraGridColumn oCol in grdDetail.DisplayLayout.Bands[0].Columns)
                {
                    if (oCol.Key != clsPOSDBConstants.TransDetail_Fld_NetPrice && oCol.Key != clsPOSDBConstants.TransDetail_Fld_Discount
                        && oCol.Key != clsPOSDBConstants.TransDetail_Fld_Category && oCol.Key != clsPOSDBConstants.TransDetail_Fld_ItemDescriptionMasked
                        && oCol.Key != clsPOSDBConstants.TransDetail_Fld_ItemID && oCol.Key != clsPOSDBConstants.TransDetail_Fld_Price
                        && oCol.Key != clsPOSDBConstants.TransDetail_Fld_QTY && oCol.Key != "DelCol"
                        && oCol.Key != "TaxCode")
                    {
                        this.grdDetail.DisplayLayout.Bands[0].Columns[oCol.Key].Hidden = true;
                    }
                }
            }
            catch (System.Exception)
            {
            }
        }
        //completed by sandeep
        private void ApplyGridFormat()
        {
            try
            {
                logger.Trace("ApplyGrigFormat() - " + clsPOSDBConstants.Log_Entering);
                clsUIHelper.SetAppearance(this.grdDetail);
                HideGridCol();
                this.grdDetail.DisplayLayout.Scrollbars = Scrollbars.None;
                this.grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.TransDetail_Fld_TransID].Hidden = true;
                this.grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.TransDetail_Fld_TransDetailID].Hidden = true;
                this.grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.TransDetail_Fld_TaxID].Hidden = true;
                this.grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.TransDetail_Fld_TaxAmount].Hidden = true;
                this.grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.TransDetail_Fld_ItemCost].Hidden = true;
                this.grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.TransDetail_Fld_IsPriceChanged].Hidden = true;
                this.grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.TransDetail_Fld_IsPriceChangedByOverride].Hidden = true;    //Sprint-26 - PRIMEPOS-2294 27-Jul-2017 JY Added

                this.grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.TransDetail_Fld_CouponID].Hidden = true;    //PRIMEPOS-2034 05-Mar-2018 JY Added
                this.grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.TransDetail_Fld_IsNonRefundable].Hidden = true; //PRIMEPOS-2592 02-Nov-2018 JY Added 

                this.grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.TransDetail_Fld_UserID].Hidden = true;
                this.grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.TransDetail_Fld_IsIIAS].Hidden = true;

                if (this.grdDetail.DisplayLayout.Bands[0].Columns.Exists(clsPOSDBConstants.TransDetail_Fld_IsEBT))  //Sprint-18 20-Nov-2014 JY Added
                    this.grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.TransDetail_Fld_IsEBT].Hidden = true;

                this.grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.TransDetail_Fld_IsCompanionItem].Hidden = true;
                this.grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.TransDetail_Fld_Discount].Format = "##0.00";
                this.grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.TransDetail_Fld_ExtendedPrice].Format = "########0.00";
                this.grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.TransDetail_Fld_Price].Format = "########0.00";
                this.grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.TransDetail_Fld_ItemCost].Hidden = true;
                this.grdDetail.DisplayLayout.Bands[0].Columns["Total Amount"].Hidden = true;

                if (this.grdDetail.DisplayLayout.Bands[0].Columns.Exists(clsPOSDBConstants.TransDetail_Fld_ReturnTransDetailId))  //Sprint-18 20-Nov-2014 JY Added
                    this.grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.TransDetail_Fld_ReturnTransDetailId].Hidden = true;///Added By krishna on 15 july 2011

                //Added By Amit Date 24 Nov 2011
                if (this.grdDetail.DisplayLayout.Bands[0].Columns.Exists(clsPOSDBConstants.TransDetail_Fld_IsMonitored))  //Sprint-18 20-Nov-2014 JY Added
                    this.grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.TransDetail_Fld_IsMonitored].Hidden = true;
                //End
                this.grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.TransDetail_Fld_NetPrice].Header.VisiblePosition = this.grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.TransDetail_Fld_ExtendedPrice].Header.VisiblePosition;
                this.grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.TransDetail_Fld_ExtendedPrice].Hidden = true;
                this.grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.TransDetail_Fld_NetPrice].Format = "########0.00";
                clsUIHelper.SetReadonlyRow(this.grdDetail);

                if (this.grdDetail.DisplayLayout.Bands[0].Columns.Exists(clsPOSDBConstants.TransDetail_Fld_IsRxItem))  //Sprint-18 20-Nov-2014 JY Added
                    this.grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.TransDetail_Fld_IsRxItem].Hidden = true;
                if (this.grdDetail.DisplayLayout.Bands[0].Columns.Exists(clsPOSDBConstants.TransDetail_Fld_IsComboItem))  //Sprint-18 20-Nov-2014 JY Added
                    this.grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.TransDetail_Fld_IsComboItem].Hidden = true;
                if (this.grdDetail.DisplayLayout.Bands[0].Columns.Exists(clsPOSDBConstants.TransDetail_Fld_LoyaltyPoints))  //Sprint-18 20-Nov-2014 JY Added
                    this.grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.TransDetail_Fld_LoyaltyPoints].Hidden = true;
                if (this.grdDetail.DisplayLayout.Bands[0].Columns.Exists(clsPOSDBConstants.TransDetail_Fld_OrignalPrice))  //Sprint-18 20-Nov-2014 JY Added
                    this.grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.TransDetail_Fld_OrignalPrice].Hidden = true;
                this.grdDetail.DisplayLayout.Bands[0].Columns["NonComboUnitPricingID"].Hidden = true;

                //grdDetail.DisplayLayout.Override.SelectedRowAppearance.BackColor = Color.Black;
                grdDetail.DisplayLayout.Override.ActiveRowAppearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
                grdDetail.DisplayLayout.Override.ActiveRowAppearance.ForeColor = Color.Black;
                grdDetail.DisplayLayout.Override.ActiveRowAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(138)))), ((int)(((byte)(32)))));

                //this.txtCustomer.MaxLength = 20;
                logger.Trace("ApplyGrigFormat() - " + clsPOSDBConstants.Log_Exiting);
            }
            catch (Exception Ex)
            {
                logger.Fatal(Ex, "ApplyGrigFormat()");
            }
        }

        //completed by sandeep
        private void ShowHideGridScroll()
        {
            if (grdDetail.Rows.Count < 12)
            {
                grdDetail.DisplayLayout.Scrollbars = Scrollbars.None;
            }
            else
            {
                grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.TransDetail_Fld_NetPrice].RowLayoutColumnInfo.PreferredCellSize = new Size((int)(grdDetail.Width * 0.12) - 12, 0);
                grdDetail.DisplayLayout.Scrollbars = Scrollbars.Vertical;
            }
        }

        #endregion

        #region EditItem
        //completed by sandeep
        private void stTBEditItem_ButtonClick(object sender, EventArgs e)
        {
            if (sender.GetType() == typeof(Infragistics.Win.Misc.UltraLabel))
            {
                tbEditItemActions(((Infragistics.Win.Misc.UltraLabel)sender).Text.Trim().ToString());
            }
            else
            {
                tbEditItemActions(((Infragistics.Win.Misc.UltraButton)sender).Tag.ToString());
            }
        }
        //completed by sandeep
        private void grpEditOptions_VisibleChanged(object sender, EventArgs e)
        {
            if (oPOSTrans.CurrentTransactionType == POS_Core.TransType.POSTransactionType.Sales)
                btnMarkAllRx.Visible = mGrpEditOptions.Visible;
        }

        #endregion

        #region EditItemMethod
        //completed by sandeep
        private void tbEditItemActions(string strKey, string sPOSUserID = "", string sRxUserID = "")    //PRIMEPOS-2402 14-Jul-2021 JY modified
        {
            strActionButton = strKey;  //Sprint-23 - PRIMEPOS-2302 23-May-2016 JY Added 
            logger.Trace("tbEditItemActions() - " + clsPOSDBConstants.Log_Entering);
            //Added By Shitaljit on 5 Sept 2011
            //To Block Deleting items from grid on return transaction if the transaction have invoice discount
            //Modifeid by on 23 March to allow delete items on hold and items ring up in return mode
            if (strKey == "Del" && oPOSTrans.CurrentTransactionType == POS_Core.TransType.POSTransactionType.SalesReturn)
            {
                try
                {
                    oPOSTrans.ClickRow = grdDetail.ActiveRow.Index + 1;
                    TransHeaderData oHData = oPOSTrans.GetTransactionHeader(isOnHoldTrans, isCallofRetTrans, onHoldTransID, TransID);
                    TransHeaderRow oTransHeaderRow = null;
                    if (oHData != null && oHData.Tables[0].Rows.Count > 0)
                    {
                        oTransHeaderRow = (TransHeaderRow)oHData.Tables[0].Rows[0];
                        //Math.Abs( Configuration.convertNullToDecimal(this.lblInvDiscount.Text)) > 0
                        //is added to block partial return of transactions which were done before invoice discount is captured separately.
                        if (oTransHeaderRow.InvoiceDiscount > 0 || Math.Abs(Configuration.convertNullToDecimal(this.lblInvDiscount.Text)) > 0)
                        {
                            clsUIHelper.ShowErrorMsg("Transaction have invoice discount, you cannot return partial transaction.");
                        }
                        else
                        {
                            tbEditItemActions(strKey, this.grdDetail.ActiveRow);
                        }
                    }
                    else
                    {
                        tbEditItemActions(strKey, this.grdDetail.ActiveRow);
                    }
                }
                catch (Exception ex)
                {
                    clsUIHelper.ShowErrorMsg(ex.Message);
                }
            }
            else
            {
                tbEditItemActions(strKey, this.grdDetail.ActiveRow, sPOSUserID, sRxUserID); //PRIMEPOS-2402 14-Jul-2021 JY modified
            }
            logger.Trace("tbEditItemActions() - " + clsPOSDBConstants.Log_Exiting);
        }
        //completed by sandeep
        private void tbEditItemActions(string strKey, Infragistics.Win.UltraWinGrid.UltraGridRow oGridActiveRow, string sPOSUserID = "", string sRxUserID = "") //PRIMEPOS-2402 14-Jul-2021 JY modified
        {
            try
            {
                logger.Trace("tbEditItemActions() - " + clsPOSDBConstants.Log_Entering);
                if (oGridActiveRow != null)
                {
                    oPOSTrans.LineRow = oGridActiveRow.Index + 1;
                    if (strKey == "Q")
                    {
                        string sUserID = String.Empty;
                        bool isEditable = UserPriviliges.getPermission(UserPriviliges.Modules.POSTransaction.ID, UserPriviliges.Screens.POSTransaction.ID, UserPriviliges.Permissions.QuantityOverride.ID, UserPriviliges.Permissions.QuantityOverride.Name, out sUserID);    //Sprint-26 - PRIMEPOS-2416 04-Jul-2017 JY Added for override quantity
                        if (isEditable)
                        {
                            if (oGridActiveRow.Cells[clsPOSDBConstants.TransDetail_Fld_ItemID].Text != "RX")
                            {
                                #region Q
                                ////Add  by Ravindra for Sale Limit
                                TransDetailRow TempRow = oPOSTrans.GetTransactionDetatilRow(grdDetail.ActiveRow.Cells[clsPOSDBConstants.TransDetail_Fld_TransDetailID].Value.ToString(), grdDetail.ActiveRow.ListIndex);
                                int quantity = oPOSTrans.GetExistingQuantity(TempRow, oPOSTrans.oTransDData);
                                //Till here Add  by Ravindra for Sale Limit
                                oPOSTrans.GetTransDetailRow(oGridActiveRow.Cells[clsPOSDBConstants.TransDetail_Fld_TransDetailID].Value.ToString(), oGridActiveRow.ListIndex);
                                isAddRow = false;
                                isEditRow = true;
                                setItemValues(oPOSTrans.oTDRow);
                                oPOSTrans.oTDRow.QuantityOverrideUser = sUserID; //PRIMEPOS-2402 13-Jul-2021 JY Added
                                EnabledDisableItemRow(false);
                                txtItemCode.Enabled = false;
                                txtQty.Enabled = true;
                                txtQty.Focus();
                                #endregion Q
                            }
                        }
                    }
                    else if (strKey == "N")
                    {
                        if (oGridActiveRow.Cells[clsPOSDBConstants.TransDetail_Fld_ItemID].Text != "RX")
                        {
                            #region N
                            oPOSTrans.GetTransDetailRow(oGridActiveRow.Cells[clsPOSDBConstants.TransDetail_Fld_TransDetailID].Value.ToString(), oGridActiveRow.ListIndex);
                            isAddRow = false;
                            isEditRow = true;
                            setItemValues(oPOSTrans.oTDRow);
                            EnabledDisableItemRow(false);
                            txtItemCode.Enabled = false;
                            txtDescription.Enabled = true;
                            txtDescription.Focus();
                            isDescriptionOverride = true;
                            #endregion N
                        }
                    }
                    else if (strKey == "P")
                    {
                        #region P
                        string sUserID = "";
                        bool isEditable = false;
                        if (oGridActiveRow.Cells[clsPOSDBConstants.TransDetail_Fld_ItemID].Value.ToString() == "RX")
                        {
                            isEditable = UserPriviliges.getPermission(UserPriviliges.Modules.POSTransaction.ID, UserPriviliges.Screens.POSTransaction.ID, UserPriviliges.Permissions.PriceOverrideForRXItemsfromPOSTrans.ID, UserPriviliges.Permissions.PriceOverrideForRXItemsfromPOSTrans.Name, out sUserID);
                        }
                        else
                        {
                            isEditable = UserPriviliges.getPermission(UserPriviliges.Modules.POSTransaction.ID, UserPriviliges.Screens.POSTransaction.ID, UserPriviliges.Permissions.PriceOverridefromPOSTrans.ID, UserPriviliges.Permissions.PriceOverridefromPOSTrans.Name, out sUserID);
                        }

                        if (isEditable)
                        {
                            oPOSTrans.GetTransDetailRow(oGridActiveRow.Cells[clsPOSDBConstants.TransDetail_Fld_TransDetailID].Value.ToString(), oGridActiveRow.ListIndex);
                            isAddRow = false;
                            isEditRow = true;
                            setItemValues(oPOSTrans.oTDRow);
                            oPOSTrans.oTDRow.UserID = sUserID;
                            EnabledDisableItemRow(false);
                            txtItemCode.Enabled = false;
                            txtUnitPrice.Enabled = true;
                            txtUnitPrice.Focus();
                            oPOSTrans.IsPriceChange = true;
                            oPOSTrans.oTDRow.IsPriceChangedByOverride = true; //Sprint-26 - PRIMEPOS-2294 27-Jul-2017 JY Added
                        }

                        #endregion P
                    }
                    else if (strKey == "D")
                    {
                        #region D
                        //PRIMEPOS-2402 07-Jul-2021 JY If item is not discountable, then no need to check override action.
                        oPOSTrans.GetTransDetailRow(oGridActiveRow.Cells[clsPOSDBConstants.TransDetail_Fld_TransDetailID].Value.ToString(), oGridActiveRow.ListIndex);
                        if (oPOSTrans.AllowDiscount(oPOSTrans.oTDRow.ItemID.ToString()) == false)
                        {
                            Resources.Message.Display("Item is not Discountable.", Configuration.ApplicationName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                        else
                        {
                            string sUserID = string.Empty;
                            if (UserPriviliges.getPermission(UserPriviliges.Modules.POSTransaction.ID, UserPriviliges.Screens.POSTransaction.ID, UserPriviliges.Permissions.DiscOverridefromPOSTrans.ID, UserPriviliges.Permissions.DiscOverridefromPOSTrans.Name, out sUserID))    //PRIMEPOS-2402 08-Jul-2021 JY Added sUserID
                            {
                                //Added By Shitaljit on 17 Feb
                                //To block discount on sale Items if Allow Discount Of Items On Sale is uncheck in preference.
                                if (oPOSTrans.AllowDiscountOfItemsOnSale(oPOSTrans.oTDRow.ItemID.ToString()) == false)
                                {
                                    Resources.Message.Display("Current setting does not allow Discount on sale item.", Configuration.ApplicationName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    return;
                                }
                                isAddRow = false;
                                isEditRow = true;
                                setItemValues(oPOSTrans.oTDRow);
                                EnabledDisableItemRow(false);
                                txtItemCode.Enabled = false;
                                frmPOSDicount ofrm;
                                if (frmDiscountOptions.CallDiscountFrm == true)
                                {
                                    ofrm = new frmPOSDicount(trPrice, sUserID);
                                }
                                else
                                {
                                    ofrm = new frmPOSDicount(oPOSTrans.oTDRow.ExtendedPrice, sUserID);
                                }
                                ofrm.Text = "Line Item Discount";
                                ofrm.bCalledFromLineItem = true;
                                if (ofrm.ShowDialog(this) == DialogResult.OK)
                                {
                                    System.Decimal DiscAmt = 0;
                                    if (Convert.ToDecimal(ofrm.numDiscPerc.Value) != 0)
                                        DiscAmt = oPOSTrans.GetDiscount(Convert.ToDecimal((Convert.ToDecimal(ofrm.numDiscPerc.Value) / 100 * Convert.ToDecimal(oPOSTrans.oTDRow.ExtendedPrice))));
                                    else
                                        DiscAmt = Math.Round(Convert.ToDecimal(ofrm.numDiscAmount.Value), 2);

                                    if (DiscAmt > oPOSTrans.oTDRow.ExtendedPrice && oPOSTrans.CurrentTransactionType == POS_Core.TransType.POSTransactionType.Sales)
                                    {
                                        clsUIHelper.ShowErrorMsg("Discount Amount can not be more than Extended Price");
                                    }
                                    else
                                    {
                                        #region PRIMEPOS-2402 08-Jul-2021 JY Added
                                        oPOSTrans.oTDRow.OldDiscountAmt = oPOSTrans.oTDRow.Discount;
                                        oPOSTrans.oTDRow.DiscountOverrideUser = sUserID;
                                        if (ofrm.strMaxDiscountLimitOverrideUser != "")
                                            oPOSTrans.oTDRow.MaxDiscountLimitUser = ofrm.strMaxDiscountLimitOverrideUser;
                                        #endregion
                                        oPOSTrans.oTDRow.Discount = DiscAmt;
                                        string sTaxCodes = string.Empty;
                                        if (oPOSTrans.IsItemTaxableForTrasaction(oPOSTrans.oTDTaxData, oPOSTrans.oTDRow.ItemID, out sTaxCodes, oPOSTrans.oTDRow.TransDetailID) == true)
                                        {
                                            EditTax(oPOSTrans.oTDRow.TaxCode, clsPOSDBConstants.TaxCodes_tbl);
                                        }
                                        ValidateItemDicount(txtDiscount, new CancelEventArgs(false));//Added By Shitaljit new function to handle txtDicount validation
                                        grdDetail.Selected.Rows[0].Cells["Total Amount"].Value = oPOSTrans.oTDRow.ExtendedPrice - oPOSTrans.oTDRow.Discount;

                                        if (Configuration.CPOSSet.UseSigPad == true)
                                        {
                                            //Added by Manoj 10/13/2011 - this is to get know if the item  was onhold or not
                                            if (oPOSTrans.oTDRow.TransID > 0 && onHoldTransID > 0)
                                            {
                                                SigPadUtil.DefaultInstance.iSItemOnHold(true);
                                            }
                                            else
                                            {
                                                SigPadUtil.DefaultInstance.iSItemOnHold(false);
                                            }
                                            logger.Trace("tbEditItemActions() - POSTransaction Edit Item Info- Sig Pad " + strKey + " ItemId: " + oPOSTrans.oTDRow.ItemID + " ItemDescription: " + oPOSTrans.oTDRow.ItemDescription + " TransDetailID: " + oPOSTrans.oTDRow.TransDetailID);
                                            int index = oPOSTrans.GetTransIndex(oPOSTrans.oTransDData, oPOSTrans.oTDRow.TransDetailID);
                                            if (SigPadUtil.DefaultInstance.IsVF)
                                                DeviceItemsProcess("UpdateItem", oPOSTrans.oTDRow, index);
                                            else
                                                SigPadUtil.DefaultInstance.UpdateItem(oPOSTrans.oTDRow, index);
                                        }
                                        //end of added by atul 02-nov-2010
                                    }
                                }
                                else if (frmDiscountOptions.CallDiscountFrm == true)
                                {
                                    oDiscountOptions.ShowDialog();
                                    if (oDiscountOptions.DialogResult == DialogResult.Retry)
                                        tbEditItemActions("D");
                                }
                                else //PRIMEPOS-2402 08-Jul-2021 JY Added
                                {
                                    Configuration.IsDiscOverridefromPOSTrans = false;
                                }
                                ClearItemRow();
                                grdDetail.Focus();
                            }
                        }
                        #endregion D
                    }
                    else if (strKey == "T")
                    {
                        //if (UserPriviliges.getPermission(UserPriviliges.Modules.POSTransaction.ID, UserPriviliges.Screens.POSTransaction.ID, UserPriviliges.Permissions.TaxOverride.ID, UserPriviliges.Permissions.TaxOverride.Name)){   //PRIMEPOS-2510 26-Apr-2018 JY Commented
                        //if (oGridActiveRow.Cells[clsPOSDBConstants.TransDetail_Fld_ItemID].Text != "RX"){  //PRIMEPOS-2510 26-Apr-2018 JY Commented
                        //PRIMEPOS-2510 26-Apr-2018 JY Added user level permisson to control tax override for Rx item
                        bool isEditable = false;
                        string sUserID = string.Empty;  //PRIMEPOS-2402 13-Jul-2021 JY Added
                        if (oGridActiveRow.Cells[clsPOSDBConstants.TransDetail_Fld_ItemID].Value.ToString().Trim().ToUpper() == "RX")
                        {
                            isEditable = UserPriviliges.getPermission(UserPriviliges.Modules.POSTransaction.ID, UserPriviliges.Screens.POSTransaction.ID, UserPriviliges.Permissions.TaxOverrideForRx.ID, UserPriviliges.Permissions.TaxOverrideForRx.Name, out sUserID);   //PRIMEPOS-2402 13-Jul-2021 JY Added userid
                        }
                        else
                        {
                            isEditable = UserPriviliges.getPermission(UserPriviliges.Modules.POSTransaction.ID, UserPriviliges.Screens.POSTransaction.ID, UserPriviliges.Permissions.TaxOverride.ID, UserPriviliges.Permissions.TaxOverride.Name, out sUserID); //PRIMEPOS-2402 13-Jul-2021 JY Added userid
                        }
                        if (isEditable)
                        {
                            #region T
                            oPOSTrans.GetTransDetailRow(oGridActiveRow.Cells[clsPOSDBConstants.TransDetail_Fld_TransDetailID].Value.ToString(), oGridActiveRow.ListIndex);
                            oPOSTrans.oTDRow.TaxOverrideUser = sUserID; //PRIMEPOS-2402 13-Jul-2021 JY Added
                            this.isAddRow = false;
                            this.isEditRow = true;
                            this.setItemValues(oPOSTrans.oTDRow);
                            this.EnabledDisableItemRow(false);
                            this.txtItemCode.Enabled = false;
                            this.cmbTaxCode.Enabled = true;
                            this.cmbTaxCode.Text = oPOSTrans.oTDRow.TaxCode;
                            this.cmbTaxCode.Focus();
                            #endregion T
                        }
                        //}
                        //}
                    }
                    else if (strKey == "removeindTax")
                    {
                        //if (oGridActiveRow.Cells[clsPOSDBConstants.TransDetail_Fld_ItemID].Text != "RX") {    //PRIMEPOS-2510 26-Apr-2018 JY Commented
                        #region T
                        oPOSTrans.GetTransDetailRow(oGridActiveRow.Cells[clsPOSDBConstants.TransDetail_Fld_TransDetailID].Value.ToString(), oGridActiveRow.ListIndex);
                        oPOSTrans.oTDRow.TaxOverrideAllOTCUser = sPOSUserID; //PRIMEPOS-2402 15-Jul-2021 JY Added
                        oPOSTrans.oTDRow.TaxOverrideAllRxUser = sRxUserID; //PRIMEPOS-2402 15-Jul-2021 JY Added
                        this.isAddRow = false;
                        this.isEditRow = true;
                        this.setItemValues(oPOSTrans.oTDRow, strKey);   //PRIMEPOS-2924 08-Dec-2020 JY Added strKey parameter
                        this.EnabledDisableItemRow(false);
                        this.txtItemCode.Enabled = false;
                        this.cmbTaxCode.Enabled = true;

                        if (taxoverflag == 1)
                        {
                            this.cmbTaxCode.Text = "";
                            ValidateItemTax(cmbTaxCode, new CancelEventArgs(false));//Added By Shitaljit new function to handle cmbTaxCode validation
                        }
                        else
                            this.cmbTaxCode.Focus();

                        #endregion T
                        //}
                    }
                    else if (strKey == "U")
                    {
                        if (oGridActiveRow.Cells[clsPOSDBConstants.TransDetail_Fld_ItemID].Text == "RX")
                        {
                            #region U
                            oPOSTrans.GetTransDetailRow(oGridActiveRow.Cells[clsPOSDBConstants.TransDetail_Fld_TransDetailID].Value.ToString(), oGridActiveRow.ListIndex);
                            if (oPOSTrans.oTDRow.ItemDescription.IndexOf("-") > 0)
                            {
                                string sRXNo = oPOSTrans.oTDRow.ItemDescription.Substring(0, oPOSTrans.oTDRow.ItemDescription.IndexOf("-")).Trim();
                                oPOSTrans.oTDRow = null;
                                RXHeader oPatient = oPOSTrans.getPatientFromRxHeaderList(oPOSTrans.oRXHeaderList, sRXNo);
                                if (oPatient != null)
                                {
                                    FillUnPickedRXs(oPatient, false);
                                }
                            }

                            #endregion U
                        }
                    }
                    else if (strKey == "Del")
                    {
                        #region Del

                        string ItemID = Configuration.convertNullToString(this.grdDetail.ActiveRow.Cells[clsPOSDBConstants.Item_Fld_ItemID].Value);
                        logger.Trace("tbEditItemActions() - Delete Item Item# " + ItemID + " Deleted - " + clsPOSDBConstants.Log_Entering);
                        //in case user cancel the signature capturing event invoice discount stays and does not recalculate accrodingly.
                        if (Configuration.convertNullToDecimal(this.lblInvDiscount.Text) > 0 && oPOSTrans.CurrentTransactionType == POS_Core.TransType.POSTransactionType.Sales)
                        {
                            if (Resources.Message.Display("Current transaction has invoce discount of " + Configuration.CInfo.CurrencySymbol.ToString() + this.lblInvDiscount.Text
                                + ".\nDeleting item will discard current invoice discount, are you sure?", "POS Transaction", MessageBoxButtons.YesNo) == DialogResult.Yes)
                            {
                                DelFromGridFlag = true;
                                lblInvDiscount.Text = "0.00";
                                RecalculateTax();
                                ultraCalcManager1.ReCalc();
                            }
                            else
                            {
                                return;
                            }
                        }

                        if (this.grdDetail.Rows.Count > 0)
                        {
                            try
                            {
                                int index = grdDetail.ActiveRow.Index;
                                this.grdDetail.ActiveRow.Delete(true);
                                oPOSTrans.UpdateTransTaxDetails(oPOSTrans.oTDTaxData, oPOSTrans.oTDRow.TransDetailID);   //Sprint-26 - PRIMEPOS-XXXX 01-Sep-2017 JY Added //PRIMEPOS-2500 02-Apr-2018 JY removed unwanted parameter
                                this.grdDetail.UpdateData();
                                this.grdDetail.Refresh();
                                HideGridCol();
                                if (Configuration.CPOSSet.UseSigPad == true)
                                {
                                    if (SigPadUtil.DefaultInstance.IsVF)
                                    {
                                        DeviceItemsProcess("DeleteItem", oPOSTrans.oTDRow, index);
                                    }
                                    else if (SigPadUtil.DefaultInstance.isPAX && (Configuration.CPOSSet.PinPadModel.ToUpper().Trim() == PAX_DEVICE_ARIES8.ToUpper().Trim()
                                      || Configuration.CPOSSet.PinPadModel.ToUpper().Trim() == PAX_DEVICE_A920.ToUpper().Trim())) //PRIMEPOS-2952 PRIMEPOS-3146
                                    {
                                        SigPadUtil.DefaultInstance.DeleteItemHPSPaxAX(index, oPOSTrans.oTDRow);
                                    }
                                    else if (SigPadUtil.DefaultInstance.isPAX && Configuration.CPOSSet.PinPadModel.ToUpper().Trim() == PAX_DEVICE_PAX7.ToUpper().Trim()) ////PRIMEPOS-2952
                                    {
                                        SigPadUtil.DefaultInstance.DeleteItemHPSPax7(index, oPOSTrans.oTDRow);
                                    }
                                    else
                                        SigPadUtil.DefaultInstance.DeleteItem(index);
                                }
                                #region AuditTrail - NileshJ PRIMEPOS-2808 
                                if (oAuditTrail.oAuditDataSet.Tables[0].Rows.Count > 0)
                                {
                                    //DataTable dt = new DataTable();
                                    //dt = oAuditTrail.oAuditDataSet.Tables[0].AsEnumerable().Where(a => a.Field<string>("EntityKey") == oPOSTrans.oTDRow.ItemDescription.Trim()).CopyToDataTable();
                                    for (int i = oAuditTrail.oAuditDataSet.Tables[0].Rows.Count - 1; i >= 0; i--)
                                    {
                                        DataRow dr = oAuditTrail.oAuditDataSet.Tables[0].Rows[i];
                                        if (dr["EntityKey"] == oPOSTrans.oTDRow.ItemDescription.Trim())
                                        {
                                            dr.Delete();
                                        }
                                    }
                                    oAuditTrail.oAuditDataSet.Tables[0].AcceptChanges();
                                }
                                #endregion
                            }
                            catch (Exception) { }
                        }
                        if (this.grdDetail.Rows.Count == 0)
                        {
                            isAddRow = false;
                            isEditRow = false;
                            EnabledDisableItemRow(false);
                            txtItemCode.Focus();
                            onHoldTransID = 0;//Added By Shitaljit(QuicSolv) on 10 Nov 2011
                            lstOnHoldRxs = new List<OnholdRxs>(); //PRIMEPOS-2639 27-Mar-2019 JY Added
                            isOnHoldTrans = false;
                            TransID = 0;
                            this.isCallofRetTrans = false;
                            if (Configuration.isNullOrEmptyDataSet(oPOSTrans.oTransDRXData) == false)
                            {
                                oPOSTrans.oTransDRXData.TransDetailRX.Rows.Clear();
                            }
                            //Added by shitaljit on 29 october to clear selected customer if there all the items are deleted.
                            if (oPOSTrans.oCustomerRow != null && oPOSTrans.CurrentTransactionType == POS_Core.TransType.POSTransactionType.Sales)
                            {
                                if (oPOSTrans.oCustomerRow.AccountNumber != -1)
                                {
                                    if (Resources.Message.Display("There is no item remaining in the transaction.\nDo you want to discard selected customer?", "POS Transaction", MessageBoxButtons.YesNo) == DialogResult.Yes)
                                    {
                                        #region PRIMEPOS-3207
                                        clearHyphenAlert();
                                        #endregion
                                        SetNew(false);
                                        if (Configuration.CSetting.EnableCustomerEngagement) // PRIMEPOS-2794
                                        {
                                            GetCustomerDetails(); //SAJID PRIMEPOS-2794
                                            rightTabPayCust.Tabs[0].Selected = true;
                                        }
                                    }
                                    else
                                    {
                                        CustomerRow TempRow = oPOSTrans.oCustomerRow;
                                        ClearAll();
                                        oPOSTrans.oCustomerRow = TempRow;   //PRIMEPOS-2535 04-Jun-2018 JY CustomerRow should persist
                                    }
                                }
                            }
                            //PRIMEPOS-2534 (Suraj) 31-may-18 Suraj - Added VFInactivityMonitor for ingenico lane close issue
                            StartVFInactivityMonitor(this);
                            //
                        }
                        logger.Trace("tbEditItemActions() - Delete Item Item# " + ItemID + " Deleted - " + clsPOSDBConstants.Log_Exiting);
                        #endregion Del
                    }
                    else if (strKey == "Ctrl+I")
                    {
                        if (oGridActiveRow != null)
                        {
                            EditItem(oGridActiveRow.Cells[clsPOSDBConstants.Item_Fld_ItemID].Text.ToString());
                        }
                    }
                    else if (strKey == "Ctrl+T")
                    {
                        #region Tax Overrilde All

                        if (oGridActiveRow != null)
                        {
                            taxOverrideAll();
                            this.txtItemCode.Focus();
                        }

                        #endregion Tax Overrilde All
                    }
                    if (strKey.ToLower() == "esc" || strKey.ToLower() == "return")
                    {
                        #region Esc

                        this.EnabledDisableItemRow(true);
                        this.grdDetail.Selected.Rows.Clear();
                        oGridActiveRow = null;
                        this.txtItemCode.Enabled = true;

                        if (this.grdDetail.Rows.Count > 0)
                        {
                            this.changeStToolbars(TransactionStToolbars.strTBTerminalEntery);
                        }
                        else
                        {
                            this.changeStToolbars(TransactionStToolbars.strTBTerminal);
                        }

                        this.txtItemCode.Focus();

                        #endregion Esc
                    }
                }
            }
            catch (Exception e)
            {
                ClearItemRow();
                logger.Fatal(e, "tbEditItemActions()");
                clsUIHelper.ShowErrorMsg(e.Message);
            }
            logger.Trace("tbEditItemActions() - " + clsPOSDBConstants.Log_Exiting);
        }

        #region PRIMEPOS-2979 19-Aug-2021 JY Added
        public bool ValidateUserDicountLimit(decimal DicsValueToVerify, out decimal InvDicsValueToVerify, out string strMaxDiscountLimitOverrideUser, string DiscountOverrideUser = "")
        {
            InvDicsValueToVerify = 0;
            bool RetVal = false;
            strMaxDiscountLimitOverrideUser = "";

            try
            {
                string sUserID = string.Empty;
                UserManagement.clsLogin oLogin = new UserManagement.clsLogin();
                if (DicsValueToVerify > Configuration.UserMaxDiscountLimit && Configuration.IsDiscOverridefromPOSTrans == false)
                {
                    frmPOSTransaction.InvDicsValueToVerify = DicsValueToVerify;
                    if (oLogin.loginForPreviliges(clsPOSDBConstants.UserMaxDiscountLimit, "", out sUserID, "Security Override For Max Discount Limit") == false)
                    {
                        InvDicsValueToVerify = 0;
                        return false;
                    }
                    else
                    {
                        strMaxDiscountLimitOverrideUser = sUserID;
                    }
                }
                else if (Configuration.IsDiscOverridefromPOSTrans == true)
                {
                    //Check with logged in user limit
                    if (DicsValueToVerify > Configuration.UserMaxDiscountLimit)
                    {
                        //Check with discount override user limit
                        bool bStatus = false;
                        if (DiscountOverrideUser != "" && DiscountOverrideUser != Configuration.UserName)
                            bStatus = UserPriviliges.IsUserHasPriviligesToOverrideInvoiceDiscount(DiscountOverrideUser, DicsValueToVerify);
                        if (bStatus == false)
                        {
                            frmPOSTransaction.InvDicsValueToVerify = DicsValueToVerify;
                            if (oLogin.loginForPreviliges(clsPOSDBConstants.UserMaxDiscountLimit, "", out sUserID, "Security Override For MaxDiscountLimit: exceeds logged-in & Disc Override user") == false)
                            {
                                InvDicsValueToVerify = 0;
                                return false;
                            }
                            else
                            {
                                strMaxDiscountLimitOverrideUser = sUserID;
                            }
                        }
                    }
                }
                frmPOSTransaction.InvDicsValueToVerify = 0;
                return true;
            }
            catch (Exception Ex)
            {
                RetVal = false;
                logger.Fatal(Ex, "ValidateUserDiscountLimit()");
                throw Ex;
            }
            return RetVal;
        }
        #endregion

        private void EnabledDisableItemRow(bool bValue)
        {
            try
            {
                this.txtDescription.Enabled = bValue;
                this.txtExtAmount.Enabled = bValue;
                this.txtDiscount.Enabled = bValue;
                this.cmbTaxCode.Enabled = bValue;
                this.txtUnitPrice.Enabled = bValue;
            }
            catch (Exception exp)
            {
                logger.Fatal(exp, "EnabledDisableItemRow()");
            }
        }

        private void setItemValues(TransDetailRow oRow, string strKey = "") //PRIMEPOS-2924 08-Dec-2020 JY Added strKey parameter
        {
            try
            {
                logger.Trace("setItemValues() - " + clsPOSDBConstants.Log_Entering);
                if (oRow == null)
                {
                    logger.Trace("setItemValues() - " + clsPOSDBConstants.Log_Exiting + "1");
                    return;
                }
                txtItemCode.Text = oRow.ItemID;
                txtDescription.Text = oRow.ItemDescription;
                txtQty.Value = oRow.QTY;
                txtUnitPrice.Value = Math.Round(oRow.Price, 2);
                TaxCodesData oTaxCodesData = new TaxCodesData();
                LoadTaxCodes(oPOSTrans.oTDRow.ItemID, out oTaxCodesData, strKey);   //PRIMEPOS-2924 08-Dec-2020 JY Added strKey parameter
                txtDiscount.Value = Math.Round(oRow.Discount, 2);
                txtExtAmount.Text = Convert.ToString(Math.Round(oRow.ExtendedPrice, 2));
                logger.Trace("setItemValues() - " + clsPOSDBConstants.Log_Exiting + "2");
            }
            catch (Exception exp)
            {
                logger.Fatal(exp, "setItemValues()");
            }
        }

        //done by sandeep
        private void ClearItemRow()
        {
            try
            {
                logger.Trace("ClearItemRow() - " + clsPOSDBConstants.Log_Entering);
                txtItemCode.Text = String.Empty;
                txtExtAmount.Text = "0";
                txtDescription.Text = String.Empty;
                txtDiscount.Value = 0;
                txtQty.Value = this.DefaultQTY;
                txtUnitPrice.Value = 0;
                EnabledDisableItemRow(false);
                cmbTaxCode.DataSource = null;
                cmbTaxCode.Text = String.Empty;
                foreach (var item in this.cmbTaxCode.Items)
                {
                    item.CheckState = CheckState.Unchecked;
                }
                if (txtItemCode.Enabled == false)
                    txtItemCode.Enabled = true; //Sprint-27 - PRIMEPOS-2451 06-Oct-2017 JY Added
                txtItemCode.Focus();
                isEditRow = false;
                isAddRow = false;
                logger.Trace("ClearItemRow() - " + clsPOSDBConstants.Log_Exiting);
            }
            catch (Exception Ex)
            {
                logger.Fatal(Ex, "ClearItemRow()");
            }
        }

        #endregion

        #region sigpad

        //completed by sandeep
        private void DeviceItemsProcess(string ProcessName, TransDetailRow oRow, int index)
        {
            //Recalculate the price

            #region Recalculate the Sum

            ultraCalcManager1.ReCalc();
            //Update the pad sum
            UpdateAmounts();

            #endregion Recalculate the Sum

            switch (ProcessName.ToUpper())
            {
                #region AddItem

                case "ADDITEM":
                    {
                        //Items
                        if (oRow != null)
                        {
                            Invoke(new MethodInvoker(() =>
                            {
                                SigPadUtil.DefaultInstance.AddItem(oRow, grdDetail.Rows.Count - 1);
                            }));
                            logger.Trace("DeviceItemsProcess() - POSTransaction Add Item-Added to Sig Pad", "ValidateRow()", "CurrentTransID " + oRow.TransID + " ItemId: " + oRow.ItemID + " ItemDescription: " + oRow.ItemDescription + " TransDetailID: " + oRow.TransDetailID);
                        }
                    }
                    break;

                #endregion AddItem

                #region Add Companion Item

                case "ADDCOMPANIONITEM":
                    {
                        //Companion Items
                        if (oRow != null)
                        {
                            Invoke(new MethodInvoker(() =>
                            {
                                SigPadUtil.DefaultInstance.AddItem(oRow, grdDetail.Rows.Count - 1);
                            }));
                            logger.Trace("DeviceItemsProcess() - POSTransaction Add Companion Item-Added to Sig Pad", "ValidateRow()", "CurrentTransID " + oRow.TransID + " ItemId: " + oRow.ItemID + " ItemDescription: " + oRow.ItemDescription + " TransDetailID: " + oRow.TransDetailID);
                        }
                    }
                    break;

                #endregion Add Companion Item

                #region Update Item

                case "UPDATEITEM":
                    {
                        if (oRow != null)
                        {
                            Invoke(new MethodInvoker(() =>
                            {
                                SigPadUtil.DefaultInstance.UpdateItem(oRow, index);
                            }));
                        }
                    }
                    break;

                #endregion Update Item

                #region Delete Item

                case "DELETEITEM":
                    {
                        Invoke(new MethodInvoker(() =>
                        {
                            SigPadUtil.DefaultInstance.DeleteItem(index);
                        }));
                    }
                    break;

                    #endregion Delete Item
            }
        }

        //completed by sandeep
        private void DisplayItemOnSigPad()
        {
            try
            {

                if (Configuration.CPOSSet.UseSigPad == true)
                {
                    if (SigPadUtil.DefaultInstance.IsVF)
                        DeviceItemsProcess("AddItem", oPOSTrans.oTDRow, grdDetail.Rows.Count - 1);
                    else
                        SigPadUtil.DefaultInstance.AddItem(oPOSTrans.oTDRow, grdDetail.Rows.Count - 1);
                }
            }
            catch (Exception exp)
            {
                logger.Fatal(exp, "DisplayItemOnSigPad()");
                throw exp;
            }
        }

        //completed by sandeep
        private bool VerifySignatureisValid(string strSigString, string _sigType, out bool rphVerified)
        {
            rphVerified = false;
            bool ret = false;
            bool isVerifySignature = false;
            byte[] btarr = null;
            try
            {

                ret = oPOSTrans.IsSignatureValid(strSigString, _sigType, out rphVerified, out isVerifySignature, out btarr);

                if (isVerifySignature)
                {
                    frmVerifySignature ofrm = new frmVerifySignature(btarr, this.SigType);
                    ofrm.ShowDialog();
                    if (ofrm.IsSignatureRejected)
                    {
                        ofrm = null;
                        SigPadUtil.DefaultInstance.CustomerSignature = null;
                        ret = false;
                    }
                    else
                    {
                        ret = true;
                        rphVerified = true;
                    }
                }
            }
            catch (Exception exp)
            {
                logger.Fatal(exp, "VerifySignatureisValid()");
                throw exp;
            }

            return ret;
        }

        //completed by sandeep
        private bool CaptureOTCItemsSignature(ref byte[] SignBinary, ref signatureType oSignatureType, bool isPSEItem)//PRIMEPOS-3109
        {
            string strSignature = "";
            byte[] btarr = null;
            bool retVal = false;
            oPOSTrans.OTCSignDataText = "";
            // SignBinary = null;
            //oSignatureType = new signatureType();   //Sprint-24 - PRIMEPOS-2332 23-Aug-2016 JY Added
            try
            {

                #region Sprint-23 - PRIMEPOS-2321 22-Jul-2016 JY Added
                if (((Configuration.CPOSSet.PinPadModel.Trim().ToUpper() == "Windows Tablet".Trim().ToUpper() || Configuration.CPOSSet.PinPadModel.Trim().ToUpper() == "VANTIV_ISMP_WITHTOUCHSCREEN" || Configuration.CPOSSet.PinPadModel.Trim().ToUpper() == "VANTIV_LINK_2500") || Configuration.CPOSSet.IsTouchScreen) && !oPOSTrans.SkipForDelivery())//3002 //PRIMEPOS-3231N
                {
                    frmDrawSignInWPF ofrmDrawSignInWPF = new frmDrawSignInWPF();
                    ofrmDrawSignInWPF.eCalledFromScreen = eCalledFrom.ItemMonitoring;
                    bool? bResult = ofrmDrawSignInWPF.ShowDialog();
                    if (ofrmDrawSignInWPF.DialogResult.Equals(true))
                    {
                        SignBinary = ofrmDrawSignInWPF.imgData;
                        retVal = true;
                        IsOTCsignCancelled = false;

                        #region Sprint-24 - PRIMEPOS-2332 22-Aug-2016 JY Added
                        try
                        {
                            oSignatureType.mimeType = "image/png";
                            oSignatureType.Value = Convert.ToBase64String(SignBinary);
                        }
                        catch (Exception Ex1)
                        {
                            //it is optional paramter, so no need to capture exception
                        }
                        #endregion
                    }
                    else
                    {
                        retVal = false;
                        IsOTCsignCancelled = true;
                    }
                }
                #endregion
                else if (Configuration.CPOSSet.UseSigPad == true && !oPOSTrans.SkipForDelivery())
                {
                    strSignature = "";
                    if (SigPadUtil.DefaultInstance.CaptureOTcItemsSignature(oPOSTrans.strOTCItemDescriptions, isPSEItem) == true)
                    {
                        strSignature = SigPadUtil.DefaultInstance.CustomerSignature;

                        bool hasRPHVerifiedSignature;
                        bool isSignatureValid = VerifySignatureisValid(strSignature, SigPadUtil.DefaultInstance.SigType, out hasRPHVerifiedSignature);
                        if (!isSignatureValid)
                        {
                            strSignature = "";
                            retVal = CaptureOTCItemsSignature(ref btarr, ref oSignatureType, isPSEItem);
                        }
                        else if (Configuration.CPOSSet.DispSigOnTrans == true && strSignature.Trim().Length > 0 && !hasRPHVerifiedSignature)
                        {
                            #region RPH NOt VErified SIgnature
                            if (Configuration.CPOSSet.UseSigPad == true)
                            {
                                SigPadUtil.DefaultInstance.ShowCustomScreen("Validating Signature. Please wait...");
                            }
                            this.SigType = SigPadUtil.DefaultInstance.SigType;
                            frmViewSignature ofrm = new frmViewSignature(strSignature, this.SigType, true);
                            ofrm.SetMsgDetails("Validating Customer Signature...");
                            ofrm.ShowDialog();
                            if (Configuration.CPOSSet.PaymentProcessor == clsPOSDBConstants.ELAVON || Configuration.CPOSSet.PaymentProcessor == "EVERTEC")
                            {
                                strSignature = SigPadUtil.DefaultInstance.CustomerSignature;
                            }
                            if (ofrm.IsSignatureRejected == true)
                            {
                                oSignatureType = new signatureType();   //Sprint-24 - PRIMEPOS-2332 23-Aug-2016 JY Added
                                ofrm = null;
                                SigPadUtil.DefaultInstance.CustomerSignature = null;
                                strSignature = "";
                                btarr = null;
                                retVal = CaptureOTCItemsSignature(ref btarr, ref oSignatureType, isPSEItem);
                            }
                            else
                            {
                                oPOSTrans.GetSigTypeForOTCItem(strSignature, sigType, ref SignBinary, ref oSignatureType);
                                retVal = true;
                            }
                            #endregion
                        }
                        else if (Configuration.CPOSSet.DispSigOnTrans == true && strSignature.Trim().Length > 0 && hasRPHVerifiedSignature)
                        {
                            #region RPH  VErified SIgnature
                            if (Configuration.CPOSSet.UseSigPad == true)
                            {
                                SigPadUtil.DefaultInstance.ShowCustomScreen("Validating Signature. Please wait...");
                            }
                            oPOSTrans.GetSigTypeForOTCItem(strSignature, sigType, ref SignBinary, ref oSignatureType);
                            retVal = true;
                            #endregion
                        }
                        else if (Configuration.CPOSSet.DispSigOnTrans == false && strSignature.Trim().Length > 0)
                        {
                            if (Configuration.CPOSSet.PaymentProcessor == "EVERTEC" || Configuration.CPOSSet.PaymentProcessor == "ELAVON")//2664//2943
                            {
                                frmViewSignature ofrm = new frmViewSignature(strSignature, this.SigType, true);
                                ofrm.SetMsgDetails("Validating Customer Signature...");
                                strSignature = SigPadUtil.DefaultInstance.CustomerSignature;
                            }
                            this.SigType = SigPadUtil.DefaultInstance.SigType;
                            oPOSTrans.GetSigTypeForOTCItem(strSignature, sigType, ref SignBinary, ref oSignatureType);
                            retVal = true;
                        }
                        IsOTCsignCancelled = false;
                    }
                    else
                    {
                        retVal = false;
                        IsOTCsignCancelled = true;
                    }
                }
                else
                {
                    retVal = false;  //PRIMEPOS-2935 21-Jan-2021 JY Added
                }

            }
            catch (Exception exp)
            {
                logger.Fatal(exp, "CaptureOTCItemsSignature()");
                throw exp;
            }
            return retVal;
        }

        //completed by sandeep
        private bool? CaptureRXSignatureWPF(RXHeader oRXHeader)
        {
            logger.Trace("CaptureRXSignatureWPF() - " + clsPOSDBConstants.Log_Entering);
            bool? retVal = true;
            bool? rval = null;
            string patientCounceling = string.Empty;
            try
            {


                frmDrawSignInWPF ofrmDrawSignInWPF = new frmDrawSignInWPF();
                ofrmDrawSignInWPF.eCalledFromScreen = eCalledFrom.RXSign;
                rval = ofrmDrawSignInWPF.ShowDialog();
                retVal = rval;

                if (ofrmDrawSignInWPF.DialogResult.Equals(true))
                {
                    patientCounceling = ofrmDrawSignInWPF.patientCounceling;
                    oRXHeader.CounselingRequest = patientCounceling;
                    oRXHeader.bBinarySign = ofrmDrawSignInWPF.imgData;
                    this.SigType = "M";
                }
                logger.Trace("CaptureRXSignatureWPF() - " + clsPOSDBConstants.Log_Exiting);

            }
            catch (Exception exp)
            {
                logger.Fatal(exp, "CaptureRXSignatureWPF()");
                throw exp;
            }
            return retVal;
        }
        //completed by sandeep
        private bool CaptureRXSignature(RXHeader oRXHeader)
        {
            logger.Trace("CaptureRXSignature() - " + clsPOSDBConstants.Log_Entering);
            string strSignature = "";
            bool retVal = true;
            bool RxSigdone = false; // Add by Manoj 5/10/2012
            try
            {

                if (Configuration.CPOSSet.UseSigPad == true && SigPadUtil.DefaultInstance.isConnected())
                {
                    while (!RxSigdone)// Manoj 5/10/2012
                    {
                        strSignature = "";
                        string patientCounceling = "";
                        if (SigPadUtil.DefaultInstance.CaptureRXSignature(oRXHeader, out patientCounceling) == true)
                        {
                            //Modified By Rohit Nair on 08/07/2017 for PRIMEPOS-2442
                            if (SigPadUtil.DefaultInstance.isISC)
                            {
                                RxSigdone = true;
                                retVal = true;
                                bCaptureSignature = true;
                                oRXHeader.CounselingRequest = patientCounceling;
                            }
                            else if (SigPadUtil.DefaultInstance.isPAX)
                            {
                                //PRIMEPOS-2528 (Suraj) 1-june-18 Suraj - Added RXSign for HPSPAX
                                RxSigdone = true;
                                retVal = true;
                                bCaptureSignature = true;
                                oRXHeader.CounselingRequest = patientCounceling; // NileshJ - PatientCounceling Issue
                            }
                            else if (SigPadUtil.DefaultInstance.isEvertec)//PRIMEPOS-2664
                            {
                                RxSigdone = true;
                                retVal = true;
                                bCaptureSignature = true;
                                oRXHeader.CounselingRequest = patientCounceling; //PRIMEPOS-3209
                            }
                            //PRIMEPOS-2636
                            else if (SigPadUtil.DefaultInstance.isVantiv)
                            {
                                RxSigdone = true;
                                retVal = true;
                                bCaptureSignature = true;
                                oRXHeader.CounselingRequest = patientCounceling;
                            }
                            //
                            else if (SigPadUtil.DefaultInstance.isElavon)//2943
                            {
                                RxSigdone = true;
                                retVal = true;
                                bCaptureSignature = true;
                                oRXHeader.CounselingRequest = patientCounceling;
                            }
                            else
                            {
                                strSignature = SigPadUtil.DefaultInstance.CustomerSignature;
                                RxSigdone = true;
                                // Manoj 5/10/2012
                                //Added By Rohit Nair on Jan -11-2017
                                bool hasRPHVerifiedSignature;
                                bool isSignatureValid = VerifySignatureisValid(strSignature, SigPadUtil.DefaultInstance.SigType, out hasRPHVerifiedSignature);
                                if (!isSignatureValid)
                                {
                                    strSignature = "";
                                    retVal = CaptureRXSignature(oRXHeader);
                                }
                                else if (Configuration.CPOSSet.DispSigOnTrans == true && strSignature.Trim().Length > 0 && !hasRPHVerifiedSignature) //Modified By ROhit Nair 
                                {
                                    #region RPH NOt VErified SIgnature
                                    if (Configuration.CPOSSet.UseSigPad == true)
                                    {
                                        SigPadUtil.DefaultInstance.ShowCustomScreen("Validating Signature. Please wait...");
                                    }
                                    this.SigType = SigPadUtil.DefaultInstance.SigType;
                                    //  Mantis Id : 0000119 Modified By Dharmendra (SRT) on Dec-02-08 passed extra parameter true
                                    frmViewSignature ofrm = new frmViewSignature(strSignature, this.SigType, true);
                                    ofrm.SetMsgDetails("Validating RX Signature...");
                                    ofrm.ShowDialog();
                                    if (ofrm.IsSignatureRejected == true)
                                    {
                                        ofrm = null;
                                        SigPadUtil.DefaultInstance.CustomerSignature = null;//Added By Dharmendra on Dec-15-08 to prevent from illegal flow continuetion
                                        strSignature = "";
                                        retVal = CaptureRXSignature(oRXHeader);
                                    }
                                    //Modified By Dharmendra (SRT) on Dec-15-08 to restrict the pos screen navigation to paymentlist
                                    else
                                    {
                                        oPOSTrans.GetRXHeaderForSiganture(sigType, strSignature, patientCounceling, ref oRXHeader);
                                        retVal = true;
                                    }
                                    #endregion

                                }
                                else if (Configuration.CPOSSet.DispSigOnTrans == true && strSignature.Trim().Length > 0 && hasRPHVerifiedSignature) //added By ROhit Nair 
                                {
                                    #region RPH  VErified SIgnature
                                    if (Configuration.CPOSSet.UseSigPad == true)
                                    {
                                        SigPadUtil.DefaultInstance.ShowCustomScreen("Validating Signature. Please wait...");
                                    }
                                    this.SigType = SigPadUtil.DefaultInstance.SigType;
                                    oPOSTrans.GetRXHeaderForSiganture(sigType, strSignature, patientCounceling, ref oRXHeader);
                                    retVal = true;
                                    #endregion

                                }
                                //Modified By Dharmendra on Apr-08-09 to assign the values of
                                //rx approval signature and councelling to the variables
                                // oRXHeader.RXSignature and oRXHeader.CounselingRequest
                                //even if the value of Configuration.CPOSSet.DispSigOnTrans is false
                                else if (Configuration.CPOSSet.DispSigOnTrans == false && strSignature.Trim().Length > 0)
                                {
                                    if (Configuration.CPOSSet.PaymentProcessor == "EVERTEC")//2664
                                    {
                                        frmViewSignature ofrm = new frmViewSignature(strSignature, this.SigType, true);
                                        ofrm.SetMsgDetails("Validating Customer Signature...");
                                        strSignature = SigPadUtil.DefaultInstance.CustomerSignature;
                                    }
                                    this.SigType = SigPadUtil.DefaultInstance.SigType;
                                    oPOSTrans.GetRXHeaderForSiganture(sigType, strSignature, patientCounceling, ref oRXHeader);
                                    retVal = true;
                                }
                            }

                        }
                        else
                        {
                            RxSigdone = true;
                            retVal = false;
                        }
                    }
                }
                //Logs.Logger(clsPOSDBConstants.Log_Module_Transaction, "CaptureRXSignature()", clsPOSDBConstants.Log_Exiting);
                logger.Trace("CaptureRXSignature() - " + clsPOSDBConstants.Log_Exiting);

            }
            catch (Exception exp)
            {
                logger.Fatal(exp, "CaptureRXSignature()");
                throw exp;
            }
            return retVal;
        }

        //completed by sandeep
        private bool? CaptureNOPPSignatureWPF(RXHeader oRXHeader, bool signLater = false)
        {
            logger.Trace("CaptureNOPPSignatureWPF() - " + clsPOSDBConstants.Log_Entering);
            bool? retVal = true;
            bool? rval = null;
            try
            {
                if (oPOSTrans.CurrentTransactionType == POS_Core.TransType.POSTransactionType.SalesReturn)//3013
                {
                    return true;
                }
                frmDrawSignInWPF ofrmDrawSignInWPF = new frmDrawSignInWPF(oRXHeader.PatientName, oRXHeader.PatientAddr);
                ofrmDrawSignInWPF.eCalledFromScreen = eCalledFrom.HIPPASign;
                if (signLater == true)
                    ofrmDrawSignInWPF.eCalledFromScreen = eCalledFrom.HIPPATextOnly;
                rval = ofrmDrawSignInWPF.ShowDialog();
                retVal = rval;
                if (ofrmDrawSignInWPF.DialogResult.Equals(true))
                {
                    oRXHeader.NoppBinarySign = ofrmDrawSignInWPF.imgData;
                    this.SigType = "M"; //SigPadUtil.DefaultInstance.SigType;
                    oRXHeader.NOPPStatus = "Y";
                    oRXHeader.NOPPSignature = string.Empty;
                    oRXHeader.PrivacyText = Configuration.CInfo.PrivacyText; //needs to get from device
                }
                else if (rval == false)
                {
                    oRXHeader.NOPPStatus = "N";
                    oRXHeader.NOPPSignature = "";
                    oRXHeader.PrivacyText = "";
                }
                else if (rval == null)
                {
                    oRXHeader.NOPPStatus = null;
                    oRXHeader.NOPPSignature = "";
                    oRXHeader.PrivacyText = "";
                }
                logger.Trace("CaptureNOPPSignatureWPF() - " + clsPOSDBConstants.Log_Exiting);

            }
            catch (Exception exp)
            {
                logger.Fatal(exp, "CaptureNOPPSignatureWPF()");
                throw exp;
            }
            return retVal;
        }


        #endregion

        #region TerminalEntry
        //completed by sandeep
        private void stTBTerminalEntery_ButtonClick(object sender, EventArgs e)
        {
            if (sender.GetType() == typeof(Infragistics.Win.Misc.UltraLabel))
            {
                tbTerminalEnteryActions(((Infragistics.Win.Misc.UltraLabel)sender).Text.Trim().ToString());
            }
            else
            {
                tbTerminalEnteryActions(((Infragistics.Win.Misc.UltraButton)sender).Tag.ToString());
            }
        }

        #endregion

        #region TerminalEntryMethod

        //completed by sandeep
        public void ShowMainDialogEA()
        {
            frmSearchMain oMainSearch = new frmSearchMain();
            oMainSearch.SearchTable = clsPOSDBConstants.item_PriceInv_Lookup;
            oMainSearch.isReadonly = true;
            oMainSearch.DisplayRecordAtStartup = false;
            oMainSearch.LabelText1 = "Item Co&de";  //Sprint-26 - PRIMEPOS-414 29-Jun-2017 JY Added
            oMainSearch.LabelText2 = "Item &Name";  //Sprint-26 - PRIMEPOS-414 29-Jun-2017 JY Added
            oMainSearch.StartPosition = FormStartPosition.CenterParent;
            oMainSearch.ShowDialog();
        }

        //completed by sandeep
        public void PutOnHoldEA()
        {
            if (UserPriviliges.getPermission(UserPriviliges.Modules.POSTransaction.ID, UserPriviliges.Screens.POSTransaction.ID, UserPriviliges.Permissions.OnHoldTrans.ID, UserPriviliges.Permissions.OnHoldTrans.Name))
            {
                #region Sprint-26 - PRIMEPOS-417 25-Jul-2017 JY Added - user can't hold invoice if customer is blank
                if (string.IsNullOrWhiteSpace(txtCustomer.Text.Trim()))
                {
                    Resources.Message.Display("Please select customer", "PrimePOS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    if (txtCustomer.Enabled)
                        txtCustomer.Focus();
                    return;
                }
                else
                {
                    if (string.IsNullOrWhiteSpace(txtCustomer.Tag.ToString().Trim()))
                        txtCustomer_Leave(null, null);
                }
                #endregion
                PutOnHold("");
            }
        }

        //completed by sandeep
        private void tbTerminalEnteryActions(string strKey)
        {
            try
            {
                logger.Trace("tbTerminalEnteryActions() - " + strKey + " - " + clsPOSDBConstants.Log_Entering);
                #region strtbterminalentry
                //"F3=Price/Inv. Check, Ctrl+L = Lock Register, Ctrl+N No Sale";
                if (strKey == "F3")
                {
                    ShowMainDialogEA();
                }
                else if (strKey == "F5")
                {
                    ApplyDiscEA();
                }
                else if (strKey == "Ctrl+L")
                {
                    clsUIHelper.LocakStation();

                }
                else if (strKey == "Ctrl+N")
                {
                    string UserId = string.Empty;//PRIMEPOS-2808
                    if (UserPriviliges.getPermission(UserPriviliges.Modules.POSTransaction.ID, UserPriviliges.Screens.POSTransaction.ID, UserPriviliges.Permissions.NoSaleTrans.ID, UserPriviliges.Permissions.NoSaleTrans.Name, out UserId))
                    {
                        NoSaleTransaction noSaleTransaction = new NoSaleTransaction();
                        noSaleTransaction.SetNoSaleTransaction(UserId, Configuration.StationID);

                        RxLabel oRX = new RxLabel(null, null, null, ReceiptType.Void, null);
                        oRX.OpenDrawer();
                        Resources.Message.Display("Cash drawer is open \n Please close the drawer and press enter", "Cash Drawer", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else if (strKey == "Ctrl+H")
                {
                    PutOnHoldEA();
                }
                //Added Shitaljit(QuicSolv)on 6 May 2011
                else if (strKey == "Ctrl+T")
                {
                    btnCancelTrans.Focus();

                    taxOverrideAll();
                    txtItemCode.Focus();
                }
                // Till here Added By Shitaljit(QuicSolv)
                else if (strKey == "Ctrl+C")
                {
                    #region Clear All

                    //Added By Shitaljit on 16 May 2012
                    if (this.grdDetail.Rows.Count > 0)
                    {
                        if (UserPriviliges.getPermission(UserPriviliges.Modules.POSTransaction.ID, UserPriviliges.Screens.POSTransaction.ID, UserPriviliges.Permissions.DeleteTrans.ID, UserPriviliges.Permissions.DeleteTrans.Name))
                        {
                            if (Resources.Message.Display("Are you sure, you want to clear the screen?", "Clear screen", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                            {
                                #region PRIMEPOS-3207
                                clearHyphenAlert();
                                #endregion
                                SetNew(false);
                                if (Configuration.CPOSSet.UsePoleDisplay)
                                {
                                    frmMain.PoleDisplay.ClearPoleDisplay();
                                }
                                clsUIHelper.ShowWelcomeMessage();
                            }
                        }
                    }
                    else
                    {
                        #region PRIMEPOS-3207
                        clearHyphenAlert();
                        #endregion
                        SetNew(false);
                        this.txtItemCode.Focus();
                    }

                    #endregion Clear All
                }
                //Added to add coupon in transaction
                else if (strKey == "F6")
                {
                    AddCouponEA();
                }

                #endregion strtbterminalentry
                logger.Trace("tbTerminalEnteryActions() - " + strKey + " - " + clsPOSDBConstants.Log_Exiting);
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "tbTerminalEnteryActions() - " + strKey);
                clsUIHelper.ShowErrorMsg(ex.Message);
            }
        }

        //completed by sandeep
        private void SetTransactionHeaderRow()
        {

            oPOSTrans.oTransHRow = oPOSTrans.oTransHData.TransHeader[0];
            oPOSTrans.oTransHRow.CustomerID = Convert.ToInt32(this.txtCustomer.Tag);
            oPOSTrans.oTransHRow.TransDate = (System.DateTime)this.dtpTransDate.Value;
            oPOSTrans.oTransHRow.TransType = Convert.ToInt32(oPOSTrans.CurrentTransactionType);
            oPOSTrans.oTransHRow.GrossTotal = Convert.ToDecimal(this.txtAmtSubTotal.Text);
            oPOSTrans.oTransHRow.TotalTaxAmount = Convert.ToDecimal(this.txtAmtTax.Text);
            oPOSTrans.oTransHRow.TotalDiscAmount = Convert.ToDecimal(this.txtAmtDiscount.Text);
            oPOSTrans.oTransHRow.TotalPaid = Convert.ToDecimal(this.txtAmtTotal.Text) + TransFeeAmt;    //PRIMEPOS-3117 11-Jul-2022 JY modified
            oPOSTrans.oTransHRow.TenderedAmount = Convert.ToDecimal(this.txtAmtTendered.Value);
            oPOSTrans.oTransHRow.stClosedID = 0;
            oPOSTrans.oTransHRow.isStationClosed = 0;
            oPOSTrans.oTransHRow.isEOD = 0;
            oPOSTrans.oTransHRow.EODID = 0;
            oPOSTrans.oTransHRow.DrawerNo = Configuration.DrawNo;
            oPOSTrans.oTransHRow.TransID = onHoldTransID;
            oPOSTrans.oTransHRow.StationID = Configuration.StationID;
            oPOSTrans.oTransHRow.InvoiceDiscount = Configuration.convertNullToDecimal(this.lblInvDiscount.Text);//Added By Shitaljit(QuicSolv) on 1 Sept 2011
            oPOSTrans.oTransHRow.WasonHold = true;    //Sprint-24 - PRIMEPOS-2342 17-Oct-2016 JY Added
            oPOSTrans.oTransHRow.DeliverySigSkipped = Configuration.CPOSSet.SkipDelSign;  //Sprint-24 - PRIMEPOS-2342 17-Oct-2016 JY Added
            oPOSTrans.oTransHRow.IsCustomerDriven = this.IsCustomerDriven;  //2915
            oPOSTrans.oTransHRow.TotalTransFeeAmt = oPOSTrans.CalculateTotalTransFeeAmt(oPOSTrans.oPOSTransPaymentData);  //PRIMEPOS-3117 11-Jul-2022 JY Added
        }
        //completed by sandeep
        private void PutOnHold(string Action)
        {
            logger.Trace("PutOnHold() - " + clsPOSDBConstants.Log_Entering);
            CustomerRow oCRow = null;

            string strCode = string.Empty;

            try
            {
                DialogResult result = DialogResult.Cancel;
                if (string.IsNullOrEmpty(Action) == true)
                {
                    result = Resources.Message.DisplayLocal("Do you want to hold this transaction for Delivery?", "Transaction on Hold", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning, "Hold For &Delivery", "&Hold", "&Cancel");
                }
                else if (Action.Equals("D"))
                {
                    result = DialogResult.Yes;
                }
                if (result == DialogResult.Cancel && string.IsNullOrEmpty(Action) == true)
                {
                    return;
                }

                #region PRIMEPOS-3342
                if (lstOnHoldRxs.Count > 0)
                {
                    for (int i = 0; i < oPOSTrans.oTransDRXData.Tables[0].Rows.Count; i++)
                    {
                        foreach (OnholdRxs objOnholdRxs in lstOnHoldRxs)
                        {
                            if (Convert.ToString(objOnholdRxs.RxNo) == Convert.ToString(oPOSTrans.oTransDRXData.Tables[0].Rows[i]["RXNo"]))
                            {
                                Resources.Message.Display("The Rx :" + Convert.ToString(objOnholdRxs.RxNo) + " is currently on hold; please note that you cannot put it on hold again.", Configuration.ApplicationName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return;
                            }
                        }

                    }
                }
                #endregion

                SetTransactionHeaderRow();

                if (result == DialogResult.Yes || oPOSTrans.oTransHRow.IsDelivery) //PRIMEPOS-3494
                {
                    oPOSTrans.oTransHRow.IsDelivery = true;
                    if (oPOSTrans.oRXHeaderList.Count > 0)
                    {
                        oCRow = oPOSTrans.GetCustomerRowFromRxHeaderList();
                    }
                    else
                    {
                        oCRow = SearchCustomerForOnHoldTrans();
                        if (oCRow == null)
                        {
                            return;
                        }
                        else
                        {
                            if (oCRow.PatientNo > 0)
                            {
                                oPOSTrans.oTransHRow.DeliveryAddress = PrimeRXHelper.GetPatientDeliveryAddress(oCRow.PatientNo.ToString());
                            }
                            else
                            {
                                oPOSTrans.oTransHRow.DeliveryAddress = oCRow.GetAddress();
                            }
                        }
                    }
                    //Added By Shitaljit on 8 Arpil 2013 for PRIMEPOS 408 wrong customer is save for on Hold Trans.
                    if (oCRow != null)
                    {
                        strCode = oCRow.CustomerId.ToString();
                        EditCustomer(strCode, clsPOSDBConstants.Customer_tbl);
                        oPOSTrans.oTransHRow.CustomerID = oCRow.CustomerId;
                    }
                    else
                    {
                        ErrorHandler.throwCustomError(POSErrorENUM.TransHeader_CustomerIDCanNotNull);
                    }
                    //Till here is added by shitaljit
                }
                else
                {
                    oPOSTrans.oTransHRow.IsDelivery = false;
                }
                oPOSTrans.oTransHRow.AllowRxPicked = Configuration.AllowRxPicked;   //PRIMEPOS-2865 16-Jul-2020 JY Added
                oPOSTrans.oTransHRow.RxTaxPolicyID = Configuration.convertNullToInt(Configuration.CSetting.RxTaxPolicy.Trim()); //PRIMEPOS-3053 08-Feb-2021 JY Added

                if (Action == "P")//2915
                    oPOSTrans.PutOnHold(oPOSTrans.oTransHData, oPOSTrans.oTransDData, oPOSTrans.oTransDRXData, oPOSTrans.oTDTaxData, oPOSTrans.oPOSTransPaymentData);
                else
                    oPOSTrans.PutOnHold(oPOSTrans.oTransHData, oPOSTrans.oTransDData, oPOSTrans.oTransDRXData, oPOSTrans.oTDTaxData);
                if (Configuration.CPOSSet.UseSigPad == true)
                {
                    SigPadUtil.DefaultInstance.AbortTransaction();
                }
                try
                {
                    DataTable dtTransDetailTax = oPOSTrans.GetTransDetailTaxOnHold(Configuration.convertNullToInt(((TransHeaderRow)oPOSTrans.oTransHData.TransHeader.Rows[0]).TransID)); //Sprint-26 - PRIMEPOS-2445 28-Aug-2017 JY Added
                    RxLabel oRxLabel = new RxLabel(oPOSTrans.oTransHData, oPOSTrans.oTransDData, oPOSTrans.oPOSTransPaymentData, true, ReceiptType.OnHoldTransction, dtTransDetailTax);
                    Application.DoEvents();
                    if (oPOSTrans.isPrintAble(oPOSTrans.oTransDData))
                    {
                        PrintReceiptForOnHoldTransaction(oRxLabel);
                    }
                }
                catch
                {
                    //	clsUIHelper.ShowErrorMsg(pExp.Message);
                }
                SetNew(true);
                clsUIHelper.ShowWelcomeMessage();
            }
            catch (POSExceptions exp)
            {
                logger.Fatal(exp, "PutOnHold()");
                clsUIHelper.ShowErrorMsg(exp.ErrMessage);
                switch (exp.ErrNumber)
                {
                    case (long)POSErrorENUM.TransHeader_CustomerIDCanNotNull:
                        this.txtCustomer.Focus();
                        break;
                    case (long)POSErrorENUM.TransHeader_DateIsInvalid:
                        this.dtpTransDate.Focus();
                        break;
                }
            }
            catch (Exception exp)
            {
                logger.Fatal(exp, "PutOnHold()");
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
            logger.Trace("PutOnHold() - " + clsPOSDBConstants.Log_Exiting);
        }

        //completed by sandeep
        private void PrintReceiptForOnHoldTransaction(RxLabel oRxLabel)
        {
            logger.Trace("PrintReceiptForOnHoldTransaction() - " + clsPOSDBConstants.Log_Entering);
            if (Configuration.convertNullToDouble(oPOSTrans.oTransHData.Tables[0].Rows[0]["TotalPaid"]) == 0.00 && Configuration.CInfo.AllowPrintZeroTrans == false)
            {
                logger.Trace("PrintReceiptForOnHoldTransaction() - " + clsPOSDBConstants.Log_Exiting);
                return;
            }
            else if (Configuration.CInfo.PrintReceiptForOnHoldTrans == PrintReciepForOnHoldTransaction.Yes)
            {
                oRxLabel.Print();
                Application.DoEvents();
            }
            else if (Configuration.CInfo.PrintReceiptForOnHoldTrans == PrintReciepForOnHoldTransaction.Verify)
            {
                if (Resources.Message.Display("Do you want to print this transaction?", "Print Receipt", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                {
                    oRxLabel.Print();
                    Application.DoEvents();
                }
            }
            logger.Trace("PrintReceiptForOnHoldTransaction() - " + clsPOSDBConstants.Log_Exiting);
        }


        #endregion

        #region Terminal
        //completed by sandeep
        private void stTBTerminal_ButtonClick(object sender, EventArgs e)
        {
            if (sender.GetType() == typeof(Infragistics.Win.Misc.UltraLabel))
            {
                tbTerminalActions(((Infragistics.Win.Misc.UltraLabel)sender).Text.Trim().ToString());
            }
            else
            {
                tbTerminalActions(((Infragistics.Win.Misc.UltraButton)sender).Tag.ToString());
            }
        }

        #endregion

        #region TerminalMethod

        //completed by sandeep
        private void DeviceItemsProcess(string ProcessName, DataTable dt)
        {
            //Recalculate the price
            #region Recalculate the Sum
            ultraCalcManager1.ReCalc();
            //Update the pad sum
            UpdateAmounts();
            #endregion Recalculate the Sum
            switch (ProcessName.ToUpper())
            {
                #region On Hold Items to Display
                case "ONHOLDITEM":
                    {
                        Invoke(new MethodInvoker(() =>
                        {
                            SigPadUtil.DefaultInstance.SendOnHoldItems(dt);
                        }));
                        Thread.Sleep(10);
                    }
                    break;
                    #endregion On Hold Items to Display
            }
        }

        //completed by sandeep
        private void tbTerminalActions(string strKey)
        {
            logger.Trace("tbTerminalActions() - " + strKey + " - " + clsPOSDBConstants.Log_Entering);
            PharmBL oPharmBL = new PharmBL();
            string RXList = string.Empty;
            try
            {
                #region strtbterminal

                if (strKey == "F2")
                {
                    if (oPOSTrans.CurrentTransactionType == POS_Core.TransType.POSTransactionType.Sales && Configuration.CSetting.StrictReturn == false)//PRIMEPOS-2738 ADDED BY ARVIND FOR STRICT RETURN 
                    {
                        SetNew(false);
                        this.setTransactionType(POS_Core.TransType.POSTransactionType.SalesReturn);
                    }
                    #region PRIMEPOS-2738
                    else if (oPOSTrans.CurrentTransactionType == POS_Core.TransType.POSTransactionType.Sales && Configuration.CSetting.StrictReturn == true)
                    {
                        SetNew(false);
                        this.setTransactionType(POS_Core.TransType.POSTransactionType.SalesReturn);
                        //this.Cursor = Cursors.WaitCursor;
                        frmSelectTransaction ofrmVSR = new frmSelectTransaction(CopyRxInReturnTrans.ToString());
                        ofrmVSR.ShowDialog();
                        //this.Cursor = Cursors.Default;
                        //this.Cursor = Cursors.Default;
                        if (ofrmVSR.isClosed == true)
                        {
                            SetNew(false);
                            this.setTransactionType(POS_Core.TransType.POSTransactionType.Sales);
                        }
                        if (ofrmVSR.isCopied == true)
                        {
                            CopySelectReturnTransaction(ofrmVSR);
                        }
                        if (ofrmVSR.isViewTransaction)
                        {
                            frmViewTransactionDetail ofrmVTD = new frmViewTransactionDetail(ofrmVSR.TransID, "", Configuration.StationID, true);
                            ofrmVTD.ShowDialog();

                            if (ofrmVTD.isClosed == true)
                            {
                                SetNew(false);
                                this.setTransactionType(POS_Core.TransType.POSTransactionType.Sales);
                            }
                            else if (ofrmVTD.isCopied == true)
                            {
                                //CopyTransaction(ofrmVTD); //PRIMEPOS-2773 30-Dec-2019 JY Commented
                                CopyViewTransaction(ofrmVTD);   //PRIMEPOS-2773 30-Dec-2019 JY copied from release branch
                            }
                            //this.Cursor = Cursors.Default;
                        }
                        if (ofrmVSR.isReturnCash)
                        {
                            //local variable = true
                            //Add items
                            isStrictReturn = false;

                        }
                        this.txtItemCode.Text = "";
                    }
                    //ProcessTransactionViewRequest
                    #endregion
                    else
                    {
                        SetNew(false);
                        this.setTransactionType(POS_Core.TransType.POSTransactionType.Sales);
                    }
                }
                else if (strKey == "F3")
                {
                    frmSearchMain oMainSearch = new frmSearchMain();
                    oMainSearch.FormCaption = "Item File Lookup";
                    oMainSearch.LabelText1 = "Item Code";
                    oMainSearch.LabelText2 = "Description";

                    oMainSearch.btnFillPartialOrder.Visible = false;
                    oMainSearch.btnCopyOrder.Visible = false;
                    oMainSearch.btnAckManual.Visible = false;

                    oMainSearch.SearchTable = clsPOSDBConstants.item_PriceInv_Lookup;
                    oMainSearch.isReadonly = true;
                    oMainSearch.DisplayRecordAtStartup = false;
                    oMainSearch.ShowDialog();
                }
                else if (strKey == "Ctrl+P")
                {
                    if (UserPriviliges.getPermission(UserPriviliges.Modules.POSTransaction.ID, UserPriviliges.Screens.POSTransaction.ID, UserPriviliges.Permissions.OnHoldTrans.ID, UserPriviliges.Permissions.OnHoldTrans.Name))
                    {
                        Boolean bIsProcessUnbilled = false; //PRIMEPOS-3163 30-Dec-2022 JY Added
                        frmPOSOnHoldTrans oOnHoldTrans = new frmPOSOnHoldTrans();
                        oOnHoldTrans.FormCaption = "OnHold Transactions";
                        oOnHoldTrans.DisplayRecordAtStartup = false;
                        oOnHoldTrans.ShowDialog();

                        if (oOnHoldTrans.IsCanceled == false)
                        {
                            if (oOnHoldTrans.TransID < 1)
                                return;
                            SetNew(false);
                            Int32 TransID = oOnHoldTrans.TransID;
                            onHoldTransID = TransID;
                            isOnHoldTrans = true; //Added By shitaljit(QuicSolv) on 10 nov 2011

                            //Get Transaction Data
                            TransDetailData oHoldTransDData;
                            TransHeaderData oHoldTransHData = oPOSTrans.GetOnHoldTransData(TransID, out oHoldTransDData);

                            #region PRIMEPOS-3163 30-Dec-2022 JY Added
                            try
                            {
                                if (Configuration.CPOSSet.FetchUnbilledRx == 2)
                                {
                                    string strRxNos = string.Empty;
                                    foreach (TransDetailRow oRow in oHoldTransDData.TransDetail.Rows)
                                    {
                                        if (oRow.ItemID.ToUpper() == "RX")
                                        {
                                            if (oRow.ItemDescription.IndexOf("-") > 0)
                                            {
                                                using (DataTable oRxInfo = oPOSTrans.GetRxInfo(oRow.ItemDescription))
                                                {
                                                    if ((oRxInfo != null) && oRxInfo.Rows.Count > 0)
                                                    {
                                                        if (oRxInfo.Rows[0]["Status"].ToString().ToUpper() == "U")
                                                        {
                                                            if (strRxNos == string.Empty)
                                                            {
                                                                strRxNos = oRxInfo.Rows[0]["Rxno"].ToString() + "-" + oRxInfo.Rows[0]["nrefill"].ToString();
                                                            }
                                                            else
                                                            {
                                                                strRxNos += ", " + oRxInfo.Rows[0]["Rxno"].ToString() + "-" + oRxInfo.Rows[0]["nrefill"].ToString();
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    if (strRxNos != string.Empty)
                                    {
                                        if (Resources.Message.Display(strRxNos + " - Rx(s) are unbilled, do you wish to process it?", "Process UnBilled Rx", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                                        {
                                            bIsProcessUnbilled = true;
                                        }
                                    }
                                }
                            }
                            catch (Exception Ex)
                            {
                                logger.Fatal(Ex, "tbTerminalActions(string strKey) - Ctrl + P - 2");
                            }
                            #endregion

                            oPOSTrans.oTransHRow = oHoldTransHData.TransHeader[0];
                            TransStartDateTime = oPOSTrans.oTransHRow.TransactionStartDate.ToString();//Added By Shitaljit(QuicSolv) on 31 Oct 2011
                            if (oHoldTransHData.TransHeader[0].TransType == 1)
                                this.setTransactionType(POS_Core.TransType.POSTransactionType.Sales);
                            else
                                this.setTransactionType(POS_Core.TransType.POSTransactionType.SalesReturn);

                            this.txtCustomer.Text = oHoldTransHData.TransHeader[0].CustomerID.ToString();
                            //this.txtCustomer.Tag = oHoldTransHData.TransHeader[0].CustomerID.ToString();  //PRIMEPOS-2536 22-May-2019 JY Added
                            this.lblCustomerName.Text = oHoldTransHData.TransHeader[0].CustomerName.ToString();
                            ShowCustomerTokenImage(oHoldTransHData.TransHeader[0].CustomerID);   //PRIMEPOS-2611 13-Nov-2018 JY Added
                            this.lblInvDiscount.Text = Convert.ToString(oHoldTransHData.TransHeader[0].TotalDiscAmount);
                            this.dtpTransDate.Value = oHoldTransHData.TransHeader[0].TransDate;
                            // Added By Ravindra (QuicSolv) to get cutomerDiscount Detail for hold Transaction
                            if (txtCustomer.Text.Length > 0)
                            {
                                SearchCustomer(true);
                            }
                            else
                            {
                                ClearCustomer();
                            }
                            grdDetail.Refresh();
                            RXList = string.Empty;
                            oPharmBL = new PharmData.PharmBL();
                            ArrayList arrPatients = new ArrayList();    //PRIMEPOS-2536 22-May-2019 JY Added
                            bool bPatientCounselingPrompt = false;  //PRIMEPOS-2461 23-Feb-2021 JY Added
                            int rowCount = 0; //PRIMEPOS-3446
                            foreach (TransDetailRow oRow in oHoldTransDData.TransDetail.Rows)
                            {
                                rowCount++; //PRIMEPOS-3446
                                //this.oTransDData.TransDetail.Rows.Add(oRow.ItemArray);
                                bool ExceptionFlag = false;
                                if (oRow.ItemID.ToUpper() == "RX")
                                {
                                    if (oRow.ItemDescription.IndexOf("-") > 0)
                                    {
                                        oPOSTrans.oTDRow = oRow;
                                        oPOSTrans.oTDRow.Category = "F";//Added By Shitaljit on 15 march 2012 to marked FSA item
                                        try
                                        {
                                            DataTable oRxInfo = oPOSTrans.GetRxInfo(oRow.ItemDescription);
                                            //Following if else and else part code is Added By Shitaljit(QuicSolv) on 9 Nov 2011
                                            if ((oRxInfo != null) && oRxInfo.Rows.Count > 0)
                                            {
                                                if ((oRxInfo.Rows[0]["Pickedup"].ToString() == "Y" || oRxInfo.Rows[0]["PickupPOS"].ToString() == "Y"))
                                                    FillRXInformation(oPOSTrans.ExtractRXInfoFromDescription(oRow.ItemDescription), false, rowCount, bIsProcessUnbilled); //PRIMEPOS-3163 30-Dec-2022 JY Added bIsProcessUnbilled parameter //PRIMEPOS-3446 Added rowCount parameter
                                                else
                                                    FillRXInformation(oPOSTrans.ExtractRXInfoFromDescription(oRow.ItemDescription), true, rowCount, bIsProcessUnbilled);  //PRIMEPOS-3163 30-Dec-2022 JY Added bIsProcessUnbilled parameter //PRIMEPOS-3446 Added rowCount parameter

                                                #region PRIMEPOS-2536 22-May-2019 JY Added logic to show RX and Patient notes from PrimeRX.
                                                ShowPrimeRXPOSNotes(Configuration.convertNullToString(oRxInfo.Rows[0]["RXNO"]), ref arrPatients);
                                                #endregion Show RX and Patient notes from PrimeRX.

                                                if (!PrimeRxPatientCounselingAudited())
                                                {
                                                    #region PRIMEPOS-2461 23-Feb-2021 JY Added
                                                    try
                                                    {
                                                        if (bPatientCounselingPrompt == false && (Configuration.CSetting.PatientCounselingPrompt == "1" || Configuration.CSetting.PatientCounselingPrompt == "2"))
                                                        {
                                                            bPatientCounselingPrompt = true;
                                                            Resources.Message.Display("Please provide counseling for the prescription(s).", "PrimePOS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                                        }
                                                    }
                                                    catch (Exception Ex)
                                                    {
                                                    }
                                                    #endregion
                                                }
                                            }
                                            else
                                            {
                                                ErrorHandler.throwCustomError(POSErrorENUM.TransDetail_RXNotFound);
                                            }
                                        }
                                        catch (Exception e)
                                        {
                                            logger.Fatal(e, "tbTerminalActions()");
                                            RXList = RXList + "\n" + e.Message;
                                            ExceptionFlag = true;
                                            throw e; //PRIMEPOS-3340
                                        }
                                    }
                                }
                                if (ExceptionFlag == false)
                                {
                                    #region PRIMEPOS-2536 13-May-2019 JY Added
                                    if (Configuration.CPOSSet.ShowItemNotes == true)
                                    {
                                        logger.Trace("tbTerminalActions(string strKey)" + Configuration.convertNullToString(oRow.ItemID));
                                        ShowItemNotes(Configuration.convertNullToString(Configuration.convertNullToString(oRow.ItemID)));
                                    }
                                    #endregion
                                    this.oPOSTrans.oTransDData.TransDetail.Rows.Add(oRow.ItemArray);
                                }

                                this.lblInvDiscount.Text = Convert.ToString(Convert.ToDecimal(this.lblInvDiscount.Text) - oRow.Discount);
                            }
                            TransDetailRXSvr.ProcOnHoldFlag = false;
                            txtItemCode.Text = "";
                            if (RXList != string.Empty)
                            {
                                if (!(isOnHoldTrans && Configuration.CPOSSet.FetchUnbilledRx == 2))  //PRIMEPOS-3163 30-Dec-2022 JY Added
                                    clsUIHelper.ShowErrorMsg(RXList);
                                RXList = string.Empty;
                            }
                            oPOSTrans.oTDRow = null;

                            this.grdDetail.Refresh();
                            if (this.grdDetail.Rows.Count > 0)
                            {
                                this.changeStToolbars(TransactionStToolbars.strTBTerminalEntery);
                            }
                            else
                            {
                                this.changeStToolbars(TransactionStToolbars.strTBTerminal);
                            }
                            oPOSTrans.oTransHData.AcceptChanges();
                            this.isAddRow = false;
                            this.isEditRow = false;
                            //Change by Manoj 1/14/2014

                            if (Configuration.CPOSSet.UseSigPad == true && oPOSTrans.oTransDData.Tables[0].Rows.Count > 0 && !SigPadUtil.DefaultInstance.IsVF && !SigPadUtil.DefaultInstance.isPAX && !SigPadUtil.DefaultInstance.isVantiv)//PRIMEPOS-2636
                            {
                                SigPadUtil.DefaultInstance.SendOnHoldItemListOnHHP(oPOSTrans.oTransDData, onHoldTransID);
                                SigPadUtil.DefaultInstance.itemonHoldID(onHoldTransID); // added by Manoj 10/31/2011
                            }
                            else if (Configuration.CPOSSet.UseSigPad == true && oPOSTrans.oTransDData.Tables[0].Rows.Count > 0 && SigPadUtil.DefaultInstance.IsVF)
                            {
                                DeviceItemsProcess("OnHoldItem", oPOSTrans.oTransDData.Tables[0]);
                                SigPadUtil.DefaultInstance.itemonHoldID(onHoldTransID);
                            }
                            else if (Configuration.CPOSSet.UseSigPad == true && oPOSTrans.oTransDData.Tables[0].Rows.Count > 0 && SigPadUtil.DefaultInstance.isPAX && SigPadUtil.DefaultInstance.isVantiv)//Added by Arvind PRIMEPOS-2636
                            {
                                //PRIMEPOS-2528 (Suraj) 31-may-18 PAX
                                DeviceItemsProcess("OnHoldItem", oPOSTrans.oTransDData.Tables[0]);
                                SigPadUtil.DefaultInstance.itemonHoldID(onHoldTransID);
                            }
                            else if (Configuration.CPOSSet.UseSigPad == true && oPOSTrans.oTransDData.Tables[0].Rows.Count > 0 && SigPadUtil.DefaultInstance.isPAX) //PRIMEPOS-2952
                            {
                                DeviceItemsProcess("OnHoldItem", oPOSTrans.oTransDData.Tables[0]);
                                SigPadUtil.DefaultInstance.itemonHoldID(onHoldTransID);
                            }
                        }
                        else if (oOnHoldTrans.IsPendingPayment)//2915
                        {
                            try
                            {
                                logger.Trace("Entering in Pending Payment Click");
                                TransDetailData oHoldTransDData;
                                TransHeaderSvr oTransHeaderSvr = new TransHeaderSvr();
                                TransDetailRXSvr oTransDetailRxSvr = new TransDetailRXSvr();
                                TransDetailTaxSvr oTDTaxSvr = new TransDetailTaxSvr();
                                POSTransPaymentSvr oPosTransPaymentSvr = new POSTransPaymentSvr();
                                this.IsCustomerDriven = true;
                                if (oOnHoldTrans.TransID < 1)
                                    return;
                                //SetNew(false);
                                Int32 TransID = oOnHoldTrans.TransID;
                                onHoldTransID = TransID;
                                isOnHoldTrans = true;

                                ArrayList arrPatients = new ArrayList();    //PRIMEPOS-2536 22-May-2019 JY Added

                                //Get Transaction Data
                                TransHeaderData oHoldTransHData = oPOSTrans.oTransHData = oPOSTrans.GetOnHoldTransData(TransID, out oHoldTransDData, true);
                                oPOSTrans.oTransHRow = oHoldTransHData.TransHeader[0];
                                oPOSTrans.oTransDData = oHoldTransDData;
                                oPOSTrans.oTransDRXData = oTransDetailRxSvr.PopulateDataOnHold(oOnHoldTrans.TransID);
                                oPOSTrans.oTDTaxData = oTDTaxSvr.PopulateDataOnHold(oOnHoldTrans.TransID);
                                oPOSTrans.oPOSTransPaymentData = oPosTransPaymentSvr.PopulateOnHold(oOnHoldTrans.TransID);
                                TransStartDateTime = oPOSTrans.oTransHRow.TransactionStartDate.ToString();
                                if (oHoldTransHData.TransHeader[0].TransType == 1)
                                    this.setTransactionType(POS_Core.TransType.POSTransactionType.Sales);
                                else
                                    this.setTransactionType(POS_Core.TransType.POSTransactionType.SalesReturn);

                                this.txtCustomer.Text = oHoldTransHData.TransHeader[0].CustomerID.ToString();
                                //this.txtCustomer.Tag = oHoldTransHData.TransHeader[0].CustomerID.ToString();  //PRIMEPOS-2536 22-May-2019 JY Added
                                this.lblCustomerName.Text = oHoldTransHData.TransHeader[0].CustomerName.ToString();
                                ShowCustomerTokenImage(oHoldTransHData.TransHeader[0].CustomerID);   //PRIMEPOS-2611 13-Nov-2018 JY Added
                                this.lblInvDiscount.Text = Convert.ToString(oHoldTransHData.TransHeader[0].TotalDiscAmount);
                                this.dtpTransDate.Value = oHoldTransHData.TransHeader[0].TransDate;

                                if (txtCustomer.Text.Length > 0)
                                {
                                    SearchCustomer(true);
                                }
                                else
                                {
                                    ClearCustomer();
                                }
                                int rowCount = 0; //PRIMEPOS-3446
                                isDeliveryAndOnHoldTrans = true; //PRIMEPOS-3494
                                foreach (TransDetailRow oRow in oHoldTransDData.TransDetail.Rows)
                                {
                                    rowCount++; //PRIMEPOS-3446
                                    //this.oTransDData.TransDetail.Rows.Add(oRow.ItemArray);
                                    bool ExceptionFlag = false;
                                    if (oRow.ItemID.ToUpper() == "RX")
                                    {
                                        if (oRow.ItemDescription.IndexOf("-") > 0)
                                        {
                                            oPOSTrans.oTDRow = oRow;
                                            oPOSTrans.oTDRow.Category = "F";//Added By Shitaljit on 15 march 2012 to marked FSA item
                                            try
                                            {
                                                DataTable oRxInfo = oPOSTrans.GetRxInfo(oRow.ItemDescription);
                                                //Following if else and else part code is Added By Shitaljit(QuicSolv) on 9 Nov 2011
                                                if ((oRxInfo != null) && oRxInfo.Rows.Count > 0)
                                                {
                                                    if ((oRxInfo.Rows[0]["Pickedup"].ToString() == "Y" || oRxInfo.Rows[0]["PickupPOS"].ToString() == "Y"))
                                                        FillRXInformation(oPOSTrans.ExtractRXInfoFromDescription(oRow.ItemDescription), false, rowCount); //PRIMEPOS-3446 Added rowCount parameter
                                                    else
                                                        FillRXInformation(oPOSTrans.ExtractRXInfoFromDescription(oRow.ItemDescription), true, rowCount); //PRIMEPOS-3446 Added rowCount parameter

                                                    #region PRIMEPOS-2536 22-May-2019 JY Added logic to show RX and Patient notes from PrimeRX.
                                                    ShowPrimeRXPOSNotes(Configuration.convertNullToString(oRxInfo.Rows[0]["RXNO"]), ref arrPatients);
                                                    #endregion Show RX and Patient notes from PrimeRX.
                                                }
                                                else
                                                {
                                                    ErrorHandler.throwCustomError(POSErrorENUM.TransDetail_RXNotFound);
                                                }
                                            }
                                            catch (Exception e)
                                            {
                                                logger.Fatal(e, "tbTerminalActions()");
                                                RXList = RXList + "\n" + e.Message;
                                                ExceptionFlag = true;
                                                throw e; //PRIMEPOS-3340
                                            }
                                        }
                                    }
                                    if (ExceptionFlag == false)
                                    {
                                        #region PRIMEPOS-2536 13-May-2019 JY Added
                                        if (Configuration.CPOSSet.ShowItemNotes == true)
                                        {
                                            logger.Trace("tbTerminalActions(string strKey)" + Configuration.convertNullToString(oRow.ItemID));
                                            ShowItemNotes(Configuration.convertNullToString(Configuration.convertNullToString(oRow.ItemID)));
                                        }
                                        #endregion
                                        //this.oPOSTrans.oTransDData.TransDetail.Rows.Add(oRow.ItemArray);
                                    }

                                    this.lblInvDiscount.Text = Convert.ToString(Convert.ToDecimal(this.lblInvDiscount.Text) - oRow.Discount);
                                }
                                isDeliveryAndOnHoldTrans = false; //PRIMEPOS-3494

                                //this.txtAmtTotal.Text = Convert.ToString(oOnHoldTrans.PendingAmount);
                                Configuration.isPrimeRxPay = true;//PRIMEPOS-3186
                                this.IsCustomerDriven = true;
                                this.PendingAmount = oOnHoldTrans.PendingAmount;


                                this.grdDetail.DataSource = oHoldTransDData.Tables[0];
                                this.grdDetail.DataBind();

                                this.grdDetail.Refresh();
                                if (oOnHoldTrans.PendingAmount != 0)
                                {
                                    this.txtAmtTotal.Text = Convert.ToString(oOnHoldTrans.PendingAmount);
                                }
                                if (this.grdDetail.Rows.Count > 0)
                                {
                                    this.changeStToolbars(TransactionStToolbars.strTBTerminalEntery);
                                }
                                else
                                {
                                    this.changeStToolbars(TransactionStToolbars.strTBTerminal);
                                }
                                oPOSTrans.oTransHData.AcceptChanges();

                                this.isAddRow = false;
                                this.isEditRow = false;

                                //Reset TransId = 0
                                if (oPOSTrans.oPOSTransPaymentData.Tables.Count > 0)
                                {
                                    for (int i = 0; i < oPOSTrans.oPOSTransPaymentData.Tables[0].Rows.Count; i++)
                                    {
                                        oPOSTrans.oPOSTransPaymentData.Tables[0].Rows[i]["TransID"] = 0;
                                        if (!string.IsNullOrWhiteSpace(Convert.ToString(oPOSTrans.oPOSTransPaymentData.Tables[0].Rows[i]["PrimeRxPayTransId"])))
                                        {
                                            if (oOnHoldTrans.keyValuePairs.ContainsKey(Convert.ToString(oPOSTrans.oPOSTransPaymentData.Tables[0].Rows[i]["PrimeRxPayTransId"])))
                                            {
                                                oPOSTrans.oPOSTransPaymentData.Tables[0].Rows[i]["AuthNo"] = oOnHoldTrans.keyValuePairs[Convert.ToString(oPOSTrans.oPOSTransPaymentData.Tables[0].Rows[i]["PrimeRxPayTransId"])];
                                                oPOSTrans.oPOSTransPaymentData.Tables[0].Rows[i]["Amount"] = oPOSTrans.oPOSTransPaymentData.Tables[0].Rows[i]["ApprovedAmount"];
                                            }
                                        }
                                    }
                                }
                                IDbConnection oConn = null;
                                IDbTransaction oTrans = null;
                                oConn = DataFactory.CreateConnection(Configuration.ConnectionString);
                                oTrans = oConn.BeginTransaction(IsolationLevel.ReadCommitted);

                                int transID = oPOSTrans.oTransHRow.TransID;

                                btnPayment_Click(null, null);
                                this.IsCustomerDriven = false;
                                Configuration.isPrimeRxPay = false;//PRIMEPOS-3186
                                if (this.IsRemoveOnHoldPayment)
                                {
                                    logger.Trace("Deleting the OnHold records");
                                    oTransHeaderSvr.DeleteOnHoldRows(oTrans, transID);
                                    oTrans.Commit();
                                    logger.Trace("Deleted the OnHold records");
                                }
                                this.PendingAmount = 0;
                            }
                            catch (Exception ex)
                            {
                                logger.Error(ex, "Pending Payment Click()");
                                this.PendingAmount = 0;
                                throw ex; //PRIMEPOS-3340
                            }
                        }
                    }
                    if (oPOSTrans.CurrentTransactionType == POS_Core.TransType.POSTransactionType.Sales)
                    {
                        isOnHoldTrans = false; //Added By shitaljit(QuicSolv) on 10 nov 2011
                    }
                }
                else if (strKey == "Ctrl+L")
                {
                    clsUIHelper.LocakStation();
                }
                else if (strKey == "F5")
                {
                    if (UserPriviliges.getPermission(UserPriviliges.Modules.POSTransaction.ID, UserPriviliges.Screens.POSTransaction.ID, UserPriviliges.Permissions.ROA.ID, UserPriviliges.Permissions.ROA.Name))
                    {
                        MakeHouseCharge();
                        //recieve on account
                    }
                }
                else if (strKey == "Ctrl+N")
                {
                    string sUserId = "";//PRIMEPOS-2808
                    if (UserPriviliges.getPermission(UserPriviliges.Modules.POSTransaction.ID, UserPriviliges.Screens.POSTransaction.ID, UserPriviliges.Permissions.NoSaleTrans.ID, UserPriviliges.Permissions.NoSaleTrans.Name, out sUserId))
                    {
                        NoSaleTransaction noSaleTransaction = new NoSaleTransaction();
                        noSaleTransaction.SetNoSaleTransaction(sUserId, Configuration.StationID);

                        RxLabel oRX = new RxLabel(null, null, null, ReceiptType.Void, null);
                        oRX.OpenDrawer();
                        Resources.Message.Display("Cash drawer is open \n Please close the drawer and press enter", "Cash Drawer", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else if (strKey == "Ctrl+O")
                {
                    //payout
                    frmPayOut ofrmPayOut = new frmPayOut();
                    ofrmPayOut.ShowDialog();
                }
                #region PRIMEPOS-2738 Added by Arvind 
                //else if (strKey == "Esc" && Configuration.CSetting.StrictReturn == true && oPOSTrans.CurrentTransactionType == POS_Core.TransType.POSTransactionType.SalesReturn)
                else if (strKey == "Esc" && isStrictReturn == true && oPOSTrans.CurrentTransactionType == POS_Core.TransType.POSTransactionType.SalesReturn)
                {
                    //Empty
                }
                #endregion
                else if (strKey == "Esc" && (this.txtItemCode.ContainsFocus == true || this.btnClose.ContainsFocus))
                {                    
                    SetNew(false);//Nileshj - Clear Item on Device
                    if (this.grdDetail.Rows.Count == 0)
                    {
                        #region PRIMEPOS-3207
                        clearHyphenAlert();
                        #endregion
                        this.txtItemCode.Text = "";
                        this.Close();
                    }
                    else
                    {
                        if (UserPriviliges.getPermission(UserPriviliges.Modules.POSTransaction.ID, UserPriviliges.Screens.POSTransaction.ID, UserPriviliges.Permissions.DeleteTrans.ID, UserPriviliges.Permissions.DeleteTrans.Name)) //PRIMEPOS-1923 08-Aug-2018 JY Added
                        {
                            if (Resources.Message.Display("Are you sure, you want to exit?", "Exit", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                            {
                                #region PRIMEPOS-3207
                                clearHyphenAlert();
                                #endregion
                                this.txtItemCode.Text = "";
                                this.Close();
                            }
                        }
                    }
                }
                #region Sprint-27 - PRIMEPOS-2456 09-Oct-2017 JY Added to lookup transaction
                else if (strKey == "Ctrl+S")
                {
                    if ((this.grdDetail.Rows.Count != 0) || string.IsNullOrWhiteSpace(txtItemCode.Text) == false)
                    {
                        return;
                    }
                    else
                    {
                        ProcessTransactionViewRequest(this.txtItemCode.Text.Trim());
                        bIsCopied = false;
                        if (oPOSTrans.oRXHeaderList.Count > 0)
                            AutoSelectFirstRxCustomer();
                    }
                    this.txtItemCode.Text = "";
                }
                #endregion
                #region PRIMEPOS-2786
                else if (strKey == "Ctrl+E")
                {
                    btnEBTBalance_Click(null, null);
                }
                #endregion
                #endregion strtbterminal
                logger.Trace("tbTerminalActions() - " + strKey + " - " + clsPOSDBConstants.Log_Exiting);
            }
            catch (Exception e)
            {
                logger.Fatal(e, "tbTerminalActions()" + strKey);
                clsUIHelper.ShowErrorMsg(e.Message);
            }
        }

        //completed by sandeep
        private void UpdateAmounts()
        {
            logger.Trace("UpdateAmounts() - " + DateTime.Now.Minute + ":" + DateTime.Now.Second + ":" + DateTime.Now.Millisecond + " - " + clsPOSDBConstants.Log_Entering);
            //Change by Manoj 1/14/2014
            if (Configuration.CPOSSet.UseSigPad == true)
            {
                //if (SigPadUtil.DefaultInstance.IsVF)
                //{
                //    SigPadUtil.DefaultInstance.isOnHold = onHoldTransID > 0 ? true : false;
                //}
                Invoke(new MethodInvoker(() =>
                {
                    SigPadUtil.DefaultInstance.UpdateTransSummary(this.txtAmtSubTotal.Text, this.txtAmtDiscount.Text, this.txtAmtTax.Text, this.txtAmtTotal.Text, grdDetail.Rows.Count.ToString());//Modified by SRT to provide ItemCOunt in the HHP.
                }));
            }

            logger.Trace("UpdateAmounts() - " + DateTime.Now.Minute + ":" + DateTime.Now.Second + ":" + DateTime.Now.Millisecond + " - " + clsPOSDBConstants.Log_Exiting);
        }

        //completed by sandeep
        //private string CheckRxCountForFill(DataTable oRxInfo, string ItemCode, bool isAlreadyProcessRx, bool bNeverUsed)  //PRIMEPOS-2699 05-Aug-2019 JY Commented
        private string CheckRxCountForFill(DataTable oRxInfo, string ItemCode, bool bAlreadyPickedUp, bool bAlreadyReturned, bool bNeverUsed, bool bIsProcessUnbilled = true)    //PRIMEPOS-2699 05-Aug-2019 JY Added few parameters    //PRIMEPOS-3163 30-Dec-2022 JY added bIsProcessUnbilled parameter
        {
            string result = "";
            if (oRxInfo.Rows.Count > 0)
            {
                logger.Trace("FillRXInformation() - Successfully Populated Rx Item#" + ItemCode);
                if (oRxInfo.Rows[0]["Status"].ToString() == "F")
                {
                    this.txtItemCode.Focus();
                    ErrorHandler.throwCustomError(POSErrorENUM.TransDetail_FiledRX);
                }
                if (!isOnHoldTrans)
                {
                    if (this.oPOSTrans.CurrentTransactionType == POS_Core.TransType.POSTransactionType.SalesReturn)
                    {
                        if (bAlreadyReturned)
                        {
                            this.txtItemCode.Clear();
                            this.txtItemCode.Focus();

                            //bring up message
                            bool isExceptionThrown = false;
                            AlreadyReturnedRxTransDetails(oRxInfo, out isExceptionThrown);
                            return null;
                        }
                        else if (bNeverUsed)
                        {
                            this.txtItemCode.Focus();
                            clsUIHelper.ShowErrorMsg("Rx#: " + ItemCode + " was never processed.\nCannot return a none processed RX!");
                            result = null;
                        }
                    }
                    else
                    {
                        //if (!bAlreadyPickedUp)
                        //{
                        //    this.txtItemCode.Clear();
                        //    this.txtItemCode.Focus();
                        //    result = null;
                        //}
                        //else 
                        if (!Configuration.CPOSSet.AllowPickedUpRxToTrans && Configuration.convertNullToString(oRxInfo.Rows[0]["Pickedup"]).Trim().ToUpper() == "Y")
                        {
                            if (!isBatchDelivery) // NileshJ - BatchDelivery PRIMERX-7688 23-Sept-2019 This is to supress the Pickedup Message if batchdelivery is set
                            {
                                if (!bAlreadyPickedUp)
                                {
                                    var RxNo = Configuration.convertNullToInt64(oRxInfo.Rows[0]["Rxno"]);
                                    var RefillNo = Configuration.convertNullToInt(oRxInfo.Rows[0]["nrefill"]);
                                    clsUIHelper.ShowErrorMsg("Rx#: " + RxNo + "-" + RefillNo
                                        + " is already Marked as picked up \nThe current POS setting prevents picked up prescriptions from being scanned in.");
                                    this.txtItemCode.Clear();
                                    this.txtItemCode.Focus();
                                    return null;
                                }
                            }
                        }
                        //
                    }
                    //oPOSTrans.isValidDrugClassRx(oRxInfo.Rows[0]["RXNO"].ToString().Trim(), oRxInfo.Rows[0]["nrefill"].ToString(), oRxInfo.Rows[0]["Class"].ToString().Trim(), ref RxWithValidClass); //PRIMEPOS-2468 23-Apr-2018 JY Commented
                }
                string sPartialFillNo = "0";
                if (oRxInfo.Columns.Contains("PartialFillNo"))
                    sPartialFillNo = oRxInfo.Rows[0]["PartialFillNo"].ToString();
                oPOSTrans.isValidDrugClassRx(oRxInfo.Rows[0]["RXNO"].ToString().Trim(), oRxInfo.Rows[0]["nrefill"].ToString(), oRxInfo.Rows[0]["Class"].ToString().Trim(), ref RxWithValidClass, sPartialFillNo);   //PRIMEPOS-2468 23-Apr-2018 JY Added       
            }
            if (oRxInfo.Rows.Count == 0)
            {
                this.txtItemCode.Focus();
                ErrorHandler.throwCustomError(POSErrorENUM.TransDetail_RXNotFound);
            }
            //else if (oPOSTrans.isUnBilledRx == true)
            //{
            //    this.txtItemCode.Focus();
            //    ErrorHandler.throwCustomError(POSErrorENUM.TransDetail_UnbilledRX);
            //}
            #region PRIMEPOS-2398 04-Jan-2021 JY Added
            else if (oRxInfo.Rows[0]["Status"].ToString() == "U")
            {
                if (oPOSTrans.isUnBilledRx == 0 || (isOnHoldTrans && oPOSTrans.isUnBilledRx == 2 && bIsProcessUnbilled == false))    //PRIMEPOS-3163 30-Dec-2022 JY Modified - scenario comes into picture when we process onhold transaction having unbilled Rxs
                {
                    this.txtItemCode.Focus();
                    ErrorHandler.throwCustomError(POSErrorENUM.TransDetail_UnbilledRX);
                }
                else if (!isOnHoldTrans && oPOSTrans.isUnBilledRx == 2)    //PRIMEPOS-3163 30-Dec-2022 JY Modified - Scenario comes into picture when we scan unbilled rx
                {
                    if (Resources.Message.Display("Scanned Rx is unbilled, do you wish to process it?", "Process UnBilled Rx", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.No)
                    {
                        this.txtItemCode.Focus();
                        return null;
                    }
                }
            }
            #endregion
            return result;
        }

        #region PRIMEPOS-2699 06-Aug-2019 JY Added
        private void AlreadyReturnedRxTransDetails(DataTable oRxInfo, out bool isExceptionThrown)
        {
            bool shouldFetchRX = false;
            string strException = "";
            long transId = 0;
            isExceptionThrown = false;
            oPOSTrans.ReturnedRxTransDetails(oRxInfo, out strException, out transId);
            if (!shouldFetchRX)
            {
                RXException rxException = new RXException(strException);
                rxException.TransID = transId;
                isExceptionThrown = true;
                ShouldReturnedRx(rxException);
            }
        }

        private void ShouldReturnedRx(RXException ex)
        {
            logger.Trace("ShouldReturnedRx(RXException ex) - " + clsPOSDBConstants.Log_Entering);

            string excepMessage = ex.Message;
            if (ex.TransID > 0)
            {
                excepMessage += "\n\n" + "Do You Wish to view the Transaction?";
            }

            DialogResult diaRes;
            if (ex.TransID > 0)
                diaRes = Resources.Message.Display(excepMessage, Configuration.ApplicationName, (ex.TransID > 0 ? MessageBoxButtons.OKCancel : MessageBoxButtons.OK), MessageBoxIcon.Information, MessageBoxDefaultButton.Button2);
            else
                diaRes = Resources.Message.Display(excepMessage, Configuration.ApplicationName, MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button2);

            if (diaRes == DialogResult.Yes)
            {
                frmViewTransactionDetail ofrmVTD;
                if (ex.TransID > 0)
                {
                    this.Cursor = Cursors.WaitCursor;
                    ofrmVTD = new frmViewTransactionDetail(ex.TransID.ToString(), "", Configuration.StationID, false);
                    ofrmVTD.ShowDialog();
                    this.Cursor = Cursors.Default;
                }
            }
            else
            {
                this.txtItemCode.Text = "";
            }
            logger.Trace("ShouldReturnedRx(RXException ex) - " + clsPOSDBConstants.Log_Exiting);
        }
        #endregion

        //completed by sandeep
        private void ValidateRXFillDaysStatus(DataTable oRxInfo, string ItemCode, ref bool isExceptionThrown, ref int IsAllowedForTrans)
        {

            string excepMessage = "";
            logger.Trace("FillRXInformation() - Checking Rx Fill days limit status for Rx Item#" + ItemCode);
            DialogResult diaRes;
            if (Configuration.CInfo.PreventRxMaxFillDayNotifyAction == 1)
            {
                excepMessage = "RX " + oRxInfo.Rows[0]["RXNO"] + " older than " + Configuration.CInfo.PreventRxMaxFillDayLimit + " days. Would you like to continue?";
                diaRes = Resources.Message.Display(excepMessage, Configuration.ApplicationName, MessageBoxButtons.YesNo, MessageBoxIcon.Information, MessageBoxDefaultButton.Button2);
                if (diaRes != DialogResult.Yes)
                {
                    IsAllowedForTrans = 0;
                    isExceptionThrown = true;
                    txtItemCode.Text = string.Empty;
                    txtItemCode.Focus();
                }
            }
            else
            {
                excepMessage = "RX " + oRxInfo.Rows[0]["RXNO"] + " older than " + Configuration.CInfo.PreventRxMaxFillDayLimit + " days not permitted";
                diaRes = Resources.Message.Display(excepMessage, Configuration.ApplicationName, MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button2);
                IsAllowedForTrans = 0;
                isExceptionThrown = true;
                txtItemCode.Text = string.Empty;
                txtItemCode.Focus();
            }

            logger.Trace("FillRXInformation() - Completed Checking Rx Fill days limit status for Rx Item#" + ItemCode);
        }
        //completed by sandeep
        public bool ValidateMultipleRX(DataTable oRxInfo)//, ref bool isDiffPatient)
        {
            if (POS_Core.Resources.Configuration.CInfo.WarnMultiPatientRX == true)
            {
                if (!isOnHoldTrans && oPOSTrans.CurrentTransactionType != POS_Core.TransType.POSTransactionType.SalesReturn)
                {
                    if (this.oPOSTrans.oTransDData.TransDetail.Rows.Count > 0)
                    {
                        if (oPOSTrans.oRXHeaderList.Count > 0 && oPOSTrans.oRXHeaderList.FindByPatient(oRxInfo.Rows[0]["PatientNo"].ToString()) == null)
                        {
                            DataTable oTable = oPOSTrans.GetPatient(oRxInfo.Rows[0]["PatientNo"].ToString());
                            string strMessage = "An Rx belonging to " + oTable.Rows[0]["Lname"].ToString().Trim() + ", " + oTable.Rows[0]["Fname"].ToString().Trim() + " is being scanned in the transaction. \nDo you wish to include the Rx in this transaction? ";
                            oTable.Dispose();
                            oTable = null;

                            //isDiffPatient = true;//Added by Krishna on 20 August 2012 to show Patient information if another patient RX is scanned

                            if (Resources.Message.Display(strMessage, "POS Transaction", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.No)
                            {
                                this.txtItemCode.Text = string.Empty;
                                return false; // throw (new Exception("####"));
                            }
                        }
                    }
                }
            }
            //Added By shitaljit to display the patient confirm form even if WarnMultiPatientRX is false
            //else if (oPOSTrans.oRXHeaderList.Count > 0 && oPOSTrans.oRXHeaderList.FindByPatient(oRxInfo.Rows[0]["PatientNo"].ToString()) == null)
            //{
            //    isDiffPatient = true;
            //}
            return true;

        }

        public void BuildTransDetailDataRX(DataTable oRxInfo)
        {
            TransDetailData oTData = new TransDetailData();
            this.txtItemCode.Text = "RX";
            bool bMatchFound = false;   //PRIMEPOS-2924 03-Dec-2020 JY Added
            if (oPOSTrans.InsToIgnoreCopay(oRxInfo.Rows[0]["Pattype"].ToString(), oRxInfo.Rows[0]["billtype"].ToString(), Configuration.convertNullToDecimal(oRxInfo.Rows[0]["PatAmt"].ToString())) == false)
            {
                //this.txtExtAmount.Text = oRxInfo.Rows[0]["PatAmt"].ToString();
                #region PRIMEPOS-2924 03-Dec-2020 JY Added
                if (Configuration.CSetting.RxTaxPolicy.Trim() == "1" && Configuration.CSetting.RxInsuranceToBeTaxed.Trim() != "" && oRxInfo.Rows[0]["Pattype"].ToString().Trim() != "" && Configuration.convertNullToDecimal(oRxInfo.Rows[0]["PatAmt"]) > 0)    //PRIMEPOS-3053 04-Feb-2021 JY Added RxTaxPolicy condition
                {
                    string[] arrRxInsuranceToBeTaxed = Configuration.CSetting.RxInsuranceToBeTaxed.Split(',');
                    for (int i = 0; i < arrRxInsuranceToBeTaxed.Length; i++)
                    {
                        if (arrRxInsuranceToBeTaxed[i].Trim().ToUpper() == oRxInfo.Rows[0]["Pattype"].ToString().Trim().ToUpper())
                        {
                            bMatchFound = true;
                            break;
                        }
                    }
                }
                if (bMatchFound)
                {
                    if (oPOSTrans.CurrentTransactionType == POS_Core.TransType.POSTransactionType.SalesReturn)
                        this.txtExtAmount.Text = ((Configuration.convertNullToDecimal(oRxInfo.Rows[0]["PatAmt"]) - Configuration.convertNullToDecimal(oRxInfo.Rows[0]["STAX"])) * -1).ToString();
                    else
                        this.txtExtAmount.Text = (Configuration.convertNullToDecimal(oRxInfo.Rows[0]["PatAmt"]) - Configuration.convertNullToDecimal(oRxInfo.Rows[0]["STAX"])).ToString();
                }
                else
                {
                    if (oPOSTrans.CurrentTransactionType == POS_Core.TransType.POSTransactionType.SalesReturn)
                        this.txtExtAmount.Text = (Configuration.convertNullToDecimal(oRxInfo.Rows[0]["PatAmt"]) * -1).ToString();
                    else
                        this.txtExtAmount.Text = oRxInfo.Rows[0]["PatAmt"].ToString();
                }
                #endregion
            }
            else
            {
                this.txtExtAmount.Text = "0";
            }
            int iPartialFillNo = 0;
            if (oRxInfo.Columns.Contains("PartialFillNo"))
                iPartialFillNo = Configuration.convertNullToInt(oRxInfo.Rows[0]["PartialFillNo"]);
            string stxtDescription = oRxInfo.Rows[0]["RXNo"].ToString() + "-" + oRxInfo.Rows[0]["nRefill"].ToString() + "-" + oRxInfo.Rows[0]["DETDRGNAME"].ToString();
            if (iPartialFillNo > 0)
                stxtDescription = oRxInfo.Rows[0]["RXNo"].ToString() + "-" + oRxInfo.Rows[0]["nRefill"].ToString() + "-" + iPartialFillNo.ToString() + "-" + oRxInfo.Rows[0]["DETDRGNAME"].ToString();
            this.txtQty.Value = this.DefaultQTY;
            this.txtDescription.Enabled = false;

            if(!isDeliveryAndOnHoldTrans) //PRIMEPOS-3494
            {
                oPOSTrans.oTDRow = oTData.TransDetail.AddRow(clsUIHelper.GetNextNumber(oPOSTrans.oTransDData, clsPOSDBConstants.TransDetail_Fld_TransDetailID), 0, this.DefaultQTY, Configuration.convertNullToDecimal(this.txtExtAmount.Text), 0, 0, Configuration.convertNullToDecimal(this.txtExtAmount.Text), 0, "RX", stxtDescription);    //Sprint-18 18-Nov-2014 JY Added  - changed unit price to some value instead of zero
                oPOSTrans.oTDRow.Category = "F";//Added By Shitaljit on 13 march 2012 to marked FSA item
            }
            
            #region PRIMEPOS-2836 15-Apr-2020 JY Added
            oPOSTrans.oTDRow.S3TransID = 0;
            oPOSTrans.oTDRow.S3PurAmount = 0;
            oPOSTrans.oTDRow.S3DiscountAmount = 0;
            oPOSTrans.oTDRow.S3TaxAmount = 0;
            #endregion
            oPOSTrans.oTDRow.ItemDescriptionMasked = MaskDrugName(oPOSTrans.oTDRow.ItemID, oPOSTrans.oTDRow.ItemDescription);  //PRIMEPOS-3130
            #region PRIMEPOS-2924 03-Dec-2020 JY Added
            if (bMatchFound)
            {
                ItemTax oTaxCodes = new ItemTax();
                TaxCodesData oTaxCodesData = new TaxCodesData();
                using (var dao = new TaxCodesSvr())
                {
                    oTaxCodesData = dao.PopulateList(" WHERE " + clsPOSDBConstants.TaxCodes_Fld_TaxCode + " = '" + clsPOSDBConstants.TaxCodes_Fld_RxTax + "'");
                }

                if (Configuration.isNullOrEmptyDataSet(oTaxCodesData) == false)
                {
                    oPOSTrans.UpdateTransTaxDetails(oPOSTrans.oTDTaxData, oPOSTrans.oTDRow.TransDetailID);
                    if (oPOSTrans.CurrentTransactionType == POS_Core.TransType.POSTransactionType.SalesReturn)
                        oPOSTrans.oTDRow.TaxAmount = Configuration.convertNullToDecimal(oRxInfo.Rows[0]["STAX"]) * -1;
                    else
                        oPOSTrans.oTDRow.TaxAmount = Configuration.convertNullToDecimal(oRxInfo.Rows[0]["STAX"]);

                    int ItemRow = 1;
                    if (oPOSTrans.oTDTaxData.TransDetailTax.Rows.Count > 0)
                    {
                        ItemRow += Configuration.convertNullToInt(oPOSTrans.oTDTaxData.TransDetailTax.Rows[oPOSTrans.oTDTaxData.TransDetailTax.Rows.Count - 1]["ItemRow"]);
                    }
                    oPOSTrans.oTDRow.TaxCode = "";
                    bool AddRow = false;
                    int rowIndex = 1;
                    for (int i = 1; i <= oTaxCodesData.Tables[0].Rows.Count; i++)
                    {
                        if (oPOSTrans.oTDTaxData.TransDetailTax.Rows.Count > 0 && !AddRow)
                        {
                            rowIndex += Configuration.convertNullToInt(oPOSTrans.oTDTaxData.TransDetailTax.Rows[oPOSTrans.oTDTaxData.TransDetailTax.Rows.Count - 1]["TransDetailTaxID"]);
                        }
                        AddRow = true;
                        oPOSTrans.oTDTaxData.TransDetailTax.AddRow(rowIndex, oPOSTrans.oTDRow.TransDetailID, 0, 0, oPOSTrans.oTDRow.TaxAmount, oPOSTrans.oTDRow.ItemID, Convert.ToInt32(oTaxCodesData.Tables[0].Rows[i - 1]["TaxID"].ToString()), ItemRow);
                        rowIndex++;

                        #region added logic to update TaxCode in TransDetail
                        if (oPOSTrans.oTDRow.TaxCode == "")
                            oPOSTrans.oTDRow.TaxCode = Configuration.convertNullToString(oTaxCodesData.TaxCodes[i - 1]["TaxCode"]);
                        else
                            oPOSTrans.oTDRow.TaxCode += "," + Configuration.convertNullToString(oTaxCodesData.TaxCodes[i - 1]["TaxCode"]);
                        #endregion
                    }
                }
            }
            #endregion
            #region PRIMEPOS-3053 07-Feb-2021 JY Added
            else if (Configuration.CSetting.RxTaxPolicy.Trim() == "2")
            {
                ItemRow oItemRow = null;
                oPOSTrans.GetItemRowByItemId(oPOSTrans.oTDRow.ItemID, ref oItemRow);
                oPOSTrans.ApplyTaxPolicy(oItemRow, oPOSTrans.oTDRow.TransDetailID);
                string sTaxCodes;
                if (oPOSTrans.IsItemTaxableForTrasaction(oPOSTrans.oTDTaxData, oPOSTrans.oTDRow.ItemID, out sTaxCodes, oPOSTrans.oTDRow.TransDetailID) == true)
                {
                    EditTax(oPOSTrans.oTDRow.TaxCode, clsPOSDBConstants.TaxCodes_tbl);
                }
            }
            #endregion
        }

        //completed by sandeep
        private RXHeader FillRXInformation(string ItemCode, bool doNotAddInTransaction, int rowCount, bool bIsProcessUnbilled = true) //PRIMEPOS-3163 30-Dec-2022 JY added bIsProcessUnbilled parameter //PRIMEPOS-3446 Added rowCount parameter
        {
            logger.Trace("FillRXInformation() - " + clsPOSDBConstants.Log_Entering);
            string nRefill = "";
            int iPartialFillNo = 0;
            string sPartialFillNo = string.Empty;
            DataTable oRxInfo = new DataTable();
            //bool isAlreadyProcessRx = false;    //PRIMEPOS-2699 05-Aug-2019 JY Commented
            //bool isNotReturned = false;   //PRIMEPOS-2699 05-Aug-2019 JY Commented
            bool bAlreadyPickedUp = false, bAlreadyReturned = false;    //PRIMEPOS-2699 05-Aug-2019 JY Added
            bool bNeverUsed = false;    //PRIMEPOS-2699 05-Aug-2019 JY Added
            bool isDiffPatient = false;//Added by Krishna on 20 August 2012

            //Added by Prog1 06Sep2009
            //Provides faicility to specify refillno with rxno
            int iIndex = ItemCode.IndexOf("-");
            if (iIndex > 0)
            {
                string[] strWork = ItemCode.Split('-');
                if (strWork.Length == 2)
                {
                    nRefill = ItemCode.Substring(iIndex + 1, ItemCode.Length - iIndex - 1);
                    ItemCode = ItemCode.Substring(0, iIndex);
                }
                else if (strWork.Length == 3)
                {
                    ItemCode = strWork[0].Trim();
                    nRefill = strWork[1].Trim();
                    sPartialFillNo = strWork[2].Trim();
                }
            }
            if (clsUIHelper.isNumeric(ItemCode) == false)
            {
                this.txtItemCode.Focus();
                ErrorHandler.throwCustomError(POSErrorENUM.TransDetail_RXNotFound);
            }
            //Added by SRT(Abhishek)  Date : 19 Aug 2009
            //added to check condition if flag is set to true or false
            //Followingif-else and if part code is Added By shitaljit(QuicSolv) to process RX regardless of its bill status in case of Hold Transactions.
            logger.Trace("FillRXInformation() - About to Call PharmSQL - " + clsPOSDBConstants.Log_CommunicatingRX);
            //start timer
            //oRxInfo = oPOSTrans.GetRxWithStatus(ItemCode, nRefill, isOnHoldTrans, ref isAlreadyProcessRx, ref bNeverUsed);    //PRIMEPOS-2699 05-Aug-2019 JY Commented
            oRxInfo = oPOSTrans.GetRxWithStatus(ItemCode, nRefill, isOnHoldTrans, ref bAlreadyPickedUp, ref bAlreadyReturned, ref bNeverUsed, sPartialFillNo);  //PRIMEPOS-2699 05-Aug-2019 JY Added            
            logger.Trace("FillRXInformation() - Successfully Query To PharmSQL for Rx Item#" + ItemCode);

            #region PRIMEPOS-2861 17-Jun-2020 JY just moved the block outside to first check the verified rx if "AllowVerifiedRXOnly" settings turned on
            //else if (oRxInfo.Rows[0]["Verified"].ToString().ToUpper() != "V" && Configuration.CInfo.AllowVerifiedRXOnly == true)  //PRIMEPOS-2789 04-Feb-2020 JY Commented
            if (oRxInfo != null && oRxInfo.Rows.Count > 0 && (Configuration.CInfo.AllowVerifiedRXOnly == 1 || Configuration.CInfo.AllowVerifiedRXOnly == 2))    //PRIMEPOS-2789 04-Feb-2020 JY Added   //PRIMEPOS-2593 23-Jun-2020 JY modified
            {
                if (!(oRxInfo.Rows[0]["Verified"].ToString().ToUpper() == "V" && Configuration.convertNullToInt(oRxInfo.Rows[0]["VRFStage"]) == 2)) //PRIMEPOS-2789 04-Feb-2020 JY Added
                {
                    //Added by SRT(Abhishek) Date : 24 Aug 2009
                    if (oPOSTrans.isUnBilledRx == 0) //PRIMEPOS-2398 04-Jan-2021 JY modified
                        oPOSTrans.countUnBilledRx--;
                    //End of Added by SRT(Abhishek)
                    //throw (new Exception("Rx cannot be checked out because it has not been verified by the pharmacist."));
                    #region PRIMEPOS-2586 27-Sep-2018 JY Added
                    string sRxCannotBeCheckedOutExMsg = "Rx cannot be checked out because it has not been verified by the pharmacist.";
                    var ex = new Exception(string.Format("{0} - {1}", sRxCannotBeCheckedOutExMsg, Configuration.iRxCannotBeCheckedOutExMsg));
                    ex.Data.Add(Configuration.iRxCannotBeCheckedOutExMsg, sRxCannotBeCheckedOutExMsg);
                    throw ex;
                    #endregion
                }
            }
            #endregion

            #region PRIMEPOS-2740 21-Feb-2020 JY Added
            if (this.oPOSTrans.CurrentTransactionType == POS_Core.TransType.POSTransactionType.Sales && oRxInfo != null && oRxInfo.Rows.Count > 0 && oRxInfo.Rows[0]["Status"].ToString() == "T") //PRIMEPOS-2844 13-May-2020 JY modified
            {
                if (!isBatchDelivery)
                {
                    var RxNo = Configuration.convertNullToInt64(oRxInfo.Rows[0]["Rxno"]);
                    var RefillNo = Configuration.convertNullToInt(oRxInfo.Rows[0]["nrefill"]);
                    if (RefillNo == 0)
                    {
                        //clsUIHelper.ShowErrorMsg("No refill available for this Rx#: " + RxNo);
                        clsUIHelper.ShowErrorMsg("Rx#: " + RxNo + "This prescription has been transferred-out and cannot be processed.");
                    }
                    else
                    {
                        //clsUIHelper.ShowErrorMsg("Rx#: " + RxNo + "-" + RefillNo + " : This is transferred prescriptions and POS couldn't scan it.");
                        clsUIHelper.ShowErrorMsg("Rx#: " + RxNo + "-" + RefillNo + " : This prescription has been transferred-out and cannot be processed.");
                        CheckUnPickedRXs(oRxInfo.Rows[0]["PATIENTNO"].ToString());
                    }
                    this.txtItemCode.Clear();
                    this.txtItemCode.Focus();
                    return null;
                }
            }
            #endregion

            if (CheckRxCountForFill(oRxInfo, ItemCode, bAlreadyPickedUp, bAlreadyReturned, bNeverUsed, bIsProcessUnbilled) == null) //PRIMEPOS-3163 30-Dec-2022 JY added bIsProcessUnbilled parameter
            {
                return null;
            }
            //End of Added by SRT(Abhishek)

            RXHeader oRXHeader = null;
            string StationID = string.Empty;
            string UserId = string.Empty;
            string TransID = string.Empty;//Added by shitaljit on 3 arpil 2012            
            dtRxOnHold = null;   //Sprint-21 08-Feb-2016 JY added 

            //isOnHoldTrans == false is added By shitaljit on 8 june 2012 to let user complete transaction if it is already in hold.
            logger.Trace("FillRXInformation() - Validating for adding into transaction Rx Item#" + ItemCode);

            #region PICKED Up Rx Transaction Details.
            bool isExceptionThrown = false;
            nRefill = oRxInfo.Rows[0]["NREFILL"].ToString();
            if (oRxInfo.Columns.Contains("PartialFillNo"))
                iPartialFillNo = Configuration.convertNullToInt(oRxInfo.Rows[0]["PartialFillNo"]);
            DateTime FillDate = DateTime.Now;
            if (oRxInfo.Rows[0]["DATEF"] != null)
            {
                FillDate = Convert.ToDateTime(oRxInfo.Rows[0]["DATEF"].ToString());
            }
            int filldayDiff = Configuration.convertNullToInt(((int)(DateTime.Now - FillDate).TotalDays).ToString());

            bool isAlreadyProcessedInPOS = false;
            bool IsAllowESCRx = oPOSTrans.AllowPickedUpRX(oRxInfo.Rows[0]["PickupPOS"].ToString(), oRxInfo.Rows[0]["Pickedup"].ToString(), ItemCode, nRefill, out isAlreadyProcessedInPOS, iPartialFillNo); //Added by Manoj 1/24/2013
            int IsAllowedForTrans = -1;
            if (oPOSTrans.CurrentTransactionType == POS_Core.TransType.POSTransactionType.Sales && doNotAddInTransaction == false && isOnHoldTrans == false && isAlreadyProcessedInPOS == true)
            {
                logger.Trace("FillRXInformation() - Checking Picked up status for Rx Item#" + ItemCode);
                IsAllowedForTrans = Configuration.convertBoolToInt(PickedUpRxTransDetails(oRxInfo, IsAllowESCRx, out isExceptionThrown));
                logger.Trace("FillRXInformation() - Completed Checking Picked up status for Rx Item#" + ItemCode);
            }
            #endregion PICKED Up Rx Transaction Details.

            else if (oPOSTrans.CurrentTransactionType == POS_Core.TransType.POSTransactionType.Sales && doNotAddInTransaction == false && isOnHoldTrans == false && Configuration.CInfo.PreventRxMaxFillDayNotifyAction > 0 && Configuration.CInfo.PreventRxMaxFillDayLimit > 0 && Configuration.CInfo.PreventRxMaxFillDayLimit < filldayDiff)
            {
                ValidateRXFillDaysStatus(oRxInfo, ItemCode, ref isExceptionThrown, ref IsAllowedForTrans);
            }
            #region Sprint-21 04-Feb-2016 JY added code to fix - SCENARIO: If you put some rxs for a patient on hold. Then if you enter a different rx for the same patient on the pos the unpicked rx
            //08-Apr-2016 JY added && isOnHoldTrans == false to restrict popup the message while processing onhold transaction
            else if (oPOSTrans.CurrentTransactionType == POS_Core.TransType.POSTransactionType.Sales && doNotAddInTransaction == false && isOnHoldTrans == false && oPOSTrans.RxIsOnHold(oRxInfo.Rows[0]["PatientNo"].ToString(), out dtRxOnHold))
            {
                string strRxNoTransNo;
                #region PRIMEPOS-3248
                bool isRxIsOnHoldForPrimeRxPayment = oPOSTrans.RxIsOnHoldForPrimeRxPayment(oRxInfo.Rows[0]["PatientNo"].ToString(), out dtRxOnHoldForPrimeRxPayment);

                if (isRxIsOnHoldForPrimeRxPayment)
                {
                    bool isRxIsOnHoldForPrimeRxPaymentFlag = oPOSTrans.HasOnHoldRXForPrimeRxPayment(dtRxOnHoldForPrimeRxPayment, oRxInfo, nRefill);
                    if (isRxIsOnHoldForPrimeRxPaymentFlag)
                    {
                        string sRxOnHoldForPaymentExMsg = "This prescription is on hold for PrimeRxPay payment. Please check the Process On Hold screen.";
                        Resources.Message.Display(sRxOnHoldForPaymentExMsg, "PrimePOS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        var ex = new Exception(string.Format("{0} - {1}", sRxOnHoldForPaymentExMsg, Configuration.iRxOnHoldExMsg));
                        ex.Data.Add(Configuration.iRxOnHoldExMsg, sRxOnHoldForPaymentExMsg);
                        throw ex;
                    }
                }
                #endregion
                bool flag = oPOSTrans.HasOnHoldRX(dtRxOnHold, oRxInfo, nRefill, out strRxNoTransNo);
                if (flag == true)
                {
                    #region PRIMEPOS-2586 27-Sep-2018 JY Added
                    string sRxOnHoldExMsg = "This Patient has Rx(s) on hold:" + Environment.NewLine + strRxNoTransNo;
                    var ex = new Exception(string.Format("{0} - {1}", sRxOnHoldExMsg, Configuration.iRxOnHoldExMsg));
                    ex.Data.Add(Configuration.iRxOnHoldExMsg, sRxOnHoldExMsg);
                    #endregion

                    #region PRIMEPOS-2639 27-Mar-2019 JY Added
                    bool bProcess = false;
                    if (Resources.Message.Display("Scanned Rx is on hold, do You wish to process it?", "Process on hold Rx", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                    {
                        if (UserPriviliges.getPermission(UserPriviliges.Modules.POSTransaction.ID, UserPriviliges.Screens.POSTransaction.ID, UserPriviliges.Permissions.OnHoldTrans.ID, UserPriviliges.Permissions.OnHoldTrans.Name))
                        {
                            OnholdRxs objOnholdRxs = new OnholdRxs();
                            objOnholdRxs.RxNo = Configuration.convertNullToInt64(oRxInfo.Rows[0]["RXNO"].ToString());
                            objOnholdRxs.NRefill = Configuration.convertNullToInt(oRxInfo.Rows[0]["NREFILL"].ToString());
                            lstOnHoldRxs.Add(objOnholdRxs);
                            bProcess = true;
                        }
                    }

                    if (bProcess == false)
                    {
                        throw ex;
                    }
                    #endregion
                }
                else
                {
                    clsUIHelper.ShowErrorMsg("This Patient has Rx(s) on hold: " + Environment.NewLine + strRxNoTransNo);
                }
            }
            #endregion            
            //End Of  Added by SRT (Sachin) Date 06 March 2010

            #region PRIMEPOS-2317 19-Mar-2019 JY Added
            try
            {
                if (oPOSTrans.oRXHeaderList.Count > 0 && oPOSTrans.oRXHeaderList.FindByPatient(oRxInfo.Rows[0]["PatientNo"].ToString()) == null)
                {
                    isDiffPatient = true;
                }
            }
            catch { }
            #endregion

            if ((IsAllowedForTrans == -1 || IsAllowedForTrans == 1) && doNotAddInTransaction == false)
            {
                #region Display Multiple Patient RX warning
                //Show warning messages if user has already scanned some patient RXs and currently scanned rx belongs to another patient.
                //If configuration option is on then system will display this warning.
                //ValidateMultipleRX(oRxInfo, ref isDiffPatient);
                //ValidateMultipleRX(oRxInfo);  //PRIMEPOS-2719 31-Jul-2019 JY Commented
                bool bStatus = ValidateMultipleRX(oRxInfo); //PRIMEPOS-2719 31-Jul-2019 JY Added
                if (!bStatus)
                    return null;  //PRIMEPOS-2719 31-Jul-2019 JY Added
                #endregion Display Multiple Patient RX warning

                #region Checking RX and Refill# is already in transction or not
                oRXHeader = oPOSTrans.CheckRXAlreadyInTrans(oRxInfo);
                #endregion Checking RX and Refill# is already in transction or not
                //Added by SRT(Abhishek) Date : 24 Aug 2009
                //Msg string will be formed after validation of RX.                
                #region UnBilledRX
                if (oPOSTrans.isUnBilledRx == 0)    //PRIMEPOS-2398 04-Jan-2021 JY modified
                {
                    if (oPOSTrans.countUnBilledRx == 1)
                        oPOSTrans.unbilledRx += oRxInfo.Rows[0]["RXNO"].ToString();
                    else
                        oPOSTrans.unbilledRx += " , " + oRxInfo.Rows[0]["RXNO"].ToString();
                }
                #endregion UnBilledRX
                //End of Added by SRT(Abhishek)

                BuildTransDetailDataRX(oRxInfo);

                if (oPOSTrans.oTDRow == null)
                {
                    //System Error Contact MMS Support.
                    oRXHeader = null;
                    throw (new Exception("Cannot scan same RX in same transaction."));
                }
                else
                {
                    if (oPOSTrans.CurrentTransactionType == POS_Core.TransType.POSTransactionType.SalesReturn) //Sprint-22 15-Dec-2015 JY Added if condition to restrict the updation of ReturnTransDetailId in case of sales transaction
                    {
                        oPOSTrans.oTDRow.ReturnTransDetailId = oPOSTrans.GetTransDetailID(oRxInfo.Rows[0]["RXNO"].ToString(), oRxInfo.Rows[0]["NREFILL"].ToString());
                        if (bIsCopied == false && isOnHoldTrans == false)   //PRIMEPOS-3012 16-Nov-2021 JY dont bring up message in case of on-hold transaction
                        {
                            #region Sprint-26 - PRIMEPOS-2418 01-Aug-2017 JY Added logic to popup message when we found the relavant "SALE" record against Rx return
                            DataSet oTDData;
                            string strMsg = oPOSTrans.GetPaymentDetailForReturnTrans(oRxInfo.Rows[0]["PICKUPDATE"].ToString(), out oTDData);
                            if (strMsg != "")
                            {
                                DialogResult diaRes = Resources.Message.Display(strMsg, Configuration.ApplicationName, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Information, MessageBoxDefaultButton.Button2);
                                if (diaRes == DialogResult.Yes)
                                {
                                    CopyRxInReturnTrans = Configuration.convertNullToInt(oTDData.Tables[0].Rows[0]["TransID"]);
                                    this.txtItemCode.Text = "";
                                    return oRXHeader;
                                }
                                else if (diaRes == DialogResult.No)
                                {
                                    oPOSTrans.oTDRow.Price = Configuration.convertNullToDecimal(oTDData.Tables[0].Rows[0]["Price"]);
                                    oPOSTrans.oTDRow.ExtendedPrice = oPOSTrans.oTDRow.Price * oPOSTrans.oTDRow.QTY;
                                }
                                else
                                {
                                    this.txtItemCode.Text = "";
                                    return oRXHeader;
                                }
                            }
                            #endregion
                        }
                    }
                }
                oPOSTrans.SetRowTrans(oPOSTrans.oTDRow);
                //Added form Naim 26032009
                if (Configuration.CInfo.CheckRXItemsForIIAS == true) //Changed by Naim 17Apr2009
                {
                    Item oItem = new Item();
                    oPOSTrans.oTDRow.IsIIAS = oItem.IsIIASItem(oRxInfo.Rows[0]["ndc"].ToString());
                }
                else
                {
                    oPOSTrans.oTDRow.IsIIAS = true;
                }
                //Added till here Prog1 26032009

                //Naim 16Apr2009
                //Commented By SRT(Ritesh Parekh) Date : 19-Aug-2009
                //Commented for appling the
                //oTDRow.IsIIAS = true;
                //Added By SRT(Ritesh Parekh) Date: 17-Aug-2009
                //To Identify whether this row was Rx or not.
                oPOSTrans.oTDRow.IsRxItem = true;
                //End Of Added By SRT(Ritesh Parekh)
            }

            if (isExceptionThrown == false)
            {
                DataTable oTable;
                bool bNewHeader = false;
                oPOSTrans.BuildRxHeader(oRxInfo.Rows[0], ref oRXHeader, ref bNewHeader);
                RXDetail oRXDetail = oPOSTrans.BuildRxDetail(oRxInfo.Rows[0]);
                oRXHeader.RXDetails.Add(oRXDetail);
                //oPOSTrans.InsertTransRxData(oRxInfo.Rows[0], oRXDetail, grdDetail.Rows.Count, ref oRXHeader, out oTable); //PRIMEPOS-2694 20-Jun-2019 JY Commented
                oPOSTrans.InsertTransRxData(oRxInfo, oRXDetail, grdDetail.Rows.Count, rowCount, ref oRXHeader, out oTable);   //PRIMEPOS-2694 20-Jun-2019 JY Added //PRIMEPOS-3446 Added rowCount parameter

                if (oPOSTrans.oTDRow != null) //Add by SRT (Abhidhek D) date : 6 March 2010
                {
                    //if (Configuration.CInfo.ConfirmPatient == true)   //PRIMEPOS-2317 15-Mar-2019 JY Commented
                    if (Configuration.CInfo.ConfirmPatient == 1)    //PRIMEPOS-2317 15-Mar-2019 JY Added
                    {
                        #region PRIMEPOS-2617 12-Nov-2018 JY Added 
                        //if (this.oPOSTrans.oTransDData.TransDetail.Rows.Count == 0) 
                        //{
                        //    ShowPatientInformation(oTable);
                        //}
                        //else if (isDiffPatient == true)
                        //{
                        //    ShowPatientInformation(oTable);
                        //}
                        #endregion

                        if (this.oPOSTrans.oTransDData.TransDetail.Rows.Count == 0 || this.oPOSTrans.oTransDRXData.TransDetailRX.Rows.Count == 1 || isDiffPatient == true)  //PRIMEPOS-2617 12-Nov-2018 JY Added 
                        {
                            //ShowPatientInformation(oTable, false);
                            #region PRIMEPOS-3065 10-Mar-2022 JY Added                            
                            if (oPOSTrans.CurrentTransactionType == POS_Core.TransType.POSTransactionType.Sales && this.oPOSTrans.oTransDRXData.TransDetailRX.Rows.Count == 1 && (Configuration.CPOSSet.ControlByID == 1 || Configuration.CPOSSet.ControlByID == 2))
                            {
                                bIsPatient = false;
                                strDriversLicense = "";
                                DriversLicenseExpDate = DateTime.MinValue;
                                DialogResult diaRes = Resources.Message.DisplayLocal("Who is picking up the medication?", Configuration.ApplicationName, MessageBoxButtons.YesNo, MessageBoxIcon.Question, "Patient", "Other", "");
                                if (diaRes == DialogResult.Yes)
                                {
                                    bIsPatient = true;
                                }
                            }
                            else
                            {
                                bIsPatient = true;
                            }
                            ShowPatientInformation(oTable, false, bIsPatient);
                            #endregion
                        }
                    }
                    #region PRIMEPOS-2317 15-Mar-2019 JY Added to input DOB of patient
                    else if (Configuration.CInfo.ConfirmPatient == 2)
                    {
                        if (!isOnHoldTrans && oPOSTrans.CurrentTransactionType != POS_Core.TransType.POSTransactionType.SalesReturn) //PRIMEPOS-3319
                        {
                            if (this.oPOSTrans.oTransDData.TransDetail.Rows.Count == 0 || this.oPOSTrans.oTransDRXData.TransDetailRX.Rows.Count == 1 || isDiffPatient == true)  //PRIMEPOS-2617 12-Nov-2018 JY Added 
                            {
                                #region PRIMEPOS-3065 10-Mar-2022 JY Added
                                bIsPatient = false;
                                strDriversLicense = "";
                                DriversLicenseExpDate = DateTime.MinValue;
                                if (oPOSTrans.CurrentTransactionType == POS_Core.TransType.POSTransactionType.Sales && this.oPOSTrans.oTransDRXData.TransDetailRX.Rows.Count == 1 && (Configuration.CPOSSet.ControlByID == 1 || Configuration.CPOSSet.ControlByID == 2))
                                {
                                    DialogResult diaRes = Resources.Message.Display("Is the person picking up the medication is the patient?", Configuration.ApplicationName, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
                                    if (diaRes == DialogResult.Yes)
                                    {
                                        bIsPatient = true;
                                    }
                                }
                                else
                                {
                                    bIsPatient = true;
                                }
                                #endregion
                                if (!ShowPatientInformation(oTable, true, bIsPatient))
                                {
                                    bool bTemp = isSearchUnPickCancel;
                                    if (this.oPOSTrans.oTransDRXData.TransDetailRX.Rows.Count > 0)
                                        this.oPOSTrans.oTransDRXData.TransDetailRX.Rows[this.oPOSTrans.oTransDRXData.TransDetailRX.Rows.Count - 1].Delete();

                                    oPOSTrans.DeleteRxOnValidating(RxWithValidClass, DrugClassInfoCapture, false, ref isSearchUnPickCancel);
                                    isSearchUnPickCancel = bTemp;
                                    return null;
                                }
                            }
                        }
                        else 
                        {
                            bIsPatient = true;
                        }
                    }
                    else
                    {
                        bIsPatient = true;  //PRIMEPOS-3065 10-Mar-2022 JY Added
                    }
                    #endregion
                }
                oTable.Dispose();
                oTable = null;
                isAddRow = true;
                logger.Trace("FillRXInformation() - Successfullty Validated RX Added  into transaction, Rx Item#" + ItemCode);
            }

            return oRXHeader;
        }

        //completed by sandeep
        private RXHeader FillRXInformation(string ItemCode)
        {
            try
            {
                return FillRXInformation(ItemCode, false, 0); //PRIMEPOS-3446
            }
            catch (Exception exp)
            {
                //logger.Fatal(Ex, "FillRXInformation(string ItemCode)");
                #region PRIMEPOS-2586 12-Sep-2018 JY Added
                string statusMessage = string.Empty;
                bool bLogException = true;
                if (exp.Data.Count > 0)
                {
                    foreach (DictionaryEntry de in exp.Data)
                    {
                        if (de.Key.ToString() == ((long)POSErrorENUM.TransDetail_RXNotFound).ToString() || de.Key.ToString() == ((long)POSErrorENUM.TransDetail_FiledRX).ToString()
                            || de.Key.ToString() == ((long)POSErrorENUM.TransDetail_UnbilledRX).ToString() || de.Key.ToString() == Configuration.iCanNotScanSameRx.ToString()
                            || de.Key.ToString() == Configuration.iRxIsOlderThan.ToString() || de.Key.ToString() == Configuration.iRxOnHoldExMsg.ToString()
                            || de.Key.ToString() == Configuration.iRxCannotBeCheckedOutExMsg.ToString())
                        {
                            statusMessage = exp.Data[de.Key].ToString();
                            bLogException = false;
                            break;
                        }
                    }
                }
                else //PRIMEPOS-2844 13-May-2020 JY Added
                {
                    try
                    {
                        if (exp.Source == "RXPICKEDUPMSG")
                        {
                            bLogException = false;
                        }
                        else if (((POSExceptions)exp).ErrNumber.ToString() == ((long)POSErrorENUM.TransDetail_RXNotFound).ToString() || ((POSExceptions)exp).ErrNumber.ToString() == ((long)POSErrorENUM.TransDetail_FiledRX).ToString()
                                || ((POSExceptions)exp).ErrNumber.ToString() == ((long)POSErrorENUM.TransDetail_UnbilledRX).ToString() || ((POSExceptions)exp).ErrNumber.ToString() == Configuration.iCanNotScanSameRx.ToString()
                                || ((POSExceptions)exp).ErrNumber.ToString() == Configuration.iRxIsOlderThan.ToString() || ((POSExceptions)exp).ErrNumber.ToString() == Configuration.iRxOnHoldExMsg.ToString()
                                || ((POSExceptions)exp).ErrNumber.ToString() == Configuration.iRxCannotBeCheckedOutExMsg.ToString())
                        {
                            bLogException = false;
                            statusMessage = ((POSExceptions)exp).ErrMessage;
                        }
                    }
                    catch { }
                }

                if (bLogException == true)
                {
                    logger.Fatal(exp, "FillRXInformation(string ItemCode)");
                }
                #endregion
                throw (exp);
            }
            finally
            {
                logger.Trace("FillRXInformation(string ItemCode) - " + clsPOSDBConstants.Log_Exiting);
            }
        }

        //completed by sandeep
        private bool PickedUpRxTransDetails(DataTable oRxInfo, bool isAllowPrimeESC, out bool isExceptionThrown)
        {
            bool shouldFetchRX = false;
            string strException;
            long transId;
            bool retVal = oPOSTrans.PickedUpRxTransDetails(oRxInfo, isAllowPrimeESC, out isExceptionThrown, out shouldFetchRX, out strException, out transId);
            if (shouldFetchRX)
            {
                RXException rxException = new RXException(strException);
                rxException.Source = "RXPICKEDUPMSG";
                rxException.TransID = transId;
                isExceptionThrown = true;
                ShouldFetchRx(rxException);
            }
            return retVal;
        }

        //completed by sandeep
        private void ShouldFetchRx(RXException ex)
        {
            logger.Trace("ShouldFetchRx() - " + clsPOSDBConstants.Log_Entering);
            //Added by Manoj 2/21/2013
            if (ex.Source == "RXPICKEDUPMSG")
            {
                string excepMessage = ex.Message;
                if (ex.TransID > 0)
                {
                    excepMessage += "\n\n" + "Do You Wish to view the Transaction?";
                    oPOSTrans.ItemAlreadyProcess = true;
                }

                DialogResult diaRes;
                if (ex.TransID > 0)
                    diaRes = Resources.Message.Display(excepMessage, Configuration.ApplicationName, (ex.TransID > 0 ? MessageBoxButtons.YesNoCancel : MessageBoxButtons.OKCancel), MessageBoxIcon.Information, MessageBoxDefaultButton.Button2);
                else
                    diaRes = Resources.Message.Display(excepMessage, Configuration.ApplicationName, MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button2);

                if (diaRes == DialogResult.Yes)
                {
                    frmViewTransactionDetail ofrmVTD;
                    if (ex.TransID > 0)
                    {
                        this.Cursor = Cursors.WaitCursor;
                        ofrmVTD = new frmViewTransactionDetail(ex.TransID.ToString(), "", Configuration.StationID, true);
                        ofrmVTD.ShowDialog();
                        this.Cursor = Cursors.Default;
                    }
                }
                else if (diaRes == DialogResult.No && Configuration.CInfo.AllowUnPickedRX != "0")
                {
                    isFetchRx = true;
                }
                else
                {
                    this.txtItemCode.Text = "";
                }
            }
            logger.Trace("ShouldFetchRx() - " + clsPOSDBConstants.Log_Exiting);
        }

        //completed by sandeep
        private bool ShowPatientInformation(DataTable dtPatInfo, bool bInputDOB, bool bIsPatient = false)    //PRIMEPOS-2317 18-Mar-2019 JY Added bInputDOB parameter   //PRIMEPOS-3065 10-Mar-2022 JY Added bIsPatient
        {
            frmPatientInfo frmPatInfo = new frmPatientInfo(dtPatInfo);
            frmPatInfo.bInputDOB = bInputDOB;
            #region PRIMEPOS-3065 10-Mar-2022 JY Added
            frmPatInfo.bIsPatient = bIsPatient;
            frmPatInfo.strDriversLicense = "";
            frmPatInfo.DriversLicenseExpDate = DateTime.MinValue;
            #endregion
            if (frmPatInfo.ShowDialog() == DialogResult.Cancel)
            {
                return false;
            }
            strDriversLicense = frmPatInfo.strDriversLicense;   //PRIMEPOS-3065 10-Mar-2022 JY Added
            DriversLicenseExpDate = frmPatInfo.DriversLicenseExpDate;   //PRIMEPOS-3065 10-Mar-2022 JY Added
            return true;
        }

        #endregion

        #region ItemCodeEvent
        //completed by sandeep
        private void txtItemCode_EditorButtonClick(object sender, Infragistics.Win.UltraWinEditors.EditorButtonEventArgs e)
        {
            try
            {
                SearchItem(txtItemCode.Text.Trim(), "");
            }
            catch (Exception) { }
        }


        //done by sandeep
        //added by sandeep for event handler Validating
        private void ItemBox_Validatiang(object sender, CancelEventArgs e)
        {
            #region PRIMERX-7688 
            allowUnPickedRX = Configuration.CInfo.AllowUnPickedRX;
            #endregion
            //TransDetailRow oTempRow = null;
            int gridRow = this.grdDetail.Rows.Count;
            #region PRIMEPOS-2738 
            //if StrictReturn = true and transType=Return 
            // return
            //if (oPOSTrans.CurrentTransactionType == POS_Core.TransType.POSTransactionType.SalesReturn && Configuration.CSetting.StrictReturn == true && this.txtItemCode.Text.Trim() != "")
            if (oPOSTrans.CurrentTransactionType == POS_Core.TransType.POSTransactionType.SalesReturn && isStrictReturn == true && this.txtItemCode.Text.Trim() != "")
            {
                txtItemCode.Text = "";
                clsUIHelper.ShowErrorMsg("You Cannot Add an item in StrictReturn Mode.");
                return;
            }
            // check if this method is not loading while we copy a transaction
            #endregion
            if (isBatchWtihShippingItem)//2927
            {
                if ((oPOSTrans.CurrentTransactionType == POS_Core.TransType.POSTransactionType.Sales || oPOSTrans.CurrentTransactionType == POS_Core.TransType.POSTransactionType.SalesReturn) && this.txtItemCode.Text.Trim() != "")
                {
                    clsUIHelper.ShowErrorMsg("You Cannot Add or Remove an item in Shipping.");
                    return;
                }
            }
            //Nileshj Ingenico Lane Close
            try
            {
                if (gridRow == 0 && SigPadUtil.DefaultInstance.isISC)
                {
                    logger.Trace("POSTransaction VF Device Monitor Trigger start row count in grid is 0");
                    SigPadUtil.DefaultInstance.VF.ReInitializeISC(Convert.ToInt32(Configuration.CPOSSet.PinPadPortNo));
                    logger.Trace("POSTransaction VF Device Monitor Trigger start ");
                    //Task.Delay(10000);
                    //Thread.Sleep(500);
                }
                else if (gridRow == 0 && SigPadUtil.DefaultInstance.isPAX)//NileshJ - PAX - Call Reinit when first Item add on Device - Reconnect Logic
                {
                    logger.Trace("POSTransaction PAX Device reinitialize start");
                    SigPadUtil.DefaultInstance.PD.ReInit();
                    logger.Trace("POSTransaction PAX Device reinitialize end");
                }
            }
            catch (Exception ex)
            {
            }
            try
            {
                string ItemCD = this.txtItemCode.Text.ToUpper();//PRIMEPOS-2643
                sMode = ""; //JY Added to reset the mode
                //if (oPOSTrans.oTDRow != null) {
                //    oTempRow = (TransDetailRow)this.oPOSTrans.oTransDData.TransDetail.NewRow();
                //    oPOSTrans.oTDRow.Copy(oTempRow);
                //}
                if (this.txtItemCode.Text.Trim() == "")
                {
                    return;
                }
                if (sender.Equals(this.txtItemCode) == true)
                {
                    #region Items
                    string ItemId = txtItemCode.Text;
                    logger.Trace("ItemBox_Validatiang() - " + clsPOSDBConstants.Log_Entering);
                    if (this.txtItemCode.Text.Trim() != "")
                    {
                        if (txtCustomer.Text.Trim() != "-1" && Configuration.CSetting.EnableCustomerEngagement) // Add Condition - NileshJ PRIMEPOS-2794
                        {
                            rightTabPayCust.Tabs[1].Visible = true; // Nileshj
                            rightTabPayCust.Tabs[1].Selected = true;
                            GetCustomerDetails(); //SAJID PRIMEPOS-2794
                        }
                        // check to see if customer phone# was entered
                        if (txtItemCode.Text.ToUpper().EndsWith("C") && txtItemCode.Text.Length > 1)
                        {
                            if (GetCustomer(txtItemCode.Text))
                                return;
                        }

                        // check to see if a transaction# is entered for viewing
                        if (this.txtItemCode.Text.StartsWith("?"))
                        {
                            #region copy transaction
                            if (this.grdDetail.Rows.Count != 0)
                            {
                                e.Cancel = true;
                            }
                            else
                            {
                                ProcessTransactionViewRequest(this.txtItemCode.Text.Trim());
                                bIsCopied = false;  //Sprint-26 - PRIMEPOS-2418 02-Aug-2017 JY Added
                                if (oPOSTrans.oRXHeaderList.Count > 0)
                                    AutoSelectFirstRxCustomer();    //Sprint-23 - PRIMEPOS-2293 04-Nov-2016 JY Added to auto select first rx customer in sale transaction

                                e.Cancel = true;
                            }
                            this.txtItemCode.Text = "";
                            #endregion copy transaction
                        }
                        else if (Configuration.CInfo.EnableIntakeBatch
                              && !string.IsNullOrWhiteSpace(Configuration.CInfo.IntakeBatchCode)
                              && Configuration.CPOSSet.UsePrimeRX == true
                              && this.txtItemCode.Text.ToUpper().EndsWith(Configuration.CInfo.IntakeBatchCode.ToUpper())
                              && this.txtItemCode.Text.ToUpper().Trim() != Configuration.CInfo.IntakeBatchCode.ToUpper().Trim()
                              && this.txtItemCode.Text.Contains("@") == false
                              )
                        {
                            #region PrimePOS-2448 Added BY Rohit Nair
                            RXHeader oRXHeader = null;
                            string sBatchNo = this.txtItemCode.Text.ToUpper().Replace(Configuration.CInfo.IntakeBatchCode.ToUpper(), string.Empty);

                            FIllBatchInformation(sBatchNo);
                            if (grdDetail.Rows.Count == 1 || string.IsNullOrEmpty(TransStartDateTime))
                            {
                                TransStartDateTime = DateTime.Now.ToString();//To Capture TranXn Start time;		
                            }
                            if (oPOSTrans.oRXHeaderList.Count > 0)
                            {
                                if (oRXHeader == null)
                                {
                                    oRXHeader = oPOSTrans.oRXHeaderList.FirstOrDefault();
                                }
                                #region Auto import customer from PrimeRx
                                if ((Configuration.CInfo.AutoImportCustAtTrans == 1 || Configuration.CInfo.AutoImportCustAtTrans == 2) && (txtCustomer.Text == "-1" || txtCustomer.Text == "") && (oPOSTrans.CurrentTransactionType == POS_Core.TransType.POSTransactionType.Sales))
                                {
                                    string PatientNo = oRXHeader.PatientNo;
                                    if (PatientNo.Trim().Length > 0)
                                    {
                                        Customer oCustomer = new Customer();
                                        CustomerData oData = new CustomerData();
                                        oData = oPOSTrans.PopulateCustomerList(" where PatientNo=" + PatientNo);
                                        if (oData.Customer.Rows.Count == 0)
                                        {
                                            DataSet oDS = null;
                                            MMSChargeAccount.ContAccount oAcct = new MMSChargeAccount.ContAccount();
                                            oAcct.GetPatientByCode(Convert.ToInt32(PatientNo), out oDS);
                                            if (oDS != null)
                                            {
                                                #region PRIMEPOS-2886 24-Aug-2020 JY Commented
                                                //Application.DoEvents();
                                                //CustomerData oCustomerData = oPOSTrans.CreateCustomerDSFromPatientDS(oDS, false);

                                                //if (oCustomerData.Customer.Rows.Count == 0)
                                                //{
                                                //    POS_Core_UI.Resources.Message.Display(this, "No Patient data found.", "PrimePOS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                                //}
                                                //else
                                                //{
                                                //    Application.DoEvents();
                                                //    oCustomer.DataRowSaved += new Customer.DataRowSavedHandler(oCustomer_DataRowSave);
                                                //    oCustomer.Persist(oCustomerData, true);
                                                //}
                                                #endregion
                                            }
                                            else
                                            {
                                                POS_Core_UI.Resources.Message.Display(this, "No Patient data found.", "PrimePOS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                            }
                                        }
                                    }
                                }
                                #endregion
                                AutoSelectFirstRxCustomer();
                            }
                            txtItemCode.Clear();
                            txtItemCode.Focus();

                            #endregion
                        }
                        #region PRIMEPOS-2036 22-Jan-2019 JY Added
                        else if (this.txtItemCode.Text.ToUpper().EndsWith("-" + Configuration.CPOSSet.RXCode.ToUpper())
                              && Configuration.CPOSSet.RXCode.Trim() != ""
                              && Configuration.CPOSSet.UsePrimeRX == true
                              && this.txtItemCode.Text.ToUpper().Trim() != "-" + Configuration.CPOSSet.RXCode.ToUpper().Trim()
                              && this.txtItemCode.Text.Contains("@") == false)
                        {
                            bool isBatchSelected = false;
                            RXHeader oRXHeader = null;
                            string sFacilityCode = this.txtItemCode.Text.Substring(0, this.txtItemCode.Text.Length - (Configuration.CPOSSet.RXCode.Length + 1));
                            FillUnPickedRXs(sFacilityCode, false, 'F');

                            #region Auto import customer from PrimeRx
                            if (oRXHeader != null && (Configuration.CInfo.AutoImportCustAtTrans == 1 || Configuration.CInfo.AutoImportCustAtTrans == 2) && (txtCustomer.Text == "-1" || txtCustomer.Text == "") && (oPOSTrans.CurrentTransactionType == POS_Core.TransType.POSTransactionType.Sales))
                            {
                                string PatientNo = oRXHeader.PatientNo;
                                if (PatientNo.Trim().Length > 0)
                                {
                                    Customer oCustomer = new Customer();
                                    CustomerData oData = new CustomerData();
                                    oData = oCustomer.PopulateList(" where PatientNo=" + PatientNo);
                                    if (oData.Customer.Rows.Count == 0)
                                    {
                                        DataSet oDS = null;
                                        MMSChargeAccount.ContAccount oAcct = new MMSChargeAccount.ContAccount();
                                        oAcct.GetPatientByCode(Convert.ToInt32(PatientNo), out oDS);
                                        if (oDS != null)
                                        {
                                            #region PRIMEPOS-2886 24-Aug-2020 JY Commented
                                            //Application.DoEvents();
                                            //CustomerData oCustomerData = oPOSTrans.CreateCustomerDSFromPatientDS(oDS, false);

                                            //if (oCustomerData.Customer.Rows.Count == 0)
                                            //{
                                            //    POS_Core_UI.Resources.Message.Display(this, "No Patient data found.", "PrimePOS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                            //}
                                            //else
                                            //{
                                            //    Application.DoEvents();
                                            //    oCustomer.DataRowSaved += new Customer.DataRowSavedHandler(oCustomer_DataRowSave);
                                            //    oCustomer.Persist(oCustomerData, true);
                                            //}
                                            #endregion
                                        }
                                        else
                                        {
                                            POS_Core_UI.Resources.Message.Display(this, "No Patient data found.", "PrimePOS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                        }
                                    }
                                }
                            }
                            #endregion
                            AutoSelectFirstRxCustomer();    //Added to auto select first rx customer in sale transaction

                            txtItemCode.Clear();
                            txtItemCode.Focus();
                        }
                        #endregion
                        else if (this.txtItemCode.Text.ToUpper().EndsWith(Configuration.CPOSSet.RXCode.ToUpper())
                            && Configuration.CPOSSet.RXCode.Trim() != ""
                            && Configuration.CPOSSet.UsePrimeRX == true
                            && this.txtItemCode.Text.ToUpper().Trim() != Configuration.CPOSSet.RXCode.ToUpper().Trim()
                            && this.txtItemCode.Text.Contains("@") == false)
                        {
                            bool isBatchSelected = false;
                            RXHeader oRXHeader = null;
                            string sRXNo = this.txtItemCode.Text.Substring(0, this.txtItemCode.Text.Length - Configuration.CPOSSet.RXCode.Length);
                            oRXHeader = FillRXInformation(this.txtItemCode.Text.Substring(0, this.txtItemCode.Text.Length - Configuration.CPOSSet.RXCode.Length));

                            #region PrimePOS-2448 Added BY Rohit Nair
                            if (oPOSTrans.CurrentTransactionType != POS_Core.TransType.POSTransactionType.SalesReturn && Configuration.CInfo.EnableIntakeBatch && oPOSTrans.IsRXInBatch(oRXHeader))
                            {
                                //PharmBL oPharmBL = new PharmBL();
                                long rxno = oRXHeader.RXDetails[0].RXNo;
                                int nrefill = oRXHeader.RXDetails[0].RefillNo;

                                long lbatchNo = oPOSTrans.GetBatchIDFromRxno(rxno, nrefill);

                                Resources.Message.Display("The Rx Scanned is Tagged to an IntakeBatch POS will Fetch other Rx's tagged to the Batch ", "Intake Batch", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                if (oPOSTrans.oTDRow != null)
                                {
                                    this.ValidateRow(sender, e);
                                }
                                isBatchSelected = true;
                                FIllBatchInformation(lbatchNo.ToString());
                            }
                            #endregion
                            if (!isBatchSelected)
                            {
                                #region Sprint-26 - PRIMEPOS-2418 08-Aug-2017 JY Added
                                try
                                {
                                    if (oPOSTrans.CurrentTransactionType == POS_Core.TransType.POSTransactionType.SalesReturn && CopyRxInReturnTrans > 0)
                                    {
                                        frmViewTransactionDetail ofrmVTD;
                                        this.Cursor = Cursors.WaitCursor;
                                        ofrmVTD = new frmViewTransactionDetail(CopyRxInReturnTrans.ToString(), "", Configuration.StationID, true);
                                        ofrmVTD.ShowDialog();
                                        this.Cursor = Cursors.Default;
                                        CopyTransaction(ofrmVTD);
                                        this.txtItemCode.Text = "";
                                    }
                                }
                                finally
                                {
                                    CopyRxInReturnTrans = 0;
                                }
                                #endregion
                                //Added by Manoj 2/21/2013
                                if (isFetchRx && oRXHeader == null)
                                {
                                    oRXHeader = FetchUnPickRx(this.txtItemCode.Text.Substring(0, this.txtItemCode.Text.Length - Configuration.CPOSSet.RXCode.Length));
                                }

                                if (oRXHeader != null)
                                {
                                    isAnyUnPickRx = (oRXHeader.RXDetails.Count > 0) ? true : false; //Added by Manoj 7/16/2014
                                    if (oPOSTrans.oTDRow != null)
                                    {
                                        this.ValidateRow(sender, e);
                                    }
                                    // POSLITE - Set AllowUnPickedRX = 2
                                    // PRIMERX-7688 - NileshJ - BatchDelivery 23-Sept-2019
                                    //if (allowUnPickedRX == string.Empty)
                                    //{
                                    //    allowUnPickedRX = Configuration.CInfo.AllowUnPickedRX;
                                    //}

                                    if (IsPOSLite || isBatchDelivery)  //This is done temporarily to ensure it makes rx go without verification if it is batch or if POSLite is used. It will be set again to original value stored in AllowUnPickedRx
                                    {
                                        Configuration.CInfo.AllowUnPickedRX = "2"; //2 means without verification
                                    }

                                    if (Configuration.CInfo.AllowUnPickedRX != "0")//pick RX(s) with verification screen
                                    {

                                        FillUnPickedRXs(oRXHeader, false);
                                        //Added By shitaljit fo pRIMEPOS- 801 for
                                        //String is not valid datetime error while adding RX item in transaction on 18 Arpil 2013
                                        if (grdDetail.Rows.Count == 1 || string.IsNullOrEmpty(TransStartDateTime))
                                        {
                                            TransStartDateTime = DateTime.Now.ToString();//To Capture TranXn Start time;
                                        }
                                    }

                                    #region Show RX and Patient notes from PrimeRX.
                                    //if (!isSearchUnPickCancel) //Added by Manoj 2/25/2013 If the user click Cancel don't show any message.
                                    if (!isSearchUnPickCancel || (oRXHeader != null && oRXHeader.RXDetails != null && oRXHeader.RXDetails.Count > 0))   //PRIMEPOS-2459 01-Mar-2019 JY Added
                                    {
                                        ShowPrimeRXPOSNotes(sRXNo, oRXHeader.PatientNo);

                                        if (!PrimeRxPatientCounselingAudited())
                                        {
                                            #region PRIMEPOS-2461 16-Feb-2021 JY Added
                                            try
                                            {
                                                if ((Configuration.CSetting.PatientCounselingPrompt == "1" && Configuration.convertNullToInt(oRXHeader.RXDetails[oRXHeader.RXDetails.Count - 1].RefillNo) == 0)
                                                    || Configuration.CSetting.PatientCounselingPrompt == "2")
                                                {
                                                    Resources.Message.Display("Please provide counseling for the prescription(s).", "PrimePOS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                                }
                                            }
                                            catch (Exception Ex)
                                            {
                                            }
                                            #endregion
                                        }
                                    }
                                    #endregion Show RX and Patient notes from PrimeRX.
                                }
                            }

                            #region Auto import customer from PrimeRx
                            if (oRXHeader != null && (Configuration.CInfo.AutoImportCustAtTrans == 1 || Configuration.CInfo.AutoImportCustAtTrans == 2) && (txtCustomer.Text == "-1" || txtCustomer.Text == "") && (oPOSTrans.CurrentTransactionType == POS_Core.TransType.POSTransactionType.Sales))
                            {
                                string PatientNo = oRXHeader.PatientNo;
                                if (PatientNo.Trim().Length > 0)
                                {
                                    Customer oCustomer = new Customer();
                                    CustomerData oData = new CustomerData();
                                    oData = oCustomer.PopulateList(" where PatientNo=" + PatientNo);
                                    if (oData.Customer.Rows.Count == 0)
                                    {
                                        DataSet oDS = null;
                                        MMSChargeAccount.ContAccount oAcct = new MMSChargeAccount.ContAccount();
                                        oAcct.GetPatientByCode(Convert.ToInt32(PatientNo), out oDS);
                                        if (oDS != null)
                                        {
                                            #region PRIMEPOS-2886 24-Aug-2020 JY Commented
                                            //Application.DoEvents();
                                            //CustomerData oCustomerData = oPOSTrans.CreateCustomerDSFromPatientDS(oDS, false);
                                            //if (oCustomerData.Customer.Rows.Count == 0)
                                            //{
                                            //    POS_Core_UI.Resources.Message.Display(this, "No Patient data found.", "PrimePOS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                            //}
                                            //else
                                            //{
                                            //    Application.DoEvents();
                                            //    oCustomer.DataRowSaved += new Customer.DataRowSavedHandler(oCustomer_DataRowSave);
                                            //    oCustomer.Persist(oCustomerData, true);
                                            //}
                                            #endregion
                                        }
                                        else
                                        {
                                            POS_Core_UI.Resources.Message.Display(this, "No Patient data found.", "PrimePOS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                        }
                                    }
                                }
                            }
                            #endregion
                            AutoSelectFirstRxCustomer(isBatchSelected);    //Sprint-23 - PRIMEPOS-2293 31-May-2016 JY Added to auto select first rx customer in sale transaction
                            ShowHyphenAlert();
                            txtItemCode.Clear();
                            txtItemCode.Focus();
                        }
                        else if (this.txtItemCode.Text.ToUpper().StartsWith(Configuration.CPOSSet.RXCode.ToUpper())
                            && Configuration.CPOSSet.RXCode.Trim() != ""
                            && Configuration.CPOSSet.UsePrimeRX == false
                            && this.txtItemCode.Text.Trim().Length == 24
                            && this.txtItemCode.Text.ToUpper().Trim() != Configuration.CPOSSet.RXCode.ToUpper().Trim()
                            && this.txtItemCode.Text.Contains("@") == false)
                        {
                            FillNonRXInformation(this.txtItemCode.Text);
                            this.ValidateRow(sender, e);
                        }
                        #region PRIMEPOS-3447
                        else if (this.txtItemCode.Text.StartsWith("~"))
                        {
                            PharmBL oPharmBL = new PharmBL();
                            string RXList = string.Empty;
                            Boolean bIsProcessUnbilled = false;
                            frmPOSOnHoldTrans oOnHoldTrans = new frmPOSOnHoldTrans();
                            oOnHoldTrans.txtTransID.Value = this.txtItemCode.Text.Substring(1).Trim();
                            oOnHoldTrans.Search(true);
                            #region PRIMEPOS-3465
                            if (oOnHoldTrans.oDataSet.Tables[0].Rows.Count == 0)
                            {
                                clsUIHelper.ShowErrorMsg("Invalid TransID, Please enter a valid TransID.");
                                this.txtItemCode.Clear();
                                this.txtItemCode.Focus();
                                return;
                            }
                            #endregion
                            oOnHoldTrans.FormCaption = "OnHold Transactions";
                            oOnHoldTrans.DisplayRecordAtStartup = false;
                            oOnHoldTrans.ShowDialog();

                            if (oOnHoldTrans != null && oOnHoldTrans.IsCanceled == false)
                            {
                                if (oOnHoldTrans.TransID < 1)
                                    return;
                                SetNew(false);
                                Int32 TransID = oOnHoldTrans.TransID;
                                onHoldTransID = TransID;
                                isOnHoldTrans = true;

                                //Get Transaction Data
                                TransDetailData oHoldTransDData;
                                TransHeaderData oHoldTransHData = oPOSTrans.GetOnHoldTransData(TransID, out oHoldTransDData);

                                try
                                {
                                    if (Configuration.CPOSSet.FetchUnbilledRx == 2)
                                    {
                                        string strRxNos = string.Empty;
                                        if (oHoldTransDData != null && oHoldTransDData.TransDetail.Rows.Count > 0)
                                        {
                                            foreach (TransDetailRow oRow in oHoldTransDData.TransDetail.Rows)
                                            {
                                                if (oRow.ItemID.ToUpper() == "RX")
                                                {
                                                    if (oRow.ItemDescription.IndexOf("-") > 0)
                                                    {
                                                        using (DataTable oRxInfo = oPOSTrans.GetRxInfo(oRow.ItemDescription))
                                                        {
                                                            if ((oRxInfo != null) && oRxInfo.Rows.Count > 0)
                                                            {
                                                                if (oRxInfo.Rows[0]["Status"].ToString().ToUpper() == "U")
                                                                {
                                                                    if (strRxNos == string.Empty)
                                                                    {
                                                                        strRxNos = oRxInfo.Rows[0]["Rxno"].ToString() + "-" + oRxInfo.Rows[0]["nrefill"].ToString();
                                                                    }
                                                                    else
                                                                    {
                                                                        strRxNos += ", " + oRxInfo.Rows[0]["Rxno"].ToString() + "-" + oRxInfo.Rows[0]["nrefill"].ToString();
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            } 
                                        }
                                        if (strRxNos != string.Empty)
                                        {
                                            if (Resources.Message.Display(strRxNos + " - Rx(s) are unbilled, do you wish to process it?", "Process UnBilled Rx", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                                            {
                                                bIsProcessUnbilled = true;
                                            }
                                        }
                                    }
                                }
                                catch (Exception Ex)
                                {
                                    logger.Fatal(Ex, "ItemBox_Validatiang()");
                                }

                                if (oHoldTransHData != null && oHoldTransHData.TransHeader != null)
                                {
                                    oPOSTrans.oTransHRow = oHoldTransHData.TransHeader[0];
                                    TransStartDateTime = oPOSTrans.oTransHRow.TransactionStartDate.ToString();
                                    if (oHoldTransHData.TransHeader[0].TransType == 1)
                                        this.setTransactionType(POS_Core.TransType.POSTransactionType.Sales);
                                    else
                                        this.setTransactionType(POS_Core.TransType.POSTransactionType.SalesReturn);

                                    this.txtCustomer.Text = oHoldTransHData.TransHeader[0].CustomerID.ToString();
                                    this.lblCustomerName.Text = oHoldTransHData.TransHeader[0].CustomerName.ToString();
                                    ShowCustomerTokenImage(oHoldTransHData.TransHeader[0].CustomerID);
                                    this.lblInvDiscount.Text = Convert.ToString(oHoldTransHData.TransHeader[0].TotalDiscAmount);
                                    this.dtpTransDate.Value = oHoldTransHData.TransHeader[0].TransDate; 
                                }

                                if (txtCustomer.Text.Length > 0)
                                {
                                    SearchCustomer(true);
                                }
                                else
                                {
                                    ClearCustomer();
                                }
                                grdDetail.Refresh();
                                RXList = string.Empty;
                                oPharmBL = new PharmData.PharmBL();
                                ArrayList arrPatients = new ArrayList();   
                                bool bPatientCounselingPrompt = false;
                                int rowCount=0; //PRIMEPOS-3446
                                if (oHoldTransDData != null && oHoldTransDData.TransDetail.Rows.Count > 0)
                                {
                                    rowCount++; //PRIMEPOS-3446
                                    foreach (TransDetailRow oRow in oHoldTransDData.TransDetail.Rows)
                                    {
                                        bool ExceptionFlag = false;
                                        if (oRow.ItemID.ToUpper() == "RX")
                                        {
                                            if (oRow.ItemDescription.IndexOf("-") > 0)
                                            {
                                                oPOSTrans.oTDRow = oRow;
                                                oPOSTrans.oTDRow.Category = "F";
                                                try
                                                {
                                                    DataTable oRxInfo = oPOSTrans.GetRxInfo(oRow.ItemDescription);
                                                    if ((oRxInfo != null) && oRxInfo.Rows.Count > 0)
                                                    {
                                                        if ((oRxInfo.Rows[0]["Pickedup"].ToString() == "Y" || oRxInfo.Rows[0]["PickupPOS"].ToString() == "Y"))
                                                            FillRXInformation(oPOSTrans.ExtractRXInfoFromDescription(oRow.ItemDescription), false, rowCount, bIsProcessUnbilled); //PRIMEPOS-3446 Added rowCount parameter
                                                        else
                                                            FillRXInformation(oPOSTrans.ExtractRXInfoFromDescription(oRow.ItemDescription), true, rowCount, bIsProcessUnbilled); //PRIMEPOS-3446 Added rowCount parameter

                                                        #region logic to show RX and Patient notes from PrimeRX.
                                                        ShowPrimeRXPOSNotes(Configuration.convertNullToString(oRxInfo.Rows[0]["RXNO"]), ref arrPatients);
                                                        #endregion

                                                        if (!PrimeRxPatientCounselingAudited())
                                                        {
                                                            try
                                                            {
                                                                if (bPatientCounselingPrompt == false && (Configuration.CSetting.PatientCounselingPrompt == "1" || Configuration.CSetting.PatientCounselingPrompt == "2"))
                                                                {
                                                                    bPatientCounselingPrompt = true;
                                                                    Resources.Message.Display("Please provide counseling for the prescription(s).", "PrimePOS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                                                }
                                                            }
                                                            catch (Exception ex)
                                                            {
                                                                logger.Fatal(ex, "ItemBox_Validatiang()");
                                                            }
                                                        }
                                                    }
                                                    else
                                                    {
                                                        ErrorHandler.throwCustomError(POSErrorENUM.TransDetail_RXNotFound);
                                                    }
                                                }
                                                catch (Exception ex)
                                                {
                                                    logger.Fatal(ex, "ItemBox_Validatiang()");
                                                    RXList = RXList + "\n" + ex.Message;
                                                    ExceptionFlag = true;
                                                    throw ex;
                                                }
                                            }
                                        }
                                        if (ExceptionFlag == false)
                                        {
                                            if (Configuration.CPOSSet.ShowItemNotes == true)
                                            {
                                                ShowItemNotes(Configuration.convertNullToString(Configuration.convertNullToString(oRow.ItemID)));
                                            }
                                            this.oPOSTrans.oTransDData.TransDetail.Rows.Add(oRow.ItemArray);
                                        }

                                        this.lblInvDiscount.Text = Convert.ToString(Convert.ToDecimal(this.lblInvDiscount.Text) - oRow.Discount);
                                    } 
                                }
                                TransDetailRXSvr.ProcOnHoldFlag = false;
                                txtItemCode.Text = "";
                                if (RXList != string.Empty)
                                {
                                    if (!(isOnHoldTrans && Configuration.CPOSSet.FetchUnbilledRx == 2)) 
                                        clsUIHelper.ShowErrorMsg(RXList);
                                    RXList = string.Empty;
                                }
                                oPOSTrans.oTDRow = null;

                                this.grdDetail.Refresh();
                                if (this.grdDetail.Rows.Count > 0)
                                {
                                    this.changeStToolbars(TransactionStToolbars.strTBTerminalEntery);
                                }
                                else
                                {
                                    this.changeStToolbars(TransactionStToolbars.strTBTerminal);
                                }
                                oPOSTrans.oTransHData.AcceptChanges();
                                this.isAddRow = false;
                                this.isEditRow = false;

                                if (Configuration.CPOSSet.UseSigPad == true && oPOSTrans.oTransDData.Tables[0].Rows.Count > 0 && !SigPadUtil.DefaultInstance.IsVF && !SigPadUtil.DefaultInstance.isPAX && !SigPadUtil.DefaultInstance.isVantiv)
                                {
                                    SigPadUtil.DefaultInstance.SendOnHoldItemListOnHHP(oPOSTrans.oTransDData, onHoldTransID);
                                    SigPadUtil.DefaultInstance.itemonHoldID(onHoldTransID);
                                }
                                else if (Configuration.CPOSSet.UseSigPad == true && oPOSTrans.oTransDData.Tables[0].Rows.Count > 0 && SigPadUtil.DefaultInstance.IsVF)
                                {
                                    DeviceItemsProcess("OnHoldItem", oPOSTrans.oTransDData.Tables[0]);
                                    SigPadUtil.DefaultInstance.itemonHoldID(onHoldTransID);
                                }
                                else if (Configuration.CPOSSet.UseSigPad == true && oPOSTrans.oTransDData.Tables[0].Rows.Count > 0 && SigPadUtil.DefaultInstance.isPAX && SigPadUtil.DefaultInstance.isVantiv)
                                {
                                    DeviceItemsProcess("OnHoldItem", oPOSTrans.oTransDData.Tables[0]);
                                    SigPadUtil.DefaultInstance.itemonHoldID(onHoldTransID);
                                }
                                else if (Configuration.CPOSSet.UseSigPad == true && oPOSTrans.oTransDData.Tables[0].Rows.Count > 0 && SigPadUtil.DefaultInstance.isPAX) 
                                {
                                    DeviceItemsProcess("OnHoldItem", oPOSTrans.oTransDData.Tables[0]);
                                    SigPadUtil.DefaultInstance.itemonHoldID(onHoldTransID);
                                }
                            }
                            else if (oOnHoldTrans != null &&  oOnHoldTrans.IsPendingPayment)
                            {
                                try
                                {
                                    logger.Trace("Entering in Pending Payment Click");
                                    TransDetailData oHoldTransDData;
                                    TransHeaderSvr oTransHeaderSvr = new TransHeaderSvr();
                                    TransDetailRXSvr oTransDetailRxSvr = new TransDetailRXSvr();
                                    TransDetailTaxSvr oTDTaxSvr = new TransDetailTaxSvr();
                                    POSTransPaymentSvr oPosTransPaymentSvr = new POSTransPaymentSvr();
                                    this.IsCustomerDriven = true;
                                    if (oOnHoldTrans.TransID < 1)
                                        return;
                                    Int32 TransID = oOnHoldTrans.TransID;
                                    onHoldTransID = TransID;
                                    isOnHoldTrans = true;

                                    ArrayList arrPatients = new ArrayList();   

                                    //Get Transaction Data
                                    TransHeaderData oHoldTransHData = oPOSTrans.oTransHData = oPOSTrans.GetOnHoldTransData(TransID, out oHoldTransDData, true);
                                    oPOSTrans.oTransHRow = oHoldTransHData.TransHeader[0];
                                    oPOSTrans.oTransDData = oHoldTransDData;
                                    oPOSTrans.oTransDRXData = oTransDetailRxSvr.PopulateDataOnHold(oOnHoldTrans.TransID);
                                    oPOSTrans.oTDTaxData = oTDTaxSvr.PopulateDataOnHold(oOnHoldTrans.TransID);
                                    oPOSTrans.oPOSTransPaymentData = oPosTransPaymentSvr.PopulateOnHold(oOnHoldTrans.TransID);
                                    TransStartDateTime = oPOSTrans.oTransHRow.TransactionStartDate.ToString();
                                    if (oHoldTransHData.TransHeader[0].TransType == 1)
                                        this.setTransactionType(POS_Core.TransType.POSTransactionType.Sales);
                                    else
                                        this.setTransactionType(POS_Core.TransType.POSTransactionType.SalesReturn);

                                    this.txtCustomer.Text = oHoldTransHData.TransHeader[0].CustomerID.ToString();
                                    this.lblCustomerName.Text = oHoldTransHData.TransHeader[0].CustomerName.ToString();
                                    ShowCustomerTokenImage(oHoldTransHData.TransHeader[0].CustomerID);  
                                    this.lblInvDiscount.Text = Convert.ToString(oHoldTransHData.TransHeader[0].TotalDiscAmount);
                                    this.dtpTransDate.Value = oHoldTransHData.TransHeader[0].TransDate;

                                    if (txtCustomer.Text.Length > 0)
                                    {
                                        SearchCustomer(true);
                                    }
                                    else
                                    {
                                        ClearCustomer();
                                    }
                                    int rowCount = 0; //PRIMEPOS-3446
                                    if (oHoldTransDData != null && oHoldTransDData.TransDetail.Rows.Count > 0)
                                    {
                                        rowCount++; //PRIMEPOS-3446
                                        foreach (TransDetailRow oRow in oHoldTransDData.TransDetail.Rows)
                                        {
                                            bool ExceptionFlag = false;
                                            if (oRow.ItemID.ToUpper() == "RX")
                                            {
                                                if (oRow.ItemDescription.IndexOf("-") > 0)
                                                {
                                                    oPOSTrans.oTDRow = oRow;
                                                    oPOSTrans.oTDRow.Category = "F";
                                                    try
                                                    {
                                                        DataTable oRxInfo = oPOSTrans.GetRxInfo(oRow.ItemDescription);
                                                        if ((oRxInfo != null) && oRxInfo.Rows.Count > 0)
                                                        {
                                                            if ((oRxInfo.Rows[0]["Pickedup"].ToString() == "Y" || oRxInfo.Rows[0]["PickupPOS"].ToString() == "Y"))
                                                                FillRXInformation(oPOSTrans.ExtractRXInfoFromDescription(oRow.ItemDescription), false, rowCount); //PRIMEPOS-3446 Added rowCount parameter
                                                            else
                                                                FillRXInformation(oPOSTrans.ExtractRXInfoFromDescription(oRow.ItemDescription), true, rowCount); //PRIMEPOS-3446 Added rowCount parameter

                                                            ShowPrimeRXPOSNotes(Configuration.convertNullToString(oRxInfo.Rows[0]["RXNO"]), ref arrPatients);
                                                        }
                                                        else
                                                        {
                                                            ErrorHandler.throwCustomError(POSErrorENUM.TransDetail_RXNotFound);
                                                        }
                                                    }
                                                    catch (Exception ex)
                                                    {
                                                        logger.Fatal(ex, "ItemBox_Validatiang()");
                                                        RXList = RXList + "\n" + ex.Message;
                                                        ExceptionFlag = true;
                                                        throw ex;
                                                    }
                                                }
                                            }
                                            if (ExceptionFlag == false)
                                            {
                                                if (Configuration.CPOSSet.ShowItemNotes == true)
                                                {
                                                    logger.Trace("ItemBox_Validatiang()" + Configuration.convertNullToString(oRow.ItemID));
                                                    ShowItemNotes(Configuration.convertNullToString(Configuration.convertNullToString(oRow.ItemID)));
                                                }
                                            }

                                            this.lblInvDiscount.Text = Convert.ToString(Convert.ToDecimal(this.lblInvDiscount.Text) - oRow.Discount);
                                        } 
                                    }

                                    Configuration.isPrimeRxPay = true;
                                    this.IsCustomerDriven = true;
                                    this.PendingAmount = oOnHoldTrans.PendingAmount;


                                    this.grdDetail.DataSource = oHoldTransDData.Tables[0];
                                    this.grdDetail.DataBind();

                                    this.grdDetail.Refresh();
                                    if (oOnHoldTrans.PendingAmount != 0)
                                    {
                                        this.txtAmtTotal.Text = Convert.ToString(oOnHoldTrans.PendingAmount);
                                    }
                                    if (this.grdDetail.Rows.Count > 0)
                                    {
                                        this.changeStToolbars(TransactionStToolbars.strTBTerminalEntery);
                                    }
                                    else
                                    {
                                        this.changeStToolbars(TransactionStToolbars.strTBTerminal);
                                    }
                                    oPOSTrans.oTransHData.AcceptChanges();

                                    this.isAddRow = false;
                                    this.isEditRow = false;

                                    if (oPOSTrans.oPOSTransPaymentData != null && oPOSTrans.oPOSTransPaymentData.Tables.Count > 0)
                                    {
                                        for (int i = 0; i < oPOSTrans.oPOSTransPaymentData.Tables[0].Rows.Count; i++)
                                        {
                                            oPOSTrans.oPOSTransPaymentData.Tables[0].Rows[i]["TransID"] = 0;
                                            if (!string.IsNullOrWhiteSpace(Convert.ToString(oPOSTrans.oPOSTransPaymentData.Tables[0].Rows[i]["PrimeRxPayTransId"])))
                                            {
                                                if (oOnHoldTrans.keyValuePairs.ContainsKey(Convert.ToString(oPOSTrans.oPOSTransPaymentData.Tables[0].Rows[i]["PrimeRxPayTransId"])))
                                                {
                                                    oPOSTrans.oPOSTransPaymentData.Tables[0].Rows[i]["AuthNo"] = oOnHoldTrans.keyValuePairs[Convert.ToString(oPOSTrans.oPOSTransPaymentData.Tables[0].Rows[i]["PrimeRxPayTransId"])];
                                                    oPOSTrans.oPOSTransPaymentData.Tables[0].Rows[i]["Amount"] = oPOSTrans.oPOSTransPaymentData.Tables[0].Rows[i]["ApprovedAmount"];
                                                }
                                            }
                                        }
                                    }
                                    IDbConnection oConn = null;
                                    IDbTransaction oTrans = null;
                                    oConn = DataFactory.CreateConnection(Configuration.ConnectionString);
                                    oTrans = oConn.BeginTransaction(IsolationLevel.ReadCommitted);

                                    int transID = oPOSTrans.oTransHRow.TransID;

                                    btnPayment_Click(null, null);
                                    this.IsCustomerDriven = false;
                                    Configuration.isPrimeRxPay = false;
                                    if (this.IsRemoveOnHoldPayment)
                                    {
                                        logger.Trace("Deleting the OnHold records");
                                        oTransHeaderSvr.DeleteOnHoldRows(oTrans, transID);
                                        oTrans.Commit();
                                        logger.Trace("Deleted the OnHold records");
                                    }
                                    this.PendingAmount = 0;
                                }
                                catch (Exception ex)
                                {
                                    logger.Error(ex, "Pending Payment Click()");
                                    this.PendingAmount = 0;
                                    throw ex; 
                                }
                            }
                            else if(oOnHoldTrans != null && oOnHoldTrans.IsCanceled == true)
                            {
                                this.txtItemCode.Clear();
                                this.txtItemCode.Focus();
                            }
                        }
                        #endregion
                        else
                        {
                            #region 30-Jan-2019 JY commented the code after discussing with Vivek and Fahad, due to which we couldn't scan the item with code "8934901810289"
                            //if (txtItemCode.Text.StartsWith("89") && txtItemCode.Text.EndsWith("89") && txtItemCode.Text.Length > 4)
                            //{
                            //    #region Scan Customer
                            //    string sID = "";
                            //    string strDecrptText = txtItemCode.Text.Substring(2, txtItemCode.Text.Length - 4);
                            //    if (strDecrptText.Length == 11)
                            //    {
                            //        strDecrptText = strDecrptText + "=";
                            //    }

                            //    try
                            //    {
                            //        sID = EncryptString.CustomDecrypt(strDecrptText);
                            //    }
                            //    catch { }
                            //    //"89DFIKACT89\r\n"

                            //    if (sID.Trim().Length > 0)
                            //    {
                            //        //Customer oCustomer = new Customer();
                            //        CustomerData oData = new CustomerData();
                            //        oData = oPOSTrans.PopulateCustomerList(" where AcctNumber=" + sID);
                            //        if (oData.Customer.Rows.Count > 0)
                            //        {
                            //            this.txtCustomer.Text = oData.Customer[0].AccountNumber.ToString();
                            //            this.lblCustomerName.Text = oData.Customer[0].CustomerName;
                            //            ShowCustomerTokenImage(oData.Customer[0].CustomerId);   //PRIMEPOS-2611 13-Nov-2018 JY Added
                            //            this.txtCustomer.Tag = oData.Customer[0].CustomerId;
                            //            txtItemCode.Text = "";
                            //            return;
                            //        }
                            //    }
                            //    #endregion Scan Customer
                            //}
                            #endregion

                            isAddRow = true;
                            FKEdit(this.txtItemCode.Text, clsPOSDBConstants.Item_tbl);//, false);

                            if (txtItemCode.Text.Length == 0)
                            {
                                txtItemCode.Focus();
                            }
                        }
                    }
                    logger.Trace("ItemBox_Validatiang() - " + clsPOSDBConstants.Log_Exiting);
                    #endregion Items
                }

                if (isFetchRx) // Added by Manoj 6/20/2013
                {
                    oPOSTrans.DeleteRxOnValidating(RxWithValidClass, DrugClassInfoCapture, isAnyUnPickRx, ref isSearchUnPickCancel);
                    txtItemCode.Text = "";
                    isFetchRx = false;
                    isAnyUnPickRx = false;//Added by Manoj 7/16/2014
                    txtItemCode.Focus();
                }

                #region PrimeInterfaceAmplicare PRIMEPOS-2643 05-Sep-2019
                if (Configuration.CInfo.PIEnable && Configuration.eInterfaceStatus == EventHub.InterfaceStatus.ServiceIsConnected && ItemCD.EndsWith(Configuration.CPOSSet.RXCode.ToUpper()))
                {
                    RXHeader oRXHeader = null;
                    List<PrimeRxIntData> rxData = new List<PrimeRxIntData>();
                    if (oPOSTrans.oRXHeaderList.Count > 0)
                    {
                        oRXHeader = oPOSTrans.oRXHeaderList[oPOSTrans.oRXHeaderList.Count - 1];

                        DataSet dsDRXData = new DataSet();
                        dsDRXData = oPOSTrans.oTransDRXData; // Fill_Date,NDC, RxNo,PatientNo
                        if (dsDRXData.Tables[0].Rows.Count > 0)
                        {
                            //for (int rowId = 0; rowId < dsDRXData.Tables[0].Rows.Count; rowId++)
                            //{
                            int latRowId = dsDRXData.Tables[0].Rows.Count - 1;

                            PrimeRxIntData oPrimeRxIntData = new PrimeRxIntData();
                            oPrimeRxIntData.claims = new ClaimsInt();

                            oPrimeRxIntData.pharmacyInt = new PharmacyInt();
                            oPrimeRxIntData.pharmacyInt.EmpId = POS_Core.Resources.Configuration.UserName;
                            oPrimeRxIntData.pharmacyInt.EmpFirstName = Configuration.UserName;
                            oPrimeRxIntData.pharmacyInt.EmpLastName = Configuration.UserName;
                            oPrimeRxIntData.claims.PATIENTNO = oRXHeader.PatientNo;
                            oPrimeRxIntData.claims.RXNO = (long)dsDRXData.Tables[0].Rows[latRowId]["RxNo"];
                            oPrimeRxIntData.claims.NDC = Convert.ToString(dsDRXData.Tables[0].Rows[latRowId]["DrugNDC"]);
                            oPrimeRxIntData.claims.DATEF = Convert.ToString(dsDRXData.Tables[0].Rows[latRowId]["DateFilled"]);
                            //oPrimeRxIntData.claims.DATEF = Convert.ToString(dsDRXData.Tables[0].Rows[rowId]["DateFilled"]);//oPOSTrans.oTransHData.ToString(); //?transaction date
                            oPrimeRxIntData.claims.DATEO = oPOSTrans.oRXHeaderList[oPOSTrans.oRXHeaderList.Count - 1].RXDetails[0].RxDate;
                            rxData.Add(oPrimeRxIntData);
                            //}
                            InterfaceStatus eInterfaceStatus;
                            EventHub.getInstance().RaiseEvent(PrimeRxEvents.POSPayment, rxData);
                        }
                    }
                }
                #endregion
            }
            catch (RXException rxException)
            {
                //logger.Fatal(rxException, "ItemBox_Validatiang()");
                logger.Trace("ItemBox_Validatiang() - " + rxException.Message); //PRIMEPOS-2844 13-May-2020 JY Added
                if (rxException.Source == "RXPICKEDUPMSG")
                {
                    string excepMessage = rxException.Message;
                    if (rxException.TransID > 0)
                        excepMessage += "\n\n" + "Do You Wish to view the Transaction?";

                    DialogResult diaRes;
                    if (rxException.TransID > 0 && rxException.DisplayYesNoButonOnly == false)
                        diaRes = Resources.Message.Display(excepMessage, Configuration.ApplicationName, (rxException.TransID > 0 ? MessageBoxButtons.YesNoCancel : MessageBoxButtons.OKCancel), MessageBoxIcon.Information, MessageBoxDefaultButton.Button2);
                    else if (rxException.TransID > 0 && rxException.DisplayYesNoButonOnly == true)
                    {
                        //Added By Shitaljit to block processign RX if itti alread process in POS
                        diaRes = Resources.Message.Display(excepMessage, Configuration.ApplicationName, MessageBoxButtons.OKCancel, MessageBoxIcon.Error, MessageBoxDefaultButton.Button2);
                    }
                    else
                    {
                        //Added By Shitaljit(QuicSolv) on 23 August 2011
                        diaRes = Resources.Message.Display(excepMessage, Configuration.ApplicationName, MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button2);
                    }

                    if (diaRes == DialogResult.Yes)
                    {
                        frmViewTransactionDetail ofrmVTD;
                        if (rxException.TransID > 0)
                        {
                            this.Cursor = Cursors.WaitCursor;
                            ofrmVTD = new frmViewTransactionDetail(rxException.TransID.ToString(), "", Configuration.StationID, true);
                            ofrmVTD.ShowDialog();
                            this.Cursor = Cursors.Default;
                        }
                    }

                    this.txtItemCode.Text = string.Empty;
                }
                else
                {
                    clsUIHelper.ShowErrorMsg(rxException.Message);
                }

                e.Cancel = true;
                this.ActiveControl.Focus();
            }
            catch (Exception exp)
            {
                //logger.Fatal(exp, "ItemBox_Validatiang()");   //PRIMEPOS-2586 11-Sep-2018 JY Commented
                //clsUIHelper.ShowErrorMsg(exp.Message);    //PRIMEPOS-2586 11-Sep-2018 JY Commented
                #region PRIMEPOS-2586 11-Sep-2018 JY Added
                string statusMessage = string.Empty;
                string strStatusId = string.Empty;  //PRIMEPOS-2639 22-Feb-2019 JY Added
                bool bLogException = true;
                if (exp.Data.Count > 0)
                {
                    foreach (DictionaryEntry de in exp.Data)
                    {
                        if (de.Key.ToString() == Configuration.iTransAlreadyReturned.ToString() || de.Key.ToString() == ((long)POSErrorENUM.TransDetail_RXNotFound).ToString()
                            || de.Key.ToString() == ((long)POSErrorENUM.TransDetail_FiledRX).ToString() || de.Key.ToString() == ((long)POSErrorENUM.TransDetail_UnbilledRX).ToString()
                            || de.Key.ToString() == Configuration.iCanNotScanSameRx.ToString() || de.Key.ToString() == Configuration.iRxIsOlderThan.ToString()
                            || de.Key.ToString() == Configuration.iRxOnHoldExMsg.ToString() || de.Key.ToString() == Configuration.iRxCannotBeCheckedOutExMsg.ToString())
                        {
                            strStatusId = de.Key.ToString();
                            statusMessage = exp.Data[de.Key].ToString();
                            bLogException = false;
                            break;
                        }
                    }
                }
                else //PRIMEPOS-2844 13-May-2020 JY Added
                {
                    try
                    {
                        if (((POSExceptions)exp).ErrNumber.ToString() == Configuration.iTransAlreadyReturned.ToString() || ((POSExceptions)exp).ErrNumber.ToString() == ((long)POSErrorENUM.TransDetail_RXNotFound).ToString()
                                || ((POSExceptions)exp).ErrNumber.ToString() == ((long)POSErrorENUM.TransDetail_FiledRX).ToString() || ((POSExceptions)exp).ErrNumber.ToString() == ((long)POSErrorENUM.TransDetail_UnbilledRX).ToString()
                                || ((POSExceptions)exp).ErrNumber.ToString() == Configuration.iCanNotScanSameRx.ToString() || ((POSExceptions)exp).ErrNumber.ToString() == Configuration.iRxIsOlderThan.ToString()
                                || ((POSExceptions)exp).ErrNumber.ToString() == Configuration.iRxOnHoldExMsg.ToString() || ((POSExceptions)exp).ErrNumber.ToString() == Configuration.iRxCannotBeCheckedOutExMsg.ToString())
                        {
                            strStatusId = ((POSExceptions)exp).ErrNumber.ToString();
                            statusMessage = ((POSExceptions)exp).ErrMessage;
                            bLogException = false;
                        }
                    }
                    catch { }
                }

                if (bLogException == false)
                {
                    #region PRIMEPOS-2639 22-Feb-2019 JY Added
                    if (strStatusId == Configuration.iRxOnHoldExMsg.ToString())
                    {
                        //    if (Resources.Message.Display(statusMessage + "\nScanned Rx is on hold, do You want to process it?", "Process on hold", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                        //    {
                        //        if (Resources.UserPriviliges.getPermission(UserPriviliges.Modules.POSTransaction.ID, UserPriviliges.Screens.POSTransaction.ID, UserPriviliges.Permissions.OnHoldTrans.ID, UserPriviliges.Permissions.OnHoldTrans.Name))
                        //        {
                        //            string strTransIds = string.Empty;
                        //            if (dtRxOnHold != null && dtRxOnHold.Rows.Count > 0)
                        //            {
                        //                DataTable uniquetransIds = dtRxOnHold.DefaultView.ToTable(true, "TransID");                                    
                        //                foreach (DataRow oRow in uniquetransIds.Rows)
                        //                {
                        //                    if (strTransIds == string.Empty)
                        //                        strTransIds = oRow["TransID"].ToString();
                        //                    else
                        //                        strTransIds += "," + oRow["TransID"].ToString();
                        //                }
                        //            }

                        //            frmPOSOnHoldTrans oOnHoldTrans = new frmPOSOnHoldTrans();
                        //            oOnHoldTrans.FormCaption = "OnHold Transactions";
                        //            oOnHoldTrans.DisplayRecordAtStartup = false;
                        //            oOnHoldTrans.strTransIDs = strTransIds;
                        //            oOnHoldTrans.ShowDialog();

                        //            PharmBL oPharmBL = new PharmBL();
                        //            string RXList = string.Empty;
                        //            if (oOnHoldTrans.IsCanceled == false)
                        //            {
                        //                if (oOnHoldTrans.TransID < 1) return;
                        //                SetNew(false);
                        //                Int32 TransID = oOnHoldTrans.TransID;
                        //                onHoldTransID = TransID;
                        //                isOnHoldTrans = true;

                        //                TransDetailData oHoldTransDData;
                        //                TransHeaderData oHoldTransHData = oPOSTrans.GetOnHoldTransData(TransID, out oHoldTransDData);
                        //                oPOSTrans.oTransHRow = oHoldTransHData.TransHeader[0];
                        //                TransStartDateTime = oPOSTrans.oTransHRow.TransactionStartDate.ToString();
                        //                if (oHoldTransHData.TransHeader[0].TransType == 1)
                        //                    this.setTransactionType(POSTransactionType.Sales);
                        //                else
                        //                    this.setTransactionType(POSTransactionType.SalesReturn);

                        //                this.txtCustomer.Text = oHoldTransHData.TransHeader[0].CustomerID.ToString();
                        //                this.txtCustomer.Tag = oHoldTransHData.TransHeader[0].CustomerID.ToString();
                        //                this.lblCustomerName.Text = oHoldTransHData.TransHeader[0].CustomerName.ToString();
                        //                ShowCustomerTokenImage(oHoldTransHData.TransHeader[0].CustomerID);
                        //                this.lblInvDiscount.Text = Convert.ToString(oHoldTransHData.TransHeader[0].TotalDiscAmount);
                        //                this.dtpTransDate.Value = oHoldTransHData.TransHeader[0].TransDate;

                        //                if (txtCustomer.Text.Length > 0)
                        //                {
                        //                    SearchCustomer(true);
                        //                }
                        //                else
                        //                {
                        //                    ClearCustomer();
                        //                }
                        //                grdDetail.Refresh();

                        //                RXList = string.Empty;
                        //                oPharmBL = new PharmData.PharmBL();
                        //                foreach (TransDetailRow oRow in oHoldTransDData.TransDetail.Rows)
                        //                {
                        //                    bool ExceptionFlag = false;
                        //                    if (oRow.ItemID.ToUpper() == "RX")
                        //                    {
                        //                        if (oRow.ItemDescription.IndexOf("-") > 0)
                        //                        {
                        //                            oPOSTrans.oTDRow = oRow;
                        //                            oPOSTrans.oTDRow.Category = "F";
                        //                            try
                        //                            {
                        //                                DataTable oRxInfo = oPOSTrans.GetRxInfo(oRow.ItemDescription);
                        //                                if ((oRxInfo != null) && oRxInfo.Rows.Count > 0)
                        //                                {
                        //                                    if ((oRxInfo.Rows[0]["Pickedup"].ToString() == "Y" || oRxInfo.Rows[0]["PickupPOS"].ToString() == "Y"))
                        //                                        FillRXInformation(oPOSTrans.ExtractRXInfoFromDescription(oRow.ItemDescription), false);
                        //                                    else
                        //                                        FillRXInformation(oPOSTrans.ExtractRXInfoFromDescription(oRow.ItemDescription), true);
                        //                                }
                        //                                else
                        //                                {
                        //                                    ErrorHandler.throwCustomError(POSErrorENUM.TransDetail_RXNotFound);
                        //                                }
                        //                            }
                        //                            catch (Exception ex1)
                        //                            {
                        //                                logger.Fatal(ex1, "tbTerminalActions()");
                        //                                RXList = RXList + "\n" + ex1.Message;
                        //                                ExceptionFlag = true;
                        //                            }
                        //                        }
                        //                    }
                        //                    if (ExceptionFlag == false)
                        //                        this.oPOSTrans.oTransDData.TransDetail.Rows.Add(oRow.ItemArray);

                        //                    this.lblInvDiscount.Text = Convert.ToString(Convert.ToDecimal(this.lblInvDiscount.Text) - oRow.Discount);
                        //                }
                        //                TransDetailRXSvr.ProcOnHoldFlag = false;
                        //                txtItemCode.Text = "";
                        //                if (RXList != string.Empty)
                        //                {
                        //                    clsUIHelper.ShowErrorMsg(RXList);
                        //                    RXList = string.Empty;
                        //                }
                        //                oPOSTrans.oTDRow = null;

                        //                this.grdDetail.Refresh();
                        //                if (this.grdDetail.Rows.Count > 0)
                        //                {
                        //                    this.changeStToolbars(TransactionStToolbars.strTBTerminalEntery);
                        //                }
                        //                else
                        //                {
                        //                    this.changeStToolbars(TransactionStToolbars.strTBTerminal);
                        //                }
                        //                oPOSTrans.oTransHData.AcceptChanges();
                        //                this.isAddRow = false;
                        //                this.isEditRow = false;

                        //                if (Configuration.CPOSSet.UseSigPad == true && oPOSTrans.oTransDData.Tables[0].Rows.Count > 0 && !SigPadUtil.DefaultInstance.IsVF && !SigPadUtil.DefaultInstance.isPAX)
                        //                {
                        //                    SigPadUtil.DefaultInstance.SendOnHoldItemListOnHHP(oPOSTrans.oTransDData, onHoldTransID);
                        //                    SigPadUtil.DefaultInstance.itemonHoldID(onHoldTransID);
                        //                }
                        //                else if (Configuration.CPOSSet.UseSigPad == true && oPOSTrans.oTransDData.Tables[0].Rows.Count > 0 && SigPadUtil.DefaultInstance.IsVF)
                        //                {
                        //                    DeviceItemsProcess("OnHoldItem", oPOSTrans.oTransDData.Tables[0]);
                        //                    SigPadUtil.DefaultInstance.itemonHoldID(onHoldTransID);
                        //                }
                        //                else if (Configuration.CPOSSet.UseSigPad == true && oPOSTrans.oTransDData.Tables[0].Rows.Count > 0 && SigPadUtil.DefaultInstance.isPAX)
                        //                {
                        //                    DeviceItemsProcess("OnHoldItem", oPOSTrans.oTransDData.Tables[0]);
                        //                    SigPadUtil.DefaultInstance.itemonHoldID(onHoldTransID);
                        //                }
                        //            }
                        //        }
                        //    }
                    }
                    #endregion
                    else
                    {
                        clsUIHelper.ShowErrorMsg(statusMessage);
                    }
                }
                else
                {
                    logger.Fatal(exp, "ItemBox_Validatiang()");
                    clsUIHelper.ShowErrorMsg(exp.Message);
                }
                #endregion

                txtItemCode.Focus();    //Sprint-21 - PRIMEPOS-2285 07-Mar-2016 JY Added
                e.Cancel = true;
                this.ActiveControl.Focus();
                return;
            }
            finally
            {
                if (this.grdDetail.Rows.Count > gridRow)
                {
                    if (this.grdDetail.Rows.Count > 0)
                    {
                        this.grdDetail.Selected.Rows.Clear();
                        this.grdDetail.ActiveRow = this.grdDetail.Rows[this.grdDetail.Rows.Count - 1];
                        this.grdDetail.ActiveRow.Cells["Total Amount"].Value = Convert.ToDouble(this.grdDetail.ActiveRow.Cells["ExtendedPrice"].Value) - Convert.ToDouble(this.grdDetail.ActiveRow.Cells["Discount"].Value);  // added by atul 02-nov-2010
                    }
                }

                HideGridCol();

                if (!Configuration.moveToQtyCol)
                    txtDescription.Enabled = true;
            }
            Configuration.CInfo.AllowUnPickedRX = allowUnPickedRX;// PRIMERX-7688 - NlieshJ 23-Sept-2019 - BatchDelivery the original value will be set again back to original
        }

        //added by sandeep
        private void txtItemCode_ValueChanged(object sender, EventArgs e)
        {
            IsItemExist = true;
            if (this.txtItemCode.Text.Trim() == "")
                sMode = ""; //19-Jun-2015 JY Added
        }


        //added by sandeep for event handler KeyDown
        private void txtItemCode_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (this.bInItemCodeEvent)
                {
                    e.Handled = true;
                    return;
                }
                else
                    bInItemCodeEvent = true;

                if (this.txtItemCode.Text.Trim() != "" && e.KeyData == Keys.Escape)
                {
                    this.txtItemCode.Text = "";
                    e.Handled = true;
                }
                else if (this.txtItemCode.Text.Trim() == "" && (e.KeyData == Keys.Enter || e.KeyData == Keys.Escape))
                {
                    if (this.grdDetail.Rows.Count > 0)
                    {
                        if (!IsPOSLite)// NileshJ - POSLITE - Added condition
                        {
                            if (oPOSTrans.oTransDRXData.TransDetailRX.Rows.Count > 0 && oPOSTrans.CurrentTransactionType == POS_Core.TransType.POSTransactionType.Sales) //PRIMEPOS-3530
                            {
                                if (!RecordPatientCounseling())
                                {
                                    bInItemCodeEvent = false;
                                    return;
                                }
                            }
                            InitPayment();
                        }
                    }
                    e.Handled = true;
                }
                else if (e.KeyData == Keys.Down)
                {
                    if (this.grdDetail.Rows.Count > 0)
                    {
                        this.grdDetail.Focus();
                        e.Handled = true;
                    }
                }
                else if (e.KeyData == Keys.Enter)
                {
                    this.SelectNextControl(this.ActiveControl, true, true, true, true);
                    e.Handled = true;
                    if (AutoGirdfallFlag)
                    {
                        this.SelectNextControl(this.ActiveControl, true, true, true, true);
                        AutoGirdfallFlag = false;
                    }
                    if (txtCustomer.Text.Trim() != "-1" && Configuration.CSetting.EnableCustomerEngagement) // Add Condition - NileshJ PRIMEPOS-2794
                    {
                        //rightTabPayCust.Tabs[1].Visible = true; // Nileshj
                        rightTabPayCust.Tabs[1].Selected = true; //PRIMEPOS-2794 SAJID DHUKKA
                        GetCustomerDetails(); //SAJID PRIMEPOS-2794
                    }
                }
                //Start :Added by Amit Date 07 April 2011
                else if (e.KeyData == Keys.Tab)
                {
                    this.txtDescription.Enabled = true;
                    if (txtCustomer.Text.Trim() != "-1" && Configuration.CSetting.EnableCustomerEngagement) // Add Condition - NileshJ PRIMEPOS-2794
                    {
                        //rightTabPayCust.Tabs[1].Visible = true; // Nileshj
                        rightTabPayCust.Tabs[1].Selected = true; //PRIMEPOS-2794 SAJID DHUKKA
                        GetCustomerDetails(); //SAJID PRIMEPOS-2794
                    }
                }

                bInItemCodeEvent = false;
                //End
            }
            catch (Exception exp)
            {
                bInItemCodeEvent = false;
                logger.Fatal(exp, "txtItemCode_KeyDown()");
            }
            finally
            {
            }
        }

        #endregion

        #region ItemDescriptionEvent

        //completed by sandeep
        private void ValidateItemDescription(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                #region description
                //if (this.oPOSTrans.oTDRow != null && !string.IsNullOrWhiteSpace(this.txtDescription.Text))    //PRIMEPOS-2707 05-Jul-2019 JY Commented
                //{
                //    this.oPOSTrans.oTDRow.ItemDescription = this.txtDescription.Text;
                //}
                if (isEditRow == true && isAddRow == false)
                {
                    this.oPOSTrans.oTDRow.ItemDescription = this.txtDescription.Text;   //PRIMEPOS-2707 05-Jul-2019 JY Added
                    ValidateRow(sender, e);
                    this.ClearItemRow();
                    this.grdDetail.Focus();
                }
                isDescriptionOverride = false;
                #endregion description
            }
            catch (Exception Ex)
            {
                logger.Fatal(Ex, "ValidateItemDescription()");
                clsUIHelper.ShowErrorMsg(Ex.Message);
            }
        }
        //completed by sandeep
        private void txtDescription_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter && sMode != "S")
            {
                try
                {
                    SearchItem("", txtDescription.Text.Trim());
                }
                catch (Exception) { }
            }
        }
        //completed by sandeep
        private void txtDescription_EditorButtonClick(object sender, Infragistics.Win.UltraWinEditors.EditorButtonEventArgs e)
        {
            try
            {
                SearchItem("", txtDescription.Text.Trim());
            }
            catch (Exception) { }
        }

        #endregion

        #region ItemMethod
        //completed by sandeep
        private void EditItem(String ItemID)
        {
            try
            {
                logger.Trace("EditItem() - " + clsPOSDBConstants.Log_Entering);
                frmItems oItems = new frmItems();
                if (!UserPriviliges.IsUserHasPriviliges(UserPriviliges.Modules.InventoryMgmt.ID, UserPriviliges.Screens.ItemFile.ID, -998))
                {
                    oItems.AllowEdit = false;
                    //Added By Shitaljit(QuicSolv) on 5 oct 2011
                    clsUIHelper.ShowOKMsg("You do not have right to Edit Item.");
                    return;
                }
                else
                {
                    oItems.AllowEdit = true;
                }

                //Following If is added by shitaljit(QuicSolv) on 25 August to stop calling AskApplyToGrid() if cancel button is click on Item Form.
                ItemData oItemData = oPOSTrans.PopulateItemList(" where ItemId='" + ItemID + "'");
                ItemRow oItemRow = (ItemRow)oItemData.Tables[0].Rows[0];
                string strTaxPolicy = oItemRow.TaxPolicy;
                oItems.Edit(ItemID);
                oItems.ShowDialog(this);
                oItemData = oPOSTrans.PopulateItemList(" where ItemId='" + ItemID + "'");
                oItemRow = (ItemRow)oItemData.Tables[0].Rows[0];

                if (oItems.IsCanceled == false)
                {
                    isItemInfoChanged = true;
                    AskApplyToGrid(strTaxPolicy);
                    oPOSTrans.oTDRow.TaxID = 0;
                    isItemInfoChanged = false;
                }
                //End of added by shitaljit on 25 August 2011
            }
            catch (Exception exp)
            {
                logger.Fatal(exp, "EditItem()");
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
            logger.Trace("EditItem() - " + clsPOSDBConstants.Log_Exiting);
        }
        private void ValidateRow(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                txtQty.Enabled = false;// Configuration.moveToQtyCol;

                logger.Trace("ValidateRow() - " + clsPOSDBConstants.Log_Entering);
                if (!isDescriptionOverride)
                    oPOSTrans.Validate_ItemID(this.txtItemCode.Text.ToString());
                oPOSTrans.Validate_Qty(this.txtQty.Value.ToString());
                if (isAddRow == true)
                {
                    logger.Trace("ValidateRow() - Item Add Mode");
                    if (oPOSTrans.IsItemExists() == false)
                    {
                        try
                        {
                            #region Sprint-19 - 2146 29-Dec-2014 JY Added multiple tax selection validation
                            if (cmbTaxCode.Text.Trim() != "")
                            {
                                if (!Configuration.CPOSSet.SelectMultipleTaxes && cmbTaxCode.CheckedItems.Count > 1)
                                {
                                    clsUIHelper.ShowErrorMsg("\nYou can not select multiple Tax Codes as the respective settings is off.");
                                    cmbTaxCode.Focus();
                                    return;
                                }
                            }

                            #endregion Sprint-19 - 2146 29-Dec-2014 JY Added multiple tax selection validation

                            ItemData oIData = new ItemData();
                            //Added By shitaljit for Item Description Text Prediction on 10 Aug 2012
                            if (Configuration.CInfo.ShowTextPrediction == true)
                            {
                                this.txtDescription.Text = this.txtItemDescription.Text;
                                //oPOSTrans.oTDRow.ItemDescription = this.txtItemDescription.Text.Trim();
                            }
                            else  //PRIMEPOS-2721 13-Aug-2019 JY Added else part
                            {
                                this.txtItemDescription.Text = this.txtDescription.Text;
                            }
                            oPOSTrans.oTDRow.ItemDescription = this.txtItemDescription.Text.Trim(); //PRIMEPOS-2721 13-Aug-2019 JY just moved outside if

                            oPOSTrans.oTDRow.QTY = DefaultQTY;
                            //Added By Dharmendra(SRT) to check the price field value
                            if (Convert.ToDecimal(oPOSTrans.oTDRow.Price) > 0)
                            {
                                //Updated By SRT(Gaurav) Date: 08-Jul-2009 after adding Naim changes and SRT code.
                                //Added initialization for parameter Prefered Vendor
                                //Added By Shitaljit(QuicSolv) on 1 July 2011
                                //Added last "false" value for isEBTItem field to set it false By default
                                oIData.Item.AddRow(oPOSTrans.oTDRow.ItemID, 0, oPOSTrans.oTDRow.ItemDescription, "", "", "", "", "", 0, Convert.ToDecimal(oPOSTrans.oTDRow.Price), 0, 0, true, oPOSTrans.oTDRow.TaxID, true, oPOSTrans.oTDRow.Discount / Convert.ToDecimal(oPOSTrans.oTDRow.Price) * 100, DateTime.Now, DateTime.Now, 0, 0, "", 0, 0, 0, DateTime.Now, "", "", DateTime.MinValue, DateTime.Now, "", false, false, false, false, true, 0, false, 0, "", true, true, false, 0, 0, 0, 0, 0);   //Sprint-21 - 2173 06-Jul-2015 JY Added "True" parameter for IsActive   //PRIMEPOS-2592 01-Nov-2018 JY Added "false" for IsNonRefundable // Added for Solutran: 0,0,0,0 - PRIMEPOS-2663 - NileshJ - 05-July-2019
                            }
                            else
                            {
                                //Updated By SRT(Gaurav) Date: 08-Jul-2009 after adding Naim changes and SRT code.
                                //Added initialization for parameter Prefered Vendor

                                //Following If-Else Added by Krishna on 6 May 2011
                                //Added By Shitaljit(QuicSolv) on 1 July 2011
                                //Added last "false" value for isEBTItem field to set it false By default
                                if (cmbTaxCode.Text == "")
                                {
                                    oIData.Item.AddRow(oPOSTrans.oTDRow.ItemID, 0, oPOSTrans.oTDRow.ItemDescription, "", "", "", "", "", 0, Convert.ToDecimal(oPOSTrans.oTDRow.Price), 0, 0, false, oPOSTrans.oTDRow.TaxID, true, 0, DateTime.Now, DateTime.Now, 0, 0, "", 0, 0, 0, DateTime.Now, "", "", DateTime.MinValue, DateTime.Now, "", false, false, false, false, true, 0, false, 0, "", true, true, false, 0, 0, 0, 0, 0);    //Sprint-21 - 2173 06-Jul-2015 JY Added "True" parameter for IsActive   //PRIMEPOS-2592 01-Nov-2018 JY Added "false" for IsNonRefundable // Added for Solutran: 0,0,0,0 - PRIMEPOS-2663 - NileshJ - 05-July-2019
                                }
                                else
                                {
                                    oIData.Item.AddRow(oPOSTrans.oTDRow.ItemID, 0, oPOSTrans.oTDRow.ItemDescription, "", "", "", "", "", 0, Convert.ToDecimal(oPOSTrans.oTDRow.Price), 0, 0, true, oPOSTrans.oTDRow.TaxID, true, 0, DateTime.Now, DateTime.Now, 0, 0, "", 0, 0, 0, DateTime.Now, "", "", DateTime.MinValue, DateTime.Now, "", false, false, false, false, true, 0, false, 0, "", true, true, false, 0, 0, 0, 0, 0);    //Sprint-21 - 2173 06-Jul-2015 JY Added "True" parameter for IsActive    //PRIMEPOS-2592 01-Nov-2018 JY Added "false" for IsNonRefundable // Added for Solutran: 0,0,0,0 - PRIMEPOS-2663 - NileshJ - 05-July-2019
                                }
                                //Till here Added by Krishna

                                //Added by SRT(Abhishek) Date : 26 Aug 2009
                                try
                                {
                                    if (IsItemExist && (oIData.Item.Rows[0].RowState == DataRowState.Unchanged || oIData.Item.Rows[0].RowState == DataRowState.Added))
                                    {
                                        oIData.Item.Rows[0].SetModified();
                                    }
                                }
                                catch (Exception)
                                {
                                }
                                //End of Added by SRT(Abhishek)  Date : 26 Aug 2009
                            }

                            if (txtDepartmentCode.Text != "" && this.txtDepartmentCode.Tag == "" && this.oPOSTrans.oDeptRow == null)
                            {
                                SearchDeptCode();
                                if (this.txtDepartmentCode.Tag != "")
                                {
                                    oIData.Item[0].DepartmentID = Configuration.convertNullToInt(this.txtDepartmentCode.Tag);
                                    lblHExtPrice.Text = "NET Price";
                                    txtDepartmentCode.Text = "";
                                    txtDepartmentCode.Visible = false;
                                    this.txtDepartmentCode.Tag = "";
                                }
                                else
                                {
                                    this.txtDepartmentCode.Focus();
                                    return;
                                }
                            }
                            if (txtDepartmentCode.Text != "" && this.txtDepartmentCode.Tag != "" && this.oPOSTrans.oDeptRow != null)
                            {
                                oIData.Item[0].DepartmentID = Configuration.convertNullToInt(this.txtDepartmentCode.Tag);
                                lblHExtPrice.Text = "NET. Price";
                                txtDepartmentCode.Text = "";
                                txtDepartmentCode.Visible = false;
                                this.txtDepartmentCode.Tag = "";
                            }
                            else if (txtDepartmentCode.Text == "")
                            {
                                if (Resources.Message.DisplayDefaultNo("Default Department for Item # " + oIData.Item[0].ItemID + " is not set, Do you want to set it?", "Department Not Set", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                                {
                                    txtDepartmentCode.Visible = true;
                                    lblHExtPrice.Text = "Dept.Code";
                                    txtDepartmentCode.Focus();
                                    return;
                                }
                                else
                                {
                                    oIData.Item[0].DepartmentID = Configuration.CInfo.DefaultDeptId;
                                    lblHExtPrice.Text = "NET. Price";
                                    this.txtDepartmentCode.Visible = false;
                                }
                            }

                            if (txtDescription.Text == string.Empty)
                            {
                                clsUIHelper.ShowErrorMsg("Item description can not be null");
                                txtDescription.Focus();
                                return;
                            }

                            #region Sprint-21 - 2204 30-Jun-2015 JY Added to validate selling price if respective settings is off
                            if (sMode == "S" && Configuration.CInfo.AllowZeroSellingPrice == false && double.Parse(txtUnitPrice.Value.ToString()) == 0.0)
                            {
                                clsUIHelper.ShowErrorMsg("Unit price can not be 0");
                                if (txtUnitPrice.Enabled)
                                    txtUnitPrice.Focus();
                                return;
                            }
                            #endregion

                            //Added By shitaljit(QuicSolv) on 15 July 2011
                            //To set is taxable = false if texcpode is blank.
                            if (cmbTaxCode.Text == "")
                            {
                                oIData.Item[0].isTaxable = false;
                            }
                            //END of added By Shitaljit.
                            Configuration.UpdatedBy = "M";//Added By Shitaljit on 1/17/2014
                            oPOSTrans.PersistItem(oIData);
                            List<int> selectedTaxCodes = cmbTaxCode.CheckedItems.Select(checkedItem => int.Parse(checkedItem.DataValue.ToString())).ToList();
                            TaxCodeHelper.PersistItemTaxCodes(selectedTaxCodes, oPOSTrans.oTDRow.ItemID);
                            this.txtDepartmentCode.Tag = "";
                            this.txtDescription.Enabled = false;
                            //Added By shitaljit for Item Description Test Prediction on 10 Aug 2012
                            if (Configuration.CInfo.ShowTextPrediction == true)
                            {
                                this.txtItemDescription.Visible = false;
                                this.txtDescription.Visible = true;
                                setDescriptionView();
                                this.txtDescription.BringToFront();
                                this.txtDescription.Text = "";
                                this.txtItemDescription.Text = "";
                            }
                            oPOSTrans.CalcExtdPrice(oPOSTrans.oTDRow);
                            logger.Trace("ValidateRow() - CalcExtdPrice End");
                        }
                        catch (POSExceptions exp)
                        {
                            logger.Fatal(exp, "ValidateRow()");
                            clsUIHelper.ShowErrorMsg("Unable to add Item. " + exp.Message);
                            this.txtItemCode.Focus();
                            return;
                        }
                    }
                    #region Sprint-21 - 2173 09-Jul-2015 JY Added if InActive item selected then it should warn the user
                    else
                    {
                        if (Configuration.CInfo.RestrictInActiveItem == true && oPOSTrans.IsItemActive() == false)
                        {
                            Resources.Message.Display("Current setting does not allow to add in-active item.", "Restrict In-Active Item", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            if (this.txtItemCode.Enabled)
                                txtItemCode.Focus();
                            return;
                        }
                    }
                    #endregion

                    if (this.txtCustomer.Text.Trim().Length == 0)   //Sprint-25 - PRIMEPOS-2380 15-Feb-2017 JY Added if to improve logic
                    {
                        #region Sprint-25 - PRIMEPOS-2380 15-Feb-2017 JY Added for PSE Items checking in list
                        if (Configuration.CInfo.useNplex == true)
                        {
                            DataTable dtPSE_Items = oPOSTrans.checkPSEItems(oPOSTrans.oTDRow);
                            if (dtPSE_Items.Rows.Count > 0)
                            {
                                clsUIHelper.ShowErrorMsg(dtPSE_Items.Rows[0]["Description"].ToString().Trim() + "- is PSE Item,\n" + "Please Select a Customer other than Default.");
                                SearchCustomer(true);
                                if (txtCustomer.Text.Trim().Length == 0)
                                {
                                    this.ClearItemRow();
                                    if (this.grdDetail.Rows.Count > 0)
                                    {
                                        this.changeStToolbars(TransactionStToolbars.strTBTerminalEntery);
                                    }
                                    else
                                    {
                                        this.changeStToolbars(TransactionStToolbars.strTBTerminal);
                                    }
                                    return;
                                }
                            }
                        }
                        #endregion
                        else
                        {
                            ItemRow oIRow = oPOSTrans.checkOTCItems(oPOSTrans.oTDRow);
                            if (oIRow != null)
                            {
                                oPOSTrans.oIMCDetailRow = oPOSTrans.GetItemMonitoringDetails(oIRow.ItemID.Trim());//Added By Shitaljit(QuicSolv) 0n 5 oct 2011
                                                                                                                  //Edited the meggage add oIMCDetailRow.Description.Trim() By Shitaljit(QuicSolv) 0n 5 oct 2011
                                if (oPOSTrans.oIMCDetailRow != null)
                                {
                                    clsUIHelper.ShowErrorMsg(oIRow.Description.Trim() + ", Item is Marked for Monitoring,\n" + oPOSTrans.oIMCDetailRow.Description.Trim() + "\nPlease Select a Customer other than Default.");
                                    SearchCustomer(true);
                                    if (txtCustomer.Text.Trim().Length == 0)
                                    {
                                        this.ClearItemRow();
                                        if (this.grdDetail.Rows.Count > 0)
                                        {
                                            this.changeStToolbars(TransactionStToolbars.strTBTerminalEntery);
                                        }
                                        else
                                        {
                                            this.changeStToolbars(TransactionStToolbars.strTBTerminal);
                                        }
                                        return;
                                    }
                                }
                            }
                        }
                    }

                    oPOSTrans.SetItemCategory(oPOSTrans.oTDRow);
                    TransDetailRow oExistingRow = null;
                    if (Configuration.CInfo.GroupTransItems == true)
                    {
                        oExistingRow = oPOSTrans.GetExistingRow(oPOSTrans.oTDRow, false);
                    }

                    if (oExistingRow != null)
                    {
                        int newQty = oPOSTrans.oTDRow.QTY;
                        decimal newTax = oPOSTrans.oTDRow.TaxAmount;
                        txtAmtTax.Text = Convert.ToString(oExistingRow.TaxAmount += newTax);
                        txtQty.Value = oExistingRow.QTY += newQty;
                        oPOSTrans.oTDRow = oExistingRow;
                        isEditRow = true;
                        isAddRow = false;
                        CancelEventArgs ce = new CancelEventArgs(false);
                        this.ValidateItemQty(txtQty, ce);//New Function to validate Qty
                        this.CheckCompanionItems(oExistingRow.ItemID, newQty);
                    }
                    else
                    {
                        if (!isFetchRx) // Added by Manoj 2/25/2013 This will stop the already Pickedup rx not to appear in the grid.
                        {
                            DataRow[] transdetailRow = this.oPOSTrans.oTransDData.TransDetail.Select(" TransDetailID ='" + oPOSTrans.oTDRow.TransDetailID + "'");
                            if (transdetailRow != null && transdetailRow.Length > 0)
                            {
                                oPOSTrans.oTDRow.TransDetailID = clsUIHelper.GetNextNumber(oPOSTrans.oTransDData, clsPOSDBConstants.TransDetail_Fld_TransDetailID);
                            }
                            this.oPOSTrans.oTransDData.TransDetail.Rows.Add(this.oPOSTrans.oTDRow.ItemArray);
                            //Following Added by Krishna on 2 June 2011
                            if (grdDetail.Rows.Count == 1)
                            {
                                TransStartDateTime = DateTime.Now.ToString();//To Capture TranXn Start time;
                            }
                            //Till Here Added by Krishna on 2 June 2011
                            if (Configuration.CPOSSet.UsePoleDisplay)
                            {
                                ShowItemOnPoleDisp(oPOSTrans.oTDRow);//end
                            }
                            if (Configuration.CPOSSet.UseSigPad)
                            {
                                if (SigPadUtil.DefaultInstance.IsVF)
                                {
                                    DeviceItemsProcess("AddItem", oPOSTrans.oTDRow, grdDetail.Rows.Count - 1);
                                }
                                else
                                {
                                    SigPadUtil.DefaultInstance.AddItem(oPOSTrans.oTDRow, grdDetail.Rows.Count - 1);
                                }
                            }
                            this.CheckCompanionItems();
                            this.ClearItemRow();
                            if (this.grdDetail.Rows.Count > 0)
                            {
                                this.changeStToolbars(TransactionStToolbars.strTBTerminalEntery);
                            }
                            else
                            {
                                this.changeStToolbars(TransactionStToolbars.strTBTerminal);
                            }
                        }
                    }
                    this.isAddRow = false;
                    this.isEditRow = false;
                    logger.Trace("ValidateRow() - Item add mode validation completed");
                }
                else if (this.isEditRow == true)
                {
                    logger.Trace("ValidateRow() - Item edit mode validation started");
                    //Added By shitaljit fo pRIMEPOS- 801 for
                    //String is not valid datetime error while adding RX item in transaction on 18 Arpil 2013
                    if (grdDetail.Rows.Count == 1)
                    {
                        TransStartDateTime = DateTime.Now.ToString();//To Capture TranXn Start time;
                    }

                    //this will check if the data is there in the dataset
                    //as we are using this dataset as datasource for the grid
                    DataRow[] transdetailRow = this.oPOSTrans.oTransDData.TransDetail.Select(" TransDetailID ='" + oPOSTrans.oTDRow.TransDetailID + "'");
                    if (transdetailRow != null && transdetailRow.Length == 0)
                    {
                        this.oPOSTrans.oTransDData.TransDetail.Rows.Add(this.oPOSTrans.oTDRow.ItemArray);
                    }
                    //End of Added by SRT(Abhishek)

                    this.grdDetail.UpdateData();
                    this.grdDetail.Refresh();

                    try
                    {
                        int LineLen = 0;
                        if (Configuration.CPOSSet.UsePoleDisplay)
                        {
                            LineLen = Configuration.CPOSSet.PD_LINELEN;
                            frmMain.PoleDisplay.ClearPoleDisplay();
                        }
                        string strItem = "";
                        if (Configuration.CPOSSet.PrintRXDescription == false && grdDetail.ActiveRow.Cells[clsPOSDBConstants.TransDetail_Fld_ItemID].Text == "RX")
                        {
                            strItem = this.grdDetail.ActiveRow.Cells[clsPOSDBConstants.TransDetail_Fld_ItemDescription].Text.ToString();
                            strItem = strItem.Substring(0, strItem.IndexOf("-"));
                        }
                        else
                        {
                            strItem = this.grdDetail.ActiveRow.Cells[clsPOSDBConstants.TransDetail_Fld_ItemDescription].Text.ToString();
                        }
                        String ExPrice = this.grdDetail.ActiveRow.Cells[clsPOSDBConstants.TransDetail_Fld_ExtendedPrice].Text.ToString();
                        if (Configuration.CPOSSet.UsePoleDisplay)
                        {
                            frmMain.PoleDisplay.ClearPoleDisplay();
                            if (LineLen > strItem.Length)
                                frmMain.PoleDisplay.WriteToPoleDisplay(strItem
                                    + clsUIHelper.Spaces(Configuration.CPOSSet.PD_LINELEN - strItem.Length - 7) + " " + Convert.ToDecimal(ExPrice).ToString(Configuration.CInfo.CurrencySymbol + "##0.00"));
                            else
                                frmMain.PoleDisplay.WriteToPoleDisplay(strItem.Substring(0, LineLen - 7)
                                    + " " + Convert.ToDecimal(ExPrice).ToString(Configuration.CInfo.CurrencySymbol + "##0.00"));
                        }
                        if (Configuration.CPOSSet.UseSigPad == true)
                        {
                            //Added by Manoj 10/13/2011 - this is to get know if the item  was onhold or not
                            if (oPOSTrans.oTDRow.TransID > 0 && onHoldTransID > 0)
                            {
                                SigPadUtil.DefaultInstance.iSItemOnHold(true);
                            }
                            else
                            {
                                SigPadUtil.DefaultInstance.iSItemOnHold(false);
                            }
                            logger.Trace("ValidateRow() - POSTransaction Edit Item-Update to Sig Pad", "ValidateRow()", " ItemId: " + oPOSTrans.oTDRow.ItemID + " ItemDescription: " + oPOSTrans.oTDRow.ItemDescription + " TransDetailID: " + oPOSTrans.oTDRow.TransDetailID);
                            int index = oPOSTrans.GetTransIndex(oPOSTrans.oTransDData, oPOSTrans.oTDRow.TransDetailID);
                            if (SigPadUtil.DefaultInstance.IsVF)
                                DeviceItemsProcess("UpdateItem", oPOSTrans.oTDRow, index);
                            else
                                SigPadUtil.DefaultInstance.UpdateItem(oPOSTrans.oTDRow, index);
                        }
                    }
                    catch (Exception Exp)
                    {
                        logger.Fatal(Exp, "ValidateRow()");
                    }
                    logger.Trace("ValidateRow() - Item edit mode validation Completed");
                }
                oPOSTrans.ProcessItemsForComboPricing(oPOSTrans.oTransDData.TransDetail, oPOSTrans.oTDRow, false, oPOSTrans.oTDTaxData);
                logger.Trace("ValidateRow() - " + clsPOSDBConstants.Log_Exiting);
            }
            catch (Exception exp)
            {
                logger.Fatal(exp, "ValidateRow()");
                clsUIHelper.ShowErrorMsg(exp.Message);
                e.Cancel = true;
            }
        }

        //completed by sandeep
        private string SearchItem(string sItemCode, string sDescription)
        {
            logger.Trace("SearchItem() - " + clsPOSDBConstants.Log_Entering);
            string strCode = "";
            try
            {
                if (sItemCode.Trim() == "" && sDescription.Trim() == "")
                {
                    sItemCode = "--##--";
                }

                frmSearch oSearch = new frmSearch(clsPOSDBConstants.Item_tbl, sItemCode, sDescription);

                if (isDescriptionOverride)
                    oSearch.IsCanceled = false;
                else
                    oSearch.ShowDialog(this);

                if (!oSearch.IsCanceled)
                {
                    strCode = oSearch.SelectedRowID();
                    this.txtItemCode.Text = oSearch.SelectedRowID();
                    //Start :Added by Amit Date 07 April 2011 Comment:To add item directly to grid of POStransaction form search form
                    if (this.txtDescription.Focused == true)
                        this.txtItemCode.Focus();
                    this.SelectNextControl(this.ActiveControl, true, true, true, true);
                    //End
                }
                if (txtCustomer.Text.Trim() != "-1" && Configuration.CSetting.EnableCustomerEngagement) // Add Condition - NileshJ PRIMEPOS-2794
                {
                    // rightTabPayCust.Tabs[1].Visible = true; // Nileshj
                    GetCustomerDetails(); //SAJID PRIMEPOS-2794
                    rightTabPayCust.Tabs[1].Selected = true;
                }
                logger.Trace("SearchItem() - " + clsPOSDBConstants.Log_Exiting);
            }
            catch (Exception exp)
            {
                logger.Fatal(exp, "SearchItem()");
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
            return strCode;
        }

        public void SetEBTItemToGrid(ItemRow oItemRow)
        {
            #region Set Item EBT Info

            if (oItemRow.IsEBTItem != oPOSTrans.oTDRow.IsEBTItem)
            {
                string CurrentVal = Configuration.convertNullToString(grdDetail.ActiveRow.Cells[clsPOSDBConstants.TransDetail_Fld_Category].Value);
                if (oItemRow.IsEBTItem == true)
                {
                    if (CurrentVal.Contains("E") == false)
                    {
                        grdDetail.ActiveRow.Cells[clsPOSDBConstants.TransDetail_Fld_Category].Value += (string.IsNullOrEmpty(CurrentVal) == true) ? "E" : ",E";
                    }
                }
                else
                {
                    grdDetail.ActiveRow.Cells[clsPOSDBConstants.TransDetail_Fld_Category].Value = grdDetail.ActiveRow.Cells[clsPOSDBConstants.TransDetail_Fld_Category].Text.ToString().Replace("E", "");
                    ;
                }
                CurrentVal = Configuration.convertNullToString(grdDetail.ActiveRow.Cells[clsPOSDBConstants.TransDetail_Fld_Category].Value);
                if (CurrentVal.EndsWith(",") == true)
                {
                    CurrentVal = CurrentVal.Substring(0, CurrentVal.Length - 1);
                }
                grdDetail.ActiveRow.Cells[clsPOSDBConstants.TransDetail_Fld_Category].Value = CurrentVal;
                oPOSTrans.oTDRow.IsEBTItem = oItemRow.IsEBTItem;
                grdDetail_InitializeRow(grdDetail, new InitializeRowEventArgs(this.grdDetail.ActiveRow, true));
            }

            #endregion Set Item EBT Info
        }
        public void SetMonitoredItemToGrid(ItemRow oItemRow)
        {
            #region Set Item Monitered Info

            if (oItemRow.isOTCItem != oPOSTrans.oTDRow.IsMonitored)
            {
                string CurrentVal = Configuration.convertNullToString(grdDetail.ActiveRow.Cells[clsPOSDBConstants.TransDetail_Fld_Category].Value);
                if (oItemRow.isOTCItem == true)
                {
                    if (CurrentVal.Contains("M") == false)
                    {
                        grdDetail.ActiveRow.Cells[clsPOSDBConstants.TransDetail_Fld_Category].Value += (string.IsNullOrEmpty(CurrentVal) == true) ? "M" : ",M";
                    }
                }
                else
                {
                    grdDetail.ActiveRow.Cells[clsPOSDBConstants.TransDetail_Fld_Category].Value = grdDetail.ActiveRow.Cells[clsPOSDBConstants.TransDetail_Fld_Category].Text.ToString().Replace("M", "");
                    ;
                }
                CurrentVal = Configuration.convertNullToString(grdDetail.ActiveRow.Cells[clsPOSDBConstants.TransDetail_Fld_Category].Value);
                if (CurrentVal.EndsWith(",") == true)
                {
                    CurrentVal = CurrentVal.Substring(0, CurrentVal.Length - 1);
                }
                grdDetail.ActiveRow.Cells[clsPOSDBConstants.TransDetail_Fld_Category].Value = CurrentVal;
                oPOSTrans.oTDRow.IsMonitored = oItemRow.isOTCItem;
                grdDetail_InitializeRow(grdDetail, new InitializeRowEventArgs(this.grdDetail.ActiveRow, true));
            }

            #endregion Set Item Monitered Info

        }

        private void AskApplyToGrid(string strTaxPolicy)
        {
            oPOSTrans.GetTransDetailRow(grdDetail.ActiveRow.Cells[clsPOSDBConstants.TransDetail_Fld_TransDetailID].Value.ToString(), grdDetail.ActiveRow.ListIndex);
            TaxCodes oTaxCodes = new TaxCodes();
            ItemData oItemData = oPOSTrans.PopulateItemList(" where ItemId='" + oPOSTrans.oTDRow.ItemID + "'");
            ItemRow oItemRow = (ItemRow)oItemData.Tables[0].Rows[0];
            frmDiscountOptions.CallDiscountFrm = false;
            int oldSaleLimit = oItemRow.SaleLimitQty;
            //decimal DicountAmt = (oItemRow.Discount * oItemRow.SellingPrice) / 100;
            decimal DicountAmt = oPOSTrans.CalculateDiscount(oItemRow.ItemID, oPOSTrans.oTDRow.QTY, oPOSTrans.oTDRow.Price);
            //Added By shitaljit on 16 Sept 2011
            string MSG = "";

            #region Logic to Populate TaxCode For Item according to new TaxPolicy logic
            bool isDeptHasDisc = false;
            DepartmentData oDeptData;
            bool bTaxChanged = false;
            TaxCodesData oItemTaxCodesData = oPOSTrans.PopulateTaxCodeAccrdTONewTaxPolicy(ref oItemRow, ref isDeptHasDisc, out oDeptData, Configuration.convertNullToString(oPOSTrans.oTDRow.TaxCode), ref bTaxChanged);
            #endregion Logic to Populate TaxCode For Item according to new TaxPolicy logic

            #region if item price is under combo price in current trans then only Description and Tax is allowed to change.
            //PRIMEPOS-2500 03-Apr-2018 JY oPOSTrans.oTDRow.TaxID != oItemRow.TaxID replaced with new tax condition 
            if (oPOSTrans.oTDRow.ItemComboPricingID > 0)
            {
                if (bTaxChanged == false && oItemRow.Description == oPOSTrans.oTDRow.ItemDescription)
                {
                    return;
                }
                else if (bTaxChanged == true && oItemRow.Description != oPOSTrans.oTDRow.ItemDescription)
                {
                    MSG = "You have changed the Description,TaxCode.\nDo you want to Apply these changes on Current Transaction?";
                }
                else if (bTaxChanged == true && oItemRow.Description == oPOSTrans.oTDRow.ItemDescription)
                {
                    MSG = "You have changed the TaxCode.\nDo you want to Apply these changes on Current Transaction?";
                }
                else if (bTaxChanged == false && oItemRow.Description != oPOSTrans.oTDRow.ItemDescription)
                {
                    MSG = "You have changed the Description.\nDo you want to Apply these changes on Current Transaction?";
                }
            }

            if (MSG != "" && Resources.Message.Display(MSG, "Update Current Transaction", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
            {
                if (bTaxChanged)
                {
                    //oPOSTrans.ApplyTaxPolicy(oItemRow, oPOSTrans.oTDRow.TransDetailID);   //Sprint-26 - PRIMEPOS-XXXX 01-Sep-2017 JY Added TransDetailId to bind TransDetailTax record with TransDetail record
                    if (UserPriviliges.getPermission(UserPriviliges.Modules.POSTransaction.ID, UserPriviliges.Screens.POSTransaction.ID, UserPriviliges.Permissions.TaxOverride.ID, UserPriviliges.Permissions.TaxOverride.Name))   //PRIMEPOS-2514 08-May-2018 JY Added
                        oPOSTrans.UpdateTaxCode(oPOSTrans.oTDTaxData, oItemTaxCodesData, oPOSTrans.oTDRow.TransDetailID);   //PRIMEPOS-2500 03-Apr-2018 JY Added logic to update tax
                }
                if (oItemRow.Description != oPOSTrans.oTDRow.ItemDescription)
                {
                    oPOSTrans.oTDRow.ItemDescription = oItemRow.Description;
                }
                return;
            }
            #endregion if item price is under combo price in current trans then only Description and Tax is allowed to change.
            //End of Added By shitaljit on 16 Sept 2011

            #region GetDisplayMSG
            bool bSellingPriceChanged = false;  //PRIMEPOS-2514 08-May-2018 JY Added
            bool bDiscountChanged = false;      //PRIMEPOS-2514 09-May-2018 JY Added
            bool bIsNonRefundableChanged = false;  //PRIMEPOS-2592 06-Nov-2018 JY Added 
            MSG = oPOSTrans.GetTaxPolicyMessage(oItemRow, DicountAmt, strTaxPolicy, isDeptHasDisc, bTaxChanged, ref bSellingPriceChanged, ref bDiscountChanged, ref bIsNonRefundableChanged);
            if (MSG == "")
                return;
            #endregion GetDisplayMSG

            if (MSG != "" && Resources.Message.Display(MSG, "Update Current Transaction", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
            {
                bool isSaleAppy = false;
                int extQty = 0;
                decimal OrgGrdSellingPrice = oPOSTrans.oTDRow.Price;
                string ItemId = grdDetail.ActiveRow.Cells[clsPOSDBConstants.TransDetail_Fld_ItemID].Text;

                try
                {
                    bool bRestrictOverrideScreenForTaxChange = false;
                    if (bSellingPriceChanged)
                    {
                        string sUserID = string.Empty;
                        if (UserPriviliges.getPermission(UserPriviliges.Modules.POSTransaction.ID, UserPriviliges.Screens.POSTransaction.ID, UserPriviliges.Permissions.PriceOverridefromPOSTrans.ID, UserPriviliges.Permissions.PriceOverridefromPOSTrans.Name, out sUserID))
                        {
                            bRestrictOverrideScreenForTaxChange = true;
                            if (oItemRow.SellingPrice < oItemRow.LastCostPrice)
                            {
                                if (isPromptForSellingPriceLessThanCost(oPOSTrans.oTDRow.ItemID, oItemRow.SellingPrice))
                                {
                                    this.txtItemCode.Focus();
                                    return;
                                }
                            }
                            OrgGrdSellingPrice = oPOSTrans.oTDRow.NonComboUnitPrice = oPOSTrans.oTDRow.Price = oItemRow.SellingPrice;
                            oPOSTrans.GetSalePrice(oItemRow, oDeptData, ref extQty, ref isSaleAppy);
                        }
                        bDiscountChanged = false;   //if we change selling price of discountable item, then it should apply correct discount, so no need to bring up security screen for discount override
                    }
                    if (bTaxChanged)
                    {
                        if (bRestrictOverrideScreenForTaxChange || UserPriviliges.getPermission(UserPriviliges.Modules.POSTransaction.ID, UserPriviliges.Screens.POSTransaction.ID, UserPriviliges.Permissions.TaxOverride.ID, UserPriviliges.Permissions.TaxOverride.Name))   //PRIMEPOS-2514 08-May-2018 JY Added
                            oPOSTrans.UpdateTaxCode(oPOSTrans.oTDTaxData, oItemTaxCodesData, oPOSTrans.oTDRow.TransDetailID);   //PRIMEPOS-2500 03-Apr-2018 JY Added logic to update tax
                    }

                    bool bApplyDiscount = true;
                    if (bDiscountChanged)
                    {
                        if (!UserPriviliges.getPermission(UserPriviliges.Modules.POSTransaction.ID, UserPriviliges.Screens.POSTransaction.ID, UserPriviliges.Permissions.DiscOverridefromPOSTrans.ID, UserPriviliges.Permissions.DiscOverridefromPOSTrans.Name))
                        {
                            bApplyDiscount = false;
                        }
                    }

                    #region PRIMEPOS-2592 06-Nov-2018 JY Added 
                    if (bIsNonRefundableChanged)
                    {
                        oPOSTrans.oTDRow.IsNonRefundable = oItemRow.IsNonRefundable;
                    }
                    #endregion

                    SetEBTItemToGrid(oItemRow);
                    SetMonitoredItemToGrid(oItemRow);

                    ValidateItemQty(this, new CancelEventArgs(), bApplyDiscount);   //PRIMEPOS-2514 09-May-2018 JY Added bApplyDiscount parameter

                    #region TaxPolicy Logic
                    //oPOSTrans.UpdateTransTaxDetails(oPOSTrans.oTDTaxData, oItemRow.ItemID);
                    //oPOSTrans.ApplyTaxPolicy(oItemRow, oPOSTrans.oTDRow.TransDetailID);   //Sprint-26 - PRIMEPOS-XXXX 01-Sep-2017 JY Added TransDetailId to bind TransDetailTax record with TransDetail record
                    #endregion TaxPolicy Logic
                    //End of added by shitaljit 0n 23 august 2011

                    oPOSTrans.CalcExtdPrice(oPOSTrans.oTDRow);
                    //Start : Added By Amit Date 24 May 2011
                    //Modified by shitaljit add Math.Abs() to work in return mode.
                    if ((Math.Abs(oPOSTrans.oTDRow.ExtendedPrice + oPOSTrans.oTDRow.TaxAmount) - Math.Abs(oPOSTrans.oTDRow.Discount)) < 0)
                    {
                        trPrice = oPOSTrans.oTDRow.Price;
                        decimal itmPrice = oItemRow.SellingPrice;
                        decimal discount = oPOSTrans.oTDRow.Discount;

                        oDiscountOptions = new frmDiscountOptions(trPrice, OrgGrdSellingPrice, discount);
                        oDiscountOptions.ShowDialog();
                        if (frmDiscountOptions.CallDiscountFrm == true)
                            tbEditItemActions("D");
                        else
                        {
                            oPOSTrans.oTDRow.Discount = oDiscountOptions.discount;
                            oPOSTrans.oTDRow.NonComboUnitPrice = oPOSTrans.oTDRow.Price = oDiscountOptions.trPrice;
                            oPOSTrans.oTDRow.ExtendedPrice = oPOSTrans.oTDRow.Price;
                            oPOSTrans.oTDRow.TaxCode = "";
                        }
                    }
                    //End

                    //Start: Added By Amit Date 26 May 2011
                    TransDetailRow tempTDRow = oPOSTrans.oTDRow;
                    int selectedRowtranceiD = oPOSTrans.oTDRow.TransDetailID;
                    for (int i = 0; i < this.grdDetail.Rows.Count; i++)
                    {
                        //grdDetail.Rows[i].Activated;
                        if (this.grdDetail.Rows[i].Cells[clsPOSDBConstants.Item_Fld_ItemID].Value.ToString() == ItemId && !this.grdDetail.Rows[i].Activated)
                        {
                            isSaleAppy = false;
                            oPOSTrans.GetTransDetailRow(grdDetail.Rows[i].Cells[clsPOSDBConstants.TransDetail_Fld_TransDetailID].Value.ToString(), grdDetail.Rows[i].ListIndex);
                            ItemData oItemDatatemp = oPOSTrans.PopulateItemList(" where ItemId='" + oPOSTrans.oTDRow.ItemID + "'");
                            ItemRow oItemRowTemp = (ItemRow)oItemData.Tables[0].Rows[0];
                            if (oPOSTrans.oTDRow.TransDetailID == selectedRowtranceiD)
                            {
                                continue;
                            }
                            oPOSTrans.oTDRow.ItemDescription = tempTDRow.ItemDescription;
                            oPOSTrans.oTDRow.NonComboUnitPrice = oPOSTrans.oTDRow.Price = tempTDRow.Price;
                            if (oItemRow.isOnSale == true)
                            {
                                //Add by Ravindra (QuicSolv) PRIMEPOS-670 8 April 2013
                                foreach (UltraGridRow ActRow in grdDetail.Rows)
                                {
                                    if (ActRow.Cells["TransDetailID"].Value.ToString() == oPOSTrans.oTDRow.TransDetailID.ToString())
                                    {
                                        this.grdDetail.ActiveRow = ActRow;
                                        break;
                                    }
                                }
                                if ((oPOSTrans.oTDRow.QTY + extQty) <= 0)
                                {
                                    bool tempflag = DelFromGridFlag;
                                    DelFromGridFlag = true;
                                    extQty = oPOSTrans.oTDRow.QTY + extQty;
                                    oPOSTrans.oTDRow.IsPriceChanged = false;
                                    this.grdDetail.ActiveRow.Delete(true);
                                    oPOSTrans.UpdateTransTaxDetails(oPOSTrans.oTDTaxData, oPOSTrans.oTDRow.ItemID);
                                    this.grdDetail.UpdateData();
                                    this.grdDetail.Refresh();
                                    DelFromGridFlag = tempflag;
                                    continue;
                                }
                                else
                                {
                                    oPOSTrans.oTDRow.QTY = oPOSTrans.oTDRow.QTY + extQty;
                                    extQty = 0;
                                }
                                //till here Add by Ravindra (QuicSolv) PRIMEPOS-670 8 April 2013
                                int setSalelimit = oItemRow.SaleLimitQty;
                                if (oItemRow.SaleStartDate != DBNull.Value && oItemRow.SaleEndDate != DBNull.Value)
                                    if (DateTime.Now.Date >= Convert.ToDateTime(oItemRow.SaleStartDate).Date && DateTime.Now.Date <= Convert.ToDateTime(oItemRow.SaleEndDate).Date)
                                    {
                                        //Added by SRT(Abhishek) Date : 05/09/2009

                                        //Add by Ravindra (QuicSolv) PRIMEPOS-670 8 April 2013
                                        if (setSalelimit == 0)
                                        {
                                            oPOSTrans.oTDRow.NonComboUnitPrice = oPOSTrans.oTDRow.Price = Math.Round(oItemRow.OnSalePrice, 2);
                                            // End of Added by SRT(Abhishek) Date : 05/09/2009
                                            oPOSTrans.oTDRow.IsPriceChanged = true;
                                            isSaleAppy = true;
                                        }
                                        else //Add by Ravindra (QuicSolv) PRIMEPOS-670 8 April 2013
                                        {
                                            oPOSTrans.oTDRow.NonComboUnitPrice = oPOSTrans.oTDRow.Price = oItemRow.SellingPrice;
                                            oPOSTrans.oTDRow.IsPriceChanged = false;
                                        }
                                    }
                            }
                            //Added By Ravindra
                            oPOSTrans.CalculateDiscount(oItemRow.ItemID, oPOSTrans.oTDRow.QTY, oPOSTrans.oTDRow.Price); //Add by Ravindra (QuicSolv) PRIMEPOS-670 8 April 2013

                            oPOSTrans.oTDRow.Discount = oPOSTrans.CalculateDiscount(oItemRow.ItemID, oPOSTrans.oTDRow.QTY, oPOSTrans.oTDRow.Price); //Add by Ravindra (QuicSolv) PRIMEPOS-670 8 April 2013

                            oPOSTrans.oTDRow.ExtendedPrice = tempTDRow.ExtendedPrice;
                            oPOSTrans.oTDRow.TaxAmount = tempTDRow.TaxAmount;
                            oPOSTrans.ApplyTaxPolicy(oItemRow, oPOSTrans.oTDRow.TransDetailID);   //Sprint-26 - PRIMEPOS-XXXX 01-Sep-2017 JY Added TransDetailId to bind TransDetailTax record with TransDetail record
                            oPOSTrans.CalcExtdPrice(oPOSTrans.oTDRow);
                        }
                    }

                    if (extQty > 0)
                    {
                        isQtyChange = false;
                        bool changemovetemp = Configuration.moveToQtyCol;
                        if (changemovetemp)
                        {
                            Configuration.moveToQtyCol = false;
                        }
                        // this.txtQty.Value = extQty;
                        this.txtItemCode.Focus();
                        this.txtItemCode.Value = extQty + "/" + oPOSTrans.oTDRow.ItemID.ToString();
                        this.txtQty.Focus();
                        // FKEdit(this.txtQty.Value.ToString().Trim() + "/" + oTDRow.ItemID, clsPOSDBConstants.Item_tbl, false);
                        if (changemovetemp)
                        {
                            Configuration.moveToQtyCol = changemovetemp;
                        }
                        this.txtItemCode.Focus();
                    }
                    if (Configuration.CPOSSet.UseSigPad == true)
                    {
                        logger.Trace("AskApplyToGrid() - ItemId: " + oPOSTrans.oTDRow.ItemID + " ItemDescription: " + oPOSTrans.oTDRow.ItemDescription + " TransDetailID: " + oPOSTrans.oTDRow.TransDetailID);
                        int index = oPOSTrans.GetTransIndex(oPOSTrans.oTransDData, oPOSTrans.oTDRow.TransDetailID);
                        if (SigPadUtil.DefaultInstance.IsVF)
                            DeviceItemsProcess("UpdateItem", oPOSTrans.oTDRow, index);
                        else
                            SigPadUtil.DefaultInstance.UpdateItem(oPOSTrans.oTDRow, index);
                    }
                    //End
                    ClearItemRow();
                }
                catch (Exception ex)
                {
                    logger.Fatal(ex, "AskApplyToGrid()");
                    clsUIHelper.ShowErrorMsg(ex.Message);
                }
            }
        }

        //completed by sandeep
        private bool isPromptForSellingPriceLessThanCost(string ItemCode, decimal SellingPrice)
        {
            bool RetVal = false;

            bool PriceOverrideLessThanCostPrice = UserPriviliges.IsUserHasPriviliges(UserPriviliges.Modules.POSTransaction.ID, UserPriviliges.Screens.POSTransaction.ID, UserPriviliges.Permissions.PriceOverrideLessThanCostPrice.ID);

            if (!(PriceOverrideLessThanCostPrice == true && Configuration.CInfo.PromptForSellingPriceLessThanCost == false))
            {
                ItemRow oItemRow = null;
                oPOSTrans.GetItemRowByItemId(ItemCode, ref oItemRow);
                if (oItemRow != null)
                {
                    if (!(SellingPrice > 0 || SellingPrice == 0))   //|| SellingPrice == 0 added to handle 0 selling price override 
                    {
                        SellingPrice = oItemRow.SellingPrice;
                    }
                    if (SellingPrice < oItemRow.LastCostPrice)
                    {
                        if (PriceOverrideLessThanCostPrice == false)
                        {
                            Resources.Message.Display("Selling price " + Configuration.CInfo.CurrencySymbol.ToString() + SellingPrice + " is less than cost price " + Configuration.CInfo.CurrencySymbol.ToString() + oItemRow.LastCostPrice.ToString("##########0.00") + Environment.NewLine + " You are not authorized to perform this action.", "Selling price Less than cost price", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
                            RetVal = true;
                        }
                        else
                        {
                            if (Configuration.CInfo.PromptForSellingPriceLessThanCost == true)
                            {
                                if (Resources.Message.Display("Selling price " + Configuration.CInfo.CurrencySymbol.ToString() + SellingPrice + " is less than cost price " + Configuration.CInfo.CurrencySymbol.ToString() + oItemRow.LastCostPrice.ToString("##########0.00") + Environment.NewLine + " Do you want to continue?", "Selling price Less than cost price", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.No)
                                    RetVal = true;
                            }
                        }
                    }
                }
            }
            return RetVal;
        }

        private void FKEdit(string code, string senderName)//Remove extra parameter by shitaljit as it is not using inany function call , bool searchByItemDescription)
        {
            logger.Trace("FKEdit() - " + senderName + " - " + clsPOSDBConstants.Log_Entering);
            if (senderName == clsPOSDBConstants.Item_tbl)
            {
                #region Items
                try
                {
                    System.Int32 QTY = 0;
                    System.Decimal SalePrice = 0;
                    SeperateItem(ref code, ref QTY, ref SalePrice);
                    ItemRow oItemRow = null;
                    string MaxDiscountLimitUser = string.Empty; //PRIMEPOS-2402 16-Jul-2021 JY Added
                    ValidateItemForAddingInTransaction(ref code, ref QTY, ref SalePrice, out oItemRow, out MaxDiscountLimitUser);
                    if (oItemRow != null)
                    {
                        txtQty.Enabled = Configuration.moveToQtyCol;
                        oPOSTrans.IsItemExist = true;

                        if (Configuration.CPOSSet.ShowItemNotes == true)
                        {
                            logger.Trace("FKEdit - " + senderName + " - About to ShowItemNotes" + oItemRow.ItemID);
                            ShowItemNotes(oItemRow.ItemID);//Added By shitaljit(QuicSolv) on 10 oct 2011
                            logger.Trace("FKEdit - " + senderName + " - ShowItemNotes completed" + oItemRow.ItemID);
                        }

                        ShowItemWarningMessage(oItemRow);
                        DepartmentData oDeptData;
                        bool isDeptTaxable = false;
                        oPOSTrans.SetTransDetails(oItemRow, DefaultQTY, CouponItemDesc, CouponID, ref QTY, ref isQtyChange, ref ExtraQty, ref isDeptTaxable, out oDeptData);
                        oPOSTrans.oTDRow.MaxDiscountLimitUser = MaxDiscountLimitUser; //PRIMEPOS-2402 16-Jul-2021 JY Added
                        #region get saleprice
                        if (SalePrice == 0)
                        {
                            //Added by SRT(Abhishek) Date : 05/09/2009
                            oPOSTrans.oTDRow.NonComboUnitPrice = oPOSTrans.oTDRow.Price = Math.Round(oItemRow.SellingPrice, 2);
                            if (oPOSTrans.GetExistingQuantity(oPOSTrans.oTDRow, oPOSTrans.oTransDData) <= 0 && SalePrice == 0 && oItemRow.LastCostPrice > oPOSTrans.oTDRow.Price)
                            {
                                if (isPromptForSellingPriceLessThanCost(code, oPOSTrans.oTDRow.Price))
                                {
                                    txtItemCode.Focus();
                                    return;
                                }
                            }
                            //End of Added by SRT(Abhishek) Date : 05/09/2009
                            oPOSTrans.SetSalePrice(oItemRow, oDeptData);
                        }
                        else
                        {
                            //Added by SRT(Abhishek) Date : 05/09/2009
                            oPOSTrans.oTDRow.NonComboUnitPrice = oPOSTrans.oTDRow.Price = Math.Round(SalePrice, 2);
                            //End of Added by SRT(Abhishek) Date : 05/09/2009
                            oPOSTrans.oTDRow.IsPriceChanged = true;
                            oPOSTrans.oTDRow.IsPriceChangedByOverride = true;   //PRIMEPOS-1871 23-Mar-2020 JY Added
                        }
                        #endregion get saleprice  

                        oPOSTrans.CalculateDiscount(oItemRow.ItemID, oPOSTrans.oTDRow.QTY, oPOSTrans.oTDRow.Price);
                        if (oPOSTrans.CurrentTransactionType == POS_Core.TransType.POSTransactionType.SalesReturn)
                        {
                            oPOSTrans.oTDRow.Discount = oPOSTrans.CalculateDiscount(oItemRow.ItemID, oPOSTrans.oTDRow.QTY, oPOSTrans.oTDRow.Price);
                        }

                        this.txtDescription.Enabled = false;
                        this.EnabledDisableItemRow(false);
                        setItemValues(oPOSTrans.oTDRow);

                        #region get Tax
                        //Following Logic for Tax on item is added By Shitaljit(QuicSolv) on 23 August 2011
                        oPOSTrans.ApplyTaxPolicy(oItemRow, oPOSTrans.oTDRow.TransDetailID);   //Sprint-26 - PRIMEPOS-XXXX 01-Sep-2017 JY Added TransDetailId to bind TransDetailTax record with TransDetail record
                        #endregion get Tax

                        if (oItemRow.Itemtype == "3" && Configuration.CInfo.ConsiderItemType == true)   //Sprint-22 17-Dec-2015 JY Added condition "Configuration.CInfo.ConsiderItemType"
                        {
                            this.EnabledDisableItemRow(true);
                            this.txtDescription.Enabled = true;
                            this.txtDescription.Focus();
                        }
                        else if ((Configuration.moveToQtyCol == true || (oItemRow.Itemtype == "2" && oPOSTrans.skipMoveNext == false && Configuration.CInfo.ConsiderItemType == true)) && (oItemRow.ItemID != Configuration.CouponItemCode))    //Sprint-22 11-Dec-2015 JY Added "oItemRow.ItemID == Configuration.CouponItemCode" because if "Application-Preferences-User settings-Stop At Qty Col?" then we can not add discount coupon in transaction //Sprint-22 17-Dec-2015 JY Added condition "Configuration.CInfo.ConsiderItemType"
                        {
                            CancelEventArgs ce = new CancelEventArgs(false);
                            string sUserID = string.Empty;

                            #region Sprint-26 - PRIMEPOS-2416 25-Jul-2017 JY Added logic to bypass "Stop at Qty" even if it is ON, if "Quantity Override" is unchecked
                            if (!UserPriviliges.IsUserHasPriviliges(UserPriviliges.Modules.POSTransaction.ID, UserPriviliges.Screens.POSTransaction.ID, UserPriviliges.Permissions.QuantityOverride.ID))
                            {
                                this.ValidateItemQty(txtQty, ce);
                            }
                            else
                            {
                                if (!isQtyChange)
                                {
                                    txtItemCode.Validating -= new CancelEventHandler(ItemBox_Validatiang);
                                    this.txtQty.Focus();
                                    txtItemCode.Validating += new CancelEventHandler(ItemBox_Validatiang);
                                }
                                else//Added by Ravindra for SaleLimit 21 March 2013
                                {
                                    txtItemCode.Validating -= new CancelEventHandler(ItemBox_Validatiang);
                                    this.txtQty.Focus();
                                    txtItemCode.Validating += new CancelEventHandler(ItemBox_Validatiang);
                                    txtQty.Value = Convert.ToInt32(ExtraQty.ToString());
                                    this.txtItemCode.Value = oItemRow.ItemID;
                                    this.ValidateItemQty(txtQty, ce);//New Function to validate Qty
                                    if (ExtraQty > 0)
                                    {
                                        FKEdit(oItemRow.ItemID, senderName);
                                        this.ValidateItemQty(txtQty, ce);//New Function to validate Qty
                                    }
                                    this.txtItemCode.Value = "";
                                    this.txtItemCode.Focus();
                                }
                            }
                            #endregion
                        }
                        else
                        {
                            CancelEventArgs ce = new CancelEventArgs(false);
                            if (!isQtyChange)//If condition Added by Ravindra for SaleLimit 21 March 2013
                            {
                                this.ValidateItemQty(txtQty, ce);//New Function to validate Qty
                            }
                            else//Added by Ravindra for SaleLimit 21 March 2013
                            {
                                //this.ItemBox_Validatiang(txtQty, ce); //Commented by shitaljit to avoid calling ItemBox_Validatiang recursively
                                this.ValidateItemQty(txtQty, ce);//New Function to validate Qty
                                txtQty.Value = Convert.ToInt32(ExtraQty.ToString());
                                this.txtItemCode.Value = oItemRow.ItemID;
                                this.ItemBox_Validatiang(this.txtItemCode, ce);
                                if (ExtraQty > 0)
                                {
                                    FKEdit(ExtraQty + "/" + oItemRow.ItemID, senderName);
                                }
                                this.txtItemCode.Value = "";
                                this.txtItemCode.Focus();
                            }
                        }
                    }
                }
                catch (System.IndexOutOfRangeException Ex)
                {
                    logger.Fatal(Ex, "FKEdit()");
                    clsUIHelper.ShowErrorMsg("Item does not exist");
                    this.txtItemCode.Focus();
                }
                catch (POSExceptions exp)
                {
                    logger.Fatal(exp, "FKEdit()");
                    clsUIHelper.ShowErrorMsg(exp.Message);
                    switch (exp.ErrNumber)
                    {
                        case (long)POSErrorENUM.ItemPrice_Validation:
                            txtItemCode.Focus();
                            break;
                    }
                }
                catch (Exception exp)
                {
                    logger.Fatal(exp, "FKEdit()");
                    clsUIHelper.ShowErrorMsg(exp.Message);

                    this.txtItemCode.Text = String.Empty;
                    this.txtDescription.Text = String.Empty;
                    SearchItem();
                }
                #endregion Items
            }

            logger.Trace("FKEdit() - " + senderName + " - " + clsPOSDBConstants.Log_Exiting);
        }

        //completed by sandeep
        private string SearchItem()
        {
            return SearchItem("", "");
        }

        //completed by sandeep
        private void SeperateItem(ref System.String ItemID, ref System.Int32 QTY, ref System.Decimal SalePrice)
        {
            try
            {
                DataSet dsVendorItem;
                oPOSTrans.SeperateItem(ref ItemID, ref QTY, ref SalePrice, out dsVendorItem);
                logger.Trace("SeperateItem() - " + clsPOSDBConstants.Log_Entering);
                if (dsVendorItem != null)
                {
                    frmSearch ofrmSearch = new frmSearch(clsPOSDBConstants.Item_tbl);
                    ofrmSearch.grdSearch.DataSource = dsVendorItem;
                    ofrmSearch.ShowDialog();
                    if (ofrmSearch.IsCanceled == false)
                    {
                        ItemID = ofrmSearch.SelectedRowID();
                    }
                }
            }
            catch (Exception exp)
            {
                logger.Fatal(exp, "SeperateItem()");
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
            logger.Trace("SeperateItem() - " + clsPOSDBConstants.Log_Exiting);
        }

        //completed by sandeep
        private bool ValidateItemForAddingInTransaction(ref string code, ref int QTY, ref decimal SalePrice, out ItemRow oItemRow, out string MaxDiscountLimitUser) //PRIMEPOS-2402 16-Jul-2021 JY Added MaxDiscountLimitUser
        {
            MaxDiscountLimitUser = "";  //PRIMEPOS-2402 16-Jul-2021 JY Added
            try
            {
                logger.Trace("ValidateItemForAddingInTransaction() - About to Populate item by Item Code, itemCode: " + code);
                oItemRow = null;
                oPOSTrans.GetItemRowByItemId(code, ref oItemRow);
                logger.Trace("ValidateItemForAddingInTransaction() - Populated item completed by Item Code, itemCode: " + code);
                if (oItemRow != null)
                {
                    oPOSTrans.IsItemExist = true;
                    #region Sprint-22 18-Dec-2015 JY Added logic to handle item having item type Non-stock or comment
                    if ((oItemRow.Itemtype == "2" || oItemRow.Itemtype == "3") && Configuration.CInfo.ConsiderItemType == true)
                    {
                        POS_Core_UI.Resources.Message.Display(this, "User can not add the item in transaction having item-type NON-STOCK or COMMENT as the respective settings is ON.", "PrimePOS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.txtItemCode.Focus();
                        oItemRow = null;
                        return false;
                    }
                    #endregion
                    #region Prompt For $0 cost or selling price
                    if ((oItemRow.SellingPrice == 0 && Configuration.CInfo.PromptForZeroSellingPrice == true)
                        || oItemRow.LastCostPrice == 0 && Configuration.CInfo.PromptForZeroCostPrice == true)
                    {
                        UpdatePriceDetails(code, (oItemRow.SellingPrice == 0), ref oItemRow);
                    }
                    #endregion Prompt For $0 cost or selling price

                    if (SalePrice != 0 && oItemRow.SaleStartDate != DBNull.Value && oItemRow.SaleEndDate != DBNull.Value && DateTime.Now.Date >= Convert.ToDateTime(oItemRow.SaleStartDate).Date && DateTime.Now.Date <= Convert.ToDateTime(oItemRow.SaleEndDate).Date
                        && oItemRow.LastCostPrice > SalePrice && Configuration.CInfo.PromptForSellingPriceLessThanCost) //Sprint-27 - PRIMEPOS-2413 15-Sep-2017 JY Added SaleStartDate and SaleEndDate parameters
                    {
                        if (isPromptForSellingPriceLessThanCost(code, SalePrice))
                        {
                            this.txtItemCode.Focus();
                            oItemRow = null;
                            return false;
                        }
                    }

                    #region PRIMEPOS-2979 29-Jun-2021 JY Added
                    try
                    {
                        if (Configuration.convertNullToDecimal(oItemRow.Discount) > 0)
                        {
                            InvDicsValueToVerify = Configuration.convertNullToDecimal(oItemRow.Discount);
                            if (InvDicsValueToVerify > Configuration.UserMaxDiscountLimit)
                            {
                                POS_Core_UI.UserManagement.clsLogin oLogin = new POS_Core_UI.UserManagement.clsLogin();
                                if (oLogin.loginForPreviliges(clsPOSDBConstants.UserMaxDiscountLimit, "", out MaxDiscountLimitUser, "Security Override For Maximum Discount Limit") == false)
                                {
                                    this.txtItemCode.Focus();
                                    oItemRow = null;
                                    return false;
                                }
                            }
                            InvDicsValueToVerify = 0;
                        }
                    }
                    catch (Exception Exp)
                    {
                        logger.Fatal(Exp, "ValidateItemForAddingInTransaction(ref string code, ref int QTY, ref decimal SalePrice, out ItemRow oItemRow) - Max Discount Validation");
                    }
                    #endregion
                }
                else
                {
                    if (oPOSTrans.CheckForMatchingItem(code, ref oItemRow))
                    {
                        return true;
                    }
                    #region Search Item By SKU Code
                    oPOSTrans.IsItemExist = false;
                    logger.Trace("ValidateItemForAddingInTransaction() - About to FindItem item by SKU code: " + code);
                    oItemRow = oPOSTrans.GetItemRowBySKUCode(code);     //PRIMEPOS-2819 05-Mar-2020 JY Added
                    //if (oPOSTrans.GetItemRowBySKUCode(code) != null)  //PRIMEPOS-2819 05-Mar-2020 JY Commented
                    if (oItemRow != null)    //PRIMEPOS-2819 05-Mar-2020 JY Added
                    {
                        logger.Trace("ValidateItemForAddingInTransaction() - Finish FindItem item by SKU code: " + code);
                        return true;
                    }
                    #endregion Search Item By SKU Code

                    bool hasPrevliges = false;
                    try
                    {
                        if (UserPriviliges.IsUserHasPriviliges(UserPriviliges.Modules.InventoryMgmt.ID, UserPriviliges.Screens.ItemFile.ID, -999))  //PRIMEPOS-2488 22-Jun-2020 JY it should check item "add" priviliges - corrected the same
                        {
                            hasPrevliges = true;
                        }
                        oPOSTrans.AddTransactionDetailData(txtItemCode.Text.Trim());
                    }
                    catch (Exception ex)
                    {
                        logger.Fatal(ex, "ValidateItemForAddingInTransaction()");
                        POS_Core.ErrorLogging.ErrorHandler.logException(ex, string.Empty, string.Empty);
                    }
                    if (Configuration.AllowAddItemInTrans == true)
                    {
                        string result;
                        frmItemAdd oAddItem = new frmItemAdd("", this.txtItemCode.Text);
                        oAddItem.ItemID = txtItemCode.Text;
                        //Added by Shitaljit(QuicSolv) on May 2011
                        if (!hasPrevliges)
                        {
                            oAddItem.bttnState = true;
                        }
                        //Till here Added By Shitaljit(QuicSolv
                        oAddItem.ShowDialog();
                        //Added By shitaljit for JIRA-385 on !6May 2013
                        if (oAddItem.ItemID.Equals(Configuration.CInfo.DefaultNonTaxableItem) || oAddItem.ItemID.Equals(Configuration.CInfo.DefaultTaxableItem))
                        {
                            this.txtItemCode.Text += "@" + oAddItem.ItemID;
                            QTY = 0;
                            SalePrice = 0;
                            code = this.txtItemCode.Text;
                            SeperateItem(ref code, ref QTY, ref SalePrice);
                            oPOSTrans.GetItemRowByItemId(code, ref oItemRow);
                            if (oItemRow != null)
                            {
                                return true;
                            }
                        }
                        //End
                        result = oAddItem.btnName;
                        switch (result)
                        {
                            case "SimpleMode":
                                sMode = "S"; //Sprint-19 - 2146 23-Dec-2014 JY Added to set mode
                                this.EnabledDisableItemRow(true);
                                this.txtDiscount.Enabled = false;   //PRIMEPOS-2979 29-Jun-2021 JY Added
                                #region Fetching Item Descriptions for showing Inteligence to users
                                if (Configuration.CInfo.ShowTextPrediction == true)
                                {
                                    this.txtItemDescription.Visible = true;
                                    this.txtItemDescription.BringToFront();
                                    this.txtDescription.Visible = false;
                                    setDescriptionView();

                                    Search oSearch = new Search();
                                    AutoCompleteStringCollection ItemDescCollection = new AutoCompleteStringCollection();
                                    ItemDescCollection = oSearch.GetAutoCompleteCollectionData(clsPOSDBConstants.Item_tbl, clsPOSDBConstants.Item_Fld_Description);
                                    this.txtItemDescription.Enabled = true;
                                    this.txtItemDescription.Focus();
                                    //break;    //PRIMEPOS-2662 19-Mar-2019 JY Commented
                                }
                                #endregion Fetching Item Descriptions for showing Inteligence to users

                                TaxCodesData oTaxCodeData;
                                LoadTaxCodes(this.txtItemCode.Text, out oTaxCodeData);
                                oPOSTrans.oTaxCodesData = oTaxCodeData;
                                this.txtDescription.Enabled = true;
                                this.txtDescription.Focus();
                                break;

                            case "QuickMode":
                                frmItemsQuickAdd FrmQuickAddItem = new frmItemsQuickAdd(oAddItem.ItemID);
                                FrmQuickAddItem.ShowDialog();
                                txtItemCode.Focus();
                                AutoGirdfallFlag = true;
                                break;
                            case "AdvancedMode":
                                frmItems FrmItems = new frmItems(oAddItem.ItemID);
                                FrmItems.ShowDialog();
                                txtItemCode.Focus();
                                AutoGirdfallFlag = true;
                                break;
                            case "Escape":
                                txtItemCode.Focus();
                                break;
                            case "MMSSearch":   //PRIMEPOS-2671 24-Apr-2019 JY Added
                                //if type in itemid/description not found, then user can click on "MMS search", so he can fetch the data from central repositary, 
                                //When we select item from repositorary, it will be added in transaction, before that it will be checked in POS database, if not exists then need to add the same.
                                try
                                {
                                    if (Configuration.CInfo.PHNPINO.Trim() == "")
                                    {
                                        Resources.Message.Display("NPI should not be blank", "PrimePOS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                        break;
                                    }
                                    else if (Configuration.CInfo.PSServiceAddress.Trim() == "")
                                    {
                                        Resources.Message.Display("Please set service address in settings " + Environment.NewLine + " and try again", "PrimePOS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                        break;
                                    }
                                    OpenMMSSearch(oAddItem.ItemID);
                                    txtItemCode.Focus();
                                    AutoGirdfallFlag = true;
                                }
                                catch (Exception exp)
                                {
                                    MessageBox.Show(exp.Message);
                                }
                                break;
                        }
                    }
                    else
                    {
                        oItemRow = null;
                        this.txtItemCode.Focus();
                        return false;
                    }
                }
                if (oItemRow != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception Ex)
            {
                logger.Fatal(Ex, "ValidateItemForAddingInTransaction()");
                oItemRow = null;
                return false;
                throw Ex;
            }
        }
        //completed by sandeep

        #region PRIMEPOS-2671 24-Apr-2019 JY Added
        private void OpenMMSSearch(string ItemID)
        {
            try
            {
                frmSearchMain oSearch = new frmSearchMain(true);
                oSearch.txtCode.Text = ItemID;
                oSearch.SearchTable = clsPOSDBConstants.MMSSearch;
                oSearch.sNPINo = Configuration.CInfo.PHNPINO;
                oSearch.PSServiceAddress = Configuration.CInfo.PSServiceAddress;
                EventArgs e = new EventArgs();
                if (oSearch.txtCode.Text.Trim() != "")
                    oSearch.btnSearch_Click(null, e);
                oSearch.ShowDialog(this);
                if (!oSearch.IsCanceled)
                {
                    ItemM oItemM = new ItemM();
                    oItemM = oSearch.SelectedItemMRow();
                    if (oItemM == null)
                    {
                        Resources.Message.Display("Please select any record....", "PrimePOS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }

                    ItemSvr oItemSvr = new ItemSvr();
                    ItemData oItemData = oItemSvr.Populate(oItemM.ItemID);
                    if (oItemData != null && oItemData.Tables.Count > 0 && oItemData.Item.Rows.Count > 0)
                    {
                        if (Resources.Message.Display("Item already exists in the POS database, do you want to add it into transaction?", "PrimePOS", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                        {
                            txtItemCode.Text = oItemM.ItemID;
                        }
                        else
                            return;
                    }
                    else
                    {
                        #region PRIMEPOS-2701 02-Jul-2019 JY Commented - it is used to save item in POS database
                        //oItemData = new ItemData();
                        //oItemData.Item.AddRow(oItemM.ItemID, Configuration.CInfo.DefaultDeptId, oItemM.Description, oItemM.Itemtype, oItemM.ProductCode,"", oItemM.SeasonCode, oItemM.Unit, Configuration.convertNullToDecimal(oItemM.Freight), Configuration.convertNullToDecimal(oItemM.SellingPrice), 0, 0, oItemM.isTaxable,0, false, 0, DateTime.Now, DateTime.Now, 0, 0, "", 0, 0, 0, DateTime.Now, "", "", DateTime.MinValue, DateTime.Now, "", false, false, false, false, true, 0, oItemM.IsEBTItem, 0, "", true, true, false, 0);
                        //oItemData.Item[0].PckSize = oItemM.PCKSIZE;
                        //oItemData.Item[0].PckQty = oItemM.PCKQTY;
                        //oItemData.Item[0].PckUnit = oItemM.PCKUNIT;
                        //oItemData.Item[0].ManufacturerName = oItemM.ManufacturerName;
                        //oItemSvr.Persist(oItemData);
                        //txtItemCode.Text = oItemM.ItemID;
                        #endregion

                        #region PRIMEPOS-2701 02-Jul-2019 JY Added
                        frmItems FrmItems = new frmItems(oItemM);
                        FrmItems.ShowDialog();
                        if (!FrmItems.IsCanceled)
                            txtItemCode.Text = oItemM.ItemID;
                        #endregion
                    }
                }
            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }
        #endregion

        private ItemRow UpdatePriceDetails(string sItemID, bool isPropmtForSellingPrice, ref ItemRow oItemRow)
        {
            frmItems ofrmItem = new frmItems();
            try
            {
                if (!UserPriviliges.IsUserHasPriviliges(UserPriviliges.Modules.InventoryMgmt.ID, UserPriviliges.Screens.ItemFile.ID, -998))
                {
                    return oItemRow;
                }
                string strMsg = "";
                strMsg = (oItemRow.LastCostPrice == 0 && oItemRow.SellingPrice == 0) ? "Please update selling price and cost price." : (isPropmtForSellingPrice == true) ? "Please update selling price" : "Please update cost price";
                clsUIHelper.ShowOKMsg(strMsg);
                ofrmItem.Edit(sItemID);
                ofrmItem.ControlToFocusOnLoad = (isPropmtForSellingPrice == false) ? ofrmItem.numLastCostPrice.Name : ofrmItem.numSellingPrice.Name;
                ofrmItem.ShowDialog();
                if (ofrmItem.IsCanceled == false)
                {
                    oPOSTrans.GetItemRowByItemId(sItemID, ref oItemRow);
                }
            }
            catch (Exception Ex)
            {
                logger.Fatal(Ex, "UpdatePriceDetails()");
                throw Ex;
            }
            return oItemRow;
        }

        #region PRIMEPOS-3105 16-Jun-2022 JY Commented
        //public bool ROACopyTransaction(frmViewTransactionDetail ofrmVTD)
        //{
        //    bool retVal = false;
        //    if (oPOSTrans.CurrentTransactionType == POS_Core.TransType.POSTransactionType.SalesReturn)
        //    {
        //        DataSet DsROA = oPOSTrans.CheckROATransForReturn(ofrmVTD.TransID.ToString());
        //        string AcctName = "";
        //        Decimal Amount = 0;
        //        Decimal AmountToReturn = 0;
        //        string AccntNO = string.Empty;
        //        DataTable ChargeAccount;
        //        AccntNO = Configuration.convertNullToString(DsROA.Tables[0].Rows[0]["Account_No"]);
        //        AmountToReturn = Configuration.convertNullToDecimal(DsROA.Tables[0].Rows[0]["TotalPaid"]);
        //        int CustomerID = Configuration.convertNullToInt(DsROA.Tables[0].Rows[0]["CustomerID"]);  //PRIMEPOS-2570 06-Jul-2020 JY Added
        //        int PatientNo = 0;
        //        bool result = clsHouseCharge.GetReceiveOnAccount(AccntNO, out AcctName, out Amount, out ChargeAccount, AmountToReturn, CustomerID, ref PatientNo);
        //        if (result == false)
        //        {
        //            //retVal = true;
        //            return true;    //PRIMEPOS-2570 06-Jul-2020 JY Added
        //        }

        //        #region PRIMEPOS-2570 06-Jul-2020 JY Added
        //        CustomerData chargeCustomer = new CustomerData();
        //        if (PatientNo > 0)
        //        {
        //            chargeCustomer = oPOSTrans.GetChargeCustomer(PatientNo);

        //            if (chargeCustomer == null || chargeCustomer.Tables.Count == 0 || chargeCustomer.Tables[0].Rows.Count == 0)
        //            {
        //                logger.Trace("ROACopyTransaction(frmViewTransactionDetail ofrmVTD) - couldn't find POS customer against selected charge account");
        //                if (Configuration.CInfo.AutoImportCustAtTrans == 1 || Configuration.CInfo.AutoImportCustAtTrans == 2)
        //                {
        //                    logger.Trace("ROACopyTransaction(frmViewTransactionDetail ofrmVTD) - copying the linked patient from primerx to pos database");
        //                    DataSet oDS = null;
        //                    MMSChargeAccount.ContAccount oAcct = new MMSChargeAccount.ContAccount();
        //                    oAcct.GetPatientByCode(PatientNo, out oDS);
        //                    if (oDS != null && oDS.Tables.Count > 0 && oDS.Tables[0].Rows.Count > 0)
        //                    {
        //                        Customer oCustomer = new Customer();
        //                        CustomerData oCustomerData = oCustomer.CreateCustomerDSFromPatientDS(oDS, false);
        //                        if (oCustomerData != null)
        //                        {
        //                            oCustomer.Persist(oCustomerData, true);
        //                            oPOSTrans.oCustomerRow = oCustomerData.Customer[0];
        //                        }

        //                        #region PRIMEPOS-2886 24-Aug-2020 JY Commented
        //                        //frmCustomerPatientMapping ofrmCustomerPatientMapping = new frmCustomerPatientMapping(oDS);
        //                        //ofrmCustomerPatientMapping.ShowDialog();
        //                        //CustomerData oCustomerData = ofrmCustomerPatientMapping.oCustomerData;
        //                        //if (oCustomerData != null && oCustomerData.Tables.Count > 0 && oCustomerData.Tables[0].Rows.Count > 0)
        //                        //{
        //                        //    oPOSTrans.oCustomerRow = oCustomerData.Customer[0];
        //                        //}
        //                        #endregion
        //                    }
        //                }
        //                else
        //                {
        //                    Resources.Message.Display("couldn't find POS customer against selected charge account in POS database and logged in user don't have enough privileges", Configuration.ApplicationName, MessageBoxButtons.OK, MessageBoxIcon.Information);
        //                    return true;
        //                }
        //            }
        //            else
        //            {
        //                oPOSTrans.oCustomerRow = chargeCustomer.Customer[0];
        //            }
        //        }
        //        else
        //        {
        //            if (Configuration.CSetting.ProceedROATransWithHCaccNotLinked)  //PRIMEPOS-2570 17-Aug-2020 JY Added
        //            {
        //                Resources.Message.Display("Selected HC account is not linked to a patient, so we cant proceed", Configuration.ApplicationName, MessageBoxButtons.OK, MessageBoxIcon.Information);
        //                return true;
        //            }
        //            else
        //            {
        //                //proceed with the transaction
        //            }
        //        }

        //        if (oPOSTrans.oCustomerRow != null)
        //        {
        //            this.txtCustomer.Text = oPOSTrans.oCustomerRow.AccountNumber.ToString();
        //            this.lblCustomerName.Text = oPOSTrans.oCustomerRow.CustomerFullName;
        //            ShowCustomerTokenImage(oPOSTrans.oCustomerRow.CustomerId);
        //            this.txtCustomer.Tag = oPOSTrans.oCustomerRow.CustomerId;
        //            tempCustId = oPOSTrans.oCustomerRow.CustomerId;
        //        }
        //        #endregion

        //        oPOSTrans.oTransHRow.TransactionStartDate = DateTime.Now;
        //        Amount = -AmountToReturn;
        //        frmPOSPayTypesList oPTList = new frmPOSPayTypesList(Convert.ToDecimal(Amount), POS_Core.TransType.POSTransactionType.SalesReturn, 0, true, this.sSigPadTransID, 0, lblCustomerName.Text, bIsCustomerTokenExists, oPOSTrans, false, 0);  //PRIMEPOS-2611 13-Nov-2018 JY Added bIsCustomerTokenExists   // Added oPOSTrans for Solutran - NileshJ - PRIMEPOS-2663 // NileshJ - BatchDelivery - Added false,0 for IsBatchDelivery and BatchDelTotalPaidAmount - PRIMERX-7688 23-Sept-2019
        //        oPTList.ChargeAccount = ChargeAccount;
        //        try
        //        {
        //            oPTList.oPosPTList.maxClCouponAmount = oPOSTrans.CalculateMaxCouponAmount(oPOSTrans.oTransDData.TransDetail, txtAmtDiscount.Text.ToString());
        //        }
        //        catch (Exception Ex)
        //        {
        //            logger.Fatal(Ex, "ROACopyTransaction()");
        //        }

        //        oPTList.ShowDialog(this);

        //        if (oPTList.oPosPTList.CancelTransaction == true)
        //        {
        //            this.SetNew(true);
        //            retVal = true;
        //        }
        //        if (oPTList.oPosPTList.oPOSTransPaymentDataPaid == null)
        //        {
        //            this.oPOSTrans.oPOSTransPaymentData = new POSTransPaymentData();
        //            this.txtAmtBalanceDue.Text = "0";
        //            this.txtAmtChangeDue.Text = "0";
        //            this.ChangeDue = 0;
        //            this.txtAmtTendered.Value = 0;
        //            this.lblInvDiscount.Text = "0";
        //            SetNew(true);
        //        }
        //        else
        //        {
        //            this.oPOSTrans.oPOSTransPaymentData = oPTList.oPosPTList.oPOSTransPaymentDataPaid;
        //            try
        //            {
        //                SaveReceiveOnAccount(long.Parse(AccntNO.Trim()), Convert.ToDecimal(Amount), Convert.ToDecimal(oPTList.txtAmtPaid.Text), Convert.ToDecimal(oPTList.oPosPTList.ChangeDue), Convert.ToInt32(ofrmVTD.TransID.ToString()));
        //            }
        //            catch (Exception Ex1) { logger.Fatal(Ex1, "ROACopyTransaction()"); }
        //        }
        //        txtItemCode.Focus();
        //    }
        //    return retVal;
        //}
        #endregion

        #region PRIMEPOS-3105 16-Jun-2022 JY modified
        public bool ROACopyTransaction(int TransID)
        {
            bool retVal = false;
            if (oPOSTrans.CurrentTransactionType == POS_Core.TransType.POSTransactionType.SalesReturn)
            {
                DataSet DsROA = oPOSTrans.CheckROATransForReturn(TransID.ToString());
                string AcctName = "";
                Decimal Amount = 0;
                Decimal AmountToReturn = 0;
                string AccntNO = string.Empty;
                DataTable ChargeAccount;
                AccntNO = Configuration.convertNullToString(DsROA.Tables[0].Rows[0]["Account_No"]);
                AmountToReturn = Configuration.convertNullToDecimal(DsROA.Tables[0].Rows[0]["TotalPaid"]);
                int CustomerID = Configuration.convertNullToInt(DsROA.Tables[0].Rows[0]["CustomerID"]);
                int PatientNo = 0;
                bool result = clsHouseCharge.GetReceiveOnAccount(AccntNO, out AcctName, out Amount, out ChargeAccount, AmountToReturn, CustomerID, ref PatientNo);
                if (result == false)
                {
                    return true;
                }

                CustomerData chargeCustomer = new CustomerData();
                if (PatientNo > 0)
                {
                    chargeCustomer = oPOSTrans.GetChargeCustomer(PatientNo);

                    if (chargeCustomer == null || chargeCustomer.Tables.Count == 0 || chargeCustomer.Tables[0].Rows.Count == 0)
                    {
                        logger.Trace("ROACopyTransaction(string TransID) - couldn't find POS customer against selected charge account");
                        if (Configuration.CInfo.AutoImportCustAtTrans == 1 || Configuration.CInfo.AutoImportCustAtTrans == 2)
                        {
                            logger.Trace("ROACopyTransaction(string TransID) - copying the linked patient from primerx to pos database");
                            DataSet oDS = null;
                            MMSChargeAccount.ContAccount oAcct = new MMSChargeAccount.ContAccount();
                            oAcct.GetPatientByCode(PatientNo, out oDS);
                            if (oDS != null && oDS.Tables.Count > 0 && oDS.Tables[0].Rows.Count > 0)
                            {
                                Customer oCustomer = new Customer();
                                CustomerData oCustomerData = oCustomer.CreateCustomerDSFromPatientDS(oDS, false);
                                if (oCustomerData != null)
                                {
                                    oCustomer.Persist(oCustomerData, true);
                                    oPOSTrans.oCustomerRow = oCustomerData.Customer[0];
                                }
                            }
                        }
                        else
                        {
                            Resources.Message.Display("couldn't find POS customer against selected charge account in POS database and logged in user don't have enough privileges", Configuration.ApplicationName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return true;
                        }
                    }
                    else
                    {
                        oPOSTrans.oCustomerRow = chargeCustomer.Customer[0];
                    }
                }
                else
                {
                    if (Configuration.CSetting.ProceedROATransWithHCaccNotLinked)
                    {
                        Resources.Message.Display("Selected HC account is not linked to a patient, so we cant proceed", Configuration.ApplicationName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return true;
                    }
                    else
                    {
                        //proceed with the transaction
                    }
                }

                if (oPOSTrans.oCustomerRow != null)
                {
                    this.txtCustomer.Text = oPOSTrans.oCustomerRow.AccountNumber.ToString();
                    this.lblCustomerName.Text = oPOSTrans.oCustomerRow.CustomerFullName;
                    ShowCustomerTokenImage(oPOSTrans.oCustomerRow.CustomerId);
                    this.txtCustomer.Tag = oPOSTrans.oCustomerRow.CustomerId;
                    tempCustId = oPOSTrans.oCustomerRow.CustomerId;
                }

                oPOSTrans.oTransHRow.TransactionStartDate = DateTime.Now;
                Amount = -AmountToReturn;
                frmPOSPayTypesList oPTList = new frmPOSPayTypesList(Convert.ToDecimal(Amount), POS_Core.TransType.POSTransactionType.SalesReturn, 0, true, this.sSigPadTransID, 0, lblCustomerName.Text, bIsCustomerTokenExists, oPOSTrans, false, 0);  //PRIMEPOS-2611 13-Nov-2018 JY Added bIsCustomerTokenExists   // Added oPOSTrans for Solutran - NileshJ - PRIMEPOS-2663 // NileshJ - BatchDelivery - Added false,0 for IsBatchDelivery and BatchDelTotalPaidAmount - PRIMERX-7688 23-Sept-2019
                oPTList.ChargeAccount = ChargeAccount;
                #region PRIMEPOS-3105 16-Jun-2022 JY Added
                if (oPOSTrans.CurrentTransactionType == POS_Core.TransType.POSTransactionType.SalesReturn)
                {
                    oPTList.TransID = TransID;
                    oPTList.isStrictReturn = this.isStrictReturn;
                }
                #endregion
                try
                {
                    oPTList.oPosPTList.maxClCouponAmount = oPOSTrans.CalculateMaxCouponAmount(oPOSTrans.oTransDData.TransDetail, txtAmtDiscount.Text.ToString());
                }
                catch (Exception Ex)
                {
                    logger.Fatal(Ex, "ROACopyTransaction(string TransID)");
                }

                oPTList.ShowDialog(this);

                if (oPTList.oPosPTList.CancelTransaction == true)
                {
                    this.SetNew(true);
                    retVal = true;
                }
                if (oPTList.oPosPTList.oPOSTransPaymentDataPaid == null)
                {
                    this.oPOSTrans.oPOSTransPaymentData = new POSTransPaymentData();
                    this.txtAmtBalanceDue.Text = "0";
                    this.txtAmtChangeDue.Text = "0";
                    this.ChangeDue = 0;
                    this.txtAmtTendered.Value = 0;
                    this.lblInvDiscount.Text = "0";
                    SetNew(true);
                }
                else
                {
                    this.oPOSTrans.oPOSTransPaymentData = oPTList.oPosPTList.oPOSTransPaymentDataPaid;
                    try
                    {
                        #region PRIMEPOS-3117 11-Jul-2022 JY Added
                        TransFeeAmt = oPTList.oPosPTList.TransFeeAmt;
                        if (oPOSTrans.CurrentTransactionType == POS_Core.TransType.POSTransactionType.SalesReturn)
                            Amount += TransFeeAmt;
                        #endregion

                        SaveReceiveOnAccount(long.Parse(AccntNO.Trim()), Convert.ToDecimal(Amount), Convert.ToDecimal(oPTList.txtAmtPaid.Text), Convert.ToDecimal(oPTList.oPosPTList.ChangeDue), TransID);
                    }
                    catch (Exception Ex1) { logger.Fatal(Ex1, "ROACopyTransaction(string TransID)"); }
                }
                txtItemCode.Focus();
            }
            return retVal;
        }
        #endregion

        private void CopyViewTransaction(frmViewTransactionDetail ofrmVTD)
        {
            logger.Trace("CopyViewTransaction() - " + clsPOSDBConstants.Log_Entering);

            bIsCopied = true;

            #region ROA Transaction Return
            if (ofrmVTD.isROATrans == true) //PRIMEPOS-2751 04-Nov-2019 JY Added if condition
            {
                bool retVal = ROACopyTransaction(ofrmVTD.TransID);  //PRIMEPOS-3105 16-Jun-2022 JY modified to optimize code
                if (retVal)
                {
                    return;
                }
            }
            #endregion ROA Transaction Return

            if (oPOSTrans.CurrentTransactionType == POS_Core.TransType.POSTransactionType.SalesReturn)
            {
                this.isCallofRetTrans = true;
            }
            DataSet oData = oPOSTrans.PopulateTransactionByTransactionId(ofrmVTD.TransID.ToString(), isCallofRetTrans);
            TransHeaderData oHData = oPOSTrans.GetTransactionHeaderByTransactionId(ofrmVTD.TransID.ToString());
            TransID = Convert.ToInt32(ofrmVTD.TransID.ToString());//Added By Shitaljit(QuicSolv) on 5 Sept 2011
            if (oData != null)
            {
                if (oPOSTrans.CurrentTransactionType == POS_Core.TransType.POSTransactionType.SalesReturn)
                {
                    if (oData.Tables[0].Rows.Count == 0)
                    {
                        #region PRIMEPOS-2738
                        if (Configuration.CSetting.StrictReturn == true)
                        {
                            this.SetNew(false);
                            this.setTransactionType(POS_Core.TransType.POSTransactionType.Sales);
                        }
                        #endregion
                        //throw (new Exception("This transaction is already returned."));   //PRIMEPOS-2586 11-Sep-2018 JY Commented
                        #region PRIMEPOS-2586 11-Sep-2018 JY Added
                        var ex = new Exception(string.Format("{0} - {1}", Configuration.sTransAlreadyReturned, Configuration.iTransAlreadyReturned));
                        ex.Data.Add(Configuration.iTransAlreadyReturned, Configuration.sTransAlreadyReturned);
                        throw ex;
                        #endregion
                    }
                    oPOSTrans.oTransHRow.ReturnTransID = Convert.ToInt32(ofrmVTD.TransID.ToString());

                    #region Pupulating cust for Return Transactions added by shitaljit.
                    PopulateCustForRetTrans(oHData.Tables[0].Rows[0][clsPOSDBConstants.Customer_Fld_CustomerId].ToString());
                    #endregion Pupulating cust for Return Transactions added by shitaljit.
                }
                int row = 1;
                TransDetailTaxData TmpTaxData = new TransDetailTaxData();
                if (oData.Tables[0].Rows.Count > 0)
                    TransStartDateTime = DateTime.Now.ToString();//To Capture TranXn Start time;

                ArrayList arrPatients = new ArrayList();    //PRIMEPOS-2536 14-May-2019 JY Added
                foreach (DataRow dr in oData.Tables[0].Rows)
                {
                    if (dr[clsPOSDBConstants.TransDetail_Fld_ItemID].ToString() == "RX")
                    {
                        string RXItemID = oPOSTrans.ExtractRXInfoFromDescription(dr[clsPOSDBConstants.TransDetail_Fld_ItemDescription].ToString());
                        //if (RXItemID != "")   //PRIMEPOS-2863 18-Jun-2020 JY Commented
                        if (RXItemID != "" && RXItemID.Trim() != "0")   //PRIMEPOS-2863 18-Jun-2020 JY Added
                        {
                            RXHeader oRXHeader = FillRXInformation(RXItemID.ToString());
                            if (oRXHeader == null)
                                continue;    //PRIMEPOS-2699 05-Aug-2019 JY Added

                            #region PRIMEPOS-2536 14-May-2019 JY Added logic to show RX and Patient notes from PrimeRX.
                            int iIndex = RXItemID.IndexOf("-");
                            string ItemCode = RXItemID;
                            if (iIndex > 0)
                                ItemCode = ItemCode.Substring(0, iIndex);
                            ShowPrimeRXPOSNotes(ItemCode, ref arrPatients);
                            #endregion Show RX and Patient notes from PrimeRX.
                        }
                        else
                        {
                            //oPOSTrans.oTDRow = this.oPOSTrans.oTransDData.TransDetail.AddRow(clsUIHelper.GetNextNumber(oPOSTrans.oTransDData, clsPOSDBConstants.TransDetail_Fld_TransDetailID), 0, 0, 0, 0, 0, 0, 0, "", "");
                            //oPOSTrans.SetRowTrans(oPOSTrans.oTDRow);
                        }
                    }
                    else if (dr[clsPOSDBConstants.TransDetail_Fld_ItemID].ToString() == Configuration.CouponItemCode)   //PRIMEPOS-2034 08-Mar-2018 JY Added to skip coupon if its expired		
                    {
                        Int64 CouponID = Configuration.convertNullToInt64(dr[clsPOSDBConstants.TransDetail_Fld_CouponID]);
                        if (CouponID == 0 && oPOSTrans.CurrentTransactionType == POS_Core.TransType.POSTransactionType.Sales)
                        {
                            //it is old coupon and it is not linked with the transaction so we can't check its expiry, so not adding it into sale transaction		
                            continue;
                        }
                        else if (oPOSTrans.IsCouponExpired(CouponID))
                        {
                            if (oPOSTrans.CurrentTransactionType == POS_Core.TransType.POSTransactionType.Sales)
                            {
                                Resources.Message.Display(dr[clsPOSDBConstants.TransDetail_Fld_ItemDescription].ToString() + Environment.NewLine + "This coupon is expired so we cant consume it in sales.", "PRIMEPOS", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                                continue;
                            }
                            else
                            {
                                Resources.Message.Display(dr[clsPOSDBConstants.TransDetail_Fld_ItemDescription].ToString() + Environment.NewLine + "This coupon is expired.", "PRIMEPOS", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                            }
                        }
                    }

                    oPOSTrans.GetTransDetailTaxForCopyViewTrans(dr, ofrmVTD.TransID, ref TmpTaxData, ref row);

                    #region PRIMEPOS-2536 13-May-2019 JY Added
                    if (Configuration.CPOSSet.ShowItemNotes == true)
                    {
                        logger.Trace("CopyViewTransaction(frmViewTransactionDetail ofrmVTD) - About to ShowItemNotes" + Configuration.convertNullToString(dr[clsPOSDBConstants.TransDetail_Fld_ItemID]));
                        ShowItemNotes(Configuration.convertNullToString(dr[clsPOSDBConstants.TransDetail_Fld_ItemID]));
                    }
                    #endregion

                    if (Configuration.convertNullToInt64(dr[clsPOSDBConstants.TransDetail_Fld_CouponID].ToString()) > 0)    //PRIMEPOS-2034 08-Mar-2018 JY Added		
                        this.lblCouponDiscount.Text = dr[clsPOSDBConstants.TransDetail_Fld_ExtendedPrice].ToString();
                }
                oPOSTrans.oTDTaxData = TmpTaxData;

                this.grdDetail.Update();
                System.Windows.Forms.Application.DoEvents();
                this.ultraCalcManager1.ReCalc();

                if (oHData.TransHeader[0].TotalDiscAmount != Configuration.convertNullToDecimal(this.txtAmtDiscount.Text))
                {
                    if (oPOSTrans.CurrentTransactionType != (POS_Core.TransType.POSTransactionType)oHData.TransHeader[0].TransType)
                    {
                        if (oPOSTrans.CurrentTransactionType == POS_Core.TransType.POSTransactionType.Sales)
                        {
                            this.lblInvDiscount.Text = Convert.ToString(Math.Abs(Configuration.convertNullToDecimal(oHData.TransHeader[0].TotalDiscAmount.ToString())) - Math.Abs(Configuration.convertNullToDecimal(this.txtAmtDiscount.Text)));
                        }
                        else if (oPOSTrans.CurrentTransactionType == POS_Core.TransType.POSTransactionType.SalesReturn)
                        {
                            //Following Code(if-els) is Added By  Shitaljit(QuicSolv) on 29 August 2011

                            if (oHData.TransHeader[0].TotalDiscAmount != oHData.TransHeader[0].InvoiceDiscount && oHData.TransHeader[0].InvoiceDiscount == 0)
                                this.lblInvDiscount.Text = Convert.ToString(-1 * (Math.Abs(Configuration.convertNullToDecimal(oHData.TransHeader[0].TotalDiscAmount.ToString())) - Math.Abs(Configuration.convertNullToDecimal(this.txtAmtDiscount.Text))));//Commented by Shitaljit 0n 13 Sept 2011
                            else
                                this.lblInvDiscount.Text = Convert.ToString(-1 * Math.Abs(oHData.TransHeader[0].InvoiceDiscount));//Added by Shitaljit 0n 13 Sept 2011
                        }
                    }
                    else
                    {
                        this.lblInvDiscount.Text = Convert.ToString(Configuration.convertNullToDecimal(oHData.TransHeader[0].TotalDiscAmount.ToString()) - Configuration.convertNullToDecimal(this.txtAmtDiscount.Text));
                    }
                    this.ultraCalcManager1.ReCalc();
                    InitPayment();
                }
            }
            bIsCopied = false;  //Sprint-26 - PRIMEPOS-2418 02-Aug-2017 JY Added
            logger.Trace("CopyViewTransaction() - " + clsPOSDBConstants.Log_Exiting);
        }

        private void CopyTransaction(frmViewTransactionDetail ofrmVTD)
        {
            bIsCopied = true;

            #region ROA Transaction Return
            if (ofrmVTD.isROATrans == true) //PRIMEPOS-2751 04-Nov-2019 JY Added if condition
            {
                bool retVal = ROACopyTransaction(ofrmVTD.TransID);  //PRIMEPOS-3105 16-Jun-2022 JY modified to optimize code
                if (retVal)
                {
                    return;
                }
            }
            #endregion ROA Transaction Return

            if (oPOSTrans.CurrentTransactionType == POS_Core.TransType.POSTransactionType.SalesReturn)
            {
                POS_Core.DataAccess.TransDetailSvr.isCallofRetTrans = true;
                this.isCallofRetTrans = true;
            }
            DataSet oData = oPOSTrans.PopulateTransactionDetData(Convert.ToInt32(ofrmVTD.TransID.ToString()));

            POS_Core.CommonData.TransHeaderData oHData = oPOSTrans.GetTransactionHeaderByTransactionId(ofrmVTD.TransID.ToString());
            TransID = Convert.ToInt32(ofrmVTD.TransID.ToString());//Added By Shitaljit(QuicSolv) on 5 Sept 2011
            if (oData != null)
            {
                if (oPOSTrans.CurrentTransactionType == POS_Core.TransType.POSTransactionType.SalesReturn)
                {
                    if (oData.Tables[0].Rows.Count == 0)
                    {
                        #region PRIMEPOS-2738
                        //if (Configuration.CSetting.StrictReturn == true)
                        if (isStrictReturn == true)
                        {
                            this.SetNew(false);
                            this.setTransactionType(POS_Core.TransType.POSTransactionType.Sales);
                        }
                        #endregion
                        //throw (new Exception("This transaction is already returned."));   //PRIMEPOS-2586 11-Sep-2018 JY Commented
                        #region PRIMEPOS-2586 11-Sep-2018 JY Added
                        var ex = new Exception(string.Format("{0} - {1}", Configuration.sTransAlreadyReturned, Configuration.iTransAlreadyReturned));
                        ex.Data.Add(Configuration.iTransAlreadyReturned, Configuration.sTransAlreadyReturned);
                        throw ex;
                        #endregion
                    }
                    oPOSTrans.oTransHRow.ReturnTransID = Convert.ToInt32(ofrmVTD.TransID.ToString());

                    #region Pupulating cust for Return Transactions added by shitaljit.
                    PopulateCustForRetTrans(oHData.Tables[0].Rows[0][clsPOSDBConstants.Customer_Fld_CustomerId].ToString());
                    #endregion Pupulating cust for Return Transactions added by shitaljit.
                }
                int row = 1;
                TransDetailTaxData TmpTaxData = new TransDetailTaxData();
                ArrayList arrPatients = new ArrayList();    //PRIMEPOS-2536 14-May-2019 JY Added
                foreach (DataRow dr in oData.Tables[0].Rows)
                {
                    if (oData.Tables[0].Rows.Count > 0)
                        TransStartDateTime = DateTime.Now.ToString();//To Capture TranXn Start time;

                    if (dr[clsPOSDBConstants.TransDetail_Fld_ItemID].ToString() == "RX")
                    {
                        string RXItemID = oPOSTrans.ExtractRXInfoFromDescription(dr[clsPOSDBConstants.TransDetail_Fld_ItemDescription].ToString());
                        //if (RXItemID != "")   //PRIMEPOS-2863 18-Jun-2020 JY Commented
                        if (RXItemID != "" && RXItemID.Trim() != "0")   //PRIMEPOS-2863 18-Jun-2020 JY Added
                        {
                            FillRXInformation(RXItemID.ToString());

                            #region PRIMEPOS-2536 14-May-2019 JY Added logic to show RX and Patient notes from PrimeRX.
                            int iIndex = RXItemID.IndexOf("-");
                            string ItemCode = RXItemID;
                            if (iIndex > 0)
                                ItemCode = ItemCode.Substring(0, iIndex);
                            ShowPrimeRXPOSNotes(ItemCode, ref arrPatients);
                            #endregion Show RX and Patient notes from PrimeRX.
                        }
                        //else
                        //{
                        //    oPOSTrans.oTDRow = this.oPOSTrans.oTransDData.TransDetail.AddRow(clsUIHelper.GetNextNumber(oPOSTrans.oTransDData, clsPOSDBConstants.TransDetail_Fld_TransDetailID), 0, 0, 0, 0, 0, 0, 0, "", "");
                        //    oPOSTrans.SetRowTrans(oPOSTrans.oTDRow);
                        //}
                    }
                    else if (dr[clsPOSDBConstants.TransDetail_Fld_ItemID].ToString() == Configuration.CouponItemCode)   //PRIMEPOS-2034 08-Mar-2018 JY Added to skip coupon if its expired		
                    {
                        Int64 CouponID = Configuration.convertNullToInt64(dr[clsPOSDBConstants.TransDetail_Fld_CouponID]);
                        if (CouponID == 0 && oPOSTrans.CurrentTransactionType == POS_Core.TransType.POSTransactionType.Sales)
                        {
                            //it is old coupon and it is not linked with the transaction so we can't check its expiry, so not adding it into sale transaction		
                            continue;
                        }
                        else if (oPOSTrans.IsCouponExpired(CouponID))
                        {
                            if (oPOSTrans.CurrentTransactionType == POS_Core.TransType.POSTransactionType.Sales)
                            {
                                Resources.Message.Display(dr[clsPOSDBConstants.TransDetail_Fld_ItemDescription].ToString() + Environment.NewLine + ": This coupon is expired so we cant consume it in sales.", "PRIMEPOS", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                                continue;
                            }
                            else
                            {
                                Resources.Message.Display(dr[clsPOSDBConstants.TransDetail_Fld_ItemDescription].ToString() + Environment.NewLine + ": This coupon is expired.", "PRIMEPOS", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                            }
                        }
                    }
                    oPOSTrans.GetTransDetailTaxForCopyTrans(dr, ofrmVTD.TransID, ref TmpTaxData, ref row);

                    #region PRIMEPOS-2536 13-May-2019 JY Added
                    if (Configuration.CPOSSet.ShowItemNotes == true)
                    {
                        logger.Trace("CopyViewTransaction(frmViewTransactionDetail ofrmVTD) - About to ShowItemNotes" + Configuration.convertNullToString(dr[clsPOSDBConstants.TransDetail_Fld_ItemID]));
                        ShowItemNotes(Configuration.convertNullToString(dr[clsPOSDBConstants.TransDetail_Fld_ItemID]));
                    }
                    #endregion
                }
                oPOSTrans.oTDTaxData = TmpTaxData;
                //this.grdDetail.DataSource = this.oTransDData;
                //MessageBox.Show(this.grdDetail.Rows[0].Cells["Discount"].Text);

                this.grdDetail.Update();
                System.Windows.Forms.Application.DoEvents();
                this.ultraCalcManager1.ReCalc();

                if (oHData.TransHeader[0].TotalDiscAmount != Configuration.convertNullToDecimal(this.txtAmtDiscount.Text))
                {
                    if (oPOSTrans.CurrentTransactionType != (POS_Core.TransType.POSTransactionType)oHData.TransHeader[0].TransType)
                    {
                        if (oPOSTrans.CurrentTransactionType == POS_Core.TransType.POSTransactionType.Sales)
                        {
                            this.lblInvDiscount.Text = Convert.ToString(Math.Abs(Configuration.convertNullToDecimal(oHData.TransHeader[0].TotalDiscAmount.ToString())) - Math.Abs(Configuration.convertNullToDecimal(this.txtAmtDiscount.Text)));
                        }
                        else if (oPOSTrans.CurrentTransactionType == POS_Core.TransType.POSTransactionType.SalesReturn)
                        {
                            //Following Code(if-els) is Added By  Shitaljit(QuicSolv) on 29 August 2011

                            if (oHData.TransHeader[0].TotalDiscAmount != oHData.TransHeader[0].InvoiceDiscount && oHData.TransHeader[0].InvoiceDiscount == 0)
                                this.lblInvDiscount.Text = Convert.ToString(-1 * (Math.Abs(Configuration.convertNullToDecimal(oHData.TransHeader[0].TotalDiscAmount.ToString())) - Math.Abs(Configuration.convertNullToDecimal(this.txtAmtDiscount.Text))));//Commented by Shitaljit 0n 13 Sept 2011
                            else
                                this.lblInvDiscount.Text = Convert.ToString(-1 * Math.Abs(oHData.TransHeader[0].InvoiceDiscount));//Added by Shitaljit 0n 13 Sept 2011
                        }
                    }
                    else
                    {
                        this.lblInvDiscount.Text = Convert.ToString(Configuration.convertNullToDecimal(oHData.TransHeader[0].TotalDiscAmount.ToString()) - Configuration.convertNullToDecimal(this.txtAmtDiscount.Text));
                    }
                    // RecalculateTax();
                    this.ultraCalcManager1.ReCalc();
                    InitPayment();
                }

            }
            bIsCopied = false;  //Sprint-26 - PRIMEPOS-2418 02-Aug-2017 JY Added
        }
        #region PRIMEPOS-2738
        private void CopySelectReturnTransaction(frmSelectTransaction ofrmST)
        {
            bIsCopied = true;

            #region PRIMEPOS-3105 16-Jun-2022 JY Added for ROA Transaction Return
            if (ofrmST.isROATrans == true)
            {
                bool retVal = ROACopyTransaction(Configuration.convertNullToInt(ofrmST.TransID));
                if (retVal) { return; }
            }
            #endregion

            if (oPOSTrans.CurrentTransactionType == POS_Core.TransType.POSTransactionType.SalesReturn)
            {
                POS_Core.DataAccess.TransDetailSvr.isCallofRetTrans = true;
                this.isCallofRetTrans = true;
            }
            DataSet oData = oPOSTrans.PopulateTransactionDetData(Convert.ToInt32(ofrmST.TransID.ToString()));

            POS_Core.CommonData.TransHeaderData oHData = oPOSTrans.GetTransactionHeaderByTransactionId(ofrmST.TransID.ToString());
            TransID = Convert.ToInt32(ofrmST.TransID.ToString());//Added By Shitaljit(QuicSolv) on 5 Sept 2011
            if (oData != null)
            {
                if (oPOSTrans.CurrentTransactionType == POS_Core.TransType.POSTransactionType.SalesReturn)
                {
                    if (oData.Tables[0].Rows.Count == 0)
                    {
                        #region PRIMEPOS-2738
                        //if (Configuration.CSetting.StrictReturn == true)
                        if (isStrictReturn == true)
                        {
                            this.SetNew(false);
                            this.setTransactionType(POS_Core.TransType.POSTransactionType.Sales);
                        }
                        #endregion
                        //throw (new Exception("This transaction is already returned."));   //PRIMEPOS-2586 11-Sep-2018 JY Commented
                        #region PRIMEPOS-2586 11-Sep-2018 JY Added
                        var ex = new Exception(string.Format("{0} - {1}", Configuration.sTransAlreadyReturned, Configuration.iTransAlreadyReturned));
                        ex.Data.Add(Configuration.iTransAlreadyReturned, Configuration.sTransAlreadyReturned);
                        throw ex;
                        #endregion
                    }
                    oPOSTrans.oTransHRow.ReturnTransID = Convert.ToInt32(ofrmST.TransID.ToString());

                    #region Pupulating cust for Return Transactions added by shitaljit.
                    PopulateCustForRetTrans(oHData.Tables[0].Rows[0][clsPOSDBConstants.Customer_Fld_CustomerId].ToString());
                    #endregion Pupulating cust for Return Transactions added by shitaljit.
                }
                int row = 1;
                TransDetailTaxData TmpTaxData = new TransDetailTaxData();
                ArrayList arrPatients = new ArrayList();    //PRIMEPOS-2536 14-May-2019 JY Added
                foreach (DataRow dr in oData.Tables[0].Rows)
                {
                    if (oData.Tables[0].Rows.Count > 0)
                        TransStartDateTime = DateTime.Now.ToString();//To Capture TranXn Start time;

                    if (dr[clsPOSDBConstants.TransDetail_Fld_ItemID].ToString() == "RX")
                    {
                        string RXItemID = oPOSTrans.ExtractRXInfoFromDescription(dr[clsPOSDBConstants.TransDetail_Fld_ItemDescription].ToString());
                        //if (RXItemID != "")   //PRIMEPOS-2863 18-Jun-2020 JY Commented
                        if (RXItemID != "" && RXItemID.Trim() != "0")   //PRIMEPOS-2863 18-Jun-2020 JY Added
                        {
                            FillRXInformation(RXItemID.ToString());

                            #region PRIMEPOS-2536 14-May-2019 JY Added logic to show RX and Patient notes from PrimeRX.
                            int iIndex = RXItemID.IndexOf("-");
                            string ItemCode = RXItemID;
                            if (iIndex > 0)
                                ItemCode = ItemCode.Substring(0, iIndex);
                            ShowPrimeRXPOSNotes(ItemCode, ref arrPatients);
                            #endregion Show RX and Patient notes from PrimeRX.
                        }
                        //else
                        //{
                        //    oPOSTrans.oTDRow = this.oPOSTrans.oTransDData.TransDetail.AddRow(clsUIHelper.GetNextNumber(oPOSTrans.oTransDData, clsPOSDBConstants.TransDetail_Fld_TransDetailID), 0, 0, 0, 0, 0, 0, 0, "", "");
                        //    oPOSTrans.SetRowTrans(oPOSTrans.oTDRow);
                        //}
                    }
                    else if (dr[clsPOSDBConstants.TransDetail_Fld_ItemID].ToString() == Configuration.CouponItemCode)   //PRIMEPOS-2034 08-Mar-2018 JY Added to skip coupon if its expired		
                    {
                        Int64 CouponID = Configuration.convertNullToInt64(dr[clsPOSDBConstants.TransDetail_Fld_CouponID]);
                        if (CouponID == 0 && oPOSTrans.CurrentTransactionType == POS_Core.TransType.POSTransactionType.Sales)
                        {
                            //it is old coupon and it is not linked with the transaction so we can't check its expiry, so not adding it into sale transaction		
                            continue;
                        }
                        else if (oPOSTrans.IsCouponExpired(CouponID))
                        {
                            if (oPOSTrans.CurrentTransactionType == POS_Core.TransType.POSTransactionType.Sales)
                            {
                                Resources.Message.Display(dr[clsPOSDBConstants.TransDetail_Fld_ItemDescription].ToString() + Environment.NewLine + ": This coupon is expired so we cant consume it in sales.", "PRIMEPOS", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                                continue;
                            }
                            else
                            {
                                Resources.Message.Display(dr[clsPOSDBConstants.TransDetail_Fld_ItemDescription].ToString() + Environment.NewLine + ": This coupon is expired.", "PRIMEPOS", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                            }
                        }
                    }
                    oPOSTrans.GetTransDetailTaxForCopyTrans(dr, Convert.ToInt32(ofrmST.TransID), ref TmpTaxData, ref row);

                    #region PRIMEPOS-2536 13-May-2019 JY Added
                    if (Configuration.CPOSSet.ShowItemNotes == true)
                    {
                        logger.Trace("CopyViewTransaction(frmViewTransactionDetail ofrmVTD) - About to ShowItemNotes" + Configuration.convertNullToString(dr[clsPOSDBConstants.TransDetail_Fld_ItemID]));
                        ShowItemNotes(Configuration.convertNullToString(dr[clsPOSDBConstants.TransDetail_Fld_ItemID]));
                    }
                    #endregion
                }
                oPOSTrans.oTDTaxData = TmpTaxData;
                //this.grdDetail.DataSource = this.oTransDData;
                //MessageBox.Show(this.grdDetail.Rows[0].Cells["Discount"].Text);

                this.grdDetail.Update();
                System.Windows.Forms.Application.DoEvents();
                this.ultraCalcManager1.ReCalc();

                if (oHData.TransHeader[0].TotalDiscAmount != Configuration.convertNullToDecimal(this.txtAmtDiscount.Text))
                {
                    if (oPOSTrans.CurrentTransactionType != (POS_Core.TransType.POSTransactionType)oHData.TransHeader[0].TransType)
                    {
                        if (oPOSTrans.CurrentTransactionType == POS_Core.TransType.POSTransactionType.Sales)
                        {
                            this.lblInvDiscount.Text = Convert.ToString(Math.Abs(Configuration.convertNullToDecimal(oHData.TransHeader[0].TotalDiscAmount.ToString())) - Math.Abs(Configuration.convertNullToDecimal(this.txtAmtDiscount.Text)));
                        }
                        else if (oPOSTrans.CurrentTransactionType == POS_Core.TransType.POSTransactionType.SalesReturn)
                        {
                            //Following Code(if-els) is Added By  Shitaljit(QuicSolv) on 29 August 2011

                            if (oHData.TransHeader[0].TotalDiscAmount != oHData.TransHeader[0].InvoiceDiscount && oHData.TransHeader[0].InvoiceDiscount == 0)
                                this.lblInvDiscount.Text = Convert.ToString(-1 * (Math.Abs(Configuration.convertNullToDecimal(oHData.TransHeader[0].TotalDiscAmount.ToString())) - Math.Abs(Configuration.convertNullToDecimal(this.txtAmtDiscount.Text))));//Commented by Shitaljit 0n 13 Sept 2011
                            else
                                this.lblInvDiscount.Text = Convert.ToString(-1 * Math.Abs(oHData.TransHeader[0].InvoiceDiscount));//Added by Shitaljit 0n 13 Sept 2011
                        }
                    }
                    else
                    {
                        this.lblInvDiscount.Text = Convert.ToString(Configuration.convertNullToDecimal(oHData.TransHeader[0].TotalDiscAmount.ToString()) - Configuration.convertNullToDecimal(this.txtAmtDiscount.Text));
                    }
                    // RecalculateTax();
                    this.ultraCalcManager1.ReCalc();
                    InitPayment();
                }

            }
            bIsCopied = false;  //Sprint-26 - PRIMEPOS-2418 02-Aug-2017 JY Added
        }
        #endregion
        #region PRIMEPOS-2034 12-Mar-2018 JY Added		
        private Boolean IsCouponAddedIntoTransaction()
        {
            Boolean bStatus = false;
            try
            {
                foreach (UltraGridRow oGRow in this.grdDetail.Rows)
                {
                    if (Configuration.convertNullToInt64(oGRow.Cells[clsPOSDBConstants.TransDetail_Fld_CouponID].Value) != 0)
                    {
                        bStatus = true;
                        break;
                    }
                }
                return bStatus;
            }
            catch (Exception Ex)
            {
                logger.Fatal(Ex, "IsCouponAddedIntoTransaction()");
                return false;
            }
        }
        #endregion

        //completed by sandeep
        private void CheckCompanionItems()
        {
            CheckCompanionItems(this.txtItemCode.Text, Configuration.convertNullToInt(this.txtQty.Value.ToString()));
        }

        //completed by sandeep
        private void CheckCompanionItems(string sItemCode, int qty)
        {
            try
            {
                logger.Trace("CheckCompanionItems() - " + clsPOSDBConstants.Log_Entering);
                ItemCompanion oIComp = new ItemCompanion();
                ItemCompanionData oICompData;

                oICompData = oIComp.PopulateList(String.Concat(" Companion.", clsPOSDBConstants.ItemCompanion_Fld_ItemID, "='", sItemCode, "' "));
                foreach (ItemCompanionRow OICompRow in oICompData.ItemCompanion.Rows)
                {
                    TransDetailRow oExistingRow;
                    TransDetailRow oTDCompRow = oPOSTrans.AddCompanionItem(OICompRow, qty, txtUnitPrice.Value.ToString(), out oExistingRow);
                    if (oTDCompRow != null)
                    {
                        this.grdDetail.Refresh();
                        if (Configuration.CPOSSet.UseSigPad == true)
                        {
                            if (oExistingRow == null)
                            {
                                logger.Trace("CheckCompanionItems() - POSTransaction Add Companion Item-Update to Sig Pad - CurrentTransID " + oPOSTrans.oTransHRow.TransID + " ItemId: " + oTDCompRow.ItemID + " ItemDescription: " + oTDCompRow.ItemDescription + " TransDetailID: " + oTDCompRow.TransDetailID);
                                if (SigPadUtil.DefaultInstance.IsVF)
                                {
                                    DeviceItemsProcess("AddCompanionItem", oTDCompRow, grdDetail.Rows.Count - 1);
                                }
                                else
                                {
                                    SigPadUtil.DefaultInstance.AddItem(oTDCompRow, grdDetail.Rows.Count - 1);//Changed by SRT for optimization
                                }
                            }
                            else
                            {
                                //Added by Manoj 10/13/2011 - this is to get know if the item  was onhold or not
                                if (oPOSTrans.oTDRow.TransID > 0 && onHoldTransID > 0)
                                {
                                    SigPadUtil.DefaultInstance.iSItemOnHold(true);
                                }
                                else
                                {
                                    SigPadUtil.DefaultInstance.iSItemOnHold(false);
                                }
                                logger.Trace("CheckCompanionItems() - POSTransaction Add Companion Item-Update to Sig Pad - ItemId: " + oTDCompRow.ItemID + " ItemDescription: " + oTDCompRow.ItemDescription + " TransDetailID: " + oTDCompRow.TransDetailID);
                                int index = oPOSTrans.GetTransIndex(oPOSTrans.oTransDData, oExistingRow.TransDetailID);
                                if (SigPadUtil.DefaultInstance.IsVF)
                                    DeviceItemsProcess("UpdateItem", oTDCompRow, index);
                                else
                                    SigPadUtil.DefaultInstance.UpdateItem(oTDCompRow, index);
                            }
                        }

                        try
                        {
                            string strItem = "";
                            if (Configuration.CPOSSet.PrintRXDescription == false && oTDCompRow.ItemID == "RX")
                            {
                                strItem = oTDCompRow.ItemDescription.Substring(0, oPOSTrans.oTDRow.ItemDescription.IndexOf("-"));
                            }
                            else
                            {
                                strItem = oTDCompRow.ItemDescription;
                            }

                            if (Configuration.CPOSSet.UsePoleDisplay)
                            {
                                frmMain.PoleDisplay.ClearPoleDisplay();

                                if (Configuration.CPOSSet.PD_LINES > 1)
                                {
                                    int LineLen = Configuration.CPOSSet.PD_LINELEN;
                                    if (LineLen > strItem.Length)
                                        frmMain.PoleDisplay.WriteToPoleDisplay(strItem
                                            + clsUIHelper.Spaces(Configuration.CPOSSet.PD_LINELEN - strItem.Length - 7) + " " + oTDCompRow.ExtendedPrice.ToString("###.00"));
                                    else
                                        frmMain.PoleDisplay.WriteToPoleDisplay(strItem.Substring(0, LineLen - 7)
                                            + " " + oTDCompRow.ExtendedPrice.ToString(Configuration.CInfo.CurrencySymbol + "##0.00"));
                                }
                            }
                        }
                        catch (Exception) { }
                    }
                }
            }
            catch (Exception exp)
            {
                logger.Fatal(exp, "CheckCompanionItems()");
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
            logger.Trace("CheckCompanionItems() - " + clsPOSDBConstants.Log_Exiting);
        }

        //completed by sandeep
        private void ShowItemWarningMessage(ItemRow oItemRow)
        {
            logger.Trace("ShowItemWarningMessage() - " + clsPOSDBConstants.Log_Entering);
            string strMessage = oPOSTrans.GetWarningMessage(oItemRow.ItemID);
            if (strMessage.Length > 0)
            {
                clsUIHelper.ShowWarningMsg(strMessage, "Warning");
            }
            logger.Trace("ShowItemWarningMessage() - " + clsPOSDBConstants.Log_Exiting);
        }
        //completed by sandeep
        private void ShowItemNotes(string ItemID)
        {
            logger.Trace("ShowItemNotes() - " + clsPOSDBConstants.Log_Entering);
            NotesData oNotesData = new NotesData();
            string whereClause = " WHERE " + clsPOSDBConstants.Notes_Fld_EntityId + "= '" + ItemID + "'  AND  " + clsPOSDBConstants.Notes_Fld_EntityType + "= '" + clsEntityType.ItemNote + "' AND " + clsPOSDBConstants.Notes_Fld_POPUPMSG + "= '" + true + "'";
            oNotesData = oPOSTrans.PopulateItemNotes(whereClause);
            if (oNotesData.Notes.Rows.Count > 0)
            {
                frmCustomerNotesView ofrmCustomerNotesView = new frmCustomerNotesView(ItemID, clsEntityType.ItemNote);
                ofrmCustomerNotesView.ShowDialog();
            }
            logger.Trace("ShowItemNotes() - " + clsPOSDBConstants.Log_Exiting);
        }

        //completed by sandeep
        private RXHeader FetchUnPickRx(string ItemCode)
        {
            logger.Trace("FetchUnPickRx() - " + clsPOSDBConstants.Log_Entering);
            string nRefill = "";
            DataTable oRxInfo = new DataTable();
            oPOSTrans.isUnBilledRx = 0; //PRIMEPOS-2398 04-Jan-2021 JY modified
            RXHeader oRXHeader;
            try
            {
                int iIndex = ItemCode.IndexOf("-");
                if (iIndex > 0)
                {
                    nRefill = ItemCode.Substring(iIndex + 1, ItemCode.Length - iIndex - 1);
                    ItemCode = ItemCode.Substring(0, iIndex);
                }
                logger.Trace("FetchUnPickRx() - About to call PharmSQL");
                oRxInfo = oPOSTrans.GetRxWithStatus(ItemCode, nRefill);
                if (isOnHoldTrans == true)
                {
                    if (oRxInfo.Rows.Count > 0)
                    {
                        if (oRxInfo.Rows[0]["Status"].ToString() == "F")
                        {
                            this.txtItemCode.Focus();
                            ErrorHandler.throwCustomError(POSErrorENUM.TransDetail_RXNotFound);
                        }
                    }
                }
                logger.Trace("FetchUnPickRx() - Call PharmSQL Successful");
                oRXHeader = oPOSTrans.InsertRXHeader(oRxInfo);
                frmPatientRXSearch.RXNo = Convert.ToInt64(oRxInfo.Rows[0]["RXNo"].ToString());
                frmPatientRXSearch.RefillNo = Convert.ToInt16(oRxInfo.Rows[0]["nrefill"].ToString());
                if (oRxInfo.Columns.Contains("PartialFillNo"))
                    frmPatientRXSearch.PartialFillNo = Convert.ToInt16(oRxInfo.Rows[0]["PartialFillNo"].ToString());
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "FetchUnPickRx()");
                throw new Exception(ex.ToString());
            }
            logger.Trace("FetchUnPickRx() - " + clsPOSDBConstants.Log_Exiting);
            return oRXHeader;
        }
        //completed by sandeep
        private void FillUnPickedRXs(RXHeader oRXHeader, bool validateWithMinDate)
        {
            PharmBL oPharmBL = new PharmBL();
            isSearchUnPickCancel = false;// Added by rohit Nair on 11/28/2017 so as to prevent rx's skipping hippa and rx sign 
            if (oPOSTrans.CurrentTransactionType == POS_Core.TransType.POSTransactionType.SalesReturn)
                return;

            logger.Trace("FillUnPickedRXs() - " + clsPOSDBConstants.Log_Entering);
            DataTable oRxInfo;

            int iNDC = 0;

            frmPatientRXSearch ofrm = new frmPatientRXSearch(oRXHeader);
            DateTime oDate = Configuration.MinimumDate;
            if (validateWithMinDate == false)
            {
                if (IsPOSLite == true) // Nileshj - POSLITE
                {
                    string[] rxOrItemList = argsPass[0].Split(',');

                    ofrm.SearchPrimeRx(rxOrItemList);
                    // ofrm.SearchPrimeRx(argsPass[0].ToString());
                }
                else if (isBatchDelivery) // BatchDelivery - Nileshj - PRIMERX-7688 23-Sept-2019
                {
                    try
                    {
                        ofrm.SearchPrimeRx(argsPass, isBatchDelivery); // PRIMRX-7688 - NileshJ - BatchDelivery added isBatchDelivery 23-Sept-2019
                    }
                    catch (Exception ex)
                    {
                        logger.Fatal(ex, "FillUnPickedRXs()");
                        clsUIHelper.ShowErrorMsg(ex.Message);

                    }
                }
                else
                {
                    ofrm.Search();
                }
                DataTable oTable = ofrm.SelectedData;
                if (oTable == null)
                {
                    isSearchUnPickCancel = true;    //PRIMEPOS-2461 03-Mar-2021 JY Added
                    return;
                }
                if (oTable.Rows.Count == 0)
                {
                    isSearchUnPickCancel = true;    //PRIMEPOS-2461 03-Mar-2021 JY Added
                    return;
                }
            }
            oRxInfo = ofrm.SelectedData;

            string prevRxNo = string.Empty;
            string pRxNoRefill = string.Empty;
            if (oPOSTrans.CheckUnpickedRxLocally(oRxInfo, oRXHeader))
            {
                return;
            }

            if (ofrm.ShowDialog(this) == DialogResult.Cancel)
            {
                isSearchUnPickCancel = true; //User click cancel
                return;
            }

            oRxInfo = ofrm.SelectedData;

            if (oRxInfo.Rows.Count == 0)
            {
                isSearchUnPickCancel = true; //Added by Manoj 3/31/2014 -- User did not select any rx
                return;
            }

            #region  PRIMEPOS-2459 27-Feb-2019 JY Added
            ArrayList alRxNo = new ArrayList();
            ArrayList alPatientNo = new ArrayList();
            try
            {
                foreach (DataRow row in oPOSTrans.oTransDRXData.TransDetailRX.Rows)
                {
                    if (!alRxNo.Contains(row["RXNo"].ToString()))
                        alRxNo.Add(row["RXNo"].ToString());
                    if (!alPatientNo.Contains(row["PatientNo"].ToString()))
                        alPatientNo.Add(row["PatientNo"].ToString());
                }
            }
            catch { }
            #endregion

            #region selected unpicked Rx loop  (Claims table)
            bool bSkipRx = false;
            foreach (DataRow oRXRow in oRxInfo.Rows)
            {
                string sRXNo = oRXRow["RXNo"].ToString();
                string sRefill = oRXRow["nrefill"].ToString();
                int iPartialFillNo = 0;
                if (oRxInfo.Columns.Contains("PartialFillNo"))
                    iPartialFillNo = Configuration.convertNullToInt(oRXRow["PartialFillNo"]);

                if (isBatchDelivery) // BatchDelievery - NileshJ - Add Condition to skip Pickedup condition for BatchDelivery - PRIMERX-7688
                {
                    if ((oRxInfo.Rows[0]["PickupPOS"].ToString() == "Y")
                   && oPOSTrans.CurrentTransactionType == POS_Core.TransType.POSTransactionType.Sales)
                    {
                        continue;
                    }
                }
                else
                {
                    //Added & Modified By SRT On May-27-09
                    if ((oRxInfo.Rows[0]["Pickedup"].ToString() == "Y" || oRxInfo.Rows[0]["PickupPOS"].ToString() == "Y")
                   && oPOSTrans.CurrentTransactionType == POS_Core.TransType.POSTransactionType.Sales)
                    {
                        continue;
                    }
                }

                bool itemAlreadyExist = false;
                TransDetailRXRow oDetail = null;

                //for(int Index = 0; Index < oTransDRXData.TransDetailRX.Rows.Count; Index++)
                //{
                bool bNewPatient = false;
                #region inner loop for TransDetailRX
                foreach (DataRow rID in oPOSTrans.oTransDRXData.TransDetailRX.Rows)
                {
                    bNewPatient = false;
                    oDetail = oPOSTrans.oTransDRXData.TransDetailRX.GetRowByID(Convert.ToInt32(rID["RXDETAILID"].ToString()));//Fix by Manoj 8/15/2014 - (Index + 1)
                    if (oDetail == null)
                    {
                        continue;
                    }
                    if ((oDetail.RXNo.ToString() == oRXRow["RXNo"].ToString() && Configuration.CInfo.AllowMultipleRXRefillsInSameTrans == false)
                    || (oDetail.RXNo.ToString() == oRXRow["RXNo"].ToString() && oDetail.NRefill.ToString() == oRXRow["nrefill"].ToString()
                    && Configuration.CInfo.AllowMultipleRXRefillsInSameTrans == true && (!oRxInfo.Columns.Contains("PartialFillNo") || oDetail.PartialFillNo.ToString() == oRXRow["PartialFillNo"].ToString())))
                    {
                        itemAlreadyExist = true;
                        break;
                    }
                    else//Added By shitaljit for displaying RX notes if any.
                    {
                        #region PRIMEPOS-2459 27-Feb-2019 JY Added
                        bool bNewRx = false;
                        try
                        {
                            if (!alRxNo.Contains(oRXRow["RXNo"].ToString()))
                            {
                                bNewRx = true;
                                alRxNo.Add(oRXRow["RXNo"].ToString());

                                if (!alPatientNo.Contains(oRXRow["PatientNo"].ToString()))
                                {
                                    bNewPatient = true;
                                    alPatientNo.Add(oRXRow["PatientNo"].ToString());
                                }
                            }

                            #region PRIMEPOS-2317 02-Apr-2019 JY Added
                            try
                            {
                                if (bNewPatient)
                                {
                                    bSkipRx = false;
                                    DataTable oTable1 = oPharmBL.GetPatient(oRXRow["PatientNo"].ToString());
                                    if (Configuration.CInfo.ConfirmPatient == 1)
                                    {
                                        ShowPatientInformation(oTable1, false);
                                    }
                                    else if (Configuration.CInfo.ConfirmPatient == 2)
                                    {
                                        if (!ShowPatientInformation(oTable1, true))
                                        {
                                            bool bTemp = isSearchUnPickCancel;
                                            oPOSTrans.DeleteRxOnValidating(RxWithValidClass, DrugClassInfoCapture, isAnyUnPickRx, ref isSearchUnPickCancel); //PRIMEPOS-3319
                                            isSearchUnPickCancel = bTemp;
                                            bSkipRx = true;
                                            break;
                                        }
                                    }
                                    oTable1.Dispose();
                                    oTable1 = null;
                                }
                            }
                            catch { }
                            #endregion
                            if (bSkipRx == false)
                            {
                                if (bNewRx && bNewPatient)
                                    ShowPrimeRXPOSNotes(oRXRow["RXNo"].ToString(), oRXRow["PatientNo"].ToString());
                                else if (bNewRx && !bNewPatient)
                                    ShowPrimeRXPOSNotes(oRXRow["RXNo"].ToString(), "");

                                #region PRIMEPOS-2461 16-Feb-2021 JY Added
                                //try
                                //{
                                //    if (bNewRx && Configuration.CSetting.PatientCounselingPrompt == "2")
                                //    {
                                //        Resources.Message.Display("Please provide counseling for the prescription(s).", "PrimePOS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                //    }
                                //}
                                //catch (Exception Ex){}
                                #endregion
                            }
                        }
                        catch { }
                        #endregion
                    }
                }
                #endregion
                //}

                #region PRIMEPOS-2317 04-Apr-2019 JY Added
                if (oPOSTrans.oTransDRXData.TransDetailRX.Rows.Count == 0)
                {
                    if (!alRxNo.Contains(oRXRow["RXNO"].ToString()))
                        alRxNo.Add(oRXRow["RXNO"].ToString());
                    if (!alPatientNo.Contains(oRXRow["PatientNo"].ToString()))
                    {
                        bNewPatient = true;
                        alPatientNo.Add(oRXRow["PatientNo"].ToString());
                    }

                    try
                    {
                        if (bNewPatient)
                        {
                            bSkipRx = false;
                            oPharmBL = new PharmData.PharmBL();
                            DataTable oTable1 = oPharmBL.GetPatient(oRXRow["PatientNo"].ToString());
                            if (Configuration.CInfo.ConfirmPatient == 1)
                            {
                                ShowPatientInformation(oTable1, false);
                            }
                            else if (Configuration.CInfo.ConfirmPatient == 2)
                            {
                                if (!ShowPatientInformation(oTable1, true))
                                {
                                    bool bTemp = isSearchUnPickCancel;
                                    oPOSTrans.DeleteRxOnValidating(RxWithValidClass, DrugClassInfoCapture, false, ref isSearchUnPickCancel);
                                    isSearchUnPickCancel = bTemp;
                                    bSkipRx = true;
                                }
                            }
                            oTable1.Dispose();
                            oTable1 = null;

                            if (bSkipRx == false)
                            {
                                ShowPrimeRXPOSNotes(oRXRow["RXNo"].ToString(), oRXRow["PatientNo"].ToString());

                                //#region PRIMEPOS-2461 16-Feb-2021 JY Added
                                //try
                                //{
                                //    if (Configuration.CSetting.PatientCounselingPrompt == "2")
                                //    {
                                //        Resources.Message.Display("Please provide counseling for the prescription(s).", "PrimePOS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                //    }
                                //}
                                //catch (Exception Ex){}
                                //#endregion
                            }
                        }
                    }
                    catch { }
                }
                #endregion

                if (itemAlreadyExist == true || bSkipRx == true)
                {
                    continue;
                }

                oPOSTrans.isValidDrugClassRx(oRXRow["RXNO"].ToString(), oRXRow["NRefill"].ToString(), oRXRow["Class"].ToString(), ref RxWithValidClass, iPartialFillNo.ToString());

                oPOSTrans.oTDRow = this.oPOSTrans.oTransDData.TransDetail.AddRow(clsUIHelper.GetNextNumber(oPOSTrans.oTransDData, clsPOSDBConstants.TransDetail_Fld_TransDetailID), 0, this.DefaultQTY, 0, 0, 0, 0, 0, "RX", "");

                oPOSTrans.oTDRow.Category = "F";//Added By Shitaljit on 13 march 2012 to marked FSA item

                bool bMatchFound = false;   //PRIMEPOS-2924 09-Dec-2020 JY Added
                if (oPOSTrans.InsToIgnoreCopay(oRXRow["Pattype"].ToString(), oRXRow["billtype"].ToString(), Configuration.convertNullToDecimal(oRXRow["PatAmt"].ToString())) == false)
                {
                    //if (oPOSTrans.CurrentTransactionType == POS_Core.TransType.POSTransactionType.SalesReturn)
                    //    oPOSTrans.oTDRow.ExtendedPrice = Configuration.convertNullToDecimal(oRXRow["PatAmt"].ToString()) * -1;
                    //else
                    //    oPOSTrans.oTDRow.ExtendedPrice = Configuration.convertNullToDecimal(oRXRow["PatAmt"].ToString());

                    #region PRIMEPOS-2924 09-Dec-2020 JY Added
                    if (Configuration.CSetting.RxTaxPolicy.Trim() == "1" && Configuration.CSetting.RxInsuranceToBeTaxed.Trim() != "" && oRXRow["Pattype"].ToString().Trim() != "" && Configuration.convertNullToDecimal(oRXRow["PatAmt"]) > 0)  //PRIMEPOS-3053 04-Feb-2021 JY Added RxTaxPolicy condition
                    {
                        string[] arrRxInsuranceToBeTaxed = Configuration.CSetting.RxInsuranceToBeTaxed.Split(',');
                        for (int i = 0; i < arrRxInsuranceToBeTaxed.Length; i++)
                        {
                            if (arrRxInsuranceToBeTaxed[i].Trim().ToUpper() == oRXRow["Pattype"].ToString().Trim().ToUpper())
                            {
                                bMatchFound = true;
                                break;
                            }
                        }
                    }
                    if (bMatchFound)
                    {
                        if (oPOSTrans.CurrentTransactionType == POS_Core.TransType.POSTransactionType.SalesReturn)
                            oPOSTrans.oTDRow.ExtendedPrice = (Configuration.convertNullToDecimal(oRXRow["PatAmt"]) - Configuration.convertNullToDecimal(oRXRow["STAX"])) * -1;
                        else
                            oPOSTrans.oTDRow.ExtendedPrice = (Configuration.convertNullToDecimal(oRXRow["PatAmt"]) - Configuration.convertNullToDecimal(oRXRow["STAX"]));
                    }
                    else
                    {
                        if (oPOSTrans.CurrentTransactionType == POS_Core.TransType.POSTransactionType.SalesReturn)
                            oPOSTrans.oTDRow.ExtendedPrice = Configuration.convertNullToDecimal(this.txtExtAmount.Text) * -1;
                        else
                            oPOSTrans.oTDRow.ExtendedPrice = Configuration.convertNullToDecimal(oRXRow["PatAmt"]);
                    }
                    #endregion                    
                    oPOSTrans.oTDRow.Price = oPOSTrans.oTDRow.ExtendedPrice;    //Sprint-18 24-Nov-2014 JY Added to set price to non-zero in case of rx item
                }
                else
                {
                    oPOSTrans.oTDRow.ExtendedPrice = 0;
                }

                #region PRIMEPOS-2924 03-Dec-2020 JY Added
                if (bMatchFound)
                {
                    ItemTax oTaxCodes = new ItemTax();
                    TaxCodesData oTaxCodesData = new TaxCodesData();
                    using (var dao = new TaxCodesSvr())
                    {
                        oTaxCodesData = dao.PopulateList(" WHERE " + clsPOSDBConstants.TaxCodes_Fld_TaxCode + " = '" + clsPOSDBConstants.TaxCodes_Fld_RxTax + "'");
                    }

                    if (Configuration.isNullOrEmptyDataSet(oTaxCodesData) == false)
                    {
                        oPOSTrans.UpdateTransTaxDetails(oPOSTrans.oTDTaxData, oPOSTrans.oTDRow.TransDetailID);
                        if (oPOSTrans.CurrentTransactionType == POS_Core.TransType.POSTransactionType.SalesReturn)
                            oPOSTrans.oTDRow.TaxAmount = Configuration.convertNullToDecimal(oRXRow["STAX"]) * -1;
                        else
                            oPOSTrans.oTDRow.TaxAmount = Configuration.convertNullToDecimal(oRXRow["STAX"]);

                        int ItemRow = 1;
                        if (oPOSTrans.oTDTaxData.TransDetailTax.Rows.Count > 0)
                        {
                            ItemRow += Configuration.convertNullToInt(oPOSTrans.oTDTaxData.TransDetailTax.Rows[oPOSTrans.oTDTaxData.TransDetailTax.Rows.Count - 1]["ItemRow"]);
                        }
                        oPOSTrans.oTDRow.TaxCode = "";
                        bool AddRow = false;
                        int rowIndex = 1;
                        for (int i = 1; i <= oTaxCodesData.Tables[0].Rows.Count; i++)
                        {
                            if (oPOSTrans.oTDTaxData.TransDetailTax.Rows.Count > 0 && !AddRow)
                            {
                                rowIndex += Configuration.convertNullToInt(oPOSTrans.oTDTaxData.TransDetailTax.Rows[oPOSTrans.oTDTaxData.TransDetailTax.Rows.Count - 1]["TransDetailTaxID"]);
                            }
                            AddRow = true;
                            oPOSTrans.oTDTaxData.TransDetailTax.AddRow(rowIndex, oPOSTrans.oTDRow.TransDetailID, 0, 0, oPOSTrans.oTDRow.TaxAmount, oPOSTrans.oTDRow.ItemID, Convert.ToInt32(oTaxCodesData.Tables[0].Rows[i - 1]["TaxID"].ToString()), ItemRow);
                            rowIndex++;

                            #region added logic to update TaxCode in TransDetail
                            if (oPOSTrans.oTDRow.TaxCode == "")
                                oPOSTrans.oTDRow.TaxCode = Configuration.convertNullToString(oTaxCodesData.TaxCodes[i - 1]["TaxCode"]);
                            else
                                oPOSTrans.oTDRow.TaxCode += "," + Configuration.convertNullToString(oTaxCodesData.TaxCodes[i - 1]["TaxCode"]);
                            #endregion
                        }
                    }
                }
                #endregion
                #region PRIMEPOS-3053 07-Feb-2021 JY Added
                else if (Configuration.CSetting.RxTaxPolicy.Trim() == "2")
                {
                    ItemRow oItemRow = null;
                    oPOSTrans.GetItemRowByItemId(oPOSTrans.oTDRow.ItemID, ref oItemRow);
                    oPOSTrans.ApplyTaxPolicy(oItemRow, oPOSTrans.oTDRow.TransDetailID);
                    string sTaxCodes;
                    if (oPOSTrans.IsItemTaxableForTrasaction(oPOSTrans.oTDTaxData, oPOSTrans.oTDRow.ItemID, out sTaxCodes, oPOSTrans.oTDRow.TransDetailID) == true)
                    {
                        EditTax(oPOSTrans.oTDRow.TaxCode, clsPOSDBConstants.TaxCodes_tbl);
                    }
                }
                #endregion

                oPOSTrans.oTDRow.ItemDescription = oRXRow["RXNo"].ToString() + "-" + oRXRow["nRefill"].ToString() + "-" + oRXRow["DETDRGNAME"].ToString();
                oPOSTrans.oTDRow.ItemDescriptionMasked = MaskDrugName(oPOSTrans.oTDRow.ItemID, oPOSTrans.oTDRow.ItemDescription);  //PRIMEPOS-3130
                if (iPartialFillNo > 0)
                    oPOSTrans.oTDRow.ItemDescription = oRXRow["RXNo"].ToString() + "-" + oRXRow["nRefill"].ToString() + "-" + iPartialFillNo.ToString() + "-" + oRXRow["DETDRGNAME"].ToString();
                oPOSTrans.oTDRow.QTY = this.DefaultQTY;
                grdDetail.Refresh(); //Added by Manoj 1/22/2015
                oPOSTrans.SetRowTrans(oPOSTrans.oTDRow);

                //Added form Naim 26032009
                if (Configuration.CInfo.CheckRXItemsForIIAS == true) //Changed by Naim 17Apr2009
                {
                    oPOSTrans.oTDRow.IsIIAS = oPOSTrans.IsIIASItem(oRXRow["ndc"].ToString());
                }
                else
                {
                    oPOSTrans.oTDRow.IsIIAS = true;
                }
                //Added till here Naim 26032009

                //Added By SRT(Ritesh Parekh) Date: 17-Aug-2009
                //To Identify whether this row was Rx or not.
                oPOSTrans.oTDRow.IsRxItem = true;
                //End Of Added By SRT(Ritesh Parekh)

                #region Sprint-25 - PRIMEPOS-2322 03-Feb-2017 JY Added logic to maintain other family member details in RxHeader - otherwise all records will be marked against same patientid
                RXHeader oRXHeaderNew = new RXHeader();
                bool bNewRxHEader = false;

                oPOSTrans.BuildRxHeader(oRXRow, ref oRXHeaderNew, ref bNewRxHEader);
                #endregion

                RXDetail oRXDetail = oPOSTrans.BuildRxDetail(oRXRow);
                //if (bNewRxHEader == true) //PRIMEPOS-2650 28-Feb-2019 JY Commented
                if (bNewRxHEader == true && oPOSTrans.ItemAlreadyProcess == false)  //PRIMEPOS-2650 28-Feb-2019 JY Added
                {
                    //bNewRxHEader = false; // commented By Rohit Nair for PRIMEPOS-2469
                    //
                    oRXHeaderNew.RXDetails.Add(oRXDetail);
                    if (oRXHeaderNew.RXDetails.Count > 0)
                        isAnyUnPickRx = true;
                }
                else
                {
                    #region PRIMEPOS-2694 14-Jun-2019 JY Added
                    if (oRXHeaderNew != null)
                    {
                        oRXHeaderNew.RXDetails.Add(oRXDetail);
                        if (oRXHeaderNew.RXDetails.Count > 0)
                            isAnyUnPickRx = true;
                    }
                    #endregion
                    else
                    {
                        oRXHeader.RXDetails.Add(oRXDetail);
                        if (oRXHeader.RXDetails.Count > 0)
                            isAnyUnPickRx = true;
                    }
                }

                DataTable oTable;
                //oPOSTrans.InsertTransRxData(oRxInfo.Rows[0], oRXDetail, grdDetail.Rows.Count, bNewRxHEader, ref oRXHeaderNew, ref oRXHeader, out oTable); //PRIMEPOS-2694 20-Jun-2019 JY Commented
                oPOSTrans.InsertTransRxData(oRxInfo, oRXDetail, grdDetail.Rows.Count, bNewRxHEader, ref oRXHeaderNew, ref oRXHeader, out oTable);   //PRIMEPOS-2694 20-Jun-2019 JY Added

                if (oPOSTrans.oTDRow != null)
                {
                    iNDC += 1; //Added by Manoj 1/7/2015
                }
                #region PRIMEPOS-2469 By Rohit Nair		
                if (bNewRxHEader == true)
                {
                    bNewRxHEader = false;
                }
                #endregion
                this.isAddRow = true;
                if (Configuration.CPOSSet.UsePoleDisplay)
                {
                    ShowItemOnPoleDisp(oPOSTrans.oTDRow);
                }
                if (Configuration.CPOSSet.UseSigPad == true)
                {
                    logger.Trace("FillUnPickedRXs() -  Add Unpicked RX Item - ItemId: " + oPOSTrans.oTDRow.ItemID + " ItemDescription: " + oPOSTrans.oTDRow.ItemDescription + " TransDetailID: " + oPOSTrans.oTDRow.TransDetailID);
                    if (SigPadUtil.DefaultInstance.IsVF)
                        DeviceItemsProcess("AddItem", oPOSTrans.oTDRow, grdDetail.Rows.Count - 1);
                    else
                        SigPadUtil.DefaultInstance.AddItem(oPOSTrans.oTDRow, grdDetail.Rows.Count - 1);
                }
                oPOSTrans.oTDRow = null;  //Sprint-18 20-Nov-2014 JY Added to resolve blank item description or update item desc as per search text box  
            }
            #endregion
            logger.Trace("FillUnPickedRXs() - " + clsPOSDBConstants.Log_Exiting);
        }

        #region PRIMEPOS-2036 24-Jan-2019 JY Added
        private void FillUnPickedRXs(string sFacilityCode, bool validateWithMinDate, char cType)
        {
            RXHeader oRXHeader;
            PharmBL oPharmBL = new PharmBL();
            isSearchUnPickCancel = false;
            if (oPOSTrans.CurrentTransactionType == POS_Core.TransType.POSTransactionType.SalesReturn)
                return;

            logger.Trace("FillUnPickedRXs(string sFacilityCode, bool validateWithMinDate, char cType) - " + clsPOSDBConstants.Log_Entering);

            DataTable oRxInfo;
            int iNDC = 0;

            frmPatientRXSearch ofrm = new frmPatientRXSearch(sFacilityCode, cType);
            DateTime oDate = Configuration.MinimumDate;
            if (validateWithMinDate == false)
            {
                ofrm.Search();
                DataTable oTable = ofrm.SelectedData;
                if (oTable == null)
                {
                    return;
                }
                if (oTable.Rows.Count == 0)
                {
                    return;
                }
            }
            oRxInfo = ofrm.SelectedData;
            oRXHeader = oPOSTrans.InsertRXHeader(oRxInfo);

            string prevRxNo = string.Empty;
            string pRxNoRefill = string.Empty;
            if (oPOSTrans.CheckUnpickedRxLocally(oRxInfo, oRXHeader))
            {
                return;
            }

            this.txtItemCode.Text = "";
            isSearchUnPickCancel = false;//PRIMEPOS-3319
            if (ofrm.ShowDialog(this) == DialogResult.Cancel)
            {
                isSearchUnPickCancel = true; //User click cancel
                return;
            }

            oRxInfo = ofrm.SelectedData;

            if (oRxInfo.Rows.Count == 0)
            {
                isSearchUnPickCancel = true;
                return;
            }

            #region  PRIMEPOS-2459 27-Feb-2019 JY Added
            ArrayList alRxNo = new ArrayList();
            ArrayList alPatientNo = new ArrayList();
            try
            {
                foreach (DataRow row in oPOSTrans.oTransDRXData.TransDetailRX.Rows)
                {
                    if (!alRxNo.Contains(row["RXNo"].ToString()))
                        alRxNo.Add(row["RXNo"].ToString());
                    if (!alPatientNo.Contains(row["PatientNo"].ToString()))
                        alPatientNo.Add(row["PatientNo"].ToString());
                }
            }
            catch { }
            #endregion
            bool bSkipRx = false;
            //bool bPharmacistNeedsToCouncilPatient = false;   //PRIMEPOS-2461 23-Feb-2021 JY Added
            foreach (DataRow oRXRow in oRxInfo.Rows)
            {
                string sRXNo = oRXRow["RXNo"].ToString();
                string sRefill = oRXRow["nrefill"].ToString();
                string sPartialFillNo = "0";
                if (oRxInfo.Columns.Contains("PartialFillNo"))
                    sPartialFillNo = oRXRow["PartialFillNo"].ToString();

                if ((oRxInfo.Rows[0]["Pickedup"].ToString() == "Y" || oRxInfo.Rows[0]["PickupPOS"].ToString() == "Y")
                    && oPOSTrans.CurrentTransactionType == POS_Core.TransType.POSTransactionType.Sales)
                {
                    continue;
                }

                bool itemAlreadyExist = false;
                TransDetailRXRow oDetail = null;
                bool bNewPatient = false;
                foreach (DataRow rID in oPOSTrans.oTransDRXData.TransDetailRX.Rows)
                {
                    bNewPatient = false;
                    oDetail = oPOSTrans.oTransDRXData.TransDetailRX.GetRowByID(Convert.ToInt32(rID["RXDETAILID"].ToString()));
                    if (oDetail == null)
                    {
                        continue;
                    }
                    if ((oDetail.RXNo.ToString() == oRXRow["RXNo"].ToString() && Configuration.CInfo.AllowMultipleRXRefillsInSameTrans == false)
                    || (oDetail.RXNo.ToString() == oRXRow["RXNo"].ToString() && oDetail.NRefill.ToString() == oRXRow["nrefill"].ToString()
                    && Configuration.CInfo.AllowMultipleRXRefillsInSameTrans == true))
                    {
                        itemAlreadyExist = true;
                        break;
                    }
                    else
                    {
                        #region PRIMEPOS-2459 27-Feb-2019 JY Added
                        bool bNewRx = false;
                        try
                        {
                            if (!alRxNo.Contains(oRXRow["RXNo"].ToString()))
                            {
                                bNewRx = true;
                                alRxNo.Add(oRXRow["RXNo"].ToString());
                            }

                            if (!alPatientNo.Contains(oRXRow["PatientNo"].ToString()))
                            {
                                bNewPatient = true;
                                alPatientNo.Add(oRXRow["PatientNo"].ToString());
                            }

                            #region PRIMEPOS-2317 02-Apr-2019 JY Added
                            try
                            {
                                if (bNewPatient)
                                {
                                    bSkipRx = false;
                                    DataTable oTable1 = oPharmBL.GetPatient(oRXRow["PatientNo"].ToString());
                                    if (Configuration.CInfo.ConfirmPatient == 1)
                                    {
                                        ShowPatientInformation(oTable1, false);
                                    }
                                    else if (Configuration.CInfo.ConfirmPatient == 2)
                                    {
                                        if (!ShowPatientInformation(oTable1, true))
                                        {
                                            bool bTemp = isSearchUnPickCancel;
                                            oPOSTrans.DeleteRxOnValidating(RxWithValidClass, DrugClassInfoCapture, false, ref isSearchUnPickCancel);
                                            isSearchUnPickCancel = bTemp;
                                            bSkipRx = true;
                                            break;
                                        }
                                    }
                                    oTable1.Dispose();
                                    oTable1 = null;
                                }
                            }
                            catch { }
                            #endregion

                            if (bSkipRx == false)
                            {
                                if (bNewRx && bNewPatient)
                                    ShowPrimeRXPOSNotes(oRXRow["RXNo"].ToString(), oRXRow["PatientNo"].ToString());
                                else if (bNewRx && !bNewPatient)
                                    ShowPrimeRXPOSNotes(oRXRow["RXNo"].ToString(), "");
                                else if (!bNewRx && bNewPatient)
                                    ShowPrimeRXPOSNotes("", oRXRow["PatientNo"].ToString());

                                //bPharmacistNeedsToCouncilPatient = true; //PRIMEPOS-2461 23-Feb-2021 JY Added
                            }
                        }
                        catch { }
                        #endregion
                    }
                }

                #region PRIMEPOS-2317 04-Apr-2019 JY Added
                if (oPOSTrans.oTransDRXData.TransDetailRX.Rows.Count == 0)
                {
                    if (!alRxNo.Contains(oRXRow["RXNO"].ToString()))
                        alRxNo.Add(oRXRow["RXNO"].ToString());
                    if (!alPatientNo.Contains(oRXRow["PatientNo"].ToString()))
                    {
                        bNewPatient = true;
                        alPatientNo.Add(oRXRow["PatientNo"].ToString());
                    }

                    try
                    {
                        if (bNewPatient)
                        {
                            bSkipRx = false;
                            oPharmBL = new PharmData.PharmBL();
                            DataTable oTable1 = oPharmBL.GetPatient(oRXRow["PatientNo"].ToString());
                            if (Configuration.CInfo.ConfirmPatient == 1)
                            {
                                ShowPatientInformation(oTable1, false);
                            }
                            else if (Configuration.CInfo.ConfirmPatient == 2)
                            {
                                if (!ShowPatientInformation(oTable1, true))
                                {
                                    bool bTemp = isSearchUnPickCancel;
                                    oPOSTrans.DeleteRxOnValidating(RxWithValidClass, DrugClassInfoCapture, false, ref isSearchUnPickCancel);
                                    isSearchUnPickCancel = bTemp;
                                    bSkipRx = true;
                                }
                            }
                            oTable1.Dispose();
                            oTable1 = null;

                            if (bSkipRx == false)
                            {
                                ShowPrimeRXPOSNotes(oRXRow["RXNo"].ToString(), oRXRow["PatientNo"].ToString());
                                //bPharmacistNeedsToCouncilPatient = true;    //PRIMEPOS-2461 23-Feb-2021 JY Added
                            }
                        }
                    }
                    catch { }
                }
                #endregion

                if (itemAlreadyExist == true || bSkipRx == true)
                {
                    continue;
                }

                oPOSTrans.isValidDrugClassRx(oRXRow["RXNO"].ToString(), oRXRow["NRefill"].ToString(), oRXRow["Class"].ToString(), ref RxWithValidClass, sPartialFillNo);

                oPOSTrans.oTDRow = this.oPOSTrans.oTransDData.TransDetail.AddRow(clsUIHelper.GetNextNumber(oPOSTrans.oTransDData, clsPOSDBConstants.TransDetail_Fld_TransDetailID), 0, this.DefaultQTY, 0, 0, 0, 0, 0, "RX", "");
                oPOSTrans.oTDRow.Category = "F";

                if (oPOSTrans.InsToIgnoreCopay(oRXRow["Pattype"].ToString(), oRXRow["billtype"].ToString(), Configuration.convertNullToDecimal(oRXRow["PatAmt"].ToString())) == false)
                {
                    if (oPOSTrans.CurrentTransactionType == POS_Core.TransType.POSTransactionType.SalesReturn)
                    {
                        oPOSTrans.oTDRow.ExtendedPrice = Configuration.convertNullToDecimal(oRXRow["PatAmt"].ToString()) * -1;
                    }
                    else
                    {
                        oPOSTrans.oTDRow.ExtendedPrice = Configuration.convertNullToDecimal(oRXRow["PatAmt"].ToString());
                    }
                    oPOSTrans.oTDRow.Price = oPOSTrans.oTDRow.ExtendedPrice;    //Sprint-18 24-Nov-2014 JY Added to set price to non-zero in case of rx item
                }
                else
                {
                    oPOSTrans.oTDRow.ExtendedPrice = 0;
                }

                oPOSTrans.oTDRow.ItemDescription = oRXRow["RXNo"].ToString() + "-" + oRXRow["nRefill"].ToString() + "-" + oRXRow["DETDRGNAME"].ToString();
                oPOSTrans.oTDRow.QTY = this.DefaultQTY;
                grdDetail.Refresh();
                oPOSTrans.SetRowTrans(oPOSTrans.oTDRow);

                if (Configuration.CInfo.CheckRXItemsForIIAS == true)
                {
                    oPOSTrans.oTDRow.IsIIAS = oPOSTrans.IsIIASItem(oRXRow["ndc"].ToString());
                }
                else
                {
                    oPOSTrans.oTDRow.IsIIAS = true;
                }

                //To Identify whether this row was Rx or not.
                oPOSTrans.oTDRow.IsRxItem = true;

                #region Added logic to maintain other family member details in RxHeader - otherwise all records will be marked against same patientid
                RXHeader oRXHeaderNew = new RXHeader();
                bool bNewRxHEader = false;
                oPOSTrans.BuildRxHeader(oRXRow, ref oRXHeaderNew, ref bNewRxHEader);
                #endregion

                RXDetail oRXDetail = oPOSTrans.BuildRxDetail(oRXRow);
                if (bNewRxHEader == true)
                {
                    oRXHeaderNew.RXDetails.Add(oRXDetail);
                    if (oRXHeaderNew.RXDetails.Count > 0)
                        isAnyUnPickRx = true;
                }
                else
                {
                    #region PRIMEPOS-2694 14-Jun-2019 JY Added
                    if (oRXHeaderNew != null)
                    {
                        oRXHeaderNew.RXDetails.Add(oRXDetail);
                        if (oRXHeaderNew.RXDetails.Count > 0)
                            isAnyUnPickRx = true;
                    }
                    #endregion
                    else
                    {
                        oRXHeader.RXDetails.Add(oRXDetail);
                        if (oRXHeader.RXDetails.Count > 0)
                            isAnyUnPickRx = true;
                    }
                }
                DataTable oTable;
                //oPOSTrans.InsertTransRxData(oRxInfo.Rows[0], oRXDetail, grdDetail.Rows.Count, bNewRxHEader, ref oRXHeaderNew, ref oRXHeader, out oTable); //PRIMEPOS-2694 20-Jun-2019 JY Commented
                oPOSTrans.InsertTransRxData(oRxInfo, oRXDetail, grdDetail.Rows.Count, bNewRxHEader, ref oRXHeaderNew, ref oRXHeader, out oTable);   //PRIMEPOS-2694 20-Jun-2019 JY Added

                if (oPOSTrans.oTDRow != null)
                {
                    iNDC += 1;
                }

                #region PRIMEPOS-2469 By Rohit Nair		
                if (bNewRxHEader == true)
                {
                    bNewRxHEader = false;
                }
                #endregion

                this.isAddRow = true;
                if (Configuration.CPOSSet.UsePoleDisplay)
                {
                    ShowItemOnPoleDisp(oPOSTrans.oTDRow);
                }
                if (Configuration.CPOSSet.UseSigPad == true)
                {
                    logger.Trace("FillUnPickedRXs(string sFacilityCode, bool validateWithMinDate, char cType) -  Add Unpicked RX Item - ItemId: " + oPOSTrans.oTDRow.ItemID + " ItemDescription: " + oPOSTrans.oTDRow.ItemDescription + " TransDetailID: " + oPOSTrans.oTDRow.TransDetailID);
                    if (SigPadUtil.DefaultInstance.IsVF)
                        DeviceItemsProcess("AddItem", oPOSTrans.oTDRow, grdDetail.Rows.Count - 1);
                    else
                        SigPadUtil.DefaultInstance.AddItem(oPOSTrans.oTDRow, grdDetail.Rows.Count - 1);
                }
                oPOSTrans.oTDRow = null;  //Added to resolve blank item description or update item desc as per search text box  
            }

            #region PRIMEPOS-2461 23-Feb-2021 JY Added
            //try
            //{
            //    if (bPharmacistNeedsToCouncilPatient && (Configuration.CSetting.PatientCounselingPrompt == "1" || Configuration.CSetting.PatientCounselingPrompt == "2"))
            //    {
            //        Resources.Message.Display("Please provide counseling for the prescription(s).", "PrimePOS", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //        bPharmacistNeedsToCouncilPatient = false;
            //    }
            //}
            //catch (Exception Ex){}
            #endregion
            logger.Trace("FillUnPickedRXs(string sFacilityCode, bool validateWithMinDate, char cType) - " + clsPOSDBConstants.Log_Exiting);
        }
        #endregion

        //completed by sandeep
        private void FillNonRXInformation(string ItemCode)
        {
            logger.Trace("FillNonRXInformation() - " + clsPOSDBConstants.Log_Entering);
            this.txtItemCode.Text = "RX";
            string stxtDescription = ItemCode.Substring(3, 13);
            this.txtUnitPrice.Value = Configuration.convertNullToDecimal(ItemCode.Substring(17, 7));
            this.txtExtAmount.Text = this.txtUnitPrice.Value.ToString();

            foreach (UltraGridRow oRow in this.grdDetail.Rows)
            {
                if (oRow.Cells[clsPOSDBConstants.TransDetail_Fld_ItemDescription].Value.ToString() == stxtDescription)
                {
                    this.txtItemCode.Text = "";
                    this.txtDescription.Text = "";
                    this.txtUnitPrice.Value = 0;
                    this.txtExtAmount.Text = "0";
                    throw (new Exception("Cannot scan same RX in same transaction."));
                }
            }
            if (oPOSTrans.CurrentTransactionType == POS_Core.TransType.POSTransactionType.SalesReturn)
            {
                this.txtExtAmount.Text = Convert.ToString(Configuration.convertNullToDecimal(this.txtExtAmount.Text) * -1);
            }
            this.txtQty.Value = this.DefaultQTY;
            this.txtDescription.Enabled = false;
            TransDetailData oTData = new TransDetailData();
            oPOSTrans.oTDRow = oTData.TransDetail.AddRow(clsUIHelper.GetNextNumber(oPOSTrans.oTransDData, clsPOSDBConstants.TransDetail_Fld_TransDetailID), 0, this.DefaultQTY, 0, 0, 0, Configuration.convertNullToDecimal(this.txtExtAmount.Text), 0, "RX", stxtDescription);
            oPOSTrans.oTDRow.IsIIAS = true;
            oPOSTrans.oTDRow.IsRxItem = true;
            oPOSTrans.SetRowTrans(oPOSTrans.oTDRow);
            this.isAddRow = true;
            logger.Trace("FillNonRXInformation() - " + clsPOSDBConstants.Log_Exiting);
        }

        //completed by sandeep
        private void ProcessTransactionViewRequest(string sTransId)
        {
            logger.Trace("ProcessTransactionViewRequest() - " + clsPOSDBConstants.Log_Entering);
            frmViewTransactionDetail ofrmVTD = new frmViewTransactionDetail();
            if (sTransId.Trim().Length > 1)
            {
                String TransID = sTransId.Substring(1);

                if (clsUIHelper.isNumeric(TransID))
                {
                    DataSet oTDData = oPOSTrans.PopulateTransactionDetData(Convert.ToInt32(TransID));
                    DataSet oHData = oPOSTrans.GetTransactionHeaderByTransactionId(TransID);
                    bool isROATrans = oPOSTrans.IsROATrans(oHData);
                    if (oTDData != null)
                    {
                        if (oTDData.Tables[0].Rows.Count > 0 || isROATrans == true)
                        {
                            ofrmVTD = new frmViewTransactionDetail(TransID, "", Configuration.StationID, true);
                        }
                        else
                        {
                            clsUIHelper.ShowErrorMsg("Invalid TransID, Please enter a valid TransID.");
                            return;
                        }
                    }
                }
                else
                {
                    ofrmVTD = new frmViewTransactionDetail("0", "", Configuration.StationID, true);
                }
            }
            else
            {
                ofrmVTD = new frmViewTransactionDetail("0", "", Configuration.StationID, true);
            }

            ofrmVTD.ShowDialog();
            if (ofrmVTD.isCopied)
            {
                CopyViewTransaction(ofrmVTD);
            }
            logger.Trace("ProcessTransactionViewRequest() - " + clsPOSDBConstants.Log_Exiting);
        }

        //completed by sandeep
        private void FIllBatchInformation(string BatchCode)
        {
            logger.Trace("FIllBatchInformation() - " + clsPOSDBConstants.Log_Entering);
            PharmBL oPharmBL = new PharmBL();
            DataTable oBatchInfo = new DataTable();
            DataTable oRxInfo = new DataTable();

            long lbatchCode = oPOSTrans.GetInt(BatchCode);

            if (lbatchCode > 0)
            {

                if (this.oPOSTrans.CurrentTransactionType == POS_Core.TransType.POSTransactionType.SalesReturn)
                {
                    #region Fetch Rx Information
                    oRxInfo = oPharmBL.GetRxsinBatch(lbatchCode);
                    if (oRxInfo != null && oRxInfo.Rows.Count > 0)
                    {
                        int rowCount = 0; //PRIMEPOS-3446
                        foreach (DataRow oRXRow in oRxInfo.Rows)
                        {
                            rowCount++; //PRIMEPOS-3446
                            //bool alreadyProcessed = false;    //PRIMEPOS-2699 05-Aug-2019 JY Commented
                            //bool isNotReturned = false;   //PRIMEPOS-2699 05-Aug-2019 JY Commented
                            //bool isAlreadyProcessRx = oPOSTrans.CheckRxPickupDetailPOS(oRXRow, out isNotReturned);    //PRIMEPOS-2699 05-Aug-2019 JY Commented

                            bool balreadyExists = false;
                            bool bAlreadyPickedUp = false, bAlreadyReturned = false, bNeverUsed = false;    //PRIMEPOS-2699 05-Aug-2019 JY Added
                            oPOSTrans.CheckRxPickupDetailPOS(oRXRow, out bAlreadyPickedUp, out bAlreadyReturned, out bNeverUsed);   //PRIMEPOS-2699 05-Aug-2019 JY Added

                            if (bNeverUsed)
                            {
                                clsUIHelper.ShowErrorMsg("Rx#: " + oRXRow["RXNO"].ToString() + " - " + oRXRow["nrefill"].ToString() + " was never processed.\nCannot return a none processed RX!");
                                //alreadyProcessed = true;  //PRIMEPOS-2699 05-Aug-2019 JY Commented
                            }
                            else if (bAlreadyReturned)
                            {
                                clsUIHelper.ShowErrorMsg("Rx#: " + oRXRow["RXNO"].ToString() + " - " + oRXRow["nrefill"].ToString() + " is already returned, so please scan another Rx.");
                            }
                            //if (!alreadyProcessed)    //PRIMEPOS-2699 05-Aug-2019 JY Commented
                            else
                            {
                                RXHeader tmpHeader = oPOSTrans.oRXHeaderList.FindByPatient(oRXRow["PatientNo"].ToString());
                                if (tmpHeader != null)
                                {
                                    var rxitem = tmpHeader.RXDetails.Find(x => (x.RXNo == Convert.ToInt64(oRXRow["RXNo"].ToString()) && x.RefillNo == Convert.ToInt16(oRXRow["nrefill"].ToString())));
                                    if (rxitem != null)
                                    {
                                        balreadyExists = true;
                                    }
                                }
                                else
                                {
                                    balreadyExists = false;
                                }
                                if (!balreadyExists)
                                {
                                    #region Fetching Rx's in Batch
                                    oPOSTrans.oTDRow = this.oPOSTrans.oTransDData.TransDetail.AddRow(clsUIHelper.GetNextNumber(oPOSTrans.oTransDData, clsPOSDBConstants.TransDetail_Fld_TransDetailID), 0, this.DefaultQTY, 0, 0, 0, 0, 0, "RX", "");
                                    oPOSTrans.oTDRow.Category = "F";//Added By Shitaljit on 13 march 2012 to marked FSA item

                                    if (oPOSTrans.InsToIgnoreCopay(oRXRow["Pattype"].ToString(), oRXRow["billtype"].ToString(), Configuration.convertNullToDecimal(oRXRow["PatAmt"].ToString())) == false)
                                    {
                                        if (oPOSTrans.CurrentTransactionType == POS_Core.TransType.POSTransactionType.SalesReturn)
                                        {
                                            oPOSTrans.oTDRow.ExtendedPrice = Configuration.convertNullToDecimal(oRXRow["PatAmt"].ToString()) * -1;
                                        }
                                        else
                                        {
                                            oPOSTrans.oTDRow.ExtendedPrice = Configuration.convertNullToDecimal(oRXRow["PatAmt"].ToString());
                                        }
                                        oPOSTrans.oTDRow.Price = oPOSTrans.oTDRow.ExtendedPrice;    //Sprint-18 24-Nov-2014 JY Added to set price to non-zero in case of rx item
                                    }
                                    else
                                    {
                                        oPOSTrans.oTDRow.ExtendedPrice = 0;
                                    }

                                    oPOSTrans.oTDRow.ItemDescription = oRXRow["RXNo"].ToString() + "-" + oRXRow["nRefill"].ToString() + "-" + oRXRow["DETDRGNAME"].ToString();
                                    oPOSTrans.oTDRow.QTY = this.DefaultQTY;
                                    grdDetail.Refresh(); //Added by Manoj 1/22/2015
                                    oPOSTrans.SetRowTrans(oPOSTrans.oTDRow);

                                    //Added form Naim 26032009
                                    if (Configuration.CInfo.CheckRXItemsForIIAS == true) //Changed by Naim 17Apr2009
                                    {
                                        oPOSTrans.oTDRow.IsIIAS = oPOSTrans.IsIIASItem(oRXRow["ndc"].ToString());
                                    }
                                    else //PRIMEPOS-2715 16-Jul-2019 JY Added
                                    {
                                        oPOSTrans.oTDRow.IsIIAS = true;
                                    }
                                    RXHeader oRxHeader = null; //= new RXHeader();
                                    bool bNewRxHEader = false;

                                    oPOSTrans.BuildRxHeader(oRXRow, ref oRxHeader, ref bNewRxHEader, lbatchCode.ToString());

                                    RXDetail oRXDetail = oPOSTrans.BuildRxDetail(oRXRow);

                                    oRxHeader.RXDetails.Add(oRXDetail);

                                    if (oRxHeader.RXDetails.Count > 0)
                                        isAnyUnPickRx = true;

                                    DataTable oTable;
                                    //oPOSTrans.InsertTransRxData(oRXRow, oRXDetail, grdDetail.Rows.Count, ref oRxHeader, out oTable);  //PRIMEPOS-2694 20-Jun-2019 JY Commented
                                    oPOSTrans.InsertTransRxData(oRxInfo, oRXDetail, grdDetail.Rows.Count,rowCount, ref oRxHeader, out oTable);    //PRIMEPOS-2694 20-Jun-2019 JY Added //PRIMEPOS-3446 Added rowCount parameter

                                    this.isAddRow = true;

                                    if (Configuration.CPOSSet.UsePoleDisplay)
                                    {
                                        ShowItemOnPoleDisp(oPOSTrans.oTDRow);
                                    }
                                    if (Configuration.CPOSSet.UseSigPad == true)
                                    {
                                        logger.Trace("FillUnPickedRXs() -  Add Unpicked RX Item - ItemId: " + oPOSTrans.oTDRow.ItemID + " ItemDescription: " + oPOSTrans.oTDRow.ItemDescription + " TransDetailID: " + oPOSTrans.oTDRow.TransDetailID);
                                        if (SigPadUtil.DefaultInstance.IsVF)
                                            DeviceItemsProcess("AddItem", oPOSTrans.oTDRow, grdDetail.Rows.Count - 1);
                                        else
                                            SigPadUtil.DefaultInstance.AddItem(oPOSTrans.oTDRow, grdDetail.Rows.Count - 1);
                                    }
                                    oPOSTrans.oTDRow = null;
                                    #endregion
                                }
                            }
                        }
                        if (oPOSTrans.oTransHRow?.ReturnTransID != null && oPOSTrans.oTransHRow.ReturnTransID != 0)//PRIMEPOS-2927
                        {
                            try
                            {
                                DataTable dtShipping = oPOSTrans.AddShippingItem(Convert.ToString(oPOSTrans?.oTransHRow?.ReturnTransID));
                                AddShippingCharge(Convert.ToString(Convert.ToDecimal(dtShipping.Rows[0]["Price"]) * -1));
                            }
                            catch (Exception ex)
                            {
                                logger.Error("Exception occured : " + ex);
                            }
                        }
                    }
                    #endregion
                }
                else
                {
                    #region Else Block 
                    oBatchInfo = oPOSTrans.GetBatchStatusfromView(lbatchCode.ToString());

                    if (oBatchInfo != null && oBatchInfo.Rows.Count > 0)
                    {
                        string strErrorMsg = string.Empty;
                        //PrimePOS-2518 Jenny
                        //if (oBatchInfo.Rows[0]["OFSbatchStatus"].ToString().Equals(Configuration.CInfo.IntakeBatchStatus, StringComparison.OrdinalIgnoreCase))
                        if (Convert.ToBoolean(oBatchInfo.Rows[0]["IsReadyforPayment"]))//Arvind 2925
                        {
                            #region Fetch Rx Information
                            oRxInfo = oPharmBL.GetRxsinBatch(lbatchCode);
                            bool bShippingItemAdded = false;
                            if (oRxInfo != null && oRxInfo.Rows.Count > 0)
                            {
                                bool balreadyExists = false;
                                bool bAlreadyPickedUp = false, bAlreadyReturned = false, bNeverUsed = false;    //PRIMEPOS-2699 05-Aug-2019 JY Added
                                int rowCount = 0; //PRIMEPOS-3446
                                foreach (DataRow oRXRow in oRxInfo.Rows)
                                {
                                    rowCount++; //PRIMEPOS-3446
                                    //bool alreadyProcessed = false;    //PRIMEPOS-2699 05-Aug-2019 JY Commented                                        
                                    //bool isNotReturned = false;   //PRIMEPOS-2699 05-Aug-2019 JY Commented
                                    //bool isAlreadyProcessRx = oPOSTrans.CheckRxPickupDetailPOS(oRXRow, out isNotReturned);    //PRIMEPOS-2699 05-Aug-2019 JY Commented

                                    oPOSTrans.CheckRxPickupDetailPOS(oRXRow, out bAlreadyPickedUp, out bAlreadyReturned, out bNeverUsed);   //PRIMEPOS-2699 05-Aug-2019 JY Added

                                    //if (isAlreadyProcessRx && isNotReturned)  //PRIMEPOS-2699 05-Aug-2019 JY Commented  
                                    if (bAlreadyPickedUp)   //PRIMEPOS-2699 05-Aug-2019 JY Added
                                    {
                                        clsUIHelper.ShowErrorMsg("Rx#: " + oRXRow["RXNO"].ToString() + " - " + oRXRow["nrefill"].ToString() + " was already processed and Picked up");
                                        //alreadyProcessed = true;  //PRIMEPOS-2699 05-Aug-2019 JY Commented    
                                    }
                                    //if (!alreadyProcessed)    //PRIMEPOS-2699 05-Aug-2019 JY Commented    
                                    else
                                    {
                                        RXHeader tmpHeader = oPOSTrans.oRXHeaderList.FindByPatient(oRXRow["PatientNo"].ToString());
                                        if (tmpHeader != null)
                                        {
                                            var rxitem = tmpHeader.RXDetails.Find(x => (x.RXNo == Convert.ToInt64(oRXRow["RXNo"].ToString()) && x.RefillNo == Convert.ToInt16(oRXRow["nrefill"].ToString())));
                                            if (rxitem != null)
                                            {
                                                balreadyExists = true;
                                            }
                                        }
                                        else
                                        {
                                            balreadyExists = false;
                                        }
                                        if (!balreadyExists)
                                        {
                                            #region Fetching Rx's in Batch
                                            oPOSTrans.oTDRow = this.oPOSTrans.oTransDData.TransDetail.AddRow(clsUIHelper.GetNextNumber(oPOSTrans.oTransDData, clsPOSDBConstants.TransDetail_Fld_TransDetailID), 0, this.DefaultQTY, 0, 0, 0, 0, 0, "RX", "");
                                            oPOSTrans.oTDRow.Category = "F";//Added By Shitaljit on 13 march 2012 to marked FSA item

                                            if (oPOSTrans.InsToIgnoreCopay(oRXRow["Pattype"].ToString(), oRXRow["billtype"].ToString(), Configuration.convertNullToDecimal(oRXRow["PatAmt"].ToString())) == false)
                                            {
                                                if (oPOSTrans.CurrentTransactionType == POS_Core.TransType.POSTransactionType.SalesReturn)
                                                {
                                                    oPOSTrans.oTDRow.ExtendedPrice = Configuration.convertNullToDecimal(oRXRow["PatAmt"].ToString()) * -1;
                                                }
                                                else
                                                {
                                                    oPOSTrans.oTDRow.ExtendedPrice = Configuration.convertNullToDecimal(oRXRow["PatAmt"].ToString());
                                                }
                                                oPOSTrans.oTDRow.Price = oPOSTrans.oTDRow.ExtendedPrice;    //Sprint-18 24-Nov-2014 JY Added to set price to non-zero in case of rx item
                                            }
                                            else
                                            {
                                                oPOSTrans.oTDRow.ExtendedPrice = 0;
                                            }

                                            oPOSTrans.oTDRow.ItemDescription = oRXRow["RXNo"].ToString() + "-" + oRXRow["nRefill"].ToString() + "-" + oRXRow["DETDRGNAME"].ToString();
                                            oPOSTrans.oTDRow.QTY = this.DefaultQTY;
                                            grdDetail.Refresh(); //Added by Manoj 1/22/2015
                                            oPOSTrans.SetRowTrans(oPOSTrans.oTDRow);

                                            //Added form Naim 26032009
                                            if (Configuration.CInfo.CheckRXItemsForIIAS == true) //Changed by Naim 17Apr2009
                                            {
                                                oPOSTrans.oTDRow.IsIIAS = oPOSTrans.IsIIASItem(oRXRow["ndc"].ToString());
                                            }
                                            else //PRIMEPOS-2715 16-Jul-2019 JY Added
                                            {
                                                oPOSTrans.oTDRow.IsIIAS = true;
                                            }
                                            RXHeader oRxHeader = null; //= new RXHeader();
                                            bool bNewRxHEader = false;

                                            oPOSTrans.BuildRxHeader(oRXRow, ref oRxHeader, ref bNewRxHEader, lbatchCode.ToString());

                                            RXDetail oRXDetail = oPOSTrans.BuildRxDetail(oRXRow);

                                            oRxHeader.RXDetails.Add(oRXDetail);

                                            if (oRxHeader.RXDetails.Count > 0)
                                                isAnyUnPickRx = true;

                                            DataTable oTable;
                                            //oPOSTrans.InsertTransRxData(oRXRow, oRXDetail, grdDetail.Rows.Count, ref oRxHeader, out oTable);  //PRIMEPOS-2694 20-Jun-2019 JY Commented
                                            oPOSTrans.InsertTransRxData(oRxInfo, oRXDetail, grdDetail.Rows.Count,rowCount, ref oRxHeader, out oTable);    //PRIMEPOS-2694 20-Jun-2019 JY Added //PRIMEPOS-3446 Added rowCount parameter

                                            this.isAddRow = true;

                                            if (Configuration.CPOSSet.UsePoleDisplay)
                                            {
                                                ShowItemOnPoleDisp(oPOSTrans.oTDRow);
                                            }
                                            if (Configuration.CPOSSet.UseSigPad == true)
                                            {
                                                logger.Trace("FillUnPickedRXs() -  Add Unpicked RX Item - ItemId: " + oPOSTrans.oTDRow.ItemID + " ItemDescription: " + oPOSTrans.oTDRow.ItemDescription + " TransDetailID: " + oPOSTrans.oTDRow.TransDetailID);
                                                if (SigPadUtil.DefaultInstance.IsVF)
                                                    DeviceItemsProcess("AddItem", oPOSTrans.oTDRow, grdDetail.Rows.Count - 1);
                                                else
                                                    SigPadUtil.DefaultInstance.AddItem(oPOSTrans.oTDRow, grdDetail.Rows.Count - 1);
                                            }
                                            oPOSTrans.oTDRow = null;
                                            #endregion
                                        }
                                    }
                                    if ((!bShippingItemAdded) && (!balreadyExists && !bAlreadyPickedUp))
                                    {
                                        bShippingItemAdded = true;
                                    }
                                }
                            }
                            #endregion                            
                            if (bShippingItemAdded)
                            {
                                if (MailOrderRequiredShipping(Convert.ToString(oBatchInfo.Rows[0]["MailOrderType"]), Convert.ToString(oBatchInfo.Rows[0]["ShippingPrice"])))
                                {
                                    AddShippingCharge(Convert.ToString(oBatchInfo.Rows[0]["ShippingPrice"]));
                                }
                            }
                        }
                        else
                        {
                            strErrorMsg = oPOSTrans.GetErrorMsgForBatchInformation(Convert.ToString(oBatchInfo.Rows[0]["IsReadyforPayment"]).ToUpper(), lbatchCode);//PRIMEPOS-2926
                            clsUIHelper.ShowErrorMsg(strErrorMsg);
                        }
                    }
                    else //PRIMEPOS-3251
                    {
                        string strErrMsg = oPOSTrans.GetErrorMsgForBatchInformation("NF", lbatchCode);
                        clsUIHelper.ShowErrorMsg(strErrMsg);
                    }
                    #endregion
                }
            }
            //return null;
        }

        //PRIMEPOS-2927
        private bool MailOrderRequiredShipping(string MailOrderType, string ShippingAmount)
        {
            logger.Trace("Entered in MailOrderRequiredShipping method");
            if (Configuration.CSetting.AllowMailOrder)
            {
                if (Configuration.CSetting.AllowZeroShippingCharge)
                    return true;
                else if (!string.IsNullOrWhiteSpace(ShippingAmount) && Convert.ToDecimal(ShippingAmount) > 0)
                    return true;
                else
                    return false;
            }
            else
                return false;
        }

        private void AddShippingCharge(string ShippingPrice)
        {
            logger.Trace("Entered in AddShippingCharge()");
            txtItemCode.Text = Configuration.ShippingItem;
            try
            {
                ItemBox_Validatiang(txtItemCode, null);

                isBatchWtihShippingItem = true;

                oPOSTrans.oTDRow.ItemCost = oPOSTrans.oTDRow.ExtendedPrice = oPOSTrans.oTDRow.Price = oPOSTrans.oTDRow.NonComboUnitPrice = Convert.ToDecimal(ShippingPrice);
                oPOSTrans.CalcExtdPrice(oPOSTrans.oTDRow);

                for (int count = 0; count < oPOSTrans.oTransDData.Tables[0].Rows.Count; count++)
                {
                    if (Convert.ToString(oPOSTrans.oTransDData.Tables[0].Rows[count]["ItemID"]) == Configuration.ShippingItem)
                    {
                        oPOSTrans.oTransDData.Tables[0].Rows[count]["ItemCost"] = oPOSTrans.oTransDData.Tables[0].Rows[count]["Price"] = oPOSTrans.oTransDData.Tables[0].Rows[count]["ExtendedPrice"] = oPOSTrans.oTransDData.Tables[0].Rows[count]["NonComboUnitPrice"] = ShippingPrice;
                    }
                }

                this.ClearItemRow();
                this.grdDetail.Focus();
                this.grdDetail.Refresh();
            }
            catch (Exception ex)
            {
                logger.Error("Exception in AddShippingCharge(string ShippingPrice)" + ex);
            }

        }

        //completed by sandeep
        private void SaveReceiveOnAccount(System.Int64 Account_No, System.Decimal Amount, System.Decimal AmountTend, System.Decimal ChagngeDue, Int32 retTransID)
        {
            try
            {
                logger.Trace("SaveReceiveOnAccount() - " + clsPOSDBConstants.Log_Entering + "Amount :" + Amount.ToString(Configuration.CInfo.CurrencySymbol.ToString() + " #####0.00"));
                setTransHeaderRow(Amount, AmountTend, Account_No, retTransID);
                bool isCLTierreached = false;
                decimal CLCouponValue = 0;  //Sprint-18 - 2039 12-Jan-2015 JY Added to preserve active coupon value  

                //Sprint-23 - PRIMEPOS-2029 06-Apr-2016 JY Added MethItem, PatInfo, inquiryId, pseTrxId parameters
                oPOSTrans.Persist(oPOSTrans.oTransHData, oPOSTrans.oTransDData, oPOSTrans.oPOSTransPaymentData, 0, 0, true, this.oPOSTrans.oRXHeaderList, oPOSTransPayment_CCLogList, oPOSTrans.oTransDRXData, null, null, null, null, ref isCLTierreached, ref CLCouponValue, pseTrxId, bItemMonitorInTrans, lstOnHoldRxs, false, strOverrideMaxStationCloseCashLimit, strMaxTransactionAmountUser, strMaxReturnTransactionAmountUser, strInvDiscOverrideUser, strMaxDiscountLimitOverrideUser);   //Sprint-18 - 2039 12-Jan-2015 JY Added to preserve active coupon value    //PRIMEPOS-2402 12-Jul-2021 JY Added strOverrideMaxStationCloseCashLimit, strMaxTransactionAmountUser, strMaxReturnTransactionAmountUser, strInvDiscOverrideUser, strMaxDiscountLimitOverrideUser
                ChangeDue = ChagngeDue;//Added by shitaljit on 13 Sept 2011
                frmPOSChangeDue oChagneDue = new frmPOSChangeDue(oPOSTrans.oTransHRow, ChangeDue, null, null);   //PRIMEPOS-2384 29-Oct-2018 JY pass blank object // PRIMEPOS-2747 -StoreCredit - NileshJ - Pass Null - 08-Nov-2019
                try
                {
                    RxLabel oRxLabel = oPOSTrans.GetRXLabelForRecvOnAcct(isCLTierreached, CLCouponValue);
                    if (Convert.ToDouble(oPOSTrans.oTransHData.Tables[0].Rows[0]["TotalPaid"]) != 00.00 || Configuration.CInfo.OpenDrawerForZeroAmtTrans == true)
                    {
                        oPOSTrans.OpenDrawerOnTransComplete(oRxLabel);
                    }
                    PrintReceipt(oRxLabel);
                }
                catch { }
                // not to show change due screeen in case of change due amount is 0.00
                this.txtAmtChangeDue.Text = this.ChangeDue.ToString();//Added by shitaljit on 13 Sept 2011
                if (Convert.ToDecimal(this.txtAmtChangeDue.Text) != 0.0M)
                {
                    oChagneDue.ShowDialog();
                }
                SetNew(true);
                logger.Trace("SaveReceiveOnAccount() - " + clsPOSDBConstants.Log_Exiting + "Amount :" + Amount.ToString(Configuration.CInfo.CurrencySymbol.ToString() + " #####0.00"));
            }
            catch (POSExceptions exp)
            {
                logger.Fatal(exp, "SaveReceiveOnAccount()");
                clsUIHelper.ShowErrorMsg(exp.ErrMessage);
                switch (exp.ErrNumber)
                {
                    case (long)POSErrorENUM.TransHeader_CustomerIDCanNotNull:
                        this.txtCustomer.Focus();
                        break;
                    case (long)POSErrorENUM.TransHeader_DateIsInvalid:
                        this.dtpTransDate.Focus();
                        break;
                }
            }
            catch (Exception exp)
            {
                logger.Fatal(exp, "SaveReceiveOnAccount()");
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }

        //completed by sandeep
        private void setTransHeaderRow(decimal amount, decimal amountTend, long accountNo, int retTransID)
        {
            oPOSTrans.oTransHRow = oPOSTrans.oTransHData.TransHeader[0];
            oPOSTrans.oTransHRow.CustomerID = Convert.ToInt32(this.txtCustomer.Tag);
            oPOSTrans.oTransHRow.TransDate = (System.DateTime)this.dtpTransDate.Value;
            oPOSTrans.oTransHRow.TransType = Convert.ToInt32(POS_Core.TransType.POSTransactionType.ReceiveOnAccount);
            oPOSTrans.oTransHRow.GrossTotal = Convert.ToDecimal(0);
            oPOSTrans.oTransHRow.TotalTaxAmount = Convert.ToDecimal(0);
            oPOSTrans.oTransHRow.TotalDiscAmount = Convert.ToDecimal(0);
            oPOSTrans.oTransHRow.TotalPaid = amount;
            oPOSTrans.oTransHRow.TenderedAmount = amountTend;
            oPOSTrans.oTransHRow.stClosedID = 0;
            oPOSTrans.oTransHRow.isStationClosed = 0;
            oPOSTrans.oTransHRow.isEOD = 0;
            oPOSTrans.oTransHRow.EODID = 0;
            oPOSTrans.oTransHRow.Acc_No = accountNo;
            oPOSTrans.oTransHRow.DrawerNo = Configuration.DrawNo;
            oPOSTrans.oTransHRow.StationID = Configuration.StationID;
            oPOSTrans.oTransHRow.TransDate = DateTime.Now;
            oPOSTrans.oTransHRow.ReturnTransID = retTransID;
        }

        private void ShowPrimeRXPOSNotes(string sRxNo, string sPatNo)
        {
            logger.Trace("ShowPrimeRXPOSNotes(string sRxNo, string sPatNo) - " + clsPOSDBConstants.Log_Entering);
            try
            {
                #region Rx Notes
                if (Configuration.CPOSSet.ShowRxNotes == true)  //PRIMEPOS-2459 03-Apr-2019 JY Added
                {
                    DataTable dtRxNotes = oPOSTrans.GetRxNotes(sRxNo);
                    if (dtRxNotes != null)
                    {
                        if (dtRxNotes.Rows.Count > 0) // Add by Manoj 2/21/2013 only if there is a note
                        {
                            frmCustomerNotesView ofrmCustomerNotesView = new frmCustomerNotesView(sRxNo, clsEntityType.RXNote);
                            ofrmCustomerNotesView.Initialize(dtRxNotes);
                            ofrmCustomerNotesView.ShowDialog();
                        }
                    }
                }
                #endregion

                #region Patient Notes
                if (Configuration.CPOSSet.ShowPatientNotes == true)  //PRIMEPOS-2459 03-Apr-2019 JY Added
                {
                    DataTable dtPatNote = oPOSTrans.GetPatientNotes(sPatNo);
                    if (dtPatNote != null)
                    {
                        if (dtPatNote.Rows.Count > 0) // Add by Manoj 2/21/2013 only display if there is a note
                        {
                            frmCustomerNotesView ofrmCustomerNotesView = new frmCustomerNotesView(sPatNo, clsEntityType.PatNote);
                            ofrmCustomerNotesView.Initialize(dtPatNote);
                            ofrmCustomerNotesView.ShowDialog();
                        }
                    }
                }
                #endregion
            }
            catch (Exception Ex)
            {
                logger.Fatal(Ex, "ShowPrimeRXPOSNotes(string sRxNo, string sPatNo)");
                clsUIHelper.ShowErrorMsg(Ex.Message);
            }
            logger.Trace("ShowPrimeRXPOSNotes(string sRxNo, string sPatNo) - " + clsPOSDBConstants.Log_Exiting);
        }

        #region PRIMEPOS-2536 14-May-2019 JY Added
        private void ShowPrimeRXPOSNotes(string sRxNo, ref ArrayList arrPatients)
        {
            logger.Trace("ShowPrimeRXPOSNotes(string sRxNo) - " + clsPOSDBConstants.Log_Entering);
            try
            {
                #region Rx Notes
                if (Configuration.CPOSSet.ShowRxNotes == true)
                {
                    DataTable dtRxNotes = oPOSTrans.GetRxNotes(sRxNo);
                    if (dtRxNotes != null)
                    {
                        if (dtRxNotes.Rows.Count > 0)
                        {
                            frmCustomerNotesView ofrmCustomerNotesView = new frmCustomerNotesView(sRxNo, clsEntityType.RXNote);
                            ofrmCustomerNotesView.Initialize(dtRxNotes);
                            ofrmCustomerNotesView.ShowDialog();
                        }
                    }
                }
                #endregion

                #region Patient Notes                                
                if (Configuration.CPOSSet.ShowPatientNotes == true)
                {
                    PharmBL oPharmBL = new PharmBL();
                    DataTable dtPat = oPharmBL.GetPatientByRxNo(sRxNo); //get rx patient                
                    if (dtPat != null && dtPat.Rows.Count > 0)
                    {
                        string sPatNo = string.Empty;
                        sPatNo = Configuration.convertNullToString(dtPat.Rows[0]["PATIENTNO"]);
                        if (!arrPatients.Contains(sPatNo))
                        {
                            arrPatients.Add(sPatNo);

                            DataTable dtPatNote = oPOSTrans.GetPatientNotes(sPatNo);
                            if (dtPatNote != null)
                            {
                                if (dtPatNote.Rows.Count > 0)
                                {
                                    frmCustomerNotesView ofrmCustomerNotesView = new frmCustomerNotesView(sPatNo, clsEntityType.PatNote);
                                    ofrmCustomerNotesView.Initialize(dtPatNote);
                                    ofrmCustomerNotesView.ShowDialog();
                                }
                            }
                        }
                    }
                }
                #endregion
            }
            catch (Exception Ex)
            {
                logger.Fatal(Ex, "ShowPrimeRXPOSNotes(string sRxNo)");
                clsUIHelper.ShowErrorMsg(Ex.Message);
            }
            logger.Trace("ShowPrimeRXPOSNotes(string sRxNo) - " + clsPOSDBConstants.Log_Exiting);
        }
        #endregion

        private void ShowItemOnPoleDisp(TransDetailRow oRow)
        {
            try
            {
                logger.Trace("ShowItemOnPoleDisp() - " + clsPOSDBConstants.Log_Entering);
                frmMain.PoleDisplay.ClearPoleDisplay();

                int LineLen = Configuration.CPOSSet.PD_LINELEN;
                //frmMain.PoleDisplay.WriteToPoleDisplay(oTDRow.ItemDescription.Substring(1,Configuration.CPOSSet.PD_LINELEN-7));
                //+ 								clsUIHelper.Spaces(Configuration.CPOSSet.PD_LINELEN-oTDRow.ItemDescription.Length-7) + " " + oTDRow.ExtendedPrice.ToString("###.00"));

                //Remove by Manoj
                // frmMain.PoleDisplay.ClearPoleDisplay();

                string strDescption = "";
                if (Configuration.CPOSSet.PrintRXDescription == false && oRow.ItemID == "RX")
                {
                    strDescption = oRow.ItemDescription.Substring(0, oRow.ItemDescription.IndexOf("-"));
                }
                else
                {
                    strDescption = oRow.ItemDescription;
                }

                if (LineLen > strDescption.Length)
                    frmMain.PoleDisplay.WriteToPoleDisplay(strDescption
                        + clsUIHelper.Spaces(Configuration.CPOSSet.PD_LINELEN - strDescption.Length - 7) + " " + oRow.ExtendedPrice.ToString(Configuration.CInfo.CurrencySymbol + "##0.00"));
                else
                    frmMain.PoleDisplay.WriteToPoleDisplay(strDescption.Substring(0, LineLen - 7)
                        + " " + oRow.ExtendedPrice.ToString(Configuration.CInfo.CurrencySymbol + "##0.00"));

                logger.Trace("ShowItemOnPoleDisp() - " + clsPOSDBConstants.Log_Exiting);
            }
            catch (Exception Ex)
            {
                logger.Fatal(Ex, "ShowItemOnPoleDisp()");
            }
        }

        #endregion

        #region Qty
        private void ValidateItemQty(object sender, System.ComponentModel.CancelEventArgs e)
        {
            ValidateItemQty(sender, e, true);
        }
        private void ValidateItemQty(object sender, System.ComponentModel.CancelEventArgs e, bool bApplyDiscount = true)
        {
            try
            {
                #region qty

                int oldQty;
                if (oPOSTrans.oTDRow == null)
                    return;

                oPOSTrans.ValidateItemQTY(txtQty.Value.ToString(), isItemInfoChanged, isQtyChange, isEditRow, out oldQty, ref GroupPrice, bApplyDiscount);  //PRIMEPOS-2514 09-May-2018 JY Added bApplyDiscount parameter

                string sTaxCodes = string.Empty;

                if (oPOSTrans.IsItemTaxableForTrasaction(oPOSTrans.oTDTaxData, oPOSTrans.oTDRow.ItemID, out sTaxCodes, oPOSTrans.oTDRow.TransDetailID) == true)
                {
                    EditTax(oPOSTrans.oTDRow.TaxCode, clsPOSDBConstants.TaxCodes_tbl);
                }

                if (GroupPrice != -1)
                {
                    txtExtAmount.Text = Convert.ToString(GroupPrice);
                    oPOSTrans.SetRowTrans(oPOSTrans.oTDRow, false);
                    oPOSTrans.oTDRow.ExtendedPrice = GroupPrice;
                    oPOSTrans.oTDRow.Price = oPOSTrans.oTDRow.ExtendedPrice / oPOSTrans.oTDRow.QTY;
                    sTaxCodes = string.Empty;
                    if (oPOSTrans.IsItemTaxableForTrasaction(oPOSTrans.oTDTaxData, oPOSTrans.oTDRow.ItemID, out sTaxCodes, oPOSTrans.oTDRow.TransDetailID) == true)
                    {
                        EditTax(oPOSTrans.oTDRow.TaxCode, clsPOSDBConstants.TaxCodes_tbl);
                    }
                    oPOSTrans.SetRowTrans(oPOSTrans.oTDRow);
                }
                #region PRIMEPOS-3098 20-Jun-2022 JY Added
                else if (oPOSTrans.oTDRow.ItemGroupPrice == true)
                {
                    oPOSTrans.oTDRow.ItemGroupPrice = false;
                    oPOSTrans.oTDRow.Price = oPOSTrans.oTDRow.OrignalPrice;
                    txtExtAmount.Text = Convert.ToString(oPOSTrans.oTDRow.Price * oPOSTrans.oTDRow.QTY);
                    if (bApplyDiscount)
                        oPOSTrans.oTDRow.Discount = oPOSTrans.CalculateDiscount(oPOSTrans.oTDRow.ItemID, oPOSTrans.oTDRow.QTY, oPOSTrans.oTDRow.Price);
                    oPOSTrans.SetRowTrans(oPOSTrans.oTDRow, false);
                    oPOSTrans.oTDRow.ExtendedPrice = oPOSTrans.oTDRow.Price * oPOSTrans.oTDRow.QTY;
                    sTaxCodes = string.Empty;
                    if (oPOSTrans.IsItemTaxableForTrasaction(oPOSTrans.oTDTaxData, oPOSTrans.oTDRow.ItemID, out sTaxCodes, oPOSTrans.oTDRow.TransDetailID) == true)
                    {
                        EditTax(oPOSTrans.oTDRow.TaxCode, clsPOSDBConstants.TaxCodes_tbl);
                    }
                    oPOSTrans.SetRowTrans(oPOSTrans.oTDRow);
                }
                #endregion

                if (this.txtDescription.Enabled == false && this.isAddRow == true)
                {
                    ItemData oIData = oPOSTrans.PopulateItem(oPOSTrans.oTDRow.ItemID.ToString());
                    if (oIData.Item.Rows.Count > 0)
                    {
                        if (oIData.Item[0].Itemtype.ToString().Trim() == "2" && oPOSTrans.skipMoveNext == false && Configuration.CInfo.ConsiderItemType == true)  //Sprint-22 17-Dec-2015 JY Added condition "Configuration.CInfo.ConsiderItemType"
                        {
                            txtUnitPrice.Focus();
                        }
                        else
                        {
                            this.ValidateRow(sender, e);
                        }
                    }
                }
                else if (this.isEditRow == true)
                {
                    if (oPOSTrans.oTDRow.Discount > oPOSTrans.oTDRow.ExtendedPrice + oPOSTrans.oTDRow.TaxAmount)
                    {
                        if (oPOSTrans.CurrentTransactionType == POS_Core.TransType.POSTransactionType.Sales)
                            oPOSTrans.oTDRow.Discount = 0;
                    }

                    this.ValidateRow(sender, e);
                    if (oPOSTrans.oTDRow.QTY - oldQty > 0)
                    {
                        CheckCompanionItems(oPOSTrans.oTDRow.ItemID, oPOSTrans.oTDRow.QTY - oldQty);
                    }
                    this.ClearItemRow();
                    this.grdDetail.Focus();
                }

                #endregion qty

            }
            catch (Exception Ex)
            {
                logger.Fatal(Ex, "ValidateItemQty()");
                clsUIHelper.ShowErrorMsg(Ex.Message);
            }
        }

        #endregion

        #region UnitPrice

        private void ValidateItemUnitPrice(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                logger.Trace("ValidateItemUnitPrice() - " + clsPOSDBConstants.Log_Entering);
                if (this.txtUnitPrice.ContainsFocus == true || this.txtItemCode.ContainsFocus == true)
                {
                    #region unitprice

                    if (oPOSTrans.oTDRow == null)
                    {
                        return;
                    }
                    if (isAddRow == false)
                    {
                        if (oPOSTrans.oTDRow.Price != Convert.ToDecimal(this.txtUnitPrice.Value))
                        {
                            if (oPOSTrans.CurrentTransactionType == POS_Core.TransType.POSTransactionType.Sales)
                            {
                                TransDetailRow oTDRowCopy = (TransDetailRow)oPOSTrans.oTransDData.TransDetail.NewRow();

                                oPOSTrans.oTDRow.Copy(oTDRowCopy);
                                oTDRowCopy.NonComboUnitPrice = oTDRowCopy.Price = Convert.ToDecimal(this.txtUnitPrice.Value);
                                oPOSTrans.oTDRow.IsOnSale = false;   //PRIMEPOS-2907 13-Oct-2020 JY Added
                                oPOSTrans.CalcExtdPrice(oTDRowCopy);
                                if ((oTDRowCopy.ExtendedPrice + oTDRowCopy.TaxAmount - oTDRowCopy.Discount) < 0)
                                {
                                    //throw (new Exception("Changing Item Price Will Result In Negative Value; This Change Is Not Allowed."));
                                    #region PRIMEPOS-2586 24-Sep-2018 JY Added
                                    var ex = new Exception(string.Format("{0} - {1}", Configuration.sNegativeItemPrice, Configuration.iNegativeItemPrice));
                                    ex.Data.Add(Configuration.iNegativeItemPrice, Configuration.sNegativeItemPrice);
                                    throw ex;
                                    #endregion                                    
                                }
                            }
                            oPOSTrans.oTDRow.IsPriceChanged = true;
                        }
                        else
                        {
                            oPOSTrans.oTDRow.IsPriceChangedByOverride = false;    //Sprint-26 - PRIMEPOS-2294 27-Jul-2017 JY Added
                        }
                    }
                    //Following Code added by Krishna on 7 October 2011
                    if (oPOSTrans.oTDRow.ItemID.Contains("RX"))
                    {
                        oPOSTrans.oTDRow.ItemCost = oPOSTrans.oTDRow.ExtendedPrice;
                    }
                    //Till here added by Krishna on 7 October 2011

                    //if (Configuration.CInfo.PromptForSellingPriceLessThanCost)    //Sprint-21 - PRIMEPOS-2168 01-Feb-2016 JY Commented
                    //{
                    Item oItem1 = new Item();
                    ItemData oItemData1;
                    ItemRow oItemRow1 = null;
                    oItemData1 = oItem1.Populate(oPOSTrans.oTDRow.ItemID);
                    decimal SalePrice = Convert.ToDecimal(this.txtUnitPrice.Value);

                    if (oItemData1.Item.Rows.Count > 0)
                    {
                        oItemRow1 = oItemData1.Item[0];
                        oPOSTrans.IsItemExist = true;
                        //if (oItemRow.LastCostPrice > SalePrice && Configuration.CInfo.PromptForSellingPriceLessThanCost)  //Sprint-21 - PRIMEPOS-2168 01-Feb-2016 JY Commented
                        if (oItemRow1.LastCostPrice > SalePrice) //Sprint-21 - PRIMEPOS-2168 01-Feb-2016 JY Added 
                        {
                            if (isPromptForSellingPriceLessThanCost(oPOSTrans.oTDRow.ItemID, SalePrice))
                            {
                                this.txtUnitPrice.Focus();
                                return;
                            }
                        }
                    }
                    //}

                    oPOSTrans.oTDRow.NonComboUnitPrice = oPOSTrans.oTDRow.Price = Convert.ToDecimal(this.txtUnitPrice.Value);
                    oPOSTrans.CalcExtdPrice(oPOSTrans.oTDRow);

                    #region Recalculate discount

                    oPOSTrans.oTDRow.Discount = oPOSTrans.RecalculateDiscount(oPOSTrans.oTDRow);

                    #endregion Recalculate discount

                    if (isAddRow == false)
                    {
                        if (UserPriviliges.IsUserHasPriviliges(UserPriviliges.Modules.InventoryMgmt.ID, UserPriviliges.Screens.ItemFile.ID, -998))
                        {
                            //PRIMEPOS-2602 28-Jan-2019 JY Added condition to control item price update behavior in item file with price override
                            if (Configuration.CInfo.PromptForItemPriceUpdate == true && Resources.Message.Display("Do You Want To Update Price In Item File.", "Price Override", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                            {
                                try
                                {
                                    Item oItem = new Item();
                                    ItemData odata = oItem.Populate(oPOSTrans.oTDRow.ItemID.Trim());
                                    odata.Item[0].SellingPrice = oPOSTrans.oTDRow.Price;
                                    ItemPriceValidation oItemPriceValid = new ItemPriceValidation();

                                    if (oItemPriceValid.ValidateItem(odata.Item[0], odata.Item[0].SellingPrice) == false)
                                    {
                                        clsUIHelper.ShowErrorMsg("Current values in item conflicts with validation settings.");
                                    }
                                    else
                                    {
                                        Configuration.UpdatedBy = "M";  //Added By Amit Date 10 May 2011
                                        oItem.Persist(odata);
                                    }
                                }
                                catch (Exception) { }
                            }
                            else // AuditTrail - Nileshj - PRIMEPOS-2808
                            {
                                if (oAuditTrail.oAuditDataSet.Tables[0].Rows.Count > 0)
                                {
                                    for (int i = oAuditTrail.oAuditDataSet.Tables[0].Rows.Count - 1; i >= 0; i--)
                                    {
                                        DataRow dr = oAuditTrail.oAuditDataSet.Tables[0].Rows[i];
                                        if (dr["EntityKey"] == oPOSTrans.oTDRow.ItemDescription.Trim() && dr["EntityName"] == "Item" && dr["FieldChanged"] == "SellingPrice")
                                        {
                                            dr.Delete();
                                        }
                                    }
                                    oAuditTrail.oAuditDataSet.Tables[0].AcceptChanges();
                                }

                                Item oItem = new Item();
                                ItemData odata = oItem.Populate(oPOSTrans.oTDRow.ItemID.Trim());
                                DataTable dtItemTable = new DataTable();
                                dtItemTable = oAuditTrail.CreateAuditLogDatatable();
                                DataRow row = dtItemTable.NewRow();
                                row["EntityName"] = "Item";
                                row["EntityKey"] = oPOSTrans.oTDRow.ItemDescription;
                                row["FieldChanged"] = "SellingPrice";
                                row["OldValue"] = odata.Tables[0].Rows[0]["SellingPrice"].ToString();
                                row["NewValue"] = oPOSTrans.oTDRow.Price;
                                row["DateChanged"] = DateTime.Now;
                                row["ActionBy"] = Configuration.UserName;
                                row["Operation"] = "I";
                                row["ApplicationName"] = "PrimePOS";

                                dtItemTable.Rows.Add(row);
                                oAuditTrail.oAuditDataSet.Tables[0].Merge(dtItemTable);
                            }
                        }
                    }
                    if (this.txtDescription.Enabled == false && this.isAddRow == true)
                    {
                        this.ValidateRow(sender, e);
                    }
                    else if (this.isEditRow == true)
                    {
                        //if(string.IsNullOrEmpty(oTDRow.TaxCode.Trim()) == false)
                        string sTaxCode = string.Empty;
                        if (oPOSTrans.IsItemTaxableForTrasaction(oPOSTrans.oTDTaxData, oPOSTrans.oTDRow.ItemID, out sTaxCode, oPOSTrans.oTDRow.TransDetailID) == true)
                        {
                            //FKEdit(oTDRow.TaxCode, clsPOSDBConstants.TaxCodes_tbl, false);
                            #region PRIMEPOS-2924 11-Dec-2020 JY Added if condition
                            try
                            {
                                if (oPOSTrans.oTDRow.ItemID.Trim().ToUpper() == "RX")
                                {
                                    TaxCodesData oTaxCodesData = new TaxCodesData();
                                    LoadTaxCodes(oPOSTrans.oTDRow.ItemID, out oTaxCodesData);
                                    if (oTaxCodesData != null && oTaxCodesData.Tables.Count > 0 && oTaxCodesData.TaxCodes.Rows.Count > 0)
                                    {
                                        if (Configuration.convertNullToString(oTaxCodesData.TaxCodes.Rows[0][clsPOSDBConstants.TaxCodes_Fld_TaxCode]).Trim().ToUpper() == clsPOSDBConstants.TaxCodes_Fld_RxTax.ToUpper())
                                        {
                                            oPOSTrans.oTDRow.TaxAmount = 0;
                                            oPOSTrans.oTDRow.TaxCode = "";
                                            oPOSTrans.UpdateTransTaxDetails(oPOSTrans.oTDTaxData, oPOSTrans.oTDRow.TransDetailID);
                                        }
                                        else
                                            EditTax(oPOSTrans.oTDRow.TaxCode, clsPOSDBConstants.TaxCodes_tbl);
                                    }
                                }
                                #endregion
                                else
                                    EditTax(oPOSTrans.oTDRow.TaxCode, clsPOSDBConstants.TaxCodes_tbl);
                            }
                            catch (Exception Ex)
                            {
                                logger.Fatal(Ex, "ValidateItemUnitPrice()");
                            }
                        }
                        this.ValidateRow(sender, e);
                        this.ClearItemRow();
                        this.grdDetail.Focus();
                    }
                    #endregion unitprice
                }
                logger.Trace("ValidateItemUnitPrice() - " + clsPOSDBConstants.Log_Exiting);
            }
            catch (Exception Ex)
            {
                #region PRIMEPOS-2586 24-Sep-2018 JY Added
                string statusMessage = string.Empty;
                bool bLogException = true;

                if (Ex.Data.Count > 0)
                {
                    foreach (DictionaryEntry de in Ex.Data)
                    {
                        if (de.Key.ToString() == Configuration.iNegativeItemPrice.ToString())
                        {
                            statusMessage = Ex.Data[de.Key].ToString();
                            bLogException = false;
                            break;
                        }
                    }
                }
                else //PRIMEPOS-2844 13-May-2020 JY Added
                {
                    try
                    {
                        if (((POSExceptions)Ex).ErrNumber.ToString() == Configuration.iNegativeItemPrice.ToString())
                        {
                            statusMessage = ((POSExceptions)Ex).ErrMessage;
                            bLogException = false;
                        }
                    }
                    catch { }
                }

                if (bLogException == false)
                {
                    clsUIHelper.ShowErrorMsg(statusMessage);
                }
                else
                {
                    logger.Fatal(Ex, "ValidateItemUnitPrice() : " + this.txtUnitPrice.Value);
                    clsUIHelper.ShowErrorMsg(Ex.Message);
                }
                #endregion                
            }
        }

        //completed by sandeep
        private void txtUnitPrice_ValueChanged(object sender, EventArgs e)
        {
            if (oPOSTrans.oTDRow != null)
            {
                if (oPOSTrans.oTDRow.Price != Convert.ToDecimal(this.txtUnitPrice.Value) && oPOSTrans.IsItemExist == true)
                {
                    isAddRow = false;
                    isEditRow = true;
                }
            }
        }


        #endregion

        #region aggregateMethod

        //completed by sandeep
        private void ItemsOnEnter(object sender, CancelEventArgs e)
        {
            try
            {
                if (this.txtItemCode.Text.Trim() == "")
                {
                    this.txtItemCode.Focus();
                }
                if (this.grdDetail.Rows.Count > 0)
                {
                    this.changeStToolbars(TransactionStToolbars.strTBTerminalEntery);
                }
                else
                {
                    this.changeStToolbars(TransactionStToolbars.strTBTerminal);
                }
            }
            catch (Exception exp)
            {
                logger.Fatal(exp, "ItemsOnEnter()");
            }
        }
        //completed by sandeep
        private void AfterEnterEditMode(object sender, System.EventArgs e)
        {
            try
            {
                if (this.txtItemCode.Text.Trim() == "")
                {
                    this.txtItemCode.Focus();
                }
            }
            catch (Exception) { }
        }
        //completed by sandeep
        private void Items_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            try
            {
                if (this.Cursor == Cursors.WaitCursor)
                    return;

                if (e.KeyData == Keys.Escape)
                {
                    if (isAddRow == true)
                    {
                        isAddRow = false;
                        isEditRow = false;
                        this.ClearItemRow();
                        this.txtItemCode.Focus();
                    }
                    else if (isEditRow == true)
                    {
                        isAddRow = false;
                        isEditRow = false;
                        this.ClearItemRow();
                        this.grdDetail.Focus();
                    }
                    e.Handled = false;
                }
                else if (e.KeyData == Keys.Enter && this.txtItemCode.Text.Trim() == "")
                    this.txtItemCode.Focus();
            }
            catch (Exception) { }
        }

        #endregion

        #region gbOption

        //completed by sandeep
        private void btnPriceCheck_Click(object sender, EventArgs e)
        {
            this.tbTerminalEnteryActions("F3");
            SetFocusBack();
        }

        //completed by sandeep
        private void btnNoSale_Click(object sender, EventArgs e)
        {
            this.tbTerminalEnteryActions("Ctrl+N");
            SetFocusBack();
        }

        //completed by sandeep
        private void btnClose_Click_1(object sender, EventArgs e)
        {
            this.tbTerminalActions("Esc");
            #region PRIMEPOS-3207
            clearHyphenAlert();
            #endregion
            SetFocusBack();
        }
        //completed by sandeep
        private void btnLockRegister_Click(object sender, EventArgs e)
        {
            this.tbTerminalEnteryActions("Ctrl+L");
            SetFocusBack();
        }

        //completed by sandeep
        private void btnCLControlPane_Click(object sender, EventArgs e)
        {
            if (this.grdDetail.Rows.Count > 0)
            {
                ShowCLCardInputScreen(true);
            }
            else
            {
                frmPOSTransCLCardInput ofrm = new frmPOSTransCLCardInput(true);
                ofrm.ShowDialog();
            }
            this.txtItemCode.Focus();//Added By Shitaljit on 2/6/2014 to bring focus back to ItemCode after closing the form
        }

        //completed by sandeep
        private void btnMarkAllRx_Click(object sender, EventArgs e)
        {
            if (Resources.Message.Display("Are you sure to override all Rx to $0?", "Override All Rx to $0", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
            {
                try
                {
                    string sUserID = string.Empty;
                    bool isEditable = UserPriviliges.getPermission(UserPriviliges.Modules.POSTransaction.ID, UserPriviliges.Screens.POSTransaction.ID, UserPriviliges.Permissions.PriceOverrideForRXItemsfromPOSTrans.ID, UserPriviliges.Permissions.PriceOverrideForRXItemsfromPOSTrans.Name, out sUserID);
                    if (isEditable == false)
                        return;

                    for (int i = 0; i < grdDetail.Rows.Count; i++)
                    {
                        if ((grdDetail.Rows[i].Cells[clsPOSDBConstants.TransDetail_Fld_ItemID].Value.ToString() == "RX") && (Convert.ToDecimal(grdDetail.Rows[i].Cells[clsPOSDBConstants.TransDetail_Fld_ExtendedPrice].Value) != 0))
                        {
                            grdDetail.Rows[i].Cells[clsPOSDBConstants.TransDetail_Fld_ItemCost].Value = Configuration.convertNullToDecimal(grdDetail.Rows[i].Cells[clsPOSDBConstants.TransDetail_Fld_Price].Value); //PRIMEPOS-2745 16-Oct-2019 JY Added
                            grdDetail.Rows[i].Cells[clsPOSDBConstants.TransDetail_Fld_Price].Value = 0;
                            grdDetail.Rows[i].Cells[clsPOSDBConstants.TransDetail_Fld_ExtendedPrice].Value = 0;
                            grdDetail.Rows[i].Cells[clsPOSDBConstants.TransDetail_Fld_IsPriceChanged].Value = true; //PRIMEPOS-2745 02-Oct-2019 JY Added
                            grdDetail.Rows[i].Cells[clsPOSDBConstants.TransDetail_Fld_IsPriceChangedByOverride].Value = true;   //PRIMEPOS-2745 02-Oct-2019 JY Added
                            grdDetail.Rows[i].Cells[clsPOSDBConstants.TransDetail_Fld_TaxAmount].Value = 0; //PRIMEPOS-2924 03-Dec-2020 JY Added
                            grdDetail.Rows[i].Cells[clsPOSDBConstants.TaxCodes_Fld_TaxCode].Value = ""; //PRIMEPOS-2924 03-Dec-2020 JY Added

                            #region Fix For PRIMEPOS 2362 [Rohit Nair Feb 1 2017]  To Show Price Change in Signature PAD

                            TransDetailRow tempTDRow = oPOSTrans.oTransDData.TransDetail.FindRow(Convert.ToInt32(grdDetail.Rows[i].Cells[clsPOSDBConstants.TransDetail_Fld_TransDetailID].Value.ToString()));

                            if (tempTDRow != null)
                            {
                                tempTDRow.NonComboUnitPrice = 0.0M;
                                //tempTDRow.OrignalPrice = 0.0M;    //PRIMEPOS-2743 04-Nov-2019 JY Commented
                                tempTDRow.Price = 0.0M;
                                tempTDRow.ExtendedPrice = 0.0M;

                                int index = oPOSTrans.GetTransIndex(oPOSTrans.oTransDData, tempTDRow.TransDetailID);
                                if (SigPadUtil.DefaultInstance.IsVF)
                                {
                                    DeviceItemsProcess("UpdateItem", tempTDRow, index);
                                }
                                else if (SigPadUtil.DefaultInstance.isPAX) //20-Dec-2018 - Added for Override Rx - NileshJ
                                {
                                    if (i == 0)
                                    {
                                        SigPadUtil.DefaultInstance.PD.ClearItems();
                                    }
                                    SigPadUtil.DefaultInstance.AddItem(tempTDRow, index);
                                }
                                else if (SigPadUtil.DefaultInstance.isVantiv) //PRIMEPOS-2636 ADDED BY ARVIND 
                                {
                                    if (i == 0)
                                    {
                                        SigPadUtil.DefaultInstance.VFD.ClearItems();
                                    }
                                    SigPadUtil.DefaultInstance.AddItem(tempTDRow, index);
                                }
                                else
                                {
                                    SigPadUtil.DefaultInstance.UpdateItem(tempTDRow, index);
                                }
                            }
                            #endregion
                        }
                    }

                    #region PRIMEPOS-2924 03-Dec-2020 JY Added
                    try
                    {
                        if (oPOSTrans != null && oPOSTrans.oTDTaxData != null && oPOSTrans.oTDTaxData.Tables.Count > 0 && oPOSTrans.oTDTaxData.TransDetailTax.Rows.Count > 0)
                        {
                            for (int i = oPOSTrans.oTDTaxData.TransDetailTax.Rows.Count - 1; i >= 0; i--)
                            {
                                TransDetailTaxRow oTransDetailTaxRow = oPOSTrans.oTDTaxData.TransDetailTax[i];
                                if (oTransDetailTaxRow.ItemID.Trim().ToUpper() == "RX")
                                    oTransDetailTaxRow.Delete();
                            }
                            oPOSTrans.oTDTaxData.AcceptChanges();
                            oPOSTrans.oTDTaxData.TransDetailTax.AcceptChanges();
                        }
                    }
                    catch (Exception Ex)
                    {
                        logger.Fatal(Ex, "UpdateTransTaxDetails()");
                    }
                    #endregion

                    this.grdDetail.UpdateData();
                    this.grdDetail.Refresh();
                    Application.DoEvents();
                }
                catch (Exception ex)
                {
                    clsUIHelper.ShowErrorMsg(ex.Message);
                }
            }
        }


        #endregion

        #region totaltxtBox
        //completed by sandeep
        private void SummaryLabes_TextChaned(object sender, System.EventArgs e)
        {
            this.txtAmtSubTotal.Text = Configuration.convertNullToDecimal(this.txtAmtSubTotal.Text).ToString("######0.00");
            this.txtAmtBalanceDue.Text = Configuration.convertNullToDecimal(this.txtAmtBalanceDue.Text).ToString("######0.00");
            this.txtAmtChangeDue.Text = Configuration.convertNullToDecimal(this.txtAmtChangeDue.Text).ToString("######0.00");
            this.txtAmtDiscount.Text = Configuration.convertNullToDecimal(this.txtAmtDiscount.Text).ToString("######0.00");
            this.txtAmtTax.Text = Configuration.convertNullToDecimal(this.txtAmtTax.Text).ToString("######0.00");

            //if (Configuration.CSetting.EnableCustomerEngagement) //PRIMEPOS-2794
            //{
            //}
            // SAJID PRIMEPOS-2794
            //PRIMEPOS-2794 Arvind removed because of the labels not showing the amountwhen customer engagement was off
            lblSubTotal2.Text = txtAmtSubTotal.Text;
            lblTax2.Text = txtAmtTax.Text;
            lblTotalAmount2.Text = txtAmtTotal.Text;
            lblDiscount2.Text = txtAmtDiscount.Text;

            //if (this.txtAmtDiscount.Text != "0.00") {
            //    this.txtAmtDiscount.Appearance.BackColor = Color.Red;
            //} else if (this.txtAmtDiscount.Text == "0.00") {
            //    this.txtAmtDiscount.Appearance.BackColor = Color.LightCyan;
            //}
            ShowHideGridScroll();
        }
        //completed by sandeep
        private void txtAmtTotal_TextChanged(object sender, EventArgs e)
        {
            this.txtAmtTotal.Text = Configuration.convertNullToDecimal(this.txtAmtTotal.Text).ToString("######0.00");
            SummaryLabes_TextChaned(null, null);//Arvind added because of Total Amount mismatch
        }

        #endregion

        #region txtItemDescription

        //completed by sandeep
        private void txtItemDescription_Enter(object sender, EventArgs e)
        {
            clsUIHelper.AfterEnterEditMode(sender, e);
        }
        //completed by sandeep
        private void txtItemDescription_Leave(object sender, EventArgs e)
        {
            clsUIHelper.AfterExitEditMode(sender, e);
        }
        //completed by sandeep
        private void txtItemDescription_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter || e.KeyData == Keys.Tab)
            {
                this.txtItemDescription.Text = this.txtItemDescription.Text.Trim();
                this.txtQty.Enabled = true;
            }
        }
        //completed by sandeep
        private void setDescriptionView()
        {
            if (txtDescription.Visible)
            {
                tblDescription.ColumnStyles[1].SizeType = SizeType.Percent;
                tblDescription.ColumnStyles[1].Width = 100;
                tblDescription.ColumnStyles[0].SizeType = SizeType.Absolute;
                tblDescription.ColumnStyles[0].Width = 0;
            }
            else
            {
                tblDescription.ColumnStyles[1].SizeType = SizeType.Absolute;
                tblDescription.ColumnStyles[1].Width = 0;
                tblDescription.ColumnStyles[0].SizeType = SizeType.Percent;
                tblDescription.ColumnStyles[0].Width = 100;
            }

        }

        #endregion

        #region NumericPad

        //completed by sandeep
        public void btnNumericPad_Click(object sender, System.EventArgs e)
        {

            try
            {
                if (m_threadStarted)
                {
                    this.NumericPad.closeForm();
                    this.btnNumericPad.Text = "Show Num Pad";
                }
                else
                {
                    showNumPad();
                    clsMain.Mainform.Activate();
                    m_threadStarted = true;
                    this.btnNumericPad.Text = "Hide Num Pad";
                }
                this.txtItemCode.Focus();
            }
            catch (Exception exp)
            {
                logger.Fatal(exp, "btnNumericPad_Click()");
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }

        //completed by sandeep
        public void showNumPad()
        {
            try
            {
                frmNumPad = this.NumericPad;
                frmNumPad.TopLevel = false;
                frmNumPad.Dock = DockStyle.Fill;
                frmNumPad.Show();
                tblRight.Controls.Add(frmNumPad);

            }
            catch (Exception exp)
            {
                logger.Fatal(exp, "showNumPad()");
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }

        #endregion

        #region IngenicoLane Close Issue
        //PRIMEPOS-2534 (Suraj) 31-may-18 Suraj - Added VFInactivityMonitor for ingenico lane close issue 
        //PRIMEPOS-2534 (Suraj) 8-aug-18 Suraj - Added PinPadPortNo to support CDC With WorldPay 
        private void StartVFInactivityMonitor(object form)
        {
            if (SigPadUtil.DefaultInstance.IsVF && Configuration.CPOSSet.PaymentProcessor == "WORLDPAY" && Configuration.CPOSSet.PinPadPortNo == "0")  //Added WorldPay Condition 22-6-2018 //Added PinPadPortNo check For USBCDC to 8-8-2018
            {
                logger.Trace("POSTransaction VF Device Monitor Init start");
                VFInactivityTrackingTimer = new System.Timers.Timer(300000);
                VFInactivityTrackingTimer.Elapsed += delegate
                {
                    VFDeviceHeartBeat(this, null, form);
                };
                VFInactivityTrackingTimer.Enabled = true;
                logger.Trace("POSTransaction VF Device Monitor Enabled " + VFInactivityTrackingTimer.Enabled);
                logger.Trace("POSTransaction VF Device Monitor Init End");
            }
        }


        private void StopVFInactivityMonitor()
        {
            if (VFInactivityTrackingTimer != null && SigPadUtil.DefaultInstance.IsVF)
            {
                VFInactivityTrackingTimer.Enabled = false;
                VFInactivityTrackingTimer.Stop();
                logger.Trace("POSTransaction VF Device Monitor  Enabled " + VFInactivityTrackingTimer.Enabled);
                VFInactivityTrackingTimer = null;
            }

        }

        void VFDeviceHeartBeat(object source, System.Timers.ElapsedEventArgs e, object form)
        {
            if (this.grdDetail.Rows.Count == 0)
            {
                logger.Trace("POSTransaction VF Device Monitor Trigger start row count in grid is 0");
                SigPadUtil.DefaultInstance.VF.ReInitializeISC(Convert.ToInt32(Configuration.CPOSSet.PinPadPortNo));
                logger.Trace("POSTransaction VF Device Monitor Trigger start ");
            }
            else if (this.grdDetail.Rows.Count != 0 && form.GetType() == typeof(frmPOSChangeDue))
            {
                logger.Trace("POSTransaction VF Device Monitor Trigger start");
                SigPadUtil.DefaultInstance.VF.ReInitializeISC(Convert.ToInt32(Configuration.CPOSSet.PinPadPortNo));
            }
            else
            {
                logger.Trace("POSTransaction VF Device Monitor Trigger Stop row count >0");
                StopVFInactivityMonitor();
            }
        }
        //
        #endregion
        #region BatchDelivery - PRIMERX-7688 - NileshJ 23-Sept-2019
        private void btnBatchDel_Click(object sender, EventArgs e)
        {
            this.tbTerminalActions("Esc");
            SetFocusBack();
            //this.Close();
            frmReconciliationDeliveryReport objfrmReconciliationDeliveryReport = new frmReconciliationDeliveryReport(this);
            objfrmReconciliationDeliveryReport.ShowDialog();
        }
        #endregion
        private void btnEBTBalance_Click(object sender, EventArgs e)
        {
            #region PRIMEPOS-2786 EVERTEC
            if (Configuration.CPOSSet.PaymentProcessor.ToUpper() == "EVERTEC")
            {
                string ipAddress = Configuration.CPOSSet.SigPadHostAddr.Split(':')[0];
                int portNo = Convert.ToInt32(Configuration.CPOSSet.SigPadHostAddr.Split('/')[0].Split(':')[1]);
                EvertechProcessor evertecProcessor = EvertechProcessor.getInstance(ipAddress, portNo);

                evertecResponse = evertecProcessor.GetEBTBalance();

                if (!String.IsNullOrWhiteSpace(evertecResponse.foodBalance) && !String.IsNullOrWhiteSpace(evertecResponse.cashBalance))
                {
                    if (Resources.Message.Display("The EBT FoodBalance is = " + evertecResponse.foodBalance + "\n The EBT CashBalance is = " + evertecResponse.cashBalance + "\n Do you want to print the receipt? ", " EBT Balance ", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        RxLabel oRxlabel = new RxLabel(null, null, null, ReceiptType.Void, null);
                        oRxlabel.isEBTBalance = true;
                        oRxlabel.FoodBalance = evertecResponse.foodBalance;
                        oRxlabel.CashBalance = evertecResponse.cashBalance;
                        #region PRIMEPOS-2833
                        oRxlabel.AuthNo = evertecResponse.AuthNo;
                        oRxlabel.ResultDescription = evertecResponse.ResultDescription;
                        oRxlabel.Amount = evertecResponse.AmountApproved.ToString();
                        oRxlabel.EvertecDenialDate = DateTime.Now;
                        if (!String.IsNullOrWhiteSpace(evertecResponse.EmvReceipt?.ResponseCode))
                        {
                            oRxlabel.ResultDescription = evertecResponse.EmvReceipt?.ResponseCode;
                        }
                        oRxlabel.PaymentProcessor = Configuration.CPOSSet.PaymentProcessor.ToUpper();
                        if (evertecResponse?.EmvReceipt != null)
                        {
                            oRxlabel.MerchantID = evertecResponse.EmvReceipt.MerchantID.Trim();
                            oRxlabel.InvoiceNumber = evertecResponse.EmvReceipt.InvoiceNumber;
                            oRxlabel.TerminalID = evertecResponse.EmvReceipt.TerminalID;
                            oRxlabel.Batch = evertecResponse.EmvReceipt.BatchNumber;
                            oRxlabel.ReferenceNumber = evertecResponse.EmvReceipt.ReferenceNumber;
                            oRxlabel.Trace = evertecResponse.EmvReceipt.TraceNumber;
                        }
                        #endregion
                        oRxlabel.Print();
                    }
                }
            }
            #endregion
        }
        // SAJID PRIMEPOS-2794
        private void CustomerEngagementPaymentDetails_SelectedTabChanged(object sender, Infragistics.Win.UltraWinTabControl.SelectedTabChangedEventArgs e)
        {
            //if (txtCustomer.Text.Trim() != "-1")
                tbllHorizTotal.Visible = e.Tab.Text == "Customer Engagement";
        }
        #region PRIMEPOS-2669 08-Jul-2020 JY Added
        //private void lblTax_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        //{
        //    try
        //    {
        //        if (Configuration.convertNullToDecimal(this.txtAmtTax.Text) != 0)
        //        {
        //            if (oPOSTrans.oTDTaxData != null && oPOSTrans.oTDTaxData.TransDetailTax != null && oPOSTrans.oTDTaxData.TransDetailTax.Rows.Count > 0)
        //            {
        //                frmTaxBreakDown ofrmTaxBreakDown = new frmTaxBreakDown(oPOSTrans.oTDTaxData);
        //                ofrmTaxBreakDown.ShowDialog();
        //            }
        //        }
        //        else
        //        {
        //            Resources.Message.Display("cant show the tax breakdown as couldn't found taxable line items in a transaction.", Configuration.ApplicationName, MessageBoxButtons.OK, MessageBoxIcon.Information);
        //        }
        //    }
        //    catch (Exception Ex)
        //    {
        //        logger.Fatal(Ex, "lblTax_LinkClicked()");
        //    }
        //}

        private void lblTax_LinkClicked(object sender, EventArgs e)
        {
            try
            {
                if (Configuration.convertNullToDecimal(this.txtAmtTax.Text) != 0)
                {
                    if (oPOSTrans.oTDTaxData != null && oPOSTrans.oTDTaxData.TransDetailTax != null && oPOSTrans.oTDTaxData.TransDetailTax.Rows.Count > 0)
                    {
                        frmTaxBreakDown ofrmTaxBreakDown = new frmTaxBreakDown(oPOSTrans.oTDTaxData, FormDataOnScreen.TaxData); //PRIMEPOS-2651 08-Apr-2022 JY Modified
                        ofrmTaxBreakDown.ShowDialog();
                    }
                }
                else
                {
                    Resources.Message.Display("cant show the tax breakdown as couldn't found taxable line items in a transaction.", Configuration.ApplicationName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception Ex)
            {
                logger.Fatal(Ex, "lblTax_LinkClicked()");
            }
        }
        #endregion        

        private string CalculateEvertecTax()
        {
            if (Configuration.CPOSSet.PaymentProcessor.ToUpper() == "EVERTEC")
            {
                DataView view = new DataView(oPOSTrans.oTDTaxData.Tables[0]);
                DataTable distinctValues = view.ToTable(true, "TaxID", "ItemID", "TaxAmount", "TaxPercent");//PRIMEPOS-3099


                var lstEvertecTaxDetails = distinctValues.AsEnumerable().GroupBy(r1 => new
                {
                    TaxId = r1.Field<int>("TaxID"),
                    TaxPercent = r1.Field<decimal>("TaxPercent")
                }).Select(g => new
                {
                    TaxID = g.Key.TaxId,
                    TaxPercent = g.Key.TaxPercent,
                    TaxAmount = g.Sum(x => x.Field<decimal>("TaxAmount"))
                }).ToList();

                //decimal TaxAmount = 0;
                //string 


                //for(int i =0; i < oPOSTrans.oTDTaxData.Tables[0].Rows?.Count; i++)
                //{

                //}

                DataTable dtEvertecTaxDetail = new DataTable();
                dtEvertecTaxDetail.Columns.Add("TaxID");
                dtEvertecTaxDetail.Columns.Add("Description");
                dtEvertecTaxDetail.Columns.Add("TaxAmount");
                dtEvertecTaxDetail.Columns.Add("TotalAmount");
                dtEvertecTaxDetail.Columns.Add("TotalTaxAmount");
                dtEvertecTaxDetail.Columns.Add("TaxType");
                dtEvertecTaxDetail.Columns.Add("TaxCode");
                dtEvertecTaxDetail.Columns.Add("TaxPercent");//PRIMEPOS-3099
                DataSet oTaxCodeData = oPOSTrans.GetTaxCodeData();

                foreach (var taxDetail in lstEvertecTaxDetails)
                {
                    DataRow dr = dtEvertecTaxDetail.NewRow();
                    dr["TaxID"] = taxDetail.TaxID;
                    dr["TaxAmount"] = taxDetail.TaxAmount;
                    dr["Description"] = oTaxCodeData.Tables[0].AsEnumerable().Where(a => a.Field<int>("TaxID") == taxDetail.TaxID).Select(g => g.Field<string>("Description")).SingleOrDefault();
                    dr["TaxCode"] = oTaxCodeData.Tables[0].AsEnumerable().Where(a => a.Field<int>("TaxID") == taxDetail.TaxID).Select(g => g.Field<string>("TaxCode")).SingleOrDefault();
                    dr["TotalAmount"] = txtAmtTotal.Text;
                    dr["TotalTaxAmount"] = txtAmtTax.Text;
                    dr["TaxPercent"] = taxDetail.TaxPercent;
                    dr["TaxType"] = oTaxCodeData.Tables[0].AsEnumerable().Where(a => a.Field<int>("TaxID") == taxDetail.TaxID).Select(g => g.Field<int?>("TaxType")).SingleOrDefault();
                    dtEvertecTaxDetail.Rows.Add(dr);
                }
                StateTaxAmount = dtEvertecTaxDetail.AsEnumerable().Where(a => !string.IsNullOrWhiteSpace(a.Field<string>("Description")) && Convert.ToInt32(a.Field<string>("TaxType")) == (int)TaxTypes.State && a.Field<string>("TaxCode").ToUpper().Contains("PRS")).Select(s => s.Field<string>("TaxAmount")).SingleOrDefault();
                ReduceStateTaxAmount = dtEvertecTaxDetail.AsEnumerable().Where(a => !string.IsNullOrWhiteSpace(a.Field<string>("Description")) && Convert.ToInt32(a.Field<string>("TaxType")) == (int)TaxTypes.State && a.Field<string>("TaxCode").ToUpper().Contains("PRRS")).Select(s => s.Field<string>("TaxAmount")).SingleOrDefault();
                CityTaxAmount = dtEvertecTaxDetail.AsEnumerable().Where(a => !string.IsNullOrWhiteSpace(a.Field<string>("Description")) && Convert.ToInt32(a.Field<string>("TaxType")) == (int)TaxTypes.Municipality && a.Field<string>("TaxCode").ToUpper().Contains("PRM")).Select(s => s.Field<string>("TaxAmount")).SingleOrDefault();
                string reduceStateTaxPercent = dtEvertecTaxDetail.AsEnumerable().Where(a => !string.IsNullOrWhiteSpace(a.Field<string>("Description")) && Convert.ToInt32(a.Field<string>("TaxType")) == (int)TaxTypes.State && a.Field<string>("TaxCode").ToUpper().Contains("PRRS")).Select(s => s.Field<string>("TaxPercent")).SingleOrDefault();//PRIMEPOS-3099
                //EvertecTaxAmount = cityTaxAmount + "|" + stateTaxAmount;
                #region PRIMEPOS-3099
                if (Convert.ToDecimal(reduceStateTaxPercent) == 0)
                {
                    ReduceCityTaxAmount = "0.00";
                }
                else
                {
                    ReduceCityTaxAmount = Math.Round((Convert.ToDecimal(ReduceStateTaxAmount) * 100) / Convert.ToDecimal(reduceStateTaxPercent)).ToString();//PRIMEPOS-3099 
                }
                #endregion
                return Newtonsoft.Json.JsonConvert.SerializeObject(dtEvertecTaxDetail);
            }
            else
            {
                return string.Empty;
            }
        }

        private bool CalculateElavonTax(out string TotalTax)
        {
            if (Configuration.CPOSSet.PaymentProcessor.ToUpper() == "ELAVON")
            {
                TotalTax = txtAmtTax.Text;
                DataView view = new DataView(oPOSTrans.oTDTaxData.Tables[0]);
                DataTable distinctValues = view.ToTable(true, "TaxID", "ItemID", "TaxAmount");


                var lstEvertecTaxDetails = distinctValues.AsEnumerable().GroupBy(r1 => new
                {
                    TaxId = r1.Field<int>("TaxID")
                }).Select(g => new
                {
                    TaxID = g.Key.TaxId,
                    TaxAmount = g.Sum(x => x.Field<decimal>("TaxAmount"))
                }).ToList();

                if (lstEvertecTaxDetails.Count > 0)
                {
                    return true;
                }

                DataTable dtEvertecTaxDetail = new DataTable();
                dtEvertecTaxDetail.Columns.Add("TaxID");
                dtEvertecTaxDetail.Columns.Add("Description");
                dtEvertecTaxDetail.Columns.Add("TaxAmount");
                dtEvertecTaxDetail.Columns.Add("TotalAmount");
                dtEvertecTaxDetail.Columns.Add("TotalTaxAmount");
                dtEvertecTaxDetail.Columns.Add("TaxType");
                dtEvertecTaxDetail.Columns.Add("TaxCode");

                DataSet oTaxCodeData = oPOSTrans.GetTaxCodeData();

                foreach (var taxDetail in lstEvertecTaxDetails)
                {
                    DataRow dr = dtEvertecTaxDetail.NewRow();
                    dr["TaxID"] = taxDetail.TaxID;
                    dr["TaxAmount"] = taxDetail.TaxAmount;
                    dr["Description"] = oTaxCodeData.Tables[0].AsEnumerable().Where(a => a.Field<int>("TaxID") == taxDetail.TaxID).Select(g => g.Field<string>("Description")
                    ).SingleOrDefault();
                    dr["TaxCode"] = oTaxCodeData.Tables[0].AsEnumerable().Where(a => a.Field<int>("TaxID") == taxDetail.TaxID).Select(g => g.Field<string>("TaxCode")
                    ).SingleOrDefault();
                    dr["TotalAmount"] = txtAmtTotal.Text;
                    dr["TotalTaxAmount"] = txtAmtTax.Text;
                    dr["TaxType"] = oTaxCodeData.Tables[0].AsEnumerable().Where(a => a.Field<int>("TaxID") == taxDetail.TaxID).Select(g => g.Field<int?>("TaxType")
                    ).SingleOrDefault();
                    dtEvertecTaxDetail.Rows.Add(dr);
                }
                StateTaxAmount = dtEvertecTaxDetail.AsEnumerable().Where(a => !string.IsNullOrWhiteSpace(a.Field<string>("Description")) && Convert.ToInt32(a.Field<string>("TaxType")) == (int)TaxTypes.State && a.Field<string>("TaxCode").ToUpper().Contains("PRS")).Select(s => s.Field<string>("TaxAmount")).SingleOrDefault();
                ReduceStateTaxAmount = dtEvertecTaxDetail.AsEnumerable().Where(a => !string.IsNullOrWhiteSpace(a.Field<string>("Description")) && Convert.ToInt32(a.Field<string>("TaxType")) == (int)TaxTypes.State && a.Field<string>("TaxCode").ToUpper().Contains("PRRS")).Select(s => s.Field<string>("TaxAmount")).SingleOrDefault();
                CityTaxAmount = dtEvertecTaxDetail.AsEnumerable().Where(a => !string.IsNullOrWhiteSpace(a.Field<string>("Description")) && Convert.ToInt32(a.Field<string>("TaxType")) == (int)TaxTypes.Municipality && a.Field<string>("TaxCode").ToUpper().Contains("PRM")).Select(s => s.Field<string>("TaxAmount")).SingleOrDefault();
                //EvertecTaxAmount = cityTaxAmount + "|" + stateTaxAmount;
                //return false;
                //return Newtonsoft.Json.JsonConvert.SerializeObject(dtEvertecTaxDetail);
            }
            else
            {
                TotalTax = string.Empty;
                return false;
            }
            return false;
        }

        #region PRIMEPOS-3034 02-Dec-2021 JY Added
        private void grdDetail_ClickCell(object sender, ClickCellEventArgs e)
        {
            if (e.Cell.Column.Header.Caption == this.grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.TransDetail_Fld_ItemID].Header.Caption
                && e.Cell.Style == Infragistics.Win.UltraWinGrid.ColumnStyle.URL)
            {
                frmTaxBreakDown ofrmTaxBreakDown = new frmTaxBreakDown(oPOSTrans.oTransDData, FormDataOnScreen.LineItemDetailData); //PRIMEPOS-2651 08-Apr-2022 JY Modified
                ofrmTaxBreakDown.ShowDialog();
            }
        }
        #endregion

        #region PRIMEPOS-3157 28-Nov-2022 JY Added
        private void btnPreviousImage_Click(object sender, EventArgs e)
        {
            if (loadedImages.Count > 0)
            {
                currentImageIndex = mod(currentImageIndex - 1, loadedImages.Count);
                pbImage.Image = loadedImages[currentImageIndex];
            }
        }

        private void btnNextImage_Click(object sender, EventArgs e)
        {
            if (loadedImages.Count > 0)
            {
                currentImageIndex = mod(currentImageIndex + 1, loadedImages.Count);
                pbImage.Image = loadedImages[currentImageIndex];
            }
        }

        int mod(int a, int b)
        {
            return (a % b + b) % b;
        }
        #endregion

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            //if (notifyicon != null)
            //{
            //    notifyicon.Visible = false;
            //    //messagebox.show("hello everyone");
            //    //string urldemo = "www.google.com";
            //    if (frmUrl == null)
            //        frmUrl = new frmUrlView(Convert.ToString(notifyicon.Tag));
            //    else
            //        frmUrl._url = Convert.ToString(notifyicon.Tag);
            //    frmUrl.Show();
            //    frmUrl.Focus();
            //    frmUrl = null;
            //    //notifyicon.Visible = false;
            //    notifyicon.Icon.Dispose();
            //}

            //if (notifyicon != null)
            //{
            notifyIcon1.Visible = false;
            if (frmUrl == null)
                frmUrl = new frmUrlView(Convert.ToString(notifyIcon1.Tag));
            else
                frmUrl._url = Convert.ToString(notifyIcon1.Tag);
            frmUrl.Show();
            frmUrl.Focus();
            //    frmUrl = null;
            //    //notifyicon.Visible = false;
            //    notifyicon.Icon.Dispose();
            //}
        }

        #region PRIMEPOS-3207
        private void ShowHyphenAlert()
        {
            try
            {
                if (!txtCustomer.Enabled) return;
                logger.Trace("ShowHyphenAlert() - " + clsPOSDBConstants.Log_Entering);

                foreach (RXHeader oRXHeader1 in oPOSTrans.oRXHeaderList)
                {
                    if (!string.IsNullOrWhiteSpace(oRXHeader1.PatientNo))
                    {
                        if (hyphenAlertDone != null && hyphenAlertDone.Count > 0)
                        {
                            if (hyphenAlertDone.Contains(oRXHeader1.PatientNo))
                            {
                                continue;
                            }
                            else
                            {
                                var rss2 = GetHyphenData(oRXHeader1.PatientNo);
                                hyphenAlertDone.Add(oRXHeader1.PatientNo);
                            }
                        }
                        else
                        {
                            var rss5 = GetHyphenData(oRXHeader1.PatientNo);
                            hyphenAlertDone.Add(oRXHeader1.PatientNo);
                        }
                    }
                }
                logger.Trace("ShowHyphenAlert() - " + clsPOSDBConstants.Log_Exiting);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "frmPOSTransaction==>ShowHyphenAlert(): An Exception Occured");
            }
        }
        public async Task<Newtonsoft.Json.Linq.JObject> GetHyphenData(string PatientNo)
        {
            try
            {
                //if (frmUrl != null)
                //{
                //    frmUrl.isUserClosed = false;
                //    frmUrl.Close();
                //    frmUrl.Dispose();
                //    frmUrl = null;
                //}
                #region logic to remove all the UrlView Form PRIMEPOS-3292
                FormCollection fc = Application.OpenForms;
                Dictionary<string, Form> dict = new Dictionary<string, Form>();
                int i = 0;
                try
                {
                    foreach (Form frm in fc)
                    {
                        if (frm.Name == "frmUrlView")
                        {
                            dict.Add(i.ToString(), frm);
                        }
                        i++;
                    }
                    foreach (KeyValuePair<string, Form> kv in dict)
                    {
                        Form fr = kv.Value;
                        frmUrlView.isUserClosed = false;
                        fr.Close();
                        fr.Dispose();
                    }
                }
                catch(Exception execp) 
                {
                    logger.Error(execp, "GetHyphenData()==>While closing the frmUrlView: An Exception Occured");
                }
                #endregion

                HyphenProcessor hp = new HyphenProcessor(PatientNo);

                System.Net.Http.HttpResponseMessage response = await hp.ManageHyphenAlert();

                if (response != null)
                {
                    var json = response.Content.ReadAsStringAsync().Result;
                    if (response.StatusCode.Equals(System.Net.HttpStatusCode.OK))
                    {
                        logger.Trace("frmPOSTransaction==>GetHyphenData()==> Hyphen Response Payload: " + json);
                        Newtonsoft.Json.Linq.JObject rss = Newtonsoft.Json.Linq.JObject.Parse(json);

                        HyphenAlertResponseBO hyphenAlertResponseBO = Newtonsoft.Json.JsonConvert.DeserializeObject<HyphenAlertResponseBO>(json);

                        //hyphenAlertResponseBO.showAlert = true; //PRIMEPOS-3207-Need to delete.

                        if (hyphenAlertResponseBO.showAlert)
                        {
                            frmAlert frm = new frmAlert();
                            #region PRIMEPOS-3292
                            string launchUrl = string.Empty;
                            launchUrl += Convert.ToString(rss["launchURL"]);
                            launchUrl += "&appId=" + enumHyphenAppId.PrimePOS.ToString() + "&source=" + enumHyphenSource.PrimePOS.ToString();
                            frm.LaunchUrl = launchUrl;
                            frm.notifyIcon = notifyIcon1;
                            #endregion
                            frm.showAlert("Hyphen Pharmacy Assistant alert", frmAlert.enmType.Success);
                            #region PRIMEPOS-3207
                            //notifyicon = new NotifyIcon(this.components);

                            //System.ComponentModel.ComponentResourceManager res = new System.ComponentModel.ComponentResourceManager(typeof(frmPOSTransaction));

                            //notifyicon.Visible = true;
                            ////notifyIcon1.BalloonTipText = "Click here to see the details";
                            ////notifyIcon1.BalloonTipTitle = "Hyphen Pharmacy Assistant alert("+ Convert.ToString(HyphenProcessor.insuranceID).Trim() + ")";
                            //notifyicon.Icon = ((System.Drawing.Icon)(res.GetObject("notifyIcon1.Icon")));
                            //notifyicon.Tag = Convert.ToString(rss["launchURL"]);
                            //notifyicon.Tag += "&appId=" + enumHyphenAppId.PrimePOS.ToString() + "&source=" + enumHyphenSource.PrimePOS.ToString();
                            //notifyicon.Text = "Hyphen Pharmacy Assistant alert(" + Convert.ToString(HyphenProcessor.insuranceID).Trim() + ")";
                            //notifyicon.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon1_MouseDoubleClick);

                            ////notifyIcon1.ShowBalloonTip(100);

                            #endregion

                            notifyIcon1.Visible = true;
                            //notifyIcon1.BalloonTipText = "Click here to see the details";
                            //notifyIcon1.BalloonTipTitle = "Hyphen Pharmacy Assistant alert("+ Convert.ToString(HyphenProcessor.insuranceID).Trim() + ")";
                            //notifyIcon1.Tag = Convert.ToString(rss["launchURL"]);
                            //notifyIcon1.Tag += "&appId=" + enumHyphenAppId.PrimePOS.ToString() + "&source=" + enumHyphenSource.PrimePOS.ToString();
                            notifyIcon1.Tag = launchUrl; //PRIMEPOS-3292
                            notifyIcon1.Text = "Hyphen Pharmacy Assistant alert(" + Convert.ToString(HyphenProcessor.insuranceID).Trim() + ")";
                            //notifyIcon1.ShowBalloonTip(100);
                        }
                        logger.Trace(string.Format("frmPOSTransaction==>GetHyphenData()-> Launch Url for {0} : {1}", Convert.ToString(HyphenProcessor.insuranceID).Trim(), notifyIcon1.Tag));
                        return rss;
                    }
                    else
                    {
                        logger.Error($"frmPOSTransaction==>GetHyphenData()==>Error getting data from Hyphen \nResponse Message: {json} and StatusCode: {response.StatusCode}");
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex, "frmPOSTransaction==>GetHyphenData(): An Exception Occured");
            }

            return null;
        }

        public void clearHyphenAlert()
        {
            //if (frmUrl != null)
            //{
            //    frmUrl.isUserClosed = false;
            //    frmUrl.Close();
            //    frmUrl.Dispose();
            //    frmUrl = null;
            //}
            #region logic to remove all the UrlView Form PRIMEPOS-3292
            FormCollection fc = Application.OpenForms;
            Dictionary<string, Form> dict = new Dictionary<string, Form>();
            int i = 0;
            try
            {
                if (Configuration.hyphenSetting.EnableHyphenIntegration.Equals("Y"))
                {
                    foreach (Form frm in fc)
                    {
                        if (frm.Name == "frmUrlView")
                        {
                            dict.Add(i.ToString(), frm);
                        }
                        i++;
                    }
                    foreach (KeyValuePair<string, Form> kv in dict)
                    {
                        Form fr = kv.Value;
                        frmUrlView.isUserClosed = false;

                        fr.Close();
                        fr.Dispose();
                    }
                }
            }
            catch (Exception execp)
            {
                logger.Error(execp, "clearHyphenAlert()==>While closing the frmUrlView: An Exception Occured");
            }
            #endregion
            hyphenAlertDone.Clear();
            this.notifyIcon1.Visible = false;
        }

        private bool PrimeRxPatientCounselingAudited()
        {
            PharmBL oPharmBL = new PharmBL();
           
            if (oPharmBL.ConnectedTo_ePrimeRx())
                return false; // only implemented patient counseling audit in PrimeRx
            if (oPharmBL.GetSettingPatientCounselingAudited() == "N")  // PrimeRx Switch is off
                return false;   
            return true;
        }

        #region PRIMEPOS-3130
        public string MaskDrugName(string itemID, string itemDescription)
        {
            try
            {
                if (Configuration.CSetting.MaskDrugName == true)
                {
                    if (itemID.ToLower() == "rx")
                    {
                        string[] arr=itemDescription.Split('-');
                        string itemCode = arr[0];
                        string refillNo = arr[1];
                        //string itemName = arr[2];
                        string itemName = string.Join("-",arr.Skip(2));
                        if (itemName!=null && itemName.Length > 0)
                        {
                            int length = itemName.Length;

                            if (length > 3)
                            {
                                string hidden = new string('*', length - 3);
                                itemName = itemName.Substring(0, 3) + hidden;
                                itemDescription = itemCode + "-" + refillNo + "-" + itemName;
                            }
                            if(length == 2)
                            {
                                itemName = itemName.Substring(0,1) + "*";
                                itemDescription = itemCode + "-" + refillNo + "-" + itemName;
                            }
                            return itemDescription;
                        }
                        else
                            return itemDescription;
                    }
                    else
                        return itemDescription;
                }
                else
                    return itemDescription;
            }
            catch (Exception ex)
            {
                return itemDescription;
            }
        }
        #endregion
        #endregion
    }

    //public enum POSTransactionType
    //{
    //    Sales = 1,
    //    SalesReturn = 2,
    //    ReceiveOnAccount = 3,
    //    Void = 4,
    //    VoidReturn = 5,
    //    Reverse = 6
    //}
    public class PayTypes
    {
        public const string CreditCard = "CC";
        public const string Cash = "CA";
        public const string Cheque = "CH";
        public const string Misc = "MI";
        public const string CancelTrans = "CT";
        public const string DebitCard = "DB"; //Added By Dharmendra(SRT) ON 26-08-08
        public const string EBT = "BT";
    }

    //public class OnholdRxs // This class move into business tier POSTransaction.cs
}
