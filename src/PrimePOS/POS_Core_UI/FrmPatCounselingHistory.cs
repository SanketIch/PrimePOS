using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Interop;
using Infragistics.Documents.Reports.Report.Preferences.PDF;
using Infragistics.Win;
using MMS.GlobalPayments.Api.Utils;
using NLog;
using PharmData;
using POS_Core.BusinessRules;
using POS_Core.Resources;

namespace Phw
{
    public partial class FrmPatCounselingHistory : Form
    {
        private static ILogger logger = LogManager.GetCurrentClassLogger();

        public RXHeader oRXHeader { get; set; }
        public DataTable tblCounselRules { get; set; }
        public DataTable tblCounselRXs { get; set; }
        private bool m_PharmacistRequired = false;
        private string m_Status = "N";
        PharmBL oPharmBL = new PharmBL();
        private string m_PatientName = string.Empty;
        private int m_PatientNo = 0;
        private bool m_bFromPOS = true;
        private DataTable m_TblRXs;
        private DataTable m_TblCounselHist;
        DataSet dsUsers;

        public FrmPatCounselingHistory()
        {
            InitializeComponent();
            m_TblRXs = null;
            m_TblCounselHist = null;
            dsUsers = null;
        }

        private void FrmPatCounselingHistory_Load(object sender, EventArgs e)
        {
            if(oRXHeader != null)
            {
                m_PatientName = oRXHeader.PatientName;
                m_PatientNo = Convert.ToInt32(oRXHeader.PatientNo);

                if (tblCounselRXs != null)
                    m_TblRXs = tblCounselRXs.Copy();
                else
                {
                    m_TblRXs = new DataTable("Patient_RXs");
                    m_TblRXs.Columns.Add("RxNo", typeof(long));
                    m_TblRXs.Columns.Add("RefillNo", typeof(string));
                    m_TblRXs.Columns.Add("Drug Name", typeof(string));
                    for (int i = 0; i < oRXHeader.RXDetails.Count; i++)
                    {
                        DataRow dr = m_TblRXs.NewRow();
                        dr[0] = oRXHeader.RXDetails[i].RXNo;
                        dr[1] = oRXHeader.RXDetails[i].RefillNo.ToString();
                        dr[2] = oRXHeader.RXDetails[i].DrugName.Trim();
                        m_TblRXs.Rows.Add(dr);
                    }
                }
            }

            txtPatientName.Text = m_PatientName;
            comEdStatus.Value = m_Status;
            gridRXs.DataSource = m_TblRXs;

            InitCounselorDropdown();

            m_TblCounselHist = oPharmBL.LoadPatientCounselHistory(m_PatientNo);
            gridPatCounselHist.DataSource = m_TblCounselHist;

            if (tblCounselRules != null && tblCounselRules.Rows.Count != 0)
            {
                m_PharmacistRequired = Configuration.convertNullToBoolean(tblCounselRules.Rows[0]["CounselByPharmacistOnly"]);

                string strState = "ALL,  ";
                if (!string.IsNullOrWhiteSpace(tblCounselRules.Rows[0]["State"].ToString()))
                {
                    strState = tblCounselRules.Rows[0]["State"].ToString() + ",  ";
                }
                string strINS = "ALL,  ";
                if (!string.IsNullOrWhiteSpace(tblCounselRules.Rows[0]["Insurance"].ToString()))
                {
                    strINS = tblCounselRules.Rows[0]["Insurance"].ToString() + ",  ";
                    if (strINS.Length > 40)
                        strINS = strINS.Substring(0, 37) + "...";
                }
                string strRxType = "New RX or REFILL,  ";
                if (!string.IsNullOrWhiteSpace(tblCounselRules.Rows[0]["RxType"].ToString()))
                {
                    if (tblCounselRules.Rows[0]["RxType"].ToString().Contains("O") && !tblCounselRules.Rows[0]["RxType"].ToString().Contains("R"))
                        strRxType = "New RX,  ";
                    else if (!tblCounselRules.Rows[0]["RxType"].ToString().Contains("O") && tblCounselRules.Rows[0]["RxType"].ToString().Contains("R"))
                        strRxType = "REFILL,  ";
                }

                int iCycleDays = Configuration.convertNullToInt(tblCounselRules.Rows[0]["CounselingRenewalCycle"]);
                string strCycle = "Counseling Renewal Cycle #Days = " + iCycleDays.ToString() + ",  ";
                string strPhReq = "Offer Only by Pharmacist = FALSE";
                if (m_PharmacistRequired)
                    strPhReq = "Offer Only by Pharmacist = TRUE";

                string strRules = string.Empty;
                strRules += "State = " + strState + "Rx Type = " + strRxType + "Insurance = " + strINS + Environment.NewLine;
                strRules += strCycle + strPhReq;

                lblRules.Text = strRules;

                if(tblCounselRules.Rows[0]["AllowToQueueUp"]!=DBNull.Value && Configuration.convertNullToBoolean(tblCounselRules.Rows[0]["AllowToQueueUp"])==true )
                    btnAddQueue.Enabled = true;
                else
                    btnAddQueue.Enabled = false;
            }
        }

