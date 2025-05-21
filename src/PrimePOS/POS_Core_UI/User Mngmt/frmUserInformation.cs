using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;

using POS_Core.ErrorLogging;
using System.Data.SqlClient;
using System.Data.OleDb;
using POS_Core.CommonData;
//using POS_Core.DataAccess;
using System.IO;
using System.Drawing.Imaging;
using System.Resources;
using System.Reflection;
using Resources;
using POS_Core.UserManagement;
using POS_Core.Resources;
using System.Text.RegularExpressions;
using MMSInterfaceLib.Utils;

namespace POS_Core_UI.UserManagement
{
    /// <summary>
    /// Summary description for UserInformation.
    /// </summary>
    public class frmUserInformation : System.Windows.Forms.Form
    {
        byte[] picArray = null;
        private string mUserId = "";
        public bool IsCanceled;
        private SaveModeENUM mSaveMode;
        private Infragistics.Win.UltraWinDataSource.UltraDataSource ultraDataSource1;
        private System.Windows.Forms.GroupBox ultraGroupBox2;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox ultraGroupBox3;

        private Infragistics.Win.Misc.UltraLabel lblTransactionType;
        private System.Windows.Forms.TextBox txtUserID;
        private System.Windows.Forms.CheckBox chkIsActive;
        private System.Windows.Forms.TextBox txtLoginReg;
        private System.Windows.Forms.TextBox txtPassword;
        private Infragistics.Win.Misc.UltraButton btnCancel;
        private Infragistics.Win.Misc.UltraButton btnOk;
        private Infragistics.Win.UltraWinEditors.UltraNumericEditor numSecurityLevel;
        private Infragistics.Win.UltraWinEditors.UltraComboEditor numCachDrawerNo;
        private System.Windows.Forms.GroupBox gbRights;
        private Infragistics.Win.Misc.UltraButton btnAllowAll;
        private Infragistics.Win.Misc.UltraButton btnDisallowAll;
        private Infragistics.Win.UltraWinTree.UltraTree trvPermissions;
        private TextBox txtFirstName;
        private Label label1;
        private TextBox txtLastName;
        private Label label2;
        private IContainer components;
        private string password1 = string.Empty;
        private string password2 = string.Empty;
        private Infragistics.Win.Misc.UltraButton btnUserNotes;
        private Label label5;
        private Infragistics.Win.UltraWinEditors.UltraNumericEditor numMaxPerDisc;
        private Label label9;
        private Infragistics.Win.UltraWinEditors.UltraNumericEditor numMaxTransactionLimit;
        private Infragistics.Win.UltraWinEditors.UltraComboEditor cmbUserGroup;
        private Infragistics.Win.Misc.UltraButton btnBrowseImage;
        private PictureBox PictUserImg;
        private Label label10;
        private Infragistics.Win.UltraWinEditors.UltraNumericEditor numMaxReturnTransLimit;
        private Label lblMaxTenderedAmount;
        private Infragistics.Win.UltraWinEditors.UltraNumericEditor numMaxTenderedAmount;
        private string password3 = string.Empty;
        private Infragistics.Win.UltraWinEditors.UltraCheckEditor chkChangePasswordAtLogin;
        private Infragistics.Win.Misc.UltraButton btnEnrollFingerprint;
        private TextBox txtWindowsLogin;
        private Label lblWindowsLogin;
        private Label lblMaxCashbackLimit;
        private Infragistics.Win.UltraWinEditors.UltraNumericEditor numMaxCashbackLimit;
        private Infragistics.Win.UltraWinEditors.UltraNumericEditor numHourlyRate;
        private Label lblHourlyRate;
        private Label lblEmailID;
        private TextBox txtEmailID;
        private static int nUserGroup = -1;  //PRIMEPOS-2577 14-Aug-2018 JY Added
        Regex CheckvalidEmailid = new Regex(@"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z");  //PRIMEPOS-2989 11-Aug-2021 JY Added
        #region PRIMEPOS-3484
        private Label lblLanID;
        private TextBox txtLanID;
        #endregion

