using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using POS_Core.BusinessRules;
using POS_Core.CommonData;
using POS_Core.ErrorLogging;
using POS_Core.Resources;
using PharmData;    //Sprint-20 25-May-2015 JY Added to get NABP from PrimeRx for Auto updater
using POS_Core.DataAccess;   //Sprint-20 25-May-2015 JY Added to get NABP from PrimeRx for Auto updater
using System.DirectoryServices.Protocols;   //PRIMEPOS-2616 14-Dec-2018 JY Added
using System.DirectoryServices.ActiveDirectory; //PRIMEPOS-2616 14-Dec-2018 JY Added
using System.Net;   //PRIMEPOS-2616 14-Dec-2018 JY Added
using POS_Core.Resources.DelegateHandler;
using Resources;
using POS_Core_UI.UI;
using System.Collections;
using MMSInterfaces.Helpers;
using NLog;
using System.Xml;
using System.Xml.Linq;
using System.IO;
using System.Security.AccessControl;
using System.Security.Permissions;
//using Resources;

namespace POS_Core_UI.UserManagement
{
    /// <summary>
    /// Summary description for frmLogin.
    /// </summary>
    public class frmLogin : System.Windows.Forms.Form
    {
        public clsLogin oclsLogin = new clsLogin();
        private static frmLogin oLogin;
        private bool m_Canceled;
        public string strPrevlige = "";
        public string strPermission = string.Empty;
        public LoginENUM OpenType;
        private bool allowClose = false;
        public Infragistics.Win.UltraWinEditors.UltraTextEditor txtUserName;
        private Infragistics.Win.Misc.UltraButton btnOk;
        public Infragistics.Win.Misc.UltraButton btnCancel;
        private Infragistics.Win.Misc.UltraLabel ultraLabel2;
        private Infragistics.Win.Misc.UltraLabel ultraLabel1;
        public Infragistics.Win.UltraWinEditors.UltraTextEditor txtPassward;
        private Infragistics.Win.Misc.UltraGroupBox grpLogin;
        private Infragistics.Win.Misc.UltraGroupBox groupBox1;
        private Infragistics.Win.Misc.UltraLabel lblPharmacy;
        private Infragistics.Win.Misc.UltraLabel ultraLabel3;
        private PictureBox pictureBox1;
        private Infragistics.Win.Misc.UltraLabel ultraLabel4;
        private Infragistics.Win.Misc.UltraGroupBox groupBox2;
        private Infragistics.Win.Misc.UltraLabel lblPrimePOS;
        private Infragistics.Win.Misc.UltraLabel lblLoginAttempt;
        private Infragistics.Win.Misc.UltraLabel lblPOSV;
        private PictureBox pctPrimePOS;
        #region  NileshJ 
        public string username = string.Empty;
        public string password = string.Empty;
        public bool IsPOSLite = false;
        #endregion
        public bool bScheduledTaskExecute = false;  //PRIMEPOS-2485 29-Mar-2021 JY Added

        #region PRIMEPOS-2576 29-Aug-2018 JY Added
        private Label lblTouchID;
        private bool IsFPReaderConnected = false;
        private FingerPrintReader fpReader = null;
        private int fpCaptureCount = 0;
        private Infragistics.Win.Misc.UltraLabel lblLoginWith;
        private Infragistics.Win.UltraWinEditors.UltraComboEditor cboLoginWith;
        private Infragistics.Win.Misc.UltraLabel lblDomain;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor txtDomain;
        public WebBrowser webBrowser1;
        private Timer timer1;
        private IContainer components;
        private static ILogger logger = LogManager.GetCurrentClassLogger();
        #endregion

        #region PRIMEPOS-2616 14-Dec-2018 JY Added
        [System.Runtime.InteropServices.DllImport("advapi32.dll")]
        public static extern bool LogonUser(string userName, string domainName, string password, int LogonType, int LogonProvider, ref IntPtr phToken);
        #endregion

        public frmLogin()
        {
            //
            // Required for Windows Form Designer support
            //
            m_Canceled = true;
            InitializeComponent();
        }


