//-----------------------------------------------------------------------------------------------------------
//Added By Shitaljit(QuicSolv) on 12 June 2011
//For Station Close Cash Denominations Entries.
//-----------------------------------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using POS_Core.CommonData;
using POS_Core.BusinessRules;
//using POS_Core.DataAccess;
using POS_Core_UI.Reports.Reports;
//using POS_Core_UI.Reports.ReportsUI;
using System.Data.SqlClient;
using POS_Core.Resources;
using POS_Core.LabelHandler;
using POS_Core_UI.Reports.ReportsUI;
using POS_Core.LabelHandler.RxLabel;

namespace POS_Core_UI
{
    public partial class frmStationCloseCash : Form
    {
        #region Declaration       
        public static decimal GrandTotal = 0; 
        private CloseStationData oCloseStationData;
        private StationClose oStationClose = new StationClose();
        DataSet dsCurrencyList;
        DataSet dsStationCloseData;

        #region PRIMEPOS-2554 02-Jul-2018 JY Commented        
        //decimal billTotal;
        //decimal coinTotal;
        //decimal pennyTotal = 0;
        //decimal nickelTotal = 0;
        //decimal dimeTotal = 0;
        //decimal quaterTotal = 0;
        //decimal halfDollarTotal = 0;
        //decimal oneDollarCoinTotal = 0;
        //decimal twoDollarCoinTotal = 0;
        //decimal oneDollarBillTotal = 0;
        //decimal twoDollarBillTotal = 0;
        //decimal fiveDollarTotal = 0;
        //decimal tenDollarTotal = 0;
        //decimal twentyDollarTotal = 0;
        //decimal fiftyDollarTotal = 0;
        //decimal hundredDollarTotal = 0;
        //decimal thousandDollarTotal = 0;

        //int pennyCount;
        //int dimeCount;
        //int nickelCount;
        //int quaterCount;
        //int halfDollarCount;
        //int oneDollarCointCount;
        //int twoDollarCoinCount;

        //int oneDollarBillCount;
        //int twoDollarBillCount;
        //int fiveDollarCount;
        //int tenDollarCount;
        //int twentyDollarCount;
        //int fiftyDollarCount;
        //int hundredDollarCount;
        //int thousandDollarCount;
        #endregion

        string strDrawerNo = "";
        decimal UserEnterAmount = 0;
        decimal SystemCalculatedAmount = 0;
        bool VerifyFlag = false;
        bool VerifyTotalOnly = false;
        int StationCloseID;
        public static decimal CCAmount = 0;
        public static decimal CheckAmount = 0;
        public static decimal CouponTotal = 0;
        public static decimal EBTAmount = 0;
        public static decimal ROAAmount = 0;
        public static decimal PayoutAmount = 0;
        public static decimal CashbackAmount = 0;
        public static decimal HouseChargeTotal = 0;
        public static bool FlagCloseStationForm = false;
        string stationName = "";
        string strCompanyLogoPath = string.Empty;   //PRIMEPOS-2386 26-Feb-2021 JY Added
        public static decimal TransFee = 0; //PRIMEPOS-3118 03-Aug-2022 JY Added
        #endregion

        public frmStationCloseCash(string drawerNo)
        {
            this.strDrawerNo = drawerNo;
            InitializeComponent();
        }

        public frmStationCloseCash(int StCloseID, decimal UserEnterAmnt, decimal SytemCalculatedAmnt)
        {
            UserEnterAmount = UserEnterAmnt;
            SystemCalculatedAmount = SytemCalculatedAmnt;
            VerifyFlag = true;
            StationCloseID = StCloseID;
            InitializeComponent();
        }

        public frmStationCloseCash()
        {
            InitializeComponent();
        }

        private void frmStationCloseCash_Load(object sender, EventArgs e)
        {
            clsUIHelper.setColorSchecme(this);
            Display();
            #region Enter and Leave Mode of Controls
            this.numPenny.AfterEnterEditMode += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
            this.numPenny.AfterExitEditMode += new System.EventHandler(clsUIHelper.AfterExitEditMode);

            this.numNickel.AfterEnterEditMode += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
            this.numNickel.AfterExitEditMode += new System.EventHandler(clsUIHelper.AfterExitEditMode);

            this.numQuater.AfterEnterEditMode += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
            this.numQuater.AfterExitEditMode += new System.EventHandler(clsUIHelper.AfterExitEditMode);

            this.numDime.AfterEnterEditMode += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
            this.numDime.AfterExitEditMode += new System.EventHandler(clsUIHelper.AfterExitEditMode);

            this.numHalfDollar.AfterEnterEditMode += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
            this.numHalfDollar.AfterExitEditMode += new System.EventHandler(clsUIHelper.AfterExitEditMode);

            this.numOneDollar.AfterEnterEditMode += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
            this.numOneDollar.AfterExitEditMode += new System.EventHandler(clsUIHelper.AfterExitEditMode);

            this.numTwoDollar.AfterEnterEditMode += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
            this.numTwoDollar.AfterExitEditMode += new System.EventHandler(clsUIHelper.AfterExitEditMode);

            this.numOneDollarBill.AfterEnterEditMode += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
            this.numOneDollarBill.AfterExitEditMode += new System.EventHandler(clsUIHelper.AfterExitEditMode);

            this.numTwoDollarBill.AfterEnterEditMode += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
            this.numTwoDollarBill.AfterExitEditMode += new System.EventHandler(clsUIHelper.AfterExitEditMode);

            this.numFiveDollar.AfterEnterEditMode += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
            this.numFiveDollar.AfterExitEditMode += new System.EventHandler(clsUIHelper.AfterExitEditMode);

            this.numTenDollar.AfterEnterEditMode += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
            this.numTenDollar.AfterExitEditMode += new System.EventHandler(clsUIHelper.AfterExitEditMode);

            this.numTwentyDollar.AfterEnterEditMode += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
            this.numTwentyDollar.AfterExitEditMode += new System.EventHandler(clsUIHelper.AfterExitEditMode);

            this.numFiftyDollar.AfterEnterEditMode += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
            this.numFiftyDollar.AfterExitEditMode += new System.EventHandler(clsUIHelper.AfterExitEditMode);

            this.numHundredDollar.AfterEnterEditMode += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
            this.numHundredDollar.AfterExitEditMode += new System.EventHandler(clsUIHelper.AfterExitEditMode);

            this.numOneThousandDollar.AfterEnterEditMode += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
            this.numOneThousandDollar.AfterExitEditMode += new System.EventHandler(clsUIHelper.AfterExitEditMode);
            #endregion

            strCompanyLogoPath = Configuration.GetCompanyLogoPath(this);   //PRIMEPOS-2386 26-Feb-2021 JY Added

            numPenny.Focus();
        }

