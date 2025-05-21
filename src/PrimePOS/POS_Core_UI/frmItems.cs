using System;
using System.Collections.Generic;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using POS_Core.CommonData.Rows;
using POS_Core.CommonData;
using POS_Core.BusinessRules;
using POS_Core.ErrorLogging;
using System.Diagnostics;
using Infragistics.Win;
using Infragistics.Win.UltraWinTabs;
using Infragistics.Win.UltraWinTabControl;
using Infragistics.Win.UltraWinGrid;    //Added By Amit Date 2 Dec 2011
using System.Data;
//using POS_Core.DataAccess;
using POS_Core.DataAccess;
using System.Globalization;
using POS_Core.Resources;
using NLog;

namespace POS_Core_UI
{
    /// <summary>
    /// Summary description for frmItems.
    /// </summary>
    public class frmItems : System.Windows.Forms.Form
    {
        #region declarations
        public bool IsCanceled = true;
        string errorMsg = "";//Added By Shitaljit(QuicSolv) on 29 june 2011
        bool ShowEntityNotes = false;
        public bool AllowEdit = true;
        public bool isItemVendorSave = false;
        private bool isItemEditMode = false;  //Added by Amit Date 30 Nov 2011

        private ItemData oItemData = new ItemData();
        private ItemRow oItemRow;
        private Item oBRItem = new Item();

        private DepartmentData oDepartmentData = new DepartmentData();
        private DepartmentRow oDepartmentRow;
        private Department oBRDepartment = new Department();

        private SubDepartmentData oSubDepartmentData = new SubDepartmentData();
        private SubDepartmentRow oSubDepartmentRow;
        private SubDepartment oSubBRDepartment = new SubDepartment();

        private TaxCodesData oTaxcodeData = new TaxCodesData();
        private TaxCodesRow oTaxCodesRow;
        private TaxCodes oBRTaxCodes = new TaxCodes();

        private VendorData oVendorData = new VendorData();
        private VendorRow oVendorRow;

        //Added By Amit Date 2 Dec 2011
        private ItemVendor oItemVendor = new ItemVendor();
        private ItemVendorData oItemVendorData = new ItemVendorData();

        private bool m_FromError = false;
        private string mItemId = "";
        private int m_rowIndex = -1;
        private string m_VendorCode;
        private int m_LastCell;
        private bool m_IsCellUpdateCalled = false;
        public string ControlToFocusOnLoad = string.Empty;

        //End

        private POS_Core.BusinessRules.Vendor oBRVendor = new POS_Core.BusinessRules.Vendor();
        private Infragistics.Win.Misc.UltraLabel lblTransactionType;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.GroupBox groupBox3;
        private Infragistics.Win.Misc.UltraLabel ultraLabel13;
        private Infragistics.Win.Misc.UltraLabel ultraLabel14;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor txtUnit;
        private Infragistics.Win.UltraWinEditors.UltraNumericEditor numFreight;
        private Infragistics.Win.UltraWinEditors.UltraCheckEditor chkIsDiscountable;
        private Infragistics.Win.UltraWinEditors.UltraCheckEditor chkIsTaxable;
        public Infragistics.Win.UltraWinEditors.UltraNumericEditor numLastCostPrice;
        private Infragistics.Win.UltraWinEditors.UltraNumericEditor numAveragePrice;
        public Infragistics.Win.UltraWinEditors.UltraNumericEditor numSellingPrice;
        private Infragistics.Win.UltraWinEditors.UltraNumericEditor numDiscountAmt;
        private Infragistics.Win.Misc.UltraLabel ultraLabel11;
        private Infragistics.Win.Misc.UltraLabel ultraLabel8;
        private Infragistics.Win.Misc.UltraLabel ultraLabel10;
        private System.Windows.Forms.GroupBox groupBox2;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor txtDepartmentCode;
        private Infragistics.Win.UltraWinEditors.UltraComboEditor cboItemType;
        private Infragistics.Win.Misc.UltraLabel ultraLabel9;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor txtSaleTypeCode;
        private Infragistics.Win.Misc.UltraLabel ultraLabel7;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor txtSeasonCode;
        private Infragistics.Win.Misc.UltraLabel txtDepartmentDescription;
        private Infragistics.Win.Misc.UltraLabel ultraLabel5;
        private Infragistics.Win.Misc.UltraLabel ultraLabel3;
        private Infragistics.Win.Misc.UltraLabel ultraLabel4;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor txtProductCode;
        private Infragistics.Win.Misc.UltraLabel ultraLabel2;
        private Infragistics.Win.Misc.UltraLabel ultraLabel1;
        public Infragistics.Win.UltraWinEditors.UltraTextEditor txtItemCode;
        private System.Windows.Forms.GroupBox groupBox6;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor txtLastVendor;
        private Infragistics.Win.Misc.UltraLabel ultraLabel28;
        private Infragistics.Win.Misc.UltraLabel txtVendorName;
        private Infragistics.Win.Misc.UltraLabel ultraLabel31;
        private Infragistics.Win.Misc.UltraLabel ultraLabel32;
        private Infragistics.Win.Misc.UltraLabel ultraLabel33;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor txtRemarks;
        private Infragistics.Win.UltraWinSchedule.UltraCalendarCombo dtpLastRecvDate;
        private Infragistics.Win.UltraWinSchedule.UltraCalendarCombo dtpLastSaleDate;
        private System.Windows.Forms.GroupBox groupBox5;
        private Infragistics.Win.UltraWinEditors.UltraNumericEditor numMinQty;
        public Infragistics.Win.UltraWinEditors.UltraNumericEditor numQtyInStock;
        private Infragistics.Win.UltraWinEditors.UltraNumericEditor numQtyOnOrder;
        private Infragistics.Win.UltraWinEditors.UltraNumericEditor numReorderLevel;
        private Infragistics.Win.UltraWinSchedule.UltraCalendarCombo dtpExpectedDelDate;
        private Infragistics.Win.Misc.UltraLabel ultraLabel27;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor txtLocation;
        private Infragistics.Win.Misc.UltraLabel ultraLabel22;
        private Infragistics.Win.Misc.UltraLabel ultraLabel25;
        private Infragistics.Win.Misc.UltraLabel ultraLabel24;
        private Infragistics.Win.Misc.UltraLabel ultraLabel23;
        private Infragistics.Win.Misc.UltraLabel ultraLabel26;
        private Infragistics.Win.Misc.UltraButton btnClose;
        private Infragistics.Win.Misc.UltraButton btnGrpPricing;
        private Infragistics.Win.Misc.UltraButton btnCompanionItem;
        private Infragistics.Win.UltraWinSchedule.UltraCalendarCombo dtpSaleStartDate;
        private Infragistics.Win.UltraWinSchedule.UltraCalendarCombo dtpSaleEndDate;
        private Infragistics.Win.UltraWinEditors.UltraNumericEditor numSalePrice;
        private Infragistics.Win.Misc.UltraLabel ultraLabel6;
        private Infragistics.Win.Misc.UltraLabel ultraLabel12;
        private Infragistics.Win.Misc.UltraLabel ultraLabel15;
        private System.Windows.Forms.GroupBox faOnSale;
        private Infragistics.Win.Misc.UltraButton btnOk;
        private Infragistics.Win.Misc.UltraButton btnSave;
        private Infragistics.Win.Misc.UltraLabel ultraLabel18;
        private Infragistics.Win.UltraWinEditors.UltraCheckEditor chkExclFromAutoPO;
        private Infragistics.Win.UltraWinEditors.UltraCheckEditor chkExclFromRecpt;
        private Infragistics.Win.Misc.UltraButton btnInventoryHistory;
        private Infragistics.Win.Misc.UltraButton btnPriceValidation;
        private Infragistics.Win.UltraWinEditors.UltraCheckEditor chkIsOTCItem;
        private Infragistics.Win.UltraWinEditors.UltraCheckEditor chkUpdatePriceItemWise;
        private Infragistics.Win.Misc.UltraButton btnItemMonitorCategory;
        private Infragistics.Win.Misc.UltraLabel ultraLabelPreferredVendor;
        public Infragistics.Win.UltraWinEditors.UltraComboEditor combEditorPrefVendor;
        private Infragistics.Win.Misc.UltraLabel ultraLblLastVendorLabel;
        private GroupBox groupBox7;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor txtSubDepartment;
        private Infragistics.Win.Misc.UltraLabel lblSubDepartment;
        private Infragistics.Win.Misc.UltraLabel ultraLabel20;
        private GroupBox grpItemPacketInfo;
        private Infragistics.Win.Misc.UltraLabel lblPacketUnit;
        private Infragistics.Win.Misc.UltraLabel lblPacketQuantity;
        private Infragistics.Win.Misc.UltraLabel lblPacketSize;
        public Infragistics.Win.UltraWinEditors.UltraTextEditor txtPacketUnit;
        public Infragistics.Win.UltraWinEditors.UltraTextEditor txtPacketSize;
        public Infragistics.Win.UltraWinEditors.UltraTextEditor txtPacketQuantity;
        private Infragistics.Win.UltraWinEditors.UltraCheckEditor chkPacketSize;
        private Infragistics.Win.Misc.UltraButton btnInventoryReceived;
        private Infragistics.Win.UltraWinEditors.UltraCheckEditor chkIsEBT;
        public Infragistics.Win.UltraWinEditors.UltraComboEditor cmbTaxPolicy;
        private Infragistics.Win.Misc.UltraLabel ultraLabel19;
        private UltraTabControl tabItemInformation;
        private UltraTabSharedControlsPage ultraTabSharedControlsPage1;
        private UltraTabPageControl ultraTabPageControl1;
        private UltraTabPageControl ultraTabPageControl2;
        private GroupBox groupBox4;
        private Infragistics.Win.Misc.UltraButton btnItemNote;
        private Infragistics.Win.UltraWinEditors.UltraCheckEditor chkIsOnSale;
        private Infragistics.Win.UltraWinGrid.UltraGrid grdDetail;
        private GroupBox grbVendorInformation;
        private Infragistics.Win.Misc.UltraButton btnSaveItemVendor;
        private UltraTabPageControl ultraTabPageControl3;
        private GroupBox groupBox8;
        private UltraGrid grdPriceChangeLog;
        private UltraTabPageControl ultraTabPageControl4;
        private GroupBox groupBox9;
        private UltraGrid grdPhysicalInvView;
        private UltraTabPageControl ultraTabPageControl5;
        private TextBox txtItemDesc;
        private Infragistics.Win.Misc.UltraButton btnDeleteItemVendorInfo;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor txtManufacturerName;
        private Infragistics.Win.Misc.UltraLabel ultraLabel16;
        public Infragistics.Win.UltraWinEditors.UltraNumericEditor numSaleLimitQty;
        private Infragistics.Win.Misc.UltraLabel ultraLabel17;
        public Infragistics.Win.UltraWinEditors.UltraComboEditor cmbDiscountPolicy;
        private Infragistics.Win.Misc.UltraLabel ultraLabel21;
        private Infragistics.Win.Misc.UltraButton btnAddSecDesc;
        public Infragistics.Win.UltraWinEditors.UltraTextEditor txtDescription;
        private Infragistics.Win.UltraWinEditors.UltraComboEditor cboTaxCodes;
        private Infragistics.Win.UltraWinEditors.UltraNumericEditor numCLPointsPerDollar;
        public Infragistics.Win.UltraWinEditors.UltraComboEditor cmbCLPointPolicy;
        private Infragistics.Win.Misc.UltraLabel ultraLabel29;
        private Infragistics.Win.Misc.UltraLabel ultraLabel30;
        private Infragistics.Win.UltraWinEditors.UltraCheckEditor chkIsDefaultCLPoints;
        private Infragistics.Win.UltraWinSchedule.UltraCalendarCombo dtpExpiryDate;
        private Infragistics.Win.Misc.UltraLabel lblExpiryDate;
        private Infragistics.Win.UltraWinEditors.UltraCheckEditor chkIsActive;
        private Infragistics.Win.Misc.UltraButton btnCopyContents;
        // private Infragistics.Win.UltraWinGrid.UltraGrid grdDetail;
        private System.ComponentModel.IContainer components;
        private Infragistics.Win.Misc.UltraPanel pnlClose;
        private Infragistics.Win.Misc.UltraLabel lblClose;
        private Infragistics.Win.Misc.UltraPanel pnlInventoryReceived;
        private Infragistics.Win.Misc.UltraLabel lblInventoryReceived;
        private Infragistics.Win.Misc.UltraPanel pnlGrpPricing;
        private Infragistics.Win.Misc.UltraLabel lblGrpPricing;
        private Infragistics.Win.Misc.UltraPanel pnlOk;
        private Infragistics.Win.Misc.UltraLabel lblOk;
        private Infragistics.Win.Misc.UltraPanel pnlItemNote;
        private Infragistics.Win.Misc.UltraLabel lblItemNote;
        private Infragistics.Win.Misc.UltraPanel pnlCompanionItem;
        private Infragistics.Win.Misc.UltraLabel lblCompanionItem;
        private Infragistics.Win.Misc.UltraPanel pnlAddSecDesc;
        private Infragistics.Win.Misc.UltraLabel lblAddSecDesc;
        private Infragistics.Win.Misc.UltraPanel pnlItemMonitorCategory;
        private Infragistics.Win.Misc.UltraLabel lblItemMonitorCategory;
        private Infragistics.Win.Misc.UltraPanel pnlDeleteItemVendorInfo;
        private Infragistics.Win.Misc.UltraLabel lblDeleteItemVendorInfo;
        private Infragistics.Win.Misc.UltraPanel pnlSaveItemVendor;
        private Infragistics.Win.Misc.UltraLabel lblSaveItemVendor;
        private Infragistics.Win.UltraWinEditors.UltraCheckEditor chkNonRefundable;
        private Infragistics.Win.Misc.UltraButton btnMMSSearch;
        private Infragistics.Win.Misc.UltraButton btnRefreshDataFromMMS;
        private UltraTabPageControl ultraTabPageControl6;
        private TableLayoutPanel tableLayoutPanel1;
        private TableLayoutPanel tableLayoutPanel2;
        private TableLayoutPanel tableLayoutPanel3;
        private UltraGrid grdVendor;
        private Infragistics.Win.UltraWinChart.UltraChart ucSummary;
        private TableLayoutPanel tableLayoutPanel4;
        private FlowLayoutPanel flowLayoutPanel3;
        private Label label4;
        private Infragistics.Win.UltraWinEditors.UltraOptionSet opSummBy;
        private GroupBox groupBox1;
        private Button btnView;
        private Label label7;
        private Infragistics.Win.UltraWinEditors.UltraDateTimeEditor dtFrom;
        private Label label3;
        private Infragistics.Win.UltraWinEditors.UltraDateTimeEditor dtTo;
        private TableLayoutPanel tableLayoutPanel5;
        private TableLayoutPanel tableLayoutPanel6;
        private Label lblQtyInHand;
        private Label label12;
        private Label lblAvgProfitPerItem;
        private Label label10;
        private Label lblAvgRecvPerMonth;
        private Label label8;
        private Label lblAvgReturnPerMonth;
        private Label label5;
        private Label lblAvgSalePerMonth;
        private Label lblSale;
        private Label label1;
        bool bCopyRecord = false;   //Sprint-26 - PRIMEPOS-2417 07-Jul-2017 JY Added

        #region PRIMEPOS-2654 - NileshJ - 05-Sep-2019
        private DataSet dsItemInfo = new DataSet();
        ItemPerformance itemperfom = new ItemPerformance();
        public string ItemPerformCode = string.Empty;

        private DataTable dtSoldRecv = new DataTable();
        private DataTable dtCostSaleamt = new DataTable();
        private DataTable dtProfit = new DataTable();
        private DataTable dtReturn = new DataTable();

        private DataTable dtQtyInHand = new DataTable();
        private Infragistics.Win.Misc.UltraLabel lblActualTax;
        private Infragistics.Win.Misc.UltraLabel lblActualTaxToDisplay;
        private DataTable dtVendor = new DataTable();
        #endregion
        private Infragistics.Win.Misc.UltraLabel lblCaseCost;
        private Infragistics.Win.UltraWinEditors.UltraNumericEditor numCaseCost;
        private static ILogger logger = LogManager.GetCurrentClassLogger(); //PRIMEPOS-3037 13-Dec-2021 JY Added
        #endregion

        //Added By SRT(Gaurav) Date: 10-Jul-2009
        public ItemRow CurrentItem
        {
            get
            {
                return (oItemRow);
            }
        }
        //End Of Added By SRT(Gaurav)

        public frmItems()
        {
            InitializeComponent();
            try
            {
                Initialize();
                FillVendors();
            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
            }

            //
            // TODO: Add any constructor code after InitializeComponent call
            //
        }
        
        //Following constructor Added by Krishna on 07April 2011

        public frmItems(string ItemId)
        {
            InitializeComponent();
            try
            {
                Initialize();
                FillVendors();
                this.txtItemCode.Text = ItemId;
            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }
        //Till here added by krishna;

        #region PRIMEPOS-2701 02-Jul-2019 JY Added
        public frmItems(ItemM oItemM)
        {
            InitializeComponent();
            try
            {
                Initialize();
                FillVendors();
                this.txtItemCode.Text = oItemM.ItemID;
                this.txtDescription.Text = oItemM.Description;
                this.txtItemDesc.Text = oItemM.Description;
                if (Configuration.convertNullToString(oItemM.Itemtype) != "")
                {
                    if (Configuration.convertNullToInt(oItemM.Itemtype) >= 1 && Configuration.convertNullToInt(oItemM.Itemtype) <= 3)
                    {
                        this.cboItemType.SelectedIndex = Configuration.convertNullToInt(oItemM.Itemtype);
                    }
                }
                this.txtProductCode.Text = oItemM.ProductCode;
                this.txtSeasonCode.Text = oItemM.SeasonCode;
                this.txtUnit.Text = oItemM.Unit;
                this.numFreight.Value = Configuration.convertNullToDecimal(oItemM.Freight);
                //this.numSellingPrice.Value = Configuration.convertNullToDecimal(oItemM.SellingPrice); //should not populate selling price
                this.chkIsTaxable.Checked = oItemM.isTaxable;
                this.chkIsEBT.Checked = oItemM.IsEBTItem;
                this.txtManufacturerName.Text = oItemM.ManufacturerName;
                this.txtPacketSize.Text = oItemM.PCKSIZE;
                this.txtPacketQuantity.Text = oItemM.PCKQTY;
                this.txtPacketUnit.Text = oItemM.PCKUNIT;

                //update ItemRow as well
                if (oItemRow != null)
                {
                    oItemRow.ItemID = this.txtItemCode.Text;
                    oItemRow.Description = this.txtDescription.Text;
                    oItemRow.Description = this.txtDescription.Text;
                    if (this.cboItemType.Text.Trim() != string.Empty)
                        oItemRow.Itemtype = Configuration.convertNullToString(oItemM.Itemtype);
                    oItemRow.ProductCode = this.txtProductCode.Text;
                    oItemRow.SeasonCode = this.txtSeasonCode.Text = oItemM.SeasonCode;
                    oItemRow.Unit = this.txtUnit.Text;
                    oItemRow.Freight = Configuration.convertNullToDecimal(oItemM.Freight);
                    oItemRow.isTaxable = this.chkIsTaxable.Checked;
                    oItemRow.IsEBTItem = this.chkIsEBT.Checked;
                    oItemRow.ManufacturerName = this.txtManufacturerName.Text;
                    oItemRow.PckSize = this.txtPacketSize.Text;
                    oItemRow.PckQty = this.txtPacketQuantity.Text;
                    oItemRow.PckUnit = this.txtPacketUnit.Text;
                }
            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }
        #endregion

        //Added By shitaljit(QuicSolv)on 5 Dec 2011
        public frmItems(string ItemID, string vendorCode)
        {
            InitializeComponent();
            try
            {
                Initialize();
                FillVendors();
                this.txtItemCode.Text = ItemID;
                this.txtItemCode.Enabled = false;
                oVendorData = oBRVendor.Populate(vendorCode);
                if (oVendorData != null && oVendorData.Vendor.Count > 0)
                {
                    oVendorRow = oVendorData.Vendor[0];
                }
                else
                {
                    oVendorRow = null;
                }

                if (oVendorRow != null)
                {
                    txtLastVendor.Text = oVendorRow.Vendorcode;
                    txtVendorName.Text = oVendorRow.Vendorname;
                    oItemRow.LastVendor = oVendorRow.Vendorcode;
                }
            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }
        //END of added by shitaljit
        public void Initialize()
        {
            FillTaxCodeInfo();
            SetNew();
        }

        #region PRIMEPOS-2464 04-Mar-2020 JY Added
        private void SetVisibilityAndAccessOfCostPrice()
        {
            if (UserPriviliges.IsUserHasPriviliges(UserPriviliges.Modules.InventoryMgmt.ID, UserPriviliges.Screens.ItemFile.ID, UserPriviliges.Permissions.EditItemCost.ID))
            {
                numLastCostPrice.ReadOnly = false;
                numLastCostPrice.Appearance.ForeColor = System.Drawing.Color.Black;
                //if (UserPriviliges.IsUserHasPriviliges(UserPriviliges.Modules.InventoryMgmt.ID, UserPriviliges.Screens.ItemFile.ID, UserPriviliges.Permissions.DisplayItemCost.ID) == false)
                //{
                //    Item oItem = new Item();
                //    oItem.UpdateDisplayItemCost( UserPriviliges.Modules.InventoryMgmt.ID, UserPriviliges.Screens.ItemFile.ID, UserPriviliges.Permissions.DisplayItemCost.ID, true);
                //}
            }
            else
            {
                numLastCostPrice.ReadOnly = true;
                if (UserPriviliges.IsUserHasPriviliges(UserPriviliges.Modules.InventoryMgmt.ID, UserPriviliges.Screens.ItemFile.ID, UserPriviliges.Permissions.DisplayItemCost.ID) == false)
                    numLastCostPrice.Visible = false;
                else
                    numLastCostPrice.Visible = true;
            }
            #region PRIMEPOS-3045 12-Jan-2021 JY Added
            if (this.txtPacketUnit.Text.Trim().ToUpper() == "CS" || this.txtPacketUnit.Text.Trim().ToUpper() == "CA")
            {
                numCaseCost.ReadOnly = numLastCostPrice.ReadOnly;
                lblCaseCost.Visible = numCaseCost.Visible = numLastCostPrice.Visible;
            }
            #endregion
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
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance7 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance8 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance9 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance10 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance11 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance12 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance13 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance14 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance15 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance16 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinEditors.EditorButton editorButton1 = new Infragistics.Win.UltraWinEditors.EditorButton();
            Infragistics.Win.Appearance appearance17 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance18 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance19 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance20 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance21 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance22 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance23 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinEditors.EditorButton editorButton2 = new Infragistics.Win.UltraWinEditors.EditorButton();
            Infragistics.Win.Appearance appearance24 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance25 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance26 = new Infragistics.Win.Appearance();
            Infragistics.Win.ValueListItem valueListItem13 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem14 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem15 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.Appearance appearance27 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance28 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance29 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance30 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance31 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance32 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance33 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance34 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance35 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance36 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance37 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance38 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance39 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance40 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance41 = new Infragistics.Win.Appearance();
            Infragistics.Win.ValueListItem valueListItem7 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem8 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem9 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem12 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.Appearance appearance42 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance43 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance44 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance45 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance46 = new Infragistics.Win.Appearance();
            Infragistics.Win.ValueListItem valueListItem4 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem5 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.Appearance appearance47 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance48 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance49 = new Infragistics.Win.Appearance();
            Infragistics.Win.ValueListItem valueListItem6 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem10 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem11 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.Appearance appearance50 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance51 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance52 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance53 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance54 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance55 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance56 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance57 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance58 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance59 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance60 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance61 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance62 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance63 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance64 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance65 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinSchedule.CalendarCombo.DateButton dateButton1 = new Infragistics.Win.UltraWinSchedule.CalendarCombo.DateButton();
            Infragistics.Win.Appearance appearance66 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance67 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance68 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance69 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance70 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance71 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance72 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance73 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance74 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance75 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance76 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance77 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance78 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance79 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance80 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinSchedule.CalendarCombo.DateButton dateButton2 = new Infragistics.Win.UltraWinSchedule.CalendarCombo.DateButton();
            Infragistics.Win.Appearance appearance81 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance82 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance83 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance84 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance85 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance86 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance87 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance88 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance89 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinSchedule.CalendarCombo.DateButton dateButton3 = new Infragistics.Win.UltraWinSchedule.CalendarCombo.DateButton();
            Infragistics.Win.Appearance appearance90 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinSchedule.CalendarCombo.DateButton dateButton4 = new Infragistics.Win.UltraWinSchedule.CalendarCombo.DateButton();
            Infragistics.Win.Appearance appearance91 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance92 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance93 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance94 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance95 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance96 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance97 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance98 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance99 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance100 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance101 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand1 = new Infragistics.Win.UltraWinGrid.UltraGridBand("Band 0", -1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn1 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Vendor Code");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn2 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Vendor Name");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn3 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Vendor Item Code", -1, null, 0, Infragistics.Win.UltraWinGrid.SortIndicator.Ascending, false);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn4 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Cost Price");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn5 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Last Order Date");
            Infragistics.Win.Appearance appearance102 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance103 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance104 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance105 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance106 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance107 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance108 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance109 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance110 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance111 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance112 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance113 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance114 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance115 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance116 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance117 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance118 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance119 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance120 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinScrollBar.ScrollBarLook scrollBarLook1 = new Infragistics.Win.UltraWinScrollBar.ScrollBarLook();
            Infragistics.Win.Appearance appearance121 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance122 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance123 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance124 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance125 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance126 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinEditors.EditorButton editorButton3 = new Infragistics.Win.UltraWinEditors.EditorButton();
            Infragistics.Win.Appearance appearance127 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance128 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance129 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance130 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance131 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance132 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinSchedule.CalendarCombo.DateButton dateButton5 = new Infragistics.Win.UltraWinSchedule.CalendarCombo.DateButton();
            Infragistics.Win.Appearance appearance133 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance134 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinSchedule.CalendarCombo.DateButton dateButton6 = new Infragistics.Win.UltraWinSchedule.CalendarCombo.DateButton();
            Infragistics.Win.Appearance appearance135 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance136 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand2 = new Infragistics.Win.UltraWinGrid.UltraGridBand("Band 0", -1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn6 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("ID");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn7 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("ItemCode");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn8 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Description");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn9 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("AddedOn");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn10 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("SalePrice");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn11 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("CostPrice");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn12 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("UserID");
            Infragistics.Win.Appearance appearance137 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance138 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance139 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance140 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance141 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance142 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance143 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance144 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance145 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance146 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance147 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance148 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance149 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance150 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance151 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance152 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance153 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance154 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance155 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinScrollBar.ScrollBarLook scrollBarLook2 = new Infragistics.Win.UltraWinScrollBar.ScrollBarLook();
            Infragistics.Win.Appearance appearance156 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance157 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance158 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance159 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance160 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand3 = new Infragistics.Win.UltraWinGrid.UltraGridBand("Band 0", -1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn13 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("ID");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn14 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("ItemCode");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn15 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Description");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn16 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("OldQty");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn17 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("NewQty");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn18 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("isProcessed");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn19 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("UserID");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn20 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("TransDate");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn21 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("PUserID");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn22 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("PTransDate");
            Infragistics.Win.Appearance appearance161 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance162 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance163 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance164 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance165 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance166 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance167 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance168 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance169 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance170 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance171 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance172 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance173 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance174 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance175 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance176 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance177 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance178 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance179 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinScrollBar.ScrollBarLook scrollBarLook3 = new Infragistics.Win.UltraWinScrollBar.ScrollBarLook();
            Infragistics.Win.Appearance appearance180 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance181 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance182 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance183 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance184 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand4 = new Infragistics.Win.UltraWinGrid.UltraGridBand("", -1);
            Infragistics.Win.Appearance appearance185 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance186 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance187 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance188 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance189 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance190 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance191 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance192 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance193 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance194 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance195 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance196 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance197 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance198 = new Infragistics.Win.Appearance();
            Infragistics.UltraChart.Resources.Appearance.PaintElement paintElement1 = new Infragistics.UltraChart.Resources.Appearance.PaintElement();
            Infragistics.UltraChart.Resources.Appearance.GradientEffect gradientEffect1 = new Infragistics.UltraChart.Resources.Appearance.GradientEffect();
            Infragistics.Win.Appearance appearance199 = new Infragistics.Win.Appearance();
            Infragistics.Win.ValueListItem valueListItem1 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem2 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem3 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.Appearance appearance200 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance201 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance202 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance203 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance204 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance205 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance206 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance207 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance208 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance209 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance210 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance211 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab3 = new Infragistics.Win.UltraWinTabControl.UltraTab();
            Infragistics.Win.Appearance appearance212 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab4 = new Infragistics.Win.UltraWinTabControl.UltraTab();
            Infragistics.Win.Appearance appearance213 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab1 = new Infragistics.Win.UltraWinTabControl.UltraTab();
            Infragistics.Win.Appearance appearance214 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab2 = new Infragistics.Win.UltraWinTabControl.UltraTab();
            Infragistics.Win.Appearance appearance215 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab5 = new Infragistics.Win.UltraWinTabControl.UltraTab();
            Infragistics.Win.Appearance appearance216 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance217 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance218 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance219 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance220 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance221 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance222 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance223 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance224 = new Infragistics.Win.Appearance();
            this.ultraTabPageControl1 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.chkIsOnSale = new Infragistics.Win.UltraWinEditors.UltraCheckEditor();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.chkNonRefundable = new Infragistics.Win.UltraWinEditors.UltraCheckEditor();
            this.pnlItemMonitorCategory = new Infragistics.Win.Misc.UltraPanel();
            this.lblItemMonitorCategory = new Infragistics.Win.Misc.UltraLabel();
            this.pnlAddSecDesc = new Infragistics.Win.Misc.UltraPanel();
            this.btnAddSecDesc = new Infragistics.Win.Misc.UltraButton();
            this.lblAddSecDesc = new Infragistics.Win.Misc.UltraLabel();
            this.chkIsActive = new Infragistics.Win.UltraWinEditors.UltraCheckEditor();
            this.txtDescription = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.txtItemDesc = new System.Windows.Forms.TextBox();
            this.txtManufacturerName = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.ultraLabel2 = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabel16 = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabel1 = new Infragistics.Win.Misc.UltraLabel();
            this.chkIsEBT = new Infragistics.Win.UltraWinEditors.UltraCheckEditor();
            this.ultraLabel14 = new Infragistics.Win.Misc.UltraLabel();
            this.txtItemCode = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.txtUnit = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.txtSubDepartment = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.lblSubDepartment = new Infragistics.Win.Misc.UltraLabel();
            this.btnItemMonitorCategory = new Infragistics.Win.Misc.UltraButton();
            this.chkIsOTCItem = new Infragistics.Win.UltraWinEditors.UltraCheckEditor();
            this.chkExclFromRecpt = new Infragistics.Win.UltraWinEditors.UltraCheckEditor();
            this.chkExclFromAutoPO = new Infragistics.Win.UltraWinEditors.UltraCheckEditor();
            this.txtDepartmentCode = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.cboItemType = new Infragistics.Win.UltraWinEditors.UltraComboEditor();
            this.txtSaleTypeCode = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.ultraLabel7 = new Infragistics.Win.Misc.UltraLabel();
            this.txtSeasonCode = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.txtDepartmentDescription = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabel5 = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabel3 = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabel4 = new Infragistics.Win.Misc.UltraLabel();
            this.txtProductCode = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.ultraLabel20 = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabel9 = new Infragistics.Win.Misc.UltraLabel();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.lblActualTaxToDisplay = new Infragistics.Win.Misc.UltraLabel();
            this.lblActualTax = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabel30 = new Infragistics.Win.Misc.UltraLabel();
            this.cmbCLPointPolicy = new Infragistics.Win.UltraWinEditors.UltraComboEditor();
            this.ultraLabel29 = new Infragistics.Win.Misc.UltraLabel();
            this.numCLPointsPerDollar = new Infragistics.Win.UltraWinEditors.UltraNumericEditor();
            this.chkIsDefaultCLPoints = new Infragistics.Win.UltraWinEditors.UltraCheckEditor();
            this.cboTaxCodes = new Infragistics.Win.UltraWinEditors.UltraComboEditor();
            this.cmbDiscountPolicy = new Infragistics.Win.UltraWinEditors.UltraComboEditor();
            this.ultraLabel21 = new Infragistics.Win.Misc.UltraLabel();
            this.cmbTaxPolicy = new Infragistics.Win.UltraWinEditors.UltraComboEditor();
            this.chkUpdatePriceItemWise = new Infragistics.Win.UltraWinEditors.UltraCheckEditor();
            this.ultraLabel19 = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabel18 = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabel13 = new Infragistics.Win.Misc.UltraLabel();
            this.numFreight = new Infragistics.Win.UltraWinEditors.UltraNumericEditor();
            this.chkIsTaxable = new Infragistics.Win.UltraWinEditors.UltraCheckEditor();
            this.numLastCostPrice = new Infragistics.Win.UltraWinEditors.UltraNumericEditor();
            this.numAveragePrice = new Infragistics.Win.UltraWinEditors.UltraNumericEditor();
            this.numSellingPrice = new Infragistics.Win.UltraWinEditors.UltraNumericEditor();
            this.numDiscountAmt = new Infragistics.Win.UltraWinEditors.UltraNumericEditor();
            this.ultraLabel11 = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabel8 = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabel10 = new Infragistics.Win.Misc.UltraLabel();
            this.chkIsDiscountable = new Infragistics.Win.UltraWinEditors.UltraCheckEditor();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.btnRefreshDataFromMMS = new Infragistics.Win.Misc.UltraButton();
            this.dtpExpiryDate = new Infragistics.Win.UltraWinSchedule.UltraCalendarCombo();
            this.lblExpiryDate = new Infragistics.Win.Misc.UltraLabel();
            this.numMinQty = new Infragistics.Win.UltraWinEditors.UltraNumericEditor();
            this.grpItemPacketInfo = new System.Windows.Forms.GroupBox();
            this.numCaseCost = new Infragistics.Win.UltraWinEditors.UltraNumericEditor();
            this.lblCaseCost = new Infragistics.Win.Misc.UltraLabel();
            this.txtPacketSize = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.txtPacketQuantity = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.txtPacketUnit = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.lblPacketUnit = new Infragistics.Win.Misc.UltraLabel();
            this.lblPacketQuantity = new Infragistics.Win.Misc.UltraLabel();
            this.lblPacketSize = new Infragistics.Win.Misc.UltraLabel();
            this.chkPacketSize = new Infragistics.Win.UltraWinEditors.UltraCheckEditor();
            this.numQtyInStock = new Infragistics.Win.UltraWinEditors.UltraNumericEditor();
            this.numQtyOnOrder = new Infragistics.Win.UltraWinEditors.UltraNumericEditor();
            this.numReorderLevel = new Infragistics.Win.UltraWinEditors.UltraNumericEditor();
            this.dtpExpectedDelDate = new Infragistics.Win.UltraWinSchedule.UltraCalendarCombo();
            this.ultraLabel27 = new Infragistics.Win.Misc.UltraLabel();
            this.txtLocation = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.ultraLabel22 = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabel25 = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabel24 = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabel23 = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabel26 = new Infragistics.Win.Misc.UltraLabel();
            this.faOnSale = new System.Windows.Forms.GroupBox();
            this.numSalePrice = new Infragistics.Win.UltraWinEditors.UltraNumericEditor();
            this.dtpSaleEndDate = new Infragistics.Win.UltraWinSchedule.UltraCalendarCombo();
            this.dtpSaleStartDate = new Infragistics.Win.UltraWinSchedule.UltraCalendarCombo();
            this.numSaleLimitQty = new Infragistics.Win.UltraWinEditors.UltraNumericEditor();
            this.ultraLabel17 = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabel15 = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabel6 = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabel12 = new Infragistics.Win.Misc.UltraLabel();
            this.btnSave = new Infragistics.Win.Misc.UltraButton();
            this.ultraTabPageControl2 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.grbVendorInformation = new System.Windows.Forms.GroupBox();
            this.pnlDeleteItemVendorInfo = new Infragistics.Win.Misc.UltraPanel();
            this.btnDeleteItemVendorInfo = new Infragistics.Win.Misc.UltraButton();
            this.lblDeleteItemVendorInfo = new Infragistics.Win.Misc.UltraLabel();
            this.pnlSaveItemVendor = new Infragistics.Win.Misc.UltraPanel();
            this.btnSaveItemVendor = new Infragistics.Win.Misc.UltraButton();
            this.lblSaveItemVendor = new Infragistics.Win.Misc.UltraLabel();
            this.grdDetail = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.combEditorPrefVendor = new Infragistics.Win.UltraWinEditors.UltraComboEditor();
            this.ultraLblLastVendorLabel = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabelPreferredVendor = new Infragistics.Win.Misc.UltraLabel();
            this.txtLastVendor = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.txtVendorName = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabel31 = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabel33 = new Infragistics.Win.Misc.UltraLabel();
            this.txtRemarks = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.dtpLastSaleDate = new Infragistics.Win.UltraWinSchedule.UltraCalendarCombo();
            this.ultraLabel28 = new Infragistics.Win.Misc.UltraLabel();
            this.dtpLastRecvDate = new Infragistics.Win.UltraWinSchedule.UltraCalendarCombo();
            this.ultraLabel32 = new Infragistics.Win.Misc.UltraLabel();
            this.ultraTabPageControl3 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.groupBox8 = new System.Windows.Forms.GroupBox();
            this.grdPriceChangeLog = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.ultraTabPageControl4 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.groupBox9 = new System.Windows.Forms.GroupBox();
            this.grdPhysicalInvView = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.ultraTabPageControl6 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.grdVendor = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.ucSummary = new Infragistics.Win.UltraWinChart.UltraChart();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.flowLayoutPanel3 = new System.Windows.Forms.FlowLayoutPanel();
            this.label4 = new System.Windows.Forms.Label();
            this.opSummBy = new Infragistics.Win.UltraWinEditors.UltraOptionSet();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnView = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.dtFrom = new Infragistics.Win.UltraWinEditors.UltraDateTimeEditor();
            this.label3 = new System.Windows.Forms.Label();
            this.dtTo = new Infragistics.Win.UltraWinEditors.UltraDateTimeEditor();
            this.tableLayoutPanel5 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel6 = new System.Windows.Forms.TableLayoutPanel();
            this.lblQtyInHand = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.lblAvgProfitPerItem = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.lblAvgRecvPerMonth = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.lblAvgReturnPerMonth = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.lblAvgSalePerMonth = new System.Windows.Forms.Label();
            this.lblSale = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.btnItemNote = new Infragistics.Win.Misc.UltraButton();
            this.lblTransactionType = new Infragistics.Win.Misc.UltraLabel();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.btnClose = new Infragistics.Win.Misc.UltraButton();
            this.btnOk = new Infragistics.Win.Misc.UltraButton();
            this.btnGrpPricing = new Infragistics.Win.Misc.UltraButton();
            this.btnCompanionItem = new Infragistics.Win.Misc.UltraButton();
            this.btnInventoryHistory = new Infragistics.Win.Misc.UltraButton();
            this.btnPriceValidation = new Infragistics.Win.Misc.UltraButton();
            this.btnInventoryReceived = new Infragistics.Win.Misc.UltraButton();
            this.tabItemInformation = new Infragistics.Win.UltraWinTabControl.UltraTabControl();
            this.ultraTabSharedControlsPage1 = new Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.pnlItemNote = new Infragistics.Win.Misc.UltraPanel();
            this.lblItemNote = new Infragistics.Win.Misc.UltraLabel();
            this.pnlCompanionItem = new Infragistics.Win.Misc.UltraPanel();
            this.lblCompanionItem = new Infragistics.Win.Misc.UltraLabel();
            this.pnlInventoryReceived = new Infragistics.Win.Misc.UltraPanel();
            this.lblInventoryReceived = new Infragistics.Win.Misc.UltraLabel();
            this.pnlGrpPricing = new Infragistics.Win.Misc.UltraPanel();
            this.lblGrpPricing = new Infragistics.Win.Misc.UltraLabel();
            this.pnlOk = new Infragistics.Win.Misc.UltraPanel();
            this.lblOk = new Infragistics.Win.Misc.UltraLabel();
            this.pnlClose = new Infragistics.Win.Misc.UltraPanel();
            this.lblClose = new Infragistics.Win.Misc.UltraLabel();
            this.btnCopyContents = new Infragistics.Win.Misc.UltraButton();
            this.btnMMSSearch = new Infragistics.Win.Misc.UltraButton();
            this.ultraTabPageControl5 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.ultraTabPageControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chkIsOnSale)).BeginInit();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chkNonRefundable)).BeginInit();
            this.pnlItemMonitorCategory.ClientArea.SuspendLayout();
            this.pnlItemMonitorCategory.SuspendLayout();
            this.pnlAddSecDesc.ClientArea.SuspendLayout();
            this.pnlAddSecDesc.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chkIsActive)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDescription)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtManufacturerName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkIsEBT)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtItemCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtUnit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSubDepartment)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkIsOTCItem)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkExclFromRecpt)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkExclFromAutoPO)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDepartmentCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboItemType)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSaleTypeCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSeasonCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtProductCode)).BeginInit();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbCLPointPolicy)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numCLPointsPerDollar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkIsDefaultCLPoints)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboTaxCodes)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbDiscountPolicy)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbTaxPolicy)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkUpdatePriceItemWise)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numFreight)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkIsTaxable)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numLastCostPrice)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numAveragePrice)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numSellingPrice)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numDiscountAmt)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkIsDiscountable)).BeginInit();
            this.groupBox5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dtpExpiryDate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numMinQty)).BeginInit();
            this.grpItemPacketInfo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numCaseCost)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPacketSize)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPacketQuantity)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPacketUnit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkPacketSize)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numQtyInStock)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numQtyOnOrder)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numReorderLevel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtpExpectedDelDate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtLocation)).BeginInit();
            this.faOnSale.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numSalePrice)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtpSaleEndDate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtpSaleStartDate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numSaleLimitQty)).BeginInit();
            this.ultraTabPageControl2.SuspendLayout();
            this.grbVendorInformation.SuspendLayout();
            this.pnlDeleteItemVendorInfo.ClientArea.SuspendLayout();
            this.pnlDeleteItemVendorInfo.SuspendLayout();
            this.pnlSaveItemVendor.ClientArea.SuspendLayout();
            this.pnlSaveItemVendor.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdDetail)).BeginInit();
            this.groupBox6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.combEditorPrefVendor)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtLastVendor)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtRemarks)).BeginInit();
            this.groupBox7.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dtpLastSaleDate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtpLastRecvDate)).BeginInit();
            this.ultraTabPageControl3.SuspendLayout();
            this.groupBox8.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdPriceChangeLog)).BeginInit();
            this.ultraTabPageControl4.SuspendLayout();
            this.groupBox9.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdPhysicalInvView)).BeginInit();
            this.ultraTabPageControl6.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdVendor)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ucSummary)).BeginInit();
            this.tableLayoutPanel4.SuspendLayout();
            this.flowLayoutPanel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.opSummBy)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dtFrom)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtTo)).BeginInit();
            this.tableLayoutPanel5.SuspendLayout();
            this.tableLayoutPanel6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tabItemInformation)).BeginInit();
            this.tabItemInformation.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.pnlItemNote.ClientArea.SuspendLayout();
            this.pnlItemNote.SuspendLayout();
            this.pnlCompanionItem.ClientArea.SuspendLayout();
            this.pnlCompanionItem.SuspendLayout();
            this.pnlInventoryReceived.ClientArea.SuspendLayout();
            this.pnlInventoryReceived.SuspendLayout();
            this.pnlGrpPricing.ClientArea.SuspendLayout();
            this.pnlGrpPricing.SuspendLayout();
            this.pnlOk.ClientArea.SuspendLayout();
            this.pnlOk.SuspendLayout();
            this.pnlClose.ClientArea.SuspendLayout();
            this.pnlClose.SuspendLayout();
            this.SuspendLayout();
            // 
            // ultraTabPageControl1
            // 
            this.ultraTabPageControl1.Controls.Add(this.chkIsOnSale);
            this.ultraTabPageControl1.Controls.Add(this.groupBox2);
            this.ultraTabPageControl1.Controls.Add(this.groupBox3);
            this.ultraTabPageControl1.Controls.Add(this.groupBox5);
            this.ultraTabPageControl1.Controls.Add(this.faOnSale);
            this.ultraTabPageControl1.Location = new System.Drawing.Point(2, 23);
            this.ultraTabPageControl1.Name = "ultraTabPageControl1";
            this.ultraTabPageControl1.Size = new System.Drawing.Size(924, 521);
            // 
            // chkIsOnSale
            // 
            appearance1.BackColor = System.Drawing.Color.Transparent;
            appearance1.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(108)))), ((int)(((byte)(172)))));
            appearance1.FontData.BoldAsString = "True";
            appearance1.ForeColor = System.Drawing.Color.Black;
            appearance1.ForeColorDisabled = System.Drawing.Color.White;
            this.chkIsOnSale.Appearance = appearance1;
            this.chkIsOnSale.BackColor = System.Drawing.Color.Transparent;
            this.chkIsOnSale.BackColorInternal = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(108)))), ((int)(((byte)(172)))));
            this.chkIsOnSale.Location = new System.Drawing.Point(572, 335);
            this.chkIsOnSale.Name = "chkIsOnSale";
            this.chkIsOnSale.Size = new System.Drawing.Size(85, 16);
            this.chkIsOnSale.TabIndex = 3;
            this.chkIsOnSale.Text = "On Sale";
            this.chkIsOnSale.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.chkIsOnSale.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.chkIsOnSale.CheckedChanged += new System.EventHandler(this.CheckBoxs_Checked);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.chkNonRefundable);
            this.groupBox2.Controls.Add(this.pnlItemMonitorCategory);
            this.groupBox2.Controls.Add(this.pnlAddSecDesc);
            this.groupBox2.Controls.Add(this.chkIsActive);
            this.groupBox2.Controls.Add(this.txtDescription);
            this.groupBox2.Controls.Add(this.txtItemDesc);
            this.groupBox2.Controls.Add(this.txtManufacturerName);
            this.groupBox2.Controls.Add(this.ultraLabel2);
            this.groupBox2.Controls.Add(this.ultraLabel16);
            this.groupBox2.Controls.Add(this.ultraLabel1);
            this.groupBox2.Controls.Add(this.chkIsEBT);
            this.groupBox2.Controls.Add(this.ultraLabel14);
            this.groupBox2.Controls.Add(this.txtItemCode);
            this.groupBox2.Controls.Add(this.txtUnit);
            this.groupBox2.Controls.Add(this.txtSubDepartment);
            this.groupBox2.Controls.Add(this.lblSubDepartment);
            this.groupBox2.Controls.Add(this.btnItemMonitorCategory);
            this.groupBox2.Controls.Add(this.chkIsOTCItem);
            this.groupBox2.Controls.Add(this.chkExclFromRecpt);
            this.groupBox2.Controls.Add(this.chkExclFromAutoPO);
            this.groupBox2.Controls.Add(this.txtDepartmentCode);
            this.groupBox2.Controls.Add(this.cboItemType);
            this.groupBox2.Controls.Add(this.txtSaleTypeCode);
            this.groupBox2.Controls.Add(this.ultraLabel7);
            this.groupBox2.Controls.Add(this.txtSeasonCode);
            this.groupBox2.Controls.Add(this.txtDepartmentDescription);
            this.groupBox2.Controls.Add(this.ultraLabel5);
            this.groupBox2.Controls.Add(this.ultraLabel3);
            this.groupBox2.Controls.Add(this.ultraLabel4);
            this.groupBox2.Controls.Add(this.txtProductCode);
            this.groupBox2.Controls.Add(this.ultraLabel20);
            this.groupBox2.Controls.Add(this.ultraLabel9);
            this.groupBox2.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.groupBox2.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.ForeColor = System.Drawing.Color.White;
            this.groupBox2.Location = new System.Drawing.Point(6, 3);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(560, 299);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Item Information";
            // 
            // chkNonRefundable
            // 
            appearance2.FontData.BoldAsString = "False";
            appearance2.FontData.Name = "Verdana";
            appearance2.FontData.SizeInPoints = 10F;
            appearance2.ForeColor = System.Drawing.Color.Black;
            appearance2.ForeColorDisabled = System.Drawing.Color.White;
            this.chkNonRefundable.Appearance = appearance2;
            this.chkNonRefundable.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkNonRefundable.Location = new System.Drawing.Point(7, 276);
            this.chkNonRefundable.Name = "chkNonRefundable";
            this.chkNonRefundable.Size = new System.Drawing.Size(252, 19);
            this.chkNonRefundable.TabIndex = 10;
            this.chkNonRefundable.Text = "Non-Refundable";
            this.chkNonRefundable.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.chkNonRefundable.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.chkNonRefundable.CheckedChanged += new System.EventHandler(this.CheckBoxs_Checked);
            // 
            // pnlItemMonitorCategory
            // 
            // 
            // pnlItemMonitorCategory.ClientArea
            // 
            this.pnlItemMonitorCategory.ClientArea.Controls.Add(this.lblItemMonitorCategory);
            this.pnlItemMonitorCategory.Location = new System.Drawing.Point(465, 250);
            this.pnlItemMonitorCategory.Name = "pnlItemMonitorCategory";
            this.pnlItemMonitorCategory.Size = new System.Drawing.Size(60, 30);
            this.pnlItemMonitorCategory.TabIndex = 17;
            this.pnlItemMonitorCategory.Visible = false;
            // 
            // lblItemMonitorCategory
            // 
            appearance3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(138)))), ((int)(((byte)(31)))));
            appearance3.FontData.BoldAsString = "True";
            appearance3.ForeColor = System.Drawing.Color.White;
            appearance3.TextHAlignAsString = "Center";
            appearance3.TextVAlignAsString = "Middle";
            this.lblItemMonitorCategory.Appearance = appearance3;
            this.lblItemMonitorCategory.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblItemMonitorCategory.Font = new System.Drawing.Font("Verdana", 8F, System.Drawing.FontStyle.Bold);
            this.lblItemMonitorCategory.Location = new System.Drawing.Point(0, 0);
            this.lblItemMonitorCategory.Margin = new System.Windows.Forms.Padding(3, 3, 0, 3);
            this.lblItemMonitorCategory.Name = "lblItemMonitorCategory";
            this.lblItemMonitorCategory.Size = new System.Drawing.Size(60, 30);
            this.lblItemMonitorCategory.TabIndex = 0;
            this.lblItemMonitorCategory.Tag = "NOCOLOR";
            this.lblItemMonitorCategory.Text = "F9";
            this.lblItemMonitorCategory.Click += new System.EventHandler(this.btnItemMonitorCategory_Click);
            // 
            // pnlAddSecDesc
            // 
            // 
            // pnlAddSecDesc.ClientArea
            // 
            this.pnlAddSecDesc.ClientArea.Controls.Add(this.btnAddSecDesc);
            this.pnlAddSecDesc.ClientArea.Controls.Add(this.lblAddSecDesc);
            this.pnlAddSecDesc.Location = new System.Drawing.Point(428, 42);
            this.pnlAddSecDesc.Name = "pnlAddSecDesc";
            this.pnlAddSecDesc.Size = new System.Drawing.Size(126, 30);
            this.pnlAddSecDesc.TabIndex = 16;
            this.pnlAddSecDesc.Visible = false;
            // 
            // btnAddSecDesc
            // 
            appearance4.BackColor = System.Drawing.Color.White;
            appearance4.ForeColor = System.Drawing.Color.Black;
            this.btnAddSecDesc.Appearance = appearance4;
            this.btnAddSecDesc.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Flat;
            this.btnAddSecDesc.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnAddSecDesc.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold);
            this.btnAddSecDesc.Location = new System.Drawing.Point(30, 0);
            this.btnAddSecDesc.Name = "btnAddSecDesc";
            this.btnAddSecDesc.Size = new System.Drawing.Size(96, 30);
            this.btnAddSecDesc.TabIndex = 15;
            this.btnAddSecDesc.TabStop = false;
            this.btnAddSecDesc.Text = "Alt. Lang.";
            this.btnAddSecDesc.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnAddSecDesc.Click += new System.EventHandler(this.btnAddSecDesc_Click);
            // 
            // lblAddSecDesc
            // 
            appearance5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(138)))), ((int)(((byte)(31)))));
            appearance5.FontData.BoldAsString = "True";
            appearance5.ForeColor = System.Drawing.Color.White;
            appearance5.TextHAlignAsString = "Center";
            appearance5.TextVAlignAsString = "Middle";
            this.lblAddSecDesc.Appearance = appearance5;
            this.lblAddSecDesc.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblAddSecDesc.Font = new System.Drawing.Font("Verdana", 8F, System.Drawing.FontStyle.Bold);
            this.lblAddSecDesc.Location = new System.Drawing.Point(0, 0);
            this.lblAddSecDesc.Margin = new System.Windows.Forms.Padding(3, 3, 0, 3);
            this.lblAddSecDesc.Name = "lblAddSecDesc";
            this.lblAddSecDesc.Size = new System.Drawing.Size(30, 30);
            this.lblAddSecDesc.TabIndex = 0;
            this.lblAddSecDesc.Tag = "NOCOLOR";
            this.lblAddSecDesc.Text = "F8";
            this.lblAddSecDesc.Click += new System.EventHandler(this.btnAddSecDesc_Click);
            // 
            // chkIsActive
            // 
            appearance6.FontData.BoldAsString = "False";
            appearance6.ForeColor = System.Drawing.Color.Black;
            appearance6.ForeColorDisabled = System.Drawing.Color.White;
            this.chkIsActive.Appearance = appearance6;
            this.chkIsActive.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkIsActive.Checked = true;
            this.chkIsActive.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkIsActive.Location = new System.Drawing.Point(273, 121);
            this.chkIsActive.Name = "chkIsActive";
            this.chkIsActive.Size = new System.Drawing.Size(252, 20);
            this.chkIsActive.TabIndex = 11;
            this.chkIsActive.Text = "Is Active Item";
            this.chkIsActive.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.chkIsActive.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.chkIsActive.CheckedChanged += new System.EventHandler(this.CheckBoxs_Checked);
            // 
            // txtDescription
            // 
            appearance7.FontData.BoldAsString = "False";
            appearance7.FontData.ItalicAsString = "False";
            appearance7.FontData.StrikeoutAsString = "False";
            appearance7.FontData.UnderlineAsString = "False";
            appearance7.ForeColor = System.Drawing.Color.Black;
            appearance7.ForeColorDisabled = System.Drawing.Color.Black;
            this.txtDescription.Appearance = appearance7;
            this.txtDescription.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.txtDescription.Location = new System.Drawing.Point(128, 42);
            this.txtDescription.MaxLength = 40;
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.Size = new System.Drawing.Size(292, 23);
            this.txtDescription.TabIndex = 1;
            this.toolTip1.SetToolTip(this.txtDescription, "Press F4 To Search");
            this.txtDescription.Validated += new System.EventHandler(this.txtBoxs_Validate);
            // 
            // txtItemDesc
            // 
            this.txtItemDesc.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtItemDesc.Location = new System.Drawing.Point(128, 42);
            this.txtItemDesc.Name = "txtItemDesc";
            this.txtItemDesc.Size = new System.Drawing.Size(292, 24);
            this.txtItemDesc.TabIndex = 1;
            this.txtItemDesc.TabStop = false;
            this.txtItemDesc.Visible = false;
            this.txtItemDesc.Enter += new System.EventHandler(this.txtItemDesc_Enter);
            this.txtItemDesc.Leave += new System.EventHandler(this.txtItemDesc_Leave);
            this.txtItemDesc.Validated += new System.EventHandler(this.txtBoxs_Validate);
            // 
            // txtManufacturerName
            // 
            appearance8.FontData.BoldAsString = "False";
            appearance8.FontData.ItalicAsString = "False";
            appearance8.FontData.StrikeoutAsString = "False";
            appearance8.FontData.UnderlineAsString = "False";
            appearance8.ForeColor = System.Drawing.Color.Black;
            appearance8.ForeColorDisabled = System.Drawing.Color.Black;
            this.txtManufacturerName.Appearance = appearance8;
            this.txtManufacturerName.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.txtManufacturerName.Location = new System.Drawing.Point(128, 224);
            this.txtManufacturerName.MaxLength = 50;
            this.txtManufacturerName.Name = "txtManufacturerName";
            this.txtManufacturerName.Size = new System.Drawing.Size(132, 23);
            this.txtManufacturerName.TabIndex = 8;
            this.txtManufacturerName.Validated += new System.EventHandler(this.txtBoxs_Validate);
            // 
            // ultraLabel2
            // 
            appearance9.FontData.BoldAsString = "False";
            appearance9.ForeColor = System.Drawing.Color.Black;
            this.ultraLabel2.Appearance = appearance9;
            this.ultraLabel2.Location = new System.Drawing.Point(7, 45);
            this.ultraLabel2.Name = "ultraLabel2";
            this.ultraLabel2.Size = new System.Drawing.Size(98, 19);
            this.ultraLabel2.TabIndex = 3;
            this.ultraLabel2.Text = "Description *";
            this.ultraLabel2.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // ultraLabel16
            // 
            appearance10.FontData.BoldAsString = "False";
            appearance10.ForeColor = System.Drawing.Color.Black;
            this.ultraLabel16.Appearance = appearance10;
            this.ultraLabel16.Location = new System.Drawing.Point(7, 226);
            this.ultraLabel16.Name = "ultraLabel16";
            this.ultraLabel16.Size = new System.Drawing.Size(112, 19);
            this.ultraLabel16.TabIndex = 14;
            this.ultraLabel16.Text = "Manufacturer";
            this.ultraLabel16.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // ultraLabel1
            // 
            appearance11.FontData.BoldAsString = "False";
            appearance11.ForeColor = System.Drawing.Color.Black;
            this.ultraLabel1.Appearance = appearance11;
            this.ultraLabel1.Location = new System.Drawing.Point(7, 18);
            this.ultraLabel1.Name = "ultraLabel1";
            this.ultraLabel1.Size = new System.Drawing.Size(96, 19);
            this.ultraLabel1.TabIndex = 1;
            this.ultraLabel1.Text = "Item Code *";
            this.ultraLabel1.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // chkIsEBT
            // 
            appearance12.FontData.BoldAsString = "False";
            appearance12.ForeColor = System.Drawing.Color.Black;
            appearance12.ForeColorDisabled = System.Drawing.Color.White;
            this.chkIsEBT.Appearance = appearance12;
            this.chkIsEBT.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkIsEBT.Location = new System.Drawing.Point(273, 173);
            this.chkIsEBT.Name = "chkIsEBT";
            this.chkIsEBT.Size = new System.Drawing.Size(252, 20);
            this.chkIsEBT.TabIndex = 13;
            this.chkIsEBT.Text = "Mark Item as EBT";
            this.chkIsEBT.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.chkIsEBT.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.chkIsEBT.CheckedChanged += new System.EventHandler(this.CheckBoxs_Checked);
            // 
            // ultraLabel14
            // 
            appearance13.FontData.BoldAsString = "False";
            appearance13.ForeColor = System.Drawing.Color.Black;
            this.ultraLabel14.Appearance = appearance13;
            this.ultraLabel14.Location = new System.Drawing.Point(7, 254);
            this.ultraLabel14.Name = "ultraLabel14";
            this.ultraLabel14.Size = new System.Drawing.Size(112, 19);
            this.ultraLabel14.TabIndex = 0;
            this.ultraLabel14.Text = "Unit";
            this.ultraLabel14.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // txtItemCode
            // 
            appearance14.FontData.BoldAsString = "False";
            appearance14.FontData.ItalicAsString = "False";
            appearance14.FontData.StrikeoutAsString = "False";
            appearance14.FontData.UnderlineAsString = "False";
            appearance14.ForeColor = System.Drawing.Color.Black;
            appearance14.ForeColorDisabled = System.Drawing.Color.Black;
            this.txtItemCode.Appearance = appearance14;
            this.txtItemCode.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.txtItemCode.Location = new System.Drawing.Point(128, 16);
            this.txtItemCode.MaxLength = 20;
            this.txtItemCode.Name = "txtItemCode";
            this.txtItemCode.Size = new System.Drawing.Size(132, 23);
            this.txtItemCode.TabIndex = 0;
            this.toolTip1.SetToolTip(this.txtItemCode, "Press F4 To Search");
            this.txtItemCode.Leave += new System.EventHandler(this.ForiegnKeys_Leave);
            this.txtItemCode.Validated += new System.EventHandler(this.txtBoxs_Validate);
            // 
            // txtUnit
            // 
            appearance15.FontData.BoldAsString = "False";
            appearance15.FontData.ItalicAsString = "False";
            appearance15.FontData.StrikeoutAsString = "False";
            appearance15.FontData.UnderlineAsString = "False";
            appearance15.ForeColor = System.Drawing.Color.Black;
            appearance15.ForeColorDisabled = System.Drawing.Color.Black;
            this.txtUnit.Appearance = appearance15;
            this.txtUnit.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.txtUnit.Location = new System.Drawing.Point(128, 250);
            this.txtUnit.MaxLength = 10;
            this.txtUnit.Name = "txtUnit";
            this.txtUnit.Size = new System.Drawing.Size(132, 23);
            this.txtUnit.TabIndex = 9;
            this.txtUnit.Validated += new System.EventHandler(this.txtBoxs_Validate);
            // 
            // txtSubDepartment
            // 
            appearance16.FontData.BoldAsString = "False";
            appearance16.FontData.ItalicAsString = "False";
            appearance16.FontData.StrikeoutAsString = "False";
            appearance16.FontData.UnderlineAsString = "False";
            appearance16.ForeColor = System.Drawing.Color.Black;
            appearance16.ForeColorDisabled = System.Drawing.Color.Black;
            this.txtSubDepartment.Appearance = appearance16;
            this.txtSubDepartment.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance17.BackColor = System.Drawing.Color.White;
            appearance17.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(231)))));
            appearance17.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance17.BorderAlpha = Infragistics.Win.Alpha.Transparent;
            editorButton1.Appearance = appearance17;
            editorButton1.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Flat;
            editorButton1.Text = "";
            editorButton1.Width = 16;
            this.txtSubDepartment.ButtonsRight.Add(editorButton1);
            this.txtSubDepartment.Location = new System.Drawing.Point(128, 94);
            this.txtSubDepartment.MaxLength = 20;
            this.txtSubDepartment.Name = "txtSubDepartment";
            this.txtSubDepartment.Size = new System.Drawing.Size(132, 23);
            this.txtSubDepartment.TabIndex = 3;
            this.toolTip1.SetToolTip(this.txtSubDepartment, "Press F4 To Search");
            this.txtSubDepartment.EditorButtonClick += new Infragistics.Win.UltraWinEditors.EditorButtonEventHandler(this.txtItemCode_EditorButtonClick);
            this.txtSubDepartment.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtSubDepartment_KeyPress);
            this.txtSubDepartment.Leave += new System.EventHandler(this.ForiegnKeys_Leave);
            // 
            // lblSubDepartment
            // 
            appearance18.FontData.BoldAsString = "False";
            appearance18.FontData.ItalicAsString = "False";
            appearance18.FontData.StrikeoutAsString = "False";
            appearance18.FontData.UnderlineAsString = "False";
            appearance18.ForeColor = System.Drawing.Color.White;
            appearance18.ForeColorDisabled = System.Drawing.Color.White;
            this.lblSubDepartment.Appearance = appearance18;
            this.lblSubDepartment.BorderStyleOuter = Infragistics.Win.UIElementBorderStyle.Solid;
            this.lblSubDepartment.Location = new System.Drawing.Point(260, 94);
            this.lblSubDepartment.Name = "lblSubDepartment";
            this.lblSubDepartment.Size = new System.Drawing.Size(160, 23);
            this.lblSubDepartment.TabIndex = 5;
            // 
            // btnItemMonitorCategory
            // 
            appearance19.BackColor = System.Drawing.Color.White;
            appearance19.ForeColor = System.Drawing.Color.Black;
            this.btnItemMonitorCategory.Appearance = appearance19;
            this.btnItemMonitorCategory.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Flat;
            this.btnItemMonitorCategory.Location = new System.Drawing.Point(452, 249);
            this.btnItemMonitorCategory.Name = "btnItemMonitorCategory";
            this.btnItemMonitorCategory.Size = new System.Drawing.Size(76, 25);
            this.btnItemMonitorCategory.TabIndex = 16;
            this.btnItemMonitorCategory.TabStop = false;
            this.btnItemMonitorCategory.Text = "(F9)";
            this.btnItemMonitorCategory.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnItemMonitorCategory.Visible = false;
            this.btnItemMonitorCategory.Click += new System.EventHandler(this.btnItemMonitorCategory_Click);
            // 
            // chkIsOTCItem
            // 
            appearance20.FontData.BoldAsString = "False";
            appearance20.ForeColor = System.Drawing.Color.Black;
            appearance20.ForeColorDisabled = System.Drawing.Color.White;
            this.chkIsOTCItem.Appearance = appearance20;
            this.chkIsOTCItem.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkIsOTCItem.Location = new System.Drawing.Point(273, 225);
            this.chkIsOTCItem.Name = "chkIsOTCItem";
            this.chkIsOTCItem.Size = new System.Drawing.Size(252, 20);
            this.chkIsOTCItem.TabIndex = 15;
            this.chkIsOTCItem.Text = "Monitor this Item";
            this.chkIsOTCItem.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.chkIsOTCItem.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.chkIsOTCItem.CheckedChanged += new System.EventHandler(this.CheckBoxs_Checked);
            // 
            // chkExclFromRecpt
            // 
            appearance21.FontData.BoldAsString = "False";
            appearance21.ForeColor = System.Drawing.Color.Black;
            appearance21.ForeColorDisabled = System.Drawing.Color.White;
            this.chkExclFromRecpt.Appearance = appearance21;
            this.chkExclFromRecpt.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkExclFromRecpt.Location = new System.Drawing.Point(273, 147);
            this.chkExclFromRecpt.Name = "chkExclFromRecpt";
            this.chkExclFromRecpt.Size = new System.Drawing.Size(252, 20);
            this.chkExclFromRecpt.TabIndex = 12;
            this.chkExclFromRecpt.Text = "Exclude from receipt";
            this.chkExclFromRecpt.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.chkExclFromRecpt.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.chkExclFromRecpt.CheckedChanged += new System.EventHandler(this.CheckBoxs_Checked);
            // 
            // chkExclFromAutoPO
            // 
            appearance22.FontData.BoldAsString = "False";
            appearance22.ForeColor = System.Drawing.Color.Black;
            appearance22.ForeColorDisabled = System.Drawing.Color.White;
            this.chkExclFromAutoPO.Appearance = appearance22;
            this.chkExclFromAutoPO.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkExclFromAutoPO.Location = new System.Drawing.Point(273, 199);
            this.chkExclFromAutoPO.Name = "chkExclFromAutoPO";
            this.chkExclFromAutoPO.Size = new System.Drawing.Size(252, 20);
            this.chkExclFromAutoPO.TabIndex = 14;
            this.chkExclFromAutoPO.Text = "Exclude from Auto PO generation";
            this.chkExclFromAutoPO.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.chkExclFromAutoPO.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.chkExclFromAutoPO.CheckedChanged += new System.EventHandler(this.CheckBoxs_Checked);
            // 
            // txtDepartmentCode
            // 
            appearance23.FontData.BoldAsString = "False";
            appearance23.FontData.ItalicAsString = "False";
            appearance23.FontData.StrikeoutAsString = "False";
            appearance23.FontData.UnderlineAsString = "False";
            appearance23.ForeColor = System.Drawing.Color.Black;
            appearance23.ForeColorDisabled = System.Drawing.Color.Black;
            this.txtDepartmentCode.Appearance = appearance23;
            this.txtDepartmentCode.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance24.BackColor = System.Drawing.Color.White;
            appearance24.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(231)))));
            appearance24.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance24.BorderAlpha = Infragistics.Win.Alpha.Transparent;
            editorButton2.Appearance = appearance24;
            editorButton2.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Flat;
            editorButton2.Text = "";
            editorButton2.Width = 16;
            this.txtDepartmentCode.ButtonsRight.Add(editorButton2);
            this.txtDepartmentCode.Location = new System.Drawing.Point(128, 68);
            this.txtDepartmentCode.MaxLength = 20;
            this.txtDepartmentCode.Name = "txtDepartmentCode";
            this.txtDepartmentCode.Size = new System.Drawing.Size(132, 23);
            this.txtDepartmentCode.TabIndex = 2;
            this.toolTip1.SetToolTip(this.txtDepartmentCode, "Press F4 To Search");
            this.txtDepartmentCode.ValueChanged += new System.EventHandler(this.txtDepartmentCode_ValueChanged);
            this.txtDepartmentCode.EditorButtonClick += new Infragistics.Win.UltraWinEditors.EditorButtonEventHandler(this.txtItemCode_EditorButtonClick);
            this.txtDepartmentCode.Leave += new System.EventHandler(this.ForiegnKeys_Leave);
            // 
            // cboItemType
            // 
            appearance25.BorderAlpha = Infragistics.Win.Alpha.Opaque;
            appearance25.BorderColor3DBase = System.Drawing.Color.Black;
            appearance25.FontData.BoldAsString = "False";
            appearance25.FontData.ItalicAsString = "False";
            appearance25.FontData.StrikeoutAsString = "False";
            appearance25.FontData.UnderlineAsString = "False";
            appearance25.ForeColor = System.Drawing.Color.Black;
            appearance25.ForeColorDisabled = System.Drawing.Color.Black;
            this.cboItemType.Appearance = appearance25;
            this.cboItemType.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance26.BackColor = System.Drawing.Color.White;
            appearance26.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(231)))), ((int)(((byte)(230)))));
            appearance26.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance26.BorderAlpha = Infragistics.Win.Alpha.Transparent;
            this.cboItemType.ButtonAppearance = appearance26;
            this.cboItemType.DropDownStyle = Infragistics.Win.DropDownStyle.DropDownList;
            valueListItem13.DataValue = "1";
            valueListItem13.DisplayText = "Stock";
            valueListItem14.DataValue = "2";
            valueListItem14.DisplayText = "Non Stock";
            valueListItem15.DataValue = "3";
            valueListItem15.DisplayText = "Comment";
            this.cboItemType.Items.AddRange(new Infragistics.Win.ValueListItem[] {
            valueListItem13,
            valueListItem14,
            valueListItem15});
            this.cboItemType.Location = new System.Drawing.Point(128, 172);
            this.cboItemType.MaxLength = 20;
            this.cboItemType.Name = "cboItemType";
            this.cboItemType.Size = new System.Drawing.Size(134, 23);
            this.cboItemType.TabIndex = 6;
            this.cboItemType.Leave += new System.EventHandler(this.cboItemType_SelectionChanged);
            // 
            // txtSaleTypeCode
            // 
            appearance27.FontData.BoldAsString = "False";
            appearance27.FontData.ItalicAsString = "False";
            appearance27.FontData.StrikeoutAsString = "False";
            appearance27.FontData.UnderlineAsString = "False";
            appearance27.ForeColor = System.Drawing.Color.Black;
            appearance27.ForeColorDisabled = System.Drawing.Color.Black;
            this.txtSaleTypeCode.Appearance = appearance27;
            this.txtSaleTypeCode.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.txtSaleTypeCode.Location = new System.Drawing.Point(128, 146);
            this.txtSaleTypeCode.MaxLength = 2;
            this.txtSaleTypeCode.Name = "txtSaleTypeCode";
            this.txtSaleTypeCode.Size = new System.Drawing.Size(132, 23);
            this.txtSaleTypeCode.TabIndex = 5;
            this.txtSaleTypeCode.Leave += new System.EventHandler(this.txtBoxs_Validate);
            this.txtSaleTypeCode.Validated += new System.EventHandler(this.txtBoxs_Validate);
            // 
            // ultraLabel7
            // 
            appearance28.FontData.BoldAsString = "False";
            appearance28.ForeColor = System.Drawing.Color.Black;
            this.ultraLabel7.Appearance = appearance28;
            this.ultraLabel7.Location = new System.Drawing.Point(7, 200);
            this.ultraLabel7.Name = "ultraLabel7";
            this.ultraLabel7.Size = new System.Drawing.Size(106, 19);
            this.ultraLabel7.TabIndex = 12;
            this.ultraLabel7.Text = "Season Code";
            this.ultraLabel7.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // txtSeasonCode
            // 
            appearance29.FontData.BoldAsString = "False";
            appearance29.FontData.ItalicAsString = "False";
            appearance29.FontData.StrikeoutAsString = "False";
            appearance29.FontData.UnderlineAsString = "False";
            appearance29.ForeColor = System.Drawing.Color.Black;
            appearance29.ForeColorDisabled = System.Drawing.Color.Black;
            this.txtSeasonCode.Appearance = appearance29;
            this.txtSeasonCode.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.txtSeasonCode.Location = new System.Drawing.Point(128, 198);
            this.txtSeasonCode.MaxLength = 2;
            this.txtSeasonCode.Name = "txtSeasonCode";
            this.txtSeasonCode.Size = new System.Drawing.Size(132, 23);
            this.txtSeasonCode.TabIndex = 7;
            this.txtSeasonCode.Leave += new System.EventHandler(this.txtBoxs_Validate);
            this.txtSeasonCode.Validated += new System.EventHandler(this.txtBoxs_Validate);
            // 
            // txtDepartmentDescription
            // 
            appearance30.FontData.BoldAsString = "False";
            appearance30.FontData.ItalicAsString = "False";
            appearance30.FontData.StrikeoutAsString = "False";
            appearance30.FontData.UnderlineAsString = "False";
            appearance30.ForeColor = System.Drawing.Color.White;
            appearance30.ForeColorDisabled = System.Drawing.Color.White;
            this.txtDepartmentDescription.Appearance = appearance30;
            this.txtDepartmentDescription.BorderStyleOuter = Infragistics.Win.UIElementBorderStyle.Solid;
            this.txtDepartmentDescription.Location = new System.Drawing.Point(260, 68);
            this.txtDepartmentDescription.Name = "txtDepartmentDescription";
            this.txtDepartmentDescription.Size = new System.Drawing.Size(160, 23);
            this.txtDepartmentDescription.TabIndex = 2;
            // 
            // ultraLabel5
            // 
            appearance31.FontData.BoldAsString = "False";
            appearance31.ForeColor = System.Drawing.Color.Black;
            this.ultraLabel5.Appearance = appearance31;
            this.ultraLabel5.Location = new System.Drawing.Point(7, 174);
            this.ultraLabel5.Name = "ultraLabel5";
            this.ultraLabel5.Size = new System.Drawing.Size(84, 19);
            this.ultraLabel5.TabIndex = 8;
            this.ultraLabel5.Text = "Item Type";
            this.ultraLabel5.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // ultraLabel3
            // 
            appearance32.FontData.BoldAsString = "False";
            appearance32.ForeColor = System.Drawing.Color.Black;
            this.ultraLabel3.Appearance = appearance32;
            this.ultraLabel3.Location = new System.Drawing.Point(7, 70);
            this.ultraLabel3.Name = "ultraLabel3";
            this.ultraLabel3.Size = new System.Drawing.Size(91, 19);
            this.ultraLabel3.TabIndex = 0;
            this.ultraLabel3.Text = "Department";
            this.ultraLabel3.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // ultraLabel4
            // 
            appearance33.FontData.BoldAsString = "False";
            appearance33.ForeColor = System.Drawing.Color.Black;
            this.ultraLabel4.Appearance = appearance33;
            this.ultraLabel4.Location = new System.Drawing.Point(7, 122);
            this.ultraLabel4.Name = "ultraLabel4";
            this.ultraLabel4.Size = new System.Drawing.Size(99, 19);
            this.ultraLabel4.TabIndex = 6;
            this.ultraLabel4.Text = "SKU Code";
            this.ultraLabel4.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // txtProductCode
            // 
            appearance34.FontData.BoldAsString = "False";
            appearance34.FontData.ItalicAsString = "False";
            appearance34.FontData.StrikeoutAsString = "False";
            appearance34.FontData.UnderlineAsString = "False";
            appearance34.ForeColor = System.Drawing.Color.Black;
            appearance34.ForeColorDisabled = System.Drawing.Color.Black;
            this.txtProductCode.Appearance = appearance34;
            this.txtProductCode.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.txtProductCode.Location = new System.Drawing.Point(128, 120);
            this.txtProductCode.MaxLength = 20;
            this.txtProductCode.Name = "txtProductCode";
            this.txtProductCode.Size = new System.Drawing.Size(132, 23);
            this.txtProductCode.TabIndex = 4;
            this.txtProductCode.Validated += new System.EventHandler(this.txtBoxs_Validate);
            // 
            // ultraLabel20
            // 
            appearance35.FontData.BoldAsString = "False";
            appearance35.ForeColor = System.Drawing.Color.Black;
            this.ultraLabel20.Appearance = appearance35;
            this.ultraLabel20.Location = new System.Drawing.Point(7, 96);
            this.ultraLabel20.Name = "ultraLabel20";
            this.ultraLabel20.Size = new System.Drawing.Size(126, 19);
            this.ultraLabel20.TabIndex = 3;
            this.ultraLabel20.Text = "Sub Department";
            this.ultraLabel20.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // ultraLabel9
            // 
            appearance36.FontData.BoldAsString = "False";
            appearance36.ForeColor = System.Drawing.Color.Black;
            this.ultraLabel9.Appearance = appearance36;
            this.ultraLabel9.Location = new System.Drawing.Point(7, 148);
            this.ultraLabel9.Name = "ultraLabel9";
            this.ultraLabel9.Size = new System.Drawing.Size(112, 19);
            this.ultraLabel9.TabIndex = 10;
            this.ultraLabel9.Text = "Sale Type Code";
            this.ultraLabel9.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.lblActualTaxToDisplay);
            this.groupBox3.Controls.Add(this.lblActualTax);
            this.groupBox3.Controls.Add(this.ultraLabel30);
            this.groupBox3.Controls.Add(this.cmbCLPointPolicy);
            this.groupBox3.Controls.Add(this.ultraLabel29);
            this.groupBox3.Controls.Add(this.numCLPointsPerDollar);
            this.groupBox3.Controls.Add(this.chkIsDefaultCLPoints);
            this.groupBox3.Controls.Add(this.cboTaxCodes);
            this.groupBox3.Controls.Add(this.cmbDiscountPolicy);
            this.groupBox3.Controls.Add(this.ultraLabel21);
            this.groupBox3.Controls.Add(this.cmbTaxPolicy);
            this.groupBox3.Controls.Add(this.chkUpdatePriceItemWise);
            this.groupBox3.Controls.Add(this.ultraLabel19);
            this.groupBox3.Controls.Add(this.ultraLabel18);
            this.groupBox3.Controls.Add(this.ultraLabel13);
            this.groupBox3.Controls.Add(this.numFreight);
            this.groupBox3.Controls.Add(this.chkIsTaxable);
            this.groupBox3.Controls.Add(this.numLastCostPrice);
            this.groupBox3.Controls.Add(this.numAveragePrice);
            this.groupBox3.Controls.Add(this.numSellingPrice);
            this.groupBox3.Controls.Add(this.numDiscountAmt);
            this.groupBox3.Controls.Add(this.ultraLabel11);
            this.groupBox3.Controls.Add(this.ultraLabel8);
            this.groupBox3.Controls.Add(this.ultraLabel10);
            this.groupBox3.Controls.Add(this.chkIsDiscountable);
            this.groupBox3.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.groupBox3.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox3.ForeColor = System.Drawing.Color.White;
            this.groupBox3.Location = new System.Drawing.Point(572, 4);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(349, 323);
            this.groupBox3.TabIndex = 2;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Pricing";
            // 
            // lblActualTaxToDisplay
            // 
            appearance37.FontData.BoldAsString = "False";
            appearance37.FontData.ItalicAsString = "False";
            appearance37.FontData.StrikeoutAsString = "False";
            appearance37.FontData.UnderlineAsString = "False";
            appearance37.ForeColor = System.Drawing.Color.White;
            appearance37.ForeColorDisabled = System.Drawing.Color.White;
            this.lblActualTaxToDisplay.Appearance = appearance37;
            this.lblActualTaxToDisplay.BorderStyleOuter = Infragistics.Win.UIElementBorderStyle.Solid;
            this.lblActualTaxToDisplay.Location = new System.Drawing.Point(126, 93);
            this.lblActualTaxToDisplay.Name = "lblActualTaxToDisplay";
            this.lblActualTaxToDisplay.Size = new System.Drawing.Size(217, 23);
            this.lblActualTaxToDisplay.TabIndex = 24;
            // 
            // lblActualTax
            // 
            appearance38.FontData.BoldAsString = "False";
            appearance38.FontData.ItalicAsString = "False";
            appearance38.FontData.StrikeoutAsString = "False";
            appearance38.FontData.UnderlineAsString = "False";
            appearance38.ForeColor = System.Drawing.Color.Black;
            appearance38.ForeColorDisabled = System.Drawing.Color.Black;
            this.lblActualTax.Appearance = appearance38;
            this.lblActualTax.Location = new System.Drawing.Point(10, 95);
            this.lblActualTax.Name = "lblActualTax";
            this.lblActualTax.Size = new System.Drawing.Size(103, 19);
            this.lblActualTax.TabIndex = 23;
            this.lblActualTax.Text = "Actual Tax";
            this.lblActualTax.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // ultraLabel30
            // 
            appearance39.FontData.BoldAsString = "False";
            appearance39.FontData.ItalicAsString = "False";
            appearance39.FontData.StrikeoutAsString = "False";
            appearance39.FontData.UnderlineAsString = "False";
            appearance39.ForeColor = System.Drawing.Color.Black;
            appearance39.ForeColorDisabled = System.Drawing.Color.Black;
            this.ultraLabel30.Appearance = appearance39;
            this.ultraLabel30.Location = new System.Drawing.Point(10, 199);
            this.ultraLabel30.Name = "ultraLabel30";
            this.ultraLabel30.Size = new System.Drawing.Size(103, 19);
            this.ultraLabel30.TabIndex = 22;
            this.ultraLabel30.Text = "CL Points";
            this.ultraLabel30.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // cmbCLPointPolicy
            // 
            appearance40.BorderAlpha = Infragistics.Win.Alpha.Opaque;
            appearance40.BorderColor3DBase = System.Drawing.Color.Black;
            appearance40.FontData.BoldAsString = "False";
            appearance40.FontData.ItalicAsString = "False";
            appearance40.FontData.StrikeoutAsString = "False";
            appearance40.FontData.UnderlineAsString = "False";
            appearance40.ForeColor = System.Drawing.Color.Black;
            appearance40.ForeColorDisabled = System.Drawing.Color.Black;
            this.cmbCLPointPolicy.Appearance = appearance40;
            this.cmbCLPointPolicy.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance41.BackColor = System.Drawing.Color.White;
            appearance41.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(231)))), ((int)(((byte)(230)))));
            appearance41.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance41.BorderAlpha = Infragistics.Win.Alpha.Transparent;
            this.cmbCLPointPolicy.ButtonAppearance = appearance41;
            this.cmbCLPointPolicy.DropDownStyle = Infragistics.Win.DropDownStyle.DropDownList;
            valueListItem7.DataValue = "ValueListItem0";
            valueListItem7.DisplayText = "Default CL Point Setting";
            valueListItem8.DataValue = "ValueListItem1";
            valueListItem8.DisplayText = "Item CL Point Setting";
            valueListItem9.DataValue = "ValueListItem2";
            valueListItem9.DisplayText = "Department CL Point Setting";
            valueListItem12.DataValue = "ValueListItem3";
            valueListItem12.DisplayText = "Sub-Dept CL Point Setting";
            this.cmbCLPointPolicy.Items.AddRange(new Infragistics.Win.ValueListItem[] {
            valueListItem7,
            valueListItem8,
            valueListItem9,
            valueListItem12});
            this.cmbCLPointPolicy.Location = new System.Drawing.Point(126, 171);
            this.cmbCLPointPolicy.MaxLength = 20;
            this.cmbCLPointPolicy.Name = "cmbCLPointPolicy";
            this.cmbCLPointPolicy.Size = new System.Drawing.Size(217, 23);
            this.cmbCLPointPolicy.TabIndex = 9;
            // 
            // ultraLabel29
            // 
            appearance42.FontData.BoldAsString = "False";
            appearance42.FontData.ItalicAsString = "False";
            appearance42.FontData.StrikeoutAsString = "False";
            appearance42.FontData.UnderlineAsString = "False";
            appearance42.ForeColor = System.Drawing.Color.Black;
            appearance42.ForeColorDisabled = System.Drawing.Color.Black;
            this.ultraLabel29.Appearance = appearance42;
            this.ultraLabel29.Location = new System.Drawing.Point(10, 173);
            this.ultraLabel29.Name = "ultraLabel29";
            this.ultraLabel29.Size = new System.Drawing.Size(103, 19);
            this.ultraLabel29.TabIndex = 21;
            this.ultraLabel29.Text = "Follow CL By";
            this.ultraLabel29.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // numCLPointsPerDollar
            // 
            appearance43.FontData.BoldAsString = "False";
            appearance43.FontData.ItalicAsString = "False";
            appearance43.FontData.StrikeoutAsString = "False";
            appearance43.FontData.UnderlineAsString = "False";
            appearance43.ForeColor = System.Drawing.Color.Black;
            appearance43.ForeColorDisabled = System.Drawing.Color.Black;
            this.numCLPointsPerDollar.Appearance = appearance43;
            this.numCLPointsPerDollar.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.numCLPointsPerDollar.Location = new System.Drawing.Point(126, 197);
            this.numCLPointsPerDollar.MaskInput = "nnnnnnn";
            this.numCLPointsPerDollar.MaxValue = 100;
            this.numCLPointsPerDollar.MinValue = -100;
            this.numCLPointsPerDollar.Name = "numCLPointsPerDollar";
            this.numCLPointsPerDollar.NumericType = Infragistics.Win.UltraWinEditors.NumericType.Double;
            this.numCLPointsPerDollar.Size = new System.Drawing.Size(111, 23);
            this.numCLPointsPerDollar.TabIndex = 11;
            this.numCLPointsPerDollar.Validated += new System.EventHandler(this.numBoxs_Validate);
            // 
            // chkIsDefaultCLPoints
            // 
            appearance44.FontData.BoldAsString = "False";
            appearance44.ForeColor = System.Drawing.Color.Black;
            appearance44.ForeColorDisabled = System.Drawing.Color.White;
            this.chkIsDefaultCLPoints.Appearance = appearance44;
            this.chkIsDefaultCLPoints.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkIsDefaultCLPoints.Location = new System.Drawing.Point(240, 173);
            this.chkIsDefaultCLPoints.Name = "chkIsDefaultCLPoints";
            this.chkIsDefaultCLPoints.Size = new System.Drawing.Size(5, 5);
            this.chkIsDefaultCLPoints.TabIndex = 10;
            this.chkIsDefaultCLPoints.Text = "Default CL Points?";
            this.chkIsDefaultCLPoints.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.chkIsDefaultCLPoints.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.chkIsDefaultCLPoints.Visible = false;
            this.chkIsDefaultCLPoints.CheckedChanged += new System.EventHandler(this.CheckBoxs_Checked);
            // 
            // cboTaxCodes
            // 
            this.cboTaxCodes.CheckedListSettings.CheckBoxStyle = Infragistics.Win.CheckStyle.CheckBox;
            this.cboTaxCodes.CheckedListSettings.ItemCheckArea = Infragistics.Win.ItemCheckArea.Item;
            this.cboTaxCodes.CheckedListSettings.ListSeparator = ", ";
            this.cboTaxCodes.Location = new System.Drawing.Point(126, 41);
            this.cboTaxCodes.Name = "cboTaxCodes";
            this.cboTaxCodes.Size = new System.Drawing.Size(217, 25);
            this.cboTaxCodes.TabIndex = 2;
            this.cboTaxCodes.BeforeDropDown += new System.ComponentModel.CancelEventHandler(this.cboTaxCodes_BeforeDropDown);
            this.cboTaxCodes.AfterCloseUp += new System.EventHandler(this.cboTaxCodes_AfterCloseUp);
            this.cboTaxCodes.TextChanged += new System.EventHandler(this.cboTaxCodes_TextChanged);
            // 
            // cmbDiscountPolicy
            // 
            appearance45.BorderAlpha = Infragistics.Win.Alpha.Opaque;
            appearance45.BorderColor3DBase = System.Drawing.Color.Black;
            appearance45.FontData.BoldAsString = "False";
            appearance45.FontData.ItalicAsString = "False";
            appearance45.FontData.StrikeoutAsString = "False";
            appearance45.FontData.UnderlineAsString = "False";
            appearance45.ForeColor = System.Drawing.Color.Black;
            appearance45.ForeColorDisabled = System.Drawing.Color.Black;
            this.cmbDiscountPolicy.Appearance = appearance45;
            this.cmbDiscountPolicy.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance46.BackColor = System.Drawing.Color.White;
            appearance46.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(231)))), ((int)(((byte)(230)))));
            appearance46.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance46.BorderAlpha = Infragistics.Win.Alpha.Transparent;
            this.cmbDiscountPolicy.ButtonAppearance = appearance46;
            this.cmbDiscountPolicy.DropDownStyle = Infragistics.Win.DropDownStyle.DropDownList;
            valueListItem4.DataValue = "I";
            valueListItem4.DisplayText = "Item";
            valueListItem5.DataValue = "D";
            valueListItem5.DisplayText = "Department";
            this.cmbDiscountPolicy.Items.AddRange(new Infragistics.Win.ValueListItem[] {
            valueListItem4,
            valueListItem5});
            this.cmbDiscountPolicy.Location = new System.Drawing.Point(126, 145);
            this.cmbDiscountPolicy.MaxLength = 20;
            this.cmbDiscountPolicy.Name = "cmbDiscountPolicy";
            this.cmbDiscountPolicy.Size = new System.Drawing.Size(217, 23);
            this.cmbDiscountPolicy.TabIndex = 8;
            this.cmbDiscountPolicy.ValueChanged += new System.EventHandler(this.cmbDiscountPolicy_ValueChanged);
            // 
            // ultraLabel21
            // 
            appearance47.FontData.BoldAsString = "False";
            appearance47.FontData.ItalicAsString = "False";
            appearance47.FontData.StrikeoutAsString = "False";
            appearance47.FontData.UnderlineAsString = "False";
            appearance47.ForeColor = System.Drawing.Color.Black;
            appearance47.ForeColorDisabled = System.Drawing.Color.Black;
            this.ultraLabel21.Appearance = appearance47;
            this.ultraLabel21.Location = new System.Drawing.Point(10, 147);
            this.ultraLabel21.Name = "ultraLabel21";
            this.ultraLabel21.Size = new System.Drawing.Size(120, 19);
            this.ultraLabel21.TabIndex = 17;
            this.ultraLabel21.Text = "Apply Disc Type";
            this.ultraLabel21.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // cmbTaxPolicy
            // 
            appearance48.BorderAlpha = Infragistics.Win.Alpha.Opaque;
            appearance48.BorderColor3DBase = System.Drawing.Color.Black;
            appearance48.FontData.BoldAsString = "False";
            appearance48.FontData.ItalicAsString = "False";
            appearance48.FontData.StrikeoutAsString = "False";
            appearance48.FontData.UnderlineAsString = "False";
            appearance48.ForeColor = System.Drawing.Color.Black;
            appearance48.ForeColorDisabled = System.Drawing.Color.Black;
            this.cmbTaxPolicy.Appearance = appearance48;
            this.cmbTaxPolicy.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance49.BackColor = System.Drawing.Color.White;
            appearance49.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(231)))), ((int)(((byte)(230)))));
            appearance49.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance49.BorderAlpha = Infragistics.Win.Alpha.Transparent;
            this.cmbTaxPolicy.ButtonAppearance = appearance49;
            this.cmbTaxPolicy.DropDownStyle = Infragistics.Win.DropDownStyle.DropDownList;
            valueListItem6.DataValue = "ValueListItem0";
            valueListItem6.DisplayText = "Item Tax Setting";
            valueListItem10.DataValue = "ValueListItem1";
            valueListItem10.DisplayText = "Department Tax Setting";
            valueListItem11.DataValue = "ValueListItem2";
            valueListItem11.DisplayText = "Dept Setting if dept is Taxable";
            this.cmbTaxPolicy.Items.AddRange(new Infragistics.Win.ValueListItem[] {
            valueListItem6,
            valueListItem10,
            valueListItem11});
            this.cmbTaxPolicy.Location = new System.Drawing.Point(126, 68);
            this.cmbTaxPolicy.MaxLength = 20;
            this.cmbTaxPolicy.Name = "cmbTaxPolicy";
            this.cmbTaxPolicy.Size = new System.Drawing.Size(217, 23);
            this.cmbTaxPolicy.TabIndex = 4;
            this.cmbTaxPolicy.ValueChanged += new System.EventHandler(this.cmbTaxPolicy_ValueChanged);
            // 
            // chkUpdatePriceItemWise
            // 
            appearance50.FontData.BoldAsString = "False";
            appearance50.ForeColor = System.Drawing.Color.Black;
            appearance50.ForeColorDisabled = System.Drawing.Color.White;
            this.chkUpdatePriceItemWise.Appearance = appearance50;
            this.chkUpdatePriceItemWise.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkUpdatePriceItemWise.Checked = true;
            this.chkUpdatePriceItemWise.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkUpdatePriceItemWise.Location = new System.Drawing.Point(10, 301);
            this.chkUpdatePriceItemWise.Name = "chkUpdatePriceItemWise";
            this.chkUpdatePriceItemWise.Size = new System.Drawing.Size(131, 19);
            this.chkUpdatePriceItemWise.TabIndex = 15;
            this.chkUpdatePriceItemWise.Text = "Update Price";
            this.chkUpdatePriceItemWise.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.chkUpdatePriceItemWise.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.chkUpdatePriceItemWise.CheckedChanged += new System.EventHandler(this.CheckBoxs_Checked);
            // 
            // ultraLabel19
            // 
            appearance51.FontData.BoldAsString = "False";
            appearance51.FontData.ItalicAsString = "False";
            appearance51.FontData.StrikeoutAsString = "False";
            appearance51.FontData.UnderlineAsString = "False";
            appearance51.ForeColor = System.Drawing.Color.Black;
            appearance51.ForeColorDisabled = System.Drawing.Color.Black;
            this.ultraLabel19.Appearance = appearance51;
            this.ultraLabel19.Location = new System.Drawing.Point(10, 70);
            this.ultraLabel19.Name = "ultraLabel19";
            this.ultraLabel19.Size = new System.Drawing.Size(103, 19);
            this.ultraLabel19.TabIndex = 3;
            this.ultraLabel19.Text = "Follow Tax By";
            this.ultraLabel19.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // ultraLabel18
            // 
            appearance52.ForeColor = System.Drawing.Color.White;
            appearance52.TextHAlignAsString = "Center";
            appearance52.TextVAlignAsString = "Middle";
            this.ultraLabel18.Appearance = appearance52;
            this.ultraLabel18.BorderStyleInner = Infragistics.Win.UIElementBorderStyle.Solid;
            this.ultraLabel18.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultraLabel18.Location = new System.Drawing.Point(242, 119);
            this.ultraLabel18.Name = "ultraLabel18";
            this.ultraLabel18.Size = new System.Drawing.Size(29, 23);
            this.ultraLabel18.TabIndex = 7;
            this.ultraLabel18.Text = "%";
            this.ultraLabel18.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // ultraLabel13
            // 
            appearance53.FontData.BoldAsString = "False";
            appearance53.ForeColor = System.Drawing.Color.Black;
            this.ultraLabel13.Appearance = appearance53;
            this.ultraLabel13.Location = new System.Drawing.Point(10, 225);
            this.ultraLabel13.Name = "ultraLabel13";
            this.ultraLabel13.Size = new System.Drawing.Size(56, 19);
            this.ultraLabel13.TabIndex = 2;
            this.ultraLabel13.Text = "Freight";
            this.ultraLabel13.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // numFreight
            // 
            appearance54.FontData.BoldAsString = "False";
            appearance54.FontData.ItalicAsString = "False";
            appearance54.FontData.StrikeoutAsString = "False";
            appearance54.FontData.UnderlineAsString = "False";
            appearance54.ForeColor = System.Drawing.Color.Black;
            appearance54.ForeColorDisabled = System.Drawing.Color.Black;
            this.numFreight.Appearance = appearance54;
            this.numFreight.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.numFreight.Location = new System.Drawing.Point(126, 223);
            this.numFreight.MaskInput = "{LOC}-nn,nnn.nn";
            this.numFreight.MaxValue = 1316134911;
            this.numFreight.MinValue = -9999999;
            this.numFreight.Name = "numFreight";
            this.numFreight.NumericType = Infragistics.Win.UltraWinEditors.NumericType.Double;
            this.numFreight.Size = new System.Drawing.Size(111, 23);
            this.numFreight.TabIndex = 12;
            this.numFreight.Validated += new System.EventHandler(this.numBoxs_Validate);
            // 
            // chkIsTaxable
            // 
            appearance55.FontData.BoldAsString = "False";
            appearance55.ForeColor = System.Drawing.Color.Black;
            this.chkIsTaxable.Appearance = appearance55;
            this.chkIsTaxable.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkIsTaxable.Location = new System.Drawing.Point(10, 44);
            this.chkIsTaxable.Name = "chkIsTaxable";
            this.chkIsTaxable.Size = new System.Drawing.Size(112, 19);
            this.chkIsTaxable.TabIndex = 1;
            this.chkIsTaxable.Text = "Taxable? *";
            this.chkIsTaxable.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.chkIsTaxable.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.chkIsTaxable.CheckedChanged += new System.EventHandler(this.CheckBoxs_Checked);
            // 
            // numLastCostPrice
            // 
            appearance56.FontData.BoldAsString = "False";
            appearance56.FontData.ItalicAsString = "False";
            appearance56.FontData.StrikeoutAsString = "False";
            appearance56.FontData.UnderlineAsString = "False";
            appearance56.ForeColor = System.Drawing.Color.Black;
            appearance56.ForeColorDisabled = System.Drawing.Color.Black;
            this.numLastCostPrice.Appearance = appearance56;
            this.numLastCostPrice.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.numLastCostPrice.Location = new System.Drawing.Point(126, 249);
            this.numLastCostPrice.MaskInput = "{LOC}-nn,nnn.nn";
            this.numLastCostPrice.MaxValue = 9999999;
            this.numLastCostPrice.MinValue = -999999;
            this.numLastCostPrice.Name = "numLastCostPrice";
            this.numLastCostPrice.NumericType = Infragistics.Win.UltraWinEditors.NumericType.Double;
            this.numLastCostPrice.Size = new System.Drawing.Size(111, 23);
            this.numLastCostPrice.TabIndex = 13;
            this.numLastCostPrice.ValueChanged += new System.EventHandler(this.numLastCostPrice_ValueChanged);
            this.numLastCostPrice.Validated += new System.EventHandler(this.numBoxs_Validate);
            // 
            // numAveragePrice
            // 
            appearance57.FontData.BoldAsString = "False";
            appearance57.FontData.ItalicAsString = "False";
            appearance57.FontData.StrikeoutAsString = "False";
            appearance57.FontData.UnderlineAsString = "False";
            appearance57.ForeColor = System.Drawing.Color.Black;
            appearance57.ForeColorDisabled = System.Drawing.Color.Black;
            this.numAveragePrice.Appearance = appearance57;
            this.numAveragePrice.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.numAveragePrice.Location = new System.Drawing.Point(126, 275);
            this.numAveragePrice.MaskInput = "{LOC}-nn,nnn.nn";
            this.numAveragePrice.MaxValue = 99999999;
            this.numAveragePrice.MinValue = -9999999;
            this.numAveragePrice.Name = "numAveragePrice";
            this.numAveragePrice.NumericType = Infragistics.Win.UltraWinEditors.NumericType.Double;
            this.numAveragePrice.Size = new System.Drawing.Size(111, 23);
            this.numAveragePrice.TabIndex = 14;
            this.numAveragePrice.Validated += new System.EventHandler(this.numBoxs_Validate);
            // 
            // numSellingPrice
            // 
            appearance58.FontData.BoldAsString = "False";
            appearance58.FontData.ItalicAsString = "False";
            appearance58.FontData.StrikeoutAsString = "False";
            appearance58.FontData.UnderlineAsString = "False";
            appearance58.ForeColor = System.Drawing.Color.Black;
            appearance58.ForeColorDisabled = System.Drawing.Color.Black;
            this.numSellingPrice.Appearance = appearance58;
            this.numSellingPrice.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.numSellingPrice.Location = new System.Drawing.Point(126, 16);
            this.numSellingPrice.MaskInput = "{LOC}-nn,nnn.nn";
            this.numSellingPrice.MaxValue = 99999999;
            this.numSellingPrice.MinValue = -9999999;
            this.numSellingPrice.Name = "numSellingPrice";
            this.numSellingPrice.NumericType = Infragistics.Win.UltraWinEditors.NumericType.Double;
            this.numSellingPrice.Size = new System.Drawing.Size(111, 23);
            this.numSellingPrice.TabIndex = 0;
            this.numSellingPrice.Validated += new System.EventHandler(this.numBoxs_Validate);
            // 
            // numDiscountAmt
            // 
            appearance59.FontData.BoldAsString = "False";
            appearance59.FontData.ItalicAsString = "False";
            appearance59.FontData.StrikeoutAsString = "False";
            appearance59.FontData.UnderlineAsString = "False";
            appearance59.ForeColor = System.Drawing.Color.Black;
            appearance59.ForeColorDisabled = System.Drawing.Color.Black;
            this.numDiscountAmt.Appearance = appearance59;
            this.numDiscountAmt.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.numDiscountAmt.Location = new System.Drawing.Point(126, 119);
            this.numDiscountAmt.MaskInput = "{LOC}-nnn.nn";
            this.numDiscountAmt.MaxValue = 100;
            this.numDiscountAmt.MinValue = 0D;
            this.numDiscountAmt.Name = "numDiscountAmt";
            this.numDiscountAmt.NumericType = Infragistics.Win.UltraWinEditors.NumericType.Double;
            this.numDiscountAmt.Size = new System.Drawing.Size(111, 23);
            this.numDiscountAmt.TabIndex = 6;
            this.numDiscountAmt.Validated += new System.EventHandler(this.numBoxs_Validate);
            // 
            // ultraLabel11
            // 
            appearance60.FontData.BoldAsString = "False";
            appearance60.ForeColor = System.Drawing.Color.Black;
            this.ultraLabel11.Appearance = appearance60;
            this.ultraLabel11.Location = new System.Drawing.Point(10, 18);
            this.ultraLabel11.Name = "ultraLabel11";
            this.ultraLabel11.Size = new System.Drawing.Size(92, 19);
            this.ultraLabel11.TabIndex = 7;
            this.ultraLabel11.Text = "Selling Price";
            this.ultraLabel11.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // ultraLabel8
            // 
            appearance61.FontData.BoldAsString = "False";
            appearance61.ForeColor = System.Drawing.Color.Black;
            this.ultraLabel8.Appearance = appearance61;
            this.ultraLabel8.Location = new System.Drawing.Point(10, 251);
            this.ultraLabel8.Name = "ultraLabel8";
            this.ultraLabel8.Size = new System.Drawing.Size(108, 19);
            this.ultraLabel8.TabIndex = 11;
            this.ultraLabel8.Text = "Last Cost Price";
            this.ultraLabel8.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // ultraLabel10
            // 
            appearance62.FontData.BoldAsString = "False";
            appearance62.ForeColor = System.Drawing.Color.Black;
            this.ultraLabel10.Appearance = appearance62;
            this.ultraLabel10.Location = new System.Drawing.Point(10, 279);
            this.ultraLabel10.Name = "ultraLabel10";
            this.ultraLabel10.Size = new System.Drawing.Size(103, 19);
            this.ultraLabel10.TabIndex = 9;
            this.ultraLabel10.Text = "Average Price";
            this.ultraLabel10.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // chkIsDiscountable
            // 
            appearance63.FontData.BoldAsString = "False";
            appearance63.ForeColor = System.Drawing.Color.Black;
            appearance63.ForeColorDisabled = System.Drawing.Color.White;
            this.chkIsDiscountable.Appearance = appearance63;
            this.chkIsDiscountable.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkIsDiscountable.Location = new System.Drawing.Point(10, 121);
            this.chkIsDiscountable.Name = "chkIsDiscountable";
            this.chkIsDiscountable.Size = new System.Drawing.Size(112, 19);
            this.chkIsDiscountable.TabIndex = 5;
            this.chkIsDiscountable.Text = "Discountable?";
            this.chkIsDiscountable.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.chkIsDiscountable.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.chkIsDiscountable.CheckedChanged += new System.EventHandler(this.CheckBoxs_Checked);
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.btnRefreshDataFromMMS);
            this.groupBox5.Controls.Add(this.dtpExpiryDate);
            this.groupBox5.Controls.Add(this.lblExpiryDate);
            this.groupBox5.Controls.Add(this.numMinQty);
            this.groupBox5.Controls.Add(this.grpItemPacketInfo);
            this.groupBox5.Controls.Add(this.chkPacketSize);
            this.groupBox5.Controls.Add(this.numQtyInStock);
            this.groupBox5.Controls.Add(this.numQtyOnOrder);
            this.groupBox5.Controls.Add(this.numReorderLevel);
            this.groupBox5.Controls.Add(this.dtpExpectedDelDate);
            this.groupBox5.Controls.Add(this.ultraLabel27);
            this.groupBox5.Controls.Add(this.txtLocation);
            this.groupBox5.Controls.Add(this.ultraLabel22);
            this.groupBox5.Controls.Add(this.ultraLabel25);
            this.groupBox5.Controls.Add(this.ultraLabel24);
            this.groupBox5.Controls.Add(this.ultraLabel23);
            this.groupBox5.Controls.Add(this.ultraLabel26);
            this.groupBox5.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.groupBox5.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox5.ForeColor = System.Drawing.Color.White;
            this.groupBox5.Location = new System.Drawing.Point(6, 308);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(560, 206);
            this.groupBox5.TabIndex = 1;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Inventory";
            this.groupBox5.Enter += new System.EventHandler(this.groupBox5_Enter);
            // 
            // btnRefreshDataFromMMS
            // 
            appearance64.BackColor = System.Drawing.Color.White;
            appearance64.ForeColor = System.Drawing.Color.Black;
            this.btnRefreshDataFromMMS.Appearance = appearance64;
            this.btnRefreshDataFromMMS.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Flat;
            this.btnRefreshDataFromMMS.Font = new System.Drawing.Font("Verdana", 9F);
            this.btnRefreshDataFromMMS.Location = new System.Drawing.Point(446, 156);
            this.btnRefreshDataFromMMS.Name = "btnRefreshDataFromMMS";
            this.btnRefreshDataFromMMS.Size = new System.Drawing.Size(108, 40);
            this.btnRefreshDataFromMMS.TabIndex = 13;
            this.btnRefreshDataFromMMS.TabStop = false;
            this.btnRefreshDataFromMMS.Text = "Refresh Data From MMS";
            this.btnRefreshDataFromMMS.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnRefreshDataFromMMS.Visible = false;
            this.btnRefreshDataFromMMS.Click += new System.EventHandler(this.btnRefreshDataFromMMS_Click);
            // 
            // dtpExpiryDate
            // 
            appearance65.FontData.BoldAsString = "False";
            appearance65.FontData.ItalicAsString = "False";
            appearance65.FontData.StrikeoutAsString = "False";
            appearance65.FontData.UnderlineAsString = "False";
            appearance65.ForeColor = System.Drawing.Color.Black;
            appearance65.ForeColorDisabled = System.Drawing.Color.Black;
            this.dtpExpiryDate.Appearance = appearance65;
            this.dtpExpiryDate.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Office2003ToolbarButton;
            this.dtpExpiryDate.DateButtons.Add(dateButton1);
            this.dtpExpiryDate.Location = new System.Drawing.Point(122, 171);
            this.dtpExpiryDate.Name = "dtpExpiryDate";
            this.dtpExpiryDate.NonAutoSizeHeight = 10;
            this.dtpExpiryDate.Size = new System.Drawing.Size(132, 22);
            this.dtpExpiryDate.TabIndex = 6;
            this.dtpExpiryDate.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.dtpExpiryDate.Value = new System.DateTime(2004, 5, 25, 0, 0, 0, 0);
            this.dtpExpiryDate.Validated += new System.EventHandler(this.dtpBoxs_Validate);
            // 
            // lblExpiryDate
            // 
            appearance66.FontData.BoldAsString = "False";
            appearance66.FontData.ItalicAsString = "False";
            appearance66.FontData.StrikeoutAsString = "False";
            appearance66.FontData.UnderlineAsString = "False";
            appearance66.ForeColor = System.Drawing.Color.Black;
            appearance66.ForeColorDisabled = System.Drawing.Color.Black;
            this.lblExpiryDate.Appearance = appearance66;
            this.lblExpiryDate.Location = new System.Drawing.Point(10, 173);
            this.lblExpiryDate.Name = "lblExpiryDate";
            this.lblExpiryDate.Size = new System.Drawing.Size(106, 19);
            this.lblExpiryDate.TabIndex = 12;
            this.lblExpiryDate.Text = "Exp. Date";
            this.lblExpiryDate.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // numMinQty
            // 
            appearance67.FontData.BoldAsString = "False";
            appearance67.FontData.ItalicAsString = "False";
            appearance67.FontData.StrikeoutAsString = "False";
            appearance67.FontData.UnderlineAsString = "False";
            appearance67.ForeColor = System.Drawing.Color.Black;
            appearance67.ForeColorDisabled = System.Drawing.Color.Black;
            this.numMinQty.Appearance = appearance67;
            this.numMinQty.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.numMinQty.Location = new System.Drawing.Point(122, 42);
            this.numMinQty.MaskInput = "-nn,nnn";
            this.numMinQty.MaxValue = 9999999;
            this.numMinQty.MinValue = -999999;
            this.numMinQty.Name = "numMinQty";
            this.numMinQty.Size = new System.Drawing.Size(132, 23);
            this.numMinQty.TabIndex = 1;
            this.numMinQty.Validated += new System.EventHandler(this.numBoxs_Validate);
            // 
            // grpItemPacketInfo
            // 
            this.grpItemPacketInfo.Controls.Add(this.numCaseCost);
            this.grpItemPacketInfo.Controls.Add(this.lblCaseCost);
            this.grpItemPacketInfo.Controls.Add(this.txtPacketSize);
            this.grpItemPacketInfo.Controls.Add(this.txtPacketQuantity);
            this.grpItemPacketInfo.Controls.Add(this.txtPacketUnit);
            this.grpItemPacketInfo.Controls.Add(this.lblPacketUnit);
            this.grpItemPacketInfo.Controls.Add(this.lblPacketQuantity);
            this.grpItemPacketInfo.Controls.Add(this.lblPacketSize);
            this.grpItemPacketInfo.Enabled = false;
            this.grpItemPacketInfo.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.grpItemPacketInfo.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpItemPacketInfo.ForeColor = System.Drawing.Color.White;
            this.grpItemPacketInfo.Location = new System.Drawing.Point(260, 17);
            this.grpItemPacketInfo.Name = "grpItemPacketInfo";
            this.grpItemPacketInfo.Size = new System.Drawing.Size(294, 125);
            this.grpItemPacketInfo.TabIndex = 7;
            this.grpItemPacketInfo.TabStop = false;
            // 
            // numCaseCost
            // 
            appearance68.FontData.BoldAsString = "False";
            appearance68.FontData.ItalicAsString = "False";
            appearance68.FontData.StrikeoutAsString = "False";
            appearance68.FontData.UnderlineAsString = "False";
            appearance68.ForeColor = System.Drawing.Color.Black;
            appearance68.ForeColorDisabled = System.Drawing.Color.Black;
            this.numCaseCost.Appearance = appearance68;
            this.numCaseCost.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.numCaseCost.Location = new System.Drawing.Point(133, 94);
            this.numCaseCost.MaskInput = "{LOC}-nn,nnn.nn";
            this.numCaseCost.MaxValue = 99999999;
            this.numCaseCost.MinValue = -9999999;
            this.numCaseCost.Name = "numCaseCost";
            this.numCaseCost.NumericType = Infragistics.Win.UltraWinEditors.NumericType.Double;
            this.numCaseCost.Size = new System.Drawing.Size(132, 23);
            this.numCaseCost.TabIndex = 4;
            this.numCaseCost.Visible = false;
            this.numCaseCost.ValueChanged += new System.EventHandler(this.numCaseCost_ValueChanged);
            // 
            // lblCaseCost
            // 
            appearance69.FontData.BoldAsString = "False";
            appearance69.ForeColor = System.Drawing.Color.Black;
            this.lblCaseCost.Appearance = appearance69;
            this.lblCaseCost.Location = new System.Drawing.Point(14, 96);
            this.lblCaseCost.Name = "lblCaseCost";
            this.lblCaseCost.Size = new System.Drawing.Size(86, 19);
            this.lblCaseCost.TabIndex = 5;
            this.lblCaseCost.Text = "Case Cost";
            this.lblCaseCost.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.lblCaseCost.Visible = false;
            // 
            // txtPacketSize
            // 
            appearance70.FontData.BoldAsString = "False";
            appearance70.FontData.ItalicAsString = "False";
            appearance70.FontData.StrikeoutAsString = "False";
            appearance70.FontData.UnderlineAsString = "False";
            appearance70.ForeColor = System.Drawing.Color.Black;
            appearance70.ForeColorDisabled = System.Drawing.Color.Black;
            this.txtPacketSize.Appearance = appearance70;
            this.txtPacketSize.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.txtPacketSize.Location = new System.Drawing.Point(133, 13);
            this.txtPacketSize.MaxLength = 20;
            this.txtPacketSize.Name = "txtPacketSize";
            this.txtPacketSize.Size = new System.Drawing.Size(132, 23);
            this.txtPacketSize.TabIndex = 1;
            this.toolTip1.SetToolTip(this.txtPacketSize, "Press F4 To Search");
            this.txtPacketSize.ValueChanged += new System.EventHandler(this.txtPacketSize_ValueChanged);
            this.txtPacketSize.Validated += new System.EventHandler(this.txtBoxs_Validate);
            // 
            // txtPacketQuantity
            // 
            appearance71.FontData.BoldAsString = "False";
            appearance71.FontData.ItalicAsString = "False";
            appearance71.FontData.StrikeoutAsString = "False";
            appearance71.FontData.UnderlineAsString = "False";
            appearance71.ForeColor = System.Drawing.Color.Black;
            appearance71.ForeColorDisabled = System.Drawing.Color.Black;
            this.txtPacketQuantity.Appearance = appearance71;
            this.txtPacketQuantity.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.txtPacketQuantity.Location = new System.Drawing.Point(133, 40);
            this.txtPacketQuantity.MaxLength = 20;
            this.txtPacketQuantity.Name = "txtPacketQuantity";
            this.txtPacketQuantity.Size = new System.Drawing.Size(132, 23);
            this.txtPacketQuantity.TabIndex = 2;
            this.toolTip1.SetToolTip(this.txtPacketQuantity, "Press F4 To Search");
            this.txtPacketQuantity.Validated += new System.EventHandler(this.txtBoxs_Validate);
            // 
            // txtPacketUnit
            // 
            appearance72.FontData.BoldAsString = "False";
            appearance72.FontData.ItalicAsString = "False";
            appearance72.FontData.StrikeoutAsString = "False";
            appearance72.FontData.UnderlineAsString = "False";
            appearance72.ForeColor = System.Drawing.Color.Black;
            appearance72.ForeColorDisabled = System.Drawing.Color.Black;
            this.txtPacketUnit.Appearance = appearance72;
            this.txtPacketUnit.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.txtPacketUnit.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtPacketUnit.Location = new System.Drawing.Point(133, 67);
            this.txtPacketUnit.MaxLength = 2;
            this.txtPacketUnit.Name = "txtPacketUnit";
            this.txtPacketUnit.Size = new System.Drawing.Size(132, 23);
            this.txtPacketUnit.TabIndex = 3;
            this.toolTip1.SetToolTip(this.txtPacketUnit, "Press F4 To Search");
            this.txtPacketUnit.ValueChanged += new System.EventHandler(this.txtPacketUnit_ValueChanged);
            this.txtPacketUnit.Validated += new System.EventHandler(this.txtBoxs_Validate);
            // 
            // lblPacketUnit
            // 
            appearance73.FontData.BoldAsString = "False";
            appearance73.ForeColor = System.Drawing.Color.Black;
            this.lblPacketUnit.Appearance = appearance73;
            this.lblPacketUnit.Location = new System.Drawing.Point(14, 69);
            this.lblPacketUnit.Name = "lblPacketUnit";
            this.lblPacketUnit.Size = new System.Drawing.Size(85, 19);
            this.lblPacketUnit.TabIndex = 4;
            this.lblPacketUnit.Text = "Packet Unit";
            this.lblPacketUnit.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // lblPacketQuantity
            // 
            appearance74.FontData.BoldAsString = "False";
            appearance74.ForeColor = System.Drawing.Color.Black;
            this.lblPacketQuantity.Appearance = appearance74;
            this.lblPacketQuantity.Location = new System.Drawing.Point(13, 42);
            this.lblPacketQuantity.Name = "lblPacketQuantity";
            this.lblPacketQuantity.Size = new System.Drawing.Size(117, 19);
            this.lblPacketQuantity.TabIndex = 2;
            this.lblPacketQuantity.Text = "Packet Quantity";
            this.lblPacketQuantity.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // lblPacketSize
            // 
            appearance75.FontData.BoldAsString = "False";
            appearance75.ForeColor = System.Drawing.Color.Black;
            this.lblPacketSize.Appearance = appearance75;
            this.lblPacketSize.Location = new System.Drawing.Point(13, 15);
            this.lblPacketSize.Name = "lblPacketSize";
            this.lblPacketSize.Size = new System.Drawing.Size(86, 19);
            this.lblPacketSize.TabIndex = 0;
            this.lblPacketSize.Text = "Packet Size";
            this.lblPacketSize.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // chkPacketSize
            // 
            appearance76.FontData.BoldAsString = "True";
            appearance76.ForeColor = System.Drawing.Color.Black;
            appearance76.ForeColorDisabled = System.Drawing.Color.White;
            this.chkPacketSize.Appearance = appearance76;
            this.chkPacketSize.Location = new System.Drawing.Point(260, 3);
            this.chkPacketSize.Name = "chkPacketSize";
            this.chkPacketSize.Size = new System.Drawing.Size(149, 15);
            this.chkPacketSize.TabIndex = 0;
            this.chkPacketSize.Text = "Edit Packet Size";
            this.chkPacketSize.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.chkPacketSize.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.chkPacketSize.CheckedChanged += new System.EventHandler(this.CheckBoxs_Checked);
            // 
            // numQtyInStock
            // 
            appearance77.FontData.BoldAsString = "False";
            appearance77.FontData.ItalicAsString = "False";
            appearance77.FontData.StrikeoutAsString = "False";
            appearance77.FontData.UnderlineAsString = "False";
            appearance77.ForeColor = System.Drawing.Color.Black;
            appearance77.ForeColorDisabled = System.Drawing.Color.Black;
            this.numQtyInStock.Appearance = appearance77;
            this.numQtyInStock.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.numQtyInStock.Enabled = false;
            this.numQtyInStock.Location = new System.Drawing.Point(122, 16);
            this.numQtyInStock.MaskInput = "-nn,nnn";
            this.numQtyInStock.MaxValue = 99999999;
            this.numQtyInStock.MinValue = -9999999;
            this.numQtyInStock.Name = "numQtyInStock";
            this.numQtyInStock.Size = new System.Drawing.Size(132, 23);
            this.numQtyInStock.TabIndex = 0;
            this.numQtyInStock.Validated += new System.EventHandler(this.numBoxs_Validate);
            // 
            // numQtyOnOrder
            // 
            appearance78.FontData.BoldAsString = "False";
            appearance78.FontData.ItalicAsString = "False";
            appearance78.FontData.StrikeoutAsString = "False";
            appearance78.FontData.UnderlineAsString = "False";
            appearance78.ForeColor = System.Drawing.Color.Black;
            appearance78.ForeColorDisabled = System.Drawing.Color.Black;
            this.numQtyOnOrder.Appearance = appearance78;
            this.numQtyOnOrder.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.numQtyOnOrder.Location = new System.Drawing.Point(122, 94);
            this.numQtyOnOrder.MaskInput = "-nn,nnn";
            this.numQtyOnOrder.MaxValue = 99999999;
            this.numQtyOnOrder.MinValue = -9999999;
            this.numQtyOnOrder.Name = "numQtyOnOrder";
            this.numQtyOnOrder.Size = new System.Drawing.Size(132, 23);
            this.numQtyOnOrder.TabIndex = 3;
            this.numQtyOnOrder.Validated += new System.EventHandler(this.numBoxs_Validate);
            // 
            // numReorderLevel
            // 
            appearance79.FontData.BoldAsString = "False";
            appearance79.FontData.ItalicAsString = "False";
            appearance79.FontData.StrikeoutAsString = "False";
            appearance79.FontData.UnderlineAsString = "False";
            appearance79.ForeColor = System.Drawing.Color.Black;
            appearance79.ForeColorDisabled = System.Drawing.Color.Black;
            this.numReorderLevel.Appearance = appearance79;
            this.numReorderLevel.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.numReorderLevel.Location = new System.Drawing.Point(122, 68);
            this.numReorderLevel.MaskInput = "-nn,nnn";
            this.numReorderLevel.MaxValue = 99999;
            this.numReorderLevel.MinValue = -99999;
            this.numReorderLevel.Name = "numReorderLevel";
            this.numReorderLevel.Size = new System.Drawing.Size(132, 23);
            this.numReorderLevel.TabIndex = 2;
            this.numReorderLevel.Validated += new System.EventHandler(this.numBoxs_Validate);
            // 
            // dtpExpectedDelDate
            // 
            appearance80.FontData.BoldAsString = "False";
            appearance80.FontData.ItalicAsString = "False";
            appearance80.FontData.StrikeoutAsString = "False";
            appearance80.FontData.UnderlineAsString = "False";
            appearance80.ForeColor = System.Drawing.Color.Black;
            appearance80.ForeColorDisabled = System.Drawing.Color.Black;
            this.dtpExpectedDelDate.Appearance = appearance80;
            this.dtpExpectedDelDate.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Office2003ToolbarButton;
            this.dtpExpectedDelDate.DateButtons.Add(dateButton2);
            this.dtpExpectedDelDate.Location = new System.Drawing.Point(122, 146);
            this.dtpExpectedDelDate.Name = "dtpExpectedDelDate";
            this.dtpExpectedDelDate.NonAutoSizeHeight = 10;
            this.dtpExpectedDelDate.Size = new System.Drawing.Size(132, 22);
            this.dtpExpectedDelDate.TabIndex = 5;
            this.dtpExpectedDelDate.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.dtpExpectedDelDate.Value = new System.DateTime(2004, 5, 25, 0, 0, 0, 0);
            this.dtpExpectedDelDate.Validated += new System.EventHandler(this.dtpBoxs_Validate);
            // 
            // ultraLabel27
            // 
            appearance81.FontData.BoldAsString = "False";
            appearance81.FontData.ItalicAsString = "False";
            appearance81.FontData.StrikeoutAsString = "False";
            appearance81.FontData.UnderlineAsString = "False";
            appearance81.ForeColor = System.Drawing.Color.Black;
            appearance81.ForeColorDisabled = System.Drawing.Color.Black;
            this.ultraLabel27.Appearance = appearance81;
            this.ultraLabel27.Location = new System.Drawing.Point(7, 44);
            this.ultraLabel27.Name = "ultraLabel27";
            this.ultraLabel27.Size = new System.Drawing.Size(123, 19);
            this.ultraLabel27.TabIndex = 6;
            this.ultraLabel27.Text = "Desired Ord. Qty";
            this.ultraLabel27.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // txtLocation
            // 
            appearance82.FontData.BoldAsString = "False";
            appearance82.FontData.ItalicAsString = "False";
            appearance82.FontData.StrikeoutAsString = "False";
            appearance82.FontData.UnderlineAsString = "False";
            appearance82.ForeColor = System.Drawing.Color.Black;
            appearance82.ForeColorDisabled = System.Drawing.Color.Black;
            this.txtLocation.Appearance = appearance82;
            this.txtLocation.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.txtLocation.Location = new System.Drawing.Point(122, 120);
            this.txtLocation.MaxLength = 50;
            this.txtLocation.Name = "txtLocation";
            this.txtLocation.Size = new System.Drawing.Size(132, 23);
            this.txtLocation.TabIndex = 4;
            this.txtLocation.Validated += new System.EventHandler(this.txtBoxs_Validate);
            // 
            // ultraLabel22
            // 
            appearance83.FontData.BoldAsString = "False";
            appearance83.FontData.ItalicAsString = "False";
            appearance83.FontData.StrikeoutAsString = "False";
            appearance83.FontData.UnderlineAsString = "False";
            appearance83.ForeColor = System.Drawing.Color.Black;
            appearance83.ForeColorDisabled = System.Drawing.Color.Black;
            this.ultraLabel22.Appearance = appearance83;
            this.ultraLabel22.Location = new System.Drawing.Point(10, 148);
            this.ultraLabel22.Name = "ultraLabel22";
            this.ultraLabel22.Size = new System.Drawing.Size(106, 19);
            this.ultraLabel22.TabIndex = 8;
            this.ultraLabel22.Text = "Delivery Date";
            this.ultraLabel22.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // ultraLabel25
            // 
            appearance84.FontData.BoldAsString = "False";
            appearance84.FontData.ItalicAsString = "False";
            appearance84.FontData.StrikeoutAsString = "False";
            appearance84.FontData.UnderlineAsString = "False";
            appearance84.ForeColor = System.Drawing.Color.Black;
            appearance84.ForeColorDisabled = System.Drawing.Color.Black;
            this.ultraLabel25.Appearance = appearance84;
            this.ultraLabel25.Location = new System.Drawing.Point(7, 18);
            this.ultraLabel25.Name = "ultraLabel25";
            this.ultraLabel25.Size = new System.Drawing.Size(97, 19);
            this.ultraLabel25.TabIndex = 0;
            this.ultraLabel25.Text = "Qty in Stock";
            this.ultraLabel25.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // ultraLabel24
            // 
            appearance85.FontData.BoldAsString = "False";
            appearance85.FontData.ItalicAsString = "False";
            appearance85.FontData.StrikeoutAsString = "False";
            appearance85.FontData.UnderlineAsString = "False";
            appearance85.ForeColor = System.Drawing.Color.Black;
            appearance85.ForeColorDisabled = System.Drawing.Color.Black;
            this.ultraLabel24.Appearance = appearance85;
            this.ultraLabel24.Location = new System.Drawing.Point(7, 70);
            this.ultraLabel24.Name = "ultraLabel24";
            this.ultraLabel24.Size = new System.Drawing.Size(112, 19);
            this.ultraLabel24.TabIndex = 2;
            this.ultraLabel24.Text = "Reorder Level";
            this.ultraLabel24.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // ultraLabel23
            // 
            appearance86.FontData.BoldAsString = "False";
            appearance86.FontData.ItalicAsString = "False";
            appearance86.FontData.StrikeoutAsString = "False";
            appearance86.FontData.UnderlineAsString = "False";
            appearance86.ForeColor = System.Drawing.Color.Black;
            appearance86.ForeColorDisabled = System.Drawing.Color.Black;
            this.ultraLabel23.Appearance = appearance86;
            this.ultraLabel23.Location = new System.Drawing.Point(7, 96);
            this.ultraLabel23.Name = "ultraLabel23";
            this.ultraLabel23.Size = new System.Drawing.Size(96, 19);
            this.ultraLabel23.TabIndex = 4;
            this.ultraLabel23.Text = "Qty on Order";
            this.ultraLabel23.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // ultraLabel26
            // 
            appearance87.FontData.BoldAsString = "False";
            appearance87.FontData.ItalicAsString = "False";
            appearance87.FontData.StrikeoutAsString = "False";
            appearance87.FontData.UnderlineAsString = "False";
            appearance87.ForeColor = System.Drawing.Color.Black;
            appearance87.ForeColorDisabled = System.Drawing.Color.Black;
            this.ultraLabel26.Appearance = appearance87;
            this.ultraLabel26.Location = new System.Drawing.Point(10, 122);
            this.ultraLabel26.Name = "ultraLabel26";
            this.ultraLabel26.Size = new System.Drawing.Size(77, 19);
            this.ultraLabel26.TabIndex = 10;
            this.ultraLabel26.Text = "Location";
            this.ultraLabel26.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // faOnSale
            // 
            this.faOnSale.Controls.Add(this.numSalePrice);
            this.faOnSale.Controls.Add(this.dtpSaleEndDate);
            this.faOnSale.Controls.Add(this.dtpSaleStartDate);
            this.faOnSale.Controls.Add(this.numSaleLimitQty);
            this.faOnSale.Controls.Add(this.ultraLabel17);
            this.faOnSale.Controls.Add(this.ultraLabel15);
            this.faOnSale.Controls.Add(this.ultraLabel6);
            this.faOnSale.Controls.Add(this.ultraLabel12);
            this.faOnSale.Controls.Add(this.btnSave);
            this.faOnSale.Enabled = false;
            this.faOnSale.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.faOnSale.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.faOnSale.ForeColor = System.Drawing.Color.White;
            this.faOnSale.Location = new System.Drawing.Point(572, 340);
            this.faOnSale.Name = "faOnSale";
            this.faOnSale.Size = new System.Drawing.Size(349, 174);
            this.faOnSale.TabIndex = 5;
            this.faOnSale.TabStop = false;
            // 
            // numSalePrice
            // 
            appearance88.FontData.BoldAsString = "False";
            appearance88.FontData.ItalicAsString = "False";
            appearance88.FontData.StrikeoutAsString = "False";
            appearance88.FontData.UnderlineAsString = "False";
            appearance88.ForeColor = System.Drawing.Color.Black;
            appearance88.ForeColorDisabled = System.Drawing.Color.Black;
            this.numSalePrice.Appearance = appearance88;
            this.numSalePrice.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.numSalePrice.Location = new System.Drawing.Point(126, 16);
            this.numSalePrice.MaskInput = "{LOC}-nn,nnn.nn";
            this.numSalePrice.MaxValue = 999999999;
            this.numSalePrice.MinValue = -999999999;
            this.numSalePrice.Name = "numSalePrice";
            this.numSalePrice.NumericType = Infragistics.Win.UltraWinEditors.NumericType.Double;
            this.numSalePrice.Size = new System.Drawing.Size(111, 23);
            this.numSalePrice.TabIndex = 0;
            this.numSalePrice.ValueChanged += new System.EventHandler(this.numSalePrice_ValueChanged);
            this.numSalePrice.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.numSalePrice_KeyPress);
            this.numSalePrice.Validated += new System.EventHandler(this.numBoxs_Validate);
            // 
            // dtpSaleEndDate
            // 
            this.dtpSaleEndDate.AllowNull = false;
            appearance89.FontData.BoldAsString = "False";
            appearance89.FontData.ItalicAsString = "False";
            appearance89.FontData.StrikeoutAsString = "False";
            appearance89.FontData.UnderlineAsString = "False";
            appearance89.ForeColor = System.Drawing.Color.Black;
            appearance89.ForeColorDisabled = System.Drawing.Color.Black;
            this.dtpSaleEndDate.Appearance = appearance89;
            this.dtpSaleEndDate.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Office2003ToolbarButton;
            this.dtpSaleEndDate.DateButtons.Add(dateButton3);
            this.dtpSaleEndDate.Location = new System.Drawing.Point(126, 99);
            this.dtpSaleEndDate.Name = "dtpSaleEndDate";
            this.dtpSaleEndDate.NonAutoSizeHeight = 10;
            this.dtpSaleEndDate.NullDateLabel = "";
            this.dtpSaleEndDate.Size = new System.Drawing.Size(111, 22);
            this.dtpSaleEndDate.TabIndex = 3;
            this.dtpSaleEndDate.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.dtpSaleEndDate.Value = new System.DateTime(2004, 5, 25, 0, 0, 0, 0);
            this.dtpSaleEndDate.Validated += new System.EventHandler(this.dtpBoxs_Validate);
            // 
            // dtpSaleStartDate
            // 
            this.dtpSaleStartDate.AllowNull = false;
            appearance90.FontData.BoldAsString = "False";
            appearance90.FontData.ItalicAsString = "False";
            appearance90.FontData.StrikeoutAsString = "False";
            appearance90.FontData.UnderlineAsString = "False";
            appearance90.ForeColor = System.Drawing.Color.Black;
            appearance90.ForeColorDisabled = System.Drawing.Color.Black;
            this.dtpSaleStartDate.Appearance = appearance90;
            this.dtpSaleStartDate.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Office2003ToolbarButton;
            this.dtpSaleStartDate.DateButtons.Add(dateButton4);
            this.dtpSaleStartDate.Location = new System.Drawing.Point(126, 72);
            this.dtpSaleStartDate.Name = "dtpSaleStartDate";
            this.dtpSaleStartDate.NonAutoSizeHeight = 10;
            this.dtpSaleStartDate.NullDateLabel = "";
            this.dtpSaleStartDate.Size = new System.Drawing.Size(111, 22);
            this.dtpSaleStartDate.TabIndex = 2;
            this.dtpSaleStartDate.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.dtpSaleStartDate.Value = new System.DateTime(2007, 4, 10, 0, 0, 0, 0);
            this.dtpSaleStartDate.Validated += new System.EventHandler(this.dtpBoxs_Validate);
            // 
            // numSaleLimitQty
            // 
            appearance91.FontData.BoldAsString = "False";
            appearance91.FontData.ItalicAsString = "False";
            appearance91.FontData.StrikeoutAsString = "False";
            appearance91.FontData.UnderlineAsString = "False";
            appearance91.ForeColor = System.Drawing.Color.Black;
            appearance91.ForeColorDisabled = System.Drawing.Color.Black;
            this.numSaleLimitQty.Appearance = appearance91;
            this.numSaleLimitQty.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.numSaleLimitQty.Location = new System.Drawing.Point(126, 44);
            this.numSaleLimitQty.MaskInput = "-nn,nnn";
            this.numSaleLimitQty.MaxValue = 99999999;
            this.numSaleLimitQty.MinValue = 0;
            this.numSaleLimitQty.Name = "numSaleLimitQty";
            this.numSaleLimitQty.Size = new System.Drawing.Size(111, 23);
            this.numSaleLimitQty.TabIndex = 1;
            this.numSaleLimitQty.Validated += new System.EventHandler(this.numBoxs_Validate);
            // 
            // ultraLabel17
            // 
            appearance92.FontData.BoldAsString = "False";
            appearance92.ForeColor = System.Drawing.Color.Black;
            this.ultraLabel17.Appearance = appearance92;
            this.ultraLabel17.Location = new System.Drawing.Point(7, 46);
            this.ultraLabel17.Name = "ultraLabel17";
            this.ultraLabel17.Size = new System.Drawing.Size(115, 19);
            this.ultraLabel17.TabIndex = 8;
            this.ultraLabel17.Text = "Sale Limit Qty";
            this.ultraLabel17.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // ultraLabel15
            // 
            appearance93.FontData.BoldAsString = "False";
            appearance93.ForeColor = System.Drawing.Color.Black;
            this.ultraLabel15.Appearance = appearance93;
            this.ultraLabel15.Location = new System.Drawing.Point(7, 20);
            this.ultraLabel15.Name = "ultraLabel15";
            this.ultraLabel15.Size = new System.Drawing.Size(97, 19);
            this.ultraLabel15.TabIndex = 0;
            this.ultraLabel15.Text = "On Sale Price";
            this.ultraLabel15.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // ultraLabel6
            // 
            appearance94.FontData.BoldAsString = "False";
            appearance94.ForeColor = System.Drawing.Color.Black;
            this.ultraLabel6.Appearance = appearance94;
            this.ultraLabel6.Location = new System.Drawing.Point(7, 101);
            this.ultraLabel6.Name = "ultraLabel6";
            this.ultraLabel6.Size = new System.Drawing.Size(103, 19);
            this.ultraLabel6.TabIndex = 4;
            this.ultraLabel6.Text = "Sale End Date";
            this.ultraLabel6.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // ultraLabel12
            // 
            appearance95.FontData.BoldAsString = "False";
            appearance95.ForeColor = System.Drawing.Color.Black;
            this.ultraLabel12.Appearance = appearance95;
            this.ultraLabel12.Location = new System.Drawing.Point(7, 74);
            this.ultraLabel12.Name = "ultraLabel12";
            this.ultraLabel12.Size = new System.Drawing.Size(111, 19);
            this.ultraLabel12.TabIndex = 2;
            this.ultraLabel12.Text = "Sale Start Date";
            this.ultraLabel12.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // btnSave
            // 
            appearance96.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(62)))), ((int)(((byte)(176)))));
            appearance96.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance96.FontData.BoldAsString = "True";
            appearance96.ForeColor = System.Drawing.Color.White;
            this.btnSave.Appearance = appearance96;
            this.btnSave.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            this.btnSave.Location = new System.Drawing.Point(264, 142);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(90, 26);
            this.btnSave.TabIndex = 6;
            this.btnSave.Text = "&Save";
            this.btnSave.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnSave.Visible = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // ultraTabPageControl2
            // 
            this.ultraTabPageControl2.Controls.Add(this.grbVendorInformation);
            this.ultraTabPageControl2.Controls.Add(this.groupBox6);
            this.ultraTabPageControl2.Location = new System.Drawing.Point(-10000, -10000);
            this.ultraTabPageControl2.Name = "ultraTabPageControl2";
            this.ultraTabPageControl2.Size = new System.Drawing.Size(924, 521);
            // 
            // grbVendorInformation
            // 
            this.grbVendorInformation.Controls.Add(this.pnlDeleteItemVendorInfo);
            this.grbVendorInformation.Controls.Add(this.pnlSaveItemVendor);
            this.grbVendorInformation.Controls.Add(this.grdDetail);
            this.grbVendorInformation.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.grbVendorInformation.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grbVendorInformation.ForeColor = System.Drawing.Color.White;
            this.grbVendorInformation.Location = new System.Drawing.Point(5, 145);
            this.grbVendorInformation.Name = "grbVendorInformation";
            this.grbVendorInformation.Size = new System.Drawing.Size(775, 319);
            this.grbVendorInformation.TabIndex = 34;
            this.grbVendorInformation.TabStop = false;
            this.grbVendorInformation.Text = "Add / Edit / Delete Vendor Item Information";
            this.grbVendorInformation.Visible = false;
            // 
            // pnlDeleteItemVendorInfo
            // 
            // 
            // pnlDeleteItemVendorInfo.ClientArea
            // 
            this.pnlDeleteItemVendorInfo.ClientArea.Controls.Add(this.btnDeleteItemVendorInfo);
            this.pnlDeleteItemVendorInfo.ClientArea.Controls.Add(this.lblDeleteItemVendorInfo);
            this.pnlDeleteItemVendorInfo.Location = new System.Drawing.Point(499, 280);
            this.pnlDeleteItemVendorInfo.Name = "pnlDeleteItemVendorInfo";
            this.pnlDeleteItemVendorInfo.Size = new System.Drawing.Size(130, 30);
            this.pnlDeleteItemVendorInfo.TabIndex = 14;
            // 
            // btnDeleteItemVendorInfo
            // 
            appearance97.BackColor = System.Drawing.Color.White;
            appearance97.ForeColor = System.Drawing.Color.Black;
            this.btnDeleteItemVendorInfo.Appearance = appearance97;
            this.btnDeleteItemVendorInfo.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Flat;
            this.btnDeleteItemVendorInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnDeleteItemVendorInfo.Location = new System.Drawing.Point(50, 0);
            this.btnDeleteItemVendorInfo.Name = "btnDeleteItemVendorInfo";
            this.btnDeleteItemVendorInfo.Size = new System.Drawing.Size(80, 30);
            this.btnDeleteItemVendorInfo.TabIndex = 11;
            this.btnDeleteItemVendorInfo.Text = "&Delete";
            this.btnDeleteItemVendorInfo.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnDeleteItemVendorInfo.Click += new System.EventHandler(this.btnDeleteItemVendorInfo_Click);
            // 
            // lblDeleteItemVendorInfo
            // 
            appearance98.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(138)))), ((int)(((byte)(31)))));
            appearance98.FontData.BoldAsString = "True";
            appearance98.ForeColor = System.Drawing.Color.White;
            appearance98.TextHAlignAsString = "Center";
            appearance98.TextVAlignAsString = "Middle";
            this.lblDeleteItemVendorInfo.Appearance = appearance98;
            this.lblDeleteItemVendorInfo.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblDeleteItemVendorInfo.Font = new System.Drawing.Font("Verdana", 8F, System.Drawing.FontStyle.Bold);
            this.lblDeleteItemVendorInfo.Location = new System.Drawing.Point(0, 0);
            this.lblDeleteItemVendorInfo.Margin = new System.Windows.Forms.Padding(3, 3, 0, 3);
            this.lblDeleteItemVendorInfo.Name = "lblDeleteItemVendorInfo";
            this.lblDeleteItemVendorInfo.Size = new System.Drawing.Size(50, 30);
            this.lblDeleteItemVendorInfo.TabIndex = 0;
            this.lblDeleteItemVendorInfo.Tag = "NOCOLOR";
            this.lblDeleteItemVendorInfo.Text = "Alt + D";
            this.lblDeleteItemVendorInfo.Click += new System.EventHandler(this.btnDeleteItemVendorInfo_Click);
            // 
            // pnlSaveItemVendor
            // 
            // 
            // pnlSaveItemVendor.ClientArea
            // 
            this.pnlSaveItemVendor.ClientArea.Controls.Add(this.btnSaveItemVendor);
            this.pnlSaveItemVendor.ClientArea.Controls.Add(this.lblSaveItemVendor);
            this.pnlSaveItemVendor.Location = new System.Drawing.Point(635, 280);
            this.pnlSaveItemVendor.Name = "pnlSaveItemVendor";
            this.pnlSaveItemVendor.Size = new System.Drawing.Size(130, 30);
            this.pnlSaveItemVendor.TabIndex = 13;
            // 
            // btnSaveItemVendor
            // 
            appearance99.BackColor = System.Drawing.Color.White;
            appearance99.ForeColor = System.Drawing.Color.Black;
            this.btnSaveItemVendor.Appearance = appearance99;
            this.btnSaveItemVendor.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Flat;
            this.btnSaveItemVendor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnSaveItemVendor.Location = new System.Drawing.Point(50, 0);
            this.btnSaveItemVendor.Name = "btnSaveItemVendor";
            this.btnSaveItemVendor.Size = new System.Drawing.Size(80, 30);
            this.btnSaveItemVendor.TabIndex = 10;
            this.btnSaveItemVendor.Text = "&Save";
            this.btnSaveItemVendor.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnSaveItemVendor.Click += new System.EventHandler(this.btnSaveItemVendor_Click);
            // 
            // lblSaveItemVendor
            // 
            appearance100.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(138)))), ((int)(((byte)(31)))));
            appearance100.FontData.BoldAsString = "True";
            appearance100.ForeColor = System.Drawing.Color.White;
            appearance100.TextHAlignAsString = "Center";
            appearance100.TextVAlignAsString = "Middle";
            this.lblSaveItemVendor.Appearance = appearance100;
            this.lblSaveItemVendor.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblSaveItemVendor.Font = new System.Drawing.Font("Verdana", 8F, System.Drawing.FontStyle.Bold);
            this.lblSaveItemVendor.Location = new System.Drawing.Point(0, 0);
            this.lblSaveItemVendor.Margin = new System.Windows.Forms.Padding(3, 3, 0, 3);
            this.lblSaveItemVendor.Name = "lblSaveItemVendor";
            this.lblSaveItemVendor.Size = new System.Drawing.Size(50, 30);
            this.lblSaveItemVendor.TabIndex = 0;
            this.lblSaveItemVendor.Tag = "NOCOLOR";
            this.lblSaveItemVendor.Text = "Alt + S";
            this.lblSaveItemVendor.Click += new System.EventHandler(this.btnSaveItemVendor_Click);
            // 
            // grdDetail
            // 
            appearance101.BackColor = System.Drawing.Color.White;
            appearance101.BackColor2 = System.Drawing.Color.White;
            appearance101.BackColorDisabled = System.Drawing.Color.White;
            appearance101.BackColorDisabled2 = System.Drawing.Color.White;
            appearance101.BackGradientStyle = Infragistics.Win.GradientStyle.None;
            this.grdDetail.DisplayLayout.Appearance = appearance101;
            ultraGridColumn1.ButtonDisplayStyle = Infragistics.Win.UltraWinGrid.ButtonDisplayStyle.Always;
            ultraGridColumn1.Header.VisiblePosition = 0;
            ultraGridColumn1.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.EditButton;
            ultraGridColumn1.Width = 95;
            ultraGridColumn2.CellActivation = Infragistics.Win.UltraWinGrid.Activation.Disabled;
            ultraGridColumn2.Header.VisiblePosition = 1;
            ultraGridColumn2.Width = 128;
            ultraGridColumn3.Header.VisiblePosition = 2;
            ultraGridColumn3.Width = 157;
            ultraGridColumn4.Header.VisiblePosition = 3;
            ultraGridColumn4.Width = 87;
            ultraGridColumn5.Header.VisiblePosition = 4;
            ultraGridColumn5.Width = 140;
            ultraGridBand1.Columns.AddRange(new object[] {
            ultraGridColumn1,
            ultraGridColumn2,
            ultraGridColumn3,
            ultraGridColumn4,
            ultraGridColumn5});
            this.grdDetail.DisplayLayout.BandsSerializer.Add(ultraGridBand1);
            this.grdDetail.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.grdDetail.DisplayLayout.InterBandSpacing = 10;
            this.grdDetail.DisplayLayout.MaxColScrollRegions = 1;
            this.grdDetail.DisplayLayout.MaxRowScrollRegions = 1;
            appearance102.BackColor = System.Drawing.Color.White;
            appearance102.BackColor2 = System.Drawing.Color.White;
            this.grdDetail.DisplayLayout.Override.ActiveCardCaptionAppearance = appearance102;
            appearance103.BackColor = System.Drawing.Color.White;
            appearance103.BackColor2 = System.Drawing.Color.White;
            appearance103.BorderColor = System.Drawing.Color.Gray;
            this.grdDetail.DisplayLayout.Override.ActiveRowAppearance = appearance103;
            appearance104.BackColor = System.Drawing.Color.White;
            appearance104.BackColor2 = System.Drawing.Color.White;
            appearance104.BorderColor = System.Drawing.Color.Gray;
            this.grdDetail.DisplayLayout.Override.AddRowAppearance = appearance104;
            this.grdDetail.DisplayLayout.Override.AllowAddNew = Infragistics.Win.UltraWinGrid.AllowAddNew.TemplateOnBottom;
            this.grdDetail.DisplayLayout.Override.AllowColMoving = Infragistics.Win.UltraWinGrid.AllowColMoving.WithinBand;
            this.grdDetail.DisplayLayout.Override.AllowColSwapping = Infragistics.Win.UltraWinGrid.AllowColSwapping.NotAllowed;
            this.grdDetail.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.True;
            this.grdDetail.DisplayLayout.Override.AllowRowFiltering = Infragistics.Win.DefaultableBoolean.False;
            this.grdDetail.DisplayLayout.Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.True;
            appearance105.BackColor = System.Drawing.Color.Transparent;
            this.grdDetail.DisplayLayout.Override.CardAreaAppearance = appearance105;
            appearance106.BackColor = System.Drawing.Color.White;
            appearance106.BackColor2 = System.Drawing.Color.White;
            appearance106.BackColorDisabled = System.Drawing.Color.White;
            appearance106.BackColorDisabled2 = System.Drawing.Color.White;
            appearance106.BorderColor = System.Drawing.Color.Black;
            appearance106.BorderColor3DBase = System.Drawing.Color.Black;
            this.grdDetail.DisplayLayout.Override.CellAppearance = appearance106;
            appearance107.BackColor = System.Drawing.Color.White;
            appearance107.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(231)))), ((int)(((byte)(230)))));
            appearance107.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance107.BorderAlpha = Infragistics.Win.Alpha.Transparent;
            appearance107.BorderColor = System.Drawing.Color.Gray;
            appearance107.BorderColor3DBase = System.Drawing.Color.Gray;
            appearance107.ImageBackgroundAlpha = Infragistics.Win.Alpha.Transparent;
            this.grdDetail.DisplayLayout.Override.CellButtonAppearance = appearance107;
            appearance108.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            appearance108.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.grdDetail.DisplayLayout.Override.EditCellAppearance = appearance108;
            appearance109.BorderColor = System.Drawing.Color.Gray;
            this.grdDetail.DisplayLayout.Override.FilteredInRowAppearance = appearance109;
            appearance110.BorderColor = System.Drawing.Color.Gray;
            this.grdDetail.DisplayLayout.Override.FilteredOutRowAppearance = appearance110;
            appearance111.BackColor = System.Drawing.Color.White;
            appearance111.BackColor2 = System.Drawing.Color.White;
            appearance111.BackColorDisabled = System.Drawing.Color.White;
            appearance111.BackColorDisabled2 = System.Drawing.Color.White;
            this.grdDetail.DisplayLayout.Override.FixedCellAppearance = appearance111;
            appearance112.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            appearance112.BackColor2 = System.Drawing.Color.Beige;
            this.grdDetail.DisplayLayout.Override.FixedHeaderAppearance = appearance112;
            appearance113.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(62)))), ((int)(((byte)(176)))));
            appearance113.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance113.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance113.FontData.BoldAsString = "True";
            appearance113.FontData.SizeInPoints = 10F;
            appearance113.ForeColor = System.Drawing.Color.White;
            appearance113.TextHAlignAsString = "Left";
            appearance113.ThemedElementAlpha = Infragistics.Win.Alpha.Transparent;
            this.grdDetail.DisplayLayout.Override.HeaderAppearance = appearance113;
            this.grdDetail.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortSingle;
            this.grdDetail.DisplayLayout.Override.MergedCellStyle = Infragistics.Win.UltraWinGrid.MergedCellStyle.Never;
            appearance114.BorderColor = System.Drawing.Color.Gray;
            this.grdDetail.DisplayLayout.Override.RowAlternateAppearance = appearance114;
            appearance115.BackColor = System.Drawing.Color.White;
            appearance115.BackColor2 = System.Drawing.Color.White;
            appearance115.BackGradientStyle = Infragistics.Win.GradientStyle.None;
            appearance115.BackHatchStyle = Infragistics.Win.BackHatchStyle.None;
            appearance115.BorderColor = System.Drawing.Color.Gray;
            this.grdDetail.DisplayLayout.Override.RowAppearance = appearance115;
            appearance116.BorderColor = System.Drawing.Color.Gray;
            this.grdDetail.DisplayLayout.Override.RowPreviewAppearance = appearance116;
            appearance117.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(62)))), ((int)(((byte)(176)))));
            appearance117.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance117.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance117.BorderColor = System.Drawing.Color.Gray;
            appearance117.ForeColor = System.Drawing.Color.White;
            this.grdDetail.DisplayLayout.Override.RowSelectorAppearance = appearance117;
            this.grdDetail.DisplayLayout.Override.RowSelectorWidth = 12;
            this.grdDetail.DisplayLayout.Override.RowSizingArea = Infragistics.Win.UltraWinGrid.RowSizingArea.EntireRow;
            this.grdDetail.DisplayLayout.Override.RowSpacingBefore = 2;
            appearance118.BackColor = System.Drawing.Color.Navy;
            appearance118.BackColorAlpha = Infragistics.Win.Alpha.Opaque;
            this.grdDetail.DisplayLayout.Override.SelectedCellAppearance = appearance118;
            appearance119.BackColor = System.Drawing.Color.Navy;
            appearance119.BackColorDisabled = System.Drawing.Color.Navy;
            appearance119.BackGradientStyle = Infragistics.Win.GradientStyle.None;
            appearance119.BackHatchStyle = Infragistics.Win.BackHatchStyle.None;
            appearance119.BorderColor = System.Drawing.Color.Gray;
            appearance119.ForeColor = System.Drawing.Color.White;
            this.grdDetail.DisplayLayout.Override.SelectedRowAppearance = appearance119;
            this.grdDetail.DisplayLayout.Override.SelectTypeCell = Infragistics.Win.UltraWinGrid.SelectType.Single;
            this.grdDetail.DisplayLayout.Override.SelectTypeCol = Infragistics.Win.UltraWinGrid.SelectType.Single;
            this.grdDetail.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.Single;
            appearance120.BorderColor = System.Drawing.Color.Gray;
            this.grdDetail.DisplayLayout.Override.TemplateAddRowAppearance = appearance120;
            this.grdDetail.DisplayLayout.RowConnectorColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            this.grdDetail.DisplayLayout.RowConnectorStyle = Infragistics.Win.UltraWinGrid.RowConnectorStyle.Solid;
            appearance121.BackColor = System.Drawing.Color.WhiteSmoke;
            appearance121.BackColor2 = System.Drawing.Color.WhiteSmoke;
            appearance121.ImageBackgroundOrigin = Infragistics.Win.ImageBackgroundOrigin.Form;
            scrollBarLook1.TrackAppearance = appearance121;
            this.grdDetail.DisplayLayout.ScrollBarLook = scrollBarLook1;
            this.grdDetail.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.grdDetail.Location = new System.Drawing.Point(12, 28);
            this.grdDetail.Name = "grdDetail";
            this.grdDetail.Size = new System.Drawing.Size(753, 241);
            this.grdDetail.TabIndex = 1;
            this.grdDetail.UpdateMode = Infragistics.Win.UltraWinGrid.UpdateMode.OnCellChangeOrLostFocus;
            this.grdDetail.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.grdDetail.AfterCellUpdate += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.grdDetail_AfterCellUpdate);
            this.grdDetail.AfterRowInsert += new Infragistics.Win.UltraWinGrid.RowEventHandler(this.grdDetail_AfterRowInsert);
            this.grdDetail.CellChange += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.grdDetail_CellChange);
            this.grdDetail.ClickCellButton += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.grdDetail_ClickCellButton);
            this.grdDetail.BeforeCellDeactivate += new System.ComponentModel.CancelEventHandler(this.grdDetail_BeforeCellDeactivate);
            this.grdDetail.BeforeRowDeactivate += new System.ComponentModel.CancelEventHandler(this.grdDetail_BeforeRowDeactivate);
            this.grdDetail.Error += new Infragistics.Win.UltraWinGrid.ErrorEventHandler(this.grdDetail_Error);
            this.grdDetail.Enter += new System.EventHandler(this.grdDetail_Enter);
            this.grdDetail.KeyDown += new System.Windows.Forms.KeyEventHandler(this.grdDetail_KeyDown);
            this.grdDetail.KeyUp += new System.Windows.Forms.KeyEventHandler(this.grdDetail_KeyUp);
            this.grdDetail.Validated += new System.EventHandler(this.grdDetail_Validated);
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.combEditorPrefVendor);
            this.groupBox6.Controls.Add(this.ultraLblLastVendorLabel);
            this.groupBox6.Controls.Add(this.ultraLabelPreferredVendor);
            this.groupBox6.Controls.Add(this.txtLastVendor);
            this.groupBox6.Controls.Add(this.txtVendorName);
            this.groupBox6.Controls.Add(this.ultraLabel31);
            this.groupBox6.Controls.Add(this.ultraLabel33);
            this.groupBox6.Controls.Add(this.txtRemarks);
            this.groupBox6.Controls.Add(this.groupBox7);
            this.groupBox6.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.groupBox6.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox6.ForeColor = System.Drawing.Color.White;
            this.groupBox6.Location = new System.Drawing.Point(4, 12);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(776, 126);
            this.groupBox6.TabIndex = 33;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "Vendor Information";
            // 
            // combEditorPrefVendor
            // 
            appearance122.BorderAlpha = Infragistics.Win.Alpha.Opaque;
            appearance122.BorderColor3DBase = System.Drawing.Color.Black;
            appearance122.FontData.BoldAsString = "False";
            appearance122.FontData.ItalicAsString = "False";
            appearance122.FontData.StrikeoutAsString = "False";
            appearance122.FontData.UnderlineAsString = "False";
            appearance122.ForeColor = System.Drawing.Color.Black;
            appearance122.ForeColorDisabled = System.Drawing.Color.Black;
            this.combEditorPrefVendor.Appearance = appearance122;
            this.combEditorPrefVendor.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance123.BackColor = System.Drawing.Color.White;
            appearance123.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(231)))), ((int)(((byte)(230)))));
            appearance123.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance123.BorderAlpha = Infragistics.Win.Alpha.Transparent;
            this.combEditorPrefVendor.ButtonAppearance = appearance123;
            this.combEditorPrefVendor.DropDownStyle = Infragistics.Win.DropDownStyle.DropDownList;
            this.combEditorPrefVendor.Location = new System.Drawing.Point(128, 26);
            this.combEditorPrefVendor.MaxLength = 20;
            this.combEditorPrefVendor.Name = "combEditorPrefVendor";
            this.combEditorPrefVendor.Size = new System.Drawing.Size(132, 23);
            this.combEditorPrefVendor.TabIndex = 36;
            this.combEditorPrefVendor.ValueChanged += new System.EventHandler(this.combEditorPrefVendor_ValueChanged);
            this.combEditorPrefVendor.Leave += new System.EventHandler(this.cboItemType_SelectionChanged);
            // 
            // ultraLblLastVendorLabel
            // 
            appearance124.FontData.BoldAsString = "False";
            appearance124.FontData.ItalicAsString = "False";
            appearance124.FontData.StrikeoutAsString = "False";
            appearance124.FontData.UnderlineAsString = "False";
            appearance124.ForeColor = System.Drawing.Color.White;
            appearance124.ForeColorDisabled = System.Drawing.Color.White;
            this.ultraLblLastVendorLabel.Appearance = appearance124;
            this.ultraLblLastVendorLabel.BorderStyleOuter = Infragistics.Win.UIElementBorderStyle.Solid;
            this.ultraLblLastVendorLabel.Location = new System.Drawing.Point(260, 26);
            this.ultraLblLastVendorLabel.Name = "ultraLblLastVendorLabel";
            this.ultraLblLastVendorLabel.Size = new System.Drawing.Size(251, 23);
            this.ultraLblLastVendorLabel.TabIndex = 52;
            // 
            // ultraLabelPreferredVendor
            // 
            appearance125.FontData.BoldAsString = "False";
            appearance125.FontData.ItalicAsString = "False";
            appearance125.FontData.StrikeoutAsString = "False";
            appearance125.FontData.UnderlineAsString = "False";
            appearance125.ForeColor = System.Drawing.Color.White;
            appearance125.ForeColorDisabled = System.Drawing.Color.Black;
            this.ultraLabelPreferredVendor.Appearance = appearance125;
            this.ultraLabelPreferredVendor.Location = new System.Drawing.Point(7, 30);
            this.ultraLabelPreferredVendor.Name = "ultraLabelPreferredVendor";
            this.ultraLabelPreferredVendor.Size = new System.Drawing.Size(126, 19);
            this.ultraLabelPreferredVendor.TabIndex = 51;
            this.ultraLabelPreferredVendor.Text = "Preferred Vendor";
            this.ultraLabelPreferredVendor.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // txtLastVendor
            // 
            appearance126.FontData.BoldAsString = "False";
            appearance126.FontData.ItalicAsString = "False";
            appearance126.FontData.StrikeoutAsString = "False";
            appearance126.FontData.UnderlineAsString = "False";
            appearance126.ForeColor = System.Drawing.Color.Black;
            appearance126.ForeColorDisabled = System.Drawing.Color.Black;
            this.txtLastVendor.Appearance = appearance126;
            this.txtLastVendor.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance127.BackColor = System.Drawing.Color.White;
            appearance127.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(231)))));
            appearance127.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance127.BorderAlpha = Infragistics.Win.Alpha.Transparent;
            editorButton3.Appearance = appearance127;
            editorButton3.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Flat;
            editorButton3.Text = "";
            editorButton3.Width = 16;
            this.txtLastVendor.ButtonsRight.Add(editorButton3);
            this.txtLastVendor.Location = new System.Drawing.Point(128, 55);
            this.txtLastVendor.MaxLength = 20;
            this.txtLastVendor.Name = "txtLastVendor";
            this.txtLastVendor.Size = new System.Drawing.Size(132, 23);
            this.txtLastVendor.TabIndex = 37;
            this.toolTip1.SetToolTip(this.txtLastVendor, "Press F4 To Search");
            this.txtLastVendor.EditorButtonClick += new Infragistics.Win.UltraWinEditors.EditorButtonEventHandler(this.txtItemCode_EditorButtonClick);
            this.txtLastVendor.Leave += new System.EventHandler(this.ForiegnKeys_Leave);
            // 
            // txtVendorName
            // 
            appearance128.FontData.BoldAsString = "False";
            appearance128.FontData.ItalicAsString = "False";
            appearance128.FontData.StrikeoutAsString = "False";
            appearance128.FontData.UnderlineAsString = "False";
            appearance128.ForeColor = System.Drawing.Color.White;
            appearance128.ForeColorDisabled = System.Drawing.Color.White;
            this.txtVendorName.Appearance = appearance128;
            this.txtVendorName.BorderStyleOuter = Infragistics.Win.UIElementBorderStyle.Solid;
            this.txtVendorName.Location = new System.Drawing.Point(260, 55);
            this.txtVendorName.Name = "txtVendorName";
            this.txtVendorName.Size = new System.Drawing.Size(251, 23);
            this.txtVendorName.TabIndex = 45;
            // 
            // ultraLabel31
            // 
            appearance129.FontData.BoldAsString = "False";
            appearance129.FontData.ItalicAsString = "False";
            appearance129.FontData.StrikeoutAsString = "False";
            appearance129.FontData.UnderlineAsString = "False";
            appearance129.ForeColor = System.Drawing.Color.White;
            appearance129.ForeColorDisabled = System.Drawing.Color.Black;
            this.ultraLabel31.Appearance = appearance129;
            this.ultraLabel31.Location = new System.Drawing.Point(7, 55);
            this.ultraLabel31.Name = "ultraLabel31";
            this.ultraLabel31.Size = new System.Drawing.Size(92, 19);
            this.ultraLabel31.TabIndex = 43;
            this.ultraLabel31.Text = "Last Vendor";
            this.ultraLabel31.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // ultraLabel33
            // 
            appearance130.FontData.BoldAsString = "False";
            appearance130.FontData.ItalicAsString = "False";
            appearance130.FontData.StrikeoutAsString = "False";
            appearance130.FontData.UnderlineAsString = "False";
            appearance130.ForeColor = System.Drawing.Color.White;
            appearance130.ForeColorDisabled = System.Drawing.Color.Black;
            this.ultraLabel33.Appearance = appearance130;
            this.ultraLabel33.Location = new System.Drawing.Point(7, 87);
            this.ultraLabel33.Name = "ultraLabel33";
            this.ultraLabel33.Size = new System.Drawing.Size(83, 19);
            this.ultraLabel33.TabIndex = 50;
            this.ultraLabel33.Text = "Remarks";
            this.ultraLabel33.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // txtRemarks
            // 
            appearance131.FontData.BoldAsString = "False";
            appearance131.FontData.ItalicAsString = "False";
            appearance131.FontData.StrikeoutAsString = "False";
            appearance131.FontData.UnderlineAsString = "False";
            appearance131.ForeColor = System.Drawing.Color.Black;
            appearance131.ForeColorDisabled = System.Drawing.Color.Black;
            this.txtRemarks.Appearance = appearance131;
            this.txtRemarks.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.txtRemarks.Location = new System.Drawing.Point(128, 85);
            this.txtRemarks.MaxLength = 200;
            this.txtRemarks.Name = "txtRemarks";
            this.txtRemarks.Size = new System.Drawing.Size(383, 23);
            this.txtRemarks.TabIndex = 38;
            this.txtRemarks.Validated += new System.EventHandler(this.txtBoxs_Validate);
            // 
            // groupBox7
            // 
            this.groupBox7.Controls.Add(this.dtpLastSaleDate);
            this.groupBox7.Controls.Add(this.ultraLabel28);
            this.groupBox7.Controls.Add(this.dtpLastRecvDate);
            this.groupBox7.Controls.Add(this.ultraLabel32);
            this.groupBox7.Location = new System.Drawing.Point(517, 35);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Size = new System.Drawing.Size(255, 73);
            this.groupBox7.TabIndex = 53;
            this.groupBox7.TabStop = false;
            // 
            // dtpLastSaleDate
            // 
            this.dtpLastSaleDate.AllowNull = false;
            appearance132.FontData.BoldAsString = "False";
            appearance132.FontData.ItalicAsString = "False";
            appearance132.FontData.StrikeoutAsString = "False";
            appearance132.FontData.UnderlineAsString = "False";
            appearance132.ForeColor = System.Drawing.Color.Black;
            appearance132.ForeColorDisabled = System.Drawing.Color.Black;
            this.dtpLastSaleDate.Appearance = appearance132;
            this.dtpLastSaleDate.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Office2003ToolbarButton;
            this.dtpLastSaleDate.DateButtons.Add(dateButton5);
            this.dtpLastSaleDate.Location = new System.Drawing.Point(117, 14);
            this.dtpLastSaleDate.Name = "dtpLastSaleDate";
            this.dtpLastSaleDate.NonAutoSizeHeight = 10;
            this.dtpLastSaleDate.Size = new System.Drawing.Size(132, 22);
            this.dtpLastSaleDate.TabIndex = 39;
            this.dtpLastSaleDate.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.dtpLastSaleDate.Value = new System.DateTime(2004, 5, 25, 0, 0, 0, 0);
            this.dtpLastSaleDate.Validated += new System.EventHandler(this.dtpBoxs_Validate);
            // 
            // ultraLabel28
            // 
            appearance133.FontData.BoldAsString = "False";
            appearance133.FontData.ItalicAsString = "False";
            appearance133.FontData.StrikeoutAsString = "False";
            appearance133.FontData.UnderlineAsString = "False";
            appearance133.ForeColor = System.Drawing.Color.Black;
            appearance133.ForeColorDisabled = System.Drawing.Color.Black;
            this.ultraLabel28.Appearance = appearance133;
            this.ultraLabel28.Location = new System.Drawing.Point(9, 14);
            this.ultraLabel28.Name = "ultraLabel28";
            this.ultraLabel28.Size = new System.Drawing.Size(106, 19);
            this.ultraLabel28.TabIndex = 46;
            this.ultraLabel28.Text = "Last Sold Date";
            this.ultraLabel28.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // dtpLastRecvDate
            // 
            this.dtpLastRecvDate.AllowNull = false;
            appearance134.FontData.BoldAsString = "False";
            appearance134.FontData.ItalicAsString = "False";
            appearance134.FontData.StrikeoutAsString = "False";
            appearance134.FontData.UnderlineAsString = "False";
            appearance134.ForeColor = System.Drawing.Color.Black;
            appearance134.ForeColorDisabled = System.Drawing.Color.Black;
            this.dtpLastRecvDate.Appearance = appearance134;
            this.dtpLastRecvDate.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Office2003ToolbarButton;
            this.dtpLastRecvDate.DateButtons.Add(dateButton6);
            this.dtpLastRecvDate.Location = new System.Drawing.Point(117, 42);
            this.dtpLastRecvDate.Name = "dtpLastRecvDate";
            this.dtpLastRecvDate.NonAutoSizeHeight = 10;
            this.dtpLastRecvDate.Size = new System.Drawing.Size(132, 22);
            this.dtpLastRecvDate.TabIndex = 40;
            this.dtpLastRecvDate.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.dtpLastRecvDate.Value = new System.DateTime(2004, 5, 25, 0, 0, 0, 0);
            this.dtpLastRecvDate.Validated += new System.EventHandler(this.dtpBoxs_Validate);
            // 
            // ultraLabel32
            // 
            appearance135.FontData.BoldAsString = "False";
            appearance135.FontData.ItalicAsString = "False";
            appearance135.FontData.StrikeoutAsString = "False";
            appearance135.FontData.UnderlineAsString = "False";
            appearance135.ForeColor = System.Drawing.Color.Black;
            appearance135.ForeColorDisabled = System.Drawing.Color.Black;
            this.ultraLabel32.Appearance = appearance135;
            this.ultraLabel32.Location = new System.Drawing.Point(9, 45);
            this.ultraLabel32.Name = "ultraLabel32";
            this.ultraLabel32.Size = new System.Drawing.Size(109, 19);
            this.ultraLabel32.TabIndex = 48;
            this.ultraLabel32.Text = "Last Recv.Date";
            this.ultraLabel32.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // ultraTabPageControl3
            // 
            this.ultraTabPageControl3.Controls.Add(this.groupBox8);
            this.ultraTabPageControl3.Location = new System.Drawing.Point(-10000, -10000);
            this.ultraTabPageControl3.Name = "ultraTabPageControl3";
            this.ultraTabPageControl3.Size = new System.Drawing.Size(924, 521);
            // 
            // groupBox8
            // 
            this.groupBox8.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox8.Controls.Add(this.grdPriceChangeLog);
            this.groupBox8.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.groupBox8.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox8.ForeColor = System.Drawing.Color.White;
            this.groupBox8.Location = new System.Drawing.Point(5, 3);
            this.groupBox8.Name = "groupBox8";
            this.groupBox8.Size = new System.Drawing.Size(915, 532);
            this.groupBox8.TabIndex = 30;
            this.groupBox8.TabStop = false;
            // 
            // grdPriceChangeLog
            // 
            this.grdPriceChangeLog.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            appearance136.BackColor = System.Drawing.Color.White;
            appearance136.BackColor2 = System.Drawing.Color.White;
            appearance136.BackColorDisabled = System.Drawing.Color.White;
            appearance136.BackColorDisabled2 = System.Drawing.Color.White;
            appearance136.BackGradientStyle = Infragistics.Win.GradientStyle.None;
            this.grdPriceChangeLog.DisplayLayout.Appearance = appearance136;
            ultraGridColumn6.Header.VisiblePosition = 0;
            ultraGridColumn6.Hidden = true;
            ultraGridColumn6.RowLayoutColumnInfo.PreferredCellSize = new System.Drawing.Size(34, 0);
            ultraGridColumn7.Header.VisiblePosition = 1;
            ultraGridColumn8.Header.VisiblePosition = 2;
            ultraGridColumn8.RowLayoutColumnInfo.PreferredCellSize = new System.Drawing.Size(159, 0);
            ultraGridColumn9.Header.VisiblePosition = 3;
            ultraGridColumn10.Header.VisiblePosition = 4;
            ultraGridColumn11.Header.VisiblePosition = 5;
            ultraGridColumn12.Header.VisiblePosition = 6;
            ultraGridBand2.Columns.AddRange(new object[] {
            ultraGridColumn6,
            ultraGridColumn7,
            ultraGridColumn8,
            ultraGridColumn9,
            ultraGridColumn10,
            ultraGridColumn11,
            ultraGridColumn12});
            ultraGridBand2.RowLayoutStyle = Infragistics.Win.UltraWinGrid.RowLayoutStyle.ColumnLayout;
            this.grdPriceChangeLog.DisplayLayout.BandsSerializer.Add(ultraGridBand2);
            this.grdPriceChangeLog.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.grdPriceChangeLog.DisplayLayout.InterBandSpacing = 10;
            this.grdPriceChangeLog.DisplayLayout.MaxColScrollRegions = 1;
            this.grdPriceChangeLog.DisplayLayout.MaxRowScrollRegions = 1;
            appearance137.BackColor = System.Drawing.Color.White;
            appearance137.BackColor2 = System.Drawing.Color.White;
            this.grdPriceChangeLog.DisplayLayout.Override.ActiveCardCaptionAppearance = appearance137;
            appearance138.BackColor = System.Drawing.Color.White;
            appearance138.BackColor2 = System.Drawing.Color.White;
            appearance138.BorderColor = System.Drawing.Color.Gray;
            this.grdPriceChangeLog.DisplayLayout.Override.ActiveRowAppearance = appearance138;
            appearance139.BackColor = System.Drawing.Color.White;
            appearance139.BackColor2 = System.Drawing.Color.White;
            appearance139.BorderColor = System.Drawing.Color.Gray;
            this.grdPriceChangeLog.DisplayLayout.Override.AddRowAppearance = appearance139;
            this.grdPriceChangeLog.DisplayLayout.Override.AllowAddNew = Infragistics.Win.UltraWinGrid.AllowAddNew.No;
            this.grdPriceChangeLog.DisplayLayout.Override.AllowColMoving = Infragistics.Win.UltraWinGrid.AllowColMoving.WithinBand;
            this.grdPriceChangeLog.DisplayLayout.Override.AllowColSwapping = Infragistics.Win.UltraWinGrid.AllowColSwapping.NotAllowed;
            this.grdPriceChangeLog.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.False;
            this.grdPriceChangeLog.DisplayLayout.Override.AllowRowFiltering = Infragistics.Win.DefaultableBoolean.False;
            this.grdPriceChangeLog.DisplayLayout.Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.True;
            appearance140.BackColor = System.Drawing.Color.Transparent;
            this.grdPriceChangeLog.DisplayLayout.Override.CardAreaAppearance = appearance140;
            appearance141.BackColor = System.Drawing.Color.White;
            appearance141.BackColor2 = System.Drawing.Color.White;
            appearance141.BackColorDisabled = System.Drawing.Color.White;
            appearance141.BackColorDisabled2 = System.Drawing.Color.White;
            appearance141.BorderColor = System.Drawing.Color.Black;
            appearance141.BorderColor3DBase = System.Drawing.Color.Black;
            this.grdPriceChangeLog.DisplayLayout.Override.CellAppearance = appearance141;
            appearance142.BackColor = System.Drawing.Color.White;
            appearance142.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(231)))), ((int)(((byte)(230)))));
            appearance142.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance142.BorderAlpha = Infragistics.Win.Alpha.Transparent;
            appearance142.BorderColor = System.Drawing.Color.Gray;
            appearance142.BorderColor3DBase = System.Drawing.Color.Gray;
            appearance142.ImageBackgroundAlpha = Infragistics.Win.Alpha.Transparent;
            appearance142.ImageBackgroundStyle = Infragistics.Win.ImageBackgroundStyle.Centered;
            this.grdPriceChangeLog.DisplayLayout.Override.CellButtonAppearance = appearance142;
            appearance143.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            appearance143.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.grdPriceChangeLog.DisplayLayout.Override.EditCellAppearance = appearance143;
            appearance144.BorderColor = System.Drawing.Color.Gray;
            this.grdPriceChangeLog.DisplayLayout.Override.FilteredInRowAppearance = appearance144;
            appearance145.BorderColor = System.Drawing.Color.Gray;
            this.grdPriceChangeLog.DisplayLayout.Override.FilteredOutRowAppearance = appearance145;
            appearance146.BackColor = System.Drawing.Color.White;
            appearance146.BackColor2 = System.Drawing.Color.White;
            appearance146.BackColorDisabled = System.Drawing.Color.White;
            appearance146.BackColorDisabled2 = System.Drawing.Color.White;
            this.grdPriceChangeLog.DisplayLayout.Override.FixedCellAppearance = appearance146;
            appearance147.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            appearance147.BackColor2 = System.Drawing.Color.Beige;
            this.grdPriceChangeLog.DisplayLayout.Override.FixedHeaderAppearance = appearance147;
            appearance148.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(62)))), ((int)(((byte)(176)))));
            appearance148.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance148.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance148.FontData.BoldAsString = "True";
            appearance148.FontData.SizeInPoints = 10F;
            appearance148.ForeColor = System.Drawing.Color.White;
            appearance148.TextHAlignAsString = "Left";
            appearance148.ThemedElementAlpha = Infragistics.Win.Alpha.Transparent;
            this.grdPriceChangeLog.DisplayLayout.Override.HeaderAppearance = appearance148;
            this.grdPriceChangeLog.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortSingle;
            this.grdPriceChangeLog.DisplayLayout.Override.MergedCellStyle = Infragistics.Win.UltraWinGrid.MergedCellStyle.Never;
            appearance149.BorderColor = System.Drawing.Color.Gray;
            this.grdPriceChangeLog.DisplayLayout.Override.RowAlternateAppearance = appearance149;
            appearance150.BackColor = System.Drawing.Color.White;
            appearance150.BackColor2 = System.Drawing.Color.White;
            appearance150.BackGradientStyle = Infragistics.Win.GradientStyle.None;
            appearance150.BackHatchStyle = Infragistics.Win.BackHatchStyle.None;
            appearance150.BorderColor = System.Drawing.Color.Gray;
            this.grdPriceChangeLog.DisplayLayout.Override.RowAppearance = appearance150;
            appearance151.BorderColor = System.Drawing.Color.Gray;
            this.grdPriceChangeLog.DisplayLayout.Override.RowPreviewAppearance = appearance151;
            appearance152.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(62)))), ((int)(((byte)(176)))));
            appearance152.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance152.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance152.BorderColor = System.Drawing.Color.Gray;
            this.grdPriceChangeLog.DisplayLayout.Override.RowSelectorAppearance = appearance152;
            this.grdPriceChangeLog.DisplayLayout.Override.RowSelectorWidth = 12;
            this.grdPriceChangeLog.DisplayLayout.Override.RowSizingArea = Infragistics.Win.UltraWinGrid.RowSizingArea.EntireRow;
            this.grdPriceChangeLog.DisplayLayout.Override.RowSpacingBefore = 2;
            appearance153.BackColor = System.Drawing.Color.Navy;
            appearance153.BackColorAlpha = Infragistics.Win.Alpha.Opaque;
            this.grdPriceChangeLog.DisplayLayout.Override.SelectedCellAppearance = appearance153;
            appearance154.BackColor = System.Drawing.Color.Navy;
            appearance154.BackColorDisabled = System.Drawing.Color.Navy;
            appearance154.BackGradientStyle = Infragistics.Win.GradientStyle.None;
            appearance154.BackHatchStyle = Infragistics.Win.BackHatchStyle.None;
            appearance154.BorderColor = System.Drawing.Color.Gray;
            appearance154.ForeColor = System.Drawing.Color.White;
            this.grdPriceChangeLog.DisplayLayout.Override.SelectedRowAppearance = appearance154;
            this.grdPriceChangeLog.DisplayLayout.Override.SelectTypeCell = Infragistics.Win.UltraWinGrid.SelectType.None;
            this.grdPriceChangeLog.DisplayLayout.Override.SelectTypeCol = Infragistics.Win.UltraWinGrid.SelectType.None;
            this.grdPriceChangeLog.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.None;
            appearance155.BorderColor = System.Drawing.Color.Gray;
            this.grdPriceChangeLog.DisplayLayout.Override.TemplateAddRowAppearance = appearance155;
            this.grdPriceChangeLog.DisplayLayout.RowConnectorColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            this.grdPriceChangeLog.DisplayLayout.RowConnectorStyle = Infragistics.Win.UltraWinGrid.RowConnectorStyle.Solid;
            appearance156.BackColor = System.Drawing.Color.White;
            appearance156.BackColor2 = System.Drawing.Color.Gainsboro;
            appearance156.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance156.BackHatchStyle = Infragistics.Win.BackHatchStyle.Horizontal;
            appearance156.BorderColor = System.Drawing.Color.WhiteSmoke;
            appearance156.BorderColor3DBase = System.Drawing.Color.WhiteSmoke;
            appearance156.ImageBackgroundStyle = Infragistics.Win.ImageBackgroundStyle.Stretched;
            scrollBarLook2.Appearance = appearance156;
            appearance157.BackColor = System.Drawing.Color.LightGray;
            appearance157.BackColor2 = System.Drawing.Color.WhiteSmoke;
            appearance157.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            scrollBarLook2.ButtonAppearance = appearance157;
            appearance158.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            scrollBarLook2.ThumbAppearance = appearance158;
            appearance159.BackColor = System.Drawing.Color.Gainsboro;
            appearance159.BackColor2 = System.Drawing.Color.WhiteSmoke;
            appearance159.BackGradientStyle = Infragistics.Win.GradientStyle.None;
            appearance159.BackHatchStyle = Infragistics.Win.BackHatchStyle.None;
            appearance159.BorderAlpha = Infragistics.Win.Alpha.UseAlphaLevel;
            appearance159.BorderColor = System.Drawing.Color.White;
            appearance159.BorderColor3DBase = System.Drawing.Color.Gainsboro;
            appearance159.ImageBackgroundOrigin = Infragistics.Win.ImageBackgroundOrigin.Form;
            scrollBarLook2.TrackAppearance = appearance159;
            this.grdPriceChangeLog.DisplayLayout.ScrollBarLook = scrollBarLook2;
            this.grdPriceChangeLog.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.grdPriceChangeLog.Location = new System.Drawing.Point(14, 24);
            this.grdPriceChangeLog.Name = "grdPriceChangeLog";
            this.grdPriceChangeLog.Size = new System.Drawing.Size(889, 496);
            this.grdPriceChangeLog.TabIndex = 5;
            this.grdPriceChangeLog.UpdateMode = Infragistics.Win.UltraWinGrid.UpdateMode.OnCellChangeOrLostFocus;
            this.grdPriceChangeLog.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // ultraTabPageControl4
            // 
            this.ultraTabPageControl4.Controls.Add(this.groupBox9);
            this.ultraTabPageControl4.Location = new System.Drawing.Point(-10000, -10000);
            this.ultraTabPageControl4.Name = "ultraTabPageControl4";
            this.ultraTabPageControl4.Size = new System.Drawing.Size(924, 521);
            // 
            // groupBox9
            // 
            this.groupBox9.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox9.Controls.Add(this.grdPhysicalInvView);
            this.groupBox9.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.groupBox9.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox9.ForeColor = System.Drawing.Color.White;
            this.groupBox9.Location = new System.Drawing.Point(5, 3);
            this.groupBox9.Name = "groupBox9";
            this.groupBox9.Size = new System.Drawing.Size(913, 532);
            this.groupBox9.TabIndex = 30;
            this.groupBox9.TabStop = false;
            // 
            // grdPhysicalInvView
            // 
            this.grdPhysicalInvView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            appearance160.BackColor = System.Drawing.Color.White;
            appearance160.BackColor2 = System.Drawing.Color.White;
            appearance160.BackColorDisabled = System.Drawing.Color.White;
            appearance160.BackColorDisabled2 = System.Drawing.Color.White;
            appearance160.BackGradientStyle = Infragistics.Win.GradientStyle.None;
            this.grdPhysicalInvView.DisplayLayout.Appearance = appearance160;
            ultraGridColumn13.Header.VisiblePosition = 0;
            ultraGridColumn13.Hidden = true;
            ultraGridColumn13.RowLayoutColumnInfo.PreferredCellSize = new System.Drawing.Size(34, 0);
            ultraGridColumn14.Header.VisiblePosition = 1;
            ultraGridColumn15.Header.VisiblePosition = 2;
            ultraGridColumn15.RowLayoutColumnInfo.PreferredCellSize = new System.Drawing.Size(159, 0);
            ultraGridColumn16.Header.VisiblePosition = 3;
            ultraGridColumn17.Header.VisiblePosition = 4;
            ultraGridColumn17.RowLayoutColumnInfo.PreferredCellSize = new System.Drawing.Size(108, 0);
            ultraGridColumn18.Header.VisiblePosition = 5;
            ultraGridColumn18.Hidden = true;
            ultraGridColumn19.Header.VisiblePosition = 6;
            ultraGridColumn20.Header.VisiblePosition = 7;
            ultraGridColumn21.Header.VisiblePosition = 8;
            ultraGridColumn21.Hidden = true;
            ultraGridColumn22.Header.VisiblePosition = 9;
            ultraGridColumn22.Hidden = true;
            ultraGridBand3.Columns.AddRange(new object[] {
            ultraGridColumn13,
            ultraGridColumn14,
            ultraGridColumn15,
            ultraGridColumn16,
            ultraGridColumn17,
            ultraGridColumn18,
            ultraGridColumn19,
            ultraGridColumn20,
            ultraGridColumn21,
            ultraGridColumn22});
            ultraGridBand3.RowLayoutStyle = Infragistics.Win.UltraWinGrid.RowLayoutStyle.ColumnLayout;
            this.grdPhysicalInvView.DisplayLayout.BandsSerializer.Add(ultraGridBand3);
            this.grdPhysicalInvView.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.grdPhysicalInvView.DisplayLayout.InterBandSpacing = 10;
            this.grdPhysicalInvView.DisplayLayout.MaxColScrollRegions = 1;
            this.grdPhysicalInvView.DisplayLayout.MaxRowScrollRegions = 1;
            appearance161.BackColor = System.Drawing.Color.White;
            appearance161.BackColor2 = System.Drawing.Color.White;
            this.grdPhysicalInvView.DisplayLayout.Override.ActiveCardCaptionAppearance = appearance161;
            appearance162.BackColor = System.Drawing.Color.White;
            appearance162.BackColor2 = System.Drawing.Color.White;
            appearance162.BorderColor = System.Drawing.Color.Gray;
            this.grdPhysicalInvView.DisplayLayout.Override.ActiveRowAppearance = appearance162;
            appearance163.BackColor = System.Drawing.Color.White;
            appearance163.BackColor2 = System.Drawing.Color.White;
            appearance163.BorderColor = System.Drawing.Color.Gray;
            this.grdPhysicalInvView.DisplayLayout.Override.AddRowAppearance = appearance163;
            this.grdPhysicalInvView.DisplayLayout.Override.AllowAddNew = Infragistics.Win.UltraWinGrid.AllowAddNew.No;
            this.grdPhysicalInvView.DisplayLayout.Override.AllowColMoving = Infragistics.Win.UltraWinGrid.AllowColMoving.WithinBand;
            this.grdPhysicalInvView.DisplayLayout.Override.AllowColSwapping = Infragistics.Win.UltraWinGrid.AllowColSwapping.NotAllowed;
            this.grdPhysicalInvView.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.False;
            this.grdPhysicalInvView.DisplayLayout.Override.AllowRowFiltering = Infragistics.Win.DefaultableBoolean.False;
            this.grdPhysicalInvView.DisplayLayout.Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.True;
            appearance164.BackColor = System.Drawing.Color.Transparent;
            this.grdPhysicalInvView.DisplayLayout.Override.CardAreaAppearance = appearance164;
            appearance165.BackColor = System.Drawing.Color.White;
            appearance165.BackColor2 = System.Drawing.Color.White;
            appearance165.BackColorDisabled = System.Drawing.Color.White;
            appearance165.BackColorDisabled2 = System.Drawing.Color.White;
            appearance165.BorderColor = System.Drawing.Color.Black;
            appearance165.BorderColor3DBase = System.Drawing.Color.Black;
            this.grdPhysicalInvView.DisplayLayout.Override.CellAppearance = appearance165;
            appearance166.BackColor = System.Drawing.Color.White;
            appearance166.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(231)))), ((int)(((byte)(230)))));
            appearance166.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance166.BorderAlpha = Infragistics.Win.Alpha.Transparent;
            appearance166.BorderColor = System.Drawing.Color.Gray;
            appearance166.BorderColor3DBase = System.Drawing.Color.Gray;
            appearance166.ImageBackgroundAlpha = Infragistics.Win.Alpha.Transparent;
            appearance166.ImageBackgroundStyle = Infragistics.Win.ImageBackgroundStyle.Centered;
            this.grdPhysicalInvView.DisplayLayout.Override.CellButtonAppearance = appearance166;
            appearance167.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            appearance167.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.grdPhysicalInvView.DisplayLayout.Override.EditCellAppearance = appearance167;
            appearance168.BorderColor = System.Drawing.Color.Gray;
            this.grdPhysicalInvView.DisplayLayout.Override.FilteredInRowAppearance = appearance168;
            appearance169.BorderColor = System.Drawing.Color.Gray;
            this.grdPhysicalInvView.DisplayLayout.Override.FilteredOutRowAppearance = appearance169;
            appearance170.BackColor = System.Drawing.Color.White;
            appearance170.BackColor2 = System.Drawing.Color.White;
            appearance170.BackColorDisabled = System.Drawing.Color.White;
            appearance170.BackColorDisabled2 = System.Drawing.Color.White;
            this.grdPhysicalInvView.DisplayLayout.Override.FixedCellAppearance = appearance170;
            appearance171.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            appearance171.BackColor2 = System.Drawing.Color.Beige;
            this.grdPhysicalInvView.DisplayLayout.Override.FixedHeaderAppearance = appearance171;
            appearance172.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(62)))), ((int)(((byte)(176)))));
            appearance172.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance172.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance172.FontData.BoldAsString = "True";
            appearance172.FontData.SizeInPoints = 10F;
            appearance172.ForeColor = System.Drawing.Color.White;
            appearance172.TextHAlignAsString = "Left";
            appearance172.ThemedElementAlpha = Infragistics.Win.Alpha.Transparent;
            this.grdPhysicalInvView.DisplayLayout.Override.HeaderAppearance = appearance172;
            this.grdPhysicalInvView.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortSingle;
            this.grdPhysicalInvView.DisplayLayout.Override.MergedCellStyle = Infragistics.Win.UltraWinGrid.MergedCellStyle.Never;
            appearance173.BorderColor = System.Drawing.Color.Gray;
            this.grdPhysicalInvView.DisplayLayout.Override.RowAlternateAppearance = appearance173;
            appearance174.BackColor = System.Drawing.Color.White;
            appearance174.BackColor2 = System.Drawing.Color.White;
            appearance174.BackGradientStyle = Infragistics.Win.GradientStyle.None;
            appearance174.BackHatchStyle = Infragistics.Win.BackHatchStyle.None;
            appearance174.BorderColor = System.Drawing.Color.Gray;
            this.grdPhysicalInvView.DisplayLayout.Override.RowAppearance = appearance174;
            appearance175.BorderColor = System.Drawing.Color.Gray;
            this.grdPhysicalInvView.DisplayLayout.Override.RowPreviewAppearance = appearance175;
            appearance176.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(62)))), ((int)(((byte)(176)))));
            appearance176.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance176.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance176.BorderColor = System.Drawing.Color.Gray;
            this.grdPhysicalInvView.DisplayLayout.Override.RowSelectorAppearance = appearance176;
            this.grdPhysicalInvView.DisplayLayout.Override.RowSelectorWidth = 12;
            this.grdPhysicalInvView.DisplayLayout.Override.RowSizingArea = Infragistics.Win.UltraWinGrid.RowSizingArea.EntireRow;
            this.grdPhysicalInvView.DisplayLayout.Override.RowSpacingBefore = 2;
            appearance177.BackColor = System.Drawing.Color.Navy;
            appearance177.BackColorAlpha = Infragistics.Win.Alpha.Opaque;
            this.grdPhysicalInvView.DisplayLayout.Override.SelectedCellAppearance = appearance177;
            appearance178.BackColor = System.Drawing.Color.Navy;
            appearance178.BackColorDisabled = System.Drawing.Color.Navy;
            appearance178.BackGradientStyle = Infragistics.Win.GradientStyle.None;
            appearance178.BackHatchStyle = Infragistics.Win.BackHatchStyle.None;
            appearance178.BorderColor = System.Drawing.Color.Gray;
            appearance178.ForeColor = System.Drawing.Color.White;
            this.grdPhysicalInvView.DisplayLayout.Override.SelectedRowAppearance = appearance178;
            this.grdPhysicalInvView.DisplayLayout.Override.SelectTypeCell = Infragistics.Win.UltraWinGrid.SelectType.None;
            this.grdPhysicalInvView.DisplayLayout.Override.SelectTypeCol = Infragistics.Win.UltraWinGrid.SelectType.None;
            this.grdPhysicalInvView.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.None;
            appearance179.BorderColor = System.Drawing.Color.Gray;
            this.grdPhysicalInvView.DisplayLayout.Override.TemplateAddRowAppearance = appearance179;
            this.grdPhysicalInvView.DisplayLayout.RowConnectorColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            this.grdPhysicalInvView.DisplayLayout.RowConnectorStyle = Infragistics.Win.UltraWinGrid.RowConnectorStyle.Solid;
            appearance180.BackColor = System.Drawing.Color.White;
            appearance180.BackColor2 = System.Drawing.Color.Gainsboro;
            appearance180.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance180.BackHatchStyle = Infragistics.Win.BackHatchStyle.Horizontal;
            appearance180.BorderColor = System.Drawing.Color.WhiteSmoke;
            appearance180.BorderColor3DBase = System.Drawing.Color.WhiteSmoke;
            appearance180.ImageBackgroundStyle = Infragistics.Win.ImageBackgroundStyle.Stretched;
            scrollBarLook3.Appearance = appearance180;
            appearance181.BackColor = System.Drawing.Color.LightGray;
            appearance181.BackColor2 = System.Drawing.Color.WhiteSmoke;
            appearance181.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            scrollBarLook3.ButtonAppearance = appearance181;
            appearance182.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            scrollBarLook3.ThumbAppearance = appearance182;
            appearance183.BackColor = System.Drawing.Color.Gainsboro;
            appearance183.BackColor2 = System.Drawing.Color.WhiteSmoke;
            appearance183.BackGradientStyle = Infragistics.Win.GradientStyle.None;
            appearance183.BackHatchStyle = Infragistics.Win.BackHatchStyle.None;
            appearance183.BorderAlpha = Infragistics.Win.Alpha.UseAlphaLevel;
            appearance183.BorderColor = System.Drawing.Color.White;
            appearance183.BorderColor3DBase = System.Drawing.Color.Gainsboro;
            appearance183.ImageBackgroundOrigin = Infragistics.Win.ImageBackgroundOrigin.Form;
            scrollBarLook3.TrackAppearance = appearance183;
            this.grdPhysicalInvView.DisplayLayout.ScrollBarLook = scrollBarLook3;
            this.grdPhysicalInvView.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.grdPhysicalInvView.Location = new System.Drawing.Point(14, 24);
            this.grdPhysicalInvView.Name = "grdPhysicalInvView";
            this.grdPhysicalInvView.Size = new System.Drawing.Size(887, 496);
            this.grdPhysicalInvView.TabIndex = 5;
            this.grdPhysicalInvView.UpdateMode = Infragistics.Win.UltraWinGrid.UpdateMode.OnCellChangeOrLostFocus;
            this.grdPhysicalInvView.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // ultraTabPageControl6
            // 
            this.ultraTabPageControl6.Controls.Add(this.tableLayoutPanel1);
            this.ultraTabPageControl6.Location = new System.Drawing.Point(-10000, -10000);
            this.ultraTabPageControl6.Name = "ultraTabPageControl6";
            this.ultraTabPageControl6.Size = new System.Drawing.Size(924, 521);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 74.48201F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25.51799F));
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel5, 1, 0);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(4, 3);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(917, 515);
            this.tableLayoutPanel1.TabIndex = 3;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 1;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Controls.Add(this.tableLayoutPanel3, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.tableLayoutPanel4, 0, 0);
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 2;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 18.86051F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 81.13949F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(677, 509);
            this.tableLayoutPanel2.TabIndex = 0;
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 1;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.Controls.Add(this.grdVendor, 0, 1);
            this.tableLayoutPanel3.Controls.Add(this.ucSummary, 0, 0);
            this.tableLayoutPanel3.Location = new System.Drawing.Point(3, 98);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 2;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(671, 402);
            this.tableLayoutPanel3.TabIndex = 0;
            // 
            // grdVendor
            // 
            appearance184.BackColor = System.Drawing.SystemColors.Window;
            appearance184.BorderColor = System.Drawing.Color.DarkBlue;
            this.grdVendor.DisplayLayout.Appearance = appearance184;
            this.grdVendor.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ResizeAllColumns;
            appearance185.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            appearance185.FontData.BoldAsString = "True";
            appearance185.ForeColor = System.Drawing.Color.White;
            ultraGridBand4.Override.ActiveRowAppearance = appearance185;
            this.grdVendor.DisplayLayout.BandsSerializer.Add(ultraGridBand4);
            this.grdVendor.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.grdVendor.DisplayLayout.BorderStyleCaption = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance186.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(1)))), ((int)(((byte)(68)))), ((int)(((byte)(97)))));
            appearance186.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance186.BorderColor = System.Drawing.Color.Black;
            appearance186.FontData.BoldAsString = "True";
            appearance186.FontData.SizeInPoints = 10F;
            appearance186.ForeColor = System.Drawing.Color.White;
            this.grdVendor.DisplayLayout.CaptionAppearance = appearance186;
            this.grdVendor.DisplayLayout.DefaultSelectedBackColor = System.Drawing.SystemColors.HighlightText;
            appearance187.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance187.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance187.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance187.BorderColor = System.Drawing.SystemColors.Window;
            this.grdVendor.DisplayLayout.GroupByBox.Appearance = appearance187;
            appearance188.ForeColor = System.Drawing.SystemColors.GrayText;
            this.grdVendor.DisplayLayout.GroupByBox.BandLabelAppearance = appearance188;
            this.grdVendor.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance189.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance189.BackColor2 = System.Drawing.SystemColors.Control;
            appearance189.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance189.ForeColor = System.Drawing.SystemColors.GrayText;
            this.grdVendor.DisplayLayout.GroupByBox.PromptAppearance = appearance189;
            this.grdVendor.DisplayLayout.MaxColScrollRegions = 1;
            this.grdVendor.DisplayLayout.MaxRowScrollRegions = 1;
            this.grdVendor.DisplayLayout.Override.ActiveAppearancesEnabled = Infragistics.Win.DefaultableBoolean.False;
            appearance190.BackColor = System.Drawing.SystemColors.Window;
            appearance190.ForeColor = System.Drawing.SystemColors.ControlText;
            this.grdVendor.DisplayLayout.Override.ActiveCellAppearance = appearance190;
            appearance191.BackColor = System.Drawing.SystemColors.Highlight;
            appearance191.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.grdVendor.DisplayLayout.Override.ActiveRowAppearance = appearance191;
            this.grdVendor.DisplayLayout.Override.AllowAddNew = Infragistics.Win.UltraWinGrid.AllowAddNew.No;
            this.grdVendor.DisplayLayout.Override.AllowColMoving = Infragistics.Win.UltraWinGrid.AllowColMoving.NotAllowed;
            this.grdVendor.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.False;
            this.grdVendor.DisplayLayout.Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.False;
            appearance192.BackColor = System.Drawing.SystemColors.Window;
            this.grdVendor.DisplayLayout.Override.CardAreaAppearance = appearance192;
            appearance193.BorderColor = System.Drawing.Color.Silver;
            appearance193.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.grdVendor.DisplayLayout.Override.CellAppearance = appearance193;
            this.grdVendor.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;
            this.grdVendor.DisplayLayout.Override.CellPadding = 0;
            appearance194.BackColor = System.Drawing.SystemColors.Control;
            appearance194.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance194.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance194.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance194.BorderColor = System.Drawing.SystemColors.Window;
            this.grdVendor.DisplayLayout.Override.GroupByRowAppearance = appearance194;
            appearance195.BackColor = System.Drawing.Color.DodgerBlue;
            appearance195.BackColor2 = System.Drawing.Color.Azure;
            appearance195.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance195.FontData.BoldAsString = "True";
            appearance195.TextHAlignAsString = "Left";
            this.grdVendor.DisplayLayout.Override.HeaderAppearance = appearance195;
            this.grdVendor.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortSingle;
            appearance196.BackColor = System.Drawing.Color.LemonChiffon;
            this.grdVendor.DisplayLayout.Override.RowAlternateAppearance = appearance196;
            appearance197.BackColor = System.Drawing.Color.White;
            appearance197.BorderColor = System.Drawing.Color.Silver;
            this.grdVendor.DisplayLayout.Override.RowAppearance = appearance197;
            this.grdVendor.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            this.grdVendor.DisplayLayout.Override.SelectedAppearancesEnabled = Infragistics.Win.DefaultableBoolean.False;
            appearance198.BackColor = System.Drawing.SystemColors.ControlLight;
            this.grdVendor.DisplayLayout.Override.TemplateAddRowAppearance = appearance198;
            this.grdVendor.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.grdVendor.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.grdVendor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdVendor.Location = new System.Drawing.Point(3, 204);
            this.grdVendor.Name = "grdVendor";
            this.grdVendor.Size = new System.Drawing.Size(665, 195);
            this.grdVendor.TabIndex = 100;
            this.grdVendor.Text = "Vendor";
            this.grdVendor.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // ucSummary
            // 
            this.ucSummary.Axis.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(248)))), ((int)(((byte)(220)))));
            paintElement1.ElementType = Infragistics.UltraChart.Shared.Styles.PaintElementType.None;
            paintElement1.Fill = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(248)))), ((int)(((byte)(220)))));
            this.ucSummary.Axis.PE = paintElement1;
            this.ucSummary.Axis.X.Extent = 27;
            this.ucSummary.Axis.X.Labels.HorizontalAlign = System.Drawing.StringAlignment.Near;
            this.ucSummary.Axis.X.Labels.ItemFormatString = "";
            this.ucSummary.Axis.X.Labels.Orientation = Infragistics.UltraChart.Shared.Styles.TextOrientation.VerticalLeftFacing;
            this.ucSummary.Axis.X.Labels.SeriesLabels.Font = new System.Drawing.Font("Verdana", 7F);
            this.ucSummary.Axis.X.Labels.SeriesLabels.FontColor = System.Drawing.Color.DimGray;
            this.ucSummary.Axis.X.Labels.SeriesLabels.HorizontalAlign = System.Drawing.StringAlignment.Center;
            this.ucSummary.Axis.X.Labels.SeriesLabels.Layout.Behavior = Infragistics.UltraChart.Shared.Styles.AxisLabelLayoutBehaviors.Auto;
            this.ucSummary.Axis.X.Labels.SeriesLabels.Orientation = Infragistics.UltraChart.Shared.Styles.TextOrientation.Horizontal;
            this.ucSummary.Axis.X.Labels.SeriesLabels.VerticalAlign = System.Drawing.StringAlignment.Center;
            this.ucSummary.Axis.X.Labels.SeriesLabels.Visible = true;
            this.ucSummary.Axis.X.Labels.VerticalAlign = System.Drawing.StringAlignment.Near;
            this.ucSummary.Axis.X.Labels.Visible = false;
            this.ucSummary.Axis.X.MajorGridLines.AlphaLevel = ((byte)(255));
            this.ucSummary.Axis.X.MajorGridLines.Color = System.Drawing.Color.Gainsboro;
            this.ucSummary.Axis.X.MajorGridLines.DrawStyle = Infragistics.UltraChart.Shared.Styles.LineDrawStyle.Dot;
            this.ucSummary.Axis.X.MajorGridLines.Visible = true;
            this.ucSummary.Axis.X.MinorGridLines.AlphaLevel = ((byte)(255));
            this.ucSummary.Axis.X.MinorGridLines.Color = System.Drawing.Color.LightGray;
            this.ucSummary.Axis.X.MinorGridLines.DrawStyle = Infragistics.UltraChart.Shared.Styles.LineDrawStyle.Dot;
            this.ucSummary.Axis.X.MinorGridLines.Visible = false;
            this.ucSummary.Axis.X.Visible = true;
            this.ucSummary.Axis.X2.Labels.Font = new System.Drawing.Font("Verdana", 7F);
            this.ucSummary.Axis.X2.Labels.FontColor = System.Drawing.Color.Gray;
            this.ucSummary.Axis.X2.Labels.HorizontalAlign = System.Drawing.StringAlignment.Far;
            this.ucSummary.Axis.X2.Labels.ItemFormatString = "<ITEM_LABEL>";
            this.ucSummary.Axis.X2.Labels.Layout.Behavior = Infragistics.UltraChart.Shared.Styles.AxisLabelLayoutBehaviors.Auto;
            this.ucSummary.Axis.X2.Labels.Orientation = Infragistics.UltraChart.Shared.Styles.TextOrientation.VerticalLeftFacing;
            this.ucSummary.Axis.X2.Labels.SeriesLabels.Font = new System.Drawing.Font("Verdana", 7F);
            this.ucSummary.Axis.X2.Labels.SeriesLabels.FontColor = System.Drawing.Color.Gray;
            this.ucSummary.Axis.X2.Labels.SeriesLabels.HorizontalAlign = System.Drawing.StringAlignment.Center;
            this.ucSummary.Axis.X2.Labels.SeriesLabels.Layout.Behavior = Infragistics.UltraChart.Shared.Styles.AxisLabelLayoutBehaviors.Auto;
            this.ucSummary.Axis.X2.Labels.SeriesLabels.Orientation = Infragistics.UltraChart.Shared.Styles.TextOrientation.Horizontal;
            this.ucSummary.Axis.X2.Labels.SeriesLabels.VerticalAlign = System.Drawing.StringAlignment.Center;
            this.ucSummary.Axis.X2.Labels.SeriesLabels.Visible = true;
            this.ucSummary.Axis.X2.Labels.VerticalAlign = System.Drawing.StringAlignment.Center;
            this.ucSummary.Axis.X2.Labels.Visible = false;
            this.ucSummary.Axis.X2.LineThickness = 1;
            this.ucSummary.Axis.X2.MajorGridLines.AlphaLevel = ((byte)(255));
            this.ucSummary.Axis.X2.MajorGridLines.Color = System.Drawing.Color.Gainsboro;
            this.ucSummary.Axis.X2.MajorGridLines.DrawStyle = Infragistics.UltraChart.Shared.Styles.LineDrawStyle.Dot;
            this.ucSummary.Axis.X2.MajorGridLines.Visible = true;
            this.ucSummary.Axis.X2.MinorGridLines.AlphaLevel = ((byte)(255));
            this.ucSummary.Axis.X2.MinorGridLines.Color = System.Drawing.Color.LightGray;
            this.ucSummary.Axis.X2.MinorGridLines.DrawStyle = Infragistics.UltraChart.Shared.Styles.LineDrawStyle.Dot;
            this.ucSummary.Axis.X2.MinorGridLines.Visible = false;
            this.ucSummary.Axis.X2.Visible = false;
            this.ucSummary.Axis.Y.Extent = 55;
            this.ucSummary.Axis.Y.Labels.FontColor = System.Drawing.Color.DimGray;
            this.ucSummary.Axis.Y.Labels.HorizontalAlign = System.Drawing.StringAlignment.Far;
            this.ucSummary.Axis.Y.Labels.ItemFormatString = "<DATA_VALUE:00.##>";
            this.ucSummary.Axis.Y.Labels.Orientation = Infragistics.UltraChart.Shared.Styles.TextOrientation.Horizontal;
            this.ucSummary.Axis.Y.Labels.SeriesLabels.Font = new System.Drawing.Font("Verdana", 7F);
            this.ucSummary.Axis.Y.Labels.SeriesLabels.FontColor = System.Drawing.Color.DimGray;
            this.ucSummary.Axis.Y.Labels.SeriesLabels.FormatString = "";
            this.ucSummary.Axis.Y.Labels.SeriesLabels.HorizontalAlign = System.Drawing.StringAlignment.Far;
            this.ucSummary.Axis.Y.Labels.SeriesLabels.Layout.Behavior = Infragistics.UltraChart.Shared.Styles.AxisLabelLayoutBehaviors.Auto;
            this.ucSummary.Axis.Y.Labels.SeriesLabels.Orientation = Infragistics.UltraChart.Shared.Styles.TextOrientation.VerticalLeftFacing;
            this.ucSummary.Axis.Y.Labels.SeriesLabels.VerticalAlign = System.Drawing.StringAlignment.Center;
            this.ucSummary.Axis.Y.Labels.SeriesLabels.Visible = true;
            this.ucSummary.Axis.Y.Labels.VerticalAlign = System.Drawing.StringAlignment.Center;
            this.ucSummary.Axis.Y.Labels.Visible = true;
            this.ucSummary.Axis.Y.MajorGridLines.AlphaLevel = ((byte)(255));
            this.ucSummary.Axis.Y.MajorGridLines.Color = System.Drawing.Color.Gainsboro;
            this.ucSummary.Axis.Y.MajorGridLines.DrawStyle = Infragistics.UltraChart.Shared.Styles.LineDrawStyle.Dot;
            this.ucSummary.Axis.Y.MajorGridLines.Visible = true;
            this.ucSummary.Axis.Y.MinorGridLines.AlphaLevel = ((byte)(255));
            this.ucSummary.Axis.Y.MinorGridLines.Color = System.Drawing.Color.LightGray;
            this.ucSummary.Axis.Y.MinorGridLines.DrawStyle = Infragistics.UltraChart.Shared.Styles.LineDrawStyle.Dot;
            this.ucSummary.Axis.Y.MinorGridLines.Visible = false;
            this.ucSummary.Axis.Y.Visible = true;
            this.ucSummary.Axis.Y2.Labels.Font = new System.Drawing.Font("Verdana", 7F);
            this.ucSummary.Axis.Y2.Labels.FontColor = System.Drawing.Color.Gray;
            this.ucSummary.Axis.Y2.Labels.HorizontalAlign = System.Drawing.StringAlignment.Near;
            this.ucSummary.Axis.Y2.Labels.ItemFormatString = "<DATA_VALUE:00.##>";
            this.ucSummary.Axis.Y2.Labels.Layout.Behavior = Infragistics.UltraChart.Shared.Styles.AxisLabelLayoutBehaviors.Auto;
            this.ucSummary.Axis.Y2.Labels.Orientation = Infragistics.UltraChart.Shared.Styles.TextOrientation.Horizontal;
            this.ucSummary.Axis.Y2.Labels.SeriesLabels.Font = new System.Drawing.Font("Verdana", 7F);
            this.ucSummary.Axis.Y2.Labels.SeriesLabels.FontColor = System.Drawing.Color.Gray;
            this.ucSummary.Axis.Y2.Labels.SeriesLabels.FormatString = "";
            this.ucSummary.Axis.Y2.Labels.SeriesLabels.HorizontalAlign = System.Drawing.StringAlignment.Near;
            this.ucSummary.Axis.Y2.Labels.SeriesLabels.Layout.Behavior = Infragistics.UltraChart.Shared.Styles.AxisLabelLayoutBehaviors.Auto;
            this.ucSummary.Axis.Y2.Labels.SeriesLabels.Orientation = Infragistics.UltraChart.Shared.Styles.TextOrientation.VerticalLeftFacing;
            this.ucSummary.Axis.Y2.Labels.SeriesLabels.VerticalAlign = System.Drawing.StringAlignment.Center;
            this.ucSummary.Axis.Y2.Labels.SeriesLabels.Visible = true;
            this.ucSummary.Axis.Y2.Labels.VerticalAlign = System.Drawing.StringAlignment.Center;
            this.ucSummary.Axis.Y2.Labels.Visible = false;
            this.ucSummary.Axis.Y2.LineThickness = 1;
            this.ucSummary.Axis.Y2.MajorGridLines.AlphaLevel = ((byte)(255));
            this.ucSummary.Axis.Y2.MajorGridLines.Color = System.Drawing.Color.Gainsboro;
            this.ucSummary.Axis.Y2.MajorGridLines.DrawStyle = Infragistics.UltraChart.Shared.Styles.LineDrawStyle.Dot;
            this.ucSummary.Axis.Y2.MajorGridLines.Visible = true;
            this.ucSummary.Axis.Y2.MinorGridLines.AlphaLevel = ((byte)(255));
            this.ucSummary.Axis.Y2.MinorGridLines.Color = System.Drawing.Color.LightGray;
            this.ucSummary.Axis.Y2.MinorGridLines.DrawStyle = Infragistics.UltraChart.Shared.Styles.LineDrawStyle.Dot;
            this.ucSummary.Axis.Y2.MinorGridLines.Visible = false;
            this.ucSummary.Axis.Y2.TickmarkInterval = 100D;
            this.ucSummary.Axis.Y2.Visible = false;
            this.ucSummary.Axis.Z.Labels.Font = new System.Drawing.Font("Verdana", 7F);
            this.ucSummary.Axis.Z.Labels.FontColor = System.Drawing.Color.DimGray;
            this.ucSummary.Axis.Z.Labels.HorizontalAlign = System.Drawing.StringAlignment.Near;
            this.ucSummary.Axis.Z.Labels.ItemFormatString = "<ITEM_LABEL>";
            this.ucSummary.Axis.Z.Labels.Layout.Behavior = Infragistics.UltraChart.Shared.Styles.AxisLabelLayoutBehaviors.Auto;
            this.ucSummary.Axis.Z.Labels.Orientation = Infragistics.UltraChart.Shared.Styles.TextOrientation.Horizontal;
            this.ucSummary.Axis.Z.Labels.SeriesLabels.Font = new System.Drawing.Font("Verdana", 7F);
            this.ucSummary.Axis.Z.Labels.SeriesLabels.FontColor = System.Drawing.Color.DimGray;
            this.ucSummary.Axis.Z.Labels.SeriesLabels.HorizontalAlign = System.Drawing.StringAlignment.Near;
            this.ucSummary.Axis.Z.Labels.SeriesLabels.Layout.Behavior = Infragistics.UltraChart.Shared.Styles.AxisLabelLayoutBehaviors.Auto;
            this.ucSummary.Axis.Z.Labels.SeriesLabels.Orientation = Infragistics.UltraChart.Shared.Styles.TextOrientation.Horizontal;
            this.ucSummary.Axis.Z.Labels.SeriesLabels.VerticalAlign = System.Drawing.StringAlignment.Center;
            this.ucSummary.Axis.Z.Labels.SeriesLabels.Visible = true;
            this.ucSummary.Axis.Z.Labels.VerticalAlign = System.Drawing.StringAlignment.Center;
            this.ucSummary.Axis.Z.Labels.Visible = false;
            this.ucSummary.Axis.Z.LineThickness = 1;
            this.ucSummary.Axis.Z.MajorGridLines.AlphaLevel = ((byte)(255));
            this.ucSummary.Axis.Z.MajorGridLines.Color = System.Drawing.Color.Gainsboro;
            this.ucSummary.Axis.Z.MajorGridLines.DrawStyle = Infragistics.UltraChart.Shared.Styles.LineDrawStyle.Dot;
            this.ucSummary.Axis.Z.MajorGridLines.Visible = true;
            this.ucSummary.Axis.Z.MinorGridLines.AlphaLevel = ((byte)(255));
            this.ucSummary.Axis.Z.MinorGridLines.Color = System.Drawing.Color.LightGray;
            this.ucSummary.Axis.Z.MinorGridLines.DrawStyle = Infragistics.UltraChart.Shared.Styles.LineDrawStyle.Dot;
            this.ucSummary.Axis.Z.MinorGridLines.Visible = false;
            this.ucSummary.Axis.Z.Visible = false;
            this.ucSummary.Axis.Z2.Labels.Font = new System.Drawing.Font("Verdana", 7F);
            this.ucSummary.Axis.Z2.Labels.FontColor = System.Drawing.Color.Gray;
            this.ucSummary.Axis.Z2.Labels.HorizontalAlign = System.Drawing.StringAlignment.Near;
            this.ucSummary.Axis.Z2.Labels.ItemFormatString = "<ITEM_LABEL>";
            this.ucSummary.Axis.Z2.Labels.Layout.Behavior = Infragistics.UltraChart.Shared.Styles.AxisLabelLayoutBehaviors.Auto;
            this.ucSummary.Axis.Z2.Labels.Orientation = Infragistics.UltraChart.Shared.Styles.TextOrientation.Horizontal;
            this.ucSummary.Axis.Z2.Labels.SeriesLabels.Font = new System.Drawing.Font("Verdana", 7F);
            this.ucSummary.Axis.Z2.Labels.SeriesLabels.FontColor = System.Drawing.Color.Gray;
            this.ucSummary.Axis.Z2.Labels.SeriesLabels.HorizontalAlign = System.Drawing.StringAlignment.Near;
            this.ucSummary.Axis.Z2.Labels.SeriesLabels.Layout.Behavior = Infragistics.UltraChart.Shared.Styles.AxisLabelLayoutBehaviors.Auto;
            this.ucSummary.Axis.Z2.Labels.SeriesLabels.Orientation = Infragistics.UltraChart.Shared.Styles.TextOrientation.Horizontal;
            this.ucSummary.Axis.Z2.Labels.SeriesLabels.VerticalAlign = System.Drawing.StringAlignment.Center;
            this.ucSummary.Axis.Z2.Labels.SeriesLabels.Visible = true;
            this.ucSummary.Axis.Z2.Labels.VerticalAlign = System.Drawing.StringAlignment.Center;
            this.ucSummary.Axis.Z2.Labels.Visible = false;
            this.ucSummary.Axis.Z2.LineThickness = 1;
            this.ucSummary.Axis.Z2.MajorGridLines.AlphaLevel = ((byte)(255));
            this.ucSummary.Axis.Z2.MajorGridLines.Color = System.Drawing.Color.Gainsboro;
            this.ucSummary.Axis.Z2.MajorGridLines.DrawStyle = Infragistics.UltraChart.Shared.Styles.LineDrawStyle.Dot;
            this.ucSummary.Axis.Z2.MajorGridLines.Visible = true;
            this.ucSummary.Axis.Z2.MinorGridLines.AlphaLevel = ((byte)(255));
            this.ucSummary.Axis.Z2.MinorGridLines.Color = System.Drawing.Color.LightGray;
            this.ucSummary.Axis.Z2.MinorGridLines.DrawStyle = Infragistics.UltraChart.Shared.Styles.LineDrawStyle.Dot;
            this.ucSummary.Axis.Z2.MinorGridLines.Visible = false;
            this.ucSummary.Axis.Z2.Visible = false;
            this.ucSummary.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.ucSummary.ColorModel.AlphaLevel = ((byte)(150));
            this.ucSummary.ColorModel.ModelStyle = Infragistics.UltraChart.Shared.Styles.ColorModels.CustomLinear;
            this.ucSummary.Data.EmptyStyle.LineStyle.DrawStyle = Infragistics.UltraChart.Shared.Styles.LineDrawStyle.Dash;
            this.ucSummary.Data.ZeroAligned = true;
            this.ucSummary.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ucSummary.Effects.Effects.Add(gradientEffect1);
            this.ucSummary.EmptyChartText = "Data Not Available.";
            this.ucSummary.Legend.SpanPercentage = 15;
            this.ucSummary.Legend.Visible = true;
            this.ucSummary.Location = new System.Drawing.Point(3, 3);
            this.ucSummary.Name = "ucSummary";
            this.ucSummary.Size = new System.Drawing.Size(665, 195);
            this.ucSummary.TabIndex = 1;
            this.ucSummary.TitleBottom.Visible = false;
            this.ucSummary.TitleLeft.Text = "Item ";
            this.ucSummary.Tooltips.HighlightFillColor = System.Drawing.Color.DimGray;
            this.ucSummary.Tooltips.HighlightOutlineColor = System.Drawing.Color.DarkGray;
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.ColumnCount = 1;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel4.Controls.Add(this.flowLayoutPanel3, 0, 1);
            this.tableLayoutPanel4.Controls.Add(this.groupBox1, 0, 0);
            this.tableLayoutPanel4.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 2;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 51.08696F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 48.91304F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(535, 89);
            this.tableLayoutPanel4.TabIndex = 1;
            // 
            // flowLayoutPanel3
            // 
            this.flowLayoutPanel3.Controls.Add(this.label4);
            this.flowLayoutPanel3.Controls.Add(this.opSummBy);
            this.flowLayoutPanel3.Location = new System.Drawing.Point(3, 48);
            this.flowLayoutPanel3.Name = "flowLayoutPanel3";
            this.flowLayoutPanel3.Size = new System.Drawing.Size(529, 38);
            this.flowLayoutPanel3.TabIndex = 91;
            // 
            // label4
            // 
            this.label4.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.label4.ForeColor = System.Drawing.Color.Maroon;
            this.label4.Location = new System.Drawing.Point(3, 6);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(90, 15);
            this.label4.TabIndex = 83;
            this.label4.Text = "Summarize By:";
            // 
            // opSummBy
            // 
            this.opSummBy.Anchor = System.Windows.Forms.AnchorStyles.None;
            appearance199.ForeColor = System.Drawing.Color.Maroon;
            appearance199.TextVAlignAsString = "Middle";
            this.opSummBy.Appearance = appearance199;
            this.opSummBy.BorderStyle = Infragistics.Win.UIElementBorderStyle.None;
            valueListItem1.CheckState = System.Windows.Forms.CheckState.Checked;
            valueListItem1.DataValue = "Q";
            valueListItem1.DisplayText = "Quantity";
            valueListItem2.DataValue = "A";
            valueListItem2.DisplayText = "Amount";
            valueListItem3.DataValue = "P";
            valueListItem3.DisplayText = "Profit";
            this.opSummBy.Items.AddRange(new Infragistics.Win.ValueListItem[] {
            valueListItem1,
            valueListItem2,
            valueListItem3});
            this.opSummBy.Location = new System.Drawing.Point(99, 3);
            this.opSummBy.Name = "opSummBy";
            this.opSummBy.Size = new System.Drawing.Size(253, 21);
            this.opSummBy.TabIndex = 4;
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.Color.WhiteSmoke;
            this.groupBox1.Controls.Add(this.btnView);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.dtFrom);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.dtTo);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.ForeColor = System.Drawing.Color.DarkBlue;
            this.groupBox1.Location = new System.Drawing.Point(3, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(529, 39);
            this.groupBox1.TabIndex = 90;
            this.groupBox1.TabStop = false;
            // 
            // btnView
            // 
            this.btnView.BackColor = System.Drawing.Color.Gainsboro;
            this.btnView.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnView.ForeColor = System.Drawing.Color.Black;
            this.btnView.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnView.Location = new System.Drawing.Point(391, 11);
            this.btnView.Name = "btnView";
            this.btnView.Size = new System.Drawing.Size(90, 24);
            this.btnView.TabIndex = 84;
            this.btnView.TabStop = false;
            this.btnView.Text = "Show Data";
            this.btnView.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnView.UseVisualStyleBackColor = false;
            this.btnView.Click += new System.EventHandler(this.btnView_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.label7.ForeColor = System.Drawing.Color.Black;
            this.label7.Location = new System.Drawing.Point(26, 15);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(68, 15);
            this.label7.TabIndex = 81;
            this.label7.Text = "From Date:";
            // 
            // dtFrom
            // 
            appearance200.FontData.BoldAsString = "False";
            this.dtFrom.Appearance = appearance200;
            this.dtFrom.DateTime = new System.DateTime(2018, 7, 15, 0, 0, 0, 0);
            this.dtFrom.Location = new System.Drawing.Point(98, 12);
            this.dtFrom.Name = "dtFrom";
            this.dtFrom.Size = new System.Drawing.Size(117, 23);
            this.dtFrom.TabIndex = 1;
            this.dtFrom.Value = new System.DateTime(2018, 7, 15, 0, 0, 0, 0);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.label3.ForeColor = System.Drawing.Color.Black;
            this.label3.Location = new System.Drawing.Point(238, 16);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(24, 15);
            this.label3.TabIndex = 82;
            this.label3.Text = "To:";
            // 
            // dtTo
            // 
            appearance201.FontData.BoldAsString = "False";
            this.dtTo.Appearance = appearance201;
            this.dtTo.Location = new System.Drawing.Point(268, 12);
            this.dtTo.Name = "dtTo";
            this.dtTo.Size = new System.Drawing.Size(117, 23);
            this.dtTo.TabIndex = 2;
            // 
            // tableLayoutPanel5
            // 
            this.tableLayoutPanel5.ColumnCount = 1;
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel5.Controls.Add(this.tableLayoutPanel6, 0, 1);
            this.tableLayoutPanel5.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel5.Location = new System.Drawing.Point(686, 3);
            this.tableLayoutPanel5.Name = "tableLayoutPanel5";
            this.tableLayoutPanel5.RowCount = 2;
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 5.108055F));
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 94.89194F));
            this.tableLayoutPanel5.Size = new System.Drawing.Size(228, 509);
            this.tableLayoutPanel5.TabIndex = 1;
            // 
            // tableLayoutPanel6
            // 
            this.tableLayoutPanel6.ColumnCount = 2;
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel6.Controls.Add(this.lblQtyInHand, 1, 4);
            this.tableLayoutPanel6.Controls.Add(this.label12, 0, 4);
            this.tableLayoutPanel6.Controls.Add(this.lblAvgProfitPerItem, 1, 3);
            this.tableLayoutPanel6.Controls.Add(this.label10, 0, 3);
            this.tableLayoutPanel6.Controls.Add(this.lblAvgRecvPerMonth, 1, 2);
            this.tableLayoutPanel6.Controls.Add(this.label8, 0, 2);
            this.tableLayoutPanel6.Controls.Add(this.lblAvgReturnPerMonth, 1, 1);
            this.tableLayoutPanel6.Controls.Add(this.label5, 0, 1);
            this.tableLayoutPanel6.Controls.Add(this.lblAvgSalePerMonth, 1, 0);
            this.tableLayoutPanel6.Controls.Add(this.lblSale, 0, 0);
            this.tableLayoutPanel6.Location = new System.Drawing.Point(3, 29);
            this.tableLayoutPanel6.Name = "tableLayoutPanel6";
            this.tableLayoutPanel6.RowCount = 5;
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel6.Size = new System.Drawing.Size(222, 100);
            this.tableLayoutPanel6.TabIndex = 7;
            // 
            // lblQtyInHand
            // 
            this.lblQtyInHand.AutoSize = true;
            this.lblQtyInHand.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblQtyInHand.Location = new System.Drawing.Point(114, 80);
            this.lblQtyInHand.Name = "lblQtyInHand";
            this.lblQtyInHand.Size = new System.Drawing.Size(0, 13);
            this.lblQtyInHand.TabIndex = 9;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.Location = new System.Drawing.Point(3, 80);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(82, 13);
            this.label12.TabIndex = 8;
            this.label12.Text = "Qty In Hand :";
            // 
            // lblAvgProfitPerItem
            // 
            this.lblAvgProfitPerItem.AutoSize = true;
            this.lblAvgProfitPerItem.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAvgProfitPerItem.Location = new System.Drawing.Point(114, 60);
            this.lblAvgProfitPerItem.Name = "lblAvgProfitPerItem";
            this.lblAvgProfitPerItem.Size = new System.Drawing.Size(0, 13);
            this.lblAvgProfitPerItem.TabIndex = 7;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(3, 60);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(93, 13);
            this.label10.TabIndex = 6;
            this.label10.Text = "Avg.Profit/Item";
            // 
            // lblAvgRecvPerMonth
            // 
            this.lblAvgRecvPerMonth.AutoSize = true;
            this.lblAvgRecvPerMonth.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAvgRecvPerMonth.Location = new System.Drawing.Point(114, 40);
            this.lblAvgRecvPerMonth.Name = "lblAvgRecvPerMonth";
            this.lblAvgRecvPerMonth.Size = new System.Drawing.Size(0, 13);
            this.lblAvgRecvPerMonth.TabIndex = 5;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(3, 40);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(100, 13);
            this.label8.TabIndex = 4;
            this.label8.Text = "Avg.Recv/Month";
            // 
            // lblAvgReturnPerMonth
            // 
            this.lblAvgReturnPerMonth.AutoSize = true;
            this.lblAvgReturnPerMonth.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAvgReturnPerMonth.Location = new System.Drawing.Point(114, 20);
            this.lblAvgReturnPerMonth.Name = "lblAvgReturnPerMonth";
            this.lblAvgReturnPerMonth.Size = new System.Drawing.Size(0, 13);
            this.lblAvgReturnPerMonth.TabIndex = 3;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(3, 20);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(102, 20);
            this.label5.TabIndex = 2;
            this.label5.Text = "Avg.Return/Month";
            // 
            // lblAvgSalePerMonth
            // 
            this.lblAvgSalePerMonth.AutoSize = true;
            this.lblAvgSalePerMonth.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAvgSalePerMonth.Location = new System.Drawing.Point(114, 0);
            this.lblAvgSalePerMonth.Name = "lblAvgSalePerMonth";
            this.lblAvgSalePerMonth.Size = new System.Drawing.Size(0, 13);
            this.lblAvgSalePerMonth.TabIndex = 1;
            // 
            // lblSale
            // 
            this.lblSale.AutoSize = true;
            this.lblSale.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSale.Location = new System.Drawing.Point(3, 0);
            this.lblSale.Name = "lblSale";
            this.lblSale.Size = new System.Drawing.Size(105, 13);
            this.lblSale.TabIndex = 0;
            this.lblSale.Text = "Avg Sale/Month :";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(222, 26);
            this.label1.TabIndex = 8;
            this.label1.Text = "Dispensing History";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnItemNote
            // 
            appearance202.BackColor = System.Drawing.Color.White;
            appearance202.ForeColor = System.Drawing.Color.Black;
            this.btnItemNote.Appearance = appearance202;
            this.btnItemNote.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Flat;
            this.btnItemNote.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnItemNote.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold);
            this.btnItemNote.Location = new System.Drawing.Point(30, 0);
            this.btnItemNote.Name = "btnItemNote";
            this.btnItemNote.Size = new System.Drawing.Size(70, 40);
            this.btnItemNote.TabIndex = 2;
            this.btnItemNote.TabStop = false;
            this.btnItemNote.Text = "Item Note";
            this.btnItemNote.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnItemNote.Click += new System.EventHandler(this.btnItemNote_Click);
            // 
            // lblTransactionType
            // 
            appearance203.BackColor = System.Drawing.Color.DeepSkyBlue;
            appearance203.BackColor2 = System.Drawing.Color.Azure;
            appearance203.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance203.ForeColor = System.Drawing.Color.Navy;
            appearance203.ForeColorDisabled = System.Drawing.Color.Navy;
            appearance203.TextHAlignAsString = "Center";
            appearance203.TextVAlignAsString = "Middle";
            this.lblTransactionType.Appearance = appearance203;
            this.lblTransactionType.BackColorInternal = System.Drawing.Color.Transparent;
            this.lblTransactionType.BorderStyleOuter = Infragistics.Win.UIElementBorderStyle.Solid;
            this.lblTransactionType.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblTransactionType.Font = new System.Drawing.Font("Verdana", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTransactionType.Location = new System.Drawing.Point(0, 0);
            this.lblTransactionType.Name = "lblTransactionType";
            this.lblTransactionType.Size = new System.Drawing.Size(938, 35);
            this.lblTransactionType.TabIndex = 0;
            this.lblTransactionType.Text = "Item Information";
            // 
            // btnClose
            // 
            appearance204.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(62)))), ((int)(((byte)(76)))));
            appearance204.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(62)))), ((int)(((byte)(76)))));
            appearance204.ForeColor = System.Drawing.Color.Black;
            this.btnClose.Appearance = appearance204;
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnClose.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClose.Location = new System.Drawing.Point(50, 0);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(60, 40);
            this.btnClose.TabIndex = 1;
            this.btnClose.Tag = "";
            this.btnClose.Text = "&Cancel";
            this.btnClose.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnOk
            // 
            appearance205.BackColor = System.Drawing.Color.White;
            appearance205.ForeColor = System.Drawing.Color.Black;
            this.btnOk.Appearance = appearance205;
            this.btnOk.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Flat;
            this.btnOk.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnOk.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold);
            this.btnOk.Location = new System.Drawing.Point(50, 0);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(60, 40);
            this.btnOk.TabIndex = 0;
            this.btnOk.Text = "&Ok";
            this.btnOk.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnGrpPricing
            // 
            appearance206.BackColor = System.Drawing.Color.White;
            appearance206.ForeColor = System.Drawing.Color.Black;
            this.btnGrpPricing.Appearance = appearance206;
            this.btnGrpPricing.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Flat;
            this.btnGrpPricing.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnGrpPricing.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold);
            this.btnGrpPricing.Location = new System.Drawing.Point(30, 0);
            this.btnGrpPricing.Name = "btnGrpPricing";
            this.btnGrpPricing.Size = new System.Drawing.Size(80, 40);
            this.btnGrpPricing.TabIndex = 6;
            this.btnGrpPricing.TabStop = false;
            this.btnGrpPricing.Text = "&Group Pricing";
            this.btnGrpPricing.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnGrpPricing.Click += new System.EventHandler(this.btnGrpPricing_Click);
            // 
            // btnCompanionItem
            // 
            appearance207.BackColor = System.Drawing.Color.White;
            appearance207.ForeColor = System.Drawing.Color.Black;
            this.btnCompanionItem.Appearance = appearance207;
            this.btnCompanionItem.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Flat;
            this.btnCompanionItem.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnCompanionItem.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold);
            this.btnCompanionItem.Location = new System.Drawing.Point(30, 0);
            this.btnCompanionItem.Name = "btnCompanionItem";
            this.btnCompanionItem.Size = new System.Drawing.Size(90, 40);
            this.btnCompanionItem.TabIndex = 4;
            this.btnCompanionItem.TabStop = false;
            this.btnCompanionItem.Text = "Co&mpanion";
            this.btnCompanionItem.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnCompanionItem.Click += new System.EventHandler(this.btnCompanionItem_Click);
            // 
            // btnInventoryHistory
            // 
            appearance208.BackColor = System.Drawing.Color.White;
            appearance208.ForeColor = System.Drawing.Color.Black;
            this.btnInventoryHistory.Appearance = appearance208;
            this.btnInventoryHistory.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Flat;
            this.btnInventoryHistory.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold);
            this.btnInventoryHistory.Location = new System.Drawing.Point(116, 13);
            this.btnInventoryHistory.Name = "btnInventoryHistory";
            this.btnInventoryHistory.Size = new System.Drawing.Size(100, 40);
            this.btnInventoryHistory.TabIndex = 3;
            this.btnInventoryHistory.TabStop = false;
            this.btnInventoryHistory.Text = "Inventory History";
            this.btnInventoryHistory.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnInventoryHistory.Visible = false;
            this.btnInventoryHistory.Click += new System.EventHandler(this.btnInventoryHistory_Click);
            // 
            // btnPriceValidation
            // 
            appearance209.BackColor = System.Drawing.Color.White;
            appearance209.ForeColor = System.Drawing.Color.Black;
            this.btnPriceValidation.Appearance = appearance209;
            this.btnPriceValidation.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Flat;
            this.btnPriceValidation.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold);
            this.btnPriceValidation.Location = new System.Drawing.Point(586, 13);
            this.btnPriceValidation.Name = "btnPriceValidation";
            this.btnPriceValidation.Size = new System.Drawing.Size(100, 40);
            this.btnPriceValidation.TabIndex = 7;
            this.btnPriceValidation.TabStop = false;
            this.btnPriceValidation.Text = "Price Validation";
            this.btnPriceValidation.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnPriceValidation.Visible = false;
            this.btnPriceValidation.Click += new System.EventHandler(this.btnPriceValidation_Click);
            // 
            // btnInventoryReceived
            // 
            appearance210.BackColor = System.Drawing.Color.White;
            appearance210.ForeColor = System.Drawing.Color.Black;
            this.btnInventoryReceived.Appearance = appearance210;
            this.btnInventoryReceived.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Flat;
            this.btnInventoryReceived.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnInventoryReceived.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold);
            this.btnInventoryReceived.Location = new System.Drawing.Point(30, 0);
            this.btnInventoryReceived.Name = "btnInventoryReceived";
            this.btnInventoryReceived.Size = new System.Drawing.Size(70, 40);
            this.btnInventoryReceived.TabIndex = 5;
            this.btnInventoryReceived.TabStop = false;
            this.btnInventoryReceived.Text = "Inv. &Recvd";
            this.btnInventoryReceived.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnInventoryReceived.Click += new System.EventHandler(this.btnInventoryReceived_Click);
            // 
            // tabItemInformation
            // 
            this.tabItemInformation.Controls.Add(this.ultraTabPageControl1);
            this.tabItemInformation.Controls.Add(this.ultraTabSharedControlsPage1);
            this.tabItemInformation.Controls.Add(this.ultraTabPageControl2);
            this.tabItemInformation.Controls.Add(this.ultraTabPageControl3);
            this.tabItemInformation.Controls.Add(this.ultraTabPageControl4);
            this.tabItemInformation.Controls.Add(this.ultraTabPageControl6);
            this.tabItemInformation.Location = new System.Drawing.Point(6, 41);
            this.tabItemInformation.Name = "tabItemInformation";
            appearance211.BackColor = System.Drawing.Color.Blue;
            appearance211.ForeColor = System.Drawing.Color.White;
            this.tabItemInformation.SelectedTabAppearance = appearance211;
            this.tabItemInformation.SharedControlsPage = this.ultraTabSharedControlsPage1;
            this.tabItemInformation.Size = new System.Drawing.Size(928, 546);
            this.tabItemInformation.Style = Infragistics.Win.UltraWinTabControl.UltraTabControlStyle.PropertyPage2003;
            this.tabItemInformation.TabIndex = 1;
            this.tabItemInformation.TabLayoutStyle = Infragistics.Win.UltraWinTabs.TabLayoutStyle.SingleRowSizeToFit;
            appearance212.FontData.BoldAsString = "True";
            ultraTab3.Appearance = appearance212;
            ultraTab3.Key = "ItemInformation";
            ultraTab3.TabPage = this.ultraTabPageControl1;
            ultraTab3.Text = "&Item Information";
            appearance213.FontData.BoldAsString = "True";
            ultraTab4.Appearance = appearance213;
            ultraTab4.Key = "VendorInformation";
            ultraTab4.TabPage = this.ultraTabPageControl2;
            ultraTab4.Text = "&Vendor Information";
            appearance214.FontData.BoldAsString = "True";
            ultraTab1.Appearance = appearance214;
            ultraTab1.Key = "PriceChangeLog";
            ultraTab1.TabPage = this.ultraTabPageControl3;
            ultraTab1.Text = "P&rice Change Log";
            appearance215.FontData.BoldAsString = "True";
            ultraTab2.Appearance = appearance215;
            ultraTab2.Key = "PhysicalInventoryView";
            ultraTab2.TabPage = this.ultraTabPageControl4;
            ultraTab2.Text = "&Physical. Inv. View";
            appearance216.FontData.BoldAsString = "True";
            ultraTab5.Appearance = appearance216;
            ultraTab5.Key = "ITP";
            ultraTab5.TabPage = this.ultraTabPageControl6;
            ultraTab5.Text = "Item Pe&rformance";
            this.tabItemInformation.Tabs.AddRange(new Infragistics.Win.UltraWinTabControl.UltraTab[] {
            ultraTab3,
            ultraTab4,
            ultraTab1,
            ultraTab2,
            ultraTab5});
            this.tabItemInformation.ViewStyle = Infragistics.Win.UltraWinTabControl.ViewStyle.Office2003;
            this.tabItemInformation.SelectedTabChanged += new Infragistics.Win.UltraWinTabControl.SelectedTabChangedEventHandler(this.tabItemInformation_SelectedTabChanged);
            // 
            // ultraTabSharedControlsPage1
            // 
            this.ultraTabSharedControlsPage1.Location = new System.Drawing.Point(-10000, -10000);
            this.ultraTabSharedControlsPage1.Name = "ultraTabSharedControlsPage1";
            this.ultraTabSharedControlsPage1.Size = new System.Drawing.Size(924, 521);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.btnInventoryHistory);
            this.groupBox4.Controls.Add(this.pnlItemNote);
            this.groupBox4.Controls.Add(this.pnlCompanionItem);
            this.groupBox4.Controls.Add(this.pnlInventoryReceived);
            this.groupBox4.Controls.Add(this.pnlGrpPricing);
            this.groupBox4.Controls.Add(this.pnlOk);
            this.groupBox4.Controls.Add(this.pnlClose);
            this.groupBox4.Controls.Add(this.btnPriceValidation);
            this.groupBox4.Controls.Add(this.btnCopyContents);
            this.groupBox4.Controls.Add(this.btnMMSSearch);
            this.groupBox4.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.groupBox4.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox4.Location = new System.Drawing.Point(3, 593);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(931, 58);
            this.groupBox4.TabIndex = 2;
            this.groupBox4.TabStop = false;
            // 
            // pnlItemNote
            // 
            // 
            // pnlItemNote.ClientArea
            // 
            this.pnlItemNote.ClientArea.Controls.Add(this.btnItemNote);
            this.pnlItemNote.ClientArea.Controls.Add(this.lblItemNote);
            this.pnlItemNote.Location = new System.Drawing.Point(6, 13);
            this.pnlItemNote.Name = "pnlItemNote";
            this.pnlItemNote.Size = new System.Drawing.Size(100, 40);
            this.pnlItemNote.TabIndex = 16;
            this.pnlItemNote.Visible = false;
            // 
            // lblItemNote
            // 
            appearance217.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(138)))), ((int)(((byte)(31)))));
            appearance217.FontData.BoldAsString = "True";
            appearance217.ForeColor = System.Drawing.Color.White;
            appearance217.TextHAlignAsString = "Center";
            appearance217.TextVAlignAsString = "Middle";
            this.lblItemNote.Appearance = appearance217;
            this.lblItemNote.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblItemNote.Font = new System.Drawing.Font("Verdana", 8F, System.Drawing.FontStyle.Bold);
            this.lblItemNote.Location = new System.Drawing.Point(0, 0);
            this.lblItemNote.Margin = new System.Windows.Forms.Padding(3, 3, 0, 3);
            this.lblItemNote.Name = "lblItemNote";
            this.lblItemNote.Size = new System.Drawing.Size(30, 40);
            this.lblItemNote.TabIndex = 0;
            this.lblItemNote.Tag = "NOCOLOR";
            this.lblItemNote.Text = "F3";
            this.lblItemNote.Click += new System.EventHandler(this.btnItemNote_Click);
            // 
            // pnlCompanionItem
            // 
            // 
            // pnlCompanionItem.ClientArea
            // 
            this.pnlCompanionItem.ClientArea.Controls.Add(this.btnCompanionItem);
            this.pnlCompanionItem.ClientArea.Controls.Add(this.lblCompanionItem);
            this.pnlCompanionItem.Location = new System.Drawing.Point(226, 13);
            this.pnlCompanionItem.Name = "pnlCompanionItem";
            this.pnlCompanionItem.Size = new System.Drawing.Size(120, 40);
            this.pnlCompanionItem.TabIndex = 15;
            this.pnlCompanionItem.Visible = false;
            // 
            // lblCompanionItem
            // 
            appearance218.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(138)))), ((int)(((byte)(31)))));
            appearance218.FontData.BoldAsString = "True";
            appearance218.ForeColor = System.Drawing.Color.White;
            appearance218.TextHAlignAsString = "Center";
            appearance218.TextVAlignAsString = "Middle";
            this.lblCompanionItem.Appearance = appearance218;
            this.lblCompanionItem.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblCompanionItem.Font = new System.Drawing.Font("Verdana", 8F, System.Drawing.FontStyle.Bold);
            this.lblCompanionItem.Location = new System.Drawing.Point(0, 0);
            this.lblCompanionItem.Margin = new System.Windows.Forms.Padding(3, 3, 0, 3);
            this.lblCompanionItem.Name = "lblCompanionItem";
            this.lblCompanionItem.Size = new System.Drawing.Size(30, 40);
            this.lblCompanionItem.TabIndex = 0;
            this.lblCompanionItem.Tag = "NOCOLOR";
            this.lblCompanionItem.Text = "F5";
            this.lblCompanionItem.Click += new System.EventHandler(this.btnCompanionItem_Click);
            // 
            // pnlInventoryReceived
            // 
            // 
            // pnlInventoryReceived.ClientArea
            // 
            this.pnlInventoryReceived.ClientArea.Controls.Add(this.btnInventoryReceived);
            this.pnlInventoryReceived.ClientArea.Controls.Add(this.lblInventoryReceived);
            this.pnlInventoryReceived.Location = new System.Drawing.Point(356, 13);
            this.pnlInventoryReceived.Name = "pnlInventoryReceived";
            this.pnlInventoryReceived.Size = new System.Drawing.Size(100, 40);
            this.pnlInventoryReceived.TabIndex = 14;
            this.pnlInventoryReceived.Visible = false;
            // 
            // lblInventoryReceived
            // 
            appearance219.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(138)))), ((int)(((byte)(31)))));
            appearance219.FontData.BoldAsString = "True";
            appearance219.ForeColor = System.Drawing.Color.White;
            appearance219.TextHAlignAsString = "Center";
            appearance219.TextVAlignAsString = "Middle";
            this.lblInventoryReceived.Appearance = appearance219;
            this.lblInventoryReceived.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblInventoryReceived.Font = new System.Drawing.Font("Verdana", 8F, System.Drawing.FontStyle.Bold);
            this.lblInventoryReceived.Location = new System.Drawing.Point(0, 0);
            this.lblInventoryReceived.Margin = new System.Windows.Forms.Padding(3, 3, 0, 3);
            this.lblInventoryReceived.Name = "lblInventoryReceived";
            this.lblInventoryReceived.Size = new System.Drawing.Size(30, 40);
            this.lblInventoryReceived.TabIndex = 0;
            this.lblInventoryReceived.Tag = "NOCOLOR";
            this.lblInventoryReceived.Text = "F2";
            this.lblInventoryReceived.Click += new System.EventHandler(this.btnInventoryReceived_Click);
            // 
            // pnlGrpPricing
            // 
            // 
            // pnlGrpPricing.ClientArea
            // 
            this.pnlGrpPricing.ClientArea.Controls.Add(this.btnGrpPricing);
            this.pnlGrpPricing.ClientArea.Controls.Add(this.lblGrpPricing);
            this.pnlGrpPricing.Location = new System.Drawing.Point(466, 13);
            this.pnlGrpPricing.Name = "pnlGrpPricing";
            this.pnlGrpPricing.Size = new System.Drawing.Size(110, 40);
            this.pnlGrpPricing.TabIndex = 13;
            this.pnlGrpPricing.Visible = false;
            // 
            // lblGrpPricing
            // 
            appearance220.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(138)))), ((int)(((byte)(31)))));
            appearance220.FontData.BoldAsString = "True";
            appearance220.ForeColor = System.Drawing.Color.White;
            appearance220.TextHAlignAsString = "Center";
            appearance220.TextVAlignAsString = "Middle";
            this.lblGrpPricing.Appearance = appearance220;
            this.lblGrpPricing.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblGrpPricing.Font = new System.Drawing.Font("Verdana", 8F, System.Drawing.FontStyle.Bold);
            this.lblGrpPricing.Location = new System.Drawing.Point(0, 0);
            this.lblGrpPricing.Margin = new System.Windows.Forms.Padding(3, 3, 0, 3);
            this.lblGrpPricing.Name = "lblGrpPricing";
            this.lblGrpPricing.Size = new System.Drawing.Size(30, 40);
            this.lblGrpPricing.TabIndex = 0;
            this.lblGrpPricing.Tag = "NOCOLOR";
            this.lblGrpPricing.Text = "F6";
            this.lblGrpPricing.Click += new System.EventHandler(this.btnGrpPricing_Click);
            // 
            // pnlOk
            // 
            // 
            // pnlOk.ClientArea
            // 
            this.pnlOk.ClientArea.Controls.Add(this.btnOk);
            this.pnlOk.ClientArea.Controls.Add(this.lblOk);
            this.pnlOk.Location = new System.Drawing.Point(696, 13);
            this.pnlOk.Name = "pnlOk";
            this.pnlOk.Size = new System.Drawing.Size(110, 40);
            this.pnlOk.TabIndex = 12;
            // 
            // lblOk
            // 
            appearance221.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(138)))), ((int)(((byte)(31)))));
            appearance221.FontData.BoldAsString = "True";
            appearance221.ForeColor = System.Drawing.Color.White;
            appearance221.TextHAlignAsString = "Center";
            appearance221.TextVAlignAsString = "Middle";
            this.lblOk.Appearance = appearance221;
            this.lblOk.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblOk.Font = new System.Drawing.Font("Verdana", 8F, System.Drawing.FontStyle.Bold);
            this.lblOk.Location = new System.Drawing.Point(0, 0);
            this.lblOk.Margin = new System.Windows.Forms.Padding(3, 3, 0, 3);
            this.lblOk.Name = "lblOk";
            this.lblOk.Size = new System.Drawing.Size(50, 40);
            this.lblOk.TabIndex = 0;
            this.lblOk.Tag = "NOCOLOR";
            this.lblOk.Text = "Alt + O";
            this.lblOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // pnlClose
            // 
            // 
            // pnlClose.ClientArea
            // 
            this.pnlClose.ClientArea.Controls.Add(this.btnClose);
            this.pnlClose.ClientArea.Controls.Add(this.lblClose);
            this.pnlClose.Location = new System.Drawing.Point(816, 13);
            this.pnlClose.Name = "pnlClose";
            this.pnlClose.Size = new System.Drawing.Size(110, 40);
            this.pnlClose.TabIndex = 11;
            // 
            // lblClose
            // 
            appearance222.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(138)))), ((int)(((byte)(31)))));
            appearance222.FontData.BoldAsString = "True";
            appearance222.ForeColor = System.Drawing.Color.White;
            appearance222.TextHAlignAsString = "Center";
            appearance222.TextVAlignAsString = "Middle";
            this.lblClose.Appearance = appearance222;
            this.lblClose.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblClose.Font = new System.Drawing.Font("Verdana", 8F, System.Drawing.FontStyle.Bold);
            this.lblClose.Location = new System.Drawing.Point(0, 0);
            this.lblClose.Margin = new System.Windows.Forms.Padding(3, 3, 0, 3);
            this.lblClose.Name = "lblClose";
            this.lblClose.Size = new System.Drawing.Size(50, 40);
            this.lblClose.TabIndex = 0;
            this.lblClose.Tag = "NOCOLOR";
            this.lblClose.Text = "Alt + C";
            this.lblClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnCopyContents
            // 
            appearance223.BackColor = System.Drawing.Color.White;
            appearance223.ForeColor = System.Drawing.Color.Black;
            this.btnCopyContents.Appearance = appearance223;
            this.btnCopyContents.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Flat;
            this.btnCopyContents.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold);
            this.btnCopyContents.Location = new System.Drawing.Point(5, 13);
            this.btnCopyContents.Name = "btnCopyContents";
            this.btnCopyContents.Size = new System.Drawing.Size(150, 40);
            this.btnCopyContents.TabIndex = 8;
            this.btnCopyContents.TabStop = false;
            this.btnCopyContents.Text = "Copy Content From Another Item";
            this.btnCopyContents.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnCopyContents.Click += new System.EventHandler(this.btnCopyContents_Click);
            // 
            // btnMMSSearch
            // 
            appearance224.BackColor = System.Drawing.Color.White;
            appearance224.ForeColor = System.Drawing.Color.Black;
            this.btnMMSSearch.Appearance = appearance224;
            this.btnMMSSearch.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Flat;
            this.btnMMSSearch.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold);
            this.btnMMSSearch.Location = new System.Drawing.Point(165, 13);
            this.btnMMSSearch.Name = "btnMMSSearch";
            this.btnMMSSearch.Size = new System.Drawing.Size(110, 40);
            this.btnMMSSearch.TabIndex = 17;
            this.btnMMSSearch.TabStop = false;
            this.btnMMSSearch.Text = "MMS Search";
            this.btnMMSSearch.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnMMSSearch.Click += new System.EventHandler(this.btnMMSSearch_Click);
            // 
            // ultraTabPageControl5
            // 
            this.ultraTabPageControl5.Location = new System.Drawing.Point(-10000, -10000);
            this.ultraTabPageControl5.Name = "ultraTabPageControl5";
            this.ultraTabPageControl5.Size = new System.Drawing.Size(784, 542);
            // 
            // frmItems
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(7, 17);
            this.BackColor = System.Drawing.SystemColors.Control;
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(938, 670);
            this.Controls.Add(this.tabItemInformation);
            this.Controls.Add(this.lblTransactionType);
            this.Controls.Add(this.groupBox4);
            this.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.Name = "frmItems";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Item Information";
            this.toolTip1.SetToolTip(this, "Press F4 To Search");
            this.Activated += new System.EventHandler(this.frmItems_Activated);
            this.Load += new System.EventHandler(this.frmItems_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmItems_KeyDown);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.frmItems_KeyUp);
            this.ultraTabPageControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.chkIsOnSale)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chkNonRefundable)).EndInit();
            this.pnlItemMonitorCategory.ClientArea.ResumeLayout(false);
            this.pnlItemMonitorCategory.ResumeLayout(false);
            this.pnlAddSecDesc.ClientArea.ResumeLayout(false);
            this.pnlAddSecDesc.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.chkIsActive)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDescription)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtManufacturerName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkIsEBT)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtItemCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtUnit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSubDepartment)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkIsOTCItem)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkExclFromRecpt)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkExclFromAutoPO)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDepartmentCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboItemType)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSaleTypeCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSeasonCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtProductCode)).EndInit();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbCLPointPolicy)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numCLPointsPerDollar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkIsDefaultCLPoints)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboTaxCodes)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbDiscountPolicy)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbTaxPolicy)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkUpdatePriceItemWise)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numFreight)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkIsTaxable)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numLastCostPrice)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numAveragePrice)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numSellingPrice)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numDiscountAmt)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkIsDiscountable)).EndInit();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dtpExpiryDate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numMinQty)).EndInit();
            this.grpItemPacketInfo.ResumeLayout(false);
            this.grpItemPacketInfo.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numCaseCost)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPacketSize)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPacketQuantity)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPacketUnit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkPacketSize)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numQtyInStock)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numQtyOnOrder)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numReorderLevel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtpExpectedDelDate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtLocation)).EndInit();
            this.faOnSale.ResumeLayout(false);
            this.faOnSale.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numSalePrice)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtpSaleEndDate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtpSaleStartDate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numSaleLimitQty)).EndInit();
            this.ultraTabPageControl2.ResumeLayout(false);
            this.grbVendorInformation.ResumeLayout(false);
            this.pnlDeleteItemVendorInfo.ClientArea.ResumeLayout(false);
            this.pnlDeleteItemVendorInfo.ResumeLayout(false);
            this.pnlSaveItemVendor.ClientArea.ResumeLayout(false);
            this.pnlSaveItemVendor.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdDetail)).EndInit();
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.combEditorPrefVendor)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtLastVendor)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtRemarks)).EndInit();
            this.groupBox7.ResumeLayout(false);
            this.groupBox7.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dtpLastSaleDate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtpLastRecvDate)).EndInit();
            this.ultraTabPageControl3.ResumeLayout(false);
            this.groupBox8.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdPriceChangeLog)).EndInit();
            this.ultraTabPageControl4.ResumeLayout(false);
            this.groupBox9.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdPhysicalInvView)).EndInit();
            this.ultraTabPageControl6.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdVendor)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ucSummary)).EndInit();
            this.tableLayoutPanel4.ResumeLayout(false);
            this.flowLayoutPanel3.ResumeLayout(false);
            this.flowLayoutPanel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.opSummBy)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dtFrom)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtTo)).EndInit();
            this.tableLayoutPanel5.ResumeLayout(false);
            this.tableLayoutPanel5.PerformLayout();
            this.tableLayoutPanel6.ResumeLayout(false);
            this.tableLayoutPanel6.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tabItemInformation)).EndInit();
            this.tabItemInformation.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.pnlItemNote.ClientArea.ResumeLayout(false);
            this.pnlItemNote.ResumeLayout(false);
            this.pnlCompanionItem.ClientArea.ResumeLayout(false);
            this.pnlCompanionItem.ResumeLayout(false);
            this.pnlInventoryReceived.ClientArea.ResumeLayout(false);
            this.pnlInventoryReceived.ResumeLayout(false);
            this.pnlGrpPricing.ClientArea.ResumeLayout(false);
            this.pnlGrpPricing.ResumeLayout(false);
            this.pnlOk.ClientArea.ResumeLayout(false);
            this.pnlOk.ResumeLayout(false);
            this.pnlClose.ClientArea.ResumeLayout(false);
            this.pnlClose.ResumeLayout(false);
            this.ResumeLayout(false);

        }
        #endregion

        private void frmItems_Load(object sender, System.EventArgs e)
        {
            #region Events attachment
            this.txtDepartmentCode.AfterEnterEditMode += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
            this.txtDepartmentCode.AfterExitEditMode += new System.EventHandler(clsUIHelper.AfterExitEditMode);

            this.txtSubDepartment.AfterEnterEditMode += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
            this.txtSubDepartment.AfterExitEditMode += new System.EventHandler(clsUIHelper.AfterExitEditMode);

            this.txtDescription.AfterEnterEditMode += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
            this.txtDescription.AfterExitEditMode += new System.EventHandler(clsUIHelper.AfterExitEditMode);

            this.txtItemCode.AfterEnterEditMode += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
            this.txtItemCode.AfterExitEditMode += new System.EventHandler(clsUIHelper.AfterExitEditMode);

            this.cboItemType.AfterEnterEditMode += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
            this.cboItemType.AfterExitEditMode += new System.EventHandler(clsUIHelper.AfterExitEditMode);

            this.txtLastVendor.AfterEnterEditMode += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
            this.txtLastVendor.AfterExitEditMode += new System.EventHandler(clsUIHelper.AfterExitEditMode);

            this.txtLocation.AfterEnterEditMode += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
            this.txtLocation.AfterExitEditMode += new System.EventHandler(clsUIHelper.AfterExitEditMode);
            this.txtProductCode.AfterEnterEditMode += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
            this.txtProductCode.AfterExitEditMode += new System.EventHandler(clsUIHelper.AfterExitEditMode);

            this.txtRemarks.AfterEnterEditMode += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
            this.txtRemarks.AfterExitEditMode += new System.EventHandler(clsUIHelper.AfterExitEditMode);

            this.txtSaleTypeCode.AfterEnterEditMode += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
            this.txtSaleTypeCode.AfterExitEditMode += new System.EventHandler(clsUIHelper.AfterExitEditMode);

            this.txtSeasonCode.AfterEnterEditMode += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
            this.txtSeasonCode.AfterExitEditMode += new System.EventHandler(clsUIHelper.AfterExitEditMode);

            //this.txtTaxCode.AfterEnterEditMode += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
            //this.txtTaxCode.AfterExitEditMode += new System.EventHandler(clsUIHelper.AfterExitEditMode);

            this.txtUnit.AfterEnterEditMode += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
            this.txtUnit.AfterExitEditMode += new System.EventHandler(clsUIHelper.AfterExitEditMode);

            //this.chkIsDiscountable.Enter += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
            //this.chkIsDiscountable.Leave += new System.EventHandler(clsUIHelper.AfterExitEditMode);

            //this.chkIsTaxable.Enter += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
            //this.chkIsTaxable.Leave += new System.EventHandler(clsUIHelper.AfterExitEditMode);

            this.dtpExpectedDelDate.Enter += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
            this.dtpExpectedDelDate.Leave += new System.EventHandler(clsUIHelper.AfterExitEditMode);

            this.dtpLastRecvDate.Enter += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
            this.dtpLastRecvDate.Leave += new System.EventHandler(clsUIHelper.AfterExitEditMode);

            this.dtpLastSaleDate.Enter += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
            this.dtpLastSaleDate.Leave += new System.EventHandler(clsUIHelper.AfterExitEditMode);

            this.dtpSaleEndDate.Enter += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
            this.dtpSaleEndDate.Leave += new System.EventHandler(clsUIHelper.AfterExitEditMode);

            this.dtpSaleStartDate.Enter += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
            this.dtpSaleStartDate.Leave += new System.EventHandler(clsUIHelper.AfterExitEditMode);

            this.numAveragePrice.AfterEnterEditMode += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
            this.numAveragePrice.AfterExitEditMode += new System.EventHandler(clsUIHelper.AfterExitEditMode);

            this.numDiscountAmt.AfterEnterEditMode += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
            this.numDiscountAmt.AfterExitEditMode += new System.EventHandler(clsUIHelper.AfterExitEditMode);

            this.numFreight.AfterEnterEditMode += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
            this.numFreight.AfterExitEditMode += new System.EventHandler(clsUIHelper.AfterExitEditMode);

            this.numLastCostPrice.AfterEnterEditMode += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
            this.numLastCostPrice.AfterExitEditMode += new System.EventHandler(clsUIHelper.AfterExitEditMode);

            this.numMinQty.AfterEnterEditMode += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
            this.numMinQty.AfterExitEditMode += new System.EventHandler(clsUIHelper.AfterExitEditMode);

            this.numQtyInStock.AfterEnterEditMode += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
            this.numQtyInStock.AfterExitEditMode += new System.EventHandler(clsUIHelper.AfterExitEditMode);

            this.numQtyOnOrder.AfterEnterEditMode += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
            this.numQtyOnOrder.AfterExitEditMode += new System.EventHandler(clsUIHelper.AfterExitEditMode);

            this.numReorderLevel.AfterEnterEditMode += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
            this.numReorderLevel.AfterExitEditMode += new System.EventHandler(clsUIHelper.AfterExitEditMode);

            this.numSalePrice.AfterEnterEditMode += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
            this.numSalePrice.AfterExitEditMode += new System.EventHandler(clsUIHelper.AfterExitEditMode);

            this.numSellingPrice.AfterEnterEditMode += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
            this.numSellingPrice.AfterExitEditMode += new System.EventHandler(clsUIHelper.AfterExitEditMode);
            //Added By Shitaljit(QuicSolv) on 18 August
            this.cmbTaxPolicy.AfterEnterEditMode += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
            this.cmbTaxPolicy.AfterExitEditMode += new System.EventHandler(clsUIHelper.AfterExitEditMode);
            //END

            this.txtManufacturerName.AfterEnterEditMode += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
            this.txtManufacturerName.AfterExitEditMode += new System.EventHandler(clsUIHelper.AfterExitEditMode);

            this.dtpExpiryDate.Enter += new System.EventHandler(clsUIHelper.AfterEnterEditMode);    //Sprint-21 - 2206 09-Mar-2016 JY Added
            this.dtpExpiryDate.Leave += new System.EventHandler(clsUIHelper.AfterExitEditMode);     //Sprint-21 - 2206 09-Mar-2016 JY Added
            #endregion

            pnlInventoryReceived.Enabled = UserPriviliges.IsUserHasPriviliges(UserPriviliges.Modules.InventoryMgmt.ID, UserPriviliges.Screens.InventoryRecvd.ID, UserPriviliges.Permissions.InventoryReceived.ID);  //PRIMEPOS-3141 27-Oct-2022 JY Added

            #region Sprint-18 - 2041 28-Oct-2014 JY Added
            this.cmbCLPointPolicy.AfterEnterEditMode += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
            this.cmbCLPointPolicy.AfterExitEditMode += new System.EventHandler(clsUIHelper.AfterExitEditMode);
            #endregion

            this.pnlCompanionItem.Enabled = AllowEdit;
            this.pnlGrpPricing.Enabled = AllowEdit;
            this.pnlOk.Enabled = AllowEdit;
            chkPacketSize.BringToFront();
            chkPacketSize.Enabled = true;

            if (POS_Core.Resources.UserPriviliges.IsUserHasPriviliges(POS_Core.Resources.UserPriviliges.Modules.InventoryMgmt.ID, POS_Core.Resources.UserPriviliges.Screens.MinimumItemPrice.ID))
            {
                this.btnPriceValidation.Enabled = true;
            }
            else
            {
                this.btnPriceValidation.Enabled = false;
            }
            this.chkIsDiscountable.Checked = oItemRow.isDiscountable;// Addded Shitaljit(QuicSolv) on 10 Nov 2011 make by default item isDiscountable true
            IsCanceled = true;
            clsUIHelper.setColorSchecme(this);
            btnClose.Appearance.BackColor = Color.FromArgb(255, System.Convert.ToByte(POS_Core.Resources.Configuration.arrCloseBk[0].ToString()), System.Convert.ToByte(POS_Core.Resources.Configuration.arrCloseBk[1]), System.Convert.ToByte(POS_Core.Resources.Configuration.arrCloseBk[2]));
            btnClose.Appearance.BackColor2 = Color.FromArgb(255, System.Convert.ToByte(POS_Core.Resources.Configuration.arrCloseBk[0].ToString()), System.Convert.ToByte(POS_Core.Resources.Configuration.arrCloseBk[1]), System.Convert.ToByte(POS_Core.Resources.Configuration.arrCloseBk[2]));

            if (isItemEditMode == true)
            {
                this.grbVendorInformation.Visible = true;
                mItemId = oItemRow.ItemID;
                getItemVendorDetail(mItemId);
                setGridDetailFormat();
                this.tabItemInformation.Tabs["PriceChangeLog"].Visible = true;
                this.tabItemInformation.Tabs["PhysicalInventoryView"].Visible = true;
                this.tabItemInformation.Tabs["VendorInformation"].Visible = AllowEdit;
                this.pnlAddSecDesc.Visible = true;
                btnRefreshDataFromMMS.Visible = true;
                SetVisibilityAndAccessOfCostPrice();    //PRIMEPOS-2464 04-Mar-2020 JY Added
            }
            else
            {
                txtItemCode.Select();
                //Added By shitaljit on 9 April 2012
                //To allow user to add VendorItem information while adding item.
                this.grbVendorInformation.Visible = true;
                this.grbVendorInformation.Enabled = false;
                mItemId = "##-99-##";
                getItemVendorDetail(mItemId);
                setGridDetailFormat();
                numQtyInStock.Enabled = true;
                this.tabItemInformation.Tabs["PriceChangeLog"].Visible = false;
                this.tabItemInformation.Tabs["PhysicalInventoryView"].Visible = false;
                this.tabItemInformation.Tabs["VendorInformation"].Visible = true;//modifeid by shitaljit to make vendor information tab visible on item add
                this.pnlSaveItemVendor.Visible = false;
                btnRefreshDataFromMMS.Visible = false;
            }

            //Added By shitaljit on 17 Aug 2012 for Text Prediction while ading new Items.
            #region Fetching Item Descriptions for showing Inteligence to users
            if (Configuration.CInfo.ShowTextPrediction == true && isItemEditMode == false)
            {
                this.txtItemDesc.Visible = true;
                this.txtItemDesc.BorderStyle = BorderStyle.FixedSingle;
                this.txtDescription.Visible = false;
                Search oSearch = new Search();
                AutoCompleteStringCollection ItemDescCollection = new AutoCompleteStringCollection();

                ItemDescCollection = oSearch.GetAutoCompleteCollectionData(clsPOSDBConstants.Item_tbl, clsPOSDBConstants.Item_Fld_Description);

                this.txtItemDesc.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
                this.txtItemDesc.AutoCompleteSource = AutoCompleteSource.CustomSource;
                this.txtItemDesc.AutoCompleteCustomSource = ItemDescCollection;
            }

            if (!isItemEditMode)
                this.numQtyInStock.Enabled = UserPriviliges.IsUserHasPriviliges(UserPriviliges.Modules.InventoryMgmt.ID, UserPriviliges.Screens.PhysicalInventory.ID);  //PRIMEPOS-3121 12-Sep-2022 JY Added

            if (frmInventoryRecieved.isInventoryRecieved)
            {
                this.numQtyInStock.Enabled = false;
            }
            #endregion

            if (string.IsNullOrEmpty(ControlToFocusOnLoad) == false)
            {
                this.groupBox3.Focus();
                switch (ControlToFocusOnLoad)
                {
                    case "numLastCostPrice":
                        this.numLastCostPrice.Select();
                        break;
                    case "numSellingPrice":
                        this.numSellingPrice.Select();
                        break;
                }
            }

            #region Item Performance PRIMEPOS-2654
            dtFrom.DateTime = DateTime.Now.AddYears(-1);
            dtTo.DateTime = DateTime.Now;

            if (opSummBy.Value == "" || opSummBy.Value == null)
            {
                opSummBy.Value = "Q";
                // PopulateItemPerformance(oItemRow.ItemID);
            }
            #endregion
        }

        private void SetTaxCodeChecked(ItemTaxData itemTaxInfo)
        {
            foreach (ValueListItem item in cboTaxCodes.Items)
            {
                int value = Convert.ToInt32((item.DataValue));
                foreach (DataRow row in itemTaxInfo.ItemTaxTable.Rows)
                {
                    int taxCode = int.Parse(row[clsPOSDBConstants.ItemTaxTable_TaxIdColumnName].ToString());

                    if (value == taxCode)
                    {
                        item.CheckState = CheckState.Checked;
                        if (bCopyRecord) cboTaxCodes.Text = " ";
                    }
                }
            }
        }

        private void FillTaxCodeInfo()
        {
            DataTable taxCodeDataTable = TaxCodeHelper.GetTaxCodeDataTable();

            cboTaxCodes.DataSource = taxCodeDataTable;
            cboTaxCodes.ValueMember = taxCodeDataTable.Columns[0].ColumnName;
            cboTaxCodes.DisplayMember = taxCodeDataTable.Columns[1].ColumnName;
        }

        private void btnClose_Click(object sender, System.EventArgs e)
        {
            IsCanceled = true;
            this.Close();
        }

        private void btnCompanionItem_Click(object sender, System.EventArgs e)
        {
            try
            {
                if (!AllowEdit) return;
                if (oItemRow.ItemID == "")
                    return;

                Form objFrm = new frmCompanionItem(oItemRow.ItemID);
                objFrm.ShowDialog(this);
            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }

        private void btnGrpPricing_Click(object sender, System.EventArgs e)
        {
            try
            {
                if (!AllowEdit) return;
                if (oItemRow.ItemID == "") return;
                Form objFrm = new frmGroupPricing(oItemRow.ItemID);
                objFrm.ShowDialog(this);
            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
            }

        }

        private void btnVendors_Click(object sender, System.EventArgs e)
        {
            try
            {
                if (!AllowEdit) return;
                if (oItemRow.ItemID == "")
                    return;
                Form objFrm = new frmVendorItem(oItemRow.ItemID);
                objFrm.ShowDialog(this);
            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }

        private void txtBoxs_Validate(object sender, System.EventArgs e)
        {
            try
            {
                System.Windows.Forms.TextBox txtBox;
                Infragistics.Win.UltraWinEditors.UltraTextEditor txtEditor;
                string sText = "";
                string EditorName = "";
                if (oItemRow == null)
                    return;
                //Added By shitaljit on 17 Aug 2012 for Text Prediction while ading new Items.
                if (Configuration.CInfo.ShowTextPrediction == true && isItemEditMode == false && sender.ToString().Contains("System.Windows.Forms.TextBox"))
                {
                    string str = sender.ToString();
                    txtBox = (System.Windows.Forms.TextBox)sender;
                    sText = txtBox.Text.Trim(); //PRIMEPOS-2582 05-Sep-2018 JY need to trim the text to avoid white space before and after ItemCode and other text values
                    EditorName = txtBox.Name;
                }
                else if (sender.ToString().Contains("Infragistics.Win.UltraWinEditors.UltraTextEditor"))
                {
                    txtEditor = (Infragistics.Win.UltraWinEditors.UltraTextEditor)sender;
                    sText = txtEditor.Text.Trim();  //PRIMEPOS-2582 05-Sep-2018 JY need to trim the text to avoid white space before and after ItemCode and other text values
                    EditorName = txtEditor.Name;
                }

                switch (EditorName)
                {
                    case "txtItemCode":
                        oItemRow.ItemID = sText;
                        break;
                    case "txtItemDesc":
                    case "txtDescription":
                        oItemRow.Description = sText;
                        break;
                    case "txtProductCode":
                        oItemRow.ProductCode = sText;
                        break;
                    case "txtSaleTypeCode":
                        oItemRow.SaleTypeCode = sText;
                        break;
                    case "txtSeasonCode":
                        oItemRow.SeasonCode = sText;
                        break;
                    case "txtLocation":
                        oItemRow.Location = sText;
                        break;
                    case "txtRemarks":
                        oItemRow.Remarks = sText;
                        break;
                    case "txtUnit":
                        oItemRow.Unit = sText;
                        break;
                    case "txtPacketUnit":
                        oItemRow.PckUnit = sText;
                        break;
                    case "txtPacketSize":
                        oItemRow.PckSize = sText;
                        break;
                    case "txtPacketQuantity":
                        oItemRow.PckQty = sText;
                        break;
                    case "txtManufacturerName":
                        oItemRow.ManufacturerName = sText;
                        break;
                        //case "txtLotNumber"://Added by Krishna on 5 October 2011
                        //    oItemRow.LotNumber = sText;
                        //    break;
                }
            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
            }

        }

        private void numBoxs_Validate(object sender, System.EventArgs e)
        {
            try
            {
                System.Decimal numvalue = 0;
                if (oItemRow == null)
                    return;
                Infragistics.Win.UltraWinEditors.UltraNumericEditor numEditor = (Infragistics.Win.UltraWinEditors.UltraNumericEditor)sender;
                if (numEditor.Name.Equals("numDiscountAmt") && numEditor.Value.ToString().Contains("-"))
                {
                    clsUIHelper.ShowErrorMsg("Negative value for discount is not allowed");
                    numEditor.Value = 0;
                    return;
                }
                if (numEditor.NumericType == Infragistics.Win.UltraWinEditors.NumericType.Double)
                    numvalue = Decimal.Parse(numEditor.Value.ToString());
                else
                    numvalue = (int)numEditor.Value;

                switch (numEditor.Name)
                {
                    case "numFreight":
                        oItemRow.Freight = numvalue;
                        break;
                    case "numSellingPrice":
                        #region Sprint-21 - 2204 30-Jun-2015 JY Commented
                        //if (numvalue == 0)
                        //{
                        //    clsUIHelper.ShowErrorMsg("Selling Price Can Not Be 0.00");
                        //    this.numSellingPrice.Focus();
                        //}
                        //else
                        //    oItemRow.SellingPrice = numvalue;
                        //break;
                        #endregion
                        #region Sprint-21 - 2204 30-Jun-2015 JY Added to validate selling price if respective settings is off
                        //if (numvalue == 0 && ((isItemEditMode == false && Configuration.CInfo.AllowZeroSellingPrice == false) || (isItemEditMode == true)))
                        if (numvalue == 0 && Configuration.CInfo.AllowZeroSellingPrice == false)    //PRIMEPOS-2923 13-Nov-2020 JY modified to 
                        {
                            clsUIHelper.ShowErrorMsg("Selling Price Can Not Be 0.00");
                            this.numSellingPrice.Focus();
                        }
                        else
                            oItemRow.SellingPrice = numvalue;
                        break;
                    #endregion
                    case "numAveragePrice":
                        oItemRow.AvgPrice = numvalue;
                        break;
                    case "numSalePrice":
                        oItemRow.OnSalePrice = numvalue;
                        break;
                    case "numLastCostPrice":
                        oItemRow.LastCostPrice = numvalue;
                        break;
                    case "numDiscountAmt":
                        oItemRow.Discount = numvalue;
                        break;
                    case "numQtyInStock":
                        oItemRow.QtyInStock = (int)numvalue;
                        break;
                    case "numMinQty":
                        oItemRow.MinOrdQty = (int)numvalue;
                        break;
                    case "numReorderLevel":
                        oItemRow.ReOrderLevel = (int)numvalue; ;
                        break;
                    case "numQtyOnOrder":
                        oItemRow.QtyOnOrder = (int)numvalue;
                        break;
                    case "numSaleLimitQty"://Added By Ravindra for Sale limit 22 March 2013
                        oItemRow.SaleLimitQty = (int)numvalue;
                        break;
                    case "numCLPointsPerDollar":
                        oItemRow.PointsPerDollar = Configuration.convertNullToInt(numvalue);
                        oItemRow.IsDefaultCLPoint = false;
                        break;
                }
            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }

        private void dtpBoxs_Validate(object sender, System.EventArgs e)
        {
            try
            {
                if (oItemRow == null)
                    return;
                Infragistics.Win.UltraWinSchedule.UltraCalendarCombo dtpEditor = (Infragistics.Win.UltraWinSchedule.UltraCalendarCombo)sender;
                DateTime date;

                #region Sprint-21 - 2206 03-Jul-2015 JY Added code for item Exp. date
                if (dtpEditor.Name == "dtpExpiryDate" && dtpExpiryDate.Value.ToString() == "")
                {
                    oItemRow.ExpDate = null;
                    return;
                }
                #endregion

                #region PRIMEPOS-3000 17-Nov-2021 JY Added
                if (dtpEditor.Name == "dtpExpectedDelDate" && dtpExpectedDelDate.Value.ToString() == "")
                {
                    oItemRow.ExptDeliveryDate = null;
                    return;
                }
                #endregion

                date = Convert.ToDateTime(dtpEditor.Value.ToString());

                //Following Code Added by Krishna on 6 October 2011
                //if (dtpEditor.Name == "dtpExpiryDate" && dtpExpiryDate.Value.ToString() == "")
                //{
                //    oItemRow.ExpDate = null;
                //    return;
                //}
                //else
                //    date = Convert.ToDateTime(dtpEditor.Value.ToString());
                //Till Here added by Krishna on 6 October 2011

                switch (dtpEditor.Name)
                {
                    case "dtpSaleEndDate":
                        oItemRow.SaleEndDate = date;
                        break;
                    case "dtpSaleStartDate":
                        oItemRow.SaleStartDate = date;
                        break;
                    case "dtpLastRecvDate":
                        oItemRow.LastRecievDate = date;
                        break;
                    case "dtpLastSaleDate":
                        oItemRow.LastSellingDate = date;
                        break;
                    case "dtpExpectedDelDate":
                        oItemRow.ExptDeliveryDate = date;
                        break;
                    case "dtpExpiryDate"://This case Added by Krishna on 5 October 2011 //Sprint-21 - 2206 03-Jul-2015 JY uncommented code for item Exp. date
                        oItemRow.ExpDate = date;
                        break;
                }
            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
            }

        }

        private void CheckBoxs_Checked(object sender, System.EventArgs e)
        {
            try
            {
                if (oItemRow == null)
                    return;
                Infragistics.Win.UltraWinEditors.UltraCheckEditor chkEditor = (Infragistics.Win.UltraWinEditors.UltraCheckEditor)sender;

                bool IsChecked = chkEditor.Checked;

                switch (chkEditor.Name)
                {

                    case "chkIsDiscountable":
                        oItemRow.isDiscountable = IsChecked;
                        EnableDisableDisc();
                        break;
                    case "chkIsTaxable":
                        oItemRow.isTaxable = IsChecked;
                        EnableDisableTax();
                        DisplayActualTax(cboTaxCodes.Text); //PRIMEPOS-3037 13-Dec-2021 JY Added
                        break;
                    case "chkUpdatePriceItemWise":
                        oItemRow.UpdatePrice = IsChecked;
                        EnableDisableTax();
                        break;
                    case "chkIsOnSale":
                        oItemRow.isOnSale = IsChecked;
                        if (IsChecked == true)
                        {
                            oItemRow.SaleStartDate = Convert.ToDateTime(dtpSaleStartDate.Value);
                            oItemRow.SaleEndDate = Convert.ToDateTime(dtpSaleEndDate.Value);
                        }
                        EnableDisableOnSale();
                        break;
                    case "chkExclFromAutoPO":
                        oItemRow.ExclFromAutoPO = IsChecked;
                        break;
                    case "chkExclFromRecpt":
                        oItemRow.ExclFromRecpt = IsChecked;
                        break;
                    case "chkIsOTCItem":
                        oItemRow.isOTCItem = IsChecked;
                        break;
                    case "chkPacketSize":
                        grpItemPacketInfo.Enabled = chkPacketSize.Checked;
                        txtPacketSize.Focus();
                        break;
                    case "chkIsEBT":
                        oItemRow.IsEBTItem = chkIsEBT.Checked;
                        break;
                    //Added By Shitaljit on 2/6/2014 for PRIMEPOS-1806 Seperate Rx and OTC point calculation in CL
                    case "chkIsDefaultCLPoints":
                        oItemRow.IsDefaultCLPoint = chkIsDefaultCLPoints.Checked;
                        //this.numCLPointsPerDollar.Enabled = !chkIsDefaultCLPoints.Checked;    //Sprint-18 - 2041 29-Oct-2014 JY commented
                        break;
                    case "chkIsActive": //Sprint-21 - 2173 06-Jul-2015 JY Added
                        oItemRow.IsActive = chkIsActive.Checked;
                        break;
                    case "chkNonRefundable":    //PRIMEPOS-2592 01-Nov-2018 JY Added 
                        oItemRow.IsNonRefundable = chkNonRefundable.Checked;
                        break;
                }
            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }

        private void EnableDisableOnSale()
        {
            faOnSale.Enabled = chkIsOnSale.Checked;
            numSalePrice.Focus();
        }

        private void EnableDisableTax()
        {
            //txtTaxCode.Enabled = chkIsTaxable.Checked;
            //txtTaxDescription.Enabled = chkIsTaxable.Checked;
            //txtTaxCode.Focus();     //Added By Amit Date 1 Dec 2011
            //if (chkIsTaxable.Checked == false)
            //{
            //	txtTaxCode.Text = "";
            //	txtTaxDescription.Text = "";
            //	if (oItemRow != null) oItemRow.TaxID = 0;
            //}

            cboTaxCodes.Enabled = chkIsTaxable.Checked;
            //txtTaxDescription.Enabled = chkIsTaxable.Checked;
            cboTaxCodes.Focus();     //Added By Amit Date 1 Dec 2011
            //if (chkIsTaxable.Checked == false)
            //{
            //	txtTaxCode.Text = "";
            //	txtTaxDescription.Text = "";
            //	if (oItemRow != null) oItemRow.TaxID = 0;
            //}
        }
        private void EnableDisableDisc()
        {
            numDiscountAmt.Enabled = chkIsDiscountable.Checked;
            numDiscountAmt.Focus();     //Added By Amit Date 1 Dec 2011
            if (chkIsDiscountable.Checked == false)
            {
                numDiscountAmt.Value = 0;
                if (oItemRow != null) oItemRow.Discount = 0;
            }
        }

        private void ForiegnKeys_Leave(object sender, System.EventArgs e)
        {
            try
            {
                Infragistics.Win.UltraWinEditors.UltraTextEditor txtEditor = (Infragistics.Win.UltraWinEditors.UltraTextEditor)sender;

                string txtValue = txtEditor.Text;
                //Modified By shitaljit(QuicSolv) on 25 Jan 2001
                if (this.txtDepartmentCode.Text != "")
                {
                    if (oDepartmentRow != null)
                    {
                        if (this.txtDepartmentCode.Text != oDepartmentRow.DeptCode)
                        {
                            ShowEntityNotes = true;
                        }
                        else
                        {
                            ShowEntityNotes = false;
                        }
                    }
                    else
                    {
                        ShowEntityNotes = true;
                    }
                }                

                FKEdit(txtValue, txtEditor.Name);

            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }

        private void DisplayFK()
        {
            if (isItemEditMode == true && oItemData != null && oItemData.Tables.Count > 0 && oItemData.Tables[0].Rows.Count > 0 && Configuration.convertNullToInt(oItemData.Tables[0].Rows[0][clsPOSDBConstants.Item_Fld_DepartmentID]) == 0) //PRIMEPOS-2778 16-Jan-2020 JY Added if part to set the same data for dept that we have in the database
            {
                txtDepartmentCode.Text = "";
                txtDepartmentDescription.Text = "";
            }
            else if (oItemRow.DepartmentID != 0)
            {
                try
                {
                    oDepartmentData = oBRDepartment.Populate(oItemRow.DepartmentID);
                    oDepartmentRow = oDepartmentData.Department[0];
                    txtDepartmentCode.Text = oDepartmentRow.DeptCode;
                    txtDepartmentDescription.Text = oDepartmentRow.DeptName;
                }
                catch (Exception)
                {
                    txtDepartmentCode.Text = "";
                    txtDepartmentDescription.Text = "";
                }

                if (oItemRow.SubDepartmentID != 0)
                {
                    try
                    {
                        oSubDepartmentData = oSubBRDepartment.PopulateList(" And SubDepartmentID=" + oItemRow.SubDepartmentID.ToString());
                        oSubDepartmentRow = oSubDepartmentData.SubDepartment[0];
                        txtSubDepartment.Text = oSubDepartmentRow.SubDepartmentID.ToString();
                        lblSubDepartment.Text = oSubDepartmentRow.Description;
                    }
                    catch (Exception)
                    {
                        txtSubDepartment.Text = "";
                        lblSubDepartment.Text = "";
                    }
                }
            }

            SetTaxCodeChecked(TaxCodeHelper.FetchItemTaxInfo(oItemRow.ItemID));
            //Commented by Shrikant Mali, on 04/09/2014 as setting the tax code to text box is not required, replaced this code by above line.
            //if (oItemRow.TaxID != 0)
            //{
            //	try
            //	{
            //		oTaxcodeData = oBRTaxCodes.Populate(oItemRow.TaxID);
            //		oTaxCodesRow = oTaxcodeData.TaxCodes[0];
            //		txtTaxCode.Text = oTaxCodesRow.TaxCode;
            //		txtTaxDescription.Text = oTaxCodesRow.Description;
            //	}
            //	catch (Exception)
            //	{
            //		txtTaxCode.Text = "";
            //		txtTaxDescription.Text = "";
            //	}
            //}

            if (oItemRow.LastVendor != "")
            {
                try
                {
                    //Modified By SRT(Ritesh Parekh) Date : 27-Jul-2009
                    //modified by checking numeric value present in LastVendor, decission to take based on that.
                    if (clsUIHelper.isNumeric(oItemRow.LastVendor))
                    {
                        oVendorData = oBRVendor.Populate(Convert.ToInt32(oItemRow.LastVendor));
                    }
                    else
                    {
                        oVendorData = oBRVendor.Populate(oItemRow.LastVendor);
                    }
                    //End Of modified by SRT(Ritesh Parekh)

                    oVendorRow = oVendorData.Vendor[0];
                    txtLastVendor.Text = oVendorRow.Vendorcode;
                    txtVendorName.Text = oVendorRow.Vendorname;
                }
                catch (Exception)
                {
                    txtLastVendor.Text = "";
                    txtVendorName.Text = "";
                }
            }

        }
        //Added by shitalit(QuicSolv) on 11 Oct 2011
        private void ShowNotes(string code, string sendername)
        {
            Notes oNotes = new Notes();
            NotesData oNotesData = new NotesData();
            if (sendername == "Department")
            {
                string whereClause = " WHERE " + clsPOSDBConstants.Notes_Fld_EntityId + "= '" + code + "'  AND  " + clsPOSDBConstants.Notes_Fld_EntityType + "= '" + clsEntityType.DepartmentNote + "' AND " + clsPOSDBConstants.Notes_Fld_POPUPMSG + "= '" + true + "'";
                oNotesData = oNotes.PopulateList(whereClause);
                if (oNotesData.Notes.Rows.Count > 0)
                {
                    frmCustomerNotesView ofrmCustomerNotesView = new frmCustomerNotesView(code, clsEntityType.DepartmentNote);
                    ofrmCustomerNotesView.ShowDialog();
                }
            }
            if (sendername == "Vendor")
            {
                string whereClause = " WHERE " + clsPOSDBConstants.Notes_Fld_EntityId + "= '" + code + "'  AND  " + clsPOSDBConstants.Notes_Fld_EntityType + "= '" + clsEntityType.VendorNote + "' AND " + clsPOSDBConstants.Notes_Fld_POPUPMSG + "= '" + true + "'";
                oNotesData = oNotes.PopulateList(whereClause);
                if (oNotesData.Notes.Rows.Count > 0)
                {
                    frmCustomerNotesView ofrmCustomerNotesView = new frmCustomerNotesView(code, clsEntityType.VendorNote);
                    ofrmCustomerNotesView.ShowDialog();
                }
            }

        }
        //END of  Added by shitalit(QuicSolv) on 11 Oct 2011

        private void FKEdit(string code, string senderName)
        {

            oDepartmentRow = null;
            oTaxCodesRow = null;
            oVendorRow = null;

            switch (senderName)
            {
                //Added By Amit Date 30 Nov 2011    //Added to validated Item code already exist or not in add new item mode 
                case "txtItemCode":
                    try
                    {
                        if (isItemEditMode == false)
                        {
                            ItemData dsItem = new ItemData();

                            if (code.Trim() != "")
                            {
                                dsItem = oBRItem.Populate(code.Trim());

                                if (dsItem.Item != null && dsItem.Item.Rows.Count > 0)
                                {
                                    this.txtItemCode.Clear();
                                    this.txtItemCode.Focus();
                                    clsUIHelper.ShowErrorMsg(ErrorHandler.getCustomMessageFromDb(Convert.ToInt64(POSErrorENUM.Item_DuplicateCode)));
                                }
                            }
                        }
                    }
                    catch (Exception Ex)
                    {
                        clsUIHelper.ShowErrorMsg(Ex.Message);
                    }
                    break;
                //End

                case "txtDepartmentCode":
                    #region Commented By Amit Date 1 Dec 2011
                    //try
                    //{
                    //    oDepartmentData = oBRDepartment.Populate(code);
                    //    oDepartmentRow = oDepartmentData.Department[0];
                    //}
                    //catch (System.IndexOutOfRangeException)
                    //{
                    //    txtDepartmentCode.Text = "";
                    //    txtDepartmentDescription.Text = "";
                    //    oItemRow.DepartmentID = 0;
                    //}
                    //catch (Exception exp)
                    //{
                    //    clsUIHelper.ShowErrorMsg(exp.Message);
                    //}


                    //if (oDepartmentRow != null)
                    //{
                    //    txtDepartmentCode.Text = oDepartmentRow.DeptCode;
                    //    txtDepartmentDescription.Text = oDepartmentRow.DeptName;
                    //    oItemRow.DepartmentID = oDepartmentRow.DeptID;
                    //    //Added By Shitaljit(QuicSolv) 0n 11 oct 2011
                    //    if (isCallFromSearch == true)
                    //        ShowNotes(oDepartmentRow.DeptID.ToString(), "Department");
                    //    isCallFromSearch = false;
                    //    //END of Added By Shitaljit(QuicSolv) 0n 11 oct 2011
                    //}
                    //else
                    //{
                    //    txtDepartmentCode.Text = "";
                    //    txtDepartmentDescription.Text = "";
                    //    oItemRow.DepartmentID = 0;
                    //}
                    #endregion
                    //Added By Amit Date 1 Dec 2011
                    try
                    {
                        if (code.Trim() != "")
                        {
                            oDepartmentData = oBRDepartment.Populate(code);

                            if (oDepartmentData.Department != null && oDepartmentData.Department.Rows.Count > 0)
                            {
                                oDepartmentRow = oDepartmentData.Department[0];
                                txtDepartmentCode.Text = oDepartmentRow.DeptCode;
                                txtDepartmentDescription.Text = oDepartmentRow.DeptName;
                                oItemRow.DepartmentID = oDepartmentRow.DeptID;

                                if (ShowEntityNotes == true)
                                    ShowNotes(oDepartmentRow.DeptID.ToString(), "Department");
                                ShowEntityNotes = false;

                                #region Sprint-26 - PRIMEPOS-2412 29-Aug-2017 JY whenever we change the department, we need to clear the subdepartment
                                txtSubDepartment.Text = "";
                                oItemRow.SubDepartmentID = 0;
                                lblSubDepartment.Text = "";
                                #endregion
                            }
                            else
                            {
                                txtDepartmentCode.Text = "";
                                txtDepartmentDescription.Text = "";
                                oItemRow.DepartmentID = 0;
                                txtDepartmentCode.Focus();

                                clsUIHelper.ShowErrorMsg("Department code does not exist");
                            }
                        }
                        else
                        {
                            txtDepartmentCode.Text = "";
                            txtDepartmentDescription.Text = "";
                            oItemRow.DepartmentID = 0;

                            #region PRIMEPOS-3171 19-Dec-2022 JY Modified whenever we change the department, we need to clear the subdepartment
                            txtSubDepartment.Text = "";
                            oItemRow.SubDepartmentID = 0;
                            lblSubDepartment.Text = "";
                            #endregion
                        }
                    }
                    catch (Exception Ex)
                    {
                        clsUIHelper.ShowErrorMsg(Ex.Message);
                    }
                    //End
                    break;

                case "txtSubDepartment":
                    try
                    {
                        if (oItemRow != null && Configuration.convertNullToInt(oItemRow.DepartmentID) > 0 && Configuration.convertNullToInt(code.Trim()) > 0)
                        {
                            oSubDepartmentData = oSubBRDepartment.PopulateList(" AND DepartmentID = " + oItemRow.DepartmentID + " AND SubDepartmentID = " + code);  //PRIMEPOS-3171 19-Dec-2022 JY aded DeptID filter
                            oSubDepartmentRow = oSubDepartmentData.SubDepartment[0];
                        }
                        else
                        {

                            txtSubDepartment.Text = "";
                            lblSubDepartment.Text = "";
                            oItemRow.SubDepartmentID = 0;
                            oSubDepartmentRow = null;
                        }
                    }
                    catch (System.IndexOutOfRangeException)
                    {
                        txtSubDepartment.Text = "";
                        lblSubDepartment.Text = "";
                        oItemRow.SubDepartmentID = 0;
                    }
                    catch (Exception exp)
                    {
                        clsUIHelper.ShowErrorMsg(exp.Message);
                    }


                    if (oSubDepartmentRow != null)
                    {
                        txtSubDepartment.Text = oSubDepartmentRow.SubDepartmentID.ToString();
                        lblSubDepartment.Text = oSubDepartmentRow.Description;
                        oItemRow.SubDepartmentID = oSubDepartmentRow.SubDepartmentID;
                    }
                    else
                    {
                        txtSubDepartment.Text = "";
                        txtSubDepartment.Text = "";
                        oItemRow.SubDepartmentID = 0;
                    }
                    break;
                //Commented by Shrikant Mali, on 04/09/2014 as this control is removed.
                //case "txtTaxCode":
                //	try
                //	{
                //		oTaxcodeData = oBRTaxCodes.Populate(code);
                //		oTaxCodesRow = oTaxcodeData.TaxCodes[0];
                //	}
                //	catch (System.IndexOutOfRangeException)
                //	{
                //		txtTaxCode.Text = "";
                //		txtTaxDescription.Text = "";
                //		oItemRow.TaxID = 0;
                //	}
                //	catch (Exception exp)
                //	{
                //		clsUIHelper.ShowErrorMsg(exp.Message);
                //	}

                //	if (oTaxCodesRow != null)
                //	{
                //		txtTaxCode.Text = oTaxCodesRow.TaxCode;
                //		txtTaxDescription.Text = oTaxCodesRow.Description;
                //		oItemRow.TaxID = oTaxCodesRow.TaxID;
                //	}
                //	else
                //	{
                //		txtTaxCode.Text = "";
                //		txtTaxDescription.Text = "";
                //		oItemRow.TaxID = 0;
                //	}

                //	break;
                case "txtLastVendor":
                    try
                    {
                        oVendorData = oBRVendor.Populate(code);
                        oVendorRow = oVendorData.Vendor[0];
                        //Added By shitaljit on 9 April 2012
                        if (isItemEditMode == false)
                        {
                            this.grbVendorInformation.Enabled = true;
                            this.pnlSaveItemVendor.Visible = false;
                        }

                    }
                    catch (System.IndexOutOfRangeException)
                    {
                        txtLastVendor.Text = "";
                        txtVendorName.Text = "";
                        oItemRow.LastVendor = "";
                    }
                    catch (Exception exp)
                    {
                        clsUIHelper.ShowErrorMsg(exp.Message);
                    }

                    if (oVendorRow != null)
                    {
                        txtLastVendor.Text = oVendorRow.Vendorcode;
                        txtVendorName.Text = oVendorRow.Vendorname;
                        //Updated By SRT(Ritesh Parekh) Date : 27-Jul-2009
                        //Updated For last vendor updation in code format.
                        oItemRow.LastVendor = oVendorRow.Vendorcode;// oVendorRow.VendorId.ToString();
                        //End OF Updated By SRT(Ritesh Parekh)
                        //Added By Shitaljit(QuicSolv) 0n 11 oct 2011 
                        if (ShowEntityNotes == true)
                            ShowNotes(oVendorRow.VendorId.ToString(), "Vendor");
                        ShowEntityNotes = false;
                        //END of Added By Shitaljit(QuicSolv) 0n 11 oct 2011 
                    }
                    else
                    {
                        txtLastVendor.Text = "";
                        txtVendorName.Text = "";
                        oItemRow.LastVendor = "";
                    }
                    break;
            }

        }
        //Added By SRT(Prashant) On Date: 1/06/2009
        private void FillVendors()
        {
            POS_Core.BusinessRules.Vendor vendor = null;
            VendorData vendorData = null;
            try
            {
                vendor = new POS_Core.BusinessRules.Vendor();
                vendorData = vendor.PopulateList("");

                if (vendorData.Tables.Count > 0 && vendorData.Tables[0].Rows.Count > 0)
                {
                    //Added by Abhishek(SRT) -- 07/09/2009
                    //Added to sort the data by vendor 
                    DataRow[] vendorRows = vendorData.Tables[0].Select("", "VendorCode ASC");
                    //End of Added by Abhishek(SRT) 

                    foreach (DataRow vendorRow in vendorRows)
                    {
                        this.combEditorPrefVendor.Items.Add(vendorRow["VendorID"].ToString(), vendorRow["VendorCode"].ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                clsUIHelper.ShowErrorMsg(ex.ToString());
            }
        }
        //End of Added By SRT(Prashant) On Date: 1/06/2009 
        public void Edit(string ItemCode)
        {
            try
            {
                isItemEditMode = true;    //Added By Amit Date 30 Nov 2011
                btnCopyContents.Visible = false;    //Sprint-26 - PRIMEPOS-2417 06-Jul-2017 JY Added
                btnMMSSearch.Visible = false;   //PRIMEPOS-2671 17-Apr-2019 JY Added
                txtItemCode.Enabled = false;
                oItemData = oBRItem.Populate(ItemCode);
                oItemRow = oItemData.Item[0];
                if (oItemRow != null)
                    Display();

            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
            }

        }

        /// <summary>
        /// Author: Ritesh Parekh
        /// Date: 19-Aug-2009
        /// Details: Override of edit method with result parameter and validation for data.
        /// </summary>
        /// <param name="ItemCode"></param>
        /// <param name="Result"></param>
        public void Edit(string ItemCode, out bool Result)
        {
            try
            {
                isItemEditMode = true; //Added By AMit Date 30 Nov 2011
                btnCopyContents.Visible = false;    //Sprint-26 - PRIMEPOS-2417 06-Jul-2017 JY Added
                btnMMSSearch.Visible = false;   //PRIMEPOS-2671 17-Apr-2019 JY Added
                txtItemCode.Enabled = false;
                oItemData = oBRItem.Populate(ItemCode);
                if (oItemData != null && oItemData.Tables.Count > 0 && oItemData.Tables[0].Rows.Count > 0)
                {
                    oItemRow = oItemData.Item[0];
                    if (oItemRow != null)
                    {
                        //Following Code Added by Krishna on 12April2011
                        //pnlInventoryReceived.Location = new System.Drawing.Point(509, 668);
                        Display();
                    }
                    Result = true;
                }
                else
                {
                    throw (new Exception("Please use valid Item to update."));
                }
            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
                Result = false;
            }

        }

        private void Clear()
        {
            txtDepartmentCode.Text = "";
            txtDepartmentDescription.Text = "";
            txtDescription.Text = "";

            txtSubDepartment.Text = "";
            lblSubDepartment.Text = "";

            cboItemType.Text = "";
            txtLastVendor.Text = "";
            txtLocation.Text = "";
            txtProductCode.Text = "";
            txtRemarks.Text = "";
            txtSaleTypeCode.Text = "";
            txtSeasonCode.Text = "";
            //Commented by Shrikant Mali, on 04/09/2014 as this control is now removed.
            //txtTaxCode.Text = "";
            //txtTaxDescription.Text = "";
            txtUnit.Text = "";
            txtManufacturerName.Text = String.Empty;
            txtVendorName.Text = "";

            numAveragePrice.Value = 0;
            numDiscountAmt.Value = 0;
            numFreight.Value = 0;
            numLastCostPrice.Value = 0;
            numMinQty.Value = 0;
            numQtyInStock.Value = 0;
            numQtyOnOrder.Value = 0;
            numReorderLevel.Value = 0;
            numSalePrice.Value = 0;
            numSaleLimitQty.Value = 0;//Add by Ravindra for sale limit 22 March 2013
            numSellingPrice.Value = 0;
            numCaseCost.Value = 0;  //PRIMEPOS-3045 12-Jan-2021 JY Added

            dtpExpectedDelDate.Value = "";  //PRIMEPOS-3000 17-Nov-2021 JY Added
            dtpLastRecvDate.Value = System.DateTime.Now;
            dtpLastSaleDate.Value = System.DateTime.Now;
            dtpSaleEndDate.Value = System.DateTime.Now;
            dtpSaleStartDate.Value = System.DateTime.Now;

            chkIsDiscountable.Checked = false;
            chkIsTaxable.Checked = false;
            chkUpdatePriceItemWise.Checked = true;
            txtItemCode.Enabled = true;
            this.chkExclFromRecpt.Checked = false;
            this.chkExclFromAutoPO.Checked = false;

            chkIsOTCItem.Checked = false;
            chkIsOTCItem.Enabled = false;
            EnableDisableTax();
            EnableDisableDisc();

            if (oItemData != null) oItemData.Item.Rows.Clear();
            dtpExpiryDate.Value = "";   //Sprint-21 - 2206 06-Jul-2015 JY Added
            chkIsActive.Checked = true; //Sprint-21 - 2173 06-Jul-2015 JY Added 
            chkNonRefundable.Checked = false;   //PRIMEPOS-2592 01-Nov-2018 JY Added 
        }

        private void SetPreferedVendor(string strVendor)
        {
            foreach (ValueListItem vlItem in combEditorPrefVendor.Items)
            {
                if (vlItem.DisplayText.Trim().ToUpper() == strVendor.Trim().ToUpper())
                {
                    combEditorPrefVendor.SelectedItem = vlItem;
                    break;
                }
            }
        }
        private void SetTaxPolicy(string strTaxPolicy)
        {

            if (strTaxPolicy.Trim().ToUpper() == "I")
            {
                cmbTaxPolicy.SelectedItem = cmbTaxPolicy.Items[0];
            }
            else if (strTaxPolicy.Trim().ToUpper() == "D")
            {
                cmbTaxPolicy.SelectedItem = cmbTaxPolicy.Items[1];
            }
            else if (strTaxPolicy.Trim().ToUpper() == "O")
            {
                cmbTaxPolicy.SelectedItem = cmbTaxPolicy.Items[2];
            }
        }

        private void Display()
        {
            txtItemCode.Text = oItemRow.ItemID;
            //txtDepartmentCode = oItemRow.DepartmentID;
            //txtDepartmentDescription = oItemRow.DepartmentDesc;
            txtDescription.Text = oItemRow.Description;
            txtItemCode.Text = oItemRow.ItemID;
            cboItemType.Text = oItemRow.Itemtype;
            txtLastVendor.Text = oItemRow.LastVendor;
            //combEditorPrefVendor.Text  = oItemRow.PreferredVendor;  
            //Updated By SRT(Ritesh Parekh) Date : 27-Jul-2009
            //
            if (oItemRow.PreferredVendor != null)
            {
                SetPreferedVendor(oItemRow.PreferredVendor);
            }
            //End Of Updated By SRT(Ritesh Parekh)
            txtLocation.Text = oItemRow.Location;
            txtProductCode.Text = oItemRow.ProductCode;
            txtRemarks.Text = oItemRow.Remarks;
            txtSaleTypeCode.Text = oItemRow.SaleTypeCode;
            txtSeasonCode.Text = oItemRow.SeasonCode;
            //			txtTaxCode = oItemRow.TaxCo;
            //			txtTaxDescription = oItemRow.TaxDesc;
            txtUnit.Text = oItemRow.Unit;
            //			txtVendorName = oItemRow.;

            //txtLotNumber.Text = oItemRow.LotNumber;

            numAveragePrice.Value = oItemRow.AvgPrice;
            if (Configuration.convertNullToDecimal(oItemRow.Discount) <= Configuration.convertNullToDecimal(numDiscountAmt.MaxValue) && Configuration.convertNullToDecimal(oItemRow.Discount) >= Configuration.convertNullToDecimal(numDiscountAmt.MinValue))   //PRIMEPOS-2610 07-Nov-2018 JY Added constraint as it is throwing exception if discouint amount not in the range of min and max value
                numDiscountAmt.Value = oItemRow.Discount;

            numFreight.Value = oItemRow.Freight;
            numLastCostPrice.Value = oItemRow.LastCostPrice;
            numMinQty.Value = oItemRow.MinOrdQty;
            numQtyInStock.Value = oItemRow.QtyInStock;
            numQtyOnOrder.Value = oItemRow.QtyOnOrder;
            numReorderLevel.Value = oItemRow.ReOrderLevel;
            numSalePrice.Value = oItemRow.OnSalePrice;

            numSaleLimitQty.Value = oItemRow.SaleLimitQty;//Added By Ravindra for sale limit 22 March 2013
            numSellingPrice.Value = oItemRow.SellingPrice;

            dtpExpectedDelDate.Value = oItemRow.ExptDeliveryDate;
            dtpLastRecvDate.Value = oItemRow.LastRecievDate;
            dtpLastSaleDate.Value = oItemRow.LastSellingDate;
            dtpSaleEndDate.Value = oItemRow.SaleEndDate;
            dtpSaleStartDate.Value = oItemRow.SaleStartDate;
            dtpExpiryDate.Value = oItemRow.ExpDate;//Added by Krishna on 5 October 2011 //Sprint-21 - 2206 03-Jul-2015 JY uncommented code for item Exp. date

            chkIsDiscountable.Checked = oItemRow.isDiscountable;
            chkIsTaxable.Checked = oItemRow.isTaxable;
            chkIsOnSale.Checked = oItemRow.isOnSale;

            chkExclFromAutoPO.Checked = oItemRow.ExclFromAutoPO;
            chkExclFromRecpt.Checked = oItemRow.ExclFromRecpt;
            chkIsOTCItem.Checked = oItemRow.isOTCItem;
            chkIsOTCItem.Enabled = true;
            chkUpdatePriceItemWise.Checked = oItemRow.UpdatePrice;

            txtPacketSize.Value = oItemRow.PckSize.Trim();
            txtPacketQuantity.Value = oItemRow.PckQty.Trim();
            txtPacketUnit.Value = oItemRow.PckUnit.Trim();
            chkIsEBT.Checked = oItemRow.IsEBTItem;
            if (string.IsNullOrEmpty(oItemRow.TaxPolicy) == false)
            {
                SetTaxPolicy(oItemRow.TaxPolicy);
            }
            if (string.IsNullOrEmpty(oItemRow.DiscountPolicy) == false)
            {
                if (oItemRow.DiscountPolicy == "I")
                {
                    cmbDiscountPolicy.SelectedIndex = 0;
                }
                else if (oItemRow.DiscountPolicy == "D")
                {
                    cmbDiscountPolicy.SelectedIndex = 1;
                }
            }
            txtManufacturerName.Text = oItemRow.ManufacturerName;
            //Added By Shitaljit for PRIMEPOS-1806 Seperate Rx and OTC point calculation in CL
            this.chkIsDefaultCLPoints.Checked = oItemRow.IsDefaultCLPoint;
            this.numCLPointsPerDollar.Value = oItemRow.PointsPerDollar;
            //this.numCLPointsPerDollar.Enabled = !chkIsDefaultCLPoints.Checked;    //Sprint-18 - 2041 29-Oct-2014 JY commented
            //END
            chkIsActive.Checked = oItemRow.IsActive;    //Sprint-21 - 2173 06-Jul-2015 JY Added
            chkNonRefundable.Checked = oItemRow.IsNonRefundable;    //PRIMEPOS-2592 01-Nov-2018 JY Added 

            SetCLPointPolicy(oItemRow.CLPointPolicy);

            DisplayFK();
            EnableDisableTax();
            EnableDisableDisc();
            EnableDisableOnSale();
            if (!bCopyRecord)
                EnabelButtons();
            bCopyRecord = false;
            DisplayActualTax(cboTaxCodes.Text); //PRIMEPOS-3037 13-Dec-2021 JY Added            
        }

        private void Search(string SenderName)
        {
            try
            {
                //frmSearch oSearch = new frmSearch("");
                frmSearchMain oSearch = new frmSearchMain(true);    //20-Dec-2017 JY Added new reference

                switch (SenderName)
                {
                    case "txtItemCode":
                        //oSearch = new frmSearch(clsPOSDBConstants.Item_tbl);
                        oSearch.SearchTable = clsPOSDBConstants.Item_tbl;    //20-Dec-2017 JY Added
                        break;
                    case "txtDepartmentCode":
                        //oSearch = new frmSearch(clsPOSDBConstants.Department_tbl);
                        oSearch.SearchTable = clsPOSDBConstants.Department_tbl;    //20-Dec-2017 JY Added
                        //Added by Krishna on 29 April 2011------------------------
                        //  oSearch.txtCode.Text = txtDepartmentCode.Text;//Commented on 4 October 2011 for searching the whole Dept List as per Jira 178 ticket
                        oSearch.SearchInConstructor = true;
                        ShowEntityNotes = true; //Added By Shitaljit(QuicSolv) 0n 11 oct 2011 
                        //Till Here Added by Krishna-------------------
                        //Added by Amit Date 2 Nov 2011
                        oSearch.strSetSelected = txtDepartmentCode.Text.Trim();
                        //End
                        break;
                    case "txtSubDepartment":
                        //if (oDepartmentRow != null && oDepartmentRow.DeptID > 0)  //PRIMEPOS-3171 19-Dec-2022 JY Commented
                        if (oItemRow != null && Configuration.convertNullToInt(oItemRow.DepartmentID) > 0)  //PRIMEPOS-3171 19-Dec-2022 JY Added
                        {
                            //oSearch = new frmSearch(clsPOSDBConstants.SubDepartment_tbl);
                            oSearch.SearchTable = clsPOSDBConstants.SubDepartment_tbl;    //20-Dec-2017 JY Added
                            oSearch.SearchInConstructor = true;//Added By shitaljit on 9 April 2012
                            oSearch.AdditionalParameter = oItemRow.DepartmentID;    //PRIMEPOS-3171 19-Dec-2022 JY modified //oDepartmentRow.DeptID;
                        }
                        else
                        {
                            return;
                        }
                        break;
                    //Commented by Shrikant Mali, on 04/09/2014 as this control is removed.
                    //case "txtTaxCode":
                    //	oSearch = new frmSearch(clsPOSDBConstants.TaxCodes_tbl);
                    //	//Added by Krishna on 29 April 2011------------------------
                    //	oSearch.txtCode.Text = txtTaxCode.Text;
                    //	oSearch.searchInConstructor = true;
                    //	//Till Here Added by Krishna-------------------
                    //	break;
                    case "txtLastVendor":
                        //oSearch = new frmSearch(clsPOSDBConstants.Vendor_tbl);
                        oSearch.SearchTable = clsPOSDBConstants.Vendor_tbl;    //20-Dec-2017 JY Added
                        oSearch.SearchInConstructor = true;//Added By shitaljit on 9 April 2012
                        oSearch.ActiveOnly = 1;
                        ShowEntityNotes = true; //Added By Shitaljit(QuicSolv) 0n 11 oct 2011 
                        break;
                }
                oSearch.ShowDialog(this);
                if (!oSearch.IsCanceled)
                {
                    string strCode = oSearch.SelectedRowID();
                    if (strCode == "")
                        return;

                    if (SenderName == "txtItemCode")
                    {
                        Edit(strCode);
                    }
                    //Addede by Amit Date 3 Nov 2011 Showing popup if changing department of item
                    else if (SenderName == "txtDepartmentCode" && strCode != txtDepartmentCode.Text.Trim() && string.IsNullOrEmpty(this.txtDepartmentCode.Text.Trim()) == false && txtItemCode.Enabled == false) // in item add mode do not show popup
                    {
                        oDepartmentData = oBRDepartment.Populate(strCode);
                        oDepartmentRow = oDepartmentData.Department[0];
                        if (oItemRow.DepartmentID != oDepartmentRow.DeptID) //Sprint-26 - PRIMEPOS-2412 29-Aug-2017 JY Added logic to restrict pop up if user select the same department which was already selected
                        {
                            if (Resources.Message.Display("Do you want to change current Department of this Item from \"" + txtDepartmentDescription.Text.Trim() + "\" to Department \"" + oDepartmentRow.DeptName.Trim() + "\" ?",
                                                          "PrimePOS", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                            {
                                FKEdit(strCode, SenderName);
                            }
                        }
                    }
                    //End
                    else
                    {
                        FKEdit(strCode, SenderName);
                    }
                }
            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }

        private void btnSave_Click(object sender, System.EventArgs e)
        {
            Save();
        }


        //Added By Shitaljit(QuicSolv) on June 29 2011
        /// <summary>
        /// This will Validate all necessary fields.
        /// </summary>
        /// <returns></returns>
        private bool ValidateFields()
        {
            bool Validationflag = false;
            bool retVal = false;
            bool setDeptFlag = false;
            errorMsg = "";
            ItemRow oRow;


            oItemRow.ItemID = txtItemCode.Text;
            try
            {
                if ((oItemRow.ItemID == ""))
                {
                    Validationflag = true;
                    errorMsg = errorMsg + "\nPlease Enter Item Code Is Not Set";
                }
                if ((oItemRow.Description == ""))
                {
                    Validationflag = true;
                    errorMsg = errorMsg + "\nPlease Enter Item Description Is Not Set";
                }
                if (chkIsTaxable.Checked)
                {
                    if (cboTaxCodes.CheckedItems.Count == 0)
                    {
                        errorMsg = errorMsg + "\nPlease select Tax Code(s).";
                        Validationflag = true;
                    }
                    #region Sprint-19 - 2146 29-Dec-2014 JY Added multiple tax selection validation
                    else if (!Configuration.CPOSSet.SelectMultipleTaxes && cboTaxCodes.CheckedItems.Count > 1)
                    {
                        errorMsg = errorMsg + "\nYou can not select multiple Tax Codes as the respective settings is off.";
                        Validationflag = true;
                    }
                    #endregion
                }
                #region Sprint-21 - 2204 29-Jun-2015 JY Added to validate selling price if respective settings is off
                //if (isItemEditMode == false && Configuration.CInfo.AllowZeroSellingPrice == false && double.Parse(numSellingPrice.Value.ToString()) == 0.0)
                if (Configuration.CInfo.AllowZeroSellingPrice == false && double.Parse(numSellingPrice.Value.ToString()) == 0.0)    //PRIMEPOS-2923 13-Nov-2020 JY Added
                {
                    Validationflag = true;
                    errorMsg = errorMsg + "\nPlease Enter Selling Price";
                }
                #endregion
                if (Validationflag)
                {
                    throw (new Exception(errorMsg));
                }
                if (txtDepartmentCode.Text == "")
                {
                    if (Resources.Message.DisplayDefaultNo("Default Department for Item # " + oItemRow.ItemID + " is not set, Do you want to set it?", "Department Not Set", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        txtDepartmentCode.Focus();
                        setDeptFlag = true;
                    }
                    else
                    {
                        oItemRow.DepartmentID = Configuration.CInfo.DefaultDeptId;
                    }
                }
                //Added By Shitaljit(QuicSolv) on 19 August 2011
                if (cmbTaxPolicy.Text == "")
                {
                    oItemRow.TaxPolicy = "";
                }
                else
                {
                    if (cmbTaxPolicy.SelectedIndex == 0)
                        oItemRow.TaxPolicy = "I";
                    if (cmbTaxPolicy.SelectedIndex == 1)
                        oItemRow.TaxPolicy = "D";
                    if (cmbTaxPolicy.SelectedIndex == 2)
                        oItemRow.TaxPolicy = "O";
                }
                //END of added on 19 August.

                #region Sprint-18 - 2041 28-Oct-2014 JY Added
                if (cmbCLPointPolicy.SelectedIndex == 0)
                    oItemRow.CLPointPolicy = "";
                else if (cmbCLPointPolicy.SelectedIndex == 1)
                    oItemRow.CLPointPolicy = "I";
                else if (cmbCLPointPolicy.SelectedIndex == 2)
                    oItemRow.CLPointPolicy = "D";
                else if (cmbCLPointPolicy.SelectedIndex == 3)
                    oItemRow.CLPointPolicy = "S";
                #endregion

                oRow = (ItemRow)oItemData.Tables[0].Rows[0];
                ItemPriceValidation oItemPriceValidation = new ItemPriceValidation();
                if (oItemPriceValidation.ValidateItem(oItemData.Item[0], oItemData.Item[0].SellingPrice) == false)
                {
                    throw (new Exception("Current values in item conflicts with validation settings."));
                }
                if (oItemData.Item[0].isOTCItem == true)
                {
                    ItemMonitorCategoryDetail oIMCDetail = new ItemMonitorCategoryDetail();
                    ItemMonitorCategoryDetailData oIMCData = oIMCDetail.Populate(oItemData.Item[0].ItemID);
                    if (oIMCData.Tables[0].Rows.Count == 0)
                    {
                        throw (new Exception("Please select atleast one Item Monitoring Category."));
                    }
                }
                if (!setDeptFlag)
                    retVal = true;
            }
            catch (Exception exp)
            {
                if (errorMsg != "")
                    clsUIHelper.ShowErrorMsg(errorMsg);
                else
                    clsUIHelper.ShowErrorMsg(exp.Message);
            }

            #region PRIMEPOS-2705 13-May-2021 JY Added
            try
            {
                if (chkIsOnSale.Checked)
                {
                    if ((Convert.ToDateTime(dtpSaleEndDate.Value).Subtract(Convert.ToDateTime(dtpSaleStartDate.Value))).Days < 0)
                    {
                        throw (new Exception("The sale start date should be less than the sale end date."));
                    }
                }
            }
            catch (Exception Ex)
            {
                throw (Ex);
            }
            #endregion

            return retVal;
        }
        //End Of Added By Shitaljit.
        private bool Save()
        {
            bool retVal = false;
            try
            {
                //Following If is Added By Shitaljit(QuicSolv) on 30 June 2011
                if (ValidateFields())
                {
                    #region commented By Shitaljit move the code to ValidateFields() To Stop Saving items on Department not Set dialouge Box "Yes" option
                    //ItemRow oRow;
                    //bool Validationflag = false;
                    //bool setDeptFlag = false;
                    //string errorMsg = "";
                    //oItemRow.ItemID = txtItemCode.Text;


                    ////Following if condition is modified by shitaljit(QuicSolv) on 28 June 2011
                    //if (txtDepartmentCode.Text == "")
                    //{
                    //    if (Resources.Message.DisplayDefaultNo("Default DepartmentID For Item # " + oItemRow.ItemID + " Is Not Set Do You Want To Set It?", "Default De[partment", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    //    {
                    //        txtDepartmentCode.Focus();
                    //        setDeptFlag = true;
                    //    }
                    //}

                    ////ORIGINAL Commented By Shitaljit(QuicSolv) on 28 june 2011
                    ////Added by SRT(Abhishek)  Date : 01/09/2009
                    ////Message box will be shown if the department id is 0.
                    //// if (oItemRow.DepartmentID == 0 && Configuration.CInfo.DefaultDeptId == 0)
                    ////{
                    //////clsUIHelper.ShowErrorMsg("Default Department For Item " + oItemRow.ItemID + " Is Not Set.");
                    //    ////flag = true;
                    //    ////errorMsg = "\nDefault Department For Item " + oItemRow.ItemID + " Is Not Set";
                    //    ////throw (new Exception());
                    // // }
                    ////Commented By Shitaljit(QuicSolv) on 28 june 2011
                    ////if (Configuration.CInfo.DefaultDeptId != 0 && oItemRow.DepartmentID == Configuration.CInfo.DefaultDeptId)
                    ////{
                    ////    if (Resources.Message.DisplayDefaultNo("Default DepartmentID For Item # " + oItemRow.ItemID + " Is Not Set Do You Want To Set It?", "Default Department", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    ////    {
                    ////        txtDepartmentCode.Focus();
                    ////        setDeptFlag = true;
                    ////    }
                    ////    //oItemRow.DepartmentID = Configuration.CInfo.DefaultDeptId;
                    ////    //clsUIHelper.ShowErrorMsg("DepartmentID For Item "+ oItemRow.ItemID +" Is Not Set.\nDefault DepartmentID for Item is "+ Configuration.CInfo.DefaultDeptId.ToString()+".");
                    ////    //throw (new Exception());
                    ////    //flag = true;
                    ////    //errorMsg = errorMsg + "\nDepartmentID For Item " + oItemRow.ItemID + " Is Not Set.\nDefault DepartmentID for Item is " + Configuration.CInfo.DefaultDeptId.ToString() + "";
                    ////}
                    ////End of Added by SRT(Abhishek) 
                    ////Message Box will be shown if the Item Description and the Taxcode is not set


                    //#endregion
                    ////Following if is added by Shitaljit(QuicSolv) on june 28 2011
                    //if (Configuration.CInfo.DefaultDeptId != 0 && !setDeptFlag)
                    //{
                    //    oItemRow.DepartmentID = Configuration.CInfo.DefaultDeptId;
                    //}
                    ////Added By shitaljit(Quicsolv) Date: 01/04/2011
                    //if ((oItemRow.Description == ""))
                    //{
                    //    //Commented By Shitaljit(QuicSolv) on Date: 01/04/2011
                    //    //clsUIHelper.ShowErrorMsg("Plese Enter  Item Description"+oItemRow.Description+" Is Not Set");
                    //    //throw (new Exception());
                    //    Validationflag = true;
                    //    errorMsg = errorMsg + "\nPlease Enter  Item Description" + oItemRow.Description + " Is Not Set";

                    //}
                    //if (chkIsTaxable.Checked)
                    //{
                    //    if (oItemRow.TaxID == 0) 
                    //    {
                    //        //Commented By Shitaljit(QuicSolv) on Date: 01/04/2011
                    //        //clsUIHelper.ShowErrorMsg("Plese Enter Tax Ammount, Is Not Set");
                    //        //throw (new Exception());
                    //        errorMsg = errorMsg + "\nPlease Enter Tax Code, Is Not Set";
                    //        Validationflag = true;

                    //    }
                    //}
                    //if (Validationflag)
                    //{
                    //    throw (new Exception(errorMsg));
                    //}
                    ////End of added by shitaljit 
                    //oRow = (ItemRow)oItemData.Tables[0].Rows[0];
                    //ItemPriceValidation oItemPriceValidation = new ItemPriceValidation();
                    //if (oItemPriceValidation.ValidateItem(oItemData.Item[0], oItemData.Item[0].SellingPrice) == false)
                    //{
                    //    throw (new Exception("Current values in item conflicts with validation settings."));
                    //}
                    //if (oItemData.Item[0].isOTCItem == true)
                    //{
                    //    ItemMonitorCategoryDetail oIMCDetail = new ItemMonitorCategoryDetail();
                    //    ItemMonitorCategoryDetailData oIMCData = oIMCDetail.Populate(oItemData.Item[0].ItemID);
                    //    if (oIMCData.Tables[0].Rows.Count == 0)
                    //    {
                    //        throw (new Exception("Please select atleast one Item Monitoring Category."));
                    //    }
                    //}
                    ////Following if Condition is set By Shitaljit(QuicSolv) to stop item to save without dept 
                    //// if user says "yes" in the department not set message
                    #endregion
                    Configuration.UpdatedBy = "M";
                    oBRItem.Persist(oItemData);
                    EnabelButtons();

                    PersistSelectedTaxCodes();

                    retVal = true;
                }
            }

            catch (POSExceptions exp)
            {
                clsUIHelper.ShowErrorMsg(exp.ErrMessage);
                switch (exp.ErrNumber)
                {
                    case (long)POSErrorENUM.Item_DuplicateCode:
                        txtItemCode.Focus();
                        break;
                    case (long)POSErrorENUM.Item_CodeCanNotBeNULL:
                        txtItemCode.Focus();
                        break;
                    case (long)POSErrorENUM.Item_DepartmentCanNotBeNull:
                        txtDepartmentCode.Focus();
                        break;
                    case (long)POSErrorENUM.Item_DescriptionCanNotBeNULL:
                        txtDescription.Focus();
                        break;
                    case (long)POSErrorENUM.Item_TaxCodeCanNotBeNull:
                        cboTaxCodes.Focus();
                        break;
                }
            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
            return retVal;
        }

        private void PersistSelectedTaxCodes()
        {
            List<int> selectedTaxCodes = cboTaxCodes.CheckedItems.Select(checkedItem => int.Parse(checkedItem.DataValue.ToString())).ToList();

            TaxCodeHelper.PersistItemTaxCodes(selectedTaxCodes, oItemData.Item[0].ItemID.ToString(CultureInfo.InvariantCulture));
        }

        private void EnabelButtons()
        {
            pnlCompanionItem.Visible = true;
            pnlGrpPricing.Visible = true;
            //btnVendors.Visible = true;    Commented by Amit Date 6 Dec 2011
            btnInventoryHistory.Visible = true;
            btnPriceValidation.Visible = true;
            pnlItemMonitorCategory.Visible = true;
            pnlInventoryReceived.Visible = true;//Added by Krishna 
            pnlItemNote.Visible = UserPriviliges.IsUserHasPriviliges(UserPriviliges.Modules.Notes.ID, UserPriviliges.Screens.ItemNotes.ID);//Added by Krishna on 10 October 2011
        }

        private void SetNew()
        {
            oBRItem = new Item();
            oItemData = new ItemData();

            Clear();
            //Added By Shitaljit(QuicSolv) on 1 July 2011
            //Added last "false" value for isEBTItem field to set it false By default
            //Edited By Shitaljit(QuicSolv) on 10 Nov 2011 make by default item isDiscountable from false to true
            oItemRow = oItemData.Item.AddRow("", 0, "", "", "", "", "", "", 0, 0, 0, 0, false, 0, true, 0, DBNull.Value
                , DBNull.Value, 0, 0, "", 0, 0, 0, null
                , "", "", System.DateTime.MinValue, System.DateTime.MinValue, "", false, false, false, false, true, 0, false, 0, "", false, true, false, 0
                ,0,0,0,0);
            //Sprint-21 - 2173 06-Jul-2015 JY Added "True" parameter for IsActive //PRIMEPOS-2592 01-Nov-2018 JY Added "false" for IsNonRefundable // Added for Solutran: 0,0,0,0 - PRIMEPOS-2663 - NileshJ - 05-July-2019
        }

        private void btnNew_Click(object sender, System.EventArgs e)
        {
            try
            {
                txtItemCode.Text = "";
                SetNew();
            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }

        private void btnSearch_Click(object sender, System.EventArgs e)
        {
            try
            {
                Search(txtItemCode.Name);
            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
            }

        }

        private void txtItemCode_EditorButtonClick(object sender, Infragistics.Win.UltraWinEditors.EditorButtonEventArgs e)
        {
            try
            {
                Infragistics.Win.UltraWinEditors.UltraTextEditor txtBox = (Infragistics.Win.UltraWinEditors.UltraTextEditor)sender;
                Search(txtBox.Name);
            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
            }

        }

        private void cboItemType_SelectionChanged(object sender, System.EventArgs e)
        {
            try
            {
                if (oItemRow == null)
                    return;
                if (cboItemType.SelectedItem != null)
                    oItemRow.Itemtype = cboItemType.SelectedItem.DataValue.ToString();
                if (combEditorPrefVendor.SelectedItem != null)
                {
                    oItemRow.PreferredVendor = combEditorPrefVendor.SelectedItem.DisplayText;
                    this.grbVendorInformation.Enabled = true;//Added By shitaljit on 9 April 2012
                    this.pnlSaveItemVendor.Visible = false;
                }
            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
            }

        }

        private void frmItems_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            try
            {
                //System.Windows.Forms.KeysConverter keyConverter;
                switch (e.KeyData)
                {
                    case System.Windows.Forms.Keys.F5:
                        if (pnlCompanionItem.Visible == true)
                            btnCompanionItem_Click(null, null);
                        break;
                    case System.Windows.Forms.Keys.F6:
                        if (pnlGrpPricing.Visible == true)
                            btnGrpPricing_Click(null, null);
                        break;
                    case System.Windows.Forms.Keys.F9:
                        if (pnlItemMonitorCategory.Visible == true)
                            btnItemMonitorCategory_Click(null, null);
                        break;
                    case Keys.F2:
                        if (pnlInventoryReceived.Visible == true)
                            btnInventoryReceived_Click(null, null);
                        break;
                    case System.Windows.Forms.Keys.F3://Added by Krishna on 10 October 2011
                        if (pnlItemNote.Visible == true)
                            btnItemNote_Click(null, null);
                        break;
                    case System.Windows.Forms.Keys.F8://Added by shital;jit for secondery description 
                        if (pnlAddSecDesc.Visible == true)
                            btnAddSecDesc_Click(null, null);
                        break;
                }

                if (e.KeyData == System.Windows.Forms.Keys.F4)
                {
                    if (txtItemCode.ContainsFocus)
                        this.Search(txtItemCode.Name);
                    else if (txtDepartmentCode.ContainsFocus)
                        this.Search(txtDepartmentCode.Name);
                    else if (txtSubDepartment.ContainsFocus)
                        this.Search(txtSubDepartment.Name);
                    //Commented by Shrikant Mali, on 04/09/2014 as this control is removed and also serching taxcode is no longer required..
                    //else if (txtTaxCode.ContainsFocus)
                    //	this.Search(txtTaxCode.Name);
                    else if (txtLastVendor.ContainsFocus)
                        this.Search(txtLastVendor.Name);
                }

            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }
        private void frmItems_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            try
            {
                if (e.KeyData == System.Windows.Forms.Keys.Enter)
                {
                    this.SelectNextControl(this.ActiveControl, true, true, true, true);
                }
            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
            }

        }

        private void tabItemInfo_SelectedTabChanged(object sender, Infragistics.Win.UltraWinTabControl.SelectedTabChangedEventArgs e)
        {
            this.SelectNextControl(this.ActiveControl, true, true, true, true);

        }

        private void btnOk_Click(object sender, System.EventArgs e)
        {
            if (AllowEdit)
            {
                if (Save())
                {
                    try
                    {

                        if (isItemEditMode == false && (this.txtLastVendor.Text != "" || this.combEditorPrefVendor.Text.ToString() != "") && grdDetail.Rows[0].Cells["VendorID"].Value.ToString() != "")
                        {
                            SaveItemVendor();
                        }
                        IsCanceled = false;
                        this.Close();
                    }

                    catch (Exception Ex)
                    {
                        IsCanceled = false;
                        this.Close();
                    }
                }
            }
            else
            {
                clsUIHelper.ShowErrorMsg("You are not allowed to save changes.");
            }

        }

        private void frmItems_Activated(object sender, System.EventArgs e)
        {
            clsUIHelper.CurrentForm = this;
        }

        private void numSalePrice_ValueChanged(object sender, System.EventArgs e)
        {

        }

        private void numSalePrice_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
        {
            //			Infragistics.Win.UltraWinEditors.UltraNumericEditor numBox = (Infragistics.Win.UltraWinEditors.UltraNumericEditor)	sender;
            //			if (e.KeyChar.Equals('-'))
            //			{
            //				e.Handled = true;
            //				numBox.Value = (double)numBox.Value * (-1);
            //			}
        }

        private void btnInventoryHistory_Click(object sender, EventArgs e)
        {
            try
            {
                if (oItemRow.ItemID == "")
                    return;
                frmItemInvHistory ofrm = new frmItemInvHistory(oItemRow.ItemID.ToString(), oItemRow.Description);
                ofrm.ShowDialog(frmMain.getInstance());
            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }

        private void btnPriceValidation_Click(object sender, EventArgs e)
        {
            try
            {
                if (oItemRow.ItemID == "")
                    return;
                frmItemPriceValid ofrm = new frmItemPriceValid(oItemRow.ItemID.ToString());
                ofrm.ShowDialog(frmMain.getInstance());
            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }

        private void btnItemMonitorCategory_Click(object sender, EventArgs e)
        {
            try
            {
                if (!AllowEdit) return;
                if (oItemRow.ItemID == "")
                    return;

                Form objFrm = new frmItemMonitorCategoryDetail(oItemRow.ItemID, oItemRow.Description);
                objFrm.ShowDialog(this);
            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }
        //Following button Added by krishna
        private void btnInventoryReceived_Click(object sender, EventArgs e)
        {
            try
            {
                if (AllowEdit)
                {
                    string tempId = oItemRow.ItemID;
                    Item oBRitem1 = new Item();
                    ItemData oItemData1 = new ItemData();
                    ItemRow oitemrow1 = null;
                    oItemData1 = oBRitem1.Populate(tempId);
                    if (oItemData1.Tables[0].Rows.Count > 0)
                    {
                        string LastVendor = "";
                        if (oVendorRow != null)
                            LastVendor = oVendorRow.Vendorcode;
                        frmInventoryRecieved FrmInvntRecvd = new frmInventoryRecieved(oItemRow, LastVendor);
                        //FrmInvntRecvd.ShowDialog();//Commented By Shitaljit(QuicSolv) on 21 Sept 2011 
                        //Following if Is added By Shitaljit(QuicSolv) on 21 Sept 2011 
                        if (FrmInvntRecvd.ShowDialog() == DialogResult.OK)
                        {
                            oItemData1 = oBRitem1.Populate(tempId);
                            oitemrow1 = (ItemRow)oItemData1.Tables[0].Rows[0];
                            numQtyInStock.Value = oitemrow1.QtyInStock;
                            oItemRow.QtyInStock = Convert.ToInt32(numQtyInStock.Value);
                            numSellingPrice.Value = Convert.ToDouble(oitemrow1.SellingPrice);
                            oItemRow.SellingPrice = oitemrow1.SellingPrice;
                            numLastCostPrice.Value = Convert.ToDouble(oitemrow1.LastCostPrice);
                            oItemRow.LastCostPrice = oitemrow1.LastCostPrice;

                            //checking numeric value present in LastVendor, decission to take based on that.
                            if (clsUIHelper.isNumeric(oitemrow1.LastVendor))
                            {
                                oVendorData = oBRVendor.Populate(Convert.ToInt32(oitemrow1.LastVendor));
                            }
                            else
                            {
                                oVendorData = oBRVendor.Populate(oitemrow1.LastVendor);
                            }
                            //Following if Is added By Shitaljit(QuicSolv) on 21 Sept 2011 
                            if (oVendorData.Tables[0].Rows.Count > 0)
                            {
                                oVendorRow = oVendorData.Vendor[0];
                                txtLastVendor.Text = oVendorRow.Vendorcode;
                                txtVendorName.Text = oVendorRow.Vendorname;
                                oItemRow.LastVendor = oVendorRow.Vendorcode;
                            }
                        }
                    }
                    else
                    {
                        clsUIHelper.ShowWarningMsg("Item is not saved in the System.\nPlease save the Item first.");
                    }
                }
            }
            catch (Exception ex)
            {
                clsUIHelper.ShowErrorMsg(ex.Message);
            }
            // oItemRow.ItemID = "101";
        }
        //Till here Added by Krishna

        //Following Code Added by Krishna on 7 october 2011
        private void btnItemNote_Click(object sender, EventArgs e)
        {
            frmCustomerNotes oFrmCustNotes = new frmCustomerNotes(this.txtItemCode.Text, clsPOSDBConstants.Item_tbl, clsEntityType.ItemNote);
            oFrmCustNotes.ShowDialog();

        }
        //Till here Added by Krishna on 7 october 2011

        //Added By Shitaljit(QuicSolv) 0n 11 oct 2011
        private void combEditorPrefVendor_ValueChanged(object sender, EventArgs e)
        {
            if (combEditorPrefVendor.SelectedItem != null && oItemRow.PreferredVendor != combEditorPrefVendor.SelectedItem.DisplayText)
            {
                ShowNotes(combEditorPrefVendor.SelectedItem.DataValue.ToString(), "Vendor");
            }
        }
        //END of Added By Shitaljit(QuicSolv) 0n 11 oct 2011

        //Added By AmitDate 1 Dec 2011       
        private void tabItemInformation_SelectedTabChanged(object sender, SelectedTabChangedEventArgs e)
        {
            if (e.Tab.Key == "ItemInformation" && isItemEditMode == false)
            {
                this.txtItemCode.Select();
            }
            else if (e.Tab.Key == "PriceChangeLog" && isItemEditMode == true)
            {
                LoadPriceChangeLog();
            }
            else if (e.Tab.Key == "PhysicalInventoryView" && isItemEditMode == true)
            {
                LoadPhysicalInvView();
            }
            else if (e.Tab.Key == "ITP" && isItemEditMode == true)
            {
                PopulateItemPerformance(oItemRow.ItemID);
            }
        }
        //End
        //Added By Amit Date 2 Dec 2011
        #region Code for Item Vendor Information Tab

        private void getItemVendorDetail(string mItemId)
        {
            try
            {
                oItemVendorData = oItemVendor.PopulateList(" WHERE ItemVendor." + clsPOSDBConstants.ItemVendor_Fld_ItemID + " = '" + mItemId + "'");
                grdDetail.DataSource = oItemVendorData;
                grdDetail.Refresh();
                ApplyGrigFormat();
            }
            catch (POSExceptions exp)
            {
                clsUIHelper.ShowErrorMsg(exp.ErrMessage);
            }
        }

        private void ApplyGrigFormat()
        {

            grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.ItemVendor_Fld_VendorItemID].MaxLength = 20;
            grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.ItemVendor_Fld_ItemID].Hidden = true;
            grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.ItemVendor_Fld_VendorID].Hidden = true;
            grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.ItemVendor_Fld_ItemDetailID].Hidden = true;

            grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.ItemVendor_Fld_VendorCostPrice].CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right;
            grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.ItemVendor_Fld_VendorCostPrice].MaxValue = 99999.99;
            grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.ItemVendor_Fld_VendorCostPrice].Format = "####0.00";
            grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.ItemVendor_Fld_VendorCostPrice].MaskInput = "99999.99";

            grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.ItemVendor_Fld_VendorItemID].MaxLength = 20;

            grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.ItemVendor_Fld_VendorCode].MaxLength = 20;
            grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.ItemVendor_Fld_VendorCode].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.EditButton;

            grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.ItemVendor_Fld_LastOrderDate].DefaultCellValue = DateTime.Now;

            grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.ItemVendor_Fld_VendorCode].Header.Caption = "Vendor Code";
            grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.ItemVendor_Fld_VendorItemID].Header.Caption = "Vendor Item Code";
            grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.ItemVendor_Fld_VendorCostPrice].Header.Caption = "Price";
            grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.ItemVendor_Fld_VendorCode].Header.Caption = "Vendor Code";
            grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.ItemVendor_Fld_VendorName].CellActivation = Activation.Disabled;

            clsUIHelper.SetAppearance(this.grdDetail);

        }

        private void grdDetail_AfterCellUpdate(object sender, CellEventArgs e)
        {
            try
            {
                if (m_LastCell == 4)
                    EditVendor();
            }
            catch (POSExceptions exp)
            {
                clsUIHelper.ShowErrorMsg(exp.ErrMessage);
            }
        }

        private void EditVendor()
        {
            try
            {
                if (m_IsCellUpdateCalled)
                {
                    m_IsCellUpdateCalled = false;
                    return;
                }

                System.Int32 iVendorId;
                VendorData mVendorData = new VendorData();
                POS_Core.BusinessRules.Vendor mBRVendor = new POS_Core.BusinessRules.Vendor();
                VendorRow mVendorRow;
                mVendorData = mBRVendor.Populate(m_VendorCode);
                mVendorRow = (VendorRow)mVendorData.Tables[0].Rows[0];

                iVendorId = mVendorRow.VendorId;
                grdDetail.ActiveRow.Cells[clsPOSDBConstants.ItemVendor_Fld_VendorName].Value = mVendorRow.Vendorname;
                grdDetail.ActiveRow.Cells[clsPOSDBConstants.ItemVendor_Fld_VendorID].Value = iVendorId;
            }
            catch (Exception)
            {
                m_IsCellUpdateCalled = true;
                grdDetail.ActiveRow.Cells[clsPOSDBConstants.ItemVendor_Fld_VendorID].Value = System.DBNull.Value;
                m_IsCellUpdateCalled = true;
                grdDetail.ActiveRow.Cells[clsPOSDBConstants.ItemVendor_Fld_VendorCode].Value = System.DBNull.Value;
                m_IsCellUpdateCalled = true;
                grdDetail.ActiveRow.Cells[clsPOSDBConstants.ItemVendor_Fld_VendorName].Value = System.DBNull.Value;
                grdDetail.Focus();
            }
        }

        private void grdDetail_AfterRowInsert(object sender, RowEventArgs e)
        {
            try
            {
                if ((m_rowIndex) < 0) return;

                ItemVendorRow oRow = (ItemVendorRow)oItemVendorData.Tables[0].Rows[m_rowIndex];
                oRow.ItemID = oItemRow.ItemID;
                grdDetail.Update();
            }
            catch (Exception Exp)
            {
                clsUIHelper.ShowErrorMsg(Exp.Message);
            }
        }

        private void grdDetail_BeforeCellDeactivate(object sender, CancelEventArgs e)
        {
            /*UltraGridCell oCurrentCell;
			oCurrentCell=this.grdDetail.ActiveCell;
			if (oCurrentCell.DataChanged==false)
				return;
			try
			{
				if (oCurrentCell.Column.Key==clsPOSDBConstants.ItemVendor_Fld_VendorCode && oCurrentCell.Value.ToString()!="")
				{
					EditVendor();
					if (oCurrentCell.Value.ToString()=="")
					{
						e.Cancel=true;
						this.grdDetail.PerformAction(UltraGridAction.EnterEditMode);
					}
				}
				else if (oCurrentCell.Column.Key==clsPOSDBConstants.ItemVendor_Fld_VendorCostPrice)
				{
					oItemVendor.Validate_Price(oCurrentCell.Text.ToString());
				}
				else if (oCurrentCell.Column.Key==clsPOSDBConstants.ItemVendor_Fld_VendorItemID )
				{
					oItemVendor.Validate_VendorItemID(oCurrentCell.Text.ToString());
				}
			} 
			catch(Exception exp)
			{
				clsUIHelper.ShowErrorMsg(exp.Message);
				e.Cancel=true;
				this.grdDetail.PerformAction(UltraGridAction.EnterEditMode);
			}*/
        }

        private void grdDetail_BeforeRowDeactivate(object sender, CancelEventArgs e)
        {
            UltraGridRow oCurrentRow;
            UltraGridCell oCurrentCell;
            oCurrentRow = this.grdDetail.ActiveRow;
            oCurrentCell = null;
            bool blnCellChanged;
            blnCellChanged = false;

            if (oCurrentRow.Cells[clsPOSDBConstants.ItemVendor_Fld_VendorItemID].Text != "" ||
                oCurrentRow.Cells[clsPOSDBConstants.ItemVendor_Fld_VendorCode].Text != "" ||
                oCurrentRow.Cells[clsPOSDBConstants.ItemVendor_Fld_VendorCostPrice].Text != "")
            {
                blnCellChanged = true;
            }

            if (blnCellChanged == false)
            {
                return;
            }
            try
            {
                oCurrentCell = oCurrentRow.Cells[clsPOSDBConstants.ItemVendor_Fld_VendorItemID];
                oItemVendor.Validate_VendorItemID(oCurrentCell.Text.ToString());

                oCurrentCell = oCurrentRow.Cells[clsPOSDBConstants.ItemVendor_Fld_VendorCode];
                oItemVendor.Validate_VendorID(oCurrentCell.Text.ToString());

                oCurrentCell = oCurrentRow.Cells[clsPOSDBConstants.ItemVendor_Fld_VendorCostPrice];
                oItemVendor.Validate_Price(oCurrentCell.Text.ToString());

            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
                if (oCurrentCell != null)
                {
                    //oCurrentCell.Activate();
                    //oCurrentCell.Activate();
                    e.Cancel = true;
                    this.grdDetail.ActiveCell = oCurrentCell;
                    this.grdDetail.PerformAction(UltraGridAction.ActivateCell);
                    this.grdDetail.PerformAction(UltraGridAction.EnterEditMode);
                }
            }
        }

        private void grdDetail_CellChange(object sender, CellEventArgs e)
        {
            try
            {
                if (grdDetail.ActiveRow == null)
                {
                    m_rowIndex = -1;
                    return;
                }

                m_LastCell = e.Cell.Column.Index;

                if (e.Cell.Column.Index == 4)
                {
                    m_VendorCode = e.Cell.Text;
                }

                m_rowIndex = grdDetail.ActiveRow.Index;
            }
            catch (POSExceptions exp)
            {
                clsUIHelper.ShowErrorMsg(exp.ErrMessage);
            }
        }

        private void grdDetail_ClickCellButton(object sender, CellEventArgs e)
        {
            try
            {
                if (e.Cell.Column.Index == 4)
                    SearchVendor();
            }
            catch (POSExceptions exp)
            {
                clsUIHelper.ShowErrorMsg(exp.ErrMessage);
            }
        }

        private void SearchVendor()
        {
            try
            {
                System.Int32 iVendorId;
                VendorData mVendorData = new VendorData();
                POS_Core.BusinessRules.Vendor mBRVendor = new POS_Core.BusinessRules.Vendor();
                VendorRow mVendorRow;
                //frmSearch oSearch = new frmSearch(clsPOSDBConstants.Vendor_tbl);
                frmSearchMain oSearch = new frmSearchMain(true);    //20-Dec-2017 JY Added new reference
                oSearch.SearchTable = clsPOSDBConstants.Vendor_tbl; //20-Dec-2017 JY Added 
                oSearch.ActiveOnly = 1;
                oSearch.SearchInConstructor = true;//Added By shitaljit on 9 April 2012
                oSearch.ShowDialog(this);
                if (!oSearch.IsCanceled)
                {
                    mVendorData = mBRVendor.Populate(oSearch.SelectedRowID());
                    mVendorRow = (VendorRow)mVendorData.Tables[0].Rows[0];

                    iVendorId = mVendorRow.VendorId;
                    m_VendorCode = mVendorRow.Vendorcode;
                    grdDetail.ActiveRow.Cells[5].Value = mVendorRow.Vendorname;
                    grdDetail.ActiveRow.Cells[4].Value = mVendorRow.Vendorcode;
                    grdDetail.ActiveRow.Cells[3].Value = iVendorId;
                    grdDetail.Update();
                    grdDetail.Refresh();
                }
            }
            catch (Exception Exp)
            {
                clsUIHelper.ShowErrorMsg(Exp.Message);
            }
        }

        private void grdDetail_Enter(object sender, EventArgs e)
        {
            if (this.grdDetail.Rows.Count > 0)
            {
                if (!m_FromError) this.grdDetail.Rows[0].Cells[clsPOSDBConstants.ItemVendor_Fld_VendorItemID].Activate();
                this.grdDetail.PerformAction(UltraGridAction.EnterEditMode);
            }
            else
            {
                this.grdDetail.Rows.Band.AddNew();
            }
        }

        private void grdDetail_Error(object sender, ErrorEventArgs e)
        {
            m_FromError = true;
            e.Cancel = true;
            //CommonUI.checkGridError((Infragistics.Win.UltraWinGrid.UltraGrid)sender,e,clsPOSDBConstants.ItemVendor_Fld_VendorItemID,clsPOSDBConstants.ItemVendor_Fld_VendorCode);
        }

        private void grdDetail_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyData == Keys.Enter)
                {
                    if (this.grdDetail.ContainsFocus == true && this.grdDetail.ActiveCell.Text.Trim() == "" && this.grdDetail.ActiveCell.Column.Key == clsPOSDBConstants.ItemVendor_Fld_VendorItemID && this.grdDetail.ActiveCell.Row.IsAddRow == true)
                    {
                        //this.SelectNextControl(this.grdGroupPricing,true,true,true,true);
                        System.Windows.Forms.SendKeys.Send("{Tab}");
                        e.Handled = true;
                    }
                    else if (this.grdDetail.ContainsFocus == false)
                    {
                        this.SelectNextControl(this.ActiveControl, true, true, true, true);
                    }
                }
            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
            }

        }

        private void grdDetail_KeyUp(object sender, KeyEventArgs e)
        {
            if (this.grdDetail.ContainsFocus == true)
            {
                if (this.grdDetail.ActiveCell != null)
                {
                    if (this.grdDetail.ActiveCell.Column.Key == clsPOSDBConstants.ItemVendor_Fld_VendorCode)
                        if (e.KeyData == System.Windows.Forms.Keys.F4)
                        {
                            this.SearchVendor();
                            e.Handled = true;
                        }
                }
            }
            /*else if(e.KeyData==System.Windows.Forms.Keys.Enter)
            {
                this.SelectNextControl(this.ActiveControl,true,true,true,true);
            }
            */
        }

        private void grdDetail_Validated(object sender, EventArgs e)
        {
            grdDetail.PerformAction(UltraGridAction.LastCellInGrid);
        }

        private bool SaveItemVendor()
        {
            try
            {
                oItemVendor.Persist(oItemVendorData);
                this.isItemVendorSave = true;
                return true;
            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
                return false;
            }
        }


        private void setGridDetailFormat()
        {

            this.grdDetail.AfterEnterEditMode += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
            this.grdDetail.AfterExitEditMode += new System.EventHandler(clsUIHelper.AfterExitEditMode);

            this.grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.ItemVendor_Fld_AvgWholeSalePrice].Hidden = true;
            this.grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.ItemVendor_Fld_CatPrice].Hidden = true;
            this.grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.ItemVendor_Fld_ContractPrice].Hidden = true;
            this.grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.ItemVendor_Fld_DealerAdjustPrice].Hidden = true;
            this.grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.ItemVendor_Fld_FederalUpperLimitPrice].Hidden = true;
            this.grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.ItemVendor_Fld_ManufacturerSuggPrice].Hidden = true;
            this.grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.ItemVendor_Fld_NetItemPrice].Hidden = true;
            this.grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.ItemVendor_Fld_RetailPrice].Hidden = true;
            this.grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.ItemVendor_Fld_ProducerPrice].Hidden = true;
            this.grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.ItemVendor_Fld_IsDeleted].Hidden = true;

            //Added by SRT(Abhishek) Date : 23/09/2009 
            this.grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.ItemVendor_Fld_InvBillingPrice].Hidden = true;
            this.grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.ItemVendor_Fld_UnitPriceBegQuantity].Hidden = true;
            this.grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.ItemVendor_Fld_BaseCharge].Hidden = true;
            this.grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.ItemVendor_Fld_ResalePrice].Hidden = true;
            //End Of Added by SRT(Abhishek) Date : 23/09/2009

            this.grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.ItemVendor_Fld_AvgWholeSalePrice].DefaultCellValue = 0.00;
            this.grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.ItemVendor_Fld_CatPrice].DefaultCellValue = 0.00;
            this.grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.ItemVendor_Fld_ContractPrice].DefaultCellValue = 0.00;
            this.grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.ItemVendor_Fld_DealerAdjustPrice].DefaultCellValue = 0.00;
            this.grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.ItemVendor_Fld_FederalUpperLimitPrice].DefaultCellValue = 0.00;
            this.grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.ItemVendor_Fld_ManufacturerSuggPrice].DefaultCellValue = 0.00;
            this.grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.ItemVendor_Fld_NetItemPrice].DefaultCellValue = 0.00;
            this.grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.ItemVendor_Fld_RetailPrice].DefaultCellValue = 0.00;
            this.grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.ItemVendor_Fld_ProducerPrice].DefaultCellValue = 0.00;
            this.grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.ItemVendor_Fld_IsDeleted].DefaultCellValue = false;

            //Added By SRT(Abhishek) Date: 23/09/2009 
            this.grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.ItemVendor_Fld_InvBillingPrice].DefaultCellValue = 0.00;
            this.grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.ItemVendor_Fld_UnitPriceBegQuantity].DefaultCellValue = 0.00;
            this.grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.ItemVendor_Fld_BaseCharge].DefaultCellValue = 0.00;
            this.grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.ItemVendor_Fld_ResalePrice].DefaultCellValue = 0.00;
            //End Of Added By SRT(Abhishek) Date: 23/09/2009

            clsUIHelper.SetAppearance(this.grdDetail);
            clsUIHelper.SetKeyActionMappings(this.grdDetail);
            clsUIHelper.setColorSchecme(this);
            btnClose.Appearance.BackColor = Color.FromArgb(255, System.Convert.ToByte(POS_Core.Resources.Configuration.arrCloseBk[0].ToString()), System.Convert.ToByte(POS_Core.Resources.Configuration.arrCloseBk[1]), System.Convert.ToByte(POS_Core.Resources.Configuration.arrCloseBk[2]));
            btnClose.Appearance.BackColor2 = Color.FromArgb(255, System.Convert.ToByte(POS_Core.Resources.Configuration.arrCloseBk[0].ToString()), System.Convert.ToByte(POS_Core.Resources.Configuration.arrCloseBk[1]), System.Convert.ToByte(POS_Core.Resources.Configuration.arrCloseBk[2]));
        }

        private void btnSaveItemVendor_Click(object sender, EventArgs e)
        {
            try
            {
                if (grdDetail.Rows.Count > 0)
                {
                    if (SaveItemVendor())
                    {
                        clsUIHelper.ShowSuccessMsg("Item Vendor information saved successfully", "Saved Successfully");
                    }
                }
            }
            catch (Exception ex)
            {
                clsUIHelper.ShowErrorMsg(ex.ToString());
            }

        }


        private void btnDeleteItemVendorInfo_Click(object sender, EventArgs e)
        {
            try
            {
                if (grdDetail.Rows.Count < 0)
                {
                    return;
                }
                if (grdDetail.Selected.Rows.Count == 0)
                {
                    return;
                }
                else
                {
                    grdDetail.ActiveRow.Delete(false);

                }
                if (SaveItemVendor())
                {
                    clsUIHelper.ShowSuccessMsg("Item Vendor information Deleted successfully", "Deleted Successfully");
                }

            }
            catch (Exception ex)
            {
                clsUIHelper.ShowErrorMsg(ex.ToString());
            }

        }
        #endregion

        private void LoadPriceChangeLog()
        {
            try
            {
                if (isItemEditMode == true)
                {

                    DataSet oData;
                    Item oItem = new Item();
                    oData = oItem.GetItemPriceLog(oItemRow.ItemID);
                    this.grdPriceChangeLog.DataSource = oData;
                    this.grdPriceChangeLog.Refresh();
                    ApplyFormatToPriceChangeLogGrig();
                    resizeColumns(grdPriceChangeLog);

                    clsUIHelper.SetReadonlyRow(this.grdPriceChangeLog);
                    clsUIHelper.SetAppearance(this.grdPriceChangeLog);
                    clsUIHelper.setColorSchecme(this);
                    btnClose.Appearance.BackColor = Color.FromArgb(255, System.Convert.ToByte(POS_Core.Resources.Configuration.arrCloseBk[0].ToString()), System.Convert.ToByte(POS_Core.Resources.Configuration.arrCloseBk[1]), System.Convert.ToByte(POS_Core.Resources.Configuration.arrCloseBk[2]));
                    btnClose.Appearance.BackColor2 = Color.FromArgb(255, System.Convert.ToByte(POS_Core.Resources.Configuration.arrCloseBk[0].ToString()), System.Convert.ToByte(POS_Core.Resources.Configuration.arrCloseBk[1]), System.Convert.ToByte(POS_Core.Resources.Configuration.arrCloseBk[2]));
                }
            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }

        private void resizeColumns(UltraGrid oGrid)
        {
            try
            {
                foreach (UltraGridBand oBand in oGrid.DisplayLayout.Bands)
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

        private void ApplyFormatToPriceChangeLogGrig()
        {
            grdPriceChangeLog.DisplayLayout.Bands[0].Columns["ID"].Header.VisiblePosition = 0;
            grdPriceChangeLog.DisplayLayout.Bands[0].Columns["ItemID"].Header.VisiblePosition = 1;
            grdPriceChangeLog.DisplayLayout.Bands[0].Columns["Description"].Header.VisiblePosition = 2;
            grdPriceChangeLog.DisplayLayout.Bands[0].Columns["UserId"].Header.VisiblePosition = 3;
            grdPriceChangeLog.DisplayLayout.Bands[0].Columns["AddedOn"].Header.VisiblePosition = 4;
            grdPriceChangeLog.DisplayLayout.Bands[0].Columns["SalePrice"].Header.VisiblePosition = 5;
            grdPriceChangeLog.DisplayLayout.Bands[0].Columns["ORGSELLINGPRICE"].Header.VisiblePosition = 6;
            grdPriceChangeLog.DisplayLayout.Bands[0].Columns["CostPrice"].Header.VisiblePosition = 7;
            grdPriceChangeLog.DisplayLayout.Bands[0].Columns["OrgCostPrice"].Header.VisiblePosition = 8;
            grdPriceChangeLog.DisplayLayout.Bands[0].Columns["PromotionalPrice"].Header.VisiblePosition = 9;
            grdPriceChangeLog.DisplayLayout.Bands[0].Columns["OrgPromotionalPrice"].Header.VisiblePosition = 10;
            grdPriceChangeLog.DisplayLayout.Bands[0].Columns["UPDATEDBY"].Header.VisiblePosition = 11;
            grdPriceChangeLog.DisplayLayout.Bands[0].Columns["TRANSID"].Header.VisiblePosition = 12;

            this.grdPriceChangeLog.DisplayLayout.Bands[0].Columns["ID"].Hidden = true;
            this.grdPriceChangeLog.DisplayLayout.Bands[0].Columns["ChangedIn"].Hidden = true;
            grdPriceChangeLog.DisplayLayout.Bands[0].Columns["SalePrice"].Format = "#######0.00";
            grdPriceChangeLog.DisplayLayout.Bands[0].Columns["CostPrice"].Format = "#######0.00";
            grdPriceChangeLog.DisplayLayout.Bands[0].Columns["AddedOn"].Format = "MM/dd/yyyy hh:mm tt";
            grdPriceChangeLog.DisplayLayout.Bands[0].Columns["SalePrice"].Format = "#######0.00";

            grdPriceChangeLog.DisplayLayout.Bands[0].Columns["ORGSELLINGPRICE"].Format = "#######0.00";
            grdPriceChangeLog.DisplayLayout.Bands[0].Columns["OrgCostPrice"].Format = "#######0.00";
            grdPriceChangeLog.DisplayLayout.Bands[0].Columns["PromotionalPrice"].Format = "#######0.00";
            grdPriceChangeLog.DisplayLayout.Bands[0].Columns["OrgPromotionalPrice"].Format = "#######0.00";

            clsUIHelper.SetAppearance(this.grdPriceChangeLog);
            this.grdPriceChangeLog.DisplayLayout.Override.RowSelectors = DefaultableBoolean.False;
            //this.grdHistory.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.PhysicalInv_Fld_Amount]GetItemPriceLog.CellAppearance.TextHAlign= Infragistics.Win.HAlign.Right;

            grdPriceChangeLog.DisplayLayout.Bands[0].Columns["SalePrice"].Header.Caption = "Sale Price";
            grdPriceChangeLog.DisplayLayout.Bands[0].Columns["CostPrice"].Header.Caption = "Cost Price";
            grdPriceChangeLog.DisplayLayout.Bands[0].Columns["AddedOn"].Header.Caption = "Changed On";
            grdPriceChangeLog.DisplayLayout.Bands[0].Columns["UserId"].Header.Caption = "Changed By";
            grdPriceChangeLog.DisplayLayout.Bands[0].Columns["ItemID"].Header.Caption = "Item Code";
            grdPriceChangeLog.DisplayLayout.Bands[0].Columns["ORGSELLINGPRICE"].Header.Caption = "Original Selling Price";
            grdPriceChangeLog.DisplayLayout.Bands[0].Columns["OrgCostPrice"].Header.Caption = "Original Cost Price";
            grdPriceChangeLog.DisplayLayout.Bands[0].Columns["PromotionalPrice"].Header.Caption = "Promotional Price";
            grdPriceChangeLog.DisplayLayout.Bands[0].Columns["OrgPromotionalPrice"].Header.Caption = "Original Promotional Price";
            grdPriceChangeLog.DisplayLayout.Bands[0].Columns["UPDATEDBY"].Header.Caption = "Updated By";
            grdPriceChangeLog.DisplayLayout.Bands[0].Columns["TRANSID"].Header.Caption = "TransId";
        }

        private void LoadPhysicalInvView()
        {
            try
            {
                if (isItemEditMode == false)
                {
                    return;
                }

                PhysicalInv oPhysicalInv = new PhysicalInv();
                PhysicalInvData oGPhysicalInvData;
                oGPhysicalInvData = oPhysicalInv.PopulateList(" itemcode='" + oItemRow.ItemID + "' and isprocessed=1"); //JY corrected  //PRIMEPOS-2583 05-Sep-2018 JY Added
                this.grdPhysicalInvView.DataSource = oGPhysicalInvData;
                this.grdPhysicalInvView.Refresh();
                ApplyFormatToPhysicalInvView();
                resizeColumns(grdPhysicalInvView);
                clsUIHelper.SetReadonlyRow(this.grdPhysicalInvView);
                clsUIHelper.SetAppearance(this.grdPhysicalInvView);
                clsUIHelper.setColorSchecme(this);
                btnClose.Appearance.BackColor = Color.FromArgb(255, System.Convert.ToByte(POS_Core.Resources.Configuration.arrCloseBk[0].ToString()), System.Convert.ToByte(POS_Core.Resources.Configuration.arrCloseBk[1]), System.Convert.ToByte(POS_Core.Resources.Configuration.arrCloseBk[2]));
                btnClose.Appearance.BackColor2 = Color.FromArgb(255, System.Convert.ToByte(POS_Core.Resources.Configuration.arrCloseBk[0].ToString()), System.Convert.ToByte(POS_Core.Resources.Configuration.arrCloseBk[1]), System.Convert.ToByte(POS_Core.Resources.Configuration.arrCloseBk[2]));
            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }

        private void ApplyFormatToPhysicalInvView()
        {
            this.grdPhysicalInvView.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.PhysicalInv_Fld_ID].Hidden = true;
            this.grdPhysicalInvView.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.PhysicalInv_Fld_isProcessed].Hidden = true;
            this.grdPhysicalInvView.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.PhysicalInv_Fld_PTransDate].Hidden = true;
            this.grdPhysicalInvView.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.PhysicalInv_Fld_PUserID].Hidden = true;
            this.grdPhysicalInvView.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.PhysicalInv_Fld_TransDate].SortIndicator = SortIndicator.Descending;   //PRIMEPOS-2583 05-Sep-2018 JY Added
            clsUIHelper.SetAppearance(this.grdPhysicalInvView);
            this.grdPhysicalInvView.DisplayLayout.Override.RowSelectors = DefaultableBoolean.False;
        }

        private void txtItemDesc_Leave(object sender, EventArgs e)
        {
            clsUIHelper.AfterExitEditMode(sender, e);
        }

        private void txtItemDesc_Enter(object sender, EventArgs e)
        {
            clsUIHelper.AfterEnterEditMode(sender, e);
        }

        private void groupBox5_Enter(object sender, EventArgs e)
        {

        }

        //Added By Shitaljit for discount policy in item on 3 April 2013
        private void cmbDiscountPolicy_ValueChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(cmbDiscountPolicy.Text) == false)
            {
                if (cmbDiscountPolicy.SelectedIndex == 0)
                {
                    oItemRow.DiscountPolicy = "I";//Follow Item Discount settings
                }
                else
                {
                    oItemRow.DiscountPolicy = "D";//Follow Department Discount settings
                }
            }
        }

        private void btnAddSecDesc_Click(object sender, EventArgs e)
        {
            frmItemSecondaryDescription ofrmItemSecDesc = new frmItemSecondaryDescription();
            ofrmItemSecDesc.ItemID = oItemRow.ItemID;
            ofrmItemSecDesc.Description = oItemRow.Description;
            ofrmItemSecDesc.ShowDialog();
        }

        private const int pixelsToMoveItemCodeCbo = 115;

        private void cboTaxCodes_AfterCloseUp(object sender, EventArgs e)
        {
            cboTaxCodes.Location = new Point(cboTaxCodes.Left + pixelsToMoveItemCodeCbo, cboTaxCodes.Top);
            cboTaxCodes.Width = cboTaxCodes.Width - pixelsToMoveItemCodeCbo;

            cboTaxCodes.Text = TaxCodeHelper.GetTrimmedTaxCodes(cboTaxCodes.CheckedItems);
            #region Sprint-19 - 2146 29-Dec-2014 JY Added multiple tax selection validation
            if (chkIsTaxable.Checked)
            {
                if (cboTaxCodes.CheckedItems.Count == 0)
                {
                    clsUIHelper.ShowErrorMsg("\nPlease select Tax Code(s).");
                }
                else if (!Configuration.CPOSSet.SelectMultipleTaxes && cboTaxCodes.CheckedItems.Count > 1)
                {
                    clsUIHelper.ShowErrorMsg("\nYou can not select multiple Tax Codes as the respective settings is off.");
                }
            }
            #endregion
        }

        private void cboTaxCodes_BeforeDropDown(object sender, CancelEventArgs e)
        {
            cboTaxCodes.Location = new Point(cboTaxCodes.Left - pixelsToMoveItemCodeCbo, cboTaxCodes.Top);
            cboTaxCodes.Width = cboTaxCodes.Width + pixelsToMoveItemCodeCbo;
        }

        private void cboTaxCodes_TextChanged(object sender, EventArgs e)
        {
            cboTaxCodes.Text = TaxCodeHelper.GetTrimmedTaxCodes(cboTaxCodes.CheckedItems);
            DisplayActualTax(cboTaxCodes.Text); //PRIMEPOS-3037 13-Dec-2021 JY Added
        }

        #region Sprint-18 - 2041 28-Oct-2014 JY Added
        private void SetCLPointPolicy(string strCLPointPolicy)
        {
            if (strCLPointPolicy.Trim().ToUpper() == "I")
                cmbCLPointPolicy.SelectedItem = cmbCLPointPolicy.Items[1];
            else if (strCLPointPolicy.Trim().ToUpper() == "D")
                cmbCLPointPolicy.SelectedItem = cmbCLPointPolicy.Items[2];
            else if (strCLPointPolicy.Trim().ToUpper() == "S")
                cmbCLPointPolicy.SelectedItem = cmbCLPointPolicy.Items[3];
            else
                cmbCLPointPolicy.SelectedItem = cmbCLPointPolicy.Items[0];
        }
        #endregion

        #region Sprint-26 - PRIMEPOS-2417 06-Jul-2017 JY Added
        private void btnCopyContents_Click(object sender, EventArgs e)
        {
            POS_Core.ErrorLogging.Logs.Logger(this.Text, "btnCopyContents_Click() ", clsPOSDBConstants.Log_Entering);
            try
            {
                //frmSearch oSearch = new frmSearch(clsPOSDBConstants.Item_tbl);
                frmSearchMain oSearch = new frmSearchMain(true);    //20-Dec-2017 JY Added new reference
                oSearch.SearchTable = clsPOSDBConstants.Item_tbl;  //20-Dec-2017 JY Added 
                oSearch.sCalledFrom = this.Name;
                oSearch.ShowDialog(this);
                if (!oSearch.IsCanceled)
                {
                    string sItemCode = oSearch.SelectedRowID();
                    if (!String.IsNullOrWhiteSpace(sItemCode))
                    {
                        oItemData = oBRItem.Populate(sItemCode);
                        oItemData.Item[0].SetAdded();
                        oItemRow = oItemData.Item[0];
                        if (oItemRow != null)
                        {
                            bCopyRecord = true;
                            oItemRow.QtyInStock = 0;
                            oItemRow.isOTCItem = false;
                            string tempItemCode = string.Empty;
                            if (!string.IsNullOrWhiteSpace(this.txtItemCode.Text))
                                tempItemCode = this.txtItemCode.Text;
                            Display();
                            if (!string.IsNullOrWhiteSpace(tempItemCode))
                            {
                                oItemRow.ItemID = tempItemCode;
                                txtItemCode.Text = tempItemCode;
                            }
                            else
                            {
                                oItemRow.ItemID = "";
                                txtItemCode.Text = "";
                            }
                            this.txtItemCode.Focus();
                        }
                    }
                }
                POS_Core.ErrorLogging.Logs.Logger(this.Text, "btnCopyContents_Click() ", clsPOSDBConstants.Log_Exiting);
            }
            catch (Exception exp)
            {
                POS_Core.ErrorLogging.Logs.Logger(this.Text, "btnCopyContents_Click() ", clsPOSDBConstants.Log_Exception_Occured);
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }
        #endregion

        #region PRIMEPOS-2671 17-Apr-2019 JY Added
        private void btnMMSSearch_Click(object sender, EventArgs e)
        {
            try
            {
                if (Configuration.CInfo.PHNPINO.Trim() == "")
                {
                    Resources.Message.Display("NPI should not be blank", "PrimePOS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                else if (Configuration.CInfo.PSServiceAddress.Trim() == "")
                {
                    Resources.Message.Display("Please set service address in settings " + Environment.NewLine + " and try again", "PrimePOS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                OpenMMSSearch();
            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.Message);
            }
        }

        private void OpenMMSSearch()
        {
            try
            {
                frmSearchMain oSearch = new frmSearchMain(true);
                oSearch.SearchTable = clsPOSDBConstants.MMSSearch;
                oSearch.sNPINo = Configuration.CInfo.PHNPINO;
                oSearch.PSServiceAddress = Configuration.CInfo.PSServiceAddress;
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
                        Resources.Message.Display("Item already exists in the database", "PrimePOS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                    bCopyRecord = true;
                    oItemRow.ItemID = oItemM.ItemID;
                    oItemRow.Description = oItemM.Description;
                    oItemRow.ProductCode = oItemM.ProductCode;
                    oItemRow.SeasonCode = oItemM.SeasonCode;
                    oItemRow.Unit = oItemM.Unit;
                    oItemRow.Freight = oItemM.Freight;
                    //oItemRow.SellingPrice = oItemM.SellingPrice;  //no need to share selling price information
                    oItemRow.Itemtype = oItemM.Itemtype;
                    oItemRow.isTaxable = oItemM.isTaxable;
                    oItemRow.PckSize = oItemM.PCKSIZE;
                    oItemRow.PckQty = oItemM.PCKQTY;
                    oItemRow.PckUnit = oItemM.PCKUNIT;
                    //oItemRow.ItemStatus = oItemM.ItemStatus;  //not used as on 22-Apr-2019
                    oItemRow.ManufacturerName = oItemM.ManufacturerName;
                    oItemRow.IsEBTItem = oItemM.IsEBTItem;
                    txtDescription.Text = oItemRow.Description;
                    Display();
                    this.txtItemCode.Focus();
                }
            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }

        private void btnRefreshDataFromMMS_Click(object sender, EventArgs e)
        {
            try
            {
                if (Configuration.CInfo.PHNPINO.Trim() == "")
                {
                    Resources.Message.Display("NPI should not be blank", "PrimePOS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                else if (Configuration.CInfo.PSServiceAddress.Trim() == "")
                {
                    Resources.Message.Display("Please set service address in settings " + Environment.NewLine + " and try again", "PrimePOS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                if (txtItemCode.Text.Trim() != "")
                {
                    //get data from cetral location
                    DataSet oDataSet = new DataSet();
                    List<String> Criteria = new List<string>();
                    MMSSearch.Service objService = new MMSSearch.Service();

                    string sItemId = txtItemCode.Text.Trim();

                    if (sItemId.Length > 0)
                    {
                        Criteria.Add("ItemId" + "|" + sItemId);
                    }

                    if (Criteria.Count > 0)
                    {
                        objService.Url = Configuration.CInfo.PSServiceAddress.Trim() + @"Prime.SearchService.asmx";
                        string errMsg = objService.SearchItem(Criteria.ToArray(), Configuration.CInfo.PHNPINO, out oDataSet);

                        if (errMsg.Trim().ToUpper() == "invalid npi".ToUpper())
                        {
                            Resources.Message.Display("invalid npi", "PrimePOS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }
                        else
                        {
                            if (oDataSet != null && oDataSet.Tables.Count > 0 && oDataSet.Tables[0].Rows.Count > 0)
                            {
                                //send values to copy form
                                frmCopyMMSOpts ofrmCopyMMSOpts = new frmCopyMMSOpts(oDataSet, this.oItemData);
                                if (ofrmCopyMMSOpts.ShowDialog(this) == DialogResult.Yes)
                                {
                                    if (ofrmCopyMMSOpts.bUpdate == true)
                                    {
                                        bool callResult = false;
                                        Edit(txtItemCode.Text.Trim(), out callResult);
                                    }
                                    else
                                    {
                                        Resources.Message.Display("Selected parameters have been unchanged.", "PrimePOS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    }
                                }
                            }
                            else
                            {
                                Resources.Message.Display("Record not found.", "PrimePOS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                clsUIHelper.ShowErrorMsg(Ex.Message);
            }
        }
        #endregion

        #region Item Performance - NileshJ - PRIMEPOS-2654  - 05-Sep-2019
        public void PopulateItemPerformance(string itemID)
        {
            try
            {
                dsItemInfo = itemperfom.getItemPerformData(itemID, dtFrom.DateTime.Date, dtTo.DateTime.Date);
                dtSoldRecv = dsItemInfo.Tables[0];
                dtCostSaleamt = dsItemInfo.Tables[1];
                dtProfit = dsItemInfo.Tables[2];
                dtVendor = dsItemInfo.Tables[3];
                dtReturn = dsItemInfo.Tables[4];
                dtQtyInHand = dsItemInfo.Tables[5];
                ShowVendor();
                ShowGraph();
                ShowSummary();
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
            }
        }

        private void opSummBy_ValueChanged(object sender, EventArgs e)
        {
            ShowGraph();
        }

        private void ShowVendor()
        {
            grdVendor.DataSource = dtVendor;

            grdVendor.DisplayLayout.Bands[0].Columns["VendorCode"].Header.Caption = "Vendor Code";
            grdVendor.DisplayLayout.Bands[0].Columns["VendorCode"].Header.Appearance.TextHAlign = Infragistics.Win.HAlign.Center;
            grdVendor.DisplayLayout.Bands[0].Columns["VendorCode"].CellAppearance.TextHAlign = Infragistics.Win.HAlign.Center;

            grdVendor.DisplayLayout.Bands[0].Columns["VendorName"].Header.Caption = "Vendor Name";
            grdVendor.DisplayLayout.Bands[0].Columns["VendorName"].Header.Appearance.TextHAlign = Infragistics.Win.HAlign.Center;
            grdVendor.DisplayLayout.Bands[0].Columns["VendorName"].CellAppearance.TextHAlign = Infragistics.Win.HAlign.Center;

            grdVendor.DisplayLayout.Bands[0].Columns["QTY"].Header.Caption = "Quantity";
            grdVendor.DisplayLayout.Bands[0].Columns["QTY"].Header.Appearance.TextHAlign = Infragistics.Win.HAlign.Center;
            grdVendor.DisplayLayout.Bands[0].Columns["QTY"].CellAppearance.TextHAlign = Infragistics.Win.HAlign.Center;

            grdVendor.DisplayLayout.Bands[0].Columns["COST"].Format = "$00.00";
            grdVendor.DisplayLayout.Bands[0].Columns["COST"].Header.Caption = "Unit Cost";
            grdVendor.DisplayLayout.Bands[0].Columns["COST"].Header.Appearance.TextHAlign = Infragistics.Win.HAlign.Center;
            grdVendor.DisplayLayout.Bands[0].Columns["COST"].CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right;

            grdVendor.DisplayLayout.Bands[0].Columns["SALE"].Format = "$00.00";
            grdVendor.DisplayLayout.Bands[0].Columns["SALE"].Header.Caption = "Sale Price";
            grdVendor.DisplayLayout.Bands[0].Columns["SALE"].Header.Appearance.TextHAlign = Infragistics.Win.HAlign.Center;
            grdVendor.DisplayLayout.Bands[0].Columns["SALE"].CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right;
        }

        private void ShowGraph()
        {
            if (dsItemInfo.Tables.Count > 0)
            {
                if (opSummBy.Value == "Q")
                {
                    //dsChart.Tables[0].Columns.Remove("monthNo");
                    //dsChart.Tables[0].Columns.Remove("yearNo");
                    if (dtSoldRecv.Rows.Count > 0)
                    {
                        ucSummary.Axis.Y.Labels.ItemFormatString = "<DATA_VALUE:00>";
                        double maxRecv = Convert.ToDouble(dtSoldRecv.Compute("max(SOLD)", string.Empty).ToString());
                        double maxDisp = Convert.ToDouble(dtSoldRecv.Compute("max(RECEIVED)", string.Empty).ToString());
                        double maxRange = (maxRecv > maxDisp) ? maxRecv : maxDisp;
                        ucSummary.Axis.Y.RangeMax = maxRange + 5;

                        ucSummary.Axis.Y.RangeType = Infragistics.UltraChart.Shared.Styles.AxisRangeType.Custom;
                        ucSummary.Axis.Y.RangeMin = 0;
                        ucSummary.Axis.Y.TickmarkPercentage = 20.00;
                        ucSummary.Axis.Y.TickmarkIntervalType = Infragistics.UltraChart.Shared.Styles.AxisIntervalType.Ticks;
                        ucSummary.Refresh();

                        ucSummary.DataSource = dtSoldRecv;
                        ucSummary.DataBind();
                    }
                    else
                    {
                        ucSummary.DataSource = null;
                        ucSummary.DataBind();
                    }

                }
                else if (opSummBy.Value == "A")
                {
                    if (dtCostSaleamt.Rows.Count > 0)
                    {
                        //dsChart.Tables[0].Columns.Remove("datemonth");
                        //dsChart.Tables[0].Columns.Remove("dateyear");
                        ucSummary.Axis.Y.Labels.ItemFormatString = "<DATA_VALUE:$00.00>";
                        ucSummary.Axis.Y.RangeMax = Convert.ToDouble(dtCostSaleamt.Compute("max(SALEAMOUNT)", string.Empty)) + 5;

                        ucSummary.Axis.Y.RangeType = Infragistics.UltraChart.Shared.Styles.AxisRangeType.Custom;
                        ucSummary.Axis.Y.RangeMin = 0;
                        ucSummary.Axis.Y.TickmarkPercentage = 20.00;
                        ucSummary.Axis.Y.TickmarkIntervalType = Infragistics.UltraChart.Shared.Styles.AxisIntervalType.Ticks;
                        ucSummary.Refresh();

                        ucSummary.DataSource = dtCostSaleamt;
                        ucSummary.DataBind();
                    }
                    else
                    {
                        ucSummary.DataSource = null;
                        ucSummary.DataBind();
                    }
                }
                else if (opSummBy.Value == "P")
                {
                    if (dtProfit.Rows.Count > 0)
                    {
                        //dsChart.Tables[0].Columns.Remove("datemonth");
                        //dsChart.Tables[0].Columns.Remove("dateyear");
                        ucSummary.Axis.Y.Labels.ItemFormatString = "<DATA_VALUE:$00.00>";
                        ucSummary.Axis.Y.RangeMax = Convert.ToDouble(dtProfit.Compute("max(PROFIT)", string.Empty)) + 5;

                        ucSummary.Axis.Y.RangeType = Infragistics.UltraChart.Shared.Styles.AxisRangeType.Custom;
                        ucSummary.Axis.Y.RangeMin = 0;
                        ucSummary.Axis.Y.TickmarkPercentage = 20.00;
                        ucSummary.Axis.Y.TickmarkIntervalType = Infragistics.UltraChart.Shared.Styles.AxisIntervalType.Ticks;
                        ucSummary.Refresh();

                        ucSummary.DataSource = dtProfit;
                        ucSummary.DataBind();
                    }
                    else
                    {
                        ucSummary.DataSource = null;
                        ucSummary.DataBind();
                    }
                }
            }
            else
            {
                ucSummary.DataSource = null;
                ucSummary.DataBind();
            }
        }

        private void ShowSummary()
        {
            try
            {
                // Avg Sale Per Month
                lblAvgSalePerMonth.Text = (dtSoldRecv.Rows.Count == 0) ? "0" : Convert.ToString(Math.Round(Convert.ToDecimal(dtSoldRecv.Compute("AVG(SOLD)", "") == DBNull.Value ? 0 : dtSoldRecv.Compute("AVG(SOLD)", "")), 2));

                //Avg Return Per Month
                lblAvgReturnPerMonth.Text = (dtReturn.Rows.Count == 0) ? "0" : Convert.ToString(Math.Round(Convert.ToDecimal(dtReturn.Compute("AVG(RETURN)", "") ==
                    DBNull.Value ? 0 : dtReturn.Compute("AVG(RETURN)", "")), 2));

                // Avg Recv Per Month
                lblAvgRecvPerMonth.Text = (dtSoldRecv.Rows.Count == 0) ? "0" : Convert.ToString(Math.Round(Convert.ToDecimal(dtSoldRecv.Compute("AVG(RECEIVED)", "") == DBNull.Value ? 0 : dtSoldRecv.Compute("AVG(RECEIVED)", "")), 2));

                // Avg Profit Per Item
                string Sold = (dtSoldRecv.Rows.Count == 0) ? "0" : Convert.ToString(Math.Round(Convert.ToDecimal(dtSoldRecv.Compute("AVG(SOLD)", "") == DBNull.Value ? 0 : dtSoldRecv.Compute("AVG(SOLD)", "")), 2));
                // decimal sold = Convert.ToDecimal(dtSoldRecv.Compute("AVG(SOLD)", ""));
                decimal sold = Convert.ToDecimal(Sold);
                if (sold > 0)
                {
                    decimal avgProfit = Convert.ToDecimal(dtProfit.Compute("AVG(PROFIT)", "")) / Convert.ToDecimal(dtSoldRecv.Compute("AVG(SOLD)", ""));
                    //Convert.ToDecimal( (dtProfit.Rows.Count == 0) ? "0" : Convert.ToString(Math.Round(Convert.ToDecimal(dtProfit.Compute("AVG(PROFIT)", "") == DBNull.Value ? 0 : dtProfit.Compute("AVG(PROFIT)", "")), 2)));
                    lblAvgProfitPerItem.Text = String.Format("{0:C}", avgProfit);
                }
                else
                {
                    lblAvgProfitPerItem.Text = "0";
                }


                lblQtyInHand.Text = (dtQtyInHand.Rows.Count == 0) ? "0" : Convert.ToString(dtQtyInHand.Rows[0]["QtyInHand"] == DBNull.Value ? 0 : dtQtyInHand.Rows[0]["QtyInHand"]);//dtQtyInHand.Rows[0]["QtyInHand"].ToString();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }


        private void btnView_Click(object sender, EventArgs e)
        {
            string sItemCode = oItemRow.ItemID;
            PopulateItemPerformance(oItemRow.ItemID);
        }
        #endregion

        #region PRIMEPOS-3037 13-Dec-2021 JY Added
        private void cmbTaxPolicy_ValueChanged(object sender, EventArgs e)
        {
            DisplayActualTax(cboTaxCodes.Text);
        }

        private void DisplayActualTax(string strTaxCodes)
        {
            try
            {
                logger.Trace("DisplayActualTax() - " + clsPOSDBConstants.Log_Entering);
                lblActualTaxToDisplay.Text = "";
                Department oDepartment = new Department();
                DepartmentData oDepartmentData = new DepartmentData();
                ItemTax oTaxCodes = new ItemTax();
                TaxCodesData oTaxCodesData = new TaxCodesData();
                string DepartmentID = "";
                bool isDeptTaxable = false;
                string strDesc = string.Empty;

                oDepartmentData = oDepartment.Populate(txtDepartmentCode.Text);
                if (oDepartmentData != null && oDepartmentData.Tables.Count > 0 && oDepartmentData.Department.Rows.Count > 0)
                {
                    DepartmentRow oDepartmentRow = oDepartmentData.Department[0];
                    DepartmentID = Configuration.convertNullToString(oDepartmentRow.DeptID);
                    isDeptTaxable = oDepartmentRow.IsTaxable;
                }               

                if (cmbTaxPolicy.SelectedIndex == 2 || cmbTaxPolicy.SelectedIndex == -1)    //"O" - Dept Setting if dept is Taxable or NULL/blank
                {
                    if (isDeptTaxable)
                    {
                        oTaxCodesData = oTaxCodes.PopulateTaxCodeData(DepartmentID, EntityType.Department);
                        if (oTaxCodesData != null && oTaxCodesData.Tables.Count > 0 && oTaxCodesData.TaxCodes.Rows.Count > 0)
                        {
                            strDesc = "Dept Tax Setting";
                        }                        
                        else if (chkIsTaxable.Checked == true)
                        {
                            //if department is taxable but no tax record found then we need to consider item tax policy
                            oTaxCodesData = oTaxCodes.PopulateTaxCodeData(strTaxCodes, EntityType.Item, false);
                            if (oTaxCodesData != null && oTaxCodesData.Tables.Count > 0 && oTaxCodesData.TaxCodes.Rows.Count > 0)
                                strDesc = "Item Tax Setting";
                        }
                    }
                    else if (chkIsTaxable.Checked)
                    {
                        oTaxCodesData = oTaxCodes.PopulateTaxCodeData(strTaxCodes, EntityType.Item, false);
                        if (oTaxCodesData != null && oTaxCodesData.Tables.Count > 0 && oTaxCodesData.TaxCodes.Rows.Count > 0)
                            strDesc = "Item Tax Setting";
                    }
                }
                else if (cmbTaxPolicy.SelectedIndex == 1)   //"D" - Department Tax Setting
                {
                    if (isDeptTaxable == true)
                    {
                        oTaxCodesData = oTaxCodes.PopulateTaxCodeData(DepartmentID, EntityType.Department);
                        if (oTaxCodesData != null && oTaxCodesData.Tables.Count > 0 && oTaxCodesData.TaxCodes.Rows.Count > 0)
                            strDesc = "Dept Tax Setting";
                    }
                }
                else if (cmbTaxPolicy.SelectedIndex == 0)    //"I" - Item Tax Setting
                {
                    if (chkIsTaxable.Checked)
                    {
                        oTaxCodesData = oTaxCodes.PopulateTaxCodeData(strTaxCodes, EntityType.Item, false);
                        if (oTaxCodesData != null && oTaxCodesData.Tables.Count > 0 && oTaxCodesData.TaxCodes.Rows.Count > 0)
                            strDesc = "Item Tax Setting";
                    }
                }

                if (oTaxCodesData != null && oTaxCodesData.Tables.Count > 0 && oTaxCodesData.TaxCodes.Rows.Count > 0)
                {
                    foreach(TaxCodesRow row in oTaxCodesData.TaxCodes.Rows)
                    {
                        if (lblActualTaxToDisplay.Text == "")
                            lblActualTaxToDisplay.Text = row.TaxCode + "-" + row.Amount + "%";
                        else
                            lblActualTaxToDisplay.Text += "," + row.TaxCode + "-" + row.Amount + "%";
                    }
                    lblActualTaxToDisplay.Text += ":" + strDesc;
                }
                logger.Trace("DisplayActualTax() - " + clsPOSDBConstants.Log_Exiting);
            }
            catch (Exception Ex)
            {
                logger.Fatal(Ex, "DisplayActualTax()");
            }
        }        

        private void txtDepartmentCode_ValueChanged(object sender, EventArgs e)
        {
            DisplayActualTax(cboTaxCodes.Text);
        }
        #endregion

        #region PRIMEPOS-3045 12-Jan-2021 JY Added
        private void txtPacketUnit_ValueChanged(object sender, EventArgs e)
        {
            if (this.txtPacketUnit.Text.Trim().ToUpper() == "CS" || this.txtPacketUnit.Text.Trim().ToUpper() == "CA")
            {
                lblCaseCost.Visible = numCaseCost.Visible = true;
                try
                {
                    numCaseCost.Value = Configuration.convertNullToInt(txtPacketSize.Text) * Configuration.convertNullToDecimal(numLastCostPrice.Value);
                }
                catch { }
            }
            else
                lblCaseCost.Visible = numCaseCost.Visible = false;            
        }        

        private void txtPacketSize_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                numCaseCost.Value = Configuration.convertNullToInt(txtPacketSize.Text) * Configuration.convertNullToDecimal(numLastCostPrice.Value);
            }
            catch { }
        }

        private void numLastCostPrice_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                numCaseCost.Value = Configuration.convertNullToInt(txtPacketSize.Text) * Configuration.convertNullToDecimal(numLastCostPrice.Value);
            }
            catch { }
        }
        #endregion

        private void numCaseCost_ValueChanged(object sender, EventArgs e)
        {
            if (!numCaseCost.Enabled) return;
            try
            {
                if (Configuration.convertNullToDecimal(numCaseCost.Value) > 0 && Configuration.convertNullToInt(txtPacketSize.Text) > 0)
                {
                    decimal val = Configuration.convertNullToDecimal(numCaseCost.Value) / Configuration.convertNullToInt(txtPacketSize.Text);

                    if (Configuration.convertNullToDecimal(numLastCostPrice.Value) != val)
                    {
                        numLastCostPrice.Value = val;
                        oItemRow.LastCostPrice = val;
                    }
                }
            }
            catch { }
        }

        #region PRIMEPOS-3171 19-Dec-2022 JY Modified
        private void txtSubDepartment_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (oItemRow == null || Configuration.convertNullToInt(oItemRow.DepartmentID) == 0)
                {
                    e.Handled = true;
                }
            }
            catch (Exception Ex)
            {
            }
        }
        #endregion
    }
}