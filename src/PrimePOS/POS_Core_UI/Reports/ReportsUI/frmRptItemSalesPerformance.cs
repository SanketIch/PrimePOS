using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Infragistics.Win.UltraWinGrid;
//using POS.UI;
//using POS_Core.DataAccess;
using POS_Core.CommonData;
using POS_Core_UI.Reports.Reports;
using POS.Reports.ReportXSD;
//using POS_Core_UI.Reports.ReportsUI;
using POS_Core.DataAccess;
using POS_Core.BusinessRules;
using System.Data.SqlClient;
using POS_Core.CommonData.Rows;
using POS_Core.CommonData.Tables;
using POS_Core.Resources;

namespace POS_Core_UI.Reports.ReportsUI
{
    
    public partial class frmRptItemSalesPerformance : Form
    {
        #region Declaration
        DateTime dtFromDate;
        DateTime dtToDate;
        string strError = "";

        #endregion


        public frmRptItemSalesPerformance()
        {
            InitializeComponent();
        }

        private void frmRptItemSalesPerformance_Load(object sender, EventArgs e)
        {
            clsUIHelper.setColorSchecme(this);
            txtToDate.Value = DateTime.Now;
            txtFromDate.Value = DateTime.Now;
            this.txtDeptCode.Tag = "";
            this.txtItemCode.AfterEnterEditMode += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
            this.txtItemCode.AfterExitEditMode += new System.EventHandler(clsUIHelper.AfterExitEditMode);

            this.txtDeptCode.AfterEnterEditMode += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
            this.txtDeptCode.AfterExitEditMode += new System.EventHandler(clsUIHelper.AfterExitEditMode);			

            this.txtFromDate.Enter += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
            this.txtFromDate.Leave += new System.EventHandler(clsUIHelper.AfterExitEditMode);

            this.txtToDate.Enter += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
            this.txtToDate.Leave += new System.EventHandler(clsUIHelper.AfterExitEditMode);
            
            this.txtVendorCode.Enter += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
            this.txtVendorCode.Leave += new System.EventHandler(clsUIHelper.AfterExitEditMode);

            this.txtTopProducts.Enter += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
            this.txtTopProducts.Leave += new System.EventHandler(clsUIHelper.AfterExitEditMode);

            this.txtSubDeptCode.AfterEnterEditMode += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
            this.txtSubDeptCode.AfterExitEditMode += new System.EventHandler(clsUIHelper.AfterExitEditMode);
        }
        
        private void SearchDept()
        {
            try
            {
                //frmSearch oSearch = new frmSearch(clsPOSDBConstants.Department_tbl);
                frmSearchMain oSearch = new frmSearchMain(true);    //20-Dec-2017 JY Added new reference
                oSearch.SearchTable = clsPOSDBConstants.Department_tbl; //20-Dec-2017 JY Added
                oSearch.AllowMultiRowSelect = true;
                oSearch.SearchInConstructor = true;
                oSearch.ShowDialog(this);
                if (!oSearch.IsCanceled)
                {
                    string strDeptCode = "";
                    string strDeptName = "";
                    foreach (UltraGridRow oRow in oSearch.grdSearch.Rows)
                    {
                        if ((bool)oRow.Cells["check"].Value == true)
                        {
                            strDeptCode += ",'" + oRow.Cells["Code"].Text + "'";
                            strDeptName += "," + oRow.Cells["Name"].Text;                           
                        }
                    }

                    if (strDeptCode.StartsWith(","))
                    {
                        strDeptCode = strDeptCode.Substring(1);
                        strDeptName = strDeptName.Substring(1);
                    }
                    txtDeptCode.Text = strDeptName;
                    txtDeptCode.Tag = strDeptCode;                    
                }
                else
                {
                    txtDeptCode.Text = string.Empty;
                    txtDeptCode.Tag = string.Empty;
                }

            }

            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }

        private void txtDeptCode_EditorButtonClick(object sender, Infragistics.Win.UltraWinEditors.EditorButtonEventArgs e)
        {
            SearchDept();
        }

