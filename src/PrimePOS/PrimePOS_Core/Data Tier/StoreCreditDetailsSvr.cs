using POS_Core.CommonData;
using POS_Core.CommonData.Rows;
using POS_Core.CommonData.Tables;
using POS_Core.ErrorLogging;
using POS_Core.Resources;
using Resources;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS_Core.DataAccess
{
    public class StoreCreditDetailsSvr : IDisposable
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
                ErrorHandler.throwException(ex, "", "");
            }
        }


        // Inserts, updates or deletes rows in a DataSet.

        public void Persist(DataSet updates)
        {

            IDbTransaction tx = null;
            try
            {
                IDbConnection conn = DataFactory.CreateConnection(Configuration.ConnectionString);
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
                DataTable dtItem = DataHelper.ExecuteDataset(DBConfig.ConnectionString, CommandType.Text, "Select StoreCreditDetailsID,StoreCreditID,CustomerID,TransactionID,CreditAmt,UpdatedOn,UpdatedBy from StoreCreditDetails where StoreCreditDetailsID = '" + CurrentID + "'").Tables[0];
                if (dtItem.Rows.Count == 0)
                {
                    sSQL = " delete from StoreCreditDetails where StoreCreditDetailsID= '" + CurrentID + "'";
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

        public StoreCreditDetailsData Populate(System.Int32 ID, IDbConnection conn)
        {
            try
            {
                string sSQL = "Select  StoreCreditDetailsID,StoreCreditID,CustomerID,TransactionID,CreditAmt,UpdatedOn,UpdatedBy  "
                                + " FROM "
                                    + clsPOSDBConstants.StoreCreditDetails_tbl
                                + " WHERE " + clsPOSDBConstants.StoreCreditDetails_ID + " ='" + ID + "'";


                StoreCreditDetailsData ds = new StoreCreditDetailsData();
                ds.StoreCreditDetails.MergeTable(DataHelper.ExecuteDataset(conn, CommandType.Text
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

        public StoreCreditDetailsData Populate(System.Int32 ID)
        {
            using (IDbConnection conn = DataFactory.CreateConnection(POS_Core.Resources.Configuration.ConnectionString))
            {
                return (Populate(ID, conn));
            }
        }


        public StoreCreditDetailsData GetByCustomerID(System.Int32 iCustomerID)
        {
            using (IDbConnection conn = DataFactory.CreateConnection(POS_Core.Resources.Configuration.ConnectionString))
            {

                return (PopulateList(" where CustomerID=" + iCustomerID.ToString() + " ORDER BY UpdatedOn DESC", conn));
            }
        }





        public StoreCreditDetailsData PopulateList(string sWhereClause, IDbConnection conn)
        {
            try
            {
                string sSQL = String.Concat("Select StoreCreditDetailsID, StoreCreditID, CustomerID, TransactionID, CreditAmt,UpdatedOn,UpdatedBy, SUM(CreditAmt) OVER(PARTITION BY CustomerID ORDER BY TransactionID ) RemainingAmount  "
                                    + " FROM "
                                        + clsPOSDBConstants.StoreCreditDetails_tbl
                                    , sWhereClause);

                StoreCreditDetailsData ds = new StoreCreditDetailsData();
                ds.StoreCreditDetails.MergeTable(DataHelper.ExecuteDataset(conn, CommandType.Text, sSQL, whereParameters(sWhereClause)).Tables[0]);
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

        public StoreCreditDetailsData PopulateList(string sWhereClause, IDbTransaction tx)
        {
            try
            {
                string sSQL = String.Concat("Select  StoreCreditDetailsID,StoreCreditID,CustomerID,TransactionID,CreditAmt,UpdatedOn,UpdatedBy  "
                                    + " FROM "
                                        + clsPOSDBConstants.StoreCreditDetails_tbl
                                    , sWhereClause);

                StoreCreditDetailsData ds = new StoreCreditDetailsData();
                ds.StoreCreditDetails.MergeTable(DataHelper.ExecuteDataset(tx, CommandType.Text, sSQL, whereParameters(sWhereClause)).Tables[0]);
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

        public StoreCreditDetailsData PopulateList(string whereClause)
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

            StoreCreditDetailsTable addedTable = (StoreCreditDetailsTable)ds.Tables[0].GetChanges(DataRowState.Added);
            string sSQL;
            IDbDataParameter[] insParam;

            if (addedTable != null && addedTable.Rows.Count > 0)
            {
                foreach (StoreCreditDetailsRow row in addedTable.Rows)
                {
                    try
                    {

                        insParam = InsertParameters(row);
                        sSQL = BuildInsertSQL(clsPOSDBConstants.StoreCreditDetails_tbl, insParam);
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
                foreach (StoreCreditDetailsRow row in modifiedTable.Rows)
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
                foreach (StoreCreditDetailsRow row in table.Rows)
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

        private IDbDataParameter[] PKParameters(StoreCreditDetailsRow row)
        {
            //return a SqlParameterCollection
            IDbDataParameter[] sqlParams = DataFactory.CreateParameterArray(1);


            sqlParams[0] = DataFactory.CreateParameter();
            sqlParams[0].ParameterName = "@ID";
            sqlParams[0].DbType = System.Data.DbType.Int32;

            sqlParams[0].Value = row.StoreCreditID;
            sqlParams[0].SourceColumn = clsPOSDBConstants.StoreCreditDetails_ID;

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

        private IDbDataParameter[] UpdateParameters(StoreCreditDetailsRow row)
        {
            IDbDataParameter[] sqlParams = DataFactory.CreateParameterArray(7);
            sqlParams[0] = DataFactory.CreateParameter("@" + clsPOSDBConstants.StoreCreditDetails_ID, System.Data.DbType.Int32);
            sqlParams[1] = DataFactory.CreateParameter("@" + clsPOSDBConstants.StoreCreditDetails_CustomerID, System.Data.DbType.Int32);
            sqlParams[2] = DataFactory.CreateParameter("@" + clsPOSDBConstants.StoreCreditDetails_CreditAmt, System.Data.DbType.Decimal);
            sqlParams[3] = DataFactory.CreateParameter("@" + clsPOSDBConstants.StoreCreditDetails_StoreCreditID, System.Data.DbType.Int32);
            sqlParams[4] = DataFactory.CreateParameter("@" + clsPOSDBConstants.StoreCreditDetails_TransactionID, System.Data.DbType.Int32);
            sqlParams[5] = DataFactory.CreateParameter("@" + clsPOSDBConstants.StoreCreditDetails_UpdatedOn, System.Data.DbType.DateTime);
            sqlParams[6] = DataFactory.CreateParameter("@" + clsPOSDBConstants.StoreCreditDetails_UpdatedBy, System.Data.DbType.String);

            sqlParams[0].SourceColumn = clsPOSDBConstants.StoreCreditDetails_ID;
            sqlParams[1].SourceColumn = clsPOSDBConstants.StoreCreditDetails_CustomerID;
            sqlParams[2].SourceColumn = clsPOSDBConstants.StoreCredit_CreditAmt;
            sqlParams[3].SourceColumn = clsPOSDBConstants.StoreCreditDetails_StoreCreditID;
            sqlParams[4].SourceColumn = clsPOSDBConstants.StoreCreditDetails_TransactionID;
            sqlParams[5].SourceColumn = clsPOSDBConstants.StoreCreditDetails_UpdatedOn;
            sqlParams[6].SourceColumn = clsPOSDBConstants.StoreCreditDetails_UpdatedBy;

            if (row.StoreCreditDetailsID != 0)
                sqlParams[0].Value = row.StoreCreditDetailsID;
            else
                sqlParams[0].Value = 0;

            sqlParams[1].Value = row.CustomerID;
            sqlParams[2].Value = row.CreditAmt;
            sqlParams[3].Value = row.StoreCreditID;
            sqlParams[4].Value = row.TransactionID;
            sqlParams[5].Value = row.UpdatedOn;
            sqlParams[6].Value = row.UpdatedBy;
            return (sqlParams);
        }


        private IDbDataParameter[] InsertParameters(StoreCreditDetailsRow row)
        {
            IDbDataParameter[] sqlParams = DataFactory.CreateParameterArray(6);

            //sqlParams[0] = DataFactory.CreateParameter("@" + clsPOSDBConstants.StoreCreditDetails_ID, System.Data.DbType.Int32);
            sqlParams[0] = DataFactory.CreateParameter("@" + clsPOSDBConstants.StoreCreditDetails_CustomerID, System.Data.DbType.Int32);
            sqlParams[1] = DataFactory.CreateParameter("@" + clsPOSDBConstants.StoreCreditDetails_CreditAmt, System.Data.DbType.Decimal);
            sqlParams[2] = DataFactory.CreateParameter("@" + clsPOSDBConstants.StoreCreditDetails_StoreCreditID, System.Data.DbType.Int32);
            sqlParams[3] = DataFactory.CreateParameter("@" + clsPOSDBConstants.StoreCreditDetails_TransactionID, System.Data.DbType.Int32);
            sqlParams[4] = DataFactory.CreateParameter("@" + clsPOSDBConstants.StoreCreditDetails_UpdatedOn, System.Data.DbType.DateTime);
            sqlParams[5] = DataFactory.CreateParameter("@" + clsPOSDBConstants.StoreCreditDetails_UpdatedBy, System.Data.DbType.String);

            //sqlParams[0].SourceColumn = clsPOSDBConstants.StoreCreditDetails_ID;
            sqlParams[0].SourceColumn = clsPOSDBConstants.StoreCreditDetails_CustomerID;
            sqlParams[1].SourceColumn = clsPOSDBConstants.StoreCreditDetails_CreditAmt;
            sqlParams[2].SourceColumn = clsPOSDBConstants.StoreCreditDetails_StoreCreditID;
            sqlParams[3].SourceColumn = clsPOSDBConstants.StoreCreditDetails_TransactionID;
            sqlParams[4].SourceColumn = clsPOSDBConstants.StoreCreditDetails_UpdatedOn;
            sqlParams[5].SourceColumn = clsPOSDBConstants.StoreCreditDetails_UpdatedBy;

            //if (row.StoreCreditDetailsID != 0)
            //    sqlParams[0].Value = row.StoreCreditDetailsID;
            //else
            //    sqlParams[0].Value = 0;

            sqlParams[0].Value = row.CustomerID;
            sqlParams[1].Value = row.CreditAmt;
            sqlParams[2].Value = row.StoreCreditID;
            sqlParams[3].Value = row.TransactionID;
            sqlParams[4].Value = row.UpdatedOn;
            sqlParams[5].Value = row.UpdatedBy;
            return (sqlParams);
        }
        public void Dispose() { }
    }
}
