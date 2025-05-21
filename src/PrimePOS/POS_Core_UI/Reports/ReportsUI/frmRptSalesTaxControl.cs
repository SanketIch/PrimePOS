using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using CrystalDecisions.CrystalReports.Engine;
using POS_Core.BusinessRules;
//using POS.UI;
using POS_Core_UI.Reports.Reports;
using CrystalDecisions.Shared;

namespace POS_Core_UI.Reports.ReportsUI
{
    public partial class frmRptSalesTaxControl : Form
    {
        public frmRptSalesTaxControl()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            Preview(true);
        }

        private void Preview(bool PrintIt)
        {
            try
            {
                ReportClass oRpt = null;
                DataSet oDS = Search();
                oRpt = new rptSalesTaxControl();
                oRpt.Database.Tables[0].SetDataSource(oDS.Tables[0]);
               
                String arFlag = string.Empty;

                if (chbARFlag.Checked)
                {
                    clsReports.SetRepParam(oRpt, "ARFlag", "true");
                }
                else
                {
                    clsReports.SetRepParam(oRpt, "ARFlag", "false");
                }
                clsReports.DStoExport = oDS; //PRIMEPOS-2471 16-Feb-2021 JY Added
                clsReports.Preview(PrintIt, oRpt);
            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }

        public DataSet Search()
        {
            DataSet oData = new DataSet();

            Search oSearch = new Search();
            DateTime startDate = (DateTime)dtpSaleStartDate.Value;
            DateTime endDate = (DateTime)dtpSaleEndDate.Value;

            String whereClause = " AND a.closedate BETWEEN '" + startDate.ToString("MM-dd-yyyy") + " 00:00:00" + "' And '" + endDate.ToString("MM-dd-yyyy") + " 23:59:59'";
            //String whereClause = " and a.closedate BETWEEN '" + dtpSaleStartDate.Text + "' And '" + dtpSaleEndDate.Text + "'";

            //PRIMEPOS-2782 24-Jan-2020 JY commented
            //string sSQL = " select distinct a.closedate as ClosedDate, a.EODID, " +
            //              " isnull((select SUM(EndOfDayDetail.Amount) from EndOfDayDetail,EndOfDayHeader where  PayTypeID='1' and  EndOfDayDetail.EODID = EndOfDayHeader.EODID and EndOfDayHeader.EODID=a.EODID and CONVERT(varchar,closedate ,112) = CONVERT(varchar,a.closedate,112)),0.00) as 'Cash Received', " +
            //              " isnull((select SUM(EndOfDayDetail.Amount) from EndOfDayDetail,EndOfDayHeader where PayTypeID in ('4','5','3','7','6')  and EndOfDayDetail.EODID = EndOfDayHeader.EODID and EndOfDayHeader.EODID=a.EODID and CONVERT(varchar,closedate ,112) = CONVERT(varchar,a.closedate,112)),0.00) as 'Credit Card Received', " +
            //              " isnull(a.TotalSales - (select isnull(SUM(EndOfDayDetail.Amount),0) from EndOfDayDetail,EndOfDayHeader where PayTypeID in ('H')   and EndOfDayDetail.EODID = EndOfDayHeader.EODID and EndOfDayHeader.EODID=a.EODID and CONVERT(varchar,closedate ,112) = CONVERT(varchar,a.closedate,112)) + isnull(a.TotalTax,0) - isnull(a.TotalDiscount,0),0.00) as 'Total Collected', " +
            //              " isnull(a.TotalSales - (select isnull(SUM(EndOfDayDetail.Amount),0) from EndOfDayDetail,EndOfDayHeader where PayTypeID in ('H')   and EndOfDayDetail.EODID = EndOfDayHeader.EODID and EndOfDayHeader.EODID=a.EODID and CONVERT(varchar,closedate ,112) = CONVERT(varchar,a.closedate,112)),0.00) as 'Net Sales',  " +
            //              " isnull((select SUM(EndOfDayDetail.Amount) from EndOfDayDetail,EndOfDayHeader where PayTypeID in ('H')   and EndOfDayDetail.EODID = EndOfDayHeader.EODID and EndOfDayHeader.EODID=a.EODID and CONVERT(varchar,closedate ,112) = CONVERT(varchar,a.closedate,112)),0.00) as AR, " +
            //              " isnull((select sum(Price * qty) from postransaction, postransactiondetail, item  where postransactiondetail.transid = postransaction.transid  and postransactiondetail.itemid = item.itemid  and item.istaxable = 0 and CONVERT(varchar,postransaction.TransDate,112) = CONVERT(varchar,a.closedate,112) and postransaction.EODID=a.EODID),0.00) as 'Exempt Sales' ,   " +
            //              " isnull((select sum(Price * qty) from postransaction, postransactiondetail, item  where postransactiondetail.transid = postransaction.transid  and postransactiondetail.itemid = item.itemid  and item.istaxable = 1 and CONVERT(varchar,postransaction.TransDate,112) = CONVERT(varchar,a.closedate,112) and postransaction.EODID=a.EODID) + isnull(a.TotalTax,0),0.00) as 'Taxable Sales' , " +
            //              " isnull(a.TotalTax,0.00) as 'Sales Tax Collected' " +
            //              " from EndOfDayHeader a, EndOfDayDetail b where a.EODID = b.EODID " + whereClause;

            //PRIMEPOS-2782 24-Jan-2020 JY Corrected query
            //PRIMEPOS-2817 04-Mar-2020 JY modified queriy for prescription sales
            string sSQL = " SELECT DISTINCT a.closedate as ClosedDate, a.EODID, " +
                        " ISNULL((SELECT SUM(bb.Amount) FROM EndOfDayHeader aa INNER JOIN EndOfDayDetail bb ON  aa.EODID = bb.EODID WHERE bb.PayTypeID = '1' AND aa.EODID = a.EODID), 0.00) AS 'Cash Received', " +
                        " ISNULL((select SUM(bb.Amount) from EndOfDayHeader aa INNER JOIN EndOfDayDetail bb ON aa.EODID = bb.EODID WHERE bb.PayTypeID IN ('3','4','5','6','7') AND aa.EODID = a.EODID), 0.00) as 'Credit Card Received', " +
                        " ISNULL((SELECT SUM(bb.Amount) FROM EndOfDayHeader aa INNER JOIN EndOfDayDetail bb ON aa.EODID = bb.EODID WHERE bb.PayTypeID NOT IN ('1', '3', '4', '5', '6', '7') AND aa.EODID = a.EODID), 0.00) AS 'Other Paytypes Received', " +
                        " ISNULL((SELECT SUM(bb.Amount) FROM EndOfDayHeader aa INNER JOIN EndOfDayDetail bb ON aa.EODID = bb.EODID WHERE aa.EODID = a.EODID), 0.00) AS 'Total Collected', " + 
                        " ISNULL((SELECT SUM(ISNULL(aa.TotalSales, 0)) + SUM(ISNULL(aa.TotalReturns, 0)) FROM EndOfDayHeader aa WHERE aa.EODID = a.EODID), 0.00) AS 'Gross Sales', " +
                        " ISNULL((SELECT SUM(bb.Amount) FROM EndOfDayHeader aa INNER JOIN EndOfDayDetail bb ON aa.EODID = bb.EODID WHERE bb.PayTypeID IN ('H') AND aa.EODID = a.EODID), 0.00) AS AR, " +
                        " ISNULL((SELECT ISNULL(SUM(PTD.ExtendedPrice), 0) FROM POSTransaction PT INNER JOIN POSTransactionDetail PTD ON PT.TransID = PTD.TransID LEFT OUTER JOIN POSTransactionRXDetail PTDRX ON PTD.TransDetailID = PTDRX.TransDetailID " +
                            " WHERE PTD.TaxAmount = 0 AND PT.TransType = 1 and PTDRX.TransDetailID IS NULL AND PT.EODID = a.EODID), 0.00) AS 'Exempt Sales', " + 
                        " ISNULL((SELECT ISNULL(SUM(PTD.ExtendedPrice), 0) FROM POSTransaction PT INNER JOIN POSTransactionDetail PTD ON PT.TransID = PTD.TransID LEFT OUTER JOIN POSTransactionRXDetail PTDRX ON PTD.TransDetailID = PTDRX.TransDetailID " +
                            " WHERE PTD.TaxAmount > 0 and PT.TransType = 1 AND PTDRX.TransDetailID IS NULL AND PT.EODID = a.EODID), 0.00) AS 'Taxable Sales', " +
                        " ISNULL((SELECT ISNULL(SUM(ptd.ExtendedPrice), 0) FROM POSTransaction PT INNER JOIN POSTransactionDetail PTD ON PT.TransID = PTD.TransID INNER JOIN (SELECT DISTINCT RXCode FROM Util_POSSET cc) c ON LTRIM(RTRIM(UPPER(ptd.ItemID))) = LTRIM(RTRIM(UPPER(c.RXCode))) " +
                            " WHERE PT.TransType != 2 and PT.EODID = a.EODID), 0.00) AS 'Prescriptions Sales', " +//PRIMEPOS-2813 20-Feb-2020 JY Added Prescriptions Sales
                        " ISNULL(a.TotalTax, 0.00) AS 'Sales Tax Collected', ISNULL(a.TotalDiscount, 0.00) AS 'Total Discount'" +
                        ", ISNULL(a.TotalTransFee,0.00) AS 'Total Trans Fee'" + //PRIMEPOS-3119 11-Aug-2022 JY Added TotalTransFee
                        " FROM EndOfDayHeader a, EndOfDayDetail b where a.EODID = b.EODID " + whereClause;

            oData = oSearch.SearchData(sSQL);
            return oData;
        }

        private void frmRptSalesTaxControl_Load(object sender, EventArgs e)
        {
            dtpSaleEndDate.Value = DateTime.Now;
            dtpSaleStartDate.Value = DateTime.Now;

            this.dtpSaleStartDate.Enter += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
            this.dtpSaleStartDate.Leave += new System.EventHandler(clsUIHelper.AfterExitEditMode);
            this.dtpSaleEndDate.Enter += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
            this.dtpSaleEndDate.Leave += new System.EventHandler(clsUIHelper.AfterExitEditMode);

            clsUIHelper.setColorSchecme(this);
        }

        private void btnView_Click(object sender, EventArgs e)
        {
            Preview(false);
        }

        private void frmRptSalesTaxControl_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyData == System.Windows.Forms.Keys.Escape)
                {
                    this.Close();
                }
            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }

    }
}