using NLog;
using POS_Core.CommonData;
using POS_Core.CommonData.Rows;
using POS_Core.CommonData.Tables;
using POS_Core.ErrorLogging;
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
    public class ConsentTansmissionLogSvr
    {
        private static ILogger logger = LogManager.GetCurrentClassLogger();

        #region Persist Methods
        // Inserts, updates or deletes rows in a DataSet, within a database transaction.
        public void Persist(ConsentTransmissionData updates, IDbTransaction tx, int ConsentLogId)
        {
            try
            {
                this.Insert(updates, tx, ConsentLogId);
            }
            catch (POSExceptions ex)
            {
                logger.Fatal(ex, "Persist(ConsentTransmissionData updates, IDbTransaction tx, System.Int32 TransID)");
                throw (ex);
            }
            catch (OtherExceptions ex)
            {
                logger.Fatal(ex, "Persist(ConsentTransmissionData updates, IDbTransaction tx, System.Int32 TransID)");
                throw (ex);
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "Persist(ConsentTransmissionData updates, IDbTransaction tx, System.Int32 TransID)");
                tx.Rollback();
                POS_Core.ErrorLogging.ErrorHandler.throwException(ex, "", "");
            }
        }
        #endregion

        #region Insert, Update, and Delete Methods
        public void Insert(ConsentTransmissionData ds, IDbTransaction tx, System.Int32 ConsentLogId)
        {
            ConsentTransmissionDataTable addedTable = ds.ConsentTransmission;
            string sSQL;
            IDbDataParameter[] insParam;

            if (addedTable != null && addedTable.Rows.Count > 0)
            {
                foreach (ConsentTransmissionDataRow row in addedTable.Rows)
                {
                    try
                    {
                        if (row.ConsentLogId != 0)
                        {
                            if (row.SignatureData != null)
                            {
                                insParam = InsertParameters(row);
                                sSQL = BuildInsertSQL(clsPOSDBConstants.ConsentTransmissionData_tbl, insParam);
                                DataHelper.ExecuteNonQuery(tx, CommandType.Text, sSQL, insParam);
                            }
                        }
                    }
                    catch (POSExceptions ex)
                    {
                        logger.Fatal(ex, "Insert(ConsentTransmissionData ds, IDbTransaction tx, System.Int32 TransID)");
                        throw (ex);
                    }
                    catch (OtherExceptions ex)
                    {
                        logger.Fatal(ex, "Insert(ConsentTransmissionData ds, IDbTransaction tx, System.Int32 TransID)");
                        throw (ex);
                    }
                    catch (Exception ex)
                    {
                        logger.Fatal(ex, "Insert(ConsentTransmissionData ds, IDbTransaction tx, System.Int32 TransID)");
                        ErrorHandler.throwException(ex, "", "");
                    }
                }
                addedTable.AcceptChanges();
            }
        }

        public void PersistPrescription(DataTable prescriptionDt, IDbTransaction tx)//PRIMEPOS-3192
        {
            try
            {
                this.InsertPrescription(prescriptionDt, tx);
            }
            catch (POSExceptions ex)
            {
                logger.Fatal(ex, "PersistPrescription(ConsentTransmissionData updates, IDbTransaction tx, System.Int32 TransID)");
                throw (ex);
            }
            catch (OtherExceptions ex)
            {
                logger.Fatal(ex, "PersistPrescription(ConsentTransmissionData updates, IDbTransaction tx, System.Int32 TransID)");
                throw (ex);
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "PersistPrescription(ConsentTransmissionData updates, IDbTransaction tx, System.Int32 TransID)");
                tx.Rollback();
                POS_Core.ErrorLogging.ErrorHandler.throwException(ex, "", "");
            }
        }

        public void InsertPrescription(DataTable dt, IDbTransaction tx)
        {
            string sSQL = string.Empty;

            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    try
                    {
                        int specificProdId = string.IsNullOrEmpty(Convert.ToString(row["SpecificProductId"])) ? 0 : Convert.ToInt32(row["SpecificProductId"]);
                        if (string.IsNullOrEmpty(Convert.ToString(row["PatientConsentID"])))
                        {
                            sSQL = "insert into PrescriptionConsentTransmissionLog (RxNo,PatientNo,DrugNDC,IsNewRx,IsChecked,SpecificProductId) values (" + Convert.ToInt32(row["RxNo"]) + "," + Convert.ToInt64(row["PatientNo"]) + ",'" + Convert.ToString(row["DrugNDC"]) + "','" + Convert.ToString(row["IsNewRx"]) + "','" + Convert.ToString(row["IsChecked"]) + "'," + specificProdId + ")";
                        }
                        else
                        {
                            sSQL = "insert into PrescriptionConsentTransmissionLog (RxNo,PatientNo,DrugNDC,PatientConsentID,IsNewRx,IsChecked,SpecificProductId) values (" + Convert.ToInt32(row["RxNo"]) + "," + Convert.ToInt64(row["PatientNo"]) + ",'" + Convert.ToString(row["DrugNDC"]) + "'," + Convert.ToInt32(row["PatientConsentID"]) + ",'" + Convert.ToString(row["IsNewRx"]) + "','" + Convert.ToString(row["IsChecked"]) + "'," + specificProdId + ")";
                        }
                        DataHelper.ExecuteNonQuery(tx, CommandType.Text, sSQL);
                    }
                    catch (POSExceptions ex)
                    {
                        logger.Fatal(ex, "InsertPrescription(DataTable dt, IDbTransaction tx)");
                        throw (ex);
                    }
                    catch (OtherExceptions ex)
                    {
                        logger.Fatal(ex, "InsertPrescription(DataTable dt, IDbTransaction tx)");
                        throw (ex);
                    }
                    catch (Exception ex)
                    {
                        logger.Fatal(ex, "InsertPrescription(DataTable dt, IDbTransaction tx)");
                        ErrorHandler.throwException(ex, "", "");
                    }
                }
            }
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
        private IDbDataParameter[] PKParameters(System.Int32 ConsentLogId)
        {
            IDbDataParameter[] sqlParams = DataFactory.CreateParameterArray(1);

            sqlParams[0] = DataFactory.CreateParameter();
            sqlParams[0].ParameterName = "@ConsentLogId";
            sqlParams[0].DbType = System.Data.DbType.Int32;

            sqlParams[0].Value = ConsentLogId;

            return (sqlParams);
        }

        private IDbDataParameter[] PKParameters(ConsentTransmissionDataRow row)
        {
            //return a SqlParameterCollection
            IDbDataParameter[] sqlParams = DataFactory.CreateParameterArray(1);

            sqlParams[0] = DataFactory.CreateParameter();
            sqlParams[0].ParameterName = "@ConsentLogId";
            sqlParams[0].DbType = System.Data.DbType.Int32;

            sqlParams[0].Value = row.ConsentLogId;
            sqlParams[0].SourceColumn = clsPOSDBConstants.ConsentTransmissionData_Fld_ConsentLogId;

            return (sqlParams);
        }

        private IDbDataParameter[] InsertParameters(ConsentTransmissionDataRow row)
        {
            IDbDataParameter[] sqlParams = DataFactory.CreateParameterArray(12);

            //sqlParams[0] = DataFactory.CreateParameter("@" + clsPOSDBConstants.ConsentTransmissionData_Fld_RxTransNo, System.Data.DbType.Int32);
            //sqlParams[0] = DataFactory.CreateParameter("@" + clsPOSDBConstants.ConsentTransmissionData_Fld_ConsentLogId, System.Data.DbType.Int32);
            sqlParams[0] = DataFactory.CreateParameter("@" + clsPOSDBConstants.ConsentTransmissionData_Fld_ConsentSourceID, System.Data.DbType.Int32);
            sqlParams[1] = DataFactory.CreateParameter("@" + clsPOSDBConstants.ConsentTransmissionData_Fld_ConsentTypeId, System.Data.DbType.Int32);
            sqlParams[2] = DataFactory.CreateParameter("@" + clsPOSDBConstants.ConsentTransmissionData_Fld_ConsentTextId, System.Data.DbType.Int32);
            sqlParams[3] = DataFactory.CreateParameter("@" + clsPOSDBConstants.ConsentTransmissionData_Fld_ConsentStatusId, System.Data.DbType.Int32);
            sqlParams[4] = DataFactory.CreateParameter("@" + clsPOSDBConstants.ConsentTransmissionData_Fld_ConsentRelationId, System.Data.DbType.Int32);
            sqlParams[5] = DataFactory.CreateParameter("@" + clsPOSDBConstants.ConsentTransmissionData_Fld_PatientNo, System.Data.DbType.Int32);
            sqlParams[6] = DataFactory.CreateParameter("@" + clsPOSDBConstants.ConsentTransmissionData_Fld_ConsentCaptureDate, System.Data.DbType.DateTime);
            sqlParams[7] = DataFactory.CreateParameter("@" + clsPOSDBConstants.ConsentTransmissionData_Fld_ConsentExpiryDate, System.Data.DbType.DateTime);
            sqlParams[8] = DataFactory.CreateParameter("@" + clsPOSDBConstants.ConsentTransmissionData_Fld_SigneeName, System.Data.DbType.String);
            sqlParams[9] = DataFactory.CreateParameter("@" + clsPOSDBConstants.ConsentTransmissionData_Fld_SignatureData, System.Data.DbType.Byte);
            sqlParams[10] = DataFactory.CreateParameter("@" + clsPOSDBConstants.ConsentTransmissionData_Fld_RxNo, System.Data.DbType.Int64);
            sqlParams[11] = DataFactory.CreateParameter("@" + clsPOSDBConstants.ConsentTransmissionData_Fld_Nrefill, System.Data.DbType.Int32);

            //sqlParams[0].SourceColumn = clsPOSDBConstants.ConsentTransmissionData_Fld_RxTransNo;
            //sqlParams[0].SourceColumn = clsPOSDBConstants.ConsentTransmissionData_Fld_ConsentLogId;
            sqlParams[0].SourceColumn = clsPOSDBConstants.ConsentTransmissionData_Fld_ConsentSourceID;
            sqlParams[1].SourceColumn = clsPOSDBConstants.ConsentTransmissionData_Fld_ConsentTypeId;
            sqlParams[2].SourceColumn = clsPOSDBConstants.ConsentTransmissionData_Fld_ConsentTextId;
            sqlParams[3].SourceColumn = clsPOSDBConstants.ConsentTransmissionData_Fld_ConsentStatusId;
            sqlParams[4].SourceColumn = clsPOSDBConstants.ConsentTransmissionData_Fld_ConsentRelationId;
            sqlParams[5].SourceColumn = clsPOSDBConstants.ConsentTransmissionData_Fld_PatientNo;
            sqlParams[6].SourceColumn = clsPOSDBConstants.ConsentTransmissionData_Fld_ConsentCaptureDate;
            sqlParams[7].SourceColumn = clsPOSDBConstants.ConsentTransmissionData_Fld_ConsentExpiryDate;
            sqlParams[8].SourceColumn = clsPOSDBConstants.ConsentTransmissionData_Fld_SigneeName;
            sqlParams[9].SourceColumn = clsPOSDBConstants.ConsentTransmissionData_Fld_SignatureData;
            sqlParams[10].SourceColumn = clsPOSDBConstants.ConsentTransmissionData_Fld_RxNo;
            sqlParams[11].SourceColumn = clsPOSDBConstants.ConsentTransmissionData_Fld_Nrefill;

            //sqlParams[0].Value = string.IsNullOrEmpty(row.ConsentLogId.ToString()) ? 0 : row.ConsentLogId;
            sqlParams[0].Value = string.IsNullOrEmpty(row.ConsentSourceID.ToString()) ? 0 : row.ConsentSourceID;
            sqlParams[1].Value = string.IsNullOrEmpty(row.ConsentTypeId.ToString()) ? 0 : row.ConsentTypeId;
            sqlParams[2].Value = string.IsNullOrEmpty(row.ConsentTextId.ToString()) ? 0 : row.ConsentTextId;
            sqlParams[3].Value = string.IsNullOrEmpty(row.ConsentStatusId.ToString()) ? 0 : row.ConsentStatusId;
            sqlParams[4].Value = string.IsNullOrEmpty(row.ConsentRelationId.ToString()) ? 0 : row.ConsentRelationId;
            sqlParams[5].Value = string.IsNullOrEmpty(row.PatientNo.ToString()) ? 0 : row.PatientNo;
            sqlParams[6].Value = row.ConsentCaptureDate == DateTime.MinValue ? Convert.ToDateTime("1/1/1753 12:00:00") : row.ConsentCaptureDate;
            sqlParams[7].Value = row.ConsentExpiryDate == DateTime.MinValue ? Convert.ToDateTime("1/1/1753 12:00:00") : row.ConsentExpiryDate;
            sqlParams[8].Value = string.IsNullOrEmpty(row.SigneeName.ToString()) ? string.Empty : row.SigneeName;
            sqlParams[9].Value = row.SignatureData == null ? null : row.SignatureData;
            sqlParams[10].Value = string.IsNullOrEmpty(row.RxNo.ToString()) ? 0 : row.RxNo;
            sqlParams[11].Value = string.IsNullOrEmpty(row.Nrefill.ToString()) ? 0 : row.Nrefill;
            return (sqlParams);
        }

        #endregion


        public DataSet Populate(IDbConnection conn)
        {
            string sSQL = "";
            try
            {
                sSQL = "Select  0 RxTransNo , ConsentCaptureDate TransDateTime		, PatientNo, RxNo, Nrefill, '' PickedUp, 0 TransID, '' IsDelivery, RxSync IsRxSync, 0 TransAmount, 0 StationID, 0 UserID, '' CreatedDate, '' ModifiedBy, '' ModifiedDate, '' PickUpPOS, ConsentTextID, ConsentTypeID, ConsentStatusID, ConsentCaptureDate, '' ConsentEffectiveDate, ConsentExpiryDate ConsentEndDate, ConsentRelationId RelationID, SigneeName, SignatureData, '' PickUpdate, 0 CopayPaid,ConsentSourceID FROM " + clsPOSDBConstants.ConsentTransmissionData_tbl + " Where RxSync = 0";
                ConsentTransmissionData ds = new ConsentTransmissionData();
                ds.ConsentTransmission.MergeTable(DataHelper.ExecuteDataset(conn, CommandType.Text, sSQL).Tables[0]);
                return ds;
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Populate(System.Int32 TransId, IDbConnection conn)");
                ErrorHandler.throwException(ex, "", "");
                return null;
            }
        }

        public DataSet Populate()
        {
            IDbConnection conn = DataFactory.CreateConnection();
            conn.ConnectionString = POS_Core.Resources.Configuration.ConnectionString;
            return (Populate(conn));
        }
        #region PRIMEPOS-3192
        public DataTable RxPrescConsentPopulate()
        {
            IDbConnection conn = DataFactory.CreateConnection();
            conn.ConnectionString = POS_Core.Resources.Configuration.ConnectionString;
            return (RxPrescConsentPopulatePopulate(conn));
        }
        public DataTable RxPrescConsentPopulatePopulate(IDbConnection conn)
        {
            string sSQL = "";
            try
            {
                sSQL = "Select RxNo,PatientNo,DrugNDC,PatientConsentID,IsNewRx,IsChecked,SpecificProductId from PrescriptionConsentTransmissionLog with (NOLOCK)";
                DataTable dt = new DataTable();
                dt = DataHelper.ExecuteDataset(conn, CommandType.Text, sSQL).Tables[0];
                return dt;
            }
            catch (Exception ex)
            {
                logger.Error(ex, "RxPrescConsentPopulatePopulate(IDbConnection conn)");
                ErrorHandler.throwException(ex, "", "");
                return null;
            }
        }

        public bool UpdatePrescriptionConsentRXSync(string RxNo)
        {
            bool IsSyncSuccess = false;
            try
            {
                using (IDbConnection conn = DataFactory.CreateConnection())
                {
                    conn.ConnectionString = DBConfig.ConnectionString;
                    conn.Open();
                    IDbTransaction oTrans = conn.BeginTransaction();
                    try
                    {
                        DeletePrescriptionConsentRXSync(oTrans, RxNo);
                        IsSyncSuccess = true;
                        oTrans.Commit();
                    }
                    catch (Exception Ex1)
                    {
                        logger.Error(Ex1.Message, "Inner loop - UpdatePrescriptionConsentRXSync()");
                        if (oTrans != null)
                            oTrans.Rollback();
                        ErrorHandler.throwException(Ex1, "", "");
                    }
                }
            }
            catch (Exception Ex)
            {
                logger.Error(Ex.Message, "Outer loop - UpdatePrescriptionConsentRXSync()");
                ErrorHandler.throwException(Ex, "", "");
            }
            return IsSyncSuccess;
        }
        private bool DeletePrescriptionConsentRXSync(IDbTransaction oTrans, string RxNo)
        {
            bool IsDeleted = false;
            try
            {
                string sSQL = "DELETE PrescriptionConsentTransmissionLog Where RxNo =" + RxNo;
                DataHelper.ExecuteNonQuery(oTrans, CommandType.Text, sSQL);
                IsDeleted = true;
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message, "DeletePrescriptionConsentRXSync(IDbTransaction oTrans,string RxNo)");
                if (oTrans != null)
                    oTrans.Rollback();
                ErrorHandler.throwException(ex, "", "");
            }
            return IsDeleted;
        }

        #endregion

        #region commented
        //// PRIMEPOS-CONSENT SAJID DHUKKA
        //public bool UpdateConsentRXSync(IDbTransaction oTrans)
        //{
        //    bool IsSyncSuccess = false;
        //    try
        //    {
        //        string sSQL = "UPDATE " + clsPOSDBConstants.ConsentTransmissionData_tbl + " SET RxSync = 1 where ISNULL(RxSync,0) = 0";
        //        IDbCommand cmd = DataFactory.CreateCommand();
        //        cmd.CommandType = CommandType.Text;
        //        cmd.CommandText = sSQL;
        //        cmd.Transaction = oTrans;
        //        cmd.Connection = oTrans.Connection;
        //        object result;
        //        result = cmd.ExecuteNonQuery();

        //        InsertConsentRxTransmissionHistory(oTrans);
        //        IsSyncSuccess = true;
        //    }
        //    catch (Exception Ex)
        //    {
        //        logger.Error(Ex.Message, "UpdateRXSync(int TransID, string UserName, IDbTransaction oTrans)");
        //        oTrans.Rollback();
        //        throw Ex;

        //    }
        //    return IsSyncSuccess;
        //}
        //private void InsertConsentRxTransmissionHistory(IDbTransaction oTrans)
        //{
        //    try
        //    {
        //        IDbConnection conn = DataFactory.CreateConnection();
        //        conn.ConnectionString = POS_Core.Resources.Configuration.ConnectionString;
        //        string sSQL = "Insert Into RXTransmission_History(PatientID, RxNo,Nrefill,IsRxSync,ConsentTextID,ConsentTypeID,ConsentStatusID,ConsentCaptureDate,ConsentEndDate, RelationID, SigneeName, SignatureData,ConsentSourceId) ";
        //        sSQL += " Select PatientNo,RxNo,Nrefill,RxSync,ConsentTextID,ConsentTypeID,ConsentStatusID,ConsentCaptureDate,ConsentExpiryDate, ConsentRelationID, SigneeName,SignatureData,ConsentSourceId " +
        //           " FROM " + clsPOSDBConstants.ConsentTransmissionData_tbl + " Where RxSync = 1 AND ConsentSourceId != 1";

        //        IDbCommand cmd = DataFactory.CreateCommand();
        //        cmd.CommandType = CommandType.Text;
        //        cmd.CommandText = sSQL;
        //        cmd.Transaction = oTrans;
        //        cmd.Connection = oTrans.Connection;
        //        object result;
        //        result = cmd.ExecuteNonQuery();


        //        DeleteConsentRXSync(oTrans);

        //        //DataSet ds = DataHelper.ExecuteDataset(conn, CommandType.Text, sSQL);
        //    }
        //    catch (Exception ex)
        //    {
        //        logger.Error(ex, "InsertConsentRxTransmissionHistory()");
        //        oTrans.Rollback();
        //        ErrorHandler.throwException(ex, "", "");
        //    }
        //}
        //private bool DeleteConsentRXSync(IDbTransaction oTrans)
        //{
        //    bool IsDeleted = false;
        //    try
        //    {
        //        string sSQL = "DELETE " + clsPOSDBConstants.ConsentTransmissionData_tbl + " Where RxSync = 1";
        //        IDbCommand cmd = DataFactory.CreateCommand();
        //        cmd.CommandType = CommandType.Text;
        //        cmd.CommandText = sSQL;
        //        cmd.Transaction = oTrans;
        //        cmd.Connection = oTrans.Connection;
        //        object result;
        //        result = cmd.ExecuteNonQuery();
        //        IsDeleted = true;
        //    }
        //    catch (Exception ex)
        //    {
        //        oTrans.Rollback();
        //        logger.Error(ex.Message, "DeleteConsentRXSync(IDbTransaction oTrans)");
        //        throw ex;

        //    }
        //    return IsDeleted;
        //}
        #endregion

        #region PRIMEPOS-2976 15-Jun-2021 JY modified only connection and not logic (PRIMEPOS-CONSENT SAJID DHUKKA)
        public bool UpdateConsentRXSync()
        {
            bool IsSyncSuccess = false;
            try
            {
                using (IDbConnection conn = DataFactory.CreateConnection())
                {
                    conn.ConnectionString = DBConfig.ConnectionString;
                    conn.Open();
                    IDbTransaction oTrans = conn.BeginTransaction();
                    try
                    {
                        string sSQL = "UPDATE " + clsPOSDBConstants.ConsentTransmissionData_tbl + " SET RxSync = 1 where ISNULL(RxSync,0) = 0";
                        DataHelper.ExecuteNonQuery(oTrans, CommandType.Text, sSQL);
                        InsertConsentRxTransmissionHistory(oTrans);
                        IsSyncSuccess = true;
                        oTrans.Commit();
                    }
                    catch (Exception Ex1)
                    {
                        logger.Error(Ex1.Message, "Inner loop - UpdateConsentRXSync()");
                        if (oTrans != null)
                            oTrans.Rollback();
                        ErrorHandler.throwException(Ex1, "", "");
                    }
                }
            }
            catch (Exception Ex)
            {
                logger.Error(Ex.Message, "Outer loop - UpdateConsentRXSync()");
                ErrorHandler.throwException(Ex, "", "");
            }
            return IsSyncSuccess;
        }


        private void InsertConsentRxTransmissionHistory(IDbTransaction oTrans)
        {
            try
            {
                string sSQL = "Insert Into RXTransmission_History(PatientID, RxNo,Nrefill,IsRxSync,ConsentTextID,ConsentTypeID,ConsentStatusID,ConsentCaptureDate,ConsentEndDate, RelationID, SigneeName, SignatureData,ConsentSourceId) ";
                sSQL += " Select PatientNo,RxNo,Nrefill,RxSync,ConsentTextID,ConsentTypeID,ConsentStatusID,ConsentCaptureDate,ConsentExpiryDate, ConsentRelationID, SigneeName,SignatureData,ConsentSourceId " +
                   " FROM " + clsPOSDBConstants.ConsentTransmissionData_tbl + " Where RxSync = 1 AND ConsentSourceId != 1";

                DataHelper.ExecuteNonQuery(oTrans, CommandType.Text, sSQL);
                DeleteConsentRXSync(oTrans);
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message, "InsertConsentRxTransmissionHistory(IDbTransaction oTrans)");
                if (oTrans != null)
                    oTrans.Rollback();
                ErrorHandler.throwException(ex, "", "");
            }
        }

        private bool DeleteConsentRXSync(IDbTransaction oTrans)
        {
            bool IsDeleted = false;
            try
            {
                string sSQL = "DELETE " + clsPOSDBConstants.ConsentTransmissionData_tbl + " Where RxSync = 1";
                DataHelper.ExecuteNonQuery(oTrans, CommandType.Text, sSQL);
                IsDeleted = true;
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message, "DeleteConsentRXSync(IDbTransaction oTrans)");
                if (oTrans != null)
                    oTrans.Rollback();
                ErrorHandler.throwException(ex, "", "");
            }
            return IsDeleted;
        }
        #endregion
    }
}