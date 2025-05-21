using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using POS_Core.Resources;
using Resources;

namespace POS_Core_UI.UserManagement
{
    public class DBUser
    {
        public const string DefaultUserID = "PRIME";
        public const string UserPrefix = "MMS";

        //This method will create db user and will return true in case of success

        public static bool CreateDBUser(string sUserID, string sPassword)
        {
            return CreateDBUser(sUserID, sPassword, true);
        }

        public static bool DBUserExists(string sUserID)
        {
            SqlParameter[] oParam = new SqlParameter[1];
            oParam[0] = new SqlParameter("@LoginID", SqlDbType.VarChar, 50);
            oParam[0].Value = sUserID;

            string strSQL = " SELECT COUNT(*) FROM [MASTER].[DBO].[SYSLOGINS] WHERE LOGINNAME = @LoginID";
            int iUserExist = Convert.ToInt16(DataHelper.ExecuteScalar(strSQL, oParam));
            if (iUserExist == 0)
                return false;
            else
                return true;

        }
        //Added By Shitaljit(QuicSolv) on 10 August 2011
        //To check that the user is exixt or not in the current BD sysusers table
        public static bool DBUserExistsInCurrentDB(string sUserID)
        {
            SqlParameter[] oParam = new SqlParameter[1];
            oParam[0] = new SqlParameter("@LoginID", SqlDbType.VarChar, 50);
            oParam[0].Value = sUserID;

            string strSQL = " SELECT COUNT(*) FROM [" + POS_Core.Resources.Configuration.DatabaseName + "].[DBO].[SYSUSERS] WHERE NAME = @LoginID";
            int iUserExist = Convert.ToInt16(DataHelper.ExecuteScalar(strSQL, oParam));
            if (iUserExist == 0)
                return false;
            else
                return true;

        }
        //End of added by shitaljit

        //Following Code Added by Krishna on 1 August 2011
        public static void CheckForUsers()
        {
            DataSet DsUsers = GetAllUsersPassword();
            string User = "";
            string sPassword = ""; 
            foreach (DataRow datarow in DsUsers.Tables[0].Rows)
            {
                User = datarow[0].ToString().Replace("'", "''");    //PRIMEPOS-3095 16-May-2022 JY Modified
                sPassword = EncryptString.Decrypt(datarow[1].ToString()).Replace("'", "''");    //PRIMEPOS-3095 16-May-2022 JY Modified
                CreateDBUser(User, sPassword, true);
            }
        }
        //Till here added by Krishna on 1 August 2011

