using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using POS_Core.BusinessRules;
using POS_Core.CommonData;

namespace POS_Core_UI
{
    public partial class frmComparePrices : Form
    {
        private Boolean isIncluded = false;  
        private DataSet oDataSet = null;
        private Search oBLSearch = null;
        private string orderBy = string.Empty;       
        private string sqlQueryForView = "SELECT dbo.Vendor.VendorName as [Vendor Name],dbo.ItemVendor.VendorItemID as [Vendor Item Code] , dbo.Item.ItemID as [Item Code],"+
                                "dbo.Item.Description, dbo.ItemVendor.VendorCostPrice as [Cost Price],dbo.ItemVendor.VendorID FROM  dbo.Item INNER JOIN " +
                                "dbo.ItemVendor ON dbo.Item.ItemID = dbo.ItemVendor.ItemID INNER JOIN dbo.Vendor ON dbo.ItemVendor.VendorID = dbo.Vendor.VendorID ";
        
        /// <summary>
        /// Author : Gaurav
        /// Date : 08-Jul-2009
        /// Gets Count of vendors available to compare.
        /// </summary>
        public int OtherVendorsCount
        {
            get
            {
                return (oDataSet != null ? oDataSet.Tables[0].Rows.Count : 0);
            }
        }
        public frmComparePrices()
        {
            InitializeComponent();
            oBLSearch = new Search();
            oDataSet = new DataSet();
        }
        private void groupBoxItemGrid_Enter(object sender, EventArgs e)
        {

        }
        public void CompareVendorPrices(String upcCode)
        {
            string queryForBestPrice = string.Empty;
            string itemId = string.Empty;
            string tableName = string.Empty;
            string orderBy = string.Empty;
            try
            {
                itemId = clsPOSDBConstants.Item_Fld_ItemID;
                tableName = clsPOSDBConstants.Item_tbl;
                orderBy = "dbo.ItemVendor.VendorCostPrice";
                queryForBestPrice = sqlQueryForView + " WHERE " + tableName + "." + itemId + "='" + upcCode + "' ORDER BY " + orderBy + "";
                oDataSet = oBLSearch.SearchData(queryForBestPrice);
                this.gridItemDetails.DataSource = oDataSet;
                this.ultraTxtEditorNoOfVendors.Text  = oDataSet.Tables[0].Rows.Count.ToString(); 
            }
            catch (Exception ex)
            {
                clsUIHelper.ShowErrorMsg(ex.Message);
            }
        }
        public string SelectedVendItemId()
        {
            string selectedRowVendItemId = string.Empty;
            try
            {
                if (gridItemDetails.ActiveRow.Cells["Vendor Item Code"] != null)
                {
                    selectedRowVendItemId = gridItemDetails.ActiveRow.Cells["Vendor Item Code"].Text;
                }
            }
            catch (Exception ex)
            {
                clsUIHelper.ShowErrorMsg(ex.Message);
            }
            return selectedRowVendItemId;
        }
        public string SelectedDescription()
        {
            string selectedDescription = string.Empty; 
            try
            {
                if (gridItemDetails.ActiveRow.Cells["Description"] != null)
                {
                    selectedDescription = gridItemDetails.ActiveRow.Cells["Description"].Text;
                }
            }
            catch (Exception ex)
            {
                clsUIHelper.ShowErrorMsg(ex.Message);
            }
            return selectedDescription;
        }
        public string SelectedItemID()
        {
            string selectedRowItemId = string.Empty;
            try
            {
                if (gridItemDetails.ActiveRow.Cells["Item Code"] != null)
                {
                    selectedRowItemId = gridItemDetails.ActiveRow.Cells["Item Code"].Text;
                }
            }
            catch (Exception ex)
            {
                clsUIHelper.ShowErrorMsg(ex.Message);
            }
            return selectedRowItemId;
        }
        public string SelectedCostPrice()
        {
            string selectedRowCostPrice = string.Empty;
            try
            {
                if (gridItemDetails.ActiveRow.Cells["Cost Price"] != null)
                {
                    selectedRowCostPrice = gridItemDetails.ActiveRow.Cells["Cost Price"].Text;
                }
            }
            catch (Exception ex)
            {
                clsUIHelper.ShowErrorMsg(ex.Message);
            }
            return selectedRowCostPrice;
        }
        public Boolean IsIncluded
        {
           get{return isIncluded;}
        }
        public string SelectedVendorName()
        {
            string selectedVendorName = string.Empty; 
            try
            {
                if (gridItemDetails.ActiveRow.Cells["Vendor Name"] != null)
                {
                    selectedVendorName = gridItemDetails.ActiveRow.Cells["Vendor Name"].Text;
                }
            }
            catch (Exception ex)
            {
                clsUIHelper.ShowErrorMsg(ex.Message);
            }
            return selectedVendorName;
        }
        public string SelectedVendorID()
        {
            string selectedVendorName = string.Empty;
            try
            {
                if (gridItemDetails.ActiveRow!= null)
                {
                    selectedVendorName = gridItemDetails.ActiveRow.Cells["VendorID"].Text;
                }
            }
            catch (Exception ex)
            {
                clsUIHelper.ShowErrorMsg(ex.Message);
            }
            return selectedVendorName;
        }
        private void btnIncludeSelectedItem_Click(object sender, EventArgs e)
        {
            isIncluded = true;
            this.Close();
        }
        private void gridItemDetails_DoubleClick(object sender, EventArgs e)
        {
            isIncluded = true;
            this.Close();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmComparePrices_Load(object sender, EventArgs e)
        {
            clsUIHelper.setColorSchecme(this);
            clsUIHelper.setColorSchecme(this.ultraTxtEditorNoOfVendors);
            this.gridItemDetails.DisplayLayout.Bands[0].Columns["VendorID"].Hidden = true;   
        }

        private void gridItemDetails_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                isIncluded = true;
                this.Close();
            }
        }
    }
}