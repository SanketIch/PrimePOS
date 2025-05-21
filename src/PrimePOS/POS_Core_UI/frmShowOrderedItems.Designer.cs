namespace POS_Core_UI
{
    partial class frmShowOrderedItems
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
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance7 = new Infragistics.Win.Appearance();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmShowOrderedItems));
            Infragistics.Win.Appearance appearance8 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance11 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance12 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance13 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance14 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinSchedule.CalendarCombo.DateButton dateButton1 = new Infragistics.Win.UltraWinSchedule.CalendarCombo.DateButton();
            Infragistics.Win.Appearance appearance15 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinSchedule.CalendarCombo.DateButton dateButton2 = new Infragistics.Win.UltraWinSchedule.CalendarCombo.DateButton();
            Infragistics.Win.Appearance appearance16 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance17 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance18 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance19 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance10 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance9 = new Infragistics.Win.Appearance();
            this.ultraLblOrderDate = new Infragistics.Win.Misc.UltraLabel();
            this.gridItemDetails = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.btnIncludeSelectedItem = new Infragistics.Win.Misc.UltraButton();
            this.btnExcludeSelectedItem = new Infragistics.Win.Misc.UltraButton();
            this.groupBoxItemGrid = new System.Windows.Forms.GroupBox();
            this.btnSearch = new Infragistics.Win.Misc.UltraButton();
            this.ultraLabel2 = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabel1 = new Infragistics.Win.Misc.UltraLabel();
            this.dtpEndDate = new Infragistics.Win.UltraWinSchedule.UltraCalendarCombo();
            this.dtpStartDate = new Infragistics.Win.UltraWinSchedule.UltraCalendarCombo();
            this.chkSelectAll = new System.Windows.Forms.CheckBox();
            this.ultraTxtEditorNoOfOrdItems = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.ultraLblNoOfOrdItems = new Infragistics.Win.Misc.UltraLabel();
            this.btnClose = new Infragistics.Win.Misc.UltraButton();
            this.lblTransactionType = new Infragistics.Win.Misc.UltraLabel();
            this.lblItemCode = new Infragistics.Win.Misc.UltraLabel();
            this.lblItemName = new Infragistics.Win.Misc.UltraLabel();
            this.txtItemName = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.txtItemCode = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.chkShowAllOrderedItems = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.gridItemDetails)).BeginInit();
            this.groupBoxItemGrid.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dtpEndDate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtpStartDate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraTxtEditorNoOfOrdItems)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtItemName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtItemCode)).BeginInit();
            this.SuspendLayout();
            // 
            // ultraLblOrderDate
            // 
            appearance1.ForeColor = System.Drawing.Color.White;
            this.ultraLblOrderDate.Appearance = appearance1;
            this.ultraLblOrderDate.AutoSize = true;
            this.ultraLblOrderDate.Location = new System.Drawing.Point(213, 147);
            this.ultraLblOrderDate.Name = "ultraLblOrderDate";
            this.ultraLblOrderDate.Size = new System.Drawing.Size(90, 18);
            this.ultraLblOrderDate.TabIndex = 81;
            this.ultraLblOrderDate.Text = "Order Date";
            this.ultraLblOrderDate.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // gridItemDetails
            // 
            this.gridItemDetails.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            appearance2.BackColor = System.Drawing.Color.White;
            this.gridItemDetails.DisplayLayout.Appearance = appearance2;
            this.gridItemDetails.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.True;
            this.gridItemDetails.DisplayLayout.Override.AllowRowFiltering = Infragistics.Win.DefaultableBoolean.False;
            this.gridItemDetails.DisplayLayout.Override.AllowRowSummaries = Infragistics.Win.UltraWinGrid.AllowRowSummaries.False;
            this.gridItemDetails.DisplayLayout.Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.True;
            appearance3.BackColor = System.Drawing.Color.Transparent;
            this.gridItemDetails.DisplayLayout.Override.CardAreaAppearance = appearance3;
            appearance4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(89)))), ((int)(((byte)(135)))), ((int)(((byte)(214)))));
            appearance4.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(7)))), ((int)(((byte)(59)))), ((int)(((byte)(150)))));
            appearance4.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance4.FontData.BoldAsString = "True";
            appearance4.FontData.Name = "Arial";
            appearance4.FontData.SizeInPoints = 10F;
            appearance4.ForeColor = System.Drawing.Color.White;
            appearance4.ThemedElementAlpha = Infragistics.Win.Alpha.Transparent;
            this.gridItemDetails.DisplayLayout.Override.HeaderAppearance = appearance4;
            this.gridItemDetails.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            appearance5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(89)))), ((int)(((byte)(135)))), ((int)(((byte)(214)))));
            appearance5.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(7)))), ((int)(((byte)(59)))), ((int)(((byte)(150)))));
            appearance5.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            this.gridItemDetails.DisplayLayout.Override.RowSelectorAppearance = appearance5;
            this.gridItemDetails.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            appearance6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(251)))), ((int)(((byte)(230)))), ((int)(((byte)(148)))));
            appearance6.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(149)))), ((int)(((byte)(21)))));
            appearance6.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            this.gridItemDetails.DisplayLayout.Override.SelectedRowAppearance = appearance6;
            this.gridItemDetails.Location = new System.Drawing.Point(11, 95);
            this.gridItemDetails.Name = "gridItemDetails";
            this.gridItemDetails.Size = new System.Drawing.Size(960, 260);
            this.gridItemDetails.TabIndex = 7;
            this.gridItemDetails.Click += new System.EventHandler(this.gridItemDetails_Click);
            this.gridItemDetails.DoubleClick += new System.EventHandler(this.gridItemDetails_DoubleClick);
            this.gridItemDetails.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.gridItemDetails_KeyPress);
            // 
            // btnIncludeSelectedItem
            // 
            this.btnIncludeSelectedItem.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            appearance7.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(62)))), ((int)(((byte)(176)))));
            appearance7.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance7.FontData.BoldAsString = "True";
            appearance7.ForeColor = System.Drawing.Color.White;
            appearance7.Image = ((object)(resources.GetObject("appearance7.Image")));
            this.btnIncludeSelectedItem.Appearance = appearance7;
            this.btnIncludeSelectedItem.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            this.btnIncludeSelectedItem.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnIncludeSelectedItem.Location = new System.Drawing.Point(860, 366);
            this.btnIncludeSelectedItem.Name = "btnIncludeSelectedItem";
            this.btnIncludeSelectedItem.Size = new System.Drawing.Size(111, 26);
            this.btnIncludeSelectedItem.TabIndex = 9;
            this.btnIncludeSelectedItem.Text = "Include";
            this.btnIncludeSelectedItem.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnIncludeSelectedItem.Click += new System.EventHandler(this.btnIncludeSelectedItem_Click);
            // 
            // btnExcludeSelectedItem
            // 
            this.btnExcludeSelectedItem.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            appearance8.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(62)))), ((int)(((byte)(176)))));
            appearance8.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance8.FontData.BoldAsString = "True";
            appearance8.ForeColor = System.Drawing.Color.White;
            appearance8.Image = ((object)(resources.GetObject("appearance8.Image")));
            this.btnExcludeSelectedItem.Appearance = appearance8;
            this.btnExcludeSelectedItem.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            this.btnExcludeSelectedItem.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnExcludeSelectedItem.Location = new System.Drawing.Point(732, 366);
            this.btnExcludeSelectedItem.Name = "btnExcludeSelectedItem";
            this.btnExcludeSelectedItem.Size = new System.Drawing.Size(111, 26);
            this.btnExcludeSelectedItem.TabIndex = 8;
            this.btnExcludeSelectedItem.Text = "Exclude";
            this.btnExcludeSelectedItem.UseHotTracking = Infragistics.Win.DefaultableBoolean.False;
            this.btnExcludeSelectedItem.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnExcludeSelectedItem.Visible = false;
            this.btnExcludeSelectedItem.Click += new System.EventHandler(this.btnExcludeSelectedItem_Click);
            // 
            // groupBoxItemGrid
            // 
            this.groupBoxItemGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxItemGrid.Controls.Add(this.chkShowAllOrderedItems);
            this.groupBoxItemGrid.Controls.Add(this.txtItemCode);
            this.groupBoxItemGrid.Controls.Add(this.txtItemName);
            this.groupBoxItemGrid.Controls.Add(this.lblItemName);
            this.groupBoxItemGrid.Controls.Add(this.lblItemCode);
            this.groupBoxItemGrid.Controls.Add(this.btnSearch);
            this.groupBoxItemGrid.Controls.Add(this.ultraLabel2);
            this.groupBoxItemGrid.Controls.Add(this.ultraLabel1);
            this.groupBoxItemGrid.Controls.Add(this.dtpEndDate);
            this.groupBoxItemGrid.Controls.Add(this.dtpStartDate);
            this.groupBoxItemGrid.Controls.Add(this.chkSelectAll);
            this.groupBoxItemGrid.Controls.Add(this.ultraTxtEditorNoOfOrdItems);
            this.groupBoxItemGrid.Controls.Add(this.ultraLblNoOfOrdItems);
            this.groupBoxItemGrid.Controls.Add(this.btnExcludeSelectedItem);
            this.groupBoxItemGrid.Controls.Add(this.btnIncludeSelectedItem);
            this.groupBoxItemGrid.Controls.Add(this.gridItemDetails);
            this.groupBoxItemGrid.Controls.Add(this.ultraLblOrderDate);
            this.groupBoxItemGrid.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBoxItemGrid.ForeColor = System.Drawing.Color.White;
            this.groupBoxItemGrid.Location = new System.Drawing.Point(10, 40);
            this.groupBoxItemGrid.Name = "groupBoxItemGrid";
            this.groupBoxItemGrid.Size = new System.Drawing.Size(980, 404);
            this.groupBoxItemGrid.TabIndex = 6;
            this.groupBoxItemGrid.TabStop = false;
            // 
            // btnSearch
            // 
            appearance11.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance11.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(108)))), ((int)(((byte)(172)))));
            appearance11.FontData.BoldAsString = "True";
            appearance11.ForeColor = System.Drawing.Color.White;
            appearance11.Image = ((object)(resources.GetObject("appearance11.Image")));
            this.btnSearch.Appearance = appearance11;
            this.btnSearch.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            this.btnSearch.Location = new System.Drawing.Point(847, 25);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(124, 28);
            this.btnSearch.TabIndex = 6;
            this.btnSearch.Text = "&Search";
            this.btnSearch.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // ultraLabel2
            // 
            appearance12.ForeColor = System.Drawing.Color.White;
            this.ultraLabel2.Appearance = appearance12;
            this.ultraLabel2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultraLabel2.Location = new System.Drawing.Point(11, 50);
            this.ultraLabel2.Name = "ultraLabel2";
            this.ultraLabel2.Size = new System.Drawing.Size(66, 22);
            this.ultraLabel2.TabIndex = 87;
            this.ultraLabel2.Text = "To Date:";
            // 
            // ultraLabel1
            // 
            appearance13.ForeColor = System.Drawing.Color.White;
            this.ultraLabel1.Appearance = appearance13;
            this.ultraLabel1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultraLabel1.Location = new System.Drawing.Point(11, 21);
            this.ultraLabel1.Name = "ultraLabel1";
            this.ultraLabel1.Size = new System.Drawing.Size(80, 22);
            this.ultraLabel1.TabIndex = 86;
            this.ultraLabel1.Text = "From Date:";
            // 
            // dtpEndDate
            // 
            this.dtpEndDate.AllowNull = false;
            appearance14.FontData.BoldAsString = "False";
            appearance14.FontData.ItalicAsString = "False";
            appearance14.FontData.StrikeoutAsString = "False";
            appearance14.FontData.UnderlineAsString = "False";
            appearance14.ForeColor = System.Drawing.Color.Black;
            appearance14.ForeColorDisabled = System.Drawing.Color.Black;
            this.dtpEndDate.Appearance = appearance14;
            this.dtpEndDate.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Office2003ToolbarButton;
            this.dtpEndDate.DateButtons.Add(dateButton1);
            this.dtpEndDate.Location = new System.Drawing.Point(97, 50);
            this.dtpEndDate.Name = "dtpEndDate";
            this.dtpEndDate.NonAutoSizeHeight = 10;
            this.dtpEndDate.Size = new System.Drawing.Size(132, 22);
            this.dtpEndDate.TabIndex = 2;
            this.dtpEndDate.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.dtpEndDate.Value = new System.DateTime(2004, 5, 25, 0, 0, 0, 0);
            this.dtpEndDate.Leave += new System.EventHandler(this.dtpEndDate_Leave);
            // 
            // dtpStartDate
            // 
            this.dtpStartDate.AllowNull = false;
            appearance15.FontData.BoldAsString = "False";
            appearance15.FontData.ItalicAsString = "False";
            appearance15.FontData.StrikeoutAsString = "False";
            appearance15.FontData.UnderlineAsString = "False";
            appearance15.ForeColor = System.Drawing.Color.Black;
            appearance15.ForeColorDisabled = System.Drawing.Color.Black;
            this.dtpStartDate.Appearance = appearance15;
            this.dtpStartDate.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Office2003ToolbarButton;
            this.dtpStartDate.DateButtons.Add(dateButton2);
            this.dtpStartDate.Location = new System.Drawing.Point(97, 21);
            this.dtpStartDate.Name = "dtpStartDate";
            this.dtpStartDate.NonAutoSizeHeight = 10;
            this.dtpStartDate.Size = new System.Drawing.Size(132, 22);
            this.dtpStartDate.TabIndex = 1;
            this.dtpStartDate.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.dtpStartDate.Value = new System.DateTime(2004, 5, 25, 0, 0, 0, 0);
            this.dtpStartDate.Leave += new System.EventHandler(this.dtpStartDate_Leave);
            // 
            // chkSelectAll
            // 
            this.chkSelectAll.AutoSize = true;
            this.chkSelectAll.Location = new System.Drawing.Point(13, -1);
            this.chkSelectAll.Name = "chkSelectAll";
            this.chkSelectAll.Size = new System.Drawing.Size(216, 21);
            this.chkSelectAll.TabIndex = 0;
            this.chkSelectAll.Text = "Select All Ordered Items";
            this.chkSelectAll.UseVisualStyleBackColor = true;
            this.chkSelectAll.CheckedChanged += new System.EventHandler(this.chkSelectAll_CheckedChanged);
            this.chkSelectAll.Click += new System.EventHandler(this.chkSelectAll_Click);
            // 
            // ultraTxtEditorNoOfOrdItems
            // 
            this.ultraTxtEditorNoOfOrdItems.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            appearance16.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(108)))), ((int)(((byte)(172)))));
            appearance16.ForeColor = System.Drawing.Color.White;
            this.ultraTxtEditorNoOfOrdItems.Appearance = appearance16;
            this.ultraTxtEditorNoOfOrdItems.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(108)))), ((int)(((byte)(172)))));
            this.ultraTxtEditorNoOfOrdItems.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultraTxtEditorNoOfOrdItems.Location = new System.Drawing.Point(155, 370);
            this.ultraTxtEditorNoOfOrdItems.Name = "ultraTxtEditorNoOfOrdItems";
            this.ultraTxtEditorNoOfOrdItems.ReadOnly = true;
            this.ultraTxtEditorNoOfOrdItems.Size = new System.Drawing.Size(100, 24);
            this.ultraTxtEditorNoOfOrdItems.TabIndex = 83;
            // 
            // ultraLblNoOfOrdItems
            // 
            this.ultraLblNoOfOrdItems.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            appearance17.ForeColor = System.Drawing.Color.White;
            this.ultraLblNoOfOrdItems.Appearance = appearance17;
            this.ultraLblNoOfOrdItems.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultraLblNoOfOrdItems.Location = new System.Drawing.Point(11, 370);
            this.ultraLblNoOfOrdItems.Name = "ultraLblNoOfOrdItems";
            this.ultraLblNoOfOrdItems.Size = new System.Drawing.Size(138, 23);
            this.ultraLblNoOfOrdItems.TabIndex = 82;
            this.ultraLblNoOfOrdItems.Text = "No. Of Ordered Items";
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            appearance18.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(62)))), ((int)(((byte)(176)))));
            appearance18.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance18.FontData.BoldAsString = "True";
            appearance18.ForeColor = System.Drawing.Color.White;
            appearance18.Image = ((object)(resources.GetObject("appearance18.Image")));
            this.btnClose.Appearance = appearance18;
            this.btnClose.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClose.Location = new System.Drawing.Point(882, 453);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(109, 29);
            this.btnClose.TabIndex = 10;
            this.btnClose.Text = "&Close";
            this.btnClose.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // lblTransactionType
            // 
            appearance19.ForeColor = System.Drawing.Color.White;
            appearance19.ForeColorDisabled = System.Drawing.Color.Navy;
            appearance19.TextHAlignAsString = "Center";
            this.lblTransactionType.Appearance = appearance19;
            this.lblTransactionType.BackColorInternal = System.Drawing.Color.Transparent;
            this.lblTransactionType.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblTransactionType.Font = new System.Drawing.Font("Microsoft Sans Serif", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTransactionType.Location = new System.Drawing.Point(0, 0);
            this.lblTransactionType.Name = "lblTransactionType";
            this.lblTransactionType.Size = new System.Drawing.Size(1004, 35);
            this.lblTransactionType.TabIndex = 11;
            this.lblTransactionType.Tag = "Header";
            this.lblTransactionType.Text = "Ordered Items";
            this.lblTransactionType.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // lblItemCode
            // 
            appearance10.ForeColor = System.Drawing.Color.White;
            this.lblItemCode.Appearance = appearance10;
            this.lblItemCode.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblItemCode.Location = new System.Drawing.Point(252, 21);
            this.lblItemCode.Name = "lblItemCode";
            this.lblItemCode.Size = new System.Drawing.Size(80, 22);
            this.lblItemCode.TabIndex = 89;
            this.lblItemCode.Text = "Item Code:";
            // 
            // lblItemName
            // 
            appearance9.ForeColor = System.Drawing.Color.White;
            this.lblItemName.Appearance = appearance9;
            this.lblItemName.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblItemName.Location = new System.Drawing.Point(252, 50);
            this.lblItemName.Name = "lblItemName";
            this.lblItemName.Size = new System.Drawing.Size(80, 22);
            this.lblItemName.TabIndex = 90;
            this.lblItemName.Text = "Item Name:";
            // 
            // txtItemName
            // 
            this.txtItemName.Location = new System.Drawing.Point(343, 50);
            this.txtItemName.Multiline = true;
            this.txtItemName.Name = "txtItemName";
            this.txtItemName.Size = new System.Drawing.Size(200, 22);
            this.txtItemName.TabIndex = 4;
            // 
            // txtItemCode
            // 
            this.txtItemCode.Location = new System.Drawing.Point(343, 21);
            this.txtItemCode.Multiline = true;
            this.txtItemCode.Name = "txtItemCode";
            this.txtItemCode.Size = new System.Drawing.Size(200, 22);
            this.txtItemCode.TabIndex = 3;
            // 
            // chkShowAllOrderedItems
            // 
            this.chkShowAllOrderedItems.AutoSize = true;
            this.chkShowAllOrderedItems.Location = new System.Drawing.Point(562, 22);
            this.chkShowAllOrderedItems.Name = "chkShowAllOrderedItems";
            this.chkShowAllOrderedItems.Size = new System.Drawing.Size(214, 21);
            this.chkShowAllOrderedItems.TabIndex = 5;
            this.chkShowAllOrderedItems.Text = "Show All Ordered Items";
            this.chkShowAllOrderedItems.UseVisualStyleBackColor = true;
            // 
            // frmShowOrderedItems
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(108)))), ((int)(((byte)(172)))));
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(1004, 491);
            this.Controls.Add(this.lblTransactionType);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.groupBoxItemGrid);
            this.Name = "frmShowOrderedItems";
            this.Text = "Show Ordered Items";
            this.Load += new System.EventHandler(this.frmShowOrderedItems_Load);
            ((System.ComponentModel.ISupportInitialize)(this.gridItemDetails)).EndInit();
            this.groupBoxItemGrid.ResumeLayout(false);
            this.groupBoxItemGrid.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dtpEndDate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtpStartDate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraTxtEditorNoOfOrdItems)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtItemName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtItemCode)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Infragistics.Win.Misc.UltraLabel ultraLblOrderDate;
        private Infragistics.Win.UltraWinGrid.UltraGrid gridItemDetails;
        private Infragistics.Win.Misc.UltraButton btnIncludeSelectedItem;
        private Infragistics.Win.Misc.UltraButton btnExcludeSelectedItem;
        private System.Windows.Forms.GroupBox groupBoxItemGrid;
        private Infragistics.Win.Misc.UltraButton btnClose;
        private Infragistics.Win.Misc.UltraLabel lblTransactionType;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor ultraTxtEditorNoOfOrdItems;
        private Infragistics.Win.Misc.UltraLabel ultraLblNoOfOrdItems;
        private System.Windows.Forms.CheckBox chkSelectAll;
		private Infragistics.Win.Misc.UltraLabel ultraLabel1;
		private Infragistics.Win.UltraWinSchedule.UltraCalendarCombo dtpEndDate;
		private Infragistics.Win.UltraWinSchedule.UltraCalendarCombo dtpStartDate;
		private Infragistics.Win.Misc.UltraLabel ultraLabel2;
		private Infragistics.Win.Misc.UltraButton btnSearch;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor txtItemCode;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor txtItemName;
        private Infragistics.Win.Misc.UltraLabel lblItemName;
        private Infragistics.Win.Misc.UltraLabel lblItemCode;
        private System.Windows.Forms.CheckBox chkShowAllOrderedItems;
    }
}