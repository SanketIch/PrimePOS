using Infragistics.Win.UltraWinGrid;
using NLog;
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

namespace POS_Core_UI
{
    public partial class frmCustomerPatientMapping : Form
    {
        private static ILogger logger = LogManager.GetCurrentClassLogger();
        public CustomerData oCustomerData;
        DataSet dsPatient = new DataSet();
        Font fontBold = new System.Drawing.Font("Verdana", 9, FontStyle.Bold);

        public frmCustomerPatientMapping()
        {
            InitializeComponent();
        }

        public frmCustomerPatientMapping(DataSet dsPat)
        {
            dsPatient = dsPat;
            InitializeComponent();
            //PopulatePatient(dsPat);
            DisplayPatientInformation(dsPat);
            LoadDefaultFilters(dsPat);
            PopulateMatchingCustomers(dsPat);
        }

        //private void PopulatePatient(DataSet dsPatient)
        //{
        //    try
        //    {                
        //        txtPLastName.Text = Configuration.convertNullToString(dsPatient.Tables[0].Rows[0]["LNAME"]);
        //        txtPFirstName.Text = Configuration.convertNullToString(dsPatient.Tables[0].Rows[0]["FNAME"]);
        //        DateTime oDOB;
        //        DateTime.TryParse(Configuration.convertNullToString(dsPatient.Tables[0].Rows[0]["DOB"]).Trim(), out oDOB);
        //        dtpPDOB.Value = oDOB;
        //        txtPHomePhone.Text = Configuration.convertNullToString(dsPatient.Tables[0].Rows[0]["PHONE"]);
        //        txtPOfficePhone.Text = Configuration.convertNullToString(dsPatient.Tables[0].Rows[0]["WORKNO"]);
        //        txtPMobileNo.Text = Configuration.convertNullToString(dsPatient.Tables[0].Rows[0]["MOBILENO"]);
        //    }
        //    catch(Exception Ex)
        //    {
        //    }
        //}

        private void DisplayPatientInformation(DataSet dsPatient)
        {
            StringBuilder sbPatInfo = new System.Text.StringBuilder(1024);
            string strPatName = string.Empty;
            if (Configuration.convertNullToString(dsPatient.Tables[0].Rows[0]["LNAME"]).Trim() != "")
                strPatName = Configuration.convertNullToString(dsPatient.Tables[0].Rows[0]["LNAME"]).Trim();
            if (Configuration.convertNullToString(dsPatient.Tables[0].Rows[0]["FNAME"]).Trim() != "")
            {
                if (strPatName != string.Empty)
                    strPatName += ", " + Configuration.convertNullToString(dsPatient.Tables[0].Rows[0]["FNAME"]).Trim();
                else
                    strPatName = Configuration.convertNullToString(dsPatient.Tables[0].Rows[0]["FNAME"]).Trim();
            }
            sbPatInfo.Append("Name: " + strPatName);

            string strPatAddress = Configuration.convertNullToString(dsPatient.Tables[0].Rows[0]["ADDRSTR"]).Trim();
            if (strPatAddress != "")
                strPatAddress += " " + Configuration.convertNullToString(dsPatient.Tables[0].Rows[0]["ADDRSTRLINE2"]).Trim();
            else
                strPatAddress = Configuration.convertNullToString(dsPatient.Tables[0].Rows[0]["ADDRSTRLINE2"]).Trim();
            if (strPatAddress != "")
                strPatAddress += " " + Configuration.convertNullToString(dsPatient.Tables[0].Rows[0]["ADDRCT"]).Trim();
            else
                strPatAddress = Configuration.convertNullToString(dsPatient.Tables[0].Rows[0]["ADDRCT"]).Trim();
            if (strPatAddress != "")
                strPatAddress += " " + Configuration.convertNullToString(dsPatient.Tables[0].Rows[0]["ADDRST"]).Trim();
            else
                strPatAddress = Configuration.convertNullToString(dsPatient.Tables[0].Rows[0]["ADDRST"]).Trim();
            if (strPatAddress != "")
                strPatAddress += " " + Configuration.convertNullToString(dsPatient.Tables[0].Rows[0]["ADDRZP"]).Trim();
            else
                strPatAddress = Configuration.convertNullToString(dsPatient.Tables[0].Rows[0]["ADDRZP"]).Trim();

            sbPatInfo.Append("\t" + "Address: " + strPatAddress);

            DateTime oDOB;
            bool bStatus = DateTime.TryParse(Configuration.convertNullToString(dsPatient.Tables[0].Rows[0]["DOB"]).Trim(), out oDOB);
            if (bStatus)
                sbPatInfo.Append("\n" + "DOB: " + oDOB.Date.ToShortDateString());
            sbPatInfo.Append("\t" + "Sex: " + Configuration.convertNullToString(dsPatient.Tables[0].Rows[0]["SEX"]).Trim());

            string strPhoneNos = string.Empty;
            strPhoneNos = Configuration.convertNullToString(dsPatient.Tables[0].Rows[0]["PHONE"]).Trim();
            if (strPhoneNos != "")
                strPhoneNos += ", " + Configuration.convertNullToString(dsPatient.Tables[0].Rows[0]["WORKNO"]).Trim();
            else
                strPhoneNos = Configuration.convertNullToString(dsPatient.Tables[0].Rows[0]["WORKNO"]).Trim();
            if (strPhoneNos != "")
                strPhoneNos += ", " + Configuration.convertNullToString(dsPatient.Tables[0].Rows[0]["MOBILENO"]).Trim();
            else
                strPhoneNos = Configuration.convertNullToString(dsPatient.Tables[0].Rows[0]["MOBILENO"]).Trim();
            sbPatInfo.Append("\n" + "PhoneNos: " + strPhoneNos);

            rtbPatientInfo.Text = sbPatInfo.ToString();

            rtbPatientInfo.Find("Name:", 0);
            rtbPatientInfo.SelectionFont = fontBold;
            rtbPatientInfo.SelectionColor = Color.Maroon;

            rtbPatientInfo.Find("Address:", 0);
            rtbPatientInfo.SelectionFont = fontBold;
            rtbPatientInfo.SelectionColor = Color.Maroon;

            rtbPatientInfo.Find("DOB:", 0);
            rtbPatientInfo.SelectionFont = fontBold;
            rtbPatientInfo.SelectionColor = Color.Maroon;

            rtbPatientInfo.Find("Sex:", 0);
            rtbPatientInfo.SelectionFont = fontBold;
            rtbPatientInfo.SelectionColor = Color.Maroon;

            rtbPatientInfo.Find("PhoneNos:", 0);
            rtbPatientInfo.SelectionFont = fontBold;
            rtbPatientInfo.SelectionColor = Color.Maroon;
        }

