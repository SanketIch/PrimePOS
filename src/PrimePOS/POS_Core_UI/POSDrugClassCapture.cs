/*
 * Author: Manoj Kumar
 * Description: This use all the controls from the form.
 *              No code is under the control on the form.
 * Date: 4/12/2013
 *
 */

using POS_Core.Resources.DelegateHandler;
using POS_Core_UI;
using System;
using System.Windows.Forms;
using System.Data;  //PRIMEPOS-2429 27-Jun-2019 JY Added
using PharmData;    //PRIMEPOS-2429 27-Jun-2019 JY Added
using POS_Core.Resources;   //PRIMEPOS-2429 27-Jun-2019 JY Added
using NLog;

namespace POS_Core.Business_Tier
{
    internal class POSDrugClassCapture
    {
        private frmPOSDrugClassCapture uControl = null;
        private bool _isCancelled;
        private bool isUserScan = false;
        private string Data;
        private DL DL = null;
        private delegate void ScanEventHandler(); //Added by Manoj 9/12/2013
        private event ScanEventHandler ScanData; //Added by Manoj 9/12/2013
        private event ScanEventHandler AfterScanData; //Added by Manoj 9/12/2013
        private static ILogger logger = LogManager.GetCurrentClassLogger();

        public bool isCancelled
        {
            get { return _isCancelled; }
            set { _isCancelled = value; }
        }

        public POSDrugClassCapture(Form fControls)
        {
            uControl = (frmPOSDrugClassCapture) fControls; //Get the control from the form
            uControl.Shown += new System.EventHandler(uControl_Load); //load event
            uControl.btncancel.Click += new System.EventHandler(btncancel_Click); //button click
            uControl.btnContinue.Click += new System.EventHandler(btnContinue_Click); //button click
            EscEvent();
        }

        /// <summary>
        /// This will load the form with the location
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void uControl_Load(object sender, System.EventArgs e)
        {
            uControl.Left = 100;
            uControl.Top = 100;
            //uControl.lblPatientName11.Text = uControl.PatientNo;  //PRIMEPOS-2429 27-Jun-2019 JY Commented
            GetPatientInformation();    //PRIMEPOS-2429 27-Jun-2019 JY Added
            #region PRIMEPOS-3065 10-Mar-2022 JY Added
            if (!uControl._bIsPatient)
            {
                uControl.cboRelation.SelectedIndex = 4;
            }
            else
            {
                if (uControl._strDriversLicense != string.Empty)
                {
                    uControl.txtIDNum.Text = uControl._strDriversLicense;
                    try
                    {
                        if (uControl._DriversLicenseExpDate != null && uControl._DriversLicenseExpDate != DateTime.MinValue)
                            uControl.dtpDriversLicenseExpDate.Value = uControl._DriversLicenseExpDate.ToShortDateString();
                    }
                    catch
                    {
                        uControl.dtpDriversLicenseExpDate.Text = "";
                    }
                }
            }
            #endregion

            if (uControl.cboRelation.SelectedIndex == 0)
            {
                uControl.txtFirstName.Enabled = false;
                uControl.txtLastName.Enabled = false;
                uControl.txtState.Enabled = false;
            }
            if(uControl.cboVerifID.SelectedIndex == 0 && uControl.cboRelation.SelectedIndex == 0)
            {
                if (uControl.txtIDNum.Text.Trim() == "")
                    uControl.txtIDNum.Focus();
                else if (uControl.dtpDriversLicenseExpDate.Text.Trim() == "//")
                    uControl.dtpDriversLicenseExpDate.Focus();
                else
                    uControl.btnContinue.Focus();
            }
        }

