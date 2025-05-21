using System;
using System.Data;
using System.Collections;
using POS_Core.CommonData.Tables;
using POS_Core.CommonData.Rows;
using POS_Core.CommonData;
//using Resources;
using System.Windows.Forms;
using POS_Core.ErrorLogging;
using System.Data.SqlClient;
using POS_Core.Resources;
using Resources;
using POS_Core.Resources.DelegateHandler;
////using POS.Resources;
using NLog;

namespace POS_Core.DataAccess
{
    // Provides data access methods for DeptCode
    public class TimesheetSvr : IDisposable
    {
        private static ILogger logger = LogManager.GetCurrentClassLogger();

        #region Persist Methods

        // Inserts, updates or deletes rows in a DataSet, within a database transaction.

        public void Persist(DataSet updates)
        {
            SqlTransaction tx;
            SqlConnection conn = new SqlConnection(POS_Core.Resources.Configuration.ConnectionString);

            conn.Open();
            tx = conn.BeginTransaction();
            try
            {
                Int64 iID = 0;
                this.Insert(updates, tx, ref iID);
                this.Update(updates, tx, ref iID);

                updates.AcceptChanges();
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
                logger.Fatal(ex, "Persist(DataSet updates)");  //PRIMEPOS-2971 07-Jun-2021 JY Added
                //ErrorHandler.throwException(ex, "", "");
            }
        }

