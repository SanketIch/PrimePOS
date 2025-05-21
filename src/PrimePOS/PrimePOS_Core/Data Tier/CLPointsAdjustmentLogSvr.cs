using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using POS_Core.CommonData.Tables;
using POS_Core.CommonData.Rows;
using POS_Core.CommonData;
using Resources;
using System.Windows.Forms;
using POS_Core.ErrorLogging;
using POS_Core.Resources;
//using POS.Resources;
using NLog;

namespace POS_Core.DataAccess
{
	// Provides data access methods for DeptCode
    public class CLPointsAdjustmentLogSvr : IDisposable
    {
        private static ILogger logger = LogManager.GetCurrentClassLogger();

        #region Persist Methods

        // Inserts, updates or deletes rows in a DataSet, within a database transaction.
        public void Persist(DataSet updates, IDbTransaction tx)
        {
            try
            {
                this.Delete(updates, tx);
                this.Insert(updates, tx);
                this.Update(updates, tx);

                updates.AcceptChanges();
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
                logger.Fatal(ex, "Persist(DataSet updates, IDbTransaction tx)");  //PRIMEPOS-2971 07-Jun-2021 JY Added
                //ErrorLogging.ErrorHandler.throwException(ex, "", "");   //PRIMEPOS-2971 07-Jun-2021 JY Commented as no need to log it in errorlog
            }
        }

        // Inserts, updates or deletes rows in a DataSet.
        public void Persist(DataSet updates)
        {

            IDbTransaction tx = null;
            try
            {
                IDbConnection conn = DataFactory.CreateConnection(POS_Core.Resources.Configuration.ConnectionString);
                tx = conn.BeginTransaction();
                this.Persist(updates, tx);
                
                tx.Commit();
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
                tx.Rollback();
                logger.Fatal(ex, "Persist(DataSet updates)");  //PRIMEPOS-2971 07-Jun-2021 JY Added
                //ErrorHandler.throwException(ex, "", "");  //PRIMEPOS-2971 07-Jun-2021 JY Commented as no need to log it in errorlog
            }

        }

        public bool DeleteRow(string CurrentID)
        {
            string sSQL;
            try
            {
                sSQL = " delete from CL_PointsAdjustmentLog where ID= '" + CurrentID + "'";
                DataHelper.ExecuteNonQuery(DBConfig.ConnectionString, CommandType.Text, sSQL);
                return true;
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "DeleteRow(string CurrentID)");  //PRIMEPOS-2971 07-Jun-2021 JY Added
                //ErrorHandler.throwException(ex, "", "");  //PRIMEPOS-2971 07-Jun-2021 JY Commented as no need to log it in errorlog
                return false;
            }
        }

        #endregion

        #region Get Methods

        public CLPointsAdjustmentLogData Populate(System.Int64 ID, IDbConnection conn)
        {
            try
            {
                string sSQL = "Select * "
                                + " FROM "
                                    + clsPOSDBConstants.CLPointsAdjustmentLog_tbl
                                + " WHERE " + clsPOSDBConstants.CLPointsAdjustmentLog_Fld_ID + " =" + ID + "";


                CLPointsAdjustmentLogData ds = new CLPointsAdjustmentLogData();
                ds.CLPointsAdjustmentLog.MergeTable(DataHelper.ExecuteDataset(conn, CommandType.Text
                                            , sSQL
                                            , PKParameters(ID)).Tables[0]);
                return ds;
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
                logger.Fatal(ex, "Populate(System.Int64 ID, IDbConnection conn)");  //PRIMEPOS-2971 07-Jun-2021 JY Added
                //ErrorHandler.throwException(ex, "", "");  //PRIMEPOS-2971 07-Jun-2021 JY Commented as no need to log it in errorlog
                return null;
            }
        }

        public CLPointsAdjustmentLogData Populate(System.Int64 ID)
        {
            using (IDbConnection conn = DataFactory.CreateConnection(POS_Core.Resources.Configuration.ConnectionString))
            {
                return (Populate(ID, conn));
            }
        }

        public CLPointsAdjustmentLogData GetHistoryByCLCardID(System.Int64 iCLCardID)
        {
            using (IDbConnection conn = DataFactory.CreateConnection(POS_Core.Resources.Configuration.ConnectionString))
            {
                return (PopulateList(" where CardID=" + iCLCardID.ToString() +" order by createdOn desc ", conn));
            }
        }

