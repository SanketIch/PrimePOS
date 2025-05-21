using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MMSUtil;
using Microsoft.Win32.TaskScheduler;
using System.IO;
using System.Windows.Forms;
using POS_Core.BusinessRules;
using POS_Core.CommonData.Rows;
using POS_Core.CommonData;

namespace POS_Core_UI
{
    public static class ManageScheduledTask
    {
        public static bool addModifyScheuled(ScheduledTasksRow oScheduledTasksRow, Object oTaskBaseControl)
        {
            bool bReturn = true; //PRIMEPOS-3039 16-Dec-2021 JY Added
            try
            {
                IScheduledControl oPerformTaskBaseControl = (IScheduledControl)oTaskBaseControl;

                TaskService ts = new TaskService();
                TaskFolder tf = ts.RootFolder;

                try
                {
                    if (oScheduledTasksRow.TaskNameOld != oScheduledTasksRow.TaskName)
                    {
                        if (oScheduledTasksRow.TaskNameOld != "")
                            tf.DeleteTask(oScheduledTasksRow.TaskNameOld);
                    }
                    if (oScheduledTasksRow.TaskName != "")
                        tf.DeleteTask(oScheduledTasksRow.TaskName);
                }
                catch (Exception exp)
                {
                }

                TaskDefinition td = ts.NewTask();
                Trigger tg = null;

                if (oPerformTaskBaseControl == null)
                {
                    tg = new Microsoft.Win32.TaskScheduler.TimeTrigger();
                }
                else if (oPerformTaskBaseControl.GetType().Name == "usrDaily")
                    tg = new DailyTrigger((short)oScheduledTasksRow.RepeatTask);

                else if (oPerformTaskBaseControl.GetType().Name == "usrWeekly")
                {
                    WeeklyTrigger wt = new WeeklyTrigger();
                    usrWeeklyData ousrWeeklyData = new usrWeeklyData();
                    ousrWeeklyData = (usrWeeklyData)oPerformTaskBaseControl.getObject();
                    if (ousrWeeklyData != null && ousrWeeklyData.Tables.Count > 0 && ousrWeeklyData.ScheduledTasksDetailWeek.Rows.Count > 0)
                    {
                        usrWeeklyRow ousrWeeklyRow = ousrWeeklyData.ScheduledTasksDetailWeek[0];
                        wt.WeeksInterval = (short)ousrWeeklyRow.Days;
                        int[] iDays = ousrWeeklyRow.SelectedDays.Split(',').Select(x => int.Parse(x)).ToArray();

                        switch (iDays[0])
                        {
                            case 0:
                                wt.DaysOfWeek = DaysOfTheWeek.Sunday;
                                break;
                            case 1:
                                wt.DaysOfWeek = DaysOfTheWeek.Monday;
                                break;
                            case 2:
                                wt.DaysOfWeek = DaysOfTheWeek.Tuesday;
                                break;
                            case 3:
                                wt.DaysOfWeek = DaysOfTheWeek.Wednesday;
                                break;
                            case 4:
                                wt.DaysOfWeek = DaysOfTheWeek.Thursday;
                                break;
                            case 5:
                                wt.DaysOfWeek = DaysOfTheWeek.Friday;
                                break;
                            case 6:
                                wt.DaysOfWeek = DaysOfTheWeek.Saturday;
                                break;
                        }

                        for (int count = 1; count < (iDays.Length); count++)
                        {
                            switch (iDays[count])
                            {
                                case 0:
                                    wt.DaysOfWeek = wt.DaysOfWeek | DaysOfTheWeek.Sunday;
                                    break;
                                case 1:
                                    wt.DaysOfWeek = wt.DaysOfWeek | DaysOfTheWeek.Monday;
                                    break;
                                case 2:
                                    wt.DaysOfWeek = wt.DaysOfWeek | DaysOfTheWeek.Tuesday;
                                    break;
                                case 3:
                                    wt.DaysOfWeek = wt.DaysOfWeek | DaysOfTheWeek.Wednesday;
                                    break;
                                case 4:
                                    wt.DaysOfWeek = wt.DaysOfWeek | DaysOfTheWeek.Thursday;
                                    break;
                                case 5:
                                    wt.DaysOfWeek = wt.DaysOfWeek | DaysOfTheWeek.Friday;
                                    break;
                                case 6:
                                    wt.DaysOfWeek = wt.DaysOfWeek | DaysOfTheWeek.Saturday;
                                    break;
                            }
                        }
                        tg = wt;
                    }
                }
                else if (oPerformTaskBaseControl.GetType().Name == "usrMonthly")
                {
                    MonthlyTrigger mt = new MonthlyTrigger();
                    usrMonthlyData ousrMonthlyData = new usrMonthlyData();
                    ousrMonthlyData = (usrMonthlyData)oPerformTaskBaseControl.getObject();
                    if (ousrMonthlyData != null && ousrMonthlyData.Tables.Count > 0 && ousrMonthlyData.ScheduledTasksDetailMonth.Rows.Count > 0)
                    {
                        usrMonthlyRow ousrMonthlyRow = ousrMonthlyData.ScheduledTasksDetailMonth[0];
                        int[] iDays = ousrMonthlyRow.SelectionDays.Split(',').Select(x => int.Parse(x)).ToArray(); ;
                        string[] sMonth = ousrMonthlyRow.SelectionMonths.Split(',');
                        mt.DaysOfMonth = iDays;

                        switch (sMonth[0].ToString())
                        {
                            case "1":
                                mt.MonthsOfYear = MonthsOfTheYear.January;
                                break;
                            case "2":
                                mt.MonthsOfYear = MonthsOfTheYear.February;
                                break;
                            case "3":
                                mt.MonthsOfYear = MonthsOfTheYear.March;
                                break;
                            case "4":
                                mt.MonthsOfYear = MonthsOfTheYear.April;
                                break;
                            case "5":
                                mt.MonthsOfYear = MonthsOfTheYear.May;
                                break;
                            case "6":
                                mt.MonthsOfYear = MonthsOfTheYear.June;
                                break;
                            case "7":
                                mt.MonthsOfYear = MonthsOfTheYear.July;
                                break;
                            case "8":
                                mt.MonthsOfYear = MonthsOfTheYear.August;
                                break;
                            case "9":
                                mt.MonthsOfYear = MonthsOfTheYear.September;
                                break;
                            case "10":
                                mt.MonthsOfYear = MonthsOfTheYear.October;
                                break;
                            case "11":
                                mt.MonthsOfYear = MonthsOfTheYear.November;
                                break;
                            case "12":
                                mt.MonthsOfYear = MonthsOfTheYear.December;
                                break;
                        }

                        for (int count = 1; count < (sMonth.Length); count++)
                        {
                            switch (sMonth[count].ToString())
                            {
                                case "1":
                                    mt.MonthsOfYear = mt.MonthsOfYear | MonthsOfTheYear.January;
                                    break;
                                case "2":
                                    mt.MonthsOfYear = mt.MonthsOfYear | MonthsOfTheYear.February;
                                    break;
                                case "3":
                                    mt.MonthsOfYear = mt.MonthsOfYear | MonthsOfTheYear.March;
                                    break;
                                case "4":
                                    mt.MonthsOfYear = mt.MonthsOfYear | MonthsOfTheYear.April;
                                    break;
                                case "5":
                                    mt.MonthsOfYear = mt.MonthsOfYear | MonthsOfTheYear.May;
                                    break;
                                case "6":
                                    mt.MonthsOfYear = mt.MonthsOfYear | MonthsOfTheYear.June;
                                    break;
                                case "7":
                                    mt.MonthsOfYear = mt.MonthsOfYear | MonthsOfTheYear.July;
                                    break;
                                case "8":
                                    mt.MonthsOfYear = mt.MonthsOfYear | MonthsOfTheYear.August;
                                    break;
                                case "9":
                                    mt.MonthsOfYear = mt.MonthsOfYear | MonthsOfTheYear.September;
                                    break;
                                case "10":
                                    mt.MonthsOfYear = mt.MonthsOfYear | MonthsOfTheYear.October;
                                    break;
                                case "11":
                                    mt.MonthsOfYear = mt.MonthsOfYear | MonthsOfTheYear.November;
                                    break;
                                case "12":
                                    mt.MonthsOfYear = mt.MonthsOfYear | MonthsOfTheYear.December;
                                    break;
                            }
                        }
                        tg = mt;
                    }
                }

                tg.StartBoundary = Convert.ToDateTime(Convert.ToDateTime(oScheduledTasksRow.StartDate).ToShortDateString().ToString() + " " + Convert.ToDateTime(oScheduledTasksRow.StartTime).ToShortTimeString().ToString());
                if (oScheduledTasksRow.AdvancedSeetings == true)
                {
                    tg.Repetition.Interval = TimeSpan.FromTicks(oScheduledTasksRow.RepeatTaskInterval);
                    tg.Repetition.Duration = TimeSpan.FromTicks(oScheduledTasksRow.Duration);
                    tg.Repetition.StopAtDurationEnd = true;
                }
                tg.Enabled = oScheduledTasksRow.Enabled;
                td.Triggers.Add(tg);
                td.RegistrationInfo.Author = "PrimePOS";
                string sFilePath = Application.ExecutablePath.ToString();
                MappedDriveResolver mp = new MappedDriveResolver();
                if (mp.isNetworkDrive(sFilePath) == true)
                    sFilePath = mp.ResolveToUNC(sFilePath);
                else
                    sFilePath = mp.ResolveToUNC(sFilePath);

                td.Actions.Add(new ExecAction(sFilePath, "01 ScheduledTaskExecute " + oScheduledTasksRow.ScheduledTasksID.ToString() + "  " + oScheduledTasksRow.TaskId.ToString(), null));
                td.RegistrationInfo.Description = oScheduledTasksRow.TaskDescription;

                td.Principal.RunLevel = TaskRunLevel.Highest;
                tf.RegisterTaskDefinition(oScheduledTasksRow.TaskName, td);
            }
            catch (Exception exp)
            {
                bReturn = false;
                if (exp.HResult == -2147024891 || exp.Message.Trim().ToUpper() == "Access is denied. (Exception from HRESULT: 0x80070005 (E_ACCESSDENIED))".ToUpper())
                    clsUIHelper.ShowErrorMsg("Please make sure to run exe as an administrator to perform any action related to the scheduler task.");
                    //throw new ApplicationException("Please make sure to run exe as an administrator to perform any action related to the scheduler task.", exp);
                else
                    throw (exp);
            }
            return bReturn;
        }

        public static bool DeleteTask(string sTaskName)
        {
            try
            {
                TaskService ts = new TaskService();
                TaskFolder tf = ts.RootFolder;
                Task tk = ts.FindTask(sTaskName, false);
                tf.DeleteTask(tk.Name);
                return true;
            }
            catch { return false; }
        }
        public static bool RunTask(string sTaskName)
        {
            try
            {
                TaskService ts = new TaskService();
                Task tk = ts.FindTask(sTaskName, true);
                tk.Run();
                return true;
            }
            catch { return false; }
        }

        public static bool DeleteAllTask()
        {
            try
            {
                TaskService ts = new TaskService();
                TaskFolder tf = ts.RootFolder;

                tf.GetTasks();
                foreach (Task stask in tf.GetTasks())
                {
                    if (stask.Definition.RegistrationInfo.Author == "Prime Rx")
                    {
                        tf.DeleteTask(stask.Name);
                    }
                }
                return true;
            }
            catch { return false; };
        }
    }
}
