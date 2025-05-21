// ----------------------------------------------------------------
// Library: Data Access
// Author: Adeel Shehzad.
// Company: D-P-S, Inc. (www.d-p-s.com)
//
// ----------------------------------------------------------------
using System;
using System.Data;
using System.Collections;
using POS_Core.CommonData.Tables;
using POS_Core.CommonData.Rows;
using POS_Core.CommonData;
using Resources;
using System.Windows.Forms;
using POS_Core.ErrorLogging;
using NLog;

namespace POS_Core.DataAccess
{

    
    // Provides data access methods for DeptCode
    public class TransDetailRXSvr : IDisposable
    {
        private static ILogger logger = LogManager.GetCurrentClassLogger();

        #region Persist Methods
       // Inserts, updates or deletes rows in a DataSet, within a database transaction.
        public void Persist(TransDetailRXData updates, IDbTransaction tx)
        {
            try
            {
                //this.Delete(updates, tx);
                this.Insert(updates, tx);
                //this.Update(updates, tx);
            }
            catch (POSExceptions ex)
            {
                logger.Fatal(ex, "Persist(TransDetailRXData updates, IDbTransaction tx)");
                throw (ex);
            }
            catch (OtherExceptions ex)
            {
                logger.Fatal(ex, "Persist(TransDetailRXData updates, IDbTransaction tx)");
                throw (ex);
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "Persist(TransDetailRXData updates, IDbTransaction tx)");
                POS_Core.ErrorLogging.ErrorHandler.throwException(ex, "", "");
            }
        }