        public CLPointsAdjustmentLogData GetByCLCardID(System.Int64 iCLCardID)
        {
            using (IDbConnection conn = DataFactory.CreateConnection(POS_Core.Resources.Configuration.ConnectionString))
            {
                return (PopulateList(" where CardID=" + iCLCardID.ToString(), conn));
            }
        }

        public CLPointsAdjustmentLogData PopulateList(string sWhereClause, IDbConnection conn)
        {
            try
            {
                string sSQL = String.Concat("Select * "
                                    + " FROM "
                                        + clsPOSDBConstants.CLPointsAdjustmentLog_tbl
                                    , sWhereClause);

                CLPointsAdjustmentLogData ds = new CLPointsAdjustmentLogData();
                ds.CLPointsAdjustmentLog.MergeTable(DataHelper.ExecuteDataset(conn, CommandType.Text, sSQL, whereParameters(sWhereClause)).Tables[0]);
                return ds;
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
                logger.Fatal(ex, "PopulateList(string sWhereClause, IDbConnection conn)");  //PRIMEPOS-2971 07-Jun-2021 JY Added
                //ErrorHandler.throwException(ex, "", "");  //PRIMEPOS-2971 07-Jun-2021 JY Commented as no need to log it in errorlog
                return null;
            }
        }

        public CLPointsAdjustmentLogData PopulateList(string sWhereClause, IDbTransaction tx)
        {
            try
            {
                string sSQL = String.Concat("Select * "
                                    + " FROM "
                                        + clsPOSDBConstants.CLPointsAdjustmentLog_tbl
                                    , sWhereClause);

                CLPointsAdjustmentLogData ds = new CLPointsAdjustmentLogData();
                ds.CLPointsAdjustmentLog.MergeTable(DataHelper.ExecuteDataset(tx, CommandType.Text, sSQL, whereParameters(sWhereClause)).Tables[0]);
                return ds;
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
                logger.Fatal(ex, "PopulateList(string sWhereClause, IDbTransaction tx)");  //PRIMEPOS-2971 07-Jun-2021 JY Added
                //ErrorHandler.throwException(ex, "", "");  //PRIMEPOS-2971 07-Jun-2021 JY Commented as no need to log it in errorlog
                return null;
            }
        }

        public CLPointsAdjustmentLogData PopulateList(string whereClause)
        {
            using (IDbConnection conn = DataFactory.CreateConnection(POS_Core.Resources.Configuration.ConnectionString))
            {
                return (PopulateList(whereClause, conn));
            }
        }

        #endregion //Get Method

        #region Insert, Update, and Delete Methods
        public void Insert(DataSet ds, IDbTransaction tx)
        {

            CLPointsAdjustmentLogTable addedTable = (CLPointsAdjustmentLogTable)ds.Tables[0].GetChanges(DataRowState.Added);
            string sSQL;
            IDbDataParameter[] insParam;

            if (addedTable != null && addedTable.Rows.Count > 0)
            {
                foreach (CLPointsAdjustmentLogRow row in addedTable.Rows)
                {
                    try
                    {

                        insParam = InsertParameters(row);
                        sSQL = BuildInsertSQL(clsPOSDBConstants.CLPointsAdjustmentLog_tbl, insParam);
                        DataHelper.ExecuteNonQuery(tx, CommandType.Text, sSQL, insParam);
                    }
                    catch (POSExceptions ex)
                    {
                        throw (ex);
                    }
                    catch (OtherExceptions ex)
                    {
                        throw (ex);
                    }
                    catch (SqlException ex)
                    {
                        throw (ex);
                    }

                    catch (Exception ex)
                    {
                        logger.Fatal(ex, "Insert(DataSet ds, IDbTransaction tx)");  //PRIMEPOS-2971 07-Jun-2021 JY Added
                        //ErrorHandler.throwException(ex, "", "");  //PRIMEPOS-2971 07-Jun-2021 JY Commented as no need to log it in errorlog
                    }
                }
                addedTable.AcceptChanges();
            }
        }

