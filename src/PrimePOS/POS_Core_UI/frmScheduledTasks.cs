using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using POS_Core.CommonData;
using POS_Core.CommonData.Rows;
using POS_Core.BusinessRules;
using POS_Core.ErrorLogging;
using POS_Core.Resources;
using Infragistics.Win;


namespace POS_Core_UI
{
    public partial class frmScheduledTasks : Form
    {
        private bool bEditMode = false;
        public bool IsCanceled = true;

        private ScheduledTasks oScheduledTasks = new ScheduledTasks();
        private ScheduledTasksData oScheduledTasksData = new ScheduledTasksData();
        private ScheduledTasksRow oScheduledTasksRow;

        private ICommandLIneTaskControl obj;
        IScheduledControl oPerformTaskBaseControl = null;

        usrDaily ousrDaily = new usrDaily();
        usrMonthly ousrMonthly = new usrMonthly();
        usrWeekly ousrWeekly = new usrWeekly();
        private bool isClose = true;

        public frmScheduledTasks()
        {
            InitializeComponent();
            try
            {
                Initialize();
            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }

        public void Initialize()
        {
            SetNew();
        }

        private void SetNew()
        {
            oScheduledTasks = new ScheduledTasks();
            oScheduledTasksData = new ScheduledTasksData();

            Clear();

            oScheduledTasksRow = oScheduledTasksData.ScheduledTasks.AddRow(0, "", "", 0, 1, DateTime.Now, DateTime.Now, 0, false, "", false, 0, 0, true, true);

            oScheduledTasksRow.RepeatTaskInterval = Configuration.convertNullToInt64(tsRepeat.MinValue);
            oScheduledTasksRow.Duration = Configuration.convertNullToInt64(tsduration.MinValue);
            
            this.ultraExplorerBarTasks.Groups[0].Items[oScheduledTasks.GetTask(0)].Checked = true;
            GbCustomControls.Controls.Add(obj.GetParameterControl());

            chkSendPrint.Checked = true;
            chkEnabled.Checked = true;
            chkSendEmail.Checked = false;
        }

        private void Clear()
        {
            obj = new POS_Core_UI.Reports.ReportsUI.frmRptSalesTax();
            dtpStartDate.Value = DateTime.Today.Date;
            dtpStartTime.Value = System.DateTime.Now;
            txtTaskName.Text = "";
            txtTaskDescription.Text = "";
            txtEmailAddress.Text = "";
            optTaskPerform.CheckedIndex = 0;          

            txtTaskName.Focus();

            if (oScheduledTasksData != null) oScheduledTasksData.ScheduledTasks.Rows.Clear();
        }

        private void frmScheduledTasks_Load(object sender, EventArgs e)
        {
            #region Events attachment
            this.txtTaskName.AfterEnterEditMode += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
            this.txtTaskName.AfterExitEditMode += new System.EventHandler(clsUIHelper.AfterExitEditMode);
            this.txtTaskDescription.AfterEnterEditMode += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
            this.txtTaskDescription.AfterExitEditMode += new System.EventHandler(clsUIHelper.AfterExitEditMode);
            #endregion
            IsCanceled = true;
            clsUIHelper.setColorSchecme(this);

            if (bEditMode)
                this.txtTaskName.Enabled = false;
            else
                this.txtTaskName.Select();
        }

        private void btnProcess_Click(object sender, EventArgs e)
        {
            if (Validate())
            {
                //add task
                if (ManageScheduledTask.addModifyScheuled(oScheduledTasksData.ScheduledTasks[0], oPerformTaskBaseControl) == true)
                {
                    if (Save())
                    {
                        try
                        {
                            IsCanceled = false;
                        }
                        catch (Exception Ex)
                        {
                            IsCanceled = false;
                        }
                        finally
                        {
                            this.Close();
                        }
                    }
                }
            }
        }

        private bool Validate()
        {
            bool bValid = false;
            if (oPerformTaskBaseControl != null)
            {
                if (oPerformTaskBaseControl.checkValidation() == false)
                    bValid = true;
            }
            if (ValidateFields())
                bValid = true;
            return bValid;
        }

        private bool Save()
        {
            bool bRetVal = false;
            try
            {
                int ScheduledTasksID = oScheduledTasks.Persist(oScheduledTasksData);

                if (bEditMode)
                {
                    ScheduledTasksID = oScheduledTasksRow.ScheduledTasksID;
                }
                usrWeeklyBL ousrWeeklyBL = new usrWeeklyBL();
                ousrWeeklyBL.Delete(ScheduledTasksID);
                usrMonthlyBL ousrMonthlyBL = new usrMonthlyBL();
                ousrMonthlyBL.Delete(ScheduledTasksID);

                if (oScheduledTasksRow.PerformTask == 1)
                {
                    ousrWeekly.save(ScheduledTasksID);
                }
                else if (oScheduledTasksRow.PerformTask == 2)
                {
                    ousrMonthly.save(ScheduledTasksID);
                }

                #region parameters
                DataTable dt = new DataTable();
                dt.Columns.Add("ControlsName");
                dt.Columns.Add("ControlsValue");

                obj.GetTaskParameters(ref dt, ScheduledTasksID);
                obj.SaveTaskParameters(dt, ScheduledTasksID);
                #endregion

                bRetVal = true;
            }
            catch (Exception Ex)
            {
                clsUIHelper.ShowErrorMsg(Ex.Message);
            }
            return bRetVal;
        }

        private bool ValidateFields()
        {
            bool bRetVal = false;

            string strErrorMsg = "";
            ScheduledTasksRow oRow;

            try
            {
                if ((oScheduledTasksRow.TaskName.Trim() == ""))
                {
                    if (strErrorMsg == "")
                        strErrorMsg = "Please enter task name.";
                    else
                        strErrorMsg = strErrorMsg + Environment.NewLine + "Please enter task name.";
                }
                if (oScheduledTasksRow.SendEmail == false && oScheduledTasksRow.SendPrint == false)
                {
                    if (strErrorMsg == "")
                        strErrorMsg = "Send Email or Send Print should be selected.";
                    else
                        strErrorMsg = strErrorMsg + Environment.NewLine + "Send Email or Send Print should be selected.";
                }
                if (oScheduledTasksRow.SendEmail == true)
                {
                    if (oScheduledTasksRow.EmailAddress.Trim() == string.Empty)
                    {
                        if (strErrorMsg == "")
                            strErrorMsg = "Email can't be left blank.";
                        else
                            strErrorMsg = strErrorMsg + Environment.NewLine + "Email can't be left blank.";
                    }
                    string[] sEmail = oScheduledTasksRow.EmailAddress.Trim().Split(',');

                    foreach (string str in sEmail)
                    {
                        if (str.Trim() != string.Empty)
                        {
                            if (Configuration.ValidateEmailAddress(str.Trim()) == false)
                            {
                                if (strErrorMsg == "")
                                    strErrorMsg = "Invalid email address.";
                                else
                                    strErrorMsg = strErrorMsg + Environment.NewLine + "Invalid email address.";
                            }
                        }
                    }
                }

                if (strErrorMsg != "")
                    throw (new Exception(strErrorMsg));
                else
                    bRetVal = true;
            }
            catch (Exception Ex)
            {
                bRetVal = false;
                if (strErrorMsg != "")
                    clsUIHelper.ShowErrorMsg(strErrorMsg);
                else
                    clsUIHelper.ShowErrorMsg(Ex.Message);
            }
            return bRetVal;
        }

        private void frmScheduledTasks_Activated(object sender, EventArgs e)
        {
            clsUIHelper.CurrentForm = this;
        }

        private void frmScheduledTasks_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyData == System.Windows.Forms.Keys.Enter)
                {
                    this.SelectNextControl(this.ActiveControl, true, true, true, true);
                }
            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }

