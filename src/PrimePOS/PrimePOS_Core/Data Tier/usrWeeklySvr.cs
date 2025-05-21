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
    public class usrWeeklySvr : IDisposable
    {
        private static ILogger logger = LogManager.GetCurrentClassLogger();

        #region Persist Methods
        public void Persist(DataSet updates, IDbTransaction tx)
        {
            try
            {
                this.Delete(updates, tx);
                this.Insert(updates, tx);
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
        }
        #endregion                             

        #region insert
        public void Insert(DataSet ds, IDbTransaction tx)
        {
            usrWeeklyTable addedTable = (usrWeeklyTable)ds.Tables[clsPOSDBConstants.ScheduledTasksDetailWeek_tbl].GetChanges(DataRowState.Added);
            string sSQL;
            IDbDataParameter[] insParam;

            if (addedTable != null && addedTable.Rows.Count > 0)
            {
                foreach (usrWeeklyRow row in addedTable.Rows)
                {
                    try
                    {
                        insParam = InsertParameters(row);
                        sSQL = BuildInsertSQL(clsPOSDBConstants.ScheduledTasksDetailWeek_tbl, insParam);
                        DataHelper.ExecuteNonQuery(DBConfig.ConnectionString, CommandType.Text, sSQL, insParam);
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
        }

        public void Insert(DataSet ds)
        {
            string sSQL;
            IDbDataParameter[] insParam;

            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (usrWeeklyRow row in ds.Tables[0].Rows)
                {
                    try
                    {
                        insParam = InsertParameters(row);
                        sSQL = BuildInsertSQL(clsPOSDBConstants.ScheduledTasksDetailWeek_tbl, insParam);
                        DataHelper.ExecuteNonQuery(DBConfig.ConnectionString, CommandType.Text, sSQL, insParam);
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
                        logger.Fatal(ex, "Insert(DataSet ds)");  //PRIMEPOS-2971 07-Jun-2021 JY Added
                        //ErrorHandler.throwException(ex, "", "");
                    }
                }
                ds.AcceptChanges();
            }
        }

        private string BuildInsertSQL(string tableName, IDbDataParameter[] delParam)
        {
            string sInsertSQL = "INSERT INTO " + tableName + " (";

            for (int i = 0; i < delParam.Length; i++)
            {
                if (i == 0)
                    sInsertSQL += delParam[i].SourceColumn;
                else
                    sInsertSQL += ", " + delParam[i].SourceColumn;
            }
            sInsertSQL += ") Values (";

            for (int i = 0; i < delParam.Length; i++)
            {
                if (i == 0)
                    sInsertSQL += delParam[i].ParameterName;
                else
                    sInsertSQL += ", " + delParam[i].ParameterName;
            }
            sInsertSQL = sInsertSQL + ")";
            return sInsertSQL;
        }

        private IDbDataParameter[] InsertParameters(usrWeeklyRow row)
        {
            IDbDataParameter[] sqlParams = DataFactory.CreateParameterArray(3);

            sqlParams[0] = new SqlParameter("@" + clsPOSDBConstants.ScheduledTasksDetailWeek_Fld_ScheduledTasksID, System.Data.SqlDbType.Int);
            sqlParams[1] = new SqlParameter("@" + clsPOSDBConstants.ScheduledTasksDetailWeek_Fld_Days, System.Data.SqlDbType.Int);
            sqlParams[2] = new SqlParameter("@" + clsPOSDBConstants.ScheduledTasksDetailWeek_Fld_SelectedDays, System.Data.SqlDbType.VarChar);

            sqlParams[0].SourceColumn = clsPOSDBConstants.ScheduledTasksDetailWeek_Fld_ScheduledTasksID;
            sqlParams[1].SourceColumn = clsPOSDBConstants.ScheduledTasksDetailWeek_Fld_Days;
            sqlParams[2].SourceColumn = clsPOSDBConstants.ScheduledTasksDetailWeek_Fld_SelectedDays;

            if (row.ScheduledTasksID != 0)
                sqlParams[0].Value = row.ScheduledTasksID;
            else
                sqlParams[0].Value = DBNull.Value;

            if (row.Days != 0)
                sqlParams[1].Value = row.Days;
            else
                sqlParams[1].Value = DBNull.Value;

            if (row.SelectedDays != System.String.Empty)
                sqlParams[2].Value = row.SelectedDays;
            else
                sqlParams[2].Value = DBNull.Value;

            return (sqlParams);
        }
        #endregion

        #region update
        public void Update(DataSet ds, IDbTransaction tx)
        {
            usrWeeklyTable modifiedTable = (usrWeeklyTable)ds.Tables[clsPOSDBConstants.ScheduledTasksDetailWeek_tbl].GetChanges(DataRowState.Modified);

            string sSQL;
            IDbDataParameter[] updParam;

            if (modifiedTable != null && modifiedTable.Rows.Count > 0)
            {
                foreach (usrWeeklyRow row in modifiedTable.Rows)
                {
                    try
                    {
                        updParam = UpdateParameters(row);
                        sSQL = BuildUpdateSQL(clsPOSDBConstants.ScheduledTasksDetailWeek_tbl, updParam);
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

        private IDbDataParameter[] UpdateParameters(usrWeeklyRow row)
        {
            IDbDataParameter[] sqlParams = DataFactory.CreateParameterArray(3);

            sqlParams[0] = new SqlParameter("@" + clsPOSDBConstants.ScheduledTasksDetailWeek_Fld_ScheduledTasksID, System.Data.SqlDbType.Int);
            sqlParams[1] = new SqlParameter("@" + clsPOSDBConstants.ScheduledTasksDetailWeek_Fld_Days, System.Data.SqlDbType.Int);
            sqlParams[2] = new SqlParameter("@" + clsPOSDBConstants.ScheduledTasksDetailWeek_Fld_SelectedDays, System.Data.SqlDbType.VarChar);

            sqlParams[0].SourceColumn = clsPOSDBConstants.ScheduledTasksDetailWeek_Fld_ScheduledTasksID;
            sqlParams[1].SourceColumn = clsPOSDBConstants.ScheduledTasksDetailWeek_Fld_Days;
            sqlParams[2].SourceColumn = clsPOSDBConstants.ScheduledTasksDetailWeek_Fld_SelectedDays;

            if (row.ScheduledTasksID != 0)
                sqlParams[0].Value = row.ScheduledTasksID;
            else
                sqlParams[0].Value = DBNull.Value;

            if (row.Days != 0)
                sqlParams[1].Value = row.Days;
            else
                sqlParams[1].Value = DBNull.Value;

            if (row.SelectedDays != System.String.Empty)
                sqlParams[2].Value = row.SelectedDays;
            else
                sqlParams[2].Value = DBNull.Value;

            return (sqlParams);
        }
        #endregion

        #region delete
        public void Delete(System.Int32 ScheduledTasksID)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(POS_Core.Resources.Configuration.ConnectionString))
                {
                    string sSQL = "DELETE FROM " + clsPOSDBConstants.ScheduledTasksDetailWeek_tbl + " WHERE " + clsPOSDBConstants.ScheduledTasksDetailWeek_Fld_ScheduledTasksID + " = " + ScheduledTasksID;
                    DataHelper.ExecuteNonQuery(conn,CommandType.Text, sSQL);
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
                logger.Fatal(ex, "Delete(System.Int32 ScheduledTasksID)");  //PRIMEPOS-2971 07-Jun-2021 JY Added
                //ErrorHandler.throwException(ex, "", "");
            }
        }

        public void Delete(DataSet ds, IDbTransaction tx)
        {
            usrWeeklyTable table = (usrWeeklyTable)ds.Tables[clsPOSDBConstants.ScheduledTasksDetailWeek_tbl].GetChanges(DataRowState.Deleted);
            string sSQL;
            IDbDataParameter[] delParam;

            if (table != null && table.Rows.Count > 0)
            {
                table.RejectChanges();
                foreach (usrWeeklyRow row in table.Rows)
                {
                    try
                    {
                        delParam = PKParameters(row);

                        sSQL = BuildDeleteSQL(clsPOSDBConstants.ScheduledTasksDetailWeek_tbl, delParam);
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
        #endregion

        private IDbDataParameter[] PKParameters(usrWeeklyRow row)
        {
            IDbDataParameter[] sqlParams = DataFactory.CreateParameterArray(1);

            sqlParams[0] = DataFactory.CreateParameter();
            sqlParams[0].ParameterName = "@ScheduledTasksID";
            sqlParams[0].DbType = System.Data.DbType.Int32;
            sqlParams[0].Value = row.ScheduledTasksID;
            sqlParams[0].SourceColumn = clsPOSDBConstants.ScheduledTasksDetailWeek_Fld_ScheduledTasksID;
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

        public usrWeeklyData GetScheduledTasksDetailWeek(System.Int32 ScheduledTasksID)
        {
            using (SqlConnection conn = new SqlConnection(POS_Core.Resources.Configuration.ConnectionString))
            {
                try
                {
                    string strSQL = "SELECT Days, SelectedDays, ScheduledTasksID FROM ScheduledTasksDetailWeek WHERE ScheduledTasksID = " + ScheduledTasksID;
                    usrWeeklyData ousrWeeklyData = new usrWeeklyData();
                    ousrWeeklyData.ScheduledTasksDetailWeek.MergeTable(DataHelper.ExecuteDataset(conn, CommandType.Text, strSQL, PKParameters(ScheduledTasksID)).Tables[0]);
                    return ousrWeeklyData;
                }
                catch (POSExceptions ex)
                {
                    logger.Fatal(ex, "GetScheduledTasksDetailWeek(System.Int32 ScheduledTasksID)");
                    throw (ex);
                }
                catch (OtherExceptions ex)
                {
                    logger.Fatal(ex, "GetScheduledTasksDetailWeek(System.Int32 ScheduledTasksID)");
                    throw (ex);
                }
                catch (Exception ex)
                {
                    logger.Fatal(ex, "GetScheduledTasksDetailWeek(System.Int32 ScheduledTasksID)");
                    //ErrorHandler.throwException(ex, "", "");
                    return null;
                }
            }
        }

        public void Dispose() { }
    }
}