        private void SearchItem()
        {
            try
            {
                //frmSearch oSearch = new frmSearch(clsPOSDBConstants.Item_tbl);
                frmSearchMain oSearch = new frmSearchMain(true);    //20-Dec-2017 JY Added new reference
                oSearch.SearchTable = clsPOSDBConstants.Item_tbl;  //20-Dec-2017 JY Added 
                oSearch.AllowMultiRowSelect = true;
                oSearch.ShowDialog(this);
                if (!oSearch.IsCanceled)
                {
                    string strItemCode = "";
                    string strItemDesc = "";
                    foreach (UltraGridRow oRow in oSearch.grdSearch.Rows)
                    {
                        if ((bool)oRow.Cells["check"].Value == true)
                        {
                            strItemCode += ",'" + oRow.Cells["Item Code"].Text + "'";
                            strItemDesc += "," + oRow.Cells["item Description"].Text;

                        }
                    }

                    if (strItemCode.StartsWith(","))
                    {
                        strItemCode = strItemCode.Substring(1);
                        strItemDesc = strItemDesc.Substring(1);
                    }
                    txtItemCode.Text = strItemDesc;
                    txtItemCode.Tag = strItemCode;
                }
                else
                {
                    txtItemCode.Text = string.Empty;
                    txtItemCode.Tag = string.Empty;
                }

            }

            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }
        private void SearchVendor()
        {
            try
            {
                //frmSearch oSearch = new frmSearch(clsPOSDBConstants.Vendor_tbl);
                frmSearchMain oSearch = new frmSearchMain(true);    //20-Dec-2017 JY Added new reference
                oSearch.SearchTable = clsPOSDBConstants.Vendor_tbl; //20-Dec-2017 JY Added 
                oSearch.AllowMultiRowSelect = true;
                oSearch.SearchInConstructor = true;
                oSearch.ShowDialog(this);
                if (!oSearch.IsCanceled)
                {
                    string strVendorCode = "";
                    string strVendorDesc = "";
                    foreach (UltraGridRow oRow in oSearch.grdSearch.Rows)
                    {
                        if ((bool)oRow.Cells["check"].Value == true)
                        {
                            strVendorCode += ",'" + oRow.Cells["Code"].Text + "'";
                            strVendorDesc += "," + oRow.Cells["Name"].Text;

                        }
                    }

                    if (strVendorCode.StartsWith(","))
                    {
                        strVendorCode = strVendorCode.Substring(1);
                        strVendorDesc = strVendorDesc.Substring(1);
                    }
                    txtVendorCode.Text = strVendorDesc;
                    txtVendorCode.Tag = strVendorCode;
                }
                else
                {
                    txtVendorCode.Text = string.Empty;
                    txtVendorCode.Tag = string.Empty;
                }
            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }

        private void txtItem_EditorButtonClick(object sender, Infragistics.Win.UltraWinEditors.EditorButtonEventArgs e)
        {
            SearchItem();
        }

        private bool validateFields(out string fieldName)
        {
            bool isValid = true;
            string strErrorMsg = string.Empty;
            DateTime ToDate = (DateTime)txtToDate.Value;
            DateTime FromDate = (DateTime)txtFromDate.Value;
            try
            {
               
                if ( FromDate > ToDate )
                {
                    isValid = false;
                    strErrorMsg = "From Date can not be greater than To Date.";
                }
                if (ToDate.Year != FromDate.Year)
                {
                    isValid = false;
                    strErrorMsg += "\nFrom Date and To Date must be of same year.";
                }
            }
            catch (Exception ex)
            {
                clsUIHelper.ShowErrorMsg(ex.Message);
            }
            fieldName = strErrorMsg;
            return isValid;
        }


        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (!validateFields(out strError))
                clsUIHelper.ShowErrorMsg(strError);
                Preview(true);
        }

        private void btnView_Click(object sender, EventArgs e)
        {
            if (!validateFields(out strError))
                clsUIHelper.ShowErrorMsg(strError);
             else
                Preview(false);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtVendorCode_Click(object sender, EventArgs e)
        {
            SearchVendor();
        }

        private void frmRptItemSalesPerformance_KeyDown(object sender, KeyEventArgs e)
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

        private void txtItemCode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.F4)
            {
                SearchItem();
            }
        }

