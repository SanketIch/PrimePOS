// ----------------------------------------------------------------
// Library: Data Access
// Author: Adeel Shehzad.
// Company: D-P-S, Inc. (www.d-p-s.com)
//
// ----------------------------------------------------------------
using System;
using System.Data;
using System.Data.SqlClient;
using POS_Core.CommonData;
using POS_Core.CommonData.Rows;
using POS_Core.CommonData.Tables;
using POS_Core.ErrorLogging;
using Resources;
using NLog;
using POS_Core.Resources;
//using POS.Resources;

namespace POS_Core.DataAccess
{
    

    // Provides data access methods for TaxCode

    public class ItemMonitorCategorySvr : IDisposable
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
            catch(POSExceptions ex)
            {
                logger.Fatal(ex, "Persist(DataSet updates, SqlTransaction tx)");
                throw (ex);
            }

            catch(OtherExceptions ex)
            {
                logger.Fatal(ex, "Persist(DataSet updates, SqlTransaction tx)");
                throw (ex);
            }

            catch(Exception ex)
            {
                logger.Fatal(ex, "Persist(DataSet updates, SqlTransaction tx)");
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
            catch(POSExceptions ex)
            {
                logger.Fatal(ex, "Persist(DataSet updates)");
                throw (ex);
            }

            catch(OtherExceptions ex)
            {
                logger.Fatal(ex, "Persist(DataSet updates)");
                throw (ex);
            }

            catch(Exception ex)
            {
                logger.Fatal(ex, "Persist(DataSet updates)");
                tx.Rollback();
                //ErrorHandler.throwException(ex, "", "");
            }
        }

        #endregion Persist Methods

        #region Get Methods

        // Looks up a TaxCode based on its primary-key:System.Int32 Taxcode

        public ItemMonitorCategoryData Populate(System.Int32 ID, SqlConnection conn)
        {
            try
            {
                ItemMonitorCategoryData ds = new ItemMonitorCategoryData();
                ds.ItemMonitorCategory.MergeTable(DataHelper.ExecuteDataset(conn, CommandType.Text, "Select * " +
                          " FROM " + clsPOSDBConstants.ItemMonitorCategory_tbl + " WHERE " + clsPOSDBConstants.ItemMonitorCategory_Fld_ID + " =" + ID.ToString(), PKParameters(ID)).Tables[0]);
                return ds;
            }
            catch(POSExceptions ex)
            {
                logger.Fatal(ex, "Populate(System.Int32 ID, SqlConnection conn)");
                throw (ex);
            }

            catch(OtherExceptions ex)
            {
                logger.Fatal(ex, "Populate(System.Int32 ID, SqlConnection conn)");
                throw (ex);
            }

            catch(Exception ex)
            {
                logger.Fatal(ex, "Populate(System.Int32 ID, SqlConnection conn)");
                //ErrorHandler.throwException(ex, "", "");
                return null;
            }
        }

        public ItemMonitorCategoryData Populate(System.Int32 ID)
        {
            using(SqlConnection conn = new SqlConnection(POS_Core.Resources.Configuration.ConnectionString))
            {
                return (Populate(ID, conn));
            }
        }

        public ItemMonitorCategoryData PopulateList(string sWhereClause, IDbConnection conn)
        {
            try
            {
                string sSQL = String.Concat("Select * " + " FROM " + clsPOSDBConstants.ItemMonitorCategory_tbl, sWhereClause);

                ItemMonitorCategoryData ds = new ItemMonitorCategoryData();
                ds.ItemMonitorCategory.MergeTable(DataHelper.ExecuteDataset(conn, CommandType.Text, sSQL, whereParameters(sWhereClause)).Tables[0]);
                return ds;
            }
            catch(POSExceptions ex)
            {
                logger.Fatal(ex, "PopulateList(string sWhereClause, IDbConnection conn)");
                throw (ex);
            }

            catch(OtherExceptions ex)
            {
                logger.Fatal(ex, "PopulateList(string sWhereClause, IDbConnection conn)");
                throw (ex);
            }

            catch(Exception ex)
            {
                logger.Fatal(ex, "PopulateList(string sWhereClause, IDbConnection conn)");
                //ErrorHandler.throwException(ex, "", "");
                return null;
            }
        }

