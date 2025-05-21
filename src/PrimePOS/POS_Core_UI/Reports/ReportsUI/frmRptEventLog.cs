using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
//using POS.UI;
using POS_Core.CommonData;
using POS_Core_UI.Reports.Reports;
//using POS_Core.DataAccess;
using System.Data.SqlClient;
using POS_Core.ErrorLogging;
using Resources;
using POS_Core.Resources;

namespace POS_Core_UI.Reports.ReportsUI
{
    public partial class frmRptEventLog : Form
    {
        public frmRptEventLog()
        {
            InitializeComponent();
        }

        private void txtUserId_EditorButtonClick(object sender, Infragistics.Win.UltraWinEditors.EditorButtonEventArgs e)
        {
            try
            {
                //frmSearch oSearch = new frmSearch(clsPOSDBConstants.Users_tbl);
                frmSearchMain oSearch = new frmSearchMain(true);    //20-Dec-2017 JY Added new reference        
                oSearch.SearchTable = clsPOSDBConstants.Users_tbl;  //20-Dec-2017 JY Added 
                oSearch.ShowDialog();
                if (oSearch.IsCanceled) return;
                //txtUserId.Text = oSearch.SelectedRowID();
            }
            catch (Exception Ex)
            {
            }
        }

        private void dtpFromDate_ValueChanged(object sender, EventArgs e)
        {
            string fieldName = string.Empty;
            try
            {
                if (!validateFields(out fieldName))
                {
                    if (fieldName == "DATE")
                    {
                        clsUIHelper.ShowErrorMsg("From Date can not be greater than To Date.");
                        dtpFromDate.Value = DateTime.Now.Date;
                    }

                    return;
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void dtpFromDate_KeyDown(object sender, KeyEventArgs e)
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

        private void dtpToDate_KeyDown(object sender, KeyEventArgs e)
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

        private void dtpToDate_ValueChanged(object sender, EventArgs e)
        {
            string fieldName = string.Empty;
            try
            {
                if (!validateFields(out fieldName))
                {
                    if (fieldName == "DATE")
                    {
                        clsUIHelper.ShowErrorMsg("From Date can not be greater than To Date.");
                        dtpToDate.Value = DateTime.Now.Date;
                    }

                    return;
                }
            }
            catch (Exception ex)
            {
            }
        }

        private bool validateFields(out string fieldName)
        {
            bool isValid = true;
            string field = string.Empty;
            try
            {
                if ((DateTime)dtpFromDate.Value > (DateTime)dtpToDate.Value)
                {
                    isValid = false;
                    fieldName = "DATE";
                    return isValid;
                }
            }
            catch (Exception ex)
            {
                clsUIHelper.ShowErrorMsg(ex.Message);
            }
            fieldName = field;
            return isValid;
        }

        private void txtUserId_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyData == System.Windows.Forms.Keys.Enter)
                {
                    this.SelectNextControl(this.ActiveControl, true, true, true, true);
                }
                else if (e.KeyData == System.Windows.Forms.Keys.F4)
                {
                    this.txtUserId_EditorButtonClick(null, null);
                }
            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }

        private void frmRptEventLog_Shown(object sender, EventArgs e)
        {
            try
            {
                clsUIHelper.setColorSchecme(this);
                dtpToDate.Value = DateTime.Now.Date;
                dtpFromDate.Value = DateTime.Now.Date;
                FillUserCombo();
                foreach (string item in Enum.GetNames(typeof(LogENUM)))
                {
                    string[] sSplitedStr = item.Split("_".ToCharArray());
                    string sItem = string.Empty;
                    if (sSplitedStr.Length == 2)
                    {
                        sItem = sSplitedStr[0].ToString() + " " + sSplitedStr[1].ToString();
                    }
                    else
                    {
                        sItem = sSplitedStr[0].ToString(); 
                    }
                    int value = (int)Enum.Parse(typeof(LogENUM), item);
                    cmbEvents.Items.Add(sItem);
                }
                //cmbEvents.DataSource = System.Enum.GetValues(typeof(LogENUM));
                cmbEvents.Text = "";
            }
            catch (Exception Ex)
            {
            }
        }

        private void FillUserCombo()
        {
            IDbCommand cmd = DataFactory.CreateCommand();
			 string sSQL = "";
			
			 DataSet ds = new DataSet();
			 IDataAdapter da = DataFactory.CreateDataAdapter();
			 IDbConnection conn = DataFactory.CreateConnection();
			 conn.ConnectionString = Configuration.ConnectionString;			
			 conn.Open();
             try
             {
                 sSQL = "SELECT * FROM Users";
                 cmd.CommandType = CommandType.Text;
                 cmd.CommandText = sSQL;
                 cmd.Connection = conn;
                 SqlDataAdapter sqlDa = (SqlDataAdapter)da;
                 sqlDa.SelectCommand = (SqlCommand)cmd;
                 da.Fill(ds);
                 conn.Close();
                 if (ds != null && ds.Tables[0].Rows.Count > 0)
                 {
                     cmbUsers.DataSource = ds.Tables[0];
                     cmbUsers.DisplayMember = ds.Tables[0].Columns[clsPOSDBConstants.Users_Fld_UserID].ColumnName;
                 }
                 cmbUsers.Text = "";
             }
             catch (Exception exp)
             {
                 clsUIHelper.ShowErrorMsg(exp.Message);
                 conn.Close();
             }
        }

        private void frmRptEventLog_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyData == System.Windows.Forms.Keys.F4)
                {
                    //if (txtUserId.ContainsFocus)
                    //    txtUserId_EditorButtonClick(null, null);
                }
            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }

