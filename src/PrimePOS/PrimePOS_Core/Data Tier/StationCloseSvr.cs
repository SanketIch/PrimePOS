// ----------------------------------------------------------------
// ----------------------------------------------------------------
using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using POS_Core.CommonData.Tables;
using POS_Core.CommonData.Rows;
using POS_Core.CommonData;
//using Resources;
using POS_Core.ErrorLogging;
////using POS.Resources;
using POS_Core.Resources;
using Resources;

namespace POS_Core.DataAccess
{


    public class StationCloseSvr
    {
        //Added By shitaljit(QuicSolv) on 12 June 2011
        private static int stationCloseID;
        //End of added by shitaljit
        private void UpdatePayoutStatus(IDbTransaction tr, IDbConnection conn, int Id, String DrawNo)
        {
            String sDrawNo = "";
            if (DrawNo == "ALL" || DrawNo == "")
            {
                sDrawNo = "";
            }
            else
            {
                sDrawNo = " and drawno='" + DrawNo + "'";
            }
            string sSQL = "UPDATE Payout SET IsStationClosed = 1 , StationCloseID = " + Id + " WHERE StationId = '" + Configuration.StationID + "' and (isStationClosed is null or isStationClosed=0)" + sDrawNo;
            IDbCommand cmd = DataFactory.CreateCommand();

            cmd.CommandType = CommandType.Text;
            cmd.CommandText = sSQL;
            cmd.Transaction = tr;
            cmd.Connection = conn;
            cmd.ExecuteNonQuery();
        }

        private void UpdateTrnsactionStatus(IDbTransaction tr, IDbConnection conn, int Id, String DrawNo)
        {
            String sDrawNo = "";
            if (DrawNo == "ALL" || DrawNo == "")
            {
                sDrawNo = "";
            }
            else
            {
                sDrawNo = " and drawerno='" + DrawNo + "'";
            }
            string sSQL = "UPDATE POSTransaction SET IsStationClosed = 1 , StClosedID = " + Id + " WHERE StationId = '" + Configuration.StationID + "' and (IsStationClosed  is null or IsStationClosed=0)" + sDrawNo;

            IDbCommand cmd = DataFactory.CreateCommand();

            cmd.CommandType = CommandType.Text;
            cmd.CommandText = sSQL;
            cmd.Transaction = tr;
            cmd.Connection = conn;
            cmd.ExecuteNonQuery();
        }

        private int SaveMaster(IDbTransaction tr, IDbConnection conn)
        {
            int Id = GetNextId();

            //Added By shitaljit(QuicSolv) on 12 June 2011
            stationCloseID = Id;
            //End of added by shitaljit

            //Sprint-19 - 2165 18-Mar-2015 JY Added DefCDStartBalance column
            string sSQL = "INSERT INTO StationCloseHeader ( " +
                " StationCloseID " +
                " , StationID " +
                " , UserID " +
                " , CloseDate " +
                " , DefCDStartBalance " +
                " ) " +
                " VALUES " +
                " ( " + Id +
                " , '" + Resources.Configuration.StationID + "' " +
                " , '" + Resources.Configuration.UserName + "' " +
                " , '" + DateTime.Now + "' " +
                " , " + (Configuration.CInfo.UseCashManagement == true ? Configuration.convertNullToDecimal(Configuration.CInfo.DefCDStartBalance) : 0) +
                " )";

            IDbCommand cmd = DataFactory.CreateCommand();

            cmd.CommandType = CommandType.Text;
            cmd.CommandText = sSQL;
            cmd.Transaction = tr;
            cmd.Connection = conn;

            cmd.ExecuteNonQuery();
            return Id;
        }