        #region Sprint-25 - PRIMEPOS-2380 15-Feb-2017 JY Added
        public ItemMonitorCategoryData PopulateList(IDbConnection conn)
        {
            try
            {
                string sSQL = "SELECT * FROM " +
                            " (SELECT ID, Description, UserID, UOM, DailyLimit, ThirtyDaysLimit, MaxExempt, VerificationBy, LimitPeriodDays, LimitPeriodQty, AgeLimit, IsAgeLimit, ePSE FROM ItemMonitorCategory " +
                                " UNION " +
                                " SELECT 0 as ID, 'NPLEx' AS Description, 'POS' AS UserID, 2 AS UOM, 0 AS DailyLimit, 0 AS ThirtyDaysLimit, 0 AS MaxExempt, 'B' AS VerificationBy, 0 AS LimitPeriodDays, 0 AS LimitPeriodQty, 0 AS AgeLimit, CAST('FALSE' as bit) AS IsAgeLimit, CAST('TRUE' as bit) AS ePSE) AS IMT";

                ItemMonitorCategoryData ds = new ItemMonitorCategoryData();
                ds.ItemMonitorCategory.MergeTable(DataHelper.ExecuteDataset(conn, CommandType.Text, sSQL, whereParameters("")).Tables[0]);
                return ds;
            }
            catch (POSExceptions ex)
            {
                logger.Fatal(ex, "PopulateList(IDbConnection conn)");
                throw (ex);
            }

            catch (OtherExceptions ex)
            {
                logger.Fatal(ex, "PopulateList(IDbConnection conn)");
                throw (ex);
            }

            catch (Exception ex)
            {
                logger.Fatal(ex, "PopulateList(IDbConnection conn)");
                //ErrorHandler.throwException(ex, "", "");
                return null;
            }
        }
        #endregion

        // Fills a ItemMonitorCategoryData with all TaxCode

        public ItemMonitorCategoryData PopulateList(string whereClause)
        {
            using(IDbConnection conn = DataFactory.CreateConnection())
            {
                conn.ConnectionString = POS_Core.Resources.Configuration.ConnectionString;
                return (PopulateList(whereClause, conn));
            }
        }

        #region Sprint-25 - PRIMEPOS-2380 15-Feb-2017 JY Added
        public ItemMonitorCategoryData PopulateList()
        {
            using (IDbConnection conn = DataFactory.CreateConnection())
            {
                conn.ConnectionString = POS_Core.Resources.Configuration.ConnectionString;
                return (PopulateList(conn));
            }
        }
        #endregion

        public ItemMonitorCategoryData GetByItemID(System.String itemID)
        {
            IDbConnection conn = DataFactory.CreateConnection();
            conn.ConnectionString = POS_Core.Resources.Configuration.ConnectionString;
            return (GetByItemID(itemID, conn));
        }

        public ItemMonitorCategoryData GetByItemID(System.String itemID, IDbConnection conn)
        {
            try
            {
                string sSQL = "Select Master.*"
                                + " FROM "
                                    + clsPOSDBConstants.ItemMonitorCategoryDetail_tbl + " As Detail"
                                    + " , " + clsPOSDBConstants.ItemMonitorCategory_tbl + " As Master "
                                + " WHERE "
                                    + clsPOSDBConstants.Item_Fld_ItemID + " ='" + itemID + "'"
                                    + " AND Master." + clsPOSDBConstants.ItemMonitorCategory_Fld_ID + " = Detail." + clsPOSDBConstants.ItemMonitorCategory_Fld_ItemMonCatID;

                ItemMonitorCategoryData ds = new ItemMonitorCategoryData();
                ds.ItemMonitorCategory.MergeTable(DataHelper.ExecuteDataset(conn, CommandType.Text, sSQL).Tables[0]);
                return ds;
            }
            catch(POSExceptions ex)
            {
                logger.Fatal(ex, "GetByItemID(System.String itemID, IDbConnection conn)");
                throw (ex);
            }

            catch(OtherExceptions ex)
            {
                logger.Fatal(ex, "GetByItemID(System.String itemID, IDbConnection conn)");
                throw (ex);
            }

            catch(Exception ex)
            {
                logger.Fatal(ex, "GetByItemID(System.String itemID, IDbConnection conn)");
                //ErrorHandler.throwException(ex, "", "");
                return null;
            }
        }