        private void LoadDefaultFilters(DataSet dsPatient)
        {
            try
            {
                txtLastName.Text = Configuration.convertNullToString(dsPatient.Tables[0].Rows[0]["LNAME"].ToString()).Trim();
                txtFirstName.Text = Configuration.convertNullToString(dsPatient.Tables[0].Rows[0]["FNAME"].ToString()).Trim();
                DateTime oDOB;
                DateTime.TryParse(Configuration.convertNullToString(dsPatient.Tables[0].Rows[0]["DOB"]).Trim(), out oDOB);
                dtpDOB.Value = oDOB;
                txtPhoneNo.Text = Configuration.convertNullToString(dsPatient.Tables[0].Rows[0]["PHONE"]).Trim();
            }
            catch { }
        }

        private void PopulateMatchingCustomers(DataSet dsPatient)
        {
            POSTransaction oPOSTrans = new POSTransaction();
            int LevelId = 0;
            CustomerData oCustomer = oPOSTrans.GetCustomerByMultiplePatientsPatameters(dsPatient, ref LevelId);
            if (oCustomer != null && oCustomer.Tables.Count > 0 && oCustomer.Customer.Rows.Count > 0)
            {
                grdCustomers.DataSource = oCustomer.Customer;
                for (int i = 0; i < oCustomer.Customer.Columns.Count; i++)
                {
                    grdCustomers.DisplayLayout.Bands[0].Columns[i].Hidden = true;
                }
                CustomerGridFormatting();

                if (grdCustomers.Rows.Count > 0)
                {
                    if (LevelId == 3)
                    {
                        var row = grdCustomers.Rows.FirstOrDefault(r => Convert.ToDateTime(r.Cells["DATEOFBIRTH"].Value) == Convert.ToDateTime(dtpDOB.Value) && r.Cells["CustomerName"].Value.ToString().Trim() == txtLastName.Text.ToString().Trim());
                        if (row == null)
                        {
                            row = grdCustomers.Rows.FirstOrDefault(r => Convert.ToDateTime(r.Cells["DATEOFBIRTH"].Value) == Convert.ToDateTime(dtpDOB.Value) && r.Cells["FIRSTNAME"].Value.ToString().Trim() == txtFirstName.Text.ToString().Trim());
                        }
                        grdCustomers.ActiveRow = row;
                    }
                    //if (LevelId == 4)
                    //{
                    //    var row = grdCustomers.Rows.FirstOrDefault(r => r.Cells["CustomerName"].Value.ToString().Trim() == txtPLastName.Text.ToString().Trim());
                    //    if (row == null)
                    //    {
                    //        row = grdCustomers.Rows.FirstOrDefault(r => r.Cells["FIRSTNAME"].Value.ToString().Trim() == txtPFirstName.Text.ToString().Trim());
                    //    }
                    //    grdCustomers.ActiveRow = row;
                    //}
                }
            }
        }