        public void AddToTimesheet(DataSet updates)
        {
            SqlTransaction tx;
            SqlConnection conn = new SqlConnection(POS_Core.Resources.Configuration.ConnectionString);
            bool isMissingINOUT = false;
            #region Sprint-19 - 2053 12-Mar-2015 JY Added to validate TimeIn should be less than TimeOut
            bool isInvalidTimeIn = false, isDiff24 = false;
            TimeSpan ts;
            #endregion
            conn.Open();
            tx = conn.BeginTransaction();
            try
            {
                Int64 iID = 0;

                foreach (DataRow oRow in updates.Tables[0].Rows)
                {
                    DateTime TimeIN = string.IsNullOrEmpty(oRow["TimeIn"].ToString()) ? DateTime.MinValue : Convert.ToDateTime(oRow["TimeIn"].ToString());
                    DateTime TimeOUT = string.IsNullOrEmpty(oRow["TimeOut"].ToString()) ? DateTime.MinValue : Convert.ToDateTime(oRow["TimeOut"].ToString());
                    if (TimeIN.Year > 1901 && TimeOUT.Year > 1901)
                    {
                        #region Sprint-19 - 2053 12-Mar-2015 JY Added
                        try
                        {
                            ts = TimeOUT.Subtract(TimeIN);
                            if (ts.TotalHours < 0)
                            {
                                isInvalidTimeIn = true;
                                continue;
                            }
                            if (ts.TotalHours > 24)
                            {
                                isDiff24 = true;
                                continue;
                            }
                        }
                        catch { }
                        #endregion

                        #region Sprint-25 - PRIMEPOS-2253 24-Mar-2017 JY Added logic to update processed timesheet record
                        if (Configuration.convertNullToInt(oRow["TimesheetId"].ToString()) > 0)
                        {
                            if (oRow.RowState == DataRowState.Modified)
                            {
                                string strSQL = "UPDATE Timesheet SET TimeIn = @TimeIn, TimeOut = @TimeOut, LastUpdatedBy = '" + Configuration.UserName + "'" +
                                                " , LastUpdatedOn = GetDate() WHERE ID = " + Configuration.convertNullToInt(oRow["TimesheetId"].ToString());

                                IDbDataParameter[] sqlParams = DataFactory.CreateParameterArray(2);
                                sqlParams[0] = DataFactory.CreateParameter("@TimeIn", System.Data.DbType.DateTime, "TimeIn", oRow["TimeIn"].ToString());
                                sqlParams[1] = DataFactory.CreateParameter("@TimeOut", System.Data.DbType.DateTime, "Timeout", oRow["Timeout"].ToString());
                                DataHelper.ExecuteNonQuery(tx, CommandType.Text, strSQL, sqlParams);

                                if (Configuration.convertNullToInt(oRow["TimeInID"].ToString()) > 0)
                                {
                                    string strTimeInUpdate = "Update TimesheetEvents Set EventDate = @TimeIn, LastUpdatedBy = '" + Configuration.UserName + "', LastUpdatedOn = GetDate() Where ID=" + oRow["TimeInID"].ToString();

                                    sqlParams = DataFactory.CreateParameterArray(1);
                                    sqlParams[0] = DataFactory.CreateParameter("@TimeIn", System.Data.DbType.DateTime, "TimeIn", oRow["TimeIn"].ToString());
                                    DataHelper.ExecuteNonQuery(tx, CommandType.Text, strTimeInUpdate, sqlParams);
                                }
                                if (Configuration.convertNullToInt(oRow["TimeOutID"].ToString()) > 0)
                                {
                                    string strTimeOutUpdate = "Update TimesheetEvents Set EventDate = @TimeOut, LastUpdatedBy = '" + Configuration.UserName + "', LastUpdatedOn = GetDate() Where ID=" + oRow["TimeOutID"].ToString();

                                    sqlParams = DataFactory.CreateParameterArray(1);
                                    sqlParams[0] = DataFactory.CreateParameter("@TimeOut", System.Data.DbType.DateTime, "Timeout", oRow["Timeout"].ToString());
                                    DataHelper.ExecuteNonQuery(tx, CommandType.Text, strTimeOutUpdate, sqlParams);
                                }
                            }
                        }
                        #endregion
                        else
                        {
                            string strSQL = " INSERT INTO Timesheet (UserID, [TimeIn], [TimeOut], [IsManualIn], [IsManualOut], " +
                                        " [LastUpdatedBy], [LastUpdatedOn]) " +
                                        " VALUES(@UserID,@TimeIn,@TimeOut,0,0,@LastUpdatedBy,GetDate()) ";

                            IDbDataParameter[] sqlParams = DataFactory.CreateParameterArray(4);

                            sqlParams[0] = DataFactory.CreateParameter("@UserID", System.Data.DbType.String, "UserID", oRow["UserID"].ToString());
                            sqlParams[1] = DataFactory.CreateParameter("@TimeIn", System.Data.DbType.DateTime, "TimeIn", oRow["TimeIn"].ToString());
                            sqlParams[2] = DataFactory.CreateParameter("@TimeOut", System.Data.DbType.DateTime, "Timeout", oRow["Timeout"].ToString());
                            sqlParams[3] = DataFactory.CreateParameter("@LastUpdatedBy", System.Data.DbType.String, "LastUpdatedBy", Configuration.UserName);

                            DataHelper.ExecuteNonQuery(tx, CommandType.Text, strSQL, sqlParams);

                            if (Configuration.convertNullToInt(oRow["TimeInID"].ToString()) > 0)
                            {
                                string strTimeInUpdate = "Update TimesheetEvents Set isProcessed=1, EventDate = @TimeIn, LastUpdatedBy = '" + Configuration.UserName + "', LastUpdatedOn = GetDate() Where ID=" + oRow["TimeInID"].ToString();

                                #region Sprint-25 - PRIMEPOS-2253 24-Mar-2017 JY Added
                                sqlParams = DataFactory.CreateParameterArray(1);
                                sqlParams[0] = DataFactory.CreateParameter("@TimeIn", System.Data.DbType.DateTime, "TimeIn", oRow["TimeIn"].ToString());
                                DataHelper.ExecuteNonQuery(tx, CommandType.Text, strTimeInUpdate, sqlParams);
                                #endregion
                                //DataHelper.ExecuteNonQuery(tx, CommandType.Text, strTimeInUpdate);
                            }
                            #region Sprint-25 - PRIMEPOS-2253 28-Mar-2017 JY Added logic to add missing TimeIn record
                            else
                            {
                                strSQL = "INSERT INTO TimesheetEvents(EventDate,Status,LastUpdatedBy,LastUpdatedOn,isProcessed,UserID) VALUES(@TimeIn, 0, '" + Configuration.UserName + "', GetDate(), 1, @UserID)";
                                sqlParams = DataFactory.CreateParameterArray(2);

                                sqlParams[0] = DataFactory.CreateParameter("@TimeIn", System.Data.DbType.DateTime, "TimeIn", oRow["TimeIn"].ToString());
                                sqlParams[1] = DataFactory.CreateParameter("@UserID", System.Data.DbType.String, "UserID", oRow["UserID"].ToString());
                                DataHelper.ExecuteNonQuery(tx, CommandType.Text, strSQL, sqlParams);
                            }
                            #endregion

                            if (Configuration.convertNullToInt(oRow["TimeOutID"].ToString()) > 0)
                            {
                                string strTimeOutUpdate = "Update TimesheetEvents Set isProcessed=1, EventDate = @TimeOut, LastUpdatedBy = '" + Configuration.UserName + "', LastUpdatedOn = GetDate() Where ID=" + oRow["TimeOutID"].ToString();

                                #region Sprint-25 - PRIMEPOS-2253 24-Mar-2017 JY Added
                                sqlParams = DataFactory.CreateParameterArray(1);
                                sqlParams[0] = DataFactory.CreateParameter("@TimeOut", System.Data.DbType.DateTime, "Timeout", oRow["Timeout"].ToString());
                                DataHelper.ExecuteNonQuery(tx, CommandType.Text, strTimeOutUpdate, sqlParams);
                                #endregion
                                //DataHelper.ExecuteNonQuery(tx, CommandType.Text, strTimeOutUpdate);
                            }
                            #region Sprint-25 - PRIMEPOS-2253 28-Mar-2017 JY Added logic to add missing TimeOut record
                            else
                            {
                                strSQL = "INSERT INTO TimesheetEvents(EventDate,Status,LastUpdatedBy,LastUpdatedOn,isProcessed,UserID) VALUES(@TimeOut, 1, '" + Configuration.UserName + "', GetDate(), 1, @UserID)";
                                sqlParams = DataFactory.CreateParameterArray(2);

                                sqlParams[0] = DataFactory.CreateParameter("@TimeOut", System.Data.DbType.DateTime, "Timeout", oRow["Timeout"].ToString());
                                sqlParams[1] = DataFactory.CreateParameter("@UserID", System.Data.DbType.String, "UserID", oRow["UserID"].ToString());
                                DataHelper.ExecuteNonQuery(tx, CommandType.Text, strSQL, sqlParams);
                            }
                            #endregion
                        }
                    }
                    else if (TimeIN.Year < 1901)
                    {
                        isMissingINOUT = true;
                        continue;
                    }
                    else if (TimeOUT.Year < 1901)
                    {
                        isMissingINOUT = true;
                        continue;
                    }
                }
                if (isMissingINOUT == true)
                {
                    clsCoreUIHelper.ShowErrorMsg("Please enter missing Time Out/Time In to create time sheet.");
                }
                #region Sprint-19 - 2053 12-Mar-2015 JY Added 
                if (isInvalidTimeIn == true)
                    clsCoreUIHelper.ShowErrorMsg("Please make sure that TimeIn should be less than TimeOut");
                if (isDiff24 == true)
                    clsCoreUIHelper.ShowErrorMsg("Please make sure that the total hours should not be greater than 24 hours");
                #endregion

                tx.Commit();
            }
            catch (POSExceptions ex)
            {
                tx.Rollback();
                throw (ex);
            }
            catch (OtherExceptions ex)
            {
                tx.Rollback();
                throw (ex);
            }
            catch (Exception ex)
            {
                tx.Rollback();
                logger.Fatal(ex, "AddToTimesheet(DataSet updates)");  //PRIMEPOS-2971 07-Jun-2021 JY Added
                //ErrorHandler.throwException(ex, "", "");
            }
        }
        #endregion

