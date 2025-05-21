using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using DPUruNet;
using System.Drawing.Imaging;
using System.Threading;
using POS_Core.Resources;
//using POS_Core.DataAccess;

namespace POS_Core_UI.UserManagement
{
    public partial class frmEnrollFingerPrint : Form
    {
        private DataTable dtUser=null;
        private string mUserID = string.Empty;
        private bool IsReadyForSaving = false;
        private bool IsFPReaderConnected = false;
        private FingerPrintReader fpReader = null;
        private Fmd enrollmentFinger = null;
        public bool HasChangedEnrollment { get; set; }
        private Color origBgColor = Color.LightBlue;
        private FingerPrintData objFinger1 = null;
        private FingerPrintData objFinger2 = null;
        DBUser oDBUser = new DBUser();
        
        public frmEnrollFingerPrint(string UserID)
        {
            mUserID = UserID;
            InitializeComponent();
            frmEnrollFingerPrint_LoadData();
            frmEnrollFingerPrint_SetScreen();
        }

        public int frmEnrollFingerprint_GetNumOfEnrollment()
        {
            int numEnrollment = 0;

            if (objFinger1 != null && objFinger1.HasSaved)
                numEnrollment++;
            if (objFinger2 != null && objFinger2.HasSaved)
                numEnrollment++;

            return numEnrollment;
        }

        private void frmEnrollFingerPrint_LoadData()
        {            
            dtUser = oDBUser.GetUserDetails(mUserID);   //get user information

            objFinger1 = new FingerPrintData();
            objFinger1.User = mUserID;
            objFinger1.HasSaved = false;
            objFinger1.FingerIndex = "RI";
            objFinger2 = new FingerPrintData();
            objFinger2.User = mUserID;
            objFinger2.HasSaved = false;
            objFinger2.FingerIndex = "LI";

            DataTable fpTable = oDBUser.LoadUserFingerPrint(mUserID);
            if (fpTable != null && fpTable.Rows.Count >= 1)
            {
                objFinger1.HasSaved = true;
                objFinger1.FingerIndex = fpTable.Rows[0]["FingerIndex"].ToString();

                if(fpTable.Rows.Count >= 2)
                {
                    objFinger2.HasSaved = true;
                    objFinger2.FingerIndex = fpTable.Rows[1]["FingerIndex"].ToString();
                }
            }
            HasChangedEnrollment = false;
        }

        private void frmEnrollFingerPrint_SetScreen()
        {
            txtUser.Text = mUserID;
            txtUserName.Text = dtUser.Rows[0]["UserName"].ToString();

            combEdFingers1.Value = objFinger1.FingerIndex;
            if (objFinger1.HasSaved)
            {
                btnEnroll1.Text = "Re Enroll";
                lblFingerPic1.Visible = true;
            }
            else
            {
                btnEnroll1.Text = "Enroll";
                lblFingerPic1.Visible = false;
            }

            combEdFingers2.Value = objFinger2.FingerIndex;
            if (objFinger2.HasSaved)
            {
                btnEnroll2.Text = "Re Enroll";
                lblFingerPic2.Visible = true;
            }
            else
            {
                btnEnroll2.Text = "Enroll";
                lblFingerPic2.Visible = false;
            }
        }

        private void frmEnrollFingerPrint_Load(object sender, EventArgs e)
        {
            fpReader = new FingerPrintReader();
            fpReader.senderER = this;
            fpReader.enrollmentFlag = true;

            IsFPReaderConnected = fpReader.FingerPrintReaderConnect();
            if (!IsFPReaderConnected)
                lblStatus.Text = "Error: Fingerprint reader has not been connected to the computer.";
            else
                lblStatus.Text = "Fingerprint reader is ready.";
        }

