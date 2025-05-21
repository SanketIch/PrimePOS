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

    
    public class TransDetailTaxSvr : IDisposable
    {
        private static ILogger logger = LogManager.GetCurrentClassLogger();

        #region Persist Methods
        // Inserts, updates or deletes rows in a DataSet, within a database transaction.
        public void Persist(TransDetailTaxData updates, IDbTransaction tx, System.Int32 TransID)
        {
            try
            {
                this.Insert(updates, tx, TransID);
            }
            catch (POSExceptions ex)
            {
                logger.Fatal(ex, "Persist(TransDetailTaxData updates, IDbTransaction tx, System.Int32 TransID)");
                throw (ex);
            }
            catch (OtherExceptions ex)
            {
                logger.Fatal(ex, "Persist(TransDetailTaxData updates, IDbTransaction tx, System.Int32 TransID)");
                throw (ex);
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "Persist(TransDetailTaxData updates, IDbTransaction tx, System.Int32 TransID)");
                POS_Core.ErrorLogging.ErrorHandler.throwException(ex, "", "");
            }
        }

        public void PutOnHold(TransDetailTaxData updates, IDbTransaction tx, System.Int32 TransID)
        {
            try
            {
                this.DeleteOnHold(TransID, tx);
                this.InsertOnHold(updates, tx, TransID);
            }
            catch (POSExceptions ex)
            {
                logger.Fatal(ex, "PutOnHold(TransDetailTaxData updates, IDbTransaction tx, System.Int32 TransID)");
                throw (ex);
            }
            catch (OtherExceptions ex)
            {
                logger.Fatal(ex, "PutOnHold(TransDetailTaxData updates, IDbTransaction tx, System.Int32 TransID)");
                throw (ex);
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "PutOnHold(TransDetailTaxData updates, IDbTransaction tx, System.Int32 TransID)");
                POS_Core.ErrorLogging.ErrorHandler.throwException(ex, "", "");
            }
        }
        #endregion

        #region Insert, Update, and Delete Methods
        public static string strFormatedRXItemID = "";//Added by Krishna to access this information from ItemSvr to verify Item ID for Price change in AddItemPriceHistory(..) method.
        public void Insert(TransDetailTaxData ds, IDbTransaction tx, System.Int32 TransID)
        {
            TransDetailTaxTable addedTable = ds.TransDetailTax;
            //.GetChanges(DataRowState.Added);
            string sSQL;
            IDbDataParameter[] insParam;

            if (addedTable != null && addedTable.Rows.Count > 0)
            {
                foreach (TransDetailTaxRow row in addedTable.Rows)
                {
                    try
                    {
                        //if (row.TaxAmount != 0) //Sprint-21 23-Sep-2015 JY Added to restrict the records having 0 taxamount - (also when we insert non-taxable item using function key then such records gets added, also fixed this where such records initiated but for safer side added this code)
                        if (row.TransDetailID != 0) //Sprint-23 - PRIMEPOS-N TaxType 27-Jun-2016 JY Added 
                        {
                            row.TransID = TransID;
                            insParam = InsertParameters(row);
                            sSQL = BuildInsertSQL(clsPOSDBConstants.TransDetailTax_tbl, insParam);

                            DataHelper.ExecuteNonQuery(tx, CommandType.Text, sSQL, insParam);
                        }
                    }
                    catch (POSExceptions ex)
                    {
                        logger.Fatal(ex, "Insert(TransDetailTaxData ds, IDbTransaction tx, System.Int32 TransID)");
                        throw (ex);
                    }
                    catch (OtherExceptions ex)
                    {
                        logger.Fatal(ex, "Insert(TransDetailTaxData ds, IDbTransaction tx, System.Int32 TransID)");
                        throw (ex);
                    }
                    catch (Exception ex)
                    {
                        logger.Fatal(ex, "Insert(TransDetailTaxData ds, IDbTransaction tx, System.Int32 TransID)");
                        ErrorHandler.throwException(ex, "", "");
                    }
                }
                addedTable.AcceptChanges();
            }
        }
        
        public void InsertOnHold(TransDetailTaxData ds, IDbTransaction tx, System.Int32 TransID)
        {

            TransDetailTaxTable addedTable = (TransDetailTaxTable)ds.TransDetailTax;
            //.GetChanges(DataRowState.Added);
            string sSQL;
            IDbDataParameter[] insParam;

            if (addedTable != null && addedTable.Rows.Count > 0)
            {
                foreach (TransDetailTaxRow row in addedTable.Rows)
                {
                    if (row.RowState == DataRowState.Deleted) continue;
                    try
                    {
                        if (row.TransDetailID != 0) //Sprint-26 - PRIMEPOS-XXXX 01-Sep-2017 JY Added condition
                        {
                            row.TransID = TransID;
                            insParam = InsertParameters(row);
                            sSQL = BuildInsertSQL(clsPOSDBConstants.TransDetailTax_OnHold_tbl, insParam);

                            DataHelper.ExecuteNonQuery(tx, CommandType.Text, sSQL, insParam);
                        }
                    }
                    catch (POSExceptions ex)
                    {
                        logger.Fatal(ex, "InsertOnHold(TransDetailTaxData ds, IDbTransaction tx, System.Int32 TransID)");
                        throw (ex);
                    }
                    catch (OtherExceptions ex)
                    {
                        logger.Fatal(ex, "InsertOnHold(TransDetailTaxData ds, IDbTransaction tx, System.Int32 TransID)");
                        throw (ex);
                    }
                    catch (Exception ex)
                    {
                        logger.Fatal(ex, "InsertOnHold(TransDetailTaxData ds, IDbTransaction tx, System.Int32 TransID)");
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
            string sSQL;
            try
            {
                sSQL = BuildDeleteOnHoldSQL(clsPOSDBConstants.TransDetailTax_OnHold_tbl, TransID);
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

        private string BuildDeleteSQL(string tableName, TransDetailRow row)
        {
            string sDeleteSQL = "DELETE FROM " + tableName + " WHERE ";
            sDeleteSQL += clsPOSDBConstants.TransDetail_Fld_TransDetailID + " = " + row.TransDetailID.ToString();
            return sDeleteSQL;
        }

        private string BuildDeleteOnHoldSQL(string tableName, Int32 TransID)
        {
            string sDeleteSQL = "DELETE FROM " + tableName + " WHERE ";
            sDeleteSQL += clsPOSDBConstants.TransDetail_Fld_TransID + " = " + TransID.ToString();
            return sDeleteSQL;
        }
        
        public void UpdateOnHold(TransDetailTaxData ds, IDbTransaction tx, System.Int32 OrderID)
        {
            TransDetailTaxTable modifiedTable = (TransDetailTaxTable)ds.TransDetailTax.GetChanges(DataRowState.Modified);

            string sSQL;
            IDbDataParameter[] updParam;

            if (modifiedTable != null && modifiedTable.Rows.Count > 0)
            {
                foreach (TransDetailTaxRow row in modifiedTable.Rows)
                {
                    try
                    {
                        updParam = UpdateParameters(row);
                        sSQL = BuildUpdateSQL(clsPOSDBConstants.TransDetail_OnHold_tbl, updParam);

                        DataHelper.ExecuteNonQuery(tx, CommandType.Text, sSQL, updParam);
                    }
                    catch (POSExceptions ex)
                    {
                        logger.Fatal(ex, "UpdateOnHold(TransDetailTaxData ds, IDbTransaction tx, System.Int32 OrderID)");
                        throw (ex);
                    }
                    catch (OtherExceptions ex)
                    {
                        logger.Fatal(ex, "UpdateOnHold(TransDetailTaxData ds, IDbTransaction tx, System.Int32 OrderID)");
                        throw (ex);
                    }
                    catch (Exception ex)
                    {
                        logger.Fatal(ex, "UpdateOnHold(TransDetailTaxData ds, IDbTransaction tx, System.Int32 OrderID)");
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
        private IDbDataParameter[] PKParameters(System.Int32 TransDetailTaxID)
        {
            IDbDataParameter[] sqlParams = DataFactory.CreateParameterArray(1);

            sqlParams[0] = DataFactory.CreateParameter();
            sqlParams[0].ParameterName = "@TransDetailTaxID";
            sqlParams[0].DbType = System.Data.DbType.String;

            sqlParams[0].Value = TransDetailTaxID;

            return (sqlParams);
        }

        private IDbDataParameter[] PKParameters(TransDetailTaxRow row)
        {
            //return a SqlParameterCollection
            IDbDataParameter[] sqlParams = DataFactory.CreateParameterArray(1);

            sqlParams[0] = DataFactory.CreateParameter();
            sqlParams[0].ParameterName = "@TransDetailTaxID";
            sqlParams[0].DbType = System.Data.DbType.String;

            sqlParams[0].Value = row.TransDetailTaxID;
            sqlParams[0].SourceColumn = clsPOSDBConstants.TransDetailTax_fld_TransDetailTaxID;

            return (sqlParams);
        }

		private IDbDataParameter[] InsertParameters(TransDetailTaxRow row) 
		{
            IDbDataParameter[] sqlParams = DataFactory.CreateParameterArray(6);

            sqlParams[0] = DataFactory.CreateParameter("@" + clsPOSDBConstants.TransDetail_Fld_TransDetailID, System.Data.DbType.Int32);
            sqlParams[1] = DataFactory.CreateParameter("@" + clsPOSDBConstants.TransDetail_Fld_TransID, System.Data.DbType.Int32);
            sqlParams[2] = DataFactory.CreateParameter("@" + clsPOSDBConstants.TransDetailTax_fld_TaxPercent, System.Data.DbType.Currency);
            sqlParams[3] = DataFactory.CreateParameter("@" + clsPOSDBConstants.TransDetail_Fld_TaxAmount, System.Data.DbType.Currency);
            sqlParams[4] = DataFactory.CreateParameter("@" + clsPOSDBConstants.TransDetail_Fld_ItemID, System.Data.DbType.Int32);
            sqlParams[5] = DataFactory.CreateParameter("@" + clsPOSDBConstants.TransDetail_Fld_TaxID, System.Data.DbType.Int32);

            sqlParams[0].SourceColumn = clsPOSDBConstants.TransDetail_Fld_TransDetailID;
            sqlParams[1].SourceColumn = clsPOSDBConstants.TransDetail_Fld_TransID;
            sqlParams[2].SourceColumn = clsPOSDBConstants.TransDetailTax_fld_TaxPercent;
            sqlParams[3].SourceColumn = clsPOSDBConstants.TransDetail_Fld_TaxAmount;
            sqlParams[4].SourceColumn = clsPOSDBConstants.TransDetail_Fld_ItemID;
            sqlParams[5].SourceColumn = clsPOSDBConstants.TransDetail_Fld_TaxID;

            if (row.TransDetailID.ToString() != System.String.Empty)
                sqlParams[0].Value = row.TransDetailID;
            else
                sqlParams[0].Value = DBNull.Value;

            if (row.TransID.ToString() != System.String.Empty)
                sqlParams[1].Value = row.TransID;
            else
                sqlParams[1].Value = DBNull.Value;

            if (row.TaxPercent.ToString() != System.String.Empty)
                sqlParams[2].Value = row.TaxPercent;
            else
                sqlParams[2].Value = DBNull.Value;

            if (row.TaxAmount.ToString() != System.String.Empty)
                sqlParams[3].Value = row.TaxAmount;
            else
                sqlParams[3].Value = DBNull.Value;

            if (row.ItemID.ToString() != System.String.Empty)
                sqlParams[4].Value = row.ItemID;
            else
                sqlParams[4].Value = DBNull.Value;
            if (row.TaxID.ToString() != System.String.Empty)
            {
                sqlParams[5].Value = row.TaxID;
            }
            else
            {
                sqlParams[5].Value = 0;
            }
			return(sqlParams);
		}

		private IDbDataParameter[] UpdateParameters(TransDetailTaxRow row) 
		{
            //IDbDataParameter[] sqlParams = DataFactory.CreateParameterArray(12);//Orignal commented by Krishna on 15 July 2011
            IDbDataParameter[] sqlParams = DataFactory.CreateParameterArray(7);

            sqlParams[0] = DataFactory.CreateParameter("@" + clsPOSDBConstants.TransDetail_Fld_TransDetailID, System.Data.DbType.Int32);
            sqlParams[1] = DataFactory.CreateParameter("@" + clsPOSDBConstants.TransDetail_Fld_TransID, System.Data.DbType.Int32);
            sqlParams[2] = DataFactory.CreateParameter("@" + clsPOSDBConstants.TransDetailTax_fld_TaxPercent, System.Data.DbType.Currency);
            sqlParams[3] = DataFactory.CreateParameter("@" + clsPOSDBConstants.TransDetail_Fld_TaxAmount, System.Data.DbType.Currency);
            sqlParams[4] = DataFactory.CreateParameter("@" + clsPOSDBConstants.TransDetail_Fld_ItemID, System.Data.DbType.Int32);
            sqlParams[5] = DataFactory.CreateParameter("@" + clsPOSDBConstants.TransDetail_Fld_TaxID, System.Data.DbType.Int32);
            sqlParams[6] = DataFactory.CreateParameter("@" + clsPOSDBConstants.TransDetailTax_fld_TransDetailTaxID, System.Data.DbType.Int32);

            sqlParams[0].SourceColumn = clsPOSDBConstants.TransDetail_Fld_TransDetailID;
            sqlParams[1].SourceColumn = clsPOSDBConstants.TransDetail_Fld_TransID;
            sqlParams[2].SourceColumn = clsPOSDBConstants.TransDetailTax_fld_TaxPercent;
            sqlParams[3].SourceColumn = clsPOSDBConstants.TransDetail_Fld_TaxAmount;
            sqlParams[4].SourceColumn = clsPOSDBConstants.TransDetail_Fld_ItemID;
            sqlParams[5].SourceColumn = clsPOSDBConstants.TransDetail_Fld_TaxID;
            sqlParams[6].SourceColumn = clsPOSDBConstants.TransDetailTax_fld_TransDetailTaxID;

            if (row.TransDetailID.ToString() != System.String.Empty)
                sqlParams[0].Value = row.TransDetailID;
            else
                sqlParams[0].Value = DBNull.Value;

            if (row.TransID.ToString() != System.String.Empty)
                sqlParams[1].Value = row.TransID;
            else
                sqlParams[1].Value = DBNull.Value;

            if (row.TaxPercent.ToString() != System.String.Empty)
                sqlParams[2].Value = row.TaxPercent;
            else
                sqlParams[2].Value = DBNull.Value;

            if (row.TaxAmount.ToString() != System.String.Empty)
                sqlParams[3].Value = row.TaxAmount;
            else
                sqlParams[3].Value = DBNull.Value;

            if (row.ItemID.ToString() != System.String.Empty)
                sqlParams[4].Value = row.ItemID;
            else
                sqlParams[4].Value = DBNull.Value;
            if (row.TaxID.ToString() != System.String.Empty)
            {
                sqlParams[5].Value = row.TaxID;
            }
            else
            {
                sqlParams[5].Value = 0;
            }
            sqlParams[6].Value = row.TransDetailTaxID;

            return (sqlParams);
		}

        #endregion

        #region Get Methods

        // Looks up a ItemVendor based on its primary-key:System.String VendorItemID


        public static bool isCallofRetTrans = false;//Added by Krishna for checking if the call is made from return trans
        public DataSet Populate(System.Int32 TransId, IDbConnection conn)
        {
            string sSQL="";
            try
            {
               
                    sSQL = "Select *  "
                       + " FROM "
                       + clsPOSDBConstants.TransDetailTax_tbl + " td left join "
                       + clsPOSDBConstants.TaxCodes_tbl + " tx"
                       + " on ( "
                       + "tx." + clsPOSDBConstants.TaxCodes_Fld_TaxID + "=td." + clsPOSDBConstants.TransDetail_Fld_TaxID
                       + ") where " + clsPOSDBConstants.TransDetail_Fld_TransID + " = " + TransId;
                
				TransDetailTaxData ds = new TransDetailTaxData();
				ds.TransDetailTax.MergeTable(DataHelper.ExecuteDataset(conn, CommandType.Text, sSQL,PKParameters(TransId)).Tables[0]);
				return ds;
            }
			catch (Exception ex)
            {
                logger.Fatal(ex, "Populate(System.Int32 TransId, IDbConnection conn)");
                ErrorHandler.throwException(ex, "", "");
                return null;
            }
        }

        public DataSet PopulateOnHold(System.Int32 TransId, IDbConnection conn)//2915
        {
            string sSQL = "";
            try
            {

                sSQL = "Select *  "
                   + " FROM "
                   + clsPOSDBConstants.TransDetailTax_OnHold_tbl + " td left join "
                   + clsPOSDBConstants.TaxCodes_tbl + " tx"
                   + " on ( "
                   + "tx." + clsPOSDBConstants.TaxCodes_Fld_TaxID + "=td." + clsPOSDBConstants.TransDetail_Fld_TaxID
                   + ") where " + clsPOSDBConstants.TransDetail_Fld_TransID + " = " + TransId;

                TransDetailTaxData ds = new TransDetailTaxData();
                ds.TransDetailTax.MergeTable(DataHelper.ExecuteDataset(conn, CommandType.Text, sSQL, PKParameters(TransId)).Tables[0]);
                return ds;
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "Populate(System.Int32 TransId, IDbConnection conn)");
                ErrorHandler.throwException(ex, "", "");
                return null;
            }
        }

        public DataSet PopulateTransDetailId(System.Int32 TransDetailId, IDbConnection conn)
        {
            try
            {

                string sSQL = "Select *  "
                   + " FROM "
                   + clsPOSDBConstants.TransDetailTax_tbl + " td left join "
                   + clsPOSDBConstants.TaxCodes_tbl + " tx"
                   + " on ( "
                   + "tx." + clsPOSDBConstants.TaxCodes_Fld_TaxID + "=td." + clsPOSDBConstants.TransDetail_Fld_TaxID
                   + ") where " + clsPOSDBConstants.TransDetail_Fld_TransDetailID + " = " + TransDetailId;

                TransDetailTaxData ds = new TransDetailTaxData();
                ds.TransDetailTax.MergeTable(DataHelper.ExecuteDataset(conn, CommandType.Text, sSQL, PKParameters(TransDetailId)).Tables[0]);
                return ds;
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "PopulateTransDetailId(System.Int32 TransDetailId, IDbConnection conn)");
                ErrorHandler.throwException(ex, "", "");
                return null;
            }
        }

       
        public DataSet Populate(System.Int32 TransId)
        {
            IDbConnection conn = DataFactory.CreateConnection();
            conn.ConnectionString = POS_Core.Resources.Configuration.ConnectionString;
            return (Populate(TransId, conn));
        }
        public DataSet PopulateTransDetailId(System.Int32 TransDetailId)
        {
            IDbConnection conn = DataFactory.CreateConnection();
            conn.ConnectionString = POS_Core.Resources.Configuration.ConnectionString;
            return (PopulateTransDetailId(TransDetailId, conn));
        }

        public TransDetailTaxData GetOnHoldTransDetail(System.Int32 TransId)
        {
            IDbConnection conn = DataFactory.CreateConnection();
            conn.ConnectionString = POS_Core.Resources.Configuration.ConnectionString;
            return (GetOnHoldTransDetail(TransId, conn));
        }

        public TransDetailTaxData GetOnHoldTransDetail(System.Int32 TransId, IDbConnection conn)
        {
            try
            {
                string sSQL = "Select "
                    + " td.*"
                    + " FROM "
                    + clsPOSDBConstants.TransDetailTax_OnHold_tbl + " td left join "
                    + clsPOSDBConstants.TaxCodes_tbl + " tx"
                    + " on ( "
                    + "tx." + clsPOSDBConstants.TaxCodes_Fld_TaxID + "=td." + clsPOSDBConstants.TransDetail_Fld_TaxID
                    + ") where " + clsPOSDBConstants.TransDetail_Fld_TransID + " = " + TransId;

                TransDetailTaxData ds = new TransDetailTaxData();
                ds.TransDetailTax.MergeTable(DataHelper.ExecuteDataset(conn, CommandType.Text, sSQL).Tables[0]);
                ds.AcceptChanges();
                return ds;
            }
            catch (POSExceptions ex)
            {
                logger.Fatal(ex, "GetOnHoldTransDetail(System.Int32 TransId, IDbConnection conn)");
                throw (ex);
            }
            catch (OtherExceptions ex)
            {
                logger.Fatal(ex, "GetOnHoldTransDetail(System.Int32 TransId, IDbConnection conn)");
                throw (ex);
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "GetOnHoldTransDetail(System.Int32 TransId, IDbConnection conn)");
                ErrorHandler.throwException(ex, "", "");
                return null;
            }
        }

        public TransDetailTaxData PopulateData(System.Int32 TransId)
        {
            IDbConnection conn = DataFactory.CreateConnection();
            conn.ConnectionString = POS_Core.Resources.Configuration.ConnectionString;
            DataSet ds = Populate(TransId, conn);

            TransDetailTaxData oTD = new TransDetailTaxData();
            oTD.TransDetailTax.MergeTable(ds.Tables[0]);
            return oTD;
        }

        public TransDetailTaxData PopulateDataOnHold(System.Int32 TransId)//2915
        {
            IDbConnection conn = DataFactory.CreateConnection();
            conn.ConnectionString = POS_Core.Resources.Configuration.ConnectionString;
            DataSet ds = PopulateOnHold(TransId, conn);

            TransDetailTaxData oTD = new TransDetailTaxData();
            oTD.TransDetailTax.MergeTable(ds.Tables[0]);
            return oTD;
        }

        #region Sprint-26 - PRIMEPOS-2445 28-Aug-2017 JY Added
        public DataTable GetTransDetailTax(System.Int32 TransId)
        {
            IDbConnection conn = DataFactory.CreateConnection();
            conn.ConnectionString = POS_Core.Resources.Configuration.ConnectionString;
            return (GetTransDetailTax(TransId, conn));
        }

        public DataTable GetTransDetailTax(System.Int32 TransId, IDbConnection conn)
        {
            try
            {
                string sSQL = "SELECT a.TaxID, ISNULL(b.TaxType,0) AS TaxType, CASE WHEN TaxType = 1 THEN 'State Tax' WHEN TaxType = 2 THEN 'Municipality Tax' WHEN TaxType = 3 THEN 'Federal Tax' WHEN TaxType = 4 THEN 'City Tax' WHEN TaxType = 5 THEN 'Local Tax' WHEN TaxType = 6 THEN 'County Tax' ELSE '' END AS TaxTypeDesc," +
                            " SUM(a.TaxAmount) As TaxAmt FROM POSTransactionDetailTax a " +
                            " INNER JOIN TaxCodes b ON a.TaxID = b.TaxID " +
                            " WHERE ISNULL(b.TaxType,0) <> 0 AND a.TransID = " + TransId +
                            " GROUP BY a.TaxID, b.TaxType " +
                            " ORDER BY TaxTypeDesc";
                    
                DataSet ds = DataHelper.ExecuteDataset(conn, CommandType.Text, sSQL);
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    return ds.Tables[0];
                else
                    return null;
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "Populate(System.Int32 TransId, IDbConnection conn)");
                ErrorHandler.throwException(ex, "", "");
                return null;
            }
        }

        public DataTable GetTransDetailTaxOnHold(System.Int32 TransId)
        {
            IDbConnection conn = DataFactory.CreateConnection();
            conn.ConnectionString = POS_Core.Resources.Configuration.ConnectionString;
            return (GetTransDetailTaxOnHold(TransId, conn));
        }

        public DataTable GetTransDetailTaxOnHold(System.Int32 TransId, IDbConnection conn)
        {
            try
            {
                string sSQL = "SELECT a.TaxID, ISNULL(b.TaxType,0) AS TaxType, CASE WHEN TaxType = 1 THEN 'State Tax' WHEN TaxType = 2 THEN 'Municipality Tax' WHEN TaxType = 3 THEN 'Federal Tax' WHEN TaxType = 4 THEN 'City Tax' WHEN TaxType = 5 THEN 'Local Tax' WHEN TaxType = 6 THEN 'County Tax' ELSE '' END AS TaxTypeDesc," +
                            " SUM(a.TaxAmount) As TaxAmt FROM POSTransactionDetailTax_OnHold a " +
                            " INNER JOIN TaxCodes b ON a.TaxID = b.TaxID " +
                            " WHERE ISNULL(b.TaxType,0) <> 0 AND a.TransID = " + TransId +
                            " GROUP BY a.TaxID, b.TaxType " +
                            " ORDER BY TaxTypeDesc";

                DataSet ds = DataHelper.ExecuteDataset(conn, CommandType.Text, sSQL);
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    return ds.Tables[0];
                else
                    return null;
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "Populate(System.Int32 TransId, IDbConnection conn)");
                ErrorHandler.throwException(ex, "", "");
                return null;
            }
        }
        #endregion

        #region  Arvind 2664
        public DataTable GetTaxCodeDetail(Int32 Transid)
        {
            IDbConnection conn = DataFactory.CreateConnection();
            conn.ConnectionString = Resources.Configuration.ConnectionString;
            return (GetTaxCodeDetail(Transid, conn));
        }
        public DataTable GetTaxCodeDetail(Int32 Transid, IDbConnection conn)
        {
            try
            {
                //PRIMEPOS - 3099 change query
                //string sSQL = "SELECT a.TaxID, b.orders ,ISNULL(b.TaxType,0) AS TaxType, SUM(b.amount) as Amount,b.Description, CASE WHEN TaxType = 1 THEN 'State Tax' WHEN TaxType = 2 THEN 'Municipality Tax' WHEN TaxType = 3 THEN 'Federal Tax' WHEN TaxType = 4 THEN 'City Tax' WHEN TaxType = 5 THEN 'Local Tax' WHEN TaxType = 6 THEN 'County Tax' ELSE '' END AS TaxTypeDesc, SUM(ISNULL(a.TaxAmount, 0)) As TaxAmt FROM TaxCodes b left JOIN(Select * from POSTransactionDetailTax where TransID = '" + Transid + "') a ON a.TaxID = b.TaxID  where ISNULL(b.TaxType, 0) <> 0 and b.orders is not null  GROUP BY a.TaxID, b.TaxType,b.Description ,b.orders ORDER BY b.Orders ";

                string sSQL = "SELECT a.TaxID, b.orders ,ISNULL(b.TaxType,0) AS TaxType, SUM(b.amount) as Amount,b.Description, CASE WHEN TaxType = 1 THEN 'State Tax' WHEN TaxType = 2 THEN 'Municipality Tax' WHEN TaxType = 3 THEN 'Federal Tax' WHEN TaxType = 4 THEN 'City Tax' WHEN TaxType = 5 THEN 'Local Tax' WHEN TaxType = 6 THEN 'County Tax' ELSE '' END AS TaxTypeDesc, SUM(ISNULL(a.TaxAmount, 0)) As TaxAmt FROM TaxCodes b left JOIN(Select * from POSTransactionDetailTax where TransID = '" + Transid + "') a ON a.TaxID = b.TaxID  where (b.TaxCode in ('PRS','PRM','PRRS') OR (a.TaxID is not Null))  GROUP BY a.TaxID, b.TaxType,b.Description ,b.orders ORDER BY b.Orders ";

                DataSet ds = DataHelper.ExecuteDataset(conn, CommandType.Text, sSQL);
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    return ds.Tables[0];
                else
                    return null;
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "GetTaxCodeDetail(IDbConnection conn)");
                ErrorHandler.throwException(ex, "", "");
                return null;
            }
        }
        #endregion

        #endregion //Get Method

        public void Dispose() { }
    }    
}
