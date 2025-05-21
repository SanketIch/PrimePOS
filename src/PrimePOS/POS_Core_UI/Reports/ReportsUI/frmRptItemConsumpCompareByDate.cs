using System;
using System.Data.SqlClient;
using Infragistics.Win.UltraWinGrid;
using Infragistics.Win;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using POS_Core.CommonData;
using POS_Core.BusinessRules;
using POS_Core.ErrorLogging;
using POS_Core.CommonData.Rows;
using Infragistics.Win.UltraWinMaskedEdit;
using Infragistics.Win.CalcEngine;
using Infragistics.Win.UltraWinCalcManager;
//using POS_Core.DataAccess;
using System.Data;
using System.Reflection;
using POS_Core.UserManagement;
using System.Threading;
using PharmData;
using POS_Core.DataAccess;
//using POS_Core_UI.Reports.ReportsUI;
using POS.Reports;
using System.Collections.Generic;
//using POS.UI;


namespace POS_Core_UI.Reports.ReportsUI
{
    /// <summary>
    /// Added by Krishna on 13 June 2011
    /// UI for the Report of Sales Compare Report
    /// </summary>
    public partial class frmRptItemConsumptionCompare : Form
    {
        public frmRptItemConsumptionCompare()
        {
            InitializeComponent();
        }

        private void frmRptSalesCompare_Load(object sender, EventArgs e)
        {
            this.dtpFromDateFirst.Enter += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
            this.dtpFromDateFirst.Leave += new System.EventHandler(clsUIHelper.AfterExitEditMode);

            this.dtpToDateFirst.Enter += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
            this.dtpToDateFirst.Leave += new System.EventHandler(clsUIHelper.AfterExitEditMode);

            dtpFromDateFirst.Value = DateTime.Now;
            dtpToDateFirst.Value = DateTime.Now;

            dtpFromDateFirst.Text = DateTime.Now.Date.ToString();
            dtpToDateFirst.Text = DateTime.Now.Date.ToString();

            SetNew();
            clsUIHelper.setColorSchecme(this);
        }


        private void SetNew()
        {
        txtItem.Text = "";
        txtItem.Tag = "";
        txtDepartment.Text = "";
        txtDepartment.Tag = "";
        txtSubDepartment.Text = "";
        txtSubDepartment.Tag = "";
        }
        //--------------------------------------------------------------------------------------------------------------------------------

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        //--------------------------------------------------------------------------------------------------------------------------------

        private void btnView_Click(object sender, EventArgs e)
        {
            Preview(false);
        }

