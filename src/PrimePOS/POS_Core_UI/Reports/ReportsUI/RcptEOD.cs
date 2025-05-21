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
using POS_Core.CommonData;
using POS_Core.DataAccess;
//using POS.UI;
//using POS_Core.DataAccess;
using System.Data;
using System.Runtime.InteropServices;
//using POS_Core_UI.Reports.ReportsUI;
using POS_Core.CommonData.Rows;
//using Phw;
using POS_Core.Resources;
using POS_Core.LabelHandler;
using POS_Core.LabelHandler.RxLabel;

namespace POS_Core_UI.Reports.ReportsUI
{
    /// <summary>
    /// Summary description for RcptEOD.
    /// </summary>
    public class RcptEOD
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
        public string CloseDate;
        public Decimal TotalPayout;
        public Decimal TotalCash;
        public DataSet DSEODClose;

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

        public string FLen(string Data, int lLen)
        {
            /*string str;
            str=Data;
            for(int i=1; i<=(Len-Data.Length);i++)
                str+=" ";
            return str;
            old logic
            */
            //New logic as per primepos
            if (Data == null)
            {
                return Space(lLen);
            }
            else
            {

                if (Data.Length >= lLen)
                    return Data.Substring(0, lLen);
                else
                {
                    string sD = Data + Space(lLen);
                    return sD.Substring(0, lLen);
                }
            }
        }

        public string Space(int Spaces)
        {
            string str = "";
            for (int i = 1; i <= Spaces; i++)
                str += " ";
            return str;
        }

