// ----------------------------------------------------------------
// ----------------------------------------------------------------
using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using POS_Core.CommonData.Tables;
using POS_Core.CommonData.Rows;
using POS_Core.CommonData;
using Resources;
using POS_Core.ErrorLogging;
using System.Collections.Generic;
using POS_Core.Resources;
//using POS.Resources;

namespace POS_Core.DataAccess
{
    public class EndOFDaySvr
    {
        private void UpdateTrnsactionStatus(IDbTransaction tr, IDbConnection conn, int Id)
        {
            string sSQL = "UPDATE POSTransaction SET IsEOD = 1 , EODID = " + Id + " WHERE EODID IS NULL OR EODID = 0";

            IDbCommand cmd = DataFactory.CreateCommand();

            cmd.CommandType = CommandType.Text;
            cmd.CommandText = sSQL;
            cmd.Transaction = tr;
            cmd.Connection = conn;

            cmd.ExecuteNonQuery();

            cmd.CommandText = "UPDATE payout SET IsEOD = 1 , EODID = " + Id + " WHERE EODID IS NULL OR EODID = 0";
            cmd.ExecuteNonQuery();
        }

        private void UpdateStationClose(IDbTransaction tr, IDbConnection conn, int EODID)
        {
            int rowsEffected;
            string sSQL = "UPDATE StationCloseHeader SET EODID = " + EODID + "  WHERE EODID IS NULL OR EODID = 0";

            IDbCommand cmd = DataFactory.CreateCommand();

            cmd.CommandType = CommandType.Text;
            cmd.CommandText = sSQL;
            cmd.Transaction = tr;
            cmd.Connection = conn;

            rowsEffected = cmd.ExecuteNonQuery();

            if (rowsEffected == 0)
                ErrorHandler.throwCustomError(POSErrorENUM.StationClose_NoTransactionFoundForEndOFDay);
        }

        /*		 private void UpdateTrnsactionStatus(IDbTransaction tr,IDbConnection conn)
                 {
                     string sSQL = "UPDATE POSTransaction SET IsStationClosed = 1 WHERE StationId = '" + Configuration.StationID + "'";

                     IDbCommand cmd = DataFactory.CreateCommand();

                     cmd.CommandType = CommandType.Text;
                     cmd.CommandText = sSQL;
                     cmd.Transaction = tr;
                     cmd.Connection = conn;


                     cmd.ExecuteNonQuery();
                 }*/

        private int SaveMaster(IDbTransaction tr, IDbConnection conn)
        {
            int Id = GetNextId();
            string sSQLSubQry = " SELECT " +
                Id +
                " , '" + Configuration.UserName + "'" +
                " , '" + System.DateTime.Now + "'" +
                " , ISNULL((SELECT Sum(TransAmount) FROM StationCloseDetail D , StationCloseHeader H WHERE TransType = 'S' AND H.StationCloseId = D.StationCloseId AND H.EODID IS NULL),0) As Sale " +
                " , ISNULL((SELECT Sum(TransAmount) FROM StationCloseDetail D , StationCloseHeader H WHERE  TransType = 'SR'  AND H.StationCloseId = D.StationCloseId AND H.EODID IS NULL ),0) As SalesReturn " +
                " , ISNULL((SELECT Sum(TransAmount) FROM StationCloseDetail D , StationCloseHeader H WHERE  TransType = 'DT' AND H.StationCloseId = D.StationCloseId AND H.EODID IS NULL),0) As Discount " +
                " , ISNULL((SELECT Sum(TransAmount) FROM StationCloseDetail D , StationCloseHeader H WHERE  TransType = 'TX' AND H.StationCloseId = D.StationCloseId AND H.EODID IS NULL),0) As Tax " +
                " , ISNULL((SELECT Sum(TransAmount) FROM StationCloseDetail D , StationCloseHeader H WHERE TransType = 'A' AND H.StationCloseId = D.StationCloseId AND H.EODID IS NULL),0) As ROA " +
                " , ISNULL((SELECT Sum(TransAmount) FROM StationCloseDetail D , StationCloseHeader H WHERE TransType = 'TF' AND H.StationCloseId = D.StationCloseId AND H.EODID IS NULL),0) As TransFee ";    //PRIMEPOS-3118 03-Aug-2022 JY Added

            string sSQL = "INSERT INTO EndOfDayHeader (EODID,UserID,CloseDate,TotalSales,TotalReturns,TotalDiscount,TotalTax,TotalROA,TotalTransFee) " + sSQLSubQry;
            // PRIMEPOS-2627  NileshJ - Add Column name - 06_Feb_2019 

            IDbCommand cmd = DataFactory.CreateCommand();

            cmd.CommandType = CommandType.Text;
            cmd.CommandText = sSQL;
            cmd.Transaction = tr;
            cmd.Connection = conn;

            cmd.ExecuteNonQuery();
            return Id;
        }

