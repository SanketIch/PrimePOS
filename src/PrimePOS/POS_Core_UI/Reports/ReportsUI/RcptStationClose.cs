using System;
using System.Reflection;
using System.IO;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using Microsoft.Vsa;
using System.Drawing.Printing;
using System.Windows.Forms;
using POS_Core.BusinessRules;
using System.Data;
using POS_Core.Resources;
using POS_Core.LabelHandler.RxLabel;

namespace POS_Core_UI.Reports.ReportsUI
{
    /// <summary>
    /// Summary description for RcptStationClose.
    /// </summary>
    public class RcptStationClose
    {
        protected PrintDestination mPrintDest = PrintDestination.Printer;
        protected string PrinterName;
        protected string PaperSource;
        protected string CounsType = " ";
        protected bool InsertBlankLine = false;
        public System.Drawing.Font CFont;
        public System.Drawing.Brush CBrush;

        protected float CSize;
        protected PrintDocument mPD;

        public System.Drawing.Font Courier;
        public System.Drawing.Font Arial;
        public System.Drawing.Font MSSanSerif;
        public System.Drawing.Font Times;

        public readonly int CloseID;
        public string UserID;
        public string StationID;
        public string CloseDate;

        public DataSet DSstClose;
        public DataSet DSstCloseSummary;
        public DataRelation RlPayment;
        public DataRelation RlTransaction;

        protected bool PrintPHName = false;

        public PrintPageEventArgs PR;

        private int mCopies = 1;
        private System.IntPtr lhPrinter;

        public string Replicate(string Data, int Len)
        {
            string str = Data;
            for (int i = 1; i <= Len; i++)
                str += Data;
            return str;
        }

        public string FLen(string Data, int Len)
        {
            string str;
            str = Data;
            for (int i = 1; i <= (Len - Data.Length); i++)
                str += " ";
            return str;
        }

        public string Space(int Spaces)
        {
            string str = "";
            for (int i = 1; i <= Spaces; i++)
                str += " ";
            return str;
        }

        public RcptStationClose(int StCloseID)
        {
            //
            // TODO: Add constructor logic here
            //
            this.CloseID = StCloseID;
            InitClass();
        }

        public PrintDestination PDestination
        {
            set { this.mPrintDest = value; }
            get { return mPrintDest; }
        }

        public int CopiesToPrint
        {
            set { this.mCopies = value; }
            get { return mCopies; }
        }

        private void InitClass()
        {
            Courier = new System.Drawing.Font("Courier", 10, System.Drawing.FontStyle.Regular);
            Arial = new System.Drawing.Font("Arial", 10, System.Drawing.FontStyle.Regular);
            MSSanSerif = new System.Drawing.Font("MS San Serif", 10, System.Drawing.FontStyle.Regular);
            Times = new System.Drawing.Font("New Times Roman", 10, System.Drawing.FontStyle.Regular);
            this.CFont = Courier;
            this.CSize = 10;

            this.CBrush = new System.Drawing.SolidBrush(Color.Black);

            mPD = new PrintDocument();
            mPD.PrintController = new StandardPrintController();
            initReportData();
            initReportDataForWholeStation();
        }


        private void initReportData()
        {
            DSstClose = new DataSet();
            DataSet oDS = new DataSet();
            Search oSearch = new Search();
            oDS = oSearch.SearchData("SELECT DISTINCT SCH.*,SCD.DRAWNO FROM StationCloseHeader sch INNER JOIN " +
                  " StationCloseDetail scd ON sch.StationCloseID = scd.StationCloseID WHERE sch.StationCloseID = " + CloseID);
            DSstClose.Tables.Add(oDS.Tables[0].Copy());
            DSstClose.Tables[0].TableName = "masterTable";

            oDS = new DataSet();
            //PRIMEPOS-2983 02-Jul-2021 JY Modified
            oDS = oSearch.SearchData(" select drawno,pt.paytypedesc,scd.transcount, transamount " +
                    " from stationclosedetail scd,paytype pt where substring(scd.transtype,3,len(scd.transtype))=pt.paytypeid" +
                    " and stationcloseid=" + CloseID);

            DSstClose.Tables.Add(oDS.Tables[0].Copy());
            DSstClose.Tables[1].TableName = "payment";

            oDS = new DataSet();
            oDS = oSearch.SearchData(" select drawno,'Sales' as Type ,transcount,transamount from stationclosedetail where stationcloseid=" + CloseID + " and transtype='S' " +
                    " union all " +
                    " select drawno,'Return' as Type ,transcount,transamount from stationclosedetail where stationcloseid=" + CloseID + " and transtype='SR' " +
                    " union all " +
                    " select drawno,'Tax' as Type ,transcount,transamount from stationclosedetail where stationcloseid=" + CloseID + " and transtype='TX' " +
                    " union all " +
                    " select drawno,'Discount' as Type ,transcount,-1*transamount from stationclosedetail where stationcloseid=" + CloseID + " and transtype='DT'");
            DSstClose.Tables.Add(oDS.Tables[0].Copy());
            DSstClose.Tables[2].TableName = "transaction";

            RlPayment = DSstClose.Relations.Add(DSstClose.Tables[0].Columns["drawno"], DSstClose.Tables[1].Columns["drawno"]);
            RlTransaction = DSstClose.Relations.Add(DSstClose.Tables[0].Columns["DrawNo"], DSstClose.Tables[2].Columns["drawno"]);

            if (DSstClose.Tables[0].Rows.Count > 0)
            {
                this.UserID = DSstClose.Tables[0].Rows[0]["Userid"].ToString();
                this.StationID = DSstClose.Tables[0].Rows[0]["Stationid"].ToString();
                //this.CloseDate = Convert.ToDateTime(DSstClose.Tables[0].Rows[0]["CloseDate"].ToString()).ToShortDateString(); //PRIMEPOS-3114 26-Jul-2022 JY Commented
                this.CloseDate = Convert.ToDateTime(DSstClose.Tables[0].Rows[0]["CloseDate"].ToString()).ToString("MM/dd/yyyy HH:mm tt");   //PRIMEPOS-3114 26-Jul-2022 JY Added
            }

        }

