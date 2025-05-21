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
using POS_Core_UI.Reports.Reports;
//using POS_Core_UI.Reports.ReportsUI;
using POS_Core.DataAccess;
using System.Collections.Generic;
using POS_Core.UserManagement;
using System.IO;
using POS_Core.ErrorLogging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using POS_Core_UI.Layout;
using NLog;
using MMSChargeAccount;
using POS_Core_UI.UserManagement;
using POS_Core.Resources;
using POS_Core_UI.Reports.ReportsUI;
using POS_Core_UI.Resources;

using System.Timers;
using System.Threading;
using System.Runtime.Remoting;
using Vendor.DAO.PurchaseOrder;
using Vendor.CommonData.PurchaseOrder;
using Vendor.DataTier.PurchaseOrder;
using Vendor.Interface.PurchaseOrder;
using System.Net.Sockets;
using Vendor.DAO.Item;
using Resources;
using POS_Core.CommonData.Tables;

namespace POS_Core_UI
{
    public partial class frmSearchMain : frmMasterLayout
    {
        #region Declaration
        public string SearchTable = "";
        public bool DisplayRecordAtStartup = false;
        private Search oBLSearch = new Search();
        private DataSet oDataSet = new DataSet();
        public bool isReadonly = false; // used in price inventory lookup
        public bool isInComplete = false;
        public bool isPartiallyAck = false;
        public bool isCopyOrder = false;
        public bool isFlaggedOrder = false;
        private int CurrentX;
        private int CurrentY;
        //Added By shitaljit 0n 17 June 0213 for PRIMEPOS-1189 Advance Search Result list is not retained when you come back to it after editing an item
        frmItemAdvSearch FrmAdvSearch = new frmItemAdvSearch();
        private bool IsAdvSearchDone = false;
        //End
        public string FormCaption = string.Empty;
        public string LabelText1 = string.Empty;
        public string LabelText2 = string.Empty;
        CustomerData oCustomerData = new CustomerData();//Added by Ravindra(QuicSolv) For Serach Rx customer 
        private string getPoStatus = string.Empty;
        private string GetPOStatus
        {
            set { getPoStatus = value; }
            get { return getPoStatus; }
        }

        #region customer search screen
        public bool IsCanceled = true;
        public int ActiveOnly = 0;
        private bool includeCPLCardInfo = false;
        private bool bAutoSelectSingleRow = false;
        private bool bOnlyCLPCardCustomers = false;
        public bool bSearchExactCustomer = false;//Added By Shitaljit to search the exact customer.
        private bool bSelection = false;
        #endregion

        #region Search screen
        public int AdditionalParameter = -1;
        private string ParamValue = string.Empty;
        private string PrgFlag;
        string strItemID = string.Empty;
        string strVendorItemID = string.Empty;
        string strBestVendorName = string.Empty;
        string strVendorCostPrice = string.Empty;
        public string SelectedRowCode;
        public string VendorID = string.Empty;    //PRIMEPOS-3155 12-Oct-2022 JY Added
        public string strSetSelected = string.Empty;
        public string DefaultCode = string.Empty;
        public string sCalledFrom = string.Empty;   //Sprint-26 - PRIMEPOS-2417 06-Jul-2017 JY Added
        private bool allowMultiRowSelect = false;
        internal bool AllowMultiRowSelect
        {
            get { return allowMultiRowSelect; }
            set { allowMultiRowSelect = value; }
        }

        private string selectedRowsData = string.Empty;
        internal string SelectedRowsData
        {
            get { return selectedRowsData; }
            set { selectedRowsData = value; }
        }

        /// <summary>
        /// Author : Gaurav
        /// Date : 11-Jul-2009
        /// Returns The Data count of searched data.
        /// </summary>
        public Int32 SearchDataRowsCount
        {
            get
            {
                return (oDataSet != null && oDataSet.Tables.Count > 0 ? oDataSet.Tables[0].Rows.Count : 0);
            }
        }

        //Added By Shitaljit(QuicSolv) Date(dd/mm/yy): 06-04-2011
        //Will search the data at constructor level if property 'SearchInConstructor' set to true.
        private bool searchInConstructor = false;
        public bool SearchInConstructor
        {
            get
            {
                return (searchInConstructor);
            }
            set
            {
                searchInConstructor = value;
                //if (searchInConstructor)
                //{
                //    GetFrmSearchData();
                //}
                //End OfAdded By Shitaljit(QuicSolv)
            }
        }

        private bool fromPurchaseOrder = false;
        public bool IsFromPurchaseOrder
        {
            set { fromPurchaseOrder = value; }
            get { return fromPurchaseOrder; }
        }

        private bool fromBestVendorPrice = false;
        public bool IsBestVendorPrice
        {
            get { return fromBestVendorPrice; }
        }

        #endregion
        private ILogger logger = LogManager.GetCurrentClassLogger();

        #region PRIMEPOS-2671 18-Apr-2019 JY Added
        public string sNPINo = string.Empty;
        private string sPSServiceAddress = "";
        public string PSServiceAddress
        {
            get { return sPSServiceAddress; }
            set { sPSServiceAddress = value; }
        }
        private MMSSearch.Service objService = new MMSSearch.Service();
        #endregion

        #endregion

        #region Constructors
        public frmSearchMain()
        {
            InitializeComponent();
            chkIncludeRXCust.Checked = Configuration.CSetting.AutoSearchPrimeRxPatient; //PRIMEPOS-2845 14-May-2020 JY Added
            setChildControlProperties(this); //PrimePOS-2523 Added by Farman Ansari on 24 May 2018
        }

        #region frmSearch constructors
        public frmSearchMain(Boolean bSelection)
        {
            //need to assign table while calling through this constructor
            this.bSelection = bSelection;
            InitializeComponent();
            setChildControlProperties(this); //PrimePOS-2523 Added by Farman Ansari on 24 May 2018
            //SearchTable = Table;
            //try
            //{
            //    grdSearch.DataSource = oDataSet;
            //    grdSearch.Refresh();
            //    SearchTable = Table;
            //    resizeColumns();
            //}
            //catch (Exception exp)
            //{
            //    clsUIHelper.ShowErrorMsg(exp.Message);
            //}
        }

