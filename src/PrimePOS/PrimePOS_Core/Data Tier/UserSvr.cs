using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using POS_Core.CommonData.Tables;
using POS_Core.CommonData.Rows;
using POS_Core.CommonData;
//using Resources;
using System.Windows.Forms;
using POS_Core.ErrorLogging;
////using POS.Resources;
using POS_Core.Resources;
using Resources;
using POS_Core.Resources.DelegateHandler;

namespace POS_Core.DataAccess
{
    
    // Provides data access methods for User

    public class UserSvr:IDisposable
    {

        #region Persist Methods

        // Inserts, updates or deletes rows in a DataSet, within a database transaction.
        public void Persist(DataSet updates, SqlTransaction tx)
        {
            try
            {
                //this.Delete(updates, tx);
                this.Insert(updates, tx);
                this.Update(updates, tx);

                updates.AcceptChanges();
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
                POS_Core.ErrorLogging.ErrorHandler.throwException(ex, "", "");
            }
        }
        
        // Inserts, updates or deletes rows in a DataSet.
        public void Persist(DataSet updates)
        {

            SqlTransaction tx;
            SqlConnection conn = new SqlConnection(DBConfig.ConnectionString);

            conn.Open();
            tx = conn.BeginTransaction();
            try
            {
                this.Persist(updates, tx);
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
                ErrorHandler.throwException(ex, "", "");
            }

        }

        public bool DeleteRow(string UserID)
        {
            string sSQL;
            try
            {
                sSQL = " delete from User where UserID= '" + UserID + "'";
                DataHelper.ExecuteNonQuery(DBConfig.ConnectionString, CommandType.Text, sSQL);
                return true;
            }
            catch (Exception ex)
            {
                ErrorHandler.throwException(ex, "", "");
                return false;
            }
        }

        #endregion

        #region Get Methods

        
        public virtual UserData Populate(SqlConnection conn)
        {
            try
            {
                string sSQL = "";
                sSQL = "Select * FROM " + clsPOSDBConstants.Users_tbl ;

                UserData ds = new UserData();
                ds.User.MergeTable(DataHelper.ExecuteDataset(conn, CommandType.Text, sSQL).Tables[0]);
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
                ErrorHandler.throwException(ex, "", "");
                return null;
            }
        }

        public UserData Populate()
        {
            using (SqlConnection conn = new SqlConnection(DBConfig.ConnectionString))
            {
                return (PopulateList(string.Empty));
            }
        }

        public UserData GetUserByUserID(System.String UserID)
        {
            using (SqlConnection conn = new SqlConnection(DBConfig.ConnectionString))
            {
                string sSQL = "Select * FROM " + clsPOSDBConstants.Users_tbl + " WHERE " + clsPOSDBConstants.Users_Fld_UserID + " ='" + UserID.ToString() + "' ";

                UserData ds = new UserData();
                ds.User.MergeTable(DataHelper.ExecuteDataset(conn, CommandType.Text, sSQL, PKParameters(UserID)).Tables[0]);
                return ds;
            }
        }

        #region PRIMEPOS-2780 27-Sep-2021 JY Added
        public DataTable GetUserByID(System.Int32 ID)
        {
            using (SqlConnection conn = new SqlConnection(DBConfig.ConnectionString))
            {
                string sSQL = "Select * FROM " + clsPOSDBConstants.Users_tbl + " WHERE " + clsPOSDBConstants.Users_Fld_ID + " = " + ID;

                DataTable dt = DataHelper.ExecuteDataTable(conn, CommandType.Text, sSQL, null);
                return dt;
            }
        }
        #endregion

        public DataSet GetUserGroup(System.String UserID, System.String UserFName)
        {
            using (SqlConnection conn = new SqlConnection(DBConfig.ConnectionString))
            {
                string sSQL = "SELECT Userid, FName FROM " + clsPOSDBConstants.Users_tbl + " WHERE UserType = 'G' ";    //PRIMEPOS-2780 27-Sep-2021 JY modified
                if (string.IsNullOrEmpty(UserID) == false)
                {
                    sSQL += " AND " + clsPOSDBConstants.Users_Fld_UserID + " LIKE('" + UserID.Trim() + "%')";
                }
                if (string.IsNullOrEmpty(UserFName) == false)
                {
                    sSQL += "AND " + clsPOSDBConstants.Users_Fld_fName + " LIKE('" + UserFName.Trim() + "%')";
                }

                DataSet ds = DataHelper.ExecuteDataset(conn, CommandType.Text, sSQL);
                return ds;
            }
        }

