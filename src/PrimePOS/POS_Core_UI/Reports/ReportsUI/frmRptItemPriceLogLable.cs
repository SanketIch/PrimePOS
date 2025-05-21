using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using CrystalDecisions.Shared;
using CrystalDecisions.ReportSource;
using CrystalDecisions.CrystalReports.Engine;
using POS_Core_UI.Reports.Reports;
//using POS.UI;
using POS_Core.CommonData;
using System.Data;
using POS_Core.BusinessRules;
using POS_Core.DataAccess;
using POS_Core.CommonData.Rows;
using POS_Core.CommonData.Tables;
using Infragistics.Win.UltraWinGrid;
using Infragistics.Win;
//using POS_Core.DataAccess;
using System.Data.SqlClient;
using Resources;
using POS_Core.Resources;
using POS_Core.LabelHandler;
using System.Timers;

namespace POS_Core_UI.Reports.ReportsUI
{
    public partial class frmRptItemPriceLogLable : Form
    {
        //private Infragistics.Win.UltraWinEditors.UltraTextEditor txtDeptCode;
        private DataSet oDS;
        private DataSet ds;
        private IDataAdapter da;
        private POSSET oPOSSet;
        private int RowCount = 0;
        ReportClass oTemplateRptObj = null;

        #region PRIMEPOS-2464 10-Mar-2020 JY Added
        int nDisplayItemCost = 0;
        System.Timers.Timer tmrBlinking;
        private long iBlinkCnt = 0;
        #endregion
        public int InvRecievedID = 0;   //PRIMEPOS-2397 17-Nov-2020 JY Added
        public string ItemID = string.Empty;    //PRIMEPOS-2243 27-Apr-2021 JY Added
        public int VendorID = 0;    //PRIMEPOS-2243 27-Apr-2021 JY Added

        public frmRptItemPriceLogLable()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            InvRecievedID = 0;  //PRIMEPOS-2397 17-Nov-2020 JY Added
            this.Close();
        }