        private int SaveDetail(IDbTransaction tr, IDbConnection conn, int Id, String DrawNo)
        {
            int rowEffected = 0;

            String sDrawNo = "";
            String sDrawNo1 = "";
            if (DrawNo == "ALL" || DrawNo == "")
            {
                sDrawNo = "";
                sDrawNo1 = "";
            }
            else
            {
                sDrawNo = " and drawerno='" + DrawNo + "'";
                sDrawNo1 = " and drawno='" + DrawNo + "'";
            }
            //PRIMEPOS-2504 20-Apr-2018 JY considered StationId as string, previously it was considered as int
            String sSQLSubQry =
                "((Select DrawNo as DrawerNo , 'PO' TransType , Count(*) TransCount , Sum(Amount) TotalAmount From Payout WHERE StationId = '" + Configuration.StationID + "' AND isnull(IsStationClosed,0) = 0 " + sDrawNo1 + " group by DrawNo) " +
                 " UNION ALL " +
                 " (SELECT DrawerNo , 'S' TransType, Count(*) TransCount, Sum(GrossTotal) FROM POSTransaction WHERE StationId = '" + Configuration.StationID + "' AND TransType = 1 AND  isnull(IsStationClosed,0) = 0  " + sDrawNo + " Group By DrawerNo)  " +
                 " UNION ALL " +
                 " (SELECT DrawerNo , 'A' TransType, Count(*) TransCount, Sum(totalpaid) FROM POSTransaction WHERE StationId = '" + Configuration.StationID + "' AND TransType = 3 AND  isnull(IsStationClosed,0) = 0 " + sDrawNo + "  Group By DrawerNo)  " +
                 " UNION ALL " +
                 " (SELECT DrawerNo , 'SR' TransType, Count(*) TransCount, Sum(GrossTotal) FROM POSTransaction WHERE StationId = '" + Configuration.StationID + "' AND TransType = 2 AND  isnull(IsStationClosed,0) = 0  " + sDrawNo + "   Group By DrawerNo) " +
                 " UNION ALL " +
                 " (SELECT DrawerNo , 'DT' TransType, Count(*) TransCount, Sum(TotalDiscAmount) FROM POSTransaction  WHERE isnull(totaldiscamount,0)<>0 and StationId = '" + Configuration.StationID + "' AND  isnull(IsStationClosed,0) = 0   " + sDrawNo + "  Group By DrawerNo)  " +
                 " UNION ALL " +
                 " (SELECT DrawerNo , 'TX' TransType, Count(*) TransCount, Sum(TotalTaxAmount) FROM POSTransaction WHERE isnull(totaltaxamount,0)<>0 and StationId = '" + Configuration.StationID + "' AND  isnull(IsStationClosed,0) = 0   " + sDrawNo + " Group By DrawerNo)  " +
                 " UNION ALL " +
                 " (SELECT DrawerNo , 'U-'+PT.PayTypeId  as TransType, Count(*) as TransCount , Sum(TP.Amount) FROM POSTransPayment As TP , PayType PT , POSTransaction Trn WHERE TP.TransTypeCode = PT.PayTypeId AND TP.TransID = Trn.TransID AND Trn.StationId = '" + Configuration.StationID + "' AND  isnull(IsStationClosed,0) = 0   " + sDrawNo + " GROUP BY PT.PayTypeId,Trn.DrawerNo)" +
                 " UNION ALL " +
                 " (SELECT DrawerNo, 'TF' TransType, Count(*) TransCount, Sum(ISNULL(TotalTransFeeAmt,0)) FROM POSTransaction WHERE StationId = '" + Configuration.StationID + "' AND isnull(IsStationClosed,0) = 0 " + sDrawNo + " Group By DrawerNo))";    //PRIMEPOS-3118 03-Aug-2022 JY Added

            string sSQL = "INSERT INTO StationCloseDetail SELECT " + Id + " As StationCloseID , subQuery.DrawerNo , subQuery.TransType , subQuery.TransCount , IsNULL(subQuery.TotalAmount,0) FROM " + sSQLSubQry + " As subQuery ";

            IDbCommand cmd = DataFactory.CreateCommand();

            cmd.CommandType = CommandType.Text;
            cmd.CommandText = sSQL;
            cmd.Transaction = tr;
            cmd.Connection = conn;
            rowEffected = cmd.ExecuteNonQuery();
            return rowEffected;
        }

