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

namespace POS_Core_UI
{
	/// <summary>
	/// Summary description for frmHouseChargeConfirmation.
	/// </summary>
	public class frmHouseChargeConfirmation : System.Windows.Forms.Form
	{
		private string AccountNo;
		private Decimal AmountCharged=0;
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
        private TextBox txtComments;
        private Infragistics.Win.Misc.UltraLabel ultraLabel4;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
        private static ILogger logger = LogManager.GetCurrentClassLogger();
        public string OverrideHousechargeLimitUser = string.Empty;   //PRIMEPOS-2402 09-Jul-2021 JY Added

        public frmHouseChargeConfirmation(string AcctNo,Decimal AmtCharged)
		{
			//
			// Required for Windows Form Designer support
			//
			AmountCharged=AmtCharged;
			if (AcctNo.Trim()=="")
				throw(new Exception("Invalid account no selected."));
			InitializeComponent();
			AccountNo=AcctNo;
			ContAccount oAcct=new ContAccount();
			DataSet oDS=new DataSet();
            //oAcct.GetAccountByCode(AcctNo,out oDS);
            oAcct.GetAccountByCode(AcctNo, out oDS, true);  //PRIMEPOS-2888 28-Aug-2020 JY Added third parameter as "true" to get the exact HouseCharge record
            if (oDS==null)
				throw(new Exception("Invalid account no selected."));
			else if (oDS.Tables[0].Rows.Count==0)
				throw(new Exception("Invalid account no selected."));
			else if (oDS.Tables[0].Rows[0]["Status"].ToString().Trim().ToUpper()=="I")
				throw(new Exception("Account is inactive."));
			else
			{
				this.lblAccountNo.Text=oDS.Tables[0].Rows[0]["acct_no"].ToString();
				this.lblAddress.Text=oDS.Tables[0].Rows[0]["address1"].ToString();
				this.lblCreditLimit.Text= POS_Core.Resources.Configuration.convertNullToDecimal(oDS.Tables[0].Rows[0]["credit_lmt"].ToString()).ToString("c");
				this.lblDiscount.Text=oDS.Tables[0].Rows[0]["discount"].ToString();
				this.lblName.Text=oDS.Tables[0].Rows[0]["acct_Name"].ToString();
				this.lblPhone.Text=oDS.Tables[0].Rows[0]["phone_no"].ToString();
				this.lblBalance.Text=oAcct.GetAccountBalance(Convert.ToInt32( this.lblAccountNo.Text)).ToString("c");
                this.txtComments.Text = oDS.Tables[0].Rows[0]["comment"].ToString();
			}
			//
			// TODO: Add any constructor code after InitializeComponent call
			//
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmHouseChargeConfirmation));
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            this.lblTransactionType = new Infragistics.Win.Misc.UltraLabel();
            this.btnReject = new Infragistics.Win.Misc.UltraButton();
            this.btnAccept = new Infragistics.Win.Misc.UltraButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtComments = new System.Windows.Forms.TextBox();
            this.ultraLabel4 = new Infragistics.Win.Misc.UltraLabel();
            this.lblDiscount = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabel14 = new Infragistics.Win.Misc.UltraLabel();
            this.lblCreditLimit = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabel12 = new Infragistics.Win.Misc.UltraLabel();
            this.lblBalance = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabel9 = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabel8 = new Infragistics.Win.Misc.UltraLabel();
            this.lblAccountNo = new Infragistics.Win.Misc.UltraLabel();
            this.lblPhone = new Infragistics.Win.Misc.UltraLabel();
            this.lblName = new Infragistics.Win.Misc.UltraLabel();
            this.lblAddress = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabel3 = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabel2 = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabel1 = new Infragistics.Win.Misc.UltraLabel();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblTransactionType
            // 
            appearance1.BackColor = System.Drawing.Color.LightCyan;
            appearance1.BackColor2 = System.Drawing.Color.SteelBlue;
            appearance1.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance1.ForeColor = System.Drawing.Color.Blue;
            appearance1.ForeColorDisabled = System.Drawing.Color.Navy;
            appearance1.TextHAlignAsString = "Center";
            this.lblTransactionType.Appearance = appearance1;
            this.lblTransactionType.BackColorInternal = System.Drawing.Color.Transparent;
            this.lblTransactionType.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblTransactionType.Font = new System.Drawing.Font("Verdana", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTransactionType.Location = new System.Drawing.Point(0, 0);
            this.lblTransactionType.Name = "lblTransactionType";
            this.lblTransactionType.Size = new System.Drawing.Size(615, 45);
            this.lblTransactionType.TabIndex = 64;
            this.lblTransactionType.Tag = "NOCOLOR";
            this.lblTransactionType.Text = "House Charge Confirmation";
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
            this.btnReject.Location = new System.Drawing.Point(487, 19);
            this.btnReject.Name = "btnReject";
            this.btnReject.Size = new System.Drawing.Size(88, 28);
            this.btnReject.TabIndex = 70;
            this.btnReject.Text = "&Reject";
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
            this.btnAccept.Location = new System.Drawing.Point(390, 19);
            this.btnAccept.Name = "btnAccept";
            this.btnAccept.Size = new System.Drawing.Size(88, 28);
            this.btnAccept.TabIndex = 69;
            this.btnAccept.Text = "&Accept";
            this.btnAccept.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnAccept.Click += new System.EventHandler(this.btnAccept_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtComments);
            this.groupBox1.Controls.Add(this.ultraLabel4);
            this.groupBox1.Controls.Add(this.lblDiscount);
            this.groupBox1.Controls.Add(this.ultraLabel14);
            this.groupBox1.Controls.Add(this.lblCreditLimit);
            this.groupBox1.Controls.Add(this.ultraLabel12);
            this.groupBox1.Controls.Add(this.lblBalance);
            this.groupBox1.Controls.Add(this.ultraLabel9);
            this.groupBox1.Controls.Add(this.ultraLabel8);
            this.groupBox1.Controls.Add(this.lblAccountNo);
            this.groupBox1.Controls.Add(this.lblPhone);
            this.groupBox1.Controls.Add(this.lblName);
            this.groupBox1.Controls.Add(this.lblAddress);
            this.groupBox1.Controls.Add(this.ultraLabel3);
            this.groupBox1.Controls.Add(this.ultraLabel2);
            this.groupBox1.Controls.Add(this.ultraLabel1);
            this.groupBox1.Location = new System.Drawing.Point(12, 63);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(588, 253);
            this.groupBox1.TabIndex = 71;
            this.groupBox1.TabStop = false;
            // 
            // txtComments
            // 
            this.txtComments.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtComments.Enabled = false;
            this.txtComments.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtComments.Location = new System.Drawing.Point(20, 186);
            this.txtComments.Multiline = true;
            this.txtComments.Name = "txtComments";
            this.txtComments.ReadOnly = true;
            this.txtComments.Size = new System.Drawing.Size(562, 60);
            this.txtComments.TabIndex = 83;
            this.txtComments.TabStop = false;
            // 
            // ultraLabel4
            // 
            this.ultraLabel4.Font = new System.Drawing.Font("Arial", 13F, System.Drawing.FontStyle.Bold);
            this.ultraLabel4.Location = new System.Drawing.Point(20, 167);
            this.ultraLabel4.Name = "ultraLabel4";
            this.ultraLabel4.Size = new System.Drawing.Size(100, 23);
            this.ultraLabel4.TabIndex = 84;
            this.ultraLabel4.Text = "Comments";
            // 
            // lblDiscount
            // 
            this.lblDiscount.BorderStyleOuter = Infragistics.Win.UIElementBorderStyle.Solid;
            this.lblDiscount.Font = new System.Drawing.Font("Arial", 13F, System.Drawing.FontStyle.Bold);
            this.lblDiscount.Location = new System.Drawing.Point(424, 112);
            this.lblDiscount.Name = "lblDiscount";
            this.lblDiscount.Size = new System.Drawing.Size(151, 23);
            this.lblDiscount.TabIndex = 82;
            // 
            // ultraLabel14
            // 
            this.ultraLabel14.Font = new System.Drawing.Font("Arial", 13F, System.Drawing.FontStyle.Bold);
            this.ultraLabel14.Location = new System.Drawing.Point(321, 112);
            this.ultraLabel14.Name = "ultraLabel14";
            this.ultraLabel14.Size = new System.Drawing.Size(100, 23);
            this.ultraLabel14.TabIndex = 81;
            this.ultraLabel14.Text = "Discount";
            // 
            // lblCreditLimit
            // 
            this.lblCreditLimit.BorderStyleOuter = Infragistics.Win.UIElementBorderStyle.Solid;
            this.lblCreditLimit.Font = new System.Drawing.Font("Arial", 13F, System.Drawing.FontStyle.Bold);
            this.lblCreditLimit.Location = new System.Drawing.Point(424, 141);
            this.lblCreditLimit.Name = "lblCreditLimit";
            this.lblCreditLimit.Size = new System.Drawing.Size(151, 23);
            this.lblCreditLimit.TabIndex = 80;
            // 
            // ultraLabel12
            // 
            this.ultraLabel12.Font = new System.Drawing.Font("Arial", 13F, System.Drawing.FontStyle.Bold);
            this.ultraLabel12.Location = new System.Drawing.Point(321, 141);
            this.ultraLabel12.Name = "ultraLabel12";
            this.ultraLabel12.Size = new System.Drawing.Size(105, 23);
            this.ultraLabel12.TabIndex = 79;
            this.ultraLabel12.Text = "Credit Limit";
            // 
            // lblBalance
            // 
            this.lblBalance.BorderStyleOuter = Infragistics.Win.UIElementBorderStyle.Solid;
            this.lblBalance.Font = new System.Drawing.Font("Arial", 13F, System.Drawing.FontStyle.Bold);
            this.lblBalance.Location = new System.Drawing.Point(132, 141);
            this.lblBalance.Name = "lblBalance";
            this.lblBalance.Size = new System.Drawing.Size(175, 23);
            this.lblBalance.TabIndex = 78;
            // 
            // ultraLabel9
            // 
            this.ultraLabel9.Font = new System.Drawing.Font("Arial", 13F, System.Drawing.FontStyle.Bold);
            this.ultraLabel9.Location = new System.Drawing.Point(20, 83);
            this.ultraLabel9.Name = "ultraLabel9";
            this.ultraLabel9.Size = new System.Drawing.Size(100, 23);
            this.ultraLabel9.TabIndex = 77;
            this.ultraLabel9.Text = "Address";
            // 
            // ultraLabel8
            // 
            this.ultraLabel8.Font = new System.Drawing.Font("Arial", 13F, System.Drawing.FontStyle.Bold);
            this.ultraLabel8.Location = new System.Drawing.Point(20, 54);
            this.ultraLabel8.Name = "ultraLabel8";
            this.ultraLabel8.Size = new System.Drawing.Size(100, 23);
            this.ultraLabel8.TabIndex = 76;
            this.ultraLabel8.Text = "Name";
            // 
            // lblAccountNo
            // 
            this.lblAccountNo.BorderStyleOuter = Infragistics.Win.UIElementBorderStyle.Solid;
            this.lblAccountNo.Font = new System.Drawing.Font("Arial", 13F, System.Drawing.FontStyle.Bold);
            this.lblAccountNo.Location = new System.Drawing.Point(132, 25);
            this.lblAccountNo.Name = "lblAccountNo";
            this.lblAccountNo.Size = new System.Drawing.Size(100, 23);
            this.lblAccountNo.TabIndex = 75;
            // 
            // lblPhone
            // 
            this.lblPhone.BorderStyleOuter = Infragistics.Win.UIElementBorderStyle.Solid;
            this.lblPhone.Font = new System.Drawing.Font("Arial", 13F, System.Drawing.FontStyle.Bold);
            this.lblPhone.Location = new System.Drawing.Point(132, 112);
            this.lblPhone.Name = "lblPhone";
            this.lblPhone.Size = new System.Drawing.Size(175, 23);
            this.lblPhone.TabIndex = 74;
            // 
            // lblName
            // 
            this.lblName.BorderStyleOuter = Infragistics.Win.UIElementBorderStyle.Solid;
            this.lblName.Font = new System.Drawing.Font("Arial", 13F, System.Drawing.FontStyle.Bold);
            this.lblName.Location = new System.Drawing.Point(132, 54);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(445, 23);
            this.lblName.TabIndex = 73;
            // 
            // lblAddress
            // 
            this.lblAddress.BorderStyleOuter = Infragistics.Win.UIElementBorderStyle.Solid;
            this.lblAddress.Font = new System.Drawing.Font("Arial", 13F, System.Drawing.FontStyle.Bold);
            this.lblAddress.Location = new System.Drawing.Point(132, 83);
            this.lblAddress.Name = "lblAddress";
            this.lblAddress.Size = new System.Drawing.Size(445, 23);
            this.lblAddress.TabIndex = 72;
            // 
            // ultraLabel3
            // 
            this.ultraLabel3.Font = new System.Drawing.Font("Arial", 13F, System.Drawing.FontStyle.Bold);
            this.ultraLabel3.Location = new System.Drawing.Point(20, 112);
            this.ultraLabel3.Name = "ultraLabel3";
            this.ultraLabel3.Size = new System.Drawing.Size(100, 23);
            this.ultraLabel3.TabIndex = 71;
            this.ultraLabel3.Text = "Phone";
            // 
            // ultraLabel2
            // 
            this.ultraLabel2.Font = new System.Drawing.Font("Arial", 13F, System.Drawing.FontStyle.Bold);
            this.ultraLabel2.Location = new System.Drawing.Point(20, 141);
            this.ultraLabel2.Name = "ultraLabel2";
            this.ultraLabel2.Size = new System.Drawing.Size(100, 23);
            this.ultraLabel2.TabIndex = 70;
            this.ultraLabel2.Text = "Balance";
            // 
            // ultraLabel1
            // 
            this.ultraLabel1.Font = new System.Drawing.Font("Arial", 13F, System.Drawing.FontStyle.Bold);
            this.ultraLabel1.Location = new System.Drawing.Point(20, 25);
            this.ultraLabel1.Name = "ultraLabel1";
            this.ultraLabel1.Size = new System.Drawing.Size(100, 23);
            this.ultraLabel1.TabIndex = 69;
            this.ultraLabel1.Text = "Acct #";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnReject);
            this.groupBox2.Controls.Add(this.btnAccept);
            this.groupBox2.Location = new System.Drawing.Point(14, 322);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(587, 60);
            this.groupBox2.TabIndex = 72;
            this.groupBox2.TabStop = false;
            // 
            // frmHouseChargeConfirmation
            // 
            this.AcceptButton = this.btnAccept;
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(615, 395);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.lblTransactionType);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "frmHouseChargeConfirmation";
            this.ShowInTaskbar = false;
            this.Text = "House Charge Confirmation";
            this.Load += new System.EventHandler(this.frmHouseChargeConfirmation_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);

		}
		#endregion

		private void frmHouseChargeConfirmation_Load(object sender, System.EventArgs e)
		{
			clsUIHelper.setColorSchecme(this);
            this.txtComments.BackColor = Color.White;
            this.txtComments.ForeColor = Color.Red;
		}

		private void btnReject_Click(object sender, System.EventArgs e)
		{
			this.DialogResult=DialogResult.Cancel;
			this.Close();
		}

        private void btnAccept_Click(object sender, System.EventArgs e)
        {
            logger.Trace("btnAccept_Click(object sender, System.EventArgs e() - " + POS_Core.CommonData.clsPOSDBConstants.Log_Entering);
            try
            {
                if (AmountCharged +
                    Configuration.convertNullToDecimal(
                        this.lblBalance.Text.Replace(
                            Configuration.CInfo.CurrencySymbol.ToString(CultureInfo.InvariantCulture), "")) >
                    Configuration.convertNullToDecimal(
                        this.lblCreditLimit.Text.Replace(
                            Configuration.CInfo.CurrencySymbol.ToString(CultureInfo.InvariantCulture), "")))
                {
                    this.DialogResult = CreditLimitConfirmation("Amount is more than your credit limit. Are you sure to continue?", out OverrideHousechargeLimitUser);  //PRIMEPOS-2402 09-Jul-2021 JY Modified
                    if (OverrideHousechargeLimitUser != "") OverrideHousechargeLimitUser += "~" + "Amount is more than your credit limit."; //PRIMEPOS-2402 09-Jul-2021 JY Added
                    this.Close();
                }
                else if (Configuration.CPOSSet.MaxCATransAmt > 0 && AmountCharged > Configuration.CPOSSet.MaxCATransAmt)
                {
                    this.DialogResult = CreditLimitConfirmation("Amount is more than your charge account transaction limit. Are you sure to continue?", out OverrideHousechargeLimitUser);  //PRIMEPOS-2402 09-Jul-2021 JY Modified
                    if (OverrideHousechargeLimitUser != "") OverrideHousechargeLimitUser += "~" + "Amount is more than your charge account transaction limit."; //PRIMEPOS-2402 09-Jul-2021 JY Added
                    this.Close();
                }
                else
                {
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
            }
            finally
            {
                logger.Trace("btnAccept_Click(object sender, System.EventArgs e() - " + POS_Core.CommonData.clsPOSDBConstants.Log_Exiting);
            }
        }

	    private DialogResult CreditLimitConfirmation(string sMessage, out string sUserID)
        {
            DialogResult dialogResult;
            sUserID = string.Empty; //PRIMEPOS-2402 09-Jul-2021 JY Added
            if (Resources.Message.Display(sMessage, Configuration.ApplicationName, MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            //if(MessageBox.Show("Amount is more than your credit limit.",Application.ProductName,MessageBoxButtons.YesNo)==DialogResult.Yes)
            {
                if (UserPriviliges.getPermission(UserPriviliges.Modules.POSTransaction.ID, UserPriviliges.Screens.POSTransaction.ID, UserPriviliges.Permissions.OverrideHouseChargeLimit.ID, UserPriviliges.Permissions.OverrideHouseChargeLimit.Name, out sUserID))    //PRIMEPOS-2402 08-Jul-2021 JY Added sUserID
                {
                    dialogResult = DialogResult.OK;
                }
                else
                {
                    dialogResult = DialogResult.Cancel;
                }
            }
            else
            {
                dialogResult = DialogResult.Cancel;
            }
            return dialogResult;
        }
	}
}
