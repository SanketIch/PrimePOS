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

namespace POS_Core_UI
{
    public partial class usrWeekly : UserControl, IScheduledControl
    {
        usrWeeklyBL ousrWeeklyBL = new usrWeeklyBL();
        public usrWeeklyData ousrWeeklyData = new usrWeeklyData();
        usrWeeklyRow ousrWeeklyRow;

        public usrWeekly()
        {
            InitializeComponent();
            this.txtRecur.AfterEnterEditMode += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
            this.txtRecur.AfterExitEditMode += new System.EventHandler(clsUIHelper.AfterExitEditMode);
            SetNew();
        }

        private void SetNew()
        {
            ousrWeeklyBL = new usrWeeklyBL();
            ousrWeeklyData = new usrWeeklyData();

            Clear();
            ousrWeeklyRow = ousrWeeklyData.ScheduledTasksDetailWeek.AddRow(0, 0, "1");
        }

        private void Clear()
        {
        }

        public void SetFocusControl()
        {
            this.txtRecur.Focus();
            this.txtRecur.SelectAll();
        }

        public bool checkValidation()
        {
            try
            {
                ousrWeeklyRow.SelectedDays = string.Empty;
                string str = string.Empty;
                foreach (Control ctrl in pnl.Controls[0].Controls)
                {
                    if (ctrl.Name != "lblError")
                    {
                        UltraCheckEditor chk = (UltraCheckEditor)ctrl;
                        switch (chk.Name)
                        {
                            case "chkSunday":
                                str = "0";
                                break;
                            case "chkMonday":
                                str = "1";
                                break;
                            case "chkTuesday":
                                str = "2";
                                break;
                            case "chkWednesday":
                                str = "3";
                                break;
                            case "chkThursday":
                                str = "4";
                                break;
                            case "chkFriday":
                                str = "5";
                                break;
                            case "chkSaturday":
                                str = "6";
                                break;
                        }

                        if (chk.Checked == true)
                        {
                            if (ousrWeeklyRow.SelectedDays == string.Empty)
                                ousrWeeklyRow.SelectedDays = str;
                            else
                                ousrWeeklyRow.SelectedDays = ousrWeeklyRow.SelectedDays + "," + str;
                        }
                        else
                        {
                            str = string.Empty;
                        }
                    }
                }
                int iDays = Convert.ToInt32(txtRecur.Value);
                if (ousrWeeklyRow.SelectedDays == string.Empty)
                {
                    errorProvider.SetError(lblError, "At least one week day must be select.");
                    return false;
                }
                else if (iDays < 1 && iDays > 52)
                {
                    errorProvider.SetError(lblError, "Days can’t be less than 0 or greater than 999.");
                    return false;
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        private void usrWeekly_Validating(object sender, CancelEventArgs e)
        {
        }

        private void usrWeekly_Enter(object sender, EventArgs e)
        {
            if (ousrWeeklyRow == null)
            {
                SetNew();
                ousrWeeklyRow.Days = Convert.ToInt32(txtRecur.Value);
            }
        }

        public Object getObject()
        {
            return ousrWeeklyData;
        }

        public void SetObject(Object ScheduledTasksID)
        {            
            ousrWeeklyData = ousrWeeklyBL.GetScheduledTasksDetailWeek((int)ScheduledTasksID);

            if (ousrWeeklyData != null && ousrWeeklyData.ScheduledTasksDetailWeek != null && ousrWeeklyData.ScheduledTasksDetailWeek.Rows.Count > 0)
            {
                ousrWeeklyRow = (usrWeeklyRow)ousrWeeklyData.ScheduledTasksDetailWeek.Rows[0];
                txtRecur.Value = Configuration.convertNullToString(ousrWeeklyRow.Days);
                int[] iDays = ousrWeeklyRow.SelectedDays.Split(',').Select(x => int.Parse(x)).ToArray();
                chkSunday.Checked = false;
                chkMonday.Checked = false;
                chkTuesday.Checked = false;
                chkWednesday.Checked = false;
                chkThursday.Checked = false;
                chkFriday.Checked = false;
                chkSaturday.Checked = false;

                for (int count = 0; count < (iDays.Length); count++)
                {
                    switch (iDays[count])
                    {
                        case 0:
                            chkSunday.Checked = true;
                            break;
                        case 1:
                            chkMonday.Checked = true;
                            break;
                        case 2:
                            chkTuesday.Checked = true;
                            break;
                        case 3:
                            chkWednesday.Checked = true;
                            break;
                        case 4:
                            chkThursday.Checked = true;
                            break;
                        case 5:
                            chkFriday.Checked = true;
                            break;
                        case 6:
                            chkSaturday.Checked = true;
                            break;
                    }
                }
            }
        }

        public Control getControl()
        {
            return this;
        }

        private void txtRecur_Validating(object sender, CancelEventArgs e)
        {
            ousrWeeklyRow.Days = Convert.ToInt32(txtRecur.Value);
        }

        public void save(int iScheduledTasksID)
        {
            if (ousrWeeklyData != null && ousrWeeklyData.Tables.Count > 0 && ousrWeeklyData.ScheduledTasksDetailWeek.Rows.Count > 0)
            {
                ousrWeeklyData.ScheduledTasksDetailWeek.Rows[0][clsPOSDBConstants.ScheduledTasksDetailWeek_Fld_ScheduledTasksID] = iScheduledTasksID;
                ousrWeeklyBL.Insert(ousrWeeklyData);
            }
        }

        public void Insert(int iScheduledTasksID)
        {
            ousrWeeklyRow.ScheduledTasksID = iScheduledTasksID;
            ousrWeeklyBL.Persist(ousrWeeklyData);
            //ContScheduledTasksDetailWeek oContScheduledTasksDetailWeek = new ContScheduledTasksDetailWeek();
            //ousrWeeklyBL.Save(oScheduledTasksDetailWeek);
        }                
    }
}