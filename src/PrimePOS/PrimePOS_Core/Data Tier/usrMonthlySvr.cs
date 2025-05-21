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
    public class usrMonthlySvr : IDisposable
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
            usrMonthlyTable addedTable = (usrMonthlyTable)ds.Tables[clsPOSDBConstants.ScheduledTasksDetailMonth_tbl].GetChanges(DataRowState.Added);
            string sSQL;
            IDbDataParameter[] insParam;

            if (addedTable != null && addedTable.Rows.Count > 0)
            {
                foreach (usrMonthlyRow row in addedTable.Rows)
                {
                    try
                    {
                        insParam = InsertParameters(row);
                        sSQL = BuildInsertSQL(clsPOSDBConstants.ScheduledTasksDetailMonth_tbl, insParam);
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
                foreach (usrMonthlyRow row in ds.Tables[0].Rows)
                {
                    try
                    {
                        insParam = InsertParameters(row);
                        sSQL = BuildInsertSQL(clsPOSDBConstants.ScheduledTasksDetailMonth_tbl, insParam);
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

        private IDbDataParameter[] InsertParameters(usrMonthlyRow row)
        {
            IDbDataParameter[] sqlParams = DataFactory.CreateParameterArray(6);

            sqlParams[0] = new SqlParameter("@" + clsPOSDBConstants.ScheduledTasksDetailMonth_Fld_ScheduledTasksID, System.Data.SqlDbType.Int);
            sqlParams[1] = new SqlParameter("@" + clsPOSDBConstants.ScheduledTasksDetailMonth_Fld_DaysOrOn, System.Data.SqlDbType.Bit);
            sqlParams[2] = new SqlParameter("@" + clsPOSDBConstants.ScheduledTasksDetailMonth_Fld_SelectionMonths, System.Data.SqlDbType.VarChar);
            sqlParams[3] = new SqlParameter("@" + clsPOSDBConstants.ScheduledTasksDetailMonth_Fld_SelectionDays, System.Data.SqlDbType.VarChar);
            sqlParams[4] = new SqlParameter("@" + clsPOSDBConstants.ScheduledTasksDetailMonth_Fld_Monthperiods, System.Data.SqlDbType.VarChar);
            sqlParams[5] = new SqlParameter("@" + clsPOSDBConstants.ScheduledTasksDetailMonth_Fld_weekDays, System.Data.SqlDbType.VarChar);

            sqlParams[0].SourceColumn = clsPOSDBConstants.ScheduledTasksDetailMonth_Fld_ScheduledTasksID;
            sqlParams[1].SourceColumn = clsPOSDBConstants.ScheduledTasksDetailMonth_Fld_DaysOrOn;
            sqlParams[2].SourceColumn = clsPOSDBConstants.ScheduledTasksDetailMonth_Fld_SelectionMonths;
            sqlParams[3].SourceColumn = clsPOSDBConstants.ScheduledTasksDetailMonth_Fld_SelectionDays;
            sqlParams[4].SourceColumn = clsPOSDBConstants.ScheduledTasksDetailMonth_Fld_Monthperiods;
            sqlParams[5].SourceColumn = clsPOSDBConstants.ScheduledTasksDetailMonth_Fld_weekDays;

            if (row.ScheduledTasksID != 0)
                sqlParams[0].Value = row.ScheduledTasksID;
            else
                sqlParams[0].Value = DBNull.Value;

            sqlParams[1].Value = row.DaysOrOn;

            if (row.ScheduledTasksID != 0)
                sqlParams[1].Value = row.ScheduledTasksID;
            else
                sqlParams[1].Value = DBNull.Value;

            if (row.SelectionMonths != System.String.Empty)
                sqlParams[2].Value = row.SelectionMonths;
            else
                sqlParams[2].Value = DBNull.Value;

            if (row.SelectionDays != System.String.Empty)
                sqlParams[3].Value = row.SelectionDays;
            else
                sqlParams[3].Value = DBNull.Value;

            if (row.Monthperiods != System.String.Empty)
                sqlParams[4].Value = row.Monthperiods;
            else
                sqlParams[4].Value = DBNull.Value;

            if (row.weekDays != System.String.Empty)
                sqlParams[5].Value = row.weekDays;
            else
                sqlParams[5].Value = DBNull.Value;

            return (sqlParams);
        }
        #endregion

        #region update
        public void Update(DataSet ds, IDbTransaction tx)
        {
            usrMonthlyTable modifiedTable = (usrMonthlyTable)ds.Tables[clsPOSDBConstants.ScheduledTasksDetailMonth_tbl].GetChanges(DataRowState.Modified);

            string sSQL;
            IDbDataParameter[] updParam;

            if (modifiedTable != null && modifiedTable.Rows.Count > 0)
            {
                foreach (usrMonthlyRow row in modifiedTable.Rows)
                {
                    try
                    {
                        updParam = UpdateParameters(row);
                        sSQL = BuildUpdateSQL(clsPOSDBConstants.ScheduledTasksDetailMonth_tbl, updParam);
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

        private IDbDataParameter[] UpdateParameters(usrMonthlyRow row)
        {
            IDbDataParameter[] sqlParams = DataFactory.CreateParameterArray(6);

            sqlParams[0] = new SqlParameter("@" + clsPOSDBConstants.ScheduledTasksDetailMonth_Fld_ScheduledTasksID, System.Data.SqlDbType.Int);
            sqlParams[1] = new SqlParameter("@" + clsPOSDBConstants.ScheduledTasksDetailMonth_Fld_DaysOrOn, System.Data.SqlDbType.Bit);
            sqlParams[2] = new SqlParameter("@" + clsPOSDBConstants.ScheduledTasksDetailMonth_Fld_SelectionMonths, System.Data.SqlDbType.VarChar);
            sqlParams[3] = new SqlParameter("@" + clsPOSDBConstants.ScheduledTasksDetailMonth_Fld_SelectionDays, System.Data.SqlDbType.VarChar);
            sqlParams[4] = new SqlParameter("@" + clsPOSDBConstants.ScheduledTasksDetailMonth_Fld_Monthperiods, System.Data.SqlDbType.VarChar);
            sqlParams[5] = new SqlParameter("@" + clsPOSDBConstants.ScheduledTasksDetailMonth_Fld_weekDays, System.Data.SqlDbType.VarChar);

            sqlParams[0].SourceColumn = clsPOSDBConstants.ScheduledTasksDetailMonth_Fld_ScheduledTasksID;
            sqlParams[1].SourceColumn = clsPOSDBConstants.ScheduledTasksDetailMonth_Fld_DaysOrOn;
            sqlParams[2].SourceColumn = clsPOSDBConstants.ScheduledTasksDetailMonth_Fld_SelectionMonths;
            sqlParams[3].SourceColumn = clsPOSDBConstants.ScheduledTasksDetailMonth_Fld_SelectionDays;
            sqlParams[4].SourceColumn = clsPOSDBConstants.ScheduledTasksDetailMonth_Fld_Monthperiods;
            sqlParams[5].SourceColumn = clsPOSDBConstants.ScheduledTasksDetailMonth_Fld_weekDays;

            if (row.ScheduledTasksID != 0)
                sqlParams[0].Value = row.ScheduledTasksID;
            else
                sqlParams[0].Value = DBNull.Value;

            sqlParams[1].Value = row.DaysOrOn;

            if (row.ScheduledTasksID != 0)
                sqlParams[1].Value = row.ScheduledTasksID;
            else
                sqlParams[1].Value = DBNull.Value;

            if (row.SelectionMonths != System.String.Empty)
                sqlParams[2].Value = row.SelectionMonths;
            else
                sqlParams[2].Value = DBNull.Value;

            if (row.SelectionDays != System.String.Empty)
                sqlParams[3].Value = row.SelectionDays;
            else
                sqlParams[3].Value = DBNull.Value;

            if (row.Monthperiods != System.String.Empty)
                sqlParams[4].Value = row.Monthperiods;
            else
                sqlParams[4].Value = DBNull.Value;

            if (row.weekDays != System.String.Empty)
                sqlParams[5].Value = row.weekDays;
            else
                sqlParams[5].Value = DBNull.Value;

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
                    string sSQL = "DELETE FROM " + clsPOSDBConstants.ScheduledTasksDetailMonth_tbl + " WHERE " + clsPOSDBConstants.ScheduledTasksDetailWeek_Fld_ScheduledTasksID + " = " + ScheduledTasksID;
                    DataHelper.ExecuteNonQuery(conn, CommandType.Text, sSQL);
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
            usrMonthlyTable table = (usrMonthlyTable)ds.Tables[clsPOSDBConstants.ScheduledTasksDetailMonth_tbl].GetChanges(DataRowState.Deleted);
            string sSQL;
            IDbDataParameter[] delParam;

            if (table != null && table.Rows.Count > 0)
            {
                table.RejectChanges();
                foreach (usrMonthlyRow row in table.Rows)
                {
                    try
                    {
                        delParam = PKParameters(row);

                        sSQL = BuildDeleteSQL(clsPOSDBConstants.ScheduledTasksDetailMonth_tbl, delParam);
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

        private IDbDataParameter[] PKParameters(usrMonthlyRow row)
        {
            IDbDataParameter[] sqlParams = DataFactory.CreateParameterArray(1);

            sqlParams[0] = DataFactory.CreateParameter();
            sqlParams[0].ParameterName = "@ScheduledTasksID";
            sqlParams[0].DbType = System.Data.DbType.Int32;
            sqlParams[0].Value = row.ScheduledTasksID;
            sqlParams[0].SourceColumn = clsPOSDBConstants.ScheduledTasksDetailMonth_Fld_ScheduledTasksID;
            return (sqlParams);
        }

        private IDbDataParameter[] PKParameters(System.Int32 ScheduledTasksID)
        {
            IDbDataParameter[] sqlParams = DataFactory.CreateParameterArray(1);

            sqlParams[0] = DataFactory.CreateParameter();
            sqlParams[0].ParameterName = "@ScheduledTasksID";
            sqlParams[0].DbType = System.Data.DbType.Int32;
            sqlParams[0].Value = ScheduledTasksID;
            sqlParams[0].SourceColumn = clsPOSDBConstants.ScheduledTasksDetailMonth_Fld_ScheduledTasksID;
            return (sqlParams);
        }

        public usrMonthlyData GetScheduledTasksDetailMonth(System.Int32 ScheduledTasksID)
        {
            using (SqlConnection conn = new SqlConnection(POS_Core.Resources.Configuration.ConnectionString))
            {
                try
                {
                    string strSQL = "Select DaysOrOn, SelectionMonths, SelectionDays, Monthperiods, weekDays, ScheduledTasksID From ScheduledTasksDetailMonth WHERE ScheduledTasksID = " + ScheduledTasksID;
                    usrMonthlyData ousrMonthlyData = new usrMonthlyData();
                    ousrMonthlyData.ScheduledTasksDetailMonth.MergeTable(DataHelper.ExecuteDataset(conn, CommandType.Text, strSQL, PKParameters(ScheduledTasksID)).Tables[0]);
                    return ousrMonthlyData;
                }
                catch (POSExceptions ex)
                {
                    logger.Fatal(ex, "GetScheduledTasksDetailMonth(System.Int32 ScheduledTasksID)");
                    throw (ex);
                }
                catch (OtherExceptions ex)
                {
                    logger.Fatal(ex, "GetScheduledTasksDetailMonth(System.Int32 ScheduledTasksID)");
                    throw (ex);
                }
                catch (Exception ex)
                {
                    logger.Fatal(ex, "GetScheduledTasksDetailMonth(System.Int32 ScheduledTasksID)");
                    //ErrorHandler.throwException(ex, "", "");
                    return null;
                }
            }
        }

        public void Dispose() { }
    }
}