        /// <summary>
        /// KeyDown Function
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void uControl_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Escape)
            {
                isCancelled = true;
                uControl.Hide();
            }
            else if(e.KeyCode == Keys.Enter && uControl.txtIDNum.Focused)
            {
                uControl.btnContinue.Focus();
            }
        }

        /// <summary>
        /// This function is for the Escape Event on any entry
        /// </summary>
        private void EscEvent()
        {
            uControl.cboVerifID.Select();
            uControl.cboRelation.SelectedIndex = 0; //defaulting to Patient
            uControl.cboVerifID.SelectedIndex = 0; //defaulting to DL
            uControl.cboVerifID.KeyDown += new KeyEventHandler(uControl_KeyDown);
            uControl.cboVerifID.SelectedIndexChanged += new System.EventHandler(cboVerifID_SelectedIndexChanged);
            uControl.cboRelation.KeyDown += new KeyEventHandler(uControl_KeyDown);
            uControl.txtFirstName.KeyDown += new KeyEventHandler(uControl_KeyDown);
            uControl.txtLastName.KeyDown += new KeyEventHandler(uControl_KeyDown);
            uControl.txtIDNum.KeyDown += new KeyEventHandler(uControl_KeyDown);
            uControl.txtState.KeyDown += new KeyEventHandler(uControl_KeyDown);
            ScanData += POSDrugClassCapture_ScanData;
            ScanData.Invoke();
            uControl.cboRelation.SelectedIndexChanged += new System.EventHandler(cboRelation_SelectedIndexChanged);
            uControl.cboRelation.DropDownStyle = ComboBoxStyle.DropDownList;
            uControl.cboVerifID.DropDownStyle = ComboBoxStyle.DropDownList;
        }

        /// <summary>
        /// Text box Events
        /// </summary>
        private void POSDrugClassCapture_ScanData()
        {
            uControl.txtIDNum.TextChanged += txtIDNum_TextChanged;
            uControl.txtIDNum.LostFocus += txtIDNum_LostFocus;
        }

        /// <summary>
        /// After scan set text box to lost focus so it can pass data to get parse
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtIDNum_LostFocus(object sender, EventArgs e)
        {
            if(isUserScan && Data != string.Empty)
            {
                logger.Trace("txtIDNum_LostFocus(object sender, EventArgs e) - " + Data); //PRIMEPOS-3162 18-Nov-2022 JY Added
                if (DL != null)
                {
                    DL = null;
                }
                DL = new DL(Data);
                isUserScan = false;
                AfterScanData += POSDrugClassCapture_AfterScanData;
                AfterScanData.Invoke();
            }
        }

        /// <summary>
        /// Event After the Driver Lic is scan
        /// </summary>
        private void POSDrugClassCapture_AfterScanData()
        {
            if(!string.IsNullOrEmpty(DL.DAQ))
            {
                //ReSet the Text box to default state
                uControl.txtIDNum.Text = string.Empty;
                uControl.txtIDNum.Multiline = false;
                uControl.txtIDNum.PasswordChar = '\0';

                if(uControl.cboRelation.SelectedIndex != 0)
                {
                    uControl.txtFirstName.Text = DL.DCT;
                    uControl.txtState.Text = DL.DAJ;
                    uControl.txtLastName.Text = DL.DCS;
                }
                uControl.txtIDNum.Text = DL.DAQ.Trim();

                #region PRIMEPOS-3065 10-Mar-2022 JY Added
                try
                {
                    String dateString = Configuration.convertNullToString(DL.DBA); //Get the Driver License Expiration Date
                    logger.Trace("POSDrugClassCapture_AfterScanData() - " + dateString); //PRIMEPOS-3162 18-Nov-2022 JY Added
                    String format = "MMddyyyy";
                    DateTime result = DateTime.MinValue;
                    bool bError = false;
                    try
                    {
                        result = DateTime.ParseExact(dateString, format, System.Globalization.CultureInfo.InvariantCulture);
                    }
                    catch (Exception Ex)
                    {
                        bError = true;
                        logger.Fatal(Ex, "POSDrugClassCapture_AfterScanData() - 1");    //PRIMEPOS-3162 18-Nov-2022 JY Added
                    }
                    if (!bError)
                    {
                        if (result.ToShortDateString() != DateTime.MinValue.ToShortDateString())
                            uControl.dtpDriversLicenseExpDate.Value = result.ToShortDateString();
                        else
                        {
                            uControl.dtpDriversLicenseExpDate.Text = "";
                            try
                            {
                                uControl.dtpDriversLicenseExpDate.Text = DL.DBA.Substring(0, 2) + "/" + DL.DBA.Substring(2, 2) + "/" + DL.DBA.Substring(4, 4);
                            }
                            catch (Exception Ex)
                            {
                                logger.Fatal(Ex, "POSDrugClassCapture_AfterScanData() - 2");    //PRIMEPOS-3162 18-Nov-2022 JY Added
                            }
                        }
                    }
                }
                catch { }
                #endregion
            }
        }

        /// <summary>
        /// Manoj 9/12/2013
        /// Text box change event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtIDNum_TextChanged(object sender, System.EventArgs e)
        {
            Data = uControl.txtIDNum.Text;
            if(!isUserScan && Data.StartsWith("@"))
            {
                isUserScan = true;
                uControl.txtIDNum.Multiline = true;
                uControl.txtIDNum.PasswordChar = 'X';
            }
        }

        private void cboRelation_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if(uControl.cboRelation.SelectedIndex == 0)
            {
                uControl.txtFirstName.Text = "";
                uControl.txtFirstName.Enabled = false;
                uControl.txtLastName.Text = "";
                uControl.txtLastName.Enabled = false;
                uControl.txtState.Text = "";
                uControl.txtState.Enabled = false;
                uControl.cboRelation.DropDownStyle = ComboBoxStyle.DropDownList;
            }
            else
            {
                uControl.txtFirstName.Enabled = true;
                uControl.txtLastName.Enabled = true;
                uControl.txtState.Enabled = true;
            }
            uControl.txtIDNum.Text = string.Empty;
            uControl.txtIDNum.Focus();
            isUserScan = false;
        }

        private void cboVerifID_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            uControl.txtIDNum.Text = string.Empty;
            uControl.txtIDNum.Focus();
            uControl.cboVerifID.DropDownStyle = ComboBoxStyle.DropDownList;
            isUserScan = false;
        }

        /// <summary>
        /// This validate all fields to make sure all info are entered
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnContinue_Click(object sender, System.EventArgs e)
        {
            if(uControl.cboVerifID.Text != "")
            {
                if(uControl.cboRelation.Text != "")
                {
                    if(uControl.cboRelation.SelectedIndex == 0)
                    {
                        if(uControl.txtIDNum.Text.Trim() != "")
                        {
                            if (!ValidIdNumber()) return;   //PRIMEPOS-2519 30-May-2018 JY Added 
                            uControl.Close();
                        }
                        else
                        {
                            uControl.txtIDNum.Focus();
                        }
                    }
                    else
                    {
                        if(uControl.txtIDNum.Text.Trim() != "")
                        {
                            if (!ValidIdNumber()) return;   //PRIMEPOS-2519 30-May-2018 JY Added
                            if (uControl.txtState.Text != "")
                            {
                                if(uControl.txtFirstName.Text != "")
                                {
                                    if(uControl.txtLastName.Text != "")
                                    {
                                        uControl.Close();
                                    }
                                    else
                                    {
                                        uControl.txtLastName.Focus();
                                    }
                                }
                                else
                                {
                                    uControl.txtFirstName.Focus();
                                }
                            }
                            else
                            {
                                uControl.txtState.Focus();
                            }
                        }
                        uControl.txtIDNum.Focus();
                    }
                }
                else
                {
                    uControl.cboRelation.DroppedDown = true;
                }
            }
            else
            {
                uControl.cboVerifID.DroppedDown = true;
            }
        }

        /// <summary>
        /// Set the isCancelled to True and close the form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btncancel_Click(object sender, System.EventArgs e)
        {
            isCancelled = true;
            uControl.Hide();
        }

        #region PRIMEPOS-2519 30-May-2018 JY Added IdNumber validation
        private bool ValidIdNumber()
        {
            if (uControl.txtIDNum.Text.Trim().Length > 20)
            {
                POS_Core_UI.Resources.Message.Display("\"Id Number\" should be less than or equal to 20 characters", uControl.Text, MessageBoxButtons.OK);
                uControl.txtIDNum.Focus();
                return false;
            }
            return true;
        }
        #endregion

        #region PRIMEPOS-2429 27-Jun-2019 JY Added
        private void GetPatientInformation()
        {
            try
            {
                DataTable dtPatInfo = null;
                if (uControl.TblPatient != null)
                    dtPatInfo = uControl.TblPatient;
                else
                {
                    PharmBL oPharmBL = new PharmBL();
                    dtPatInfo = oPharmBL.GetPatient(uControl.PatientNo);
                }
                if (dtPatInfo != null && dtPatInfo.Rows.Count > 0)
                {
                    //, , , , DOB, MOBILENO
                    uControl.txtPatientName.Text = Configuration.convertNullToString(dtPatInfo.Rows[0]["LNAME"]).Trim()  + ", " + Configuration.convertNullToString(dtPatInfo.Rows[0]["FNAME"]).Trim();
                    uControl.txtPhone.Text = Configuration.convertNullToString(dtPatInfo.Rows[0]["PHONE"]).Trim();
                    uControl.txtAddress.Text = Configuration.convertNullToString(dtPatInfo.Rows[0]["ADDRSTR"]).Trim() + ", " + Configuration.convertNullToString(dtPatInfo.Rows[0]["ADDRCT"]).Trim() + ", " + Configuration.convertNullToString(dtPatInfo.Rows[0]["ADDRST"]).Trim() + ", " + Configuration.convertNullToString(dtPatInfo.Rows[0]["ADDRZP"]).Trim();

                    if (string.IsNullOrEmpty(Configuration.convertNullToString(dtPatInfo.Rows[0]["DOB"].ToString().Trim())))
                        uControl.txtDOB.Text = "";
                    else
                        uControl.txtDOB.Text = Convert.ToDateTime(dtPatInfo.Rows[0]["DOB"].ToString().Trim()).ToShortDateString();

                    uControl.txtCellPhone.Text = Configuration.convertNullToString(dtPatInfo.Rows[0]["MOBILENO"]).Trim();
                }
            }
            catch(Exception Ex)
            {

            }
        }
        #endregion
    }
}