        private void SaveDetail(IDbTransaction tr, IDbConnection conn, int Id)
        {
            String sSQL = "INSERT INTO EndOfDayDetail " +
                                   " SELECT " + Id + " As EODID , subString(TransType,3,len(TransType)) As PayTypeID , sum(TransAmount) As Amount FROM StationCloseDetail D , StationCloseHeader H WHERE (TransType LIKE 'U-%') AND (H.EODID IS NULL or H.EODID=0) AND D.StationCloseId = H.StationCloseId Group By D.TransType";

            IDbCommand cmd = DataFactory.CreateCommand();

            cmd.CommandType = CommandType.Text;
            cmd.CommandText = sSQL;
            cmd.Transaction = tr;
            cmd.Connection = conn;
            cmd.ExecuteNonQuery();
        }

        private int GetNextId()
        {
            IDbCommand cmd = DataFactory.CreateCommand();
            string sSQL = "";

            int Id = 0;
            IDbConnection conn = DataFactory.CreateConnection();

            conn.ConnectionString = DBConfig.ConnectionString;

            conn.Open();

            try
            {

                sSQL = String.Concat("SELECT ",
                    " MAX(EODID)",
                    "  FROM EndOfDayHeader ");

                cmd.CommandType = CommandType.Text;
                cmd.CommandText = sSQL;
                cmd.Connection = conn;

                Id = Convert.ToInt32(((cmd.ExecuteScalar().ToString() == "") ? "0" : cmd.ExecuteScalar().ToString()));

                if (Id == 0)
                    Id = 1;
                else
                    Id = Id + 1;

                conn.Close();
                return Id;
            }
            catch (NullReferenceException)
            {
                conn.Close();
                ErrorHandler.throwCustomError(POSErrorENUM.User_InvalidUserName);
            }
            catch (Exception exp)
            {
                conn.Close();
                throw (exp);
            }
            return Id;
        }

        /// <summary>
        /// Author: Shitaljit 
        /// Public function to check all the stations are closed before doing EOD.
        /// </summary>
        public string[] CheckIfAllStationClosed()
        {
            IDbTransaction tr = null;
            IDbConnection conn = DataFactory.CreateConnection();
            conn.ConnectionString = Resources.Configuration.ConnectionString;

            conn.Open();
            tr = conn.BeginTransaction();
            return this.CheckIfAllStationClosed(tr, conn);
        }

