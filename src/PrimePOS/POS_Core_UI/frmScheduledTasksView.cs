using POS_Core.BusinessRules;
using POS_Core.CommonData;
using POS_Core.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.IO;
using POS_Core.DataAccess;
using POS_Core.CommonData.Rows;
using POS_Core.ErrorLogging;
using POS_Core_UI.Reports.ReportsUI;
using NLog;
using System.Globalization;
using Infragistics.Win.UltraWinGrid;
using CrystalDecisions.CrystalReports.Engine;

namespace POS_Core_UI
{
    public partial class frmScheduledTasksView : Form
    {
        private ILogger logger = LogManager.GetCurrentClassLogger();
        private ScheduledTasks oScheduledTasks = new ScheduledTasks();

        public frmScheduledTasksView()
        {
            InitializeComponent();
        }

        private void frmScheduledTasksView_Load(object sender, EventArgs e)
        {
            try
            {
                #region Events attachment
                this.cbSelectedTask.AfterEnterEditMode += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
                this.cbSelectedTask.AfterExitEditMode += new System.EventHandler(clsUIHelper.AfterExitEditMode);
                this.cmbTaskList.AfterEnterEditMode += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
                this.cmbTaskList.AfterExitEditMode += new System.EventHandler(clsUIHelper.AfterExitEditMode);

                this.dtFromDate.Enter += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
                this.dtFromDate.Leave += new System.EventHandler(clsUIHelper.AfterExitEditMode);
                this.dtToDate.Enter += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
                this.dtToDate.Leave += new System.EventHandler(clsUIHelper.AfterExitEditMode);
                #endregion

                pnlClose.Left = pnlButtons.Width - pnlClose.Width - 10;
                pnlDelete.Left = pnlClose.Left - pnlDelete.Width - 10;
                pnlEdit.Left = pnlDelete.Left - pnlEdit.Width - 10;
                pnlAdd.Left = pnlEdit.Left - pnlAdd.Width - 10;
                pnlSyncWithWindowScheduler.Left = pnlAdd.Left - pnlSyncWithWindowScheduler.Width - 10;

                dtFromDate.Value = Convert.ToDateTime(DateTime.Today.AddDays(-15));
                dtToDate.Value = DateTime.Today;
                FillTaskCombos();
                cbSelectedTask.SelectedIndex = 0;
                cmbTaskList.SelectedIndex = 0;

                LoadTasks();
                FillHistoryGrid();
            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }

        private void LoadTasks()
        {
            int iTask = -1;

            if (cbSelectedTask.SelectedIndex != 0)
                iTask = ScheduledTasks.GetTaskIndex(cbSelectedTask.SelectedItem.Tag.ToString());
            grdSearch.ClearXsdConstraints();

            ScheduledTasksData oScheduledTasksData;
            oScheduledTasksData = oScheduledTasks.GetScheduledTasksList(iTask);
            this.grdSearch.DataSource = null;
            this.grdSearch.DataSource = oScheduledTasksData;
            this.grdSearch.Refresh();

            int CountRow = 0;
            int TaskID = -1;
            for (CountRow = 0; CountRow < grdSearch.Rows.Count; CountRow++)
            {
                TaskID = Configuration.convertNullToInt(grdSearch.Rows[CountRow].Cells["TaskId"].Value);
                grdSearch.Rows[CountRow].Cells["colTask"].Value = ScheduledTasks.GetTaskName(TaskID);
            }

            ApplyGridFormat();
        }

        private void ApplyGridFormat()
        {
            CultureInfo cultureENUS = CultureInfo.CreateSpecificCulture("en-us");
            grdSearch.DisplayLayout.Bands[0].Columns["StartTime"].Format = "t";
            grdSearch.DisplayLayout.Bands[0].Columns["StartTime"].FormatInfo = cultureENUS;
            grdSearch.DisplayLayout.Bands[0].Columns["StartTime"].CellActivation = Activation.NoEdit;

            grdSearch.DisplayLayout.Bands[0].Columns["TaskName"].Width = 120;
            grdSearch.DisplayLayout.Bands[0].Columns["colTask"].Width = 120;
            grdSearch.DisplayLayout.Bands[0].Columns["StartTime"].Width = 70;
            grdSearch.DisplayLayout.Bands[0].Columns["PerformTaskText"].Width = 85;
            grdSearch.DisplayLayout.Bands[0].Columns["RepeatTask"].Width = 80;
            grdSearch.DisplayLayout.Bands[0].Columns["SendEmailText"].Width = 35;
            grdSearch.DisplayLayout.Bands[0].Columns["SendPrintText"].Width = 35;
            grdSearch.DisplayLayout.Bands[0].Columns["EmailAddress"].Width = 100;
            grdSearch.DisplayLayout.Bands[0].Columns["TaskDescription"].Width = 180;
            grdSearch.DisplayLayout.Bands[0].Columns["EnabledText"].Width = 70;

            grdSearch.DisplayLayout.Bands[0].Columns["colTask"].Header.Caption = "Task";
            grdSearch.DisplayLayout.Bands[0].Columns["SendEmail"].Header.Caption = "Email";
            grdSearch.DisplayLayout.Bands[0].Columns["PerformTaskText"].Header.Caption = "PerformTask";
            grdSearch.DisplayLayout.Bands[0].Columns["SendEmailText"].Header.Caption = "SendEmail";
            grdSearch.DisplayLayout.Bands[0].Columns["SendPrintText"].Header.Caption = "SendPrint";
            grdSearch.DisplayLayout.Bands[0].Columns["EnabledText"].Header.Caption = "Enabled";

            grdSearch.DisplayLayout.Bands[0].Columns["ScheduledTasksID"].Hidden = true;
            grdSearch.DisplayLayout.Bands[0].Columns["SendPrint"].Hidden = true;
            grdSearch.DisplayLayout.Bands[0].Columns["PerformTask"].Hidden = true;
            grdSearch.DisplayLayout.Bands[0].Columns["RepeatTask"].Hidden = true;
            grdSearch.DisplayLayout.Bands[0].Columns["SendEmail"].Hidden = true;
            grdSearch.DisplayLayout.Bands[0].Columns["SendPrint"].Hidden = true;
            grdSearch.DisplayLayout.Bands[0].Columns["Enabled"].Hidden = true;
            grdSearch.DisplayLayout.Bands[0].Columns["AdvancedSeetings"].Hidden = true;
            grdSearch.DisplayLayout.Bands[0].Columns["Duration"].Hidden = true;
            grdSearch.DisplayLayout.Bands[0].Columns["RepeatTaskInterval"].Hidden = true;
            grdSearch.DisplayLayout.Bands[0].Columns["TaskId"].Hidden = true;
            grdSearch.DisplayLayout.Bands[0].Columns["TaskNameOld"].Hidden = true;

            grdSearch.DisplayLayout.Bands[0].Columns["TaskName"].CellActivation = Activation.NoEdit;
            grdSearch.DisplayLayout.Bands[0].Columns["StartTime"].CellActivation = Activation.NoEdit;
            grdSearch.DisplayLayout.Bands[0].Columns["StartDate"].CellActivation = Activation.NoEdit;
            grdSearch.DisplayLayout.Bands[0].Columns["PerformTaskText"].CellActivation = Activation.NoEdit;
            grdSearch.DisplayLayout.Bands[0].Columns["RepeatTask"].CellActivation = Activation.NoEdit;
            grdSearch.DisplayLayout.Bands[0].Columns["SendEmailText"].CellActivation = Activation.NoEdit;
            grdSearch.DisplayLayout.Bands[0].Columns["SendPrintText"].CellActivation = Activation.NoEdit;
            grdSearch.DisplayLayout.Bands[0].Columns["EmailAddress"].CellActivation = Activation.NoEdit;
            grdSearch.DisplayLayout.Bands[0].Columns["TaskDescription"].CellActivation = Activation.NoEdit;

            grdSearch.DisplayLayout.Bands[0].Columns["colTask"].Header.VisiblePosition = 0;
            grdSearch.DisplayLayout.Bands[0].Columns["TaskName"].Header.VisiblePosition = 1;
            grdSearch.DisplayLayout.Bands[0].Columns["TaskDescription"].Header.VisiblePosition = 2;
            grdSearch.DisplayLayout.Bands[0].Columns["PerformTaskText"].Header.VisiblePosition = 3;
            grdSearch.DisplayLayout.Bands[0].Columns["StartDate"].Header.VisiblePosition = 4;
            grdSearch.DisplayLayout.Bands[0].Columns["StartTime"].Header.VisiblePosition = 5;
            grdSearch.DisplayLayout.Bands[0].Columns["SendEmailText"].Header.VisiblePosition = 6;
            grdSearch.DisplayLayout.Bands[0].Columns["SendPrintText"].Header.VisiblePosition = 7;
            grdSearch.DisplayLayout.Bands[0].Columns["EnabledText"].Header.VisiblePosition = 8;
            grdSearch.DisplayLayout.Bands[0].Columns["EmailAddress"].Header.VisiblePosition = 9;
            grdSearch.DisplayLayout.Bands[0].Columns["LastExecuted"].Header.VisiblePosition = 10;

        }

        private void FillHistoryGrid()
        {
            ScheduledTasksLog oScheduledTasksLog = new ScheduledTasksLog();
            int iTask = -1;
            if (cmbTaskList.SelectedIndex != 0)
                iTask = ScheduledTasks.GetTaskIndex(cmbTaskList.SelectedItem.Tag.ToString());
            DataSet ds = oScheduledTasksLog.GetScheduledTasksLogList(iTask, dtFromDate.Text, dtToDate.Text);
            this.grdHistory.DataSource = null;
            this.grdHistory.DataSource = ds;
            this.grdHistory.Refresh();

            ApplyHistoryGridFormat();
        }

        private void ApplyHistoryGridFormat()
        {
            grdHistory.DisplayLayout.Bands[0].Columns["ScheduledTasksLogID"].Hidden = true;
            grdHistory.DisplayLayout.Bands[0].Columns["TaskName"].Width = 200;
            grdHistory.DisplayLayout.Bands[0].Columns["TaskStatus"].Width = 120;
            grdHistory.DisplayLayout.Bands[0].Columns["LogDescription"].Width = 310;
            grdHistory.DisplayLayout.Bands[0].Columns["StartDate"].Width = 85;
            grdHistory.DisplayLayout.Bands[0].Columns["StartTime"].Width = 85;
            grdHistory.DisplayLayout.Bands[0].Columns["EndTime"].Width = 85;
            grdHistory.DisplayLayout.Bands[0].Columns["ScheduledTasksID"].Hidden = true;
        }

        private void FillTaskCombos()
        {
            ScheduledTasks.AddTaskList();
            cmbTaskList.Items.Clear();
            cbSelectedTask.Items.Clear();

            Infragistics.Win.ValueListItem valItem = new Infragistics.Win.ValueListItem("0", "All");
            valItem.Tag = "0";
            cmbTaskList.Items.Add(valItem);
            foreach (DataRow dr in ScheduledTasks.dtTaskList.Rows)
            {
                Infragistics.Win.ValueListItem valuelstItem = new Infragistics.Win.ValueListItem();
                valuelstItem.DataValue = (Convert.ToInt32(dr["ID"].ToString()) + 1).ToString();
                valuelstItem.DisplayText = dr["TaskName"].ToString();
                valuelstItem.Tag = dr["Task"].ToString();
                cmbTaskList.Items.Add(valuelstItem);
            }

            Infragistics.Win.ValueListItem valItem1 = new Infragistics.Win.ValueListItem("0", "All");
            valItem.Tag = "0";
            cbSelectedTask.Items.Add(valItem1);
            foreach (DataRow dr in ScheduledTasks.dtTaskList.Rows)
            {
                Infragistics.Win.ValueListItem valuelstItem = new Infragistics.Win.ValueListItem();
                valuelstItem.DataValue = (Convert.ToInt32(dr["ID"].ToString()) + 1).ToString();
                valuelstItem.DisplayText = dr["TaskName"].ToString();
                valuelstItem.Tag = dr["Task"].ToString();
                cbSelectedTask.Items.Add(valuelstItem);
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                frmScheduledTasks oScheduledTask = new frmScheduledTasks();
                oScheduledTask.StartPosition = FormStartPosition.CenterScreen;
                oScheduledTask.ShowDialog(this);
                LoadTasks();
            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.Message);
            }
        }