        private void Display()
        {
            dsCurrencyList = new DataSet();
            dsCurrencyList = oStationClose.PopulateCurrencyList();
            if (!VerifyFlag)
            {
                for (int index = 0; index < dsCurrencyList.Tables[0].Rows.Count; index++)
                {
                    if (Configuration.convertNullToBoolean(dsCurrencyList.Tables[0].Rows[index]["IncludeInStClose"].ToString()))
                    {
                        if (dsCurrencyList.Tables[0].Rows[index]["CurrencyDescription"].ToString() == "Penny")
                        {
                            numPenny.Enabled = true;
                        }
                        if (dsCurrencyList.Tables[0].Rows[index]["CurrencyDescription"].ToString() == "Nickel")
                        {
                            numNickel.Enabled = true;
                        }
                        if (dsCurrencyList.Tables[0].Rows[index]["CurrencyDescription"].ToString() == "Dime")
                        {
                            numDime.Enabled = true;
                        }
                        if (dsCurrencyList.Tables[0].Rows[index]["CurrencyDescription"].ToString() == "Quarter")
                        {
                            numQuater.Enabled = true;
                        }
                        if (dsCurrencyList.Tables[0].Rows[index]["CurrencyDescription"].ToString() == "Half Dollar")
                        {
                            numHalfDollar.Enabled = true;
                        }
                        if (dsCurrencyList.Tables[0].Rows[index]["CurrencyDescription"].ToString() == "One Dollar" && Configuration.convertNullToBoolean(dsCurrencyList.Tables[0].Rows[index]["IsCoin"].ToString()))
                        {
                            numOneDollar.Enabled = true;
                        }
                        if (dsCurrencyList.Tables[0].Rows[index]["CurrencyDescription"].ToString() == "Two Dollar" && Configuration.convertNullToBoolean(dsCurrencyList.Tables[0].Rows[index]["IsCoin"].ToString()))
                        {
                            numTwoDollar.Enabled = true;
                        }
                        if (dsCurrencyList.Tables[0].Rows[index]["CurrencyDescription"].ToString() == "One Dollar" && !Configuration.convertNullToBoolean(dsCurrencyList.Tables[0].Rows[index]["IsCoin"].ToString()))
                        {
                            numOneDollarBill.Enabled = true;
                        }
                        if (dsCurrencyList.Tables[0].Rows[index]["CurrencyDescription"].ToString() == "Two Dollar" && !Configuration.convertNullToBoolean(dsCurrencyList.Tables[0].Rows[index]["IsCoin"].ToString()))
                        {
                            numTwoDollarBill.Enabled = true;
                        }
                        if (dsCurrencyList.Tables[0].Rows[index]["CurrencyDescription"].ToString() == "Five Dollar")
                        {
                            numFiveDollar.Enabled = true;
                        }
                        if (dsCurrencyList.Tables[0].Rows[index]["CurrencyDescription"].ToString() == "Ten Dollar")
                        {
                            numTenDollar.Enabled = true;
                        }
                        if (dsCurrencyList.Tables[0].Rows[index]["CurrencyDescription"].ToString() == "Twenty Dollar")
                        {
                            numTwentyDollar.Enabled = true;
                        }
                        if (dsCurrencyList.Tables[0].Rows[index]["CurrencyDescription"].ToString() == "Fifty Dollar")
                        {
                            numFiftyDollar.Enabled = true;
                        }
                        if (dsCurrencyList.Tables[0].Rows[index]["CurrencyDescription"].ToString() == "One Hundered Dollar")
                        {
                            numHundredDollar.Enabled = true;
                        }
                        if (dsCurrencyList.Tables[0].Rows[index]["CurrencyDescription"].ToString() == "One Thousand Dollar")
                        {
                            numOneThousandDollar.Enabled = true;
                        }
                    }
                }
            }
            else
            {
                grpVerifyCash.Visible = true;
                btnContinue.Text = "&Verify";
                lblTransactionType.Text = "Close Station Cash Verification";
                numSystemCalCash.Value = SystemCalculatedAmount;
                numUserEnterCash.Value = UserEnterAmount;
                if (SystemCalculatedAmount > UserEnterAmount)
                    numVerifyAmt.Value = SystemCalculatedAmount - UserEnterAmount;
                else
                    numVerifyAmt.Value = UserEnterAmount - SystemCalculatedAmount;
                dsStationCloseData = new DataSet();
                dsStationCloseData = oStationClose.GetStationCloseCashDetail(StationCloseID);
                if (dsStationCloseData.Tables[0].Rows.Count > 0)
                {
                    for (int index = 0; index < dsStationCloseData.Tables[0].Rows.Count; index++)
                    {
                        if (Convert.ToBoolean(dsStationCloseData.Tables[0].Rows[index]["CurrencyDenomID"]))
                        {
                            int id = Convert.ToInt32(dsStationCloseData.Tables[0].Rows[index]["CurrencyDenomID"]);
                            id = id - 1;
                            if (dsCurrencyList.Tables[0].Rows[id]["CurrencyDescription"].ToString() == "Penny")
                            {
                                numPenny.Enabled = true;
                            }
                            if (dsCurrencyList.Tables[0].Rows[id]["CurrencyDescription"].ToString() == "Nickel")
                            {
                                numNickel.Enabled = true;
                            }
                            if (dsCurrencyList.Tables[0].Rows[id]["CurrencyDescription"].ToString() == "Dime")
                            {
                                numDime.Enabled = true;
                            }
                            if (dsCurrencyList.Tables[0].Rows[id]["CurrencyDescription"].ToString() == "Quarter")
                            {
                                numQuater.Enabled = true;
                            }
                            if (dsCurrencyList.Tables[0].Rows[id]["CurrencyDescription"].ToString() == "Half Dollar")
                            {
                                numHalfDollar.Enabled = true;
                            }
                            if (dsCurrencyList.Tables[0].Rows[id]["CurrencyDescription"].ToString() == "One Dollar" && Configuration.convertNullToBoolean(dsCurrencyList.Tables[0].Rows[id]["IsCoin"].ToString()))
                            {
                                numOneDollar.Enabled = true;
                            }
                            if (dsCurrencyList.Tables[0].Rows[id]["CurrencyDescription"].ToString() == "Two Dollar" && Configuration.convertNullToBoolean(dsCurrencyList.Tables[0].Rows[id]["IsCoin"].ToString()))
                            {
                                numTwoDollar.Enabled = true;
                            }
                            if (dsCurrencyList.Tables[0].Rows[id]["CurrencyDescription"].ToString() == "One Dollar" && !Configuration.convertNullToBoolean(dsCurrencyList.Tables[0].Rows[id]["IsCoin"].ToString()))
                            {
                                numOneDollarBill.Enabled = true;
                            }
                            if (dsCurrencyList.Tables[0].Rows[id]["CurrencyDescription"].ToString() == "Two Dollar" && !Configuration.convertNullToBoolean(dsCurrencyList.Tables[0].Rows[id]["IsCoin"].ToString()))
                            {
                                numTwoDollarBill.Enabled = true;
                            }
                            if (dsCurrencyList.Tables[0].Rows[id]["CurrencyDescription"].ToString() == "Five Dollar")
                            {
                                numFiveDollar.Enabled = true;
                            }
                            if (dsCurrencyList.Tables[0].Rows[id]["CurrencyDescription"].ToString() == "Ten Dollar")
                            {
                                numTenDollar.Enabled = true;
                            }
                            if (dsCurrencyList.Tables[0].Rows[id]["CurrencyDescription"].ToString() == "Twenty Dollar")
                            {
                                numTwentyDollar.Enabled = true;
                            }
                            if (dsCurrencyList.Tables[0].Rows[id]["CurrencyDescription"].ToString() == "Fifty Dollar")
                            {
                                numFiftyDollar.Enabled = true;
                            }
                            if (dsCurrencyList.Tables[0].Rows[id]["CurrencyDescription"].ToString() == "One Hundered Dollar")
                            {
                                numHundredDollar.Enabled = true;
                            }
                            if (dsCurrencyList.Tables[0].Rows[id]["CurrencyDescription"].ToString() == "One Thousand Dollar")
                            {
                                numOneThousandDollar.Enabled = true;
                            }
                        }
                    }
                }
            }
        }

