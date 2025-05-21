using System;
using POS_Core.ErrorLogging;
using NLog;
using POS_Core.CommonData.Tables;
using POS_Core.CommonData.Rows;
using POS_Core.CommonData;
using System.Data;
using Resources;
using System.Data.SqlTypes;
using POS_Core.Resources;

namespace POS_Core.DataAccess
{
    public class RxTransactionDataSvr : IDisposable
    {
        private static ILogger logger = LogManager.GetCurrentClassLogger();

        #region Persist Methods
        // Inserts, updates or deletes rows in a DataSet, within a database transaction.
        public void Persist(RxTransactionData updates, IDbTransaction tx, System.Int32 TransID)
        {
            try
            {
                this.Insert(updates, tx, TransID);
            }
            catch (POSExceptions ex)
            {
                logger.Fatal(ex, "Persist(RxTransactionData updates, IDbTransaction tx, System.Int32 TransID)");
                throw (ex);
            }
            catch (OtherExceptions ex)
            {
                logger.Fatal(ex, "Persist(RxTransactionData updates, IDbTransaction tx, System.Int32 TransID)");
                throw (ex);
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "Persist(RxTransactionData updates, IDbTransaction tx, System.Int32 TransID)");
                tx.Rollback();
                POS_Core.ErrorLogging.ErrorHandler.throwException(ex, "", "");
            }
        }
        #endregion

