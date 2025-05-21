using System;
using System.Data.SqlClient;
using System.Data;
using System.Windows.Forms;
using Infragistics.Win;
using POS_Core.ErrorLogging;
using POS_Core.CommonData;
using POS_Core_UI.Resources;
using System.Timers;
using System.Threading;
using System.Collections.Generic;
using POS_Core_UI.UI;
//using Resources;
using POS_Core.Resources;
using POS_Core.Resources.DelegateHandler;
using Resources;

namespace POS_Core_UI.UserManagement
{
	/// <summary>
	/// Summary description for clsLogin.
	/// </summary>
	public class clsLogin
	{
		private string m_ConnString;
		private bool m_IsCanceled;
//		private bool mSecurityLevel;
		private static frmLogin ofrmLogin;
        private int iMaxAttempt;
        private object lockObj;
        string sUserName = string.Empty;
        //Timer
        private System.Timers.Timer loginAttemptTimer = null;
        static Dictionary<string, DateTime> dictLoginFail = new Dictionary<string, DateTime>();

		public clsLogin()
		{
            iMaxAttempt = 1;
            lockObj = new object();
            InitializeLoginAttemptTimer();
            StartTimer(this.loginAttemptTimer);
		}

		public string ConnString
		{
			set { this.m_ConnString = value; }
		}

