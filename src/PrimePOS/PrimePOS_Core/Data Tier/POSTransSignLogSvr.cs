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
   

    // Provides data access methods for DeptCode

    public class POSTransSignLogSvr : IDisposable
    {
        private static ILogger logger = LogManager.GetCurrentClassLogger();

        #region Persist Methods
        // Inserts, updates or deletes rows in a DataSet, within a database transaction.
        public void Persist(POSTransSignLogData updates, IDbTransaction tx, System.Int32 TransID)
        {
            try
            {
                this.Insert(updates, tx, TransID);
            }
            catch (POSExceptions ex)
            {
                logger.Fatal(ex, "Persist(POSTransSignLogData updates, IDbTransaction tx, System.Int32 TransID)");
                throw (ex);
            }

            catch (OtherExceptions ex)
            {
                logger.Fatal(ex, "Persist(POSTransSignLogData updates, IDbTransaction tx, System.Int32 TransID)");
                throw (ex);
            }

            catch (Exception ex)
            {
                logger.Fatal(ex, "Persist(POSTransSignLogData updates, IDbTransaction tx, System.Int32 TransID)");
                ErrorLogging.ErrorHandler.throwException(ex, "", "");
            }
        }
        #endregion

        #region Insert Methods
        public void Insert(POSTransSignLogData ds, IDbTransaction tx, System.Int32 TransID)
        {

            string sSQL;
            IDbDataParameter[] insParam;

            if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
            {
                foreach (POSTransSignLogRow row in ds.Tables[0].Rows)
                {
                    try
                    {
                        row.POSTransId = TransID;
                        insParam = InsertParameters(row);
                        sSQL = BuildInsertSQL(clsPOSDBConstants.POSTransSignLog_tbl, insParam);

                        DataHelper.ExecuteNonQuery(tx, CommandType.Text, sSQL, insParam);
                    }
                    catch (POSExceptions ex)
                    {
                        logger.Fatal(ex, "Insert(POSTransSignLogData ds, IDbTransaction tx, System.Int32 TransID)");
                        throw (ex);
                    }

                    catch (OtherExceptions ex)
                    {
                        logger.Fatal(ex, "Insert(POSTransSignLogData ds, IDbTransaction tx, System.Int32 TransID)");
                        throw (ex);
                    }

                    catch (Exception ex)
                    {
                        logger.Fatal(ex, "Insert(POSTransSignLogData ds, IDbTransaction tx, System.Int32 TransID)");
                        ErrorHandler.throwException(ex, "", "");
                    }
                }
                ds.Tables[0].AcceptChanges();
            }
        }

        private string BuildInsertSQL(string tableName, IDbDataParameter[] insertParam)
        {
            string sInsertSQL = "INSERT INTO " + tableName + " ( ";
            // build where clause
            sInsertSQL = sInsertSQL + insertParam[0].SourceColumn;

            for (int i = 1; i < insertParam.Length; i++)
            {
                sInsertSQL = sInsertSQL + " , " + insertParam[i].SourceColumn;
            }
            sInsertSQL = sInsertSQL + " ) Values (" + insertParam[0].ParameterName;

            for (int i = 1; i < insertParam.Length; i++)
            {
                if (insertParam[i].SourceColumn.ToString() == clsPOSDBConstants.POSTransSignLog_Fld_SignDataBinary)
                {
                    sInsertSQL = sInsertSQL + " , cast(" + insertParam[i].ParameterName + " as varbinary(MAX))";
                }
                else
                {
                    sInsertSQL = sInsertSQL + " , " + insertParam[i].ParameterName;
                }
            }
            sInsertSQL = sInsertSQL + " )";
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
        private IDbDataParameter[] PKParameters(System.String SignLogID)
        {
            IDbDataParameter[] sqlParams = DataFactory.CreateParameterArray(1);

            sqlParams[0] = DataFactory.CreateParameter();
            sqlParams[0].ParameterName = clsPOSDBConstants.POSTransSignLog_Fld_SignLogID;
            sqlParams[0].DbType = System.Data.DbType.Int32;
            sqlParams[0].Value = SignLogID;

            return (sqlParams);
        }


        private IDbDataParameter[] InsertParameters(POSTransSignLogRow row)
        {
            IDbDataParameter[] sqlParams = DataFactory.CreateParameterArray(8);

            sqlParams[0] = DataFactory.CreateParameter("@" + clsPOSDBConstants.POSTransSignLog_Fld_POSTransId, System.Data.DbType.Int32);
            sqlParams[1] = DataFactory.CreateParameter("@" + clsPOSDBConstants.POSTransSignLog_Fld_SignContext, System.Data.DbType.String);
            sqlParams[2] = DataFactory.CreateParameter("@" + clsPOSDBConstants.POSTransSignLog_Fld_SignContextData, System.Data.DbType.String);
            sqlParams[3] = DataFactory.CreateParameter("@" + clsPOSDBConstants.POSTransSignLog_Fld_SignDataBinary, System.Data.DbType.Binary);
            sqlParams[4] = DataFactory.CreateParameter("@" + clsPOSDBConstants.POSTransSignLog_Fld_SignDataText, System.Data.DbType.String);
            sqlParams[5] = DataFactory.CreateParameter("@" + clsPOSDBConstants.POSTransSignLog_Fld_CustomerIDType, System.Data.DbType.String);
            sqlParams[6] = DataFactory.CreateParameter("@" + clsPOSDBConstants.POSTransSignLog_Fld_CustomerIDDetail, System.Data.DbType.String);
            sqlParams[7] = DataFactory.CreateParameter("@" + clsPOSDBConstants.POSTransSignLog_Fld_TransDetailID, System.Data.DbType.Int32);

            sqlParams[0].SourceColumn = clsPOSDBConstants.POSTransSignLog_Fld_POSTransId;
            sqlParams[1].SourceColumn = clsPOSDBConstants.POSTransSignLog_Fld_SignContext;
            sqlParams[2].SourceColumn = clsPOSDBConstants.POSTransSignLog_Fld_SignContextData;
            sqlParams[3].SourceColumn = clsPOSDBConstants.POSTransSignLog_Fld_SignDataBinary;
            sqlParams[4].SourceColumn = clsPOSDBConstants.POSTransSignLog_Fld_SignDataText;
            sqlParams[5].SourceColumn = clsPOSDBConstants.POSTransSignLog_Fld_CustomerIDType;
            sqlParams[6].SourceColumn = clsPOSDBConstants.POSTransSignLog_Fld_CustomerIDDetail;
            sqlParams[7].SourceColumn = clsPOSDBConstants.POSTransSignLog_Fld_TransDetailID;

            if (row.POSTransId != 0)
                sqlParams[0].Value = row.POSTransId;
            else
                sqlParams[0].Value = DBNull.Value;

            if (row.SignContext != System.String.Empty)
                sqlParams[1].Value = row.SignContext;
            else
                sqlParams[1].Value = DBNull.Value;

            if (row.SignContextData != System.String.Empty)
            {
                sqlParams[2].Value = row.SignContextData;
            }
            else
                sqlParams[2].Value = DBNull.Value;

            if (row.SignDataBinary != null)
                sqlParams[3].Value = row.SignDataBinary;
            else
                sqlParams[3].Value = DBNull.Value;

            if (row.SignDataText != System.String.Empty)
                sqlParams[4].Value = row.SignDataText;
            else
                sqlParams[4].Value = DBNull.Value;

            if (row.CustomerIDType != System.String.Empty)
                sqlParams[5].Value = row.CustomerIDType;
            else
                sqlParams[5].Value = DBNull.Value;

            if (row.CustomerIDDetail != System.String.Empty)
            {
                if (row.CustomerIDDetail.Trim().Length > 50)
                    sqlParams[6].Value = row.CustomerIDDetail.Trim().Substring(0, 50);   //PRIMEPOS-2692 09-Sep-2019 JY Added
                else
                    sqlParams[6].Value = row.CustomerIDDetail.Trim();
            }
            else
                sqlParams[6].Value = DBNull.Value;

            if (row.TransDetailID != 0)
                sqlParams[7].Value = row.TransDetailID;
            else
                sqlParams[7].Value = DBNull.Value;

            return (sqlParams);
        }

        #endregion

        #region Get Methods


        public DataSet Populate(System.Int32 SignLogID, IDbConnection conn)
        {
            try
            {
                string sSQL = "Select "
                    + " TSL.* "
                    + " FROM "
                    + clsPOSDBConstants.POSTransSignLog_tbl + " as TSL "
                    + " WHERE " + clsPOSDBConstants.POSTransSignLog_Fld_SignLogID + " = " + SignLogID ;

                DataSet ds = new DataSet();
                ds = DataHelper.ExecuteDataset(conn, CommandType.Text, sSQL);
                return ds;
            }
            catch (POSExceptions ex)
            {
                logger.Fatal(ex, "Populate(System.Int32 SignLogID, IDbConnection conn)");
                throw (ex);
            }

            catch (OtherExceptions ex)
            {
                logger.Fatal(ex, "Populate(System.Int32 SignLogID, IDbConnection conn)");
                throw (ex);
            }

            catch (Exception ex)
            {
                logger.Fatal(ex, "Populate(System.Int32 SignLogID, IDbConnection conn)");
                ErrorHandler.throwException(ex, "", "");
                return null;
            }
        }

        public POSTransSignLogData Populate(System.Int32 SignLogID)
        {
            IDbConnection conn = DataFactory.CreateConnection();
            conn.ConnectionString = POS_Core.Resources.Configuration.ConnectionString;
            DataSet ds = Populate(SignLogID, conn);

            POSTransSignLogData oPOSTransSignLogData = new POSTransSignLogData();
            oPOSTransSignLogData.POSTransSignLog.MergeTable(ds.Tables[0]);
            return oPOSTransSignLogData;

        }

        public POSTransSignLogData PupulateByTransID(System.Int32 TransId)
        {
            IDbConnection conn = DataFactory.CreateConnection();
            conn.ConnectionString = POS_Core.Resources.Configuration.ConnectionString;
            DataSet ds = PupulateByTransID(TransId, conn);

            POSTransSignLogData oPOSTransSignLogData = new POSTransSignLogData();
            oPOSTransSignLogData.POSTransSignLog.MergeTable(ds.Tables[0]);
            return oPOSTransSignLogData;

        }
        public POSTransSignLogData PopulateList(string whereClause)
        {
            IDbConnection conn = DataFactory.CreateConnection();
            conn.ConnectionString = POS_Core.Resources.Configuration.ConnectionString;
            try
            {
                string sSQL = "Select "
                        + " TSL.* "
                        + " FROM "
                        + clsPOSDBConstants.POSTransSignLog_tbl + " as TSL "
                        + " WHERE " + whereClause;

                DataSet ds = new DataSet();
                ds = DataHelper.ExecuteDataset(conn, CommandType.Text, sSQL);

                POSTransSignLogData oPOSTransSignLogData = new POSTransSignLogData();
                oPOSTransSignLogData.POSTransSignLog.MergeTable(ds.Tables[0]);

                return oPOSTransSignLogData;
            }
            catch (POSExceptions ex)
            {
                logger.Fatal(ex, "PopulateList(string whereClause)");
                throw (ex);
            }

            catch (OtherExceptions ex)
            {
                logger.Fatal(ex, "PopulateList(string whereClause)");
                throw (ex);
            }

            catch (Exception ex)
            {
                logger.Fatal(ex, "PopulateList(string whereClause)");
                ErrorHandler.throwException(ex, "", "");
                return null;
            }

        }
        public DataSet PupulateByTransID(System.Int32 TransId, IDbConnection conn)
        {
            try
            {
                string sSQL = "Select "
                    + " TSL.* "
                    + " FROM "
                    + clsPOSDBConstants.POSTransSignLog_tbl + " as TSL "
                    + " WHERE " + clsPOSDBConstants.POSTransSignLog_Fld_POSTransId + " = " + TransId;

                DataSet ds = new DataSet();
                ds = DataHelper.ExecuteDataset(conn, CommandType.Text, sSQL);
                return ds;
            }
            catch (POSExceptions ex)
            {
                logger.Fatal(ex, "PupulateByTransID(System.Int32 TransId, IDbConnection conn)");
                throw (ex);
            }

            catch (OtherExceptions ex)
            {
                logger.Fatal(ex, "PupulateByTransID(System.Int32 TransId, IDbConnection conn)");
                throw (ex);
            }

            catch (Exception ex)
            {
                logger.Fatal(ex, "PupulateByTransID(System.Int32 TransId, IDbConnection conn)");
                ErrorHandler.throwException(ex, "", "");
                return null;
            }
        }

        #endregion //Get Method

        public void Dispose() { }
    }
}