        public RcptEOD(int StCloseID)
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
        }

        private void PrintEOD()
        {
            //Modified By Shitaljit(QuicSolv) on 7 July 2011
            //Redesigned the alignment of the receipt to fit properly to the paper
            //Set left margin to 0 
            //Set Trans Count length to 16
            //Set Trans count to 16
            //Replace PadLeft(13) with PadRight(14)
            //replicate - from 49 to 40

            PrintLine("\x1B" + "x0");

            this.BlankLineBeforeHeading = true;

            PrintLine(Convert.ToChar(1).ToString());

            //PrintLine("\n" + FLen("END OF DAY REPORT FOR " + DSEODClose.Tables[0].Rows[0]["CloseDate"].ToString(), 40));  //PRIMEPOS-3114 26-Jul-2022 JY Commented
            PrintLine("\n" + FLen("END OF DAY REPORT FOR " + this.CloseDate, 45));  //PRIMEPOS-3114 26-Jul-2022 JY Added
            PrintLine("\n" + FLen("GRAND TOTAL", 40));
            PrintLine("\n" + FLen("EOD #" + CloseID.ToString(), 17));

            PrintLine("\n" + Replicate("-", 40));
            PrintLine("\n" + Space(0) + FLen("Pay Type", 13) + FLen("Trans Count", 18) + FLen("Trans AMT.", 12));
            PrintLine("\n" + Replicate("-", 40));

            decimal nTotAmount;
            int nTotTrans;

            nTotAmount = 0;
            nTotTrans = 0;

            foreach (DataRow oRow1 in DSEODClose.Tables[1].Rows)
            {
                if (oRow1["PayTypeDesc"].ToString() == "Elect. Benefit Transfer")
                {
                    oRow1["PayTypeDesc"] = "EBT";
                }
                PrintLine("\n" + Space(0) + FLen(oRow1["PayTypeDesc"].ToString(), 17) + FLen(Configuration.convertNullToString(oRow1["TransCount"].ToString()), 14));
                nTotTrans += Configuration.convertNullToInt(oRow1["TransCount"].ToString());
                PrintLine(Configuration.convertNullToDecimal(oRow1["TransAmount"].ToString()).ToString(Configuration.CInfo.CurrencySymbol.ToString() + "#######0.00").PadRight(10));
                nTotAmount += Configuration.convertNullToDecimal(oRow1["TransAmount"].ToString());
            }

            PrintLine("\n" + Replicate("-", 40));
            PrintLine("\n" + Space(0) + FLen("Total", 17) + FLen(nTotTrans.ToString(), 14) + nTotAmount.ToString(Configuration.CInfo.CurrencySymbol.ToString() + "########0.00").PadRight(10));

            PrintLine("\n\n" + Space(0) + FLen("Total Cash:", 15) + FLen(TotalCash.ToString(Configuration.CInfo.CurrencySymbol.ToString() + "########0.00"), 25));

            PrintLine("\n" + Space(0) + FLen("Total Payout:", 15) + FLen(TotalPayout.ToString(Configuration.CInfo.CurrencySymbol.ToString() + "########0.00"), 25));

            PrintLine("\n" + Replicate("-", 40));
            PrintLine("\n" + Space(0) + FLen("Net Total:", 15) + FLen((TotalCash - TotalPayout).ToString(Configuration.CInfo.CurrencySymbol.ToString() + "########0.00"), 25));

            PrintLine("\n\n" + Space(0) + FLen("Trans Type", 13) + FLen("Trans Count", 16) + FLen("Trans AMT.", 11));
            PrintLine("\n" + Replicate("-", 40));

            nTotTrans = 0;
            nTotAmount = 0;

            DataRow oRow = DSEODClose.Tables[0].Rows[0];

            PrintLine("\n" + Space(0) + FLen("Sale", 17) + FLen(Configuration.convertNullToString(oRow["totalsalecount"].ToString()), 14) + Configuration.convertNullToDecimal(oRow["totalsales"].ToString()).ToString(Configuration.CInfo.CurrencySymbol.ToString() + "#######0.00").PadRight(8));

            nTotTrans += Configuration.convertNullToInt(oRow["totalsaleCount"].ToString());
            nTotAmount += Configuration.convertNullToDecimal(oRow["totalsales"].ToString());

            PrintLine("\n" + Space(0) + FLen("Return", 17) + FLen(Configuration.convertNullToString(oRow["totalreturncount"].ToString()), 14) + Configuration.convertNullToDecimal(oRow["totalreturns"].ToString()).ToString(Configuration.CInfo.CurrencySymbol.ToString() + "#######0.00").PadRight(8));
            nTotTrans += Configuration.convertNullToInt(oRow["TotalreturnCount"].ToString());
            nTotAmount += Configuration.convertNullToDecimal(oRow["totalreturns"].ToString());


            PrintLine("\n" + Space(0) + FLen("Discount", 17) + FLen(Configuration.convertNullToString(oRow["totalDiscountcount"].ToString()), 14) + Configuration.convertNullToDecimal(oRow["totaldiscount"].ToString()).ToString(Configuration.CInfo.CurrencySymbol.ToString() + "#######0.00").PadRight(8));
            nTotTrans += Configuration.convertNullToInt(oRow["TotaldiscountCount"].ToString());
            nTotAmount -= Configuration.convertNullToDecimal(oRow["totaldiscount"].ToString());

            PrintLine("\n" + Space(0) + FLen("Tax", 17) + FLen(Configuration.convertNullToString(oRow["totaltaxcount"].ToString()), 14) + Configuration.convertNullToDecimal(oRow["totaltax"].ToString()).ToString(Configuration.CInfo.CurrencySymbol.ToString() + "#######0.00").PadRight(6));
            nTotTrans += Configuration.convertNullToInt(oRow["TotaltaxCount"].ToString());
            nTotAmount += Configuration.convertNullToDecimal(oRow["totaltax"].ToString());

            PrintLine("\n" + Replicate("-", 40));
            PrintLine("\n" + Space(0) + FLen("Total", 17) + FLen(nTotTrans.ToString(), 14) + nTotAmount.ToString(Configuration.CInfo.CurrencySymbol.ToString() + "#######0.00").PadRight(15));

            if (Configuration.convertNullToDecimal(oRow["TotalROA"].ToString()) != 0 || Configuration.convertNullToDecimal(oRow["TotalTransFee"].ToString()) > 0)    //PRIMEPOS-3118 03-Aug-2022 JY Added TotalTransFee
            {
                PrintLine("\n" + Replicate("-", 40));
                if (Configuration.convertNullToDecimal(oRow["TotalROA"].ToString()) != 0)
                {
                    PrintLine("\n" + Space(0) + FLen("ROA", 17) + FLen("", 16) + FLen(Configuration.convertNullToDecimal(oRow["TotalROA"].ToString()).ToString(Configuration.CInfo.CurrencySymbol.ToString() + "########0.00"), 15));
                }
                if (Configuration.convertNullToDecimal(oRow["TotalTransFee"].ToString()) > 0)
                {
                    PrintLine("\n" + Space(0) + FLen("Transaction Fee", 17) + FLen("", 16) + FLen(Configuration.convertNullToDecimal(oRow["TotalTransFee"].ToString()).ToString(Configuration.CInfo.CurrencySymbol.ToString() + "########0.00"), 15));
                }
                PrintLine("\n" + Space(0) + FLen("Grand Total", 17) + FLen("", 16) + FLen((nTotAmount + Configuration.convertNullToDecimal(oRow["TotalROA"].ToString()) + Configuration.convertNullToDecimal(oRow["TotalTransFee"].ToString())).ToString(Configuration.CInfo.CurrencySymbol.ToString() + "#######0.00"), 15));
            }

            PrintLine("\n");

            #region Added By Shitaljit for JIRA Ticket 286 to add RX , Taxable , Non-Taxable Item sales break down in EOD Total Report.

            string strRxTotal = "0";
            string strRXItemCount = "0";
            string strTaxableItemTotal = "0";
            string strTaxableItemCount = "0";
            string strNonTaxableTotal = "0";
            string strNonTaxableCount = "0";
            string strTotalWithoutTax = "0";
            string strTotalWithTax = "0";
            POS_Core.BusinessRules.EndOFDay oEndOFDay = new POS_Core.BusinessRules.EndOFDay();
            PrintLine("\n\n" + Space(0) + FLen("Trans Type", 18) + FLen("Item Count", 13) + FLen("Trans AMT.", 10));

            PrintLine("\n" + Replicate("-", 40));
            DataSet dsRXitem = oEndOFDay.GetRxSalesDetails(CloseID);
            if (dsRXitem != null)
            {
                strRXItemCount = Configuration.convertNullToDecimal(dsRXitem.Tables[0].Rows[0][0].ToString()).ToString();
                strRxTotal = Configuration.convertNullToDecimal(dsRXitem.Tables[0].Rows[0][1].ToString()).ToString("######0.00");
            }
            PrintLine("\n" + Space(0) + FLen("Prescriptions Sales", 22) + FLen(Configuration.convertNullToString(strRXItemCount), 10) + Configuration.convertNullToDecimal(strRxTotal).ToString(Configuration.CInfo.CurrencySymbol.ToString() + "#######0.00").PadRight(4));
            DataSet DsTaxableItem = oEndOFDay.GetTaxableItemSalesDetails(CloseID);
            if (DsTaxableItem != null)
            {
                strTaxableItemCount = Configuration.convertNullToDecimal(DsTaxableItem.Tables[0].Rows[0][0].ToString()).ToString();
                strTaxableItemTotal = Configuration.convertNullToDecimal(DsTaxableItem.Tables[0].Rows[0][1].ToString()).ToString("######0.00");
            }
            PrintLine("\n" + Space(0) + FLen("Net Taxable Sales", 22) + FLen(Configuration.convertNullToString(strTaxableItemCount.ToString()), 10) + Configuration.convertNullToDecimal(strTaxableItemTotal).ToString(Configuration.CInfo.CurrencySymbol.ToString() + "#######0.00").PadRight(4));

            DataSet DsNonTaxableItem = oEndOFDay.GetNonTaxableItemsSalesDetails(CloseID);
            if (DsNonTaxableItem != null)
            {
                strNonTaxableCount = Configuration.convertNullToDecimal(DsNonTaxableItem.Tables[0].Rows[0][0].ToString()).ToString();
                strNonTaxableTotal = Configuration.convertNullToDecimal(DsNonTaxableItem.Tables[0].Rows[0][1].ToString()).ToString("######0.00");
            }
            PrintLine("\n" + Space(0) + FLen("Net Non-Taxable Sales", 22) + FLen(Configuration.convertNullToString(strNonTaxableCount.ToString()), 10) + Configuration.convertNullToDecimal(strNonTaxableTotal).ToString(Configuration.CInfo.CurrencySymbol.ToString() + "#######0.00").PadRight(4));


            strTotalWithoutTax = (Configuration.convertNullToDecimal(strRxTotal) + Configuration.convertNullToDecimal(strNonTaxableTotal) + Configuration.convertNullToDecimal(strTaxableItemTotal)).ToString("######0.00");
            strTotalWithTax = (Configuration.convertNullToDecimal(strTotalWithoutTax) + Configuration.convertNullToDecimal(oRow["totaltax"].ToString())).ToString("######0.00");
            PrintLine("\n" + Replicate("-", 40));

            PrintLine("\n" + Space(0) + FLen("Total Sales:(Without Tax)", 25) + FLen("", 7) + FLen((Configuration.convertNullToDecimal(strTotalWithoutTax)).ToString(Configuration.CInfo.CurrencySymbol.ToString() + "#######0.00"), 10));
            PrintLine("\n" + Space(0) + FLen("Total Sales: (With Tax)", 25) + FLen("", 7) + FLen((Configuration.convertNullToDecimal(strTotalWithTax)).ToString(Configuration.CInfo.CurrencySymbol.ToString() + "#######0.00"), 10));
            PrintLine("\n" + Space(0) + FLen("Sale Tax Amtount", 25) + FLen("", 7) + FLen((Configuration.convertNullToDecimal(oRow["totaltax"].ToString())).ToString(Configuration.CInfo.CurrencySymbol.ToString() + "#######0.00"), 10));
            PrintLine("\n\n\n");
            #endregion
            //PrintLine("\x1B"+"J"+Convert.ToChar(250));
            //PrintLine("\x1B"+"i");
        }

        private void initReportData()
        {
            DSEODClose = new DataSet();
            DataSet oDS = new DataSet();
            Search oSearch = new Search();
            oDS = oSearch.SearchData("SELECT  *, " +
                      " isnull((SELECT SUM(transcount) FROM stationclosedetail scd, stationcloseheader sch WHERE sch.stationcloseid = scd.stationcloseid AND sch.eodid = h.eodid AND scd.transtype = 'S'),0) AS TotalSalecount, " +
                      " isnull((SELECT SUM(transcount) FROM stationclosedetail scd, stationcloseheader sch WHERE sch.stationcloseid = scd.stationcloseid AND sch.eodid = h.eodid AND scd.transtype = 'SR'),0) AS TotalReturnCount, " +
                      " isnull((SELECT SUM(transcount) FROM stationclosedetail scd, stationcloseheader sch WHERE sch.stationcloseid = scd.stationcloseid AND sch.eodid = h.eodid AND scd.transtype = 'DT'),0) AS TotalDiscountCount, " +
                      " isnull((SELECT SUM(transcount) FROM stationclosedetail scd, stationcloseheader sch WHERE sch.stationcloseid = scd.stationcloseid AND sch.eodid = h.eodid AND scd.transtype = 'TX'),0) AS TotalTaxCount, " +
                      " isnull((SELECT SUM(transcount) FROM stationclosedetail scd, stationcloseheader sch WHERE sch.stationcloseid = scd.stationcloseid AND sch.eodid = h.eodid AND scd.transtype = 'A'),0) AS TotalROACount, " +
                      " isnull((SELECT SUM(transcount) FROM stationclosedetail scd, stationcloseheader sch WHERE sch.stationcloseid = scd.stationcloseid AND sch.eodid = h.eodid AND scd.transtype = 'TF'),0) AS TotalTransFeeCount " + //PRIMEPOS-3118 03-Aug-2022 JY Added
                      "	FROM EndOfDayHeader h WHERE EODID =" + CloseID);

            DSEODClose.Tables.Add(oDS.Tables[0].Copy());
            DSEODClose.Tables[0].TableName = "masterTable";

            oDS = new DataSet();
            oDS = oSearch.SearchData(" SELECT pt.PayTypeDesc, ed.Amount as TransAmount, SUM(scd.TransCount) AS TransCount " +
                    " FROM EndOfDayDetail ed INNER JOIN PayType pt ON ed.PayTypeID = pt.PayTypeID INNER JOIN " +
                    " StationCloseHeader sc ON ed.EODID = sc.EODID INNER JOIN " +
                    " StationCloseDetail scd ON sc.StationCloseID = scd.StationCloseID AND ed.PayTypeID = SUBSTRING(scd.TransType, 3, len(scd.TransType)) " +
                    " WHERE ed.EODID = " + CloseID + " GROUP BY pt.PayTypeDesc, ed.Amount ");


            DSEODClose.Tables.Add(oDS.Tables[0].Copy());
            DSEODClose.Tables[1].TableName = "payment";


            if (DSEODClose.Tables[0].Rows.Count > 0)
            {
                //this.CloseDate = Convert.ToDateTime(DSEODClose.Tables[0].Rows[0]["CloseDate"].ToString()).ToShortDateString();    //PRIMEPOS-3114 26-Jul-2022 JY Commented
                this.CloseDate = Convert.ToDateTime(DSEODClose.Tables[0].Rows[0]["CloseDate"].ToString()).ToString("MM/dd/yyyy HH:mm:ss");   //PRIMEPOS-3114 26-Jul-2022 JY Added
            }

            oDS = new DataSet();
            oDS = oSearch.SearchData("select isnull((SELECT sum(isnull(transamount,0)) FROM stationclosedetail sch, stationcloseheader sc WHERE sc.stationcloseid=sch.stationcloseid and (sc.EODID = " + CloseID + " and transtype='PO')),0) ");
            this.TotalPayout = Configuration.convertNullToDecimal(oDS.Tables[0].Rows[0][0].ToString());

            oDS = new DataSet();
            oDS = oSearch.SearchData("select isnull((SELECT sum(isnull(transamount,0)) FROM stationclosedetail sch, stationcloseheader sc WHERE sc.stationcloseid=sch.stationcloseid and (sc.EODID = " + CloseID + " and transtype='U-1')),0) ");
            this.TotalCash = Configuration.convertNullToDecimal(oDS.Tables[0].Rows[0][0].ToString());
        }
        public void PrintLine(string LineToPrint)
        {
            int pcWritten = 0;
            PrintDirectAPIs.WritePrinter(lhPrinter, LineToPrint, LineToPrint.Length, ref pcWritten);
            return;
        }

        public void Print()
        {
            // this routine prints the dot matrix label
            //				string sLabelFile;
            //				sLabelFile="rcptEOD.js";
            //
            //				if (System.IO.File.Exists(sLabelFile))
            //				{
            this.lhPrinter = new System.IntPtr();

            DOCINFO di = new DOCINFO();
            di.pDocName = "Label";
            di.pDataType = "RAW";

            // the \x1b means an ascii escape character
            //st1="\x1b*c600a6b0P\f";
            //lhPrinter contains the handle for the printer opened
            //If lhPrinter is 0 then an error has occured
            //bool bSuccess=false;
            if (Configuration.CPOSSet.RP_Name.Trim() == "")
            {
                //bSuccess=true;
            }
            else if (PrintDirectAPIs.OpenPrinter(Configuration.CPOSSet.RP_Name.Trim(), ref lhPrinter, 0))
            {
                if (PrintDirectAPIs.StartDocPrinter(lhPrinter, 1, ref di))
                {
                    if (PrintDirectAPIs.StartPagePrinter(lhPrinter))
                    {

                        //ScriptableLabel _lbl = new ScriptableLabel(this,sLabelFile);

                        try
                        {
                            PrintEOD();
                            //for(int i=0; i < Configuration.CInfo.NoOfReceipt; i++)
                            //{
                            //_lbl.PrintCLabel();   // this will actually send the label printing lines
                            //}
                            //	bSuccess=true;
                        }
                        catch (Exception e)
                        {
                            clsUIHelper.ShowErrorMsg(e.Message);

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
            //					return;
            //				}
        }

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


    public class RcptEODByStation
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
        public string CloseDate;

        public DataSet DSEODClose;
        public DataSet DSEODDepartment;
        public DataRelation RlPayment;
        public DataRelation RlTransaction;


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

        public string FLen(string Data, int lLen)
        {
            /*string str;
			str=Data;
			for(int i=1; i<=(Len-Data.Length);i++)
				str+=" ";
			return str;
			old logic
			*/
            //New logic as per primepos
            if (Data == null)
            {
                return Space(lLen);
            }
            else
            {

                if (Data.Length >= lLen)
                    return Data.Substring(0, lLen);
                else
                {
                    string sD = Data + Space(lLen);
                    return sD.Substring(0, lLen);
                }
            }
        }

        public string Space(int Spaces)
        {
            string str = "";
            for (int i = 1; i <= Spaces; i++)
                str += " ";
            return str;
        }

        public RcptEODByStation(int StCloseID)
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
        }

        private void initReportData()
        {
            DSEODClose = new DataSet();
            DataSet oDS = new DataSet();
            Search oSearch = new Search();
            oDS = oSearch.SearchData("SELECT DISTINCT ed.*, sch.StationID FROM EndOfDayHeader ed INNER JOIN " +
                      " StationCloseHeader sch ON ed.EODID = sch.EODID INNER JOIN " +
                      " StationCloseDetail scd ON sch.StationCloseID = scd.StationCloseID " +
                      "	WHERE sch.EODID = " + CloseID);

            DSEODClose.Tables.Add(oDS.Tables[0].Copy());
            DSEODClose.Tables[0].TableName = "masterTable";

            oDS = new DataSet();
            oDS = oSearch.SearchData(" SELECT sch.StationID, pt.PayTypeDesc, SUM(scd.TransCount) AS TransCount, SUM(scd.TransAmount) AS TransAmount " +
                    " FROM StationCloseHeader sch INNER JOIN StationCloseDetail scd ON sch.StationCloseID = scd.StationCloseID INNER JOIN " +
                    " PayType pt ON SUBSTRING(scd.TransType, 3, len(scd.TransType)) = pt.PayTypeID WHERE sch.EODID =" + CloseID.ToString() +
                    " GROUP BY sch.StationID, pt.PayTypeDesc, pt.PayTypeID ORDER BY sch.StationID, pt.PayTypeID");  //PRIMEPOS-2983 02-Jul-2021 JY Modified

            DSEODClose.Tables.Add(oDS.Tables[0].Copy());
            DSEODClose.Tables[1].TableName = "payment";

            oDS = new DataSet();
            oDS = oSearch.SearchData(" SELECT SCH.StationID, 'Sales' AS Type, SUM(SCD.TransCount) AS TransCount, SUM(SCD.TransAmount) AS TransAmount " +
                    " FROM StationCloseDetail SCD INNER JOIN StationCloseHeader SCH ON SCD.StationCloseID = SCH.StationCloseID WHERE  (SCD.TransType = 'S') AND SCH.EODID = " + CloseID.ToString() + " GROUP BY SCH.StationID " +
                " union all " +
                " SELECT SCH.StationID, 'Returns' AS Type, SUM(SCD.TransCount) AS TransCount, SUM(SCD.TransAmount) AS TransAmount " +
                " FROM StationCloseDetail SCD INNER JOIN StationCloseHeader SCH ON SCD.StationCloseID = SCH.StationCloseID WHERE  (SCD.TransType = 'SR') AND SCH.EODID = " + CloseID.ToString() + " GROUP BY SCH.StationID " +
                " union all " +
                " SELECT SCH.StationID, 'Tax' AS Type, SUM(SCD.TransCount) AS TransCount, SUM(SCD.TransAmount) AS TransAmount " +
                " FROM StationCloseDetail SCD INNER JOIN StationCloseHeader SCH ON SCD.StationCloseID = SCH.StationCloseID WHERE  (SCD.TransType = 'TX') AND SCH.EODID = " + CloseID.ToString() + " GROUP BY SCH.StationID " +
                " union all " +
                " SELECT SCH.StationID, 'Discount' AS Type, SUM(SCD.TransCount) AS TransCount, -1*SUM(SCD.TransAmount) AS TransAmount " +
                " FROM StationCloseDetail SCD INNER JOIN StationCloseHeader SCH ON SCD.StationCloseID = SCH.StationCloseID WHERE  (SCD.TransType = 'DT') AND SCH.EODID = " + CloseID.ToString() + " GROUP BY SCH.StationID ");
            DSEODClose.Tables.Add(oDS.Tables[0].Copy());
            DSEODClose.Tables[2].TableName = "transaction";

            RlPayment = DSEODClose.Relations.Add(DSEODClose.Tables[0].Columns["stationid"], DSEODClose.Tables[1].Columns["stationid"]);
            RlTransaction = DSEODClose.Relations.Add(DSEODClose.Tables[0].Columns["stationid"], DSEODClose.Tables[2].Columns["stationid"]);

            if (DSEODClose.Tables[0].Rows.Count > 0)
            {
                //this.CloseDate = Convert.ToDateTime(DSEODClose.Tables[0].Rows[0]["CloseDate"].ToString()).ToShortDateString();    //PRIMEPOS-3114 26-Jul-2022 JY Commented
                this.CloseDate = Convert.ToDateTime(DSEODClose.Tables[0].Rows[0]["CloseDate"].ToString()).ToString("MM/dd/yyyy HH:mm:ss");   //PRIMEPOS-3114 26-Jul-2022 JY Added
            }

            string sql;
            sql = " SELECT HDR.CLOSEDATE ,T.EODID,dept.deptname, " +
                " Sum(CASE T.TRANSTYPE WHEN 1 THEN ExtendedPrice ELSE 0 END) TotalSale " +
                " , Sum(CASE T.TRANSTYPE WHEN 2 THEN ExtendedPrice ELSE 0 END) TotalReturn " +
                " , Sum(td.Discount) TotalDiscount, Sum(TaxAmount) TotalTax " +
                " FROM POSTransactionDetail TD join item i on (i.itemid=td.itemid) " +
                " left join department dept on ( dept.deptid=i.departmentid) " +
                " , POSTransaction T ,ENDOFDAYHEADER HDR WHERE TD.TransID = T.TransID AND T.EODID=HDR.EODID AND T.EODID =" + CloseID +
                " GROUP BY dept.deptname,T.EODID,HDR.CLOSEDATE ";
            DSEODDepartment = oSearch.SearchData(sql);
        }
        public void PrintLine(string LineToPrint)
        {
            int pcWritten = 0;
            PrintDirectAPIs.WritePrinter(lhPrinter, LineToPrint, LineToPrint.Length, ref pcWritten);
            return;
        }

        public void Print()
        {
            // this routine prints the dot matrix label
            //			string sLabelFile;
            //			sLabelFile="rcptEODByStation.js";

            //			if (System.IO.File.Exists(sLabelFile))
            //			{
            this.lhPrinter = new System.IntPtr();

            DOCINFO di = new DOCINFO();
            di.pDocName = "Label";
            di.pDataType = "RAW";

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

                        //ScriptableLabel _lbl = new ScriptableLabel(this,sLabelFile);

                        try
                        {
                            //for(int i=0; i < Configuration.CInfo.NoOfReceipt; i++)
                            //{
                            //_lbl.PrintCLabel();   // this will actually send the label printing lines
                            //}
                            PrintEODByStation();
                            PrintEODByDepartment();
                            //bSuccess=true;
                        }
                        catch (Exception e)
                        {
                            clsUIHelper.ShowErrorMsg(e.Message);

                        }
                    }
                }
            }
            PrintDirectAPIs.EndPagePrinter(lhPrinter);
            PrintDirectAPIs.EndDocPrinter(lhPrinter);
            PrintDirectAPIs.ClosePrinter(lhPrinter);
            //				if (bSuccess == false)
            //				{
            //					clsUIHelper.ShowErrorMsg("Unable to connect to printer");
            //					
            //				}
            //				return;
            //			}
        }

        private void PrintEODByStation()
        {
            //Modified By Shitaljit(QuicSolv) on 27 Jan 2012
            //Redesigned the alignment of the receipt to fit properly to the paper
            //Set left margin to 0 
            //Set Trans Count length to 14
            //Set Pay Type length to 13
            //Set Total Returns to 12
            //Replace PadLeft(13) with PadRight(14)
            //replicate - from 49 to 40


            PrintLine("\x1B" + "x0");

            this.BlankLineBeforeHeading = true;

            PrintLine(Convert.ToChar(1).ToString());

            //PrintLine("\n" + FLen("END OF DAY REPORT FOR " + DSEODClose.Tables[0].Rows[0]["CloseDate"].ToString(), 40));  //PRIMEPOS-3114 26-Jul-2022 JY Commented
            PrintLine("\n" + FLen("END OF DAY REPORT FOR " + this.CloseDate, 45));  //PRIMEPOS-3114 26-Jul-2022 JY Added
            PrintLine("\n" + FLen("SALE BY STATION", 40));


            PrintLine("\n" + FLen("EOD #" + CloseID.ToString(), 13));

            PrintLine("\n" + Replicate("-", 40));

            for (int i = 0; i < DSEODClose.Tables[0].Rows.Count; i++)
            {
                PrintLine("\nStation # " + Configuration.GetStationName(DSEODClose.Tables[0].Rows[i]["STATIONid"].ToString()));
                PrintLine("\n" + FLen("Pay Type", 13) + FLen("Trans Count", 16) + FLen("Trans AMT.", 12));
                PrintLine("\n" + Replicate("-", 40));

                decimal nTotAmount;
                int nTotTrans;

                nTotAmount = 0;
                nTotTrans = 0;

                foreach (DataRow oRow in DSEODClose.Tables[0].Rows[i].GetChildRows(RlPayment))
                {
                    if (oRow["PayTypeDesc"].ToString() == "Elect. Benefit Transfer")
                    {
                        oRow["PayTypeDesc"] = "EBT";
                    }
                    PrintLine("\n" + FLen(oRow["PayTypeDesc"].ToString(), 17) + FLen(Configuration.convertNullToString(oRow["TransCount"].ToString()), 14) + Configuration.convertNullToDecimal(oRow["TransAmount"].ToString()).ToString(Configuration.CInfo.CurrencySymbol.ToString() + "#######0.00").PadRight(10));
                    nTotTrans += Configuration.convertNullToInt(oRow["TransCount"].ToString());
                    nTotAmount += Configuration.convertNullToDecimal(oRow["TransAmount"].ToString());
                }

                PrintLine("\n" + Replicate("-", 40));
                PrintLine("\n" + FLen("Total", 17) + FLen(nTotTrans.ToString(), 14) + nTotAmount.ToString(Configuration.CInfo.CurrencySymbol.ToString() + "########0.00").PadRight(15));

                decimal nTotalCash;
                nTotalCash = TotalCash(DSEODClose.Tables[0].Rows[i]["STATIONid"].ToString());
                PrintLine("\n\n" + FLen("Total Cash:", 16) + FLen(nTotalCash.ToString(Configuration.CInfo.CurrencySymbol.ToString() + "########0.00"), 25));

                decimal nTotalPO;
                nTotalPO = TotalPayout(DSEODClose.Tables[0].Rows[i]["STATIONid"].ToString());

                PrintLine("\n" + FLen("Total Payout:", 16) + FLen(nTotalPO.ToString(Configuration.CInfo.CurrencySymbol.ToString() + "########0.00"), 25));

                PrintLine("\n" + Replicate("-", 40));
                PrintLine("\n" + FLen("Net Total:", 16) + FLen((nTotalCash - nTotalPO).ToString(Configuration.CInfo.CurrencySymbol.ToString() + "########0.00"), 25));

                PrintLine("\n\n" + FLen("Trans Type", 13) + FLen("Trans Count", 16) + FLen("Trans AMT.", 12));

                PrintLine("\n" + Replicate("-", 40));
                nTotTrans = 0;
                nTotAmount = 0;

                foreach (DataRow oRow in DSEODClose.Tables[0].Rows[i].GetChildRows(RlTransaction))
                {
                    PrintLine("\n" + FLen(oRow["Type"].ToString(), 17) + FLen(Configuration.convertNullToString(oRow["TransCount"].ToString()), 15) + Configuration.convertNullToDecimal(oRow["TransAmount"].ToString()).ToString(Configuration.CInfo.CurrencySymbol.ToString() + "#######0.00").PadRight(10));
                    nTotTrans += Configuration.convertNullToInt(oRow["TransCount"].ToString());
                    nTotAmount += Configuration.convertNullToDecimal(oRow["TransAmount"].ToString());
                }

                PrintLine("\n" + Replicate("-", 40));
                PrintLine("\n" + FLen("Total", 17) + FLen(nTotTrans.ToString(), 15) + nTotAmount.ToString(Configuration.CInfo.CurrencySymbol.ToString() + "########0.00").PadRight(15));

                decimal nTotalROA = TotalROA(DSEODClose.Tables[0].Rows[i]["STATIONid"].ToString());
                decimal nTotalTransFee = TotalTransFee(DSEODClose.Tables[0].Rows[i]["STATIONid"].ToString());
                if (nTotalROA != 0 || nTotalTransFee > 0)
                {
                    if (nTotalROA != 0)
                    {
                        PrintLine("\n\n" + FLen("ROA", 13) + FLen("", 15) + FLen(nTotalROA.ToString(Configuration.CInfo.CurrencySymbol.ToString() + "########0.00"), 15));
                    }
                    if (nTotalTransFee > 0)
                    {
                        PrintLine("\n" + FLen("Transaction Fee", 15) + FLen("", 15) + FLen(nTotalTransFee.ToString(Configuration.CInfo.CurrencySymbol.ToString() + "########0.00"), 15));
                    }
                    PrintLine("\n" + FLen("Grand Total", 13) + FLen("", 15) + FLen((nTotAmount + nTotalROA + nTotalTransFee).ToString(Configuration.CInfo.CurrencySymbol.ToString() + "#######0.00"), 15));
                }
                PrintLine("\n" + Replicate("-", 40));
            }

            PrintLine("\n\n\n");
            //PrintLine("\x1B"+"J"+Convert.ToChar(250));
            //PrintLine("\x1B"+"i");
        }


        private void PrintEODByDepartment()
        {
            //Modified By Shitaljit(QuicSolv) on 27 Jan 2012
            //Redesigned the alignment of the receipt to fit properly to the paper
            //Set left margin to 0 
            //Set Department to 15
            //Set Total Sale to 15
            //Replace PadLeft(13) with PadRight(14)
            //replicate - from 49 to 40

            PrintLine("\x1B" + "p" + "011");

            //PrintLine("\n" + FLen("END OF DAY REPORT FOR " + DSEODClose.Tables[0].Rows[0]["CloseDate"].ToString(), 40));  //PRIMEPOS-3114 26-Jul-2022 JY Commented
            PrintLine("\n" + FLen("END OF DAY REPORT FOR " + this.CloseDate, 45));  //PRIMEPOS-3114 26-Jul-2022 JY Added
            PrintLine("\n" + FLen("Department Summary", 40));


            PrintLine("\n" + FLen("EOD #" + CloseID.ToString(), 13));


            PrintLine("\n\n" + Space(0) + FLen("Department", 15) + FLen("Total Sale", 15) + FLen("Total Returns", 12));
            PrintLine("\n" + Replicate("-", 40));

            decimal nTotSale;
            decimal nTotReturn;

            nTotSale = 0;
            nTotReturn = 0;

            foreach (DataRow oRow in DSEODDepartment.Tables[0].Rows)
            {
                PrintLine("\n" + Space(0) + FLen(oRow["DeptName"].ToString(), 19) + FLen(Configuration.convertNullToDecimal(oRow["TotalSale"].ToString()).ToString(Configuration.CInfo.CurrencySymbol.ToString() + "#######0.00"), 13) + Configuration.convertNullToDecimal(oRow["TotalReturn"].ToString()).ToString(Configuration.CInfo.CurrencySymbol.ToString() + "#######0.00").PadRight(10));
                nTotSale += Configuration.convertNullToDecimal(oRow["TotalSale"].ToString());
                nTotReturn += Configuration.convertNullToDecimal(oRow["TotalReturn"].ToString());
            }

            PrintLine("\n" + Replicate("-", 40));
            PrintLine("\n" + Space(0) + FLen("Total", 19) + FLen(nTotSale.ToString(Configuration.CInfo.CurrencySymbol.ToString() + "########0.00"), 13) + nTotReturn.ToString(Configuration.CInfo.CurrencySymbol.ToString() + "########0.00").PadRight(15));

            PrintLine("\n\n\n");
            PrintLine("\x1B" + "J" + Convert.ToChar(250));
            PrintLine("\x1B" + "i");
        }
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


        public decimal TotalPayout(string StationID)
        {
            DataSet oDS = new DataSet();
            Search oSearch = new Search();
            oDS = oSearch.SearchData("select isnull((SELECT sum(isnull(transamount,0)) FROM stationclosedetail scd,stationcloseheader sch WHERE (sch.StationCloseID =scd.StationCloseID and sch.EODID= " + CloseID + " and sch.StationID=" + StationID + " and transtype='PO')),0) ");
            return Configuration.convertNullToDecimal(oDS.Tables[0].Rows[0][0].ToString());
        }
        public decimal TotalCash(string StationID)
        {
            DataSet oDS = new DataSet();
            Search oSearch = new Search();
            oDS = oSearch.SearchData("select isnull((SELECT sum(isnull(transamount,0)) FROM stationclosedetail scd,stationcloseheader sch WHERE (sch.StationCloseID =scd.StationCloseID and sch.EODID= " + CloseID + " and sch.StationID=" + StationID + " and transtype='U-1')),0) ");
            return Configuration.convertNullToDecimal(oDS.Tables[0].Rows[0][0].ToString());
        }

        public decimal TotalROA(string StationID)
        {
            DataSet oDS = new DataSet();
            Search oSearch = new Search();
            oDS = oSearch.SearchData("select isnull((SELECT sum(isnull(transamount,0)) FROM stationclosedetail scd,stationcloseheader sch WHERE (sch.StationCloseID =scd.StationCloseID and sch.EODID= " + CloseID + " and sch.StationID=" + StationID + " and transtype='A')),0) ");
            return Configuration.convertNullToDecimal(oDS.Tables[0].Rows[0][0].ToString());
        }

        #region PRIMEPOS-3118 03-Aug-2022 JY Added
        public decimal TotalTransFee(string StationID)
        {
            DataSet oDS = new DataSet();
            try
            {
                Search oSearch = new Search();
                oDS = oSearch.SearchData("select isnull((SELECT sum(isnull(transamount,0)) FROM stationclosedetail scd,stationcloseheader sch WHERE (sch.StationCloseID =scd.StationCloseID and sch.EODID= " + CloseID + " and sch.StationID=" + StationID + " and transtype='TF')),0) ");
            }
            catch { }
            return Configuration.convertNullToDecimal(oDS.Tables[0].Rows[0][0].ToString());
        }
        #endregion
    }

}