        private string[] CheckIfAllStationClosed(IDbTransaction tr, IDbConnection conn)
        {
            IDbCommand cmd = DataFactory.CreateCommand();
            string sSQL = "";
            IDataReader dr = null;
            List<string> stationsArr = new List<string>();
            try
            {
                //sSQL = "SELECT Count(*) As Count FROM POSTransaction WHERE IsNULL(isStationClosed,0) = 0 ";

                //chaged by SRT(Abhishek) Date : 24/09/2009
                sSQL = "SELECT Count(*) As Count , StationID FROM POSTransaction WHERE IsNULL(isStationClosed,0) = 0 group by StationID" +
                    " UNION " +
                     //Added By Shitaljit(QuicSolv) on 20 Oct 2011
                     " SELECT Count(*) As Count, StationID FROM Payout WHERE IsNULL(isStationClosed,0) = 0 group by StationID ";
                //End Of chaged by SRT(Abhishek) Date : 24/09/2009

                cmd.CommandType = CommandType.Text;
                cmd.CommandText = sSQL;
                cmd.Transaction = tr;
                cmd.Connection = conn;

                dr = cmd.ExecuteReader();

                bool isRead = dr.Read();
                //changed by SRT(Abhishek) Date : 24/09/2009
                if (isRead)
                {
                    if (dr.GetInt32(0) > 0)
                    {
                        //string stations = string.Empty;
                        //stations += "'" + dr.GetString(1) + "'";
                        stationsArr.Add(dr.GetString(1));
                        while (dr.Read())
                        {
                            // stations += " , '" + dr.GetString(1) + "'";
                            stationsArr.Add(dr.GetString(1));
                        }
                        //ErrorHandler.throwCustomError(POSErrorENUM.EndOfDay_CloseAllStationsFirst);
                        //throw new POSExceptions("Please Close Stations " + stations + " First", 0);

                    }
                }

                //End Of changed by SRT(Abhishek) Date : 24/09/2009
                dr.Close();
            }
            catch (Exception exp)
            {
                dr.Close();
                throw (exp);
            }
            return stationsArr.ToArray();
        }

