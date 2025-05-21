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
using System.Text;
using Resources;
using POS_Core.ErrorLogging;
using POS_Core.Resources;

namespace POS_Core_UI.Reports.ReportsUI
{
    public partial class frmRptItemDepartment : Form
    {
        private DataSet ds;   
        private IDataAdapter da;
        private POSSET oPOSSet;

        private bool m_exceptionAccoured = false;
        public frmRptItemDepartment()
        {
            InitializeComponent();
        }
        
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmRptItemPriceLogLable_Load(object sender, EventArgs e)
        {
            /*ds = new DataSet();
            DataTable dt = new DataTable();
            dt.Columns.Add(new DataColumn("Item#", typeof(string)));
            dt.Columns.Add(new DataColumn("Item Description", typeof(string)));
            dt.Columns.Add(new DataColumn("Vendor Name", typeof(string)));
            dt.Columns.Add(new DataColumn("Department", typeof(string)));

            DataRow dr = dt.NewRow();
            dr["Item#"] = "";
            dr["Item Description"] = "";
            dr["Vendor Name"] = "";
            dr["Department"] = "";
            dt.Rows.Add(dr);
            ds.Tables.Add(dt);*/

            ApplyGrigFormat();

            oPOSSet = Configuration.CPOSSet;
            //ds = new DataSet();   //Sprint-18 - 2144 09-Dec-2014 JY commented  which is not required
            da = DataFactory.CreateDataAdapter();
            txtItemCode.Focus();
            //Mabry.Windows.Forms.Barcode.Licenser.Key = "E1P8-HKELVF8R04Q0";
            clsUIHelper.setColorSchecme(this);
            clsUIHelper.SetKeyActionMappings(this.grdSearch);
            //grdSearch.DisplayLayout.Bands[0].Columns["Check Items"].Header.VisiblePosition = 0;                      
                
            this.txtDeptCode.AfterEnterEditMode += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
            this.txtDeptCode.AfterExitEditMode += new System.EventHandler(clsUIHelper.AfterExitEditMode);           
        }                                 

        private void txtDeptCode_EditorButtonClick(object sender, Infragistics.Win.UltraWinEditors.EditorButtonEventArgs e)
        {
            //frmSearch oSearch = new frmSearch(clsPOSDBConstants.Department_tbl);
            frmSearchMain oSearch = new frmSearchMain(true);    //20-Dec-2017 JY Added new reference
            oSearch.SearchTable = clsPOSDBConstants.Department_tbl; //20-Dec-2017 JY Added 
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
                //txtDeptCode.Text = oSearch.SelectedCode();
            }
        }

        private void txtDeptCode_Validating(object sender, CancelEventArgs e)
        {
            if (txtDeptCode.Text.Trim() == "")
            {
                txtDeptCode.Tag = null;
            }
        }

        #region Sprint-18 - 2144 09-Dec-2014 JY commented
        //private void butShowItems_Click(object sender, EventArgs e)
        //{
        //    int rowIndex = 0;                        
        //    bool ByDepartment = false ;
        //  //  this.grdSearch.col [rowIndex].Cells["Check Items"].Value = f
        //    if (ShowData(true, false, ByDepartment))
        //    {
        //        foreach (DataRow oRow in ds.Tables[0].Rows)
        //        {
        //            this.grdSearch.Rows[rowIndex].Cells["Check Items"].Value = false;                   
        //            rowIndex++;
        //        }
        //        grdSearch.DisplayLayout.Bands[0].Columns["Vendor Name"].Header.VisiblePosition = 3;
        //    }            
        //}
        #endregion

        #region Sprint-18 - 2144 09-Dec-2014 JY commented below code which is not required
        //private bool ShowData(bool PrintId, bool AddItems, bool byDepatment)
        //{
        //    try
        //    {                
        //        IDbCommand cmd = DataFactory.CreateCommand();
        //        string sSQL = "";                             
        //        IDbConnection conn = DataFactory.CreateConnection();
        //        conn.ConnectionString = Configuration.ConnectionString;
        //        conn.Open();