        #region PRIMEPOS-3042 22-Dec-2021 JY Added
        private int SaveCloseStationMaster(IDbTransaction tr, IDbConnection conn, string strStationID)
        {
            int Id = GetNextId();
            stationCloseID = Id;

            string sSQL = "INSERT INTO StationCloseHeader (StationCloseID, StationID, UserID, CloseDate, DefCDStartBalance)" +
                " VALUES (" + Id + ", '" + strStationID + "', '" + Resources.Configuration.UserName + "', '" + DateTime.Now + "'" +
                ", " + (Configuration.CInfo.UseCashManagement == true ? Configuration.convertNullToDecimal(Configuration.CInfo.DefCDStartBalance) : 0) + ")";

            int nRowsAffected = DataHelper.ExecuteNonQuery(tr, CommandType.Text, sSQL);

            String sSQLSubQry =
                "((Select DrawNo as DrawerNo , 'PO' TransType , Count(*) TransCount , Sum(Amount) TotalAmount From Payout WHERE StationId = '" + strStationID + "' AND isnull(IsStationClosed,0) = 0 group by DrawNo) " +
                 " UNION ALL " +
                 " (SELECT DrawerNo , 'S' TransType, Count(*) TransCount, Sum(GrossTotal) FROM POSTransaction WHERE StationId = '" + strStationID + "' AND TransType = 1 AND  isnull(IsStationClosed,0) = 0 Group By DrawerNo)  " +
                 " UNION ALL " +
                 " (SELECT DrawerNo , 'A' TransType, Count(*) TransCount, Sum(totalpaid) FROM POSTransaction WHERE StationId = '" + strStationID + "' AND TransType = 3 AND  isnull(IsStationClosed,0) = 0 Group By DrawerNo)  " +
                 " UNION ALL " +
                 " (SELECT DrawerNo , 'SR' TransType, Count(*) TransCount, Sum(GrossTotal) FROM POSTransaction WHERE StationId = '" + strStationID + "' AND TransType = 2 AND  isnull(IsStationClosed,0) = 0 Group By DrawerNo) " +
                 " UNION ALL " +
                 " (SELECT DrawerNo , 'DT' TransType, Count(*) TransCount, Sum(TotalDiscAmount) FROM POSTransaction  WHERE isnull(totaldiscamount,0)<>0 and StationId = '" + strStationID + "' AND  isnull(IsStationClosed,0) = 0 Group By DrawerNo)  " +
                 " UNION ALL " +
                 " (SELECT DrawerNo , 'TX' TransType, Count(*) TransCount, Sum(TotalTaxAmount) FROM POSTransaction WHERE isnull(totaltaxamount,0)<>0 and StationId = '" + strStationID + "' AND  isnull(IsStationClosed,0) = 0 Group By DrawerNo)  " +
                 " UNION ALL " +
                 " (SELECT DrawerNo , 'U-'+PT.PayTypeId  as TransType, Count(*) as TransCount , Sum(TP.Amount) FROM POSTransPayment As TP , PayType PT , POSTransaction Trn WHERE TP.TransTypeCode = PT.PayTypeId AND TP.TransID = Trn.TransID AND Trn.StationId = '" + strStationID + "' AND  isnull(IsStationClosed,0) = 0 GROUP BY PT.PayTypeId,Trn.DrawerNo)" +
                 " UNION ALL " +
                 " (SELECT DrawerNo, 'TF' TransType, Count(*) TransCount, Sum(ISNULL(TotalTransFeeAmt,0)) FROM POSTransaction WHERE StationId = '" + strStationID + "' AND isnull(IsStationClosed,0) = 0 Group By DrawerNo))";    //PRIMEPOS-3118 03-Aug-2022 JY Added

            sSQL = "INSERT INTO StationCloseDetail SELECT " + Id + " As StationCloseID , subQuery.DrawerNo , subQuery.TransType , subQuery.TransCount , IsNULL(subQuery.TotalAmount,0) FROM " + sSQLSubQry + " As subQuery ";
            nRowsAffected = DataHelper.ExecuteNonQuery(tr, CommandType.Text, sSQL);

            sSQL = "UPDATE Payout SET IsStationClosed = 1 , StationCloseID = " + Id + " WHERE StationId = '" + strStationID + "' and (isStationClosed is null or isStationClosed=0)";
            nRowsAffected = DataHelper.ExecuteNonQuery(tr, CommandType.Text, sSQL);

            sSQL = "UPDATE POSTransaction SET IsStationClosed = 1 , StClosedID = " + Id + " WHERE StationId = '" + strStationID + "' and (IsStationClosed  is null or IsStationClosed=0)";
            nRowsAffected = DataHelper.ExecuteNonQuery(tr, CommandType.Text, sSQL);
            return Id;
        }
        #endregion

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
                    " MAX(StationCloseID)",
                    "  FROM StationCloseHeader ");

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

