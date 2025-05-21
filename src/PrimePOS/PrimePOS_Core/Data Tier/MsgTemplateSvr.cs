using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;
using System.Data;
using System.Data.SqlClient;
using POS_Core.ErrorLogging;
using Resources;
using POS_Core.CommonData;
using POS_Core.CommonData.Tables;
using POS_Core.CommonData.Rows;
using POS_Core.Resources;
//using POS.Resources;

namespace POS_Core.DataAccess
{
    class MsgTemplateSvr : IDisposable
    {
        private static ILogger logger = LogManager.GetCurrentClassLogger();
        public delegate void DataRowSavedHandler();
        public event DataRowSavedHandler DataRowSaved;

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
                logger.Fatal(ex, "Persist(DataSet updates, SqlTransaction tx)");
                throw (ex);
            }
            catch (OtherExceptions ex)
            {
                logger.Fatal(ex, "Persist(DataSet updates, SqlTransaction tx)");
                throw (ex);
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "Persist(DataSet updates, SqlTransaction tx)");
                //ErrorLogging.ErrorHandler.throwException(ex, "", "");
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
                logger.Fatal(ex, "Persist(DataSet updates)");
                tx.Rollback();
                throw (ex);
            }
            catch (OtherExceptions ex)
            {
                logger.Fatal(ex, "Persist(DataSet updates)");
                tx.Rollback();
                throw (ex);
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "Persist(DataSet updates)");
                tx.Rollback();
                //ErrorHandler.throwException(ex, "", "");
            }
            finally
            {
                conn.Close();
            }
        }
        #endregion

        public bool DeleteRow(string RecID)
        {
            string sSQL;
            try
            {
                sSQL = "DELETE FROM FMessage WHERE RecID = " + RecID;
                DataHelper.ExecuteNonQuery(DBConfig.ConnectionString, CommandType.Text, sSQL);
                return true;
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "DeleteRow(string RecID)");
                //ErrorHandler.throwException(ex, "", "");
                return false;
            }
        }

        private void RaiseDataRowSaved()
        {
            if (DataRowSaved != null)
            {
                DataRowSaved();
            }
        }

        #region Insert, Update, and Delete Methods
        public void Insert(DataSet ds, SqlTransaction tx)
        {
            MsgTemplateTable addedTable = (MsgTemplateTable)ds.Tables[clsPOSDBConstants.FMessage_tbl].GetChanges(DataRowState.Added);
            string sSQL;
            IDbDataParameter[] insParam;

            if (addedTable != null && addedTable.Rows.Count > 0)
            {
                foreach (MsgTemplateRow row in addedTable.Rows)
                {
                    try
                    {
                        insParam = InsertParameters(row);
                        sSQL = BuildInsertSQL(clsPOSDBConstants.FMessage_tbl, insParam);
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
                    catch (Exception ex)
                    {
                        logger.Fatal(ex, "Insert(DataSet ds, SqlTransaction tx)");  //PRIMEPOS-2971 07-Jun-2021 JY Added
                        //ErrorHandler.throwException(ex, "", "");
                    }
                }
                addedTable.AcceptChanges();
            }
        }

        public void Update(DataSet ds, SqlTransaction tx)
        {
            MsgTemplateTable modifiedTable = (MsgTemplateTable)ds.Tables[clsPOSDBConstants.FMessage_tbl].GetChanges(DataRowState.Modified);

            string sSQL;
            IDbDataParameter[] updParam;

            if (modifiedTable != null && modifiedTable.Rows.Count > 0)
            {
                foreach (MsgTemplateRow row in modifiedTable.Rows)
                {
                    try
                    {
                        updParam = UpdateParameters(row);
                        sSQL = BuildUpdateSQL(clsPOSDBConstants.FMessage_tbl, updParam);

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
                    catch (Exception ex)
                    {
                        logger.Fatal(ex, "Update(DataSet ds, SqlTransaction tx)");  //PRIMEPOS-2971 07-Jun-2021 JY Added
                        //ErrorHandler.throwException(ex, "", "");
                    }
                }
                modifiedTable.AcceptChanges();
            }
        }

        private string BuildInsertSQL(string tableName, IDbDataParameter[] delParam)
        {
            string sInsertSQL = "INSERT INTO " + tableName + " ( ";
            sInsertSQL = sInsertSQL + delParam[0].SourceColumn;

            for (int i = 1; i < delParam.Length; i++)
            {
                sInsertSQL = sInsertSQL + "," + delParam[i].SourceColumn;
            }
            sInsertSQL = sInsertSQL + "," + clsPOSDBConstants.FMessage_Fld_CreatedBy + "," + clsPOSDBConstants.FMessage_Fld_CreatedOn + "," + clsPOSDBConstants.FMessage_Fld_UpdatedBy + "," + clsPOSDBConstants.FMessage_Fld_UpdatedOn;
            sInsertSQL = sInsertSQL + " ) Values (" + delParam[0].ParameterName;

            for (int i = 1; i < delParam.Length; i++)
            {
                sInsertSQL = sInsertSQL + "," + delParam[i].ParameterName;
            }
            sInsertSQL = sInsertSQL + ",'" + Configuration.UserName + "',GETDATE(),'" + Configuration.UserName + "',GETDATE()";
            sInsertSQL = sInsertSQL + " )";
            return sInsertSQL;
        }

        private string BuildUpdateSQL(string tableName, IDbDataParameter[] updParam)
        {
            string sUpdateSQL = "UPDATE " + tableName + " SET ";
            sUpdateSQL = sUpdateSQL + updParam[1].SourceColumn + "  = " + updParam[1].ParameterName;

            for (int i = 2; i < updParam.Length; i++)
            {
                sUpdateSQL = sUpdateSQL + " , " + updParam[i].SourceColumn + "  = " + updParam[i].ParameterName;
            }
            sUpdateSQL = sUpdateSQL + "," + clsPOSDBConstants.FMessage_Fld_UpdatedBy + "='" + Configuration.UserName + "'," + clsPOSDBConstants.FMessage_Fld_UpdatedOn + "= GETDATE()";
            sUpdateSQL = sUpdateSQL + " WHERE " + updParam[0].SourceColumn + " = " + updParam[0].ParameterName;
            return sUpdateSQL;
        }

        // Delete all rows within a Customer DataSet, within a given database transaction.
        public void Delete(DataSet ds, SqlTransaction tx)
        {
            MsgTemplateTable table = (MsgTemplateTable)ds.Tables[clsPOSDBConstants.FMessage_tbl].GetChanges(DataRowState.Deleted);
            string sSQL;
            IDbDataParameter[] delParam;

            if (table != null && table.Rows.Count > 0)
            {
                table.RejectChanges(); //so we can access the rows
                foreach (MsgTemplateRow row in table.Rows)
                {
                    try
                    {
                        delParam = PKParameters(row);

                        sSQL = BuildDeleteSQL(clsPOSDBConstants.FMessage_tbl, delParam);
                        DataHelper.ExecuteNonQuery(tx, CommandType.Text, sSQL, delParam);
                        RaiseDataRowSaved();
                    }
                    catch (POSExceptions ex)
                    {
                        logger.Fatal(ex, "Delete(DataSet ds, SqlTransaction tx)");
                        throw (ex);
                    }
                    catch (OtherExceptions ex)
                    {
                        logger.Fatal(ex, "Delete(DataSet ds, SqlTransaction tx)");
                        throw (ex);
                    }
                    catch (Exception ex)
                    {
                        logger.Fatal(ex, "Delete(DataSet ds, SqlTransaction tx)");
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

        private IDbDataParameter[] PKParameters(System.Int32 RecID)
        {
            IDbDataParameter[] sqlParams = DataFactory.CreateParameterArray(1);

            sqlParams[0] = DataFactory.CreateParameter();
            sqlParams[0].ParameterName = "@RecID";
            sqlParams[0].DbType = System.Data.DbType.Int32;
            sqlParams[0].Value = RecID;
            return (sqlParams);
        }

        private IDbDataParameter[] PKParameters(System.String MessageCode)
        {
            IDbDataParameter[] sqlParams = DataFactory.CreateParameterArray(1);

            sqlParams[0] = DataFactory.CreateParameter();
            sqlParams[0].ParameterName = "@MessageCode";
            sqlParams[0].DbType = System.Data.DbType.String;
            sqlParams[0].Value = MessageCode.Trim();
            return (sqlParams);
        }

        private IDbDataParameter[] PKParameters(MsgTemplateRow row)
        {
            //return a SqlParameterCollection
            IDbDataParameter[] sqlParams = DataFactory.CreateParameterArray(1);

            sqlParams[0] = DataFactory.CreateParameter();
            sqlParams[0].ParameterName = "@RecID";
            sqlParams[0].DbType = System.Data.DbType.Int32;

            sqlParams[0].Value = row.RecID.ToString().Trim();
            sqlParams[0].SourceColumn = clsPOSDBConstants.FMessage_Fld_RecID;
            return (sqlParams);
        }

        private IDbDataParameter[] InsertParameters(MsgTemplateRow row)
        {
            IDbDataParameter[] sqlParams = DataFactory.CreateParameterArray(5);

            sqlParams[0] = DataFactory.CreateParameter("@" + clsPOSDBConstants.FMessage_Fld_MessageCode, System.Data.DbType.String);
            sqlParams[1] = DataFactory.CreateParameter("@" + clsPOSDBConstants.FMessage_Fld_Message, System.Data.DbType.String);
            sqlParams[2] = DataFactory.CreateParameter("@" + clsPOSDBConstants.FMessage_Fld_MessageSub, System.Data.DbType.String);
            sqlParams[3] = DataFactory.CreateParameter("@" + clsPOSDBConstants.FMessage_Fld_MessageCatId, System.Data.DbType.Int32);
            sqlParams[4] = DataFactory.CreateParameter("@" + clsPOSDBConstants.FMessage_Fld_MessageTypeId, System.Data.DbType.Int32);

            sqlParams[0].SourceColumn = clsPOSDBConstants.FMessage_Fld_MessageCode;
            sqlParams[1].SourceColumn = clsPOSDBConstants.FMessage_Fld_Message;
            sqlParams[2].SourceColumn = clsPOSDBConstants.FMessage_Fld_MessageSub;
            sqlParams[3].SourceColumn = clsPOSDBConstants.FMessage_Fld_MessageCatId;
            sqlParams[4].SourceColumn = clsPOSDBConstants.FMessage_Fld_MessageTypeId;
            
            sqlParams[0].Value = row.MessageCode.Trim();
            sqlParams[1].Value = row.Message.Trim();
            sqlParams[2].Value = row.MessageSub.Trim();
            sqlParams[3].Value = Configuration.convertNullToInt(row.MessageCatId) == 0 ? -1 : row.MessageCatId;
            sqlParams[4].Value = Configuration.convertNullToInt(row.MessageTypeId);

            return (sqlParams);
        }

        private IDbDataParameter[] UpdateParameters(MsgTemplateRow row)
        {
            IDbDataParameter[] sqlParams = DataFactory.CreateParameterArray(6);

            sqlParams[0] = DataFactory.CreateParameter("@" + clsPOSDBConstants.FMessage_Fld_RecID, System.Data.DbType.Int32);
            sqlParams[1] = DataFactory.CreateParameter("@" + clsPOSDBConstants.FMessage_Fld_MessageCode, System.Data.DbType.String);
            sqlParams[2] = DataFactory.CreateParameter("@" + clsPOSDBConstants.FMessage_Fld_Message, System.Data.DbType.String);
            sqlParams[3] = DataFactory.CreateParameter("@" + clsPOSDBConstants.FMessage_Fld_MessageSub, System.Data.DbType.String);
            sqlParams[4] = DataFactory.CreateParameter("@" + clsPOSDBConstants.FMessage_Fld_MessageCatId, System.Data.DbType.Int32);
            sqlParams[5] = DataFactory.CreateParameter("@" + clsPOSDBConstants.FMessage_Fld_MessageTypeId, System.Data.DbType.Int32);

            sqlParams[0].SourceColumn = clsPOSDBConstants.FMessage_Fld_RecID;
            sqlParams[1].SourceColumn = clsPOSDBConstants.FMessage_Fld_MessageCode;
            sqlParams[2].SourceColumn = clsPOSDBConstants.FMessage_Fld_Message;
            sqlParams[3].SourceColumn = clsPOSDBConstants.FMessage_Fld_MessageSub;
            sqlParams[4].SourceColumn = clsPOSDBConstants.FMessage_Fld_MessageCatId;
            sqlParams[5].SourceColumn = clsPOSDBConstants.FMessage_Fld_MessageTypeId;

            if (row.RecID != System.Int32.MinValue)
                sqlParams[0].Value = row.RecID;
            else
                sqlParams[0].Value = DBNull.Value;

            sqlParams[1].Value = row.MessageCode.Trim();
            sqlParams[2].Value = row.Message.Trim();
            sqlParams[3].Value = row.MessageSub.Trim();
            sqlParams[4].Value = Configuration.convertNullToInt(row.MessageCatId) == 0 ? -1 : row.MessageCatId;
            sqlParams[5].Value = Configuration.convertNullToInt(row.MessageTypeId);

            return (sqlParams);
        }
        #endregion

        #region Get methods
        public MsgTemplateData Populate(System.Int32 RecID)
        {
            using (SqlConnection conn = new SqlConnection(POS_Core.Resources.Configuration.ConnectionString))
            {
                return (Populate(RecID, conn));
            }
        }

        public MsgTemplateData Populate(System.Int32 RecID, SqlConnection conn)
        {
            try
            {
                MsgTemplateData oMsgTemplateData = new MsgTemplateData();
                string strSQL = "SELECT " + clsPOSDBConstants.FMessage_Fld_RecID + ","
                    + clsPOSDBConstants.FMessage_Fld_MessageCode + "," + clsPOSDBConstants.FMessage_Fld_Message + ","
                    + clsPOSDBConstants.FMessage_Fld_MessageSub + "," + clsPOSDBConstants.FMessage_Fld_MessageCatId + ","
                    + clsPOSDBConstants.FMessage_Fld_MessageTypeId + " FROM " + clsPOSDBConstants.FMessage_tbl + " WHERE " +
                    clsPOSDBConstants.FMessage_Fld_RecID + " ='" + RecID + "'";

                oMsgTemplateData.MsgTemplate.MergeTable(DataHelper.ExecuteDataset(conn, CommandType.Text, strSQL, PKParameters(RecID)).Tables[0]);
                return oMsgTemplateData;
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
                logger.Fatal(ex, "PKParameters(System.Int32 RecID)");  //PRIMEPOS-2971 07-Jun-2021 JY Added
                //ErrorHandler.throwException(ex, "", "");
                return null;
            }
        }
        #endregion

        public void Dispose() { }
    }
}
