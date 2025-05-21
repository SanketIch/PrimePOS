using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using NLog;
using POS_Core.DataAccess;
using POS_Core.ErrorLogging;
using POS_Core.CommonData;
using Resources;
using POS_Core.CommonData.Tables;
using POS_Core.CommonData.Rows;

namespace POS_Core.BusinessRules
{
    public class usrMonthlyBL : IDisposable
    {
        private static ILogger logger = LogManager.GetCurrentClassLogger();

        public usrMonthlyBL()
        {
        }

        #region Persist Methods
        public void Persist(usrMonthlyData updates)
        {
            try
            {
                using (IDbConnection oConn = DataFactory.CreateConnection(POS_Core.Resources.Configuration.ConnectionString))
                {
                    IDbTransaction tx = oConn.BeginTransaction();
                    Persist(updates, tx);
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
                logger.Fatal(ex, "Persist(usrMonthlyData updates)");  //PRIMEPOS-2971 07-Jun-2021 JY Added
                //ErrorHandler.throwException(ex, "", "");
            }
        }

        public void Persist(usrMonthlyData updates, IDbTransaction tx)
        {
            try
            {
                //checkIsValidData(updates);
                using (usrMonthlySvr ousrMonthlySvr = new usrMonthlySvr())
                {
                    ousrMonthlySvr.Persist(updates, tx);
                    tx.Commit();
                }
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
                logger.Fatal(ex, "Persist(usrMonthlyData updates, IDbTransaction tx)");  //PRIMEPOS-2971 07-Jun-2021 JY Added
                //ErrorHandler.throwException(ex, "", "");
            }
        }
        #endregion

        #region Validation Methods
        public void checkIsValidData(usrMonthlyData updates)
        {
            usrMonthlyTable table;
            usrMonthlyRow oRow;
            
            oRow = (usrMonthlyRow)updates.Tables[0].Rows[0];

            table = (usrMonthlyTable)updates.Tables[0].GetChanges(DataRowState.Added);

            if (table == null)
                table = (usrMonthlyTable)updates.Tables[0].GetChanges(DataRowState.Modified);
            else if ((usrMonthlyTable)updates.Tables[0].GetChanges(DataRowState.Modified) != null)
                table.MergeTable((usrMonthlyTable)updates.Tables[0].GetChanges(DataRowState.Modified));

            if (table == null)
                table = (usrMonthlyTable)updates.Tables[0].GetChanges(DataRowState.Unchanged);
            else if ((usrMonthlyTable)updates.Tables[0].GetChanges(DataRowState.Unchanged) != null)
                table.MergeTable((usrMonthlyTable)updates.Tables[0].GetChanges(DataRowState.Unchanged));

            if (table == null) return;
        }
        #endregion

        public void Insert(usrMonthlyData ousrMonthlyData)
        {
            try
            {
                using (usrMonthlySvr ousrMonthlySvr = new usrMonthlySvr())
                {
                    ousrMonthlySvr.Insert(ousrMonthlyData);
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
                logger.Fatal(ex, "Insert(usrMonthlyData ousrMonthlyData)");  //PRIMEPOS-2971 07-Jun-2021 JY Added
                //ErrorHandler.throwException(ex, "", "");
            }
        }

        public void Delete(System.Int32 ScheduledTasksID)
        {
            try
            {
                using (usrMonthlySvr ousrMonthlySvr = new usrMonthlySvr())
                {
                    ousrMonthlySvr.Delete(ScheduledTasksID);
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

        public usrMonthlyData GetScheduledTasksDetailMonth(System.Int32 ScheduledTasksID)
        {
            try
            {
                using (usrMonthlySvr ousrMonthlySvr = new usrMonthlySvr())
                {
                    return ousrMonthlySvr.GetScheduledTasksDetailMonth(ScheduledTasksID);
                }
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

        public void Dispose()
        {
            GC.SuppressFinalize(true);
        }
    }
}