        private void btnEnroll1_Click(object sender, EventArgs e)
        {
            if(!IsReadyForSaving || enrollmentFinger==null)
            {
                MessageBox.Show("Please capture the fingerprint first.");
                return;
            }

            string currFingerIdx = combEdFingers1.Value.ToString().Trim();
            if (objFinger2.HasSaved == true && objFinger2.FingerIndex.Equals(currFingerIdx, StringComparison.OrdinalIgnoreCase))
            {
                MessageBox.Show("Please chose different finger index for Finger 1.");
                return;
            }

            if( fpReader != null )
            {
                string UserID = string.Empty;
                fpReader.MatchFingerPrintToUser(enrollmentFinger, out UserID);
                UserID = UserID.Trim();
                if( !string.IsNullOrWhiteSpace(UserID) && !mUserID.Equals(UserID, StringComparison.OrdinalIgnoreCase))
                {
                    MessageBox.Show("The same fingerprint has been enrolled under different user '" + UserID + "'.\nPlease enroll your fingerprint under your own PHUser.");
                    return;
                }
            }

            string statusMsg = "Finger 1 has been enrolled successfully.";
            try
            {
                if (objFinger1.HasSaved)
                {
                    oDBUser.UpdateUserFingerPrint(dtUser.Rows[0]["ID"].ToString(), objFinger1.FingerIndex, currFingerIdx, Fmd.SerializeXml(enrollmentFinger));
                    statusMsg = "Finger 1 has been re-enrolled successfully.";
                }
                else
                    oDBUser.SaveUserFingerPrint(dtUser.Rows[0]["ID"].ToString(), currFingerIdx, Fmd.SerializeXml(enrollmentFinger));

                HasChangedEnrollment = true;

                objFinger1.FingerIndex = currFingerIdx;
                objFinger1.HasSaved = true;
                lblFingerPic1.Visible = true;
                IsReadyForSaving = false;
                btnEnroll1.Text = "Re Enroll";

                fpReader.FingerprintReaderResetData();

                if (objFinger2.HasSaved)
                    this.Close();
            }
            catch(Exception exp)
            {
                statusMsg = "Failed to save Fingerprint 1 to db. " + exp.Message;
            }

            lblStatus.Text = statusMsg;
        }

