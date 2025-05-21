using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
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
using POS_Core_UI.Reports.Reports;
using POS_Core_UI.Reports.ReportsUI;
using POS_Core.Resources;

namespace POS_Core_UI
{
    public partial class frmNonVendorItems : Form
    {
        ItemData dsItem = new ItemData();
        PODetailData dsNonVendItems;
        PODetailData  VendItems;
        public PODetailData dsAddedItems = new PODetailData();
        PODetailRow drAddedItems;
        
        public frmNonVendorItems(PODetailData ds)
        {
            InitializeComponent();
            dsNonVendItems = ds;
            VendItems = ds;
        }

        private void frmNonVendorItems_Load(object sender, EventArgs e)
        {
            try
            {                
                grdItemDetails.DataSource = dsNonVendItems;
                SetGridDisplay();
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message);
            }
        }

        private void SetGridDisplay()
        {
            try
            {
                grdItemDetails.DisplayLayout.Bands[0].Columns["PODetailId"].Hidden = true;
                grdItemDetails.DisplayLayout.Bands[0].Columns["Cost"].Hidden = true;
                grdItemDetails.DisplayLayout.Bands[0].Columns["OrderId"].Hidden = true;
                grdItemDetails.DisplayLayout.Bands[0].Columns["Vendor"].Hidden = false;
                grdItemDetails.DisplayLayout.Bands[0].Columns["AckQty"].Hidden = true;
                grdItemDetails.DisplayLayout.Bands[0].Columns["AckStatus"].Hidden = true;
                grdItemDetails.DisplayLayout.Bands[0].Columns["Comments"].Hidden = true;
                grdItemDetails.DisplayLayout.Bands[0].Columns["VendorName"].Hidden = true;
                //grdItemDetails.DisplayLayout.Bands[0].Columns["VendorItemCode"].Hidden = true;    //Sprint-25 - PRIMEPOS-XXXX 30-Mar-2017 JY Unhide the column
                grdItemDetails.DisplayLayout.Bands[0].Columns["Best Vend.Price"].Hidden = true;
                grdItemDetails.DisplayLayout.Bands[0].Columns["Best Vendor"].Hidden = true;
                grdItemDetails.DisplayLayout.Bands[0].Columns["Last Ord.Vendor"].Hidden = true;
                grdItemDetails.DisplayLayout.Bands[0].Columns["Last Ord.Qty"].Hidden = true;
                grdItemDetails.DisplayLayout.Bands[0].Columns["ChangedProductQualifier"].Hidden = true;
                grdItemDetails.DisplayLayout.Bands[0].Columns["ChangedProductId"].Hidden = true;
                grdItemDetails.DisplayLayout.Bands[0].Columns["QtySold100Days"].Hidden = true;
                grdItemDetails.DisplayLayout.Bands[0].Columns["idescription"].Hidden = true;
                grdItemDetails.DisplayLayout.Bands[0].Columns["ItemDescType"].Hidden = true;
                grdItemDetails.DisplayLayout.Bands[0].Columns["QtyOnOrder"].Hidden = true;
                grdItemDetails.DisplayLayout.Bands[0].Columns["DeptName"].Hidden = true;
                grdItemDetails.DisplayLayout.Bands[0].Columns["SubDeptName"].Hidden = true;
                grdItemDetails.DisplayLayout.Bands[0].Columns["SoldItems"].Hidden = true;
                grdItemDetails.DisplayLayout.Bands[0].Columns["Packet Quantity"].Hidden = true;
                grdItemDetails.DisplayLayout.Bands[0].Columns["VendorId"].Hidden = true;
                grdItemDetails.DisplayLayout.Bands[0].Columns["Packet Size"].Hidden = true;
                grdItemDetails.DisplayLayout.Bands[0].Columns["Packet Size"].Hidden = true;
                grdItemDetails.DisplayLayout.Bands[0].Columns["Packet Unit"].Hidden = true;
                grdItemDetails.DisplayLayout.Bands[0].Columns["InvRecDate"].Hidden = true;
                grdItemDetails.DisplayLayout.Bands[0].Columns["SoldItems"].Hidden = true;
                grdItemDetails.DisplayLayout.Bands[0].Columns["RetailPrice"].Hidden = true;
                grdItemDetails.DisplayLayout.Bands[0].Columns["Items Sold"].Hidden = true;  //Sprint-25 - PRIMEPOS-XXXX 30-Mar-2017 JY hide

                this.grdItemDetails.DisplayLayout.Bands[0].Columns["VendorType"].Header.SetVisiblePosition(5,false);
                this.grdItemDetails.DisplayLayout.Bands[0].Columns["Vendor"].Header.SetVisiblePosition(4, false);
                this.grdItemDetails.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.Item_Fld_Description].SortIndicator = SortIndicator.Disabled;
                this.grdItemDetails.DisplayLayout.Bands[0].Columns["VendorType"].SortIndicator = SortIndicator.Descending;
                this.grdItemDetails.DisplayLayout.Bands[0].Columns["VendorType"].Band.SortedColumns.RefreshSort(true);
                this.grdItemDetails.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.Item_Fld_Description].SortIndicator = SortIndicator.Ascending;
                this.grdItemDetails.DisplayLayout.AutoFitStyle = AutoFitStyle.ResizeAllColumns;                                
            }
            catch (Exception Ex)
            {              
                throw;
            }
        }
        //Done button event
        private void btnOk_Click(object sender, EventArgs e)
        {
            try
            {
                ItemVendorData dsItemVendor = new ItemVendorData();
                ItemVendorSvr ItemVendorSvr = new ItemVendorSvr();
                List<string> lst = new List<string>();
                List<string> lstVendor = new List<string>();

                
                if (grdItemDetails.Rows.Count > 0)
                {
                    DialogResult result = POS_Core_UI.Resources.Message.Display("You have not assigned VendorItemCode to the item(s) in list.\n Your action will discard  EDI Vendor item(s) from the list, Are you sure? ", "Non Vendor Items", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (result == DialogResult.No)
                    {
                        return;
                    }
                }
                foreach (PODetailRow row in VendItems.PODetail.Rows)
                {
                    lst.Add(row["ItemId"].ToString());
                }
                string str = string.Join("','", lst.ToArray());
                Item oItem = new Item();
                ItemData oItemData = new ItemData();

                oItemData = oItem.PopulateList(" Where  ItemId in ('" + string.Join("','", lst.ToArray()) + "')" + " AND ( LastVendor <> '' OR  PreferredVendor <> ''" + ")");

                POS_Core.BusinessRules.Vendor oVendor = new POS_Core.BusinessRules.Vendor();
                VendorData oVenData = new VendorData();
                foreach (PODetailRow pdr in VendItems.PODetail.Rows)
                {
                    if (pdr.VendorItemCode == "" && pdr.VendorID == 0)
                    {
                        string itemcode = pdr["ItemId"].ToString();
                        DataRow[] rows = oItemData.Item.Select("ItemId = '" + pdr["ItemId"].ToString().Trim() + "'");
                        if (rows != null && rows.Length > 0)
                        {
                            ItemRow row = (ItemRow)rows[0];
                            string VendorCode = "";
                            if (row.PreferredVendor != "")
                            {
                                VendorCode = row.PreferredVendor; 
                            }
                            else if (row.LastVendor != "")
                            {
                                VendorCode = row.LastVendor;
                            }
                            if (VendorCode != "")
                            {
                                oVenData = oVendor.Populate(VendorCode.Trim());
                                if (oVenData != null)
                                {
                                    if (oVenData.Vendor.Rows.Count > 0)
                                    {
                                        VendorRow oRow = oVenData.Vendor[0];
                                        if (oRow.USEVICForEPO == false)
                                        {
                                            pdr.VendorName = oRow.Vendorname;
                                            pdr.VendorID = oRow.VendorId;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                PODetailData odata = new PODetailData();
                int Index = 0;
                foreach (PODetailRow pdr in VendItems.PODetail.Rows)
                {
                    if (pdr.VendorID != 0)
                    {
                        odata.PODetail.ImportRow(pdr);
                        odata.PODetail.Rows[Index]["VendorID"] = pdr.VendorID;
                        Index++;
                    }
                }
                lst.Clear();
                foreach (PODetailRow row in dsAddedItems.PODetail.Rows)
                {
                    lst.Add(row["ItemId"].ToString());
                }

                if (lst.Count > 0)
                {
                    dsItemVendor = ItemVendorSvr.PopulateList(" Where  ItemId in ('" + string.Join("','", lst.ToArray()) + "')");
                    //Add vendor information in podetails rows.
                    FillVendorDetails(dsItemVendor);
                }
                foreach (PODetailRow pdr in dsAddedItems.PODetail.Rows)
                {
                    odata.PODetail.ImportRow(pdr);
                    odata.PODetail.Rows[Index]["VendorID"] = pdr.VendorID;
                    Index++;
                }
               
                dsAddedItems = odata;
                this.DialogResult = DialogResult.OK;
                this.Close();
            }

            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message);
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                string ItemId = grdItemDetails.ActiveRow.Cells["ItemId"].Value.ToString();
                string Description = grdItemDetails.ActiveRow.Cells["Description"].Value.ToString();

                frmAddNewItem addItems = new frmAddNewItem();
                addItems.FillItemDescription(ItemId, Description, "");
                addItems.ShowDialog();

                if (!addItems.IsCanceled)
                {
                    //Refresh Grid;
                    DataRow[] rows = dsNonVendItems.PODetail.Select("ItemId = '" + ItemId + "'");
                    if (rows.Length > 0)
                    {
                        drAddedItems = dsAddedItems.PODetail.NewPODetailRow();
                        drAddedItems.ItemArray = ((PODetailRow)rows[0]).ItemArray;
                        dsAddedItems.PODetail.Rows.Add(drAddedItems);
                        dsNonVendItems.PODetail.Rows.Remove(rows[0]);
                        grdItemDetails.Update();
                    }
                }
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message);
            }
        }

        private void FillVendorDetails(ItemVendorData dsItemVendor)
        {
            int podId = 0;
            bool isFirstTime = true;
            string vendorCode = string.Empty;
            string ItemDescription = string.Empty;
            foreach (PODetailRow pdr in dsAddedItems.PODetail.Rows)
            {
                DataRow[] rows = dsItemVendor.ItemVendor.Select("ItemId = '" + pdr["ItemId"].ToString().Trim() + "'");
                ItemVendorRow drItemVendor = (ItemVendorRow)rows[0];
                pdr[clsPOSDBConstants.PODetail_Fld_VendorName] = drItemVendor.VendorName;
                pdr[clsPOSDBConstants.PODetail_Fld_VendorItemCode] = drItemVendor.VendorItemID;
                pdr["VendorID"] = drItemVendor.VendorID;               
            }
        }

        private void grdItemDetails_InitializeRow(object sender, InitializeRowEventArgs e)
        {
            POS_Core.BusinessRules.Vendor oVendor = new POS_Core.BusinessRules.Vendor();
            VendorData oVenData = new VendorData();
            try
            {
                string VendorCode = e.Row.Cells["VendorName"].Value.ToString();
                e.Row.Cells["Vendor"].Value = VendorCode;
                e.Row.Cells["VendorName"].Value = "";
                VendorCode = e.Row.Cells["Vendor"].Value.ToString();
                if (Configuration.convertNullToString(e.Row.Cells["VendorItemCode"].Value) == string.Empty)
                    e.Row.Cells["VendorItemCode"].Appearance.BackColor = Color.DarkOrange;  //Sprint-25 - PRIMEPOS-XXXX 30-Mar-2017 JY Added

                if (VendorCode != "")
                {
                    oVenData = oVendor.Populate(VendorCode);
                    if (oVenData != null)
                    {
                        if (oVenData.Vendor.Rows.Count > 0)
                        {
                            VendorRow oRow = oVenData.Vendor[0];
                            if (oRow.USEVICForEPO == true)
                            {
                                e.Row.Cells["VendorType"].Value = "EDI";
                                e.Row.Cells["VendorType"].Appearance.BackColor = Color.Cyan;
                            }
                            else
                            {
                                e.Row.Cells["VendorType"].Value = "Non-EDI";
                                e.Row.Cells["VendorType"].Appearance.BackColor = Color.DarkOrange;
                            }
                        }
                    }
                }
                else
                {
                    e.Row.Cells["VendorType"].Appearance.BackColor = Color.Red;
                }
            }
            catch
            {
            }
            
            #region Sprint-22 - 2233 06-Oct-2015 JY Added
            try
            {
                this.txtNoOfItems.Text = this.grdItemDetails.Rows.Count.ToString();
            }
            catch { }
            #endregion
        }

        private void grdItemDetails_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            if (e.Layout.Bands[0].Columns.Exists("VendorType") == false)
            {
                e.Layout.Bands[0].Columns.Add("VendorType");
            }
        }

        private void btnDiscard_Click(object sender, EventArgs e)
        {
            if (grdItemDetails.Rows.Count > 0)
            {
                DialogResult result = POS_Core_UI.Resources.Message.Display("You have not assigned VendorItemCode to the item(s) in list.\n Your action will discard  all item(s) from the list, Are you sure? ", "Non Vendor Items", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (result == DialogResult.No)
                {
                    return;
                }
                else
                {
                    this.DialogResult = DialogResult.Cancel;
                    this.Close();
                }
            }
            else //Sprint 24 - PRIMEPOS-2343 08-Sep-2016 JY Added else to fix the issue - as if we deleted all items then we can't close this window
            {
                this.DialogResult = DialogResult.Cancel;
                this.Close();
            }
        }

        private void btnItemEdit_Click(object sender, EventArgs e)
        {
            if (grdItemDetails.ActiveRow != null)
            {
                frmItems oItems = new frmItems();
                string itemId = grdItemDetails.ActiveRow.Cells["ItemId"].Text.Trim();
                oItems.Edit(itemId);
                oItems.ShowDialog(this);
                {
                    DataRow[] rows = dsNonVendItems.PODetail.Select("ItemId = '" + itemId + "'");
                    if (oItems.isItemVendorSave && rows.Length > 0)
                    {


                        drAddedItems = dsAddedItems.PODetail.NewPODetailRow();
                        drAddedItems.ItemArray = ((PODetailRow)rows[0]).ItemArray;
                        dsAddedItems.PODetail.Rows.Add(drAddedItems);
                        dsNonVendItems.PODetail.Rows.Remove(rows[0]);
                        grdItemDetails.Update();

                    }
                    else
                    {
                        try
                        {
                            ItemSvr oItemSvr = new ItemSvr();
                            ItemData oitemData = oItemSvr.Populate(itemId);
                            rows[0]["Description"] = oitemData.Tables[0].Rows[0][clsPOSDBConstants.Item_Fld_Description];
                            rows[0]["Cost"] = oitemData.Tables[0].Rows[0][clsPOSDBConstants.Item_Fld_LastCostPrice];
                            //rows[0]["Price"] = oitemData.Tables[0].Rows[0][clsPOSDBConstants.Item_Fld_UpdatePrice];
                            //rows[0]["Qty"] = oitemData.Tables[0].Rows[0][clsPOSDBConstants.Item_Fld_qt];
                            //rows[0]["LastOrd.Qty"] = oitemData.Tables[0].Rows[0][clsPOSDBConstants.Item_Fld];
                            //rows[0]["Item Sold"] = oitemData.Tables[0].Rows[0][clsPOSDBConstants.Item_Fld_];//QtyInStock	MinOrdQty
                            rows[0]["QtyInStock"] = oitemData.Tables[0].Rows[0][clsPOSDBConstants.Item_Fld_QtyInStock];
                            rows[0]["MinOrdQty"] = oitemData.Tables[0].Rows[0][clsPOSDBConstants.Item_Fld_MinOrdQty];
                            rows[0]["ItemPrice"] = oitemData.Tables[0].Rows[0][clsPOSDBConstants.Item_Fld_SellingPrice];
                            rows[0]["Discount"] = oitemData.Tables[0].Rows[0][clsPOSDBConstants.Item_Fld_Discount];

                            //dsNonVendItems.r}
                        }
                        catch { }
                        grdItemDetails.Refresh();
                    }
                }
            }
        }

        private void ultraButton1_Click(object sender, EventArgs e)
        {
            PrintGridData();
        }

        private void PrintGridData()
        {
            DataSet dsReport = new DataSet();
            DataTable dt = new DataTable();
            dsReport.Tables.Add(dt);
            rptItemWithoutVendorItemNo oRpt = new rptItemWithoutVendorItemNo();

            try
            {
                dsReport.Tables[0].Columns.Add("VendorCode");
                dsReport.Tables[0].Columns.Add("ItemID");
                dsReport.Tables[0].Columns.Add("Description");
                dsReport.Tables[0].Columns.Add("CostPrice");
                dsReport.Tables[0].Columns.Add("SellingPrice");
                dsReport.Tables[0].Columns.Add("QTY");
                dsReport.Tables[0].Columns.Add("QTYInStock");
                dsReport.Tables[0].Columns.Add("MinOrdQty");
                dsReport.Tables[0].Columns.Add("ReorderLevel");
                dsReport.Tables[0].Columns.Add("Vendortype");

                int RowIndex = 0;
                foreach (PODetailRow oRow in dsNonVendItems.Tables[0].Rows)
                {
                    DataRow drRow = dsReport.Tables[0].NewRow();
                    string vendorcode = Configuration.convertNullToString(grdItemDetails.Rows[RowIndex].Cells["Vendor"].Value.ToString());
                    drRow["VendorCode"] = (string.IsNullOrEmpty(vendorcode) == false) ? vendorcode : "No-Vendor";
                    drRow["ItemID"] = oRow.ItemID;
                    drRow["Description"] = oRow.ItemDescription;
                    drRow["CostPrice"] = oRow.Price;
                    drRow["SellingPrice"] = oRow.RetailPrice;
                    drRow["QTY"] = oRow.QTY;
                    drRow["QTYInStock"] = oRow.QtyInStock;
                    drRow["MinOrdQty"] = oRow.MinOrdQty;
                    drRow["ReorderLevel"] = oRow.ReOrderLevel;
                    string vendorType = grdItemDetails.Rows[RowIndex].Cells["Vendortype"].Text.Trim();
                    drRow["Vendortype"] = (string.IsNullOrEmpty(vendorType) == false) ? vendorType : "No-Vendor";
                    dsReport.Tables[0].Rows.Add(drRow);
                    RowIndex++;
                }
                oRpt.SetDataSource(dsReport.Tables[0]);
                clsReports.PrintReport(oRpt);
            }
            catch (Exception Ex)
            {

                clsUIHelper.ShowErrorMsg(Ex.Message);
            }
        }

        #region Sprint-22 - 2233 06-Oct-2015 JY Added
        private void grdItemDetails_AfterRowsDeleted(object sender, EventArgs e)
        {
            try
            {
                this.txtNoOfItems.Text = this.grdItemDetails.Rows.Count.ToString();
            }
            catch { }
        }
        #endregion

    }//Class
}//Namespace