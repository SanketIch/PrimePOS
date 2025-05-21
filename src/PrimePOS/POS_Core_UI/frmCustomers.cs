using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NLog;
using POS_Core.CommonData.Rows;
using POS_Core.BusinessRules;
using System.Collections.ObjectModel;
using POS_Core.Resources;
using POS_Core.CommonData;
using System.Text.RegularExpressions;
using POS_Core.Business_Tier;
using MMS.Device;
using POS_Core.ErrorLogging;
using MMSChargeAccount;
using MMS.Device.Global;
using PharmData;
using Infragistics.Win.UltraWinGrid;
using POS_Core_UI.Layout;
using POS_Core.Resources.PaymentHandler;
using POS_Core.LabelHandler.RxLabel;
using POS_Core_UI.Reports.ReportsUI;
using POS_Core.BusinessTier;
using PrimeRxPay;

namespace POS_Core_UI
{
    public partial class frmCustomers : frmMasterLayout
    {
        #region Decalaration
        private ILogger logger = LogManager.GetCurrentClassLogger();

        Regex CheckvalidEmailid = new Regex(@"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z");
        public bool IsCanceled = true;
        private CustomerRow oCustomerRow;
        private Customer oBRCustomer = new Customer();
        private CustomerData oCustomerData = new CustomerData();
        private DL ScanID = null; //Added by Manoj 9/11/2013
        private delegate void ScanEventHandler(); //Added by Manoj 9/11/2013
        private event ScanEventHandler ScanData; //Added by Manoj 9/11/2013
        private event ScanEventHandler AfterScanData; //Added by Manoj 9/11/2013
        private string Data = string.Empty; //Added by Manoj 9/11/2013
        private bool isScan; //Added by Manoj 9/11/2013
        DataTable dtLanguage = new DataTable();//Added By Shitaljit 10/10/2013
        bool isEdit = false;//Added By Shitaljit 10/10/2013
        private string sSigPadTransID = ""; //unique id for sigpad
        private string sSigPadTransIDLast = ""; //unique id for sigpad previously
        CCCustomerTokInfoData oCCCustomerTokInfoData = new CCCustomerTokInfoData();

        #region PRIMEPOS-2655 - NileshJ - 06-Spet-2019
        private DataSet dsCustomerInfo = new DataSet();
        CustomerPerformance custperfom = new CustomerPerformance();
        private DataTable dtTransaction = new DataTable();
        private DataTable dtItemDetails = new DataTable();
        private DataTable dtDepartmentDetails = new DataTable();
        private DataTable dtGraph = new DataTable();
        #endregion

        #region Added to bind combo boxes
        private ObservableCollection<ComboItem> _GenderList;
        public ObservableCollection<ComboItem> GenderList
        {
            get { return _GenderList; }
            set { SetField(ref _GenderList, value, "GenderList"); }
        }

        private ObservableCollection<ComboItem> _PrimaryContactList;
        public ObservableCollection<ComboItem> PrimaryContactList
        {
            get { return _PrimaryContactList; }
            set { SetField(ref _PrimaryContactList, value, "PrimaryContactList"); }
        }

        private ObservableCollection<ComboItem> _LanguageList;
        public ObservableCollection<ComboItem> LanguageList
        {
            get { return _LanguageList; }
            set { SetField(ref _LanguageList, value, "LanguageList"); }
        }
        #endregion               

        public CustomerData CustmerData
        {
            get
            {
                return oCustomerData;
            }
        }
        #region PRIMEPOS-2747
        StoreCreditDetails oStoreCreditDetails = new StoreCreditDetails();
        DataSet dsStoreDetails = new DataSet();
        DataTable dtStoreDetails = new DataTable();
        StoreCredit oStoreCredit = new StoreCredit();
        DataSet dsStoreData = new DataSet();
        DataTable dtStoreData = new DataTable();

        #endregion

        #region PRIMEPOS-2896 Arvind
        public bool IsTokenize = false;
        #endregion

        #endregion

        #region constructor
        public frmCustomers()
        {
            logger.Trace("frmCustomers() - " + clsPOSDBConstants.Log_Entering);
            InitializeComponent();
            try
            {
                setChildControlProperties(this);
                clsUIHelper.SetHeader(this, this.Name);
                Initialize();
                PopulateGender();
                PopulatePrimaryContact();
                PopulateLanguage();
                logger.Trace("frmCustomers() - " + clsPOSDBConstants.Log_Exiting);
            }
            catch (Exception Ex)
            {
                logger.Fatal(Ex, "frmCustomers()");
                clsUIHelper.ShowErrorMsg(Ex.Message);
            }
        }
        #endregion

        public void Initialize()
        {
            logger.Trace("Initialize() - " + clsPOSDBConstants.Log_Entering);
            oBRCustomer = new Customer();
            oCustomerData = new CustomerData();

            Clear();

            bool bSaveCCToken = false;  //Sprint-25 - PRIMEPOS-2373 16-Feb-2017 JY Added
            if (Configuration.CInfo.SaveCCToken == true && Configuration.CInfo.DefaultCustomerTokenValue == true) bSaveCCToken = true;    //Sprint-25 - PRIMEPOS-2373 16-Feb-2017 JY Added

            oCustomerRow = oCustomerData.Customer.AddRow(0, "", "", "", "", "", "", "", "", "", "", "", true, 0, 0, Configuration.MinimumDate, "", 0, true, 0, 0, bSaveCCToken);    //Sprint-23 - PRIMEPOS-2314 10-Jun-2016 JY Added SaveCardProfile parameter
            if (bSaveCCToken)
                oCustomerRow.SaveCardProfile = true;

            chkIsActive.Checked = true;
            chkUseForCustomerLoyalty.Checked = true;
            chkIsActive.Enabled = false;
            this.btnPrint.Visible = false;
            if (Configuration.CPOSSet.UsePrimeRX == true)
            {
                this.lblFromPharmacy.Visible = this.btnFromPharmacy.Visible = true;
            }
            if (Configuration.CPOSSet.UsePrimeRX == false)
            {
                txtHouseChargeID.Enabled = false;
                ClearHouseChargeCtrl();
            }

            tabCustomerInfo.Tabs[1].Visible = false;
            if (Configuration.CInfo.SaveCCToken == true && Configuration.CInfo.DefaultCustomerTokenValue == true)   //Sprint-25 - PRIMEPOS-2373 16-Feb-2017 JY Added if condition
                chkSaveCardProfile.Checked = true;
            else
                chkSaveCardProfile.Checked = false;  //Sprint-23 - PRIMEPOS-2314 09-Jun-2016 JY Added

            //Sprint-23 - PRIMEPOS-2315 21-Jun-2016 JY Added
            if (Configuration.CInfo.SaveCCToken == true)
                chkSaveCardProfile.Enabled = true;
            else
                chkSaveCardProfile.Enabled = false;

            #region  Nileshj - PRIMEPOS-2655
            dtFrom.Value = DateTime.Now.AddYears(-1);
            dtTo.Value = DateTime.Now;
            #endregion

            logger.Trace("Initialize() - " + clsPOSDBConstants.Log_Exiting);
        }

        #region Edit customer
        public void Edit(string CustomerCode, bool IsCreditCardTabSelected = false)//PRIMEPOS-2896 
        {
            try
            {
                logger.Trace("Edit() - " + clsPOSDBConstants.Log_Entering);
                if (CustomerCode.Equals("-1"))
                {
                    //oCustomerData = oBRCustomer.GetCustomerByAcctNumber(CustomerCode);
                    oCustomerData = oBRCustomer.Populate(CustomerCode);
                }
                else
                {
                    //oCustomerData = oBRCustomer.GetCustomerByCustomerID(Convert.ToInt32(CustomerCode));
                    oCustomerData = oBRCustomer.GetCustomerByID(Convert.ToInt32(CustomerCode));
                }
                if (Configuration.isNullOrEmptyDataSet(oCustomerData) == true)
                {
                    return;
                }
                else
                {
                    oCustomerRow = oCustomerData.Customer[0];

                    if (oCustomerRow != null)
                    {
                        Display();
                        if (oCustomerRow.AccountNumber == -1)
                        {
                            this.btnPrint.Visible = false;
                            #region PRIMEPOS-2858 19-Jun-2020 JY Added
                            tableLayoutPanel5.Enabled = false;
                            tableLayoutPanel6.Enabled = false;
                            tableLayoutPanel8.Enabled = false;
                            btnSave.Enabled = false;
                            #endregion
                        }
                        else
                        {
                            this.btnPrint.Visible = true;
                        }

                        //btnEmail.Visible = Configuration.CInfo.CardExpAlert;    //PRIMEPOS-2613 28-Dec-2018 JY Added
                        tabCustomerInfo.Tabs[1].Visible = true;
                        //PRIMEPOS-2896 
                        if (!IsCreditCardTabSelected)
                        {
                            tabCustomerInfo.ActiveTab = tabCustomerInfo.Tabs[0];
                            tabCustomerInfo.ActiveTab.Selected = true;
                            this.ActiveControl = txtCustomerCode;
                        }
                        else
                        {
                            tabCustomerInfo.ActiveTab = tabCustomerInfo.Tabs[2];
                            tabCustomerInfo.ActiveTab.Selected = true;
                            this.ActiveControl = txtCustomerCode;
                        }
                        isEdit = true;

                        txtCustomerName.Focus();
                    }
                }
                logger.Trace("Edit() - " + clsPOSDBConstants.Log_Exiting);
            }
            catch (Exception Ex)
            {
                logger.Fatal(Ex, "Edit()");
                clsUIHelper.ShowErrorMsg(Ex.ToString());
            }
        }

        private void Display()
        {
            logger.Trace("Display() - " + clsPOSDBConstants.Log_Entering);

            #region commented below code as we binded those controls
            //txtAccountNo.Text = oCustomerRow.AccountNumber.ToString();   //22Jul2009 Naim 
            //txtCustomerName.Text = oCustomerRow.CustomerName;
            //txtPhoneHome.Text = oCustomerRow.PhoneHome;
            //txtZipCode.Text = oCustomerRow.Zip;
            //txtAddress1.Text = oCustomerRow.Address1;
            //txtAddress2.Text = oCustomerRow.Address2;
            //txtCellNo.Text = oCustomerRow.CellNo;
            //txtCity.Text = oCustomerRow.City;
            //txtEmailAddr.Text = oCustomerRow.Email;
            //txtFaxNo.Text = oCustomerRow.FaxNo;
            //txtPhoneOff.Text = oCustomerRow.PhoneOffice;
            //txtState.Text = oCustomerRow.State;
            //chkIsActive.Checked = oCustomerRow.IsActive;            
            //chkUseForCustomerLoyalty.Checked = oCustomerRow.UseForCustomerLoyalty;
            //this.cboPrimaryContact.Value = oCustomerRow.PrimaryContact.ToString();
            //this.cboGender.Value = oCustomerRow.Gender.ToString();
            //this.dtpDateOfBirth.Value = oCustomerRow.DateOfBirth;
            //this.txtComments.Text = oCustomerRow.Comments;
            //txtCustomerFirstName.Text = oCustomerRow.FirstName;
            //txtCustomerCode.Text = oCustomerRow.CustomerCode;
            //txtDrivingLicNo.Text = oCustomerRow.DriveLicNo;
            //txtDrivingLicState.Text = oCustomerRow.DriveLicState;
            //this.txtDiscount.Value = oCustomerRow.Discount;//Added By Shitaljit 0n 17 Feb 2012
            //chkSaveCardProfile.Checked = oCustomerRow.SaveCardProfile;  //Sprint-23 - PRIMEPOS-2314 09-Jun-2016 JY Added
            #endregion

            chkIsActive.Enabled = true;
            if (Configuration.CPOSSet.UsePrimeRX == true)
            {
                if (oCustomerRow.HouseChargeAcctID > 0)
                {
                    //txtHouseChargeID.Tag = oCustomerRow.HouseChargeAcctID;
                    ShowHouseChargeName(oCustomerRow.HouseChargeAcctID.ToString());
                }
            }
            else
            {
                txtHouseChargeID.Enabled = false;
            }
            this.lblCustomerNotes.Visible = this.btnCustomerNotes.Visible = UserPriviliges.IsUserHasPriviliges(UserPriviliges.Modules.Notes.ID, UserPriviliges.Screens.CustomerNotes.ID); //Added by shitaljit(QuicSolv) on 11 October 2011
            this.txtAccountNo.Enabled = false;
            this.lblFromPharmacy.Visible = this.btnFromPharmacy.Visible = false;
            PopulateCustomerLoyaltyGrid(oCustomerRow.CustomerId);
            PopulateCreditCardProfiles(oCustomerRow.CustomerId);    //Sprint-23 - PRIMEPOS-2316 16-Jun-2016 JY Added
            logger.Trace("Display() - " + clsPOSDBConstants.Log_Exiting);
        }

