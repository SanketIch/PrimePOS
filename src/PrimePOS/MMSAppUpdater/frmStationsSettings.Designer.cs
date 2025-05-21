namespace MMSAppUpdater
{
    partial class frmStationsSettings
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
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand1 = new Infragistics.Win.UltraWinGrid.UltraGridBand("Band 0", -1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn1 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("AppName", -1, null, 0, Infragistics.Win.UltraWinGrid.SortIndicator.Ascending, false);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn2 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("StationID");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn3 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("UpdateType");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn4 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("MachineName", 0);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn5 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("LastRecordedRunning", 1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn6 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("ApplicationRunningpath", 2);
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance7 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance8 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance9 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance10 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance11 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinScrollBar.ScrollBarLook scrollBarLook1 = new Infragistics.Win.UltraWinScrollBar.ScrollBarLook();
            Infragistics.Win.Appearance appearance12 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance13 = new Infragistics.Win.Appearance();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmStationsSettings));
            Infragistics.Win.Appearance appearance14 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance15 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance16 = new Infragistics.Win.Appearance();
            this.grpMain = new Infragistics.Win.Misc.UltraGroupBox();
            this.cbAppFilter = new Infragistics.Win.UltraWinEditors.UltraComboEditor();
            this.ultraLabel3 = new Infragistics.Win.Misc.UltraLabel();
            this.label2 = new System.Windows.Forms.Label();
            this.btnMinimize = new Infragistics.Win.Misc.UltraButton();
            this.btnWindowClose = new Infragistics.Win.Misc.UltraButton();
            this.label1 = new System.Windows.Forms.Label();
            this.dgUpdates = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.cmStation = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.SMDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.btnSample2 = new Infragistics.Win.Misc.UltraButton();
            this.btnDownloadInstall = new Infragistics.Win.Misc.UltraButton();
            this.btnClose = new Infragistics.Win.Misc.UltraButton();
            this.txtStationID = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.lblMessage = new Infragistics.Win.Misc.UltraLabel();
            ((System.ComponentModel.ISupportInitialize)(this.grpMain)).BeginInit();
            this.grpMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cbAppFilter)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgUpdates)).BeginInit();
            this.cmStation.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtStationID)).BeginInit();
            this.SuspendLayout();
            // 
            // grpMain
            // 
            appearance1.BackColor = System.Drawing.Color.White;
            appearance1.BackColor2 = System.Drawing.SystemColors.Control;
            appearance1.BackGradientStyle = Infragistics.Win.GradientStyle.GlassTop50Bright;
            this.grpMain.Appearance = appearance1;
            this.grpMain.BackColorInternal = System.Drawing.Color.Transparent;
            this.grpMain.BorderStyle = Infragistics.Win.Misc.GroupBoxBorderStyle.RectangularSolid;
            this.grpMain.Controls.Add(this.cbAppFilter);
            this.grpMain.Controls.Add(this.ultraLabel3);
            this.grpMain.Controls.Add(this.label2);
            this.grpMain.Controls.Add(this.btnMinimize);
            this.grpMain.Controls.Add(this.btnWindowClose);
            this.grpMain.Controls.Add(this.label1);
            this.grpMain.Controls.Add(this.dgUpdates);
            this.grpMain.Controls.Add(this.btnSample2);
            this.grpMain.Controls.Add(this.btnDownloadInstall);
            this.grpMain.Controls.Add(this.btnClose);
            this.grpMain.Controls.Add(this.txtStationID);
            this.grpMain.Controls.Add(this.lblMessage);
            this.grpMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpMain.HeaderBorderStyle = Infragistics.Win.UIElementBorderStyle.None;
            this.grpMain.Location = new System.Drawing.Point(0, 0);
            this.grpMain.Name = "grpMain";
            this.grpMain.Size = new System.Drawing.Size(795, 304);
            this.grpMain.TabIndex = 23;
            // 
            // cbAppFilter
            // 
            this.cbAppFilter.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cbAppFilter.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button;
            this.cbAppFilter.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbAppFilter.Location = new System.Drawing.Point(547, 41);
            this.cbAppFilter.Name = "cbAppFilter";
            this.cbAppFilter.Size = new System.Drawing.Size(176, 20);
            this.cbAppFilter.TabIndex = 61;
            this.cbAppFilter.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.cbAppFilter.SelectionChanged += new System.EventHandler(this.cbAppFilter_SelectionChanged);
            // 
            // ultraLabel3
            // 
            this.ultraLabel3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ultraLabel3.BackColorInternal = System.Drawing.Color.Transparent;
            this.ultraLabel3.Font = new System.Drawing.Font("Verdana", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultraLabel3.Location = new System.Drawing.Point(437, 48);
            this.ultraLabel3.Name = "ultraLabel3";
            this.ultraLabel3.Size = new System.Drawing.Size(115, 13);
            this.ultraLabel3.TabIndex = 60;
            this.ultraLabel3.Text = "Select Application";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.label2.Location = new System.Drawing.Point(101, 281);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(76, 13);
            this.label2.TabIndex = 34;
            this.label2.Text = "Station ID:";
            this.label2.Visible = false;
            // 
            // btnMinimize
            // 
            this.btnMinimize.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            appearance2.BackColor = System.Drawing.Color.LightSkyBlue;
            appearance2.FontData.SizeInPoints = 13F;
            appearance2.ForeColor = System.Drawing.Color.Black;
            appearance2.ImageHAlign = Infragistics.Win.HAlign.Center;
            appearance2.ImageVAlign = Infragistics.Win.VAlign.Top;
            appearance2.TextHAlignAsString = "Center";
            appearance2.TextVAlignAsString = "Middle";
            this.btnMinimize.Appearance = appearance2;
            this.btnMinimize.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsVistaButton;
            this.btnMinimize.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnMinimize.ImageSize = new System.Drawing.Size(34, 32);
            this.btnMinimize.Location = new System.Drawing.Point(737, 2);
            this.btnMinimize.Name = "btnMinimize";
            this.btnMinimize.Size = new System.Drawing.Size(26, 18);
            this.btnMinimize.TabIndex = 32;
            this.btnMinimize.Text = "-";
            this.btnMinimize.UseHotTracking = Infragistics.Win.DefaultableBoolean.True;
            this.btnMinimize.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnMinimize.Click += new System.EventHandler(this.btnMinimize_Click);
            // 
            // btnWindowClose
            // 
            this.btnWindowClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            appearance3.BackColor = System.Drawing.Color.Tomato;
            appearance3.FontData.SizeInPoints = 8F;
            appearance3.ForeColor = System.Drawing.Color.Black;
            appearance3.ImageHAlign = Infragistics.Win.HAlign.Center;
            appearance3.ImageVAlign = Infragistics.Win.VAlign.Top;
            appearance3.TextHAlignAsString = "Center";
            appearance3.TextVAlignAsString = "Middle";
            this.btnWindowClose.Appearance = appearance3;
            this.btnWindowClose.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsVistaButton;
            this.btnWindowClose.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnWindowClose.ImageSize = new System.Drawing.Size(34, 32);
            this.btnWindowClose.Location = new System.Drawing.Point(764, 2);
            this.btnWindowClose.Name = "btnWindowClose";
            this.btnWindowClose.Size = new System.Drawing.Size(26, 18);
            this.btnWindowClose.TabIndex = 31;
            this.btnWindowClose.Text = "X";
            this.btnWindowClose.UseHotTracking = Infragistics.Win.DefaultableBoolean.True;
            this.btnWindowClose.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnWindowClose.Click += new System.EventHandler(this.btnWindowClose_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.label1.Location = new System.Drawing.Point(12, 56);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(371, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Following Updates are available for the Current Station ";
            this.label1.Visible = false;
            // 
            // dgUpdates
            // 
            this.dgUpdates.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgUpdates.ContextMenuStrip = this.cmStation;
            appearance4.BackColor = System.Drawing.Color.Transparent;
            appearance4.BackColor2 = System.Drawing.Color.Transparent;
            this.dgUpdates.DisplayLayout.Appearance = appearance4;
            this.dgUpdates.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ExtendLastColumn;
            ultraGridColumn1.Header.Caption = "Application Name";
            ultraGridColumn1.Header.VisiblePosition = 0;
            ultraGridColumn1.Width = 147;
            ultraGridColumn2.Header.VisiblePosition = 1;
            ultraGridColumn2.Width = 82;
            ultraGridColumn3.Header.VisiblePosition = 2;
            ultraGridColumn3.Width = 63;
            ultraGridColumn4.Header.Caption = "Machine Name";
            ultraGridColumn4.Header.VisiblePosition = 3;
            ultraGridColumn4.Width = 123;
            ultraGridColumn5.Header.Caption = "Last Recorded Running";
            ultraGridColumn5.Header.VisiblePosition = 4;
            ultraGridColumn6.Header.Caption = "Application Running path";
            ultraGridColumn6.Header.VisiblePosition = 5;
            ultraGridBand1.Columns.AddRange(new object[] {
            ultraGridColumn1,
            ultraGridColumn2,
            ultraGridColumn3,
            ultraGridColumn4,
            ultraGridColumn5,
            ultraGridColumn6});
            ultraGridBand1.Override.AllowAddNew = Infragistics.Win.UltraWinGrid.AllowAddNew.No;
            ultraGridBand1.Override.AllowColMoving = Infragistics.Win.UltraWinGrid.AllowColMoving.NotAllowed;
            ultraGridBand1.Override.AllowColSwapping = Infragistics.Win.UltraWinGrid.AllowColSwapping.NotAllowed;
            ultraGridBand1.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.False;
            ultraGridBand1.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;
            appearance5.ForeColor = System.Drawing.Color.Black;
            ultraGridBand1.Override.SelectedRowAppearance = appearance5;
            this.dgUpdates.DisplayLayout.BandsSerializer.Add(ultraGridBand1);
            this.dgUpdates.DisplayLayout.Override.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Office2003ToolbarButton;
            appearance6.BackColor = System.Drawing.Color.Transparent;
            this.dgUpdates.DisplayLayout.Override.CardAreaAppearance = appearance6;
            this.dgUpdates.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.Edit;
            appearance7.BackColor = System.Drawing.Color.White;
            appearance7.BackColor2 = System.Drawing.SystemColors.Control;
            appearance7.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance7.FontData.BoldAsString = "False";
            appearance7.FontData.Name = "Verdana";
            appearance7.FontData.SizeInPoints = 8F;
            appearance7.ForeColor = System.Drawing.Color.Black;
            appearance7.ThemedElementAlpha = Infragistics.Win.Alpha.Transparent;
            this.dgUpdates.DisplayLayout.Override.HeaderAppearance = appearance7;
            this.dgUpdates.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortSingle;
            appearance8.BackColor = System.Drawing.Color.Transparent;
            appearance8.BackColor2 = System.Drawing.Color.Transparent;
            this.dgUpdates.DisplayLayout.Override.RowAlternateAppearance = appearance8;
            appearance9.BackColor = System.Drawing.Color.Transparent;
            appearance9.BackColor2 = System.Drawing.Color.Transparent;
            appearance9.FontData.BoldAsString = "False";
            appearance9.FontData.Name = "Verdana";
            appearance9.FontData.SizeInPoints = 8F;
            this.dgUpdates.DisplayLayout.Override.RowAppearance = appearance9;
            appearance10.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(195)))), ((int)(((byte)(218)))), ((int)(((byte)(249)))));
            appearance10.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(158)))), ((int)(((byte)(190)))), ((int)(((byte)(245)))));
            appearance10.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            this.dgUpdates.DisplayLayout.Override.RowSelectorAppearance = appearance10;
            this.dgUpdates.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            appearance11.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(240)))), ((int)(((byte)(194)))));
            appearance11.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(240)))), ((int)(((byte)(194)))));
            appearance11.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance11.ForeColor = System.Drawing.Color.Black;
            this.dgUpdates.DisplayLayout.Override.SelectedRowAppearance = appearance11;
            appearance12.BackColor = System.Drawing.SystemColors.Control;
            appearance12.BackColor2 = System.Drawing.Color.White;
            appearance12.BackGradientStyle = Infragistics.Win.GradientStyle.GlassTop20;
            scrollBarLook1.Appearance = appearance12;
            this.dgUpdates.DisplayLayout.ScrollBarLook = scrollBarLook1;
            this.dgUpdates.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.dgUpdates.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgUpdates.Location = new System.Drawing.Point(5, 71);
            this.dgUpdates.Name = "dgUpdates";
            this.dgUpdates.Size = new System.Drawing.Size(785, 199);
            this.dgUpdates.TabIndex = 29;
            this.dgUpdates.TabStop = false;
            this.dgUpdates.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.dgUpdates.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // cmStation
            // 
            this.cmStation.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.SMDelete});
            this.cmStation.Name = "cmStation";
            this.cmStation.Size = new System.Drawing.Size(108, 26);
            // 
            // SMDelete
            // 
            this.SMDelete.Name = "SMDelete";
            this.SMDelete.Size = new System.Drawing.Size(107, 22);
            this.SMDelete.Text = "Delete";
            this.SMDelete.Click += new System.EventHandler(this.SMDelete_Click);
            // 
            // btnSample2
            // 
            appearance13.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            appearance13.Image = ((object)(resources.GetObject("appearance13.Image")));
            appearance13.ImageHAlign = Infragistics.Win.HAlign.Center;
            appearance13.ImageVAlign = Infragistics.Win.VAlign.Top;
            appearance13.TextHAlignAsString = "Center";
            appearance13.TextVAlignAsString = "Middle";
            this.btnSample2.Appearance = appearance13;
            this.btnSample2.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsVistaButton;
            this.btnSample2.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnSample2.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSample2.ImageSize = new System.Drawing.Size(40, 40);
            this.btnSample2.Location = new System.Drawing.Point(729, 29);
            this.btnSample2.Name = "btnSample2";
            this.btnSample2.Size = new System.Drawing.Size(54, 36);
            this.btnSample2.TabIndex = 20;
            this.btnSample2.TabStop = false;
            this.btnSample2.Text = "No";
            this.btnSample2.UseHotTracking = Infragistics.Win.DefaultableBoolean.True;
            this.btnSample2.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnSample2.Visible = false;
            // 
            // btnDownloadInstall
            // 
            this.btnDownloadInstall.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            appearance14.FontData.SizeInPoints = 8F;
            appearance14.ForeColor = System.Drawing.Color.Black;
            appearance14.ImageHAlign = Infragistics.Win.HAlign.Center;
            appearance14.ImageVAlign = Infragistics.Win.VAlign.Top;
            appearance14.TextHAlignAsString = "Center";
            appearance14.TextVAlignAsString = "Middle";
            this.btnDownloadInstall.Appearance = appearance14;
            this.btnDownloadInstall.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsVistaButton;
            this.btnDownloadInstall.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDownloadInstall.ImageSize = new System.Drawing.Size(30, 30);
            this.btnDownloadInstall.Location = new System.Drawing.Point(585, 275);
            this.btnDownloadInstall.Name = "btnDownloadInstall";
            this.btnDownloadInstall.Size = new System.Drawing.Size(98, 24);
            this.btnDownloadInstall.TabIndex = 1;
            this.btnDownloadInstall.Text = "Set Station ";
            this.btnDownloadInstall.UseHotTracking = Infragistics.Win.DefaultableBoolean.True;
            this.btnDownloadInstall.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnDownloadInstall.Visible = false;
            this.btnDownloadInstall.Click += new System.EventHandler(this.btnDownloadInstall_Click);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            appearance15.FontData.SizeInPoints = 8F;
            appearance15.ForeColor = System.Drawing.Color.Black;
            appearance15.ImageHAlign = Infragistics.Win.HAlign.Center;
            appearance15.ImageVAlign = Infragistics.Win.VAlign.Top;
            appearance15.TextHAlignAsString = "Center";
            appearance15.TextVAlignAsString = "Middle";
            this.btnClose.Appearance = appearance15;
            this.btnClose.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsVistaButton;
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClose.ImageSize = new System.Drawing.Size(34, 32);
            this.btnClose.Location = new System.Drawing.Point(689, 275);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(98, 24);
            this.btnClose.TabIndex = 2;
            this.btnClose.Text = "Close";
            this.btnClose.UseHotTracking = Infragistics.Win.DefaultableBoolean.True;
            this.btnClose.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // txtStationID
            // 
            this.txtStationID.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.txtStationID.BorderStyle = Infragistics.Win.UIElementBorderStyle.Etched;
            this.txtStationID.Location = new System.Drawing.Point(353, 277);
            this.txtStationID.Name = "txtStationID";
            this.txtStationID.Size = new System.Drawing.Size(48, 21);
            this.txtStationID.TabIndex = 33;
            this.txtStationID.TabStop = false;
            this.txtStationID.Visible = false;
            // 
            // lblMessage
            // 
            appearance16.BackColor = System.Drawing.Color.White;
            appearance16.BackColor2 = System.Drawing.Color.Gray;
            appearance16.BackGradientStyle = Infragistics.Win.GradientStyle.GlassBottom50Bright;
            appearance16.ForeColor = System.Drawing.Color.Black;
            appearance16.TextHAlignAsString = "Center";
            appearance16.TextVAlignAsString = "Bottom";
            this.lblMessage.Appearance = appearance16;
            this.lblMessage.BackColorInternal = System.Drawing.Color.Transparent;
            this.lblMessage.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblMessage.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMessage.Location = new System.Drawing.Point(2, 2);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.Size = new System.Drawing.Size(791, 35);
            this.lblMessage.TabIndex = 23;
            this.lblMessage.Text = "Station Settings";
            this.lblMessage.UseHotTracking = Infragistics.Win.DefaultableBoolean.True;
            this.lblMessage.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lblMessage_MouseDown);
            this.lblMessage.MouseMove += new System.Windows.Forms.MouseEventHandler(this.lblMessage_MouseMove);
            this.lblMessage.MouseUp += new System.Windows.Forms.MouseEventHandler(this.lblMessage_MouseUp);
            // 
            // frmStationsSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(795, 304);
            this.ControlBox = false;
            this.Controls.Add(this.grpMain);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmStationsSettings";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Load += new System.EventHandler(this.frmStationsSettings_Load);
            ((System.ComponentModel.ISupportInitialize)(this.grpMain)).EndInit();
            this.grpMain.ResumeLayout(false);
            this.grpMain.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cbAppFilter)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgUpdates)).EndInit();
            this.cmStation.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.txtStationID)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Infragistics.Win.Misc.UltraGroupBox grpMain;
        private Infragistics.Win.Misc.UltraButton btnMinimize;
        private Infragistics.Win.Misc.UltraButton btnWindowClose;
        private Infragistics.Win.UltraWinGrid.UltraGrid dgUpdates;
        private Infragistics.Win.Misc.UltraButton btnSample2;
        private Infragistics.Win.Misc.UltraButton btnDownloadInstall;
        private Infragistics.Win.Misc.UltraLabel lblMessage;
        private Infragistics.Win.Misc.UltraButton btnClose;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor txtStationID;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ContextMenuStrip cmStation;
        private System.Windows.Forms.ToolStripMenuItem SMDelete;
        private Infragistics.Win.UltraWinEditors.UltraComboEditor cbAppFilter;
        private Infragistics.Win.Misc.UltraLabel ultraLabel3;
    }
}