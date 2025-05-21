
namespace POS_Core.BusinessRules
{
    using System;
    using System.Data;
    using POS_Core.DataAccess;
    using POS_Core.CommonData.Tables;
    using POS_Core.CommonData.Rows;
    using POS_Core.CommonData;
    using POS_Core.ErrorLogging;
    //using POS.Resources;
    using NLog;

    /// <summary>
    /// This is Business Tier Class for Timesheet.
    /// This class contains all business rules related to Timesheet.
    /// </summary>
    public class Timesheet : IDisposable
    {
        private static ILogger logger = LogManager.GetCurrentClassLogger();

        #region Persist Methods
        /// <summary>
        /// A method for inserting and updating, Delete Timesheet data it will delete rows from database which has been deleted from dataset. 
        /// </summary>
        /// <param name="updates">It is Timesheet type dataset class. It contains all information of Timesheets.</param>
        public void Persist(TimesheetData updates)
        {
            try
            {
                //UserPriviliges.IsUserHasPriviliges(UserPriviliges.Modules.POSTransaction.ID, UserPriviliges.Screens.Timesheets.ID, -998);
                using (TimesheetSvr dao = new TimesheetSvr())
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
                logger.Fatal(ex, "Persist(TimesheetData updates)");  //PRIMEPOS-2971 07-Jun-2021 JY Added
                //ErrorHandler.throwException(ex, "", "");    //PRIMEPOS-2971 07-Jun-2021 JY Commented as no need to log it in errorlog
            }
        }

        public void AddToTimesheet(DataSet updates)
        {
            try
            {
                //UserPriviliges.IsUserHasPriviliges(UserPriviliges.Modules.POSTransaction.ID, UserPriviliges.Screens.Timesheets.ID, -998);
                using (TimesheetSvr dao = new TimesheetSvr())
                {
                    dao.AddToTimesheet(updates);
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
                logger.Fatal(ex, "AddToTimesheet(DataSet updates)");  //PRIMEPOS-2971 07-Jun-2021 JY Added
                //ErrorHandler.throwException(ex, "", "");  //PRIMEPOS-2971 07-Jun-2021 JY Commented as no need to log it in errorlog
            }
        }

        #endregion

        #region Get Methods
        /// <summary>
        /// Fills a Timesheet type DataSet with all Timesheets based on a condition.
        /// </summary>
        /// <param name="whereClause">Condition for filtering data.</param>
        /// <returns>Returns Timesheet type Dataset</returns>
        public TimesheetData PopulateList(string whereClause)
        {
            try
            {
                using (TimesheetSvr dao = new TimesheetSvr())
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
                logger.Fatal(ex, "PopulateList(string whereClause)");  //PRIMEPOS-2971 07-Jun-2021 JY Added
                //ErrorHandler.throwException(ex, "", "");  //PRIMEPOS-2971 07-Jun-2021 JY Commented as no need to log it in errorlog
                return null;
            }
        }

        public TimesheetData PopulateByUserID(string suserID)
        {
            try
            {
                using (TimesheetSvr dao = new TimesheetSvr())
                {
                    return dao.PopulateByUserID(suserID);
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
                logger.Fatal(ex, "PopulateByUserID(string suserID)");  //PRIMEPOS-2971 07-Jun-2021 JY Added
                //ErrorHandler.throwException(ex, "", "");  //PRIMEPOS-2971 07-Jun-2021 JY Commented as no need to log it in errorlog
                return null;
            }
        }

        public DataSet SearchDataFromEvents(System.String sUserID, DateTime sStartDate, DateTime sEndDate, System.Boolean bIncludeProcessedTimesheet)   //Sprint-25 - PRIMEPOS-2253 24-Mar-2017 JY Added bIncludeProcessedTimesheet 
        {
            try
            {
                using (TimesheetSvr dao = new TimesheetSvr())
                {
                    return dao.SearchDataFromEvents(sUserID,sStartDate,sEndDate, bIncludeProcessedTimesheet);   //Sprint-25 - PRIMEPOS-2253 24-Mar-2017 JY Added bIncludeProcessedTimesheet 
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
                logger.Fatal(ex, "SearchDataFromEvents(System.String sUserID, DateTime sStartDate, DateTime sEndDate, System.Boolean bIncludeProcessedTimesheet)");  //PRIMEPOS-2971 07-Jun-2021 JY Added
                //ErrorHandler.throwException(ex, "", "");  //PRIMEPOS-2971 07-Jun-2021 JY Commented as no need to log it in errorlog
                return null;
            }
        }

        public TimesheetData GetLastTimeIn(string suserID)
        {
            try
            {
                using (TimesheetSvr dao = new TimesheetSvr())
                {
                    return dao.GetLastTimeIn(suserID);
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
                logger.Fatal(ex, "GetLastTimeIn(string suserID)");  //PRIMEPOS-2971 07-Jun-2021 JY Added
                //ErrorHandler.throwException(ex, "", "");  //PRIMEPOS-2971 07-Jun-2021 JY Commented as no need to log it in errorlog
                return null;
            }
        }

        #endregion

        #region Validation Methods
        /// <summary>
        /// Get Timesheet data with respect to Timesheet code.
        /// </summary>
        /// <param name="Timesheetcode">This is database field of cutomer.</param>
        /// <returns>Collection of Timesheet type records.</returns>
        ///<exception cref="">ss</exception>
        public virtual TimesheetData Populate(System.Int64 iID)
        {
            try
            {
                using (TimesheetSvr dao = new TimesheetSvr())
                {
                    return dao.Populate(iID);
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
                logger.Fatal(ex, "Populate(System.Int64 iID)");  //PRIMEPOS-2971 07-Jun-2021 JY Added
                //ErrorHandler.throwException(ex, "", "");  //PRIMEPOS-2971 07-Jun-2021 JY Commented as no need to log it in errorlog
                return null;
            }
        }

        public void checkIsValidPrimaryKey(TimesheetData updates)
        {
            TimesheetTable table = (TimesheetTable)updates.Tables[clsPOSDBConstants.Timesheet_tbl];
            foreach (TimesheetRow row in table.Rows)
            {
                if (this.Populate(row.ID).Tables[clsPOSDBConstants.Timesheet_tbl].Rows.Count != 0)
                {
                    throw new Exception("Primary key violation for Timesheet ");
                }
            }
        }

        /// <summary>
        /// Check whether an attempted delete is valid for Timesheet. This function has no implementation. 
        /// </summary>
        /// <param name="updates"></param>
        public void checkIsValidDelete(TimesheetData updates)
        {
        }
        #endregion
        /// <summary>
        /// Dispose Timesheet contents.
        /// </summary>
        public void Dispose()
        {
            GC.SuppressFinalize(true);
        }

    }
}