        private void PopulateCustomerLoyaltyGrid(int customerID)
        {
            logger.Trace("PopulateCustomerLoyaltyGrid() - " + clsPOSDBConstants.Log_Entering);
            try
            {
                DataTable dt = oBRCustomer.PopulateCustomerLoyaltyGrid(customerID);
                grdCLCards.DataSource = dt;

                //grdCLCards.DisplayLayout.Bands[0].Columns["ID"].Hidden = true;
                //grdCLCards.DisplayLayout.Bands[0].Columns["Description"].Hidden = true;
                //grdCLCards.DisplayLayout.Bands[0].Columns["ExpiryDays"].Hidden = true;
                //grdCLCards.DisplayLayout.Bands[0].Columns["CustomerID"].Hidden = true;
                //grdCLCards.DisplayLayout.Bands[0].Columns["IsActive"].Hidden = true;

                //grdCLCards.DisplayLayout.Bands[0].Columns["CardID"].Header.SetVisiblePosition(0, false);                

                if (grdCLCards.Rows.Count > 0)
                {
                    this.grdCLCards.ActiveRow = this.grdCLCards.Rows[0];
                    grdCLCards.Focus();
                }
                TextAllignmentForGrid(grdCLCards);
                grdCLCards.DisplayLayout.Bands[0].Columns["CardID"].Header.Column.Width = 130;
                //grdCLCards.DisplayLayout.Bands[0].Columns[1].Header.Column.Width = 110;
                //grdCLCards.DisplayLayout.Bands[0].Columns[2].Header.Column.Width = 130;
                //grdCLCards.DisplayLayout.Bands[0].Columns[3].Header.Column.Width = 100;
                grdCLCards.PerformAction(Infragistics.Win.UltraWinGrid.UltraGridAction.FirstRowInGrid);
                grdCLCards.Refresh();
                logger.Trace("PopulateCustomerLoyaltyGrid() - " + clsPOSDBConstants.Log_Exiting);
            }
            catch(Exception Ex)
            {
                logger.Fatal(Ex, "PopulateCustomerLoyaltyGrid()");
            }
        }

        #region Sprint-23 - PRIMEPOS-2316 16-Jun-2016 JY Added to populate credit card profiles
        private void PopulateCreditCardProfiles(int customerID)
        {
            try
            {
                if (!oCustomerRow.SaveCardProfile)//PRIMEPOS-2896 Arvind
                {
                    btnAddCard.Visible = false;
                }
                logger.Trace("PopulateCreditCardProfiles() - " + clsPOSDBConstants.Log_Entering);

                oCCCustomerTokInfoData = oBRCustomer.GetTokenByCustomerID(customerID);
                grdCCProfile.DataSource = oCCCustomerTokInfoData.Tables[0];

                #region PRIMEPOS-2634 30-Jan-2019 JY Added
                clsUIHelper.SetKeyActionMappings(this.grdCCProfile);
                grdCCProfile.PerformAction(UltraGridAction.EnterEditMode);
                grdCCProfile.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.CCCustomerTokInfo__Fld_CardAlias].CellActivation = Activation.AllowEdit;
                grdCCProfile.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.CCCustomerTokInfo__Fld_CardAlias].MaxLength = 50;

                #region Preference                
                CCCustomerTokInfo oCCCustomerTokInfo = new CCCustomerTokInfo();
                UltraDropDown oUltraDropDown = new UltraDropDown();
                oUltraDropDown.SetDataBinding(oCCCustomerTokInfo.GetCardPreferences(), null);
                oUltraDropDown.ValueMember = "Id";
                oUltraDropDown.DisplayMember = "PreferenceDesc";
                oUltraDropDown.DisplayLayout.Bands[0].ColHeadersVisible = false;
                oUltraDropDown.DisplayLayout.Bands[0].Columns[0].Hidden = true;

