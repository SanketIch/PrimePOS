using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Forms;
using MMSChargeAccount;
using System.Data;
//using POS_Core.DataAccess;
using NLog;
using POS_Core.Resources;
using POS_Core.Resources.DelegateHandler;
using POS_Core.CommonData;
using POS_Core.BusinessRules;

namespace POS_Core_UI
{
    /// <summary>
    /// Summary description for frmHouseChargeConfirmation.
    /// </summary>
    public class frmHouseChargePayment : System.Windows.Forms.Form
    {
        private string AccountNo;
        public static string ROARefference = "";
        private Infragistics.Win.Misc.UltraLabel lblTransactionType;
        private Infragistics.Win.Misc.UltraButton btnReject;
        private Infragistics.Win.Misc.UltraButton btnAccept;
        private System.Windows.Forms.GroupBox groupBox1;
        private Infragistics.Win.Misc.UltraLabel ultraLabel14;
        private Infragistics.Win.Misc.UltraLabel ultraLabel12;
        private Infragistics.Win.Misc.UltraLabel ultraLabel9;
        private Infragistics.Win.Misc.UltraLabel ultraLabel8;
        private Infragistics.Win.Misc.UltraLabel ultraLabel3;
        private Infragistics.Win.Misc.UltraLabel ultraLabel2;
        private Infragistics.Win.Misc.UltraLabel ultraLabel1;
        private System.Windows.Forms.GroupBox groupBox2;
        private Infragistics.Win.Misc.UltraLabel lblDiscount;
        private Infragistics.Win.Misc.UltraLabel lblCreditLimit;
        private Infragistics.Win.Misc.UltraLabel lblBalance;
        private Infragistics.Win.Misc.UltraLabel lblAccountNo;
        private Infragistics.Win.Misc.UltraLabel lblPhone;
        public Infragistics.Win.Misc.UltraLabel lblName;
        private Infragistics.Win.Misc.UltraLabel lblAddress;
        private Infragistics.Win.Misc.UltraLabel ultraLabel5;
        private Infragistics.Win.Misc.UltraLabel ultraLabel7;
        private Infragistics.Win.Misc.UltraLabel ultraLabel11;
        private Infragistics.Win.Misc.UltraLabel lblLastStatBal;
        private Infragistics.Win.Misc.UltraLabel lblLastStatDate;
        private Infragistics.Win.Misc.UltraLabel lblComments;
        private Infragistics.Win.Misc.UltraLabel lblAmount;
        public Infragistics.Win.UltraWinEditors.UltraNumericEditor txtAmount;
        private DataSet oDS;
        private Infragistics.Win.Misc.UltraLabel lblRefference;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor txtROARefference;
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;
        internal Infragistics.Win.UltraWinGrid.UltraGrid grdPatients;
        private Infragistics.Win.Misc.UltraLabel lblPatientName;
        private Infragistics.Win.Misc.UltraLabel lblPatient;
        private static ILogger logger = LogManager.GetCurrentClassLogger();
        public int PatientNo = 0;
        private int nCustomerID = 0;