        #region Sprint-18 - 2133 11-Nov-2014 JY Added
        public ItemMonitorCategoryData GetByItemID()
        {
            IDbConnection conn = DataFactory.CreateConnection();
            conn.ConnectionString = POS_Core.Resources.Configuration.ConnectionString;
            return (GetByItemID(conn));
        }

        public ItemMonitorCategoryData GetByItemID(IDbConnection conn)
        {
            try
            {
                string sSQL = "Select Master.* FROM " + clsPOSDBConstants.ItemMonitorCategory_tbl + " As Master";

                ItemMonitorCategoryData ds = new ItemMonitorCategoryData();
                ds.ItemMonitorCategory.MergeTable(DataHelper.ExecuteDataset(conn, CommandType.Text, sSQL).Tables[0]);
                return ds;
            }
            catch (POSExceptions ex)
            {
                logger.Fatal(ex, "GetByItemID(IDbConnection conn)");
                throw (ex);
            }

            catch (OtherExceptions ex)
            {
                logger.Fatal(ex, "GetByItemID(IDbConnection conn)");
                throw (ex);
            }

            catch (Exception ex)
            {
                logger.Fatal(ex, "GetByItemID(IDbConnection conn)");
                //ErrorHandler.throwException(ex, "", "");
                return null;
            }
        }
        #endregion

        #endregion Get Methods

        #region Insert, Update, and Delete Methods

        public void Insert(DataSet ds, SqlTransaction tx)
        {
            ItemMonitorCategoryTable addedTable = (ItemMonitorCategoryTable) ds.Tables[clsPOSDBConstants.ItemMonitorCategory_tbl].GetChanges(DataRowState.Added);
            string sSQL;
            IDbDataParameter []insParam;

            if(addedTable != null && addedTable.Rows.Count > 0)
            {
                foreach(ItemMonitorCategoryRow row in addedTable.Rows)
                {
                    try
                    {
                        insParam = InsertParameters(row);
                        sSQL = BuildInsertSQL(clsPOSDBConstants.ItemMonitorCategory_tbl, insParam);
                        DataHelper.ExecuteNonQuery(tx, CommandType.Text, sSQL, insParam);
                    }
                    catch(POSExceptions ex)
                    {
                        logger.Fatal(ex, "Insert(DataSet ds, SqlTransaction tx)");
                        throw (ex);
                    }

                    catch(OtherExceptions ex)
                    {
                        logger.Fatal(ex, "Insert(DataSet ds, SqlTransaction tx)");
                        throw (ex);
                    }

                    catch(Exception ex)
                    {
                        logger.Fatal(ex, "Insert(DataSet ds, SqlTransaction tx)");
                        //ErrorHandler.throwException(ex, "", "");
                    }
                }
                addedTable.AcceptChanges();
            }
        }

        // Update all rows in a ItemMonitorCategory DataSet, within a given database transaction.

