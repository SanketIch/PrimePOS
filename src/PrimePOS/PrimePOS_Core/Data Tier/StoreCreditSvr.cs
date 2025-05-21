using POS_Core.CommonData;
using POS_Core.CommonData.Rows;
using POS_Core.CommonData.Tables;
using POS_Core.ErrorLogging;
using Resources;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using POS_Core.Resources;
namespace POS_Core.Data_Tier
{


    public class StoreCreditSvr : IDisposable
    {

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
                ErrorLogging.ErrorHandler.throwException(ex, "", "");
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
                tx.Rollback();
                throw (ex);
            }
            catch (OtherExceptions ex)
            {
                tx.Rollback();
                throw (ex);
            }
            catch (Exception ex)
            {
                tx.Rollback();
                ErrorHandler.throwException(ex, "", "");
            }
        }

        public bool DeleteRow(string CurrentID)
        {
            string sSQL;
            try
            {
                DataTable dtItem = DataHelper.ExecuteDataset(DBConfig.ConnectionString, CommandType.Text, "Select StoreCreditID,CustomerID,CreditAmt, LastUpdated,LastUpdatedBy,ExpiresOn from StoreCredit where StoreCreditID = '" + CurrentID + "'").Tables[0];
                if (dtItem.Rows.Count == 0)
                {
                    sSQL = " delete from StoreCredit where StoreCreditID= '" + CurrentID + "'";
                    DataHelper.ExecuteNonQuery(DBConfig.ConnectionString, CommandType.Text, sSQL);
                    return true;
                }
                else
                    return false;
            }
            catch (Exception ex)
            {
                ErrorHandler.throwException(ex, "", "");
                return false;
            }
        }
        #endregion

        #region Get Methods

        // Looks up a DeptCode based on its primary-key:System.Int32 DeptCode

        public StoreCreditData Populate(System.Int32 ID, IDbConnection conn)
        {
            try
            {
                string sSQL = "Select StoreCreditID,CustomerID,CreditAmt, LastUpdated,LastUpdatedBy,ExpiresOn  "
                                + " FROM "
                                    + clsPOSDBConstants.StoreCredit_tbl
                                + " WHERE " + clsPOSDBConstants.StoreCredit_ID + " ='" + ID + "'";


                StoreCreditData ds = new StoreCreditData();
                ds.StoreCredit.MergeTable(DataHelper.ExecuteDataset(conn, CommandType.Text
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
                ErrorHandler.throwException(ex, "", "");
                return null;
            }
        }

        public StoreCreditData Populate(System.Int32 ID)
        {
            using (IDbConnection conn = DataFactory.CreateConnection(POS_Core.Resources.Configuration.ConnectionString))
            {
                return (Populate(ID, conn));
            }
        }


        public StoreCreditData GetByCustomerID(System.Int32 iCustomerID)
        {
            using (IDbConnection conn = DataFactory.CreateConnection(POS_Core.Resources.Configuration.ConnectionString))
            {
                return (PopulateList(" where CustomerID=" + iCustomerID.ToString() + " ORDER BY LastUpdated DESC", conn));
            }
        }

        public StoreCreditData GetByCustomerID(System.Int32 iCustomerID, IDbTransaction tx)
        {
            using (IDbConnection conn = DataFactory.CreateConnection(POS_Core.Resources.Configuration.ConnectionString))
            {
                return (PopulateList(" where CustomerID=" + iCustomerID.ToString() + " ORDER BY LastUpdated DESC", tx));
            }
        }

        public StoreCreditData PopulateList(string sWhereClause, IDbConnection conn)
        {
            try
            {
                string sSQL = String.Concat("Select StoreCreditID,CustomerID,CreditAmt, LastUpdated,LastUpdatedBy,ExpiresOn "
                                    + " FROM "
                                        + clsPOSDBConstants.StoreCredit_tbl
                                    , sWhereClause);

                StoreCreditData ds = new StoreCreditData();
                ds.StoreCredit.MergeTable(DataHelper.ExecuteDataset(conn, CommandType.Text, sSQL, whereParameters(sWhereClause)).Tables[0]);
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
                ErrorHandler.throwException(ex, "", "");
                return null;
            }
        }

        public StoreCreditData PopulateList(string sWhereClause, IDbTransaction tx)
        {
            try
            {
                string sSQL = String.Concat("Select StoreCreditID,CustomerID,CreditAmt, LastUpdated,LastUpdatedBy,ExpiresOn "
                                    + " FROM "
                                        + clsPOSDBConstants.StoreCredit_tbl
                                    , sWhereClause);

                StoreCreditData ds = new StoreCreditData();
                ds.StoreCredit.MergeTable(DataHelper.ExecuteDataset(tx, CommandType.Text, sSQL, whereParameters(sWhereClause)).Tables[0]);
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
                ErrorHandler.throwException(ex, "", "");
                return null;
            }
        }

        public StoreCreditData PopulateList(string whereClause)
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
            StoreCreditTable addedTable = (StoreCreditTable)ds.Tables[0].GetChanges(DataRowState.Added);
            string sSQL;
            IDbDataParameter[] insParam;

            if (addedTable != null && addedTable.Rows.Count > 0)
            {
                foreach (StoreCreditRow row in addedTable.Rows)
                {
                    try
                    {
                        insParam = InsertParameters(row);
                        sSQL = BuildInsertSQL(clsPOSDBConstants.StoreCredit_tbl, insParam);
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
                        throw (ex);
                    }
                    catch (Exception ex)
                    {
                        ErrorHandler.throwException(ex, "", "");
                    }
                }
                addedTable.AcceptChanges();
            }
        }

        // Update all rows in a DeptCodes DataSet, within a given database transaction.

        public void Update(DataSet ds, IDbTransaction tx)
        {
            StoreCreditTable modifiedTable = (StoreCreditTable)ds.Tables[0].GetChanges(DataRowState.Modified);

            string sSQL;
            IDbDataParameter[] updParam;

            if (modifiedTable != null && modifiedTable.Rows.Count > 0)
            {
                foreach (StoreCreditRow row in modifiedTable.Rows)
                {
                    try
                    {
                        updParam = UpdateParameters(row);
                        sSQL = BuildUpdateSQL(clsPOSDBConstants.CLCards_tbl, updParam);

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
                        ErrorHandler.throwException(ex, "", "");
                    }
                }
                modifiedTable.AcceptChanges();
            }
        }

        // Delete all rows within a DeptCodes DataSet, within a given database transaction.
        public void Delete(DataSet ds, IDbTransaction tx)
        {

            StoreCreditTable table = (StoreCreditTable)ds.Tables[0].GetChanges(DataRowState.Deleted);
            string sSQL;
            IDbDataParameter[] delParam;

            if (table != null && table.Rows.Count > 0)
            {
                table.RejectChanges(); //so we can access the rows
                foreach (StoreCreditRow row in table.Rows)
                {
                    try
                    {
                        delParam = PKParameters(row);

                        sSQL = BuildDeleteSQL(clsPOSDBConstants.CLCards_tbl, delParam);
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
                        ErrorHandler.throwException(ex, "", "");
                    }
                }
            }
        }


        public void UpdateCreditAmount(DataSet ds, IDbTransaction tx)
        {
            //IDbTransaction tx = null;
            try
            {
                IDbConnection conn = DataFactory.CreateConnection(POS_Core.Resources.Configuration.ConnectionString);
                //tx = conn.BeginTransaction();
                StoreCreditTable modifiedTable = (StoreCreditTable)ds.Tables[0].GetChanges(DataRowState.Modified);

                string sSQL;
                IDbDataParameter[] updParam;

                if (modifiedTable != null && modifiedTable.Rows.Count > 0)
                {
                    foreach (StoreCreditRow row in modifiedTable.Rows)
                    {
                        try
                        {
                            updParam = UpdateParameters(row);
                            sSQL = BuildUpdateSQL(clsPOSDBConstants.StoreCredit_tbl, updParam);

                            DataHelper.ExecuteNonQuery(tx, CommandType.Text, sSQL, updParam);
                        }
                        catch (POSExceptions ex)
                        {
                            //tx.Rollback();
                            throw (ex);
                        }
                        catch (OtherExceptions ex)
                        {
                            //tx.Rollback();
                            throw (ex);
                        }
                        catch (SqlException ex)
                        {
                            //tx.Rollback();
                            throw (ex);
                        }
                        catch (Exception ex)
                        {
                            //tx.Rollback();
                            ErrorHandler.throwException(ex, "", "");
                        }
                    }
                    //tx.Commit();
                    modifiedTable.AcceptChanges();
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
            //sInsertSQL = sInsertSQL + " , UserId, LastUpdatedOn ";
            sInsertSQL = sInsertSQL + " ) Values (" + delParam[0].ParameterName;

            for (int i = 1; i < delParam.Length; i++)
            {
                sInsertSQL = sInsertSQL + " , " + delParam[i].ParameterName;
            }
            //sInsertSQL = sInsertSQL + " , GetDate()";
            sInsertSQL = sInsertSQL + " )";
            return sInsertSQL;
        }

        private string BuildUpdateSQL(string tableName, IDbDataParameter[] updParam)
        {
            string sUpdateSQL = "UPDATE " + tableName + " SET ";
            // build where clause
            sUpdateSQL = sUpdateSQL + updParam[1].SourceColumn + "  = " + updParam[1].ParameterName;
            //sUpdateSQL = sUpdateSQL + updParam[0].SourceColumn + "  = " + updParam[0].ParameterName;

            for (int i = 2; i < updParam.Length; i++)
            {
                sUpdateSQL = sUpdateSQL + " , " + updParam[i].SourceColumn + "  = " + updParam[i].ParameterName;
            }

            sUpdateSQL = sUpdateSQL + " , LastUpdated=GetDate()";

            sUpdateSQL = sUpdateSQL + " WHERE " + updParam[0].SourceColumn + " = " + updParam[0].ParameterName;
            return sUpdateSQL;
        }
        #endregion


        private IDbDataParameter[] PKParameters(System.Int32 ID)
        {
            IDbDataParameter[] sqlParams = DataFactory.CreateParameterArray(1);

            sqlParams[0] = DataFactory.CreateParameter();
            sqlParams[0].ParameterName = "@ID";
            sqlParams[0].DbType = System.Data.DbType.Int32;
            sqlParams[0].Value = ID;

            return (sqlParams);
        }

        private IDbDataParameter[] PKParameters(StoreCreditRow row)
        {
            //return a SqlParameterCollection
            IDbDataParameter[] sqlParams = DataFactory.CreateParameterArray(1);


            sqlParams[0] = DataFactory.CreateParameter();
            sqlParams[0].ParameterName = "@ID";
            sqlParams[0].DbType = System.Data.DbType.Int32;

            sqlParams[0].Value = row.StoreCreditID;
            sqlParams[0].SourceColumn = clsPOSDBConstants.StoreCredit_ID;

            return (sqlParams);
        }

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

        private IDbDataParameter[] UpdateParameters(StoreCreditRow row)
        {
            IDbDataParameter[] sqlParams = DataFactory.CreateParameterArray(2);

            //sqlParams[0] = DataFactory.CreateParameter("@" + clsPOSDBConstants.StoreCredit_ID, System.Data.DbType.Int32);
            sqlParams[0] = DataFactory.CreateParameter("@" + clsPOSDBConstants.StoreCredit_CustomerID, System.Data.DbType.Int32);
            sqlParams[1] = DataFactory.CreateParameter("@" + clsPOSDBConstants.StoreCredit_CreditAmt, System.Data.DbType.Decimal);
            //sqlParams[3] = DataFactory.CreateParameter("@" + clsPOSDBConstants.StoreCredit_LastUpdated, System.Data.DbType.DateTime);
            //sqlParams[4] = DataFactory.CreateParameter("@" + clsPOSDBConstants.StoreCredit_LastUpdatedBy, System.Data.DbType.String);
            //sqlParams[5] = DataFactory.CreateParameter("@" + clsPOSDBConstants.StoreCredit_ExpiresOn, System.Data.DbType.Int32);

            //sqlParams[0].SourceColumn = clsPOSDBConstants.StoreCredit_ID;
            sqlParams[0].SourceColumn = clsPOSDBConstants.StoreCredit_CustomerID;
            sqlParams[1].SourceColumn = clsPOSDBConstants.StoreCredit_CreditAmt;
            //sqlParams[3].SourceColumn = clsPOSDBConstants.StoreCredit_LastUpdated;
            //sqlParams[4].SourceColumn = clsPOSDBConstants.StoreCredit_LastUpdatedBy;
            //sqlParams[5].SourceColumn = clsPOSDBConstants.StoreCredit_ExpiresOn;

            //if (row.StoreCreditID != 0)
            //    sqlParams[0].Value = row.StoreCreditID;
            //else
            //    sqlParams[0].Value = 0;

            sqlParams[0].Value = row.CustomerID;

            sqlParams[1].Value = row.CreditAmt;
            //sqlParams[3].Value = row.LastUpdated;

            //sqlParams[4].Value = row.LastUpdatedBy;
            //sqlParams[5].Value = row.ExpiresOn;
            return (sqlParams);
        }


        private IDbDataParameter[] InsertParameters(StoreCreditRow row)
        {
            IDbDataParameter[] sqlParams = DataFactory.CreateParameterArray(5);

            //sqlParams[0] = DataFactory.CreateParameter("@" + clsPOSDBConstants.StoreCredit_ID, System.Data.DbType.Int32);
            sqlParams[0] = DataFactory.CreateParameter("@" + clsPOSDBConstants.StoreCredit_CustomerID, System.Data.DbType.Int32);
            sqlParams[1] = DataFactory.CreateParameter("@" + clsPOSDBConstants.StoreCredit_CreditAmt, System.Data.DbType.Decimal);
            sqlParams[2] = DataFactory.CreateParameter("@" + clsPOSDBConstants.StoreCredit_LastUpdated, System.Data.DbType.DateTime);
            sqlParams[3] = DataFactory.CreateParameter("@" + clsPOSDBConstants.StoreCredit_LastUpdatedBy, System.Data.DbType.String);
            sqlParams[4] = DataFactory.CreateParameter("@" + clsPOSDBConstants.StoreCredit_ExpiresOn, System.Data.DbType.Int32);


            //sqlParams[0].SourceColumn = clsPOSDBConstants.StoreCredit_ID;
            sqlParams[0].SourceColumn = clsPOSDBConstants.StoreCredit_CustomerID;
            sqlParams[1].SourceColumn = clsPOSDBConstants.StoreCredit_CreditAmt;
            sqlParams[2].SourceColumn = clsPOSDBConstants.StoreCredit_LastUpdated;
            sqlParams[3].SourceColumn = clsPOSDBConstants.StoreCredit_LastUpdatedBy;
            sqlParams[4].SourceColumn = clsPOSDBConstants.StoreCredit_ExpiresOn;

            //if (row.StoreCreditID != 0)
            //    sqlParams[0].Value = row.StoreCreditID;
            //else
            //    sqlParams[0].Value = 0;

            sqlParams[0].Value = row.CustomerID;
            sqlParams[1].Value = row.CreditAmt;
            sqlParams[2].Value = row.LastUpdated;
            sqlParams[3].Value = row.LastUpdatedBy;
            sqlParams[4].Value = row.ExpiresOn;

            return (sqlParams);
        }
        public void Dispose() { }
    }


}