        private void TextBox_Validate(object sender, System.EventArgs e)
        {
            try
            {
                if (oScheduledTasksRow == null)
                    return;
                Infragistics.Win.UltraWinEditors.UltraTextEditor txtEditor = (Infragistics.Win.UltraWinEditors.UltraTextEditor)sender;
                switch (txtEditor.Name)
                {
                    case "txtTaskName":
                        oScheduledTasksRow.TaskName = txtTaskName.Text;
                        break;
                    case "txtTaskDescription":
                        oScheduledTasksRow.TaskDescription = txtTaskDescription.Text;
                        break;
                    case "txtEmailAddress":
                        oScheduledTasksRow.EmailAddress = txtEmailAddress.Text;
                        break;
                }
            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }

        private void CheckBoxs_Checked(object sender, System.EventArgs e)
        {
            try
            {
                if (oScheduledTasksRow == null)
                    return;
                Infragistics.Win.UltraWinEditors.UltraCheckEditor chkEditor = (Infragistics.Win.UltraWinEditors.UltraCheckEditor)sender;

                bool IsChecked = chkEditor.Checked;

                switch (chkEditor.Name)
                {
                    case "chkEnabled":
                        oScheduledTasksRow.Enabled = IsChecked;
                        break;
                    case "chkAdvancedSettings":
                        oScheduledTasksRow.AdvancedSeetings = IsChecked;
                        break;
                    case "chkSendEmail":
                        oScheduledTasksRow.SendEmail = IsChecked;
                        break;
                    case "chkSendPrint":
                        oScheduledTasksRow.SendPrint = IsChecked;
                        break;
                }
            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }

        private void dtpBoxs_Validate(object sender, System.EventArgs e)
        {
            try
            {
                Infragistics.Win.UltraWinEditors.UltraDateTimeEditor dtpEditor = (Infragistics.Win.UltraWinEditors.UltraDateTimeEditor)sender;
                DateTime date = Convert.ToDateTime(dtpEditor.Value);

                switch (dtpEditor.Name)
                {
                    case "dtpStartDate":
                        oScheduledTasksRow.StartDate = date;
                        break;
                    case "dtpStartTime":
                        oScheduledTasksRow.StartTime = date;
                        break;
                }
            }
            catch (Exception exp)
            {
                //clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }

        private void optTaskPerform_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                oScheduledTasksRow.PerformTask = optTaskPerform.CheckedIndex;
                switch (optTaskPerform.CheckedIndex)
                {
                    case 0:
                        ousrDaily = (usrDaily)oPerformTaskBaseControl;
                        break;
                    case 1:
                        ousrWeekly = (usrWeekly)oPerformTaskBaseControl;
                        break;
                    case 2:
                        ousrMonthly = (usrMonthly)oPerformTaskBaseControl;
                        break;
                    case 3:
                        oPerformTaskBaseControl = null;
                        break;
                }
            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }

        private void optTaskPerform_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                switch (optTaskPerform.CheckedIndex)
                {
                    case 0:
                        oPerformTaskBaseControl = ousrDaily;
                        break;
                    case 1:
                        oPerformTaskBaseControl = ousrWeekly;
                        break;
                    case 2:
                        oPerformTaskBaseControl = ousrMonthly;
                        break;
                    case 3:
                        oPerformTaskBaseControl = null;
                        break;

                }
                grbPrrformTaskControl.Controls.Clear();
                if (optTaskPerform.CheckedIndex != 3)
                {
                    grbPrrformTaskControl.Controls.Add(oPerformTaskBaseControl.getControl());
                    oPerformTaskBaseControl.SetFocusControl();
                    chkAdvancedSettings.Checked = true;
                    chkAdvancedSettings.Enabled = true;
                    chkEnabled.Checked = true;
                }
                else
                {
                    chkAdvancedSettings.Checked = false;
                    chkAdvancedSettings.Enabled = false;
                    chkEnabled.Checked = false;
                }
            }
            catch { }
        }

        public void Edit(Int32 ScheduledTasksID)
        {
            try
            {
                bEditMode = true;
                //        private ScheduledTasksData oScheduledTasksData = new ScheduledTasksData();
                //private ScheduledTasksRow oScheduledTasksRow;
                //private ScheduledTasks oScheduledTasks = new ScheduledTasks();

                oScheduledTasksData = oScheduledTasks.GetScheduledTaskByScheduledTasksID(ScheduledTasksID);
                oScheduledTasksRow = oScheduledTasksData.ScheduledTasks[0];
                if (oScheduledTasksRow != null)
                    Display();
            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }

        private void Display()
        {
            txtTaskName.Text = Configuration.convertNullToString(oScheduledTasksRow.TaskName);
            oScheduledTasksRow.TaskNameOld = Configuration.convertNullToString(oScheduledTasksRow.TaskName);
            txtTaskDescription.Text = Configuration.convertNullToString(oScheduledTasksRow.TaskDescription);
            optTaskPerform.CheckedIndex = Configuration.convertNullToInt(oScheduledTasksRow.PerformTask);
            chkEnabled.Checked = Configuration.convertNullToBoolean(oScheduledTasksRow.Enabled);
            dtpStartDate.Value = oScheduledTasksRow.StartDate;
            dtpStartTime.Value = oScheduledTasksRow.StartTime;
            chkSendEmail.Checked = Configuration.convertNullToBoolean(oScheduledTasksRow.SendEmail);
            chkSendPrint.Checked = Configuration.convertNullToBoolean(oScheduledTasksRow.SendPrint);
            txtEmailAddress.Text = Configuration.convertNullToString(oScheduledTasksRow.EmailAddress);
            chkAdvancedSettings.Checked = Configuration.convertNullToBoolean(oScheduledTasksRow.AdvancedSeetings);
            tsduration.Value = Configuration.convertNullToInt64(oScheduledTasksRow.Duration);
            tsRepeat.Value = Configuration.convertNullToInt64(oScheduledTasksRow.RepeatTaskInterval);

            string[] sControlNotBeInclude = new string[0];
            ultraExplorerBarTasks.Groups[0].Items[oScheduledTasks.GetTask(oScheduledTasksRow.TaskId)].Checked = true;

            if (oPerformTaskBaseControl != null)
                if (oPerformTaskBaseControl.GetType().Name == "usrDaily")
                    oPerformTaskBaseControl.SetObject(Configuration.convertNullToInt(oScheduledTasksRow.RepeatTask));
                else
                    oPerformTaskBaseControl.SetObject(Configuration.convertNullToInt(oScheduledTasksRow.ScheduledTasksID));

            //report parameters
            if (obj.GetType().Name != "frmPrintRxsFilledRepCus" && obj.GetType().Name != "FrmRefsDueRep" && obj.GetType().Name != "frmAutoBillingSettings")
            {
                object control = obj.GetParameterControl();
                if (control is Form)
                {
                    LoadControl(control as Form, sControlNotBeInclude);
                }
                else
                {
                    GbCustomControls.Controls.Add(control as Control);
                }
            }
            //else if (obj.GetType().Name == "frmPrintRxsFilledRepCus" || obj.GetType().Name == "FrmRefsDueRep" || obj.GetType().Name == "frmAutoBillingSettings")
            //{
            //    GbCustomControls.Controls.Add(btn);
            //    btn.Visible = true;
            //}
            obj.SetControlParameters(Configuration.convertNullToInt(oScheduledTasksRow.ScheduledTasksID));
            this.ActiveControl = txtTaskName;
        }

        private void ultraExplorerBarTasks_ItemCheckStateChanging(object sender, Infragistics.Win.UltraWinExplorerBar.CancelableItemEventArgs e)
        {
            try
            {
                string[] sControlNotBeInclude = new string[0];
                //if (e.Item.Key == "Phw.FrmDailyLog" || e.Item.Key == "Phw.FrmRxFilledSummary" || e.Item.Key == "Phw.frmPrintRxsFilledRepCus")
                //    chkSendPrint.Visible = true;
                //else
                //    chkSendPrint.Visible = false;

                Type type = Type.GetType(e.Item.Key, false, false);
                obj = (ICommandLIneTaskControl)System.Activator.CreateInstance(type);
                oScheduledTasksRow.TaskId = ScheduledTasks.GetTaskIndex(e.Item.Key);
                GbCustomControls.Text = e.Item.Text;
                GbCustomControls.Controls.Clear();
                if ((e.Item.Key == "Phw.frmPrintRxsFilledRepCus") || (e.Item.Key == "Phw.FrmRefsDueRep") || (e.Item.Key == "Phw.frmAutoBillingSettings"))
                {
                    btn.Visible = true;
                    GbCustomControls.Controls.Add(btn);
                }
                else
                {
                    btn.Visible = false;
                    object control = obj.GetParameterControl();
                    if (control is Form)
                    {
                        LoadControl(control as Form, sControlNotBeInclude);
                    }
                    else
                    {
                        GbCustomControls.Controls.Add(control as Control);
                        setKeydownevent(control as Control);
                    }
                }
            }
            catch (Exception ex) { }
        }

        private void setKeydownevent(Control ctrl)
        {
            foreach (Control ctlchild in ctrl.Controls)
            {
                ctlchild.KeyDown += new KeyEventHandler(this.GbCustomControls_KeyDown);
                if (ctlchild.HasChildren)
                    setKeydownevent(ctlchild);
            }
        }

        void LoadControl(Form ofrm, string[] sControlNotBeInclude)
        {
            for (int i = 0; i < sControlNotBeInclude.Length; i++)
            {
                try
                {
                    Control[] oControllCollection = ofrm.Controls.Find(sControlNotBeInclude[i], true);
                    if (oControllCollection.Length > 0)
                    {
                        oControllCollection[0].Visible = false;
                    }
                }
                catch
                { }
            }
            for (int i = ofrm.Controls.Count - 1; i >= 0; i--)
            {
                Control TempControl = ofrm.Controls[i];
                TempControl.Refresh();
                GbCustomControls.Controls.Add(TempControl);
            }
        }

        private void GbCustomControls_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13)
            {
                this.SelectNextControl((Control)sender, true, true, true, true);
            }
        }
    }
}
