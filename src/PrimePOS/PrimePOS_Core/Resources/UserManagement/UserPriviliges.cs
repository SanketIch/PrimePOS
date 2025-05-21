using System;
using System.Data;
using POS_Core.CommonData;
using POS_Core.UserManagement;
using POS_Core.Resources;
using POS_Core.Resources.DelegateHandler;
using Resources;

namespace POS_Core.Resources
{
    /// <summary>
    /// Summary description for UserPriviliges.
    /// </summary>
    public class UserPriviliges
    {
        public static sm_Modules Modules=new sm_Modules();
        public static sm_Screens Screens=new sm_Screens();
        public static sm_Permissions Permissions=new sm_Permissions();

		public UserPriviliges()
		{
		}
		//public static void UserHasPriviliges(string colName)
		//{
		//	if (!IsUserHasPriviliges(colName))
		//		ErrorHandler.throwCustomError(POSErrorENUM.Users_UserNotHavePriviligesForThisAction);
		//}



        



		public static bool IsUserHasPriviliges(int ModuleID)
		{
			return CheckPriviliges(ModuleID,0,0,Configuration.UserName);
		}
		
		public static bool IsUserHasPriviliges(int ModuleID,int ScreenID)
		{
			return CheckPriviliges(ModuleID,ScreenID,0,Configuration.UserName);
		}

		public static bool IsUserHasPriviliges(int ModuleID,int ScreenID,int PermissionID)
		{
			return CheckPriviliges(ModuleID,ScreenID,PermissionID,Configuration.UserName);
		}

		public static bool IsUserHasPriviliges(int ModuleID, string UserName)
		{
			return CheckPriviliges(ModuleID,0,0,UserName);
		}
		
		public static bool IsUserHasPriviliges(int ModuleID,int ScreenID, string UserName)
		{
			return CheckPriviliges(ModuleID,ScreenID,0,UserName);
		}

		public static bool IsUserHasPriviliges(int ModuleID,int ScreenID,int PermissionID, string UserName)
		{
			return CheckPriviliges(ModuleID,ScreenID,PermissionID,UserName);
		}

