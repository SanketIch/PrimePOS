using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using POS_Core.CommonData;
using POS_Core.Resources;
using System.Data;
using System.Data.SqlClient;
using POS_Core.ErrorLogging;
using POS_Core.Resources.DelegateHandler;
using Resources;

namespace POS_Core_UI.UserManagement
{
	/// <summary>
	/// Summary description for frmLogin.
	/// </summary>
	public class frmChangePassword : System.Windows.Forms.Form
	{
		public Infragistics.Win.UltraWinEditors.UltraTextEditor txtUserName;
		private Infragistics.Win.Misc.UltraButton btnOk;
		public Infragistics.Win.Misc.UltraButton btnCancel;
		private Infragistics.Win.Misc.UltraLabel ultraLabel2;
		private Infragistics.Win.Misc.UltraLabel ultraLabel1;
		public Infragistics.Win.UltraWinEditors.UltraTextEditor txtPassward;
		private System.Windows.Forms.GroupBox grpLogin;
		private Infragistics.Win.Misc.UltraLabel ultraLabel3;
		public Infragistics.Win.UltraWinEditors.UltraTextEditor txtNewPassword;
		private Infragistics.Win.Misc.UltraLabel ultraLabel4;
		public Infragistics.Win.UltraWinEditors.UltraTextEditor txtRNewPassword;
        //private string sPassword1 = string.Empty;
        //private string sPassword2 = string.Empty;
        //private string sPassword3 = string.Empty;
        private string sPassword = string.Empty;
        private string sNewPassword = string.Empty;
        private string sUserName = string.Empty;

		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public frmChangePassword()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();		

		}

        public frmChangePassword(string sUserID, string sPassword)
        {
            InitializeComponent();
            sUserName = sUserID;
            this.txtPassward.Text = sPassword;
        }

