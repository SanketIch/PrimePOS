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
    public class ScheduledTasksSvr : IDisposable
    {
        private static ILogger logger = LogManager.GetCurrentClassLogger();
        public delegate void DataRowSavedHandler();
        public event DataRowSavedHandler DataRowSaved;

        #region Persist Methods
        public System.Int32 Persist(DataSet updates, IDbTransaction tx)
        {
            System.Int32 ScheduledTasksID = 0;
            try
            {
                this.Delete(updates, tx);
                ScheduledTasksID = this.Insert(updates, tx);
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
            return ScheduledTasksID;
        }
        #endregion

        #region insert
        public System.Int32 Insert(DataSet ds, IDbTransaction tx)
        {
            System.Int32 ScheduledTasksID = 0;
            ScheduledTasksTable addedTable = (ScheduledTasksTable)ds.Tables[clsPOSDBConstants.ScheduledTasks_tbl].GetChanges(DataRowState.Added);
            string sSQL;
            IDbDataParameter[] insParam;

            if (addedTable != null && addedTable.Rows.Count > 0)
            {
                foreach (ScheduledTasksRow row in addedTable.Rows)
                {
                    try
                    {
                        insParam = InsertParameters(row);
                        sSQL = BuildInsertSQL(clsPOSDBConstants.ScheduledTasks_tbl, insParam);
                        int nRowsAffected = DataHelper.ExecuteNonQuery(DBConfig.ConnectionString, CommandType.Text, sSQL, insParam);
                        if (nRowsAffected > 0)
                        {
                            ScheduledTasksID = Convert.ToInt32(DataHelper.ExecuteScalar(tx, CommandType.Text, "SELECT IDENT_CURRENT('ScheduledTasks')"));
                        }
                    }
                    catch (POSExceptions ex)
                    {
                        return 0;
                        throw (ex);
                    }
                    catch (OtherExceptions ex)
                    {
                        return 0;
                        throw (ex);
                    }
                    catch (SqlException ex)
                    {
                        return 0;
                        throw (ex);
                    }
                    catch (Exception ex)
                    {
                        logger.Fatal(ex, "Insert(DataSet ds, IDbTransaction tx)");  //PRIMEPOS-2971 07-Jun-2021 JY Added
                        //ErrorHandler.throwException(ex, "", "");
                        return 0;
                    }
                }
                addedTable.AcceptChanges();
            }
            return ScheduledTasksID;
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

        private IDbDataParameter[] InsertParameters(ScheduledTasksRow row)
        {
            IDbDataParameter[] sqlParams = DataFactory.CreateParameterArray(15);

            sqlParams[0] = new SqlParameter("@" + clsPOSDBConstants.ScheduledTasks_Fld_ScheduledTasksID, System.Data.SqlDbType.Int);
            sqlParams[1] = new SqlParameter("@" + clsPOSDBConstants.ScheduledTasks_Fld_TaskName, System.Data.SqlDbType.VarChar);
            sqlParams[2] = new SqlParameter("@" + clsPOSDBConstants.ScheduledTasks_Fld_TaskDescription, System.Data.SqlDbType.VarChar);
            sqlParams[3] = new SqlParameter("@" + clsPOSDBConstants.ScheduledTasks_Fld_PerformTask, System.Data.SqlDbType.Int);
            sqlParams[4] = new SqlParameter("@" + clsPOSDBConstants.ScheduledTasks_Fld_RepeatTask, System.Data.SqlDbType.Int);
            sqlParams[5] = new SqlParameter("@" + clsPOSDBConstants.ScheduledTasks_Fld_StartDate, System.Data.SqlDbType.DateTime);
            sqlParams[6] = new SqlParameter("@" + clsPOSDBConstants.ScheduledTasks_Fld_StartTime, System.Data.SqlDbType.DateTime);
            sqlParams[7] = new SqlParameter("@" + clsPOSDBConstants.ScheduledTasks_Fld_TaskId, System.Data.SqlDbType.Int);
            sqlParams[8] = new SqlParameter("@" + clsPOSDBConstants.ScheduledTasks_Fld_SendEmail, System.Data.SqlDbType.Bit);
            sqlParams[9] = new SqlParameter("@" + clsPOSDBConstants.ScheduledTasks_Fld_EmailAddress, System.Data.SqlDbType.VarChar);
            sqlParams[10] = new SqlParameter("@" + clsPOSDBConstants.ScheduledTasks_Fld_AdvancedSeetings, System.Data.SqlDbType.Bit);
            sqlParams[11] = new SqlParameter("@" + clsPOSDBConstants.ScheduledTasks_Fld_RepeatTaskInterval, System.Data.SqlDbType.BigInt);
            sqlParams[12] = new SqlParameter("@" + clsPOSDBConstants.ScheduledTasks_Fld_Duration, System.Data.SqlDbType.BigInt);
            sqlParams[13] = new SqlParameter("@" + clsPOSDBConstants.ScheduledTasks_Fld_SendPrint, System.Data.SqlDbType.Bit);
            sqlParams[14] = new SqlParameter("@" + clsPOSDBConstants.ScheduledTasks_Fld_Enabled, System.Data.SqlDbType.Bit);

            sqlParams[0].SourceColumn = clsPOSDBConstants.ScheduledTasks_Fld_ScheduledTasksID;
            sqlParams[1].SourceColumn = clsPOSDBConstants.ScheduledTasks_Fld_TaskName;
            sqlParams[2].SourceColumn = clsPOSDBConstants.ScheduledTasks_Fld_TaskDescription;
            sqlParams[3].SourceColumn = clsPOSDBConstants.ScheduledTasks_Fld_PerformTask;
            sqlParams[4].SourceColumn = clsPOSDBConstants.ScheduledTasks_Fld_RepeatTask;
            sqlParams[5].SourceColumn = clsPOSDBConstants.ScheduledTasks_Fld_StartDate;
            sqlParams[6].SourceColumn = clsPOSDBConstants.ScheduledTasks_Fld_StartTime;
            sqlParams[7].SourceColumn = clsPOSDBConstants.ScheduledTasks_Fld_TaskId;
            sqlParams[8].SourceColumn = clsPOSDBConstants.ScheduledTasks_Fld_SendEmail;
            sqlParams[9].SourceColumn = clsPOSDBConstants.ScheduledTasks_Fld_EmailAddress;
            sqlParams[10].SourceColumn = clsPOSDBConstants.ScheduledTasks_Fld_AdvancedSeetings;
            sqlParams[11].SourceColumn = clsPOSDBConstants.ScheduledTasks_Fld_RepeatTaskInterval;
            sqlParams[12].SourceColumn = clsPOSDBConstants.ScheduledTasks_Fld_Duration;
            sqlParams[13].SourceColumn = clsPOSDBConstants.ScheduledTasks_Fld_SendPrint;
            sqlParams[14].SourceColumn = clsPOSDBConstants.ScheduledTasks_Fld_Enabled;

            if (row.ScheduledTasksID != 0)
                sqlParams[0].Value = row.ScheduledTasksID;
            else
                sqlParams[0].Value = DBNull.Value;

            if (row.TaskName != System.String.Empty)
                sqlParams[1].Value = row.TaskName.Replace("'", "''");
            else
                sqlParams[1].Value = DBNull.Value;

            if (row.TaskDescription != System.String.Empty)
                sqlParams[2].Value = row.TaskDescription.Replace("'", "''");
            else
                sqlParams[2].Value = DBNull.Value;

            sqlParams[3].Value = row.PerformTask;
            sqlParams[4].Value = row.RepeatTask;

            if (row.StartDate != DBNull.Value)
                sqlParams[5].Value = row.StartDate;
            else
                sqlParams[5].Value = DBNull.Value;

            if (row.StartTime != DBNull.Value)
                sqlParams[6].Value = row.StartTime;
            else
                sqlParams[6].Value = DBNull.Value;

            sqlParams[7].Value = row.TaskId;
            sqlParams[8].Value = row.SendEmail;

            if (row.EmailAddress != System.String.Empty)
                sqlParams[9].Value = row.EmailAddress.Replace("'", "''");
            else
                sqlParams[9].Value = DBNull.Value;

            sqlParams[10].Value = row.AdvancedSeetings;
            sqlParams[11].Value = row.RepeatTaskInterval;
            sqlParams[12].Value = row.Duration;
            sqlParams[13].Value = row.SendPrint;
            sqlParams[14].Value = row.Enabled;

            return (sqlParams);
        }
        #endregion

        #region update
        public void Update(DataSet ds, IDbTransaction tx)
        {
            ScheduledTasksTable modifiedTable = (ScheduledTasksTable)ds.Tables[clsPOSDBConstants.ScheduledTasks_tbl].GetChanges(DataRowState.Modified);

            string sSQL;
            IDbDataParameter[] updParam;

            if (modifiedTable != null && modifiedTable.Rows.Count > 0)
            {
                foreach (ScheduledTasksRow row in modifiedTable.Rows)
                {
                    try
                    {
                        updParam = UpdateParameters(row);
                        sSQL = BuildUpdateSQL(clsPOSDBConstants.ScheduledTasks_tbl, updParam);
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

        private IDbDataParameter[] UpdateParameters(ScheduledTasksRow row)
        {
            IDbDataParameter[] sqlParams = DataFactory.CreateParameterArray(15);

            sqlParams[0] = new SqlParameter("@" + clsPOSDBConstants.ScheduledTasks_Fld_ScheduledTasksID, System.Data.SqlDbType.Int);
            sqlParams[1] = new SqlParameter("@" + clsPOSDBConstants.ScheduledTasks_Fld_TaskName, System.Data.SqlDbType.VarChar);
            sqlParams[2] = new SqlParameter("@" + clsPOSDBConstants.ScheduledTasks_Fld_TaskDescription, System.Data.SqlDbType.VarChar);
            sqlParams[3] = new SqlParameter("@" + clsPOSDBConstants.ScheduledTasks_Fld_PerformTask, System.Data.SqlDbType.Int);
            sqlParams[4] = new SqlParameter("@" + clsPOSDBConstants.ScheduledTasks_Fld_RepeatTask, System.Data.SqlDbType.Int);
            sqlParams[5] = new SqlParameter("@" + clsPOSDBConstants.ScheduledTasks_Fld_StartDate, System.Data.SqlDbType.DateTime);
            sqlParams[6] = new SqlParameter("@" + clsPOSDBConstants.ScheduledTasks_Fld_StartTime, System.Data.SqlDbType.DateTime);
            sqlParams[7] = new SqlParameter("@" + clsPOSDBConstants.ScheduledTasks_Fld_TaskId, System.Data.SqlDbType.Int);
            sqlParams[8] = new SqlParameter("@" + clsPOSDBConstants.ScheduledTasks_Fld_SendEmail, System.Data.SqlDbType.Bit);
            sqlParams[9] = new SqlParameter("@" + clsPOSDBConstants.ScheduledTasks_Fld_EmailAddress, System.Data.SqlDbType.VarChar);
            sqlParams[10] = new SqlParameter("@" + clsPOSDBConstants.ScheduledTasks_Fld_AdvancedSeetings, System.Data.SqlDbType.Bit);
            sqlParams[11] = new SqlParameter("@" + clsPOSDBConstants.ScheduledTasks_Fld_RepeatTaskInterval, System.Data.SqlDbType.BigInt);
            sqlParams[12] = new SqlParameter("@" + clsPOSDBConstants.ScheduledTasks_Fld_Duration, System.Data.SqlDbType.BigInt);
            sqlParams[13] = new SqlParameter("@" + clsPOSDBConstants.ScheduledTasks_Fld_SendPrint, System.Data.SqlDbType.Bit);
            sqlParams[14] = new SqlParameter("@" + clsPOSDBConstants.ScheduledTasks_Fld_Enabled, System.Data.SqlDbType.Bit);

            sqlParams[0].SourceColumn = clsPOSDBConstants.ScheduledTasks_Fld_ScheduledTasksID;
            sqlParams[1].SourceColumn = clsPOSDBConstants.ScheduledTasks_Fld_TaskName;
            sqlParams[2].SourceColumn = clsPOSDBConstants.ScheduledTasks_Fld_TaskDescription;
            sqlParams[3].SourceColumn = clsPOSDBConstants.ScheduledTasks_Fld_PerformTask;
            sqlParams[4].SourceColumn = clsPOSDBConstants.ScheduledTasks_Fld_RepeatTask;
            sqlParams[5].SourceColumn = clsPOSDBConstants.ScheduledTasks_Fld_StartDate;
            sqlParams[6].SourceColumn = clsPOSDBConstants.ScheduledTasks_Fld_StartTime;
            sqlParams[7].SourceColumn = clsPOSDBConstants.ScheduledTasks_Fld_TaskId;
            sqlParams[8].SourceColumn = clsPOSDBConstants.ScheduledTasks_Fld_SendEmail;
            sqlParams[9].SourceColumn = clsPOSDBConstants.ScheduledTasks_Fld_EmailAddress;
            sqlParams[10].SourceColumn = clsPOSDBConstants.ScheduledTasks_Fld_AdvancedSeetings;
            sqlParams[11].SourceColumn = clsPOSDBConstants.ScheduledTasks_Fld_RepeatTaskInterval;
            sqlParams[12].SourceColumn = clsPOSDBConstants.ScheduledTasks_Fld_Duration;
            sqlParams[13].SourceColumn = clsPOSDBConstants.ScheduledTasks_Fld_SendPrint;
            sqlParams[14].SourceColumn = clsPOSDBConstants.ScheduledTasks_Fld_Enabled;

            if (row.ScheduledTasksID != 0)
                sqlParams[0].Value = row.ScheduledTasksID;
            else
                sqlParams[0].Value = DBNull.Value;

            if (row.TaskName != System.String.Empty)
                sqlParams[1].Value = row.TaskName.Replace("'", "''");
            else
                sqlParams[1].Value = DBNull.Value;

            if (row.TaskDescription != System.String.Empty)
                sqlParams[2].Value = row.TaskDescription.Replace("'", "''");
            else
                sqlParams[2].Value = DBNull.Value;

            sqlParams[3].Value = row.PerformTask;
            sqlParams[4].Value = row.RepeatTask;

            if (row.StartDate != DBNull.Value)
                sqlParams[5].Value = row.StartDate;
            else
                sqlParams[5].Value = DBNull.Value;

            if (row.StartTime != DBNull.Value)
                sqlParams[6].Value = row.StartTime;
            else
                sqlParams[6].Value = DBNull.Value;

            sqlParams[7].Value = row.TaskId;
            sqlParams[8].Value = row.SendEmail;

            if (row.EmailAddress != System.String.Empty)
                sqlParams[9].Value = row.EmailAddress.Replace("'", "''");
            else
                sqlParams[9].Value = DBNull.Value;

            sqlParams[10].Value = row.AdvancedSeetings;
            sqlParams[11].Value = row.RepeatTaskInterval;
            sqlParams[12].Value = row.Duration;
            sqlParams[13].Value = row.SendPrint;
            sqlParams[14].Value = row.Enabled;

            return (sqlParams);
        }
        #endregion

        #region delete
        public void Delete(DataSet ds, IDbTransaction tx)
        {
            ScheduledTasksTable table = (ScheduledTasksTable)ds.Tables[clsPOSDBConstants.ScheduledTasks_tbl].GetChanges(DataRowState.Deleted);
            string sSQL;
            IDbDataParameter[] delParam;

            if (table != null && table.Rows.Count > 0)
            {
                table.RejectChanges();
                foreach (ScheduledTasksRow row in table.Rows)
                {
                    try
                    {
                        delParam = PKParameters(row);

                        sSQL = BuildDeleteSQL(clsPOSDBConstants.ScheduledTasks_tbl, delParam);
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
                        logger.Fatal(ex, "Delete(DataSet ds, IDbTransaction tx)");  //PRIMEPOS-2971 07-Jun-2021 JY Added
                        //ErrorHandler.throwException(ex, "", "");
                    }
                }
            }
        }

        private string BuildDeleteSQL(string tableName, IDbDataParameter[] delParam)
        {
            string sDeleteSQL = "DELETE FROM " + tableName + " WHERE ";
            for (int i = 0; i < delParam.Length; i++)
            {
                sDeleteSQL += delParam[i].SourceColumn + " = " + delParam[i].ParameterName;
            }
            return sDeleteSQL;
        }

        public bool Delete(Int32 ScheduledTasksID)
        {
            try
            {
                //string sSQL = "DELETE FROM ScheduledTasks WHERE ScheduledTasksID = " + ScheduledTasksID;
                string sSQL = "DELETE FROM ScheduledTasks WHERE ScheduledTasksID = " + ScheduledTasksID + ";";
                sSQL += " DELETE FROM ScheduledTasksDetailMonth WHERE ScheduledTasksID = " + ScheduledTasksID + ";";
                sSQL += " DELETE FROM ScheduledTasksDetailWeek WHERE ScheduledTasksID = " + ScheduledTasksID + ";";
                sSQL += " DELETE FROM ScheduledTasksControls WHERE ScheduledTasksID = " + ScheduledTasksID + ";";
                DataHelper.ExecuteNonQuery(DBConfig.ConnectionString, CommandType.Text, sSQL);
                return true;
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "Delete(Int32 ScheduledTasksID)");
                //ErrorHandler.throwException(ex, "", "");
                return false;
            }
        }
        #endregion

        private IDbDataParameter[] PKParameters(ScheduledTasksRow row)
        {
            IDbDataParameter[] sqlParams = DataFactory.CreateParameterArray(1);

            sqlParams[0] = DataFactory.CreateParameter();
            sqlParams[0].ParameterName = "@ScheduledTasksID";
            sqlParams[0].DbType = System.Data.DbType.Int32;
            sqlParams[0].Value = row.ScheduledTasksID;
            sqlParams[0].SourceColumn = clsPOSDBConstants.ScheduledTasks_Fld_ScheduledTasksID;
            return (sqlParams);
        }

        private IDbDataParameter[] PKParameters(System.Int32 ScheduledTasksID)
        {
            IDbDataParameter[] sqlParams = DataFactory.CreateParameterArray(1);

            sqlParams[0] = DataFactory.CreateParameter();
            sqlParams[0].ParameterName = "@ScheduledTasksID";
            sqlParams[0].DbType = System.Data.DbType.Int32;
            sqlParams[0].Value = ScheduledTasksID;
            sqlParams[0].SourceColumn = clsPOSDBConstants.ScheduledTasks_Fld_ScheduledTasksID;
            return (sqlParams);
        }

        public ScheduledTasksData GetScheduledTasksList(int TaskId)
        {
            using (SqlConnection conn = new SqlConnection(POS_Core.Resources.Configuration.ConnectionString))
            {
                try
                {
                    string sCriteria = string.Empty;
                    if (TaskId > -1)
                    {
                        sCriteria = "Where TaskId = " + TaskId;
                    }

                    string strSQL = @"Select ScheduledTasksID, '' As colTask, TaskName, TaskDescription, PerformTask,
                        case PerformTask when 0 then 'Daily' when 1 Then 'Weekly' when 2 then 'Monthly'
                        when 3 then 'One Time Only' End As PerformTaskText,                            
                        RepeatTask, StartDate, StartTime, TaskId, SendEmail,
                        case SendEmail when 0 then 'No' when 1 then 'Yes' End As SendEmailText , 
                        SendPrint, case SendPrint when 0 then 'No' when 1 then 'Yes' End As SendPrintText,
                        Enabled, case Enabled when 0 then 'Disabled' when 1 then 'Enabled' End As EnabledText,
                        EmailAddress, AdvancedSeetings, Duration, RepeatTaskInterval,
                        (select Top 1 EndTime from ScheduledTasksLog where TaskId = ScheduledTasksID order by EndTime Desc) as LastExecuted
                        From ScheduledTasks " + sCriteria + " order by TaskName";

                    ScheduledTasksData oScheduledTasksData = new ScheduledTasksData();
                    oScheduledTasksData.ScheduledTasks.MergeTable(DataHelper.ExecuteDataset(conn, CommandType.Text, strSQL, PKParameters(TaskId)).Tables[0]);
                    return oScheduledTasksData;
                }
                catch (POSExceptions ex)
                {
                    logger.Fatal(ex, "GetScheduledTasksList(int TaskId)");
                    throw (ex);
                }
                catch (OtherExceptions ex)
                {
                    logger.Fatal(ex, "GetScheduledTasksList(int TaskId)");
                    throw (ex);
                }
                catch (Exception ex)
                {
                    logger.Fatal(ex, "GetScheduledTasksList(int TaskId)");
                    //ErrorHandler.throwException(ex, "", "");
                    return null;
                }
            }
        }

        public ScheduledTasksData GetScheduledTaskByScheduledTasksID(System.Int32 ScheduledTasksID)
        {
            using (SqlConnection conn = new SqlConnection(POS_Core.Resources.Configuration.ConnectionString))
            {
                string sSQL = @"Select ScheduledTasksID, TaskName, TaskDescription, PerformTask,
                        case PerformTask when 0 then 'Daily' when 1 Then 'Weekly' when 2 then 'Monthly' when 3 then 'One Time Only' End As PerformTaskText,
                        RepeatTask, StartDate, StartTime, TaskId, SendEmail, case SendEmail when 0 then 'No' when 1 then 'Yes' End As SendEmailText, 
                        SendPrint, case SendPrint when 0 then 'No' when 1 then 'Yes' End As SendPrintText, Enabled,
                        case Enabled when 0 then 'Disabled' when 1 then 'Enabled' End As EnabledText, EmailAddress, AdvancedSeetings, Duration, RepeatTaskInterval 
                        From ScheduledTasks Where " + clsPOSDBConstants.ScheduledTasks_Fld_ScheduledTasksID + " = " + ScheduledTasksID;

                ScheduledTasksData oScheduledTasksData = new ScheduledTasksData();
                oScheduledTasksData.ScheduledTasks.MergeTable(DataHelper.ExecuteDataset(conn, CommandType.Text, sSQL, PKParameters(ScheduledTasksID)).Tables[0]);
                return oScheduledTasksData;
            }
        }

        public DataTable GetScheduledTasksControlsList(System.Int32 ScheduledTasksID)
        {
            using (SqlConnection conn = new SqlConnection(POS_Core.Resources.Configuration.ConnectionString))
            {
                string sSQL = @"Select ControlsID, ScheduledParaMeterID, ControlsName, ControlsValue, ScheduledTasksID From ScheduledTasksControls Where " + clsPOSDBConstants.ScheduledTasks_Fld_ScheduledTasksID + " = " + ScheduledTasksID;
                DataTable dt = DataHelper.ExecuteDataTable(conn, CommandType.Text, sSQL);
                return dt;
            }
        }

        public bool SaveTaskParameters(DataTable dt, int ScheduledTasksID)
        {
            try
            {
                if (dt != null && dt.Rows.Count > 0)
                {
                    string strSQL = "DELETE FROM ScheduledTasksControls WHERE ScheduledTasksID = " + ScheduledTasksID;
                    DataHelper.ExecuteNonQuery(strSQL);
                    strSQL = string.Empty;
                    foreach (DataRow dr in dt.Rows)
                    {
                        if (strSQL == string.Empty)
                        {
                            strSQL = "INSERT INTO ScheduledTasksControls(ControlsName, ControlsValue, ScheduledTasksID) VALUES" +
                                "('" + Configuration.convertNullToString(dr["ControlsName"]) + "','" + Configuration.convertNullToString(dr["ControlsValue"]) + "'," + ScheduledTasksID + ")";
                        }
                        else
                        {
                            strSQL += ",('" + Configuration.convertNullToString(dr["ControlsName"]) + "','" + Configuration.convertNullToString(dr["ControlsValue"]) + "'," + ScheduledTasksID + ")";
                        }
                    }
                    strSQL += ";";

                    DataHelper.ExecuteNonQuery(strSQL);
                }
            }
            catch(Exception Ex)
            { }
            return true;
        }

        public void Dispose() { }
    }
}
