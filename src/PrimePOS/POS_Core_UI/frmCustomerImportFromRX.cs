using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using POS_Core.CommonData.Rows;
using POS_Core.CommonData;
using POS_Core.BusinessRules;
using POS_Core.ErrorLogging;
using Infragistics.Win.UltraWinEditors;
using System.Data;
using POS_Core.DataAccess;
//using POS_Core.DataAccess;
using NLog;
using POS_Core.Resources;

namespace POS_Core_UI
{
	/// <summary>
    /// Summary description for frmCustomerImportFromRX.
	/// </summary>
    public class frmCustomerImportFromRX : System.Windows.Forms.Form
    {
        private Infragistics.Win.Misc.UltraLabel lblTransactionType;
        private Infragistics.Win.Misc.UltraButton btnClose;
        private Infragistics.Win.Misc.UltraButton btnOk;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.RadioButton optAllPatients;
        private System.Windows.Forms.RadioButton optNewPatients;
        private RadioButton optSpecificPatient;
        private ProgressPanel progressPanel1;
        private System.ComponentModel.IContainer components;
        private bool IsInProgress;
        private static ILogger logger = LogManager.GetCurrentClassLogger();

        public frmCustomerImportFromRX()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();
            //
            // TODO: Add any constructor code after InitializeComponent call
            //
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmCustomerImportFromRX));
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            this.btnClose = new Infragistics.Win.Misc.UltraButton();
            this.btnOk = new Infragistics.Win.Misc.UltraButton();
            this.lblTransactionType = new Infragistics.Win.Misc.UltraLabel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.optSpecificPatient = new System.Windows.Forms.RadioButton();
            this.optNewPatients = new System.Windows.Forms.RadioButton();
            this.optAllPatients = new System.Windows.Forms.RadioButton();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.progressPanel1 = new ProgressPanel();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            appearance1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(62)))), ((int)(((byte)(176)))));
            appearance1.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance1.FontData.BoldAsString = "True";
            appearance1.ForeColor = System.Drawing.Color.White;
            appearance1.Image = ((object)(resources.GetObject("appearance1.Image")));
            this.btnClose.Appearance = appearance1;
            this.btnClose.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(488, 32);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(85, 26);
            this.btnClose.TabIndex = 7;
            this.btnClose.Text = "&Cancel";
            this.btnClose.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnOk
            // 
            this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            appearance2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(62)))), ((int)(((byte)(176)))));
            appearance2.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance2.FontData.BoldAsString = "True";
            appearance2.ForeColor = System.Drawing.Color.White;
            appearance2.Image = ((object)(resources.GetObject("appearance2.Image")));
            this.btnOk.Appearance = appearance2;
            this.btnOk.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            this.btnOk.Location = new System.Drawing.Point(396, 32);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(85, 26);
            this.btnOk.TabIndex = 6;
            this.btnOk.Text = "&Ok";
            this.btnOk.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnOk.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // lblTransactionType
            // 
            this.lblTransactionType.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            appearance3.ForeColor = System.Drawing.Color.White;
            appearance3.ForeColorDisabled = System.Drawing.Color.White;
            appearance3.TextHAlignAsString = "Center";
            this.lblTransactionType.Appearance = appearance3;
            this.lblTransactionType.BackColorInternal = System.Drawing.Color.Transparent;
            this.lblTransactionType.Font = new System.Drawing.Font("Arial", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTransactionType.Location = new System.Drawing.Point(10, 10);
            this.lblTransactionType.Name = "lblTransactionType";
            this.lblTransactionType.Size = new System.Drawing.Size(580, 40);
            this.lblTransactionType.TabIndex = 23;
            this.lblTransactionType.Tag = "Header";
            this.lblTransactionType.Text = "Import Patients From PrimeRX";
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.optSpecificPatient);
            this.groupBox1.Controls.Add(this.optNewPatients);
            this.groupBox1.Controls.Add(this.optAllPatients);
            this.groupBox1.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.groupBox1.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(10, 52);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(580, 61);
            this.groupBox1.TabIndex = 24;
            this.groupBox1.TabStop = false;
            // 
            // optSpecificPatient
            // 
            this.optSpecificPatient.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.optSpecificPatient.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.optSpecificPatient.ForeColor = System.Drawing.Color.White;
            this.optSpecificPatient.Location = new System.Drawing.Point(300, 24);
            this.optSpecificPatient.Name = "optSpecificPatient";
            this.optSpecificPatient.Size = new System.Drawing.Size(226, 25);
            this.optSpecificPatient.TabIndex = 2;
            this.optSpecificPatient.Text = "Specific Patient Only";
            // 
            // optNewPatients
            // 
            this.optNewPatients.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.optNewPatients.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.optNewPatients.ForeColor = System.Drawing.Color.White;
            this.optNewPatients.Location = new System.Drawing.Point(128, 24);
            this.optNewPatients.Name = "optNewPatients";
            this.optNewPatients.Size = new System.Drawing.Size(160, 25);
            this.optNewPatients.TabIndex = 1;
            this.optNewPatients.Text = "New Patients Only";
            // 
            // optAllPatients
            // 
            this.optAllPatients.Checked = true;
            this.optAllPatients.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.optAllPatients.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.optAllPatients.ForeColor = System.Drawing.Color.White;
            this.optAllPatients.Location = new System.Drawing.Point(13, 24);
            this.optAllPatients.Name = "optAllPatients";
            this.optAllPatients.Size = new System.Drawing.Size(128, 25);
            this.optAllPatients.TabIndex = 0;
            this.optAllPatients.TabStop = true;
            this.optAllPatients.Text = "All Patients";
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.progressPanel1);
            this.groupBox2.Controls.Add(this.btnClose);
            this.groupBox2.Controls.Add(this.btnOk);
            this.groupBox2.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.groupBox2.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.Location = new System.Drawing.Point(10, 113);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(580, 71);
            this.groupBox2.TabIndex = 25;
            this.groupBox2.TabStop = false;
            // 
            // progressPanel1
            // 
            this.progressPanel1.BackColor = System.Drawing.Color.WhiteSmoke;
            this.progressPanel1.Location = new System.Drawing.Point(2, 10);
            this.progressPanel1.Name = "progressPanel1";
            this.progressPanel1.Size = new System.Drawing.Size(338, 60);
            this.progressPanel1.TabIndex = 8;
            this.progressPanel1.Title = "Loading Data...";
            this.progressPanel1.Visible = false;
            // 
            // frmCustomerImportFromRX
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(108)))), ((int)(((byte)(172)))));
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(604, 189);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.lblTransactionType);
            this.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmCustomerImportFromRX";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Import Patients From PrimeRX";
            this.Activated += new System.EventHandler(this.frmCustomerImportFromRX_Activated);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmCustomerImportFromRX_FormClosing);
            this.Load += new System.EventHandler(this.frmCustomerImportFromRX_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmCustomerImportFromRX_KeyDown);
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);

        }
        #endregion
        private bool Save(CustomerData oCustomerData)
        {
            bool retVal = false;
            try
            {
                logger.Trace("Save(CustomerData oCustomerData) - " + clsPOSDBConstants.Log_Entering);
                Customer oCustomer = new Customer();
                oCustomer.DataRowSaved += new Customer.DataRowSavedHandler(oCustomer_DataRowSaved);
                oCustomer.Persist(oCustomerData,true);
                retVal = true;
                logger.Trace("Save(CustomerData oCustomerData) - " + clsPOSDBConstants.Log_Exiting);
            }
            catch (Exception exp)
            {
                logger.Fatal(exp, "Save(CustomerData oCustomerData)");
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
            return retVal;
        }

        void oCustomer_DataRowSaved()
        {
            Application.DoEvents();
        }

        private void btnSave_Click(object sender, System.EventArgs e)
        {
            Timer timer = new Timer();
            timer.Tick += new EventHandler(timer_Tick);
            timer.Enabled = true;
            timer.Start();
        }

        void timer_Tick(object sender, EventArgs e)
        {
            try
            {
                ((Timer)sender).Enabled = false;
                IsInProgress = true;

                progressPanel1.Title = "Loading Data...";
                progressPanel1.Visible = true;
                progressPanel1.Start();
                Application.DoEvents();

                this.Cursor = Cursors.WaitCursor;
                DataSet oDS = GetPatientDataFromPharmacy();

                // following if is Added by shitaljit on 14 march 2012
                //if ODs is not null then only do actions to avoid error from data not available
               
                if (oDS != null)
                {
                    progressPanel1.Title = "Preparing for Save...";
                    Application.DoEvents();
                    Customer oCustomer = new Customer();
                    CustomerData oCustomerData = oCustomer.CreateCustomerDSFromPatientDS(oDS, optNewPatients.Checked);


                    if (oCustomerData.Customer.Rows.Count == 0)
                    {
                        POS_Core_UI.Resources.Message.Display(this, "No data found.", "PrimePOS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else if (POS_Core_UI.Resources.Message.Display(this, "This action will override any existing data. Are you sure to continue?", "PrimePOS", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        Application.DoEvents();
                        progressPanel1.Title = "Saving Data...";
                        if (Save(oCustomerData))
                        {
                            IsInProgress = false;
                            POS_Core_UI.Resources.Message.Display(this, "Patients imported successfully.", "PrimePOS", MessageBoxButtons.OK, MessageBoxIcon.Question);
                            this.DialogResult = DialogResult.OK;
                            this.Close();
                        }
                    }
                }
            }
            finally
            {
                IsInProgress = false;
                progressPanel1.Stop();
                progressPanel1.Visible = false;
                this.Cursor = Cursors.Default;
            }
        
        }

        private void btnClose_Click(object sender, System.EventArgs e)
        {
            this.Close();
        }

        private DataSet GetPatientDataFromPharmacy()
        {
            DataSet oDS = null;

            if (optSpecificPatient.Checked == true)
            {
                oDS = GetSelectedPatientFromPharmacy();
            }
            else if(optAllPatients.Checked==true)
            {
                oDS=GetAllPatientsFromPharmacy();
            }
            else if (optNewPatients.Checked == true)
            {
                oDS = GetAllPatientsFromPharmacy();
            }

            return oDS;
        }

        private DataSet GetAllPatientsFromPharmacy()
        {
            MMSChargeAccount.ContAccount oAcct = new MMSChargeAccount.ContAccount();
            return oAcct.GetAllPatients();
            
        }

        private DataSet GetSelectedPatientFromPharmacy()
        {
            DataSet oDS = null;

            //frmSearch oSearch = null;
            frmSearchMain oSearch = null;

            //oSearch = new frmSearch(clsPOSDBConstants.PrimeRX_PatientInterface, "", "");
            oSearch = new frmSearchMain(clsPOSDBConstants.PrimeRX_PatientInterface, "", "", true);
            oSearch.ShowDialog(this);
            if (!oSearch.IsCanceled)
            {
                MMSChargeAccount.ContAccount oAcct = new MMSChargeAccount.ContAccount();
                    
                int iPatientNo = Configuration.convertNullToInt(oSearch.SelectedRowID());
                
                //Added by shitaljit on 14 March to handle table[0] not found error
                //on ok botton click with blank data.
                
                if (iPatientNo > 0)
                {
                    oAcct.GetPatientByCode(iPatientNo, out oDS);
                }
                else
                {
                    oDS = null;
                }
            }
            return oDS;
        }

        private void frmCustomerImportFromRX_Load(object sender, System.EventArgs e)
        {
            clsUIHelper.setColorSchecme(this);
        }

        private void frmCustomerImportFromRX_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
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
                logger.Fatal(exp, "frmCustomerImportFromRX_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)");
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }

        private void frmCustomerImportFromRX_Activated(object sender, System.EventArgs e)
        {
            clsUIHelper.CurrentForm = this;
        }

        private void frmCustomerImportFromRX_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = IsInProgress;
        }

    }
}