        #region PRIMEPOS-2638 07-Feb-2019 JY Added
        private decimal TotalCashByDraw()
        {
            DataSet oDS = new DataSet();
            Search oSearch = new Search();
            string strSQL = "SELECT ISNULL(SUM(ISNULL(TransAmount,0)),0) FROM StationCloseDetail WHERE StationCloseID = " + CloseID + " and TransType = 'U-1'";
            oDS = oSearch.SearchData(strSQL);
            return Configuration.convertNullToDecimal(oDS.Tables[0].Rows[0][0].ToString());
        }
        #endregion

        private decimal TotalCashByDraw(String DrawNo)
        {
            DataSet oDS = new DataSet();
            Search oSearch = new Search();
            string strSQL = "SELECT ISNULL(SUM(ISNULL(TransAmount,0)),0) FROM StationCloseDetail " +
                            " WHERE DrawNo = " + DrawNo + " AND StationCloseID = " + CloseID + " and TransType = 'U-1'";
            oDS = oSearch.SearchData(strSQL);
            return Configuration.convertNullToDecimal(oDS.Tables[0].Rows[0][0].ToString());
        }

        #region PRIMEPOS-2638 07-Feb-2019 JY Added
        private decimal TotalPayoutByDraw()
        {
            DataSet oDS = new DataSet();
            Search oSearch = new Search();
            string strSQL = "SELECT ISNULL(SUM(ISNULL(TransAmount,0)),0) FROM StationCloseDetail WHERE StationCloseID = " + CloseID + " and TransType = 'PO'";
            oDS = oSearch.SearchData(strSQL);
            return Configuration.convertNullToDecimal(oDS.Tables[0].Rows[0][0].ToString());
        }
        #endregion

        private decimal TotalPayoutByDraw(String DrawNo)
        {
            DataSet oDS = new DataSet();
            Search oSearch = new Search();
            string strSQL = "SELECT ISNULL(SUM(ISNULL(TransAmount,0)),0) FROM StationCloseDetail " +
                            " WHERE DrawNo = " + DrawNo + " AND StationCloseID = " + CloseID + " and TransType = 'PO'";
            oDS = oSearch.SearchData(strSQL);
            return Configuration.convertNullToDecimal(oDS.Tables[0].Rows[0][0].ToString());
        }

        private void initReportDataForWholeStation()
        {
            DSstCloseSummary = new DataSet();
            DataSet oDS = new DataSet();
            Search oSearch = new Search();
            oDS = oSearch.SearchData("SELECT DISTINCT SCH.* FROM StationCloseHeader sch " +
                " WHERE sch.StationCloseID = " + CloseID);
            DSstCloseSummary.Tables.Add(oDS.Tables[0].Copy());
            DSstCloseSummary.Tables[0].TableName = "masterTable";

            oDS = new DataSet();
            //PRIMEPOS-2983 02-Jul-2021 JY Modified
            oDS = oSearch.SearchData(" select pt.paytypedesc,sum(scd.transcount) as transcount, sum(transamount) as transamount " +
                " from stationclosedetail scd,paytype pt where substring(scd.transtype,3,len(scd.transtype))=pt.paytypeid" +
                " and stationcloseid=" + CloseID + " group by pt.paytypedesc,pt.PayTypeID order by pt.PayTypeID ");

            DSstCloseSummary.Tables.Add(oDS.Tables[0].Copy());
            DSstCloseSummary.Tables[1].TableName = "payment";

            oDS = new DataSet();
            oDS = oSearch.SearchData(" select 'Sales' as Type ,sum(transcount) as transcount,sum(transamount) as transamount from stationclosedetail where stationcloseid=" + CloseID + " and transtype='S' " +
                " union all " +
                " select 'Return' as Type ,sum(transcount) as transcount,sum(transamount) as transamount from stationclosedetail where stationcloseid=" + CloseID + " and transtype='SR' " +
                " union all " +
                " select 'Tax' as Type ,sum(transcount) as transcount,sum(transamount) as transamount from stationclosedetail where stationcloseid=" + CloseID + " and transtype='TX' " +
                " union all " +
                " select 'Discount' as Type ,sum(transcount) as transcount,sum(-1*transamount) as transamount from stationclosedetail where stationcloseid=" + CloseID + " and transtype='DT'");
            DSstCloseSummary.Tables.Add(oDS.Tables[0].Copy());
            DSstCloseSummary.Tables[2].TableName = "transaction";

        }
        public void PrintLine(string LineToPrint)
        {
            int pcWritten = 0;
            POS_Core.ErrorLogging.Logs.Logger(LineToPrint);
            PrintDirectAPIs.WritePrinter(lhPrinter, LineToPrint, LineToPrint.Length, ref pcWritten);
            return;
        }

        public void Print()
        {
            // this routine prints the dot matrix label
            //				string sLabelFile;
            //				sLabelFile="rcptStationClose.js";
            //
            //				if (System.IO.File.Exists(sLabelFile))
            //				{
            this.lhPrinter = new System.IntPtr();
            //                        
            DOCINFO di = new DOCINFO();
            di.pDocName = "Label";
            di.pDataType = "RAW";
            //Added By Shitaljit(QuicSolv) on 20 June 2011
            StationClose oStationClose = new StationClose();
            DataSet dsStCloseData = oStationClose.GetStationCloseCashDetail(CloseID);
            // the \x1b means an ascii escape character
            //st1="\x1b*c600a6b0P\f";
            //lhPrinter contains the handle for the printer opened
            //If lhPrinter is 0 then an error has occured
            //bool bSuccess=false;
            if (Configuration.CPOSSet.RP_Name.Trim() == "")
            {
                //	bSuccess=true;
            }
            else if (PrintDirectAPIs.OpenPrinter(Configuration.CPOSSet.RP_Name.Trim(), ref lhPrinter, 0))
            {
                if (PrintDirectAPIs.StartDocPrinter(lhPrinter, 1, ref di))
                {
                    if (PrintDirectAPIs.StartPagePrinter(lhPrinter))
                    {

                        //								ScriptableLabel _lbl = new ScriptableLabel(this,sLabelFile);

                        try
                        {
                            //for(int i=0; i < Configuration.CInfo.NoOfReceipt; i++)
                            //{
                            //									_lbl.PrintCLabel();   // this will actually send the label printing lines
                            //Following If is added By Shitaljit(QuicSolv) 0n 20 june 2011
                            if (dsStCloseData.Tables[0].Rows.Count > 0)
                            {
                                PrintStationCloseCash();
                            }
                            else
                            {
                                PrintStationClose();
                            }


                            //}
                            //bSuccess=true;
                        }
                        catch (Exception e)
                        {
                            Resources.Message.Display(e.Message.ToString(), "ERROR Printing Label", MessageBoxButtons.OK, MessageBoxIcon.Error);

                        }
                    }
                }
            }
            PrintDirectAPIs.EndPagePrinter(lhPrinter);
            PrintDirectAPIs.EndDocPrinter(lhPrinter);
            PrintDirectAPIs.ClosePrinter(lhPrinter);
            //					if (bSuccess == false)
            //					{
            //						clsUIHelper.ShowErrorMsg("Unable to connect to printer");
            //					
            //					}
            return;
            //				}
        }

