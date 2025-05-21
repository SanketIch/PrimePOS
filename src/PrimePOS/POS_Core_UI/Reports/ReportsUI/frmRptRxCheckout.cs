using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
//using POS_Core.DataAccess;
//using POS.UI;
using POS_Core.DataAccess;
using POS_Core.CommonData;
using System.Data.SqlClient;
using POS_Core.Resources;

namespace POS_Core_UI.Reports.ReportsUI
{
    public partial class frmRptRxCheckout : Form
    {
        //------------------------------------------------------------------------------------------------------------------
        public frmRptRxCheckout()
        {
            InitializeComponent();
        }
        //------------------------------------------------------------------------------------------------------------------
        private void frmRptRxCheckout_Load(object sender, EventArgs e)
        {
            this.cboUsers.AfterEnterEditMode += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
            this.cboUsers.AfterExitEditMode += new System.EventHandler(clsUIHelper.AfterExitEditMode);
            this.cboStnId.AfterEnterEditMode += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
            this.cboStnId.AfterExitEditMode += new System.EventHandler(clsUIHelper.AfterExitEditMode);
            DateTime curDate =new DateTime();
            curDate = DateTime.Now.Date;
            dtpToDate.Text = curDate.ToString();
            dtpFromDate.Text = curDate.ToString();           
            clsUIHelper.setColorSchecme(this);
            PopulateUsers();
            FillStationIDs();

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
        //------------------------------------------------------------------------------------------------------------------
        private void btnView_Click_1(object sender, EventArgs e)
        {
            string strError = "";
            if (!validateFields(out strError))
                clsUIHelper.ShowErrorMsg(strError);
            else
                Preview(false);
        }
        //------------------------------------------------------------------------------------------------------------------
        private void dtpFromDate_ValueChanged(object sender, EventArgs e)
        {
            string fieldName = string.Empty;
            try
            {
                if (!validateFields(out fieldName))
                {
                    if (fieldName == "DATE")
                    {
                        dtpFromDate.Text = DateTime.Now.ToShortDateString();  
                        clsUIHelper.ShowErrorMsg("From Date can not be greater than To Date.");
                    }

                    return;
                }
            }
            catch (Exception ex)
            {
            }
        }
        //------------------------------------------------------------------------------------------------------------------
        private void dtpToDate_ValueChanged(object sender, EventArgs e)
        {
            string fieldName = string.Empty;
            try
            {
                if (!validateFields(out fieldName))
                {
                    if (fieldName == "DATE")
                    {
                        dtpToDate.Text = DateTime.Now.ToShortDateString();
                        clsUIHelper.ShowErrorMsg("From Date can not be greater than To Date.");
                    }

                    return;
                }
            }
            catch (Exception ex)
            {
            }
        }
        //------------------------------------------------------------------------------------------------------------------
        private bool validateFields(out string fieldName)
        {
            bool isValid = true;
            string field = string.Empty;
            try
            {
                if ((DateTime)dtpFromDate.Value > (DateTime)dtpToDate.Value)
                {
                    isValid = false;
                    fieldName = "DATE";
                    return isValid;
                }
            }
            catch (Exception ex)
            {
                clsUIHelper.ShowErrorMsg(ex.Message);
            }
            fieldName = field;
            return isValid;
        }
        //------------------------------------------------------------------------------------------------------------------
        private void Preview(bool PrintIt)
        {
            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            string strQuery = "";
            string WhereClause = "";
            try
            {
                DateTime FromDate = new DateTime();
                FromDate = DateTime.Parse(dtpFromDate.Text);
                DateTime ToDate = new DateTime();
                ToDate = DateTime.Parse(dtpToDate.Text);
                ToDate = ToDate.AddHours(23.9999);
                //PRIMEPOS-2743 04-Nov-2019 JY Added OrignalPrice
                //strQuery = "select PT.TransID, PTD.TransDetailID, PTDRX.RXNo, PT.TransDate, PTD.ItemID, PTD.ExtendedPrice, pty.PayTypeDesc, PTD.OrignalPrice "
                //            + " from POSTransaction PT "
                //            + " inner join POSTransactionDetail PTD on PTD.TransID=PT.TransID  "
                //            + " inner join	POSTransPayment PTP on PT.TransID=PTP.TransID"
                //            + " inner join PayType PTy on PTP.TransTypeCode=PTy.PayTypeID"
                //            + " inner join POSTransactionRXDetail PTDRX on PTD.TransDetailID =PTDRX.TransDetailID  "
                //            + " where PT.TransDate between '" + FromDate.ToString() + "' and '" + ToDate.ToString() + "' and PTD.ItemID='RX'";

                //PRIMEPOS-2881 05-Aug-2020 JY Added for above commented query
                strQuery = "select DISTINCT PT.TransID, PTD.TransDetailID, PTDRX.RXNo, PT.TransDate, PTD.ItemID, PTD.ExtendedPrice, PTD.OrignalPrice, "
                                + " STUFF((SELECT '/' + CAST(c.PayTypeDesc AS varchar(100)) FROM POSTransactionDetail a"
                                + " INNER JOIN POSTransPayment b ON a.TransID = b.TransID INNER JOIN PayType c ON b.TransTypeCode = c.PayTypeID"
                                + " WHERE a.TransDetailID = PTD.TransDetailID FOR XML PATH('')),1,1,'') AS PayTypeDesc"
                            + " from POSTransaction PT "
                            + " inner join POSTransactionDetail PTD on PTD.TransID=PT.TransID  "
                            + " inner join POSTransPayment PTP on PT.TransID=PTP.TransID"
                            + " inner join PayType PTy on PTP.TransTypeCode=PTy.PayTypeID"
                            + " inner join POSTransactionRXDetail PTDRX on PTD.TransDetailID =PTDRX.TransDetailID  "
                            + " where PT.TransDate between '" + FromDate.ToString() + "' and '" + ToDate.ToString() + "' and PTD.ItemID='RX'";

                if (cboUsers.Text != "ALL")
                {
                    WhereClause = " AND PT.UserId='" + cboUsers.Text + "'";
                }
                if (cboStnId.Text != "ALL")
                {
                    WhereClause += " AND PT.StationId=" + cboStnId.Text + "";
                }
                if (WhereClause != "")
                    strQuery += WhereClause;

                strQuery += " ORDER BY PT.TransID, PTD.TransDetailID";   //PRIMEPOS-2881 05-Aug-2020 JY Added

                Reports.rptRXCheckout oRpt = new POS_Core_UI.Reports.Reports.rptRXCheckout();
                dsRXCheckout DsRxCheckOut = new dsRXCheckout();
                SqlConnection conn = new SqlConnection(POS_Core.Resources.Configuration.ConnectionString);
                SqlDataAdapter da = new SqlDataAdapter(strQuery, conn);
                da.Fill(DsRxCheckOut, "RxCheckOut");

                #region PRIMEPOS-2881 05-Aug-2020 JY Commented
                //dsRXCheckout DsRxCheckOutTemp = new dsRXCheckout();
                //string paytype = "";
                //int TransId;
                //bool found = false;
                //int TransDetailId;
                ////DsRxCheckOutTemp = DsRxCheckOut;
                //// int i = 0;
                //foreach (DataRow row in DsRxCheckOut.Tables["RxCheckOut"].Rows)
                //{
                //    paytype = "";//row["PayTypeDesc"].ToString();
                //    TransId = Convert.ToInt32(row["Transid"]);
                //    TransDetailId = Convert.ToInt32(row["TransDetailId"]);
                //    DataRow[] temprows = DsRxCheckOutTemp.Tables["RxCheckOut"].Select("TransId=" + TransId.ToString() + " AND TransDetailId=" + TransDetailId.ToString());
                //    if (temprows == null || temprows.Length == 0)
                //    {
                //        DsRxCheckOutTemp.Tables["RxCheckOut"].Rows.Add(row.ItemArray);
                //    }
                //    else
                //    {
                //        paytype = temprows[0].ItemArray[6].ToString();
                //        DsRxCheckOutTemp.Tables["RxCheckOut"].Rows.Remove(temprows[0]);
                //        row["PayTypeDesc"] = paytype + "/" + row["PayTypeDesc"].ToString();

                //        DsRxCheckOutTemp.Tables["RxCheckOut"].Rows.Add(row.ItemArray);
                //    }
                //}

                //double CashSum = 0, CheckSum = 0, AmexSum = 0, VisaSum = 0, MCSum = 0;
                //double DiscSum = 0, DebitSum = 0, CashBackSum = 0, CouponSum = 0, EBTSum = 0, HCSum = 0;

                //int countcash = 0, countcheck = 0, countamex = 0, countvisa = 0, countmc = 0, countdisc = 0, countdebit = 0, countcashback = 0,
                //    countcoupon = 0, countebt = 0, counthc = 0;

                //foreach (DataRow row in DsRxCheckOutTemp.Tables["RxCheckOut"].Rows)
                //{
                //    string sumCase = row["PayTypeDesc"].ToString() + "/";
                //    sumCase = sumCase.Substring(0, sumCase.IndexOf("/"));
                //    switch (sumCase)
                //    {
                //        case clsPOSDBConstants.PayType_Fld_Cash:
                //            CashSum = CashSum + Convert.ToDouble(row["ExtendedPrice"]);
                //            countcash++;
                //            break;
                //        case clsPOSDBConstants.PayType_Fld_Check:
                //            CheckSum = CheckSum + Convert.ToDouble(row["ExtendedPrice"]);
                //            countcheck++;
                //            break;
                //        case clsPOSDBConstants.PayType_Fld_Amex:
                //            AmexSum = AmexSum + Convert.ToDouble(row["ExtendedPrice"]);
                //            countamex++;
                //            break;
                //        case clsPOSDBConstants.PayType_Fld_Visa:
                //            VisaSum = VisaSum + Convert.ToDouble(row["ExtendedPrice"]);
                //            countvisa++;
                //            break;
                //        case clsPOSDBConstants.PayType_Fld_MC:
                //            MCSum = MCSum + Convert.ToDouble(row["ExtendedPrice"]);
                //            countmc++;
                //            break;
                //        case clsPOSDBConstants.PayType_Fld_Disc:
                //            DiscSum = DiscSum + Convert.ToDouble(row["ExtendedPrice"]);
                //            countdisc++;
                //            break;
                //        case clsPOSDBConstants.PayType_Fld_Debit:
                //            DebitSum = DebitSum + Convert.ToDouble(row["ExtendedPrice"]);
                //            countdebit++;
                //            break;
                //        case clsPOSDBConstants.PayType_Fld_CashBack:
                //            CashBackSum = CashBackSum + Convert.ToDouble(row["ExtendedPrice"]);
                //            countcashback++;
                //            break;
                //        case clsPOSDBConstants.PayType_Fld_Coupon:
                //            CouponSum = CouponSum + Convert.ToDouble(row["ExtendedPrice"]);
                //            countcoupon++;
                //            break;
                //        case clsPOSDBConstants.PayType_Fld_EBT:
                //            EBTSum = EBTSum + Convert.ToDouble(row["ExtendedPrice"]);
                //            countebt++;
                //            break;
                //        case clsPOSDBConstants.PayType_Fld_HC:
                //            HCSum = HCSum + Convert.ToDouble(row["ExtendedPrice"]);
                //            counthc++;
                //            break;
                //        default:
                //            break;
                //    }
                //}

                //clsReports.setCRTextObjectText("CashSum",Configuration.CInfo.CurrencySymbol + CashSum.ToString(), oRpt);
                //clsReports.setCRTextObjectText("CheckSum",Configuration.CInfo.CurrencySymbol + CheckSum.ToString(), oRpt);
                //clsReports.setCRTextObjectText("AmexSum",Configuration.CInfo.CurrencySymbol + AmexSum.ToString(), oRpt);
                //clsReports.setCRTextObjectText("VisaSum",Configuration.CInfo.CurrencySymbol + VisaSum.ToString(), oRpt);
                //clsReports.setCRTextObjectText("MCSum",Configuration.CInfo.CurrencySymbol + MCSum.ToString(), oRpt);
                //clsReports.setCRTextObjectText("DiscSum",Configuration.CInfo.CurrencySymbol + DiscSum.ToString(), oRpt);
                //clsReports.setCRTextObjectText("DebitSum",Configuration.CInfo.CurrencySymbol + DebitSum.ToString(), oRpt);
                //clsReports.setCRTextObjectText("CashBackSum",Configuration.CInfo.CurrencySymbol + CashBackSum.ToString(), oRpt);
                //clsReports.setCRTextObjectText("CouponSum",Configuration.CInfo.CurrencySymbol + CouponSum.ToString(), oRpt);
                //clsReports.setCRTextObjectText("EBTSum",Configuration.CInfo.CurrencySymbol + EBTSum.ToString(), oRpt);
                //clsReports.setCRTextObjectText("HCSum",Configuration.CInfo.CurrencySymbol + HCSum.ToString(), oRpt);

                //clsReports.setCRTextObjectText("countcash", countcash.ToString(), oRpt);
                //clsReports.setCRTextObjectText("countcheck", countcheck.ToString(), oRpt);
                //clsReports.setCRTextObjectText("countamex", countamex.ToString(), oRpt);
                //clsReports.setCRTextObjectText("countvisa", countvisa.ToString(), oRpt);
                //clsReports.setCRTextObjectText("countmc", countmc.ToString(), oRpt);
                //clsReports.setCRTextObjectText("countdisc", countdisc.ToString(), oRpt);
                //clsReports.setCRTextObjectText("countdebit", countdebit.ToString(), oRpt);
                //clsReports.setCRTextObjectText("countcashback", countcashback.ToString(), oRpt);
                //clsReports.setCRTextObjectText("countcoupon", countcoupon.ToString(), oRpt);
                //clsReports.setCRTextObjectText("countebt", countebt.ToString(), oRpt);
                //clsReports.setCRTextObjectText("counthc", counthc.ToString(), oRpt);
                // Add by Ravindra to add From and To Date on Rx CheckOut Report
                #endregion

                if (optList.Value.ToString() == "1")
                {
                    oRpt.Section2.SectionFormat.EnableSuppress = true;
                    oRpt.Section3.SectionFormat.EnableSuppress = true;
                    oRpt.ReportFooterSection1.SectionFormat.EnableSuppress = true;  //PRIMEPOS-2776 09-Jan-2020 JY Added
                }
                else if (optList.Value.ToString() == "2")
                {
                    oRpt.Section2.SectionFormat.EnableSuppress = false;
                    oRpt.Section3.SectionFormat.EnableSuppress = false;
                    oRpt.ReportFooterSection1.SectionFormat.EnableSuppress = false; //PRIMEPOS-2776 09-Jan-2020 JY Added
                }
                clsReports.setCRTextObjectText("txtFromDate", "From :" + this.dtpFromDate.Text, oRpt);
                clsReports.setCRTextObjectText("txtToDate", "To :" + dtpToDate.Text, oRpt);
                
                clsReports.Preview(PrintIt, DsRxCheckOut, oRpt);    //PRIMEPOS-2776 09-Jan-2020 JY modified
            }
            catch (Exception ex)
            {
            }
            finally
            {
                //frmMain.getInstance().Cursor = Cursors.Default;   //PRIMEPOS-2743 04-Nov-2019 JY Commented
                this.Cursor = System.Windows.Forms.Cursors.Default; //PRIMEPOS-2743 04-Nov-2019 JY Added
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            Preview(true);
        }

    }
}