        #region Get Methods

        // Looks up a DeptCode based on its primary-key:System.Int32 DeptCode

        public TimesheetData Populate(System.Int64 iID)
        {
            using (SqlConnection conn = new SqlConnection(POS_Core.Resources.Configuration.ConnectionString))
            {
                return (Populate(iID, conn));
            }
        }

        public TimesheetData Populate(System.Int64 ID, IDbConnection conn)
        {
            try
            {
                string sSQL = "Select * "
                                + " FROM "
                                + clsPOSDBConstants.Timesheet_tbl
                                + " WHERE "
                                + " " + clsPOSDBConstants.Timesheet_Fld_ID + "=" + ID.ToString();

                TimesheetData ds = new TimesheetData();
                ds.Timesheet.MergeTable(DataHelper.ExecuteDataset(conn, CommandType.Text
                                            , sSQL
                                            , PKParameters(ID)).Tables[0]);
                return ds;
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
                logger.Fatal(ex, "Populate(System.Int64 ID, IDbConnection conn)");  //PRIMEPOS-2971 07-Jun-2021 JY Added
                                                                                    //ErrorHandler.throwException(ex,"","");
                return null;
            }
        }

        public TimesheetData PopulateByUserID(System.String sUserID)
        {
            using (SqlConnection conn = new SqlConnection(POS_Core.Resources.Configuration.ConnectionString))
            {
                return PopulateByUserID(sUserID, conn);
            }
        }