        private void HasAnyTransactions(String DrawNo)
        {
            IDbCommand cmd = DataFactory.CreateCommand();
            string sSQL = "";

            String sDrawNo = "";
            String sDrawNo1 = "";
            if (DrawNo == "ALL" || DrawNo == "")
            {
                sDrawNo = "";
                sDrawNo1 = "";
            }
            else
            {
                sDrawNo = " and drawno='" + DrawNo + "'";
                sDrawNo1 = " and drawerno='" + DrawNo + "'";
            }

            int Count = 0;
            IDbConnection conn = DataFactory.CreateConnection();

            conn.ConnectionString = DBConfig.ConnectionString;

            conn.Open();

            try
            {

                sSQL = "SELECT (SELECT Count(*) FROM POSTransaction  WHERE isStationClosed <> 1 " + sDrawNo1 + " and stationid='" + Configuration.StationID + "') + (SELECT Count(*) FROM Payout WHERE IsStationClosed <> 1 " + sDrawNo + " and stationid='" + Configuration.StationID + "')";

                cmd.CommandType = CommandType.Text;
                cmd.CommandText = sSQL;
                cmd.Connection = conn;

                Count = Convert.ToInt32(((cmd.ExecuteScalar().ToString() == "") ? "0" : cmd.ExecuteScalar().ToString()));

                if (Count == 0)
                    ErrorHandler.throwCustomError(POSErrorENUM.StationClose_NoTransactionFoundForCloseStation);

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
        }

        /// <summary>
        /// Added By Shitaljit to check current cash in the drawer
        /// </summary>
        /// <param name="DrawNo"></param>
        /// <returns></returns>
        public Decimal CurrentCashStatus(String DrawNo)
        {
            decimal TotalAmnt = 0;
            IDbConnection conn = DataFactory.CreateConnection();
            IDbTransaction tr = null;
            try
            {
                conn.ConnectionString = DBConfig.ConnectionString;
                conn.Open();
                tr = conn.BeginTransaction();
                string sSQL = @"SELECT Sum(TP.Amount) as TransAmount 
                                FROM POSTransPayment As TP , PayType PT , POSTransaction Trn WHERE 
                                TP.TransTypeCode = PT.PayTypeId AND TP.TransID = Trn.TransID AND 
                                Trn.StationId = '" + Configuration.StationID + "'  AND  isnull(IsStationClosed,0) = 0  AND  PT.PayTypeId = '1' ";
                if (string.IsNullOrEmpty(DrawNo) == false)
                {
                    sSQL = "AND Trn.DrawerNo ='" + DrawNo + "'";
                }

                IDbCommand cmd = DataFactory.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = sSQL;
                cmd.Transaction = tr;
                cmd.Connection = conn;

                Object RetVal = cmd.ExecuteScalar();

                if (RetVal != DBNull.Value)
                {
                    TotalAmnt = Convert.ToDecimal(RetVal.ToString());
                }
                else
                {
                    TotalAmnt = -1;
                }

                tr.Commit();
                conn.Close();
                return TotalAmnt;
            }
            catch (Exception Ex)
            {
                tr.Rollback();
                throw Ex;
            }
            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
            }
        }

        public CloseStationData closeWorkstation(String DrawNo)
        {
            int Id = 0, rowsEffected = 0;

            IDbTransaction tr = null;
            IDbConnection conn = DataFactory.CreateConnection();
            conn.ConnectionString = DBConfig.ConnectionString;

            HasAnyTransactions(DrawNo);

            conn.Open();
            tr = conn.BeginTransaction();
            try
            {
                Id = this.SaveMaster(tr, conn);
                rowsEffected = this.SaveDetail(tr, conn, Id, DrawNo);

                UpdatePayoutStatus(tr, conn, Id, DrawNo);
                UpdateTrnsactionStatus(tr, conn, Id, DrawNo);


                tr.Commit();
                tr = null;
                conn.Close();

                return this.FillCloseStationData(Id);
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

            //			catch(SqlException ) 
            //			 {
            //				 if (tr!=null) tr.Rollback();
            //				 if (conn.State == ConnectionState.Open) conn.Close();
            //
            //				 //ErrorHandler.throwCustomError(POSErrorENUM.StationClose_NoTransactionFoundForCloseStation); 
            //			 }
            catch (Exception ex)
            {
                if (tr != null) tr.Rollback();
                if (conn.State == ConnectionState.Open) conn.Close();
                ErrorHandler.throwException(ex, "", "");
            }
            return null;
        }

        #region PRIMEPOS-3042 22-Dec-2021 JY Added
        public int CloseStation(String strStationID, ref string strErrMsg)
        {
            int StationCloseID = 0;
            IDbTransaction tr = null;
            IDbConnection conn = DataFactory.CreateConnection();
            conn.ConnectionString = DBConfig.ConnectionString;

            conn.Open();
            tr = conn.BeginTransaction();
            try
            {
                StationCloseID = this.SaveCloseStationMaster(tr, conn, strStationID);
                tr.Commit();
                tr = null;
                conn.Close();
            }
            catch (POSExceptions ex)
            {
                if (tr != null) tr.Rollback();
                strErrMsg = ex.Message;
            }
            catch (OtherExceptions ex)
            {
                if (tr != null) tr.Rollback();
                strErrMsg = ex.Message;
            }
            catch (Exception ex)
            {
                if (tr != null) tr.Rollback();
                strErrMsg = ex.Message;
            }
            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
            }
            return StationCloseID;
        }
        #endregion

