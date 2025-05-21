using System;
using Infragistics.Win.UltraWinGrid;
using Infragistics.Win;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using POS_Core.CommonData;
using POS_Core.BusinessRules;
using POS_Core.ErrorLogging;
using POS_Core.CommonData.Rows;
using Infragistics.Win.UltraWinMaskedEdit;
using Infragistics.Win.CalcEngine;
using Infragistics.Win.UltraWinCalcManager;
//using POS_Core.DataAccess;
using System.Data;
using System.Reflection;
using POS_Core.UserManagement;
using System.Threading;
using PharmData;
using POS_Core.DataAccess;
//using POS_Core_UI.Reports.ReportsUI;


namespace POS_Core_UI.Reports.ReportsUI
{
    public partial class frmRptTransactionTime : Form
    {
        public frmRptTransactionTime()
        {
            InitializeComponent();
        }

        private void frmRptTransactionTime_Load(object sender, EventArgs e)
        {
            clsUIHelper.setColorSchecme(this);
            this.dtpFromDate.Enter += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
            this.dtpFromDate.Leave += new System.EventHandler(clsUIHelper.AfterExitEditMode);

            this.dtpToDate.Enter += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
            this.dtpToDate.Leave += new System.EventHandler(clsUIHelper.AfterExitEditMode);

            this.cboTransType.AfterEnterEditMode += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
            this.cboTransType.AfterExitEditMode += new System.EventHandler(clsUIHelper.AfterExitEditMode);

            this.cboUsers.AfterEnterEditMode += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
            this.cboUsers.AfterExitEditMode += new System.EventHandler(clsUIHelper.AfterExitEditMode);

            dtpFromDate.Value = DateTime.Now;
            dtpToDate.Value = DateTime.Now;
            dtpFromDate.Text = DateTime.Now.Date.ToString();
            dtpToDate.Text = DateTime.Now.Date.ToString();
            cboUsers.Items.Add("ALL");
            cboTransType.SelectedItem = cboTransType.Items[0];
            cboUsers.SelectedItem = cboUsers.Items[0];
            PopulateUsers();
            FillStationIDs();
            this.BringToFront();
        }

        public void PopulateUsers()
        {
            SearchSvr oSearchSvr = new SearchSvr();
            DataSet UserDS= oSearchSvr.Search(clsPOSDBConstants.Users_tbl, "", "", 0, -1);
            foreach (DataRow row in UserDS.Tables[0].Rows)
            {
                cboUsers.Items.Add(row["UserId"].ToString());
            }
        }

        public int GetTransType()
        {
            try
            {
                switch (cboTransType.Text)
                {
                    case "All":
                        return 0;
                        break;
                    case "Sales":
                        return 1;
                        break;
                    case "Return":
                        return 2;
                        break;
                }
                return 0;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
        private bool validateDate()
        {
            int result=DateTime.Compare((DateTime)dtpFromDate.Value,(DateTime)dtpToDate.Value);
            if(result==1)
            {
                clsUIHelper.ShowErrorMsg("Start Date Cannot be Greater than End Date");
                return false;
            }
            else
                return true;
        }

        private void btnView_Click(object sender, EventArgs e)
        {
            if(validateDate())
            Preview(false);
        }

        private void Preview(bool flgPrint)
        {
            try
            {
                this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
                POS_Core_UI.Reports.Reports.rptTransactionTime orptTranTime = new POS_Core_UI.Reports.Reports.rptTransactionTime();

                TransDetailSvr oTransDetailSvr = new TransDetailSvr();
                TransDetailData oTransData = null;
                TransDetailRow oTransDtRow = null;
                string StartDate = dtpFromDate.Value.ToString();//.Text;
                DateTime dt = DateTime.SpecifyKind((DateTime)dtpToDate.Value, DateTimeKind.Local);
                dt = dt.AddDays(1);
                string EndDate = dt.ToString();
                string sWhereClause = " where pt.TransID=pdt.TransID AND pt.TransDate Between'" + StartDate + "' and '" + EndDate + "'";
                int TransType = GetTransType();

                if (TransType != 0)
                    sWhereClause = String.Concat(sWhereClause, " AND pt.TransType='" + TransType + "'");
                if (cboUsers.SelectedItem.ToString() != "" && cboUsers.SelectedItem.ToString() != "ALL")
                    sWhereClause = String.Concat(sWhereClause, " And pt.UserId='" + cboUsers.SelectedItem.ToString() + "'");

                //Following Code Added by Krishna on 21 November 2011
                if (cboStnId.SelectedItem.ToString() != "ALL" && cboStnId.SelectedItem.ToString() != "")
                {
                    sWhereClause = String.Concat(sWhereClause, " AND pt.StationID=" + cboStnId.SelectedItem.ToString() + "");
                }
                if (txtFromRange.Text!="" && txtToRange.Text!="")
                {
                    if (Convert.ToInt32(txtFromRange.Text) > Convert.ToInt32(txtToRange.Text))
                        throw new Exception("From range cannot be greater than To range. ");
                    sWhereClause = String.Concat(sWhereClause, " And (DATEDIFF(S,pt.TransactionStartDate,pt.TransDate) between "+txtFromRange.Text+" AND "+txtToRange.Text+" )");
                }
                //Till here Added by Krishna on 21 November

                DataSet ds = oTransDetailSvr.PopulatePOSTrans(sWhereClause);

                clsReports.setCRTextObjectText("TransType", "Transaction Type: " + cboTransType.SelectedItem.ToString(), orptTranTime);
                clsReports.setCRTextObjectText("StartDate", dtpFromDate.Value.ToString(), orptTranTime);
                clsReports.setCRTextObjectText("EndDate", dtpToDate.Value.ToString(), orptTranTime);
                
                clsReports.Preview(flgPrint, ds, orptTranTime);

                this.Cursor = System.Windows.Forms.Cursors.Default;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void frmRptTransactionTime_KeyDown(object sender, KeyEventArgs e)
        {

            try
            {
                if (e.KeyData == System.Windows.Forms.Keys.Enter)
                {
                    this.SelectNextControl(this.ActiveControl, true, true, true, true);
                }
                else if (e.KeyData == Keys.Escape)
                    this.Close();
                
            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
            #region Commented
            //switch(e.KeyData)
            //{
            //    case System.Windows.Forms.Keys.Enter:
            //        this.SelectNextControl(this.ActiveControl, true, true, true, true);
            //        break;
            //    case Keys.V:
            //        btnView_Click(null, null);
            //        break;
            //    case Keys.P:
            //        btnPrint_Click(null, null);
            //        break;
            //    case Keys.Escape:
            //        this.Close();
            //        break;        
            //    case Keys.C:
            //        this.Close();
            //        break;
            //}
            #endregion
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (validateDate())
            Preview(true);
        }
        //Following Code Added by Krishna on 21 November 2011
        private void FillStationIDs()
        {
            try
            {
                DataSet oStationDs = new DataSet();
                SearchSvr oSearchSvr = new SearchSvr();
                string sSQL = "Select Distinct(StationID) From Util_POSSET";

                oStationDs.Tables.Add(oSearchSvr.Search(sSQL).Tables[0].Copy());
                oStationDs.Tables[0].TableName = "STATION";

                this.cboStnId.Items.Add("ALL");

                foreach (DataRow stationRow in oStationDs.Tables[0].Rows)
                {
                    this.cboStnId.Items.Add(stationRow["StationID"].ToString());
                }
                this.cboStnId.SelectedIndex = 0;
            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }
        //Till here Added by Krishna 

    }
}