        private void CustomerGridFormatting()
        {
            grdCustomers.DisplayLayout.Bands[0].Columns["CustomerName"].Hidden = false;
            grdCustomers.DisplayLayout.Bands[0].Columns["FIRSTNAME"].Hidden = false;
            grdCustomers.DisplayLayout.Bands[0].Columns["DATEOFBIRTH"].Hidden = false;
            grdCustomers.DisplayLayout.Bands[0].Columns["PHONEHOME"].Hidden = false;
            grdCustomers.DisplayLayout.Bands[0].Columns["PhoneOff"].Hidden = false;
            grdCustomers.DisplayLayout.Bands[0].Columns["CellNo"].Hidden = false;
            grdCustomers.DisplayLayout.Bands[0].Columns["PatientNo"].Hidden = false;

            grdCustomers.DisplayLayout.Bands[0].Columns["CustomerName"].Header.VisiblePosition = 0;
            grdCustomers.DisplayLayout.Bands[0].Columns["FIRSTNAME"].Header.VisiblePosition = 1;
            grdCustomers.DisplayLayout.Bands[0].Columns["DATEOFBIRTH"].Header.VisiblePosition = 2;
            grdCustomers.DisplayLayout.Bands[0].Columns["PHONEHOME"].Header.VisiblePosition = 3;
            grdCustomers.DisplayLayout.Bands[0].Columns["PhoneOff"].Header.VisiblePosition = 4;
            grdCustomers.DisplayLayout.Bands[0].Columns["CellNo"].Header.VisiblePosition = 5;
            grdCustomers.DisplayLayout.Bands[0].Columns["PatientNo"].Header.VisiblePosition = 6;

            grdCustomers.DisplayLayout.Bands[0].Columns["CustomerName"].Header.Caption = "Last Name";
            grdCustomers.DisplayLayout.Bands[0].Columns["FIRSTNAME"].Header.Caption = "First Name";
            grdCustomers.DisplayLayout.Bands[0].Columns["DATEOFBIRTH"].Header.Caption = "DOB";
            grdCustomers.DisplayLayout.Bands[0].Columns["PHONEHOME"].Header.Caption = "Home Phone";
            grdCustomers.DisplayLayout.Bands[0].Columns["PhoneOff"].Header.Caption = "Office Phone";
            grdCustomers.DisplayLayout.Bands[0].Columns["CellNo"].Header.Caption = "Mobile No";
            grdCustomers.DisplayLayout.Bands[0].Columns["PatientNo"].Header.Caption = "Patient No";

            grdCustomers.DisplayLayout.Bands[0].Columns["CustomerName"].Width = 100;
            grdCustomers.DisplayLayout.Bands[0].Columns["FIRSTNAME"].Width = 100;
            grdCustomers.DisplayLayout.Bands[0].Columns["DATEOFBIRTH"].Width = 90;
            grdCustomers.DisplayLayout.Bands[0].Columns["PHONEHOME"].Width = 100;
            grdCustomers.DisplayLayout.Bands[0].Columns["PhoneOff"].Width = 100;
            grdCustomers.DisplayLayout.Bands[0].Columns["CellNo"].Width = 100;
            grdCustomers.DisplayLayout.Bands[0].Columns["PatientNo"].Width = 110;

            grdCustomers.DisplayLayout.Bands[0].Columns["DATEOFBIRTH"].Format = "MM/dd/yyyy";
        }

