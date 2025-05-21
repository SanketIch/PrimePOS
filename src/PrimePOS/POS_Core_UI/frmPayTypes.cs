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

namespace POS_Core_UI
{
    /// <summary>
    /// Author: Shitaljit Thounaojam
    /// For managing new payment types apart from existing payment types.
    /// </summary>
    public class frmPayTypes : System.Windows.Forms.Form
    {
        public bool IsCanceled = true;
        private PayTypeData oPayTypeData = new PayTypeData();
        private PayTypeRow oPayTypeRow;
        private PayType oPayTypes = new PayType();
        private Infragistics.Win.Misc.UltraLabel ultraLabel11;
        private Infragistics.Win.Misc.UltraLabel ultraLabel14;
        private Infragistics.Win.Misc.UltraLabel lblTransactionType;
        private Infragistics.Win.Misc.UltraButton btnClose;
        private Infragistics.Win.Misc.UltraButton btnSave;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ToolTip toolTip1;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor txtPaytypeName;
        public UltraComboEditor cmbPaytypeBehaviour;
        private UltraCheckEditor chkIsHide;
        private UltraCheckEditor chkStopAtRefNo;
        private System.ComponentModel.IContainer components;
        private Infragistics.Win.Misc.UltraLabel lblSortOrder;
        private UltraNumericEditor numSortOrder;
        private bool bEdit = false; //PRIMEPOS-2661 05-Apr-2019 JY Added
        public void Initialize()
        {
            SetNew();
        }

