using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using POS_Core.ErrorLogging;
using Resources;
using POS_Core.CommonData;
using POS_Core.CommonData.Tables;
using POS_Core.CommonData.Rows;
using NLog;

namespace POS_Core.DataAccess
{
    public class FileHistorySvr : IDisposable
    {
        private static ILogger logger = LogManager.GetCurrentClassLogger();

        #region Persist Methods

        // Inserts, updates or deletes rows in a DataSet, within a database transaction.

        public void Persist(DataSet updates, SqlTransaction tx)
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
                logger.Fatal(ex, "Persist(DataSet updates, SqlTransaction tx)");  //PRIMEPOS-2971 07-Jun-2021 JY Added
                //POS_Core.ErrorLogging.ErrorHandler.throwException(ex, "", "");
            }
        }


        // Inserts, updates or deletes rows in a DataSet.

        public void Persist(DataSet updates)
        {

            SqlTransaction tx;
            SqlConnection conn = new SqlConnection(POS_Core.Resources.Configuration.ConnectionString);

            conn.Open();
            tx = conn.BeginTransaction();
            try
            {
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
                //ErrorHandler.throwException(ex, "", "");
            }

        }
        #endregion

        #region Get Methods

        // Looks up a Customer based on its primary-key:System.Int32 Customercode

        public virtual FileHistoryData Populate(System.Int64 Key, bool SrhByName, SqlConnection conn)
        {
            try
            {
                string sSQL = "";
                if (SrhByName == false)
                    sSQL = "Select * FROM " + clsPOSDBConstants.FileHistory_tbl + " WHERE " + clsPOSDBConstants.FileHistory_Fld_FileID + " =" + Key + "";
                else
                    sSQL = "Select * FROM " + clsPOSDBConstants.FileHistory_tbl + " WHERE " + clsPOSDBConstants.FileHistory_Fld_FileID + " =" + Key + "";


                FileHistoryData ds = new FileHistoryData();
                ds.FileHistoryTable.MergeTable(DataHelper.ExecuteDataset(conn, CommandType.Text, sSQL, PKParameters(Key)).Tables[0]);
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
                logger.Fatal(ex, "Populate(System.Int64 Key, bool SrhByName, SqlConnection conn)");  //PRIMEPOS-2971 07-Jun-2021 JY Added
                //ErrorHandler.throwException(ex, "", "");
                return null;
            }
        }

        public FileHistoryData Populate(System.Int64 Key, bool SearchByName)
        {
            using (SqlConnection conn = new SqlConnection(POS_Core.Resources.Configuration.ConnectionString))
            {
                return (Populate(Key, SearchByName, conn));
            }
        }


        internal long GetMaxFileID()
        {
            Object response = null;
            Int64 fileID = 0;

            try
            {
                IDbConnection conn = DataFactory.CreateConnection(Resources.Configuration.ConnectionString);
                string sSQL = "Select  MAX(" + clsPOSDBConstants.FileHistory_Fld_FileID + ") from " + clsPOSDBConstants.FileHistory_tbl;
                response = DataHelper.ExecuteScalar(conn, CommandType.Text, sSQL);

                if (response != null)
                    fileID = Convert.ToInt64(response);
            }
            catch (Exception ex)
            {

            }
            return fileID;
        }



        public FileHistoryData PopulateList(string sWhereClause, SqlConnection conn)
        {
            try
            {
                string sSQL = String.Concat("SELECT * FROM ", clsPOSDBConstants.FileHistory_tbl, sWhereClause);

                FileHistoryData ds = new FileHistoryData();
                ds.FileHistoryTable.MergeTable(DataHelper.ExecuteDataset(conn, CommandType.Text, sSQL, whereParameters(sWhereClause)).Tables[0]);
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
                logger.Fatal(ex, "PopulateList(string sWhereClause, SqlConnection conn)");  //PRIMEPOS-2971 07-Jun-2021 JY Added
                //ErrorHandler.throwException(ex, "", "");
                return null;
            }
        }

        // Fills a CustomerData with all Customer

        public FileHistoryData PopulateList(string whereClause)
        {
            using (SqlConnection conn = new SqlConnection(POS_Core.Resources.Configuration.ConnectionString))
            {
                return (PopulateList(whereClause, conn));
            }
        }

        #endregion //Get Method

        #region Insert, Update, and Delete Methods

        public void Insert(DataSet ds, SqlTransaction tx)
        {

            FileHistoryTable addedTable = (FileHistoryTable)ds.Tables[clsPOSDBConstants.FileHistory_tbl].GetChanges(DataRowState.Added);

            string sSQL;
            IDbDataParameter[] insParam;

            if (addedTable != null && addedTable.Rows.Count > 0)
            {
                foreach (FileHistoryRow row in addedTable.Rows)
                {
                    try
                    {

                        insParam = InsertParameters(row);
                        sSQL = BuildInsertSQL(clsPOSDBConstants.FileHistory_tbl, insParam);

                        for (int i = 0; i < insParam.Length; i++)
                        {
                            Console.WriteLine(insParam[i].ParameterName + "  " + insParam[i].Value);
                        }
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
                        if (ex.Number == 2627)
                            ErrorHandler.throwCustomError(POSErrorENUM.Customer_DuplicateCode);
                        else
                            throw (ex);

                    }

                    catch (Exception ex)
                    {
                        logger.Fatal(ex, "Insert(DataSet ds, SqlTransaction tx)");  //PRIMEPOS-2971 07-Jun-2021 JY Added
                        //ErrorHandler.throwException(ex, "", "");
                    }

                }
                addedTable.AcceptChanges();
            }
        }

        // Update all rows in a Customer DataSet, within a given database transaction.

        public void Update(DataSet ds, SqlTransaction tx)
        {

            FileHistoryTable modifiedTable = (FileHistoryTable)ds.Tables[clsPOSDBConstants.FileHistory_tbl].GetChanges(DataRowState.Modified);

            string sSQL;
            IDbDataParameter[] updParam;

            if (modifiedTable != null && modifiedTable.Rows.Count > 0)
            {
                foreach (FileHistoryRow row in modifiedTable.Rows)
                {
                    try
                    {

                        updParam = UpdateParameters(row);
                        sSQL = BuildUpdateSQL(clsPOSDBConstants.FileHistory_tbl, updParam);
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
                        if (ex.Number == 2627)
                            ErrorHandler.throwCustomError(POSErrorENUM.Customer_DuplicateCode);
                        else
                            throw (ex);
                    }


                    catch (Exception ex)
                    {
                        logger.Fatal(ex, "Update(DataSet ds, SqlTransaction tx)");  //PRIMEPOS-2971 07-Jun-2021 JY Added
                        //ErrorHandler.throwException(ex, "", "");
                    }
                }
                modifiedTable.AcceptChanges();
            }
        }

        // Delete all rows within a Customer DataSet, within a given database transaction.
        public void Delete(DataSet ds, SqlTransaction tx)
        {

            FileHistoryTable table = (FileHistoryTable)ds.Tables[clsPOSDBConstants.FileHistory_tbl].GetChanges(DataRowState.Deleted);

            string sSQL;
            IDbDataParameter[] delParam;

            if (table != null && table.Rows.Count > 0)
            {
                table.RejectChanges(); //so we can access the rows
                foreach (FileHistoryRow row in table.Rows)
                {

                    try
                    {
                        delParam = PKParameters(row);

                        sSQL = BuildDeleteSQL(clsPOSDBConstants.FileHistory_tbl, delParam);

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
                        logger.Fatal(ex, "Delete(DataSet ds, SqlTransaction tx)");  //PRIMEPOS-2971 07-Jun-2021 JY Added
                        //ErrorHandler.throwException(ex, "", "");
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
            string sInsertSQL = "Insert Into " + tableName + " ( ";
            // build where clause

            sInsertSQL = sInsertSQL + delParam[0].SourceColumn;

            sInsertSQL = sInsertSQL + " , " + delParam[1].SourceColumn;

            sInsertSQL = sInsertSQL + " , " + delParam[2].SourceColumn;

            sInsertSQL = sInsertSQL + " ) Values (" + delParam[0].ParameterName;

            sInsertSQL = sInsertSQL + " , " + delParam[1].ParameterName;

            sInsertSQL = sInsertSQL + " , " + delParam[2].ParameterName;

            sInsertSQL = sInsertSQL + " )";

            return sInsertSQL;
        }


        private string BuildUpdateSQL(string tableName, IDbDataParameter[] updParam)
        {
            string sUpdateSQL = "UPDATE " + tableName + " SET ";
            // build where clause
            sUpdateSQL = sUpdateSQL + updParam[1].SourceColumn + "  = " + updParam[1].ParameterName + updParam[2].SourceColumn + " = " + updParam[2].ParameterName;
            //for(int i = 3;i<updParam.Length;i++)
            //{
            //sUpdateSQL = sUpdateSQL + " , " + updParam[1].SourceColumn +"  = " + updParam[i].ParameterName ;

            //}
            //sUpdateSQL = sUpdateSQL + " , UserID  = '" + Configuration.UserName + "'" ;
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

        private IDbDataParameter[] PKParameters(System.Int64 FileID)
        {

            IDbDataParameter[] sqlParams = DataFactory.CreateParameterArray(1);

            sqlParams[0] = DataFactory.CreateParameter();
            sqlParams[0].ParameterName = "@FileID";
            sqlParams[0].DbType = System.Data.DbType.String;
            sqlParams[0].Value = FileID;

            return (sqlParams);
        }

        private IDbDataParameter[] PKParameters(FileHistoryRow row)
        {
            //return a SqlParameterCollection
            IDbDataParameter[] sqlParams = DataFactory.CreateParameterArray(1);


            sqlParams[0] = DataFactory.CreateParameter();
            sqlParams[0].ParameterName = "@FileID";
            sqlParams[0].DbType = System.Data.DbType.String;

            sqlParams[0].Value = row.FileID;
            sqlParams[0].SourceColumn = clsPOSDBConstants.FileHistory_Fld_FileID;

            return (sqlParams);
        }

        private IDbDataParameter[] InsertParameters(FileHistoryRow row)
        {

            IDbDataParameter[] sqlParams = DataFactory.CreateParameterArray(3);

            sqlParams[0] = DataFactory.CreateParameter("@" + clsPOSDBConstants.FileHistory_Fld_FileID, System.Data.DbType.Int64);
            sqlParams[1] = DataFactory.CreateParameter("@" + clsPOSDBConstants.FileHistory_Fld_LastUpdateDate, System.Data.DbType.DateTime);
            sqlParams[2] = DataFactory.CreateParameter("@" + clsPOSDBConstants.FileHistory_Fld_SynchronizedCentrally, System.Data.DbType.Boolean);

            sqlParams[0].SourceColumn = clsPOSDBConstants.FileHistory_Fld_FileID;
            sqlParams[1].SourceColumn = clsPOSDBConstants.FileHistory_Fld_LastUpdateDate;
            sqlParams[2].SourceColumn = clsPOSDBConstants.FileHistory_Fld_SynchronizedCentrally;

            if (row.FileID != System.Int64.MinValue)
                sqlParams[0].Value = row.FileID;
            else
                sqlParams[0].Value = DBNull.Value;

            if (row.LastUpdateDate != System.DateTime.MinValue)
                sqlParams[1].Value = row.LastUpdateDate;
            else
                sqlParams[1].Value = System.DateTime.MinValue;

            sqlParams[2].Value = row.SynchronizedCentrally;

            return (sqlParams);
        }

        private IDbDataParameter[] UpdateParameters(FileHistoryRow row)
        {

            IDbDataParameter[] sqlParams = DataFactory.CreateParameterArray(3);

            sqlParams[0] = DataFactory.CreateParameter("@" + clsPOSDBConstants.FileHistory_Fld_FileID, System.Data.DbType.Int64);
            sqlParams[1] = DataFactory.CreateParameter("@" + clsPOSDBConstants.FileHistory_Fld_LastUpdateDate, System.Data.DbType.DateTime);
            sqlParams[2] = DataFactory.CreateParameter("@" + clsPOSDBConstants.FileHistory_Fld_SynchronizedCentrally, System.Data.DbType.Boolean);

            sqlParams[0].SourceColumn = clsPOSDBConstants.FileHistory_Fld_FileID;
            sqlParams[1].SourceColumn = clsPOSDBConstants.FileHistory_Fld_LastUpdateDate;
            sqlParams[2].SourceColumn = clsPOSDBConstants.FileHistory_Fld_SynchronizedCentrally;

            if (row.FileID != System.Int64.MinValue)
                sqlParams[0].Value = row.FileID;
            else
                sqlParams[0].Value = DBNull.Value;

            if (row.LastUpdateDate != System.DateTime.MinValue)
                sqlParams[1].Value = row.LastUpdateDate;
            else
                sqlParams[1].Value = System.DateTime.MinValue;

            sqlParams[2].Value = row.SynchronizedCentrally;

            return (sqlParams);
        }

        #endregion

        #region IDisposable Members
        public void Dispose()
        {

        }
        #endregion


    }
}