                grdCCProfile.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.CCCustomerTokInfo__Fld_PreferenceId].ValueList = oUltraDropDown;
                grdCCProfile.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.CCCustomerTokInfo__Fld_PreferenceId].Width = 98;
                grdCCProfile.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.CCCustomerTokInfo__Fld_PreferenceId].Header.Caption = "Preference";
                grdCCProfile.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.CCCustomerTokInfo__Fld_PreferenceId].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
                grdCCProfile.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.CCCustomerTokInfo__Fld_PreferenceId].CellActivation = Activation.AllowEdit;
                #endregion

                grdCCProfile.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.CCCustomerTokInfo__Fld_CardType].CellActivation = Activation.NoEdit;
                grdCCProfile.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.CCCustomerTokInfo__Fld_Last4].CellActivation = Activation.NoEdit;
                grdCCProfile.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.CCCustomerTokInfo__Fld_ProfiedID].CellActivation = Activation.NoEdit;
                grdCCProfile.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.CCCustomerTokInfo__Fld_Processor].CellActivation = Activation.NoEdit;
                grdCCProfile.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.CCCustomerTokInfo__Fld_EntryType].CellActivation = Activation.NoEdit;
                grdCCProfile.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.CCCustomerTokInfo__Fld_TokenDate].CellActivation = Activation.NoEdit;
                grdCCProfile.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.CCCustomerTokInfo__Fld_ExpDate].CellActivation = Activation.NoEdit;
                #endregion

                #region 09-Oct-2020 JY Added to select row even if user clicked on the inactive/disabled cell
                grdCCProfile.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.CCCustomerTokInfo__Fld_CardType].CellClickAction = CellClickAction.RowSelect;
                grdCCProfile.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.CCCustomerTokInfo__Fld_Last4].CellClickAction = CellClickAction.RowSelect;
                grdCCProfile.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.CCCustomerTokInfo__Fld_ProfiedID].CellClickAction = CellClickAction.RowSelect;
                grdCCProfile.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.CCCustomerTokInfo__Fld_Processor].CellClickAction = CellClickAction.RowSelect;
                grdCCProfile.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.CCCustomerTokInfo__Fld_EntryType].CellClickAction = CellClickAction.RowSelect;
                grdCCProfile.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.CCCustomerTokInfo__Fld_TokenDate].CellClickAction = CellClickAction.RowSelect;
                grdCCProfile.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.CCCustomerTokInfo__Fld_ExpDate].CellClickAction = CellClickAction.RowSelect;
                #endregion

                grdCCProfile.DisplayLayout.Bands[0].Columns["EntryID"].Hidden = true;
                grdCCProfile.DisplayLayout.Bands[0].Columns["CustomerID"].Hidden = true;

                if (oCCCustomerTokInfoData.Tables[0].Columns.Contains("ExpDate"))
                    grdCCProfile.DisplayLayout.Bands[0].Columns["ExpDate"].Format = "MM/yyyy";

                if (grdCCProfile.Rows.Count > 0)
                {
                    this.grdCCProfile.ActiveRow = this.grdCCProfile.Rows[0];
                    #region PRIMEPOS-2687 31-Jan-2021 JY Added
                    try
                    {
                        for (int i = 0; i < grdCCProfile.Rows.Count; i++)
                        {
                            bool bExp = false;
                            if (grdCCProfile.Rows[i].Cells["ExpDate"].Value != null && grdCCProfile.Rows[i].Cells["ExpDate"].Value.ToString() != "")
                            {
                                if (Convert.ToDateTime(grdCCProfile.Rows[i].Cells["ExpDate"].Value).Year < DateTime.Now.Year ||
                                    (Convert.ToDateTime(grdCCProfile.Rows[i].Cells["ExpDate"].Value).Year == DateTime.Now.Year && Convert.ToDateTime(grdCCProfile.Rows[i].Cells["ExpDate"].Value).Month < DateTime.Now.Month))
                                {
                                    grdCCProfile.Rows[i].Appearance.BackColor = Color.Red;
                                    bExp = true;
                                }
                            }
                            if (!bExp)
                            {
                                grdCCProfile.Rows[i].Cells[clsPOSDBConstants.CCCustomerTokInfo__Fld_CardType].Appearance.BackColor = Color.LightGray;
                                grdCCProfile.Rows[i].Cells[clsPOSDBConstants.CCCustomerTokInfo__Fld_Last4].Appearance.BackColor = Color.LightGray;
                                grdCCProfile.Rows[i].Cells[clsPOSDBConstants.CCCustomerTokInfo__Fld_ProfiedID].Appearance.BackColor = Color.LightGray;
                                grdCCProfile.Rows[i].Cells[clsPOSDBConstants.CCCustomerTokInfo__Fld_Processor].Appearance.BackColor = Color.LightGray;
                                grdCCProfile.Rows[i].Cells[clsPOSDBConstants.CCCustomerTokInfo__Fld_EntryType].Appearance.BackColor = Color.LightGray;
                                grdCCProfile.Rows[i].Cells[clsPOSDBConstants.CCCustomerTokInfo__Fld_TokenDate].Appearance.BackColor = Color.LightGray;
                                grdCCProfile.Rows[i].Cells[clsPOSDBConstants.CCCustomerTokInfo__Fld_ExpDate].Appearance.BackColor = Color.LightGray;
                            }
                        }
                    }
                    catch (Exception Ex)
                    {
                        logger.Fatal(Ex, "PopulateCreditCardProfiles() - 0");
                    }
                    #endregion
                    grdCCProfile.Focus();
                }
                TextAllignmentForGrid(grdCCProfile);
                grdCCProfile.PerformAction(Infragistics.Win.UltraWinGrid.UltraGridAction.FirstRowInGrid);
                grdCCProfile.Refresh();
                logger.Trace("PopulateCreditCardProfiles() - " + clsPOSDBConstants.Log_Exiting);
            }
            catch (Exception Ex)
            {
                logger.Fatal(Ex, "PopulateCreditCardProfiles() - 1");
            }
        }
        #endregion

        #endregion

        private void Clear()
        {
            logger.Trace("Clear() - " + clsPOSDBConstants.Log_Entering);

            txtCustomerName.Text = "";
            txtPhoneHome.Text = "";
            txtZipCode.Text = "";
            txtAddress1.Text = "";
            txtAddress2.Text = "";
            txtCellNo.Text = "";
            txtCity.Text = "";
            txtEmailAddr.Text = "";
            txtFaxNo.Text = "";
            txtPhoneOff.Text = "";
            txtState.Text = "";
            txtComments.Text = "";
            txtCustomerCode.Text = "";
            txtCustomerFirstName.Text = "";
            txtDrivingLicNo.Text = "";
            txtDrivingLicState.Text = "";

            dtpDateOfBirth.Value = "";  //Sprint-25 - PRIMEPOS-433 02-Feb-2017 JY set to blank

            this.txtDiscount.Value = 0;//Added By Shitaljit 0n 17 Feb 2012

            if (oCustomerData != null) oCustomerData.Customer.Rows.Clear();

            chkIsActive.Checked = false;
            chkUseForCustomerLoyalty.Checked = true;
            chkSaveCardProfile.Checked = true;  //Sprint-23 - PRIMEPOS-2314 09-Jun-2016 JY Added

            txtAccountNo.Enabled = false;

            logger.Trace("Clear() - " + clsPOSDBConstants.Log_Exiting);
        }

        private void frmCustomers_Load(object sender, EventArgs e)
        {
            logger.Trace("frmCustomers_Load() - " + clsPOSDBConstants.Log_Entering);

            BindingControls();

            //clsUIHelper.setColorSchecme(this);
            this.txtCustomerName.AfterEnterEditMode += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
            this.txtCustomerName.AfterExitEditMode += new System.EventHandler(clsUIHelper.AfterExitEditMode);
            this.txtCustomerFirstName.AfterEnterEditMode += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
            this.txtCustomerFirstName.AfterExitEditMode += new System.EventHandler(clsUIHelper.AfterExitEditMode);
            this.txtAddress1.AfterEnterEditMode += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
            this.txtAddress1.AfterExitEditMode += new System.EventHandler(clsUIHelper.AfterExitEditMode);
            this.txtAddress2.AfterEnterEditMode += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
            this.txtAddress2.AfterExitEditMode += new System.EventHandler(clsUIHelper.AfterExitEditMode);
            this.txtCity.AfterEnterEditMode += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
            this.txtCity.AfterExitEditMode += new System.EventHandler(clsUIHelper.AfterExitEditMode);
            this.txtState.AfterEnterEditMode += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
            this.txtState.AfterExitEditMode += new System.EventHandler(clsUIHelper.AfterExitEditMode);
            this.txtZipCode.Enter += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
            this.txtZipCode.Leave += new System.EventHandler(clsUIHelper.AfterExitEditMode);
            this.txtPhoneHome.Enter += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
            this.txtPhoneHome.Leave += new System.EventHandler(clsUIHelper.AfterExitEditMode);
            this.txtCellNo.Enter += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
            this.txtCellNo.Leave += new System.EventHandler(clsUIHelper.AfterExitEditMode);
            this.txtPhoneOff.Enter += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
            this.txtPhoneOff.Leave += new System.EventHandler(clsUIHelper.AfterExitEditMode);

            this.cboPrimaryContact.Enter += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
            this.cboPrimaryContact.Leave += new System.EventHandler(clsUIHelper.AfterExitEditMode);
            this.cmbLanguage.Enter += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
            this.cmbLanguage.Leave += new System.EventHandler(clsUIHelper.AfterExitEditMode);
            this.txtComments.Enter += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
            this.txtComments.Leave += new System.EventHandler(clsUIHelper.AfterExitEditMode);

            this.txtCustomerCode.AfterEnterEditMode += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
            this.txtCustomerCode.AfterExitEditMode += new System.EventHandler(clsUIHelper.AfterExitEditMode);
            this.txtEmailAddr.AfterEnterEditMode += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
            this.txtEmailAddr.AfterExitEditMode += new System.EventHandler(clsUIHelper.AfterExitEditMode);
            this.txtDrivingLicNo.Enter += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
            this.txtDrivingLicNo.Leave += new System.EventHandler(clsUIHelper.AfterExitEditMode);
            this.txtDrivingLicState.Enter += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
            this.txtDrivingLicState.Leave += new System.EventHandler(clsUIHelper.AfterExitEditMode);
            this.cboGender.Enter += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
            this.cboGender.Leave += new System.EventHandler(clsUIHelper.AfterExitEditMode);
            this.txtFaxNo.Enter += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
            this.txtFaxNo.Leave += new System.EventHandler(clsUIHelper.AfterExitEditMode);
            //this.txtHouseChargeID.Enter += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
            //this.txtHouseChargeID.Leave += new System.EventHandler(clsUIHelper.AfterExitEditMode);
            this.txtDiscount.Enter += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
            this.txtDiscount.Leave += new System.EventHandler(clsUIHelper.AfterExitEditMode);

            this.grdCCProfile.AfterEnterEditMode += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
            this.grdCCProfile.AfterExitEditMode += new System.EventHandler(clsUIHelper.AfterExitEditMode);

            if (!oCustomerRow.SaveCardProfile || oCustomerRow.CustomerId == 0)//PRIMEPOS-2896 Arvind
            {
                btnAddCard.Visible = false;
            }

            //Added by Manoj 9/11/2013
            ScanData += new ScanEventHandler(frmCustomers_ScanData);
            ScanData.Invoke();
            IsCanceled = true;
            if (!isEdit) dtpDateOfBirth.Value = "";
            if (UserPriviliges.IsUserHasPriviliges(UserPriviliges.Modules.AdminFunc.ID, UserPriviliges.Screens.CustomerLoyaltyCards.ID))// To edit Customer Information 24Dec2012
            {
                if (grdCLCards.Rows.Count > 0)
                {
                    BtnEditLoyalty.Enabled = true;
                }
                else
                {
                    BtnEditLoyalty.Enabled = false;
                }
            }
            else
            {
                BtnEditLoyalty.Visible = false;
            }
            //if (isEdit == true && oCustomerRow != null && oCustomerRow.LanguageId > 0)
            //{
            //    this.cmbLanguage.Value = oCustomerRow.LanguageId;
            //}
            this.ActiveControl = this.txtCustomerName;
            #region Customer Performance PRIMEPOS-2655
            dtFrom.DateTime = DateTime.Now.AddYears(-1);
            dtTo.DateTime = DateTime.Now;
            #endregion
            logger.Trace("frmCustomers_Load() - " + clsPOSDBConstants.Log_Exiting);
        }

        private void BindingControls()
        {
            logger.Trace("BindingControls() - " + clsPOSDBConstants.Log_Entering);
            txtAccountNo.DataBindings.Add("Text", oCustomerRow, "AccountNumber", true, DataSourceUpdateMode.OnPropertyChanged);
            txtCustomerName.DataBindings.Add("Text", oCustomerRow, "CustomerName", true, DataSourceUpdateMode.OnPropertyChanged);
            txtCustomerFirstName.DataBindings.Add("Text", oCustomerRow, "FirstName", true, DataSourceUpdateMode.OnPropertyChanged);
            txtAddress1.DataBindings.Add("Text", oCustomerRow, "Address1", true, DataSourceUpdateMode.OnPropertyChanged);
            txtAddress2.DataBindings.Add("Text", oCustomerRow, "Address2", true, DataSourceUpdateMode.OnPropertyChanged);
            txtCity.DataBindings.Add("Text", oCustomerRow, "City", true, DataSourceUpdateMode.OnPropertyChanged);
            txtState.DataBindings.Add("Text", oCustomerRow, "State", true, DataSourceUpdateMode.OnPropertyChanged);
            txtZipCode.DataBindings.Add("Text", oCustomerRow, "Zip", true, DataSourceUpdateMode.OnPropertyChanged);
            txtPhoneHome.DataBindings.Add("Text", oCustomerRow, "PhoneHome", true, DataSourceUpdateMode.OnPropertyChanged);
            txtCellNo.DataBindings.Add("Text", oCustomerRow, "CellNo", true, DataSourceUpdateMode.OnPropertyChanged);
            txtPhoneOff.DataBindings.Add("Text", oCustomerRow, "PhoneOffice", true, DataSourceUpdateMode.OnPropertyChanged);
            txtComments.DataBindings.Add("Text", oCustomerRow, "Comments", true, DataSourceUpdateMode.OnPropertyChanged);
            txtCustomerCode.DataBindings.Add("Text", oCustomerRow, "CustomerCode", true, DataSourceUpdateMode.OnPropertyChanged);
            txtEmailAddr.DataBindings.Add("Text", oCustomerRow, "Email", true, DataSourceUpdateMode.OnPropertyChanged);
            txtDrivingLicNo.DataBindings.Add("Text", oCustomerRow, "DriveLicNo", true, DataSourceUpdateMode.OnPropertyChanged);
            txtDrivingLicState.DataBindings.Add("Text", oCustomerRow, "DriveLicState", true, DataSourceUpdateMode.OnPropertyChanged);
            txtFaxNo.DataBindings.Add("Text", oCustomerRow, "FaxNo", true, DataSourceUpdateMode.OnPropertyChanged);
            txtHouseChargeID.DataBindings.Add("Tag", oCustomerRow, "HouseChargeAcctID", true, DataSourceUpdateMode.OnPropertyChanged);
            txtDiscount.DataBindings.Add("Text", oCustomerRow, "Discount", true, DataSourceUpdateMode.OnPropertyChanged);

            cboPrimaryContact.DataBindings.Add("Value", oCustomerRow, "PrimaryContact", true, DataSourceUpdateMode.OnPropertyChanged);
            cmbLanguage.DataBindings.Add("Value", oCustomerRow, "LanguageId", true, DataSourceUpdateMode.OnPropertyChanged);
            cboGender.DataBindings.Add("Value", oCustomerRow, "Gender", true, DataSourceUpdateMode.OnPropertyChanged);

            dtpDateOfBirth.DataBindings.Add("Value", oCustomerRow, "DateOfBirth", true, DataSourceUpdateMode.OnPropertyChanged);

            chkUseForCustomerLoyalty.DataBindings.Add("Checked", oCustomerRow, "UseForCustomerLoyalty", true, DataSourceUpdateMode.OnPropertyChanged);
            chkSaveCardProfile.DataBindings.Add("Checked", oCustomerRow, "SaveCardProfile", true, DataSourceUpdateMode.OnPropertyChanged);
            chkIsActive.DataBindings.Add("Checked", oCustomerRow, "IsActive", true, DataSourceUpdateMode.OnPropertyChanged);
            logger.Trace("BindingControls() - " + clsPOSDBConstants.Log_Exiting);
        }

        private void PopulateGender()
        {
            try
            {
                logger.Trace("PopulateGender() - " + clsPOSDBConstants.Log_Entering);
                if (GenderList == null)
                {
                    GenderList = new ObservableCollection<ComboItem>();
                }
                this.GenderList.Add(new ComboItem(0, "Male"));
                this.GenderList.Add(new ComboItem(1, "Female"));

                this.cboGender.DataBindings.Add("DataSource", this, "GenderList");

                this.cboGender.DisplayMember = "Text";
                this.cboGender.ValueMember = "Value";

                logger.Trace("PopulateGender() - " + clsPOSDBConstants.Log_Exiting);
            }
            catch (Exception Ex)
            {
                logger.Fatal(Ex, "PopulateGender()");
            }
        }

        private void PopulatePrimaryContact()
        {
            try
            {
                logger.Trace("PopulatePrimaryContact() - " + clsPOSDBConstants.Log_Entering);
                if (PrimaryContactList == null)
                {
                    PrimaryContactList = new ObservableCollection<ComboItem>();
                }
                this.PrimaryContactList.Add(new ComboItem(0, "Cell No."));
                this.PrimaryContactList.Add(new ComboItem(1, "Phone Office"));
                this.PrimaryContactList.Add(new ComboItem(2, "Phone Home"));

                this.cboPrimaryContact.DataBindings.Add("DataSource", this, "PrimaryContactList");
                this.cboPrimaryContact.DisplayMember = "Text";
                this.cboPrimaryContact.ValueMember = "Value";

                logger.Trace("PopulatePrimaryContact() - " + clsPOSDBConstants.Log_Exiting);
            }
            catch (Exception Ex)
            {
                logger.Fatal(Ex, "PopulatePrimaryContact()");
            }
        }

        /// <summary>
        /// Author: Shitaljit Aded Date 10/10/2013
        /// Added to Load Language 
        /// </summary>
        private void PopulateLanguage()
        {
            try
            {
                logger.Trace("PopulateLanguage() - " + clsPOSDBConstants.Log_Entering);
                cmbLanguage.Items.Clear();
                Language oLanguage = new Language();
                dtLanguage = oLanguage.PopulateList();
                if (Configuration.isNullOrEmptyDataTable(dtLanguage) == true)
                {
                    clsUIHelper.ShowErrorMsg("Fail to populate languages.");
                    return;
                }
                if (LanguageList == null)
                {
                    LanguageList = new ObservableCollection<ComboItem>();
                }
                foreach (DataRow oRow in dtLanguage.Rows)
                {
                    this.LanguageList.Add(new ComboItem(Configuration.convertNullToInt(oRow[clsPOSDBConstants.Language_Fld_ID]), Configuration.convertNullToString(oRow[clsPOSDBConstants.Language_Fld_Name])));
                }
                this.cmbLanguage.DataBindings.Add("DataSource", this, "LanguageList");
                this.cmbLanguage.DisplayMember = "Text";
                this.cmbLanguage.ValueMember = "Value";
                logger.Trace("PopulateLanguage() - " + clsPOSDBConstants.Log_Exiting);
            }
            catch (Exception Ex)
            {
                logger.Fatal(Ex, "PopulateLanguage()");
                clsUIHelper.ShowErrorMsg(Ex.Message);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!ValidateFields()) return;

            if (Save())
            {
                IsCanceled = false;
                this.Close();
            }
        }

        #region validations
        private bool ValidateFields()
        {
            Boolean nStatus = true;
            try
            {
                logger.Trace("ValidateFields() - " + clsPOSDBConstants.Log_Entering);

                if (string.IsNullOrWhiteSpace(txtCustomerName.Text))
                {
                    errorProvider.SetIconPadding(txtCustomerName, 2);
                    errorProvider.SetError(txtCustomerName, "Customer name cannot be null.");
                    nStatus = false;
                }
                if (string.IsNullOrWhiteSpace(txtCustomerFirstName.Text))
                {
                    errorProvider.SetIconPadding(txtCustomerFirstName, 2);
                    errorProvider.SetError(txtCustomerFirstName, "Customer first name cannot be null.");
                    nStatus = false;
                }
                if (string.IsNullOrWhiteSpace(txtAddress1.Text))
                {
                    errorProvider.SetIconPadding(txtAddress1, 2);
                    errorProvider.SetError(txtAddress1, "Customer address cannot be null.");
                    nStatus = false;
                }
                if (string.IsNullOrWhiteSpace(txtCity.Text))
                {
                    errorProvider.SetIconPadding(txtCity, 2);
                    errorProvider.SetError(txtCity, "Customer city cannot be null.");
                    nStatus = false;
                }
                if (string.IsNullOrWhiteSpace(txtState.Text))
                {
                    errorProvider.SetIconPadding(txtState, 2);
                    errorProvider.SetError(txtState, "Customer state cannot be null.");
                    nStatus = false;
                }
                #region Sprint-25 - PRIMEPOS-433 03-Feb-2017 JY Added
                if (string.IsNullOrWhiteSpace(dtpDateOfBirth.Value.ToString()))
                {
                    errorProvider.SetIconPadding(dtpDateOfBirth, 2);
                    errorProvider.SetError(dtpDateOfBirth, "Please select a valid Date of Birth.");
                    nStatus = false;
                }
                else if (Convert.ToDateTime(dtpDateOfBirth.Value).Date >= DateTime.Now.Date)
                {
                    errorProvider.SetIconPadding(dtpDateOfBirth, 2);
                    errorProvider.SetError(dtpDateOfBirth, "Date of Birth should be less than Todays Date.");
                    nStatus = false;
                }
                #endregion
                if (!string.IsNullOrWhiteSpace(this.txtEmailAddr.Text) && !CheckvalidEmailid.IsMatch(txtEmailAddr.Text.Trim()))
                {
                    errorProvider.SetIconPadding(txtEmailAddr, 2);
                    errorProvider.SetError(txtEmailAddr, "Invalid email format.");
                    nStatus = false;
                }
                logger.Trace("ValidateFields() - " + clsPOSDBConstants.Log_Exiting);
                return nStatus;
            }
            catch(Exception Ex)
            {
                logger.Fatal(Ex, "ValidateFields()");
                return false;
            }
        }

        private void dtpDateOfBirth_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                logger.Trace("dtpDateOfBirth_Validating() - " + clsPOSDBConstants.Log_Entering);
                if (string.IsNullOrWhiteSpace(dtpDateOfBirth.Value.ToString()))
                {
                    errorProvider.SetIconPadding(dtpDateOfBirth, 2);
                    errorProvider.SetError(dtpDateOfBirth, "Please select a valid Date of Birth.");
                }
                else if (Convert.ToDateTime(dtpDateOfBirth.Value).Date >= DateTime.Now.Date)
                {
                    errorProvider.SetIconPadding(dtpDateOfBirth, 2);
                    errorProvider.SetError(dtpDateOfBirth, "Date of Birth should be less than Todays Date.");
                }
                logger.Trace("dtpDateOfBirth_Validating() - " + clsPOSDBConstants.Log_Exiting);
            }
            catch (Exception Ex)
            {
                logger.Fatal(Ex, "ValidateFields()");
                clsUIHelper.ShowErrorMsg(Ex.Message);
            }
        }

        private void txtEmailAddr_Validated(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(this.txtEmailAddr.Text) && !CheckvalidEmailid.IsMatch(txtEmailAddr.Text.Trim()))
            {
                errorProvider.SetIconPadding(txtEmailAddr, 2);
                errorProvider.SetError(txtEmailAddr, "Invalid email format.");
            }
        }

        private void txtHouseChargeID_Validating(object sender, CancelEventArgs e)
        {
            if (txtHouseChargeID.Text.Trim().Length != 0)
            {
                SearchHouseChargeInfo();
            }
            else
            {
                ClearHouseChargeCtrl();
            }
        }
        #endregion

        private bool Save()
        {
            try
            {
                logger.Trace("Save() - " + clsPOSDBConstants.Log_Entering);
                //oCustomerRow.PrimaryContact = POS_Core.Resources.Configuration.convertNullToInt(this.cboPrimaryContact.Value.ToString());
                //oCustomerRow.Gender = POS_Core.Resources.Configuration.convertNullToInt(this.cboGender.Value.ToString());
                //if (cmbLanguage.SelectedItem != null)
                //{
                //    oCustomerRow.LanguageId = Configuration.convertNullToInt(this.cmbLanguage.Value);
                //}
                oBRCustomer.Persist(oCustomerData);
                logger.Trace("Save() - " + clsPOSDBConstants.Log_Exiting);
                return true;
            }
            catch (POSExceptions exp)
            {
                logger.Fatal(exp, "Save()");
                clsUIHelper.ShowErrorMsg(exp.ErrMessage);
                switch (exp.ErrNumber)
                {
                    case (long)POSErrorENUM.Customer_DuplicateCode:
                        txtCustomerCode.Focus();
                        break;
                    case (long)POSErrorENUM.Customer_CodeCanNotBeNULL:
                        txtCustomerCode.Focus();
                        break;
                }
                return false;
            }
            catch (Exception exp)
            {
                logger.Fatal(exp, "Save()");
                clsUIHelper.ShowErrorMsg(exp.Message);
                return false;
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            IsCanceled = true;
            this.Close();
        }

        /// <summary>
        /// Manoj 9/11/2013
        /// Textbox Events 
        /// </summary>
        void frmCustomers_ScanData()
        {
            txtCustomerName.TextChanged += new EventHandler(txtCustomerName_TextChanged);
            txtCustomerName.Leave += new EventHandler(txtCustomerName_Leave);
        }

        /// <summary>
        ///  Manoj 9/11/2013
        /// After Scan driver lic# it move to the next text box so it parse scan data on leaving
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtCustomerName_Leave(object sender, EventArgs e)
        {
            if (isScan && Data != string.Empty)
            {
                logger.Trace("txtCustomerName_Leave(object sender, EventArgs e) - " + Data); //PRIMEPOS-3162 18-Nov-2022 JY Added
                if (ScanID != null)
                {
                    ScanID = null;
                }
                ScanID = new DL(Data);
                isScan = false;
                txtCustomerName.Multiline = false;
                txtCustomerName.PasswordChar = '\0';
                txtCustomerName.Text = "";
                AfterScanData += new ScanEventHandler(frmCustomers_AfterScanData);
                AfterScanData.Invoke();
            }
        }

        /// <summary>
        ///  Manoj 9/11/2013
        /// Populate fields after Scan and Parse
        /// </summary>
        void frmCustomers_AfterScanData()
        {
            logger.Trace("frmCustomers_AfterScanData() - " + clsPOSDBConstants.Log_Entering);
            if (!string.IsNullOrEmpty(ScanID.DAQ))
            {
                txtCustomerName.Text = ScanID.DCS; //Last Name
                txtCustomerFirstName.Text = ScanID.DCT; //First Name
                txtAddress1.Text = ScanID.DAG; //Address
                txtCity.Text = ScanID.DAI; //City
                txtState.Text = ScanID.DAJ; //State
                txtZipCode.Text = ScanID.DAK; //zip
                txtDrivingLicState.Text = ScanID.DAJ; //State
                txtDrivingLicNo.Text = ScanID.DAQ.Trim(); //Lic #
                try
                {
                    //cboGender.Text = ScanID.DBC; //Sex
                    if (ScanID.DBC.ToUpper().StartsWith("M"))
                    {
                        cboGender.SelectedIndex = 0;
                    }
                    else
                    {
                        cboGender.SelectedIndex = 1;
                    }
                }
                catch (Exception Ex)
                {
                    logger.Fatal(Ex, "frmCustomers_AfterScanData()");
                }
                logger.Trace("frmCustomers_AfterScanData() - DOB: " + ScanID.DBB);  //PRIMEPOS-3162 18-Nov-2022 JY Added
                dtpDateOfBirth.Text = ScanID.DBB.Substring(0, 2) + "/" + ScanID.DBB.Substring(2, 2) + "/" + ScanID.DBB.Substring(4, 4); //DOB
                try
                {
                    if (oCustomerRow == null)
                    {
                        logger.Trace("frmCustomers_AfterScanData() - " + clsPOSDBConstants.Log_Exiting);
                        return;
                    }
                    else
                    {
                        oCustomerRow.CustomerName = txtCustomerName.Text;
                        oCustomerRow.Address1 = txtAddress1.Text;
                        oCustomerRow.City = txtCity.Text;
                        oCustomerRow.State = txtState.Text;
                        oCustomerRow.FirstName = txtCustomerFirstName.Text;
                        oCustomerRow.DriveLicNo = txtDrivingLicNo.Text;
                        oCustomerRow.DriveLicState = txtDrivingLicState.Text;
                        oCustomerRow.Zip = txtZipCode.Text;
                        oCustomerRow.DateOfBirth = Convert.ToDateTime(dtpDateOfBirth.Value);
                        if (cboGender.SelectedIndex >= 0)
                        {
                            oCustomerRow.Gender = cboGender.SelectedIndex;
                        }
                        else
                        {
                            oCustomerRow.Gender = 0;
                        }
                    }
                }
                catch (Exception Ex)
                {
                    logger.Fatal(Ex, "frmCustomers_AfterScanData()");
                }
            }
            logger.Trace("frmCustomers_AfterScanData() - " + clsPOSDBConstants.Log_Exiting);
        }

        /// <summary>
        ///  Manoj 9/11/2013
        /// Last Name Text change event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtCustomerName_TextChanged(object sender, EventArgs e)
        {
            Data = txtCustomerName.Text;
            if (!isScan && Data.StartsWith("@"))
            {
                isScan = true;
                txtCustomerName.Multiline = true;
                txtCustomerName.PasswordChar = 'X';
                txtCustomerName.MaxLength = 42000;
            }
        }

        private void frmCustomers_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyData == System.Windows.Forms.Keys.Enter)
                {
                    this.SelectNextControl(this.ActiveControl, true, true, true, true);
                }
                else if (e.KeyData == Keys.F6)
                {
                    ShowNotes();
                }
                else if (e.KeyData == Keys.F3 && this.btnFromPharmacy.Visible == true)
                {
                    PopulateCustomerFromPharmacy();
                }
            }
            catch (Exception Ex)
            {
                logger.Fatal(Ex, "frmCustomers_KeyDown()");
                clsUIHelper.ShowErrorMsg(Ex.Message);
            }
        }

        private void frmCustomers_Activated(object sender, EventArgs e)
        {
            clsUIHelper.CurrentForm = this;
        }

        #region print label
        private void btnPrint_Click(object sender, EventArgs e)
        {
            PrintCustomerLabel();
        }

        private void PrintCustomerLabel()
        {
            Image oImage = null;
            try
            {
                logger.Trace("PrintCustomerLabel() - " + clsPOSDBConstants.Log_Entering);

                if (this.oCustomerRow == null)
                {
                    return;
                }

                string sImagePath = System.IO.Path.Combine(System.IO.Path.GetTempPath(), oCustomerRow.AccountNumber.ToString() + ".bmp");

                Mabry.Windows.Forms.Barcode.Licenser.Key = "E1P8-HKELVF8R04Q0";

                string sBarcode = "89" + EncryptString.CustomEncrypt(oCustomerRow.AccountNumber) + "89";

                if (System.IO.File.Exists(sImagePath) == true)
                {
                    System.IO.File.Delete(sImagePath);
                }

                Configuration.PrintBarcode(sBarcode, 0, 0, 20, 200, "CODE128", "H", sImagePath);

                oImage = Image.FromFile(sImagePath);
                CustomerLabel oCLable = new CustomerLabel(this.oCustomerRow, oImage);
                oCLable.Print();
                oImage.Dispose();
                logger.Trace("PrintCustomerLabel() - " + clsPOSDBConstants.Log_Exiting);
            }
            catch (Exception Ex)
            {
                logger.Fatal(Ex, "PrintCustomerLabel()");
                clsUIHelper.ShowErrorMsg(Ex.Message);
            }
            finally
            {
                if (oImage != null)
                {
                    oImage.Dispose();
                }
            }
        }
        #endregion

        #region customer notes
        private void btnCustomerNotes_Click(object sender, EventArgs e)
        {
            try
            {
                logger.Trace("btnCustomerNotes_Click() - " + clsPOSDBConstants.Log_Entering);
                ShowNotes();
                logger.Trace("btnCustomerNotes_Click() - " + clsPOSDBConstants.Log_Exiting);
            }
            catch (Exception Ex)
            {
                logger.Fatal(Ex, "btnCustomerNotes_Click()");
                clsUIHelper.ShowErrorMsg(Ex.Message);
            }
        }

        private void ShowNotes()
        {
            if (this.btnCustomerNotes.Visible == true)
            {
                frmCustomerNotes oNotes = new frmCustomerNotes(this.oCustomerRow);
                oNotes.ShowDialog(this);
            }
        }
        #endregion        

        #region House Charge Id
        private void txtHouseChargeID_EditorButtonClick(object sender, Infragistics.Win.UltraWinEditors.EditorButtonEventArgs e)
        {
            SearchHouseChargeInfo();
        }

        private void SearchHouseChargeInfo()
        {
            try
            {
                logger.Trace("SearchHouseChargeInfo() - " + clsPOSDBConstants.Log_Entering);

                if (Configuration.CPOSSet.UsePrimeRX == false)
                {
                    txtHouseChargeID.Enabled = false;
                    ClearHouseChargeCtrl();
                }
                {
                    if (this.txtHouseChargeID.Text.Length > 0 && txtHouseChargeID.Tag != null)
                    {
                        if (this.txtHouseChargeID.Text == oBRCustomer.GetHouseChargeName(txtHouseChargeID.Tag.ToString()))
                        {
                            return;
                        }
                    }

                    //frmSearch oSearch = new frmSearch(clsPOSDBConstants.PrimeRX_HouseChargeInterface, "", this.txtHouseChargeID.Text.Replace(",", ""));
                    frmSearchMain oSearch = new frmSearchMain(clsPOSDBConstants.PrimeRX_HouseChargeInterface, "", this.txtHouseChargeID.Text.Replace(",", ""), true);   //20-Dec-2017 JY Added new reference
                    oSearch.ShowDialog(this);
                    if (!oSearch.IsCanceled)
                    {
                        string strCode = oSearch.SelectedRowID();
                        if (strCode == "")
                        {
                            ClearHouseChargeCtrl();
                        }
                        else
                        {
                            this.txtHouseChargeID.Tag = strCode;
                            ShowHouseChargeName(strCode);
                            //oCustomerRow.HouseChargeAcctID = Configuration.convertNullToInt(strCode);
                        }
                    }
                }
                logger.Trace("SearchHouseChargeInfo() - " + clsPOSDBConstants.Log_Exiting);
            }
            catch (Exception Ex)
            {
                logger.Fatal(Ex, "SearchHouseChargeInfo()");
                ClearHouseChargeCtrl();
                clsUIHelper.ShowErrorMsg(Ex.Message);
            }
        }

        private void ShowHouseChargeName(string sAccountID)
        {
            this.txtHouseChargeID.Text = oBRCustomer.GetHouseChargeName(sAccountID);
        }

        private void ClearHouseChargeCtrl()
        {
            txtHouseChargeID.Text = string.Empty;
            txtHouseChargeID.Tag = null;
            //oCustomerRow.HouseChargeAcctID = 0;
        }
        #endregion

        private void btnFromPharmacy_Click(object sender, EventArgs e)
        {
            PopulateCustomerFromPharmacy();
        }

        private void PopulateCustomerFromPharmacy()
        {
            try
            {
                logger.Trace("PopulateCustomerFromPharmacy() - " + clsPOSDBConstants.Log_Entering);

                //frmSearch oSearch = null;
                //oSearch = new frmSearch(clsPOSDBConstants.PrimeRX_PatientInterface, "", "");
                frmSearchMain oSearch = null;
                oSearch = new frmSearchMain(clsPOSDBConstants.PrimeRX_PatientInterface, "", "", true);  //20-Dec-2017 JY Added new reference

                oSearch.ShowDialog(this);
                if (!oSearch.IsCanceled)
                {
                    int iPatientNo = Configuration.convertNullToInt(oSearch.SelectedRowID());
                    if (iPatientNo == 0)
                        return;

                    MMSChargeAccount.ContAccount oAcct = new ContAccount();

                    DataSet oDS = new DataSet();
                    oAcct.GetPatientByCode(iPatientNo, out oDS);


                    if (oDS == null)
                        throw (new Exception("Invalid patient no selected."));
                    else if (oDS.Tables[0].Rows.Count == 0)
                        throw (new Exception("Invalid patient no selected."));
                    else
                    {
                        Clear();
                        oCustomerRow = oCustomerData.Customer.AddRow(0, "", "", "", "", "", "", "", "", "", "", "", true, 0, 0, Configuration.MinimumDate, "", 0, true, 0, 0, false); //Sprint-23 - PRIMEPOS-2314 10-Jun-2016 JY Added SaveCardProfile parameter

                        if (Configuration.CInfo.SaveCCToken == true && Configuration.CInfo.DefaultCustomerTokenValue == true)   //Sprint-25 - PRIMEPOS-2373 16-Feb-2017 JY Added
                            oCustomerRow.SaveCardProfile = true;

                        oCustomerRow.CustomerCode = this.txtCustomerCode.Text = oDS.Tables[0].Rows[0]["patientno"].ToString();

                        oCustomerRow.PatientNo = Convert.ToInt32(oDS.Tables[0].Rows[0]["PatientNo"].ToString().Trim());
                        this.txtCustomerName.Text = oCustomerRow.CustomerName = oDS.Tables[0].Rows[0]["lname"].ToString();
                        this.txtCustomerFirstName.Text = oCustomerRow.FirstName = oDS.Tables[0].Rows[0]["fname"].ToString();

                        this.txtAddress1.Text = oCustomerRow.Address1 = oDS.Tables[0].Rows[0]["addrstr"].ToString();
                        this.txtAddress2.Text = oCustomerRow.Address2 = oDS.Tables[0].Rows[0]["ADDRSTRLINE2"].ToString();

                        //this.txtEmailAddr.Text = oCustomerRow.Email = oDS.Tables[0].Rows[0]["email"].ToString();  //PRIMEPOS-2903 06-Oct-2020 JY Commented
                        #region PRIMEPOS-2903 06-Oct-2020 JY Added
                        string strEmailId = Configuration.convertNullToString(oDS.Tables[0].Rows[0]["email"]).ToLower().Trim();
                        if (!string.IsNullOrWhiteSpace(strEmailId) && CheckvalidEmailid.IsMatch(strEmailId))
                            this.txtEmailAddr.Text = oCustomerRow.Email = strEmailId;
                        #endregion

                        this.txtDrivingLicNo.Text = oCustomerRow.DriveLicNo = oDS.Tables[0].Rows[0]["driverslicense"].ToString();

                        this.txtCellNo.Text = oCustomerRow.CellNo = oDS.Tables[0].Rows[0]["mobileno"].ToString();
                        this.txtCity.Text = oCustomerRow.City = oDS.Tables[0].Rows[0]["addrct"].ToString();
                        this.txtState.Text = oCustomerRow.State = oDS.Tables[0].Rows[0]["addrst"].ToString();
                        this.txtZipCode.Text = oCustomerRow.Zip = oDS.Tables[0].Rows[0]["addrzp"].ToString();

                        this.txtPhoneHome.Text = oCustomerRow.PhoneHome = oDS.Tables[0].Rows[0]["phone"].ToString();
                        this.txtComments.Text = oCustomerRow.Comments = oDS.Tables[0].Rows[0]["Remark"].ToString();

                        DateTime oDOB;
                        if (DateTime.TryParse(oDS.Tables[0].Rows[0]["DOB"].ToString(), out oDOB) == true)
                        {
                            this.dtpDateOfBirth.Value = oCustomerRow.DateOfBirth = oDOB;
                        }
                        if (oDS.Tables[0].Rows[0]["SEX"].ToString().Trim().ToUpper() == "F")
                        {
                            oCustomerRow.Gender = 1;
                            this.cboGender.Value = 1;
                        }
                        else
                        {
                            oCustomerRow.Gender = 0;
                            cboGender.Value = 0;
                        }

                        this.chkIsActive.Checked = true;
                        this.chkUseForCustomerLoyalty.Checked = true;
                        this.cboPrimaryContact.SelectedIndex = 2;
                        this.chkSaveCardProfile.Checked = true; //Sprint-23 - PRIMEPOS-2314 09-Jun-2016 JY Added
                    }
                }
                logger.Trace("PopulateCustomerFromPharmacy() - " + clsPOSDBConstants.Log_Exiting);
            }
            catch (Exception Ex)
            {
                logger.Fatal(Ex, "PopulateCustomerFromPharmacy()");
                clsUIHelper.ShowErrorMsg(Ex.Message);
            }
        }

        private void BtnEditLoyalty_Click(object sender, EventArgs e)
        {
            try
            {
                logger.Trace("BtnEditLoyalty_Click() - " + clsPOSDBConstants.Log_Entering);
                if (grdCLCards.Rows.Count <= 0) return;
                frmCLCards ofrmCLCards = new frmCLCards();
                ofrmCLCards.Edit(Configuration.convertNullToInt64(grdCLCards.ActiveRow.Cells["CardID"].Text.Trim()));
                ofrmCLCards.ShowDialog(this);
                logger.Trace("BtnEditLoyalty_Click() - " + clsPOSDBConstants.Log_Exiting);
            }
            catch (Exception Ex)
            {
                logger.Fatal(Ex, "BtnEditLoyalty_Click()");
                clsUIHelper.ShowErrorMsg(Ex.Message);
            }
        }

        private void tabCustomerInfo_SelectedTabChanged(object sender, Infragistics.Win.UltraWinTabControl.SelectedTabChangedEventArgs e)
        {
            if (tabCustomerInfo.SelectedTab.Key == "CL")
            {
                if (grdCLCards.Rows.Count > 0)
                {
                    this.grdCLCards.ActiveRow = this.grdCLCards.Rows[0];
                    grdCLCards.Focus();
                }
            }
            else if (tabCustomerInfo.SelectedTab.Key == "CCP")  //Sprint-23 - PRIMEPOS-2316 16-Jun-2016 JY Added
            {
                if (grdCCProfile.Rows.Count > 0)
                {
                    this.grdCCProfile.ActiveRow = this.grdCCProfile.Rows[0];
                    grdCCProfile.Focus();
                }
            }
            else if (tabCustomerInfo.SelectedTab.Key == "CPF")  //Customer Perfromance PRIMEPOS-2655
            {
                PopulateCustomerPerformance(oCustomerRow.CustomerId);
            }
            else if (tabCustomerInfo.SelectedTab.Key == "SCD")  //PRIMEPOS-2747 - StoreCredit - Nileshj
            {
                PopulateCustomerStoreTransactionDetails(oCustomerRow.CustomerId);
            }
        }

        //Sprint-23 - PRIMEPOS-2316 15-Jun-2016 JY Added to delete CC Profile
        private void btnDeleteCCProfile_Click(object sender, EventArgs e)
        {
            try
            {
                logger.Trace("btnDeleteCCProfile_Click() - " + clsPOSDBConstants.Log_Entering);
                //if (this.lvCCProfile.SelectedItems == null || this.lvCCProfile.SelectedItems.Count == 0) return;
                if (grdCCProfile.Rows.Count < 1)
                    return;

                //if (Resources.Message.Display("Do you want to delete profile: " + lvCCProfile.SelectedItems[0].SubItems[3].Text.ToString() + " ?", "Delete Profile", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                if (Resources.Message.Display("Do you want to delete profile: " + grdCCProfile.ActiveRow.Cells["ProfiledID"].Text.ToString() + " ? ", "Delete Profile", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    string sUserID = string.Empty;
                    bool bDelete = UserPriviliges.getPermission(UserPriviliges.Modules.AdminFunc.ID, UserPriviliges.Screens.DeleteCreditCardProfiles.ID, 0, UserPriviliges.Screens.DeleteCreditCardProfiles.Name, out sUserID);
                    if (bDelete == false) return;

                    CCCustomerTokInfo o = new CCCustomerTokInfo();
                    bool bStatus = o.DeleteToken(Configuration.convertNullToInt(grdCCProfile.ActiveRow.Cells["EntryID"].Text));
                    if (bStatus) grdCCProfile.DeleteSelectedRows();
                    if (Configuration.convertNullToInt(oCustomerRow.CustomerId) != 0)   PopulateCreditCardProfiles(oCustomerRow.CustomerId);    //09-Oct-2020 JY Added
                }
                logger.Trace("btnDeleteCCProfile_Click() - " + clsPOSDBConstants.Log_Exiting);
            }
            catch (Exception Ex)
            {
                logger.Fatal(Ex, "btnDeleteCCProfile_Click()");
                clsUIHelper.ShowErrorMsg(Ex.Message);
            }
        }

        #region PRIMEPOS-2442 ADDED BT ROHIT NAIR
        private void btnConsent_Click(object sender, EventArgs e)
        {
            logger.Trace("btnConsent_Click() - " + clsPOSDBConstants.Log_Entering);

            if (Configuration.CInfo.EnableConsentCapture && !string.IsNullOrWhiteSpace(Configuration.CInfo.SelectedConsentSource))
            {
                if (oCustomerRow != null)
                {
                    if (oCustomerRow.PatientNo <= 0)
                    {
                        Resources.Message.Display("Unable to get Consent, Customer does not appear to be linked to a PrimeRx Patient!!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    bool bsaveConsent = false;
                    PatientConsent oconsent = null;
                    byte[] sigdata = null;
                    if (Configuration.CPOSSet.UseSigPad == true)
                    {
                        if (SigPadUtil.DefaultInstance.isISC)
                        {

                            if (!SigPadUtil.DefaultInstance.isConnected())
                            {
                                startSigpad();
                            }
                            bool? val = CapturePatientConsent(Configuration.CInfo.SelectedConsentSource.ToUpper().Trim(), out oconsent);
                            if (oconsent != null && !oconsent.IsConsentSkip)
                            {
                                try
                                {
                                    PharmBL oPBl = new PharmBL();
                                    oconsent.ConsentSourceID = oPBl.GetConsentSourceID(oconsent.ConsentSourceName);
                                    oconsent.ConsentTextID = oPBl.GetConsentTextID(oconsent.ConsentSourceID, 1);
                                    oconsent.ConsentTypeID = oPBl.GetConsentTypeID(oconsent.ConsentSourceID, oconsent.ConsentTypeCode);
                                    oconsent.ConsentStatusID = oPBl.GetConsentStatusID(oconsent.ConsentSourceID, oconsent.ConsentStatusCode);
                                    oconsent.PatConsentRelationID = oPBl.GetConsentRelationShipID(oconsent.ConsentSourceID, oconsent.PatConsentRelationShipDescription);

                                    oconsent.ConsentCaptureDate = DateTime.Now;
                                    oconsent.ConsentEffectiveDate = DateTime.Now;
                                    oconsent.ConsentEndDate = DateTime.Today.AddDays(oPBl.GetConsentValidityPeriod(oconsent.ConsentSourceID, oconsent.ConsentStatusID) + 1).AddSeconds(-1);//PRIMEPOS-3120
                                    //oconsent.ConsentEndDate = DateTime.MaxValue;  //Sprint-26 - PRIMEPOS-2442 24-Aug-2017 JY Consent end date should be null, don't know the impact so keeping parameter as it is
                                    bsaveConsent = true;
                                }
                                catch (Exception ex)
                                {
                                    string message = "An Error occured while Fetching Pat Consent Values" + ex.Message;
                                    logger.Error(ex, message);
                                    bsaveConsent = false;
                                    Resources.Message.Display(message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                            }
                            else
                            {
                                if (oconsent != null)
                                {
                                    if (oconsent.IsConsentSkip)
                                    {
                                        Resources.Message.Display("Unable to get Patient Consent!! Patient is not legal representative or Patient Skipped Consent", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                        logger.Trace("btnConsent_Click() - " + clsPOSDBConstants.Log_Exiting);
                                        return;
                                    }
                                }
                                bsaveConsent = false;
                            }
                            if (bsaveConsent)
                            {
                                CapturePatientSignature(out sigdata);
                            }

                            if (sigdata != null)
                            {
                                oconsent.SignatureData = sigdata;
                            }
                            else
                            {
                                bsaveConsent = false;
                            }

                            if (bsaveConsent)
                            {
                                try
                                {
                                    PharmBL oPBl = new PharmBL();
                                    oPBl.SavePatientConsent(oCustomerRow.PatientNo, oconsent.ConsentTextID, oconsent.ConsentTypeID, oconsent.ConsentStatusID, oconsent.ConsentCaptureDate,
                                       oconsent.ConsentEffectiveDate, oconsent.ConsentEndDate, oconsent.PatConsentRelationID, oconsent.SigneeName, oconsent.SignatureData);
                                }
                                catch (Exception ex)
                                {
                                    string message = "An error Occured while trying to save Patient Consnet Data " + ex.Message;
                                    logger.Error(ex, message);

                                    Resources.Message.Display(message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                            }
                            else
                            {
                                Resources.Message.Display("Unable to get Patient Consent at this time Please try again later!!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                            #region Sprint-26 - PRIMEPOS-2442 23-Aug-2017 JY Added logic to clear signature pad
                            if (SigPadUtil.DefaultInstance.isConnected())
                            {
                                startSigpad();
                            }
                            #endregion
                        }
                        else
                        {
                            Resources.Message.Display("Unable to get Consent, This Function is not supported in the current SignaturePad Model", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            logger.Trace("btnConsent_Click() - " + clsPOSDBConstants.Log_Exiting);
                            return;
                        }
                    }
                    else
                    {
                        Resources.Message.Display("Unable to get Consent, no Signaturepads Enabled in this Station", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        logger.Trace("btnConsent_Click() - " + clsPOSDBConstants.Log_Exiting);
                        return;
                    }
                }
            }
            else
            {
                Resources.Message.Display("Unable to get Consent, Setting Not Enabled", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                logger.Trace("btnConsent_Click() - " + clsPOSDBConstants.Log_Exiting);
                return;
            }
            logger.Trace("btnConsent_Click() - " + clsPOSDBConstants.Log_Exiting);
        }

        private bool? CapturePatientConsent(string ConsentSource, out PatientConsent oConsent)
        {
            logger.Trace("CapturePatientConsent() - " + clsPOSDBConstants.Log_Entering);
            bool? retVal = null;
            oConsent = null;
            bool? rval = null;
            if (Configuration.CPOSSet.UseSigPad == true && SigPadUtil.DefaultInstance.isConnected())
            {
                rval = SigPadUtil.DefaultInstance.CapturePatientConsent(ConsentSource, out oConsent);

                if (rval == true)
                {
                    retVal = true;
                }
                else
                {
                    retVal = false;
                }
            }
            else
            {
                retVal = false;
            }

            logger.Trace("CapturePatientConsent() - " + clsPOSDBConstants.Log_Exiting);
            return retVal;
        }

        private bool CapturePatientSignature(out byte[] sigdata)
        {
            logger.Trace("CapturePatientSignature() - " + clsPOSDBConstants.Log_Entering);
            sigdata = null; ;
            string strSignature = "";
            bool retVal = true;
            bool bPatSigDone = false;
            if (Configuration.CPOSSet.UseSigPad == true && SigPadUtil.DefaultInstance.isConnected())
            {
                while (!bPatSigDone)
                {
                    strSignature = "";
                    if (SigPadUtil.DefaultInstance.CapturePatientSignature())
                    {
                        strSignature = SigPadUtil.DefaultInstance.CustomerSignature;
                        bPatSigDone = true;
                        Bitmap bmp = new Bitmap(335, 245, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                        //Bitmap bmp = new Bitmap(335, 245, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
                        string errorMsg = string.Empty;
                        SigDiplay.SigDisplay sigDisp = new SigDiplay.SigDisplay();
                        if (SigPadUtil.DefaultInstance.isISC)
                        {
                            byte[] iscsig = Convert.FromBase64String(strSignature);
                            sigDisp.DrawSignatureMX(iscsig, ref bmp, out errorMsg);
                        }
                        else
                        {
                            sigDisp.DrawSignature(strSignature, ref bmp, out errorMsg, clsPOSDBConstants.BINARYIMAGE);
                        }
                        ImageConverter converter = new ImageConverter();
                        byte[] btarr = (byte[])converter.ConvertTo(bmp, typeof(byte[]));
                        sigdata = btarr;
                        retVal = true;
                    }
                    else
                    {
                        bPatSigDone = true;
                        retVal = false;
                    }
                }
            }
            logger.Trace("CapturePatientSignature() - " + clsPOSDBConstants.Log_Exiting);
            return retVal;
        }

        private void startSigpad()
        {
            if (Configuration.CPOSSet.UseSigPad == true)
            {
                //POS_Core.ErrorLogging.Logs.Logger(this.Text, "Showing Transaction Screen on Sig Pad", clsPOSDBConstants.Log_Entering);
                logger.Trace("SetNew() - Showing Transaction Screen on Sig Pad " + clsPOSDBConstants.Log_Entering);
                //sSigPadTransIDLast = sSigPadTransID;
                //string sSigPadTransID = Configuration.StationID + DateTime.Now.ToString("ddMMyyyyhhssmm");
                sSigPadTransIDLast = sSigPadTransID;
                sSigPadTransID = Configuration.StationID + DateTime.Now.ToString("ddMMyyyyhhssmm");
                SigPadUtil.DefaultInstance.isFinishDevice = true;
                SigPadUtil.DefaultInstance.StartTransaction(sSigPadTransID, "");
                SigPadUtil.DefaultInstance.isFinishDevice = false;
                //POS_Core.ErrorLogging.Logs.Logger(this.Text, "Showing Transaction Screen on Sig Pad", clsPOSDBConstants.Log_Exiting);
                logger.Trace("SetNew() - Showing Transaction Screen on Sig Pad " + clsPOSDBConstants.Log_Exiting);
            }
        }
        #endregion

        #region code seems to be not in use
        //private void btnNew_Click(object sender, System.EventArgs e)
        //{
        //    try
        //    {
        //        txtCustomerCode.Text = "";
        //        SetNew();
        //        this.txtCustomerName.Focus();
        //    }
        //    catch (Exception exp)
        //    {
        //        clsUIHelper.ShowErrorMsg(exp.Message);
        //    }
        //}        

        //private void txtCustomerCode_EditorButtonClick(object sender, Infragistics.Win.UltraWinEditors.EditorButtonEventArgs e)
        //{
        //    Search();
        //}

        //moved to bal
        //private string GetHouseChargeName(string sAccountID)
        //{
        //    string strReturnValue = "";
        //    ContAccount oAcct = new ContAccount();
        //    DataSet oDS = new DataSet();
        //    if (sAccountID.Trim() != string.Empty)  //PRIMEPOS-2205 02-Aug-2016 JY Added if condition as if AcctNo is NULL then it will return wrong data
        //        oAcct.GetAccountByCode(sAccountID, out oDS);

        //    if (oDS != null && oDS.Tables.Count > 0 && oDS.Tables[0].Rows.Count > 0)
        //    {
        //        strReturnValue = oDS.Tables[0].Rows[0]["acct_Name"].ToString();
        //    }

        //    oDS.Dispose();
        //    oAcct = null;

        //    return strReturnValue;
        //} 

        //private void btnSearch_Click(object sender, System.EventArgs e)
        //{
        //    Search();
        //}

        //Commented this code as it is related to edit customer on customer screen, which might be implemented when we dont have the list of customers and then ADD/EDIT functionalities
        //private void frmCustomers_KeyUp(object sender, KeyEventArgs e)
        //{
        //    try
        //    {
        //        if (e.KeyData == System.Windows.Forms.Keys.F4)
        //        {
        //            if (txtCustomerCode.ContainsFocus)
        //                this.Search();
        //        }
        //    }
        //    catch (Exception exp)
        //    {
        //        clsUIHelper.ShowErrorMsg(exp.Message);
        //    }
        //}

        //private void Search()
        //{
        //    try
        //    {
        //        frmSearch oSearch = new frmSearch(clsPOSDBConstants.Customer_tbl);
        //        oSearch.ShowDialog(this);
        //        if (!oSearch.IsCanceled)
        //        {
        //            string strCode = oSearch.SelectedRowID();
        //            if (strCode == "")
        //                return;

        //            Edit(strCode);
        //        }
        //    }
        //    catch (Exception exp)
        //    {
        //        clsUIHelper.ShowErrorMsg(exp.Message);
        //    }
        //}

        //this code is not required as we bind hte controls
        //private void txtBoxs_Validate(object sender, System.EventArgs e)
        //{
        //    try
        //    {
        //        //btnSearch.Enabled = (!oCustomerData.HasChanges());

        //        if (oCustomerRow == null)
        //            return;
        //        Infragistics.Win.UltraWinEditors.UltraTextEditor txtEditor = (Infragistics.Win.UltraWinEditors.UltraTextEditor)sender;
        //        switch (txtEditor.Name)
        //        {
        //            case "txtCustomerName":
        //                oCustomerRow.CustomerName = txtCustomerName.Text;
        //                break;
        //            case "txtPhoneHome":
        //                oCustomerRow.PhoneHome = txtPhoneHome.Text;
        //                break;
        //            case "txtAddress1":
        //                oCustomerRow.Address1 = txtAddress1.Text;
        //                break;
        //            case "txtAddress2":
        //                oCustomerRow.Address2 = txtAddress2.Text;
        //                break;
        //            case "txtCellNo":
        //                oCustomerRow.CellNo = txtCellNo.Text;
        //                break;
        //            case "txtCity":
        //                oCustomerRow.City = txtCity.Text;
        //                break;
        //            case "txtEmailAddr":
        //                {
        //                    if (this.txtEmailAddr.Text.Trim() != null && this.txtEmailAddr.Text.Trim() != "" && !CheckvalidEmailid.IsMatch(txtEmailAddr.Text.Trim()))
        //                    {
        //                        clsUIHelper.ShowErrorMsg("Invalid email format.");
        //                        this.txtEmailAddr.Focus();
        //                        break;
        //                    }
        //                    else
        //                    {
        //                        oCustomerRow.Email = txtEmailAddr.Text;
        //                        break;
        //                    }
        //                }

        //            case "txtFaxNo":
        //                oCustomerRow.FaxNo = txtFaxNo.Text;
        //                break;
        //            case "txtState":
        //                oCustomerRow.State = txtState.Text;
        //                break;
        //            case "txtComments":
        //                oCustomerRow.Comments = txtComments.Text;
        //                break;
        //            case "txtCustomerCode":
        //                oCustomerRow.CustomerCode = txtCustomerCode.Text;
        //                break;
        //            case "txtCustomerFirstName":
        //                oCustomerRow.FirstName = txtCustomerFirstName.Text;
        //                break;
        //            case "txtDrivingLicNo":
        //                oCustomerRow.DriveLicNo = txtDrivingLicNo.Text;
        //                break;
        //            case "txtDrivingLicState":
        //                oCustomerRow.DriveLicState = txtDrivingLicState.Text;
        //                break;
        //        }
        //    }
        //    catch (Exception)
        //    {
        //    }
        //}

        //private void MaskEditBoxes_Validate(object sender, System.EventArgs e)
        //{
        //    try
        //    {
        //        if (oCustomerRow == null)
        //            return;
        //        Infragistics.Win.UltraWinMaskedEdit.UltraMaskedEdit txtEditor = (Infragistics.Win.UltraWinMaskedEdit.UltraMaskedEdit)sender;
        //        switch (txtEditor.Name)
        //        {
        //            case "txtZipCode":
        //                oCustomerRow.Zip = txtZipCode.Text;
        //                break;
        //            case "txtFaxNo":
        //                oCustomerRow.FaxNo = txtFaxNo.Value.ToString();
        //                break;
        //            case "txtPhoneOff":
        //                oCustomerRow.PhoneOffice = txtPhoneOff.Value.ToString();
        //                break;
        //            case "txtPhoneHome":
        //                oCustomerRow.PhoneHome = txtPhoneHome.Value.ToString();
        //                break;
        //            case "txtCellNo":
        //                oCustomerRow.CellNo = txtCellNo.Value.ToString();
        //                break;
        //        }
        //    }
        //    catch (Exception) { }
        //}

        //private void txtDiscount_Validating(object sender, CancelEventArgs e)
        //{
        //    try
        //    {
        //        oCustomerRow.Discount = Configuration.convertNullToDecimal(this.txtDiscount.Value.ToString());
        //    }
        //    catch (Exception exp)
        //    {
        //        clsUIHelper.ShowErrorMsg(exp.Message);
        //    }
        //}
        //private void chkIsActive_CheckedChanged(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        if (oCustomerRow == null)
        //            return;
        //        oCustomerRow.IsActive = chkIsActive.Checked;
        //    }
        //    catch (Exception exp)
        //    {
        //        clsUIHelper.ShowErrorMsg(exp.Message);
        //    }
        //}

        //private void chkUseForCustomerLoyalty_CheckedChanged(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        if (oCustomerRow == null)
        //            return;
        //        oCustomerRow.UseForCustomerLoyalty = chkUseForCustomerLoyalty.Checked;
        //    }
        //    catch (Exception exp)
        //    {
        //        clsUIHelper.ShowErrorMsg(exp.Message);
        //    }
        //}
        //#region Sprint-23 - PRIMEPOS-2314 09-Jun-2016 JY Added
        //private void chkSaveCardProfile_CheckedChanged(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        if (oCustomerRow == null)
        //            return;
        //        //oCustomerRow.SaveCardProfile = chkSaveCardProfile.Checked;
        //    }
        //    catch (Exception exp)
        //    {
        //        clsUIHelper.ShowErrorMsg(exp.Message);
        //    }
        //}
        //#endregion
        #endregion

        #region PRIMEPOS-2613 11-Dec-2018 JY Added
        private void btnEmail_Click(object sender, EventArgs e)
        {
            if (Configuration.convertNullToString(oCustomerRow.Email) == string.Empty)
            {
                Resources.Message.Display("Please update email details for this customer", "Token expiration details", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (Configuration.convertNullToString(Configuration.CInfo.OwnersEmailId) == string.Empty)
            {
                Resources.Message.Display("Please update senders email details", "Token expiration details", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                string strMsg = oBRCustomer.EmailReport(oCustomerRow.CustomerId);
                if (strMsg != string.Empty)
                {
                    if (Resources.Message.Display("Do you want to email below details to the customer" + Environment.NewLine + strMsg, "Token expiration details", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        strMsg = strMsg.Replace("\r\n", "<br/>");
                        new System.Threading.Thread(delegate ()
                        {
                            clsReports.EmailReport(null, Configuration.CInfo.OwnersEmailId, "Token expiration details", strMsg, "File");
                        }).Start();

                    }
                }
                else
                {
                    Resources.Message.Display("Token expiration details are not available.", "Token expiration details", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }
        #endregion

        private void grdCCProfile_BeforeCellDeactivate(object sender, CancelEventArgs e)
        {
        }

        private void grdCCProfile_Enter(object sender, EventArgs e)
        {
            try
            {
                if (this.grdCCProfile.Rows.Count > 0)
                {
                    this.grdCCProfile.PerformAction(UltraGridAction.EnterEditMode);
                }
            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }

        private void frmCustomers_Shown(object sender, EventArgs e)
        {
            this.grdCCProfile.PerformAction(UltraGridAction.EnterEditMode);
            if (IsTokenize)//PRIMEPOS-2896
            {
                btnAddCard_Click(null, null);
                IsTokenize = false;
            }
        }

        #region PRIMEPOS-2634 31-Jan-2019 JY Added
        private void btnSaveCCProfile_Click(object sender, EventArgs e)
        {
            try
            {
                logger.Trace("btnSaveCCProfile_Click() - " + clsPOSDBConstants.Log_Entering);
                if (grdCCProfile.Rows.Count < 1)
                    return;

                if (ValidatePreferences() == true)
                {
                    Resources.Message.Display("Preferred card preference should be assigned to more than a single card.", Configuration.ApplicationName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                //get the data in list and send for update
                CCCustomerTokInfo oCCCustomerTokInfo = new CCCustomerTokInfo();
                oCCCustomerTokInfo.Persist(oCCCustomerTokInfoData);
                logger.Trace("btnSaveCCProfile_Click() - " + clsPOSDBConstants.Log_Exiting);
            }
            catch (Exception Ex)
            {
                logger.Fatal(Ex, "btnSaveCCProfile_Click()");
                clsUIHelper.ShowErrorMsg(Ex.Message);
            }
        }

        private Boolean ValidatePreferences()
        {
            bool bReturn = false;
            int nPreferredCards = 0;
            for (int i = 0; i < grdCCProfile.Rows.Count; i++)
            {
                if (Configuration.convertNullToInt(grdCCProfile.Rows[i].Cells[clsPOSDBConstants.CCCustomerTokInfo__Fld_PreferenceId].Value) == 1)
                    nPreferredCards += 1;
            }

            if (nPreferredCards > 1)
                bReturn = true;

            return bReturn;
        }
        #endregion

        #region Customer Performance - NileshJ - PRIMEPOS-2655 - 06-Sept-2019
        public void PopulateCustomerPerformance(int customerID)
        {
            try
            {
                dsCustomerInfo = custperfom.getCustPerformData(customerID, dtFrom.DateTime.Date, dtTo.DateTime.Date);
                dtTransaction = dsCustomerInfo.Tables[0];
                dtItemDetails = dsCustomerInfo.Tables[1];
                dtDepartmentDetails = dsCustomerInfo.Tables[2];
                dtGraph = dsCustomerInfo.Tables[3];

                ShowTransaction();
                ShowItem();
                ShowDepartment();
                ShowSummary();
                ShowGraph();
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
            }
        }

        private void btnView_Click(object sender, EventArgs e)
        {

            if (isValidDate())
            {
                // this.lblWait.Visible = true;
                PopulateCustomerPerformance(oCustomerRow.CustomerId);
            }
        }

        private bool isValidDate()
        {

            this.errorProvider1.SetError(dtFrom, "");
            this.errorProvider1.SetError(dtTo, "");
            bool isValid = false;
            if (MMSUtil.UtilFunc.IsDateValid(dtFrom.Value) && MMSUtil.UtilFunc.IsDateValid(dtTo.Value))
            {
                if (dtFrom.DateTime.Date <= dtTo.DateTime.Date)
                    isValid = true;
                else if (dtFrom.DateTime.Date > dtTo.DateTime.Date)
                {
                    this.errorProvider1.SetError(dtFrom, " 'From' Date cannot be greater than 'To' Date");
                    this.errorProvider1.SetIconAlignment(dtFrom, ErrorIconAlignment.MiddleRight);
                }
            }
            else if (!MMSUtil.UtilFunc.IsDateValid(dtFrom.Value) && MMSUtil.UtilFunc.IsDateValid(dtTo.Value))
            {
                this.errorProvider1.SetError(dtFrom, "Correct format is required");
                this.errorProvider1.SetIconAlignment(dtFrom, ErrorIconAlignment.MiddleRight);
            }
            else if (MMSUtil.UtilFunc.IsDateValid(dtFrom.Value) && !MMSUtil.UtilFunc.IsDateValid(dtTo.Value))
            {
                this.errorProvider1.SetError(dtTo, "Correct format is required");
                this.errorProvider1.SetIconAlignment(dtTo, ErrorIconAlignment.MiddleRight);
            }
            else if (!MMSUtil.UtilFunc.IsDateValid(dtFrom.Value) && !MMSUtil.UtilFunc.IsDateValid(dtTo.Value))
            {
                this.errorProvider1.SetError(dtFrom, "Correct format is required");
                this.errorProvider1.SetIconAlignment(dtFrom, ErrorIconAlignment.MiddleRight);
                this.errorProvider1.SetError(dtTo, "Correct format is required");
                this.errorProvider1.SetIconAlignment(dtTo, ErrorIconAlignment.MiddleRight);
            }
            return isValid;
        }

        private void ShowTransaction()
        {
            grdTransaction.DataSource = dtTransaction;

            grdTransaction.DisplayLayout.Bands[0].Columns["TRANSACTIONTYPE"].Header.Caption = "Transaction Type";
            grdTransaction.DisplayLayout.Bands[0].Columns["TRANSACTIONTYPE"].Header.Appearance.TextHAlign = Infragistics.Win.HAlign.Center;
            grdTransaction.DisplayLayout.Bands[0].Columns["TRANSACTIONTYPE"].CellAppearance.TextHAlign = Infragistics.Win.HAlign.Left;

            grdTransaction.DisplayLayout.Bands[0].Columns["TOTALTRANSACTION"].Header.Caption = "Total Transaction";
            grdTransaction.DisplayLayout.Bands[0].Columns["TOTALTRANSACTION"].Header.Appearance.TextHAlign = Infragistics.Win.HAlign.Center;
            grdTransaction.DisplayLayout.Bands[0].Columns["TOTALTRANSACTION"].CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right;
            //grdTransaction.DisplayLayout.Bands[0].Columns["TOTALTRANSACTION"].Width = 70;

            grdTransaction.DisplayLayout.Bands[0].Columns["TOTALAMOUNT"].Format = "$00.00";
            grdTransaction.DisplayLayout.Bands[0].Columns["TOTALAMOUNT"].Header.Caption = "Total Amount";
            grdTransaction.DisplayLayout.Bands[0].Columns["TOTALAMOUNT"].Header.Appearance.TextHAlign = Infragistics.Win.HAlign.Center;
            grdTransaction.DisplayLayout.Bands[0].Columns["TOTALAMOUNT"].CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right;

            grdTransaction.DisplayLayout.Bands[0].Columns["PROFIT"].Format = "$00.00";
            grdTransaction.DisplayLayout.Bands[0].Columns["PROFIT"].Header.Caption = "Profit";
            grdTransaction.DisplayLayout.Bands[0].Columns["PROFIT"].Header.Appearance.TextHAlign = Infragistics.Win.HAlign.Center;
            grdTransaction.DisplayLayout.Bands[0].Columns["PROFIT"].CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right;
        }

        private void ShowItem()
        {
            grdItem.DataSource = dtItemDetails;

            grdItem.DisplayLayout.Bands[0].Columns["Itemcode"].Header.Caption = "Item Code";
            grdItem.DisplayLayout.Bands[0].Columns["Itemcode"].Header.Appearance.TextHAlign = Infragistics.Win.HAlign.Center;
            grdItem.DisplayLayout.Bands[0].Columns["Itemcode"].CellAppearance.TextHAlign = Infragistics.Win.HAlign.Left;

            #region PRIMEPOS-3083 12-Apr-2022 JY Added
            grdItem.DisplayLayout.Bands[0].Columns["Description"].Header.Caption = "Item Description";
            grdItem.DisplayLayout.Bands[0].Columns["Description"].Header.Appearance.TextHAlign = Infragistics.Win.HAlign.Center;
            grdItem.DisplayLayout.Bands[0].Columns["Description"].CellAppearance.TextHAlign = Infragistics.Win.HAlign.Left;
            grdItem.DisplayLayout.Bands[0].Columns["Description"].Width = 110;

            grdItem.DisplayLayout.Bands[0].Columns["DeptCode"].Header.Caption = "Dept Code";
            grdItem.DisplayLayout.Bands[0].Columns["DeptCode"].Header.Appearance.TextHAlign = Infragistics.Win.HAlign.Center;
            grdItem.DisplayLayout.Bands[0].Columns["DeptCode"].CellAppearance.TextHAlign = Infragistics.Win.HAlign.Left;
            grdItem.DisplayLayout.Bands[0].Columns["DeptCode"].Width = 70;
            #endregion

            grdItem.DisplayLayout.Bands[0].Columns["DeptName"].Header.Caption = "Dept Name";
            grdItem.DisplayLayout.Bands[0].Columns["DeptName"].Header.Appearance.TextHAlign = Infragistics.Win.HAlign.Center;
            grdItem.DisplayLayout.Bands[0].Columns["DeptName"].CellAppearance.TextHAlign = Infragistics.Win.HAlign.Left;
            //grdItem.DisplayLayout.Bands[0].Columns["DeptName"].Width = 70;

            grdItem.DisplayLayout.Bands[0].Columns["TotalQty"].Header.Caption = "Qty";
            grdItem.DisplayLayout.Bands[0].Columns["TotalQty"].Header.Appearance.TextHAlign = Infragistics.Win.HAlign.Center;
            grdItem.DisplayLayout.Bands[0].Columns["TotalQty"].CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right;

            grdItem.DisplayLayout.Bands[0].Columns["TotalPrice"].Format = "$00.00";
            grdItem.DisplayLayout.Bands[0].Columns["TotalPrice"].Header.Caption = "Total Price";
            grdItem.DisplayLayout.Bands[0].Columns["TotalPrice"].Header.Appearance.TextHAlign = Infragistics.Win.HAlign.Center;
            grdItem.DisplayLayout.Bands[0].Columns["TotalPrice"].CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right;

            //grdItem.DisplayLayout.Bands[0].Columns["LastSellingDate"].Header.Caption = "Last Selling Date";
            //grdItem.DisplayLayout.Bands[0].Columns["LastSellingDate"].Header.Appearance.TextHAlign = Infragistics.Win.HAlign.Center;
            //grdItem.DisplayLayout.Bands[0].Columns["LastSellingDate"].CellAppearance.TextHAlign = Infragistics.Win.HAlign.Left;
            grdItem.DisplayLayout.Bands[0].Columns["DepartmentID"].Header.Column.Hidden = true;   //PRIMEPOS-3083 12-Apr-2022 JY Added
        }

        private void ShowDepartment()
        {
            grdDepartment.DataSource = dtDepartmentDetails;

            grdDepartment.DisplayLayout.Bands[0].Columns["Deptcode"].Header.Caption = "Dept Code";
            grdDepartment.DisplayLayout.Bands[0].Columns["Deptcode"].Header.Appearance.TextHAlign = Infragistics.Win.HAlign.Center;
            grdDepartment.DisplayLayout.Bands[0].Columns["Deptcode"].CellAppearance.TextHAlign = Infragistics.Win.HAlign.Left;
            grdDepartment.DisplayLayout.Bands[0].Columns["DeptCode"].Width = 70;

            grdDepartment.DisplayLayout.Bands[0].Columns["Deptname"].Header.Caption = "Dept Name";
            grdDepartment.DisplayLayout.Bands[0].Columns["Deptname"].Header.Appearance.TextHAlign = Infragistics.Win.HAlign.Center;
            grdDepartment.DisplayLayout.Bands[0].Columns["Deptname"].CellAppearance.TextHAlign = Infragistics.Win.HAlign.Left;
            grdDepartment.DisplayLayout.Bands[0].Columns["Deptname"].Width = 110;

            grdDepartment.DisplayLayout.Bands[0].Columns["TotalQTY"].Header.Caption = "Qty";
            grdDepartment.DisplayLayout.Bands[0].Columns["TotalQTY"].Header.Appearance.TextHAlign = Infragistics.Win.HAlign.Center;
            grdDepartment.DisplayLayout.Bands[0].Columns["TotalQTY"].CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right;

            grdDepartment.DisplayLayout.Bands[0].Columns["TotalSalePrice"].Format = "$00.00";
            grdDepartment.DisplayLayout.Bands[0].Columns["TotalSalePrice"].Header.Caption = "Total Sale Price";
            grdDepartment.DisplayLayout.Bands[0].Columns["TotalSalePrice"].Header.Appearance.TextHAlign = Infragistics.Win.HAlign.Center;
            grdDepartment.DisplayLayout.Bands[0].Columns["TotalSalePrice"].CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right;
        }

        private void ShowSummary()
        {
            if (dsCustomerInfo.Tables.Count > 0)
            {
                long totItems = dtItemDetails.Rows.Count;
                double totRevGen = Convert.ToDouble(dtTransaction.Rows[1]["TOTALAMOUNT"]);
                double totProfit = Convert.ToDouble(dtTransaction.Rows[1]["PROFIT"]);

                int daysDiff = 0;
                DateTime patRegDt = Convert.ToDateTime(dtTransaction.Rows[1]["TransDate"]);
                if (patRegDt.Date > dtFrom.DateTime.Date)
                    daysDiff = (dtTo.DateTime.Date - patRegDt.Date).Days;
                else
                    daysDiff = (dtTo.DateTime.Date - dtFrom.DateTime.Date).Days;


                if (daysDiff < 30)
                {
                    lblAvgItem.Text = totItems.ToString();
                    lblAvgRevGen.Text = "$" + Math.Round(totRevGen, 2).ToString();
                    lblAvgProfit.Text = "$" + Math.Round(totProfit, 2).ToString();
                }
                else
                {

                    lblAvgItem.Text = Math.Round(Convert.ToDouble(totItems) / Math.Round((daysDiff / 30.00), 2), 2).ToString();
                    lblAvgRevGen.Text = "$" + Math.Round(totRevGen / Math.Round((daysDiff / 30.00), 2), 2).ToString();
                    lblAvgProfit.Text = "$" + Math.Round(totProfit / Math.Round((daysDiff / 30.00), 2), 2).ToString();
                }
            }
            else
            {
                lblAvgItem.Text = "0.00";
                lblAvgRevGen.Text = "$0.00";
                lblAvgProfit.Text = "$0.00";
            }
        }
        private void ShowGraph()
        {

            if (dtGraph.Rows.Count > 0)
            {
                ucSummary.Axis.Y.Labels.ItemFormatString = "<DATA_VALUE:$00.00>";
                double maxRecv = Convert.ToDouble(dtGraph.Compute("max(SALE)", string.Empty).ToString());
                double maxDisp = Convert.ToDouble(dtGraph.Compute("max(RETURN)", string.Empty).ToString());
                double maxRange = (maxRecv > maxDisp) ? maxRecv : maxDisp;
                ucSummary.Axis.Y.RangeMax = maxRange + 5;
                ucSummary.Axis.Y.RangeType = Infragistics.UltraChart.Shared.Styles.AxisRangeType.Custom;
                ucSummary.Axis.Y.RangeMin = 0;
                ucSummary.Axis.Y.TickmarkPercentage = 20.00;
                ucSummary.Axis.Y.TickmarkIntervalType = Infragistics.UltraChart.Shared.Styles.AxisIntervalType.Ticks;

                //ucSummary.Axis.X.RangeType = Infragistics.UltraChart.Shared.Styles.AxisRangeType.Custom;
                //ucSummary.Axis.X.RangeMin = 0;
                //ucSummary.Axis.X.TickmarkPercentage = 20.00;
                //ucSummary.Axis.X.TickmarkIntervalType = Infragistics.UltraChart.Shared.Styles.AxisIntervalType.Ticks;

                //DateTime maxDate =Convert.ToDateTime( dsCustomerInfo.Tables[3].Compute("max(DATE)", string.Empty).ToString());
                //DateTime minDate = Convert.ToDateTime(dsCustomerInfo.Tables[3].Compute("min(DATE)", string.Empty).ToString());
                //DateTime maxDateRange = (maxDate > minDate) ? maxDate : minDate;

                ucSummary.Refresh();
                ucSummary.DataSource = dtGraph;
                ucSummary.DataBind();


            }
            else
            {
                ucSummary.DataSource = null;
                ucSummary.DataBind();
            }
        }
        #endregion

        #region PRIMEPOS-2747 - Store Credit
        public void PopulateCustomerStoreTransactionDetails(int customerID)
        {
            try
            {
                dsStoreData = oStoreCredit.GetByCustomerID(customerID);
                if (dsStoreData.Tables.Count > 0)
                {
                    dtStoreData = dsStoreData.Tables[0];

                    if (dtStoreData.Rows.Count > 0)
                    {
                        txtStoreCredit.Text = Convert.ToDecimal(dtStoreData.Rows[0]["CreditAmt"]).ToString("0.00");
                    }
                }

                dsStoreDetails = oStoreCreditDetails.GetByCustomerID(customerID);

                if (dsStoreDetails.Tables.Count > 0)
                {
                    dtStoreDetails = dsStoreDetails.Tables[0];

                    if (dtStoreDetails.Rows.Count > 0)
                    {
                        StoreDetailsGridBind();
                    }
                }
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
            }
        }

        public void StoreDetailsGridBind()
        {
            grdStoreDetails.DataSource = dtStoreDetails;

            grdStoreDetails.DisplayLayout.Bands[0].Columns["TransactionID"].Header.Caption = "Transaction ID";
            grdStoreDetails.DisplayLayout.Bands[0].Columns["TransactionID"].Header.Appearance.TextHAlign = Infragistics.Win.HAlign.Center;
            grdStoreDetails.DisplayLayout.Bands[0].Columns["TransactionID"].CellAppearance.TextHAlign = Infragistics.Win.HAlign.Center;

            grdStoreDetails.DisplayLayout.Bands[0].Columns["CreditAmt"].Format = "$00.00";
            grdStoreDetails.DisplayLayout.Bands[0].Columns["CreditAmt"].Header.Caption = "Trans. Credit Amount";
            grdStoreDetails.DisplayLayout.Bands[0].Columns["CreditAmt"].Header.Appearance.TextHAlign = Infragistics.Win.HAlign.Center;
            grdStoreDetails.DisplayLayout.Bands[0].Columns["CreditAmt"].CellAppearance.TextHAlign = Infragistics.Win.HAlign.Center;

            grdStoreDetails.DisplayLayout.Bands[0].Columns["RemainingAmount"].Format = "$00.00";
            grdStoreDetails.DisplayLayout.Bands[0].Columns["RemainingAmount"].Header.Caption = "Total Store Credit";
            grdStoreDetails.DisplayLayout.Bands[0].Columns["RemainingAmount"].Header.Appearance.TextHAlign = Infragistics.Win.HAlign.Center;
            grdStoreDetails.DisplayLayout.Bands[0].Columns["RemainingAmount"].CellAppearance.TextHAlign = Infragistics.Win.HAlign.Center;


            grdStoreDetails.DisplayLayout.Bands[0].Columns["StoreCreditDetailsID"].Hidden = true;
            grdStoreDetails.DisplayLayout.Bands[0].Columns["StoreCreditID"].Hidden = true;
            grdStoreDetails.DisplayLayout.Bands[0].Columns["CustomerID"].Hidden = true;
            grdStoreDetails.DisplayLayout.Bands[0].Columns["UpdatedOn"].Hidden = true;
            grdStoreDetails.DisplayLayout.Bands[0].Columns["UpdatedBy"].Hidden = true;

        }
        #endregion

        #region //PRIMEPOS-2896 Arvind
        private void btnAddCard_Click(object sender, EventArgs e)
        {
            if (UserPriviliges.getPermission(UserPriviliges.Modules.POSTransaction.ID, UserPriviliges.Screens.POSTransaction.ID, UserPriviliges.Permissions.AllowTokenizationFromCustomerFile.ID, UserPriviliges.Permissions.AllowTokenizationFromCustomerFile.Name))
            {
                POSTransaction pOSTransaction = new POSTransaction();

                try
                {
                    if (Configuration.CSetting.OnlinePayment)
                    {
                        Configuration.isPrimeRxPay = true;
                        Dictionary<string, string> fields = new Dictionary<string, string>();
                        fields.Add("PHARMACYNO", Configuration.CSetting.PharmacyNPI);//PRIMEPOS-2902
                        fields.Add("AMOUNT", "0");
                        fields.Add("TICKETNUMBER", Configuration.StationID + clsUIHelper.GetRandomNo().ToString());
                        fields.Add("URL", Configuration.CSetting.PrimeRxPayUrl + Configuration.CSetting.PrimerxPayExtensionUrl);//2956
                        fields.Add("PASSWORD", Configuration.CSetting.PrimeRxPaySecretKey);
                        fields.Add("APIKEY", Configuration.CSetting.PrimeRxPayClientId);
                        fields.Add("PAYPROVIDERID", Configuration.CSetting.PayProviderID.ToString());//PRIMEPOS-2902
                        fields.Add("ONLYTOKEN","True");
                        fields.Add("LINKEXPIRY", Configuration.CSetting.LinkExpriyInMinutes);//PRIMEPOS-3134
                        #region PRIMEPOS-3455
                        if (Configuration.CPOSSet.IsSecureDevice)
                        {
                            fields.Add("ISSECUREDEVICE", Convert.ToString(Configuration.CPOSSet.IsSecureDevice));
                            fields.Add("TERMINALMODEL", Configuration.CPOSSet.SecureDeviceModel);
                            fields.Add("TERMINALSRNUMBER", Configuration.CPOSSet.SecureDeviceSrNumber);
                        }
                        #endregion
                        PrimeRxPayProcessor primeRxPayProcessor = PrimeRxPayProcessor.GetInstance();
                        PrimeRxPayResponse primeRxPayResponse = primeRxPayProcessor.Sale(fields);

                        if (primeRxPayResponse.Result.ToUpper() != "SUCCESS")
                            Resources.Message.Display(primeRxPayResponse.Result);
                        else if(!string.IsNullOrWhiteSpace(primeRxPayResponse.CardType))
                            pOSTransaction.SaveOnlyCCToken(oCustomerRow.CustomerId, primeRxPayResponse.CardType, primeRxPayResponse.MaskedCardNo, primeRxPayResponse.ProfiledID, primeRxPayResponse.EntryMethod, Configuration.CSetting.PayProviderName, primeRxPayResponse.request, primeRxPayResponse.response,primeRxPayResponse.Expiration);//PRIMEPOS-2902

                        Configuration.isPrimeRxPay = false;
                    }
                }
                catch (Exception ex)
                {
                    logger.Error("ERROR IN ADD CARD METHOD : " + ex.Message);
                }
                if (IsTokenize)
                {
                    tabCustomerInfo.ActiveTab = tabCustomerInfo.Tabs[2];
                    tabCustomerInfo.ActiveTab.Selected = true;
                    this.ActiveControl = txtCustomerCode;
                }
                PopulateCreditCardProfiles(oCustomerRow.CustomerId);
            }
        }
        #endregion
    }
}