        /// <summary>
        /// Added strWindowTitle parameter by shitaljit to allow users to set the title if required.
        /// </summary>
        /// <param name="strPrevlige"></param>
        /// <param name="sUserID"></param>
        /// <param name="strWindowTitle"></param>
        /// <returns></returns>
		public bool loginForPreviliges(string strPrevlige, string strPermission, out string sUserID, string strWindowTitle)
		{
            POS_Core.ErrorLogging.Logs.Logger(clsPOSDBConstants.Log_Module_Login, " loginForPreviliges()", clsPOSDBConstants.Log_Entering);
            sUserID = "";
			ofrmLogin = frmLogin.GetInstance();
			try
			{
				ofrmLogin.oclsLogin = this;
                if (string.IsNullOrEmpty(strWindowTitle) == false)
                {
                    ofrmLogin.Text = strWindowTitle;
                }
                else
                {
                    ofrmLogin.Text = "Security";
                }
				ofrmLogin.txtUserName.Text = "";
				//ofrmLogin.btnCancel.Enabled = false;
				//ofrmLogin.ControlBox = false;
				ofrmLogin.txtPassward.Text = "";
				ofrmLogin.strPrevlige=strPrevlige;
                ofrmLogin.strPermission = strPermission;
                if (strPrevlige.Trim().ToUpper() == clsPOSDBConstants.UserMaxTransactionLimit.Trim().ToUpper() || strPrevlige.Trim().ToUpper() == clsPOSDBConstants.UserMaxReturnTransLimit.Trim().ToUpper() || strPrevlige.Trim().ToUpper() == clsPOSDBConstants.UserMaxDiscountLimit.Trim().ToUpper() || strPrevlige.Trim().ToUpper() == clsPOSDBConstants.UserMaxTenderedAmountLimit.Trim().ToUpper())
                {
                    if (POS_Core_UI.Resources.Message.Display("Amount entered exceeds user authorized limit." + Environment.NewLine + "Would you like to obtain permission to override this action?", strWindowTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.No)
                    {
                        m_IsCanceled = true;
                        POS_Core.Resources.Configuration.IsDiscOverridefromPOSTrans = false;
                        StopTimer(this.loginAttemptTimer);
					    return false;
                    }
                }
                else if (strPrevlige.Trim().ToUpper() == clsPOSDBConstants.AllowHouseChargePaytype.Trim().ToUpper() || strPrevlige.Trim().ToUpper() == clsPOSDBConstants.UserUsePayOutCatagory.Trim().ToUpper()
                    || strPrevlige.Trim().ToUpper() == UserPriviliges.Permissions.QuantityOverride.Name.Trim().ToUpper()
                    || strPrevlige.Trim().ToUpper() == UserPriviliges.Screens.DeleteCreditCardProfiles.Name.Trim().ToUpper()
                    || strPrevlige.Trim().ToUpper() == UserPriviliges.Permissions.MakeCouponPayment.Name.Trim().ToUpper()
                    || strPrevlige.Trim().ToUpper() == UserPriviliges.Permissions.AllowCashback.Name.Trim().ToUpper()
                    || strPrevlige.Trim().ToUpper() == UserPriviliges.Permissions.PriceOverridefromPOSTrans.Name.Trim().ToUpper()
                    || strPrevlige.Trim().ToUpper() == UserPriviliges.Permissions.PriceOverrideForRXItemsfromPOSTrans.Name.Trim().ToUpper()
                    || strPrevlige.Trim().ToUpper() == UserPriviliges.Permissions.StandAloneReturn.Name.Trim().ToUpper()
                    || strPrevlige.Trim().ToUpper() == UserPriviliges.Permissions.OnHoldTrans.Name.Trim().ToUpper() //PRIMEPOS-2639 22-Feb-2019 JY Added
                    )
                {
                    if (Resources.Message.Display("You do not have sufficient privileges to perform this action." + Environment.NewLine + "Would you like to obtain permission to override this action?", strWindowTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.No)
                    {
                        m_IsCanceled = true;
                        POS_Core.Resources.Configuration.IsDiscOverridefromPOSTrans = false;
                        StopTimer(this.loginAttemptTimer);
                        return false;
                    }
                }
				ofrmLogin.ShowDialog();
				if (!ofrmLogin.Canceled)
				{
                    sUserID=ofrmLogin.txtUserName.Text;
					m_IsCanceled = false;
					return true;
				}
				else
				{
					m_IsCanceled = true;
                    POS_Core.Resources.Configuration.IsDiscOverridefromPOSTrans = false;//Added By shitaljit on 2/11/2014 for JIRA PRIMEPOS-1810
                    StopTimer(this.loginAttemptTimer);
					return false;
				}
				
			}
			catch(POS_Core.ErrorLogging.POSExceptions exp)
			{
				ofrmLogin.txtUserName.Text = POS_Core.Resources.Configuration.UserName;
                POS_Core.ErrorLogging.Logs.Logger(clsPOSDBConstants.Log_Module_Login, " loginForPreviliges()", clsPOSDBConstants.Log_Exception_Occured + exp.StackTrace.ToString());
				throw(exp);
			}
			catch(Exception exp)
			{
                POS_Core.ErrorLogging.Logs.Logger(clsPOSDBConstants.Log_Module_Login, " loginForPreviliges()", clsPOSDBConstants.Log_Exception_Occured + exp.StackTrace.ToString());
				throw(exp);
			}
			finally
			{
				ofrmLogin.Dispose();
				ofrmLogin=null;
                POS_Core.ErrorLogging.Logs.Logger(clsPOSDBConstants.Log_Module_Login, " loginForPreviliges()", clsPOSDBConstants.Log_Exiting);
			}
		}
		public void login(LoginENUM loginType)
		 {
            POS_Core.ErrorLogging.Logs.Logger(clsPOSDBConstants.Log_Module_Login, " login()", clsPOSDBConstants.Log_Entering);
			ofrmLogin = frmLogin.GetInstance();
           
			try
			{
				ofrmLogin.oclsLogin = this;
				ofrmLogin.OpenType=loginType;
				ofrmLogin.SizeChanged += ofrmLogin_SizeChanged;
				if (loginType == LoginENUM.Lock)
				{
                    Logs.Logger(clsPOSDBConstants.Log_Module_Login, "Lock Station", "Station Locked By User" + POS_Core.Resources.Configuration.UserName);
                    frmCreateNewPurchaseOrder ofrmPOOrder = new frmCreateNewPurchaseOrder();
                    ofrmPOOrder = frmCreateNewPurchaseOrder.getInstance();
                    if (ofrmPOOrder != null)
                    {
                        ofrmPOOrder.TopMost = false;
                    }
					ofrmLogin.Text  ="Work Station Locked By User(" + POS_Core.Resources.Configuration.UserName+")";
					ofrmLogin.txtUserName.Text = POS_Core.Resources.Configuration.UserName;
                    ofrmLogin.TopMost = true;
                    //ofrmLogin.txtUserName.Enabled = false;
                    //ofrmLogin.btnCancel.Enabled = false;  //PRIMEPOS-2958 22-Apr-2021 JY Commented
                    //ofrmLogin.ControlBox = false;
                    ofrmLogin.txtPassward.Text = "";
				}
				if (loginType == LoginENUM.LogOff)
				{
					ofrmLogin.Text  ="Prime POS Login";
					ofrmLogin.txtUserName.Text = "";
					ofrmLogin.ControlBox = false;
					ofrmLogin.txtPassward.Text = "";
				}

				ofrmLogin.Visible=false;
                ofrmLogin.ShowDialog();

				ofrmLogin.SizeChanged -= ofrmLogin_SizeChanged;

				if (!ofrmLogin.Canceled)
				{
                    if (loginType == LoginENUM.Lock)
                    {
                        Logs.Logger(clsPOSDBConstants.Log_Module_Login, "Unlock Station", "By user" + POS_Core.Resources.Configuration.UserName );
                    }
					//UpdateIPAddress(Configuration.UserName);
					m_IsCanceled = false;
					buildDefaultForUser();
					//POS.UI.RemotingUtility.SetUserInfo(Configuration.UserName,Configuration.StationName);
				}
				else
				{
					m_IsCanceled = true;
                    StopTimer(this.loginAttemptTimer);
				}
			}
			catch(POS_Core.ErrorLogging.POSExceptions exp)
			{
				ofrmLogin.txtUserName.Text = POS_Core.Resources.Configuration.UserName;
                POS_Core.ErrorLogging.Logs.Logger(clsPOSDBConstants.Log_Module_Login, " login()", clsPOSDBConstants.Log_Exception_Occured + exp.StackTrace.ToString());
				throw(exp);
			}

			catch(Exception exp)
			{
                POS_Core.ErrorLogging.Logs.Logger(clsPOSDBConstants.Log_Module_Login, " login()", clsPOSDBConstants.Log_Exception_Occured + exp.StackTrace.ToString());
				throw(exp);
			}
			finally
			{
                POS_Core.ErrorLogging.Logs.Logger(clsPOSDBConstants.Log_Module_Login, " login()", clsPOSDBConstants.Log_Exiting);
				ofrmLogin.Dispose();
				ofrmLogin=null;
			}
		}

		public void ofrmLogin_SizeChanged(object sender, EventArgs e)
		{
            if (Configuration.UserName.Trim() != "")
            {
                if (ofrmLogin.WindowState == FormWindowState.Minimized)
                {
                    frmMain.getInstance().WindowState = FormWindowState.Minimized;
                }
                else if (ofrmLogin.WindowState == FormWindowState.Normal)
                {
                    frmMain.getInstance().WindowState = FormWindowState.Maximized;
                }
            }
			ofrmLogin.Show();
		}

		private void UpdateIPAddress(string sUserName)
		{
			string strSQL="Update Users set "+ clsPOSDBConstants.Users_Fld_LastLoginIP + "='" + clsUIHelper.GetLocalHostIP() + "' where " + clsPOSDBConstants.Users_Fld_UserID + "='" +  sUserName.Replace("'","''")+ "'";
			IDbCommand cmd = DataFactory.CreateCommand();
			IDbConnection conn = DataFactory.CreateConnection();
			conn.ConnectionString = POS_Core.Resources.Configuration.ConnectionString;
			conn.Open();
			cmd.Connection=conn;
			cmd.CommandText=strSQL;
			try
			{
				cmd.ExecuteNonQuery();
			}
			catch
			{

			}
			finally
			{
				if(conn.State!=ConnectionState.Closed)
				{
					conn.Close();
				}
				conn.Dispose();
			}
		}
        //PRIMEPOS-2664 ADDED BY ARVIND
        internal string GetCashierID(string pUserName)
        {
            POS_Core.ErrorLogging.Logs.Logger(clsPOSDBConstants.Log_Module_Login, " GETCASHIERID()", clsPOSDBConstants.Log_Entering);
            sUserName = pUserName;
            string sSQL = string.Empty;
            string sCashierID = string.Empty;
            DataSet oDS;
            POS_Core.BusinessRules.Search oSearch = new POS_Core.BusinessRules.Search();
            try
            {
                sSQL = String.Concat("SELECT "
                                                        , clsPOSDBConstants.Users_Fld_ID
                                                    , " FROM "
                                                        , clsPOSDBConstants.Users_tbl
                                                    , "  WHERE "
                                                        , clsPOSDBConstants.Users_Fld_UserID, " = '", pUserName.Replace("'", "''"), "'",
                                                        " AND IsActive = 1 AND  UserType is null OR UserType <> 'G' order by LastLoginAttempt desc");
                oDS = oSearch.SearchData(sSQL);
                if (oDS.Tables[0].Rows.Count > 0)
                {
                    sCashierID = oDS.Tables[0].Rows[0][0].ToString();
                }
                else
                {
                    ErrorHandler.throwCustomError(POSErrorENUM.User_InvalidUserName);
                }

            }
            catch (NullReferenceException)
            {
                //conn.Close();
                POS_Core.ErrorLogging.Logs.Logger(clsPOSDBConstants.Log_Module_Login, " login()", clsPOSDBConstants.Log_Exception_Occured);
                ErrorHandler.throwCustomError(POSErrorENUM.User_InvalidUserName);
            }
            catch (Exception exp)
            {
                //conn.Close();
                POS_Core.ErrorLogging.Logs.Logger(clsPOSDBConstants.Log_Module_Login, " login()", clsPOSDBConstants.Log_Exception_Occured + exp.StackTrace.ToString());
                throw (exp);
            }
            POS_Core.ErrorLogging.Logs.Logger(clsPOSDBConstants.Log_Module_Login, " GetCashierID()", clsPOSDBConstants.Log_Exiting);
            return sCashierID;
        }

        internal void ValidateUserNamePassward(string pUserName,string pPassward,out string sIPAddress)
		{
            POS_Core.ErrorLogging.Logs.Logger(clsPOSDBConstants.Log_Module_Login, " ValidateUserNamePassward()", clsPOSDBConstants.Log_Entering);
            int attempt = 0;
            sUserName = pUserName;
            sIPAddress = "";
            string sSQL = string.Empty;
            DataSet oDS;
            POS_Core.BusinessRules.Search oSearch = new POS_Core.BusinessRules.Search();
            try
            {
                if (iMaxAttempt >= 0 && iMaxAttempt <= 6)
                {
                    sSQL = String.Concat("SELECT "
                                                            , clsPOSDBConstants.Users_Fld_Password
                                                            , ",", clsPOSDBConstants.Users_Fld_LastLoginIP
                                                            , ",", clsPOSDBConstants.Users_Fld_IsLocked
                                                            , ",", clsPOSDBConstants.Users_Fld_UserID
                                                            , ",", clsPOSDBConstants.Users_Fld_ModifiedOn
                                                            , ",", clsPOSDBConstants.Users_Fld_NoOfAttempt
                                                            ,",",clsPOSDBConstants.Users_Fld_LastLoginAttempt
                                                        , " FROM "
                                                            , clsPOSDBConstants.Users_tbl
                                                        , "  WHERE "
                                                            , clsPOSDBConstants.Users_Fld_UserID, " = '", pUserName.Replace("'", "''"), "'",
                                                            " AND IsActive = 1 AND  UserType is null OR UserType <> 'G' order by LastLoginAttempt desc");

                    //cmd.CommandType = CommandType.Text;
                    //cmd.CommandText = sSQL;
                    //cmd.Connection = conn;
                    
                    oDS = oSearch.SearchData(sSQL);
                    if (oDS.Tables[0].Rows.Count > 0)
                    {
                        //string sPassward = oDS.Tables[0].Rows[0][0].ToString();

                        string sPassward = EncryptString.Decrypt(oDS.Tables[0].Rows[0][0].ToString());
                        bool bIsLocked = POS_Core.Resources.Configuration.convertNullToBoolean(oDS.Tables[0].Rows[0][clsPOSDBConstants.Users_Fld_IsLocked].ToString());

                        if (bIsLocked)
                        {
                            if (oDS.Tables[0].Rows[0][clsPOSDBConstants.Users_Fld_LastLoginAttempt] != null)
                            {
                                DateTime lastLogin = Convert.ToDateTime(oDS.Tables[0].Rows[0][clsPOSDBConstants.Users_Fld_LastLoginAttempt].ToString());
                                TimeSpan timeElapsed = DateTime.Now.Subtract(lastLogin);
                                if (timeElapsed.Days > 0 || timeElapsed.Hours > 0 || timeElapsed.Minutes > 30)
                                    bIsLocked = false;
                                
                            }
                            else
                                bIsLocked=false;
                        }
                        attempt = POS_Core.Resources.Configuration.convertNullToInt(oDS.Tables[0].Rows[0][clsPOSDBConstants.Users_Fld_NoOfAttempt].ToString());
                        
                        if (bIsLocked || attempt == 6)
                        {
                            iMaxAttempt = attempt;
                            //Start : Added By Amit Date 18 May 2011
                            LoginFailedUsers(pUserName, 30, true);
                            ErrorHandler.UpdateUserLoginAttempt(pUserName, iMaxAttempt, true);
                            //End
                            ErrorHandler.throwCustomError(POSErrorENUM.User_Locked);
                            //POS_Core_UI.Resources.Message.Display("Account is Locked. Please contact Administrator\n Or Wait for 30 mins for unlocking ", "User Authentication", MessageBoxButtons.OK);
                            
                        }
                        else if (pPassward != sPassward)
                        {
                            if (iMaxAttempt == 6)
                            {
                                //POS_Core_UI.Resources.Message.Display("Account is Locked. Please contact Administrator\n Or Wait for 30 mins for unlocking ", "User Authentication", MessageBoxButtons.OK);
                                //Added By Amit Date 19 May 2011
                                LoginFailedUsers(pUserName,30,true);
                               
                                ErrorHandler.UpdateUserLoginAttempt(pUserName, iMaxAttempt, true);
                                ErrorHandler.throwCustomError(POSErrorENUM.User_Locked);
                            }
                            else
                            {
                                ErrorHandler.UpdateUserLoginAttempt(pUserName, iMaxAttempt, false);
                                iMaxAttempt += 1;
                                ErrorHandler.throwCustomError(POSErrorENUM.User_InValidPassward);
                            }   
                            sIPAddress = oDS.Tables[0].Rows[0][1].ToString();
                            iMaxAttempt = 1;
                        }
                    }
                    else
                    {
                        ErrorHandler.throwCustomError(POSErrorENUM.User_InvalidUserName);
                    }
                    //conn.Close();
                }
                else
                {
                    if (!(dictLoginFail.ContainsKey(pUserName)))
                    {
                        dictLoginFail.Add(pUserName, DateTime.Now.AddMinutes(30));
                    }
                    //POS_Core_UI.Resources.Message.Display("Account is Locked. Please contact Administrator\n Or Wait for 30 mins for unlocking ", "User Authentication", MessageBoxButtons.OK);
                    ErrorHandler.throwCustomError(POSErrorENUM.User_Locked);
                }

            }
            catch (NullReferenceException)
            {
                //conn.Close();
                POS_Core.ErrorLogging.Logs.Logger(clsPOSDBConstants.Log_Module_Login, " login()", clsPOSDBConstants.Log_Exception_Occured);
                ErrorHandler.throwCustomError(POSErrorENUM.User_InvalidUserName);
            }
            catch (Exception exp)
            {
                //conn.Close();
                POS_Core.ErrorLogging.Logs.Logger(clsPOSDBConstants.Log_Module_Login, " login()", clsPOSDBConstants.Log_Exception_Occured + exp.StackTrace.ToString());
                throw (exp);
            }
            POS_Core.ErrorLogging.Logs.Logger(clsPOSDBConstants.Log_Module_Login, " ValidateUserNamePassward()", clsPOSDBConstants.Log_Exiting);
			
		}
        /// <summary>
        /// This function is called when user login with barcode
        /// added new parameter sPassword by shitaljit to resolve missing password error while login with Barcode.
        /// </summary>
        /// <param name="sID"></param>
        /// <param name="strUserID"></param>
        /// <param name="sIPAddress"></param>
        /// <param name="sPassword"></param>
        internal void ValidateUserByID(string sID,out string strUserID, out string sIPAddress,out string sPassword)
        {
           
            POS_Core.ErrorLogging.Logs.Logger(clsPOSDBConstants.Log_Module_Login, " ValidateUserByID() ", clsPOSDBConstants.Log_Entering);
            /*IDbCommand cmd = DataFactory.CreateCommand();
            string sSQL = "";
			
            string sPassward = "";
            IDbConnection conn = DataFactory.CreateConnection();

            conn.ConnectionString = Configuration.ConnectionString;
			
            conn.Open();*/
            sIPAddress = "";
            strUserID = "";
            sPassword = "";
            try
            {

                string sSQL = String.Concat("SELECT "
                                            , clsPOSDBConstants.Users_Fld_Password
                                            , ",", clsPOSDBConstants.Users_Fld_UserID
                                            , ",", clsPOSDBConstants.Users_Fld_LastLoginIP
                                        , " FROM "
                                            , clsPOSDBConstants.Users_tbl
                                        , "  WHERE "
                                            , clsPOSDBConstants.Users_Fld_ID, " = ", sID.Replace("'", "''"), "", " AND IsActive = 1 AND UserType is null OR UserType <> 'G' ");

                //cmd.CommandType = CommandType.Text;
                //cmd.CommandText = sSQL;
                //cmd.Connection = conn;

                POS_Core.BusinessRules.Search oSearch = new POS_Core.BusinessRules.Search();
                DataSet oDS = oSearch.SearchData(sSQL);

                if (oDS.Tables[0].Rows.Count > 0)
                {
                    sPassword = oDS.Tables[0].Rows[0][0].ToString();
                    //Configuration.oCryption.Decrypt(
                    sIPAddress = oDS.Tables[0].Rows[0][clsPOSDBConstants.Users_Fld_LastLoginIP].ToString();
                    strUserID = oDS.Tables[0].Rows[0][clsPOSDBConstants.Users_Fld_UserID].ToString();
                }
                else
                {
                    ErrorHandler.throwCustomError(POSErrorENUM.User_InvalidUserName);
                }
                //conn.Close();

            }
            catch (NullReferenceException)
            {
                //conn.Close();
                ErrorHandler.throwCustomError(POSErrorENUM.User_InvalidUserName);
            }
            catch (Exception exp)
            {
                POS_Core.ErrorLogging.Logs.Logger(clsPOSDBConstants.Log_Module_Login, " ValidateUserByID() ", clsPOSDBConstants.Log_Exception_Occured + exp.StackTrace.ToString());
                //conn.Close();
                throw (exp);
            }
            POS_Core.ErrorLogging.Logs.Logger(clsPOSDBConstants.Log_Module_Login, " ValidateUserByID() ", clsPOSDBConstants.Log_Exiting);
        }

		public void GetUsersRole(string pUserName)
		{
            POS_Core.ErrorLogging.Logs.Logger(clsPOSDBConstants.Log_Module_Login, " GetUsersRole()", clsPOSDBConstants.Log_Entering);
			IDbCommand cmd = DataFactory.CreateCommand();
			string sSQL = "";
	
			IDataReader dr;

			IDbConnection conn = DataFactory.CreateConnection();

			conn.ConnectionString = this.m_ConnString;
			
			conn.Open();

			try 
			{

				sSQL = String.Concat( "SELECT " 
					, clsPOSDBConstants.Users_Fld_Role
					, ", " , clsPOSDBConstants.Users_Fld_DrawNo
                    , ", ", clsPOSDBConstants.Users_Fld_MaxDiscountLimit
                      , ", ", clsPOSDBConstants.Users_Fld_MaxTransactionLimit
                      , ", ", clsPOSDBConstants.Users_Fld_MaxReturnTransLimit   //Sprint-23 - PRIMEPOS-2303 24-May-2016 JY Added 
                      , ", ", clsPOSDBConstants.Users_Fld_MaxTenderedAmountLimit    //Sprint-25 - PRIMEPOS-2411 21-Apr-2017 JY Added 
                      , ", ", clsPOSDBConstants.Users_Fld_MaxCashbackLimit  //PRIMEPOS-2741 26-Sep-2019 JY Added
                    , " FROM " 
					, clsPOSDBConstants.Users_tbl
					, "  WHERE " 
					,clsPOSDBConstants.Users_Fld_UserID , " = '" ,  pUserName.Replace("'","''"), "'");

				cmd.CommandType = CommandType.Text;
				cmd.CommandText = sSQL;
				cmd.Connection = conn;

				dr = cmd.ExecuteReader();
				dr.Read();

                POS_Core.Resources.Configuration.DrawNo=dr.GetInt32(dr.GetOrdinal(clsPOSDBConstants.Users_Fld_DrawNo));
                if (!dr.IsDBNull(dr.GetOrdinal(clsPOSDBConstants.Users_Fld_MaxDiscountLimit)))
                {
                    POS_Core.Resources.Configuration.UserMaxDiscountLimit = dr.GetDecimal(dr.GetOrdinal(clsPOSDBConstants.Users_Fld_MaxDiscountLimit));//added by shitaljit to define % disc user can give
                }
                else
                {
                    POS_Core.Resources.Configuration.UserMaxDiscountLimit = 0;
                }
                if (!dr.IsDBNull(dr.GetOrdinal(clsPOSDBConstants.Users_Fld_MaxTransactionLimit)))//Added by Ravindra(QuicSolv) For Maximum Transaction limit
                {
                    POS_Core.Resources.Configuration.UserMaxTransactionLimit = dr.GetDecimal(dr.GetOrdinal(clsPOSDBConstants.Users_Fld_MaxTransactionLimit));//Added by Ravindra to define Maximum Transaction Limit user can give
                }
                else
                {
                    POS_Core.Resources.Configuration.UserMaxTransactionLimit = 0;
                }
                //Sprint-23 - PRIMEPOS-2303 24-May-2016 JY Added 
                if (!dr.IsDBNull(dr.GetOrdinal(clsPOSDBConstants.Users_Fld_MaxReturnTransLimit)))
                {
                    POS_Core.Resources.Configuration.UserMaxReturnTransLimit = dr.GetDecimal(dr.GetOrdinal(clsPOSDBConstants.Users_Fld_MaxReturnTransLimit));
                }
                else
                {
                    POS_Core.Resources.Configuration.UserMaxReturnTransLimit= 0;
                }

                //Sprint-25 - PRIMEPOS-2411 21-Apr-2017 JY Added 
                if (!dr.IsDBNull(dr.GetOrdinal(clsPOSDBConstants.Users_Fld_MaxTenderedAmountLimit)))
                {
                    POS_Core.Resources.Configuration.UserMaxTenderedAmountLimit = dr.GetDecimal(dr.GetOrdinal(clsPOSDBConstants.Users_Fld_MaxTenderedAmountLimit));
                }
                else
                {
                    POS_Core.Resources.Configuration.UserMaxTenderedAmountLimit = 0;
                }

                //PRIMEPOS-2741 26-Sep-2019 JY Added
                if (!dr.IsDBNull(dr.GetOrdinal(clsPOSDBConstants.Users_Fld_MaxCashbackLimit)))
                {
                    Configuration.UserMaxCashbackLimit = dr.GetDecimal(dr.GetOrdinal(clsPOSDBConstants.Users_Fld_MaxCashbackLimit));
                }
                else
                {
                    Configuration.UserMaxCashbackLimit = 0;
                }

                //			if (CheckSecurityLevel)
                conn.Close();
			}

			catch(Exception exp) 
			{
				conn.Close();
                POS_Core.ErrorLogging.Logs.Logger(clsPOSDBConstants.Log_Module_Login, " GetUsersRole() ", clsPOSDBConstants.Log_Exception_Occured + exp.StackTrace.ToString());
				throw(exp);
			}
            POS_Core.ErrorLogging.Logs.Logger(clsPOSDBConstants.Log_Module_Login, " GetUsersRole()", clsPOSDBConstants.Log_Exiting);
		}

		public void ValidateSecuriyLevel(string pUserName)
		{
            POS_Core.ErrorLogging.Logs.Logger(clsPOSDBConstants.Log_Module_Login, " ValidateSecuriyLevel()", clsPOSDBConstants.Log_Entering);
			IDbCommand cmd = DataFactory.CreateCommand();
			string sSQL = "";
			int drawNo,SecurityLevel;
			IDataReader dr;

			IDbConnection conn = DataFactory.CreateConnection();

			conn.ConnectionString = this.m_ConnString;
			
			conn.Open();

			try 
			{

				sSQL = String.Concat( "SELECT " 
					, clsPOSDBConstants.Users_Fld_DrawNo 
					, " , " , clsPOSDBConstants.Users_Fld_SecurityLevel
					, " , " , clsPOSDBConstants.Users_Fld_Role
                    , " , ", clsPOSDBConstants.Users_Fld_MaxDiscountLimit
                    , " , ", clsPOSDBConstants.Users_Fld_MaxTransactionLimit
                    , " , ", clsPOSDBConstants.Users_Fld_MaxReturnTransLimit    //Sprint-23 - PRIMEPOS-2303 11-Aug-2016 JY Added 
                    , " , ", clsPOSDBConstants.Users_Fld_MaxTenderedAmountLimit //Sprint-25 - PRIMEPOS-2411 21-Apr-2017 JY Added 
                    , " FROM " 
					, clsPOSDBConstants.Users_tbl
					, "  WHERE " 
					,clsPOSDBConstants.Users_Fld_UserID , " = '" ,  pUserName.Replace("'","''"), "'");

				cmd.CommandType = CommandType.Text;
				cmd.CommandText = sSQL;
				cmd.Connection = conn;

				dr = cmd.ExecuteReader();
				//
				//Configuration.oCryption.Decrypt(
				dr.Read();

				drawNo = dr.GetInt32(dr.GetOrdinal(clsPOSDBConstants.Users_Fld_DrawNo));
				SecurityLevel = dr.GetInt32(dr.GetOrdinal(clsPOSDBConstants.Users_Fld_SecurityLevel));
                if (!dr.IsDBNull(dr.GetOrdinal(clsPOSDBConstants.Users_Fld_MaxDiscountLimit)))
                {
                    POS_Core.Resources.Configuration.UserMaxDiscountLimit = dr.GetDecimal(dr.GetOrdinal(clsPOSDBConstants.Users_Fld_MaxDiscountLimit));//added by shitaljit to define % disc user can give
                }
                else
                {
                    POS_Core.Resources.Configuration.UserMaxDiscountLimit = 0;
                }
                if (!dr.IsDBNull(dr.GetOrdinal(clsPOSDBConstants.Users_Fld_MaxTransactionLimit)))//Added by Ravindra
                {
                    POS_Core.Resources.Configuration.UserMaxTransactionLimit = dr.GetDecimal(dr.GetOrdinal(clsPOSDBConstants.Users_Fld_MaxTransactionLimit));//added by Ravindra to MaxTransaction Limit user can give
                }
                else
                {
                    POS_Core.Resources.Configuration.UserMaxTransactionLimit = 0;
                }
                //Sprint-23 - PRIMEPOS-2303 24-May-2016 JY Added 
                if (!dr.IsDBNull(dr.GetOrdinal(clsPOSDBConstants.Users_Fld_MaxReturnTransLimit)))
                {
                    POS_Core.Resources.Configuration.UserMaxReturnTransLimit= dr.GetDecimal(dr.GetOrdinal(clsPOSDBConstants.Users_Fld_MaxReturnTransLimit));
                }
                else
                {
                    POS_Core.Resources.Configuration.UserMaxReturnTransLimit= 0;
                }

                //Sprint-25 - PRIMEPOS-2411 21-Apr-2017 JY Added 
                if (!dr.IsDBNull(dr.GetOrdinal(clsPOSDBConstants.Users_Fld_MaxTenderedAmountLimit)))
                {
                    POS_Core.Resources.Configuration.UserMaxTenderedAmountLimit = dr.GetDecimal(dr.GetOrdinal(clsPOSDBConstants.Users_Fld_MaxTenderedAmountLimit));
                }
                else
                {
                    POS_Core.Resources.Configuration.UserMaxTenderedAmountLimit = 0;
                }


                //				if (CheckSecurityLevel)
                //				if (Configuration.UserRole==UserRoleENUM.Cashier)
                //				{
                //					if (m_UserRole!=UserRoleENUM.HeadCashier && m_UserRole!=UserRoleENUM.Manager && m_UserRole!=UserRoleENUM.Supervisor)
                //						ErrorHandler.throwCustomError(POSErrorENUM.User_InvalidSecurityLevel);
                //				}
                //				else if (Configuration.UserRole==UserRoleENUM.HeadCashier)
                //				{
                //					if (m_UserRole!=UserRoleENUM.Manager && m_UserRole!=UserRoleENUM.Supervisor)
                //						ErrorHandler.throwCustomError(POSErrorENUM.User_InvalidSecurityLevel);
                //				}
                //				else if (Configuration.UserRole==UserRoleENUM.Manager)
                //				{
                //					if (m_UserRole!=UserRoleENUM.Supervisor)
                //						ErrorHandler.throwCustomError(POSErrorENUM.User_InvalidSecurityLevel);
                //				}
                //				else if (Configuration.UserRole==UserRoleENUM.Supervisor)
                //				{
                //					if (m_UserRole!=UserRoleENUM.Supervisor)
                //						ErrorHandler.throwCustomError(POSErrorENUM.User_InvalidSecurityLevel);
                //				}
                //if (Configuration.SecurityLevel < SecurityLevel)
                //else
                //ErrorHandler.throwCustomError(POSErrorENUM.User_InvalidSecurityLevel);
                POS_Core.Resources.Configuration.SecurityLevel = SecurityLevel;
                POS_Core.Resources.Configuration.DrawNo=drawNo;
                POS_Core.Resources.Configuration.UserName= pUserName;

				frmMain.getInstance().ultraStatusBar.Panels["User"].Text="User Name: " + POS_Core.Resources.Configuration.UserName;

				conn.Close();
			}

			catch(Exception exp) 
			{
                POS_Core.ErrorLogging.Logs.Logger(clsPOSDBConstants.Log_Module_Login, " ValidateSecuriyLevel()", clsPOSDBConstants.Log_Exception_Occured+exp.StackTrace.ToString());
				conn.Close();
				throw(exp);
			}
            POS_Core.ErrorLogging.Logs.Logger(clsPOSDBConstants.Log_Module_Login, " ValidateSecuriyLevel()", clsPOSDBConstants.Log_Exiting);
		}

		public void ValidateSingleUserLogin(string pUserName)
		{
            POS_Core.ErrorLogging.Logs.Logger(clsPOSDBConstants.Log_Module_Login, " ValidateSingleUserLogin()", clsPOSDBConstants.Log_Entering);
			IDbCommand cmd = DataFactory.CreateCommand();
			string sSQL = "";
			IDataReader dr;

			IDbConnection conn = DataFactory.CreateConnection();
			conn.ConnectionString = this.m_ConnString;
			conn.Open();

            try
            {

                sSQL = String.Concat("SELECT  ", clsPOSDBConstants.Users_Fld_UserID
                    , " FROM "
                    , clsPOSDBConstants.TransHeader_tbl
                    , "  WHERE "
                    , clsPOSDBConstants.TransHeader_Fld_StationID, "='", POS_Core.Resources.Configuration.StationID, "' and ",
                    clsPOSDBConstants.TransHeader_Fld_isStationClosed, "=0");

                cmd.CommandType = CommandType.Text;
                cmd.CommandText = sSQL;
                cmd.Connection = conn;

                dr = cmd.ExecuteReader();
                if (dr.Read() == false)
                {
                    POS_Core.ErrorLogging.Logs.Logger(clsPOSDBConstants.Log_Module_Login, " ValidateSingleUserLogin()", clsPOSDBConstants.Log_Exiting);
                    return;
                }
                string User = dr.GetString(0);
                if (pUserName == User)
                {
                    POS_Core.ErrorLogging.Logs.Logger(clsPOSDBConstants.Log_Module_Login, " ValidateSingleUserLogin()", clsPOSDBConstants.Log_Exiting);
                    return;
                }
                throw (new Exception("You cannot login. User '" + User + "' already has unClosed transctions on this station."));
            }

            catch (Exception exp)
            {
                POS_Core.ErrorLogging.Logs.Logger(clsPOSDBConstants.Log_Module_Login, " ValidateSingleUserLogin()", clsPOSDBConstants.Log_Exception_Occured + exp.StackTrace.ToString());
                conn.Close();
                throw (exp);
            }
            finally
            {
                POS_Core.ErrorLogging.Logs.Logger(clsPOSDBConstants.Log_Module_Login, " ValidateSingleUserLogin()", clsPOSDBConstants.Log_Exiting);
            }
		}

		public bool IsCanceled
		{
			get 
			{ 
				return this.m_IsCanceled;
			} 
		}

		public void buildDefaultForUser()
		{
			frmUserInformation ofrm=new	frmUserInformation();
            ofrm.SetAllPermissionsForLoggedInUser();
			ofrm.Dispose();
		}

        public string GetUsername(string strUserID)
        {
            string strUsername = "";
            try
            {

                string sSQL = String.Concat("SELECT "
                                            , clsPOSDBConstants.Users_Fld_lName + " +', ' + " + clsPOSDBConstants.Users_Fld_fName + " as UserName"
                                        , " FROM "
                                            , clsPOSDBConstants.Users_tbl
                                        , "  WHERE "
                                            , clsPOSDBConstants.Users_Fld_UserID, " = '", strUserID.Replace("'", "''"), "'");

                POS_Core.BusinessRules.Search oSearch = new POS_Core.BusinessRules.Search();
                DataSet oDS = oSearch.SearchData(sSQL);

                if (oDS.Tables[0].Rows.Count > 0)
                {
                    strUsername = oDS.Tables[0].Rows[0][0].ToString();
                }
                else
                {
                    ErrorHandler.throwCustomError(POSErrorENUM.User_InvalidUserName);
                }
                //conn.Close();
            }
            catch (NullReferenceException)
            {
                //conn.Close();
                ErrorHandler.throwCustomError(POSErrorENUM.User_InvalidUserName);
            }
            catch (Exception exp)
            {
                //conn.Close();
                throw (exp);
            }
            return strUsername;
        }

        public int MaxAttempt
        {
            get
            {
                return iMaxAttempt;
            }
            set
            {
                iMaxAttempt = value;
            }
        }

        private void InitializeLoginAttemptTimer()
        {
            int loginAttemptMSec = 60000;
            
            if (loginAttemptTimer == null)
            {
                loginAttemptTimer = new System.Timers.Timer();
                loginAttemptTimer.Interval = loginAttemptMSec;
                loginAttemptTimer.Enabled = false;
                loginAttemptTimer.Elapsed -= new ElapsedEventHandler(OnLoginAttemptEvent);
                loginAttemptTimer.Elapsed += new ElapsedEventHandler(OnLoginAttemptEvent);
            }
        }

        private void OnLoginAttemptEvent(object sender, ElapsedEventArgs e)
        {
            try
            {
                if (dictLoginFail.Count > 0)
                {
                    foreach (KeyValuePair<string, DateTime> users in dictLoginFail)
                    {
                        TimeSpan ts = users.Value.Subtract(DateTime.Now);
                        if (ts.Minutes <= 0)
                        {
                            ErrorHandler.UpdateUserLoginAttempt(users.Key, 0, false);
                            dictLoginFail.Remove(users.Key);
                            iMaxAttempt = 1;
                            return;
                        }
                    }
                }
            }
            catch (TimeoutException ex)
            {
                //Monitor.Exit(lockObj);
            }
            finally
            {
                //if (fromLock)
                //    Monitor.Exit(lockObj);
            }
        }

        private void StartTimer(System.Timers.Timer timer)
        {
            if (timer != null)
            {
                timer.AutoReset = true;
                timer.Enabled = true;
            }
        }

        private void StopTimer(System.Timers.Timer timer)
        {
            if (timer != null)
            {
                timer.AutoReset = false;
                timer.Enabled = false;
                timer.Stop();
            }
        }

        public void CheckResetPassword(string pUserName)
        {
            POS_Core.ErrorLogging.Logs.Logger(clsPOSDBConstants.Log_Module_Login, " CheckResetPassword()", clsPOSDBConstants.Log_Entering);
            string sSQL = string.Empty;
            DataSet oDS;
            POS_Core.BusinessRules.Search oSearch = new POS_Core.BusinessRules.Search();
            try
            {
                #region PRIMEPOS-2562 03-Aug-2018 JY Commented
                //sSQL = String.Concat("SELECT "
                //                                            , clsPOSDBConstants.Users_Fld_Password
                //                                            , ",", clsPOSDBConstants.Users_Fld_LastLoginIP
                //                                            , ",", clsPOSDBConstants.Users_Fld_IsLocked
                //                                            , ",", clsPOSDBConstants.Users_Fld_UserID
                //                                            , ",", clsPOSDBConstants.Users_Fld_ModifiedOn
                //                                            , ",", clsPOSDBConstants.Users_Fld_PasswordChangedOn
                //                                        , " FROM "
                //                                            , clsPOSDBConstants.Users_tbl
                //                                        , "  WHERE "
                //                                            , clsPOSDBConstants.Users_Fld_UserID, " = '", pUserName.Replace("'", "''"), "'", " AND IsActive = 1");
                //oDS = oSearch.SearchData(sSQL);
                //if (oDS.Tables[0].Rows.Count > 0)
                //{
                //    if (oDS.Tables[0].Rows[0][clsPOSDBConstants.Users_Fld_PasswordChangedOn].ToString() != "")
                //    {
                //        DateTime dtLoginCreated = Convert.ToDateTime(oDS.Tables[0].Rows[0][clsPOSDBConstants.Users_Fld_PasswordChangedOn].ToString());
                //        TimeSpan daysDiff = DateTime.Now.Subtract(dtLoginCreated);
                //        if (daysDiff.Days >= Configuration.CPOSSet.DaysToResetPwd)
                //            ErrorHandler.throwCustomError(POSErrorENUM.User_ResetPassword);
                //    }
                //}
                #endregion

                #region PRIMEPOS-2562 03-Aug-2018 JY Added
                sSQL = "SELECT " + clsPOSDBConstants.Users_Fld_PasswordChangedOn + " FROM Users WHERE " + clsPOSDBConstants.Users_Fld_UserID + " = '" + pUserName.Replace("'", "''") + "'";
                oDS = oSearch.SearchData(sSQL);
                if (oDS.Tables[0].Rows.Count > 0)
                {
                    DateTime dtLoginCreated = Convert.ToDateTime(oDS.Tables[0].Rows[0][clsPOSDBConstants.Users_Fld_PasswordChangedOn].ToString());
                    TimeSpan daysDiff = DateTime.Now.Subtract(dtLoginCreated);
                    if (daysDiff.Days >= POS_Core.Resources.Configuration.CInfo.PasswordExpirationDays)
                        ErrorHandler.throwCustomError(POSErrorENUM.User_ResetPassword);
                }
                #endregion
            }
            catch (NullReferenceException)
            {
                ErrorHandler.throwCustomError(POSErrorENUM.User_InvalidUserName);
            }
            catch (Exception exp)
            {
                throw (exp);
            }
            POS_Core.ErrorLogging.Logs.Logger(clsPOSDBConstants.Log_Module_Login, " CheckResetPassword()", clsPOSDBConstants.Log_Exiting);
        }

        public DataSet GetUserPasswords(string sUserID)
        {
            POS_Core.ErrorLogging.Logs.Logger(clsPOSDBConstants.Log_Module_Login, " GetUserPasswords()", clsPOSDBConstants.Log_Entering);
            string sSQL = string.Empty;
            DataSet oDS;
            POS_Core.BusinessRules.Search oSearch = new POS_Core.BusinessRules.Search();

            sSQL = String.Concat("SELECT "
                                                            , clsPOSDBConstants.Users_Fld_Password
                                                            , ",", clsPOSDBConstants.Users_Fld_LastLoginIP
                                                            , ",", clsPOSDBConstants.Users_Fld_IsLocked
                                                            , ",", clsPOSDBConstants.Users_Fld_UserID
                                                            , ",", clsPOSDBConstants.Users_Fld_NoOfAttempt
                                                            , ",", clsPOSDBConstants.Users_Fld_Password1
                                                            , ",", clsPOSDBConstants.Users_Fld_Password2
                                                            , ",", clsPOSDBConstants.Users_Fld_Password3
                                                        , " FROM "
                                                            , clsPOSDBConstants.Users_tbl
                                                            , "  WHERE "
                                                            , clsPOSDBConstants.Users_Fld_UserID, " = '", sUserID.Replace("'", "''"), "'", " AND IsActive = 1");

            oDS = oSearch.SearchData(sSQL);
            POS_Core.ErrorLogging.Logs.Logger(clsPOSDBConstants.Log_Module_Login, " GetUserPasswords()", clsPOSDBConstants.Log_Exiting);
            return oDS;
        }

        internal void ValidateUserName(string sID)
        {
            POS_Core.ErrorLogging.Logs.Logger(clsPOSDBConstants.Log_Module_Login, " ValidateUserName()", clsPOSDBConstants.Log_Entering);
            try
            {
                string sSQL = String.Concat("SELECT "
                                            , clsPOSDBConstants.Users_Fld_Password
                                            , ",", clsPOSDBConstants.Users_Fld_UserID
                                            , ",", clsPOSDBConstants.Users_Fld_LastLoginIP
                                            , ",", clsPOSDBConstants.Users_Fld_NoOfAttempt
                                            ,",",clsPOSDBConstants.Users_Fld_LastLoginAttempt   //Added By Amit
                                        , " FROM "
                                            , clsPOSDBConstants.Users_tbl
                                        , "  WHERE "
                                            , clsPOSDBConstants.Users_Fld_UserID, " = '", sID.Replace("'", "''"), "'", " AND IsActive = 1 AND UserType is null OR UserType <> 'G'");

                POS_Core.BusinessRules.Search oSearch = new POS_Core.BusinessRules.Search();
                DataSet oDS = oSearch.SearchData(sSQL);

                if (oDS.Tables[0].Rows.Count > 0)
                {
                    string sUserName = oDS.Tables[0].Rows[0][clsPOSDBConstants.Users_Fld_UserID].ToString();
                    string sPassward = oDS.Tables[0].Rows[0][clsPOSDBConstants.Users_Fld_Password].ToString();
                    DateTime dtLastLoginAttempt;
                    //Following Code added by Krishna on 3 August 2011
                    if (oDS.Tables[0].Rows[0][clsPOSDBConstants.Users_Fld_LastLoginAttempt].ToString() == "")
                        dtLastLoginAttempt = DateTime.Now;
                    else
                    //Till here added by krishna
                    dtLastLoginAttempt=Convert.ToDateTime(oDS.Tables[0].Rows [0][clsPOSDBConstants.Users_Fld_LastLoginAttempt]);

                    if (sPassward == "yAAf2D1RJJM=")
                    {
                        POS_Core_UI.Resources.Message.Display("Please reset default password", "User Authentication", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        frmChangePassword objChangPwd = new frmChangePassword(sID, sPassward);
                        objChangPwd.ShowDialog();
                    }
                    else
                    {
                        iMaxAttempt = POS_Core.Resources.Configuration.convertNullToInt(oDS.Tables[0].Rows[0][clsPOSDBConstants.Users_Fld_NoOfAttempt].ToString());
                        if (iMaxAttempt == 6)
                        {
                            
                            //Added By Amit Date 18 May 2011    
                            TimeSpan ts = DateTime.Now.Subtract(dtLastLoginAttempt);
                            if (ts.Minutes>=30)
                            {
                                ErrorHandler.UpdateUserLoginAttempt(sUserName, 0, false);                               
                                iMaxAttempt = 1;
                            }
                            else
                            {
                                LoginFailedUsers(sUserName,ts.Minutes,false);
                                ErrorHandler.throwCustomError(POSErrorENUM.User_Locked,(30-ts.Minutes));
                            }
                            //End
                            //POS_Core_UI.Resources.Message.Display("Account is Locked. Please contact Administrator\n Or Wait for 30 mins for unlocking ", "User Authentication", MessageBoxButtons.OK);
                            //ErrorHandler.throwCustomError(POSErrorENUM.User_Locked);  Commented By Amit Date 
                        }
                        else
                        {
                            iMaxAttempt += 1;
                        }
                    }
                }
                else
                {
                    POS_Core.ErrorLogging.Logs.Logger(clsPOSDBConstants.Log_Module_Login, " ValidateUserName()", clsPOSDBConstants.Log_Exception_Occured);
                    ErrorHandler.throwCustomError(POSErrorENUM.User_InvalidUserName);
                }
            }
            catch (NullReferenceException)
            {
                POS_Core.ErrorLogging.Logs.Logger(clsPOSDBConstants.Log_Module_Login, " ValidateUserName()", clsPOSDBConstants.Log_Exception_Occured );
                ErrorHandler.throwCustomError(POSErrorENUM.User_InvalidUserName);
            }
            catch (Exception exp)
            {
                POS_Core.ErrorLogging.Logs.Logger(clsPOSDBConstants.Log_Module_Login, " ValidateUserName()", clsPOSDBConstants.Log_Exception_Occured+exp.StackTrace.ToString());
                throw (exp);
            }
            POS_Core.ErrorLogging.Logs.Logger(clsPOSDBConstants.Log_Module_Login, " ValidateUserName()", clsPOSDBConstants.Log_Exiting);
        }
        /// <summary>
        /// Added By Amit Date 18 May 2011
        /// Adds login failed user name in dictionary
        /// </summary>
        /// <param name="pUserName"></param>
        private void LoginFailedUsers(string pUserName,int pMinutes,bool pRmove)
        {
            //Remove Users
            if (pRmove && dictLoginFail.ContainsKey(pUserName))
            { 
                dictLoginFail.Remove(pUserName);
            }
            //Add Users
            if (!(dictLoginFail.ContainsKey(pUserName)))
            {
                dictLoginFail.Add(pUserName, DateTime.Now.AddMinutes(pMinutes));
            }
        }

        public IDataReader GetUserByID(string pUserId)
        {
            using (IDbConnection connection = DataFactory.CreateConnection(POS_Core.Resources.Configuration.ConnectionString))
            {
                return GetUserByID(pUserId, connection);
            }
        }

        public IDataReader GetUserByID(string pUserId,IDbConnection oConn)
        {
            string sSQL = String.Concat("SELECT * FROM "
                    , clsPOSDBConstants.Users_tbl
                    , " WHERE "
                    , clsPOSDBConstants.Users_Fld_UserID, " = '", pUserId, "'");

            return DataHelper.ExecuteReader(oConn, CommandType.Text, sSQL);
        }

        public IDataReader GetUserByID(string pUserId, IDbTransaction oTrans)
        {
            string sSQL = String.Concat("SELECT * FROM "
                    , clsPOSDBConstants.Users_tbl
                    , " WHERE "
                    , clsPOSDBConstants.Users_Fld_UserID, " = '", pUserId, "'");

            return DataHelper.ExecuteReader(oTrans, CommandType.Text, sSQL);
        }

        //Sprint-24 - PRIMEPOS-2290 19-Jan-2017 JY Added
        public void AddAllowHouseChargePaytypeAccessRight(string strUserName)
        {
            try
            {
                string sSQL = "SELECT UserID FROM Util_UserRights WHERE UserID = '" + strUserName.Replace("'", "''") + "' AND ModuleID = 1 AND ScreenID = 1 AND PermissionID = 73";
                object objValue = DataHelper.ExecuteScalar(sSQL);
                if (objValue == null)
                {
                    sSQL = "INSERT INTO Util_UserRights(UserID,ModuleID,ScreenID,PermissionID,isAllowed) VALUES('" + strUserName.Replace("'", "''") + "',1,1,73,1)";
                    DataHelper.ExecuteNonQuery(POS_Core.Resources.Configuration.ConnectionString, CommandType.Text, sSQL);
                }
            }
            catch (Exception Ex)
            {
            }
        }

        //Sprint-25 - PRIMEPOS-2379 15-Mar-2017 JY Added
        public void AddPSEItemListAccessRight(string strUserName)
        {
            try
            {
                string sSQL = "SELECT UserID FROM Util_UserRights WHERE UserID = '" + strUserName.Replace("'", "''") + "' AND ModuleID = 3 AND ScreenID = 74";
                object objValue = DataHelper.ExecuteScalar(sSQL);
                if (objValue == null)
                {
                    sSQL = "INSERT INTO Util_UserRights(UserID,ModuleID,ScreenID,PermissionID,isAllowed) VALUES('" + strUserName.Replace("'", "''") + "',3,74,NULL,1),('" + strUserName.Replace("'", "''") + "',3,74,-999,1),('" + strUserName.Replace("'", "''") + "',3,74,-998,1)";
                    DataHelper.ExecuteNonQuery(POS_Core.Resources.Configuration.ConnectionString, CommandType.Text, sSQL);
                }
            }
            catch (Exception Ex)
            {

            }
        }

        #region Sprint-26 - PRIMEPOS-2416 25-Jul-2017 JY Added
        public void AddQuantityOverrideAccessRight(string strUserName)
        {
            try
            {
                string sSQL = "SELECT UserID FROM Util_UserRights WHERE UserID = '" + strUserName.Replace("'", "''") + "' AND ModuleID = 1 AND ScreenID = 1 AND PermissionID = 76";
                object objValue = DataHelper.ExecuteScalar(sSQL);
                if (objValue == null)
                {
                    sSQL = "INSERT INTO Util_UserRights(UserID,ModuleID,ScreenID,PermissionID,isAllowed) VALUES('" + strUserName.Replace("'", "''") + "',1,1,76,1)";
                    DataHelper.ExecuteNonQuery(POS_Core.Resources.Configuration.ConnectionString, CommandType.Text, sSQL);
                }
            }
            catch (Exception Ex)
            {
            }
        }
        #endregion

        #region Sprint-26 - PRIMEPOS-2383 08-Aug-2017 JY Added
        public void AddStandAloneReturnAccessRight(string strUserName)
        {
            try
            {
                string sSQL = "SELECT UserID FROM Util_UserRights WHERE UserID = '" + strUserName.Replace("'", "''") + "' AND ModuleID = 1 AND ScreenID = 1 AND PermissionID = 77";
                object objValue = DataHelper.ExecuteScalar(sSQL);
                if (objValue == null)
                {
                    sSQL = "INSERT INTO Util_UserRights(UserID,ModuleID,ScreenID,PermissionID,isAllowed) VALUES('" + strUserName.Replace("'", "''") + "',1,1,77,1)";
                    DataHelper.ExecuteNonQuery(POS_Core.Resources.Configuration.ConnectionString, CommandType.Text, sSQL);
                }
            }
            catch (Exception Ex)
            {
            }
        }
        #endregion

        #region PRIMEPOS-2539 06-Jul-2018 JY Added
        public void AddAllowCheckPaymentAccessRight(string strUserName)
        {
            try
            {
                string sSQL = "SELECT UserID FROM Util_UserRights WHERE UserID = '" + strUserName.Replace("'", "''") + "' AND ModuleID = 1 AND ScreenID = 1 AND PermissionID = 80";
                object objValue = DataHelper.ExecuteScalar(sSQL);
                if (objValue == null)
                {
                    sSQL = "INSERT INTO Util_UserRights(UserID,ModuleID,ScreenID,PermissionID,isAllowed) VALUES('" + strUserName.Replace("'", "''") + "',1,1,80,1)";
                    DataHelper.ExecuteNonQuery(POS_Core.Resources.Configuration.ConnectionString, CommandType.Text, sSQL);
                }
            }
            catch (Exception Ex)
            {
            }
        }
        #endregion

        #region PRIMEPOS-2464 26-Mar-2020 JY Added
        public void AddDisplayItemCostAccessRight(string strUserName)
        {
            try
            {
                string sSQL = "SELECT UserID FROM Util_UserRights WHERE UserID = '" + strUserName.Replace("'", "''") + "' AND ModuleID = 2 AND ScreenID = 6 AND PermissionID = 81";
                object objValue = DataHelper.ExecuteScalar(sSQL);
                if (objValue == null)
                {
                    sSQL = "INSERT INTO Util_UserRights(UserID,ModuleID,ScreenID,PermissionID,isAllowed) VALUES('" + strUserName.Replace("'", "''") + "',2,6,81,1)";
                    DataHelper.ExecuteNonQuery(POS_Core.Resources.Configuration.ConnectionString, CommandType.Text, sSQL);
                }
            }
            catch (Exception Ex)
            {
            }
        }
        #endregion

        #region PRIMEPOS-2577 15-Aug-2018 JY Added
        public void ChangePasswordAtLogin(string UserName)
        {
            POS_Core.ErrorLogging.Logs.Logger(clsPOSDBConstants.Log_Module_Login, " ChangePasswordAtLogin()", clsPOSDBConstants.Log_Entering);
            string sSQL = string.Empty;
            DataSet oDS;
            POS_Core.BusinessRules.Search oSearch = new POS_Core.BusinessRules.Search();
            try
            {
                sSQL = "SELECT " + clsPOSDBConstants.Users_Fld_ChangePasswordAtLogin + " FROM Users WHERE " + clsPOSDBConstants.Users_Fld_UserID + " = '" + UserName.Replace("'", "''") + "'";
                oDS = oSearch.SearchData(sSQL);
                if (oDS.Tables[0].Rows.Count > 0)
                {
                    Boolean bStatus = false;
                    try
                    {
                        bStatus = POS_Core.Resources.Configuration.convertNullToBoolean(oDS.Tables[0].Rows[0][clsPOSDBConstants.Users_Fld_ChangePasswordAtLogin]);
                    }
                    catch { }
                    
                    if (bStatus == true)
                        ErrorHandler.throwCustomError(POSErrorENUM.User_ChangePasswordAtLogin);
                }
            }
            catch (NullReferenceException)
            {
                ErrorHandler.throwCustomError(POSErrorENUM.User_InvalidUserName);
            }
            catch (Exception exp)
            {
                throw (exp);
            }
            POS_Core.ErrorLogging.Logs.Logger(clsPOSDBConstants.Log_Module_Login, " ChangePasswordAtLogin()", clsPOSDBConstants.Log_Exiting);
        }
        #endregion

        #region PRIMEPOS-2676 20-May-2019 JY Added
        public void AddTransSettingsAccessRight(string strUserName)
        {
            try
            {
                string sSQL = "SELECT UserID FROM Util_UserRights WHERE UserID = '" + strUserName.Replace("'", "''") + "' AND ModuleID = 4 AND ScreenID = 75";
                object objValue = DataHelper.ExecuteScalar(sSQL);
                if (objValue == null)
                {
                    sSQL = "INSERT INTO Util_UserRights(UserID,ModuleID,ScreenID,isAllowed) VALUES('" + strUserName.Replace("'", "''") + "',4,75,1)";
                    DataHelper.ExecuteNonQuery(Configuration.ConnectionString, CommandType.Text, sSQL);
                }
            }
            catch (Exception Ex)
            {
            }
        }

        public void AddRxSettingsAccessRight(string strUserName)
        {
            try
            {
                string sSQL = "SELECT UserID FROM Util_UserRights WHERE UserID = '" + strUserName.Replace("'", "''") + "' AND ModuleID = 4 AND ScreenID = 76";
                object objValue = DataHelper.ExecuteScalar(sSQL);
                if (objValue == null)
                {
                    sSQL = "INSERT INTO Util_UserRights(UserID,ModuleID,ScreenID,isAllowed) VALUES('" + strUserName.Replace("'", "''") + "',4,76,1)";
                    DataHelper.ExecuteNonQuery(Configuration.ConnectionString, CommandType.Text, sSQL);
                }
            }
            catch (Exception Ex)
            {
            }
        }

        public void AddPrimePOSettingsAccessRight(string strUserName)
        {
            try
            {
                string sSQL = "SELECT UserID FROM Util_UserRights WHERE UserID = '" + strUserName.Replace("'", "''") + "' AND ModuleID = 4 AND ScreenID = 77";
                object objValue = DataHelper.ExecuteScalar(sSQL);
                if (objValue == null)
                {
                    sSQL = "INSERT INTO Util_UserRights(UserID,ModuleID,ScreenID,isAllowed) VALUES('" + strUserName.Replace("'", "''") + "',4,77,1)";
                    DataHelper.ExecuteNonQuery(Configuration.ConnectionString, CommandType.Text, sSQL);
                }
            }
            catch (Exception Ex)
            {
            }
        }

        public void AddCLPSettingsAccessRight(string strUserName)
        {
            try
            {
                string sSQL = "SELECT UserID FROM Util_UserRights WHERE UserID = '" + strUserName.Replace("'", "''") + "' AND ModuleID = 4 AND ScreenID = 78";
                object objValue = DataHelper.ExecuteScalar(sSQL);
                if (objValue == null)
                {
                    sSQL = "INSERT INTO Util_UserRights(UserID,ModuleID,ScreenID,isAllowed) VALUES('" + strUserName.Replace("'", "''") + "',4,78,1)";
                    DataHelper.ExecuteNonQuery(Configuration.ConnectionString, CommandType.Text, sSQL);
                }
            }
            catch (Exception Ex)
            {
            }
        }
        #endregion

        #region PRIMEPOS-3141 27-Oct-2022 JY Added
        public void AddInventoryReceivedAccessRight(string strUserName)
        {
            try
            {
                string sSQL = "SELECT UserID FROM Util_UserRights WHERE UserID = '" + strUserName.Replace("'", "''") + "' AND ModuleID = 2 AND ScreenID = 4 AND PermissionID = 91";
                object objValue = DataHelper.ExecuteScalar(sSQL);
                if (objValue == null)
                {
                    sSQL = "INSERT INTO Util_UserRights(UserID, ModuleID, ScreenID, PermissionID, isAllowed)" +
                        " SELECT UserID, ModuleID, ScreenID, 91 AS PermissionID, isAllowed FROM Util_UserRights WHERE UserID = '" + strUserName.Replace("'", "''") + "' AND ModuleID = 2 AND ScreenID = 4 AND PermissionID = -999";
                    DataHelper.ExecuteNonQuery(Configuration.ConnectionString, CommandType.Text, sSQL);
                }
            }
            catch (Exception Ex)
            {
            }
        }
        #endregion
    }
}