        public static bool IsUserHasPriviligesToOverrideInvoiceDiscount(string UserName, decimal InvoiceDiscValue)
        {
            IDbCommand cmd = DataFactory.CreateCommand();
			string sSQL = "";
	
			IDataReader dr;

			IDbConnection conn = DataFactory.CreateConnection();

            conn.ConnectionString = Configuration.ConnectionString;
			
			conn.Open();
            bool RetVal = false;
			try 
			{

				sSQL = String.Concat( "SELECT " 
					, clsPOSDBConstants.Users_Fld_MaxDiscountLimit
					," FROM " 
					, clsPOSDBConstants.Users_tbl
					, "  WHERE "
                    , clsPOSDBConstants.Users_Fld_UserID, " = '", UserName.Replace("'", "''"), "'");

				cmd.CommandType = CommandType.Text;
				cmd.CommandText = sSQL;
				cmd.Connection = conn;

				dr = cmd.ExecuteReader();
				dr.Read();

                if (!dr.IsDBNull(dr.GetOrdinal(clsPOSDBConstants.Users_Fld_MaxDiscountLimit)))
                {
                    decimal Discount = dr.GetDecimal(dr.GetOrdinal(clsPOSDBConstants.Users_Fld_MaxDiscountLimit));//added by shitaljit to define % disc user can give
                    if (Discount >= InvoiceDiscValue)
                    {
                        RetVal = true;
                    }
                }
				conn.Close();
			}
			catch(Exception exp) 
			{
				conn.Close();
				throw(exp);
			}
            return RetVal;
        }
       public static bool IsUserHasPriviligesToOverrideUsePayOutCat(string strUserName, long intPayOutId)
        {
            IDbCommand cmd = DataFactory.CreateCommand();
            string sSQL = "";
            IDataReader dr;
            IDbConnection conn = DataFactory.CreateConnection();
            conn.ConnectionString = Configuration.ConnectionString;
            conn.Open();
            bool RetVal = false;
            try
            {
                sSQL = String.Concat("SELECT "
                    , clsPOSDBConstants.Util_UserOptionDetailRights_Fld_DetailId
                    , " FROM "
                    , clsPOSDBConstants.Util_UserOptionDetailRights_tbl
                    , "  WHERE "
                    , clsPOSDBConstants.Users_Fld_UserID, " = '" + strUserName + "'"
                , " And "
                , clsPOSDBConstants.Util_UserOptionDetailRights_Fld_DetailId, " = '" + intPayOutId + "'");

                cmd.CommandType = CommandType.Text;
                cmd.CommandText = sSQL;
                cmd.Connection = conn;

                dr = cmd.ExecuteReader();


                if (dr.Read())
                {

                    RetVal = true;

                }
                else
                {
                    RetVal = false;
                }
                conn.Close();
            }
            catch (Exception exp)
            {
                RetVal = false;
                conn.Close();
                throw (exp);
            }
            return RetVal;
        }
        public static bool IsUserHasPriviligesToOverrideTransactionAmount(string UserName, decimal TrasactionAmount)
        {
            IDbCommand cmd = DataFactory.CreateCommand();
            string sSQL = "";

            IDataReader dr;

            IDbConnection conn = DataFactory.CreateConnection();

            conn.ConnectionString = Configuration.ConnectionString;

            conn.Open();
            bool RetVal = false;
            try
            {
                sSQL = String.Concat("SELECT "
                    , clsPOSDBConstants.Users_Fld_MaxTransactionLimit
                    , " FROM "
                    , clsPOSDBConstants.Users_tbl
                    , "  WHERE "
                    , clsPOSDBConstants.Users_Fld_UserID, " = '", UserName.Replace("'", "''"), "'");

                cmd.CommandType = CommandType.Text;
                cmd.CommandText = sSQL;
                cmd.Connection = conn;

                dr = cmd.ExecuteReader();
                dr.Read();

                if (!dr.IsDBNull(dr.GetOrdinal(clsPOSDBConstants.Users_Fld_MaxTransactionLimit)))
                {
                    decimal TransAmountLimit = dr.GetDecimal(dr.GetOrdinal(clsPOSDBConstants.Users_Fld_MaxTransactionLimit));//added by Ravindra to fro transaction limit
                    if (TransAmountLimit >= TrasactionAmount)  
                    {
                        RetVal = true;
                    }
                }
                else  //This scenario will not be possible, but in case it is null then it should not succeed 
                {
                    RetVal = false;
                }
                conn.Close();
            }
            catch (Exception exp)
            {
                conn.Close();
                throw (exp);
            }
            return RetVal;
        }

        //Sprint-23 - PRIMEPOS-2303 24-May-2016 JY Added
        public static bool IsUserHasPriviligesToOverrideReturnTransAmount(string UserName, decimal TrasactionAmount)
        {
            IDbCommand cmd = DataFactory.CreateCommand();
            IDataReader dr;
            IDbConnection conn = DataFactory.CreateConnection();
            string sSQL = string.Empty;

            conn.ConnectionString = Configuration.ConnectionString;
            conn.Open();
            bool RetVal = false;
            try
            {
                sSQL = String.Concat("SELECT " + clsPOSDBConstants.Users_Fld_MaxReturnTransLimit + " FROM " + clsPOSDBConstants.Users_tbl
                    + "  WHERE " + clsPOSDBConstants.Users_Fld_UserID + " = '" + UserName.Replace("'", "''") + "'");

                cmd.CommandType = CommandType.Text;
                cmd.CommandText = sSQL;
                cmd.Connection = conn;

                dr = cmd.ExecuteReader();
                dr.Read();

                if (!dr.IsDBNull(dr.GetOrdinal(clsPOSDBConstants.Users_Fld_MaxReturnTransLimit)))
                {
                    decimal ReturnTransAmountLimit = dr.GetDecimal(dr.GetOrdinal(clsPOSDBConstants.Users_Fld_MaxReturnTransLimit));
                    if (ReturnTransAmountLimit >= TrasactionAmount)
                    {
                        RetVal = true;
                    }
                }
                else  //This scenario will not be possible, but in case it is null then it should not succeed 
                {
                    RetVal = false;
                }
            }
            catch (Exception exp)
            {
                throw (exp);
            }
            finally
            {
                conn.Close();
            }
            return RetVal;
        }