        //        try
        //        {
        //            sSQL = "  SELECT distinct(    dbo.Item.ItemID) as 'Item Id', dbo.Item.Description as 'Item Name', "
        //                 + " dbo.Department.DeptName as 'Department',dbo.Vendor.VendorName as 'Vendor Name'  "
        //                 + " FROM         dbo.Vendor RIGHT OUTER JOIN "
        //                 + " dbo.ItemVendor RIGHT OUTER JOIN "
        //                 + " dbo.Item ON dbo.ItemVendor.ItemID = dbo.Item.ItemID LEFT OUTER JOIN "
        //                 + " dbo.Department ON dbo.Item.DepartmentID = dbo.Department.DeptID "
        //                 + " ON dbo.Vendor.VendorID = dbo.ItemVendor.VendorID"
        //                 + " WHERE  1=1";

        //            if (txtItemCode.Text != string.Empty)
        //                sSQL = sSQL + " And Item.itemID like '" + txtItemCode.Text.Trim() + "%'";
        //            if (txtItemName.Text != string.Empty)
        //                sSQL = sSQL + " And Item.Description like '" + txtItemName.Text.Trim() + "%'";

        //            cmd.CommandType = CommandType.Text;
        //            cmd.CommandText = sSQL;
        //            cmd.Connection = conn;
        //            SqlDataAdapter sqlDa = (SqlDataAdapter)da;
        //            sqlDa.SelectCommand = (SqlCommand)cmd;
        //            DataSet tempds = new DataSet();


        //            da.Fill(tempds);
        //            conn.Close();
        //            if (ds.Tables.Count > 0)
        //            {
        //                ds.Tables[0].Merge(tempds.Tables[0]);
        //                #region Sprint-18 - 2127 03-Dec-2014 JY Added to avoid duplicates
        //                DataTable dt = ds.Tables[0].DefaultView.ToTable(true);
        //                ds.Clear();
        //                ds.Merge(dt);
        //                #endregion
        //            }
        //            else
        //            {
        //                ds.Merge(tempds);
        //            }

        //            grdSearch.DataSource = ds;
        //        }
        //        catch (NullReferenceException)
        //        {
        //            conn.Close();

        //        }
        //        catch (Exception exp)
        //        {
        //            conn.Close();
        //            throw (exp);
        //        } 
		
        //    }
        //    catch (Exception exp)
        //    {
        //        clsUIHelper.ShowErrorMsg(exp.Message);
        //    }
        //    return true;
        //}
        #endregion

        private void frmRptItemPriceLogLable_KeyUp(object sender, KeyEventArgs e)
        {

            //if (e.KeyData == System.Windows.Forms.Keys.F4)
            //{
            //    btnAddItem_Click(null, null);
            //}    

            //if (e.KeyData == System.Windows.Forms.Keys.F4)
            //{
            //    txtDeptCode_EditorButtonClick(null, null);
            //}                                               
        }

        bool SelectAllFlag = false;

        private void ultraButSelectAll_Click(object sender, EventArgs e)
        {
            #region Sprint-18 - 2144 09-Dec-2014 JY commented not in use
            //int rowIndex = 0;
            //if (grdSearch.Rows.Count > 0)
            //{
            //    if (SelectAllFlag == false)
            //    {
            //        foreach (DataRow oRow in ds.Tables[0].Rows)
            //        {
            //            this.grdSearch.Rows[rowIndex].Cells["Check Items"].Value = true;
            //            rowIndex++;
            //        }
            //        SelectAllFlag = true;
            //    }
            //    else
            //    {
            //        foreach (DataRow oRow in ds.Tables[0].Rows)
            //        {
            //            this.grdSearch.Rows[rowIndex].Cells["Check Items"].Value = false;
            //            rowIndex++;
            //        }
            //        SelectAllFlag = false;
            //    }
            //}
            #endregion
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
            #region Sprint-18 - 2144 09-Dec-2014 JY commented old code
            //if ((e.KeyData == Keys.Enter || e.KeyData == Keys.Tab || e.KeyData == Keys.Down || e.KeyData == Keys.Up))
            //    {
            //           try
            //            {
            //                if (e.KeyData == Keys.Down || e.KeyData == Keys.Up)
            //                {
            //                    int grdRowId = 0;
            //                    if (e.KeyData == Keys.Down)
            //                    {
            //                        this.grdSearch.PerformAction(Infragistics.Win.UltraWinGrid.UltraGridAction.NextRow);
            //                    }
            //                    else if (e.KeyData == Keys.Up)
            //                    {
            //                        this.grdSearch.PerformAction(Infragistics.Win.UltraWinGrid.UltraGridAction.PrevRow);
            //                    }

            //                    grdRowId = grdSearch.ActiveRow.Index;
            //                    this.grdSearch.Rows[grdRowId].Selected = true;
            //                    this.grdSearch.Rows[grdRowId].Activate();
            //                    this.grdSearch.ActiveRow.Cells["Check Items"].Activate();
            //                    this.grdSearch.PerformAction(UltraGridAction.EnterEditMode);
            //                }
            //            }
            //            catch (Exception Ex)
            //            {
            //            }
            //            e.Handled = true;
                    
            //    }
            //    if (e.KeyData == Keys.Space )
            //    {
            //        if (grdSearch.ActiveRow != null)
            //        {
            //            if (!this.grdSearch.ActiveRow.Cells["Check Items"].Activated)
            //            {
            //                this.grdSearch.ActiveRow.Cells["Check Items"].Activate();
            //                grdSearch_KeyDown(sender, e);
            //                this.grdSearch.PerformAction(UltraGridAction.EnterEditMode);
            //                if (Convert.ToBoolean(grdSearch.ActiveRow.Cells["Check Items"].Value) == true)
            //                    grdSearch.ActiveRow.Cells["Check Items"].Value = false;
            //                else
            //                    grdSearch.ActiveRow.Cells["Check Items"].Value = true;
            //            }
            //        }

            //    } 
            #endregion
        }

