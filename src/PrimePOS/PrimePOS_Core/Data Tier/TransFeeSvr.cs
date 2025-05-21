using NLog;
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
using System.Windows.Forms;

namespace POS_Core.DataAccess
{
    public class TransFeeSvr : IDisposable
    {
        private ILogger logger = LogManager.GetCurrentClassLogger();

        public void Persist(DataSet updates)
        {
            SqlTransaction tx;
            SqlConnection conn = new SqlConnection(DBConfig.ConnectionString);

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
                logger.Fatal(ex, "Persist(DataSet updates)");
            }
        }

        public void Persist(DataSet updates, SqlTransaction tx)
        {
            try

            {
                this.Delete(updates, tx);
                this.Update(updates, tx);
                this.Insert(updates, tx);

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
                logger.Fatal(ex, "Persist(DataSet updates, SqlTransaction tx)");
            }
        }


        #region Insert, Update, and Delete Methods
        public void Insert(DataSet ds, SqlTransaction tx)
        {
            TransFeeTable addedTable = (TransFeeTable)ds.Tables[clsPOSDBConstants.TransFee_tbl].GetChanges(DataRowState.Added);
            string sSQL;
            IDbDataParameter[] insParam;

            if (addedTable != null && addedTable.Rows.Count > 0)
            {
                foreach (TransFeeRow row in addedTable.Rows)
                {
                    try
                    {
                        insParam = InsertParameters(row);
                        sSQL = BuildInsertSQL(clsPOSDBConstants.TransFee_tbl, insParam);
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
                        logger.Fatal(ex, "Insert(DataSet ds, SqlTransaction tx)");
                    }
                }
                addedTable.AcceptChanges();
            }
        }

        public void Update(DataSet ds, SqlTransaction tx)
        {
            TransFeeTable modifiedTable = (TransFeeTable)ds.Tables[clsPOSDBConstants.TransFee_tbl].GetChanges(DataRowState.Modified);

            string sSQL;
            IDbDataParameter[] updParam;

            if (modifiedTable != null && modifiedTable.Rows.Count > 0)
            {
                foreach (TransFeeRow row in modifiedTable.Rows)
                {
                    try
                    {
                        updParam = UpdateParameters(row);
                        sSQL = BuildUpdateSQL(clsPOSDBConstants.TransFee_tbl, updParam);
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
                        logger.Fatal(ex, "Update(DataSet ds, SqlTransaction tx)");
                    }
                }
                modifiedTable.AcceptChanges();
            }
        }

        public void Delete(DataSet ds, SqlTransaction tx)
        {
            TransFeeTable table = (TransFeeTable)ds.Tables[clsPOSDBConstants.TransFee_tbl].GetChanges(DataRowState.Deleted);
            string sSQL;
            IDbDataParameter[] delParam;

            if (table != null && table.Rows.Count > 0)
            {
                table.RejectChanges();
                foreach (TransFeeRow row in table.Rows)
                {
                    try
                    {
                        delParam = PKParameters(row);
                        sSQL = BuildDeleteSQL(clsPOSDBConstants.TransFee_tbl, delParam);
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
                        logger.Fatal(ex, "Delete(DataSet ds, SqlTransaction tx) ");
                    }
                }
            }
        }

        private string BuildDeleteSQL(string tableName, IDbDataParameter[] delParam)
        {
            string sDeleteSQL = "DELETE FROM " + tableName + " WHERE ";
            for (int i = 0; i < delParam.Length; i++)
            {
                sDeleteSQL = sDeleteSQL + delParam[i].SourceColumn + " = " + delParam[i].ParameterName;
            }
            return sDeleteSQL;
        }