		/// <summary>
		/// Ensures that user changes password, if user choose not to change password, then it will exit the application.
		/// </summary>
		public bool EnsurePasswordChange { get; set; }

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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmChangePassword));
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance7 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance8 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance9 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance10 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance11 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance12 = new Infragistics.Win.Appearance();
            this.txtUserName = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.btnOk = new Infragistics.Win.Misc.UltraButton();
            this.btnCancel = new Infragistics.Win.Misc.UltraButton();
            this.ultraLabel2 = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabel1 = new Infragistics.Win.Misc.UltraLabel();
            this.txtPassward = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.grpLogin = new System.Windows.Forms.GroupBox();
            this.ultraLabel4 = new Infragistics.Win.Misc.UltraLabel();
            this.txtRNewPassword = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.ultraLabel3 = new Infragistics.Win.Misc.UltraLabel();
            this.txtNewPassword = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            ((System.ComponentModel.ISupportInitialize)(this.txtUserName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPassward)).BeginInit();
            this.grpLogin.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtRNewPassword)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtNewPassword)).BeginInit();
            this.SuspendLayout();
            // 
            // txtUserName
            // 
            appearance1.BackColor = System.Drawing.SystemColors.HighlightText;
            appearance1.ForeColor = System.Drawing.Color.White;
            this.txtUserName.Appearance = appearance1;
            this.txtUserName.BackColor = System.Drawing.SystemColors.HighlightText;
            this.txtUserName.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.txtUserName.Enabled = false;
            this.txtUserName.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtUserName.ForeColor = System.Drawing.Color.White;
            this.txtUserName.Location = new System.Drawing.Point(154, 38);
            this.txtUserName.Name = "txtUserName";
            this.txtUserName.ReadOnly = true;
            this.txtUserName.Size = new System.Drawing.Size(219, 23);
            this.txtUserName.TabIndex = 0;
            this.txtUserName.TabStop = false;
            // 
            // btnOk
            // 
            appearance2.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            appearance2.BackColor2 = System.Drawing.SystemColors.Control;
            appearance2.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance2.ForeColor = System.Drawing.Color.Black;
            appearance2.Image = ((object)(resources.GetObject("appearance2.Image")));
            this.btnOk.Appearance = appearance2;
            this.btnOk.BackColorInternal = System.Drawing.SystemColors.HighlightText;
            this.btnOk.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Flat;
            this.btnOk.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOk.ForeColor = System.Drawing.Color.White;
            appearance3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(188)))), ((int)(((byte)(193)))), ((int)(((byte)(207)))));
            appearance3.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(188)))), ((int)(((byte)(193)))), ((int)(((byte)(207)))));
            this.btnOk.HotTrackAppearance = appearance3;
            this.btnOk.Location = new System.Drawing.Point(179, 160);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(94, 26);
            this.btnOk.TabIndex = 4;
            this.btnOk.Text = "&Change";
            this.btnOk.UseHotTracking = Infragistics.Win.DefaultableBoolean.True;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            appearance4.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            appearance4.BackColor2 = System.Drawing.SystemColors.Control;
            appearance4.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance4.ForeColor = System.Drawing.Color.Black;
            appearance4.Image = ((object)(resources.GetObject("appearance4.Image")));
            this.btnCancel.Appearance = appearance4;
            this.btnCancel.BackColorInternal = System.Drawing.SystemColors.HighlightText;
            this.btnCancel.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Flat;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancel.ForeColor = System.Drawing.Color.White;
            appearance5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(188)))), ((int)(((byte)(193)))), ((int)(((byte)(207)))));
            appearance5.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(188)))), ((int)(((byte)(193)))), ((int)(((byte)(207)))));
            this.btnCancel.HotTrackAppearance = appearance5;
            this.btnCancel.Location = new System.Drawing.Point(281, 160);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(94, 26);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseHotTracking = Infragistics.Win.DefaultableBoolean.True;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            this.btnCancel.Leave += new System.EventHandler(this.btnCancel_Leave);
            // 
            // ultraLabel2
            // 
            appearance6.ForeColor = System.Drawing.Color.Black;
            appearance6.TextHAlignAsString = "Right";
            this.ultraLabel2.Appearance = appearance6;
            this.ultraLabel2.BackColorInternal = System.Drawing.Color.Transparent;
            this.ultraLabel2.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultraLabel2.ForeColor = System.Drawing.Color.White;
            this.ultraLabel2.Location = new System.Drawing.Point(17, 72);
            this.ultraLabel2.Name = "ultraLabel2";
            this.ultraLabel2.Size = new System.Drawing.Size(132, 13);
            this.ultraLabel2.TabIndex = 38;
            this.ultraLabel2.Text = "Old Password *";
            // 
            // ultraLabel1
            // 
            appearance7.ForeColor = System.Drawing.Color.Black;
            appearance7.TextHAlignAsString = "Right";
            this.ultraLabel1.Appearance = appearance7;
            this.ultraLabel1.BackColorInternal = System.Drawing.Color.Transparent;
            this.ultraLabel1.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultraLabel1.ForeColor = System.Drawing.Color.White;
            this.ultraLabel1.Location = new System.Drawing.Point(17, 40);
            this.ultraLabel1.Name = "ultraLabel1";
            this.ultraLabel1.Size = new System.Drawing.Size(132, 13);
            this.ultraLabel1.TabIndex = 37;
            this.ultraLabel1.Text = "User Name";
            // 
            // txtPassward
            // 
            appearance8.BackColor = System.Drawing.SystemColors.HighlightText;
            appearance8.ForeColor = System.Drawing.Color.White;
            this.txtPassward.Appearance = appearance8;
            this.txtPassward.BackColor = System.Drawing.SystemColors.HighlightText;
            this.txtPassward.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.txtPassward.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPassward.ForeColor = System.Drawing.Color.White;
            this.txtPassward.Location = new System.Drawing.Point(154, 68);
            this.txtPassward.MaxLength = 50;
            this.txtPassward.Name = "txtPassward";
            this.txtPassward.PasswordChar = '*';
            this.txtPassward.Size = new System.Drawing.Size(219, 23);
            this.txtPassward.TabIndex = 1;
            // 
            // grpLogin
            // 
            this.grpLogin.Controls.Add(this.ultraLabel4);
            this.grpLogin.Controls.Add(this.txtRNewPassword);
            this.grpLogin.Controls.Add(this.ultraLabel3);
            this.grpLogin.Controls.Add(this.txtNewPassword);
            this.grpLogin.Controls.Add(this.ultraLabel2);
            this.grpLogin.Controls.Add(this.ultraLabel1);
            this.grpLogin.Controls.Add(this.btnOk);
            this.grpLogin.Controls.Add(this.txtUserName);
            this.grpLogin.Controls.Add(this.btnCancel);
            this.grpLogin.Controls.Add(this.txtPassward);
            this.grpLogin.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpLogin.Location = new System.Drawing.Point(23, 20);
            this.grpLogin.Name = "grpLogin";
            this.grpLogin.Size = new System.Drawing.Size(406, 213);
            this.grpLogin.TabIndex = 39;
            this.grpLogin.TabStop = false;
            this.grpLogin.Text = "Change your password";
            // 
            // ultraLabel4
            // 
            appearance9.ForeColor = System.Drawing.Color.Black;
            appearance9.TextHAlignAsString = "Right";
            this.ultraLabel4.Appearance = appearance9;
            this.ultraLabel4.BackColorInternal = System.Drawing.Color.Transparent;
            this.ultraLabel4.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultraLabel4.ForeColor = System.Drawing.Color.White;
            this.ultraLabel4.Location = new System.Drawing.Point(6, 131);
            this.ultraLabel4.Name = "ultraLabel4";
            this.ultraLabel4.Size = new System.Drawing.Size(143, 16);
            this.ultraLabel4.TabIndex = 42;
            this.ultraLabel4.Text = "Re-type Password *";
            // 
            // txtRNewPassword
            // 
            appearance10.BackColor = System.Drawing.SystemColors.HighlightText;
            appearance10.ForeColor = System.Drawing.Color.White;
            this.txtRNewPassword.Appearance = appearance10;
            this.txtRNewPassword.BackColor = System.Drawing.SystemColors.HighlightText;
            this.txtRNewPassword.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.txtRNewPassword.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtRNewPassword.ForeColor = System.Drawing.Color.White;
            this.txtRNewPassword.Location = new System.Drawing.Point(154, 127);
            this.txtRNewPassword.MaxLength = 15;
            this.txtRNewPassword.Name = "txtRNewPassword";
            this.txtRNewPassword.PasswordChar = '*';
            this.txtRNewPassword.Size = new System.Drawing.Size(219, 23);
            this.txtRNewPassword.TabIndex = 3;
            // 
            // ultraLabel3
            // 
            appearance11.ForeColor = System.Drawing.Color.Black;
            appearance11.TextHAlignAsString = "Right";
            this.ultraLabel3.Appearance = appearance11;
            this.ultraLabel3.BackColorInternal = System.Drawing.Color.Transparent;
            this.ultraLabel3.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultraLabel3.ForeColor = System.Drawing.Color.White;
            this.ultraLabel3.Location = new System.Drawing.Point(17, 101);
            this.ultraLabel3.Name = "ultraLabel3";
            this.ultraLabel3.Size = new System.Drawing.Size(132, 13);
            this.ultraLabel3.TabIndex = 40;
            this.ultraLabel3.Text = "New Password *";
            // 
            // txtNewPassword
            // 
            appearance12.BackColor = System.Drawing.SystemColors.HighlightText;
            appearance12.ForeColor = System.Drawing.Color.White;
            this.txtNewPassword.Appearance = appearance12;
            this.txtNewPassword.BackColor = System.Drawing.SystemColors.HighlightText;
            this.txtNewPassword.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.txtNewPassword.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtNewPassword.ForeColor = System.Drawing.Color.White;
            this.txtNewPassword.Location = new System.Drawing.Point(154, 97);
            this.txtNewPassword.MaxLength = 15;
            this.txtNewPassword.Name = "txtNewPassword";
            this.txtNewPassword.PasswordChar = '*';
            this.txtNewPassword.Size = new System.Drawing.Size(219, 23);
            this.txtNewPassword.TabIndex = 2;
            this.txtNewPassword.Leave += new System.EventHandler(this.txtNewPassword_Leave);
            // 
            // frmChangePassword
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(7, 17);
            this.BackColor = System.Drawing.Color.White;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(456, 259);
            this.Controls.Add(this.grpLogin);
            this.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmChangePassword";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Change Password";
            this.Closing += new System.ComponentModel.CancelEventHandler(this.frmLogin_Closing);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmChangePassword_FormClosing);
            this.Load += new System.EventHandler(this.frmLogin_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmLogin_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.txtUserName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPassward)).EndInit();
            this.grpLogin.ResumeLayout(false);
            this.grpLogin.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtRNewPassword)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtNewPassword)).EndInit();
            this.ResumeLayout(false);

		}
		#endregion

		private void btnOk_Click(object sender, System.EventArgs e)
		{
            try
            {
                Logs.Logger("Start Password changed for User "+this.txtUserName.Text+" ");
                clsLogin oclsLogin = new clsLogin();
                string IPAddress;
                //oclsLogin.ValidateUserNamePassward(this.txtUserName.Text,this.txtPassward.Text,out IPAddress);
                if (this.txtNewPassword.Text == "" || this.txtPassward.Text == "" || this.txtRNewPassword.Text == "")
                {
                    this.txtNewPassword.Focus();
                    throw (new Exception("Please enter all required fields."));
                }
                if (this.txtNewPassword.Text != this.txtRNewPassword.Text)
                {
                    this.txtNewPassword.Focus();
                    throw (new Exception("New Password does not match with Re-type password."));
                }
                #region PRIMEPOS-2562 02-Aug-2018 JY commented - now onwards we are maintaining passwords in User_Log
                //sPassword3 = sPassword2;
                //sPassword2 = sPassword1;
                //sPassword1 = this.txtPassward.Text;
                #endregion
                //string strSQL = "update users set password=@password, passwordchangedon=@passwordchangedon, password1=@password1, password2=@password2, password3=@password3, ModifiedBy = @ModifiedBy where userid=@userID"; //PRIMEPOS-2562 01-Aug-2018 JY Commented
                string strSQL = "update users set password=@password, passwordchangedon=@passwordchangedon, ModifiedBy = @ModifiedBy, ChangePasswordAtLogin = 0 where userid=@userID";   //PRIMEPOS-2562 01-Aug-2018 JY Added
                System.Data.IDbDataParameter[] oparm = DataFactory.CreateParameterArray(4);
                oparm[0] = DataFactory.CreateParameter();
                oparm[0].ParameterName = "@password";
                string srtTemp = EncryptString.Encrypt(this.txtNewPassword.Text);
                oparm[0].Value = srtTemp;
                oparm[0].DbType = System.Data.DbType.String;

                oparm[1] = DataFactory.CreateParameter();
                oparm[1].ParameterName = "@userid";
                oparm[1].Value = this.txtUserName.Text;
                oparm[1].DbType = System.Data.DbType.String;

                oparm[2] = DataFactory.CreateParameter();
                oparm[2].ParameterName = "@passwordchangedon";
                oparm[2].Value = System.DateTime.Now;
                oparm[2].DbType = System.Data.DbType.DateTime;

                //PRIMEPOS-2562 01-Aug-2018 JY Added new parameter
                oparm[3] = DataFactory.CreateParameter();
                oparm[3].ParameterName = "@ModifiedBy";
                oparm[3].Value = Configuration.UserName.Trim().Replace("'", "''");
                oparm[3].DbType = System.Data.DbType.String;

                #region PRIMEPOS-2562 02-Aug-2018 JY Commented
                //oparm[3] = DataFactory.CreateParameter();
                //oparm[3].ParameterName = "@password1";
                //string srtPassword1 = EncryptString.Encrypt(sPassword1);
                //oparm[3].Value = srtPassword1;
                //oparm[3].DbType = System.Data.DbType.String;

                //oparm[4] = DataFactory.CreateParameter();
                //oparm[4].ParameterName = "@password2";
                //string srtPassword2 = EncryptString.Encrypt(sPassword2);
                //oparm[4].Value = srtPassword2;
                //oparm[4].DbType = System.Data.DbType.String;

                //oparm[5] = DataFactory.CreateParameter();
                //oparm[5].ParameterName = "@password3";
                //string srtPassword3 = EncryptString.Encrypt(sPassword3);
                //oparm[5].Value = srtPassword3;
                //oparm[5].DbType = System.Data.DbType.String;
                #endregion               

                DataHelper.ExecuteNonQuery(DataFactory.CreateConnection(POS_Core.Resources.Configuration.ConnectionString), System.Data.CommandType.Text, strSQL, oparm);
                ErrorHandler.SaveLog((int)LogENUM.Change_Password, Configuration.UserName, "Success", "Password changed successfully");
                #region PRIMEPOS-3204
                //DBUser.ChangeDBUserPwd(this.txtUserName.Text,this.txtPassward.Text, this.txtNewPassword.Text);
                //Configuration.SQLUserID = "MMS" + this.txtUserName.Text;
                //Configuration.SQLUserPassword = this.txtNewPassword.Text;
                //Configuration.m_ConnString = string.Empty;
                #endregion
                Resources.Message.Display("Password changed successfully.", Configuration.ApplicationName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                string settingChanges = "By User :" + Configuration.UserName + " Station ID :" + Configuration.StationID + Environment.NewLine ;
                Logs.Logger(" Password changed successfully for User "+this.txtUserName.Text+" "+settingChanges);

				//Added by Shrikant Mali on 11/02/2014 so as to bypass the logic to check if
				//the passwrod has been changed or not whihc is on the form closing event.
	            FormClosing -= frmChangePassword_FormClosing;

                this.Close();
            }
            catch (POSExceptions exp)
            { 
                string settingChanges = "Current User :" + Configuration.UserName + " Station ID :" + Configuration.StationID + Environment.NewLine ;
                Logs.Logger(" Unable to change Password for User "+this.txtUserName.Text+" Exception:"+exp.Message+Environment.NewLine+settingChanges );
                ErrorHandler.SaveLog((int)LogENUM.Change_Password, Configuration.UserName, "Fail", "Password not changed successfully");
                POS_Core_UI.Resources.Message.Display(exp.Message, "User Authentication", MessageBoxButtons.OK, MessageBoxIcon.Error);
                switch (exp.ErrNumber)
                {
                    case (long)POSErrorENUM.User_InValidPassward:
                        txtPassward.Focus();
                        break;
                }
            }
            catch (Exception exp)
            {     string settingChanges = "Current User :" + Configuration.UserName + " Station ID :" + Configuration.StationID + Environment.NewLine ;
                Logs.Logger(" Unable to change Password for User "+this.txtUserName.Text+" Exception:"+exp.Message+Environment.NewLine+settingChanges );
                POS_Core_UI.Resources.Message.Display(exp.Message, "User Authentication", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.txtUserName.Focus();
            }
		}

		private void btnCancel_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}

		private void frmLogin_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
		}

		private void grpBoxLogin_Enter(object sender, System.EventArgs e)
		{
			this.txtPassward.Focus();
		}

		private void btnCancel_Leave(object sender, System.EventArgs e)
		{
			this.txtPassward.Focus();
		}

		private void frmLogin_Load(object sender, System.EventArgs e)
		{
			this.txtUserName.AfterEnterEditMode += new System.EventHandler(clsCoreUIHelper.AfterEnterEditMode);
			this.txtUserName.AfterExitEditMode += new System.EventHandler(clsCoreUIHelper.AfterExitEditMode);

			this.txtPassward.AfterEnterEditMode += new System.EventHandler(clsCoreUIHelper.AfterEnterEditMode);
			this.txtPassward.AfterExitEditMode += new System.EventHandler(clsCoreUIHelper.AfterExitEditMode);

			this.txtNewPassword.AfterEnterEditMode += new System.EventHandler(clsCoreUIHelper.AfterEnterEditMode);
			this.txtNewPassword.AfterExitEditMode += new System.EventHandler(clsCoreUIHelper.AfterExitEditMode);

			this.txtRNewPassword.AfterEnterEditMode += new System.EventHandler(clsCoreUIHelper.AfterEnterEditMode);
			this.txtRNewPassword.AfterExitEditMode += new System.EventHandler(clsCoreUIHelper.AfterExitEditMode);

            #region PRIMEPOS-2575 07-Aug-2018 JY Commented
            //if (Configuration.UserName != "")
            //    this.txtUserName.Text = Configuration.UserName;
            //else
            //    this.txtUserName.Text = sUserName;
            #endregion
            this.txtUserName.Text = sUserName.Trim() != "" ? sUserName : Configuration.UserName;    //PRIMEPOS-2575 07-Aug-2018 JY Added

            clsCoreUIHelper.setColorSchecme(this);
		}

		private void frmLogin_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			try
			{
				if(e.KeyData==System.Windows.Forms.Keys.Enter)
				{
					this.SelectNextControl(this.ActiveControl,true,true,true,true);
				}
				
			}
			catch(Exception exp)
			{
                clsCoreUIHelper.ShowBtnIconMsg(exp.Message, "User Authentication", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
		}

		private void grpBoxLogin_Click(object sender, System.EventArgs e)
		{
		
		}

        private void txtNewPassword_Leave(object sender, EventArgs e)
        {
            try
            {
                //following if- else is Added By shitaljit on 10 Oct 2012 to bypass password complexity check logic if Enforce Complex Password is true.
                if (!string.IsNullOrEmpty(txtNewPassword.Text))
                {
	                const string errorMessageTitle = "User Authentication";
	                if (Configuration.CInfo.EnforceComplexPassword)
                    {
                        #region PRIMEPOS-2562 02-Aug-2018 JY Commented
                        string strMessage = string.Empty;
                        Boolean bIsValid = ValidateNewPassword(txtUserName.Text.Trim(), txtNewPassword.Text.Trim(), ref strMessage);
                        if (bIsValid == false)
                        {
                            Resources.Message.Display(strMessage, errorMessageTitle, MessageBoxButtons.OK,MessageBoxIcon.Error);
                            txtNewPassword.Focus();
                        }
                        #endregion

                        #region PRIMEPOS-2562 02-Aug-2018 JY Commented
                        //const string errorMessage = "Error in validating the password. Please ensure \n " +
                        //                            "1.Password should be atleast seven characters \n " +
                        //                            "2.Password should be alphanumeric \n " +
                        //                            "3.Password should not the same as any of the last four passwords \n " +
                        //                            "4.Password should contain special characters";

                        ////if (txtNewPassword.Text.Length < 7)
                        //if (txtNewPassword.Text.Length < 7 ||
                        //    !IsValidAlphaNumeric(txtNewPassword.Text) ||
                        //    !IsValidPassword() ||
                        //    !DoseContainsSpecialCharacter(txtNewPassword.Text))
                        //{
                        // Resources.Message.Display(errorMessage, errorMessageTitle, MessageBoxButtons.OK,
                        //                           MessageBoxIcon.Error);
                        // txtNewPassword.Focus();
                        //   }
                        #endregion

                        #region Commented by Shrikant Mali on 05-Feb-2014 so as to optimize it, and it's optimized into above if condition.
                        //else
                        //{
                        //	if (!(IsValidAlphaNumeric(txtNewPassword.Text)))
                        //	{
                        //		Resources.Message.Display(errorMessage, errorMessageTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        //		txtNewPassword.Focus();
                        //	}
                        //	else if (!(IsValidPassword()))
                        //	{
                        //		Resources.Message.Display(errorMessage, errorMessageTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        //		txtNewPassword.Focus();
                        //	} else if (!DoseContainsSpecialCharacter(txtNewPassword.Text))
                        //	{
                        //		Resources.Message.Display(errorMessage, errorMessageTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        //		txtNewPassword.Focus();
                        //	}

                        //}
                        #endregion
                    }
                    else
                    {
                        if (txtNewPassword.Text.Length < 4)
                        {
                            clsCoreUIHelper.ShowBtnIconMsg("Error in validating the password.\nPassword should be atleast four characters", errorMessageTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                            txtNewPassword.Focus();
                        }
                    }
                }
            }
            catch (Exception exp)
            {
                clsCoreUIHelper.ShowErrorMsg(exp.Message);
            }
        }

		/// <summary>
		/// Written by Shrikant Mali, on 02/Feb/2014,
		/// Determins whether the new password contains any special characted or not.
		/// </summary>
		/// <param name="newPassword">The password to be checked againts the special character</param>
		/// <returns>True if newPassword contains any special character otherwise false</returns>
		private static bool DoseContainsSpecialCharacter(string newPassword)
		{
			return newPassword.Any(character => !char.IsLetterOrDigit(character));
		}

		private bool IsValidAlphaNumeric(string sPassword)
        {
            bool isChar = false;
            bool isDigit = false;
            if (string.IsNullOrEmpty(sPassword))
                return false;
            for (int cnt = 0; cnt < sPassword.Length; cnt++)
            {
                if (char.IsLetter(sPassword[cnt]))
                {
                    isChar = true;
                }
                else if (char.IsNumber(sPassword[cnt]))
                {
                    isDigit = true;
                }
            }
            if (!isChar || !isDigit)
                return false;
            else
                return true;
        }

        #region PRIMEPOS-2562 02-Aug-2018 JY Commented
        //private bool IsValidPassword()
        //{
        //    bool result = false;
        //    try
        //    {
        //        sPassword = this.txtPassward.Text;
        //        sNewPassword = this.txtNewPassword.Text;
        //        clsLogin oclsLogin = new clsLogin();
        //        DataSet oDS;
        //        if (this.txtUserName.Text != string.Empty)
        //        {
        //            oDS = oclsLogin.GetUserPasswords(this.txtUserName.Text);
        //            if (oDS != null && oDS.Tables[0].Rows.Count > 0)
        //            {
        //                if (oDS.Tables[0].Rows[0][clsPOSDBConstants.Users_Fld_Password1].ToString() != string.Empty)
        //                {
        //                    sPassword1 = EncryptString.Decrypt(oDS.Tables[0].Rows[0][clsPOSDBConstants.Users_Fld_Password1].ToString());
        //                }
        //                if (oDS.Tables[0].Rows[0][clsPOSDBConstants.Users_Fld_Password2].ToString() != string.Empty)
        //                {
        //                    sPassword2 = EncryptString.Decrypt(oDS.Tables[0].Rows[0][clsPOSDBConstants.Users_Fld_Password2].ToString());
        //                }
        //                if (oDS.Tables[0].Rows[0][clsPOSDBConstants.Users_Fld_Password3].ToString() != string.Empty)
        //                {
        //                    sPassword3 = EncryptString.Decrypt(oDS.Tables[0].Rows[0][clsPOSDBConstants.Users_Fld_Password3].ToString());
        //                }
        //                if (sNewPassword != sPassword && sNewPassword != sPassword1 && sNewPassword != sPassword2 && sNewPassword != sPassword3)
        //                {
        //                    result = true;
        //                } 
                        
                        
        //            }
        //            else
        //            {
        //                result = true;
        //            }
        //        }
        //    }
        //    catch (Exception exp)
        //    {
        //        throw (exp);
        //    }
        //    return result;
        //}
        #endregion

        private void frmChangePassword_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (EnsurePasswordChange)
			{
				var dialogResult =
					Resources.Message.Display(
					"To login into PrimePOS you need to change your password." + "\nAre you sure you want to exit from the system?",
					"User Authentication", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
				if (dialogResult == DialogResult.No)
				{
					e.Cancel = true;
					txtPassward.Focus();
				}
				else
				{
					EnsurePasswordChange = false;
					Application.Exit();
				}

			}
		}

        #region PRIMEPOS-2562 02-Aug-2018 JY Added 
        public static Boolean ValidateNewPassword(string UserName, string strPassword, ref string strMessage)
        {
            Boolean bIsValid = true;
            strMessage = "Error in validating the password. Please ensure";
            //Password length
            if (Configuration.CInfo.PasswordLength > 0 && strPassword.Length < Configuration.CInfo.PasswordLength)
            {
                strMessage += Environment.NewLine + "Password should be atleast " + Configuration.CInfo.PasswordLength + " characters";
                bIsValid = false;
            }
                        
            bool hasUpper = false, hasLower = false, hasDigit = false, hasSpecialChar = false;
            for (int i = 0; i < strPassword.Length && !(hasUpper && hasLower && hasDigit && hasSpecialChar); i++)
            {
                char c = strPassword[i];
                if (Configuration.CInfo.EnforceLowerCaseChar == true)
                {
                    if (!hasLower)  hasLower = char.IsLower(c);
                }
                if (Configuration.CInfo.EnforceUpperCaseChar == true)
                {
                    if (!hasUpper)  hasUpper = char.IsUpper(c);
                }
                if (Configuration.CInfo.EnforceNumber == true)
                {
                    if (!hasDigit)  hasDigit = char.IsDigit(c);
                }
                if (Configuration.CInfo.EnforceSpecialChar == true)
                {
                    if (!hasSpecialChar)
                    {
                        if (!char.IsLetterOrDigit(c))
                            hasSpecialChar = true;
                    }
                }
            }

            //At least one lower case character
            if (Configuration.CInfo.EnforceLowerCaseChar == true && hasLower == false)
            {
                strMessage += Environment.NewLine + "Password should contain at least one lower case character";
                bIsValid = false;
            }
            //At least one upper case character
            if (Configuration.CInfo.EnforceUpperCaseChar == true && hasUpper == false)
            {
                strMessage += Environment.NewLine + "Password should contain at least one upper case character";
                bIsValid = false;
            }
            //At least one number
            if (Configuration.CInfo.EnforceNumber == true && hasDigit == false)
            {
                strMessage += Environment.NewLine + "Password should contain at least one number";
                bIsValid = false;
            }

            //At least one special character
            if (Configuration.CInfo.EnforceSpecialChar == true && hasSpecialChar == false) //PRIMEPOS-3210
            {
                strMessage += Environment.NewLine + "Password should contain at least one special character";
                bIsValid = false;
            }

            //Password History count
            if (Configuration.CInfo.PasswordHistoryCount > 0)
            {
                DataTable dt = GetLastNPasswords(UserName.Replace("'", "''"));
                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        string tempPwd = EncryptString.Decrypt(Configuration.convertNullToString(dr["NewValue"].ToString()));
                        if (tempPwd != "")
                        {
                            if (strPassword == tempPwd)
                            {
                                strMessage += Environment.NewLine + "We can't reuse the last " + Configuration.CInfo.PasswordHistoryCount + " password(s)";
                                bIsValid = false;
                                break;
                            }
                        }
                    }
                }
            }
            return bIsValid;
        }

        private static DataTable GetLastNPasswords(string UserName)
        {
            DataTable dt = new DataTable();
            try
            {
                string strSQL = "SELECT TOP " + Configuration.CInfo.PasswordHistoryCount + " UL.NewValue FROM User_Log UL WHERE UL.FieldChanged = 'Password' AND UL.UserID IN (SELECT ID FROM Users WHERE UserID = '" + UserName.Replace("'","''") + "') ORDER BY ModifiedOn DESC";
                dt = DataHelper.ExecuteDataTable(DataFactory.CreateConnection(Configuration.ConnectionString), CommandType.Text, strSQL);
                return dt;
            }
            catch(Exception Ex)
            {
                return null;
            }
        }
        #endregion
    }
}
