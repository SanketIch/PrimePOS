using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using POS_Core.CommonData.Rows;
using POS_Core.CommonData;
using POS_Core.BusinessRules;
using POS_Core.ErrorLogging;
using System.Diagnostics;
using Infragistics.Win;
using Infragistics.Win.UltraWinTabs;
using Infragistics.Win.UltraWinTabControl;
using System.Data;
//using POS_Core.DataAccess;
using POS_Core.DataAccess;


namespace POS_Core_UI
{
    public partial class frmItemAdvSearch : Form
    {
        public bool IsCanceled = true;
        public DataSet itemData = new DataSet();
        private ItemSvr itemSvr = new ItemSvr();
        //private frmSearch oSearch;
        private frmSearchMain oSearch;
        private TaxCodesData oTaxcodeData = new TaxCodesData();
        private TaxCodes oBRTaxCodes = new TaxCodes();

        public frmItemAdvSearch()
        {
            InitializeComponent();
            #region Sprint-21 - 2206 07-Mar-2016 JY Added
            this.dtpExpiryDate1.Value = DateTime.Today.Date.Subtract(new System.TimeSpan(7, 0, 0, 0));
            this.dtpExpiryDate2.Value = DateTime.Today;
            cboExpiryDate.SelectedIndex = 0;
            #endregion
            cboTaxable.SelectedIndex = 0;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            try
            {
                bool isVendRequired = false;
                bool isItemVendorRequired = false;
                string WhereClause1 = string.Empty; //19-Jun-2015 JY Added WhereClause1 
                string WhereClause = BuildWhereClause(ref isVendRequired, ref isItemVendorRequired, ref WhereClause1);  //19-Jun-2015 JY Added WhereClause1 parameter
                itemData = itemSvr.PopulateAdvSearch(WhereClause, ref isVendRequired, ref isItemVendorRequired, WhereClause1);    //19-Jun-2015 JY Added WhereClause1 parameter
                this.IsCanceled = false;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private string BuildWhereClause(ref bool isVendRequired, ref bool isItemVendorRequired, ref string WhereQuery1)
        {
            string WhereQuery = "";

            if (txtVendorCode.Text != "")
            {
                WhereQuery = " V." + clsPOSDBConstants.Vendor_Fld_VendorCode + " like '" + txtVendorCode.Text + "' AND";
                isVendRequired = true;
            }
            if (txtVendorItemCode.Text != "")
            {
                WhereQuery += " IV." + clsPOSDBConstants.ItemVendor_Fld_VendorItemID + " like '" + txtVendorItemCode.Text + "%' AND";
                isItemVendorRequired = true;
            }
            if (txtSkuCode.Text != "")
                WhereQuery += " I."+clsPOSDBConstants.Item_Fld_ProductCode+" like '" + txtSkuCode.Text + "%' AND";
            if(txtDepartment.Text!="")
                WhereQuery += " I."+clsPOSDBConstants.Item_Fld_DepartmentID+" like '" + txtDepartment.Tag + "' AND";
            if (txtSubDepartment.Text != "")
                WhereQuery += " I." + clsPOSDBConstants.Item_Fld_SubDepartmentID + " like '" + txtSubDepartment.Tag + "' AND";
            if (txtTaxCode.Text != "")
            {
                //WhereQuery += " I." + clsPOSDBConstants.Item_Fld_TaxID + " LIKE '" + txtTaxCode.Tag + "' AND";    //PRIMEPOS-3000 25-Oct-2021 JY Commented
                #region PRIMEPOS-3000 25-Oct-2021 JY Added
                if (cboTaxable.SelectedIndex == 0 || cboTaxable.SelectedIndex == 1)
                {
                    WhereQuery += " I.ItemID IN (SELECT ItemID FROM " +
                                            "(" +
                                            "SELECT a.ItemID, a.TaxPolicy, b.EntityType, b.TaxID FROM Item a" +
                                            " INNER JOIN ItemTax b ON a.ItemID = b.EntityID AND b.EntityType = 'I'" +
                                            " WHERE a.TaxPolicy = 'I'" +
                                            " UNION" +
                                            " SELECT a.ItemID, a.TaxPolicy, b.EntityType, b.TaxID FROM Item a" +
                                            " INNER JOIN ItemTax b ON a.DepartmentID = b.EntityID AND b.EntityType = 'D'" +
                                            " WHERE a.TaxPolicy = 'D'" +
                                            " UNION" +
                                            " SELECT ItemID, TaxPolicy, EntityType, TaxID FROM" +
                                                " (SELECT RANK() OVER(PARTITION BY ItemID  ORDER BY EntityType ASC) AS Rnk, * FROM" +
                                                    " (SELECT a.ItemID, a.TaxPolicy, b.EntityType, b.TaxID FROM Item a INNER JOIN ItemTax b ON a.DepartmentID = b.EntityID AND b.EntityType = 'D' WHERE a.TaxPolicy = 'O'" +
                                                        " UNION SELECT a.ItemID, a.TaxPolicy, c.EntityType, c.TaxID FROM Item a INNER JOIN ItemTax c ON a.ItemID = c.EntityID AND c.EntityType = 'I' WHERE a.TaxPolicy = 'O'" +
                                                    ") x" +
                                                ") y WHERE Rnk = 1" +
                                            ") z WHERE TaxID = '" + txtTaxCode.Tag.ToString().Trim().Replace("'", "''") + "') AND ";
                }
                else
                {
                    WhereQuery += "1=2 AND ";
                }
                #endregion
            }
            #region PRIMEPOS-3000 25-Oct-2021 JY Added
            else
            {
                if (cboTaxable.SelectedIndex == 1)
                {
                    WhereQuery += " I.ItemID IN (SELECT ItemID FROM " +
                                            "(" +
                                            "SELECT a.ItemID, a.TaxPolicy, b.EntityType, b.TaxID FROM Item a" +
                                            " INNER JOIN ItemTax b ON a.ItemID = b.EntityID AND b.EntityType = 'I'" +
                                            " WHERE a.TaxPolicy = 'I'" +
                                            " UNION" +
                                            " SELECT a.ItemID, a.TaxPolicy, b.EntityType, b.TaxID FROM Item a" +
                                            " INNER JOIN ItemTax b ON a.DepartmentID = b.EntityID AND b.EntityType = 'D'" +
                                            " WHERE a.TaxPolicy = 'D'" +
                                            " UNION" +
                                            " SELECT ItemID, TaxPolicy, EntityType, TaxID FROM" +
                                                " (SELECT RANK() OVER(PARTITION BY ItemID  ORDER BY EntityType ASC) AS Rnk, * FROM" +
                                                    " (SELECT a.ItemID, a.TaxPolicy, b.EntityType, b.TaxID FROM Item a INNER JOIN ItemTax b ON a.DepartmentID = b.EntityID AND b.EntityType = 'D' WHERE a.TaxPolicy = 'O'" +
                                                        " UNION SELECT a.ItemID, a.TaxPolicy, c.EntityType, c.TaxID FROM Item a INNER JOIN ItemTax c ON a.ItemID = c.EntityID AND c.EntityType = 'I' WHERE a.TaxPolicy = 'O'" +
                                                    ") x" +
                                                ") y WHERE Rnk = 1" +
                                            ") z) AND ";
                }
                else if (cboTaxable.SelectedIndex == 2)
                {
                    WhereQuery += " I.ItemID NOT IN (SELECT ItemID FROM " +
                                            "(" +
                                            "SELECT a.ItemID, a.TaxPolicy, b.EntityType, b.TaxID FROM Item a" +
                                            " INNER JOIN ItemTax b ON a.ItemID = b.EntityID AND b.EntityType = 'I'" +
                                            " WHERE a.TaxPolicy = 'I'" +
                                            " UNION" +
                                            " SELECT a.ItemID, a.TaxPolicy, b.EntityType, b.TaxID FROM Item a" +
                                            " INNER JOIN ItemTax b ON a.DepartmentID = b.EntityID AND b.EntityType = 'D'" +
                                            " WHERE a.TaxPolicy = 'D'" +
                                            " UNION" +
                                            " SELECT ItemID, TaxPolicy, EntityType, TaxID FROM" +
                                                " (SELECT RANK() OVER(PARTITION BY ItemID  ORDER BY EntityType ASC) AS Rnk, * FROM" +
                                                    " (SELECT a.ItemID, a.TaxPolicy, b.EntityType, b.TaxID FROM Item a INNER JOIN ItemTax b ON a.DepartmentID = b.EntityID AND b.EntityType = 'D' WHERE a.TaxPolicy = 'O'" +
                                                        " UNION SELECT a.ItemID, a.TaxPolicy, c.EntityType, c.TaxID FROM Item a INNER JOIN ItemTax c ON a.ItemID = c.EntityID AND c.EntityType = 'I' WHERE a.TaxPolicy = 'O'" +
                                                    ") x" +
                                                ") y WHERE Rnk = 1" +
                                            ") z) AND ";
                }
            }
            #endregion

            //if (cboTaxable.SelectedIndex > 0)
            //    WhereQuery += " I."+clsPOSDBConstants.Item_Fld_isTaxable+" = "+cboTaxable.SelectedItem.Tag+" AND ";
            //Added By Shitaljit for PrimePOS JIRA -1321- Search Item with Specific string
            if (string.IsNullOrEmpty(this.txtDescription.Text) == false)
            {
                WhereQuery += " I." + clsPOSDBConstants.Item_Fld_Description + " LIKE('" + this.txtDescription.Text.Replace("'", "''") + "%') AND ";   //17-Jun-2015 JY Added to filter items by matching any string within the Item Description
            }
            if (string.IsNullOrEmpty(this.txtItemCode.Text) == false)
            {
                WhereQuery += " I." + clsPOSDBConstants.Item_Fld_ItemID + " LIKE('" + this.txtItemCode.Text.Replace("'", "''") + "%') AND ";
            }
            if (this.chkOnlyEBTItems.Checked == true)
            {
                WhereQuery += " I." + clsPOSDBConstants.Item_Fld_IsEBTItem + " = '1' AND ";
            }
            if (this.chkNonRefundable.Checked == true)  //PRIMEPOS-2592 06-Nov-2018 JY Added 
            {
                WhereQuery += " I." + clsPOSDBConstants.Item_Fld_IsNonRefundable + " = '1' AND ";
            }
            if (this.chkIsIIAS.Checked == true)
            {
                WhereQuery += " I." + clsPOSDBConstants.Item_Fld_ItemID + " IN(SELECT UPCCODE FROM IIAS_Items WHERE isActive=1 and IsNull(changeIndicator,'')<>'D')";
            }
            #region PRIMEPOS-2705 13-May-2021 JY Added
            if (this.chkSaleItemOnly.Checked == true)
            {
                WhereQuery += " I." + clsPOSDBConstants.Item_Fld_isOnSale + " = 1 AND CONVERT(date,SaleStartDate) <= CONVERT(date, GETDATE()) AND CONVERT(date,SaleEndDate) >= CONVERT(date, GETDATE())";
            }
            #endregion

            WhereQuery += BuildSqlForExpDate(); //Sprint-21 - PRIMEPOS-2206 07-Mar-2016 JY Added code for Exp. date

            if (WhereQuery != "" && WhereQuery.Trim().EndsWith("AND"))
                WhereQuery = " Where " + WhereQuery.Remove(WhereQuery.Length - 4);
            else if(WhereQuery!="")
                WhereQuery = " Where " + WhereQuery;

            #region 19-Jun-2015 JY Added
            if (string.IsNullOrEmpty(this.txtDescription.Text) == false)
            {
                if (txtVendorCode.Text != "")
                {
                    WhereQuery1 = " V." + clsPOSDBConstants.Vendor_Fld_VendorCode + " like '" + txtVendorCode.Text + "' AND";
                    isVendRequired = true;
                }
                if (txtVendorItemCode.Text != "")
                {
                    WhereQuery1 += " IV." + clsPOSDBConstants.ItemVendor_Fld_VendorItemID + " like '" + txtVendorItemCode.Text + "%' AND";
                    isItemVendorRequired = true;
                }
                if (txtSkuCode.Text != "")
                    WhereQuery1 += " I." + clsPOSDBConstants.Item_Fld_ProductCode + " like '" + txtSkuCode.Text + "%' AND";
                if (txtDepartment.Text != "")
                    WhereQuery1 += " I." + clsPOSDBConstants.Item_Fld_DepartmentID + " like '" + txtDepartment.Tag + "' AND";
                if (txtSubDepartment.Text != "")
                    WhereQuery1 += " I." + clsPOSDBConstants.Item_Fld_SubDepartmentID + " like '" + txtSubDepartment.Tag + "' AND";
                if (txtTaxCode.Text != "")
                    WhereQuery1 += " I." + clsPOSDBConstants.Item_Fld_TaxID + " LIKE '" + txtTaxCode.Tag + "' AND";
                if (cboTaxable.SelectedIndex > 0)
                    WhereQuery1 += " I." + clsPOSDBConstants.Item_Fld_isTaxable + " = " + cboTaxable.SelectedItem.Tag + " AND ";
                if (string.IsNullOrEmpty(this.txtDescription.Text) == false)
                {
                    WhereQuery1 += " I." + clsPOSDBConstants.Item_Fld_Description + " LIKE('%" + this.txtDescription.Text.Replace("'", "''") + "%') AND I." + clsPOSDBConstants.Item_Fld_Description + " NOT LIKE ('" + this.txtDescription.Text.Replace("'", "''") + "%') AND ";   
                }
                if (string.IsNullOrEmpty(this.txtItemCode.Text) == false)
                {
                    WhereQuery1 += " I." + clsPOSDBConstants.Item_Fld_ItemID + " LIKE('" + this.txtItemCode.Text.Replace("'", "''") + "%') AND ";
                }
                if (this.chkOnlyEBTItems.Checked == true)
                {
                    WhereQuery1 += " I." + clsPOSDBConstants.Item_Fld_IsEBTItem + " = '1' AND ";
                }
                if (this.chkNonRefundable.Checked == true)  //PRIMEPOS-2592 06-Nov-2018 JY Added 
                {
                    WhereQuery1 += " I." + clsPOSDBConstants.Item_Fld_IsNonRefundable + " = '1' AND ";
                }
                if (this.chkIsIIAS.Checked == true)
                {
                    WhereQuery1 += " I." + clsPOSDBConstants.Item_Fld_ItemID + " IN(SELECT UPCCODE FROM IIAS_Items WHERE isActive=1 and IsNull(changeIndicator,'')<>'D')";
                }

                WhereQuery1 += BuildSqlForExpDate(); //Sprint-21 - PRIMEPOS-2206 07-Mar-2016 JY Added code for Exp. date

                if (WhereQuery1 != "" && WhereQuery1.Trim().EndsWith("AND"))
                    WhereQuery1 = " Where " + WhereQuery1.Remove(WhereQuery1.Length - 4);
                else if (WhereQuery1 != "")
                    WhereQuery1 = " Where " + WhereQuery1;
            }
            #endregion

            return WhereQuery;
        }

        private void frmItemAdvSearch_Load(object sender, EventArgs e)
        {
            this.txtSkuCode.AfterEnterEditMode += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
            this.txtSkuCode.AfterExitEditMode += new System.EventHandler(clsUIHelper.AfterExitEditMode);

            this.txtVendorItemCode.AfterEnterEditMode += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
            this.txtVendorItemCode.AfterExitEditMode += new System.EventHandler(clsUIHelper.AfterExitEditMode);

            this.txtVendorCode.AfterEnterEditMode += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
            this.txtVendorCode.AfterExitEditMode += new System.EventHandler(clsUIHelper.AfterExitEditMode);

            this.txtTaxCode.AfterEnterEditMode += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
            this.txtTaxCode.AfterExitEditMode += new System.EventHandler(clsUIHelper.AfterExitEditMode);

            this.txtDepartment.AfterEnterEditMode += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
            this.txtDepartment.AfterExitEditMode += new System.EventHandler(clsUIHelper.AfterExitEditMode);

            this.txtSubDepartment.AfterEnterEditMode += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
            this.txtSubDepartment.AfterExitEditMode += new System.EventHandler(clsUIHelper.AfterExitEditMode);

            this.txtDescription.AfterEnterEditMode += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
            this.txtDescription.AfterExitEditMode += new System.EventHandler(clsUIHelper.AfterExitEditMode);

            this.txtItemCode.AfterEnterEditMode += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
            this.txtItemCode.AfterExitEditMode += new System.EventHandler(clsUIHelper.AfterExitEditMode);

            clsUIHelper.setColorSchecme(this);
            this.Location = new Point(200, 250);
        }

        private void txtVendorCode_EditorButtonClick(object sender, Infragistics.Win.UltraWinEditors.EditorButtonEventArgs e)
        {
            SearchVendor();
        }

        private void txtDepartment_EditorButtonClick(object sender, Infragistics.Win.UltraWinEditors.EditorButtonEventArgs e)
        {
            SearchDept();
        }
        private void SearchDept()
        {
            try
            {

                //oSearch = new frmSearch(clsPOSDBConstants.Department_tbl,this.txtDepartment.Text, "");
                oSearch = new frmSearchMain(clsPOSDBConstants.Department_tbl, this.txtDepartment.Text, "", true);   //20-Dec-2017 JY Added new reference
                oSearch.SearchInConstructor = true;
                oSearch.ShowDialog();
                if (!oSearch.IsCanceled)
                {
                    DepartmentData deptData = new DepartmentData();
                    DepartmentSvr Deptsvr = new DepartmentSvr();
                    deptData = Deptsvr.Populate(oSearch.SelectedRowID());

                    txtDepartment.Tag = deptData.Tables[0].Rows[0][0].ToString();
                    txtDepartment.Text = deptData.Tables[0].Rows[0][clsPOSDBConstants.Department_Fld_DeptCode].ToString();  //Sprint-23 - PRIMEPOS-2289 20-May-2016 JY Added
                }
                else
                {
                    txtDepartment.Text = string.Empty;
                    txtDepartment.Tag = string.Empty;
                }

            }

            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }

        private void txtSubDepartment_EditorButtonClick(object sender, Infragistics.Win.UltraWinEditors.EditorButtonEventArgs e)
        {
            SearchSubDept();
        }
        private void SearchSubDept()
        {
            try
            {

                //oSearch = new frmSearch(clsPOSDBConstants.SubDepartment_tbl, this.txtSubDepartment.Text , "");
                oSearch = new frmSearchMain(clsPOSDBConstants.SubDepartment_tbl, this.txtSubDepartment.Text, "", true); //20-Dec-2017 JY Added new reference
                oSearch.SearchInConstructor = true;
                oSearch.ShowDialog();
                if (!oSearch.IsCanceled)
                {
                    txtSubDepartment.Tag = oSearch.SelectedRowID();
                    txtSubDepartment.Text = oSearch.SelectedRowCode;
                }
                else
                {
                    txtSubDepartment.Text = string.Empty;
                    txtSubDepartment.Tag = string.Empty;
                }

            }

            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.IsCanceled = true;
            this.Close();
        }

        private void txtTaxCode_EditorButtonClick(object sender, Infragistics.Win.UltraWinEditors.EditorButtonEventArgs e)
        {
            this.SearchTaxCode();
        }
        private void SearchTaxCode()
        {
            try
            {
                SearchSvr.StrName = txtTaxCode.Text;
                //oSearch = new frmSearch(clsPOSDBConstants.TaxCodes_tbl);
                //oSearch.txtCode.Text = txtTaxCode.Text;
                frmSearchMain oSearch = new frmSearchMain(clsPOSDBConstants.TaxCodes_tbl, txtTaxCode.Text, "", true);    //20-Dec-2017 JY Added new reference
                oSearch.SearchInConstructor = true;
                
                oSearch.ShowDialog();
                if (!oSearch.IsCanceled)
                {
                    oTaxcodeData = oBRTaxCodes.Populate(oSearch.SelectedRowID());
                    if (oTaxcodeData.TaxCodes.Rows.Count > 0)
                    {
                        txtTaxCode.Tag = oTaxcodeData.TaxCodes[0].TaxID;
                        txtTaxCode.Text = oSearch.SelectedRowID();
                    }
                }
                else
                {
                    txtTaxCode.Tag = string.Empty;
                    txtTaxCode.Text = string.Empty;
                }
            }
            catch (Exception ex)
            {
                clsUIHelper.ShowErrorMsg(ex.Message);
            }
        }

        private void SearchVendor()
        {
            try
            {
                //oSearch = new frmSearch(clsPOSDBConstants.Vendor_tbl, this.txtVendorCode.Text,"");
                oSearch = new frmSearchMain(clsPOSDBConstants.Vendor_tbl, this.txtVendorCode.Text, "", true);   //20-Dec-2017 JY Added new reference
                oSearch.SearchInConstructor = true;
                oSearch.ShowDialog();
                if (!oSearch.IsCanceled)
                {
                    txtVendorCode.Tag = oSearch.SelectedRowID();
                    txtVendorCode.Text = oSearch.SelectedRowCode.Trim();
                }
                else 
                { 

                }
            }
            catch (Exception ex)
            {
                clsUIHelper.ShowErrorMsg(ex.Message);
            }
        }

        private void KeyUp_Event(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyData == System.Windows.Forms.Keys.F4)
                {
                    if (txtDepartment.ContainsFocus)
                        this.SearchDept();
                    else if (txtTaxCode.ContainsFocus)
                        this.SearchTaxCode();
                    else if (txtSubDepartment.ContainsFocus)
                        this.SearchSubDept();
                    else if (txtVendorCode.ContainsFocus)
                        this.SearchVendor();
                }
                if (e.KeyData == System.Windows.Forms.Keys.F6)
                {
                    btnOk_Click(null,null);
                }
            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }

        #region Sprint-21 - 2206 07-Mar-2016 JY Added
        private void cboExpiryDate_ValueChanged(object sender, EventArgs e)
        {
            if (cboExpiryDate.Visible == false) return;

            if (cboExpiryDate.SelectedItem.DataValue.ToString() == "All" || cboExpiryDate.SelectedItem.DataValue.ToString() == "NULL" || cboExpiryDate.SelectedItem.DataValue.ToString() == "NOT NULL")
            {
                dtpExpiryDate1.Visible = false;
                dtpExpiryDate2.Visible = false;
            }
            else if (cboExpiryDate.SelectedItem.DataValue.ToString() == "=" || cboExpiryDate.SelectedItem.DataValue.ToString() == ">" || cboExpiryDate.SelectedItem.DataValue.ToString() == "<")
            {
                dtpExpiryDate1.Visible = true;
                dtpExpiryDate2.Visible = false;
            }
            else if (cboExpiryDate.SelectedItem.DataValue.ToString() == "Between")
            {
                dtpExpiryDate1.Visible = true;
                dtpExpiryDate2.Visible = true;
            }
        }
        #endregion

        #region Sprint-21 - PRIMEPOS-2206 07-Mar-2016 JY Added code for Exp. date
        private string BuildSqlForExpDate()
        {
            String strSQL = string.Empty;
            try
            {
                if (cboExpiryDate.SelectedItem.DataValue.ToString() == "NULL")
                    strSQL += " I." + clsPOSDBConstants.Item_Fld_ExpDate + " IS NULL ";
                else if (cboExpiryDate.SelectedItem.DataValue.ToString() == "NOT NULL")
                    strSQL += " I." + clsPOSDBConstants.Item_Fld_ExpDate + " IS NOT NULL ";
                else if (cboExpiryDate.SelectedItem.DataValue.ToString() == "=" || cboExpiryDate.SelectedItem.DataValue.ToString() == ">" || cboExpiryDate.SelectedItem.DataValue.ToString() == "<")
                    strSQL += " Convert(date, " + "I." + clsPOSDBConstants.Item_Fld_ExpDate + ") " + cboExpiryDate.SelectedItem.DataValue.ToString() + " Convert(date, '" + dtpExpiryDate1.Value + "')";
                else if (cboExpiryDate.SelectedItem.DataValue.ToString() == "Between")
                {
                    if ((DateTime)dtpExpiryDate1.Value < (DateTime)dtpExpiryDate2.Value)
                        strSQL += " Convert(date, " + "I." + clsPOSDBConstants.Item_Fld_ExpDate + ") BETWEEN Convert(date, '" + dtpExpiryDate1.Value + "') AND Convert(date, '" + dtpExpiryDate2.Value + "')";
                    else
                        strSQL += "Convert(date, " + "I." + clsPOSDBConstants.Item_Fld_ExpDate + ") BETWEEN Convert(date, '" + dtpExpiryDate2.Value + "') AND Convert(date, '" + dtpExpiryDate1.Value + "')";
                }
            }
            catch { strSQL = string.Empty; }
            return strSQL;
        }
        #endregion
    }
}