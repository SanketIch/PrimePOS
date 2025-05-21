using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Infragistics.Win.UltraWinEditors;
using POS_Core.BusinessRules;
using POS_Core.CommonData;
using POS_Core.CommonData.Rows;
using POS_Core.Resources;
using POS_Core.DataAccess;

namespace POS_Core_UI
{
    public partial class usrMonthly : UserControl, IScheduledControl
    {
        usrMonthlyBL ousrMonthlyBL = new usrMonthlyBL();
        public usrMonthlyData ousrMonthlyData = new usrMonthlyData();
        usrMonthlyRow ousrMonthlyRow;

        bool isSelf = true;

        public usrMonthly()
        {
            InitializeComponent();
            this.cmbSelectMonth.AfterEnterEditMode += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
            this.cmbSelectMonth.AfterExitEditMode += new System.EventHandler(clsUIHelper.AfterExitEditMode);
            this.cmbOnweekPeriod.AfterEnterEditMode += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
            this.cmbOnweekPeriod.AfterExitEditMode += new System.EventHandler(clsUIHelper.AfterExitEditMode);
            this.cmbDays.AfterEnterEditMode += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
            this.cmbDays.AfterExitEditMode += new System.EventHandler(clsUIHelper.AfterExitEditMode);
            SetNew();
        }

        private void SetNew()
        {
            ousrMonthlyBL = new usrMonthlyBL();
            ousrMonthlyData = new usrMonthlyData();

            Clear();
            ousrMonthlyRow = ousrMonthlyData.ScheduledTasksDetailMonth.AddRow(0, true, "1,2,3,4,5,6,7,8,9,10,11,12", "1", "", "");
        }

        private void Clear()
        {
        }

        public void SetFocusControl()
        {
            this.cmbSelectMonth.Focus();
            this.cmbSelectMonth.SelectAll();
        }

        private void fillComboCheckbox(ref UltraComboEditor cmb)
        {
            for (int i = 1; i <= (cmbSelectMonth.Items.Count - 1); i++)
            {
                cmb.Items[i].CheckState = cmb.Items[0].CheckState;
            }

            cmb.Items[0].CheckState = CheckState.Unchecked;
            isSelf = true;
        }

