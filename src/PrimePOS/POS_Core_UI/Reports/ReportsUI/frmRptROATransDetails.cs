using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using POS_Core.CommonData.Rows;
using POS_Core.CommonData;
using POS_Core.BusinessRules;
using POS_Core.ErrorLogging;
using Infragistics.Win.UltraWinEditors;
//using POS.UI;
using POS_Core_UI.Reports.Reports;
using System.Data;
using PharmData;
using POS_Core.Resources;
//using POS_Core.DataAccess;

namespace POS_Core_UI.Reports.ReportsUI
{
    public partial class frmRptROATransDetails : Form
    {
        string strCustCode = "";
        public frmRptROATransDetails()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        private void btnView_Click(object sender, EventArgs e)
        {
            PreviewReport(false);
        }

        private void frmRptROA_Load(object sender, EventArgs e)
        {
            this.dtStartDate.Enter += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
            this.dtStartDate.Leave += new System.EventHandler(clsUIHelper.AfterExitEditMode);

            this.dtEndDate.Enter += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
            this.dtEndDate.Leave += new System.EventHandler(clsUIHelper.AfterExitEditMode);

            this.txtUserID.AfterEnterEditMode += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
            this.txtUserID.AfterExitEditMode += new System.EventHandler(clsUIHelper.AfterExitEditMode);

            this.Left = (frmMain.getInstance().Width - frmMain.getInstance().ultraExplorerBar1.Width - this.Width) / 2;
            this.Top = (frmMain.getInstance().Height - this.Height) / 2;

            clsUIHelper.setColorSchecme(this);
            this.dtEndDate.Value = DateTime.Now;
            this.dtStartDate.Value = DateTime.Now;
        }
        private void PreviewReport(bool blnPrint)
        {
            try
            {
                this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
                rptROATransDetails oRpt = new rptROATransDetails();
                Search oSearch = new Search();
                String strQuery="";

                strQuery = "SELECT PT.TransId, Posset.StationID, LTRIM(RTRIM(C.CustomerName + ' ' + C.FIRSTNAME)) AS CustomerName, '' AS AccountName, '' AS AccountNo, PT.CustomerID, PT.TransID, " +
                            "PTP.PayTypeDesc, PT.TransDate, PT.UserID, PTY.Amount AS ROA, PTY.RefNo, PTY.AuthNo " +
                            ",'" + this.dtStartDate.Text + "' AS StartDate, '" + this.dtEndDate.Text + "' AS EndDate " + 
                            " FROM postransaction PT left join Util_POSSet POSSet on POSSet.StationID = PT.StationID " +
                            "INNER JOIN POSTransPayment PTY ON PT.TransID = PTY.TransID " +
                            "INNER JOIN PayType PTP ON PTY.TransTypeCode = PTP.PayTypeID " +
                            "LEFT JOIN Customer C ON C.CustomerID = PT.CustomerID " +
                            "WHERE PT.TransType = '3' and convert(datetime,PT.TransDate,109) between convert(datetime, cast('" + this.dtStartDate.Text + " 00:00:00' as datetime) ,113) and convert(datetime, cast('" + this.dtEndDate.Text + " 23:59:59' as datetime) ,113) ";

                DataSet ds = clsReports.GetReportSource(strQuery);

                if (this.optSummary.Checked == true)
                {
                    oRpt.DetailSection1.SectionFormat.EnableSuppress = true;
                }
                else
                {
                    PharmBL oPharmBL = new PharmBL();
                    for (int iCnt = 0; iCnt < ds.Tables[0].Rows.Count; iCnt++)
                    {
                        DataTable dt = oPharmBL.GetHCAccountDetailsByPosTransId(Configuration.convertNullToInt64(ds.Tables[0].Rows[iCnt]["TransId"]));
                        if (dt != null && dt.Rows.Count > 0)
                        {
                            ds.Tables[0].Rows[iCnt]["AccountName"] = Configuration.convertNullToString(dt.Rows[0]["ACCT_NAME"]).Trim();
                            ds.Tables[0].Rows[iCnt]["AccountNo"] = Configuration.convertNullToString(dt.Rows[0]["ACCT_NO"]);
                        }
                    }
                    ds.Tables[0].AcceptChanges();
                }
                
                oRpt.Database.Tables[0].SetDataSource(ds.Tables[0]);
                clsReports.DStoExport = ds; //PRIMEPOS-2471 16-Feb-2021 JY Added
                clsReports.Preview(blnPrint, oRpt);
                this.Cursor = System.Windows.Forms.Cursors.Default;

            }
            catch (Exception exp)
            {
                this.Cursor = System.Windows.Forms.Cursors.Default;
                if (Configuration.CPOSSet.UsePrimeRX)   //PRIMEPOS-3106 22-Jun-2022 JY Added
                    clsUIHelper.ShowErrorMsg(exp.Message);
            }

        }

        private void btnPrint_Click(object sender, System.EventArgs e)
        {
            PreviewReport(true);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void SearchHouseChargeCustomers()
        {
            //frmSearch oSearch = new frmSearch(clsPOSDBConstants.PrimeRX_HouseChargeInterface);
            frmSearchMain oSearch = new frmSearchMain(true);    //20-Dec-2017 JY Added new reference
            oSearch.SearchTable = clsPOSDBConstants.PrimeRX_HouseChargeInterface;   //20-Dec-2017 JY Added 
            try
            {
                oSearch.SearchInConstructor = true;//Added By Shitaljit(QuicSolv) on 13 Sept 2011
                oSearch.ShowDialog(this);
                if (!oSearch.IsCanceled)
                {
                     strCustCode = oSearch.SelectedRowID();

                     if (strCustCode == "")
                        return;
                    this.txtCustomer.Text = strCustCode;
                    this.lblCustomerName.Text = oSearch.SelectedRowCode.Trim() ;
                }
            }
            catch (Exception Ex)
            {
                clsUIHelper.ShowErrorMsg(Ex.Message);
            }
        }

        private void txtCustomer_EditorButtonClick(object sender, EditorButtonEventArgs e)
        {
            SearchHouseChargeCustomers();
        }

        private void frmRptROATransDetails_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == System.Windows.Forms.Keys.Enter)
            {
                this.SelectNextControl(this.ActiveControl, true, true, true, true);
            }
            if (e.KeyData == Keys.F4 && this.txtCustomer.ContainsFocus == true)
            {
                SearchHouseChargeCustomers();
            }
            else if (e.KeyData == Keys.Escape)
            {
                this.Close();
            }
        }

    }
}