        //Added By Shitaljit(QuicSolv) on june 20 2011
        //Print StatinClose Report with cash denominations.
        private void PrintStationCloseCash()
        {
            //PrintLine("\x1B"+"p"+"011");

            //PrintLine("\x1B"+"x0");

            this.BlankLineBeforeHeading = true;

            PrintLine(Convert.ToChar(1).ToString());
            PrintLine("\n" + FLen("CLOSE STATION REPORT", 40));
            PrintLine("\n" + FLen("Close#:" + CloseID.ToString(), 17) + "User ID:" + UserID);   //PRIMEPOS-3114 26-Jul-2022 JY Modified
            PrintLine("\n" + FLen("Station#:" + StationID, 17) + "Date:" + CloseDate);  //PRIMEPOS-3114 26-Jul-2022 JY Modified
            #region Currency Denominations
            Decimal TotalCashSummary = 0;
            Decimal TotalPayoutSummary = 0;
            Decimal TotalROASummary = 0;
            Decimal nTotAmount;
            Decimal nTotTrans;
            Decimal billTotCount = 0;
            Decimal billTotal = 0;
            Decimal coinTotCount = 0;
            Decimal coinTotal = 0;
            Decimal TotalTransFee = 0;   //PRIMEPOS-3118 03-Aug-2022 JY Added

            int id;
            Decimal userEnterCash = 0;
            StationClose oStationClose = new StationClose();
            DataSet dsStationCloseCash = new DataSet();
            DataSet dsCurrencyList = new DataSet();
            dsStationCloseCash = oStationClose.GetStationCloseCashDetail(CloseID);
            dsCurrencyList = oStationClose.PopulateCurrencyList();
            PrintLine("\nCurrency Denominations ");
            PrintLine("\n" + Space(0) + FLen("Coin Type", 17) + FLen("Count", 14) + FLen("Amount", 10));
            PrintLine("\n" + Replicate("-", 40));
            for (int i = 0; i < dsStationCloseCash.Tables[0].Rows.Count; i++)
            {
                id = Convert.ToInt32(dsStationCloseCash.Tables[0].Rows[i]["CurrencyDenomID"]);
                id = id - 1;
                if (Configuration.convertNullToBoolean(dsCurrencyList.Tables[0].Rows[id]["IsCoin"].ToString()))
                {
                    //Added if else by shitaljit on 3 Dec 2013 to print verified cash details once the station close asch is verified by Manager.
                    if (Configuration.convertNullToBoolean(dsStationCloseCash.Tables[0].Rows[i]["IsVerified"].ToString()) == false)
                    {
                        PrintLine("\n" + Space(0) + FLen(dsCurrencyList.Tables[0].Rows[id]["CurrencyDescription"].ToString(), 20) + FLen(Configuration.convertNullToString(dsStationCloseCash.Tables[0].Rows[i]["Count"].ToString()), 9) + Configuration.convertNullToDecimal(dsStationCloseCash.Tables[0].Rows[i]["TotalValue"].ToString()).ToString(Configuration.CInfo.CurrencySymbol.ToString() + "#######0.00").PadRight(8));
                    }
                    else
                    {
                        PrintLine("\n" + Space(0) + FLen(dsCurrencyList.Tables[0].Rows[id]["CurrencyDescription"].ToString(), 20) + FLen(Configuration.convertNullToString(dsStationCloseCash.Tables[0].Rows[i]["VerifiedCount"].ToString()), 9) + Configuration.convertNullToDecimal(dsStationCloseCash.Tables[0].Rows[i]["VerifiedTotalValue"].ToString()).ToString(Configuration.CInfo.CurrencySymbol.ToString() + "#######0.00").PadRight(8));
                    }
                    //Modified by shitaljit on 3dec2013 to print verified total once manager has verified the station close 
                    if (Configuration.convertNullToBoolean(dsStationCloseCash.Tables[0].Rows[i]["IsVerified"].ToString()) == false)
                    {
                        coinTotCount += Configuration.convertNullToDecimal(dsStationCloseCash.Tables[0].Rows[i]["Count"].ToString());
                        coinTotal += Configuration.convertNullToDecimal(dsStationCloseCash.Tables[0].Rows[i]["TotalValue"].ToString());
                    }
                    else
                    {
                        coinTotCount += Configuration.convertNullToDecimal(dsStationCloseCash.Tables[0].Rows[i]["VerifiedCount"].ToString());
                        coinTotal += Configuration.convertNullToDecimal(dsStationCloseCash.Tables[0].Rows[i]["VerifiedTotalValue"].ToString());
                    }
                }
            }
            userEnterCash += coinTotal;
            PrintLine("\n\n" + Space(0) + FLen("Bill Type", 17) + FLen("Count", 14) + FLen("Amount", 10));
            PrintLine("\n" + Replicate("-", 40));
            for (int i = 0; i < dsStationCloseCash.Tables[0].Rows.Count; i++)
            {
                id = Convert.ToInt32(dsStationCloseCash.Tables[0].Rows[i]["CurrencyDenomID"]);
                id = id - 1;
                if (!Configuration.convertNullToBoolean(dsCurrencyList.Tables[0].Rows[id]["IsCoin"].ToString()))
                {
                    //Added if else by shitaljit on 3 Dec 2013 to print verified cash details once the station close asch is verified by Manager.
                    if (Configuration.convertNullToBoolean(dsStationCloseCash.Tables[0].Rows[i]["IsVerified"].ToString()) == false)
                    {
                        PrintLine("\n" + Space(0) + FLen(dsCurrencyList.Tables[0].Rows[id]["CurrencyDescription"].ToString(), 20) + FLen(Configuration.convertNullToString(dsStationCloseCash.Tables[0].Rows[i]["Count"].ToString()), 9) + Configuration.convertNullToDecimal(dsStationCloseCash.Tables[0].Rows[i]["TotalValue"].ToString()).ToString(Configuration.CInfo.CurrencySymbol.ToString() + "#######0.00").PadRight(8));
                    }
                    else
                    {
                        PrintLine("\n" + Space(0) + FLen(dsCurrencyList.Tables[0].Rows[id]["CurrencyDescription"].ToString(), 20) + FLen(Configuration.convertNullToString(dsStationCloseCash.Tables[0].Rows[i]["VerifiedCount"].ToString()), 9) + Configuration.convertNullToDecimal(dsStationCloseCash.Tables[0].Rows[i]["VerifiedTotalValue"].ToString()).ToString(Configuration.CInfo.CurrencySymbol.ToString() + "#######0.00").PadRight(8));
                    }
                    //Modified by shitaljit on 3dec2013 to print verified total once manager has verified the station close 
                    if (Configuration.convertNullToBoolean(dsStationCloseCash.Tables[0].Rows[i]["IsVerified"].ToString()) == false)
                    {
                        billTotCount += Configuration.convertNullToDecimal(dsStationCloseCash.Tables[0].Rows[i]["Count"].ToString());
                        billTotal += Configuration.convertNullToDecimal(dsStationCloseCash.Tables[0].Rows[i]["TotalValue"].ToString());
                    }
                    else
                    {
                        billTotCount += Configuration.convertNullToDecimal(dsStationCloseCash.Tables[0].Rows[i]["VerifiedCount"].ToString());
                        billTotal += Configuration.convertNullToDecimal(dsStationCloseCash.Tables[0].Rows[i]["VerifiedTotalValue"].ToString());
                    }
                }
            }
            userEnterCash += billTotal;
            PrintLine("\n" + Replicate("-", 40));
            PrintLine("\n" + FLen("Total Cash Entered: " + userEnterCash.ToString(Configuration.CInfo.CurrencySymbol.ToString() + "#######0.00"), 20));
            PrintLine("\n" + Replicate("-", 40));
            #endregion

            #region Print by Drawer Commented
            //////PrintLine("\n"+FLen("Station #"+ StationID ,20)+" Close Date:"+CloseDate);

            //////PrintLine("\n"+Replicate("-",45));

            //for (int i = 0; i < DSstClose.Tables[0].Rows.Count; i++)
            //{
            //    //PrintLine("\nDraw # " + DSstClose.Tables[0].Rows[i]["DrawNo"].ToString());

            //    PrintLine("\n" + Space(0) + FLen("Pay Type", 20) + FLen("Trans Count", 8) + FLen("Trans AMT.", 13));

            //    PrintLine("\n" + Replicate("-", 45));
            //    nTotTrans = coinTotCount + billTotCount;

            //    nTotAmount = 0;
            //    nTotTrans = 0;
            //    PrintLine("\n" + Space(0) + FLen("Cash Entered", 20) + FLen(Configuration.convertNullToString(nTotTrans.ToString()), 8) + Configuration.convertNullToDecimal(userEnterCash.ToString()).ToString("$#######0.00").PadRight(16));
            //    foreach (DataRow oRow in DSstClose.Tables[0].Rows[i].GetChildRows(RlPayment))
            //    {
            //        if (oRow["PayTypeDesc"].ToString() != "Cash")
            //        {
            //            if (oRow["PayTypeDesc"].ToString() == "Elect. Benefit Transfer")
            //                oRow["PayTypeDesc"] = "EBT";

            //            PrintLine("\n" + Space(0) + FLen(oRow["PayTypeDesc"].ToString(), 20) + FLen(Configuration.convertNullToString(oRow["TransCount"].ToString()), 8) + Configuration.convertNullToDecimal(oRow["TransAmount"].ToString()).ToString("$#######0.00").PadRight(16));
            //            nTotTrans += Configuration.convertNullToInt(oRow["TransCount"].ToString());
            //            nTotAmount += Configuration.convertNullToDecimal(oRow["TransAmount"].ToString());
            //        }
            //    }

            //    PrintLine("\n" + Replicate("-", 45));

            //    Decimal TotalCash = TotalCashByDraw(DSstClose.Tables[0].Rows[i]["DrawNo"].ToString());
            //    Decimal TotalPayout = TotalPayoutByDraw(DSstClose.Tables[0].Rows[i]["DrawNo"].ToString());

            //    //TotalCashSummary += TotalCash;
            //    TotalCashSummary += userEnterCash;
            //    TotalPayoutSummary += TotalPayout;

            //    PrintLine("\n" + Space(0) + FLen("Total", 20) + FLen(nTotTrans.ToString(),8) + nTotAmount.ToString("$########0.00").PadRight(16));
            //    //PrintLine("\n\n"+Space(0) + FLen("Total Cash:",20) + FLen(TotalCash.ToString("$########0.00"),25));
            //    PrintLine("\n\n" + Space(0) + FLen("Total Cash Entered:", 20) + FLen(userEnterCash.ToString("$########0.00"), 25));
            //    PrintLine("\n" + Space(0) + FLen("Total Payout:", 20) + FLen(TotalPayout.ToString("$########0.00"), 25));

            //    PrintLine("\n" + Replicate("-", 45));
            //    //PrintLine("\n"+Space(0) + FLen("Net Total:",20) + FLen((TotalCash-TotalPayout).ToString("$########0.00"),25));
            //    PrintLine("\n" + Space(0) + FLen("Net Total:", 20) + FLen((userEnterCash - TotalPayout).ToString("$########0.00"), 25));
            //    PrintLine("\n\n" + Space(0) + FLen("Trans Type", 20) + FLen("Trans Count", 10) + FLen("Trans AMT.", 13));

            //    PrintLine("\n" + Replicate("-", 45));
            //    nTotTrans = 0;
            //    nTotAmount = 0;

            //    foreach (DataRow oRow in DSstClose.Tables[0].Rows[i].GetChildRows(RlTransaction))
            //    {
            //        PrintLine("\n" + Space(0) + FLen(oRow["Type"].ToString(), 20) + FLen(Configuration.convertNullToString(oRow["TransCount"].ToString()), 8) + Configuration.convertNullToDecimal(oRow["TransAmount"].ToString()).ToString("$#######0.00").PadRight(16));
            //        nTotTrans += Configuration.convertNullToInt(oRow["TransCount"].ToString());
            //        nTotAmount += Configuration.convertNullToDecimal(oRow["TransAmount"].ToString());
            //    }

            //    PrintLine("\n" + Replicate("-", 45));
            //    PrintLine("\n" + Space(0) + FLen("Total", 20) + FLen(Configuration.convertNullToString(nTotTrans.ToString()), 8) + (Configuration.convertNullToDecimal(nTotAmount.ToString()).ToString("$########0.00").PadRight(16)));

            //    decimal ROATotal = 0;
            //    ROATotal = GetROATotal(CloseID.ToString(), DSstClose.Tables[0].Rows[i]["DrawNo"].ToString());
            //    TotalROASummary += ROATotal;
            //    if (ROATotal > 0)
            //    {
            //        //PrintLine("\n"+Replicate("-",45));
            //        PrintLine("\n\n" + Space(0) + FLen("ROA", 20) + FLen("", 8) + FLen(ROATotal.ToString("$########0.00"), 13));

            //        PrintLine("\n" + Space(0) + FLen("Grand Total", 20) + FLen("", 8) + FLen((nTotAmount + ROATotal).ToString("$#######0.00"), 13));
            //    }
            //}
            #endregion
            #region Print Station Close Summary

            //PrintLine("\n\n\n"+Replicate("-",40));//Commented by S
            PrintLine("\n" + "Total for the Station");

            PrintLine("\n" + Space(0) + FLen("Pay Type", 15) + FLen("Trans Count", 14) + FLen("Trans AMT.", 10));

            PrintLine("\n" + Replicate("-", 40));
            nTotAmount = 0;
            nTotTrans = 0;

            foreach (DataRow oRow in DSstCloseSummary.Tables[1].Rows)
            {
                if (oRow["PayTypeDesc"].ToString() == "Elect. Benefit Transfer")
                {
                    oRow["PayTypeDesc"] = "EBT";
                }
                PrintLine("\n" + Space(0) + FLen(oRow["PayTypeDesc"].ToString(), 17) + FLen(Configuration.convertNullToString(oRow["TransCount"].ToString()), 14) + Configuration.convertNullToDecimal(oRow["TransAmount"].ToString()).ToString(Configuration.CInfo.CurrencySymbol.ToString() + "#######0.00").PadRight(6));
                nTotTrans += Configuration.convertNullToInt(oRow["TransCount"].ToString());
                nTotAmount += Configuration.convertNullToDecimal(oRow["TransAmount"].ToString());
            }

            //Decimal TotalCash = TotalCashByDraw("1"); //PRIMEPOS-2638 07-Feb-2019 JY Commented
            //TotalCash += TotalCashByDraw("2");    //PRIMEPOS-2638 07-Feb-2019 JY Commented
            //Decimal TotalPayout = TotalPayoutByDraw("1");   //PRIMEPOS-2638 07-Feb-2019 JY Commented
            //TotalPayout += TotalPayoutByDraw("2");  //PRIMEPOS-2638 07-Feb-2019 JY Commented

            Decimal TotalCash = TotalCashByDraw();  //PRIMEPOS-2638 07-Feb-2019 JY Added
            Decimal TotalPayout = TotalPayoutByDraw();  //PRIMEPOS-2638 07-Feb-2019 JY Added

            TotalCashSummary += TotalCash;
            TotalPayoutSummary += TotalPayout;
            PrintLine("\n" + Replicate("-", 40));
            PrintLine("\n" + Space(0) + FLen("Total", 17) + FLen(nTotTrans.ToString(), 14) + nTotAmount.ToString(Configuration.CInfo.CurrencySymbol.ToString() + "########0.00").PadRight(10));
            PrintLine("\n\n" + Space(0) + FLen("Total Cash:", 17) + FLen(TotalCashSummary.ToString(Configuration.CInfo.CurrencySymbol.ToString() + "########0.00"), 25));
            PrintLine("\n" + Space(0) + FLen("Total Payout:", 17) + FLen(TotalPayoutSummary.ToString(Configuration.CInfo.CurrencySymbol.ToString() + "########0.00"), 25));

            PrintLine("\n" + Replicate("-", 40));
            PrintLine("\n" + Space(0) + FLen("Net Total:", 17) + FLen((TotalCashSummary - TotalPayoutSummary).ToString(Configuration.CInfo.CurrencySymbol.ToString() + "########0.00"), 25));

            PrintLine("\n\n" + Space(0) + FLen("Trans Type", 15) + FLen("Trans Count", 14) + FLen("Trans AMT.", 10));

            PrintLine("\n" + Replicate("-", 40));
            nTotTrans = 0;
            nTotAmount = 0;

            foreach (DataRow oRow in DSstCloseSummary.Tables[2].Rows)
            {
                if (oRow["Type"].ToString().Trim().ToUpper() == "TAX" || oRow["Type"].ToString().Trim().ToUpper() == "DISCOUNT")  //PRIMEPOS-2550 28-Jun-2018 JY Added to restrict display count
                {
                    PrintLine("\n" + Space(0) + FLen(oRow["Type"].ToString(), 17) + Space(14) + Configuration.convertNullToDecimal(oRow["TransAmount"].ToString()).ToString(Configuration.CInfo.CurrencySymbol.ToString() + "#######0.00").PadRight(6));
                }
                else
                {
                    PrintLine("\n" + Space(0) + FLen(oRow["Type"].ToString(), 17) + FLen(Configuration.convertNullToInt(oRow["TransCount"].ToString()).ToString(), 14) + Configuration.convertNullToDecimal(oRow["TransAmount"].ToString()).ToString(Configuration.CInfo.CurrencySymbol.ToString() + "#######0.00").PadRight(6));
                }

                nTotTrans += Configuration.convertNullToInt(oRow["TransCount"].ToString());
                nTotAmount += Configuration.convertNullToDecimal(oRow["TransAmount"].ToString());
            }

            PrintLine("\n" + Replicate("-", 40));
            PrintLine("\n" + Space(0) + FLen("Total", 17) + FLen(nTotTrans.ToString(), 14) + Configuration.convertNullToDecimal(nTotAmount.ToString()).ToString(Configuration.CInfo.CurrencySymbol.ToString() + "########0.00").PadRight(6));

            #region PRIMEPOS-3101 09-Jun-2022 JY Added
            TotalROASummary = GetROATotal(CloseID);
            #endregion

            TotalTransFee = GetTotalTransFee(CloseID);    //PRIMEPOS-3118 03-Aug-2022 JY Added

            if (TotalROASummary != 0 || TotalTransFee > 0)
            {
                if (TotalROASummary != 0)
                    PrintLine("\n" + Space(0) + FLen("ROA", 17) + FLen("", 14) + FLen(TotalROASummary.ToString(Configuration.CInfo.CurrencySymbol.ToString() + "########0.00"), 10));

                if (TotalTransFee > 0)
                    PrintLine("\n" + Space(0) + FLen("Transaction Fee", 17) + FLen("", 14) + FLen(TotalTransFee.ToString(Configuration.CInfo.CurrencySymbol.ToString() + "########0.00"), 10));

                PrintLine("\n" + Space(0) + FLen("Grand Total", 17) + FLen("", 14) + FLen((nTotAmount + TotalROASummary + TotalTransFee).ToString(Configuration.CInfo.CurrencySymbol.ToString() + "#######0.00"), 10));
            }
            #endregion
            PrintLine("\x1B" + "J" + Convert.ToChar(250));
            PrintLine("\x1B" + "i");

        }
        //Till here Added By Shitaljit.


