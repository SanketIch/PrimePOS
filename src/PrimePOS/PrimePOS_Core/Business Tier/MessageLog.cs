using System;
using System.Collections.Generic;
using System.Text;
using POS_Core.ErrorLogging;
using POS_Core.CommonData.Tables;
using POS_Core.CommonData.Rows;
using System.Data;
using POS_Core.DataAccess;
//using POS.Resources;
using POS_Core.CommonData;
using POS_Core.Resources;
using NLog;

namespace POS_Core.BusinessRules
{
    public class MessageLog
    {
        private static ILogger logger = LogManager.GetCurrentClassLogger();

        #region Persist Methods

        public void Persist(MessageLogData updates)
        {
            try
            {
                UserPriviliges.IsUserHasPriviliges(UserPriviliges.Modules.POSTransaction.ID, UserPriviliges.Screens.Customers.ID, -998);
                checkIsValidData(updates);
                using (MessageLogSvr dao = new MessageLogSvr())
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
                logger.Fatal(ex, "Persist(MessageLogData updates)");  //PRIMEPOS-2971 07-Jun-2021 JY Added
                //ErrorHandler.throwException(ex, "", "");    //PRIMEPOS-2971 07-Jun-2021 JY Commented as no need to log it in errorlog
            }
        }
        #endregion

        #region Get Methods
      
        public MessageLogData PopulateList(string whereClause)
        {
            try
            {
                using (MessageLogSvr dao = new MessageLogSvr())
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
                //ErrorHandler.throwException(ex, "", "");    //PRIMEPOS-2971 07-Jun-2021 JY Commented as no need to log it in errorlog
                return null;
            }
        }

        #endregion

        #region Validation Methods      

        public void checkIsValidData(MessageLogData updates)
        {
            MessageLogTable table;
            MessageLogRow oRow;

            oRow = (MessageLogRow)updates.Tables[0].Rows[0];

            table = (MessageLogTable)updates.Tables[0].GetChanges(DataRowState.Added);

            if (table == null)
                table = (MessageLogTable)updates.Tables[0].GetChanges(DataRowState.Modified);
            else if ((MessageLogTable)updates.Tables[0].GetChanges(DataRowState.Modified) != null)
                table.MergeTable((MessageLogTable)updates.Tables[0].GetChanges(DataRowState.Modified));

            if (table == null)
                table = (MessageLogTable)updates.Tables[0].GetChanges(DataRowState.Unchanged);
            else if ((MessageLogTable)updates.Tables[0].GetChanges(DataRowState.Unchanged) != null)
                table.MergeTable((MessageLogTable)updates.Tables[0].GetChanges(DataRowState.Unchanged));

            if (table == null) return;

            foreach (MessageLogRow row in table.Rows)
            {
                //if (row.CustomerCode == "")
                //    ErrorHandler.throwCustomError(POSErrorENUM.Customer_CodeCanNotBeNULL);
                //if (row.CustomerName.Trim() == "")
                //    ErrorHandler.throwCustomError(POSErrorENUM.Customer_NameCanNotBeNULL);
                //if (row.Address1.Trim() == "")
                //    ErrorHandler.throwCustomError(POSErrorENUM.Customer_Address1CanNotBeNULL);
                //if (row.City.Trim() == "")
                //    ErrorHandler.throwCustomError(POSErrorENUM.Customer_CityCanNotBeNull);
                //if (row.State.Trim() == "")
                //    ErrorHandler.throwCustomError(POSErrorENUM.Customer_StateCannotBeNull);

            }
        }

      
        public virtual MessageLogData Populate(System.DateTime date)
        {
            try
            {
                using (MessageLogSvr dao = new MessageLogSvr())
                {
                    return dao.Populate(date);
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
                logger.Fatal(ex, "Populate(System.DateTime date)");  //PRIMEPOS-2971 07-Jun-2021 JY Added
                //ErrorHandler.throwException(ex, "", "");    //PRIMEPOS-2971 07-Jun-2021 JY Commented as no need to log it in errorlog
                return null;
            }
        }

        public void checkIsValidPrimaryKey(MessageLogData updates)
        {
            MessageLogTable table = (MessageLogTable)updates.Tables[clsPOSDBConstants.MessageLog_tbl];
            foreach (MessageLogRow row in table.Rows)
            {
                if (this.Populate(row.LogDate).Tables[clsPOSDBConstants.MessageLog_tbl].Rows.Count != 0)
                {
                    throw new Exception("Primary key violation for Customer ");
                }
            }
        }

      
        public void checkIsValidDelete(MessageLogData updates)
        {
        }
        #endregion
       
        public void Dispose()
        {
            GC.SuppressFinalize(true);
        }

    }
}
