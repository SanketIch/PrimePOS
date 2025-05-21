using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
//using POS_Core.DataAccess;
//using POS_Core_UI.Reports.ReportsUI;
using POS_Core.CommonData;
using POS_Core.CommonData.Rows;
using Infragistics.Win;
using POS_Core.BusinessRules;
using POS_Core.CommonData.Tables;
using POS_Core.ErrorLogging;
using POS_Core.DataAccess;
using Infragistics.Win.UltraWinGrid;
using Infragistics.Win.UltraWinEditors;
using POS_Core.Resources;
using POS_Core_UI.Reports.ReportsUI;
using POS_Core_UI.Resources;
using NLog;

namespace POS_Core_UI
{
    public partial class frmCreateNewPurchaseOrder : Form
    {
        private string vendorCode = string.Empty;
        private POHeaderData oPOHeaderData;
        private PODetailData oPODetailData;
        private POHeaderRow oPOHeaderRow;
        private OrderDetailsData orderDetailData;
        private MasterOrderDetailsData masterOrderDetailsData = null;
        private IDictionary<String, String> vendorDetails = null;
        private VendorData vendorData = null;
        private string nonBestVendor = string.Empty;
        private string poID = string.Empty;
        private int totalNoOfPOs = 0;
        private IList<String> vendorList = null;
        private List<String> orderNOs = null;
        private IDictionary<String, String> podIdOrdernoDict = null;
        private IDictionary<String, String> orderNoOrderIdDict = null;
        private IDictionary<String, POHeaderData> poHeaderList = null;
        private IDictionary<String, List<String>> poOrdersNos = null;
        private List<String> newlyAddedPOIDs = null;
        private IDictionary<String, String> partiallyAckPOIDs = null;
        private List<String> newlyAddedOrderIDs = null;
        static bool useBestVendor = false;
        //private static int poDetailId;
        private bool isMaxOrderIDFirstTime = false;
        private bool isEditPO = false;
        private bool isAutoPO = false;
        private bool isGridRowDeleted = true;
        private bool isBestVendorUpdated = false;
        private bool isCanceled = false;
        private bool isIncomplete = false;
        private bool isPartiallyAck = false;
        private bool isCopyOrder = false;
        private bool isFlaggedOrder = false;
        private bool isFilledPODetails = false;
        private bool m_IsExceptionOccured = false;
        private bool isItemMovedToVendorSelected = false;
        private bool haveVendorInfo = false;
        private int orderNumber = 0;
        private PurchaseOrder oPurchaseOrder = null;
        private UltraDropDown vendorDropDown = null;
        private bool isAckManual = false;

        private UltraDropDown orderNosDropDown = null;
        private string vendorSelected = string.Empty;
        private string vendorBeforeSelection = string.Empty;
        private string selectedPODetailIDFromVendorCombo = string.Empty;
        private string selectedOrderIDFromVendorCombo = string.Empty;
        private bool AskForOrderedItem = true;
        //Added By SRT(Gaurav) Date : 15-Jul-2009
        //This private variable will holde value to validate to add or description for order or not.
        private bool GetOrderDescription = true;
        //End Of Added By SRT(Gaurav)
        private bool isShowOrderedItems = false;
        //Added By SRT(Gaurav) Date : 17-Jul-2009    

        //Added By SRT(Abhshek) Date : 13-Aug-2009
        private bool isItemPopulated = false;

        //Added By SRT(Ritesh Parekh) Date: 19-Aug-2009
        frmShowMsgBox showOrderMsgBox = null;
        //End Of Added By SRT(Ritesh Parekh)

        //Added By SRT(Ritesh Parekh) date: 29-Aug-2009
        private bool IsReportCalled = false;
        //End Of Added By sRT(Ritesh Parekh)

        //Added By SRT(Abhishek) date: 09-Sept-2009
        private int activeRowIndex = 0;
        //End Of Added By SRT(Abhishek)

        //Added by Amit Date 21 June 2011
        public static string strBarCodeOf = null;

        //added by Shitaljit 
        private static frmCreateNewPurchaseOrder ofrmCreateNewPurchaseOrder;
        private ILogger logger = LogManager.GetCurrentClassLogger();    //PRIMEPOS-3030 26-Nov-2021 JY Added       

        public static frmCreateNewPurchaseOrder getInstance()
        {
            if (ofrmCreateNewPurchaseOrder == null || ofrmCreateNewPurchaseOrder.IsDisposed == true)
            {
                ofrmCreateNewPurchaseOrder = new frmCreateNewPurchaseOrder();
            }
            return ofrmCreateNewPurchaseOrder;
        }

        public bool IsItemPopulated
        {
            get
            {
                if (!isItemPopulated)
                {
                    if (this.gridItemDetails.ActiveRow != null)
                    {
                        try
                        {
                            isItemPopulated = IsPODetailExist(this.gridItemDetails.ActiveRow.Cells[clsPOSDBConstants.PODetail_Fld_PODetailID].Value.ToString());
                            if (isItemPopulated == false)
                            {
                                isItemPopulated = true;
                            }
                        }
                        catch (Exception Ex)
                        {
                            isItemPopulated = false;
                        }
                        //isItemPopulated = true;
                    }
                }
                return (isItemPopulated);
            }
        }

        public bool IsThisRowValidated
        {
            get
            {
                bool result = false;
                if (this.gridItemDetails.ActiveRow != null)
                {
                    try
                    {
                        result = IsPODetailExist(this.gridItemDetails.ActiveRow.Cells[clsPOSDBConstants.PODetail_Fld_PODetailID].Value.ToString());

                    }
                    catch (Exception Ex)
                    {
                        result = false;
                    }
                }


                return (result);
            }
        }
        //End Of Added By SRT(Abhishek)

        private int TotalItemsCount
        {
            get
            {
                int count = 0;
                if (this.isEditPO)
                {
                    if (masterOrderDetailsData == null || masterOrderDetailsData.Tables.Count == 0)
                    {
                        count = 0;
                    }
                    else
                    {
                        count = masterOrderDetailsData.PODetailTable.Rows.Count;
                    }
                }
                else
                {
                    if (orderDetailData == null || orderDetailData.Tables.Count == 0)
                    {
                        count = 0;
                    }
                    else
                    {
                        count = orderDetailData.PODetailTable.Rows.Count;
                    }
                }
                return (count);
            }
        }

        private int TotalVendorsCount
        {
            get
            {
                int count = 0;
                if (this.isEditPO)
                {
                    if (masterOrderDetailsData == null || masterOrderDetailsData.Tables.Count == 0)
                    {
                        count = 0;
                    }
                    else
                    {
                        count = masterOrderDetailsData.MasterOrderDetailTable.Rows.Count;
                    }
                }
                else
                {
                    if (orderDetailData == null || orderDetailData.Tables.Count == 0)
                    {
                        count = 0;
                    }
                    else
                    {
                        count = orderDetailData.OrderDetailsTable.Rows.Count;
                    }
                }
                return (count);
            }
        }

        private int CSItemCount
        {
            get
            {
                int count = 0;
                if (gridItemDetails.Rows.Count == 0)
                {
                    count = 0;
                }
                else
                {
                    foreach (UltraGridRow oRow in gridItemDetails.Rows)
                    {
                        // Commented by Shrikant Mali : extracted a function so as to extract a method out of the if conditioin to check if the item is
                        // case item or not.
                        // if (oRow.Cells[clsPOSDBConstants.PODetail_Fld_PackUnit].Value.ToString().ToUpper().Trim() == clsPOSDBConstants.PckUnit_CS)
                        if (IsCaseItem(oRow))
                            //if (gridItemDetails.ActiveRow.Cells[clsPOSDBConstants.PODetail_Fld_PackUnit].Value.ToString().Trim() == clsPOSDBConstants.PckUnit_CS)
                            count = count + 1;
                    }
                }
                return (count);
            }
        }

        /// <summary>
        /// Auther : Shrikant Mali
        /// Date : 07/02/2014
        /// Checks if the row provided is of Case item or not.
        /// </summary>
        /// <param name="oRow">Row of Item details grid view.</param>
        /// <returns>True if the item is a Case Item.</returns>
        private static bool IsCaseItem(UltraGridRow oRow)
        {
            try
            {
                return ((oRow.Cells[clsPOSDBConstants.PODetail_Fld_PackUnit].Value.ToString().ToUpper().Trim() == clsPOSDBConstants.PckUnit_CS) || (oRow.Cells[clsPOSDBConstants.PODetail_Fld_PackUnit].Value.ToString().ToUpper().Trim() == clsPOSDBConstants.PckUnit_CA));    //Sprint-21 22-Feb-2016 JY Added CA for case item
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Auther : Shrikant Mali
        /// Date : 07/02/2014
        /// Checks if the row provided is of NON Case item or not.
        /// </summary>
        /// <param name="oRow">Row of Item details grid view.</param>
        /// <returns>True if the item is a Case Item.</returns>
        private static bool IsNonCaseItem(UltraGridRow oRow)
        {
            return !IsCaseItem(oRow);
        }

        private decimal totalCostForAllPO
        {
            get
            {
                decimal TotalPOCost = 0;
                if (isEditPO == false)
                {
                    if (gridItemDetails.Rows.Count == 0)
                    {
                        TotalPOCost = 0;
                    }
                    else
                    {
                        foreach (UltraGridRow oRow in gridPODetails.Rows)
                        {
                            TotalPOCost += Configuration.convertNullToDecimal(oRow.Cells[clsPOSDBConstants.OrderDetail_Fld_TotalCost].Value);
                        }
                    }
                }
                return (TotalPOCost);
            }
        }

        private bool IsThisVendorValid(string VendCD)
        {
            bool result = false;
            VendorData oData = new VendorData();
            POS_Core.BusinessRules.Vendor oVendorClass = new POS_Core.BusinessRules.Vendor();
            if (oVendorClass.GetVendorId(VendCD) > 0)
            {
                result = true;
            }
            return result;
        }

        public frmCreateNewPurchaseOrder()
        {
            InitializeComponent();
            clsUIHelper.setColorSchecme(this);
            btnClose.Appearance.BackColor = Color.FromArgb(255, System.Convert.ToByte(POS_Core.Resources.Configuration.arrCloseBk[0].ToString()), System.Convert.ToByte(POS_Core.Resources.Configuration.arrCloseBk[1]), System.Convert.ToByte(POS_Core.Resources.Configuration.arrCloseBk[2]));
            btnClose.Appearance.BackColor2 = Color.FromArgb(255, System.Convert.ToByte(POS_Core.Resources.Configuration.arrCloseBk[0].ToString()), System.Convert.ToByte(POS_Core.Resources.Configuration.arrCloseBk[1]), System.Convert.ToByte(POS_Core.Resources.Configuration.arrCloseBk[2]));
            //this.MdiParent = frmMain.getInstance();
            vendorDetails = new Dictionary<String, String>();
            oPurchaseOrder = new PurchaseOrder();
            vendorDropDown = new UltraDropDown();
            orderNosDropDown = new UltraDropDown();
            vendorList = new List<String>();
            partiallyAckPOIDs = new Dictionary<String, String>();
            newlyAddedPOIDs = new List<String>();
            newlyAddedOrderIDs = new List<String>();
            orderNoOrderIdDict = new Dictionary<String, String>();
            podIdOrdernoDict = new Dictionary<String, String>();
            poHeaderList = new Dictionary<String, POHeaderData>();
            poOrdersNos = new Dictionary<String, List<String>>();
            orderNOs = new List<String>();

            cboItemFilterOptions.Items[0].Tag = POItemDisplayFilter.All;
            cboItemFilterOptions.Items[1].Tag = POItemDisplayFilter.OnlyCaseItems;
            cboItemFilterOptions.Items[2].Tag = POItemDisplayFilter.OnlyNonCaseItems;

            cboItemFilterOptions.SelectedIndex = 0;
        }
        private void frmCreateNewPurchaseOrder_Load(object sender, EventArgs e)
        {
            logger.Trace("frmCreateNewPurchaseOrder_Load() - " + clsPOSDBConstants.Log_Entering);
            //clsUIHelper.setColorSchecme(this);
            if (pnlAddNewItem.Enabled) this.btnAddNewItem.Select();
            clsUIHelper.SetKeyActionMappings(this.gridItemDetails);
            clsUIHelper.SetAppearance(this.gridItemDetails);
            //this.ulDropDown.Enabled = false;         
            this.vendorDropDown.RowSelected += new RowSelectedEventHandler(vendorDropDown_RowSelected);
            this.vendorDropDown.KeyDown += new KeyEventHandler(vendorDropDown_KeyDown);
            this.gridItemDetails.AfterEnterEditMode += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
            this.gridItemDetails.AfterExitEditMode += new System.EventHandler(clsUIHelper.AfterExitEditMode);

            this.Left = 0;
            this.Top = 0;
            this.MaximizeBox = false;   //Sprint-21 18-Nov-2015 JY Added to avoid overlap bottom buttons with taskbar
            frmMain.HideCloseButton = false;//PRIMEPOS-3294
            frmMain.getInstance().ShowInTaskbar = false;
            txtEditorNoOfCSItems.Text = "0";
            // Added by Shrikant Mali on 02/10/2014 to update the count of non case Items.
            tbxNonCaseItems.Text = "0";
            if (this.gridItemDetails.DisplayLayout.Bands[0].Columns.Count == 32)
                this.gridItemDetails.DisplayLayout.Bands[0].Columns["SoldItems"].Hidden = true;
            //lblTotalCostForAllPO.Visible = txtTotalCostForAllPO.Visible = !isEditPO;  //Sprint-21 - 1803 11-Sep-2015 JY Commented to display total cost
            logger.Trace("frmCreateNewPurchaseOrder_Load() - " + clsPOSDBConstants.Log_Exiting);
        }        

        void vendorDropDown_KeyDown(object sender, KeyEventArgs e)
        {
            //throw new Exception("The method or operation is not implemented.");
            if (e.KeyCode == Keys.Enter)
            {
                MessageBox.Show("Enter Pressed");
            }
        }

        private void vendorDropDown_RowSelected(object sender, RowSelectedEventArgs e)
        {
            string vendorName = string.Empty;
            string selectedOrderID = string.Empty;
            String origVendorID = String.Empty;
            DataRow[] orgVendRows = null;
            bool isVendorSelected = false;
            bool isVendorExist = false;
            //DataRow vendorRow = vendorDropDown
            try
            {
                if (e.Row != null)
                {
                    vendorName = this.gridItemDetails.ActiveRow.Cells[clsPOSDBConstants.PODetail_Fld_VendorName].Value.ToString();
                    vendorSelected = e.Row.Cells["Value"].Value.ToString();

                    //Updated By SRT(Gaurav) Date: 09-Jul-2009
                    //Updated To solve crash issue regarding empty vendorname.
                    if (vendorName.Trim().Length == 0 || vendorSelected.Trim().Length == 0)
                    {
                        return;
                    }
                    //End Of Updated By SRT(Gaurav)

                    if (vendorSelected != vendorName)
                    {
                        orgVendRows = vendorData.Vendor.Select("VendorCode ='" + vendorName + "'");
                        if (orgVendRows.Length > 0)
                        {
                            origVendorID = orgVendRows[0][clsPOSDBConstants.POHeader_Fld_VendorID].ToString();
                            selectedOrderID = this.gridItemDetails.ActiveRow.Cells[clsPOSDBConstants.PODetail_Fld_OrderID].Value.ToString();
                            selectedPODetailIDFromVendorCombo = this.gridItemDetails.ActiveRow.Cells[clsPOSDBConstants.PODetail_Fld_PODetailID].Value.ToString();
                            ////Check whether the row is updated? ALONG WITH QTY IS NOT NULL OR ZERO
                            ////IF QTY IS NULL.... IRRESPECTIVE OF ANY VENDOR CHANGE EXIT AND FOCUS QTY....
                            ////If ROW not UPDATED AND QTY IS NOT NULL then

                            if (selectedPODetailIDFromVendorCombo.Trim().Length > 0)
                            {
                                if (!IsPODetailExist(selectedPODetailIDFromVendorCombo))
                                {
                                    //store current row id of grid in temp variable.
                                    int curRowId = 0;
                                    curRowId = gridItemDetails.ActiveRow.Index;
                                    //go to next row... this will update the current row with data
                                    bool isNext = gridItemDetails.PerformAction(UltraGridAction.NextRow);
                                    //Go back to current location of old row.
                                    if (isNext == false)
                                    {
                                        gridItemDetails.PerformAction(UltraGridAction.PrevRow);
                                        this.gridItemDetails.ActiveRow.Cells[clsPOSDBConstants.PODetail_Fld_VendorName].Value = vendorName;
                                        this.gridItemDetails.ActiveCell.CancelUpdate();
                                        //gridItemDetails.ActiveRow.Cells[clsPOSDBConstants.PODetail_Fld_QTY].Activate();
                                        FocusQty();
                                        return;
                                    }
                                    gridItemDetails.PerformAction(UltraGridAction.PrevRow);
                                    FocusToCell(clsPOSDBConstants.PODetail_Fld_VendorName);
                                    ////gridItemDetails.ActiveRow.Cells[clsPOSDBConstants.PODetail_Fld_VendorName].Activate();
                                }
                                ////If row is updated then continue following logic.                            
                                isVendorSelected = UpdateVendorSelectedInDropDown(vendorSelected, selectedOrderID, out isVendorExist);
                            }
                        }

                        if (isVendorExist)
                        {
                            if (!isEditPO)
                            {
                                CalculateTotals(origVendorID);
                            }
                            else
                            {
                                vendorBeforeSelection = origVendorID;
                                selectedOrderIDFromVendorCombo = selectedOrderID;
                            }
                        }
                        else
                        {
                            this.gridItemDetails.ActiveRow.Cells[clsPOSDBConstants.PODetail_Fld_VendorName].Value = vendorName;
                            this.gridItemDetails.ActiveCell.CancelUpdate();
                            FocusToCell(clsPOSDBConstants.PODetail_Fld_VendorName);
                        }
                        this.gridItemDetails.Refresh();
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "vendorDropDown_RowSelected(object sender, RowSelectedEventArgs e)");
                clsUIHelper.ShowErrorMsg(ex.Message);
            }
        }

        /// <summary>
        /// Checkes if the PODetail is already exist.
        /// </summary>
        /// <param name="selectedPODetailIDFromVendorCombo"></param>
        /// <remarks>Last Modified by Shrikant Mali : Changed the name from PODetailExist to IsPODetailExist</remarks>
        /// <returns></returns>
        private bool IsPODetailExist(string selectedPODetailIDFromVendorCombo)
        {
            bool isPODetailExist = false;

            //Updated By SRT(Ritesh Parekh) Date: 29-Aug-2009
            if (selectedPODetailIDFromVendorCombo.Trim().Length > 0)
            {
                if (!isEditPO)
                {
                    if (orderDetailData != null)
                    {
                        DataRow[] poDeatilRow = orderDetailData.PODetailTable.Select(clsPOSDBConstants.PODetail_Fld_PODetailID + "= " + selectedPODetailIDFromVendorCombo);
                        if (poDeatilRow != null && poDeatilRow.Length > 0)
                            isPODetailExist = true;
                        else
                            isPODetailExist = false;
                    }
                }
                else
                {
                    if (masterOrderDetailsData != null)
                    {
                        DataRow[] poDeatilRow = masterOrderDetailsData.PODetailTable.Select(clsPOSDBConstants.PODetail_Fld_PODetailID + "= " + selectedPODetailIDFromVendorCombo);
                        if (poDeatilRow != null && poDeatilRow.Length > 0)
                            isPODetailExist = true;
                        else
                            isPODetailExist = false;
                    }
                }
            }
            return isPODetailExist;
        }

        private void orderNosDropDown_RowSelected(object sender, RowSelectedEventArgs e)
        {
            string orderNoSelected = string.Empty;
            string orderNo = string.Empty;
            string vendorID = string.Empty;
            string orderIDSelected = string.Empty;
            string orderID = string.Empty;
            string itemID = string.Empty;

            DataRow[] orders = null;
            //Added by SRT(Abhishek)
            string poDetailIDChanged = string.Empty;
            DataRow[] rowForOrdeid = null;
            //End of Added By SRT(Abhishek)
            int rowCount = 0;

            try
            {
                if (e.Row == null) return;
                orderNo = this.gridItemDetails.ActiveRow.Cells[clsPOSDBConstants.OrderDetail_Fld_OrderNo].Value.ToString();
                orderNoSelected = e.Row.Cells["Value"].Value.ToString();
                if (orderNoSelected != orderNo)
                {
                    orderNosDropDown.UpdateData();
                    itemID = this.gridItemDetails.ActiveRow.Cells[clsPOSDBConstants.PODetail_Fld_ItemID].Value.ToString();
                    vendorID = this.gridItemDetails.ActiveRow.Cells[clsPOSDBConstants.POHeader_Fld_VendorID].Value.ToString();
                    rowCount = this.gridPODetails.Rows.Count;
                    for (int count = 0; count < rowCount; count++)
                    {
                        if (vendorID == this.gridPODetails.Rows[count].Cells[clsPOSDBConstants.POHeader_Fld_VendorID].Value.ToString())
                        {
                            this.gridPODetails.ActiveRow = this.gridPODetails.Rows[count];
                            break;
                        }
                    }
                }
                else
                {
                    return;
                }
                orderNoOrderIdDict.TryGetValue(orderNoSelected, out orderIDSelected);
                orderNoOrderIdDict.TryGetValue(orderNo, out orderID);
                rowForOrdeid = masterOrderDetailsData.PODetailTable.Select("OrderID ='" + orderIDSelected + "' AND ItemID ='" + itemID + "'");
                orders = masterOrderDetailsData.PODetailTable.Select("OrderID ='" + orderID + "' AND ItemID ='" + itemID + "'");

                if (rowForOrdeid.Length > 0)
                {
                    if (rowForOrdeid[0][clsPOSDBConstants.PODetail_Fld_ItemID].ToString() == orders[0][clsPOSDBConstants.PODetail_Fld_ItemID].ToString())
                    {
                        rowForOrdeid[0][clsPOSDBConstants.PODetail_Fld_Cost] = Convert.ToDecimal(rowForOrdeid[0][clsPOSDBConstants.PODetail_Fld_Cost].ToString()) + Convert.ToDecimal(orders[0][clsPOSDBConstants.PODetail_Fld_Cost].ToString());
                        rowForOrdeid[0][clsPOSDBConstants.PODetail_Fld_LastCostPrice] = Convert.ToDecimal(rowForOrdeid[0][clsPOSDBConstants.PODetail_Fld_LastCostPrice].ToString()) + Convert.ToDecimal(orders[0][clsPOSDBConstants.PODetail_Fld_LastCostPrice].ToString());
                        rowForOrdeid[0][clsPOSDBConstants.PODetail_Fld_QTY] = Convert.ToInt32(rowForOrdeid[0][clsPOSDBConstants.PODetail_Fld_QTY].ToString()) + Convert.ToInt32(orders[0][clsPOSDBConstants.PODetail_Fld_QTY].ToString());
                    }
                    masterOrderDetailsData.AcceptChanges();
                    poDetailIDChanged = orders[0][clsPOSDBConstants.PODetail_Fld_PODetailID].ToString();
                    foreach (UltraGridRow oRow in gridItemDetails.Rows)
                    {
                        if (oRow.Cells[clsPOSDBConstants.PODetail_Fld_PODetailID].Value.ToString() == poDetailIDChanged)
                        {
                            //gridItemDetails.Rows[oRow.Index].Delete(false);
                            DeleteGridRow(false, this.gridItemDetails.ActiveRow);
                            //Delete POHeader from database 
                            //added by SRT(Abhishek)  29/07/2009                               
                            DataRow[] poDetails = masterOrderDetailsData.PODetailTable.Select("OrderID = '" + orderID + "'");
                            if (poDetails.Length == 0)
                            {
                                oPurchaseOrder.DeletePOHeader(Convert.ToInt32(orderID));
                            }
                            //End of Added by Abhishek(SRT) Date:29/07/09
                        }
                    }
                    this.gridItemDetails.Update();
                    this.gridItemDetails.Refresh();
                }
                orders[0][clsPOSDBConstants.POHeader_Fld_OrderID] = orderIDSelected;
                CalculateTotals(vendorID, orderID);
                CalculateTotals(vendorID, orderIDSelected);
                this.gridPODetails.Update();
            }
            catch (Exception ex)
            {
            }
        }

        private void ForSameItemID(string poDetailId, string itemID)
        {
            DataRow[] row = masterOrderDetailsData.PODetailTable.Select("PODetailID ='" + poDetailId + "'AND ItemID ='" + itemID + "'");
            masterOrderDetailsData.PODetailTable.Rows.Remove(row[0]);
            masterOrderDetailsData.PODetailTable.AcceptChanges();
        }

        private void ulDropDown_RowSelected(object sender, RowSelectedEventArgs e)
        {
            string vendorName = string.Empty;
            string originalVendor = string.Empty;
            String origVendorID = String.Empty;
            DataRow[] orgVendRows = null;
            bool isVendorSelected = false;
            bool isVendorExist = false;
            try
            {
                if (e.Row != null)
                {
                    if (!isEditPO)
                    {
                        vendorName = this.gridItemDetails.ActiveRow.Cells[clsPOSDBConstants.PODetail_Fld_VendorName].Value.ToString();
                        vendorSelected = e.Row.Cells["Value"].Value.ToString();
                        //Updated By SRT(Gaurav) Date: 09-Jul-2009
                        //Updated To solve crash issue regarding empty vendorname.
                        if (vendorName.Trim().Length == 0 || vendorSelected.Trim().Length == 0)
                        {
                            return;
                        }
                        //End Of Updated By SRT(Gaurav)
                        if (vendorSelected != vendorName)
                        {
                            orgVendRows = vendorData.Vendor.Select("VendorCode ='" + vendorName + "'");
                            if (orgVendRows.Length > 0)
                            {
                                origVendorID = orgVendRows[0][clsPOSDBConstants.POHeader_Fld_VendorID].ToString();
                                isVendorSelected = UpdateVendorSelectedInDropDown(vendorSelected, out isVendorExist);
                            }
                            if (isVendorExist)
                            {
                                CalculateTotals(origVendorID);
                            }
                            else
                            {
                                this.gridItemDetails.ActiveRow.Cells[clsPOSDBConstants.PODetail_Fld_VendorName].Value = vendorName;
                            }
                            this.gridItemDetails.Refresh();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "ulDropDown_RowSelected(object sender, RowSelectedEventArgs e)");
                clsUIHelper.ShowErrorMsg(ex.Message);
            }
        }

        private bool UpdateVendorSelectedInDropDown(string vendorSelected, out bool isVendorExist)
        {
            string vendorId = string.Empty;
            string orgVendorId = string.Empty;
            string itemCode = string.Empty;
            string vendorItemCode = string.Empty;
            string vendorCostPrice = string.Empty;
            string whereClause = string.Empty;
            bool tempVendorExist = false;
            ItemVendor itemVendor = null;
            ItemVendorData itemVendorData = null;
            DataRow[] vendRows = null;

            bool isVendorSelected = false;
            try
            {
                if (vendorSelected != string.Empty)
                {

                    itemCode = this.gridItemDetails.ActiveRow.Cells[clsPOSDBConstants.PODetail_Fld_ItemID].Value.ToString();

                    if (itemCode == DBNull.Value.ToString())
                    {
                        isVendorExist = false;
                        return false;
                    }
                    itemVendor = new ItemVendor();

                    vendRows = vendorData.Vendor.Select("VendorCode ='" + vendorSelected + "'");
                    vendorId = vendRows[0][clsPOSDBConstants.POHeader_Fld_VendorID].ToString();

                    whereClause = " AND " + clsPOSDBConstants.ItemVendor_tbl + ".ItemID = '" + itemCode + "' AND " + clsPOSDBConstants.ItemVendor_tbl + ".VendorID = '" + vendorId + "'";
                    itemVendorData = itemVendor.PopulateList(whereClause);
                    if (itemVendorData.ItemVendor.Rows.Count > 0)
                    {
                        vendorItemCode = itemVendorData.ItemVendor.Rows[0]["VendorItemID"].ToString();
                        vendorCostPrice = itemVendorData.ItemVendor.Rows[0]["VendorCostPrice"].ToString();

                        if (!isEditPO)
                        {
                            InitalizePODetails(vendorId);
                        }
                        else
                        {
                            //Get max PO ID from database or datasource bind to grid
                            //Then call the method below.
                            //InitalizeMasterOrderDetails(vendorId,
                            //Implement flow to populate the items in Grid PO details.
                            foreach (MasterOrderDetailsRow oMRow in masterOrderDetailsData.MasterOrderDetailTable)
                            {

                            }
                        }
                        this.gridItemDetails.ActiveRow.Cells[clsPOSDBConstants.POHeader_Fld_VendorID].Value = vendorId;
                        this.gridItemDetails.ActiveRow.Cells[clsPOSDBConstants.PODetail_Fld_VendorItemCode].Value = vendorItemCode;
                        this.gridItemDetails.ActiveRow.Cells[clsPOSDBConstants.PODetail_Fld_Price].Value = vendorCostPrice;
                        this.gridItemDetails.Refresh();
                        //Add by ravindra for JIra PRIMEPOS-582 Remove red highlight when best vendor is selected in PO
                        VendorData vendorDataTemp = new VendorData();
                        VendorSvr vendorSvrTemp = new VendorSvr();
                        vendorDataTemp = vendorSvrTemp.PopulateList(" Where VendorName ='" + this.gridItemDetails.ActiveRow.Cells["Best Vendor"].Text + "'");
                        string VSelectedCode = vendorDataTemp.Tables[0].Rows[0][clsPOSDBConstants.Vendor_Fld_VendorCode].ToString();

                        if (VSelectedCode != this.gridItemDetails.ActiveRow.Cells["VendorName"].Text)
                        {
                            this.gridItemDetails.ActiveRow.Cells["Best Vendor"].Appearance.BackColorDisabled = Color.Red;//Change by Ravindra from ForeColor to BackColorDisabled
                            this.gridItemDetails.ActiveRow.Cells["Best Vend.Price"].Appearance.BackColorDisabled = Color.Red;
                        }
                        else
                        {
                            this.gridItemDetails.ActiveRow.Cells["Best Vendor"].Appearance.BackColorDisabled = Color.White;
                            this.gridItemDetails.ActiveRow.Cells["Best Vend.Price"].Appearance.BackColorDisabled = Color.White;
                        }

                        this.gridItemDetails.ActiveRow.Update();
                        CalculateTotals(vendorId);
                        tempVendorExist = true;
                    }
                    else
                    {
                        tempVendorExist = false;
                        //this.gridItemDetails.ActiveRow.Cells[clsPOSDBConstants.PODetail_Fld_VendorName].DroppedDown = false;
                        string message = "Item# " + itemCode + " is not available for " + vendorSelected + ".\nDo you want to Add New Record?";

                        //clsUIHelper.ShowErrorMsg(message);

                        clsUIHelper.ShowInfoMsg(message);
                        if (clsUIHelper.IsOK)
                        {
                            frmAddNewItem addItems = new frmAddNewItem();
                            addItems.FillItemDescription(itemCode, this.gridItemDetails.ActiveRow.Cells["Description"].Text.ToString(),
                                vendorSelected);
                            addItems.ShowDialog();
                            addItems.BringToFront();
                            clsUIHelper.IsOK = false;
                            //Added by SRT(Gaurav) Date: 10-Jul-2009
                            if (!addItems.IsCanceled)
                            {
                                //Updated By SRT(Gaurav) Date: 11-Jul-2009
                                //This is to get the exact data based on search criteria on updating the data in Add New Item Flow.
                                UpdateVendorSelectedInDropDown(vendorSelected, out isVendorExist);

                            }
                            //End Of Added By SRT(Gaurav)
                        }
                    }
                    isVendorSelected = true;
                }
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "UpdateVendorSelectedInDropDown(string vendorSelected, out bool isVendorExist)");
                clsUIHelper.ShowErrorMsg(ex.ToString());
            }
            isVendorExist = tempVendorExist;
            return isVendorSelected;
        }

        //Added By Gaurav
        private bool UpdateVendorSelectedInDropDown(string vendorSelected, string POID, out bool isVendorExist)
        {
            string VSelectedCode = "";
            string vendorId = string.Empty;
            string orgVendorId = string.Empty;
            string itemCode = string.Empty;
            string vendorItemCode = string.Empty;
            string vendorCostPrice = string.Empty;
            string whereClause = string.Empty;
            bool tempVendorExist = false;
            ItemVendor itemVendor = null;
            ItemVendorData itemVendorData = null;
            DataRow[] vendRows = null;

            bool isVendorSelected = false;
            try
            {
                if (vendorSelected != string.Empty)
                {

                    itemCode = this.gridItemDetails.ActiveRow.Cells[clsPOSDBConstants.PODetail_Fld_ItemID].Value.ToString();

                    if (itemCode == DBNull.Value.ToString())
                    {
                        isVendorExist = false;
                        return false;
                    }
                    itemVendor = new ItemVendor();

                    vendRows = vendorData.Vendor.Select("VendorCode ='" + vendorSelected.Trim().Replace("'", "''") + "'");    //PRIMEPOS-3104 23-Jun-2022 JY modified
                    if (vendRows.Length > 0)
                    {
                        vendorId = vendRows[0][clsPOSDBConstants.POHeader_Fld_VendorID].ToString();
                    }
                    else
                    {
                        clsUIHelper.ShowErrorMsg(" Vendor Selected Does Not Exist");
                        isVendorExist = false;
                        return false;
                    }

                    whereClause = " AND " + clsPOSDBConstants.ItemVendor_tbl + ".ItemID = '" + itemCode + "' AND " + clsPOSDBConstants.ItemVendor_tbl + ".VendorID = '" + vendorId + "'";
                    itemVendorData = itemVendor.PopulateList(whereClause);
                    if (itemVendorData.ItemVendor.Rows.Count > 0)
                    {
                        vendorItemCode = itemVendorData.ItemVendor.Rows[0]["VendorItemID"].ToString();
                        vendorCostPrice = itemVendorData.ItemVendor.Rows[0]["VendorCostPrice"].ToString();
                        this.gridItemDetails.ActiveRow.Cells[clsPOSDBConstants.POHeader_Fld_VendorID].Value = vendorId;
                        if (!isEditPO)
                        {
                            InitalizePODetails(vendorId);
                            CalculateTotals(vendorId);
                        }
                        else
                        {
                            isItemMovedToVendorSelected = true;
                            CalculateTotals(vendorId, POID);
                        }

                        if (this.gridItemDetails.ActiveRow != null)
                        {
                            this.gridItemDetails.ActiveRow.Cells[clsPOSDBConstants.PODetail_Fld_VendorItemCode].Value = vendorItemCode;
                            this.gridItemDetails.ActiveRow.Cells[clsPOSDBConstants.PODetail_Fld_Price].Value = vendorCostPrice;
                            this.gridItemDetails.Refresh();
                            VendorData vendorData1 = new VendorData();
                            VendorSvr vendorSvr = new VendorSvr();
                            vendorData1 = vendorSvr.PopulateList(" Where VendorName ='" + this.gridItemDetails.ActiveRow.Cells["Best Vendor"].Text + "'");
                            string VSelected = vendorData1.Tables[0].Rows[0][clsPOSDBConstants.Vendor_Fld_VendorCode].ToString();
                            if (VSelected != this.gridItemDetails.ActiveRow.Cells["VendorName"].Text)
                            {
                                this.gridItemDetails.ActiveRow.Cells["Best Vendor"].Appearance.BackColorDisabled = Color.Red;//change by Ravindra from  forrecolor to BackColorDisabled 
                                this.gridItemDetails.ActiveRow.Cells["Best Vend.Price"].Appearance.BackColorDisabled = Color.Red;
                            }
                            else
                            {
                                this.gridItemDetails.ActiveRow.Cells["Best Vendor"].Appearance.BackColorDisabled = Color.White;
                                this.gridItemDetails.ActiveRow.Cells["Best Vend.Price"].Appearance.BackColorDisabled = Color.White;
                            }
                        }
                        //this.gridItemDetails.ActiveRow.Update(); 
                        tempVendorExist = true;
                    }
                    else
                    {
                        tempVendorExist = false;

                        //this.gridItemDetails.ActiveRow.Cells[clsPOSDBConstants.PODetail_Fld_VendorName].DroppedDown = false;
                        string message = "Item# " + itemCode + " is not available for " + vendorSelected + ".\nDo you want to Add New Record?";

                        //clsUIHelper.ShowErrorMsg(message);

                        clsUIHelper.ShowInfoMsg(message);
                        if (clsUIHelper.IsOK)
                        {
                            frmAddNewItem addItems = new frmAddNewItem();
                            addItems.FillItemDescription(itemCode, this.gridItemDetails.ActiveRow.Cells["Description"].Text.ToString(),
                                vendorSelected);
                            addItems.ShowDialog();
                            addItems.BringToFront();
                            clsUIHelper.IsOK = false;
                            //Added by SRT(Gaurav) Date: 10-Jul-2009
                            if (!addItems.IsCanceled)
                            {
                                //Updated By SRT(Gaurav) Date: 11-Jul-2009
                                //This is to get the exact data based on search criteria on updating the data in Add New Item Flow.
                                UpdateVendorSelectedInDropDown(vendorSelected, POID, out isVendorExist);
                                if (isVendorExist)
                                {
                                    tempVendorExist = true;
                                }
                            }
                            //End Of Added By SRT(Gaurav)
                        }
                    }
                    //Add by Ravindra previouslly it compai vondor code with Vendor name
                    VendorData vendorDataTemp = new VendorData();
                    VendorSvr vendorSvrTemp = new VendorSvr();
                    vendorDataTemp = vendorSvrTemp.PopulateList(" Where VendorName ='" + this.gridItemDetails.ActiveRow.Cells["Best Vendor"].Text + "'");
                    VSelectedCode = vendorDataTemp.Tables[0].Rows[0][clsPOSDBConstants.Vendor_Fld_VendorCode].ToString();

                    if (VSelectedCode != this.gridItemDetails.ActiveRow.Cells["VendorName"].Text)
                    {
                        this.gridItemDetails.ActiveRow.Cells["Best Vendor"].Appearance.BackColorDisabled = Color.Red;//change by Ravindra from Forecolor to BackColorDisabled
                        this.gridItemDetails.ActiveRow.Cells["Best Vend.Price"].Appearance.BackColorDisabled = Color.Red;
                    }
                    else
                    {
                        this.gridItemDetails.ActiveRow.Cells["Best Vendor"].Appearance.BackColorDisabled = Color.White;
                        this.gridItemDetails.ActiveRow.Cells["Best Vend.Price"].Appearance.BackColorDisabled = Color.White;
                    }
                    isVendorSelected = true;
                }
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "UpdateVendorSelectedInDropDown(string vendorSelected, string POID, out bool isVendorExist)");
                clsUIHelper.ShowErrorMsg(ex.ToString());
            }
            isVendorExist = tempVendorExist;
            return isVendorSelected;
        }

        private void MakeRelation()
        {
            try
            {
                DataRelation dataRelation = orderDetailData.Relations.Add(clsPOSDBConstants.POItemRelationName, orderDetailData.OrderDetailsTable.VendorID, orderDetailData.PODetailTable.VendorID);
            }
            catch (Exception ex)
            {
                clsUIHelper.ShowErrorMsg(ex.ToString());
            }
        }

        private void BindRelations()
        {
            DataRelation dataRelation = null;
            try
            {
                dataRelation = masterOrderDetailsData.Relations.Add(clsPOSDBConstants.MasterPOItemRelationName, masterOrderDetailsData.MasterOrderDetailTable.VendorID, masterOrderDetailsData.OrderDetailsTable.VendorID);
                dataRelation = masterOrderDetailsData.Relations.Add(clsPOSDBConstants.POItemRelationName, masterOrderDetailsData.OrderDetailsTable.OrderID, masterOrderDetailsData.PODetailTable.OrderID);
            }
            catch (Exception ex)
            {
                clsUIHelper.ShowErrorMsg(ex.ToString());
            }
        }

        private void FillVendors()
        {
            POS_Core.BusinessRules.Vendor vendor = null;
            try
            {
                vendor = new POS_Core.BusinessRules.Vendor();
                vendorData = vendor.PopulateList("");
                if (vendorDetails.Count <= 0 && vendorList.Count <= 0)
                {
                    //Added by Abhishek(SRT) -- 12 Aug 2009
                    //Added to sort the data by vendor 
                    DataRow[] vendorRows = vendorData.Tables[0].Select("", "VendorCode ASC");
                    //End of Added by Abhishek(SRT) 

                    foreach (DataRow vendorRow in vendorRows)
                    {
                        this.vendorDetails.Add(vendorRow["VendorID"].ToString(), vendorRow["VendorCode"].ToString());
                        vendorList.Add(vendorRow["VendorCode"].ToString());
                    }
                    vendorDropDown.DataSource = vendorList;
                }
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "FillVendors()");
                clsUIHelper.ShowErrorMsg(ex.ToString());
            }
        }

        private void btnAddNewItem_Click(object sender, EventArgs e)
        {
            UltraGridCell oCurrentCell;
            AskForOrderedItem = false;
            string errorMsg = string.Empty;
            int podId = 0;
            bool isFirstTime = true;

            GetOrderDescription = false;
            isShowOrderedItems = false;
            this.gridItemDetails.AfterCellUpdate -= new Infragistics.Win.UltraWinGrid.CellEventHandler(this.gridItemDetails_AfterCellUpdate);   //PRIMEPOS-3155 06-Oct-2022 JY Added
            try
            {
                AddRow();

                oCurrentCell = this.gridItemDetails.ActiveCell;
                frmShowOrderedItems showOrderedItems = new frmShowOrderedItems();
                if (!showOrderedItems.Edit())
                {
                    clsUIHelper.ShowErrorMsg("There are no order items in last 30 days.");
                }

                showOrderedItems.ShowDialog();
                showOrderedItems.BringToFront();
                if (!showOrderedItems.IsCanceled)
                {
                    PODetailData SelectedItems = new PODetailData();
                    SelectedItems = showOrderedItems.SelectedItems;

                    #region
                    logger.Trace("btnAddNewItem_Click() - Start loop: " + DateTime.Now);
                    isAutoPO = true;
                    #endregion

                    foreach (PODetailRow pdr in SelectedItems.PODetail.Rows)
                    {
                        //Following if-else is added by shitaljit on 1/16/2014 
                        //For JIRA 1719, 1720
                        pdr.QTY = (pdr.QTY == 0) ? 1 : pdr.QTY;
                        DataRow[] porows = null;
                        if (isEditPO == false && Configuration.isNullOrEmptyDataSet(orderDetailData) == false)
                        {
                            porows = orderDetailData.PODetailTable.Select("ItemID='" + pdr.ItemID + "'");
                        }
                        else if (Configuration.isNullOrEmptyDataSet(masterOrderDetailsData) == false)
                        {
                            porows = masterOrderDetailsData.PODetailTable.Select("ItemID='" + pdr.ItemID + "'");
                        }
                        if (porows == null || porows.Length == 0)
                        {
                            if (gridItemDetails.ActiveRow == null || !isFirstTime)
                            {
                                gridItemDetails.Rows.Band.AddNew();
                            }
                            podId = GetMaxPODID();
                            //Following if is added by shitaljit on 1/16/2014 
                            //For JIRA 1719, 1720
                            if (isEditPO == false)
                            {
                                InitalizePODetails(pdr.VendorID.ToString());
                            }
                            gridItemDetails.ActiveRow.Cells[clsPOSDBConstants.PODetail_Fld_PODetailID].Value = podId;
                            gridItemDetails.ActiveRow.Cells["ItemId"].Value = pdr.ItemID;
                            gridItemDetails.ActiveRow.Cells["Description"].Value = pdr.ItemDescription;
                            gridItemDetails.ActiveRow.Cells["Price"].Value = pdr.Price;
                            gridItemDetails.ActiveRow.Cells["Qty"].Value = pdr.QTY;
                            gridItemDetails.ActiveRow.Cells["cost"].Value = pdr.Price * pdr.QTY;
                            gridItemDetails.ActiveRow.Cells["comments"].Value = pdr.Comments;
                            gridItemDetails.ActiveRow.Cells[clsPOSDBConstants.PODetail_Fld_VendorName].Value = pdr.VendorName;

                            gridItemDetails.ActiveRow.Cells["VendorItemCode"].Value = pdr.VendorItemCode; //vend. Itm Code
                            gridItemDetails.ActiveRow.Cells["Best Price"].Value = pdr.BestPrice;
                            gridItemDetails.ActiveRow.Cells["Best Vendor"].Value = pdr.BestVendor;
                            gridItemDetails.ActiveRow.Cells["Last Vendor"].Value = string.Empty;
                            gridItemDetails.ActiveRow.Cells["VendorID"].Value = pdr.VendorID;
                            String tempQty = string.Empty;
                            String itemQty = String.Empty;
                            String itemPrice = String.Empty;

                            Application.DoEvents();
                            haveVendorInfo = true;
                            FindBestVendorANDPrice(pdr.ItemID);
                            isShowOrderedItems = true;
                            gridItemDetails.Refresh();
                            gridItemDetails.ActiveRow.Update();
                            if (isEditPO == false && Configuration.isNullOrEmptyDataSet(orderDetailData) == false)
                            {
                                orderDetailData.AcceptChanges();
                            }
                            else if (Configuration.isNullOrEmptyDataSet(masterOrderDetailsData) == false)
                            {
                                masterOrderDetailsData.AcceptChanges();
                            }
                            isFirstTime = false;
                        }
                        else
                        {
                            //Fix spelling mistake JIRA ticket# 1718
                            clsUIHelper.ShowInfoMsg(" Item - " + pdr.ItemID + " is Already In Order List For Vendor - " +
                                                    porows[0][clsPOSDBConstants.Vendor_Fld_VendorName].ToString() +
                                                    "\n Do You Want To Add The Item For Vendor - " + pdr.VendorName);
                            if (clsUIHelper.IsOK)
                            {
                                foreach (UltraGridRow oRow in gridItemDetails.Rows)
                                {
                                    if (oRow.Cells[clsPOSDBConstants.PODetail_Fld_ItemID].Value.ToString() == pdr.ItemID)
                                    {
                                        oRow.Activate();
                                        oRow.Selected = true;
                                        DeleteGridRow(false, this.gridItemDetails.ActiveRow);
                                        this.gridItemDetails.Update();
                                        this.gridItemDetails.Refresh();
                                        break;
                                    }
                                }
                                this.gridItemDetails.Update();
                                this.gridItemDetails.Refresh();

                                if (gridItemDetails.ActiveRow == null || !isFirstTime)
                                {
                                    try
                                    {
                                        gridItemDetails.Rows.Band.AddNew();
                                    }
                                    catch { }
                                }
                                podId = GetMaxPODID();
                                //Following if is added by shitaljit on 1/16/2014 
                                //For JIRA 1719, 1720
                                if (isEditPO == false)
                                {
                                    InitalizePODetails(pdr.VendorID.ToString());
                                }
                                gridItemDetails.ActiveRow.Cells[clsPOSDBConstants.PODetail_Fld_PODetailID].Value = podId;
                                gridItemDetails.ActiveRow.Cells["ItemId"].Value = pdr.ItemID;
                                gridItemDetails.ActiveRow.Cells["Description"].Value = pdr.ItemDescription;
                                gridItemDetails.ActiveRow.Cells["Price"].Value = pdr.Price;
                                gridItemDetails.ActiveRow.Cells["Qty"].Value = pdr.QTY;
                                gridItemDetails.ActiveRow.Cells["cost"].Value = pdr.Price * pdr.QTY;
                                gridItemDetails.ActiveRow.Cells["comments"].Value = pdr.Comments;
                                gridItemDetails.ActiveRow.Cells[clsPOSDBConstants.PODetail_Fld_VendorName].Value = pdr.VendorName;

                                gridItemDetails.ActiveRow.Cells["VendorItemCode"].Value = pdr.VendorItemCode; //vend. Itm Code
                                gridItemDetails.ActiveRow.Cells["Best Price"].Value = pdr.BestPrice;
                                gridItemDetails.ActiveRow.Cells["Best Vendor"].Value = pdr.BestVendor;
                                gridItemDetails.ActiveRow.Cells["Last Vendor"].Value = string.Empty;
                                gridItemDetails.ActiveRow.Cells["VendorID"].Value = pdr.VendorID;
                                haveVendorInfo = true;
                                Application.DoEvents();
                                FindBestVendorANDPrice(pdr.ItemID);
                                isShowOrderedItems = true;
                                gridItemDetails.Refresh();
                                gridItemDetails.ActiveRow.Update();
                                if (isEditPO == false && Configuration.isNullOrEmptyDataSet(orderDetailData) == false)
                                {
                                    orderDetailData.AcceptChanges();
                                }
                                else if (Configuration.isNullOrEmptyDataSet(masterOrderDetailsData) == false)
                                {
                                    masterOrderDetailsData.AcceptChanges();
                                }
                                isFirstTime = false;
                            }
                        }
                    }

                    #region PRIMEPOS-3155 06-Oct-2022 JY Added
                    isAutoPO = false;
                    TotalCostForAllPO();
                    int csItemCount = CSItemCount;
                    txtEditorNoOfCSItems.Text = csItemCount.ToString();
                    tbxNonCaseItems.Text = GetNonCaseItemCount(csItemCount).ToString();
                    FormatGrid("ADD");
                    foreach (OrderDetailRow orderDetailRow in orderDetailData.OrderDetailsTable.Rows)
                    {
                        CalculateTotals(orderDetailRow.VendorID.ToString());
                    }
                    logger.Trace("btnAddNewItem_Click() - End loop: " + DateTime.Now);
                    #endregion

                    Application.DoEvents();
                    gridItemDetails.Refresh();
                    gridItemDetails.UpdateData();
                    gridPODetails.Refresh();
                    gridPODetails.UpdateData();
                    if (SelectedItems.PODetail.Rows.Count > 0)
                    {
                        FocusNewItemId();
                    }
                }
                //Added By sRT(Ritesh Parekh) Date : 19-Aug-2009
                //Focused new item id if any one cancels the form.
                else
                {
                    FocusNewItemId();
                }
                //End Of Added By SRT(Ritesh Parekh)
                isShowOrderedItems = false;
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "btnAddNewItem_Click(object sender, EventArgs e)");
                clsUIHelper.ShowErrorMsg(ex.Message);
            }
            finally
            {
                this.gridItemDetails.AfterCellUpdate += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.gridItemDetails_AfterCellUpdate);   //PRIMEPOS-3155 06-Oct-2022 JY Added
            }
            AskForOrderedItem = true;
            GetOrderDescription = true;
        }

        private void AddRow()
        {
            //int countOfRowsinGrid = 0;
            try
            {
                this.gridItemDetails.Focus();

                this.gridItemDetails.ActiveCell = this.gridItemDetails.Rows[this.gridItemDetails.ActiveRow.Index].Cells[clsPOSDBConstants.PODetail_Fld_ItemID];
                this.gridItemDetails.PerformAction(UltraGridAction.FirstCellInRow);
                this.gridItemDetails.PerformAction(UltraGridAction.LastCellInGrid);
                this.gridItemDetails.PerformAction(UltraGridAction.EnterEditMode);
                this.gridItemDetails.ActiveRow.Cells[clsPOSDBConstants.PODetail_Fld_ItemID].Activate();
                this.gridItemDetails.Refresh();
            }
            catch (Exception)
            {
            }
        }

        private void AddRowForPODetails()
        {
            try
            {
                this.gridPODetails.Focus();
                this.gridPODetails.Rows.Band.Columns[0].CellActivation = Activation.AllowEdit;
                this.gridPODetails.PerformAction(UltraGridAction.FirstCellInRow);
                this.gridPODetails.PerformAction(UltraGridAction.LastCellInGrid);
                this.gridPODetails.PerformAction(UltraGridAction.EnterEditMode);
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "AddRowForPODetails()");
                clsUIHelper.ShowErrorMsg(ex.Message);
            }
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            isCanceled = true;
            //poDetailId = 0;
            bool closeWithSave = false;

            try
            {
                if (this.gridPODetails.Rows.Count == 0)
                {
                    this.Close();
                    showOrderMsgBox = null;
                    return;
                }
                //Moved By SRT(Ritesh Parekh) Date : 27-Aug-2009
                //To handel null active row and avoid object referance issue.
                if (this.gridItemDetails.ActiveRow == null)
                {
                    return;
                }
                //End Of Moved By SRT(Ritesh Parekh)

                //Added by SRT(Abhishek) Date : 26 Aug 2009
                string selectedPODetailIDFromVendorCombo = this.gridItemDetails.ActiveRow.Cells[clsPOSDBConstants.PODetail_Fld_PODetailID].Value.ToString();
                if (selectedPODetailIDFromVendorCombo == string.Empty || !IsPODetailExist(selectedPODetailIDFromVendorCombo))
                {
                    if (this.gridItemDetails.Rows.Count > 0)
                    {
                        //this.gridItemDetails.ActiveRow.Delete(false);
                        string itemID = this.gridItemDetails.ActiveRow.Cells[clsPOSDBConstants.Item_Fld_ItemID].Value.ToString();
                        if (itemID != string.Empty)
                            DeleteGridRow(false, this.gridItemDetails.ActiveRow);
                        if (this.gridItemDetails.Rows.Count > 0)
                            FocusNewItemId();
                        if (this.gridPODetails.Rows.Count == 0)
                        {
                            this.Close();
                            showOrderMsgBox = null;
                            return;
                        }
                    }
                }
                //End of Added by SRT(Abhishek) Date : 26 Aug 2009             

                if (this.gridItemDetails.ActiveRow.Index > 0)
                {
                    this.gridItemDetails.Rows[0].Activate();
                }
                showOrderMsgBox = new frmShowMsgBox();
                showOrderMsgBox.TopMost = true;
                showOrderMsgBox.ShowDialog();
                showOrderMsgBox.BringToFront();
                if (showOrderMsgBox.SaveOrCloseOrder == clsPOSDBConstants.PO_Save)
                {
                    closeWithSave = false;
                }
                else if (showOrderMsgBox.SaveOrCloseOrder == clsPOSDBConstants.PO_SaveClose)
                {
                    closeWithSave = true;
                }
                else if (showOrderMsgBox.SaveOrCloseOrder == clsPOSDBConstants.PO_Cancel)
                {
                    return;
                }
                else if (showOrderMsgBox.SaveOrCloseOrder == clsPOSDBConstants.PO_DiscardChanges)
                {
                    this.Close();
                    return;
                }
                if (SendAllOrders(closeWithSave))
                {
                    this.Close();
                }
                newlyAddedPOIDs.Clear();
                newlyAddedOrderIDs.Clear();
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "btnClose_Click(object sender, EventArgs e)");
                clsUIHelper.ShowErrorMsg(ex.Message);
            }
        }
        private void ultraBtnItemHistory_Click(object sender, EventArgs e)
        {
            string upcCode = string.Empty;
            try
            {
                if (gridItemDetails.ActiveRow != null)
                {
                    upcCode = gridItemDetails.ActiveRow.Cells["ItemID"].Value.ToString();
                    if (upcCode != "")
                    {
                        frmViewHistory viewHistory = new frmViewHistory(upcCode);
                        if (viewHistory.CanShowItemHistory)
                        {
                            viewHistory.ShowDialog();
                            viewHistory.BringToFront();
                        }
                        else
                        {
                            clsUIHelper.ShowErrorMsg("Please select valid Item First.");
                            //Added By SRT(Ritesh Parekh) Date : 19-Aug-2009
                            FocusNewItemId();
                            //End Of Added By SRT(Ritesh Parekh)
                        }
                    }
                    else
                    {
                        clsUIHelper.ShowErrorMsg(" Please Select Item First");
                        //Added By SRT(Ritesh Parekh) Date : 19-Aug-2009
                        FocusNewItemId();
                        //End Of Added By SRT(Ritesh Parekh)
                    }
                }
                else
                {
                    clsUIHelper.ShowErrorMsg(" Please Select Item First");
                    //Added By SRT(Ritesh Parekh) Date : 19-Aug-2009
                    FocusNewItemId();
                    //End Of Added By SRT(Ritesh Parekh)
                }
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "ultraBtnItemHistory_Click(object sender, EventArgs e)");
                clsUIHelper.ShowErrorMsg(ex.Message);
                //Added By SRT(Ritesh Parekh) Date : 19-Aug-2009
                FocusNewItemId();
                //End Of Added By SRT(Ritesh Parekh)
            }
        }
        public void Initialize()
        {
            orderDetailData = new OrderDetailsData();
            this.FillVendors();
            SetNew();
        }
        private void SetNew()
        {
            masterOrderDetailsData = new MasterOrderDetailsData();
            oPOHeaderData = new POHeaderData();
            oPOHeaderRow = oPOHeaderData.POHeader.AddRow(0, "", System.DateTime.MinValue, System.DateTime.MinValue, 0, 0);
            oPODetailData = new PODetailData();
            this.gridPODetails.DataSource = orderDetailData.OrderDetailsTable;
            this.gridPODetails.Refresh();
            this.gridItemDetails.DataSource = orderDetailData.PODetailTable;
            this.gridItemDetails.Refresh();
            this.gridItemDetails.Rows.Band.AddNew();
            AddRow();
            orderNumber = Convert.ToInt32(oPurchaseOrder.GetNextPONumber());
            ApplyGrigFormat();
        }
        private void EditItem()
        {
            String errorMsg = String.Empty;
            try
            {
                if (this.gridItemDetails.ActiveRow == null)
                {
                    errorMsg = "Please select the item";
                    clsUIHelper.ShowErrorMsg(errorMsg);
                    //Added By SRT(Ritesh Parekh) Date : 19-Aug-2009
                    FocusNewItemId();
                    //End Of Added By SRT(Ritesh Parekh)
                    return;
                }
                if (this.gridItemDetails.ActiveRow.Cells[clsPOSDBConstants.PODetail_Fld_ItemID].Value.ToString().Trim() == "") return;
                String ItemID = this.gridItemDetails.ActiveRow.Cells[clsPOSDBConstants.PODetail_Fld_ItemID].Value.ToString().Trim();

                frmItems oItems = new frmItems();
                if (!UserPriviliges.IsUserHasPriviliges(UserPriviliges.Modules.InventoryMgmt.ID, UserPriviliges.Screens.PurchaseOrder.ID, -998))
                {
                    oItems.AllowEdit = false;
                }
                else
                {
                    oItems.AllowEdit = true;
                }
                //Updated By SRT(Ritesh Parekh) Date: 19-Aug-2009
                //Updated to avoid no data row at position 0 issue.
                bool callResult = false;
                oItems.Edit(ItemID, out callResult);
                if (callResult == true)
                {
                    oItems.ShowDialog(this);
                }
                //End Of Updated By SRT(Ritesh Parekh)
            }
            catch (Exception exp)
            {
                logger.Fatal(exp, "EditItem()");
                clsUIHelper.ShowErrorMsg(exp.Message);
                //Added By SRT(Ritesh Parekh) Date : 19-Aug-2009
                FocusNewItemId();
                //End Of Added By SRT(Ritesh Parekh)
            }
        }
        private void ApplyGridFormatForEdit()
        {
            //gridItemDetails
            //this.gridItemDetails.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.PODetail_Fld_VendorName].CellActivation = Activation.Disabled;
            //this.btnAutoGenPO.Enabled = false;

            this.gridItemDetails.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.OrderDetail_Fld_OrderNo].Hidden = false;
            this.gridItemDetails.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.OrderDetail_Fld_OrderNo].Header.VisiblePosition = 7;
            this.gridItemDetails.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.OrderDetail_Fld_OrderNo].MaxLength = 20;
            this.gridItemDetails.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.OrderDetail_Fld_OrderNo].Width = 100;

            this.gridItemDetails.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.POHeader_Fld_VendorID].Hidden = true;
            this.gridItemDetails.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.OrderDetail_Tbl_OrderId].Hidden = true;

            this.gridPODetails.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.MasterOrderDetails_Fld_TotalPOs].Header.Caption = "Total PO";
            this.gridPODetails.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.MasterOrderDetails_Fld_TotalPOs].Header.VisiblePosition = 3;
            this.gridPODetails.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.MasterOrderDetails_Fld_TotalPOs].MaxLength = 10;
            this.gridPODetails.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.MasterOrderDetails_Fld_TotalPOs].CellAppearance.TextHAlign = HAlign.Right;
            this.gridPODetails.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.MasterOrderDetails_Fld_TotalPOs].Width = 10;

            this.gridPODetails.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.MasterOrderDetails_Fld_VendorName].Header.Caption = "Vendor Name";
            this.gridPODetails.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.MasterOrderDetails_Fld_VendorName].CellAppearance.TextHAlign = HAlign.Left;
            this.gridPODetails.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.MasterOrderDetails_Fld_VendorName].MaxLength = 40;
            this.gridPODetails.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.MasterOrderDetails_Fld_VendorName].Width = 40;

            this.gridPODetails.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.POHeader_Fld_Description].Hidden = true;
            this.gridPODetails.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.OrderDetail_Fld_TotalCost].Hidden = true;
            this.gridPODetails.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.OrderDetail_Fld_TotalItems].Hidden = true;
            this.gridPODetails.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.OrderDetail_Fld_TotalQty].Hidden = true;
            this.gridPODetails.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.OrderDetail_Fld_OrderNo].Hidden = true;
            this.gridPODetails.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.OrderDetail_Fld_VendorName].Hidden = true;

            this.gridPODetails.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.OrderDetail_Fld_TimeToOrder].Hidden = true;

            this.gridPODetails.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.OrderDetail_Tbl_TimeToOrder].Header.Caption = "Time To Order";
            this.gridPODetails.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.OrderDetail_Tbl_TimeToOrder].MaxLength = 10;
            this.gridPODetails.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.OrderDetail_Tbl_TimeToOrder].CellAppearance.TextHAlign = HAlign.Right;
            this.gridPODetails.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.OrderDetail_Fld_AuroSend].MaxLength = 5;

            this.gridPODetails.DisplayLayout.Bands[1].Columns[clsPOSDBConstants.Item_Fld_Description].Header.Caption = "Order Reference";
            this.gridPODetails.DisplayLayout.Bands[1].Columns[clsPOSDBConstants.OrderDetail_Fld_TimeToOrder].Hidden = true;
            this.gridPODetails.DisplayLayout.Bands[1].Columns[clsPOSDBConstants.OrderDetail_Fld_TimeToOrder].Header.VisiblePosition = 10;

            this.gridPODetails.DisplayLayout.Bands[1].Columns[clsPOSDBConstants.OrderDetail_Fld_TotalCost].MaxLength = 10;
            this.gridPODetails.DisplayLayout.Bands[1].Columns[clsPOSDBConstants.OrderDetail_Fld_TotalCost].CellAppearance.TextHAlign = HAlign.Right;
            this.gridPODetails.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.OrderDetail_Fld_TotalCost].Format = "####.00";
            this.gridPODetails.DisplayLayout.Bands[1].Columns[clsPOSDBConstants.OrderDetail_Fld_TotalItems].MaxLength = 6;
            this.gridPODetails.DisplayLayout.Bands[1].Columns[clsPOSDBConstants.OrderDetail_Fld_TotalItems].Width = 70;
            this.gridPODetails.DisplayLayout.Bands[1].Columns[clsPOSDBConstants.OrderDetail_Fld_TotalItems].CellAppearance.TextHAlign = HAlign.Right;
            this.gridPODetails.DisplayLayout.Bands[1].Columns[clsPOSDBConstants.OrderDetail_Fld_TotalQty].MaxLength = 5;
            this.gridPODetails.DisplayLayout.Bands[1].Columns[clsPOSDBConstants.OrderDetail_Fld_TotalQty].Width = 70;
            this.gridPODetails.DisplayLayout.Bands[1].Columns[clsPOSDBConstants.OrderDetail_Fld_TotalQty].CellAppearance.TextHAlign = HAlign.Right;
            this.gridPODetails.DisplayLayout.Bands[1].Columns[clsPOSDBConstants.OrderDetail_Fld_AuroSend].Hidden = true;

            this.gridPODetails.DisplayLayout.Bands[1].Columns[clsPOSDBConstants.OrderDetail_Tbl_CloseOrder].Header.VisiblePosition = 4;

            this.gridPODetails.DisplayLayout.Bands[2].Columns[clsPOSDBConstants.PODetail_Fld_QtyInStock].Hidden = true;
            this.gridPODetails.DisplayLayout.Bands[2].Columns[clsPOSDBConstants.PODetail_Fld_QtySold100Days].Hidden = true;
            this.gridPODetails.DisplayLayout.Bands[2].Columns[clsPOSDBConstants.PODetail_Fld_ReOrderLevel].Hidden = true;

            this.gridPODetails.DisplayLayout.Bands[2].Columns[clsPOSDBConstants.PODetail_Fld_ItemID].CellAppearance.TextHAlign = HAlign.Right;
            this.gridPODetails.DisplayLayout.Bands[2].Columns[clsPOSDBConstants.PODetail_Fld_Cost].CellAppearance.TextHAlign = HAlign.Right;
            //this.gridPODetails.DisplayLayout.Bands[2].Columns[clsPOSDBConstants.].CellAppearance.TextHAlign = HAlign.Right;
            this.gridPODetails.DisplayLayout.Bands[2].Columns[clsPOSDBConstants.PODetail_Fld_Price].CellAppearance.TextHAlign = HAlign.Right;
            this.gridPODetails.DisplayLayout.Bands[2].Columns[clsPOSDBConstants.PODetail_Fld_Comments].CellAppearance.TextHAlign = HAlign.Left;

            this.gridPODetails.DisplayLayout.Bands[2].Columns[clsPOSDBConstants.Item_Fld_Description].Header.VisiblePosition = 3;
            this.gridPODetails.DisplayLayout.Bands[2].Columns[clsPOSDBConstants.Item_Fld_Description].CellAppearance.TextHAlign = HAlign.Left;
            this.gridPODetails.DisplayLayout.Bands[2].Columns[clsPOSDBConstants.PODetail_Fld_QTY].Width = 40;
            this.gridPODetails.DisplayLayout.Bands[2].Columns[clsPOSDBConstants.PODetail_Fld_QTY].MaxLength = 5;
            this.gridPODetails.DisplayLayout.Bands[2].Columns[clsPOSDBConstants.PODetail_Fld_QTY].CellAppearance.TextHAlign = HAlign.Right;
            this.gridPODetails.DisplayLayout.Bands[2].Columns[clsPOSDBConstants.PODetail_Fld_VendorItemCode].CellAppearance.TextHAlign = HAlign.Right;

            this.gridItemDetails.DisplayLayout.Bands[0].Columns["Select"].Header.VisiblePosition = 0;//added By Krishna
            this.gridItemDetails.DisplayLayout.Bands[0].Columns["Select"].Header.Caption = "Select To\nDelete";//added By Krishna

        }
        private void ApplyGrigFormat()
        {
            try
            {
                clsUIHelper.SetAppearance(this.gridItemDetails);
                this.gridItemDetails.DisplayLayout.Bands[0].ColHeaderLines = 2;
                //Commented By Amit Date 16 June 2011
                //this.gridItemDetails.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.PODetail_Fld_QtyInStock].Hidden = true;
                //this.gridItemDetails.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.PODetail_Fld_ReOrderLevel].Hidden = true;
                //Modified By Amit Date 16 June 2011

                this.gridItemDetails.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.PODetail_Fld_QtySold100Days].Hidden = true;
                this.gridItemDetails.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.PODetail_Fld_OrderID].Hidden = true;
                this.gridItemDetails.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.PODetail_Fld_PODetailID].Hidden = true;
                this.gridItemDetails.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.PODetail_Fld_AckQTY].Hidden = true;
                this.gridItemDetails.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.PODetail_Fld_AckStatus].Hidden = true;
                this.gridItemDetails.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.PODetail_Fld_ChangedProductID].Hidden = true;
                this.gridItemDetails.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.PODetail_Fld_ChangedProductQualifier].Hidden = true;
                this.gridItemDetails.DisplayLayout.Bands[0].Columns["Order NO"].Hidden = true;
                this.gridItemDetails.DisplayLayout.Bands[0].Columns["Vendor"].Hidden = true;
                this.gridItemDetails.DisplayLayout.Bands[0].Columns["Vend. Itm Code"].Hidden = true;

                this.gridItemDetails.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.PODetail_Fld_ItemID].CellAppearance.TextHAlign = HAlign.Left;
                this.gridItemDetails.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.PODetail_Fld_ItemID].MaxLength = 25;
                this.gridItemDetails.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.PODetail_Fld_ItemID].Width = 135;
                this.gridItemDetails.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.PODetail_Fld_ItemID].Header.Caption = "Item";
                this.gridItemDetails.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.PODetail_Fld_ItemID].Header.Appearance.FontData.SizeInPoints = 9;
                this.gridItemDetails.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.PODetail_Fld_ItemID].Header.VisiblePosition = 0;

                this.gridItemDetails.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.PODetail_Fld_VendorItemCode].CellAppearance.TextHAlign = HAlign.Right;
                this.gridItemDetails.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.PODetail_Fld_VendorItemCode].CellActivation = Activation.Disabled;
                this.gridItemDetails.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.PODetail_Fld_VendorItemCode].MaxLength = 20;
                this.gridItemDetails.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.PODetail_Fld_VendorItemCode].Width = 120;
                this.gridItemDetails.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.PODetail_Fld_VendorItemCode].Header.Caption = "Vend. Itm Code";
                this.gridItemDetails.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.PODetail_Fld_VendorItemCode].Header.Appearance.FontData.SizeInPoints = 9;
                this.gridItemDetails.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.PODetail_Fld_VendorItemCode].Header.VisiblePosition = 1;

                this.gridItemDetails.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.Item_Fld_Description].CellAppearance.TextHAlign = HAlign.Left;
                this.gridItemDetails.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.Item_Fld_Description].CellActivation = Activation.Disabled;
                this.gridItemDetails.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.Item_Fld_Description].Width = 150;
                this.gridItemDetails.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.Item_Fld_Description].MaxLength = 60;
                this.gridItemDetails.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.Item_Fld_Description].Header.VisiblePosition = 2;
                this.gridItemDetails.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.Item_Fld_Description].Header.Appearance.FontData.SizeInPoints = 9;

                this.gridItemDetails.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.PODetail_Fld_Cost].CellActivation = Activation.Disabled;
                this.gridItemDetails.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.PODetail_Fld_Cost].CellAppearance.TextHAlign = HAlign.Right;
                this.gridItemDetails.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.PODetail_Fld_Cost].MaxValue = 9999.99;
                this.gridItemDetails.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.PODetail_Fld_Cost].MaskInput = "9999.99";
                this.gridItemDetails.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.PODetail_Fld_Cost].Format = "####.00";
                this.gridItemDetails.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.PODetail_Fld_Cost].Header.Appearance.FontData.SizeInPoints = 9;
                this.gridItemDetails.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.PODetail_Fld_Cost].Header.VisiblePosition = 3;

                this.gridItemDetails.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.PODetail_Fld_Price].CellAppearance.TextHAlign = HAlign.Right;
                this.gridItemDetails.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.PODetail_Fld_Price].MaxLength = 20;
                this.gridItemDetails.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.PODetail_Fld_Price].Width = 80;
                this.gridItemDetails.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.PODetail_Fld_Price].CellActivation = Activation.Disabled;
                this.gridItemDetails.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.PODetail_Fld_Price].Header.Appearance.FontData.SizeInPoints = 9;
                this.gridItemDetails.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.PODetail_Fld_Price].Header.VisiblePosition = 4;

                this.gridItemDetails.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.PODetail_Fld_BestVendPrice].CellAppearance.TextHAlign = HAlign.Right;
                this.gridItemDetails.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.PODetail_Fld_BestVendPrice].MaxLength = 20;
                this.gridItemDetails.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.PODetail_Fld_BestVendPrice].Width = 80;
                this.gridItemDetails.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.PODetail_Fld_BestVendPrice].CellActivation = Activation.Disabled;
                this.gridItemDetails.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.PODetail_Fld_BestVendPrice].Header.Caption = "Best Price";
                this.gridItemDetails.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.PODetail_Fld_BestVendPrice].Header.Appearance.FontData.SizeInPoints = 9;
                this.gridItemDetails.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.PODetail_Fld_BestVendPrice].Header.VisiblePosition = 5;

                this.gridItemDetails.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.PODetail_Fld_QtyInStock].Width = 55;
                this.gridItemDetails.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.PODetail_Fld_QtyInStock].CellAppearance.TextHAlign = HAlign.Right;
                this.gridItemDetails.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.PODetail_Fld_QtyInStock].CellActivation = Activation.Disabled;
                this.gridItemDetails.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.PODetail_Fld_QtyInStock].Header.Caption = "Qty In\nStock";
                this.gridItemDetails.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.PODetail_Fld_QtyInStock].Header.Appearance.FontData.SizeInPoints = 9;
                this.gridItemDetails.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.PODetail_Fld_QtyInStock].Header.VisiblePosition = 6;

                this.gridItemDetails.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.Item_Fld_MinOrdQty].Width = 55;
                this.gridItemDetails.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.Item_Fld_MinOrdQty].CellAppearance.TextHAlign = HAlign.Right;
                this.gridItemDetails.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.Item_Fld_MinOrdQty].CellActivation = Activation.Disabled;
                this.gridItemDetails.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.Item_Fld_MinOrdQty].Header.Caption = "Desired\nQty";
                this.gridItemDetails.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.Item_Fld_MinOrdQty].Header.Appearance.FontData.SizeInPoints = 9;
                this.gridItemDetails.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.Item_Fld_MinOrdQty].Header.VisiblePosition = 7;

                this.gridItemDetails.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.Item_Fld_ReOrderLevel].Width = 55;
                this.gridItemDetails.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.Item_Fld_ReOrderLevel].CellAppearance.TextHAlign = HAlign.Right;
                this.gridItemDetails.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.Item_Fld_ReOrderLevel].CellActivation = Activation.Disabled;
                this.gridItemDetails.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.Item_Fld_ReOrderLevel].Header.Caption = "Reorder\nLevel";
                this.gridItemDetails.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.Item_Fld_ReOrderLevel].Header.Appearance.FontData.SizeInPoints = 9;
                this.gridItemDetails.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.Item_Fld_ReOrderLevel].Header.VisiblePosition = 8;

                this.gridItemDetails.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.Item_Fld_QtyOnOrder].Hidden = true;
                this.gridItemDetails.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.Item_Fld_QtyOnOrder].Width = 55;
                this.gridItemDetails.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.Item_Fld_QtyOnOrder].CellAppearance.TextHAlign = HAlign.Right;
                this.gridItemDetails.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.Item_Fld_QtyOnOrder].CellActivation = Activation.Disabled;
                this.gridItemDetails.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.Item_Fld_QtyOnOrder].Header.Caption = "Qty On\nOrder";
                this.gridItemDetails.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.Item_Fld_QtyOnOrder].Header.Appearance.FontData.SizeInPoints = 9;
                this.gridItemDetails.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.Item_Fld_QtyOnOrder].Header.VisiblePosition = 9;


                this.gridItemDetails.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.PODetail_Fld_QTY].MaxLength = 6;
                this.gridItemDetails.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.PODetail_Fld_QTY].CellAppearance.TextHAlign = HAlign.Right;
                this.gridItemDetails.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.PODetail_Fld_QTY].CellActivation = Activation.AllowEdit;
                this.gridItemDetails.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.PODetail_Fld_QTY].MaxValue = 999999;
                this.gridItemDetails.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.PODetail_Fld_QTY].MaskInput = "999999";
                this.gridItemDetails.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.PODetail_Fld_QTY].Format = "######";
                this.gridItemDetails.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.PODetail_Fld_QTY].Header.Appearance.FontData.SizeInPoints = 9;
                this.gridItemDetails.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.PODetail_Fld_QTY].Header.VisiblePosition = 11;

                this.gridItemDetails.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.PODetail_Fld_PackSize].Header.Caption = "Packet\nSize";
                this.gridItemDetails.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.PODetail_Fld_PackSize].Header.Appearance.FontData.SizeInPoints = 9;
                this.gridItemDetails.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.PODetail_Fld_PackSize].Width = 50;
                this.gridItemDetails.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.PODetail_Fld_PackSize].Header.VisiblePosition = 12;

                this.gridItemDetails.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.Item_Fld_PacketQuant].Header.Caption = "Packet\nQty";
                this.gridItemDetails.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.Item_Fld_PacketQuant].Header.Appearance.FontData.SizeInPoints = 9;
                this.gridItemDetails.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.Item_Fld_PacketQuant].Width = 50;
                this.gridItemDetails.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.Item_Fld_PacketQuant].Header.VisiblePosition = 13;

                this.gridItemDetails.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.PODetail_Fld_PackUnit].Header.Caption = "Packet\nUnit";
                this.gridItemDetails.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.PODetail_Fld_PackUnit].Header.Appearance.FontData.SizeInPoints = 9;
                this.gridItemDetails.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.PODetail_Fld_PackUnit].Width = 50;
                this.gridItemDetails.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.PODetail_Fld_PackUnit].Header.VisiblePosition = 14;

                this.gridItemDetails.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.PODetail_Fld_Comments].CellAppearance.TextHAlign = HAlign.Left;
                this.gridItemDetails.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.PODetail_Fld_Comments].MaxLength = 50;
                this.gridItemDetails.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.PODetail_Fld_Comments].Header.Appearance.FontData.SizeInPoints = 9;
                this.gridItemDetails.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.PODetail_Fld_Comments].Header.VisiblePosition = 15;

                this.gridItemDetails.DisplayLayout.Bands[0].Columns["Last Vendor"].Hidden = true;



                this.gridItemDetails.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.PODetail_Fld_VendorName].Width = 130;
                this.gridItemDetails.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.PODetail_Fld_VendorName].ValueList = vendorDropDown;
                this.gridItemDetails.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.PODetail_Fld_VendorName].ButtonDisplayStyle = Infragistics.Win.UltraWinGrid.ButtonDisplayStyle.Always;
                this.gridItemDetails.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.PODetail_Fld_VendorName].Header.Caption = "Vendor";
                this.gridItemDetails.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.PODetail_Fld_VendorName].Header.Appearance.FontData.SizeInPoints = 9;
                this.gridItemDetails.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.PODetail_Fld_VendorName].Header.VisiblePosition = 16;

                this.gridItemDetails.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.PODetail_Fld_BestVendor].Width = 120;
                this.gridItemDetails.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.PODetail_Fld_BestVendor].CellAppearance.TextHAlign = HAlign.Left;
                this.gridItemDetails.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.PODetail_Fld_BestVendor].CellActivation = Activation.Disabled;
                this.gridItemDetails.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.PODetail_Fld_BestVendor].Header.Appearance.FontData.SizeInPoints = 9;
                this.gridItemDetails.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.PODetail_Fld_BestVendor].Header.VisiblePosition = 17;

                this.gridItemDetails.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.PODetail_Fld_LastOrdVendor].Width = 120;
                this.gridItemDetails.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.PODetail_Fld_LastOrdVendor].CellActivation = Activation.Disabled;
                this.gridItemDetails.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.PODetail_Fld_LastOrdVendor].Header.Caption = "Last Vendor";
                this.gridItemDetails.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.PODetail_Fld_LastOrdVendor].Header.Appearance.FontData.SizeInPoints = 9;
                this.gridItemDetails.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.PODetail_Fld_LastOrdVendor].Header.VisiblePosition = 18;


                //added by prashant 02-dec-2010
                this.gridItemDetails.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.PODetail_Fld_Idescription].Hidden = true;
                this.gridItemDetails.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.PODetail_Fld_ItemDescType].Hidden = true;
                this.gridItemDetails.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.PODetail_Fld_ItemSold].Hidden = true;

                this.gridItemDetails.DisplayLayout.Bands[0].Columns["Best Price"].Hidden = true;

                this.gridItemDetails.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.PODetail_Fld_LastOrdQty].Header.Caption = "Last Ord\nQty";
                this.gridItemDetails.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.PODetail_Fld_LastOrdQty].Header.Appearance.FontData.SizeInPoints = 9;
                this.gridItemDetails.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.PODetail_Fld_LastOrdQty].CellActivation = Activation.Disabled;
                this.gridItemDetails.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.PODetail_Fld_LastOrdQty].Width = 70;
                this.gridItemDetails.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.PODetail_Fld_LastOrdQty].Header.VisiblePosition = 10;

                this.gridPODetails.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.Item_Fld_Description].Header.Caption = "Order Reference";

                this.gridPODetails.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.OrderDetail_Fld_CloseOrder].Header.Caption = "Send Order";
                this.gridPODetails.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.OrderDetail_Fld_CloseOrder].Width = 180;
                this.gridPODetails.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.OrderDetail_Fld_CloseOrder].Header.VisiblePosition = 19;
                this.gridPODetails.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.OrderDetail_Tbl_CloseOrder].Hidden = true;

                this.gridPODetails.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.PODetail_Fld_OrderID].Hidden = true;

                this.gridPODetails.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.OrderDetail_Fld_TotalCost].Width = 50;
                this.gridPODetails.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.OrderDetail_Fld_TotalCost].MaxLength = 10;
                this.gridPODetails.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.OrderDetail_Fld_TotalCost].CellAppearance.TextHAlign = HAlign.Right;
                this.gridPODetails.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.OrderDetail_Fld_TotalCost].Format = "####.00"; //

                this.gridPODetails.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.OrderDetail_Fld_TimeToOrder].Width = 160;
                this.gridPODetails.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.OrderDetail_Fld_TimeToOrder].MaxLength = 10;
                this.gridPODetails.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.OrderDetail_Fld_TimeToOrder].Header.VisiblePosition = 16;
                this.gridPODetails.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.OrderDetail_Fld_TimeToOrder].CellAppearance.TextHAlign = HAlign.Right;

                this.gridPODetails.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.OrderDetail_Fld_TotalItems].Width = 40;
                this.gridPODetails.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.OrderDetail_Fld_TotalItems].MaxLength = 6;
                this.gridPODetails.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.OrderDetail_Fld_TotalItems].CellAppearance.TextHAlign = HAlign.Right;

                this.gridPODetails.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.OrderDetail_Fld_TotalQty].Width = 40;
                this.gridPODetails.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.OrderDetail_Fld_TotalQty].MaxLength = 5;
                this.gridPODetails.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.OrderDetail_Fld_TotalQty].CellAppearance.TextHAlign = HAlign.Right;

                this.gridPODetails.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.OrderDetail_Fld_AuroSend].Width = 50;
                this.gridPODetails.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.OrderDetail_Fld_AuroSend].MaxLength = 5;
                //Added by Amit Date 6 July 2011
                this.gridItemDetails.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.PODetail_Fld_DeptName].Hidden = true;
                this.gridItemDetails.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.PODetail_Fld_SubDeptName].Hidden = true;
                //Added by Amit Date 4 Aug 2011
                this.gridItemDetails.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.PODetail_Fld_RetailPrice].Hidden = true;
                this.gridItemDetails.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.PODetail_Fld_ItemPrice].Hidden = true;
                this.gridItemDetails.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.PODetail_Fld_Discount].Hidden = true;
                //End
                //TODO: Uncomment it after code check in on item reorder form
                //Added By Amit Date 30 Nov 2011
                //this.gridItemDetails.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.PODetail_Fld_InvRecDate].Hidden = true;
                //End
                this.gridItemDetails.DisplayLayout.Bands[0].Columns["Select"].Header.VisiblePosition = 0;//added By Krishna
                this.gridItemDetails.DisplayLayout.Bands[0].Columns["Select"].Header.Caption = "Select To\nDelete";//added By Krishna
            }
            catch (Exception ex)
            {
            }
        }
        //private void SearchItem(String searchKey)
        //{
        //    try
        //    {
        //        //frmSearch oSearch = new frmSearch(clsPOSDBConstants.Item_tbl);
        //        //Changed By SRT(Gaurav) Date: 30-OCT-2008
        //        //*Possible change
        //        //if (this.cboVendorList.Text != clsPOSDBConstants.NONE)
        //        //{
        //        //frmSearch oSearch = new frmSearch(clsPOSDBConstants.Vendor_Wise_Item_View, "VendorWise", clsPOSDBConstants.Vendor_Fld_VendorCode, vendorCode);
        //        frmSearchMain oSearch = new frmSearchMain(clsPOSDBConstants.Vendor_Wise_Item_View, "VendorWise", clsPOSDBConstants.Vendor_Fld_VendorCode, vendorCode, true);    //20-Dec-2017 JY Added new reference
        //        //End Of Changed By SRT(Gaurav)
        //        oSearch.ShowDialog(this);
        //        if (!oSearch.IsCanceled)
        //        {
        //            string strCode = oSearch.SelectedRowID();
        //            if (strCode == "")
        //                return;

        //            //FKEdit(strCode, clsPOSDBConstants.Item_tbl);
        //        }
        //        //}
        //        //else
        //        //{
        //        //    clsUIHelper.ShowErrorMsg(" No Vendor Is Selected ");
        //        //}
        //    }
        //    catch (Exception exp)
        //    {
        //        clsUIHelper.ShowErrorMsg(exp.Message);
        //    }
        //}

        private void btnItemInfo_Click(object sender, EventArgs e)
        {
            EditItem();
        }

        private void btnAutoGenPO_Click(object sender, EventArgs e)
        {
            try
            {
                isAutoPO = true;
                this.Cursor = Cursors.WaitCursor;
                FillOrder();
                this.Cursor = Cursors.Default;
            }
            catch (Exception Ex)
            {
            }
            finally
            {
                isAutoPO = false;
            }
        }
        #region fillsavePO
        private void fillSavedPO(PODetailRow pdr, bool isFirstTime)
        {
            string POID = string.Empty;
            string podID = string.Empty;
            string vendorID = string.Empty;
            string orderDescription = string.Empty;
            string bestVendor = string.Empty;
            string bestPrice = string.Empty;
            string itemID = string.Empty;
            //DataRow orderRow = null;           
            PODetailRow poDetailRow = null;
            try
            {
                this.FillVendors();
                int podId = GetMaxPODID();
                //InitalizePODetails(orderRow.VendorID.ToString());
                gridItemDetails.ActiveRow.Cells[clsPOSDBConstants.PODetail_Fld_PODetailID].Value = podId;
                gridItemDetails.ActiveRow.Cells["ItemId"].Value = pdr.ItemID;
                gridItemDetails.ActiveRow.Cells["Description"].Value = pdr.ItemDescription;
                gridItemDetails.ActiveRow.Cells["Price"].Value = pdr.Price;
                gridItemDetails.ActiveRow.Cells["Qty"].Value = pdr.QTY > 0 ? pdr.QTY : 1;
                gridItemDetails.ActiveRow.Cells["comments"].Value = pdr.Comments;

                vendorDetails.TryGetValue(pdr.VendorID.ToString(), out vendorCode);
                gridItemDetails.ActiveRow.Cells[clsPOSDBConstants.PODetail_Fld_VendorName].Value = vendorCode;
                //gridItemDetails.ActiveRow.Cells["vend. Itm Code"].Value = pdr.VendorItemCode;
                gridItemDetails.ActiveRow.Cells[clsPOSDBConstants.PODetail_Fld_VendorItemCode].Value = pdr.VendorItemCode;
                gridItemDetails.ActiveRow.Cells["Best Price"].Value = pdr.BestPrice;
                gridItemDetails.ActiveRow.Cells["Best Vendor"].Value = pdr.BestVendor;
                gridItemDetails.ActiveRow.Cells["Last Vendor"].Value = string.Empty;
                gridItemDetails.ActiveRow.Cells["VendorID"].Value = pdr.VendorID;

                gridItemDetails.ActiveRow.Cells["Items Sold"].Value = pdr.SoldItems;

                gridItemDetails.ActiveRow.Cells[clsPOSDBConstants.PODetail_Fld_PackSize].Value = pdr.PacketSize;
                gridItemDetails.ActiveRow.Cells[clsPOSDBConstants.PODetail_Fld_PackQuant].Value = pdr.PacketQuant;
                gridItemDetails.ActiveRow.Cells[clsPOSDBConstants.PODetail_Fld_PackUnit].Value = pdr.Packetunit;
                gridItemDetails.ActiveRow.Cells[clsPOSDBConstants.PODetail_Fld_QTY].Value = pdr.QTY;

                //Added by Amit Date 6 July 2011
                gridItemDetails.ActiveRow.Cells[clsPOSDBConstants.Item_Fld_QtyInStock].Value = pdr.QtyInStock;
                gridItemDetails.ActiveRow.Cells[clsPOSDBConstants.Item_Fld_ReOrderLevel].Value = pdr.ReOrderLevel;
                gridItemDetails.ActiveRow.Cells[clsPOSDBConstants.Item_Fld_MinOrdQty].Value = pdr.MinOrdQty;
                gridItemDetails.ActiveRow.Cells[clsPOSDBConstants.Item_Fld_QtyOnOrder].Value = pdr.QtyOnOrder;
                //End
                if ((gridItemDetails.ActiveRow.Cells[clsPOSDBConstants.PODetail_Fld_PackUnit].Value.ToString().ToUpper().Trim() == clsPOSDBConstants.PckUnit_CS) || (gridItemDetails.ActiveRow.Cells[clsPOSDBConstants.PODetail_Fld_PackUnit].Value.ToString().ToUpper().Trim() == clsPOSDBConstants.PckUnit_CA))   //Sprint-21 22-Feb-2016 JY Added CA for case item
                {
                    if (MMSUtil.UtilFunc.ValorZeroDEC(pdr.PacketSize) > 0)
                    {
                        //gridItemDetails.ActiveRow.Cells["cost"].Value = pdr.Price * MMSUtil.UtilFunc.ValorZeroDEC(gridItemDetails.ActiveRow.Cells["Qty"].Value.ToString()) * MMSUtil.UtilFunc.ValorZeroDEC(pdr.PacketSize);   //Sprint-21 - 1803 28-Sep-2015 JY Commented
                        gridItemDetails.ActiveRow.Appearance.BackColor = Color.Yellow;
                    }
                    else
                    {
                        //gridItemDetails.ActiveRow.Cells["cost"].Value = pdr.Price * MMSUtil.UtilFunc.ValorZeroDEC(gridItemDetails.ActiveRow.Cells["Qty"].Value.ToString());   //Sprint-21 - 1803 28-Sep-2015 JY Commented
                        gridItemDetails.ActiveRow.Appearance.BackColor = Color.Red;
                    }

                }
                else
                {
                    //gridItemDetails.ActiveRow.Cells["cost"].Value = pdr.Price * MMSUtil.UtilFunc.ValorZeroDEC(gridItemDetails.ActiveRow.Cells["Qty"].Value.ToString());   //Sprint-21 - 1803 28-Sep-2015 JY Commented
                }
                gridItemDetails.ActiveRow.Cells["cost"].Value = pdr.Price * MMSUtil.UtilFunc.ValorZeroDEC(gridItemDetails.ActiveRow.Cells["Qty"].Value.ToString()); //Sprint-21 - 1803 28-Sep-2015 JY Added

                haveVendorInfo = true;
                FindBestVendorANDPrice(pdr.ItemID);
                //gridItemDetails.Refresh();
                // gridItemDetails.ActiveRow.Update();
                isFirstTime = false;
                orderDetailData.AcceptChanges();
                //Added by SRT(Abhishek) Date : 08/09/2009
                string compVendor = string.Empty;
                DataRow[] vendorRow = VendorData.Vendor.Select(" VendorID=" + pdr.VendorID);
                if (vendorRow != null && vendorRow.Length > 0)
                    compVendor = vendorRow[0].ItemArray[2].ToString();
                bestVendor = gridItemDetails.ActiveRow.Cells["Best Vendor"].Value.ToString();
                FormatGrid(bestVendor, compVendor);
                //End Of Added by SRT(Abhishek) Date : 08/09/2009                                

                gridItemDetails.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.PODetail_Fld_PackSize].Hidden = true;
                gridItemDetails.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.PODetail_Fld_PackQuant].Hidden = true;
                gridItemDetails.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.PODetail_Fld_PackUnit].Hidden = true;
                //Added by Amit Date 6 july 2011
                gridItemDetails.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.PODetail_Fld_DeptName].Hidden = true;
                gridItemDetails.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.PODetail_Fld_SubDeptName].Hidden = true;
                //End
                //Added by Amit Date 4 Aug 2011
                gridItemDetails.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.PODetail_Fld_RetailPrice].Hidden = true;
                gridItemDetails.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.PODetail_Fld_Discount].Hidden = true;
                gridItemDetails.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.PODetail_Fld_ItemPrice].Hidden = true;

                Application.DoEvents();
                try
                {

                    this.gridItemDetails.ActiveCell = this.gridItemDetails.ActiveRow.Cells[clsPOSDBConstants.PODetail_Fld_QTY];
                    this.gridItemDetails.ActiveCell.Activate();
                    gridItemDetails_KeyDown(this.gridItemDetails, new KeyEventArgs(Keys.Tab));

                    //FocusNewItemId();
                    FocusNewItemId(true);
                    //FocusNewItemId();
                    //gridItemDetails_KeyDown(this.gridItemDetails, new KeyEventArgs(Keys.Enter));
                    //gridItemDetails_KeyDown(this.gridItemDetails, new KeyEventArgs(Keys.Enter));
                    //gridItemDetails_KeyDown(this.gridItemDetails, new KeyEventArgs(Keys.Enter));
                    //gridItemDetails_KeyDown(this.gridItemDetails, new KeyEventArgs(Keys.Enter));
                    //gridItemDetails_KeyDown(this.gridItemDetails, new KeyEventArgs(Keys.Enter));
                }
                catch (Exception)
                {

                    //throw;
                }
                //FindBestVendorANDPrice(itemID, out bestVendor, out bestPrice);
                //poDetailRow[clsPOSDBConstants.PODetail_Fld_BestVendor] = bestVendor;
                //poDetailRow[clsPOSDBConstants.PODetail_Fld_BestVendPrice] = bestPrice;
                //this.haveVendorInfo = true;

                masterOrderDetailsData.PODetailTable.AddRow(poDetailRow);
                try
                {

                    gridItemDetails_KeyDown(this.gridItemDetails, new KeyEventArgs(Keys.Enter));
                    gridItemDetails_KeyDown(this.gridItemDetails, new KeyEventArgs(Keys.Enter));
                    gridItemDetails_KeyDown(this.gridItemDetails, new KeyEventArgs(Keys.Enter));
                    gridItemDetails_KeyDown(this.gridItemDetails, new KeyEventArgs(Keys.Enter));
                    gridItemDetails_KeyDown(this.gridItemDetails, new KeyEventArgs(Keys.Enter));
                    gridItemDetails_KeyDown(this.gridItemDetails, new KeyEventArgs(Keys.Enter));
                }
                catch (Exception)
                {

                    //throw;
                }

                FocusNewItemId();


            }
            catch { }



        }
        #endregion fillsavePO


        private void FillOrder()
        {
            int podId = 0;
            bool isFirstTime = true;
            string vendorCode = string.Empty;
            string ItemDescription = string.Empty;

            this.gridItemDetails.AfterCellUpdate -= new Infragistics.Win.UltraWinGrid.CellEventHandler(this.gridItemDetails_AfterCellUpdate);   //PRIMEPOS-3155 06-Oct-2022 JY Added
            try
            {
                Reports.ReportsUI.frmRptItemReOrder oRptOrder = new POS_Core_UI.Reports.ReportsUI.frmRptItemReOrder(true);
                oRptOrder.ShowDialog(this);
                oRptOrder.BringToFront();
                PODetailData dsNonVendorItems = new PODetailData();//Added by Krishna on 27 June 2012
                if (!oRptOrder.IsCanceled)
                {
                    if (oRptOrder.PODetailDataSet.PODetail.Rows.Count > 0)
                    {
                        if (POS_Core_UI.Resources.Message.Display("Auto PO Populated "
                        + oRptOrder.PODetailDataSet.PODetail.Rows.Count.ToString() + " No(s) of Item(s).\nDo you want to add these items?", "Purchase Order", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                            DisableControls();
                            if (isEditPO)
                            {
                                FocusNewItemId();
                            }

                            gridItemDetails.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.PODetail_Fld_PackSize].Hidden = true;
                            gridItemDetails.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.PODetail_Fld_PackQuant].Hidden = true;
                            gridItemDetails.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.PODetail_Fld_PackUnit].Hidden = true;
                            gridItemDetails.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.PODetail_Fld_DeptName].Hidden = true;
                            gridItemDetails.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.PODetail_Fld_SubDeptName].Hidden = true;
                            gridItemDetails.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.PODetail_Fld_RetailPrice].Hidden = true;
                            gridItemDetails.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.PODetail_Fld_Discount].Hidden = true;
                            gridItemDetails.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.PODetail_Fld_ItemPrice].Hidden = true;

                            foreach (PODetailRow pdr in oRptOrder.PODetailDataSet.PODetail.Rows)
                            {
                                //Added by Atul joshi on 20-11-2010
                                if (pdr.VendorItemCode.ToString() == "" || pdr.VendorID.ToString() == "" || pdr.VendorID.ToString() == "0")
                                {
                                    ItemDescription = pdr.ItemID;
                                    //clsUIHelper.ShowErrorMsg("ITem " + ItemDescription + " do not have VendorItemId can't Process.");//Commented by Krishna
                                    //this.btnAutoGenPO.Enabled = true;
                                    //Added by Krishna on 27 June 2012
                                    PODetailRow drNonVendorItemRow = dsNonVendorItems.PODetail.NewPODetailRow();
                                    drNonVendorItemRow.ItemArray = pdr.ItemArray;
                                    dsNonVendorItems.PODetail.AddRow(drNonVendorItemRow);
                                    //End of added by Krishna on 27 June 2012
                                    continue;
                                }
                                //End of Changes

                                if (gridItemDetails.ActiveRow == null || !isFirstTime)
                                {
                                    if (gridItemDetails.Rows != null)
                                        gridItemDetails.Rows.Band.AddNew();
                                    else
                                        break;
                                }
                                if (isEditPO)
                                {
                                    fillSavedPO(pdr, isFirstTime);
                                    Application.DoEvents();
                                    continue;
                                }
                                podId = GetMaxPODID();
                                InitalizePODetails(pdr.VendorID.ToString());
                                gridItemDetails.ActiveRow.Cells[clsPOSDBConstants.PODetail_Fld_PODetailID].Value = podId;
                                gridItemDetails.ActiveRow.Cells["ItemId"].Value = pdr.ItemID;
                                gridItemDetails.ActiveRow.Cells["Description"].Value = pdr.ItemDescription;
                                gridItemDetails.ActiveRow.Cells["Price"].Value = pdr.Price;
                                gridItemDetails.ActiveRow.Cells["Qty"].Value = pdr.QTY > 0 ? pdr.QTY : 1;
                                gridItemDetails.ActiveRow.Cells["comments"].Value = pdr.Comments;
                                vendorDetails.TryGetValue(pdr.VendorID.ToString(), out vendorCode);
                                gridItemDetails.ActiveRow.Cells[clsPOSDBConstants.PODetail_Fld_VendorName].Value = vendorCode;
                                //gridItemDetails.ActiveRow.Cells["vend. Itm Code"].Value = pdr.VendorItemCode;
                                gridItemDetails.ActiveRow.Cells[clsPOSDBConstants.PODetail_Fld_VendorItemCode].Value = pdr.VendorItemCode;
                                gridItemDetails.ActiveRow.Cells["Best Price"].Value = pdr.BestPrice;
                                gridItemDetails.ActiveRow.Cells["Best Vendor"].Value = pdr.BestVendor;
                                gridItemDetails.ActiveRow.Cells["Last Vendor"].Value = string.Empty;
                                gridItemDetails.ActiveRow.Cells["VendorID"].Value = pdr.VendorID;
                                gridItemDetails.ActiveRow.Cells["Items Sold"].Value = pdr.SoldItems;
                                gridItemDetails.ActiveRow.Cells[clsPOSDBConstants.PODetail_Fld_PackSize].Value = pdr.PacketSize;
                                gridItemDetails.ActiveRow.Cells[clsPOSDBConstants.PODetail_Fld_PackQuant].Value = pdr.PacketQuant;
                                gridItemDetails.ActiveRow.Cells[clsPOSDBConstants.PODetail_Fld_PackUnit].Value = pdr.Packetunit;
                                //gridItemDetails.ActiveRow.Cells[clsPOSDBConstants.PODetail_Fld_QTY].Value = pdr.QTY;  //Sprint-27 - 10-Oct-2017 JY Commented
                                //Added by Amit Date 6 July 2011
                                gridItemDetails.ActiveRow.Cells[clsPOSDBConstants.Item_Fld_QtyInStock].Value = pdr.QtyInStock;
                                gridItemDetails.ActiveRow.Cells[clsPOSDBConstants.Item_Fld_ReOrderLevel].Value = pdr.ReOrderLevel;
                                gridItemDetails.ActiveRow.Cells[clsPOSDBConstants.Item_Fld_MinOrdQty].Value = pdr.MinOrdQty;
                                gridItemDetails.ActiveRow.Cells[clsPOSDBConstants.Item_Fld_QtyOnOrder].Value = pdr.QtyOnOrder;
                                //End
                                if ((gridItemDetails.ActiveRow.Cells[clsPOSDBConstants.PODetail_Fld_PackUnit].Value.ToString().ToUpper().Trim() == clsPOSDBConstants.PckUnit_CS) || (gridItemDetails.ActiveRow.Cells[clsPOSDBConstants.PODetail_Fld_PackUnit].Value.ToString().ToUpper().Trim() == clsPOSDBConstants.PckUnit_CA))   //Sprint-21 22-Feb-2016 JY Added CA for case item
                                {
                                    if (MMSUtil.UtilFunc.ValorZeroDEC(pdr.PacketSize) > 0)
                                    {
                                        //gridItemDetails.ActiveRow.Cells["cost"].Value = pdr.Price * MMSUtil.UtilFunc.ValorZeroDEC(gridItemDetails.ActiveRow.Cells["Qty"].Value.ToString()) * MMSUtil.UtilFunc.ValorZeroDEC(pdr.PacketSize);   //Sprint-21 - 1803 28-Sep-2015 JY Commented
                                        gridItemDetails.ActiveRow.Appearance.BackColor = Color.Yellow;
                                    }
                                    else
                                    {
                                        //gridItemDetails.ActiveRow.Cells["cost"].Value = pdr.Price * MMSUtil.UtilFunc.ValorZeroDEC(gridItemDetails.ActiveRow.Cells["Qty"].Value.ToString());   //Sprint-21 - 1803 28-Sep-2015 JY Commented
                                        gridItemDetails.ActiveRow.Appearance.BackColor = Color.Red;
                                    }
                                }
                                else
                                {
                                    //gridItemDetails.ActiveRow.Cells["cost"].Value = pdr.Price * MMSUtil.UtilFunc.ValorZeroDEC(gridItemDetails.ActiveRow.Cells["Qty"].Value.ToString());   //Sprint-21 - 1803 28-Sep-2015 JY Commented
                                }
                                gridItemDetails.ActiveRow.Cells["cost"].Value = pdr.Price * MMSUtil.UtilFunc.ValorZeroDEC(gridItemDetails.ActiveRow.Cells["Qty"].Value.ToString());   //Sprint-21 - 1803 28-Sep-2015 JY Added

                                haveVendorInfo = true;
                                FindBestVendorANDPrice(pdr.ItemID);
                                gridItemDetails.Refresh();
                                gridItemDetails.ActiveRow.Update();
                                isFirstTime = false;
                                orderDetailData.AcceptChanges();
                                //Added by SRT(Abhishek) Date : 08/09/2009
                                string compVendor = string.Empty;
                                DataRow[] vendorRow = VendorData.Vendor.Select(" VendorID=" + pdr.VendorID);
                                if (vendorRow != null && vendorRow.Length > 0)
                                    compVendor = vendorRow[0].ItemArray[2].ToString();
                                String bestVendor = gridItemDetails.ActiveRow.Cells["Best Vendor"].Value.ToString();
                                FormatGrid(bestVendor, compVendor);
                                //End Of Added by SRT(Abhishek) Date : 08/09/2009
                                Application.DoEvents();
                            }

                            #region PRIMEPOS-3155 06-Oct-2022 JY Added
                            TotalCostForAllPO();
                            int csItemCount = CSItemCount;
                            txtEditorNoOfCSItems.Text = csItemCount.ToString();
                            tbxNonCaseItems.Text = GetNonCaseItemCount(csItemCount).ToString();
                            FormatGrid("ADD");
                            foreach (OrderDetailRow orderDetailRow in orderDetailData.OrderDetailsTable.Rows)
                            {
                                CalculateTotals(orderDetailRow.VendorID.ToString());
                            }
                            #endregion

                            //Added by Krishna on 2 July 2012
                            if (dsNonVendorItems.PODetail.Rows.Count > 0)
                            {
                                frmNonVendorItems ofrmNonVendItems = new frmNonVendorItems(dsNonVendorItems);
                                ofrmNonVendItems.ShowDialog();
                                ofrmNonVendItems.BringToFront();
                                if (ofrmNonVendItems.DialogResult == DialogResult.OK)
                                {
                                    FillAddedItemsinGrid(ofrmNonVendItems.dsAddedItems);
                                }
                            }
                            //End of added by Krishna

                            gridItemDetails.UpdateData();

                            if (gridItemDetails.ActiveRow == null || oRptOrder.PODetailDataSet.PODetail.Rows.Count > 0)
                            {
                                FocusNewItemId();
                            }
                            EnableControls();
                        }
                        else
                        {
                            if (gridItemDetails.ActiveRow == null)
                            {
                                FocusNewItemId();
                            }
                        }
                    }
                    else
                    {
                        clsUIHelper.ShowErrorMsg("No items populated in AutoPO.");
                        if (gridItemDetails.ActiveRow == null)
                        {
                            FocusNewItemId();
                        }
                    }
                }
                else
                {
                    if (gridItemDetails.ActiveRow == null)
                    {
                        FocusNewItemId();
                    }
                }
            }
            catch (Exception exp)
            {
                logger.Fatal(exp, "FillOrder()");
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
            finally
            {
                this.gridItemDetails.AfterCellUpdate += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.gridItemDetails_AfterCellUpdate);   //PRIMEPOS-3155 06-Oct-2022 JY Added
            }
        }


        private void DisableControls()
        {
            this.pnlBestPrice.Enabled = false;
            this.pnlItemHistory.Enabled = false;
            this.pnlClearGrid.Enabled = false;
            //this.Controls.
            this.pnlAddNewItem.Enabled = false;
            this.pnlDeleteSelectedItem.Enabled = false;
            this.pnlItemInfo.Enabled = false;
            //this.gridPODetails.DisplayLayout.Bands[1].Columns[clsPOSDBConstants.OrderDetail_Fld_CloseOrder].CellActivation = Activation.Disabled;
            this.gridPODetails.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.OrderDetail_Fld_CloseOrder].CellActivation = Activation.Disabled;
            this.ControlBox = false;

            this.pnlAutoGenPO.Enabled = false;
            this.pnlCreateForAllVendors.Enabled = false;
            this.ultraBtnPrintPreview.Enabled = false;
            this.ultraBtnPrint.Enabled = false;
            this.pnlSave.Enabled = false;
            this.pnlClose.Enabled = false;
            this.pnlVendorHistory.Enabled = false;
        }
        private void EnableControls()
        {
            this.pnlBestPrice.Enabled = true;
            this.pnlItemHistory.Enabled = true;
            this.pnlClearGrid.Enabled = true;
            this.pnlAddNewItem.Enabled = true;
            this.pnlDeleteSelectedItem.Enabled = true;
            this.pnlItemInfo.Enabled = true;
            this.pnlCreateForAllVendors.Enabled = true;
            this.ultraBtnPrintPreview.Enabled = true;
            this.ultraBtnPrint.Enabled = true;
            this.pnlSave.Enabled = true;
            this.pnlClose.Enabled = true;
            this.pnlVendorHistory.Enabled = true;
            this.ControlBox = true;
            this.gridPODetails.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.OrderDetail_Fld_CloseOrder].CellActivation = Activation.AllowEdit;
        }

        private void FocusItemId()
        {
            this.gridItemDetails.Focus();
            if (gridItemDetails.ActiveRow == null)
            {
                if (gridItemDetails.Rows.Count < 1)
                {
                    return;
                }
                this.gridItemDetails.ActiveRow = this.gridItemDetails.Rows[gridItemDetails.Rows.Count - 1];
            }
            this.gridItemDetails.ActiveCell = gridItemDetails.ActiveRow.Cells["ItemId"];
            this.gridItemDetails.ActiveRow.Cells["ItemId"].Activate();
            this.gridItemDetails.PerformAction(Infragistics.Win.UltraWinGrid.UltraGridAction.ActivateCell);
            this.gridItemDetails.PerformAction(Infragistics.Win.UltraWinGrid.UltraGridAction.EnterEditMode);
        }

        /// <summary>
        /// Author: Ritesh Parekh
        /// Date : 28-Aug-2009
        /// Details : Implemented alternet defination of focus item id for implementation of
        /// on focus select grid item id.
        /// </summary>
        /// <param name="isFocusOnGrid"></param>
        private void FocusItemId(bool isFocusOnGrid)
        {
            if (isFocusOnGrid == false)
            {
                this.gridItemDetails.Focus();
            }
            if (gridItemDetails.ActiveRow == null)
            {
                if (gridItemDetails.Rows.Count < 1)
                {
                    return;
                }
                this.gridItemDetails.ActiveRow = this.gridItemDetails.Rows[gridItemDetails.Rows.Count - 1];
            }
            this.gridItemDetails.ActiveCell = gridItemDetails.ActiveRow.Cells["ItemId"];
            this.gridItemDetails.ActiveRow.Cells["ItemId"].Activate();
            this.gridItemDetails.PerformAction(Infragistics.Win.UltraWinGrid.UltraGridAction.ActivateCell);
            this.gridItemDetails.PerformAction(Infragistics.Win.UltraWinGrid.UltraGridAction.EnterEditMode);
        }


        private void FocusNewItemId()
        {
            try
            {
                //this.gridItemDetails.
                this.gridItemDetails.Focus();
                //Updated By SRT(Gaurav) Date : 14-Jul-2009
                if (gridItemDetails.Rows.Count <= Configuration.convertNullToInt(TotalItemsCount.ToString()))
                {
                    this.gridItemDetails.Rows.Band.AddNew();
                }
                //Updated By SRT(Ritesh Parekh) Date : 27-Jul-2009
                //current implementation of the condition is '!=' changed to '=='
                //need to check null of active row here.
                if (gridItemDetails.ActiveRow == null)
                {
                    this.gridItemDetails.ActiveRow = this.gridItemDetails.Rows[gridItemDetails.Rows.Count - 1];
                }
                //End Of Updated By SRT(Ritesh Parekh)
                this.gridItemDetails.ActiveCell = gridItemDetails.ActiveRow.Cells["ItemId"];
                this.gridItemDetails.ActiveRow.Cells["ItemId"].Activate();
                this.gridItemDetails.PerformAction(Infragistics.Win.UltraWinGrid.UltraGridAction.ActivateCell);
                this.gridItemDetails.PerformAction(Infragistics.Win.UltraWinGrid.UltraGridAction.EnterEditMode);
            }
            catch (StackOverflowException Ex)
            {
            }
        }

        //Added by SRT(Abhishek) Date : 09/09/2009
        //Added for focusing the row behind which is just deleted   
        private void FocusRowBeforeDeleted()
        {
            try
            {
                this.gridItemDetails.Focus();

                if (activeRowIndex > 0)
                    this.gridItemDetails.ActiveRow = this.gridItemDetails.Rows[activeRowIndex - 1];
                else
                {
                    if (this.gridItemDetails.Rows.All.Length == 0)
                        this.gridItemDetails.Rows.Band.AddNew();
                    this.gridItemDetails.ActiveRow = this.gridItemDetails.Rows[activeRowIndex];
                }

                this.gridItemDetails.ActiveCell = gridItemDetails.ActiveRow.Cells["ItemId"];
                this.gridItemDetails.ActiveRow.Cells["ItemId"].Activate();
                this.gridItemDetails.PerformAction(Infragistics.Win.UltraWinGrid.UltraGridAction.ActivateCell);
                this.gridItemDetails.PerformAction(Infragistics.Win.UltraWinGrid.UltraGridAction.EnterEditMode);
            }
            catch (StackOverflowException Ex)
            {
            }
        }
        //End of Added by SRT(Abhishek) Date : 09/09/2009


        /// <summary>
        /// Author Ritesh Parekh
        /// Date: 28-Aug-2009
        /// Details: Implemented alternet method for on focus item select of the grid.
        /// </summary>
        /// <param name="isFocusOnGrid"></param>
        private void FocusNewItemId(bool isFocusOnGrid)
        {
            try
            {
                if (isFocusOnGrid == false)
                {
                    this.gridItemDetails.Focus();
                }
                //Updated By SRT(Gaurav) Date : 14-Jul-2009
                if (gridItemDetails.Rows.Count <= Configuration.convertNullToInt(TotalItemsCount.ToString()))
                {
                    this.gridItemDetails.Rows.Band.AddNew();
                }
                //Updated By SRT(Ritesh Parekh) Date : 27-Jul-2009
                //current implementation of the condition is '!=' changed to '=='
                //need to check null of active row here.
                if (gridItemDetails.ActiveRow == null)
                {
                    this.gridItemDetails.ActiveRow = this.gridItemDetails.Rows[gridItemDetails.Rows.Count - 1];
                }
                //End Of Updated By SRT(Ritesh Parekh)
                this.gridItemDetails.ActiveCell = gridItemDetails.ActiveRow.Cells["ItemId"];
                this.gridItemDetails.ActiveRow.Cells["ItemId"].Activate();
                this.gridItemDetails.PerformAction(Infragistics.Win.UltraWinGrid.UltraGridAction.ActivateCell);
                this.gridItemDetails.PerformAction(Infragistics.Win.UltraWinGrid.UltraGridAction.EnterEditMode);
            }
            catch (StackOverflowException Ex)
            {
            }
        }

        private void FocusQty()
        {
            this.gridItemDetails.Focus();
            if (gridItemDetails.ActiveRow != null)
            {
                this.gridItemDetails.ActiveRow = this.gridItemDetails.Rows[gridItemDetails.Rows.Count - 1];
            }
            this.gridItemDetails.ActiveCell = this.gridItemDetails.ActiveRow.Cells[clsPOSDBConstants.PODetail_Fld_QTY];
            this.gridItemDetails.ActiveCell.Activate();
            this.gridItemDetails.PerformAction(UltraGridAction.ActivateCell);
            this.gridItemDetails.PerformAction(UltraGridAction.EnterEditMode);
        }

        private void ultraBtnBestPrice_Click(object sender, EventArgs e)
        {
            string vendorItemCode = string.Empty;
            string itemCode = string.Empty;
            string costPrice = string.Empty;
            string upcCode = string.Empty;
            string vendorName = string.Empty;
            string vendorID = string.Empty;
            string description = string.Empty;
            try
            {
                if (gridItemDetails.ActiveRow != null)
                {
                    upcCode = gridItemDetails.ActiveRow.Cells[clsPOSDBConstants.PODetail_Fld_ItemID].Value.ToString();
                }
                else
                {
                    string selectMsg = "You have not selected any item.";
                    clsUIHelper.ShowErrorMsg(selectMsg);
                    //Added By SRT(Ritesh Parekh) Date : 19-Aug-2009
                    FocusNewItemId();
                    //End Of Added By SRT(Ritesh Parekh)
                    return;
                }
                frmComparePrices bestPrice = new frmComparePrices();
                bestPrice.CompareVendorPrices(upcCode);
                //Updated By SRT(Gaurav) Date: 08-Jul-2009
                //Validated Best Price / Vndor Comparison for more than 1 vendors.
                if (bestPrice.OtherVendorsCount > 1)
                {
                    bestPrice.ShowDialog();
                    if (bestPrice.IsIncluded)
                    {
                        vendorID = bestPrice.SelectedVendorID();
                        vendorItemCode = bestPrice.SelectedVendItemId();
                        itemCode = bestPrice.SelectedItemID();
                        costPrice = bestPrice.SelectedCostPrice();
                        vendorName = bestPrice.SelectedVendorName();
                        description = bestPrice.SelectedDescription();
                        FillGrid(vendorID, vendorItemCode, costPrice, vendorName, description);
                    }
                }
                else
                {
                    clsUIHelper.ShowErrorMsg("Other Vendors Are Not Available For Item #" + upcCode);
                }
                //End Of Updated By SRT(Gaurav)
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "ultraBtnBestPrice_Click(object sender, EventArgs e)");
                clsUIHelper.ShowErrorMsg(ex.Message);
                //Added By SRT(Ritesh Parekh) Date : 19-Aug-2009
                FocusNewItemId();
                //End Of Added By SRT(Ritesh Parekh)
            }
        }

        private void FillGrid(string bestVendorID, string vendorItemId, string costPrice, string vendorName, string description)
        {
            bool isExist = false;
            string vendorID = string.Empty;
            try
            {
                isBestVendorUpdated = false;
                if (gridItemDetails.ActiveRow != null)
                {
                    // isExist = this.orderDetailData.OrderDetailsTable.Rows.Contains(bestVendorID);
                    //Following if-else if is added by shitaljit on 1/16/2014 
                    //For JIRA 1719, 1720
                    if (isEditPO == false && Configuration.isNullOrEmptyDataSet(orderDetailData) == false)
                    {
                        isExist = this.orderDetailData.OrderDetailsTable.Rows.Contains(bestVendorID);
                    }
                    else if (Configuration.isNullOrEmptyDataSet(masterOrderDetailsData) == false)
                    {
                        isExist = this.masterOrderDetailsData.OrderDetailsTable.Rows.Contains(bestVendorID);
                    }
                    if (isExist == false && isEditPO == false)
                    {
                        InitalizePODetails(bestVendorID);
                    }
                    else if (isExist == false && isEditPO == true)
                    {
                        InitalizeMasterOrderDetails(bestVendorID, "", description, false);
                    }
                    vendorID = gridItemDetails.ActiveRow.Cells["VendorID"].Value.ToString().Trim();
                    this.gridItemDetails.ActiveRow.Cells["VendorID"].Value = bestVendorID;
                    //Updated By SRT(Gaurav) Date : 16-Jul-2009
                    //Changed The column name.
                    this.gridItemDetails.ActiveRow.Cells["VendorName"].Value = vendorName;
                    this.gridItemDetails.ActiveRow.Cells["Price"].Value = costPrice;
                    this.gridItemDetails.ActiveRow.Cells[clsPOSDBConstants.PODetail_Fld_VendorItemCode].Value = vendorItemId;
                    this.gridItemDetails.ActiveRow.Cells["Description"].Value = description;
                    this.gridItemDetails.Refresh();
                    nonBestVendor = vendorID;
                    isBestVendorUpdated = true;
                    this.gridItemDetails.ActiveRow.Update();
                }
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "FillGrid(string bestVendorID, string vendorItemId, string costPrice, string vendorName, string description)");
                clsUIHelper.ShowErrorMsg(ex.Message);
            }
        }



        private string FKEdit(string code, string senderName, string searchCriteria)
        {
            UltraGridCell oCurrentCell;
            oCurrentCell = this.gridItemDetails.ActiveCell;
            string vendorID = string.Empty;
            string bestVendor = string.Empty;
            string vendorName = string.Empty;
            string vendorItemCode = string.Empty;
            string vendorCostPrice = string.Empty;
            string bestVendorPrice = string.Empty;
            string status = string.Empty;
            //Added By Amit Date 15 May 2011
            string QtyInStock = string.Empty;
            string ReorderLevel = string.Empty;
            string QtyOnOrder = string.Empty;
            string MinOrdQty = string.Empty;
            //End

            string sqlForLastOrderVendQty = "SELECT VN.VENDORCODE,POD.ORDERID,QTY FROM PURCHASEORDERDETAIL AS POD,PURCHASEORDER AS PO, VENDOR AS VN " +
                                             "WHERE PODETAILID=( select max(podetailid)  from purchaseorderdetail where itemid='{0}') " +
                                             "AND PO.ORDERID=POD.ORDERID AND PO.VENDORID=VN.VENDORID";
            bool launchSearch = false;
            bool isItemExist = false;
            DataSet dataSetlastOrderVendQty = null;
            Search oBLSearch = new Search();

            if (senderName == clsPOSDBConstants.Vendor_tbl)
            {
                try
                {
                    POS_Core.BusinessRules.Vendor oVendor = new POS_Core.BusinessRules.Vendor();
                    VendorData oVendorData;
                    VendorRow oVendorRow = null;
                    oVendorData = oVendor.Populate(code);
                    oVendorRow = oVendorData.Vendor[0];
                    if (oVendorRow != null)
                    {

                    }
                }
                catch (System.IndexOutOfRangeException)
                { }
                catch (Exception exp)
                {
                    logger.Fatal(exp, "FKEdit(string code, string senderName, string searchCriteria)");
                    clsUIHelper.ShowErrorMsg(exp.Message);
                }
            }
            else if (senderName == clsPOSDBConstants.Item_tbl)
            {
                try
                {
                    int poDetailId = 0;
                    Item item = new Item();
                    ItemData itemData = null;
                    ItemRow itemRow = null;
                    ItemVendor itemVendor = new ItemVendor();
                    ItemVendorData itemVendorData = null;
                    //frmSearch oSearch = null;
                    frmSearchMain oSearch = null;   //20-Dec-2017 JY Added new reference
                    string lastOrderVendor = string.Empty;
                    int lastOrderQty = 0;

                    vendorCostPrice = "0.00";
                    //Updated By SRT(Gaurav) Date: 03-Jul-2009
                    //Checked for ActiveRow Is null and handeled it.
                    if (this.gridItemDetails.ActiveRow == null)
                    {
                        FocusNewItemId();
                    }
                    //End Of updated by SRT(Gaurav)
                    //this.gridItemDetails.ActiveRow.Cells[clsPOSDBConstants.PODetail_Fld_VendorName].DroppedDown = true;
                    if (searchCriteria == "UPC" && code != string.Empty)
                    {
                        //itemVendorData = itemVendor.PopulateList(" AND " + clsPOSDBConstants.ItemVendor_tbl + ".ItemID like '" + code + "%'");
                        itemVendorData = GetTargetedVenorInfo(code);
                        if (itemVendorData.ItemVendor.Rows.Count > 1)
                        {
                            //oSearch = new frmSearch(clsPOSDBConstants.ItemId, code);
                            oSearch = new frmSearchMain(clsPOSDBConstants.ItemId, code, true);  //20-Dec-2017 JY Added new reference
                            launchSearch = true;
                            oSearch.IsFromPurchaseOrder = true;
                        }
                        else
                        {
                            itemData = item.PopulateList(" WHERE " + clsPOSDBConstants.Item_tbl + ".ItemID like '" + code + "%'");
                        }
                    }
                    else if (searchCriteria == "VENDOR_ITEM_CODE" && code != string.Empty)
                    {
                        itemVendorData = itemVendor.PopulateList(" AND " + clsPOSDBConstants.ItemVendor_tbl + ".VendorItemID like'" + code.Trim() + "%'");
                        if (itemVendorData.ItemVendor.Rows.Count > 1)
                        {
                            //oSearch = new frmSearch(clsPOSDBConstants.VendorItemCodeWise, oCurrentCell.Text.ToString().Trim());
                            oSearch = new frmSearchMain(clsPOSDBConstants.VendorItemCodeWise, oCurrentCell.Text.ToString().Trim(), true);   //20-Dec-2017 JY Added new reference
                            launchSearch = true;
                            oSearch.IsFromPurchaseOrder = true;
                            //Added By Amit Date 17 June 2011
                            //oSearch.searchInConstructor = true;
                        }
                    }
                    else if (searchCriteria == "DESCRIPTION" && code != string.Empty)
                    {
                        itemData = item.PopulateList(" WHERE " + clsPOSDBConstants.Item_tbl + ".Description like '" + code + "%'");
                        //Updated By SRT(Gaurav) Date : 11-Jul-2009
                        //Updated to solve the empty search screen open issue in description search.
                        //Condition occurs if data is available in item table and not available in itemvendor table.

                        //oSearch = new frmSearch(clsPOSDBConstants.DescriptionWise, oCurrentCell.Text.ToString().Trim());
                        oSearch = new frmSearchMain(clsPOSDBConstants.DescriptionWise, oCurrentCell.Text.ToString().Trim(), true);  //20-Dec-2017 JY Added new reference
                        oSearch.IsFromPurchaseOrder = true;
                        oSearch.SearchInConstructor = true;
                        //Updated condition by Gaurav
                        //if(oSearch.SearchDataRowsCount > 0)

                        if (itemData.Item.Rows.Count > 1 && oSearch.SearchDataRowsCount > 0)
                        {
                            //End OF Updated By SRT(Gaurav)    
                            oSearch.ShowDialog(this);

                            if (oSearch.IsCanceled)
                            {
                                oCurrentCell.CancelUpdate();
                                this.gridItemDetails.ActiveCell = oCurrentCell;
                                oCurrentCell.Selected = true;
                                this.gridItemDetails.PerformAction(UltraGridAction.ActivateCell);
                                this.gridItemDetails.PerformAction(UltraGridAction.EnterEditMode);
                                return status;
                            }
                            //Added by SRT(Abhishek) Date: 13-Aug-2009
                            isItemPopulated = true;
                            //End Of Added By SRT(Abhishek)

                            code = oSearch.SelectedRowID().Trim();
                            isItemExist = true;
                            //itemVendorData = itemVendor.PopulateList(" AND " + clsPOSDBConstants.ItemVendor_tbl + ".ItemID like '" + code + "%'");
                            itemVendorData = GetTargetedVenorInfo(code);
                        }
                        else
                        {
                            if (itemData.Item.Rows.Count == 1)
                            {
                                //itemVendorData = itemVendor.PopulateList(" AND " + clsPOSDBConstants.ItemVendor_tbl + ".ItemID like '" + itemData.Item.Rows[0]["ItemID"].ToString().Trim() + "%'");
                                code = itemData.Item.Rows[0]["ItemID"].ToString().Trim();
                                itemVendorData = GetTargetedVenorInfo(code);
                            }
                        }
                        if (itemVendorData != null)
                        {
                            if (itemVendorData.ItemVendor.Rows.Count > 1)
                            {
                                oSearch.IsFromPurchaseOrder = true;
                                launchSearch = true;
                            }
                        }
                    }
                    else if (searchCriteria == "UPC" && code == string.Empty)
                    {
                        //oSearch = new frmSearch(clsPOSDBConstants.ItemId, oCurrentCell.Text.ToString().Trim());
                        oSearch = new frmSearchMain(clsPOSDBConstants.ItemId, oCurrentCell.Text.ToString().Trim(), true);   //20-Dec-2017 JY Added new reference
                        launchSearch = true;
                    }
                    if (launchSearch)
                    {
                        oSearch.ShowDialog(this);
                        if (oSearch.IsCanceled)
                        {
                            oCurrentCell.CancelUpdate();
                            this.gridItemDetails.SelectNextControl(this.gridItemDetails, false, true, false, true);
                            return "ItemSearch-Canceled";
                        }
                        haveVendorInfo = true;
                        code = oSearch.SelectedRowID();
                        if (code == "")
                        {
                            return "";
                        }
                        isItemExist = true;
                        vendorID = oSearch.SelectedVendorID();
                        bestVendor = oSearch.SelectedBestVendor();
                        //vendorItemCode = oSearch.SelectedCode();
                        //Added by SRT(Abhishek) Date : 21 Aug 2009
                        //Added to return value for this coulomn ,only value for VendorItemID 
                        vendorItemCode = oSearch.SelectedVendorItemCode("VendorItemId");
                        //Added by SRT(Abhishek)
                        //Added By Amit Date 15 May 2011
                        QtyInStock = oSearch.SelectedVendorItemCode("QtyInStock");
                        ReorderLevel = oSearch.SelectedVendorItemCode("ReorderLevel");
                        QtyOnOrder = oSearch.SelectedVendorItemCode("QtyOnOrder");
                        MinOrdQty = oSearch.SelectedVendorItemCode("MinOrdQty");

                        bestVendorPrice = oSearch.SelectedBestVendorPrice();
                        vendorCostPrice = oSearch.SelectedVendorCostPrice();
                        this.vendorDetails.TryGetValue(vendorID, out vendorName);
                        //this.vendorDetails.TryGetValue(
                        this.gridItemDetails.ActiveRow.Cells[clsPOSDBConstants.PODetail_Fld_BestVendor].Value = bestVendor;
                        this.gridItemDetails.ActiveRow.Cells[clsPOSDBConstants.PODetail_Fld_BestVendPrice].Value = bestVendorPrice;
                        //Added By Amit Date 16 June 2011
                        this.gridItemDetails.ActiveRow.Cells[clsPOSDBConstants.Item_Fld_QtyInStock].Value = QtyInStock;
                        this.gridItemDetails.ActiveRow.Cells[clsPOSDBConstants.Item_Fld_ReOrderLevel].Value = ReorderLevel;
                        this.gridItemDetails.ActiveRow.Cells[clsPOSDBConstants.Item_Fld_MinOrdQty].Value = MinOrdQty;
                        this.gridItemDetails.ActiveRow.Cells[clsPOSDBConstants.Item_Fld_QtyOnOrder].Value = QtyOnOrder;
                    }
                    else if (itemVendorData != null && itemVendorData.Tables.Count > 0 && itemVendorData.ItemVendor.Rows.Count > 0)
                    {
                        haveVendorInfo = true;
                        if (itemVendorData.ItemVendor.Rows.Count > 0)
                        {
                            code = itemVendorData.ItemVendor.Rows[0][clsPOSDBConstants.PODetail_Fld_ItemID].ToString();
                            isItemExist = true;
                            FindBestVendorANDPrice(code);
                            vendorID = itemVendorData.ItemVendor.Rows[0][clsPOSDBConstants.POHeader_Fld_VendorID].ToString();
                            vendorItemCode = itemVendorData.ItemVendor.Rows[0]["VendorItemID"].ToString();
                            vendorCostPrice = itemVendorData.ItemVendor.Rows[0]["VendorCostPrice"].ToString();
                            this.vendorDetails.TryGetValue(vendorID, out vendorName);
                        }
                    }
                    else
                    {
                        haveVendorInfo = false;
                    }
                    if (isItemExist && haveVendorInfo)
                    {
                        //WRITE A CODE HERE TO CHECK WHETHER ITEM IS ALLREADY ORDERED OR NOT....
                        //Updated By SRT(Gaurav) Date: 03-Jul-2009
                        //Updated with check for ItemId Allready ordered or Not.
                        itemData = item.Populate(code.Trim());
                        if (itemData.Item.Rows.Count > 0)
                        {
                            bool ItemAllreadyOrdered = IsItemOrderedAllready(itemData.Item[0].ItemID);
                            if (!ItemAllreadyOrdered)
                            {
                                itemRow = itemData.Item[0];
                            }
                            else if ((ItemAllreadyOrdered && !AskForOrderedItem) || (ItemAllreadyOrdered && POS_Core_UI.Resources.Message.Display("Item Is Allready Ordered. Do you want to Order Item Again?", "PrimePOS", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes))
                            {
                                itemRow = itemData.Item[0];
                                string tempSql = String.Format(sqlForLastOrderVendQty, new object[] { itemRow.ItemID });
                                dataSetlastOrderVendQty = oBLSearch.SearchData(tempSql);
                                lastOrderVendor = dataSetlastOrderVendQty.Tables[0].Rows[0]["VENDORCODE"].ToString();
                                lastOrderQty = (int)dataSetlastOrderVendQty.Tables[0].Rows[0]["QTY"];
                            }
                            else
                            {
                                //Added by SRT(Abhishek) Date: 13-Aug-2009
                                isItemPopulated = false;
                                //End Of Added By SRT(Abhishek)

                                lastOrderQty = 0;
                                this.gridItemDetails.ActiveRow.CancelUpdate();
                                this.gridItemDetails.PerformAction(UltraGridAction.PrevCell);
                                this.gridItemDetails.PerformAction(UltraGridAction.ExitEditMode);
                                FocusNewItemId();
                                return "OrdItem-NO";
                            }
                            this.gridItemDetails.ActiveRow.Cells[clsPOSDBConstants.PODetail_Fld_LastOrdVendor].Value = lastOrderVendor;
                            this.gridItemDetails.ActiveRow.Cells[clsPOSDBConstants.PODetail_Fld_LastOrdQty].Value = lastOrderQty;
                        }
                        //End Of Updated By SRT(Gaurav)
                    }
                    else
                    {
                        //this.gridItemDetails.ActiveRow.Cells[clsPOSDBConstants.PODetail_Fld_VendorName].DroppedDown = false;
                        clsUIHelper.ShowInfoMsg(clsPOSDBConstants.AddItemMessage);
                        if (clsUIHelper.IsOK)
                        {
                            frmAddNewItem addItems = new frmAddNewItem();
                            bool isInt = clsUIHelper.isNumeric(code);
                            if (isInt && searchCriteria == "DESCRIPTION")
                            {
                                searchCriteria = "UPC";
                            }
                            addItems.FillItemDescription(code, searchCriteria);
                            addItems.ShowDialog();
                            clsUIHelper.IsOK = false;
                            //Added by SRT(Gaurav) Date: 10-Jul-2009
                            if (!addItems.IsCanceled)
                            {
                                //Updated By SRT(Gaurav) Date: 11-Jul-2009
                                //This is to get the exact data based on search criteria on updating the data in Add New Item Flow.
                                if (searchCriteria == "DESCRIPTION" || searchCriteria == "VENDOR_ITEM_CODE")
                                {
                                    searchCriteria = "UPC";
                                    code = addItems.UPCCODE;
                                }
                                FKEdit(code, clsPOSDBConstants.Item_tbl, searchCriteria);// == "UPC" ? "DESCRIPTION" : "UPC");
                            }
                            //End Of Added By SRT(Gaurav)

                            //Added by SRT(Abhishek) Date: 13-Aug-2009
                            isItemPopulated = true;
                            //End Of Added By SRT(Abhishek)
                        }
                        else
                        {
                            //Added by SRT(Abhishek) Date: 13-Aug-2009
                            isItemPopulated = false;
                            //End Of Added By SRT(Abhishek)

                            //Added by SRT(Abhishek) Date : 26 Aug 2009
                            //Added if user decided not to add item in the db then row will be deleted and 
                            //focus will be set to the new row.
                            this.gridItemDetails.ActiveRow.Delete(false);
                            FocusNewItemId();
                            //End of Added by SRT(Abhishek) Date : 26 Aug 2009

                            return "";
                        }
                        return "";
                    }
                    foreach (UltraGridRow orow in this.gridItemDetails.Rows)
                    {
                        if (orow != gridItemDetails.ActiveRow)
                        {
                            if (orow.Cells[clsPOSDBConstants.PODetail_Fld_ItemID].Text == itemRow.ItemID)
                            {
                                gridItemDetails.ActiveRow.Delete(false);
                                clsUIHelper.ShowErrorMsg("Item is already in the list.");
                                FocusNewItemId();
                                return "ITEM-EXIST";
                            }
                        }
                    }

                    poDetailId = GetMaxPODID();
                    this.gridItemDetails.ActiveRow.Cells[clsPOSDBConstants.PODetail_Fld_PODetailID].Value = poDetailId;
                    this.gridItemDetails.ActiveRow.Cells[clsPOSDBConstants.PODetail_Fld_ItemID].Value = itemRow.ItemID;
                    this.gridItemDetails.ActiveRow.Cells[clsPOSDBConstants.Description].Value = itemRow.Description;
                    this.gridItemDetails.ActiveRow.Cells[clsPOSDBConstants.PODetail_Fld_Price].Value = vendorCostPrice;
                    this.gridItemDetails.ActiveRow.Cells[clsPOSDBConstants.PODetail_Fld_QTY].Value = itemRow.MinOrdQty;

                    //Added by SRT(Abhishek)
                    gridItemDetails.ActiveRow.Cells[clsPOSDBConstants.PODetail_Fld_PackSize].Value = itemRow.PckSize;
                    gridItemDetails.ActiveRow.Cells[clsPOSDBConstants.PODetail_Fld_PackQuant].Value = itemRow.PckQty;
                    gridItemDetails.ActiveRow.Cells[clsPOSDBConstants.PODetail_Fld_PackUnit].Value = itemRow.PckUnit;
                    if ((gridItemDetails.ActiveRow.Cells[clsPOSDBConstants.PODetail_Fld_PackUnit].Value.ToString().ToUpper().Trim() == clsPOSDBConstants.PckUnit_CS) || (gridItemDetails.ActiveRow.Cells[clsPOSDBConstants.PODetail_Fld_PackUnit].Value.ToString().ToUpper().Trim() == clsPOSDBConstants.PckUnit_CA))   //Sprint-21 22-Feb-2016 JY Added CA for case item
                    {
                        gridItemDetails.ActiveRow.Appearance.BackColor = Color.Yellow;
                    }
                    //Commented By Amit Date 21 june 2011
                    //gridItemDetails.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.PODetail_Fld_PackSize].Hidden = true;
                    //gridItemDetails.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.PODetail_Fld_PackQuant].Hidden = true;
                    //gridItemDetails.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.PODetail_Fld_PackUnit].Hidden = true;

                    //End of added by SRT(Abhishek)

                    if (haveVendorInfo == true)
                    {
                        this.gridItemDetails.ActiveRow.Cells[clsPOSDBConstants.PODetail_Fld_VendorItemCode].Value = vendorItemCode;
                        this.gridItemDetails.ActiveRow.Cells[clsPOSDBConstants.POHeader_Fld_VendorID].Value = Configuration.convertNullToInt(vendorID);

                        this.vendorDetails.TryGetValue(vendorID, out vendorName);
                        this.gridItemDetails.ActiveRow.Cells[clsPOSDBConstants.PODetail_Fld_VendorName].Value = vendorName;

                        if (isEditPO)
                        {
                            orderNosDropDown = new UltraDropDown();
                            this.orderNosDropDown.RowSelected += new RowSelectedEventHandler(orderNosDropDown_RowSelected);

                            List<String> orders = null;
                            poOrdersNos.TryGetValue(vendorID, out orders);
                            orderNosDropDown.DataSource = orders;
                            this.gridItemDetails.ActiveRow.Cells[clsPOSDBConstants.OrderDetail_Fld_OrderNo].ValueList = orderNosDropDown;
                        }
                        else
                        {
                            InitalizePODetails(vendorID);
                        }

                        //Added by SRT(Abhishek) 25 Aug 2009
                        //Added to compare vendor name 
                        string compVendor = string.Empty;
                        DataRow[] vendorRow = VendorData.Vendor.Select(" VendorID=" + vendorID);
                        if (vendorRow != null && vendorRow.Length > 0)
                            compVendor = vendorRow[0].ItemArray[2].ToString();
                        bestVendor = this.gridItemDetails.ActiveRow.Cells[clsPOSDBConstants.PODetail_Fld_BestVendor].Value.ToString();

                        FormatGrid(bestVendor, compVendor);
                        //End of Added by SRT(Abhishek) 25 Aug 2009
                    }

                    this.gridItemDetails.Refresh();
                    this.txtEditorNoOfItems.Text = TotalItemsCount.ToString();
                    txtEditorNoOfVendors.Text = TotalVendorsCount.ToString();
                    int csItemCount = CSItemCount;
                    txtEditorNoOfCSItems.Text = csItemCount.ToString();
                    // Added by Shrikant Mali on 02/10/2014 to update the count of non case Items.
                    tbxNonCaseItems.Text = GetNonCaseItemCount(csItemCount).ToString();
                    //this.txtTotalCostForAllPO.Text = totalCostForAllPO.ToString();//Added By shitaljit for all PO(s) total cost   //Sprint-21 - 1803 15-Sep-2015 JY Commented
                    isAutoPO = false;

                    //this.gridItemDetails.ActiveRow.Update();
                    //FocusQty();
                    #region commented
                    //if (vendorID != clsPOSDBConstants.NONE)
                    //{
                    //    ItemVendor oIVend = new ItemVendor();
                    //    ItemVendorData oIVData = oIVend.PopulateList(" and ItemVendor."
                    //        + clsPOSDBConstants.ItemVendor_Fld_ItemID + "='" + oItemRow.ItemID.ToString() + "' " +
                    //        //*Possible change
                    //        " and ItemVendor." + clsPOSDBConstants.ItemVendor_Fld_VendorID + "=" + vendorID.ToString() + "");

                    //    if (oIVData.ItemVendor.Rows.Count > 0)
                    //    {
                    //        // if item exist for vendorCode 
                    //        // then we will check if last vendor for the item matches with the current vendor.
                    //        // if last vendor and current vendor is same then assign the values in grid .  
                    //        if (!oIVData.ItemVendor[0].IsDeleted)
                    //        {
                    //            //*Possible change
                    //            if (this.vendorID.ToString() == oItemRow.LastVendor.ToString())
                    //            {
                    //                this.gridItemDetails.ActiveRow.Cells[clsPOSDBConstants.PODetail_Fld_VendorItemCode].Value = oIVData.ItemVendor[0][clsPOSDBConstants.ItemVendor_Fld_VendorItemID].ToString().Trim();
                    //                this.gridItemDetails.ActiveRow.Cells[clsPOSDBConstants.PODetail_Fld_Cost].Value = Configuration.convertNullToDecimal(oIVData.ItemVendor[0][clsPOSDBConstants.ItemVendor_Fld_VendorCostPrice].ToString().Trim());
                    //            }
                    //            else
                    //            {
                    //                // else we will insert the item into the Dictionary which will be used to 
                    //                // check the ItemId whose Last vendor is changed. 
                    //                // dictionary will be used while saving the PO 

                    //                this.gridItemDetails.ActiveRow.Cells[clsPOSDBConstants.PODetail_Fld_VendorItemCode].Value = oIVData.ItemVendor[0][clsPOSDBConstants.ItemVendor_Fld_VendorItemID].ToString().Trim();
                    //                this.gridItemDetails.ActiveRow.Cells[clsPOSDBConstants.PODetail_Fld_Cost].Value = Configuration.convertNullToDecimal(oIVData.ItemVendor[0][clsPOSDBConstants.ItemVendor_Fld_VendorCostPrice].ToString().Trim());

                    //                //if (dictIsChanged.ContainsKey(oItemRow.ItemID))
                    //                //{
                    //                //    dictIsChanged.Remove(oItemRow.ItemID);
                    //                //    dictIsChanged.Add(oItemRow.ItemID, true);
                    //                //}
                    //                //else
                    //                //{
                    //                //    dictIsChanged.Add(oItemRow.ItemID, true);
                    //                //}
                    //            }
                    //        }
                    //        else
                    //        {
                    //            clsUIHelper.ShowErrorMsg(" Item Is Deleted For The Vendor ");
                    //            this.gridItemDetails.ActiveRow.Cells[clsPOSDBConstants.PODetail_Fld_ItemID].Value = this.gridItemDetails.ActiveCell.Column.NullText;
                    //            this.gridItemDetails.ActiveRow.CancelUpdate();
                    //        }
                    //    }
                    //    else
                    //    {
                    //        //No item exist for Kinray
                    //        ItemVendorData itemVendorData = null;
                    //        //*Possible change
                    //        itemVendorData = PrimePOUtil.DefaultInstance.GetItemVendorData(oItemRow.ItemID, vendorCode);

                    //        if (itemVendorData != null && itemVendorData.ItemVendor.Rows.Count > 0)
                    //        {
                    //            ItemVendor oItemVendor = new ItemVendor();
                    //            POS_Core.BusinessRules.Vendor vender = new POS_Core.BusinessRules.Vendor();
                    //            //*Possible change
                    //            VendorData vendData = vender.Populate(vendorCode);
                    //            //itemVendorData.ItemVendor[0].VenorCostPrice = GetPriceForVendor(vendData.Vendor[0].PriceQualifier.ToString(), itemVendorData);

                    //            oItemVendor.Persist(itemVendorData);
                    //            this.gridItemDetails.ActiveRow.Cells[clsPOSDBConstants.PODetail_Fld_VendorItemCode].Value = itemVendorData.ItemVendor[0][clsPOSDBConstants.ItemVendor_Fld_VendorItemID].ToString().Trim();
                    //            this.gridItemDetails.ActiveRow.Cells[clsPOSDBConstants.PODetail_Fld_Cost].Value = Configuration.convertNullToDecimal(itemVendorData.ItemVendor[0][clsPOSDBConstants.ItemVendor_Fld_VendorCostPrice].ToString().Trim());

                    //            if (itemVendorData.ItemVendor[0].IsDeleted)
                    //            {
                    //                clsUIHelper.ShowErrorMsg(" Item Is Deleted For The Vendor ");
                    //                this.gridItemDetails.ActiveRow.Cells[clsPOSDBConstants.PODetail_Fld_ItemID].Value = this.gridItemDetails.ActiveCell.Column.NullText;
                    //                this.gridItemDetails.ActiveRow.CancelUpdate();
                    //            }
                    //        }
                    //        else
                    //        {
                    //            clsUIHelper.ShowErrorMsg(" No Item Exit For Vendor");
                    //            this.gridItemDetails.ActiveRow.Cells[clsPOSDBConstants.PODetail_Fld_ItemID].Value = this.gridItemDetails.ActiveCell.Column.NullText;
                    //            this.gridItemDetails.ActiveRow.CancelUpdate();

                    //        }
                    //    }
                    //}
                    #endregion
                    status = "SUCCESS";
                }
                catch (System.IndexOutOfRangeException)
                {
                    this.gridItemDetails.ActiveCell.Value = String.Empty;
                    this.gridItemDetails.ActiveRow.Cells["Description"].Value = String.Empty;
                }
                catch (Exception exp)
                {
                    logger.Fatal(exp, "FKEdit(string code, string senderName, string searchCriteria)");
                    clsUIHelper.ShowErrorMsg(exp.Message);

                    this.gridItemDetails.ActiveCell.Value = String.Empty;
                    this.gridItemDetails.ActiveRow.Cells["Description"].Value = String.Empty;
                }
            }
            return status;
        }
        /// <summary>
        /// Author: Prashant
        /// Date: 16-Jul-2009
        /// Description: Search item by PreferredVendor->LastVendor->DefaultVendor->OriginalVendor.
        /// </summary>
        /// <param name="ItemCode"></param>
        /// <returns></returns>
        private ItemVendorData GetTargetedVenorInfo(String itemCode)
        {
            Item item = new Item();
            ItemVendor itemVendor = new ItemVendor();
            ItemVendorData itemVendorData = null;
            DataRow[] vendorRows = null;
            string vendrorID = string.Empty;
            string lastOrderVendor = string.Empty;
            string preferredVendor = string.Empty;
            bool isItemExistForPrefVend = false;
            bool isItemExistForLastVend = false;
            bool isItemExistForDefVend = false;
            string sqlSelectPreferredVendor = "SELECT PREFERREDVENDOR FROM ITEM WHERE LTRIM(ITEMID) = '{0}'";
            string sqlSelectLastVendor = "SELECT LASTVENDOR FROM ITEM WHERE RTRIM(ITEMID) = '{0}'";
            Search oBLSearch = new Search();
            try
            {
                preferredVendor = oBLSearch.SearchScalar(String.Format(sqlSelectPreferredVendor, new object[] { itemCode }));
                lastOrderVendor = oBLSearch.SearchScalar(String.Format(sqlSelectLastVendor, new object[] { itemCode }));

                if (preferredVendor != string.Empty)
                {
                    vendorRows = vendorData.Vendor.Select("VendorCode ='" + preferredVendor + "'");
                    if (vendorRows.Length > 0)
                    {
                        vendrorID = vendorRows[0][clsPOSDBConstants.POHeader_Fld_VendorID].ToString();
                        itemVendorData = itemVendor.PopulateList(" AND LTRIM(RTRIM(" + clsPOSDBConstants.ItemVendor_tbl + ".ItemID)) = '" + itemCode + "' AND " + clsPOSDBConstants.ItemVendor_tbl + ".VendorID = " + vendrorID);
                        if (itemVendorData.ItemVendor.Rows.Count == 0)
                        {
                            isItemExistForPrefVend = false;
                        }
                        else
                        {
                            isItemExistForPrefVend = true;
                        }
                    }
                    else
                        isItemExistForPrefVend = false;
                }
                if (lastOrderVendor != string.Empty && !isItemExistForPrefVend)
                {
                    //vendorRows = vendorData.Vendor.Select("VendorCode ='" + preferredVendor + "'");
                    //vendrorID = vendorRows[0][clsPOSDBConstants.POHeader_Fld_VendorID].ToString();
                    POS_Core.BusinessRules.Vendor tempVendor = new POS_Core.BusinessRules.Vendor();

                    //Added by Atul Joshi on 20-11-2010 for to Check on VendorItemId not Availble for selected Id
                    itemVendorData = itemVendor.PopulateList(" AND " + clsPOSDBConstants.ItemVendor_tbl + ".ItemID = '" + itemCode + "' AND " + clsPOSDBConstants.ItemVendor_tbl + ".VendorId = '" + tempVendor.GetVendorId(lastOrderVendor).ToString() + "' And VendorItemId<>''");

                    if (itemVendorData.ItemVendor.Rows.Count == 0)
                    {
                        ItemVendorData DtItemVendor = new ItemVendorData();
                        DtItemVendor = itemVendor.PopulateList(" AND " + clsPOSDBConstants.ItemVendor_tbl + ".ItemID = '" + itemCode + "' AND " + clsPOSDBConstants.ItemVendor_tbl + ".VendorId = '" + tempVendor.GetVendorId(lastOrderVendor).ToString() + "' And VendorItemId=''");
                        if (DtItemVendor.ItemVendor.Rows.Count > 0)
                        {
                            clsUIHelper.ShowErrorMsg("VendorItemId is not available for selected ItemID");
                            return itemVendorData;
                        }
                    }
                    //End of Added by Atul Joshi on 20-11-2010 for to Check on VendorItemId not Availble for selected Id

                    itemVendorData = itemVendor.PopulateList(" AND " + clsPOSDBConstants.ItemVendor_tbl + ".ItemID = '" + itemCode + "' AND " + clsPOSDBConstants.ItemVendor_tbl + ".VendorId = '" + tempVendor.GetVendorId(lastOrderVendor).ToString() + "'");

                    if (itemVendorData.ItemVendor.Rows.Count == 0)
                    {
                        isItemExistForLastVend = false;
                    }
                    else
                    {
                        isItemExistForLastVend = true;
                    }
                }
                if (Configuration.CPrimeEDISetting.DefaultVendor != string.Empty && !isItemExistForLastVend && !isItemExistForPrefVend) //PRIMEPOS-3167 07-Nov-2022 JY Modified
                {
                    DataRow[] defaultVendorRows = vendorData.Vendor.Select("VendorCode ='" + Configuration.CPrimeEDISetting.DefaultVendor + "'");
                    if (defaultVendorRows.Length > 0)
                    {
                        String defaultVendorId = defaultVendorRows[0][clsPOSDBConstants.POHeader_Fld_VendorID].ToString();
                        itemVendorData = itemVendor.PopulateList(" AND LTRIM(RTRIM(" + clsPOSDBConstants.ItemVendor_tbl + ".ItemID)) = '" + itemCode + "' AND " + clsPOSDBConstants.ItemVendor_tbl + ".VendorID = " + defaultVendorId);
                        if (itemVendorData.ItemVendor.Rows.Count == 0)
                        {
                            isItemExistForDefVend = false;
                        }
                        else
                        {
                            isItemExistForDefVend = true;
                        }
                    }
                    else
                        isItemExistForDefVend = true;

                }
                if (!isItemExistForDefVend && !isItemExistForLastVend && !isItemExistForPrefVend)
                {
                    itemVendorData = itemVendor.PopulateList(" AND " + clsPOSDBConstants.ItemVendor_tbl + ".ItemID like '" + itemCode + "%'");
                }
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "GetTargetedVenorInfo(String itemCode)");
                clsUIHelper.ShowErrorMsg(ex.Message);
            }
            return itemVendorData;
        }
        /// <summary>
        /// Author: Gaurav
        /// Date: 03-Jul-2009
        /// Details: Item to check if allready ordered or not.
        /// </summary>
        /// <param name="ItemCode"></param>
        /// <returns></returns>
        private bool IsItemOrderedAllready(string ItemCode)
        {
            bool ItemAllreadyOrdered = false;
            string fetchItemQuery = "Select POD.ItemID as [ItemId],Itm.Description,POD.VendorItemCode as [Vendor ItemCode],PO.OrderID as [Order Id],PO.VendorID as [Vendor Id],vend.VendorName as [Vendor Name] ,OrderDate as [Order Date]," +
                                "case Status when 0 then 'Incomplete' when  1 then 'Pending' when 2 then 'Queued' when 3 then 'Submitted' when 4 then 'Canceled'" +
                                "when 5 then 'Acknowledge' when 6 then 'AcknowledgeManually' when 7 then 'MaxAttempt' when 8  then 'Processed' when 9  then 'Expired' when 10  then 'PartiallyAck' when 11 then 'PartiallyAck-Reorder'  when 12 then 'Error' when 16 then 'DeliveryReceived' when 17 then 'DirectDelivery' end as [PO Status]" +
                                "From PurchaseOrder as PO ,PurchaseOrderDetail as POD ,Item as Itm,Vendor as vend where Itm.ItemID = POD.ItemID AND PO.OrderID = POD.OrderID AND PO.VendorID=vend.VendorID";
            DataSet dataSet = null;
            DataTable dataTable = null;

            dataSet = new DataSet();
            Search newSearch = new Search();
            dataSet = newSearch.SearchData(fetchItemQuery);
            DataRow[] drs = dataSet.Tables[0].Select(clsPOSDBConstants.ItemId + " = '" + ItemCode + "'");
            if (drs != null && drs.Length > 0)
            {
                ItemAllreadyOrdered = true;
            }
            return (ItemAllreadyOrdered);
        }
        private void UpdateItemDetails(frmSearchMain oSearch)
        {
            string code = string.Empty;
            string vendorID = string.Empty;
            string bestVendor = string.Empty;
            string vendorName = string.Empty;
            string vendorItemCode = string.Empty;
            string vendorCostPrice = string.Empty;
            string bestVendorPrice = string.Empty;

            code = oSearch.SelectedRowID();
            vendorID = oSearch.SelectedVendorID();
            bestVendor = oSearch.SelectedBestVendor();
            vendorItemCode = oSearch.SelectedCode();
            bestVendorPrice = oSearch.SelectedBestVendorPrice();
            vendorCostPrice = oSearch.SelectedVendorCostPrice();
            this.vendorDetails.TryGetValue(vendorID, out vendorName);
            this.gridItemDetails.ActiveRow.Cells[clsPOSDBConstants.PODetail_Fld_BestVendor].Value = bestVendor;
            this.gridItemDetails.ActiveRow.Cells[clsPOSDBConstants.PODetail_Fld_BestVendPrice].Value = bestVendorPrice;
        }
        private void FindBestVendorANDPrice(String ItemID, out string bestVendor, out string bestPrice)
        {
            DataSet dsBestVendPrice = null;
            Search oBLSearch = new Search();
            string tempBestPrice = string.Empty;
            string tempBestVendor = string.Empty;
            String findBestPriceQuery = "Select VendorCostPrice,Vendor.VendorName from ItemVendor, Vendor where Vendor.VendorID=ItemVendor.VendorID AND ItemVendor.VendorCostPrice= (Select MIN(ItemVendor.VendorCostPrice) from ItemVendor where ItemID = '" + ItemID + "')" +
                                        "AND ItemID = '" + ItemID + "'";
            try
            {
                dsBestVendPrice = new DataSet();
                dsBestVendPrice = oBLSearch.SearchData(findBestPriceQuery);
                if (dsBestVendPrice.Tables.Count > 0)
                {
                    if (dsBestVendPrice.Tables[0].Rows.Count > 0)
                    {
                        tempBestVendor = dsBestVendPrice.Tables[0].Rows[0]["VendorName"].ToString().Trim();
                        tempBestPrice = dsBestVendPrice.Tables[0].Rows[0]["VendorCostPrice"].ToString().Trim();
                    }
                }
                this.gridItemDetails.Refresh();
                dsBestVendPrice = null;
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "FindBestVendorANDPrice(String ItemID, out string bestVendor, out string bestPrice)");
                clsUIHelper.ShowErrorMsg(ex.Message);
            }
            bestPrice = tempBestPrice;
            bestVendor = tempBestVendor;
        }
        private void FindBestVendorANDPrice(String ItemID)
        {
            //Modified by Atul Joshi on 20-11-2010
            String findBestPriceQuery = "Select VendorCostPrice,Vendor.VendorName from ItemVendor, Vendor where Vendor.VendorID=ItemVendor.VendorID AND ItemVendor.VendorCostPrice= (Select MIN(ItemVendor.VendorCostPrice) from ItemVendor where ItemID = '" + ItemID + "')" +
                                        "AND ItemID = '" + ItemID + "' And VendorItemID<>''";
            try
            {
                using (Search oBLSearch = new Search())
                {
                    using (DataSet dsBestVendPrice = oBLSearch.SearchData(findBestPriceQuery))
                    {
                        if (dsBestVendPrice.Tables.Count > 0)
                        {
                            if (dsBestVendPrice.Tables[0].Rows.Count > 0)
                            {
                                this.gridItemDetails.ActiveRow.Cells[clsPOSDBConstants.PODetail_Fld_BestVendor].Value = dsBestVendPrice.Tables[0].Rows[0]["VendorName"].ToString().Trim();
                                this.gridItemDetails.ActiveRow.Cells[clsPOSDBConstants.PODetail_Fld_BestVendPrice].Value = dsBestVendPrice.Tables[0].Rows[0]["VendorCostPrice"].ToString().Trim();
                            }
                        }
                    }
                }
                this.gridItemDetails.Refresh();
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "FindBestVendorANDPrice(String ItemID)");
                clsUIHelper.ShowErrorMsg(ex.Message);
            }
        }

        private void FormatGrid(string bestVendor, string vendorName)
        {
            try
            {
                if (bestVendor != string.Empty && vendorName != string.Empty)
                {
                    if (!bestVendor.Equals(vendorName, StringComparison.InvariantCultureIgnoreCase))
                    {
                        this.gridItemDetails.ActiveRow.Cells[clsPOSDBConstants.PODetail_Fld_BestVendor].Appearance.BackColorDisabled = Color.Red;
                        this.gridItemDetails.ActiveRow.Cells[clsPOSDBConstants.PODetail_Fld_BestVendPrice].Appearance.BackColorDisabled = Color.Red;
                        this.gridItemDetails.ActiveRow.Cells[clsPOSDBConstants.PODetail_Fld_BestVendor].Appearance.ForeColorDisabled = Color.Black;
                        this.gridItemDetails.ActiveRow.Cells[clsPOSDBConstants.PODetail_Fld_BestVendPrice].Appearance.ForeColorDisabled = Color.Black;
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "FormatGrid(string bestVendor, string vendorName)");
                clsUIHelper.ShowErrorMsg(ex.Message);
            }
        }

        private void gridItemDetails_ClickCellButton(object sender, CellEventArgs e)
        {
            SearchItem();
            //Commented By Amit Date 24 June 2011           
            //this.SelectNextControl(this.gridItemDetails, true, true, true, true);

        }
        private string SearchItem()
        {
            string valueInTheCell = string.Empty;
            string searchStatus = string.Empty;
            bool isInt = false;
            //Item oItem = new Item();
            //ItemVendor oItemVendor = new ItemVendor();
            try
            {
                UltraGridCell oCurrentCell;
                if (orderDetailData == null)
                {
                    orderDetailData = new OrderDetailsData();
                }

                oCurrentCell = this.gridItemDetails.ActiveCell;
                if (oCurrentCell.Column.Key != clsPOSDBConstants.PODetail_Fld_ItemID)
                    return searchStatus;
                valueInTheCell = oCurrentCell.Text;
                if (valueInTheCell == string.Empty)
                {
                    searchStatus = FKEdit(oCurrentCell.Text.ToString(), clsPOSDBConstants.Item_tbl, "UPC");
                    if (oCurrentCell.Text.ToString() == "")
                    {
                        oCurrentCell.CancelUpdate();
                        this.gridItemDetails.PerformAction(UltraGridAction.EnterEditMode);
                    }

                }
                else
                {
                    isInt = clsUIHelper.isNumeric(valueInTheCell);
                    if (isInt)
                    {
                        if (valueInTheCell.Length >= 11)
                        {
                            searchStatus = FKEdit(oCurrentCell.Text.ToString(), clsPOSDBConstants.Item_tbl, "UPC");
                            if (oCurrentCell.Text.ToString() == "")
                            {
                                oCurrentCell.CancelUpdate();
                                this.gridItemDetails.PerformAction(UltraGridAction.EnterEditMode);
                            }
                        }
                        else
                        {
                            searchStatus = FKEdit(oCurrentCell.Text.ToString(), clsPOSDBConstants.Item_tbl, "VENDOR_ITEM_CODE");
                            if (oCurrentCell.Text.ToString() == "")
                            {
                                oCurrentCell.CancelUpdate();
                                this.gridItemDetails.PerformAction(UltraGridAction.EnterEditMode);
                            }
                        }
                    }
                    else
                    {
                        searchStatus = FKEdit(oCurrentCell.Text.ToString(), clsPOSDBConstants.Item_tbl, "DESCRIPTION");
                        if (oCurrentCell.Text.ToString() == "")
                        {
                            oCurrentCell.CancelUpdate();
                            this.gridItemDetails.PerformAction(UltraGridAction.EnterEditMode);
                        }
                    }
                }
                if (oCurrentCell.Text.ToString() == "")
                {
                    oCurrentCell.CancelUpdate();
                    this.gridItemDetails.PerformAction(UltraGridAction.EnterEditMode);
                    return "";
                }
                //Added By Amit Date 25 June 2011
                if (this.gridItemDetails.ActiveRow != null)
                {
                    String tempQty = string.Empty;
                    String itemQty = String.Empty;
                    String itemPrice = String.Empty;

                    //this.gridItemDetails.ActiveRow.Cells[clsPOSDBConstants.PODetail_Fld_VendorName].DroppedDown = false;
                    itemQty = this.gridItemDetails.ActiveRow.Cells[clsPOSDBConstants.PODetail_Fld_QTY].Value.ToString().Trim();
                    tempQty = this.gridItemDetails.ActiveRow.Cells[clsPOSDBConstants.PODetail_Fld_QTY].Text.Trim();
                    itemPrice = this.gridItemDetails.ActiveRow.Cells[clsPOSDBConstants.PODetail_Fld_Price].Value.ToString().Trim();
                    if (tempQty != string.Empty)
                    {
                        if (itemQty != DBNull.Value.ToString())
                        {
                            //Added By SRT(Gaurav) Date : 10-Jul-2009
                            decimal tempItemPrice = 0;
                            int tempItemQty = 0;
                            if (decimal.TryParse(itemPrice, out tempItemPrice) && Int32.TryParse(itemQty, out tempItemQty))
                            {

                                this.gridItemDetails.ActiveRow.Cells[clsPOSDBConstants.PODetail_Fld_Cost].Value = tempItemQty * tempItemPrice;
                            }
                            //End Of Added By SRT(Gaurav)
                        }
                    }
                    else
                    {
                        this.gridItemDetails.ActiveCell = this.gridItemDetails.ActiveRow.Cells[clsPOSDBConstants.PODetail_Fld_QTY];
                        this.gridItemDetails.ActiveCell.Activate();
                        this.gridItemDetails.PerformAction(UltraGridAction.EnterEditMode);

                    }
                }
            }
            catch (Exception exp)
            {
                logger.Fatal(exp, "SearchItem()");
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
            return searchStatus;
        }

        private string SearchItem(out bool SearchReasult)
        {
            string valueInTheCell = string.Empty;
            string searchStatus = string.Empty;
            bool isInt = false;

            SearchReasult = true;
            try
            {
                UltraGridCell oCurrentCell;
                if (orderDetailData == null)
                {
                    orderDetailData = new OrderDetailsData();
                }

                oCurrentCell = this.gridItemDetails.ActiveCell;
                if (oCurrentCell.Column.Key != clsPOSDBConstants.PODetail_Fld_ItemID)
                    return searchStatus;
                valueInTheCell = oCurrentCell.Text;
                if (valueInTheCell == string.Empty)
                {
                    searchStatus = FKEdit(oCurrentCell.Text.ToString(), clsPOSDBConstants.Item_tbl, "UPC");
                    if (oCurrentCell.Text.ToString() == "")
                    {
                        oCurrentCell.CancelUpdate();
                        this.gridItemDetails.PerformAction(UltraGridAction.EnterEditMode);
                    }
                }
                else
                {
                    isInt = clsUIHelper.isNumeric(valueInTheCell);
                    if (isInt)
                    {
                        if (valueInTheCell.Length >= 11)
                        {
                            searchStatus = FKEdit(oCurrentCell.Text.ToString(), clsPOSDBConstants.Item_tbl, "UPC");
                            if (oCurrentCell.Text.ToString() == "")
                            {
                                oCurrentCell.CancelUpdate();
                                this.gridItemDetails.PerformAction(UltraGridAction.EnterEditMode);
                            }
                        }
                        else
                        {
                            searchStatus = FKEdit(oCurrentCell.Text.ToString(), clsPOSDBConstants.Item_tbl, "VENDOR_ITEM_CODE");
                            if (oCurrentCell.Text.ToString() == "")
                            {
                                oCurrentCell.CancelUpdate();
                                this.gridItemDetails.PerformAction(UltraGridAction.EnterEditMode);
                            }
                        }
                    }
                    else
                    {
                        searchStatus = FKEdit(oCurrentCell.Text.ToString(), clsPOSDBConstants.Item_tbl, "DESCRIPTION");
                        if (oCurrentCell.Text.ToString() == "")
                        {
                            oCurrentCell.CancelUpdate();
                            this.gridItemDetails.PerformAction(UltraGridAction.EnterEditMode);
                        }
                    }
                }
            }
            catch (Exception exp)
            {
                logger.Fatal(exp, "SearchItem()");
                clsUIHelper.ShowErrorMsg(exp.Message);
                SearchReasult = false;
            }
            //Added By SRT(Gaurav) Date : 14-Jul-2009
            //Added To Solve Focus ItemId Issue
            if (searchStatus.Trim().Length == 0 || searchStatus.Trim() == "OrdItem-NO" || searchStatus.Trim() == "ItemSearch-Canceled")
            {
                SearchReasult = false;
            }
            //End OF Added By SRT(Gaurav)
            return searchStatus;
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            logger.Trace("btnSave_Click() - " + clsPOSDBConstants.Log_Entering);
            //Added By SRT(Ritesh Parekh) Date : 19-Aug-2009
            //Checked if Grid count of orders is zero then raised error.
            if (this.gridPODetails.Rows.Count == 0)
            {
                clsUIHelper.ShowErrorMsg("There are no any Orders in the list");
                //Added By SRT(Ritesh Parekh) Date : 19-Aug-2009
                FocusNewItemId();
                //End Of Added By SRT(Ritesh Parekh)
                return;
            }
            //End Of Added By SRT(Ritesh Parekh)

            //Modified By SRT(Ritesh Parekh) Date : 19-Aug-2009
            //Updated to Check whether the active row is not null
            if (this.gridItemDetails.ActiveRow != null)
            {
                if (this.gridItemDetails.ActiveRow.Index > 0)
                {
                    this.gridItemDetails.Rows[0].Activate();
                }
            }
            //End Of Updated By SRT(Ritesh Parekh)          

            #region commented
            //String OrderNo = string.Empty;
            //String ordDescription = String.Empty; 
            //int lclVendorId = 0;
            //string lclVendorCode = string.Empty;
            //bool isCloseOrder = false;           
            //Int32 PartiallyAckOrderId = 0;
            //bool closeWithSave = false;
            //try
            //{
            //    if (this.gridPODetails.Rows.Count > 0)
            //    {
            //        for (int count = 0; count < gridPODetails.Rows.Count; count++)
            //        {
            //            closeWithSave = false;
            //            isCloseOrder = true;
            //            ordDescription = gridPODetails.Rows[count].Cells[clsPOSDBConstants.POHeader_Fld_Description].Text;
            //            OrderNo = gridPODetails.Rows[count].Cells["Order No"].Text;
            //            lclVendorId = (int)gridPODetails.Rows[count].Cells["VendorID"].Value;
            //            lclVendorCode = gridPODetails.Rows[count].Cells["Vendor Name"].Text;
            //            closeOrder(OrderNo,"",ordDescription, lclVendorId, lclVendorCode,isCloseOrder, PartiallyAckOrderId, closeWithSave);
            //        }
            //        //this.Close(); 
            //    }
            //    else
            //    {
            //        //show message box if there are no orders
            //    }
            //}
            //catch (Exception ex)
            //{
            //    clsUIHelper.ShowErrorMsg(ex.Message);
            //}
            #endregion
            bool closeWithSave = false;
            //frmShowMsgBox showOrderMsgBox = null;
            try
            {
                if (this.gridPODetails.Rows.Count == 0)
                {
                    this.Close();
                }

                //Added by SRT(Abhishek) Date : 26 Aug 2009
                string selectedPODetailIDFromVendorCombo = this.gridItemDetails.ActiveRow.Cells[clsPOSDBConstants.PODetail_Fld_PODetailID].Value.ToString();
                if (selectedPODetailIDFromVendorCombo == string.Empty || !IsPODetailExist(selectedPODetailIDFromVendorCombo))
                    this.gridItemDetails.ActiveRow.Delete(false);
                //End of Added by SRT(Abhishek) Date : 26 Aug 2009

                showOrderMsgBox = new frmShowMsgBox();
                showOrderMsgBox.ShowDialog();
                showOrderMsgBox.BringToFront();
                //isCloseOrder = true;
                logger.Trace("btnSave_Click() - " + showOrderMsgBox.SaveOrCloseOrder);
                if (showOrderMsgBox.SaveOrCloseOrder == clsPOSDBConstants.PO_Save)
                {
                    closeWithSave = false;
                }
                else if (showOrderMsgBox.SaveOrCloseOrder == clsPOSDBConstants.PO_SaveClose)
                {
                    closeWithSave = true;
                }
                else if (showOrderMsgBox.SaveOrCloseOrder == clsPOSDBConstants.PO_Cancel)
                {
                    //Added by SRT(Abhishek) Date : 27 Aug 2009
                    //Added to focus ItemID if process is canceled.
                    FocusNewItemId();
                    //End of Added by SRT(Abhishek) Date : 27 Aug 2009
                    return;
                }
                else if (showOrderMsgBox.SaveOrCloseOrder == clsPOSDBConstants.PO_DiscardChanges)
                {
                    this.Close();
                    return;
                }
                if (SendAllOrders(closeWithSave))
                    this.Close();
                logger.Trace("btnSave_Click() - " + clsPOSDBConstants.Log_Exiting);
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "btnSave_Click()");
                clsUIHelper.ShowErrorMsg(ex.Message);
            }
        }

        private void gridItemDetails_AfterSelectChange(object sender, AfterSelectChangeEventArgs e)
        {
            if (isEditPO)
            {
                if (masterOrderDetailsData.PODetailTable != null)
                {
                    this.txtEditorNoOfItems.Text = masterOrderDetailsData.PODetailTable.Rows.Count.ToString();
                }
                if (vendorBeforeSelection != string.Empty && selectedOrderIDFromVendorCombo != string.Empty)
                {
                    //Added by Prashant(SRT) Date:18-7-09
                    //Delete PODetails from databse                    
                    oPurchaseOrder.DeletePODetail(Convert.ToInt32(selectedPODetailIDFromVendorCombo));
                    //Delete POHeader from database 
                    DataRow[] poDetails = masterOrderDetailsData.PODetailTable.Select("OrderID = '" + selectedOrderIDFromVendorCombo + "'");
                    if (poDetails.Length == 0)
                    {
                        oPurchaseOrder.DeletePOHeader(Convert.ToInt32(selectedOrderIDFromVendorCombo));
                    }
                    //End of Added by Prashant(SRT) Date:18-7-09
                    CalculateTotals(vendorBeforeSelection, selectedOrderIDFromVendorCombo);

                    vendorBeforeSelection = string.Empty;
                    selectedOrderIDFromVendorCombo = string.Empty;
                }
            }
            else
            {
                if (orderDetailData.PODetailTable != null)
                {
                    this.txtEditorNoOfItems.Text = orderDetailData.PODetailTable.Rows.Count.ToString();
                }
            }
        }

        private void CalculateTotals(string vendorID)
        {
            DataRow[] ordDetailsRow = null;
            decimal totalCost = 0.00M;
            try
            {
                //Updated By SRT(Gaurav) Date: 16-Jul-2009
                //Updated By SRT(Prashant) Date: 17-Jul-2009
                if (orderDetailData == null || orderDetailData.PODetailTable == null) return;

                orderDetailData.AcceptChanges();
                DataRow[] dataRow = orderDetailData.PODetailTable.Select("VendorID = '" + vendorID + "'");
                if (dataRow.Length > 0)
                {
                    int totalItems = 0;
                    int totalQty = 0;
                    for (int count = 0; count < dataRow.Length; count++)
                    {
                        //Updated By SRT(Ritesh Parekh) Date: 22-Jul-2009
                        //Updated for handiling null value.
                        totalQty += Configuration.convertNullToInt(dataRow[count][clsPOSDBConstants.PODetail_Fld_QTY].ToString());
                        totalCost += Configuration.convertNullToDecimal(dataRow[count][clsPOSDBConstants.PODetail_Fld_Cost].ToString());
                    }
                    totalItems = dataRow.Length;
                    ordDetailsRow = orderDetailData.OrderDetailsTable.Select("vendorID = '" + vendorID + "'");
                    if (ordDetailsRow.Length > 0)
                    {
                        ordDetailsRow[0][clsPOSDBConstants.OrderDetail_Fld_TotalItems] = totalItems;
                        ordDetailsRow[0][clsPOSDBConstants.OrderDetail_Fld_TotalQty] = totalQty;
                        ordDetailsRow[0][clsPOSDBConstants.OrderDetail_Fld_TotalCost] = totalCost;
                    }
                }
                else if (dataRow.Length == 0)
                {
                    ordDetailsRow = orderDetailData.OrderDetailsTable.Select("vendorID = '" + vendorID + "'");
                    if (ordDetailsRow.Length > 0)
                    {
                        for (int count = 0; count < ordDetailsRow.Length; count++)
                        {
                            orderDetailData.OrderDetailsTable.Rows.Remove(ordDetailsRow[count]);
                        }
                    }
                }
                orderDetailData.AcceptChanges();
                this.gridPODetails.Refresh();
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "CalculateTotals(string vendorID)");
                clsUIHelper.ShowErrorMsg(ex.Message);
            }
        }

        private void CalculateTotals(string vendorID, string orderID)
        {
            DataRow[] ordDetailsRow = null;
            DataRow[] poDetailRow = null;
            DataRow[] masterDetailRow = null;
            int totalItems = 0;
            int totalQty = 0;
            decimal totalCost = 0.00M;
            try
            {
                masterOrderDetailsData.AcceptChanges();
                poDetailRow = masterOrderDetailsData.PODetailTable.Select("OrderID = '" + orderID + "'");
                if (poDetailRow.Length > 0)
                {
                    for (int count = 0; count < poDetailRow.Length; count++)
                    {
                        if (poDetailRow[count][clsPOSDBConstants.PODetail_Fld_QTY] != DBNull.Value)
                        {
                            totalQty += Convert.ToInt32(poDetailRow[count][clsPOSDBConstants.PODetail_Fld_QTY]);
                        }
                        if (poDetailRow[count][clsPOSDBConstants.PODetail_Fld_Cost] != DBNull.Value)
                        {
                            totalCost += Convert.ToDecimal(poDetailRow[count][clsPOSDBConstants.PODetail_Fld_Cost]);
                        }
                        totalItems = poDetailRow.Length;
                        ordDetailsRow = masterOrderDetailsData.OrderDetailsTable.Select("OrderID = '" + orderID + "'");
                        if (ordDetailsRow.Length > 0)
                        {
                            ordDetailsRow[0][clsPOSDBConstants.OrderDetail_Fld_TotalItems] = totalItems;
                            ordDetailsRow[0][clsPOSDBConstants.OrderDetail_Fld_TotalQty] = totalQty;
                            ordDetailsRow[0][clsPOSDBConstants.OrderDetail_Fld_TotalCost] = totalCost;
                        }
                        ordDetailsRow = masterOrderDetailsData.OrderDetailsTable.Select("VendorID = '" + vendorID + "'");
                        totalItems = ordDetailsRow.Length;
                        masterDetailRow = masterOrderDetailsData.MasterOrderDetailTable.Select("VendorID = '" + vendorID + "'");
                        if (masterDetailRow.Length > 0)
                        {
                            masterDetailRow[0][clsPOSDBConstants.MasterOrderDetails_Fld_TotalPOs] = totalItems;
                        }
                    }
                }
                else
                {
                    //Update count if order changes
                    string ordNO = string.Empty;

                    ordDetailsRow = masterOrderDetailsData.OrderDetailsTable.Select("OrderID = '" + orderID + "' AND VendorID = '" + vendorID + "'");
                    List<String> ordNos = null;

                    if (ordDetailsRow.Length > 0)
                    {
                        ordNO = ordDetailsRow[0][clsPOSDBConstants.OrderDetail_Fld_OrderNo].ToString();
                        poOrdersNos.TryGetValue(vendorID, out ordNos);
                        if (ordNos.Contains(ordNO))
                        {
                            ordNos.Remove(ordNO);
                        }
                        masterOrderDetailsData.OrderDetailsTable.Rows.Remove(ordDetailsRow[0]);
                        masterOrderDetailsData.OrderDetailsTable.AcceptChanges();
                    }
                    ordDetailsRow = masterOrderDetailsData.OrderDetailsTable.Select("VendorID = '" + vendorID + "'");
                    totalItems = ordDetailsRow.Length;
                    masterDetailRow = masterOrderDetailsData.MasterOrderDetailTable.Select("VendorID = '" + vendorID + "'");
                    if (totalItems == 0)
                    {
                        masterOrderDetailsData.MasterOrderDetailTable.Rows.Remove(masterDetailRow[0]);
                    }
                    else
                    {
                        masterDetailRow[0][clsPOSDBConstants.MasterOrderDetails_Fld_TotalPOs] = totalItems;
                    }
                }
                masterOrderDetailsData.MasterOrderDetailTable.AcceptChanges();
                this.gridPODetails.Refresh();
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "CalculateTotals(string vendorID, string orderID)");
                clsUIHelper.ShowErrorMsg(ex.Message);
            }
        }

        private void InitalizePODetails(string vendorID)
        {
            if (Configuration.convertNullToInt(vendorID.Trim()) > 0)
            {
                String orderDescription = String.Empty;
                String vendorName = string.Empty;
                try
                {
                    if (!orderDetailData.OrderDetailsTable.ChildRelations.Contains(clsPOSDBConstants.POItemRelationName))
                    {
                        MakeRelation();
                    }
                    this.vendorDetails.TryGetValue(vendorID, out vendorName);
                    DataRow[] orderRows = orderDetailData.OrderDetailsTable.Select("VendorID = " + vendorID);
                    if (orderRows.Length == 0)
                    {
                        //Updated By SRT(Gaurav) Date : 15-Jul-2009
                        //GetOrderDescription to set true allways by calling method of InitializePODetails to get this code execute.
                        if (!isAutoPO && GetOrderDescription && !useBestVendor)
                        {
                            using (frmOrderDetails frmOrderDetails = new frmOrderDetails())
                            {
                                frmOrderDetails.lblTransactionType.Text += vendorName;
                                frmOrderDetails.ShowDialog(this);
                                orderDescription = frmOrderDetails.OrderDescription;
                            }
                        }
                        else if (useBestVendor)
                        {
                            orderDescription = "Use Best Vendor";
                        }
                        orderNumber++;

                        OrderDetailRow orderDetailRow = orderDetailData.OrderDetailsTable.NewOrderDetailRow();
                        orderDetailRow.OrderNo = orderNumber;
                        orderDetailRow.OrderDescription = orderDescription;
                        orderDetailRow.VendorID = vendorID.Trim().Length > 0 ? Convert.ToInt32(vendorID) : 0;
                        orderDetailRow.VendorName = vendorName.Trim();
                        orderDetailData.OrderDetailsTable.AddRow(orderDetailRow);

                        this.gridPODetails.UpdateData();
                        this.gridPODetails.Refresh();
                    }
                    //this.gridPODetails.DisplayLayout.Bands[1].Columns[clsPOSDBConstants.PODetail_Fld_Cost].Format = "####.00";    //PRIMEPOS-3155 06-Oct-2022 JY Commented
                }
                catch (Exception ex)
                {
                    logger.Fatal(ex, "InitalizePODetails(string vendorID)");
                    clsUIHelper.ShowErrorMsg(ex.Message);
                }
            }
        }

        private void InitalizeMasterOrderDetails(string vendorID, string orderId, string orderDescription, bool isChangeVendor)
        {
            MasterOrderDetailsRow masterOrderDetailRow = null;
            OrderDetailRow orderDetailRow = null;
            DataRow[] vendorRows = null;
            DataRow[] orderRows = null;
            String vendorName = string.Empty;
            String timeToOrder = string.Empty;
            String isAutoClose = string.Empty;
            try
            {
                if (!masterOrderDetailsData.MasterOrderDetailTable.ChildRelations.Contains(clsPOSDBConstants.MasterPOItemRelationName))
                {
                    BindRelations();
                    this.ApplyGridFormatForEdit();
                    //ApplyGrigFormat();
                }
                this.vendorDetails.TryGetValue(vendorID, out vendorName);
                vendorRows = VendorData.Vendor.Select("vendorID = '" + vendorID + "'");
                timeToOrder = vendorRows[0][clsPOSDBConstants.OrderDetail_Tbl_TimeToOrder].ToString();
                isAutoClose = vendorRows[0][clsPOSDBConstants.OrderDetail_Tbl_AuroSend].ToString();

                masterOrderDetailRow = masterOrderDetailsData.MasterOrderDetailTable.NewOrderDetailRow();
                masterOrderDetailRow.VendorID = vendorID.Trim().Length > 0 ? Convert.ToInt32(vendorID) : 0;
                masterOrderDetailRow.VendorName = vendorName.Trim();
                masterOrderDetailRow.TimeToOrder = timeToOrder;

                masterOrderDetailsData.MasterOrderDetailTable.AddRow(masterOrderDetailRow);


                masterOrderDetailsData.OrderDetailsTable.PrimaryKey = new DataColumn[] { masterOrderDetailsData.OrderDetailsTable.OrderID };

                orderDetailRow = masterOrderDetailsData.OrderDetailsTable.NewOrderDetailRow();
                orderDetailRow.OrderNo = orderNumber;
                orderDetailRow.OrderDescription = orderDescription;
                orderDetailRow.OrderID = orderId.Trim().Length > 0 ? Convert.ToInt32(orderId) : 0;
                orderDetailRow.VendorID = vendorID.Trim().Length > 0 ? Convert.ToInt32(vendorID) : 0;
                orderDetailRow.VendorName = vendorName.Trim();
                masterOrderDetailsData.OrderDetailsTable.AddRow(orderDetailRow);
                this.gridPODetails.Refresh();

            }
            catch (Exception ex)
            {
                //clsUIHelper.ShowErrorMsg(ex.Message);
            }
        }
        private void PopulatePODetails(string vendorID, int poIndex, string poNumber)
        {
            DataRow[] ordDetailsRow = null;
            DataRow[] vendorRows = null;
            string itemID = string.Empty;
            string vendorName = string.Empty;
            string timeToOrder = string.Empty;
            string isAutoClose = string.Empty;
            try
            {

                itemID = this.orderDetailData.PODetailTable.Rows[poIndex][clsPOSDBConstants.PODetail_Fld_ItemID].ToString();
                if (itemID == string.Empty)
                    return;

                vendorName = this.orderDetailData.PODetailTable.Rows[poIndex][clsPOSDBConstants.PODetail_Fld_VendorName].ToString();
                ordDetailsRow = orderDetailData.OrderDetailsTable.Select("vendorID = '" + vendorID + "'");
                vendorRows = VendorData.Vendor.Select("vendorID = '" + vendorID + "'");

                timeToOrder = vendorRows[0][clsPOSDBConstants.OrderDetail_Tbl_TimeToOrder].ToString();
                isAutoClose = vendorRows[0][clsPOSDBConstants.OrderDetail_Tbl_AuroSend].ToString();

                if (ordDetailsRow.Length > 0)
                {
                    ordDetailsRow[0][clsPOSDBConstants.OrderDetail_Fld_VendorName] = vendorName;
                    ordDetailsRow[0][clsPOSDBConstants.OrderDetail_Fld_AuroSend] = isAutoClose;
                    ordDetailsRow[0][clsPOSDBConstants.OrderDetail_Fld_TimeToOrder] = timeToOrder.Trim() != string.Empty ? timeToOrder.ToString() : DateTime.Now.ToString(); ;
                    CalculateTotals(vendorID);
                }
                this.gridPODetails.UpdateData();
            }
            catch (Exception ex)
            {
                //clsUIHelper.ShowErrorMsg(ex.Message);
            }
        }
        private void PopulateMasterPODetails(string vendorID, int poIndex, string poNumber, string orderId)
        {
            DataRow[] ordDetailsRow = null;
            DataRow[] vendorRows = null;
            string itemID = string.Empty;
            string vendorName = string.Empty;
            string timeToOrder = string.Empty;
            string isAutoClose = string.Empty;
            try
            {
                itemID = this.masterOrderDetailsData.PODetailTable.Rows[poIndex][clsPOSDBConstants.PODetail_Fld_ItemID].ToString();
                if (itemID == string.Empty)
                    return;

                vendorName = this.masterOrderDetailsData.PODetailTable.Rows[poIndex][clsPOSDBConstants.PODetail_Fld_VendorName].ToString();
                ordDetailsRow = masterOrderDetailsData.OrderDetailsTable.Select("OrderId = '" + orderId + "'");
                vendorRows = VendorData.Vendor.Select("vendorID = '" + vendorID + "'");

                timeToOrder = vendorRows[0][clsPOSDBConstants.OrderDetail_Tbl_TimeToOrder].ToString();
                isAutoClose = vendorRows[0][clsPOSDBConstants.OrderDetail_Tbl_AuroSend].ToString();

                if (ordDetailsRow.Length > 0)
                {
                    ordDetailsRow[0][clsPOSDBConstants.OrderDetail_Tbl_CloseOrder] = "Send PO(F12)";
                    ordDetailsRow[0][clsPOSDBConstants.OrderDetail_Fld_OrderNo] = Convert.ToInt32(poNumber); ;
                    ordDetailsRow[0][clsPOSDBConstants.OrderDetail_Fld_VendorName] = vendorName;
                    ordDetailsRow[0][clsPOSDBConstants.OrderDetail_Fld_AuroSend] = isAutoClose;
                    ordDetailsRow[0][clsPOSDBConstants.OrderDetail_Fld_TimeToOrder] = timeToOrder.Trim() != string.Empty ? timeToOrder.ToString() : DateTime.Now.ToString(); ;
                }
                this.gridPODetails.UpdateData();
            }
            catch (Exception ex)
            {
                //clsUIHelper.ShowErrorMsg(ex.Message);
            }
        }

        private void FillPODetails(string vendorID)
        {
            String defaultTimeToOrder = "2.00 PM";
            try
            {
                string itemID = this.gridItemDetails.ActiveRow.Cells[clsPOSDBConstants.PODetail_Fld_ItemID].Value.ToString();
                if (itemID == string.Empty)
                    return;

                string vendorName = this.gridItemDetails.ActiveRow.Cells[clsPOSDBConstants.PODetail_Fld_VendorName].Value.ToString();
                if (vendorName == string.Empty)
                {
                    vendorDetails.TryGetValue(vendorID, out vendorName);
                }

                DataRow[] ordDetailsRow = orderDetailData.OrderDetailsTable.Select("vendorID = '" + vendorID + "'");
                DataRow[] vendorRows = VendorData.Vendor.Select("vendorID = '" + vendorID + "'");

                string timeToOrder = vendorRows[0][clsPOSDBConstants.OrderDetail_Tbl_TimeToOrder].ToString();
                string isAutoClose = vendorRows[0][clsPOSDBConstants.OrderDetail_Tbl_AuroSend].ToString();

                if (ordDetailsRow.Length > 0)
                {
                    ordDetailsRow[0][clsPOSDBConstants.OrderDetail_Fld_VendorName] = vendorName;
                    ordDetailsRow[0][clsPOSDBConstants.OrderDetail_Fld_AuroSend] = isAutoClose;
                    ordDetailsRow[0][clsPOSDBConstants.OrderDetail_Fld_TimeToOrder] = timeToOrder.Trim() != string.Empty ? timeToOrder.ToString() : defaultTimeToOrder;
                    if (!isAutoPO)
                        CalculateTotals(vendorID);  //PRIMEPOS-3155 06-Oct-2022 JY Added if condition
                }

                this.gridPODetails.Refresh();
                this.gridPODetails.UpdateData();
                // this.isFilledPODetails = true;
                if (!isAutoPO)
                {
                    if (!isShowOrderedItems)
                        FocusNewItemId();
                }
            }
            catch (Exception ex)
            {
                //clsUIHelper.ShowErrorMsg(ex.Message);
            }
        }

        private void btnCreateForAllVendors_Click(object sender, EventArgs e)
        {
            bool closeWithSave = false;
            bool isCloseOrder = false;
            string orderDescription = string.Empty;
            string orderID = string.Empty;
            string OrderNo = string.Empty;
            string lclVendorCode = string.Empty;
            string errorMsg = string.Empty;
            String msgString = "Do you want to Send ";

            try
            {
                if (this.gridPODetails.Rows.Count == 0)
                {
                    errorMsg = "There are no any Orders in the list";
                    clsUIHelper.ShowErrorMsg(errorMsg);
                    //Added By SRT(Ritesh Parekh) Date : 19-Aug-2009
                    FocusNewItemId();
                    //End Of Added By SRT(Ritesh Parekh)
                    return;
                }
                if (this.gridItemDetails.ActiveRow.Index > 0)
                {
                    this.gridItemDetails.Rows[0].Activate();
                }
                #region commented
                //for (int cntRow = 0; cntRow < gridPODetails.Rows.Count; cntRow++)
                //{
                //    gridPODetails.ActiveRow = gridPODetails.Rows[cntRow];
                //    if (isEditPO)
                //    {
                //        totalOrders = this.gridPODetails.ActiveRow.ChildBands[0].Rows.Count;
                //        for (int count = 0; count < totalOrders; count++)
                //        {
                //            OrderNo = this.gridPODetails.ActiveRow.ChildBands[0].Rows[count].Cells[clsPOSDBConstants.OrderDetail_Fld_OrderNo].Value.ToString();
                //            lclVendorId = (int)this.gridPODetails.ActiveRow.ChildBands[0].Rows[count].Cells["VendorID"].Value;
                //            lclVendorCode = this.gridPODetails.ActiveRow.ChildBands[0].Rows[count].Cells["Vendor Name"].Text;
                //            orderDescription = this.gridPODetails.ActiveRow.ChildBands[0].Rows[count].Cells[clsPOSDBConstants.POHeader_Fld_Description].Text;

                //            orderID = this.gridPODetails.ActiveRow.ChildBands[0].Rows[count].Cells["OrderID"].Text;
                //            closeOrder(OrderNo, orderID,orderDescription, lclVendorId, lclVendorCode, isCloseOrder, PartiallyAckOrderId, closeWithSave);
                //        }
                //    }
                //    else
                //    {
                //        if (gridPODetails.ActiveRow != null)
                //        {
                //            OrderNo = gridPODetails.ActiveRow.Cells["Order No"].Text;
                //            lclVendorId = (int)gridPODetails.ActiveRow.Cells["VendorID"].Value;
                //            lclVendorCode = gridPODetails.ActiveRow.Cells["Vendor Name"].Text;
                //            isCloseOrder = true;
                //            closeOrder(OrderNo, "",orderDescription, lclVendorId, lclVendorCode, isCloseOrder, PartiallyAckOrderId, closeWithSave);
                //        }
                //    }
                //}
                #endregion
                isCloseOrder = true;

                //Added by SRT(Abhishek) Date : 26 Aug 2009
                //this part will clear the row if podetail id does not exist 
                string selectedPODetailIDFromVendorCombo = this.gridItemDetails.ActiveRow.Cells[clsPOSDBConstants.PODetail_Fld_PODetailID].Value.ToString();
                if (selectedPODetailIDFromVendorCombo == string.Empty || !IsPODetailExist(selectedPODetailIDFromVendorCombo))
                    this.gridItemDetails.ActiveRow.Delete(false);
                //End of Added by SRT(Abhishek) Date : 26 Aug 2009

                if (Resources.Message.Display(msgString + " all the orders ?", "Close Purchase Order", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    closeWithSave = true;
                }
                else
                {
                    closeWithSave = false;
                    //Added by SRT(Abhishek) Date : 27 Aug 2009
                    //Added to focus new ItemID 
                    //if send all orders is set to false.
                    FocusNewItemId();
                    //End of Added by SRT(Abhishek) Date : 27 Aug 2009
                    return;
                }
                //Updated By SRT(Gaurav) Date: 09-Jul-2009
                if (SendAllOrders(closeWithSave))
                    this.Close();
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "btnCreateForAllVendors_Click(object sender, EventArgs e)");
                clsUIHelper.ShowErrorMsg(ex.Message);
            }
        }

        private bool SendAllOrders(bool closeWithSave)
        {
            logger.Trace("SendAllOrders(bool closeWithSave) - " + clsPOSDBConstants.Log_Entering);

            if (GetSelectedItemDisplayFilter() != POItemDisplayFilter.All)
                DeleteFilteredOutItems(IsToExcludeFilteredItems(), GetSelectedItemDisplayFilter());

            int lclVendorId = 0;
            int PartiallyAckOrderId = 0;
            int totalOrders = 0;
            bool isCloseOrder = false;
            string vendorCode = string.Empty;
            string orderDescription = string.Empty;
            string orderID = string.Empty;
            string OrderNo = string.Empty;
            string lclVendorCode = string.Empty;
            string errorMsg = string.Empty;
            bool isClose = false;
            string currentCell = string.Empty;

            try
            {
                //Added By SRT(Ritesh Parekh) Date : 21-07-2009
                foreach (UltraGridRow uRow in gridItemDetails.Rows)
                {
                    if (Configuration.convertNullToInt(uRow.Cells["Qty"].Value.ToString()) <= 0)
                    {
                        //throw (new Exception("One or more item(s) has invalid quantity.\nPlease set desired quantity."));
                        clsUIHelper.ShowErrorMsg("One or more item(s) has invalid quantity.\nPlease set desired quantity.");
                        gridItemDetails.Rows[uRow.Index].Activate();
                        FocusToCell("Qty");
                        //POS_Core.ErrorLogging.Logs.Logger("** SendAllOrders(bool** ) One or more item(s) has invalid quantity.\nPlease set desired quantity.");
                        logger.Trace("SendAllOrders(bool closeWithSave) - One or more item(s) has invalid quantity.\nPlease set desired quantity.");
                        return false;
                    }
                }
                //End Of Added By SRT(Ritesh Parekh)

                if (isEditPO)
                {
                    orderDetailData = new OrderDetailsData();
                    orderDetailData.PODetailTable = masterOrderDetailsData.PODetailTable;
                }
                vendorCode = this.gridItemDetails.ActiveRow.Cells[clsPOSDBConstants.PODetail_Fld_VendorName].Value.ToString();

                DataRow[] poDetails = orderDetailData.PODetailTable.Select("VendorName ='" + vendorCode.Trim().Replace("'", "''") + "'"); //PRIMEPOS-3104 23-Jun-2022 JY modified

                if (poDetails.Length == 0)
                {
                    errorMsg = "The item details is not updated for \"" + vendorCode.ToUpper() + "\" in grid. \nThis operation will be ignored";
                    clsUIHelper.ShowErrorMsg(errorMsg);

                    for (int count = 0; count < this.gridPODetails.Rows.Count; count++)
                    {
                        string poVendorName = this.gridPODetails.Rows[count].Cells[clsPOSDBConstants.OrderDetail_Fld_VendorName].Value.ToString();
                        if (poVendorName == vendorCode)
                        {
                            this.gridPODetails.Rows[count].Delete(false);
                            orderDetailData.AcceptChanges();
                            break;
                        }
                    }
                    return (false);
                }
                for (int cntRow = 0; cntRow < gridPODetails.Rows.Count; cntRow++)
                {
                    gridPODetails.ActiveRow = gridPODetails.Rows[cntRow];

                    if (isEditPO)
                    {
                        totalOrders = this.gridPODetails.ActiveRow.ChildBands[0].Rows.Count;
                        for (int count = 0; count < totalOrders; count++)
                        {
                            OrderNo = this.gridPODetails.ActiveRow.ChildBands[0].Rows[count].Cells[clsPOSDBConstants.OrderDetail_Fld_OrderNo].Value.ToString();
                            lclVendorId = (int)this.gridPODetails.ActiveRow.ChildBands[0].Rows[count].Cells["VendorID"].Value;
                            lclVendorCode = this.gridPODetails.ActiveRow.ChildBands[0].Rows[count].Cells["Vendor Name"].Text;
                            orderDescription = this.gridPODetails.ActiveRow.ChildBands[0].Rows[count].Cells[clsPOSDBConstants.POHeader_Fld_Description].Text;

                            orderID = this.gridPODetails.ActiveRow.ChildBands[0].Rows[count].Cells["OrderID"].Text;
                            isClose = closeOrder(OrderNo, orderID, orderDescription, lclVendorId, lclVendorCode, isCloseOrder, PartiallyAckOrderId, closeWithSave);

                        }
                    }
                    else
                    {
                        if (gridPODetails.ActiveRow != null)
                        {

                            OrderNo = gridPODetails.ActiveRow.Cells["Order No"].Text;
                            lclVendorId = (int)gridPODetails.ActiveRow.Cells["VendorID"].Value;
                            lclVendorCode = gridPODetails.ActiveRow.Cells["Vendor Name"].Text;
                            orderDescription = this.gridPODetails.ActiveRow.Cells[clsPOSDBConstants.POHeader_Fld_Description].Text;
                            isCloseOrder = true;
                            isClose = closeOrder(OrderNo, "", orderDescription, lclVendorId, lclVendorCode, isCloseOrder, PartiallyAckOrderId, closeWithSave);

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "SendAllOrders(bool closeWithSave)");
                clsUIHelper.ShowErrorMsg(ex.Message);
                //POS_Core.ErrorLogging.Logs.Logger(ex.Message);
                return (false);
            }
            logger.Trace("SendAllOrders(bool closeWithSave) - " + clsPOSDBConstants.Log_Exiting);
            return (isClose);
        }

        private int _recurssionCount;

        /// <summary>
        /// Auther : Shrikant Mali
        /// Date : 02/10/2014
        /// Deletes the items which are filtered out by Item selection filter depending on case or not case items.
        /// </summary>
        /// <param name="isToExcludeFilteredItems">True to deletes the items.</param>
        /// <param name="filter">User selected Item.</param>
        private void DeleteFilteredOutItems(bool isToExcludeFilteredItems, POItemDisplayFilter filter)
        {
            if (!isToExcludeFilteredItems)
                return;

            try
            {
                var ultraGridRows = filter == POItemDisplayFilter.OnlyCaseItems
                                                ? gridItemDetails.Rows.Where(IsNonCaseItem)
                                                : gridItemDetails.Rows.Where(IsCaseItem);

                foreach (var row in ultraGridRows)
                {
                    DeleteGridRow(false, row);
                }
            }
            catch (InvalidOperationException)
            {
                if (_recurssionCount == 50)
                    return;

                _recurssionCount++;
                // ToDo Need to find proper cause of this issue and solve the issue properly!
                // Added by Shrikant Mali on 17/02/2014
                // While iterating through the grid item details rows and deleting items, it crashes
                // with invalide operation exception, but if we iterate again, then it works, the 
                // cause of this is not known so, calling the function recursively to fix it 
                DeleteFilteredOutItems(isToExcludeFilteredItems, filter);
                _recurssionCount = 0;
            }
        }

        /// <summary>
        /// Auther : Shrikan Mali
        /// Date : 10/02/2014
        /// Gets user's choise whether to all the Items in Item details or only the items which are selected using 
        /// Item selection filter.
        /// </summary>
        /// <returns>True if user wants to use all the items; otherwise false.</returns>
        private bool IsToExcludeFilteredItems()
        {
            return
                Resources.Message.Display(
                    string.Format("Are you sure you want to exclude the hidden {0} from the list for PO?",
                                  GetSelectedItemDisplayFilter() == POItemDisplayFilter.OnlyCaseItems
                        ? "Non Case Items"
                        : "Case Items"),
                    "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Question) ==
                DialogResult.Yes;
        }

        private bool Save(String OrderNo, String orderID, String orderDescription, int VendorId, string VendorCode, bool isCloseOrder, Int32 PartiallyAckOrderId, out int PartiallyAckStatus, out bool IsCancelled, out bool IsPartiallyAcknowledge)
        {
            bool retValue = false;
            IsPartiallyAcknowledge = false;
            IsCancelled = false;
            PartiallyAckStatus = 0;
            oPurchaseOrder = new PurchaseOrder();
            //POS_Core.ErrorLogging.Logs.Logger("**Start Save Purchase Order");
            logger.Trace("Save(....) - Start Save Purchase Order - " + clsPOSDBConstants.Log_Entering);
            try
            {
                oPOHeaderData.POHeader = new POHeaderTable();
                oPODetailData = new PODetailData();
                if (isEditPO)
                {
                    oPOHeaderRow = oPOHeaderData.POHeader.AddRow(Convert.ToInt32(orderID), "", System.DateTime.MinValue, System.DateTime.MinValue, 0, 0);
                }
                else
                {
                    oPOHeaderRow = oPOHeaderData.POHeader.AddRow(oPOHeaderData.POHeader.Rows.Count, "", System.DateTime.MinValue, System.DateTime.MinValue, 0, 0);
                }
                oPOHeaderRow.VendorCode = VendorCode;
                oPOHeaderRow.OrderNo = OrderNo;
                oPOHeaderRow.VendorID = VendorId;
                oPOHeaderRow.OrderDate = DateTime.Now;
                oPOHeaderRow.ExptDelvDate = DateTime.Now;
                oPOHeaderRow.Description = orderDescription;
                PartiallyAckStatus = oPOHeaderRow.Status;
                //ErrorLogging.Logs.Logger(" OrderNo :" + OrderNo.ToString() + " Vendor Code" + VendorCode.ToString());
                if (isAckManual)
                    oPOHeaderRow.Status = (int)PurchseOrdStatus.AcknowledgeManually;
                else
                    oPOHeaderRow.Status = (int)PurchseOrdStatus.Incomplete;

                if (isEditPO)
                {
                    if (!newlyAddedOrderIDs.Contains(orderID))
                    {
                        oPOHeaderRow.AcceptChanges();
                        oPOHeaderRow.SetModified();
                    }
                }
                oPOHeaderData.POHeader.AddRow(oPOHeaderRow);
                oPOHeaderData.Tables.Remove("PurchaseOrder");
                oPOHeaderData.Tables.Add(oPOHeaderData.POHeader);

                oPODetailData.PODetail = new PODetailTable();
                if (isEditPO)
                {
                    DataRow[] podetailsRows = orderDetailData.PODetailTable.Select("OrderID=" + orderID);
                    int count = 0;
                    foreach (PODetailRow dr in podetailsRows)
                    {
                        oPODetailData.PODetail.AddRow(dr.VendorID.ToString(), dr.VendorName, dr.VendorItemCode, Configuration.convertNullToInt(dr.BestPrice), dr.BestVendor, 0, 0,
                            dr.PODetailID, dr.OrderID, Configuration.convertNullToInt(dr.QTY.ToString()),
                            Configuration.convertNullToDecimal(dr.Price.ToString()), dr.ItemID.ToString(), dr.Comments.ToString(),
                            dr.ItemDescription.ToString(), Configuration.convertNullToDecimal(dr.Price.ToString()));
                        if (!newlyAddedPOIDs.Contains(dr.PODetailID.ToString()))
                        {
                            oPODetailData.PODetail.Rows[count].AcceptChanges();
                            oPODetailData.PODetail.Rows[count].SetModified();
                        }
                        count++;
                    }
                    if (podetailsRows.Length == 0)
                    {
                        oPOHeaderData.POHeader[0].AcceptChanges();
                        oPOHeaderData.POHeader[0].Delete();
                    }
                }
                else
                {
                    foreach (PODetailRow dr in orderDetailData.PODetailTable.Select("VendorID =" + VendorId))
                    {
                        oPODetailData.PODetail.AddRow(dr.VendorID.ToString(), dr.VendorName, dr.VendorItemCode, Configuration.convertNullToInt(dr.BestPrice), dr.BestVendor, 0, 0,
                        dr.PODetailID, dr.OrderID, Configuration.convertNullToInt(dr.QTY.ToString()),
                        Configuration.convertNullToDecimal(dr.Price.ToString()), dr.ItemID.ToString(), dr.Comments.ToString(),
                        dr.ItemDescription.ToString(), Configuration.convertNullToDecimal(dr.Price.ToString()));// change by ravindra for PRIMEPOS-10431 sub-taskShow COST from purchase order(850) against the acknowledgement(855\810)
                    }
                }
                oPODetailData.Tables.Remove("PurchaseOrderDetail");
                oPODetailData.Tables.Add(oPODetailData.PODetail);

                oPurchaseOrder.Persist(oPOHeaderData, oPODetailData);
                LastVendorChange(VendorId.ToString(), oPODetailData.PODetail);
                try
                {
                    //POS_Core.ErrorLogging.Logs.Logger(" OrderNo :" + OrderNo.ToString() + " Vendor Code" + VendorCode.ToString() + "No of Item order" + oPODetailData.PODetail.Rows.Count);
                    logger.Trace("Save(....) - OrderNo: " + OrderNo.ToString() + " Vendor Code: " + VendorCode.ToString() + " No of Item order: " + oPODetailData.PODetail.Rows.Count);
                }
                catch { }
                retValue = true;
                System.Windows.Forms.Application.DoEvents();
            }
            catch (POSExceptions exp)
            {
                //POS_Core.ErrorLogging.Logs.Logger("Save Purchase Order" + exp.ErrMessage);
                logger.Fatal(exp, "Save(....) - 1");
                clsUIHelper.ShowErrorMsg(exp.ErrMessage);
                retValue = false;
            }
            catch (Exception exp)
            {
                logger.Fatal(exp, "Save(....) - 2");
                clsUIHelper.ShowErrorMsg(exp.Message);
                //POS_Core.ErrorLogging.Logs.Logger("Save Purchase Order" + exp.Message);
                retValue = false;
            }
            //POS_Core.ErrorLogging.Logs.Logger("**End Save Purchase Order");
            logger.Trace("Save(....) - " + clsPOSDBConstants.Log_Exiting);
            return retValue;
        }

        private bool Save()
        {
            return (true);
        }
        private void UpdateStatus(Int32 PartiallyAckOrderId, out bool IsPartiallyAcknowledge)
        {
            IsPartiallyAcknowledge = false;
            try
            {
                oPOHeaderData = oPurchaseOrder.PopulateHeader(Convert.ToInt32(PartiallyAckOrderId));
                oPOHeaderRow = (POHeaderRow)oPOHeaderData.POHeader.Rows[0];
                oPODetailData = oPurchaseOrder.PopulateDetail(Convert.ToInt32(PartiallyAckOrderId));
                oPOHeaderData.POHeader[0].Status = (int)PurchseOrdStatus.PartiallyAckReorder;
                if (oPOHeaderRow.Status == (int)PurchseOrdStatus.PartiallyAckReorder)
                    IsPartiallyAcknowledge = true;
                oPurchaseOrder.Persist(oPOHeaderData, oPODetailData);
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "UpdateStatus(Int32 PartiallyAckOrderId, out bool IsPartiallyAcknowledge)");
                clsUIHelper.ShowErrorMsg(ex.Message);
            }
        }
        private void SendPO()
        {
            try
            {
                //POS_Core.ErrorLogging.Logs.Logger("Start SendPO() Send Purchase Order to PrimeEDI ");
                logger.Trace("SendPO() - Send Purchase Order to PrimeEDI - " + clsPOSDBConstants.Log_Entering);
                int isSend = PrimePOUtil.DefaultInstance.SendPO(oPOHeaderData, oPODetailData);

                if (isSend == PrimePOUtil.ERROR)
                {
                    oPOHeaderData.POHeader[0].Status = (int)PurchseOrdStatus.Pending; //0;
                    oPurchaseOrder.Persist(oPOHeaderData, oPODetailData);

                    ClsUpdatePOStatus.UpdateStatusInst.UpdatePOCount();
                    //POS_Core.ErrorLogging.Logs.Logger("Purchase Order  Order " + " Order No. " + oPOHeaderData.POHeader[0].OrderNo.ToString() + " Status: " + PurchseOrdStatus.Pending.ToString() + "No of Iteme Order" + oPODetailData.Tables[0].Rows.Count.ToString());
                    logger.Trace("SendPO() - Purchase No: " + oPOHeaderData.POHeader[0].OrderNo.ToString() + " Status: " + PurchseOrdStatus.Pending.ToString() + "No of Items Order: " + oPODetailData.Tables[0].Rows.Count.ToString());
                }
                else if (isSend == PrimePOUtil.SUCCESS)
                {
                    oPOHeaderData.POHeader[0].Status = (int)PurchseOrdStatus.Queued;//2;
                    oPurchaseOrder.Persist(oPOHeaderData, oPODetailData);
                    ClsUpdatePOStatus.UpdateStatusInst.UpdatePOCount();
                    //POS_Core.ErrorLogging.Logs.Logger("Purchase Order  Order " + " Order No. " + oPOHeaderData.POHeader[0].OrderNo.ToString() + " Status: " + PurchseOrdStatus.Queued.ToString() + "No of Iteme Order" + oPODetailData.Tables[0].Rows.Count.ToString());
                    logger.Trace("SendPO() - Order No: " + oPOHeaderData.POHeader[0].OrderNo.ToString() + " Status: " + PurchseOrdStatus.Queued.ToString() + "No of Items Order: " + oPODetailData.Tables[0].Rows.Count.ToString());
                }
                else if (isSend == PrimePOUtil.MANUAL)
                {
                    oPOHeaderData.POHeader[0].Status = (int)PurchseOrdStatus.AcknowledgeManually; //5
                    oPurchaseOrder.Persist(oPOHeaderData, oPODetailData);

                    ClsUpdatePOStatus.UpdateStatusInst.FillLogDataSet(" Order No. " + oPOHeaderData.POHeader[0].OrderNo.ToString() + " w  ");
                    ClsUpdatePOStatus.UpdateStatusInst.UpdatePOCount();
                    //POS_Core.ErrorLogging.Logs.Logger("Purchase Order  Order " + " Order No. " + oPOHeaderData.POHeader[0].OrderNo.ToString() + " Status: " + PurchseOrdStatus.AcknowledgeManually.ToString() + "No of Iteme Order" + oPODetailData.Tables[0].Rows.Count.ToString());
                    logger.Trace("SendPO() - Order No: " + oPOHeaderData.POHeader[0].OrderNo.ToString() + " Status: " + PurchseOrdStatus.AcknowledgeManually.ToString() + "No of Items Order: " + oPODetailData.Tables[0].Rows.Count.ToString());
                }
                else if (isSend == PrimePOUtil.PODISCOONECT)
                {
                    clsUIHelper.ShowErrorMsg(" PrimePO is Disconnected ");

                    oPOHeaderData.POHeader[0].Status = (int)PurchseOrdStatus.Pending; //0;
                    oPurchaseOrder.Persist(oPOHeaderData, oPODetailData);
                    ClsUpdatePOStatus.UpdateStatusInst.UpdatePOCount();
                    //POS_Core.ErrorLogging.Logs.Logger("Purchase Order  Order " + " Order No. " + oPOHeaderData.POHeader[0].OrderNo.ToString() + " Status: " + PurchseOrdStatus.Pending.ToString() + "No of Iteme Order" + oPODetailData.Tables[0].Rows.Count.ToString());
                    logger.Trace("SendPO() - Order No: " + oPOHeaderData.POHeader[0].OrderNo.ToString() + " Status: " + PurchseOrdStatus.Pending.ToString() + "No of Items Order: " + oPODetailData.Tables[0].Rows.Count.ToString());
                }
                clsUIHelper.PrintPurchaseOrder(this.oPOHeaderRow.OrderID.ToString(), false, true);
                IsReportCalled = true;
                logger.Trace("SendPO() - Send Purchase Order to PrimeEDI - " + clsPOSDBConstants.Log_Exiting);
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "SendPO()");
                oPOHeaderData.POHeader[0].Status = (int)PurchseOrdStatus.Pending;
                oPurchaseOrder.Persist(oPOHeaderData, oPODetailData);
                ClsUpdatePOStatus.UpdateStatusInst.FillLogDataSet(" Purchse Order -" + oPOHeaderData.POHeader[0].OrderNo + " is Pending ");
                ClsUpdatePOStatus.UpdateStatusInst.UpdatePOCount();
                //POS_Core.ErrorLogging.Logs.Logger("**SendPO()  Purchase Order Order Status: " + ex.Message);
            }
        }
        private void LastVendorChange(string vendorID, PODetailTable dtPoDetail)
        {
            POS_Core.BusinessRules.Vendor vendor = new POS_Core.BusinessRules.Vendor();
            try
            {
                foreach (PODetailRow row in dtPoDetail.Rows)
                {
                    Item oItem = new Item();
                    ItemData oItemData;
                    ItemRow oItemRow = null;

                    oItemData = oItem.Populate(row.ItemID.Trim());
                    oItemRow = oItemData.Item[0];
                    //Updated By SRT(Ritesh Parekh) Date : 27-Jul-2009
                    //Updated for last vendor to save in code format.
                    oItemRow.LastVendor = vendor.GetVendorCode(Convert.ToString(vendorID));//Convert.ToString(vendorID);
                    //End Of Updated By SRT(Ritesh Parekh)
                    oItem.Persist(oItemData);
                }
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "LastVendorChange(string vendorID, PODetailTable dtPoDetail)");
                throw (ex);
            }
        }
        private bool closeOrder(String OrderNo, String orderID, String orderDescription, int VendorId, string VendorCode, bool isCloseOrder, Int32 PartiallyAckOrderId, bool closeWithSave)
        {
            int PartiallyAckStatus = 0;
            bool IsCancelled = false;
            bool IsPartiallyAcknowledge = false;
            string partiallAckPOID = string.Empty;
            bool isClose = false;

            try
            {
                bool IsInComplete = false;
                if (Save(OrderNo, orderID, orderDescription, VendorId, VendorCode, isCloseOrder, PartiallyAckOrderId, out PartiallyAckStatus, out IsCancelled, out IsPartiallyAcknowledge))
                {
                    Edit(oPOHeaderData.POHeader.Rows[0][clsPOSDBConstants.POHeader_Fld_OrderID].ToString(), out IsInComplete);

                    if (closeWithSave)
                    {
                        string items = string.Empty;
                        VendorData oVData = new VendorData();
                        POS_Core.BusinessRules.Vendor oVen = new POS_Core.BusinessRules.Vendor();
                        oVData = oVen.Populate(oPOHeaderRow.VendorCode);
                        VendorRow oRow = oVData.Vendor[0];

                        if (oRow.USEVICForEPO)
                        {
                            SendPO();
                        }
                        else
                        {
                            if ((Resources.Message.Display("Purchase Order " + OrderNo + " Can Not Be Send Electronically For Vendor " + oRow.Vendorname.Trim() + ".\n" + oRow.Vendorname.Trim() + " does not support electronic purchase orders.\nDo You Want to Process it Manually ?\n", "Electronic PO", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes))
                            {
                                oPOHeaderData.POHeader[0].Status = (int)PurchseOrdStatus.SubmittedManually;
                                oPurchaseOrder.Persist(oPOHeaderData, oPODetailData);
                            }
                            clsUIHelper.PrintPurchaseOrder(this.oPOHeaderRow.OrderID.ToString(), false, false);
                            //Added By sRT(Ritesh Parekh) date: 29-Aug-2009
                            IsReportCalled = true;
                        }
                        if (IsPartiallyAck)
                        {
                            partiallyAckPOIDs.TryGetValue(orderID.ToString(), out partiallAckPOID);
                            UpdateStatus(Convert.ToInt32(partiallAckPOID), out IsPartiallyAcknowledge);
                        }
                    }
                    //added to display report for the incomplete order
                    //added by SRT(Abhishek)  29/07/2009   
                    else if (IsInComplete)
                    {
                        Application.DoEvents();
                        clsUIHelper.PrintPurchaseOrder(this.oPOHeaderRow.OrderID.ToString(), false, true);
                        //Added By sRT(Ritesh Parekh) date: 29-Aug-2009
                        IsReportCalled = true;
                    }
                    isClose = true;
                }
            }
            catch (Exception exp)
            {
                isClose = false;
                try
                {
                    oPOHeaderData.POHeader[0].Status = 0;
                    oPurchaseOrder.Persist(oPOHeaderData, oPODetailData);
                }
                catch (Exception ex) { }
            }
            return isClose;
        }

        public void Edit(string POID)
        {
            bool isTrue = false;
            try
            {

                oPOHeaderData = oPurchaseOrder.PopulateHeader(Convert.ToInt32(POID));
                oPOHeaderRow = (POHeaderRow)oPOHeaderData.POHeader.Rows[0];

                if (IsInComplete)
                {
                    if (oPOHeaderRow.Status == (int)PurchseOrdStatus.Incomplete)
                    {
                        isTrue = true;
                        totalNoOfPOs++;
                    }
                }
                else if (IsPartiallyAck)
                {
                    isTrue = true;
                }
                else if (IsCopyOrder)
                {
                    isTrue = true;
                }
                if (poHeaderList != null && isTrue == true)
                {
                    poHeaderList.Add(POID, oPOHeaderData);
                }
            }
            catch (Exception exp)
            {
                logger.Fatal(exp, "Edit(string POID)");
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }
        #region commented
        //public void FillInCompleteOrders(POHeaderRow poHeaderRow,String orderNo)
        //{
        //    string POID = string.Empty;
        //    string podID = string.Empty; 
        //    string vendorID = string.Empty;
        //    string orderID = string.Empty;
        //    string bestVendor = string.Empty;
        //    string bestPrice = string.Empty;
        //    string itemID = string.Empty; 
        //    DataRow headerRow = null;
        //    DataRow orderRow = null;
        //    POHeaderData poheaderData = null;
        //    PODetailRow poDetailRow = null;
        //    int remQty = 0; 
        //    try
        //    {
        //        POID = poHeaderRow.OrderID.ToString();               
        //        oPODetailData = oPurchaseOrder.PopulateDetail(Convert.ToInt32(POID));
        //        vendorID = poHeaderRow.VendorID.ToString();
        //        //No need as there is param orderID.
        //        orderID = oPODetailData.PODetail.Rows[0][clsPOSDBConstants.POHeader_Fld_OrderID].ToString();
        //        //orderID <-> POID for PartiallyAck.
        //        poHeaderList.TryGetValue(orderID, out poheaderData);             
        //        headerRow = poheaderData.POHeader.Rows[0];             

        //        //InitalizeMasterOrderDetails(vendorID, headerRow[clsPOSDBConstants.OrderDetail_Tbl_OrderId].ToString());
        //        InitalizeMasterOrderDetails(vendorID, headerRow[clsPOSDBConstants.OrderDetail_Tbl_OrderId].ToString());
        //        for (int count = 0; count < oPODetailData.PODetail.Rows.Count; count++)
        //        {
        //            orderRow = oPODetailData.PODetail.Rows[count];
        //            poDetailRow = masterOrderDetailsData.PODetailTable.NewPODetailRow();
        //            podID = orderRow[clsPOSDBConstants.PODetail_Fld_PODetailID].ToString();
        //            itemID = orderRow[clsPOSDBConstants.PODetail_Fld_ItemID].ToString();
        //            //Order ID need to be increment for Partially Ack Order.
        //            //FillDictionaries(podID, itemID, vendorID, orderNo, poHeaderRow.OrderID.ToString());
        //            FillDictionaries(podID, itemID, vendorID, orderNo, poHeaderRow.OrderID.ToString());
        //            poDetailRow[clsPOSDBConstants.PODetail_Fld_PODetailID] = orderRow[clsPOSDBConstants.PODetail_Fld_PODetailID].ToString();
        //            poDetailRow[clsPOSDBConstants.POHeader_Fld_OrderID] = orderRow[clsPOSDBConstants.POHeader_Fld_OrderID].ToString();
        //            poDetailRow[clsPOSDBConstants.PODetail_Fld_ItemID] = orderRow[clsPOSDBConstants.PODetail_Fld_ItemID].ToString();
        //            poDetailRow[clsPOSDBConstants.Description] = orderRow[clsPOSDBConstants.Item_Fld_Description].ToString();
        //            poDetailRow[clsPOSDBConstants.PODetail_Fld_Price] = orderRow[clsPOSDBConstants.PODetail_Fld_Cost].ToString();
        //            if (IsPartiallyAck)
        //            {
        //                remQty = (int)orderRow[clsPOSDBConstants.PODetail_Fld_QTY] - (int)orderRow[clsPOSDBConstants.PODetail_Fld_AckQTY];
        //                poDetailRow[clsPOSDBConstants.PODetail_Fld_QTY] = remQty.ToString();
        //            }
        //            else
        //            {
        //                poDetailRow[clsPOSDBConstants.PODetail_Fld_QTY] = orderRow[clsPOSDBConstants.PODetail_Fld_QTY].ToString();
        //            }
        //            poDetailRow[clsPOSDBConstants.PODetail_Fld_VendorItemCode] = orderRow[clsPOSDBConstants.PODetail_Fld_VendorItemCode].ToString();
        //            poDetailRow[clsPOSDBConstants.POHeader_Fld_VendorID] = orderRow[clsPOSDBConstants.POHeader_Fld_VendorID].ToString();
        //            poDetailRow[clsPOSDBConstants.OrderDetail_Tbl_VendorName] = headerRow[clsPOSDBConstants.OrderDetail_Tbl_VendorName].ToString();

        //            FindBestVendorANDPrice(itemID, out bestVendor, out bestPrice);
        //            poDetailRow[clsPOSDBConstants.PODetail_Fld_BestVendor] = bestVendor;
        //            poDetailRow[clsPOSDBConstants.PODetail_Fld_BestVendPrice] = bestPrice;
        //            this.haveVendorInfo = true;
        //            masterOrderDetailsData.PODetailTable.AddRow(poDetailRow);
        //        }
        //        //this.gridItemDetails.UpdateData(); 
        //    }
        //    catch (Exception ex)
        //    {
        //        clsUIHelper.ShowErrorMsg(ex.Message);
        //    }            
        //}
        #endregion
        public void FillInCompleteOrders(POHeaderRow poHeaderRow, String orderNo, String orderID)
        {
            string POID = string.Empty;
            string podID = string.Empty;
            string vendorID = string.Empty;
            string orderDescription = string.Empty;
            string bestVendor = string.Empty;
            string bestPrice = string.Empty;
            string itemID = string.Empty;
            DataRow headerRow = null;
            DataRow orderRow = null;
            POHeaderData poheaderData = null;
            PODetailRow poDetailRow = null;
            int remQty = 0;
            try
            {
                if (IsCopyOrder == false)
                    orderNumber = Configuration.convertNullToInt(poHeaderRow.OrderNo);  //PRIMEPOS-2825 02-Apr-2020 JY Added

                POID = poHeaderRow.OrderID.ToString();
                orderDescription = poHeaderRow.Description;

                oPODetailData = oPurchaseOrder.PopulateDetail(Convert.ToInt32(POID));

                vendorID = poHeaderRow.VendorID.ToString();

                poHeaderList.TryGetValue(POID, out poheaderData);

                headerRow = poheaderData.POHeader.Rows[0];

                InitalizeMasterOrderDetails(vendorID, orderID, orderDescription, false);

                for (int count = 0; count < oPODetailData.PODetail.Rows.Count; count++)
                {
                    orderRow = oPODetailData.PODetail.Rows[count];

                    //Added by SRT(Abhishek) Date : 11/09/2009
                    //Added to check condition if order is Partilly Ack Order, if it's PartAck then  
                    //items displayed for newly generated order will be item whose 
                    //remaining quantity to be acknowledged is greater than zero 
                    if (IsPartiallyAck)
                    {
                        remQty = (int)orderRow[clsPOSDBConstants.PODetail_Fld_QTY] - (int)orderRow[clsPOSDBConstants.PODetail_Fld_AckQTY];
                        if (remQty <= 0)
                            continue;
                    }
                    //End of Added by SRT(Abhishek) Date : 11/09/2009
                    poDetailRow = masterOrderDetailsData.PODetailTable.NewPODetailRow();
                    podID = orderRow[clsPOSDBConstants.PODetail_Fld_PODetailID].ToString();
                    itemID = orderRow[clsPOSDBConstants.PODetail_Fld_ItemID].ToString();

                    FillDictionaries(podID, itemID, vendorID, orderNo, orderID);

                    poDetailRow[clsPOSDBConstants.PODetail_Fld_PODetailID] = orderRow[clsPOSDBConstants.PODetail_Fld_PODetailID].ToString();
                    poDetailRow[clsPOSDBConstants.POHeader_Fld_OrderID] = orderID;
                    poDetailRow[clsPOSDBConstants.PODetail_Fld_ItemID] = orderRow[clsPOSDBConstants.PODetail_Fld_ItemID].ToString();
                    poDetailRow[clsPOSDBConstants.Description] = orderRow[clsPOSDBConstants.Item_Fld_Description].ToString();
                    poDetailRow[clsPOSDBConstants.PODetail_Fld_Price] = orderRow[clsPOSDBConstants.PODetail_Fld_Cost].ToString();
                    if (IsPartiallyAck)
                    {
                        remQty = (int)orderRow[clsPOSDBConstants.PODetail_Fld_QTY] - (int)orderRow[clsPOSDBConstants.PODetail_Fld_AckQTY];
                        poDetailRow[clsPOSDBConstants.PODetail_Fld_QTY] = remQty.ToString();
                    }
                    else
                    {
                        poDetailRow[clsPOSDBConstants.PODetail_Fld_QTY] = orderRow[clsPOSDBConstants.PODetail_Fld_QTY].ToString();
                    }
                    poDetailRow[clsPOSDBConstants.PODetail_Fld_VendorItemCode] = orderRow[clsPOSDBConstants.PODetail_Fld_VendorItemCode].ToString();
                    poDetailRow[clsPOSDBConstants.POHeader_Fld_VendorID] = orderRow[clsPOSDBConstants.POHeader_Fld_VendorID].ToString();
                    poDetailRow[clsPOSDBConstants.OrderDetail_Tbl_VendorName] = headerRow[clsPOSDBConstants.PODetail_Fld_VendorCode].ToString();

                    FindBestVendorANDPrice(itemID, out bestVendor, out bestPrice);
                    poDetailRow[clsPOSDBConstants.PODetail_Fld_BestVendor] = bestVendor;
                    poDetailRow[clsPOSDBConstants.PODetail_Fld_BestVendPrice] = bestPrice;
                    this.haveVendorInfo = true;
                    if (IsPartiallyAck)
                    {
                        if (!partiallyAckPOIDs.Keys.Contains(orderID))
                        {
                            partiallyAckPOIDs.Add(orderID, POID);
                        }
                        newlyAddedPOIDs.Add(podID);
                    }
                    else if (isCopyOrder)
                    {
                        newlyAddedPOIDs.Add(podID);
                    }

                    #region Sprint-21 - 1803 11-Sep-2015 JY Added code as few POdetail cells are not updating previously
                    try
                    {
                        poDetailRow[clsPOSDBConstants.PODetail_Fld_PackSize] = orderRow[clsPOSDBConstants.PODetail_Fld_PackSize].ToString();
                        poDetailRow[clsPOSDBConstants.PODetail_Fld_PackQuant] = orderRow[clsPOSDBConstants.PODetail_Fld_PackQuant].ToString();
                        poDetailRow[clsPOSDBConstants.PODetail_Fld_PackUnit] = orderRow[clsPOSDBConstants.PODetail_Fld_PackUnit].ToString();
                    }
                    catch { }
                    #endregion

                    masterOrderDetailsData.PODetailTable.AddRow(poDetailRow);
                }
                this.gridPODetails.Refresh();
                this.gridItemDetails.Refresh();
                if (IsCopyOrder == true)
                    orderNumber = Convert.ToInt32(poHeaderRow.OrderNo);     //PRIMEPOS-2825 02-Apr-2020 JY Added
                ApplyGrigFormat();

                #region Sprint-21 - 1803 14-Sep-2015 JY Added
                for (int i = 0; i < gridItemDetails.Rows.Count; i++)
                {
                    if ((gridItemDetails.Rows[i].Cells[clsPOSDBConstants.PODetail_Fld_PackUnit].Value.ToString().ToUpper().Trim() == clsPOSDBConstants.PckUnit_CS) || (gridItemDetails.Rows[i].Cells[clsPOSDBConstants.PODetail_Fld_PackUnit].Value.ToString().ToUpper().Trim() == clsPOSDBConstants.PckUnit_CA))   //Sprint-21 22-Feb-2016 JY Added CA for case item
                    {
                        if (MMSUtil.UtilFunc.ValorZeroDEC(gridItemDetails.Rows[i].Cells[clsPOSDBConstants.PODetail_Fld_PackSize].Value.ToString()) > 0)
                        {
                            gridItemDetails.Rows[i].Appearance.BackColor = Color.Yellow;
                        }
                        else
                        {
                            gridItemDetails.Rows[i].Appearance.BackColor = Color.Red;
                        }
                    }

                    #region this code is added the highlight the best price in red color in incomplete order same as new order
                    string compVendor = string.Empty;
                    DataRow[] vendorRow = VendorData.Vendor.Select(" VendorID=" + gridItemDetails.Rows[i].Cells[clsPOSDBConstants.POHeader_Fld_VendorID].Value.ToString());
                    if (vendorRow != null && vendorRow.Length > 0)
                        compVendor = vendorRow[0].ItemArray[2].ToString();
                    bestVendor = gridItemDetails.Rows[i].Cells["Best Vendor"].Value.ToString();

                    if (bestVendor != string.Empty && compVendor != string.Empty)
                    {
                        if (!bestVendor.Equals(compVendor, StringComparison.InvariantCultureIgnoreCase))
                        {
                            gridItemDetails.Rows[i].Cells[clsPOSDBConstants.PODetail_Fld_BestVendor].Appearance.BackColorDisabled = Color.Red;
                            gridItemDetails.Rows[i].Cells[clsPOSDBConstants.PODetail_Fld_BestVendPrice].Appearance.BackColorDisabled = Color.Red;
                            gridItemDetails.Rows[i].Cells[clsPOSDBConstants.PODetail_Fld_BestVendor].Appearance.ForeColorDisabled = Color.Black;
                            gridItemDetails.Rows[i].Cells[clsPOSDBConstants.PODetail_Fld_BestVendPrice].Appearance.ForeColorDisabled = Color.Black;
                        }
                    }
                    #endregion
                }
                #endregion
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "FillInCompleteOrders(POHeaderRow poHeaderRow, String orderNo, String orderID)");
                clsUIHelper.ShowErrorMsg(ex.Message);
            }
        }
        private void FillDictionaries(string podID, string itemID, string vendorID, string orderNo, string orderID)
        {
            try
            {
                if (!poOrdersNos.ContainsKey(vendorID))
                {
                    orderNOs = new List<string>();
                    poOrdersNos.Add(vendorID, orderNOs);
                    orderNOs.Add(orderNo);
                }
                else
                {
                    poOrdersNos.TryGetValue(vendorID, out orderNOs);
                    if (!orderNOs.Contains(orderNo))
                    {
                        orderNOs.Add(orderNo);
                    }
                }
                if (!podIdOrdernoDict.ContainsKey(podID))
                {
                    podIdOrdernoDict.Add(podID, orderNo);
                }
                else
                {

                }
                if (!orderNoOrderIdDict.ContainsKey(orderNo))
                {
                    orderNoOrderIdDict.Add(orderNo, orderID);
                }
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "FillDictionaries(string podID, string itemID, string vendorID, string orderNo, string orderID)");
                clsUIHelper.ShowErrorMsg(ex.Message);
            }
        }
        public void Edit(string POID, out bool IsInComplete)
        {
            IsInComplete = false;
            try
            {
                oPOHeaderData = oPurchaseOrder.PopulateHeader(Convert.ToInt32(POID));
                oPOHeaderRow = (POHeaderRow)oPOHeaderData.POHeader.Rows[0];
                oPODetailData = oPurchaseOrder.PopulateDetail(Convert.ToInt32(POID));

                if (oPOHeaderRow.Status == (int)PurchseOrdStatus.Incomplete)
                    IsInComplete = true;
            }
            catch (Exception exp)
            {
                logger.Fatal(exp, "Edit(string POID, out bool IsInComplete)");
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }

        public bool IsCanceled
        {
            get
            {
                return isCanceled;
            }
        }
        public bool IsInComplete
        {
            set { isIncomplete = value; }
            get { return isIncomplete; }
        }
        public bool IsPartiallyAck
        {
            set { isPartiallyAck = value; }
            get { return isPartiallyAck; }
        }
        public bool IsCopyOrder
        {
            set { isCopyOrder = value; }
            get { return isCopyOrder; }
        }
        public bool IsFlaggedOrder
        {
            set { isFlaggedOrder = value; }
            get { return isFlaggedOrder; }
        }

        private void FormatGrid(string criteria)
        {
            try
            {
                if (criteria == "EDIT")
                {
                    //gridPODetails
                    this.gridPODetails.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.OrderDetail_Fld_CloseOrder].Header.Caption = "Send Order";
                    this.gridPODetails.DisplayLayout.Bands[1].Columns[clsPOSDBConstants.POHeader_Fld_VendorID].Hidden = true;
                    this.gridPODetails.DisplayLayout.Bands[1].Columns[clsPOSDBConstants.OrderDetail_Tbl_OrderId].Hidden = true;
                    this.gridPODetails.DisplayLayout.Bands[1].Columns[clsPOSDBConstants.OrderDetail_Fld_VendorName].Hidden = true;

                    this.gridPODetails.DisplayLayout.Bands[1].Columns[clsPOSDBConstants.OrderDetail_Tbl_CloseOrder].Header.Caption = "Send Order";
                    this.gridPODetails.DisplayLayout.Bands[1].Columns[clsPOSDBConstants.OrderDetail_Tbl_CloseOrder].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.Button;
                    this.gridPODetails.DisplayLayout.Bands[1].Columns[clsPOSDBConstants.OrderDetail_Tbl_CloseOrder].ButtonDisplayStyle = Infragistics.Win.UltraWinGrid.ButtonDisplayStyle.Always;

                    this.gridPODetails.DisplayLayout.Bands[2].Columns[clsPOSDBConstants.PODetail_Fld_OrderID].Hidden = true;
                    this.gridPODetails.DisplayLayout.Bands[2].Columns[clsPOSDBConstants.PODetail_Fld_PODetailID].Hidden = true;
                    this.gridPODetails.DisplayLayout.Bands[2].Columns[clsPOSDBConstants.PODetail_Fld_AckQTY].Hidden = true;
                    this.gridPODetails.DisplayLayout.Bands[2].Columns[clsPOSDBConstants.PODetail_Fld_AckStatus].Hidden = true;
                    this.gridPODetails.DisplayLayout.Bands[2].Columns[clsPOSDBConstants.PODetail_Fld_ChangedProductID].Hidden = true;
                    this.gridPODetails.DisplayLayout.Bands[2].Columns[clsPOSDBConstants.PODetail_Fld_ChangedProductQualifier].Hidden = true;

                    this.gridPODetails.DisplayLayout.Bands[2].Columns[clsPOSDBConstants.POHeader_Fld_VendorID].Hidden = true;
                    this.gridPODetails.DisplayLayout.Bands[2].Columns[clsPOSDBConstants.PODetail_Fld_VendorName].Hidden = true;
                    this.gridPODetails.DisplayLayout.Bands[2].Columns[clsPOSDBConstants.PODetail_Fld_BestVendor].Hidden = true;
                    this.gridPODetails.DisplayLayout.Bands[2].Columns[clsPOSDBConstants.PODetail_Fld_BestVendPrice].Hidden = true;
                    this.gridPODetails.DisplayLayout.Bands[2].Columns[clsPOSDBConstants.PODetail_Fld_LastOrdVendor].Hidden = true;
                    this.gridPODetails.DisplayLayout.Bands[2].Columns[clsPOSDBConstants.PODetail_Fld_LastOrdQty].Hidden = true;
                }
                else
                {
                    this.gridPODetails.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.PODetail_Fld_OrderID].Hidden = true;
                    this.gridPODetails.DisplayLayout.Bands[1].Columns[clsPOSDBConstants.PODetail_Fld_QtyInStock].Hidden = true;
                    this.gridPODetails.DisplayLayout.Bands[1].Columns[clsPOSDBConstants.PODetail_Fld_ReOrderLevel].Hidden = true;
                    this.gridPODetails.DisplayLayout.Bands[1].Columns[clsPOSDBConstants.PODetail_Fld_QtySold100Days].Hidden = true;
                    this.gridPODetails.DisplayLayout.Bands[1].Columns[clsPOSDBConstants.PODetail_Fld_OrderID].Hidden = true;
                    this.gridPODetails.DisplayLayout.Bands[1].Columns[clsPOSDBConstants.PODetail_Fld_PODetailID].Hidden = true;
                    this.gridPODetails.DisplayLayout.Bands[1].Columns[clsPOSDBConstants.PODetail_Fld_AckQTY].Hidden = true;
                    this.gridPODetails.DisplayLayout.Bands[1].Columns[clsPOSDBConstants.PODetail_Fld_AckStatus].Hidden = true;
                    this.gridPODetails.DisplayLayout.Bands[1].Columns[clsPOSDBConstants.PODetail_Fld_ChangedProductID].Hidden = true;
                    this.gridPODetails.DisplayLayout.Bands[1].Columns[clsPOSDBConstants.PODetail_Fld_ChangedProductQualifier].Hidden = true;

                    this.gridPODetails.DisplayLayout.Bands[1].Columns[clsPOSDBConstants.POHeader_Fld_VendorID].Hidden = true;
                    this.gridPODetails.DisplayLayout.Bands[1].Columns[clsPOSDBConstants.PODetail_Fld_VendorName].Hidden = true;
                    this.gridPODetails.DisplayLayout.Bands[1].Columns[clsPOSDBConstants.PODetail_Fld_BestVendor].Hidden = true;
                    this.gridPODetails.DisplayLayout.Bands[1].Columns[clsPOSDBConstants.PODetail_Fld_BestVendPrice].Hidden = true;
                    this.gridPODetails.DisplayLayout.Bands[1].Columns[clsPOSDBConstants.PODetail_Fld_LastOrdVendor].Hidden = true;
                    this.gridPODetails.DisplayLayout.Bands[1].Columns[clsPOSDBConstants.PODetail_Fld_LastOrdQty].Hidden = true;
                    //Added By Amit Date 6 july 2011
                    this.gridItemDetails.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.PODetail_Fld_DeptName].Hidden = true;
                    this.gridItemDetails.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.PODetail_Fld_SubDeptName].Hidden = true;
                    //End
                    // Added by Amit Date 4 Aug 2011
                    this.gridItemDetails.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.PODetail_Fld_RetailPrice].Hidden = true;
                    this.gridItemDetails.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.PODetail_Fld_ItemPrice].Hidden = true;
                    this.gridItemDetails.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.PODetail_Fld_Discount].Hidden = true;
                    //End

                    this.gridPODetails.DisplayLayout.Bands[1].Columns[clsPOSDBConstants.Item_Fld_Description].Header.VisiblePosition = 3;
                }
                this.gridPODetails.Refresh();
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "FormatGrid(string criteria)");
                clsUIHelper.ShowErrorMsg(ex.Message);
            }
        }

        private void gridItemDetails_AfterRowUpdate(object sender, RowEventArgs e)
        {
            if (gridItemDetails.ActiveRow == null)  //Sprint-21 - 1803 15-Sep-2015 JY Added
                return;
            else
            {
                try
                {
                    String message = " Item does not have vendor information";

                    if (this.isFilledPODetails)
                    {
                        this.isFilledPODetails = false;
                        return;
                    }

                    String itemQty = this.gridItemDetails.ActiveRow.Cells[clsPOSDBConstants.PODetail_Fld_QTY].Value.ToString().Trim();
                    String itemPrice = this.gridItemDetails.ActiveRow.Cells[clsPOSDBConstants.PODetail_Fld_Price].Value.ToString().Trim();
                    String vendorID = this.gridItemDetails.ActiveRow.Cells[clsPOSDBConstants.POHeader_Fld_VendorID].Value.ToString();
                    string packetSize = this.gridItemDetails.ActiveRow.Cells[clsPOSDBConstants.PODetail_Fld_PackSize].Value.ToString();

                    #region Sprint-21 - 1803 28-Sep-2015 JY Commented as keep same logic for all items
                    //if (this.gridItemDetails.ActiveRow.Cells[clsPOSDBConstants.PODetail_Fld_PackUnit].Value.ToString().ToUpper().Trim() == clsPOSDBConstants.PckUnit_CS && itemQty != string.Empty)
                    //{
                    //    this.gridItemDetails.ActiveRow.Cells[clsPOSDBConstants.PODetail_Fld_Cost].Value = Convert.ToInt32(itemQty) * Convert.ToDecimal(itemPrice) * Convert.ToDecimal(packetSize);
                    //}
                    #endregion

                    if (itemQty != string.Empty)
                    {
                        this.gridItemDetails.ActiveRow.Cells[clsPOSDBConstants.PODetail_Fld_Cost].Value = Convert.ToInt32(itemQty) * Convert.ToDecimal(itemPrice);
                    }
                    //if (this.gridItemDetails.ActiveRow.Cells[clsPOSDBConstants.PODetail_Fld_PackUnit].Value.ToString().ToUpper().Trim() == clsPOSDBConstants.PckUnit_CS && itemQty != string.Empty)
                    //{
                    //    this.gridItemDetails.ActiveRow.Cells[clsPOSDBConstants.PODetail_Fld_LastCostPrice].Value = Convert.ToInt32(itemQty) * Convert.ToDecimal(itemPrice) * Convert.ToDecimal(packetSize);
                    //}
                    //else if (itemQty != string.Empty)
                    //{
                    //    this.gridItemDetails.ActiveRow.Cells[clsPOSDBConstants.PODetail_Fld_LastCostPrice].Value = Convert.ToInt32(itemQty) * Convert.ToDecimal(itemPrice);
                    //}  
                    if (!isEditPO)
                    {
                        this.gridItemDetails.Refresh();
                    }

                    if (isEditPO)
                    {
                        List<String> orderNos = null;
                        bool inNewOrder = false;
                        int itemOrderNo = 0;
                        int orderID = 0;
                        int poDetailId = 0;
                        string ordDescription = string.Empty;

                        if (haveVendorInfo)
                        {
                            DataRow[] ordetailRows = masterOrderDetailsData.OrderDetailsTable.Select("VendorID=" + vendorID);
                            if (ordetailRows != null && ordetailRows.Length == 0)
                            {
                                orderNumber++;
                                itemOrderNo = orderNumber;
                                inNewOrder = true;
                                orderNos = new List<string>();
                                if (!isMaxOrderIDFirstTime)
                                {
                                    orderID = Convert.ToInt32(oPurchaseOrder.GetNextPOID());
                                    poDetailId = Convert.ToInt32(oPurchaseOrder.GetNextPODID());
                                    isMaxOrderIDFirstTime = true;
                                }
                                else
                                {
                                    orderID = GetMaxOrderID();
                                    poDetailId = GetMaxPODID();
                                }

                                if (!poOrdersNos.ContainsKey(vendorID))
                                {
                                    if (!orderNos.Contains(orderNumber.ToString()))
                                    {
                                        orderNos.Add(orderNumber.ToString());
                                    }
                                    poOrdersNos.Add(vendorID, orderNos);
                                }
                                else
                                {
                                    poOrdersNos.TryGetValue(vendorID, out orderNos);
                                    if (!orderNos.Contains(orderNumber.ToString()))
                                    {
                                        orderNos.Add(orderNumber.ToString());
                                    }
                                }
                                if (!podIdOrdernoDict.ContainsKey(poDetailId.ToString()))
                                {
                                    podIdOrdernoDict.Add(poDetailId.ToString(), orderNumber.ToString());
                                }

                                String vendorName = String.Empty;
                                vendorDetails.TryGetValue(vendorID, out vendorName);
                                using (frmOrderDetails frmOrderDetails = new frmOrderDetails())
                                {
                                    frmOrderDetails.lblTransactionType.Text += vendorName;
                                    frmOrderDetails.ShowDialog(this);
                                    ordDescription = frmOrderDetails.OrderDescription;
                                }

                                InitalizeMasterOrderDetails(vendorID, orderID.ToString(), ordDescription, false);
                                inNewOrder = true;
                            }
                            else
                            {
                                inNewOrder = false;
                                string tempPODID = string.Empty;
                                poOrdersNos.TryGetValue(vendorID, out orderNos);
                                if (orderNos != null)
                                {
                                    orderNos.Sort();
                                    itemOrderNo = Convert.ToInt32(orderNos[orderNos.Count - 1]);
                                }
                                orderID = GetMaxOrderID(ordetailRows);

                                poDetailId = GetExistPODID(itemOrderNo.ToString());
                                poDetailId++;
                                //InitalizeMasterOrderDetails(vendorID, orderID.ToString(), ordDescription);
                            }
                            AddItemsInEditOrders(vendorID, poDetailId.ToString(), orderID.ToString(), itemOrderNo.ToString(), inNewOrder);
                            CalculateTotals(vendorID, orderID.ToString());
                            //Uncommented the if by shitaljit to avoid updating item grid details in edit mode.
                            if (!isEditPO)
                            {
                                this.gridItemDetails.UpdateData();
                            }
                            //poDetailId = 0;
                        }
                        else
                        {
                            clsUIHelper.ShowErrorMsg(message);
                        }
                    }
                    else
                    {
                        if (haveVendorInfo)
                        {
                            if (!isAutoPO)
                                FormatGrid("ADD");
                            FillPODetails(vendorID);
                            if (isBestVendorUpdated)
                                CalculateTotals(nonBestVendor);

                            this.txtEditorNoOfVendors.Text = this.gridPODetails.Rows.Count.ToString();
                            //this.txtTotalCostForAllPO.Text = totalCostForAllPO.ToString();//Added By shitaljit for all PO(s) total cost   //Sprint-21 - 1803 15-Sep-2015 JY Commented
                        }
                        else
                        {
                            clsUIHelper.ShowErrorMsg(message);
                        }
                    }
                }
                catch (Exception ex)
                {
                    logger.Fatal(ex, "gridItemDetails_AfterRowUpdate(object sender, RowEventArgs e)");
                    clsUIHelper.ShowErrorMsg(ex.Message);
                }
            }

            m_IsExceptionOccured = false;
            //Added By SRT(Gaurav) Date: 17-Jul-2009
            txtEditorNoOfItems.Text = TotalItemsCount.ToString();
            txtEditorNoOfVendors.Text = TotalVendorsCount.ToString();
            if (!isAutoPO)
            {
                int csItemCount = CSItemCount;
                txtEditorNoOfCSItems.Text = csItemCount.ToString();
                // Added by Shrikant Mali on 02/10/2014 to update the count of non case Items.
                tbxNonCaseItems.Text = GetNonCaseItemCount(csItemCount).ToString();
            }
            //this.txtTotalCostForAllPO.Text = totalCostForAllPO.ToString();//Added By shitaljit for all PO(s) total cost   //Sprint-21 - 1803 15-Sep-2015 JY Commented
        }

        private void AddItemsInEditOrders(String vendorID, String newPoDetailId, String orderID, String maxOrderNo, bool inNewOrder)
        {
            DataRow newVendorRow = null;
            DataRow newDataRow = null;
            DataRow newOrderRow = null;
            DataRow[] vendorRows = null;
            List<String> orderNOs = null;
            String timeToOrder = String.Empty;
            String isAutoClose = String.Empty;
            try
            {
                vendorRows = VendorData.Vendor.Select("vendorID = '" + vendorID + "'");
                timeToOrder = vendorRows[0][clsPOSDBConstants.OrderDetail_Tbl_TimeToOrder].ToString();
                isAutoClose = vendorRows[0][clsPOSDBConstants.OrderDetail_Tbl_AuroSend].ToString();

                for (int count = 0; count < masterOrderDetailsData.MasterOrderDetailTable.Count; count++)
                {
                    newVendorRow = masterOrderDetailsData.MasterOrderDetailTable.Rows[count];
                }
                for (int count = 0; count < masterOrderDetailsData.OrderDetailsTable.Count; count++)
                {
                    newOrderRow = masterOrderDetailsData.OrderDetailsTable.Rows[count];

                    if (newOrderRow.RowState == DataRowState.Unchanged)
                    {
                        newOrderRow[clsPOSDBConstants.OrderDetail_Tbl_CloseOrder] = "Send PO(F12)";
                    }
                }
                for (int count = 0; count < masterOrderDetailsData.PODetailTable.Rows.Count; count++)
                {
                    newDataRow = masterOrderDetailsData.PODetailTable.Rows[count];

                    if (newDataRow.RowState == DataRowState.Added || newDataRow.RowState == DataRowState.Modified)
                    {
                        orderNosDropDown = new UltraDropDown();
                        this.orderNosDropDown.RowSelected += new RowSelectedEventHandler(orderNosDropDown_RowSelected);
                        poOrdersNos.TryGetValue(vendorID, out orderNOs);
                        if (inNewOrder)
                        {
                            newlyAddedOrderIDs.Add(orderID);
                            newDataRow[clsPOSDBConstants.PODetail_Fld_PODetailID] = newPoDetailId;
                        }
                        else
                        {
                            newPoDetailId = newDataRow[clsPOSDBConstants.PODetail_Fld_PODetailID].ToString();
                        }

                        newDataRow[clsPOSDBConstants.POHeader_Fld_OrderID] = orderID;
                        //orderNOs != null && orderNOs.Count > 0 is added by shitaljit on 1/16/2014 
                        //For JIRA 1722
                        if (orderNOs != null && orderNOs.Count > 0 && !orderNOs.Contains(maxOrderNo))
                        {
                            orderNOs.Add(maxOrderNo);
                        }
                        orderNosDropDown.DataSource = orderNOs;
                        this.gridItemDetails.ActiveRow.Cells["Order NO"].ValueList = orderNosDropDown;
                        this.gridItemDetails.ActiveRow.Cells["Order NO"].Value = maxOrderNo;

                        //Chnaged by SRT(Abhishek) Date : 30/10/2009
                        //Added this condition to check if rowstate is new then and only then we should insert the
                        //item in the newly added  POIDs
                        if (newDataRow.RowState == DataRowState.Added)
                            newlyAddedPOIDs.Add(newPoDetailId);
                        //End Of Changed By SRT(Abhishek) Date : 30/10/2009
                        break;
                    }
                }

                //if(false)
                //masterOrderDetailsData.PODetailTable.AcceptChanges();
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "AddItemsInEditOrders(String vendorID, String newPoDetailId, String orderID, String maxOrderNo, bool inNewOrder)");
                clsUIHelper.ShowErrorMsg(ex.Message);
            }
        }

        private void FillPoDetailsForIncompleteOrders(string poIndex, string poNumber)
        {
            String message = " Item does not have vendor information";
            String itemQty = String.Empty;
            String itemPrice = String.Empty;
            String vendorID = String.Empty;

            try
            {
                int index = Convert.ToInt32(poIndex);
                if (haveVendorInfo)
                {
                    itemQty = this.orderDetailData.PODetailTable.Rows[index][clsPOSDBConstants.PODetail_Fld_QTY].ToString().Trim();
                    itemPrice = this.orderDetailData.PODetailTable.Rows[index][clsPOSDBConstants.PODetail_Fld_Price].ToString().Trim();
                    vendorID = this.orderDetailData.PODetailTable.Rows[index][clsPOSDBConstants.POHeader_Fld_VendorID].ToString();
                    if (itemQty != string.Empty)
                        this.orderDetailData.PODetailTable.Rows[index][clsPOSDBConstants.PODetail_Fld_Cost] = Convert.ToInt32(itemQty) * Convert.ToDecimal(itemPrice);

                    PopulatePODetails(vendorID, index, poNumber);

                    if (isBestVendorUpdated)
                        CalculateTotals(nonBestVendor);

                    this.txtEditorNoOfVendors.Text = this.gridPODetails.Rows.Count.ToString();
                    //this.txtTotalCostForAllPO.Text = totalCostForAllPO.ToString();//Added By shitaljit for all PO(s) total cost   //Sprint-21 - 1803 15-Sep-2015 JY Commented
                }
                else
                {
                    clsUIHelper.ShowErrorMsg(message);
                }
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "FillPoDetailsForIncompleteOrders(string poIndex, string poNumber)");
                clsUIHelper.ShowErrorMsg(ex.Message);
            }
            m_IsExceptionOccured = false;
        }

        private void FillMasterPoDetailsForIncompleteOrders(string poNumber, string orderId)
        {
            String message = " Item does not have vendor information";
            String itemQty = String.Empty;
            String itemPrice = String.Empty;
            String vendorID = String.Empty;
            int rowCount = 0;
            string PackUnit = string.Empty, PackSize = string.Empty;

            try
            {
                if (haveVendorInfo)
                {
                    decimal dTotalCostForAllPO = 0.0M;  //Sprint-21 - 1803 11-Sep-2015 JY Added
                    rowCount = this.masterOrderDetailsData.PODetailTable.Rows.Count;
                    for (int count = 0; count < rowCount; count++)
                    {
                        itemQty = this.masterOrderDetailsData.PODetailTable.Rows[count][clsPOSDBConstants.PODetail_Fld_QTY].ToString().Trim();
                        itemPrice = this.masterOrderDetailsData.PODetailTable.Rows[count][clsPOSDBConstants.PODetail_Fld_Price].ToString().Trim();
                        vendorID = this.masterOrderDetailsData.PODetailTable.Rows[count][clsPOSDBConstants.POHeader_Fld_VendorID].ToString();

                        PackUnit = this.masterOrderDetailsData.PODetailTable.Rows[count][clsPOSDBConstants.PODetail_Fld_PackUnit].ToString().Trim();
                        PackSize = this.masterOrderDetailsData.PODetailTable.Rows[count][clsPOSDBConstants.PODetail_Fld_PackSize].ToString().Trim();

                        if (itemQty != string.Empty)
                        {
                            this.masterOrderDetailsData.PODetailTable.Rows[count][clsPOSDBConstants.PODetail_Fld_Cost] = Convert.ToInt32(itemQty) * Convert.ToDecimal(itemPrice); //Sprint-21 - 1803 15-Sep-2015 JY Un-commented
                            #region Sprint-21 - 1803 15-Sep-2015 JY Commented as keep same logic for all items
                            //if (PackUnit.ToUpper() == clsPOSDBConstants.PckUnit_CS)
                            //{
                            //    if (MMSUtil.UtilFunc.ValorZeroDEC(PackSize) > 0)
                            //    {
                            //        this.masterOrderDetailsData.PODetailTable.Rows[count][clsPOSDBConstants.PODetail_Fld_Cost] = Convert.ToInt32(itemQty) * Convert.ToDecimal(itemPrice) * MMSUtil.UtilFunc.ValorZeroDEC(PackSize);
                            //    }
                            //    else
                            //    {
                            //        this.masterOrderDetailsData.PODetailTable.Rows[count][clsPOSDBConstants.PODetail_Fld_Cost] = Convert.ToInt32(itemQty) * Convert.ToDecimal(itemPrice);
                            //    }
                            //}
                            //else
                            //{
                            //    this.masterOrderDetailsData.PODetailTable.Rows[count][clsPOSDBConstants.PODetail_Fld_Cost] = Convert.ToInt32(itemQty) * Convert.ToDecimal(itemPrice);
                            //}
                            #endregion
                            if (isEditPO) dTotalCostForAllPO += Convert.ToDecimal(this.masterOrderDetailsData.PODetailTable.Rows[count][clsPOSDBConstants.PODetail_Fld_Cost]);    //Sprint-21 - 1803 11-Sep-2015 JY Added
                        }

                        //PopulatePODetails(vendorID, index, poNumber);
                        //PopulateMasterPODetails(vendorID, count, poNumber, orderId);  //Sprint-21 - 1803 17-Sep-2015 JY commented to improve performance - alternate code added

                        if (isBestVendorUpdated)
                            CalculateTotals(nonBestVendor, orderId);

                        this.txtEditorNoOfVendors.Text = this.gridPODetails.Rows.Count.ToString();
                        //this.txtTotalCostForAllPO.Text = totalCostForAllPO.ToString();//Added By shitaljit for all PO(s) total cost   //Sprint-21 - 1803 15-Sep-2015 JY Commented
                        //CalculateTotals(vendorID, orderId);   //Sprint-21 - 1803 17-Sep-2015 JY commented to improve performance - alternate code added
                    }

                    if (isEditPO) this.txtTotalCostForAllPO.Text = dTotalCostForAllPO.ToString("G29");   //Sprint-21 - 1803 11-Sep-2015 JY Added
                }
                else
                {
                    clsUIHelper.ShowErrorMsg(message);
                }
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "FillMasterPoDetailsForIncompleteOrders(string poNumber, string orderId)");
                clsUIHelper.ShowErrorMsg(ex.Message);
            }
            m_IsExceptionOccured = false;
        }

        private void gridItemDetails_BeforeRowUpdate(object sender, CancelableRowEventArgs e)
        {
            UltraGridRow oCurrentRow = this.gridItemDetails.ActiveRow;

            if (oCurrentRow == null) return;    //Sprint-21 - 1803 15-Sep-2015 JY Added

            UltraGridCell oCurrentCell = null;
            bool blnCellChanged = false;
            int podID = 0;

            foreach (UltraGridCell oCell in oCurrentRow.Cells)
            {
                if (oCell.DataChanged == true || oCell.Text.Trim() != "")
                {
                    blnCellChanged = true;
                    break;
                }
            }
            if (blnCellChanged == false)
            {
                podID = GetMaxPODID();
                oCurrentRow.Cells[clsPOSDBConstants.PODetail_Fld_PODetailID].Value = podID;
                return;
            }
            try
            {
                if (oCurrentRow.DataChanged == true)
                {
                    //Added by SRT(Abhishek) Date : 13 Aug 2009   
                    //added to check condition if row is commited 
                    //Added By SRT(Ritesh Parekh) Date : 13-Aug-2009
                    //Added logic to validate for empty row.
                    selectedPODetailIDFromVendorCombo = this.gridItemDetails.ActiveRow.Cells[clsPOSDBConstants.PODetail_Fld_PODetailID].Value.ToString();
                    if (IsItemPopulated && selectedPODetailIDFromVendorCombo.Trim().Length > 0)
                    {
                        oCurrentCell = oCurrentRow.Cells[clsPOSDBConstants.PODetail_Fld_ItemID];
                        oPurchaseOrder.Validate_ItemID(oCurrentCell.Text.ToString());
                        //string itemId = oCurrentCell.Text.ToString();

                        oCurrentCell = oCurrentRow.Cells[clsPOSDBConstants.PODetail_Fld_QTY];
                        oPurchaseOrder.Validate_Qty(oCurrentCell.Text.ToString());

                        oCurrentCell = oCurrentRow.Cells[clsPOSDBConstants.PODetail_Fld_Price];
                        oPurchaseOrder.Validate_Cost(oCurrentCell.Text.ToString());
                    }
                    //Added by SRT(Abhishek) Date : 13 Aug 2009   
                    //added to handle the event 
                    //and focus will be set on the itemid                    
                    else
                    {
                        FocusItemId();
                        if (e != null)
                            e.Cancel = true;
                    }
                    #region commented
                    //if (IsInComplete != true)
                    //{
                    //    PODetailSvr poDetSvr = new PODetailSvr();
                    //    PurchaseOrder purchaseOrd = new PurchaseOrder();
                    //    PODetailData poDetData = new PODetailData();
                    //    POHeaderSvr poHeaderSvr = new POHeaderSvr();
                    //    poDetData = poDetSvr.PopulateList(" AND " + clsPOSDBConstants.PODetail_tbl + "." + clsPOSDBConstants.PODetail_Fld_ItemID + " = '" + itemId.ToString() + "'");
                    //    POHeaderData poData = new POHeaderData();
                    //    String str = String.Empty;
                    //    int count = 0;


                    //    if (poDetData.PODetail.Rows.Count > 0)
                    //    {
                    //        foreach (PODetailRow poDetailRow in poDetData.PODetail.Rows)
                    //        {
                    //            POHeaderData poHeaderData = purchaseOrd.PopulateHeader(poDetData.PODetail[count].OrderID);
                    //            poData.Merge(poHeaderData);
                    //            // filling the string for the each po number and Item code 
                    //            str = str + " Purchase Order (" + poData.POHeader[count].OrderNo + " ," + ((PurchseOrdStatus)poData.POHeader[count].Status).ToString() + ") already has Item (" + poDetailRow.ItemID + " ," + poDetailRow.ItemDescription + ") \n ";
                    //            count++;
                    //        }
                    //        // message box will be displayed new row for each po and each item 
                    //        if (!(Resources.Message.Display(str + "Do You Want To Continue ?\n", " Confirm PO ", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes))
                    //        {
                    //            this.grdDetail.Rows[grdDetail.ActiveRow.Index].CancelUpdate();
                    //        }
                    //    }
                    //}
                    #endregion
                }
            }
            catch (Exception ex)
            {

                if (oCurrentCell != null)
                {
                    logger.Fatal(ex, "gridItemDetails_BeforeRowUpdate(object sender, CancelableRowEventArgs e)");
                    clsUIHelper.ShowErrorMsg(ex.Message);
                    if (e != null)
                        e.Cancel = true;
                    if (!isAutoPO && oCurrentCell != null)
                    {
                        FocusToCell(oCurrentCell.Column.Key);
                    }

                }
            }
        }

        private void FocusToCell(string cellName)
        {
            UltraGridRow oCurrentRow;
            UltraGridCell oCurrentCell;
            try
            {
                oCurrentRow = this.gridItemDetails.ActiveRow;
                oCurrentCell = oCurrentRow.Cells[cellName];
                this.gridItemDetails.ActiveCell = oCurrentCell;
                oCurrentCell.Selected = true;

                this.gridItemDetails.PerformAction(UltraGridAction.ActivateCell);
                this.gridItemDetails.PerformAction(UltraGridAction.EnterEditMode);

            }
            catch (Exception ex)
            {
            }
        }
        private void btnDeleteSelectedItem_Click(object sender, EventArgs e)
        {
            try
            {
                string strDeleteMsg = "Are you sure you want to delete the following items?\n";
                string itemID;
                if (gridItemDetails.Rows.Count > 0)
                {
                    //Added by Krishna on 16 August 2012
                    Infragistics.Win.UltraWinGrid.RowsCollection GridRows = gridItemDetails.Rows;

                    for (int j = 0; j < GridRows.Count; j++)
                    {
                        if (Convert.ToBoolean(GridRows[j].Cells["Select"].Value) == true)
                        {
                            strDeleteMsg += "\n " + GridRows[j].Cells[clsPOSDBConstants.PODetail_Fld_ItemID].Value.ToString().Trim() + " - " + GridRows[j].Cells[clsPOSDBConstants.Item_Fld_Description].Value.ToString().Trim();
                        }
                    }
                    if (POS_Core_UI.Resources.Message.DisplayDefaultNo(strDeleteMsg, "Purchase Order", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        for (int i = GridRows.Count - 1; i >= 0; i--)
                        {
                            if (Convert.ToBoolean(GridRows[i].Cells["Select"].Value) == true)
                            {
                                gridItemDetails.ActiveRow = GridRows[i];
                                itemID = this.gridItemDetails.ActiveRow.Cells[clsPOSDBConstants.PODetail_Fld_ItemID].Value.ToString();
                                if (itemID != "")
                                {
                                    // Modified by Shrikant Mali on 10/02/2014 so as to add the active row parameter in the 
                                    // following function as this function needed to be reused for the logic to 
                                    //exclude the filted out items using the case and non case item filter.
                                    DeleteGridRow(false, this.gridItemDetails.ActiveRow);
                                }
                            }
                        }
                        #region Sprint-21 - 1803 14-Sep-2015 JY Added
                        try
                        {
                            if (gridItemDetails.Rows.Count == 0)
                            {
                                this.txtTotalCostForAllPO.Text = "0";
                            }
                            else
                            {
                                decimal dTotalCostForAllPO = 0.0M;
                                for (int i = 0; i < gridItemDetails.Rows.Count; i++)
                                {
                                    dTotalCostForAllPO += Configuration.convertNullToDecimal(gridItemDetails.Rows[i].Cells[clsPOSDBConstants.PODetail_Fld_Cost].Value);
                                }
                                this.txtTotalCostForAllPO.Text = dTotalCostForAllPO.ToString("G29");
                            }
                        }
                        catch { }
                        #endregion
                    }
                    //DeleteGridRow(true);Orignal commented by Krishna
                    //Added by SRT(Abhishek) Date : 05/11/2009
                    if (TotalItemsCount == 0)
                        this.pnlAutoGenPO.Enabled = true;
                    //End Of Added by SRT(Abhishek) Date : 05/11/2009
                }
                else
                {
                    clsUIHelper.ShowErrorMsg("There are No Items In PO");
                    //Added By SRT(Ritesh Parekh) Date : 19-Aug-2009
                    FocusNewItemId();
                    //End Of Added By SRT(Ritesh Parekh)
                }
                //Added By SRT(Gaurav) Date: 17-Jul-2009
                txtEditorNoOfItems.Text = TotalItemsCount.ToString();
                txtEditorNoOfVendors.Text = TotalVendorsCount.ToString();
                int csItemCount = CSItemCount;
                txtEditorNoOfCSItems.Text = csItemCount.ToString();
                // Added by Shrikant Mali on 02/10/2014 to update the count of non case Items.
                tbxNonCaseItems.Text = GetNonCaseItemCount(csItemCount).ToString();
                //this.txtTotalCostForAllPO.Text = totalCostForAllPO.ToString();//Added By shitaljit for all PO(s) total cost   //Sprint-21 - 1803 15-Sep-2015 JY Commented
            }
            catch (Exception Ex)
            {
                logger.Fatal(Ex, "btnDeleteSelectedItem_Click(object sender, EventArgs e)");
                clsUIHelper.ShowErrorMsg(Ex.Message);
            }
        }

        private void DeleteGridRow(bool AskForDelete, UltraGridRow itemToBeDeleted)
        {
            isGridRowDeleted = false;
            string vendorID = string.Empty;
            string totalQty = string.Empty;
            string itemID = string.Empty;
            string orderId = string.Empty;
            string poDetailId = string.Empty;
            string message = "Do you want to delete the Item #";
            try
            {
                if (itemToBeDeleted == null) return;
                vendorID = itemToBeDeleted.Cells[clsPOSDBConstants.POHeader_Fld_VendorID].Value.ToString();
                //if (vendorID == string.Empty)
                //{
                // this.gridItemDetails.UpdateData();  
                //}
                orderId = itemToBeDeleted.Cells[clsPOSDBConstants.PODetail_Fld_OrderID].Value.ToString();
                itemID = itemToBeDeleted.Cells[clsPOSDBConstants.PODetail_Fld_ItemID].Value.ToString();

                //Added by SRT(Abhishek) Date : 24/07/2009
                if (itemID == "")
                {
                    clsUIHelper.ShowErrorMsg("Please select Item First");
                    return;
                }
                //End of Added by SRT(Abhishek) Date : 24/07/2009

                //Added by SRT(Abhishek) 
                if (AskForDelete)
                    clsUIHelper.ShowInfoMsg(message + itemID);
                //End of Added by SRT(Abhishek)

                //added one condition to delete PODeatils 
                //added by SRT(Abhishek)  29/07/2009   
                if (clsUIHelper.IsOK == true || AskForDelete == false)
                {
                    //Added by Prashant(SRT) Date:18-7-09
                    //Delete PODetails from databse                    
                    poDetailId = itemToBeDeleted.Cells[clsPOSDBConstants.PODetail_Fld_PODetailID].Value.ToString();
                    orderId = itemToBeDeleted.Cells[clsPOSDBConstants.PODetail_Fld_OrderID].Value.ToString();
                    //Updated By SRT(Ritesh Parekh) Date : 13-Aug-2009
                    //Checked for int value of po detail id.
                    Int32 intPODetId = 0;
                    if (Int32.TryParse(poDetailId.ToString().Trim(), out intPODetId))
                    {
                        oPurchaseOrder.DeletePODetail(Convert.ToInt32(poDetailId));
                    }
                    // End of updated by SRT(Ritesh Parekh)
                    //End of Added by Prashant(SRT) Date:18-7-09

                    activeRowIndex = itemToBeDeleted.Index;

                    isGridRowDeleted = itemToBeDeleted.Delete(false);
                    this.gridItemDetails.UpdateData();
                    //Added By SRT(Gaurav) Date: 17-Jul-2009
                    txtEditorNoOfItems.Text = TotalItemsCount.ToString();
                    txtEditorNoOfVendors.Text = TotalVendorsCount.ToString();
                    int csItemCount = CSItemCount;
                    txtEditorNoOfCSItems.Text = csItemCount.ToString();
                    // Added by Shrikant Mali on 02/10/2014 to update the count of non case Items.
                    tbxNonCaseItems.Text = GetNonCaseItemCount(csItemCount).ToString();
                    //this.txtTotalCostForAllPO.Text = totalCostForAllPO.ToString();//Added By shitaljit for all PO(s) total cost   //Sprint-21 - 1803 15-Sep-2015 JY Commented

                    //Added By SRT(Gaurav) Date : 14-Jul-2009
                    //This code is added to solve focus issue after delete.

                    //FocusNewItemId();
                    //FocusRowBeforeDeleted();

                    //End Of Added By SRT(Gaurav)
                }
                else
                {
                    //Added by SRT(Abhishek) Date : 09/09/2009
                    //Added to focus row which is just befor the row which is deleted.
                    //if there are no rows present then it will set to focus to first row.
                    //FocusNewItemId();
                    FocusRowBeforeDeleted();
                    //End of Added by SRT(Abhishek)            
                    return;
                }
                //Updated By SRT(Gaurav)  09-Jul-2009
                if (vendorID.Trim().Length > 0)
                {
                    if (!isEditPO)
                    {
                        CalculateTotals(vendorID);
                    }
                    else
                    {
                        CalculateTotals(vendorID, orderId);
                    }
                    if (this.gridPODetails.ActiveRow != null)
                    {
                        if (this.gridPODetails.ActiveRow.ChildBands != null)
                        {
                            if (this.gridPODetails.ActiveRow.ChildBands.FirstRow == null)
                            {
                                isGridRowDeleted = this.gridPODetails.ActiveRow.Delete(false);
                            }
                        }
                    }
                    txtEditorNoOfItems.Text = TotalItemsCount.ToString();
                    txtEditorNoOfVendors.Text = TotalVendorsCount.ToString();
                    int csItemCount = CSItemCount;
                    txtEditorNoOfCSItems.Text = csItemCount.ToString();
                    // Added by Shrikant Mali on 02/10/2014 to update the count of non case Items.
                    tbxNonCaseItems.Text = GetNonCaseItemCount(csItemCount).ToString();
                    //this.txtTotalCostForAllPO.Text = totalCostForAllPO.ToString();//Added By shitaljit for all PO(s) total cost   //Sprint-21 - 1803 15-Sep-2015 JY Commented
                    //Added by SRT(Abhishek) Date : 09/09/2009
                    //Added to focus row which is just befor the row which is deleted.
                    //if there are no rows present then it will set to focus to first row.
                    //FocusNewItemId();
                    FocusRowBeforeDeleted();
                    //End of Added by SRT(Abhishek)
                }
                //End Of Updated By SRT(Gaurav)
            }
            catch (Exception exp)
            {
                logger.Fatal(exp, "DeleteGridRow(bool AskForDelete, UltraGridRow itemToBeDeleted)");
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }
        private void gridItemDetails_KeyDown(object sender, KeyEventArgs e)
        {
            string searchStatus = string.Empty;
            //KeyEventArgs.
            if ((e.KeyData == System.Windows.Forms.Keys.Enter || e.KeyData == System.Windows.Forms.Keys.F4 || e.KeyData == Keys.Escape) && !e.Control)
            {
                String itemQty = String.Empty;
                String itemPrice = String.Empty;
                UltraGridCell oCurrentCell;
                if (this.gridItemDetails.ActiveCell == null)
                    return;

                oCurrentCell = this.gridItemDetails.ActiveCell;

                if (!oCurrentCell.IsInEditMode)
                {
                    oCurrentCell.IsInEditMode = true;
                }

                //Added By SRT(Ritesh Parekh) Date: 29-Aug-2009
                if ((e.KeyData == Keys.Enter || e.KeyData == Keys.Tab || e.KeyData == Keys.Right || e.KeyData == Keys.Left || e.KeyData == Keys.Up || e.KeyData == Keys.Down) && oCurrentCell.Column.Key.Trim().ToUpper() == clsPOSDBConstants.PODetail_Fld_VendorName.Trim().ToUpper())
                {
                    if (!IsThisVendorValid(oCurrentCell.Text))
                    {
                        try
                        {
                            oCurrentCell.CancelUpdate();
                            this.gridItemDetails.ActiveCell = oCurrentCell;
                            this.gridItemDetails.ActiveRow.Cells[clsPOSDBConstants.PODetail_Fld_VendorName].Activate();
                            this.gridItemDetails.PerformAction(Infragistics.Win.UltraWinGrid.UltraGridAction.EnterEditMode);
                            //FocusNewItemId();
                        }
                        catch (Exception Ex)
                        {
                        }
                        e.Handled = true;
                    }
                }
                //End Of Added By SRT(Ritesh Parekh)

                if (e.KeyData == Keys.Escape && oCurrentCell.Column.Key.Trim().ToUpper() != "ItemId".Trim().ToUpper())
                {
                    e.Handled = true;
                    return;
                }
                else if (e.KeyData == Keys.Escape && oCurrentCell.Column.Key.Trim().ToUpper() == "ItemId".Trim().ToUpper()
                    && Configuration.convertNullToInt(txtEditorNoOfItems.Text) == 0)
                {

                    this.Close();
                }
                else if (e.KeyData == Keys.Escape && oCurrentCell.Column.Key == "ItemId".Trim().ToUpper()
                    && Configuration.convertNullToInt(txtEditorNoOfItems.Text) > 0)
                {
                    e.Handled = true;
                    return;
                }

                if (oCurrentCell.Column.Key.Trim().ToUpper() == "ItemId".Trim().ToUpper() && oCurrentCell.Text.Trim().Length == 0)
                {
                    //Added By SRT(Gaurav) To Avoid move next in grid.
                    e.Handled = true;
                    //Added By SRT(Ritesh Parekh) Date: 27-Aug-2009
                    //Added to solve deleted item id (temp) to set back issue.
                    if (oCurrentCell.Text.Trim() != oCurrentCell.Value.ToString().Trim())
                    {
                        oCurrentCell.CancelUpdate();
                        FocusNewItemId();
                    }
                    //End Of Added By SRT(Ritesh Parekh)
                    return;
                }
                if (oCurrentCell.Column.Key.Trim().ToUpper() == "ProcessedQty".Trim().ToUpper())
                {
                    this.gridItemDetails.PerformAction(UltraGridAction.NextRow);
                    FocusNewItemId();
                    e.Handled = true;
                    return;
                }

                if (oCurrentCell == null) return;

                if (oCurrentCell.Column.Key.Trim().ToUpper() == "ItemId".Trim().ToUpper()
                    && (oCurrentCell.DataChanged == false || m_IsExceptionOccured == true))
                {
                    //Added By SRT(Gaurav) To Avoid move next in grid.
                    FocusItemId();
                    e.Handled = true;
                    return;
                }
                //Added by SRT(Gaurav) Date:14-7-09
                bool SearchResult = false;
                e.Handled = true;
                searchStatus = SearchItem(out SearchResult);
                m_IsExceptionOccured = false;
                if (!SearchResult)
                {
                    e.Handled = !SearchResult;
                }
                else
                {
                    e.Handled = false;
                }
                //End of Added by SRT(Gaurav) Date:14-7-09
                //Updated by SRT(Prashant) Date:14-7-09
                if (searchStatus == "OrdItem-NO" || searchStatus == "ITEM-EXIST")
                {
                    e.Handled = true;
                    return;
                }

                //End of Updated by SRT(Prashant) Date:14-7-09
                if (this.gridItemDetails.ActiveRow != null)
                {
                    string tempQty = string.Empty;
                    //this.gridItemDetails.ActiveRow.Cells[clsPOSDBConstants.PODetail_Fld_VendorName].DroppedDown = false;
                    itemQty = this.gridItemDetails.ActiveRow.Cells[clsPOSDBConstants.PODetail_Fld_QTY].Value.ToString().Trim();
                    tempQty = this.gridItemDetails.ActiveRow.Cells[clsPOSDBConstants.PODetail_Fld_QTY].Text.Trim();
                    itemPrice = this.gridItemDetails.ActiveRow.Cells[clsPOSDBConstants.PODetail_Fld_Price].Value.ToString().Trim();
                    if (tempQty != string.Empty)
                    {
                        if (itemQty != DBNull.Value.ToString())
                        {
                            //Added By SRT(Gaurav) Date : 10-Jul-2009
                            decimal tempItemPrice = 0;
                            int tempItemQty = 0;
                            if (decimal.TryParse(itemPrice, out tempItemPrice) && Int32.TryParse(itemQty, out tempItemQty))
                            {
                                this.gridItemDetails.ActiveRow.Cells[clsPOSDBConstants.PODetail_Fld_Cost].Value = tempItemQty * tempItemPrice;
                            }
                            //End Of Added By SRT(Gaurav)
                        }
                    }
                    else
                    {
                        this.gridItemDetails.ActiveCell = this.gridItemDetails.ActiveRow.Cells[clsPOSDBConstants.PODetail_Fld_QTY];
                        this.gridItemDetails.ActiveCell.Activate();
                        if (e.KeyData != System.Windows.Forms.Keys.F4)
                        {
                            this.gridItemDetails.PerformAction(UltraGridAction.PrevCell);
                        }
                        this.gridItemDetails.PerformAction(UltraGridAction.EnterEditMode);
                    }
                }
            }
            //Added By SRT(Gaurav) Date : 14-Jun-2009
            //This if is added to handle the keyboard shortcut Ctrl+Enter to focus in ItemId of the grid.
            else if (e.KeyData == System.Windows.Forms.Keys.Enter && e.Control)
            {
                FocusNewItemId();
            }
            //Added By SRT(Gaurav) Date : 15-Jun-2009
            //This code is added to handle the Right arrow key to avoid user updating the data in grid with wrong scenario.
            else if (e.KeyData == Keys.Right)
            {
                if (this.gridItemDetails.ActiveRow.Cells["PODetailId"].Text.Trim().Length < 1)
                {
                    FocusItemId();
                    e.Handled = true;
                }
                if (gridItemDetails.ActiveCell.Column.Key.Trim().ToUpper() == "ProcessedQty".Trim().ToUpper())
                {
                    FocusItemId();
                    e.Handled = true;
                    return;
                }
            }
            else if ((e.KeyData == Keys.Up || e.KeyData == Keys.Down) && this.gridItemDetails.ActiveRow != null
           && this.gridItemDetails.ActiveRow.Cells["Comments"].IsActiveCell
           && this.gridItemDetails.ActiveRow.Cells["PODetailId"].Text.Trim().Length > 0)
            {
                DataRow[] orows = null;
                if (!isEditPO)
                {
                    orows = orderDetailData.PODetailTable.Select("PODetailId=" + this.gridItemDetails.ActiveRow.Cells["PODetailId"].Text.Trim());
                }
                else
                {
                    orows = masterOrderDetailsData.PODetailTable.Select("PODetailId=" + this.gridItemDetails.ActiveRow.Cells["PODetailId"].Text.Trim());
                }
                if (orows.Length == 0)
                {
                    e.Handled = true;
                    return;
                }
            }
            //Added By SRT(Ritesh Parekh) Date : 27-Jul-2009
            else if (e.KeyData == Keys.Tab)
            {
                this.gridItemDetails.PerformAction(UltraGridAction.NextRow);
            }
            //End Of Added By SRT(Ritesh Parekh)
            //Changed By SRT(Ritesh Parekh) Date: 27-Aug-2009
            //Update to solve down arrow key deleted item issue.
            else if ((e.KeyData == Keys.Down || e.KeyData == Keys.Up) && (this.gridItemDetails.ActiveRow != null) && (this.gridItemDetails.ActiveRow.Cells[clsPOSDBConstants.PODetail_Fld_ItemID].IsActiveCell || this.gridItemDetails.ActiveRow.Cells[clsPOSDBConstants.PODetail_Fld_QTY].IsActiveCell) && (!IsThisRowEmpty(this.gridItemDetails.ActiveRow)))
            {
                if ((this.gridItemDetails.ActiveRow.Cells[clsPOSDBConstants.PODetail_Fld_ItemID].Value.ToString().Trim() != this.gridItemDetails.ActiveRow.Cells[clsPOSDBConstants.PODetail_Fld_ItemID].Text.Trim())
                    || (this.gridItemDetails.ActiveRow.Cells[clsPOSDBConstants.PODetail_Fld_QTY].Value.ToString().Trim() != this.gridItemDetails.ActiveRow.Cells[clsPOSDBConstants.PODetail_Fld_QTY].Text.Trim()))
                {
                    this.gridItemDetails.ActiveRow.Cells[clsPOSDBConstants.PODetail_Fld_ItemID].CancelUpdate();
                    FocusItemId();
                    //selectedVendorRow.Handled = true;
                }
            }
            else if ((e.KeyData == Keys.Down || e.KeyData == Keys.Up || e.KeyData == Keys.Right) && IsThisRowEmpty(this.gridItemDetails.ActiveRow))
            {
                ////Added by SRT(Abhishek)  Date : 02/09/2009                
                //selectedVendorRow.Handled = true;
                //Added by SRT(Abhishek) 
                this.gridItemDetails.PerformAction(UltraGridAction.NextRow);
                FocusItemId();
                e.Handled = true;
                return;
            }
        }

        private void ultraBtnVendorHistory_Click(object sender, EventArgs e)
        {
            string vendorId = string.Empty;
            try
            {
                if (gridPODetails.ActiveRow != null)
                {
                    vendorId = gridPODetails.ActiveRow.Cells["VendorID"].Value.ToString();
                    if (vendorId != "")
                    {
                        frmVendorHistory viewHistory = new frmVendorHistory(vendorId);
                        viewHistory.ShowDialog();
                    }
                    else
                    {
                        clsUIHelper.ShowErrorMsg(" Please Select Vendor First");
                    }
                }
                else
                {
                    clsUIHelper.ShowErrorMsg(" Please Select Vendor First");
                }
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "ultraBtnVendorHistory_Click(object sender, EventArgs e)");
                clsUIHelper.ShowErrorMsg(ex.Message);
            }
        }
        private void gridPODetails_ClickCellButton(object sender, CellEventArgs e)
        {
            String OrderNo = string.Empty;
            int lclVendorId = 0;
            string lclVendorCode = string.Empty;
            string orderID = string.Empty;
            bool isCloseOrder = false;
            Int32 PartiallyAckOrderId = 0;
            bool closeWithSave = false;
            DataRow[] closedOrders = null;
            DataRow[] closedInOrders = null;
            String orderDescription = String.Empty;
            String msgString = "Do you want to Send ";
            if (gridPODetails.ActiveRow != null)
            {

                if (gridPODetails.ActiveRow.ChildBands[0].Key == clsPOSDBConstants.POItemRelationName)
                {
                    if (isEditPO)
                    {
                        orderDetailData = new OrderDetailsData();
                        orderDetailData.OrderDetailsTable = masterOrderDetailsData.OrderDetailsTable;
                        orderDetailData.PODetailTable = masterOrderDetailsData.PODetailTable;
                    }

                    //Added by SRT(Abhishek) Date : 26 Aug 2009
                    //this part will clear the row if podetail id does not exist 
                    string selectedPODetailIDFromVendorCombo = this.gridItemDetails.ActiveRow.Cells[clsPOSDBConstants.PODetail_Fld_PODetailID].Value.ToString();
                    if (selectedPODetailIDFromVendorCombo == string.Empty || !IsPODetailExist(selectedPODetailIDFromVendorCombo))
                        this.gridItemDetails.ActiveRow.Delete(false);
                    //End of Added by SRT(Abhishek) Date : 26 Aug 2009

                    OrderNo = gridPODetails.ActiveRow.Cells["Order No"].Text;
                    orderID = gridPODetails.ActiveRow.Cells["OrderId"].Text;
                    lclVendorId = (int)gridPODetails.ActiveRow.Cells["VendorID"].Value;
                    lclVendorCode = gridPODetails.ActiveRow.Cells["Vendor Name"].Text;
                    orderDescription = gridPODetails.ActiveRow.Cells[clsPOSDBConstants.POHeader_Fld_Description].Text;
                    isCloseOrder = true;
                    if (Resources.Message.Display(msgString + " PO #" + OrderNo + " for " + lclVendorCode, "Close Purchase Order", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                    {
                        closeWithSave = true;
                    }
                    else
                    {
                        closeWithSave = false;
                        //Added by SRT(Abhishek) Date : 27 Aug 2009
                        FocusNewItemId();
                        //End of Added by SRT(Abhishek) 
                        return;
                    }
                    closeOrder(OrderNo, orderID, orderDescription, lclVendorId, lclVendorCode, isCloseOrder, PartiallyAckOrderId, closeWithSave);
                    //Delete saved rows ---need to optimize
                    if (!isEditPO)
                    {
                        closedOrders = orderDetailData.OrderDetailsTable.Select("VendorID = '" + lclVendorId + "'");
                        orderDetailData.OrderDetailsTable.Rows.Remove(closedOrders[0]);
                        orderDetailData.AcceptChanges();
                        if (orderDetailData.OrderDetailsTable.Rows.Count == 0)
                        {
                            this.Close();
                        }
                    }
                    else
                    {
                        string tempOrderId = string.Empty;
                        closedInOrders = masterOrderDetailsData.OrderDetailsTable.Select("OrderID = '" + orderID + "'");
                        for (int countOfClose = 0; countOfClose < closedInOrders.Length; countOfClose++)
                        {
                            tempOrderId = closedInOrders[countOfClose][clsPOSDBConstants.POHeader_Fld_OrderID].ToString();
                            masterOrderDetailsData.OrderDetailsTable.Rows.Remove(closedInOrders[countOfClose]);
                            CalculateTotals(lclVendorId.ToString(), tempOrderId);
                        }
                        closedInOrders = masterOrderDetailsData.OrderDetailsTable.Select("VendorID = '" + lclVendorId + "'");
                        if (closedInOrders.Length == 0)
                        {
                            closedInOrders = masterOrderDetailsData.MasterOrderDetailTable.Select("VendorID = '" + lclVendorId + "'");
                            for (int countOfClose = 0; countOfClose < closedInOrders.Length; countOfClose++)
                            {
                                masterOrderDetailsData.MasterOrderDetailTable.Rows.Remove(closedInOrders[countOfClose]);
                            }
                        }
                        masterOrderDetailsData.AcceptChanges();
                        this.gridPODetails.Update();
                    }
                }
                else if (gridPODetails.ActiveRow.ChildBands[0].Key == clsPOSDBConstants.MasterPOItemRelationName)
                {
                    int totalOrders = 0;
                    string vendorName = string.Empty;
                    totalOrders = this.gridPODetails.ActiveRow.ChildBands[0].Rows.Count;

                    if (totalOrders > 0)
                    {
                        if (isEditPO)
                        {
                            orderDetailData = new OrderDetailsData();
                            orderDetailData.OrderDetailsTable = masterOrderDetailsData.OrderDetailsTable;
                            orderDetailData.PODetailTable = masterOrderDetailsData.PODetailTable;
                        }
                        vendorName = this.gridPODetails.ActiveRow.Cells[clsPOSDBConstants.PODetail_Fld_VendorName].Value.ToString();
                        isCloseOrder = true;
                        if (Resources.Message.Display(msgString + " All PO for " + vendorName, "Close Purchase Order", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                        {
                            closeWithSave = true;
                        }
                        else
                        {
                            closeWithSave = false;
                            return;
                        }
                    }
                    else
                    {
                        return;
                    }
                    for (int count = 0; count < totalOrders; count++)
                    {
                        OrderNo = this.gridPODetails.ActiveRow.ChildBands[0].Rows[count].Cells[clsPOSDBConstants.OrderDetail_Fld_OrderNo].Value.ToString();
                        lclVendorId = (int)this.gridPODetails.ActiveRow.ChildBands[0].Rows[count].Cells["VendorID"].Value;
                        lclVendorCode = this.gridPODetails.ActiveRow.ChildBands[0].Rows[count].Cells["Vendor Name"].Text;
                        orderDescription = this.gridPODetails.ActiveRow.ChildBands[0].Rows[count].Cells[clsPOSDBConstants.POHeader_Fld_Description].Text;
                        orderID = this.gridPODetails.ActiveRow.ChildBands[0].Rows[count].Cells["OrderID"].Text;
                        closeOrder(OrderNo, orderID, orderDescription, lclVendorId, lclVendorCode, isCloseOrder, PartiallyAckOrderId, closeWithSave);
                    }
                    closedOrders = masterOrderDetailsData.OrderDetailsTable.Select("VendorID = '" + lclVendorId + "'");
                    closedInOrders = masterOrderDetailsData.MasterOrderDetailTable.Select("VendorID = '" + lclVendorId + "'");
                    for (int countOfClose = 0; countOfClose < closedOrders.Length; countOfClose++)
                    {
                        masterOrderDetailsData.OrderDetailsTable.Rows.Remove(closedOrders[countOfClose]);
                    }
                    for (int countOfClose = 0; countOfClose < closedInOrders.Length; countOfClose++)
                    {
                        masterOrderDetailsData.MasterOrderDetailTable.Rows.Remove(closedInOrders[countOfClose]);
                    }
                    masterOrderDetailsData.AcceptChanges();
                    this.gridPODetails.Update();
                    this.Close();
                }
            }
            else
            {
                clsUIHelper.ShowErrorMsg("Please select vendor to close the order.");
            }
        }
        public VendorData VendorData
        {
            get
            {
                return this.vendorData;
            }
        }
        private void gridPODetails_AfterSelectChange(object sender, AfterSelectChangeEventArgs e)
        {
            int vendorID = 0;
            string itemCode = string.Empty;
            for (int itemCount = 0; itemCount < this.gridItemDetails.Rows.Count; itemCount++)
            {
                this.gridItemDetails.Rows[itemCount].Selected = false;
            }
            if (this.gridPODetails.ActiveRow == null) return;
            if (this.gridPODetails.ActiveRow.ChildBands != null)
            {
                for (int cntRow = 0; cntRow < this.gridPODetails.ActiveRow.ChildBands[0].Rows.Count; cntRow++)
                {
                    this.gridPODetails.ActiveRow.ChildBands[0].Rows[cntRow].Selected = true;
                }
                vendorID = (int)gridPODetails.ActiveRow.Cells["VendorID"].Value;
                if (this.gridItemDetails.Rows != null)
                {
                    for (int itemCount = 0; itemCount < this.gridItemDetails.Rows.Count; itemCount++)
                    {

                        if (this.gridItemDetails.Rows[itemCount].Cells[clsPOSDBConstants.POHeader_Fld_VendorID].Value != DBNull.Value)
                        {
                            int valueVendorId = (int)this.gridItemDetails.Rows[itemCount].Cells[clsPOSDBConstants.POHeader_Fld_VendorID].Value;
                            if (valueVendorId == vendorID)
                            {
                                this.gridItemDetails.Rows[itemCount].Selected = true;
                            }
                        }
                    }
                }
                this.gridItemDetails.Refresh();
            }
            else
            {
                this.gridPODetails.ActiveRow.ParentRow.Selected = true;
                itemCode = gridPODetails.ActiveRow.Cells["ItemId"].Text;
                for (int itemCount = 0; itemCount < this.gridItemDetails.Rows.Count; itemCount++)
                {
                    string valueItemId = this.gridItemDetails.Rows[itemCount].Cells["ItemId"].Text;
                    if (valueItemId == itemCode)
                    {
                        this.gridItemDetails.Rows[itemCount].Selected = true;
                        this.gridItemDetails.Rows[itemCount].Activate();
                        break;
                    }
                }
            }
        }
        private void frmCreateNewPurchaseOrder_Activated(object sender, EventArgs e)
        {
            this.Top = 5;
            this.Left = 20;
        }
        private void gridPODetails_InitializeRow(object sender, InitializeRowEventArgs e)
        {
            if (e.ReInitialize)
            {
                return;
            }
            if (e.Row.Band.Key != clsPOSDBConstants.POItemRelationName && e.Row.Band.Key != clsPOSDBConstants.MasterPOItemRelationName)
            {
                e.Row.Cells[clsPOSDBConstants.OrderDetail_Fld_CloseOrder].Value = "Send All PO(s)(F11)";
            }
        }
        private void btnClearGrid_Click(object sender, EventArgs e)
        {
            try
            {
                string message = "Do you want to clear all items?";
                // Added bu Shrikant Mali on 10/02/2014, this variable holds the the count of case items.
                int caseItemCount;
                if (gridItemDetails.Rows.Count > 0)
                {
                    clsUIHelper.ShowInfoMsg(message);
                    if (clsUIHelper.IsOK)
                    {
                        for (int rowCnt = gridItemDetails.Rows.Count; rowCnt > 0; rowCnt--)
                        {
                            gridItemDetails.Rows[rowCnt - 1].Delete(false);
                            //Added By SRT(Gaurav) Date: 17-Jul-2009
                            txtEditorNoOfItems.Text = TotalItemsCount.ToString();
                            txtEditorNoOfVendors.Text = TotalVendorsCount.ToString();
                            int csItemCount = CSItemCount;
                            txtEditorNoOfCSItems.Text = csItemCount.ToString();
                            // Added by Shrikant Mali on 02/10/2014 to update the count of non case Items.
                            tbxNonCaseItems.Text = GetNonCaseItemCount(csItemCount).ToString();
                            //this.txtTotalCostForAllPO.Text = totalCostForAllPO.ToString();//Added By shitaljit for all PO(s) total cost   //Sprint-21 - 1803 15-Sep-2015 JY Commented
                        }
                        gridItemDetails.Refresh();
                        gridPODetails.Refresh();
                        for (int rowCnt = gridPODetails.Rows.Count; rowCnt > 0; rowCnt--)
                        {
                            try
                            {
                                gridPODetails.Rows[rowCnt - 1].Delete(false);
                            }
                            catch (Exception Ex)
                            {
                            }
                            //Added By SRT(Gaurav) Date: 17-Jul-2009
                            txtEditorNoOfItems.Text = TotalItemsCount.ToString();
                            txtEditorNoOfVendors.Text = "0";
                            this.txtEditorNoOfVendors.Text = "0";//Added By shitaljit for all PO(s) total cost
                            caseItemCount = CSItemCount;
                            txtEditorNoOfCSItems.Text = caseItemCount.ToString();
                            // Added by Shrikant Mali on 02/10/2014 to update the count of non case Items.
                            tbxNonCaseItems.Text = GetNonCaseItemCount(caseItemCount).ToString();
                        }
                        //Added By SRT(Gaurav) Date :  14-Jul-2009
                        //This code is added to solve issue of Focus on Item Grid after invoking
                        //Clear option.
                        FocusNewItemId();
                        //End Of Added By SRT(Gaurav)
                        //Added By SRT(Gaurav) Date: 17-Jul-2009
                        txtEditorNoOfItems.Text = TotalItemsCount.ToString();
                        txtEditorNoOfVendors.Text = TotalVendorsCount.ToString();
                        caseItemCount = CSItemCount;
                        txtEditorNoOfCSItems.Text = caseItemCount.ToString();
                        // Added by Shrikant Mali on 02/10/2014 to update the count of non case Items.
                        tbxNonCaseItems.Text = GetNonCaseItemCount(caseItemCount).ToString();
                        if (isEditPO)
                        {
                            this.Close();
                            return;
                        }
                        this.txtTotalCostForAllPO.Text = "0";   //Sprint-21 - 1803 14-Sep-2015 JY Added
                    }
                    else
                    {
                        FocusNewItemId();
                        return;
                    }
                }
                else
                {
                    clsUIHelper.ShowErrorMsg("There are no items in grid.");
                    //Added By SRT(Ritesh Parekh) Date : 19-Aug-2009
                    FocusNewItemId();
                    //End Of Added By SRT(Ritesh Parekh)
                }
                //Added By SRT(Gaurav) Date: 17-Jul-2009

                if (gridItemDetails.Rows.Count == 1)
                    this.pnlAutoGenPO.Enabled = true;

                txtEditorNoOfItems.Text = TotalItemsCount.ToString();
                txtEditorNoOfVendors.Text = TotalVendorsCount.ToString();
                caseItemCount = CSItemCount;
                txtEditorNoOfCSItems.Text = caseItemCount.ToString();
                // Added by Shrikant Mali on 02/10/2014 to update the count of non case Items.
                tbxNonCaseItems.Text = GetNonCaseItemCount(caseItemCount).ToString();
                //this.txtTotalCostForAllPO.Text = totalCostForAllPO.ToString();//Added By shitaljit for all PO(s) total cost   //Sprint-21 - 1803 15-Sep-2015 JY Commented
            }
            catch (Exception Ex)
            {
                logger.Fatal(Ex, "btnClearGrid_Click(object sender, EventArgs e)");
                clsUIHelper.ShowErrorMsg(Ex.Message);
                //Added By SRT(Ritesh Parekh) Date : 19-Aug-2009
                FocusNewItemId();
                //End Of Added By SRT(Ritesh Parekh)
            }
        }

        /// <summary>
        /// Auther : Shrikant Mali
        /// Date : 02/10/2014
        /// Calculates the number of non case items.
        /// </summary>
        /// <param name="caseItemCount">The Case items count</param>
        /// <returns>The Number of Non Case Item count in Item detail grid view.</returns>
        private int GetNonCaseItemCount(int caseItemCount)
        {
            return (gridItemDetails.Rows.Count - caseItemCount);
        }

        private void gridPODetails_AfterRowUpdate(object sender, RowEventArgs e)
        {
            string vendorID = string.Empty;
            try
            {
                vendorID = gridPODetails.ActiveRow.Cells["VendorID"].Value.ToString();
                CalculateTotals(vendorID);
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "gridPODetails_AfterRowUpdate(object sender, RowEventArgs e)");
                clsUIHelper.ShowErrorMsg(ex.Message);
            }
        }
        private int GetMaxPODID()
        {
            int podID = 0;
            try
            {
                if (isEditPO)
                {

                    if (orderDetailData == null)
                    {
                        orderDetailData = new OrderDetailsData();
                        this.FillVendors();
                        //SetNew();
                    }
                    orderDetailData.PODetailTable = masterOrderDetailsData.PODetailTable;
                }

                try
                {
                    if (orderDetailData.PODetailTable.Rows.Count > 0)
                    {
                        podID = Convert.ToInt32(orderDetailData.PODetailTable.Compute("max([" + clsPOSDBConstants.PODetail_Fld_PODetailID + "])", string.Empty));   //PRIMEPOS-3155 06-Oct-2022 JY Added
                    }
                }
                catch
                {
                    int tempPodId = 0;
                    int rowCount = orderDetailData.PODetailTable.Rows.Count;
                    for (int count = 0; count < rowCount; count++)
                    {
                        tempPodId = (int)orderDetailData.PODetailTable.Rows[count][clsPOSDBConstants.PODetail_Fld_PODetailID];
                        if (tempPodId > podID)
                        {
                            podID = tempPodId;
                        }
                    }
                }
                podID++;
            }
            catch (Exception ex)
            {
            }
            return podID;
        }

        private int GetExistPODID(String orderNo)
        {
            int rowCount = 0;
            int podID = 0;
            try
            {
                foreach (KeyValuePair<string, string> pair in podIdOrdernoDict)
                {
                    if (pair.Value == orderNo)
                    {
                        podID = Convert.ToInt32(pair.Key);
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return podID;
        }
        private int GetMaxOrderID()
        {
            int rowCount = 0;
            int tempPoId = 0;
            int poID = 0;
            try
            {
                if (isEditPO)
                {
                    orderDetailData.OrderDetailsTable = masterOrderDetailsData.OrderDetailsTable;
                }
                rowCount = orderDetailData.OrderDetailsTable.Rows.Count;
                for (int count = 0; count < rowCount; count++)
                {
                    tempPoId = (int)orderDetailData.OrderDetailsTable.Rows[count][clsPOSDBConstants.OrderDetail_Tbl_OrderId];
                    if (tempPoId > poID)
                    {
                        poID = tempPoId;
                    }
                }
                poID++;
            }
            catch (Exception ex)
            {
            }
            return poID;
        }
        private int GetMaxOrderID(DataRow[] ordetailRows)
        {
            int rowCount = 0;
            int tempOrderId = 0;
            int orderID = 0;
            try
            {
                for (int count = 0; count < ordetailRows.Length; count++)
                {
                    tempOrderId = (int)ordetailRows[count][clsPOSDBConstants.POHeader_Fld_OrderID];
                    if (tempOrderId > orderID)
                    {
                        orderID = tempOrderId;
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return orderID;
        }
        private void gridItemDetails_InitializeRow(object sender, InitializeRowEventArgs e)
        {
            try
            {
                if (!e.ReInitialize)
                {
                    string podID = string.Empty;
                    try
                    {
                        //if (isAutoPO)
                        //{
                        //    int poDetailId = 0;
                        //    poDetailId = GetMaxPODID();
                        //    podID = e.Row.Cells[clsPOSDBConstants.PODetail_Fld_PODetailID].Value.ToString();
                        //    if (podID == String.Empty)
                        //    {
                        //        e.Row.Cells[clsPOSDBConstants.PODetail_Fld_PODetailID].Value = poDetailId;
                        //    }
                        //}
                        if (isEditPO)
                        {
                            List<String> orders = null;
                            string itemOrderNo = null;
                            string vendorID = string.Empty;
                            string itemID = string.Empty;

                            orderNosDropDown = new UltraDropDown();
                            this.orderNosDropDown.RowSelected += new RowSelectedEventHandler(orderNosDropDown_RowSelected);
                            vendorID = e.Row.Cells[clsPOSDBConstants.POHeader_Fld_VendorID].Value.ToString();
                            itemID = e.Row.Cells[clsPOSDBConstants.PODetail_Fld_ItemID].Value.ToString();

                            podID = e.Row.Cells[clsPOSDBConstants.PODetail_Fld_PODetailID].Value.ToString();

                            poOrdersNos.TryGetValue(vendorID, out orders);
                            podIdOrdernoDict.TryGetValue(podID, out itemOrderNo);
                            orderNosDropDown.DataSource = orders;
                            e.Row.Cells[clsPOSDBConstants.Item_Fld_Description].Activation = Activation.ActivateOnly;
                            e.Row.Cells[clsPOSDBConstants.OrderDetail_Fld_OrderNo].Value = itemOrderNo;
                            e.Row.Cells[clsPOSDBConstants.OrderDetail_Fld_OrderNo].ValueList = orderNosDropDown;
                        }
                    }
                    catch (Exception ex)
                    {
                    }
                }
                //e.Row.Cells[clsPOSDBConstants.PODetail_Fld_VendorName].DroppedDown = false;   //PRIMEPOS-3155 06-Oct-2022 JY Commented
            }
            catch (Exception ex)
            {
            }
        }
        private void gridItemDetails_AfterRowInsert(object sender, RowEventArgs e)
        {
            e.Row.Cells[clsPOSDBConstants.PODetail_Fld_ItemID].Activate();
            if (!isAutoPO)  //PRIMEPOS-3155 06-Oct-2022 JY Added if condition
            {
                e.Row.Cells[clsPOSDBConstants.PODetail_Fld_ItemID].Activation = Activation.AllowEdit;
                e.Row.Cells[clsPOSDBConstants.PODetail_Fld_VendorName].ValueList = vendorDropDown;
                FocusItemId();
            }
        }

        private void FindMaxIDs(out int maxPoId)
        {
            //int countOfID = 0;
            int poID = 0;

            try
            {
                //String sql = "select * from " + clsPOSDBConstants.POHeader_tbl;
                //Search oBLSearch = new Search();
                //DataSet poHeaderData = oBLSearch.SearchData(sql);
                //countOfID = poHeaderData.Tables[0].Rows.Count;
                //for (int count = 0; count < countOfID; count++)
                //{
                //    if (poID < (int)poHeaderData.Tables[0].Rows[count][clsPOSDBConstants.POHeader_Fld_OrderID])
                //    {
                //        poID = (int)poHeaderData.Tables[0].Rows[count][clsPOSDBConstants.POHeader_Fld_OrderID];
                //    }
                //}
                //poID++;

                #region PRIMEPOS-2825 06-Apr-2020 JY Added
                String sql = "SELECT ISNULL(MAX(OrderId) + 1,1) FROM PurchaseOrder";
                Search oBLSearch = new Search();
                DataSet poHeaderData = oBLSearch.SearchData(sql);
                if (poHeaderData != null && poHeaderData.Tables.Count > 0 && poHeaderData.Tables[0].Rows.Count > 0)
                    poID = Configuration.convertNullToInt(poHeaderData.Tables[0].Rows[0][0]);
                #endregion
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "FindMaxIDs(out int maxPoId)");
                clsUIHelper.ShowErrorMsg(ex.Message);
            }
            maxPoId = poID;
        }
        private void frmCreateNewPurchaseOrder_Shown(object sender, EventArgs e)
        {
            POHeaderData poHeaderData = null;
            POHeaderRow poHeaderRow = null;
            int poIndex = 0;
            int tempPONo = 0;
            int poID = 0;

            if (isEditPO)
            {
                masterOrderDetailsData = new MasterOrderDetailsData();
                this.FillVendors();
                this.gridPODetails.DataSource = masterOrderDetailsData.MasterOrderDetailTable;
                this.gridPODetails.Refresh();
                this.gridItemDetails.DataSource = masterOrderDetailsData.PODetailTable;
                this.gridItemDetails.Refresh();
                //Clear the dictionaries.
                ClearLists();
                if (IsPartiallyAck || IsCopyOrder)
                {
                    FindMaxIDs(out poID);
                }
                orderNumber = Convert.ToInt32(oPurchaseOrder.GetNextPONumber());
                IEnumerator<String> keyEnum = poHeaderList.Keys.GetEnumerator();
                this.ApplyGrigFormat();
                while (keyEnum.MoveNext())
                {
                    poHeaderList.TryGetValue(keyEnum.Current, out poHeaderData);
                    poHeaderRow = poHeaderData.POHeader[0];
                    if (IsPartiallyAck || IsCopyOrder)
                    {
                        //poID++;
                        if (poID == 0) poID++;  //PRIMEPOS-2825 02-Apr-2020 JY Added
                        orderNumber++;
                        tempPONo = orderNumber;
                        newlyAddedOrderIDs.Add(poID.ToString());
                    }
                    else
                    {
                        poID = poHeaderRow.OrderID;
                        tempPONo = Convert.ToInt32(poHeaderRow.OrderNo);
                    }

                    FillInCompleteOrders(poHeaderRow, tempPONo.ToString(), poID.ToString());
                    FillMasterPoDetailsForIncompleteOrders(tempPONo.ToString(), poID.ToString());
                    poIndex++;
                }

                #region Sprint-21 - 1803 17-Sep-2015 JY Added to improve performance
                string VendorID = string.Empty;
                DataRow[] masterDetailRow = null;
                DataRow[] VendorOrders = null;
                masterOrderDetailsData.AcceptChanges();
                for (int VendCnt = 0; VendCnt < this.masterOrderDetailsData.MasterOrderDetailTable.Rows.Count; VendCnt++)
                {
                    VendorID = this.masterOrderDetailsData.MasterOrderDetailTable.Rows[VendCnt]["VendorId"].ToString();

                    masterDetailRow = masterOrderDetailsData.MasterOrderDetailTable.Select("VendorID = '" + VendorID + "'");
                    VendorOrders = masterOrderDetailsData.OrderDetailsTable.Select("VendorID = '" + VendorID + "'");
                    for (int VendOrdCnt = 0; VendOrdCnt < VendorOrders.Length; VendOrdCnt++)
                    {
                        string OrderID = string.Empty;
                        int totalQty = 0;
                        decimal totalCost = 0.00M;
                        DataRow[] poDetailRow = null;
                        DataRow[] ordDetailsRow = null;

                        OrderID = VendorOrders[VendOrdCnt]["OrderID"].ToString();
                        poDetailRow = masterOrderDetailsData.PODetailTable.Select("OrderID = '" + OrderID + "'");
                        if (poDetailRow.Length > 0)
                        {
                            ordDetailsRow = masterOrderDetailsData.OrderDetailsTable.Select("OrderID = '" + OrderID + "'");
                            if (ordDetailsRow.Length > 0)
                            {
                                for (int i = 0; i < poDetailRow.Length; i++)
                                {
                                    if (poDetailRow[i][clsPOSDBConstants.PODetail_Fld_QTY] != DBNull.Value)
                                        totalQty += Convert.ToInt32(poDetailRow[i][clsPOSDBConstants.PODetail_Fld_QTY]);
                                    if (poDetailRow[i][clsPOSDBConstants.PODetail_Fld_Cost] != DBNull.Value)
                                        totalCost += Convert.ToDecimal(poDetailRow[i][clsPOSDBConstants.PODetail_Fld_Cost]);
                                }
                                ordDetailsRow[0][clsPOSDBConstants.OrderDetail_Fld_TotalItems] = poDetailRow.Length;
                                ordDetailsRow[0][clsPOSDBConstants.OrderDetail_Fld_TotalQty] = totalQty;
                                ordDetailsRow[0][clsPOSDBConstants.OrderDetail_Fld_TotalCost] = totalCost;
                                ordDetailsRow[0][clsPOSDBConstants.OrderDetail_Tbl_CloseOrder] = "Send PO(F12)";
                            }
                        }
                        else
                        {
                            //Update count if order changes
                            string ordNO = string.Empty;

                            ordDetailsRow = masterOrderDetailsData.OrderDetailsTable.Select("OrderID = '" + OrderID + "' AND VendorID = '" + VendorID + "'");
                            List<String> ordNos = null;

                            if (ordDetailsRow.Length > 0)
                            {
                                ordNO = ordDetailsRow[0][clsPOSDBConstants.OrderDetail_Fld_OrderNo].ToString();
                                poOrdersNos.TryGetValue(VendorID, out ordNos);
                                if (ordNos.Contains(ordNO))
                                {
                                    ordNos.Remove(ordNO);
                                }
                                masterOrderDetailsData.OrderDetailsTable.Rows.Remove(ordDetailsRow[0]);
                                masterOrderDetailsData.OrderDetailsTable.AcceptChanges();
                            }
                            ordDetailsRow = masterOrderDetailsData.OrderDetailsTable.Select("VendorID = '" + VendorID + "'");
                            masterDetailRow = masterOrderDetailsData.MasterOrderDetailTable.Select("VendorID = '" + VendorID + "'");
                            if (ordDetailsRow.Length == 0)
                                masterOrderDetailsData.MasterOrderDetailTable.Rows.Remove(masterDetailRow[0]);
                            else
                                masterDetailRow[0][clsPOSDBConstants.MasterOrderDetails_Fld_TotalPOs] = ordDetailsRow.Length;
                        }
                    }
                    masterDetailRow[0][clsPOSDBConstants.MasterOrderDetails_Fld_TotalPOs] = VendorOrders.Length;
                }
                masterOrderDetailsData.MasterOrderDetailTable.AcceptChanges();
                this.gridPODetails.Refresh();
                #endregion

                this.FormatGrid("EDIT");
                if (masterOrderDetailsData.PODetailTable != null)
                {
                    this.txtEditorNoOfItems.Text = masterOrderDetailsData.PODetailTable.Rows.Count.ToString();

                    #region Sprint-21 - 1803 11-Sep-2015 JY Added to update case and non-case item count
                    int csItemCount = CSItemCount;
                    txtEditorNoOfCSItems.Text = csItemCount.ToString();
                    tbxNonCaseItems.Text = GetNonCaseItemCount(csItemCount).ToString();
                    #endregion
                }
                this.gridItemDetails.UpdateData();
                this.gridItemDetails.Refresh();
                FocusNewItemId();
            }
            else
            {
                this.FocusItemId(true);
            }
        }

        private void ClearLists()
        {
            this.partiallyAckPOIDs.Clear();
            this.newlyAddedPOIDs.Clear();
        }
        public string PO_ID
        {
            set
            {
                this.poID = value;
            }
        }
        public bool IsEditPO
        {
            set
            {
                this.isEditPO = value;
            }
        }
        public int TotalNoOfOrders
        {
            get
            {
                return this.totalNoOfPOs;
            }
        }
        private void gridPODetails_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyData == Keys.F11 || e.KeyData == Keys.F12)
                {
                    CellEventArgs arg = new CellEventArgs(this.gridPODetails.ActiveCell);
                    this.gridPODetails_ClickCellButton(this.gridPODetails, arg);
                }
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "gridPODetails_KeyUp(object sender, KeyEventArgs e)");
                clsUIHelper.ShowErrorMsg(ex.Message);
            }
        }
        //private void gridItemDetails_BeforeRowDeactivate(object sender, CancelEventArgs e)
        //{
        //    try
        //    {
        //        //String itemId = this.gridItemDetails.ActiveRow.Cells[clsPOSDBConstants.PODetail_Fld_ItemID].Value.ToString();      
        //        //if (IsInComplete != true)
        //        //{
        //        //    PODetailSvr poDetSvr = new PODetailSvr();
        //        //    PurchaseOrder purchaseOrd = new PurchaseOrder();
        //        //    PODetailData poDetData = new PODetailData();
        //        //    POHeaderSvr poHeaderSvr = new POHeaderSvr();
        //        //    poDetData = poDetSvr.PopulateList(" AND " + clsPOSDBConstants.PODetail_tbl + "." + clsPOSDBConstants.PODetail_Fld_ItemID + " = '" + itemId + "'");
        //        //    POHeaderData poData = new POHeaderData();
        //        //    String str = String.Empty;
        //        //    int count = 0;


        //        //    if (poDetData.PODetail.Rows.Count > 0)
        //        //    {
        //        //        foreach (PODetailRow poDetailRow in poDetData.PODetail.Rows)
        //        //        {
        //        //            POHeaderData poHeaderData = purchaseOrd.PopulateHeader(poDetData.PODetail[count].OrderID);
        //        //            poData.Merge(poHeaderData);
        //        //            str = str + " Purchase Order (" + poData.POHeader[count].OrderNo + " ," + ((PurchseOrdStatus)poData.POHeader[count].Status).ToString() + ") already has Item (" + poDetailRow.ItemID + " ," + poDetailRow.ItemDescription + ") \n ";
        //        //            count++;
        //        //        }
        //        //    }
        //        //}
        //        //}

        //        //if (IsLoaded == true)
        //        //    IsInComplete = false;
        //    }
        //    catch (Exception ex)
        //    {
        //        clsUIHelper.ShowErrorMsg(ex.Message);
        //    }
        //}

        private void ultraBtnPrintPreview_Click(object sender, EventArgs e)
        {
            #region this button is disabled 
            //if (gridPODetails.Rows.Count > 0)
            //{
            //    int ordersCount = 0;
            //    string vendorID = string.Empty;
            //    string vendorName = string.Empty;
            //    string orderIDS = string.Empty;
            //    DataSet reportDs = null;
            //    DataRow[] reportRows = null;
            //    int orderNO = 0;
            //    try
            //    {
            //        if (isEditPO)
            //        {
            //            #region Commented
            //            //orderIDS = "(";
            //            //ordersCount = this.gridPODetails.ActiveRow.ChildBands[0].Rows.Count;
            //            //for (int count = 0; count < ordersCount; count++)
            //            //{
            //            //    orderIDS += "'" + this.gridPODetails.ActiveRow.ChildBands[0].Rows[count].Cells[clsPOSDBConstants.POHeader_Fld_OrderID].Value.ToString() + "',";
            //            //    //clsUIHelper.PrintPurchaseOrder(orderIDS, false);
            //            //}
            //            //orderIDS = orderIDS.Substring(0, orderIDS.Length - 1);
            //            //orderIDS += ")";
            //            //vendorID = this.gridPODetails.ActiveRow.Cells[clsPOSDBConstants.POHeader_Fld_VendorID].Value.ToString();
            //            //clsUIHelper.PrintPurchaseOrder(vendorID, orderIDS, false);
            //            // clsUIHelper.PrintPurchaseOrder(String.Empty,String.Empty,"NO", false);
            //            #endregion
            //            clsUIHelper.PrintPurchaseOrder(false);
            //        }
            //        else
            //        {
            //            reportDs = new DataSet();
            //            reportDs.Tables.Add(orderDetailData.PODetailTable.Clone());
            //            orderNO = (int)this.gridPODetails.ActiveRow.Cells["Order NO"].Value;
            //            vendorID = this.gridPODetails.ActiveRow.Cells[clsPOSDBConstants.POHeader_Fld_VendorID].Value.ToString();

            //            reportRows = orderDetailData.PODetailTable.Select("VendorID = " + vendorID);
            //            reportDs.Tables[clsPOSDBConstants.PODetail_tbl].Columns.Add("OrderNO");
            //            reportDs.Tables[clsPOSDBConstants.PODetail_tbl].Columns.Add("OrderDate");
            //            //Start Added By Amit date 17 June 2011
            //            reportDs.Tables[clsPOSDBConstants.PODetail_tbl].Columns.Add("BarcodeImg",System.Type.GetType("System.Byte[]"));
            //            //
            //            reportDs.AcceptChanges();
            //            vendorDetails.TryGetValue(vendorID, out vendorName);

            //            for (int count = 0; count < reportRows.Length; count++)
            //            {
            //                reportRows[count].AcceptChanges();
            //                reportDs.Tables[0].Rows.Add(reportRows[count].ItemArray);
            //            }

            //            int i = 0;
            //            string ItemID = null;
            //            string BarCode = null;
            //            int RowIndex = 0;
            //            foreach (DataRow dr in reportDs.Tables[0].Rows)
            //            {
            //                dr["OrderNO"] = orderNO;
            //                dr["OrderDate"] = DateTime.Now;
            //                //Added By shitaljit(Quicsolv) on 16 Feb 2012
            //                //To add Selling Price Column in PO report.
            //                Item oBRItem = new Item();
            //                ItemData oItemData = new ItemData();
            //                ItemRow oItemRow = null;
            //                string strItemID = "";
            //                strItemID = ItemID = dr[clsPOSDBConstants.PODetail_Fld_ItemID].ToString();
            //                oItemData = oBRItem.Populate(strItemID);
            //                oItemRow = oItemData.Item[0];
            //                reportDs.Tables[clsPOSDBConstants.PODetail_tbl].Rows[RowIndex]["SellingPrice"] =
            //                    oItemRow.SellingPrice.ToString(Configuration.CInfo.CurrencySymbol + "######0.00");
            //                RowIndex++;
            //                //End of Added By shitaljit(Quicsolv) on 16 Feb 2012
            //                //Added By Amit Date 17 June 2011

            //                if (strBarCodeOf=="VendItmCode")
            //                {
            //                    ItemID = dr[clsPOSDBConstants.PODetail_Fld_VendorItemCode].ToString();
            //                    BarCode = "Vendor Item Code";
            //                }
            //                else if (strBarCodeOf == "ItemID")
            //                {
            //                    ItemID = dr[clsPOSDBConstants.PODetail_Fld_ItemID].ToString();
            //                    BarCode = "Item ID";
            //                }
            //                else
            //                {
            //                    ItemID = "";
            //                    BarCode = "None";
            //                }

            //                try
            //                {
            //                    if(ItemID!="")
            //                    Configuration.PrintBarcode(ItemID, 0, 0, 20, 200, "CODE128", "H", System.IO.Path.GetTempPath() + "\\" +ItemID + ".bmp");
            //                }
            //                catch (Exception ex) 
            //                { 
            //                }
            //                if (ItemID != "")
            //                    reportDs.Tables[clsPOSDBConstants.PODetail_tbl].Rows[i]["BarcodeImg"] = Configuration.GetImageData(System.IO.Path.GetTempPath() + "\\" + ItemID + ".bmp");
            //                else
            //                    reportDs.Tables[clsPOSDBConstants.PODetail_tbl].Rows[i]["BarcodeImg"] = null;
            //                i++;
            //            }
            //            //Commented By Amit Date 21 June 2011
            //            //clsUIHelper.PrintPurchaseOrder(reportDs,false);
            //            clsUIHelper.PrintPurchaseOrder(reportDs,false, BarCode);
            //        }                  
            //    }
            //    catch (Exception ex)
            //    {
            //        clsUIHelper.ShowErrorMsg(ex.Message);
            //        //Added By SRT(Ritesh Parekh) Date : 19-Aug-2009
            //        FocusNewItemId();
            //        //End Of Added By SRT(Ritesh Parekh)
            //    }
            //}
            //else
            //{
            //    clsUIHelper.ShowErrorMsg("There are No PO To Print.");
            //    //Added By SRT(Ritesh Parekh) Date : 19-Aug-2009
            //    FocusNewItemId();
            //    //End Of Added By SRT(Ritesh Parekh)
            //}
            #endregion
        }

        private void ultraBtnPrint_Click(object sender, EventArgs e)
        {
            if (gridPODetails.Rows.Count > 0)
            {
                //Added by Amit Date 25 june 2011
                frmPOPrintOptions POPrintoptions = new frmPOPrintOptions();
                DialogResult Result = POPrintoptions.ShowDialog();
                POPrintoptions.BringToFront();

                strBarCodeOf = frmPOPrintOptions.strBarCodeOf;

                if (Result == DialogResult.OK)      //Added By Amit Date 25 June 2011
                {
                    #region Code For Print Preview PO
                    DataSet dsReportData = null;
                    try
                    {
                        string BarCode = string.Empty;
                        dsReportData = PrepareReportData(out BarCode);  //PRIMEPOS-2585 25-Sep-2018 JY moved logic inside different function 
                        if (dsReportData != null)
                            clsUIHelper.PrintPurchaseOrder(dsReportData, false, BarCode);
                    }
                    catch (Exception ex)
                    {
                        logger.Fatal(ex, "ultraBtnPrint_Click(object sender, EventArgs e)");
                        clsUIHelper.ShowErrorMsg(ex.Message);
                        FocusNewItemId();
                    }
                    #endregion
                }
                else if (Result == DialogResult.Yes)
                {
                    #region PRIMEPOS-2585 25-Sep-2018 JY Added code to Print PO
                    DataSet dsReportData = null;
                    try
                    {
                        string BarCode = string.Empty;
                        dsReportData = PrepareReportData(out BarCode);
                        if (dsReportData != null)
                            clsUIHelper.PrintPurchaseOrder(dsReportData, true, BarCode);
                    }
                    catch (Exception ex)
                    {
                        logger.Fatal(ex, "ultraBtnPrint_Click(object sender, EventArgs e)");
                        clsUIHelper.ShowErrorMsg(ex.Message);
                        FocusNewItemId();
                    }
                    #endregion

                    #region Code For Print PO
                    //int ordersCount = 0;
                    //string orderID = string.Empty;

                    //if (!isEditPO)
                    //{
                    //    try
                    //    {
                    //        if (this.gridPODetails.ActiveRow == null)
                    //        {
                    //            clsUIHelper.ShowErrorMsg("Please Select Any PO To Print.");
                    //            return;
                    //        }
                    //        ordersCount = this.gridPODetails.ActiveRow.ChildBands[0].Rows.Count;
                    //        for (int count = 0; count < ordersCount; count++)
                    //        {
                    //            orderID = this.gridPODetails.ActiveRow.ChildBands[0].Rows[count].Cells[clsPOSDBConstants.POHeader_Fld_VendorID].Value.ToString();
                    //            clsUIHelper.PrintPurchaseOrder(orderID, true, true);
                    //        }
                    //    }
                    //    catch (Exception ex)
                    //    {
                    //        clsUIHelper.ShowErrorMsg(ex.Message);
                    //        //Added By SRT(Ritesh Parekh) Date : 19-Aug-2009
                    //        FocusNewItemId();
                    //        //End Of Added By SRT(Ritesh Parekh)
                    //    }
                    //}
                    //else
                    //{
                    //    try
                    //    {
                    //        if (this.gridPODetails.ActiveRow == null)
                    //        {
                    //            clsUIHelper.ShowErrorMsg("Please Select Any PO To Print.");
                    //            return;
                    //        }
                    //        ordersCount = this.gridPODetails.ActiveRow.ChildBands[0].Rows.Count;
                    //        for (int count = 0; count < ordersCount; count++)
                    //        {
                    //            orderID = this.gridPODetails.ActiveRow.ChildBands[0].Rows[count].Cells[clsPOSDBConstants.POHeader_Fld_OrderID].Value.ToString();
                    //            clsUIHelper.PrintPurchaseOrder(orderID, true, true);
                    //        }
                    //    }
                    //    catch (Exception ex)
                    //    {
                    //        clsUIHelper.ShowErrorMsg(ex.Message);
                    //        //Added By SRT(Ritesh Parekh) Date : 19-Aug-2009
                    //        FocusNewItemId();
                    //        //End Of Added By SRT(Ritesh Parekh)
                    //    }
                    //}
                    #endregion
                }
            }
            else
            {
                clsUIHelper.ShowErrorMsg("There are No PO To Print.");
                //Added By SRT(Ritesh Parekh) Date : 19-Aug-2009
                FocusNewItemId();
                //End Of Added By SRT(Ritesh Parekh)
            }
        }

        private DataSet PrepareReportData(out string BarCode)
        {
            DataSet dsReportData = new DataSet();
            BarCode = String.Empty;
            try
            {
                string vendorID = string.Empty;
                string vendorName = string.Empty;
                string orderIDS = string.Empty;
                DataRow[] reportRows = null;
                int orderNO = 0;
                int OrderId = 0;
                if (!isEditPO)
                {
                    try
                    {
                        dsReportData.Tables.Add(orderDetailData.PODetailTable.Clone());
                        orderNO = Configuration.convertNullToInt(this.gridPODetails.ActiveRow.Cells["Order NO"].Value);
                        vendorID = Configuration.convertNullToString(this.gridPODetails.ActiveRow.Cells[clsPOSDBConstants.POHeader_Fld_VendorID].Value);
                        reportRows = orderDetailData.PODetailTable.Select("VendorID = " + vendorID);
                    }
                    catch
                    {
                        throw (new Exception("Please Select Any PO To Print."));
                    }
                }
                else
                {
                    try
                    {
                        orderNO = Configuration.convertNullToInt(this.gridPODetails.ActiveRow.Cells["Order No"].Value);
                        OrderId = Configuration.convertNullToInt(this.gridPODetails.ActiveRow.Cells["OrderId"].Value);
                    }
                    catch
                    {
                        throw (new Exception("Please Select Any PO To Print."));
                    }

                    if (orderDetailData != null && orderDetailData.Tables.Count > 0)
                    {
                        dsReportData.Tables.Add(orderDetailData.PODetailTable.Clone());
                    }
                    else
                    {
                        if (masterOrderDetailsData != null && masterOrderDetailsData.Tables.Count > 0 && masterOrderDetailsData.PODetailTable != null)
                        {
                            orderDetailData = new OrderDetailsData();
                            foreach (PODetailRow rw in masterOrderDetailsData.PODetailTable)
                            {
                                orderDetailData.PODetailTable.Rows.Add(rw.ItemArray);
                            }
                            dsReportData.Tables.Add(orderDetailData.PODetailTable.Clone());
                        }
                    }
                    if (masterOrderDetailsData != null && masterOrderDetailsData.OrderDetailsTable != null && masterOrderDetailsData.OrderDetailsTable.Rows.Count > 0)
                    {
                        vendorID = Configuration.convertNullToString(this.gridPODetails.ActiveRow.Cells[clsPOSDBConstants.POHeader_Fld_VendorID].Value);
                    }
                    reportRows = orderDetailData.PODetailTable.Select("OrderID = " + OrderId);
                }

                dsReportData.Tables[clsPOSDBConstants.PODetail_tbl].Columns.Add("OrderNO");
                dsReportData.Tables[clsPOSDBConstants.PODetail_tbl].Columns.Add("OrderDate");

                dsReportData.Tables[clsPOSDBConstants.PODetail_tbl].Columns.Add("Tax");//PRIMEPOS-3274
                //dsReportData.Tables[clsPOSDBConstants.PODetail_tbl].Columns.Add("TotalSale");//PRIMEPOS-3274

                dsReportData.Tables[clsPOSDBConstants.PODetail_tbl].Columns.Add("BarcodeImg", System.Type.GetType("System.Byte[]"));

                dsReportData.Tables[clsPOSDBConstants.PODetail_tbl].Columns.Add("SellingPrice");//Added By Shitaljit 0n 16 Feb 2012
                dsReportData.AcceptChanges();
                vendorDetails.TryGetValue(vendorID, out vendorName);

                for (int count = 0; count < reportRows.Length; count++)
                {
                    reportRows[count].AcceptChanges();
                    dsReportData.Tables[0].Rows.Add(reportRows[count].ItemArray);
                }

                int i = 0;
                string ItemID = null;
                int RowIndex = 0;
                foreach (DataRow dr in dsReportData.Tables[0].Rows)
                {
                    dr["OrderNO"] = orderNO;
                    dr["OrderDate"] = DateTime.Now;

                    //Added By shitaljit(Quicsolv) on 16 Feb 2012
                    //To add Selling Price Column in PO report.
                    Item oBRItem = new Item();
                    ItemData oItemData = new ItemData();
                    ItemRow oItemRow = null;
                    string strItemID = "";
                    strItemID = ItemID = dr[clsPOSDBConstants.PODetail_Fld_ItemID].ToString();
                    oItemData = oBRItem.Populate(strItemID);
                    oItemRow = oItemData.Item[0];
                    #region PRIMEPOS-3274
                    //dsReportData.Tables[clsPOSDBConstants.PODetail_tbl].Rows[RowIndex]["SellingPrice"] = oItemRow.SellingPrice.ToString(Configuration.CInfo.CurrencySymbol + "######0.00"); ;
                    dsReportData.Tables[clsPOSDBConstants.PODetail_tbl].Rows[RowIndex]["SellingPrice"] = Convert.ToString(oItemRow.SellingPrice);
                    decimal TempItemTax = clsUIHelper.CalculateTaxPublic(dr[clsPOSDBConstants.PODetail_Fld_ItemID].ToString(), Convert.ToDecimal(dr[clsPOSDBConstants.PODetail_Fld_QTY]), Convert.ToDecimal(oItemRow.SellingPrice));
                    dsReportData.Tables[clsPOSDBConstants.PODetail_tbl].Rows[RowIndex]["Tax"] = Convert.ToString(TempItemTax); //PRIMEPOS-3274
                    //dsReportData.Tables[clsPOSDBConstants.PODetail_tbl].Rows[RowIndex]["TotalSale"] =
                    //    Convert.ToString((Convert.ToDecimal(dr[clsPOSDBConstants.PODetail_Fld_QTY]) * Convert.ToDecimal(oItemRow.SellingPrice)) + TempItemTax);
                    #endregion
                    RowIndex++;
                    //End of Added By shitaljit(Quicsolv) on 16 Feb 2012

                    if (strBarCodeOf == "VendItmCode")
                    {
                        ItemID = dr[clsPOSDBConstants.PODetail_Fld_VendorItemCode].ToString();
                        BarCode = "Vendor Item Code";
                    }
                    else if (strBarCodeOf == "ItemID")
                    {
                        ItemID = dr[clsPOSDBConstants.PODetail_Fld_ItemID].ToString();
                        BarCode = "Item ID";
                    }
                    else
                    {
                        ItemID = "";
                        BarCode = "None";
                    }

                    try
                    {
                        if (ItemID != "")
                            Configuration.PrintBarcode(ItemID, 0, 0, 20, 200, "CODE128", "H", System.IO.Path.GetTempPath() + "\\" + ItemID + ".bmp");
                    }
                    catch (Exception ex)
                    {
                    }
                    if (ItemID != "")
                        dsReportData.Tables[clsPOSDBConstants.PODetail_tbl].Rows[i]["BarcodeImg"] = Configuration.GetImageData(System.IO.Path.GetTempPath() + "\\" + ItemID + ".bmp");
                    else
                        dsReportData.Tables[clsPOSDBConstants.PODetail_tbl].Rows[i]["BarcodeImg"] = null;
                    i++;
                }
                return dsReportData;
            }
            catch (Exception Ex)
            {
                logger.Fatal(Ex, "PrepareReportData(out string BarCode)");
                clsUIHelper.ShowErrorMsg(Ex.Message);
                return null;
            }
        }

        private void gridPODetails_DoubleClick(object sender, EventArgs e)
        {
            frmOrderDetails frmOrderDetails = null;
            string ordDescription = string.Empty;
            string vendorCode = string.Empty;
            try
            {
                if (this.gridPODetails.ActiveRow == null)
                {
                    string message = "Please select the order";
                    clsUIHelper.ShowErrorMsg(message);
                    return;
                }

                if (this.gridPODetails.ActiveRow.Band.Key == clsPOSDBConstants.MasterOrderDetails_tbl || this.gridPODetails.ActiveRow.Band.Key == clsPOSDBConstants.POItemRelationName)
                {
                    return;
                }

                vendorCode = this.gridPODetails.ActiveRow.Cells[clsPOSDBConstants.OrderDetail_Fld_VendorName].Value.ToString();
                frmOrderDetails = new frmOrderDetails();
                frmOrderDetails.lblTransactionType.Text += vendorCode;
                frmOrderDetails.ShowDialog(this);
                frmOrderDetails.BringToFront();
                ordDescription = frmOrderDetails.OrderDescription;

                this.gridPODetails.ActiveRow.Cells[clsPOSDBConstants.POHeader_Fld_Description].Value = ordDescription;
                this.gridPODetails.UpdateData();
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "gridPODetails_DoubleClick(object sender, EventArgs e)");
                clsUIHelper.ShowErrorMsg(ex.Message);
            }
        }

        //private void gridItemDetails_BeforeCellDeactivate(object sender, CancelEventArgs e)
        //{
        #region Commented
        //UltraGridRow oCurrentRow;
        //UltraGridCell oCurrentCell;
        //oCurrentRow = this.gridItemDetails.ActiveRow;
        //oCurrentCell = null;
        //bool blnCellChanged;
        //blnCellChanged = false;
        //DataRow[] orgVendorRow = null;

        //foreach (UltraGridCell oCell in oCurrentRow.Cells)
        //{
        //    if (oCell.DataChanged == true || oCell.Text.Trim() != "")
        //    {
        //        blnCellChanged = true;
        //        break;
        //    }
        //}

        //if (gridItemDetails.ActiveRow.Cells[clsPOSDBConstants.PODetail_Fld_VendorName].IsActiveCell)
        //{
        //    string VendorNameOrg = this.gridItemDetails.ActiveRow.Cells[clsPOSDBConstants.PODetail_Fld_VendorName].OriginalValue.ToString();
        //    vendorSelected = this.gridItemDetails.ActiveRow.Cells[clsPOSDBConstants.PODetail_Fld_VendorName].Value.ToString();

        //    if(VendorNameOrg.Trim() == string.Empty || vendorSelected.Trim() == string.Empty)
        //    {
        //        return;
        //    }

        //    if(VendorNameOrg != vendorSelected)
        //    {
        //        orgVendorRow = vendorData.Vendor.Select(" VendorCode ='" + VendorNameOrg + "'");

        //        if (orgVendorRow.Length > 0)
        //        {
        //            string origVendorID = orgVendorRow[0][clsPOSDBConstants.Vendor_Fld_VendorId].ToString();
        //            bool isVendoExist = false;
        //            string selectedOrderID = this.gridItemDetails.ActiveRow.Cells[clsPOSDBConstants.PODetail_Fld_OrderID].Value.ToString();
        //            selectedPODetailIDFromVendorCombo = this.gridItemDetails.ActiveRow.Cells[clsPOSDBConstants.PODetail_Fld_PODetailID].Value.ToString();
        //            UpdateVendorSelectedInDropDown(vendorSelected, selectedOrderID, out isVendoExist);
        //            if (isVendoExist)
        //            {
        //                if (!isEditPO)
        //                {
        //                    CalculateTotals(origVendorID);
        //                }
        //                else
        //                {
        //                    vendorBeforeSelection = origVendorID;
        //                    selectedOrderIDFromVendorCombo = selectedOrderID;
        //                }
        //            }
        //            else
        //            {
        //                this.gridItemDetails.ActiveRow.Cells[clsPOSDBConstants.PODetail_Fld_VendorName].Value = VendorNameOrg;
        //                this.gridItemDetails.ActiveCell.CancelUpdate();
        //            }
        //            this.gridItemDetails.Refresh();
        //        }
        //    }
        //}
        #endregion
        //}

        private void frmCreateNewPurchaseOrder_KeyUp(object sender, KeyEventArgs e)
        {
            string searchStatus = string.Empty;
            try
            {
                #region Commented
                //if (selectedVendorRow.KeyData == System.Windows.Forms.Keys.F4)
                //{
                //    if (this.gridItemDetails.ContainsFocus == true)
                //    {
                //        if (this.gridItemDetails.ActiveCell != null)
                //        {
                //            if (this.gridItemDetails.ActiveCell.Column.Key == clsPOSDBConstants.PODetail_Fld_ItemID)
                //                searchStatus = this.SearchItem();

                //            if (searchStatus == "OrdItem-NO" || searchStatus == "ITEM-EXIST")
                //            {
                //                selectedVendorRow.Handled = true;
                //                return;
                //            }
                //            else
                //            {
                //                FocusQty();
                //            }
                //        }
                //    }
                //}
                #endregion
                if (e.KeyData == System.Windows.Forms.Keys.F2)
                {
                    if (this.pnlAddNewItem.Enabled == true)
                    {
                        System.EventArgs arg = new EventArgs();
                        this.btnAddNewItem_Click(this.btnAddNewItem, arg);
                    }
                }
                else if (e.KeyData == System.Windows.Forms.Keys.F6)
                {
                    if (this.pnlAutoGenPO.Visible == true && this.pnlAutoGenPO.Enabled == true)
                    {
                        try
                        {
                            isAutoPO = true;
                            //poDetailId = 0;
                            FillOrder();
                        }
                        catch { }
                        finally
                        {
                            isAutoPO = false;
                        }
                    }
                }
                else if (e.KeyData == System.Windows.Forms.Keys.F7)
                {
                    if (this.pnlBestPrice.Enabled == true)
                    {
                        System.EventArgs arg = new EventArgs();
                        this.ultraBtnBestPrice_Click(this.ultraBtnBestPrice, arg);
                    }
                }
                else if (e.KeyData == System.Windows.Forms.Keys.F3)
                {
                    EditItem();
                }
                else if (e.KeyData == System.Windows.Forms.Keys.F5)
                {
                    if (this.pnlDeleteSelectedItem.Enabled == true)
                    {
                        System.EventArgs arg = new EventArgs();
                        this.btnDeleteSelectedItem_Click(this.btnDeleteSelectedItem, arg);
                    }
                }
                else if (e.KeyData == System.Windows.Forms.Keys.F10)
                {
                    if (this.pnlItemHistory.Enabled == true)
                    {
                        System.EventArgs arg = new EventArgs();
                        this.ultraBtnItemHistory_Click(this.ultraBtnItemHistory, arg);
                    }
                }
                else if (e.KeyData == System.Windows.Forms.Keys.F8)
                {
                    if (this.pnlVendorHistory.Enabled == true)
                    {
                        System.EventArgs arg = new EventArgs();
                        this.ultraBtnVendorHistory_Click(this.ultraBtnVendorHistory, arg);
                    }
                }
                else if (e.KeyData == System.Windows.Forms.Keys.F9)
                {
                    if (this.gridItemDetails.ActiveRow != null)
                    {
                        string itemID = this.gridItemDetails.ActiveRow.Cells[clsPOSDBConstants.PODetail_Fld_ItemID].Value.ToString();
                        if (itemID != string.Empty)
                            this.gridItemDetails.ActiveRow.Update();
                    }
                    if (this.pnlCreateForAllVendors.Enabled == true)
                    {
                        System.EventArgs arg = new EventArgs();
                        this.btnCreateForAllVendors_Click(this.btnCreateForAllVendors, arg);
                    }
                }
                else if (e.KeyData == Keys.F11 || e.KeyData == Keys.F12)
                {
                    if (this.gridPODetails.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.OrderDetail_Fld_CloseOrder].CellActivation == Activation.AllowEdit)
                    {
                        CellEventArgs arg = new CellEventArgs(this.gridPODetails.ActiveCell);
                        this.gridPODetails_ClickCellButton(this.gridPODetails, arg);
                    }
                }
                #region PRIMEPOS-2648 01-Mar-2019 JY Added    
                else if (e.KeyData == System.Windows.Forms.Keys.F4)
                {
                    if (this.gridItemDetails.ContainsFocus == true && this.gridItemDetails.ActiveRow != null)
                    {
                        if (this.gridItemDetails.ActiveCell.Column.Key.Trim().ToUpper() == "ItemId".Trim().ToUpper())
                            SearchItem();
                    }
                }
                #endregion           
                e.Handled = true;
            }
            catch (Exception exp)
            {
                logger.Fatal(exp, "frmCreateNewPurchaseOrder_KeyUp(object sender, KeyEventArgs e)");
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }

        private void gridItemDetails_AfterRowsDeleted(object sender, EventArgs e)
        {
            this.gridItemDetails.UpdateData();
            if (isEditPO)
            {
                masterOrderDetailsData.AcceptChanges();
            }
            else
            {
                orderDetailData.AcceptChanges();
            }
        }

        //Added by Prashant(SRT) Date:11-7-09
        private void frmCreateNewPurchaseOrder_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.KeyCode == Keys.Escape)
            {
                //Added by Prashant(SRT) Date:17-Jul-09
                if (this.gridItemDetails.ActiveRow != null)
                {
                    //Added By SRT(Ritesh Parekh) Date : 27-Aug-2009
                    //To solve Esc issue on item grid.
                    string itemIDVal = this.gridItemDetails.ActiveRow.Cells[clsPOSDBConstants.PODetail_Fld_ItemID].Value.ToString();
                    string itemIDText = this.gridItemDetails.ActiveRow.Cells[clsPOSDBConstants.PODetail_Fld_ItemID].Text.ToString();
                    if (itemIDVal != string.Empty)
                    {
                        //Added By SRT(Ritesh Parekh) Date : 27-Aug-2009
                        //This code is added to solve esc scenario - if user deletes exiting item id text and press escape.
                        if (itemIDVal.Trim() != itemIDText.Trim())
                        {
                            this.gridItemDetails.ActiveRow.Cells[clsPOSDBConstants.PODetail_Fld_ItemID].CancelUpdate();
                            e.Handled = true;
                            return;
                        }
                        this.gridItemDetails.ActiveRow.Update();

                    }
                    #region commented
                    //Added by SRT(Abhishek) Date : 25 Aug 2009                   
                    else
                    {
                        //Updated condition By SRT(Ritesh Parekh) To Solve Esc issue. Date: 27-Aug-2009
                        if (this.gridItemDetails != null && this.gridItemDetails.Rows.Count > 0 && itemIDText.Trim().Length > 0)
                        {
                            this.gridItemDetails.ActiveRow.Delete(false);
                            //if(this.gridItemDetails.Rows.Count >0)
                            FocusNewItemId();
                            return;
                        }
                        //Added By SRT(Rtiesh Parekh) Date: 28-Aug-2009                        
                        else if (itemIDVal.Trim() == string.Empty && itemIDText.Trim() == string.Empty && IsThisRowValidated)
                        {
                            this.gridItemDetails.ActiveRow.Delete(false);
                            if (this.gridItemDetails.Rows.Count > 0)
                                FocusNewItemId();
                            return;
                        }
                    }
                    //Added by SRT(Abhishek) Date : 25 Aug 2009
                    #endregion
                }
                //End Added by Prashant(SRT) Date:17-Jul-09 

                System.EventArgs arg = new EventArgs();
                this.btnClose_Click(this.btnClose, arg);
                if (showOrderMsgBox != null && showOrderMsgBox.IsCancelled)
                {
                    e.Handled = true;
                    if (this.gridItemDetails.Rows.Count > 0)
                        FocusNewItemId();
                }
            }

            //Added By SRT(Gaurav) Date : 14-Jul-2009
            //This code is used to access Item Grid by Ctrl+Enter
            else if (e.KeyCode == Keys.Enter && e.Control)
            {
                FocusNewItemId();
                e.Handled = true;
            }
            else if ((e.KeyData == Keys.Down && IsThisRowEmpty(this.gridItemDetails.ActiveRow)))
            {
                // this.gridItemDetails.ActiveRow.Delete(false);
                //if (this.gridItemDetails.ActiveRow != null)
                //{
                //  this.gridItemDetails.UpdateData();
                //string itemID = this.gridItemDetails.ActiveRow.Cells[clsPOSDBConstants.PODetail_Fld_ItemID].Value.ToString();
                //if (itemID != "")
                //    DeleteGridRow(false);                    
                //}
            }
            else if (e.KeyData == Keys.Up && IsThisRowEmpty(this.gridItemDetails.ActiveRow))
            {
                if (this.gridItemDetails.ActiveRow != null)
                {
                    this.gridItemDetails.UpdateData();
                    //    //string itemID = this.gridItemDetails.ActiveRow.Cells[clsPOSDBConstants.PODetail_Fld_ItemID].Value.ToString();
                    //    //if (itemID != "")
                    //    //    DeleteGridRow(false);                    
                }
            }
            //End OF Added By SRT(Gaurav)
        }

        private void gridPODetails_AfterRowsDeleted(object sender, EventArgs e)
        {
            //Added by SRT(Abhishek) Date : 07/23/2009
            //Added to reset the count of vendor count
            orderDetailData.AcceptChanges();
            this.gridPODetails.Refresh();
            //End of Added by SRT(Abhishek) Date : 07/23/2009

            this.txtEditorNoOfItems.Text = TotalItemsCount.ToString();
            txtEditorNoOfVendors.Text = TotalVendorsCount.ToString();
            int csItemCount = CSItemCount;
            txtEditorNoOfCSItems.Text = csItemCount.ToString();
            // Added by Shrikant Mali on 02/10/2014 to update the count of non case Items.
            tbxNonCaseItems.Text = GetNonCaseItemCount(csItemCount).ToString();
            //this.txtTotalCostForAllPO.Text = totalCostForAllPO.ToString();//Added By shitaljit for all PO(s) total cost   //Sprint-21 - 1803 15-Sep-2015 JY Commented
        }

        private void frmCreateNewPurchaseOrder_FormClosed(object sender, FormClosedEventArgs e)
        {

            if (!IsReportCalled)
            {
                frmMain.getInstance().ShowInTaskbar = true;
                frmMain.getInstance().BringToFront();
            }
            Dispose(true);

        }

        /// <summary>
        /// Author : Ritesh Parekh
        /// Date: 28-Aug-2009
        /// When grid focus is activated, this event is invoked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //private void gridItemDetails_Enter(object sender, EventArgs e)
        //{

        //}

        private bool IsThisRowEmpty(UltraGridRow oRow)
        {
            bool result = true;
            if (oRow != null)
            {
                string selectedOrderID = oRow.Cells[clsPOSDBConstants.PODetail_Fld_OrderID].Value.ToString();
                selectedPODetailIDFromVendorCombo = oRow.Cells[clsPOSDBConstants.PODetail_Fld_PODetailID].Value.ToString();
                result = !IsPODetailExist(selectedPODetailIDFromVendorCombo);
            }
            return result;
        }


        private void gridItemDetails_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            try
            {
                string packetUnit = this.gridItemDetails.ActiveRow.Cells[clsPOSDBConstants.PODetail_Fld_PackUnit].Value.ToString().ToUpper().Trim();
                if ((packetUnit == clsPOSDBConstants.PckUnit_CS) || (packetUnit == clsPOSDBConstants.PckUnit_CA))   //Sprint-21 22-Feb-2016 JY Added CA for case item
                {
                    string sItemID = this.gridItemDetails.ActiveRow.Cells[clsPOSDBConstants.PODetail_Fld_ItemID].Value.ToString().Trim();
                    int SoldItem = Convert.ToInt32(this.gridItemDetails.ActiveRow.Cells["items Sold"].Value);
                    frmCSItems frmCSItem = new frmCSItems(sItemID, SoldItem);
                    frmCSItem.Qty = MMSUtil.UtilFunc.ValorZeroI(this.gridItemDetails.ActiveRow.Cells[clsPOSDBConstants.PODetail_Fld_QTY].Value.ToString().Trim());
                    frmCSItem.ShowDialog();
                    frmCSItem.BringToFront();
                    if (!frmCSItem.IsCancel)
                    {
                        gridItemDetails.ActiveRow.Cells[clsPOSDBConstants.PODetail_Fld_QTY].Value = frmCSItem.Qty > 0 ? frmCSItem.Qty : 1;
                    }
                }
                this.gridItemDetails.UpdateData();
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "gridItemDetails_MouseDoubleClick(object sender, MouseEventArgs e)");
                clsUIHelper.ShowErrorMsg(ex.Message);
            }
        }

        //private void gridItemDetails_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        //{

        //}

        //Folowing Code Added By Krishna on 3 June 2012
        private void FillAddedItemsinGrid(PODetailData dsAddeItems)
        {
            int podId = 0;
            bool isFirstTime = true;
            string vendorCode = string.Empty;
            string ItemDescription = string.Empty;
            try
            {
                DisableControls();
                gridItemDetails.Rows.Band.AddNew();
                foreach (PODetailRow pdr in dsAddeItems.PODetail.Rows)
                {
                    if (gridItemDetails.ActiveRow == null || !isFirstTime)
                    {
                        if (gridItemDetails.Rows != null)
                            gridItemDetails.Rows.Band.AddNew();
                        else
                            break;
                    }
                    podId = GetMaxPODID();

                    InitalizePODetails(pdr.VendorID.ToString());

                    gridItemDetails.ActiveRow.Cells[clsPOSDBConstants.PODetail_Fld_PODetailID].Value = podId;
                    gridItemDetails.ActiveRow.Cells["ItemId"].Value = pdr.ItemID;
                    gridItemDetails.ActiveRow.Cells["Description"].Value = pdr.ItemDescription;
                    gridItemDetails.ActiveRow.Cells["Price"].Value = pdr.Price;
                    gridItemDetails.ActiveRow.Cells["Qty"].Value = pdr.QTY > 0 ? pdr.QTY : 1;
                    gridItemDetails.ActiveRow.Cells["comments"].Value = pdr.Comments;
                    vendorDetails.TryGetValue(pdr.VendorID.ToString(), out vendorCode);
                    gridItemDetails.ActiveRow.Cells[clsPOSDBConstants.PODetail_Fld_VendorName].Value = vendorCode;
                    //gridItemDetails.ActiveRow.Cells["vend. Itm Code"].Value = pdr.VendorItemCode;
                    gridItemDetails.ActiveRow.Cells[clsPOSDBConstants.PODetail_Fld_VendorItemCode].Value = pdr.VendorItemCode;
                    gridItemDetails.ActiveRow.Cells["Best Price"].Value = pdr.BestPrice;
                    gridItemDetails.ActiveRow.Cells["Best Vendor"].Value = pdr.BestVendor;
                    gridItemDetails.ActiveRow.Cells["Last Vendor"].Value = string.Empty;
                    gridItemDetails.ActiveRow.Cells["VendorID"].Value = pdr.VendorID;
                    gridItemDetails.ActiveRow.Cells["Items Sold"].Value = pdr.SoldItems;
                    gridItemDetails.ActiveRow.Cells[clsPOSDBConstants.PODetail_Fld_PackSize].Value = pdr.PacketSize;
                    gridItemDetails.ActiveRow.Cells[clsPOSDBConstants.PODetail_Fld_PackQuant].Value = pdr.PacketQuant;
                    gridItemDetails.ActiveRow.Cells[clsPOSDBConstants.PODetail_Fld_PackUnit].Value = pdr.Packetunit;
                    gridItemDetails.ActiveRow.Cells[clsPOSDBConstants.Item_Fld_QtyInStock].Value = pdr.QtyInStock;
                    gridItemDetails.ActiveRow.Cells[clsPOSDBConstants.Item_Fld_ReOrderLevel].Value = pdr.ReOrderLevel;
                    gridItemDetails.ActiveRow.Cells[clsPOSDBConstants.Item_Fld_MinOrdQty].Value = pdr.MinOrdQty;
                    gridItemDetails.ActiveRow.Cells[clsPOSDBConstants.Item_Fld_QtyOnOrder].Value = pdr.QtyOnOrder;

                    if ((gridItemDetails.ActiveRow.Cells[clsPOSDBConstants.PODetail_Fld_PackUnit].Value.ToString().ToUpper().Trim() == clsPOSDBConstants.PckUnit_CS) || (gridItemDetails.ActiveRow.Cells[clsPOSDBConstants.PODetail_Fld_PackUnit].Value.ToString().ToUpper().Trim() == clsPOSDBConstants.PckUnit_CA))   //Sprint-21 22-Feb-2016 JY Added CA for case item
                    {
                        if (MMSUtil.UtilFunc.ValorZeroDEC(pdr.PacketSize) > 0)
                        {
                            //gridItemDetails.ActiveRow.Cells["cost"].Value = pdr.Price * MMSUtil.UtilFunc.ValorZeroDEC(gridItemDetails.ActiveRow.Cells["Qty"].Value.ToString()) * MMSUtil.UtilFunc.ValorZeroDEC(pdr.PacketSize);   //Sprint-21 - 1803 28-Sep-2015 JY Commented
                            gridItemDetails.ActiveRow.Appearance.BackColor = Color.Yellow;
                        }
                        else
                        {
                            //gridItemDetails.ActiveRow.Cells["cost"].Value = pdr.Price * MMSUtil.UtilFunc.ValorZeroDEC(gridItemDetails.ActiveRow.Cells["Qty"].Value.ToString());   //Sprint-21 - 1803 28-Sep-2015 JY Commented
                            gridItemDetails.ActiveRow.Appearance.BackColor = Color.Red;
                        }
                    }
                    else
                    {
                        //gridItemDetails.ActiveRow.Cells["cost"].Value = pdr.Price * MMSUtil.UtilFunc.ValorZeroDEC(gridItemDetails.ActiveRow.Cells["Qty"].Value.ToString());   //Sprint-21 - 1803 28-Sep-2015 JY Commented
                    }
                    gridItemDetails.ActiveRow.Cells["cost"].Value = pdr.Price * MMSUtil.UtilFunc.ValorZeroDEC(gridItemDetails.ActiveRow.Cells["Qty"].Value.ToString()); //Sprint-21 - 1803 28-Sep-2015 JY Added

                    haveVendorInfo = true;
                    FindBestVendorANDPrice(pdr.ItemID);
                    gridItemDetails.Refresh();
                    gridItemDetails.ActiveRow.Update();
                    isFirstTime = false;
                    orderDetailData.AcceptChanges();
                    string compVendor = string.Empty;
                    DataRow[] vendorRow = VendorData.Vendor.Select(" VendorID=" + pdr.VendorID);
                    if (vendorRow != null && vendorRow.Length > 0)
                        compVendor = vendorRow[0].ItemArray[2].ToString();
                    String bestVendor = gridItemDetails.ActiveRow.Cells["Best Vendor"].Value.ToString();
                    FormatGrid(bestVendor, compVendor);

                    gridItemDetails.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.PODetail_Fld_PackSize].Hidden = true;
                    gridItemDetails.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.PODetail_Fld_PackQuant].Hidden = true;
                    gridItemDetails.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.PODetail_Fld_PackUnit].Hidden = true;
                    gridItemDetails.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.PODetail_Fld_DeptName].Hidden = true;
                    gridItemDetails.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.PODetail_Fld_SubDeptName].Hidden = true;
                    gridItemDetails.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.PODetail_Fld_RetailPrice].Hidden = true;
                    gridItemDetails.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.PODetail_Fld_Discount].Hidden = true;
                    gridItemDetails.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.PODetail_Fld_ItemPrice].Hidden = true;

                    Application.DoEvents();
                }
                EnableControls();
            }
            catch (Exception Ex)
            {
                logger.Fatal(Ex, "FillAddedItemsinGrid(PODetailData dsAddeItems)");
                MessageBox.Show(Ex.Message);
            }
        }
        //Added by Shitaljit on 110 Sept 2012
        private void btnDeleteCaseItems_Click(object sender, EventArgs e)
        {
            try
            {
                string strDeleteMsg = "Are you sure you want to delete the all case(CS) item(s)?\n";
                string itemID;
                int caseItemCount = 0;
                if (gridItemDetails.Rows.Count > 0)
                {

                    Infragistics.Win.UltraWinGrid.RowsCollection GridRows = gridItemDetails.Rows;

                    for (int j = 0; j < GridRows.Count; j++)
                    {
                        if ((Convert.ToBoolean(GridRows[j].Cells[clsPOSDBConstants.PODetail_Fld_PackUnit].Value.ToString().ToUpper().Trim() == clsPOSDBConstants.PckUnit_CS)) || (Convert.ToBoolean(GridRows[j].Cells[clsPOSDBConstants.PODetail_Fld_PackUnit].Value.ToString().ToUpper().Trim() == clsPOSDBConstants.PckUnit_CA))) //Sprint-21 22-Feb-2016 JY Added CA for case item
                        {
                            caseItemCount++;
                            strDeleteMsg += "\n " + GridRows[j].Cells[clsPOSDBConstants.PODetail_Fld_ItemID].Value.ToString().Trim() + " - " + GridRows[j].Cells[clsPOSDBConstants.Item_Fld_Description].Value.ToString().Trim();
                        }
                    }
                    if (caseItemCount == 0)
                    {
                        clsUIHelper.ShowErrorMsg("There is no Case Item(s) in the current Purchase Order.");
                        return;
                    }
                    if (POS_Core_UI.Resources.Message.DisplayDefaultNo(strDeleteMsg, "Purchase Order", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        for (int i = GridRows.Count - 1; i >= 0; i--)
                        {
                            if ((Convert.ToBoolean(GridRows[i].Cells[clsPOSDBConstants.PODetail_Fld_PackUnit].Value.ToString().ToUpper().Trim() == clsPOSDBConstants.PckUnit_CS)) || (Convert.ToBoolean(GridRows[i].Cells[clsPOSDBConstants.PODetail_Fld_PackUnit].Value.ToString().ToUpper().Trim() == clsPOSDBConstants.PckUnit_CA))) //Sprint-21 22-Feb-2016 JY Added CA for case item
                            {
                                gridItemDetails.ActiveRow = GridRows[i];
                                itemID = this.gridItemDetails.ActiveRow.Cells[clsPOSDBConstants.PODetail_Fld_ItemID].Value.ToString();
                                if (itemID != "")
                                {
                                    DeleteGridRow(false, this.gridItemDetails.ActiveRow);
                                }
                            }
                        }
                        #region Sprint-21 - 1803 14-Sep-2015 JY Added
                        try
                        {
                            if (gridItemDetails.Rows.Count == 0)
                            {
                                this.txtTotalCostForAllPO.Text = "0";
                            }
                            else
                            {
                                decimal dTotalCostForAllPO = 0.0M;
                                for (int i = 0; i < gridItemDetails.Rows.Count; i++)
                                {
                                    dTotalCostForAllPO += Configuration.convertNullToDecimal(gridItemDetails.Rows[i].Cells[clsPOSDBConstants.PODetail_Fld_Cost].Value);
                                }
                                this.txtTotalCostForAllPO.Text = dTotalCostForAllPO.ToString("G29");
                            }
                        }
                        catch { }
                        #endregion
                    }

                    if (TotalItemsCount == 0)
                        this.pnlAutoGenPO.Enabled = true;
                }
                else
                {
                    clsUIHelper.ShowErrorMsg("There are No Items In PO");
                    FocusNewItemId();
                }

                txtEditorNoOfItems.Text = TotalItemsCount.ToString();
                txtEditorNoOfVendors.Text = TotalVendorsCount.ToString();
                int csItemCount = CSItemCount;
                txtEditorNoOfCSItems.Text = csItemCount.ToString();
                // Added by Shrikant Mali on 02/10/2014 to update the count of non case Items.
                tbxNonCaseItems.Text = GetNonCaseItemCount(csItemCount).ToString();
                //this.txtTotalCostForAllPO.Text = totalCostForAllPO.ToString();//Added By shitaljit for all PO(s) total cost   //Sprint-21 - 1803 15-Sep-2015 JY Commented
            }
            catch (Exception Ex)
            {
                logger.Fatal(Ex, "btnDeleteCaseItems_Click(object sender, EventArgs e)");
                clsUIHelper.ShowErrorMsg(Ex.Message);
            }
        }

        private void btnShortByCSItem_Click(object sender, EventArgs e)
        {
            this.gridItemDetails.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.Item_Fld_Description].SortIndicator = SortIndicator.Disabled;
            this.gridItemDetails.DisplayLayout.Bands[0].Columns["Vendor"].SortIndicator = SortIndicator.Disabled;
            this.gridItemDetails.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.PODetail_Fld_PackUnit].SortIndicator = SortIndicator.Descending;
            this.gridItemDetails.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.PODetail_Fld_PackUnit].Band.SortedColumns.RefreshSort(true);
            this.gridItemDetails.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.Item_Fld_Description].SortIndicator = SortIndicator.Ascending;
            this.gridItemDetails.DisplayLayout.Bands[0].Columns["Vendor"].SortIndicator = SortIndicator.Ascending;
        }
        //End of Added by shitaljit.

        private void btnUseBestVendor_Click(object sender, EventArgs e)
        {
            useBestVendor = false;
            string message = "Do you want use best Vendor for all items?";
            try
            {
                if (gridItemDetails.Rows.Count > 0)
                {
                    clsUIHelper.ShowInfoMsg(message);
                    if (clsUIHelper.IsOK)
                    {
                        VendorData vendorData2 = new VendorData();
                        VendorSvr vendorSvr2 = new VendorSvr();
                        foreach (UltraGridRow itemDetailRow in gridItemDetails.Rows)
                        {
                            string originalVendor = string.Empty;
                            string selectedOrderID = string.Empty;
                            String origVendorID = String.Empty;
                            string vendorCodeSelected = string.Empty;

                            try
                            {
                                string vendorName = string.Empty;
                                //if (uRow.Cells[clsPOSDBConstants.PODetail_Fld_BestVendor].Value.ToString() != "")
                                // Commented by Shrikant Mali on 10-02-2014 to correct the condition.
                                if (itemDetailRow.Cells[clsPOSDBConstants.PODetail_Fld_BestVendor].Value.ToString() != "" &&
                                    itemDetailRow.Cells[clsPOSDBConstants.PODetail_Fld_VendorName].Value.ToString().Equals(itemDetailRow.Cells[clsPOSDBConstants.PODetail_Fld_BestVendor].Value.ToString()) == false)
                                {
                                    //this.gridItemDetails.ActiveRow = uRow;
                                    vendorData2 = vendorSvr2.PopulateList(" Where VendorName ='" + itemDetailRow.Cells[clsPOSDBConstants.PODetail_Fld_BestVendor].Value.ToString() + "'");
                                    vendorCodeSelected = vendorData2.Tables[0].Rows[0][clsPOSDBConstants.Vendor_Fld_VendorCode].ToString();

                                    UltraGridRow dropdowmRowSelected = null;
                                    foreach (UltraGridRow dropdowmRow in vendorDropDown.Rows)
                                    {

                                        if (dropdowmRow.Cells[0].Value.ToString() == vendorCodeSelected)
                                        {
                                            dropdowmRowSelected = dropdowmRow;
                                            break;
                                        }
                                    }

                                    #region Commented by Shrikant Mali, on 04-Feb-2014 to solve ticket no.1807 (Jeera). and following function is added.
                                    //uRow.Cells[clsPOSDBConstants.PODetail_Fld_VendorName].Activate();
                                    //uRow.Cells[clsPOSDBConstants.PODetail_Fld_VendorName].Value = vendorCodeSelected;
                                    ////Added By shitaljit on 1/16/2014 for JIRA-1720
                                    //if (uRow.Cells[clsPOSDBConstants.PODetail_Fld_VendorName].ValueList != null)
                                    //{
                                    //	uRow.Cells[clsPOSDBConstants.PODetail_Fld_VendorName].ValueList.SelectedItemIndex = vendorDropDown.Rows.IndexOf(dropdowmRowSelected);
                                    //}
                                    ////uRow.Cells[clsPOSDBConstants.PODetail_Fld_VendorName].Value = vendorCodeSelected;
                                    //uRow.Cells[clsPOSDBConstants.PODetail_Fld_VendorName].Activate();
                                    //uRow.Update();

                                    // VendorData vendorDataTemp = new VendorData();
                                    // VendorSvr vendorSvrTemp = new VendorSvr();
                                    // vendorDataTemp = vendorSvrTemp.PopulateList(" Where VendorName ='" + uRow.Cells["Best Vendor"].Text + "'");
                                    // string VSelectedCode = vendorDataTemp.Tables[0].Rows[0][clsPOSDBConstants.Vendor_Fld_VendorCode].ToString();

                                    // if (VSelectedCode != uRow.Cells["VendorName"].Text)
                                    // {
                                    //	 uRow.Cells[clsPOSDBConstants.PODetail_Fld_BestVendor].Appearance.BackColorDisabled = Color.Red;//Change by Ravindra previously iw was changing Forcolor
                                    //	 uRow.Cells[clsPOSDBConstants.PODetail_Fld_BestVendPrice].Appearance.BackColorDisabled = Color.Red;
                                    // }
                                    // else
                                    // {
                                    //	 uRow.Cells[clsPOSDBConstants.PODetail_Fld_BestVendor].Appearance.BackColorDisabled = Color.White;
                                    //	 uRow.Cells[clsPOSDBConstants.PODetail_Fld_BestVendPrice].Appearance.BackColorDisabled = Color.White;
                                    #endregion

                                    // }
                                    this.gridItemDetails.ActiveRow = itemDetailRow;

                                    UpdateBestVendorInPODetails(dropdowmRowSelected, itemDetailRow);
                                    //uRow.Appearance.ForeColor = Color.Yellow;
                                }////If row is updated then continue following logic.  

                            }
                            catch
                            {
                                throw;
                            }
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                logger.Fatal(Ex, "btnUseBestVendor_Click(object sender, EventArgs e)");
                clsUIHelper.ShowErrorMsg(Ex.Message);
            }
            finally
            {
                FocusItemId();
                this.gridItemDetails.Refresh();
                useBestVendor = false;
            }
        }

        /// <summary>
        /// Checks whether the best vendor for the particular Items is to be used or not, if it is to be used
        /// then it uses the best vendor and updated the purchase order details to as to reflect the newly used 
        /// best vendor.
        /// </summary>
        /// <param name="selectedVendorRow">The Row selected in DropDown to select the vendor.</param>
        /// <param name="itemDetailRow">the Item detail row againt which we are checking where to use the best vendor for or not.</param>
        private void UpdateBestVendorInPODetails(UltraGridRow selectedVendorRow, UltraGridRow itemDetailRow)
        {
            var selectedOrderId = string.Empty;
            var origVendorId = String.Empty;
            var isVendorExist = false;
            try
            {
                var currentVendorName = GetCurrentVendorName(itemDetailRow);
                vendorSelected = GetSelectedBestVendorName(selectedVendorRow);

                if (!IsToUseBestVendor(vendorSelected, itemDetailRow))
                    return;

                var orgVendRows = vendorData.Vendor.Select("VendorCode ='" + currentVendorName + "'");
                if (orgVendRows.Length > 0)
                {
                    origVendorId = orgVendRows[0][clsPOSDBConstants.POHeader_Fld_VendorID].ToString();
                    selectedOrderId =
                        itemDetailRow.Cells[clsPOSDBConstants.PODetail_Fld_OrderID].Value.ToString();
                    selectedPODetailIDFromVendorCombo =
                        itemDetailRow.Cells[clsPOSDBConstants.PODetail_Fld_PODetailID].Value.ToString();

                    ////Check whether the row is updated? ALONG WITH QTY IS NOT NULL OR ZERO
                    ////IF QTY IS NULL.... IRRESPECTIVE OF ANY VENDOR CHANGE EXIT AND FOCUS QTY....
                    ////If ROW not UPDATED AND QTY IS NOT NULL then
                    if (selectedPODetailIDFromVendorCombo.Trim().Length > 0)
                    {
                        if (!IsPODetailExist(selectedPODetailIDFromVendorCombo))
                        {
                            var isNext = gridItemDetails.PerformAction(UltraGridAction.NextRow);

                            //Go back to current location of old row.
                            if (!isNext)
                            {
                                gridItemDetails.PerformAction(UltraGridAction.PrevRow);
                                itemDetailRow.Cells[clsPOSDBConstants.PODetail_Fld_VendorName].Value = currentVendorName;
                                itemDetailRow.CancelUpdate();
                                //gridItemDetails.ActiveRow.Cells[clsPOSDBConstants.PODetail_Fld_QTY].Activate();
                                FocusQty();
                                return;
                            }

                            gridItemDetails.PerformAction(UltraGridAction.PrevRow);
                            FocusToCell(clsPOSDBConstants.PODetail_Fld_VendorName);
                            ////gridItemDetails.ActiveRow.Cells[clsPOSDBConstants.PODetail_Fld_VendorName].Activate();
                        }
                        else
                        {
                            itemDetailRow.Cells[clsPOSDBConstants.PODetail_Fld_VendorName].Value = vendorSelected;
                            itemDetailRow.Cells[clsPOSDBConstants.PODetail_Fld_BestVendor].Appearance.BackColorDisabled = Color.White;
                            itemDetailRow.Cells[clsPOSDBConstants.PODetail_Fld_BestVendPrice].Appearance.BackColorDisabled = Color.White;
                        }

                        ////If row is updated then continue following logic.                            
                        UpdateVendorSelectedInDropDown(vendorSelected, selectedOrderId, out isVendorExist);
                        itemDetailRow.Update();
                        gridItemDetails.Refresh();
                    }
                }

                if (isVendorExist)
                {
                    if (!isEditPO)
                        CalculateTotals(origVendorId);
                    else
                    {
                        vendorBeforeSelection = origVendorId;
                        selectedOrderIDFromVendorCombo = selectedOrderId;
                    }
                }
                else
                {
                    itemDetailRow.Cells[clsPOSDBConstants.PODetail_Fld_VendorName].Value = currentVendorName;
                    itemDetailRow.CancelUpdate();
                    FocusToCell(clsPOSDBConstants.PODetail_Fld_VendorName);
                }
                gridItemDetails.Refresh();
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "UpdateBestVendorInPODetails(UltraGridRow selectedVendorRow, UltraGridRow itemDetailRow)");
                clsUIHelper.ShowErrorMsg(ex.Message);
            }
        }

        private static bool IsToUseBestVendor(string bestVendorName, UltraGridRow itemDetailRow)
        {
            var currentVendorName = GetCurrentVendorName(itemDetailRow);

            if (currentVendorName.Trim().Length == 0 || bestVendorName.Trim().Length == 0)
                return false;

            return bestVendorName != currentVendorName;
        }

        private static string GetCurrentVendorName(UltraGridRow itemDetailRow)
        {
            return itemDetailRow.Cells[clsPOSDBConstants.PODetail_Fld_VendorName].Value.ToString();
        }

        private static string GetSelectedBestVendorName(UltraGridRow selectedVendorRow)
        {
            return selectedVendorRow.Cells["Value"].Value.ToString();
        }

        private void gridItemDetails_BeforeCellActivate(object sender, CancelableCellEventArgs e)
        {
            try
            {
                if (gridItemDetails.ActiveCell == null) return; //Sprint-21 - 1803 15-Sep-2015 JY Added

                if (gridItemDetails.ActiveCell.Column.Key == "Select")
                {
                    gridItemDetails.PerformAction(UltraGridAction.NextCell);
                }
            }
            catch { }
        }

        private void cboItemFilterOptions_ValueChanged(object sender, EventArgs e)
        {
            if (cboItemFilterOptions.SelectedItem != null)
                ApplyItemSelectionFilter(GetSelectedItemDisplayFilter());
        }

        /// <summary>
        /// Auther : Shrikant Mali
        /// Date : 02/10/2014.
        /// Get the filter selected by user for Item selection.
        /// </summary>
        /// <returns></returns>
        private POItemDisplayFilter GetSelectedItemDisplayFilter()
        {
            return (POItemDisplayFilter)cboItemFilterOptions.SelectedItem.Tag;
        }

        /// <summary>
        /// Auther : Shrikant Mali
        /// Date : 02/10/2014
        /// Applyies the Item selection filter to the items in Item details grid view.
        /// </summary>
        /// <param name="itemDisplayFilter">The type of filter to be applied.</param>
        private void ApplyItemSelectionFilter(POItemDisplayFilter itemDisplayFilter)
        {
            if (itemDisplayFilter == POItemDisplayFilter.All)
            {
                ShowAllItems();
            }
            else if (itemDisplayFilter == POItemDisplayFilter.OnlyCaseItems)
            {
                ShowOnlyCaseItems();
            }
            else if (itemDisplayFilter == POItemDisplayFilter.OnlyNonCaseItems)
            {
                ShowOnlyNonCaseItems();
            }
        }

        /// <summary>
        /// Auther : Shrikant Mali
        /// Date : 02/10/2014
        /// Makes all the Items visible in Item details grid view.
        /// </summary>
        private void ShowAllItems()
        {
            foreach (var item in gridItemDetails.Rows)
            {
                item.Hidden = false;
            }
        }

        /// <summary>
        /// Auther : Shrikant Mali
        /// Date : 02/10/2014
        /// Makes only non case items visible in Item details grid.
        /// </summary>
        private void ShowOnlyNonCaseItems()
        {
            foreach (var item in gridItemDetails.Rows)
            {
                item.Hidden = IsCaseItem(item);
            }
        }

        /// <summary>
        /// Auther : Shrikant Mali
        /// Date : 02/10/2014
        /// Makes only case items visible in Item details grid.
        /// </summary>
        private void ShowOnlyCaseItems()
        {
            foreach (var item in gridItemDetails.Rows)
            {
                item.Hidden = !IsCaseItem(item);
            }
        }

        /// <summary>
        /// Auther : Shrikant Mali
        /// Date : 07/02/2014
        /// Represents the filter used to show the items in Item details grid view.
        /// </summary>
        private enum POItemDisplayFilter
        {
            All,
            OnlyCaseItems,
            OnlyNonCaseItems
        }

        #region Sprint-21 - 1803 14-Sep-2015 JY Added
        private void gridItemDetails_AfterCellUpdate(object sender, CellEventArgs e)
        {
            try
            {
                if (e.Cell.Column.Key.ToString().Trim().ToUpper() == clsPOSDBConstants.PODetail_Fld_Cost.Trim().ToUpper())
                {
                    decimal dTotalCostForAllPO = 0.0M;
                    for (int i = 0; i < gridItemDetails.Rows.Count; i++)
                    {
                        dTotalCostForAllPO += Configuration.convertNullToDecimal(gridItemDetails.Rows[i].Cells[clsPOSDBConstants.PODetail_Fld_Cost].Value);
                    }
                    this.txtTotalCostForAllPO.Text = dTotalCostForAllPO.ToString("G29");
                }
            }
            catch { }
        }
        #endregion

        #region PRIMEPOS-3155 06-Oct-2022 JY Added
        private void TotalCostForAllPO()
        {
            decimal dTotalCostForAllPO = 0.0M;
            for (int i = 0; i < gridItemDetails.Rows.Count; i++)
            {
                dTotalCostForAllPO += Configuration.convertNullToDecimal(gridItemDetails.Rows[i].Cells[clsPOSDBConstants.PODetail_Fld_Cost].Value);
            }
            this.txtTotalCostForAllPO.Text = dTotalCostForAllPO.ToString("G29");
        }
        #endregion        

    }//Class    
}//Namespace