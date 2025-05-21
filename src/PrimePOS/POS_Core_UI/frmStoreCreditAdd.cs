using Infragistics.Win.UltraWinEditors;
using POS_Core.BusinessRules;
using POS_Core.CommonData;
using POS_Core.CommonData.Rows;
using POS_Core.ErrorLogging;
using POS_Core.Resources;
using System;
using System.Windows.Forms;
using NLog;

namespace POS_Core_UI
{
    /// <summary>
    /// Summary description for frmStoreCreditAdd. // StoreCredit - PRIMEPOS-2747 - NileshJ
    /// </summary>
    public class frmStoreCreditAdd : System.Windows.Forms.Form
    {
        private static ILogger logger = LogManager.GetCurrentClassLogger();
        public bool IsCanceled = true;
        public bool IsSuccess = false;
        public decimal CreditAmount = 0M;
        private StoreCreditData oStoreCreditData = new StoreCreditData();
        private StoreCreditRow oStoreCreditRow;
        private StoreCredit oStoreCredit = new StoreCredit();
        private StoreCreditDetailsData oStoreCreditDetailsData = new StoreCreditDetailsData();
        private StoreCreditDetailsRow oStoreCreditDetailsRow;
        private StoreCreditDetails oStoreCreditDetails = new StoreCreditDetails();
        private Infragistics.Win.Misc.UltraLabel lblCreditAmount;
        private Infragistics.Win.Misc.UltraLabel lblTitle;
        private Infragistics.Win.Misc.UltraButton btnClose;
        private Infragistics.Win.Misc.UltraButton btnSave;
        private System.Windows.Forms.ToolTip toolTip1;
        private Infragistics.Win.Misc.UltraLabel lblCustomerID;
        private System.ComponentModel.IContainer components;
        private Infragistics.Win.UltraWinDataSource.UltraDataSource ultraDataSource1;
        private UltraTextEditor txtTransactionID;
        private Infragistics.Win.Misc.UltraLabel lblTransactionID;
        private UltraTextEditor txtCustomerID;
        private UltraTextEditor txtCreditAmount;
        private TableLayoutPanel tableLayoutPanel1;
        private TableLayoutPanel tableLayoutPanel2;
        private TableLayoutPanel tableLayoutPanel3;
        private TableLayoutPanel tableLayoutPanel5;


        public frmStoreCreditAdd()
        {    
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();
            //
            // TODO: Add any constructor code after InitializeComponent call
            //
        }

