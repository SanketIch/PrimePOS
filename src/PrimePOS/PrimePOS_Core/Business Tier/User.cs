using System;
using System.Data;
using POS_Core.DataAccess;
using POS_Core.CommonData.Tables;
using POS_Core.CommonData.Rows;
using POS_Core.CommonData;
using POS_Core.ErrorLogging;
using Resources;
namespace POS_Core.BusinessRules
{
    

    /// <summary>
    /// This is Business Tier Class for User.
    /// This class contains all business rules related to User.
    /// </summary>
    public class User : IDisposable
    {

        #region Persist Methods
        /// <summary>
        /// A method for inserting and updating, Delete User data it will delete rows from database which has been deleted from dataset. 
        /// </summary>
        /// <param name="updates">It is User type dataset class. It contains all information of Users.</param>
        public void Persist(UserData updates)
        {
            try
            {
                checkIsValidData(updates);
                using (UserSvr dao = new UserSvr())
                {
                    dao.Persist(updates);
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
                ErrorHandler.throwException(ex, "", "");
            }
        }
        #endregion

        public bool DeleteRow(string UserId)
        {
            System.Data.IDbConnection oConn = null;
            try
            {
                oConn = DataFactory.CreateConnection(POS_Core.Resources.Configuration.ConnectionString);
                UserSvr oSvr = new UserSvr();
                return oSvr.DeleteRow(UserId);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        #region Get Methods
        /// <summary>
        /// Fills a User type DataSet with all Users based on a condition.
        /// </summary>
        /// <param name="whereClause">Condition for filtering data.</param>
        /// <returns>Returns User type Dataset</returns>
        public UserData PopulateList(string whereClause)
        {
            try
            {
                using (UserSvr dao = new UserSvr())
                {
                    return dao.PopulateList(whereClause);
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
                ErrorHandler.throwException(ex, "", "");
                return null;
            }
        }

       

        #endregion

        #region Validation Methods
        /// <summary>
        /// Validate a User.This would be the place to put field validations.
        /// </summary>
        /// <param name="updates">Contains collection of User type data.</param>

        public void checkIsValidData(UserData updates)
        {
            UserTable table; 
            UserRow oRow;

            oRow = (UserRow)updates.Tables[0].Rows[0];

            table = (UserTable)updates.Tables[0].GetChanges(DataRowState.Added);

            if (table == null)
                table = (UserTable)updates.Tables[0].GetChanges(DataRowState.Modified);
            else if ((UserTable)updates.Tables[0].GetChanges(DataRowState.Modified) != null)
                table.MergeTable((UserTable)updates.Tables[0].GetChanges(DataRowState.Modified));

            if (table == null)
                table = (UserTable)updates.Tables[0].GetChanges(DataRowState.Unchanged);
            else if ((UserTable)updates.Tables[0].GetChanges(DataRowState.Unchanged) != null)
                table.MergeTable((UserTable)updates.Tables[0].GetChanges(DataRowState.Unchanged));

            if (table == null) return;

            foreach (UserRow row in table.Rows)
            {


            }
        }

        public virtual UserData GetUserByUserID(System.String UserID)
        {
            try
            {
                using (UserSvr dao = new UserSvr())
                {
                    return dao.GetUserByUserID(UserID);
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
                ErrorHandler.throwException(ex, "", "");
                return null;
            }
            
        }

        #region PRIMEPOS-2780 27-Sep-2021 JY Added
        public DataTable GetUserByID(System.Int32 ID)
        {
            try
            {
                using (UserSvr dao = new UserSvr())
                {
                    return dao.GetUserByID(ID);
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
                ErrorHandler.throwException(ex, "", "");
                return null;
            }
        }
        #endregion

        public virtual DataSet GetUserGroup(System.String UserID, System.String UserFName)
        {
            try
            {
                using (UserSvr dao = new UserSvr())
                {
                    return dao.GetUserGroup(UserID, UserFName);
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
                ErrorHandler.throwException(ex, "", "");
                return null;
            }
        }

        /// <summary>
        /// Get User data with respect to User code.
        /// </summary>
        /// <param name="Usercode">This is database field of cutomer.</param>
        /// <returns>Collection of User type records.</returns>
        ///<exception cref="">ss</exception>
        public virtual UserData Populate()
        {
            try
            {
                using (UserSvr dao = new UserSvr())
                {
                    return dao.Populate();
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
                ErrorHandler.throwException(ex, "", "");
                return null;
            }
        }


        public void checkIsValidPrimaryKey(UserData updates)
        {
            UserTable table = (UserTable)updates.Tables[clsPOSDBConstants.Users_tbl];
            foreach (UserRow row in table.Rows)
            {
                if (this.GetUserByUserID(row.UserID).Tables[clsPOSDBConstants.Users_tbl].Rows.Count != 0)
                {
                    throw new Exception("Primary key violation for User ");
                }
            }
        }

        /// <summary>
        /// Check whether an attempted delete is valid for User. This function has no implementation. 
        /// </summary>
        /// <param name="updates"></param>
        public void checkIsValidDelete(UserData updates)
        {
        }
        #endregion


        //Added By Shitaljit(QuicSolv) on May 23 2011
        /// <summary>
        /// this function will unlock the user in user table
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public bool UnlokUserRow(string userID)
      {
          try
          {
              UserSvr oUserSvr = new  UserSvr();
              bool retValue = false;
              retValue = oUserSvr.UnlockUserRow(userID);
              return retValue;
          }
          catch (Exception ex)
          {
              throw (ex);
          }

      }

        public bool resetUserPassword(string userID)
        {
            try
            {
                UserSvr oUserSvr = new UserSvr();
                bool retValue = false;
                retValue = oUserSvr.resetUserPassword(userID);
                return retValue;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
  
        }


        //Till here added By Shitaljit

        /// <summary>
        /// Dispose User contents.
        /// </summary>
        public void Dispose()
        {
            GC.SuppressFinalize(true);
        }

    }
}

