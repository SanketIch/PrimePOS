//----------------------------------------------------------------------------------------------------
//Sprint-23 - PRIMEPOS-2029 19-Apr-2016 JY Added to maintain item monitor trans log
//----------------------------------------------------------------------------------------------------
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
using NLog;
namespace POS_Core.DataAccess
{
    

    class ItemMonitorTransDetailSvr : IDisposable
    {
        private static ILogger logger = LogManager.GetCurrentClassLogger();

        #region Persist Methods
        // Inserts, updates or deletes rows in a DataSet, within a database transaction.
        public void Persist(DataSet updates, IDbTransaction tx)
        {
            try
            {
                //this.Delete(updates, tx);
                this.Insert(updates, tx);
                //this.Update(updates, tx);

                updates.AcceptChanges();
            }
            catch (POSExceptions ex)
            {
                logger.Fatal(ex, "Persist(DataSet updates, IDbTransaction tx)");
                throw (ex);
            }
            catch (OtherExceptions ex)
            {
                logger.Fatal(ex, "Persist(DataSet updates, IDbTransaction tx)");
                throw (ex);
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "Persist(DataSet updates, IDbTransaction tx)");
                POS_Core.ErrorLogging.ErrorHandler.throwException(ex, "", "");
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
                if (conn.State == ConnectionState.Open) 
                    conn.Close();
            }
            catch (POSExceptions ex)
            {
                logger.Fatal(ex, "Persist(DataSet updates)");
                throw (ex);
            }
            catch (OtherExceptions ex)
            {
                logger.Fatal(ex, "Persist(DataSet updates)");
                throw (ex);
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "Persist(DataSet updates)");
                tx.Rollback();
                ErrorHandler.throwException(ex, "", "");
            }
        }
        #endregion

        #region Get Methods
        public ItemMonitorTransDetailData Populate(System.Int64 ID)
        {
            using (IDbConnection conn = DataFactory.CreateConnection(POS_Core.Resources.Configuration.ConnectionString))
            {
                return (Populate(ID, conn));
            }
        }

        public ItemMonitorTransDetailData Populate(System.Int64 ID, IDbConnection conn)
        {
            try
            {
                string sSQL = "Select * "
                                + " FROM "
                                    + clsPOSDBConstants.ItemMonitorTransDetail_tbl
                                + " WHERE " + clsPOSDBConstants.ItemMonitorTransDetail_Fld_Id + " = " + ID;

                ItemMonitorTransDetailData ds = new ItemMonitorTransDetailData();
                ds.ItemMonitorTransDetail.MergeTable(DataHelper.ExecuteDataset(conn, CommandType.Text
                                            , sSQL
                                            , PKParameters(ID)).Tables[0]);
                return ds;
            }
            catch (POSExceptions ex)
            {
                logger.Fatal(ex, "Populate(System.Int64 ID, IDbConnection conn)");
                throw (ex);
            }
            catch (OtherExceptions ex)
            {
                logger.Fatal(ex, "Populate(System.Int64 ID, IDbConnection conn)");
                throw (ex);
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "Populate(System.Int64 ID, IDbConnection conn)");
                ErrorHandler.throwException(ex, "", "");
                return null;
            }
        }
        #endregion //Get Method

        #region Insert, Update, and Delete Methods
        public void Insert(DataSet ds, IDbTransaction tx)
        {

            ItemMonitorTransDetailTable addedTable = (ItemMonitorTransDetailTable)ds.Tables[0].GetChanges(DataRowState.Added);
            string sSQL;
            IDbDataParameter[] insParam;

            if (addedTable != null && addedTable.Rows.Count > 0)
            {
                foreach (ItemMonitorTransDetailRow row in addedTable.Rows)
                {
                    try
                    {
                        insParam = InsertParameters(row);
                        sSQL = BuildInsertSQL(clsPOSDBConstants.ItemMonitorTransDetail_tbl, insParam);

                        DataHelper.ExecuteNonQuery(tx, CommandType.Text, sSQL, insParam);
                    }
                    catch (POSExceptions ex)
                    {
                        logger.Fatal(ex, "Insert(DataSet ds, IDbTransaction tx)");
                        throw (ex);
                    }
                    catch (OtherExceptions ex)
                    {
                        logger.Fatal(ex, "Insert(DataSet ds, IDbTransaction tx)");
                        throw (ex);
                    }
                    catch (SqlException ex)
                    {
                        logger.Fatal(ex, "Insert(DataSet ds, IDbTransaction tx)");
                        throw (ex);
                    }
                    catch (Exception ex)
                    {
                        logger.Fatal(ex, "Insert(DataSet ds, IDbTransaction tx)");
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

        public bool DeleteRow(long CurrentID)
        {
            try
            {
                string sDeleteSQL = "DELETE FROM " + clsPOSDBConstants.ItemMonitorTransDetail_tbl + " WHERE ID = " + CurrentID;
                DataHelper.ExecuteNonQuery(sDeleteSQL);
                return true;
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "DeleteRow(long CurrentID)");
                ErrorHandler.throwException(ex, "", "");
                return false;
            }
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
            sqlParams[0].DbType = System.Data.DbType.Int32;
            sqlParams[0].Value = ID;

            return (sqlParams);
        }

        private IDbDataParameter[] PKParameters(ItemMonitorTransDetailRow row)
        {
            //return a SqlParameterCollection
            IDbDataParameter[] sqlParams = DataFactory.CreateParameterArray(1);

            sqlParams[0] = DataFactory.CreateParameter();
            sqlParams[0].ParameterName = "@ID";
            sqlParams[0].DbType = System.Data.DbType.Int32;

            sqlParams[0].Value = row.ID;
            sqlParams[0].SourceColumn = clsPOSDBConstants.ItemMonitorTransDetail_Fld_Id;

            return (sqlParams);
        }

        private IDbDataParameter[] InsertParameters(ItemMonitorTransDetailRow row)
        {
            IDbDataParameter[] sqlParams = DataFactory.CreateParameterArray(7);

            sqlParams[0] = DataFactory.CreateParameter("@" + clsPOSDBConstants.ItemMonitorTransDetail_Fld_TransDetailID, System.Data.DbType.Int32);
            sqlParams[1] = DataFactory.CreateParameter("@" + clsPOSDBConstants.ItemMonitorTransDetail_Fld_ItemMonCatID, System.Data.DbType.Int32);
            sqlParams[2] = DataFactory.CreateParameter("@" + clsPOSDBConstants.ItemMonitorTransDetail_Fld_UOM, System.Data.DbType.String);
            sqlParams[3] = DataFactory.CreateParameter("@" + clsPOSDBConstants.ItemMonitorTransDetail_Fld_UnitsPerPackage, System.Data.DbType.Decimal);
            sqlParams[4] = DataFactory.CreateParameter("@" + clsPOSDBConstants.ItemMonitorTransDetail_Fld_SentToNplex, System.Data.DbType.Boolean);
            sqlParams[5] = DataFactory.CreateParameter("@" + clsPOSDBConstants.ItemMonitorTransDetail_Fld_PostSaleInd, System.Data.DbType.Boolean);
            sqlParams[6] = DataFactory.CreateParameter("@" + clsPOSDBConstants.ItemMonitorTransDetail_Fld_pseTrxId, System.Data.DbType.Int64);

            sqlParams[0].SourceColumn = clsPOSDBConstants.ItemMonitorTransDetail_Fld_TransDetailID;
            sqlParams[1].SourceColumn = clsPOSDBConstants.ItemMonitorTransDetail_Fld_ItemMonCatID;
            sqlParams[2].SourceColumn = clsPOSDBConstants.ItemMonitorTransDetail_Fld_UOM;
            sqlParams[3].SourceColumn = clsPOSDBConstants.ItemMonitorTransDetail_Fld_UnitsPerPackage;
            sqlParams[4].SourceColumn = clsPOSDBConstants.ItemMonitorTransDetail_Fld_SentToNplex;
            sqlParams[5].SourceColumn = clsPOSDBConstants.ItemMonitorTransDetail_Fld_PostSaleInd;
            sqlParams[6].SourceColumn = clsPOSDBConstants.ItemMonitorTransDetail_Fld_pseTrxId;

            sqlParams[0].Value = row.TransDetailID;
            sqlParams[1].Value = row.ItemMonCatID;
            sqlParams[2].Value = row.UOM;
            sqlParams[3].Value = row.UnitsPerPackage;
            sqlParams[4].Value = row.SentToNplex;
            sqlParams[5].Value = row.PostSaleInd;
            sqlParams[6].Value = row.pseTrxId;
            return (sqlParams);
        }
        #endregion

        public void Dispose() { }
    }
}