        //Modified By Shitaljit(QuicSolv) on 28 Jan 2012
        //Redesigned the alignment of the receipt to fit properly to the paper
        //Set left margin to 2 
        //Set Trans Count length to 14
        //Replace PadLeft(13) with PadRight(16)
        //replicate - from 49 to 45
        private void PrintStationClose()
        {
            //PrintLine("\x1B"+"p"+"011");

            //PrintLine("\x1B"+"x0");

            this.BlankLineBeforeHeading = true;

            PrintLine(Convert.ToChar(1).ToString());
            PrintLine("\n" + FLen("CLOSE STATION REPORT", 40));
            PrintLine("\n" + FLen("Close #" + CloseID.ToString(), 19) + "  User ID:" + UserID);

            Decimal TotalCashSummary = 0;
            Decimal TotalPayoutSummary = 0;
            Decimal TotalROASummary = 0;
            Decimal nTotAmount;
            Decimal nTotTrans;
            Decimal TotalTransFee = 0;   //PRIMEPOS-3118 03-Aug-2022 JY Added

            #region Print by Drawer
            PrintLine("\n" + FLen("Station #" + StationID, 17) + "Date:" + CloseDate);  //PRIMEPOS-3114 26-Jul-2022 JY Modified

            PrintLine("\n" + Replicate("-", 40));

            for (int i = 0; i < DSstClose.Tables[0].Rows.Count; i++)
            {
                PrintLine("\nDraw # " + DSstClose.Tables[0].Rows[i]["DrawNo"].ToString());

                PrintLine("\n" + Space(0) + FLen("Pay Type", 13) + FLen("Trans Count", 16) + FLen("Trans AMT.", 10));

                PrintLine("\n" + Replicate("-", 40));

                nTotAmount = 0;
                nTotTrans = 0;

                foreach (DataRow oRow in DSstClose.Tables[0].Rows[i].GetChildRows(RlPayment))
                {
                    if (oRow["PayTypeDesc"].ToString() == "Elect. Benefit Transfer")
                    {
                        oRow["PayTypeDesc"] = "EBT";
                    }
                    PrintLine("\n" + Space(0) + FLen(oRow["PayTypeDesc"].ToString(), 17) + FLen(Configuration.convertNullToString(oRow["TransCount"].ToString()), 14) + Configuration.convertNullToDecimal(oRow["TransAmount"].ToString()).ToString(Configuration.CInfo.CurrencySymbol.ToString() + "#######0.00").PadRight(6));
                    nTotTrans += Configuration.convertNullToInt(oRow["TransCount"].ToString());
                    nTotAmount += Configuration.convertNullToDecimal(oRow["TransAmount"].ToString());
                }

                PrintLine("\n" + Replicate("-", 40));

                Decimal TotalCash = TotalCashByDraw(DSstClose.Tables[0].Rows[i]["DrawNo"].ToString());
                Decimal TotalPayout = TotalPayoutByDraw(DSstClose.Tables[0].Rows[i]["DrawNo"].ToString());

                TotalCashSummary += TotalCash;
                TotalPayoutSummary += TotalPayout;

                PrintLine("\n" + Space(0) + FLen("Total", 17) + FLen(nTotTrans.ToString(), 14) + nTotAmount.ToString(Configuration.CInfo.CurrencySymbol.ToString() + "########0.00").PadRight(6));
                PrintLine("\n\n" + Space(0) + FLen("Total Cash:", 17) + FLen(TotalCash.ToString(Configuration.CInfo.CurrencySymbol.ToString() + "########0.00"), 25));
                PrintLine("\n" + Space(0) + FLen("Total Payout:", 17) + FLen(TotalPayout.ToString(Configuration.CInfo.CurrencySymbol.ToString() + "########0.00"), 25));

                PrintLine("\n" + Replicate("-", 40));
                PrintLine("\n" + Space(0) + FLen("Net Total:", 17) + FLen((TotalCash - TotalPayout).ToString(Configuration.CInfo.CurrencySymbol.ToString() + "########0.00"), 25));
                PrintLine("\n\n" + Space(0) + FLen("Trans Type", 15) + FLen("Trans Count", 14) + FLen("Trans AMT.", 11));

                PrintLine("\n" + Replicate("-", 40));
                nTotTrans = 0;
                nTotAmount = 0;

                foreach (DataRow oRow in DSstClose.Tables[0].Rows[i].GetChildRows(RlTransaction))
                {
                    if (oRow["Type"].ToString().Trim().ToUpper() == "TAX" || oRow["Type"].ToString().Trim().ToUpper() == "DISCOUNT")  //PRIMEPOS-2550 28-Jun-2018 JY Added to restrict display count
                    {
                        PrintLine("\n" + Space(0) + FLen(oRow["Type"].ToString(), 17) + Space(14) + Configuration.convertNullToDecimal(oRow["TransAmount"].ToString()).ToString(Configuration.CInfo.CurrencySymbol.ToString() + "#######0.00").PadRight(6));
                    }
                    else
                    {
                        PrintLine("\n" + Space(0) + FLen(oRow["Type"].ToString(), 17) + FLen(Configuration.convertNullToString(oRow["TransCount"].ToString()), 14) + Configuration.convertNullToDecimal(oRow["TransAmount"].ToString()).ToString(Configuration.CInfo.CurrencySymbol.ToString() + "#######0.00").PadRight(6));
                    }

                    nTotTrans += Configuration.convertNullToInt(oRow["TransCount"].ToString());
                    nTotAmount += Configuration.convertNullToDecimal(oRow["TransAmount"].ToString());
                }

                PrintLine("\n" + Replicate("-", 40));
                PrintLine("\n" + Space(0) + FLen("Total", 17) + FLen(Configuration.convertNullToString(nTotTrans.ToString()), 14) + (Configuration.convertNullToDecimal(nTotAmount.ToString()).ToString(Configuration.CInfo.CurrencySymbol.ToString() + "########0.00").PadRight(6)));

                decimal ROATotal = 0;
                ROATotal = GetROATotal(CloseID.ToString(), DSstClose.Tables[0].Rows[i]["DrawNo"].ToString());
                TotalROASummary += ROATotal;

                decimal TransFee = 0;
                TransFee = GetTotalTransFee(CloseID.ToString(), DSstClose.Tables[0].Rows[i]["DrawNo"].ToString());
                TotalTransFee += TransFee;

                if (ROATotal != 0 || TransFee > 0)
                {
                    //PrintLine("\n"+Replicate("-",40));
                    if (ROATotal != 0)
                        PrintLine("\n\n" + Space(0) + FLen("ROA", 17) + FLen("", 14) + FLen(ROATotal.ToString(Configuration.CInfo.CurrencySymbol.ToString() + "########0.00"), 6));
                    if (TransFee > 0)
                        PrintLine("\n\n" + Space(0) + FLen("Transaction Fee", 17) + FLen("", 14) + FLen(TransFee.ToString(Configuration.CInfo.CurrencySymbol.ToString() + "########0.00"), 6));

                    PrintLine("\n" + Space(0) + FLen("Grand Total", 17) + FLen("", 14) + FLen((nTotAmount + ROATotal + TransFee).ToString(Configuration.CInfo.CurrencySymbol.ToString() + "#######0.00"), 6));
                }
            }
            #endregion
            #region Print Station Close Summary

            //PrintLine("\n\n\n"+Replicate("-",40)); //Commented by Shitaljit to reduce the space.s
            PrintLine("\n" + "Total for the Station");

            PrintLine("\n" + Space(0) + FLen("Pay Type", 15) + FLen("Trans Count", 14) + FLen("Trans AMT.", 10));

            PrintLine("\n" + Replicate("-", 40));
            nTotAmount = 0;
            nTotTrans = 0;

            foreach (DataRow oRow in DSstCloseSummary.Tables[1].Rows)
            {
                if (oRow["PayTypeDesc"].ToString() == "Elect. Benefit Transfer")
                {
                    oRow["PayTypeDesc"] = "EBT";
                }
                PrintLine("\n" + Space(0) + FLen(oRow["PayTypeDesc"].ToString(), 17) + FLen(Configuration.convertNullToString(oRow["TransCount"].ToString()), 14) + Configuration.convertNullToDecimal(oRow["TransAmount"].ToString()).ToString(Configuration.CInfo.CurrencySymbol.ToString() + "#######0.00").PadRight(6));
                nTotTrans += Configuration.convertNullToInt(oRow["TransCount"].ToString());
                nTotAmount += Configuration.convertNullToDecimal(oRow["TransAmount"].ToString());
            }

            PrintLine("\n" + Replicate("-", 40));
            PrintLine("\n" + Space(0) + FLen("Total", 17) + FLen(nTotTrans.ToString(), 14) + nTotAmount.ToString(Configuration.CInfo.CurrencySymbol.ToString() + "########0.00").PadRight(6));
            PrintLine("\n\n" + Space(0) + FLen("Total Cash:", 17) + FLen(TotalCashSummary.ToString(Configuration.CInfo.CurrencySymbol.ToString() + "########0.00"), 25));
            PrintLine("\n" + Space(0) + FLen("Total Payout:", 17) + FLen(TotalPayoutSummary.ToString(Configuration.CInfo.CurrencySymbol.ToString() + "########0.00"), 25));

            PrintLine("\n" + Replicate("-", 40));
            PrintLine("\n" + Space(0) + FLen("Net Total:", 17) + FLen((TotalCashSummary - TotalPayoutSummary).ToString(Configuration.CInfo.CurrencySymbol.ToString() + "########0.00"), 25));

            PrintLine("\n\n" + Space(0) + FLen("Trans Type", 15) + FLen("Trans Count", 15) + FLen("Trans AMT.", 10));

            PrintLine("\n" + Replicate("-", 40));
            nTotTrans = 0;
            nTotAmount = 0;

            foreach (DataRow oRow in DSstCloseSummary.Tables[2].Rows)
            {
                if (oRow["Type"].ToString().Trim().ToUpper() == "TAX" || oRow["Type"].ToString().Trim().ToUpper() == "DISCOUNT")  //PRIMEPOS-2550 28-Jun-2018 JY Added to restrict display count
                {
                    PrintLine("\n" + Space(0) + FLen(oRow["Type"].ToString(), 17) + Space(14) + Configuration.convertNullToDecimal(oRow["TransAmount"].ToString()).ToString(Configuration.CInfo.CurrencySymbol.ToString() + "#######0.00").PadRight(6));
                }
                else
                {
                    PrintLine("\n" + Space(0) + FLen(oRow["Type"].ToString(), 17) + FLen(Configuration.convertNullToInt(oRow["TransCount"].ToString()).ToString(), 14) + Configuration.convertNullToDecimal(oRow["TransAmount"].ToString()).ToString(Configuration.CInfo.CurrencySymbol.ToString() + "#######0.00").PadRight(6));
                }

                nTotTrans += Configuration.convertNullToInt(oRow["TransCount"].ToString());
                nTotAmount += Configuration.convertNullToDecimal(oRow["TransAmount"].ToString());
            }

            PrintLine("\n" + Replicate("-", 40));
            PrintLine("\n" + Space(0) + FLen("Total", 17) + FLen(nTotTrans.ToString(), 14) + Configuration.convertNullToDecimal(nTotAmount.ToString()).ToString(Configuration.CInfo.CurrencySymbol.ToString() + "########0.00").PadRight(6));


            if (TotalROASummary != 0 || TotalTransFee > 0)   //PRIMEPOS-3118 03-Aug-2022 JY added TotalTransFee
            {
                if (TotalROASummary != 0)
                    PrintLine("\n" + Space(0) + FLen("ROA", 17) + FLen("", 14) + FLen(TotalROASummary.ToString(Configuration.CInfo.CurrencySymbol.ToString() + "########0.00"), 6));
                if (TotalTransFee > 0)
                    PrintLine("\n" + Space(0) + FLen("Transaction Fee", 17) + FLen("", 14) + FLen(TotalTransFee.ToString(Configuration.CInfo.CurrencySymbol.ToString() + "########0.00"), 6));

                PrintLine("\n" + Space(0) + FLen("Grand Total", 17) + FLen("", 14) + FLen((nTotAmount + TotalROASummary + TotalTransFee).ToString(Configuration.CInfo.CurrencySymbol.ToString() + "#######0.00"), 6));
            }
            #endregion
            PrintLine("\x1B" + "J" + Convert.ToChar(250));
            PrintLine("\x1B" + "i");
        }