        public frmSearchMain(string Table, string searchValue, Boolean bSelection)
        {
            this.bSelection = bSelection;
            InitializeComponent();
            setChildControlProperties(this); //PrimePOS-2523 Added by Farman Ansari on 24 May 2018
            try
            {
                //grdSearch.DataSource = oDataSet;
                //grdSearch.Refresh();
                ParamValue = searchValue;
                PrgFlag = Table;
                //Added By SRT(Ritesh Parekh) Date : 22-Jul-2009
                //assigned search type as search table to identify the caller at time of getting the vendoritemcode on post selection.
                SearchTable = Table;
                if (searchValue != "")
                    GetFrmSearchData();
                resizeColumns();
                //End Of Added By SRT(Ritesh Parekh)
            }
            catch (Exception exp)
            {
                logger.Fatal(exp, "frmSearchMain(string Table, string searchValue, Boolean bSelection)");
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }

        public frmSearchMain(string Table, string sCode, string sDescription, Boolean bSelection)
        {
            this.bSelection = bSelection;
            InitializeComponent();
            setChildControlProperties(this); //PrimePOS-2523 Added by Farman Ansari on 24 May 2018
            try
            {
                //this.txtCode.Text = sCode; ORIGINAL commected By Shitaljit(QuicSolv)
                //Modified By Shitaljit(QuicSolv) on 3 June 2011
                if (SearchSvr.SubDeptIDFlag)
                {
                    this.txtCode.Text = "";
                    DefaultCode = sCode;
                }
                else
                {
                    this.txtCode.Text = sCode;
                }
                //Till here Added By Shitaljit
                this.txtName.Text = sDescription;
                //grdSearch.DataSource = oDataSet;
                //grdSearch.Refresh();
                SearchTable = Table;
                resizeColumns();
            }
            catch (Exception exp)
            {
                logger.Fatal(exp, "frmSearchMain(string Table, string sCode, string sDescription, Boolean bSelection)");
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }

        public frmSearchMain(string Table, string tmpPrgFlag, string tmpParamName, string tmpParamValue, Boolean bSelection)
        {
            this.bSelection = bSelection;
            InitializeComponent();
            setChildControlProperties(this); //PrimePOS-2523 Added by Farman Ansari on 24 May 2018
            try
            {
                //grdSearch.DataSource = oDataSet;
                //grdSearch.Refresh();
                ParamValue = tmpParamValue;
                SearchTable = Table;
                PrgFlag = tmpPrgFlag;
                //DataQuery = "select * from " + Table + " where " + tmpParamName + "='" + tmpParamValue + "'"; //commented as not in use
                resizeColumns();
            }
            catch (Exception exp)
            {
                logger.Fatal(exp, "frmSearchMain(string Table, string tmpPrgFlag, string tmpParamName, string tmpParamValue, Boolean bSelection)");
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }
        #endregion

        #region frmCustomerSearch constructors
        public frmSearchMain(string sInitCriteria, Boolean bSelection, string sTable)   //PRIMEPOS-2475 20-Aug-2018 JY Added sTable parameter
            : this(sInitCriteria, false, false, bSelection, sTable)
        {
        }

        public frmSearchMain(string sInitCriteria, bool autoSelectSingleRow, Boolean bSelection, string sTable) //PRIMEPOS-2475 20-Aug-2018 JY Added sTable parameter
            : this(sInitCriteria, false, autoSelectSingleRow, bSelection, sTable)
        {
        }

        public frmSearchMain(string sInitCriteria, bool includeCLPCard, bool autoSelectSingleRow, Boolean bSelection, string sTable)    //PRIMEPOS-2475 20-Aug-2018 JY Added sTable parameter
            : this(sInitCriteria, includeCLPCard, autoSelectSingleRow, false, false, bSelection, sTable)
        {
        }

        public frmSearchMain(string sInitCriteria, bool includeCLPCard, bool autoSelectSingleRow, bool onlyCLPCardCustomers, Boolean bSelection, string sTable) //PRIMEPOS-2475 20-Aug-2018 JY Added sTable parameter
            : this(sInitCriteria, includeCLPCard, autoSelectSingleRow, onlyCLPCardCustomers, false, bSelection, sTable)
        {
        }

        public frmSearchMain(string sInitCriteria, bool includeCLPCard, bool autoSelectSingleRow, bool onlyCLPCardCustomers, bool bSearchExactCustomer, Boolean bSelection, string sTable)  //PRIMEPOS-2475 20-Aug-2018 JY Added sTable parameter
        {
            this.SearchTable = sTable;  //PRIMEPOS-2475 20-Aug-2018 JY Added
            this.bSelection = bSelection;
            this.bSearchExactCustomer = bSearchExactCustomer;
            InitializeComponent();
            chkIncludeRXCust.Checked = Configuration.CSetting.AutoSearchPrimeRxPatient; //PRIMEPOS-2845 14-May-2020 JY Added
            setChildControlProperties(this); //PrimePOS-2523 Added by Farman Ansari on 24 May 2018
            try
            {
                bool searchContact = false;
                if (sInitCriteria.EndsWith("/") && POS_Core.Resources.Configuration.convertNullToDouble(sInitCriteria.Substring(0, sInitCriteria.Length - 1)) > 0)
                {
                    searchContact = true;
                }

                if (searchContact == true)
                {
                    txtContactNumber.Text = sInitCriteria.Substring(0, sInitCriteria.Length - 1);
                    sInitCriteria = string.Empty;
                }
                #region PRIMEPOS-2475 22-Aug-2018 JY Commented
                //else if (Resources.Configuration.convertNullToDouble(sInitCriteria) != 0)
                //{
                //    txtCode.Text = sInitCriteria;
                //}
                //else
                //{
                //    txtName.Text = sInitCriteria;
                //}
                #endregion
                bAutoSelectSingleRow = autoSelectSingleRow;
                includeCPLCardInfo = includeCLPCard;
                bOnlyCLPCardCustomers = onlyCLPCardCustomers;
                #region Master search code added by shitaljit on 29Apr2013
                if (bSearchExactCustomer == false)
                {
                    this.txtMasterSearchVal.Text = sInitCriteria;
                }
                #endregion
            }
            catch (Exception exp)
            {
                logger.Fatal(exp, "frmSearchMain(string sInitCriteria, bool includeCLPCard, bool autoSelectSingleRow, bool onlyCLPCardCustomers, bool bSearchExactCustomer, Boolean bSelection)");
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }
        #endregion

        #endregion

        #region Events
        #region form event
        private void frmSearchMain_Load(object sender, System.EventArgs e)
        {
            try
            {
                logger.Trace("frmSearchMain_Load() - " + clsPOSDBConstants.Log_Entering);
                this.GetPOStatus = string.Empty;

                this.txtCode.Enter += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
                this.txtCode.Leave += new System.EventHandler(clsUIHelper.AfterExitEditMode);
                this.txtName.Enter += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
                this.txtName.Leave += new System.EventHandler(clsUIHelper.AfterExitEditMode);
                this.txtMasterSearchVal.Enter += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
                this.txtMasterSearchVal.Leave += new System.EventHandler(clsUIHelper.AfterExitEditMode);
                this.txtContactNumber.Enter += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
                this.txtContactNumber.Leave += new System.EventHandler(clsUIHelper.AfterExitEditMode);

                #region PRIMEPOS-2613 05-Dec-2018 JY Added
                this.dtpExpDate1.Value = DateTime.Today;
                this.dtpExpDate2.Value = DateTime.Today.Date.AddMonths(1);
                cboExpDate.SelectedIndex = 0;
                #endregion

                #region PRIMEPOS-2645 05-Mar-2019 JY Added
                this.dtpDateOfBirth1.Value = DateTime.Today;
                this.dtpDateOfBirth2.Value = DateTime.Today.Date.AddMonths(1);
                cboDOB.SelectedIndex = 0;
                #endregion

                if (!(SearchTable == clsPOSDBConstants.UsersGroup_tbl || SearchTable == clsPOSDBConstants.SubDepartment_tbl || SearchTable == "ItemID" || SearchTable == clsPOSDBConstants.DescriptionWise || SearchTable == clsPOSDBConstants.TransHeader_tbl || SearchTable == clsPOSDBConstants.MMSSearch))
                {
                    //if (SearchTable == clsPOSDBConstants.Customer_tbl) {
                    //    Search();
                    //}
                    //if (bSelection == true && this.allowMultiRowSelect == true && this.SearchTable == clsPOSDBConstants.Item_tbl)
                    //else
                    {
                        oDataSet = oBLSearch.SearchData(SearchTable, "----------------Invalid--------------", "----------------Invalid----------------", ActiveOnly, -1);
                        grdSearch.DataSource = oDataSet;
                        //this.resizeColumns();
                        grdSearch.Refresh();
                    }
                }

                SetControlsVisibility();

                //clsUIHelper.SetReadonlyRow(this.grdSearch);

                //if (this.grdSearch.Rows.Count == 0)
                //{
                //    this.ActiveControl = this.txtCode;
                //}
                //else
                //{
                //    grdSearch.Focus();
                //    grdSearch.PerformAction(UltraGridAction.FirstRowInGrid);
                //    grdSearch.Refresh();
                //}


                if (bSelection == true)
                {
                    if (SearchTable == clsPOSDBConstants.Customer_tbl)
                    {
                        this.Text = "Search Customer";
                        SearchCustomer();
                    }

                    if (bAutoSelectSingleRow == true && grdSearch.Rows.Count == 1)
                    {
                        IsCanceled = false;
                        this.Close();
                    }
                }


                if (Configuration.isNullOrEmptyDataSet(oDataSet) == false && oDataSet.Tables[0].Rows.Count < 1)
                {
                    this.txtCode.Focus();
                }
                resizeColumns();
                grdSearch.Refresh();
                #region Added logic to set width of "Check" column
                try
                {
                    if (SearchTable == clsPOSDBConstants.Item_tbl)  //PRIMEPOS-2395 22-Jun-2018 JY Added if clause
                    {
                        if (this.AllowMultiRowSelect == true)
                        {
                            if (this.grdSearch.DisplayLayout.Bands[0].Columns.Exists("CHECK") == false)
                            {
                                this.grdSearch.DisplayLayout.Bands[0].Columns.Add("CHECK");
                                this.grdSearch.DisplayLayout.Bands[0].Columns["CHECK"].Header.Caption = "";

                                this.grdSearch.DisplayLayout.Bands[0].Columns["CHECK"].DataType = typeof(bool);
                                this.grdSearch.DisplayLayout.Bands[0].Columns["CHECK"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.CheckBox;
                            }

                            this.grdSearch.DisplayLayout.Bands[0].Columns["Check"].Header.VisiblePosition = 0;
                        }
                    }
                    else
                    {
                        grdSearch.DisplayLayout.Bands[0].Columns["Check"].Width = 31;
                    }
                }
                catch
                {
                    //no need to catch exception as the specified column might not found in few search criterias
                }
                #endregion

                //Added Icon for all search screens Sandeep
                System.Resources.ResourceManager rs = new System.Resources.ResourceManager(typeof(UserManagement.frmLogin));
                this.Icon = (System.Drawing.Icon)rs.GetObject("$this.Icon");

                #region PRIMEPOS-3167 07-Nov-2022 JY Added
                try
                {
                    if (Configuration.CPrimeEDISetting.UsePrimePO == true && SearchTable == clsPOSDBConstants.POHeader_tbl && POS_Core.Resources.Configuration.StationID != "01")
                    {

                        InitializeOrderTimer();                        
                    }
                }
                catch (Exception Ex)
                {
                    logger.Fatal(Ex, "frmCreateNewPurchaseOrder_Load() - InitializeOrderTimer()");
                }
                #endregion

                logger.Trace("frmSearchMain_Load() - " + clsPOSDBConstants.Log_Exiting);
            }
            catch (Exception exp)
            {
                logger.Fatal(exp, "frmSearchMain_Load()");
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }

        private void frmSearchMain_Activated(object sender, System.EventArgs e)
        {
            clsUIHelper.CurrentForm = this;
            if (SearchTable != clsPOSDBConstants.item_PriceInv_Lookup)
            {
                if (bSelection == false)
                {
                    this.Left = clsPOSDBConstants.formLeft;
                    this.Top = clsPOSDBConstants.formLeft;
                }
            }
            if (bSelection == false && DisplayRecordAtStartup) Search();
            // Add SRT (Sachin) Date : 13 Nov 2009
            //SearchTopItems();
            //Comment By SRT (Sachin) Date : 23 Nov 2009
            //To Load Blank Item Screen 
        }

        //Added by shitaljit to fixed payment type populating issues in other report.
        private void frmSearchMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            SearchSvr.ISManagePaytype = false;

            if (orderUpdateTimer != null)   //PRIMEPOS-3167 07-Nov-2022 JY Added
            {
                orderUpdateTimer.Enabled = false;
                orderUpdateTimer.Stop();
            }
        }
        //Till here Added by Krishna 

        private void frmSearchMain_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            try
            {
                logger.Trace("frmSearchMain_KeyDown() - " + clsPOSDBConstants.Log_Entering);
                if (e.KeyData == System.Windows.Forms.Keys.Enter)
                {
                    if (!bSelection)
                    {
                        if (this.ActiveControl.Name == "grdSearch")
                        {
                            if (this.grdSearch.Rows.Count > 0)
                            {
                                int countIncompleteOrders = 0;
                                if (SearchTable == clsPOSDBConstants.POHeader_tbl)
                                {
                                    if (this.grdSearch.ActiveRow.Cells["PO Status"].Text.ToUpper().Equals("INCOMPLETE"))
                                    {
                                        for (int count = 0; count < this.grdSearch.Rows.Count; count++)
                                        {
                                            string poStatus = grdSearch.Rows[count].Cells["PO Status"].Text;
                                            if (poStatus.ToUpper().Equals("INCOMPLETE"))
                                            {
                                                countIncompleteOrders++;
                                            }
                                        }
                                        if (countIncompleteOrders > 0)
                                        {
                                            isInComplete = true;
                                            Edit();
                                        }
                                    }
                                }
                                else
                                {
                                    Edit();
                                }
                            }
                        }
                    }
                    else
                    {
                        if (this.grdSearch.ContainsFocus == true)
                        {
                            if (this.grdSearch.Rows.Count > 0)
                            {
                                IsCanceled = false;
                                this.Close();
                            }
                        }
                        if (this.txtMasterSearchVal.ContainsFocus == true)
                        {
                            if (SearchTable == clsPOSDBConstants.Customer_tbl)
                                SearchCustomer();
                            else
                                GetFrmSearchData();
                        }
                        else
                        {
                            this.SelectNextControl(this.ActiveControl, true, true, true, true);
                        }
                    }
                }
                else  //PRIMEPOS-2475 07-Jun-2018 JY Added
                {
                    if (e.Alt)
                        ShortCutKeyAction(e.KeyCode);
                }
                //if (e.Alt && e.KeyCode == Keys.C) {
                //    if (SearchTable == clsPOSDBConstants.Customer_tbl)
                //        btnClose_Click(btnClose, new EventArgs());
                //}
                logger.Trace("frmSearchMain_KeyDown() - " + clsPOSDBConstants.Log_Exiting);
            }
            catch (Exception exp)
            {
                logger.Fatal(exp, "frmSearchMain_KeyDown()");
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }

        #region PRIMEPOS-2475 07-Jun-2018 JY Added
        private void ShortCutKeyAction(Keys KeyCode)
        {
            switch (KeyCode)
            {
                case Keys.C:
                    if (pnlClose.Visible == true && pnlClose.Enabled == true)
                        btnClose_Click(btnClose, new EventArgs());
                    break;
                case Keys.I:
                    if (pnlImportCustomerFromPrimeRX.Visible)
                        btnImportCustomerFromPrimeRX_Click(btnImportCustomerFromPrimeRX, new EventArgs());
                    break;
                case Keys.L:
                    if (pnlClear.Visible == true && pnlClear.Enabled == true)
                        btnClear_Click(btnClear, new EventArgs());
                    break;
                case Keys.O:
                    if (pnlOk.Visible == true && pnlOk.Enabled == true)
                        btnOk_Click(btnOk, new EventArgs());
                    break;
                case Keys.P:
                    if (pnlPrint.Visible == true && pnlPrint.Enabled == true)
                        btnPrint_Click(btnPrint, new EventArgs());
                    break;
                case Keys.R:
                    if (pnlResetPass.Visible == true && pnlResetPass.Enabled == true)
                        btnResetPass_Click(btnResetPass, new EventArgs());
                    break;
                case Keys.U:
                    if (pnlUnlock.Visible == true && pnlUnlock.Enabled == true)
                        btnUnlock_Click(btnUnlock, new EventArgs());
                    break;
                case Keys.V:
                    if (pnlViewDeptList.Visible == true && pnlViewDeptList.Enabled == true)
                        btnViewDeptList_Click(btnViewDeptList, new EventArgs());
                    break;
                case Keys.E:    //PRIMEPOS-2779 17-Jan-2020 JY Added
                    if (pnlExport.Visible == true && pnlExport.Enabled == true)
                        btnExport_Click(btnExport, new EventArgs());
                    break;
                case Keys.T://PRIMEPOS-2896
                    if (pnlCustTokenize.Visible == true && pnlCustTokenize.Enabled == true)
                        btnCustTokenize_Click(btnCustTokenize, new EventArgs());
                    break;
                default:
                    break;
            }
        }
        #endregion

        private void frmSearchMain_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            logger.Trace("frmSearchMain_KeyUp() - " + clsPOSDBConstants.Log_Entering);

            switch (e.KeyData)
            {
                case Keys.F2:
                    if (pnlAdd.Visible == true && pnlAdd.Enabled == true) Add();
                    break;
                case Keys.F3:
                    if (pnlEdit.Visible == true && pnlEdit.Enabled == true)
                    {
                        if (grdSearch.Rows.Count < 1) return;
                        isInComplete = true;
                        Edit();
                    }
                    break;
                case Keys.F4:
                    if (pnlSearch.Visible == true && pnlSearch.Enabled == true)
                    {
                        btnSearch_Click(null, null);
                    }
                    break;
                case Keys.F5:
                    if (pnlCopy.Visible == true && pnlCopy.Enabled == true)
                        btnCopy_Click(null, null);
                    break;
                case Keys.F6:
                    if (pnlCopyOrder.Visible == true && pnlCopyOrder.Enabled == true)
                    {
                        if (grdSearch.Rows.Count < 1) return;
                        bool flagStatus = false;
                        flagStatus = Configuration.convertNullToBoolean(grdSearch.ActiveRow.Cells["Template"].Value.ToString());
                        CopyOrder(flagStatus);
                    }
                    break;
                case Keys.F7:
                    if (pnlAckManual.Visible == true && pnlAckManual.Enabled == true)
                        AcknowledgeManually();
                    break;
                case Keys.F8:
                    if (pnlFillPartialOrder.Visible == true && pnlFillPartialOrder.Enabled == true)
                    {
                        if (grdSearch.Rows.Count < 1) return;
                        FillOrder();
                    }
                    break;
                case Keys.F9:
                    if (pnlResubmit.Visible == true && pnlResubmit.Enabled == true)
                    {
                        if (grdSearch.Rows.Count < 1)
                            return;
                        ReSubmitOrder();
                    }
                    break;
                case Keys.F10:
                    if (pnlDelete.Visible == true && pnlDelete.Enabled == true)
                        btnDelete_Click(null, null);
                    break;
                //Added By Shitaljit For Advance Search.
                case Keys.F11:
                    if (this.SearchTable == clsPOSDBConstants.Item_tbl && pnlAdvSearch.Visible == true && pnlAdvSearch.Enabled == true)
                    {
                        btnAdvSearch_Click(null, null);
                    }
                    break;
                case Keys.F12:
                    if (pnlRefresh.Visible == true && pnlRefresh.Enabled == true)
                    {
                        btnRefresh_Click(null, null);
                    }
                    break;

            }
            logger.Trace("frmSearchMain_KeyUp() - " + clsPOSDBConstants.Log_Exiting);
        }

        private void frmSearchMain_Resize(object sender, EventArgs e)
        {
            this.Left = 0;      //ClsConstant.formLeft;
            this.Top = 0;     //ClsConstant.formTop;
        }
        #endregion

        #region Grid event
        //Changed by Prashant(SRT) Date:1-06-09
        private void grdSearch_AfterSelectChange(object sender, AfterSelectChangeEventArgs e)
        {
            logger.Trace("grdSearch_AfterSelectChange() - " + clsPOSDBConstants.Log_Entering);
            string statusInGrid = "";
            if (SearchTable == clsPOSDBConstants.POHeader_tbl)
            {
                try
                {
                    if (grdSearch.Selected.Rows.Count > 0) //PRIMEPOS-3295
                    {
                        statusInGrid = grdSearch.Selected.Rows[0].Cells["PO Status"].Value.ToString();
                    }
                }
                catch (Exception exp)
                {
                    logger.Fatal(exp, "grdSearch_AfterSelectChange()");
                }
            }

            if (grdSearch.Selected.Rows.Count > 0 && statusInGrid != "")
            {
                try
                {
                    this.pnlFillPartialOrder.Enabled = false;
                    this.pnlCopyOrder.Enabled = false;
                    this.pnlAckManual.Enabled = true;
                    this.pnlResubmit.Enabled = false;
                    statusInGrid = grdSearch.Selected.Rows[0].Cells["PO Status"].Value.ToString();
                    switch (statusInGrid)
                    {
                        case "Incomplete":
                            //this.btnEdit.Enabled = true;
                            //Added By SRT(Gaurav) Date: 06/07/2009
                            this.pnlCopyOrder.Enabled = Configuration.convertNullToBoolean(grdSearch.Selected.Rows[0].Cells["Template"].Value.ToString());
                            this.pnlDelete.Enabled = true;//Added by Ravindra for Delete Incomplete PO  
                            //End Of Added By SRT(Gaurav)
                            break;
                        case "Processed":
                            this.pnlAckManual.Enabled = false;
                            //this.btnEdit.Enabled = false;
                            //Added By SRT(Gaurav) Date: 06/07/2009
                            this.pnlDelete.Enabled = false;//Added by Ravindra for Delete Incomplete PO  
                            this.pnlCopyOrder.Enabled = Configuration.convertNullToBoolean(grdSearch.Selected.Rows[0].Cells["Template"].Value.ToString());
                            //End Of Added By SRT(Gaurav)
                            break;
                        case "Expired":
                            this.pnlCopyOrder.Enabled = true;
                            this.pnlDelete.Enabled = true;//Added by Ravindra for Delete Incomplete PO  
                            //this.btnEdit.Enabled = false;
                            this.pnlResubmit.Enabled = true;
                            break;
                        case "Canceled":
                            this.pnlCopyOrder.Enabled = true;
                            //this.btnEdit.Enabled = false;
                            this.pnlAckManual.Enabled = false;
                            break;
                        case "Pending":
                            //Added By SRT(Gaurav) Date: 06/07/2009
                            this.pnlDelete.Enabled = false;//Added by Ravindra for Delete Incomplete PO  
                            this.pnlCopyOrder.Enabled = Configuration.convertNullToBoolean(grdSearch.Selected.Rows[0].Cells["Template"].Value.ToString());
                            //End Of Added By SRT(Gaurav)
                            this.pnlAckManual.Enabled = false;
                            //this.btnEdit.Enabled = false;
                            break;
                        case "Queued":
                            //Added By SRT(Gaurav) Date: 06/07/2009
                            this.pnlDelete.Enabled = false;//Added by Ravindra for Delete Incomplete PO  
                            this.pnlCopyOrder.Enabled = Configuration.convertNullToBoolean(grdSearch.Selected.Rows[0].Cells["Template"].Value.ToString());
                            //End Of Added By SRT(Gaurav)
                            this.pnlAckManual.Enabled = false;
                            //this.btnEdit.Enabled = false;
                            break;
                        case "Submitted":
                            //Added By SRT(Gaurav) Date: 06/07/2009
                            this.pnlDelete.Enabled = false;//Added by Ravindra for Delete Incomplete PO  
                            this.pnlCopyOrder.Enabled = Configuration.convertNullToBoolean(grdSearch.Selected.Rows[0].Cells["Template"].Value.ToString());
                            //End Of Added By SRT(Gaurav)
                            //this.btnEdit.Enabled = false;
                            this.pnlResubmit.Enabled = true;
                            break;
                        case "Overdue":
                            //Added By SRT(Gaurav) Date: 06/07/2009
                            this.pnlDelete.Enabled = false;//Added by Ravindra for Delete Incomplete PO  
                            this.pnlCopyOrder.Enabled = Configuration.convertNullToBoolean(grdSearch.Selected.Rows[0].Cells["Template"].Value.ToString());
                            //End Of Added By SRT(Gaurav)
                            //this.btnEdit.Enabled = false;
                            this.pnlResubmit.Enabled = true;
                            break;
                        case "Acknowledge":
                            //Added By SRT(Gaurav) Date: 06/07/2009
                            this.pnlDelete.Enabled = false;//Added by Ravindra for Delete Incomplete PO  
                            this.pnlCopyOrder.Enabled = Configuration.convertNullToBoolean(grdSearch.Selected.Rows[0].Cells["Template"].Value.ToString());
                            //End Of Added By SRT(Gaurav)
                            this.pnlAckManual.Enabled = false;
                            //this.btnEdit.Enabled = false;
                            break;
                        case "AcknowledgeManually":
                            //Added By SRT(Gaurav) Date: 06/07/2009
                            this.pnlDelete.Enabled = false;//Added by Ravindra for Delete Incomplete PO  
                            this.pnlCopyOrder.Enabled = Configuration.convertNullToBoolean(grdSearch.Selected.Rows[0].Cells["Template"].Value.ToString());
                            //End Of Added By SRT(Gaurav)
                            this.pnlAckManual.Enabled = false;
                            //this.btnEdit.Enabled = false;
                            break;
                        case "MaxAttempt":
                            //Added By SRT(Gaurav) Date: 06/07/2009
                            this.pnlResubmit.Enabled = true;
                            this.pnlDelete.Enabled = false;//Added by Ravindra for Delete Incomplete PO  
                            this.pnlCopyOrder.Enabled = Configuration.convertNullToBoolean(grdSearch.Selected.Rows[0].Cells["Template"].Value.ToString());
                            //End Of Added By SRT(Gaurav)
                            //this.btnEdit.Enabled = false;
                            break;
                        case "Error":
                            //Added By SRT(Gaurav) Date: 06/07/2009
                            this.pnlResubmit.Enabled = true;
                            this.pnlDelete.Enabled = false;//Added by Ravindra for Delete Incomplete PO  
                            this.pnlCopyOrder.Enabled = Configuration.convertNullToBoolean(grdSearch.Selected.Rows[0].Cells["Template"].Value.ToString());
                            //End Of Added By SRT(Gaurav)
                            //this.btnEdit.Enabled = false;
                            break;
                        case "PartiallyAck":
                            //Added By SRT(Gaurav) Date: 06/07/2009
                            this.pnlCopyOrder.Enabled = Configuration.convertNullToBoolean(grdSearch.Selected.Rows[0].Cells["Template"].Value.ToString());
                            //End Of Added By SRT(Gaurav)
                            this.pnlDelete.Enabled = false;//Added by Ravindra for Delete Incomplete PO  
                            this.pnlFillPartialOrder.Enabled = true;
                            this.pnlAckManual.Enabled = false;
                            //this.btnEdit.Enabled = false;
                            break;
                        case "PartiallyAck-Reorder":
                            //Added By SRT(Prashant) Date: 07/07/2009
                            this.pnlDelete.Enabled = false;//Added by Ravindra for Delete Incomplete PO  
                            this.pnlCopyOrder.Enabled = Configuration.convertNullToBoolean(grdSearch.Selected.Rows[0].Cells["Template"].Value.ToString());
                            //End Of Added By SRT(Prashant)
                            this.pnlAckManual.Enabled = false;
                            //this.btnEdit.Enabled = false;
                            break;
                        case "SubmittedManually":
                            this.pnlDelete.Enabled = false;//Added by Ravindra for Delete Incomplete PO  
                            //Added by SRT(Abhishek)
                            this.pnlAckManual.Enabled = true;
                            this.pnlCopyOrder.Enabled = Configuration.convertNullToBoolean(grdSearch.Selected.Rows[0].Cells["Template"].Value.ToString());
                            break;
                        default:
                            //Added By SRT(Gaurav) Date: 06/07/2009
                            this.pnlDelete.Enabled = false;//Added by Ravindra for Delete Incomplete PO  
                            this.pnlCopyOrder.Enabled = Configuration.convertNullToBoolean(grdSearch.Selected.Rows[0].Cells["Template"].Value.ToString());
                            //End Of Added By SRT(Gaurav)
                            //this.btnEdit.Enabled = false;
                            break;
                    }
                }
                catch (Exception ex)
                {
                    logger.Fatal(ex, "grdSearch_AfterSelectChange()");
                }
            }
            logger.Trace("grdSearch_AfterSelectChange() - " + clsPOSDBConstants.Log_Exiting);
        }
        //End of Changed by Prashant(SRT) Date:1-06-09

        private void grdSearch_BeforeSelectChange(object sender, BeforeSelectChangeEventArgs e)
        {
            try
            {
                if (SearchTable == clsPOSDBConstants.Users_tbl)
                {
                    string strLocked = "";
                    strLocked = grdSearch.ActiveRow.Cells[clsPOSDBConstants.Users_Fld_IsLocked].Value.ToString();
                    SetUnlockBtnState(strLocked);
                }
            }
            catch { }
        }

        private void grdSearch_ClickCellButton(object sender, CellEventArgs e)
        {
            POHeaderSvr poData = new POHeaderSvr();
            String poNumber = String.Empty;
            String poStatus = String.Empty;
            try
            {
                logger.Trace("grdSearch_ClickCellButton() - " + clsPOSDBConstants.Log_Entering);
                poNumber = grdSearch.ActiveRow.Cells[0].Text;
                poStatus = grdSearch.ActiveRow.Cells["PO Status"].Text;
                if (grdSearch.ActiveCell.Value.ToString() == true.ToString())//grdSearch.ActiveCell.ButtonAppearance.Image.ToString() == imageList1.Images[0].ToString() || grdSearch.ActiveCell.ButtonAppearance.Image ==null)
                {
                    grdSearch.ActiveCell.ButtonAppearance.Image = imageList1.Images[1];
                    grdSearch.ActiveCell.Value = false;
                    isFlaggedOrder = false;
                    poData.UpdateFlaggedStatus(Convert.ToInt32(grdSearch.ActiveRow.Cells["OrderId"].Value.ToString()), false);
                    if (clsPOSDBConstants.Expired != poStatus || clsPOSDBConstants.Canceled != poStatus)
                        this.pnlCopyOrder.Enabled = false;
                }
                else
                {
                    grdSearch.ActiveCell.ButtonAppearance.Image = imageList1.Images[0];
                    grdSearch.ActiveCell.Value = true;
                    isFlaggedOrder = true;
                    poData.UpdateFlaggedStatus(Convert.ToInt32(grdSearch.ActiveRow.Cells["OrderId"].Value.ToString()), true);
                    this.pnlCopyOrder.Enabled = true;
                }
                logger.Trace("grdSearch_ClickCellButton() - " + clsPOSDBConstants.Log_Exiting);
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "grdSearch_ClickCellButton()");
            }
        }

        private void grdSearch_DoubleClick(object sender, System.EventArgs e)
        {
            try
            {
                logger.Trace("grdSearch_DoubleClick() - " + clsPOSDBConstants.Log_Entering);

                Point point = System.Windows.Forms.Cursor.Position;
                point = this.grdSearch.PointToClient(point);
                Infragistics.Win.UIElement oUI = this.grdSearch.DisplayLayout.UIElement.ElementFromPoint(point);
                if (oUI == null)
                    return;

                if (!bSelection)
                {
                    if (SearchTable == clsPOSDBConstants.POHeader_tbl)
                    {
                        if (!this.grdSearch.ActiveRow.Cells["PO Status"].Text.ToUpper().Equals("INCOMPLETE"))
                        {
                            return;
                        }
                        while (oUI != null)
                        {
                            if (oUI.GetType() == typeof(Infragistics.Win.UltraWinGrid.RowUIElement))
                            {

                                Infragistics.Win.UltraWinGrid.RowUIElement oRowUI = oUI as Infragistics.Win.UltraWinGrid.RowUIElement;
                                int countPO = (int)grdSearch.Rows.Count;
                                int countIncompleteOrders = 0;

                                for (int count = 0; count < countPO; count++)
                                {
                                    string poStatus = grdSearch.Rows[count].Cells["PO Status"].Text;
                                    if (poStatus.ToUpper().Equals("INCOMPLETE"))
                                    {
                                        countIncompleteOrders++;
                                    }
                                }
                                if (countIncompleteOrders > 0)
                                {
                                    isInComplete = true;
                                    Edit();
                                }
                            }
                            oUI = oUI.Parent;
                        }
                    }
                    else
                    {
                        // Edit(); Commented by Shitaljit(QuicSolv) on 21 Oct 2011 to solve grid scrolling issue
                    }
                }
                else
                {
                    while (oUI != null)
                    {
                        if (oUI.GetType() == typeof(Infragistics.Win.UltraWinGrid.RowUIElement))
                        {
                            if (grdSearch.Rows.Count == 0)
                                return;
                            IsCanceled = false;
                            this.Close();
                        }
                        oUI = oUI.Parent;
                    }
                }
                logger.Trace("grdSearch_DoubleClick() - " + clsPOSDBConstants.Log_Exiting);
            }
            catch (Exception exp)
            {
                logger.Fatal(exp, "grdSearch_DoubleClick()");
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }

        private void grdSearch_DoubleClickRow(object sender, DoubleClickRowEventArgs e)
        {
            if (!bSelection && SearchTable != clsPOSDBConstants.POHeader_tbl)
            {
                Edit();
            }
        }

        private void grdSearch_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
        {
            for (int i = 0; i < this.grdSearch.DisplayLayout.Bands[0].Columns.Count; i++)
            {
                if (this.grdSearch.DisplayLayout.Bands[0].Columns[i].DataType == typeof(System.Decimal))
                {
                    this.grdSearch.DisplayLayout.Bands[0].Columns[i].Format = "#######0.000#";// changed 0.00 to 0.000# by atul 11-jan-2010 for jira issue
                    this.grdSearch.DisplayLayout.Bands[0].Columns[i].CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right;
                }
            }

            if (SearchTable == clsPOSDBConstants.Users_tbl)
            {
                this.grdSearch.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.Users_Fld_ID].Hidden = true;
                this.grdSearch.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.Users_Fld_IsLocked].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.CheckBox;
            }
            else if (SearchTable == clsPOSDBConstants.Customer_tbl)
            {
                grdSearch.DisplayLayout.Bands[0].Columns["Phone Home"].MaskInput = "(###) ###-####";
                grdSearch.DisplayLayout.Bands[0].Columns["Phone Home"].MaskDataMode = Infragistics.Win.UltraWinMaskedEdit.MaskMode.Raw;
                grdSearch.DisplayLayout.Bands[0].Columns["Phone Home"].MaskClipMode = Infragistics.Win.UltraWinMaskedEdit.MaskMode.IncludeBoth;
                grdSearch.DisplayLayout.Bands[0].Columns["Phone Home"].MaskDisplayMode = Infragistics.Win.UltraWinMaskedEdit.MaskMode.IncludeBoth;

                grdSearch.DisplayLayout.Bands[0].Columns["Phone Office"].MaskInput = "(###) ###-####";
                grdSearch.DisplayLayout.Bands[0].Columns["Phone Office"].MaskDataMode = Infragistics.Win.UltraWinMaskedEdit.MaskMode.Raw;
                grdSearch.DisplayLayout.Bands[0].Columns["Phone Office"].MaskClipMode = Infragistics.Win.UltraWinMaskedEdit.MaskMode.IncludeBoth;
                grdSearch.DisplayLayout.Bands[0].Columns["Phone Office"].MaskDisplayMode = Infragistics.Win.UltraWinMaskedEdit.MaskMode.IncludeBoth;

                grdSearch.DisplayLayout.Bands[0].Columns["Cell No."].MaskInput = "(###) ###-####";
                grdSearch.DisplayLayout.Bands[0].Columns["Cell No."].MaskDataMode = Infragistics.Win.UltraWinMaskedEdit.MaskMode.Raw;
                grdSearch.DisplayLayout.Bands[0].Columns["Cell No."].MaskClipMode = Infragistics.Win.UltraWinMaskedEdit.MaskMode.IncludeBoth;
                grdSearch.DisplayLayout.Bands[0].Columns["Cell No."].MaskDisplayMode = Infragistics.Win.UltraWinMaskedEdit.MaskMode.IncludeBoth;

                this.grdSearch.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.Customer_Fld_CustomerId].Hidden = true;
                if (grdSearch.DisplayLayout.Bands[0].Columns.Exists(clsPOSDBConstants.Customer_Fld_Name))
                {
                    grdSearch.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.Customer_Fld_Name].Header.VisiblePosition = 1; //PRIMEPOS-3556
                }

                if (grdSearch.DisplayLayout.Bands[0].Columns.Exists(clsPOSDBConstants.Customer_Fld_DOB))
                {
                    grdSearch.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.Customer_Fld_DOB].Header.VisiblePosition = 2; //PRIMEPOS-3556
                }
            }
            else if (SearchTable == clsPOSDBConstants.TransFee_tbl)
            {
                this.grdSearch.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.TransFee_Fld_ChargeTransFeeFor].Hidden = true;
                this.grdSearch.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.TransFee_Fld_TransFeeMode].Hidden = true;
                this.grdSearch.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.TransFee_Fld_PayTypeID].Hidden = true;
            }

            #region dynamic record count 
            SummarySettings summary = new SummarySettings();
            UltraGridColumn columnToSummarize = e.Layout.Bands[0].Columns[0];
            if (SearchTable == clsPOSDBConstants.Customer_tbl)
                columnToSummarize = e.Layout.Bands[0].Columns[1];

            try
            {
                summary = e.Layout.Bands[0].Summaries.Add("Record(s) Count = ", SummaryType.Count, columnToSummarize);
            }
            catch { }
            summary.DisplayFormat = "Record(s) Count = {0}";
            summary.Appearance.TextHAlign = Infragistics.Win.HAlign.Left;
            summary.SummaryPosition = SummaryPosition.Left;
            summary.SummaryDisplayArea = SummaryDisplayAreas.BottomFixed;
            e.Layout.Bands[0].Summaries[0].SummaryPositionColumn = columnToSummarize;
            e.Layout.Bands[0].Override.SummaryFooterCaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            e.Layout.Override.SummaryDisplayArea = SummaryDisplayAreas.BottomFixed;

            e.Layout.Override.SummaryFooterAppearance.BackColor = Color.Silver;
            e.Layout.Override.SummaryValueAppearance.BackColor = Color.Silver;
            e.Layout.Override.SummaryValueAppearance.ForeColor = Color.Maroon;
            e.Layout.Override.SummaryValueAppearance.FontData.Bold = Infragistics.Win.DefaultableBoolean.True;

            e.Layout.Override.SummaryFooterSpacingAfter = 5;
            e.Layout.Override.SummaryFooterSpacingBefore = 5;

            //e.Layout.Override.GroupBySummaryDisplayStyle = GroupBySummaryDisplayStyle.SummaryCells;
            #endregion
        }

        private void grdSearch_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            this.CurrentX = e.X;
            this.CurrentY = e.Y;
        }
        #endregion

        public void btnSearch_Click(object sender, System.EventArgs e)
        {
            if (bSelection == false && chkIncludeInActiveTax.Checked == true) //Added By Arvind
                Search(true);
            else
            {
                if (SearchTable == clsPOSDBConstants.Customer_tbl)
                    SearchCustomer();
                else if (SearchTable == clsPOSDBConstants.UsersGroup_tbl)
                    SearchUserGruop();  //PRIMEPOS-2780 27-Sep-2021 JY Added
                else
                    GetFrmSearchData();
            }
        }

        private void btnAdd_Click(object sender, System.EventArgs e)
        {
            Add();
        }

        private void btnEdit_Click(object sender, System.EventArgs e)
        {
            if (grdSearch.Rows.Count < 1)
                return;
            //isInComplete = true;
            //this.Edit();
            try
            {
                logger.Trace("btnEdit_Click() - " + clsPOSDBConstants.Log_Entering);

                if (this.grdSearch.Rows.Count > 0)
                {
                    int countIncompleteOrders = 0;
                    if (SearchTable == clsPOSDBConstants.POHeader_tbl)
                    {
                        //if (this.grdSearch.ActiveRow.Cells["PO Status"].Text.ToUpper().Equals("INCOMPLETE"))
                        //{
                        for (int count = 0; count < this.grdSearch.Rows.Count; count++)
                        {
                            string poStatus = grdSearch.Rows[count].Cells["PO Status"].Text;
                            if (poStatus.ToUpper().Equals("INCOMPLETE"))
                            {
                                countIncompleteOrders++;
                            }
                        }
                        if (countIncompleteOrders > 0)
                        {
                            isInComplete = true;
                            Edit();
                        }
                        //}
                    }
                    else
                    {
                        Edit();
                    }
                }
                logger.Trace("btnEdit_Click() - " + clsPOSDBConstants.Log_Exiting);
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "btnEdit_Click()");
            }
        }

        private void btnClose_Click(object sender, System.EventArgs e)
        {
            IsCanceled = true;
            this.Close();
        }

        private void txtCode_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                if (txtCode.Text.Trim() != "")
                {
                    btnSearch_Click(btnSearch, new EventArgs());
                }
                else
                {
                    this.SelectNextControl(this.ActiveControl, true, true, true, true);
                }
            }
            if (e.KeyData == Keys.Down)
            {
                if (this.grdSearch.Rows.Count > 0)
                {
                    this.grdSearch.Focus();
                    this.grdSearch.ActiveRow = this.grdSearch.Rows[0];
                }
            }
        }

        private void txtName_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                if (txtName.Text.Trim() != "")
                {
                    btnSearch_Click(btnSearch, new EventArgs());
                }
                else
                {
                    this.SelectNextControl(this.ActiveControl, true, true, true, true);
                }
            }

            if (e.KeyData == Keys.Down)
            {
                if (this.grdSearch.Rows.Count > 0)
                {
                    this.grdSearch.Focus();
                    this.grdSearch.ActiveRow = this.grdSearch.Rows[0];
                }
            }
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            switch (SearchTable)
            {
                case clsPOSDBConstants.Users_tbl:
                    Preview(false);
                    break;
                case clsPOSDBConstants.Item_tbl:
                    PrintGridData(clsPOSDBConstants.Item_tbl);
                    break;
            }
        }

        private void cboAddEditPOStatusList_SelectionChangeCommitted(object sender, EventArgs e)
        {
            try
            {
                logger.Trace("cboAddEditPOStatusList_SelectionChangeCommitted() - " + clsPOSDBConstants.Log_Entering);
                if (!bSelection)
                {
                    this.GetPOStatus = (string)cboAddEditPOStatusList.SelectedItem.DisplayText;
                    Search();
                }
                logger.Trace("cboAddEditPOStatusList_SelectionChangeCommitted() - " + clsPOSDBConstants.Log_Exiting);
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "cboAddEditPOStatusList_SelectionChangeCommitted()");
                clsUIHelper.ShowErrorMsg(ex.Message);
            }
        }

        private void btnFillPartialOrder_Click(object sender, EventArgs e)
        {
            if (grdSearch.Rows.Count < 1)
                return;
            FillOrder();
        }

        private void btnCopyOrder_Click(object sender, EventArgs e)
        {
            bool flagStatus = false;
            if (grdSearch.Rows.Count < 1)
                return;

            if (grdSearch.ActiveRow.Cells["Template"].Value != DBNull.Value)
                flagStatus = Configuration.convertNullToBoolean(grdSearch.ActiveRow.Cells["Template"].Value.ToString());

            CopyOrder(flagStatus);
        }

        private void btnAckManual_Click(object sender, EventArgs e)
        {
            AcknowledgeManually();
        }

        private void cboAddEditPOStatusList_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyData != Keys.Enter)
                {
                    cboAddEditPOStatusList.DropDown();
                }
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "cboAddEditPOStatusList_KeyDown()");
                //ErrorLogging.ErrorHandler.logException(ex, "", "");
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            if (!bSelection)
                Search();
        }

        private void btnResubmit_Click(object sender, EventArgs e)
        {
            ReSubmitOrder();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (grdSearch.Rows.Count < 1)
                return;
            //isInComplete = true;
            //this.Edit();
            try
            {
                Delete();
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "btnDelete_Click()");
            }
        }

        //Added By Shitaljit(QuicSolv) 23 May 2011
        private void btnUnlock_Click(object sender, EventArgs e)
        {
            if (grdSearch.Rows.Count < 1)
                return;
            try
            {
                unLockUser();
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "btnUnlock_Click()");
                clsUIHelper.ShowErrorMsg(ex.Message);
            }
        }

        private void btnResetPass_Click(object sender, EventArgs e)
        {
            if (grdSearch.Rows.Count < 1)
                return;
            try
            {
                logger.Trace("btnResetPass_Click() - " + clsPOSDBConstants.Log_Entering);
                if (this.btnResetPass.Text == "&Merge Card")
                {
                    MergeCLCard();
                }
                else
                {
                    resetUserPassword();
                }
                logger.Trace("btnResetPass_Click() - " + clsPOSDBConstants.Log_Exiting);
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "btnResetPass_Click()");
                clsUIHelper.ShowErrorMsg(ex.Message);
            }
        }
        //till here added By Shitaljit

        private void btnImportCustomerFromPrimeRX_Click(object sender, EventArgs e)
        {
            frmCustomerImportFromRX oCustomerImport = new frmCustomerImportFromRX();
            //oCustomerImport.Owner = this;
            if (oCustomerImport.ShowDialog(this) == DialogResult.OK)
            {
                Search();
            }
        }

        //Following Code Added by Krishna on 30 November 2011
        private void btnAdvSearch_Click(object sender, EventArgs e)
        {
            FrmAdvSearch.ShowDialog();
            if (!FrmAdvSearch.IsCanceled)
            {
                this.grdSearch.DataSource = FrmAdvSearch.itemData;
                //sbMain.Panels[0].Text = "Record(s) Count = " + FrmAdvSearch.itemData.Tables[0].Rows.Count.ToString();
                IsAdvSearchDone = true;
            }
        }

        private void txtMasterSearchVal_KeyDown(object sender, KeyEventArgs e)
        {
            if (string.IsNullOrEmpty(this.txtMasterSearchVal.Text) == false && e.KeyData == Keys.Enter)
            {
                //Search();
                this.txtMasterSearchVal.Focus();
                this.txtMasterSearchVal.SelectionStart = Configuration.convertNullToInt(this.txtMasterSearchVal.Text.Length);
                this.txtCode.Enabled = false;
                this.txtName.Enabled = false;
                this.txtContactNumber.Enabled = false;

                #region PRIMEPOS-2645 06-Mar-2019 JY Added
                cboExpDate.Enabled = false;
                cboExpDate.SelectedIndex = 0;
                cboDOB.Enabled = false;
                cboDOB.SelectedIndex = 0;
                #endregion
            }
            else
            {
                this.txtCode.Enabled = true;
                this.txtName.Enabled = true;
                if (bSelection == true) this.txtContactNumber.Enabled = true;
                cboExpDate.Enabled = true; //PRIMEPOS-2645 06-Mar-2019 JY Added
                cboDOB.Enabled = true; //PRIMEPOS-2645 06-Mar-2019 JY Added
            }
        }

        private void txtMasterSearchVal_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.txtMasterSearchVal.Text) == false)
            {
                this.txtMasterSearchVal.Focus();
                this.txtMasterSearchVal.SelectionStart = Configuration.convertNullToInt(this.txtMasterSearchVal.Text.Length);
                this.txtCode.Enabled = false;
                this.txtName.Enabled = false;
                this.txtContactNumber.Enabled = false;

                #region PRIMEPOS-2645 06-Mar-2019 JY Added
                cboExpDate.Enabled = false;
                cboExpDate.SelectedIndex = 0;
                cboDOB.Enabled = false;
                cboDOB.SelectedIndex = 0;
                #endregion
            }
            else
            {
                this.txtCode.Enabled = true;
                this.txtName.Enabled = true;
                if (bSelection == true) this.txtContactNumber.Enabled = true;
                cboExpDate.Enabled = true; //PRIMEPOS-2645 06-Mar-2019 JY Added
                cboDOB.Enabled = true; //PRIMEPOS-2645 06-Mar-2019 JY Added
            }
        }

        #region Sprint-21 - 1272 27-Aug-2015 JY Added
        private void btnReceiptDescInOL_Click(object sender, EventArgs e)
        {
            frmOtherLanguageDesc ofrmOtherLanguageDesc = new frmOtherLanguageDesc();
            ofrmOtherLanguageDesc.ShowDialog(this);
        }
        #endregion

        #region Sprint-24 - PRIMEPOS-2292 27-Jan-2017 JY Added
        private void btnViewDeptList_Click(object sender, EventArgs e)
        {
            logger.Trace("btnViewDeptList_Click() - " + clsPOSDBConstants.Log_Entering);
            FormCollection fc = Application.OpenForms;
            foreach (Form oFrm in fc)
            {
                if (oFrm.GetType() == typeof(frmRptDeptList))
                {
                    oFrm.Dispose();
                    break;
                }
            }

            frmRptDeptList ofrmRptDeptList = new frmRptDeptList();
            ofrmRptDeptList.Show();
            logger.Trace("btnViewDeptList_Click() - " + clsPOSDBConstants.Log_Exiting);
        }
        #endregion

        private void radioFor7Days_CheckedChanged(object sender, EventArgs e)
        {
            oBLSearch.IsFor7Days = true;
            Search();
        }

        private void radioFlagged_CheckedChanged(object sender, EventArgs e)
        {
            oBLSearch.IsFor7Days = false;
            Search();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            #region PRIMEPOS-2671 22-Apr-2019 JY Added
            if (SearchTable == clsPOSDBConstants.MMSSearch && SelectedRowID() == string.Empty)
            {
                Resources.Message.Display("Please select any record", "PrimePOS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            #endregion

            IsCanceled = false;
            this.Close();
        }

        private void chkSelectAll_CheckedChanged(object sender, EventArgs e)
        {
            logger.Trace("chkSelectAll_CheckedChanged() - " + clsPOSDBConstants.Log_Entering);
            grdSearch.BeginUpdate();
            foreach (UltraGridRow oRow in grdSearch.Rows)
            {
                oRow.Cells["check"].Value = chkSelectAll.Checked;
                oRow.Update();
            }
            grdSearch.EndUpdate();
            logger.Trace("chkSelectAll_CheckedChanged() - " + clsPOSDBConstants.Log_Exiting);
        }

        #region Sprint-26 - PRIMEPOS-2417 06-Jul-2017 JY Added
        private void btnCopy_Click(object sender, EventArgs e)
        {
            logger.Trace("btnCopy_Click() - " + clsPOSDBConstants.Log_Entering);
            if (SelectedRowID() == string.Empty)
            {
                Resources.Message.Display("Please select any record", "Copy Item...");
                return;
            }
            IsCanceled = false;
            this.Close();
            logger.Trace("btnCopy_Click() - " + clsPOSDBConstants.Log_Exiting);
        }
        #endregion

        private void grdSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Space)
            {
                if (grdSearch.ActiveRow != null)
                {
                    CheckUncheckGridRow(this.grdSearch.ActiveRow.Cells["check"]);
                }
            }
        }

        private void grdSearch_MouseClick(object sender, MouseEventArgs e)
        {
            if (this.allowMultiRowSelect == true)
            {

                Point point = System.Windows.Forms.Cursor.Position;
                point = this.grdSearch.PointToClient(point);
                Infragistics.Win.UIElement oUI = this.grdSearch.DisplayLayout.UIElement.ElementFromPoint(point, Infragistics.Win.UIElementInputType.MouseClick);
                if (oUI == null)
                    return;

                while (oUI != null)
                {
                    if (oUI.GetType() == typeof(Infragistics.Win.UltraWinGrid.CellUIElement))
                    {
                        Infragistics.Win.UltraWinGrid.CellUIElement cellUIElement = (Infragistics.Win.UltraWinGrid.CellUIElement)oUI;
                        if (cellUIElement.Column.Key.ToUpper() == "CHECK")
                        {
                            CheckUncheckGridRow(cellUIElement.Cell);
                        }
                        break;
                    }
                    oUI = oUI.Parent;
                }
            }
        }
        #endregion

        #region Methods
        private void SetControlsVisibility()
        {
            logger.Trace("SetControlsVisibility() - " + clsPOSDBConstants.Log_Entering);
            if (this.bSelection == false)
            {
                #region Controls visibility for record listing
                //these controls are visible false by default
                //pnlAdvSearch.Visible = lbl3.Visible = txtMasterSearchVal.Visible = chkIncludeRXCust.Visible = false;
                //chkDispInActiveUsers.Visible = chkPrimtUserImg.Visible = false;
                //pnlFillPartialOrder.Visible = pnlAckManual.Visible = pnlResubmit.Visible = false;
                //pnlViewDeptList.Visible = pnlImportCustomerFromPrimeRX.Visible = btnReceiptDescInOL.Visible = false;
                //pnlFillPartialOrder.Visible = pnlAckManual.Visible = pnlResubmit.Visible = false;
                //pnlRefresh.Visible = pnlCopyOrder.Visible = false;
                //btnPrint.Visible = btnUnlock.Visible = btnResetPass.Visible = false;

                this.lbl1.Text = this.LabelText1;
                this.lbl2.Text = this.LabelText2;

                //set grid height                
                if (SearchTable == clsPOSDBConstants.POHeader_tbl)
                {
                    tlpControls.RowStyles[0].Height = 20;
                    tlpControls.RowStyles[1].Height = 61;
                    tlpControls.RowStyles[2].Height = 19;
                }
                else if (SearchTable == clsPOSDBConstants.Customer_tbl) //PRIMEPOS-2613 05-Dec-2018 JY modified 
                {
                    tlpControls.RowStyles[0].Height = 26;
                    tlpControls.RowStyles[1].Height = 64;
                    tlpControls.RowStyles[2].Height = 10;
                }
                else if (SearchTable == clsPOSDBConstants.Users_tbl)
                {
                    tlpControls.RowStyles[0].Height = 20;
                    tlpControls.RowStyles[1].Height = 70;
                    tlpControls.RowStyles[2].Height = 10;
                }
                else
                {
                    tlpControls.RowStyles[0].Height = 15;
                    tlpControls.RowStyles[1].Height = 75;
                    tlpControls.RowStyles[2].Height = 10;
                }

                if (SearchTable != clsPOSDBConstants.POHeader_tbl)
                {
                    pnlAdd.Top = pnlEdit.Top = pnlDelete.Top = pnlImportCustomerFromPrimeRX.Top = btnReceiptDescInOL.Top = pnlPrint.Top = pnlClose.Top = pnlViewDeptList.Top;
                }

                lbl2.Left = txtCode.Left + txtCode.Width + 10;
                txtName.Left = lbl2.Left + lbl2.Width + 10;

                if (SearchTable == clsPOSDBConstants.POHeader_tbl)
                {
                    lbl1.Visible = txtCode.Visible = lbl2.Visible = txtName.Visible = cboAddEditPOStatusList.Visible = pnlSearch.Visible = true;
                    radioFlagged.Visible = radioFor7Days.Visible = true;

                    this.pnlAdd.Visible = UserPriviliges.IsUserHasPriviliges(UserPriviliges.Modules.InventoryMgmt.ID, UserPriviliges.Screens.PurchaseOrder.ID, -999);
                    this.pnlEdit.Visible = UserPriviliges.IsUserHasPriviliges(UserPriviliges.Modules.InventoryMgmt.ID, UserPriviliges.Screens.PurchaseOrder.ID, -998);
                    pnlDelete.Visible = true;
                    if (pnlAdd.Visible == false && pnlEdit.Visible == true) pnlEdit.Left = pnlAdd.Left;
                    pnlFillPartialOrder.Visible = pnlAckManual.Visible = pnlResubmit.Visible = true;
                    pnlRefresh.Visible = pnlCopyOrder.Visible = true;

                    cboAddEditPOStatusList.SelectedIndex = (int)PurchseOrdStatus.All;
                    btnAdd.Text = "Create &New";
                    btnEdit.Text = "&Edit Incomplete PO";
                    this.btnDelete.Text = "Delete Order";
                    EnableOrDisable(clsPOSDBConstants.Incomplete);

                    pnlAdd.Width = 170;
                    this.pnlEdit.Left = pnlAdd.Left + pnlAdd.Width + 10;
                    this.pnlEdit.Width = 200;
                    this.pnlDelete.Left = this.pnlEdit.Left;
                    this.pnlDelete.Top = pnlResubmit.Top;
                    this.pnlDelete.Width = 200;
                    this.pnlCopyOrder.TabIndex = this.pnlPrint.TabIndex;
                    this.pnlRefresh.Left = this.pnlFillPartialOrder.Left;
                    this.pnlClose.Left = this.pnlResubmit.Left;

                    this.pnlDelete.Enabled = false;//Added by Ravindra for Delete Incomplete PO  
                }
                else if (SearchTable == clsPOSDBConstants.Customer_tbl)
                {
                    lbl1.Visible = txtCode.Visible = lbl2.Visible = txtName.Visible = pnlSearch.Visible = true;
                    lbl3.Visible = txtMasterSearchVal.Visible = chkIncludeRXCust.Visible = chkNoStoreCard.Visible = true;//PRIMEPOS-2896
                    this.pnlAdd.Visible = UserPriviliges.IsUserHasPriviliges(UserPriviliges.Modules.POSTransaction.ID, UserPriviliges.Screens.Customers.ID, -999);
                    this.pnlEdit.Visible = UserPriviliges.IsUserHasPriviliges(UserPriviliges.Modules.POSTransaction.ID, UserPriviliges.Screens.Customers.ID, -998);
                    pnlDelete.Visible = UserPriviliges.IsUserHasPriviliges(UserPriviliges.Modules.AdminFunc.ID, UserPriviliges.Screens.DeleteCustomer.ID, 0);
                    btnReceiptDescInOL.Visible = true;  //Sprint-21 - 1272 27-Aug-2015 JY Added 
                    pnlClear.Visible = true;    //PRIMEPOS-2475 07-Jun-2018 JY Added
                    if (pnlAdd.Visible == true && pnlEdit.Visible == true && Configuration.CPOSSet.UsePrimeRX == true)  //PRIMEPOS-3106 22-Jun-2022 JY Added UsePrimeRX check
                    {
                        pnlImportCustomerFromPrimeRX.Visible = true;
                    }

                    //row 1                    
                    this.txtMasterSearchVal.Left = this.txtCode.Left;
                    chkIncludeRXCust.Left = txtMasterSearchVal.Left + txtMasterSearchVal.Width + 10;  //PRIMEPOS-2475 08-Jun-2018 JY Added                    
                    this.chkIncludeRXCust.Top = lbl3.Top - 1;   //PRIMEPOS-2475 08-Jun-2018 JY Added

                    //chkNoStoreCard.Left = chkIncludeRXCust.Left + chkIncludeRXCust.Width + 10;  //PRIMEPOS-2896
                    //this.chkNoStoreCard.Top = lbl3.Top - 1;   //PRIMEPOS-2896 

                    //row 2
                    this.txtCode.Top = this.txtMasterSearchVal.Top + this.txtMasterSearchVal.Height + 5;    //PRIMEPOS-2896 23-Sep-2020 JY Modified
                    this.txtName.Top = this.txtCode.Top;
                    this.lbl1.Top = this.txtCode.Top + 5;
                    this.lbl2.Top = this.lbl1.Top;
                    this.txtName.Left += 5;

                    #region PRIMEPOS-2645 05-Mar-2019 JY Added
                    this.lblDOB.Visible = cboDOB.Visible = true;
                    this.lblDOB.Top = this.lbl1.Top;
                    this.cboDOB.Top = this.dtpDateOfBirth1.Top = this.dtpDateOfBirth2.Top = this.txtCode.Top;
                    this.lblDOB.Left = chkIncludeRXCust.Left;
                    this.cboDOB.Left = this.lblDOB.Left + this.lblDOB.Width + 10;
                    this.dtpDateOfBirth1.Left = this.cboDOB.Left + this.cboDOB.Width + 5;
                    this.dtpDateOfBirth2.Left = this.dtpDateOfBirth1.Left + this.dtpDateOfBirth1.Width + 5;
                    #endregion

                    //row 3
                    #region PRIMEPOS-2613 05-Dec-2018 JY Added
                    this.lblExpDate.Visible = this.cboExpDate.Visible = true;
                    this.cboExpDate.Top = this.txtName.Top + this.txtName.Height + 5;   //PRIMEPOS-2896 23-Sep-2020 JY Modified
                    this.lblExpDate.Top = this.cboExpDate.Top + 5;
                    this.dtpExpDate1.Top = this.dtpExpDate2.Top = this.cboExpDate.Top;
                    this.lblExpDate.Left = this.lbl3.Left;
                    this.cboExpDate.Left = this.txtMasterSearchVal.Left;
                    this.dtpExpDate1.Left = this.cboExpDate.Left + this.cboExpDate.Width + 10;
                    this.dtpExpDate2.Left = this.dtpExpDate1.Left + this.dtpExpDate1.Width + 10;
                    #endregion
                    this.pnlSearch.Top = this.cboExpDate.Top;   //PRIMEPOS-2896 23-Sep-2020 JY Modified
                    this.pnlClear.Top = this.pnlSearch.Top; //PRIMEPOS-2475 07-Jun-2018 JY Added
                    this.pnlClear.Left = this.pnlSearch.Left - this.pnlClear.Width - 10; //PRIMEPOS-2475 07-Jun-2018 JY Added     

                    this.chkNoStoreCard.Left = this.cboExpDate.Left;    //PRIMEPOS-2896 23-Sep-2020 JY Added
                    this.chkNoStoreCard.Top = this.cboExpDate.Top + this.cboExpDate.Height + 5; //PRIMEPOS-2896 23-Sep-2020 JY Added

                    this.pnlCustTokenize.Top = this.pnlClear.Top; //PRIMEPOS-2896
                    this.pnlCustTokenize.Left = this.pnlClear.Left - this.pnlCustTokenize.Width - 10;

                    //bottom buttons
                    if (pnlAdd.Visible == false && pnlEdit.Visible == true) pnlEdit.Left = pnlAdd.Left;
                    this.pnlClose.Left = this.pnlResubmit.Left;
                    this.pnlClose.Top = this.pnlResubmit.Top;
                    btnReceiptDescInOL.Left = pnlImportCustomerFromPrimeRX.Left + pnlImportCustomerFromPrimeRX.Width + 10;

                    this.txtMasterSearchVal.TabIndex = 0;
                    this.chkIncludeRXCust.TabIndex = 1;
                    this.txtName.TabIndex = 3;
                    this.txtCode.TabIndex = 2;
                    this.txtContactNumber.TabIndex = 4;
                    this.cboExpDate.TabIndex = 5;
                    this.dtpExpDate1.TabIndex = 6;
                    this.dtpExpDate2.TabIndex = 7;
                    this.cboDOB.TabIndex = 8;
                    this.dtpDateOfBirth1.TabIndex = 9;
                    this.dtpDateOfBirth2.TabIndex = 10;
                    this.pnlClear.TabIndex = 13;
                    this.pnlSearch.TabIndex = 14;
                    #region PRIMEPOS-2896
                    this.chkNoStoreCard.TabIndex = 11;
                    this.pnlCustTokenize.TabIndex = 12;
                    #endregion
                }
                else if (SearchTable == clsPOSDBConstants.Item_tbl)
                {
                    this.pnlAdd.Visible = UserPriviliges.IsUserHasPriviliges(UserPriviliges.Modules.InventoryMgmt.ID, UserPriviliges.Screens.ItemFile.ID, -999);
                    this.pnlEdit.Visible = UserPriviliges.IsUserHasPriviliges(UserPriviliges.Modules.InventoryMgmt.ID, UserPriviliges.Screens.ItemFile.ID, -998);
                    pnlDelete.Visible = UserPriviliges.IsUserHasPriviliges(UserPriviliges.Modules.AdminFunc.ID, UserPriviliges.Screens.DeleteItem.ID, 0);
                    if (pnlAdd.Visible == false && pnlEdit.Visible == true) pnlEdit.Left = pnlAdd.Left;

                    pnlAdvSearch.Visible = pnlPrint.Visible = true;
                    btnReceiptDescInOL.Visible = true;  //Sprint-21 - 1272 27-Aug-2015 JY Added
                    pnlExport.Visible = true;   //PRIMEPOS-2779 17-Jan-2020 JY Added

                    this.pnlSearch.Left = pnlAdvSearch.Left - pnlSearch.Width - 10;

                    pnlPrint.Left = pnlClose.Left - pnlPrint.Width - 10;
                    btnReceiptDescInOL.Left = pnlPrint.Left - btnReceiptDescInOL.Width - 10;
                    #region PRIMEPOS-2779 17-Jan-2020 JY Added
                    pnlExport.Top = pnlAdd.Top;
                    if (pnlDelete.Visible == true)
                        pnlExport.Left = pnlDelete.Left + pnlDelete.Width + 10;
                    else if (pnlEdit.Visible == true)
                        pnlExport.Left = pnlEdit.Left + pnlEdit.Width + 10;
                    else
                        pnlExport.Left = pnlAdd.Left + pnlAdd.Width + 10;
                    #endregion
                    this.txtCode.Select();
                    this.txtCode.Focus();
                }
                else if (SearchTable == clsPOSDBConstants.Users_tbl)
                {
                    pnlDelete.Visible = UserPriviliges.IsUserHasPriviliges(UserPriviliges.Modules.AdminFunc.ID, UserPriviliges.Screens.DeleteUser.ID, 0);
                    this.chkPrimtUserImg.Visible = this.chkDispInActiveUsers.Visible = true;
                    this.pnlPrint.Visible = this.pnlResetPass.Visible = this.pnlUnlock.Visible = true;
                    this.pnlPrint.Top = this.pnlResetPass.Top = this.pnlUnlock.Top = this.pnlClose.Top = pnlAdd.Top;

                    chkDispInActiveUsers.Left = txtCode.Left;
                    chkPrimtUserImg.Left = txtName.Left;
                    pnlPrint.Left = pnlResetPass.Left + pnlResetPass.Width + 10;
                    pnlUnlock.Left = pnlPrint.Right + 10;   //PRIMEPOS-2780 27-Sep-2021 JY modified
                }
                else if (SearchTable.Equals(clsPOSDBConstants.UsersGroup_tbl) == true)
                {
                    pnlDelete.Visible = UserPriviliges.IsUserHasPriviliges(UserPriviliges.Modules.AdminFunc.ID, UserPriviliges.Screens.DeleteUser.ID, 0);
                    //SearchUserGruop();
                }
                else if (SearchTable == clsPOSDBConstants.Department_tbl)
                {
                    this.pnlAdd.Visible = UserPriviliges.IsUserHasPriviliges(UserPriviliges.Modules.AdminFunc.ID, UserPriviliges.Screens.Department.ID, -999);
                    this.pnlEdit.Visible = UserPriviliges.IsUserHasPriviliges(UserPriviliges.Modules.AdminFunc.ID, UserPriviliges.Screens.Department.ID, -998);
                    pnlDelete.Visible = UserPriviliges.IsUserHasPriviliges(UserPriviliges.Modules.AdminFunc.ID, UserPriviliges.Screens.DeleteDepartment.ID, 0);
                    if (pnlAdd.Visible == false && pnlEdit.Visible == true) pnlEdit.Left = pnlAdd.Left;
                    pnlViewDeptList.Visible = true; //Sprint-24 - PRIMEPOS-2292 27-Jan-2017 JY Added
                    pnlViewDeptList.Left = pnlDelete.Left + pnlDelete.Width + 10;
                    pnlViewDeptList.Top = pnlDelete.Top;
                }
                else if (SearchTable == clsPOSDBConstants.PayType_tbl)
                {
                    pnlDelete.Visible = UserPriviliges.IsUserHasPriviliges(UserPriviliges.Modules.AdminFunc.ID, UserPriviliges.Screens.ManagePayType.ID, 0);
                    SearchSvr.ISManagePaytype = false;   //Sprint-23 - PRIMEPOS-2255 18-May-2016 JY set it to false to display old paytypes as well
                }
                else if (SearchTable == clsPOSDBConstants.item_PriceInv_Lookup)
                {
                    //if (this.isReadonly == true)
                    //{
                    this.pnlAdd.Visible = this.pnlEdit.Visible = this.pnlDelete.Visible = false;
                    //}
                }
                else if (SearchTable == clsPOSDBConstants.StationCloseHeader_tbl)
                {
                    int itop = 0;
                    tabMain.Visible = false;
                    itop = this.grdSearch.Top;
                    this.grdSearch.Top = tabMain.Top;
                    this.grdSearch.Height = this.grdSearch.Height + (itop - this.grdSearch.Top);
                    pnlAdd.Visible = false;
                    pnlEdit.Left = pnlAdd.Left;
                    btnEdit.Text = "View";
                }
                else if (SearchTable == clsPOSDBConstants.EndOfDay_tbl)
                {
                    int itop = 0;
                    tabMain.Visible = false;
                    itop = this.grdSearch.Top;
                    this.grdSearch.Top = tabMain.Top;
                    this.grdSearch.Height = this.grdSearch.Height + (itop - this.grdSearch.Top);
                    pnlAdd.Visible = false;
                    pnlEdit.Left = pnlAdd.Left;
                    btnEdit.Text = "View";
                }
                else if (SearchTable == clsPOSDBConstants.CLCards_tbl)
                {
                    this.pnlAdd.Visible = UserPriviliges.IsUserHasPriviliges(UserPriviliges.Modules.AdminFunc.ID, UserPriviliges.Screens.CustomerLoyaltyCards.ID, -999);
                    this.pnlEdit.Visible = UserPriviliges.IsUserHasPriviliges(UserPriviliges.Modules.AdminFunc.ID, UserPriviliges.Screens.CustomerLoyaltyCards.ID, -998);
                    this.pnlDelete.Visible = UserPriviliges.IsUserHasPriviliges(UserPriviliges.Modules.AdminFunc.ID, UserPriviliges.Screens.CustomerLoyaltyCards.ID, -999);
                    if (pnlAdd.Visible == false && pnlEdit.Visible == true) pnlEdit.Left = pnlAdd.Left;

                    this.pnlResetPass.Visible = true;
                    lbl1.Text = "CL Card #";
                    lbl2.Text = "CL Card Name";
                    txtName.Left = lbl2.Left + lbl2.Width + 10;

                    this.btnDelete.Text = "&Deactivate";
                    this.pnlDelete.Width = 160;
                    //this.btnDelete.Location = new Point(218, 481);
                    this.btnResetPass.Text = "&Merge Card";
                    this.pnlResetPass.Width = 160;
                    this.pnlResetPass.Left = this.pnlDelete.Left + this.pnlDelete.Width + 10;
                    this.pnlResetPass.Top = this.pnlAdd.Top;
                }
                else if (SearchTable == clsPOSDBConstants.CLPointsRewardTier_tbl)
                {
                    this.pnlAdd.Visible = UserPriviliges.IsUserHasPriviliges(UserPriviliges.Modules.AdminFunc.ID, UserPriviliges.Screens.PointsRewardTier.ID, -999);
                    this.pnlEdit.Visible = UserPriviliges.IsUserHasPriviliges(UserPriviliges.Modules.AdminFunc.ID, UserPriviliges.Screens.PointsRewardTier.ID, -998);
                    if (pnlAdd.Visible == false && pnlEdit.Visible == true) pnlEdit.Left = pnlAdd.Left;
                }
                else if (SearchTable == clsPOSDBConstants.ItemMonitorCategory_tbl)
                {
                    this.pnlAdd.Visible = UserPriviliges.IsUserHasPriviliges(UserPriviliges.Modules.InventoryMgmt.ID, UserPriviliges.Screens.ItemFile.ID, -999);
                    this.pnlEdit.Visible = UserPriviliges.IsUserHasPriviliges(UserPriviliges.Modules.InventoryMgmt.ID, UserPriviliges.Screens.ItemFile.ID, -998);
                    if (pnlAdd.Visible == false && pnlEdit.Visible == true) pnlEdit.Left = pnlAdd.Left;
                }
                //else if (SearchTable == clsPOSDBConstants.InvRecvHeader_tbl)
                //{
                //    this.pnlAdd.Visible = UserPriviliges.IsUserHasPriviliges(UserPriviliges.Modules.InventoryMgmt.ID, UserPriviliges.Screens.InventoryRecvd.ID, -999);
                //    this.pnlEdit.Visible = UserPriviliges.IsUserHasPriviliges(UserPriviliges.Modules.InventoryMgmt.ID, UserPriviliges.Screens.InventoryRecvd.ID, -998);
                //    if (pnlAdd.Visible == false && pnlEdit.Visible == true) pnlEdit.Left = pnlAdd.Left;
                //}
                else if (SearchTable == clsPOSDBConstants.Vendor_tbl)
                {
                    this.pnlAdd.Visible = UserPriviliges.IsUserHasPriviliges(UserPriviliges.Modules.InventoryMgmt.ID, UserPriviliges.Screens.VendorFile.ID, -999);
                    this.pnlEdit.Visible = UserPriviliges.IsUserHasPriviliges(UserPriviliges.Modules.InventoryMgmt.ID, UserPriviliges.Screens.VendorFile.ID, -998);
                    pnlDelete.Visible = UserPriviliges.IsUserHasPriviliges(UserPriviliges.Modules.AdminFunc.ID, UserPriviliges.Screens.DeleteVendor.ID, 0);
                    if (pnlAdd.Visible == false && pnlEdit.Visible == true) pnlEdit.Left = pnlAdd.Left;
                    txtCode.Left = lbl1.Left + lbl1.Width + 10;
                    lbl2.Left = txtCode.Left + txtCode.Width + 20;
                    txtName.Left = lbl2.Left + lbl2.Width + 10;
                }
                else if (SearchTable == clsPOSDBConstants.InvTransType_tbl)
                {
                    this.pnlAdd.Visible = UserPriviliges.IsUserHasPriviliges(UserPriviliges.Modules.InventoryMgmt.ID, UserPriviliges.Screens.InvTransType.ID, -999);
                    this.pnlEdit.Visible = UserPriviliges.IsUserHasPriviliges(UserPriviliges.Modules.InventoryMgmt.ID, UserPriviliges.Screens.InvTransType.ID, -998);
                    if (pnlAdd.Visible == false && pnlEdit.Visible == true) pnlEdit.Left = pnlAdd.Left;
                }
                else if (SearchTable == clsPOSDBConstants.TaxCodes_tbl)
                {
                    this.pnlAdd.Visible = UserPriviliges.IsUserHasPriviliges(UserPriviliges.Modules.AdminFunc.ID, UserPriviliges.Screens.TaxCodes.ID, -999);
                    this.pnlEdit.Visible = UserPriviliges.IsUserHasPriviliges(UserPriviliges.Modules.AdminFunc.ID, UserPriviliges.Screens.TaxCodes.ID, -998);
                    pnlDelete.Visible = UserPriviliges.IsUserHasPriviliges(UserPriviliges.Modules.AdminFunc.ID, UserPriviliges.Screens.DeleteTaxCode.ID, 0);
                    this.pnlClear.Visible = true;
                    this.chkIncludeInActiveTax.Visible = UserPriviliges.IsUserHasPriviliges(UserPriviliges.Modules.AdminFunc.ID, UserPriviliges.Screens.DeleteTaxCode.ID, 0);//2664
                    this.lblClear.Visible = this.btnClear.Visible = false;
                    if (pnlAdd.Visible == false && pnlEdit.Visible == true) pnlEdit.Left = pnlAdd.Left;
                }
                else if (SearchTable == clsPOSDBConstants.WarningMessages_tbl)
                {
                    this.pnlAdd.Visible = UserPriviliges.IsUserHasPriviliges(UserPriviliges.Modules.InventoryMgmt.ID, UserPriviliges.Screens.WarningMessages.ID, -999);
                    this.pnlEdit.Visible = UserPriviliges.IsUserHasPriviliges(UserPriviliges.Modules.InventoryMgmt.ID, UserPriviliges.Screens.WarningMessages.ID, -998);
                    if (pnlAdd.Visible == false && pnlEdit.Visible == true) pnlEdit.Left = pnlAdd.Left;
                }
                else if (SearchTable == clsPOSDBConstants.TransFee_tbl) //PRIMEPOS-3116 11-Jul-2022 JY Added
                {
                    pnlDelete.Visible = true;
                }
                #endregion
            }
            else
            {
                pnlOk.Visible = true;
                pnlAdd.Top = pnlEdit.Top = pnlClose.Top = pnlOk.Top = pnlViewDeptList.Top;

                //set grid height                
                if (SearchTable == clsPOSDBConstants.Customer_tbl)
                {
                    tlpControls.RowStyles[0].Height = 26;
                    tlpControls.RowStyles[1].Height = 64;
                    tlpControls.RowStyles[2].Height = 10;
                }
                else if (SearchTable == clsPOSDBConstants.PrimeRX_HouseChargeInterface || SearchTable == clsPOSDBConstants.PrimeRX_PatientInterface)
                {
                    tlpControls.RowStyles[0].Height = 20;
                    tlpControls.RowStyles[1].Height = 70;
                    tlpControls.RowStyles[2].Height = 10;
                }
                else if (this.SearchTable == clsPOSDBConstants.VendorCostPrice_View)
                {
                    tlpControls.RowStyles[0].Height = 0;
                    tlpControls.RowStyles[1].Height = 90;
                    tlpControls.RowStyles[2].Height = 10;
                }
                else
                {
                    tlpControls.RowStyles[0].Height = 15;
                    tlpControls.RowStyles[1].Height = 75;
                    tlpControls.RowStyles[2].Height = 10;
                }

                if (SearchTable == clsPOSDBConstants.Customer_tbl) //frmCustomerSearch screen
                {
                    lbl1.Visible = txtCode.Visible = lbl2.Visible = txtName.Visible = pnlSearch.Visible = true;
                    lbl3.Visible = txtMasterSearchVal.Visible = chkIncludeRXCust.Visible = chkNoStoreCard.Visible = true;//PRIMEPOS-2896
                    this.pnlAdd.Visible = UserPriviliges.IsUserHasPriviliges(UserPriviliges.Modules.POSTransaction.ID, UserPriviliges.Screens.Customers.ID, -999);
                    this.pnlEdit.Visible = UserPriviliges.IsUserHasPriviliges(UserPriviliges.Modules.POSTransaction.ID, UserPriviliges.Screens.Customers.ID, -998);
                    pnlDelete.Visible = UserPriviliges.IsUserHasPriviliges(UserPriviliges.Modules.AdminFunc.ID, UserPriviliges.Screens.DeleteCustomer.ID, 0);
                    pnlClear.Visible = true;    //PRIMEPOS-2475 07-Jun-2018 JY Added       
                    btnReceiptDescInOL.Visible = true;  //Sprint-21 - 1272 27-Aug-2015 JY Added 
                    if (pnlAdd.Visible == true && pnlEdit.Visible == true && Configuration.CPOSSet.UsePrimeRX == true)  //PRIMEPOS-3106 22-Jun-2022 JY Added UsePrimeRX check
                        pnlImportCustomerFromPrimeRX.Visible = true;
                    this.lblContact.Visible = this.txtContactNumber.Visible = true;

                    //Row 1
                    this.txtMasterSearchVal.Width += 15;
                    this.chkIncludeRXCust.Top = lbl3.Top - 1;   //PRIMEPOS-2613 05-Dec-2018 JY Added
                    this.chkIncludeRXCust.Left = this.txtMasterSearchVal.Left + this.txtMasterSearchVal.Width + 10;

                    this.txtContactNumber.Top = this.txtMasterSearchVal.Top;  //PRIMEPOS-2613 05-Dec-2018 JY Added
                    this.lblContact.Top = this.txtContactNumber.Top + 5;
                    this.lblContact.Left = this.chkIncludeRXCust.Left + this.chkIncludeRXCust.Width + 10; //PRIMEPOS-2613 05-Dec-2018 JY Modified
                    this.txtContactNumber.Left = this.lblContact.Left + this.lblContact.Width + 10;  //PRIMEPOS-2613 05-Dec-2018 JY Modified

                    //Row 2
                    this.txtCode.Top = this.txtMasterSearchVal.Top + this.txtMasterSearchVal.Height + 5;    //PRIMEPOS-2896 23-Sep-2020 JY Modified
                    this.txtName.Top = this.txtCode.Top;
                    this.txtName.Left = this.txtMasterSearchVal.Left;

                    this.lbl1.Text = "&Name";
                    this.lbl2.Text = "Account #";
                    this.lbl1.Top = this.txtCode.Top + 5;
                    this.lbl2.Top = this.lbl1.Top;
                    this.lbl2.Left = this.txtName.Left + this.txtName.Width + 10;
                    this.txtCode.Left = this.lbl2.Left + this.lbl2.Width + 10;

                    #region PRIMEPOS-2645 05-Mar-2019 JY Added
                    this.lblDOB.Visible = cboDOB.Visible = true;
                    this.lblDOB.Top = this.lbl1.Top;
                    this.cboDOB.Top = this.dtpDateOfBirth1.Top = this.dtpDateOfBirth2.Top = this.txtCode.Top;
                    this.lblDOB.Left = chkIncludeRXCust.Left;
                    this.cboDOB.Left = this.lblDOB.Left + this.lblDOB.Width + 10;
                    this.dtpDateOfBirth1.Left = this.cboDOB.Left + this.cboDOB.Width + 5;
                    this.dtpDateOfBirth2.Left = this.dtpDateOfBirth1.Left + this.dtpDateOfBirth1.Width + 5;
                    #endregion

                    //Row 3
                    #region PRIMEPOS-2613 05-Dec-2018 JY Added
                    this.lblExpDate.Visible = this.cboExpDate.Visible = true;
                    this.cboExpDate.Top = this.txtCode.Top + this.txtCode.Height + 5;   //PRIMEPOS-2896 23-Sep-2020 JY Modified
                    this.lblExpDate.Top = this.cboExpDate.Top + 5;
                    this.dtpExpDate1.Top = this.dtpExpDate2.Top = this.cboExpDate.Top;
                    this.lblExpDate.Left = this.lbl3.Left;
                    this.cboExpDate.Left = this.txtMasterSearchVal.Left;
                    this.dtpExpDate1.Left = this.cboExpDate.Left + this.cboExpDate.Width + 10;
                    this.dtpExpDate2.Left = this.dtpExpDate1.Left + this.dtpExpDate1.Width + 10;
                    #endregion
                    this.pnlSearch.Top = this.cboExpDate.Top;   //PRIMEPOS-2896 23-Sep-2020 JY Modified
                    this.pnlClear.Top = this.pnlSearch.Top; //PRIMEPOS-2475 07-Jun-2018 JY Added
                    this.pnlClear.Left = this.pnlSearch.Left - this.pnlClear.Width - 10; //PRIMEPOS-2475 07-Jun-2018 JY Added     

                    this.chkNoStoreCard.Left = this.cboExpDate.Left;    //PRIMEPOS-2896 23-Sep-2020 JY Added
                    this.chkNoStoreCard.Top = this.cboExpDate.Top + this.cboExpDate.Height + 5; //PRIMEPOS-2896 23-Sep-2020 JY Added

                    #region PRIMEPOS-2896
                    this.pnlCustTokenize.Top = this.pnlClear.Top;
                    this.pnlCustTokenize.Left = this.pnlClear.Left - this.pnlCustTokenize.Width - 10;
                    #endregion

                    //bottom buttons
                    if (pnlAdd.Visible == false && pnlEdit.Visible == true) pnlEdit.Left = pnlAdd.Left;
                    if (pnlDelete.Visible) pnlDelete.Top = pnlAdd.Top;

                    this.txtMasterSearchVal.TabIndex = 0;
                    this.chkIncludeRXCust.TabIndex = 1;
                    this.txtName.TabIndex = 2;
                    this.txtCode.TabIndex = 3;
                    this.txtContactNumber.TabIndex = 4;
                    this.cboExpDate.TabIndex = 5;
                    this.dtpExpDate1.TabIndex = 6;
                    this.dtpExpDate2.TabIndex = 7;
                    this.cboDOB.TabIndex = 8;
                    this.dtpDateOfBirth1.TabIndex = 9;
                    this.dtpDateOfBirth2.TabIndex = 10;
                    this.pnlClear.TabIndex = 13;
                    this.pnlSearch.TabIndex = 14;

                    #region PRIMEPOS-2896
                    this.chkNoStoreCard.TabIndex = 11;
                    this.pnlCustTokenize.TabIndex = 12;
                    #endregion

                    this.txtMasterSearchVal.Select();
                    this.txtMasterSearchVal.Focus();
                }
                else //frmSearch screen
                {
                    this.grdSearch.DisplayLayout.Bands[0].Override.SelectTypeCell = SelectType.None;
                    if (allowMultiRowSelect == false)
                    {
                        this.grdSearch.DisplayLayout.Bands[0].Override.SelectTypeRow = SelectType.Single;
                    }
                    else
                    {
                        this.grdSearch.DisplayLayout.Bands[0].Override.SelectTypeRow = SelectType.Extended;
                    }
                    this.chkSelectAll.Visible = this.allowMultiRowSelect;

                    if ((SearchTable != clsPOSDBConstants.MMSSearch) && !string.IsNullOrWhiteSpace(txtCode.Text) || !string.IsNullOrWhiteSpace(txtName.Text) || searchInConstructor)    //PRIMEPOS-2488 22-Jun-2020 JY Added condition to ignore code for MMSSearch
                    {
                        GetFrmSearchData();
                    }
                    if (txtCode.Text == "--##--")
                    {
                        txtCode.Text = "";
                    }

                    DefaultCode = "";

                    if (selectedRowsData.Trim().Length > 0)
                    {
                        string[] IDs = selectedRowsData.Split(new char[] { ',' });
                        for (int i = 0; i < IDs.Length; i++)
                        {
                            string sID = IDs[i].Substring(1, IDs[i].Length - 2);
                            foreach (UltraGridRow gridRow in grdSearch.Rows)
                            {
                                if (gridRow.Cells.Count > 0 && gridRow.Cells[0].Text == sID)
                                {
                                    gridRow.Cells["CHECK"].Value = true;
                                    gridRow.Update();
                                }
                            }
                        }

                        grdSearch.DisplayLayout.Bands[0].SortedColumns.Add("CHECK", true);
                    }

                    if (this.SearchTable == clsPOSDBConstants.VendorCostPrice_View)
                    {
                        this.pnlOk.Visible = false;
                    }
                    else if (SearchTable == clsPOSDBConstants.Item_tbl)
                    {
                        #region Sprint-26 - PRIMEPOS-2417 06-Jul-2017 JY Added
                        if (sCalledFrom.Trim().ToUpper() == "frmItems".ToUpper())
                        {
                            pnlAdd.Visible = pnlEdit.Visible = pnlDelete.Visible = pnlOk.Visible = false;
                            pnlCopy.Visible = true;
                            pnlCopy.Left = pnlClose.Left - pnlCopy.Width - 10;
                            pnlCopy.Top = pnlClose.Top;
                        }
                        #endregion
                        else
                        {
                            this.pnlAdd.Visible = UserPriviliges.IsUserHasPriviliges(UserPriviliges.Modules.InventoryMgmt.ID, UserPriviliges.Screens.ItemFile.ID, -999);
                            this.pnlEdit.Visible = UserPriviliges.IsUserHasPriviliges(UserPriviliges.Modules.InventoryMgmt.ID, UserPriviliges.Screens.ItemFile.ID, -998);
                            pnlDelete.Visible = UserPriviliges.IsUserHasPriviliges(UserPriviliges.Modules.AdminFunc.ID, UserPriviliges.Screens.DeleteItem.ID, 0);
                            if (pnlAdd.Visible == false && pnlEdit.Visible == true) pnlEdit.Left = pnlAdd.Left;
                            if (pnlDelete.Visible) pnlDelete.Top = pnlAdd.Top;
                            pnlAdvSearch.Visible = true;
                            btnReceiptDescInOL.Visible = true;
                            pnlExport.Visible = true;   //PRIMEPOS-2779 17-Jan-2020 JY Added

                            this.pnlSearch.Left = pnlAdvSearch.Left - pnlSearch.Width - 10;
                            btnReceiptDescInOL.Left = pnlClose.Left - btnReceiptDescInOL.Width - 10;
                            pnlExport.Top = pnlAdd.Top; //PRIMEPOS-2779 17-Jan-2020 JY Added
                            pnlExport.Left = pnlDelete.Left + pnlDelete.Width + 10; //PRIMEPOS-2779 17-Jan-2020 JY Added
                        }
                        this.txtCode.Select();
                        this.txtCode.Focus();
                    }
                    else if (SearchTable == clsPOSDBConstants.PrimeRX_HouseChargeInterface || SearchTable == clsPOSDBConstants.PrimeRX_PatientInterface)
                    {
                        //this.txtMasterSearchVal.Width += 15;
                        pnlAdd.Visible = pnlEdit.Visible = false;   //PRIMEPOS-2649 04-Mar-2019 JY Added
                        this.txtCode.Top = this.txtName.Top = this.txtMasterSearchVal.Top + this.txtMasterSearchVal.Height + 5; //PRIMEPOS-2896 23-Sep-2020 JY Modified
                        this.lbl1.Top = this.lbl2.Top = this.txtCode.Top + 5;
                        this.txtCode.Left = this.txtMasterSearchVal.Left;
                        this.lbl2.Left = this.txtCode.Left + this.txtCode.Width + 10;
                        this.txtName.Left = this.lbl2.Left + this.lbl2.Width + 5;

                        //this.lbl1.Text = "&Code";
                        //this.lbl2.Text = "Name";
                        this.txtMasterSearchVal.Visible = true;
                        this.lbl3.Visible = true;
                        this.pnlSearch.Top = txtName.Top - 10;
                        this.txtMasterSearchVal.TabIndex = 0;
                        this.txtMasterSearchVal.Focus();
                    }
                    else if (SearchTable == clsPOSDBConstants.Department_tbl)
                    {
                        pnlAdd.Visible = pnlEdit.Visible = false;
                        if (grdSearch.Rows.Count > 0 && strSetSelected != "")
                        {
                            int count = 0;
                            foreach (UltraGridRow row in grdSearch.Rows)
                            {
                                if (row.Cells[0].Value.ToString() == strSetSelected)
                                {
                                    grdSearch.ActiveRow = grdSearch.Rows[count];
                                    break;
                                }
                                count++;
                            }
                        }
                    }
                    else if (SearchTable == clsPOSDBConstants.TransHeader_tbl)
                    {
                        pnlAdd.Visible = pnlEdit.Visible = false;
                        this.lbl2.Visible = this.txtName.Visible = false;
                    }
                    #region PRIMEPOS-2671 18-Apr-2019 JY Added
                    else if (SearchTable == clsPOSDBConstants.MMSSearch)
                    {
                        pnlAdd.Visible = pnlEdit.Visible = pnlDelete.Visible = false;
                        this.txtCode.Select();
                        this.txtCode.Focus();
                    }
                    #endregion
                    else
                    {
                        if (SearchTable == clsPOSDBConstants.SubDepartment_tbl || SearchTable == clsPOSDBConstants.TaxCodes_With_NoTax || SearchTable == clsPOSDBConstants.Vendor_tbl || SearchTable == clsPOSDBConstants.DescriptionWise)  //PRIMEPOS-2502 26-Apr-2018 JY Added SearchTable == clsPOSDBConstants.DescriptionWise
                        {
                            pnlAdd.Visible = pnlEdit.Visible = false;
                        }

                        if (grdSearch.Rows.Count > 0)
                        {
                            grdSearch.Focus();
                            this.grdSearch.ActiveRow = this.grdSearch.Rows[0];
                        }
                        else
                        {
                            txtCode.Focus();
                        }
                    }
                }
            }
            SetAutoComplete();
            setTitle();
            logger.Trace("SetControlsVisibility() - " + clsPOSDBConstants.Log_Exiting);
        }

        private void SetAutoComplete()
        {
            logger.Trace("SetAutoComplete() - " + clsPOSDBConstants.Log_Entering);
            if (Configuration.CInfo.ShowTextPrediction == true)
            {
                Search oSearch = new Search();
                AutoCompleteStringCollection ItemDescCollection = new AutoCompleteStringCollection();
                switch (SearchTable)
                {
                    case clsPOSDBConstants.Customer_tbl:
                        ItemDescCollection = oSearch.GetAutoCompleteCollectionData(clsPOSDBConstants.Customer_tbl, clsPOSDBConstants.Customer_Fld_CustomerName + "+','+" + clsPOSDBConstants.Customer_Fld_FirstName);
                        this.txtName.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
                        this.txtName.AutoCompleteSource = AutoCompleteSource.CustomSource;
                        this.txtName.AutoCompleteCustomSource = ItemDescCollection;

                        string sSQL = oSearch.GetSearchEngineQuery(clsPOSDBConstants.Customer_tbl);
                        ItemDescCollection = oSearch.GetAutoCompleteCollectionData(clsPOSDBConstants.Customer_tbl, sSQL);
                        this.txtMasterSearchVal.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
                        this.txtMasterSearchVal.AutoCompleteSource = AutoCompleteSource.CustomSource;
                        this.txtMasterSearchVal.AutoCompleteCustomSource = ItemDescCollection;
                        break;
                    case clsPOSDBConstants.Item_tbl:
                        ItemDescCollection = oSearch.GetAutoCompleteCollectionData(clsPOSDBConstants.Item_tbl, clsPOSDBConstants.Item_Fld_Description);
                        this.txtName.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
                        this.txtName.AutoCompleteSource = AutoCompleteSource.CustomSource;
                        this.txtName.AutoCompleteCustomSource = ItemDescCollection;
                        break;
                    case clsPOSDBConstants.Department_tbl:
                        ItemDescCollection = oSearch.GetAutoCompleteCollectionData(clsPOSDBConstants.Department_tbl, clsPOSDBConstants.Department_Fld_DeptName);
                        this.txtName.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
                        this.txtName.AutoCompleteSource = AutoCompleteSource.CustomSource;
                        this.txtName.AutoCompleteCustomSource = ItemDescCollection;
                        break;
                    case clsPOSDBConstants.Vendor_tbl:
                        ItemDescCollection = oSearch.GetAutoCompleteCollectionData(clsPOSDBConstants.Vendor_tbl, clsPOSDBConstants.Vendor_Fld_VendorName);
                        this.txtName.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
                        this.txtName.AutoCompleteSource = AutoCompleteSource.CustomSource;
                        this.txtName.AutoCompleteCustomSource = ItemDescCollection;

                        ItemDescCollection = oSearch.GetAutoCompleteCollectionData(clsPOSDBConstants.Vendor_tbl, clsPOSDBConstants.Vendor_Fld_VendorCode);
                        this.txtCode.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
                        this.txtCode.AutoCompleteSource = AutoCompleteSource.CustomSource;
                        this.txtCode.AutoCompleteCustomSource = ItemDescCollection;
                        break;
                    case clsPOSDBConstants.PrimeRX_HouseChargeInterface:
                    case clsPOSDBConstants.PrimeRX_PatientInterface:
                        oDataSet = oBLSearch.GetHouseChargeSearchData(this.txtCode.Text.Trim().Replace("'", "''"), this.txtName.Text.Trim().Replace("'", "''"), this.txtMasterSearchVal.Text.Trim().Replace("'", "''"));
                        foreach (DataRow oRow in oDataSet.Tables[0].Rows)
                        {
                            for (int index = 0; index < oRow.ItemArray.Length; index++)
                            {
                                ItemDescCollection.Add(oRow.ItemArray[index].ToString());
                            }
                        }
                        this.txtMasterSearchVal.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
                        this.txtMasterSearchVal.AutoCompleteSource = AutoCompleteSource.CustomSource;
                        this.txtMasterSearchVal.AutoCompleteCustomSource = ItemDescCollection;
                        break;
                    default:
                        break;
                }
            }
            logger.Trace("SetAutoComplete() - " + clsPOSDBConstants.Log_Exiting);
        }

        private void setTitle()
        {
            logger.Trace("setTitle() - " + clsPOSDBConstants.Log_Entering);
            string strGridHead = string.Empty;

            switch (SearchTable)
            {
                case clsPOSDBConstants.item_PriceInv_Lookup:
                    strGridHead = "Item Price Lookup";
                    break;
                case clsPOSDBConstants.ItemMonitorCategory_tbl:
                    strGridHead = "Item Monitor Category";
                    break;
                case clsPOSDBConstants.Customer_tbl:
                    strGridHead = "Customer";
                    break;
                case clsPOSDBConstants.Department_tbl:
                    strGridHead = "Department";
                    break;
                case clsPOSDBConstants.SubDepartment_tbl:
                    strGridHead = "Sub Department";
                    break;
                case clsPOSDBConstants.FunctionKeys_tbl:
                    strGridHead = "Function Keys";
                    break;
                case clsPOSDBConstants.InvRecvHeader_tbl:
                    strGridHead = "Inventory Received"; //PRIMEPOS-2824 25-Mar-2020 JY modified
                    break;
                case clsPOSDBConstants.Item_tbl:
                    strGridHead = "Item File";
                    break;
                case clsPOSDBConstants.CLCards_tbl:
                    strGridHead = "Customer Loyalty Cards";
                    break;
                case clsPOSDBConstants.CLPointsRewardTier_tbl:
                    strGridHead = "Customer Points Reward Tiers";
                    break;
                case clsPOSDBConstants.WarningMessages_tbl:
                    strGridHead = "Warning Messages";
                    break;
                case clsPOSDBConstants.PayOut_tbl:
                    strGridHead = "Payout";
                    break;
                case clsPOSDBConstants.PayType_tbl:
                    strGridHead = "Payment Type";
                    break;
                case clsPOSDBConstants.PhysicalInv_tbl:
                    strGridHead = "Physical Inventory";
                    break;
                case clsPOSDBConstants.POHeader_tbl:
                    strGridHead = "Purchase Order";
                    break;
                case clsPOSDBConstants.TaxCodes_tbl:
                    strGridHead = "Tax Table";
                    break;
                case clsPOSDBConstants.InvTransType_tbl:
                    strGridHead = "Inv. Trans. Type Table";
                    break;
                case clsPOSDBConstants.TransHeader_tbl:
                    strGridHead = "POS Transaction";
                    break;
                case clsPOSDBConstants.Users_tbl:
                    strGridHead = "Users Information";
                    break;
                case clsPOSDBConstants.UsersGroup_tbl:
                    strGridHead = "Users Group Information";
                    break;
                case clsPOSDBConstants.Vendor_tbl:
                    strGridHead = "Vendors";
                    break;
                case clsPOSDBConstants.StationCloseHeader_tbl:
                    strGridHead = "View Station Close";
                    break;
                case clsPOSDBConstants.EndOfDay_tbl:
                    strGridHead = "View End Of Day";
                    break;
                //added by shitaljit on 15 march 2012
                case clsPOSDBConstants.PayOutCat_tbl:
                    strGridHead = "Payout Catagory Table";
                    break;
                case clsPOSDBConstants.ItemComboPricing_tbl:
                    strGridHead = "Item Combo Pricing";
                    break;
                case clsPOSDBConstants.VendorCostPrice_View:
                    strGridHead = "Vendor Cost Price";
                    break;
                case clsPOSDBConstants.PrimeRX_PatientInterface:
                    strGridHead = "PrimeRX Patient";
                    break;
                case clsPOSDBConstants.PrimeRX_HouseChargeInterface:
                    strGridHead = "House Charge Account";
                    break;
                case clsPOSDBConstants.TaxCodes_With_NoTax:
                    strGridHead = "Tax Codes";
                    break;
                default:
                    strGridHead = FormCaption;
                    break;
            }
            this.Text = "Search " + strGridHead;
            this.grdSearch.Text = strGridHead;
            logger.Trace("setTitle() - " + clsPOSDBConstants.Log_Exiting);
        }

        private void Search(Boolean IsInActive = false)//Added by Arvind 2664
        {
            try
            {
                logger.Trace("Search() - " + clsPOSDBConstants.Log_Entering);
                this.Cursor = System.Windows.Forms.Cursors.WaitCursor;

                if (SearchTable == clsPOSDBConstants.UsersGroup_tbl)
                {
                    SearchUserGruop();
                    return;
                }
                else if (SearchTable == clsPOSDBConstants.Item_tbl)  //Sprint-21 - 2206 15-Jul-2015 JY Added if condition
                {
                    oDataSet = oBLSearch.SearchData(SearchTable, this.txtCode.Text.Trim().Replace("'", "''"), this.txtName.Text.Trim().Replace("'", "''"), ActiveOnly, -1);
                    grdSearch.DataSource = oDataSet;
                }
                else if (SearchTable == clsPOSDBConstants.Customer_tbl)
                {
                    oDataSet = oBLSearch.GetCustomerSearchResult(Convert.ToDateTime(dtpDateOfBirth1.Value.ToString()), Convert.ToDateTime(dtpDateOfBirth2.Value.ToString()), Convert.ToDateTime(dtpExpDate1.Value.ToString()), Convert.ToDateTime(dtpExpDate2.Value.ToString()), this.txtCode.Text.Trim().Replace("'", "''"), this.txtName.Text.Trim().Replace("'", "''"), this.txtMasterSearchVal.Text.Trim().Replace("'", "''"), this.chkIncludeRXCust.Checked, out oCustomerData,
                        "", false, 0, false, false, false, cboExpDate.SelectedItem.DataValue.ToString(), cboDOB.SelectedItem.DataValue.ToString(), chkNoStoreCard.Checked);    //PRIMEPOS-2613 07-Dec-2018 JY Added new parameters //PRIMEPOS-2645 05-Mar-2019 JY Added DOB parameters//PRIMEPOS-2896 Added NoStoreCard check
                    //PRIMEPOS-2896
                    if (chkNoStoreCard.Checked)
                    {
                        pnlCustTokenize.Visible = true;
                    }
                    else
                        pnlCustTokenize.Visible = false;

                    grdSearch.DataSource = oDataSet;
                    this.grdSearch.DisplayLayout.Bands[0].Columns["Name"].Width = 150;
                    this.grdSearch.DisplayLayout.Bands[0].Columns["Address1"].Width = 150;
                    this.txtMasterSearchVal.Focus();
                }
                else if (SearchTable == clsPOSDBConstants.TaxCodes_tbl)
                {
                    oDataSet = oBLSearch.SearchData(SearchTable, this.txtCode.Text.Trim().Replace("'", "''"), this.txtName.Text.Trim().Replace("'", "''"), ActiveOnly, -1, 0, IsInActive);//Added by Arvind
                    if (oDataSet.Tables.Count > 0)
                        oDataSet = TaxCodeHelper.GetDataSetWithStringTaxTypeColumns(oDataSet);
                    grdSearch.DataSource = oDataSet;
                }
                else if (SearchTable == clsPOSDBConstants.POHeader_tbl)
                {
                    // Added By Abhishek
                    //logic added is specific to po table 
                    // where we will bring the data for purchase ord table 
                    //this.cboAddEditPOStatusList.Text = clsPOSDBConstants.All;
                    oDataSet = oBLSearch.SearchData(SearchTable, this.txtCode.Text.Trim().Replace("'", "''"), this.txtName.Text.Trim().Replace("'", "''"), ActiveOnly, -1);
                    if (this.GetPOStatus != string.Empty && this.GetPOStatus != clsPOSDBConstants.All)
                    {
                        //this.GetPOStatus will get the value for staus seletced from the    
                        DataRow[] row = oDataSet.Tables[0].Select(" [PO Status]= '" + this.GetPOStatus + "'");
                        DataTable tblFilteredStat = new DataTable();
                        tblFilteredStat = oDataSet.Tables[0].Clone();

                        foreach (DataRow rw in row)
                        {
                            tblFilteredStat.ImportRow(rw);
                        }

                        this.cboAddEditPOStatusList.Text = this.cboAddEditPOStatusList.SelectedItem.DisplayText.ToString();
                        oDataSet.Tables[0].Rows.Clear();
                        oDataSet.Merge(tblFilteredStat);
                    }

                    this.grdSearch.DisplayLayout.Bands[0].Columns["Template"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.Button;
                    this.grdSearch.DisplayLayout.Bands[0].Columns["Template"].CellAppearance.Image = imageList1.Images[1];
                    this.grdSearch.DisplayLayout.Bands[0].Columns["Template"].ButtonDisplayStyle = ButtonDisplayStyle.Always;
                    this.grdSearch.DisplayLayout.Bands[0].Columns["Vendor"].Header.VisiblePosition = 3;
                    this.grdSearch.DisplayLayout.Bands[0].Columns["Template"].CellButtonAppearance.Image = imageList1.Images[1];

                    HideColumn(clsPOSDBConstants.POHeader_Fld_OrderID);

                    foreach (UltraGridRow uRow in this.grdSearch.Rows)
                    {
                        try
                        {
                            //DataRow[] drs=oDataSet.Tables[0].Select("OrderNo='"+uRow.Cells[2].Value.ToString()+"'");
                            if (uRow.Cells["Template"].Value.ToString() == "True")
                            {
                                uRow.Cells["Template"].ButtonAppearance.Image = imageList1.Images[0];
                                //uRow.Cells["Template"].Value = null;
                            }
                            else
                            {
                                uRow.Cells["Template"].ButtonAppearance.Image = imageList1.Images[1];
                            }
                        }
                        catch (Exception ex)
                        {
                            logger.Fatal(ex, "Search()");
                        }
                    }

                    //Added By shitaljit on 20 Feb 2012 to select First Row of the grid
                    //To execute Manually Acknowlwdge botton click functionality as it requires row to be selected.
                    if (grdSearch.Selected.Rows.Count == 0 && grdSearch.Rows.Count > 0)
                    {
                        grdSearch.Selected.Rows.Add(grdSearch.Rows[0]);
                        grdSearch.ActiveRow = grdSearch.Rows[0];
                        this.grdSearch.DisplayLayout.Override.CellClickAction = CellClickAction.RowSelect;
                    }
                    grdSearch.DataSource = oDataSet;
                    this.grdSearch.Refresh();
                    this.grdSearch.DisplayLayout.Bands[0].Columns["Template"].Width = 20;
                    EnableOrDisable(clsPOSDBConstants.Incomplete);
                }
                else if (SearchTable == clsPOSDBConstants.Users_tbl)
                {
                    oDataSet = oBLSearch.SearchData(SearchTable, this.txtCode.Text.Trim().Replace("'", "''"), this.txtName.Text.Trim().Replace("'", "''"), ActiveOnly, -1);
                    if (oDataSet.Tables[0].Rows.Count > 0)
                    {
                        //Added By Shitaljit to hide and display I-Active Users 
                        if (this.chkDispInActiveUsers.Checked == false)
                        {
                            oDataSet = oBLSearch.GetActiveUsers(oDataSet);
                            grdSearch.DataSource = oDataSet;
                            grdSearch.PerformAction(UltraGridAction.FirstRowInGrid);
                            grdSearch.Refresh();
                        }
                        if (oDataSet.Tables[0].Rows.Count > 0) SetUnlockBtnState(oDataSet.Tables[0].Rows[0]["IsLocked"].ToString());
                    }
                    grdSearch.DataSource = oDataSet;
                }
                else if (SearchTable == clsPOSDBConstants.ItemMonitorCategory_tbl)
                {
                    oDataSet = oBLSearch.SearchData(SearchTable, this.txtCode.Text.Trim().Replace("'", "''"), this.txtName.Text.Trim().Replace("'", "''"), ActiveOnly, -1);
                    grdSearch.DataSource = oDataSet;
                    grdSearch.DisplayLayout.AutoFitStyle = AutoFitStyle.ResizeAllColumns;
                }
                else if (SearchTable == clsPOSDBConstants.Department_tbl)
                {
                    oDataSet = oBLSearch.SearchData(SearchTable, this.txtCode.Text.Trim().Replace("'", "''"), this.txtName.Text.Trim().Replace("'", "''"), ActiveOnly, -1);
                    grdSearch.DataSource = oDataSet;
                    this.grdSearch.DisplayLayout.Bands[0].Columns["ID"].Hidden = true;
                    resizeColumns();
                }
                else if (SearchTable == clsPOSDBConstants.CLCards_tbl)
                {
                    oDataSet = oBLSearch.SearchData(SearchTable, this.txtCode.Text.Trim().Replace("'", "''"), this.txtName.Text.Trim().Replace("'", "''"), ActiveOnly, -1);
                    grdSearch.DataSource = oDataSet;
                    this.grdSearch.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.Customer_Fld_CustomerId].Hidden = true;
                }
                else
                {
                    oDataSet = oBLSearch.SearchData(SearchTable, this.txtCode.Text.Trim().Replace("'", "''"), this.txtName.Text.Trim().Replace("'", "''"), ActiveOnly, -1);
                    grdSearch.DataSource = oDataSet;
                }

                //if (oDataSet.Tables[0].Rows.Count < 1)
                //{
                //    this.ActiveControl = this.cboAddEditPOStatusList;
                //    this.cboAddEditPOStatusList.Focus();
                //}
                //else
                //{
                //    grdSearch.Focus();
                //}
                grdSearch.PerformAction(UltraGridAction.FirstRowInGrid);
                grdSearch.Refresh();
                //this.resizeColumns();

                //Till here added By Shitaljit
                //Commented by Prashant(SRT) Date:30-5-09
                //sbMain.Panels[0].Text = "Record(s) Count = " + grdSearch.Rows.Count;
                //End of Commented by Prashant(SRT) Date:30-5-09                           
                logger.Trace("Search() - " + clsPOSDBConstants.Log_Exiting);
            }
            catch (Exception Ex)
            {
                logger.Fatal(Ex, "Search()");
                clsUIHelper.ShowErrorMsg(Ex.Message);
            }
            finally
            {
                this.Cursor = System.Windows.Forms.Cursors.Default;
            }
            //End Of Added By SRT(Gaurav)
        }

        private void SearchUserGruop()
        {
            string sSQL = string.Empty;

            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            try
            {
                logger.Trace("SearchUserGruop() - " + clsPOSDBConstants.Log_Entering);

                User oUser = new User();
                oDataSet = oUser.GetUserGroup(this.txtCode.Text, this.txtName.Text);
                grdSearch.DataSource = oDataSet;
                grdSearch.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.Users_Fld_UserID].Header.Caption = "Group Code";
                grdSearch.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.Users_Fld_fName].Header.Caption = "Group Name";
                //resizeColumns();
                grdSearch.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.Users_Fld_UserID].Width = 200;
                grdSearch.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.Users_Fld_fName].Width = 150;

                logger.Trace("SearchUserGruop() - " + clsPOSDBConstants.Log_Entering);
            }
            catch (Exception Ex)
            {
                logger.Fatal(Ex, "SearchUserGruop()");
                this.Cursor = System.Windows.Forms.Cursors.Default;
                throw Ex;
            }
            finally
            {
                this.Cursor = System.Windows.Forms.Cursors.Default;
            }
        }

        private void HideColumn(String colName)
        {
            try
            {
                logger.Trace("HideColumn() - " + clsPOSDBConstants.Log_Entering);
                this.grdSearch.DisplayLayout.Bands[0].Columns[colName].Hidden = true;
                this.grdSearch.Update();
                logger.Trace("HideColumn() - " + clsPOSDBConstants.Log_Exiting);
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "HideColumn()");
                clsUIHelper.ShowErrorMsg(ex.Message);
            }
        }

        #region Add functionality
        private void Add()
        {
            try
            {
                logger.Trace("Add() - " + clsPOSDBConstants.Log_Entering);

                if (pnlAdd.Visible == false) return;
                if (frmMain.oPublicNumericPad != null && (!frmMain.oPublicNumericPad.IsDisposed)) frmMain.oPublicNumericPad.RemoveParent();
                switch (SearchTable)
                {
                    case clsPOSDBConstants.Customer_tbl:
                        AddCustomer();
                        break;
                    case clsPOSDBConstants.Department_tbl:
                        AddDepartment();
                        break;
                    case clsPOSDBConstants.CLPointsRewardTier_tbl:
                        AddCLPointsRewardTier();
                        break;
                    case clsPOSDBConstants.CLCards_tbl:
                        AddCLCards();
                        break;
                    case clsPOSDBConstants.WarningMessages_tbl:
                        AddWarningMessages();
                        break;
                    case clsPOSDBConstants.TaxCodes_tbl:
                        AddTaxCodes();
                        break;
                    case clsPOSDBConstants.ItemMonitorCategory_tbl:
                        AddItemMonitorCategory();
                        break;
                    case clsPOSDBConstants.InvTransType_tbl:
                        AddInvTransType();
                        break;
                    case clsPOSDBConstants.Item_tbl:
                        AddItem();
                        break;
                    case clsPOSDBConstants.Vendor_tbl:
                        AddVendor();
                        break;
                    case clsPOSDBConstants.POHeader_tbl:
                        AddPurchaseOrder();
                        break;
                    case clsPOSDBConstants.Users_tbl:
                        AddUser();
                        break;
                    case clsPOSDBConstants.PayOutCat_tbl:
                        AddPayoutCategory();
                        break;
                    case clsPOSDBConstants.ItemComboPricing_tbl:
                        AddItemComboPricing();
                        break;
                    case clsPOSDBConstants.PayType_tbl://Added By Shitaljit for ne pay types on 28 jan 2013
                        AddPayType();
                        break;
                    case clsPOSDBConstants.UsersGroup_tbl:
                        AddUserGroup();
                        break;
                    case clsPOSDBConstants.Coupon_tbl:
                        AddCoupon();
                        break;
                    case clsPOSDBConstants.TransFee_tbl:    //PRIMEPOS-3116 11-Jul-2022 JY Added
                        AddTransactionFee();
                        break;
                }
                if (frmMain.oPublicNumericPad != null && (!frmMain.oPublicNumericPad.IsDisposed)) frmMain.oPublicNumericPad.AttachParent(new IntPtr(0));

                logger.Trace("Add() - " + clsPOSDBConstants.Log_Exiting);
            }
            catch (Exception exp)
            {
                logger.Fatal(exp, "Add()");
                if (frmMain.oPublicNumericPad != null && (!frmMain.oPublicNumericPad.IsDisposed)) frmMain.oPublicNumericPad.AttachParent(new IntPtr(0));
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }

        private void AddCustomer()
        {
            try
            {
                logger.Trace("AddCustomer() - " + clsPOSDBConstants.Log_Entering);
                frmCustomers oCustomer = new frmCustomers();
                //oCustomer.Initialize();   //28-Nov-2017 JY No need to initiate 
                //oCustomer.Owner = this;   //PRIMEPOS-2649 04-Mar-2019 JY Commented
                oCustomer.ShowDialog(this); //PRIMEPOS-2649 04-Mar-2019 JY passed "this" as parameter
                if (!oCustomer.IsCanceled)
                    btnSearch_Click(null, null);

                logger.Trace("AddCustomer() - " + clsPOSDBConstants.Log_Exiting);
            }
            catch (Exception exp)
            {
                logger.Fatal(exp, "AddCustomer()");
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }

        private void AddCLPointsRewardTier()
        {
            //if (grdSearch.Rows.Count <=0) return;
            try
            {
                logger.Trace("AddCLPointsRewardTier() - " + clsPOSDBConstants.Log_Entering);

                frmCLPointsRewardTier ofrmCLPointsRewardTier = new frmCLPointsRewardTier();
                ofrmCLPointsRewardTier.Initialize();
                ofrmCLPointsRewardTier.ShowDialog(this);
                if (!ofrmCLPointsRewardTier.IsCanceled)
                    btnSearch_Click(null, null);

                logger.Trace("AddCLPointsRewardTier() - " + clsPOSDBConstants.Log_Exiting);
            }
            catch (Exception exp)
            {
                logger.Fatal(exp, "AddCLPointsRewardTier()");
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }

        private void AddCLCards()
        {
            try
            {
                logger.Trace("AddCLCards() - " + clsPOSDBConstants.Log_Entering);

                frmCLCards ofrmCLCards = new frmCLCards();
                ofrmCLCards.Initialize(string.Empty, string.Empty);
                ofrmCLCards.ShowDialog(this);
                if (!ofrmCLCards.IsCanceled)
                    btnSearch_Click(null, null);

                logger.Trace("AddCLCards() - " + clsPOSDBConstants.Log_Exiting);
            }
            catch (Exception exp)
            {
                logger.Fatal(exp, "AddCLCards()");
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }

        private void AddDepartment()
        {
            //if (grdSearch.Rows.Count <=0) return;
            try
            {
                logger.Trace("AddDepartment() - " + clsPOSDBConstants.Log_Entering);

                frmDepartment oDepartment = new frmDepartment();
                oDepartment.Initialize();
                oDepartment.ShowDialog(this);
                if (!oDepartment.IsCanceled)
                    btnSearch_Click(null, null);

                logger.Trace("AddDepartment() - " + clsPOSDBConstants.Log_Entering);
            }
            catch (Exception exp)
            {
                logger.Fatal(exp, "AddDepartment()");
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }

        private void AddWarningMessages()
        {
            try
            {
                logger.Trace("AddWarningMessages() - " + clsPOSDBConstants.Log_Entering);

                frmWarningMessages oFrm = new frmWarningMessages();
                oFrm.Initialize();
                oFrm.ShowDialog(this);
                if (!oFrm.IsCanceled)
                    btnSearch_Click(null, null);

                logger.Trace("AddWarningMessages() - " + clsPOSDBConstants.Log_Exiting);
            }
            catch (Exception exp)
            {
                logger.Fatal(exp, "AddWarningMessages()");
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }

        private void AddTaxCodes()
        {
            try
            {
                logger.Trace("AddTaxCodes() - " + clsPOSDBConstants.Log_Entering);

                frmTaxCodes oTaxCodes = new frmTaxCodes();
                oTaxCodes.Initialize();
                oTaxCodes.ShowDialog(this);
                if (!oTaxCodes.IsCanceled)
                    btnSearch_Click(null, null);

                logger.Trace("AddTaxCodes() - " + clsPOSDBConstants.Log_Exiting);
            }
            catch (Exception exp)
            {
                logger.Fatal(exp, "AddTaxCodes()");
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }

        /// <summary>
        /// Added By shitaljit for adding new payment type.
        /// </summary>
        private void AddPayType()
        {
            try
            {
                logger.Trace("AddPayType() - " + clsPOSDBConstants.Log_Entering);

                string sPayTypeCode = string.Empty;
                PayTypeSvr oPayTypeSvr = new PayTypeSvr();
                sPayTypeCode = oPayTypeSvr.GetNextPayTypeID();
                if (string.IsNullOrEmpty(sPayTypeCode))
                {
                    return;
                }
                frmPayTypes ofrmPayTypes = new frmPayTypes();
                ofrmPayTypes.Initialize();
                ofrmPayTypes.ShowDialog(this);
                if (!ofrmPayTypes.IsCanceled)
                {
                    btnSearch_Click(null, null);
                }

                logger.Trace("AddPayType() - " + clsPOSDBConstants.Log_Exiting);
            }
            catch (Exception exp)
            {
                logger.Fatal(exp, "AddPayType()");
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }

        private void AddCoupon()
        {
            try
            {
                logger.Trace("AddCoupon() - " + clsPOSDBConstants.Log_Entering);

                frmPOSCoupon ofrmPOSCoupon = new frmPOSCoupon();
                ofrmPOSCoupon.SetNew();
                ofrmPOSCoupon.ShowDialog();
                if (ofrmPOSCoupon.IsCanceled == false)
                {
                    btnSearch_Click(null, null);
                }
                logger.Trace("AddCoupon() - " + clsPOSDBConstants.Log_Exiting);
            }
            catch (Exception exp)
            {
                logger.Fatal(exp, "AddCoupon()");
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }

        private void AddItemMonitorCategory()
        {
            try
            {
                logger.Trace("AddItemMonitorCategory() - " + clsPOSDBConstants.Log_Entering);

                frmItemMonitorCategory ofrm = new frmItemMonitorCategory();
                ofrm.Initialize();
                ofrm.ShowDialog(this);
                if (!ofrm.IsCanceled)
                    btnSearch_Click(null, null);

                logger.Trace("AddItemMonitorCategory() - " + clsPOSDBConstants.Log_Exiting);
            }
            catch (Exception exp)
            {
                logger.Fatal(exp, "AddItemMonitorCategory()");
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }

        private void AddInvTransType()
        {
            try
            {
                logger.Trace("AddInvTransType() - " + clsPOSDBConstants.Log_Entering);

                frmInvTransType oInvTransType = new frmInvTransType();
                oInvTransType.Initialize();
                oInvTransType.ShowDialog(this);
                if (!oInvTransType.IsCanceled)
                    btnSearch_Click(null, null);

                logger.Trace("AddInvTransType() - " + clsPOSDBConstants.Log_Exiting);
            }
            catch (Exception exp)
            {
                logger.Fatal(exp, "AddInvTransType()");
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }

        private void AddVendor()
        {
            try
            {
                logger.Trace("AddVendor() - " + clsPOSDBConstants.Log_Entering);

                frmVendor oVendor = new frmVendor();
                oVendor.Initialize();
                oVendor.ShowDialog(this);
                if (!oVendor.IsCanceled)
                    btnSearch_Click(null, null);

                logger.Trace("AddVendor() - " + clsPOSDBConstants.Log_Exiting);
            }
            catch (Exception exp)
            {
                logger.Fatal(exp, "AddVendor()");
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }

        private void AddPurchaseOrder()
        {
            String poStatus = String.Empty;
            try
            {
                logger.Trace("AddPurchaseOrder() - " + clsPOSDBConstants.Log_Entering);

                frmCreateNewPurchaseOrder oPO = new frmCreateNewPurchaseOrder();
                oPO.Initialize();
                oPO.ShowDialog();

                logger.Trace("AddPurchaseOrder() - " + clsPOSDBConstants.Log_Exiting);
            }
            catch (Exception exp)
            {
                logger.Fatal(exp, "AddPurchaseOrder()");
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }

        private void AddItem()
        {
            try
            {
                logger.Trace("AddItem() - " + clsPOSDBConstants.Log_Entering);

                frmItems oItems = new frmItems();
                oItems.Initialize();
                oItems.ShowDialog(this);
                if (!oItems.IsCanceled)
                    btnSearch_Click(null, null);

                logger.Trace("AddItem() - " + clsPOSDBConstants.Log_Exiting);
            }
            catch (Exception exp)
            {
                logger.Fatal(exp, "AddItem()");
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }

        private void AddUser()
        {
            //if (grdSearch.Rows.Count <=0) return;
            try
            {
                logger.Trace("AddUser() - " + clsPOSDBConstants.Log_Entering);

                UserManagement.frmUserInformation oUserInformation = new UserManagement.frmUserInformation();
                oUserInformation.SetNew();
                oUserInformation.ShowDialog(this);
                if (!oUserInformation.IsCanceled)
                    btnSearch_Click(null, null);

                logger.Trace("AddUser() - " + clsPOSDBConstants.Log_Exiting);
            }
            catch (Exception exp)
            {
                logger.Fatal(exp, "AddUser()");
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }

        private void AddUserGroup()
        {
            //if (grdSearch.Rows.Count <=0) return;
            try
            {
                logger.Trace("AddUserGroup() - " + clsPOSDBConstants.Log_Entering);

                UserManagement.frmUserGroup oUserGroup = new UserManagement.frmUserGroup();
                oUserGroup.SetNew();
                oUserGroup.ShowDialog(this);
                if (!oUserGroup.IsCanceled)
                    SearchUserGruop();

                logger.Trace("AddUserGroup() - " + clsPOSDBConstants.Log_Exiting);
            }
            catch (Exception exp)
            {
                logger.Fatal(exp, "AddUserGroup()");
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }

        private void AddPayoutCategory()
        {
            try
            {
                logger.Trace("AddPayoutCategory() - " + clsPOSDBConstants.Log_Entering);

                frmPayoutCatagory ofrmPayoutCatagory = new frmPayoutCatagory();
                ofrmPayoutCatagory.SetNew();
                ofrmPayoutCatagory.ShowDialog();
                if (!ofrmPayoutCatagory.IsCanceled)
                {
                    btnSearch_Click(null, null);
                }

                logger.Trace("AddPayoutCategory() - " + clsPOSDBConstants.Log_Exiting);
            }
            catch (Exception exp)
            {
                logger.Fatal(exp, "AddPayoutCategory()");
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }

        private void AddItemComboPricing()
        {
            try
            {
                logger.Trace("AddItemComboPricing() - " + clsPOSDBConstants.Log_Entering);

                frmItemComboPricing ofrm = new frmItemComboPricing();
                ofrm.SetNew();
                if (ofrm.ShowDialog() == DialogResult.OK)
                {
                    btnSearch_Click(null, null);
                }

                logger.Trace("AddItemComboPricing() - " + clsPOSDBConstants.Log_Exiting);
            }
            catch (Exception exp)
            {
                logger.Fatal(exp, "AddItemComboPricing()");
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }
        #endregion

        #region Edit record
        private void Edit()
        {
            try
            {
                logger.Trace("Edit() - " + clsPOSDBConstants.Log_Entering);

                if (pnlEdit.Visible == false) return;
                if (grdSearch.ActiveRow == null)
                    if (grdSearch.ActiveRow.Cells.Count == 0)
                        return;

                if (frmMain.oPublicNumericPad != null && (!frmMain.oPublicNumericPad.IsDisposed)) frmMain.oPublicNumericPad.RemoveParent();
                switch (SearchTable)
                {
                    case clsPOSDBConstants.StationCloseHeader_tbl:
                        EditStationClose();
                        break;
                    case clsPOSDBConstants.EndOfDay_tbl:
                        EndOfDayClose();
                        break;
                    case clsPOSDBConstants.Customer_tbl:
                        EditCustomer();
                        break;
                    case clsPOSDBConstants.Department_tbl:
                        EditDepartment();
                        break;
                    case clsPOSDBConstants.CLPointsRewardTier_tbl:
                        EditCLPointsRewardTier();
                        break;
                    case clsPOSDBConstants.CLCards_tbl:
                        EditCLCards();
                        break;
                    case clsPOSDBConstants.WarningMessages_tbl:
                        EditWarningMessages();
                        break;
                    case clsPOSDBConstants.TaxCodes_tbl:
                        EditTaxCodes();
                        break;
                    case clsPOSDBConstants.ItemMonitorCategory_tbl:
                        EditItemMonitorCategory();
                        break;
                    case clsPOSDBConstants.InvTransType_tbl:
                        EditInvTransType();
                        break;
                    case clsPOSDBConstants.Item_tbl:
                        EditItem();
                        break;
                    case clsPOSDBConstants.Vendor_tbl:
                        EditVendor();
                        break;
                    case clsPOSDBConstants.POHeader_tbl:
                        //this.btnEdit.Enabled = false;
                        EditPurchaseOrder();
                        break;
                    case clsPOSDBConstants.Users_tbl:
                        EditUser();
                        break;
                    //Added by shitaljit on 15 march 2012
                    case clsPOSDBConstants.PayOutCat_tbl:
                        EditPayoutCategory();
                        break;
                    case clsPOSDBConstants.ItemComboPricing_tbl:
                        EditItemComboPricing();
                        break;
                    case clsPOSDBConstants.PayType_tbl:
                        EditPayType();
                        break;
                    case clsPOSDBConstants.UsersGroup_tbl:
                        EditUserGroup();
                        break;
                    case clsPOSDBConstants.Coupon_tbl:
                        EditCoupon();
                        break;
                    case clsPOSDBConstants.TransFee_tbl:    //PRIMEPOS-3116 11-Jul-2022 JY Added
                        EditTransactionFee();
                        break;
                }
                if (frmMain.oPublicNumericPad != null && (!frmMain.oPublicNumericPad.IsDisposed)) frmMain.oPublicNumericPad.AttachParent(new IntPtr(0));

                logger.Trace("Edit() - " + clsPOSDBConstants.Log_Exiting);
            }
            catch (Exception exp)
            {
                logger.Fatal(exp, "Edit()");
                if (frmMain.oPublicNumericPad != null && (!frmMain.oPublicNumericPad.IsDisposed)) frmMain.oPublicNumericPad.AttachParent(new IntPtr(0));
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }

        private void EditCustomer()
        {
            if (grdSearch.Rows.Count <= 0) return;

            Customer ogetCustomer = new Customer();
            CustomerData oCustdata = new CustomerData();
            CustomerRow oCustRow = null;
            try
            {
                logger.Trace("EditCustomer() - " + clsPOSDBConstants.Log_Entering);

                string strCode = string.Empty;
                if (grdSearch.ActiveRow != null && grdSearch.ActiveRow.Cells.Count > 0)
                    strCode = grdSearch.ActiveRow.Cells[clsPOSDBConstants.CLCards_Fld_CustomerID].Text;

                oCustdata = ogetCustomer.GetCustomerByID(Configuration.convertNullToInt(strCode));

                if (oCustdata.Tables[0].Rows.Count == 0)
                {
                    oCustRow = SelectedRow();
                    if (oCustRow != null)
                    {
                        if (oCustRow.Address1.ToString().Trim() == "")
                        {
                            oCustRow.Address1 = "-";
                        }
                        if (oCustRow.State.ToString().Trim() == "")
                        {
                            oCustRow.State = "-";
                        }
                        oCustdata.Tables[0].ImportRow(oCustRow);

                        ogetCustomer.Persist(oCustdata);
                        oCustdata = ogetCustomer.GetCustomerByPatientNo(oCustRow.PatientNo);
                        if (oCustdata.Tables[0].Rows.Count > 0)
                        {
                            oCustRow = (CustomerRow)oCustdata.Customer.Rows[0];
                            strCode = oCustRow.CustomerId.ToString();
                        }
                    }
                }
                else
                    strCode = grdSearch.ActiveRow.Cells[0].Text;
                frmCustomers oCustomer = new frmCustomers();
                oCustomer.Edit(strCode);
                oCustomer.ShowDialog(this);
                if (!oCustomer.IsCanceled)
                    btnSearch_Click(null, null);

                logger.Trace("EditCustomer() - " + clsPOSDBConstants.Log_Exiting);
            }
            catch (Exception exp)
            {
                logger.Fatal(exp, "EditCustomer()");
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }

        private void EditItemComboPricing()
        {
            if (grdSearch.Rows.Count <= 0) return;

            try
            {
                logger.Trace("EditItemComboPricing() - " + clsPOSDBConstants.Log_Entering);

                frmItemComboPricing ofrm = new frmItemComboPricing();
                ofrm.Edit(Configuration.convertNullToInt(grdSearch.ActiveRow.Cells["code"].Text));
                if (ofrm.ShowDialog(this) == DialogResult.OK)
                    btnSearch_Click(null, null);

                logger.Trace("EditItemComboPricing() - " + clsPOSDBConstants.Log_Exiting);
            }
            catch (Exception exp)
            {
                logger.Fatal(exp, "EditItemComboPricing()");
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }

        private void EditStationClose()
        {
            if (grdSearch.Rows.Count <= 0) return;

            try
            {
                logger.Trace("EditStationClose() - " + clsPOSDBConstants.Log_Entering);

                frmStationClose oStationClose = new frmStationClose();
                int id = Convert.ToInt32(grdSearch.ActiveRow.Cells[0].Text);
                oStationClose.Edit(id);
                oStationClose.StartPosition = FormStartPosition.CenterScreen;
                oStationClose.ShowDialog(this);

                logger.Trace("EditStationClose() - " + clsPOSDBConstants.Log_Exiting);
            }
            catch (Exception exp)
            {
                logger.Fatal(exp, "EditStationClose()");
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }

        private void EditUser()
        {
            if (grdSearch.Rows.Count <= 0) return;

            try
            {
                logger.Trace("EditUser() - " + clsPOSDBConstants.Log_Entering);

                UserManagement.frmUserInformation oUserInformation = new UserManagement.frmUserInformation();
                oUserInformation.Edit(grdSearch.ActiveRow.Cells[0].Text);
                oUserInformation.ShowDialog(this);

                if (!oUserInformation.IsCanceled)
                    btnSearch_Click(null, null);

                logger.Trace("EditUser() - " + clsPOSDBConstants.Log_Exiting);
            }
            catch (Exception exp)
            {
                logger.Fatal(exp, "EditUser()");
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }

        private void EditUserGroup()
        {
            if (grdSearch.Rows.Count <= 0) return;

            try
            {
                logger.Trace("EditUserGroup() - " + clsPOSDBConstants.Log_Entering);

                UserManagement.frmUserGroup ofrmUserGroup = new UserManagement.frmUserGroup();
                ofrmUserGroup.Edit(grdSearch.ActiveRow.Cells[0].Text);
                ofrmUserGroup.ShowDialog(this);

                if (!ofrmUserGroup.IsCanceled)
                    btnSearch_Click(null, null);

                logger.Trace("EditUserGroup() - " + clsPOSDBConstants.Log_Exiting);
            }
            catch (Exception exp)
            {
                logger.Fatal(exp, "EditUserGroup()");
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }

        private void EditCoupon()
        {
            logger.Trace("EditCoupon() - " + clsPOSDBConstants.Log_Entering);
            frmPOSCoupon ofrmPOSCoupon = new frmPOSCoupon();
            ofrmPOSCoupon.SetNew();
            ofrmPOSCoupon.txtCouponCode.Enabled = false;
            ofrmPOSCoupon.Text = "Coupon";
            ofrmPOSCoupon.lblTransactionType.Text = "Eidt Coupon";
            ofrmPOSCoupon.Edit(Convert.ToInt64(grdSearch.ActiveRow.Cells[0].Value.ToString()));
            ofrmPOSCoupon.ShowDialog(this);
            if (!ofrmPOSCoupon.IsCanceled)
            {
                btnSearch_Click(null, null);
            }
            logger.Trace("EditCoupon() - " + clsPOSDBConstants.Log_Exiting);
        }

        //added by shitaljit to edit payout category.
        private void EditPayoutCategory()
        {
            if (grdSearch.Rows.Count <= 0) return;
            try
            {
                logger.Trace("EditPayoutCategory() - " + clsPOSDBConstants.Log_Entering);
                frmPayoutCatagory ofrmPayoutCatagory = new frmPayoutCatagory();
                ofrmPayoutCatagory.Edit(Convert.ToInt32(grdSearch.ActiveRow.Cells[0].Value.ToString()));
                ofrmPayoutCatagory.ShowDialog(this);
                if (!ofrmPayoutCatagory.IsCanceled)
                {
                    btnSearch_Click(null, null);
                }
                logger.Trace("EditPayoutCategory() - " + clsPOSDBConstants.Log_Exiting);
            }
            catch (Exception exp)
            {
                logger.Fatal(exp, "EditPayoutCategory()");
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }

        private void EditVendor()
        {
            if (grdSearch.Rows.Count <= 0) return;
            try
            {
                logger.Trace("EditVendor() - " + clsPOSDBConstants.Log_Entering);
                frmVendor oVendor = new frmVendor();
                oVendor.Edit(grdSearch.ActiveRow.Cells[0].Text);
                oVendor.ShowDialog(this);
                if (!oVendor.IsCanceled)
                    btnSearch_Click(null, null);
                logger.Trace("EditVendor() - " + clsPOSDBConstants.Log_Exiting);
            }
            catch (Exception exp)
            {
                logger.Fatal(exp, "EditVendor()");
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }

        private void EditPurchaseOrder()
        {
            frmCreateNewPurchaseOrder editPO = null;
            int countPO = 0;
            try
            {
                logger.Trace("EditPurchaseOrder() - " + clsPOSDBConstants.Log_Entering);
                editPO = new frmCreateNewPurchaseOrder();
                editPO.IsEditPO = true;
                if (isInComplete)
                {
                    editPO.IsInComplete = true;
                }
                countPO = (int)grdSearch.Rows.Count;
                for (int count = 0; count < countPO; count++)
                {
                    string poNumber = grdSearch.Rows[count].Cells[0].Text;
                    editPO.Edit(poNumber);
                }
                if (editPO.TotalNoOfOrders > 0)
                    editPO.ShowDialog();
                else
                    clsUIHelper.ShowErrorMsg("There are no Incomplete orders to edit");
                if (!editPO.IsCanceled)
                    btnSearch_Click(null, null);
                logger.Trace("EditPurchaseOrder() - " + clsPOSDBConstants.Log_Exiting);
            }
            catch (Exception exp)
            {
                logger.Fatal(exp, "EditPurchaseOrder()");
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
            finally
            {
                editPO.IsInComplete = false;
            }
        }

        private void EditDepartment()
        {
            if (grdSearch.Rows.Count <= 0) return;

            try
            {
                logger.Trace("EditDepartment() - " + clsPOSDBConstants.Log_Entering);
                frmDepartment oDepartment = new frmDepartment();
                oDepartment.Edit(grdSearch.ActiveRow.Cells[0].Text);
                oDepartment.ShowDialog(this);
                if (!oDepartment.IsCanceled)
                    btnSearch_Click(null, null);
                logger.Trace("EditDepartment() - " + clsPOSDBConstants.Log_Exiting);
            }
            catch (Exception exp)
            {
                logger.Fatal(exp, "EditDepartment()");
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }

        private void EditCLPointsRewardTier()
        {
            if (grdSearch.Rows.Count <= 0) return;

            try
            {
                logger.Trace("EditCLPointsRewardTier() - " + clsPOSDBConstants.Log_Entering);
                frmCLPointsRewardTier ofrmCLPointsRewardTier = new frmCLPointsRewardTier();
                ofrmCLPointsRewardTier.Edit(Configuration.convertNullToInt(grdSearch.ActiveRow.Cells[0].Text));
                ofrmCLPointsRewardTier.ShowDialog(this);
                if (!ofrmCLPointsRewardTier.IsCanceled)
                    btnSearch_Click(null, null);
                logger.Trace("EditCLPointsRewardTier() - " + clsPOSDBConstants.Log_Exiting);
            }
            catch (Exception exp)
            {
                logger.Fatal(exp, "EditCLPointsRewardTier()");
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }

        private void EditCLCards()
        {
            if (grdSearch.Rows.Count <= 0) return;

            try
            {
                logger.Trace("EditCLCards() - " + clsPOSDBConstants.Log_Entering);
                frmCLCards ofrmCLCards = new frmCLCards();
                ofrmCLCards.Edit(Configuration.convertNullToInt64(grdSearch.ActiveRow.Cells[0].Text));
                ofrmCLCards.ShowDialog(this);
                if (!ofrmCLCards.IsCanceled)
                    btnSearch_Click(null, null);
                logger.Trace("EditCLCards() - " + clsPOSDBConstants.Log_Exiting);
            }
            catch (Exception exp)
            {
                logger.Fatal(exp, "EditCLCards()");
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }

        private void EditWarningMessages()
        {
            if (grdSearch.Rows.Count <= 0) return;

            try
            {
                logger.Trace("EditWarningMessages() - " + clsPOSDBConstants.Log_Entering);
                frmWarningMessages oFrm = new frmWarningMessages();
                oFrm.Edit(grdSearch.ActiveRow.Cells[0].Text);
                oFrm.ShowDialog(this);
                if (!oFrm.IsCanceled)
                    btnSearch_Click(null, null);
                logger.Trace("EditWarningMessages() - " + clsPOSDBConstants.Log_Exiting);
            }
            catch (Exception exp)
            {
                logger.Fatal(exp, "EditWarningMessages()");
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }

        private void EditTaxCodes()
        {
            if (grdSearch.Rows.Count <= 0) return;

            try
            {
                logger.Trace("EditTaxCodes() - " + clsPOSDBConstants.Log_Entering);
                frmTaxCodes oTaxCodes = new frmTaxCodes();
                oTaxCodes.Edit(grdSearch.ActiveRow.Cells[0].Text);
                oTaxCodes.ShowDialog(this);
                if (!oTaxCodes.IsCanceled)
                    btnSearch_Click(null, null);
                logger.Trace("EditTaxCodes() - " + clsPOSDBConstants.Log_Exiting);
            }
            catch (Exception exp)
            {
                logger.Fatal(exp, "EditTaxCodes()");
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }

        /// <summary>
        /// Added By shitaljit for editing new payment types 
        /// </summary>
        private void EditPayType()
        {
            if (grdSearch.Rows.Count <= 0) return;

            try
            {
                logger.Trace("EditPayType() - " + clsPOSDBConstants.Log_Entering);
                frmPayTypes ofrmPayTypes = new frmPayTypes();
                ofrmPayTypes.Edit(grdSearch.ActiveRow.Cells[0].Text);
                ofrmPayTypes.ShowDialog(this);
                if (!ofrmPayTypes.IsCanceled)
                {
                    btnSearch_Click(null, null);
                }
                logger.Trace("EditPayType() - " + clsPOSDBConstants.Log_Exiting);
            }
            catch (Exception exp)
            {
                logger.Fatal(exp, "EditPayType()");
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }

        private void EditItemMonitorCategory()
        {
            if (grdSearch.Rows.Count <= 0) return;

            try
            {
                logger.Trace("EditItemMonitorCategory() - " + clsPOSDBConstants.Log_Entering);
                frmItemMonitorCategory ofrm = new frmItemMonitorCategory();
                ofrm.Edit(grdSearch.ActiveRow.Cells[0].Text);
                ofrm.ShowDialog(this);
                if (!ofrm.IsCanceled)
                    btnSearch_Click(null, null);
                logger.Trace("EditItemMonitorCategory() - " + clsPOSDBConstants.Log_Exiting);
            }
            catch (Exception exp)
            {
                logger.Fatal(exp, "EditItemMonitorCategory()");
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }

        private void EditInvTransType()
        {
            if (grdSearch.Rows.Count <= 0) return;

            try
            {
                logger.Trace("EditInvTransType() - " + clsPOSDBConstants.Log_Entering);
                frmInvTransType oInvTransType = new frmInvTransType();
                oInvTransType.Edit(Configuration.convertNullToInt(grdSearch.ActiveRow.Cells[0].Text));
                oInvTransType.ShowDialog(this);
                if (!oInvTransType.IsCanceled)
                    btnSearch_Click(null, null);
                logger.Trace("EditInvTransType() - " + clsPOSDBConstants.Log_Exiting);
            }
            catch (Exception exp)
            {
                logger.Fatal(exp, "EditInvTransType()");
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }

        private void EditItem()
        {
            if (grdSearch.Rows.Count <= 0) return;

            try
            {
                logger.Trace("EditItem() - " + clsPOSDBConstants.Log_Entering);
                frmItems oItems = new frmItems();
                oItems.Edit(grdSearch.ActiveRow.Cells[0].Text.Trim());
                oItems.ShowDialog(this);
                if (!oItems.IsCanceled && IsAdvSearchDone == false)
                {
                    btnSearch_Click(null, null);
                }
                //Added By shitaljit 0n 17 June 0213 for PRIMEPOS-1189 Advance Search Result list is not retained when you come back to it after editing an item
                else if (oItems.IsCanceled == false && IsAdvSearchDone == true)
                {
                    bool isVendRequired = false;
                    bool isItemVendorRequired = false;
                    DataSet itemData = new DataSet();
                    ItemSvr itemSvr = new ItemSvr();
                    string WhereClause = " WHERE ItemID = '" + grdSearch.ActiveRow.Cells[0].Text.Trim().Replace("'", "''") + "'";
                    itemData = itemSvr.PopulateAdvSearch(WhereClause, ref isVendRequired, ref isItemVendorRequired);
                    if (Configuration.isNullOrEmptyDataSet(itemData) == false)
                    {
                        for (int Index = 0; Index < itemData.Tables[0].Columns.Count; Index++)
                        {
                            if (itemData.Tables[0].Rows[0][Index].GetType().Name != "DBNull")
                                grdSearch.ActiveRow.Cells[Index].Value = Configuration.convertNullToString(itemData.Tables[0].Rows[0][Index]);
                            else
                            {
                                grdSearch.ActiveRow.Cells[Index].Value = DBNull.Value;
                            }

                        }
                        grdSearch.UpdateData();
                    }
                }
                //End
                logger.Trace("EditItem() - " + clsPOSDBConstants.Log_Exiting);
            }
            catch (Exception exp)
            {
                logger.Fatal(exp, "EditItem()");
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }

        private void EndOfDayClose()
        {
            if (grdSearch.Rows.Count <= 0) return;

            try
            {
                logger.Trace("EndOfDayClose() - " + clsPOSDBConstants.Log_Entering);
                frmEndOfDay oEndOfDay = new frmEndOfDay();
                int id = Convert.ToInt32(grdSearch.ActiveRow.Cells[0].Text);
                oEndOfDay.Edit(id);
                oEndOfDay.StartPosition = FormStartPosition.CenterScreen;
                oEndOfDay.ShowDialog(this);
                logger.Trace("EndOfDayClose() - " + clsPOSDBConstants.Log_Exiting);
            }
            catch (Exception exp)
            {
                logger.Fatal(exp, "EndOfDayClose()");
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }
        #endregion

        #region Delete record
        private void Delete()
        {
            try
            {
                logger.Trace("Delete() - " + clsPOSDBConstants.Log_Entering);
                switch (SearchTable)
                {
                    case clsPOSDBConstants.Department_tbl:
                        DeleteDepartment();
                        break;
                    case clsPOSDBConstants.TaxCodes_tbl:
                        DeleteTaxcode();
                        break;
                    case clsPOSDBConstants.Item_tbl:
                        DeleteItem();
                        break;
                    case clsPOSDBConstants.Vendor_tbl:
                        DeleteVendor();
                        break;
                    case clsPOSDBConstants.Users_tbl:
                        DeleteUsers();
                        break;
                    case clsPOSDBConstants.Customer_tbl:
                        DeleteCustomer();
                        break;
                    case clsPOSDBConstants.PayType_tbl:
                        DeletePayType();
                        break;
                    case clsPOSDBConstants.POHeader_tbl:
                        DeletePo();
                        break;
                    case clsPOSDBConstants.CLPointsRewardTier_tbl:
                        DeleteCLPointsRewardTier();
                        break;
                    case clsPOSDBConstants.UsersGroup_tbl:
                        DeleteUsersGroup();
                        break;
                    case clsPOSDBConstants.CLCards_tbl:
                        DactivateCLCard();
                        break;
                    case clsPOSDBConstants.TransFee_tbl:    //PRIMEPOS-3116 11-Jul-2022 JY Added
                        DeleteTransFee();
                        break;
                }
                logger.Trace("Delete() - " + clsPOSDBConstants.Log_Exiting);
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "Delete()");
            }
        }

        private void DactivateCLCard()
        {
            long DeactivatingCardId = 0;
            long MergingCardID = 0;
            CLCards oCLCards = new CLCards();
            CLCardsData oCLData = null;
            try
            {
                logger.Trace("DactivateCLCard() - " + clsPOSDBConstants.Log_Entering);
                if (this.grdSearch.ActiveRow == null)
                {
                    return;
                }
                else if (Resources.Message.Display("Deactivating card will make current points and coupons of card un-usable.\nIf you wish to add current card points and coupons to another card \nof same customer then please select merge option.\nAre you sure, you want to deactivate selected card?",
                    "Deactivate Card", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == System.Windows.Forms.DialogResult.No)
                {
                    return;
                }
                DeactivatingCardId = Configuration.convertNullToInt64(this.grdSearch.ActiveRow.Cells["CL Card #"].Text);    //Sprint-19 - 26-Mar-2015 JY Corrected the column name from "Card #" to "CL Card #" 
                string sCustId = Configuration.convertNullToString(this.grdSearch.ActiveRow.Cells[clsPOSDBConstants.Customer_Fld_CustomerId].Text);
                if (string.IsNullOrEmpty(sCustId) == true)
                {
                    oCLData = oCLCards.GetByCLCardID(DeactivatingCardId);
                }
                else
                {
                    oCLData = oCLCards.GetByCustomerID(Configuration.convertNullToInt(sCustId));
                }
                if (Configuration.isNullOrEmptyDataSet(oCLData) == true)
                {
                    clsUIHelper.ShowErrorMsg("Failed to deactivate card.");
                    return;
                }

                if (oCLCards.DeactivateCard(DeactivatingCardId) == true)
                {
                    logger.Trace("DactivateCLCard() - " + "Deactivated Selected CL Card# " + sCustId + "  Successfully" + clsPOSDBConstants.Log_Exiting);
                    //ErrorLogging.Logs.Logger("Customer Loyalty", "DactivateCLCard()", "Deactivated Selected CL Card# " + sCustId + "  Successfully");
                    clsUIHelper.ShowSuccessMsg("Card deactivated successfully.", "Card Deactivation");
                    btnSearch_Click(null, null);
                    return;
                }
                logger.Trace("DactivateCLCard() - " + clsPOSDBConstants.Log_Exiting);
            }
            catch (Exception Ex)
            {
                logger.Fatal(Ex, "DactivateCLCard()");
                //ErrorLogging.Logs.Logger("Customer Loyalty", "DactivateCLCard()", "Deactivatiing Selected CL Card# "
                //    + MergingCardID + " Failed Exception Occured " + Ex.StackTrace.ToString());
                clsUIHelper.ShowErrorMsg(Ex.Message);
            }
        }

        private void DeleteItem()
        {
            Item objItem = new Item();
            string itemID = string.Empty;
            try
            {
                logger.Trace("DeleteItem() - " + clsPOSDBConstants.Log_Entering);
                if (grdSearch.ActiveRow != null)
                {
                    itemID = grdSearch.ActiveRow.Cells["Item Code"].Value.ToString();
                    if (Resources.Message.Display("Do you want to Delete Item# " + itemID + "?", "Delete Item", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        if (objItem.DeleteRow(itemID))
                            btnSearch_Click(null, null);
                        else
                            Resources.Message.Display("You cannot delete selected Item.\nItem# " + itemID + "  is used in transaction(s).");
                    }
                }
                logger.Trace("DeleteItem() - " + clsPOSDBConstants.Log_Exiting);
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "DeleteItem()");
                Resources.Message.Display("You cannot delete selected Item.\nItem# " + itemID + "  is used in transaction(s).");
            }
        }

        private void DeleteDepartment()
        {
            Department objDept = new Department();

            try
            {
                logger.Trace("DeleteDepartment() - " + clsPOSDBConstants.Log_Entering);
                if (grdSearch.ActiveRow != null)
                {
                    //string deptID =grdSearch.ActiveRow.Cells[clsPOSDBConstants.Department_Fld_DeptID].Value.ToString();   Commented By Shitaljit QuicSolv on 4 oct 2011
                    //Added By Shitaljit QuicSolv on 4 oct 2011
                    DepartmentData oDepartmentData = new DepartmentData();
                    DepartmentRow oDepartmentRow = null;
                    string deptCode = grdSearch.ActiveRow.Cells["code"].Value.ToString();
                    string deptID = "";
                    oDepartmentData = objDept.Populate(deptCode);
                    oDepartmentRow = oDepartmentData.Department.GetRowByID(deptCode);
                    deptID = Configuration.convertNullToString(oDepartmentRow.DeptID);
                    //End of Added By Shitaljit QuicSolv on 4 oct 2011
                    if (Resources.Message.Display("Do you want to Delete Department # " + deptCode + "?", "Delete Department", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        if (objDept.DeleteRow(deptID))
                            btnSearch_Click(null, null);
                        else
                        {
                            //Added By Shitaljit(QuicSolv) on 3 Nov 2011
                            //Modified the Message to be displayed in case of error and add logic to populate the items in which Department is used.
                            Item OItem = new Item();
                            string strwhereclause = "  WHERE DepartmentID = '" + deptID + "'";
                            ItemData oItemData = OItem.PopulateList(strwhereclause);
                            if (oItemData != null && oItemData.Tables[0].Rows.Count > 0) //PRIMEPOS-3309
                            {
                                Resources.Message.Display("Department #" + deptCode + "  holds " + (Configuration.convertNullToInt(oItemData.Tables[0].Rows.Count.ToString())) + " Items,You cannot delete the Department. ");
                            }
                            else //PRIMEPOS-3309
                            {
                                SubDepartment subDept = new SubDepartment();
                                string strwhereclausetemp = "  WHERE DepartmentID = '" + deptID + "'";
                                SubDepartmentData subDeptData = subDept.PopulateList(strwhereclausetemp);
                                Resources.Message.Display("Department #" + deptCode + "  holds " + (Configuration.convertNullToInt(subDeptData.Tables[0].Rows.Count.ToString())) + " SubDepartments,You cannot delete the Department. ");
                            }
                        }
                    }
                }
                logger.Trace("DeleteDepartment() - " + clsPOSDBConstants.Log_Exiting);
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "DeleteDepartment()");
                clsUIHelper.ShowErrorMsg(ex.Message);
            }
        }

        private void DeleteCLPointsRewardTier()
        {
            CLPointsRewardTier tier = new CLPointsRewardTier();

            try
            {
                logger.Trace("DeleteCLPointsRewardTier() - " + clsPOSDBConstants.Log_Entering);
                if (grdSearch.ActiveRow != null)
                {
                    CLPointsRewardTierData oData = new CLPointsRewardTierData();
                    int id = Configuration.convertNullToInt(grdSearch.ActiveRow.Cells["code"].Value.ToString());
                    if (Resources.Message.Display("Do you want to delete selected CL Points Reward Tier ?", "Delete CL Points Reward Tier", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        if (tier.DeleteRow(id))
                            btnSearch_Click(null, null);
                    }
                }
                logger.Trace("DeleteCLPointsRewardTier() - " + clsPOSDBConstants.Log_Exiting);
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "DeleteCLPointsRewardTier()");
                clsUIHelper.ShowErrorMsg(ex.Message);
            }
        }

        private void DeleteTaxcode()
        {
            TaxCodes objTax = new TaxCodes();
            try
            {
                logger.Trace("DeleteTaxcode() - " + clsPOSDBConstants.Log_Entering);
                if (grdSearch.ActiveRow != null)
                {
                    string TaxCodeID = grdSearch.ActiveRow.Cells[clsPOSDBConstants.TaxCodes_Fld_TaxID].Value.ToString();
                    if (Resources.Message.Display("Do you want to Delete TaxCode # " + TaxCodeID + "?", "Delete Tax", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        bool bRxTax = false;
                        if (objTax.DeleteRow(TaxCodeID, ref bRxTax))
                        {
                            btnSearch_Click(null, null);
                        }
                        else
                        {
                            if (bRxTax)
                                Resources.Message.Display("" + TaxCodeID + " We cant delete RxTax Code");
                            else
                                Resources.Message.Display("" + TaxCodeID + " Tax Code is in use");
                        }
                    }
                }
                logger.Trace("DeleteTaxcode() - " + clsPOSDBConstants.Log_Exiting);
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "DeleteTaxcode()");
                clsUIHelper.ShowErrorMsg(ex.Message);
            }
        }

        private void DeleteVendor()
        {
            POS_Core.BusinessRules.Vendor objTax = new POS_Core.BusinessRules.Vendor();
            try
            {
                logger.Trace("DeleteVendor() - " + clsPOSDBConstants.Log_Entering);
                if (grdSearch.ActiveRow != null)
                {
                    string vendorID = grdSearch.ActiveRow.Cells[clsPOSDBConstants.Vendor_Fld_VendorId].Value.ToString();
                    ItemVendor objItemVendor = new ItemVendor();
                    if (Resources.Message.Display("Do you want to Delete Vendor # " + grdSearch.ActiveRow.Cells["Name"].Value.ToString() + "?", "Delete Vendor", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        if (objTax.DeleteRow(vendorID))
                            btnSearch_Click(null, null);
                        else
                            Resources.Message.Display("" + grdSearch.ActiveRow.Cells["Name"].Value.ToString() + " Vendor is in use");
                    }
                }
                logger.Trace("DeleteVendor() - " + clsPOSDBConstants.Log_Exiting);
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "DeleteVendor()");
                clsUIHelper.ShowErrorMsg(ex.Message);
            }
        }

        private void DeleteUsers()
        {
            POS_Core.BusinessRules.Search objSearch = new POS_Core.BusinessRules.Search();
            try
            {
                logger.Trace("DeleteUsers() - " + clsPOSDBConstants.Log_Entering);
                if (grdSearch.ActiveRow != null)
                {
                    string userID = grdSearch.ActiveRow.Cells[clsPOSDBConstants.Users_Fld_UserID].Value.ToString();
                    if (Resources.Message.Display("Do you want to Delete User # " + grdSearch.ActiveRow.Cells[0].Value.ToString() + "?", "Delete User", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        //if (objSearch.DeleteUserRow(userID))
                        objSearch.DeleteUserRow(userID);
                        btnSearch_Click(null, null);
                        //else
                        //    Resources.Message.Display("User " + userID + " is in use");
                    }
                }
                logger.Trace("DeleteUsers() - " + clsPOSDBConstants.Log_Exiting);
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "DeleteUsers()");
                string userID = grdSearch.ActiveRow.Cells[clsPOSDBConstants.Users_Fld_UserID].Value.ToString();
                Resources.Message.Display("User " + userID + " is in use");
            }
        }

        private void DeleteUsersGroup()
        {
            POS_Core.BusinessRules.Search objSearch = new POS_Core.BusinessRules.Search();
            try
            {
                logger.Trace("DeleteUsersGroup() - " + clsPOSDBConstants.Log_Entering);
                if (grdSearch.ActiveRow != null)
                {
                    string userID = grdSearch.ActiveRow.Cells[clsPOSDBConstants.Users_Fld_UserID].Value.ToString();
                    if (Resources.Message.Display("Do you want to delete user group " + grdSearch.ActiveRow.Cells[0].Value.ToString() + "?", "Delete User Group", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        //if (objSearch.DeleteUserRow(userID, false)) //PRIMEPOS-2780 27-Sep-2021 JY commented
                        objSearch.DeleteUserRow(userID, false);  //PRIMEPOS-2780 27-Sep-2021 JY Added
                        btnSearch_Click(null, null);
                        //else
                        //    Resources.Message.Display("User group" + userID + " is in use");
                    }
                }
                logger.Trace("DeleteUsersGroup() - " + clsPOSDBConstants.Log_Exiting);
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "DeleteUsersGroup()");
                string userID = grdSearch.ActiveRow.Cells[clsPOSDBConstants.Users_Fld_UserID].Value.ToString();
                //Resources.Message.Display("User " + userID + " is in use");
            }
        }

        private void DeleteCustomer()
        {
            POS_Core.BusinessRules.Customer objCustomer = new POS_Core.BusinessRules.Customer();
            try
            {
                logger.Trace("DeleteCustomer() - " + clsPOSDBConstants.Log_Entering);
                if (grdSearch.ActiveRow != null)
                {
                    string CustomerID = grdSearch.ActiveRow.Cells[clsPOSDBConstants.Customer_Fld_CustomerId].Value.ToString();
                    string AccountNumber = grdSearch.ActiveRow.Cells["Account#"].Value.ToString();

                    if (AccountNumber == "-1")
                    {
                        clsUIHelper.ShowErrorMsg("You cannot delete default customer.");
                    }
                    else
                    {
                        if (Resources.Message.Display("Do you want to delete Customer  " + grdSearch.ActiveRow.Cells["Name"].Value.ToString().Trim() + "?", "Delete Customer", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            if (objCustomer.IsCustomerAssociatedWithTransaction(CustomerID))    //PRIMEPOS-2598 12-Oct-2018 JY Added
                            {
                                clsUIHelper.ShowErrorMsg("You cannot delete this customer as it is associated with the transaction(s).");
                            }
                            else
                            {
                                if (objCustomer.DeleteRow(CustomerID))
                                {
                                    btnSearch_Click(null, null);
                                }
                                else
                                {
                                    string CustomerName = grdSearch.ActiveRow.Cells["Name"].Value.ToString();
                                    Resources.Message.Display("Cannot delete the selected customer.\nCustomer " + CustomerName + " is in used.");
                                }
                            }
                        }
                    }
                }
                logger.Trace("DeleteCustomer() - " + clsPOSDBConstants.Log_Exiting);
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "DeleteCustomer()");
                string CustomerName = grdSearch.ActiveRow.Cells["Name"].Value.ToString();
                Resources.Message.Display("Cannot delete the selected customer.\nCustomer " + CustomerName + " is in used.");
            }
        }

        //Added BY Ravindra for Delete incomplete/Expiered PO
        private void DeletePo()
        {
            logger.Trace("DeletePo() - " + clsPOSDBConstants.Log_Entering);
            string message = " Are you sure you wants to Delete  Order  ";
            if (pnlDelete.Enabled && Resources.Message.Display(message, "Delete  Order", MessageBoxButtons.YesNo, MessageBoxIcon.Information, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
            {
                PurchaseOrder purchaseOrder = new PurchaseOrder();
                POHeaderData poHeaderData = new POHeaderData();
                PODetailData poDetailData = new PODetailData();

                try
                {
                    double orderId = Convert.ToInt32(grdSearch.Selected.Rows[0].Cells[clsPOSDBConstants.POHeader_Fld_OrderID].Value);
                    poHeaderData = purchaseOrder.PopulateHeader(Convert.ToInt32(grdSearch.Selected.Rows[0].Cells[clsPOSDBConstants.POHeader_Fld_OrderID].Value));
                    poDetailData = purchaseOrder.PopulateDetail(poHeaderData.POHeader[0].OrderID);
                    poHeaderData.POHeader[0].Delete();
                    poDetailData.PODetail[0].Delete();
                    purchaseOrder.Persist(poHeaderData, poDetailData);
                    btnSearch_Click(null, null);
                }
                catch (Exception ex)
                {
                    logger.Fatal(ex, "DeletePo()");
                    clsUIHelper.ShowErrorMsg("Order Can Not Be Delete");
                }
            }
            logger.Trace("DeletePo() - " + clsPOSDBConstants.Log_Exiting);
        }

        //Added By shitaljit to delete payment types.
        private void DeletePayType()
        {
            POS_Core.BusinessRules.PayType oPayType = new POS_Core.BusinessRules.PayType();
            try
            {
                logger.Trace("DeletePayType() - " + clsPOSDBConstants.Log_Entering);
                if (grdSearch.ActiveRow != null)
                {
                    string PaytypeID = grdSearch.ActiveRow.Cells["Code"].Value.ToString();
                    string PaytypeDesc = grdSearch.ActiveRow.Cells["Description"].Value.ToString();

                    if (Resources.Message.Display("Do you want to delete payment type \"" + PaytypeDesc + "\"?", "Delete Payment Type", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        if (oPayType.DeleteRow(PaytypeID))
                        {
                            btnSearch_Click(null, null);
                        }
                        else
                        {
                            clsUIHelper.ShowErrorMsg("You cannot delete payment type \"" + PaytypeDesc + "\" is already use in transaction(s).");
                        }
                    }
                }
                logger.Trace("DeletePayType() - " + clsPOSDBConstants.Log_Exiting);
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "DeletePayType()");
                string PaytypeDesc = grdSearch.ActiveRow.Cells["Description"].Value.ToString();
                clsUIHelper.ShowErrorMsg("Payment Type \"" + PaytypeDesc + "\" is in use"); ;
            }
        }

        #endregion                       

        public CustomerRow SelectedRow()
        {
            logger.Trace("SelectedRow() - " + clsPOSDBConstants.Log_Entering);
            CustomerRow oCustRow = null;
            if (grdSearch.ActiveRow != null)
            {
                if (grdSearch.ActiveRow.Cells.Count > 0 && this.oCustomerData.Tables[0].Rows.Count > 0 && this.chkIncludeRXCust.Checked == true)
                {
                    foreach (CustomerRow oRow in oCustomerData.Tables[0].Rows)
                    {
                        if (oRow.CustomerId == POS_Core.Resources.Configuration.convertNullToInt(grdSearch.ActiveRow.Cells[clsPOSDBConstants.CLCards_Fld_CustomerID].Text))
                        {
                            oCustRow = oRow;
                            break;
                        }
                    }
                }
            }
            logger.Trace("SelectedRow() - " + clsPOSDBConstants.Log_Exiting);
            return oCustRow;
        }

        private void resizeColumns()
        {
            logger.Trace("resizeColumns() - " + clsPOSDBConstants.Log_Entering);
            grdSearch.Refresh();
            foreach (UltraGridColumn oCol in grdSearch.DisplayLayout.Bands[0].Columns)
            {
                oCol.Width = oCol.CalculateAutoResizeWidth(PerformAutoSizeType.VisibleRows, true) + 10;
                if (oCol.DataType.Equals(typeof(System.Int32)) || oCol.DataType.Equals(typeof(System.Decimal)) || oCol.DataType.Equals(typeof(System.DateTime)))
                {
                    oCol.CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right;
                }
            }
            logger.Trace("resizeColumns() - " + clsPOSDBConstants.Log_Exiting);
        }

        private void EnableOrDisable(String status)
        {
            DataSet searchDataSet = null;
            DataRow[] rows = null;
            try
            {
                logger.Trace("EnableOrDisable() - " + clsPOSDBConstants.Log_Entering);
                searchDataSet = (DataSet)this.grdSearch.DataSource;
                if (searchDataSet.Tables.Count > 0)
                {
                    rows = searchDataSet.Tables[0].Select("[PO Status] = '" + status + "'");
                    if (rows.Length > 0 && status == clsPOSDBConstants.Incomplete)
                    {
                        this.pnlEdit.Enabled = true;
                    }
                    else
                    {
                        this.pnlEdit.Enabled = false;
                    }
                }
                logger.Trace("EnableOrDisable() - " + clsPOSDBConstants.Log_Exiting);
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "EnableOrDisable()");
                clsUIHelper.ShowErrorMsg(ex.Message);
            }
        }

        private void FillOrder()
        {
            frmCreateNewPurchaseOrder editPO = null;
            String poStatus = String.Empty;
            try
            {
                logger.Trace("FillOrder() - " + clsPOSDBConstants.Log_Entering);
                editPO = new frmCreateNewPurchaseOrder();
                editPO.IsEditPO = true;
                string poNumber = grdSearch.ActiveRow.Cells["OrderID"].Text;
                poStatus = grdSearch.ActiveRow.Cells["PO Status"].Text;

                if (clsPOSDBConstants.PartiallyAcknowledge == poStatus)
                {
                    editPO.IsPartiallyAck = true;
                    editPO.Edit(poNumber);
                    editPO.ShowDialog(this);
                }
                else
                {
                    clsUIHelper.ShowErrorMsg("For Partial Fill PO Status should be Partially Acknowledged");
                }
                if (!editPO.IsCanceled)
                    btnSearch_Click(null, null);
                logger.Trace("FillOrder() - " + clsPOSDBConstants.Log_Exiting);
            }
            catch (Exception exp)
            {
                logger.Fatal(exp, "FillOrder()");
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
            finally
            {
                editPO.IsPartiallyAck = false;
            }
        }

        /// <summary>
        /// Author: Gaurav 
        /// Date: 06/07/2009
        /// Details: Implements Force Fully access of PO if the status is Template.
        /// </summary>
        /// <param name="CopyFlagged"></param>
        private void CopyOrder(bool CopyFlagged)
        {
            frmCreateNewPurchaseOrder editPO = null;
            String poStatus = String.Empty;
            try
            {
                logger.Trace("CopyOrder() - " + clsPOSDBConstants.Log_Entering);
                editPO = new frmCreateNewPurchaseOrder();
                editPO.IsEditPO = true;
                string poNumber = grdSearch.ActiveRow.Cells["OrderID"].Text;
                poStatus = grdSearch.ActiveRow.Cells["PO Status"].Text;

                if (clsPOSDBConstants.Expired == poStatus || clsPOSDBConstants.Canceled == poStatus || CopyFlagged == true)
                {
                    editPO.IsCopyOrder = true;
                    editPO.Edit(poNumber);
                    editPO.ShowDialog(this);
                }
                else
                {
                    clsUIHelper.ShowErrorMsg("For Copy Order PO Status should be Expired/Cancelled/Flagged");
                }
                if (!editPO.IsCanceled)
                    btnSearch_Click(null, null);
                logger.Trace("CopyOrder() - " + clsPOSDBConstants.Log_Exiting);
            }
            catch (Exception exp)
            {
                logger.Fatal(exp, "CopyOrder()");
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
            finally
            {
                editPO.IsCopyOrder = false;
            }
        }

        private void AcknowledgeManually()
        {
            POHeaderData oPOHeaderData = null;
            PODetailData oPODetailData = null;
            PurchaseOrder oPurchaseOrder = null;
            try
            {
                logger.Trace("AcknowledgeManually() - " + clsPOSDBConstants.Log_Entering);
                if (grdSearch.Selected.Rows.Count == 0)
                    return;
                oPurchaseOrder = new PurchaseOrder();
                //Changed by Prashant(SRT) Date:30-5-09
                String poStatus = grdSearch.Selected.Rows[0].Cells["PO Status"].Value.ToString();
                //End of Changed by Prashant(SRT) Date:30-5-09
                //poStatus == clsPOSDBConstants.Incomplete is added By Shitaljit on 20 Feb
                if (poStatus == clsPOSDBConstants.Expired || poStatus == clsPOSDBConstants.Canceled || poStatus == clsPOSDBConstants.SubmittedManually || poStatus == clsPOSDBConstants.Incomplete)
                {
                    string POID = grdSearch.ActiveRow.Cells[0].Text;
                    oPOHeaderData = oPurchaseOrder.PopulateHeader(Convert.ToInt32(POID));
                    oPODetailData = oPurchaseOrder.PopulateDetail(Convert.ToInt32(POID));
                    oPOHeaderData.POHeader[0].Status = (int)PurchseOrdStatus.AcknowledgeManually;
                    oPurchaseOrder.Persist(oPOHeaderData, oPODetailData);
                    btnSearch_Click(null, null);
                }
                else
                {
                    //Changed by shitaljit on 20 feb 2012 added "SubmittedManually or Incomplete" in the message.
                    clsUIHelper.ShowErrorMsg("For Manual Acknowledgement, PO Status should be Expired or Cancelled or SubmittedManually or Incomplete");
                }
                logger.Trace("AcknowledgeManually() - " + clsPOSDBConstants.Log_Exiting);
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "AcknowledgeManually()");
                clsUIHelper.ShowErrorMsg(ex.Message);
            }
        }

        private void Preview(bool PrintId)
        {
            try
            {
                logger.Trace("Preview() - " + clsPOSDBConstants.Log_Entering);
                if (!this.chkPrimtUserImg.Checked)
                {
                    if (oDataSet == null)
                    {
                        return;
                    }
                    else if (oDataSet.Tables[0].Rows.Count == 0)
                    {
                        return;
                    }

                    rptUsersLabel oRpt = new rptUsersLabel();
                    Mabry.Windows.Forms.Barcode.Licenser.Key = "E1P8-HKELVF8R04Q0";
                    DataSet rptDS = new DataSet();
                    rptDS.Tables.Add();
                    rptDS.Tables[0].Columns.Add("ItemID");
                    rptDS.Tables[0].Columns.Add("Picture", System.Type.GetType("System.Byte[]"));
                    User oUser = new User();
                    UserData OUserData = new UserData();
                    UserRow oUserRow = null;
                    foreach (DataRow oRow in oDataSet.Tables[0].Rows)
                    {
                        try
                        {
                            // string sBarcode = "99" + EncryptString.CustomEncrypt(Convert.ToInt32(oRow["ID"].ToString())) + "99";
                            string sBarcode = Convert.ToInt32(oRow["ID"].ToString()) + clsPOSDBConstants.UserBarcodeSeperatorString;
                            OUserData = oUser.GetUserByUserID(oRow["UserID"].ToString());
                            if (Configuration.isNullOrEmptyDataSet(OUserData) == false)
                            {
                                oUserRow = (UserRow)OUserData.User.Rows[0];
                                string SPassword = EncryptString.Decrypt(oUserRow.Password);
                                sBarcode += EncryptString.Encrypt(SPassword);
                            }
                            this.PrintBarcode(sBarcode, 0, 0, 20, 300, "CODE128", "H", System.IO.Path.GetTempPath() + "\\" + oRow[0].ToString() + ".bmp");
                            rptDS.Tables[0].Rows.Add(new object[] { oRow[0].ToString(), GetImageData(System.IO.Path.GetTempPath() + "\\" + oRow[0].ToString() + ".bmp") });
                        }
                        catch (Exception exp)
                        {
                            logger.Fatal(exp, "Preview()");
                            clsUIHelper.ShowErrorMsg(exp.Message);
                        }
                    }
                    oRpt.Database.Tables[0].SetDataSource(rptDS.Tables[0]);
                    if (PrintId == false)
                    {
                        clsReports.ShowReport(oRpt);
                    }
                    else
                    {
                        clsReports.PrintReport(oRpt);
                    }
                }
                else
                {
                    if (oDataSet == null)
                    {
                        return;
                    }
                    else if (oDataSet.Tables[0].Rows.Count == 0)
                    {
                        return;
                    }

                    rptUserIDWithPicture oRpt = new rptUserIDWithPicture();
                    Mabry.Windows.Forms.Barcode.Licenser.Key = "E1P8-HKELVF8R04Q0";
                    DataSet rptDS = new DataSet();
                    rptDS.Tables.Add();
                    rptDS.Tables[0].Columns.Add("Lname");
                    rptDS.Tables[0].Columns.Add("Fname");
                    rptDS.Tables[0].Columns.Add("picture", System.Type.GetType("System.Byte[]"));
                    rptDS.Tables[0].Columns.Add("UserImg", System.Type.GetType("System.Byte[]"));
                    User oUser = new User();
                    UserData OUserData = new UserData();
                    UserRow oUserRow = null;
                    foreach (DataRow oRow in oDataSet.Tables[0].Rows)
                    {
                        try
                        {
                            // string sBarcode = "99" + EncryptString.CustomEncrypt(Convert.ToInt32(oRow["ID"].ToString())) + "99";
                            string sBarcode = Convert.ToInt32(oRow["ID"].ToString()) + clsPOSDBConstants.UserBarcodeSeperatorString;
                            OUserData = oUser.GetUserByUserID(oRow["UserID"].ToString());
                            if (Configuration.isNullOrEmptyDataSet(OUserData) == false)
                            {
                                oUserRow = (UserRow)OUserData.User.Rows[0];
                                string SPassword = EncryptString.Decrypt(oUserRow.Password);
                                sBarcode += EncryptString.Encrypt(SPassword);
                            }
                            this.PrintBarcode(sBarcode, 0, 0, 20, 300, "CODE128", "H", System.IO.Path.GetTempPath() + "\\" + oRow[0].ToString() + ".bmp");
                            DataRow dr = rptDS.Tables[0].NewRow();


                            dr["Fname"] = oUserRow.FirstName;
                            dr["Lname"] = oUserRow.LastName;
                            dr["UserImg"] = oUserRow.UserImage;
                            dr["picture"] = GetImageData(System.IO.Path.GetTempPath() + "\\" + oRow[0].ToString() + ".bmp");

                            rptDS.Tables[0].Rows.Add(dr);
                            // rptDS.Tables[0].Rows.Add(new object[] { oRow[0].ToString(),  });
                        }
                        catch (Exception exp)
                        {
                            logger.Fatal(exp, "Preview()");
                            clsUIHelper.ShowErrorMsg(exp.Message);
                        }
                    }

                    oRpt.Database.Tables[0].SetDataSource(rptDS.Tables[0]);
                    if (PrintId == false)
                    {
                        clsReports.ShowReport(oRpt);
                    }
                    else
                    {
                        clsReports.PrintReport(oRpt);
                    }
                }
                logger.Trace("Preview() - " + clsPOSDBConstants.Log_Exiting);
            }
            catch (Exception exp)
            {
                logger.Fatal(exp, "Preview()");
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }

        public bool PrintBarcode(string Barcode, long lnX, long lnY, int Height, int Width, string BCType, string Orientation, string sFilePath)
        {
            logger.Trace("PrintBarcode() - " + clsPOSDBConstants.Log_Entering);
            bool bPrOk = true;
            Mabry.Windows.Forms.Barcode.Licenser.Key = "E1P8-HKELVF8R04Q0";
            Mabry.Windows.Forms.Barcode.Barcode BC = new Mabry.Windows.Forms.Barcode.Barcode();
            BC.Size = new Size(Height - 15, Width);
            Image bci;
            BC.Data = Barcode;
            BC.DisplayAlignment = Mabry.Windows.Forms.Barcode.Barcode.TextAlignment.Standard;

            //BC.DisplayData=false;
            //BC.Xunit=8;
            //BC.BarcodeHeight = Height;

            switch (BCType)
            {
                case "CODE39":
                    BC.Symbology = Mabry.Windows.Forms.Barcode.Barcode.BarcodeSymbologies.Code39;
                    Width *= 2;
                    Height *= 2;
                    BC.ChecksumStyle = Mabry.Windows.Forms.Barcode.Barcode.ChecksumStyles.None;
                    break;
                case "CODE128": // coDe 128
                    BC.Symbology = Mabry.Windows.Forms.Barcode.Barcode.BarcodeSymbologies.Code128;
                    break;
                case "CODE128B":
                    BC.Symbology = Mabry.Windows.Forms.Barcode.Barcode.BarcodeSymbologies.Code128B;
                    break;
                case "CODE128A":
                    BC.Symbology = Mabry.Windows.Forms.Barcode.Barcode.BarcodeSymbologies.Code128A;
                    break;

                case "IL25": // interleaved 2 of 5
                    BC.Symbology = Mabry.Windows.Forms.Barcode.Barcode.BarcodeSymbologies.Interleaved2of5;
                    break;

                case "ST25": // standard 2 of 5
                    BC.Symbology = Mabry.Windows.Forms.Barcode.Barcode.BarcodeSymbologies.Code2of5;
                    break;

                case "UPCA": // upc A
                    BC.Symbology = Mabry.Windows.Forms.Barcode.Barcode.BarcodeSymbologies.UPCA;
                    break;

                case "UPCE": // upc E
                    BC.Symbology = Mabry.Windows.Forms.Barcode.Barcode.BarcodeSymbologies.UPCE;
                    break;

                case "CODE39X":
                    BC.Symbology = Mabry.Windows.Forms.Barcode.Barcode.BarcodeSymbologies.Code39Extended;
                    break;
                default:
                    BC.Symbology = Mabry.Windows.Forms.Barcode.Barcode.BarcodeSymbologies.Code39;
                    Width *= 2;
                    Height *= 2;
                    BC.ChecksumStyle = Mabry.Windows.Forms.Barcode.Barcode.ChecksumStyles.None;
                    break;
            }

            //			if (Orientation == "V")
            //				BC.Orientation = 90;

            BC.BarRatio = BC.SuggestedBarRatio;
            //BC.BarRatio = 1.75F;

            bci = BC.Image(Width * 3, Height * 3);
            if (Orientation == "V")
                bci.RotateFlip(RotateFlipType.Rotate90FlipNone);//.Rotate270FlipXY); //.Rotate270FlipNone);

            //PR.Graphics.DrawImage(BC.Barcode(this.PR.Graphics),lnX, lnY);
            bci.Save(sFilePath);
            logger.Trace("PrintBarcode() - " + clsPOSDBConstants.Log_Exiting);
            return bPrOk;
        }

        private byte[] GetImageData(String fileName)
        {
            //'Method to load an image from disk and return it as a bytestream
            System.IO.FileStream fs = new System.IO.FileStream(fileName, System.IO.FileMode.Open, System.IO.FileAccess.Read);
            System.IO.BinaryReader br = new System.IO.BinaryReader(fs);
            return (br.ReadBytes(Convert.ToInt32(br.BaseStream.Length)));
        }

        /// <summary>
        /// Author: Shitaljit
        /// Print the search result from grid.
        /// </summary>
        /// <param name="sSearchTable"></param>
        private void PrintGridData(string sSearchTable)
        {
            try
            {
                logger.Trace("PrintGridData() - " + clsPOSDBConstants.Log_Entering);
                switch (sSearchTable)
                {
                    case clsPOSDBConstants.Item_tbl:
                        rptItemFile oRpt = new rptItemFile();
                        if (IsAdvSearchDone == false)
                        {
                            oRpt.SetDataSource(oDataSet.Tables[0]);
                            clsReports.DStoExport = oDataSet;   //PRIMEPOS-2471 16-Feb-2021 JY Added
                        }
                        else
                        {
                            oRpt.SetDataSource(FrmAdvSearch.itemData.Tables[0]);
                            clsReports.DStoExport = FrmAdvSearch.itemData;   //PRIMEPOS-2471 16-Feb-2021 JY Added
                        }
                        clsReports.Preview(false, oRpt);
                        break;
                }
                logger.Trace("PrintGridData() - " + clsPOSDBConstants.Log_Exiting);
            }
            catch (Exception exp)
            {
                logger.Fatal(exp, "PrintGridData()");
                throw;
            }
        }

        private void ReSubmitOrder()
        {
            PurchaseOrder oPurchaseOrder = null;
            try
            {
                logger.Trace("ReSubmitOrder() - " + clsPOSDBConstants.Log_Entering);
                if (grdSearch.Selected.Rows.Count == 0)
                    return;
                oPurchaseOrder = new PurchaseOrder();
                String poStatus = grdSearch.Selected.Rows[0].Cells["PO Status"].Value.ToString();
                if (poStatus == clsPOSDBConstants.Expired || poStatus == clsPOSDBConstants.Error || poStatus == clsPOSDBConstants.Submitted || poStatus == clsPOSDBConstants.Overdue)
                {
                    PurchaseOrder purchaseOrder = new PurchaseOrder();
                    POHeaderData poHeaderData = new POHeaderData();
                    PODetailData poDetailData = new PODetailData();

                    poHeaderData = purchaseOrder.PopulateHeader(Convert.ToInt32(grdSearch.Selected.Rows[0].Cells[clsPOSDBConstants.POHeader_Fld_OrderID].Value));
                    Dictionary<long, string> dictStatus = new Dictionary<long, string>();
                    if (poHeaderData.POHeader.Rows.Count > 0)
                    {
                        dictStatus.Add(poHeaderData.POHeader[0].PrimePOrderId, PurchseOrdStatus.Queued.ToString());
                    }
                    if (PrimePOUtil.DefaultInstance.ReSubmitPO(dictStatus))
                    {
                        poHeaderData.POHeader[0].Status = (int)PurchseOrdStatus.Queued;
                        poDetailData = purchaseOrder.PopulateDetail(poHeaderData.POHeader[0].OrderID);
                        purchaseOrder.Persist(poHeaderData, poDetailData);
                        btnSearch_Click(null, null);

                        ClsUpdatePOStatus.UpdateStatusInst.FillLogDataSet("Purchase Order -" + poHeaderData.POHeader[0].OrderNo + " , Status -" + PurchseOrdStatus.Queued.ToString());
                        ClsUpdatePOStatus.UpdateStatusInst.UpdatePOCount();
                    }
                    else
                    {
                        clsUIHelper.ShowErrorMsg("PrimePO is offline. Order can not be Re-Submitted");
                        return;
                    }
                }
                else
                {
                    clsUIHelper.ShowErrorMsg("For Resubmission PO status should be Expired/Submitted/Overdue");
                }
                logger.Trace("ReSubmitOrder() - " + clsPOSDBConstants.Log_Exiting);
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "ReSubmitOrder()");
                clsUIHelper.ShowErrorMsg(ex.Message);
            }
        }

        /// <summary>
        /// Author: Shitaljit Created Date: 10/10/2013
        /// Added To merge points and coupons of two or nore CL cards
        /// </summary>
        private void MergeCLCard()
        {
            long MergingingCardId = 0;
            CLCards oCLCards = new CLCards();
            CLCardsData oCLData = null;
            try
            {
                logger.Trace("MergeCLCard() - " + clsPOSDBConstants.Log_Entering);
                if (this.grdSearch.ActiveRow == null)
                {
                    return;
                }

                MergingingCardId = Configuration.convertNullToInt64(this.grdSearch.ActiveRow.Cells["CL Card #"].Text); //Sprint-19 - 15-May-2015 JY Corrected the column name from "Card #" to "CL Card #" 
                string sCustId = Configuration.convertNullToString(this.grdSearch.ActiveRow.Cells[clsPOSDBConstants.Customer_Fld_CustomerId].Text);
                if (string.IsNullOrEmpty(sCustId) == true)
                {
                    clsUIHelper.ShowErrorMsg("Please assign a customer to card.");
                    return;
                }
                else
                {
                    oCLData = oCLCards.GetByCustomerID(Configuration.convertNullToInt(sCustId));
                }
                //Modified hte error message By Shitaljit on 11/28/2013 for
                // PRIMEPOS-1654  "Unable to perform merge. Selected customer has only 1 card.
                if (Configuration.isNullOrEmptyDataSet(oCLData) == true)
                {
                    //clsUIHelper.ShowErrorMsg("Unable to perform merge, customer belonging this card has only 1 card.");
                    clsUIHelper.ShowErrorMsg("Unable to perform merge. Selected customer has only 1 card.");
                    return;
                }
                if (oCLData.CLCards.Rows.Count == 1)
                {
                    //clsUIHelper.ShowErrorMsg("Unable to perform merge, customer belonging this card has only 1 card.");
                    clsUIHelper.ShowErrorMsg("Unable to perform merge. Selected customer has only 1 card.");
                    return;
                }

                List<long> listDeactivatingCardIds = new List<long>();
                oCLData.CLCards.Rows[0][clsPOSDBConstants.CLCards_Fld_IsActive] = false;
                logger.Trace("MergeCLCard() - " + "Deactivating Selected CL Card# " + sCustId);
                //ErrorLogging.Logs.Logger("Customer Loyalty", "MergeCLCard()", "Deactivating Selected CL Card# " + sCustId);
                frmViewCLInfo ofrmViewCLInfo = new frmViewCLInfo(sCustId, MergingingCardId, true);
                ofrmViewCLInfo.isCallForDeactivateCard = true;
                ofrmViewCLInfo.ShowDialog();
                if (ofrmViewCLInfo.IsCanceled == false)
                {
                    foreach (UltraGridRow oRow in ofrmViewCLInfo.grdSearch.Rows)
                    {

                        if ((bool)oRow.Cells["check"].Value == true)
                        {
                            listDeactivatingCardIds.Add(Configuration.convertNullToInt64(Configuration.convertNullToInt64(oRow.Cells["CL Card #"].Text))); //Sprint-19 - 15-May-2015 JY Corrected the column name from "Card #" to "CL Card #" 
                        }
                    }
                    if (listDeactivatingCardIds.Count == 0)
                    {
                        return;
                    }
                    if (oCLCards.DeactivateMergeCard(MergingingCardId, listDeactivatingCardIds) == true)
                    {
                        logger.Trace("MergeCLCard() - " + "Deactivated Selected CL Card# " + sCustId + "  Successfully");
                        //ErrorLogging.Logs.Logger("Customer Loyalty", "DactivateCLCard()", "Deactivated Selected CL Card# " + sCustId + "  Successfully");
                        clsUIHelper.ShowSuccessMsg("Card points and coupons merged successfully.", "Card Merge");
                        btnSearch_Click(null, null);
                        return;
                    }
                }
                logger.Trace("MergeCLCard() - " + clsPOSDBConstants.Log_Exiting);
            }
            catch (Exception Ex)
            {
                logger.Fatal(Ex, "MergeCLCard()");
                //ErrorLogging.Logs.Logger("Customer Loyalty", "MergeCLCard()", "Deactivatiing Selected CL Card# "
                //    + MergingingCardId + " Failed Exception Occured " + Ex.StackTrace.ToString());
                clsUIHelper.ShowErrorMsg(Ex.Message);
            }
        }

        /// <summary>
        /// Author: Shitaljit Date: 9/27/2013
        /// Added deactivate CL card 
        /// </summary>

        void unLockUser()
        {
            POS_Core.BusinessRules.User objUser = new POS_Core.BusinessRules.User();
            try
            {
                logger.Trace("unLockUser() - " + clsPOSDBConstants.Log_Entering);
                if (grdSearch.ActiveRow != null)
                {
                    string userID = grdSearch.ActiveRow.Cells[clsPOSDBConstants.Users_Fld_UserID].Value.ToString();
                    if (UserPriviliges.getPermission(UserPriviliges.Modules.POSTransaction.ID, UserPriviliges.Screens.POSTransaction.ID, UserPriviliges.Permissions.OnHoldTrans.ID, UserPriviliges.Permissions.OnHoldTrans.Name))
                    {
                        if (Resources.Message.Display("Do you want to Unlock User # " + grdSearch.ActiveRow.Cells[0].Value.ToString() + "?", "PrimePOS", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            if (objUser.UnlokUserRow(userID))
                                btnSearch_Click(null, null);
                            else
                                Resources.Message.Display("User " + userID + " is in use");

                        }
                    }
                }
                logger.Trace("unLockUser() - " + clsPOSDBConstants.Log_Exiting);
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "unLockUser()");
                string userID = grdSearch.ActiveRow.Cells[clsPOSDBConstants.Users_Fld_UserID].Value.ToString();
                Resources.Message.Display("User " + userID + " is in use");
            }
        }

        private void SetUnlockBtnState(string strLocked)
        {
            try
            {
                logger.Trace("SetUnlockBtnState() - " + clsPOSDBConstants.Log_Entering);
                bool isActiveUser = false;

                if (strLocked == "")
                {
                    isActiveUser = true;
                }
                else
                {
                    isActiveUser = Convert.ToBoolean(strLocked);
                }
                if (isActiveUser)
                {
                    pnlUnlock.Visible = true;
                }
                else
                {
                    pnlUnlock.Visible = false;
                }
                logger.Trace("SetUnlockBtnState() - " + clsPOSDBConstants.Log_Exiting);
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "SetUnlockBtnState()");
                clsUIHelper.ShowErrorMsg(ex.Message);
            }
        }

        void resetUserPassword()
        {
            User objUser = new User();
            UserData dtUser = new UserData();
            string strNewPass = "";
            string userID = "";
            string sPassward = "";
            try
            {
                logger.Trace("resetUserPassword() - " + clsPOSDBConstants.Log_Entering);
                if (grdSearch.ActiveRow != null)
                {
                    userID = grdSearch.ActiveRow.Cells[clsPOSDBConstants.Users_Fld_UserID].Value.ToString();

                    if (Resources.Message.Display("Do you want to Reset Password For User # " + grdSearch.ActiveRow.Cells[0].Value.ToString() + "\nThis Will Reset Current Password To POS ?", "PrimePOS", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        dtUser = objUser.GetUserByUserID(userID);
                        string sOldPassword = "";
                        string strEncryptedPassword = dtUser.Tables[0].Rows[0]["Password"].ToString();
                        sOldPassword = EncryptString.Decrypt(strEncryptedPassword);
                        //string testp =EncryptString.Decrypt(sOldPassword);
                        strNewPass = "POS";
                        sPassward = EncryptString.Encrypt(strNewPass);
                        //Following if is added by shitaljiton 11 August 2011
                        if (strEncryptedPassword == sOldPassword)
                        {
                            SearchSvr oSearchSvr = new SearchSvr();
                            if (oSearchSvr.UpdateUserRow(sPassward, userID))
                            {
                                //DBUser.DeleteDBUser(userID); //PRIMEPOS-3185
                                //DBUser.CreateDBUser(userID.Replace("'", "''"), "POS");  //PRIMEPOS-3095 16-May-2022 JY Modified //PRIMEPOS-3185
                                string settingChanges = "Current User :" + Configuration.UserName + " Station ID :" + Configuration.StationID + Environment.NewLine;
                                Logs.Logger(" Password For User # " + userID + " Successfully Reset To POS." + Environment.NewLine + settingChanges);
                                clsUIHelper.ShowOKMsg("Password For User #" + userID + " Successfully Reset To POS.");
                            }
                            else
                            {
                                Resources.Message.Display("User " + userID + " is in use");
                            }
                        }
                        else
                        {
                            UserSvr usrSvr = new UserSvr();
                            if (usrSvr.ChangeUserPassword(userID, sOldPassword, strNewPass))
                            {
                                clsUIHelper.ShowOKMsg("Password For User #" + userID + " Successfully Reset To POS.");
                                string settingChanges = "Current User :" + Configuration.UserName + " Station ID :" + Configuration.StationID + Environment.NewLine;
                                Logs.Logger(" Password For User # " + userID + " Successfully Reset To POS." + Environment.NewLine + settingChanges);
                                btnSearch_Click(null, null);
                            }
                            else
                            {
                                Resources.Message.Display("User " + userID + " is in use");
                            }
                        }
                    }
                }
                logger.Trace("resetUserPassword() - " + clsPOSDBConstants.Log_Exiting);
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "resetUserPassword()");
                SearchSvr oSearchSvr = new SearchSvr();
                if (oSearchSvr.UpdateUserRow(sPassward, userID))
                {
                    string settingChanges = "Current User :" + Configuration.UserName + " Station ID :" + Configuration.StationID + Environment.NewLine;
                    Logs.Logger(" Password For User # " + userID + " Successfully Reset To POS." + Environment.NewLine + settingChanges);
                    //DBUser.DeleteDBUser(userID); //PRIMEPOS-3185
                    //DBUser.CreateDBUser(userID.Replace("'", "''"), "POS"); //PRIMEPOS-3095 16-May-2022 JY Modified //PRIMEPOS-3185
                    clsUIHelper.ShowOKMsg("Password For User #" + userID + " Successfully Reset To POS.");
                }
                else
                {
                    userID = grdSearch.ActiveRow.Cells[clsPOSDBConstants.Users_Fld_UserID].Value.ToString();
                    Resources.Message.Display("User " + userID + " is in use");
                }
            }
        }

        #region frmSearchCustomer screen code
        private void SearchCustomer()
        {
            try
            {
                logger.Trace("SearchCustomer() - " + clsPOSDBConstants.Log_Entering);
                oDataSet = oBLSearch.GetCustomerSearchResult(Convert.ToDateTime(dtpDateOfBirth1.Value.ToString()), Convert.ToDateTime(dtpDateOfBirth2.Value.ToString()), Convert.ToDateTime(dtpExpDate1.Value.ToString()), Convert.ToDateTime(dtpExpDate2.Value.ToString()), this.txtCode.Text.Trim().Replace("'", "''"), this.txtName.Text.Trim().Replace("'", "''"), this.txtMasterSearchVal.Text.Trim().Replace("'", "''"), this.chkIncludeRXCust.Checked, out oCustomerData, this.txtContactNumber.Text.Trim().Replace("'", "''"), includeCPLCardInfo, ActiveOnly, bOnlyCLPCardCustomers, bSearchExactCustomer, bSelection, cboExpDate.SelectedItem.DataValue.ToString(), cboDOB.SelectedItem.DataValue.ToString(), chkNoStoreCard.Checked);  //PRIMEPOS-2613 07-Dec-2018 JY Added new parameters    //PRIMEPOS-2645 05-Mar-2019 JY Added DOB parameters//PRIMEPOS-2896 Added chkNoStoreCard.Checked
                //PRIMEPOS-2896
                if (chkNoStoreCard.Checked)
                {
                    pnlCustTokenize.Visible = true;
                }
                else
                    pnlCustTokenize.Visible = false;
                grdSearch.DataSource = oDataSet;
                grdSearch.DisplayLayout.Bands[0].Columns["CustomerID"].Hidden = true;
                grdSearch.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.Customer_Fld_CustomerCode].Hidden = true;
                grdSearch.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.Customer_Fld_Name].Header.VisiblePosition = 1; //PRIMEPOS-3556
                grdSearch.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.Customer_Fld_DOB].Header.VisiblePosition = 2; //PRIMEPOS-3556
                //grdSearch.DisplayLayout.Override.ColumnAutoSizeMode = ColumnAutoSizeMode.None;

                //this.grdSearch.DisplayLayout.Bands[0].Columns["Name"].Width = 150;
                //this.grdSearch.DisplayLayout.Bands[0].Columns["Address1"].Width = 150;
                if (grdSearch.Rows.Count > 0)
                {
                    this.grdSearch.ActiveRow = this.grdSearch.Rows[0];
                    grdSearch.Focus();
                }
                else
                {
                    txtMasterSearchVal.Focus();
                }
                resizeColumns();
                grdSearch.PerformAction(UltraGridAction.FirstRowInGrid);
                grdSearch.Refresh();
                bSearchExactCustomer = false;
                logger.Trace("SearchCustomer() - " + clsPOSDBConstants.Log_Exiting);
            }
            catch (Exception Ex)
            {
                logger.Fatal(Ex, "SearchCustomer()");
                clsUIHelper.ShowErrorMsg(Ex.Message);
            }
        }

        public string SelectedAcctNo()
        {
            logger.Trace("SelectedAcctNo() - " + clsPOSDBConstants.Log_Entering);
            if (grdSearch.ActiveRow != null)
                if (grdSearch.ActiveRow.Cells.Count > 0)
                {
                    logger.Trace("SelectedAcctNo() - " + clsPOSDBConstants.Log_Exiting);
                    return grdSearch.ActiveRow.Cells["Account#"].Text;
                }
                else
                {
                    logger.Trace("SelectedAcctNo() - " + clsPOSDBConstants.Log_Exiting);
                    return "";
                }
            else
            {
                logger.Trace("SelectedAcctNo() - " + clsPOSDBConstants.Log_Exiting);
                return "";
            }
        }

        public string SelectedCLPCardID()
        {
            logger.Trace("SelectedCLPCardID() - " + clsPOSDBConstants.Log_Entering);
            if (grdSearch.ActiveRow != null)
                if (grdSearch.ActiveRow.Cells.Count > 0)
                {
                    logger.Trace("SelectedCLPCardID() - " + clsPOSDBConstants.Log_Exiting);
                    return grdSearch.ActiveRow.Cells["CLP Card ID"].Text;
                }
                else
                {
                    logger.Trace("SelectedCLPCardID() - " + clsPOSDBConstants.Log_Exiting);
                    return "";
                }
            else
            {
                logger.Trace("SelectedCLPCardID() - " + clsPOSDBConstants.Log_Exiting);
                return "";
            }
        }
        #endregion

        #region frmSearch screen code
        private void GetFrmSearchData()
        {
            try
            {
                logger.Trace("GetFrmSearchData() - " + clsPOSDBConstants.Log_Entering);
                this.grdSearch.DisplayLayout.Bands[0].SortedColumns.Clear();    //19-Jun-2015 JY Added 
                this.Cursor = Cursors.WaitCursor;
                if (DefaultCode.Trim() == "")
                    DefaultCode = this.txtCode.Text.Trim();

                if (SearchTable == clsPOSDBConstants.Item_tbl)
                    IsAdvSearchDone = false;

                oDataSet = oBLSearch.GetFrmSearchData(SearchTable, this.txtCode.Text.Trim().Replace("'", "''"), this.txtName.Text.Trim().Replace("'", "''"), this.txtMasterSearchVal.Text.Trim().Replace("'", "''"), PrgFlag, ParamValue, IsFromPurchaseOrder, ref DefaultCode, ActiveOnly, AdditionalParameter);
                grdSearch.DataSource = oDataSet;

                FrmSearchDataGridFormatting();
                logger.Trace("GetFrmSearchData() - " + clsPOSDBConstants.Log_Exiting);
            }
            catch (Exception Ex)
            {
                logger.Fatal(Ex, "GetFrmSearchData()");
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        #region formating grid
        private void FrmSearchDataGridFormatting()
        {
            logger.Trace("FrmSearchDataGridFormatting() - " + clsPOSDBConstants.Log_Entering);
            if (SearchTable == clsPOSDBConstants.PrimeRX_HouseChargeInterface)
            {
                try
                {
                    grdSearch.DisplayLayout.Bands[0].Columns["ACCT_NO"].Header.Caption = "Account#";
                    grdSearch.DisplayLayout.Bands[0].Columns["ACCT_NAME"].Header.Caption = "Name";
                    grdSearch.DisplayLayout.Bands[0].Columns["ADDRESS1"].Header.Caption = "Address1";
                    grdSearch.DisplayLayout.Bands[0].Columns["ADDRESS2"].Header.Caption = "Address2";
                    grdSearch.DisplayLayout.Bands[0].Columns["CITY"].Header.Caption = "City";
                    grdSearch.DisplayLayout.Bands[0].Columns["STATE"].Header.Caption = "State";
                    grdSearch.DisplayLayout.Bands[0].Columns["ZIP"].Header.Caption = "Zip";
                    grdSearch.DisplayLayout.Bands[0].Columns["PHONE_NO"].Header.Caption = "Phone";
                    grdSearch.DisplayLayout.Bands[0].Columns["STATUS"].Header.Caption = "Status";
                    grdSearch.DisplayLayout.Bands[0].Columns["APPFINCHRG"].Header.Caption = "App Fin Charge";
                    grdSearch.DisplayLayout.Bands[0].Columns["RECURRINGB"].Header.Caption = "Reccuring GB";
                    grdSearch.DisplayLayout.Bands[0].Columns["CCTYPE"].Header.Caption = "CC Type";
                    grdSearch.DisplayLayout.Bands[0].Columns["CCNUMBER"].Header.Caption = "CC#";
                    grdSearch.DisplayLayout.Bands[0].Columns["CCEXPDATE"].Header.Caption = "CC Exp. Date";
                    grdSearch.DisplayLayout.Bands[0].Columns["CREDIT_LMT"].Header.Caption = "Credit Limit";
                    grdSearch.DisplayLayout.Bands[0].Columns["BALANCE"].Header.Caption = "Balance";
                    grdSearch.DisplayLayout.Bands[0].Columns["DISCOUNT"].Header.Caption = "Discount";
                    grdSearch.DisplayLayout.Bands[0].Columns["MTD_CHARGE"].Header.Caption = "MDT Charge";
                    grdSearch.DisplayLayout.Bands[0].Columns["YTD_CHARGE"].Header.Caption = "YDT Charge";
                    grdSearch.DisplayLayout.Bands[0].Columns["LY_CHARGE"].Header.Caption = "LY Charge";
                    grdSearch.DisplayLayout.Bands[0].Columns["MTD_PAYM"].Header.Caption = "MDT Payment";
                    grdSearch.DisplayLayout.Bands[0].Columns["YTD_PAYM"].Header.Caption = "YDT Payment";
                    grdSearch.DisplayLayout.Bands[0].Columns["LY_PAYM"].Header.Caption = "LY Payment";
                    grdSearch.DisplayLayout.Bands[0].Columns["LAST_SBAL"].Header.Caption = "Last Bal.";
                    grdSearch.DisplayLayout.Bands[0].Columns["LAST_SDATE"].Header.Caption = "Last Date";
                    grdSearch.DisplayLayout.Bands[0].Columns["COMMENT"].Header.Caption = "Comment";
                    grdSearch.DisplayLayout.Bands[0].Columns["MOBILENO"].Header.Caption = "Mobile";
                    grdSearch.DisplayLayout.Bands[0].Columns["NAMEONCC"].Header.Caption = "Name On CC";
                    grdSearch.DisplayLayout.Bands[0].Columns["IE"].Header.Caption = "IE";
                }
                catch (Exception Ex)
                {
                    logger.Fatal(Ex, "FrmSearchDataGridFormatting()");
                }
            }
            else if (SearchTable == clsPOSDBConstants.Department_tbl)
            {
                if (grdSearch.DisplayLayout.Bands[0].Columns.Contains("id"))    //Sprint-24 - PRIMEPOS-2273 27-Oct-2016 JY Added to fix one exception which occur when we provide wrong dept code in search criteria
                    this.grdSearch.DisplayLayout.Bands[0].Columns["id"].Hidden = true;
                if (grdSearch.DisplayLayout.Bands[0].Columns.Contains("Name"))
                    this.grdSearch.DisplayLayout.Bands[0].Columns["Name"].SortIndicator = SortIndicator.Ascending;  //Sprint-19 - 2146 24-Dec-2014 JY Added to sort grid departmentwise by default 
            }
            #region Sprint-19 - 2146 24-Dec-2014 JY Added to sort grid sub-departmentwise when it populated first time
            else if (SearchTable == clsPOSDBConstants.SubDepartment_tbl)
            {
                this.grdSearch.DisplayLayout.Bands[0].Columns[1].SortIndicator = SortIndicator.Ascending;
            }
            #endregion

            if (this.AllowMultiRowSelect == true)
            {

                if (this.grdSearch.DisplayLayout.Bands[0].Columns.Exists("CHECK") == false)
                {
                    this.grdSearch.DisplayLayout.Bands[0].Columns.Add("CHECK");
                    this.grdSearch.DisplayLayout.Bands[0].Columns["CHECK"].Header.Caption = "";

                    this.grdSearch.DisplayLayout.Bands[0].Columns["CHECK"].DataType = typeof(bool);
                    this.grdSearch.DisplayLayout.Bands[0].Columns["CHECK"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.CheckBox;
                }
                this.grdSearch.DisplayLayout.Bands[0].Columns["Check"].Header.VisiblePosition = 0;
            }

            if (IsFromPurchaseOrder == true)
            {
                grdSearch.DisplayLayout.Bands[0].Columns["SellingPrice"].Hidden = true;
                grdSearch.DisplayLayout.Bands[0].Columns["VendorID"].Hidden = true;
                //Start : Added By Amit Date 17 June 2011
                grdSearch.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.Item_Fld_QtyInStock].Hidden = true;
                grdSearch.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.Item_Fld_QtyOnOrder].Hidden = true;
                grdSearch.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.Item_Fld_ReOrderLevel].Hidden = true;
                grdSearch.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.Item_Fld_MinOrdQty].Hidden = true;
                //End
                foreach (UltraGridRow row in grdSearch.Rows)
                {
                    if (row.Cells[clsPOSDBConstants.ItemVendor_Fld_VendorCostPrice].Value.ToString() != row.Cells["BestPrice"].Value.ToString())
                    {
                        grdSearch.DisplayLayout.Bands[0].Columns["BestPrice"].CellAppearance.ForeColor = Color.Red;
                        grdSearch.DisplayLayout.Bands[0].Columns["BestVendor"].CellAppearance.ForeColor = Color.Red;
                    }
                }
            }
            if (grdSearch.DisplayLayout.Bands[0].Columns.Contains("id"))
            {
                grdSearch.DisplayLayout.Bands[0].Columns["id"].Hidden = true;
            }
            //this.grdSearch.DisplayLayout.Bands[0].Columns["CHECK"].Width = 50;
            grdSearch.PerformAction(UltraGridAction.FirstRowInGrid);
            resizeColumns();
            grdSearch.Refresh();
            logger.Trace("FrmSearchDataGridFormatting() - " + clsPOSDBConstants.Log_Exiting);
        }
        #endregion

        /// <summary>
        ///      Contains variable "SelectedRowCode" to Get Code of Selected row 
        /// </summary>
        /// <returns></returns>
        public string SelectedRowID()
        {
            logger.Trace("SelectedRowID() - " + clsPOSDBConstants.Log_Entering);
            if (!fromBestVendorPrice && grdSearch.ActiveRow != null && grdSearch.ActiveRow.Cells.Count > 0)
            {
                //if-else is added by shitaljit to resolve the issues with vender search filter as vendor code is in cell [0] not in cell [1]
                if (this.SearchTable == clsPOSDBConstants.Vendor_tbl)
                {
                    SelectedRowCode = grdSearch.ActiveRow.Cells[0].Value.ToString();
                    VendorID = grdSearch.ActiveRow.Cells[clsPOSDBConstants.Vendor_Fld_VendorId].Value.ToString();
                }
                else
                {
                    SelectedRowCode = grdSearch.ActiveRow.Cells[1].Value.ToString();//this line added by Krishna on 3 May 2011
                }
                logger.Trace("SelectedRowID() - " + clsPOSDBConstants.Log_Exiting);
                return grdSearch.ActiveRow.Cells[0].Text;
            }
            else
            {
                logger.Trace("SelectedRowID() - " + clsPOSDBConstants.Log_Exiting);
                return strItemID;
            }
        }

        #region PRIMEPOS-2671 22-Apr-2019 JY Added
        public ItemM SelectedItemMRow()
        {
            logger.Trace("SelectedRow() - " + clsPOSDBConstants.Log_Entering);
            ItemM oItemM = new ItemM();
            if (grdSearch.ActiveRow != null && grdSearch.ActiveRow.Cells.Count > 0)
            {
                oItemM.ItemID = grdSearch.ActiveRow.Cells["ItemID"].Value.ToString();
                oItemM.Description = grdSearch.ActiveRow.Cells["Description"].Value.ToString();
                oItemM.ProductCode = grdSearch.ActiveRow.Cells["ProductCode"].Value.ToString();
                oItemM.SeasonCode = grdSearch.ActiveRow.Cells["SeasonCode"].Value.ToString();
                oItemM.Unit = grdSearch.ActiveRow.Cells["Unit"].Value.ToString();
                oItemM.Freight = Configuration.convertNullToDecimal(grdSearch.ActiveRow.Cells["Freight"].Value.ToString());
                oItemM.SellingPrice = Configuration.convertNullToDecimal(grdSearch.ActiveRow.Cells["SellingPrice"].Value.ToString());
                oItemM.Itemtype = grdSearch.ActiveRow.Cells["Itemtype"].Value.ToString();
                oItemM.isTaxable = Configuration.convertNullToBoolean(grdSearch.ActiveRow.Cells["isTaxable"].Value.ToString());
                oItemM.PCKSIZE = grdSearch.ActiveRow.Cells["PCKSIZE"].Value.ToString();
                oItemM.PCKQTY = grdSearch.ActiveRow.Cells["PCKQTY"].Value.ToString();
                oItemM.PCKUNIT = grdSearch.ActiveRow.Cells["PCKUNIT"].Value.ToString();
                oItemM.ItemStatus = grdSearch.ActiveRow.Cells["ItemStatus"].Value.ToString();
                oItemM.ManufacturerName = grdSearch.ActiveRow.Cells["ManufacturerName"].Value.ToString();
                oItemM.IsEBTItem = Configuration.convertNullToBoolean(grdSearch.ActiveRow.Cells["IsEBTItem"].Value.ToString());
            }
            else
            {
                oItemM = null;
            }
            logger.Trace("SelectedRow() - " + clsPOSDBConstants.Log_Exiting);
            return oItemM;
        }
        #endregion

        public string SelectedVendorItemCode(String columnName)
        {
            logger.Trace("SelectedVendorItemCode() - " + clsPOSDBConstants.Log_Entering);
            if (grdSearch.ActiveRow != null && grdSearch.ActiveRow.Cells.Count > 0)
            {
                if (grdSearch.ActiveRow.Cells.Exists(columnName))
                {
                    logger.Trace("SelectedVendorItemCode() - " + clsPOSDBConstants.Log_Exiting);
                    return grdSearch.ActiveRow.Cells[columnName].Text;
                }
                else
                {
                    logger.Trace("SelectedVendorItemCode() - " + clsPOSDBConstants.Log_Exiting);
                    return "";
                }
            }
            else
            {
                logger.Trace("SelectedVendorItemCode() - " + clsPOSDBConstants.Log_Exiting);
                return "";
            }
        }

        public string SelectedCode()
        {
            logger.Trace("SelectedCode() - " + clsPOSDBConstants.Log_Entering);

            if (!fromBestVendorPrice && grdSearch.ActiveRow != null && grdSearch.ActiveRow.Cells.Count > 0)
            {
                //Updated By SRT(Ritesh Parekh) Date : 22-Jul-2009             
                if (SearchTable == "VendorItemCodeWise")
                {
                    logger.Trace("SelectedCode() - " + clsPOSDBConstants.Log_Exiting);
                    return grdSearch.ActiveRow.Cells["VendorItemId"].Text;
                }
                else if (SearchTable == "ItemID")
                {
                    logger.Trace("SelectedCode() - " + clsPOSDBConstants.Log_Exiting);
                    return grdSearch.ActiveRow.Cells["VendorItemId"].Text;
                }
                else
                {
                    logger.Trace("SelectedCode() - " + clsPOSDBConstants.Log_Exiting);
                    return grdSearch.ActiveRow.Cells["Code"].Text;
                }
            }
            else
            {
                logger.Trace("SelectedCode() - " + clsPOSDBConstants.Log_Exiting);
                return strVendorItemID;
            }
        }

        public string SelectedVendorID()
        {
            logger.Trace("SelectedVendorID() - " + clsPOSDBConstants.Log_Entering);
            if (grdSearch.ActiveRow != null && grdSearch.ActiveRow.Cells.Count > 0)
            {
                logger.Trace("SelectedVendorID() - " + clsPOSDBConstants.Log_Exiting);
                return grdSearch.ActiveRow.Cells["VendorId"].Text;
            }
            else
            {
                logger.Trace("SelectedVendorID() - " + clsPOSDBConstants.Log_Exiting);
                return "";
            }
        }

        public string SelectedBestVendor()
        {
            logger.Trace("SelectedBestVendor() - " + clsPOSDBConstants.Log_Entering);
            if (!fromBestVendorPrice && grdSearch.ActiveRow != null && grdSearch.ActiveRow.Cells.Count > 0)
            {
                logger.Trace("SelectedBestVendor() - " + clsPOSDBConstants.Log_Exiting);
                return grdSearch.ActiveRow.Cells["BestVendor"].Text;
            }
            else
            {
                logger.Trace("SelectedBestVendor() - " + clsPOSDBConstants.Log_Exiting);
                return "";
            }
        }

        public string SelectedVendorCostPrice()
        {
            logger.Trace("SelectedVendorCostPrice() - " + clsPOSDBConstants.Log_Entering);

            if (grdSearch.ActiveRow != null && grdSearch.ActiveRow.Cells.Count > 0)
            {
                logger.Trace("SelectedVendorCostPrice() - " + clsPOSDBConstants.Log_Exiting);
                return grdSearch.ActiveRow.Cells["VendorCostPrice"].Text;
            }
            else
            {
                logger.Trace("SelectedVendorCostPrice() - " + clsPOSDBConstants.Log_Exiting);
                return "";
            }
        }

        public string SelectedBestVendorPrice()
        {
            logger.Trace("SelectedBestVendorPrice() - " + clsPOSDBConstants.Log_Entering);
            if (!fromBestVendorPrice && grdSearch.ActiveRow != null && grdSearch.ActiveRow.Cells.Count > 0)
            {
                logger.Trace("SelectedBestVendorPrice() - " + clsPOSDBConstants.Log_Exiting);
                return grdSearch.ActiveRow.Cells["BestPrice"].Text;
            }
            else
            {
                logger.Trace("SelectedBestVendorPrice() - " + clsPOSDBConstants.Log_Exiting);
                return strVendorCostPrice;
            }
        }

        private void CheckUncheckGridRow(UltraGridCell oCell)
        {
            logger.Trace("CheckUncheckGridRow() - " + clsPOSDBConstants.Log_Entering);
            if ((bool)oCell.Value == false)
            {
                oCell.Value = true;
            }
            else
            {
                oCell.Value = false;
            }
            oCell.Row.Update();
            logger.Trace("CheckUncheckGridRow() - " + clsPOSDBConstants.Log_Exiting);
        }
        #endregion

        #endregion

        #region PRIMEPOS-2475 07-Jun-2018 JY Added
        private void btnClear_Click(object sender, EventArgs e)
        {
            txtCode.ResetText();
            txtName.ResetText();
            txtMasterSearchVal.ResetText();
            txtContactNumber.ResetText();
            chkIncludeRXCust.Checked = false;
        }
        #endregion

        #region PRIMEPOS-2613 05-Dec-2018 JY Added
        private void cboExpDate_ValueChanged(object sender, EventArgs e)
        {
            if (this.cboExpDate.Visible == false) return;

            if (this.cboExpDate.SelectedItem.DataValue.ToString() == "All" || this.cboExpDate.SelectedItem.DataValue.ToString() == "NULL" || this.cboExpDate.SelectedItem.DataValue.ToString() == "NOT NULL")
            {
                this.dtpExpDate1.Visible = false;
                this.dtpExpDate2.Visible = false;
            }
            else if (this.cboExpDate.SelectedItem.DataValue.ToString() == "=" || this.cboExpDate.SelectedItem.DataValue.ToString() == ">" || this.cboExpDate.SelectedItem.DataValue.ToString() == "<")
            {
                this.dtpExpDate1.Visible = true;
                this.dtpExpDate2.Visible = false;
            }
            else if (this.cboExpDate.SelectedItem.DataValue.ToString() == "Between")
            {
                this.dtpExpDate1.Visible = true;
                this.dtpExpDate2.Visible = true;
            }
        }
        #endregion

        #region PRIMEPOS-2645 06-Mar-2019 JY Added
        private void cboDOB_ValueChanged(object sender, EventArgs e)
        {
            if (this.cboDOB.Visible == false) return;

            if (this.cboDOB.SelectedItem.DataValue.ToString() == "All" || this.cboDOB.SelectedItem.DataValue.ToString() == "NULL" || this.cboDOB.SelectedItem.DataValue.ToString() == "NOT NULL")
            {
                this.dtpDateOfBirth1.Visible = false;
                this.dtpDateOfBirth2.Visible = false;
            }
            else if (this.cboDOB.SelectedItem.DataValue.ToString() == "=" || this.cboDOB.SelectedItem.DataValue.ToString() == ">" || this.cboDOB.SelectedItem.DataValue.ToString() == "<")
            {
                this.dtpDateOfBirth1.Visible = true;
                this.dtpDateOfBirth2.Visible = false;
            }
            else if (this.cboDOB.SelectedItem.DataValue.ToString() == "Between")
            {
                this.dtpDateOfBirth1.Visible = true;
                this.dtpDateOfBirth2.Visible = true;
            }
        }
        #endregion

        #region PRIMEPOS-2671 18-Apr-2019 JY Added
        private void GetMMSSearchData()
        {
            try
            {
                logger.Trace("GetMMSSearchData() - " + clsPOSDBConstants.Log_Entering);

                this.grdSearch.DisplayLayout.Bands[0].SortedColumns.Clear();
                this.Cursor = Cursors.WaitCursor;
                if (DefaultCode.Trim() == "")
                    DefaultCode = this.txtCode.Text.Trim();

                if (sPSServiceAddress.Length == 0)
                {
                    Resources.Message.Display("please Set Service Address in System Settings " + Environment.NewLine + " and try again", "PrimePOS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                List<String> Criteria = new List<string>();

                string sItemId = txtCode.Text.Trim();
                string sItemDesc = txtName.Text.Trim();

                if (sItemId.Length > 0)
                {
                    Criteria.Add("ItemId" + "|" + sItemId + "%");
                }
                if (sItemDesc.Length > 0)
                {
                    Criteria.Add("Description" + "|" + sItemDesc + "%");
                }

                if (Criteria.Count <= 0)
                {
                    Resources.Message.Display("Please provide search criteria and try again", "PrimePOS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    if (oDataSet != null)
                        oDataSet.Clear();
                    return;
                }
                objService.Url = sPSServiceAddress.Trim() + @"Prime.SearchService.asmx";
                string errMsg = objService.SearchItem(Criteria.ToArray(), sNPINo, out oDataSet);

                if (errMsg.Trim().ToUpper() == "invalid npi".ToUpper())
                {
                    Resources.Message.Display("invalid npi", "PrimePOS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                else
                {
                    grdSearch.DataSource = oDataSet;
                    FrmSearchDataGridFormatting();
                }
                logger.Trace("GetMMSSearchData() - " + clsPOSDBConstants.Log_Exiting);
            }
            catch (Exception Ex)
            {
                logger.Fatal(Ex, "GetMMSSearchData()");
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }
        #endregion

        #region PRIMEPOS-2779 17-Jan-2020 JY Added
        private void btnExport_Click(object sender, EventArgs e)
        {
            try
            {
                if (oDataSet != null && oDataSet.Tables.Count > 0 && oDataSet.Tables[0].Rows.Count > 0)
                {
                    Configuration.ExportData(oDataSet.Tables[0], "ItemFile");
                }
                else
                {
                    Resources.Message.Display("no record(s) found to export", "PrimePOS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception Ex)
            {
                //logger.Fatal(Ex, "btnExport_Click");
                clsUIHelper.ShowErrorMsg(Ex.Message);
            }
        }
        #endregion

        private void btnCustTokenize_Click(object sender, EventArgs e)
        {
            if (grdSearch.Rows.Count <= 0) return;

            frmCustomers oCustomer = new frmCustomers();
            oCustomer.IsTokenize = true;
            oCustomer.Edit(grdSearch.ActiveRow.Cells[clsPOSDBConstants.CLCards_Fld_CustomerID].Text);
            oCustomer.ShowDialog(this);
        }

        #region PRIMEPOS-3116 11-Jul-2022 JY Added
        private void AddTransactionFee()
        {
            try
            {
                logger.Trace("AddTransactionFee() - " + clsPOSDBConstants.Log_Entering);

                frmTransFee ofrmTransFee = new frmTransFee();
                ofrmTransFee.SetNew();
                ofrmTransFee.ShowDialog();
                if (ofrmTransFee.IsCanceled == false)
                {
                    btnSearch_Click(null, null);
                }
                logger.Trace("AddTransactionFee() - " + clsPOSDBConstants.Log_Exiting);
            }
            catch (Exception exp)
            {
                logger.Fatal(exp, "AddTransactionFee()");
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }

        private void EditTransactionFee()
        {
            logger.Trace("EditTransactionFee() - " + clsPOSDBConstants.Log_Entering);
            frmTransFee ofrmTransFee = new frmTransFee();
            ofrmTransFee.SetNew();
            ofrmTransFee.Text = "Transaction Fee";
            ofrmTransFee.lblTransactionType.Text = "Edit Transaction Fee";
            ofrmTransFee.Edit(Convert.ToInt32(grdSearch.ActiveRow.Cells[0].Value.ToString()));
            ofrmTransFee.ShowDialog(this);
            if (!ofrmTransFee.IsCanceled)
            {
                btnSearch_Click(null, null);
            }
            logger.Trace("EditTransactionFee() - " + clsPOSDBConstants.Log_Exiting);
        }

        private void DeleteTransFee()
        {
            TransFee oTransFee = new TransFee();
            if (Configuration.convertNullToBoolean(grdSearch.ActiveRow.Cells["IsActive"].Value) == true)
            {
                bool bDelete = oTransFee.Delete(Convert.ToInt32(grdSearch.ActiveRow.Cells[0].Value.ToString()));
                if (bDelete)
                    btnSearch_Click(null, null);
            }
            else
            {
                Resources.Message.Display("This action turn off IsActive flag which is already false.", "PrimePOS", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        #endregion

        #region PRIMEPOS-3167 07-Nov-2022 JY Added
        private void InitializeOrderTimer()
        {
            if (POS_Core.Resources.Configuration.CPrimeEDISetting.PurchaseOrderTimer == 0)
            {
                POS_Core.Resources.Configuration.CPrimeEDISetting.PurchaseOrderTimer = 1;
            }
            orderUpdateTimer.Interval = POS_Core.Resources.Configuration.CPrimeEDISetting.PurchaseOrderTimer * 1000 * 60; //Check where this can be put in the POSSetUtil
            orderUpdateTimer.Enabled = true;
        }        

        private void orderUpdateTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                CancellationToken _cancelToken;
                Task.Factory.StartNew(() => PrimePOUtil.DefaultInstance.OnUpdatePendingOrderStatusEvent(null, null), _cancelToken, TaskCreationOptions.LongRunning, TaskScheduler.Default);
            }
            catch (Exception Ex)
            {
                Logs.Logger("orderUpdateTimer_Tick(object sender, EventArgs e)) - " + Ex);
            }
        }
        #endregion
    }
}