        public frmPayTypes()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();
            try
            {
                Initialize();
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
            Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance7 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance8 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance9 = new Infragistics.Win.Appearance();
            Infragistics.Win.ValueListItem valueListItem1 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem2 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem3 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.Appearance appearance10 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance11 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            this.btnClose = new Infragistics.Win.Misc.UltraButton();
            this.btnSave = new Infragistics.Win.Misc.UltraButton();
            this.lblTransactionType = new Infragistics.Win.Misc.UltraLabel();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.chkStopAtRefNo = new Infragistics.Win.UltraWinEditors.UltraCheckEditor();
            this.chkIsHide = new Infragistics.Win.UltraWinEditors.UltraCheckEditor();
            this.cmbPaytypeBehaviour = new Infragistics.Win.UltraWinEditors.UltraComboEditor();
            this.ultraLabel11 = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabel14 = new Infragistics.Win.Misc.UltraLabel();
            this.txtPaytypeName = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.lblSortOrder = new Infragistics.Win.Misc.UltraLabel();
            this.numSortOrder = new Infragistics.Win.UltraWinEditors.UltraNumericEditor();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chkStopAtRefNo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkIsHide)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbPaytypeBehaviour)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPaytypeName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numSortOrder)).BeginInit();
            this.SuspendLayout();
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            appearance1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(62)))), ((int)(((byte)(176)))));
            appearance1.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance1.FontData.BoldAsString = "True";
            appearance1.ForeColor = System.Drawing.Color.White;
            this.btnClose.Appearance = appearance1;
            this.btnClose.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(364, 20);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(85, 26);
            this.btnClose.TabIndex = 18;
            this.btnClose.Text = "&Cancel";
            this.btnClose.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            appearance2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(62)))), ((int)(((byte)(176)))));
            appearance2.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance2.FontData.BoldAsString = "True";
            appearance2.ForeColor = System.Drawing.Color.White;
            this.btnSave.Appearance = appearance2;
            this.btnSave.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            this.btnSave.Location = new System.Drawing.Point(272, 20);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(85, 26);
            this.btnSave.TabIndex = 17;
            this.btnSave.Text = "&Ok";
            this.btnSave.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
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
            this.lblTransactionType.Font = new System.Drawing.Font("Arial", 20.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTransactionType.Location = new System.Drawing.Point(10, 10);
            this.lblTransactionType.Name = "lblTransactionType";
            this.lblTransactionType.Size = new System.Drawing.Size(456, 40);
            this.lblTransactionType.TabIndex = 23;
            this.lblTransactionType.Tag = "Header";
            this.lblTransactionType.Text = "Paytype  Information";
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.btnClose);
            this.groupBox2.Controls.Add(this.btnSave);
            this.groupBox2.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.groupBox2.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.Location = new System.Drawing.Point(10, 206);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(456, 59);
            this.groupBox2.TabIndex = 25;
            this.groupBox2.TabStop = false;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.numSortOrder);
            this.groupBox1.Controls.Add(this.lblSortOrder);
            this.groupBox1.Controls.Add(this.chkStopAtRefNo);
            this.groupBox1.Controls.Add(this.chkIsHide);
            this.groupBox1.Controls.Add(this.cmbPaytypeBehaviour);
            this.groupBox1.Controls.Add(this.ultraLabel11);
            this.groupBox1.Controls.Add(this.ultraLabel14);
            this.groupBox1.Controls.Add(this.txtPaytypeName);
            this.groupBox1.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.groupBox1.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(12, 58);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(456, 142);
            this.groupBox1.TabIndex = 26;
            this.groupBox1.TabStop = false;
            // 
            // chkStopAtRefNo
            // 
            appearance6.ForeColor = System.Drawing.Color.White;
            this.chkStopAtRefNo.Appearance = appearance6;
            this.chkStopAtRefNo.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkStopAtRefNo.Location = new System.Drawing.Point(17, 103);
            this.chkStopAtRefNo.Name = "chkStopAtRefNo";
            this.chkStopAtRefNo.Size = new System.Drawing.Size(160, 20);
            this.chkStopAtRefNo.TabIndex = 3;
            this.chkStopAtRefNo.Text = "Stop at Ref No";
            this.chkStopAtRefNo.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            // 
            // chkIsHide
            // 
            appearance7.ForeColor = System.Drawing.Color.White;
            this.chkIsHide.Appearance = appearance7;
            this.chkIsHide.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkIsHide.Location = new System.Drawing.Point(17, 72);
            this.chkIsHide.Name = "chkIsHide";
            this.chkIsHide.Size = new System.Drawing.Size(160, 20);
            this.chkIsHide.TabIndex = 2;
            this.chkIsHide.Text = "Hide in Trans";
            this.chkIsHide.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.chkIsHide.CheckedChanged += new System.EventHandler(this.chkIsHide_CheckedChanged);
            // 
            // cmbPaytypeBehaviour
            // 
            appearance8.BorderAlpha = Infragistics.Win.Alpha.Opaque;
            appearance8.BorderColor3DBase = System.Drawing.Color.Black;
            appearance8.FontData.BoldAsString = "False";
            appearance8.FontData.ItalicAsString = "False";
            appearance8.FontData.StrikeoutAsString = "False";
            appearance8.FontData.UnderlineAsString = "False";
            appearance8.ForeColor = System.Drawing.Color.Black;
            appearance8.ForeColorDisabled = System.Drawing.Color.Black;
            this.cmbPaytypeBehaviour.Appearance = appearance8;
            this.cmbPaytypeBehaviour.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance9.BackColor = System.Drawing.Color.White;
            appearance9.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(231)))), ((int)(((byte)(230)))));
            appearance9.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance9.BorderAlpha = Infragistics.Win.Alpha.Transparent;
            this.cmbPaytypeBehaviour.ButtonAppearance = appearance9;
            this.cmbPaytypeBehaviour.DropDownStyle = Infragistics.Win.DropDownStyle.DropDownList;
            valueListItem1.DataValue = "ValueListItem0";
            valueListItem1.DisplayText = "Cash";
            valueListItem1.Tag = "CA";
            valueListItem2.DataValue = "ValueListItem1";
            valueListItem2.DisplayText = "Check";
            valueListItem2.Tag = "CH";
            valueListItem3.DataValue = "ValueListItem2";
            valueListItem3.DisplayText = "Credit Card";
            valueListItem3.Tag = "CC";
            this.cmbPaytypeBehaviour.Items.AddRange(new Infragistics.Win.ValueListItem[] {
            valueListItem1,
            valueListItem2,
            valueListItem3});
            this.cmbPaytypeBehaviour.Location = new System.Drawing.Point(162, 23);
            this.cmbPaytypeBehaviour.MaxLength = 20;
            this.cmbPaytypeBehaviour.Name = "cmbPaytypeBehaviour";
            this.cmbPaytypeBehaviour.Size = new System.Drawing.Size(195, 23);
            this.cmbPaytypeBehaviour.TabIndex = 0;
            this.cmbPaytypeBehaviour.Visible = false;
            // 
            // ultraLabel11
            // 
            appearance10.ForeColor = System.Drawing.Color.White;
            this.ultraLabel11.Appearance = appearance10;
            this.ultraLabel11.Location = new System.Drawing.Point(17, 38);
            this.ultraLabel11.Name = "ultraLabel11";
            this.ultraLabel11.Size = new System.Drawing.Size(123, 23);
            this.ultraLabel11.TabIndex = 3;
            this.ultraLabel11.Text = "Payment Type";
            // 
            // ultraLabel14
            // 
            appearance11.ForeColor = System.Drawing.Color.White;
            this.ultraLabel14.Appearance = appearance11;
            this.ultraLabel14.Location = new System.Drawing.Point(17, 26);
            this.ultraLabel14.Name = "ultraLabel14";
            this.ultraLabel14.Size = new System.Drawing.Size(160, 30);
            this.ultraLabel14.TabIndex = 1;
            this.ultraLabel14.Text = "Paytype  Behaviour";
            this.ultraLabel14.Visible = false;
            // 
            // txtPaytypeName
            // 
            this.txtPaytypeName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtPaytypeName.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.txtPaytypeName.Location = new System.Drawing.Point(162, 38);
            this.txtPaytypeName.MaxLength = 50;
            this.txtPaytypeName.Name = "txtPaytypeName";
            this.txtPaytypeName.Size = new System.Drawing.Size(286, 23);
            this.txtPaytypeName.TabIndex = 1;
            this.txtPaytypeName.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // lblSortOrder
            // 
            appearance5.ForeColor = System.Drawing.Color.White;
            this.lblSortOrder.Appearance = appearance5;
            this.lblSortOrder.Location = new System.Drawing.Point(244, 72);
            this.lblSortOrder.Name = "lblSortOrder";
            this.lblSortOrder.Size = new System.Drawing.Size(83, 20);
            this.lblSortOrder.TabIndex = 4;
            this.lblSortOrder.Text = "SortOrder";
            // 
            // numSortOrder
            // 
            appearance4.FontData.BoldAsString = "False";
            appearance4.FontData.ItalicAsString = "False";
            appearance4.FontData.StrikeoutAsString = "False";
            appearance4.FontData.UnderlineAsString = "False";
            appearance4.ForeColor = System.Drawing.Color.Black;
            appearance4.ForeColorDisabled = System.Drawing.Color.Black;
            this.numSortOrder.Appearance = appearance4;
            this.numSortOrder.AutoSize = false;
            this.numSortOrder.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.numSortOrder.Location = new System.Drawing.Point(337, 72);
            this.numSortOrder.MaskInput = "{LOC}nnnn";
            this.numSortOrder.MaxValue = 1000D;
            this.numSortOrder.MinValue = 0D;
            this.numSortOrder.Name = "numSortOrder";
            this.numSortOrder.Size = new System.Drawing.Size(111, 20);
            this.numSortOrder.TabIndex = 4;
            // 
            // frmPayTypes
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(108)))), ((int)(((byte)(172)))));
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(480, 280);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.lblTransactionType);
            this.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.Name = "frmPayTypes";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Pay Types";
            this.Activated += new System.EventHandler(this.frmPayTypes_Activated);
            this.Load += new System.EventHandler(this.frmPayTypes_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmPayTypes_KeyDown);
            this.groupBox2.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chkStopAtRefNo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkIsHide)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbPaytypeBehaviour)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPaytypeName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numSortOrder)).EndInit();
            this.ResumeLayout(false);

        }
        #endregion
        private bool Save()
        {
            try
            {
                if (bEdit == false)//PRIMEPOS-2661 05-Apr-2019 JY Added condition 
                    oPayTypeRow.PayType = PayTypes.Cheque;

                oPayTypeRow.PayTypeDesc = this.txtPaytypeName.Text.Trim();
                oPayTypeRow.IsHide = this.chkIsHide.Checked;    //Sprint-23 - PRIMEPOS-2255 16-May-2016 JY Added 
                oPayTypeRow.StopAtRefNo = this.chkStopAtRefNo.Checked;  //PRIMEPOS-2309 08-Mar-2019 JY Added
                oPayTypeRow.SortOrder = Configuration.convertNullToInt(this.numSortOrder.Value);    //PRIMEPOS-2966 20-May-2021 JY Added
                oPayTypes.Persist(oPayTypeData);
                return true;
            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
                this.txtPaytypeName.Focus();
                return false;
            }
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
                if (oPayTypeRow == null)
                    return;
                Infragistics.Win.UltraWinEditors.UltraTextEditor txtEditor = (Infragistics.Win.UltraWinEditors.UltraTextEditor)sender;
                switch (txtEditor.Name)
                {
                    case "txtName":
                        oPayTypeRow.PayTypeDesc = txtPaytypeName.Text;
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
                if (oPayTypeRow == null)
                    return;
                Infragistics.Win.UltraWinEditors.UltraNumericEditor txtEditor = (Infragistics.Win.UltraWinEditors.UltraNumericEditor)sender;
                switch (txtEditor.Name)
                {
                    case "txtTax":
                        //oPayTypeRow.Amount= Resources.Configuration.convertNullToDecimal(this.txtTax.Value.ToString());
                        break;
                }
            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }

        private void SearchTaxCode()
        {
            try
            {
                //frmSearch oSearch = new frmSearch(clsPOSDBConstants.TaxCodes_tbl);
                frmSearchMain oSearch = new frmSearchMain(true);    //20-Dec-2017 JY Added new reference
                oSearch.SearchTable = clsPOSDBConstants.TaxCodes_tbl;    //20-Dec-2017 JY Added
                oSearch.ShowDialog(this);
                if (!oSearch.IsCanceled)
                {
                    string strCode = oSearch.SelectedRowID();
                    if (strCode == "")
                        return;

                    Display();
                }
            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }
        private void SetComboBehaviourValue(string sBehaviour)
        {
            switch (sBehaviour)
            {
                case PayTypes.Cash:
                    cmbPaytypeBehaviour.SelectedIndex = 0;
                    break;
                case PayTypes.Cheque:
                    cmbPaytypeBehaviour.SelectedIndex = 1;
                    break;
                case PayTypes.CreditCard:
                    cmbPaytypeBehaviour.SelectedIndex = 2;
                    break;
            }
        }
        private void Display()
        {
            SetComboBehaviourValue(PayTypes.Cheque);
            txtPaytypeName.Text = oPayTypeRow.PayTypeDesc;
            chkIsHide.Checked = POS_Core.Resources.Configuration.convertNullToBoolean(oPayTypeRow.IsHide);   //Sprint-23 - PRIMEPOS-2255 16-May-2016 JY Added 
            chkStopAtRefNo.Checked = POS_Core.Resources.Configuration.convertNullToBoolean(oPayTypeRow.StopAtRefNo); //PRIMEPOS-2309 08-Mar-2019 JY Added
            numSortOrder.Value = Configuration.convertNullToInt(oPayTypeRow.SortOrder); //PRIMEPOS-2966 20-May-2021 JY Added
        }

        public void Edit(string PaytypeID)
        {
            try
            {
                bEdit = true;   //PRIMEPOS-2661 05-Apr-2019 JY Added
                oPayTypeData = oPayTypes.Populate(PaytypeID);
                oPayTypeRow = oPayTypeData.PayTypes.GetRowByID(PaytypeID);
                this.Text = "Edit Payment Type";
                this.lblTransactionType.Text = "Edit Payment Type";
                if (oPayTypeRow != null)
                {
                    Display();
                }
            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }

        private void SetNew()
        {
            oPayTypes = new PayType();
            oPayTypeData = new PayTypeData();
            this.Text = "Add Payment Type";
            this.lblTransactionType.Text = "Add Payment Type";
            Clear();
            int nSortOrder = oPayTypes.GetNextSortOrder();  //PRIMEPOS-2966 20-May-2021 JY Added
            oPayTypeRow = oPayTypeData.PayTypes.AddRow("", "", "", "", false, false, nSortOrder);  //Sprint-23 - PRIMEPOS-2255 16-May-2016 JY Added      //PRIMEPOS-2309 08-Mar-2019 JY Added StopAtRefNo to false    //PRIMEPOS-2966 20-May-2021 JY Added SortOrder
        }

        private void Clear()
        {
            txtPaytypeName.Text = "";
            chkIsHide.Checked = false; //Sprint-23 - PRIMEPOS-2255 16-May-2016 JY Added 
            chkStopAtRefNo.Checked = false; //PRIMEPOS-2309 08-Mar-2019 JY Added
            numSortOrder.Value = 0; //PRIMEPOS-2966 20-May-2021 JY Added
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


        private void frmPayTypes_Load(object sender, System.EventArgs e)
        {
            #region Sprint-23 - PRIMEPOS-2255 18-May-2016 JY Added to restrict user from updating description of default paytype '1','2','3','4','5','6','7','B','C','E','H'
            if (oPayTypeRow != null)
            {
                if (oPayTypeRow.PaytypeID.Trim().ToUpper().Trim().ToUpper() == "1" || oPayTypeRow.PaytypeID.Trim().ToUpper() == "2" || oPayTypeRow.PaytypeID.Trim().ToUpper() == "3" || oPayTypeRow.PaytypeID.Trim().ToUpper() == "4" || oPayTypeRow.PaytypeID.Trim().ToUpper() == "5" || oPayTypeRow.PaytypeID.Trim().ToUpper() == "6" || oPayTypeRow.PaytypeID.Trim().ToUpper() == "7"
                    || oPayTypeRow.PaytypeID.Trim().ToUpper() == "B" || oPayTypeRow.PaytypeID.Trim().ToUpper() == "C" || oPayTypeRow.PaytypeID.Trim().ToUpper() == "E" || oPayTypeRow.PaytypeID.Trim().ToUpper() == "H")
                    txtPaytypeName.Enabled = false;
                else
                    txtPaytypeName.Enabled = true;
            }
            #endregion

            #region PRIMEPOS-2309 26-Mar-2019 JY Added to restrict user to update "Stop at ref"
            if (oPayTypeRow != null)
            {
                if (oPayTypeRow.PaytypeID.Trim().ToUpper().Trim().ToUpper() == "1" || oPayTypeRow.PaytypeID.Trim().ToUpper() == "3" || oPayTypeRow.PaytypeID.Trim().ToUpper() == "4" || oPayTypeRow.PaytypeID.Trim().ToUpper() == "5" || oPayTypeRow.PaytypeID.Trim().ToUpper() == "6" || oPayTypeRow.PaytypeID.Trim().ToUpper() == "7"
                    || oPayTypeRow.PaytypeID.Trim().ToUpper() == "B" || oPayTypeRow.PaytypeID.Trim().ToUpper() == "E" || oPayTypeRow.PaytypeID.Trim().ToUpper() == "H")
                    chkStopAtRefNo.Enabled = false;
                else
                    chkStopAtRefNo.Enabled = true;
            }
            #endregion

            if (oPayTypeRow != null)    this.numSortOrder.Value = oPayTypeRow.SortOrder;    //PRIMEPOS-2966 20-May-2021 JY Added

            this.txtPaytypeName.AfterEnterEditMode += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
            this.txtPaytypeName.AfterExitEditMode += new System.EventHandler(clsUIHelper.AfterExitEditMode);

            this.numSortOrder.AfterEnterEditMode += new System.EventHandler(clsUIHelper.AfterEnterEditMode);    //PRIMEPOS-2966 20-May-2021 JY Added
            this.numSortOrder.AfterExitEditMode += new System.EventHandler(clsUIHelper.AfterExitEditMode);  //PRIMEPOS-2966 20-May-2021 JY Added
            IsCanceled = true;

            clsUIHelper.setColorSchecme(this);
        }
        private void frmPayTypes_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
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

        private void frmPayTypes_Activated(object sender, System.EventArgs e)
        {
            clsUIHelper.CurrentForm = this;
        }

        #region  PRIMEPOS-2512 05-Oct-2020 JY Added
        private void chkIsHide_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                string strReturn = string.Empty;
                if (bEdit)
                {
                    if (Configuration.CSetting.DefaultPaytype != "" && Configuration.CSetting.DefaultPaytype.Trim().ToUpper() == oPayTypeRow.PaytypeID.Trim().ToUpper())
                    {
                        Resources.Message.Display("This is a default payment type that is used to set default focus on the payment screen, so you couldn't hide it.", "Paytype Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        chkIsHide.Checked = false;
                    }
                }
            }
            catch { }
        }
        #endregion
    }
}