        private void InitCounselorDropdown()
        {
            string sSql = " SELECT RTRIM(PH_INIT)+' / '+ RTRIM(PH_NAME) +' ('+ UserRole +')'  as Counselor, RTRIM(PH_INIT) as PH_INIT, PASSWORD from PHUSER where ISNULL(status,'A')<>'I' order by PH_INIT ";
            
            dsUsers = oPharmBL.GetPhUsers(sSql);
            if (dsUsers != null && dsUsers.Tables.Count > 0)
            {
                foreach (DataRow dr1 in dsUsers.Tables[0].Rows)
                {
                    ValueListItem valueListItem1 = new ValueListItem();
                    valueListItem1.DataValue = dr1["PH_INIT"].ToString();
                    valueListItem1.DisplayText = dr1["Counselor"].ToString();
                    combCounselUser.Items.Add(valueListItem1);
                }
                
                if (oRXHeader != null && oRXHeader.RXDetails.Count>0)
                {
                    DataTable dtClaims = oRXHeader.RXDetails[0].TblClaims;
                    if (dtClaims != null && dtClaims.Rows.Count > 0)
                    {
                        if (dtClaims.Columns.Contains("PHARMACIST"))
                            combCounselUser.Value = dtClaims.Rows[0]["PHARMACIST"].ToString().Trim();
                    }
                }
            }
        }

        private void btnAddQueue_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            DataTable dtSaveRXs = m_TblRXs.Copy();
            DataTable dtSaveHist = GetPatCounselHistTable(false);
            
            string msg = "RX(s) ";
            for (int row=dtSaveRXs.Rows.Count-1; row>=0; row--)
            {
                long lRxNo = Convert.ToInt64(dtSaveRXs.Rows[row][0].ToString());
                int iRefillNo = Convert.ToInt32(dtSaveRXs.Rows[row][1].ToString());

                foreach(DataRow dr in m_TblCounselHist.Rows)
                {
                    if (Convert.ToInt64(dr["RxNo"].ToString()) == lRxNo && Convert.ToInt32(dr["RefillNo"].ToString())==iRefillNo
                        && Configuration.convertNullToBoolean(dr["isCounsCompleted"])==false && dr["AddedToQueueDate"] !=DBNull.Value 
                        && Convert.ToDateTime(dr["AddedToQueueDate"].ToString()).Date == DateTime.Now.Date )
                    {
                        msg += lRxNo.ToString() + "-" + iRefillNo.ToString() + ",";
                        dtSaveRXs.Rows[row].Delete();
                        break;
                    }
                }
            }
            