        public EndOFDayData EndOFDay()
        {
            int Id = 0;

            IDbTransaction tr = null;
            IDbConnection conn = DataFactory.CreateConnection();
            conn.ConnectionString = Resources.Configuration.ConnectionString;

            conn.Open();
            tr = conn.BeginTransaction();
            //this.CheckIfAllStationClosed(tr,conn);Coomected By Shitaljit to seperate out the logic
            try
            {
                Id = this.SaveMaster(tr, conn);
                this.SaveDetail(tr, conn, Id);

                UpdateStationClose(tr, conn, Id);
                UpdateTrnsactionStatus(tr, conn, Id);
                CheckIfAllStationClosed(tr, conn);

                tr.Commit();
                tr = null;
                conn.Close();
                return this.FillEndOFDayData(Id);
            }
            catch (POSExceptions ex)
            {

                if (tr != null) tr.Rollback();
                if (conn.State == ConnectionState.Open) conn.Close();
                throw (ex);
            }

            catch (OtherExceptions ex)
            {
                if (tr != null) tr.Rollback();
                if (conn.State == ConnectionState.Open) conn.Close();
                throw (ex);
            }

            catch (SqlException ex)
            {
                if (tr != null) tr.Rollback();
                if (conn.State == ConnectionState.Open) conn.Close();
                //				 ErrorHandler.throwCustomError(POSErrorENUM.StationClose_NoTransactionFound); 
                throw (ex);
            }

            catch (Exception ex)
            {

                tr.Rollback();
                if (conn.State == ConnectionState.Open) conn.Close();
                ErrorHandler.throwException(ex, "", "");
            }
            return null;
        }
        public EndOFDayData FillEndOFDayData(int pEODId)
        {
            IDbCommand cmd = DataFactory.CreateCommand();
            IDataReader dr;
            string sSQL = "";

            EndOFDayData oEndOFDayData = new EndOFDayData();

            IDbConnection conn = DataFactory.CreateConnection();

            conn.ConnectionString = DBConfig.ConnectionString;

            conn.Open();

            try
            {
                //sSQL = "SELECT * FROM EndOfDayHeader WHERE EODID = " + pEODId;    //Sprint-24 - PRIMEPOS-2326 20-Oct-2016 JY Commented
                sSQL = "SELECT EODID, UserID, CloseDate, TotalSales, TotalReturns, TotalDiscount, TotalTax, TotalROA, (SELECT SUM(ISNULL(Amount,0)) FROM Payout WHERE EODID = " + pEODId + ") As Payout, TotalTransFee FROM EndOfDayHeader WHERE EODID = " + pEODId; //Sprint-24 - PRIMEPOS-2326 20-Oct-2016 JY Added   //PRIMEPOS-3118 03-Aug-2022 JY Added

                cmd.CommandType = CommandType.Text;
                cmd.CommandText = sSQL;
                cmd.Connection = conn;
                dr = cmd.ExecuteReader();

                dr.Read();

                oEndOFDayData.EODID = pEODId.ToString();
                oEndOFDayData.TotalSale = Configuration.convertNullToDecimal(dr.GetValue(dr.GetOrdinal("TotalSales")).ToString());
                oEndOFDayData.ReceiveOnAccount = Configuration.convertNullToDecimal(dr.GetValue(dr.GetOrdinal("TotalROA")).ToString());
                oEndOFDayData.TotalReturn = Configuration.convertNullToDecimal(dr.GetValue(dr.GetOrdinal("TotalReturns")).ToString());
                oEndOFDayData.TotalDiscount = Configuration.convertNullToDecimal(dr.GetValue(dr.GetOrdinal("TotalDiscount")).ToString());
                oEndOFDayData.SalesTax = Configuration.convertNullToDecimal(dr.GetValue(dr.GetOrdinal("TotalTax")).ToString());
                oEndOFDayData.Payout = Configuration.convertNullToDecimal(dr.GetValue(dr.GetOrdinal("Payout")).ToString()); //Sprint-24 - PRIMEPOS-2326 20-Oct-2016 JY Added
                oEndOFDayData.CloseDate = dr.GetValue(dr.GetOrdinal("CloseDate")).ToString();   //PRIMEPOS-2480 26-Jun-2020 JY Added
                oEndOFDayData.TransFee = Configuration.convertNullToDecimal(dr.GetValue(dr.GetOrdinal("TotalTransFee")).ToString());    //PRIMEPOS-3118 03-Aug-2022 JY Added
                cmd.Dispose();
                dr.Close();

                cmd = DataFactory.CreateCommand();

                sSQL = " SELECT " +
                            " PT.PayTypeDesc " +
                            " , EOD.Amount " +
                        " FROM " +
                            " EndOfDayDetail EOD" +
                            " , PayType PT" +
                        " WHERE  " +
                            " EOD.EODID = " + pEODId +
                            " AND EOD.PayTypeID = PT.PayTypeID " +
                            " ORDER BY PT.PayTypeDesc ";    //Sprint-24 - PRIMEPOS-2326 20-Oct-2016 JY Added

                cmd.CommandType = CommandType.Text;
                cmd.CommandText = sSQL;
                cmd.Connection = conn;
                dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    EndOFDayDetail oEndOFDayDetail = new EndOFDayDetail();
                    oEndOFDayDetail.Amount = Configuration.convertNullToDecimal(dr.GetValue(dr.GetOrdinal("Amount")).ToString());
                    oEndOFDayDetail.PayTypeName = dr.GetString(dr.GetOrdinal("PayTypeDesc"));
                    oEndOFDayData.Details.Add(oEndOFDayDetail);
                }

                cmd.Dispose();
                dr.Close();

                cmd = DataFactory.CreateCommand();

                sSQL = "SELECT isnull(d.deptcode, ' ') as deptcode, isnull(d.deptname, ' ') as deptname "
                        + " ,  Sum(CASE T.TRANSTYPE WHEN 1 THEN ExtendedPrice ELSE 0 END) TotalSale  "
                        + " , Sum(CASE T.TRANSTYPE WHEN 2 THEN ExtendedPrice ELSE 0 END) TotalReturn  "
                        + " ,cast(Sum(isnull(TD.Discount,0)) as float) TotalDiscount, Sum(isnull(TD.TaxAmount,0)) TotalTax "
                        + " FROM POSTransaction T , POSTransactionDetail TD , Item i left join department d on (i.departmentid=d.deptid) "
                        + " WHERE TD.TransID = T.TransID AND T.EODID =" + pEODId + " and TD.ItemID=i.ItemID "
                        + " GROUP BY d.deptcode,d.deptname order by d.deptname";

                /*sSQL=" SELECT HDR.CLOSEDATE ,T.EODID,dept.deptname, " +
					" Sum(CASE T.TRANSTYPE WHEN 1 THEN ExtendedPrice ELSE 0 END) TotalSale " +
					" , Sum(CASE T.TRANSTYPE WHEN 2 THEN ExtendedPrice ELSE 0 END) TotalReturn " +
					" , Sum(td.Discount) TotalDiscount, Sum(TaxAmount) TotalTax " +
					" FROM POSTransactionDetail TD join item i on (i.itemid=td.itemid) " +
					" left join department dept on ( dept.deptid=i.departmentid) " +
					" , POSTransaction T ,ENDOFDAYHEADER HDR WHERE TD.TransID = T.TransID AND T.EODID=HDR.EODID AND T.EODID =" + pEODId + 
					" GROUP BY dept.deptname,T.EODID,HDR.CLOSEDATE ";*/

                cmd.CommandType = CommandType.Text;
                cmd.CommandText = sSQL;
                cmd.Connection = conn;
                dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    Departments oDept = new Departments();
                    oDept.DepartmentId = dr.GetString(dr.GetOrdinal("DeptCode"));
                    oDept.DepartmentName = dr.GetString(dr.GetOrdinal("DeptName"));
                    oDept.Sales = Convert.ToDecimal(dr["TotalSale"]);
                    oDept.Discount = Convert.ToDecimal(dr["TotalDiscount"]);
                    oDept.Tax = Convert.ToDecimal(dr["TotalTax"]);
                    oEndOFDayData.Departments.Add(oDept);
                }

