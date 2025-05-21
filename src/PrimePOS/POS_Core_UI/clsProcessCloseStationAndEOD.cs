using POS_Core.BusinessRules;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace POS_Core_UI
{
    /// <summary>
    /// PRIMEPOS-3042 22-Dec-2021 JY Added to process close station and EOD through scheduler task
    /// </summary>
    public class clsProcessCloseStationAndEOD : ICommandLIneTaskControl
    {
        public usrDateRangeParams customControl;
        private bool bCalledFromScheduler = false;
        private string strMessage = string.Empty;

        public clsProcessCloseStationAndEOD()
        {
            this.customControl = new usrDateRangeParams();
        }

        public bool CheckTags()
        {
            return true;
        }

        public bool SaveTaskParameters(DataTable dt, int ScheduledTasksID)
        {
            try
            {
                ScheduledTasks oScheduledTasks = new ScheduledTasks();
                oScheduledTasks.SaveTaskParameters(dt, ScheduledTasksID);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool SetControlParameters(int ScheduledTasksID)
        {
            ScheduledTasks oScheduledTasks = new ScheduledTasks();
            DataTable dt = oScheduledTasks.GetScheduledTasksControlsList(ScheduledTasksID);
            customControl.setControlsValues(ref dt);
            return true;
        }

        public bool RunTask(int TaskId, ref string filePath, bool bsendToPrint, ref string sNoOfRecordAffect)
        {
            try
            {
                SetControlParameters(TaskId);
                bCalledFromScheduler = true;
                bsendToPrint = false;
                frmStationClose ofrmStationClose = new frmStationClose();
                bool bStatus = ofrmStationClose.CloseAllStaitons(ref strMessage, ref filePath);
                sNoOfRecordAffect = strMessage;
            }
            catch (Exception Ex)
            { }
            finally
            {
                bCalledFromScheduler = false;
                strMessage = string.Empty;
            }
            return true;
        }

        public void GetTaskParameters(ref DataTable dt, int TaskId)
        {
            customControl.getControlsValues(ref dt);
        }

        public Control GetParameterControl()
        {
            customControl.setDateTimeControl();
            customControl.Dock = DockStyle.Fill;
            return customControl;
        }

        public bool checkValidation()
        {
            return true;
        }
    }
}
