using System;
using System.Windows.Forms;
using Infragistics.Win.UltraWinEditors;
using POS_Core.BusinessRules;
using POS_Core.CommonData;
using POS_Core.CommonData.Rows;
//using POS_Core.DataAccess;
using NLog;
using POS_Core.Resources;

namespace POS_Core_UI
{
    /// <summary>
    /// Summary description for frmItemMonitorCategory.
    /// </summary>
    public class frmItemMonitorCategory : System.Windows.Forms.Form
    {
        public bool IsCanceled = true;
        private ItemMonitorCategoryData oItemMonitorCategoryData = new ItemMonitorCategoryData();
        private ItemMonitorCategoryRow oItemMonitorCategoryRow;
        private ItemMonitorCategory oItemMonitorCategory = new ItemMonitorCategory();
        private bool isEdit = false;

        #region Windows Generated code

        private Infragistics.Win.Misc.UltraLabel ultraLabel11;
        private Infragistics.Win.Misc.UltraLabel lblTransactionType;
        private Infragistics.Win.Misc.UltraButton btnClose;
        private Infragistics.Win.Misc.UltraButton btnSave;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ToolTip toolTip1;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor txtName;
        private Infragistics.Win.Misc.UltraLabel ultraLabel2;
        private Infragistics.Win.Misc.UltraLabel ultraLabel1;
        private UltraNumericEditor numDailyLimit;
        private UltraNumericEditor num30DaysLimit;
        private Infragistics.Win.Misc.UltraLabel ultraLabel3;
        private System.ComponentModel.IContainer components;
        private GroupBox groupBox3;
        private UltraCheckEditor chckObtainSign;
        private UltraCheckEditor chckObtainID;
        private UltraCheckEditor chckCanOverride; //PRIMEPOS-3166
        private UltraNumericEditor numLimitPeriodQty;
        private Infragistics.Win.Misc.UltraLabel ultraLabel6;
        private UltraNumericEditor numLimitPeriodDays;
        private Infragistics.Win.Misc.UltraLabel ultraLabel5;
        private UltraNumericEditor numAgeLimit;
        private UltraCheckEditor chkAgeLimit;
        private UltraCheckEditor chkPSE;
        public UltraComboEditor cmbUOM;
        #endregion Windows Generated code

        private static ILogger logger = LogManager.GetCurrentClassLogger();

        public void Initialize()
        {
            SetNew();
        }

