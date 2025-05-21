//Sprint-25 - PRIMEPOS-2379 10-Mar-2017 JY Added
using System;
using System.Data;
using System.Data.SqlClient;
using POS_Core.ErrorLogging;
using POS_Core.CommonData.Tables;
using POS_Core.CommonData.Rows;
using POS_Core.CommonData;
using Resources;
using POS_Core.Resources;
using NLog;

namespace POS_Core.DataAccess
{
    

    public class PSEItemSvr : IDisposable
    {
        private static ILogger logger = LogManager.GetCurrentClassLogger();

        #region Persist Methods
        // Inserts, updates or deletes rows in a DataSet, within a database transaction.
        public void Persist(DataSet updates, SqlTransaction tx)
        {
            try
            {
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
        // Looks up a TaxCode based on its primary-key:System.Int32 Taxcode
        public PSEItemData Populate(System.Int32 Id, SqlConnection conn)
        {
            try
            {
                PSEItemData ds = new PSEItemData();
                ds.PSEItem.MergeTable(DataHelper.ExecuteDataset(conn, CommandType.Text, "Select " + clsPOSDBConstants.PSE_Items_Fld_Id +
                    "," + clsPOSDBConstants.PSE_Items_Fld_ProductId + "," + clsPOSDBConstants.PSE_Items_Fld_ProductName + "," +
                    clsPOSDBConstants.PSE_Items_Fld_ProductNDC + "," + clsPOSDBConstants.PSE_Items_Fld_ProductGrams + "," + 
                    clsPOSDBConstants.PSE_Items_Fld_ProductPillCnt + "," + clsPOSDBConstants.PSE_Items_Fld_CreatedBy + "," + 
                    clsPOSDBConstants.PSE_Items_Fld_CreatedOn + "," + clsPOSDBConstants.PSE_Items_Fld_UpdatedBy + "," + 
                    clsPOSDBConstants.PSE_Items_Fld_UpdatedOn + "," + clsPOSDBConstants.PSE_Items_Fld_IsActive + "," +
                    clsPOSDBConstants.PSE_Items_Fld_RecordStatus +
                    " FROM " + clsPOSDBConstants.PSE_Items_tbl + " WHERE " +
                    clsPOSDBConstants.PSE_Items_Fld_Id + "=" + Id, PKParameters(Id.ToString())).Tables[0]);
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
                logger.Fatal(ex, "Populate(System.Int32 Id, SqlConnection conn)");  //PRIMEPOS-2971 07-Jun-2021 JY Added
                //ErrorHandler.throwException(ex, "", "");
                return null;
            }
        }

        public PSEItemData Populate(System.Int32 Id)
        {
            using (SqlConnection conn = new SqlConnection(POS_Core.Resources.Configuration.ConnectionString))
            {
                return (Populate(Id, conn));
            }
        }

        public PSEItemData Populate(System.String ProductId, SqlConnection conn)
        {
            try
            {
                PSEItemData ds = new PSEItemData();
                ds.PSEItem.MergeTable(DataHelper.ExecuteDataset(conn, CommandType.Text, "Select " + clsPOSDBConstants.PSE_Items_Fld_Id + ","
                    + clsPOSDBConstants.PSE_Items_Fld_ProductId + "," + clsPOSDBConstants.PSE_Items_Fld_ProductName + ","
                    + clsPOSDBConstants.PSE_Items_Fld_ProductNDC + "," + clsPOSDBConstants.PSE_Items_Fld_ProductGrams + "," 
                    + clsPOSDBConstants.PSE_Items_Fld_ProductPillCnt + "," + clsPOSDBConstants.PSE_Items_Fld_CreatedBy + "," 
                    + clsPOSDBConstants.PSE_Items_Fld_CreatedOn + "," + clsPOSDBConstants.PSE_Items_Fld_UpdatedBy + "," 
                    + clsPOSDBConstants.PSE_Items_Fld_UpdatedOn + "," + clsPOSDBConstants.PSE_Items_Fld_IsActive +
                    ", CASE WHEN RecordStatus = 'M' THEN 'Manual' WHEN RecordStatus = 'E' THEN 'Electronic' ELSE '' END AS RecordStatus " +
                    " FROM " + clsPOSDBConstants.PSE_Items_tbl + " WHERE " +
                    clsPOSDBConstants.PSE_Items_Fld_ProductId + " ='" + ProductId + "'", PKParameters(ProductId)).Tables[0]);
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
                logger.Fatal(ex, "Populate(System.String ProductId, SqlConnection conn)");  //PRIMEPOS-2971 07-Jun-2021 JY Added
                //ErrorHandler.throwException(ex, "", "");
                return null;
            }
        }

        public PSEItemData Populate(System.String ProductId)
        {
            using (SqlConnection conn = new SqlConnection(POS_Core.Resources.Configuration.ConnectionString))
            {
                return (Populate(ProductId, conn));
            }
        }

        // Fills a PSEItemData with all ProductId
        public PSEItemData PopulateList(string sWhereClause, IDbConnection conn)
        {
            try
            {
                string sSQL = String.Concat("Select " + clsPOSDBConstants.PSE_Items_Fld_Id + ","
                    + clsPOSDBConstants.PSE_Items_Fld_ProductId + "," + clsPOSDBConstants.PSE_Items_Fld_ProductName + ","
                    + clsPOSDBConstants.PSE_Items_Fld_ProductNDC + "," + clsPOSDBConstants.PSE_Items_Fld_ProductGrams + "," 
                    + clsPOSDBConstants.PSE_Items_Fld_ProductPillCnt + "," + clsPOSDBConstants.PSE_Items_Fld_CreatedBy + "," 
                    + clsPOSDBConstants.PSE_Items_Fld_CreatedOn + "," + clsPOSDBConstants.PSE_Items_Fld_UpdatedBy + "," 
                    + clsPOSDBConstants.PSE_Items_Fld_UpdatedOn + "," + clsPOSDBConstants.PSE_Items_Fld_IsActive +
                    ", CASE WHEN RecordStatus = 'M' THEN 'Manual' WHEN RecordStatus = 'E' THEN 'Electronic' ELSE '' END AS RecordStatus " +
                    " FROM " + clsPOSDBConstants.PSE_Items_tbl, sWhereClause);

                PSEItemData ds = new PSEItemData();
                ds.PSEItem.MergeTable(DataHelper.ExecuteDataset(conn, CommandType.Text, sSQL, whereParameters(sWhereClause)).Tables[0]);
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
                //ErrorHandler.throwException(ex, "", "");
                return null;
            }
        }

        public PSEItemData PopulateList(string whereClause)
        {
            using (IDbConnection conn = DataFactory.CreateConnection())
            {
                conn.ConnectionString = POS_Core.Resources.Configuration.ConnectionString;
                return (PopulateList(whereClause, conn));
            }
        }
        #endregion //Get Method

        #region Insert, Update, and Delete Methods
        public void Insert(DataSet ds, SqlTransaction tx)
        {
            PSEItemTable addedTable = (PSEItemTable)ds.Tables[clsPOSDBConstants.PSE_Items_tbl].GetChanges(DataRowState.Added);
            string sSQL;
            IDbDataParameter[] insParam;

            if (addedTable != null && addedTable.Rows.Count > 0)
            {
                foreach (PSEItemRow row in addedTable.Rows)
                {
                    try
                    {
                        insParam = InsertParameters(row);
                        sSQL = BuildInsertSQL(clsPOSDBConstants.PSE_Items_tbl, insParam);
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

        // Update all rows in a TaxCodes DataSet, within a given database transaction.
        public void Update(DataSet ds, SqlTransaction tx)
        {
            PSEItemTable modifiedTable = (PSEItemTable)ds.Tables[clsPOSDBConstants.PSE_Items_tbl].GetChanges(DataRowState.Modified);

            string sSQL;
            IDbDataParameter[] updParam;

            if (modifiedTable != null && modifiedTable.Rows.Count > 0)
            {
                foreach (PSEItemRow row in modifiedTable.Rows)
                {
                    try
                    {
                        updParam = UpdateParameters(row);
                        sSQL = BuildUpdateSQL(clsPOSDBConstants.PSE_Items_tbl, updParam);

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
            // build where clause
            sInsertSQL = sInsertSQL + delParam[0].SourceColumn;

            for (int i = 1; i < delParam.Length; i++)
            {
                sInsertSQL = sInsertSQL + "," + delParam[i].SourceColumn;
            }
            sInsertSQL = sInsertSQL + "," + clsPOSDBConstants.PSE_Items_Fld_CreatedBy + "," + clsPOSDBConstants.PSE_Items_Fld_CreatedOn + "," + clsPOSDBConstants.PSE_Items_Fld_UpdatedBy + "," + clsPOSDBConstants.PSE_Items_Fld_UpdatedOn;
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
            // build where clause
            sUpdateSQL = sUpdateSQL + updParam[1].SourceColumn + "  = " + updParam[1].ParameterName;

            for (int i = 2; i < updParam.Length; i++)
            {
                sUpdateSQL = sUpdateSQL + " , " + updParam[i].SourceColumn + "  = " + updParam[i].ParameterName;
            }
            sUpdateSQL = sUpdateSQL + "," + clsPOSDBConstants.PSE_Items_Fld_UpdatedBy + "='" + Configuration.UserName + "'," + clsPOSDBConstants.PSE_Items_Fld_UpdatedOn + "= GETDATE()";
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

        private IDbDataParameter[] PKParameters(System.String ProductId)
        {
            IDbDataParameter[] sqlParams = DataFactory.CreateParameterArray(1);

            sqlParams[0] = DataFactory.CreateParameter();
            sqlParams[0].ParameterName = "@ProductId";
            sqlParams[0].DbType = System.Data.DbType.String;
            sqlParams[0].Value = ProductId;

            return (sqlParams);
        }

        private IDbDataParameter[] PKParameters(PSEItemRow row)
        {
            //return a SqlParameterCollection
            IDbDataParameter[] sqlParams = DataFactory.CreateParameterArray(1);


            sqlParams[0] = DataFactory.CreateParameter();
            sqlParams[0].ParameterName = "@ProductId";
            sqlParams[0].DbType = System.Data.DbType.String;

            sqlParams[0].Value = row.ProductId;
            sqlParams[0].SourceColumn = clsPOSDBConstants.PSE_Items_Fld_ProductId;

            return (sqlParams);
        }

        private IDbDataParameter[] InsertParameters(PSEItemRow row)
        {
            IDbDataParameter[] sqlParams = DataFactory.CreateParameterArray(7);

            sqlParams[0] = DataFactory.CreateParameter("@" + clsPOSDBConstants.PSE_Items_Fld_ProductId, System.Data.DbType.String);
            sqlParams[1] = DataFactory.CreateParameter("@" + clsPOSDBConstants.PSE_Items_Fld_ProductName, System.Data.DbType.String);
            sqlParams[2] = DataFactory.CreateParameter("@" + clsPOSDBConstants.PSE_Items_Fld_ProductNDC, System.Data.DbType.String);
            sqlParams[3] = DataFactory.CreateParameter("@" + clsPOSDBConstants.PSE_Items_Fld_ProductGrams, System.Data.DbType.Currency);
            sqlParams[4] = DataFactory.CreateParameter("@" + clsPOSDBConstants.PSE_Items_Fld_ProductPillCnt, System.Data.DbType.Int32);
            sqlParams[5] = DataFactory.CreateParameter("@" + clsPOSDBConstants.PSE_Items_Fld_IsActive, System.Data.DbType.Boolean);
            sqlParams[6] = DataFactory.CreateParameter("@" + clsPOSDBConstants.PSE_Items_Fld_RecordStatus, System.Data.DbType.String);

            sqlParams[0].SourceColumn = clsPOSDBConstants.PSE_Items_Fld_ProductId;
            sqlParams[1].SourceColumn = clsPOSDBConstants.PSE_Items_Fld_ProductName;
            sqlParams[2].SourceColumn = clsPOSDBConstants.PSE_Items_Fld_ProductNDC;
            sqlParams[3].SourceColumn = clsPOSDBConstants.PSE_Items_Fld_ProductGrams;
            sqlParams[4].SourceColumn = clsPOSDBConstants.PSE_Items_Fld_ProductPillCnt;
            sqlParams[5].SourceColumn = clsPOSDBConstants.PSE_Items_Fld_IsActive;
            sqlParams[6].SourceColumn = clsPOSDBConstants.PSE_Items_Fld_RecordStatus;

            if (row.ProductId != System.String.Empty)
                sqlParams[0].Value = row.ProductId;
            else
                sqlParams[0].Value = DBNull.Value;

            if (row.ProductName != System.String.Empty)
                sqlParams[1].Value = row.ProductName;
            else
                sqlParams[1].Value = DBNull.Value;

            if (row.ProductNDC != System.String.Empty)
                sqlParams[2].Value = row.ProductNDC;
            else
                sqlParams[2].Value = DBNull.Value;

            if (row.ProductGrams != System.Int32.MinValue)
                sqlParams[3].Value = row.ProductGrams;
            else
                sqlParams[3].Value = DBNull.Value;

            if (row.ProductPillCnt != System.Int32.MinValue)
                sqlParams[4].Value = row.ProductPillCnt;
            else
                sqlParams[4].Value = DBNull.Value;
            
            sqlParams[5].Value = row.IsActive;

            row.RecordStatus = "M";
            sqlParams[6].Value = row.RecordStatus;

            return (sqlParams);
        }

        private IDbDataParameter[] UpdateParameters(PSEItemRow row)
        {
            IDbDataParameter[] sqlParams = DataFactory.CreateParameterArray(7);

            sqlParams[0] = DataFactory.CreateParameter("@" + clsPOSDBConstants.PSE_Items_Fld_Id, System.Data.DbType.Int32);
            sqlParams[1] = DataFactory.CreateParameter("@" + clsPOSDBConstants.PSE_Items_Fld_ProductId, System.Data.DbType.String);
            sqlParams[2] = DataFactory.CreateParameter("@" + clsPOSDBConstants.PSE_Items_Fld_ProductName, System.Data.DbType.String);
            sqlParams[3] = DataFactory.CreateParameter("@" + clsPOSDBConstants.PSE_Items_Fld_ProductNDC, System.Data.DbType.String);
            sqlParams[4] = DataFactory.CreateParameter("@" + clsPOSDBConstants.PSE_Items_Fld_ProductGrams, System.Data.DbType.Currency);
            sqlParams[5] = DataFactory.CreateParameter("@" + clsPOSDBConstants.PSE_Items_Fld_ProductPillCnt, System.Data.DbType.Int32);
            sqlParams[6] = DataFactory.CreateParameter("@" + clsPOSDBConstants.PSE_Items_Fld_IsActive, System.Data.DbType.Boolean);

            sqlParams[0].SourceColumn = clsPOSDBConstants.PSE_Items_Fld_Id;
            sqlParams[1].SourceColumn = clsPOSDBConstants.PSE_Items_Fld_ProductId;
            sqlParams[2].SourceColumn = clsPOSDBConstants.PSE_Items_Fld_ProductName;
            sqlParams[3].SourceColumn = clsPOSDBConstants.PSE_Items_Fld_ProductNDC;
            sqlParams[4].SourceColumn = clsPOSDBConstants.PSE_Items_Fld_ProductGrams;
            sqlParams[5].SourceColumn = clsPOSDBConstants.PSE_Items_Fld_ProductPillCnt;
            sqlParams[6].SourceColumn = clsPOSDBConstants.PSE_Items_Fld_IsActive;

            sqlParams[0].Value = row.Id;
            sqlParams[1].Value = row.ProductId;

            if (row.ProductName != System.String.Empty)
                sqlParams[2].Value = row.ProductName;
            else
                sqlParams[2].Value = DBNull.Value;

            if (row.ProductNDC != System.String.Empty)
                sqlParams[3].Value = row.ProductNDC;
            else
                sqlParams[3].Value = DBNull.Value;

            if (row.ProductGrams != System.Int32.MinValue)
                sqlParams[4].Value = row.ProductGrams;
            else
                sqlParams[4].Value = DBNull.Value;

            if (row.ProductPillCnt != System.Int32.MinValue)
                sqlParams[5].Value = row.ProductPillCnt;
            else
                sqlParams[5].Value = DBNull.Value;

            sqlParams[6].Value = row.IsActive;

            return (sqlParams);
        }
        #endregion

        //Added logic to update PSE item from F9 screen (Item monitor category detail screen)
        public void UpdatePSEItem(System.String ItemID, System.Int32 ItemMonCatID, IDbTransaction tx)
        {
            string strSQL = string.Empty;
            if (IsPSEItemExists(ItemID, tx))
            {
                strSQL = "UPDATE a SET a.ProductName = b.Description, a.ProductGrams = CASE WHEN d.UOM = 1 THEN c.UnitsPerPackage/1000 ELSE c.UnitsPerPackage END, " +
                        " a.UpdatedBy = '" + Configuration.UserName + "', a.UpdatedOn = GETDATE(), a.IsActive = 1 " +
                        " FROM PSE_Items a " +
                        " INNER JOIN Item b ON SUBSTRING(a.ProductId, 1, 11) = SUBSTRING(b.ItemID, 1, 11) " +
                        " INNER JOIN ItemMonitorCategoryDetail c ON SUBSTRING(c.ItemID, 1, 11) = SUBSTRING(b.ItemID, 1, 11) " +
                        " INNER JOIN ItemMonitorCategory d ON c.ItemMonCatID = d.ID " +
                        " WHERE SUBSTRING(a.ProductId, 1, 11) = SUBSTRING('" + ItemID + "', 1, 11) AND c.ItemMonCatID = " + ItemMonCatID;
            }
            else
            {
                strSQL = "INSERT INTO PSE_Items (ProductId, ProductName, ProductGrams, CreatedBy, CreatedOn, UpdatedBy, UpdatedOn, IsActive, RecordStatus)" +
                        " SELECT a.ItemId, a.Description, CASE WHEN c.UOM = 1 THEN b.UnitsPerPackage / 1000 ELSE b.UnitsPerPackage END AS ProductGrams, '" +
                        Configuration.UserName + "', GETDATE(), '" + Configuration.UserName + "', GETDATE(), 1, 'M' FROM Item a " +
                        " INNER JOIN ItemMonitorCategoryDetail b ON SUBSTRING(a.ItemID, 1, 11) = SUBSTRING(b.ItemID, 1, 11) " +
                        " INNER JOIN ItemMonitorCategory c ON b.ItemMonCatID = c.ID " +
                        " WHERE SUBSTRING(a.ItemId, 1, 11) = SUBSTRING('" + ItemID + "', 1, 11) AND b.ItemMonCatID = " + ItemMonCatID;
            }
            DataHelper.ExecuteNonQuery(tx, CommandType.Text, strSQL);
        }              

        public System.Boolean IsPSEItemExists(System.String ItemID, IDbTransaction tx)
        {
            string strQuery = " Select ID From PSE_Items where SUBSTRING(ProductID,1,11) = SUBSTRING('" + ItemID.Replace("'", "''") + "',1,11)";
            object objValue = DataHelper.ExecuteScalar(tx,CommandType.Text, strQuery);
            if (objValue == null)
                return false;
            else
                return true;
        }

        public System.Boolean IsPSEItemExists(System.String ItemID)
        {
            string strQuery = " Select ID From PSE_Items where SUBSTRING(ProductID,1,11) = SUBSTRING('" + ItemID.Replace("'", "''") + "',1,11)";
            object objValue = DataHelper.ExecuteScalar(strQuery);
            if (objValue == null)
                return false;
            else
                return true;
        }

        public void Dispose() { }
    }
}