        //Sprint-25 - PRIMEPOS-2411 21-Apr-2017 JY Added 
        public static bool IsUserHasPriviligesToOverrideTenderedAmount(string UserName, decimal TrasactionAmount)
        {
            bool RetVal = false;
            try
            {
                string sSQL = String.Concat("SELECT " + clsPOSDBConstants.Users_Fld_MaxTenderedAmountLimit + " FROM " + clsPOSDBConstants.Users_tbl
                    + "  WHERE " + clsPOSDBConstants.Users_Fld_UserID + " = '" + UserName.Replace("'", "''") + "'");

                using (IDbConnection conn = DataFactory.CreateConnection(Configuration.ConnectionString))
                {
                    using (IDbCommand cmd = DataFactory.CreateCommand())
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = sSQL;
                        cmd.Connection = conn;

                        IDataReader dr;
                        dr = cmd.ExecuteReader();
                        dr.Read();

                        if (!dr.IsDBNull(dr.GetOrdinal(clsPOSDBConstants.Users_Fld_MaxTenderedAmountLimit)))
                        {
                            decimal MaxTenderedAmountLimit = dr.GetDecimal(dr.GetOrdinal(clsPOSDBConstants.Users_Fld_MaxTenderedAmountLimit));
                            if (MaxTenderedAmountLimit >= TrasactionAmount)
                            {
                                RetVal = true;
                            }
                        }
                        else //This scenario will not be possible, but in case it is null then it should not succeed 
                        {
                            RetVal = false;
                        }
                    }
                }
            }
            catch (Exception exp)
            {
                throw (exp);
            }
            return RetVal;
        }

        private static bool CheckPriviliges(int ModuleID,int ScreenID,int PermissionID,string userName)
		{
			IDbCommand cmd = DataFactory.CreateCommand();
			string sSQL = "";
			
			int returnValue = 0;
			IDbConnection conn = DataFactory.CreateConnection();
			conn.ConnectionString = Configuration.ConnectionString;

			conn.Open();

			try 
			{

//				sSQL=string.Concat( "select ",clsPOSDBConstants.Users_Fld_Role, " from " ,clsPOSDBConstants.Users_tbl,
//									" where " ,clsPOSDBConstants.Users_Fld_UserID , " = '" , userName , "'");
//
//
//
//				cmd.CommandType = CommandType.Text;
//				cmd.CommandText = sSQL;
//				cmd.Connection = conn;
//
//				returnValue = Convert.ToInt32( cmd.ExecuteScalar());

				//if ((UserRoleENUM)returnValue==UserRoleENUM.Supervisor)
				//{
				//	conn.Close();
				//	return true;
				//}
				//else
				{
					sSQL= "select isAllowed from util_userrights " +
						" where " + clsPOSDBConstants.Users_Fld_UserID + " = '" + userName + "'";

					if (ModuleID!=0)
					{
						sSQL+=" and ModuleID=" + ModuleID.ToString();
					}
					else
					{
						sSQL+=" and ModuleID is null" ;
					}

					if (ScreenID!=0)
					{
						sSQL+=" and ScreenID=" + ScreenID.ToString();
					}
					else
					{
						sSQL+=" and ScreenID is null" ;
					}

					if (PermissionID!=0)
					{
						sSQL+=" and PermissionID=" + PermissionID.ToString();
					}
					else
					{
						sSQL+=" and PermissionID is null " ;
					}

					cmd.CommandType = CommandType.Text;
					cmd.CommandText = sSQL;
					cmd.Connection = conn;

					returnValue = Convert.ToInt32( cmd.ExecuteScalar());
					conn.Close();
					return ((returnValue==0)?false:true);
				}
			}
			catch(NullReferenceException )
			{
				conn.Close();
				return false;
			}
			catch(Exception ) 
			{
				conn.Close();
				return false;
			}
		}

