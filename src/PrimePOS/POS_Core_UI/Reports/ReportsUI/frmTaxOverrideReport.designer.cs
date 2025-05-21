
namespace POS_Core_UI.Reports.ReportsUI
{
    partial class frmTaxOverrideReport
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmTaxOverrideReport));
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinSchedule.CalendarCombo.DateButton dateButton1 = new Infragistics.Win.UltraWinSchedule.CalendarCombo.DateButton();
            Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinSchedule.CalendarCombo.DateButton dateButton2 = new Infragistics.Win.UltraWinSchedule.CalendarCombo.DateButton();
            Infragistics.Win.ValueListItem valueListItem1 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem2 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem3 = new Infragistics.Win.ValueListItem();
            this.lblTransactionType = new Infragistics.Win.Misc.UltraLabel();
            this.tlpMain = new System.Windows.Forms.TableLayoutPanel();
            this.tlpBottom = new System.Windows.Forms.TableLayoutPanel();
            this.btnClose = new Infragistics.Win.Misc.UltraButton();
            this.btnPrintReport = new Infragistics.Win.Misc.UltraButton();
            this.btnViewReport = new Infragistics.Win.Misc.UltraButton();
            this.tlpDetail = new System.Windows.Forms.TableLayoutPanel();
            this.lblFromDate = new Infragistics.Win.Misc.UltraLabel();
            this.dtpFromDate = new Infragistics.Win.UltraWinSchedule.UltraCalendarCombo();
            this.lblToDate = new Infragistics.Win.Misc.UltraLabel();
            this.dtpToDate = new Infragistics.Win.UltraWinSchedule.UltraCalendarCombo();
            this.ultraLabel1 = new Infragistics.Win.Misc.UltraLabel();
            this.cboTransType = new Infragistics.Win.UltraWinEditors.UltraComboEditor();
            this.panel2 = new System.Windows.Forms.Panel();
            this.rdbOnlyRxItem = new System.Windows.Forms.RadioButton();
            this.rdbOnlyOTCItem = new System.Windows.Forms.RadioButton();
            this.rdbAllItem = new System.Windows.Forms.RadioButton();
            this.lbltransType = new Infragistics.Win.Misc.UltraLabel();
            this.tlpMain.SuspendLayout();
            this.tlpBottom.SuspendLayout();
            this.tlpDetail.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dtpFromDate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtpToDate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboTransType)).BeginInit();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblTransactionType
            // 
            appearance1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(1)))), ((int)(((byte)(68)))), ((int)(((byte)(97)))));
            appearance1.ForeColor = System.Drawing.Color.White;
            appearance1.ForeColorDisabled = System.Drawing.Color.Navy;
            appearance1.TextHAlignAsString = "Left";
            appearance1.TextVAlignAsString = "Middle";
            this.lblTransactionType.Appearance = appearance1;
            this.lblTransactionType.BackColorInternal = System.Drawing.Color.Transparent;
            this.lblTransactionType.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTransactionType.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTransactionType.Location = new System.Drawing.Point(3, 3);
            this.lblTransactionType.Name = "lblTransactionType";
            this.lblTransactionType.Padding = new System.Drawing.Size(20, 0);
            this.lblTransactionType.Size = new System.Drawing.Size(480, 32);
            this.lblTransactionType.TabIndex = 3;
            this.lblTransactionType.Tag = "Header";
            this.lblTransactionType.Text = "Tax Override Report";
            // 
            // tlpMain
            // 
            this.tlpMain.ColumnCount = 1;
            this.tlpMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpMain.Controls.Add(this.lblTransactionType, 0, 0);
            this.tlpMain.Controls.Add(this.tlpBottom, 0, 2);
            this.tlpMain.Controls.Add(this.tlpDetail, 0, 1);
            this.tlpMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpMain.Location = new System.Drawing.Point(0, 0);
            this.tlpMain.Name = "tlpMain";
            this.tlpMain.RowCount = 3;
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 21.11111F));
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 78.88889F));
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 37F));
            this.tlpMain.Size = new System.Drawing.Size(486, 218);
            this.tlpMain.TabIndex = 4;
            // 
            // tlpBottom
            // 
            this.tlpBottom.ColumnCount = 3;
            this.tlpBottom.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpBottom.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpBottom.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 141F));
            this.tlpBottom.Controls.Add(this.btnClose, 2, 0);
            this.tlpBottom.Controls.Add(this.btnPrintReport, 0, 0);
            this.tlpBottom.Controls.Add(this.btnViewReport, 1, 0);
            this.tlpBottom.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpBottom.Location = new System.Drawing.Point(3, 183);
            this.tlpBottom.Name = "tlpBottom";
            this.tlpBottom.RowCount = 1;
            this.tlpBottom.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpBottom.Size = new System.Drawing.Size(480, 32);
            this.tlpBottom.TabIndex = 4;
            // 
            // btnClose
            // 
            appearance2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(62)))), ((int)(((byte)(176)))));
            appearance2.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance2.FontData.BoldAsString = "True";
            appearance2.ForeColor = System.Drawing.Color.White;
            appearance2.Image = ((object)(resources.GetObject("appearance2.Image")));
            this.btnClose.Appearance = appearance2;
            this.btnClose.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(341, 3);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(110, 26);
            this.btnClose.TabIndex = 2;
            this.btnClose.Text = "&Close";
            this.btnClose.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnPrintReport
            // 
            appearance3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(62)))), ((int)(((byte)(176)))));
            appearance3.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance3.FontData.BoldAsString = "True";
            appearance3.ForeColor = System.Drawing.Color.White;
            appearance3.Image = ((object)(resources.GetObject("appearance3.Image")));
            this.btnPrintReport.Appearance = appearance3;
            this.btnPrintReport.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            this.btnPrintReport.Location = new System.Drawing.Point(3, 3);
            this.btnPrintReport.Name = "btnPrintReport";
            this.btnPrintReport.Size = new System.Drawing.Size(110, 26);
            this.btnPrintReport.TabIndex = 1;
            this.btnPrintReport.Text = "&Print";
            this.btnPrintReport.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnPrintReport.Click += new System.EventHandler(this.btnPrintReport_Click);
            // 
            // btnViewReport
            // 
            appearance4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(62)))), ((int)(((byte)(176)))));
            appearance4.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance4.FontData.BoldAsString = "True";
            appearance4.ForeColor = System.Drawing.Color.White;
            appearance4.Image = ((object)(resources.GetObject("appearance4.Image")));
            this.btnViewReport.Appearance = appearance4;
            this.btnViewReport.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            this.btnViewReport.Location = new System.Drawing.Point(172, 3);
            this.btnViewReport.Name = "btnViewReport";
            this.btnViewReport.Size = new System.Drawing.Size(108, 26);
            this.btnViewReport.TabIndex = 0;
            this.btnViewReport.Text = "&View";
            this.btnViewReport.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnViewReport.Click += new System.EventHandler(this.btnViewReport_Click);
            // 
            // tlpDetail
            // 
            this.tlpDetail.ColumnCount = 2;
            this.tlpDetail.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 26.25F));
            this.tlpDetail.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 73.75F));
            this.tlpDetail.Controls.Add(this.lblFromDate, 0, 0);
            this.tlpDetail.Controls.Add(this.dtpFromDate, 1, 0);
            this.tlpDetail.Controls.Add(this.lblToDate, 0, 1);
            this.tlpDetail.Controls.Add(this.dtpToDate, 1, 1);
            this.tlpDetail.Controls.Add(this.ultraLabel1, 0, 2);
            this.tlpDetail.Controls.Add(this.cboTransType, 1, 2);
            this.tlpDetail.Controls.Add(this.panel2, 1, 3);
            this.tlpDetail.Controls.Add(this.lbltransType, 0, 3);
            this.tlpDetail.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpDetail.Location = new System.Drawing.Point(3, 41);
            this.tlpDetail.Name = "tlpDetail";
            this.tlpDetail.RowCount = 5;
            this.tlpDetail.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tlpDetail.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tlpDetail.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tlpDetail.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tlpDetail.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpDetail.Size = new System.Drawing.Size(480, 136);
            this.tlpDetail.TabIndex = 5;
            // 
            // lblFromDate
            // 
            this.lblFromDate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblFromDate.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFromDate.Location = new System.Drawing.Point(3, 3);
            this.lblFromDate.Name = "lblFromDate";
            this.lblFromDate.Size = new System.Drawing.Size(120, 24);
            this.lblFromDate.TabIndex = 51;
            this.lblFromDate.Text = "From Date";
            // 
            // dtpFromDate
            // 
            this.dtpFromDate.AllowNull = false;
            appearance5.FontData.BoldAsString = "False";
            appearance5.FontData.ItalicAsString = "False";
            appearance5.FontData.StrikeoutAsString = "False";
            appearance5.FontData.UnderlineAsString = "False";
            appearance5.ForeColor = System.Drawing.Color.Black;
            appearance5.ForeColorDisabled = System.Drawing.Color.Black;
            this.dtpFromDate.Appearance = appearance5;
            this.dtpFromDate.AutoSize = false;
            this.dtpFromDate.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Office2003ToolbarButton;
            this.dtpFromDate.DateButtons.Add(dateButton1);
            this.dtpFromDate.Location = new System.Drawing.Point(129, 3);
            this.dtpFromDate.Name = "dtpFromDate";
            this.dtpFromDate.NonAutoSizeHeight = 10;
            this.dtpFromDate.Size = new System.Drawing.Size(192, 20);
            this.dtpFromDate.TabIndex = 0;
            this.dtpFromDate.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.dtpFromDate.Value = new System.DateTime(2004, 5, 25, 0, 0, 0, 0);
            // 
            // lblToDate
            // 
            this.lblToDate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblToDate.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblToDate.Location = new System.Drawing.Point(3, 33);
            this.lblToDate.Name = "lblToDate";
            this.lblToDate.Size = new System.Drawing.Size(120, 24);
            this.lblToDate.TabIndex = 53;
            this.lblToDate.Text = "To Date";
            // 
            // dtpToDate
            // 
            this.dtpToDate.AllowNull = false;
            appearance6.FontData.BoldAsString = "False";
            appearance6.FontData.ItalicAsString = "False";
            appearance6.FontData.StrikeoutAsString = "False";
            appearance6.FontData.UnderlineAsString = "False";
            appearance6.ForeColor = System.Drawing.Color.Black;
            appearance6.ForeColorDisabled = System.Drawing.Color.Black;
            this.dtpToDate.Appearance = appearance6;
            this.dtpToDate.AutoSize = false;
            this.dtpToDate.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Office2003ToolbarButton;
            this.dtpToDate.DateButtons.Add(dateButton2);
            this.dtpToDate.Location = new System.Drawing.Point(129, 33);
            this.dtpToDate.Name = "dtpToDate";
            this.dtpToDate.NonAutoSizeHeight = 10;
            this.dtpToDate.Size = new System.Drawing.Size(192, 20);
            this.dtpToDate.TabIndex = 1;
            this.dtpToDate.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.dtpToDate.Value = new System.DateTime(2004, 5, 25, 0, 0, 0, 0);
            // 
            // ultraLabel1
            // 
            this.ultraLabel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ultraLabel1.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultraLabel1.Location = new System.Drawing.Point(3, 63);
            this.ultraLabel1.Name = "ultraLabel1";
            this.ultraLabel1.Size = new System.Drawing.Size(120, 24);
            this.ultraLabel1.TabIndex = 55;
            this.ultraLabel1.Text = "Override Option";
            // 
            // cboTransType
            // 
            this.cboTransType.AutoSize = false;
            this.cboTransType.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.cboTransType.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Office2003ToolbarButton;
            this.cboTransType.DropDownListWidth = -1;
            this.cboTransType.DropDownStyle = Infragistics.Win.DropDownStyle.DropDownList;
            this.cboTransType.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.4F);
            valueListItem1.DataValue = "0";
            valueListItem1.DisplayText = "Both";
            valueListItem2.DataValue = "1";
            valueListItem2.DisplayText = "Tax Override";
            valueListItem3.DataValue = "2";
            valueListItem3.DisplayText = "Tax Override All";
            this.cboTransType.Items.AddRange(new Infragistics.Win.ValueListItem[] {
            valueListItem1,
            valueListItem2,
            valueListItem3});
            this.cboTransType.LimitToList = true;
            this.cboTransType.Location = new System.Drawing.Point(129, 63);
            this.cboTransType.Name = "cboTransType";
            this.cboTransType.Size = new System.Drawing.Size(192, 20);
            this.cboTransType.TabIndex = 2;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.rdbOnlyRxItem);
            this.panel2.Controls.Add(this.rdbOnlyOTCItem);
            this.panel2.Controls.Add(this.rdbAllItem);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(129, 93);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(348, 24);
            this.panel2.TabIndex = 3;
            // 
            // rdbOnlyRxItem
            // 
            this.rdbOnlyRxItem.AutoSize = true;
            this.rdbOnlyRxItem.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rdbOnlyRxItem.Location = new System.Drawing.Point(57, 4);
            this.rdbOnlyRxItem.Name = "rdbOnlyRxItem";
            this.rdbOnlyRxItem.Size = new System.Drawing.Size(70, 17);
            this.rdbOnlyRxItem.TabIndex = 1;
            this.rdbOnlyRxItem.Text = "Only Rx";
            this.rdbOnlyRxItem.UseVisualStyleBackColor = true;
            // 
            // rdbOnlyOTCItem
            // 
            this.rdbOnlyOTCItem.AutoSize = true;
            this.rdbOnlyOTCItem.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rdbOnlyOTCItem.Location = new System.Drawing.Point(147, 4);
            this.rdbOnlyOTCItem.Name = "rdbOnlyOTCItem";
            this.rdbOnlyOTCItem.Size = new System.Drawing.Size(80, 17);
            this.rdbOnlyOTCItem.TabIndex = 2;
            this.rdbOnlyOTCItem.Text = "Only OTC";
            this.rdbOnlyOTCItem.UseVisualStyleBackColor = true;
            // 
            // rdbAllItem
            // 
            this.rdbAllItem.AutoSize = true;
            this.rdbAllItem.Checked = true;
            this.rdbAllItem.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rdbAllItem.Location = new System.Drawing.Point(4, 3);
            this.rdbAllItem.Name = "rdbAllItem";
            this.rdbAllItem.Size = new System.Drawing.Size(39, 17);
            this.rdbAllItem.TabIndex = 0;
            this.rdbAllItem.TabStop = true;
            this.rdbAllItem.Text = "All";
            this.rdbAllItem.UseVisualStyleBackColor = true;
            // 
            // lbltransType
            // 
            this.lbltransType.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbltransType.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbltransType.Location = new System.Drawing.Point(3, 93);
            this.lbltransType.Name = "lbltransType";
            this.lbltransType.Size = new System.Drawing.Size(120, 24);
            this.lbltransType.TabIndex = 64;
            this.lbltransType.Text = "Item Type";
            // 
            // frmTaxOverrideReport
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(486, 218);
            this.Controls.Add(this.tlpMain);
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmTaxOverrideReport";
            this.ShowInTaskbar = false;
            this.Text = "Tax Override Report";
            this.Load += new System.EventHandler(this.frmTaxOverrideReport_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmTaxOverrideReport_KeyDown);
            this.tlpMain.ResumeLayout(false);
            this.tlpBottom.ResumeLayout(false);
            this.tlpDetail.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dtpFromDate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtpToDate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboTransType)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Infragistics.Win.Misc.UltraLabel lblTransactionType;
        private System.Windows.Forms.TableLayoutPanel tlpMain;
        private System.Windows.Forms.TableLayoutPanel tlpBottom;
        private Infragistics.Win.Misc.UltraButton btnClose;
        private Infragistics.Win.Misc.UltraButton btnPrintReport;
        private Infragistics.Win.Misc.UltraButton btnViewReport;
        private System.Windows.Forms.TableLayoutPanel tlpDetail;
        private Infragistics.Win.Misc.UltraLabel lblFromDate;
        private Infragistics.Win.UltraWinSchedule.UltraCalendarCombo dtpFromDate;
        private Infragistics.Win.Misc.UltraLabel lblToDate;
        private Infragistics.Win.UltraWinSchedule.UltraCalendarCombo dtpToDate;
        private Infragistics.Win.Misc.UltraLabel ultraLabel1;
        private Infragistics.Win.UltraWinEditors.UltraComboEditor cboTransType;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.RadioButton rdbOnlyRxItem;
        private System.Windows.Forms.RadioButton rdbOnlyOTCItem;
        private System.Windows.Forms.RadioButton rdbAllItem;
        private Infragistics.Win.Misc.UltraLabel lbltransType;
    }
}