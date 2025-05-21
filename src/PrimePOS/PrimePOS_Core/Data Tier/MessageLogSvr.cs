using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using POS_Core.ErrorLogging;
using POS_Core.CommonData.Tables;
using POS_Core.CommonData;
using POS_Core.CommonData.Rows;
using Resources;
using NLog;

namespace POS_Core.DataAccess
{
    public class MessageLogSvr : IDisposable
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

        // Looks up a MessageLog based on its primary-key:System.Int32 Customercode

        public virtual MessageLogData Populate(System.DateTime Key, SqlConnection conn)
        {
            try
            {
                string sSQL = "";
                sSQL = "Select * FROM " + clsPOSDBConstants.MessageLog_tbl + " WHERE " + clsPOSDBConstants.MessageLog_Fld_Date + " > '" + Key + "'";

                MessageLogData ds = new MessageLogData();
                ds.MessageLog.MergeTable(DataHelper.ExecuteDataset(conn, CommandType.Text, sSQL).Tables[0]);
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
                logger.Fatal(ex, "Populate(System.DateTime Key, SqlConnection conn)");  //PRIMEPOS-2971 07-Jun-2021 JY Added
                //ErrorHandler.throwException(ex, "", "");
                return null;
            }
        }

        public MessageLogData Populate(System.DateTime Key)
        {
            using (SqlConnection conn = new SqlConnection(POS_Core.Resources.Configuration.ConnectionString))
            {
                return (Populate(Key, conn));
            }
        }

        // Fills a CustomerData with all MessageLog

