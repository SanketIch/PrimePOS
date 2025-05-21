namespace POS_Core_UI
{
    partial class frmColorSchemeForViewPOSTrans
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
            this.components = new System.ComponentModel.Container();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand1 = new Infragistics.Win.UltraWinGrid.UltraGridBand("Band 0", -1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn1 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("ID");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn2 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("FromAmount");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn3 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("ToAmount");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn4 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("BackColor");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn5 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("ForeColor");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn6 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("UserID", -1, null, 0, Infragistics.Win.UltraWinGrid.SortIndicator.Ascending, false);
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance7 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance8 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance9 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance10 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance11 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance12 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance13 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance28 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance29 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinDataSource.UltraDataColumn ultraDataColumn1 = new Infragistics.Win.UltraWinDataSource.UltraDataColumn("ID");
            Infragistics.Win.UltraWinDataSource.UltraDataColumn ultraDataColumn2 = new Infragistics.Win.UltraWinDataSource.UltraDataColumn("FromAmount");
            Infragistics.Win.UltraWinDataSource.UltraDataColumn ultraDataColumn3 = new Infragistics.Win.UltraWinDataSource.UltraDataColumn("ToAmount");
            Infragistics.Win.UltraWinDataSource.UltraDataColumn ultraDataColumn4 = new Infragistics.Win.UltraWinDataSource.UltraDataColumn("BackColor");
            Infragistics.Win.UltraWinDataSource.UltraDataColumn ultraDataColumn5 = new Infragistics.Win.UltraWinDataSource.UltraDataColumn("ForeColor");
            Infragistics.Win.UltraWinDataSource.UltraDataColumn ultraDataColumn6 = new Infragistics.Win.UltraWinDataSource.UltraDataColumn("UserID");
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmColorSchemeForViewPOSTrans));
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance45 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance46 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance14 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance15 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance47 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance48 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance31 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance32 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinStatusBar.UltraStatusPanel ultraStatusPanel1 = new Infragistics.Win.UltraWinStatusBar.UltraStatusPanel();
            Infragistics.Win.Appearance appearance33 = new Infragistics.Win.Appearance();
            this.cpTextEditor = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.cpColor = new Infragistics.Win.UltraWinEditors.UltraColorPicker();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.grdDetail = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.ultraDataSource1 = new Infragistics.Win.UltraWinDataSource.UltraDataSource(this.components);
            this.lblTransactionType = new Infragistics.Win.Misc.UltraLabel();
            this.btnSave = new Infragistics.Win.Misc.UltraButton();
            this.btnClose = new Infragistics.Win.Misc.UltraButton();
            this.btnDelete = new Infragistics.Win.Misc.UltraButton();
            this.btnEdit = new Infragistics.Win.Misc.UltraButton();
            this.btnNewNote = new Infragistics.Win.Misc.UltraButton();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.grColorScemeDetails = new Infragistics.Win.Misc.UltraGroupBox();
            this.numToAmount = new Infragistics.Win.UltraWinEditors.UltraNumericEditor();
            this.numFromAmount = new Infragistics.Win.UltraWinEditors.UltraNumericEditor();
            this.ultraLabel4 = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabel3 = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabel2 = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabel1 = new Infragistics.Win.Misc.UltraLabel();
            this.cpBackColor = new Infragistics.Win.UltraWinEditors.UltraColorPicker();
            this.cpForeColor = new Infragistics.Win.UltraWinEditors.UltraColorPicker();
            this.sbMain = new Infragistics.Win.UltraWinStatusBar.UltraStatusBar();
            ((System.ComponentModel.ISupportInitialize)(this.cpTextEditor)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cpColor)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdDetail)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraDataSource1)).BeginInit();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grColorScemeDetails)).BeginInit();
            this.grColorScemeDetails.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numToAmount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numFromAmount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cpBackColor)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cpForeColor)).BeginInit();
            this.SuspendLayout();
            // 
            // cpTextEditor
            // 
            this.cpTextEditor.Location = new System.Drawing.Point(179, 18);
            this.cpTextEditor.Name = "cpTextEditor";
            this.cpTextEditor.Size = new System.Drawing.Size(126, 25);
            this.cpTextEditor.TabIndex = 1;
            this.cpTextEditor.Text = "0.00";
            this.cpTextEditor.Visible = false;
            // 
            // cpColor
            // 
            this.cpColor.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            this.cpColor.DisplayStyle = Infragistics.Win.EmbeddableElementDisplayStyle.OfficeXP;
            this.cpColor.Location = new System.Drawing.Point(29, 18);
            this.cpColor.Name = "cpColor";
            this.cpColor.Size = new System.Drawing.Size(144, 25);
            this.cpColor.TabIndex = 30;
            this.cpColor.Text = "Control";
            this.cpColor.Visible = false;
            // 
            // groupBox1
            // 
            this.groupBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.groupBox1.Controls.Add(this.grdDetail);
            this.groupBox1.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.groupBox1.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.ForeColor = System.Drawing.Color.White;
            this.groupBox1.Location = new System.Drawing.Point(20, 194);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(687, 326);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Add / Edit / Delete Color Scheme";
            // 
            // grdDetail
            // 
            appearance4.BackColor = System.Drawing.Color.White;
            appearance4.BackColor2 = System.Drawing.Color.White;
            appearance4.BackColorDisabled = System.Drawing.Color.White;
            appearance4.BackColorDisabled2 = System.Drawing.Color.White;
            appearance4.BackGradientStyle = Infragistics.Win.GradientStyle.None;
            this.grdDetail.DisplayLayout.Appearance = appearance4;
            this.grdDetail.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ResizeAllColumns;
            ultraGridColumn1.Header.VisiblePosition = 0;
            ultraGridColumn1.Hidden = true;
            ultraGridColumn1.Width = 59;
            ultraGridColumn2.EditorControl = this.cpTextEditor;
            ultraGridColumn2.Header.Caption = "From Amount";
            ultraGridColumn2.Header.VisiblePosition = 1;
            ultraGridColumn2.Width = 205;
            ultraGridColumn3.EditorControl = this.cpTextEditor;
            ultraGridColumn3.Header.Caption = "To Amount";
            ultraGridColumn3.Header.VisiblePosition = 2;
            ultraGridColumn3.Width = 155;
            ultraGridColumn4.EditorControl = this.cpColor;
            ultraGridColumn4.Header.Caption = "Back Color";
            ultraGridColumn4.Header.VisiblePosition = 3;
            ultraGridColumn4.Width = 150;
            ultraGridColumn5.EditorControl = this.cpColor;
            ultraGridColumn5.Header.Caption = "Fore Color";
            ultraGridColumn5.Header.FixedHeaderIndicator = Infragistics.Win.UltraWinGrid.FixedHeaderIndicator.None;
            ultraGridColumn5.Header.VisiblePosition = 4;
            ultraGridColumn5.Width = 147;
            ultraGridColumn6.Header.VisiblePosition = 5;
            ultraGridColumn6.Hidden = true;
            ultraGridColumn6.Width = 132;
            ultraGridBand1.Columns.AddRange(new object[] {
            ultraGridColumn1,
            ultraGridColumn2,
            ultraGridColumn3,
            ultraGridColumn4,
            ultraGridColumn5,
            ultraGridColumn6});
            ultraGridBand1.Override.AllowAddNew = Infragistics.Win.UltraWinGrid.AllowAddNew.Yes;
            this.grdDetail.DisplayLayout.BandsSerializer.Add(ultraGridBand1);
            this.grdDetail.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.grdDetail.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance5.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance5.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance5.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance5.BorderColor = System.Drawing.SystemColors.Window;
            this.grdDetail.DisplayLayout.GroupByBox.Appearance = appearance5;
            appearance6.ForeColor = System.Drawing.SystemColors.GrayText;
            this.grdDetail.DisplayLayout.GroupByBox.BandLabelAppearance = appearance6;
            this.grdDetail.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance7.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance7.BackColor2 = System.Drawing.SystemColors.Control;
            appearance7.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance7.ForeColor = System.Drawing.SystemColors.GrayText;
            this.grdDetail.DisplayLayout.GroupByBox.PromptAppearance = appearance7;
            this.grdDetail.DisplayLayout.MaxColScrollRegions = 1;
            this.grdDetail.DisplayLayout.MaxRowScrollRegions = 1;
            appearance8.BackColor = System.Drawing.SystemColors.Window;
            appearance8.ForeColor = System.Drawing.SystemColors.ControlText;
            this.grdDetail.DisplayLayout.Override.ActiveCellAppearance = appearance8;
            appearance9.BackColor = System.Drawing.SystemColors.Highlight;
            appearance9.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.grdDetail.DisplayLayout.Override.ActiveRowAppearance = appearance9;
            this.grdDetail.DisplayLayout.Override.AllowAddNew = Infragistics.Win.UltraWinGrid.AllowAddNew.TemplateOnBottom;
            this.grdDetail.DisplayLayout.Override.AllowColMoving = Infragistics.Win.UltraWinGrid.AllowColMoving.WithinBand;
            this.grdDetail.DisplayLayout.Override.AllowColSizing = Infragistics.Win.UltraWinGrid.AllowColSizing.None;
            this.grdDetail.DisplayLayout.Override.AllowColSwapping = Infragistics.Win.UltraWinGrid.AllowColSwapping.NotAllowed;
            this.grdDetail.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.True;
            this.grdDetail.DisplayLayout.Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.True;
            this.grdDetail.DisplayLayout.Override.BorderStyleCardArea = Infragistics.Win.UIElementBorderStyle.Solid;
            this.grdDetail.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Solid;
            this.grdDetail.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance10.BackColor = System.Drawing.SystemColors.Window;
            this.grdDetail.DisplayLayout.Override.CardAreaAppearance = appearance10;
            appearance11.BorderColor = System.Drawing.Color.Silver;
            appearance11.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.grdDetail.DisplayLayout.Override.CellAppearance = appearance11;
            this.grdDetail.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.grdDetail.DisplayLayout.Override.CellPadding = 0;
            this.grdDetail.DisplayLayout.Override.DefaultRowHeight = 40;
            appearance12.BackColor = System.Drawing.SystemColors.Control;
            appearance12.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance12.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance12.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance12.BorderColor = System.Drawing.SystemColors.Window;
            this.grdDetail.DisplayLayout.Override.GroupByRowAppearance = appearance12;
            appearance13.TextHAlignAsString = "Left";
            this.grdDetail.DisplayLayout.Override.HeaderAppearance = appearance13;
            this.grdDetail.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.grdDetail.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance28.BackColor = System.Drawing.SystemColors.Window;
            appearance28.BorderColor = System.Drawing.Color.Silver;
            this.grdDetail.DisplayLayout.Override.RowAppearance = appearance28;
            this.grdDetail.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            appearance29.BackColor = System.Drawing.SystemColors.ControlLight;
            this.grdDetail.DisplayLayout.Override.TemplateAddRowAppearance = appearance29;
            this.grdDetail.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.grdDetail.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.grdDetail.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grdDetail.Location = new System.Drawing.Point(18, 23);
            this.grdDetail.Name = "grdDetail";
            this.grdDetail.Size = new System.Drawing.Size(659, 294);
            this.grdDetail.TabIndex = 0;
            this.grdDetail.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.grdDetail.Error += new Infragistics.Win.UltraWinGrid.ErrorEventHandler(this.grdDetail_Error);
            this.grdDetail.Validated += new System.EventHandler(this.grdDetail_Validated);
            // 
            // ultraDataSource1
            // 
            this.ultraDataSource1.Band.Columns.AddRange(new object[] {
            ultraDataColumn1,
            ultraDataColumn2,
            ultraDataColumn3,
            ultraDataColumn4,
            ultraDataColumn5,
            ultraDataColumn6});
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
            this.lblTransactionType.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTransactionType.Location = new System.Drawing.Point(22, 2);
            this.lblTransactionType.Name = "lblTransactionType";
            this.lblTransactionType.Size = new System.Drawing.Size(685, 38);
            this.lblTransactionType.TabIndex = 30;
            this.lblTransactionType.Tag = "Header";
            this.lblTransactionType.Text = "Color Scheme For View POS Transaction";
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            appearance2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(62)))), ((int)(((byte)(176)))));
            appearance2.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance2.FontData.BoldAsString = "True";
            appearance2.ForeColor = System.Drawing.Color.White;
            appearance2.Image = ((object)(resources.GetObject("appearance2.Image")));
            this.btnSave.Appearance = appearance2;
            this.btnSave.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            this.btnSave.Location = new System.Drawing.Point(492, 18);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(90, 26);
            this.btnSave.TabIndex = 3;
            this.btnSave.Text = "&Save";
            this.btnSave.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            appearance1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(62)))), ((int)(((byte)(176)))));
            appearance1.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance1.FontData.BoldAsString = "True";
            appearance1.ForeColor = System.Drawing.Color.White;
            appearance1.Image = ((object)(resources.GetObject("appearance1.Image")));
            this.btnClose.Appearance = appearance1;
            this.btnClose.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(590, 18);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(90, 26);
            this.btnClose.TabIndex = 4;
            this.btnClose.Text = "&Close";
            this.btnClose.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            appearance45.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(62)))), ((int)(((byte)(176)))));
            appearance45.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance45.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance45.FontData.BoldAsString = "True";
            appearance45.ForeColor = System.Drawing.Color.Black;
            this.btnDelete.Appearance = appearance45;
            this.btnDelete.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Flat;
            appearance46.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(188)))), ((int)(((byte)(193)))), ((int)(((byte)(207)))));
            appearance46.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(188)))), ((int)(((byte)(193)))), ((int)(((byte)(207)))));
            this.btnDelete.HotTrackAppearance = appearance46;
            this.btnDelete.Location = new System.Drawing.Point(279, 18);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(120, 26);
            this.btnDelete.TabIndex = 2;
            this.btnDelete.Text = "&Delete(F11)";
            this.btnDelete.UseHotTracking = Infragistics.Win.DefaultableBoolean.True;
            this.btnDelete.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnEdit
            // 
            this.btnEdit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            appearance14.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(62)))), ((int)(((byte)(176)))));
            appearance14.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance14.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance14.FontData.BoldAsString = "True";
            appearance14.ForeColor = System.Drawing.Color.Black;
            this.btnEdit.Appearance = appearance14;
            this.btnEdit.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Flat;
            appearance15.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(188)))), ((int)(((byte)(193)))), ((int)(((byte)(207)))));
            appearance15.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(188)))), ((int)(((byte)(193)))), ((int)(((byte)(207)))));
            this.btnEdit.HotTrackAppearance = appearance15;
            this.btnEdit.Location = new System.Drawing.Point(150, 17);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(123, 26);
            this.btnEdit.TabIndex = 1;
            this.btnEdit.Text = "&Edit(F3)";
            this.btnEdit.UseHotTracking = Infragistics.Win.DefaultableBoolean.True;
            this.btnEdit.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
            // 
            // btnNewNote
            // 
            this.btnNewNote.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            appearance47.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(62)))), ((int)(((byte)(176)))));
            appearance47.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance47.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance47.FontData.BoldAsString = "True";
            appearance47.ForeColor = System.Drawing.Color.Black;
            this.btnNewNote.Appearance = appearance47;
            this.btnNewNote.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Flat;
            appearance48.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(188)))), ((int)(((byte)(193)))), ((int)(((byte)(207)))));
            appearance48.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(188)))), ((int)(((byte)(193)))), ((int)(((byte)(207)))));
            this.btnNewNote.HotTrackAppearance = appearance48;
            this.btnNewNote.Location = new System.Drawing.Point(18, 17);
            this.btnNewNote.Name = "btnNewNote";
            this.btnNewNote.Size = new System.Drawing.Size(126, 26);
            this.btnNewNote.TabIndex = 0;
            this.btnNewNote.Text = "&Add New (F1)";
            this.btnNewNote.UseHotTracking = Infragistics.Win.DefaultableBoolean.True;
            this.btnNewNote.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnNewNote.Click += new System.EventHandler(this.btnNewNote_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.groupBox2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.groupBox2.Controls.Add(this.btnNewNote);
            this.groupBox2.Controls.Add(this.btnEdit);
            this.groupBox2.Controls.Add(this.btnDelete);
            this.groupBox2.Controls.Add(this.cpTextEditor);
            this.groupBox2.Controls.Add(this.cpColor);
            this.groupBox2.Controls.Add(this.btnClose);
            this.groupBox2.Controls.Add(this.btnSave);
            this.groupBox2.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.groupBox2.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.Location = new System.Drawing.Point(20, 133);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(685, 54);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            // 
            // grColorScemeDetails
            // 
            this.grColorScemeDetails.BorderStyle = Infragistics.Win.Misc.GroupBoxBorderStyle.Rectangular3D;
            this.grColorScemeDetails.Controls.Add(this.numToAmount);
            this.grColorScemeDetails.Controls.Add(this.numFromAmount);
            this.grColorScemeDetails.Controls.Add(this.ultraLabel4);
            this.grColorScemeDetails.Controls.Add(this.ultraLabel3);
            this.grColorScemeDetails.Controls.Add(this.ultraLabel2);
            this.grColorScemeDetails.Controls.Add(this.ultraLabel1);
            this.grColorScemeDetails.Controls.Add(this.cpBackColor);
            this.grColorScemeDetails.Controls.Add(this.cpForeColor);
            this.grColorScemeDetails.Enabled = false;
            this.grColorScemeDetails.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grColorScemeDetails.Location = new System.Drawing.Point(20, 36);
            this.grColorScemeDetails.Name = "grColorScemeDetails";
            this.grColorScemeDetails.Size = new System.Drawing.Size(687, 99);
            this.grColorScemeDetails.TabIndex = 0;
            this.grColorScemeDetails.Text = "Color Scheme Details";
            // 
            // numToAmount
            // 
            this.numToAmount.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.numToAmount.DisplayStyle = Infragistics.Win.EmbeddableElementDisplayStyle.OfficeXP;
            this.numToAmount.Location = new System.Drawing.Point(400, 23);
            this.numToAmount.MaskInput = "nn,nnn.nn";
            this.numToAmount.MaxValue = 99999.99;
            this.numToAmount.MinValue = 0;
            this.numToAmount.Name = "numToAmount";
            this.numToAmount.NullText = "0.00";
            this.numToAmount.NumericType = Infragistics.Win.UltraWinEditors.NumericType.Double;
            this.numToAmount.Size = new System.Drawing.Size(153, 23);
            this.numToAmount.SpinButtonDisplayStyle = Infragistics.Win.ButtonDisplayStyle.Always;
            this.numToAmount.SpinWrap = true;
            this.numToAmount.TabIndex = 1;
            this.numToAmount.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.numToAmount.Leave += new System.EventHandler(this.NumBox_Leave);
            // 
            // numFromAmount
            // 
            this.numFromAmount.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.numFromAmount.DisplayStyle = Infragistics.Win.EmbeddableElementDisplayStyle.OfficeXP;
            this.numFromAmount.Location = new System.Drawing.Point(133, 23);
            this.numFromAmount.MaskInput = "nn,nnn.nn";
            this.numFromAmount.MaxValue = 99999.99;
            this.numFromAmount.MinValue = 0;
            this.numFromAmount.Name = "numFromAmount";
            this.numFromAmount.NullText = "0.00";
            this.numFromAmount.NumericType = Infragistics.Win.UltraWinEditors.NumericType.Double;
            this.numFromAmount.Size = new System.Drawing.Size(153, 23);
            this.numFromAmount.SpinButtonDisplayStyle = Infragistics.Win.ButtonDisplayStyle.Always;
            this.numFromAmount.SpinWrap = true;
            this.numFromAmount.TabIndex = 0;
            this.numFromAmount.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.numFromAmount.Leave += new System.EventHandler(this.NumBox_Leave);
            // 
            // ultraLabel4
            // 
            this.ultraLabel4.Location = new System.Drawing.Point(20, 22);
            this.ultraLabel4.Name = "ultraLabel4";
            this.ultraLabel4.Size = new System.Drawing.Size(114, 24);
            this.ultraLabel4.TabIndex = 37;
            this.ultraLabel4.Text = "From Amount";
            // 
            // ultraLabel3
            // 
            this.ultraLabel3.Location = new System.Drawing.Point(290, 23);
            this.ultraLabel3.Name = "ultraLabel3";
            this.ultraLabel3.Size = new System.Drawing.Size(100, 23);
            this.ultraLabel3.TabIndex = 36;
            this.ultraLabel3.Text = "To Amount";
            // 
            // ultraLabel2
            // 
            this.ultraLabel2.Location = new System.Drawing.Point(290, 61);
            this.ultraLabel2.Name = "ultraLabel2";
            this.ultraLabel2.Size = new System.Drawing.Size(100, 25);
            this.ultraLabel2.TabIndex = 35;
            this.ultraLabel2.Text = "Fore Color";
            // 
            // ultraLabel1
            // 
            this.ultraLabel1.Location = new System.Drawing.Point(23, 63);
            this.ultraLabel1.Name = "ultraLabel1";
            this.ultraLabel1.Size = new System.Drawing.Size(100, 23);
            this.ultraLabel1.TabIndex = 34;
            this.ultraLabel1.Text = "Back Color";
            // 
            // cpBackColor
            // 
            this.cpBackColor.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3DOldStyle;
            this.cpBackColor.DisplayStyle = Infragistics.Win.EmbeddableElementDisplayStyle.WindowsVista;
            this.cpBackColor.Location = new System.Drawing.Point(132, 61);
            this.cpBackColor.Name = "cpBackColor";
            this.cpBackColor.Size = new System.Drawing.Size(153, 25);
            this.cpBackColor.TabIndex = 2;
            this.cpBackColor.Text = "Control";
            this.cpBackColor.Leave += new System.EventHandler(this.cpColor_Leave);
            // 
            // cpForeColor
            // 
            this.cpForeColor.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3DOldStyle;
            this.cpForeColor.DisplayStyle = Infragistics.Win.EmbeddableElementDisplayStyle.WindowsVista;
            this.cpForeColor.Location = new System.Drawing.Point(400, 61);
            this.cpForeColor.Name = "cpForeColor";
            this.cpForeColor.Size = new System.Drawing.Size(153, 25);
            this.cpForeColor.TabIndex = 3;
            this.cpForeColor.Text = "Control";
            this.cpForeColor.Leave += new System.EventHandler(this.cpColor_Leave);
            // 
            // sbMain
            // 
            appearance31.BackColor = System.Drawing.Color.White;
            appearance31.BackColor2 = System.Drawing.SystemColors.Control;
            appearance31.BorderColor = System.Drawing.Color.Black;
            appearance31.FontData.Name = "Verdana";
            appearance31.FontData.SizeInPoints = 10F;
            appearance31.ForeColor = System.Drawing.Color.White;
            this.sbMain.Appearance = appearance31;
            this.sbMain.Location = new System.Drawing.Point(0, 523);
            this.sbMain.Name = "sbMain";
            appearance32.BorderColor = System.Drawing.Color.Black;
            appearance32.BorderColor3DBase = System.Drawing.Color.Black;
            appearance32.ForeColor = System.Drawing.Color.Black;
            this.sbMain.PanelAppearance = appearance32;
            appearance33.BorderColor = System.Drawing.Color.White;
            ultraStatusPanel1.Appearance = appearance33;
            ultraStatusPanel1.SizingMode = Infragistics.Win.UltraWinStatusBar.PanelSizingMode.Spring;
            ultraStatusPanel1.Width = 200;
            this.sbMain.Panels.AddRange(new Infragistics.Win.UltraWinStatusBar.UltraStatusPanel[] {
            ultraStatusPanel1});
            this.sbMain.Size = new System.Drawing.Size(727, 25);
            this.sbMain.TabIndex = 32;
            this.sbMain.ViewStyle = Infragistics.Win.UltraWinStatusBar.ViewStyle.VisualStudio2005;
            // 
            // frmColorSchemeForViewPOSTrans
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.SystemColors.HotTrack;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ClientSize = new System.Drawing.Size(727, 548);
            this.Controls.Add(this.sbMain);
            this.Controls.Add(this.grColorScemeDetails);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.lblTransactionType);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.Name = "frmColorSchemeForViewPOSTrans";
            this.Text = "Color Scheme For View POS Transaction";
            this.Load += new System.EventHandler(this.frmColorSchemeForViewPOSTrans_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmColorSchemeForViewPOSTrans_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.cpTextEditor)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cpColor)).EndInit();
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdDetail)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraDataSource1)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grColorScemeDetails)).EndInit();
            this.grColorScemeDetails.ResumeLayout(false);
            this.grColorScemeDetails.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numToAmount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numFromAmount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cpBackColor)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cpForeColor)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private Infragistics.Win.Misc.UltraLabel lblTransactionType;
        private Infragistics.Win.UltraWinGrid.UltraGrid grdDetail;
        private Infragistics.Win.UltraWinDataSource.UltraDataSource ultraDataSource1;
        private Infragistics.Win.Misc.UltraButton btnSave;
        private Infragistics.Win.Misc.UltraButton btnClose;
        private Infragistics.Win.UltraWinEditors.UltraColorPicker cpColor;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor cpTextEditor;
        public Infragistics.Win.Misc.UltraButton btnDelete;
        public Infragistics.Win.Misc.UltraButton btnEdit;
        public Infragistics.Win.Misc.UltraButton btnNewNote;
        private System.Windows.Forms.GroupBox groupBox2;
        private Infragistics.Win.UltraWinEditors.UltraColorPicker cpForeColor;
        private Infragistics.Win.Misc.UltraGroupBox grColorScemeDetails;
        private Infragistics.Win.Misc.UltraLabel ultraLabel4;
        private Infragistics.Win.Misc.UltraLabel ultraLabel3;
        private Infragistics.Win.Misc.UltraLabel ultraLabel2;
        private Infragistics.Win.Misc.UltraLabel ultraLabel1;
        private Infragistics.Win.UltraWinEditors.UltraColorPicker cpBackColor;
        private Infragistics.Win.UltraWinEditors.UltraNumericEditor numFromAmount;
        private Infragistics.Win.UltraWinEditors.UltraNumericEditor numToAmount;
        private Infragistics.Win.UltraWinStatusBar.UltraStatusBar sbMain;
    }
}