        private string GetDeptId()
        {
            POS_Core.BusinessRules.Department oDept = new Department();
            DepartmentData oDeptData;
            DepartmentRow oDeptRow = null;
            List<string> listDept = new List<string>();
            if (txtDepartment.Tag.ToString() != "")
            {
                oDeptData = oDept.PopulateList(" and deptcode in (" + txtDepartment.Tag.ToString() + ")");
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

        //--------------------------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// To retrieve Dataset from  Database to Show Report
        /// </summary>
        /// <param name="PrintFlag"></param>
        private void Preview(bool PrintFlag)
        {
            try
            {
                string StartDateFirst = dtpFromDateFirst.Value.ToString();
                DateTime EndDateFirst = DateTime.SpecifyKind((DateTime)dtpToDateFirst.Value, DateTimeKind.Local);
                EndDateFirst = EndDateFirst.AddDays(1);

                DateTime StartDateSecnd = DateTime.SpecifyKind((DateTime)dtpFromDateFirst.Value, DateTimeKind.Local);
                StartDateSecnd = StartDateSecnd.AddYears(-1);
                DateTime EndDateSecnd = DateTime.SpecifyKind((DateTime)dtpToDateFirst.Value, DateTimeKind.Local);
                EndDateSecnd = EndDateSecnd.AddDays(1);
                EndDateSecnd = EndDateSecnd.AddYears(-1);


                DateTime StartDateThird = DateTime.SpecifyKind((DateTime)dtpFromDateFirst.Value, DateTimeKind.Local);
                StartDateThird = StartDateThird.AddYears(-2);
                DateTime EndDateThird = DateTime.SpecifyKind((DateTime)dtpToDateFirst.Value, DateTimeKind.Local);
                EndDateThird = EndDateThird.AddDays(1);
                EndDateThird = EndDateThird.AddYears(-2);
                if(!validateDate())
                    throw new Exception("Start date cannot be greater than End date");
                string ItemJoinClause ="";
                string DeptJoinClause = "";
                string SubDeptJoinCls = "";
                if (txtItem.Tag != "")
                {
                   ItemJoinClause = " AND pdm.ItemId IN (" + txtItem.Tag.ToString() + ")";
                }
                if (txtDepartment.Tag != "")
                {
                    DeptJoinClause = " AND I.DepartmentId IN (" + GetDeptId() + ")";
                }
                if (txtSubDepartment.Tag != "")
                {
                    SubDeptJoinCls = " AND I.SubDepartmentId IN (" + txtSubDepartment.Tag.ToString() + ")";
                }
                string sqlQuery = "SELECT pdm.ItemId, pdm.ItemDescription, D.DeptName AS DepartmentName,SD.Description AS SubDepartmentName,"
                    + " (SELECT isNull(Sum(pd.Qty),0) FROM POStransaction p INNER JOIN POSTransactionDetail pd ON p.TransId = pd.TransId"
                    + " WHERE pd.ItemId = pdm.ItemId AND p.TransDate between '" + StartDateFirst + "' and '" + EndDateFirst + "') AS First_Filter_Criteria,"

                    + " (SELECT isNull(Sum(pd.Qty),0) FROM POStransaction p INNER JOIN POSTransactionDetail pd ON p.TransId = pd.TransId"
                    + " WHERE pd.ItemId = pdm.ItemId AND p.TransDate between '" + StartDateSecnd + "' and '" + EndDateSecnd + "') AS Second_Filter_Criteria,"

                    + " (SELECT isNull(Sum(pd.Qty),0) FROM POStransaction p INNER JOIN POSTransactionDetail pd ON p.TransId = pd.TransId "
                    + " WHERE pd.ItemId = pdm.ItemId AND p.TransDate between '" + StartDateSecnd + "' and '" + EndDateSecnd + "') AS Third_Filter_Criteria"

                    + " FROM POStransaction pm INNER JOIN POSTransactionDetail pdm ON pm.TransId = pdm.TransId " + ItemJoinClause + ""
                    + " INNER JOIN Item I ON (pdm.Itemid=I.ItemId) INNER JOIN Department D ON(D.DeptID=I.DepartmentId  " + DeptJoinClause + " " + SubDeptJoinCls + ")"
                    + " LEFT OUTER JOIN SubDepartment SD ON(I.SubDepartmentId=SD.SubDepartmentId)"
                    + " WHERE pm.TransDate between '" + StartDateFirst + "' and '" + EndDateFirst + "' OR pm.TransDate between '" + StartDateSecnd + "' and '" + EndDateSecnd + "'"
                    + " Group BY D.DeptName,SD.Description, pdm.ItemId, pdm.ItemDescription";

                dsSalesCompare dsCompare = new dsSalesCompare(); 
                SqlConnection conn = new SqlConnection(POS_Core.Resources.Configuration.ConnectionString);
                SqlDataAdapter da = new SqlDataAdapter(sqlQuery, conn);
                frmReportViewer FrmReportViewer = new frmReportViewer();
                
                da.Fill(dsCompare, "SalesCompare");

                POS_Core_UI.Reports.Reports.rptItemConsumptionCompare oRptItemConsumpCompare = new POS_Core_UI.Reports.Reports.rptItemConsumptionCompare();
                oRptItemConsumpCompare.SetDataSource(dsCompare.Tables["SalesCompare"]);

                string FirstFilter= dtpFromDateFirst.Text.ToString()+" to " +dtpToDateFirst.Text.ToString();
                string SecndFilter = StartDateSecnd.Date.ToShortDateString() + " to " + EndDateSecnd.AddDays(-1).ToShortDateString();
                string ThirdFilter = StartDateThird.Date.ToShortDateString() + " to " + EndDateThird.AddDays(-1).ToShortDateString();
                SecndFilter = "For Year " + StartDateSecnd.Year;
                ThirdFilter = "For Year " + StartDateThird.Year;
                clsReports.setCRTextObjectText("txtFirstFilter", FirstFilter, oRptItemConsumpCompare);
                clsReports.setCRTextObjectText("txtSecndFilter", SecndFilter, oRptItemConsumpCompare);
                clsReports.setCRTextObjectText("txtThirdFilter", ThirdFilter, oRptItemConsumpCompare);
                clsReports.SetReportHeader(oRptItemConsumpCompare,dsCompare);

                FrmReportViewer.rvReportViewer.ReportSource = oRptItemConsumpCompare;
                FrmReportViewer.rvReportViewer.Refresh();

                if (PrintFlag)
                    clsReports.PrintReport(oRptItemConsumpCompare);
                else
                    FrmReportViewer.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //--------------------------------------------------------------------------------------------------------------------------------

        private bool validateDate()
        {
            int result = DateTime.Compare((DateTime)dtpFromDateFirst.Value, (DateTime)dtpToDateFirst.Value);

            if (result == 1)
            {
               
                dtpFromDateFirst.Focus();
                return false;
            }
            return true;
        }

        private void frmRptSalesCompare_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyData == System.Windows.Forms.Keys.Enter)
                {
                    this.SelectNextControl(this.ActiveControl, true, true, true, true);
                }
                else if (e.KeyData == Keys.Escape)
                    this.Close();
            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            Preview(true);   
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
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
                            //strDeptID +="," +  oRow.Cells[""].Text;
                        }
                    }

                    if (strDeptCode.StartsWith(","))
                    {
                        strDeptCode = strDeptCode.Substring(1);
                        strDeptName = strDeptName.Substring(1);
                    }
                    txtDepartment.Text = strDeptName;
                    txtDepartment.Tag = strDeptCode;
                    //string str = GetDeptName(strDeptCode.Trim(), clsPOSDBConstants.Department_tbl);
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
                POS_Core.BusinessRules.Department oDept = new Department();
                DepartmentData oDeptData;
                DepartmentRow oDeptRow = null;
                List<string> listDept = new List<string>();
                if (txtDepartment.Tag.ToString() != "")
                {
                    oDeptData = oDept.PopulateList(" and deptcode in (" + txtDepartment.Tag.ToString() + ")");
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
                            strSubDeptCode += "," + oRow.Cells["Code"].Text + "";
                            stSubDeptName += "," + oRow.Cells["Sub Department Name"].Text;
                        }
                    }
                    if (strSubDeptCode.StartsWith(","))
                    {
                        strSubDeptCode = strSubDeptCode.Substring(1);
                        stSubDeptName = stSubDeptName.Substring(1);
                    }
                    txtSubDepartment.Text = stSubDeptName;
                    txtSubDepartment.Tag = strSubDeptCode;
                }
                else
                {
                    txtSubDepartment.Text = string.Empty;
                    txtSubDepartment.Tag = string.Empty;
                }
                SearchSvr.SubDeptIDFlag = false;
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

