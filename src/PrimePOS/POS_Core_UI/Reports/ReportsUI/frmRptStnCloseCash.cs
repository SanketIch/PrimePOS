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
using POS_Core.DataAccess;

using System.Windows.Forms;

namespace POS_Core_UI.Reports.ReportsUI
{
    /// <summary>
    /// Added by Krishna on 14 june 2011
    /// UI to Show The Station Close Cash Report
    /// </summary>
    public partial class frmRptStnCloseCash : Form
    {
        public frmRptStnCloseCash()
        {
            InitializeComponent();
        }

        private void frmRptStnCloseCash_Load(object sender, EventArgs e)
        {
            this.cboUsers.AfterEnterEditMode += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
            this.cboUsers.AfterExitEditMode += new System.EventHandler(clsUIHelper.AfterExitEditMode);
            this.cboStnId.AfterEnterEditMode += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
            this.cboStnId.AfterExitEditMode += new System.EventHandler(clsUIHelper.AfterExitEditMode);
            this.cboCashDiff.AfterEnterEditMode += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
            this.cboCashDiff.AfterExitEditMode += new System.EventHandler(clsUIHelper.AfterExitEditMode);
            dtpStartDate.Text = DateTime.Now.Date.ToString();
            dtpEndDate.Text = DateTime.Now.Date.ToString();
            clsUIHelper.setColorSchecme(this);

            PopulateUsers();
            FillStationIDs();
            cboCashDiff.SelectedIndex = 0;
        }