        private void CloseStation()
        {
            try
            {
                CCAmount = 0;
                EBTAmount = 0;
                CheckAmount = 0;
                CouponTotal = 0;
                HouseChargeTotal = 0;
                oCloseStationData = oStationClose.Close(strDrawerNo);
                StationCloseID = Configuration.convertNullToInt(oCloseStationData.StationCloseNo.ToString());
                stationName = Configuration.GetStationName(oCloseStationData.StationID);
                for (int i = 0; i < oCloseStationData.Details.Count; i++)
                {
                    Console.WriteLine(oCloseStationData.Details[i].PayTypeName);
                    if (oCloseStationData.Details[i].PayTypeName == "Cash Back")
                    {
                        CashbackAmount = oCloseStationData.Details[i].Amount;
                    }
                    if (oCloseStationData.Details[i].PayTypeName == "American Express" || oCloseStationData.Details[i].PayTypeName == "Visa" ||
                        oCloseStationData.Details[i].PayTypeName == "Master Card" || oCloseStationData.Details[i].PayTypeName == "Discover" ||
                        oCloseStationData.Details[i].PayTypeName == "Debit Card")
                    {
                        CCAmount = oCloseStationData.Details[i].Amount;
                    }

                    if (oCloseStationData.Details[i].PayTypeName == "Check")
                    {
                        CheckAmount = oCloseStationData.Details[i].Amount;
                    }

                    if (oCloseStationData.Details[i].PayTypeName == "Coupon")
                    {
                        CouponTotal = oCloseStationData.Details[i].Amount;
                    }
                    if (oCloseStationData.Details[i].PayTypeName == "Elect. Benefit Transfer")
                    {
                        EBTAmount = oCloseStationData.Details[i].Amount;
                    }
                    if (oCloseStationData.Details[i].PayTypeName == "House Charge")
                    {
                        HouseChargeTotal = oCloseStationData.Details[i].Amount;
                    }
                }
                ROAAmount = oCloseStationData.ReceiveOnAccount;
                PayoutAmount = oCloseStationData.Payout;
                TransFee = oCloseStationData.TransFee;    //PRIMEPOS-3118 03-Aug-2022 JY Added
                RxLabel oLabel = new RxLabel(null, null, null, ReceiptType.Void, null);
                
                //if (strDrawerNo == "1")//Commented By Ravindra to Open drower Befor form open 
                //{
                //    oLabel.OpenDrawer(1);
                //}
                //else if (strDrawerNo == "2")
                //{
                //    oLabel.OpenDrawer(2);
                //}
                //else
                //{
                //    oLabel.OpenDrawer(1);
                //    oLabel.OpenDrawer(2);
                //}
            }
            catch (Exception Ex)
            {
                throw (Ex);
            }
        }

