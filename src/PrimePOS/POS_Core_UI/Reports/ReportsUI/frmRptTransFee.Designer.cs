
namespace POS_Core_UI.Reports.ReportsUI
{
    partial class frmRptTransFee
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
            Infragistics.Win.UltraWinSchedule.CalendarCombo.DateButton dateButton5 = new Infragistics.Win.UltraWinSchedule.CalendarCombo.DateButton();
            Infragistics.Win.UltraWinSchedule.CalendarCombo.DateButton dateButton6 = new Infragistics.Win.UltraWinSchedule.CalendarCombo.DateButton();
            Infragistics.Win.Appearance appearance17 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance18 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance19 = new Infragistics.Win.Appearance();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            Infragistics.Win.Appearance appearance20 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance21 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance22 = new Infragistics.Win.Appearance();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmRptTransFee));
            Infragistics.Win.Appearance appearance23 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance24 = new Infragistics.Win.Appearance();
            this.dtpEndDate = new Infragistics.Win.UltraWinSchedule.UltraCalendarCombo();
            this.dtpStartDate = new Infragistics.Win.UltraWinSchedule.UltraCalendarCombo();
            this.lblStartDate = new Infragistics.Win.Misc.UltraLabel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnPayTypeList = new Infragistics.Win.Misc.UltraButton();
            this.txtPaymentType = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.ultraLabel4 = new Infragistics.Win.Misc.UltraLabel();
            this.grpPayTypeList = new System.Windows.Forms.GroupBox();
            this.chkSelectAll = new System.Windows.Forms.CheckBox();
            this.dataGridList = new System.Windows.Forms.DataGridView();
            this.chkBox = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.ultraLabel2 = new Infragistics.Win.Misc.UltraLabel();
            this.cboTransType = new Infragistics.Win.UltraWinEditors.UltraComboEditor();
            this.lblEndDate = new Infragistics.Win.Misc.UltraLabel();
            this.btnPrint = new Infragistics.Win.Misc.UltraButton();
            this.btnClose = new Infragistics.Win.Misc.UltraButton();
            this.btnView = new Infragistics.Win.Misc.UltraButton();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            ((System.ComponentModel.ISupportInitialize)(this.dtpEndDate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtpStartDate)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtPaymentType)).BeginInit();
            this.grpPayTypeList.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboTransType)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // dtpEndDate
            // 
            this.dtpEndDate.AllowNull = false;
            this.dtpEndDate.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Office2003ToolbarButton;
            this.dtpEndDate.DateButtons.Add(dateButton5);
            this.dtpEndDate.Location = new System.Drawing.Point(164, 49);
            this.dtpEndDate.Name = "dtpEndDate";
            this.dtpEndDate.NonAutoSizeHeight = 10;
            this.dtpEndDate.Size = new System.Drawing.Size(123, 22);
            this.dtpEndDate.TabIndex = 1;
            this.dtpEndDate.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.dtpEndDate.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.dtpEndDate.Value = new System.DateTime(2004, 5, 25, 0, 0, 0, 0);
            // 
            // dtpStartDate
            // 
            this.dtpStartDate.AllowNull = false;
            this.dtpStartDate.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Office2003ToolbarButton;
            this.dtpStartDate.DateButtons.Add(dateButton6);
            this.dtpStartDate.Location = new System.Drawing.Point(164, 15);
            this.dtpStartDate.Name = "dtpStartDate";
            this.dtpStartDate.NonAutoSizeHeight = 10;
            this.dtpStartDate.Size = new System.Drawing.Size(123, 22);
            this.dtpStartDate.TabIndex = 0;
            this.dtpStartDate.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.dtpStartDate.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.dtpStartDate.Value = new System.DateTime(2004, 5, 25, 0, 0, 0, 0);
            // 
            // lblStartDate
            // 
            appearance17.ForeColor = System.Drawing.Color.Black;
            this.lblStartDate.Appearance = appearance17;
            this.lblStartDate.AutoSize = true;
            this.lblStartDate.Location = new System.Drawing.Point(10, 17);
            this.lblStartDate.Name = "lblStartDate";
            this.lblStartDate.Size = new System.Drawing.Size(77, 18);
            this.lblStartDate.TabIndex = 20;
            this.lblStartDate.Text = "Start Date";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.grpPayTypeList);
            this.groupBox1.Controls.Add(this.btnPayTypeList);
            this.groupBox1.Controls.Add(this.ultraLabel4);
            this.groupBox1.Controls.Add(this.ultraLabel2);
            this.groupBox1.Controls.Add(this.cboTransType);
            this.groupBox1.Controls.Add(this.dtpEndDate);
            this.groupBox1.Controls.Add(this.dtpStartDate);
            this.groupBox1.Controls.Add(this.lblStartDate);
            this.groupBox1.Controls.Add(this.lblEndDate);
            this.groupBox1.Controls.Add(this.txtPaymentType);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.groupBox1.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(634, 307);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            // 
            // btnPayTypeList
            // 
            appearance18.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(62)))), ((int)(((byte)(176)))));
            appearance18.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance18.FontData.BoldAsString = "True";
            appearance18.ForeColor = System.Drawing.Color.White;
            this.btnPayTypeList.Appearance = appearance18;
            this.btnPayTypeList.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            this.btnPayTypeList.Location = new System.Drawing.Point(458, 14);
            this.btnPayTypeList.Name = "btnPayTypeList";
            this.btnPayTypeList.Size = new System.Drawing.Size(144, 23);
            this.btnPayTypeList.TabIndex = 3;
            this.btnPayTypeList.Text = "Pay Types";
            this.btnPayTypeList.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnPayTypeList.Click += new System.EventHandler(this.btnPayTypeList_Click);
            this.btnPayTypeList.KeyDown += new System.Windows.Forms.KeyEventHandler(this.btnPayTypeList_KeyDown);
            this.btnPayTypeList.KeyUp += new System.Windows.Forms.KeyEventHandler(this.btnPayTypeList_KeyUp);
            // 
            // txtPaymentType
            // 
            this.txtPaymentType.AutoSize = false;
            this.txtPaymentType.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.txtPaymentType.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.4F);
            this.txtPaymentType.Location = new System.Drawing.Point(304, 40);
            this.txtPaymentType.Name = "txtPaymentType";
            this.txtPaymentType.ReadOnly = true;
            this.txtPaymentType.Size = new System.Drawing.Size(294, 20);
            this.txtPaymentType.TabIndex = 47;
            this.txtPaymentType.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.txtPaymentType.EditorButtonClick += new Infragistics.Win.UltraWinEditors.EditorButtonEventHandler(this.txtPaymentType_EditorButtonClick);
            // 
            // ultraLabel4
            // 
            appearance19.ForeColor = System.Drawing.Color.Black;
            this.ultraLabel4.Appearance = appearance19;
            this.ultraLabel4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.4F);
            this.ultraLabel4.Location = new System.Drawing.Point(304, 17);
            this.ultraLabel4.Name = "ultraLabel4";
            this.ultraLabel4.Size = new System.Drawing.Size(145, 16);
            this.ultraLabel4.TabIndex = 48;
            this.ultraLabel4.Text = "Pay Types <Blank = All>";
            // 
            // grpPayTypeList
            // 
            this.grpPayTypeList.Controls.Add(this.chkSelectAll);
            this.grpPayTypeList.Controls.Add(this.dataGridList);
            this.grpPayTypeList.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpPayTypeList.Location = new System.Drawing.Point(468, 35);
            this.grpPayTypeList.Name = "grpPayTypeList";
            this.grpPayTypeList.Size = new System.Drawing.Size(144, 61);
            this.grpPayTypeList.TabIndex = 46;
            this.grpPayTypeList.TabStop = false;
            this.grpPayTypeList.Visible = false;
            // 
            // chkSelectAll
            // 
            this.chkSelectAll.AutoSize = true;
            this.chkSelectAll.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkSelectAll.Location = new System.Drawing.Point(5, 38);
            this.chkSelectAll.Name = "chkSelectAll";
            this.chkSelectAll.Size = new System.Drawing.Size(87, 17);
            this.chkSelectAll.TabIndex = 13;
            this.chkSelectAll.Text = "Select All";
            this.chkSelectAll.UseVisualStyleBackColor = true;
            this.chkSelectAll.CheckedChanged += new System.EventHandler(this.chkSelectAll_CheckedChanged);
            // 
            // dataGridList
            // 
            this.dataGridList.AllowDrop = true;
            this.dataGridList.AllowUserToAddRows = false;
            this.dataGridList.AllowUserToDeleteRows = false;
            this.dataGridList.AllowUserToResizeColumns = false;
            this.dataGridList.AllowUserToResizeRows = false;
            this.dataGridList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridList.ColumnHeadersVisible = false;
            this.dataGridList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.chkBox});
            this.dataGridList.Location = new System.Drawing.Point(1, 10);
            this.dataGridList.Name = "dataGridList";
            this.dataGridList.RowHeadersVisible = false;
            this.dataGridList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridList.Size = new System.Drawing.Size(144, 21);
            this.dataGridList.TabIndex = 12;
            this.dataGridList.Visible = false;
            this.dataGridList.RowsAdded += new System.Windows.Forms.DataGridViewRowsAddedEventHandler(this.dataGridList_RowsAdded);
            // 
            // chkBox
            // 
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
            dataGridViewCellStyle3.NullValue = false;
            this.chkBox.DefaultCellStyle = dataGridViewCellStyle3;
            this.chkBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chkBox.HeaderText = " ";
            this.chkBox.Name = "chkBox";
            this.chkBox.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.chkBox.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.chkBox.Width = 20;
            // 
            // ultraLabel2
            // 
            appearance20.ForeColor = System.Drawing.Color.Black;
            appearance20.TextVAlignAsString = "Middle";
            this.ultraLabel2.Appearance = appearance20;
            this.ultraLabel2.AutoSize = true;
            this.ultraLabel2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.4F);
            this.ultraLabel2.Location = new System.Drawing.Point(8, 86);
            this.ultraLabel2.Name = "ultraLabel2";
            this.ultraLabel2.Size = new System.Drawing.Size(70, 16);
            this.ultraLabel2.TabIndex = 44;
            this.ultraLabel2.Text = "Trans Type";
            // 
            // cboTransType
            // 
            this.cboTransType.AutoSize = false;
            this.cboTransType.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.cboTransType.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Office2003ToolbarButton;
            this.cboTransType.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.4F);
            this.cboTransType.LimitToList = true;
            this.cboTransType.Location = new System.Drawing.Point(164, 84);
            this.cboTransType.Name = "cboTransType";
            this.cboTransType.Size = new System.Drawing.Size(123, 20);
            this.cboTransType.TabIndex = 2;
            // 
            // lblEndDate
            // 
            appearance21.ForeColor = System.Drawing.Color.Black;
            this.lblEndDate.Appearance = appearance21;
            this.lblEndDate.AutoSize = true;
            this.lblEndDate.Location = new System.Drawing.Point(10, 51);
            this.lblEndDate.Name = "lblEndDate";
            this.lblEndDate.Size = new System.Drawing.Size(68, 18);
            this.lblEndDate.TabIndex = 22;
            this.lblEndDate.Text = "End Date";
            // 
            // btnPrint
            // 
            this.btnPrint.Anchor = System.Windows.Forms.AnchorStyles.Right;
            appearance22.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(62)))), ((int)(((byte)(176)))));
            appearance22.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance22.FontData.BoldAsString = "True";
            appearance22.ForeColor = System.Drawing.Color.Black;
            appearance22.Image = ((object)(resources.GetObject("appearance22.Image")));
            this.btnPrint.Appearance = appearance22;
            this.btnPrint.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            this.btnPrint.Location = new System.Drawing.Point(355, 15);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(84, 26);
            this.btnPrint.TabIndex = 2;
            this.btnPrint.Text = "&Print";
            this.btnPrint.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = System.Windows.Forms.AnchorStyles.Right;
            appearance23.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(62)))), ((int)(((byte)(176)))));
            appearance23.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance23.FontData.BoldAsString = "True";
            appearance23.ForeColor = System.Drawing.Color.Black;
            appearance23.Image = ((object)(resources.GetObject("appearance23.Image")));
            this.btnClose.Appearance = appearance23;
            this.btnClose.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(539, 15);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(84, 26);
            this.btnClose.TabIndex = 1;
            this.btnClose.Text = "&Close";
            this.btnClose.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnView
            // 
            this.btnView.Anchor = System.Windows.Forms.AnchorStyles.Right;
            appearance24.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(62)))), ((int)(((byte)(176)))));
            appearance24.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance24.FontData.BoldAsString = "True";
            appearance24.ForeColor = System.Drawing.Color.Black;
            appearance24.Image = ((object)(resources.GetObject("appearance24.Image")));
            this.btnView.Appearance = appearance24;
            this.btnView.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            this.btnView.Location = new System.Drawing.Point(447, 15);
            this.btnView.Name = "btnView";
            this.btnView.Size = new System.Drawing.Size(84, 26);
            this.btnView.TabIndex = 0;
            this.btnView.Text = "&View";
            this.btnView.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnView.Click += new System.EventHandler(this.btnView_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnPrint);
            this.groupBox2.Controls.Add(this.btnClose);
            this.groupBox2.Controls.Add(this.btnView);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox2.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.groupBox2.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.Location = new System.Drawing.Point(0, 307);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(634, 47);
            this.groupBox2.TabIndex = 5;
            this.groupBox2.TabStop = false;
            // 
            // frmRptTransFee
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(634, 361);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.Name = "frmRptTransFee";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Transaction Fee Report";
            this.Load += new System.EventHandler(this.frmRptTransFee_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmRptTransFee_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.dtpEndDate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtpStartDate)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtPaymentType)).EndInit();
            this.grpPayTypeList.ResumeLayout(false);
            this.grpPayTypeList.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboTransType)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private Infragistics.Win.UltraWinSchedule.UltraCalendarCombo dtpEndDate;
        private Infragistics.Win.UltraWinSchedule.UltraCalendarCombo dtpStartDate;
        private Infragistics.Win.Misc.UltraLabel lblStartDate;
        private System.Windows.Forms.GroupBox groupBox1;
        private Infragistics.Win.Misc.UltraLabel lblEndDate;
        private Infragistics.Win.Misc.UltraButton btnPrint;
        private Infragistics.Win.Misc.UltraButton btnClose;
        private Infragistics.Win.Misc.UltraButton btnView;
        private System.Windows.Forms.GroupBox groupBox2;
        private Infragistics.Win.Misc.UltraLabel ultraLabel2;
        private Infragistics.Win.UltraWinEditors.UltraComboEditor cboTransType;
        private Infragistics.Win.Misc.UltraButton btnPayTypeList;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor txtPaymentType;
        private Infragistics.Win.Misc.UltraLabel ultraLabel4;
        private System.Windows.Forms.GroupBox grpPayTypeList;
        private System.Windows.Forms.CheckBox chkSelectAll;
        private System.Windows.Forms.DataGridView dataGridList;
        private System.Windows.Forms.DataGridViewCheckBoxColumn chkBox;
    }
}