        private void SearchItem()
        {
            try
            {
                //frmSearch oSearch = new frmSearch(clsPOSDBConstants.Item_tbl);
                frmSearchMain oSearch = new frmSearchMain(true);    //20-Dec-2017 JY Added new reference
                oSearch.SearchTable = clsPOSDBConstants.Item_tbl;  //20-Dec-2017 JY Added 
                oSearch.AllowMultiRowSelect = true;
                oSearch.SearchInConstructor = true;
                oSearch.ShowDialog(this);
                if (!oSearch.IsCanceled)
                {
                    string strItemCode = "";
                    string strItemDescp = "";
                    foreach (UltraGridRow oRow in oSearch.grdSearch.Rows)
                    {
                        if ((bool)oRow.Cells["check"].Value == true)
                        {
                            strItemCode += ",'" + oRow.Cells["Item Code"].Text + "'";
                            strItemDescp += "," + oRow.Cells["Item Description"].Text;
                        }
                    }

                    if (strItemCode.StartsWith(","))
                    {
                        strItemCode = strItemCode.Substring(1);
                        strItemDescp = strItemDescp.Substring(1);
                    }
                    txtItem.Text = strItemDescp;
                    txtItem.Tag = strItemCode;
                }
                else
                {
                    txtItem.Text = string.Empty;
                    txtItem.Tag = string.Empty;
                }

            }

            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }

        private void txtDepartment_EditorButtonClick(object sender, Infragistics.Win.UltraWinEditors.EditorButtonEventArgs e)
        {
            SearchDept();
        }

        private void frmRptItemConsumptionCompare_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyData)
            {
                case Keys.P:
                    btnPrint_Click(null, null);
                    break;
                case Keys.V:
                    btnView_Click(null,null);
                    break;
                case Keys.C:
                    btnClose_Click(null, null);
                    break;
                default:
                    break;
            }
        }


        //--------------------------------------------------------------------------------------------------------------------------------
    }
}