        private void frmRptEventLog_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Escape)
                {
                    this.Close();
                }
            }
            catch (Exception Ex)
            {
            }
        }

        private void btnViewReport_Click(object sender, EventArgs e)
        {
            try
            {
                Preview(false);
            }
            catch (Exception Ex)
            {
            }
        }

        private void Preview(bool PrintId)
        {
            try
            {
                string rptTitle = "Event Log";
                string sSQL = string.Empty;
                string sGrouping = string.Empty;
                rptEventLog oRpt = new rptEventLog();

                sSQL = GetSelectQuery();

                if (rdbByDate.Checked)
                {
                    sGrouping = "By Date";
                }
                else if (rdbByEvent.Checked)
                {
                    sGrouping = "By Event";
                }

                clsReports.setCRTextObjectText("txtFromDate", "From :" + this.dtpFromDate.Text, oRpt);
                clsReports.setCRTextObjectText("txtToDate", "To :" + this.dtpToDate.Text, oRpt);
                //clsReports.setCRTextObjectText("txtRptTitle", rptTitle, oRpt);
                oRpt.Database.Tables[0].SetDataSource(clsReports.GetReportSource(sSQL).Tables[0]);
                clsReports.SetRepParam(oRpt, "Group", sGrouping);
                clsReports.DStoExport = clsReports.GetReportSource(sSQL); //PRIMEPOS-2471 16-Feb-2021 JY Added
                clsReports.Preview(PrintId, oRpt);
                ErrorHandler.SaveLog((int)LogENUM.View_Report, Configuration.UserName, "Success", "Event Log report viewed successfully");
            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }

        private string GetSelectQuery()
        {
            string sSQL = string.Empty;
            try
            {
                sSQL = "Select ID,Event=case Event when 1 then 'ApplicationStart' when 2 then 'ApplicationClose' when 3 then 'Login' when 4 then 'SettingsChange' when 5 then 'UserRightsChange' when 6 then 'ViewReport' End, LogDate,UserID,LogResult,LogMessage from Log where " + buildCriteria() + " order by ID asc";
            }
            catch (Exception Ex)
            {
            }
            return sSQL;
        }

        private string buildCriteria()
        {
            string sCriteria = "";
            int eventId =0;
            if (cmbEvents.Text.ToString() != string.Empty)
                eventId = (int)cmbEvents.SelectedIndex+1;//;SelectedValue;
           
            
            
            try
            {
                if (dtpFromDate.Value.ToString() != "")
                    sCriteria = sCriteria + " Convert(smalldatetime,Convert(Varchar,LogDate,107)) >= '" + dtpFromDate.Text + "' AND";
                if (dtpToDate.Value.ToString() != "")
                    sCriteria = sCriteria + " Convert(smalldatetime,Convert(Varchar,LogDate,107)) <= '" + dtpToDate.Text + "' AND";
                if (cmbUsers.Text.Trim().Replace("'", "''") != "")
                    sCriteria = sCriteria + " UserID = '" + cmbUsers.Text + "' AND";
                if (eventId != 0)
                {
                    if (cmbEvents.Text.Trim().Replace("'", "''") != "")
                        sCriteria = sCriteria + " Event = " + eventId + "";
                }
            }
            catch (Exception Ex)
            {
            }
            if (sCriteria.EndsWith("AND"))
                sCriteria = sCriteria.Remove(sCriteria.Length - 3, 3);
            return sCriteria;
        }

        private void btnPrintReport_Click(object sender, EventArgs e)
        {
            try
            {
                Preview(true);
            }
            catch (Exception Ex)
            {
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            try
            {
                this.Close();
            }
            catch (Exception Ex)
            {
            }
        }
    }
}