        private void frmRptItemPriceLogLable_Load(object sender, EventArgs e)
        {
            oPOSSet = Configuration.CPOSSet;
            ds = new DataSet();
            da = DataFactory.CreateDataAdapter();
            txtItemCode.Focus();
            Mabry.Windows.Forms.Barcode.Licenser.Key = "E1P8-HKELVF8R04Q0";
            clsUIHelper.setColorSchecme(this);
            clsUIHelper.SetKeyActionMappings(this.grdSearch);
            dtpFromDate.Value = System.DateTime.Now;
            dtpToDate.Value = System.DateTime.Now;
            CmbPriceType.SelectedIndex = 1;
            rdSelectionCriteria.Checked = true;
            Configuration.GetLabelPrintingSetup();
            fillVendorList();
            fillLabelObjectList();
            fillLabelTemplateList();
            if (oPOSSet.ReceiptPrinterType.ToUpper() == "L")
            {
                chkUseLabelPrinter.Checked = true;
            }
            else
            {
                chkUseLabelPrinter.Checked = false;
            }

            //rdElectronicRsChange.Checked = true;  //PRIMEPOS-1871 24-Mar-2020 JY Commented
            rdAll.Checked = true;   //PRIMEPOS-1871 24-Mar-2020 JY Added
            grdSearch.DisplayLayout.Bands[0].Columns["No. Of Labels"].MaxValue = 999;
            grdSearch.DisplayLayout.Bands[0].Columns["No. Of Labels"].MaskInput = "999";
            grdSearch.DisplayLayout.Bands[0].Columns["No. Of Labels"].Format = "##0";

            grdSearch.DisplayLayout.Bands[0].Columns["No. Of Sheet"].MaxValue = 999;
            grdSearch.DisplayLayout.Bands[0].Columns["No. Of Sheet"].MaskInput = "999";
            grdSearch.DisplayLayout.Bands[0].Columns["No. Of Sheet"].Format = "##0";

            grdSearch.DisplayLayout.Bands[0].Columns["Check Items"].Header.VisiblePosition = 0;

            grdSearch.DisplayLayout.Bands[0].Columns[5].Hidden = true;


            this.txtDeptCode.AfterEnterEditMode += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
            this.txtDeptCode.AfterExitEditMode += new System.EventHandler(clsUIHelper.AfterExitEditMode);

            this.txtItemCode.AfterEnterEditMode += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
            this.txtItemCode.AfterExitEditMode += new System.EventHandler(clsUIHelper.AfterExitEditMode);

            this.txtNoOfLabels.AfterEnterEditMode += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
            this.txtNoOfLabels.AfterExitEditMode += new System.EventHandler(clsUIHelper.AfterExitEditMode);

            this.txtNoOfSheet.AfterEnterEditMode += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
            this.txtNoOfSheet.AfterExitEditMode += new System.EventHandler(clsUIHelper.AfterExitEditMode);

            #region PRIMEPOS-2464 10-Mar-2020 JY Added
            nDisplayItemCost = Configuration.convertBoolToInt(UserPriviliges.IsUserHasPriviliges(UserPriviliges.Modules.InventoryMgmt.ID, UserPriviliges.Screens.ItemFile.ID, UserPriviliges.Permissions.DisplayItemCost.ID));   //PRIMEPOS-2464 09-Mar-2020 JY Added
            if (nDisplayItemCost == 0)
            {
                lblMessage.Visible = true;
                tmrBlinking = new System.Timers.Timer();
                tmrBlinking.Interval = 1000;//1 seconds
                tmrBlinking.Elapsed += new ElapsedEventHandler(tmrBlinkingTimedEvent);
                tmrBlinking.Enabled = true;
            }
            else
            {
                this.Height -= 15;
            }
            #endregion

            #region PRIMEPOS-2397 17-Nov-2020 JY Added
            //if (InvRecievedID > 0)    //PRIMEPOS-2243 27-Apr-2021 JY Commented
            if (InvRecievedID > 0 || ItemID != string.Empty)    //PRIMEPOS-2243 27-Apr-2021 JY Added
            {
                if (ShowData())
                {
                    int rowIndex = 0;
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow oRow in ds.Tables[0].Rows)
                        {
                            this.grdSearch.Rows[rowIndex].Cells["Check Items"].Value = false;
                            this.grdSearch.Rows[rowIndex].Cells["No. Of Labels"].Value = Configuration.convertNullToInt(txtNoOfLabels.Value);   //PRIMEPOS-2630 16-Jan-2019 JY modified
                            this.grdSearch.Rows[rowIndex].Cells["No. Of Sheet"].Value = Configuration.convertNullToInt(txtNoOfSheet.Value); //PRIMEPOS-2630 16-Jan-2019 JY modified
                            rowIndex++;
                        }
                    }
                    grdSearch.DisplayLayout.Bands[0].Columns["Vendor Name"].Header.VisiblePosition = 3;
                }
                lblRecordCount.Text = "Records Count = " + grdSearch.Rows.Count;
            }
            #endregion
        }

        POS_Core.BusinessRules.Vendor vendor = new POS_Core.BusinessRules.Vendor();
        VendorData vendorData = new VendorData();
        VendorSvr vendorSvr = new VendorSvr();

        private void fillVendorList()
        {

            string whereClause = " ORDER BY vendorname";
            vendorData = vendor.PopulateList(whereClause);
            dataGridList.DataSource = vendorData.Vendor;//.Select("1 = 1", "vendorname");
            for (int i = 1; i <= vendorData.Vendor.Columns.Count; i++)
            {
                dataGridList.Columns[i].ReadOnly = true;

                if (i != 3)
                    dataGridList.Columns[i].Visible = false;
                else
                {
                    dataGridList.Columns[i].Width = dataGridList.Width - dataGridList.Columns[0].Width - 20;
                    dataGridList.Columns[i].Name = "Vendors";
                }
            }
        }
       
        private bool GridVisibleFlag = false;
        private void comboVendorList_ValueChanged(object sender, EventArgs e)
        {

        }

        private void comboVendorList_MouseClick(object sender, MouseEventArgs e)
        {

        }

        private void butVendorList_Click(object sender, EventArgs e)
        {
            if (grpVendorList.Visible == false)
                VendorList_Expand(true);
            else
                VendorList_Expand(false);
        }

        private void txtDeptCode_EditorButtonClick(object sender, Infragistics.Win.UltraWinEditors.EditorButtonEventArgs e)
        {
            //frmSearch oSearch = new frmSearch(clsPOSDBConstants.Department_tbl);
            frmSearchMain oSearch = new frmSearchMain(true);    //20-Dec-2017 JY Added new reference
            oSearch.SearchTable = clsPOSDBConstants.Department_tbl;    //20-Dec-2017 JY Added 
            oSearch.ShowDialog();
            if (oSearch.IsCanceled)
            {
                txtDeptCode.Text = "";
                txtDeptCode.Tag = null;
            }
            else
            {
                txtDeptCode.Tag = oSearch.SelectedRowID();
                txtDeptCode.Text = oSearch.SelectedCode();
            }
        }

        private void txtDeptCode_Validating(object sender, CancelEventArgs e)
        {
            if (txtDeptCode.Text.Trim() == "")
            {
                txtDeptCode.Tag = null;
            }
        }

        private void butShowItems_Click(object sender, EventArgs e)
        {
            int rowIndex = 0;
            btnRemoveItems_Click(sender, e);
            VendorList_Expand(false);
            bool ByDepartment = false;
            if (rdSelectionCriteria.Checked == true)
                ByDepartment = false;
            else if (rdDepartmentSearch.Checked == true)
            {
                if (txtDeptCode.Text == string.Empty)
                {
                    clsUIHelper.ShowErrorMsg("Please select Department");
                    txtDeptCode.Focus();
                    return;
                }
                ByDepartment = true;
            }
            if (ShowData(true, false, ByDepartment))
            {
                foreach (DataRow oRow in ds.Tables[0].Rows)
                {
                    this.grdSearch.Rows[rowIndex].Cells["Check Items"].Value = false;
                    this.grdSearch.Rows[rowIndex].Cells["No. Of Labels"].Value = Configuration.convertNullToInt(txtNoOfLabels.Value);   //PRIMEPOS-2630 16-Jan-2019 JY modified
                    this.grdSearch.Rows[rowIndex].Cells["No. Of Sheet"].Value = Configuration.convertNullToInt(txtNoOfSheet.Value); //PRIMEPOS-2630 16-Jan-2019 JY modified
                    rowIndex++;
                }
                grdSearch.DisplayLayout.Bands[0].Columns["Vendor Name"].Header.VisiblePosition = 3;
            }
            lblRecordCount.Text = "Records Count = " + grdSearch.Rows.Count;
        }
        private void GridDefaultValue()
        {
            int rowIndex = 0;
            if (ds != null)
            {
                if (ds.Tables.Count != 0)
                {

                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow oRow in ds.Tables[0].Rows)
                        {

                            //if (Convert.ToBoolean(this.grdSearch.Rows[rowIndex].Cells["Check Items"].Value) == true)
                            {
                                this.grdSearch.Rows[rowIndex].Cells["No. Of Labels"].Value = Configuration.convertNullToInt(txtNoOfLabels.Value);   //PRIMEPOS-2630 16-Jan-2019 JY modified
                                this.grdSearch.Rows[rowIndex].Cells["No. Of Sheet"].Value = Configuration.convertNullToInt(txtNoOfSheet.Value); //PRIMEPOS-2630 16-Jan-2019 JY modified

                            }
                            rowIndex++;
                        }
                    }
                }
            }
        }

        private string buildCriteria()
        {
            string sCriteria = "";
            bool OrFlag = false;
            bool chkVendor = false;

            if (dtpFromDate.Value.ToString() != "")
                sCriteria = sCriteria + " AND Convert(smalldatetime,Convert(Varchar,AddedOn,107)) >= '" + dtpFromDate.Text + "'";
            if (dtpToDate.Value.ToString() != "")
                sCriteria = sCriteria + " AND Convert(smalldatetime,Convert(Varchar,AddedOn,107)) <= '" + dtpToDate.Text + "'";
            if (txtDeptCode.Tag != null)
                sCriteria = sCriteria + " AND Dept.DeptCode = '" + txtDeptCode.Tag.ToString() + "'";
            if (chkInstockItm.Checked)
                sCriteria = sCriteria + " AND Item.QtyInStock > 0 ";

            int rowsCount = dataGridList.Rows.Count;
            if (rowsCount > 0)
            {
                sCriteria = sCriteria + " AND (";
                for (int i = 0; i < rowsCount; i++)
                {
                    if (Convert.ToBoolean(dataGridList.Rows[i].Cells[0].Value) == true)
                    {
                        chkVendor = true;
                        if (OrFlag == true)
                            sCriteria = sCriteria + " OR ";
                        sCriteria = sCriteria + " ItemVendor.VendorID = '" + dataGridList.Rows[i].Cells[1].Value.ToString() + "' OR Item.LastVendor ='" + dataGridList.Rows[i].Cells[2].Value.ToString().Replace("'", "''") + "' ";  //PRIMEPOS-2630 16-Jan-2019 JY Added logic to avoid exception due to "'"
                        OrFlag = true;

                    }
                }
                sCriteria = sCriteria + " )";
            }

            #region PRIMEPOS-1871 24-Mar-2020 JY Added
            if (CmbPriceType.SelectedIndex == 0)    //cost price
            {
                sCriteria += "AND ((IPH.UpdatedBy = 'M' AND IPH.CostPrice <> IPH.OrgCostPrice) OR IPH.UpdatedBy = 'E')";
            }
            else //selling price
            {
                sCriteria += "AND ((IPH.UpdatedBy = 'M' AND IPH.SalePrice <> IPH.ORGSELLINGPRICE) OR IPH.UpdatedBy = 'E')";
            }
            #endregion

            if (chkVendor == false)
            {
                clsUIHelper.ShowErrorMsg("Please Select Vendor From Vendor List");
                return "-1";
            }

            return sCriteria;
        }

        private string buildCriteria_ForReport()
        {
            string sCriteria = "";
            bool OrFlag = false;
            bool OrFlagR = false;
            bool chkVendor = false;
            int rowIndex = 0;
            if (dtpFromDate.Value.ToString() != "")
                sCriteria = sCriteria + " AND Convert(smalldatetime,Convert(Varchar,AddedOn,107)) >= '" + dtpFromDate.Text + "'";
            if (dtpToDate.Value.ToString() != "")
                sCriteria = sCriteria + " AND Convert(smalldatetime,Convert(Varchar,AddedOn,107)) <= '" + dtpToDate.Text + "'";
            if (txtDeptCode.Tag != null)
                sCriteria = sCriteria + " AND Dept.DeptCode = '" + txtDeptCode.Tag.ToString() + "'";

            bool chkFlag = false;
            if (ds == null)
                return "-1";
            if (ds.Tables.Count == 0)
                return "-1";
            foreach (DataRow oRow in ds.Tables[0].Rows)
            {
                if (Convert.ToBoolean(this.grdSearch.Rows[rowIndex].Cells["Check Items"].Value) == true)
                {
                    chkFlag = true;
                    break;
                }
            }

            rowIndex = 0;
            if (chkFlag == true)
            {
                sCriteria = sCriteria + " AND (";
                foreach (DataRow oRow in ds.Tables[0].Rows)
                {
                    if (Convert.ToBoolean(this.grdSearch.Rows[rowIndex].Cells["Check Items"].Value) == true)
                    {
                        chkFlag = true;
                        if (OrFlagR == true)
                            sCriteria = sCriteria + " OR ";
                        sCriteria = sCriteria + " item.ItemId =  '" + this.grdSearch.Rows[rowIndex].Cells["Item ID"].Value + "' ";
                        OrFlagR = true;
                    }
                    rowIndex++;
                }
                sCriteria = sCriteria + " )";
            }
            int rowsCount = dataGridList.Rows.Count;
            if (rowsCount > 0)
            {
                sCriteria = sCriteria + " AND (";
                for (int i = 0; i < rowsCount; i++)
                {
                    if (Convert.ToBoolean(dataGridList.Rows[i].Cells[0].Value) == true)
                    {
                        chkVendor = true;
                        if (OrFlag == true)
                            sCriteria = sCriteria + " OR ";
                        sCriteria = sCriteria + " ItemVendor.VendorID = '" + dataGridList.Rows[i].Cells[1].Value.ToString() + "' ";
                        OrFlag = true;

                    }
                }
                sCriteria = sCriteria + " )";
            }

            #region PRIMEPOS-1871 24-Mar-2020 JY Added
            if (rdManualRSChange.Checked == true)
            {
                if (CmbPriceType.SelectedIndex == 0)    //cost price
                {
                    sCriteria += " AND (IPH.UpdatedBy = 'M' AND IPH.CostPrice <> IPH.OrgCostPrice)";
                }
                else //selling price
                {
                    sCriteria += " AND (IPH.UpdatedBy = 'M' AND IPH.SalePrice <> IPH.ORGSELLINGPRICE)";
                }
                
            }
            else if (rdElectronicRsChange.Checked == true)
            {
                sCriteria += " AND IPH.UpDatedBy = 'E'";
            }
            #endregion

            if (chkVendor == false)
            {
                clsUIHelper.ShowErrorMsg("Please Select Vendor From Vendor List");
                return "-1";
            }

            return sCriteria;

        }

        /// <summary>
        /// Added By shitaljit(QuicSolv) on 16 Jan 2012
        /// To Add populate item to the grid.
        /// </summary>
        /// <param name="code"></param>
        /// <param name="senderName"></param>
        //private void FKEdit(string code, string senderName)
        //{
        //    if (senderName == clsPOSDBConstants.Item_tbl)
        //    {
        //        #region Items
        //        try
        //        {
        //            Item oItem = new Item();
        //            ItemData oItemData;
        //            ItemRow oItemRow = null;
        //            oItemData = oItem.Populate(code);
        //            oItemRow = oItemData.Item[0];
        //            if (oItemRow != null)
        //            {
        //                if (oItemRow.QtyInStock > 0 || !chkInstockItm.Checked)
        //                {
        //                    ds.Tables[0].Rows.Add(new object[] { oItemRow.ItemID, "", oItemRow.Description, oItemRow.SellingPrice, oItemRow.LastCostPrice, oItemRow.LastVendor, oItemRow.ProductCode, oItemRow.Location, oItemRow.MinOrdQty });
        //                    this.grdSearch.Focus();
        //                    if (this.grdSearch.Rows.Count > 0)
        //                    {
        //                        this.grdSearch.ActiveRow = this.grdSearch.Rows[this.grdSearch.Rows.Count - 1];
        //                    }
        //                    this.grdSearch.PerformAction(UltraGridAction.EnterEditMode);
        //                }
        //                else
        //                {
        //                    clsUIHelper.ShowErrorMsg("Item is not present in stock");
        //                }
        //            }
        //        }
        //        catch (System.IndexOutOfRangeException)
        //        {
        //            this.grdSearch.ActiveCell.Value = String.Empty;
        //            this.grdSearch.ActiveRow.Cells["Description"].Value = String.Empty;
        //        }
        //        catch (Exception exp)
        //        {
        //            clsUIHelper.ShowErrorMsg(exp.Message);
        //            exp = null;
        //            this.grdSearch.ActiveCell.Value = String.Empty;
        //            this.grdSearch.ActiveRow.Cells["Description"].Value = String.Empty;
        //        }
        //        #endregion
        //    }
        //}

        /// <summary>
        /// Added By shitaljit(QuicSolv) on 16 Jan 2012
        /// To search the item.
        /// </summary>
        private string SearchItem()
        {
            string strCode = "";
            try
            {
                //frmSearch oSearch = new frmSearch(clsPOSDBConstants.Item_tbl, txtItemCode.Text, txtItemName.Text);
                frmSearchMain oSearch = new frmSearchMain(clsPOSDBConstants.Item_tbl, txtItemCode.Text, txtItemName.Text, true);    //20-Dec-2017 JY Added new reference
                oSearch.ShowDialog(this);
                if (!oSearch.IsCanceled)
                {
                    strCode = oSearch.SelectedRowID();
                }
            }
            catch (Exception exp)
            {
                this.Cursor = Cursors.Default;
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
            return strCode;
        }
        private bool ShowData(bool PrintId, bool AddItems, bool byDepatment)
        {
            try
            {
                //rptItemPriceChangeLog oRpt = new rptItemPriceChangeLog();

                //btnRemoveItems_Click(null, null);//By Shitaljit(QuicSolv) on 26 August 2011
                IDbCommand cmd = DataFactory.CreateCommand();
                string sSQL = "";
                string Criteria = "";
                string tempSQL = "";

                IDbConnection conn = DataFactory.CreateConnection();

                conn.ConnectionString = Configuration.ConnectionString;

                conn.Open();

                try
                {

                    if (AddItems == false)
                    {
                        if (byDepatment == false)
                        {
                            sSQL = " SELECT distinct(Item.itemID) as 'Item ID'" +
                                ", ItemVendor.VendorItemID" +    //Added by Amit Date 7 july 2011
                                ", Item.Description as 'Item Name'" +
                                ", Item.sellingPrice as 'Selling Price'" +
                                ", ItemVendor.vendorCostPrice as 'Cost Price'" +
                                ", Vendor.VendorName as 'Vendor Name'" +
                                ", Item.PreferredVendor as 'PreferredVendor'" +
                                ", ISNULL(Item.ProductCode,'') as 'ProductCode'" +
                                ", Item.Location as 'Location'" +
                                ", ISNULL(Item.ManufacturerName,'') as 'ManufacturerName'" +
                                ", ISNULL(Item.AvgPrice,0.0) as 'AvgPrice'" +  //JY 27-Apr-2015 Removed the extra "Round bracket" due to which query throwing the exception
                                ", ISNULL(Item.OnSalePrice,0.0) as 'OnSalePrice'" +
                            #region Sprint-21 - 09-Sep-2015 JY Added sale price related objects
                                ", ISNULL(Item.isOnSale,0) AS isOnSale" +
                                ", ISNULL(Item.SaleLimitQty,0) AS SaleLimitQty" +
                                ", Item.SaleStartDate,Item.SaleEndDate" +
                            #endregion
                                " FROM Itempricehistory IPH INNER JOIN Item ON IPH.ItemID=Item.ItemID" +
                                " LEFT JOIN Department Dept On Item.DepartmentID = Dept.DeptID" +
                                " INNER JOIN ItemVendor ON Item.ItemID = ItemVendor.ItemID" +
                                " LEFT JOIN  Vendor on vendor.VendorID = ItemVendor.VendorID" +
                                //Commented By Shitaljit(QuicSolv) on 26 August 2011
                                /* " WHERE " +
                                      " ItemVendor.ItemId = item.ItemId" +
                                           " AND IPH.ItemID=Item.ItemID ";*/
                                " WHERE 1 = 1 ";

                            Criteria = buildCriteria();
                            if (Criteria == "-1")
                            {
                                return false;
                            }
                            else
                                sSQL = sSQL + Criteria;
                        }
                        else
                        {
                            sSQL = " SELECT distinct(Item.itemID) as 'Item ID'" +
                                    ", ItemVendor.VendorItemID" + //Added by Amit Date 7 july 2011
                                    ", Item.Description as 'Item Name' " +
                                    ", Item.sellingPrice as 'Selling Price'" +
                                    ", ItemVendor.vendorCostPrice as 'Cost Price'" +
                                    ", Vendor.VendorName as 'Vendor Name'" +
                                    ", Item.PreferredVendor as 'PreferredVendor'" +
                                    ", ISNULL(Item.ProductCode,'') as 'ProductCode'" +
                                    ", Item.Location as 'Location' " +
                                    ", ISNULL(Item.ManufacturerName,'') as 'ManufacturerName'" +
                                    ", ISNULL(Item.AvgPrice,0.0) as 'AvgPrice'" +   //JY 27-Apr-2015 Removed the extra "Round bracket" due to which query throwing the exception
                                    ", ISNULL(Item.OnSalePrice,0.0) as 'OnSalePrice'" +
                            #region Sprint-21 - 09-Sep-2015 JY Added sale price related objects
                                    ", ISNULL(Item.isOnSale,0) AS isOnSale" +
                                    ", ISNULL(Item.SaleLimitQty,0) AS SaleLimitQty" +
                                    ", Item.SaleStartDate,Item.SaleEndDate" +
                            #endregion
                                    " FROM Item LEFT JOIN Department Dept ON Item.DepartmentID = Dept.DeptID" +
                                    //" ,ItemVendor Left Join Vendor on (vendor.VendorID = ItemVendor.VendorID ) "+
                                    //Added By Shitaljit (QuicSolv) on 26 August 2011
                                    " LEFT OUTER JOIN ItemVendor ON Item.ItemID = ItemVendor.ItemID" +
                                    " LEFT OUTER JOIN Vendor ON ItemVendor.VendorID = vendor.VendorID" +
                                    " WHERE 1=1 ";   //JY 27-Apr-2015 Added 1=1 condition to avoid incorrect query building
                                                     // " 1=1";
                                                     //Commented By Shitaljit(QuicSolv) on 26 August 2011
                                                     //  " ItemVendor.ItemId = item.ItemId";
                            if (txtDeptCode.Text != string.Empty)
                                sSQL = sSQL + " AND Dept.DeptCode = '" + txtDeptCode.Text.Trim() + "'";//Modifeid by Shitaljit(QuicSolv) on 26 August 2011 removed And //JY 27-Apr-2015 Addd "AND"
                            if (chkInstockItm.Checked)
                                sSQL = sSQL + "And Item.QtyInStock > 0 ";
                        }
                    }
                    else
                    {

                        sSQL = " SELECT distinct(Item.itemID) as 'Item ID'" +
                                ", ItemVendor.VendorItemID" +   //Added by Amit Date 7 july 2011
                                ", Item.Description as 'Item Name'" +
                                ", Item.sellingPrice as 'Selling Price'" +
                                ", ItemVendor.vendorCostPrice as 'Cost Price'" +
                                ", Vendor.VendorName as 'Vendor Name'" +
                                ", Item.PreferredVendor as 'PreferredVendor'" +
                                ", ISNULL(Item.ProductCode,'') as 'ProductCode'" +
                                ", Item.Location as 'Location'" +
                                ", ISNULL(Item.ManufacturerName,'') as 'ManufacturerName'" +
                                ", ISNULL(Item.AvgPrice,0.0) as 'AvgPrice'" +
                                ", ISNULL(Item.OnSalePrice,0.0) as 'OnSalePrice'" +
                        #region Sprint-21 - 09-Sep-2015 JY Added sale price related objects
                                ", ISNULL(Item.isOnSale,0) AS isOnSale" +
                                ", ISNULL(Item.SaleLimitQty,0) AS SaleLimitQty" +
                                ", Item.SaleStartDate,Item. SaleEndDate" +
                        #endregion
                                " FROM Item LEFT JOIN ItemVendor ON ItemVendor.ItemID = Item.ItemID" +
                                " Left Join Vendor ON vendor.VendorID = ItemVendor.VendorID " + //Added By Shitaljit(QuicSolv) on 26 August 2011 Added  left outer join logic
                                " WHERE 1=1 ";//ItemVendor.ItemId = item.ItemId ";//1=1";//Commented By Shitaljit(QuicSolv) on 26 August 2011  //JY 27-Apr-2015 Added 1=1

                        tempSQL = sSQL;
                        if (txtItemCode.Text != string.Empty)
                            sSQL = sSQL + "AND Item.ItemID like '" + txtItemCode.Text.Trim().Replace("'", "''") + "'"; //Modifeid by Shitaljit(QuicSolv) on 26 August 2011 removed And     //JY 27-Apr-2015 Added "AND"
                        //Modified by shitaljit(QuicSolv) added eslse part in the if condition 
                        if (txtItemName.Text != string.Empty && txtItemCode.Text != string.Empty)
                            sSQL = sSQL + " And Item.Description like '" + txtItemName.Text.Trim() + "'";
                        else if (txtItemName.Text != string.Empty && txtItemCode.Text == string.Empty)
                            sSQL = sSQL + " AND Item.Description like '" + txtItemName.Text.Trim() + "'";   //JY 27-Apr-2015 Added "AND"
                        if (chkInstockItm.Checked && txtItemCode.Text != string.Empty && txtItemName.Text != string.Empty)
                            sSQL = sSQL + "And Item.QtyInStock > 0 ";
                        else if (chkInstockItm.Checked && txtItemCode.Text == string.Empty && txtItemName.Text == string.Empty)
                            sSQL = sSQL + " AND Item.QtyInStock > 0 ";  //JY 27-Apr-2015 Added "AND"
                    }

                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = sSQL;
                    cmd.Connection = conn;
                    SqlDataAdapter sqlDa = (SqlDataAdapter)da;
                    sqlDa.SelectCommand = (SqlCommand)cmd;
                    da.Fill(ds);

                    //Added By Shitaljit(QuicSolv) on 16 Jan 2012 
                    //if item is not found will open search item form.
                    if (RowCount == ds.Tables[0].Rows.Count && AddItems == true)
                    {

                        string strItemCode = SearchItem();
                        if (strItemCode != "")
                        {
                            tempSQL = tempSQL + "AND Item.ItemID like '" + strItemCode + "'";   //JY 27-Apr-2015 Added "AND"
                            cmd.CommandType = CommandType.Text;
                            cmd.CommandText = tempSQL;
                            cmd.Connection = conn;
                            sqlDa = (SqlDataAdapter)da;
                            sqlDa.SelectCommand = (SqlCommand)cmd;
                            da.Fill(ds);
                        }
                    }
                    RowCount = ds.Tables[0].Rows.Count;
                    //END of added by shitaljit.

                    //da.Fill(ds);
                    conn.Close();

                    //txtNoOfLabels.ResetValue(); //PRIMEPOS-06012017 02-Jun-2017 JY Added
                    //txtNoOfSheet.ResetValue();  //PRIMEPOS-06012017 02-Jun-2017 JY Added

                    grdSearch.DataSource = ds;

                    #region PRIMEPOS-2464 10-Mar-2020 JY Added
                    try
                    {
                        if (nDisplayItemCost == 0)
                        {
                            grdSearch.DisplayLayout.Bands[0].Columns["Cost Price"].Hidden = true;
                        }
                    }
                    catch(Exception Ex)
                    {

                    }
                    #endregion
                }
                catch (NullReferenceException)
                {
                    conn.Close();
                    //ErrorHandler.throwCustomError(POSErrorENUM.User_InvalidUserName);
                }
                catch (Exception exp)
                {
                    conn.Close();
                    throw (exp);
                }
            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
            return true;
        }

        private void setNew()
        {
            oDS = new DataSet();
            oDS.Tables.Add();
            oDS.Tables[0].Columns.Add("ItemID", typeof(System.String));
            oDS.Tables[0].Columns.Add("ItemName", typeof(System.String));
            oDS.Tables[0].Columns.Add("NoofLabels", typeof(System.String));
            oDS.Tables[0].Columns.Add("NoofSheets", typeof(System.String));
            oDS.Tables[0].Columns.Add("SellingPrice", typeof(System.Decimal));
            oDS.Tables[0].Columns.Add("ProductCode", typeof(System.String));
            oDS.Tables[0].Columns.Add("Location", typeof(System.String));
            oDS.Tables[0].Columns.Add("VendorCode", typeof(System.String));//Added by SRT (Abhishek D) Date : 04 April 2010
            oDS.Tables[0].Columns.Add("VendorID", typeof(System.String));
            oDS.Tables[0].Columns.Add("PCKSIZE", typeof(System.String));
            oDS.Tables[0].Columns.Add("PCKQTY", typeof(System.String));
            oDS.Tables[0].Columns.Add("PCKUNIT", typeof(System.String));
            //Added by Amit Date 7 July 2011
            oDS.Tables[0].Columns.Add("VendorItemID", typeof(System.String));

            oDS.Tables[0].Columns.Add("ManufacturerName", typeof(System.String));//Added By Ravindra on 30 Sep 2014 for PRIMEPOS-2047
            oDS.Tables[0].Columns.Add("AvgPrice", typeof(System.Decimal));
            oDS.Tables[0].Columns.Add("OnSalePrice", typeof(System.Decimal));
            oDS.Tables[0].Columns.Add("DesireQuantity", typeof(System.String)); //Added By Shitaljit on 10 April 2013 for PRIMEPOS-776
            #region Sprint-21 - 09-Sep-2015 JY Added sale price related objects
            oDS.Tables[0].Columns.Add("isOnSale", typeof(System.Boolean));
            oDS.Tables[0].Columns.Add("SaleLimitQty", typeof(System.Int32));
            oDS.Tables[0].Columns.Add("SaleStartDate", typeof(System.DateTime));
            oDS.Tables[0].Columns.Add("SaleEndDate", typeof(System.DateTime));
            #endregion
            oDS.Tables[0].Columns.Add("VendorName", typeof(System.String)); //PRIMEPOS-2758 11-Nov-2019 JY Added
        }

        private byte[] GetImageData(String fileName)
        {
            //'Method to load an image from disk and return it as a bytestream
            System.IO.FileStream fs = new System.IO.FileStream(fileName, System.IO.FileMode.Open, System.IO.FileAccess.Read);
            System.IO.BinaryReader br = new System.IO.BinaryReader(fs);
            return (br.ReadBytes(Convert.ToInt32(br.BaseStream.Length)));
        }

        //PreferredVendor->LastVendor->DefaultVendor->OriginalVendor.
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
                if (lastOrderVendor != string.Empty && !isItemExistForPrefVend)
                {
                    //vendorRows = vendorData.Vendor.Select("VendorCode ='" + preferredVendor + "'");
                    //vendrorID = vendorRows[0][clsPOSDBConstants.POHeader_Fld_VendorID].ToString();
                    POS_Core.BusinessRules.Vendor tempVendor = new POS_Core.BusinessRules.Vendor();
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
                    DataRow[] defaultVendorRows = vendorData.Vendor.Select("VendorCode ='" + Configuration.CPrimeEDISetting.DefaultVendor + "'");   //PRIMEPOS-3167 07-Nov-2022 JY Modified
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
                if (!isItemExistForDefVend && !isItemExistForLastVend && !isItemExistForPrefVend)
                {
                    itemVendorData = itemVendor.PopulateList(" AND " + clsPOSDBConstants.ItemVendor_tbl + ".ItemID like '" + itemCode + "%'");
                }
            }
            catch (Exception ex)
            {
                clsUIHelper.ShowErrorMsg(ex.Message);
            }
            return itemVendorData;
        }
        ItemVendorData itemVendorData = null;
        private void Preview(bool PrintId)
        {
            try
            {
                itemVendorData = new ItemVendorData();
                rptItemLabel oRpt = new rptItemLabel();
                int rowIndex = 0;
                DataSet rptDS = new DataSet();

                setNew();
                rptDS.Tables.Add();

                /*rptDS.Tables[0].Columns.Add("ItemID");
                rptDS.Tables[0].Columns.Add("Description");
                rptDS.Tables[0].Columns.Add("Picture", System.Type.GetType("System.Byte[]"));
                rptDS.Tables[0].Columns.Add("SellingPrice");*/

                rptDS.Tables[0].Columns.Add("ItemID");
                rptDS.Tables[0].Columns.Add("Description");
                rptDS.Tables[0].Columns.Add("Picture", System.Type.GetType("System.Byte[]"));
                rptDS.Tables[0].Columns.Add("SellingPrice");
                rptDS.Tables[0].Columns.Add("ProductCode");
                rptDS.Tables[0].Columns.Add("Location");
                rptDS.Tables[0].Columns.Add("VendorCode");
                rptDS.Tables[0].Columns.Add("VendorID");
                rptDS.Tables[0].Columns.Add("PCKSIZE");
                rptDS.Tables[0].Columns.Add("PCKQTY");
                rptDS.Tables[0].Columns.Add("PCKUNIT");
                //Added By Amit Date 7 july 2011
                rptDS.Tables[0].Columns.Add("VendorItemID");
                rptDS.Tables[0].Columns.Add("VendorItemIDBarcode", System.Type.GetType("System.Byte[]"));

                ////Added By Ravindra on 30 Sep 2014 for PRIMEPOS-2047
                rptDS.Tables[0].Columns.Add("ManufacturerName");
                rptDS.Tables[0].Columns.Add("AvgPrice");
                rptDS.Tables[0].Columns.Add("OnSalePrice");
                rptDS.Tables[0].Columns.Add("SKUCode");
                ////Till here added By Ravindra on 30 Sep 2014 for PRIMEPOS-2047
                rptDS.Tables[0].Columns.Add("DesireQuantity");//Added By Shitaljit on 10 April 2013 for PRIMEPOS-776
                #region Sprint-21 - 08-Sep-2015 JY Added sale price related objects
                rptDS.Tables[0].Columns.Add("isOnSale");
                rptDS.Tables[0].Columns.Add("SaleLimitQty");
                rptDS.Tables[0].Columns.Add("SaleStartDate");
                rptDS.Tables[0].Columns.Add("SaleEndDate");
                #endregion
                rptDS.Tables[0].Columns.Add("VendorName");  //PRIMEPOS-2758 11-Nov-2019 JY Added
                //FKEdit();

                rowIndex = 0;
                if (ds != null)
                {
                    if (ds.Tables.Count != 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            foreach (DataRow oRow in ds.Tables[0].Rows)
                            {
                                if (Convert.ToBoolean(this.grdSearch.Rows[rowIndex].Cells["Check Items"].Value) == true)
                                {
                                    itemVendorData = GetTargetedVenorInfo(oRow["Item ID"].ToString());
                                    DataSet itempacksDS = GetPckData(oRow["Item ID"].ToString());
                                    {
                                        if (CmbPriceType.SelectedIndex == 0)
                                        {
                                            if (itemVendorData != null && itemVendorData.Tables.Count > 0 && itemVendorData.ItemVendor.Rows.Count > 0)
                                            {
                                                if (itempacksDS != null)
                                                {
                                                    // Following if is added by shitaljit(QuicSolv) 0n 26 August 2011
                                                    if (oRow["VendorItemID"].ToString() != "")
                                                    {
                                                        //oDS.Tables[0].Rows.Add(new object[] { oRow["Item ID"], oRow["Item Name"], Convert.ToInt32(this.grdSearch.Rows[rowIndex].Cells["No. Of Labels"].Value), Convert.ToInt32(this.grdSearch.Rows[rowIndex].Cells["No. Of Sheet"].Value), oRow["Selling Price"], oRow["ProductCode"], oRow["Location"], itemVendorData.ItemVendor.Rows[0]["VendorCode"].ToString(), itemVendorData.ItemVendor.Rows[0]["VendorID"].ToString(), itempacksDS.Tables[0].Rows[0]["PCKSIZE"].ToString(), itempacksDS.Tables[0].Rows[0]["PCKQTY"].ToString(), itempacksDS.Tables[0].Rows[0]["PCKUNIT"].ToString(), oRow["VendorItemID"] });
                                                        //oDS.Tables[0].Rows.Add(new object[] { oRow["Item ID"], oRow["Item Name"], Convert.ToInt32(this.grdSearch.Rows[rowIndex].Cells["No. Of Labels"].Value), Convert.ToInt32(this.grdSearch.Rows[rowIndex].Cells["No. Of Sheet"].Value), oRow["Selling Price"], oRow["ProductCode"], oRow["Location"], itemVendorData.ItemVendor.Rows[0]["VendorCode"].ToString(), itemVendorData.ItemVendor.Rows[0]["VendorID"].ToString(), itempacksDS.Tables[0].Rows[0]["PCKSIZE"].ToString(), itempacksDS.Tables[0].Rows[0]["PCKQTY"].ToString(), itempacksDS.Tables[0].Rows[0]["PCKUNIT"].ToString(), oRow["VendorItemID"], oRow["ManufacturerName"], Configuration.convertNullToDecimal(oRow["AvgPrice"]), Configuration.convertNullToDecimal(oRow["OnSalePrice"].ToString()), "", Configuration.convertNullToBoolean(oRow["isOnSale"]), Configuration.convertNullToInt(oRow["SaleLimitQty"]), oRow["SaleStartDate"], oRow["SaleEndDate"] });    //Sprint-21 - 09-Sep-2015 JY Added sale price related objects
                                                        oDS.Tables[0].Rows.Add(new object[] { oRow["Item ID"], oRow["Item Name"], Convert.ToInt32(this.grdSearch.Rows[rowIndex].Cells["No. Of Labels"].Value), Convert.ToInt32(this.grdSearch.Rows[rowIndex].Cells["No. Of Sheet"].Value), oRow["Cost Price"], oRow["ProductCode"], oRow["Location"], itemVendorData.ItemVendor.Rows[0]["VendorCode"].ToString(), itemVendorData.ItemVendor.Rows[0]["VendorID"].ToString(), itempacksDS.Tables[0].Rows[0]["PCKSIZE"].ToString(), itempacksDS.Tables[0].Rows[0]["PCKQTY"].ToString(), itempacksDS.Tables[0].Rows[0]["PCKUNIT"].ToString(), oRow["VendorItemID"], oRow["ManufacturerName"], Configuration.convertNullToDecimal(oRow["AvgPrice"]), Configuration.convertNullToDecimal(oRow["OnSalePrice"].ToString()), "", Configuration.convertNullToBoolean(oRow["isOnSale"]), Configuration.convertNullToInt(oRow["SaleLimitQty"]), oRow["SaleStartDate"], oRow["SaleEndDate"], Configuration.convertNullToString(oRow["Vendor Name"]) });    //PRIMEPOS-2758 11-Nov-2019 JY Added Vendor Name
                                                    }
                                                    //
                                                    else
                                                    {
                                                        //oDS.Tables[0].Rows.Add(new object[] { oRow["Item ID"], oRow["Item Name"], Convert.ToInt32(this.grdSearch.Rows[rowIndex].Cells["No. Of Labels"].Value), Convert.ToInt32(this.grdSearch.Rows[rowIndex].Cells["No. Of Sheet"].Value), oRow["Cost Price"], oRow["ProductCode"], oRow["Location"], itemVendorData.ItemVendor.Rows[0]["VendorCode"].ToString(), itemVendorData.ItemVendor.Rows[0]["VendorID"].ToString(), itempacksDS.Tables[0].Rows[0]["PCKSIZE"].ToString(), itempacksDS.Tables[0].Rows[0]["PCKQTY"].ToString(), itempacksDS.Tables[0].Rows[0]["PCKUNIT"].ToString() });
                                                        //oDS.Tables[0].Rows.Add(new object[] { oRow["Item ID"], oRow["Item Name"], Convert.ToInt32(this.grdSearch.Rows[rowIndex].Cells["No. Of Labels"].Value), Convert.ToInt32(this.grdSearch.Rows[rowIndex].Cells["No. Of Sheet"].Value), oRow["Cost Price"], oRow["ProductCode"], oRow["Location"], itemVendorData.ItemVendor.Rows[0]["VendorCode"].ToString(), itemVendorData.ItemVendor.Rows[0]["VendorID"].ToString(), itempacksDS.Tables[0].Rows[0]["PCKSIZE"].ToString(), itempacksDS.Tables[0].Rows[0]["PCKQTY"].ToString(), itempacksDS.Tables[0].Rows[0]["PCKUNIT"].ToString(), oRow["ManufacturerName"], Configuration.convertNullToDecimal(oRow["AvgPrice"]), Configuration.convertNullToDecimal(oRow["OnSalePrice"].ToString())});
                                                        //oDS.Tables[0].Rows.Add(new object[] { oRow["Item ID"], oRow["Item Name"], Convert.ToInt32(this.grdSearch.Rows[rowIndex].Cells["No. Of Labels"].Value), Convert.ToInt32(this.grdSearch.Rows[rowIndex].Cells["No. Of Sheet"].Value), oRow["Cost Price"], oRow["ProductCode"], oRow["Location"], itemVendorData.ItemVendor.Rows[0]["VendorCode"].ToString(), itemVendorData.ItemVendor.Rows[0]["VendorID"].ToString(), itempacksDS.Tables[0].Rows[0]["PCKSIZE"].ToString(), itempacksDS.Tables[0].Rows[0]["PCKQTY"].ToString(), itempacksDS.Tables[0].Rows[0]["PCKUNIT"].ToString(), string.Empty, oRow["ManufacturerName"], Configuration.convertNullToDecimal(oRow["AvgPrice"]), Configuration.convertNullToDecimal(oRow["OnSalePrice"].ToString()), "", Configuration.convertNullToBoolean(oRow["isOnSale"]), Configuration.convertNullToInt(oRow["SaleLimitQty"]), oRow["SaleStartDate"], oRow["SaleEndDate"] });  //Sprint-21 - 09-Sep-2015 JY Added sale price related objects
                                                        oDS.Tables[0].Rows.Add(new object[] { oRow["Item ID"], oRow["Item Name"], Convert.ToInt32(this.grdSearch.Rows[rowIndex].Cells["No. Of Labels"].Value), Convert.ToInt32(this.grdSearch.Rows[rowIndex].Cells["No. Of Sheet"].Value), oRow["Cost Price"], oRow["ProductCode"], oRow["Location"], itemVendorData.ItemVendor.Rows[0]["VendorCode"].ToString(), itemVendorData.ItemVendor.Rows[0]["VendorID"].ToString(), itempacksDS.Tables[0].Rows[0]["PCKSIZE"].ToString(), itempacksDS.Tables[0].Rows[0]["PCKQTY"].ToString(), itempacksDS.Tables[0].Rows[0]["PCKUNIT"].ToString(), string.Empty, oRow["ManufacturerName"], Configuration.convertNullToDecimal(oRow["AvgPrice"]), Configuration.convertNullToDecimal(oRow["OnSalePrice"].ToString()), "", Configuration.convertNullToBoolean(oRow["isOnSale"]), Configuration.convertNullToInt(oRow["SaleLimitQty"]), oRow["SaleStartDate"], oRow["SaleEndDate"], Configuration.convertNullToString(oRow["Vendor Name"]) });    //PRIMEPOS-2758 11-Nov-2019 JY Added Vendor Name
                                                    }
                                                }
                                                else
                                                {
                                                    // Following if is added by shitaljit(QuicSolv) 0n 26 August 2011
                                                    if (oRow["VendorItemID"].ToString() != "")
                                                    {
                                                        //oDS.Tables[0].Rows.Add(new object[] { oRow["Item ID"], oRow["Item Name"], Convert.ToInt32(this.grdSearch.Rows[rowIndex].Cells["No. Of Labels"].Value), Convert.ToInt32(this.grdSearch.Rows[rowIndex].Cells["No. Of Sheet"].Value), oRow["Cost Price"], oRow["ProductCode"], oRow["Location"], itemVendorData.ItemVendor.Rows[0]["VendorCode"].ToString(), itemVendorData.ItemVendor.Rows[0]["VendorID"].ToString(), string.Empty, string.Empty, string.Empty, oRow["VendorItemID"] });
                                                        //oDS.Tables[0].Rows.Add(new object[] { oRow["Item ID"], oRow["Item Name"], Convert.ToInt32(this.grdSearch.Rows[rowIndex].Cells["No. Of Labels"].Value), Convert.ToInt32(this.grdSearch.Rows[rowIndex].Cells["No. Of Sheet"].Value), oRow["Cost Price"], oRow["ProductCode"], oRow["Location"], itemVendorData.ItemVendor.Rows[0]["VendorCode"].ToString(), itemVendorData.ItemVendor.Rows[0]["VendorID"].ToString(), string.Empty, string.Empty, string.Empty, oRow["VendorItemID"], oRow["ManufacturerName"], oRow["AvgPrice"], oRow["OnSalePrice"], "", Configuration.convertNullToBoolean(oRow["isOnSale"]), Configuration.convertNullToInt(oRow["SaleLimitQty"]), oRow["SaleStartDate"], oRow["SaleEndDate"] }); //Sprint-21 - 09-Sep-2015 JY Added sale price related objects
                                                        oDS.Tables[0].Rows.Add(new object[] { oRow["Item ID"], oRow["Item Name"], Convert.ToInt32(this.grdSearch.Rows[rowIndex].Cells["No. Of Labels"].Value), Convert.ToInt32(this.grdSearch.Rows[rowIndex].Cells["No. Of Sheet"].Value), oRow["Cost Price"], oRow["ProductCode"], oRow["Location"], itemVendorData.ItemVendor.Rows[0]["VendorCode"].ToString(), itemVendorData.ItemVendor.Rows[0]["VendorID"].ToString(), string.Empty, string.Empty, string.Empty, oRow["VendorItemID"], oRow["ManufacturerName"], oRow["AvgPrice"], oRow["OnSalePrice"], "", Configuration.convertNullToBoolean(oRow["isOnSale"]), Configuration.convertNullToInt(oRow["SaleLimitQty"]), oRow["SaleStartDate"], oRow["SaleEndDate"], Configuration.convertNullToString(oRow["Vendor Name"]) });    //PRIMEPOS-2758 11-Nov-2019 JY Added Vendor Name
                                                    }
                                                    else
                                                    {
                                                        //oDS.Tables[0].Rows.Add(new object[] { oRow["Item ID"], oRow["Item Name"], Convert.ToInt32(this.grdSearch.Rows[rowIndex].Cells["No. Of Labels"].Value), Convert.ToInt32(this.grdSearch.Rows[rowIndex].Cells["No. Of Sheet"].Value), oRow["Cost Price"], oRow["ProductCode"], oRow["Location"], itemVendorData.ItemVendor.Rows[0]["VendorCode"].ToString(), itemVendorData.ItemVendor.Rows[0]["VendorID"].ToString(), string.Empty, string.Empty, string.Empty });
                                                        //oDS.Tables[0].Rows.Add(new object[] { oRow["Item ID"], oRow["Item Name"], Convert.ToInt32(this.grdSearch.Rows[rowIndex].Cells["No. Of Labels"].Value), Convert.ToInt32(this.grdSearch.Rows[rowIndex].Cells["No. Of Sheet"].Value), oRow["Cost Price"], oRow["ProductCode"], oRow["Location"], itemVendorData.ItemVendor.Rows[0]["VendorCode"].ToString(), itemVendorData.ItemVendor.Rows[0]["VendorID"].ToString(), string.Empty, string.Empty, string.Empty, oRow["ManufacturerName"], oRow["AvgPrice"], oRow["OnSalePrice"]});
                                                        //oDS.Tables[0].Rows.Add(new object[] { oRow["Item ID"], oRow["Item Name"], Convert.ToInt32(this.grdSearch.Rows[rowIndex].Cells["No. Of Labels"].Value), Convert.ToInt32(this.grdSearch.Rows[rowIndex].Cells["No. Of Sheet"].Value), oRow["Cost Price"], oRow["ProductCode"], oRow["Location"], itemVendorData.ItemVendor.Rows[0]["VendorCode"].ToString(), itemVendorData.ItemVendor.Rows[0]["VendorID"].ToString(), string.Empty, string.Empty, string.Empty, string.Empty, oRow["ManufacturerName"], oRow["AvgPrice"], oRow["OnSalePrice"], "", Configuration.convertNullToBoolean(oRow["isOnSale"]), Configuration.convertNullToInt(oRow["SaleLimitQty"]), oRow["SaleStartDate"], oRow["SaleEndDate"] }); //Sprint-21 - 09-Sep-2015 JY Added sale price related objects
                                                        oDS.Tables[0].Rows.Add(new object[] { oRow["Item ID"], oRow["Item Name"], Convert.ToInt32(this.grdSearch.Rows[rowIndex].Cells["No. Of Labels"].Value), Convert.ToInt32(this.grdSearch.Rows[rowIndex].Cells["No. Of Sheet"].Value), oRow["Cost Price"], oRow["ProductCode"], oRow["Location"], itemVendorData.ItemVendor.Rows[0]["VendorCode"].ToString(), itemVendorData.ItemVendor.Rows[0]["VendorID"].ToString(), string.Empty, string.Empty, string.Empty, string.Empty, oRow["ManufacturerName"], oRow["AvgPrice"], oRow["OnSalePrice"], "", Configuration.convertNullToBoolean(oRow["isOnSale"]), Configuration.convertNullToInt(oRow["SaleLimitQty"]), oRow["SaleStartDate"], oRow["SaleEndDate"], Configuration.convertNullToString(oRow["Vendor Name"]) });    //PRIMEPOS-2758 11-Nov-2019 JY Added Vendor Name
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                // Following if is added by shitaljit(QuicSolv) 0n 26 August 2011
                                                if (oRow["VendorItemID"].ToString() != "")
                                                {
                                                    //oDS.Tables[0].Rows.Add(new object[] { oRow["Item ID"], oRow["Item Name"], Convert.ToInt32(this.grdSearch.Rows[rowIndex].Cells["No. Of Labels"].Value), Convert.ToInt32(this.grdSearch.Rows[rowIndex].Cells["No. Of Sheet"].Value), oRow["Cost Price"], oRow["ProductCode"], oRow["Location"], string.Empty, string.Empty, string.Empty, string.Empty, oRow["VendorItemID"], oRow["ManufacturerName"], oRow["AvgPrice"], oRow["OnSalePrice"], "", Configuration.convertNullToBoolean(oRow["isOnSale"]), Configuration.convertNullToInt(oRow["SaleLimitQty"]), oRow["SaleStartDate"], oRow["SaleEndDate"] }); //Sprint-21 - 09-Sep-2015 JY Added sale price related objects
                                                    //oDS.Tables[0].Rows.Add(new object[] { oRow["Item ID"], oRow["Item Name"], Convert.ToInt32(this.grdSearch.Rows[rowIndex].Cells["No. Of Labels"].Value), Convert.ToInt32(this.grdSearch.Rows[rowIndex].Cells["No. Of Sheet"].Value), oRow["Cost Price"], oRow["ProductCode"], oRow["Location"], string.Empty, string.Empty, string.Empty, string.Empty, oRow["VendorItemID"], oRow["ManufacturerName"], oRow["AvgPrice"], oRow["OnSalePrice"], "", Configuration.convertNullToBoolean(oRow["isOnSale"]), Configuration.convertNullToInt(oRow["SaleLimitQty"]), oRow["SaleStartDate"], oRow["SaleEndDate"], Configuration.convertNullToString(oRow["Vendor Name"]) });    //PRIMEPOS-2758 11-Nov-2019 JY Added Vendor Name
                                                    oDS.Tables[0].Rows.Add(new object[] { oRow["Item ID"], oRow["Item Name"], Convert.ToInt32(this.grdSearch.Rows[rowIndex].Cells["No. Of Labels"].Value), Convert.ToInt32(this.grdSearch.Rows[rowIndex].Cells["No. Of Sheet"].Value), oRow["Cost Price"], oRow["ProductCode"], oRow["Location"], string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, oRow["VendorItemID"], oRow["ManufacturerName"], oRow["AvgPrice"], oRow["OnSalePrice"], "", Configuration.convertNullToBoolean(oRow["isOnSale"]), Configuration.convertNullToInt(oRow["SaleLimitQty"]), oRow["SaleStartDate"], oRow["SaleEndDate"], Configuration.convertNullToString(oRow["Vendor Name"]) });    //PRIMEPOS-2758 11-Nov-2019 JY Added Vendor Name   //PRIMEPOS-2909 16-Oct-2020 JY modified
                                                }
                                                else
                                                {
                                                    //oDS.Tables[0].Rows.Add(new object[] { oRow["Item ID"], oRow["Item Name"], Convert.ToInt32(this.grdSearch.Rows[rowIndex].Cells["No. Of Labels"].Value), Convert.ToInt32(this.grdSearch.Rows[rowIndex].Cells["No. Of Sheet"].Value), oRow["Cost Price"], oRow["ProductCode"], oRow["Location"], string.Empty, string.Empty, string.Empty, string.Empty, oRow["ManufacturerName"], oRow["AvgPrice"], oRow["OnSalePrice"] });
                                                    //oDS.Tables[0].Rows.Add(new object[] { oRow["Item ID"], oRow["Item Name"], Convert.ToInt32(this.grdSearch.Rows[rowIndex].Cells["No. Of Labels"].Value), Convert.ToInt32(this.grdSearch.Rows[rowIndex].Cells["No. Of Sheet"].Value), oRow["Cost Price"], oRow["ProductCode"], oRow["Location"], string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, oRow["ManufacturerName"], oRow["AvgPrice"], oRow["OnSalePrice"], "", Configuration.convertNullToBoolean(oRow["isOnSale"]), Configuration.convertNullToInt(oRow["SaleLimitQty"]), oRow["SaleStartDate"], oRow["SaleEndDate"] }); //Sprint-21 - 09-Sep-2015 JY Added sale price related objects
                                                    //oDS.Tables[0].Rows.Add(new object[] { oRow["Item ID"], oRow["Item Name"], Convert.ToInt32(this.grdSearch.Rows[rowIndex].Cells["No. Of Labels"].Value), Convert.ToInt32(this.grdSearch.Rows[rowIndex].Cells["No. Of Sheet"].Value), oRow["Cost Price"], oRow["ProductCode"], oRow["Location"], string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, oRow["ManufacturerName"], oRow["AvgPrice"], oRow["OnSalePrice"], "", Configuration.convertNullToBoolean(oRow["isOnSale"]), Configuration.convertNullToInt(oRow["SaleLimitQty"]), oRow["SaleStartDate"], oRow["SaleEndDate"], Configuration.convertNullToString(oRow["Vendor Name"]) });    //PRIMEPOS-2758 11-Nov-2019 JY Added Vendor Name
                                                    oDS.Tables[0].Rows.Add(new object[] { oRow["Item ID"], oRow["Item Name"], Convert.ToInt32(this.grdSearch.Rows[rowIndex].Cells["No. Of Labels"].Value), Convert.ToInt32(this.grdSearch.Rows[rowIndex].Cells["No. Of Sheet"].Value), oRow["Cost Price"], oRow["ProductCode"], oRow["Location"], string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, oRow["ManufacturerName"], oRow["AvgPrice"], oRow["OnSalePrice"], "", Configuration.convertNullToBoolean(oRow["isOnSale"]), Configuration.convertNullToInt(oRow["SaleLimitQty"]), oRow["SaleStartDate"], oRow["SaleEndDate"], Configuration.convertNullToString(oRow["Vendor Name"]) });    //PRIMEPOS-2758 11-Nov-2019 JY Added Vendor Name //PRIMEPOS-2909 16-Oct-2020 JY modified
                                                }
                                            }
                                        }
                                        else
                                        {
                                            if (itemVendorData != null && itemVendorData.Tables.Count > 0 && itemVendorData.ItemVendor.Rows.Count > 0)
                                            {
                                                if (itempacksDS != null)
                                                {
                                                    // Following if is added by shitaljit(QuicSolv) 0n 26 August 2011
                                                    //oRow["VendorItemID"]
                                                    if (oRow["VendorItemID"].ToString() != "")
                                                    {
                                                        //oDS.Tables[0].Rows.Add(new object[] { oRow["Item ID"], oRow["Item Name"], Convert.ToInt32(this.grdSearch.Rows[rowIndex].Cells["No. Of Labels"].Value), Convert.ToInt32(this.grdSearch.Rows[rowIndex].Cells["No. Of Sheet"].Value), oRow["Selling Price"], oRow["ProductCode"], oRow["Location"], itemVendorData.ItemVendor.Rows[0]["VendorCode"].ToString(), itemVendorData.ItemVendor.Rows[0]["VendorID"].ToString(), itempacksDS.Tables[0].Rows[0]["PCKSIZE"].ToString(), itempacksDS.Tables[0].Rows[0]["PCKQTY"].ToString(), itempacksDS.Tables[0].Rows[0]["PCKUNIT"].ToString(), oRow["VendorItemID"], oRow["ManufacturerName"], Configuration.convertNullToDecimal(oRow["AvgPrice"]), Configuration.convertNullToDecimal(oRow["OnSalePrice"].ToString()), "", Configuration.convertNullToBoolean(oRow["isOnSale"]), Configuration.convertNullToInt(oRow["SaleLimitQty"]), oRow["SaleStartDate"], oRow["SaleEndDate"] });    //Sprint-21 - 09-Sep-2015 JY Added sale price related objects
                                                        oDS.Tables[0].Rows.Add(new object[] { oRow["Item ID"], oRow["Item Name"], Convert.ToInt32(this.grdSearch.Rows[rowIndex].Cells["No. Of Labels"].Value), Convert.ToInt32(this.grdSearch.Rows[rowIndex].Cells["No. Of Sheet"].Value), oRow["Selling Price"], oRow["ProductCode"], oRow["Location"], itemVendorData.ItemVendor.Rows[0]["VendorCode"].ToString(), itemVendorData.ItemVendor.Rows[0]["VendorID"].ToString(), itempacksDS.Tables[0].Rows[0]["PCKSIZE"].ToString(), itempacksDS.Tables[0].Rows[0]["PCKQTY"].ToString(), itempacksDS.Tables[0].Rows[0]["PCKUNIT"].ToString(), oRow["VendorItemID"], oRow["ManufacturerName"], Configuration.convertNullToDecimal(oRow["AvgPrice"]), Configuration.convertNullToDecimal(oRow["OnSalePrice"].ToString()), "", Configuration.convertNullToBoolean(oRow["isOnSale"]), Configuration.convertNullToInt(oRow["SaleLimitQty"]), oRow["SaleStartDate"], oRow["SaleEndDate"], Configuration.convertNullToString(oRow["Vendor Name"]) });    //PRIMEPOS-2758 11-Nov-2019 JY Added Vendor Name
                                                    }
                                                    else
                                                    {
                                                        //oDS.Tables[0].Rows.Add(new object[] { oRow["Item ID"], oRow["Item Name"], Convert.ToInt32(this.grdSearch.Rows[rowIndex].Cells["No. Of Labels"].Value), Convert.ToInt32(this.grdSearch.Rows[rowIndex].Cells["No. Of Sheet"].Value), oRow["Selling Price"], oRow["ProductCode"], oRow["Location"], itemVendorData.ItemVendor.Rows[0]["VendorCode"].ToString(), itemVendorData.ItemVendor.Rows[0]["VendorID"].ToString(), itempacksDS.Tables[0].Rows[0]["PCKSIZE"].ToString(), itempacksDS.Tables[0].Rows[0]["PCKQTY"].ToString(), itempacksDS.Tables[0].Rows[0]["PCKUNIT"].ToString(), oRow["ManufacturerName"], Configuration.convertNullToDecimal(oRow["AvgPrice"]), Configuration.convertNullToDecimal(oRow["OnSalePrice"].ToString())});
                                                        //oDS.Tables[0].Rows.Add(new object[] { oRow["Item ID"], oRow["Item Name"], Convert.ToInt32(this.grdSearch.Rows[rowIndex].Cells["No. Of Labels"].Value), Convert.ToInt32(this.grdSearch.Rows[rowIndex].Cells["No. Of Sheet"].Value), oRow["Selling Price"], oRow["ProductCode"], oRow["Location"], itemVendorData.ItemVendor.Rows[0]["VendorCode"].ToString(), itemVendorData.ItemVendor.Rows[0]["VendorID"].ToString(), itempacksDS.Tables[0].Rows[0]["PCKSIZE"].ToString(), itempacksDS.Tables[0].Rows[0]["PCKQTY"].ToString(), itempacksDS.Tables[0].Rows[0]["PCKUNIT"].ToString(), string.Empty, oRow["ManufacturerName"], Configuration.convertNullToDecimal(oRow["AvgPrice"]), Configuration.convertNullToDecimal(oRow["OnSalePrice"].ToString()), "", Configuration.convertNullToBoolean(oRow["isOnSale"]), Configuration.convertNullToInt(oRow["SaleLimitQty"]), oRow["SaleStartDate"], oRow["SaleEndDate"] }); //Sprint-21 - 09-Sep-2015 JY Added sale price related objects
                                                        oDS.Tables[0].Rows.Add(new object[] { oRow["Item ID"], oRow["Item Name"], Convert.ToInt32(this.grdSearch.Rows[rowIndex].Cells["No. Of Labels"].Value), Convert.ToInt32(this.grdSearch.Rows[rowIndex].Cells["No. Of Sheet"].Value), oRow["Selling Price"], oRow["ProductCode"], oRow["Location"], itemVendorData.ItemVendor.Rows[0]["VendorCode"].ToString(), itemVendorData.ItemVendor.Rows[0]["VendorID"].ToString(), itempacksDS.Tables[0].Rows[0]["PCKSIZE"].ToString(), itempacksDS.Tables[0].Rows[0]["PCKQTY"].ToString(), itempacksDS.Tables[0].Rows[0]["PCKUNIT"].ToString(), string.Empty, oRow["ManufacturerName"], Configuration.convertNullToDecimal(oRow["AvgPrice"]), Configuration.convertNullToDecimal(oRow["OnSalePrice"].ToString()), "", Configuration.convertNullToBoolean(oRow["isOnSale"]), Configuration.convertNullToInt(oRow["SaleLimitQty"]), oRow["SaleStartDate"], oRow["SaleEndDate"], Configuration.convertNullToString(oRow["Vendor Name"]) });    //PRIMEPOS-2758 11-Nov-2019 JY Added Vendor Name
                                                    }
                                                }
                                                else
                                                {
                                                    // Following if is added by shitaljit(QuicSolv) 0n 26 August 2011
                                                    if (oRow["VendorItemID"].ToString() != "")
                                                    {
                                                        //oDS.Tables[0].Rows.Add(new object[] { oRow["Item ID"], oRow["Item Name"], Convert.ToInt32(this.grdSearch.Rows[rowIndex].Cells["No. Of Labels"].Value), Convert.ToInt32(this.grdSearch.Rows[rowIndex].Cells["No. Of Sheet"].Value), oRow["Selling Price"], oRow["ProductCode"], oRow["Location"], itemVendorData.ItemVendor.Rows[0]["VendorCode"].ToString(), itemVendorData.ItemVendor.Rows[0]["VendorID"].ToString(), string.Empty, string.Empty, string.Empty, oRow["VendorItemID"], oRow["ManufacturerName"], Configuration.convertNullToDecimal(oRow["AvgPrice"]), Configuration.convertNullToDecimal(oRow["OnSalePrice"].ToString()), "", Configuration.convertNullToBoolean(oRow["isOnSale"]), Configuration.convertNullToInt(oRow["SaleLimitQty"]), oRow["SaleStartDate"], oRow["SaleEndDate"] });   //Sprint-21 - 09-Sep-2015 JY Added sale price related objects
                                                        oDS.Tables[0].Rows.Add(new object[] { oRow["Item ID"], oRow["Item Name"], Convert.ToInt32(this.grdSearch.Rows[rowIndex].Cells["No. Of Labels"].Value), Convert.ToInt32(this.grdSearch.Rows[rowIndex].Cells["No. Of Sheet"].Value), oRow["Selling Price"], oRow["ProductCode"], oRow["Location"], itemVendorData.ItemVendor.Rows[0]["VendorCode"].ToString(), itemVendorData.ItemVendor.Rows[0]["VendorID"].ToString(), string.Empty, string.Empty, string.Empty, oRow["VendorItemID"], oRow["ManufacturerName"], Configuration.convertNullToDecimal(oRow["AvgPrice"]), Configuration.convertNullToDecimal(oRow["OnSalePrice"].ToString()), "", Configuration.convertNullToBoolean(oRow["isOnSale"]), Configuration.convertNullToInt(oRow["SaleLimitQty"]), oRow["SaleStartDate"], oRow["SaleEndDate"], Configuration.convertNullToString(oRow["Vendor Name"]) });    //PRIMEPOS-2758 11-Nov-2019 JY Added Vendor Name
                                                    }
                                                    else
                                                    {
                                                        //oDS.Tables[0].Rows.Add(new object[] { oRow["Item ID"], oRow["Item Name"], Convert.ToInt32(this.grdSearch.Rows[rowIndex].Cells["No. Of Labels"].Value), Convert.ToInt32(this.grdSearch.Rows[rowIndex].Cells["No. Of Sheet"].Value), oRow["Selling Price"], oRow["ProductCode"], oRow["Location"], itemVendorData.ItemVendor.Rows[0]["VendorCode"].ToString(), itemVendorData.ItemVendor.Rows[0]["VendorID"].ToString(), string.Empty, string.Empty, string.Empty, oRow["ManufacturerName"], Configuration.convertNullToDecimal(oRow["AvgPrice"]), Configuration.convertNullToDecimal(oRow["OnSalePrice"].ToString()) });
                                                        //oDS.Tables[0].Rows.Add(new object[] { oRow["Item ID"], oRow["Item Name"], Convert.ToInt32(this.grdSearch.Rows[rowIndex].Cells["No. Of Labels"].Value), Convert.ToInt32(this.grdSearch.Rows[rowIndex].Cells["No. Of Sheet"].Value), oRow["Selling Price"], oRow["ProductCode"], oRow["Location"], itemVendorData.ItemVendor.Rows[0]["VendorCode"].ToString(), itemVendorData.ItemVendor.Rows[0]["VendorID"].ToString(), string.Empty, string.Empty, string.Empty, string.Empty, oRow["ManufacturerName"], Configuration.convertNullToDecimal(oRow["AvgPrice"]), Configuration.convertNullToDecimal(oRow["OnSalePrice"].ToString()), "", Configuration.convertNullToBoolean(oRow["isOnSale"]), Configuration.convertNullToInt(oRow["SaleLimitQty"]), oRow["SaleStartDate"], oRow["SaleEndDate"] }); //Sprint-21 - 09-Sep-2015 JY Added sale price related objects
                                                        oDS.Tables[0].Rows.Add(new object[] { oRow["Item ID"], oRow["Item Name"], Convert.ToInt32(this.grdSearch.Rows[rowIndex].Cells["No. Of Labels"].Value), Convert.ToInt32(this.grdSearch.Rows[rowIndex].Cells["No. Of Sheet"].Value), oRow["Selling Price"], oRow["ProductCode"], oRow["Location"], itemVendorData.ItemVendor.Rows[0]["VendorCode"].ToString(), itemVendorData.ItemVendor.Rows[0]["VendorID"].ToString(), string.Empty, string.Empty, string.Empty, string.Empty, oRow["ManufacturerName"], Configuration.convertNullToDecimal(oRow["AvgPrice"]), Configuration.convertNullToDecimal(oRow["OnSalePrice"].ToString()), "", Configuration.convertNullToBoolean(oRow["isOnSale"]), Configuration.convertNullToInt(oRow["SaleLimitQty"]), oRow["SaleStartDate"], oRow["SaleEndDate"], Configuration.convertNullToString(oRow["Vendor Name"]) });    //PRIMEPOS-2758 11-Nov-2019 JY Added Vendor Name
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                //oDS.Tables[0].Rows.Add(new object[] { oRow["Item ID"], oRow["Item Name"], Convert.ToInt32(this.grdSearch.Rows[rowIndex].Cells["No. Of Labels"].Value), Convert.ToInt32(this.grdSearch.Rows[rowIndex].Cells["No. Of Sheet"].Value), oRow["Selling Price"], oRow["ProductCode"], oRow["Location"], string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, oRow["ManufacturerName"], oRow["AvgPrice"], oRow["OnSalePrice"]});
                                                //oDS.Tables[0].Rows.Add(new object[] { oRow["Item ID"], oRow["Item Name"], Convert.ToInt32(this.grdSearch.Rows[rowIndex].Cells["No. Of Labels"].Value), Convert.ToInt32(this.grdSearch.Rows[rowIndex].Cells["No. Of Sheet"].Value), oRow["Selling Price"], oRow["ProductCode"], oRow["Location"], string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, oRow["ManufacturerName"], oRow["AvgPrice"], oRow["OnSalePrice"], string.Empty, Configuration.convertNullToBoolean(oRow["isOnSale"]), Configuration.convertNullToInt(oRow["SaleLimitQty"]), oRow["SaleStartDate"], oRow["SaleEndDate"] }); //Sprint-21 - 09-Sep-2015 JY Added sale price related objects
                                                oDS.Tables[0].Rows.Add(new object[] { oRow["Item ID"], oRow["Item Name"], Convert.ToInt32(this.grdSearch.Rows[rowIndex].Cells["No. Of Labels"].Value), Convert.ToInt32(this.grdSearch.Rows[rowIndex].Cells["No. Of Sheet"].Value), oRow["Selling Price"], oRow["ProductCode"], oRow["Location"], string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, oRow["ManufacturerName"], oRow["AvgPrice"], oRow["OnSalePrice"], string.Empty, Configuration.convertNullToBoolean(oRow["isOnSale"]), Configuration.convertNullToInt(oRow["SaleLimitQty"]), oRow["SaleStartDate"], oRow["SaleEndDate"], Configuration.convertNullToString(oRow["Vendor Name"]) });    //PRIMEPOS-2758 11-Nov-2019 JY Added Vendor Name
                                            }
                                        }
                                        // Following line is commented by shitaljit(QuicSolv) 0n 14 oct 2011
                                        //Int64 ID = Convert.ToInt64(this.grdSearch.Rows[rowIndex].Cells["Item ID"].Value);
                                    }
                                }
                                rowIndex++;
                            }

                            frmSearchMain ofrm = new frmSearchMain();
                            int imageNo = 1;//Added By shitaljit(QuicSolv) on 16 Jan 2012 , to avoid exception of duplicate file name while writing file
                            foreach (DataRow oRow in oDS.Tables[0].Rows)
                            {
                                try
                                {
                                    //Fix Multiple print issue 2/19/2015
                                    Configuration.PrintBarcode(oRow[0].ToString(), 0, 0, 20, 200, "CODE128", "H", System.IO.Path.GetTempPath() + "\\" + oRow["ItemID"].ToString() + imageNo.ToString() + ".bmp");

                                    //Added by Amit Date 8 july 2011
                                    // Following if is added by shitaljit(QuicSolv) 0n 26 August 2011
                                    if (oRow["VendorItemID"].ToString() != "")
                                    {
                                        Configuration.PrintBarcode(oRow["VendorItemID"].ToString(), 0, 0, 20, 200, "CODE128", "H", System.IO.Path.GetTempPath() + "\\" + oRow["vendorItemID"].ToString() + ".bmp");
                                    }
                                }
                                catch (Exception ex) { }

                                string itemName = oRow[1].ToString();
                                if (itemName.Length > 25)
                                {
                                    itemName = itemName.Substring(0, 25);
                                }
                                for (int i = 0; i < Convert.ToInt32(oRow[3].ToString()) * Configuration.LabelPerSheet; i++)
                                {
                                    // Following if is added by shitaljit(QuicSolv) 0n 26 August 2011
                                    if (oRow["VendorItemID"].ToString() != "")
                                    {
                                        //rptDS.Tables[0].Rows.Add(new object[] { oRow[0].ToString(), itemName, GetImageData(System.IO.Path.GetTempPath() + "\\" + oRow["ItemID"].ToString() + imageNo.ToString() + ".bmp"), Configuration.convertNullToDecimal(oRow["sellingprice"].ToString()).ToString("Price " + Configuration.CInfo.CurrencySymbol + "#####0.00"), oRow["ProductCode"], oRow["Location"], oRow["VendorCode"], oRow["VendorID"], oRow["PCKSIZE"], oRow["PCKQTY"], oRow["PCKUNIT"], oRow["VendorItemID"].ToString(), GetImageData(System.IO.Path.GetTempPath() + "\\" + oRow["VendorItemID"].ToString() + ".bmp"), oRow["ManufacturerName"].ToString(), oRow["AvgPrice"], oRow["OnSalePrice"], oRow["ProductCode"].ToString(), "", Configuration.convertNullToBoolean(oRow["isOnSale"]), Configuration.convertNullToInt(oRow["SaleLimitQty"]), oRow["SaleStartDate"], oRow["SaleEndDate"] });  //Sprint-21 - 09-Sep-2015 JY Added sale price related objects
                                        rptDS.Tables[0].Rows.Add(new object[] { oRow[0].ToString(), itemName, GetImageData(System.IO.Path.GetTempPath() + "\\" + oRow["ItemID"].ToString() + imageNo.ToString() + ".bmp"), Configuration.convertNullToDecimal(oRow["sellingprice"].ToString()).ToString("Price " + Configuration.CInfo.CurrencySymbol + "#####0.00"), oRow["ProductCode"], oRow["Location"], oRow["VendorCode"], oRow["VendorID"], oRow["PCKSIZE"], oRow["PCKQTY"], oRow["PCKUNIT"], oRow["VendorItemID"].ToString(), GetImageData(System.IO.Path.GetTempPath() + "\\" + oRow["VendorItemID"].ToString() + ".bmp"), oRow["ManufacturerName"].ToString(), oRow["AvgPrice"], oRow["OnSalePrice"], oRow["ProductCode"].ToString(), "", Configuration.convertNullToBoolean(oRow["isOnSale"]), Configuration.convertNullToInt(oRow["SaleLimitQty"]), oRow["SaleStartDate"], oRow["SaleEndDate"], Configuration.convertNullToString(oRow["VendorName"]) });    //PRIMEPOS-2758 11-Nov-2019 JY Added VendorName
                                    }
                                    else
                                    {
                                        //rptDS.Tables[0].Rows.Add(new object[] { oRow[0].ToString(), itemName, GetImageData(System.IO.Path.GetTempPath() + "\\" + oRow["ItemID"].ToString() + imageNo.ToString() + ".bmp"), Configuration.convertNullToDecimal(oRow["sellingprice"].ToString()).ToString("Price " + Configuration.CInfo.CurrencySymbol + "#####0.00"), oRow["ProductCode"], oRow["Location"], oRow["VendorCode"], oRow["VendorID"], oRow["PCKSIZE"], oRow["PCKQTY"], oRow["PCKUNIT"], null, null, oRow["ManufacturerName"].ToString(), oRow["AvgPrice"], oRow["OnSalePrice"], oRow["ProductCode"].ToString(), "", Configuration.convertNullToBoolean(oRow["isOnSale"]), Configuration.convertNullToInt(oRow["SaleLimitQty"]), oRow["SaleStartDate"], oRow["SaleEndDate"] }); //Sprint-21 - 09-Sep-2015 JY Added sale price related objects
                                        rptDS.Tables[0].Rows.Add(new object[] { oRow[0].ToString(), itemName, GetImageData(System.IO.Path.GetTempPath() + "\\" + oRow["ItemID"].ToString() + imageNo.ToString() + ".bmp"), Configuration.convertNullToDecimal(oRow["sellingprice"].ToString()).ToString("Price " + Configuration.CInfo.CurrencySymbol + "#####0.00"), oRow["ProductCode"], oRow["Location"], oRow["VendorCode"], oRow["VendorID"], oRow["PCKSIZE"], oRow["PCKQTY"], oRow["PCKUNIT"], null, null, oRow["ManufacturerName"].ToString(), oRow["AvgPrice"], oRow["OnSalePrice"], oRow["ProductCode"].ToString(), "", Configuration.convertNullToBoolean(oRow["isOnSale"]), Configuration.convertNullToInt(oRow["SaleLimitQty"]), oRow["SaleStartDate"], oRow["SaleEndDate"], Configuration.convertNullToString(oRow["VendorName"]) });  //PRIMEPOS-2758 11-Nov-2019 JY Added VendorName
                                    }

                                }
                                for (int i = 0; i < Convert.ToInt32(oRow[2].ToString()); i++)
                                {
                                    // Following if is added by shitaljit(QuicSolv) 0n 26 August 2011
                                    if (oRow["VendorItemID"].ToString() != "")
                                    {
                                        //rptDS.Tables[0].Rows.Add(new object[] { oRow[0].ToString(), itemName, GetImageData(System.IO.Path.GetTempPath() + "\\" + oRow["ItemID"].ToString() + imageNo.ToString() + ".bmp"), Configuration.convertNullToDecimal(oRow["sellingprice"].ToString()).ToString("Price " + Configuration.CInfo.CurrencySymbol + "#####0.00"), oRow["ProductCode"], oRow["Location"], oRow["VendorCode"], oRow["VendorID"], oRow["PCKSIZE"], oRow["PCKQTY"], oRow["PCKUNIT"], oRow["VendorItemID"].ToString(), GetImageData(System.IO.Path.GetTempPath() + "\\" + oRow["VendorItemID"].ToString() + ".bmp"), oRow["ManufacturerName"].ToString(), oRow["AvgPrice"], oRow["OnSalePrice"], oRow["ProductCode"].ToString(), "", Configuration.convertNullToBoolean(oRow["isOnSale"]), Configuration.convertNullToInt(oRow["SaleLimitQty"]), oRow["SaleStartDate"], oRow["SaleEndDate"] });  //Sprint-21 - 09-Sep-2015 JY Added sale price related objects
                                        rptDS.Tables[0].Rows.Add(new object[] { oRow[0].ToString(), itemName, GetImageData(System.IO.Path.GetTempPath() + "\\" + oRow["ItemID"].ToString() + imageNo.ToString() + ".bmp"), Configuration.convertNullToDecimal(oRow["sellingprice"].ToString()).ToString("Price " + Configuration.CInfo.CurrencySymbol + "#####0.00"), oRow["ProductCode"], oRow["Location"], oRow["VendorCode"], oRow["VendorID"], oRow["PCKSIZE"], oRow["PCKQTY"], oRow["PCKUNIT"], oRow["VendorItemID"].ToString(), GetImageData(System.IO.Path.GetTempPath() + "\\" + oRow["VendorItemID"].ToString() + ".bmp"), oRow["ManufacturerName"].ToString(), oRow["AvgPrice"], oRow["OnSalePrice"], oRow["ProductCode"].ToString(), "", Configuration.convertNullToBoolean(oRow["isOnSale"]), Configuration.convertNullToInt(oRow["SaleLimitQty"]), oRow["SaleStartDate"], oRow["SaleEndDate"], Configuration.convertNullToString(oRow["VendorName"]) });    //PRIMEPOS-2758 11-Nov-2019 JY Added VendorName
                                    }
                                    else
                                    {
                                        //rptDS.Tables[0].Rows.Add(new object[] { oRow[0].ToString(), itemName, GetImageData(System.IO.Path.GetTempPath() + "\\" + oRow["ItemID"].ToString() + imageNo.ToString() + ".bmp"), Configuration.convertNullToDecimal(oRow["sellingprice"].ToString()).ToString("Price " + Configuration.CInfo.CurrencySymbol + "#####0.00"), oRow["ProductCode"], oRow["Location"], oRow["VendorCode"], oRow["VendorID"], oRow["PCKSIZE"], oRow["PCKQTY"], oRow["PCKUNIT"], null, null, oRow["ManufacturerName"].ToString(), oRow["AvgPrice"], oRow["OnSalePrice"], oRow["ProductCode"].ToString(), "", Configuration.convertNullToBoolean(oRow["isOnSale"]), Configuration.convertNullToInt(oRow["SaleLimitQty"]), oRow["SaleStartDate"], oRow["SaleEndDate"] });   //Sprint-21 - 09-Sep-2015 JY Added sale price related objects
                                        rptDS.Tables[0].Rows.Add(new object[] { oRow[0].ToString(), itemName, GetImageData(System.IO.Path.GetTempPath() + "\\" + oRow["ItemID"].ToString() + imageNo.ToString() + ".bmp"), Configuration.convertNullToDecimal(oRow["sellingprice"].ToString()).ToString("Price " + Configuration.CInfo.CurrencySymbol + "#####0.00"), oRow["ProductCode"], oRow["Location"], oRow["VendorCode"], oRow["VendorID"], oRow["PCKSIZE"], oRow["PCKQTY"], oRow["PCKUNIT"], null, null, oRow["ManufacturerName"].ToString(), oRow["AvgPrice"], oRow["OnSalePrice"], oRow["ProductCode"].ToString(), "", Configuration.convertNullToBoolean(oRow["isOnSale"]), Configuration.convertNullToInt(oRow["SaleLimitQty"]), oRow["SaleStartDate"], oRow["SaleEndDate"], Configuration.convertNullToString(oRow["VendorName"]) });    //PRIMEPOS-2758 11-Nov-2019 JY Added VendorName
                                    }
                                }
                                //imageNo++;//remove by Manoj
                            }
                            imageNo = 1;
                            if (chkUseLabelPrinter.Checked == true)
                            {
                                BarcodeLabelDataCollection oBarcodeColl = new BarcodeLabelDataCollection();
                                Item oItem = new Item();
                                foreach (DataRow oRow in rptDS.Tables[0].Rows)
                                {
                                    BarcodeLabelData oBarcode = new BarcodeLabelData();

                                    /* oBarcode.ItemCode = oRow["ItemID"].ToString();
                                     oBarcode.ItemName = oRow["Description"].ToString();
                                     oBarcode.SellingPrice = Configuration.convertNullToDecimal(oRow["SellingPrice"].ToString().Replace("Price", "").Replace("$", "").Trim());
                                     oBarcode.BarCode = Image.FromFile(System.IO.Path.GetTempPath() + "\\" + oRow["ItemID"].ToString() + ".bmp");
                                     oBarcodeColl.Add(oBarcode);*/
                                    oBarcode.Item = oItem.FindItemByID(oRow["ItemID"].ToString());
                                    oBarcode.ItemCode = oRow["ItemID"].ToString();
                                    oBarcode.ItemCode2 = oRow["ProductCode"].ToString();
                                    oBarcode.ItemName = oRow["Description"].ToString();
									oBarcode.SellingPrice = Configuration.convertNullToDecimal(oRow["SellingPrice"].ToString().Replace("Price", "").Replace(Configuration.CInfo.CurrencySymbol.ToString(), "").Trim());
                                    oBarcode.BarCode = Image.FromFile(System.IO.Path.GetTempPath() + "\\" + oRow["ItemID"].ToString() + imageNo.ToString() + ".bmp");//fix by manoj 2/19/2015
                                    oBarcode.IsItemFSA = oItem.IsIIASItem(oBarcode.ItemCode);
                                    oBarcode.FineLineCode = oItem.GetFineLineCode(oBarcode.ItemCode);
                                    oBarcode.Location = oRow["Location"].ToString();
                                    oBarcode.VendorCode = oRow["VendorCode"].ToString();//Added by SRT (Abhishek D) Date : 04 April 2010
                                    oBarcode.VendorID = oRow["VendorID"].ToString();
                                    oBarcode.PckSize = oRow["PCKSIZE"].ToString();
                                    oBarcode.PckQty = oRow["PCKQTY"].ToString();
                                    oBarcode.PckUnit = oRow["PCKUNIT"].ToString();

                                    oBarcode.ManufacturerName = oRow["ManufacturerName"].ToString();
                                    oBarcode.AvgPrice = oRow["AvgPrice"].ToString();
                                    oBarcode.OnSalePrice = oRow["OnSalePrice"].ToString();
                                    oBarcode.ProductCode = oRow["ProductCode"].ToString();
                                    
                                    #region Sprint-21 - 09-Sep-2015 JY Added sale price related objects
                                    oBarcode.IsOnSale = Configuration.convertNullToBoolean(oRow["isOnSale"]);
                                    oBarcode.SaleLimitQty =Configuration .convertNullToInt(oRow["SaleLimitQty"]);
                                    oBarcode.SaleStartDate = (oRow["SaleStartDate"].ToString() == string.Empty ? DateTime.MinValue : Convert.ToDateTime(oRow["SaleStartDate"]));
                                    oBarcode.SaleEndDate = (oRow["SaleEndDate"].ToString() == string.Empty ? DateTime.MinValue : Convert.ToDateTime(oRow["SaleEndDate"]));
                                    #endregion
                                    oBarcode.VendorName = Configuration.convertNullToString(oRow["VendorName"]);   //PRIMEPOS-2758 11-Nov-2019 JY Added 

                                    Item oBItem = new Item();
                                    ItemData oItemData;
                                    ItemRow oItemRow = null;
                                    oItemData = oBItem.Populate(oRow["ItemID"].ToString());
                                    oItemRow = oItemData.Item[0];
                                    oBarcode.DesireQuantity = Configuration.convertNullToString(oItemRow.MinOrdQty);//Added By Shitaljit on 10 April 2013 for PRIMEPOS-776
                                    //Added By Amit Date 8 July 2011
                                    // Following if is added by shitaljit(QuicSolv) 0n 26 August 2011
                                    if (string.IsNullOrEmpty(oRow["VendorItemID"].ToString()) == false)
                                    {
                                        oBarcode.VendorItemID = oRow["VendorItemID"].ToString();
                                        oBarcode.VendorItemIDBarCode = Image.FromFile(System.IO.Path.GetTempPath() + "\\" + oRow["VendorItemID"].ToString() + ".bmp");
                                        //End
                                    }
                                    oBarcodeColl.Add(oBarcode);
                                    //imageNo++; //remove by Manoj
                                }

                                BarcodeLabel oBarcodeLabel = new BarcodeLabel(oBarcodeColl);
                                oBarcodeLabel.Print();
                            }
                            else
                            {
                                #region Added By Shitaljit for new Label printing template logic
                                if (oTemplateRptObj == null)
                                {
                                    oTemplateRptObj = oRpt;
                                }
                                if (PrepareLabelReportData(rptDS) == true)
                                {
                                    oTemplateRptObj.Database.Tables[0].SetDataSource(rptDS.Tables[0]);
                                    try
                                    {
                                        if (Configuration.oLPSetup.PrintItemID == false)
                                        {
                                            ((CrystalDecisions.CrystalReports.Engine.TextObject)oTemplateRptObj.ReportDefinition.ReportObjects["txtItemCode"]).Text = "";
                                        }
                                        else
                                        {
                                            ((CrystalDecisions.CrystalReports.Engine.TextObject)oTemplateRptObj.ReportDefinition.ReportObjects["txtItemCode"]).Text = "IC";
                                        }
                                        if (Configuration.oLPSetup.PrintItemVendorID == false)
                                        {
                                            ((CrystalDecisions.CrystalReports.Engine.TextObject)oTemplateRptObj.ReportDefinition.ReportObjects["txtIVCode"]).Text = "";
                                        }
                                        else 
                                        {
                                            ((CrystalDecisions.CrystalReports.Engine.TextObject)oTemplateRptObj.ReportDefinition.ReportObjects["txtIVCode"]).Text = "IVC";
                                        }
                                    }
                                    catch
                                    {
                                        //there is nothing in the catch blok as error will 
                                        //occur in case the report does not contain above objects txtItemCode and txtIVCode
                                    }
                                #endregion
                                    if (PrintId == false)
                                    {
                                        clsReports.ShowReport(oTemplateRptObj);
                                    }
                                    else
                                    {
                                        clsReports.PrintReport(oTemplateRptObj);
                                    }
                                }
                            }
                        }
                    }
                }

            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }

        private void btnViewLable_Click(object sender, EventArgs e)
        {
            #region PRIMEPOS-06012017 01-Jun-2017 JY Added 
            if (this.chkUseLabelPrinter.Checked == true)
            {
                clsUIHelper.ShowErrorMsg("Please uncheck the \"Use Label Printer\" settings to \"View Labels\".");
                if (chkUseLabelPrinter.Enabled)
                    chkUseLabelPrinter.Focus();
                return;
            }
            #endregion

            //Preview(false);

            VendorList_Expand(false);
            int rowIndex = 0;
            bool Itemselect = false;
            if (ds != null)
            {
                if (ds.Tables.Count != 0)
                {

                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow oRow in ds.Tables[0].Rows)
                        {

                            if (Convert.ToBoolean(this.grdSearch.Rows[rowIndex].Cells["Check Items"].Value) == true)
                            {
                                Itemselect = true;
                                break;
                            }
                            rowIndex++;
                        }
                    }
                }
            }
            if (Itemselect == false)
            {
                clsUIHelper.ShowErrorMsg("Please Select the Item from list");
                return;
            }
            string fieldName = string.Empty;
            try
            {
                if (!validateFields(out fieldName))
                {
                    if (fieldName == "DATE")
                    {
                        clsUIHelper.ShowErrorMsg("From Date can not be greater than To Date.");
                    }

                    return;
                }
                //this.dtpFromDate.Focus();
                Preview(false);
            }
            catch (Exception ex)
            {
                clsUIHelper.ShowErrorMsg(ex.Message);
            }
        }

        //private void FKEdit()
        //{
        //    #region Items
        //    try
        //    {
        //        //Item oItem = new Item();
        //        //ItemData oItemData;
        //        //ItemRow oItemRow = null;
        //        //oItemData = oItem.Populate(code);
        //        //oItemRow = oItemData.Item[0];

        //        //if (oItemRow != null)
        //        //{
        //        // oDS.Tables[0].Rows.Add(new object[] { oItemRow.ItemID, oItemRow.Description, 0, 0, oItemRow.SellingPrice });
        //        oDS.Tables[0].Rows.Add(new object[] { grdSearch.DisplayLayout.Bands[0].Columns[0], grdSearch.DisplayLayout.Bands[0].Columns[1], 0, 0, grdSearch.DisplayLayout.Bands[0].Columns[2] });
        //        //this.grdSearch.Focus();
        //        //this.grdSearch.ActiveRow = this.grdSearch.Rows[this.grdSearch.Rows.Count - 1];
        //        //this.grdSearch.ActiveCell = this.grdSearch.ActiveRow.Cells[2];
        //        //this.grdSearch.PerformAction(UltraGridAction.EnterEditMode);
        //        //}
        //    }
        //    catch (System.IndexOutOfRangeException)
        //    {
        //        this.grdSearch.ActiveCell.Value = String.Empty;
        //        this.grdSearch.ActiveRow.Cells["Description"].Value = String.Empty;
        //    }
        //    catch (Exception exp)
        //    {
        //        clsUIHelper.ShowErrorMsg(exp.Message);
        //        exp = null;
        //        this.grdSearch.ActiveCell.Value = String.Empty;
        //        this.grdSearch.ActiveRow.Cells["Description"].Value = String.Empty;
        //    }
        //    #endregion
        //}

        private void btnPrintLable_Click(object sender, EventArgs e)
        {
            VendorList_Expand(false);
            Preview(true);
        }
        private bool validateFields(out string fieldName)
        {
            bool isValid = true;
            string field = string.Empty;
            try
            {
                if ((DateTime)dtpFromDate.Value > (DateTime)dtpToDate.Value)
                {
                    isValid = false;
                    fieldName = "DATE";
                    return isValid;
                }
            }
            catch (Exception ex)
            {
                clsUIHelper.ShowErrorMsg(ex.Message);
            }
            fieldName = field;
            return isValid;
        }

        private void btnViewReport_Click(object sender, EventArgs e)
        {
            VendorList_Expand(false);
            Preview_Report(false);
        }

        private void Preview_Report(bool PrintId)
        {
            string Criteria = string.Empty;
            string UpdatedBy = string.Empty;
            try
            {
                rptItemPriceChangeLog oRpt = new rptItemPriceChangeLog();
                //PRIMEPOS-2464 09-Mar-2020 JY Added nDisplayItemCost
                string sSQL = " SELECT IPH.*, Item.Description, Dept.DeptName, " + nDisplayItemCost + " AS DisplayItemCost FROM Itempricehistory IPH" +
                            " INNER JOIN Item ON IPH.ItemID = Item.ItemID" +
                            " LEFT OUTER JOIN Department Dept ON Item.DepartmentID = Dept.DeptID" +
                            " INNER JOIN ItemVendor ON ItemVendor.ItemId = item.ItemId" +
                            " LEFT JOIN Vendor on vendor.VendorID = ItemVendor.VendorID" +
                            " WHERE 1 = 1";                               

                Criteria = buildCriteria_ForReport();

                if (Criteria == "-1")
                {
                    return;
                }
                else
                    sSQL = sSQL + Criteria;

                sSQL += " Order by Dept.DeptName, Item.Description, IPH.AddedON Desc";
                clsReports.setCRTextObjectText("txtFromDate", "From :" + this.dtpFromDate.Text, oRpt);
                clsReports.setCRTextObjectText("txtToDate", "To :" + dtpToDate.Text, oRpt);
                clsReports.Preview(PrintId, sSQL, oRpt);
            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }

        private void frmRptItemPriceLogLable_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyData == System.Windows.Forms.Keys.F4)
            {
                txtDeptCode_EditorButtonClick(null, null);
            }
            if (e.KeyData == System.Windows.Forms.Keys.F6)
            {
                btnAddItem_Click(null, null);
            }

            if (e.KeyData == System.Windows.Forms.Keys.F2)
            {
                //chkSelectAll_CheckedChanged(sender, e);
                if (chkSelectAll.Checked == true)
                    chkSelectAll.Checked = false;
                else
                    chkSelectAll.Checked = true;
            }
            if (e.KeyData == System.Windows.Forms.Keys.Escape)
            {
                if (grpVendorList.Visible == true)
                {
                    dataGridList.Visible = false;
                    grpVendorList.Visible = false;
                }
                else
                    this.Close();
            }
            if (e.KeyData == System.Windows.Forms.Keys.F5)
            {
                //if (butVendorList.Focus() == true ||dataGridList.Focus() == true)
                {
                    if (grpVendorList.Visible == false)
                        VendorList_Expand(true);
                    else
                        VendorList_Expand(false);
                }

            }
            if (e.KeyData == System.Windows.Forms.Keys.Enter)
            {
                VendorList_Expand(false);
            }
            /*if (e.KeyData == System.Windows.Forms.Keys.Down)
            {
                VendorList_Expand(true);
            }
            if (e.KeyData == System.Windows.Forms.Keys.Up)
            {
                VendorList_Expand(false);
            }*/

        }

        bool SelectAllFlag = false;

        private void ultraButSelectAll_Click(object sender, EventArgs e)
        {
            int rowIndex = 0;
            if (grdSearch.Rows.Count > 0)
            {
                if (SelectAllFlag == false)
                {
                    foreach (DataRow oRow in ds.Tables[0].Rows)
                    {
                        this.grdSearch.Rows[rowIndex].Cells["Check Items"].Value = true;
                        rowIndex++;
                    }
                    SelectAllFlag = true;
                }
                else
                {
                    foreach (DataRow oRow in ds.Tables[0].Rows)
                    {
                        this.grdSearch.Rows[rowIndex].Cells["Check Items"].Value = false;
                        rowIndex++;
                    }
                    SelectAllFlag = false;
                }
            }
        }

        private void dataGridList_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void btnPrintReport_Click(object sender, EventArgs e)
        {
            VendorList_Expand(false);
            Preview_Report(true);
        }

        private void butVendorList_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyData == System.Windows.Forms.Keys.Enter)
            {
                if (grpVendorList.Visible == true)
                {
                    dataGridList.Visible = false;
                    grpVendorList.Visible = false;
                }
            }
        }

        private void butVendorList_Enter(object sender, EventArgs e)
        {
            //VendorList_Expand(true);
        }

        private void VendorList_Expand(bool Value)
        {
            if (Value == true)
            {
                if (grpVendorList.Visible == false)
                {
                    dataGridList.Visible = true;
                    grpVendorList.Visible = true;
                    dataGridList.Height = 285;
                    grpVendorList.Height = dataGridList.Height + 31;
                    GridVisibleFlag = true;
                    chkSelectAll.Location = new Point(chkSelectAll.Location.X, dataGridList.Height + 10);
                    grpVendorList.Focus();

                    //dataGridList.Focus();
                    //dataGridList.Rows[1].Selected = true;
                }
            }
            else
            {
                if (grpVendorList.Visible == true)
                {
                    dataGridList.Visible = false;
                    grpVendorList.Visible = false;
                    GridVisibleFlag = false;
                    //butVendorList.Focus();
                }
            }
        }

        private void TextBoxKeyup(object sender, System.Windows.Forms.KeyEventArgs e)
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

        private void grdSearch_KeyDown(object sender, KeyEventArgs e)
        {
            UltraGridCell oCurrentCell;

            if ((e.KeyData == Keys.Enter || e.KeyData == Keys.Tab || e.KeyData == Keys.Down || e.KeyData == Keys.Up))
            {
                try
                {
                    if (e.KeyData == Keys.Down || e.KeyData == Keys.Up)
                    {
                        int grdRowId = 0;
                        if (e.KeyData == Keys.Down)
                        {
                            this.grdSearch.PerformAction(Infragistics.Win.UltraWinGrid.UltraGridAction.NextRow);
                        }
                        else if (e.KeyData == Keys.Up)
                        {
                            this.grdSearch.PerformAction(Infragistics.Win.UltraWinGrid.UltraGridAction.PrevRow);
                        }

                        grdRowId = grdSearch.ActiveRow.Index;
                        this.grdSearch.Rows[grdRowId].Selected = true;
                        this.grdSearch.Rows[grdRowId].Activate();
                        this.grdSearch.ActiveRow.Cells["Check Items"].Activate();
                        this.grdSearch.PerformAction(UltraGridAction.EnterEditMode);
                    }
                }
                catch (Exception Ex)
                {
                }
                e.Handled = true;

            }
            if (e.KeyData == Keys.Space)
            {
                if (grdSearch.ActiveRow != null)
                {
                    if (!this.grdSearch.ActiveRow.Cells["Check Items"].Activated)
                    {
                        this.grdSearch.ActiveRow.Cells["Check Items"].Activate();
                        grdSearch_KeyDown(sender, e);
                        this.grdSearch.PerformAction(UltraGridAction.EnterEditMode);
                        //if (Convert.ToBoolean(grdSearch.ActiveRow.Cells["Check Items"].Value) == true)
                        //    grdSearch.ActiveRow.Cells["Check Items"].Value = false;
                        //else
                        //    grdSearch.ActiveRow.Cells["Check Items"].Value = true;
                    }
                }

            }


        }

        private void chkSelectAll_CheckedChanged(object sender, EventArgs e)
        {
            int rowsCount = dataGridList.Rows.Count;
            if (rowsCount > 0)
            {
                for (int i = 0; i < rowsCount; i++)
                {
                    if (chkSelectAll.Checked == true)
                        dataGridList.Rows[i].Cells[0].Value = true;
                    else
                        dataGridList.Rows[i].Cells[0].Value = false;
                }

                if (chkSelectAll.Checked == true)
                    chkSelectAll.Text = "Unselect All";
                else
                    chkSelectAll.Text = "Select All";
            }
        }

        private void grdSearch_Leave(object sender, EventArgs e)
        {
            /*if (grdSearch.Selected.Rows.Count != 0)
            {
                if (grdSearch.Selected.Rows[0].Index < grdSearch.Rows.Count - 1)
                {
                    this.grdSearch.PerformAction(Infragistics.Win.UltraWinGrid.UltraGridAction.NextRow);
                    this.grdSearch.Focus();
                }
            }*/
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {
            // VendorList_Expand(false);
        }

        private void grpVendorList_Leave(object sender, EventArgs e)
        {
            //VendorList_Expand(false);
        }

        private void dataGridList_Leave(object sender, EventArgs e)
        {
            //VendorList_Expand(false);
        }

        private void frmRptItemPriceLogLable_MouseClick(object sender, MouseEventArgs e)
        {
            //VendorList_Expand(false);
        }

        private void frmRptItemPriceLogLable_MouseDown(object sender, MouseEventArgs e)
        {

        }

        private void butVendorList_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == System.Windows.Forms.Keys.Down)
            {
                dataGridList.Focus();
            }
        }

        private void dataGridList_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == System.Windows.Forms.Keys.Down)
            {
                dataGridList.Rows[0].Selected = true;
            }
        }

        private void frmRptItemPriceLogLable_KeyDown(object sender, KeyEventArgs e)
        {
            if (grpVendorList.Visible == true)
            {
                if (e.KeyData == System.Windows.Forms.Keys.Down)
                {
                    dataGridList.Focus();
                }
            }
        }

        private void btnAddItem_Click(object sender, EventArgs e)
        {
            int rowIndex = 0;
            VendorList_Expand(false);
            if (txtItemCode.Text == string.Empty && txtItemName.Text == string.Empty)
            {
                clsUIHelper.ShowErrorMsg("Please insert Item Code or Item Name.");
                txtItemCode.Focus();
                return;
            }
            if (ShowData(true, true, false))
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow oRow in ds.Tables[0].Rows)
                    {
                        this.grdSearch.Rows[rowIndex].Cells["Check Items"].Value = false;
                        this.grdSearch.Rows[rowIndex].Cells["No. Of Labels"].Value = Configuration.convertNullToInt(txtNoOfLabels.Value);   //PRIMEPOS-2630 16-Jan-2019 JY modified
                        this.grdSearch.Rows[rowIndex].Cells["No. Of Sheet"].Value = Configuration.convertNullToInt(txtNoOfSheet.Value); //PRIMEPOS-2630 16-Jan-2019 JY modified
                        rowIndex++;
                    }
                }
                else if (chkInstockItm.Checked)
                {
                    clsUIHelper.ShowErrorMsg("Item is not present in stock");
                }
                grdSearch.DisplayLayout.Bands[0].Columns["Vendor Name"].Header.VisiblePosition = 3;
            }

            lblRecordCount.Text = "Records Count = " + grdSearch.Rows.Count;
            txtItemCode.Text = string.Empty;
            txtItemName.Text = string.Empty;
            txtItemCode.Focus();
        }

        private void btnRemoveItems_Click(object sender, EventArgs e)
        {
            RowCount = 0;//Added By Shitaljit(Quicsolv) on 16 Jan 2012
            DataSet dsTemp = new DataSet();
            dsTemp = ds.Copy();
            int rowIndex = 0;
            if (dsTemp != null)
            {
                if (ds.Tables.Count != 0)
                {

                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        ds.Tables[0].Clear();
                        lblRecordCount.Text = "Records Count = 0";
                    }
                }
            }
            groupBox3.Focus();
            txtItemCode.Focus();
            NoOfLabels = 0;
            lblNoOfLabels.Text = string.Empty;
            //txtNoOfLabels.ResetValue();
            //txtNoOfSheet.ResetValue();
        }

        private void txtItemCode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == System.Windows.Forms.Keys.Enter)
            {
                //btnSearch.Focus();
                btnAddItem_Click(sender, e);
            }
        }

        private void txtItemName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == System.Windows.Forms.Keys.Enter)
            {
                //btnSearch.Focus();
                btnAddItem_Click(sender, e);
            }
        }

        private void txtNoOfLabels_ValueChanged(object sender, EventArgs e)
        {
            int rowIndex = 0;
            if (ds != null)
            {
                if (ds.Tables.Count != 0)
                {

                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow oRow in ds.Tables[0].Rows)
                        {
                            this.grdSearch.Rows[rowIndex].Cells["No. Of Labels"].Value = txtNoOfLabels.Value;
                            rowIndex++;
                        }
                    }
                }
            }
        }

        private void txtNoOfSheet_ValueChanged(object sender, EventArgs e)
        {

            int rowIndex = 0;
            if (ds != null)
            {
                if (ds.Tables.Count != 0)
                {

                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow oRow in ds.Tables[0].Rows)
                        {
                            this.grdSearch.Rows[rowIndex].Cells["No. Of Sheet"].Value = Configuration.convertNullToInt(txtNoOfSheet.Value); //PRIMEPOS-2630 16-Jan-2019 JY modified
                            rowIndex++;
                        }
                    }
                }
            }
        }

        private void rdSelectionCriteria_CheckedChanged(object sender, EventArgs e)
        {
            if (rdSelectionCriteria.Checked == true)
            {
                dtpFromDate.Enabled = true;
                dtpToDate.Enabled = true;
                butVendorList.Enabled = true;
                groupBox4.Enabled = true;
                CmbPriceType.Enabled = true;
            }
        }

        private void rdDepartmentSearch_CheckedChanged(object sender, EventArgs e)
        {
            if (rdDepartmentSearch.Checked == true)
            {
                VendorList_Expand(false);
                dtpFromDate.Enabled = false;
                dtpToDate.Enabled = false;
                butVendorList.Enabled = false;
                groupBox4.Enabled = false;
                CmbPriceType.SelectedIndex = 1;
                CmbPriceType.Enabled = false;
            }
        }

        private void grdSearch_Enter(object sender, EventArgs e)
        {
            this.grdSearch.PerformAction(UltraGridAction.FirstCellInGrid);
            this.grdSearch.PerformAction(UltraGridAction.EnterEditMode);
            //this.grdSearch.ActiveRow.Cells["Check Items"].Activate();
        }

        private void ultraLabel2_Click(object sender, EventArgs e)
        {

        }

        int NoOfLabels = 0;
        bool ValueFlag = false;
        private void grdSearch_AfterCellUpdate(object sender, CellEventArgs e)
        {
            #region PRIMEPOS-06012017 02-Jun-2017 JY Added
            int LabelPerSheet = Configuration.convertNullToInt(Configuration.LabelPerSheet);
            //int rowIndex = 0;
            //if (ValueFlag == true)
            //{
            //    ValueFlag = false;
            //    return;
            //}
            //if (Convert.ToString(e.Cell.Value) == string.Empty)
            //{
            //    ValueFlag = true;
            //    e.Cell.Value = e.Cell.OriginalValue;

            //}
            //else
            //    ValueFlag = false;

            //if (e.Cell.Column.Key == "Check Items")
            //{
            //    if ((e.Cell.Value != null) && (Convert.ToString(e.Cell.Value) != string.Empty))
            //    {
            //        if (Convert.ToBoolean(e.Cell.Value) == true)
            //            NoOfLabels = NoOfLabels + Convert.ToInt32(this.grdSearch.Rows[e.Cell.Row.Index].Cells["No. Of Labels"].Value);
            //        else
            //        {
            //            if (NoOfLabels != 0)
            //                NoOfLabels = NoOfLabels - Convert.ToInt32(this.grdSearch.Rows[e.Cell.Row.Index].Cells["No. Of Labels"].Value);
            //        }
            //    }
            //}
            //if (e.Cell.Column.Key == "No. Of Labels")
            //{

            //    if ((e.Cell.Value != null) && (Convert.ToString(e.Cell.Value) != string.Empty))
            //    {
            //        if (Convert.ToBoolean(this.grdSearch.Rows[e.Cell.Row.Index].Cells["Check Items"].Value) == true)
            //        {
            //            if ((this.grdSearch.Rows[e.Cell.Row.Index].Cells["No. Of Labels"].OriginalValue != null) && (Convert.ToString(this.grdSearch.Rows[e.Cell.Row.Index].Cells["No. Of Labels"].OriginalValue) != string.Empty))
            //                NoOfLabels = NoOfLabels - Convert.ToInt32(this.grdSearch.Rows[e.Cell.Row.Index].Cells["No. Of Labels"].OriginalValue);
            //            NoOfLabels = NoOfLabels + Convert.ToInt32(this.grdSearch.Rows[e.Cell.Row.Index].Cells["No. Of Labels"].Value);
            //        }
            //        /*else
            //        {
            //            if (NoOfLabels != 0)
            //                NoOfLabels = NoOfLabels - Convert.ToInt32(this.grdSearch.Rows[e.Cell.Row.Index].Cells["No. Of Labels"].Value);
            //        }*/
            //    }
            //}

            //if (e.Cell.Column.Key == "No. Of Sheet")
            //{
            //    if ((e.Cell.Value != null) && (Convert.ToString(e.Cell.Value) != string.Empty))
            //    {
            //        //if (Convert.ToInt32(e.Cell.Value) > 0)
            //        {
            //            if (Convert.ToBoolean(this.grdSearch.Rows[e.Cell.Row.Index].Cells["Check Items"].Value) == true)
            //            {
            //                if ((this.grdSearch.Rows[e.Cell.Row.Index].Cells["No. Of Sheet"].OriginalValue != null) && (Convert.ToString(this.grdSearch.Rows[e.Cell.Row.Index].Cells["No. Of Sheet"].OriginalValue) != string.Empty))
            //                    NoOfLabels = NoOfLabels - (Convert.ToInt32(this.grdSearch.Rows[e.Cell.Row.Index].Cells["No. Of Sheet"].OriginalValue) * LabelPerSheet);
            //                NoOfLabels = NoOfLabels + (Convert.ToInt32(this.grdSearch.Rows[e.Cell.Row.Index].Cells["No. Of Sheet"].Value) * LabelPerSheet);
            //            }
            //            /*else
            //            {
            //                if (NoOfLabels != 0)
            //                    NoOfLabels = NoOfLabels - Convert.ToInt32(this.grdSearch.Rows[e.Cell.Row.Index].Cells["No. Of Labels"].Value);
            //            }*/
            //        }
            //    }
            //}
            #endregion

            /*int NoOfLabels = 0;
            int rowIndex = 0;
            int LabelPerSheet = Configuration.LabelPerSheet;
            if (ds != null)
            {
                if (ds.Tables.Count != 0)
                {

                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow oRow in ds.Tables[0].Rows)
                        //foreach (DataRow oRow in grdSearch.Rows.)
                        {
                            
                            if (Convert.ToBoolean(this.grdSearch.Rows[rowIndex].Cells["Check Items"].Value) == true)
                            {
                                if (Convert.ToInt32(this.grdSearch.Rows[rowIndex].Cells["No. Of Sheet"].Value) > 0)
                                    NoOfLabels = NoOfLabels + Convert.ToInt32(this.grdSearch.Rows[rowIndex].Cells["No. Of Labels"].Value) + LabelPerSheet;
                                else
                                    NoOfLabels = NoOfLabels + Convert.ToInt32(this.grdSearch.Rows[rowIndex].Cells["No. Of Labels"].Value);
                            }
                            rowIndex++;
                        }
                    }
                }
            }*/

            #region PRIMEPOS-06012017 02-Jun-2017 JY Added
            NoOfLabels = 0;
            foreach (UltraGridRow row in this.grdSearch.Rows)
            {
                if (Configuration.convertNullToBoolean(row.Cells["Check Items"].Text) == true)
                {
                    NoOfLabels += Configuration.convertNullToInt(row.Cells["No. Of Labels"].Value) + (Configuration.convertNullToInt(row.Cells["No. Of Sheet"].Value) * Configuration.convertNullToInt(Configuration.LabelPerSheet));
                }
            }
            #endregion

            lblNoOfLabels.Text = "Numbers Of Labels :" + NoOfLabels;
        }

        private DataSet GetPckData(string ItemID)
        {
            DataSet dsItem = new DataSet();
            try
            {
                //rptItemPriceChangeLog oRpt = new rptItemPriceChangeLog();


                IDbCommand cmd = DataFactory.CreateCommand();
                string sSQL = "";
                string Criteria = "";


                IDbConnection conn = DataFactory.CreateConnection();

                conn.ConnectionString = Configuration.ConnectionString;

                conn.Open();

                try
                {
                    sSQL = " SELECT " +
                                  " PCKSIZE " +
                                  " ,PCKQTY " +
                                  " ,PCKUNIT " +
                              " FROM  Item" +
                              " WHERE Item.itemID = '" + ItemID + "'";
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = sSQL;
                    cmd.Connection = conn;
                    SqlDataAdapter sqlDa = (SqlDataAdapter)da;
                    sqlDa.SelectCommand = (SqlCommand)cmd;
                    da.Fill(dsItem);
                    //da.Fill(ds);
                    conn.Close();




                    //grdSearch1.DataSource = ds;
                }
                catch (NullReferenceException)
                {
                    conn.Close();
                    //ErrorHandler.throwCustomError(POSErrorENUM.User_InvalidUserName);
                }
                catch (Exception exp)
                {
                    conn.Close();
                    throw (exp);
                }

            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
            return dsItem;
        }

        #region Added By shitaljit to new Label Printing logic JIRA PRIMEPOS-927
        private void fillLabelObjectList()
        {
            DataTable dtLabelObject = new DataTable();
            dtLabelObject.Columns.Add("Object");
            dtLabelObject.Columns.Add("Id");
            dtLabelObject.NewRow();
            dtLabelObject.Rows.Add("Item Id", "ItemID");
            dtLabelObject.NewRow();
            dtLabelObject.Rows.Add("Item Id Barcode", "ItemIDBarcode");
            dtLabelObject.NewRow();
            dtLabelObject.Rows.Add("Item Description", "Description");
            dtLabelObject.NewRow();
            dtLabelObject.Rows.Add("Item Selling Price", "Price");
            dtLabelObject.NewRow();
            dtLabelObject.Rows.Add("Vendor Item Id", "VendorITemID");
            dtLabelObject.NewRow();
            dtLabelObject.Rows.Add("Vendor Item Id Barcode", "VItemBarcode");
            dtLabelObject.NewRow(); //PRIMEPOS-2758 08-Nov-2019 JY Added
            dtLabelObject.Rows.Add("Vendor Name", "VendorName");

            #region Sprint-26 - PRIMEPOS-2415 31-Aug-2017 JY Commented as these objects are not printing on report
            //dtLabelObject.NewRow();
            //dtLabelObject.Rows.Add("Manufacturer Name", "ManufacturerName");

            //dtLabelObject.NewRow();
            //dtLabelObject.Rows.Add("Avg Price", "AvgPrice");

            //dtLabelObject.NewRow();
            //dtLabelObject.Rows.Add("On Sale Price", "OnSalePrice");

            //dtLabelObject.NewRow();
            //dtLabelObject.Rows.Add("SKU Code", "ProductCode");
            //#region Sprint-21 - 09-Sep-2015 JY Added sale price related objects
            //dtLabelObject.NewRow();
            //dtLabelObject.Rows.Add("isOnSale", "isOnSale");
            //dtLabelObject.NewRow();
            //dtLabelObject.Rows.Add("SaleLimitQty", "SaleLimitQty");
            //dtLabelObject.NewRow();
            //dtLabelObject.Rows.Add("SaleStartDate", "SaleStartDate");
            //dtLabelObject.NewRow();
            //dtLabelObject.Rows.Add("SaleEndDate", "SaleEndDate");
            //#endregion
            #endregion

            dtLabelSetting.DataSource = dtLabelObject;
            dtLabelSetting.Columns[0].Width = 20;
            dtLabelSetting.Columns[1].ReadOnly = true;
            dtLabelSetting.Columns[1].Width = 250;
            dtLabelSetting.Columns[2].Visible = false;
        }
        private void SetLabelPrintingSetup()
        {
            int rowsCount = dtLabelSetting.Rows.Count;
            try
            {
                if (rowsCount > 0)
                {
                    Configuration.oLPSetup.PrintItemID = Configuration.convertNullToBoolean(dtLabelSetting.Rows[0].Cells[0].Value);
                    Configuration.oLPSetup.PrintItemIDBarcode = Configuration.convertNullToBoolean(dtLabelSetting.Rows[1].Cells[0].Value);
                    Configuration.oLPSetup.PrintDescription = Configuration.convertNullToBoolean(dtLabelSetting.Rows[2].Cells[0].Value);
                    Configuration.oLPSetup.PrintItemPrice = Configuration.convertNullToBoolean(dtLabelSetting.Rows[3].Cells[0].Value);
                    Configuration.oLPSetup.PrintItemVendorID = Configuration.convertNullToBoolean(dtLabelSetting.Rows[4].Cells[0].Value);
                    Configuration.oLPSetup.PrintItemVendorIDBarcode = Configuration.convertNullToBoolean(dtLabelSetting.Rows[5].Cells[0].Value);
                    Configuration.oLPSetup.PrintVendorName = Configuration.convertNullToBoolean(dtLabelSetting.Rows[6].Cells[0].Value); //PRIMEPOS-2758 12-Nov-2019 JY Added

                    #region Sprint-26 - PRIMEPOS-2415 31-Aug-2017 JY Commented
                    //Configuration.oLPSetup.ManufacturerName = Configuration.convertNullToBoolean(dtLabelSetting.Rows[6].Cells[0].Value);
                    //Configuration.oLPSetup.AvgPrice = Configuration.convertNullToBoolean(dtLabelSetting.Rows[7].Cells[0].Value);
                    //Configuration.oLPSetup.OnSalePrice = Configuration.convertNullToBoolean(dtLabelSetting.Rows[8].Cells[0].Value);
                    //Configuration.oLPSetup.ProductCode = Configuration.convertNullToBoolean(dtLabelSetting.Rows[9].Cells[0].Value);
                    #endregion

                    if (this.cmbLableDimension.SelectedValue != null)
                    {
                        Configuration.oLPSetup.LabelTemplate = Configuration.convertNullToString(this.cmbLableDimension.SelectedValue);
                    }
                }
            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }
        private void GetLabelPrintingSetup()
        {
            try
            {
                dtLabelSetting.Rows[0].Cells[0].Value = Configuration.oLPSetup.PrintItemID;
                dtLabelSetting.Rows[1].Cells[0].Value = Configuration.oLPSetup.PrintItemIDBarcode;
                dtLabelSetting.Rows[2].Cells[0].Value = Configuration.oLPSetup.PrintDescription;
                dtLabelSetting.Rows[3].Cells[0].Value = Configuration.oLPSetup.PrintItemPrice;
                dtLabelSetting.Rows[4].Cells[0].Value = Configuration.oLPSetup.PrintItemVendorID;
                dtLabelSetting.Rows[5].Cells[0].Value = Configuration.oLPSetup.PrintItemVendorIDBarcode;
                dtLabelSetting.Rows[6].Cells[0].Value = Configuration.oLPSetup.PrintVendorName; //PRIMEPOS-2758 12-Nov-2019 JY Added

                #region Sprint-26 - PRIMEPOS-2415 31-Aug-2017 JY Commented
                //dtLabelSetting.Rows[6].Cells[0].Value = Configuration.oLPSetup.ManufacturerName;
                //dtLabelSetting.Rows[7].Cells[0].Value = Configuration.oLPSetup.AvgPrice;
                //dtLabelSetting.Rows[8].Cells[0].Value = Configuration.oLPSetup.OnSalePrice;
                //dtLabelSetting.Rows[9].Cells[0].Value = Configuration.oLPSetup.ProductCode;
                #endregion

                if (Configuration.oLPSetup.LabelTemplate == "WL-325" || Configuration.oLPSetup.LabelTemplate == "WL-1025")
                {
                    Configuration.oLPSetup.PrintDescription = true;
                    Configuration.oLPSetup.PrintItemPrice = true;
                    dtLabelSetting.Rows[2].Cells[0].Value = true;
                    dtLabelSetting.Rows[3].Cells[0].Value = true;
                    //dtLabelSetting.Rows[2].ReadOnly = true;
                    //dtLabelSetting.Rows[3].ReadOnly = true;
                }
                else
                {
                    //dtLabelSetting.Rows[2].ReadOnly = false;
                    //dtLabelSetting.Rows[3].ReadOnly = false;
                }
            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }
        private void fillLabelTemplateList()
        {
            DataTable dtLabelTemplate = new DataTable();
            cmbLableDimension.Items.Clear();
            dtLabelTemplate.Columns.Add("Item");
            dtLabelTemplate.Columns.Add("Details");
            cmbLableDimension.ValueMember = "Item";
            cmbLableDimension.DisplayMember = "Details";
            dtLabelTemplate.NewRow();
            dtLabelTemplate.Rows.Add("WL-875", "WL-875 1.625\"x1\" Rounded Label 30 stickers per sheet");
            dtLabelTemplate.NewRow();
            dtLabelTemplate.Rows.Add("WL-1000", "WL-1000 1.5\"x1\" Rounded 50 stickers per sheet");
            dtLabelTemplate.NewRow();
            dtLabelTemplate.Rows.Add("WL-25", " WL-25 1.75\"x0.5\" Rounded 80  stickers per sheet");
            dtLabelTemplate.NewRow();
            dtLabelTemplate.Rows.Add("WL-1050", "WL-1050 1.831\"x0.5\" Rounded 64 stickers per sheet");
            dtLabelTemplate.NewRow();
            dtLabelTemplate.Rows.Add("WL-800", "WL-800 2.5\"x1.563\" Rounded 18 stickers per sheet");
            dtLabelTemplate.NewRow();
            dtLabelTemplate.Rows.Add("WL-75", "WL-75 4\"x1\" Rounded 20 stickers per sheet");
            dtLabelTemplate.NewRow();
            dtLabelTemplate.Rows.Add("WL-200", "WL-200 3.43\"x0.67\" Rounded 30 stickers per sheet");
            dtLabelTemplate.NewRow();
            dtLabelTemplate.Rows.Add("WL-1075", "WL-1075 1.42\"x1\" Square Corner 66 stickers per sheet");
            dtLabelTemplate.NewRow();
            dtLabelTemplate.Rows.Add("WL-1100", "WL-1100 1.5\"x0.5\" Square Corner 100 stickers per sheet");
            dtLabelTemplate.NewRow();
            dtLabelTemplate.Rows.Add("WL-975", "WL-975 1.5\"x1\" Square Corner 50 stickers per sheet");
            dtLabelTemplate.NewRow();
            dtLabelTemplate.Rows.Add("WL-900", "WL-900 2.83\"x1\" Square Corner 33 stickers per sheet");
            dtLabelTemplate.NewRow();
            dtLabelTemplate.Rows.Add("WL-750", "WL-750 2.83\"x2.2\" Square Corner 15 stickers per sheet");
            dtLabelTemplate.NewRow();
            dtLabelTemplate.Rows.Add("WL-1025", "WL-1025 1\" Round Label 63");
            dtLabelTemplate.NewRow();
            dtLabelTemplate.Rows.Add("WL-325", "WL-325 1.67\" Round Label 24 stickers per sheet");
            dtLabelTemplate.NewRow();
            dtLabelTemplate.Rows.Add("WL-5375", "WL-5375 2\" Round Label 20 stickers per sheet");

            this.cmbLableDimension.DataSource = dtLabelTemplate;
            this.cmbLableDimension.SelectedValue = Configuration.oLPSetup.LabelTemplate;
            dtLabelSetting.Columns[0].Width = 20;
            dtLabelSetting.Columns[1].ReadOnly = true;
            dtLabelSetting.Columns[1].Width = 150;
        }

        private bool PrepareLabelReportData(DataSet oDs)
        {
            bool RetVal = true;
            bool is4objectonly = false;
            SetLabelPrintingSetup();

            switch (Configuration.oLPSetup.LabelTemplate)
            {
                case "WL-875"://6 objects 

                case "WL-5375"://6 objects

                case "WL-750"://6 objects

                case "WL-900"://6 objects

                case "WL-975"://6 objects

                case "WL-200"://6 objects

                case "WL-1075"://6 objects

                case "WL-75"://6 objects

                case "WL-1050"://6 objects

                case "WL-800"://6 objects

                case "WL-25"://6 objects

                case "WL-1000"://6 objects

                case "WL-1100"://6 Objects
                    RetVal = true;
                    break;
                case "WL-325"://4 Objects - (Item Id, Item Description, Item Id Barcode, Item Selling Price)
                case "WL-1025"://4 Objects - (Item Id, Item Description, Item Id Barcode, Item Selling Price)
                    if (GetNoOFObjectSelected() > 4)
                    {
                        clsUIHelper.ShowErrorMsg("Selected template allows to print maximum 4 objects.\n(Which allows Item Id, Item Description, Item Id Barcode, Item Selling Price) \nPlease select only 4 or less objects.");
                        RetVal = false;
                        dtLabelSetting.Rows[2].Cells[0].ReadOnly = true;
                        dtLabelSetting.Rows[3].Cells[0].ReadOnly = true;
                        return RetVal;
                    }
                    RetVal = true;
                    dtLabelSetting.Rows[2].Cells[0].ReadOnly = false;
                    dtLabelSetting.Rows[3].Cells[0].ReadOnly = false;
                    is4objectonly = true;
                    break;
            }
            for (int RowIndex = 0; RowIndex < oDs.Tables[0].Rows.Count; RowIndex++)
            {
                try
                {
                    if (Configuration.oLPSetup.PrintItemID == false)
                    {
                        if (is4objectonly == true && Configuration.oLPSetup.PrintItemVendorID == true)
                        {
                            oDs.Tables[0].Rows[RowIndex]["Itemid"] = oDs.Tables[0].Rows[RowIndex]["VendorItemID"];
                        }
                        else
                        {
                            oDs.Tables[0].Rows[RowIndex]["Itemid"] = "";
                        }
                    }
                    if (Configuration.oLPSetup.PrintItemIDBarcode == false)
                    {
                        if (is4objectonly == true && Configuration.oLPSetup.PrintItemVendorIDBarcode == true)
                        {
                            oDs.Tables[0].Rows[RowIndex]["picture"] = oDs.Tables[0].Rows[RowIndex]["VendorItemIDBarcode"];
                        }
                        else
                        {
                            oDs.Tables[0].Rows[RowIndex]["picture"] = null;
                        }
                    }
                    if (Configuration.oLPSetup.PrintDescription == false)
                    {
                        oDs.Tables[0].Rows[RowIndex]["Description"] = "";
                    }
                    if (Configuration.oLPSetup.PrintItemPrice == false)
                    {
                        oDs.Tables[0].Rows[RowIndex]["SellingPrice"] = "";
                    }
                    if (Configuration.oLPSetup.PrintItemVendorIDBarcode == false)
                    {
                        oDs.Tables[0].Rows[RowIndex]["VendorItemIDBarcode"] = null;
                    }
                    if (Configuration.oLPSetup.PrintItemVendorID == false)
                    {
                        oDs.Tables[0].Rows[RowIndex]["VendorItemID"] = "";
                    }
                    if (Configuration.oLPSetup.ManufacturerName == false)
                    {
                        oDs.Tables[0].Rows[RowIndex]["ManufacturerName"] = "";
                    }
                    if (Configuration.oLPSetup.AvgPrice == false)
                    {
                        oDs.Tables[0].Rows[RowIndex]["AvgPrice"] = "";
                    }
                    if (Configuration.oLPSetup.OnSalePrice == false)
                    {
                        oDs.Tables[0].Rows[RowIndex]["OnSalePrice"] = "";
                    }
                    if (Configuration.oLPSetup.ProductCode == false)
                    {
                        oDs.Tables[0].Rows[RowIndex]["SKUCode"] = "";
                    }
                    if (Configuration.oLPSetup.PrintVendorName == false)    //PRIMEPOS-2758 12-Nov-2019 JY Added
                    {
                        oDs.Tables[0].Rows[RowIndex]["VendorName"] = "";
                    }
                }
                catch { continue; }
            }
            return RetVal;
        }

        private int GetNoOFObjectSelected()
        {
            int SelectedObjects = 0;
            int rowIndex = 0;
            int rowsCount = dtLabelSetting.Rows.Count;
            try
            {
                if (rowsCount > 0)
                {
                    for (rowIndex = 0; rowIndex < rowsCount; rowIndex++)
                    {
                        if (Configuration.convertNullToBoolean(dtLabelSetting.Rows[rowIndex].Cells[0].Value) == true)
                        {
                            SelectedObjects++;
                        }
                    }
                }
            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
            return SelectedObjects;
        }
        private ReportClass CPrepareTemplateObject(string Key)
        {
            ReportClass oRpt = null;
            switch (Key)
            {
                case "WL-875"://6 objects 
                    oRpt = new rptItemLabelTemplateWLOL875();

                    break;
                case "WL-5375"://6 objects
                    oRpt = new rptItemLabelTemplateWLOL5375();
                    break;
                case "WL-750"://6 objects
                    oRpt = new rptItemLabelTemplateWLOL750();
                    break;
                case "WL-900"://6 objects
                    oRpt = new rptItemLabelTemplateWLOL900();
                    break;
                case "WL-975"://6 objects
                    oRpt = new rptItemLabelTemplateWLOL975();
                    break;
                case "WL-200"://6 objects
                    oRpt = new rptItemLabelTemplateWLOL200();
                    break;
                case "WL-1075"://6 objects
                    oRpt = new rptItemLabelTemplateWLOL1075();
                    break;
                case "WL-75"://6 objects
                    oRpt = new rptItemLabelTemplateWLOL75();
                    break;
                case "WL-1050"://6 objects
                    oRpt = new rptItemLabelTemplateWLOL1050();
                    break;
                case "WL-800"://6 objects
                    oRpt = new rptItemLabelTemplateWLOL800();
                    break;
                case "WL-25"://6 objects
                    oRpt = new rptItemLabelTemplateWLOL25();
                    break;
                case "WL-1000"://6 objects
                    oRpt = new rptItemLabelTemplateWLOL1000();
                    break;
                case "WL-1100"://6 Objects
                    oRpt = new rptItemLabelTemplateWLOL1100();
                    break;
                case "WL-325"://4 Objects
                    oRpt = new rptItemLabelTemplateWLOL325();
                    break;
                case "WL-1025"://3 Objects
                    oRpt = new rptItemLabelTemplateWLOL1025();
                    break;
            }
            
            return oRpt;
        }
        private void cmbLableDimension_DisplayMemberChanged(object sender, EventArgs e)
        {
            if (this.cmbLableDimension.SelectedValue != null)
            {
                CPrepareTemplateObject(this.cmbLableDimension.SelectedValue.ToString());
            }
        }
        private void chkUseLabelPrinter_CheckedChanged(object sender, EventArgs e)
        {
            //this.btnViewLable.Enabled = this.chkUseLabelPrinter.Checked == false; //PRIMEPOS-06012017 01-Jun-2017 JY Commented 
        }

        private void btnSelectObjet_Click(object sender, EventArgs e)
        {
           
            if (grbLableObject.Visible == false)
                LabelObjects_Expand(true);
            else
                LabelObjects_Expand(false);
            SetLabelPrintingSetup();
        }

        private void LabelObjects_Expand(bool Value)
        {
            if (Value == true)
            {
                if (grbLableObject.Visible == false)
                {
                    dtLabelSetting.Visible = true;
                    grbLableObject.Visible = true;
                    dtLabelSetting.Height = 150;
                    GetLabelPrintingSetup();
                    grbLableObject.Height = dtLabelSetting.Height + 50;
                    chkSelectAllObject.Location = new Point(chkSelectAllObject.Location.X, dtLabelSetting.Height + 20);
                    grbLableObject.Focus();
                }
            }
            else if (grbLableObject.Visible == true)
            {
                dtLabelSetting.Visible = false;
                grbLableObject.Visible = false;
            }
        }

        void chkSelectAllObject_CheckedChanged(object sender, EventArgs e)
        {
            int rowIndex = 0;
            int rowsCount = dtLabelSetting.Rows.Count;
            try
            {
                if (rowsCount > 0)
                {
                    for (rowIndex = 0; rowIndex < rowsCount; rowIndex++)
                    {
                        if (chkSelectAllObject.Checked == true)
                        {
                            dtLabelSetting.Rows[rowIndex].Cells[0].Value = true;

                            if (Configuration.oLPSetup.LabelTemplate == "WL-325" || Configuration.oLPSetup.LabelTemplate == "WL-1025")
                            {
                                if (rowIndex == 4 || rowIndex == 5 || rowIndex == 6)
                                {
                                    dtLabelSetting.Rows[rowIndex].Cells[0].Value = false;
                                }
                            }
                        }
                        else
                        {
                            dtLabelSetting.Rows[rowIndex].Cells[0].Value = false;
                        }
                    }

                    if (chkSelectAllObject.Checked == true)
                        chkSelectAllObject.Text = "Unselect All";
                    else
                        chkSelectAllObject.Text = "Select All";
                }
            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }

        private void cmbLableDimension_TextChanged(object sender, EventArgs e)
        {
            if (this.cmbLableDimension.SelectedValue != null)
            {
                oTemplateRptObj = CPrepareTemplateObject(this.cmbLableDimension.SelectedValue.ToString());
            }
        }

        #endregion

        private void frmRptItemPriceLogLable_FormClosing(object sender, FormClosingEventArgs e)
        {
            SetLabelPrintingSetup();
            Configuration.SaveLabelPrintingSetup(Configuration.oLPSetup);
        }

        private void dtLabelSetting_Leave(object sender, EventArgs e)
        {
            SetLabelPrintingSetup();
        }

        #region PRIMEPOS-06012017 02-Jun-2017 JY Added
        private void grdSearch_CellChange(object sender, CellEventArgs e)
        {
            if (e.Cell.Value != e.Cell.OriginalValue)
                grdSearch_AfterCellUpdate(sender, e);
        }
        #endregion

        #region PRIMEPOS-2464 10-Mar-2020 JY Added
        public void tmrBlinkingTimedEvent(object source, ElapsedEventArgs e)
        {
            try
            {
                iBlinkCnt++;
                if (iBlinkCnt % 4 == 0)
                    lblMessage.Appearance.ForeColor = Color.Transparent;
                else
                    lblMessage.Appearance.ForeColor = Color.Red;
            }
            catch (Exception Ex)
            {
            }
        }
        #endregion

        #region PRIMEPOS-2464 06-Mar-2020 JY Added
        private void CmbPriceType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (UserPriviliges.IsUserHasPriviliges(UserPriviliges.Modules.InventoryMgmt.ID, UserPriviliges.Screens.ItemFile.ID, UserPriviliges.Permissions.DisplayItemCost.ID) == false)
            {
                if (CmbPriceType.SelectedIndex == 0)
                {
                    string strMsg = "We couldn't select Cost Price as \"Display Item Cost\" user-level settings turned off";
                    clsUIHelper.ShowWarningMsg(strMsg);
                    CmbPriceType.SelectedIndex = 1;
                    return;
                }
            }
        }
        #endregion

        #region PRIMEPOS-2397 17-Nov-2020 JY Added
        private bool ShowData()
        {
            try
            {
                IDbCommand cmd = DataFactory.CreateCommand();
                string strSQL = string.Empty;

                using (SqlConnection conn = new SqlConnection(Configuration.ConnectionString))
                {
                    try
                    {
                        if (InvRecievedID > 0)
                        {
                            strSQL = "SELECT DISTINCT(I.ItemID) AS 'Item ID', V.VENDORID, IV.VendorItemID, I.Description AS 'Item Name'," +
                            " I.SellingPrice AS 'Selling Price', IV.VendorCostPrice AS 'Cost Price', V.VENDORNAME AS 'Vendor Name'," +
                            " I.PREFERREDVENDOR AS 'PreferredVendor', ISNULL(I.ProductCode, '') AS 'ProductCode', I.Location AS 'Location'," +
                            " ISNULL(I.ManufacturerName, '') AS 'ManufacturerName', ISNULL(I.AvgPrice, 0.0) AS 'AvgPrice'," +
                            " ISNULL(I.OnSalePrice, 0.0) AS 'OnSalePrice', ISNULL(I.isOnSale, 0) AS isOnSale, ISNULL(I.SaleLimitQty, 0) AS SaleLimitQty," +
                            " I.SaleStartDate, I.SaleEndDate FROM InventoryRecieved IR" +
                            " INNER JOIN InvRecievedDetail IRD ON IR.InvRecievedID = IRD.InvRecievedID" +
                            " INNER JOIN Item I ON IRD.ItemID = I.ItemID" +
                            " LEFT JOIN ItemVendor IV ON IV.ItemID = I.ItemID AND IV.VendorID = IR.VendorID" +
                            " LEFT JOIN VENDOR V ON V.VENDORID = IV.VendorID" +
                            " WHERE IR.InvRecievedID = " + InvRecievedID;
                        }
                        else if (ItemID != string.Empty)    //PRIMEPOS-2243 27-Apr-2021 JY Added else part
                        {
                            strSQL = " SELECT distinct(Item.itemID) as 'Item ID',ItemVendor.VendorItemID,Item.Description as 'Item Name'" +
                            ",Item.sellingPrice as 'Selling Price',ItemVendor.vendorCostPrice as 'Cost Price',Vendor.VendorName as 'Vendor Name'" +
                            ",Item.PreferredVendor as 'PreferredVendor',ISNULL(Item.ProductCode,'') as 'ProductCode',Item.Location as 'Location'" +
                            ",ISNULL(Item.ManufacturerName,'') as 'ManufacturerName',ISNULL(Item.AvgPrice,0.0) as 'AvgPrice',ISNULL(Item.OnSalePrice,0.0) as 'OnSalePrice'" +
                            ",ISNULL(Item.isOnSale,0) AS isOnSale,ISNULL(Item.SaleLimitQty,0) AS SaleLimitQty,Item.SaleStartDate,Item. SaleEndDate" +
                            " FROM Item LEFT JOIN ItemVendor ON ItemVendor.ItemID = Item.ItemID" +
                            " Left Join Vendor ON vendor.VendorID = ItemVendor.VendorID " +
                            " WHERE Item.ItemID in (" + ItemID + ")";

                            //if (VendorID > 0)
                            //{
                            //    strSQL += " AND vendor.VendorID = " + VendorID;
                            //}
                        }

                        ds = DataHelper.ExecuteDataset(conn, CommandType.Text, strSQL);
                        RowCount = ds.Tables[0].Rows.Count;
                        grdSearch.DataSource = ds;

                        try
                        {
                            if (nDisplayItemCost == 0)
                                grdSearch.DisplayLayout.Bands[0].Columns["Cost Price"].Hidden = true;
                        }
                        catch { }
                    }
                    catch (Exception exp)
                    {
                        throw (exp);
                    }
                }
            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
            return true;
        }
        #endregion
    }
}