        // Update all rows in a DeptCodes DataSet, within a given database transaction.
        public void Update(DataSet ds, IDbTransaction tx)
        {
            CLPointsAdjustmentLogTable modifiedTable = (CLPointsAdjustmentLogTable)ds.Tables[0].GetChanges(DataRowState.Modified);

            string sSQL;
            IDbDataParameter[] updParam;

            if (modifiedTable != null && modifiedTable.Rows.Count > 0)
            {
                foreach (CLPointsAdjustmentLogRow row in modifiedTable.Rows)
                {
                    try
                    {
                        updParam = UpdateParameters(row);
                        sSQL = BuildUpdateSQL(clsPOSDBConstants.CLPointsAdjustmentLog_tbl, updParam);

                        DataHelper.ExecuteNonQuery(tx, CommandType.Text, sSQL, updParam);
                    }
                    catch (POSExceptions ex)
                    {
                        throw (ex);
                    }

                    catch (OtherExceptions ex)
                    {
                        throw (ex);
                    }
                    catch (SqlException ex)
                    {
                        throw (ex);
                    }


                    catch (Exception ex)
                    {
                        logger.Fatal(ex, "Update(DataSet ds, IDbTransaction tx)");  //PRIMEPOS-2971 07-Jun-2021 JY Added
                        //ErrorHandler.throwException(ex, "", "");  //PRIMEPOS-2971 07-Jun-2021 JY Commented as no need to log it in errorlog
                    }
                }
                modifiedTable.AcceptChanges();
            }
        }

        // Delete all rows within a DeptCodes DataSet, within a given database transaction.
        public void Delete(DataSet ds, IDbTransaction tx)
        {

            CLPointsAdjustmentLogTable table = (CLPointsAdjustmentLogTable)ds.Tables[0].GetChanges(DataRowState.Deleted);
            string sSQL;
            IDbDataParameter[] delParam;

            if (table != null && table.Rows.Count > 0)
            {
                table.RejectChanges(); //so we can access the rows
                foreach (CLPointsAdjustmentLogRow row in table.Rows)
                {
                    try
                    {
                        delParam = PKParameters(row);

                        sSQL = BuildDeleteSQL(clsPOSDBConstants.CLPointsAdjustmentLog_tbl, delParam);
                        DataHelper.ExecuteNonQuery(tx, CommandType.Text, sSQL, delParam);
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
                        logger.Fatal(ex, "Delete(DataSet ds, IDbTransaction tx)");  //PRIMEPOS-2971 07-Jun-2021 JY Added
                        //ErrorHandler.throwException(ex, "", "");  //PRIMEPOS-2971 07-Jun-2021 JY Commented as no need to log it in errorlog
                    }
                }
            }
        }

        private string BuildDeleteSQL(string tableName, IDbDataParameter[] delParam)
        {
            string sDeleteSQL = "DELETE FROM " + tableName + " WHERE ";
            // build where clause
            for (int i = 0; i < delParam.Length; i++)
            {
                sDeleteSQL = sDeleteSQL + delParam[i].SourceColumn + " = " + delParam[i].ParameterName;
            }
            return sDeleteSQL;
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
            sInsertSQL = sInsertSQL + " , CreatedBy, CreatedOn ";
            sInsertSQL = sInsertSQL + " ) Values (" + delParam[0].ParameterName;

            for (int i = 1; i < delParam.Length; i++)
            {
                sInsertSQL = sInsertSQL + " , " + delParam[i].ParameterName;
            }
            sInsertSQL = sInsertSQL + " , '" + Configuration.UserName + "','"+System.DateTime.Now +"'";
            sInsertSQL = sInsertSQL + " )";
            return sInsertSQL;
        }