            dtSaveRXs.AcceptChanges();
            if(dtSaveRXs.Rows.Count==0)
            {
                string strRXs = msg.Substring(0, msg.Length-1);
                msg = strRXs + " were queued up for counseling already.\n";
                MessageBox.Show(msg, "Patient Counseling", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.DialogResult = DialogResult.OK;
                this.Close();
                return;
            }

            try
            {
                bool rtn = oPharmBL.SavePatCounselHistoryToDB(dtSaveHist, dtSaveRXs);

                if (rtn)
                {
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
            }
            catch (Exception exp)
            {
                msg = "Failed to add 'Patient Counseling' to queue.\n";
                MessageBox.Show(msg + exp.Message, "Patient Counseling", MessageBoxButtons.OK, MessageBoxIcon.Information);
                logger.Fatal(exp.ToString(), msg+"btnAddQueue_Click()");
            }
            this.Cursor = Cursors.Default;
        }

        private void btnComplete_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(combCounselUser.Text))
            {
                MessageBox.Show("Please provide Counselor first.", "Patient Counseling", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (m_PharmacistRequired==true)
            {
                string sPWInput = txtPassword.Text;
                string sCounselor = combCounselUser.Text;
                if (!sCounselor.Contains("(P)"))
                {
                    MessageBox.Show("Counselor has to be Pharmacist.", "Patient Counseling", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                if (string.IsNullOrWhiteSpace(txtPassword.Text))
                {
                    MessageBox.Show("Please provide Counselor's PASSWORD first.", "Patient Counseling", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                string sPhUser = combCounselUser.Value.ToString().Trim();
                string sPasswordSaved = string.Empty;
                if (dsUsers != null && dsUsers.Tables.Count > 0)
                {
                    foreach (DataRow dr in dsUsers.Tables[0].Rows)
                    {
                        if (dr["PH_INIT"].ToString() == sPhUser)
                        {
                            sPasswordSaved = Configuration.oCryption.NativeDecrypt(dr["PASSWORD"].ToString());
                            break;
                        }
                    }
                }

                if (sPWInput != sPasswordSaved)
                {
                    MessageBox.Show("PrimeRx PASSWORD validation is needed to continue.", "Patient Counseling", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
            }

            this.Cursor = Cursors.WaitCursor;
            DataTable dtSaveRXs = m_TblRXs.Copy();
            DataTable dtSaveHist = GetPatCounselHistTable(true);

            try
            {
                bool rtn = oPharmBL.SavePatCounselHistoryToDB(dtSaveHist, dtSaveRXs);

                if (rtn)
                {
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
            }
            catch (Exception exp)
            {
                string msg = "Failed to complete Patient Counseling.\n";
                MessageBox.Show(msg + exp.Message, "Patient Counseling", MessageBoxButtons.OK, MessageBoxIcon.Information);
                logger.Fatal(exp.ToString(), msg+"btnComplete_Click()");
            }

            this.Cursor = Cursors.Default;
        }
        
        private void btnDecline_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(combCounselUser.Text))
            {
                MessageBox.Show("Please provide Counselor first.", "Patient Counseling", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (m_PharmacistRequired == true)
            {
                string sPWInput = txtPassword.Text;
                string sCounselor = combCounselUser.Text;
                if (!sCounselor.Contains("(P)"))
                {
                    MessageBox.Show("Counselor has to be Pharmacist.", "Patient Counseling", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                if (string.IsNullOrWhiteSpace(txtPassword.Text))
                {
                    MessageBox.Show("Please provide Counselor's PASSWORD first.", "Patient Counseling", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                string sPhUser = combCounselUser.Value.ToString().Trim();
                string sPasswordSaved = string.Empty;
                if (dsUsers != null && dsUsers.Tables.Count > 0)
                {
                    foreach (DataRow dr in dsUsers.Tables[0].Rows)
                    {
                        if (dr["PH_INIT"].ToString() == sPhUser)
                        {
                            sPasswordSaved = Configuration.oCryption.NativeDecrypt(dr["PASSWORD"].ToString());
                            break;
                        }
                    }
                }

                if (sPWInput != sPasswordSaved)
                {
                    MessageBox.Show("PrimeRx PASSWORD validation is needed to continue.", "Patient Counseling", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
            }

            this.Cursor = Cursors.WaitCursor;
            DataTable dtSaveRXs = m_TblRXs.Copy();
            DataTable dtSaveHist = GetPatCounselHistTable(true, "Y");

            try
            {
                bool rtn = oPharmBL.SavePatCounselHistoryToDB(dtSaveHist, dtSaveRXs);

                if (rtn)
                {
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
            }
            catch (Exception exp)
            {
                string msg = "Failed to complete counseling when patient declined.\n";
                MessageBox.Show(msg + exp.Message, "Patient Counseling", MessageBoxButtons.OK, MessageBoxIcon.Information);
                logger.Fatal(exp.ToString(), msg+"btnDecline_Click()");
            }

            this.Cursor = Cursors.Default;
        }

        private DataTable GetPatCounselHistTable(bool bComplete, string IsDecline="N")
        {
            DataTable dtSaveHist = m_TblCounselHist.Clone();
            DataRow dr = dtSaveHist.NewRow();

            dr["CounsID"] = 0;
            dr["PatientNo"] = m_PatientNo;
            if (bComplete)
            {
                dr["isCounsCompleted"] = true;
                if (IsDecline == "N")
                    dr["CounselingResult"] = "Completed";
                else
                    dr["CounselingResult"] = "Declined";
                dr["CounsCompletedDate"] = DateTime.Now;
            }
            else
            {
                dr["isCounsCompleted"] = false;
                dr["CounselingResult"] = "Pending";
                dr["AddedToQueueDate"] = DateTime.Now;
            }
            dr["Remark"] = tedRemark.Text.Trim();
            dr["CounseledByUser"] = combCounselUser.Value.ToString();
            dr["RecordAddedByUser"] = "POS User=" + POS_Core.Resources.Configuration.UserName;
            if (rbInPerson.Checked)
                dr["ConductMethod"] = "In Person";
            if (rbByCalling.Checked)
                dr["ConductMethod"] = "By Calling";
            if (rbOther.Checked)
                dr["ConductMethod"] = "Other";

            dtSaveHist.Rows.Add(dr);
            dtSaveHist.AcceptChanges();
            return dtSaveHist;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void gridPatCounselHist_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
        {
            e.Layout.Bands[0].Columns["PatientNo"].Hidden = true;
            e.Layout.Bands[0].Columns["CounsID"].Header.Caption = "ID";
            e.Layout.Bands[0].Columns["CounsID"].Width = 35;
            e.Layout.Bands[0].Columns["RxNo"].Width = 60;
            e.Layout.Bands[0].Columns["RefillNo"].Header.Caption = "Refill#";
            e.Layout.Bands[0].Columns["RefillNo"].Width = 40;
            e.Layout.Bands[0].Columns["isCounsCompleted"].Header.Caption = "Completed?";
            e.Layout.Bands[0].Columns["isCounsCompleted"].Width = 70;
            e.Layout.Bands[0].Columns["CounselingResult"].Header.Caption = "Result";
            e.Layout.Bands[0].Columns["CounselingResult"].Width = 60;
            e.Layout.Bands[0].Columns["CounsCompletedDate"].Header.Caption = "Comp. Date";
            e.Layout.Bands[0].Columns["CounsCompletedDate"].Width = 70;
            e.Layout.Bands[0].Columns["AddedToQueueDate"].Header.Caption = "Que. Added On";
            e.Layout.Bands[0].Columns["AddedToQueueDate"].Width = 85;
            e.Layout.Bands[0].Columns["CounseledByUser"].Header.Caption = "Counselor";
            e.Layout.Bands[0].Columns["CounseledByUser"].Width = 60;
            e.Layout.Bands[0].Columns["RecordAddedByUser"].Header.Caption = "Recorded By";
            e.Layout.Bands[0].Columns["Remark"].Width = 150;
        }

        private void gridPatCounselHist_InitializeRow(object sender, Infragistics.Win.UltraWinGrid.InitializeRowEventArgs e)
        {

        }

        private void gridRXs_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
        {
            e.Layout.Bands[0].Columns["RxNo"].Width = 48;
            e.Layout.Bands[0].Columns["RefillNo"].Width = 40;
            e.Layout.Bands[0].Columns["RefillNo"].Header.Caption = "Refill#";
            e.Layout.Bands[0].Columns["Drug Name"].Width = 170;
        }
    }
}