        private void btnEnroll2_Click(object sender, EventArgs e)
        {
            if (!IsReadyForSaving || enrollmentFinger == null)
            {
                MessageBox.Show("Please capture the fingerprint first.");
                return;
            }

            string currFingerIdx = combEdFingers2.Value.ToString().Trim();
            if (objFinger1.HasSaved==true&&objFinger1.FingerIndex.Equals(currFingerIdx, StringComparison.OrdinalIgnoreCase))
            {
                MessageBox.Show("Please chose different finger index for Finger 2.");
                return;
            }

            if (fpReader != null)
            {
                string otherInit = string.Empty;
                fpReader.MatchFingerPrintToUser(enrollmentFinger, out otherInit);
                otherInit = otherInit.Trim();
                if (!string.IsNullOrWhiteSpace(otherInit) && !mUserID.Equals(otherInit, StringComparison.OrdinalIgnoreCase))
                {
                    MessageBox.Show("The same fingerprint has been enrolled under different user '" + otherInit + "'.\nPlease enroll your fingerprint under your own PHUser.");
                    return;
                }
            }

            string statusMsg2 = "Finger 2 has been enrolled successfully.";
            try
            {
                if (objFinger2.HasSaved)
                {
                    oDBUser.UpdateUserFingerPrint(dtUser.Rows[0]["ID"].ToString(), objFinger2.FingerIndex, currFingerIdx, Fmd.SerializeXml(enrollmentFinger));
                    statusMsg2 = "Finger 2 has been re-enrolled successfully.";
                }
                else
                    oDBUser.SaveUserFingerPrint(dtUser.Rows[0]["ID"].ToString(), currFingerIdx, Fmd.SerializeXml(enrollmentFinger));

                HasChangedEnrollment = true;

                objFinger2.FingerIndex = currFingerIdx;
                objFinger2.HasSaved = true;
                lblFingerPic2.Visible = true;
                IsReadyForSaving = false;
                btnEnroll2.Text = "Re Enroll";

                fpReader.FingerprintReaderResetData();

                if (objFinger1.HasSaved)
                    this.Close();
            }
            catch (Exception exp)
            {
                statusMsg2 = "Failed to save Fingerprint 2 to db. " + exp.Message;
            }

            lblStatus.Text = statusMsg2;
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            string warnMsg = string.Empty;
            string stsMsg = string.Empty;

            if (objFinger1.HasSaved && objFinger2.HasSaved)
            {
                warnMsg = "Reset will delete Finger 1 & 2 enrollments from database.";
                stsMsg = "Finger 1 and 2 enrollments have been deleted successfully.";
            }
            if (objFinger1.HasSaved && !objFinger2.HasSaved)
            {
                warnMsg = "Reset will delete Finger 1 enrollment from database.";
                stsMsg = "Finger 1 enrollment has been deleted successfully.";
            }
            if (!objFinger1.HasSaved && objFinger2.HasSaved)
            {
                warnMsg = "Reset will delete Finger 2 enrollment from database.";
                stsMsg = "Finger 2 enrollment has been deleted successfully.";
            }

            if(!string.IsNullOrWhiteSpace(warnMsg))
            {
                warnMsg += "\nDo you want to continue?";
                if( MessageBox.Show(warnMsg, "Enroll Fingerprints", MessageBoxButtons.YesNo) == DialogResult.Yes )
                {
                    try
                    {
                        oDBUser.DeleteUserFingerPrint(dtUser.Rows[0]["ID"].ToString());

                        objFinger1.HasSaved = false;
                        lblFingerPic1.Visible = false;
                        btnEnroll1.Text = "Enroll";

                        objFinger2.HasSaved = false;
                        lblFingerPic2.Visible = false;
                        btnEnroll2.Text = "Enroll";

                        HasChangedEnrollment = true;
                        fpReader.FingerprintReaderResetData();
                    }
                    catch(Exception exp)
                    {
                        stsMsg = "Failed to delete fingerprint(s) from database for user: " + mUserID;
                    }

                    picBoxFinger.Image = null;
                    lblStatus.Text = stsMsg;
                }
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmEnrollFingerPrint_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (IsFPReaderConnected)
            {
                fpReader.DisconnectCloseReader();
            }
        }

        #region SendMessage
        private delegate void SendMessageCallback(fpReaderAction action, object payload);

        public void SendMessage(fpReaderAction action, object payload)
        {
            if (this.InvokeRequired)
            {
                SendMessageCallback d = new SendMessageCallback(SendMessage);
                this.Invoke(d, new object[] { action, payload });
            }
            else
            {
                switch (action)
                {
                    case fpReaderAction.SendMessage:
                        string statusStr = (string)payload;
                        lblStatus.Text = statusStr;
                        IsReadyForSaving = false;
                        break;
                    case fpReaderAction.SendBitmap:
                        picBoxFinger.Image = (Bitmap)payload;
                        picBoxFinger.Refresh();
                        break;
                    case fpReaderAction.SendFMD:
                        enrollmentFinger = (Fmd)payload;
                        IsReadyForSaving = true;
                        break;
                    default:
                        break;
                }
            }
        }
        #endregion

        private void lblStatus_TextChanged(object sender, EventArgs e)
        {
            lblStatus.BackColor = origBgColor;

            if (lblStatus.Text != null)
            {
                if (lblStatus.Text.Contains("to save it."))
                    lblStatus.BackColor = Color.LightGreen;

                if (lblStatus.Text.Contains("Error") || lblStatus.Text.Contains("Failed to"))
                    lblStatus.BackColor = Color.Orange;
            }
        }
    }

    public class FingerPrintData
    {
        public string User { get; set; }
        public string UserName { get; set; }
        public int UserID { get; set; }
        public string FingerIndex { get; set; }
        public string Fingerprint { get; set; }
        public string Type { get; set; }
        public DateTime LastUpdated { get; set; }

        public bool HasSaved { get; set; }
    }
}
