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
using POS_Core.Resources;

namespace POS_Core_UI
{
    public partial class frmAckIncompleteItems : Form
    {
        ItemData dsItem = new ItemData();
        DataTable dt;
        //PODetailData  VendItems;
        public PODetailData dsAddedItems = new PODetailData();
        PODetailRow drAddedItems;
        
        public frmAckIncompleteItems(DataTable dtTemp)
        {
            InitializeComponent();
            dt = dtTemp;
            //VendItems = dtTemp;
        }

        private void frmAckIncompleteItems_Load(object sender, EventArgs e)
        {
            try
            {                
                grdItemDetails.DataSource = dt;
                grdItemDetails.DisplayLayout.Override.CellClickAction = CellClickAction.RowSelect;
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
                grdItemDetails.DisplayLayout.Bands[0].Columns["POVendorId"].Hidden = true;
                grdItemDetails.DisplayLayout.Bands[0].Columns["OrderId"].Hidden = true;
                
                //this.grdItemDetails.DisplayLayout.Bands[0].Columns["VendorType"].Header.SetVisiblePosition(5,false);
                //this.grdItemDetails.DisplayLayout.Bands[0].Columns["Vendor"].Header.SetVisiblePosition(4, false);
                //this.grdItemDetails.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.Item_Fld_Description].SortIndicator = SortIndicator.Disabled;
                //this.grdItemDetails.DisplayLayout.Bands[0].Columns["VendorType"].SortIndicator = SortIndicator.Descending;
                //this.grdItemDetails.DisplayLayout.Bands[0].Columns["VendorType"].Band.SortedColumns.RefreshSort(true);
                //this.grdItemDetails.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.Item_Fld_Description].SortIndicator = SortIndicator.Ascending;
                //this.grdItemDetails.DisplayLayout.AutoFitStyle = AutoFitStyle.ResizeAllColumns;                                
            }
            catch (Exception Ex)
            {              
                throw;
            }
        }
        //Done button event
        private void btnOk_Click(object sender, EventArgs e)
        {
            //try
            //{
            //    ItemVendorData dsItemVendor = new ItemVendorData();
            //    ItemVendorSvr ItemVendorSvr = new ItemVendorSvr();
            //    List<string> lst = new List<string>();
            //    List<string> lstVendor = new List<string>();

                
            //    if (grdItemDetails.Rows.Count > 0)
            //    {
            //        DialogResult result = POS_Core_UI.Resources.Message.Display("You have not assigned VendorItemCode to the item(s) in list.\n Your action will discard  EDI Vendor item(s) from the list, Are you sure? ", "Non Vendor Items", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            //        if (result == DialogResult.No)
            //        {
            //            return;
            //        }
            //    }
            //    foreach (PODetailRow row in VendItems.PODetail.Rows)
            //    {
            //        lst.Add(row["ItemId"].ToString());
            //    }
            //    string str = string.Join("','", lst.ToArray());
            //    Item oItem = new Item();
            //    ItemData oItemData = new ItemData();

            //    oItemData = oItem.PopulateList(" Where  ItemId in ('" + string.Join("','", lst.ToArray()) + "')" + " AND ( LastVendor <> '' OR  PreferredVendor <> ''" + ")");

            //    POS_Core.BusinessRules.Vendor oVendor = new POS_Core.BusinessRules.Vendor();
            //    VendorData oVenData = new VendorData();
            //    foreach (PODetailRow pdr in VendItems.PODetail.Rows)
            //    {
            //        if (pdr.VendorItemCode == "" && pdr.VendorID == 0)
            //        {
            //            string itemcode = pdr["ItemId"].ToString();
            //            DataRow[] rows = oItemData.Item.Select("ItemId = '" + pdr["ItemId"].ToString().Trim() + "'");
            //            if (rows != null && rows.Length > 0)
            //            {
            //                ItemRow row = (ItemRow)rows[0];
            //                string VendorCode = "";
            //                if (row.PreferredVendor != "")
            //                {
            //                    VendorCode = row.PreferredVendor; 
            //                }
            //                else if (row.LastVendor != "")
            //                {
            //                    VendorCode = row.LastVendor;
            //                }
            //                if (VendorCode != "")
            //                {
            //                    oVenData = oVendor.Populate(VendorCode.Trim());
            //                    if (oVenData != null)
            //                    {
            //                        if (oVenData.Vendor.Rows.Count > 0)
            //                        {
            //                            VendorRow oRow = oVenData.Vendor[0];
            //                            if (oRow.USEVICForEPO == false)
            //                            {
            //                                pdr.VendorName = oRow.Vendorname;
            //                                pdr.VendorID = oRow.VendorId;
            //                            }
            //                        }
            //                    }
            //                }
            //            }
            //        }
            //    }
            //    PODetailData odata = new PODetailData();
            //    int Index = 0;
            //    foreach (PODetailRow pdr in VendItems.PODetail.Rows)
            //    {
            //        if (pdr.VendorID != 0)
            //        {
            //            odata.PODetail.ImportRow(pdr);
            //            odata.PODetail.Rows[Index]["VendorID"] = pdr.VendorID;
            //            Index++;
            //        }
            //    }
            //    lst.Clear();
            //    foreach (PODetailRow row in dsAddedItems.PODetail.Rows)
            //    {
            //        lst.Add(row["ItemId"].ToString());
            //    }

            //    if (lst.Count > 0)
            //    {
            //        dsItemVendor = ItemVendorSvr.PopulateList(" Where  ItemId in ('" + string.Join("','", lst.ToArray()) + "')");
            //        //Add vendor information in podetails rows.
            //        FillVendorDetails(dsItemVendor);
            //    }
            //    foreach (PODetailRow pdr in dsAddedItems.PODetail.Rows)
            //    {
            //        odata.PODetail.ImportRow(pdr);
            //        odata.PODetail.Rows[Index]["VendorID"] = pdr.VendorID;
            //        Index++;
            //    }
               
            //    dsAddedItems = odata;
            //    this.DialogResult = DialogResult.OK;
            //    this.Close();
            //}

            //catch (Exception Ex)
            //{
            //    MessageBox.Show(Ex.Message);
            //}
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

        //private void grdItemDetails_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        //{
        //    if (e.Layout.Bands[0].Columns.Exists("VendorType") == false)
        //    {
        //        e.Layout.Bands[0].Columns.Add("VendorType");
        //    }
        //}

        private void btnSkipAllItems_Click(object sender, EventArgs e)
        {
            if (grdItemDetails.Rows.Count > 0)
            {
                DialogResult result = POS_Core_UI.Resources.Message.Display("You have not update Item description/Vendor to the item(s) in list.\n Your action will skip all item(s) from the list, Are you sure? ", "Blank Item Description", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (result == DialogResult.No)
                {
                    return;
                }
                else
                {
                    this.DialogResult = DialogResult.OK;                    
                    this.Close();
                }
            }
            else
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
                    ItemSvr oItemSvr = new ItemSvr();
                    DataTable dtTemp = oItemSvr.GetBlankDescItems(Configuration.convertNullToInt(dt.Rows[0]["OrderID"]));
                    grdItemDetails.DataSource = dtTemp;
                    grdItemDetails.DisplayLayout.Override.CellClickAction = CellClickAction.RowSelect;
                    grdItemDetails.Refresh();
                    Application.DoEvents();
                }
            }
        }

