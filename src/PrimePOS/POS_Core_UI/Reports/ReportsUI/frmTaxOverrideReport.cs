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
    public partial class frmTaxOverrideReport : Form
    {
        public frmTaxOverrideReport()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnViewReport_Click(object sender, EventArgs e)
        {
            try
            {
                PreviewReport(false);
            }
            catch (Exception Ex)
            {
            }
        }

        private void btnPrintReport_Click(object sender, EventArgs e)
        {
            try
            {
                PreviewReport(true);
            }
            catch (Exception Ex)
            {
            }
        }

        private void PreviewReport(bool blnPrint)
        {
            try
            {
                this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
                rptTaxOverride oRpt = new rptTaxOverride();

                string sSQL = GetSelectQuery();
                clsReports.setCRTextObjectText("txtFromDate", "From :" + this.dtpFromDate.Text, oRpt);
                clsReports.setCRTextObjectText("txtToDate", "To :" + dtpToDate.Text, oRpt);

                DataSet ds = clsReports.GetReportSource(sSQL);
                oRpt.SetDataSource(ds.Tables[0]);
                clsReports.DStoExport = ds;
                clsReports.Preview(blnPrint, oRpt);
            }
            catch (Exception Ex)
            {
            }
            finally
            {
                this.Cursor = System.Windows.Forms.Cursors.Default;
            }
        }

        private string GetSelectQuery()
        {
            string sSQL = string.Empty;
            try
            {
                string strOverrideOptions = string.Empty;
                if (cboTransType.SelectedIndex == 0)
                {
                    if (rdbAllItem.Checked)
                        strOverrideOptions = "8,9,10,11";
                    else if (rdbOnlyRxItem.Checked)
                        strOverrideOptions = "9,11";
                    else if (rdbOnlyOTCItem.Checked)
                        strOverrideOptions = "8,10";
                }
                else if (cboTransType.SelectedIndex == 1)   //Tax Override
                {
                    if (rdbAllItem.Checked)
                        strOverrideOptions = "10,11";
                    else if (rdbOnlyRxItem.Checked)
                        strOverrideOptions = "11";
                    else if (rdbOnlyOTCItem.Checked)
                        strOverrideOptions = "10";
                }
                else if (cboTransType.SelectedIndex == 2)   //Tax Override All
                {
                    if (rdbAllItem.Checked)
                        strOverrideOptions = "8,9";
                    else if (rdbOnlyRxItem.Checked)
                        strOverrideOptions = "9";
                    else if (rdbOnlyOTCItem.Checked)
                        strOverrideOptions = "8";
                }

                sSQL = "SELECT DISTINCT a.OverrideFieldId, b.OverrideFieldCaption, e.ItemID, d.ItemDescription AS Description, a.TransID, a.TransDetailID, c.TransDate, OldTax.PreviousTaxDetails, NewTax.NewTaxDetails, d.TaxAmount, d.ExtendedPrice, a.OverrideBy, c.UserID AS LoggedInUser FROM OverrideDetails a " +
                        " LEFT JOIN(select t1.TransDetailID, stuff((SELECT distinct ', ' + cast(t3.TaxCode + '-' + t3.Description + '-' + ISNULL(t2.NewValue, '') as varchar(100)) FROM OverrideDetails t2 LEFT JOIN TaxCodes t3 ON t3.TaxID = CAST(ISNULL(t2.OldValue, 0) AS int) where t2.TransDetailID = t1.TransDetailID " +
                                        " FOR XML PATH('')), 1, 1, '') AS PreviousTaxDetails " +
                                        " from OverrideDetails t1 where OverrideFieldId IN (" + strOverrideOptions + ") group by t1.TransDetailID) OldTax ON a.TransDetailID = OldTax.TransDetailID " +
                        " LEFT JOIN (select t1.TransDetailID, stuff((SELECT distinct ', ' + cast(t3.TaxCode + '-' + t3.Description + '-' + CAST(t2.TaxPercent AS VARCHAR(10)) AS varchar(100)) FROM POSTransactionDetailTax t2 LEFT JOIN TaxCodes t3 ON t3.TaxID = t2.TaxID where t2.TransDetailID = t1.TransDetailID " +
                                        " FOR XML PATH('')), 1, 1, '') AS NewTaxDetails " +
                        " from OverrideDetails t1 where OverrideFieldId IN (" + strOverrideOptions + ") group by t1.TransDetailID) NewTax ON a.TransDetailID = NewTax.TransDetailID " +
                        " INNER JOIN OverrideFields b ON b.OverrideFieldId = a.OverrideFieldId " +
                        " INNER JOIN POSTransaction c ON c.TransID = a.TransID " +
                        " INNER JOIN POSTransactionDetail d ON d.TransID = c.TransID AND d.TransID = a.TransID AND d.TransDetailID = a.TransDetailID " +
                        " INNER JOIN Item e ON e.ItemID = d.ItemID " +
                        " WHERE a.OverrideFieldId IN (" + strOverrideOptions + ") " +
                        " AND convert(datetime,c.TransDate,113) >= convert(datetime, cast('" + this.dtpFromDate.Text + " 00:00:00' as datetime), 113) and convert(datetime,c.TransDate,113) <= convert(datetime, cast('" + this.dtpToDate.Text + " 23:59:59' as datetime), 113) " +
                        " ORDER BY a.TransID, a.OverrideFieldId, a.TransDetailID";
            }
            catch (Exception Ex)
            {
            }
            return sSQL;
        }

        private void frmTaxOverrideReport_Load(object sender, EventArgs e)
        {
            this.dtpFromDate.Enter += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
            this.dtpFromDate.Leave += new System.EventHandler(clsUIHelper.AfterExitEditMode);

            this.dtpToDate.Enter += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
            this.dtpToDate.Leave += new System.EventHandler(clsUIHelper.AfterExitEditMode);

            clsUIHelper.setColorSchecme(this);
            cboTransType.AlwaysInEditMode = false;
            this.dtpFromDate.Value = DateTime.Now;
            this.dtpToDate.Value = DateTime.Now;
            cboTransType.SelectedIndex = 0;
        }

        private void frmTaxOverrideReport_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Escape)
                {
                    this.Close();
                }
            }
            catch (Exception Ex)
            {
            }
        }
    }
}