        private void btnRunTask_Click(object sender, EventArgs e)
        {
            try
            {

                if (this.grdSearch.Selected.Rows.Count == 0)
                {
                    MessageBox.Show(this, "No task selected, please select task to run");
                    return;
                }

                ScheduledTaskExecute(Configuration.convertNullToInt(grdSearch.Selected.Rows[0].Cells["ScheduledTasksID"].Value.ToString()), Configuration.convertNullToInt(grdSearch.Selected.Rows[0].Cells["TaskId"].Value.ToString()));
            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.Message);
            }
        }

        private void frmScheduledTasksView_VisibleChanged(object sender, EventArgs e)
        {
            if (this.Visible)
            {
                string msg = @"If you set this computer as scheduler computer you would not be able to Setup scheduled task on any other computer!" + Environment.NewLine + "Do you want to set this computer as scheduler computer?";

                if (Configuration.convertNullToString(Configuration.CSetting.SchedularMachine) == "")
                {
                    msg = "In order for scheduler to work, you must set one computer as scheduler computer." + Environment.NewLine + msg;
                }
                else if (Configuration.CSetting.SchedularMachine.Trim() != Environment.MachineName)
                {
                    msg = "This machine is not configured as schedular machine." + Environment.NewLine + msg;
                }

                if (string.IsNullOrEmpty(Configuration.CSetting.SchedularMachine.Trim()))
                {
                    if (DialogResult.Yes == MessageBox.Show(msg, "Schedular Machine Config Warning!", MessageBoxButtons.YesNo, MessageBoxIcon.Warning))
                    {
                        Configuration.CSetting.SchedularMachine = Environment.MachineName;
                        Prefrences oPref = new Prefrences();
                        oPref.UpdateSettingDetails(Configuration.CSetting);
                        btnSyncWithWindowScheduler.PerformClick();
                    }
                    else
                    {
                        this.Close();
                    }
                }
                else if (Configuration.CSetting.SchedularMachine.Trim() != Environment.MachineName)
                {
                    MessageBox.Show("Scheduled tasks can not be configured on this computer!" + Environment.NewLine + "Scheduled tasks can only be configured on computer named " + Configuration.CSetting.SchedularMachine.Trim());
                    this.Close();
                }
            }
        }