        private string BuildInsertSQL(string tableName, IDbDataParameter[] delParam)
        {
            string sInsertSQL = "INSERT INTO " + tableName + " ( ";
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

        private string BuildUpdateSQL(string tableName, IDbDataParameter[] updParam)
        {
            string sUpdateSQL = "UPDATE " + tableName + " SET ";
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

        private IDbDataParameter[] PKParameters(System.Int32 TransFeeID)
        {
            IDbDataParameter[] sqlParams = DataFactory.CreateParameterArray(1);

            sqlParams[0] = DataFactory.CreateParameter();
            sqlParams[0].ParameterName = "@TransFeeID";
            sqlParams[0].DbType = System.Data.DbType.Int32;
            sqlParams[0].Value = TransFeeID;

            return (sqlParams);
        }

        private IDbDataParameter[] PKParameters(TransFeeRow row)
        {
            //return a SqlParameterCollection
            IDbDataParameter[] sqlParams = DataFactory.CreateParameterArray(1);

            sqlParams[0] = DataFactory.CreateParameter();
            sqlParams[0].ParameterName = "@TransFeeID";
            sqlParams[0].DbType = System.Data.DbType.Int32;
            sqlParams[0].Value = row.TransFeeID;
            sqlParams[0].SourceColumn = clsPOSDBConstants.TransFee_Fld_TransFeeID;
            return (sqlParams);
        }

        private IDbDataParameter[] InsertParameters(TransFeeRow row)
        {
            IDbDataParameter[] sqlParams = DataFactory.CreateParameterArray(6);

            sqlParams[0] = DataFactory.CreateParameter("@" + clsPOSDBConstants.TransFee_Fld_TransFeeDesc, System.Data.DbType.String);
            sqlParams[1] = DataFactory.CreateParameter("@" + clsPOSDBConstants.TransFee_Fld_ChargeTransFeeFor, System.Data.DbType.Int16);
            sqlParams[2] = DataFactory.CreateParameter("@" + clsPOSDBConstants.TransFee_Fld_TransFeeMode, System.Data.DbType.Boolean);
            sqlParams[3] = DataFactory.CreateParameter("@" + clsPOSDBConstants.TransFee_Fld_TransFeeValue, System.Data.DbType.Decimal);
            sqlParams[4] = DataFactory.CreateParameter("@" + clsPOSDBConstants.TransFee_Fld_PayTypeID, System.Data.DbType.String);
            sqlParams[5] = DataFactory.CreateParameter("@" + clsPOSDBConstants.TransFee_Fld_IsActive, System.Data.DbType.Boolean);

            sqlParams[0].SourceColumn = clsPOSDBConstants.TransFee_Fld_TransFeeDesc;
            sqlParams[1].SourceColumn = clsPOSDBConstants.TransFee_Fld_ChargeTransFeeFor;
            sqlParams[2].SourceColumn = clsPOSDBConstants.TransFee_Fld_TransFeeMode;
            sqlParams[3].SourceColumn = clsPOSDBConstants.TransFee_Fld_TransFeeValue;
            sqlParams[4].SourceColumn = clsPOSDBConstants.TransFee_Fld_PayTypeID;
            sqlParams[5].SourceColumn = clsPOSDBConstants.TransFee_Fld_IsActive;

            sqlParams[0].Value = row.TransFeeDesc.Replace("'","''");
            sqlParams[1].Value = row.ChargeTransFeeFor;
            sqlParams[2].Value = row.TransFeeMode;
            sqlParams[3].Value = row.TransFeeValue;
            sqlParams[4].Value = row.PayTypeID;
            sqlParams[5].Value = row.IsActive;
            return (sqlParams);
        }

        private IDbDataParameter[] UpdateParameters(TransFeeRow row)
        {
            IDbDataParameter[] sqlParams = DataFactory.CreateParameterArray(7);

            sqlParams[0] = DataFactory.CreateParameter("@" + clsPOSDBConstants.TransFee_Fld_TransFeeID, System.Data.DbType.Int32);
            sqlParams[1] = DataFactory.CreateParameter("@" + clsPOSDBConstants.TransFee_Fld_TransFeeDesc, System.Data.DbType.String);
            sqlParams[2] = DataFactory.CreateParameter("@" + clsPOSDBConstants.TransFee_Fld_ChargeTransFeeFor, System.Data.DbType.Int16);
            sqlParams[3] = DataFactory.CreateParameter("@" + clsPOSDBConstants.TransFee_Fld_TransFeeMode, System.Data.DbType.Boolean);
            sqlParams[4] = DataFactory.CreateParameter("@" + clsPOSDBConstants.TransFee_Fld_TransFeeValue, System.Data.DbType.Decimal);
            sqlParams[5] = DataFactory.CreateParameter("@" + clsPOSDBConstants.TransFee_Fld_PayTypeID, System.Data.DbType.String);
            sqlParams[6] = DataFactory.CreateParameter("@" + clsPOSDBConstants.TransFee_Fld_IsActive, System.Data.DbType.Boolean);

            sqlParams[0].SourceColumn = clsPOSDBConstants.TransFee_Fld_TransFeeID;
            sqlParams[1].SourceColumn = clsPOSDBConstants.TransFee_Fld_TransFeeDesc;
            sqlParams[2].SourceColumn = clsPOSDBConstants.TransFee_Fld_ChargeTransFeeFor;
            sqlParams[3].SourceColumn = clsPOSDBConstants.TransFee_Fld_TransFeeMode;
            sqlParams[4].SourceColumn = clsPOSDBConstants.TransFee_Fld_TransFeeValue;
            sqlParams[5].SourceColumn = clsPOSDBConstants.TransFee_Fld_PayTypeID;
            sqlParams[6].SourceColumn = clsPOSDBConstants.TransFee_Fld_IsActive;

            sqlParams[0].Value = row.TransFeeID;
            sqlParams[1].Value = row.TransFeeDesc.Replace("'", "''");
            sqlParams[2].Value = row.ChargeTransFeeFor;
            sqlParams[3].Value = row.TransFeeMode;
            sqlParams[4].Value = row.TransFeeValue;
            sqlParams[5].Value = row.PayTypeID;
            sqlParams[6].Value = row.IsActive;
            return (sqlParams);
        }
        #endregion

        public bool Delete(System.Int32 TransFeeID)
        {
            string sSQL;
            bool bDelete = false;
            try
            {
                logger.Trace("Delete(System.Int32 TransFeeID) - " + clsPOSDBConstants.Log_Entering);
                if (MessageBox.Show("Do you want to make it InActive?", "PrimePOS", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                {
                    sSQL = "UPDATE TransactionFee SET IsActive = 0 WHERE TransFeeID = " + TransFeeID;
                    DataHelper.ExecuteNonQuery(POS_Core.Resources.Configuration.ConnectionString, CommandType.Text, sSQL);
                    bDelete = true;
                }
                logger.Trace("Delete(System.Int32 TransFeeID) - " + clsPOSDBConstants.Log_Exiting);                return bDelete;
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "Delete(System.Int32 TransFeeID)");
                ErrorHandler.throwException(ex, "", "");
            }
            return bDelete;
        }

        #region Get Methods
        public TransFeeData GetTransFeeDataByPayTypeID(System.String PayTypeID, TransType.POSTransactionType oTransactionType)
        {
            TransFeeData ds = new TransFeeData();
            try
            {
                using (SqlConnection conn = new SqlConnection(POS_Core.Resources.Configuration.ConnectionString))
                {
                    //if -99 means credit card, so check specific entry
                    //for 3,4,5,6,7,8 consider respective card                    
                    String sql = "SELECT * FROM " + clsPOSDBConstants.TransFee_tbl + " WHERE LTRIM(RTRIM(UPPER(" +
                    clsPOSDBConstants.TransFee_Fld_PayTypeID + "))) = '" + PayTypeID.Trim().ToUpper() +
                    "' AND ChargeTransFeeFor IN (0," + (int)oTransactionType + ") AND IsActive = 1";
                    ds.TransFee.MergeTable(DataHelper.ExecuteDataset(conn, CommandType.Text, sql).Tables[0]);
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
                return null;
            }
            return ds;
        }

        public TransFeeData GetTransFeeDataByTransFeeID(System.Int32 TransFeeID)
        {
            TransFeeData ds = new TransFeeData();
            try
            {
                using (SqlConnection conn = new SqlConnection(POS_Core.Resources.Configuration.ConnectionString))
                {
                    String sql = "SELECT * FROM " + clsPOSDBConstants.TransFee_tbl + " WHERE TransFeeID = " + TransFeeID;
                    ds.TransFee.MergeTable(DataHelper.ExecuteDataset(conn, CommandType.Text, sql).Tables[0]);
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
                return null;
            }
            return ds;
        }

        public DataTable GetPayTypeData(bool ShowOnlyOneCCType = true)
        {
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection conn = new SqlConnection(POS_Core.Resources.Configuration.ConnectionString))
                {
                    string strSQL = string.Empty;
                    if (ShowOnlyOneCCType)
                    {
                        strSQL = "SELECT PayTypeID, PayTypeDesc FROM " +
                                " (SELECT LTRIM(RTRIM(PayTypeID)) AS PayTypeID, PayTypeDesc, PayType, IsHide, 1 AS ShowOnlyOneCCType FROM PayType " +
                                " UNION " +
                                " SELECT TOP 1 '-99' AS PayTypeID, 'Credit Card' AS PayTypeDesc, 'CC' AS PayType, 0 AS IsHide, ShowOnlyOneCCType FROM Util_Company_Info) X " +
                                " WHERE PayType = 'CC' AND ShowOnlyOneCCType = 1 AND IsHide = 0";
                    }
                    else
                    {
                        strSQL = "SELECT LTRIM(RTRIM(PayTypeID)) AS PayTypeID, PayTypeDesc FROM PayType " +
                                " WHERE PayType = 'CC' AND IsHide = 0";
                    }
                    dt = DataHelper.ExecuteDataTable(conn, CommandType.Text, strSQL, (IDbDataParameter[])null);
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
                return null;
            }
            return dt;
        }               
        #endregion

        public void Dispose() { }
    }
}