        private void Print(bool isPrint)
        {
            try
            {
                isPrint = false;
                this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
                Search oSearch = new Search();

                if (Configuration.CPOSSet.UseRcptForCloseStation == true)
                {
                    POS_Core_UI.Reports.ReportsUI.RcptStationClose oSTrpt = new POS_Core_UI.Reports.ReportsUI.RcptStationClose(StationCloseID);
                    oSTrpt.Print();
                    #region PRIMEPOS-3086 20-Apr-2022 JY Added
                    if (Configuration.CInfo.AutoEmailStationCloseReport == true)
                    {
                        if (isPrint == false)
                        {
                            frmStationClose oStationClose = new frmStationClose();
                            oStationClose.EmailReport(StationCloseID);
                        }
                    }
                    #endregion
                }
                else
                {
                    rptCloseStationSummary oRptCloseStationSummary = new rptCloseStationSummary();
                    //string sql = " select(select sum(Totalvalue) from stationclosecash where stationcloseid=" + StationCloseID + ") as TotalCash, case transtype when 'U-1' then 1 when 'U-2' then 1 when 'U-3' then 1 when 'U-4' then 1 " +
                    //    " when 'U-5' then 1 when 'U-6' then 1 when 'U-7' then 1 when 'U-C' then 1 when 'U-H' then 1 when 'U-B' then 1 when 'U-E' then 1 when 'PO' then -1 else 2 end as GroupType, " +
                    //    " sch.*,scd.*,ps.* from stationcloseheader sch,stationclosedetail scd, util_POSSet ps " +
                    //    " where sch.stationid=ps.stationid and sch.stationcloseid=scd.stationcloseid and sch.stationcloseid=" + StationCloseID;

                    //PRIMEPOS-XXXX 25-May-2017 JY Added below query to work in case of multiple drawer records against one station
                    //PRIMEPOS-2983 02-Jul-2021 JY Modified
                    string sql = "select (select sum(Totalvalue) from stationclosecash where stationcloseid=" + StationCloseID + ") as TotalCash," +
                        " case transtype when 'U-1' then 1 when 'U-2' then 1 when 'U-3' then 1 when 'U-4' then 1  when 'U-5' then 1 when 'U-6' then 1 when 'U-7' then 1 when 'U-C' then 1 when 'U-H' then 1 when 'U-B' then 1 when 'U-E' then 1 when 'PO' then - 1 else 2 end as GroupType," +
                        " ISNULL((select Pt.PayTypeDesc  from Paytype PT where SUBSTRING(scd.TransType, 3, len(scd.TransType)) = pt.PayTypeID),TransType) as PayTypeDesc," +
                        " scd.TransType, SUM(ISNULL(scd.TransCount, 0)) AS TransCount, SUM(ISNULL(scd.TransAmount, 0)) AS TransAmount, ps.STATIONNAME, sch.UserID, sch.CloseDate, sch.StationCloseID" +
                        " FROM stationcloseheader sch" +
                        " INNER JOIN stationclosedetail scd ON sch.stationcloseid = scd.stationcloseid" +
                        " INNER JOIN util_POSSet ps ON sch.stationid = ps.stationid" +
                        " WHERE sch.stationcloseid =" + StationCloseID +
                        " GROUP BY scd.TransType, ps.STATIONNAME, sch.UserID, sch.CloseDate, sch.StationCloseID";

                    DataSet ds = oSearch.SearchData(sql);
                    DataSet dsStnCloseCash = new DataSet();
                    dsCloseStationCurrDenom odsCloseStationCurrDenom = new dsCloseStationCurrDenom();
                    dsStnCloseCash = oStationClose.GetStationCloseCashDetail(StationCloseID);
                    if (dsStnCloseCash.Tables[0].Rows.Count > 0)
                    {
                        rptCloseStationCurrDenom oRptCloseStationCurrDenom = new rptCloseStationCurrDenom();
                        string sqlCurrDenom = @"SELECT cd.CurrencyDescription , cd.IsCoin, scc.Count,scc.TotalValue,
                                            sch.UserID, sch.CloseDate
                                            FROM stationclosecash scc , currencydenominations cd, StationCloseHeader sch
                                            WHERE cd.currencydenomid = scc.currencydenomid and  sch.stationcloseid = scc.stationcloseid
		                                    and scc.stationcloseid =" + StationCloseID;
                        DataSet dsCurrDenom = new DataSet();
                        dsCurrDenom = oSearch.SearchData(sqlCurrDenom);
                        for (int index = 0; index < dsCurrDenom.Tables[0].Rows.Count; index++)
                        {
                            odsCloseStationCurrDenom.Tables[0].ImportRow(dsCurrDenom.Tables[0].Rows[index]);
                        }
                        ((CrystalDecisions.CrystalReports.Engine.TextObject)oRptCloseStationCurrDenom.ReportDefinition.ReportObjects["txtHeader"]).Text = Configuration.CInfo.StoreName;
                        ((CrystalDecisions.CrystalReports.Engine.TextObject)oRptCloseStationCurrDenom.ReportDefinition.ReportObjects["txtPharmacyAddress"]).Text = Configuration.CInfo.Address;
                        ((CrystalDecisions.CrystalReports.Engine.TextObject)oRptCloseStationCurrDenom.ReportDefinition.ReportObjects["txtPharmacyCityStateZip"]).Text = Configuration.CInfo.City + ", " + Configuration.CInfo.State + " " + Configuration.CInfo.Zip;
                        ((CrystalDecisions.CrystalReports.Engine.TextObject)oRptCloseStationCurrDenom.ReportDefinition.ReportObjects["txtTelephoneNo"]).Text = Configuration.CInfo.Telephone;

                        ((CrystalDecisions.CrystalReports.Engine.TextObject)oRptCloseStationSummary.ReportDefinition.ReportObjects["txtHeader"]).Text = Configuration.CInfo.StoreName;
                        ((CrystalDecisions.CrystalReports.Engine.TextObject)oRptCloseStationSummary.ReportDefinition.ReportObjects["txtPharmacyAddress"]).Text = Configuration.CInfo.Address;
                        ((CrystalDecisions.CrystalReports.Engine.TextObject)oRptCloseStationSummary.ReportDefinition.ReportObjects["txtPharmacyCityStateZip"]).Text = Configuration.CInfo.City + ", " + Configuration.CInfo.State + " " + Configuration.CInfo.Zip;
                        ((CrystalDecisions.CrystalReports.Engine.TextObject)oRptCloseStationSummary.ReportDefinition.ReportObjects["txtTelephoneNo"]).Text = Configuration.CInfo.Telephone;

                        oRptCloseStationCurrDenom.Database.Tables[0].SetDataSource(odsCloseStationCurrDenom.Tables[0]);
                        //oRptCloseStationSummary.Database.Tables[0].SetDataSource(ds.Tables[0]);   //PRIMEPOS-2883 12-Aug-2020 JY commented
                        oRptCloseStationCurrDenom.SetParameterValue("CompanyLogoPath", strCompanyLogoPath);   //PRIMEPOS-2386 26-Feb-2021 JY Added

                        #region PRIMEPOS-2883 12-Aug-2020 JY Added
                        frmEndOfDay ofrmEOD = new frmEndOfDay();
                        DataTable dtStClose = ofrmEOD.setGroupType(ds.Tables[0]);
                        oRptCloseStationSummary.Database.Tables[0].SetDataSource(dtStClose);
                        oRptCloseStationSummary.SetParameterValue("CompanyLogoPath", strCompanyLogoPath);   //PRIMEPOS-2386 26-Feb-2021 JY Added
                        #endregion

                        if (Configuration.CInfo.PrintStCloseNo)
                        {
                            clsReports.setCRTextObjectText("txtCloseID", StationCloseID.ToString(), oRptCloseStationSummary);
                            clsReports.setCRTextObjectText("txtCloseID", StationCloseID.ToString(), oRptCloseStationCurrDenom);
                        }
                        clsReports.setCRTextObjectText("txtStationName", stationName, oRptCloseStationCurrDenom);

						//oRptCloseStationCurrDenom.SetParameterValue("Currency", Configuration.CInfo.CurrencySymbol.ToString());
						//oRptCloseStationSummary.SetParameterValue("Currency", Configuration.CInfo.CurrencySymbol.ToString());

                        if (isPrint == false)
                        {
                            POS_Core_UI.Reports.ReportsUI.clsReports.ShowReport(oRptCloseStationCurrDenom);
                            POS_Core_UI.Reports.ReportsUI.clsReports.ShowReport(oRptCloseStationSummary);
                            #region PRIMEPOS-3086 20-Apr-2022 JY Added
                            if (Configuration.CInfo.AutoEmailStationCloseReport == true)
                            {
                                frmStationClose oStationClose = new frmStationClose();
                                oStationClose.EmailReport(StationCloseID);
                            }
                            #endregion
                        }
                        else
                        {
                            POS_Core_UI.Reports.ReportsUI.clsReports.PrintReport(oRptCloseStationCurrDenom);
                            POS_Core_UI.Reports.ReportsUI.clsReports.PrintReport(oRptCloseStationSummary);
                        }
                    }
                }
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

        private void btnContinue_Click(object sender, EventArgs e)
        {
            try
            {
                //GrandTotal = billTotal + coinTotal;   //PRIMEPOS-2554 02-Jul-2018 JY Commented
                GrandTotal = Configuration.convertNullToDecimal(nunBillTotal.Value) + Configuration.convertNullToDecimal(numCoinTotal.Value);    //PRIMEPOS-2554 02-Jul-2018 JY Added
                if (!VerifyFlag)
                {
                    CloseStation();
                    if (Save())
                    {
                        Print(false);
                        frmStationCloseCashDetail ofrmStCloseDetail = new frmStationCloseCashDetail(StationCloseID);
                        ofrmStCloseDetail.StartPosition = FormStartPosition.CenterScreen;
                        ofrmStCloseDetail.ShowDialog(this);
                        this.Close();
                    }
                }
                else
                {
                    if (!VerifyTotalOnly)
                    {
                        VerifyStationCloseCash();
                    }
                    oStationClose.UpdateMaster(StationCloseID, GrandTotal);
                    this.Close();
                }
            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }

        private bool VerifyStationCloseCash()
        {
            int CurrencyID;
            bool retValue = false;
            try
            {
                for (int index = 0; index < dsStationCloseData.Tables[0].Rows.Count; index++)
                {
                    int id = Convert.ToInt32(dsStationCloseData.Tables[0].Rows[index]["CurrencyDenomID"]);
                    id = id - 1;
                    #region Coins
                    if (dsCurrencyList.Tables[0].Rows[id]["CurrencyDescription"].ToString() == "Penny")
                    {
                        CurrencyID = Configuration.convertNullToInt(dsStationCloseData.Tables[0].Rows[index]["CurrencyDenomID"].ToString());
                        oStationClose.VerifyStationCloseCash(StationCloseID, CurrencyID, Configuration.convertNullToInt(numPenny.Value), Configuration.convertNullToDecimal(lblPennyTotal.Text));
                    }
                    if (dsCurrencyList.Tables[0].Rows[id]["CurrencyDescription"].ToString() == "Nickel")
                    {
                        CurrencyID = Configuration.convertNullToInt(dsStationCloseData.Tables[0].Rows[index]["CurrencyDenomID"].ToString());
                        oStationClose.VerifyStationCloseCash(StationCloseID, CurrencyID, Configuration.convertNullToInt(numNickel.Value), Configuration.convertNullToDecimal(lblNickelTotal.Text));
                    }
                    if (dsCurrencyList.Tables[0].Rows[id]["CurrencyDescription"].ToString() == "Dime")
                    {
                        CurrencyID = Configuration.convertNullToInt(dsStationCloseData.Tables[0].Rows[index]["CurrencyDenomID"].ToString());
                        oStationClose.VerifyStationCloseCash(StationCloseID, CurrencyID, Configuration.convertNullToInt(numDime.Value), Configuration.convertNullToDecimal(lblDimeTotal.Text));
                    }
                    if (dsCurrencyList.Tables[0].Rows[id]["CurrencyDescription"].ToString() == "Quarter")
                    {
                        CurrencyID = Configuration.convertNullToInt(dsStationCloseData.Tables[0].Rows[id]["CurrencyDenomID"].ToString());
                        oStationClose.VerifyStationCloseCash(StationCloseID, CurrencyID, Configuration.convertNullToInt(numQuater.Value), Configuration.convertNullToDecimal(lblQuarterTotal.Text));
                    }
                    if (dsCurrencyList.Tables[0].Rows[id]["CurrencyDescription"].ToString() == "Half Dollar")
                    {
                        CurrencyID = Configuration.convertNullToInt(dsStationCloseData.Tables[0].Rows[index]["CurrencyDenomID"].ToString());
                        oStationClose.VerifyStationCloseCash(StationCloseID, CurrencyID, Configuration.convertNullToInt(numHalfDollar.Value), Configuration.convertNullToDecimal(lblHalfDollarTotal.Text));
                    }
                    if (dsCurrencyList.Tables[0].Rows[id]["CurrencyDescription"].ToString() == "One Dollar" && Configuration.convertNullToBoolean(dsCurrencyList.Tables[0].Rows[id]["IsCoin"].ToString()))
                    {
                        CurrencyID = Configuration.convertNullToInt(dsStationCloseData.Tables[0].Rows[id]["CurrencyDenomID"].ToString());
                        oStationClose.VerifyStationCloseCash(StationCloseID, CurrencyID, Configuration.convertNullToInt(numOneDollar.Value), Configuration.convertNullToDecimal(lblOneDollarTotal.Text));
                    }
                    if (dsCurrencyList.Tables[0].Rows[id]["CurrencyDescription"].ToString() == "Two Dollar" && Configuration.convertNullToBoolean(dsCurrencyList.Tables[0].Rows[id]["IsCoin"].ToString()))
                    {
                        CurrencyID = Configuration.convertNullToInt(dsStationCloseData.Tables[0].Rows[index]["CurrencyDenomID"].ToString());
                        oStationClose.VerifyStationCloseCash(StationCloseID, CurrencyID, Configuration.convertNullToInt(numTwoDollar.Value), Configuration.convertNullToDecimal(lblTwoDollarTotal.Text));
                    }
                    #endregion

                    #region Bill
                    if (dsCurrencyList.Tables[0].Rows[id]["CurrencyDescription"].ToString() == "One Dollar" && !Configuration.convertNullToBoolean(dsCurrencyList.Tables[0].Rows[id]["IsCoin"].ToString()))
                    {
                        CurrencyID = Configuration.convertNullToInt(dsStationCloseData.Tables[0].Rows[index]["CurrencyDenomID"].ToString());
                        oStationClose.VerifyStationCloseCash(StationCloseID, CurrencyID, Configuration.convertNullToInt(numOneDollarBill.Value), Configuration.convertNullToDecimal(lblOneDollarBillTotal.Text));
                    }
                    if (dsCurrencyList.Tables[0].Rows[id]["CurrencyDescription"].ToString() == "Two Dollar" && !Configuration.convertNullToBoolean(dsCurrencyList.Tables[0].Rows[id]["IsCoin"].ToString()))
                    {
                        CurrencyID = Configuration.convertNullToInt(dsStationCloseData.Tables[0].Rows[index]["CurrencyDenomID"].ToString());
                        oStationClose.VerifyStationCloseCash(StationCloseID, CurrencyID, Configuration.convertNullToInt(numTwoDollarBill.Value), Configuration.convertNullToDecimal(lblTwoDoallrBillTotal.Text));
                    }
                    if (dsCurrencyList.Tables[0].Rows[id]["CurrencyDescription"].ToString() == "Five Dollar")
                    {
                        CurrencyID = Configuration.convertNullToInt(dsStationCloseData.Tables[0].Rows[index]["CurrencyDenomID"].ToString());
                        oStationClose.VerifyStationCloseCash(StationCloseID, CurrencyID, Configuration.convertNullToInt(numFiveDollar.Value), Configuration.convertNullToDecimal(lblFiveDollarTotal.Text));
                    }
                    if (dsCurrencyList.Tables[0].Rows[id]["CurrencyDescription"].ToString() == "Ten Dollar")
                    {
                        CurrencyID = Configuration.convertNullToInt(dsStationCloseData.Tables[0].Rows[index]["CurrencyDenomID"].ToString());
                        oStationClose.VerifyStationCloseCash(StationCloseID, CurrencyID, Configuration.convertNullToInt(numTenDollar.Value), Configuration.convertNullToDecimal(lblTenDollarTotal.Text));
                    }
                    if (dsCurrencyList.Tables[0].Rows[id]["CurrencyDescription"].ToString() == "Twenty Dollar")
                    {
                        CurrencyID = Configuration.convertNullToInt(dsStationCloseData.Tables[0].Rows[index]["CurrencyDenomID"].ToString());
                        oStationClose.VerifyStationCloseCash(StationCloseID, CurrencyID, Configuration.convertNullToInt(numTwentyDollar.Value), Configuration.convertNullToDecimal(lblTwentyDollarTotal.Text));
                    }
                    if (dsCurrencyList.Tables[0].Rows[id]["CurrencyDescription"].ToString() == "Fifty Dollar")
                    {
                        CurrencyID = Configuration.convertNullToInt(dsStationCloseData.Tables[0].Rows[index]["CurrencyDenomID"].ToString());
                        oStationClose.VerifyStationCloseCash(StationCloseID, CurrencyID, Configuration.convertNullToInt(numFiftyDollar.Value), Configuration.convertNullToDecimal(lblFiftyDollarTotal.Text));
                    }
                    if (dsCurrencyList.Tables[0].Rows[id]["CurrencyDescription"].ToString() == "One Hundered Dollar")
                    {
                        CurrencyID = Configuration.convertNullToInt(dsStationCloseData.Tables[0].Rows[index]["CurrencyDenomID"].ToString());
                        oStationClose.VerifyStationCloseCash(StationCloseID, CurrencyID, Configuration.convertNullToInt(numHundredDollar.Value), Configuration.convertNullToDecimal(lblHundredDollarTotal.Text));
                    }
                    if (dsCurrencyList.Tables[0].Rows[id]["CurrencyDescription"].ToString() == "One Thousand Dollar")
                    {
                        CurrencyID = Configuration.convertNullToInt(dsStationCloseData.Tables[0].Rows[index]["CurrencyDenomID"].ToString());
                        oStationClose.VerifyStationCloseCash(StationCloseID, CurrencyID, Configuration.convertNullToInt(numOneThousandDollar.Value), Configuration.convertNullToDecimal(lblOnrThousandDollarTotal.Text));
                    }
                    #endregion
                    retValue = true;
                }
            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
            return retValue;
        }

        private bool Save()
        {
            int CurrencyID;
            bool retValue = false;
            try
            {
                for (int index = 0; index < dsCurrencyList.Tables[0].Rows.Count; index++)
                {
                    #region Coins
                    if (dsCurrencyList.Tables[0].Rows[index]["CurrencyDescription"].ToString() == "Penny")
                    {
                        CurrencyID = Configuration.convertNullToInt(dsCurrencyList.Tables[0].Rows[index]["CurrencyDenomID"].ToString());
                        oStationClose.SaveStationCloseCashDeatil(CurrencyID, Configuration.convertNullToInt(numPenny.Value), Configuration.convertNullToDecimal(lblPennyTotal.Text));
                    }
                    if (dsCurrencyList.Tables[0].Rows[index]["CurrencyDescription"].ToString() == "Nickel")
                    {
                        CurrencyID = Configuration.convertNullToInt(dsCurrencyList.Tables[0].Rows[index]["CurrencyDenomID"].ToString());
                        oStationClose.SaveStationCloseCashDeatil(CurrencyID, Configuration.convertNullToInt(numNickel.Value), Configuration.convertNullToDecimal(lblNickelTotal.Text));
                    }
                    if (dsCurrencyList.Tables[0].Rows[index]["CurrencyDescription"].ToString() == "Dime")
                    {
                        CurrencyID = Configuration.convertNullToInt(dsCurrencyList.Tables[0].Rows[index]["CurrencyDenomID"].ToString());
                        oStationClose.SaveStationCloseCashDeatil(CurrencyID, Configuration.convertNullToInt(numDime.Value), Configuration.convertNullToDecimal(lblDimeTotal.Text));
                    }
                    if (dsCurrencyList.Tables[0].Rows[index]["CurrencyDescription"].ToString() == "Quarter")
                    {
                        CurrencyID = Configuration.convertNullToInt(dsCurrencyList.Tables[0].Rows[index]["CurrencyDenomID"].ToString());
                        oStationClose.SaveStationCloseCashDeatil(CurrencyID, Configuration.convertNullToInt(numQuater.Value), Configuration.convertNullToDecimal(lblQuarterTotal.Text));
                    }
                    if (dsCurrencyList.Tables[0].Rows[index]["CurrencyDescription"].ToString() == "Half Dollar")
                    {
                        CurrencyID = Configuration.convertNullToInt(dsCurrencyList.Tables[0].Rows[index]["CurrencyDenomID"].ToString());
                        oStationClose.SaveStationCloseCashDeatil(CurrencyID, Configuration.convertNullToInt(numHalfDollar.Value), Configuration.convertNullToDecimal(lblHalfDollarTotal.Text));
                    }
                    if (dsCurrencyList.Tables[0].Rows[index]["CurrencyDescription"].ToString() == "One Dollar" && Configuration.convertNullToBoolean(dsCurrencyList.Tables[0].Rows[index]["IsCoin"].ToString()))
                    {
                        CurrencyID = Configuration.convertNullToInt(dsCurrencyList.Tables[0].Rows[index]["CurrencyDenomID"].ToString());
                        oStationClose.SaveStationCloseCashDeatil(CurrencyID, Configuration.convertNullToInt(numOneDollar.Value), Configuration.convertNullToDecimal(lblOneDollarTotal.Text));
                    }
                    if (dsCurrencyList.Tables[0].Rows[index]["CurrencyDescription"].ToString() == "Two Dollar" && Configuration.convertNullToBoolean(dsCurrencyList.Tables[0].Rows[index]["IsCoin"].ToString()))
                    {
                        CurrencyID = Configuration.convertNullToInt(dsCurrencyList.Tables[0].Rows[index]["CurrencyDenomID"].ToString());
                        oStationClose.SaveStationCloseCashDeatil(CurrencyID, Configuration.convertNullToInt(numTwoDollar.Value), Configuration.convertNullToDecimal(lblTwoDollarTotal.Text));
                    }
                    #endregion

                    #region Bills
                    if (dsCurrencyList.Tables[0].Rows[index]["CurrencyDescription"].ToString() == "One Dollar" && !Configuration.convertNullToBoolean(dsCurrencyList.Tables[0].Rows[index]["IsCoin"].ToString()))
                    {
                        CurrencyID = Configuration.convertNullToInt(dsCurrencyList.Tables[0].Rows[index]["CurrencyDenomID"].ToString());
                        oStationClose.SaveStationCloseCashDeatil(CurrencyID, Configuration.convertNullToInt(numOneDollarBill.Value), Configuration.convertNullToDecimal(lblOneDollarBillTotal.Text));
                    }
                    if (dsCurrencyList.Tables[0].Rows[index]["CurrencyDescription"].ToString() == "Two Dollar" && !Configuration.convertNullToBoolean(dsCurrencyList.Tables[0].Rows[index]["IsCoin"].ToString()))
                    {
                        CurrencyID = Configuration.convertNullToInt(dsCurrencyList.Tables[0].Rows[index]["CurrencyDenomID"].ToString());
                        oStationClose.SaveStationCloseCashDeatil(CurrencyID, Configuration.convertNullToInt(numTwoDollarBill.Value), Configuration.convertNullToDecimal(lblTwoDoallrBillTotal.Text));
                    }
                    if (dsCurrencyList.Tables[0].Rows[index]["CurrencyDescription"].ToString() == "Five Dollar")
                    {
                        CurrencyID = Configuration.convertNullToInt(dsCurrencyList.Tables[0].Rows[index]["CurrencyDenomID"].ToString());
                        oStationClose.SaveStationCloseCashDeatil(CurrencyID, Configuration.convertNullToInt(numFiveDollar.Value), Configuration.convertNullToDecimal(lblFiveDollarTotal.Text));
                    }
                    if (dsCurrencyList.Tables[0].Rows[index]["CurrencyDescription"].ToString() == "Ten Dollar")
                    {
                        CurrencyID = Configuration.convertNullToInt(dsCurrencyList.Tables[0].Rows[index]["CurrencyDenomID"].ToString());
                        oStationClose.SaveStationCloseCashDeatil(CurrencyID, Configuration.convertNullToInt(numTenDollar.Value), Configuration.convertNullToDecimal(lblTenDollarTotal.Text));
                    }
                    if (dsCurrencyList.Tables[0].Rows[index]["CurrencyDescription"].ToString() == "Twenty Dollar")
                    {
                        CurrencyID = Configuration.convertNullToInt(dsCurrencyList.Tables[0].Rows[index]["CurrencyDenomID"].ToString());
                        oStationClose.SaveStationCloseCashDeatil(CurrencyID, Configuration.convertNullToInt(numTwentyDollar.Value), Configuration.convertNullToDecimal(lblTwentyDollarTotal.Text));
                    }
                    if (dsCurrencyList.Tables[0].Rows[index]["CurrencyDescription"].ToString() == "Fifty Dollar")
                    {
                        CurrencyID = Configuration.convertNullToInt(dsCurrencyList.Tables[0].Rows[index]["CurrencyDenomID"].ToString());
                        oStationClose.SaveStationCloseCashDeatil(CurrencyID, Configuration.convertNullToInt(numFiftyDollar.Value), Configuration.convertNullToDecimal(lblFiftyDollarTotal.Text));
                    }
                    if (dsCurrencyList.Tables[0].Rows[index]["CurrencyDescription"].ToString() == "One Hundered Dollar")
                    {
                        CurrencyID = Configuration.convertNullToInt(dsCurrencyList.Tables[0].Rows[index]["CurrencyDenomID"].ToString());
                        oStationClose.SaveStationCloseCashDeatil(CurrencyID, Configuration.convertNullToInt(numHundredDollar.Value), Configuration.convertNullToDecimal(lblHundredDollarTotal.Text));
                    }
                    if (dsCurrencyList.Tables[0].Rows[index]["CurrencyDescription"].ToString() == "One Thousand Dollar")
                    {
                        CurrencyID = Configuration.convertNullToInt(dsCurrencyList.Tables[0].Rows[index]["CurrencyDenomID"].ToString());
                        oStationClose.SaveStationCloseCashDeatil(CurrencyID, Configuration.convertNullToInt(numOneThousandDollar.Value), Configuration.convertNullToDecimal(lblOnrThousandDollarTotal.Text));
                    }
                    #endregion
                    retValue = true;
                }
            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
            return retValue;
        }

        #region NumBox TextChanged Event
        private void numBox_ValueChanged(object sender, EventArgs e)
        {
            Infragistics.Win.UltraWinEditors.UltraNumericEditor numEditor = (Infragistics.Win.UltraWinEditors.UltraNumericEditor)sender;
            switch (numEditor.Name)
            {
                case "numPenny":
                    lblPennyTotal.Text = (Decimal.Parse(numPenny.Value.ToString()) * (Decimal)0.010).ToString("######0.00");
                    break;

                case "numNickel":
                    lblNickelTotal.Text = (Decimal.Parse(numNickel.Value.ToString()) * (Decimal)0.050).ToString("######0.00");
                    break;

                case "numDime":
                    lblDimeTotal.Text = (Decimal.Parse(numDime.Value.ToString()) * (Decimal)0.10).ToString("######0.00"); 
                    break;

                case "numQuater":
                    lblQuarterTotal.Text = (Decimal.Parse(numQuater.Value.ToString()) * (Decimal)0.25).ToString("######0.00");
                    break;

                case "numHalfDollar":
                    lblHalfDollarTotal.Text = (Decimal.Parse(numHalfDollar.Value.ToString()) * (Decimal)0.50).ToString("######0.00");
                    break;
                    
                case "numOneDollar":
                    lblOneDollarTotal.Text = Convert.ToString(Decimal.Parse(numOneDollar.Value.ToString()) * (Decimal)1.00);
                    break;

                case "numTwoDollar":
                    lblTwoDollarTotal.Text = Convert.ToString(Decimal.Parse(numTwoDollar.Value.ToString()) * (Decimal)2.00);
                    break;

                case "numOneDollarBill":
                    lblOneDollarBillTotal.Text = Convert.ToString(Decimal.Parse(numOneDollarBill.Value.ToString()) * (Decimal)1.00);
                    break;

                case "numTwoDollarBill":
                    lblTwoDoallrBillTotal.Text = Convert.ToString(Decimal.Parse(numTwoDollarBill.Value.ToString()) * (Decimal)2.00);
                    break;

                case "numFiveDollar":
                    lblFiveDollarTotal.Text = Convert.ToString(Decimal.Parse(numFiveDollar.Value.ToString()) * (Decimal)5.00);
                    break;

                case "numTenDollar":
                    lblTenDollarTotal.Text = Convert.ToString(Decimal.Parse(numTenDollar.Value.ToString()) * (Decimal)10.00);
                    break;

                case "numTwentyDollar":
                    lblTwentyDollarTotal.Text = Convert.ToString(Decimal.Parse(numTwentyDollar.Value.ToString()) * (Decimal)20.00);
                    break;

                case "numFiftyDollar":
                    lblFiftyDollarTotal.Text = Convert.ToString(Decimal.Parse(numFiftyDollar.Value.ToString()) * (Decimal)50.00);
                    break;

                case "numHundredDollar":
                    lblHundredDollarTotal.Text = Convert.ToString(Decimal.Parse(numHundredDollar.Value.ToString()) * (Decimal)100.00);
                    break;

                case "numOneThousandDollar":
                    lblOnrThousandDollarTotal.Text = Convert.ToString(Decimal.Parse(numOneThousandDollar.Value.ToString()) * (Decimal)1000.00);
                    break;
                default:
                    break;
            }

            #region PRIMEPOS-2554 29-Jun-2018 JY Added
            numCoinTotal.Value = Configuration.convertNullToDecimal(lblPennyTotal.Text) + Configuration.convertNullToDecimal(lblNickelTotal.Text) +
                                Configuration.convertNullToDecimal(lblDimeTotal.Text) + Configuration.convertNullToDecimal(lblQuarterTotal.Text) +
                                Configuration.convertNullToDecimal(lblHalfDollarTotal.Text) + Configuration.convertNullToDecimal(lblOneDollarTotal.Text) +
                                Configuration.convertNullToDecimal(lblTwoDollarTotal.Text);
            nunBillTotal.Value = Configuration.convertNullToDecimal(lblOneDollarBillTotal.Text) + Configuration.convertNullToDecimal(lblTwoDoallrBillTotal.Text) +
                                Configuration.convertNullToDecimal(lblFiveDollarTotal.Text) + Configuration.convertNullToDecimal(lblTenDollarTotal.Text) +
                                Configuration.convertNullToDecimal(lblTwentyDollarTotal.Text) + Configuration.convertNullToDecimal(lblFiftyDollarTotal.Text) +
                                Configuration.convertNullToDecimal(lblHundredDollarTotal.Text) + Configuration.convertNullToDecimal(lblOnrThousandDollarTotal.Text);

            numGrandTotal.Value = Configuration.convertNullToDecimal(numCoinTotal.Value) + Configuration.convertNullToDecimal(nunBillTotal.Value);
            #endregion
        }
        #endregion

        private void frmStationCloseCash_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyData == System.Windows.Forms.Keys.Enter || e.KeyData == System.Windows.Forms.Keys.Tab)
                {
                    SelectNextControl(this.ActiveControl, true, true, true, true);
                }
            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }

        private void frmStationCloseCash_FormClosed(object sender, FormClosedEventArgs e)
        {

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            FlagCloseStationForm = true;
            this.Close();
        }

        private void numGrandTotal_Enter(object sender, EventArgs e)
        {
            if (Configuration.convertNullToDecimal(numCoinTotal.Value) == 0 && Configuration.convertNullToDecimal(nunBillTotal.Value) == 0)
            {
                numGrandTotal.ReadOnly = false;
                grpBoxBillDeno.Enabled = false;
                grpBoxCoinDeno.Enabled = false;
            }
        }

        private void numGrandTotal_Leave(object sender, EventArgs e)
        {
            numGrandTotal.ReadOnly = true;
        }

        #region PRIMEPOS-2554 29-Jun-2018 JY Commented
        //private void SetBillTotal(object sender, EventArgs e)
        //{
        //    return;
        //    billTotal = 0;
        //    try
        //    {
        //        if (Convert.ToInt32(numOneDollarBill.Value) > 0)
        //        {
        //            oneDollarBillCount = Convert.ToInt32(numOneDollarBill.Value);
        //            oneDollarBillTotal = Convert.ToDecimal(lblOneDollarBillTotal.Text);
        //            billTotal += oneDollarBillTotal;
        //        }
        //        if (Convert.ToInt32(numTwoDollarBill.Value) > 0)
        //        {
        //            twoDollarBillCount = Convert.ToInt32(numTwoDollarBill.Value);
        //            twoDollarBillTotal = Convert.ToDecimal(lblTwoDoallrBillTotal.Text);
        //            billTotal += twoDollarBillTotal;
        //        }
        //        if (Convert.ToInt32(numFiveDollar.Value) > 0)
        //        {
        //            fiveDollarCount = Convert.ToInt32(numFiveDollar.Value);
        //            fiveDollarTotal = Convert.ToDecimal(lblFiveDollarTotal.Text);
        //            billTotal += fiveDollarTotal;
        //        }
        //        if (Convert.ToInt32(numTenDollar.Value) > 0)
        //        {
        //            tenDollarCount = Convert.ToInt32(numTenDollar.Value);
        //            tenDollarTotal = Convert.ToDecimal(lblTenDollarTotal.Text);
        //            billTotal += tenDollarTotal;
        //        }
        //        if (Convert.ToInt32(numTwentyDollar.Value) > 0)
        //        {
        //            twentyDollarCount = Convert.ToInt32(numTwentyDollar.Value);
        //            twentyDollarTotal = Convert.ToDecimal(lblTwentyDollarTotal.Text);
        //            billTotal += twentyDollarTotal;
        //        }
        //        if (Convert.ToInt32(numFiftyDollar.Value) > 0)
        //        {
        //            fiftyDollarCount = Convert.ToInt32(numFiftyDollar.Value);
        //            fiftyDollarTotal = Convert.ToDecimal(lblFiftyDollarTotal.Text);
        //            billTotal += fiftyDollarTotal;
        //        }
        //        if (Convert.ToInt32(numHundredDollar.Value) > 0)
        //        {
        //            hundredDollarCount = Convert.ToInt32(numHundredDollar.Value);
        //            hundredDollarTotal = Convert.ToDecimal(lblHundredDollarTotal.Text);
        //            billTotal += hundredDollarTotal;
        //        }
        //        if (Convert.ToDecimal(numOneThousandDollar.Value) > 0)
        //        {
        //            thousandDollarCount = Convert.ToInt32(numOneThousandDollar.Value);
        //            thousandDollarTotal = Convert.ToDecimal(lblOnrThousandDollarTotal.Text);
        //            billTotal += thousandDollarTotal;
        //        }
        //        nunBillTotal.Value = billTotal;
        //        numGrandTotal.Value = billTotal + coinTotal;

        //    }
        //    catch (Exception exp)
        //    {
        //        clsUIHelper.ShowErrorMsg(exp.Message);
        //    }

        //}

        //public void SetCoinTotal(object sender, EventArgs e)
        //{
        //    coinTotal = 0;
        //    try
        //    {
        //        if (Convert.ToInt32(numNickel.Value) > 0)
        //        {
        //            nickelCount = Convert.ToInt32(numNickel.Value);
        //            nickelTotal = Convert.ToDecimal(lblNickelTotal.Text.ToString());
        //            coinTotal += nickelTotal;
        //        }

        //        if (Convert.ToInt32(numPenny.Value) > 0)
        //        {
        //            pennyCount = Convert.ToInt32(numPenny.Value);
        //            pennyTotal = Convert.ToDecimal(lblPennyTotal.Text.ToString());
        //            coinTotal += pennyTotal;
        //        }
        //        if (Convert.ToInt32(numDime.Value) > 0)
        //        {
        //            dimeCount = Convert.ToInt32(numDime.Value);
        //            dimeTotal = Convert.ToDecimal(lblDimeTotal.Text.ToString());
        //            coinTotal += dimeTotal;
        //        }
        //        if (Convert.ToInt32(numQuater.Value) > 0)
        //        {
        //            quaterCount = Convert.ToInt32(numQuater.Value);
        //            quaterTotal = Convert.ToDecimal(lblQuarterTotal.Text.ToString());
        //            coinTotal += quaterTotal;
        //        }
        //        if (Convert.ToInt32(numHalfDollar.Value) > 0)
        //        {
        //            halfDollarCount = Convert.ToInt32(numHalfDollar.Value);
        //            halfDollarTotal = Convert.ToDecimal(lblHalfDollarTotal.Text.ToString());
        //            coinTotal += halfDollarTotal;
        //        }
        //        if (Convert.ToInt32(numOneDollar.Value) > 0)
        //        {
        //            oneDollarCointCount = Convert.ToInt32(numOneDollar.Value);
        //            oneDollarCoinTotal = Convert.ToDecimal(lblOneDollarTotal.Text.ToString());
        //            coinTotal += oneDollarCoinTotal;
        //        }
        //        if (Convert.ToInt32(numTwoDollar.Value) > 0)
        //        {
        //            twoDollarCoinCount = Convert.ToInt32(numTwoDollar.Value);
        //            twoDollarCoinTotal = Convert.ToDecimal(lblTwoDollarTotal.Text.ToString());
        //            coinTotal += twoDollarCoinTotal;

        //        }
        //        numCoinTotal.Value = coinTotal;
        //        numGrandTotal.Value = coinTotal + billTotal;
        //    }
        //    catch (Exception exp)
        //    {
        //        clsUIHelper.ShowErrorMsg(exp.Message);
        //    }
        //}
        #endregion
    }
}
