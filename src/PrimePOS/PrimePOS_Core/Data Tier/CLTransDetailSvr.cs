//----------------------------------------------------------------------------------------------------
//Sprint-18 - 2090 07-Oct-2014 JY Added data access class for CL_TransDetail table
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
    
        
    class CLTransDetailSvr : IDisposable
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
               ErrorHandler.throwException(ex, "", "");
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
                if (conn.State == ConnectionState.Open) //13-Apr-2015 JY Added to avoid timeout issue due to max pool size reached.
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
        public CLTransDetailData Populate(System.Int64 ID)
        {
            using (IDbConnection conn = DataFactory.CreateConnection(POS_Core.Resources.Configuration.ConnectionString))
            {
                return (Populate(ID, conn));
            }
        }

        public CLTransDetailData Populate(System.Int64 ID, IDbConnection conn)
        {
            try
            {
                string sSQL = "Select * "
                                + " FROM "
                                    + clsPOSDBConstants.CLTransDetail_tbl
                                + " WHERE " + clsPOSDBConstants.CLTransDetail_Fld_ID + " = " + ID;

                CLTransDetailData ds = new CLTransDetailData();
                ds.CLTransDetail.MergeTable(DataHelper.ExecuteDataset(conn, CommandType.Text
                                            , sSQL
                                            , PKParameters(ID)).Tables[0]);
                return ds;
            }
            catch (POSExceptions ex)
            {
                logger.Fatal(ex, "CLTransDetailData Populate(System.Int64 ID, IDbConnection conn)");
                throw (ex);
            }
            catch (OtherExceptions ex)
            {
                logger.Fatal(ex, "CLTransDetailData Populate(System.Int64 ID, IDbConnection conn)");
                throw (ex);
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "CLTransDetailData Populate(System.Int64 ID, IDbConnection conn)");
                ErrorHandler.throwException(ex, "", "");
                return null;
            }
        }
        #endregion //Get Method

        #region Insert, Update, and Delete Methods
        public void Insert(DataSet ds, IDbTransaction tx)
        {

            CLTransDetailTable addedTable = (CLTransDetailTable)ds.Tables[0].GetChanges(DataRowState.Added);
            string sSQL;
            IDbDataParameter[] insParam;

            if (addedTable != null && addedTable.Rows.Count > 0)
            {
                foreach (CLTransDetailRow row in addedTable.Rows)
                {
                    try
                    {

                        insParam = InsertParameters(row);
                        sSQL = BuildInsertSQL(clsPOSDBConstants.CLTransDetail_tbl, insParam);

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
                string sDeleteSQL = "DELETE FROM " + clsPOSDBConstants.CLTransDetail_tbl + " WHERE CardID = " + CurrentID;
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
            sqlParams[0].DbType = System.Data.DbType.Int64;
            sqlParams[0].Value = ID;

            return (sqlParams);
        }

        private IDbDataParameter[] PKParameters(CLTransDetailRow row)
        {
            //return a SqlParameterCollection
            IDbDataParameter[] sqlParams = DataFactory.CreateParameterArray(1);

            sqlParams[0] = DataFactory.CreateParameter();
            sqlParams[0].ParameterName = "@ID";
            sqlParams[0].DbType = System.Data.DbType.Int32;

            sqlParams[0].Value = row.ID;
            sqlParams[0].SourceColumn = clsPOSDBConstants.CLTransDetail_Fld_ID;

            return (sqlParams);
        }

        private IDbDataParameter[] InsertParameters(CLTransDetailRow row)
        {
            IDbDataParameter[] sqlParams = DataFactory.CreateParameterArray(7);

            sqlParams[0] = DataFactory.CreateParameter("@" + clsPOSDBConstants.CLTransDetail_Fld_TransID, System.Data.DbType.Int32);
            sqlParams[1] = DataFactory.CreateParameter("@" + clsPOSDBConstants.CLTransDetail_Fld_CardID, System.Data.DbType.Int64);
            sqlParams[2] = DataFactory.CreateParameter("@" + clsPOSDBConstants.CLTransDetail_Fld_CurrentPoints, System.Data.DbType.Decimal);
            sqlParams[3] = DataFactory.CreateParameter("@" + clsPOSDBConstants.CLTransDetail_Fld_PointsAcquired, System.Data.DbType.Decimal);
            sqlParams[4] = DataFactory.CreateParameter("@" + clsPOSDBConstants.CLTransDetail_Fld_RunningTotalPoints, System.Data.DbType.Decimal);
            sqlParams[5] = DataFactory.CreateParameter("@" + clsPOSDBConstants.CLTransDetail_Fld_ActionType, System.Data.DbType.String);
            sqlParams[6] = DataFactory.CreateParameter("@" + clsPOSDBConstants.CLTransDetail_Fld_TransDate, System.Data.DbType.DateTime);

            sqlParams[0].SourceColumn = clsPOSDBConstants.CLTransDetail_Fld_TransID;
            sqlParams[1].SourceColumn = clsPOSDBConstants.CLTransDetail_Fld_CardID;
            sqlParams[2].SourceColumn = clsPOSDBConstants.CLTransDetail_Fld_CurrentPoints;
            sqlParams[3].SourceColumn = clsPOSDBConstants.CLTransDetail_Fld_PointsAcquired;
            sqlParams[4].SourceColumn = clsPOSDBConstants.CLTransDetail_Fld_RunningTotalPoints;
            sqlParams[5].SourceColumn = clsPOSDBConstants.CLTransDetail_Fld_ActionType;
            sqlParams[6].SourceColumn = clsPOSDBConstants.CLTransDetail_Fld_TransDate;
            
            sqlParams[0].Value = row.TransID;
            sqlParams[1].Value = row.CardID;
            sqlParams[2].Value = row.CurrentPoints;
            sqlParams[3].Value = row.PointsAcquired;
            sqlParams[4].Value = row.RunningTotalPoints;
            sqlParams[5].Value = row.ActionType;
            sqlParams[6].Value = row.TransDate;
            return (sqlParams);
        }
        #endregion

        public void Dispose() { }
    }
}
