using Infragistics.Win.UltraWinGrid;
using POS_Core.BusinessRules;
using POS_Core.CommonData;
using POS_Core.CommonData.Rows;
using POS_Core.DataAccess;
using POS_Core_UI.Reports.Reports;
//using POS.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace POS_Core_UI.Reports.ReportsUI
{
    public partial class frmRptDeptList : Form
    {
        //TaxCodes oTaxCodes = new TaxCodes();
        //TaxCodesData oTaxCodesData;
        //TaxCodesRow oTaxCodesRow = null;

        private DataSet dsDept;
        private SearchSvr oSearchSvr = new SearchSvr();

        public frmRptDeptList()
        {
            InitializeComponent();
        }

        private void btnView_Click(object sender, EventArgs e)
        {
            PreviewReport(false);
        }

        private void PreviewReport(bool blnPrint)
        {
            String strDeptSql;
            try
            {
                this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
                rptDeptList oRpt = new rptDeptList();

                GenerateSQL(out strDeptSql, true);

                rptDeptList oRptDept = new rptDeptList();

                DataSet ds = clsReports.GetReportSource(strDeptSql);
                oRptDept.SetDataSource(ds.Tables[0]);
                clsReports.DStoExport = ds; //PRIMEPOS-2471 16-Feb-2021 JY Added
                clsReports.Preview(blnPrint, oRptDept);
            }
            catch (Exception exp)
            {
                this.Cursor = System.Windows.Forms.Cursors.Default;
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
            finally
            {
                this.Cursor = System.Windows.Forms.Cursors.Default;
            }
        }

        private void frmRptDeptList_Load(object sender, EventArgs e)
        {
            this.txtDepartment.AfterEnterEditMode += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
            this.txtDepartment.AfterExitEditMode += new System.EventHandler(clsUIHelper.AfterExitEditMode);
            this.txtTaxCode.AfterEnterEditMode += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
            this.txtTaxCode.AfterExitEditMode += new System.EventHandler(clsUIHelper.AfterExitEditMode);

            txtDepartment.Tag = "";

            //this.Left = (frmMain.getInstance().Width - frmMain.getInstance().ultraExplorerBar1.Width - this.Width) / 2;
            //this.Top = (frmMain.getInstance().Height - this.Height) / 2;

            clsUIHelper.setColorSchecme(this);
            clsUIHelper.SetAppearance(this.grdDept);
            clsUIHelper.SetReadonlyRow(this.grdDept);
            grdDept.DisplayLayout.Override.HeaderClickAction = HeaderClickAction.SortMulti;
            grdDept.DisplayLayout.AutoFitStyle = AutoFitStyle.ResizeAllColumns;
            btnSearch_Click(sender, e);
        }

        private void txtDepartment_EditorButtonClick(object sender, Infragistics.Win.UltraWinEditors.EditorButtonEventArgs e)
        {
            SearchDept();
        }

        private void SearchDept()
        {
            try
            {

                //frmSearch oSearch = new frmSearch(clsPOSDBConstants.Department_tbl);
                frmSearchMain oSearch = new frmSearchMain(true);    //20-Dec-2017 JY Added new reference
                oSearch.SearchTable = clsPOSDBConstants.Department_tbl; //20-Dec-2017 JY Added new reference
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
                            strDeptCode += ",'" + oRow.Cells["Code"].Text.Replace("'","''") + "'";
                            strDeptName += "," + oRow.Cells["Name"].Text.Trim();
                        }
                    }

                    if (strDeptCode.StartsWith(","))
                    {
                        strDeptCode = strDeptCode.Substring(1);
                        strDeptName = strDeptName.Substring(1);
                    }
                    txtDepartment.Text = strDeptName;
                    txtDepartment.Tag = strDeptCode;
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

        private void frmRptDeptList_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyData == System.Windows.Forms.Keys.Enter)
                {
                    this.SelectNextControl(this.ActiveControl, true, true, true, true);
                }
                else if (e.KeyData == Keys.Escape)
                    this.Close();
                else if (e.KeyData == System.Windows.Forms.Keys.F4)
                {
                    if (this.txtDepartment.ContainsFocus == true)
                    {
                        this.SearchDept();
                    }
                    if (this.txtTaxCode.ContainsFocus == true)
                    {
                        SearchTaxCode();
                    }
                }
            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }

        private void txtTaxCode_EditorButtonClick(object sender, Infragistics.Win.UltraWinEditors.EditorButtonEventArgs e)
        {
            SearchTaxCode();
        }

        private void SearchTaxCode()
        {
            try
            {
                //frmSearch oSearch = new frmSearch(clsPOSDBConstants.TaxCodes_tbl);
                frmSearchMain oSearch = new frmSearchMain(true);    //20-Dec-2017 JY Added new reference
                oSearch.SearchTable = clsPOSDBConstants.TaxCodes_tbl;
                oSearch.AllowMultiRowSelect = true;
                oSearch.SearchInConstructor = true;
                oSearch.ShowDialog(this);
                if (!oSearch.IsCanceled)
                {
                    string strTaxID = string.Empty;
                    string strTaxCode = string.Empty;
                    foreach (UltraGridRow oRow in oSearch.grdSearch.Rows)
                    {
                        if ((bool)oRow.Cells["check"].Value == true)
                        {
                            strTaxID += "," + oRow.Cells["TaxID"].Text;
                            strTaxCode += "," + oRow.Cells["Code"].Text.Trim();
                        }
                    }

                    if (strTaxID.StartsWith(","))
                    {
                        strTaxID = strTaxID.Substring(1);
                        strTaxCode = strTaxCode.Substring(1);
                    }
                    txtTaxCode.Text = strTaxCode;
                    txtTaxCode.Tag = strTaxID;
                }
                else
                {
                    txtTaxCode.Text = string.Empty;
                    txtTaxCode.Tag = string.Empty;
                }
            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }

        private void txtTaxCode_Validated(object sender, EventArgs e)
        {
            //if (this.txtTaxCode.Text != "")
            //{
            //    oTaxCodesData = oTaxCodes.Populate(this.txtTaxCode.Text.Trim().Replace("'",""));
            //    if (oTaxCodesData != null)
            //    {
            //        if (oTaxCodesData.Tables[0].Rows.Count > 0)
            //        {
            //            oTaxCodesRow = oTaxCodesData.TaxCodes[0];
            //            this.txtTaxCode.Text = oTaxCodesRow.TaxCode;
            //            this.txtTaxCode.Tag = oTaxCodesRow.TaxID;
            //        }
            //        else
            //        {
            //            clsUIHelper.ShowErrorMsg("Invalid Tax Code.");
            //            this.txtTaxCode.Focus();
            //        }
            //    }
            //    else
            //    {
            //        clsUIHelper.ShowErrorMsg("Invalid Tax Code.");
            //    }
            //}
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            string strDeptSql = string.Empty;
            try
            {
                dsDept = new DataSet();
                DataSet dsDeptTmp = new DataSet();

                GenerateSQL(out strDeptSql, false);
                dsDeptTmp.Tables.Add(oSearchSvr.Search(strDeptSql).Tables[0].Copy());

                var DeptList = from Dept in dsDeptTmp.Tables[0].AsEnumerable()
                                  group Dept by new
                                  {
                                      DeptCode = Dept.Field<String>("DeptCode"),
                                      DeptID = Dept.Field<int>("DeptID"),
                                      DeptName = Dept.Field<String>("DeptName"),
                                      IsTaxable = Dept.Field<Boolean>("IsTaxable"),
                                      TaxCodes = Dept.Field<String>("TaxCodes")
                                  } into userg
                                  select new
                                  {
                                      DeptCode = userg.Key.DeptCode,
                                      DeptID = userg.Key.DeptID,
                                      DeptName = userg.Key.DeptName,
                                      IsTaxable = userg.Key.IsTaxable,
                                      TaxCodes = userg.Key.TaxCodes,
                                  };

                DataTable dt = new DataTable();
                dt.Columns.Add("DeptCode", typeof(System.String));
                dt.Columns.Add("DeptID", typeof(int));
                dt.Columns.Add("Department Name", typeof(System.String));
                dt.Columns.Add("IsTaxable", typeof(System.Boolean));
                dt.Columns.Add("TaxCodes", typeof(System.String));
                DataRow dr;

                foreach (var Summary in DeptList)
                {
                    dr = dt.NewRow();
                    dr["DeptCode"] = Summary.DeptCode;
                    dr["DeptID"] = Summary.DeptID;
                    dr["Department Name"] = Summary.DeptName;
                    dr["IsTaxable"] = Summary.IsTaxable;
                    dr["TaxCodes"] = Summary.TaxCodes;

                    dt.Rows.Add(dr);
                }

                dsDept.Tables.Add(dt);   //Added table for summary by department
                dsDept.Tables[0].TableName = "Departments";

                dsDept.Tables.Add(dsDeptTmp.Tables[0].Copy());
                dsDept.Tables[1].TableName = "SubDepartments";

                dsDept.Relations.Add("SubDepartments", dsDept.Tables[0].Columns["DeptID"], dsDept.Tables[1].Columns["DepartmentID"]);

                grdDept.DataSource = dsDept;
                grdDept.DisplayLayout.Bands[0].HeaderVisible = true;
                grdDept.DisplayLayout.Bands[0].Header.Appearance.FontData.SizeInPoints = 10;
                grdDept.DisplayLayout.Bands[0].Header.Appearance.TextHAlign = Infragistics.Win.HAlign.Center;
                grdDept.DisplayLayout.Bands[0].Header.Caption = "Department List";
                grdDept.DisplayLayout.Bands[0].Override.WrapHeaderText = Infragistics.Win.DefaultableBoolean.True;
                grdDept.DisplayLayout.Bands[0].Columns["DeptCode"].Hidden = true;

                grdDept.DisplayLayout.Bands[1].HeaderVisible = true;
                grdDept.DisplayLayout.Bands[1].Header.Caption = "Sub-Departments";
                grdDept.DisplayLayout.Bands[1].Header.Appearance.FontData.SizeInPoints = 10;
                grdDept.DisplayLayout.Bands[1].Header.Appearance.TextHAlign = Infragistics.Win.HAlign.Center;
                grdDept.DisplayLayout.Bands[1].Expandable = true;
                grdDept.DisplayLayout.Bands[1].Override.WrapHeaderText = Infragistics.Win.DefaultableBoolean.True;

                grdDept.DisplayLayout.Bands[1].Columns["DeptCode"].Hidden = true;
                grdDept.DisplayLayout.Bands[1].Columns["DeptID"].Hidden = true;
                grdDept.DisplayLayout.Bands[1].Columns["DeptName"].Hidden = true;
                grdDept.DisplayLayout.Bands[1].Columns["IsTaxable"].Hidden = true;
                grdDept.DisplayLayout.Bands[1].Columns["DepartmentID"].Hidden = true;
                grdDept.DisplayLayout.Bands[1].Columns["TaxCodes"].Hidden = true;
                //grdDept.DisplayLayout.Bands[1].Columns["SubDepartmentID"].Width = 100;

                resizeColumns(grdDept);
                grdDept.PerformAction(UltraGridAction.FirstRowInGrid);
                grdDept.Refresh();
            }
            catch (Exception Ex)
            {
            }
        }

        private void resizeColumns(Infragistics.Win.UltraWinGrid.UltraGrid grd)
        {
            try
            {
                foreach (UltraGridBand oBand in grd.DisplayLayout.Bands)
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

        private void GenerateSQL(out string strDeptSql, Boolean Flag)
        {
            string strFilter = string.Empty;
            strDeptSql = string.Empty;

            if (Convert.ToString(this.txtDepartment.Tag).Trim() != "")
                strFilter += " AND D.DeptCode in (" + this.txtDepartment.Tag.ToString().Trim() + ")";

            if (Convert.ToString(this.txtTaxCode.Tag).Trim() != "")
                strFilter += " AND IT.TaxId IN (" + txtTaxCode.Tag.ToString() + ") AND D.IsTaxable = 1";

            strDeptSql = "SELECT DISTINCT D.DeptCode, D.DeptID, D.DeptName, D.IsTaxable, " +
                        " STUFF((SELECT DISTINCT ',' + CAST(c.TaxCode AS VARCHAR(100)) FROM ItemTax B INNER JOIN TaxCodes C ON C.TaxID = B.TaxID WHERE IT.EntityType = B.EntityType and IT.EntityID = B.EntityID FOR XML PATH(''), TYPE).value('.', 'NVARCHAR(MAX)')        ,1,1,'') TaxCodes, " +
                        " SD.SubDepartmentID, SD.DepartmentID, SD.Description " +
                        " FROM Department D " +
                        " LEFT JOIN ItemTax IT on cast(D.DeptID AS VARCHAR(100)) = IT.EntityID AND IT.EntityType = 'D' " +
                        " LEFT JOIN SubDepartment SD ON D.DeptID = SD.DepartmentID " +
                        " WHERE 1=1 " + strFilter;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            this.txtDepartment.Focus();
            PreviewReport(true);
        }
    }
}