        //PRIMEPOS-2638 07-Feb-2019 JY modified sql
        private decimal GetROATotal(String CloseID, String DrawNo)
        {
            Search oSearch = new Search();
            string strSQL = "SELECT ISNULL(SUM(ISNULL(TransAmount,0)),0) FROM StationCloseDetail WHERE TransType = 'A' AND DrawNo = " + DrawNo + " AND StationCloseID = " + CloseID;
            DataSet ds = oSearch.SearchData(strSQL);
            return Configuration.convertNullToDecimal(ds.Tables[0].Rows[0][0].ToString());
        }

        #region PRIMEPOS-3101 09-Jun-2022 JY Added
        private decimal GetROATotal(int CloseID)
        {
            Search oSearch = new Search();
            string strSQL = "SELECT ISNULL(SUM(ISNULL(TransAmount,0)),0) FROM StationCloseDetail WHERE TransType = 'A' AND StationCloseID = " + CloseID;
            DataSet ds = oSearch.SearchData(strSQL);
            return Configuration.convertNullToDecimal(ds.Tables[0].Rows[0][0].ToString());
        }
        #endregion

        #region PRIMEPOS-3118 03-Aug-2022 JY Added
        private decimal GetTotalTransFee(int CloseID)
        {
            Search oSearch = new Search();
            string strSQL = "SELECT ISNULL(SUM(ISNULL(TransAmount,0)),0) FROM StationCloseDetail WHERE TransType = 'TF' AND StationCloseID = " + CloseID;
            DataSet ds = oSearch.SearchData(strSQL);
            return Configuration.convertNullToDecimal(ds.Tables[0].Rows[0][0].ToString());
        }

