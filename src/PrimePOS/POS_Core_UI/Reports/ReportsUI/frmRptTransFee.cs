using Infragistics.Win.UltraWinGrid;
using POS_Core.BusinessRules;
using POS_Core.CommonData;
using POS_Core.DataAccess;
using POS_Core_UI.Reports.Reports;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace POS_Core_UI.Reports.ReportsUI
{
    public partial class frmRptTransFee : Form
    {
        POS_Core.DataAccess.SearchSvr SearchSvr = new POS_Core.DataAccess.SearchSvr();

        public frmRptTransFee()
        {
            InitializeComponent();
        }

        private void frmRptTransFee_Load(object sender, EventArgs e)
        {
            cboTransType.AlwaysInEditMode = false;

            this.dtpStartDate.Enter += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
            this.dtpStartDate.Leave += new System.EventHandler(clsUIHelper.AfterExitEditMode);

            this.dtpEndDate.Enter += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
            this.dtpEndDate.Leave += new System.EventHandler(clsUIHelper.AfterExitEditMode);

            populateTransType();
            FillPayType();

            clsUIHelper.setColorSchecme(this);
            this.dtpStartDate.Value = DateTime.Now;
            this.dtpEndDate.Value = DateTime.Now;
        }

        private void frmRptTransFee_KeyDown(object sender, KeyEventArgs e)
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

        private void populateTransType()
        {
            try
            {
                this.cboTransType.Items.Add("All", "All");
                this.cboTransType.Items.Add("Sales", "Sales");
                this.cboTransType.Items.Add("Returns", "Returns");
                this.cboTransType.Items.Add("ROA", "ROA");
                this.cboTransType.SelectedIndex = 0;
            }
            catch (Exception) { }
        }

        private void FillPayType()
        {
            TransFee oTransFee = new TransFee();
            DataTable dt = oTransFee.GetPayTypeData(false);
            dataGridList.DataSource = dt;
            for (int i = 1; i <= dt.Columns.Count; i++)
            {
                dataGridList.Columns[i].ReadOnly = true;
                if (i == 1)
                {
                    dataGridList.Columns[i].Width = 30;
                    dataGridList.Columns[i].Name = "PayType";
                }
                else
                    dataGridList.Columns[i].Width = 115;
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
                    {
                        dataGridList.Rows[i].Cells[0].Value = true;
                        chkSelectAll.Text = "Unselect All";
                    }
                    else
                    {
                        dataGridList.Rows[i].Cells[0].Value = false;
                        chkSelectAll.Text = "Select All";
                    }
                }
            }
        }
        
        private void btnPayTypeList_Click(object sender, EventArgs e)
        {
            if (grpPayTypeList.Visible == false)
                PayTypeList_Expand(true);
            else
            {
                PayTypeList_Expand(false);
                GetSelectedPayTypes();
            }
        }

        private void PayTypeList_Expand(bool Value)
        {
            if (Value == true)
            {
                if (grpPayTypeList.Visible == false)
                {
                    dataGridList.Visible = true;
                    grpPayTypeList.Visible = true;
                    dataGridList.Height = 230;
                    grpPayTypeList.Height = dataGridList.Height + 31;
                    chkSelectAll.Location = new Point(chkSelectAll.Location.X, dataGridList.Height + 10);
                    grpPayTypeList.Focus();
                }
            }
            else
            {
                if (grpPayTypeList.Visible == true)
                {
                    dataGridList.Visible = false;
                    grpPayTypeList.Visible = false;
                }
            }
        }

        public void GetSelectedPayTypes()
        {
            string strPayTypeCode = "";
            string strPayTypeName = "";
            foreach (DataGridViewRow oRow in dataGridList.Rows)
            {
                if (POS_Core.Resources.Configuration.convertNullToBoolean(oRow.Cells[0].Value))
                {
                    strPayTypeCode += ",'" + oRow.Cells[1].Value.ToString() + "'";
                    strPayTypeName += "," + oRow.Cells["PayTypeDesc"].Value.ToString();
                }
            }

            if (strPayTypeCode.StartsWith(","))
            {
                strPayTypeCode = strPayTypeCode.Substring(1);
                strPayTypeName = strPayTypeName.Substring(1);
            }
            txtPaymentType.Text = strPayTypeName;
            txtPaymentType.Tag = strPayTypeCode;
        }

        private void txtPaymentType_EditorButtonClick(object sender, Infragistics.Win.UltraWinEditors.EditorButtonEventArgs e)
        {
            SearchPayType();
        }

        private void SearchPayType()
        {
            try
            {
                if (this.txtPaymentType.ButtonsRight[0].Enabled == false)
                {
                    return;
                }

                frmSearchMain oSearch = new frmSearchMain(true);
                oSearch.SearchTable = clsPOSDBConstants.PayType_tbl;
                oSearch.AllowMultiRowSelect = true;
                oSearch.ShowDialog(this);
                if (!oSearch.IsCanceled)
                {
                    string strPayTypeCode = "";
                    string strPayTypeName = "";
                    foreach (UltraGridRow oRow in oSearch.grdSearch.Rows)
                    {
                        if ((bool)oRow.Cells["check"].Value == true)
                        {
                            strPayTypeCode += ",'" + oRow.Cells["Code"].Text + "'";
                            strPayTypeName += "," + oRow.Cells["Description"].Text;
                        }
                    }

                    if (strPayTypeCode.StartsWith(","))
                    {
                        strPayTypeCode = strPayTypeCode.Substring(1);
                        strPayTypeName = strPayTypeName.Substring(1);
                    }
                    txtPaymentType.Text = strPayTypeName;
                    txtPaymentType.Tag = strPayTypeCode;
                }
                else
                {
                    txtPaymentType.Text = string.Empty;
                    txtPaymentType.Tag = string.Empty;
                }
            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }        

        private void dataGridList_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            for (int i = 0; i < dataGridList.Rows.Count; i++)
            {
                dataGridList.Rows[i].Cells[dataGridList.Columns["chkBox"].Index].Value = true;
            }
            chkSelectAll.Checked = true;
            GetSelectedPayTypes();
        }

        private void btnView_Click(object sender, EventArgs e)
        {
            PreviewReport(false);
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            PreviewReport(true);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void PreviewReport(bool blnPrint)
        {
            String strSQL;
            try
            {
                this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
                RptTransFee oRpt = new RptTransFee();
                GenerateSQL(out strSQL, false);
                DataSet ds = clsReports.GetReportSource(strSQL);
                oRpt.SetDataSource(ds.Tables[0]);
                clsReports.DStoExport = ds;
                clsReports.setCRTextObjectText("txtFromDate", "From :" + this.dtpStartDate.Text, oRpt);
                clsReports.setCRTextObjectText("txtToDate", "To :" + dtpEndDate.Text, oRpt);
                clsReports.Preview(blnPrint, oRpt);
                this.Cursor = System.Windows.Forms.Cursors.Default;
            }
            catch (Exception exp)
            {                
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
            finally 
            {
                this.Cursor = System.Windows.Forms.Cursors.Default;
            }
        }

        private void GenerateSQL(out string strSQL, Boolean Flag)
        {
            string strFilter = string.Empty;

            try
            {
                if (this.cboTransType.SelectedIndex == 1)
                {
                    strFilter = " AND a.TransType = 1";
                }
                else if (this.cboTransType.SelectedIndex == 2)
                {
                    strFilter = " AND a.TransType = 2";
                }
                else if (this.cboTransType.SelectedIndex == 3)
                {
                    strFilter = " AND a.TransType = 3";
                }

                if (this.txtPaymentType.Text.Trim() != "" && txtPaymentType.Tag != null && txtPaymentType.Tag.ToString().Trim().Length > 0)
                {
                    strFilter += " AND a.TransID IN (SELECT TransId FROM POSTransPayment WHERE TransTypeCode in (" + this.txtPaymentType.Tag.ToString().Trim() + ") )";
                }

                strSQL = "SELECT a.TransID, a.TransDate, CASE WHEN a.TransType = 1 THEN 'Sale' WHEN a.TransType = 2 THEN 'Return' WHEN a.TransType = 3 THEN 'ROA' END AS TransType," +
                        " STUFF((SELECT ',' + bb.PayTypeDesc FROM POSTransPayment aa" +
                                " INNER JOIN PayType bb ON aa.TransTypeCode = bb.PayTypeID" +
                                " WHERE aa.TransID = a.TransID FOR XML PATH('')), 1, 1, '') AS PaymentType," +
                        " a.GrossTotal, a.TotalDiscAmount, a.TotalTaxAmount, a.TotalTransFeeAmt, a.TotalPaid FROM POSTransaction a" +
                        " WHERE ISNULL(a.TotalTransFeeAmt,0) > 0 AND CONVERT(datetime,a.TransDate,113) between CONVERT(datetime, cast('" + this.dtpStartDate.Text + " 00:00:00' as datetime) ,113) and CONVERT(datetime, cast('" + this.dtpEndDate.Text + " 23:59:59' as datetime) ,113) " + strFilter;
            }
            catch (Exception Ex)
            {
                strSQL = "";
            }
        }

        private void btnPayTypeList_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == System.Windows.Forms.Keys.Down)
            {
                dataGridList.Focus();
            }
        }

        private void btnPayTypeList_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyData == System.Windows.Forms.Keys.Enter)
            {
                if (grpPayTypeList.Visible == true)
                {
                    dataGridList.Visible = false;
                    grpPayTypeList.Visible = false;
                }
            }
        }
    }
}