        public static bool getPermission(int ModuleID, int ScreenID, int PermissionID, string strPrevlige)
        {
            string sUserID = "";
            return getPermission(ModuleID, ScreenID, PermissionID, strPrevlige, out sUserID);  //Sprint-26 - PRIMEPOS-2416 05-Jul-2017 JY need to strPrevlige value
        }
        /// <summary>
        /// Author: Shitaljit 
        /// This function will set the window title while showing user login for security as just giving 
        /// Security as title users are not getting for wich security reason it is asking for login
        /// </summary>
        /// <param name="ModuleID"></param>
        /// <param name="ScreenID"></param>
        /// <param name="PermissionID"></param>
        /// <param name="sUserID"></param>
        /// <returns></returns>

        private static string SecurityLoginWindowTitle(string PermissionID)
        {
            string strSecurityLoginWindowTitle = string.Empty;
            try
            {
                switch (PermissionID)
                {
                    case "5":   //PRIMEPOS-2639 22-Feb-2019 JY Added
                        strSecurityLoginWindowTitle = "Security Override For on-hold transaction";
                        break;
                    case "6":  //PRIMEPOS-2575 07-Aug-2018 JY Added
                        strSecurityLoginWindowTitle = "Security Override For Trans Deletion";
                        break;
                    case "7":  //PRIMEPOS-2584 24-Sep-2018 JY Added 
                        strSecurityLoginWindowTitle = "Security Override For ROA";
                        break;
                    case "36": //PRIMEPOS-2514 09-May-2018 JY Added for discount override
                        strSecurityLoginWindowTitle = "Security Override For Discount";
                        break;
                    case "38": //Sprint-26 - PRIMEPOS-2416 05-Jul-2017 JY Added for UserPriviliges.Permissions.PriceOverridefromPOSTrans.Name
                        strSecurityLoginWindowTitle = "Security Override For " + UserPriviliges.Permissions.PriceOverridefromPOSTrans.Name;
                        break;
                    case "52": //Sprint-26 - PRIMEPOS-2416 05-Jul-2017 JY Added for UserPriviliges.Permissions.AllowCashback.Name
                        strSecurityLoginWindowTitle = "Security Override For " + UserPriviliges.Permissions.AllowCashback.Name;
                        break;
                    case "56": //Sprint-26 - PRIMEPOS-2416 05-Jul-2017 JY Added for UserPriviliges.Permissions.PriceOverrideForRXItemsfromPOSTrans.Name
                        strSecurityLoginWindowTitle = "Security Override For " + UserPriviliges.Permissions.PriceOverrideForRXItemsfromPOSTrans.Name;
                        break;
                    case "62": //Sprint-26 - PRIMEPOS-2416 05-Jul-2017 JY Added for UserPriviliges.Permissions.MakeCouponPayment.Name
                        strSecurityLoginWindowTitle = "Security Override For " + UserPriviliges.Permissions.MakeCouponPayment.Name;
                        break;
                    case "64"://UserPriviliges.Permissions.PriceOverrideLessThanCostPrice.ID
                        strSecurityLoginWindowTitle = "Security Override For Price Override/Selling Price Less Than Cost Price";
                        break;
                    case "68": //PRIMEPOS-2510 26-Apr-2018 JY Added for TaxOverride
                        strSecurityLoginWindowTitle = "Tax override for OTC";
                        break;
                    case "69": //PRIMEPOS-2510 26-Apr-2018 JY Added for TaxOverrideAll
                        strSecurityLoginWindowTitle = "Tax override for all OTC";
                        break;
                    case "70"://UserPriviliges.Screns.ChangeLoginUserPassword.ID
                        strSecurityLoginWindowTitle = "Security Override For Change Login User Password";
                        break;
                    case "73": //Sprint-26 - PRIMEPOS-2416 05-Jul-2017 JY Added for UserPriviliges.Screens.DeleteCreditCardProfiles.Name
                        strSecurityLoginWindowTitle = "Security Override For " + UserPriviliges.Screens.DeleteCreditCardProfiles.Name;
                        break;
                    case "76": //Sprint-26 - PRIMEPOS-2416 05-Jul-2017 JY Added for UserPriviliges.Permissions.QuantityOverride
                        strSecurityLoginWindowTitle = "Security Override For " + UserPriviliges.Permissions.QuantityOverride.Name;
                        break;
                    case "77": //Sprint-26 - PRIMEPOS-2383 08-Aug-2017 JY Added for UserPriviliges.Permissions.StandAloneReturn
                        strSecurityLoginWindowTitle = "Security Override For " + UserPriviliges.Permissions.StandAloneReturn.Name;
                        break;                    
                    case "78": //PRIMEPOS-2510 26-Apr-2018 JY Added for TaxOverrideForRx
                        strSecurityLoginWindowTitle = "Tax override for RX";
                        break;
                    case "79": //PRIMEPOS-2510 26-Apr-2018 JY Added for TaxOverrideAllForRx
                        strSecurityLoginWindowTitle = "Tax override for all RX";
                        break;                    
                }
            }
            catch (Exception Ex)
            {
                return string.Empty;
            }

            return strSecurityLoginWindowTitle;
        }