        //------------------------------------------------------------------------------------------------------------------
        private void PopulateUsers()
        {
            SearchSvr oSearchSvr = new SearchSvr();
            
            DataSet UserDS = oSearchSvr.Search(clsPOSDBConstants.Users_tbl, "", "", 0, -1);
            this.cboUsers.Items.Add("ALL");
            foreach (DataRow row in UserDS.Tables[0].Rows)
            {
                cboUsers.Items.Add(row["UserId"].ToString());
            }
            this.cboUsers.SelectedIndex = 0;
        }
        //------------------------------------------------------------------------------------------------------------------
        private void FillStationIDs()
        {
            try
            {
                DataSet oStationDs = new DataSet();
                SearchSvr oSearchSvr = new SearchSvr();
                string sSQL = "Select Distinct(StationID) From Util_POSSET";
                
                oStationDs.Tables.Add(oSearchSvr.Search(sSQL).Tables[0].Copy());
                oStationDs.Tables[0].TableName = "STATION";

                this.cboStnId.Items.Add("ALL");

                foreach (DataRow stationRow in oStationDs.Tables[0].Rows)
                {
                    this.cboStnId.Items.Add(stationRow["StationID"].ToString());
                }
                this.cboStnId.SelectedIndex = 0;
            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }

        private void btnView_Click(object sender, EventArgs e)
        {
            Preview(false);
        }
        //------------------------------------------------------------------------------------------------------------------
        private void Preview(bool PrintFlag)
        {
           try
           {
               string StartDate = dtpStartDate.Value.ToString();

               DateTime EndDate = DateTime.SpecifyKind((DateTime)dtpEndDate.Value, DateTimeKind.Local);
               EndDate = EndDate.AddDays(1);

               string sqlQuery = BuildQuery(StartDate, EndDate.ToString());


               frmReportViewer FrmReportViewer = new frmReportViewer();
               dsStnCloseCash dsStnClCsh = new dsStnCloseCash();

               SqlConnection conn = new SqlConnection(POS_Core.Resources.Configuration.ConnectionString);
               SqlDataAdapter da = new SqlDataAdapter(sqlQuery, conn);

               da.Fill(dsStnClCsh, "StnCloseCash");
               double UndertotalAmt = 0;
               double OvertotalAmt = 0;
               double MatchtotalAmt = 0;
               double MatchDiffAmt = 0;
               int UnderCnt = 0, OverCnt = 0, MatchCnt = 0;
               foreach (DataRow row in dsStnClCsh.Tables["StnCloseCash"].Rows)
               {
                   if (row["Cash Entered"].ToString() == "")
                       row["Cash Entered"] = "0";
                   if (double.Parse(row["Cash Entered"].ToString()) - Double.Parse(row["TransAmount"].ToString()) < 0)
                   {
                       UndertotalAmt = UndertotalAmt + (double.Parse(row["Cash Entered"].ToString()) - Double.Parse(row["TransAmount"].ToString()));
                       UnderCnt = UnderCnt + 1;
                   }
                   else if (double.Parse(row["Cash Entered"].ToString()) - Double.Parse(row["TransAmount"].ToString()) > 0)
                   {
                       OvertotalAmt = OvertotalAmt + (double.Parse(row["Cash Entered"].ToString()) - Double.Parse(row["TransAmount"].ToString()));
                       OverCnt = OverCnt + 1;
                   }
                   else
                   {
                       MatchtotalAmt = MatchtotalAmt + double.Parse(row["TransAmount"].ToString());
                       MatchDiffAmt = MatchDiffAmt + (double.Parse(row["Cash Entered"].ToString()) - Double.Parse(row["TransAmount"].ToString()));
                       MatchCnt = MatchCnt + 1;
                   }
               }

               //double AvgDIFFperClose = 0;
               //AvgDIFFperClose = (UndertotalAmt + OvertotalAmt + MatchDiffAmt);
               //AvgDIFFperClose = Math.Round((AvgDIFFperClose / (UnderCnt + OverCnt + MatchCnt)) * 100, 2);

               POS_Core_UI.Reports.Reports.rptStnCloseCash oRptStnCloseCash = new POS_Core_UI.Reports.Reports.rptStnCloseCash();
               oRptStnCloseCash.SetDataSource(dsStnClCsh.Tables["StnCloseCash"]);

               string HeaderDate = dtpStartDate.Text.ToString() + " to " + dtpEndDate.Text.ToString();

               clsReports.setCRTextObjectText("HeaderDate", HeaderDate, oRptStnCloseCash);
               clsReports.setCRTextObjectText("TotalUnderCash", UndertotalAmt.ToString(), oRptStnCloseCash);
               clsReports.setCRTextObjectText("TotalOverCash", OvertotalAmt.ToString(), oRptStnCloseCash);
               clsReports.setCRTextObjectText("TotalMatchCash", MatchtotalAmt.ToString(), oRptStnCloseCash);

               clsReports.setCRTextObjectText("UnderCount", UnderCnt.ToString(), oRptStnCloseCash);
               clsReports.setCRTextObjectText("OverCount", OverCnt.ToString(), oRptStnCloseCash);
               clsReports.setCRTextObjectText("MatchCount", MatchCnt.ToString(), oRptStnCloseCash);
               //clsReports.setCRTextObjectText("AvgDiffperClose", AvgDIFFperClose.ToString(), oRptStnCloseCash);
               clsReports.SetReportHeader(oRptStnCloseCash,dsStnClCsh);
               
               FrmReportViewer.rvReportViewer.ReportSource = oRptStnCloseCash;
               FrmReportViewer.rvReportViewer.Refresh();

               if (PrintFlag)
                   clsReports.PrintReport(oRptStnCloseCash);
               else
                   FrmReportViewer.Show();
           }
           catch (Exception ex)
           {
               MessageBox.Show(ex.Message);
           }
        }
        //------------------------------------------------------------------------------------------------------------------
        private string BuildQuery(string StartDate, string EndDate)
        {
            try
            {
                string sqlQuery= "SELECT SH.StationCloseId, SH.StationID, SH.UserID, SH.CloseDate, "
                            + " ISNULL(SD.TransAmount,0) + ISNULL(SH.DefCDStartBalance,0) - ISNULL((SELECT SUM(P.Amount) FROM Payout P WHERE P.StationCloseID = SD.StationCloseID AND P.DrawNo = SD.DrawNo AND P.StationID = SH.StationID),0) AS TransAmount, " //Sprint-19 - 2165 24-Mar-2015 JY Added starting cash balance and payouts 
                            +" (SELECT SUM(Totalvalue) FROM StationCloseCash SC"
                            +" WHERE SC.StationCloseId=SH.StationCloseId ) AS [Cash Entered]"
                            +" FROM StationCloseDetail SD JOIN StationCloseHeader SH"
                            +" ON (SD.StationCloseId=SH.StationCloseID) ";
                
                string WhereQury =" WHERE  SH.CloseDate BETWEEN '"+StartDate+"' AND '"+EndDate+"'"
                            +" AND SD.TransType='U-1'";

                if (cboUsers.SelectedItem.ToString() != "ALL")
                    WhereQury = WhereQury + " AND SH.UserId='" + cboUsers.SelectedItem.ToString() + "'";

                if (cboStnId.SelectedItem.ToString() != "ALL")
                    WhereQury = WhereQury + " AND SH.StationId=" + cboStnId.SelectedItem.ToString() + "";
                #region Sprint-19 - 2165 25-Mar-2015 JY Commented
                //if (cboCashDiff.SelectedItem.ToString() == "Under")
                //    WhereQury = WhereQury + " AND (SELECT SUM(Totalvalue) FROM StationCloseCash SC"
                //            + " WHERE SC.StationCloseId=SH.StationCloseId )-SD.TransAmount<0 ";
                //else if(cboCashDiff.SelectedItem.ToString() == "Over")
                //    WhereQury = WhereQury + " AND (SELECT SUM(Totalvalue) FROM StationCloseCash SC"
                //            + " WHERE SC.StationCloseId=SH.StationCloseId )-SD.TransAmount>0 ";
                //else if (cboCashDiff.SelectedItem.ToString() == "Under and Over Only")
                //    WhereQury = WhereQury + " AND (SELECT SUM(Totalvalue) FROM StationCloseCash SC"
                //            + " WHERE SC.StationCloseId=SH.StationCloseId )-SD.TransAmount!=0 ";
                //else if (cboCashDiff.SelectedItem.ToString() == "Match")
                //    WhereQury = WhereQury + " AND (SELECT SUM(Totalvalue) FROM StationCloseCash SC"
                //            + " WHERE SC.StationCloseId=SH.StationCloseId )-SD.TransAmount=0 ";
                #endregion

                #region Sprint-19 - 2165 25-Mar-2015 JY Added
                if (cboCashDiff.SelectedItem.ToString() == "Under")
                    WhereQury = WhereQury + " AND ISNULL((SELECT SUM(Totalvalue) FROM StationCloseCash SC WHERE SC.StationCloseId=SH.StationCloseId),0)- " +
                        " (ISNULL(SD.TransAmount,0) + ISNULL(SH.DefCDStartBalance,0) - ISNULL((SELECT SUM(P.Amount) FROM Payout P WHERE P.StationCloseID = SD.StationCloseID AND P.DrawNo = SD.DrawNo AND P.StationID = SH.StationID),0)) < 0";
                else if (cboCashDiff.SelectedItem.ToString() == "Over") 
                    WhereQury = WhereQury + " AND ISNULL((SELECT SUM(Totalvalue) FROM StationCloseCash SC WHERE SC.StationCloseId=SH.StationCloseId),0)- " +
                        " (ISNULL(SD.TransAmount,0) + ISNULL(SH.DefCDStartBalance,0) - ISNULL((SELECT SUM(P.Amount) FROM Payout P WHERE P.StationCloseID = SD.StationCloseID AND P.DrawNo = SD.DrawNo AND P.StationID = SH.StationID),0)) > 0";
                else if (cboCashDiff.SelectedItem.ToString() == "Under and Over Only")
                    WhereQury = WhereQury + " AND ISNULL((SELECT SUM(Totalvalue) FROM StationCloseCash SC WHERE SC.StationCloseId=SH.StationCloseId),0)- " +
                        " (ISNULL(SD.TransAmount,0) + ISNULL(SH.DefCDStartBalance,0) - ISNULL((SELECT SUM(P.Amount) FROM Payout P WHERE P.StationCloseID = SD.StationCloseID AND P.DrawNo = SD.DrawNo AND P.StationID = SH.StationID),0)) != 0";
                else if (cboCashDiff.SelectedItem.ToString() == "Match")
                    WhereQury = WhereQury + " AND ISNULL((SELECT SUM(Totalvalue) FROM StationCloseCash SC WHERE SC.StationCloseId=SH.StationCloseId),0)- " +
                        " (ISNULL(SD.TransAmount,0) + ISNULL(SH.DefCDStartBalance,0) - ISNULL((SELECT SUM(P.Amount) FROM Payout P WHERE P.StationCloseID = SD.StationCloseID AND P.DrawNo = SD.DrawNo AND P.StationID = SH.StationID),0)) = 0";
                #endregion

                WhereQury = WhereQury + " GROUP BY SH.CloseDate, SH.StationCloseId, SH.StationID, SH.UserID, SD.TransAmount, SD.StationCloseID, SD.DrawNo, SH.DefCDStartBalance ";
                if (optByName.Text == "Date")
                    WhereQury += " ORDER BY SH.CloseDate";
                else
                WhereQury += " ORDER BY SH." + optByName.Text.ToString() + "";

                sqlQuery = sqlQuery + WhereQury;

                return sqlQuery;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return "";
            }
        }
        //------------------------------------------------------------------------------------------------------------------
        private void btnPrint_Click(object sender, EventArgs e)
        {
            Preview(true);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmRptStnCloseCash_KeyDown(object sender, KeyEventArgs e)
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
    }
}