        private void cmbSelectMonth_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                UltraComboEditor cmb = (UltraComboEditor)sender;
                if (cmb.SelectedIndex == 0)
                    if (isSelf == true)
                    {
                        isSelf = false;
                        fillComboCheckbox(ref cmb);
                    }
            }
            catch { isSelf = true; }
        }

        private void rbDays_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rdb = (RadioButton)sender;
            if (rdb.Checked == true)
            {
                cmbDays.Enabled = false;
                cmbOnweekPeriod.Enabled = false;
                pnl.Enabled = true;

            }
            else
            {
                cmbDays.Enabled = true;
                cmbOnweekPeriod.Enabled = true;
                pnl.Enabled = false;
            }
        }

        public string getComboData(UltraComboEditor cmb, int StartingIndex)
        {
            string sSelectedComboIndex = string.Empty;
            try
            {

                sSelectedComboIndex = "";
                for (int count = StartingIndex; count < (cmbSelectMonth.Items.Count); count++)
                {
                    if (cmb.Items[count].CheckState == CheckState.Checked)
                    {
                        if (sSelectedComboIndex == "")
                        {
                            sSelectedComboIndex = count.ToString();
                        }
                        else
                        {
                            sSelectedComboIndex = sSelectedComboIndex + "," + count.ToString();
                        }
                    }
                }
            }
            catch
            {
            }
            return sSelectedComboIndex;
        }

        public bool checkValidation()
        {
            try
            {
                errorProvider.Clear();
                if (ousrMonthlyRow.SelectionMonths == string.Empty)
                {
                    errorProvider.SetError(cmbSelectMonth, "At least one month must be select.");
                    return false;
                }
                else if (this.rbOn.Checked)
                {
                    if (ousrMonthlyRow.weekDays == string.Empty && ousrMonthlyRow.Monthperiods == string.Empty)
                        return false;
                }
                else
                {
                    if (ousrMonthlyRow.SelectionDays == string.Empty)
                    {
                        errorProvider.SetError(lblError, "At least one month day must be select.");
                        return false;
                    }
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        private void cmbSelectMonth_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                ousrMonthlyRow.SelectionMonths = getComboData(cmbSelectMonth, 1);
            }
            catch
            {
            }

        }

        private void usrMonthly_Validating(object sender, CancelEventArgs e)
        {
        }

        private void cmbOnweekPeriod_Validating(object sender, CancelEventArgs e)
        {
            ousrMonthlyRow.Monthperiods = getComboData(cmbOnweekPeriod, 0);
        }

        private void cmbDays_Validating(object sender, CancelEventArgs e)
        {
            ousrMonthlyRow.weekDays = getComboData(cmbDays, 1);
        }

        private void pnl_Validating(object sender, CancelEventArgs e)
        {
            ousrMonthlyRow.SelectionDays = string.Empty;
            foreach (Control ctrl in pnl.Controls[0].Controls)
            {
                if (ctrl.Name != "lblError")
                {
                    UltraCheckEditor chk = (UltraCheckEditor)ctrl;
                    if (chk.Checked == true)
                    {
                        if (ousrMonthlyRow.SelectionDays == string.Empty)
                            ousrMonthlyRow.SelectionDays = chk.Text;
                        else
                            ousrMonthlyRow.SelectionDays = ousrMonthlyRow.SelectionDays + "," + chk.Text;
                    }
                }
            }
        }


        public Object getObject()
        {
            return ousrMonthlyData;
        }

        public void SetObject(Object ScheduledTasksID)
        {
            ousrMonthlyData = ousrMonthlyBL.GetScheduledTasksDetailMonth((int)ScheduledTasksID);
            if (ousrMonthlyData != null && ousrMonthlyData.ScheduledTasksDetailMonth != null && ousrMonthlyData.ScheduledTasksDetailMonth.Rows.Count > 0)
            {
                ousrMonthlyRow = (usrMonthlyRow)ousrMonthlyData.ScheduledTasksDetailMonth.Rows[0];
                int[] iDays = ousrMonthlyRow.SelectionDays.Split(',').Select(x => int.Parse(x)).ToArray();
                int[] iMonths = ousrMonthlyRow.SelectionMonths.Split(',').Select(x => int.Parse(x)).ToArray();

                foreach (Control ctrl in pnl.Controls[0].Controls)
                {
                    if (ctrl.Name != "lblError")
                    {
                        UltraCheckEditor chk = (UltraCheckEditor)ctrl;
                        chk.Checked = false;
                    }
                }

                for (int count = 0; count < (iDays.Length); count++)
                {
                    foreach (Control ctrl in pnl.Controls[0].Controls)
                    {
                        if (ctrl.Name != "lblError")
                        {
                            UltraCheckEditor chk = (UltraCheckEditor)ctrl;
                            if (chk.Text == iDays[count].ToString())
                                chk.Checked = true;
                        }
                    }
                }

                for (int count = 0; count < (iMonths.Length); count++)
                {
                    cmbSelectMonth.Items[iMonths[count]].CheckState = CheckState.Checked;
                }
            }
        }

        private void usrMonthly_Enter(object sender, EventArgs e)
        {
            if (ousrMonthlyRow == null)
                SetNew();
        }

        public Control getControl()
        {
            return this;
        }

        public void save(int iScheduledTasksID)
        {
            if (ousrMonthlyData != null && ousrMonthlyData.Tables.Count > 0 && ousrMonthlyData.ScheduledTasksDetailMonth.Rows.Count > 0)
            {
                ousrMonthlyData.ScheduledTasksDetailMonth.Rows[0][clsPOSDBConstants.ScheduledTasksDetailMonth_Fld_ScheduledTasksID] = iScheduledTasksID;
                ousrMonthlyBL.Insert(ousrMonthlyData);
            }
        }

        public void Insert(int iScheduledTasksID)
        {
            ousrMonthlyRow.ScheduledTasksID = iScheduledTasksID;
            ousrMonthlyBL.Persist(ousrMonthlyData);
        }
    }
}