        private void ultraButton1_Click(object sender, EventArgs e)
        {
            //PrintGridData();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        //private void PrintGridData()
        //{
        //    DataSet dsReport = new DataSet();
        //    DataTable dt = new DataTable();
        //    dsReport.Tables.Add(dt);
        //    rptItemWithoutVendorItemNo oRpt = new rptItemWithoutVendorItemNo();

        //    try
        //    {
        //        dsReport.Tables[0].Columns.Add("VendorCode");
        //        dsReport.Tables[0].Columns.Add("ItemID");
        //        dsReport.Tables[0].Columns.Add("Description");
        //        dsReport.Tables[0].Columns.Add("CostPrice");
        //        dsReport.Tables[0].Columns.Add("SellingPrice");
        //        dsReport.Tables[0].Columns.Add("QTY");
        //        dsReport.Tables[0].Columns.Add("QTYInStock");
        //        dsReport.Tables[0].Columns.Add("MinOrdQty");
        //        dsReport.Tables[0].Columns.Add("ReorderLevel");
        //        dsReport.Tables[0].Columns.Add("Vendortype");

        //        int RowIndex = 0;
        //        foreach (PODetailRow oRow in dsNonVendItems.Tables[0].Rows)
        //        {
        //            DataRow drRow = dsReport.Tables[0].NewRow();
        //            string vendorcode = Configuration.convertNullToString(grdItemDetails.Rows[RowIndex].Cells["Vendor"].Value.ToString());
        //            drRow["VendorCode"] = (string.IsNullOrEmpty(vendorcode) == false) ? vendorcode : "No-Vendor";
        //            drRow["ItemID"] = oRow.ItemID;
        //            drRow["Description"] = oRow.ItemDescription;
        //            drRow["CostPrice"] = oRow.Price;
        //            drRow["SellingPrice"] = oRow.RetailPrice;
        //            drRow["QTY"] = oRow.QTY;
        //            drRow["QTYInStock"] = oRow.QtyInStock;
        //            drRow["MinOrdQty"] = oRow.MinOrdQty;
        //            drRow["ReorderLevel"] = oRow.ReOrderLevel;
        //            string vendorType = grdItemDetails.Rows[RowIndex].Cells["Vendortype"].Text.Trim();
        //            drRow["Vendortype"] = (string.IsNullOrEmpty(vendorType) == false) ? vendorType : "No-Vendor";
        //            dsReport.Tables[0].Rows.Add(drRow);
        //            RowIndex++;
        //        }
        //        oRpt.SetDataSource(dsReport.Tables[0]);
        //        clsReports.PrintReport(oRpt);
        //    }
        //    catch (Exception Ex)
        //    {

        //        clsUIHelper.ShowErrorMsg(Ex.Message);
        //    }
        //}

        //private void grdItemDetails_AfterRowsDeleted(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        this.txtNoOfItems.Text = this.grdItemDetails.Rows.Count.ToString();
        //    }
        //    catch { }
        //}

    }//Class
}//Namespace