        private void frmCustomerPatientMapping_Load(object sender, EventArgs e)
        {
            logger.Trace("frmCustomerPatientMapping_Load() - " + clsPOSDBConstants.Log_Entering);
            clsUIHelper.setColorSchecme(this);
            try
            {
                rtbMessage.Find("Add New Patient", 0);
                rtbMessage.SelectionFont = fontBold;
                rtbMessage.Find("Link Customer", 0);
                rtbMessage.SelectionFont = fontBold;
                //rtbMessage.SelectionColor = Color.Maroon;
            }
            catch { }
            logger.Trace("frmCustomerPatientMapping_Load() - " + clsPOSDBConstants.Log_Exiting);
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (txtLastName.Text.Trim() == "" && txtFirstName.Text.Trim() == "" && dtpDOB == null && txtPhoneNo.Text.Trim() == "")
            {
                POS_Core_UI.Resources.Message.Display(this, "Please provide at least one filter.", "PrimePOS", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                Customer oCustomer = new Customer();
                DataTable dtCustomer = oCustomer.GetCustomersForMapping(txtLastName.Text.Trim(), txtFirstName.Text.Trim(), dtpDOB.Value, txtPhoneNo.Text.Trim(), chkIncludeLinkedCustomers.Checked);
                grdCustomers.DataSource = dtCustomer;
                if (dtCustomer != null && dtCustomer.Rows.Count > 0)
                {
                    grdCustomers.DisplayLayout.Bands[0].Columns["CustomerID"].Hidden = true;
                    CustomerGridFormatting();
                }
            }
        }
        
        private void btnLinkCustomer_Click(object sender, EventArgs e)
        {
            if (grdCustomers.ActiveRow != null && grdCustomers.ActiveRow.Cells.Count > 0)
            {
                //if (POS_Core_UI.Resources.Message.Display(this, "This action will link the selected customer with the Rx Patient. Are you sure to continue?", "PrimePOS", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                //{
                int CustomerID = Configuration.convertNullToInt(grdCustomers.ActiveRow.Cells[clsPOSDBConstants.Customer_Fld_CustomerId].Text);
                int PatientNo = Configuration.convertNullToInt(dsPatient.Tables[0].Rows[0]["PATIENTNO"]);
                int retVal = UpdatePatientNo(CustomerID, PatientNo);
                if (retVal > 0)
                {
                    //clsUIHelper.ShowSuccessMsg("The selected customer has been linked successfully with the patient.", "Success...");
                    POSTransaction oPOSTrans = new POSTransaction();
                    oCustomerData = oPOSTrans.GetCustomerByID(CustomerID);
                    this.Close();
                }
                //}
            }
            else
            {
                POS_Core_UI.Resources.Message.Display(this, "Please select a customer.", "PrimePOS", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private int UpdatePatientNo(int CustomerID, int PatientNo)
        {
            int retVal = 0;
            try
            {
                Customer oCustomer = new Customer();
                retVal = oCustomer.UpdatePatientNo(CustomerID, PatientNo);
            }
            catch (Exception exp)
            {
                logger.Fatal(exp, "UpdatePatientNo(int CustomerID, int PatientNo)");
                throw exp;
            }
            return retVal;
        }

        //private void btnClose_Click(object sender, EventArgs e)
        //{
        //    //if (POS_Core_UI.Resources.Message.Display(this, "This action will proceed with a customer already selected for the transaction. Are you sure to continue?", "PrimePOS", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
        //    //{
        //    oCustomerData = null;
        //    this.Close();
        //    //}
        //}

        private void grdCustomers_InitializeRow(object sender, Infragistics.Win.UltraWinGrid.InitializeRowEventArgs e)
        {
            //if (grdCustomers.Rows.Count > 0)
            //    btnLinkCustomer.Enabled = true;
            //else
            //    btnLinkCustomer.Enabled = false;
        }

        private void btnAddNewPatient_Click(object sender, EventArgs e)
        {
            //if (POS_Core_UI.Resources.Message.Display(this, "This action will add a new customer in POS. Are you sure to continue?", "PrimePOS", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            //{
            POSTransaction oPOSTrans = new POSTransaction();
            oCustomerData = oPOSTrans.CreateCustomerDSFromPatientDS(dsPatient, false);
            if (oCustomerData != null)
            {
                Customer oCustomer = new Customer();
                oCustomer.Persist(oCustomerData, true);
                oCustomerData = oPOSTrans.GetCustomerByPatientNo(Configuration.convertNullToInt(dsPatient.Tables[0].Rows[0]["PATIENTNO"]));
            }
            this.Close();
            //}
        }
    }
}