        private void btnSyncWithWindowScheduler_Click(object sender, EventArgs e)
        {
            try
            {
                frmSyncStatus ofrmSyncStatus = new frmSyncStatus();
                ofrmSyncStatus.oScheduledTasksData = GetRecord(-1);
                ofrmSyncStatus.ShowDialog(this);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private ScheduledTasksData GetRecord(int iTaskId)
        {
            ScheduledTasks oScheduledTasks = new ScheduledTasks();
            ScheduledTasksData oScheduledTasksData = oScheduledTasks.GetScheduledTasksList(iTaskId);
            return oScheduledTasksData;
        }

        public void SyncScheduleTasks()
        {
            btnSyncWithWindowScheduler_Click(null, null);
        }

        public void ScheduledTaskExecute(int iScheduledTasksID, int TaskID)
        {
            ScheduledTasksSvr oScheduledTasksSvr = new ScheduledTasksSvr();
            ScheduledTasksLog oScheduledTasksLog = new ScheduledTasksLog();
            ScheduledTasksLogData oScheduledTasksLogData = new ScheduledTasksLogData();
            ScheduledTasksLogRow oScheduledTasksLogRow;
            oScheduledTasksLogRow = oScheduledTasksLogData.ScheduledTasksLog.AddRow(0, "", "", DateTime.Now, DateTime.Now, DateTime.Now, 0, "");
            
            string sBody = string.Empty;
            string sTaskStatus = string.Empty;
            string filePath = string.Empty;
            string sNoOfRecordAffect = string.Empty;
            ScheduledTasks oScheduledTasks = new ScheduledTasks();
            ScheduledTasksData oScheduledTasksData = oScheduledTasks.GetScheduledTaskByScheduledTasksID(iScheduledTasksID);
            ScheduledTasksRow row = (ScheduledTasksRow)oScheduledTasksData.ScheduledTasks.Rows[0];
            System.Int32 ScheduledTasksLogID = 0;
            try
            {
                oScheduledTasksLogRow.TaskStatus = "In Progress...";
                oScheduledTasksLogRow.ScheduledTasksID = iScheduledTasksID;
                oScheduledTasksLogRow.LogDescription = "";

                // if Scheduler machine is not set then set this machine as scheduler machine 
                // and synch all db tasks to windows tasks
                if (Configuration.CSetting.SchedularMachine.Trim() == "")
                {
                    Configuration.CSetting.SchedularMachine = Environment.MachineName;
                    Prefrences oPref = new Prefrences();
                    oPref.UpdateSettingDetails(Configuration.CSetting);
                    frmScheduledTasksView frmSchedular = new frmScheduledTasksView();
                    frmSchedular.SyncScheduleTasks();
                }
                
                if (Configuration.CSetting.SchedularMachine.Trim() != Environment.MachineName)
                {
                    oScheduledTasksLogRow.TaskStatus = "In Complete";
                    oScheduledTasksLogRow.LogDescription = "Computer name Is not matching with schedular configured machine.";
                    ScheduledTasksLogID = oScheduledTasksLog.Persist(oScheduledTasksLogData);
                    if (ScheduledTasksLogID != 0)
                        oScheduledTasksLogRow.ScheduledTasksLogID = ScheduledTasksLogID;
                    return;
                }

                ICommandLIneTaskControl obj = null;
                Type type = Type.GetType(oScheduledTasks.GetTask(TaskID), false, false);
                obj = (ICommandLIneTaskControl)System.Activator.CreateInstance(type);
                ScheduledTasksLogID = oScheduledTasksLog.Persist(oScheduledTasksLogData);
                if (ScheduledTasksLogID != 0)
                    oScheduledTasksLogRow.ScheduledTasksLogID = ScheduledTasksLogID;

                obj.RunTask(iScheduledTasksID, ref filePath, row.SendPrint, ref sNoOfRecordAffect);

                //if (obj.GetType().Name == "FrmRefsDueRep")
                //{
                //    FrmRefQue oFrmRefQue = new FrmRefQue();
                //    oFrmRefQue.ShowInTaskbar = false;
                //    oFrmRefQue.Opacity = 0.0;
                //    oFrmRefQue.oFrmRefsDueRep = (FrmRefsDueRep)obj;
                //    oFrmRefQue.CalledFromCommLine = true;
                //    oFrmRefQue.CalledFromWinScheduled = true;
                //    oFrmRefQue.ShowDialog();
                //    sNoOfRecordAffect = oFrmRefQue.sNoOfRecordMessage;
                //}

                sBody = "<b> Task Name: </b>";
                sBody = sBody + " " + Configuration.convertNullToString(row.TaskName);
                sBody = sBody + "<br />";

                sBody = sBody + "<b> Task Description: </b>";
                sBody = sBody + " " + Configuration.convertNullToString(row.TaskDescription);
                sBody = sBody + "<br />";

                oScheduledTasksLogRow.EndTime = DateTime.Now;
                oScheduledTasksLogRow.TaskStatus = "Complete";
                oScheduledTasksLogRow.LogDescription = "";
                ScheduledTasksLogID = oScheduledTasksLog.Persist(oScheduledTasksLogData);
                if (ScheduledTasksLogID != 0)
                    oScheduledTasksLogRow.ScheduledTasksLogID = ScheduledTasksLogID;

                sTaskStatus = "<b> Task Status: </b>";
                sTaskStatus = sTaskStatus + " " + Configuration.convertNullToString(oScheduledTasksLogRow.TaskStatus);
                sTaskStatus = sTaskStatus + "<br />";

                if (sNoOfRecordAffect.Trim() != string.Empty)
                    sTaskStatus = sTaskStatus + sNoOfRecordAffect + "<br />";

                if (row.SendEmail == true)
                {
                    string strSubject = string.Empty;
                    if (Configuration.convertNullToString(row.TaskDescription).Trim() != "")
                        strSubject = Configuration.convertNullToString(row.TaskDescription).Trim();
                    else
                        strSubject = Configuration.convertNullToString(row.TaskName);

                    if (row.TaskId == 8 && filePath != "")  //PRIMEPOS-3042 22-Dec-2021 JY Added for Station close
                    {
                        frmStationClose ofrmStationClose = new frmStationClose();
                        List<ReportClass> lstReportClass = new List<ReportClass>();
                        string[] StationCloseIDs = filePath.Split(',');
                        for (int i = 0; i < StationCloseIDs.Length; i++)
                        {
                            List<ReportClass> lst = ofrmStationClose.EmailReport(Configuration.convertNullToInt(StationCloseIDs[i]), true);
                            lstReportClass.AddRange(lst);
                        }
                        clsReports.EmailReport(lstReportClass, row.EmailAddress, strSubject + " - CloseStation#: " + filePath, sBody + sTaskStatus + "<br />", "File", false, true);
                    }
                    else if (row.TaskId == 9 && filePath != "")    //PRIMEPOS-3039 16-Dec-2021 JY Added for Process End of Day
                    {
                        frmEndOfDay ofrmEndOfDay = new frmEndOfDay();
                        ofrmEndOfDay.EmailReport(filePath, strSubject, sBody + sTaskStatus + "<br />", row.EmailAddress, true);
                    }
                    else if (row.TaskId == 10)  //PRIMEPOS-3042 22-Dec-2021 JY Added for Station close and EOD together
                    {
                        if (filePath != "")
                        {
                            frmStationClose ofrmStationClose = new frmStationClose();
                            List<ReportClass> lstReportClass = new List<ReportClass>();
                            string[] StationCloseIDs = filePath.Split(',');
                            for (int i = 0; i < StationCloseIDs.Length; i++)
                            {
                                List<ReportClass> lst = ofrmStationClose.EmailReport(Configuration.convertNullToInt(StationCloseIDs[i]), true);
                                lstReportClass.AddRange(lst);
                            }
                            clsReports.EmailReport(lstReportClass, row.EmailAddress, strSubject + " - CloseStation#: " + filePath, sBody + sTaskStatus + "<br />", "File", false, true);
                        }

                        string strEOD = string.Empty;
                        string sNoOfRecordAffect1 = string.Empty;
                        frmEndOfDay ofrmEndOfDay = new frmEndOfDay();
                        ofrmEndOfDay.RunTask(iScheduledTasksID, ref strEOD, row.SendPrint, ref sNoOfRecordAffect1);

                        if (sNoOfRecordAffect1.Trim() != string.Empty)
                            sTaskStatus += sNoOfRecordAffect1 + "<br />";

                        if (strEOD != "")
                            ofrmEndOfDay.EmailReport(strEOD, strSubject, sBody + sTaskStatus + "<br />", row.EmailAddress, true);
                        
                        if (filePath == "" && strEOD == "")
                        {
                            sTaskStatus = "<b> Task Status: </b> Complete<br />No Transactions Found For Close Station and End Of Day<br />";
                            clsReports.EmailReport(strSubject, sBody + sTaskStatus + "<br />", filePath, row.EmailAddress);
                        }
                    }
                    else
                    { 
                        clsReports.EmailReport(strSubject, sBody + sTaskStatus + "<br />", filePath, row.EmailAddress);
                    }
                }
                else
                {
                    if (row.TaskId == 10)  //PRIMEPOS-3042 22-Dec-2021 JY Added for Station close and EOD together
                    {
                        string strEOD = string.Empty;
                        string sNoOfRecordAffect1 = string.Empty;
                        frmEndOfDay ofrmEndOfDay = new frmEndOfDay();
                        ofrmEndOfDay.RunTask(iScheduledTasksID, ref strEOD, row.SendPrint, ref sNoOfRecordAffect1);
                    }
                }
                
                if (File.Exists(filePath))
                {
                    try
                    {
                        System.Threading.Thread.Sleep(5000);
                        File.Delete(filePath);
                    }
                    catch (IOException ie)
                    {
                        System.Threading.Thread.Sleep(60 * 1000);
                    }
                }
            }
            catch (Exception exc)
            {
                oScheduledTasksLogRow.EndTime = DateTime.Now;
                oScheduledTasksLogRow.TaskStatus = "Incomplete";
                oScheduledTasksLogRow.LogDescription = exc.Message;
                ScheduledTasksLogID = oScheduledTasksLog.Persist(oScheduledTasksLogData);
                if (ScheduledTasksLogID != 0)
                    oScheduledTasksLogRow.ScheduledTasksLogID = ScheduledTasksLogID;

                sTaskStatus = "<b> Task Status: </b>";
                sTaskStatus = sTaskStatus + " " + oScheduledTasksLogRow.TaskStatus;
                sTaskStatus = sTaskStatus + "</br>";

                if (row.SendEmail == true)
                {
                    string strSubject = string.Empty;
                    if (Configuration.convertNullToString(row.TaskDescription).Trim() != "")
                        strSubject = Configuration.convertNullToString(row.TaskDescription).Trim();
                    else
                        strSubject = Configuration.convertNullToString(row.TaskName);

                    clsReports.EmailReport(strSubject, sBody + sTaskStatus + "<br />", filePath, row.EmailAddress);
                }
                
                if (File.Exists(filePath))
                {
                    try
                    {
                        System.Threading.Thread.Sleep(5000);
                        File.Delete(filePath);
                    }
                    catch (IOException ie)
                    {
                        System.Threading.Thread.Sleep(60 * 1000);
                    }
                }
                logger.Fatal(exc, "ScheduledTaskExecute(int iScheduledTasksID, int TaskID)");  //PRIMEPOS-2971 07-Jun-2021 JY Added
                //ErrorHandler.throwException(exc, "", "");
            }
            finally
            {
                clsMain.bApplicationClosed = true;
                if (File.Exists(filePath))
                {
                    try
                    {
                        File.Delete(filePath);
                    }
                    catch
                    {
                    }
                }
            }
        }

        public string GetTask(int taskIndex)
        {
            string taskname = string.Empty;
            if (ScheduledTasks.dtTaskList == null)
                ScheduledTasks.AddTaskList();
            foreach (DataRow dr in ScheduledTasks.dtTaskList.Select("ID = " + taskIndex))
            {
                taskname = dr["Task"].ToString();
            }
            return taskname;
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {

            if (this.grdSearch.Selected.Rows.Count == 0)
            {
                MessageBox.Show(this, "No task selected, please select task to Edit");
                return;
            }
            try
            {
                logger.Trace("btnEdit_Click(object sender, EventArgs e) - " + clsPOSDBConstants.Log_Entering);
                if (this.grdSearch.Rows.Count > 0)
                {
                    if (grdSearch.ActiveRow == null)
                        if (grdSearch.ActiveRow.Cells.Count == 0)
                            return;
                    frmScheduledTasks ofrmScheduledTasks = new frmScheduledTasks();
                    ofrmScheduledTasks.Edit(Configuration.convertNullToInt(grdSearch.ActiveRow.Cells["ScheduledTasksID"].Text));
                    ofrmScheduledTasks.ShowDialog(this);
                    if (!ofrmScheduledTasks.IsCanceled)
                    {
                        btnSearch_Click(null, null);
                    }
                }
                logger.Trace("btnEdit_Click(object sender, EventArgs e) - " + clsPOSDBConstants.Log_Exiting);
            }
            catch (Exception exp)
            {
                logger.Fatal(exp, "btnEdit_Click(object sender, EventArgs e)");
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                LoadTasks();
            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.grdSearch.Selected.Rows.Count == 0)
                {
                    MessageBox.Show(this, "No task selected, please select task to delete");
                    return;
                }
                if (MessageBox.Show("Are you sure you want to delete this task " + grdSearch.Rows[grdSearch.Selected.Rows[0].Index].Cells["TaskName"].Value.ToString(), "Confirm Task Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    int iTaskId = Configuration.convertNullToInt(grdSearch.Rows[grdSearch.Selected.Rows[0].Index].Cells["ScheduledTasksID"].Value.ToString());
                    if (oScheduledTasks.Delete(iTaskId) == true)
                    {
                        ManageScheduledTask.DeleteTask(grdSearch.Rows[grdSearch.Selected.Rows[0].Index].Cells["TaskName"].Value.ToString());
                        LoadTasks();
                    }
                }
            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.Message);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmScheduledTasksView_KeyUp(object sender, KeyEventArgs e)
        {
            logger.Trace("frmScheduledTasksView_KeyUp() - " + clsPOSDBConstants.Log_Entering);

            switch (e.KeyData)
            {
                case Keys.F2:
                    if (ultraTabScheduledTasks.ActiveTab.Index == 0) btnAdd_Click(null, null);
                    break;
                case Keys.F3:
                    if (ultraTabScheduledTasks.ActiveTab.Index == 0)
                    {
                        if (grdSearch.Rows.Count < 1) return;
                        btnEdit_Click(null, null);
                    }
                    break;
                case Keys.F4:
                    if (ultraTabScheduledTasks.ActiveTab.Index == 0)
                    {
                        btnSearch_Click(null, null);
                    }
                    break;
                case Keys.F6:
                    if (ultraTabScheduledTasks.ActiveTab.Index == 1)
                    {
                        btnHistorySearch_Click(null, null);
                    }
                    break;
                case Keys.F10:
                    if (ultraTabScheduledTasks.ActiveTab.Index == 0)
                        btnDelete_Click(null, null);
                    break;
            }
            logger.Trace("frmScheduledTasksView_KeyUp() - " + clsPOSDBConstants.Log_Exiting);
        }

        private void frmScheduledTasksView_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                logger.Trace("frmScheduledTasksView_KeyDown() - " + clsPOSDBConstants.Log_Entering);
                if (e.KeyData == System.Windows.Forms.Keys.Enter)
                {
                }
                else
                {
                    if (e.Alt)
                        ShortCutKeyAction(e.KeyCode);
                }
                logger.Trace("frmScheduledTasksView_KeyDown() - " + clsPOSDBConstants.Log_Exiting);
            }
            catch (Exception exp)
            {
                logger.Fatal(exp, "frmScheduledTasksView_KeyDown()");
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }

        private void ShortCutKeyAction(Keys KeyCode)
        {
            switch (KeyCode)
            {
                case Keys.C:
                    if (pnlClose.Visible)
                        btnClose_Click(btnClose, new EventArgs());
                    break;
                case Keys.R:
                    if (ultraTabScheduledTasks.ActiveTab.Index == 0)
                        btnRunTask_Click(btnRunTask, new EventArgs());
                    break;
                case Keys.S:
                    if (ultraTabScheduledTasks.ActiveTab.Index == 0)
                        btnSyncWithWindowScheduler_Click(btnSyncWithWindowScheduler, new EventArgs());
                    break;
                default:
                    break;
            }
        }

        private void btnHistorySearch_Click(object sender, EventArgs e)
        {
            try
            {
                FillHistoryGrid();
            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }

        private void ultraTabScheduledTasks_SelectedTabChanged(object sender, Infragistics.Win.UltraWinTabControl.SelectedTabChangedEventArgs e)
        {
            try
            {
                if (e.Tab.Key == "TasksList")
                    setControls(true);
                else
                    setControls(false);
            }
            catch { }
        }

        private void setControls(bool bvisible)
        {
            pnlAdd.Visible = bvisible;
            pnlEdit.Visible = bvisible;
            pnlDelete.Visible = bvisible;
            pnlSyncWithWindowScheduler.Visible = bvisible;
        }

        private void grdSearch_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            try
            {
                e.Layout.Override.SelectTypeRow = SelectType.Single;
                e.Layout.Override.ActiveCellBorderThickness = 1;
                grdSearch.DisplayLayout.Bands[0].Columns["LastExecuted"].Format = "yyyy-MM-dd HH:mm:ss tt";
                grdSearch.DisplayLayout.Bands[0].Columns["LastExecuted"].Width = 150;
            }
            catch { }
        }

        private void grdSearch_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                Point point = System.Windows.Forms.Cursor.Position;
                point = this.grdSearch.PointToClient(point);
                Infragistics.Win.UIElement oUI = this.grdSearch.DisplayLayout.UIElement.ElementFromPoint(point);

                if (oUI == null)
                    return;

                while (oUI != null)
                {
                    if (oUI.GetType() == typeof(Infragistics.Win.UltraWinGrid.RowUIElement))
                    {
                        Infragistics.Win.UltraWinGrid.RowUIElement oRowUI = oUI as Infragistics.Win.UltraWinGrid.RowUIElement;
                        btnEdit.PerformClick();
                    }
                    oUI = oUI.Parent;
                }
            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }
    }
}