        #region Sprint-18 - 2144 09-Dec-2014 JY not in use
        //private void btnAddItem_Click(object sender, EventArgs e)
        //{
        //    int rowIndex = 0;            
        //    if (txtItemCode.Text == string.Empty && txtItemName.Text == string.Empty )
        //    {
        //        clsUIHelper.ShowErrorMsg("Please insert Item Id or Item Name.");
        //        txtItemCode.Focus();
        //        return;
        //    }

        //    #region Sprint-18 - 2127 03-Dec-2014 JY commented this code to avoid clearing the dataset
        //    //if (ds.Tables.Count != 0)
        //        //{
        //        //    if (ds.Tables[0].Rows.Count > 0)
        //        //    {
        //        //        ds.Tables[0].Clear();
        //        //    }
        //    //}
        //    #endregion

        //    if (ShowData(true,true,false))
        //    {
        //        if (ds.Tables[0].Rows.Count > 0)
        //        {
        //            foreach (DataRow oRow in ds.Tables[0].Rows)
        //            {
        //                this.grdSearch.Rows[rowIndex].Cells["Check Items"].Value = false;                      
        //                rowIndex++;
        //            }
        //        }
                
        //        grdSearch.DisplayLayout.Bands[0].Columns["Vendor Name"].Header.VisiblePosition = 3;
        //    }                     
        //    //txtItemCode.Text = string.Empty;
        //    //txtItemName.Text = string.Empty;
        //    txtItemCode.Focus();
        //}
        #endregion

        //Sprint-18 - 2144 09-Dec-2014 JY not in use
        private void txtItemCode_KeyDown(object sender, KeyEventArgs e)
        {
            ////changes by atul joshi on  3-12-2010
            //if (e.KeyData==System.Windows.Forms.Keys.F4 || e.KeyData == System.Windows.Forms.Keys.Enter)
            //{                
            //    btnAddItem_Click(sender, e);
            //}
        }

        //Sprint-18 - 2144 09-Dec-2014 JY not in use
        private void txtItemName_KeyDown(object sender, KeyEventArgs e)
        {
           // //changes by atul joshi on  3-12-2010 
           //if (e.KeyData == System.Windows.Forms.Keys.F4 || e.KeyData == System.Windows.Forms.Keys.Enter)            
           // {                
           //     btnAddItem_Click(sender, e);
           // }
        }                     