        #region Insert, Update, and Delete Methods
        public void Insert(RxTransactionData ds, IDbTransaction tx, System.Int32 TransID)
        {
            RxTransactionDataTable addedTable = ds.RxTransaction;
            string sSQL;
            IDbDataParameter[] insParam;

            if (addedTable != null && addedTable.Rows.Count > 0)
            {
                foreach (RxTransactionDataRow row in addedTable.Rows)
                {
                    try
                    {
                        if (row.RxTransNo != 0)
                        {
                            row.TransID = TransID;
                            insParam = InsertParameters(row);
                            sSQL = BuildInsertSQL(clsPOSDBConstants.RxTransactionData_tbl, insParam);

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

        private string BuildDeleteSQL(string tableName, TransDetailRow row)
        {
            string sDeleteSQL = "DELETE FROM " + tableName + " WHERE ";
            sDeleteSQL += clsPOSDBConstants.TransDetail_Fld_TransDetailID + " = " + row.TransDetailID.ToString();
            return sDeleteSQL;
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
        private IDbDataParameter[] PKParameters(System.Int32 TransID)
        {
            IDbDataParameter[] sqlParams = DataFactory.CreateParameterArray(1);

            sqlParams[0] = DataFactory.CreateParameter();
            sqlParams[0].ParameterName = "@TransId";
            sqlParams[0].DbType = System.Data.DbType.Int32;

            sqlParams[0].Value = TransID;

            return (sqlParams);
        }

        private IDbDataParameter[] PKParameters(RxTransactionDataRow row)
        {
            //return a SqlParameterCollection
            IDbDataParameter[] sqlParams = DataFactory.CreateParameterArray(1);

            sqlParams[0] = DataFactory.CreateParameter();
            sqlParams[0].ParameterName = "@RxTransNo";
            sqlParams[0].DbType = System.Data.DbType.String;

            sqlParams[0].Value = row.RxTransNo;
            sqlParams[0].SourceColumn = clsPOSDBConstants.RxTransmissionData_Fld_RxTransNo;

            return (sqlParams);
        }

        private IDbDataParameter[] InsertParameters(RxTransactionDataRow row)
        {
            IDbDataParameter[] sqlParams = DataFactory.CreateParameterArray(34);//PRIMEPOS-2866

            //sqlParams[0] = DataFactory.CreateParameter("@" + clsPOSDBConstants.RxTransmissionData_Fld_RxTransNo, System.Data.DbType.Int32);
            sqlParams[0] = DataFactory.CreateParameter("@" + clsPOSDBConstants.RxTransmissionData_Fld_PatientID, System.Data.DbType.Int32);
            sqlParams[1] = DataFactory.CreateParameter("@" + clsPOSDBConstants.RxTransmissionData_Fld_RxNo, System.Data.DbType.Int64);
            sqlParams[2] = DataFactory.CreateParameter("@" + clsPOSDBConstants.RxTransmissionData_Fld_Nrefill, System.Data.DbType.Int16);
            sqlParams[3] = DataFactory.CreateParameter("@" + clsPOSDBConstants.RxTransmissionData_Fld_PickedUp, System.Data.DbType.String);
            sqlParams[4] = DataFactory.CreateParameter("@" + clsPOSDBConstants.RxTransmissionData_Fld_PickUpPOS, System.Data.DbType.String);
            sqlParams[5] = DataFactory.CreateParameter("@" + clsPOSDBConstants.RxTransmissionData_Fld_TransID, System.Data.DbType.Int32);
            sqlParams[6] = DataFactory.CreateParameter("@" + clsPOSDBConstants.RxTransmissionData_Fld_IsDelivery, System.Data.DbType.Boolean);
            sqlParams[7] = DataFactory.CreateParameter("@" + clsPOSDBConstants.RxTransmissionData_Fld_IsRxSync, System.Data.DbType.Boolean);
            sqlParams[8] = DataFactory.CreateParameter("@" + clsPOSDBConstants.RxTransmissionData_Fld_TransAmount, System.Data.DbType.Decimal);
            sqlParams[9] = DataFactory.CreateParameter("@" + clsPOSDBConstants.RxTransmissionData_Fld_StationID, System.Data.DbType.Int32);
            sqlParams[10] = DataFactory.CreateParameter("@" + clsPOSDBConstants.RxTransmissionData_Fld_UserID, System.Data.DbType.String);
            sqlParams[11] = DataFactory.CreateParameter("@" + clsPOSDBConstants.RxTransmissionData_Fld_TransDateTime, System.Data.DbType.DateTime);
            sqlParams[12] = DataFactory.CreateParameter("@" + clsPOSDBConstants.RxTransmissionData_Fld_ConsentTextID, System.Data.DbType.Int32);
            sqlParams[13] = DataFactory.CreateParameter("@" + clsPOSDBConstants.RxTransmissionData_Fld_ConsentTypeID, System.Data.DbType.Int32);
            sqlParams[14] = DataFactory.CreateParameter("@" + clsPOSDBConstants.RxTransmissionData_Fld_ConsentStatusID, System.Data.DbType.Int32);
            sqlParams[15] = DataFactory.CreateParameter("@" + clsPOSDBConstants.RxTransmissionData_Fld_ConsentCaptureDate, System.Data.DbType.DateTime);
            sqlParams[16] = DataFactory.CreateParameter("@" + clsPOSDBConstants.RxTransmissionData_Fld_ConsentEffectiveDate, System.Data.DbType.DateTime);
            sqlParams[17] = DataFactory.CreateParameter("@" + clsPOSDBConstants.RxTransmissionData_Fld_ConsentEndDate, System.Data.DbType.DateTime);
            sqlParams[18] = DataFactory.CreateParameter("@" + clsPOSDBConstants.RxTransmissionData_Fld_PatConsentRelationID, System.Data.DbType.Int32);
            sqlParams[19] = DataFactory.CreateParameter("@" + clsPOSDBConstants.RxTransmissionData_Fld_SigneeName, System.Data.DbType.String);
            sqlParams[20] = DataFactory.CreateParameter("@" + clsPOSDBConstants.RxTransmissionData_Fld_SignatureData, System.Data.DbType.Byte);
            sqlParams[21] = DataFactory.CreateParameter("@" + clsPOSDBConstants.RxTransmissionData_Fld_PickUpDate, System.Data.DbType.DateTime);
            sqlParams[22] = DataFactory.CreateParameter("@" + clsPOSDBConstants.RxTransmissionData_Fld_CopayPaid, System.Data.DbType.Boolean);
            sqlParams[23] = DataFactory.CreateParameter("@" + clsPOSDBConstants.RxTransmissionData_Fld_CreatedDate, System.Data.DbType.DateTime);

            sqlParams[24] = DataFactory.CreateParameter("@" + clsPOSDBConstants.RxTransmissionData_Fld_PackDATESIGNED, System.Data.DbType.String);
            sqlParams[25] = DataFactory.CreateParameter("@" + clsPOSDBConstants.RxTransmissionData_Fld_PackPATACCEPT, System.Data.DbType.String);
            sqlParams[26] = DataFactory.CreateParameter("@" + clsPOSDBConstants.RxTransmissionData_Fld_PackPRIVACYTEXT, System.Data.DbType.String);
            sqlParams[27] = DataFactory.CreateParameter("@" + clsPOSDBConstants.RxTransmissionData_Fld_PackPRIVACYSIG, System.Data.DbType.String);
            sqlParams[28] = DataFactory.CreateParameter("@" + clsPOSDBConstants.RxTransmissionData_Fld_PackSIGTYPE, System.Data.DbType.String);
            sqlParams[29] = DataFactory.CreateParameter("@" + clsPOSDBConstants.RxTransmissionData_Fld_PackBinarySign, System.Data.DbType.Byte);
            sqlParams[30] = DataFactory.CreateParameter("@" + clsPOSDBConstants.RxTransmissionData_Fld_PackEventType, System.Data.DbType.String);
            sqlParams[31] = DataFactory.CreateParameter("@" + clsPOSDBConstants.RxTransmissionData_Fld_DeliveryStatus, System.Data.DbType.String);
            sqlParams[32] = DataFactory.CreateParameter("@" + clsPOSDBConstants.RxTransmissionData_Fld_IsConsentSkip, System.Data.DbType.Boolean);//PRIMEPOS-2866
            sqlParams[33] = DataFactory.CreateParameter("@" + clsPOSDBConstants.RxTransmissionData_Fld_PartialFillNo, System.Data.DbType.Int16);//PRIMEPOS-2982

            //sqlParams[0].SourceColumn = clsPOSDBConstants.RxTransmissionData_Fld_RxTransNo;
            sqlParams[0].SourceColumn = clsPOSDBConstants.RxTransmissionData_Fld_PatientID;
            sqlParams[1].SourceColumn = clsPOSDBConstants.RxTransmissionData_Fld_RxNo;
            sqlParams[2].SourceColumn = clsPOSDBConstants.RxTransmissionData_Fld_Nrefill;
            sqlParams[3].SourceColumn = clsPOSDBConstants.RxTransmissionData_Fld_PickedUp;
            sqlParams[4].SourceColumn = clsPOSDBConstants.RxTransmissionData_Fld_PickUpPOS;
            sqlParams[5].SourceColumn = clsPOSDBConstants.RxTransmissionData_Fld_TransID;
            sqlParams[6].SourceColumn = clsPOSDBConstants.RxTransmissionData_Fld_IsDelivery;
            sqlParams[7].SourceColumn = clsPOSDBConstants.RxTransmissionData_Fld_IsRxSync;
            sqlParams[8].SourceColumn = clsPOSDBConstants.RxTransmissionData_Fld_TransAmount;
            sqlParams[9].SourceColumn = clsPOSDBConstants.RxTransmissionData_Fld_StationID;
            sqlParams[10].SourceColumn = clsPOSDBConstants.RxTransmissionData_Fld_UserID;
            sqlParams[11].SourceColumn = clsPOSDBConstants.RxTransmissionData_Fld_TransDateTime;
            sqlParams[12].SourceColumn = clsPOSDBConstants.RxTransmissionData_Fld_ConsentTextID;
            sqlParams[13].SourceColumn = clsPOSDBConstants.RxTransmissionData_Fld_ConsentTypeID;
            sqlParams[14].SourceColumn = clsPOSDBConstants.RxTransmissionData_Fld_ConsentStatusID;
            sqlParams[15].SourceColumn = clsPOSDBConstants.RxTransmissionData_Fld_ConsentCaptureDate;
            sqlParams[16].SourceColumn = clsPOSDBConstants.RxTransmissionData_Fld_ConsentEffectiveDate;
            sqlParams[17].SourceColumn = clsPOSDBConstants.RxTransmissionData_Fld_ConsentEndDate;
            sqlParams[18].SourceColumn = clsPOSDBConstants.RxTransmissionData_Fld_PatConsentRelationID;
            sqlParams[19].SourceColumn = clsPOSDBConstants.RxTransmissionData_Fld_SigneeName;
            sqlParams[20].SourceColumn = clsPOSDBConstants.RxTransmissionData_Fld_SignatureData;
            sqlParams[21].SourceColumn = clsPOSDBConstants.RxTransmissionData_Fld_PickUpDate;
            sqlParams[22].SourceColumn = clsPOSDBConstants.RxTransmissionData_Fld_CopayPaid;
            sqlParams[23].SourceColumn = clsPOSDBConstants.RxTransmissionData_Fld_CreatedDate;
            sqlParams[24].SourceColumn = clsPOSDBConstants.RxTransmissionData_Fld_PackDATESIGNED;
            sqlParams[25].SourceColumn = clsPOSDBConstants.RxTransmissionData_Fld_PackPATACCEPT;
            sqlParams[26].SourceColumn = clsPOSDBConstants.RxTransmissionData_Fld_PackPRIVACYTEXT;
            sqlParams[27].SourceColumn = clsPOSDBConstants.RxTransmissionData_Fld_PackPRIVACYSIG;
            sqlParams[28].SourceColumn = clsPOSDBConstants.RxTransmissionData_Fld_PackSIGTYPE;
            sqlParams[29].SourceColumn = clsPOSDBConstants.RxTransmissionData_Fld_PackBinarySign;
            sqlParams[30].SourceColumn = clsPOSDBConstants.RxTransmissionData_Fld_PackEventType;
            sqlParams[31].SourceColumn = clsPOSDBConstants.RxTransmissionData_Fld_DeliveryStatus;
            sqlParams[32].SourceColumn = clsPOSDBConstants.RxTransmissionData_Fld_IsConsentSkip; // PRIMEPOS-2866
            sqlParams[33].SourceColumn = clsPOSDBConstants.RxTransmissionData_Fld_PartialFillNo; // PRIMEPOS-2982

            //if (row.RxTransNo.ToString() != System.String.Empty)
            //{
            //    sqlParams[0].Value = row.RxTransNo;
            //}
            //else
            //{
            //    sqlParams[0].Value = DBNull.Value;
            //}

            if (row.PatientID.ToString() != System.String.Empty)
            {
                sqlParams[0].Value = row.PatientID;
            }
            else
            {
                sqlParams[0].Value = DBNull.Value;
            }

            if (row.RxNo.ToString() != System.String.Empty)
            {
                sqlParams[1].Value = row.RxNo;
            }
            else
            {
                sqlParams[1].Value = DBNull.Value;
            }

            if (row.Nrefill.ToString() != System.String.Empty)
            {
                sqlParams[2].Value = row.Nrefill;
            }
            else
            {
                sqlParams[2].Value = DBNull.Value;
            }

            if (row.PickedUp.ToString() != System.String.Empty)
            {
                sqlParams[3].Value = row.PickedUp;
            }
            else
            {
                sqlParams[3].Value = DBNull.Value;
            }

            if (row.PickUpPOS.ToString() != System.String.Empty)
            {
                sqlParams[4].Value = row.PickUpPOS;
            }
            else
            {
                sqlParams[4].Value = 0;
            }

            if (row.TransID.ToString() != System.String.Empty)
            {
                sqlParams[5].Value = row.TransID;
            }
            else
            {
                sqlParams[5].Value = 0;
            }
            if (row.IsDelivery.ToString() != System.String.Empty)
            {
                sqlParams[6].Value = row.IsDelivery;
            }
            else
            {
                sqlParams[6].Value = 0;
            }
            if (row.IsRxSync.ToString() != System.String.Empty)
            {
                sqlParams[7].Value = row.IsRxSync;
            }
            else
            {
                sqlParams[7].Value = 0;
            }
            if (row.TransAmount.ToString() != System.String.Empty)
            {
                sqlParams[8].Value = row.TransAmount;
            }
            else
            {
                sqlParams[8].Value = 0;
            }
            if (row.StationID.ToString() != System.String.Empty)
            {
                sqlParams[9].Value = row.StationID;
            }
            else
            {
                sqlParams[9].Value = 0;
            }

            if (row.UserID.ToString() != System.String.Empty)
            {
                sqlParams[10].Value = row.UserID;
            }
            else
            {
                sqlParams[10].Value = 0;
            }


            if (row.TransDate != System.DateTime.MinValue)
            {
                sqlParams[11].Value = row.TransDate;
            }
            else
            {
                sqlParams[11].Value = SqlDateTime.MinValue;
            }


            if (row.ConsentTextID.ToString() != System.String.Empty)
            {
                sqlParams[12].Value = row.ConsentTextID;
            }
            else
            {
                sqlParams[12].Value = 0;
            }

            if (row.ConsentTypeID.ToString() != System.String.Empty)
            {
                sqlParams[13].Value = row.ConsentTypeID;
            }
            else
            {
                sqlParams[13].Value = 0;
            }

            if (row.ConsentStatusID.ToString() != System.String.Empty)
            {
                sqlParams[14].Value = row.ConsentStatusID;
            }
            else
            {
                sqlParams[14].Value = 0;
            }

            if (row.ConsentCaptureDate != System.DateTime.MinValue)
            {
                sqlParams[15].Value = row.ConsentCaptureDate;
            }
            else
            {
                sqlParams[15].Value = SqlDateTime.MinValue;
            }


            if (row.ConsentEffectiveDate != System.DateTime.MinValue)
            {
                sqlParams[16].Value = row.ConsentEffectiveDate;
            }
            else
            {
                sqlParams[16].Value = SqlDateTime.MinValue;
            }

            if (row.ConsentEndDate != System.DateTime.MinValue)
            {
                sqlParams[17].Value = row.ConsentEndDate;
            }
            else
            {
                sqlParams[17].Value = SqlDateTime.MinValue;
            }

            if (row.PatConsentRelationID.ToString() != System.String.Empty)
            {
                sqlParams[18].Value = row.PatConsentRelationID;
            }
            else
            {
                sqlParams[18].Value = 0;
            }
            if (row.SigneeName.ToString() != System.String.Empty)
            {
                sqlParams[19].Value = row.SigneeName;
            }
            else
            {
                sqlParams[19].Value = "";
            }
            if (row.SignatureData != null)
            {
                sqlParams[20].Value = row.SignatureData;
            }
            else
            {
                sqlParams[20].Value = 0;
            }


            if (row.PickUpDate != System.DateTime.MinValue)
            {
                sqlParams[21].Value = row.PickUpDate;
            }
            else
            {
                sqlParams[21].Value = SqlDateTime.MinValue;
            }


            if (row.CopayPaid.ToString() != System.String.Empty)
            {
                sqlParams[22].Value = row.CopayPaid;
            }
            else
            {
                sqlParams[22].Value = 0;
            }

            sqlParams[23].Value = DateTime.Now;

            if (row.PackDATESIGNED.ToString() != System.String.Empty)
            {
                sqlParams[24].Value = row.PackDATESIGNED;
            }
            else
            {
                sqlParams[24].Value = SqlDateTime.MinValue; ;
            }

            if (row.PackPATACCEPT.ToString() != System.String.Empty)
            {
                sqlParams[25].Value = row.PackPATACCEPT;
            }
            else
            {
                sqlParams[25].Value = "";
            }

            if (row.PackPRIVACYTEXT.ToString() != System.String.Empty)
            {
                sqlParams[26].Value = row.PackPRIVACYTEXT;
            }
            else
            {
                sqlParams[26].Value = "";
            }

            if (row.PackPRIVACYSIG.ToString() != System.String.Empty)
            {
                sqlParams[27].Value = row.PackPRIVACYSIG;
            }
            else
            {
                sqlParams[27].Value = "";
            }

            if (row.PackSIGTYPE.ToString() != System.String.Empty)
            {
                sqlParams[28].Value = row.PackSIGTYPE;
            }
            else
            {
                sqlParams[28].Value = "";
            }

            if (row.PackBinarySign != null)
            {
                sqlParams[29].Value = row.PackBinarySign;
            }
            else
            {
                sqlParams[29].Value = 0;
            }

            if (row.PackEventType.ToString() != System.String.Empty)
            {
                sqlParams[30].Value = row.PackEventType;
            }
            else
            {
                sqlParams[30].Value = "";
            }

            if (row.DeliveryStatus.ToString() != System.String.Empty)
            {
                sqlParams[31].Value = row.DeliveryStatus;
            }
            else
            {
                sqlParams[31].Value = "";
            }
            if (row.IsConsentSkip.ToString() != System.String.Empty)//PRIMEPOS-2866
            {
                sqlParams[32].Value = row.IsConsentSkip;
            }
            else
            {
                sqlParams[32].Value = 0;
            }

            if (row.PartialFillNo.ToString() != System.String.Empty)
            {
                sqlParams[33].Value = row.PartialFillNo;
            }
            else
            {
                sqlParams[33].Value = 0;
            }

            return (sqlParams);
        }

        private IDbDataParameter[] UpdateParameters(RxTransactionDataRow row)
        {
            IDbDataParameter[] sqlParams = DataFactory.CreateParameterArray(33);

            sqlParams[0] = DataFactory.CreateParameter("@" + clsPOSDBConstants.RxTransmissionData_Fld_RxTransNo, System.Data.DbType.Int32);
            sqlParams[1] = DataFactory.CreateParameter("@" + clsPOSDBConstants.RxTransmissionData_Fld_PatientID, System.Data.DbType.Int32);
            sqlParams[2] = DataFactory.CreateParameter("@" + clsPOSDBConstants.RxTransmissionData_Fld_RxNo, System.Data.DbType.Int32);
            sqlParams[3] = DataFactory.CreateParameter("@" + clsPOSDBConstants.RxTransmissionData_Fld_Nrefill, System.Data.DbType.Int32);
            sqlParams[4] = DataFactory.CreateParameter("@" + clsPOSDBConstants.RxTransmissionData_Fld_PickedUp, System.Data.DbType.Int32);
            sqlParams[5] = DataFactory.CreateParameter("@" + clsPOSDBConstants.RxTransmissionData_Fld_PickUpPOS, System.Data.DbType.Int32);
            sqlParams[6] = DataFactory.CreateParameter("@" + clsPOSDBConstants.RxTransmissionData_Fld_TransID, System.Data.DbType.Int32);
            sqlParams[7] = DataFactory.CreateParameter("@" + clsPOSDBConstants.RxTransmissionData_Fld_IsDelivery, System.Data.DbType.Int32);
            sqlParams[8] = DataFactory.CreateParameter("@" + clsPOSDBConstants.RxTransmissionData_Fld_IsRxSync, System.Data.DbType.Int32);
            sqlParams[9] = DataFactory.CreateParameter("@" + clsPOSDBConstants.RxTransmissionData_Fld_TransAmount, System.Data.DbType.Int32);
            sqlParams[10] = DataFactory.CreateParameter("@" + clsPOSDBConstants.RxTransmissionData_Fld_StationID, System.Data.DbType.Int32);
            sqlParams[11] = DataFactory.CreateParameter("@" + clsPOSDBConstants.RxTransmissionData_Fld_UserID, System.Data.DbType.Int32);
            sqlParams[12] = DataFactory.CreateParameter("@" + clsPOSDBConstants.RxTransmissionData_Fld_TransDateTime, System.Data.DbType.Int32);
            sqlParams[13] = DataFactory.CreateParameter("@" + clsPOSDBConstants.RxTransmissionData_Fld_ConsentTextID, System.Data.DbType.Int32);
            sqlParams[14] = DataFactory.CreateParameter("@" + clsPOSDBConstants.RxTransmissionData_Fld_ConsentTypeID, System.Data.DbType.Int32);
            sqlParams[15] = DataFactory.CreateParameter("@" + clsPOSDBConstants.RxTransmissionData_Fld_ConsentStatusID, System.Data.DbType.Int32);
            sqlParams[16] = DataFactory.CreateParameter("@" + clsPOSDBConstants.RxTransmissionData_Fld_ConsentCaptureDate, System.Data.DbType.DateTime);
            sqlParams[17] = DataFactory.CreateParameter("@" + clsPOSDBConstants.RxTransmissionData_Fld_ConsentEffectiveDate, System.Data.DbType.DateTime);
            sqlParams[18] = DataFactory.CreateParameter("@" + clsPOSDBConstants.RxTransmissionData_Fld_ConsentEndDate, System.Data.DbType.DateTime);
            sqlParams[19] = DataFactory.CreateParameter("@" + clsPOSDBConstants.RxTransmissionData_Fld_PatConsentRelationID, System.Data.DbType.Int32);
            sqlParams[20] = DataFactory.CreateParameter("@" + clsPOSDBConstants.RxTransmissionData_Fld_SigneeName, System.Data.DbType.String);
            sqlParams[21] = DataFactory.CreateParameter("@" + clsPOSDBConstants.RxTransmissionData_Fld_SignatureData, System.Data.DbType.Byte);
            sqlParams[22] = DataFactory.CreateParameter("@" + clsPOSDBConstants.RxTransmissionData_Fld_PickUpDate, System.Data.DbType.DateTime);
            sqlParams[23] = DataFactory.CreateParameter("@" + clsPOSDBConstants.RxTransmissionData_Fld_CopayPaid, System.Data.DbType.Boolean);
            sqlParams[24] = DataFactory.CreateParameter("@" + clsPOSDBConstants.RxTransmissionData_Fld_CreatedDate, System.Data.DbType.DateTime);
            sqlParams[25] = DataFactory.CreateParameter("@" + clsPOSDBConstants.RxTransmissionData_Fld_PackDATESIGNED, System.Data.DbType.String);
            sqlParams[26] = DataFactory.CreateParameter("@" + clsPOSDBConstants.RxTransmissionData_Fld_PackPATACCEPT, System.Data.DbType.String);
            sqlParams[27] = DataFactory.CreateParameter("@" + clsPOSDBConstants.RxTransmissionData_Fld_PackPRIVACYTEXT, System.Data.DbType.String);
            sqlParams[28] = DataFactory.CreateParameter("@" + clsPOSDBConstants.RxTransmissionData_Fld_PackPRIVACYSIG, System.Data.DbType.String);
            sqlParams[29] = DataFactory.CreateParameter("@" + clsPOSDBConstants.RxTransmissionData_Fld_PackSIGTYPE, System.Data.DbType.String);
            sqlParams[30] = DataFactory.CreateParameter("@" + clsPOSDBConstants.RxTransmissionData_Fld_PackBinarySign, System.Data.DbType.Byte);
            sqlParams[31] = DataFactory.CreateParameter("@" + clsPOSDBConstants.RxTransmissionData_Fld_PackEventType, System.Data.DbType.String);
            sqlParams[32] = DataFactory.CreateParameter("@" + clsPOSDBConstants.RxTransmissionData_Fld_DeliveryStatus, System.Data.DbType.String);

            sqlParams[0].SourceColumn = clsPOSDBConstants.RxTransmissionData_Fld_RxTransNo;
            sqlParams[1].SourceColumn = clsPOSDBConstants.RxTransmissionData_Fld_PatientID;
            sqlParams[2].SourceColumn = clsPOSDBConstants.RxTransmissionData_Fld_RxNo;
            sqlParams[3].SourceColumn = clsPOSDBConstants.RxTransmissionData_Fld_Nrefill;
            sqlParams[4].SourceColumn = clsPOSDBConstants.RxTransmissionData_Fld_PickedUp;
            sqlParams[5].SourceColumn = clsPOSDBConstants.RxTransmissionData_Fld_PickUpPOS;
            sqlParams[6].SourceColumn = clsPOSDBConstants.RxTransmissionData_Fld_TransID;
            sqlParams[7].SourceColumn = clsPOSDBConstants.RxTransmissionData_Fld_IsDelivery;
            sqlParams[8].SourceColumn = clsPOSDBConstants.RxTransmissionData_Fld_IsRxSync;
            sqlParams[9].SourceColumn = clsPOSDBConstants.RxTransmissionData_Fld_TransAmount;
            sqlParams[10].SourceColumn = clsPOSDBConstants.RxTransmissionData_Fld_StationID;
            sqlParams[11].SourceColumn = clsPOSDBConstants.RxTransmissionData_Fld_UserID;
            sqlParams[12].SourceColumn = clsPOSDBConstants.RxTransmissionData_Fld_TransDateTime;
            sqlParams[13].SourceColumn = clsPOSDBConstants.RxTransmissionData_Fld_ConsentTextID;
            sqlParams[14].SourceColumn = clsPOSDBConstants.RxTransmissionData_Fld_ConsentTypeID;
            sqlParams[15].SourceColumn = clsPOSDBConstants.RxTransmissionData_Fld_ConsentStatusID;
            sqlParams[16].SourceColumn = clsPOSDBConstants.RxTransmissionData_Fld_ConsentCaptureDate;
            sqlParams[17].SourceColumn = clsPOSDBConstants.RxTransmissionData_Fld_ConsentEffectiveDate;
            sqlParams[18].SourceColumn = clsPOSDBConstants.RxTransmissionData_Fld_ConsentEndDate;
            sqlParams[19].SourceColumn = clsPOSDBConstants.RxTransmissionData_Fld_PatConsentRelationID;
            sqlParams[20].SourceColumn = clsPOSDBConstants.RxTransmissionData_Fld_SigneeName;
            sqlParams[21].SourceColumn = clsPOSDBConstants.RxTransmissionData_Fld_SignatureData;
            sqlParams[22].SourceColumn = clsPOSDBConstants.RxTransmissionData_Fld_PickUpDate;
            sqlParams[23].SourceColumn = clsPOSDBConstants.RxTransmissionData_Fld_CopayPaid;
            sqlParams[24].SourceColumn = clsPOSDBConstants.RxTransmissionData_Fld_CreatedDate;

            if (row.RxTransNo.ToString() != System.String.Empty)
            {
                sqlParams[0].Value = row.RxTransNo;
            }
            else
            {
                sqlParams[0].Value = DBNull.Value;
            }

            if (row.PatientID.ToString() != System.String.Empty)
            {
                sqlParams[1].Value = row.PatientID;
            }
            else
            {
                sqlParams[1].Value = DBNull.Value;
            }

            if (row.RxNo.ToString() != System.String.Empty)
            {
                sqlParams[2].Value = row.RxNo;
            }
            else
            {
                sqlParams[2].Value = DBNull.Value;
            }

            if (row.Nrefill.ToString() != System.String.Empty)
            {
                sqlParams[3].Value = row.Nrefill;
            }
            else
            {
                sqlParams[3].Value = DBNull.Value;
            }

            if (row.PickedUp.ToString() != System.String.Empty)
            {
                sqlParams[4].Value = row.PickedUp;
            }
            else
            {
                sqlParams[4].Value = DBNull.Value;
            }

            if (row.PickUpPOS.ToString() != System.String.Empty)
            {
                sqlParams[5].Value = row.PickUpPOS;
            }
            else
            {
                sqlParams[5].Value = 0;
            }

            if (row.TransID.ToString() != System.String.Empty)
            {
                sqlParams[6].Value = row.TransID;
            }
            else
            {
                sqlParams[6].Value = 0;
            }
            if (row.IsDelivery.ToString() != System.String.Empty)
            {
                sqlParams[7].Value = row.IsDelivery;
            }
            else
            {
                sqlParams[7].Value = 0;
            }
            if (row.IsRxSync.ToString() != System.String.Empty)
            {
                sqlParams[8].Value = row.IsRxSync;
            }
            else
            {
                sqlParams[8].Value = 0;
            }
            if (row.TransAmount.ToString() != System.String.Empty)
            {
                sqlParams[9].Value = row.TransAmount;
            }
            else
            {
                sqlParams[9].Value = 0;
            }
            if (row.UserID.ToString() != System.String.Empty)
            {
                sqlParams[10].Value = row.UserID;
            }
            else
            {
                sqlParams[10].Value = 0;
            }
            if (row.StationID.ToString() != System.String.Empty)
            {
                sqlParams[11].Value = row.StationID;
            }
            else
            {
                sqlParams[11].Value = 0;
            }
            if (row.TransDate != System.DateTime.MinValue)
                sqlParams[12].Value = row.TransDate;
            else
                sqlParams[12].Value = System.DateTime.MinValue;


            sqlParams[24].Value = DateTime.Now;
            return (sqlParams);
        }

        #endregion

        #region Get Methods

        public static bool isCallofRetTrans = false;

        public DataSet Populate(System.Int32 TransId, IDbConnection conn)
        {
            string sSQL = "";
            try
            {
                sSQL = "Select " +
                        " RxTransNo       " +
                        " , TransDateTime " +
                        " , PatientID     " +
                        " , RxNo          " +
                        " , Nrefill       " +
                        " , PickedUp      " +
                        " , TransID       " +
                        " , IsDelivery    " +
                        " , IsRxSync      " +
                        " , StationID     " +
                        " , UserID        " +
                        " , CreatedDate   " +
                        " , ModifiedBy    " +
                        " , ModifiedDate  " +
                        " , PickUpPOS     " +
                        " , ConsentTextID        " +
                        " , ConsentTypeID        " +
                        " , ConsentStatusID      " +
                        " , ConsentCaptureDate   " +
                        " , ConsentEffectiveDate " +
                        " , ConsentEndDate       " +
                        " , RelationID           " +
                        " , SigneeName           " +
                        " , SignatureData        " +
                        " , PickUpdate           " +
                        " , CopayPaid            " +
                        " , PackDATESIGNED   " +
                        " , PackPATACCEPT    " +
                        " , PackPRIVACYTEXT  " +
                        " , PackPRIVACYSIG   " +
                        " , PackSIGTYPE      " +
                        " , PackBinarySign   " +
                        " , PackEventType    " +
                        " , DeliveryStatus   " +
                        " , 1 ConsentSourceID " +//PRIMEPOS-2866,PRIMEPOS-2871
                         " , IsConsentSkip" + //PRIMEPOS-2866
                        " , PartialFillNo" + //PRIMEPOS-2982
                    " FROM "
                  + clsPOSDBConstants.RxTransactionData_tbl + " where TransID = " + TransId + "UNION ALL Select  (ROW_NUMBER() over (order by RXno) * -1)  RxTransNo  , ConsentCaptureDate TransDateTime, PatientNo, RxNo, Nrefill, '' PickedUp, 0 TransID, '' IsDelivery, RxSync IsRxSync, 0 StationID, '' UserID, '' CreatedDate, '' ModifiedBy, '' ModifiedDate, '' PickUpPOS, ConsentTextID, ConsentTypeID, ConsentStatusID, ConsentCaptureDate, ConsentCaptureDate as ConsentEffectiveDate, ConsentExpiryDate ConsentEndDate, ConsentRelationId RelationID, SigneeName, SignatureData, '' PickUpdate, 'true' CopayPaid,'' PackDATESIGNED , '' PackPATACCEPT  , '' PackPRIVACYTEXT,'' PackPRIVACYSIG ,'' PackSIGTYPE ,null PackBinarySign , '' PackEventType  , '' DeliveryStatus,ConsentSourceID, IsConsentSkip, PartialFillNo FROM ConsentTransmissionLog Where ISNULL(RxSync,0) = 0 AND ConsentSourceID != 1 ";//PRIMEPOS-2866,PRIMEPOS-2871

                RxTransactionData ds = new RxTransactionData();
                ds.RxTransaction.MergeTable(DataHelper.ExecuteDataset(conn, CommandType.Text, sSQL, PKParameters(TransId)).Tables[0]);
                return ds;
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Populate(System.Int32 TransId, IDbConnection conn)");
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

        public DataTable GetRxTransDetail(DateTime dtFrom, DateTime dtTo)
        {
            IDbConnection conn = DataFactory.CreateConnection();
            conn.ConnectionString = POS_Core.Resources.Configuration.ConnectionString;
            return (GetRxTransDetail(dtFrom, dtTo, conn));
        }

        private DataTable GetRxTransDetail(DateTime dtFrom, DateTime dtTo, IDbConnection conn)
        {
            try
            {
                string sSQL = "Select " +
                        " RxTransNo       " +
                        " , TransDateTime " +
                        " , PatientID     " +
                        " , RxNo          " +
                        " , Nrefill       " +
                        " , PickedUp      " +
                        " , TransID       " +
                        " , IsDelivery    " +
                        " , IsRxSync      " +
                        " , TransAmount   " +
                        " , StationID     " +
                        " , UserID        " +
                        " , CreatedDate   " +
                        " , ModifiedBy    " +
                        " , ModifiedDate  " +
                        " , PickUpPOS     " +
                        " , ConsentTextID        " +
                        " , ConsentTypeID        " +
                        " , ConsentStatusID      " +
                        " , ConsentCaptureDate   " +
                        " , ConsentEffectiveDate " +
                        " , ConsentEndDate       " +
                        " , RelationID           " +
                        " , SigneeName           " +
                        " , SignatureData        " +
                        " , PickUpdate           " +
                        " , CopayPaid            " +
                        " , 0 ConsentSourceID " +//PRIMEPOS-2866,PRIMEPOS-2871
                         " , IsConsentSkip" +//PRIMEPOS-2866,PRIMEPOS-2871
                    " FROM " +
                    clsPOSDBConstants.RxTransactionData_tbl + " Where StationID = '" + Configuration.StationID + "' and  IsRxSync = 0 and  Convert(date, TransDateTime, 109) between convert(date, cast('" + dtFrom + "' as datetime) ,113) and convert(date, cast('" + dtTo + "' as datetime) ,113) UNION ALL " +
                    "Select  0 RxTransNo , ConsentCaptureDate TransDateTime		, PatientNo, RxNo, Nrefill, '' PickedUp, 0 TransID, '' IsDelivery, RxSync IsRxSync, 0 TransAmount, 0 StationID, '' UserID, '' CreatedDate, '' ModifiedBy, '' ModifiedDate, '' PickUpPOS, ConsentTextID, ConsentTypeID, ConsentStatusID, ConsentCaptureDate, '' ConsentEffectiveDate, ConsentExpiryDate ConsentEndDate, ConsentRelationId RelationID, SigneeName, SignatureData, '' PickUpdate, 0 CopayPaid,ConsentSourceID, IsConsentSkip  FROM ConsentTransmissionLog Where ISNULL(RxSync,0) = 0 AND ConsentSourceID != 1 and  Convert(date, ConsentCaptureDate, 109) between convert(date, cast('" + dtFrom + "' as datetime) ,113) and convert(date, cast('" + dtTo + "' as datetime) ,113)";//PRIMEPOS-2866,PRIMEPOS-2871

                DataSet ds = DataHelper.ExecuteDataset(conn, CommandType.Text, sSQL);
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    return ds.Tables[0];
                else
                    return ds.Tables[0];
            }
            catch (Exception ex)
            {
                logger.Error(ex, "GetRxTransDetail(DateTime dtFrom, DateTime dtTo, IDbConnection conn)");
                ErrorHandler.throwException(ex, "", "");
                return null;
            }
        }
        #endregion //Get Method

        #region Commented
        //public bool UpdateRXSync(int TransID, string UserName, IDbTransaction oTrans)
        //{
        //    bool IsSyncSuccess = false;
        //    try
        //    {
        //        string sSQL = "UPDATE RXTransmission_Log SET IsRxSync = 1 , ModifiedBy = '" + UserName + "' , ModifiedDate = convert(datetime,cast('" + DateTime.Now + "' as datetime),113)  where TransID=" + TransID;
        //        IDbCommand cmd = DataFactory.CreateCommand();
        //        cmd.CommandType = CommandType.Text;
        //        cmd.CommandText = sSQL;
        //        cmd.Transaction = oTrans;
        //        cmd.Connection = oTrans.Connection;
        //        object result;
        //        result = cmd.ExecuteNonQuery();

        //        InsertRxTransmissionHistory(oTrans);
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
        //private void InsertRxTransmissionHistory(IDbTransaction oTrans)
        //{
        //    try
        //    {
        //        IDbConnection conn = DataFactory.CreateConnection();
        //        conn.ConnectionString = POS_Core.Resources.Configuration.ConnectionString;
        //        string sSQL = "Insert Into RXTransmission_History ( RxTransNo , TransDateTime,PatientID, RxNo,Nrefill,PickedUp,TransID,IsDelivery,IsRxSync,TransAmount, StationID" +
        //                 " , UserID, CreatedDate  , ModifiedBy , ModifiedDate, PickUpPOS, ConsentTextID  , ConsentTypeID , ConsentStatusID, ConsentCaptureDate , ConsentEffectiveDate " +
        //                 " , ConsentEndDate , RelationID, SigneeName, SignatureData, PickUpdate , CopayPaid, ConsentSourceId ) ";//PRIMEPOS-2866,PRIMEPOS-2871
        //        sSQL += " Select RxTransNo , TransDateTime,PatientID, RxNo,Nrefill,PickedUp,TransID,IsDelivery,IsRxSync,TransAmount, StationID" +
        //               " , UserID, CreatedDate  , ModifiedBy , ModifiedDate, PickUpPOS, ConsentTextID  , ConsentTypeID , ConsentStatusID, ConsentCaptureDate , ConsentEffectiveDate " +
        //               " , ConsentEndDate , RelationID, SigneeName, SignatureData, PickUpdate , CopayPaid, 1 " +//PRIMEPOS-2866,PRIMEPOS-2871
        //           " FROM " + clsPOSDBConstants.RxTransactionData_tbl + " Where StationID = '" + Configuration.StationID + "' and  IsRxSync = 1";

        //        IDbCommand cmd = DataFactory.CreateCommand();
        //        cmd.CommandType = CommandType.Text;
        //        cmd.CommandText = sSQL;
        //        cmd.Transaction = oTrans;
        //        cmd.Connection = oTrans.Connection;
        //        object result;
        //        result = cmd.ExecuteNonQuery();


        //        DeleteRXSync(oTrans);

        //        //DataSet ds = DataHelper.ExecuteDataset(conn, CommandType.Text, sSQL);
        //    }
        //    catch (Exception ex)
        //    {
        //        logger.Error(ex, "InsertRxTransmissionHistory()");
        //        oTrans.Rollback();
        //        ErrorHandler.throwException(ex, "", "");
        //    }
        //}
        //public bool DeleteRXSync(IDbTransaction oTrans)
        //{
        //    bool IsDeleted = false;
        //    try
        //    {
        //        string sSQL = "DELETE RXTransmission_Log Where StationID = '" + Configuration.StationID + "' and  IsRxSync = 1";
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
        //        logger.Error(ex.Message, "DeleteRXSync(IDbTransaction oTrans)");
        //        throw ex;

        //    }
        //    return IsDeleted;
        //}
        #endregion

        #region PRIMEPOS-2976 15-Jun-2021 JY modified only connection and not logic
        public bool UpdateRXSync(int TransID)
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
                        string sSQL = "UPDATE RXTransmission_Log SET IsRxSync = 1 , ModifiedBy = '" + Configuration.UserName + "' , ModifiedDate = convert(datetime,cast('" + DateTime.Now + "' as datetime),113) where TransID = " + TransID;
                        DataHelper.ExecuteNonQuery(oTrans, CommandType.Text, sSQL);
                        InsertRxTransmissionHistory(oTrans);
                        oTrans.Commit();
                        IsSyncSuccess = true;
                    }
                    catch (Exception Ex1)
                    {
                        logger.Error(Ex1.Message, "Inner loop - UpdateRXSync(int TransID)");
                        if (oTrans != null)
                            oTrans.Rollback();
                        ErrorHandler.throwException(Ex1, "", "");
                    }
                }
            }
            catch(Exception Ex)
            {
                logger.Error(Ex.Message, "Outer loop - UpdateRXSync(int TransID)");
                ErrorHandler.throwException(Ex, "", "");
            }
            return IsSyncSuccess;
        }

        private void InsertRxTransmissionHistory(IDbTransaction oTrans)
        {
            try
            {
                string sSQL = "Insert Into RXTransmission_History ( RxTransNo , TransDateTime,PatientID, RxNo,Nrefill,PickedUp,TransID,IsDelivery,IsRxSync,TransAmount, StationID" +
                         " , UserID, CreatedDate  , ModifiedBy , ModifiedDate, PickUpPOS, ConsentTextID  , ConsentTypeID , ConsentStatusID, ConsentCaptureDate , ConsentEffectiveDate " +
                         " , ConsentEndDate , RelationID, SigneeName, SignatureData, PickUpdate , CopayPaid, ConsentSourceId, PartialFillNo ) ";//PRIMEPOS-2866,PRIMEPOS-2871
                sSQL += " Select RxTransNo , TransDateTime,PatientID, RxNo,Nrefill,PickedUp,TransID,IsDelivery,IsRxSync,TransAmount, StationID" +
                       " , UserID, CreatedDate  , ModifiedBy , ModifiedDate, PickUpPOS, ConsentTextID  , ConsentTypeID , ConsentStatusID, ConsentCaptureDate , ConsentEffectiveDate " +
                       " , ConsentEndDate , RelationID, SigneeName, SignatureData, PickUpdate , CopayPaid, 1, PartialFillNo " +//PRIMEPOS-2866,PRIMEPOS-2871
                   " FROM " + clsPOSDBConstants.RxTransactionData_tbl + " Where StationID = '" + Configuration.StationID + "' and  IsRxSync = 1";

                DataHelper.ExecuteNonQuery(oTrans, CommandType.Text, sSQL);
                DeleteRXSync(oTrans);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "InsertRxTransmissionHistory(IDbTransaction oTrans)");
                if (oTrans != null)
                    oTrans.Rollback();
                ErrorHandler.throwException(ex, "", "");
            }
        }

        public bool DeleteRXSync(IDbTransaction oTrans)
        {
            bool IsDeleted = false;
            try
            {
                string sSQL = "DELETE RXTransmission_Log Where StationID = '" + Configuration.StationID + "' and  IsRxSync = 1";
                DataHelper.ExecuteNonQuery(oTrans, CommandType.Text, sSQL);
                IsDeleted = true;
            }
            catch (Exception ex)
            {
                if (oTrans != null)
                    oTrans.Rollback();
                logger.Error(ex, "DeleteRXSync(IDbTransaction oTrans)");
            }
            return IsDeleted;
        }
        #endregion

        public void Dispose() { }
    }
}