        private decimal GetTotalTransFee(String CloseID, String DrawNo)
        {
            Search oSearch = new Search();
            string strSQL = "SELECT ISNULL(SUM(ISNULL(TransAmount,0)),0) FROM StationCloseDetail WHERE TransType = 'TF' AND DrawNo = " + DrawNo + " AND StationCloseID = " + CloseID;
            DataSet ds = oSearch.SearchData(strSQL);
            return Configuration.convertNullToDecimal(ds.Tables[0].Rows[0][0].ToString());
        }
        #endregion

        private bool SetPrinter()
        {
            bool PrinterSet = true;
            if (Configuration.CPOSSet.RP_Name.Trim().Length > 0)
            {
                mPD.PrinterSettings.PrinterName = Configuration.CPOSSet.RP_Name.Trim();
                #region PRIMEPOS-2996 23-Sep-2021 JY Added
                try
                {
                    if (Configuration.CPOSSet.ReceiptPrinterPaperSource.Trim() != "")
                    {
                        System.Drawing.Printing.PrinterSettings oPrinterSettings = new System.Drawing.Printing.PrinterSettings();
                        foreach (System.Drawing.Printing.PaperSource ps in mPD.PrinterSettings.PaperSources)
                        {
                            if (ps.SourceName.Trim().ToUpper() == Configuration.CPOSSet.ReceiptPrinterPaperSource.Trim().ToUpper())
                            {
                                mPD.DefaultPageSettings.PaperSource = ps;
                                break;
                            }
                        }
                    }
                }
                catch { }
                #endregion
                /*
                if (!mPD.PrinterSettings.IsValid)
                {
                    if (PrinterSettings.InstalledPrinters.Count > 0)
                        mPD.PrinterSettings.PrinterName = PrinterSettings.InstalledPrinters[0];
                }
                if (!mPD.PrinterSettings.IsValid)
                    PrinterSet=false;
                */
            }
            else
                PrinterSet = false;
            return PrinterSet;
        }

        private void SetFont(System.Drawing.Font font, long Size, FontStyle Fs)
        {
            this.CFont = new System.Drawing.Font(font.Name, Size, Fs);
        }

        private void SetFont(System.Drawing.Font font, FontStyle Fs)
        {
            this.CFont = new System.Drawing.Font(font, Fs);
        }

        public bool BlankLineBeforeHeading
        {
            set { this.InsertBlankLine = value; }
            get { return this.InsertBlankLine; }
        }
    }
}
