using POS_Core.CommonData;
using POS_Core.CommonData.Rows;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace POS_Core_UI.UI
{
    public partial class frmPOSTransaction
    {
        #region Department

        //completed by sandeep
        private void txtDepartmentCode_KeyDown(object sender, KeyEventArgs e)
        {
            if (txtDepartmentCode.ContainsFocus) {
                if (e.KeyData == Keys.Enter && string.IsNullOrEmpty(this.txtDepartmentCode.Text) == true) {
                    ValidateRow(this, new CancelEventArgs());
                } else if (e.KeyData == Keys.Enter && string.IsNullOrEmpty(this.txtDepartmentCode.Text) == false) {
                    SearchDeptCode();
                }
            }
        }

        //completed by sandeep
        private void txtDepartmentCode_EditorButtonClick(object sender, Infragistics.Win.UltraWinEditors.EditorButtonEventArgs e)
        {
            try {
                SearchDeptCode();
            } catch (Exception ex) {
                clsUIHelper.ShowErrorMsg(ex.Message);
            }
        }
        #endregion

        #region DepartmentMethod

        //completed by sandeep
        private void SearchDeptCode()
        {
            try {
                logger.Trace("SearchDeptCode() - " + clsPOSDBConstants.Log_Entering);
                frmSearch oSearch = new frmSearch(clsPOSDBConstants.Department_tbl, this.txtDepartmentCode.Text, "");
                oSearch.searchInConstructor = true;
                oSearch.ShowDialog(this);
                if (!oSearch.IsCanceled) {
                    string strCode = oSearch.SelectedRowID();
                    if (strCode == "")
                        return;
                    EditDepartment(strCode, clsPOSDBConstants.Department_tbl);
                    ValidateRow(this, new CancelEventArgs(false));
                } else {
                    this.txtDepartmentCode.Tag = "";
                    return;
                }
                logger.Trace("SearchDeptCode() - " + clsPOSDBConstants.Log_Exiting);
            } catch (Exception exp) {
                logger.Fatal(exp, "SearchDeptCode()");
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }

        //completed by sandeep
        private void EditDepartment(string code, string senderName)
        {
            try {
                if (senderName == clsPOSDBConstants.Department_tbl) {
                    #region Department

                    try {
                        DepartmentData oDeptData = new DepartmentData();
                        oDeptData = oPOSTrans.PopulateDepartment(code);
                        if (oDeptData.Department.Rows.Count > 0) {
                            oPOSTrans.oDeptRow = (DepartmentRow)oDeptData.Tables[0].Rows[0];
                            this.txtDepartmentCode.Tag = oPOSTrans.oDeptRow.DeptID;
                            this.txtDepartmentCode.Text = oPOSTrans.oDeptRow.DeptCode;
                        }
                    } catch (Exception exp) {
                        clsUIHelper.ShowErrorMsg(exp.Message);
                        this.oPOSTrans.oTDRow.TaxCode = String.Empty;
                        this.oPOSTrans.oTDRow.TaxID = 0;
                        this.oPOSTrans.oTDRow.TaxAmount = 0;
                        SearchDeptCode();
                    }

                    #endregion Department
                }
            } catch (Exception Ex) {
                logger.Fatal(Ex, "EditDepartment()");
                clsUIHelper.ShowErrorMsg(Ex.Message);
            }
        }

        #endregion
    }
}