        public TimesheetData GetLastTimeIn(System.String sUserID)
        {
            TimesheetData ds = null;
            try
            {
                string sSQL = "Select Top 1 * "
                                + " FROM "
                                + clsPOSDBConstants.Timesheet_tbl
                                + " WHERE "
                                + " TimeOut Is Null And LTRIM(RTRIM(UPPER(" + clsPOSDBConstants.Timesheet_Fld_UserID + "))) = '" + sUserID.Trim().ToUpper() + "'";

                ds = new TimesheetData();

                using (SqlConnection conn = new SqlConnection(POS_Core.Resources.Configuration.ConnectionString))
                {

                    ds.Timesheet.MergeTable(DataHelper.ExecuteDataset(conn, CommandType.Text, sSQL).Tables[0]);
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
                logger.Fatal(ex, "GetLastTimeIn(System.String sUserID)");  //PRIMEPOS-2971 07-Jun-2021 JY Added
                //ErrorHandler.throwException(ex, "", "");
            }

            return ds;
        }

        public DataSet SearchDataFromEvents(System.String sUserID, DateTime sStartDate, DateTime sEndDate, System.Boolean bIncludeProcessedTimesheet)   //Sprint-25 - PRIMEPOS-2253 24-Mar-2017 JY Added bIncludeProcessedTimesheet 
        {
            DataSet ds = null;
            try
            {
                //PRIMEPOS-2626 07-Jan-2019 JY resolved bug in the query
                //PRIMEPOS-189 05-Aug-2021 JY Added Hourly Rate
                string strFilterUserId = string.Empty;
                if (sUserID.Trim() != "")
                {
                    strFilterUserId = " And LTRIM(RTRIM(UPPER(tin.UserID))) = '" + sUserID.Trim().ToUpper() + "'";
                }

                string sSQL = "SELECT UPPER(timein.UserID) AS UserID, TimeInOutIDs.TimeInID, TimeInOutIDs.TimeOutID," +
                " TimeIn.EventDate as TimeIn, TimeOut.EventDate as TimeOut, TimeIn.isProcessed, 0 AS TimesheetId, ISNULL(b.HourlyRate,0) AS HourlyRate " +
                " FROM " +
                " (SELECT tin.ID TimeInID," +
                " IsNull(( " +
                " SELECT TOP 1 Case When Status = 1 Then tout.ID Else 0 End as TimeOutID FROM TimesheetEvents tout " +
                " Where LTRIM(RTRIM(UPPER(tout.userID))) = LTRIM(RTRIM(UPPER(tin.UserID))) And tout.isProcessed = 0 " +
                " And tout.ID = (SELECT Min(nextEvent.ID) toutID FROM timesheetEvents nextEvent " +
                                "WHERE LTRIM(RTRIM(UPPER(nextEvent.userid))) = LTRIM(RTRIM(UPPER(tin.userid))) AND nextEvent.id > tin.id And nextEvent.isProcessed=0 " +
                                ") " +
                " ),0) as TimeOutID, tin.isProcessed From TimesheetEvents tin " +
                " Where tin.status = 0 And tin.isProcessed = 0 " +
                strFilterUserId +
                " And tin.EventDate between convert(datetime, cast('" + sStartDate.ToShortDateString() + " 00:00:00 ' as datetime) ,113) and convert(datetime, cast('" + sEndDate.ToShortDateString() + " 23:59:59 ' as datetime) ,113) " +
                " ) As TimeInOutIDs " +
                " Inner Join TimesheetEvents TimeIn On(TimeInOutIDs.TimeInID = TimeIn.ID) " +
                " Left outer Join TimesheetEvents TimeOut On (TimeInOutIDs.TimeOutID = TimeOut.ID)" +
                " LEFT JOIN Users b ON timein.UserID = b.UserID" +
                " order by timein.UserID, timein.id, timein.eventdate";

                ds = new DataSet();

                using (SqlConnection conn = new SqlConnection(POS_Core.Resources.Configuration.ConnectionString))
                {

                    ds = DataHelper.ExecuteDataset(conn, CommandType.Text, sSQL);
                }

                string exitOutIDs = "";
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    if (ds.Tables[0].Rows[i]["TimeOutID"].ToString() != "0")
                    {
                        exitOutIDs += ds.Tables[0].Rows[i]["TimeOutID"].ToString() + ",";
                    }
                }

                if (exitOutIDs.EndsWith(",") == true)
                {
                    exitOutIDs = exitOutIDs.Substring(0, exitOutIDs.Length - 1);
                }

                if (exitOutIDs.Length == 0)
                {
                    exitOutIDs = "-1";
                }

                if (sUserID.Trim() != "")
                {
                    strFilterUserId = " And LTRIM(RTRIM(UPPER(UserID))) = '" + sUserID.Trim().ToUpper() + "'";
                }

                string sSQLTimeout = "Select UserID, 0 As TimeInID,ID as TimeOutID, Null as TimeIn, EventDate as TimeOut, isProcessed, 0 AS TimesheetId " +
                " From TimesheetEvents Where ID Not In (" + exitOutIDs + ") " +
                strFilterUserId +
                " And Status = 1 And isProcessed = 0 " +
                " And EventDate between convert(datetime, cast('" + sStartDate.ToShortDateString() + " 00:00:00 ' as datetime) ,113) and convert(datetime, cast('" + sEndDate.ToShortDateString() + " 23:59:59 ' as datetime) ,113) ";

                DataSet dsTimeOut = new DataSet();
                using (SqlConnection conn = new SqlConnection(POS_Core.Resources.Configuration.ConnectionString))
                {

                    dsTimeOut = DataHelper.ExecuteDataset(conn, CommandType.Text, sSQLTimeout);
                }

                foreach (DataRow dr in dsTimeOut.Tables[0].Rows)
                {
                    ds.Tables[0].Rows.Add(dr.ItemArray);
                }

                #region Sprint-25 - PRIMEPOS-2253 24-Mar-2017 JY Added
                if (bIncludeProcessedTimesheet)
                {
                    DataSet dsProcesedTimesheet = GetProcessedTimesheetRecords(sUserID, sStartDate, sEndDate);
                    foreach (DataRow dr in dsProcesedTimesheet.Tables[0].Rows)
                    {
                        ds.Tables[0].Rows.Add(dr.ItemArray);
                    }
                }
                #endregion
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
                logger.Fatal(ex, "SearchDataFromEvents(System.String sUserID,DateTime sStartDate, DateTime sEndDate, System.Boolean bIncludeProcessedTimesheet)");  //PRIMEPOS-2971 07-Jun-2021 JY Added
                //ErrorHandler.throwException(ex, "", "");
            }
            return ds;
        }