        public void PutOnHold(TransDetailRXData updates, IDbTransaction tx)
        {
            try
            {
                //this.DeleteOnHold(updates, tx);
                this.InsertOnHold(updates, tx);
                //this.UpdateOnHold(updates, tx,TransDetailID);
            }
            catch (POSExceptions ex)
            {
                logger.Fatal(ex, "PutOnHold(TransDetailRXData updates, IDbTransaction tx)");
                throw (ex);
            }
            catch (OtherExceptions ex)
            {
                logger.Fatal(ex, "PutOnHold(TransDetailRXData updates, IDbTransaction tx)");
                throw (ex);
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "PutOnHold(TransDetailRXData updates, IDbTransaction tx)");
                ErrorLogging.ErrorHandler.throwException(ex, "", "");
            }
        }
        #endregion

        #region Insert, Update, and Delete Methods
        public void Insert(TransDetailRXData ds, IDbTransaction tx)
        {
            TransDetailRXTable addedTable = ds.TransDetailRX;
            //.GetChanges(DataRowState.Added);
            string sSQL;
            IDbDataParameter[] insParam;

            if (addedTable != null && addedTable.Rows.Count > 0)
            {
                foreach (TransDetailRXRow row in addedTable.Rows)
                {
                    try
                    {
                        insParam = InsertParameters(row);
                        sSQL = BuildInsertSQL(clsPOSDBConstants.TransDetailRX_tbl, insParam);

                        DataHelper.ExecuteNonQuery(tx, CommandType.Text, sSQL, insParam);

                    }
                    catch (POSExceptions ex)
                    {
                        logger.Fatal(ex, "Insert(TransDetailRXData ds, IDbTransaction tx)");
                        throw (ex);
                    }
                    catch (OtherExceptions ex)
                    {
                        logger.Fatal(ex, "Insert(TransDetailRXData ds, IDbTransaction tx)");
                        throw (ex);
                    }

                    catch (Exception ex)
                    {
                        logger.Fatal(ex, "Insert(TransDetailRXData ds, IDbTransaction tx)");
                        ErrorHandler.throwException(ex, "", "");
                    }
                }
                addedTable.AcceptChanges();
            }
        }

        //Added by krishna on 28 july 2011
        public static bool ProcOnHoldFlag = false;
        //till here added by Krishna on 28 July 2011

        //Added By SRT (Sachin) date : 06 Feb 2010
        //Added TransID parameter by shitaljit on 3 arpil 2012
        public bool RxIsOnHold(string RXNumber, string PatientNo,out string StationID,out string UserID, out string TransID, IDbTransaction conn)
        {
            bool isExist = false;
            StationID = string.Empty;
            UserID = string.Empty;
            TransID = string.Empty;
            try
            {
                DataTable RxOnHold = null;
                DataTable POSTransaction_OnHold = null;
                TransDetailData oHoldTransDData1 = new TransDetailData();
               
                //RxOnHold = DataHelper.ExecuteDataset(conn, CommandType.Text, "Select * FROM " + clsPOSDBConstants.TransDetailRX_OnHold_tbl + " WHERE " + clsPOSDBConstants.TransDetailRX_Fld_RXNo + " = '" + RXNumber + "' and " + clsPOSDBConstants.TransDetailRX_Fld_PatientNo + " = '" + PatientNo + "'").Tables[0];;
                //oHoldTransDData1.TransDetail 
                RxOnHold = DataHelper.ExecuteDataset(conn, CommandType.Text, "Select * FROM " + clsPOSDBConstants.TransDetail_OnHold_tbl).Tables[0];
                //isExist = IsRecordsExist(RxOnHold);

                //foreach (TransDetailRow oRow in oHoldTransDData1.TransDetail.Rows)
                foreach (DataRow oRow in RxOnHold.Rows)
                {

                    //this.oTransDData.TransDetail.Rows.Add(oRow.ItemArray);
                    if (oRow["ItemID"].ToString().ToUpper() == "RX")
                    {
                        if (oRow["ItemDescription"].ToString().IndexOf("-") > 0)
                        {
                            //illRXInformation(oRow.ItemDescription.Substring(0, oRow.ItemDescription.IndexOf("-")).Trim(), true);

                            //Following Orignal Commented by Krishna on 28 July 2011
                            //if (oRow["ItemDescription"].ToString().Substring(0, oRow["ItemDescription"].ToString().IndexOf("-")).Trim() == RXNumber)//Orignal Commented by Krishna on 28 July 2011
                            if (oRow["ItemDescription"].ToString().Substring(0, oRow["ItemDescription"].ToString().IndexOf("-")).Trim() == RXNumber && !ProcOnHoldFlag)//Flag condition added to verify that the call is for Proccessing On-Hold Trans.
                            {
                                POSTransaction_OnHold = DataHelper.ExecuteDataset(conn, CommandType.Text, "Select * FROM " + clsPOSDBConstants.TransHeader_OnHold_tbl+" where TransId = "+oRow["TransId"].ToString()).Tables[0];
                                if (POSTransaction_OnHold.Rows.Count > 0)
                                {
                                    StationID = POSTransaction_OnHold.Rows[0]["StationID"].ToString();
                                    UserID = POSTransaction_OnHold.Rows[0]["UserID"].ToString();
                                    TransID = POSTransaction_OnHold.Rows[0]["TransID"].ToString();
                                }
                                isExist = true;
                                break;
                            }
                            else
                                isExist = false;
                        }
                    }

                }
                return isExist;
            }
            catch (POSExceptions ex)
            {
                logger.Fatal(ex, "RxIsOnHold(string RXNumber, string PatientNo,out string StationID,out string UserID, out string TransID, IDbTransaction conn)");
                throw (ex);
            }
            catch (OtherExceptions ex)
            {
                logger.Fatal(ex, "RxIsOnHold(string RXNumber, string PatientNo,out string StationID,out string UserID, out string TransID, IDbTransaction conn)");
                throw (ex);
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "RxIsOnHold(string RXNumber, string PatientNo,out string StationID,out string UserID, out string TransID, IDbTransaction conn)");
                ErrorHandler.throwException(ex, "", "");
                return false;
            }
        }

        #region Sprint-21 04-Feb-2016 JY added code to fix - SCENARIO: If you put some rxs for a patient on hold. Then if you enter a different rx for the same patient on the pos the unpicked rx search feature has some problem.
        public bool RxIsOnHold(string PatientNo, out DataTable dtRxOnHold, IDbConnection conn)
        {
            bool isExist = false;
            dtRxOnHold = null;
            try
            {
                TransDetailData oHoldTransDData1 = new TransDetailData();

                string strSQL = "SELECT b.TransID, a.RXNO, a.NRefill FROM POSTransactionRXDetail_OnHold a " +
                            " INNER JOIN POSTransactionDetail_OnHold b ON a.TransDetailID = b.TransDetailID " +
                            " WHERE a.PatientNo = " + PatientNo + " ORDER BY a.RXNO, a.NRefill";

                dtRxOnHold = DataHelper.ExecuteDataset(conn, CommandType.Text, strSQL).Tables[0];
                if (dtRxOnHold.Rows.Count > 0) isExist = true;
                return isExist;
            }
            catch (POSExceptions ex)
            {
                logger.Fatal(ex, "RxIsOnHold(string PatientNo, out DataTable dtRxOnHold, IDbConnection conn)");
                throw (ex);
            }
            catch (OtherExceptions ex)
            {
                logger.Fatal(ex, "RxIsOnHold(string PatientNo, out DataTable dtRxOnHold, IDbConnection conn)");
                throw (ex);
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "RxIsOnHold(string PatientNo, out DataTable dtRxOnHold, IDbConnection conn)");
                ErrorHandler.throwException(ex, "", "");
                return false;
            }
        }
        #endregion

        #region PRIMEPOS-3248
        public bool RxIsOnHoldForPrimeRxPayment(string PatientNo, out DataTable dtRxOnHoldForPrimeRxPayment, IDbConnection conn)
        {
            bool isExist = false;
            dtRxOnHoldForPrimeRxPayment = null;
            try
            {
                TransDetailData oHoldTransDData1 = new TransDetailData();

                string strSQL = "SELECT b.TransID, a.RXNO, a.NRefill FROM POSTransactionRXDetail_OnHold a " +
                            " INNER JOIN POSTransactionDetail_OnHold b ON a.TransDetailID = b.TransDetailID " +
                            "INNER JOIN POSTransaction_OnHold PTD on PTD.TransID =b.TransID " +
                            " WHERE a.PatientNo = " + PatientNo + " and PTD.IsPaymentPending=1 ORDER BY a.RXNO, a.NRefill";

                dtRxOnHoldForPrimeRxPayment = DataHelper.ExecuteDataset(conn, CommandType.Text, strSQL).Tables[0];
                if (dtRxOnHoldForPrimeRxPayment.Rows.Count > 0) isExist = true;
                return isExist;
            }
            catch (POSExceptions ex)
            {
                logger.Fatal(ex, "RxIsOnHoldForPrimeRxPayment(string PatientNo, out DataTable dtRxOnHoldForPrimeRxPayment, IDbConnection conn)");
                throw (ex);
            }
            catch (OtherExceptions ex)
            {
                logger.Fatal(ex, "RxIsOnHoldForPrimeRxPayment(string PatientNo, out DataTable dtRxOnHoldForPrimeRxPayment, IDbConnection conn)");
                throw (ex);
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "RxIsOnHoldForPrimeRxPayment(string PatientNo, out DataTable dtRxOnHoldForPrimeRxPayment, IDbConnection conn)");
                ErrorHandler.throwException(ex, "", "");
                return false;
            }
        }
        #endregion

        private bool IsRecordsExist(DataTable dtOfItem)
        {
            bool flag = false;
            try
            {
                if (dtOfItem.Rows.Count > 0)
                    flag = true;
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "IsRecordsExist(DataTable dtOfItem)");
                ErrorHandler.throwException(ex, "", "");
                return false;
            }
            return flag;
        }
        //End of Added By SRT (Sachin) date : 06 Feb 2010

        public void InsertOnHold(TransDetailRXData ds, IDbTransaction tx)
        {
            TransDetailRXTable addedTable = (TransDetailRXTable)ds.TransDetailRX;
            //.GetChanges(DataRowState.Added);
            string sSQL;
            IDbDataParameter[] insParam;

            if (addedTable != null && addedTable.Rows.Count > 0)
            {
                foreach (TransDetailRXRow row in addedTable.Rows)
                {
                    if (row.RowState == DataRowState.Deleted) continue;
                    try
                    {
                        insParam = InsertParameters(row);
                        sSQL = BuildInsertSQL(clsPOSDBConstants.TransDetailRX_OnHold_tbl, insParam);

                        DataHelper.ExecuteNonQuery(tx, CommandType.Text, sSQL, insParam);

                    }
                    catch (POSExceptions ex)
                    {
                        logger.Fatal(ex, "InsertOnHold(TransDetailRXData ds, IDbTransaction tx)");
                        throw (ex);
                    }
                    catch (OtherExceptions ex)
                    {
                        logger.Fatal(ex, "InsertOnHold(TransDetailRXData ds, IDbTransaction tx)");
                        throw (ex);
                    }
                    catch (Exception ex)
                    {
                        logger.Fatal(ex, "InsertOnHold(TransDetailRXData ds, IDbTransaction tx)");
                        ErrorHandler.throwException(ex, "", "");
                    }
                }
                addedTable.AcceptChanges();
            }
        }
        
        private string BuildInsertSQL(string tableName, IDbDataParameter[] delParam)
        {
            string sInsertSQL = "INSERT INTO " + tableName + " ( ";
            // build where clause
            sInsertSQL = sInsertSQL + delParam[0].SourceColumn;

            for (int i = 1; i < delParam.Length; i++)
            {
                sInsertSQL = sInsertSQL + " , " + delParam[i].SourceColumn;
            }
            sInsertSQL = sInsertSQL + " ) Values (" + delParam[0].ParameterName;

            for (int i = 1; i < delParam.Length; i++)
            {
                sInsertSQL = sInsertSQL + " , " + delParam[i].ParameterName;
            }
            sInsertSQL = sInsertSQL + " )";
            return sInsertSQL;
        }

        public void DeleteOnHold(Int32 TransID, IDbTransaction tx)
        {
            //string sSQL;
            try
            {
                //delParam = PKParameters(row);

                //sSQL = BuildDeleteOnHoldSQL(clsPOSDBConstants.TransDetailRX_OnHold_tbl, TransDetailID);
                string sSQL = "delete "
                    + " FROM "
                    + clsPOSDBConstants.TransDetailRX_OnHold_tbl + " where TransDetailID In ( "
                    + " Select TransDetailID from " +clsPOSDBConstants.TransDetail_OnHold_tbl + " td inner join "
                    + clsPOSDBConstants.TransHeader_OnHold_tbl + " th on (th.TransID=td.TransID) "
                    + " where th." + clsPOSDBConstants.TransHeader_Fld_TransID + " = " + TransID+")";
                DataHelper.ExecuteNonQuery(tx, CommandType.Text, sSQL);
            }
            catch (POSExceptions ex)
            {
                logger.Fatal(ex, "DeleteOnHold(Int32 TransID, IDbTransaction tx)");
                throw (ex);
            }
            catch (OtherExceptions ex)
            {
                logger.Fatal(ex, "DeleteOnHold(Int32 TransID, IDbTransaction tx)");
                throw (ex);
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "DeleteOnHold(Int32 TransID, IDbTransaction tx)");
                ErrorHandler.throwException(ex, "", "");
            }
        }

        public void Delete(Int32 TransID, IDbTransaction tx)
        {
            //string sSQL;
            try
            {
                //delParam = PKParameters(row);

                //sSQL = BuildDeleteOnHoldSQL(clsPOSDBConstants.TransDetailRX_OnHold_tbl, TransDetailID);
                string sSQL = "delete "
                    + " FROM "
                    + clsPOSDBConstants.TransDetailRX_tbl + " where TransDetailID In ( "
                    + " Select TransDetailID from " + clsPOSDBConstants.TransDetail_tbl + " td inner join "
                    + clsPOSDBConstants.TransHeader_tbl + " th on (th.TransID=td.TransID) "
                    + " where th." + clsPOSDBConstants.TransHeader_Fld_TransID + " = " + TransID + ")";
                DataHelper.ExecuteNonQuery(tx, CommandType.Text, sSQL);
            }
            catch (POSExceptions ex)
            {
                logger.Fatal(ex, "Delete(Int32 TransID, IDbTransaction tx))");
                throw (ex);
            }
            catch (OtherExceptions ex)
            {
                logger.Fatal(ex, "Delete(Int32 TransID, IDbTransaction tx))");
                throw (ex);
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "Delete(Int32 TransID, IDbTransaction tx))");
                ErrorHandler.throwException(ex, "", "");
            }
        }

        private string BuildDeleteSQL(string tableName, TransDetailRXRow row)
        {
            string sDeleteSQL = "DELETE FROM " + tableName + " WHERE ";
            sDeleteSQL += clsPOSDBConstants.TransDetailRX_Fld_RXDetailID + " = " + row.RXDetailID.ToString();
            return sDeleteSQL;
        }

        private string BuildDeleteOnHoldSQL(string tableName, Int32 TransDetailID)
        {
            string sDeleteSQL = "DELETE FROM " + tableName + " WHERE ";
            sDeleteSQL += clsPOSDBConstants.TransDetailRX_Fld_TransDetailID + " = " + TransDetailID.ToString();
            return sDeleteSQL;
        }
        
        public void UpdateOnHold(TransDetailRXData ds, IDbTransaction tx, System.Int32 OrderID)
        {
            TransDetailRXTable modifiedTable = (TransDetailRXTable)ds.TransDetailRX.GetChanges(DataRowState.Modified);

            string sSQL;
            IDbDataParameter[] updParam;

            if (modifiedTable != null && modifiedTable.Rows.Count > 0)
            {
                foreach (TransDetailRXRow row in modifiedTable.Rows)
                {
                    try
                    {
                        updParam = UpdateParameters(row);
                        sSQL = BuildUpdateSQL(clsPOSDBConstants.TransDetailRX_OnHold_tbl, updParam);

                        DataHelper.ExecuteNonQuery(tx, CommandType.Text, sSQL, updParam);
                    }
                    catch (POSExceptions ex)
                    {
                        logger.Fatal(ex, "UpdateOnHold(TransDetailRXData ds, IDbTransaction tx, System.Int32 OrderID)");
                        throw (ex);
                    }
                    catch (OtherExceptions ex)
                    {
                        logger.Fatal(ex, "UpdateOnHold(TransDetailRXData ds, IDbTransaction tx, System.Int32 OrderID)");
                        throw (ex);
                    }
                    catch (Exception ex)
                    {
                        logger.Fatal(ex, "UpdateOnHold(TransDetailRXData ds, IDbTransaction tx, System.Int32 OrderID)");
                        ErrorHandler.throwException(ex, "", "");
                    }
                }
                modifiedTable.AcceptChanges();
            }
        }

        private string BuildUpdateSQL(string tableName, IDbDataParameter[] delParam)
        {
            string sInsertSQL = "update " + tableName + " set ";
            // build where clause
            sInsertSQL = sInsertSQL + delParam[0].SourceColumn + "=" + delParam[0].ParameterName;

            for (int i = 1; i < delParam.Length - 1; i++)
            {
                sInsertSQL = sInsertSQL + " , " + delParam[i].SourceColumn + "=" + delParam[i].ParameterName;
            }
            sInsertSQL = sInsertSQL + " where " + delParam[delParam.Length - 1].SourceColumn + " = " + delParam[delParam.Length - 1].ParameterName;

            return sInsertSQL;
        }
        #endregion
        
        #region IDBDataParameter Generator Methods
        private IDbDataParameter[] whereParameters(string swhere)
        {
            IDbDataParameter[] sqlParams = DataFactory.CreateParameterArray(1);
            sqlParams[0] = DataFactory.CreateParameter();

            sqlParams[0].DbType = System.Data.DbType.String;
            sqlParams[0].Size = 2000;
            sqlParams[0].ParameterName = "@whereClause";

            sqlParams[0].Value = swhere;
            return (sqlParams);
        }
        
        private IDbDataParameter[] PKParameters(System.Int32 RXDetailID)
        {
            IDbDataParameter[] sqlParams = DataFactory.CreateParameterArray(1);

            sqlParams[0] = DataFactory.CreateParameter();
            sqlParams[0].ParameterName = "@RXDetailID";
            sqlParams[0].DbType = System.Data.DbType.Int32;
            sqlParams[0].Value = RXDetailID;

            return (sqlParams);
        }

        private IDbDataParameter[] PKParameters(TransDetailRXRow row)
        {
            //return a SqlParameterCollection
            IDbDataParameter[] sqlParams = DataFactory.CreateParameterArray(1);

            sqlParams[0] = DataFactory.CreateParameter();
            sqlParams[0].ParameterName = "@RXDetailID";
            sqlParams[0].DbType = System.Data.DbType.String;

            sqlParams[0].Value = row.RXDetailID;
            sqlParams[0].SourceColumn = clsPOSDBConstants.TransDetailRX_Fld_RXDetailID;

            return (sqlParams);
        }

        private IDbDataParameter[] InsertParameters(TransDetailRXRow row)
        {
            IDbDataParameter[] sqlParams = DataFactory.CreateParameterArray(12);

            sqlParams[0] = DataFactory.CreateParameter("@" + clsPOSDBConstants.TransDetailRX_Fld_TransDetailID, System.Data.DbType.Int32);
            sqlParams[1] = DataFactory.CreateParameter("@" + clsPOSDBConstants.TransDetailRX_Fld_RXNo, System.Data.DbType.Int64);
            sqlParams[2] = DataFactory.CreateParameter("@" + clsPOSDBConstants.TransDetailRX_Fld_NRefill, System.Data.DbType.Int32);
            sqlParams[3] = DataFactory.CreateParameter("@" + clsPOSDBConstants.TransDetailRX_Fld_DateFilled, System.Data.DbType.Date);
            sqlParams[4] = DataFactory.CreateParameter("@" + clsPOSDBConstants.TransDetailRX_Fld_DrugNDC, System.Data.DbType.String);
            sqlParams[5] = DataFactory.CreateParameter("@" + clsPOSDBConstants.TransDetailRX_Fld_InsType, System.Data.DbType.String);
            sqlParams[6] = DataFactory.CreateParameter("@" + clsPOSDBConstants.TransDetailRX_Fld_PatientNo, System.Data.DbType.Int32);
            sqlParams[7] = DataFactory.CreateParameter("@" + clsPOSDBConstants.TransDetailRX_Fld_PatType, System.Data.DbType.String);
            // added by atul 07-jan-2011
            sqlParams[8] = DataFactory.CreateParameter("@" + clsPOSDBConstants.TransDetailRX_Fld_CounsellingReq, System.Data.DbType.String);
            sqlParams[9] = DataFactory.CreateParameter("@" + clsPOSDBConstants.TransDetailRX_Fld_Ezcap, System.Data.DbType.String);
            // end of added by atul 07-jan-2011
            //added by Manoj 1/2/2015
            sqlParams[10] = DataFactory.CreateParameter("@" + clsPOSDBConstants.TransDetail_Fld_ReturnTransDetailId, System.Data.DbType.Int32);
            sqlParams[11] = DataFactory.CreateParameter("@" + clsPOSDBConstants.TransDetailRX_Fld_PartialFillNo, System.Data.DbType.Int32);

            sqlParams[0].SourceColumn = clsPOSDBConstants.TransDetailRX_Fld_TransDetailID;
            sqlParams[1].SourceColumn = clsPOSDBConstants.TransDetailRX_Fld_RXNo;
            sqlParams[2].SourceColumn = clsPOSDBConstants.TransDetailRX_Fld_NRefill;
            sqlParams[3].SourceColumn = clsPOSDBConstants.TransDetailRX_Fld_DateFilled;
            sqlParams[4].SourceColumn = clsPOSDBConstants.TransDetailRX_Fld_DrugNDC;
            sqlParams[5].SourceColumn = clsPOSDBConstants.TransDetailRX_Fld_InsType;
            sqlParams[6].SourceColumn = clsPOSDBConstants.TransDetailRX_Fld_PatientNo;
            sqlParams[7].SourceColumn = clsPOSDBConstants.TransDetailRX_Fld_PatType;
            // added by atul 07-jan-2011
            sqlParams[8].SourceColumn = clsPOSDBConstants.TransDetailRX_Fld_CounsellingReq;
            sqlParams[9].SourceColumn = clsPOSDBConstants.TransDetailRX_Fld_Ezcap;
            // end of added by atul 07-jan-2011
            //added by Manoj 1/2/2015
            sqlParams[10].SourceColumn = clsPOSDBConstants.TransDetail_Fld_ReturnTransDetailId;
            sqlParams[11].SourceColumn = clsPOSDBConstants.TransDetailRX_Fld_PartialFillNo;

            if (row.TransDetailID.ToString() != System.String.Empty)
                sqlParams[0].Value = row.TransDetailID;
            else
                sqlParams[0].Value = DBNull.Value;

            sqlParams[1].Value = row.RXNo;
            sqlParams[2].Value = row.NRefill;

            sqlParams[3].Value = row.DateFilled;
            sqlParams[4].Value = row.DrugNDC;
            sqlParams[5].Value = row.InsType;
            sqlParams[6].Value = row.PatientNo;
            sqlParams[7].Value = row.PatType;
            // added by atul 07-jan-2011
            if (row.CounsellingReq.ToString() != System.String.Empty)
                sqlParams[8].Value = row.CounsellingReq;
            else
                sqlParams[8].Value = DBNull.Value;

            if (row.Ezcap.ToString() != System.String.Empty)
                sqlParams[9].Value = row.Ezcap;
            else
                sqlParams[9].Value = DBNull.Value;
            // end of added by atul 07-jan-2011
            //added by Manoj 1/2/2015
            sqlParams[10].Value = row.ReturnTransDetailID;
            sqlParams[11].Value = row.PartialFillNo;

            return (sqlParams);
        }

		private IDbDataParameter[] UpdateParameters(TransDetailRXRow row) 
		{
            IDbDataParameter[] sqlParams = DataFactory.CreateParameterArray(11);

            sqlParams[0] = DataFactory.CreateParameter("@" + clsPOSDBConstants.TransDetailRX_Fld_RXDetailID, System.Data.DbType.Int32);
            sqlParams[1] = DataFactory.CreateParameter("@" + clsPOSDBConstants.TransDetailRX_Fld_TransDetailID, System.Data.DbType.Int32);
            sqlParams[2] = DataFactory.CreateParameter("@" + clsPOSDBConstants.TransDetailRX_Fld_RXNo, System.Data.DbType.Int64);
            sqlParams[3] = DataFactory.CreateParameter("@" + clsPOSDBConstants.TransDetailRX_Fld_NRefill, System.Data.DbType.Int32);
            sqlParams[4] = DataFactory.CreateParameter("@" + clsPOSDBConstants.TransDetailRX_Fld_DateFilled, System.Data.DbType.Date);
            sqlParams[5] = DataFactory.CreateParameter("@" + clsPOSDBConstants.TransDetailRX_Fld_DrugNDC, System.Data.DbType.String);
            sqlParams[6] = DataFactory.CreateParameter("@" + clsPOSDBConstants.TransDetailRX_Fld_InsType, System.Data.DbType.String);
            sqlParams[7] = DataFactory.CreateParameter("@" + clsPOSDBConstants.TransDetailRX_Fld_PatientNo, System.Data.DbType.Int32);
            sqlParams[8] = DataFactory.CreateParameter("@" + clsPOSDBConstants.TransDetailRX_Fld_PatType, System.Data.DbType.String);
            // added by atul 07-jan-2011
            sqlParams[9] = DataFactory.CreateParameter("@" + clsPOSDBConstants.TransDetailRX_Fld_CounsellingReq, System.Data.DbType.String);
            sqlParams[10] = DataFactory.CreateParameter("@" + clsPOSDBConstants.TransDetailRX_Fld_Ezcap, System.Data.DbType.String);
            // end of added by atul 07-jan-2011
            //Added by Manoj 1/2/2015
            sqlParams[11] = DataFactory.CreateParameter("@" + clsPOSDBConstants.TransDetail_Fld_ReturnTransDetailId, System.Data.DbType.Int32);

            sqlParams[0].SourceColumn = clsPOSDBConstants.TransDetailRX_Fld_RXDetailID;
            sqlParams[1].SourceColumn = clsPOSDBConstants.TransDetailRX_Fld_TransDetailID;
            sqlParams[2].SourceColumn = clsPOSDBConstants.TransDetailRX_Fld_RXNo;
            sqlParams[3].SourceColumn = clsPOSDBConstants.TransDetailRX_Fld_NRefill;
            sqlParams[4].SourceColumn = clsPOSDBConstants.TransDetailRX_Fld_DateFilled;
            sqlParams[5].SourceColumn = clsPOSDBConstants.TransDetailRX_Fld_DrugNDC;
            sqlParams[6].SourceColumn = clsPOSDBConstants.TransDetailRX_Fld_InsType;
            sqlParams[7].SourceColumn = clsPOSDBConstants.TransDetailRX_Fld_PatientNo;
            sqlParams[8].SourceColumn = clsPOSDBConstants.TransDetailRX_Fld_PatType;
            // added by atul 07-jan-2011
            sqlParams[9].SourceColumn = clsPOSDBConstants.TransDetailRX_Fld_CounsellingReq;
            sqlParams[10].SourceColumn = clsPOSDBConstants.TransDetailRX_Fld_Ezcap;
            // end of added by atul 07-jan-2011
            //added by Manoj 1/2/2015
            sqlParams[11].SourceColumn = clsPOSDBConstants.TransDetail_Fld_ReturnTransDetailId;

            sqlParams[0].Value = row.RXDetailID;
            sqlParams[1].Value = row.TransDetailID;

            sqlParams[2].Value = row.RXNo;
            sqlParams[3].Value = row.NRefill;

            sqlParams[4].Value = row.DateFilled;
            sqlParams[5].Value = row.DrugNDC;
            sqlParams[6].Value = row.InsType;
            sqlParams[7].Value = row.PatientNo;
            sqlParams[8].Value = row.PatType;
            // added by atul 07-jan-2011
            sqlParams[9].Value = row.CounsellingReq;
            sqlParams[10].Value = row.Ezcap;
            // end of added by atul 07-jan-2011
            //added by Manoj 1/2/2015
            sqlParams[11].Value = row.ReturnTransDetailID;
            return (sqlParams);
		}
        #endregion

        #region Get Methods
        // Looks up a ItemVendor based on its primary-key:System.String VendorItemID
        public DataSet Populate(System.Int32 TransID, IDbConnection conn)
        {
            try
            {
                //change by atul in query 07-jan-2011
                string sSQL = "Select "
                    + " trx.*"
                    + " FROM "
                    + clsPOSDBConstants.TransDetailRX_tbl + " trx left join "
                    + clsPOSDBConstants.TransDetail_tbl + " td on (td.TransDetailID=trx.TransDetailID) left join "
                    + clsPOSDBConstants.TransHeader_tbl + " th on (th.TransID=td.TransID) "
                    + "where th." + clsPOSDBConstants.TransHeader_Fld_TransID + " = " + TransID;

	
				DataSet ds = new DataSet();
				ds=DataHelper.ExecuteDataset(conn, CommandType.Text, sSQL);
				return ds;
			}
            catch (Exception ex)
            {
                logger.Fatal(ex, "Populate(System.Int32 TransID, IDbConnection conn)");
                ErrorHandler.throwException(ex, "", "");
                return null;
            }
        }

        //2915
        public DataSet PopulateOnHold(System.Int32 TransID, IDbConnection conn)
        {
            try
            {
                //change by atul in query 07-jan-2011
                string sSQL = "Select "
                    + " trx.*"
                    + " FROM "
                    + clsPOSDBConstants.TransDetailRX_OnHold_tbl + " trx left join "
                    + clsPOSDBConstants.TransDetail_OnHold_tbl + " td on (td.TransDetailID=trx.TransDetailID) left join "
                    + clsPOSDBConstants.TransHeader_OnHold_tbl + " th on (th.TransID=td.TransID) "
                    + "where th." + clsPOSDBConstants.TransHeader_Fld_TransID + " = " + TransID;


                DataSet ds = new DataSet();
                ds = DataHelper.ExecuteDataset(conn, CommandType.Text, sSQL);
                return ds;
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "Populate(System.Int32 TransID, IDbConnection conn)");
                ErrorHandler.throwException(ex, "", "");
                return null;
            }
        }

        public DataSet Populate(System.String Rxno, IDbConnection conn)
        {
            try
            {
                string sSQL = "Select "
                    + " *"
                    + " FROM "
                    
                    + clsPOSDBConstants.TransDetailRX_tbl + "  "
                    + " where " + clsPOSDBConstants.TransHeader_Fld_RxNo + " = " + Rxno;

                DataSet ds = new DataSet();
                ds = DataHelper.ExecuteDataset(conn, CommandType.Text, sSQL);
                return ds;
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "Populate(System.String Rxno, IDbConnection conn)");
                ErrorHandler.throwException(ex, "", "");
                return null;
            }
        }

        public DataSet Populate(System.Int32 TransDetailID)
        {
            IDbConnection conn = DataFactory.CreateConnection();
            conn.ConnectionString = POS_Core.Resources.Configuration.ConnectionString;
            return (Populate(TransDetailID, conn));
        }
        public DataSet Populate(System.String RxNo)
        {
            IDbConnection conn = DataFactory.CreateConnection();
            conn.ConnectionString = POS_Core.Resources.Configuration.ConnectionString;
            return (Populate(RxNo, conn));
        }

        public DataSet Populate(System.String RxNo, string sRefill)
        {
            IDbConnection conn = DataFactory.CreateConnection();
            conn.ConnectionString = POS_Core.Resources.Configuration.ConnectionString;
            return (Populate(RxNo, conn));
        }

        public TransDetailRXData PopulateData(System.Int32 TransID)
        {
            IDbConnection conn = DataFactory.CreateConnection();
            conn.ConnectionString = POS_Core.Resources.Configuration.ConnectionString;
            DataSet ds = Populate(TransID, conn);

            TransDetailRXData oTD = new TransDetailRXData();
            oTD.TransDetailRX.MergeTable(ds.Tables[0]);
            return oTD;
        }

        public TransDetailRXData PopulateDataOnHold(System.Int32 TransID)
        {
            IDbConnection conn = DataFactory.CreateConnection();
            conn.ConnectionString = POS_Core.Resources.Configuration.ConnectionString;
            DataSet ds = PopulateOnHold(TransID, conn);

            TransDetailRXData oTD = new TransDetailRXData();
            oTD.TransDetailRX.MergeTable(ds.Tables[0]);
            return oTD;
        }

        public DataSet getOnHoldTransDetailRX(System.Int32 TransID, IDbConnection conn)
        {
            try
            {
                string sSQL = "Select "
                    + " trx.*" 
					+ " FROM "
                    + clsPOSDBConstants.TransDetailRX_OnHold_tbl + " trx left join "
                    + clsPOSDBConstants.TransDetail_OnHold_tbl + " td on (td.TransDetailID=trx.TransDetailID) left join "
                    + clsPOSDBConstants.TransHeader_OnHold_tbl + " th on (th.TransID=td.TransID) "
                    + " where th." + clsPOSDBConstants.TransHeader_Fld_TransID + " = " + TransID;

	
				DataSet ds = new DataSet();
				ds=DataHelper.ExecuteDataset(conn, CommandType.Text, sSQL);
				ds.AcceptChanges();
				return ds;
			} 
			catch(POSExceptions ex) 
			{
                logger.Fatal(ex, "getOnHoldTransDetailRX(System.Int32 TransID, IDbConnection conn)");
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

        public TransDetailRXData getOnHoldTransDetailRX(System.Int32 TransID)
        {
            IDbConnection conn = DataFactory.CreateConnection();
            conn.ConnectionString = POS_Core.Resources.Configuration.ConnectionString;
            DataSet ds = getOnHoldTransDetailRX(TransID, conn);

            TransDetailRXData oTD = new TransDetailRXData();
            oTD.TransDetailRX.MergeTable(ds.Tables[0]);
            return oTD;
        }
        public DataSet PopulateList(string sWhereClause)
        {
            IDbConnection conn = DataFactory.CreateConnection();
            conn.ConnectionString = POS_Core.Resources.Configuration.ConnectionString;
            try
            {
                return PopulateList(sWhereClause, conn);

            }
            catch (Exception Ex)
            {
                logger.Fatal(Ex, "PopulateList(string sWhereClause)");
                throw Ex;
            }
        }

        #region PRIMEPOS-2699 05-Aug-2019 JY Added
        public DataTable GetPOSTransactionRXDetailRecord(long RxNo, int RefillNo, int iPartialFillNo = 0)
        {
            IDbConnection conn = DataFactory.CreateConnection();
            conn.ConnectionString = POS_Core.Resources.Configuration.ConnectionString;
            try
            {
                string sSQL = "SELECT TOP 1 c.TransID, a.RXDetailID, a.TransDetailID, a.RXNo, a.NRefill, a.ReturnTransDetailId, c.TransType FROM POSTransactionRXDetail a " +
                             " INNER JOIN POSTransactionDetail b ON a.TransDetailID = b.TransDetailID " +
                             " INNER JOIN POSTransaction c ON b.TransID = c.TransID ";
                if (iPartialFillNo == 0)
                    sSQL += " WHERE a.RXNO = " + RxNo + " AND a.NREFILL = " + RefillNo + " ORDER BY a.RXDetailID DESC";  // no partial fill
                else
                    sSQL += " WHERE a.RXNO = " + RxNo + " AND a.NREFILL = " + RefillNo + " AND a.PartialFillNo=" + iPartialFillNo.ToString() + " ORDER BY a.RXDetailID DESC";

                DataTable dt = new DataTable();
                dt = DataHelper.ExecuteDataTable(conn, CommandType.Text, sSQL);
                return dt;
            }
            catch (POSExceptions ex)
            {
                logger.Fatal(ex, "GetPOSTransactionRXDetailRecord(long RxNo, int RefillNo)");
                throw (ex);
            }
            catch (OtherExceptions ex)
            {
                logger.Fatal(ex, "GetPOSTransactionRXDetailRecord(long RxNo, int RefillNo)");
                throw (ex);
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "GetPOSTransactionRXDetailRecord(long RxNo, int RefillNo)");
                ErrorHandler.throwException(ex, "", "");
                return null;
            }
        }

        public DataTable GetReturnedRxTransDetails(long RxNo, int RefillNo)
        {
            IDbConnection conn = DataFactory.CreateConnection();
            conn.ConnectionString = POS_Core.Resources.Configuration.ConnectionString;
            try
            {
                string sSQL = "SELECT TOP 1 c.TransID, c.TransDate, c.UserID, c.StationID, x.PayTypes, a.RXDetailID, a.RXNo, a.NRefill FROM POSTransactionRXDetail a" +
                            " INNER JOIN POSTransactionDetail b ON a.TransDetailID = b.TransDetailID" +
                            " INNER JOIN POSTransaction c ON b.TransID = c.TransID" +
                            " INNER JOIN (SELECT a.TransID, " +
                                          " PayTypes = STUFF((SELECT DISTINCT ',' + b.PayTypeDesc FROM PayType b"+
                                                                " INNER JOIN POSTransPayment aa ON aa.TransTypeCode = b.PayTypeID WHERE aa.TransID = a.TransID  ORDER BY 1" +
                                                            " FOR XML PATH(''), TYPE).value('.', 'NVARCHAR(MAX)'), 1, 1, '')" +
                                                            " FROM POSTransPayment a Group by a.TransID ) x ON x.TransID = c.TransID" +
                            " WHERE a.RXNO = " + RxNo + " AND a.NREFILL = " + RefillNo + 
                            " ORDER BY a.RXDetailID DESC";

                DataTable dt = new DataTable();
                dt = DataHelper.ExecuteDataTable(conn, CommandType.Text, sSQL);
                return dt;
            }
            catch (POSExceptions ex)
            {
                logger.Fatal(ex, "GetReturnedRxTransDetails(long RxNo, int RefillNo)");
                throw (ex);
            }
            catch (OtherExceptions ex)
            {
                logger.Fatal(ex, "GetReturnedRxTransDetails(long RxNo, int RefillNo)");
                throw (ex);
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "GetReturnedRxTransDetails(long RxNo, int RefillNo)");
                ErrorHandler.throwException(ex, "", "");
                return null;
            }
        }
        #endregion

        public DataSet PopulateList(string sWhereClause, IDbConnection conn)
        {
            try
            {

                string sSQL1 = "Select  * "
                   
                    + " FROM "
                    + clsPOSDBConstants.TransDetailRX_tbl;

                string sSQL = String.Concat(sSQL1, sWhereClause);
                DataSet ds = new DataSet();
                ds = DataHelper.ExecuteDataset(conn, CommandType.Text, sSQL, whereParameters(sWhereClause));
                return ds;
            }
            catch (POSExceptions ex)
            {
                logger.Fatal(ex, "PopulateList(string sWhereClause, IDbConnection conn)");
                throw (ex);
            }
            catch (OtherExceptions ex)
            {
                logger.Fatal(ex, "PopulateList(string sWhereClause, IDbConnection conn)");
                throw (ex);
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "PopulateList(string sWhereClause, IDbConnection conn)");
                ErrorHandler.throwException(ex, "", "");
                return null;
            }
        }

        /*public DataSet PopulateList(string whereClause)
        {
            IDbConnection conn = DataFactory.CreateConnection();
            conn.ConnectionString = POS_Core.Resources.Configuration.ConnectionString;

            return (PopulateList(whereClause, conn));
        }
         * */

        #region Sprint-26 - PRIMEPOS-2418 02-Aug-2017 JY Added
        public DataSet GetSaleTransDetailWithNoReturnLinked(string sRxNo, string sRefillNo)
        {
            string sSQL = string.Empty;
            try
            {
                IDbConnection conn = DataFactory.CreateConnection();
                conn.ConnectionString = POS_Core.Resources.Configuration.ConnectionString;
                sSQL = "SELECT  a.*  FROM POSTransactionRXDetail a " +
                    " INNER JOIN POSTransactionDetail b on a.TransDetailID = b.TransDetailID " +
                    " INNER JOIN POSTransaction c on c.TransID = b.TransID " +
                    " WHERE c.TransType = 1 and a.RXNO = " + sRxNo + " AND a.NREFILL = " + sRefillNo +
                    " AND a.TransDetailID NOT IN (SELECT ReturnTransDetailId FROM POSTransactionRXDetail WHERE RXNO = " + sRxNo + " AND NREFILL = " + sRefillNo + " AND ReturnTransDetailId > 0)";

                DataSet ds = new DataSet();
                ds = DataHelper.ExecuteDataset(conn, CommandType.Text, sSQL);
                return ds;
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "GetTransDetail(System.Int32 TransId, IDbConnection conn)");
                ErrorHandler.throwException(ex, "", "");
                return null;
            }
        }
        #endregion

        #region BatchDelivery - NileshJ - PRIMERX-7688 25-Sept-2019
        public bool RxIsOnHoldForDelivery(string PatientNo, string RxNo, out DataTable dtRxOnHold)
        {
            bool isExist = false;
            dtRxOnHold = null;
            try
            {
                string strSQL = "SELECT b.TransID, a.RXNO, a.NRefill FROM POSTransactionRXDetail_OnHold a " +
                            " INNER JOIN POSTransactionDetail_OnHold b ON a.TransDetailID = b.TransDetailID " +
                            " WHERE a.PatientNo = " + PatientNo + " and a.RXNO = " + RxNo + " ORDER BY a.RXNO, a.NRefill";

                dtRxOnHold = DataHelper.ExecuteDataset(strSQL).Tables[0];
                if (dtRxOnHold.Rows.Count > 0)
                {
                    isExist = true;
                }
            }
            catch (POSExceptions ex)
            {
                logger.Fatal(ex, "RxIsOnHoldForDelivery(string PatientNo, out DataTable dtRxOnHold, IDbConnection conn)");
                isExist = false;
                throw (ex);
            }
            catch (OtherExceptions ex)
            {
                logger.Fatal(ex, "RxIsOnHoldForDelivery(string PatientNo, out DataTable dtRxOnHold, IDbConnection conn)");
                isExist = false;
                throw (ex);
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "RxIsOnHoldForDelivery(string PatientNo, out DataTable dtRxOnHold, IDbConnection conn)");
                ErrorHandler.throwException(ex, "", "");
                isExist = false;
            }
            return isExist;
        }
        #endregion

        #endregion //Get Method


        public void Dispose() { }

        //PRIMEPOS-2927
        public DataTable GetShippingRecord(string TransID)
        {
            IDbConnection conn = DataFactory.CreateConnection();
            conn.ConnectionString = POS_Core.Resources.Configuration.ConnectionString;
            try
            {
                string sSQL = "SELECT TOP 1 * FROM POSTRANSACTIONDETAIL (nolock) WHERE TRANSID = '" + TransID + "' AND ITEMID = '" + Resources.Configuration.ShippingItem + "'";

                DataTable dt = new DataTable();
                dt = DataHelper.ExecuteDataTable(conn, CommandType.Text, sSQL);
                return dt;
            }
            catch (POSExceptions ex)
            {
                logger.Fatal(ex, "GetShippingRecord(long RxNo, int RefillNo)");
                throw (ex);
            }
            catch (OtherExceptions ex)
            {
                logger.Fatal(ex, "GetShippingRecord(long RxNo, int RefillNo)");
                throw (ex);
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "GetShippingRecord(long RxNo, int RefillNo)");
                ErrorHandler.throwException(ex, "", "");
                return null;
            }
        }
    }


}
