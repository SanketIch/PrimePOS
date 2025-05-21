using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace POS_Core_UI
{
    public partial class usrDateRangeParams : UserControl, IBaseControls
    {
        //public ContScheduledTasksControls oContScheduledTasksControls = new ContScheduledTasksControls();
        //public List<ScheduledTasksControls> oScheduledTasksControlsList = new List<ScheduledTasksControls>();

        public usrDateRangeParams()
        {
            InitializeComponent();
        }

        public void setDateTimeControl()
        {
            dtFromDate.Visible = false;
            dtToDate.Visible = false;
            cusdtFromDate.Visible = true;
            cusdtToDate.Visible = true;
        }

        public void getControlsValues(ref DataTable dt)
        {
            DataRow dr = dt.NewRow();

            dr["ControlsName"] = dtFromDate.Tag;
            dr["ControlsValue"] = cusdtFromDate.DateSelected; //DateTime.Today.Subtract((DateTime)dtFromDate.Value).Days;
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["ControlsName"] = dtToDate.Tag;
            dr["ControlsValue"] = cusdtToDate.DateSelected; // DateTime.Today.Subtract((DateTime)dtToDate.Value).Days;
            dt.Rows.Add(dr);

            dt.AcceptChanges();
        }

        public void setControlsValues(ref DataTable dt)
        {
            double Num;

            foreach (DataRow odr in dt.Select("ControlsName = '" + this.dtFromDate.Tag + " ' "))
            {
                if (double.TryParse(odr["ControlsValue"].ToString().Trim(), out Num))
                {
                    dtFromDate.Value = DateTime.Now.AddDays(Convert.ToDouble(odr["ControlsValue"].ToString().Trim()) * -1);
                }
                else
                {
                    dtFromDate.Value = odr["ControlsValue"].ToString().Trim();
                }
                cusdtFromDate.DateSelected = odr["ControlsValue"].ToString().Trim();
            }

            foreach (DataRow odr in dt.Select("ControlsName = '" + this.dtToDate.Tag + "' "))
            {
                if (double.TryParse(odr["ControlsValue"].ToString().Trim(), out Num))
                {
                    dtToDate.Value = DateTime.Now.AddDays(Convert.ToDouble(odr["ControlsValue"].ToString().Trim()) * -1);
                }
                else
                {
                    dtToDate.Value = odr["ControlsValue"].ToString().Trim();
                }
                cusdtToDate.DateSelected = odr["ControlsValue"].ToString().Trim();
            }
        }

        private void usrDailyLogReport_Load(object sender, EventArgs e)
        {
            fillControls();
        }

        public void fillControls()
        {
            try
            {
                if (!calledFromCommLine) this.dtFromDate.Value = DateTime.Today;
                if (!calledFromCommLine) this.dtToDate.Value = DateTime.Today;
            }
            catch { }
        }

        private void cusdtFromDate_Validating(object sender, CancelEventArgs e)
        {
            //if (CheckDateTimeValidation() == false)
            //    e.Cancel = true;
            //else
            //    errorProvider.Clear();
        }

        private void cusdtToDate_Validating(object sender, CancelEventArgs e)
        {
            //if (CheckDateTimeValidation() == false)
            //    e.Cancel = true;
            //else
            //    errorProvider.Clear();
        }

        public bool CheckDateTimeValidation()
        {
            DateTime tempFromDate;
            DateTime tempToDate;
            double Num;
            if (Double.TryParse(cusdtFromDate.DateSelected, out Num))
                tempFromDate = DateTime.Today.AddDays(Num * -1);
            else
                tempFromDate = Convert.ToDateTime(cusdtFromDate.DateSelected);

            if (Double.TryParse(cusdtToDate.DateSelected, out Num))
                tempToDate = DateTime.Today.AddDays(Num * -1);
            else
                tempToDate = Convert.ToDateTime(cusdtToDate.DateSelected);

            if (tempFromDate > tempToDate)
            {
                errorProvider.SetError(cusdtFromDate, "To Date Must be Greater Then Or Equal to From Date");
                return false;
            }
            else
            {
                errorProvider.Clear();
                return true;
            }
        }
    }
}