        // Fills a UserData with all User
        public UserData PopulateList(string sWhereClause, SqlConnection conn)
        {
            try
            {
                string sSQL = String.Concat("SELECT * FROM ", clsPOSDBConstants.Users_tbl, sWhereClause);

                UserData ds = new UserData();
                ds.User.MergeTable(DataHelper.ExecuteDataset(conn, CommandType.Text, sSQL).Tables[0]);
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
                ErrorHandler.throwException(ex, "", "");
                return null;
            }
        }

        public UserData PopulateList(string whereClause)
        {
            using (SqlConnection conn = new SqlConnection(DBConfig.ConnectionString))
            {
                return (PopulateList(whereClause, conn));
            }
        }

        #endregion //Get Method

        #region Insert, Update, and Delete Methods

        public void Insert(DataSet ds, SqlTransaction tx)
        {

            string sSQL;
            IDbDataParameter[] insParam;

            foreach (DataRow row in ds.Tables[0].Rows)
            {
                try
                {
                    if (row.RowState == DataRowState.Added)
                    {
                        insParam = InsertParameters((UserRow)row);
                        sSQL = BuildInsertSQL(clsPOSDBConstants.Users_tbl, insParam);
                        for (int i = 0; i < insParam.Length; i++)
                        {
                            Console.WriteLine(insParam[i].ParameterName + "  " + insParam[i].Value);
                        }

                        DataHelper.ExecuteNonQuery(tx, CommandType.Text, sSQL, insParam);
                        System.Int32 UserID = Convert.ToInt32(DataHelper.ExecuteScalar(tx, CommandType.Text, "select @@identity"));

                        row.AcceptChanges();
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
                    if (ex.Number == 2627)
                        ErrorHandler.throwCustomError(POSErrorENUM.User_Duplicate);
                    else
                        throw (ex);

                }

                catch (Exception ex)
                {
                    ErrorHandler.throwException(ex, "", "");
                }

            }
            //addedTable.AcceptChanges();
            //}		
        }

        // Update all rows in a User DataSet, within a given database transaction.
        public void Update(DataSet ds, SqlTransaction tx)
        {
            UserTable modifiedTable = (UserTable)ds.Tables[clsPOSDBConstants.Users_tbl].GetChanges(DataRowState.Modified);

            string sSQL;
            IDbDataParameter[] updParam;

            if (modifiedTable != null && modifiedTable.Rows.Count > 0)
            {
                foreach (UserRow row in modifiedTable.Rows)
                {
                    try
                    {
                        updParam = UpdateParameters(row);
                        sSQL = BuildUpdateSQL(clsPOSDBConstants.Users_tbl, updParam);

                        DataHelper.ExecuteNonQuery(tx, CommandType.Text, sSQL, updParam);
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
                        if (ex.Number == 2627)
                            ErrorHandler.throwCustomError(POSErrorENUM.User_Duplicate);
                        else
                            throw (ex);
                    }


                    catch (Exception ex)
                    {
                        ErrorHandler.throwException(ex, "", "");
                    }
                }
                modifiedTable.AcceptChanges();
            }
        }

        //Added By Shitaljit(QuicSolv) on 23 May 2011
        /// <summary>
        /// this function will unlocked the User 
        /// it will set NOOFATTEMPT to 0 and ISLOCKED to false
        /// </summary>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public bool UnlockUserRow(string UserID)
        {
            string sSQL;
            try
            {
                sSQL = " Update Users SET NOOFATTEMPT = 0 , ISLOCKED = '" + false + "' WHERE UserID= '" + UserID + "'";
                DataHelper.ExecuteNonQuery(DBConfig.ConnectionString, CommandType.Text, sSQL);
                return true;
            }
            catch (Exception ex)
            {
                ErrorHandler.throwException(ex, "", "");
                return false;
            }
        }
        public bool resetUserPassword(string UserID)
        {
            string sSQL;
            try
            {
                sSQL = " Update Users SET Password = 'POS' WHERE UserID= '" + UserID + "'";
                DataHelper.ExecuteNonQuery(DBConfig.ConnectionString, CommandType.Text, sSQL);
                return true;
            }
            catch (Exception ex)
            {
                ErrorHandler.throwException(ex, "", "");
                return false;
            }
        }
        //Till here added by Shitaljit

        private string BuildInsertSQL(string tableName, IDbDataParameter[] delParam)
        {
            string sInsertSQL = "Insert Into " + tableName + " ( ";
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

        private string BuildUpdateSQL(string tableName, IDbDataParameter[] updParam)
        {
            string sUpdateSQL = "UPDATE " + tableName + " SET ";
            // build where clause
            sUpdateSQL = sUpdateSQL + updParam[1].SourceColumn + "  = " + updParam[1].ParameterName;

            for (int i = 2; i < updParam.Length; i++)
            {
                sUpdateSQL = sUpdateSQL + " , " + updParam[i].SourceColumn + "  = " + updParam[i].ParameterName;

            }
            sUpdateSQL = sUpdateSQL + " WHERE " + updParam[0].SourceColumn + " = " + updParam[0].ParameterName;
            return sUpdateSQL;
        }

        #endregion

        #region IDBDataParameter Generator Methods

        private IDbDataParameter[] PKParameters(System.String UserID)
        {
            IDbDataParameter[] sqlParams = DataFactory.CreateParameterArray(1);


            sqlParams[0] = DataFactory.CreateParameter();
            sqlParams[0].ParameterName = "@UserID";
            sqlParams[0].DbType = System.Data.DbType.String;
            sqlParams[0].Value = UserID;

            return (sqlParams);
        }

        private IDbDataParameter[] PKParameters(UserRow row)
        {
            //return a SqlParameterCollection
            IDbDataParameter[] sqlParams = DataFactory.CreateParameterArray(1);


            sqlParams[0] = DataFactory.CreateParameter();
            sqlParams[0].ParameterName = "@Userid";
            sqlParams[0].DbType = System.Data.DbType.String;

            sqlParams[0].Value = row.UserID.ToString().Trim();
            sqlParams[0].SourceColumn = clsPOSDBConstants.Users_Fld_UserID;

            return (sqlParams);
        }

        private IDbDataParameter[] InsertParameters(UserRow row)
        {

            IDbDataParameter[] sqlParams = DataFactory.CreateParameterArray(6);

            sqlParams[0] = DataFactory.CreateParameter("@" + clsPOSDBConstants.Users_Fld_UserID, System.Data.DbType.String);
            sqlParams[1] = DataFactory.CreateParameter("@" + clsPOSDBConstants.Users_Fld_Password, System.Data.DbType.String);
            sqlParams[2] = DataFactory.CreateParameter("@" + clsPOSDBConstants.Users_Fld_fName, System.Data.DbType.String);
            sqlParams[3] = DataFactory.CreateParameter("@" + clsPOSDBConstants.Users_Fld_lName, System.Data.DbType.String);
            sqlParams[4] = DataFactory.CreateParameter("@" + clsPOSDBConstants.Users_Fld_IsActive, System.Data.DbType.Boolean);
            sqlParams[5] = DataFactory.CreateParameter("@" + clsPOSDBConstants.Users_Fld_DrawNo, System.Data.DbType.Int32);

            sqlParams[0].SourceColumn = clsPOSDBConstants.Users_Fld_UserID;
            sqlParams[1].SourceColumn = clsPOSDBConstants.Users_Fld_Password;
            sqlParams[2].SourceColumn = clsPOSDBConstants.Users_Fld_fName;
            sqlParams[3].SourceColumn = clsPOSDBConstants.Users_Fld_lName;
            sqlParams[4].SourceColumn = clsPOSDBConstants.Users_Fld_IsActive;
            sqlParams[5].SourceColumn = clsPOSDBConstants.Users_Fld_DrawNo;
            
            sqlParams[0].Value = row.UserID;
            sqlParams[1].Value = row.Password.Trim();
            sqlParams[2].Value = row.FirstName;
            sqlParams[3].Value = row.LastName.Trim();
            sqlParams[4].Value = row.IsActive;

            if (row.DrawNo != System.Int32.MinValue)
                sqlParams[5].Value = row.DrawNo;
            else
                sqlParams[5].Value = 0;

            
            return (sqlParams);
        }
        private IDbDataParameter[] UpdateParameters(UserRow row)
        {
            IDbDataParameter[] sqlParams = DataFactory.CreateParameterArray(7);

            sqlParams[0] = DataFactory.CreateParameter("@" + clsPOSDBConstants.Users_Fld_ID, System.Data.DbType.Int32);
            sqlParams[1] = DataFactory.CreateParameter("@" + clsPOSDBConstants.Users_Fld_UserID, System.Data.DbType.String);
            sqlParams[2] = DataFactory.CreateParameter("@" + clsPOSDBConstants.Users_Fld_Password, System.Data.DbType.String);
            sqlParams[3] = DataFactory.CreateParameter("@" + clsPOSDBConstants.Users_Fld_fName, System.Data.DbType.String);
            sqlParams[4] = DataFactory.CreateParameter("@" + clsPOSDBConstants.Users_Fld_lName, System.Data.DbType.String);
            sqlParams[5] = DataFactory.CreateParameter("@" + clsPOSDBConstants.Users_Fld_IsActive, System.Data.DbType.Boolean);
            sqlParams[6] = DataFactory.CreateParameter("@" + clsPOSDBConstants.Users_Fld_DrawNo, System.Data.DbType.Int32);

            sqlParams[0].SourceColumn = clsPOSDBConstants.Users_Fld_ID;
            sqlParams[1].SourceColumn = clsPOSDBConstants.Users_Fld_UserID;
            sqlParams[2].SourceColumn = clsPOSDBConstants.Users_Fld_Password;
            sqlParams[3].SourceColumn = clsPOSDBConstants.Users_Fld_fName;
            sqlParams[4].SourceColumn = clsPOSDBConstants.Users_Fld_lName;
            sqlParams[5].SourceColumn = clsPOSDBConstants.Users_Fld_IsActive;
            sqlParams[6].SourceColumn = clsPOSDBConstants.Users_Fld_DrawNo;

            sqlParams[0].Value = row.Id;
            sqlParams[1].Value = row.UserID;
            sqlParams[2].Value = row.Password.Trim();
            sqlParams[3].Value = row.FirstName;
            sqlParams[4].Value = row.LastName.Trim();
            sqlParams[5].Value = row.IsActive;

            if (row.DrawNo != System.Int32.MinValue)
                sqlParams[6].Value = row.DrawNo;
            else
                sqlParams[6].Value = 0;

            return (sqlParams);
        }

        #endregion

        /// <summary>
        /// Dispose User contents.
        /// </summary>
        public void Dispose()
        {
            GC.SuppressFinalize(true);
        }

        public bool ChangeUserPassword(string Userid, string OldPassword, string NewPassword)
        {
            //string strSQL = "update users set password=@password, passwordchangedon=@passwordchangedon where userid=@userID";
            #region PRIMEPOS-3129 22-Aug-2022 JY Added ChangePasswordAtLogin
            string strSQL = string.Empty;
            if (Configuration.convertNullToBoolean(Configuration.CSetting.ResetPwdForceUserToChangePwd) == true)
                strSQL = "update users set ChangePasswordAtLogin = 1, password=@password, passwordchangedon=@passwordchangedon where userid=@userID";
            else
                strSQL = "update users set password=@password, passwordchangedon=@passwordchangedon where userid=@userID";
            #endregion

            System.Data.IDbDataParameter[] oparm = DataFactory.CreateParameterArray(3);
            oparm[0] = DataFactory.CreateParameter();
            oparm[0].ParameterName = "@password";
            string srtTemp = EncryptString.Encrypt(NewPassword);
            oparm[0].Value = srtTemp;
            oparm[0].DbType = System.Data.DbType.String;

            oparm[1] = DataFactory.CreateParameter();
            oparm[1].ParameterName = "@userid";
            oparm[1].Value = Userid;
            oparm[1].DbType = System.Data.DbType.String;

            oparm[2] = DataFactory.CreateParameter();
            oparm[2].ParameterName = "@passwordchangedon";
            oparm[2].Value = System.DateTime.Now;
            oparm[2].DbType = System.Data.DbType.DateTime;

            //Changes the order of query update User table firts then DB login table.
            if (DataHelper.ExecuteNonQuery(DataFactory.CreateConnection(DBConfig.ConnectionString), System.Data.CommandType.Text, strSQL, oparm) > 0)
            {
                clsCoreDBuser.ChangeDBUserPwd(Userid, OldPassword, NewPassword);

                //To Handle same user updating own Password.
                if (Userid == Configuration.UserName)
                {
                    Configuration.SQLUserID = "MMS" + Userid;
                    Configuration.SQLUserPassword = NewPassword;
                    Configuration.m_ConnString = string.Empty;
                }

                ErrorHandler.SaveLog((int)LogENUM.Change_Password, Userid, "Success", "Password changed successfully");

                return true;
            }
            else
                return false;


        }

        
    }


    //Following Class Added by Krishna on 12 October 2011
    public class DbBackupSvr : IDisposable
    {
        #region Persist Methods
        public void Persist(string Query)
        { }
        # endregion

        #region Get Methods
        public DateTime? GetLastBackupDate()
        {
            string sSQL = "Select MAX(" + clsPOSDBConstants.DBBackuplog_BackupDate + ") as LastBackupDate FROM " + clsPOSDBConstants.DBBackuplog_tbl;
            DateTime? LastBackupDate = null;
            object Val =DataHelper.ExecuteScalar(sSQL);
            if (Val.ToString() == "")
                return LastBackupDate;
            else
                LastBackupDate = (DateTime)Val;
            return LastBackupDate;
        }
        # endregion

        #region IDisposable Members
        public void Dispose()
        {
        }
        #endregion
    }
    //Till here Added by Krishna on 12 October 2011
}