        public static bool CreateDBUser(string sUserID, string sPassword,bool bUsePrefix)
        {
            bool retVal = false;
            if (bUsePrefix)
                sUserID = UserPrefix+ sUserID.Trim();

            sPassword = sPassword.Trim();
            string strSQL = "";

            //oParam[2] = new SqlParameter("@Database", SqlDbType.VarChar, 50);
            //oParam[2].Value = Configuration.DatabaseName;

            //string strSQL = " SELECT COUNT(*) FROM [MASTER].[DBO].[SYSLOGINS] WHERE LOGINNAME = @LoginID";
            //int iUserExist = Convert.ToInt16(DataHelper.ExecuteScalar(strSQL, oParam));


            if (!DBUserExists(sUserID)) //(iUserExist == 0)
            {
                SqlParameter[] oParam = new SqlParameter[2];
                oParam[0] = new SqlParameter("@LoginID", SqlDbType.VarChar, 50);
                oParam[0].Value = sUserID;
                oParam[1] = new SqlParameter("@Password", SqlDbType.VarChar, 50);
                oParam[1].Value = sPassword;

                strSQL = "EXEC SP_EXECUTESQL N'CREATE LOGIN [" + sUserID + "] WITH PASSWORD=N''" + sPassword + "'', DEFAULT_DATABASE=[" + POS_Core.Resources.Configuration.DatabaseName + "], DEFAULT_LANGUAGE=[us_english], CHECK_EXPIRATION=OFF, CHECK_POLICY=OFF'";
                DataHelper.ExecuteNonQuery(strSQL, oParam);
                //Commented By Shitaljit(QuicSolv) on 10 August 2011
                //strSQL = "EXEC SP_EXECUTESQL N' CREATE USER [" + sUserID + "] FOR LOGIN [" + sUserID + "] WITH DEFAULT_SCHEMA=[dbo]'";
                //Addded By Shitaljit(QuicSolv) on 10 August 2011 Modified the commented queries and replaces DEFAULT_SCHEMA=[dbo] by DEFAULT_SCHEMA=[" + Configuration.DatabaseName + "]
                //To resolve the user alredy exist exception if user is present in some other DB in current DB server.
                //Following if condition is added by shitaljit to avoid exception while creating user if user is alredy exixt in the current BD sysusers table.
                if (!DBUserExistsInCurrentDB(sUserID))
                {
                    strSQL = "EXEC SP_EXECUTESQL N' CREATE USER [" + sUserID + "] FOR LOGIN [" + sUserID + "] WITH DEFAULT_SCHEMA=[" + POS_Core.Resources.Configuration.DatabaseName + "]'";
                    DataHelper.ExecuteNonQuery(strSQL, oParam);
                }
                strSQL = "EXEC sp_addrolemember 'db_owner', @LoginID";
                DataHelper.ExecuteNonQuery(strSQL, oParam);

                strSQL = "EXEC sp_addsrvrolemember @LoginID,'sysadmin' ";
                DataHelper.ExecuteNonQuery(strSQL, oParam);

                retVal = true;
            }
            else
            {
                //Following code is added By Shitaljit(QuicSolv) omn 11 August 2011
                //To handle the user present in Securutiy Login Table but missing from Catalog Secirity 
                SqlParameter[] oParam = new SqlParameter[2];
                oParam[0] = new SqlParameter("@LoginID", SqlDbType.VarChar, 50);
                oParam[0].Value = sUserID;
                oParam[1] = new SqlParameter("@Password", SqlDbType.VarChar, 50);
                oParam[1].Value = sPassword;

                if (!DBUserExistsInCurrentDB(sUserID))
                {
                    strSQL = "EXEC SP_EXECUTESQL N' CREATE USER [" + sUserID + "] FOR LOGIN [" + sUserID + "] WITH DEFAULT_SCHEMA=[" + POS_Core.Resources.Configuration.DatabaseName + "]'";
                    DataHelper.ExecuteNonQuery(strSQL, oParam);
                }

                strSQL = "EXEC sp_addrolemember 'db_owner', @LoginID";
                DataHelper.ExecuteNonQuery(strSQL, oParam);

                strSQL = "EXEC sp_addsrvrolemember @LoginID,'sysadmin' ";
                DataHelper.ExecuteNonQuery(strSQL, oParam);
              //Till here added By shitaljit.
               
                retVal = true;
            }
            return retVal;
        }

        //This method will remove db user and will return true in case of success
        public static bool DeleteDBUser(string sUserID)
        {
            sUserID = UserPrefix + sUserID;

            bool retVal = false;
            sUserID = sUserID.Trim();


            string strSQL = "";
            //strSQL = " SELECT COUNT(*) FROM [MASTER].[DBO].[SYSLOGINS] WHERE LOGINNAME = @LoginID";
            //int iUserExist = Convert.ToInt16(DataHelper.ExecuteScalar(strSQL, oParam));

            if (DBUserExists(sUserID)) //iUserExist != 0)
            {
                try
                {
                    SqlParameter[] oParam = new SqlParameter[1];
                    oParam[0] = new SqlParameter("@LoginID", SqlDbType.VarChar, 50);
                    oParam[0].Value = sUserID;

                    strSQL = "EXEC sp_dropuser  @LoginID";
                    DataHelper.ExecuteNonQuery(strSQL, oParam);

                    strSQL = "EXEC sp_droplogin  @LoginID";
                    DataHelper.ExecuteNonQuery(strSQL, oParam);

                    retVal = true;
                }
                catch (Exception exp)
                {
                    throw new Exception("Problem found while deleting user.\n" + exp.Message, exp);
                }
            }
            else
            {
                retVal = true;
            }
            return retVal;
        }

