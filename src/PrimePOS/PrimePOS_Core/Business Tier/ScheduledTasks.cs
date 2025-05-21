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
    public class ScheduledTasks : IDisposable
    {
        private static ILogger logger = LogManager.GetCurrentClassLogger();
        public static DataTable dtTaskList = null;

        #region Persist Methods
        public System.Int32 Persist(ScheduledTasksData updates)
        {
            System.Int32 ScheduledTasksID = 0;
            try
            {
                using (IDbConnection oConn = DataFactory.CreateConnection(POS_Core.Resources.Configuration.ConnectionString))
                {
                    IDbTransaction tx = oConn.BeginTransaction();
                    ScheduledTasksID = Persist(updates, tx);
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
                logger.Fatal(ex, "Persist(ScheduledTasksData updates)");  //PRIMEPOS-2971 07-Jun-2021 JY Added
                //ErrorHandler.throwException(ex, "", "");
            }
            return ScheduledTasksID;
        }

        public System.Int32 Persist(ScheduledTasksData updates, IDbTransaction tx)
        {
            System.Int32 ScheduledTasksID = 0;
            try
            {
                checkIsValidData(updates);
                using (ScheduledTasksSvr dao = new ScheduledTasksSvr())
                {
                    ScheduledTasksID = dao.Persist(updates, tx);
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
                logger.Fatal(ex, "Persist(ScheduledTasksData updates, IDbTransaction tx)");  //PRIMEPOS-2971 07-Jun-2021 JY Added
                //ErrorHandler.throwException(ex, "", "");
            }
            return ScheduledTasksID;
        }
        #endregion

        #region Validation Methods
        public void checkIsValidData(ScheduledTasksData updates)
        {
            ScheduledTasksTable table;
            ScheduledTasksRow oRow;

            oRow = (ScheduledTasksRow)updates.Tables[0].Rows[0];

            table = (ScheduledTasksTable)updates.Tables[0].GetChanges(DataRowState.Added);

            if (table == null)
                table = (ScheduledTasksTable)updates.Tables[0].GetChanges(DataRowState.Modified);
            else if ((ScheduledTasksTable)updates.Tables[0].GetChanges(DataRowState.Modified) != null)
                table.MergeTable((ScheduledTasksTable)updates.Tables[0].GetChanges(DataRowState.Modified));

            if (table == null)
                table = (ScheduledTasksTable)updates.Tables[0].GetChanges(DataRowState.Unchanged);
            else if ((ScheduledTasksTable)updates.Tables[0].GetChanges(DataRowState.Unchanged) != null)
                table.MergeTable((ScheduledTasksTable)updates.Tables[0].GetChanges(DataRowState.Unchanged));

            if (table == null) return;
        }
        #endregion

        public virtual ScheduledTasksData GetScheduledTaskByScheduledTasksID(System.Int32 ScheduledTasksID)
        {
            using (ScheduledTasksSvr oScheduledTasksSvr = new ScheduledTasksSvr())
            {
                return oScheduledTasksSvr.GetScheduledTaskByScheduledTasksID(ScheduledTasksID);
            }
        }

        public ScheduledTasksData GetScheduledTasksList(int TaskId)
        {
            try
            {
                using (ScheduledTasksSvr oScheduledTasksSvr = new ScheduledTasksSvr())
                {
                    return oScheduledTasksSvr.GetScheduledTasksList(TaskId);
                }
            }
            catch (POSExceptions ex)
            {
                logger.Fatal(ex, "GetScheduledTasksList(int TaskId)");
                throw (ex);
            }
            catch (OtherExceptions ex)
            {
                logger.Fatal(ex, "GetScheduledTasksList(int TaskId)");
                throw (ex);
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "GetScheduledTasksList(int TaskId)");
                //ErrorHandler.throwException(ex, "", "");
                return null;
            }
        }

        public virtual DataTable GetScheduledTasksControlsList(System.Int32 ScheduledTasksID)
        {
            using (ScheduledTasksSvr oScheduledTasksSvr = new ScheduledTasksSvr())
            {
                return oScheduledTasksSvr.GetScheduledTasksControlsList(ScheduledTasksID);
            }
        }

        public virtual bool SaveTaskParameters(DataTable dt, int ScheduledTasksID)
        {
            using (ScheduledTasksSvr oScheduledTasksSvr = new ScheduledTasksSvr())
            {
                return oScheduledTasksSvr.SaveTaskParameters(dt, ScheduledTasksID);
            }
        }

        public string GetTask(int taskIndex)
        {
            string taskname = string.Empty;
            if (dtTaskList == null)
                AddTaskList();
            foreach (DataRow dr in dtTaskList.Select("ID = " + taskIndex))
            {
                taskname = dr["Task"].ToString();
            }
            return taskname;
        }

        public static string GetTaskName(int taskIndex)
        {
            string taskname = string.Empty;
            if (dtTaskList == null)
                AddTaskList();
            foreach (DataRow dr in dtTaskList.Select("ID=" + taskIndex + ""))
            {
                taskname = dr["TaskName"].ToString();
            }
            return taskname;
        }

        public static int GetTaskIndex(string taskName)
        {
            int TaskID = -1;
            if (dtTaskList == null)
                AddTaskList();
            foreach (DataRow dr in dtTaskList.Select("Task='" + taskName + "'"))
            {
                TaskID = Convert.ToInt32(dr["ID"].ToString());
            }
            return TaskID;
        }

        public static void AddTaskList()
        {
            dtTaskList = new DataTable("TaskList");

            dtTaskList.Columns.Add("ID", System.Type.GetType("System.Int32"));
            dtTaskList.Columns.Add("TaskName", System.Type.GetType("System.String"));
            dtTaskList.Columns.Add("Task", System.Type.GetType("System.String"));

            DataRow dr = dtTaskList.NewRow();
            dr["ID"] = "0";
            dr["TaskName"] = "Sales Tax Summary Report";
            dr["Task"] = "POS_Core_UI.Reports.ReportsUI.frmRptSalesTax";
            dtTaskList.Rows.Add(dr);

            dr = dtTaskList.NewRow();
            dr["ID"] = "1";
            dr["TaskName"] = "Sales Summary by User";
            dr["Task"] = "POS_Core_UI.Reports.ReportsUI.frmRptSSalesByUID";
            dtTaskList.Rows.Add(dr);

            dr = dtTaskList.NewRow();
            dr["ID"] = "2";
            dr["TaskName"] = "Sales Report by Item";
            dr["Task"] = "POS_Core_UI.Reports.ReportsUI.frmRptSSalesByItem";
            dtTaskList.Rows.Add(dr);

            dr = dtTaskList.NewRow();
            dr["ID"] = "3";
            dr["TaskName"] = "Sales Report by Department";
            dr["Task"] = "POS_Core_UI.Reports.ReportsUI.frmRptSalesByDepartment";
            dtTaskList.Rows.Add(dr);

            dr = dtTaskList.NewRow();
            dr["ID"] = "4";
            dr["TaskName"] = "Sales Report by Payment";
            dr["Task"] = "POS_Core_UI.Reports.ReportsUI.frmRptSaleByPayType";
            dtTaskList.Rows.Add(dr);

            dr = dtTaskList.NewRow();
            dr["ID"] = "5";
            dr["TaskName"] = "Sales Report by Customer";
            dr["Task"] = "POS_Core_UI.Reports.ReportsUI.frmRptSalesByCustomer";
            dtTaskList.Rows.Add(dr);

            dr = dtTaskList.NewRow();
            dr["ID"] = "6";
            dr["TaskName"] = "Top Selling Products";
            dr["Task"] = "POS_Core_UI.Reports.ReportsUI.frmRptTopSellingProducts";
            dtTaskList.Rows.Add(dr);

            dr = dtTaskList.NewRow();
            dr["ID"] = "7";
            dr["TaskName"] = "Cost Analysis Report";
            dr["Task"] = "POS_Core_UI.Reports.ReportsUI.frmRptCostAnalysis";
            dtTaskList.Rows.Add(dr);

            //PRIMEPOS-3042 22-Dec-2021 JY Added task to Process Close Station
            dr = dtTaskList.NewRow();
            dr["ID"] = "8";
            dr["TaskName"] = "Process Close Station";
            dr["Task"] = "POS_Core_UI.frmStationClose";
            dtTaskList.Rows.Add(dr);

            //PRIMEPOS-3039 16-Dec-2021 JY Added task for Process EOD
            dr = dtTaskList.NewRow();
            dr["ID"] = "9";
            dr["TaskName"] = "Process End of Day";
            dr["Task"] = "POS_Core_UI.frmEndOfDay";
            dtTaskList.Rows.Add(dr);

            //PRIMEPOS-3042 22-Dec-2021 JY Added task to Process Close Station and EOD together
            dr = dtTaskList.NewRow();
            dr["ID"] = "10";
            dr["TaskName"] = "Process Close Station And EOD";
            dr["Task"] = "POS_Core_UI.clsProcessCloseStationAndEOD";
            dtTaskList.Rows.Add(dr);

            dtTaskList.AcceptChanges();
        }

        public bool Delete(Int32 ScheduledTasksID)
        {
            try
            {
                logger.Trace("Delete() - " + clsPOSDBConstants.Log_Entering);
                bool retValue;
                using (ScheduledTasksSvr oScheduledTasksSvr = new ScheduledTasksSvr())
                {
                    retValue = oScheduledTasksSvr.Delete(ScheduledTasksID);
                }
                logger.Trace("Delete() - " + clsPOSDBConstants.Log_Exiting);
                return retValue;
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "Delete()");
                throw (ex);
            }
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