        public static frmLogin GetInstance()
        {
            if (oLogin == null || oLogin.IsDisposed)
            {
                oLogin = new frmLogin();
            }
            return oLogin;
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (IsFPReaderConnected)
            {
                fpReader.DisconnectCloseReader();
            }

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
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance7 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance8 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance9 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance10 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance11 = new Infragistics.Win.Appearance();
            Infragistics.Win.ValueListItem valueListItem3 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem4 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem1 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.Appearance appearance12 = new Infragistics.Win.Appearance();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmLogin));
            Infragistics.Win.Appearance appearance13 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance14 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance15 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance16 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance17 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance18 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance19 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance20 = new Infragistics.Win.Appearance();
            this.txtUserName = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.btnOk = new Infragistics.Win.Misc.UltraButton();
            this.btnCancel = new Infragistics.Win.Misc.UltraButton();
            this.ultraLabel2 = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabel1 = new Infragistics.Win.Misc.UltraLabel();
            this.txtPassward = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.grpLogin = new Infragistics.Win.Misc.UltraGroupBox();
            this.webBrowser1 = new System.Windows.Forms.WebBrowser();
            this.txtDomain = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.lblDomain = new Infragistics.Win.Misc.UltraLabel();
            this.cboLoginWith = new Infragistics.Win.UltraWinEditors.UltraComboEditor();
            this.lblLoginWith = new Infragistics.Win.Misc.UltraLabel();
            this.lblTouchID = new System.Windows.Forms.Label();
            this.lblLoginAttempt = new Infragistics.Win.Misc.UltraLabel();
            this.groupBox1 = new Infragistics.Win.Misc.UltraGroupBox();
            this.lblPOSV = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabel4 = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabel3 = new Infragistics.Win.Misc.UltraLabel();
            this.lblPharmacy = new Infragistics.Win.Misc.UltraLabel();
            this.groupBox2 = new Infragistics.Win.Misc.UltraGroupBox();
            this.pctPrimePOS = new System.Windows.Forms.PictureBox();
            this.lblPrimePOS = new Infragistics.Win.Misc.UltraLabel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.txtUserName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPassward)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grpLogin)).BeginInit();
            this.grpLogin.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtDomain)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboLoginWith)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupBox1)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupBox2)).BeginInit();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pctPrimePOS)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // txtUserName
            // 
            appearance1.BackColor = System.Drawing.SystemColors.HighlightText;
            appearance1.ForeColor = System.Drawing.Color.White;
            this.txtUserName.Appearance = appearance1;
            this.txtUserName.BackColor = System.Drawing.SystemColors.HighlightText;
            this.txtUserName.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.txtUserName.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtUserName.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtUserName.ForeColor = System.Drawing.Color.White;
            this.txtUserName.Location = new System.Drawing.Point(95, 60);
            this.txtUserName.Name = "txtUserName";
            this.txtUserName.Size = new System.Drawing.Size(170, 23);
            this.txtUserName.TabIndex = 1;
            this.txtUserName.TextChanged += new System.EventHandler(this.txtUserName_TextChanged);
            this.txtUserName.Enter += new System.EventHandler(this.txtUserName_Enter);
            this.txtUserName.Leave += new System.EventHandler(this.txtUserName_Leave);
            this.txtUserName.Validating += new System.ComponentModel.CancelEventHandler(this.txtUserName_Validating);
            // 
            // btnOk
            // 
            appearance2.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            appearance2.BackColor2 = System.Drawing.SystemColors.Control;
            appearance2.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance2.ForeColor = System.Drawing.Color.Black;
            this.btnOk.Appearance = appearance2;
            this.btnOk.BackColorInternal = System.Drawing.SystemColors.HighlightText;
            this.btnOk.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Flat;
            this.btnOk.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOk.ForeColor = System.Drawing.Color.White;
            appearance3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(188)))), ((int)(((byte)(193)))), ((int)(((byte)(207)))));
            appearance3.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(188)))), ((int)(((byte)(193)))), ((int)(((byte)(207)))));
            this.btnOk.HotTrackAppearance = appearance3;
            this.btnOk.Location = new System.Drawing.Point(100, 126);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(78, 26);
            this.btnOk.TabIndex = 4;
            this.btnOk.Text = "&Login";
            this.btnOk.UseHotTracking = Infragistics.Win.DefaultableBoolean.True;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            appearance4.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            appearance4.BackColor2 = System.Drawing.SystemColors.Control;
            appearance4.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance4.ForeColor = System.Drawing.Color.Black;
            this.btnCancel.Appearance = appearance4;
            this.btnCancel.BackColorInternal = System.Drawing.SystemColors.HighlightText;
            this.btnCancel.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Flat;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancel.ForeColor = System.Drawing.Color.White;
            appearance5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(188)))), ((int)(((byte)(193)))), ((int)(((byte)(207)))));
            appearance5.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(188)))), ((int)(((byte)(193)))), ((int)(((byte)(207)))));
            this.btnCancel.HotTrackAppearance = appearance5;
            this.btnCancel.Location = new System.Drawing.Point(185, 126);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(80, 26);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseHotTracking = Infragistics.Win.DefaultableBoolean.True;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            this.btnCancel.Leave += new System.EventHandler(this.btnCancel_Leave);
            // 
            // ultraLabel2
            // 
            appearance6.ForeColor = System.Drawing.Color.Black;
            this.ultraLabel2.Appearance = appearance6;
            this.ultraLabel2.BackColorInternal = System.Drawing.Color.Transparent;
            this.ultraLabel2.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultraLabel2.ForeColor = System.Drawing.Color.White;
            this.ultraLabel2.Location = new System.Drawing.Point(8, 91);
            this.ultraLabel2.Name = "ultraLabel2";
            this.ultraLabel2.Size = new System.Drawing.Size(84, 16);
            this.ultraLabel2.TabIndex = 38;
            this.ultraLabel2.Text = "Password";
            // 
            // ultraLabel1
            // 
            appearance7.ForeColor = System.Drawing.Color.Black;
            this.ultraLabel1.Appearance = appearance7;
            this.ultraLabel1.BackColorInternal = System.Drawing.Color.Transparent;
            this.ultraLabel1.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultraLabel1.ForeColor = System.Drawing.Color.White;
            this.ultraLabel1.Location = new System.Drawing.Point(8, 63);
            this.ultraLabel1.Name = "ultraLabel1";
            this.ultraLabel1.Size = new System.Drawing.Size(84, 16);
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
            this.txtPassward.Location = new System.Drawing.Point(95, 88);
            this.txtPassward.Name = "txtPassward";
            this.txtPassward.PasswordChar = '*';
            this.txtPassward.Size = new System.Drawing.Size(170, 23);
            this.txtPassward.TabIndex = 3;
            // 
            // grpLogin
            // 
            appearance9.BorderColor = System.Drawing.Color.DarkGray;
            this.grpLogin.ContentAreaAppearance = appearance9;
            this.grpLogin.Controls.Add(this.webBrowser1);
            this.grpLogin.Controls.Add(this.txtDomain);
            this.grpLogin.Controls.Add(this.lblDomain);
            this.grpLogin.Controls.Add(this.cboLoginWith);
            this.grpLogin.Controls.Add(this.lblLoginWith);
            this.grpLogin.Controls.Add(this.lblTouchID);
            this.grpLogin.Controls.Add(this.lblLoginAttempt);
            this.grpLogin.Controls.Add(this.ultraLabel2);
            this.grpLogin.Controls.Add(this.ultraLabel1);
            this.grpLogin.Controls.Add(this.btnOk);
            this.grpLogin.Controls.Add(this.txtUserName);
            this.grpLogin.Controls.Add(this.btnCancel);
            this.grpLogin.Controls.Add(this.txtPassward);
            this.grpLogin.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpLogin.Location = new System.Drawing.Point(10, 80);
            this.grpLogin.Name = "grpLogin";
            this.grpLogin.Size = new System.Drawing.Size(363, 190);
            this.grpLogin.TabIndex = 39;
            this.grpLogin.Text = "Please Provide Your Login Credentials";
            // 
            // webBrowser1
            // 
            this.webBrowser1.Location = new System.Drawing.Point(309, 65);
            this.webBrowser1.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBrowser1.Name = "webBrowser1";
            this.webBrowser1.Size = new System.Drawing.Size(459, 300);
            this.webBrowser1.TabIndex = 45;
            // 
            // txtDomain
            // 
            this.txtDomain.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.txtDomain.Font = new System.Drawing.Font("Verdana", 10F);
            this.txtDomain.Location = new System.Drawing.Point(95, 88);
            this.txtDomain.Name = "txtDomain";
            this.txtDomain.Size = new System.Drawing.Size(170, 23);
            this.txtDomain.TabIndex = 2;
            // 
            // lblDomain
            // 
            appearance10.ForeColor = System.Drawing.Color.Black;
            this.lblDomain.Appearance = appearance10;
            this.lblDomain.BackColorInternal = System.Drawing.Color.Transparent;
            this.lblDomain.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDomain.ForeColor = System.Drawing.Color.White;
            this.lblDomain.Location = new System.Drawing.Point(8, 91);
            this.lblDomain.Name = "lblDomain";
            this.lblDomain.Size = new System.Drawing.Size(84, 16);
            this.lblDomain.TabIndex = 44;
            this.lblDomain.Text = "Domain";
            this.lblDomain.Visible = false;
            // 
            // cboLoginWith
            // 
            appearance11.FontData.Name = "Microsoft Sans Serif";
            appearance11.FontData.SizeInPoints = 9F;
            this.cboLoginWith.Appearance = appearance11;
            this.cboLoginWith.AutoSize = false;
            this.cboLoginWith.DropDownListWidth = -1;
            this.cboLoginWith.DropDownStyle = Infragistics.Win.DropDownStyle.DropDownList;
            this.cboLoginWith.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            valueListItem3.DataValue = "1";
            valueListItem3.DisplayText = "POS User";
            valueListItem4.DataValue = "0";
            valueListItem4.DisplayText = "Windows User";
            valueListItem1.DataValue = "3";
            valueListItem1.DisplayText = "Azure User";
            this.cboLoginWith.Items.AddRange(new Infragistics.Win.ValueListItem[] {
            valueListItem3,
            valueListItem4,
            valueListItem1});
            this.cboLoginWith.Location = new System.Drawing.Point(95, 30);
            this.cboLoginWith.Name = "cboLoginWith";
            this.cboLoginWith.Size = new System.Drawing.Size(170, 25);
            this.cboLoginWith.TabIndex = 0;
            this.cboLoginWith.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.cboLoginWith.ValueChanged += new System.EventHandler(this.cboLoginWith_ValueChanged);
            // 
            // lblLoginWith
            // 
            appearance12.ForeColor = System.Drawing.Color.Black;
            this.lblLoginWith.Appearance = appearance12;
            this.lblLoginWith.BackColorInternal = System.Drawing.Color.Transparent;
            this.lblLoginWith.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLoginWith.ForeColor = System.Drawing.Color.White;
            this.lblLoginWith.Location = new System.Drawing.Point(8, 34);
            this.lblLoginWith.Name = "lblLoginWith";
            this.lblLoginWith.Size = new System.Drawing.Size(84, 16);
            this.lblLoginWith.TabIndex = 42;
            this.lblLoginWith.Text = "Login with";
            // 
            // lblTouchID
            // 
            this.lblTouchID.BackColor = System.Drawing.Color.Transparent;
            this.lblTouchID.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.lblTouchID.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTouchID.ForeColor = System.Drawing.Color.Gold;
            this.lblTouchID.Image = ((System.Drawing.Image)(resources.GetObject("lblTouchID.Image")));
            this.lblTouchID.Location = new System.Drawing.Point(270, 30);
            this.lblTouchID.Name = "lblTouchID";
            this.lblTouchID.Size = new System.Drawing.Size(85, 120);
            this.lblTouchID.TabIndex = 41;
            this.lblTouchID.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.lblTouchID.Visible = false;
            // 
            // lblLoginAttempt
            // 
            appearance13.ForeColor = System.Drawing.Color.Red;
            appearance13.TextHAlignAsString = "Center";
            this.lblLoginAttempt.Appearance = appearance13;
            this.lblLoginAttempt.BackColorInternal = System.Drawing.Color.Transparent;
            this.lblLoginAttempt.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLoginAttempt.ForeColor = System.Drawing.Color.White;
            this.lblLoginAttempt.Location = new System.Drawing.Point(8, 157);
            this.lblLoginAttempt.Name = "lblLoginAttempt";
            this.lblLoginAttempt.Size = new System.Drawing.Size(344, 21);
            this.lblLoginAttempt.TabIndex = 40;
            this.lblLoginAttempt.Text = "Login Attempt(1/6)";
            // 
            // groupBox1
            // 
            appearance14.BorderColor = System.Drawing.Color.DarkGray;
            this.groupBox1.ContentAreaAppearance = appearance14;
            this.groupBox1.Controls.Add(this.lblPOSV);
            this.groupBox1.Controls.Add(this.ultraLabel4);
            this.groupBox1.Controls.Add(this.ultraLabel3);
            this.groupBox1.Controls.Add(this.lblPharmacy);
            this.groupBox1.Location = new System.Drawing.Point(10, 280);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(363, 95);
            this.groupBox1.TabIndex = 40;
            // 
            // lblPOSV
            // 
            appearance15.ForeColor = System.Drawing.Color.Black;
            appearance15.TextHAlignAsString = "Left";
            appearance15.TextVAlignAsString = "Middle";
            this.lblPOSV.Appearance = appearance15;
            this.lblPOSV.BackColorInternal = System.Drawing.Color.Transparent;
            this.lblPOSV.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPOSV.ForeColor = System.Drawing.Color.White;
            this.lblPOSV.Location = new System.Drawing.Point(9, 35);
            this.lblPOSV.Name = "lblPOSV";
            this.lblPOSV.Size = new System.Drawing.Size(344, 15);
            this.lblPOSV.TabIndex = 42;
            this.lblPOSV.Text = "PrimePOS V 2.0";
            // 
            // ultraLabel4
            // 
            appearance16.ForeColor = System.Drawing.Color.Black;
            appearance16.TextHAlignAsString = "Left";
            appearance16.TextVAlignAsString = "Middle";
            this.ultraLabel4.Appearance = appearance16;
            this.ultraLabel4.BackColorInternal = System.Drawing.Color.Transparent;
            this.ultraLabel4.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultraLabel4.ForeColor = System.Drawing.Color.White;
            this.ultraLabel4.Location = new System.Drawing.Point(9, 75);
            this.ultraLabel4.Name = "ultraLabel4";
            this.ultraLabel4.Size = new System.Drawing.Size(344, 15);
            this.ultraLabel4.TabIndex = 41;
            this.ultraLabel4.Text = "All rights reserved.";
            // 
            // ultraLabel3
            // 
            appearance17.ForeColor = System.Drawing.Color.Black;
            appearance17.TextHAlignAsString = "Left";
            appearance17.TextVAlignAsString = "Middle";
            this.ultraLabel3.Appearance = appearance17;
            this.ultraLabel3.BackColorInternal = System.Drawing.Color.Transparent;
            this.ultraLabel3.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultraLabel3.ForeColor = System.Drawing.Color.White;
            this.ultraLabel3.Location = new System.Drawing.Point(9, 55);
            this.ultraLabel3.Name = "ultraLabel3";
            this.ultraLabel3.Size = new System.Drawing.Size(344, 15);
            this.ultraLabel3.TabIndex = 40;
            this.ultraLabel3.Text = "(c)Copyrights 2004-2022 Micro Merchant Systems, Inc.";
            // 
            // lblPharmacy
            // 
            appearance18.ForeColor = System.Drawing.Color.Black;
            appearance18.TextHAlignAsString = "Center";
            this.lblPharmacy.Appearance = appearance18;
            this.lblPharmacy.BackColorInternal = System.Drawing.Color.Transparent;
            this.lblPharmacy.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPharmacy.ForeColor = System.Drawing.Color.White;
            this.lblPharmacy.Location = new System.Drawing.Point(10, 10);
            this.lblPharmacy.Name = "lblPharmacy";
            this.lblPharmacy.Size = new System.Drawing.Size(344, 20);
            this.lblPharmacy.TabIndex = 39;
            // 
            // groupBox2
            // 
            this.groupBox2.BackColorInternal = System.Drawing.Color.White;
            appearance19.BorderColor = System.Drawing.Color.DarkGray;
            this.groupBox2.ContentAreaAppearance = appearance19;
            this.groupBox2.Controls.Add(this.pctPrimePOS);
            this.groupBox2.Controls.Add(this.lblPrimePOS);
            this.groupBox2.Controls.Add(this.pictureBox1);
            this.groupBox2.Location = new System.Drawing.Point(10, 10);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(363, 69);
            this.groupBox2.TabIndex = 42;
            this.groupBox2.Tag = "NOCOLOR";
            // 
            // pctPrimePOS
            // 
            this.pctPrimePOS.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pctPrimePOS.BackColor = System.Drawing.Color.White;
            this.pctPrimePOS.Location = new System.Drawing.Point(131, 3);
            this.pctPrimePOS.Margin = new System.Windows.Forms.Padding(0);
            this.pctPrimePOS.Name = "pctPrimePOS";
            this.pctPrimePOS.Size = new System.Drawing.Size(229, 63);
            this.pctPrimePOS.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pctPrimePOS.TabIndex = 42;
            this.pctPrimePOS.TabStop = false;
            // 
            // lblPrimePOS
            // 
            appearance20.BackColor = System.Drawing.Color.White;
            appearance20.BackColorAlpha = Infragistics.Win.Alpha.Opaque;
            appearance20.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            appearance20.TextHAlignAsString = "Center";
            appearance20.TextVAlignAsString = "Middle";
            this.lblPrimePOS.Appearance = appearance20;
            this.lblPrimePOS.BackColorInternal = System.Drawing.Color.Transparent;
            this.lblPrimePOS.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPrimePOS.ForeColor = System.Drawing.Color.White;
            this.lblPrimePOS.Location = new System.Drawing.Point(131, 3);
            this.lblPrimePOS.Name = "lblPrimePOS";
            this.lblPrimePOS.Size = new System.Drawing.Size(229, 63);
            this.lblPrimePOS.TabIndex = 39;
            this.lblPrimePOS.Text = "PrimePOS";
            this.lblPrimePOS.Visible = false;
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.White;
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(3, 3);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(128, 63);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 41;
            this.pictureBox1.TabStop = false;
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // frmLogin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.SteelBlue;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(384, 386);
            this.ControlBox = false;
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.grpLogin);
            this.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.Name = "frmLogin";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Login";
            this.TopMost = true;
            this.Closing += new System.ComponentModel.CancelEventHandler(this.frmLogin_Closing);
            this.Load += new System.EventHandler(this.frmLogin_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmLogin_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.txtUserName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPassward)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grpLogin)).EndInit();
            this.grpLogin.ResumeLayout(false);
            this.grpLogin.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtDomain)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboLoginWith)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupBox1)).EndInit();
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.groupBox2)).EndInit();
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pctPrimePOS)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion Windows Form Designer generated code

        private void btnOk_Click(object sender, System.EventArgs e)
        {
            POS_Core.ErrorLogging.Logs.Logger(clsPOSDBConstants.Log_Module_Login, " btnOk_Click() with User: " + txtUserName.Text, clsPOSDBConstants.Log_Entering);
            string sIPAddress;
            string strUserID = "";
            string sID = "";
            string sBarcodePassword = string.Empty;
            string sPassword = "";
            try
            {
                //Added By Shitaljit(QuicSolv) on july 1 2011
                //To Stop Multiple Instance of POS running With Same Station ID
                if (!clsMain.bAutomation)
                    clsMain.CheckPOSInstance();
                //END of added by Shitaljit.

                if (txtUserName.Text.Contains(clsPOSDBConstants.UserBarcodeSeperatorString) && txtUserName.Text.Length > 4)
                {
                    //Folowing code added by shitaljit for Barcode Login.
                    POS_Core.ErrorLogging.Logs.Logger(clsPOSDBConstants.Log_Module_Login, " btnOk_Click() Validating Barcode Login", "Started");
                    LoginWithBarCode(out sID, out strUserID, out sIPAddress, out sPassword);
                }
                else if (string.IsNullOrEmpty(this.txtUserName.Text) == true || string.IsNullOrEmpty(this.txtPassward.Text) == true)
                {
                    throw new Exception("Please enter valid User Name and Password.");
                }
                else
                {
                    POS_Core.ErrorLogging.Logs.Logger(clsPOSDBConstants.Log_Module_Login, " btnOk_Click() Validating  Login", "Started");
                    #region PRIMEPOS-2616 14-Dec-2018 JY Added
                    //if (cboLoginWith.SelectedIndex == 0)
                    if (!clsMain.bAutomation && Configuration.convertNullToInt(cboLoginWith.SelectedItem.DataValue) == 0)   //PRIMEPOS-2989 12-Aug-2021 JY Added    //PRIMEPOS-3039 16-Dec-2021 JY Modified
                    {
                        string strErrMsg;
                        LoginWithWindowsAuthentication(this.txtUserName.Text, this.txtPassward.Text, out strErrMsg);
                        if (strErrMsg != string.Empty)
                        {
                            throw new Exception(strErrMsg);
                        }
                    }
                    #endregion

                    oclsLogin.ValidateUserNamePassward(this.txtUserName.Text, this.txtPassward.Text, out sIPAddress);
                    CheckPrivilages(this.txtUserName.Text);

                    if (this.txtUserName.Text.Trim().ToUpper() == POS_Core.Resources.Configuration.UserName.Trim().ToUpper())
                    {
                        AddAllowHouseChargePaytypeAccessRight(this.txtUserName.Text.Trim());    //Sprint-24 - PRIMEPOS-2290 19-Jan-2017 JY Added
                        AddPSEItemListAccessRight(this.txtUserName.Text.Trim());    //Sprint-25 - PRIMEPOS-2379 15-Mar-2017 JY Added
                        AddQuantityOverrideAccessRight(this.txtUserName.Text.Trim());   //Sprint-26 - PRIMEPOS-2416 25-Jul-2017 JY Added
                        AddStandAloneReturnAccessRight(this.txtUserName.Text.Trim());   //Sprint-26 - PRIMEPOS-2383 08-Aug-2017 JY Added
                        AddAllowCheckPaymentAccessRight(this.txtUserName.Text.Trim());   //PRIMEPOS-2539 06-Jul-2018 JY Added
                        AddDisplayItemCostAccessRight(this.txtUserName.Text.Trim());    //PRIMEPOS-2464 26-Mar-2020 JY Added

                        #region PRIMEPOS-2676 20-May-2019 JY Added
                        AddTransSettingsAccessRight(this.txtUserName.Text.Trim());
                        AddRxSettingsAccessRight(this.txtUserName.Text.Trim());
                        AddPrimePOSettingsAccessRight(this.txtUserName.Text.Trim());
                        AddCLPSettingsAccessRight(this.txtUserName.Text.Trim());
                        #endregion
                        AddInventoryReceivedAccessRight(this.txtUserName.Text.Trim());  //PRIMEPOS-3141 27-Oct-2022 JY Added

                        #region PRIMEPOS-2643 05-Sep-2019
                        if (Configuration.CInfo.PIEnable)
                        {
                            if (!string.IsNullOrWhiteSpace(MMSUtil.ReadIni.GetIniSetting("InterfaceDB=").Trim()))
                            {
                                Hashtable oParam = new Hashtable();
                                oParam.Add("DOCONNECT", Configuration.CInfo.PIEnable);
                                oParam.Add("URL", Configuration.CInfo.PIURL);
                                oParam.Add("STATIONID", Configuration.StationID);
                                oParam.Add("USERNAME", Configuration.CInfo.PUser);
                                oParam.Add("PASSWORD", Configuration.CInfo.PPassword);
                                //bool res = NativeEncryption.EncryptText(PW, ref PW);
                                Configuration.eInterfaceStatus = EventHub.getInstance().Initialize(oParam);
                                if (Configuration.eInterfaceStatus == EventHub.InterfaceStatus.DatabaseNotExists)
                                    Resources.Message.Display("PrimeInterface Database is not exist.\nPlease contact MMS support.", FormStartPosition.CenterScreen);
                                else if (Configuration.eInterfaceStatus == EventHub.InterfaceStatus.ServiceIsNotConnected)
                                    Resources.Message.Display("PrimeInterface is not connected. Please check whether service is running or configured properly in PrimePOS.", FormStartPosition.CenterScreen);
                            }
                            else
                                Resources.Message.Display("PrimeInterface Database is not configured.\nPlease contact MMS support.", FormStartPosition.CenterScreen);

                        }
                        #endregion
                    }
                    Configuration.AddIPLaneToConfig();    //PRIMEPOS-3024 08-Nov-2021 JY Added
                    #region PRIMEPOS-2651 07-Mar-2022 JY Added
                    if (string.IsNullOrWhiteSpace(MMSUtil.ReadIni.GetIniSetting("GSDDDB=").Trim()))
                    {
                        try
                        {
                            string strExePath = Application.ExecutablePath;
                            string strIniFilePath = string.Empty;
                            string strIniFileName = System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName + ".ini";

                            try
                            {
                                int nPos = strExePath.LastIndexOf(".");
                                if (nPos > 0)
                                    strIniFilePath = strExePath.Substring(0, nPos + 1) + "ini";
                                else
                                    strIniFilePath = strExePath + ".ini";

                                if (System.IO.File.Exists(strIniFilePath)) //check file exists
                                {
                                    string strFileData = Environment.NewLine + "GSDDDB=GSDD";
                                    FileStream outStream = null;
                                    StreamWriter Writer = null;
                                    FileSecurity security = new FileSecurity();
                                    FileIOPermission permission = new FileIOPermission(FileIOPermissionAccess.AllAccess, strIniFilePath);
                                    permission.AddPathList(FileIOPermissionAccess.Write | FileIOPermissionAccess.Read, strIniFilePath);
                                    permission.Demand();

                                    using (outStream = new FileStream(strIniFilePath, FileMode.OpenOrCreate | FileMode.Append, FileAccess.Write))
                                    {
                                        using (Writer = new StreamWriter(outStream))
                                        {
                                            Writer.WriteLine(strFileData);
                                        }
                                    }
                                }
                            }
                            catch { }
                            if (string.IsNullOrWhiteSpace(MMSUtil.ReadIni.GetIniSetting("GSDDDB=").Trim()))
                                Resources.Message.Display("GSDD Database is not configured in .ini file.\nPlease contact MMS support.", FormStartPosition.CenterScreen);
                        }
                        catch (Exception Ex)
                        {
                        }
                    }
                    #endregion
                    POS_Core.ErrorLogging.Logs.Logger(clsPOSDBConstants.Log_Module_Login, " btnOk_Click() Validating  Login", "Completed");
                }

                oclsLogin.ChangePasswordAtLogin(this.txtUserName.Text); //PRIMEPOS-2577 15-Aug-2018 JY Added

                //Added By shitaljit on 10 Oct 2012 to bypass password expire logic if Enforce Complex Password is not true.
                if (POS_Core.Resources.Configuration.CInfo.EnforceComplexPassword == true && POS_Core.Resources.Configuration.CInfo.PasswordExpirationDays > 0)
                {
                    oclsLogin.CheckResetPassword(this.txtUserName.Text);
                }
                POS_Core.Resources.Configuration.ConnectionStringType = "UserDatabase";
                POS_Core.Resources.Configuration.SQLUserID = DBConfig.UserName; //PRIMEPOS-3360
                POS_Core.Resources.Configuration.SQLUserPassword = DBConfig.Passward; //PRIMEPOS-3360
                POS_Core.Resources.Configuration.m_ConnString = string.Empty;
                POS_Core.Resources.Configuration.CashierID = oclsLogin.GetCashierID(Configuration.UserName);//PRIMEPOS-2664
                ErrorHandler.SaveLog((int)LogENUM.Login, this.txtUserName.Text.ToString(), "Success", "Login Successful");
                POS_Core.ErrorLogging.Logs.Logger(clsPOSDBConstants.Log_Module_Login, " btnOk_Click() ", "Login Successful");
                ErrorHandler.UpdateUserLoginAttempt(this.txtUserName.Text, 0, false);
                //Added By Shitaljit(QuicSolv) 0n 8 oct 2011

                #region System & User Notes

                Notes oNotes = new Notes();
                NotesData oNotesData = new NotesData();
                frmCustomerNotesView ofrmCustomerNotesView = null;
                string whereClause = "";
                whereClause = " WHERE " + clsPOSDBConstants.Notes_Fld_EntityId + "= '" + "SYSTEM" + "'  AND  " + clsPOSDBConstants.Notes_Fld_EntityType + "= '" + clsEntityType.SystemNote + "' AND " + clsPOSDBConstants.Notes_Fld_POPUPMSG + "= '" + true + "'";
                oNotesData = oNotes.PopulateList(whereClause);
                if (oNotesData.Notes.Rows.Count > 0)
                {
                    ofrmCustomerNotesView = new frmCustomerNotesView("SYSTEM", clsEntityType.SystemNote);
                    ofrmCustomerNotesView.ShowDialog();
                }
                whereClause = " WHERE " + clsPOSDBConstants.Notes_Fld_EntityId + "= '" + POS_Core.Resources.Configuration.UserName + "'  AND  " + clsPOSDBConstants.Notes_Fld_EntityType + "= '" + clsEntityType.UserNote + "' AND " + clsPOSDBConstants.Notes_Fld_POPUPMSG + "= '" + true + "'";
                oNotesData = oNotes.PopulateList(whereClause);
                if (oNotesData.Notes.Rows.Count > 0)
                {
                    ofrmCustomerNotesView = new frmCustomerNotesView(POS_Core.Resources.Configuration.UserName, clsEntityType.UserNote);
                    ofrmCustomerNotesView.ShowDialog();
                }

                #endregion System & User Notes

                //PRIMEPOS-3167 07-Nov-2022 JY Commented
                //#region Logic To check PrimePO Enable stations.
                //Search oSearch = new Search();
                //string sSQL = "";
                //string strStations = "";
                //DataSet ds = null;
                //sSQL = "SELECT STATIONID FROM Util_POSSET WHERE USEPRIMEPO =1 ";
                //ds = oSearch.SearchData(sSQL);
                //if (ds != null)
                //{
                //    if (ds.Tables[0].Rows.Count > 1)
                //    {
                //        foreach (DataRow dr in ds.Tables[0].Rows)
                //        {
                //            strStations += dr["STATIONID"].ToString() + ",";
                //        }
                //        strStations = strStations.Substring(0, strStations.Length - 1);
                //        POS_Core_UI.Resources.Message.Display("PrimePO is configured on station " + strStations + "." + "\nThis may cause a conflict and result in PrimePOS to communicate with POServer improperly.\nPlease contact Micro Merchant Systems(MMS).", FormStartPosition.CenterScreen);
                //    }
                //}
                //#endregion Logic To check PrimePO Enable stations.

                //Added By Shitaljit(QuicSolv) 0n 8 oct 2011
                frmCreateNewPurchaseOrder ofrmPOOrder = new frmCreateNewPurchaseOrder();
                ofrmPOOrder = frmCreateNewPurchaseOrder.getInstance();
                if (ofrmPOOrder != null)
                {
                    ofrmPOOrder.BringToFront();
                }
                //Following Code Added by Krishna on 12 October 2011
                POS_Core.ErrorLogging.Logs.Logger(clsPOSDBConstants.Log_Module_Login, " btnOk_Click()  ", "Checking for Last DB backup");
                POS_Core.DataAccess.DbBackupSvr DbBackupSvr = new POS_Core.DataAccess.DbBackupSvr();
                DateTime? LastBackupDate = DbBackupSvr.GetLastBackupDate();
                POS_Core.ErrorLogging.Logs.Logger(clsPOSDBConstants.Log_Module_Login, " btnOk_Click()  ", " Last DB backup Date" + LastBackupDate.ToString());
                int NoOfDays = 0;
                if (POS_Core.Resources.Configuration.CInfo.DaysForWarn == 0)
                    return;
                if (LastBackupDate == null)
                {
                    if (Convert.ToInt32(POS_Core.Resources.Configuration.StationID) == 1)
                        POS_Core_UI.Resources.Message.Display("PrimePOS backup has not been done. Please backup POS.", FormStartPosition.CenterScreen);
                    return;
                }
                else
                {
                    TimeSpan diffDate = DateTime.Now.Date.Subtract((DateTime)LastBackupDate.Value.Date);
                    NoOfDays = diffDate.Days;
                }
                string NumberOfDays;
                if (NoOfDays > 1 && NoOfDays <= POS_Core.Resources.Configuration.CInfo.DaysForWarn + 1 && Convert.ToInt32(POS_Core.Resources.Configuration.StationID) == 1)
                {
                    NumberOfDays = Convert.ToString(NoOfDays - 1);
                    POS_Core_UI.Resources.Message.Display("PrimePOS backup has not been done for past " + NumberOfDays + " days. Please backup POS.", FormStartPosition.CenterScreen);
                }
                if (NoOfDays > POS_Core.Resources.Configuration.CInfo.DaysForWarn + 1)
                {
                    NumberOfDays = Convert.ToString(NoOfDays - 1);
                    POS_Core_UI.Resources.Message.Display("PrimePOS backup has not been done for past " + NumberOfDays + " days. Please backup POS. PrimePOS will exit now", FormStartPosition.CenterScreen);
                    Application.Exit();
                }
                POS_Core.ErrorLogging.Logs.Logger(clsPOSDBConstants.Log_Module_Login, " btnOk_Click()  ", "Checking for Last DB backup completed");
                //Till here Added by krishna on 12 October 2011
            }
            catch (POSExceptions exp)
            {
                clsCoreUIHelper.ShowBtnIconMsg(exp.Message, "User Authentication", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //string sUserID = (string.IsNullOrEmpty(sID) == false) ? sID : this.txtPassward.Text;  //PRIMEPOS-2575 07-Aug-2018 JY Commented - When we are overriding by any user and unfortunately that user got expired, then it will navigate to "Change Password" screen with wrong user information. Ideally, it should be the overriden user.
                string sUserID = (string.IsNullOrEmpty(sID) == false) ? sID : this.txtUserName.Text;    //PRIMEPOS-2575 07-Aug-2018 JY Added
                switch (exp.ErrNumber)
                {
                    case (long)POSErrorENUM.User_InValidPassward:
                        if (txtUserName.Text.Contains(clsPOSDBConstants.UserBarcodeSeperatorString) && txtUserName.Text.Length > 4)
                        {
                            ErrorHandler.SaveLog((int)LogENUM.Login, strUserID, "Fail", "Invalid Password");
                            this.txtUserName.Focus();
                        }
                        else
                        {
                            ErrorHandler.SaveLog((int)LogENUM.Login, sUserID, "Fail", "Invalid Password");
                        }
                        POS_Core.ErrorLogging.Logs.Logger(clsPOSDBConstants.Log_Module_Login, " btnOk_Click()  Login Fail ", "Invalid Password");
                        txtPassward.Focus();
                        break;
                    case (long)POSErrorENUM.User_InvalidUserName:
                        ErrorHandler.SaveLog((int)LogENUM.Login, sUserID, "Fail", "Invalid UserName");
                        POS_Core.ErrorLogging.Logs.Logger(clsPOSDBConstants.Log_Module_Login, " btnOk_Click()  Login Fail ", "Invalid UserName");
                        txtUserName.Focus();
                        break;
                    case (long)POSErrorENUM.User_InvalidSecurityLevel:
                        ErrorHandler.SaveLog((int)LogENUM.Login, sUserID, "Fail", "Invalid SecurityLevel");
                        POS_Core.ErrorLogging.Logs.Logger(clsPOSDBConstants.Log_Module_Login, " btnOk_Click()  Login Fail ", "Invalid SecurityLevel");
                        txtUserName.Text = POS_Core.Resources.Configuration.UserName;
                        txtPassward.Text = "";
                        txtUserName.Focus();
                        break;
                    case (long)POSErrorENUM.User_Locked:
                        ErrorHandler.SaveLog((int)LogENUM.Account_Locked, sUserID, "Fail", "User account locked");
                        POS_Core.ErrorLogging.Logs.Logger(clsPOSDBConstants.Log_Module_Login, " btnOk_Click()  Login Fail ", "Invalid SecurityLevel");
                        ErrorHandler.UpdateUserLoginAttempt(this.txtUserName.Text, oclsLogin.MaxAttempt, true);
                        break;
                    case (long)POSErrorENUM.User_ResetPassword:
                    case (long)POSErrorENUM.User_ChangePasswordAtLogin: //PRIMEPOS-2577 15-Aug-2018 JY Added
                        using (var objChangPwd = new frmChangePassword(sUserID, txtPassward.Text))  //PRIMEPOS-2575 07-Aug-2018 JY sending userid and password
                        {
                            objChangPwd.StartPosition = FormStartPosition.CenterScreen;
                            objChangPwd.EnsurePasswordChange = true;
                            objChangPwd.ShowDialog();
                        }
                        break;
                }
                //if(oclsLogin.MaxAttempt <= 6)
                lblLoginAttempt.Text = "Login Attempt(" + oclsLogin.MaxAttempt.ToString() + "/6)";
                POS_Core.ErrorLogging.Logs.Logger(clsPOSDBConstants.Log_Module_Login, " btnOk_Click()  Login Fail ", "Login Attempt(" + oclsLogin.MaxAttempt.ToString() + "/6)");
                //else
                //    lblLoginAttempt.Text = "Login Attempt(6/6)";
                POS_Core.Resources.Configuration.SQLUserID = "";
                POS_Core.Resources.Configuration.SQLUserPassword = "";
            }
            catch (Exception exp)
            {
                m_Canceled = true;  //PRIMEPOS-Issue 16-Aug-2019 JY Added
                POS_Core.Resources.Configuration.SQLUserID = "";
                POS_Core.Resources.Configuration.SQLUserPassword = "";
                clsCoreUIHelper.ShowBtnIconMsg(exp.Message, "User Authentication", MessageBoxButtons.OK, MessageBoxIcon.Error);
                POS_Core.ErrorLogging.Logs.Logger(clsPOSDBConstants.Log_Module_Login, " btnOk_Click()  Login Fail ", clsPOSDBConstants.Log_Exception_Occured + exp.StackTrace.ToString());
                this.txtUserName.Focus();
            }
        }

        private void CheckPrivilages(string strUserName)
        {
            POS_Core.ErrorLogging.Logs.Logger(clsPOSDBConstants.Log_Module_Login, " CheckPrivilages() ", clsPOSDBConstants.Log_Entering);
            if (strPrevlige.Trim() == "")
            {
                /*if (Configuration.CPOSSet.RestrictConcurrentLogin==true)
                {
                    if (isUserAlreadyLoggedIn(sIPAddress))
                    {
                        throw(new Exception("This user is already logged in from station " ));
                    }
                }*/

                strPrevlige = "";
                m_Canceled = false;
                if (POS_Core.Resources.Configuration.UserName != strUserName.Trim() && POS_Core.Resources.Configuration.UserName != "")
                    oclsLogin.ValidateSecuriyLevel(strUserName.Trim());

                if (POS_Core.Resources.Configuration.CPOSSet.SingleUserLogin == true)
                {
                    if (!clsMain.bAutomation)
                        oclsLogin.ValidateSingleUserLogin(strUserName.Trim());
                }
                POS_Core.Resources.Configuration.UserName = strUserName.Trim();
                POS_Core.Resources.Configuration.GetUserSettings(POS_Core.Resources.Configuration.UserName);
                this.oclsLogin.GetUsersRole(POS_Core.Resources.Configuration.UserName);
                
                PreferenceSvr oPreSvr = new PreferenceSvr();
                if (!clsMain.bAutomation)   //PRIMEPOS-3080 25-Mar-2022 JY Added condition
                {
                    if (oPreSvr.UpdateEPrimeRxAppSettings(POS_Core.Resources.Configuration.CInfo))
                        throw new Exception("AppSettings for ePrimeRx connection have been synced with saved preference. \nPlease hit Cancel to re-start POS.");
                }
                try
                {
                    System.Data.IDbTransaction oTrans = null;
                    System.Data.IDbConnection oConn = null;
                    PharmBL oPhBl = new PharmBL();
                    System.Data.DataTable dtDF0001 = oPhBl.GetPhInfo();

                    #region Sprint-20 15-Jun-2015 JY Added to get NABP from PrimeRx for Auto updater
                    try
                    {
                        if (dtDF0001.Rows.Count > 0)
                        {
                            if (POS_Core.Resources.Configuration.CInfo.StoreID.Trim().ToUpper() != POS_Core.Resources.Configuration.convertNullToString(dtDF0001.Rows[0]["NABP"]).Trim().ToUpper())
                            {
                                POS_Core.Resources.Configuration.CInfo.StoreID = POS_Core.Resources.Configuration.convertNullToString(dtDF0001.Rows[0]["NABP"]).Trim().ToUpper();

                                oConn = DataFactory.CreateConnection(POS_Core.Resources.Configuration.ConnectionString);
                                oTrans = oConn.BeginTransaction(IsolationLevel.ReadCommitted);

                                oPreSvr.UpdateStoreId(oTrans, POS_Core.Resources.Configuration.convertNullToString(dtDF0001.Rows[0]["NABP"]).Trim().ToUpper());
                                oTrans.Commit();
                            }
                            else if (POS_Core.Resources.Configuration.CInfo.StoreID.Trim().ToUpper() == "")
                                POS_Core.Resources.Configuration.CInfo.StoreID = "0";
                        }
                        else
                        {
                            if (POS_Core.Resources.Configuration.CInfo.StoreID.Trim().ToUpper() == "")
                                POS_Core.Resources.Configuration.CInfo.StoreID = "0";
                        }
                    }
                    catch (Exception Ex)
                    {
                        if (oTrans != null)
                            oTrans.Rollback();

                        if (POS_Core.Resources.Configuration.CInfo.StoreID.Trim().ToUpper() == "")
                            POS_Core.Resources.Configuration.CInfo.StoreID = "0";
                        throw (Ex);
                    }
                    #endregion

                    #region PRIMEPOS-2667 12-Apr-2019 JY Added
                    try
                    {
                        if (dtDF0001.Rows.Count > 0)
                        {
                            if (Configuration.CInfo.PHNPINO.Trim().ToUpper() != Configuration.convertNullToString(dtDF0001.Rows[0]["PHNPINO"]).Trim().ToUpper())
                            {
                                Configuration.CInfo.PHNPINO = Configuration.convertNullToString(dtDF0001.Rows[0]["PHNPINO"]).Trim().ToUpper();
                                //PreferenceSvr oPreSvr = new PreferenceSvr();

                                oConn = DataFactory.CreateConnection(Configuration.ConnectionString);
                                oTrans = oConn.BeginTransaction(IsolationLevel.ReadCommitted);

                                oPreSvr.UpdatePHNPINO(oTrans, Configuration.convertNullToString(dtDF0001.Rows[0]["PHNPINO"]).Trim().ToUpper());
                                oTrans.Commit();
                            }
                        }
                    }
                    catch
                    {
                        if (oTrans != null)
                            oTrans.Rollback();
                    }
                    #endregion

                    #region PRIMEPOS-3478 Assign NPI to NBSStoreID
                    if(string.IsNullOrWhiteSpace(Configuration.CSetting.NBSStoreID))
                    {
                        Configuration.CSetting.NBSStoreID = Configuration.CInfo.PHNPINO.Trim().ToUpper();
                        oConn = DataFactory.CreateConnection(Configuration.ConnectionString);
                        oTrans = oConn.BeginTransaction(IsolationLevel.ReadCommitted);
                        //The issue can be like if we set the different NBSStoreId then while doing the NBS
                        oPreSvr.UpdatePrimeRxPayDetails("NBSStoreID", Configuration.CInfo.PHNPINO.Trim().ToUpper());
                        oTrans.Commit();
                    }
                    #endregion
                }
                catch (Exception ex)
                {
                    if (Configuration.CPOSSet.UsePrimeRX)   //PRIMEPOS-2843 10-May-2020 JY Added
                    {
                        Resources.Message.Display(ex.Message, Configuration.ApplicationName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }

                clsCoreUIHelper.setColorSchecme(frmMain.getInstance());

                allowClose = true;
                //this.Close(); //PRIMEPOS-3039 16-Dec-2021 JY Commented
                this.BeginInvoke(new MethodInvoker(Close)); //PRIMEPOS-3039 16-Dec-2021 JY Added
            }
            else if (strPrevlige == clsPOSDBConstants.UserMaxDiscountLimit)
            {
                if (UserPriviliges.IsUserHasPriviligesToOverrideInvoiceDiscount(strUserName, frmPOSTransaction.InvDicsValueToVerify) == false)
                {
                    this.txtUserName.Focus();
                    ErrorHandler.throwCustomError(POSErrorENUM.Users_UserNotHavePriviligesForThisAction);
                }
                else
                {
                    allowClose = true;
                    strPrevlige = "";
                    m_Canceled = false;
                    this.Close();
                }
            }
            else if (strPrevlige == clsPOSDBConstants.UserMaxTransactionLimit)
            {
                if (UserPriviliges.IsUserHasPriviligesToOverrideTransactionAmount(strUserName, frmPOSTransaction.TransactionAmount) == false)
                {
                    this.txtUserName.Focus();
                    ErrorHandler.throwCustomError(POSErrorENUM.Users_UserNotHavePriviligesForThisAction);
                }
                else
                {
                    allowClose = true;
                    strPrevlige = "";
                    m_Canceled = false;
                    this.Close();
                }
            }
            else if (strPrevlige == clsPOSDBConstants.UserMaxReturnTransLimit)  //Sprint-23 - PRIMEPOS-2303 24-May-2016 JY Added 
            {
                if (UserPriviliges.IsUserHasPriviligesToOverrideReturnTransAmount(strUserName, Math.Abs(frmPOSTransaction.TransactionAmount)) == false)
                {
                    this.txtUserName.Focus();
                    ErrorHandler.throwCustomError(POSErrorENUM.Users_UserNotHavePriviligesForThisAction);
                }
                else
                {
                    allowClose = true;
                    strPrevlige = "";
                    m_Canceled = false;
                    this.Close();
                }
            }
            else if (strPrevlige == clsPOSDBConstants.UserMaxTenderedAmountLimit)  //Sprint-25 - PRIMEPOS-2411 21-Apr-2017 JY Added 
            {
                if (UserPriviliges.IsUserHasPriviligesToOverrideTenderedAmount(strUserName, Math.Abs(POSPayTypeList.TransactionAmount)) == false)
                {
                    this.txtUserName.Focus();
                    ErrorHandler.throwCustomError(POSErrorENUM.Users_UserNotHavePriviligesForThisAction);
                }
                else
                {
                    allowClose = true;
                    strPrevlige = "";
                    m_Canceled = false;
                    this.Close();
                }
            }
            else if (strPrevlige == clsPOSDBConstants.UserUsePayOutCatagory)
            {
                if (UserPriviliges.IsUserHasPriviligesToOverrideUsePayOutCat(strUserName, frmPayOut.intPayOutId) == false)
                {
                    this.txtUserName.Focus();
                    ErrorHandler.throwCustomError(POSErrorENUM.Users_UserNotHavePriviligesForThisAction);
                }
                else
                {
                    allowClose = true;
                    strPrevlige = "";
                    m_Canceled = false;
                    this.Close();
                }
            }
            else if (strPrevlige == clsPOSDBConstants.StnCloseCashLimit)
            {
                if (UserPriviliges.IsUserHasPriviliges(UserPriviliges.Modules.POSTransaction.ID, UserPriviliges.Screens.POSTransaction.ID, UserPriviliges.Permissions.OverrideMaxStationCloseCashLimit.ID, strUserName) == false)
                {
                    this.txtUserName.Focus();
                    ErrorHandler.throwCustomError(POSErrorENUM.Users_UserNotHavePriviligesForThisAction);
                }
                else
                {
                    allowClose = true;
                    strPrevlige = "";
                    m_Canceled = false;
                    this.Close();
                }
            }
            else if (strPrevlige == clsPOSDBConstants.AllowHouseChargePaytype)  //Sprint-24 - PRIMEPOS-2290 16-Jan-2017 JY Added
            {
                if (UserPriviliges.IsUserHasPriviligesToOverrideAllowHouseChargePaytype(strUserName) == false)
                {
                    this.txtUserName.Focus();
                    ErrorHandler.throwCustomError(POSErrorENUM.Users_UserNotHavePriviligesForThisAction);
                }
                else
                {
                    allowClose = true;
                    strPrevlige = "";
                    m_Canceled = false;
                    this.Close();
                }
            }
            else
            {
                string[] prev = strPermission.Split(char.Parse(","));
                int ModuleID = POS_Core.Resources.Configuration.convertNullToInt(prev[0]);
                int ScreenID = POS_Core.Resources.Configuration.convertNullToInt(prev[1]);
                int PermissionID = POS_Core.Resources.Configuration.convertNullToInt(prev[2]);
                if (UserPriviliges.IsUserHasPriviliges(ModuleID, ScreenID, PermissionID, strUserName) == false)
                {
                    this.txtUserName.Focus();
                    ErrorHandler.throwCustomError(POSErrorENUM.Users_UserNotHavePriviligesForThisAction);
                }
                else
                {
                    //Added By shitaljit on 2/11/2014 for JIRA PRIMEPOS-1810
                    if (ModuleID == UserPriviliges.Modules.POSTransaction.ID && ScreenID == UserPriviliges.Screens.POSTransaction.ID
                        && PermissionID == UserPriviliges.Permissions.DiscOverridefromPOSTrans.ID)
                    {
                        POS_Core.Resources.Configuration.IsDiscOverridefromPOSTrans = true;
                    }
                    //END
                    allowClose = true;
                    strPrevlige = string.Empty;
                    strPermission = string.Empty;
                    m_Canceled = false;
                    this.Close();
                }
            }
            POS_Core.ErrorLogging.Logs.Logger(clsPOSDBConstants.Log_Module_Login, " CheckPrivilages() ", clsPOSDBConstants.Log_Exiting);
        }

        private bool isUserAlreadyLoggedIn(string sIPAddress)
        {
            bool retVal = false;

            /*try
            {
                System.Net.Dns.GetHostByName("tcp://localhost:9932/RemoteServer");
            }
            catch (System.Net.Sockets.SocketException exp)
            {
                exp = null;
                return retVal;
            }*/
            if (clsUIHelper.validateIP(sIPAddress))
            {
                System.Net.IPAddress oIPAddr = System.Net.IPAddress.Parse(sIPAddress);
                string localHostIP = clsCoreUIHelper.GetLocalHostIP();
                if (sIPAddress != localHostIP)
                {
                    AppDomain oDomain = AppDomain.CreateDomain("RemoteClient");
                    RemoteClient.Client oClient = (RemoteClient.Client)oDomain.CreateInstanceFromAndUnwrap("RemoteClient.dll", "RemoteClient.Client");
                    retVal = oClient.CheckLogin("tcp://" + sIPAddress + ":9932/RemoteServer", this.txtUserName.Text);
                    AppDomain.Unload(oDomain);

                    oDomain = null;
                    oClient = null;
                }
            }

            return retVal;
        }

        private void btnCancel_Click(object sender, System.EventArgs e)
        {
            #region PRIMEPOS-2958 22-Apr-2021 JY Added
            if (OpenType == LoginENUM.Lock)
            {
                if (Resources.Message.Display("Are you sure you want to exit?", Configuration.ApplicationName, MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
                {
                    return;
                }
                else
                {
                    this.allowClose = true;
                    m_Canceled = true;
                    this.Close();
                    Application.Exit();
                }
            }
            #endregion
            else
            {
                POS_Core.ErrorLogging.Logs.Logger(clsPOSDBConstants.Log_Module_Login, " Login Cancel ", "User Cancel Login");
                this.allowClose = true;
                m_Canceled = true;
                //ErrorHandler.SaveLog((int)LogENUM.Login, POS_Core.Resources.Configuration.UserName, "Fail", "Login Failed");
                this.Close();
            }
        }

        private void frmLogin_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (this.allowClose == true)
            {
                this.Hide();
            }
            else
                e.Cancel = true;
        }

        private void grpBoxLogin_Enter(object sender, System.EventArgs e)
        {
            this.txtUserName.Focus();
        }

        private void btnCancel_Leave(object sender, System.EventArgs e)
        {
            this.txtUserName.Focus();
        }

        private void frmLogin_Load(object sender, System.EventArgs e)
        {
            POS_Core.ErrorLogging.Logs.Logger(clsPOSDBConstants.Log_Module_Login, " Login Form Load", clsPOSDBConstants.Log_Entering);

            #region Sprint-22 - PRIMEPOS-2247 23-Nov-2015 JY Added logic for PrimePOS logo
            try
            {
                string sAppPath = Configuration.GetAppPath(this);
                if (sAppPath != string.Empty)
                {
                    string sFilePath = sAppPath + @"\PrimePOSLogo.png";
                    if (System.IO.File.Exists(sFilePath))
                    {
                        lblPrimePOS.Visible = false;
                        pctPrimePOS.Visible = true;
                        pctPrimePOS.ImageLocation = sFilePath;
                        pctPrimePOS.SizeMode = PictureBoxSizeMode.Zoom;
                    }
                    else
                    {
                        pctPrimePOS.Visible = false;
                        lblPrimePOS.Visible = true;
                        POS_Core.ErrorLogging.Logs.Logger(clsPOSDBConstants.Log_Module_Login, " PrimePOS Logo file (PrimePOSLogo.png) not found ", clsPOSDBConstants.Log_Exiting);
                    }
                }
                else
                {
                    pctPrimePOS.Visible = false;
                    lblPrimePOS.Visible = true;
                    POS_Core.ErrorLogging.Logs.Logger(clsPOSDBConstants.Log_Module_Login, " returned wrong application path", clsPOSDBConstants.Log_Exiting);
                }
            }
            catch
            {
                pctPrimePOS.Visible = false;
                lblPrimePOS.Visible = true;
                POS_Core.ErrorLogging.Logs.Logger(clsPOSDBConstants.Log_Module_Login, " ERror loading PrimePOS logo file", clsPOSDBConstants.Log_Exiting);
            }
            #endregion

            #region PRIMEPOS-2576 29-Aug-2018 JY Added
            if (POS_Core.Resources.Configuration.convertNullToString(POS_Core.Resources.Configuration.CInfo.UseBiometricDevice) != "" && POS_Core.Resources.Configuration.CInfo.UseBiometricDevice.Equals("DigitalPersona", StringComparison.OrdinalIgnoreCase))
            {
                fpReader = new FingerPrintReader(FrmLoginSendMessage);
                IsFPReaderConnected = fpReader.FingerPrintReaderConnect();
                if (IsFPReaderConnected)
                {
                    lblTouchID.Visible = true;
                    ToolTip ToolTip1 = new System.Windows.Forms.ToolTip();
                    ToolTip1.SetToolTip(lblTouchID, "Fingerprint Scanning Enabled");
                }
            }
            #endregion

            this.Activate(); //Added by Manoj 9/29/2011 This is use to give this form active focus.
            this.TopMost = true;//Added by shitaljit to bring login screen in fronth when POS gets lock after predefined time interval.
            this.txtUserName.AfterEnterEditMode += new System.EventHandler(clsCoreUIHelper.AfterEnterEditMode);
            this.txtUserName.AfterExitEditMode += new System.EventHandler(clsCoreUIHelper.AfterExitEditMode);

            this.txtPassward.AfterEnterEditMode += new System.EventHandler(clsCoreUIHelper.AfterEnterEditMode);
            this.txtPassward.AfterExitEditMode += new System.EventHandler(clsCoreUIHelper.AfterExitEditMode);
            lblPharmacy.Text = "Licensed To : " + POS_Core.Resources.Configuration.CInfo.StoreName;
            this.lblPOSV.Text = Application.ProductName.Trim() + " V" + Application.ProductVersion.TrimEnd();
            clsCoreUIHelper.setColorSchecme(this);            
            if (OpenType == LoginENUM.Login)
            {
                allowClose = true;
            }
            else
            {
                allowClose = false;
                this.txtUserName.SelectAll();
            }
            this.lblPrimePOS.Appearance.BackColor = Color.White;
            this.lblPrimePOS.Appearance.ForeColor = Color.FromArgb(34, 116, 155);
            POS_Core.Resources.Configuration.ConnectionStringType = "MasterDatabase";
            //bool isDefaultLogin = Convert.ToBoolean(System.Configuration.ConfigurationSettings.AppSettings["IsDafaultLogin"]);
            // if (isDefaultLogin == true)
            //{
            //    txtUserName.Text = System.Configuration.ConfigurationSettings.AppSettings["UserName"];
            //    txtPassward.Text = System.Configuration.ConfigurationSettings.AppSettings["Password"];
            //    btnOk_Click(btnOk, new System.EventArgs());
            //}

            if (IsPOSLite == true || bScheduledTaskExecute == true) //PRIMEPOS-2485 29-Mar-2021 JY Added bScheduledTaskExecute condition
            {
                txtUserName.Text = username;
                txtPassward.Text = password;
                btnOk_Click(btnOk, new System.EventArgs());
            }

            #region  PRIMEPOS-2616 19-Dec-2018 JY Added

            if (POS_Core.Resources.Configuration.CInfo.AuthenticationMode == 0 || POS_Core.Resources.Configuration.CInfo.AuthenticationMode == 1 || POS_Core.Resources.Configuration.CInfo.AuthenticationMode == 3)
            {
                if (POS_Core.Resources.Configuration.CInfo.AuthenticationMode == 1) //POS User
                    cboLoginWith.SelectedIndex = 0;
                if (POS_Core.Resources.Configuration.CInfo.AuthenticationMode == 0) //Windows User
                    cboLoginWith.SelectedIndex = 1;
                if (POS_Core.Resources.Configuration.CInfo.AuthenticationMode == 3) //Azure User
                    cboLoginWith.SelectedIndex = 2;

                cboLoginWith.Enabled = false;
            }
            else
            {
                cboLoginWith.SelectedIndex = 0;
            }
            //PopulateDomainNames();
            txtDomain.Text = Environment.UserDomainName;
            #endregion                       
            this.txtUserName.Focus();
            POS_Core.ErrorLogging.Logs.Logger(clsPOSDBConstants.Log_Module_Login, " Login Form Load", clsPOSDBConstants.Log_Exiting);
        }

        private void frmLogin_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            try
            {
                if (e.KeyData == System.Windows.Forms.Keys.Enter)
                {
                    if (this.txtPassward.ContainsFocus == true)
                    {
                        btnOk_Click(btnOk, new System.EventArgs());
                    }
                    else
                    {
                        this.SelectNextControl(this.ActiveControl, true, true, true, true);
                    }
                }
            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.Message);
            }
        }

        private void grpBoxLogin_Click(object sender, System.EventArgs e)
        {
        }

        public bool Canceled
        {
            get
            {
                return this.m_Canceled;
            }
        }

        private void txtUserName_Validating(object sender, CancelEventArgs e)
        {
            #region Commented Code for OLD Barcode Login

            //try
            //{
            //    string sIPAddress;
            //    string strUserID="";
            //    string sID = "";
            //    string sPassword = "";
            //    if (txtUserName.Text.StartsWith("99") && txtUserName.Text.EndsWith("99") && txtUserName.Text.Length>4)
            //    {
            //        //Added By Shitaljit(QuicSolv) on july 1 2011
            //        //To Stop Multiple Instance of POS running With Same Station ID
            //        clsMain.CheckPOSInstance();
            //        //END of added by Shitaljit.
            //        string strDecrptText = txtUserName.Text.Substring(2, txtUserName.Text.Length - 4);
            //        if (strDecrptText.Length == 11)
            //        {
            //            strDecrptText = strDecrptText + "=";
            //        }
            //        sID= EncryptString.CustomDecrypt(strDecrptText);
            //        //"@@QRUCb2H/TGRBt0CbJQaxig==@@\r\n"

            //        txtUserName.Text = "";
            //        oclsLogin.ValidateUserByID(sID,out strUserID, out sIPAddress,out sPassword);

            //        CheckPrivilages(strUserID);
            //    }
            //}

            //catch (POSExceptions exp)
            //{
            //    POS_Core_UI.Resources.Message.Display(exp.Message, "User Authentication", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //    switch (exp.ErrNumber)
            //    {
            //        case (long)POSErrorENUM.User_InValidPassward:
            //            txtPassward.Focus();
            //            break;
            //        case (long)POSErrorENUM.User_InvalidUserName:
            //            txtUserName.Focus();
            //            break;
            //        case (long)POSErrorENUM.User_InvalidSecurityLevel:
            //            txtUserName.Text = Resources.Configuration.UserName;
            //            txtPassward.Text = "";
            //            txtUserName.Focus();
            //            break;

            //    }
            //}
            //catch (Exception exp)
            //{
            //    this.txtUserName.Focus();
            //    POS_Core_UI.Resources.Message.Display(exp.Message, "User Authentication", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //}

            #endregion Commented Code for OLD Barcode Login
        }

        private void txtUserName_TextChanged(object sender, EventArgs e)
        {
            //if (txtUserName.Text.StartsWith("99") && txtUserName.Text.EndsWith("99") && txtUserName.Text.Length>2)
            //{
            //    txtPassward.Focus();
            //}
        }

        private void txtUserName_Leave(object sender, EventArgs e)
        {
            try
            {
                #region Sprint-26 - PRIMEPOS-555 27-Jun-2017 JY Added
                if (btnCancel.Focused)
                {
                    btnCancel_Click(sender, e);
                    return;
                }
                #endregion

                //Following if is added By Shitaljit(QuicSolv) on 11 July 2011
                //To Fixed Login error while login in with barcode generated by user ID.
                this.txtUserName.Text = this.txtUserName.Text.Trim();
                if (txtUserName.Text.Contains(clsPOSDBConstants.UserBarcodeSeperatorString) && txtUserName.Text.Length > 4)
                {
                    this.btnOk_Click(this, new EventArgs());
                }
                else
                {
                    //if (cboLoginWith.SelectedIndex != 0)    //PRIMEPOS-2616 14-Dec-2018 JY Added if condition
                    if (Configuration.convertNullToInt(cboLoginWith.SelectedItem.DataValue) == 1)   //PRIMEPOS-2989 12-Aug-2021 JY Modified
                    {
                        if (this.txtUserName.Text != string.Empty)
                        {
                            oclsLogin.ValidateUserName(this.txtUserName.Text);
                            lblLoginAttempt.Text = "Login Attempt(" + oclsLogin.MaxAttempt.ToString() + "/6)";
                        }
                    }
                }
            }
            catch (POSExceptions exp)
            {
                clsCoreUIHelper.ShowBtnIconMsg(exp.Message, "User Authentication", MessageBoxButtons.OK, MessageBoxIcon.Error);
                switch (exp.ErrNumber)
                {
                    case (long)POSErrorENUM.User_InvalidUserName:
                        txtUserName.Focus();
                        break;
                    case (long)POSErrorENUM.User_InvalidSecurityLevel:
                        ErrorHandler.SaveLog((int)LogENUM.Login, this.txtUserName.Text.ToString(), "Fail", "Invalid SecurityLevel");
                        txtUserName.Text = POS_Core.Resources.Configuration.UserName;
                        txtPassward.Text = "";
                        txtUserName.Focus();
                        break;
                    case (long)POSErrorENUM.User_Locked:
                        ErrorHandler.UpdateUserLoginAttempt(this.txtUserName.Text, oclsLogin.MaxAttempt, true);
                        break;
                    case (long)POSErrorENUM.User_ResetPassword:
                        frmChangePassword objChangPwd = new frmChangePassword();
                        objChangPwd.ShowDialog();
                        break;
                }
                lblLoginAttempt.Text = "Login Attempt(" + oclsLogin.MaxAttempt.ToString() + "/6)";
            }
            catch (Exception exp)
            {
                clsCoreUIHelper.ShowBtnIconMsg(exp.Message, "User Authentication", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.txtUserName.Focus();
            }
        }

        //Added By Shitaljit(QuicSolv) on 15 Feb 2012
        //To select text of the txtUserName JIRA-239
        private void txtUserName_Enter(object sender, EventArgs e)
        {
            try
            {
                this.txtUserName.SelectAll();
            }
            catch (Exception Ex)
            {
                clsCoreUIHelper.ShowErrorMsg(Ex.Message);
            }

            //End of added in 15 Feb 2012
        }

        //Sprint-24 - PRIMEPOS-2290 19-Jan-2017 JY Added
        private void AddAllowHouseChargePaytypeAccessRight(string strUserName)
        {
            oclsLogin.AddAllowHouseChargePaytypeAccessRight(strUserName);
        }

        //Sprint-25 - PRIMEPOS-2379 15-Mar-2017 JY Added
        private void AddPSEItemListAccessRight(string strUserName)
        {
            oclsLogin.AddPSEItemListAccessRight(strUserName);
        }

        //Sprint-26 - PRIMEPOS-2416 25-Jul-2017 JY Added
        private void AddQuantityOverrideAccessRight(string strUserName)
        {
            oclsLogin.AddQuantityOverrideAccessRight(strUserName);
        }

        //Sprint-26 - PRIMEPOS-2383 08-Aug-2017 JY Added
        private void AddStandAloneReturnAccessRight(string strUserName)
        {
            oclsLogin.AddStandAloneReturnAccessRight(strUserName);
        }

        //PRIMEPOS-2539 06-Jul-2018 JY Added
        private void AddAllowCheckPaymentAccessRight(string strUserName)
        {
            oclsLogin.AddAllowCheckPaymentAccessRight(strUserName);
        }

        //PRIMEPOS-2464 26-Mar-2020 JY Added
        private void AddDisplayItemCostAccessRight(string strUserName)
        {
            oclsLogin.AddDisplayItemCostAccessRight(strUserName);
        }

        #region PRIMEPOS-2576 29-Aug-2018 JY Added
        private delegate void SendMessageCallback(fpReaderAction action, object payload);
        public void FrmLoginSendMessage(fpReaderAction action, object payload)
        {
            if (this.InvokeRequired)
            {
                SendMessageCallback d = new SendMessageCallback(FrmLoginSendMessage);
                this.Invoke(d, new object[] { action, payload });
            }
            else
            {
                switch (action)
                {
                    case fpReaderAction.SendMessage:
                        MessageBox.Show((string)payload);
                        break;
                    case fpReaderAction.SendFingerprint:
                        this.Cursor = Cursors.WaitCursor;

                        string UserID = string.Empty;
                        fpReader.MatchFingerPrintToUser(payload, out UserID);

                        if (!string.IsNullOrWhiteSpace(UserID))
                        {
                            FrmLogin_ProcessFingerprintAuth(UserID, fpReader.captureCount);

                        }
                        else if (fpReader.captureCount >= 4)
                        {
                            MessageBox.Show("The fingerprint is not identified. Please enroll it first.");
                        }

                        this.Cursor = Cursors.Default;
                        break;
                    default:
                        break;
                }
            }
        }

        private void FrmLogin_ProcessFingerprintAuth(string UserID, int captureCount)
        {
            DBUser oDBUser = new DBUser();
            DataTable dtUser = oDBUser.GetUserDetails(UserID);   //get user information
            if (dtUser != null && dtUser.Rows.Count > 0)
            {
                txtUserName.Text = dtUser.Rows[0]["UserID"].ToString();
                txtPassward.Text = EncryptString.Decrypt(dtUser.Rows[0]["Password"].ToString());

                this.btnOk_Click(this, new EventArgs());
            }
        }

        private bool FrmLogin_ValidateUser(DataTable dtUser)
        {
            bool rtnCode = true;

            if (Configuration.convertNullToBoolean(dtUser.Rows[0]["IsActive"]) == false)
            {
                rtnCode = false;
                MessageBox.Show(this, dtUser.Rows[0]["UserID"].ToString() + " is not Active", "Inactive user", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Stop);
            }

            if (Configuration.convertNullToBoolean(dtUser.Rows[0]["ISLOCKED"]) == false) //check locked
            {
                rtnCode = false;
                MessageBox.Show(string.Format("Account locked! your account {0} has reached its maximum failed attempts limit, please contact system administrator.", dtUser.Rows[0]["UserID"].ToString()), "Account Locked.", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            if (rtnCode == true)
            {
                txtUserName.Text = dtUser.Rows[0]["UserID"].ToString();
                txtPassward.Text = dtUser.Rows[0]["Password"].ToString();
            }
            return rtnCode;
        }

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
                        MessageBox.Show((string)payload);
                        break;
                    case fpReaderAction.SendFingerprint:
                        this.Cursor = Cursors.WaitCursor;
                        string UserID = string.Empty;
                        fpReader.MatchFingerPrintToUser(payload, out UserID);
                        if (!string.IsNullOrWhiteSpace(UserID))
                        {
                            ProcessFingerPrintCapture(UserID);
                        }
                        else if (fpReader.captureCount >= 4)
                        {
                            MessageBox.Show("The fingerprint is not identified. Please enroll it first.");
                        }
                        this.Cursor = Cursors.Default;
                        break;
                    default:
                        break;
                }
            }

        }

        private void ProcessFingerPrintCapture(string UserID)
        {
            DBUser oDBUser = new DBUser();
            DataTable dtUser = oDBUser.GetUserDetails(UserID);
            if (dtUser != null && dtUser.Rows.Count > 0)
            {
                txtUserName.Text = dtUser.Rows[0]["UserID"].ToString();
                txtPassward.Text = EncryptString.Decrypt(dtUser.Rows[0]["Password"].ToString());

                this.btnOk_Click(this, new EventArgs());
            }
        }

        #endregion

        #region PRIMEPOS-2616 13-Dec-2018 JY Added
        private void cboLoginWith_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                this.SizeChanged -= oclsLogin.ofrmLogin_SizeChanged;    //PRIMEPOS-2678 26-Apr-2019 JY Added
                //if (cboLoginWith.SelectedIndex == 0)
                if (Configuration.convertNullToInt(cboLoginWith.SelectedItem.DataValue) == 0)   //PRIMEPOS-2989 12-Aug-2021 JY Modified
                {
                    //Windows User
                    lblDomain.Visible = txtDomain.Visible = true;
                    ultraLabel1.Visible = txtUserName.Visible = ultraLabel2.Visible = txtPassward.Visible = true;
                    webBrowser1.Visible = false;
                    btnOk.Visible = true;
                    ultraLabel2.Top = lblDomain.Top + lblDomain.Height + 5;
                    txtPassward.Top = txtDomain.Top + txtDomain.Height + 5;
                    btnOk.Top = btnCancel.Top = txtPassward.Top + txtPassward.Height + 10;
                    btnCancel.Left = btnOk.Left + btnOk.Width + 10;
                    lblLoginAttempt.Top = btnOk.Top + btnOk.Height + 5;
                    grpLogin.Height = 215;
                    groupBox1.Left = groupBox2.Left = 10;
                    groupBox1.Top = 305;
                    this.Width = 400;
                    this.Height = 445;                    
                    this.txtUserName.Text = Environment.UserName;
                    this.txtUserName.Focus();
                }
                else if (Configuration.convertNullToInt(cboLoginWith.SelectedItem.DataValue) == 1)
                {
                    //POS User
                    lblDomain.Visible = txtDomain.Visible = false;
                    ultraLabel1.Visible = txtUserName.Visible = ultraLabel2.Visible = txtPassward.Visible = true;
                    webBrowser1.Visible = false;
                    btnOk.Visible = true;
                    ultraLabel2.Top = lblDomain.Top;
                    txtPassward.Top = txtDomain.Top;
                    btnOk.Top = btnCancel.Top = lblTouchID.Top + lblTouchID.Height - btnOk.Height;
                    btnCancel.Left = btnOk.Left + btnOk.Width + 10;
                    lblLoginAttempt.Top = btnOk.Top + btnOk.Height + 5;
                    grpLogin.Height = 190;
                    groupBox1.Left = groupBox2.Left = 10;
                    groupBox1.Top = 280;
                    this.Width = 400;
                    this.Height = 420;
                    this.txtUserName.Text = "";
                    this.txtUserName.Focus();
                }
                else if (Configuration.convertNullToInt(cboLoginWith.SelectedItem.DataValue) == 3)
                {
                    //Azure User
                    webBrowser1.ObjectForScripting = this;  //PRIMEPOS-2989 13-Aug-2021 JY Added
                    webBrowser1.Navigate(Configuration.CSetting.AzureADMiddleTierUrl + "Login.aspx");   //PRIMEPOS-2989 13-Aug-2021 JY Added
                    lblDomain.Visible = txtDomain.Visible = false;
                    ultraLabel1.Visible = txtUserName.Visible = ultraLabel2.Visible = txtPassward.Visible = false;
                    webBrowser1.Visible = true;
                    btnOk.Visible = false;
                    webBrowser1.Top = txtUserName.Top;
                    webBrowser1.Left = ultraLabel1.Left;
                    webBrowser1.Height = 360;
                    btnCancel.Top = webBrowser1.Top + webBrowser1.Height + 5;
                    btnCancel.Left = webBrowser1.Left + webBrowser1.Width - btnCancel.Width;
                    this.grpLogin.Width = 480;
                    this.grpLogin.Height = 455;
                    groupBox1.Left = groupBox2.Left = 70;
                    groupBox1.Top = 540;
                    this.Width = 514;
                    this.Height = 680;
                }                
            }
            catch { }
            finally
            {
                this.SizeChanged += oclsLogin.ofrmLogin_SizeChanged;    //PRIMEPOS-2678 26-Apr-2019 JY Added
            }
        }

        private void LoginWithWindowsAuthentication(string strUserName, string strPassword, out string strErrMsg)
        {
            strErrMsg = string.Empty;
            string strLoggedInWindowsUser = Environment.UserName;
            //if (!strLoggedInWindowsUser.ToLowerInvariant().Contains(txtUserName.Text.Trim().ToLowerInvariant()))
            //{
            //    strErrMsg = "input username and Windows logged in username should be same";
            //    return;
            //}

            bool bException;
            bool bStatus = ValidateADUser(strUserName, strPassword, out bException);
            if (bException == true)
            {
                //there is issue with Ad authentication, so try the local workgroup authentication
                bStatus = IsValidateCredentials(strUserName, strPassword);
            }

            if (bStatus == false)
            {
                strErrMsg = "Something went wrong with windows authentication";
                return;
            }
            else
            {
                DBUser oDBUser = new DBUser();
                DataTable dtUser = oDBUser.GetUserByWindowsLoginId(strUserName);   //get user information by windows loginid
                if (dtUser != null && dtUser.Rows.Count > 0)
                {
                    txtUserName.Text = dtUser.Rows[0]["UserID"].ToString();
                    txtPassward.Text = EncryptString.Decrypt(dtUser.Rows[0]["Password"].ToString());
                }
            }
        }

        private bool ValidateADUser(string strUserName, string strPassword, out bool bException)
        {
            bool bIsValid = false;
            bException = false;
            try
            {
                LdapConnection lcon = new LdapConnection(new LdapDirectoryIdentifier((string)null, false, false));
                NetworkCredential nc = new NetworkCredential(strUserName, strPassword, this.txtDomain.Text);
                lcon.Credential = nc;
                lcon.AuthType = AuthType.Negotiate;
                lcon.Bind(nc); // user has authenticated at this point, as the credentials were used to login to the dc.
                bIsValid = true;
            }
            catch (LdapException ex)
            {
                bException = true;
            }
            return bIsValid;
        }

        private bool IsValidateCredentials(string strUserName, string strPassword)
        {
            IntPtr tokenHandler = IntPtr.Zero;
            bool bIsValid = LogonUser(strUserName, this.txtDomain.Text.Trim(), strPassword, 2, 0, ref tokenHandler);
            return bIsValid;
        }

        //private void PopulateDomainNames()
        //{
        //    cboDomain.Items.Clear();
        //    try
        //    {
        //        Forest currentForest = Forest.GetCurrentForest();
        //        DomainCollection domains = currentForest.Domains;
        //        foreach (Domain objDomain in domains)
        //        {
        //            cboDomain.Items.Add(objDomain.Name);
        //        }
        //    }
        //    catch
        //    {
        //        cboDomain.Items.Add(Environment.UserDomainName);
        //    }
        //    if (cboDomain.Items.Count > 0)
        //    {
        //        cboDomain.SelectedIndex = 0;
        //    }
        //}
        #endregion

        private void LoginWithBarCode(out string sID, out string strUserID, out string sIPAddress, out string sPassword)
        {
            sID = strUserID = sIPAddress = sPassword = "";
            string[] splitdata = Regex.Split(txtUserName.Text, clsPOSDBConstants.UserBarcodeSeperatorString);
            if (splitdata.Length == 2)
            {
                string strDecryptPassword = splitdata[1].ToString();
                sID = splitdata[0].ToString();
                if (string.IsNullOrWhiteSpace(sID))
                {
                    throw new Exception("Please enter valid User Name.");
                }
                oclsLogin.ValidateUserByID(sID, out strUserID, out sIPAddress, out sPassword);

                //Added By shitaljit to fixed barcode ignoring "==" and "=" issues.
                if (sPassword.EndsWith("==") == true && strDecryptPassword.EndsWith("==") == false)
                {
                    strDecryptPassword += "==";
                }
                else if (sPassword.EndsWith("=") == true && strDecryptPassword.EndsWith("=") == false)
                {
                    strDecryptPassword += "=";
                }
                //End 
                if (sPassword.ToUpper().Equals(strDecryptPassword))
                {
                    sPassword = EncryptString.Decrypt(sPassword);
                    this.txtPassward.Text = sPassword;
                    this.txtUserName.Text = strUserID;
                    oclsLogin.ValidateUserNamePassward(this.txtUserName.Text, this.txtPassward.Text, out sIPAddress);   //PRIMEPOS-2437 20-Apr-2021 JY Added
                    CheckPrivilages(strUserID);
                    POS_Core.ErrorLogging.Logs.Logger(clsPOSDBConstants.Log_Module_Login, " btnOk_Click() Validating Barcode Login", "Completed");
                }
                else
                {
                    ErrorHandler.throwCustomError(POSErrorENUM.User_InValidPassward);
                }
            }
        }

        #region PRIMEPOS-2676 20-May-2019 JY Added
        private void AddTransSettingsAccessRight(string strUserName)
        {
            oclsLogin.AddTransSettingsAccessRight(strUserName);
        }

        private void AddRxSettingsAccessRight(string strUserName)
        {
            oclsLogin.AddRxSettingsAccessRight(strUserName);
        }

        private void AddPrimePOSettingsAccessRight(string strUserName)
        {
            oclsLogin.AddPrimePOSettingsAccessRight(strUserName);
        }

        private void AddCLPSettingsAccessRight(string strUserName)
        {
            oclsLogin.AddCLPSettingsAccessRight(strUserName);
        }

        //PRIMEPOS-3141 27-Oct-2022 JY Added
        private void AddInventoryReceivedAccessRight(string strUserName)
        {
            oclsLogin.AddInventoryReceivedAccessRight(strUserName);
        }        
        #endregion

        #region PRIMEPOS-2989 13-Aug-2021 JY Added
        private void timer1_Tick(object sender, EventArgs e)
        {
            string authInfo = string.Empty;
            if (cboLoginWith.SelectedIndex == 2)
            {
                if (webBrowser1.Document != null && webBrowser1.Document.Url.ToString().Equals(Configuration.CSetting.AzureADMiddleTierUrl + "Consumer", StringComparison.OrdinalIgnoreCase))
                {
                    if (webBrowser1.Document.GetElementById("iInfo") != null)
                    {
                        DBUser oDBUser = new DBUser();
                        authInfo = webBrowser1.Document.GetElementById("iInfo").GetAttribute("value");
                        DataTable dtUser = oDBUser.GetUserIDFromSAMLResponse(authInfo);   //get user information by Azure email
                        if (dtUser != null && dtUser.Rows.Count > 0)
                        {
                            if (Configuration.convertNullToBoolean(dtUser.Rows[0]["IsActive"]) == false)
                            {
                                this.timer1.Tick -= new System.EventHandler(this.timer1_Tick);
                                //ShowPharmacistPanel();
                                webBrowser1.ObjectForScripting = this;
                                webBrowser1.Navigate(Configuration.CSetting.AzureADMiddleTierUrl + "Login.aspx");
                                MessageBox.Show(this, dtUser.Rows[0]["UserID"] + " is Not Active", "Inactive user", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Stop);
                                DeactivateActivateTimer();
                                return;
                            }
                            txtUserName.Text = dtUser.Rows[0]["UserID"].ToString();
                            txtPassward.Text = EncryptString.Decrypt(dtUser.Rows[0]["Password"].ToString());
                            btnOk_Click(btnOk, new System.EventArgs());
                        }
                        else
                        {
                            this.timer1.Tick -= new System.EventHandler(this.timer1_Tick);
                            //ShowPharmacistPanel();
                            webBrowser1.ObjectForScripting = this;
                            webBrowser1.Navigate(Configuration.CSetting.AzureADMiddleTierUrl + "Login.aspx");
                            MessageBox.Show(this, "Invalid User", "Invalid User", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Hand);
                            //ContPHLoginLog cPHLoginLog = new ContPHLoginLog();
                            //cPHLoginLog.RecordLoginAttempt(phUserId, AppGlobal.GetStationId(), Environment.MachineName, "I");
                            DeactivateActivateTimer();
                            return;
                        }
                        DisposeTimer();
                        //this.Close();
                    }
                }
            }
        }

        private void DeactivateActivateTimer()
        {
            this.timer1.Tick -= new System.EventHandler(this.timer1_Tick);
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
        }

        private void DisposeTimer()
        {
            if (timer1 != null)
            {
                timer1.Stop();
                timer1.Tick -= new EventHandler(this.timer1_Tick);
                timer1.Dispose();
                timer1 = null;
            }
        }
        #endregion
    }
}