        //Added by Shitaljit(QuicSolv) on 14 Nov 2011
        //To Update User right to sysadmin
        public static bool UpdateUserRight(string sUserID)
        {
            bool retVal = false;
            string strSQL = "";
            try
            {
                SqlParameter[] oParam = new SqlParameter[1];
                oParam[0] = new SqlParameter("@LoginID", SqlDbType.VarChar, 50);
                oParam[0].Value = sUserID;

                if (!DBUserExistsInCurrentDB(sUserID))
                {
                    strSQL = "EXEC SP_EXECUTESQL N' CREATE USER [" + sUserID + "] FOR LOGIN [" + sUserID + "] WITH DEFAULT_SCHEMA=[" + POS_Core.Resources.Configuration.DatabaseName + "]'";
                    DataHelper.ExecuteNonQuery(strSQL, oParam);
                }
                strSQL = "EXEC sp_addrolemember 'db_owner', @LoginID";
                DataHelper.ExecuteNonQuery(strSQL, oParam);

                strSQL = "EXEC sp_addsrvrolemember @LoginID,'sysadmin' ";
                DataHelper.ExecuteNonQuery(strSQL, oParam);

                retVal = true;
            }

            catch (Exception exp)
            { 
                 throw new Exception("Problem found while updating user right.\n" + exp.Message, exp);
                
            }
   
            return retVal;
        }
        
        //This method will change db user password and will return true in case of success
        public static bool ChangeDBUserPwd(string sUserID, string sOldPassword, string sPassword)
        {
            sUserID = UserPrefix + sUserID;

            bool retVal = false;
            sUserID = sUserID.Trim();
            sPassword = sPassword.Trim();
            sOldPassword = sOldPassword.Trim();


            string strSQL = "";
            //strSQL = " SELECT COUNT(*) FROM [MASTER].[DBO].[SYSLOGINS] WHERE LOGINNAME = @LoginID";
            //int iUserExist = Convert.ToInt16(DataHelper.ExecuteScalar(strSQL, oParam));

            if (DBUserExists(sUserID)) //iUserExist != 0)
            {
                try
                {
                    SqlParameter[] oParam = new SqlParameter[3];
                    oParam[0] = new SqlParameter("@LoginID", SqlDbType.VarChar, 50);
                    oParam[0].Value = sUserID;
                    oParam[1] = new SqlParameter("@Password", SqlDbType.VarChar, 50);
                    oParam[1].Value = sPassword;
                    oParam[2] = new SqlParameter("@OldPassword", SqlDbType.VarChar, 50);
                    oParam[2].Value = sOldPassword;

                    strSQL = "exec sp_password @OldPassword , @Password , @LoginID";
                    DataHelper.ExecuteNonQuery(strSQL, oParam);
                    retVal = true;
                }
                catch (Exception exp)
                {
                    throw new Exception("Problem found while updating user.\n" + exp.Message, exp);
                }
            }
            else
            {
                throw new Exception("User does not exist in database.");
            }
            return retVal;
        }
        //this Function will unlock DB user selected
       // public static bool ChangeDBUserPwd(string sUserID)
        //{
        //    sUserID = UserPrefix + sUserID;

        //    bool retVal = false;
        //    sUserID = sUserID.Trim();
        //    sPassword = sPassword.Trim();
        //    sOldPassword = sOldPassword.Trim();


        //    string strSQL = "";
        //    //strSQL = " SELECT COUNT(*) FROM [MASTER].[DBO].[SYSLOGINS] WHERE LOGINNAME = @LoginID";
        //    //int iUserExist = Convert.ToInt16(DataHelper.ExecuteScalar(strSQL, oParam));

        //    if (DBUserExists(sUserID)) //iUserExist != 0)
        //    {
        //        try
        //        {
        //            SqlParameter[] oParam = new SqlParameter[3];
        //            oParam[0] = new SqlParameter("@LoginID", SqlDbType.VarChar, 50);
        //            oParam[0].Value = sUserID;
        //            strSQL = "exec sp_password @OldPassword , @Password , @LoginID";
        //            DataHelper.ExecuteNonQuery(strSQL, oParam);
        //            retVal = true;
        //        }
        //        catch (Exception exp)
        //        {
        //            throw new Exception("Problem found while updating user.\n" + exp.Message, exp);
        //        }
        //    }
        //    else
        //    {
        //        throw new Exception("User does not exist in database.");
        //    }
        //    return retVal;
        //}