        private void grdSearch_Enter(object sender, EventArgs e)
        {
            //this.grdSearch.PerformAction(UltraGridAction.FirstCellInGrid);
            //this.grdSearch.PerformAction(UltraGridAction.EnterEditMode);
            //this.grdSearch.ActiveRow.Cells["Check Items"].Activate();

            try
            {
                if (this.grdSearch.Rows.Count > 0)
                {
                    this.grdSearch.PerformAction(UltraGridAction.EnterEditMode);
                }
                else
                {
                    this.grdSearch.Rows.Band.AddNew();
                }
            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }              
       
        bool ValueFlag = false;
        private void grdSearch_AfterCellUpdate(object sender, CellEventArgs e)
        {                       
            int LabelPerSheet = Configuration.LabelPerSheet;

            if (ValueFlag == true)
            {
                ValueFlag = false;
                return;
            }
                if (Convert.ToString(e.Cell.Value) == string.Empty)
                {
                    ValueFlag = true;
                    e.Cell.Value = e.Cell.OriginalValue;
                }
                else
                    ValueFlag = false;                                   
        }
       
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (txtDeptCode.Text == "")
            {
                clsUIHelper.ShowErrorMsg("Please Select Department.");
                txtDeptCode.Focus();
                return;
            }

            if (grdSearch.Rows.Count == 0)
            {
                clsUIHelper.ShowErrorMsg("Please Select Item.");
                return;
            }
            if (Resources.Message.Display("This will update the department for all the listed Items.\n Are you sure ?\n", "Add Items to the Department", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
            {
                return;
            }
            SqlConnection sqlcon = new SqlConnection(Configuration.ConnectionString);
            try
            {
                StringBuilder strBuildItem = new StringBuilder();
                Application.DoEvents();
                for (int GridRowNo = 0; GridRowNo < grdSearch.Rows.Count; GridRowNo++)
                {
                    //if (Convert.ToBoolean(grdSearch.Rows[GridRowNo].Cells["Check Items"].Value))
                    //{
                        if(strBuildItem.ToString()!="")
                            strBuildItem.Append(",");
                        strBuildItem.Append("'" + grdSearch.Rows[GridRowNo].Cells[clsPOSDBConstants.Item_Fld_ItemID].Value.ToString().Replace("'", "''") + "'");                      
                    //}
                }
                
                string strDeptName = string.Empty, strDeptId = string.Empty;
                DataTable dtDept = new DataTable();
                try
                {
                    SqlDataAdapter daDept = new SqlDataAdapter("select DeptID, DeptName from  Department where DeptCode='" + txtDeptCode.Text + "'", sqlcon);
                    daDept.Fill(dtDept);

                    if (dtDept.Rows.Count > 0)
                    {
                        strDeptId = dtDept.Rows[0]["DeptID"].ToString();
                        strDeptName = dtDept.Rows[0]["DeptName"].ToString();
                    }
                }
                catch (SqlException ex)
                { }
                finally
                {
                }

                string sSQL = "UPDATE Item SET DepartmentID = " + strDeptId + " WHERE ItemID IN (" + strBuildItem.ToString() + ")";
                DataHelper.ExecuteNonQuery(sqlcon, CommandType.Text, sSQL);

                /*sSQL = " SELECT * FROM  Item WHERE Item.itemID in (" + strBuildItem.ToString() + ")"; 
                SqlDataAdapter DaUpdateItems = new SqlDataAdapter(sSQL, Configuration.ConnectionString);                            
                DataTable dsItemUpdate = new DataTable();
                DaUpdateItems.Fill(dsItemUpdate);
                DaUpdateItems.FillSchema(dsItemUpdate, SchemaType.Source);                                
                SqlCommandBuilder sqlcmdbuilder = new SqlCommandBuilder(DaUpdateItems);
                */
                //DataRow dr;
                //for (int RowNo = 0; RowNo < dsItemUpdate.Rows.Count; RowNo++)
                //{
                //    dr = dsItemUpdate.Rows[RowNo];
                //    dr["DepartmentID"] = txtDeptCode.Text.ToString();
                //}
                //int Result = DaUpdateItems.Update(dsItemUpdate);
                

                //Sprint-18 - 2144 09-Dec-2014 JY commented below code which is not required
                /*DataSet dsTempClear = new DataSet();
                dsTempClear = ds.Copy();
                if (dsTempClear != null)
                {
                    if (ds.Tables.Count != 0)
                    {

                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            ds.Tables[0].Clear();
                        }
                    }
                }*/
                groupBox3.Focus();
                Application.DoEvents();
                //Boolean IsSave = ShowData(true, true, true);  //Sprint-18 - 2144 09-Dec-2014 JY commented below code which is not required
                //Sprint-18 - 2144 09-Dec-2014 JY Added to refresh grid
                for (int GridRowNo = 0; GridRowNo < grdSearch.Rows.Count; GridRowNo++)
                {
                    grdSearch.Rows[GridRowNo].Cells[clsPOSDBConstants.Department_Fld_DeptName].Value = strDeptName;
                }

                //Sprint-18 - 2144 09-Dec-2014 JY commented below code which is not required
                /*Application.DoEvents();
                int rowIndex = 0;                        
                foreach (DataRow oRow in ds.Tables[0].Rows)
                {
                    this.grdSearch.Rows[rowIndex].Cells["Check Items"].Value = false;
                    rowIndex++;
                }*/
                txtItemCode.Focus();
                Application.DoEvents();                
            }
            catch (Exception ex)
            {                
            }
           
        }

        private void btnClose_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnRemoveItems_Click(object sender, EventArgs e)
        {
            #region Sprint-18 - 2144 09-Dec-2014 JY commented code which is not required
            //DataSet dsTemp = new DataSet();
            //dsTemp = ds.Copy();            
            //if (dsTemp != null)
            //{
            //    if (ds.Tables.Count != 0)
            //    {

            //        if (ds.Tables[0].Rows.Count > 0)
            //        {
            //            ds.Tables[0].Clear();                       
            //        }
            //    }
            //}
            //txtItemCode.Focus();
            //txtItemCode.Text = string.Empty;
            //txtItemName.Text = string.Empty;
            #endregion
            //grdSearch.DataSource = ds;
            for (int i = grdSearch.Rows.Count-1; i >= 0; i--)
                grdSearch.Rows[i].Delete(false);
            grdSearch.Refresh();
            ApplyGrigFormat();
            groupBox3.Focus();
            txtDeptCode.Text = string.Empty;
        }

        private void txtDeptCode_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyData == System.Windows.Forms.Keys.F4)
            {
                txtDeptCode_EditorButtonClick(null, null);
            }
        }