        public frmStoreCreditAdd(string customerID, string transactionID, string transactionDate, decimal creditAmount)
        {
            logger.Trace("frmStoreCreditAdd(string customerID, string transactionID, string transactionDate, decimal creditAmount)" + clsPOSDBConstants.Log_Entering);
            InitializeComponent();
            txtCustomerID.Text = customerID;
            txtTransactionID.Text = transactionID;
            txtCreditAmount.Text = creditAmount.ToString();
            logger.Trace("frmStoreCreditAdd(string customerID, string transactionID, string transactionDate, decimal creditAmount)" + clsPOSDBConstants.Log_Exiting);
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmStoreCreditAdd));
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinDataSource.UltraDataColumn ultraDataColumn1 = new Infragistics.Win.UltraWinDataSource.UltraDataColumn("ID");
            Infragistics.Win.UltraWinDataSource.UltraDataColumn ultraDataColumn2 = new Infragistics.Win.UltraWinDataSource.UltraDataColumn("CLCardId");
            Infragistics.Win.UltraWinDataSource.UltraDataColumn ultraDataColumn3 = new Infragistics.Win.UltraWinDataSource.UltraDataColumn("CreatedOn");
            Infragistics.Win.UltraWinDataSource.UltraDataColumn ultraDataColumn4 = new Infragistics.Win.UltraWinDataSource.UltraDataColumn("OldPoints");
            Infragistics.Win.UltraWinDataSource.UltraDataColumn ultraDataColumn5 = new Infragistics.Win.UltraWinDataSource.UltraDataColumn("NewPoints");
            Infragistics.Win.UltraWinDataSource.UltraDataColumn ultraDataColumn6 = new Infragistics.Win.UltraWinDataSource.UltraDataColumn("CreatedBy");
            Infragistics.Win.UltraWinDataSource.UltraDataColumn ultraDataColumn7 = new Infragistics.Win.UltraWinDataSource.UltraDataColumn("Remarks");
            this.lblCreditAmount = new Infragistics.Win.Misc.UltraLabel();
            this.btnClose = new Infragistics.Win.Misc.UltraButton();
            this.btnSave = new Infragistics.Win.Misc.UltraButton();
            this.lblTitle = new Infragistics.Win.Misc.UltraLabel();
            this.txtTransactionID = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.lblTransactionID = new Infragistics.Win.Misc.UltraLabel();
            this.txtCustomerID = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.txtCreditAmount = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.lblCustomerID = new Infragistics.Win.Misc.UltraLabel();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.ultraDataSource1 = new Infragistics.Win.UltraWinDataSource.UltraDataSource(this.components);
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel5 = new System.Windows.Forms.TableLayoutPanel();
            ((System.ComponentModel.ISupportInitialize)(this.txtTransactionID)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCustomerID)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCreditAmount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraDataSource1)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.tableLayoutPanel5.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblCreditAmount
            // 
            appearance1.ForeColor = System.Drawing.Color.White;
            this.lblCreditAmount.Appearance = appearance1;
            this.lblCreditAmount.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCreditAmount.Location = new System.Drawing.Point(3, 39);
            this.lblCreditAmount.Name = "lblCreditAmount";
            this.lblCreditAmount.Size = new System.Drawing.Size(116, 20);
            this.lblCreditAmount.TabIndex = 1;
            this.lblCreditAmount.Text = "Credit Amount $ ";
            // 
            // btnClose
            // 
            appearance2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(62)))), ((int)(((byte)(176)))));
            appearance2.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance2.BorderAlpha = Infragistics.Win.Alpha.Opaque;
            appearance2.BorderColor = System.Drawing.Color.Black;
            appearance2.BorderColor3DBase = System.Drawing.Color.Black;
            appearance2.FontData.BoldAsString = "True";
            appearance2.ForeColor = System.Drawing.Color.White;
            appearance2.Image = ((object)(resources.GetObject("appearance2.Image")));
            this.btnClose.Appearance = appearance2;
            this.btnClose.ButtonStyle = Infragistics.Win.UIElementButtonStyle.OfficeXPToolbarButton;
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(369, 3);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(117, 25);
            this.btnClose.TabIndex = 1;
            this.btnClose.Text = "&Cancel";
            this.btnClose.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.btnClose.UseHotTracking = Infragistics.Win.DefaultableBoolean.True;
            this.btnClose.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // btnSave
            // 
            appearance3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(62)))), ((int)(((byte)(176)))));
            appearance3.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance3.FontData.BoldAsString = "True";
            appearance3.ForeColor = System.Drawing.Color.White;
            appearance3.Image = ((object)(resources.GetObject("appearance3.Image")));
            this.btnSave.Appearance = appearance3;
            this.btnSave.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Flat;
            this.btnSave.Location = new System.Drawing.Point(247, 3);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(116, 25);
            this.btnSave.TabIndex = 0;
            this.btnSave.Text = "&Ok";
            this.btnSave.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.btnSave.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // lblTitle
            // 
            appearance4.ForeColor = System.Drawing.Color.White;
            appearance4.ForeColorDisabled = System.Drawing.Color.White;
            appearance4.TextHAlignAsString = "Center";
            this.lblTitle.Appearance = appearance4;
            this.lblTitle.BackColorInternal = System.Drawing.Color.Transparent;
            this.lblTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitle.Location = new System.Drawing.Point(4, 4);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(495, 27);
            this.lblTitle.TabIndex = 23;
            this.lblTitle.Tag = "Header";
            this.lblTitle.Text = "Add Store Credit";
            // 
            // txtTransactionID
            // 
            this.txtTransactionID.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.txtTransactionID.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtTransactionID.Enabled = false;
            this.txtTransactionID.Location = new System.Drawing.Point(369, 3);
            this.txtTransactionID.MaxLength = 50;
            this.txtTransactionID.Name = "txtTransactionID";
            this.txtTransactionID.Size = new System.Drawing.Size(117, 20);
            this.txtTransactionID.TabIndex = 46;
            this.txtTransactionID.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // lblTransactionID
            // 
            appearance5.ForeColor = System.Drawing.Color.White;
            appearance5.TextVAlignAsString = "Middle";
            this.lblTransactionID.Appearance = appearance5;
            this.lblTransactionID.BackColorInternal = System.Drawing.Color.Transparent;
            this.lblTransactionID.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTransactionID.Location = new System.Drawing.Point(247, 3);
            this.lblTransactionID.Name = "lblTransactionID";
            this.lblTransactionID.Size = new System.Drawing.Size(116, 19);
            this.lblTransactionID.TabIndex = 45;
            this.lblTransactionID.Text = "Transaction ID";
            // 
            // txtCustomerID
            // 
            this.txtCustomerID.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.txtCustomerID.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtCustomerID.Enabled = false;
            this.txtCustomerID.Location = new System.Drawing.Point(125, 3);
            this.txtCustomerID.MaxLength = 50;
            this.txtCustomerID.Name = "txtCustomerID";
            this.txtCustomerID.Size = new System.Drawing.Size(116, 20);
            this.txtCustomerID.TabIndex = 44;
            this.txtCustomerID.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // txtCreditAmount
            // 
            this.txtCreditAmount.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.txtCreditAmount.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtCreditAmount.Location = new System.Drawing.Point(125, 39);
            this.txtCreditAmount.MaxLength = 50;
            this.txtCreditAmount.Name = "txtCreditAmount";
            this.txtCreditAmount.Size = new System.Drawing.Size(116, 20);
            this.txtCreditAmount.TabIndex = 43;
            this.txtCreditAmount.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // lblCustomerID
            // 
            appearance6.ForeColor = System.Drawing.Color.White;
            appearance6.TextVAlignAsString = "Middle";
            this.lblCustomerID.Appearance = appearance6;
            this.lblCustomerID.BackColorInternal = System.Drawing.Color.Transparent;
            this.lblCustomerID.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCustomerID.Location = new System.Drawing.Point(3, 3);
            this.lblCustomerID.Name = "lblCustomerID";
            this.lblCustomerID.Size = new System.Drawing.Size(96, 19);
            this.lblCustomerID.TabIndex = 40;
            this.lblCustomerID.Text = "Customer ID";
            // 
            // ultraDataSource1
            // 
            ultraDataColumn2.DataType = typeof(long);
            ultraDataColumn3.DataType = typeof(System.DateTime);
            ultraDataColumn4.DataType = typeof(decimal);
            ultraDataColumn5.DataType = typeof(decimal);
            this.ultraDataSource1.Band.Columns.AddRange(new object[] {
            ultraDataColumn1,
            ultraDataColumn2,
            ultraDataColumn3,
            ultraDataColumn4,
            ultraDataColumn5,
            ultraDataColumn6,
            ultraDataColumn7});
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel3, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(4, 38);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 62.5F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 37.5F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(495, 127);
            this.tableLayoutPanel1.TabIndex = 24;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 4;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel2.Controls.Add(this.txtTransactionID, 3, 0);
            this.tableLayoutPanel2.Controls.Add(this.lblCustomerID, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.lblTransactionID, 2, 0);
            this.tableLayoutPanel2.Controls.Add(this.txtCustomerID, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.txtCreditAmount, 1, 1);
            this.tableLayoutPanel2.Controls.Add(this.lblCreditAmount, 0, 1);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 2;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(489, 73);
            this.tableLayoutPanel2.TabIndex = 0;
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 4;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel3.Controls.Add(this.btnClose, 3, 0);
            this.tableLayoutPanel3.Controls.Add(this.btnSave, 2, 0);
            this.tableLayoutPanel3.Location = new System.Drawing.Point(3, 82);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 1;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(489, 34);
            this.tableLayoutPanel3.TabIndex = 1;
            // 
            // tableLayoutPanel5
            // 
            this.tableLayoutPanel5.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this.tableLayoutPanel5.ColumnCount = 1;
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel5.Controls.Add(this.lblTitle, 0, 0);
            this.tableLayoutPanel5.Controls.Add(this.tableLayoutPanel1, 0, 1);
            this.tableLayoutPanel5.Location = new System.Drawing.Point(7, 7);
            this.tableLayoutPanel5.Name = "tableLayoutPanel5";
            this.tableLayoutPanel5.RowCount = 2;
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 80F));
            this.tableLayoutPanel5.Size = new System.Drawing.Size(503, 169);
            this.tableLayoutPanel5.TabIndex = 26;
            // 
            // frmStoreCreditAdd
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(108)))), ((int)(((byte)(172)))));
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(517, 182);
            this.ControlBox = false;
            this.Controls.Add(this.tableLayoutPanel5);
            this.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.Name = "frmStoreCreditAdd";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Add Store Credit";
            this.Activated += new System.EventHandler(this.frmStoreCreditAdd_Activated);
            this.Load += new System.EventHandler(this.frmStoreCreditAdd_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmStoreCreditAdd_KeyDown);

            ((System.ComponentModel.ISupportInitialize)(this.txtTransactionID)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCustomerID)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCreditAmount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraDataSource1)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel5.ResumeLayout(false);
            this.ResumeLayout(false);

        }
        #endregion

        private bool Save()
        {
            logger.Trace("Save()" + clsPOSDBConstants.Log_Entering);
            int StoreCreditID = 0;
            try
            {
                if (txtCreditAmount.Text != string.Empty)
                {
                    if (oStoreCreditData != null)
                    {
                        oStoreCreditData.StoreCredit.Rows.Clear();
                    }
                    oStoreCreditData = oStoreCredit.GetByCustomerID(Convert.ToInt32(txtCustomerID.Text));
                    //oStoreCreditRow = oStoreCreditData.StoreCredit.AddRow(0, Convert.ToInt32(txtCustomerID.Text), Convert.ToDecimal(txtCreditAmount.Text), DateTime.Now, Configuration.UserName, 0);
                    if (oStoreCreditData.Tables[0].Rows.Count > 0)
                    {
                        oStoreCreditData.Tables[0].Rows[0]["CreditAmt"] = Convert.ToDecimal(oStoreCreditData.Tables[0].Rows[0]["CreditAmt"]) + Convert.ToDecimal(txtCreditAmount.Text);
                        oStoreCredit.UpdateCreditAmount(oStoreCreditData);
                    }
                    else
                    {
                        oStoreCreditRow = oStoreCreditData.StoreCredit.AddRow(0, Convert.ToInt32(txtCustomerID.Text), Convert.ToDecimal(txtCreditAmount.Text), DateTime.Now, Configuration.UserName, 0);
                        oStoreCredit.Persist(oStoreCreditData);
                        oStoreCreditData = oStoreCredit.GetByCustomerID(Convert.ToInt32(txtCustomerID.Text));
                        StoreCreditID = Convert.ToInt32(oStoreCreditData.Tables[0].Rows[0]["StoreCreditID"].ToString());
                    }
                    if (oStoreCreditDetailsData != null)
                    {
                        oStoreCreditDetailsData.StoreCreditDetails.Rows.Clear();
                    }
                    oStoreCreditDetailsRow = oStoreCreditDetailsData.StoreCreditDetails.AddRow(0, StoreCreditID, Convert.ToInt32(txtCustomerID.Text), Convert.ToInt32(txtTransactionID.Text), Convert.ToDecimal(txtCreditAmount.Text), DateTime.Now, Configuration.UserName);
                    oStoreCreditDetails.Persist(oStoreCreditDetailsData);
                    CreditAmount = Convert.ToDecimal(txtCreditAmount.Text);
                    IsSuccess = true;
                }
            }
            catch (POSExceptions exp)
            {
                clsUIHelper.ShowErrorMsg(exp.ErrMessage);
                logger.Fatal(exp, "Save()");
            }

            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
                logger.Fatal(exp, "Save()");
            }
            logger.Trace("Save()" + clsPOSDBConstants.Log_Exiting);
            return IsSuccess;
        }

        private void btnSave_Click(object sender, System.EventArgs e)
        {
            logger.Trace("btnSave_Click(object sender, System.EventArgs e)" + clsPOSDBConstants.Log_Entering);
            if (Save())
            {
                IsCanceled = false;
                this.Close();
            }
            logger.Trace("btnSave_Click(object sender, System.EventArgs e)" + clsPOSDBConstants.Log_Exiting);
        }


        private void frmStoreCreditAdd_KeyDown(object sender, KeyEventArgs e)
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

     

        private void frmStoreCreditAdd_Load(object sender, EventArgs e)
        {
            clsUIHelper.setColorSchecme(this);
        }

        private void frmStoreCreditAdd_Activated(object sender, EventArgs e)
        {
            clsUIHelper.CurrentForm = this;
        }
    }
}
