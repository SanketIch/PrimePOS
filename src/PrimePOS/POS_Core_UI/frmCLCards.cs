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
using POS_Core.Resources;
using POS_Core.LabelHandler;
using POS_Core.LabelHandler.RxLabel;
//using POS_Core.DataAccess;

namespace POS_Core_UI
{
    /// <summary>
    /// Summary description for frmCLCards.
    /// </summary>
    public class frmCLCards : System.Windows.Forms.Form
    {
        public bool IsCanceled = true;
        private CLCardsData oCLCardsData = new CLCardsData();
        private CLCardsRow oCLCardsRow;
        private CLCards oBRCLCards = new CLCards();
        private Infragistics.Win.Misc.UltraLabel ultraLabel20;
        private Infragistics.Win.Misc.UltraLabel ultraLabel11;
        private Infragistics.Win.Misc.UltraLabel ultraLabel14;
        private Infragistics.Win.Misc.UltraLabel lblTransactionType;
        private System.Windows.Forms.CheckBox chkIsPrepetual;
        private Infragistics.Win.Misc.UltraButton btnClose;
        private Infragistics.Win.Misc.UltraButton btnSave;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ToolTip toolTip1;
        private Infragistics.Win.Misc.UltraLabel ultraLabel7;
        private UltraTextEditor txtDescription;
        public UltraNumericEditor txtCLCardID;
        private Infragistics.Win.Misc.UltraLabel ultraLabel1;
        private UltraNumericEditor txtExpiryDays;
        private Infragistics.Win.UltraWinSchedule.UltraCalendarCombo dtpRegisterDate;
        private Infragistics.Win.Misc.UltraLabel lblCustomerName;
        private UltraTextEditor txtCustomer;
        private Infragistics.Win.Misc.UltraLabel ultraLabel2;
        private Infragistics.Win.Misc.UltraButton btnPrintCard;
        private Infragistics.Win.Misc.UltraButton btnChangeCardPoints;
        private UltraNumericEditor numCurrentPoints;
        private Infragistics.Win.Misc.UltraLabel ultraLabel3;
        private Infragistics.Win.Misc.UltraButton btnPrintCoupons;
        private Infragistics.Win.Misc.UltraLabel ultraLabel4;
        private Infragistics.Win.Misc.UltraButton btnResetCpnGenCycle;
        private System.ComponentModel.IContainer components;

        public void Initialize(string customerCode, string sCardNo)
        {
            SetNew();

            if (customerCode.Length > 0)
            {
                Customer oCustomer = new Customer();
                CustomerData oCustomerData;

                oCustomerData = oCustomer.Populate(customerCode, true);

                this.txtCustomer.Text = oCustomerData.Customer[0].AccountNumber.ToString();
                this.lblCustomerName.Text = oCustomerData.Customer[0].CustomerFullName;
                this.txtCustomer.Tag = oCustomerData.Customer[0].CustomerId;
                oCLCardsRow.CustomerID = oCustomerData.Customer[0].CustomerId;
                this.txtCustomer.Enabled = false;
                this.txtCustomer.ButtonsRight[0].Enabled = false;
                try
                {
                    this.txtCLCardID.Value = sCardNo;//Addded by shitaljit for JIRA-357 on jan 15 2013
                }
                catch
                {

                }
            }
            //Added By Dharmendra(SRT) which will be required when processing
        }