        public frmHouseChargePayment(string AcctNo, Decimal AmountToReturn, int CustomerID)
        {
            //
            // Required for Windows Form Designer support
            //
            if (AcctNo.Trim() == "")
                throw (new Exception("Invalid Account No. selected."));
            InitializeComponent();
            AccountNo = AcctNo;
            nCustomerID = CustomerID; //PRIMEPOS-2570 06-Jul-2020 JY Added
            clsUIHelper.setColorSchecme(this);  //PRIMEPOS-2570 02-Jul-2020 JY Added            
            PopulatePatientsLinkedWithHCAccount(Configuration.convertNullToInt64(AccountNo));  //PRIMEPOS-2570 02-Jul-2020 JY Added            

            ContAccount oAcct = new ContAccount();
            oDS = new DataSet();
            oAcct.GetAccountByCode(AcctNo, out oDS, true);
            if (oDS == null)
                throw (new Exception("Invalid account no selected."));
            else if (oDS.Tables[0].Rows.Count == 0)
                throw (new Exception("Invalid account no selected."));
            else
            {
                this.lblAccountNo.Text = oDS.Tables[0].Rows[0]["acct_no"].ToString();
                this.lblAddress.Text = oDS.Tables[0].Rows[0]["address1"].ToString();
                this.lblCreditLimit.Text = POS_Core.Resources.Configuration.convertNullToDecimal(oDS.Tables[0].Rows[0]["credit_lmt"].ToString()).ToString("c");
                this.lblDiscount.Text = oDS.Tables[0].Rows[0]["discount"].ToString();
                this.lblName.Text = oDS.Tables[0].Rows[0]["acct_Name"].ToString();
                this.lblPhone.Text = oDS.Tables[0].Rows[0]["phone_no"].ToString();
                this.lblBalance.Text = oAcct.GetAccountBalance(Convert.ToInt32(this.lblAccountNo.Text)).ToString("c");
                this.lblLastStatBal.Text = "";
                this.lblLastStatDate.Text = "";
                this.lblComments.Text = oDS.Tables[0].Rows[0]["comment"].ToString();
            }
            if (Math.Abs(AmountToReturn) > 0)
            {
                this.lblTransactionType.Text = "RETURN RECEIVE ON ACCOUNT";
                this.lblAmount.Text = "Enter Amount To Return";
                this.Text = "Return Receive On Account";
                this.txtAmount.Value = AmountToReturn;
                this.txtAmount.Enabled = false;
            }
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
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmHouseChargePayment));
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance7 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance8 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand1 = new Infragistics.Win.UltraWinGrid.UltraGridBand("Band 0", -1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn1 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Account#");
            Infragistics.Win.Appearance appearance9 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn2 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Name");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn3 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Address1");
            Infragistics.Win.Appearance appearance10 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance11 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance12 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance13 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance14 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance15 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance16 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance17 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance18 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance19 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance20 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance21 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance22 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance23 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance24 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance25 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance26 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance27 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance28 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance29 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance30 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinScrollBar.ScrollBarLook scrollBarLook1 = new Infragistics.Win.UltraWinScrollBar.ScrollBarLook();
            Infragistics.Win.Appearance appearance31 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance32 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance33 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance34 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            this.lblTransactionType = new Infragistics.Win.Misc.UltraLabel();
            this.btnReject = new Infragistics.Win.Misc.UltraButton();
            this.btnAccept = new Infragistics.Win.Misc.UltraButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lblAccountNo = new Infragistics.Win.Misc.UltraLabel();
            this.lblName = new Infragistics.Win.Misc.UltraLabel();
            this.lblAddress = new Infragistics.Win.Misc.UltraLabel();
            this.lblComments = new Infragistics.Win.Misc.UltraLabel();
            this.lblLastStatBal = new Infragistics.Win.Misc.UltraLabel();
            this.lblBalance = new Infragistics.Win.Misc.UltraLabel();
            this.lblPhone = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabel11 = new Infragistics.Win.Misc.UltraLabel();
            this.lblLastStatDate = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabel7 = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabel5 = new Infragistics.Win.Misc.UltraLabel();
            this.lblDiscount = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabel14 = new Infragistics.Win.Misc.UltraLabel();
            this.lblCreditLimit = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabel12 = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabel9 = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabel8 = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabel3 = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabel2 = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabel1 = new Infragistics.Win.Misc.UltraLabel();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.txtROARefference = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.lblRefference = new Infragistics.Win.Misc.UltraLabel();
            this.txtAmount = new Infragistics.Win.UltraWinEditors.UltraNumericEditor();
            this.lblAmount = new Infragistics.Win.Misc.UltraLabel();
            this.grdPatients = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.lblPatient = new Infragistics.Win.Misc.UltraLabel();
            this.lblPatientName = new Infragistics.Win.Misc.UltraLabel();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtROARefference)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtAmount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdPatients)).BeginInit();
            this.SuspendLayout();
            // 
            // lblTransactionType
            // 
            appearance1.ForeColor = System.Drawing.Color.White;
            appearance1.ForeColorDisabled = System.Drawing.Color.Navy;
            appearance1.TextHAlignAsString = "Left";
            this.lblTransactionType.Appearance = appearance1;
            this.lblTransactionType.BackColorInternal = System.Drawing.Color.Transparent;
            this.lblTransactionType.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblTransactionType.Font = new System.Drawing.Font("Verdana", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTransactionType.Location = new System.Drawing.Point(0, 0);
            this.lblTransactionType.Name = "lblTransactionType";
            this.lblTransactionType.Size = new System.Drawing.Size(754, 30);
            this.lblTransactionType.TabIndex = 64;
            this.lblTransactionType.Tag = "Header";
            this.lblTransactionType.Text = "RECEIVE ON ACCOUNT";
            // 
            // btnReject
            // 
            appearance2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance2.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(108)))), ((int)(((byte)(172)))));
            appearance2.FontData.BoldAsString = "True";
            appearance2.ForeColor = System.Drawing.Color.White;
            appearance2.Image = ((object)(resources.GetObject("appearance2.Image")));
            this.btnReject.Appearance = appearance2;
            this.btnReject.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            this.btnReject.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnReject.Location = new System.Drawing.Point(631, 40);
            this.btnReject.Name = "btnReject";
            this.btnReject.Size = new System.Drawing.Size(88, 28);
            this.btnReject.TabIndex = 3;
            this.btnReject.Text = "&Cancel";
            this.btnReject.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnReject.Click += new System.EventHandler(this.btnReject_Click);
            // 
            // btnAccept
            // 
            appearance3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance3.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(108)))), ((int)(((byte)(172)))));
            appearance3.FontData.BoldAsString = "True";
            appearance3.ForeColor = System.Drawing.Color.White;
            appearance3.Image = ((object)(resources.GetObject("appearance3.Image")));
            this.btnAccept.Appearance = appearance3;
            this.btnAccept.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            this.btnAccept.Location = new System.Drawing.Point(535, 40);
            this.btnAccept.Name = "btnAccept";
            this.btnAccept.Size = new System.Drawing.Size(88, 28);
            this.btnAccept.TabIndex = 2;
            this.btnAccept.Text = "&Ok";
            this.btnAccept.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnAccept.Click += new System.EventHandler(this.btnAccept_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lblPatientName);
            this.groupBox1.Controls.Add(this.lblPatient);
            this.groupBox1.Controls.Add(this.lblAccountNo);
            this.groupBox1.Controls.Add(this.lblName);
            this.groupBox1.Controls.Add(this.lblAddress);
            this.groupBox1.Controls.Add(this.lblComments);
            this.groupBox1.Controls.Add(this.lblLastStatBal);
            this.groupBox1.Controls.Add(this.lblBalance);
            this.groupBox1.Controls.Add(this.lblPhone);
            this.groupBox1.Controls.Add(this.ultraLabel11);
            this.groupBox1.Controls.Add(this.lblLastStatDate);
            this.groupBox1.Controls.Add(this.ultraLabel7);
            this.groupBox1.Controls.Add(this.ultraLabel5);
            this.groupBox1.Controls.Add(this.lblDiscount);
            this.groupBox1.Controls.Add(this.ultraLabel14);
            this.groupBox1.Controls.Add(this.lblCreditLimit);
            this.groupBox1.Controls.Add(this.ultraLabel12);
            this.groupBox1.Controls.Add(this.ultraLabel9);
            this.groupBox1.Controls.Add(this.ultraLabel8);
            this.groupBox1.Controls.Add(this.ultraLabel3);
            this.groupBox1.Controls.Add(this.ultraLabel2);
            this.groupBox1.Controls.Add(this.ultraLabel1);
            this.groupBox1.Location = new System.Drawing.Point(10, 140);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(735, 210);
            this.groupBox1.TabIndex = 71;
            this.groupBox1.TabStop = false;
            // 
            // lblAccountNo
            // 
            this.lblAccountNo.Font = new System.Drawing.Font("Arial", 13F, System.Drawing.FontStyle.Bold);
            this.lblAccountNo.Location = new System.Drawing.Point(130, 14);
            this.lblAccountNo.Name = "lblAccountNo";
            this.lblAccountNo.Size = new System.Drawing.Size(173, 23);
            this.lblAccountNo.TabIndex = 75;
            this.lblAccountNo.Text = "ultraLabel7";
            // 
            // lblName
            // 
            this.lblName.Font = new System.Drawing.Font("Arial", 13F, System.Drawing.FontStyle.Bold);
            this.lblName.Location = new System.Drawing.Point(130, 43);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(503, 23);
            this.lblName.TabIndex = 73;
            this.lblName.Text = "ultraLabel5";
            // 
            // lblAddress
            // 
            this.lblAddress.Font = new System.Drawing.Font("Arial", 13F, System.Drawing.FontStyle.Bold);
            this.lblAddress.Location = new System.Drawing.Point(130, 72);
            this.lblAddress.Name = "lblAddress";
            this.lblAddress.Size = new System.Drawing.Size(503, 23);
            this.lblAddress.TabIndex = 72;
            this.lblAddress.Text = "ultraLabel4";
            // 
            // lblComments
            // 
            this.lblComments.Font = new System.Drawing.Font("Arial", 13F, System.Drawing.FontStyle.Bold);
            this.lblComments.Location = new System.Drawing.Point(130, 183);
            this.lblComments.Name = "lblComments";
            this.lblComments.Size = new System.Drawing.Size(500, 23);
            this.lblComments.TabIndex = 88;
            this.lblComments.Text = "ultraLabel10";
            // 
            // lblLastStatBal
            // 
            this.lblLastStatBal.Font = new System.Drawing.Font("Arial", 13F, System.Drawing.FontStyle.Bold);
            this.lblLastStatBal.Location = new System.Drawing.Point(130, 157);
            this.lblLastStatBal.Name = "lblLastStatBal";
            this.lblLastStatBal.Size = new System.Drawing.Size(173, 23);
            this.lblLastStatBal.TabIndex = 84;
            this.lblLastStatBal.Text = "ultraLabel10";
            // 
            // lblBalance
            // 
            this.lblBalance.Font = new System.Drawing.Font("Arial", 13F, System.Drawing.FontStyle.Bold);
            this.lblBalance.Location = new System.Drawing.Point(130, 130);
            this.lblBalance.Name = "lblBalance";
            this.lblBalance.Size = new System.Drawing.Size(173, 23);
            this.lblBalance.TabIndex = 78;
            this.lblBalance.Text = "ultraLabel10";
            // 
            // lblPhone
            // 
            this.lblPhone.Font = new System.Drawing.Font("Arial", 13F, System.Drawing.FontStyle.Bold);
            this.lblPhone.Location = new System.Drawing.Point(130, 101);
            this.lblPhone.Name = "lblPhone";
            this.lblPhone.Size = new System.Drawing.Size(173, 23);
            this.lblPhone.TabIndex = 74;
            this.lblPhone.Text = "ultraLabel6";
            // 
            // ultraLabel11
            // 
            this.ultraLabel11.Font = new System.Drawing.Font("Arial", 13F, System.Drawing.FontStyle.Bold);
            this.ultraLabel11.Location = new System.Drawing.Point(5, 183);
            this.ultraLabel11.Name = "ultraLabel11";
            this.ultraLabel11.Size = new System.Drawing.Size(119, 23);
            this.ultraLabel11.TabIndex = 87;
            this.ultraLabel11.Text = "Comments";
            // 
            // lblLastStatDate
            // 
            this.lblLastStatDate.Font = new System.Drawing.Font("Arial", 13F, System.Drawing.FontStyle.Bold);
            this.lblLastStatDate.Location = new System.Drawing.Point(462, 156);
            this.lblLastStatDate.Name = "lblLastStatDate";
            this.lblLastStatDate.Size = new System.Drawing.Size(173, 23);
            this.lblLastStatDate.TabIndex = 86;
            this.lblLastStatDate.Text = "ultraLabel10";
            // 
            // ultraLabel7
            // 
            this.ultraLabel7.Font = new System.Drawing.Font("Arial", 13F, System.Drawing.FontStyle.Bold);
            this.ultraLabel7.Location = new System.Drawing.Point(318, 157);
            this.ultraLabel7.Name = "ultraLabel7";
            this.ultraLabel7.Size = new System.Drawing.Size(133, 23);
            this.ultraLabel7.TabIndex = 85;
            this.ultraLabel7.Text = "Last Stat. Date";
            // 
            // ultraLabel5
            // 
            this.ultraLabel5.Font = new System.Drawing.Font("Arial", 13F, System.Drawing.FontStyle.Bold);
            this.ultraLabel5.Location = new System.Drawing.Point(5, 157);
            this.ultraLabel5.Name = "ultraLabel5";
            this.ultraLabel5.Size = new System.Drawing.Size(119, 23);
            this.ultraLabel5.TabIndex = 83;
            this.ultraLabel5.Text = "Last Stat. Bal";
            // 
            // lblDiscount
            // 
            this.lblDiscount.Font = new System.Drawing.Font("Arial", 13F, System.Drawing.FontStyle.Bold);
            this.lblDiscount.Location = new System.Drawing.Point(462, 101);
            this.lblDiscount.Name = "lblDiscount";
            this.lblDiscount.Size = new System.Drawing.Size(173, 23);
            this.lblDiscount.TabIndex = 82;
            this.lblDiscount.Text = "ultraLabel13";
            // 
            // ultraLabel14
            // 
            this.ultraLabel14.Font = new System.Drawing.Font("Arial", 13F, System.Drawing.FontStyle.Bold);
            this.ultraLabel14.Location = new System.Drawing.Point(318, 101);
            this.ultraLabel14.Name = "ultraLabel14";
            this.ultraLabel14.Size = new System.Drawing.Size(133, 23);
            this.ultraLabel14.TabIndex = 81;
            this.ultraLabel14.Text = "Discount";
            // 
            // lblCreditLimit
            // 
            this.lblCreditLimit.Font = new System.Drawing.Font("Arial", 13F, System.Drawing.FontStyle.Bold);
            this.lblCreditLimit.Location = new System.Drawing.Point(462, 130);
            this.lblCreditLimit.Name = "lblCreditLimit";
            this.lblCreditLimit.Size = new System.Drawing.Size(173, 23);
            this.lblCreditLimit.TabIndex = 80;
            this.lblCreditLimit.Text = "ultraLabel11";
            // 
            // ultraLabel12
            // 
            this.ultraLabel12.Font = new System.Drawing.Font("Arial", 13F, System.Drawing.FontStyle.Bold);
            this.ultraLabel12.Location = new System.Drawing.Point(318, 130);
            this.ultraLabel12.Name = "ultraLabel12";
            this.ultraLabel12.Size = new System.Drawing.Size(133, 23);
            this.ultraLabel12.TabIndex = 79;
            this.ultraLabel12.Text = "Credit Limit";
            // 
            // ultraLabel9
            // 
            this.ultraLabel9.Font = new System.Drawing.Font("Arial", 13F, System.Drawing.FontStyle.Bold);
            this.ultraLabel9.Location = new System.Drawing.Point(5, 72);
            this.ultraLabel9.Name = "ultraLabel9";
            this.ultraLabel9.Size = new System.Drawing.Size(119, 23);
            this.ultraLabel9.TabIndex = 77;
            this.ultraLabel9.Text = "Address";
            // 
            // ultraLabel8
            // 
            this.ultraLabel8.Font = new System.Drawing.Font("Arial", 13F, System.Drawing.FontStyle.Bold);
            this.ultraLabel8.Location = new System.Drawing.Point(5, 43);
            this.ultraLabel8.Name = "ultraLabel8";
            this.ultraLabel8.Size = new System.Drawing.Size(119, 23);
            this.ultraLabel8.TabIndex = 76;
            this.ultraLabel8.Text = "Name";
            // 
            // ultraLabel3
            // 
            this.ultraLabel3.Font = new System.Drawing.Font("Arial", 13F, System.Drawing.FontStyle.Bold);
            this.ultraLabel3.Location = new System.Drawing.Point(5, 101);
            this.ultraLabel3.Name = "ultraLabel3";
            this.ultraLabel3.Size = new System.Drawing.Size(119, 23);
            this.ultraLabel3.TabIndex = 71;
            this.ultraLabel3.Text = "Phone";
            // 
            // ultraLabel2
            // 
            this.ultraLabel2.Font = new System.Drawing.Font("Arial", 13F, System.Drawing.FontStyle.Bold);
            this.ultraLabel2.Location = new System.Drawing.Point(5, 130);
            this.ultraLabel2.Name = "ultraLabel2";
            this.ultraLabel2.Size = new System.Drawing.Size(119, 23);
            this.ultraLabel2.TabIndex = 70;
            this.ultraLabel2.Text = "Balance";
            // 
            // ultraLabel1
            // 
            this.ultraLabel1.Font = new System.Drawing.Font("Arial", 13F, System.Drawing.FontStyle.Bold);
            this.ultraLabel1.Location = new System.Drawing.Point(5, 14);
            this.ultraLabel1.Name = "ultraLabel1";
            this.ultraLabel1.Size = new System.Drawing.Size(119, 23);
            this.ultraLabel1.TabIndex = 69;
            this.ultraLabel1.Text = "Acct #";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.txtROARefference);
            this.groupBox2.Controls.Add(this.lblRefference);
            this.groupBox2.Controls.Add(this.txtAmount);
            this.groupBox2.Controls.Add(this.lblAmount);
            this.groupBox2.Controls.Add(this.btnReject);
            this.groupBox2.Controls.Add(this.btnAccept);
            this.groupBox2.Location = new System.Drawing.Point(10, 350);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(735, 77);
            this.groupBox2.TabIndex = 72;
            this.groupBox2.TabStop = false;
            // 
            // txtROARefference
            // 
            this.txtROARefference.AutoSize = false;
            this.txtROARefference.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtROARefference.Location = new System.Drawing.Point(185, 43);
            this.txtROARefference.MaxLength = 40; //PRIMEPOS-3471
            this.txtROARefference.Name = "txtROARefference";
            this.txtROARefference.Size = new System.Drawing.Size(340, 22);
            this.txtROARefference.TabIndex = 1;
            this.txtROARefference.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            // 
            // lblRefference
            // 
            appearance6.FontData.BoldAsString = "True";
            appearance6.FontData.SizeInPoints = 10F;
            appearance6.ForeColor = System.Drawing.Color.White;
            appearance6.ForeColorDisabled = System.Drawing.Color.Black;
            this.lblRefference.Appearance = appearance6;
            this.lblRefference.Font = new System.Drawing.Font("Arial", 12.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRefference.Location = new System.Drawing.Point(5, 40);
            this.lblRefference.Name = "lblRefference";
            this.lblRefference.Size = new System.Drawing.Size(91, 18);
            this.lblRefference.TabIndex = 72;
            this.lblRefference.Text = "Reference";
            this.lblRefference.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // txtAmount
            // 
            this.txtAmount.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.txtAmount.DisplayStyle = Infragistics.Win.EmbeddableElementDisplayStyle.OfficeXP;
            this.txtAmount.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtAmount.Location = new System.Drawing.Point(188, 14);
            this.txtAmount.MaskInput = "nn,nnn.nn";
            this.txtAmount.MaxValue = 99999.99D;
            this.txtAmount.MinValue = 0;
            this.txtAmount.Name = "txtAmount";
            this.txtAmount.NullText = "0.00";
            this.txtAmount.NumericType = Infragistics.Win.UltraWinEditors.NumericType.Double;
            this.txtAmount.Size = new System.Drawing.Size(151, 22);
            this.txtAmount.SpinButtonDisplayStyle = Infragistics.Win.ButtonDisplayStyle.Always;
            this.txtAmount.SpinWrap = true;
            this.txtAmount.TabIndex = 0;
            this.txtAmount.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.txtAmount.Enter += new System.EventHandler(this.txtAmount_Enter);
            // 
            // lblAmount
            // 
            appearance7.FontData.BoldAsString = "True";
            appearance7.FontData.SizeInPoints = 10F;
            appearance7.ForeColor = System.Drawing.Color.White;
            appearance7.ForeColorDisabled = System.Drawing.Color.Black;
            this.lblAmount.Appearance = appearance7;
            this.lblAmount.Font = new System.Drawing.Font("Arial", 12.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAmount.Location = new System.Drawing.Point(5, 15);
            this.lblAmount.Name = "lblAmount";
            this.lblAmount.Size = new System.Drawing.Size(177, 18);
            this.lblAmount.TabIndex = 71;
            this.lblAmount.Text = "Enter Amount To Pay";
            this.lblAmount.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // grdPatients
            // 
            appearance8.BackColor = System.Drawing.Color.White;
            appearance8.BackColor2 = System.Drawing.Color.White;
            appearance8.BackColorDisabled = System.Drawing.Color.White;
            appearance8.BackColorDisabled2 = System.Drawing.Color.White;
            this.grdPatients.DisplayLayout.Appearance = appearance8;
            ultraGridColumn1.Header.VisiblePosition = 0;
            appearance9.BackColor = System.Drawing.Color.Gray;
            ultraGridColumn1.MergedCellAppearance = appearance9;
            ultraGridColumn2.Header.VisiblePosition = 1;
            ultraGridColumn3.Header.VisiblePosition = 2;
            ultraGridBand1.Columns.AddRange(new object[] {
            ultraGridColumn1,
            ultraGridColumn2,
            ultraGridColumn3});
            appearance10.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(102)))), ((int)(((byte)(127)))));
            appearance10.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(102)))), ((int)(((byte)(127)))));
            ultraGridBand1.Header.Appearance = appearance10;
            ultraGridBand1.SummaryFooterCaption = "";
            this.grdPatients.DisplayLayout.BandsSerializer.Add(ultraGridBand1);
            this.grdPatients.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance11.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(102)))), ((int)(((byte)(127)))));
            appearance11.FontData.BoldAsString = "True";
            appearance11.FontData.SizeInPoints = 9F;
            appearance11.ForeColor = System.Drawing.Color.White;
            appearance11.TextHAlignAsString = "Left";
            this.grdPatients.DisplayLayout.CaptionAppearance = appearance11;
            this.grdPatients.DisplayLayout.InterBandSpacing = 10;
            this.grdPatients.DisplayLayout.MaxColScrollRegions = 1;
            this.grdPatients.DisplayLayout.MaxRowScrollRegions = 1;
            appearance12.BackColor = System.Drawing.Color.White;
            appearance12.BackColor2 = System.Drawing.Color.White;
            this.grdPatients.DisplayLayout.Override.ActiveCardCaptionAppearance = appearance12;
            appearance13.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
            appearance13.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
            appearance13.BorderColor = System.Drawing.Color.Gray;
            this.grdPatients.DisplayLayout.Override.ActiveRowAppearance = appearance13;
            appearance14.BackColor = System.Drawing.Color.White;
            appearance14.BackColor2 = System.Drawing.Color.White;
            appearance14.BorderColor = System.Drawing.Color.Gray;
            this.grdPatients.DisplayLayout.Override.AddRowAppearance = appearance14;
            this.grdPatients.DisplayLayout.Override.AllowAddNew = Infragistics.Win.UltraWinGrid.AllowAddNew.No;
            this.grdPatients.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.False;
            this.grdPatients.DisplayLayout.Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.False;
            appearance15.BackColor = System.Drawing.Color.Transparent;
            this.grdPatients.DisplayLayout.Override.CardAreaAppearance = appearance15;
            appearance16.BackColor = System.Drawing.Color.White;
            appearance16.BackColor2 = System.Drawing.Color.White;
            appearance16.BackColorDisabled = System.Drawing.Color.White;
            appearance16.BackColorDisabled2 = System.Drawing.Color.White;
            appearance16.BorderColor = System.Drawing.Color.Black;
            appearance16.BorderColor3DBase = System.Drawing.Color.Black;
            this.grdPatients.DisplayLayout.Override.CellAppearance = appearance16;
            appearance17.BackColor = System.Drawing.Color.White;
            appearance17.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(231)))), ((int)(((byte)(230)))));
            appearance17.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance17.BorderAlpha = Infragistics.Win.Alpha.Transparent;
            appearance17.BorderColor = System.Drawing.Color.Gray;
            appearance17.BorderColor3DBase = System.Drawing.Color.Gray;
            appearance17.ImageBackgroundAlpha = Infragistics.Win.Alpha.Transparent;
            appearance17.ImageBackgroundStyle = Infragistics.Win.ImageBackgroundStyle.Centered;
            this.grdPatients.DisplayLayout.Override.CellButtonAppearance = appearance17;
            this.grdPatients.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;
            appearance18.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            appearance18.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.grdPatients.DisplayLayout.Override.EditCellAppearance = appearance18;
            appearance19.BorderColor = System.Drawing.Color.Gray;
            this.grdPatients.DisplayLayout.Override.FilteredInRowAppearance = appearance19;
            appearance20.BorderColor = System.Drawing.Color.Gray;
            this.grdPatients.DisplayLayout.Override.FilteredOutRowAppearance = appearance20;
            appearance21.BackColor = System.Drawing.Color.White;
            appearance21.BackColor2 = System.Drawing.Color.White;
            appearance21.BackColorDisabled = System.Drawing.Color.White;
            appearance21.BackColorDisabled2 = System.Drawing.Color.White;
            this.grdPatients.DisplayLayout.Override.FixedCellAppearance = appearance21;
            appearance22.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            appearance22.BackColor2 = System.Drawing.Color.Beige;
            this.grdPatients.DisplayLayout.Override.FixedHeaderAppearance = appearance22;
            appearance23.BackColor = System.Drawing.Color.White;
            appearance23.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            appearance23.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance23.FontData.BoldAsString = "True";
            appearance23.FontData.SizeInPoints = 9F;
            appearance23.ForeColor = System.Drawing.Color.Black;
            appearance23.TextHAlignAsString = "Left";
            appearance23.ThemedElementAlpha = Infragistics.Win.Alpha.Transparent;
            this.grdPatients.DisplayLayout.Override.HeaderAppearance = appearance23;
            this.grdPatients.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortSingle;
            appearance24.BackColor = System.Drawing.Color.LightCyan;
            appearance24.BorderColor = System.Drawing.Color.Gray;
            this.grdPatients.DisplayLayout.Override.RowAlternateAppearance = appearance24;
            appearance25.BackColor = System.Drawing.Color.White;
            appearance25.BackColor2 = System.Drawing.Color.White;
            appearance25.BackGradientStyle = Infragistics.Win.GradientStyle.None;
            appearance25.BackHatchStyle = Infragistics.Win.BackHatchStyle.None;
            appearance25.BorderColor = System.Drawing.Color.Gray;
            this.grdPatients.DisplayLayout.Override.RowAppearance = appearance25;
            appearance26.BorderColor = System.Drawing.Color.Gray;
            this.grdPatients.DisplayLayout.Override.RowPreviewAppearance = appearance26;
            appearance27.BackColor = System.Drawing.Color.White;
            appearance27.BackColor2 = System.Drawing.SystemColors.Control;
            appearance27.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance27.BorderColor = System.Drawing.Color.Gray;
            this.grdPatients.DisplayLayout.Override.RowSelectorAppearance = appearance27;
            this.grdPatients.DisplayLayout.Override.RowSelectorWidth = 12;
            this.grdPatients.DisplayLayout.Override.RowSpacingBefore = 2;
            appearance28.BackColor = System.Drawing.Color.Navy;
            appearance28.BackColorAlpha = Infragistics.Win.Alpha.Opaque;
            this.grdPatients.DisplayLayout.Override.SelectedCellAppearance = appearance28;
            appearance29.BackColor = System.Drawing.Color.Navy;
            appearance29.BackColorDisabled = System.Drawing.Color.Navy;
            appearance29.BackGradientStyle = Infragistics.Win.GradientStyle.None;
            appearance29.BackHatchStyle = Infragistics.Win.BackHatchStyle.None;
            appearance29.BorderColor = System.Drawing.Color.Gray;
            appearance29.ForeColor = System.Drawing.Color.Black;
            this.grdPatients.DisplayLayout.Override.SelectedRowAppearance = appearance29;
            this.grdPatients.DisplayLayout.Override.SelectTypeCell = Infragistics.Win.UltraWinGrid.SelectType.None;
            this.grdPatients.DisplayLayout.Override.SelectTypeCol = Infragistics.Win.UltraWinGrid.SelectType.None;
            this.grdPatients.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.Single;
            appearance30.BorderColor = System.Drawing.Color.Gray;
            this.grdPatients.DisplayLayout.Override.TemplateAddRowAppearance = appearance30;
            this.grdPatients.DisplayLayout.RowConnectorColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            this.grdPatients.DisplayLayout.RowConnectorStyle = Infragistics.Win.UltraWinGrid.RowConnectorStyle.Solid;
            appearance31.BackColor = System.Drawing.Color.White;
            appearance31.BackColor2 = System.Drawing.SystemColors.Control;
            appearance31.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            scrollBarLook1.Appearance = appearance31;
            appearance32.BackColor = System.Drawing.Color.White;
            appearance32.BackColor2 = System.Drawing.SystemColors.Control;
            appearance32.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance32.ThemedElementAlpha = Infragistics.Win.Alpha.Transparent;
            scrollBarLook1.ButtonAppearance = appearance32;
            appearance33.BackColor = System.Drawing.Color.White;
            appearance33.BackColor2 = System.Drawing.SystemColors.Control;
            scrollBarLook1.ThumbAppearance = appearance33;
            appearance34.BackColor = System.Drawing.Color.White;
            appearance34.BackColor2 = System.Drawing.Color.White;
            scrollBarLook1.TrackAppearance = appearance34;
            this.grdPatients.DisplayLayout.ScrollBarLook = scrollBarLook1;
            this.grdPatients.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grdPatients.Location = new System.Drawing.Point(10, 35);
            this.grdPatients.Name = "grdPatients";
            this.grdPatients.Size = new System.Drawing.Size(735, 100);
            this.grdPatients.TabIndex = 11;
            this.grdPatients.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.grdPatients.AfterRowActivate += new System.EventHandler(this.grdPatients_AfterRowActivate);
            // 
            // lblPatient
            // 
            appearance5.ForeColor = System.Drawing.Color.Red;
            this.lblPatient.Appearance = appearance5;
            this.lblPatient.Font = new System.Drawing.Font("Arial", 13F, System.Drawing.FontStyle.Bold);
            this.lblPatient.Location = new System.Drawing.Point(318, 14);
            this.lblPatient.Name = "lblPatient";
            this.lblPatient.Size = new System.Drawing.Size(133, 23);
            this.lblPatient.TabIndex = 89;
            this.lblPatient.Tag = "NOCOLOR";
            this.lblPatient.Text = "Patient";
            // 
            // lblPatientName
            // 
            appearance4.ForeColor = System.Drawing.Color.Red;
            this.lblPatientName.Appearance = appearance4;
            this.lblPatientName.Font = new System.Drawing.Font("Arial", 13F, System.Drawing.FontStyle.Bold);
            this.lblPatientName.Location = new System.Drawing.Point(462, 14);
            this.lblPatientName.Name = "lblPatientName";
            this.lblPatientName.Size = new System.Drawing.Size(257, 23);
            this.lblPatientName.TabIndex = 90;
            this.lblPatientName.Tag = "NOCOLOR";
            this.lblPatientName.Text = "Patient Name";
            // 
            // frmHouseChargePayment
            // 
            this.AcceptButton = this.btnAccept;
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(754, 433);
            this.Controls.Add(this.grdPatients);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.lblTransactionType);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.Name = "frmHouseChargePayment";
            this.ShowInTaskbar = false;
            this.Text = "Receive On Account";
            this.Load += new System.EventHandler(this.frmHouseChargeConfirmation_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmHouseChargePayment_KeyDown);
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtROARefference)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtAmount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdPatients)).EndInit();
            this.ResumeLayout(false);

        }
        #endregion

        private void frmHouseChargeConfirmation_Load(object sender, System.EventArgs e)
        {
            clsUIHelper.setColorSchecme(this);
            this.txtAmount.Enter += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
            this.txtAmount.Leave += new System.EventHandler(clsUIHelper.AfterExitEditMode);
            this.txtROARefference.Enter += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
            this.txtROARefference.Leave += new System.EventHandler(clsUIHelper.AfterExitEditMode);
            #region PRIMEPOS-2570 02-Jul-2020 JY Added
            if (nCustomerID == 0)
            {
                this.lblPatient.Text = "";  
                this.lblPatientName.Text = "";
            }
            #endregion
        }

        private void btnReject_Click(object sender, System.EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void btnAccept_Click(object sender, System.EventArgs e)
        {
            if (Configuration.convertNullToDecimal(txtAmount.Value) > 999)
            {
                if (Resources.Message.Display("Amount is great than "+ Configuration.CInfo.CurrencySymbol+"999.\nDo you want to continue?", "House Charge", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
                {
                    txtAmount.Focus();
                    return;
                }
            }
            ROARefference = this.txtROARefference.Text.Trim();
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        public DataTable ChargeAccountRecord
        {
            get
            {
                if (this.oDS != null && oDS.Tables.Count > 0)
                    return oDS.Tables[0];
                else
                    return null;
            }
        }


        private void frmHouseChargePayment_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
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
                logger.Fatal(exp, "frmHouseChargePayment_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)");
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }

        private void txtAmount_Enter(object sender, System.EventArgs e)
        {
            try
            {
                this.txtAmount.SelectionStart = 0;
                this.txtAmount.SelectionLength = this.txtAmount.MaxValue.ToString().Length;
            }
            catch (Exception exp)
            {
                logger.Fatal(exp, "txtAmount_Enter(object sender, System.EventArgs e)");
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }

        #region PRIMEPOS-2570 02-Jul-2020 JY Added
        private void PopulatePatientsLinkedWithHCAccount(long AccountNo)
        {
            DataSet dsRxPatient = new DataSet();
            MMSChargeAccount.ContAccount oAcct = new MMSChargeAccount.ContAccount();
            string strSQL = "SELECT ACCT_NO, LNAME, FNAME, ADDRSTR AS Address1, ADDRSTR2 AS Address2, ADDRCT AS City, ADDRZP AS ZIP, MOBILENO, PHONE, SEX, PATIENTNO FROM PATIENT WHERE ACCT_NO = " + AccountNo + " ORDER BY PATIENTNO";
            oAcct.GetRecs(strSQL, out dsRxPatient);
            grdPatients.DataSource = dsRxPatient;

            grdPatients.DisplayLayout.Bands[0].Columns[0].Hidden = true;

            grdPatients.DisplayLayout.Bands[0].Columns["LNAME"].Header.Caption = "Last Name";
            grdPatients.DisplayLayout.Bands[0].Columns["FNAME"].Header.Caption = "First Name";

            grdPatients.DisplayLayout.Bands[0].Columns["LNAME"].Width = 75;
            grdPatients.DisplayLayout.Bands[0].Columns["FNAME"].Width = 75;
            grdPatients.DisplayLayout.Bands[0].Columns["Address1"].Width = 120;
            grdPatients.DisplayLayout.Bands[0].Columns["Address2"].Width = 70;
            grdPatients.DisplayLayout.Bands[0].Columns["City"].Width = 65;
            grdPatients.DisplayLayout.Bands[0].Columns["ZIP"].Width = 45;
            grdPatients.DisplayLayout.Bands[0].Columns["MOBILENO"].Width = 68;
            grdPatients.DisplayLayout.Bands[0].Columns["PHONE"].Width = 68;
            grdPatients.DisplayLayout.Bands[0].Columns["SEX"].Width = 35;
            grdPatients.DisplayLayout.Bands[0].Columns["PATIENTNO"].Width = 80;

            for (int i = 0; i < grdPatients.DisplayLayout.Bands[0].Columns.Count; i++)
            {
                grdPatients.DisplayLayout.Bands[0].Columns[i].CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit;
            }

            if (nCustomerID > 0)
            {
                POSTransaction oPOSTrans = new POSTransaction();
                CustomerData chargeCustomer = oPOSTrans.GetCustomerByID(nCustomerID);
                if (chargeCustomer != null && chargeCustomer.Tables.Count > 0 && chargeCustomer.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < grdPatients.Rows.Count; i++)
                    {
                        if (Configuration.convertNullToInt(grdPatients.Rows[i].Cells["PATIENTNO"].Value) == Configuration.convertNullToInt(chargeCustomer.Tables[0].Rows[0]["PatientNo"]))
                        {
                            grdPatients.Rows[i].Selected = true;
                            grdPatients.ActiveRow = grdPatients.Rows[i];                            
                            break;
                        }
                    }
                }
            }

            clsUIHelper.SetReadonlyRow(this.grdPatients);
            this.grdPatients.DisplayLayout.TabNavigation = Infragistics.Win.UltraWinGrid.TabNavigation.NextControl;
        }

        private void grdPatients_AfterRowActivate(object sender, EventArgs e)
        {
            try
            {
                if (grdPatients != null && grdPatients.ActiveRow != null)
                {
                    this.lblPatient.Text = "Patient";
                    this.lblPatientName.Text = (Configuration.convertNullToString(grdPatients.ActiveRow.Cells["LNAME"].Value) + " " + Configuration.convertNullToString(grdPatients.ActiveRow.Cells["FNAME"].Value)).Trim();
                    PatientNo = Configuration.convertNullToInt(grdPatients.ActiveRow.Cells["PATIENTNO"].Value);
                }
                else
                {
                    this.lblPatient.Text = "";
                    this.lblPatientName.Text = "";
                    PatientNo = 0;
                }
            }
            catch { }
        }
        #endregion
    }
}
