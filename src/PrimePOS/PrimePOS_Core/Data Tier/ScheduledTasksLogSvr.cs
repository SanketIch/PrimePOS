using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
using POS_Core.Resources;

namespace POS_Core.DataAccess
{
    public class ScheduledTasksLogSvr : IDisposable
    {
        private static ILogger logger = LogManager.GetCurrentClassLogger();

        #region Persist Methods
        public System.Int32 Persist(DataSet updates, IDbTransaction tx)
        {
            System.Int32 ScheduledTasksLogID = 0;
            try
            {
                ScheduledTasksLogID = this.Insert(updates, tx);
                this.Update(updates, tx);
                updates.AcceptChanges();
            }
            catch (POSExceptions ex)
            {
                logger.Fatal(ex, "Persist(DataSet updates, SqlTransaction tx)");
                throw (ex);
            }
            catch (OtherExceptions ex)
            {
                logger.Fatal(ex, "Persist(DataSet updates, SqlTransaction tx)");
                throw (ex);
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "Persist(DataSet updates, SqlTransaction tx)");
                //ErrorLogging.ErrorHandler.throwException(ex, "", "");
            }
            return ScheduledTasksLogID;
        }
        #endregion

        #region insert
        public System.Int32 Insert(DataSet ds, IDbTransaction tx)
        {
            ScheduledTasksLogTable addedTable = (ScheduledTasksLogTable)ds.Tables[clsPOSDBConstants.ScheduledTasksLog_tbl].GetChanges(DataRowState.Added);
            string sSQL;
            IDbDataParameter[] insParam;
            System.Int32 ScheduledTasksLogID = 0;

            if (addedTable != null && addedTable.Rows.Count > 0)
            {
                foreach (ScheduledTasksLogRow row in addedTable.Rows)
                {
                    try
                    {
                        insParam = InsertParameters(row);
                        sSQL = BuildInsertSQL(clsPOSDBConstants.ScheduledTasksLog_tbl, insParam);
                        int nRowsAffected = DataHelper.ExecuteNonQuery(DBConfig.ConnectionString, CommandType.Text, sSQL, insParam);
                        if (nRowsAffected > 0)
                        {
                            try
                            {
                                ScheduledTasksLogID = Convert.ToInt32(DataHelper.ExecuteScalar(tx, CommandType.Text, "select @@identity"));
                            }
                            catch
                            {
                                ScheduledTasksLogID = Convert.ToInt32(DataHelper.ExecuteScalar(tx, CommandType.Text, "select Ident_Current('" + clsPOSDBConstants.ScheduledTasksLog_tbl + "')"));
                            }
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
                    catch (SqlException ex)
                    {
                        throw (ex);
                    }
                    catch (Exception ex)
                    {
                        logger.Fatal(ex, "Insert(DataSet ds, IDbTransaction tx)");  //PRIMEPOS-2971 07-Jun-2021 JY Added
                        //ErrorHandler.throwException(ex, "", "");
                    }
                }
                addedTable.AcceptChanges();
            }
            return ScheduledTasksLogID;
        }

        private string BuildInsertSQL(string tableName, IDbDataParameter[] delParam)
        {
            string sInsertSQL = "INSERT INTO " + tableName + " (";

            for (int i = 1; i < delParam.Length; i++)
            {
                if (i == 1)
                    sInsertSQL += delParam[i].SourceColumn;
                else
                    sInsertSQL += ", " + delParam[i].SourceColumn;
            }
            sInsertSQL += ") Values (";

            for (int i = 1; i < delParam.Length; i++)
            {
                if (i == 1)
                    sInsertSQL += delParam[i].ParameterName;
                else
                    sInsertSQL += ", " + delParam[i].ParameterName;
            }
            sInsertSQL = sInsertSQL + ")";
            return sInsertSQL;
        }

        private IDbDataParameter[] InsertParameters(ScheduledTasksLogRow row)
        {
            IDbDataParameter[] sqlParams = DataFactory.CreateParameterArray(8);

            sqlParams[0] = new SqlParameter("@" + clsPOSDBConstants.ScheduledTasksLog_Fld_ScheduledTasksLogID, System.Data.SqlDbType.Int);
            sqlParams[1] = new SqlParameter("@" + clsPOSDBConstants.ScheduledTasksLog_Fld_TaskStatus, System.Data.SqlDbType.VarChar);
            sqlParams[2] = new SqlParameter("@" + clsPOSDBConstants.ScheduledTasksLog_Fld_LogDescription, System.Data.SqlDbType.VarChar);
            sqlParams[3] = new SqlParameter("@" + clsPOSDBConstants.ScheduledTasksLog_Fld_StartDate, System.Data.SqlDbType.DateTime);
            sqlParams[4] = new SqlParameter("@" + clsPOSDBConstants.ScheduledTasksLog_Fld_StartTime, System.Data.SqlDbType.DateTime);
            sqlParams[5] = new SqlParameter("@" + clsPOSDBConstants.ScheduledTasksLog_Fld_EndTime, System.Data.SqlDbType.DateTime);
            sqlParams[6] = new SqlParameter("@" + clsPOSDBConstants.ScheduledTasksLog_Fld_ScheduledTasksID, System.Data.SqlDbType.Int);
            sqlParams[7] = new SqlParameter("@" + clsPOSDBConstants.ScheduledTasksLog_Fld_ComputerName, System.Data.SqlDbType.VarChar);

            sqlParams[0].SourceColumn = clsPOSDBConstants.ScheduledTasksLog_Fld_ScheduledTasksLogID;
            sqlParams[1].SourceColumn = clsPOSDBConstants.ScheduledTasksLog_Fld_TaskStatus;
            sqlParams[2].SourceColumn = clsPOSDBConstants.ScheduledTasksLog_Fld_LogDescription;
            sqlParams[3].SourceColumn = clsPOSDBConstants.ScheduledTasksLog_Fld_StartDate;
            sqlParams[4].SourceColumn = clsPOSDBConstants.ScheduledTasksLog_Fld_StartTime;
            sqlParams[5].SourceColumn = clsPOSDBConstants.ScheduledTasksLog_Fld_EndTime;
            sqlParams[6].SourceColumn = clsPOSDBConstants.ScheduledTasksLog_Fld_ScheduledTasksID;
            sqlParams[7].SourceColumn = clsPOSDBConstants.ScheduledTasksLog_Fld_ComputerName;

            if (row.ScheduledTasksLogID != 0)
                sqlParams[0].Value = row.ScheduledTasksLogID;
            else
                sqlParams[0].Value = DBNull.Value;

            if (row.TaskStatus != System.String.Empty)
                sqlParams[1].Value = row.TaskStatus.Replace("'", "''");
            else
                sqlParams[1].Value = DBNull.Value;

            if (row.LogDescription != System.String.Empty)
                sqlParams[2].Value = row.LogDescription.Replace("'", "''");
            else
                sqlParams[2].Value = DBNull.Value;

            if (row.StartDate != DBNull.Value)
                sqlParams[3].Value = row.StartDate;
            else
                sqlParams[3].Value = DBNull.Value;

            if (row.StartTime != DBNull.Value)
                sqlParams[4].Value = row.StartTime;
            else
                sqlParams[4].Value = DBNull.Value;

            if (row.EndTime != DBNull.Value)
                sqlParams[5].Value = row.EndTime;
            else
                sqlParams[5].Value = DBNull.Value;

            if (row.ScheduledTasksID != 0)
                sqlParams[6].Value = row.ScheduledTasksID;
            else
                sqlParams[6].Value = DBNull.Value;

            if (row.ComputerName != System.String.Empty)
                sqlParams[7].Value = row.ComputerName.Replace("'", "''");
            else
                sqlParams[7].Value = DBNull.Value;

            return (sqlParams);
        }
        #endregion

        #region update
        public void Update(DataSet ds, IDbTransaction tx)
        {
            ScheduledTasksLogTable modifiedTable = (ScheduledTasksLogTable)ds.Tables[clsPOSDBConstants.ScheduledTasksLog_tbl].GetChanges(DataRowState.Modified);

            string sSQL;
            IDbDataParameter[] updParam;

            if (modifiedTable != null && modifiedTable.Rows.Count > 0)
            {
                foreach (ScheduledTasksLogRow row in modifiedTable.Rows)
                {
                    try
                    {
                        updParam = UpdateParameters(row);
                        sSQL = BuildUpdateSQL(clsPOSDBConstants.ScheduledTasksLog_tbl, updParam);
                        DataHelper.ExecuteNonQuery(DBConfig.ConnectionString, CommandType.Text, sSQL, updParam);
                    }
                    catch (POSExceptions ex)
                    {
                        throw (ex);
                    }
                    catch (OtherExceptions ex)
                    {
                        throw (ex);
                    }
                    catch (SqlException ex)
                    {
                        throw (ex);
                    }
                    catch (Exception ex)
                    {
                        logger.Fatal(ex, "Update(DataSet ds, IDbTransaction tx)");  //PRIMEPOS-2971 07-Jun-2021 JY Added
                        //ErrorHandler.throwException(ex, "", "");
                    }
                }
                modifiedTable.AcceptChanges();
            }
        }

        private string BuildUpdateSQL(string tableName, IDbDataParameter[] updParam)
        {
            string sUpdateSQL = "UPDATE " + tableName + " SET ";

            for (int i = 1; i < updParam.Length; i++)
            {
                if (i == 1)
                    sUpdateSQL += updParam[i].SourceColumn + " = " + updParam[i].ParameterName;
                else
                    sUpdateSQL += ", " + updParam[i].SourceColumn + " = " + updParam[i].ParameterName;
            }

            sUpdateSQL += " WHERE " + updParam[0].SourceColumn + " = " + updParam[0].ParameterName;
            return sUpdateSQL;
        }

        private IDbDataParameter[] UpdateParameters(ScheduledTasksLogRow row)
        {
            IDbDataParameter[] sqlParams = DataFactory.CreateParameterArray(8);

            sqlParams[0] = new SqlParameter("@" + clsPOSDBConstants.ScheduledTasksLog_Fld_ScheduledTasksLogID, System.Data.SqlDbType.Int);
            sqlParams[1] = new SqlParameter("@" + clsPOSDBConstants.ScheduledTasksLog_Fld_TaskStatus, System.Data.SqlDbType.VarChar);
            sqlParams[2] = new SqlParameter("@" + clsPOSDBConstants.ScheduledTasksLog_Fld_LogDescription, System.Data.SqlDbType.VarChar);
            sqlParams[3] = new SqlParameter("@" + clsPOSDBConstants.ScheduledTasksLog_Fld_StartDate, System.Data.SqlDbType.DateTime);
            sqlParams[4] = new SqlParameter("@" + clsPOSDBConstants.ScheduledTasksLog_Fld_StartTime, System.Data.SqlDbType.DateTime);
            sqlParams[5] = new SqlParameter("@" + clsPOSDBConstants.ScheduledTasksLog_Fld_EndTime, System.Data.SqlDbType.DateTime);
            sqlParams[6] = new SqlParameter("@" + clsPOSDBConstants.ScheduledTasksLog_Fld_ScheduledTasksID, System.Data.SqlDbType.Int);
            sqlParams[7] = new SqlParameter("@" + clsPOSDBConstants.ScheduledTasksLog_Fld_ComputerName, System.Data.SqlDbType.VarChar);

            sqlParams[0].SourceColumn = clsPOSDBConstants.ScheduledTasksLog_Fld_ScheduledTasksLogID;
            sqlParams[1].SourceColumn = clsPOSDBConstants.ScheduledTasksLog_Fld_TaskStatus;
            sqlParams[2].SourceColumn = clsPOSDBConstants.ScheduledTasksLog_Fld_LogDescription;
            sqlParams[3].SourceColumn = clsPOSDBConstants.ScheduledTasksLog_Fld_StartDate;
            sqlParams[4].SourceColumn = clsPOSDBConstants.ScheduledTasksLog_Fld_StartTime;
            sqlParams[5].SourceColumn = clsPOSDBConstants.ScheduledTasksLog_Fld_EndTime;
            sqlParams[6].SourceColumn = clsPOSDBConstants.ScheduledTasksLog_Fld_ScheduledTasksID;
            sqlParams[7].SourceColumn = clsPOSDBConstants.ScheduledTasksLog_Fld_ComputerName;

            if (row.ScheduledTasksLogID != 0)
                sqlParams[0].Value = row.ScheduledTasksLogID;
            else
                sqlParams[0].Value = DBNull.Value;

            if (row.TaskStatus != System.String.Empty)
                sqlParams[1].Value = row.TaskStatus.Replace("'", "''");
            else
                sqlParams[1].Value = DBNull.Value;

            if (row.LogDescription != System.String.Empty)
                sqlParams[2].Value = row.LogDescription.Replace("'", "''");
            else
                sqlParams[2].Value = DBNull.Value;

            if (row.StartDate != DBNull.Value)
                sqlParams[3].Value = row.StartDate;
            else
                sqlParams[3].Value = DBNull.Value;

            if (row.StartTime != DBNull.Value)
                sqlParams[4].Value = row.StartTime;
            else
                sqlParams[4].Value = DBNull.Value;

            if (row.EndTime != DBNull.Value)
                sqlParams[5].Value = row.EndTime;
            else
                sqlParams[5].Value = DBNull.Value;

            if (row.ScheduledTasksID != 0)
                sqlParams[6].Value = row.ScheduledTasksID;
            else
                sqlParams[6].Value = DBNull.Value;

            if (row.ComputerName != System.String.Empty)
                sqlParams[7].Value = row.ComputerName.Replace("'", "''");
            else
                sqlParams[7].Value = DBNull.Value;

            return (sqlParams);
        }
        #endregion

        public DataSet GetScheduledTasksLogList(int TaskId, string sFromDate, string sToDate)
        {
            DataSet ds = new DataSet();
            string sWhere = string.Empty;
            using (SqlConnection conn = new SqlConnection(POS_Core.Resources.Configuration.ConnectionString))
            {
                string sSQL = @"SELECT a.TaskName, b.ScheduledTasksLogID, b.TaskStatus, b.StartDate, b.StartTime, b.EndTime, b.ScheduledTasksID, b.LogDescription FROM ScheduledTasks a
                        INNER JOIN ScheduledTasksLog b ON a.ScheduledTasksID = b.ScheduledTasksID WHERE 1 = 1 ";

                if (TaskId != -1)
                    sWhere = " AND a.TaskId = " + TaskId.ToString();

                if (sFromDate != "" && sToDate != "")
                {
                    sWhere += "AND Convert(datetime,b.StartDate,109) between convert(datetime, cast('" + sFromDate + " 00:00:00' as datetime) ,113) and convert(datetime, cast('" + sToDate + " 23:59:59' as datetime) ,113)";
                }

                sSQL += sWhere + " ORDER BY b.ScheduledTasksLogID Desc";

                ds = DataHelper.ExecuteDataset(conn, CommandType.Text, sSQL);
            }
            return ds;
        }

        public void Dispose() { }
    }
}