        private string BuildUpdateSQL(string tableName, IDbDataParameter[] updParam)
        {
            string sUpdateSQL = "UPDATE " + tableName + " SET ";
            // build where clause
            sUpdateSQL = sUpdateSQL + updParam[1].SourceColumn + "  = " + updParam[1].ParameterName;

            for (int i = 2; i < updParam.Length; i++)
            {
                sUpdateSQL = sUpdateSQL + " , " + updParam[i].SourceColumn + "  = " + updParam[i].ParameterName;
            }

            sUpdateSQL = sUpdateSQL + " WHERE " + updParam[0].SourceColumn + " = " + updParam[0].ParameterName;
            return sUpdateSQL;
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

        private IDbDataParameter[] PKParameters(System.Int64 ID)
        {
            IDbDataParameter[] sqlParams = DataFactory.CreateParameterArray(1);

            sqlParams[0] = DataFactory.CreateParameter();
            sqlParams[0].ParameterName = "@ID";
            sqlParams[0].DbType = System.Data.DbType.Int64;
            sqlParams[0].Value = ID;

            return (sqlParams);
        }

        private IDbDataParameter[] PKParameters(CLPointsAdjustmentLogRow row)
        {
            //return a SqlParameterCollection
            IDbDataParameter[] sqlParams = DataFactory.CreateParameterArray(1);


            sqlParams[0] = DataFactory.CreateParameter();
            sqlParams[0].ParameterName = "@ID";
            sqlParams[0].DbType = System.Data.DbType.Int64;

            sqlParams[0].Value = row.ID;
            sqlParams[0].SourceColumn = clsPOSDBConstants.CLPointsAdjustmentLog_Fld_ID;

            return (sqlParams);
        }

        private IDbDataParameter[] InsertParameters(CLPointsAdjustmentLogRow row)
        {
            IDbDataParameter[] sqlParams = DataFactory.CreateParameterArray(4);

            sqlParams[0] = DataFactory.CreateParameter("@" + clsPOSDBConstants.CLPointsAdjustmentLog_Fld_CLCardID, System.Data.DbType.Int64);
            sqlParams[1] = DataFactory.CreateParameter("@" + clsPOSDBConstants.CLPointsAdjustmentLog_Fld_NewPoints, System.Data.DbType.Decimal);    //Sprint-18 - 06-Nov-2014 JY Changed datatype from int to decimal
            sqlParams[2] = DataFactory.CreateParameter("@" + clsPOSDBConstants.CLPointsAdjustmentLog_Fld_OldPoints, System.Data.DbType.Decimal);    //Sprint-18 - 06-Nov-2014 JY Changed datatype from int to decimal
            sqlParams[3] = DataFactory.CreateParameter("@" + clsPOSDBConstants.CLPointsAdjustmentLog_Fld_Remarks, System.Data.DbType.String);
            
            sqlParams[0].SourceColumn = clsPOSDBConstants.CLPointsAdjustmentLog_Fld_CLCardID;
            sqlParams[1].SourceColumn = clsPOSDBConstants.CLPointsAdjustmentLog_Fld_NewPoints;
            sqlParams[2].SourceColumn = clsPOSDBConstants.CLPointsAdjustmentLog_Fld_OldPoints;
            sqlParams[3].SourceColumn = clsPOSDBConstants.CLPointsAdjustmentLog_Fld_Remarks;

            sqlParams[0].Value = row.CLCardID;
            sqlParams[1].Value = row.NewPoints;
            sqlParams[2].Value = row.OldPoints;
            sqlParams[3].Value = row.Remarks;
            
            return (sqlParams);
        }

        private IDbDataParameter[] UpdateParameters(CLPointsAdjustmentLogRow row)
        {
            IDbDataParameter[] sqlParams = DataFactory.CreateParameterArray(8);

            sqlParams[0] = DataFactory.CreateParameter("@" + clsPOSDBConstants.CLPointsAdjustmentLog_Fld_ID, System.Data.DbType.Int32);
            sqlParams[1] = DataFactory.CreateParameter("@" + clsPOSDBConstants.CLPointsAdjustmentLog_Fld_CLCardID, System.Data.DbType.Int64);
            sqlParams[2] = DataFactory.CreateParameter("@" + clsPOSDBConstants.CLPointsAdjustmentLog_Fld_NewPoints, System.Data.DbType.Decimal);    //Sprint-18 - 06-Nov-2014 JY Changed datatype from int to decimal
            sqlParams[3] = DataFactory.CreateParameter("@" + clsPOSDBConstants.CLPointsAdjustmentLog_Fld_OldPoints, System.Data.DbType.Decimal);    //Sprint-18 - 06-Nov-2014 JY Changed datatype from int to decimal
            sqlParams[4] = DataFactory.CreateParameter("@" + clsPOSDBConstants.CLPointsAdjustmentLog_Fld_Remarks, System.Data.DbType.String);

            sqlParams[0].SourceColumn = clsPOSDBConstants.CLPointsAdjustmentLog_Fld_ID;
            sqlParams[1].SourceColumn = clsPOSDBConstants.CLPointsAdjustmentLog_Fld_CLCardID;
            sqlParams[2].SourceColumn = clsPOSDBConstants.CLPointsAdjustmentLog_Fld_NewPoints;
            sqlParams[3].SourceColumn = clsPOSDBConstants.CLPointsAdjustmentLog_Fld_OldPoints;
            sqlParams[4].SourceColumn = clsPOSDBConstants.CLPointsAdjustmentLog_Fld_Remarks;

            sqlParams[0].Value = row.ID;
            sqlParams[1].Value = row.CLCardID;
            sqlParams[2].Value = row.NewPoints;
            sqlParams[3].Value = row.OldPoints;
            sqlParams[4].Value = row.Remarks;
            
            return (sqlParams);
        }
        
#endregion

        public void Dispose() { }

    }
}