        public TimesheetData PopulateByUserID(System.String sUserID, IDbConnection conn)
        {
            TimesheetData ds = null;
            try
            {
                string sSQL = "Select * "
                                + " FROM "
                                + clsPOSDBConstants.Timesheet_tbl
                                + " WHERE LTRIM(RTRIM(UPPER(" + clsPOSDBConstants.Timesheet_Fld_UserID + "))) = '" + sUserID.Trim().ToUpper() + "'";

                ds = new TimesheetData();
                ds.Timesheet.MergeTable(DataHelper.ExecuteDataset(conn, CommandType.Text, sSQL).Tables[0]);

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
                logger.Fatal(ex, "PopulateByUserID(System.String sUserID, IDbConnection conn)");  //PRIMEPOS-2971 07-Jun-2021 JY Added
                //ErrorHandler.throwException(ex, "", "");
            }

            return ds;
        }

        public TimesheetData PopulateList(System.String sWhereClause)
        {
            using (SqlConnection conn = new SqlConnection(POS_Core.Resources.Configuration.ConnectionString))
            {
                return PopulateList(sWhereClause, conn);
            }
        }

        public TimesheetData PopulateList(string sWhereClause, IDbConnection conn)
        {
            try
            {
                string sSQL = "Select * "
                    + " FROM "
                    + clsPOSDBConstants.Timesheet_tbl
                    + " WHERE "
                    + " 1=1 ";

                if (sWhereClause.Trim() != "")
                    sSQL = String.Concat(sSQL, sWhereClause);

                TimesheetData ds = new TimesheetData();
                ds.Timesheet.MergeTable(DataHelper.ExecuteDataset(conn, CommandType.Text, sSQL, whereParameters(sWhereClause)).Tables[0]);
                return ds;
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
                logger.Fatal(ex, "PopulateList(string sWhereClause, IDbConnection conn)");  //PRIMEPOS-2971 07-Jun-2021 JY Added
                                                                                            //ErrorHandler.throwException(ex,"",""); 
                return null;
            }
        }