        private void txtDeptCode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.F4)
            {
                SearchDept();
            }
        }

        private void txtVendorCode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.F4)
            {
                SearchVendor();
            }
        }
        private void SearchSubDept()
        {
            try
            {
                POS_Core.BusinessRules.Department oDept = new Department();
                DepartmentData oDeptData;
                DepartmentRow oDeptRow = null;
                List<string> listDept = new List<string>();
                if (this.txtDeptCode.Tag.ToString() != "")
                {
                    oDeptData = oDept.PopulateList(" and deptcode in (" + txtDeptCode.Tag.ToString() + ")");
                }
                else
                {
                    oDeptData = oDept.PopulateList("");
                }
                for (int RowIndex = 0; RowIndex < oDeptData.Tables[0].Rows.Count; RowIndex++)
                {
                    oDeptRow = oDeptData.Department[RowIndex];
                    listDept.Add(oDeptRow.DeptID.ToString());
                }
                string InQuery = string.Join("','", listDept.ToArray());
                if (InQuery != "")
                {
                    SearchSvr.SubDeptIDFlag = true;
                }
                //frmSearch oSearch = new frmSearch(clsPOSDBConstants.SubDepartment_tbl, InQuery, "");
                frmSearchMain oSearch = new frmSearchMain(clsPOSDBConstants.SubDepartment_tbl, InQuery, "", true);  //20-Dec-2017 JY Added new reference
                oSearch.AllowMultiRowSelect = true;
                oSearch.SearchInConstructor = true;
                oSearch.ShowDialog(this);
                if (!oSearch.IsCanceled)
                {
                    string strSubDeptCode = "";
                    string stSubDeptName = "";
                    foreach (UltraGridRow oRow in oSearch.grdSearch.Rows)
                    {
                        if ((bool)oRow.Cells["check"].Value == true)
                        {
                            strSubDeptCode += ",'" + oRow.Cells["Code"].Text + "'";
                            stSubDeptName += "," + oRow.Cells["Sub Department Name"].Text;
                        }
                    }
                    if (strSubDeptCode.StartsWith(","))
                    {
                        strSubDeptCode = strSubDeptCode.Substring(1);
                        stSubDeptName = stSubDeptName.Substring(1);
                    }
                    txtSubDeptCode.Text = stSubDeptName;
                    txtSubDeptCode.Tag = strSubDeptCode;
                }
                else
                {
                    txtSubDeptCode.Text = string.Empty;
                    txtSubDeptCode.Tag = string.Empty;
                }
                SearchSvr.SubDeptIDFlag = false;
            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }

        private string GetDeptId()
        {
            POS_Core.BusinessRules.Department oDept = new Department();
            DepartmentData oDeptData;
            DepartmentRow oDeptRow = null;
            List<string> listDept = new List<string>();
            if (string.IsNullOrEmpty(txtDeptCode.Tag.ToString()) == false)
            {
                oDeptData = oDept.PopulateList(" and deptcode in (" + txtDeptCode.Tag.ToString() + ")");
            }
            else
            {
                oDeptData = oDept.PopulateList("");
            }
            for (int RowIndex = 0; RowIndex < oDeptData.Tables[0].Rows.Count; RowIndex++)
            {
                oDeptRow = oDeptData.Department[RowIndex];
                listDept.Add(oDeptRow.DeptID.ToString());
            }
            string InQuery = string.Join(",", listDept.ToArray());
            return InQuery;
        }

        private void Preview(bool PrintIt)
        {
            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            string strQuery = "";
            try
            {
                dtFromDate = Convert.ToDateTime(this.txtFromDate.Text);
                dtToDate = Convert.ToDateTime(this.txtToDate.Text);
                dtToDate = dtToDate.AddHours(23.9999);

                #region Commected Code
                //Commected by shitaljit to fixed the issues of not populating any records PRIMEPO -JIRA- 840
                //strQuery = "SELECT TOP " + this.txtTopProducts.Value.ToString() + " ptd.ItemID, ptd.ItemDescription, vd.VendorName,dp.DeptName,sd.Description, ";
                //strQuery += SubQueryBuilder();
                //strQuery += TotQueryBuilder();
                //strQuery += " FROM POSTransactionDetail ptd INNER JOIN POSTransaction pt ON ptd.TransID=pt.TransID INNER JOIN Item it ON ptd.ItemID = it.ItemID"
                //+ " INNER JOIN Department dp ON dp.DeptID= it.DepartmentID INNER JOIN SubDepartment sd ON dp.DeptID = sd.DepartmentID INNER JOIN Vendor vd ON vd.VendorCode = it.LastVendor"
                //+ " WHERE  pt.TransDate between '" + dtFromDate + "' AND '" + dtToDate + "' OR "
                //+ " pt.TransDate between '" + dtFromDate.AddYears(-1) + "' AND '" + dtToDate.AddYears(-1) + "' OR "
                //+ " pt.TransDate between '" + dtFromDate.AddYears(-2) + "' AND '" + dtToDate.AddYears(-2) + "'";

                //if (this.txtItemCode.Text.Trim() != "" && this.txtItemCode.Tag != null && this.txtItemCode.Tag.ToString().Trim().Length > 0)
                //{
                //    strQuery += " and it.ItemID in (" + this.txtItemCode.Tag.ToString().Trim() + ")";
                //}

                //if (this.txtDeptCode.Text.Trim() != "" && this.txtDeptCode.Tag != null && this.txtDeptCode.Tag.ToString().Trim().Length > 0)
                //{
                //    strQuery += " and dp.DeptCode in (" + this.txtDeptCode.Tag.ToString().Trim() + ")";
                //}
                //if (this.txtSubDeptCode.Text.Trim() != "" && txtSubDeptCode.Tag != null && txtSubDeptCode.Tag.ToString().Trim().Length > 0)
                //{
                //    strQuery += " and SD.SubDepartmentID in (" + this.txtSubDeptCode.Tag.ToString().Trim() + ")";
                //}

                //if (this.txtVendorCode.Text.Trim() != "" && this.txtVendorCode.Tag != null && this.txtVendorCode.Tag.ToString().Trim().Length > 0)
                //{
                //    strQuery += " and vd.VendorCode in (" + this.txtVendorCode.Tag.ToString().Trim() + ")";
                //}
                #endregion

                string ItemJoinClause = "";
                string DeptJoinClause = "";
                string SubDeptJoinCls = "";
                if (string.IsNullOrEmpty(Configuration.convertNullToString(txtItemCode.Tag)) == false)
                {
                    ItemJoinClause = " AND ptd.ItemId IN (" + txtItemCode.Tag.ToString() + ")";
                }
                if (string.IsNullOrEmpty(Configuration.convertNullToString(txtDeptCode.Tag)) == false)
                {
                    DeptJoinClause = " AND It.DepartmentId IN (" + GetDeptId() + ")";
                }
                if (string.IsNullOrEmpty(Configuration.convertNullToString(txtSubDeptCode.Tag)) == false)
                {
                    SubDeptJoinCls = " AND It.SubDepartmentId IN (" + txtSubDeptCode.Tag.ToString() + ")";
                }
                strQuery = "SELECT TOP " + this.txtTopProducts.Value.ToString() + "ptd.ItemID,ISNULL( ptd.ItemDescription,'N/A') as ItemDescription, dp.DeptName,ISNULL(sd.Description,'N/A') as Description, ";
                strQuery += SubQueryBuilder();
                strQuery += TotQueryBuilder();

                strQuery += " FROM POSTransactionDetail ptd INNER JOIN POSTransaction pt ON ptd.TransID=pt.TransID " + ItemJoinClause + 
                " INNER JOIN Item it ON ptd.ItemID = it.ItemID " + DeptJoinClause + " " + SubDeptJoinCls 
                + " LEFT OUTER JOIN Department dp ON dp.DeptID = it.DepartmentID  "
                +" LEFT OUTER JOIN SubDepartment sd ON dp.DeptID = sd.DepartmentID"
                + "  WHERE  pt.TransDate between '" + dtFromDate + "' AND '" + dtToDate + "' OR "
                + " pt.TransDate between '" + dtFromDate.AddYears(-1) + "' AND '" + dtToDate.AddYears(-1) + "' OR "
                + " pt.TransDate between '" + dtFromDate.AddYears(-2) + "' AND '" + dtToDate.AddYears(-2) + "'";;
              

                strQuery += " GROUP BY dp.DeptName,sd.Description, ptd.ItemID, ptd.ItemDescription,pt.TransDate  ORDER BY Total1";

                dsItemSalesPerformance odsItemSalesPerformance = new dsItemSalesPerformance();
                rptItemSalesPerformance orptItemSalesPerformance = new rptItemSalesPerformance();
                DataSet ds = new DataSet();
                ds = clsReports.GetReportSource(strQuery);
                int index = 0;
                for (index = 0; index < ds.Tables[0].Rows.Count; index++)
                {
                    odsItemSalesPerformance.Tables[0].ImportRow(ds.Tables[0].Rows[index]);
                }
                
                ((CrystalDecisions.CrystalReports.Engine.TextObject)orptItemSalesPerformance.ReportDefinition.ReportObjects["txtHeader"]).Text = Configuration.CInfo.StoreName;
                ((CrystalDecisions.CrystalReports.Engine.TextObject)orptItemSalesPerformance.ReportDefinition.ReportObjects["txtPharmacyAddress"]).Text = Configuration.CInfo.Address;
                ((CrystalDecisions.CrystalReports.Engine.TextObject)orptItemSalesPerformance.ReportDefinition.ReportObjects["txtPharmacyCityStateZip"]).Text = Configuration.CInfo.City + ", " + Configuration.CInfo.State + " " + Configuration.CInfo.Zip;
                ((CrystalDecisions.CrystalReports.Engine.TextObject)orptItemSalesPerformance.ReportDefinition.ReportObjects["txtTelephoneNo"]).Text = Configuration.CInfo.Telephone;
                if (optPerformanceBy.CheckedIndex == 1)
                {
                    ((CrystalDecisions.CrystalReports.Engine.TextObject)orptItemSalesPerformance.ReportDefinition.ReportObjects["txtReportHeader"]).Text = "Item Sales Performance By Sales Amount ";
                }
                else if (optPerformanceBy.CheckedIndex == 0)
                {
                    ((CrystalDecisions.CrystalReports.Engine.TextObject)orptItemSalesPerformance.ReportDefinition.ReportObjects["txtReportHeader"]).Text = "Item Sales Performance By Quantity Sold ";  
                }
                clsReports.setCRTextObjectText("CurrYear", dtFromDate.Year.ToString(), orptItemSalesPerformance);
                clsReports.setCRTextObjectText("LastYear", dtFromDate.AddYears(-1).Year.ToString(), orptItemSalesPerformance);
                clsReports.setCRTextObjectText("LastToLastYear", dtFromDate.AddYears(-2).Year.ToString(), orptItemSalesPerformance);
                orptItemSalesPerformance.Database.Tables[0].SetDataSource(odsItemSalesPerformance.Tables[0]);
               
                if (PrintIt == false)
                    clsReports.ShowReport(orptItemSalesPerformance);
                else
                    clsReports.PrintReport(orptItemSalesPerformance);

                this.Cursor = System.Windows.Forms.Cursors.Default;
            }

            catch (Exception Ex)
            {
                this.Cursor = System.Windows.Forms.Cursors.Default;
                clsUIHelper.ShowErrorMsg(Ex.Message);

            }
        }
        private string SubQueryBuilder()
        {
            string strSubQuery = "";
            try
            {
                int StartMonth = dtFromDate.Month;
                int EndMonth = dtToDate.Month;
                string tempStartmnth = "";
                int TmpMnth = StartMonth; 

                for (int i = 0; i <= (EndMonth - StartMonth); i++)
                {
                    tempStartmnth = Month(TmpMnth);
                    if (optPerformanceBy.CheckedIndex == 1)
                    {
                         strSubQuery += "ISNULL((SELECT SUM(q" + (i+1) + ".Price) FROM POSTransactionDetail q" + (i+1) + " INNER JOIN POSTransaction t" + (i+1) + " ON q" + (i+1) + ".TransID=t" + (i+1) + ".TransID"
                                    + " WHERE q" + (i+1) + ".ItemID = ptd.ItemId AND t" + (i+1) + ".TransDate BETWEEN '" + dtFromDate + "' AND '" + dtToDate + "' AND MONTH(t" + (i+1) + ".TransDate)= " + TmpMnth + "),0) AS " + tempStartmnth + "1,"
                                    + " ISNULL((SELECT SUM (q" + (i + 2) + ".Price) FROM POSTransactionDetail q" + (i + 2) + " INNER JOIN POSTransaction t" + (i + 2) + " ON q" + (i + 2) + ".TransID=t" + (i + 2) + ".TransID"
                                    + " WHERE q" + (i + 2) + ".ItemID = ptd.ItemId AND t" + (i + 2) + ".TransDate BETWEEN '" + dtFromDate.AddYears(-1) + "' AND '" + dtToDate.AddYears(-1) + "' AND MONTH(t" + (i + 2) + ".TransDate)=" + TmpMnth + "),0) AS " + tempStartmnth + "2,"
                                    + " ISNULL((SELECT SUM(q" + (i + 3) + ".Price) FROM POSTransactionDetail q" + (i + 3) + " INNER JOIN POSTransaction t" + (i + 3) + " ON q" + (i + 3) + ".TransID=t" + (i + 3) + ".TransID"
                                    + " WHERE q" + (i + 3) + ".ItemID = ptd.ItemId AND t" + (i + 3) + ".TransDate BETWEEN '" + dtFromDate.AddYears(-2) + "' AND '" + dtToDate.AddYears(-2) + "'  AND MONTH(t" + (i + 3) + ".TransDate)= " + TmpMnth + "),0) AS " + tempStartmnth + "3";
                    }
                    else if (optPerformanceBy.CheckedIndex == 0)
                    {
                        strSubQuery += "ISNULL((SELECT SUM(q" + (i+1) + ".Qty) FROM POSTransactionDetail q" + (i+1) + " INNER JOIN POSTransaction t" + (i+1) + " ON q" + (i+1) + ".TransID=t" + (i+1) + ".TransID"
                                   + " WHERE q" + (i+1) + ".ItemID = ptd.ItemId AND t" + (i+1) + ".TransDate BETWEEN '" + dtFromDate + "' AND '" + dtToDate + "' AND MONTH(t" + (i+1) + ".TransDate)= " + TmpMnth + "),0) AS " + tempStartmnth + "1,"
                                   + " ISNULL((SELECT SUM (q" + (i + 2) + ".Qty) FROM POSTransactionDetail q" + (i + 2) + " INNER JOIN POSTransaction t" + (i + 2) + " ON q" + (i + 2) + ".TransID=t" + (i + 2) + ".TransID"
                                   + " WHERE q" + (i + 2) + ".ItemID = ptd.ItemId AND t" + (i + 2) + ".TransDate BETWEEN '" + dtFromDate.AddYears(-1) + "' AND '" + dtToDate.AddYears(-1) + "' AND MONTH(t" + (i + 2) + ".TransDate)=" + TmpMnth + "),0) AS " + tempStartmnth + "2,"
                                   + " ISNULL((SELECT SUM(q" + (i + 3) + ".Qty) FROM POSTransactionDetail q" + (i + 3) + " INNER JOIN POSTransaction t" + (i + 3) + " ON q" + (i + 3) + ".TransID=t" + (i + 3) + ".TransID"
                                   + " WHERE q" + (i + 3) + ".ItemID = ptd.ItemId AND t" + (i + 3) + ".TransDate BETWEEN '" + dtFromDate.AddYears(-2) + "' AND '" + dtToDate.AddYears(-2) + "'  AND MONTH(t" + (i + 3) + ".TransDate)= " + TmpMnth + "),0) AS " + tempStartmnth + "3";
                    }
                    strSubQuery += " , ";
                      TmpMnth++;
                      
                }
            }
            catch (Exception Ex)
            {
                clsUIHelper.ShowErrorMsg(Ex.Message);
            }
           
            return strSubQuery;
        }

        private string TotQueryBuilder()
        {
            string strTotQuery = "";
            string strTot1Query = "";
            string strTot2Query = "";
            string strTot3Query = "";

            try
            {
                int StartMonth = dtFromDate.Month;
                int EndMonth = dtToDate.Month;
                string tempStartmnth = "";
                int TmpMnth = StartMonth;

                for (int i = 0; i <= (EndMonth - StartMonth); i++)
                {
                    tempStartmnth = Month(TmpMnth);
                    if (optPerformanceBy.CheckedIndex == 1)
                    {
                        strTot1Query += "ISNULL((SELECT SUM(q" + (i + 1) + ".Price) FROM POSTransactionDetail q" + (i + 1) + " INNER JOIN POSTransaction t" + (i + 1) + " ON q" + (i + 1) + ".TransID=t" + (i + 1) + ".TransID"
                                   + " WHERE q" + (i + 1) + ".ItemID = ptd.ItemId AND t" + (i + 1) + ".TransDate BETWEEN '" + dtFromDate + "' AND '" + dtToDate + "' AND MONTH(t" + (i + 1) + ".TransDate)= " + TmpMnth + "),0) ";

                        strTot2Query += " ISNULL((SELECT SUM (q" + (i + 2) + ".Price) FROM POSTransactionDetail q" + (i + 2) + " INNER JOIN POSTransaction t" + (i + 2) + " ON q" + (i + 2) + ".TransID=t" + (i + 2) + ".TransID"
                                   + " WHERE q" + (i + 2) + ".ItemID = ptd.ItemId AND t" + (i + 2) + ".TransDate BETWEEN '" + dtFromDate.AddYears(-1) + "' AND '" + dtToDate.AddYears(-1) + "' AND MONTH(t" + (i + 2) + ".TransDate)=" + TmpMnth + "),0)";

                        strTot3Query += " ISNULL((SELECT SUM(q" + (i + 3) + ".Price) FROM POSTransactionDetail q" + (i + 3) + " INNER JOIN POSTransaction t" + (i + 3) + " ON q" + (i + 3) + ".TransID=t" + (i + 3) + ".TransID"
                                   + " WHERE q" + (i + 3) + ".ItemID = ptd.ItemId AND t" + (i + 3) + ".TransDate BETWEEN '" + dtFromDate.AddYears(-2) + "' AND '" + dtToDate.AddYears(-2) + "'  AND MONTH(t" + (i + 3) + ".TransDate)= " + TmpMnth + "),0) ";
                    }
                    else if (optPerformanceBy.CheckedIndex == 0)
                    {
                        strTot1Query += "ISNULL((SELECT SUM(q" + (i + 1) + ".Qty) FROM POSTransactionDetail q" + (i + 1) + " INNER JOIN POSTransaction t" + (i + 1) + " ON q" + (i + 1) + ".TransID=t" + (i + 1) + ".TransID"
                                   + " WHERE q" + (i + 1) + ".ItemID = ptd.ItemId AND t" + (i + 1) + ".TransDate BETWEEN '" + dtFromDate + "' AND '" + dtToDate + "' AND MONTH(t" + (i + 1) + ".TransDate)= " + TmpMnth + "),0)";
                       
                        strTot2Query += " ISNULL((SELECT SUM (q" + (i + 2) + ".Qty) FROM POSTransactionDetail q" + (i + 2) + " INNER JOIN POSTransaction t" + (i + 2) + " ON q" + (i + 2) + ".TransID=t" + (i + 2) + ".TransID"
                                   + " WHERE q" + (i + 2) + ".ItemID = ptd.ItemId AND t" + (i + 2) + ".TransDate BETWEEN '" + dtFromDate.AddYears(-1) + "' AND '" + dtToDate.AddYears(-1) + "' AND MONTH(t" + (i + 2) + ".TransDate)=" + TmpMnth + "),0)";
                        
                        strTot3Query += " ISNULL((SELECT SUM(q" + (i + 3) + ".Qty) FROM POSTransactionDetail q" + (i + 3) + " INNER JOIN POSTransaction t" + (i + 3) + " ON q" + (i + 3) + ".TransID=t" + (i + 3) + ".TransID"
                                   + " WHERE q" + (i + 3) + ".ItemID = ptd.ItemId AND t" + (i + 3) + ".TransDate BETWEEN '" + dtFromDate.AddYears(-2) + "' AND '" + dtToDate.AddYears(-2) + "'  AND MONTH(t" + (i + 3) + ".TransDate)= " + TmpMnth + "),0)";
                    }
                    Console.WriteLine(i);
                    if ((EndMonth != StartMonth) && i != (EndMonth - StartMonth))
                    {
                        strTot1Query += "+";
                        strTot2Query += "+";
                        strTot3Query += "+";
                    }
                    TmpMnth++;

                }
            }
            catch (Exception Ex)
            {
                clsUIHelper.ShowErrorMsg(Ex.Message);
            }
           return  strTotQuery = strTot1Query + "  AS Total1, " + strTot2Query + "  AS Total2, " + strTot3Query + " AS Total3 "; 
        }
        private string Month(int month)
        {
            switch(month)
            {
                case 1:
                    return "Jan";
                    break;

                case 2:
                    return "Feb";
                    break;

                case 3:
                    return "Mar";
                    break;

                case 4:
                    return "Apr";
                    break;

                case 5:
                    return "May";
                    break;

                case 6:
                    return "Jun";
                    break;

                case 7:
                    return "Jul";
                    break;

                case 8:
                    return "Aug";
                    break;

                case 9:
                    return "Sep";
                    break;

                case 10:
                    return "Oct";
                    break;

                case 11:
                    return "Nov";
                    break;

                case 12:
                    return "Dec";
                    break;

                default:
                    return "";
                    break;
            }
        }

        private void txtSubDeptCode_ValueChanged(object sender, EventArgs e)
        {
            SearchSubDept();
        }

        private void txtSubDeptCode_Click(object sender, EventArgs e)
        {
            SearchSubDept();
        }

        private void txtSubDeptCode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.F4)
                SearchSubDept();
        }

    }

}