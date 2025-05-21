using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using POS_Core.DataAccess;
using POS_Core.CommonData.Tables;
using POS_Core.CommonData.Rows;
using POS_Core.CommonData;
using POS_Core.ErrorLogging;
using Resources;
using POS_Core.Resources;
using System.Data;
using NLog;

namespace POS_Core.BusinessRules
{
    public class ScheduledTasksLog : IDisposable
    {
        private static ILogger logger = LogManager.GetCurrentClassLogger();

        #region Persist Methods
        public System.Int32 Persist(ScheduledTasksLogData updates)
        {
            System.Int32 ScheduledTasksLogID = 0;
            try
            {
                using (IDbConnection oConn = DataFactory.CreateConnection(POS_Core.Resources.Configuration.ConnectionString))
                {
                    IDbTransaction tx = oConn.BeginTransaction();
                    ScheduledTasksLogID = Persist(updates, tx);
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
                logger.Fatal(ex, "Persist(ScheduledTasksLogData updates)");  //PRIMEPOS-2971 07-Jun-2021 JY Added
                //ErrorHandler.throwException(ex, "", "");
            }
            return ScheduledTasksLogID;
        }

        public System.Int32 Persist(ScheduledTasksLogData updates, IDbTransaction tx)
        {
            System.Int32 ScheduledTasksLogID = 0;
            try
            {
                using (ScheduledTasksLogSvr oScheduledTasksLogSvr = new ScheduledTasksLogSvr())
                {
                    ScheduledTasksLogID = oScheduledTasksLogSvr.Persist(updates, tx);
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
                logger.Fatal(ex, "Persist(ScheduledTasksLogData updates, IDbTransaction tx)");  //PRIMEPOS-2971 07-Jun-2021 JY Added
                //ErrorHandler.throwException(ex, "", "");
            }
            return ScheduledTasksLogID;
        }
        #endregion

        public DataSet GetScheduledTasksLogList(int TaskId, string sFromDate, string sToDate)
        {
            using (ScheduledTasksLogSvr oScheduledTasksLogSvr = new ScheduledTasksLogSvr())
            {
                return oScheduledTasksLogSvr.GetScheduledTasksLogList(TaskId, sFromDate, sToDate);
            }
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
