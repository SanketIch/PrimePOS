using System;
using System.Collections.Generic;
using System.Text;
using POS_Core.CommonData;
using POS_Core.CommonData.Tables;
using POS_Core.CommonData.Rows;
using POS_Core.DataAccess;
using POS_Core.ErrorLogging;
//using POS.Resources;
using System.Data;
using POS_Core.Resources;
using NLog;

namespace POS_Core.BusinessRules
{
   public class FileHistory : IDisposable
    {

        private static ILogger logger = LogManager.GetCurrentClassLogger();

        #region Persist Methods
        /// <summary>
        /// A method for inserting and updating, Delete Customer data it will delete rows from database which has been deleted from dataset. 
        /// </summary>
        /// <param name="updates">It is customer type dataset class. It contains all information of customers.</param>

        public void Persist(FileHistoryData updates)
        {
            try
            {
                UserPriviliges.IsUserHasPriviliges(UserPriviliges.Modules.POSTransaction.ID, UserPriviliges.Screens.Customers.ID, -998);
                checkIsValidData(updates);
                using (FileHistorySvr dao = new FileHistorySvr())
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
                logger.Fatal(ex, "Persist(FileHistoryData updates)");  //PRIMEPOS-2971 07-Jun-2021 JY Added
                //ErrorHandler.throwException(ex, "", "");  //PRIMEPOS-2971 07-Jun-2021 JY Commented as no need to log it in errorlog
            }
        }
        #endregion

        #region Get Methods
        /// <summary>
        /// Fills a Custemer type DataSet with all Customers based on a condition.
        /// </summary>
        /// <param name="whereClause">Condition for filtering data.</param>
        /// <returns>Returns customer type Dataset</returns>
       public FileHistoryData PopulateList(string whereClause)
        {
            try
            {
                using (FileHistorySvr dao = new FileHistorySvr())
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
        #endregion

        #region Validation Methods
        /// <summary>
        /// Validate a Customer.This would be the place to put field validations.
        /// </summary>
        /// <param name="updates">Contains collection of customer type data.</param>

        public void checkIsValidData(FileHistoryData updates)
        {
            FileHistoryTable table;
            FileHistoryRow oRow;

            oRow = (FileHistoryRow)updates.Tables[0].Rows[0];

            table = (FileHistoryTable)updates.Tables[0].GetChanges(DataRowState.Added);

            if (table == null)
                table = (FileHistoryTable)updates.Tables[0].GetChanges(DataRowState.Modified);
            else if ((FileHistoryTable)updates.Tables[0].GetChanges(DataRowState.Modified) != null)
                table.MergeTable((FileHistoryTable)updates.Tables[0].GetChanges(DataRowState.Modified));

            if (table == null)
                table = (FileHistoryTable)updates.Tables[0].GetChanges(DataRowState.Unchanged);
            else if ((FileHistoryTable)updates.Tables[0].GetChanges(DataRowState.Unchanged) != null)
                table.MergeTable((FileHistoryTable)updates.Tables[0].GetChanges(DataRowState.Unchanged));

            if (table == null) return;

            foreach (FileHistoryRow row in table.Rows)
            {
                //if (row.FileName == 0)
                //    ErrorHandler.throwCustomError(POSErrorENUM.Customer_CodeCanNotBeNULL);                
            }
        }

        /// <summary>
        /// Get customer data with respect to customer code.
        /// </summary>
        /// <param name="Customercode">This is database field of cutomer.</param>
        /// <returns>Collection of Customer type records.</returns>
        ///<exception cref="">ss</exception>
       public virtual FileHistoryData Populate(System.Int64 FileID)
        {
            try
            {
                using (FileHistorySvr dao = new FileHistorySvr())
                {
                    return dao.Populate(FileID,false);
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
                logger.Fatal(ex, "Populate(System.Int64 FileID)");  //PRIMEPOS-2971 07-Jun-2021 JY Added
                //ErrorHandler.throwException(ex, "", "");  //PRIMEPOS-2971 07-Jun-2021 JY Commented as no need to log it in errorlog
                return null;
            }
        }

        public void checkIsValidPrimaryKey(FileHistoryData updates)
        {
            FileHistoryTable table = (FileHistoryTable)updates.Tables[clsPOSDBConstants.FileHistory_tbl];
            
            foreach (FileHistoryRow row in table.Rows)
            {
                if (this.Populate(row.FileID).Tables[clsPOSDBConstants.FileHistory_tbl].Rows.Count != 0)
                {
                    throw new Exception("Primary key violation for Customer ");
                }
            }
        }

        /// <summary>
        /// Check whether an attempted delete is valid for Customer. This function has no implementation. 
        /// </summary>
        /// <param name="updates"></param>
       public void checkIsValidDelete(FileHistoryData updates)
        {
        }
        #endregion
       
       #region IDisposable Members
        public void Dispose()
        {
            GC.SuppressFinalize(true);
        }
        #endregion

        public long GetMaxFileID()
        {
            FileHistorySvr fileHistorySvr = new FileHistorySvr();
            return fileHistorySvr.GetMaxFileID();  
        }
    }
}