        public MessageLogData PopulateList(string sWhereClause, SqlConnection conn)
        {
            try
            {
                string sSQL = String.Concat("SELECT * FROM ", clsPOSDBConstants.MessageLog_tbl, sWhereClause);

                MessageLogData ds = new MessageLogData();
                ds.MessageLog.MergeTable(DataHelper.ExecuteDataset(conn, CommandType.Text, sSQL, whereParameters(sWhereClause)).Tables[0]);
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

        // Fills a CustomerData with all MessageLog

        public MessageLogData PopulateList(string whereClause)
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

            MessageLogTable addedTable = (MessageLogTable)ds.Tables[clsPOSDBConstants.MessageLog_tbl].GetChanges(DataRowState.Added);
            string sSQL;
            IDbDataParameter[] insParam;

            if (addedTable != null && addedTable.Rows.Count > 0)
            {
                foreach (MessageLogRow row in addedTable.Rows)
                {
                    try
                    {

                        insParam = InsertParameters(row);
                        sSQL = BuildInsertSQL(clsPOSDBConstants.MessageLog_tbl, insParam);
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

        // Update all rows in a MessageLog DataSet, within a given database transaction.

        public void Update(DataSet ds, SqlTransaction tx)
        {
            MessageLogTable modifiedTable = (MessageLogTable)ds.Tables[clsPOSDBConstants.MessageLog_tbl].GetChanges(DataRowState.Modified);

            string sSQL;
            IDbDataParameter[] updParam;

            if (modifiedTable != null && modifiedTable.Rows.Count > 0)
            {
                foreach (MessageLogRow row in modifiedTable.Rows)
                {
                    try
                    {
                        updParam = UpdateParameters(row);
                        sSQL = BuildUpdateSQL(clsPOSDBConstants.MessageLog_tbl, updParam);

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

        // Delete all rows within a MessageLog DataSet, within a given database transaction.
        public void Delete(DataSet ds, SqlTransaction tx)
        {

            MessageLogTable table = (MessageLogTable)ds.Tables[clsPOSDBConstants.MessageLog_tbl].GetChanges(DataRowState.Deleted);
            string sSQL;
            IDbDataParameter[] delParam;

            if (table != null && table.Rows.Count > 0)
            {
                table.RejectChanges(); //so we can access the rows
                foreach (MessageLogRow row in table.Rows)
                {
                    try
                    {
                        delParam = PKParameters(row);

                        sSQL = BuildDeleteSQL(clsPOSDBConstants.MessageLog_tbl, delParam);
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
            //// build where clause
            sDeleteSQL = sDeleteSQL + delParam[0].SourceColumn + " = " + delParam[0].ParameterName+"";          
            return sDeleteSQL;
        }
        private string BuildInsertSQL(string tableName, IDbDataParameter[] delParam)
        {
            string sInsertSQL = "Insert Into " + tableName + " ( ";          
            sInsertSQL = sInsertSQL + delParam[0].SourceColumn;
            sInsertSQL = sInsertSQL + " ,"+  delParam[1].SourceColumn;
            sInsertSQL = sInsertSQL + " ," + delParam[2].SourceColumn;
            sInsertSQL = sInsertSQL + " ) Values (" + delParam[0].ParameterName;
            sInsertSQL = sInsertSQL + " , " + delParam[1].ParameterName;
            sInsertSQL = sInsertSQL + " , " + delParam[2].ParameterName;
            sInsertSQL = sInsertSQL + " )";
            return sInsertSQL;
        }


        private string BuildUpdateSQL(string tableName, IDbDataParameter[] updParam)
        {
            string sUpdateSQL = "UPDATE " + tableName + " SET ";
            //// build where clause
            //sUpdateSQL = sUpdateSQL + updParam[2].SourceColumn + "  = " + updParam[2].ParameterName;

            //for (int i = 3; i < updParam.Length; i++)
            //{
            //    sUpdateSQL = sUpdateSQL + " , " + updParam[i].SourceColumn + "  = " + updParam[i].ParameterName;

            //}
            //sUpdateSQL = sUpdateSQL + " , UserID  = '" + Configuration.UserName + "'";
            //sUpdateSQL = sUpdateSQL + " WHERE " + updParam[1].SourceColumn + " = " + updParam[1].ParameterName;
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
        private IDbDataParameter[] PKParameters(System.DateTime date)
        {
            IDbDataParameter[] sqlParams = DataFactory.CreateParameterArray(1);


            sqlParams[0] = DataFactory.CreateParameter();
            sqlParams[0].ParameterName = "@LogDate";
            sqlParams[0].DbType = System.Data.DbType.DateTime;
            sqlParams[0].Value = date;

            return (sqlParams);
        }

        private IDbDataParameter[] PKParameters(MessageLogRow row)
        {
            //return a SqlParameterCollection
            IDbDataParameter[] sqlParams = DataFactory.CreateParameterArray(1);

            sqlParams[0] = DataFactory.CreateParameter();
            sqlParams[0].ParameterName = "@LogDate";
            sqlParams[0].DbType = System.Data.DbType.DateTime;

            sqlParams[0].Value = row.LogDate;
            sqlParams[0].SourceColumn = clsPOSDBConstants.MessageLog_Fld_Date;

            return (sqlParams);
        }

        private IDbDataParameter[] InsertParameters(MessageLogRow row)
        {
            IDbDataParameter[] sqlParams = DataFactory.CreateParameterArray(3);

            sqlParams[0] = DataFactory.CreateParameter("@" + clsPOSDBConstants.MessageLog_Fld_Date, System.Data.DbType.DateTime);
            sqlParams[1] = DataFactory.CreateParameter("@" + clsPOSDBConstants.MessageLog_Fld_Time, System.Data.DbType.String);
            sqlParams[2] = DataFactory.CreateParameter("@" + clsPOSDBConstants.MessageLog_Fld_MessageString, System.Data.DbType.String);

            sqlParams[0].SourceColumn = clsPOSDBConstants.MessageLog_Fld_Date;
            sqlParams[1].SourceColumn = clsPOSDBConstants.MessageLog_Fld_Time;  
            sqlParams[2].SourceColumn = clsPOSDBConstants.MessageLog_Fld_MessageString;
           
            sqlParams[0].Value = row.LogDate;

            if (row.LogTime != System.String.Empty)
                sqlParams[1].Value = row.LogTime.Trim();
            else
                sqlParams[1].Value = DBNull.Value;

            if (row.LogMessage != System.String.Empty)
                sqlParams[2].Value = row.LogMessage.Trim();
            else
                sqlParams[2].Value = DBNull.Value;

            return (sqlParams);
        }

        private IDbDataParameter[] UpdateParameters(MessageLogRow row)
        {
            IDbDataParameter[] sqlParams = DataFactory.CreateParameterArray(3);

            sqlParams[0] = DataFactory.CreateParameter("@" + clsPOSDBConstants.MessageLog_Fld_Date, System.Data.DbType.DateTime);
            sqlParams[1] = DataFactory.CreateParameter("@" + clsPOSDBConstants.MessageLog_Fld_Time, System.Data.DbType.String);
            sqlParams[2] = DataFactory.CreateParameter("@" + clsPOSDBConstants.MessageLog_Fld_MessageString, System.Data.DbType.String);
         
            sqlParams[0].SourceColumn = clsPOSDBConstants.MessageLog_Fld_Date;
            sqlParams[1].SourceColumn = clsPOSDBConstants.MessageLog_Fld_Time;
            sqlParams[2].SourceColumn = clsPOSDBConstants.MessageLog_Fld_MessageString;
            
            sqlParams[0].Value = row.LogDate;

            if (row.LogTime != System.String.Empty)
                sqlParams[1].Value = row.LogTime.Trim();
            else
                sqlParams[1].Value = DBNull.Value;

            if (row.LogMessage != System.String.Empty)
                sqlParams[2].Value = row.LogMessage.Trim();
            else
                sqlParams[2].Value = DBNull.Value;
          
            return (sqlParams);
        }
        #endregion

        public void Dispose() { }
    }
}