        #endregion //Get Method

        #region Insert, Update, and Delete Methods
        public void Insert(DataSet ds, IDbTransaction tx, ref System.Int64 ID)
        {

            TimesheetTable addedTable = (TimesheetTable)ds.Tables[0].GetChanges(DataRowState.Added);
            string sSQL;
            IDbDataParameter[] insParam;

            if (addedTable != null && addedTable.Rows.Count > 0)
            {
                foreach (TimesheetRow row in addedTable.Rows)
                {
                    try
                    {
                        insParam = InsertParameters(row);
                        sSQL = BuildInsertSQL(clsPOSDBConstants.Timesheet_tbl, insParam);
                        for (int i = 0; i < insParam.Length; i++)
                        {
                            Console.WriteLine(insParam[i].ParameterName + "  " + insParam[i].Value);
                        }
                        DataHelper.ExecuteNonQuery(tx, CommandType.Text, sSQL, insParam);
                        ID = Convert.ToInt32(DataHelper.ExecuteScalar(tx, CommandType.Text, "select @@identity"));
                    }
                    catch (POSExceptions ex)
                    {
                        throw (ex);
                    }

                    catch (OtherExceptions ex)
                    {
                        throw (ex);
                    }
                    catch (ConstraintException)
                    {
                        //cExp.
                    }
                    catch (Exception ex)
                    {
                        logger.Fatal(ex, "Insert(DataSet ds, IDbTransaction tx, ref System.Int64 ID)");  //PRIMEPOS-2971 07-Jun-2021 JY Added
                                                                                                         //ErrorHandler.throwException(ex,"","");
                    }
                }
                addedTable.AcceptChanges();
            }
        }