                conn.Close();
            }
            catch (NullReferenceException)
            {
                conn.Close();
                ErrorHandler.throwCustomError(POSErrorENUM.User_InvalidUserName);
            }
            catch (Exception exp)
            {
                conn.Close();
                throw (exp);
            }
            return oEndOFDayData;
        }

        public DataSet GetReportSource(int pEODId)
        {
            IDbCommand cmd = DataFactory.CreateCommand();
            string sSQL = "";

            DataSet ds = new DataSet();
            IDataAdapter da = DataFactory.CreateDataAdapter();

            IDbConnection conn = DataFactory.CreateConnection();

            conn.ConnectionString = DBConfig.ConnectionString;

            conn.Open();

            try
            {
                sSQL = "SELECT * FROM EndOfDayHeader WHERE EODID = " + pEODId;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = sSQL;
                cmd.Connection = conn;
                SqlDataAdapter sqlDa = (SqlDataAdapter)da;
                sqlDa.SelectCommand = (SqlCommand)cmd;
                da.Fill(ds);


                conn.Close();

                return ds;
            }
            catch (NullReferenceException)
            {
                conn.Close();
                ErrorHandler.throwCustomError(POSErrorENUM.User_InvalidUserName);
            }
            catch (Exception exp)
            {
                conn.Close();
                throw (exp);
            }
            return null;
        }

        public DataSet GetSubReportSource(int pEODId)
        {
            IDbCommand cmd = DataFactory.CreateCommand();
            string sSQL = "";

            DataSet ds = new DataSet();
            IDataAdapter da = DataFactory.CreateDataAdapter();

            IDbConnection conn = DataFactory.CreateConnection();

            conn.ConnectionString = DBConfig.ConnectionString;

            conn.Open();

            try
            {
                sSQL = " SELECT " +
                    " PT.PayTypeDesc " +
                    " , EOD.Amount as transAmount " +
                    " FROM " +
                    " EndOfDayDetail EOD" +
                    " , PayType PT" +
                    " WHERE  " +
                    " EOD.EODID = " + pEODId +
                    " AND EOD.PayTypeID = PT.PayTypeID";

                cmd.CommandType = CommandType.Text;
                cmd.CommandText = sSQL;
                cmd.Connection = conn;
                SqlDataAdapter sqlDa = (SqlDataAdapter)da;
                sqlDa.SelectCommand = (SqlCommand)cmd;
                da.Fill(ds);

                conn.Close();

                return ds;
            }
            catch (NullReferenceException)
            {
                conn.Close();
                ErrorHandler.throwCustomError(POSErrorENUM.User_InvalidUserName);
            }
            catch (Exception exp)
            {
                conn.Close();
                throw (exp);
            }
            return null;
        }

        public DataSet GetDepartmentDS(int pEODId)
        {
            IDbCommand cmd = DataFactory.CreateCommand();
            string sSQL = "";

            DataSet ds = new DataSet();
            IDataAdapter da = DataFactory.CreateDataAdapter();

            IDbConnection conn = DataFactory.CreateConnection();

            conn.ConnectionString = DBConfig.ConnectionString;

            conn.Open();

            try
            {
                sSQL = "SELECT isnull(d.deptcode, ' ') as deptcode, isnull(d.deptname, ' ') as deptname "
                    + " ,  Sum(CASE T.TRANSTYPE WHEN 1 THEN ExtendedPrice ELSE 0 END) TotalSale  "
                    + " , Sum(CASE T.TRANSTYPE WHEN 2 THEN ExtendedPrice ELSE 0 END) TotalReturn  "
                    + " ,cast(-Sum(isnull(TD.Discount,0)) as float) TotalDiscount, Sum(isnull(TD.TaxAmount,0)) TotalTax "
                    + " FROM POSTransaction T , POSTransactionDetail TD , Item i left join department d on (i.departmentid=d.deptid) "
                    + " WHERE TD.TransID = T.TransID AND T.EODID =" + pEODId + " and TD.ItemID=i.ItemID "
                    + " GROUP BY d.deptcode,d.deptname order by d.deptname";

                cmd.CommandType = CommandType.Text;
                cmd.CommandText = sSQL;
                cmd.Connection = conn;
                SqlDataAdapter sqlDa = (SqlDataAdapter)da;
                sqlDa.SelectCommand = (SqlCommand)cmd;
                da.Fill(ds);

                conn.Close();

                return ds;
            }
            catch (NullReferenceException)
            {
                conn.Close();
                ErrorHandler.throwCustomError(POSErrorENUM.User_InvalidUserName);
            }
            catch (Exception exp)
            {
                conn.Close();
                throw (exp);
            }
            return null;
        }

        public DataSet GetDepartmentSource(int pEODId)
        {
            IDbCommand cmd = DataFactory.CreateCommand();
            string sSQL = "";

            DataSet ds = new DataSet();
            IDataAdapter da = DataFactory.CreateDataAdapter();

            IDbConnection conn = DataFactory.CreateConnection();

            conn.ConnectionString = DBConfig.ConnectionString;

            conn.Open();

            try
            {
                sSQL = "SELECT ItemID , ItemDescription , Sum(ExtendedPrice) TotalSale, Sum(Discount) TotalDiscount, Sum(TaxAmount) TotalTax FROM POSTransactionDetail TD , POSTransaction T WHERE TD.TransID = T.TransID AND T.EODID =" + pEODId + " GROUP BY ItemID , ItemDescription";

                cmd.CommandType = CommandType.Text;
                cmd.CommandText = sSQL;
                cmd.Connection = conn;
                SqlDataAdapter sqlDa = (SqlDataAdapter)da;
                sqlDa.SelectCommand = (SqlCommand)cmd;
                da.Fill(ds);


                conn.Close();

                return ds;
            }
            catch (NullReferenceException)
            {
                conn.Close();
                ErrorHandler.throwCustomError(POSErrorENUM.User_InvalidUserName);
            }
            catch (Exception exp)
            {
                conn.Close();
                throw (exp);
            }
            return null;
        }

        /// <summary>
        /// Added By Amit Date 13 May 2011
        /// Method to get summary by station
        /// </summary>
        /// <param name="pEODId"></param>
        /// <returns></returns>
        public DataSet GetStationDS(int pEODId)
        {
            IDbCommand cmd = DataFactory.CreateCommand();
            string sSQL = "";

            DataSet ds = new DataSet();
            IDataAdapter da = DataFactory.CreateDataAdapter();

            IDbConnection conn = DataFactory.CreateConnection();

            conn.ConnectionString = DBConfig.ConnectionString;

            conn.Open();

            try
            {
                sSQL = "select upos.StationId,upos.StationName,"
                       + "sum (case scd.transtype when 's' then scd.transamount else 0 end ) TotalSale,"
                       + "sum (case scd.transtype when 'sr' then scd.transamount else 0 end ) TotalReturn,"
                       + "-sum (case scd.transtype when 'dt' then scd.transamount else 0 end ) TotalDiscount,"

                       + "sum (case scd.transtype when 's' then scd.transamount else 0 end )"
                       + "+sum (case scd.transtype when 'sr' then scd.transamount else 0 end )"
                       + "-sum (case scd.transtype when 'dt' then scd.transamount else 0 end ) NetSale,"

                       + "sum (case scd.transtype when 'tx' then scd.transamount else 0 end ) TotalTax"
                       + " from Util_POSSET upos,StationCloseDetail scd,StationCloseHeader sch"
                       + " where sch.eodid=" + pEODId + "and scd.stationcloseid=sch.stationcloseid and upos.stationid=sch.stationid"
                       + " group by upos.Stationid,upos.stationname";

                cmd.CommandType = CommandType.Text;
                cmd.CommandText = sSQL;
                cmd.Connection = conn;
                SqlDataAdapter sqlDa = (SqlDataAdapter)da;
                sqlDa.SelectCommand = (SqlCommand)cmd;
                da.Fill(ds);

                conn.Close();

                return ds;
            }
            catch (NullReferenceException)
            {
                conn.Close();
                ErrorHandler.throwCustomError(POSErrorENUM.User_InvalidUserName);
            }
            catch (Exception exp)
            {
                conn.Close();
                throw (exp);
            }
            return null;
        }

        //Added By shitaljit 
        #region  RX,Taxable,Non-Taxable Sale Details.
        public DataSet GetRxSalesDetails(int EODID)
        {
            IDbCommand cmd = DataFactory.CreateCommand();
            string sSQL = "";

            DataSet ds = new DataSet();
            IDataAdapter da = DataFactory.CreateDataAdapter();

            IDbConnection conn = DataFactory.CreateConnection();

            conn.ConnectionString = DBConfig.ConnectionString;

            conn.Open();

            try
            {
                //PRIMEPOS-2817 04-Mar-2020 JY Commented
                //sSQL = @"select ISNULL(COUNT(ptd.ItemId), 0) as PresciptionsCount , ISNULL(SUM(ptd.ExtendedPrice),0) as TotalAmt from POSTransaction pt, 
                //                   POSTransactionDetail ptd, POSTransactionRXDetail ptdrx where pt.TransID = ptd.TransID and
                //                   ptd.TransDetailID = ptdrx.TransDetailID and pt.TransType != 2 and pt.EODID = " + EODID;

                //PRIMEPOS-2817 04-Mar-2020 JY Added
                sSQL = @"SELECT ISNULL(COUNT(ptd.ItemId), 0) AS PresciptionsCount, ISNULL(SUM(ptd.ExtendedPrice), 0) AS TotalAmt FROM POSTransaction pt
                        INNER JOIN POSTransactionDetail ptd ON pt.TransID = ptd.TransID
                        INNER JOIN (SELECT DISTINCT RXCode FROM Util_POSSET cc) c ON LTRIM(RTRIM(UPPER(ptd.ItemID))) = LTRIM(RTRIM(UPPER(c.RXCode)))
                        WHERE pt.TransType != 2 and pt.EODID = " + EODID;

                cmd.CommandType = CommandType.Text;
                cmd.CommandText = sSQL;
                cmd.Connection = conn;
                SqlDataAdapter sqlDa = (SqlDataAdapter)da;
                sqlDa.SelectCommand = (SqlCommand)cmd;
                da.Fill(ds);

                conn.Close();

                return ds;
            }
            catch (NullReferenceException)
            {
                conn.Close();
                ErrorHandler.throwCustomError(POSErrorENUM.User_InvalidUserName);
            }
            catch (Exception exp)
            {
                conn.Close();
                throw (exp);
            }
            return null;
        }

        public DataSet GetTaxableItemSalesDetails(int EODID)
        {
            IDbCommand cmd = DataFactory.CreateCommand();
            string sSQL = "";

            DataSet ds = new DataSet();
            IDataAdapter da = DataFactory.CreateDataAdapter();

            IDbConnection conn = DataFactory.CreateConnection();

            conn.ConnectionString = DBConfig.ConnectionString;

            conn.Open();

            try
            {
                //Sprint-22 20-Oct-2015 JY Commented wrong query
                //                 sSQL = @" select ISNULL(COUNT(ptd.ItemID),0) as ItemCount ,ISNULL(SUM(ptd.ExtendedPrice),0) as TotalAmount from POSTransaction pt,
                //                                        POSTransactionDetail ptd LEFT OUTER JOIN POSTransactionRXDetail  ptdrx on ptd.TransDetailID = ptdrx.TransDetailID 
                //                                        where pt.TransID = ptd.TransID and  pt.TotalTaxAmount > 0 and pt.TransType = 1  and  ptdrx.TransDetailID is null and pt.EODID = " + EODID;

                //Sprint-22 20-Oct-2015 JY Added corrected query
                sSQL = @" SELECT ISNULL(COUNT(ptd.ItemID),0) as ItemCount, ISNULL(SUM(ptd.ExtendedPrice),0) as TotalAmount FROM POSTransaction pt
                        INNER JOIN POSTransactionDetail ptd ON pt.TransID = ptd.TransID
                        LEFT OUTER JOIN POSTransactionRXDetail ptdrx ON ptd.TransDetailID = ptdrx.TransDetailID 
                        WHERE ptd.TaxAmount > 0 and pt.TransType = 1 and ptdrx.TransDetailID is null and pt.EODID = " + EODID;

                cmd.CommandType = CommandType.Text;
                cmd.CommandText = sSQL;
                cmd.Connection = conn;
                SqlDataAdapter sqlDa = (SqlDataAdapter)da;
                sqlDa.SelectCommand = (SqlCommand)cmd;
                da.Fill(ds);

                conn.Close();

                return ds;
            }
            catch (NullReferenceException)
            {
                conn.Close();
                ErrorHandler.throwCustomError(POSErrorENUM.User_InvalidUserName);
            }
            catch (Exception exp)
            {
                conn.Close();
                throw (exp);
            }
            return null;
        }

        public DataSet GetNonTaxableItemSalesDetails(int EODID)
        {
            IDbCommand cmd = DataFactory.CreateCommand();
            string sSQL = "";

            DataSet ds = new DataSet();
            IDataAdapter da = DataFactory.CreateDataAdapter();

            IDbConnection conn = DataFactory.CreateConnection();

            conn.ConnectionString = DBConfig.ConnectionString;

            conn.Open();

            try
            {
                //Sprint-22 20-Oct-2015 JY Commented wrong query
                //                sSQL = @" select ISNULL(COUNT(ptd.ItemID),0) as ItemCount ,ISNULL(SUM(ptd.ExtendedPrice),0) as TotalAmount from POSTransaction pt,
                //                        POSTransactionDetail ptd LEFT OUTER JOIN POSTransactionRXDetail  ptdrx on ptd.TransDetailID = ptdrx.TransDetailID 
                //                        where pt.TransID = ptd.TransID and  pt.TotalTaxAmount = 0 and pt.TransType = 1  and  ptdrx.TransDetailID is null and pt.EODID = " + EODID;

                //Sprint-22 20-Oct-2015 JY Added corrected query
                sSQL = @" SELECT ISNULL(COUNT(ptd.ItemID),0) as ItemCount, ISNULL(SUM(ptd.ExtendedPrice),0) as TotalAmount FROM POSTransaction pt
                    INNER JOIN POSTransactionDetail ptd ON pt.TransID = ptd.TransID 
                    LEFT OUTER JOIN POSTransactionRXDetail ptdrx ON ptd.TransDetailID = ptdrx.TransDetailID 
                    where ptd.TaxAmount = 0 and pt.TransType = 1 and ptdrx.TransDetailID is null and pt.EODID = " + EODID;

                cmd.CommandType = CommandType.Text;
                cmd.CommandText = sSQL;
                cmd.Connection = conn;
                SqlDataAdapter sqlDa = (SqlDataAdapter)da;
                sqlDa.SelectCommand = (SqlCommand)cmd;
                da.Fill(ds);

                conn.Close();

                return ds;
            }
            catch (NullReferenceException)
            {
                conn.Close();
                ErrorHandler.throwCustomError(POSErrorENUM.User_InvalidUserName);
            }
            catch (Exception exp)
            {
                conn.Close();
                throw (exp);
            }
            return null;
        }
        #endregion
    }
}