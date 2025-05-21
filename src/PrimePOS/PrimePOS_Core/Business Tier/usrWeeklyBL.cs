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
    public class usrWeeklyBL : IDisposable
    {
        private static ILogger logger = LogManager.GetCurrentClassLogger();

        public usrWeeklyBL()
        {
        }       

        #region Persist Methods
        public void Persist(usrWeeklyData updates)
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
                logger.Fatal(ex, "Persist(usrWeeklyData updates)");  //PRIMEPOS-2971 07-Jun-2021 JY Added
                //ErrorHandler.throwException(ex, "", "");
            }
        }

        public void Persist(usrWeeklyData updates, IDbTransaction tx)
        {
            try
            {
                //checkIsValidData(updates);
                using (usrWeeklySvr dao = new usrWeeklySvr())
                {
                    dao.Persist(updates, tx);
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
                logger.Fatal(ex, "Persist(usrWeeklyData updates, IDbTransaction tx)");  //PRIMEPOS-2971 07-Jun-2021 JY Added
                //ErrorHandler.throwException(ex, "", "");
            }
        }
        #endregion

        #region Validation Methods
        public void checkIsValidData(usrWeeklyData updates)
        {
            usrWeeklyTable table;            
            usrWeeklyRow oRow;            

            oRow = (usrWeeklyRow)updates.Tables[0].Rows[0];

            table = (usrWeeklyTable)updates.Tables[0].GetChanges(DataRowState.Added);

            if (table == null)
                table = (usrWeeklyTable)updates.Tables[0].GetChanges(DataRowState.Modified);
            else if ((usrWeeklyTable)updates.Tables[0].GetChanges(DataRowState.Modified) != null)
                table.MergeTable((usrWeeklyTable)updates.Tables[0].GetChanges(DataRowState.Modified));

            if (table == null)
                table = (usrWeeklyTable)updates.Tables[0].GetChanges(DataRowState.Unchanged);
            else if ((usrWeeklyTable)updates.Tables[0].GetChanges(DataRowState.Unchanged) != null)
                table.MergeTable((usrWeeklyTable)updates.Tables[0].GetChanges(DataRowState.Unchanged));

            if (table == null) return;
        }
        #endregion

        public void Insert(usrWeeklyData ousrWeeklyData)
        {
            try
            {
                using (usrWeeklySvr ousrWeeklySvr = new usrWeeklySvr())
                {
                    ousrWeeklySvr.Insert(ousrWeeklyData);
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
                logger.Fatal(ex, "Insert(usrWeeklyData ousrWeeklyData)");  //PRIMEPOS-2971 07-Jun-2021 JY Added
                //ErrorHandler.throwException(ex, "", "");
            }
        }

        public void Delete(System.Int32 ScheduledTasksID)
        {
            try
            {
                using (usrWeeklySvr ousrWeeklySvr = new usrWeeklySvr())
                {
                    ousrWeeklySvr.Delete(ScheduledTasksID);
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

        public usrWeeklyData GetScheduledTasksDetailWeek(System.Int32 ScheduledTasksID)
        {
            try
            {
                using (usrWeeklySvr ousrWeeklySvr = new usrWeeklySvr())
                {
                    return ousrWeeklySvr.GetScheduledTasksDetailWeek(ScheduledTasksID);
                }
            }
            catch (POSExceptions ex)
            {
                logger.Fatal(ex, "GetScheduledTasksDetailWeek(System.Int32 ScheduledTasksID)");
                throw (ex);
            }
            catch (OtherExceptions ex)
            {
                logger.Fatal(ex, "GetScheduledTasksDetailWeek(System.Int32 ScheduledTasksID)");
                throw (ex);
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "GetScheduledTasksDetailWeek(System.Int32 ScheduledTasksID)");
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