        //Sprint-18 - 2144 09-Dec-2014 JY not in use
        private void txtItemCode_KeyUp(object sender, KeyEventArgs e)
        {
            //if (e.KeyData == System.Windows.Forms.Keys.F4)
            //{
            //    btnAddItem_Click(null, null);
            //}    

        }    
   
        //adde by Atul joshi 3-12-2010
        private void txtDeptCode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == System.Windows.Forms.Keys.F4 || e.KeyData == System.Windows.Forms.Keys.Enter)
            {
                //frmSearch oSearch = new frmSearch(clsPOSDBConstants.Department_tbl);
                frmSearchMain oSearch = new frmSearchMain(true);    //20-Dec-2017 JY Added new reference
                oSearch.SearchTable = clsPOSDBConstants.Department_tbl; //20-Dec-2017 JY Added 
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
        }

        //Sprint-18 - 2144 09-Dec-2014 JY Added 
        private void grdSearch_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyData == System.Windows.Forms.Keys.F4)
                {
                    if (this.grdSearch.ContainsFocus == true)
                    {
                        if (this.grdSearch.ActiveCell != null)
                        {
                            if (this.grdSearch.ActiveCell.Column.Key == clsPOSDBConstants.PODetail_Fld_ItemID)
                                this.SearchItem();
                        }
                    }
                }
                else if (e.KeyData == System.Windows.Forms.Keys.F2)
                {
                    AddRow();

                }
                e.Handled = true;
            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }

        //Sprint-18 - 2144 09-Dec-2014 JY Added to search item
        private void SearchItem()
        {
            try
            {
                //frmSearch oSearch = new frmSearch(clsPOSDBConstants.Item_tbl);
                frmSearchMain oSearch = new frmSearchMain(true);    //20-Dec-2017 JY Added new reference
                oSearch.SearchTable = clsPOSDBConstants.Item_tbl;  //20-Dec-2017 JY Added 
                oSearch.ShowDialog(this);
                if (!oSearch.IsCanceled)
                {
                    string strCode = oSearch.SelectedRowID();
                    if (strCode == "")
                        return;

                    FKEdit(strCode, clsPOSDBConstants.Item_tbl);
                }
            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }

        //Sprint-18 - 2144 09-Dec-2014 JY Added
        private void FKEdit(string code, string senderName)
        {
            if (senderName == clsPOSDBConstants.Item_tbl)
            {
                #region Items
                try
                {
                    Item oItem = new Item();
                    ItemData oItemData;
                    ItemRow oItemRow = null;
                    oItemData = oItem.Populate(code);
                    if (oItemData.Tables[0].Rows.Count == 0)
                    {
                        if (UserPriviliges.IsUserHasPriviliges(UserPriviliges.Modules.InventoryMgmt.ID, UserPriviliges.Screens.ItemFile.ID, -998))
                        {
                            frmItems ofrmItem = new frmItems(code);
                            ofrmItem.numQtyInStock.ReadOnly = true;
                            ofrmItem.numQtyInStock.Enabled = false;
                            ofrmItem.ShowDialog();
                            oItemData = oItem.Populate(code);
                        }
                    }
                    oItemRow = oItemData.Item[0];
                    if (oItemRow != null)
                    {
                        if (grdSearch.ActiveRow == null)
                            this.grdSearch.Rows.Band.AddNew();
                        this.grdSearch.ActiveCell.Value = oItemRow.ItemID;
                        this.grdSearch.ActiveRow.Cells[clsPOSDBConstants.Item_Fld_Description].Value = oItemRow.Description;
                        string strDept, strVendorName;
                        GetItemDetails(oItemRow.ItemID, out strDept, out strVendorName);
                        this.grdSearch.ActiveRow.Cells[clsPOSDBConstants.Vendor_Fld_VendorName].Value = strVendorName;
                        this.grdSearch.ActiveRow.Cells[clsPOSDBConstants.Department_Fld_DeptName].Value = strDept;
                    }
                }
                catch (System.IndexOutOfRangeException)
                {
                    this.grdSearch.ActiveCell.Value = String.Empty;
                    this.grdSearch.ActiveRow.Cells["Description"].Value = String.Empty;
                }
                catch (Exception exp)
                {
                    clsUIHelper.ShowErrorMsg(exp.Message);
                    this.grdSearch.ActiveCell.Value = String.Empty;
                    this.grdSearch.ActiveRow.Cells["Description"].Value = String.Empty;
                }
                #endregion
            }
        }

        //Sprint-18 - 2144 09-Dec-2014 JY Added to get item details
        private void GetItemDetails(string strItemCode, out string strDept, out string strVendor)
        {
            strDept = String.Empty;
            strVendor = String.Empty;
            try
            {
                IDbCommand cmd = DataFactory.CreateCommand();
                string sSQL = "";
                IDbConnection conn = DataFactory.CreateConnection();
                conn.ConnectionString = Configuration.ConnectionString;
                DataSet tempds = new DataSet();
                try
                {
                    sSQL = "  SELECT distinct dbo.Department.DeptName, dbo.Vendor.VendorName "
                        + " FROM dbo.Vendor RIGHT OUTER JOIN "
                         + " dbo.ItemVendor RIGHT OUTER JOIN "
                         + " dbo.Item ON dbo.ItemVendor.ItemID = dbo.Item.ItemID LEFT OUTER JOIN "
                         + " dbo.Department ON dbo.Item.DepartmentID = dbo.Department.DeptID "
                         + " ON dbo.Vendor.VendorID = dbo.ItemVendor.VendorID"
                         + " WHERE dbo.Item.ItemID = '" + strItemCode + "'";

                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = sSQL;
                    cmd.Connection = conn;
                    SqlDataAdapter sqlDa = (SqlDataAdapter)da;
                    sqlDa.SelectCommand = (SqlCommand)cmd;
                    da.Fill(tempds);
                    if (tempds.Tables.Count > 0)
                    {
                        if (tempds.Tables[0].Rows.Count > 0)
                        {
                            strDept = tempds.Tables[0].Rows[0]["DeptName"].ToString();
                            strVendor = tempds.Tables[0].Rows[0]["VendorName"].ToString();
                        }
                    }
                }
                catch (Exception exp)
                {
                    throw (exp);
                }

            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }

        //Sprint-18 - 2144 09-Dec-2014 JY Added
        private void AddRow()
        {
            try
            {
                this.grdSearch.Focus();
                this.grdSearch.PerformAction(UltraGridAction.LastCellInGrid);
                this.grdSearch.PerformAction(UltraGridAction.FirstCellInRow);
                this.grdSearch.PerformAction(UltraGridAction.EnterEditMode);
            }
            catch (Exception) { }
        }

        //Sprint-18 - 2144 09-Dec-2014 JY Added
        private void ApplyGrigFormat()
        {
            clsUIHelper.SetAppearance(this.grdSearch);

            this.grdSearch.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.Item_Fld_ItemID].MaxLength = 20;
            this.grdSearch.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.Item_Fld_ItemID].Header.Caption = "Item#";

            this.grdSearch.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.Item_Fld_Description].MaxLength = 255;
            this.grdSearch.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.Item_Fld_Description].Header.Caption = "Item Description";
            this.grdSearch.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.Item_Fld_Description].CellActivation = Activation.NoEdit;
            this.grdSearch.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.Item_Fld_Description].Width = 250;

            this.grdSearch.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.Vendor_Fld_VendorName].MaxLength = 255;
            this.grdSearch.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.Vendor_Fld_VendorName].Header.Caption = "Vendor Name";
            this.grdSearch.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.Vendor_Fld_VendorName].CellActivation = Activation.NoEdit;
            this.grdSearch.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.Vendor_Fld_VendorName].Width = 250;

            this.grdSearch.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.Department_Fld_DeptName].MaxLength = 255;
            this.grdSearch.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.Department_Fld_DeptName].Header.Caption = "Department";
            this.grdSearch.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.Department_Fld_DeptName].CellActivation = Activation.NoEdit;
            this.grdSearch.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.Department_Fld_DeptName].Width = 250;
        }

        //Sprint-18 - 2144 09-Dec-2014 JY  Added
        private void grdSearch_ClickCellButton(object sender, CellEventArgs e)
        {
            try
            {
                if (m_exceptionAccoured)
                {
                    m_exceptionAccoured = false;
                    return;
                }

                if (e.Cell.Column.Key == clsPOSDBConstants.Item_Fld_ItemID)
                    SearchItem();
            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }

        //Sprint-18 - 2144 09-Dec-2014 JY Added
        private void grdSearch_BeforeCellDeactivate(object sender, CancelEventArgs e)
        {
            UltraGridCell oCurrentCell;
            oCurrentCell = this.grdSearch.ActiveCell;
            try
            {
                if (oCurrentCell.DataChanged == false)
                    return;
            }
            catch (Exception ex)
            {
            }
            try
            {
                if (oCurrentCell.Column.Key == clsPOSDBConstants.Item_Fld_ItemID && oCurrentCell.Value.ToString() != "")
                {
                    FKEdit(oCurrentCell.Value.ToString(), clsPOSDBConstants.Item_tbl);
                    if (oCurrentCell.Value.ToString() == "")
                    {
                        e.Cancel = true;
                        this.grdSearch.PerformAction(UltraGridAction.EnterEditMode);
                    }
                }
            }
            catch (Exception exp)
            {
                m_exceptionAccoured = true;
                clsUIHelper.ShowErrorMsg(exp.Message);
                e.Cancel = true;
                this.grdSearch.PerformAction(UltraGridAction.EnterEditMode);
            }
        }

        //Sprint-18 - 2144 09-Dec-2014 JY Added
        private void ValidateRow(object sender, CancelEventArgs e)
        {
            UltraGridRow oCurrentRow;
            UltraGridCell oCurrentCell;
            oCurrentRow = this.grdSearch.ActiveRow;
            oCurrentCell = null;
            bool blnCellChanged;
            blnCellChanged = false;

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
                return;
            }
            try
            {
                oCurrentCell = oCurrentRow.Cells[clsPOSDBConstants.Item_Fld_ItemID];
                Validate_ItemID(oCurrentCell.Text.ToString());
            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
                if (oCurrentCell != null)
                {
                    m_exceptionAccoured = true;
                    e.Cancel = true;
                    this.grdSearch.ActiveCell = oCurrentCell;
                    this.grdSearch.PerformAction(UltraGridAction.ActivateCell);
                    this.grdSearch.PerformAction(UltraGridAction.EnterEditMode);

                }
            }	
        }

        //Sprint-18 - 2144 09-Dec-2014 JY Added 
        public void Validate_ItemID(string strValue)
        {
            if (strValue.Trim() == "" || strValue == null)
            {
                ErrorHandler.throwCustomError(POSErrorENUM.InvRecvDetail_ItemCodeCanNotNull);
            }
            else
            {
                ItemData oID = new ItemData();
                Item oI = new Item();
                oID = oI.Populate(strValue);
                if (oID == null)
                {
                    throw (new Exception("Invalid Item code"));
                }
                else if (oID.Item.Rows.Count == 0)
                {
                    throw (new Exception("Invalid Item code"));
                }
            }
        }

        private void btnDeleteItem_Click(object sender, EventArgs e)
        {
            if (this.grdSearch.ActiveRow != null)
            {
                if (Resources.Message.Display("Are you sure, you want to delete the item?", "Delete Item", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                {
                    this.grdSearch.ActiveRow.Delete(false);
                }
            }
        }
    }
}