        public frmCLCards()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();
            try
            {
                SetNew();
            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
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
            this.components = new System.ComponentModel.Container();
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmCLCards));
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance7 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance8 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance9 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance10 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinEditors.EditorButton editorButton1 = new Infragistics.Win.UltraWinEditors.EditorButton("S");
            Infragistics.Win.Appearance appearance11 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance12 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance13 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance14 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance15 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance16 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance17 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinSchedule.CalendarCombo.DateButton dateButton1 = new Infragistics.Win.UltraWinSchedule.CalendarCombo.DateButton();
            Infragistics.Win.Appearance appearance18 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance19 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance20 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance21 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance22 = new Infragistics.Win.Appearance();
            this.chkIsPrepetual = new System.Windows.Forms.CheckBox();
            this.ultraLabel20 = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabel11 = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabel14 = new Infragistics.Win.Misc.UltraLabel();
            this.btnClose = new Infragistics.Win.Misc.UltraButton();
            this.btnSave = new Infragistics.Win.Misc.UltraButton();
            this.lblTransactionType = new Infragistics.Win.Misc.UltraLabel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.numCurrentPoints = new Infragistics.Win.UltraWinEditors.UltraNumericEditor();
            this.ultraLabel3 = new Infragistics.Win.Misc.UltraLabel();
            this.lblCustomerName = new Infragistics.Win.Misc.UltraLabel();
            this.txtCustomer = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.ultraLabel2 = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabel1 = new Infragistics.Win.Misc.UltraLabel();
            this.txtExpiryDays = new Infragistics.Win.UltraWinEditors.UltraNumericEditor();
            this.txtCLCardID = new Infragistics.Win.UltraWinEditors.UltraNumericEditor();
            this.ultraLabel7 = new Infragistics.Win.Misc.UltraLabel();
            this.dtpRegisterDate = new Infragistics.Win.UltraWinSchedule.UltraCalendarCombo();
            this.txtDescription = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.ultraLabel4 = new Infragistics.Win.Misc.UltraLabel();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnResetCpnGenCycle = new Infragistics.Win.Misc.UltraButton();
            this.btnPrintCoupons = new Infragistics.Win.Misc.UltraButton();
            this.btnChangeCardPoints = new Infragistics.Win.Misc.UltraButton();
            this.btnPrintCard = new Infragistics.Win.Misc.UltraButton();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numCurrentPoints)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCustomer)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtExpiryDays)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCLCardID)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtpRegisterDate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDescription)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // chkIsPrepetual
            // 
            this.chkIsPrepetual.AutoSize = true;
            this.chkIsPrepetual.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkIsPrepetual.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chkIsPrepetual.ForeColor = System.Drawing.Color.White;
            this.chkIsPrepetual.Location = new System.Drawing.Point(130, 88);
            this.chkIsPrepetual.Name = "chkIsPrepetual";
            this.chkIsPrepetual.Size = new System.Drawing.Size(12, 11);
            this.chkIsPrepetual.TabIndex = 2;
            this.chkIsPrepetual.CheckedChanged += new System.EventHandler(this.chkIsPrepetual_CheckedChanged);
            // 
            // ultraLabel20
            // 
            appearance1.FontData.BoldAsString = "False";
            appearance1.ForeColor = System.Drawing.Color.White;
            this.ultraLabel20.Appearance = appearance1;
            this.ultraLabel20.Location = new System.Drawing.Point(327, 83);
            this.ultraLabel20.Name = "ultraLabel20";
            this.ultraLabel20.Size = new System.Drawing.Size(124, 21);
            this.ultraLabel20.TabIndex = 13;
            this.ultraLabel20.Text = "Card Expiry Days";
            // 
            // ultraLabel11
            // 
            appearance2.ForeColor = System.Drawing.Color.White;
            this.ultraLabel11.Appearance = appearance2;
            this.ultraLabel11.Location = new System.Drawing.Point(12, 53);
            this.ultraLabel11.Name = "ultraLabel11";
            this.ultraLabel11.Size = new System.Drawing.Size(117, 21);
            this.ultraLabel11.TabIndex = 8;
            this.ultraLabel11.Text = "Description";
            // 
            // ultraLabel14
            // 
            appearance3.ForeColor = System.Drawing.Color.White;
            this.ultraLabel14.Appearance = appearance3;
            this.ultraLabel14.Location = new System.Drawing.Point(12, 23);
            this.ultraLabel14.Name = "ultraLabel14";
            this.ultraLabel14.Size = new System.Drawing.Size(109, 21);
            this.ultraLabel14.TabIndex = 0;
            this.ultraLabel14.Text = "Card Number";
            // 
            // btnClose
            // 
            appearance4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(62)))), ((int)(((byte)(176)))));
            appearance4.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance4.BorderAlpha = Infragistics.Win.Alpha.Opaque;
            appearance4.BorderColor = System.Drawing.Color.Black;
            appearance4.BorderColor3DBase = System.Drawing.Color.Black;
            appearance4.FontData.BoldAsString = "True";
            appearance4.ForeColor = System.Drawing.Color.White;
            appearance4.Image = ((object)(resources.GetObject("appearance4.Image")));
            this.btnClose.Appearance = appearance4;
            this.btnClose.ButtonStyle = Infragistics.Win.UIElementButtonStyle.OfficeXPToolbarButton;
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(484, 74);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(149, 50);
            this.btnClose.TabIndex = 1;
            this.btnClose.Text = "&Cancel";
            this.btnClose.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.btnClose.UseHotTracking = Infragistics.Win.DefaultableBoolean.True;
            this.btnClose.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnSave
            // 
            appearance5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(62)))), ((int)(((byte)(176)))));
            appearance5.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance5.FontData.BoldAsString = "True";
            appearance5.ForeColor = System.Drawing.Color.White;
            appearance5.Image = ((object)(resources.GetObject("appearance5.Image")));
            this.btnSave.Appearance = appearance5;
            this.btnSave.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Flat;
            this.btnSave.Location = new System.Drawing.Point(326, 74);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(149, 50);
            this.btnSave.TabIndex = 0;
            this.btnSave.Text = "&Ok";
            this.btnSave.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.btnSave.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // lblTransactionType
            // 
            appearance6.ForeColor = System.Drawing.Color.White;
            appearance6.ForeColorDisabled = System.Drawing.Color.White;
            appearance6.TextHAlignAsString = "Center";
            this.lblTransactionType.Appearance = appearance6;
            this.lblTransactionType.BackColorInternal = System.Drawing.Color.Transparent;
            this.lblTransactionType.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTransactionType.Location = new System.Drawing.Point(16, 4);
            this.lblTransactionType.Name = "lblTransactionType";
            this.lblTransactionType.Size = new System.Drawing.Size(580, 50);
            this.lblTransactionType.TabIndex = 0;
            this.lblTransactionType.Tag = "Header";
            this.lblTransactionType.Text = "Customer Loyalty Card Information";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.numCurrentPoints);
            this.groupBox1.Controls.Add(this.ultraLabel3);
            this.groupBox1.Controls.Add(this.lblCustomerName);
            this.groupBox1.Controls.Add(this.txtCustomer);
            this.groupBox1.Controls.Add(this.ultraLabel2);
            this.groupBox1.Controls.Add(this.ultraLabel1);
            this.groupBox1.Controls.Add(this.txtExpiryDays);
            this.groupBox1.Controls.Add(this.txtCLCardID);
            this.groupBox1.Controls.Add(this.ultraLabel7);
            this.groupBox1.Controls.Add(this.ultraLabel11);
            this.groupBox1.Controls.Add(this.ultraLabel20);
            this.groupBox1.Controls.Add(this.dtpRegisterDate);
            this.groupBox1.Controls.Add(this.ultraLabel14);
            this.groupBox1.Controls.Add(this.txtDescription);
            this.groupBox1.Controls.Add(this.chkIsPrepetual);
            this.groupBox1.Controls.Add(this.ultraLabel4);
            this.groupBox1.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.groupBox1.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(14, 62);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(644, 214);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "CL Card Details";
            // 
            // numCurrentPoints
            // 
            appearance7.FontData.BoldAsString = "False";
            this.numCurrentPoints.Appearance = appearance7;
            this.numCurrentPoints.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.numCurrentPoints.ButtonStyle = Infragistics.Win.UIElementButtonStyle.VisualStudio2005Button;
            this.numCurrentPoints.Enabled = false;
            this.numCurrentPoints.FormatString = "###############";
            this.numCurrentPoints.Location = new System.Drawing.Point(130, 176);
            this.numCurrentPoints.MaskClipMode = Infragistics.Win.UltraWinMaskedEdit.MaskMode.Raw;
            this.numCurrentPoints.MaskDisplayMode = Infragistics.Win.UltraWinMaskedEdit.MaskMode.Raw;
            this.numCurrentPoints.MaskInput = "nnnnnnnnnn";
            this.numCurrentPoints.MaxValue = 99999999999D;
            this.numCurrentPoints.MinValue = -1;
            this.numCurrentPoints.Name = "numCurrentPoints";
            this.numCurrentPoints.NullText = "0";
            this.numCurrentPoints.NumericType = Infragistics.Win.UltraWinEditors.NumericType.Double;
            this.numCurrentPoints.Size = new System.Drawing.Size(152, 23);
            this.numCurrentPoints.SpinButtonDisplayStyle = Infragistics.Win.ButtonDisplayStyle.Always;
            this.numCurrentPoints.TabIndex = 6;
            this.numCurrentPoints.TabNavigation = Infragistics.Win.UltraWinMaskedEdit.MaskedEditTabNavigation.NextControl;
            this.numCurrentPoints.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // ultraLabel3
            // 
            appearance8.ForeColor = System.Drawing.Color.White;
            appearance8.TextVAlignAsString = "Middle";
            this.ultraLabel3.Appearance = appearance8;
            this.ultraLabel3.BackColorInternal = System.Drawing.Color.Transparent;
            this.ultraLabel3.Location = new System.Drawing.Point(12, 171);
            this.ultraLabel3.Name = "ultraLabel3";
            this.ultraLabel3.Size = new System.Drawing.Size(117, 29);
            this.ultraLabel3.TabIndex = 12;
            this.ultraLabel3.Text = "Current Points";
            // 
            // lblCustomerName
            // 
            appearance9.ForeColor = System.Drawing.Color.White;
            this.lblCustomerName.Appearance = appearance9;
            this.lblCustomerName.Location = new System.Drawing.Point(284, 144);
            this.lblCustomerName.Name = "lblCustomerName";
            this.lblCustomerName.Size = new System.Drawing.Size(263, 19);
            this.lblCustomerName.TabIndex = 42;
            // 
            // txtCustomer
            // 
            appearance10.BorderColor = System.Drawing.Color.Lime;
            appearance10.BorderColor3DBase = System.Drawing.Color.Lime;
            this.txtCustomer.Appearance = appearance10;
            this.txtCustomer.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance11.BackColor = System.Drawing.Color.White;
            appearance11.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(231)))), ((int)(((byte)(230)))));
            appearance11.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance11.BorderAlpha = Infragistics.Win.Alpha.Transparent;
            appearance11.Image = ((object)(resources.GetObject("appearance11.Image")));
            appearance11.ImageAlpha = Infragistics.Win.Alpha.Opaque;
            appearance11.ImageBackgroundAlpha = Infragistics.Win.Alpha.Opaque;
            appearance11.ImageBackgroundStyle = Infragistics.Win.ImageBackgroundStyle.Centered;
            editorButton1.Appearance = appearance11;
            editorButton1.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            editorButton1.Key = "S";
            editorButton1.Text = "";
            editorButton1.Width = 20;
            this.txtCustomer.ButtonsRight.Add(editorButton1);
            this.txtCustomer.Location = new System.Drawing.Point(131, 144);
            this.txtCustomer.Name = "txtCustomer";
            this.txtCustomer.Size = new System.Drawing.Size(152, 23);
            this.txtCustomer.TabIndex = 5;
            this.toolTip1.SetToolTip(this.txtCustomer, "Press F4 to search Customer, Trans History=F10, Customer Notes=F11");
            this.txtCustomer.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.txtCustomer.EditorButtonClick += new Infragistics.Win.UltraWinEditors.EditorButtonEventHandler(this.txtCustomer_EditorButtonClick);
            this.txtCustomer.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtCustomer_KeyDown);
            this.txtCustomer.Leave += new System.EventHandler(this.txtCustomer_Leave);
            // 
            // ultraLabel2
            // 
            appearance12.ForeColor = System.Drawing.Color.White;
            appearance12.TextVAlignAsString = "Middle";
            this.ultraLabel2.Appearance = appearance12;
            this.ultraLabel2.BackColorInternal = System.Drawing.Color.Transparent;
            this.ultraLabel2.Location = new System.Drawing.Point(12, 144);
            this.ultraLabel2.Name = "ultraLabel2";
            this.ultraLabel2.Size = new System.Drawing.Size(96, 23);
            this.ultraLabel2.TabIndex = 11;
            this.ultraLabel2.Text = "Customer";
            // 
            // ultraLabel1
            // 
            appearance13.FontData.BoldAsString = "False";
            appearance13.ForeColor = System.Drawing.Color.White;
            this.ultraLabel1.Appearance = appearance13;
            this.ultraLabel1.Location = new System.Drawing.Point(12, 114);
            this.ultraLabel1.Name = "ultraLabel1";
            this.ultraLabel1.Size = new System.Drawing.Size(116, 21);
            this.ultraLabel1.TabIndex = 10;
            this.ultraLabel1.Text = "Register Date";
            // 
            // txtExpiryDays
            // 
            appearance14.FontData.BoldAsString = "False";
            this.txtExpiryDays.Appearance = appearance14;
            this.txtExpiryDays.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.txtExpiryDays.ButtonStyle = Infragistics.Win.UIElementButtonStyle.VisualStudio2005Button;
            this.txtExpiryDays.FormatString = "###############";
            this.txtExpiryDays.Location = new System.Drawing.Point(452, 82);
            this.txtExpiryDays.MaskClipMode = Infragistics.Win.UltraWinMaskedEdit.MaskMode.Raw;
            this.txtExpiryDays.MaskDisplayMode = Infragistics.Win.UltraWinMaskedEdit.MaskMode.Raw;
            this.txtExpiryDays.MaskInput = "nnnnnnnnnn";
            this.txtExpiryDays.MaxValue = 99999;
            this.txtExpiryDays.MinValue = -1;
            this.txtExpiryDays.Name = "txtExpiryDays";
            this.txtExpiryDays.NullText = "0";
            this.txtExpiryDays.NumericType = Infragistics.Win.UltraWinEditors.NumericType.Double;
            this.txtExpiryDays.Size = new System.Drawing.Size(184, 23);
            this.txtExpiryDays.SpinButtonDisplayStyle = Infragistics.Win.ButtonDisplayStyle.Always;
            this.txtExpiryDays.TabIndex = 3;
            this.txtExpiryDays.TabNavigation = Infragistics.Win.UltraWinMaskedEdit.MaskedEditTabNavigation.NextControl;
            this.txtExpiryDays.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.txtExpiryDays.Validated += new System.EventHandler(this.txtNumericBoxs_Validate);
            // 
            // txtCLCardID
            // 
            appearance15.FontData.BoldAsString = "False";
            this.txtCLCardID.Appearance = appearance15;
            this.txtCLCardID.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.txtCLCardID.ButtonStyle = Infragistics.Win.UIElementButtonStyle.VisualStudio2005Button;
            this.txtCLCardID.FormatString = "###################";
            this.txtCLCardID.Location = new System.Drawing.Point(130, 19);
            this.txtCLCardID.MaskClipMode = Infragistics.Win.UltraWinMaskedEdit.MaskMode.Raw;
            this.txtCLCardID.MaskDisplayMode = Infragistics.Win.UltraWinMaskedEdit.MaskMode.Raw;
            this.txtCLCardID.MaskInput = "nnnnnnnnnnnnnnn";
            this.txtCLCardID.MaxValue = ((long)(999999999999999));
            this.txtCLCardID.MinValue = -1;
            this.txtCLCardID.Name = "txtCLCardID";
            this.txtCLCardID.NullText = "0";
            this.txtCLCardID.NumericType = Infragistics.Win.UltraWinEditors.NumericType.Double;
            this.txtCLCardID.Size = new System.Drawing.Size(152, 23);
            this.txtCLCardID.SpinButtonDisplayStyle = Infragistics.Win.ButtonDisplayStyle.Always;
            this.txtCLCardID.TabIndex = 0;
            this.txtCLCardID.TabNavigation = Infragistics.Win.UltraWinMaskedEdit.MaskedEditTabNavigation.NextControl;
            this.txtCLCardID.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.txtCLCardID.Validating += new System.ComponentModel.CancelEventHandler(this.txtCLCardID_Validating);
            this.txtCLCardID.Validated += new System.EventHandler(this.txtNumericBoxs_Validate);
            // 
            // ultraLabel7
            // 
            appearance16.ForeColor = System.Drawing.Color.White;
            appearance16.TextHAlignAsString = "Center";
            appearance16.TextVAlignAsString = "Middle";
            this.ultraLabel7.Appearance = appearance16;
            this.ultraLabel7.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultraLabel7.Location = new System.Drawing.Point(113, 26);
            this.ultraLabel7.Name = "ultraLabel7";
            this.ultraLabel7.Size = new System.Drawing.Size(11, 15);
            this.ultraLabel7.TabIndex = 37;
            this.ultraLabel7.Text = "*";
            // 
            // dtpRegisterDate
            // 
            this.dtpRegisterDate.AllowNull = false;
            appearance17.FontData.BoldAsString = "False";
            this.dtpRegisterDate.Appearance = appearance17;
            this.dtpRegisterDate.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Office2003ToolbarButton;
            this.dtpRegisterDate.DateButtons.Add(dateButton1);
            this.dtpRegisterDate.Enabled = false;
            this.dtpRegisterDate.Location = new System.Drawing.Point(131, 113);
            this.dtpRegisterDate.Name = "dtpRegisterDate";
            this.dtpRegisterDate.NonAutoSizeHeight = 10;
            this.dtpRegisterDate.Size = new System.Drawing.Size(152, 22);
            this.dtpRegisterDate.TabIndex = 4;
            this.dtpRegisterDate.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.dtpRegisterDate.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.dtpRegisterDate.Value = new System.DateTime(2004, 5, 25, 0, 0, 0, 0);
            // 
            // txtDescription
            // 
            this.txtDescription.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.txtDescription.Location = new System.Drawing.Point(130, 51);
            this.txtDescription.MaxLength = 50;
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.Size = new System.Drawing.Size(506, 23);
            this.txtDescription.TabIndex = 1;
            this.txtDescription.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.txtDescription.Validated += new System.EventHandler(this.txtBoxs_Validate);
            // 
            // ultraLabel4
            // 
            appearance18.ForeColor = System.Drawing.Color.White;
            this.ultraLabel4.Appearance = appearance18;
            this.ultraLabel4.Location = new System.Drawing.Point(12, 72);
            this.ultraLabel4.Name = "ultraLabel4";
            this.ultraLabel4.Size = new System.Drawing.Size(116, 36);
            this.ultraLabel4.TabIndex = 9;
            this.ultraLabel4.Text = "Does Not Expire?";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnResetCpnGenCycle);
            this.groupBox2.Controls.Add(this.btnPrintCoupons);
            this.groupBox2.Controls.Add(this.btnChangeCardPoints);
            this.groupBox2.Controls.Add(this.btnPrintCard);
            this.groupBox2.Controls.Add(this.btnClose);
            this.groupBox2.Controls.Add(this.btnSave);
            this.groupBox2.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.groupBox2.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.Location = new System.Drawing.Point(14, 280);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(643, 138);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            // 
            // btnResetCpnGenCycle
            // 
            appearance19.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(62)))), ((int)(((byte)(176)))));
            appearance19.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance19.FontData.BoldAsString = "True";
            appearance19.ForeColor = System.Drawing.Color.White;
            this.btnResetCpnGenCycle.Appearance = appearance19;
            this.btnResetCpnGenCycle.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            this.btnResetCpnGenCycle.Location = new System.Drawing.Point(484, 18);
            this.btnResetCpnGenCycle.Name = "btnResetCpnGenCycle";
            this.btnResetCpnGenCycle.Size = new System.Drawing.Size(149, 50);
            this.btnResetCpnGenCycle.TabIndex = 5;
            this.btnResetCpnGenCycle.Text = "Reset Coupon Generation Cycle";
            this.btnResetCpnGenCycle.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnResetCpnGenCycle.Click += new System.EventHandler(this.btnResetCpnGenCycle_Click);
            // 
            // btnPrintCoupons
            // 
            appearance20.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(62)))), ((int)(((byte)(176)))));
            appearance20.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance20.FontData.BoldAsString = "True";
            appearance20.ForeColor = System.Drawing.Color.White;
            appearance20.Image = ((object)(resources.GetObject("appearance20.Image")));
            this.btnPrintCoupons.Appearance = appearance20;
            this.btnPrintCoupons.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            this.btnPrintCoupons.Enabled = false;
            this.btnPrintCoupons.Location = new System.Drawing.Point(326, 18);
            this.btnPrintCoupons.Name = "btnPrintCoupons";
            this.btnPrintCoupons.Size = new System.Drawing.Size(149, 50);
            this.btnPrintCoupons.TabIndex = 4;
            this.btnPrintCoupons.Text = "P&rint Coupons";
            this.toolTip1.SetToolTip(this.btnPrintCoupons, "Print all valid coupons");
            this.btnPrintCoupons.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnPrintCoupons.Click += new System.EventHandler(this.btnPrintCoupons_Click);
            // 
            // btnChangeCardPoints
            // 
            appearance21.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(62)))), ((int)(((byte)(176)))));
            appearance21.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance21.FontData.BoldAsString = "True";
            appearance21.ForeColor = System.Drawing.Color.White;
            this.btnChangeCardPoints.Appearance = appearance21;
            this.btnChangeCardPoints.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            this.btnChangeCardPoints.Enabled = false;
            this.btnChangeCardPoints.Location = new System.Drawing.Point(12, 18);
            this.btnChangeCardPoints.Name = "btnChangeCardPoints";
            this.btnChangeCardPoints.Size = new System.Drawing.Size(149, 50);
            this.btnChangeCardPoints.TabIndex = 2;
            this.btnChangeCardPoints.Text = "C&hange Card Points (F2)";
            this.btnChangeCardPoints.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnChangeCardPoints.Click += new System.EventHandler(this.btnChangeCardPoints_Click);
            // 
            // btnPrintCard
            // 
            appearance22.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(62)))), ((int)(((byte)(176)))));
            appearance22.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance22.FontData.BoldAsString = "True";
            appearance22.ForeColor = System.Drawing.Color.White;
            appearance22.Image = ((object)(resources.GetObject("appearance22.Image")));
            this.btnPrintCard.Appearance = appearance22;
            this.btnPrintCard.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            this.btnPrintCard.Enabled = false;
            this.btnPrintCard.Location = new System.Drawing.Point(169, 18);
            this.btnPrintCard.Name = "btnPrintCard";
            this.btnPrintCard.Size = new System.Drawing.Size(149, 50);
            this.btnPrintCard.TabIndex = 3;
            this.btnPrintCard.Text = "&Print Card";
            this.btnPrintCard.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnPrintCard.Click += new System.EventHandler(this.btnPrintCard_Click);
            // 
            // frmCLCards
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(108)))), ((int)(((byte)(172)))));
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(672, 430);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.lblTransactionType);
            this.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.Name = "frmCLCards";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Customer Loyalty Card";
            this.Activated += new System.EventHandler(this.frmCLCards_Activated);
            this.Load += new System.EventHandler(this.frmCLCards_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmCLCards_KeyDown);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.frmCLCards_KeyUp);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numCurrentPoints)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCustomer)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtExpiryDays)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCLCardID)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtpRegisterDate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDescription)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);

        }
        #endregion

        private bool Save()
        {
            bool retValue = false;
            try
            {

                if (oCLCardsRow.CLCardID == 0)
                {
                    clsUIHelper.ShowErrorMsg("Card Number must be higher than zero");
                    txtCLCardID.Focus();
                }
                else if (chkIsPrepetual.Checked == false && Configuration.convertNullToInt(txtExpiryDays.Value) == 0)
                {
                    clsUIHelper.ShowErrorMsg("Expiry days value must be higher than zero.");
                    txtExpiryDays.Focus();
                }
                else if (txtCustomer.Text.Trim() == "" || txtCustomer.Tag == null || txtCustomer.Tag.ToString() == "")
                {
                    clsUIHelper.ShowErrorMsg("Please select a customer.");
                    txtCustomer.Focus();
                }
                else
                {
                    oBRCLCards.Persist(oCLCardsData);
                    retValue = true;
                }
            }
            catch (POSExceptions exp)
            {
                clsUIHelper.ShowErrorMsg(exp.ErrMessage);
                retValue = false;
            }

            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
                retValue = false;
            }
            return retValue;
        }
        private void btnSave_Click(object sender, System.EventArgs e)
        {
            if (Save())
            {
                IsCanceled = false;
                this.Close();
            }
        }

        private void txtBoxs_Validate(object sender, System.EventArgs e)
        {
            try
            {
                if (oCLCardsRow == null)
                    return;
                Infragistics.Win.UltraWinEditors.UltraTextEditor txtEditor = (Infragistics.Win.UltraWinEditors.UltraTextEditor)sender;
                switch (txtEditor.Name)
                {
                    case "txtDescription":
                        oCLCardsRow.Description = txtDescription.Text;
                        break;
                }
            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
            }

        }

        private void txtNumericBoxs_Validate(object sender, System.EventArgs e)
        {
            try
            {
                if (oCLCardsRow == null)
                    return;
                Infragistics.Win.UltraWinEditors.UltraNumericEditor txtEditor = (Infragistics.Win.UltraWinEditors.UltraNumericEditor)sender;
                switch (txtEditor.Name)
                {
                    case "txtExpiryDays":
                        oCLCardsRow.ExpiryDays = POS_Core.Resources.Configuration.convertNullToInt(this.txtExpiryDays.Value);
                        break;
                }
            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }

        private void txtDateBoxs_Validate(object sender, System.EventArgs e)
        {
            try
            {
                if (oCLCardsRow == null)
                    return;
                Infragistics.Win.UltraWinSchedule.UltraCalendarCombo txtEditor = (Infragistics.Win.UltraWinSchedule.UltraCalendarCombo)sender;
                switch (txtEditor.Name)
                {
                    /*
                     * case "dtpExpiryDate":
						oCLCardsRow.ExpiryDate= (System.DateTime)this.dtpExpiryDate.Value;
						break;
                     */
                }
            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }

        private void chkIsPrepetual_CheckedChanged(object sender, System.EventArgs e)
        {
            try
            {
                if (oCLCardsRow == null)
                    return;
                oCLCardsRow.IsPrepetual = chkIsPrepetual.Checked;
                if (oCLCardsRow.IsPrepetual)
                {
                    txtExpiryDays.Enabled = false;
                }
                else
                {
                    txtExpiryDays.Enabled = true;
                }
            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }

        private void btnSearch_Click(object sender, System.EventArgs e)
        {
            Search();
        }

        private void Search()
        {
            try
            {
                //frmSearch oSearch = new frmSearch(clsPOSDBConstants.CLCards_tbl);
                frmSearchMain oSearch = new frmSearchMain(true);    //20-Dec-2017 JY Added new reference
                oSearch.SearchTable = clsPOSDBConstants.CLCards_tbl;    //20-Dec-2017 JY Added
                oSearch.ShowDialog(this);
                if (!oSearch.IsCanceled)
                {
                    string strCode = oSearch.SelectedRowID();
                    if (strCode == "")
                        return;

                    Edit(POS_Core.Resources.Configuration.convertNullToInt(strCode));
                }
            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }

        private void Display()
        {
            txtCLCardID.Text = oCLCardsRow.CLCardID.ToString();
            txtDescription.Text = oCLCardsRow.Description;
            txtExpiryDays.Value = oCLCardsRow.ExpiryDays;
            dtpRegisterDate.Value = oCLCardsRow.RegisterDate;
            chkIsPrepetual.Checked = oCLCardsRow.IsPrepetual;
            CLPointsRewardTier oTier = new CLPointsRewardTier();
            numCurrentPoints.Value = oTier.GetCurrentTotalPoints(oCLCardsRow.CLCardID);
            if (Configuration.convertNullToInt(numCurrentPoints.Value) == 0)
            {
                numCurrentPoints.Value = null;
            }
            if (oCLCardsRow.CustomerID > 0)
            {
                txtCustomer.Text = oCLCardsRow.CustomerID.ToString();
                Customer oCustomer = new Customer();
                FKEdit(oCLCardsRow.CustomerID.ToString(), clsPOSDBConstants.Customer_tbl, false);

            }
            btnPrintCard.Enabled = true;
            btnPrintCoupons.Enabled = true;
        }

        public void Edit(Int64 iCLCardID)
        {
            try
            {
                txtCLCardID.Enabled = false;
                oCLCardsData = oBRCLCards.GetByCLCardID(iCLCardID);
                oCLCardsRow = oCLCardsData.CLCards[0];

                if (oCLCardsRow != null)
                    Display();

            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }

        private void SetNew()
        {
            oBRCLCards = new CLCards();
            oCLCardsData = new CLCardsData();
            if (oCLCardsData != null) oCLCardsData.CLCards.Rows.Clear();
            oCLCardsRow = oCLCardsData.CLCards.AddRow(0, Configuration.CLoyaltyInfo.IsCardPrepetual, 0, "", Configuration.CLoyaltyInfo.DefaultCardExpiryDays, true);
            this.btnPrintCard.Enabled = false;
            this.btnPrintCoupons.Enabled = false;
            Clear();
        }

        private void Clear()
        {
            txtDescription.Text = string.Empty;
            txtCLCardID.Value = 0; // oCLCardsRow.CLCardID = oBRCLCards.GetNextCardNumber();
            numCurrentPoints.Value = 0;
            dtpRegisterDate.Value = oCLCardsRow.RegisterDate = System.DateTime.Now;
            txtExpiryDays.Value = oCLCardsRow.ExpiryDays = POS_Core.Resources.Configuration.CLoyaltyInfo.DefaultCardExpiryDays;
            chkIsPrepetual.Checked = oCLCardsRow.IsPrepetual = POS_Core.Resources.Configuration.CLoyaltyInfo.IsCardPrepetual;

        }

        private void txtDeptCode_Leave(object sender, System.EventArgs e)
        {
            try
            {
                SearchCLCards();
            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }

        private void SearchCLCards()
        {
            /*			if (txtDeptCode.Text==null || txtDeptCode.Text.Trim()=="")
                            return;
                        try
                        {
                            if (Edit(txtDeptCode.Text)==false)
                            {
                                SetNew();
                                oCLCardsRow.DeptCode =  txtDeptCode.Text ;
                            }
                        }
                        catch(Exception exp)
                        {
                            clsUIHelper.ShowErrorMsg( exp.Message);
                        } */
        }

        private void btnNew_Click(object sender, System.EventArgs e)
        {
            try
            {
                SetNew();
            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }

        private void btnClose_Click(object sender, System.EventArgs e)
        {
            IsCanceled = true;
            this.Close();
        }

        private void frmCLCards_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.F2)
                {
                    ChangeCardPoints();
                }
            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }

        private void frmCLCards_Load(object sender, System.EventArgs e)
        {
            this.txtCLCardID.AfterEnterEditMode += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
            this.txtCLCardID.AfterExitEditMode += new System.EventHandler(clsUIHelper.AfterExitEditMode);
            this.txtDescription.AfterEnterEditMode += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
            this.txtDescription.AfterExitEditMode += new System.EventHandler(clsUIHelper.AfterExitEditMode);

            this.dtpRegisterDate.Enter += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
            this.dtpRegisterDate.Leave += new System.EventHandler(clsUIHelper.AfterExitEditMode);

            IsCanceled = true;
            clsUIHelper.setColorSchecme(this);

            if (oCLCardsRow.CLCardID > 0)
            {
                this.btnResetCpnGenCycle.Enabled = Configuration.CLoyaltyInfo.SingleCouponPerRewardTier;
                btnChangeCardPoints.Enabled = true;
            }
            else
            {
                this.btnResetCpnGenCycle.Enabled = false;
                btnChangeCardPoints.Enabled = false;
            }
        }

        private void frmCLCards_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
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
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }

        private void frmCLCards_Activated(object sender, System.EventArgs e)
        {
            clsUIHelper.CurrentForm = this;
        }

        private void txtCLCardID_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                if (oCLCardsRow == null)
                    return;
                Int64 iCLCardID = POS_Core.Resources.Configuration.convertNullToInt64(txtCLCardID.Value);
                if (iCLCardID > 0)
                {
                    if (iCLCardID < POS_Core.Resources.Configuration.CLoyaltyInfo.CardRangeFrom || iCLCardID > POS_Core.Resources.Configuration.CLoyaltyInfo.CardRangeTo)
                    {
                        clsUIHelper.ShowErrorMsg("Invalid Card number. Valid range is from " + POS_Core.Resources.Configuration.CLoyaltyInfo.CardRangeFrom.ToString() + " to " + POS_Core.Resources.Configuration.CLoyaltyInfo.CardRangeTo.ToString() + ".");
                        e.Cancel = true;
                    }
                    else
                    {
                        if (oCLCardsRow.CLCardID != iCLCardID && oCLCardsRow.ID == 0)
                        {
                            //CLCardsData oData = oBRCLCards.GetByCLCardID(iCLCardID);  //07-Oct-2016 JY Commented
                            CLCardsData oData = oBRCLCards.CheckCLCardExists(iCLCardID);    //07-Oct-2016 JY Added to check CL Card exists irrespective of active/inactive
                            if (oData.CLCards.Count > 0)
                            {
                                clsUIHelper.ShowErrorMsg("Card number is already in use. Please type another card number.");
                                e.Cancel = true;
                                return;
                            }
                        }
                        oCLCardsRow.CLCardID = iCLCardID;
                    }
                }
            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
                e.Cancel = true;
            }
        }

        private void txtCustomer_EditorButtonClick(object sender, EditorButtonEventArgs e)
        {
            try
            {
                if (e.Button.Key == "S")
                {
                    SearchCustomer();
                }
                else if (e.Button.Key == "H")
                {
                    if (this.txtCustomer.Text.Trim().Length > 0)
                    {
                        frmViewPOSTransaction ofrm = new frmViewPOSTransaction();
                        ofrm.txtCustomer.Text = this.txtCustomer.Text;
                        ofrm.txtCustomer.Tag = this.txtCustomer.Tag;
                        ofrm.lblCustomerName.Text = this.lblCustomerName.Text;
                        ofrm.ShowDialog(this);
                    }
                }
                else if (e.Button.Key == "N")
                {
                    if (this.txtCustomer.Text.Trim().Length > 0)
                    {
                        frmCustomerNotesView ofrm = new frmCustomerNotesView(Configuration.convertNullToInt(this.txtCustomer.Tag.ToString()));
                        ofrm.ShowDialog(this);
                    }
                }
            }
            catch (Exception) { }
        }

        private void txtCustomer_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.F4)
            {
                txtCustomer_EditorButtonClick(sender, new Infragistics.Win.UltraWinEditors.EditorButtonEventArgs(txtCustomer.ButtonsRight["S"], sender));
                e.Handled = true;
            }
        }

        private void txtCustomer_Leave(object sender, EventArgs e)
        {
            try
            {
                string txtValue = this.txtCustomer.Text;
                if (txtValue.Trim() != "")
                {
                    FKEdit(txtValue, clsPOSDBConstants.Customer_tbl, false);
                }
            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
                this.txtCustomer.Focus();
            }
        }


        private void FKEdit(string code, string senderName, bool searchByItemDescription)
        {
            if (senderName == clsPOSDBConstants.Customer_tbl)
            {
                #region Customer
                try
                {
                    Customer oCustomer = new Customer();
                    CustomerData oCustomerData;
                    CustomerRow oCustomerRow = null;

                    //Update By Prog1 03Oct09
                    //If value typed in by user is number then search by account number else search by customer name
                    if (Configuration.convertNullToInt(code) != 0)
                    {
                        oCustomerData = oCustomer.Populate(code, true);
                    }
                    else
                    {
                        oCustomerData = oCustomer.PopulateByName(code, true);
                    }

                    //Updated By SRT(Gaurav) Date : 21-Jul-2009
                    //Validated whether the customer DATASET  IS NOT NULL
                    if (oCustomerData == null || oCustomerData.Customer.Count == 0)
                    {
                        if (UserPriviliges.IsUserHasPriviliges(UserPriviliges.Modules.POSTransaction.ID, UserPriviliges.Screens.Customers.ID, -999))
                        {
                            if (Resources.Message.Display("No customer found.\nDo you want to add this customer.", "Manage Customer", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                            {
                                frmCustomers ofrmCustomers = new frmCustomers();
                                ofrmCustomers.ShowDialog();
                                if (ofrmCustomers.IsCanceled == true)
                                {
                                    ClearCustomer();
                                    txtCustomer.Focus();
                                    return;
                                }
                                else
                                {
                                    oCustomerData = ofrmCustomers.CustmerData;
                                }
                            }
                            else
                            {
                                ClearCustomer();
                                txtCustomer.Focus();
                                return;
                            }
                        }
                        else
                        {
                            clsUIHelper.ShowErrorMsg("No customer found.");
                            ClearCustomer();
                            txtCustomer.Focus();
                            return;
                        }
                    }
                    oCustomerRow = oCustomerData.Customer[0];
                    if (oCustomerRow != null)
                    {
                        this.txtCustomer.Text = oCustomerRow.AccountNumber.ToString();
                        this.lblCustomerName.Text = oCustomerRow.CustomerFullName;
                        this.txtCustomer.Tag = oCustomerRow.CustomerId;
                        oCLCardsRow.CustomerID = oCustomerRow.CustomerId;
                        //Added By Dharmendra(SRT) which will be required when processing
                    }
                }
                catch (System.IndexOutOfRangeException)
                {
                    SearchCustomer();
                }
                catch (Exception exp)
                {
                    clsUIHelper.ShowErrorMsg(exp.Message);
                    SearchCustomer();
                }
                #endregion
            }
        }

        private void SearchCustomer()
        {
            try
            {
                //frmCustomerSearch oSearch = new frmCustomerSearch(txtCustomer.Text);
                frmSearchMain oSearch = new frmSearchMain(txtCustomer.Text, true, clsPOSDBConstants.Customer_tbl);    //18-Dec-2017 JY Added new reference
                oSearch.ActiveOnly = 1;
                oSearch.ShowDialog(this);
                if (!oSearch.IsCanceled)
                {
                    string strCode = oSearch.SelectedRowID();
                    if (strCode == "")
                    {
                        ClearCustomer();
                        return;
                    }

                    FKEdit(strCode, clsPOSDBConstants.Customer_tbl, false);
                    this.txtCustomer.Focus();
                }
                else
                {
                    ClearCustomer();
                }
            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
                ClearCustomer();
            }
        }

        private void ClearCustomer()
        {
            this.txtCustomer.Text = String.Empty;
            this.lblCustomerName.Text = String.Empty;
            this.txtCustomer.Tag = String.Empty;
        }

        private void PrintCLCard()
        {
            Image oImage = null;
            try
            {
                if (this.oCLCardsRow == null)
                {
                    return;
                }

                string sImagePath = System.IO.Path.Combine(System.IO.Path.GetTempPath(), oCLCardsRow.CLCardID.ToString() + ".bmp");

                Mabry.Windows.Forms.Barcode.Licenser.Key = "E1P8-HKELVF8R04Q0";

                string sBarcode = "#" + oCLCardsRow.CLCardID.ToString() + "#";

                if (System.IO.File.Exists(sImagePath) == true)
                {
                    System.IO.File.Delete(sImagePath);
                }

                Configuration.PrintBarcode(sBarcode, 0, 0, 20, 200, "CODE128", "H", sImagePath);

                oImage = Image.FromFile(sImagePath);
                CustomerLoyaltyCard oCLable = new CustomerLoyaltyCard(this.oCLCardsRow, oImage);
                //oCLable.Print();  //PRIMEPOS-2829 22-Apr-2020 JY Commented
                #region PRIMEPOS-2829 22-Apr-2020 JY Added
                if (Configuration.CPOSSet.ReceiptPrinterType == "L")
                    oCLable.PrintL();
                else
                    oCLable.PrintD();
                #endregion
                oImage.Dispose();
            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
            finally
            {
                if (oImage != null)
                {
                    oImage.Dispose();
                }
            }
        }

        private void btnChangeCardPoints_Click(object sender, EventArgs e)
        {
            ChangeCardPoints();
        }

        private void ChangeCardPoints()
        {
            if (oCLCardsRow.CLCardID > 0 && btnChangeCardPoints.Enabled == true)
            {
                frmChangeCLCardPoints ofrm = new frmChangeCLCardPoints(this.oCLCardsRow);
                ofrm.ShowDialog(this);
                if (ofrm.IsCanceled == false)
                {
                    CLPointsRewardTier oTier = new CLPointsRewardTier();
                    numCurrentPoints.Value = oTier.GetCurrentTotalPoints(oCLCardsRow.CLCardID);
                    if (Configuration.convertNullToInt(numCurrentPoints.Value) == 0)
                    {
                        numCurrentPoints.Value = null;
                    }
                }
            }
        }

        private void btnPrintCard_Click(object sender, EventArgs e)
        {
            PrintCLCard();
        }

        private void btnPrintCoupons_Click(object sender, EventArgs e)
        {
            PrintCLCoupons();
        }

        private void PrintCLCoupons()
        {
            Image oImage = null;
            try
            {
                if (this.oCLCardsRow == null)
                {
                    return;
                }

                CLCoupons coupons = new CLCoupons();
                CLCouponsData couponsData = coupons.GetUnUsedCLCoupons(this.oCLCardsRow.CLCardID);

                if (couponsData.CLCoupons.Count == 0)
                {
                    clsUIHelper.ShowErrorMsg("No unused coupon found.");
                }

                foreach (CLCouponsRow couponsRow in couponsData.CLCoupons)
                {
                    string sImagePath = System.IO.Path.Combine(System.IO.Path.GetTempPath(), couponsRow.ID.ToString() + ".bmp");

                    Mabry.Windows.Forms.Barcode.Licenser.Key = "E1P8-HKELVF8R04Q0";

                    string sBarcode = "!" + couponsRow.ID.ToString() + "!";

                    if (System.IO.File.Exists(sImagePath) == true)
                    {
                        System.IO.File.Delete(sImagePath);
                    }

                    Configuration.PrintBarcode(sBarcode, 0, 0, 20, 200, "CODE128", "H", sImagePath);

                    oImage = Image.FromFile(sImagePath);
                    CustomerLoyaltyCoupon oCLable = new CustomerLoyaltyCoupon(couponsRow, oImage, lblCustomerName.Text.ToString()); //29-Apr-2015 JY Added 
                    //oCLable.Print(); //PRIMEPOS-2829 22-Apr-2020 JY Commented
                    #region PRIMEPOS-2829 22-Apr-2020 JY Added
                    if (Configuration.CPOSSet.ReceiptPrinterType == "L")
                        oCLable.PrintL();
                    else
                        oCLable.PrintD();
                    #endregion
                    oImage.Dispose();
                }
            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
            finally
            {
                if (oImage != null)
                {
                    oImage.Dispose();
                }
            }
        }

        /// <summary>
        /// Author:Shitaljit
        /// Date: 2/6/2014
        /// Created to reset single coupon per reawrd tier logic for specific card.
        /// </summary>
        private void btnResetCpnGenCycle_Click(object sender, EventArgs e)
        {
            CLCoupons oCLCoupons = new CLCoupons();
            try
            {
                if (oCLCoupons.ResetCouponGenarationCycle(oCLCardsRow.CLCardID))
                {
                    clsUIHelper.ShowOKMsg("Coupon generation cycle reseted successfully.");
                }
            }
            catch (Exception Ex)
            {

                clsUIHelper.ShowOKMsg("Error while resetting coupon generation cycle.");
            }

        }
    }
}