        public static void UpdateAllUsersWithDBUsers()
        {
            POS_Core.ErrorLogging.Logs.Logger("Encrypt User Password", " UpdateAllUsersWithDBUsers()", "Enter Running from Station# " + POS_Core.Resources.Configuration.StationID );
            DataSet usersDS = GetAllUsersPassword();
            IDbTransaction oTrans = DataHelper.CreateTransaction();
            #region Validating Passwords were already encrypted or not
            foreach (DataRow row in usersDS.Tables[0].Rows)
            {
                string sUserID = POS_Core.Resources.Configuration.convertNullToString(row["UserID"].ToString());
                string sOriginalPWD = POS_Core.Resources.Configuration.convertNullToString(row["Password"].ToString());
                POS_Core.ErrorLogging.Logs.Logger("Encrypt User Password", "User: "+sUserID, " Current Password : " + sOriginalPWD);
                string sDecryptPWD = EncryptString.Decrypt(sOriginalPWD);
                POS_Core.ErrorLogging.Logs.Logger("Encrypt User Password", "User: "+sUserID, " Decrypted Password : " + sDecryptPWD);
                if (sOriginalPWD.Equals(sDecryptPWD) == false)
                {
                    POS_Core.ErrorLogging.Logs.Logger("Encrypt User Password", "Updating PWEncrypted = TRUE", " To databse");
                    UpdatePWEncryptedFlag(oTrans);
                    oTrans.Commit();
                    POS_Core.Resources.Configuration.CInfo.PWEncrypted = true;
                    POS_Core.ErrorLogging.Logs.Logger("Encrypt User Password", "Updating PWEncrypted = TRUE", " To databse completed");
                    POS_Core.ErrorLogging.Logs.Logger("Encrypt User Password", " UpdateAllUsersWithDBUsers()", "Running from Station# " + POS_Core.Resources.Configuration.StationID +  "Exiting As encryption logic were already implemented");
                    return;
                }
            }
            #endregion
            usersDS.Tables[0].Columns.Add("EncPassword", typeof(string));

            foreach (DataRow row in usersDS.Tables[0].Rows)
            {
                row["EncPassword"] = EncryptString.Encrypt(row["Password"].ToString());
            }

            try
            {
                CreateMMSUser(oTrans);

                foreach (DataRow row in usersDS.Tables[0].Rows)
                {
                    UpdateUserPassword(row["UserID"].ToString(), row["EncPassword"].ToString(), oTrans);

                    CreateDBUser(row["UserID"].ToString().Replace("'", "''"), row["Password"].ToString().Replace("'", "''"));   //PRIMEPOS-3095 16-May-2022 JY Modified
                }

                UpdatePWEncryptedFlag(oTrans);

                oTrans.Commit();
                POS_Core.Resources.Configuration.CInfo.PWEncrypted = true;
            }
            catch
            {
                POS_Core.ErrorLogging.Logs.Logger("Encrypt User Password", " UpdateAllUsersWithDBUsers()", "Error Occured, Running from Station# " + POS_Core.Resources.Configuration.StationID );
                oTrans.Rollback();
                throw;
            }
            finally
            {

            }
            POS_Core.ErrorLogging.Logs.Logger("Encrypt User Password", " UpdateAllUsersWithDBUsers()", "Exited, Running from Station# " + POS_Core.Resources.Configuration.StationID );
        }