        public frmUserInformation()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();
            this.IsCanceled = false;

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
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinTree.Override _override1 = new Infragistics.Win.UltraWinTree.Override();
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance7 = new Infragistics.Win.Appearance();
            Infragistics.Win.ValueListItem valueListItem10 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem11 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem12 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem13 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem14 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem15 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.Appearance appearance8 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance9 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance10 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance11 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance12 = new Infragistics.Win.Appearance();
            this.gbRights = new System.Windows.Forms.GroupBox();
            this.lblHourlyRate = new System.Windows.Forms.Label();
            this.numHourlyRate = new Infragistics.Win.UltraWinEditors.UltraNumericEditor();
            this.lblMaxCashbackLimit = new System.Windows.Forms.Label();
            this.numMaxCashbackLimit = new Infragistics.Win.UltraWinEditors.UltraNumericEditor();
            this.txtWindowsLogin = new System.Windows.Forms.TextBox();
            this.lblWindowsLogin = new System.Windows.Forms.Label();
            this.lblMaxTenderedAmount = new System.Windows.Forms.Label();
            this.numMaxTenderedAmount = new Infragistics.Win.UltraWinEditors.UltraNumericEditor();
            this.label10 = new System.Windows.Forms.Label();
            this.numMaxReturnTransLimit = new Infragistics.Win.UltraWinEditors.UltraNumericEditor();
            this.trvPermissions = new Infragistics.Win.UltraWinTree.UltraTree();
            this.label5 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.numMaxPerDisc = new Infragistics.Win.UltraWinEditors.UltraNumericEditor();
            this.btnDisallowAll = new Infragistics.Win.Misc.UltraButton();
            this.numMaxTransactionLimit = new Infragistics.Win.UltraWinEditors.UltraNumericEditor();
            this.txtLoginReg = new System.Windows.Forms.TextBox();
            this.btnAllowAll = new Infragistics.Win.Misc.UltraButton();
            this.label7 = new System.Windows.Forms.Label();
            this.ultraDataSource1 = new Infragistics.Win.UltraWinDataSource.UltraDataSource(this.components);
            this.ultraGroupBox2 = new System.Windows.Forms.GroupBox();
            this.lblEmailID = new System.Windows.Forms.Label();
            this.txtEmailID = new System.Windows.Forms.TextBox();
            this.chkChangePasswordAtLogin = new Infragistics.Win.UltraWinEditors.UltraCheckEditor();
            this.btnBrowseImage = new Infragistics.Win.Misc.UltraButton();
            this.PictUserImg = new System.Windows.Forms.PictureBox();
            this.cmbUserGroup = new Infragistics.Win.UltraWinEditors.UltraComboEditor();
            this.txtLastName = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtFirstName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.chkIsActive = new System.Windows.Forms.CheckBox();
            this.txtUserID = new System.Windows.Forms.TextBox();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.numCachDrawerNo = new Infragistics.Win.UltraWinEditors.UltraComboEditor();
            this.label4 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.numSecurityLevel = new Infragistics.Win.UltraWinEditors.UltraNumericEditor();
            this.ultraGroupBox3 = new System.Windows.Forms.GroupBox();
            this.btnEnrollFingerprint = new Infragistics.Win.Misc.UltraButton();
            this.btnUserNotes = new Infragistics.Win.Misc.UltraButton();
            this.btnCancel = new Infragistics.Win.Misc.UltraButton();
            this.btnOk = new Infragistics.Win.Misc.UltraButton();
            this.lblTransactionType = new Infragistics.Win.Misc.UltraLabel();
            #region PRIMEPOS-3484
            this.lblLanID = new System.Windows.Forms.Label();
            this.txtLanID = new System.Windows.Forms.TextBox();
            #endregion
            this.gbRights.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numHourlyRate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numMaxCashbackLimit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numMaxTenderedAmount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numMaxReturnTransLimit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trvPermissions)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numMaxPerDisc)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numMaxTransactionLimit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraDataSource1)).BeginInit();
            this.ultraGroupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chkChangePasswordAtLogin)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PictUserImg)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbUserGroup)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numCachDrawerNo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numSecurityLevel)).BeginInit();
            this.ultraGroupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbRights
            // 
            this.gbRights.Controls.Add(this.lblHourlyRate);
            this.gbRights.Controls.Add(this.numHourlyRate);
            this.gbRights.Controls.Add(this.lblMaxCashbackLimit);
            this.gbRights.Controls.Add(this.numMaxCashbackLimit);
            this.gbRights.Controls.Add(this.txtWindowsLogin);
            this.gbRights.Controls.Add(this.lblWindowsLogin);
            this.gbRights.Controls.Add(this.lblMaxTenderedAmount);
            this.gbRights.Controls.Add(this.numMaxTenderedAmount);
            this.gbRights.Controls.Add(this.label10);
            this.gbRights.Controls.Add(this.numMaxReturnTransLimit);
            this.gbRights.Controls.Add(this.trvPermissions);
            this.gbRights.Controls.Add(this.label5);
            this.gbRights.Controls.Add(this.label9);
            this.gbRights.Controls.Add(this.numMaxPerDisc);
            this.gbRights.Controls.Add(this.btnDisallowAll);
            this.gbRights.Controls.Add(this.numMaxTransactionLimit);
            this.gbRights.Controls.Add(this.txtLoginReg);
            this.gbRights.Controls.Add(this.btnAllowAll);
            this.gbRights.Controls.Add(this.label7);
            this.gbRights.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.gbRights.ForeColor = System.Drawing.Color.White;
            this.gbRights.Location = new System.Drawing.Point(12, 309);
            this.gbRights.Name = "gbRights";
            this.gbRights.Size = new System.Drawing.Size(638, 206);
            this.gbRights.TabIndex = 2;
            this.gbRights.TabStop = false;
            this.gbRights.Text = "Rights";
            // 
            // lblHourlyRate
            // 
            this.lblHourlyRate.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHourlyRate.ForeColor = System.Drawing.Color.White;
            this.lblHourlyRate.Location = new System.Drawing.Point(366, 190);
            this.lblHourlyRate.Name = "lblHourlyRate";
            this.lblHourlyRate.Size = new System.Drawing.Size(143, 20);
            this.lblHourlyRate.TabIndex = 16;
            this.lblHourlyRate.Text = "Hourly Rate";
            // 
            // numHourlyRate
            // 
            appearance1.FontData.BoldAsString = "False";
            appearance1.FontData.ItalicAsString = "False";
            appearance1.FontData.StrikeoutAsString = "False";
            appearance1.FontData.UnderlineAsString = "False";
            appearance1.ForeColor = System.Drawing.Color.Black;
            appearance1.ForeColorDisabled = System.Drawing.Color.Black;
            this.numHourlyRate.Appearance = appearance1;
            this.numHourlyRate.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.numHourlyRate.Location = new System.Drawing.Point(515, 190);
            this.numHourlyRate.MaskInput = "{LOC}-nnnn.nn";
            this.numHourlyRate.MaxValue = 1000D;
            this.numHourlyRate.MinValue = 0D;
            this.numHourlyRate.Name = "numHourlyRate";
            this.numHourlyRate.NumericType = Infragistics.Win.UltraWinEditors.NumericType.Double;
            this.numHourlyRate.Size = new System.Drawing.Size(117, 20);
            this.numHourlyRate.TabIndex = 10;
            // 
            // lblMaxCashbackLimit
            // 
            this.lblMaxCashbackLimit.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMaxCashbackLimit.ForeColor = System.Drawing.Color.White;
            this.lblMaxCashbackLimit.Location = new System.Drawing.Point(367, 165);
            this.lblMaxCashbackLimit.Name = "lblMaxCashbackLimit";
            this.lblMaxCashbackLimit.Size = new System.Drawing.Size(143, 20);
            this.lblMaxCashbackLimit.TabIndex = 15;
            this.lblMaxCashbackLimit.Text = "Max. Cashback Limit";
            // 
            // numMaxCashbackLimit
            // 
            appearance2.FontData.BoldAsString = "False";
            appearance2.FontData.ItalicAsString = "False";
            appearance2.FontData.StrikeoutAsString = "False";
            appearance2.FontData.UnderlineAsString = "False";
            appearance2.ForeColor = System.Drawing.Color.Black;
            appearance2.ForeColorDisabled = System.Drawing.Color.Black;
            this.numMaxCashbackLimit.Appearance = appearance2;
            this.numMaxCashbackLimit.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.numMaxCashbackLimit.Location = new System.Drawing.Point(515, 165);
            this.numMaxCashbackLimit.MaskInput = "{LOC}nn,nnn,nnn.nn";
            this.numMaxCashbackLimit.MaxValue = 999.99D;
            this.numMaxCashbackLimit.MinValue = 0D;
            this.numMaxCashbackLimit.Name = "numMaxCashbackLimit";
            this.numMaxCashbackLimit.NumericType = Infragistics.Win.UltraWinEditors.NumericType.Double;
            this.numMaxCashbackLimit.Size = new System.Drawing.Size(117, 20);
            this.numMaxCashbackLimit.TabIndex = 9;
            // 
            // txtWindowsLogin
            // 
            this.txtWindowsLogin.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtWindowsLogin.Location = new System.Drawing.Point(514, 15);
            this.txtWindowsLogin.MaxLength = 50;
            this.txtWindowsLogin.Name = "txtWindowsLogin";
            this.txtWindowsLogin.Size = new System.Drawing.Size(117, 21);
            this.txtWindowsLogin.TabIndex = 3;
            // 
            // lblWindowsLogin
            // 
            this.lblWindowsLogin.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblWindowsLogin.ForeColor = System.Drawing.Color.White;
            this.lblWindowsLogin.Location = new System.Drawing.Point(367, 15);
            this.lblWindowsLogin.Name = "lblWindowsLogin";
            this.lblWindowsLogin.Size = new System.Drawing.Size(143, 20);
            this.lblWindowsLogin.TabIndex = 13;
            this.lblWindowsLogin.Text = "Windows Login";
            this.lblWindowsLogin.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblMaxTenderedAmount
            // 
            this.lblMaxTenderedAmount.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMaxTenderedAmount.ForeColor = System.Drawing.Color.White;
            this.lblMaxTenderedAmount.Location = new System.Drawing.Point(367, 140);
            this.lblMaxTenderedAmount.Name = "lblMaxTenderedAmount";
            this.lblMaxTenderedAmount.Size = new System.Drawing.Size(143, 20);
            this.lblMaxTenderedAmount.TabIndex = 11;
            this.lblMaxTenderedAmount.Text = "Max. Tendered Amount";
            // 
            // numMaxTenderedAmount
            // 
            appearance3.FontData.BoldAsString = "False";
            appearance3.FontData.ItalicAsString = "False";
            appearance3.FontData.StrikeoutAsString = "False";
            appearance3.FontData.UnderlineAsString = "False";
            appearance3.ForeColor = System.Drawing.Color.Black;
            appearance3.ForeColorDisabled = System.Drawing.Color.Black;
            this.numMaxTenderedAmount.Appearance = appearance3;
            this.numMaxTenderedAmount.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.numMaxTenderedAmount.Location = new System.Drawing.Point(515, 140);
            this.numMaxTenderedAmount.MaskInput = "{LOC}nn,nnn,nnn.nn";
            this.numMaxTenderedAmount.MaxValue = 1316134911;
            this.numMaxTenderedAmount.MinValue = -9999999;
            this.numMaxTenderedAmount.Name = "numMaxTenderedAmount";
            this.numMaxTenderedAmount.NumericType = Infragistics.Win.UltraWinEditors.NumericType.Double;
            this.numMaxTenderedAmount.Size = new System.Drawing.Size(117, 20);
            this.numMaxTenderedAmount.TabIndex = 8;
            // 
            // label10
            // 
            this.label10.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.ForeColor = System.Drawing.Color.White;
            this.label10.Location = new System.Drawing.Point(367, 115);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(143, 20);
            this.label10.TabIndex = 9;
            this.label10.Text = "Max. Return Trans Amt";
            // 
            // numMaxReturnTransLimit
            // 
            appearance4.FontData.BoldAsString = "False";
            appearance4.FontData.ItalicAsString = "False";
            appearance4.FontData.StrikeoutAsString = "False";
            appearance4.FontData.UnderlineAsString = "False";
            appearance4.ForeColor = System.Drawing.Color.Black;
            appearance4.ForeColorDisabled = System.Drawing.Color.Black;
            this.numMaxReturnTransLimit.Appearance = appearance4;
            this.numMaxReturnTransLimit.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.numMaxReturnTransLimit.Location = new System.Drawing.Point(515, 115);
            this.numMaxReturnTransLimit.MaskInput = "{LOC}nn,nnn,nnn.nn";
            this.numMaxReturnTransLimit.MaxValue = 1316134911;
            this.numMaxReturnTransLimit.MinValue = -9999999;
            this.numMaxReturnTransLimit.Name = "numMaxReturnTransLimit";
            this.numMaxReturnTransLimit.NumericType = Infragistics.Win.UltraWinEditors.NumericType.Double;
            this.numMaxReturnTransLimit.Size = new System.Drawing.Size(117, 20);
            this.numMaxReturnTransLimit.TabIndex = 7;
            // 
            // trvPermissions
            // 
            this.trvPermissions.AllowAutoDragScrolling = false;
            this.trvPermissions.Location = new System.Drawing.Point(14, 26);
            this.trvPermissions.Name = "trvPermissions";
            _override1.LabelEdit = Infragistics.Win.DefaultableBoolean.False;
            _override1.NodeDoubleClickAction = Infragistics.Win.UltraWinTree.NodeDoubleClickAction.ToggleExpansion;
            _override1.NodeStyle = Infragistics.Win.UltraWinTree.NodeStyle.CheckBox;
            _override1.SelectionType = Infragistics.Win.UltraWinTree.SelectType.None;
            this.trvPermissions.Override = _override1;
            this.trvPermissions.Size = new System.Drawing.Size(348, 169);
            this.trvPermissions.TabIndex = 2;
            this.trvPermissions.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.trvPermissions.AfterCheck += new Infragistics.Win.UltraWinTree.AfterNodeChangedEventHandler(this.trvPermissions_AfterCheck);
            // 
            // label5
            // 
            this.label5.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.White;
            this.label5.Location = new System.Drawing.Point(367, 65);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(143, 20);
            this.label5.TabIndex = 5;
            this.label5.Text = "Max. Disc. Limit ( in %)";
            // 
            // label9
            // 
            this.label9.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.ForeColor = System.Drawing.Color.White;
            this.label9.Location = new System.Drawing.Point(367, 90);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(143, 20);
            this.label9.TabIndex = 7;
            this.label9.Text = "Max. Trans Amount";
            // 
            // numMaxPerDisc
            // 
            appearance5.FontData.BoldAsString = "False";
            appearance5.FontData.ItalicAsString = "False";
            appearance5.FontData.StrikeoutAsString = "False";
            appearance5.FontData.UnderlineAsString = "False";
            appearance5.ForeColor = System.Drawing.Color.Black;
            appearance5.ForeColorDisabled = System.Drawing.Color.Black;
            this.numMaxPerDisc.Appearance = appearance5;
            this.numMaxPerDisc.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.numMaxPerDisc.Location = new System.Drawing.Point(515, 65);
            this.numMaxPerDisc.MaskInput = "{LOC}-nnn.nn";
            this.numMaxPerDisc.MaxValue = 100D;
            this.numMaxPerDisc.MinValue = 0D;
            this.numMaxPerDisc.Name = "numMaxPerDisc";
            this.numMaxPerDisc.NumericType = Infragistics.Win.UltraWinEditors.NumericType.Double;
            this.numMaxPerDisc.Size = new System.Drawing.Size(117, 20);
            this.numMaxPerDisc.TabIndex = 5;
            // 
            // btnDisallowAll
            // 
            this.btnDisallowAll.Location = new System.Drawing.Point(146, 0);
            this.btnDisallowAll.Name = "btnDisallowAll";
            this.btnDisallowAll.Size = new System.Drawing.Size(106, 22);
            this.btnDisallowAll.TabIndex = 1;
            this.btnDisallowAll.Text = "Disallow All";
            this.btnDisallowAll.Click += new System.EventHandler(this.chkDisAllowAll_CheckedChanged);
            // 
            // numMaxTransactionLimit
            // 
            appearance6.FontData.BoldAsString = "False";
            appearance6.FontData.ItalicAsString = "False";
            appearance6.FontData.StrikeoutAsString = "False";
            appearance6.FontData.UnderlineAsString = "False";
            appearance6.ForeColor = System.Drawing.Color.Black;
            appearance6.ForeColorDisabled = System.Drawing.Color.Black;
            this.numMaxTransactionLimit.Appearance = appearance6;
            this.numMaxTransactionLimit.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.numMaxTransactionLimit.Location = new System.Drawing.Point(515, 90);
            this.numMaxTransactionLimit.MaskInput = "{LOC}nn,nnn,nnn.nn";
            this.numMaxTransactionLimit.MaxValue = 1316134911;
            this.numMaxTransactionLimit.MinValue = -9999999;
            this.numMaxTransactionLimit.Name = "numMaxTransactionLimit";
            this.numMaxTransactionLimit.NumericType = Infragistics.Win.UltraWinEditors.NumericType.Double;
            this.numMaxTransactionLimit.Size = new System.Drawing.Size(117, 20);
            this.numMaxTransactionLimit.TabIndex = 6;
            // 
            // txtLoginReg
            // 
            this.txtLoginReg.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtLoginReg.Location = new System.Drawing.Point(515, 40);
            this.txtLoginReg.MaxLength = 50;
            this.txtLoginReg.Name = "txtLoginReg";
            this.txtLoginReg.Size = new System.Drawing.Size(117, 21);
            this.txtLoginReg.TabIndex = 4;
            // 
            // btnAllowAll
            // 
            this.btnAllowAll.Location = new System.Drawing.Point(50, 0);
            this.btnAllowAll.Name = "btnAllowAll";
            this.btnAllowAll.Size = new System.Drawing.Size(88, 22);
            this.btnAllowAll.TabIndex = 0;
            this.btnAllowAll.Text = "Allow All";
            this.btnAllowAll.Click += new System.EventHandler(this.chkAllowAll_CheckedChanged);
            // 
            // label7
            // 
            this.label7.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.ForeColor = System.Drawing.Color.White;
            this.label7.Location = new System.Drawing.Point(367, 40);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(143, 20);
            this.label7.TabIndex = 3;
            this.label7.Text = "Login Registration ";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ultraGroupBox2
            // 
            this.ultraGroupBox2.Controls.Add(this.lblEmailID);
            this.ultraGroupBox2.Controls.Add(this.txtEmailID);
            this.ultraGroupBox2.Controls.Add(this.chkChangePasswordAtLogin);
            this.ultraGroupBox2.Controls.Add(this.btnBrowseImage);
            this.ultraGroupBox2.Controls.Add(this.PictUserImg);
            this.ultraGroupBox2.Controls.Add(this.cmbUserGroup);
            this.ultraGroupBox2.Controls.Add(this.txtLastName);
            this.ultraGroupBox2.Controls.Add(this.label2);
            this.ultraGroupBox2.Controls.Add(this.txtFirstName);
            this.ultraGroupBox2.Controls.Add(this.label1);
            this.ultraGroupBox2.Controls.Add(this.chkIsActive);
            this.ultraGroupBox2.Controls.Add(this.txtUserID);
            this.ultraGroupBox2.Controls.Add(this.txtPassword);
            this.ultraGroupBox2.Controls.Add(this.label8);
            this.ultraGroupBox2.Controls.Add(this.numCachDrawerNo);
            this.ultraGroupBox2.Controls.Add(this.label4);
            this.ultraGroupBox2.Controls.Add(this.label6);
            this.ultraGroupBox2.Controls.Add(this.label3);
            this.ultraGroupBox2.Controls.Add(this.numSecurityLevel);
            #region PRIMEPOS-3484
            this.ultraGroupBox2.Controls.Add(this.lblLanID);
            this.ultraGroupBox2.Controls.Add(this.txtLanID);
            #endregion
            this.ultraGroupBox2.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.ultraGroupBox2.ForeColor = System.Drawing.Color.White;
            this.ultraGroupBox2.Location = new System.Drawing.Point(12, 44);
            this.ultraGroupBox2.Name = "ultraGroupBox2";
            this.ultraGroupBox2.Size = new System.Drawing.Size(640, 260);
            this.ultraGroupBox2.TabIndex = 1;
            this.ultraGroupBox2.TabStop = false;
            this.ultraGroupBox2.Text = "User Information";
            // 
            // lblEmailID
            // 
            this.lblEmailID.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblEmailID.ForeColor = System.Drawing.Color.White;
            this.lblEmailID.Location = new System.Drawing.Point(12, 207);
            this.lblEmailID.Name = "lblEmailID";
            this.lblEmailID.Size = new System.Drawing.Size(100, 20);
            this.lblEmailID.TabIndex = 14;
            this.lblEmailID.Text = "Email";
            this.lblEmailID.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtEmailID
            // 
            this.txtEmailID.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtEmailID.CharacterCasing = System.Windows.Forms.CharacterCasing.Lower;
            this.txtEmailID.Location = new System.Drawing.Point(155, 207);
            this.txtEmailID.MaxLength = 50;
            this.txtEmailID.Name = "txtEmailID";
            this.txtEmailID.Size = new System.Drawing.Size(445, 21);
            this.txtEmailID.TabIndex = 15;
            this.txtEmailID.Leave += new System.EventHandler(this.txtEmailID_Leave);
            // 
            // chkChangePasswordAtLogin
            // 
            this.chkChangePasswordAtLogin.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkChangePasswordAtLogin.Location = new System.Drawing.Point(12, 80);
            this.chkChangePasswordAtLogin.Name = "chkChangePasswordAtLogin";
            this.chkChangePasswordAtLogin.Size = new System.Drawing.Size(158, 30);
            this.chkChangePasswordAtLogin.TabIndex = 4;
            this.chkChangePasswordAtLogin.Text = "User Must Change Password At Next Login";
            // 
            // btnBrowseImage
            // 
            appearance7.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(62)))), ((int)(((byte)(176)))));
            appearance7.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance7.FontData.BoldAsString = "True";
            appearance7.ImageAlpha = Infragistics.Win.Alpha.Transparent;
            this.btnBrowseImage.Appearance = appearance7;
            this.btnBrowseImage.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            this.btnBrowseImage.Location = new System.Drawing.Point(466, 129);
            this.btnBrowseImage.Name = "btnBrowseImage";
            this.btnBrowseImage.Size = new System.Drawing.Size(135, 26);
            this.btnBrowseImage.TabIndex = 12;
            this.btnBrowseImage.Text = "&Browse Image";
            this.btnBrowseImage.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnBrowseImage.Click += new System.EventHandler(this.btnBrowseImage_Click);
            // 
            // PictUserImg
            // 
            this.PictUserImg.Image = global::POS_Core_UI.Properties.Resources.defaultUser;
            this.PictUserImg.Location = new System.Drawing.Point(466, 20);
            this.PictUserImg.Name = "PictUserImg";
            this.PictUserImg.Size = new System.Drawing.Size(134, 106);
            this.PictUserImg.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.PictUserImg.TabIndex = 42;
            this.PictUserImg.TabStop = false;
            // 
            // cmbUserGroup
            // 
            this.cmbUserGroup.AutoSize = false;
            this.cmbUserGroup.DisplayStyle = Infragistics.Win.EmbeddableElementDisplayStyle.OfficeXP;
            this.cmbUserGroup.DropDownListWidth = -1;
            this.cmbUserGroup.DropDownStyle = Infragistics.Win.DropDownStyle.DropDownList;
            this.cmbUserGroup.Location = new System.Drawing.Point(155, 120);
            this.cmbUserGroup.Name = "cmbUserGroup";
            this.cmbUserGroup.Size = new System.Drawing.Size(172, 20);
            this.cmbUserGroup.TabIndex = 6;
            this.cmbUserGroup.ValueChanged += new System.EventHandler(this.cmbUserGroup_ValueChanged);
            // 
            // txtLastName
            // 
            this.txtLastName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtLastName.Location = new System.Drawing.Point(428, 180);
            this.txtLastName.MaxLength = 50;
            this.txtLastName.Name = "txtLastName";
            this.txtLastName.Size = new System.Drawing.Size(172, 21);
            this.txtLastName.TabIndex = 13;
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(345, 180);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(79, 20);
            this.label2.TabIndex = 12;
            this.label2.Text = "Last Name";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtFirstName
            // 
            this.txtFirstName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtFirstName.Location = new System.Drawing.Point(155, 180);
            this.txtFirstName.MaxLength = 50;
            this.txtFirstName.Name = "txtFirstName";
            this.txtFirstName.Size = new System.Drawing.Size(172, 21);
            this.txtFirstName.TabIndex = 11;
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(12, 180);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(100, 20);
            this.label1.TabIndex = 10;
            this.label1.Text = "First Name";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // chkIsActive
            // 
            this.chkIsActive.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkIsActive.Checked = true;
            this.chkIsActive.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkIsActive.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chkIsActive.ForeColor = System.Drawing.Color.White;
            this.chkIsActive.Location = new System.Drawing.Point(345, 151);
            this.chkIsActive.Name = "chkIsActive";
            this.chkIsActive.Size = new System.Drawing.Size(94, 19);
            this.chkIsActive.TabIndex = 9;
            this.chkIsActive.Text = "Active";
            // 
            // txtUserID
            // 
            this.txtUserID.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtUserID.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtUserID.Location = new System.Drawing.Point(155, 20);
            this.txtUserID.MaxLength = 10;
            this.txtUserID.Name = "txtUserID";
            this.txtUserID.Size = new System.Drawing.Size(172, 21);
            this.txtUserID.TabIndex = 1;
            // 
            // txtPassword
            // 
            this.txtPassword.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtPassword.Location = new System.Drawing.Point(155, 50);
            this.txtPassword.MaxLength = 50;
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.PasswordChar = '*';
            this.txtPassword.Size = new System.Drawing.Size(172, 21);
            this.txtPassword.TabIndex = 3;
            // 
            // label8
            // 
            this.label8.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.ForeColor = System.Drawing.Color.White;
            this.label8.Location = new System.Drawing.Point(12, 20);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(100, 20);
            this.label8.TabIndex = 0;
            this.label8.Text = "User ID";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // numCachDrawerNo
            // 
            this.numCachDrawerNo.AutoSize = false;
            this.numCachDrawerNo.DisplayStyle = Infragistics.Win.EmbeddableElementDisplayStyle.OfficeXP;
            this.numCachDrawerNo.DropDownStyle = Infragistics.Win.DropDownStyle.DropDownList;
            valueListItem12.DataValue = "1";
            valueListItem12.DisplayText = "1";
            valueListItem11.DataValue = valueListItem12;
            valueListItem11.DisplayText = "1";
            valueListItem10.DataValue = valueListItem11;
            valueListItem10.DisplayText = "1";
            valueListItem15.DataValue = "2";
            valueListItem15.DisplayText = "2";
            valueListItem14.DataValue = valueListItem15;
            valueListItem14.DisplayText = "2";
            valueListItem13.DataValue = valueListItem14;
            valueListItem13.DisplayText = "2";
            this.numCachDrawerNo.Items.AddRange(new Infragistics.Win.ValueListItem[] {
            valueListItem10,
            valueListItem13});
            this.numCachDrawerNo.Location = new System.Drawing.Point(155, 150);
            this.numCachDrawerNo.Name = "numCachDrawerNo";
            this.numCachDrawerNo.Size = new System.Drawing.Size(172, 20);
            this.numCachDrawerNo.TabIndex = 8;
            // 
            // label4
            // 
            this.label4.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.White;
            this.label4.Location = new System.Drawing.Point(12, 150);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(100, 20);
            this.label4.TabIndex = 7;
            this.label4.Text = "Cash Drawer #";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label6
            // 
            this.label6.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.Color.White;
            this.label6.Location = new System.Drawing.Point(12, 120);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(100, 20);
            this.label6.TabIndex = 5;
            this.label6.Text = "User Group ";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(12, 50);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(100, 20);
            this.label3.TabIndex = 2;
            this.label3.Text = "Password";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // numSecurityLevel
            // 
            this.numSecurityLevel.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.numSecurityLevel.Location = new System.Drawing.Point(155, 120);
            this.numSecurityLevel.MaskInput = "n";
            this.numSecurityLevel.MaxValue = 9;
            this.numSecurityLevel.MinValue = 0;
            this.numSecurityLevel.Name = "numSecurityLevel";
            this.numSecurityLevel.Size = new System.Drawing.Size(169, 20);
            this.numSecurityLevel.TabIndex = 9;
            this.numSecurityLevel.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.numSecurityLevel.Visible = false;
            this.numSecurityLevel.ValueChanged += new System.EventHandler(this.numSecurityLevel_ValueChanged);
            this.numSecurityLevel.Enter += new System.EventHandler(this.numSecurityLevel_Enter);
            #region PRIMEPOS-3484
            // 
            // lblLanID
            // 
            this.lblLanID.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLanID.ForeColor = System.Drawing.Color.White;
            this.lblLanID.Location = new System.Drawing.Point(12, 234);
            this.lblLanID.Name = "lblLanID";
            this.lblLanID.Size = new System.Drawing.Size(100, 20);
            this.lblLanID.TabIndex = 14;
            this.lblLanID.Text = "LanID";
            this.lblLanID.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtLanID
            // 
            this.txtLanID.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtLanID.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal;
            this.txtLanID.Location = new System.Drawing.Point(155, 234);
            this.txtLanID.MaxLength = 50;
            this.txtLanID.Name = "txtLanID";
            this.txtLanID.Size = new System.Drawing.Size(445, 21);
            this.txtLanID.TabIndex = 15;
            this.txtLanID.Leave += new System.EventHandler(this.txtLanID_Leave);
            #endregion
            // 
            // ultraGroupBox3
            // 
            this.ultraGroupBox3.Controls.Add(this.btnEnrollFingerprint);
            this.ultraGroupBox3.Controls.Add(this.btnUserNotes);
            this.ultraGroupBox3.Controls.Add(this.btnCancel);
            this.ultraGroupBox3.Controls.Add(this.btnOk);
            this.ultraGroupBox3.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.ultraGroupBox3.ForeColor = System.Drawing.Color.White;
            this.ultraGroupBox3.Location = new System.Drawing.Point(12, 515);
            this.ultraGroupBox3.Name = "ultraGroupBox3";
            this.ultraGroupBox3.Size = new System.Drawing.Size(638, 50);
            this.ultraGroupBox3.TabIndex = 3;
            this.ultraGroupBox3.TabStop = false;
            // 
            // btnEnrollFingerprint
            // 
            appearance8.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(62)))), ((int)(((byte)(176)))));
            appearance8.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance8.FontData.BoldAsString = "True";
            appearance8.ForeColor = System.Drawing.Color.White;
            this.btnEnrollFingerprint.Appearance = appearance8;
            this.btnEnrollFingerprint.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            this.btnEnrollFingerprint.Location = new System.Drawing.Point(184, 14);
            this.btnEnrollFingerprint.Name = "btnEnrollFingerprint";
            this.btnEnrollFingerprint.Size = new System.Drawing.Size(140, 26);
            this.btnEnrollFingerprint.TabIndex = 3;
            this.btnEnrollFingerprint.TabStop = false;
            this.btnEnrollFingerprint.Text = "Enroll Fingerprint";
            this.btnEnrollFingerprint.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnEnrollFingerprint.Visible = false;
            this.btnEnrollFingerprint.Click += new System.EventHandler(this.btnEnrollFingerprint_Click);
            // 
            // btnUserNotes
            // 
            appearance9.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(62)))), ((int)(((byte)(176)))));
            appearance9.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance9.FontData.BoldAsString = "True";
            appearance9.ForeColor = System.Drawing.Color.White;
            this.btnUserNotes.Appearance = appearance9;
            this.btnUserNotes.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            this.btnUserNotes.Location = new System.Drawing.Point(333, 14);
            this.btnUserNotes.Name = "btnUserNotes";
            this.btnUserNotes.Size = new System.Drawing.Size(124, 26);
            this.btnUserNotes.TabIndex = 2;
            this.btnUserNotes.TabStop = false;
            this.btnUserNotes.Text = "Notes (F6)";
            this.btnUserNotes.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnUserNotes.Visible = false;
            this.btnUserNotes.Click += new System.EventHandler(this.btnUserNotes_Click);
            // 
            // btnCancel
            // 
            appearance10.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(62)))), ((int)(((byte)(176)))));
            appearance10.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance10.FontData.BoldAsString = "True";
            this.btnCancel.Appearance = appearance10;
            this.btnCancel.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(553, 14);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(78, 26);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnCancel.Click += new System.EventHandler(this.ultraButton2_Click);
            // 
            // btnOk
            // 
            appearance11.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(62)))), ((int)(((byte)(176)))));
            appearance11.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance11.FontData.BoldAsString = "True";
            this.btnOk.Appearance = appearance11;
            this.btnOk.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            this.btnOk.Location = new System.Drawing.Point(466, 14);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(78, 26);
            this.btnOk.TabIndex = 0;
            this.btnOk.Text = "&Ok";
            this.btnOk.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // lblTransactionType
            // 
            appearance12.ForeColor = System.Drawing.Color.White;
            appearance12.ForeColorDisabled = System.Drawing.Color.White;
            appearance12.TextHAlignAsString = "Center";
            this.lblTransactionType.Appearance = appearance12;
            this.lblTransactionType.BackColorInternal = System.Drawing.Color.Transparent;
            this.lblTransactionType.Font = new System.Drawing.Font("Arial", 18F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTransactionType.Location = new System.Drawing.Point(17, 11);
            this.lblTransactionType.Name = "lblTransactionType";
            this.lblTransactionType.Size = new System.Drawing.Size(604, 30);
            this.lblTransactionType.TabIndex = 0;
            this.lblTransactionType.Tag = "Header";
            this.lblTransactionType.Text = "User Management";
            // 
            // frmUserInformation
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(108)))), ((int)(((byte)(172)))));
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(664, 571);
            this.Controls.Add(this.lblTransactionType);
            this.Controls.Add(this.ultraGroupBox3);
            this.Controls.Add(this.ultraGroupBox2);
            this.Controls.Add(this.gbRights);
            this.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.Name = "frmUserInformation";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "User Management";
            this.Load += new System.EventHandler(this.frmUserManagement_Load);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.frmUserInformation_KeyUp);
            this.gbRights.ResumeLayout(false);
            this.gbRights.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numHourlyRate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numMaxCashbackLimit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numMaxTenderedAmount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numMaxReturnTransLimit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trvPermissions)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numMaxPerDisc)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numMaxTransactionLimit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraDataSource1)).EndInit();
            this.ultraGroupBox2.ResumeLayout(false);
            this.ultraGroupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chkChangePasswordAtLogin)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PictUserImg)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbUserGroup)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numCachDrawerNo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numSecurityLevel)).EndInit();
            this.ultraGroupBox3.ResumeLayout(false);
            this.ResumeLayout(false);

        }
        #endregion

        private void frmUserManagement_Load(object sender, System.EventArgs e)
        {
            this.txtUserID.Enter += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
            this.txtUserID.Leave += new System.EventHandler(clsUIHelper.AfterExitEditMode);

            this.txtPassword.Enter += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
            this.txtPassword.Leave += new System.EventHandler(clsUIHelper.AfterExitEditMode);

            this.numCachDrawerNo.AfterEnterEditMode += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
            this.numCachDrawerNo.AfterExitEditMode += new System.EventHandler(clsUIHelper.AfterExitEditMode);

            this.numSecurityLevel.AfterEnterEditMode += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
            this.numSecurityLevel.AfterExitEditMode += new System.EventHandler(clsUIHelper.AfterExitEditMode);

            this.txtLoginReg.Enter += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
            this.txtLoginReg.Leave += new System.EventHandler(clsUIHelper.AfterExitEditMode);
            this.txtPassword.Validating += new CancelEventHandler(txtPassword_Validating);

            this.numMaxPerDisc.Enter += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
            this.numMaxPerDisc.Leave += new System.EventHandler(clsUIHelper.AfterExitEditMode);

            this.cmbUserGroup.Enter += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
            this.cmbUserGroup.Leave += new System.EventHandler(clsUIHelper.AfterExitEditMode);

            this.numHourlyRate.Enter += new System.EventHandler(clsUIHelper.AfterEnterEditMode);    //PRIMEPOS-189 09-Aug-2021 JY Added
            this.numHourlyRate.Leave += new System.EventHandler(clsUIHelper.AfterExitEditMode); //PRIMEPOS-189 09-Aug-2021 JY Added
            this.txtEmailID.Enter += new System.EventHandler(clsUIHelper.AfterEnterEditMode);   //PRIMEPOS-2989 11-Aug-2021 JY Added
            this.txtEmailID.Leave += new System.EventHandler(clsUIHelper.AfterExitEditMode);    //PRIMEPOS-2989 11-Aug-2021 JY Added
            #region PRIMEPOS-3484
            this.txtLanID.Enter += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
            this.txtLanID.Leave += new System.EventHandler(clsUIHelper.AfterExitEditMode);
            #endregion
            LoadUserGroups();
            BuildTreeView();

            #region PRIMEPOS-189 09-Aug-2021 JY Added
            int nEditHourlyRate = Configuration.convertBoolToInt(UserPriviliges.IsUserHasPriviliges(UserPriviliges.Modules.Timesheet.ID, UserPriviliges.Screens.EditHourlyRate.ID));
            if (nEditHourlyRate == 1)
                lblHourlyRate.Visible = numHourlyRate.Visible = true;
            else
            {
                int nDisplayHourlyRate = Configuration.convertBoolToInt(UserPriviliges.IsUserHasPriviliges(UserPriviliges.Modules.Timesheet.ID, UserPriviliges.Screens.DisplayHourlyRate.ID));
                if (nDisplayHourlyRate == 0)
                    lblHourlyRate.Visible = numHourlyRate.Visible = false;
                else
                    lblHourlyRate.Enabled = numHourlyRate.Enabled = false;
            }
            #endregion

            if (mSaveMode == SaveModeENUM.Modify)
                Display(mUserId, false);
            else
                txtPassword.MaxLength = 15; //PRIMEPOS-2562 07-Aug-2018 JY Added

            clsUIHelper.setColorSchecme(this);
        }

        /// <summary>
        /// Author: Shitaljit
        /// To load all available user groups to get basic settings.
        /// </summary>
        private void LoadUserGroups()
        {
            string sSQL = string.Empty;
            cmbUserGroup.Items.Clear();
            try
            {
                sSQL = "SELECT 0 AS ID,' ' AS FNAME UNION SELECT ID, FName from Users Where UserType = 'G' ORDER BY FNAME"; //PRIMEPOS-2780 27-Sep-2021 JY modified   //PRIMEPOS-3038 14-Dec-2021 JY Modified
                POS_Core.BusinessRules.Search oSearch = new POS_Core.BusinessRules.Search();
                DataSet oDataSet = oSearch.SearchData(sSQL);
                if (Configuration.isNullOrEmptyDataSet(oDataSet) == false)
                {
                    foreach (DataRow oRow in oDataSet.Tables[0].Rows)
                    {
                        cmbUserGroup.Items.Add(oRow[clsPOSDBConstants.Users_Fld_ID].ToString(), oRow[clsPOSDBConstants.Users_Fld_fName].ToString());    //PRIMEPOS-2780 27-Sep-2021 JY modified
                    }
                }
            }
            catch (Exception Ex)
            {
                clsUIHelper.ShowErrorMsg(Ex.Message);
            }
        }
        void txtPassword_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(txtPassword.Text))
                {
                    //following if- else is Added By shitaljit on 10 Oct 2012 to bypass password complexity check logic if Enforce Complex Password is true.
                    if (Configuration.CInfo.EnforceComplexPassword == true)
                    {
                        #region PRIMEPOS-2562 02-Aug-2018 JY Added
                        const string errorMessageTitle = "User Authentication";
                        string strMessage = string.Empty;
                        Boolean bIsValid = frmChangePassword.ValidateNewPassword(txtUserID.Text.Trim(), txtPassword.Text.Trim(), ref strMessage);
                        if (bIsValid == false)
                        {
                            gbRights.Enabled = false;
                            Resources.Message.Display(strMessage, errorMessageTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                            e.Cancel = true;
                            txtPassword.Focus();
                        }
                        else
                        {
                            gbRights.Enabled = true;
                        }
                        #endregion

                        #region PRIMEPOS-2562 02-Aug-2018 JY Commented
                        //if (txtPassword.Text.Length < 7)
                        //{
                        //    gbRights.Enabled = false;
                        //    POS_Core_UI.Resources.Message.Display("Error in validating the password. Please ensure \n 1.Password should be atleast seven characters \n 2.Password should be alphanumeric \n 3.Password should not the same as any of the last four passwords \n 4.Password should contain special characters", "User Management", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                        //    //txtPassword.Focus();
                        //    e.Cancel = true;
                        //    txtPassword.Select(0, this.txtPassword.Text.Length);
                        //}
                        //else
                        //{
                        //    gbRights.Enabled = true;
                        //    if (!(IsValidAlphaNumeric(this.txtPassword.Text)))
                        //    {
                        //        gbRights.Enabled = false;
                        //        POS_Core_UI.Resources.Message.Display("Error in validating the password. Please ensure \n 1.Password should be atleast seven characters \n 2.Password should be alphanumeric \n 3.Password should not the same as any of the last four passwords \n 4.Password should contain special characters", "User Management", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                        //        //txtPassword.Focus();
                        //        e.Cancel = true;
                        //        txtPassword.Select(0, this.txtPassword.Text.Length);
                        //    }
                        //    else if (!(IsValidPassword(this.txtUserID.Text)))
                        //    {
                        //        gbRights.Enabled = false;
                        //        POS_Core_UI.Resources.Message.Display("Error in validating the password. Please ensure \n 1.Password should be atleast seven characters \n 2.Password should be alphanumeric \n 3.Password should not the same as any of the last four passwords \n 4.Password should contain special characters", "User Management", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                        //        //txtPassword.Focus();
                        //        e.Cancel = true;
                        //        txtPassword.Select(0, this.txtPassword.Text.Length);
                        //    }
                        //    else
                        //    {
                        //        gbRights.Enabled = true;
                        //    }
                        //}
                        #endregion
                    }
                    else
                    {
                        if (txtPassword.Text.Length < 4)
                        {
                            POS_Core_UI.Resources.Message.Display("Error in validating the password.\nPassword should be atleast four characters", "User Authentication", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            txtPassword.Focus();
                        }
                    }
                }
            }
            catch (Exception exp)
            {
            }
        }

        private void ultraButton2_Click(object sender, System.EventArgs e)
        {
            IsCanceled = true;
            this.Close();
        }

        public void SetNew()
        {
            mSaveMode = SaveModeENUM.Create;
            Clear();
            numCachDrawerNo.SelectedIndex = 0;
        }

        public void Edit(string UserId)
        {
            mSaveMode = SaveModeENUM.Modify;
            txtUserID.Enabled = false;
            this.btnUserNotes.Visible = UserPriviliges.IsUserHasPriviliges(UserPriviliges.Modules.Notes.ID, UserPriviliges.Screens.UserNotes.ID); //Added by shitaljit(QuicSolv) on 11 October 2011
            mUserId = UserId;
            //			Display(UserId);
        }

        private bool ValidateFields()
        {
            try
            {
                if (txtUserID.Text.Trim() == "")
                {
                    clsUIHelper.ShowErrorMsg("User Name should not be blank");
                    txtUserID.Focus();
                    return false;
                }
                if (string.IsNullOrEmpty(txtPassword.Text))
                {
                    clsUIHelper.ShowErrorMsg("Password should not be blank");
                    txtPassword.Focus();
                    return false;
                }
                if (!string.IsNullOrWhiteSpace(this.txtEmailID.Text) && !CheckvalidEmailid.IsMatch(txtEmailID.Text.Trim()))
                {
                    clsUIHelper.ShowErrorMsg("Invalid email format");
                    txtEmailID.Focus();
                    return false;
                }
                if (POS_Core.Resources.Configuration.CInfo.AuthenticationMode == 3 && string.IsNullOrWhiteSpace(this.txtEmailID.Text) && POS_Core.Resources.Configuration.CInfo.SSOIdentifier == (int)POS_Core.Resources.SSOIdentifier.Email)
                {
                    clsUIHelper.ShowErrorMsg("EmailID should not be blank");
                    txtEmailID.Focus();
                    return false;
                }
                #region PRIMEPOS-3484
                if (POS_Core.Resources.Configuration.CInfo.AuthenticationMode == 3 && string.IsNullOrWhiteSpace(this.txtLanID.Text) && POS_Core.Resources.Configuration.CInfo.SSOIdentifier == (int)POS_Core.Resources.SSOIdentifier.LanID)
                {
                    clsUIHelper.ShowErrorMsg("LanID should not be blank");
                    txtLanID.Focus();
                    return false;
                }
                #endregion
                return true;
            }

            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
                return false;
            }
        }

        private bool Save()
        {
            if (ValidateFields() == false)
                return false;

            IDbTransaction tr = null;
            IDbConnection conn = DataFactory.CreateConnection();



            try
            {
                try
                {
                    if (picArray == null || picArray.Length < 1)
                    {
                        MemoryStream ms = new MemoryStream();
                        PictUserImg.Image.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
                        byte[] pic_arr = new byte[ms.Length];
                        ms.Position = 0;
                        ms.Read(pic_arr, 0, pic_arr.Length);
                        picArray = pic_arr;
                    }

                }
                catch (Exception)
                {

                    //  throw;
                }
                IDbCommand cmd = DataFactory.CreateCommand();

                bool retValue = false;

                if (mSaveMode == SaveModeENUM.Create)
                {
                    retValue = SaveNewUser();
                    if (retValue && Configuration.UserName != null)
                        ErrorHandler.SaveLog((int)LogENUM.Create_User, Configuration.UserName, "Success", "New user created");

                }
                else
                {
                    retValue = SaveEditUser();
                    if (retValue && Configuration.UserName != null)
                        ErrorHandler.SaveLog((int)LogENUM.Update_User, Configuration.UserName, "Success", "User information updated");

                }
                #region PRIMEPOS-3419
                if (EditUserInventoryMenu() && Configuration.UserName != null)
                    ErrorHandler.SaveLog((int)LogENUM.Update_User, Configuration.UserName, "Success", "Inventory menu permission for user updated");
                #endregion

                if (retValue == true)
                {
                    mSaveMode = SaveModeENUM.Modify;
                    return true;
                }
                else
                {
                    return false;
                }

            }
            catch (System.Data.SqlClient.SqlException exp)
            {
                if (exp.Number == 2627)
                    clsUIHelper.ShowErrorMsg(" User id already exist");
                else
                    clsUIHelper.ShowErrorMsg(exp.Message);

                tr.Rollback();
                conn.Close();
                return false;
            }
            catch (Exception exp)
            {

                clsUIHelper.ShowErrorMsg(exp.Message);
                tr.Rollback();
                conn.Close();
                return false;
            }

        }

        private bool SaveNewUser()
        {
            if (ValidateFields() == false)
                return false;

            IDbTransaction tr = null;
            IDbConnection conn = DataFactory.CreateConnection();
            try
            {
                IDbCommand cmd = DataFactory.CreateCommand();

                string sSQL = "";
                //Modified By Shitaljit(QuicSolv) on July 7 2011
                //Added Field Users_Fld_LastLoginAttempt on the isert Query. 
                //To Resolve error while login with new user as the Users_Fld_LastLoginAttempt is null
                sSQL = "INSERT INTO " + clsPOSDBConstants.Users_tbl + "( " +
                        clsPOSDBConstants.Users_Fld_UserID +
                        " , " + clsPOSDBConstants.Users_Fld_Password +
                        " , " + clsPOSDBConstants.Users_Fld_DrawNo +
                        " , " + clsPOSDBConstants.Users_Fld_SecurityLevel +
                        " , " + clsPOSDBConstants.Users_Fld_IsActive +
                        " , " + clsPOSDBConstants.Users_Fld_loginRegistration +
                        " , " + clsPOSDBConstants.Users_Fld_fName +
                        " , " + clsPOSDBConstants.Users_Fld_lName +
                        " , " + clsPOSDBConstants.Users_Fld_IsLocked +
                        " , " + clsPOSDBConstants.Users_Fld_PasswordChangedOn +
                        " , " + clsPOSDBConstants.Users_Fld_LastLoginAttempt +
                        " , " + clsPOSDBConstants.Users_Fld_MaxDiscountLimit + //added by shitaljit to define % disc user can give
                        " , " + clsPOSDBConstants.Users_Fld_MaxTransactionLimit + //added by Ravindra to Maximum Trasaction Limit user can give
                        " , " + clsPOSDBConstants.Users_Fld_UserImage +
                        " , " + clsPOSDBConstants.Users_Fld_MaxReturnTransLimit +    //Sprint-23 - PRIMEPOS-2303 24-May-2016 JY Added 
                        " , " + clsPOSDBConstants.Users_Fld_MaxTenderedAmountLimit +    //Sprint-25 - PRIMEPOS-2411 18-May-2017 JY Added 
                        " , " + clsPOSDBConstants.Users_Fld_MaxCashbackLimit +  //PRIMEPOS-2741 25-Sep-2019 JY Added
                        " , " + clsPOSDBConstants.Users_Fld_ModifiedBy + //PRIMEPOS-2562 01-Aug-2018 JY Added
                        " , " + clsPOSDBConstants.Users_Fld_ChangePasswordAtLogin + //PRIMEPOS-2577 15-Aug-2018 JY Added
                        " , " + clsPOSDBConstants.Users_Fld_WindowsLoginId + //PRIMEPOS-2616 14-Dec-2018 JY Added
                        " , " + clsPOSDBConstants.Users_Fld_HourlyRate +    //PRIMEPOS-189 02-Aug-2021 JY Added
                        " , " + clsPOSDBConstants.Users_Fld_EmailID +   //PRIMEPOS-2989 11-Aug-2021 JY Added
                        " , " + clsPOSDBConstants.Users_Fld_GroupID +   //PRIMEPOS-2780 27-Sep-2021 JY Added
                        " , " + clsPOSDBConstants.Users_Fld_LanID +   //PRIMEPOS-3484
                         " ) " +
                        " VALUES( " +
                        "'" + txtUserID.Text.Trim().Replace("'", "''") + "'" +
                       " , '" + EncryptString.Encrypt(txtPassword.Text.Trim().Replace("'", "''")) + "'" +
                        " , " + Configuration.convertNullToInt(numCachDrawerNo.Value) +
                        " , " + numSecurityLevel.Value +
                        " , " + ((chkIsActive.Checked == true) ? 1 : 0) +
                        " , '" + txtLoginReg.Text.Trim().Replace("'", "''") + "'" +
                        " , '" + txtFirstName.Text.Trim().Replace("'", "''") + "'" +
                        " , '" + txtLastName.Text.Trim().Replace("'", "''") + "'" +
                        " , " + 0 + "" +
                        ", '" + System.DateTime.Now + "'" +
                        ", '" + System.DateTime.Now + "'" +
                        ", '" + this.numMaxPerDisc.Value + "'" +//added by shitaljit
                          ", '" + this.numMaxTransactionLimit.Value + "'" +//added by Ravindra
                                                                           // ", CONVERT(varbinary(max),'" + this.picArray + "')" +//added by Ravindra
                         ", " + "@IMG" +
                         ", '" + this.numMaxReturnTransLimit.Value + "'" +  //Sprint-23 - PRIMEPOS-2303 24-May-2016 JY Added 
                         ", '" + this.numMaxTenderedAmount.Value + "'" +  //Sprint-25 - PRIMEPOS-2411 18-May-2017 JY Added 
                         ", '" + this.numMaxCashbackLimit.Value + "'" +    //PRIMEPOS-2741 25-Sep-2019 JY Added
                         ", '" + Configuration.UserName.Trim().Replace("'", "''") + "'" +    //PRIMEPOS-2562 01-Aug-2018 JY Added
                         "," + Configuration.convertBoolToInt(chkChangePasswordAtLogin.Checked) +    //PRIMEPOS-2577 15-Aug-2018 JY Added
                         " , '" + txtWindowsLogin.Text.Trim().Replace("'", "''") + "'" +    //PRIMEPOS-2616 14-Dec-2018 JY Added
                         ", '" + this.numHourlyRate.Value + "'" +   //PRIMEPOS-189 02-Aug-2021 JY Added
                         ", '" + this.txtEmailID.Text.Trim().Replace("'","''") + "'" +   //PRIMEPOS-2989 11-Aug-2021 JY Added
                         ", " + (cmbUserGroup.Text.Trim() != "" ? Configuration.convertNullToInt(cmbUserGroup.Value) : 0) + //PRIMEPOS-2780 27-Sep-2021 JY Added
                         ", '" + this.txtLanID.Text.Trim().Replace("'", "''") + "'" +  //PRIMEPOS-3484
                        ")";

                conn.ConnectionString = Configuration.ConnectionString;

                byte[] img = picArray;

                conn.Open();
                tr = conn.BeginTransaction();
                cmd.Parameters.Add(new SqlParameter("@IMG", img));
                //cmd.CommandType = CommandType.Text;
                cmd.CommandText = sSQL;
                cmd.Transaction = tr;
                cmd.Connection = conn;
                cmd.ExecuteNonQuery();

                cmd.CommandText = "delete from util_userrights where userid='" + this.txtUserID.Text.Trim().Replace("'", "''") + "'";
                cmd.ExecuteNonQuery();


                for (int i = 0; i < this.trvPermissions.Nodes.Count; i++)
                {
                    Infragistics.Win.UltraWinTree.UltraTreeNode oNode = this.trvPermissions.Nodes[i];
                    sSQL = " insert into Util_UserRights (UserID,ModuleID,ScreenID,PermissionID,isAllowed) ";
                    sSQL += " values ('" + this.txtUserID.Text.Trim().Replace("'", "''") + "'," +
                        Configuration.convertNullToInt(oNode.Tag.ToString()).ToString() + ",null,null," + Configuration.convertBoolToInt(oNode.CheckedState == CheckState.Checked).ToString() + ")";
                    cmd.CommandText = sSQL;
                    cmd.ExecuteNonQuery();
                    for (int j = 0; j < oNode.Nodes.Count; j++)
                    {
                        Infragistics.Win.UltraWinTree.UltraTreeNode oNode1 = oNode.Nodes[j];
                        sSQL = " insert into Util_UserRights (UserID,ModuleID,ScreenID,PermissionID,isAllowed) ";
                        sSQL += " values ('" + this.txtUserID.Text.Trim().Replace("'", "''") + "'," +
                            Configuration.convertNullToInt(oNode.Tag.ToString()).ToString() + "," +
                            Configuration.convertNullToInt(oNode1.Tag.ToString()).ToString() + ",null," + Configuration.convertBoolToInt(oNode1.CheckedState == CheckState.Checked).ToString() + ")";
                        cmd.CommandText = sSQL;
                        cmd.ExecuteNonQuery();
                        for (int k = 0; k < oNode1.Nodes.Count; k++)
                        {
                            Infragistics.Win.UltraWinTree.UltraTreeNode oNode2 = oNode1.Nodes[k];
                            sSQL = " insert into Util_UserRights (UserID,ModuleID,ScreenID,PermissionID,isAllowed) ";
                            sSQL += " values ('" + this.txtUserID.Text.Trim().Replace("'", "''") + "'," +
                                Configuration.convertNullToInt(oNode.Tag.ToString()).ToString() + "," +
                                Configuration.convertNullToInt(oNode1.Tag.ToString()).ToString() + "," +
                                Configuration.convertNullToInt(oNode2.Tag.ToString()).ToString() + "," + Configuration.convertBoolToInt(oNode2.CheckedState == CheckState.Checked).ToString() + ")";
                            cmd.CommandText = sSQL;
                            cmd.ExecuteNonQuery();
                        }
                    }
                }

                //DBUser.CreateDBUser(txtUserID.Text.Trim().Replace("'", "''"), txtPassword.Text.Trim().Replace("'", "''"));//PRIMEPOS-3185

                tr.Commit();
                conn.Close();
                mSaveMode = SaveModeENUM.Modify;

                return true;
            }
            catch (System.Data.SqlClient.SqlException exp)
            {
                if (exp.Number == 2627)
                    clsUIHelper.ShowErrorMsg(" User id already exist");
                else
                    clsUIHelper.ShowErrorMsg(exp.Message);

                tr.Rollback();
                conn.Close();
                return false;
            }
            catch (Exception exp)
            {

                clsUIHelper.ShowErrorMsg(exp.Message);
                tr.Rollback();
                conn.Close();
                return false;
            }

        }

        private bool SaveEditUser()
        {
            if (ValidateFields() == false) return false;
            IDbTransaction tr = null;
            IDbConnection conn = DataFactory.CreateConnection();
            try
            {
                IDbCommand cmd = DataFactory.CreateCommand();
                string sSQL = "UPDATE " + clsPOSDBConstants.Users_tbl + " SET "
                    + clsPOSDBConstants.Users_Fld_Password + " = '" + EncryptString.Encrypt(txtPassword.Text.Trim().Replace("'", "''")) + "'" +
                    " , " + clsPOSDBConstants.Users_Fld_DrawNo + " = " + Configuration.convertNullToInt(numCachDrawerNo.Value) +
                    " , " + clsPOSDBConstants.Users_Fld_SecurityLevel + " = " + numSecurityLevel.Value +
                    " , " + clsPOSDBConstants.Users_Fld_IsActive + " = " + ((chkIsActive.Checked == true) ? 1 : 0) +
                    " , " + clsPOSDBConstants.Users_Fld_loginRegistration + " = '" + txtLoginReg.Text.Trim().Replace("'", "''") + "'" +
                    " , " + clsPOSDBConstants.Users_Fld_fName + " = '" + txtFirstName.Text.Trim().Replace("'", "''") + "'" +
                    " , " + clsPOSDBConstants.Users_Fld_lName + " = '" + txtLastName.Text.Trim().Replace("'", "''") + "'" +
                    " , " + clsPOSDBConstants.Users_Fld_IsLocked + " = " + 0 + "" +
                    " , " + clsPOSDBConstants.Users_Fld_MaxDiscountLimit + " = '" + this.numMaxPerDisc.Value + "'" +//added by shitaljit to define % disc user can give
                    " , " + clsPOSDBConstants.Users_Fld_MaxTransactionLimit + " = '" + this.numMaxTransactionLimit.Value + "'" +//added by Ravindra to define Max Transaction Limit  user can give
                    " , " + clsPOSDBConstants.Users_Fld_UserImage + " = @IMG" +//added by Ravindra to define Max Transaction Limit  user can give
                    " , " + clsPOSDBConstants.Users_Fld_MaxReturnTransLimit + " = '" + this.numMaxReturnTransLimit.Value + "'" +    //Sprint-23 - PRIMEPOS-2303 24-May-2016 JY Added 
                    " , " + clsPOSDBConstants.Users_Fld_MaxTenderedAmountLimit + " = '" + this.numMaxTenderedAmount.Value + "'" +    //Sprint-25 - PRIMEPOS-2411 20-Apr-2017 JY Added 
                    " , " + clsPOSDBConstants.Users_Fld_MaxCashbackLimit + " = '" + this.numMaxCashbackLimit.Value + "'" +   //PRIMEPOS-2741 25-Sep-2019 JY Added   
                    " , " + clsPOSDBConstants.Users_Fld_ModifiedBy + " = '" + Configuration.UserName.Trim().Replace("'", "''") + "'" +   //PRIMEPOS-2562 01-Aug-2018 JY Added
                    " , " + clsPOSDBConstants.Users_Fld_ChangePasswordAtLogin + " = " + Configuration.convertBoolToInt(chkChangePasswordAtLogin.Checked) +   //PRIMEPOS-2577 15-Aug-2018 JY Added
                    " , " + clsPOSDBConstants.Users_Fld_WindowsLoginId + " = '" + txtWindowsLogin.Text.Trim().Replace("'", "''") + "'" +    //PRIMEPOS-2616 14-Dec-2018 JY Added
                    " , " + clsPOSDBConstants.Users_Fld_HourlyRate + " = '" + this.numHourlyRate.Value + "'" +  //PRIMEPOS-189 02-Aug-2021 JY Added
                    " , " + clsPOSDBConstants.Users_Fld_EmailID + " = '" + this.txtEmailID.Text.Trim().Replace("'","''") + "'" +  //PRIMEPOS-2989 11-Aug-2021 JY Added
                    " , " + clsPOSDBConstants.Users_Fld_GroupID + " = " + (cmbUserGroup.Text.Trim() != "" ? Configuration.convertNullToInt(cmbUserGroup.Value) : 0) +   //PRIMEPOS-2780 27-Sep-2021 JY Added
                    " , " + clsPOSDBConstants.Users_Fld_LanID + " = '" + this.txtLanID.Text.Trim().Replace("'", "''") + "'" +  //PRIMEPOS-3484
                    " WHERE " +
                    clsPOSDBConstants.Users_Fld_UserID + " = '" + txtUserID.Text.Trim().Replace("'", "''") + "'";

                conn.ConnectionString = Configuration.ConnectionString;

                conn.Open();
                tr = conn.BeginTransaction();
                byte[] img = picArray;
                cmd.Parameters.Add(new SqlParameter("@IMG", img));
                cmd.CommandText = sSQL;
                cmd.Transaction = tr;
                cmd.Connection = conn;
                cmd.ExecuteNonQuery();

                for (int i = 0; i < this.trvPermissions.Nodes.Count; i++)
                {
                    Infragistics.Win.UltraWinTree.UltraTreeNode oNode = this.trvPermissions.Nodes[i];
                    #region PRIMEPOS-2961 10-May-2021 JY Added
                    sSQL = "SELECT UserID FROM Util_UserRights WHERE USERID = '" + this.txtUserID.Text.Trim().Replace("'", "''") +
                        "' AND ModuleID = " + Configuration.convertNullToInt(oNode.Tag).ToString() +
                        " AND ISNULL(ScreenID,0) = 0 AND ISNULL(PermissionID,0) = 0";

                    DataTable dt = DataHelper.ExecuteDataTable(tr, CommandType.Text, sSQL);
                    if (dt != null && dt.Rows.Count == 0)
                    {
                        sSQL = "INSERT INTO Util_UserRights (UserID,ModuleID,ScreenID,PermissionID,isAllowed) " +
                            "VALUES ('" + this.txtUserID.Text.Trim().Replace("'", "''") + "'," + Configuration.convertNullToInt(oNode.Tag).ToString() + ",NULL,NULL," + Configuration.convertBoolToInt(oNode.CheckedState == CheckState.Checked).ToString() + ")";
                    }
                    #endregion
                    else
                    {
                        sSQL = "UPDATE Util_UserRights SET isAllowed = " + Configuration.convertBoolToInt(oNode.CheckedState == CheckState.Checked).ToString() +
                            " WHERE USERID = '" + this.txtUserID.Text.Trim().Replace("'", "''") +
                            "' AND ModuleID = " + Configuration.convertNullToInt(oNode.Tag).ToString() +
                            " AND ISNULL(ScreenID,0) = 0 AND ISNULL(PermissionID,0) = 0";
                    }
                    cmd.CommandText = sSQL;
                    cmd.ExecuteNonQuery();
                    for (int j = 0; j < oNode.Nodes.Count; j++)
                    {
                        Infragistics.Win.UltraWinTree.UltraTreeNode oNode1 = oNode.Nodes[j];
                        #region PRIMEPOS-2961 10-May-2021 JY Added
                        sSQL = "SELECT UserID FROM Util_UserRights WHERE USERID = '" + this.txtUserID.Text.Trim().Replace("'", "''") +
                            "' AND ModuleID = " + Configuration.convertNullToInt(oNode.Tag).ToString() +
                            " AND ScreenID =" + Configuration.convertNullToInt(oNode1.Tag).ToString() +
                            " AND ISNULL(PermissionID,0) = 0";

                        dt = DataHelper.ExecuteDataTable(tr, CommandType.Text, sSQL);
                        if (dt != null && dt.Rows.Count == 0)
                        {
                            sSQL = "INSERT INTO Util_UserRights (UserID,ModuleID,ScreenID,PermissionID,isAllowed) " +
                                "VALUES ('" + this.txtUserID.Text.Trim().Replace("'", "''") + "'," +
                                Configuration.convertNullToInt(oNode.Tag).ToString() + "," +
                                Configuration.convertNullToInt(oNode1.Tag).ToString() +
                                ",NULL," + Configuration.convertBoolToInt(oNode1.CheckedState == CheckState.Checked).ToString() + ")";
                        }
                        #endregion
                        else
                        {
                            sSQL = "UPDATE Util_UserRights SET isAllowed = " + Configuration.convertBoolToInt(oNode1.CheckedState == CheckState.Checked).ToString() +
                                " WHERE USERID = '" + this.txtUserID.Text.Trim().Replace("'", "''") +
                                "' AND ModuleID = " + Configuration.convertNullToInt(oNode.Tag).ToString() +
                                " AND ScreenID =" + Configuration.convertNullToInt(oNode1.Tag).ToString() +
                                " AND ISNULL(PermissionID,0) = 0";
                        }
                        cmd.CommandText = sSQL;
                        cmd.ExecuteNonQuery();
                        for (int k = 0; k < oNode1.Nodes.Count; k++)
                        {
                            Infragistics.Win.UltraWinTree.UltraTreeNode oNode2 = oNode1.Nodes[k];
                            #region PRIMEPOS-2961 10-May-2021 JY Added
                            sSQL = "SELECT UserID FROM Util_UserRights WHERE USERID = '" + this.txtUserID.Text.Trim().Replace("'", "''") +
                                    "' AND ModuleID = " + Configuration.convertNullToInt(oNode.Tag).ToString() +
                                    " and ScreenID = " + Configuration.convertNullToInt(oNode1.Tag).ToString() +
                                    " AND PermissionID = " + Configuration.convertNullToInt(oNode2.Tag).ToString();

                            dt = DataHelper.ExecuteDataTable(tr, CommandType.Text, sSQL);
                            if (dt != null && dt.Rows.Count == 0)
                            {
                                sSQL = "INSERT INTO Util_UserRights (UserID,ModuleID,ScreenID,PermissionID,isAllowed) " +
                                    "VALUES ('" + this.txtUserID.Text.Trim().Replace("'", "''") + "'," +
                                    Configuration.convertNullToInt(oNode.Tag).ToString() + "," +
                                    Configuration.convertNullToInt(oNode1.Tag).ToString() + "," +
                                    Configuration.convertNullToInt(oNode2.Tag).ToString() + "," +
                                    Configuration.convertBoolToInt(oNode.CheckedState == CheckState.Checked).ToString() + ")";
                            }
                            #endregion
                            else
                            {
                                sSQL = "UPDATE Util_UserRights SET isAllowed =  " + Configuration.convertBoolToInt(oNode2.CheckedState == CheckState.Checked).ToString() +
                                    " WHERE USERID = '" + this.txtUserID.Text.Trim().Replace("'", "''") +
                                    "' AND ModuleID = " + Configuration.convertNullToInt(oNode.Tag).ToString() +
                                    " and ScreenID = " + Configuration.convertNullToInt(oNode1.Tag).ToString() +
                                    " AND PermissionID = " + Configuration.convertNullToInt(oNode2.Tag).ToString();
                            }
                            cmd.CommandText = sSQL;
                            cmd.ExecuteNonQuery();
                        }
                    }
                }

                tr.Commit();
                conn.Close();
                mSaveMode = SaveModeENUM.Modify;
                return true;
            }
            catch (System.Data.SqlClient.SqlException exp)
            {
                if (exp.Number == 2627)
                    clsUIHelper.ShowErrorMsg(" User id already exist");
                else
                    clsUIHelper.ShowErrorMsg(exp.Message);

                tr.Rollback();
                conn.Close();
                return false;
            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
                tr.Rollback();
                conn.Close();
                return false;
            }
        }

        private void Display(string pUserId, bool bLoadOlnyRights)
        {
            SqlCommand cmd1 = new SqlCommand();
            IDbCommand cmd = DataFactory.CreateCommand();
            IDataReader reader;
            string sSQL = "";

            IDbConnection conn = DataFactory.CreateConnection();
            conn.ConnectionString = Configuration.ConnectionString;
            conn.Open();

            SqlConnection conn1 = new SqlConnection();

            conn1.ConnectionString = Configuration.ConnectionString;

            conn1.Open();

            try
            {
                //cmd.Parameters.AddWithValue("@EntryID", Convert.ToInt32(textBox1.Text));
                sSQL = String.Concat("SELECT * FROM ", clsPOSDBConstants.Users_tbl, " WHERE ", clsPOSDBConstants.Users_Fld_UserID, " = '", pUserId, "'");

                cmd.CommandType = CommandType.Text;
                cmd.CommandText = sSQL;
                cmd.Connection = conn;
                cmd1.CommandType = CommandType.Text;
                cmd1.CommandText = sSQL;
                cmd1.Connection = conn1;
                SqlDataAdapter da = new SqlDataAdapter(cmd1);
                reader = cmd.ExecuteReader();
                DataSet ds = new DataSet();
                da.Fill(ds, clsPOSDBConstants.Users_tbl);
                int count = ds.Tables[clsPOSDBConstants.Users_tbl].Rows.Count;

                reader.Read();

                if (bLoadOlnyRights == false)
                {
                    txtUserID.Text = reader.GetString(reader.GetOrdinal(clsPOSDBConstants.Users_Fld_UserID)).Trim();
                    txtPassword.Text = EncryptString.Decrypt(reader.GetString(reader.GetOrdinal(clsPOSDBConstants.Users_Fld_Password))).Trim();
                    numCachDrawerNo.Value = reader.GetInt32(reader.GetOrdinal(clsPOSDBConstants.Users_Fld_DrawNo));
                    numSecurityLevel.Value = reader.GetInt32(reader.GetOrdinal(clsPOSDBConstants.Users_Fld_SecurityLevel));
                    chkIsActive.Checked = reader.GetBoolean(reader.GetOrdinal(clsPOSDBConstants.Users_Fld_IsActive));
                    txtLoginReg.Text = reader.GetValue(reader.GetOrdinal(clsPOSDBConstants.Users_Fld_loginRegistration)).ToString().Trim();
                    txtFirstName.Text = reader.GetValue(reader.GetOrdinal(clsPOSDBConstants.Users_Fld_fName)).ToString().Trim();
                    txtLastName.Text = reader.GetValue(reader.GetOrdinal(clsPOSDBConstants.Users_Fld_lName)).ToString().Trim();

                    #region PRIMEPOS-2577 15-Aug-2018 JY Added
                    try
                    {
                        chkChangePasswordAtLogin.Checked = reader.GetBoolean(reader.GetOrdinal(clsPOSDBConstants.Users_Fld_ChangePasswordAtLogin));
                    }
                    catch
                    {
                        chkChangePasswordAtLogin.Checked = false;
                    }
                    #endregion
                    txtWindowsLogin.Text = reader.GetValue(reader.GetOrdinal(clsPOSDBConstants.Users_Fld_WindowsLoginId)).ToString().Trim();    //PRIMEPOS-2616 14-Dec-2018 JY Added

                    try
                    {
                        byte[] picData = reader[clsPOSDBConstants.Users_Fld_UserImage] as byte[] ?? null;
                        if (picData != null)
                        {
                            if (count > 0)
                            {
                                byte[] data = (Byte[])(ds.Tables[clsPOSDBConstants.Users_tbl].Rows[0][clsPOSDBConstants.Users_Fld_UserImage]);

                                Bitmap bitmap = new Bitmap(Width, Height);
                                Graphics graphics = Graphics.FromImage(bitmap);
                                Bitmap myBitmap = new Bitmap(200, 200);


                                Byte[] strBinSign = null;
                                if (ds.Tables[clsPOSDBConstants.Users_tbl].Rows[0][clsPOSDBConstants.Users_Fld_UserImage] != null)
                                {
                                    strBinSign = (byte[])ds.Tables[clsPOSDBConstants.Users_tbl].Rows[0][clsPOSDBConstants.Users_Fld_UserImage];
                                    if (strBinSign != null)
                                    {
                                        MemoryStream ms = new MemoryStream(strBinSign);
                                        myBitmap = new Bitmap(ms);
                                    }
                                }


                                Bitmap SignImage = new Bitmap(myBitmap.Width, myBitmap.Height, PixelFormat.Format32bppArgb);
                                SignImage = (Bitmap)myBitmap.Clone(new Rectangle(0, 0, myBitmap.Width, myBitmap.Height), PixelFormat.Format32bppArgb);
                                PictUserImg.Image = SignImage;
                            }
                        }
                    }
                    catch (Exception)
                    {

                    }
                    //MemoryStream ms = new MemoryStream(data);
                    ////ms.Position = 0;
                    ////ms.Seek(0, SeekOrigin.Begin);
                    //Bitmap myBitmap = new Bitmap(ms);

                    //Bitmap b = new Bitmap(myBitmap.Width, myBitmap.Height, PixelFormat.Format32bppArgb);
                    ////Bitmap b = new Bitmap(myBitmap.Width, myBitmap.Height, PixelFormat.Format24bppRgb);
                    //b = (Bitmap)myBitmap.Clone(new Rectangle(0, 0, myBitmap.Width, myBitmap.Height), PixelFormat.Format32bppArgb);
                    //b = (Bitmap)myBitmap.Clone(new Rectangle(0, 0, myBitmap.Width, myBitmap.Height), PixelFormat.Format24bppRgb);
                    //pictureBox1.Image = b;

                    ////picSignature.Image = b;
                    //var data = (Byte[])(ds.Tables[clsPOSDBConstants.Users_tbl].Rows[0][clsPOSDBConstants.Users_Fld_UserImage]);
                    //var stream = new MemoryStream(data);
                    //string error="";
                    //PictUserImg.Image = clsUIHelper.GetSignature("POS", data, "", out error);
                    //PictUserImg.Image = Image.FromStream(stream);
                    //PictUserImg.Image = b;

                    if (!reader.IsDBNull(reader.GetOrdinal(clsPOSDBConstants.Users_Fld_MaxDiscountLimit)))
                    {
                        //this.numMaxPerDisc.Value = Configuration.convertNullToDecimal(reader.GetDecimal(reader.GetOrdinal(clsPOSDBConstants.Users_Fld_MaxDiscountLimit)));    //PRIMEPOS-2979 21-Jun-2021 JY Commented
                        #region PRIMEPOS-2979 21-Jun-2021 JY Added
                        try
                        {
                            if (Configuration.convertNullToDecimal(reader.GetDecimal(reader.GetOrdinal(clsPOSDBConstants.Users_Fld_MaxDiscountLimit))) > Configuration.convertNullToDecimal(this.numMaxPerDisc.MaxValue))
                                this.numMaxPerDisc.Value = Configuration.convertNullToDecimal(this.numMaxPerDisc.MaxValue);
                            else
                                this.numMaxPerDisc.Value = Configuration.convertNullToDecimal(reader.GetDecimal(reader.GetOrdinal(clsPOSDBConstants.Users_Fld_MaxDiscountLimit)));
                        }
                        catch (Exception Ex)
                        {
                        }
                        #endregion
                    }
                    if (!reader.IsDBNull(reader.GetOrdinal(clsPOSDBConstants.Users_Fld_MaxTransactionLimit)))
                    {
                        this.numMaxTransactionLimit.Value = Configuration.convertNullToDecimal(reader.GetDecimal(reader.GetOrdinal(clsPOSDBConstants.Users_Fld_MaxTransactionLimit)));
                    }
                    //Sprint-23 - PRIMEPOS-2303 24-May-2016 JY Added 
                    if (!reader.IsDBNull(reader.GetOrdinal(clsPOSDBConstants.Users_Fld_MaxReturnTransLimit)))
                    {
                        this.numMaxReturnTransLimit.Value = Configuration.convertNullToDecimal(reader.GetDecimal(reader.GetOrdinal(clsPOSDBConstants.Users_Fld_MaxReturnTransLimit)));
                    }
                    //Sprint-25 - PRIMEPOS-2411 20-Apr-2017 JY Added 
                    if (!reader.IsDBNull(reader.GetOrdinal(clsPOSDBConstants.Users_Fld_MaxTenderedAmountLimit)))
                    {
                        this.numMaxTenderedAmount.Value = Configuration.convertNullToDecimal(reader.GetDecimal(reader.GetOrdinal(clsPOSDBConstants.Users_Fld_MaxTenderedAmountLimit)));
                    }
                    //PRIMEPOS-2741 25-Sep-2019 JY Added
                    if (!reader.IsDBNull(reader.GetOrdinal(clsPOSDBConstants.Users_Fld_MaxCashbackLimit)))
                    {
                        this.numMaxCashbackLimit.Value = Configuration.convertNullToDecimal(reader.GetDecimal(reader.GetOrdinal(clsPOSDBConstants.Users_Fld_MaxCashbackLimit)));
                    }
                    //PRIMEPOS-189 02-Aug-2021 JY Added
                    if (!reader.IsDBNull(reader.GetOrdinal(clsPOSDBConstants.Users_Fld_HourlyRate)))
                    {
                        this.numHourlyRate.Value = Configuration.convertNullToDecimal(reader.GetDecimal(reader.GetOrdinal(clsPOSDBConstants.Users_Fld_HourlyRate)));
                    }
                    //PRIMEPOS-2989 11-Aug-2021 JY Added
                    if (!reader.IsDBNull(reader.GetOrdinal(clsPOSDBConstants.Users_Fld_EmailID)))
                    {
                        this.txtEmailID.Text = reader.GetValue(reader.GetOrdinal(clsPOSDBConstants.Users_Fld_EmailID)).ToString().Trim();
                    }
                    #region PRIMEPOS-2780 27-Sep-2021 JY Added
                    if (!reader.IsDBNull(reader.GetOrdinal(clsPOSDBConstants.Users_Fld_GroupID)))
                    {
                        int GroupID = Configuration.convertNullToInt(reader.GetValue(reader.GetOrdinal(clsPOSDBConstants.Users_Fld_GroupID)));
                        if (GroupID > 0)
                        {
                            this.cmbUserGroup.ValueChanged -= new System.EventHandler(this.cmbUserGroup_ValueChanged);
                            this.cmbUserGroup.Value = GroupID;
                            this.cmbUserGroup.ValueChanged += new System.EventHandler(this.cmbUserGroup_ValueChanged);
                        }
                    }
                    #endregion
                    if (!reader.IsDBNull(reader.GetOrdinal(clsPOSDBConstants.Users_Fld_LanID)))  //PRIMEPOS-3484
                    {
                        this.txtLanID.Text = reader.GetValue(reader.GetOrdinal(clsPOSDBConstants.Users_Fld_LanID)).ToString().Trim();
                    }
                }
                #region PRIMEPOS-2780 27-Sep-2021 JY Added
                else
                {
                    POS_Core.BusinessRules.User oUser = new POS_Core.BusinessRules.User();
                    DataTable dt = oUser.GetUserByID(Configuration.convertNullToInt(pUserId));
                    if (dt != null && dt.Rows.Count > 0)
                        pUserId = Configuration.convertNullToString(dt.Rows[0]["UserID"]);
                }
                #endregion
                this.trvPermissions.AfterCheck -= new Infragistics.Win.UltraWinTree.AfterNodeChangedEventHandler(this.trvPermissions_AfterCheck);
                for (int i = 0; i < this.trvPermissions.Nodes.Count; i++)
                {
                    Infragistics.Win.UltraWinTree.UltraTreeNode oNode = this.trvPermissions.Nodes[i];
                    if (UserPriviliges.IsUserHasPriviliges(Configuration.convertNullToInt(oNode.Tag.ToString()), pUserId))
                    {
                        oNode.CheckedState = CheckState.Checked;
                    }
                    else
                    {
                        oNode.CheckedState = CheckState.Unchecked;
                    }
                    for (int j = 0; j < oNode.Nodes.Count; j++)
                    {
                        Infragistics.Win.UltraWinTree.UltraTreeNode oNode1 = oNode.Nodes[j];
                        if (UserPriviliges.IsUserHasPriviliges(Configuration.convertNullToInt(oNode.Tag.ToString()), Configuration.convertNullToInt(oNode1.Tag.ToString()), pUserId))
                        {
                            oNode1.CheckedState = CheckState.Checked;
                        }
                        else
                        {
                            oNode1.CheckedState = CheckState.Unchecked;
                        }
                        for (int k = 0; k < oNode1.Nodes.Count; k++)
                        {
                            Infragistics.Win.UltraWinTree.UltraTreeNode oNode2 = oNode1.Nodes[k];
                            if (UserPriviliges.IsUserHasPriviliges(Configuration.convertNullToInt(oNode.Tag.ToString()), Configuration.convertNullToInt(oNode1.Tag.ToString()), Configuration.convertNullToInt(oNode2.Tag.ToString()), pUserId))
                            {
                                oNode2.CheckedState = CheckState.Checked;
                            }
                            else
                            {
                                oNode2.CheckedState = CheckState.Unchecked;
                            }
                        }
                    }
                }
                txtPassword.Enabled = false;
                if (Configuration.CInfo.UseBiometricDevice.Trim().ToUpper() == "DigitalPersona".ToUpper()) btnEnrollFingerprint.Visible = true; //PRIMEPOS-2576 27-Aug-2018 JY Added
                this.trvPermissions.AfterCheck += new Infragistics.Win.UltraWinTree.AfterNodeChangedEventHandler(this.trvPermissions_AfterCheck);
                conn.Close();

            }

            catch (NullReferenceException)
            {
                conn.Close();
            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
                conn.Close();
            }
        }

        private void Clear()
        {
            txtUserID.Text = "";
            txtPassword.Text = "";
            numCachDrawerNo.Value = 0;
            numSecurityLevel.Value = 0;
            chkIsActive.Checked = true;
            txtLoginReg.Text = "";
            txtFirstName.Text = "";
            txtLastName.Text = "";
            chkChangePasswordAtLogin.Checked = false;   //PRIMEPOS-2577 15-Aug-2018 JY Added
        }

        private void btnOk_Click(object sender, System.EventArgs e)
        {

            if (Save() == true)
            {
                if (Configuration.UserName == txtUserID.Text)
                {
                    Configuration.UserMaxTransactionLimit = Configuration.convertNullToDecimal(this.numMaxTransactionLimit.Value);
                    Configuration.UserMaxDiscountLimit = Configuration.convertNullToDecimal(this.numMaxPerDisc.Value);
                    Configuration.UserMaxReturnTransLimit = Configuration.convertNullToDecimal(this.numMaxReturnTransLimit.Value);  //Sprint-23 - PRIMEPOS-2303 24-May-2016 JY Added 
                    Configuration.UserMaxTenderedAmountLimit = Configuration.convertNullToDecimal(this.numMaxTenderedAmount.Value);  //Sprint-25 - PRIMEPOS-2411 20-Apr-2017 JY Added 
                    Configuration.UserMaxCashbackLimit = Configuration.convertNullToDecimal(this.numMaxCashbackLimit.Value); //PRIMEPOS-2741 25-Sep-2019 JY Added
                    Configuration.HourlyRate = Configuration.convertNullToDecimal(this.numHourlyRate.Value);    //PRIMEPOS-189 02-Aug-2021 JY Added
                }
                if (Configuration.UserName != null)
                    ErrorHandler.SaveLog((int)LogENUM.UserRights_Change, Configuration.UserName, "Success", "User rights changed successfully");
                IsCanceled = false;
                this.Close();
            }
            else
            {
                if (Configuration.UserName != null)
                    ErrorHandler.SaveLog((int)LogENUM.UserRights_Change, Configuration.UserName, "Fail", "User rights change not saved");
                IsCanceled = true;
            }

        }

        private void frmUserInformation_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            try
            {
                if (e.KeyData == System.Windows.Forms.Keys.Enter)
                {
                    if (this.ActiveControl != txtPassword)
                        this.SelectNextControl(this.ActiveControl, true, true, true, true);
                }
                // Added By Shitaljit(QuicSolv) 0n 10 oct 2011 
                if (e.KeyData == Keys.F6)
                    btnUserNotes_Click(null, null);

            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }

        private void numSecurityLevel_ValueChanged(object sender, System.EventArgs e)
        {

        }

        private void numSecurityLevel_Enter(object sender, System.EventArgs e)
        {
            numSecurityLevel.SelectAll();
        }

        private void chkAllowAll_CheckedChanged(object sender, System.EventArgs e)
        {
            checkAll(false);    //PRIMEPOS-2995 25-Aug-2021 JY Added
            checkAll(true);
        }

        private void checkAll(bool check)
        {
            foreach (Infragistics.Win.UltraWinTree.UltraTreeNode oCtrl in this.trvPermissions.Nodes)
            {
                if (check == true)
                    oCtrl.CheckedState = CheckState.Checked;
                else
                    oCtrl.CheckedState = CheckState.Unchecked;
            }
        }

        private void chkDisAllowAll_CheckedChanged(object sender, System.EventArgs e)
        {
            checkAll(false);
        }

        private void allowEditPermissions(bool allow)
        {
            this.btnAllowAll.Enabled = allow;
            this.btnDisallowAll.Enabled = allow;
            foreach (Control oCtrl in gbRights.Controls)
            {
                if (oCtrl.GetType() == typeof(System.Windows.Forms.CheckBox) && oCtrl.Name != "btnAllowAll" && oCtrl.Name != "btnDisallowAll")
                {
                    System.Windows.Forms.CheckBox chk = (System.Windows.Forms.CheckBox)oCtrl;
                    chk.Enabled = allow;
                }
            }
        }

        private void changePermissionState(bool allow)
        {
        }

        private void BuildTreeView()
        {
            DataSet oDS = new DataSet();
            sm_Modules o = new sm_Modules();
            oDS.Tables.Add(o.Modules);

            sm_Screens oScreens = new sm_Screens();
            oDS.Tables.Add(oScreens.Screens);

            sm_Permissions oPerm = new sm_Permissions();
            oDS.Tables.Add(oPerm.Permissions);

            DataRelation drel = new DataRelation("rl_mod_scr", oDS.Tables["Modules"].Columns["ModuleID"], oDS.Tables["Screens"].Columns["ModuleID"]);
            oDS.Relations.Add(drel);

            DataRelation drelPerm = new DataRelation("rl_scr_perm", oDS.Tables["Screens"].Columns["ScID"], oDS.Tables["Permissions"].Columns["ScreenID"]);
            oDS.Relations.Add(drelPerm);

            Infragistics.Win.UltraWinTree.UltraTreeNode oACNode;

            foreach (DataRow dr in oDS.Tables["Modules"].Rows)
            {
                Infragistics.Win.UltraWinTree.UltraTreeNode oNode;
                oNode = this.trvPermissions.Nodes.Add(dr["ModuleID"].ToString(), dr["ModuleName"].ToString());
                oNode.Tag = dr["ModuleID"].ToString();
                foreach (DataRow drScreens in dr.GetChildRows(drel))
                {
                    Infragistics.Win.UltraWinTree.UltraTreeNode oScNode = oNode.Nodes.Add("sc-" + drScreens["ScID"].ToString(), drScreens["ScName"].ToString());
                    oScNode.Tag = drScreens["ScID"].ToString();
                    if (bool.Parse(drScreens["EntryScreen"].ToString()) == true)
                    {
                        oACNode = oScNode.Nodes.Add(oScNode.Key + "-999", "Add");
                        oACNode.Tag = -999;
                        oACNode = oScNode.Nodes.Add(oScNode.Key + "-998", "Change");
                        oACNode.Tag = -998;
                    }
                    foreach (DataRow drPerm in drScreens.GetChildRows(drelPerm))
                    {
                        Infragistics.Win.UltraWinTree.UltraTreeNode oPermNode = oScNode.Nodes.Add("pm-" + drPerm["pmID"].ToString(), drPerm["PmName"].ToString());
                        oPermNode.Tag = drPerm["pmID"].ToString();
                        if (bool.Parse(drPerm["EntryScreen"].ToString()) == true)
                        {
                            oACNode = oPermNode.Nodes.Add(oPermNode.Key + "-999", "Add");
                            oACNode.Tag = -999;
                            oPermNode.Nodes.Add(oPermNode.Key + "-998", "Change");
                            oACNode.Tag = -998;
                        }
                    }
                }
            }
        }

        private void trvPermissions_AfterCheck(object sender, Infragistics.Win.UltraWinTree.NodeEventArgs e)
        {
            this.trvPermissions.AfterCheck -= new Infragistics.Win.UltraWinTree.AfterNodeChangedEventHandler(this.trvPermissions_AfterCheck);
            Infragistics.Win.UltraWinTree.UltraTreeNode oNode = null;
            if (e.TreeNode.CheckedState == CheckState.Checked)
            {
                oNode = e.TreeNode.Parent;
                while (oNode != null)
                {
                    oNode.CheckedState = e.TreeNode.CheckedState;
                    oNode = oNode.Parent;
                }
                oNode = null;
            }
            this.trvPermissions.AfterCheck += new Infragistics.Win.UltraWinTree.AfterNodeChangedEventHandler(this.trvPermissions_AfterCheck);

            foreach (Infragistics.Win.UltraWinTree.UltraTreeNode oNodeC in e.TreeNode.Nodes)
            {
                oNodeC.CheckedState = e.TreeNode.CheckedState;
            }
        }

        internal protected void SetAllPermissionsForLoggedInUser()
        {
            string strSQL = "select count (*) from util_userrights where userid='" + Configuration.UserName + "'";
            object obj = DataHelper.ExecuteScalar(Configuration.ConnectionString, CommandType.Text, strSQL);
            if (obj != null)
            {
                if (Configuration.convertNullToInt(obj.ToString()) >= 1)
                {
                    return;
                }
            }
            this.Edit(Configuration.UserName);
            frmUserManagement_Load(this, new System.EventArgs());
            this.checkAll(true);
            this.btnOk_Click(this.btnOk, new System.EventArgs());
        }

        internal protected void CreateUserWithAllPermissions(string userID, string password)
        {
            SetNew();
            this.txtUserID.Text = userID;
            this.txtPassword.Text = password;
            frmUserManagement_Load(this, new System.EventArgs());
            this.checkAll(true);
            this.btnOk_Click(this.btnOk, new System.EventArgs());
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

        private bool IsValidPassword(string pUserName)
        {
            bool result = false;
            string sPassword = this.txtPassword.Text;
            string sSQL = string.Empty;
            DataSet oDS;
            POS_Core.BusinessRules.Search oSearch = new POS_Core.BusinessRules.Search();
            try
            {
                sSQL = String.Concat("SELECT "
                                                            , clsPOSDBConstants.Users_Fld_Password
                                                            , ",", clsPOSDBConstants.Users_Fld_LastLoginIP
                                                            , ",", clsPOSDBConstants.Users_Fld_IsLocked
                                                            , ",", clsPOSDBConstants.Users_Fld_UserID
                                                            , ",", clsPOSDBConstants.Users_Fld_ModifiedOn
                                                            , ",", clsPOSDBConstants.Users_Fld_PasswordChangedOn
                                                            , ",", clsPOSDBConstants.Users_Fld_Password1
                                                            , ",", clsPOSDBConstants.Users_Fld_Password2
                                                            , ",", clsPOSDBConstants.Users_Fld_Password3
                                                        , " FROM "
                                                            , clsPOSDBConstants.Users_tbl
                                                        , "  WHERE "
                                                            , clsPOSDBConstants.Users_Fld_UserID, " = '", pUserName.Replace("'", "''"), "'", " AND IsActive = 1");
                oDS = oSearch.SearchData(sSQL);
                if (oDS != null && oDS.Tables[0].Rows.Count > 0)
                {
                    string sPassword1 = oDS.Tables[0].Rows[0][clsPOSDBConstants.Users_Fld_Password1].ToString();
                    string sPassword2 = oDS.Tables[0].Rows[0][clsPOSDBConstants.Users_Fld_Password2].ToString();
                    string sPassword3 = oDS.Tables[0].Rows[0][clsPOSDBConstants.Users_Fld_Password3].ToString();
                    if (sPassword != sPassword1 && sPassword != sPassword2 && sPassword != sPassword3)
                    {
                        result = true;
                    }
                }
                else
                {
                    result = true;
                }
            }
            catch (Exception exp)
            {
            }
            return result;
        }

        private void btnUserNotes_Click(object sender, EventArgs e)
        {
            frmCustomerNotes ofrmCustNotes = new frmCustomerNotes(Configuration.UserName, clsPOSDBConstants.Users_tbl, "U");
            ofrmCustNotes.ShowDialog();

        }

        private void cmbUserGroup_ValueChanged(object sender, EventArgs e)
        {
            string sSelectedGroup = string.Empty;
            sSelectedGroup = Configuration.convertNullToString(cmbUserGroup.SelectedItem.DataValue);
            if (string.IsNullOrEmpty(sSelectedGroup) == false && sSelectedGroup != "0") //PRIMEPOS-3038 14-Dec-2021 JY Modified
            {
                if (Resources.Message.Display("This action will change current user rights to selected group defaulf rights.\nAre you sure, you want to change the user group?", "User Group", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1) == System.Windows.Forms.DialogResult.Yes)
                {
                    nUserGroup = cmbUserGroup.SelectedIndex;    //PRIMEPOS-2577 14-Aug-2018 JY Added
                    Display(sSelectedGroup, true);
                }
                else    //PRIMEPOS-2577 14-Aug-2018 JY Added logic to reset user group when we select "No"
                {
                    this.cmbUserGroup.ValueChanged -= new System.EventHandler(this.cmbUserGroup_ValueChanged);
                    cmbUserGroup.SelectedIndex = nUserGroup;
                    this.cmbUserGroup.ValueChanged += new System.EventHandler(this.cmbUserGroup_ValueChanged);
                }
            }
        }

        private void btnBrowseImage_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog fileDilog = new OpenFileDialog();
                fileDilog.Filter = "JPEG Files (*.jpeg)|*.jpeg|PNG Files (*.png)|*.png|JPG Files (*.jpg)|*.jpg|GIF Files (*.gif)|*.gif";
                DialogResult result = fileDilog.ShowDialog();
                if (result == DialogResult.OK)
                {
                    string path = fileDilog.FileName;

                    FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read);
                    picArray = new byte[fs.Length];
                    fs.Read(picArray, 0, Convert.ToInt32(fs.Length));
                    fs.Close();
                    MemoryStream ms = new MemoryStream(picArray);
                    PictUserImg.Image = Image.FromStream(ms);
                }
                else
                {
                    MemoryStream ms = new MemoryStream();
                    PictUserImg.Image.Save(ms, System.Drawing.Imaging.ImageFormat.Gif);
                    picArray = ms.ToArray();
                }
            }
            catch { }
        }

        #region PRIMEPOS-2576 27-Aug-2018 JY Added
        private void btnEnrollFingerprint_Click(object sender, EventArgs e)
        {
            frmEnrollFingerPrint frmCFP = new frmEnrollFingerPrint(mUserId);
            frmCFP.ShowDialog();
        }
        #endregion

        #region PRIMEPOS-2989 25-Aug-2021 JY Added
        private void txtEmailID_Leave(object sender, EventArgs e)
        {
            try
            {
                if (txtEmailID.Text.Trim() == "") return;

                string strSQL = string.Empty;
                if (mSaveMode == SaveModeENUM.Create)
                {
                    strSQL = "SELECT UserID FROM Users WITH (NOLOCK) WHERE EmailID = '" + txtEmailID.Text.Trim().Replace("'", "'") + "'";
                }
                else if (mSaveMode == SaveModeENUM.Modify)
                {
                    strSQL = "SELECT UserID FROM Users WITH (NOLOCK) WHERE UserID <> '" + mUserId + "' AND EmailID = '" + txtEmailID.Text.Trim().Replace("'", "'") + "'";
                }
                DataSet ds = DataHelper.ExecuteDataset(Configuration.ConnectionString, CommandType.Text, strSQL);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    string assignedUser = ds.Tables[0].Rows[0][0].ToSafeString();
                    Resources.Message.Display($"{this.txtEmailID.Text} EmailID is already assigned to user {assignedUser}, cannot assign to this user.", "Duplicate emailID", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.txtEmailID.Text = "";
                    this.txtEmailID.Focus();
                }
            }
            catch { }
        }
        #endregion

        #region PRIMEPOS-3419
        private bool EditUserInventoryMenu()
        {
            IDbTransaction tr = null;
            IDbConnection conn = DataFactory.CreateConnection();
            try
            {
                IDbCommand cmd = DataFactory.CreateCommand();

                string sSQL = "UPDATE " + clsPOSDBConstants.Users_tbl + " SET " + clsPOSDBConstants.Users_Fld_AllowInventoryMenu + " = " +
                    (UserPriviliges.IsUserHasPriviliges(UserPriviliges.Modules.InventoryMgmt.ID, UserPriviliges.Screens.AllowInventoryMenu.ID, txtUserID.Text.Trim()) == true? 1:0) +
                    " WHERE " + clsPOSDBConstants.Users_Fld_UserID + " = '" + txtUserID.Text.Trim().Replace("'", "''") + "'";

                conn.ConnectionString = Configuration.ConnectionString;
                conn.Open();
                tr = conn.BeginTransaction();
                cmd.CommandText = sSQL;
                cmd.Transaction = tr;
                cmd.Connection = conn;
                cmd.ExecuteNonQuery();
                tr.Commit();
                conn.Close();
                return true;
            }
            catch(Exception ex)
            {
                clsUIHelper.ShowErrorMsg(ex.Message);
                tr.Rollback();
                conn.Close();
                return false;
            }
        }
        #endregion
        #region PRIMEPOS-3484
        private void txtLanID_Leave(object sender, EventArgs e)
        {
            try
            {
                if (txtLanID.Text.Trim() == "") return;

                string strSQL = string.Empty;
                if (mSaveMode == SaveModeENUM.Create)
                {
                    strSQL = "SELECT UserID FROM Users WITH (NOLOCK) WHERE LanID = '" + txtLanID.Text.Trim().Replace("'", "'") + "'";
                }
                else if (mSaveMode == SaveModeENUM.Modify)
                {
                    strSQL = "SELECT UserID FROM Users WITH (NOLOCK) WHERE UserID <> '" + mUserId + "' AND LanID = '" + txtLanID.Text.Trim().Replace("'", "'") + "'";
                }
                DataSet ds = DataHelper.ExecuteDataset(Configuration.ConnectionString, CommandType.Text, strSQL);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    string assignedUser = ds.Tables[0].Rows[0][0].ToSafeString();
                    Resources.Message.Display($"{this.txtLanID.Text} LanID is already assigned to user {assignedUser}, cannot assign to this user", "Duplicate LanID", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.txtLanID.Text = "";
                    this.txtLanID.Focus();
                }
            }
            catch(Exception ex) 
            {
                clsUIHelper.ShowErrorMsg(ex.Message);
            }
        }
        #endregion
    }
}