        public CloseStationData FillCloseStationData(int pStationCloseId)
        {
            IDbCommand cmd = DataFactory.CreateCommand();
            IDataReader dr;
            string sSQL = "";

            CloseStationData oCloseStationData = new CloseStationData();

            IDbConnection conn = DataFactory.CreateConnection();

            conn.ConnectionString = DBConfig.ConnectionString;

            conn.Open();
            //Modify By Shitaljit(QuicSolv) on 24 june 2011
            //Added VerifiedBy,VerifiedDate,VerifiedCashAmt 
            try
            {
                //Sprint-19 - 2165 19-Mar-2015 JY Commented
                //sSQL = " SELECT " +
                //            " ISNULL((SELECT sum(TransAmount) FROM StationCloseDetail WHERE StationCloseId = " + pStationCloseId + " AND TransType = 'PO'),0) As PayPut " +
                //            " , ISNULL((SELECT sum(TransAmount) FROM StationCloseDetail WHERE StationCloseId = " + pStationCloseId + " AND TransType = 'S'),0) As Sale " +
                //            " , ISNULL((SELECT sum(TransAmount) FROM StationCloseDetail WHERE StationCloseId = " + pStationCloseId + " AND TransType = 'A'),0) As ROA " +
                //            " , ISNULL((SELECT sum(TransAmount) FROM StationCloseDetail WHERE StationCloseId = " + pStationCloseId + " AND TransType = 'SR'),0) As SalesReturn " +
                //            " , ISNULL((SELECT sum(TransAmount) FROM StationCloseDetail WHERE StationCloseId = " + pStationCloseId + " AND TransType = 'DT'),0) As Discount " +
                //            " , ISNULL((SELECT sum(TransAmount) FROM StationCloseDetail WHERE StationCloseId = " + pStationCloseId + " AND TransType = 'TX'),0) As Tax " +
                //            " , (SELECT VerifiedBy FROM StationCloseheader WHERE StationCloseId = " + pStationCloseId + ") As VerifiedBy " +
                //            " , ISNULL((SELECT VerifiedDate FROM StationCloseheader WHERE StationCloseId = " + pStationCloseId + ") ,0) As VerifiedDate " +
                //            " , ISNULL((SELECT VerifiedCashAmt FROM StationCloseheader WHERE StationCloseId = " + pStationCloseId + ") ,0) As VerifiedAmt " +
                //            " , ISNULL((SELECT stationid FROM StationCloseheader WHERE StationCloseId = " + pStationCloseId + ") ,0) As StationID ";

                //Sprint-19 - 2165 19-Mar-2015 JY Added DefCDStartBalance and optimized the sql
                //PRIMEPOS-2480 26-Jun-2020 JY Added CloseDate
                sSQL = " SELECT ISNULL((SELECT sum(TransAmount) FROM StationCloseDetail WHERE StationCloseId = " + pStationCloseId + " AND TransType = 'PO'),0) As PayPut " +
                            ", ISNULL((SELECT sum(TransAmount) FROM StationCloseDetail WHERE StationCloseId = " + pStationCloseId + " AND TransType = 'S'),0) As Sale " +
                            ", ISNULL((SELECT sum(TransAmount) FROM StationCloseDetail WHERE StationCloseId = " + pStationCloseId + " AND TransType = 'A'),0) As ROA " +
                            ", ISNULL((SELECT sum(TransAmount) FROM StationCloseDetail WHERE StationCloseId = " + pStationCloseId + " AND TransType = 'SR'),0) As SalesReturn " +
                            ", ISNULL((SELECT sum(TransAmount) FROM StationCloseDetail WHERE StationCloseId = " + pStationCloseId + " AND TransType = 'DT'),0) As Discount " +
                            ", ISNULL((SELECT sum(TransAmount) FROM StationCloseDetail WHERE StationCloseId = " + pStationCloseId + " AND TransType = 'TX'),0) As Tax " +
                            ", VerifiedBy, ISNULL(VerifiedDate,0) AS VerifiedDate, ISNULL(VerifiedCashAmt,0) AS VerifiedAmt, ISNULL(stationid,0) AS StationID, ISNULL(DefCDStartBalance,0) AS DefCDStartBalance, CloseDate" +
                            ", ISNULL((SELECT sum(TransAmount) FROM StationCloseDetail WHERE StationCloseId = " + pStationCloseId + " AND TransType = 'TF'),0) As TransFee " +   //PRIMEPOS-3118 03-Aug-2022 JY Added
                            " FROM StationCloseheader WHERE StationCloseId = " + pStationCloseId;

                cmd.CommandType = CommandType.Text;
                cmd.CommandText = sSQL;
                cmd.Connection = conn;
                dr = cmd.ExecuteReader();

                dr.Read();

                oCloseStationData.StationCloseNo = pStationCloseId.ToString();
                oCloseStationData.Payout = Configuration.convertNullToDecimal(dr.GetValue(dr.GetOrdinal("PayPut")).ToString());
                oCloseStationData.TotalSale = Configuration.convertNullToDecimal(dr.GetValue(dr.GetOrdinal("Sale")).ToString());
                oCloseStationData.TotalReturn = Configuration.convertNullToDecimal(dr.GetValue(dr.GetOrdinal("SalesReturn")).ToString());
                oCloseStationData.TotalDiscount = Configuration.convertNullToDecimal(dr.GetValue(dr.GetOrdinal("Discount")).ToString());
                oCloseStationData.SalesTax = Configuration.convertNullToDecimal(dr.GetValue(dr.GetOrdinal("Tax")).ToString());
                oCloseStationData.ReceiveOnAccount = Configuration.convertNullToDecimal(dr.GetValue(dr.GetOrdinal("ROA")).ToString());
                oCloseStationData.StationID = dr.GetValue(dr.GetOrdinal("StationID")).ToString();
                //Added By Shitaljit(QuicSolv) on 24 june 2011
                oCloseStationData.VerifiedBy = dr.GetValue(dr.GetOrdinal("VerifiedBy")).ToString();
                oCloseStationData.VerifiedDate = dr.GetValue(dr.GetOrdinal("VerifiedDate")).ToString();
                oCloseStationData.VerifiedAmount = Configuration.convertNullToDecimal(dr.GetValue(dr.GetOrdinal("VerifiedAmt")));
                //End of Added By Shitaljit
                oCloseStationData.DefCDStartBalance = Configuration.convertNullToDecimal(dr.GetValue(dr.GetOrdinal("DefCDStartBalance")));    //Sprint-19 - 2165 19-Mar-2015 JY Added
                oCloseStationData.CloseDate = dr.GetValue(dr.GetOrdinal("CloseDate")).ToString();   //PRIMEPOS-2480 26-Jun-2020 JY Added
                oCloseStationData.TransFee = Configuration.convertNullToDecimal(dr.GetValue(dr.GetOrdinal("TransFee")).ToString()); //PRIMEPOS-3118 03-Aug-2022 JY Added
                cmd.Dispose();
                dr.Close();

                cmd = DataFactory.CreateCommand();

                sSQL = " SELECT " +
                            " sum(SCD.TransAmount) as TransAmount  " +
                            " , PT.PayTypeDesc " +
                        " FROM " +
                            " StationCloseDetail SCD " +
                            " , PayType PT  " +
                        " WHERE  " +
                            " SCD.StationCloseId = " + pStationCloseId +
                            " AND SCD.TransType LIKE 'U-%' " +
                            " AND PT.PayTypeID = substring( SCD.TransType,3,len(SCD.TransType)) group by PT.paytypedesc " +
                            " ORDER BY PT.paytypedesc ";    //Sprint-24 - PRIMEPOS-2326 20-Oct-2016 JY Added order by clause    

                cmd.CommandType = CommandType.Text;
                cmd.CommandText = sSQL;
                cmd.Connection = conn;
                dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    CloseStationDetail oCloseStationDetail = new CloseStationDetail();
                    oCloseStationDetail.Amount = Configuration.convertNullToDecimal(dr.GetValue(dr.GetOrdinal("TransAmount")).ToString());
                    oCloseStationDetail.PayTypeName = dr.GetString(dr.GetOrdinal("PayTypeDesc"));
                    oCloseStationData.Details.Add(oCloseStationDetail);
                }

                conn.Close();

                return oCloseStationData;
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

        public DataSet GetReportSource(int pStationCloseId)
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
                    " (SELECT TransAmount FROM StationCloseDetail WHERE StationCloseId = " + pStationCloseId + " AND TransType = 'PO') As PayPut " +
                    " , (SELECT TransAmount FROM StationCloseDetail WHERE StationCloseId = " + pStationCloseId + " AND TransType = 'S') As Sale " +
                    " , (SELECT TransAmount FROM StationCloseDetail WHERE StationCloseId = " + pStationCloseId + " AND TransType = 'A') As ReceiveOnAccount " +
                    " , (SELECT TransAmount FROM StationCloseDetail WHERE StationCloseId = " + pStationCloseId + " AND TransType = 'SR') As SalesReturn " +
                    " , (SELECT TransAmount FROM StationCloseDetail WHERE StationCloseId = " + pStationCloseId + " AND TransType = 'DT') As Discount " +
                    " , (SELECT TransAmount FROM StationCloseDetail WHERE StationCloseId = " + pStationCloseId + " AND TransType = 'TX') As Tax " +
                    " , (SELECT TransAmount FROM StationCloseDetail WHERE StationCloseID = " + pStationCloseId + " AND TransType = 'U-1') As Cash " +
                    " , (SELECT TransAmount FROM StationCloseDetail WHERE StationCloseID = " + pStationCloseId + " AND TransType = 'TF') As TransFee";  //PRIMEPOS-3118 03-Aug-2022 JY Added TransFee

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

        public DataSet GetSubReportSource(int pStationCloseId)
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
                    " SCD.TransAmount as TransAmount " +
                    " , PT.PayTypeDesc " +
                    " FROM " +
                    " StationCloseDetail SCD " +
                    " , PayType PT  " +
                    " WHERE  " +
                    " SCD.StationCloseId = " + pStationCloseId +
                    " AND SCD.TransType LIKE 'U-%' " +
                    " AND PT.PayTypeID = substring( SCD.TransType,3,len(SCD.TransType))";

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

        //Added By Shitaljit(QuicSolv) on 13 June 2011
        /// <summary>
        /// Function will Populate all the records fro CurrencyDenominations Table
        /// </summary>
        /// <returns>DataSet</returns>
        public DataSet PopulateCurrencyList()
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
                sSQL = " SELECT  * " +
                    " FROM CurrencyDenominations";

                cmd.CommandType = CommandType.Text;
                cmd.CommandText = sSQL;
                cmd.Connection = conn;
                SqlDataAdapter sqlDa = (SqlDataAdapter)da;
                sqlDa.SelectCommand = (SqlCommand)cmd;
                da.Fill(ds);
                conn.Close();
                return ds;
            }
            catch (Exception exp)
            {
                conn.Close();
                throw (exp);
            }
            return null;
        }
        /// <summary>
        /// To Save The Close Station Cash Entry Details.
        /// </summary>
        /// <param name="CurrencyID"></param>
        /// <param name="count"></param>
        /// <param name="total"></param>
        public void SaveStationCloseCashDeatil(int CurrencyID, int count, Decimal total)
        {
            IDbConnection conn = DataFactory.CreateConnection();
            IDbTransaction tr = null;
            try
            {
                conn.ConnectionString = DBConfig.ConnectionString;
                conn.Open();
                tr = conn.BeginTransaction();
                string sSQL = "INSERT INTO StationCloseCash( " +
                     " StationCloseID " +
                     " , CurrencyDenomID " +
                     " , Count " +
                     " , TotalValue " +
                     " ) " +
                     " VALUES " +
                     " ('" + stationCloseID + "' " +
                     " , '" + CurrencyID + "' " +
                     " , '" + count + "' " +
                     " , '" + total + "' " +
                     " )";

                IDbCommand cmd = DataFactory.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = sSQL;
                cmd.Transaction = tr;
                cmd.Connection = conn;
                cmd.ExecuteNonQuery();
                tr.Commit();
                conn.Close();
            }
            catch (Exception exp)
            {
                tr.Rollback();
                conn.Close();
                throw (exp);
            }
        }

        /// <summary>
        /// Returns Total Cash Entry.
        /// </summary>
        /// <param name="stationCloseID"></param>
        /// <returns></returns>
        public decimal GetUserEnterStationCloseCash(int stationCloseID)
        {
            decimal TotalAmnt = 0;
            IDbConnection conn = DataFactory.CreateConnection();
            IDbTransaction tr = null;
            try
            {
                conn.ConnectionString = DBConfig.ConnectionString;
                conn.Open();
                tr = conn.BeginTransaction();
                string sSQL = @"SELECT SUM(TotalValue) as UserEnterCash FROM StationCloseCash
                     WHERE StationCloseID = '" + stationCloseID + "'";

                IDbCommand cmd = DataFactory.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = sSQL;
                cmd.Transaction = tr;
                cmd.Connection = conn;

                Object RetVal = cmd.ExecuteScalar();

                if (RetVal != DBNull.Value)
                {
                    TotalAmnt = Convert.ToDecimal(RetVal.ToString());
                }
                else
                {
                    return -1;
                }

                tr.Commit();
                conn.Close();
                return TotalAmnt;
            }
            catch (Exception Ex)
            {
                tr.Rollback();
                throw Ex;
            }
            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
            }
        }
        /// <summary>
        /// Fills DataSet With Details of Cash Entries For Given StaionCloseID
        /// </summary>
        /// <param name="stationCloseID"></param>
        /// <returns></returns>
        public DataSet GetStationCloseCashDetail(int stationCloseID)
        {
            IDbConnection conn = DataFactory.CreateConnection();
            DataSet ds = new DataSet();
            IDataAdapter da = DataFactory.CreateDataAdapter();
            try
            {
                conn.ConnectionString = DBConfig.ConnectionString;
                conn.Open();
                string sSQL = @"SELECT * FROM StationCloseCash
                     WHERE StationCloseID = '" + stationCloseID + "'";

                IDbCommand cmd = DataFactory.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = sSQL;
                cmd.Connection = conn;
                SqlDataAdapter sqlDa = (SqlDataAdapter)da;
                sqlDa.SelectCommand = (SqlCommand)cmd;
                da.Fill(ds);
                conn.Close();
                return ds;
            }
            catch (Exception exp)
            {
                conn.Close();
                throw (exp);
            }
            return null;
        }

        /// <summary>
        /// Save The Verified Close Staion Cash Details.
        /// </summary>
        /// <param name="stationCloseID"></param>
        /// <param name="CurrencyID"></param>
        /// <param name="count"></param>
        /// <param name="VerifiedTotal"></param>
        public void VerifyStationCloseCash(int stationCloseID, int CurrencyID, int count, decimal VerifiedTotal)
        {
            IDbConnection conn = DataFactory.CreateConnection();
            IDbTransaction tr = null;
            bool isVerified = true;
            try
            {
                //Modified by shitaljit on 3dec2013 to print verified total once manager has verified the station close 
                //chnage the Count parameter to VerifiedCount
                conn.ConnectionString = DBConfig.ConnectionString;
                conn.Open();
                tr = conn.BeginTransaction();
                string sSQL = "UPDATE StationCloseCash SET " +
                               "VerifiedCount =" + count +
                               ",IsVerified =" + Configuration.convertBoolToInt(isVerified) +
                               ",VerifiedBy ='" + Configuration.UserName + "'" +
                               ",VerifiedTotalValue =" + VerifiedTotal +
                               ",VerifiedDate = '" + System.DateTime.Now + "' " +
                               "WHERE StationCloseID =" + stationCloseID + " AND CurrencyDenomID=" + CurrencyID;

                IDbCommand cmd = DataFactory.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = sSQL;
                cmd.Transaction = tr;
                cmd.Connection = conn;
                cmd.ExecuteNonQuery();
                tr.Commit();
                tr = null;
                conn.Close();
            }
            catch (Exception exp)
            {
                if (tr != null)
                {
                    tr.Rollback();
                }
                conn.Close();
                throw (exp);
            }
        }

        /// <summary>
        /// Update StaionCloseHeader Table with Verified Cash Details.
        /// </summary>
        /// <param name="StationCloseID"></param>
        /// <param name="VerifiedAmt"></param>
        public void UpdateMaster(int StationCloseID, decimal VerifiedAmt)
        {
            IDbConnection conn = DataFactory.CreateConnection();
            IDbTransaction tr = null;

            try
            {
                conn.ConnectionString = DBConfig.ConnectionString;
                conn.Open();
                tr = conn.BeginTransaction();
                string sSQL = "UPDATE StationCloseHeader SET " +
                               "VerifiedBy ='" + Configuration.UserName + "'" +
                               ",VerifiedDate ='" + System.DateTime.Now + "'" +
                               ",VerifiedCashAmt =" + VerifiedAmt +
                               "WHERE StationCloseID =" + StationCloseID;

                IDbCommand cmd = DataFactory.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = sSQL;
                cmd.Transaction = tr;
                cmd.Connection = conn;
                cmd.ExecuteNonQuery();
                tr.Commit();
                tr = null;
                conn.Close();
            }
            catch (Exception exp)
            {
                if (tr != null)
                {
                    tr.Rollback();
                }
                conn.Close();
                throw (exp);
            }
        }
        //End of added by Shitaljit.

        #region Sprint-24 - PRIMEPOS-2326 20-Oct-2016 JY Added
        public DataSet GetDepartmentDS(int iStationCloseId)
        {
            try
            {
                using (IDbConnection conn = DataFactory.CreateConnection())
                {
                    conn.ConnectionString = DBConfig.ConnectionString;

                    string sSQL = "SELECT isnull(d.deptcode, ' ') as deptcode, isnull(d.deptname, ' ') as deptname "
                    + " ,  Sum(CASE T.TRANSTYPE WHEN 1 THEN ExtendedPrice ELSE 0 END) TotalSale  "
                    + " , Sum(CASE T.TRANSTYPE WHEN 2 THEN ExtendedPrice ELSE 0 END) TotalReturn  "
                    + " ,cast(-Sum(isnull(TD.Discount,0)) as float) TotalDiscount, Sum(isnull(TD.TaxAmount,0)) TotalTax FROM POSTransaction T "
                    + " INNER JOIN POSTransactionDetail TD ON TD.TransID = T.TransID "
                    + " INNER JOIN Item i ON TD.ItemID = i.ItemID "
                    + " LEFT JOIN department d on i.departmentid = d.deptid "
                    + " WHERE T.StClosedID =" + iStationCloseId + " GROUP BY d.deptcode, d.deptname order by d.deptname";

                    DataSet ds = new DataSet();
                    ds = DataHelper.ExecuteDataset(conn, CommandType.Text, sSQL);
                    return ds;
                }
            }
            catch (POSExceptions ex)
            {
                throw (ex);
            }
            catch (OtherExceptions ex)
            {
                throw (ex);
            }
            catch (Exception ex)
            {
                ErrorHandler.throwException(ex, "", "");
                return null;
            }
        }
        #endregion
    }
}