        public void Update(DataSet ds, IDbTransaction tx, ref System.Int64 ID)
        {
            TimesheetTable modifiedTable = (TimesheetTable)ds.Tables[0].GetChanges(DataRowState.Modified);

            string sSQL;
            IDbDataParameter[] updParam;

            if (modifiedTable != null && modifiedTable.Rows.Count > 0)
            {
                foreach (TimesheetRow row in modifiedTable.Rows)
                {
                    try
                    {
                        updParam = UpdateParameters(row);
                        sSQL = BuildUpdateSQL(clsPOSDBConstants.Timesheet_tbl, updParam);

                        DataHelper.ExecuteNonQuery(tx, CommandType.Text, sSQL, updParam);

                        ID = row.ID;

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
                        logger.Fatal(ex, "Update(DataSet ds, IDbTransaction tx,ref System.Int64 ID)");  //PRIMEPOS-2971 07-Jun-2021 JY Added
                                                                                                        //ErrorHandler.throwException(ex,"","");
                    }
                }
                modifiedTable.AcceptChanges();
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
            //sInsertSQL = sInsertSQL + " , UserId ";
            sInsertSQL = sInsertSQL + " ) Values (" + delParam[0].ParameterName;

            for (int i = 1; i < delParam.Length; i++)
            {
                sInsertSQL = sInsertSQL + " , " + delParam[i].ParameterName;
            }
            //sInsertSQL = 	sInsertSQL + " , '" + Configuration.UserName + "'";
            sInsertSQL = sInsertSQL + " )";
            return sInsertSQL;
        }


        private string BuildUpdateSQL(string tableName, IDbDataParameter[] updParam)
        {
            string sUpdateSQL = "UPDATE " + tableName + " SET ";
            // build where clause
            sUpdateSQL = sUpdateSQL + updParam[1].SourceColumn + "  = " + updParam[1].ParameterName;

            for (int i = 2; i < updParam.Length; i++)
            {
                sUpdateSQL = sUpdateSQL + " , " + updParam[i].SourceColumn + "  = " + updParam[i].ParameterName;
            }

            //sUpdateSQL = sUpdateSQL + " , UserID  = '" + Configuration.UserName + "'" ;

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

        private IDbDataParameter[] PKParameters(System.Int64 ID)
        {
            IDbDataParameter[] sqlParams = DataFactory.CreateParameterArray(1);

            sqlParams[0] = DataFactory.CreateParameter();
            sqlParams[0].ParameterName = "@ID";
            sqlParams[0].DbType = System.Data.DbType.Int64;
            sqlParams[0].Value = ID;

            return (sqlParams);
        }

        private IDbDataParameter[] PKParameters(TimesheetRow row)
        {
            //return a SqlParameterCollection
            IDbDataParameter[] sqlParams = DataFactory.CreateParameterArray(1);


            sqlParams[0] = DataFactory.CreateParameter();
            sqlParams[0].ParameterName = "@ID";
            sqlParams[0].DbType = System.Data.DbType.Int64;

            sqlParams[0].Value = row.ID;
            sqlParams[0].SourceColumn = clsPOSDBConstants.Timesheet_Fld_ID;

            return (sqlParams);
        }

        private IDbDataParameter[] InsertParameters(TimesheetRow row)
        {
            IDbDataParameter[] sqlParams = DataFactory.CreateParameterArray(7);

            sqlParams[0] = DataFactory.CreateParameter("@" + clsPOSDBConstants.Timesheet_Fld_UserID, System.Data.DbType.String, clsPOSDBConstants.Timesheet_Fld_UserID, row.UserID);
            sqlParams[1] = DataFactory.CreateParameter("@" + clsPOSDBConstants.Timesheet_Fld_TimeIn, System.Data.DbType.DateTime, clsPOSDBConstants.Timesheet_Fld_TimeIn, row.TimeIn);
            sqlParams[2] = DataFactory.CreateParameter("@" + clsPOSDBConstants.Timesheet_Fld_TimeOut, System.Data.DbType.DateTime, clsPOSDBConstants.Timesheet_Fld_TimeOut, row.TimeOut);
            sqlParams[3] = DataFactory.CreateParameter("@" + clsPOSDBConstants.Timesheet_Fld_IsManualIn, System.Data.DbType.Boolean, clsPOSDBConstants.Timesheet_Fld_IsManualIn, row.IsManualTimeIn);
            sqlParams[4] = DataFactory.CreateParameter("@" + clsPOSDBConstants.Timesheet_Fld_IsManualOut, System.Data.DbType.Boolean, clsPOSDBConstants.Timesheet_Fld_IsManualOut, row.IsManualTimeOut);
            sqlParams[5] = DataFactory.CreateParameter("@" + clsPOSDBConstants.Timesheet_Fld_LastUpdatedBy, System.Data.DbType.String, clsPOSDBConstants.Timesheet_Fld_LastUpdatedBy, Configuration.UserName);
            sqlParams[6] = DataFactory.CreateParameter("@" + clsPOSDBConstants.Timesheet_Fld_LastUpdatedOn, System.Data.DbType.DateTime, clsPOSDBConstants.Timesheet_Fld_LastUpdatedOn, DateTime.Now);

            return (sqlParams);
        }

        private IDbDataParameter[] UpdateParameters(TimesheetRow row)
        {
            IDbDataParameter[] sqlParams = DataFactory.CreateParameterArray(8);

            sqlParams[0] = DataFactory.CreateParameter("@" + clsPOSDBConstants.Timesheet_Fld_ID, System.Data.DbType.Int64, clsPOSDBConstants.Timesheet_Fld_ID, row.ID);
            sqlParams[1] = DataFactory.CreateParameter("@" + clsPOSDBConstants.Timesheet_Fld_UserID, System.Data.DbType.String, clsPOSDBConstants.Timesheet_Fld_UserID, row.UserID);
            sqlParams[2] = DataFactory.CreateParameter("@" + clsPOSDBConstants.Timesheet_Fld_TimeIn, System.Data.DbType.DateTime, clsPOSDBConstants.Timesheet_Fld_TimeIn, row.TimeIn);
            sqlParams[3] = DataFactory.CreateParameter("@" + clsPOSDBConstants.Timesheet_Fld_TimeOut, System.Data.DbType.DateTime, clsPOSDBConstants.Timesheet_Fld_TimeOut, row.TimeOut);
            sqlParams[4] = DataFactory.CreateParameter("@" + clsPOSDBConstants.Timesheet_Fld_IsManualIn, System.Data.DbType.Boolean, clsPOSDBConstants.Timesheet_Fld_IsManualIn, row.IsManualTimeIn);
            sqlParams[5] = DataFactory.CreateParameter("@" + clsPOSDBConstants.Timesheet_Fld_IsManualOut, System.Data.DbType.Boolean, clsPOSDBConstants.Timesheet_Fld_IsManualOut, row.IsManualTimeOut);
            sqlParams[6] = DataFactory.CreateParameter("@" + clsPOSDBConstants.Timesheet_Fld_LastUpdatedBy, System.Data.DbType.String, clsPOSDBConstants.Timesheet_Fld_LastUpdatedBy, Configuration.UserName);
            sqlParams[7] = DataFactory.CreateParameter("@" + clsPOSDBConstants.Timesheet_Fld_LastUpdatedOn, System.Data.DbType.DateTime, clsPOSDBConstants.Timesheet_Fld_LastUpdatedOn, DateTime.Now);

            return (sqlParams);
        }
        #endregion

        #region Sprint-25 - PRIMEPOS-2253 24-Mar-2017 JY Added
        private DataSet GetProcessedTimesheetRecords(System.String sUserID, DateTime sStartDate, DateTime sEndDate)
        {
            DataSet dsProcesedTimesheet = new DataSet();
            string strSQL = "SELECT tIn.UserID, tIn.ID AS TimeInID, tOut.ID AS TimeOutID, tIn.EventDate as TimeIn, tOut.EventDate as TimeOut, tIn.isProcessed, a.Id AS TimesheetId FROM TimeSheet a " +
                        " INNER JOIN TimesheetEvents tIn ON UPPER(LTRIM(RTRIM(a.UserID))) = UPPER(LTRIM(RTRIM(tIn.UserID))) AND CONVERT(varchar, a.TimeIn,20) = CONVERT(varchar, tIn.EventDate, 20) AND tIn.isProcessed = 1 AND tIn.Status = 0 " +
                        " INNER JOIN TimesheetEvents tOut ON UPPER(LTRIM(RTRIM(a.UserID))) = UPPER(LTRIM(RTRIM(tOut.UserID))) AND CONVERT(varchar, a.TimeOut,20) = CONVERT(varchar, tOut.EventDate, 20) AND tOut.isProcessed = 1 AND tOut.Status = 1 " +
                        " WHERE(a.IsManualIn = 0 OR a.IsManualOut = 0)";

            if (sUserID.Trim() != string.Empty) strSQL += " AND LTRIM(RTRIM(UPPER(a.UserID))) = '" + sUserID.Trim().ToUpper() + "' ";

            strSQL += " AND tIn.EventDate between convert(datetime, cast('" + sStartDate.ToShortDateString() + " 00:00:00 ' as datetime),113) and convert(datetime, cast('" + sEndDate.ToShortDateString() + " 23:59:59 ' as datetime),113) " +
                        " ORDER BY tIn.UserID, tIn.ID, tIn.EventDate";

            using (SqlConnection conn = new SqlConnection(POS_Core.Resources.Configuration.ConnectionString))
            {
                dsProcesedTimesheet = DataHelper.ExecuteDataset(conn, CommandType.Text, strSQL);
            }
            return dsProcesedTimesheet;
        }
        #endregion

        public void Dispose() { }
    }
}