        public void Update(DataSet ds, SqlTransaction tx)
        {
            ItemMonitorCategoryTable modifiedTable = (ItemMonitorCategoryTable) ds.Tables[clsPOSDBConstants.ItemMonitorCategory_tbl].GetChanges(DataRowState.Modified);

            string sSQL;
            IDbDataParameter []updParam;

            if(modifiedTable != null && modifiedTable.Rows.Count > 0)
            {
                foreach(ItemMonitorCategoryRow row in modifiedTable.Rows)
                {
                    try
                    {
                        updParam = UpdateParameters(row);
                        sSQL = BuildUpdateSQL(clsPOSDBConstants.ItemMonitorCategory_tbl, updParam);

                        DataHelper.ExecuteNonQuery(tx, CommandType.Text, sSQL, updParam);
                    }
                    catch(POSExceptions ex)
                    {
                        logger.Fatal(ex, "Update(DataSet ds, SqlTransaction tx)");
                        throw (ex);
                    }

                    catch(OtherExceptions ex)
                    {
                        logger.Fatal(ex, "Update(DataSet ds, SqlTransaction tx)");
                        throw (ex);
                    }

                    catch(Exception ex)
                    {
                        logger.Fatal(ex, "Update(DataSet ds, SqlTransaction tx)");
                        //ErrorHandler.throwException(ex, "", "");
                    }
                }
                modifiedTable.AcceptChanges();
            }
        }

