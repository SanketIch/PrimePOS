//----------------------------------------------------------------------------------------------------
//PRIMEPOS-2339 04-Oct-2016 JY Added to maintain InsSigTrans
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
    

    class InsSigTransSvr : IDisposable
    {
        private static ILogger logger = LogManager.GetCurrentClassLogger();

        #region Persist Methods
        // Inserts, updates or deletes rows in a DataSet, within a database transaction.
        public void Persist(DataSet updates, bool bPrivacyAck, IDbTransaction tx)
        {
            try
            {
                this.Insert(updates, bPrivacyAck, tx);
                updates.AcceptChanges();
            }
            catch (POSExceptions ex)
            {
                logger.Fatal(ex, "Persist(DataSet updates, bool bPrivacyAck, IDbTransaction tx)");
                throw (ex);
            }
            catch (OtherExceptions ex)
            {
                logger.Fatal(ex, "Persist(DataSet updates, bool bPrivacyAck, IDbTransaction tx)");
                throw (ex);
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "Persist(DataSet updates, bool bPrivacyAck, IDbTransaction tx)");
                ErrorHandler.throwException(ex, "", "");
            }
        }
        #endregion

        #region Insert Methods
        public void Insert(DataSet ds, bool bPrivacyAck, IDbTransaction tx)
        {

            InsSigTransTable addedTable = (InsSigTransTable)ds.Tables[0].GetChanges(DataRowState.Added);
            string sSQL;
            IDbDataParameter[] insParam;

            if (addedTable != null && addedTable.Rows.Count > 0)
            {
                foreach (InsSigTransRow row in addedTable.Rows)
                {
                    try
                    {
                        insParam = InsertParameters(row, bPrivacyAck);
                        sSQL = BuildInsertSQL(clsPOSDBConstants.InsSigTrans_tbl, insParam);

                        DataHelper.ExecuteNonQuery(tx, CommandType.Text, sSQL, insParam);
                    }
                    catch (POSExceptions ex)
                    {
                        logger.Fatal(ex, "Insert(DataSet ds, bool bPrivacyAck, IDbTransaction tx)");
                        throw (ex);
                    }
                    catch (OtherExceptions ex)
                    {
                        logger.Fatal(ex, "Insert(DataSet ds, bool bPrivacyAck, IDbTransaction tx)");
                        throw (ex);
                    }
                    catch (SqlException ex)
                    {
                        logger.Fatal(ex, "Insert(DataSet ds, bool bPrivacyAck, IDbTransaction tx)");
                        throw (ex);
                    }
                    catch (Exception ex)
                    {
                        logger.Fatal(ex, "Insert(DataSet ds, bool bPrivacyAck, IDbTransaction tx)");
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

        private IDbDataParameter[] PKParameters(InsSigTransRow row)
        {
            //return a SqlParameterCollection
            IDbDataParameter[] sqlParams = DataFactory.CreateParameterArray(1);

            sqlParams[0] = DataFactory.CreateParameter();
            sqlParams[0].ParameterName = "@ID";
            sqlParams[0].DbType = System.Data.DbType.Int32;

            sqlParams[0].Value = row.ID;
            sqlParams[0].SourceColumn = clsPOSDBConstants.InsSigTrans_tbl;

            return (sqlParams);
        }

        private IDbDataParameter[] InsertParameters(InsSigTransRow row, bool bPrivacyAck)
        {
            IDbDataParameter[] sqlParams;
            if (bPrivacyAck == true)
            {
                sqlParams = DataFactory.CreateParameterArray(13);

                sqlParams[0] = DataFactory.CreateParameter("@" + clsPOSDBConstants.InsSigTrans_Fld_TransID, System.Data.DbType.Int32);
                sqlParams[1] = DataFactory.CreateParameter("@" + clsPOSDBConstants.InsSigTrans_Fld_PatientNo, System.Data.DbType.Int32);
                sqlParams[2] = DataFactory.CreateParameter("@" + clsPOSDBConstants.InsSigTrans_Fld_InsType, System.Data.DbType.String);
                sqlParams[3] = DataFactory.CreateParameter("@" + clsPOSDBConstants.InsSigTrans_Fld_TransData, System.Data.DbType.String);
                sqlParams[4] = DataFactory.CreateParameter("@" + clsPOSDBConstants.InsSigTrans_Fld_TransSigData, System.Data.DbType.String);
                sqlParams[5] = DataFactory.CreateParameter("@" + clsPOSDBConstants.InsSigTrans_Fld_CounselingReq, System.Data.DbType.String);
                sqlParams[6] = DataFactory.CreateParameter("@" + clsPOSDBConstants.InsSigTrans_Fld_SigType, System.Data.DbType.String);
                sqlParams[7] = DataFactory.CreateParameter("@" + clsPOSDBConstants.InsSigTrans_Fld_BinarySign, System.Data.DbType.Binary);
                sqlParams[8] = DataFactory.CreateParameter("@" + clsPOSDBConstants.InsSigTrans_Fld_PrivacyPatAccept, System.Data.DbType.String);
                sqlParams[9] = DataFactory.CreateParameter("@" + clsPOSDBConstants.InsSigTrans_Fld_PrivacyText, System.Data.DbType.String);
                sqlParams[10] = DataFactory.CreateParameter("@" + clsPOSDBConstants.InsSigTrans_Fld_PrivacySig, System.Data.DbType.String);
                sqlParams[11] = DataFactory.CreateParameter("@" + clsPOSDBConstants.InsSigTrans_Fld_PrivacySigType, System.Data.DbType.String);
                sqlParams[12] = DataFactory.CreateParameter("@" + clsPOSDBConstants.InsSigTrans_Fld_PrivacyBinarySign, System.Data.DbType.Binary);

                sqlParams[0].SourceColumn = clsPOSDBConstants.InsSigTrans_Fld_TransID;
                sqlParams[1].SourceColumn = clsPOSDBConstants.InsSigTrans_Fld_PatientNo;
                sqlParams[2].SourceColumn = clsPOSDBConstants.InsSigTrans_Fld_InsType;
                sqlParams[3].SourceColumn = clsPOSDBConstants.InsSigTrans_Fld_TransData;
                sqlParams[4].SourceColumn = clsPOSDBConstants.InsSigTrans_Fld_TransSigData;
                sqlParams[5].SourceColumn = clsPOSDBConstants.InsSigTrans_Fld_CounselingReq;
                sqlParams[6].SourceColumn = clsPOSDBConstants.InsSigTrans_Fld_SigType;
                sqlParams[7].SourceColumn = clsPOSDBConstants.InsSigTrans_Fld_BinarySign;
                sqlParams[8].SourceColumn = clsPOSDBConstants.InsSigTrans_Fld_PrivacyPatAccept;
                sqlParams[9].SourceColumn = clsPOSDBConstants.InsSigTrans_Fld_PrivacyText;
                sqlParams[10].SourceColumn = clsPOSDBConstants.InsSigTrans_Fld_PrivacySig;
                sqlParams[11].SourceColumn = clsPOSDBConstants.InsSigTrans_Fld_PrivacySigType;
                sqlParams[12].SourceColumn = clsPOSDBConstants.InsSigTrans_Fld_PrivacyBinarySign;

                sqlParams[0].Value = row.TransID;

                if (row.PatientNo != 0)
                    sqlParams[1].Value = row.PatientNo;
                else
                    sqlParams[1].Value = DBNull.Value;

                if (row.InsType != System.String.Empty)
                    sqlParams[2].Value = row.InsType;
                else
                    sqlParams[2].Value = DBNull.Value;

                if (row.TransData != System.String.Empty)
                    sqlParams[3].Value = row.TransData;
                else
                    sqlParams[3].Value = DBNull.Value;

                if (row.TransSigData != System.String.Empty)
                    sqlParams[4].Value = row.TransSigData;
                else
                    sqlParams[4].Value = DBNull.Value;

                if (row.CounselingReq != System.String.Empty)
                    sqlParams[5].Value = row.CounselingReq;
                else
                    sqlParams[5].Value = DBNull.Value;

                if (row.SigType != System.String.Empty)
                    sqlParams[6].Value = row.SigType;
                else
                    sqlParams[6].Value = DBNull.Value;

                if (row.BinarySign != null)
                    sqlParams[7].Value = row.BinarySign;
                else
                    sqlParams[7].Value = System.Data.SqlTypes.SqlBinary.Null;

                if (row.PrivacyPatAccept != System.String.Empty)
                    sqlParams[8].Value = row.PrivacyPatAccept;
                else
                    sqlParams[8].Value = DBNull.Value;

                if (row.PrivacyText != System.String.Empty)
                    sqlParams[9].Value = row.PrivacyText;
                else
                    sqlParams[9].Value = DBNull.Value;

                if (row.PrivacySig != System.String.Empty)
                    sqlParams[10].Value = row.PrivacySig;
                else
                    sqlParams[10].Value = DBNull.Value;

                if (row.PrivacySigType != System.String.Empty)
                    sqlParams[11].Value = row.PrivacySigType;
                else
                    sqlParams[11].Value = DBNull.Value;

                if (row.PrivacyBinarySign != null)
                    sqlParams[12].Value = row.PrivacyBinarySign;
                else
                    sqlParams[12].Value = System.Data.SqlTypes.SqlBinary.Null;
            }
            else
            {
                sqlParams = DataFactory.CreateParameterArray(8);

                sqlParams[0] = DataFactory.CreateParameter("@" + clsPOSDBConstants.InsSigTrans_Fld_TransID, System.Data.DbType.Int32);
                sqlParams[1] = DataFactory.CreateParameter("@" + clsPOSDBConstants.InsSigTrans_Fld_PatientNo, System.Data.DbType.Int32);
                sqlParams[2] = DataFactory.CreateParameter("@" + clsPOSDBConstants.InsSigTrans_Fld_InsType, System.Data.DbType.String);
                sqlParams[3] = DataFactory.CreateParameter("@" + clsPOSDBConstants.InsSigTrans_Fld_TransData, System.Data.DbType.String);
                sqlParams[4] = DataFactory.CreateParameter("@" + clsPOSDBConstants.InsSigTrans_Fld_TransSigData, System.Data.DbType.String);
                sqlParams[5] = DataFactory.CreateParameter("@" + clsPOSDBConstants.InsSigTrans_Fld_CounselingReq, System.Data.DbType.String);
                sqlParams[6] = DataFactory.CreateParameter("@" + clsPOSDBConstants.InsSigTrans_Fld_SigType, System.Data.DbType.String);
                sqlParams[7] = DataFactory.CreateParameter("@" + clsPOSDBConstants.InsSigTrans_Fld_BinarySign, System.Data.DbType.Binary);

                sqlParams[0].SourceColumn = clsPOSDBConstants.InsSigTrans_Fld_TransID;
                sqlParams[1].SourceColumn = clsPOSDBConstants.InsSigTrans_Fld_PatientNo;
                sqlParams[2].SourceColumn = clsPOSDBConstants.InsSigTrans_Fld_InsType;
                sqlParams[3].SourceColumn = clsPOSDBConstants.InsSigTrans_Fld_TransData;
                sqlParams[4].SourceColumn = clsPOSDBConstants.InsSigTrans_Fld_TransSigData;
                sqlParams[5].SourceColumn = clsPOSDBConstants.InsSigTrans_Fld_CounselingReq;
                sqlParams[6].SourceColumn = clsPOSDBConstants.InsSigTrans_Fld_SigType;
                sqlParams[7].SourceColumn = clsPOSDBConstants.InsSigTrans_Fld_BinarySign;

                sqlParams[0].Value = row.TransID;

                if (row.PatientNo != 0)
                    sqlParams[1].Value = row.PatientNo;
                else
                    sqlParams[1].Value = DBNull.Value;

                if (row.InsType != System.String.Empty)
                    sqlParams[2].Value = row.InsType;
                else
                    sqlParams[2].Value = DBNull.Value;

                if (row.TransData != System.String.Empty)
                    sqlParams[3].Value = row.TransData;
                else
                    sqlParams[3].Value = DBNull.Value;

                if (row.TransSigData != System.String.Empty)
                    sqlParams[4].Value = row.TransSigData;
                else
                    sqlParams[4].Value = DBNull.Value;

                if (row.CounselingReq != System.String.Empty)
                    sqlParams[5].Value = row.CounselingReq;
                else
                    sqlParams[5].Value = DBNull.Value;

                if (row.SigType != System.String.Empty)
                    sqlParams[6].Value = row.SigType;
                else
                    sqlParams[6].Value = DBNull.Value;

                if (row.BinarySign != null)
                    sqlParams[7].Value = row.BinarySign;
                else
                    sqlParams[7].Value = System.Data.SqlTypes.SqlBinary.Null;
            }

            return (sqlParams);
        }
        #endregion
        #region PRIMEPOS-2761
        public DataSet Populate(System.Int32 TransId)
        {
            IDbConnection conn = DataFactory.CreateConnection();
            conn.ConnectionString = POS_Core.Resources.Configuration.ConnectionString;
            return (Populate(TransId, conn));
        }

        public DataSet Populate(System.Int32 TransId, IDbConnection conn)
        {
            string sSQL = "";
            try
            {
                sSQL = "select ID,TransID	,PatientNo	,InsType	,TransData	,TransSigData,	CounselingReq,	SigType	,BinarySign	,PrivacyPatAccept,	PrivacyText	,PrivacySig	,PrivacySigType,	PrivacyBinarySign  " +
                    " FROM "
                   + clsPOSDBConstants.InsSigTrans_tbl + " Where TransID=" + "@ID";

                InsSigTransData ds = new InsSigTransData();
                ds.InsSigTrans.MergeTable(DataHelper.ExecuteDataset(conn, CommandType.Text, sSQL, PKParameters(TransId)).Tables[0]);
                return ds;
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "Populate(System.Int32 TransId, IDbConnection conn)");
                ErrorHandler.throwException(ex, "", "");
                return null;
            }
        }
        #endregion

        #region PRIMEPOS-3211

        public bool UpdateIsVerified(System.Int64 TransId)
        {
            using (IDbConnection conn = DataFactory.CreateConnection())
            {
                conn.ConnectionString = POS_Core.Resources.Configuration.ConnectionString;
                return (UpdateIsVerified(TransId, conn));
            }
        }
        public bool UpdateIsVerified(System.Int64 TransId, IDbConnection conn)
        {

            string sSQL = "UPDATE InsSigTrans SET IsVerified = 1 WHERE TransID = " + Convert.ToString(TransId);
            try
            {
                DataHelper.ExecuteNonQuery(conn, CommandType.Text, sSQL);
                return true;
            }
            catch (POSExceptions ex)
            {
                logger.Fatal(ex, "InsSigTransSvr==>UpdateIsVerified(System.Int64 TransId, IDbConnection conn)");
                return false;
            }

            catch (OtherExceptions ex)
            {
                logger.Fatal(ex, "InsSigTransSvr==>UpdateIsVerified(System.Int64 TransId, IDbConnection conn)");
                return false;
            }

            catch (Exception ex)
            {
                logger.Fatal(ex, "InsSigTransSvr==>UpdateIsVerified(System.Int64 TransId, IDbConnection conn)");
                return false;
            }
        }
        #endregion

        public void Dispose() { }
    }
}