        //Sprint-26 - PRIMEPOS-2416 05-Jul-2017 JY Added new parameter as "strPrevlige" which is used to pop up message before override action
        public static bool getPermission(int ModuleID, int ScreenID, int PermissionID, string strPrevlige, out string sUserID)
		{
            sUserID = "";
            try
			{
				string strPermission=ModuleID.ToString() + "," + ScreenID.ToString() + "," + PermissionID.ToString();
                if (IsUserHasPriviliges(ModuleID, ScreenID, PermissionID) == true)
                {
                    sUserID = Configuration.UserName;
                    return true;
                }
                else
                {
                    //UserManagement.clsLogin oLogin = new UserManagement.clsLogin();
                    //return oLogin.loginForPreviliges(strPermission, out sUserID);Commented By shitaljit
                    //Added by shitaljit to set window Title for the security login screen with proper title.
                    string strSecurityLoginWindowTitle = string.Empty;
                    int SwichCaseValue = 0;
                    SwichCaseValue = PermissionID;
                    strSecurityLoginWindowTitle = SecurityLoginWindowTitle(Configuration.convertNullToString(SwichCaseValue));
                    if(string.IsNullOrEmpty(strSecurityLoginWindowTitle) == true)
                    {
                        SwichCaseValue = PermissionID;
                        strSecurityLoginWindowTitle = SecurityLoginWindowTitle(Configuration.convertNullToString(SwichCaseValue));
                    }
                    return clsCoreLogin.loginForPreviliges(strPrevlige, strPermission, out sUserID, strSecurityLoginWindowTitle);   //Sprint-26 - PRIMEPOS-2416 05-Jul-2017 JY changed strPermission to strPrevlige
                }
			}
			catch (Exception )
			{
				return false;
			}
		}

        //Sprint-24 - PRIMEPOS-2290 16-Jan-2017 JY Added
        public static bool IsUserHasPriviligesToOverrideAllowHouseChargePaytype(string UserName)
        {
            bool bReturn = false;
            try
            {
                string sSQL = "SELECT isAllowed FROM Util_UserRights WHERE UserID = '" + UserName.Replace("'", "''") + "' AND ModuleID = 1 AND ScreenID = 1 AND PermissionID = 73";
                object objValue = DataHelper.ExecuteScalar(sSQL);
                if (objValue == null)
                {
                    bReturn = false;
                }
                else
                {
                    bReturn = Configuration.convertNullToBoolean(objValue);
                }
                return bReturn;
            }
            catch(Exception Ex)
            {
                return false;
            }
        }

    }
}