        public static void CreateMMSUser(IDbTransaction oTrans)
        {
            clsLogin oLogin=new clsLogin();
            IDataReader reader;

            reader = oLogin.GetUserByID(DefaultUserID, oTrans);

            try
            {
                if (reader.Read() == true)
                {
                    CreateDBUser(DefaultUserID.Replace("'", "''"), EncryptString.Decrypt(reader.GetString(reader.GetOrdinal(POS_Core.CommonData.clsPOSDBConstants.Users_Fld_Password))).Replace("'", "''"));    //PRIMEPOS-3095 16-May-2022 JY Modified
                }
                else
                {
                    frmUserInformation ofrm = new frmUserInformation();
                    ofrm.CreateUserWithAllPermissions(DefaultUserID, "asmms$");
                    ofrm.Dispose();
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                if (reader.IsClosed == false)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
        }

        private static void UpdatePWEncryptedFlag(IDbTransaction oTrans)
        {
            string strSQL = "Update Util_Company_Info Set PWEncrypted=1";

            DataHelper.ExecuteNonQuery(oTrans, CommandType.Text, strSQL);
        }

        private static void UpdateUserPassword(string sUserID, string sPassword, IDbTransaction oTrans)
        {
            string strSQL = "Update Users Set Password=@Password where UserID=@UserID";

            SqlParameter[] oParam = new SqlParameter[2];
            oParam[0] = new SqlParameter("@UserID", SqlDbType.VarChar, 50);
            oParam[0].Value = sUserID;
            oParam[1] = new SqlParameter("@Password", SqlDbType.VarChar, 50);
            oParam[1].Value = sPassword;

            DataHelper.ExecuteNonQuery(oTrans, CommandType.Text, strSQL, oParam);
        }

        private static DataSet GetAllUsersPassword()
        {
            POS_Core.DataAccess.SearchSvr oSearch = new POS_Core.DataAccess.SearchSvr();
            return oSearch.Search("select UserID,Password from users WHERE UserType is null OR UserType <> 'G'");
        }

        public static string CheckandCreateUser(string sUserID, string sPassword)
        {
            bool crResult = false;
            bool upResult = false;//added by Shitaljit(QuicSolv) on 14 Nov 2011
            POS_Core.Resources.Configuration.ConnectionStringType="sa";
            POS_Core.Resources.Configuration.m_ConnString = "";
            bool bUserexists=DBUser.DBUserExists(sUserID);
            if (!bUserexists)
                crResult = CreateDBUser(sUserID, sPassword, false);

            upResult = UpdateUserRight(sUserID);//added by Shitaljit(QuicSolv) on 14 Nov 2011

            POS_Core.Resources.Configuration.m_ConnString = "";
            POS_Core.Resources.Configuration.ConnectionStringType = "UserDatabase";
            if (bUserexists)
                return "A"; //already exists
            else
            {
                if (crResult)
                    return "Y";
                else
                    return "N";
            }
        }

        public static void CreateMMSUserS()
        {
            POS_Core.Resources.Configuration.ConnectionStringType = "sa";
            POS_Core.Resources.Configuration.m_ConnString = "";
            IDbTransaction oTrans = DataHelper.CreateTransaction();
            try
            {
                CreateMMSUser(oTrans);
                oTrans.Commit();
            }
            catch
            {
                oTrans.Rollback();
                throw;
            }
            finally
            {
                oTrans = null;                
            }

            POS_Core.Resources.Configuration.m_ConnString = "";
            POS_Core.Resources.Configuration.ConnectionStringType = "UserDatabase";

        }

        #region PRIMEPOS-2576 28-Aug-2018 JY Added
        public DataTable LoadUserFingerPrint(string UserID)
        {
            using (IDbConnection conn = DataFactory.CreateConnection(POS_Core.Resources.Configuration.ConnectionString))
            {
                string strSQL = "SELECT * FROM User_Biometrics WHERE UserID IN (SELECT ID FROM Users WHERE UserID = '" + UserID + "') ORDER BY FingerIndex DESC";

                DataTable dt = DataHelper.ExecuteDataTable(conn, CommandType.Text, strSQL, (IDbDataParameter[])null);
                return dt;
            }
        }

        public DataTable LoadAllFingerPrint()
        {
            using (IDbConnection conn = DataFactory.CreateConnection(POS_Core.Resources.Configuration.ConnectionString))
            {
                string strSQL = "SELECT UB.FingerIndex, UB.Fingerprint, UB.Type, UB.LastUpdated, U.ID, U.UserID, LTRIM(RTRIM(ISNULL(U.LNAME,'') + ' ' + ISNULL(U.FName,''))) AS UserName FROM User_Biometrics UB LEFT JOIN Users U ON U.ID = UB.UserID ORDER BY UB.ID";

                DataTable dt = DataHelper.ExecuteDataTable(conn, CommandType.Text, strSQL, (IDbDataParameter[])null);
                return dt;
            }
        }

        public void SaveUserFingerPrint(string UserId, string fingerIndex, string sFMD)
        {
            IDbTransaction oTrans = DataHelper.CreateTransaction();
            try
            {
                string strSQL = "INSERT INTO User_Biometrics(UserID,FingerIndex,Fingerprint,Type,LastUpdated) VALUES (" + UserId + ",'" + fingerIndex + "','" + sFMD + "','F', GETDATE())";
                DataHelper.ExecuteNonQuery(oTrans, CommandType.Text, strSQL);
                oTrans.Commit();
            }
            catch
            {
                oTrans.Rollback();
                throw;
            }
            finally
            {
            }
        }

        public void UpdateUserFingerPrint(string UserId, string origFingerIndex, string newFingerIndex, string sFMD)
        {
            IDbTransaction oTrans = DataHelper.CreateTransaction();
            try
            {
                string strSQL = "UPDATE User_Biometrics SET FingerIndex = '" + newFingerIndex + "', Fingerprint = '" + sFMD + "', LastUpdated = GETDATE() WHERE UserID = " + UserId + " AND FingerIndex = '" + origFingerIndex + "'";
                DataHelper.ExecuteNonQuery(oTrans, CommandType.Text, strSQL);
                oTrans.Commit();
            }
            catch
            {
                oTrans.Rollback();
                throw;
            }
            finally
            {
            }
        }

        public void DeleteUserFingerPrint(string UserId)
        {
            IDbTransaction oTrans = DataHelper.CreateTransaction();
            try
            {
                string strSQL = "DELETE FROM User_Biometrics WHERE UserID = " + UserId;
                DataHelper.ExecuteNonQuery(oTrans, CommandType.Text, strSQL);
                oTrans.Commit();
            }
            catch
            {
                oTrans.Rollback();
                throw;
            }
            finally
            {
            }
        }        

        public DataTable GetUserDetails(string UserID)
        {
            using (IDbConnection conn = DataFactory.CreateConnection(POS_Core.Resources.Configuration.ConnectionString))
            {
                string strSQL = "SELECT ID, UserID, Password, LTRIM(RTRIM(ISNULL(LNAME,'') + ' ' + ISNULL(FName,''))) AS UserName, IsActive, ISLOCKED FROM Users WHERE UserID = '" + UserID + "'";

                DataTable dt = DataHelper.ExecuteDataTable(conn, CommandType.Text, strSQL, (IDbDataParameter[])null);
                return dt;
            }
        }
        #endregion

        //PRIMEPOS-2616 14-Dec-2018 JY Added
        public DataTable GetUserByWindowsLoginId(string WindowsLoginId)
        {
            using (IDbConnection conn = DataFactory.CreateConnection(POS_Core.Resources.Configuration.ConnectionString))
            {
                string strSQL = "SELECT UserID, Password FROM Users WHERE IsActive = 1 AND WindowsLoginId = '" + WindowsLoginId.Replace("'","''") + "'";

                DataTable dt = DataHelper.ExecuteDataTable(conn, CommandType.Text, strSQL, (IDbDataParameter[])null);
                return dt;
            }
        }

        #region PRIMEPOS-2989 13-Aug-2021 JY Added
        public DataTable GetUserIDFromSAMLResponse(string authInfo)
        {
            try
            {
                DataTable dt = null;
                string[] authDataArray = authInfo.Split(";".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                Dictionary<string, string> authDataDic = new Dictionary<string, string>();
                foreach (string s in authDataArray)
                {
                    authDataDic.Add(s.Substring(0, s.IndexOf(":")), s.Substring(s.IndexOf(":") + 1));
                }
                if (POS_Core.Resources.Configuration.CInfo.SSOIdentifier == (int)POS_Core.Resources.SSOIdentifier.LanID)  //PRIMEPOS-3484
                    dt = GetUserIDForSSOLanID(authDataDic["NetworkID"]);
                else
                    dt = GetUserIDForSSOEmailID(authDataDic["name"]);
                return dt;
            }
            catch (Exception ex)
            {
                clsUIHelper.ShowErrorMsg(ex.Message);
                return null;
            }
        }

        public DataTable GetUserIDForSSOEmailID(string emailID)
        {
            try
            {
                using (IDbConnection conn = DataFactory.CreateConnection(POS_Core.Resources.Configuration.ConnectionString))
                {
                    string strSQL = "SELECT UserID, Password, IsActive FROM Users WITH (NOLOCK) WHERE EmailID = '" + emailID.Replace("'", "''") + "'";

                    DataTable dt = DataHelper.ExecuteDataTable(conn, CommandType.Text, strSQL, (IDbDataParameter[])null);
                    return dt;
                }
            }
            catch (Exception ex)
            {
                clsUIHelper.ShowErrorMsg(ex.Message);
                return null;
            }
        }
        #endregion
        #region PRIMEPOS-3484
        public DataTable GetUserIDForSSOLanID(string lanID)
        {
            try
            {
                using (IDbConnection conn = DataFactory.CreateConnection(POS_Core.Resources.Configuration.ConnectionString))
                {
                    string strSQL = "SELECT UserID, Password, IsActive FROM Users WITH (NOLOCK) WHERE LanID = '" + lanID.Replace("'", "''") + "'";

                    DataTable dt = DataHelper.ExecuteDataTable(conn, CommandType.Text, strSQL, (IDbDataParameter[])null);
                    return dt;
                }
            }
            catch (Exception ex)
            {
                clsUIHelper.ShowErrorMsg(ex.Message);
                return null;
            }
        }
        #endregion
    }
}