        public frmItemMonitorCategory()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();
            try
            {
                Initialize();
            }
            catch(Exception exp)
            {
                logger.Fatal(exp, "frmItemMonitorCategory()");
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
            if(disposing)
            {
                if(components != null)
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmItemMonitorCategory));
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance7 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance8 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance9 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance10 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance11 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance12 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance13 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance14 = new Infragistics.Win.Appearance();
            Infragistics.Win.ValueListItem valueListItem5 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem6 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem7 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem8 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.Appearance appearance15 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance16 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance17 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance18 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance19 = new Infragistics.Win.Appearance();
            this.ultraLabel11 = new Infragistics.Win.Misc.UltraLabel();
            this.txtName = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.btnClose = new Infragistics.Win.Misc.UltraButton();
            this.btnSave = new Infragistics.Win.Misc.UltraButton();
            this.lblTransactionType = new Infragistics.Win.Misc.UltraLabel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.chkPSE = new Infragistics.Win.UltraWinEditors.UltraCheckEditor();
            this.chkAgeLimit = new Infragistics.Win.UltraWinEditors.UltraCheckEditor();
            this.numAgeLimit = new Infragistics.Win.UltraWinEditors.UltraNumericEditor();
            this.numLimitPeriodQty = new Infragistics.Win.UltraWinEditors.UltraNumericEditor();
            this.ultraLabel6 = new Infragistics.Win.Misc.UltraLabel();
            this.numLimitPeriodDays = new Infragistics.Win.UltraWinEditors.UltraNumericEditor();
            this.ultraLabel5 = new Infragistics.Win.Misc.UltraLabel();
            this.cmbUOM = new Infragistics.Win.UltraWinEditors.UltraComboEditor();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.chckObtainID = new Infragistics.Win.UltraWinEditors.UltraCheckEditor();
            this.chckObtainSign = new Infragistics.Win.UltraWinEditors.UltraCheckEditor();
            this.chckCanOverride = new Infragistics.Win.UltraWinEditors.UltraCheckEditor(); //PRIMEPOS-3166
            this.num30DaysLimit = new Infragistics.Win.UltraWinEditors.UltraNumericEditor();
            this.numDailyLimit = new Infragistics.Win.UltraWinEditors.UltraNumericEditor();
            this.ultraLabel3 = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabel2 = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabel1 = new Infragistics.Win.Misc.UltraLabel();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.txtName)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chkPSE)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkAgeLimit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numAgeLimit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numLimitPeriodQty)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numLimitPeriodDays)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbUOM)).BeginInit();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chckObtainID)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chckObtainSign)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chckCanOverride)).BeginInit(); //PRIMEPOS-3166
            ((System.ComponentModel.ISupportInitialize)(this.num30DaysLimit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numDailyLimit)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // ultraLabel11
            // 
            appearance1.ForeColor = System.Drawing.Color.White;
            appearance1.TextVAlignAsString = "Middle";
            this.ultraLabel11.Appearance = appearance1;
            this.ultraLabel11.Location = new System.Drawing.Point(13, 22);
            this.ultraLabel11.Name = "ultraLabel11";
            this.ultraLabel11.Size = new System.Drawing.Size(84, 20);
            this.ultraLabel11.TabIndex = 3;
            this.ultraLabel11.Text = "Description";
            // 
            // txtName
            // 
            this.txtName.Anchor = System.Windows.Forms.AnchorStyles.None;
            appearance2.TextHAlignAsString = "Left";
            appearance2.TextVAlignAsString = "Middle";
            this.txtName.Appearance = appearance2;
            this.txtName.AutoSize = false;
            this.txtName.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.txtName.Location = new System.Drawing.Point(215, 20);
            this.txtName.MaxLength = 50;
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(350, 25);
            this.txtName.TabIndex = 0;
            this.txtName.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.txtName.Validated += new System.EventHandler(this.txtBoxs_Validate);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            appearance3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(62)))), ((int)(((byte)(176)))));
            appearance3.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance3.FontData.BoldAsString = "True";
            appearance3.ForeColor = System.Drawing.Color.White;
            appearance3.Image = ((object)(resources.GetObject("appearance3.Image")));
            this.btnClose.Appearance = appearance3;
            this.btnClose.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(483, 20);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(85, 26);
            this.btnClose.TabIndex = 1;
            this.btnClose.Text = "&Cancel";
            this.btnClose.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            appearance4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(62)))), ((int)(((byte)(176)))));
            appearance4.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance4.FontData.BoldAsString = "True";
            appearance4.ForeColor = System.Drawing.Color.White;
            appearance4.Image = ((object)(resources.GetObject("appearance4.Image")));
            this.btnSave.Appearance = appearance4;
            this.btnSave.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            this.btnSave.Location = new System.Drawing.Point(391, 20);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(85, 26);
            this.btnSave.TabIndex = 0;
            this.btnSave.Text = "&Ok";
            this.btnSave.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // lblTransactionType
            // 
            appearance5.ForeColor = System.Drawing.Color.White;
            appearance5.ForeColorDisabled = System.Drawing.Color.White;
            appearance5.TextHAlignAsString = "Center";
            appearance5.TextVAlignAsString = "Middle";
            this.lblTransactionType.Appearance = appearance5;
            this.lblTransactionType.BackColorInternal = System.Drawing.Color.Transparent;
            this.lblTransactionType.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblTransactionType.Font = new System.Drawing.Font("Arial", 20.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTransactionType.Location = new System.Drawing.Point(0, 0);
            this.lblTransactionType.Name = "lblTransactionType";
            this.lblTransactionType.Size = new System.Drawing.Size(594, 40);
            this.lblTransactionType.TabIndex = 23;
            this.lblTransactionType.Tag = "Header";
            this.lblTransactionType.Text = "Item Monitor Category";
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.groupBox1.Controls.Add(this.txtName);
            this.groupBox1.Controls.Add(this.chkPSE);
            this.groupBox1.Controls.Add(this.chkAgeLimit);
            this.groupBox1.Controls.Add(this.numAgeLimit);
            this.groupBox1.Controls.Add(this.numLimitPeriodQty);
            this.groupBox1.Controls.Add(this.ultraLabel6);
            this.groupBox1.Controls.Add(this.numLimitPeriodDays);
            this.groupBox1.Controls.Add(this.ultraLabel5);
            this.groupBox1.Controls.Add(this.cmbUOM);
            this.groupBox1.Controls.Add(this.groupBox3);
            this.groupBox1.Controls.Add(this.num30DaysLimit);
            this.groupBox1.Controls.Add(this.numDailyLimit);
            this.groupBox1.Controls.Add(this.ultraLabel3);
            this.groupBox1.Controls.Add(this.ultraLabel2);
            this.groupBox1.Controls.Add(this.ultraLabel1);
            this.groupBox1.Controls.Add(this.ultraLabel11);
            this.groupBox1.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.groupBox1.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(10, 41);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(575, 310);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            // 
            // chkPSE
            // 
            appearance6.ForeColor = System.Drawing.Color.White;
            appearance6.TextVAlignAsString = "Middle";
            this.chkPSE.Appearance = appearance6;
            this.chkPSE.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkPSE.Location = new System.Drawing.Point(12, 222);
            this.chkPSE.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.chkPSE.Name = "chkPSE";
            this.chkPSE.Size = new System.Drawing.Size(214, 20);
            this.chkPSE.TabIndex = 6;
            this.chkPSE.Text = "Is PSE";
            this.chkPSE.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.chkPSE.CheckedChanged += new System.EventHandler(this.chkPSE_CheckedChanged); //PRIMEPOS-3166
            // 
            // chkAgeLimit
            // 
            appearance7.ForeColor = System.Drawing.Color.White;
            appearance7.TextVAlignAsString = "Middle";
            this.chkAgeLimit.Appearance = appearance7;
            this.chkAgeLimit.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkAgeLimit.Location = new System.Drawing.Point(12, 182);
            this.chkAgeLimit.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.chkAgeLimit.Name = "chkAgeLimit";
            this.chkAgeLimit.Size = new System.Drawing.Size(214, 20);
            this.chkAgeLimit.TabIndex = 4;
            this.chkAgeLimit.Text = "Age Limit";
            this.chkAgeLimit.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.chkAgeLimit.CheckedChanged += new System.EventHandler(this.chkAgeLimit_CheckedChanged);
            // 
            // numAgeLimit
            // 
            appearance8.TextVAlignAsString = "Middle";
            this.numAgeLimit.Appearance = appearance8;
            this.numAgeLimit.AutoSize = false;
            this.numAgeLimit.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.numAgeLimit.ButtonStyle = Infragistics.Win.UIElementButtonStyle.VisualStudio2005Button;
            this.numAgeLimit.Enabled = false;
            this.numAgeLimit.FormatString = "0";
            this.numAgeLimit.Location = new System.Drawing.Point(231, 180);
            this.numAgeLimit.MaskClipMode = Infragistics.Win.UltraWinMaskedEdit.MaskMode.Raw;
            this.numAgeLimit.MaskDisplayMode = Infragistics.Win.UltraWinMaskedEdit.MaskMode.Raw;
            this.numAgeLimit.MaskInput = "nnnnnnn";
            this.numAgeLimit.MaxValue = 999999;
            this.numAgeLimit.MinValue = 0;
            this.numAgeLimit.Name = "numAgeLimit";
            this.numAgeLimit.NullText = "0";
            this.numAgeLimit.NumericType = Infragistics.Win.UltraWinEditors.NumericType.Double;
            this.numAgeLimit.Size = new System.Drawing.Size(132, 25);
            this.numAgeLimit.SpinButtonDisplayStyle = Infragistics.Win.ButtonDisplayStyle.Always;
            this.numAgeLimit.TabIndex = 5;
            this.numAgeLimit.TabNavigation = Infragistics.Win.UltraWinMaskedEdit.MaskedEditTabNavigation.NextControl;
            this.numAgeLimit.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // numLimitPeriodQty
            // 
            appearance9.TextVAlignAsString = "Middle";
            this.numLimitPeriodQty.Appearance = appearance9;
            this.numLimitPeriodQty.AutoSize = false;
            this.numLimitPeriodQty.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.numLimitPeriodQty.ButtonStyle = Infragistics.Win.UIElementButtonStyle.VisualStudio2005Button;
            this.numLimitPeriodQty.FormatString = "###.00";
            this.numLimitPeriodQty.Location = new System.Drawing.Point(215, 140);
            this.numLimitPeriodQty.MaskClipMode = Infragistics.Win.UltraWinMaskedEdit.MaskMode.Raw;
            this.numLimitPeriodQty.MaskDisplayMode = Infragistics.Win.UltraWinMaskedEdit.MaskMode.Raw;
            this.numLimitPeriodQty.MaskInput = "{LOC}nnnnnnn.nn";
            this.numLimitPeriodQty.MaxValue = 999999.99D;
            this.numLimitPeriodQty.MinValue = 0;
            this.numLimitPeriodQty.Name = "numLimitPeriodQty";
            this.numLimitPeriodQty.NullText = "0";
            this.numLimitPeriodQty.NumericType = Infragistics.Win.UltraWinEditors.NumericType.Double;
            this.numLimitPeriodQty.Size = new System.Drawing.Size(148, 25);
            this.numLimitPeriodQty.SpinButtonDisplayStyle = Infragistics.Win.ButtonDisplayStyle.Always;
            this.numLimitPeriodQty.TabIndex = 3;
            this.numLimitPeriodQty.TabNavigation = Infragistics.Win.UltraWinMaskedEdit.MaskedEditTabNavigation.NextControl;
            this.numLimitPeriodQty.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // ultraLabel6
            // 
            appearance10.ForeColor = System.Drawing.Color.White;
            appearance10.TextVAlignAsString = "Middle";
            this.ultraLabel6.Appearance = appearance10;
            this.ultraLabel6.Location = new System.Drawing.Point(12, 142);
            this.ultraLabel6.Name = "ultraLabel6";
            this.ultraLabel6.Size = new System.Drawing.Size(135, 20);
            this.ultraLabel6.TabIndex = 97;
            this.ultraLabel6.Text = "Limit Period Qty";
            // 
            // numLimitPeriodDays
            // 
            appearance11.TextVAlignAsString = "Middle";
            this.numLimitPeriodDays.Appearance = appearance11;
            this.numLimitPeriodDays.AutoSize = false;
            this.numLimitPeriodDays.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.numLimitPeriodDays.ButtonStyle = Infragistics.Win.UIElementButtonStyle.VisualStudio2005Button;
            this.numLimitPeriodDays.FormatString = "0";
            this.numLimitPeriodDays.Location = new System.Drawing.Point(215, 100);
            this.numLimitPeriodDays.MaskClipMode = Infragistics.Win.UltraWinMaskedEdit.MaskMode.Raw;
            this.numLimitPeriodDays.MaskDisplayMode = Infragistics.Win.UltraWinMaskedEdit.MaskMode.Raw;
            this.numLimitPeriodDays.MaskInput = "nnnnnnn";
            this.numLimitPeriodDays.MaxValue = 999999;
            this.numLimitPeriodDays.MinValue = 0;
            this.numLimitPeriodDays.Name = "numLimitPeriodDays";
            this.numLimitPeriodDays.NullText = "0";
            this.numLimitPeriodDays.NumericType = Infragistics.Win.UltraWinEditors.NumericType.Double;
            this.numLimitPeriodDays.Size = new System.Drawing.Size(148, 25);
            this.numLimitPeriodDays.SpinButtonDisplayStyle = Infragistics.Win.ButtonDisplayStyle.Always;
            this.numLimitPeriodDays.TabIndex = 2;
            this.numLimitPeriodDays.TabNavigation = Infragistics.Win.UltraWinMaskedEdit.MaskedEditTabNavigation.NextControl;
            this.numLimitPeriodDays.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // ultraLabel5
            // 
            appearance12.ForeColor = System.Drawing.Color.White;
            appearance12.TextVAlignAsString = "Middle";
            this.ultraLabel5.Appearance = appearance12;
            this.ultraLabel5.Location = new System.Drawing.Point(12, 102);
            this.ultraLabel5.Name = "ultraLabel5";
            this.ultraLabel5.Size = new System.Drawing.Size(135, 20);
            this.ultraLabel5.TabIndex = 95;
            this.ultraLabel5.Text = "Limit Period Days";
            // 
            // cmbUOM
            // 
            appearance13.BorderAlpha = Infragistics.Win.Alpha.Opaque;
            appearance13.BorderColor3DBase = System.Drawing.Color.Black;
            appearance13.FontData.BoldAsString = "False";
            appearance13.FontData.ItalicAsString = "False";
            appearance13.FontData.StrikeoutAsString = "False";
            appearance13.FontData.UnderlineAsString = "False";
            appearance13.ForeColor = System.Drawing.Color.Black;
            appearance13.ForeColorDisabled = System.Drawing.Color.Black;
            appearance13.TextVAlignAsString = "Middle";
            this.cmbUOM.Appearance = appearance13;
            this.cmbUOM.AutoSize = false;
            this.cmbUOM.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance14.BackColor = System.Drawing.Color.White;
            appearance14.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(231)))), ((int)(((byte)(230)))));
            appearance14.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance14.BorderAlpha = Infragistics.Win.Alpha.Transparent;
            this.cmbUOM.ButtonAppearance = appearance14;
            this.cmbUOM.DropDownStyle = Infragistics.Win.DropDownStyle.DropDownList;
            valueListItem5.DataValue = "<Select a UOM>";
            valueListItem6.DataValue = "mg";
            valueListItem7.DataValue = "g";
            valueListItem8.DataValue = "ml";
            this.cmbUOM.Items.AddRange(new Infragistics.Win.ValueListItem[] {
            valueListItem5,
            valueListItem6,
            valueListItem7,
            valueListItem8});
            this.cmbUOM.Location = new System.Drawing.Point(215, 60);
            this.cmbUOM.MaxLength = 20;
            this.cmbUOM.Name = "cmbUOM";
            this.cmbUOM.Size = new System.Drawing.Size(148, 25);
            this.cmbUOM.TabIndex = 1;
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.groupBox3.Controls.Add(this.chckObtainID);
            this.groupBox3.Controls.Add(this.chckObtainSign);
            this.groupBox3.Controls.Add(this.chckCanOverride); //PRIMEPOS-3166
            this.groupBox3.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.groupBox3.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox3.Location = new System.Drawing.Point(13, 250);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(552, 53);
            this.groupBox3.TabIndex = 26;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Verification Type";
            // 
            // chckObtainID
            // 
            this.chckObtainID.Location = new System.Drawing.Point(190, 19); //PRIMEPOS-3166
            this.chckObtainID.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.chckObtainID.Name = "chckObtainID";
            this.chckObtainID.Size = new System.Drawing.Size(161, 21); //PRIMEPOS-3166
            this.chckObtainID.TabIndex = 1;
            this.chckObtainID.Text = "Obtain ID";
            this.chckObtainID.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            // 
            // chckObtainSign
            // 
            this.chckObtainSign.Location = new System.Drawing.Point(10, 19);
            this.chckObtainSign.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.chckObtainSign.Name = "chckObtainSign";
            this.chckObtainSign.Size = new System.Drawing.Size(161, 21);
            this.chckObtainSign.TabIndex = 0;
            this.chckObtainSign.Text = "Obtain Signature";
            this.chckObtainSign.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            #region PRIMEPOS-3166
            // 
            // chckCanOverride
            // 
            this.chckCanOverride.Location = new System.Drawing.Point(370, 19);
            this.chckCanOverride.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.chckCanOverride.Name = "chckCanOverride";
            this.chckCanOverride.Size = new System.Drawing.Size(161, 21);
            this.chckCanOverride.TabIndex = 2;
            this.chckCanOverride.Text = "Can Override";
            this.chckCanOverride.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            #endregion
            // 
            // num30DaysLimit
            // 
            appearance15.TextVAlignAsString = "Middle";
            this.num30DaysLimit.Appearance = appearance15;
            this.num30DaysLimit.AutoSize = false;
            this.num30DaysLimit.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.num30DaysLimit.ButtonStyle = Infragistics.Win.UIElementButtonStyle.VisualStudio2005Button;
            this.num30DaysLimit.FormatString = "###.00";
            this.num30DaysLimit.Location = new System.Drawing.Point(531, 140);
            this.num30DaysLimit.MaskClipMode = Infragistics.Win.UltraWinMaskedEdit.MaskMode.Raw;
            this.num30DaysLimit.MaskDisplayMode = Infragistics.Win.UltraWinMaskedEdit.MaskMode.Raw;
            this.num30DaysLimit.MaskInput = "nnnnnn.nn";
            this.num30DaysLimit.MaxValue = 999999;
            this.num30DaysLimit.MinValue = -999999;
            this.num30DaysLimit.Name = "num30DaysLimit";
            this.num30DaysLimit.NullText = "0";
            this.num30DaysLimit.NumericType = Infragistics.Win.UltraWinEditors.NumericType.Double;
            this.num30DaysLimit.Size = new System.Drawing.Size(34, 25);
            this.num30DaysLimit.SpinButtonDisplayStyle = Infragistics.Win.ButtonDisplayStyle.Always;
            this.num30DaysLimit.TabIndex = 4;
            this.num30DaysLimit.TabNavigation = Infragistics.Win.UltraWinMaskedEdit.MaskedEditTabNavigation.NextControl;
            this.num30DaysLimit.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.num30DaysLimit.Visible = false;
            // 
            // numDailyLimit
            // 
            appearance16.TextVAlignAsString = "Middle";
            this.numDailyLimit.Appearance = appearance16;
            this.numDailyLimit.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.numDailyLimit.ButtonStyle = Infragistics.Win.UIElementButtonStyle.VisualStudio2005Button;
            this.numDailyLimit.FormatString = "###.00";
            this.numDailyLimit.Location = new System.Drawing.Point(531, 66);
            this.numDailyLimit.MaskClipMode = Infragistics.Win.UltraWinMaskedEdit.MaskMode.Raw;
            this.numDailyLimit.MaskDisplayMode = Infragistics.Win.UltraWinMaskedEdit.MaskMode.Raw;
            this.numDailyLimit.MaskInput = "nnnnnn.nn";
            this.numDailyLimit.MaxValue = 999999;
            this.numDailyLimit.MinValue = -999999;
            this.numDailyLimit.Name = "numDailyLimit";
            this.numDailyLimit.NullText = "0";
            this.numDailyLimit.NumericType = Infragistics.Win.UltraWinEditors.NumericType.Double;
            this.numDailyLimit.Size = new System.Drawing.Size(34, 23);
            this.numDailyLimit.SpinButtonDisplayStyle = Infragistics.Win.ButtonDisplayStyle.Always;
            this.numDailyLimit.TabIndex = 6;
            this.numDailyLimit.TabNavigation = Infragistics.Win.UltraWinMaskedEdit.MaskedEditTabNavigation.NextControl;
            this.numDailyLimit.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.numDailyLimit.Visible = false;
            // 
            // ultraLabel3
            // 
            appearance17.ForeColor = System.Drawing.Color.White;
            this.ultraLabel3.Appearance = appearance17;
            this.ultraLabel3.Location = new System.Drawing.Point(531, 104);
            this.ultraLabel3.Name = "ultraLabel3";
            this.ultraLabel3.Size = new System.Drawing.Size(34, 23);
            this.ultraLabel3.TabIndex = 11;
            this.ultraLabel3.Text = "30-Days Limit";
            this.ultraLabel3.Visible = false;
            // 
            // ultraLabel2
            // 
            appearance18.ForeColor = System.Drawing.Color.White;
            this.ultraLabel2.Appearance = appearance18;
            this.ultraLabel2.Location = new System.Drawing.Point(13, 104);
            this.ultraLabel2.Name = "ultraLabel2";
            this.ultraLabel2.Size = new System.Drawing.Size(84, 23);
            this.ultraLabel2.TabIndex = 9;
            this.ultraLabel2.Text = "Daily Limit";
            this.ultraLabel2.Visible = false;
            // 
            // ultraLabel1
            // 
            appearance19.ForeColor = System.Drawing.Color.White;
            appearance19.TextVAlignAsString = "Middle";
            this.ultraLabel1.Appearance = appearance19;
            this.ultraLabel1.Location = new System.Drawing.Point(13, 62);
            this.ultraLabel1.Name = "ultraLabel1";
            this.ultraLabel1.Size = new System.Drawing.Size(197, 20);
            this.ultraLabel1.TabIndex = 7;
            this.ultraLabel1.Text = "Unit Of Measurement(UOM)";
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.groupBox2.Controls.Add(this.btnClose);
            this.groupBox2.Controls.Add(this.btnSave);
            this.groupBox2.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.groupBox2.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.Location = new System.Drawing.Point(10, 352);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(575, 59);
            this.groupBox2.TabIndex = 25;
            this.groupBox2.TabStop = false;
            // 
            // frmItemMonitorCategory
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(108)))), ((int)(((byte)(172)))));
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(594, 416);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.lblTransactionType);
            this.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.Name = "frmItemMonitorCategory";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Item Monitor Category";
            this.Activated += new System.EventHandler(this.frmItemMonitorCategory_Activated);
            this.Load += new System.EventHandler(this.frmItemMonitorCategory_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmItemMonitorCategory_KeyDown);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.frmItemMonitorCategory_KeyUp);
            ((System.ComponentModel.ISupportInitialize)(this.txtName)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chkPSE)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkAgeLimit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numAgeLimit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numLimitPeriodQty)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numLimitPeriodDays)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbUOM)).EndInit();
            this.groupBox3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.chckObtainID)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chckObtainSign)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.num30DaysLimit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numDailyLimit)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion Windows Form Designer generated code

        private bool Save()
        {
            try
            {
                logger.Trace("Save() - " + clsPOSDBConstants.Log_Entering);
                oItemMonitorCategoryRow.DailyLimit = Configuration.convertNullToInt(numDailyLimit.Value);
                oItemMonitorCategoryRow.ThirtyDaysLimit = Configuration.convertNullToInt(num30DaysLimit.Value);
                oItemMonitorCategoryRow.LimitPeriodDays = Configuration.convertNullToInt(numLimitPeriodDays.Value);
                oItemMonitorCategoryRow.LimitPeriodQty = Configuration.convertNullToDecimal(numLimitPeriodQty.Value);
                oItemMonitorCategoryRow.AgeLimit = Configuration.convertNullToInt(numAgeLimit.Value);
                oItemMonitorCategoryRow.IsAgeLimit = Configuration.convertNullToBoolean(chkAgeLimit.Checked);
                oItemMonitorCategoryRow.UOM = cmbUOM.SelectedIndex.ToString();
                oItemMonitorCategoryRow.ePSE = Configuration.convertNullToBoolean(chkPSE.Checked); //Sprint-23 - PRIMEPOS-2029 30-Mar-2016 JY Added
                oItemMonitorCategoryRow.canOverrideMonitorItem = Configuration.convertNullToBoolean(chckCanOverride.Checked); //PRIMEPOS-3166

                setVerificationBy();
                oItemMonitorCategory.Persist(oItemMonitorCategoryData);
                logger.Trace("Save() - " + clsPOSDBConstants.Log_Exiting);
                return true;
            }
            catch(Exception exp)
            {
                logger.Fatal(exp, "Save()");
                clsUIHelper.ShowErrorMsg(exp.Message);
                txtName.Focus();
                return false;
            }
        }

        private void btnSave_Click(object sender, System.EventArgs e)
        {
            logger.Trace("btnSave_Click(object sender, System.EventArgs e) - " + clsPOSDBConstants.Log_Entering);
            //Sprint-23 - PRIMEPOS-2029 27-Apr-2016 JY Added
            if (chkPSE.Checked == true && chckObtainID.Checked == false)
            {
                POS_Core_UI.Resources.Message.Display("Customer verification is required for PSE items." + Environment.NewLine + "Please enable Obtain ID checkbox.", "Prime POS", MessageBoxButtons.OK);
                if (chckObtainID.Enabled) chckObtainID.Focus();
                return;
            }
            #region PRIMEPOS-3166
            if (chckCanOverride.Checked == true && chckObtainID.Checked == false)
            {
                POS_Core_UI.Resources.Message.Display("Please enable Obtain ID checkbox." + Environment.NewLine + "To override on non PSE items that are Monitor Item", "Prime POS", MessageBoxButtons.OK);
                if (chckObtainID.Enabled) chckObtainID.Focus();
                return;
            }
            #endregion
            if (Configuration.CPOSSet.UseSigPad == false && Configuration.CPOSSet.PinPadModel.Trim().ToUpper() != "Windows Tablet".Trim().ToUpper() && chckObtainSign.Checked == true)
            {
                POS_Core_UI.Resources.Message.Display("If Use Signature Pad or Windows Tablet settings is disabled, Obtain Signature checkbox should be disabled.", "Prime POS", MessageBoxButtons.OK);
                if (chckObtainSign.Enabled) chckObtainSign.Focus();
                return;
            }

            if (Save())
            {
                IsCanceled = false;
                this.Close();
            }
            logger.Trace("btnSave_Click(object sender, System.EventArgs e) - " + clsPOSDBConstants.Log_Exiting);
        }

        private void txtBoxs_Validate(object sender, System.EventArgs e)
        {
            try
            {
                if(oItemMonitorCategoryRow == null)
                    return;
                Infragistics.Win.UltraWinEditors.UltraTextEditor txtEditor =  (Infragistics.Win.UltraWinEditors.UltraTextEditor) sender;
                switch(txtEditor.Name)
                {
                    case "txtName":
                        oItemMonitorCategoryRow.Description = txtName.Text;
                        break;
                    case "txtUOM":
                        oItemMonitorCategoryRow.UOM = cmbUOM.SelectedIndex.ToString();
                        break;
                }
            }
            catch(Exception exp)
            {
                logger.Fatal(exp, "txtBoxs_Validate(object sender, System.EventArgs e)");
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }

        private void SearchItemMonitorCategory()
        {
            try
            {
                logger.Trace("SearchItemMonitorCategory() - " + clsPOSDBConstants.Log_Entering);
                //frmSearch oSearch = new frmSearch(clsPOSDBConstants.ItemMonitorCategory_tbl);
                frmSearchMain oSearch = new frmSearchMain(true);    //20-Dec-2017 JY Added new reference
                oSearch.SearchTable = clsPOSDBConstants.ItemMonitorCategory_tbl;    //20-Dec-2017 JY Added
                oSearch.ShowDialog(this);
                if(!oSearch.IsCanceled)
                {
                    string strCode=oSearch.SelectedRowID();
                    if(strCode == "")
                        return;

                    Display();
                }
                logger.Trace("SearchItemMonitorCategory() - " + clsPOSDBConstants.Log_Exiting);
            }
            catch(Exception exp)
            {
                logger.Fatal(exp, "SearchItemMonitorCategory()");
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }

        private void Display()
        {
            txtName.Text = oItemMonitorCategoryRow.Description;
            cmbUOM.SelectedIndex = Configuration.convertNullToInt(oItemMonitorCategoryRow.UOM);
            numDailyLimit.Value = oItemMonitorCategoryRow.DailyLimit;
            num30DaysLimit.Value = oItemMonitorCategoryRow.ThirtyDaysLimit;
            this.numLimitPeriodDays.Value = oItemMonitorCategoryRow.LimitPeriodDays;
            this.numLimitPeriodQty.Value = oItemMonitorCategoryRow.LimitPeriodQty;
            this.numAgeLimit.Value = oItemMonitorCategoryRow.AgeLimit;
            chkAgeLimit.Checked = oItemMonitorCategoryRow.IsAgeLimit;
            if(oItemMonitorCategoryRow.VerificationBy == clsVerificationBy.ByID)
            {
                this.chckObtainID.Checked = true;
            }
            else if(oItemMonitorCategoryRow.VerificationBy == clsVerificationBy.BySignature)
            {
                this.chckObtainSign.Checked = true;
            }
            else if(oItemMonitorCategoryRow.VerificationBy == clsVerificationBy.ByBoth)
            {
                this.chckObtainID.Checked = true;
                this.chckObtainSign.Checked = true;
            }
            else
            {
                this.chckObtainID.Checked = false;
                this.chckObtainSign.Checked = false;
            }

            chkPSE.Checked = oItemMonitorCategoryRow.ePSE; //Sprint-23 - PRIMEPOS-2029 30-Mar-2016 JY Added
            chckCanOverride.Checked = oItemMonitorCategoryRow.canOverrideMonitorItem; //PRIMEPOS-3166
        }

        public void Edit(string sID)
        {
            try
            {
                logger.Trace("Edit(string sID) - " + clsPOSDBConstants.Log_Entering);

                oItemMonitorCategoryData = oItemMonitorCategory.Populate(POS_Core.Resources.Configuration.convertNullToInt(sID));
                if(oItemMonitorCategoryData.Tables[0].Rows.Count > 0)
                {
                    oItemMonitorCategoryRow = oItemMonitorCategoryData.ItemMonitorCategory[0];
                    this.Text = "Edit Item Monitor Category";
                    this.lblTransactionType.Text = "Edit Item Monitor Category";
                    if(oItemMonitorCategoryRow != null)
                        Display();
                    isEdit = true;
                }
                logger.Trace("Edit(string sID) - " + clsPOSDBConstants.Log_Exiting);
            }
            catch(Exception exp)
            {
                logger.Fatal(exp, "Edit(string sID)");
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }

        private void SetNew()
        {
            oItemMonitorCategory = new ItemMonitorCategory();
            oItemMonitorCategoryData = new ItemMonitorCategoryData();
            this.Text = "Add Item Monitor Category";
            this.lblTransactionType.Text = "Add Item Monitor Category";
            Clear();
            oItemMonitorCategoryRow = oItemMonitorCategoryData.ItemMonitorCategory.AddRow(0, "", "", "", 0, 0);
        }

        private void Clear()
        {
            txtName.Text = "";
        }

        private void btnNew_Click(object sender, System.EventArgs e)
        {
            try
            {
                SetNew();
            }
            catch(Exception exp)
            {
                logger.Fatal(exp, "btnNew_Click(object sender, System.EventArgs e)");
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }

        private void btnClose_Click(object sender, System.EventArgs e)
        {
            IsCanceled = true;
            this.Close();
        }

        private void frmItemMonitorCategory_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            try
            {
            }
            catch(Exception exp)
            {
                logger.Fatal(exp, "frmItemMonitorCategory_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)");
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }

        private void frmItemMonitorCategory_Load(object sender, System.EventArgs e)
        {
            this.txtName.AfterEnterEditMode += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
            this.txtName.AfterExitEditMode += new System.EventHandler(clsUIHelper.AfterExitEditMode);

            this.cmbUOM.AfterEnterEditMode += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
            this.cmbUOM.AfterExitEditMode += new System.EventHandler(clsUIHelper.AfterExitEditMode);

            this.num30DaysLimit.AfterEnterEditMode += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
            this.num30DaysLimit.AfterExitEditMode += new System.EventHandler(clsUIHelper.AfterExitEditMode);

            this.numDailyLimit.AfterEnterEditMode += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
            this.numDailyLimit.AfterExitEditMode += new System.EventHandler(clsUIHelper.AfterExitEditMode);

            this.numLimitPeriodDays.AfterEnterEditMode += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
            this.numLimitPeriodDays.AfterExitEditMode += new System.EventHandler(clsUIHelper.AfterExitEditMode);

            this.numLimitPeriodQty.AfterEnterEditMode += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
            this.numLimitPeriodQty.AfterExitEditMode += new System.EventHandler(clsUIHelper.AfterExitEditMode);

            if(isEdit == false)
            {
                this.cmbUOM.SelectedIndex = 0;
            }
            IsCanceled = true;
            
            //Sprint-23 - PRIMEPOS-2029 28-Apr-2016 JY Added
            //if (Configuration.CInfo.useNplex == true) 
            //    chkPSE.Enabled = true;
            //else
            //    chkPSE.Enabled = false;

            clsUIHelper.setColorSchecme(this);
        }

        private void frmItemMonitorCategory_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            try
            {
                if(e.KeyData == System.Windows.Forms.Keys.Enter)
                {
                    this.SelectNextControl(this.ActiveControl, true, true, true, true);
                }
            }
            catch(Exception exp)
            {
                logger.Fatal(exp, "frmItemMonitorCategory_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)");
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }

        private void frmItemMonitorCategory_Activated(object sender, System.EventArgs e)
        {
            clsUIHelper.CurrentForm = this;
        }

        private void setVerificationBy()
        {
            if(this.chckObtainSign.Checked == true && this.chckObtainID.Checked == true)
            {
                oItemMonitorCategoryRow.VerificationBy = clsVerificationBy.ByBoth;
            }
            else if(this.chckObtainSign.Checked == true && this.chckObtainID.Checked == false)
            {
                oItemMonitorCategoryRow.VerificationBy = clsVerificationBy.BySignature;
            }
            else if(this.chckObtainSign.Checked == false && this.chckObtainID.Checked == true)
            {
                oItemMonitorCategoryRow.VerificationBy = clsVerificationBy.ByID;
            }
            else
            {
                oItemMonitorCategoryRow.VerificationBy = "";
            }
        }

        private void chkAgeLimit_CheckedChanged(object sender, EventArgs e)
        {
            ChkAgeLimit();
        }

        private void ChkAgeLimit()
        {
            if(chkAgeLimit.Checked)
            {
                numAgeLimit.Enabled = true;
                cmbUOM.Enabled = false;
                numLimitPeriodDays.Enabled = false;
                numLimitPeriodQty.Enabled = false;
            }
            else
            {
                numAgeLimit.Enabled = false;
                cmbUOM.Enabled = true;
                numLimitPeriodDays.Enabled = true;
                numLimitPeriodQty.Enabled = true;
            }
        }

        #region PRIMEPOS-3166
        private void chkPSE_CheckedChanged(object sender, EventArgs e)
        {
            if (chkPSE.Checked)
            {
                chckCanOverride.Checked = false;
                chckCanOverride.Enabled = false;
            }
            else
            {
                chckCanOverride.Enabled = true;
            }
        }
        #endregion
    }

    public class clsVerificationBy
    {
        public  const  string  ByID = "D";
        public  const string BySignature = "S";
        public  const string ByBoth = "B";
    }
}