        // Delete all rows within a ItemMonitorCategory DataSet, within a given database transaction.
        public void Delete(DataSet ds, SqlTransaction tx)
        {
            ItemMonitorCategoryTable table = (ItemMonitorCategoryTable) ds.Tables[clsPOSDBConstants.ItemMonitorCategory_tbl].GetChanges(DataRowState.Deleted);
            string sSQL;
            IDbDataParameter []delParam;

            if(table != null && table.Rows.Count > 0)
            {
                table.RejectChanges(); //so we can access the rows
                foreach(ItemMonitorCategoryRow row in table.Rows)
                {
                    try
                    {
                        delParam = PKParameters(row);

                        sSQL = BuildDeleteSQL(clsPOSDBConstants.ItemMonitorCategory_tbl, delParam);
                        DataHelper.ExecuteNonQuery(tx, CommandType.Text, sSQL, delParam);
                    }
                    catch(POSExceptions ex)
                    {
                        logger.Fatal(ex, "Delete(DataSet ds, SqlTransaction tx)");
                        throw (ex);
                    }

                    catch(OtherExceptions ex)
                    {
                        logger.Fatal(ex, "Delete(DataSet ds, SqlTransaction tx)");
                        throw (ex);
                    }

                    catch(Exception ex)
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
            for(int i = 0; i < delParam.Length; i++)
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

            for(int i = 1; i < delParam.Length; i++)
            {
                sInsertSQL = sInsertSQL + " , " + delParam[i].SourceColumn;
            }
            sInsertSQL = sInsertSQL + " , UserId ";
            sInsertSQL = sInsertSQL + " ) Values (" + delParam[0].ParameterName;

            for(int i = 1; i < delParam.Length; i++)
            {
                sInsertSQL = sInsertSQL + " , " + delParam[i].ParameterName;
            }
            sInsertSQL = sInsertSQL + " , '" + Configuration.UserName + "'";
            sInsertSQL = sInsertSQL + " )";
            return sInsertSQL;
        }

        private string BuildUpdateSQL(string tableName, IDbDataParameter[] updParam)
        {
            string sUpdateSQL = "UPDATE " + tableName + " SET ";
            // build where clause
            sUpdateSQL = sUpdateSQL + updParam[1].SourceColumn + "  = " + updParam[1].ParameterName;

            for(int i = 2; i < updParam.Length; i++)
            {
                sUpdateSQL = sUpdateSQL + " , " + updParam[i].SourceColumn + "  = " + updParam[i].ParameterName;
            }

            sUpdateSQL = sUpdateSQL + " , UserID  = '" + Configuration.UserName + "'";

            sUpdateSQL = sUpdateSQL + " WHERE " + updParam[0].SourceColumn + " = " + updParam[0].ParameterName;
            return sUpdateSQL;
        }

        #endregion Insert, Update, and Delete Methods

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

        private IDbDataParameter[] PKParameters(System.Int32 ID)
        {
            IDbDataParameter[] sqlParams = DataFactory.CreateParameterArray(1);

            sqlParams[0] = DataFactory.CreateParameter();
            sqlParams[0].ParameterName = "@ID";
            sqlParams[0].DbType = System.Data.DbType.Int32;
            sqlParams[0].Value = ID;

            return (sqlParams);
        }

        private IDbDataParameter[] PKParameters(ItemMonitorCategoryRow row)
        {
            //return a SqlParameterCollection
            IDbDataParameter[] sqlParams = DataFactory.CreateParameterArray(1);

            sqlParams[0] = DataFactory.CreateParameter();
            sqlParams[0].ParameterName = "@ID";
            sqlParams[0].DbType = System.Data.DbType.Int32;

            sqlParams[0].Value = row.ID;
            sqlParams[0].SourceColumn = clsPOSDBConstants.ItemMonitorCategory_Fld_ID;

            return (sqlParams);
        }

        private IDbDataParameter[] InsertParameters(ItemMonitorCategoryRow row)
        {
            //Added  VerificationBy column By shitaljit on 30 April 2012
            IDbDataParameter[] sqlParams = DataFactory.CreateParameterArray(12);    //Sprint-23 - PRIMEPOS-2029 30-Mar-2016 JY changed 10 to 11 //PRIMEPOS-3166 changed 11 to 12

            sqlParams[0] = DataFactory.CreateParameter("@" + clsPOSDBConstants.ItemMonitorCategory_Fld_Description, System.Data.DbType.String);
            sqlParams[1] = DataFactory.CreateParameter("@" + clsPOSDBConstants.ItemMonitorCategory_Fld_UOM, System.Data.DbType.String);
            sqlParams[2] = DataFactory.CreateParameter("@" + clsPOSDBConstants.ItemMonitorCategory_Fld_DailyLimit, System.Data.DbType.Int32);
            sqlParams[3] = DataFactory.CreateParameter("@" + clsPOSDBConstants.ItemMonitorCategory_Fld_ThirtyDaysLimit, System.Data.DbType.Int32);
            sqlParams[4] = DataFactory.CreateParameter("@" + clsPOSDBConstants.ItemMonitorCategory_Fld_MaxExempt, System.Data.DbType.Int32);
            sqlParams[5] = DataFactory.CreateParameter("@" + clsPOSDBConstants.ItemMonitorCategory_Fld_VerificationBy, System.Data.DbType.String);
            sqlParams[6] = DataFactory.CreateParameter("@" + clsPOSDBConstants.ItemMonitorCategory_Fld_LimitPeriodDays, System.Data.DbType.Int32);
            sqlParams[7] = DataFactory.CreateParameter("@" + clsPOSDBConstants.ItemMonitorCategory_Fld_LimitPeriodQty, System.Data.DbType.Decimal);
            sqlParams[8] = DataFactory.CreateParameter("@" + clsPOSDBConstants.ItemMonitorCategory_Fld_AgeLimit, System.Data.DbType.Int32);
            sqlParams[9] = DataFactory.CreateParameter("@" + clsPOSDBConstants.ItemMonitorCategory_Fld_IsAgeLimit, System.Data.DbType.Boolean);
            sqlParams[10] = DataFactory.CreateParameter("@" + clsPOSDBConstants.ItemMonitorCategory_Fld_ePSE, System.Data.DbType.Boolean);  //Sprint-23 - PRIMEPOS-2029 30-Mar-2016 JY Added
            sqlParams[11] = DataFactory.CreateParameter("@" + clsPOSDBConstants.ItemMonitorCategory_Fld_canOverrideMonitorItem, System.Data.DbType.Boolean); //PRIMEPOS-3166

            sqlParams[0].SourceColumn = clsPOSDBConstants.ItemMonitorCategory_Fld_Description;
            sqlParams[1].SourceColumn = clsPOSDBConstants.ItemMonitorCategory_Fld_UOM;
            sqlParams[2].SourceColumn = clsPOSDBConstants.ItemMonitorCategory_Fld_DailyLimit;
            sqlParams[3].SourceColumn = clsPOSDBConstants.ItemMonitorCategory_Fld_ThirtyDaysLimit;
            sqlParams[4].SourceColumn = clsPOSDBConstants.ItemMonitorCategory_Fld_MaxExempt;
            sqlParams[5].SourceColumn = clsPOSDBConstants.ItemMonitorCategory_Fld_VerificationBy;
            sqlParams[6].SourceColumn = clsPOSDBConstants.ItemMonitorCategory_Fld_LimitPeriodDays;
            sqlParams[7].SourceColumn = clsPOSDBConstants.ItemMonitorCategory_Fld_LimitPeriodQty;
            sqlParams[8].SourceColumn = clsPOSDBConstants.ItemMonitorCategory_Fld_AgeLimit;
            sqlParams[9].SourceColumn = clsPOSDBConstants.ItemMonitorCategory_Fld_IsAgeLimit;
            sqlParams[10].SourceColumn = clsPOSDBConstants.ItemMonitorCategory_Fld_ePSE;   //Sprint-23 - PRIMEPOS-2029 30-Mar-2016 JY Added
            sqlParams[11].SourceColumn = clsPOSDBConstants.ItemMonitorCategory_Fld_canOverrideMonitorItem; //PRIMEPOS-3166

            if (row.Description != System.String.Empty)
                sqlParams[0].Value = row.Description;
            else
                sqlParams[0].Value = DBNull.Value;

            if(row.UOM != System.String.Empty)
                sqlParams[1].Value = row.UOM;
            else
                sqlParams[1].Value = DBNull.Value;

            sqlParams[2].Value = row.DailyLimit;
            sqlParams[3].Value = row.ThirtyDaysLimit;
            sqlParams[4].Value = row.MaxExempt;

            if(row.VerificationBy != System.String.Empty)
                sqlParams[5].Value = row.VerificationBy;
            else
                sqlParams[5].Value = DBNull.Value;

            sqlParams[6].Value = row.LimitPeriodDays;
            sqlParams[7].Value = row.LimitPeriodQty;
            sqlParams[8].Value = row.AgeLimit;
            sqlParams[9].Value = row.IsAgeLimit;
            sqlParams[10].Value = row.ePSE; //Sprint-23 - PRIMEPOS-2029 30-Mar-2016 JY Added
            sqlParams[11].Value = row.canOverrideMonitorItem; //PRIMEPOS-3166

            return (sqlParams);
        }

        private IDbDataParameter[] UpdateParameters(ItemMonitorCategoryRow row)
        {
            //Added  VerificationBy column By shitaljit on 30 April 2012
            IDbDataParameter[] sqlParams = DataFactory.CreateParameterArray(13);    //Sprint-23 - PRIMEPOS-2029 30-Mar-2016 JY changed 11 to 12 //PRIMEPOS-3166 changed 12 to 13

            sqlParams[0] = DataFactory.CreateParameter("@" + clsPOSDBConstants.ItemMonitorCategory_Fld_ID, System.Data.DbType.Int32);
            sqlParams[1] = DataFactory.CreateParameter("@" + clsPOSDBConstants.ItemMonitorCategory_Fld_Description, System.Data.DbType.String);
            sqlParams[2] = DataFactory.CreateParameter("@" + clsPOSDBConstants.ItemMonitorCategory_Fld_UOM, System.Data.DbType.String);
            sqlParams[3] = DataFactory.CreateParameter("@" + clsPOSDBConstants.ItemMonitorCategory_Fld_DailyLimit, System.Data.DbType.Int32);
            sqlParams[4] = DataFactory.CreateParameter("@" + clsPOSDBConstants.ItemMonitorCategory_Fld_ThirtyDaysLimit, System.Data.DbType.Int32);
            sqlParams[5] = DataFactory.CreateParameter("@" + clsPOSDBConstants.ItemMonitorCategory_Fld_MaxExempt, System.Data.DbType.Int32);
            sqlParams[6] = DataFactory.CreateParameter("@" + clsPOSDBConstants.ItemMonitorCategory_Fld_VerificationBy, System.Data.DbType.String);
            sqlParams[7] = DataFactory.CreateParameter("@" + clsPOSDBConstants.ItemMonitorCategory_Fld_LimitPeriodDays, System.Data.DbType.Int32);
            sqlParams[8] = DataFactory.CreateParameter("@" + clsPOSDBConstants.ItemMonitorCategory_Fld_LimitPeriodQty, System.Data.DbType.Decimal);
            sqlParams[9] = DataFactory.CreateParameter("@" + clsPOSDBConstants.ItemMonitorCategory_Fld_AgeLimit, System.Data.DbType.Int32);
            sqlParams[10] = DataFactory.CreateParameter("@" + clsPOSDBConstants.ItemMonitorCategory_Fld_IsAgeLimit, System.Data.DbType.Boolean);
            sqlParams[11] = DataFactory.CreateParameter("@" + clsPOSDBConstants.ItemMonitorCategory_Fld_ePSE, System.Data.DbType.Boolean);  //Sprint-23 - PRIMEPOS-2029 30-Mar-2016 JY Added
            sqlParams[12] = DataFactory.CreateParameter("@" + clsPOSDBConstants.ItemMonitorCategory_Fld_canOverrideMonitorItem, System.Data.DbType.Boolean); //PRIMEPOS-3166

            sqlParams[0].SourceColumn = clsPOSDBConstants.ItemMonitorCategory_Fld_ID;
            sqlParams[1].SourceColumn = clsPOSDBConstants.ItemMonitorCategory_Fld_Description;
            sqlParams[2].SourceColumn = clsPOSDBConstants.ItemMonitorCategory_Fld_UOM;
            sqlParams[3].SourceColumn = clsPOSDBConstants.ItemMonitorCategory_Fld_DailyLimit;
            sqlParams[4].SourceColumn = clsPOSDBConstants.ItemMonitorCategory_Fld_ThirtyDaysLimit;
            sqlParams[5].SourceColumn = clsPOSDBConstants.ItemMonitorCategory_Fld_MaxExempt;
            sqlParams[6].SourceColumn = clsPOSDBConstants.ItemMonitorCategory_Fld_VerificationBy;
            sqlParams[7].SourceColumn = clsPOSDBConstants.ItemMonitorCategory_Fld_LimitPeriodDays;
            sqlParams[8].SourceColumn = clsPOSDBConstants.ItemMonitorCategory_Fld_LimitPeriodQty;
            sqlParams[9].SourceColumn = clsPOSDBConstants.ItemMonitorCategory_Fld_AgeLimit;
            sqlParams[10].SourceColumn = clsPOSDBConstants.ItemMonitorCategory_Fld_IsAgeLimit;
            sqlParams[11].SourceColumn = clsPOSDBConstants.ItemMonitorCategory_Fld_ePSE;    //Sprint-23 - PRIMEPOS-2029 30-Mar-2016 JY Added
            sqlParams[12].SourceColumn = clsPOSDBConstants.ItemMonitorCategory_Fld_canOverrideMonitorItem; //PRIMEPOS-3166

            sqlParams[0].Value = row.ID;
            if(row.Description != System.String.Empty)
                sqlParams[1].Value = row.Description;
            else
                sqlParams[1].Value = DBNull.Value;

            if(row.UOM != System.String.Empty)
                sqlParams[2].Value = row.UOM;
            else
                sqlParams[2].Value = DBNull.Value;

            sqlParams[3].Value = row.DailyLimit;
            sqlParams[4].Value = row.ThirtyDaysLimit;
            sqlParams[5].Value = row.MaxExempt;

            if(row.VerificationBy != System.String.Empty)
                sqlParams[6].Value = row.VerificationBy;
            else
                sqlParams[6].Value = DBNull.Value;

            sqlParams[7].Value = row.LimitPeriodDays;
            sqlParams[8].Value = row.LimitPeriodQty;
            sqlParams[9].Value = row.AgeLimit;
            sqlParams[10].Value = row.IsAgeLimit;
            sqlParams[11].Value = row.ePSE; //Sprint-23 - PRIMEPOS-2029 30-Mar-2016 JY Added
            sqlParams[12].Value = row.canOverrideMonitorItem; //PRIMEPOS-3166
            return (sqlParams);
        }

        #endregion IDBDataParameter Generator Methods

        public void Dispose() { }
    }
}