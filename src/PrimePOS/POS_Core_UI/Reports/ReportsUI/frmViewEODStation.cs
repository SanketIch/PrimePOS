using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Windows.Forms;
using POS_Core.BusinessRules;
using POS_Core.CommonData;
//using POS_Core.DataAccess;
using POS_Core.CommonData.Rows;
using Infragistics.Win.UltraWinGrid;
using POS_Core.Resources;

namespace POS_Core_UI
{
	/// <summary>
	/// Summary description for frmVendorSearch.
	/// </summary>
	public class frmViewEODStation : System.Windows.Forms.Form
	{
		public string SearchTable ="";
		public bool IsCanceled  = true;
		public int ActiveOnly=0;
		public bool DisplayRecordAtStartup = false;
		private Search oBLSearch = new Search();
		private DataSet oDataSet = new DataSet();
		public bool isReadonly=false;
		private int CurrentX;
		private int CurrentY;

		public string FormCaption="";
		public string LabelText1="";
		public string LabelText2="";
		private Infragistics.Win.UltraWinDataSource.UltraDataSource ultraDataSource1;
		private Infragistics.Win.Misc.UltraButton btnAdd;
        internal UltraGrid grdSearch;
        private Infragistics.Win.UltraWinStatusBar.UltraStatusBar sbMain;
		private Infragistics.Win.Misc.UltraButton btnEdit;
		private Infragistics.Win.Misc.UltraButton btnClose;
		private Infragistics.Win.UltraWinTabControl.UltraTabControl tabMain;
		private Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage ultraTabSharedControlsPage1;
		private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl1;
		private Infragistics.Win.UltraWinSchedule.UltraCalendarCombo clTo;
		private Infragistics.Win.UltraWinSchedule.UltraCalendarCombo clFrom;
		private Infragistics.Win.Misc.UltraLabel lbl2;
		private Infragistics.Win.Misc.UltraLabel lbl1;
        private Infragistics.Win.Misc.UltraButton btnSearch;
        private Infragistics.Win.Misc.UltraButton btnEmailReport;
        private IContainer components;
        private CheckBox chkSelectAll;
        private Infragistics.Win.Misc.UltraButton btnOk;
        private bool allowMultiRowSelect = false;   //PRIMEPOS-2494 26-Feb-2018 JY Added
        private int EODID;  //PRIMEPOS-2700 02-Jul-2019 JY Added

        internal bool AllowMultiRowSelect   //PRIMEPOS-2494 26-Feb-2018 JY Added
        {
            get { return allowMultiRowSelect; }
            set { allowMultiRowSelect = value; }
        }

        public frmViewEODStation()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
			try
			{
				grdSearch.DataSource = oDataSet;
				this.resizeColumns();
				grdSearch.Refresh();
			}
			catch(Exception exp)
			{
				clsUIHelper.ShowErrorMsg( exp.Message);
			}			

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
		}

        public frmViewEODStation(string strTableName)
        {
            InitializeComponent();
            try
            {
                grdSearch.DataSource = oDataSet;                
                resizeColumns();
                grdSearch.Refresh();
                SearchTable = strTableName;
                btnOk.Visible = true;
                btnAdd.Visible = false;
                btnEdit.Visible = false;
                btnEmailReport.Visible = false;
            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }

        #region PRIMEPOS-2700 02-Jul-2019 JY Added
        public frmViewEODStation(string strTableName, int nEODID)
        {
            InitializeComponent();
            try
            {
                grdSearch.DataSource = oDataSet;
                resizeColumns();
                grdSearch.Refresh();
                SearchTable = strTableName;
                EODID = nEODID;
                btnOk.Visible = true;
                btnAdd.Visible = false;
                btnEdit.Visible = false;
                btnEmailReport.Visible = false;
            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }
        #endregion

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            Infragistics.Win.UltraWinSchedule.CalendarCombo.DateButton dateButton1 = new Infragistics.Win.UltraWinSchedule.CalendarCombo.DateButton();
            Infragistics.Win.UltraWinSchedule.CalendarCombo.DateButton dateButton2 = new Infragistics.Win.UltraWinSchedule.CalendarCombo.DateButton();
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmViewEODStation));
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinDataSource.UltraDataColumn ultraDataColumn1 = new Infragistics.Win.UltraWinDataSource.UltraDataColumn("Column Name");
            Infragistics.Win.UltraWinDataSource.UltraDataColumn ultraDataColumn2 = new Infragistics.Win.UltraWinDataSource.UltraDataColumn("Type");
            Infragistics.Win.UltraWinDataSource.UltraDataColumn ultraDataColumn3 = new Infragistics.Win.UltraWinDataSource.UltraDataColumn("Criteria Value");
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand1 = new Infragistics.Win.UltraWinGrid.UltraGridBand("Band 0", -1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn1 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Column Name", -1, null, 0, Infragistics.Win.UltraWinGrid.SortIndicator.Ascending, false);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn2 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Type");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn3 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Criteria Value");
            Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance7 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance8 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance9 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance10 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance11 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance12 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance13 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance14 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance15 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance16 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance17 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance18 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance19 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance20 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance21 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance22 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance23 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance24 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinScrollBar.ScrollBarLook scrollBarLook1 = new Infragistics.Win.UltraWinScrollBar.ScrollBarLook();
            Infragistics.Win.Appearance appearance25 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance26 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance27 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance28 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance29 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance30 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance31 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance32 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinStatusBar.UltraStatusPanel ultraStatusPanel1 = new Infragistics.Win.UltraWinStatusBar.UltraStatusPanel();
            Infragistics.Win.Appearance appearance33 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance34 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance35 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance36 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance37 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance38 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance39 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance40 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab1 = new Infragistics.Win.UltraWinTabControl.UltraTab();
            Infragistics.Win.Appearance appearance41 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance42 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance43 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance44 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance45 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance46 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance47 = new Infragistics.Win.Appearance();
            this.ultraTabPageControl1 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.clTo = new Infragistics.Win.UltraWinSchedule.UltraCalendarCombo();
            this.clFrom = new Infragistics.Win.UltraWinSchedule.UltraCalendarCombo();
            this.lbl2 = new Infragistics.Win.Misc.UltraLabel();
            this.lbl1 = new Infragistics.Win.Misc.UltraLabel();
            this.btnSearch = new Infragistics.Win.Misc.UltraButton();
            this.ultraDataSource1 = new Infragistics.Win.UltraWinDataSource.UltraDataSource(this.components);
            this.grdSearch = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.btnAdd = new Infragistics.Win.Misc.UltraButton();
            this.sbMain = new Infragistics.Win.UltraWinStatusBar.UltraStatusBar();
            this.btnEdit = new Infragistics.Win.Misc.UltraButton();
            this.btnClose = new Infragistics.Win.Misc.UltraButton();
            this.tabMain = new Infragistics.Win.UltraWinTabControl.UltraTabControl();
            this.ultraTabSharedControlsPage1 = new Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage();
            this.btnEmailReport = new Infragistics.Win.Misc.UltraButton();
            this.chkSelectAll = new System.Windows.Forms.CheckBox();
            this.btnOk = new Infragistics.Win.Misc.UltraButton();
            this.ultraTabPageControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.clTo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.clFrom)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraDataSource1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdSearch)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sbMain)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tabMain)).BeginInit();
            this.tabMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // ultraTabPageControl1
            // 
            this.ultraTabPageControl1.Controls.Add(this.clTo);
            this.ultraTabPageControl1.Controls.Add(this.clFrom);
            this.ultraTabPageControl1.Controls.Add(this.lbl2);
            this.ultraTabPageControl1.Controls.Add(this.lbl1);
            this.ultraTabPageControl1.Controls.Add(this.btnSearch);
            this.ultraTabPageControl1.Location = new System.Drawing.Point(2, 23);
            this.ultraTabPageControl1.Name = "ultraTabPageControl1";
            this.ultraTabPageControl1.Size = new System.Drawing.Size(1006, 50);
            // 
            // clTo
            // 
            this.clTo.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.clTo.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Office2003ToolbarButton;
            this.clTo.DateButtons.Add(dateButton1);
            this.clTo.Location = new System.Drawing.Point(278, 17);
            this.clTo.Name = "clTo";
            this.clTo.NonAutoSizeHeight = 22;
            this.clTo.Size = new System.Drawing.Size(123, 21);
            this.clTo.TabIndex = 2;
            this.clTo.Value = new System.DateTime(2014, 11, 12, 0, 0, 0, 0);
            // 
            // clFrom
            // 
            this.clFrom.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.clFrom.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Office2003ToolbarButton;
            this.clFrom.DateButtons.Add(dateButton2);
            this.clFrom.Location = new System.Drawing.Point(56, 17);
            this.clFrom.Name = "clFrom";
            this.clFrom.NonAutoSizeHeight = 22;
            this.clFrom.Size = new System.Drawing.Size(123, 21);
            this.clFrom.TabIndex = 1;
            this.clFrom.Value = new System.DateTime(2014, 11, 12, 0, 0, 0, 0);
            // 
            // lbl2
            // 
            appearance1.ForeColor = System.Drawing.Color.Black;
            this.lbl2.Appearance = appearance1;
            this.lbl2.AutoSize = true;
            this.lbl2.BackColorInternal = System.Drawing.Color.Transparent;
            this.lbl2.Location = new System.Drawing.Point(240, 18);
            this.lbl2.Name = "lbl2";
            this.lbl2.Size = new System.Drawing.Size(22, 18);
            this.lbl2.TabIndex = 7;
            this.lbl2.Text = "To ";
            this.lbl2.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // lbl1
            // 
            appearance2.ForeColor = System.Drawing.Color.Black;
            this.lbl1.Appearance = appearance2;
            this.lbl1.AutoSize = true;
            this.lbl1.BackColorInternal = System.Drawing.Color.Transparent;
            this.lbl1.Location = new System.Drawing.Point(14, 18);
            this.lbl1.Name = "lbl1";
            this.lbl1.Size = new System.Drawing.Size(40, 18);
            this.lbl1.TabIndex = 5;
            this.lbl1.Text = "From ";
            this.lbl1.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // btnSearch
            // 
            this.btnSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            appearance3.BackColor = System.Drawing.Color.White;
            appearance3.BackColor2 = System.Drawing.SystemColors.Control;
            appearance3.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance3.FontData.BoldAsString = "True";
            appearance3.ForeColor = System.Drawing.Color.Black;
            appearance3.Image = ((object)(resources.GetObject("appearance3.Image")));
            this.btnSearch.Appearance = appearance3;
            this.btnSearch.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Flat;
            appearance4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(188)))), ((int)(((byte)(193)))), ((int)(((byte)(207)))));
            appearance4.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(188)))), ((int)(((byte)(193)))), ((int)(((byte)(207)))));
            this.btnSearch.HotTrackAppearance = appearance4;
            this.btnSearch.Location = new System.Drawing.Point(874, 12);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(121, 30);
            this.btnSearch.TabIndex = 3;
            this.btnSearch.Text = "&Search (F4)";
            this.btnSearch.UseHotTracking = Infragistics.Win.DefaultableBoolean.True;
            this.btnSearch.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // ultraDataSource1
            // 
            this.ultraDataSource1.Band.Columns.AddRange(new object[] {
            ultraDataColumn1,
            ultraDataColumn2,
            ultraDataColumn3});
            // 
            // grdSearch
            // 
            this.grdSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grdSearch.DataSource = this.ultraDataSource1;
            appearance5.BackColor = System.Drawing.Color.White;
            appearance5.BackColor2 = System.Drawing.Color.White;
            appearance5.BackColorDisabled = System.Drawing.Color.White;
            appearance5.BackColorDisabled2 = System.Drawing.Color.White;
            appearance5.BackGradientStyle = Infragistics.Win.GradientStyle.None;
            this.grdSearch.DisplayLayout.Appearance = appearance5;
            ultraGridColumn1.Header.VisiblePosition = 0;
            ultraGridColumn2.Header.VisiblePosition = 1;
            ultraGridColumn3.Header.VisiblePosition = 2;
            ultraGridBand1.Columns.AddRange(new object[] {
            ultraGridColumn1,
            ultraGridColumn2,
            ultraGridColumn3});
            this.grdSearch.DisplayLayout.BandsSerializer.Add(ultraGridBand1);
            this.grdSearch.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.grdSearch.DisplayLayout.InterBandSpacing = 10;
            this.grdSearch.DisplayLayout.MaxColScrollRegions = 1;
            this.grdSearch.DisplayLayout.MaxRowScrollRegions = 1;
            appearance6.BackColor = System.Drawing.Color.White;
            appearance6.BackColor2 = System.Drawing.Color.White;
            this.grdSearch.DisplayLayout.Override.ActiveCardCaptionAppearance = appearance6;
            appearance7.BackColor = System.Drawing.Color.White;
            appearance7.BackColor2 = System.Drawing.Color.White;
            appearance7.BorderColor = System.Drawing.Color.Gray;
            this.grdSearch.DisplayLayout.Override.ActiveRowAppearance = appearance7;
            appearance8.BackColor = System.Drawing.Color.White;
            appearance8.BackColor2 = System.Drawing.Color.White;
            appearance8.BorderColor = System.Drawing.Color.Gray;
            this.grdSearch.DisplayLayout.Override.AddRowAppearance = appearance8;
            this.grdSearch.DisplayLayout.Override.AllowAddNew = Infragistics.Win.UltraWinGrid.AllowAddNew.No;
            this.grdSearch.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.False;
            this.grdSearch.DisplayLayout.Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.False;
            appearance9.BackColor = System.Drawing.Color.Transparent;
            this.grdSearch.DisplayLayout.Override.CardAreaAppearance = appearance9;
            appearance10.BackColor = System.Drawing.Color.White;
            appearance10.BackColor2 = System.Drawing.Color.White;
            appearance10.BackColorDisabled = System.Drawing.Color.White;
            appearance10.BackColorDisabled2 = System.Drawing.Color.White;
            appearance10.BorderColor = System.Drawing.Color.Black;
            appearance10.BorderColor3DBase = System.Drawing.Color.Black;
            this.grdSearch.DisplayLayout.Override.CellAppearance = appearance10;
            appearance11.BackColor = System.Drawing.Color.White;
            appearance11.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(231)))), ((int)(((byte)(230)))));
            appearance11.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance11.BorderAlpha = Infragistics.Win.Alpha.Transparent;
            appearance11.BorderColor = System.Drawing.Color.Gray;
            appearance11.BorderColor3DBase = System.Drawing.Color.Gray;
            appearance11.Image = ((object)(resources.GetObject("appearance11.Image")));
            appearance11.ImageBackgroundAlpha = Infragistics.Win.Alpha.Transparent;
            appearance11.ImageBackgroundStyle = Infragistics.Win.ImageBackgroundStyle.Centered;
            this.grdSearch.DisplayLayout.Override.CellButtonAppearance = appearance11;
            appearance12.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            appearance12.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.grdSearch.DisplayLayout.Override.EditCellAppearance = appearance12;
            appearance13.BorderColor = System.Drawing.Color.Gray;
            this.grdSearch.DisplayLayout.Override.FilteredInRowAppearance = appearance13;
            appearance14.BorderColor = System.Drawing.Color.Gray;
            this.grdSearch.DisplayLayout.Override.FilteredOutRowAppearance = appearance14;
            appearance15.BackColor = System.Drawing.Color.White;
            appearance15.BackColor2 = System.Drawing.Color.White;
            appearance15.BackColorDisabled = System.Drawing.Color.White;
            appearance15.BackColorDisabled2 = System.Drawing.Color.White;
            this.grdSearch.DisplayLayout.Override.FixedCellAppearance = appearance15;
            appearance16.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            appearance16.BackColor2 = System.Drawing.Color.Beige;
            this.grdSearch.DisplayLayout.Override.FixedHeaderAppearance = appearance16;
            appearance17.BackColor = System.Drawing.Color.White;
            appearance17.BackColor2 = System.Drawing.SystemColors.Control;
            appearance17.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance17.FontData.BoldAsString = "True";
            appearance17.ForeColor = System.Drawing.Color.Black;
            appearance17.TextHAlignAsString = "Left";
            appearance17.ThemedElementAlpha = Infragistics.Win.Alpha.Transparent;
            this.grdSearch.DisplayLayout.Override.HeaderAppearance = appearance17;
            this.grdSearch.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortSingle;
            appearance18.BorderColor = System.Drawing.Color.Gray;
            this.grdSearch.DisplayLayout.Override.RowAlternateAppearance = appearance18;
            appearance19.BackColor = System.Drawing.Color.White;
            appearance19.BackColor2 = System.Drawing.Color.White;
            appearance19.BackGradientStyle = Infragistics.Win.GradientStyle.None;
            appearance19.BackHatchStyle = Infragistics.Win.BackHatchStyle.None;
            appearance19.BorderColor = System.Drawing.Color.Gray;
            this.grdSearch.DisplayLayout.Override.RowAppearance = appearance19;
            appearance20.BorderColor = System.Drawing.Color.Gray;
            this.grdSearch.DisplayLayout.Override.RowPreviewAppearance = appearance20;
            appearance21.BackColor = System.Drawing.Color.White;
            appearance21.BackColor2 = System.Drawing.SystemColors.Control;
            appearance21.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance21.BorderColor = System.Drawing.Color.Gray;
            this.grdSearch.DisplayLayout.Override.RowSelectorAppearance = appearance21;
            this.grdSearch.DisplayLayout.Override.RowSelectorWidth = 12;
            this.grdSearch.DisplayLayout.Override.RowSpacingBefore = 2;
            appearance22.BackColor = System.Drawing.Color.Navy;
            appearance22.BackColorAlpha = Infragistics.Win.Alpha.Opaque;
            this.grdSearch.DisplayLayout.Override.SelectedCellAppearance = appearance22;
            appearance23.BackColor = System.Drawing.Color.Navy;
            appearance23.BackColorDisabled = System.Drawing.Color.Navy;
            appearance23.BackGradientStyle = Infragistics.Win.GradientStyle.None;
            appearance23.BackHatchStyle = Infragistics.Win.BackHatchStyle.None;
            appearance23.BorderColor = System.Drawing.Color.Gray;
            appearance23.ForeColor = System.Drawing.Color.Black;
            this.grdSearch.DisplayLayout.Override.SelectedRowAppearance = appearance23;
            this.grdSearch.DisplayLayout.Override.SelectTypeCell = Infragistics.Win.UltraWinGrid.SelectType.None;
            this.grdSearch.DisplayLayout.Override.SelectTypeCol = Infragistics.Win.UltraWinGrid.SelectType.None;
            this.grdSearch.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.Single;
            appearance24.BorderColor = System.Drawing.Color.Gray;
            this.grdSearch.DisplayLayout.Override.TemplateAddRowAppearance = appearance24;
            this.grdSearch.DisplayLayout.RowConnectorColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            this.grdSearch.DisplayLayout.RowConnectorStyle = Infragistics.Win.UltraWinGrid.RowConnectorStyle.Solid;
            appearance25.BackColor = System.Drawing.Color.White;
            appearance25.BackColor2 = System.Drawing.SystemColors.Control;
            appearance25.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            scrollBarLook1.Appearance = appearance25;
            appearance26.BackColor = System.Drawing.Color.White;
            appearance26.BackColor2 = System.Drawing.SystemColors.Control;
            appearance26.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance26.ThemedElementAlpha = Infragistics.Win.Alpha.Transparent;
            scrollBarLook1.ButtonAppearance = appearance26;
            appearance27.BackColor = System.Drawing.Color.White;
            appearance27.BackColor2 = System.Drawing.SystemColors.Control;
            scrollBarLook1.ThumbAppearance = appearance27;
            appearance28.BackColor = System.Drawing.Color.White;
            appearance28.BackColor2 = System.Drawing.Color.White;
            scrollBarLook1.TrackAppearance = appearance28;
            this.grdSearch.DisplayLayout.ScrollBarLook = scrollBarLook1;
            this.grdSearch.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.grdSearch.Location = new System.Drawing.Point(9, 94);
            this.grdSearch.Name = "grdSearch";
            this.grdSearch.Size = new System.Drawing.Size(1010, 399);
            this.grdSearch.TabIndex = 1;
            this.grdSearch.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.grdSearch.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.grdSearch_InitializeLayout);
            this.grdSearch.DoubleClick += new System.EventHandler(this.grdSearch_DoubleClick);
            this.grdSearch.KeyDown += new System.Windows.Forms.KeyEventHandler(this.grdSearch_KeyDown);
            this.grdSearch.KeyUp += new System.Windows.Forms.KeyEventHandler(this.grdSearch_KeyUp);
            this.grdSearch.MouseClick += new System.Windows.Forms.MouseEventHandler(this.grdSearch_MouseClick);
            this.grdSearch.MouseMove += new System.Windows.Forms.MouseEventHandler(this.grdSearch_MouseMove);
            // 
            // btnAdd
            // 
            this.btnAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            appearance29.BackColor = System.Drawing.Color.White;
            appearance29.BackColor2 = System.Drawing.SystemColors.Control;
            appearance29.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance29.FontData.BoldAsString = "True";
            appearance29.ForeColor = System.Drawing.Color.Black;
            this.btnAdd.Appearance = appearance29;
            this.btnAdd.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Flat;
            appearance30.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(188)))), ((int)(((byte)(193)))), ((int)(((byte)(207)))));
            appearance30.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(188)))), ((int)(((byte)(193)))), ((int)(((byte)(207)))));
            this.btnAdd.HotTrackAppearance = appearance30;
            this.btnAdd.Location = new System.Drawing.Point(9, 502);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(102, 30);
            this.btnAdd.TabIndex = 2;
            this.btnAdd.Text = "&Add (F2)";
            this.btnAdd.UseHotTracking = Infragistics.Win.DefaultableBoolean.True;
            this.btnAdd.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
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
            this.sbMain.Location = new System.Drawing.Point(0, 539);
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
            this.sbMain.Size = new System.Drawing.Size(1034, 25);
            this.sbMain.TabIndex = 7;
            this.sbMain.ViewStyle = Infragistics.Win.UltraWinStatusBar.ViewStyle.VisualStudio2005;
            // 
            // btnEdit
            // 
            this.btnEdit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            appearance34.BackColor = System.Drawing.Color.White;
            appearance34.BackColor2 = System.Drawing.SystemColors.Control;
            appearance34.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance34.FontData.BoldAsString = "True";
            appearance34.ForeColor = System.Drawing.Color.Black;
            this.btnEdit.Appearance = appearance34;
            this.btnEdit.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Flat;
            appearance35.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(188)))), ((int)(((byte)(193)))), ((int)(((byte)(207)))));
            appearance35.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(188)))), ((int)(((byte)(193)))), ((int)(((byte)(207)))));
            this.btnEdit.HotTrackAppearance = appearance35;
            this.btnEdit.Location = new System.Drawing.Point(122, 502);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(102, 30);
            this.btnEdit.TabIndex = 3;
            this.btnEdit.Text = "&Edit (F3)";
            this.btnEdit.UseHotTracking = Infragistics.Win.DefaultableBoolean.True;
            this.btnEdit.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            appearance36.BackColor = System.Drawing.Color.White;
            appearance36.BackColor2 = System.Drawing.SystemColors.Control;
            appearance36.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance36.FontData.BoldAsString = "True";
            appearance36.ForeColor = System.Drawing.Color.Black;
            appearance36.Image = ((object)(resources.GetObject("appearance36.Image")));
            this.btnClose.Appearance = appearance36;
            this.btnClose.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Flat;
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            appearance37.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(188)))), ((int)(((byte)(193)))), ((int)(((byte)(207)))));
            appearance37.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(188)))), ((int)(((byte)(193)))), ((int)(((byte)(207)))));
            this.btnClose.HotTrackAppearance = appearance37;
            this.btnClose.Location = new System.Drawing.Point(917, 502);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(102, 30);
            this.btnClose.TabIndex = 4;
            this.btnClose.Text = "&Close";
            this.btnClose.UseHotTracking = Infragistics.Win.DefaultableBoolean.True;
            this.btnClose.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // tabMain
            // 
            appearance38.FontData.BoldAsString = "True";
            this.tabMain.ActiveTabAppearance = appearance38;
            this.tabMain.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            appearance39.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(108)))), ((int)(((byte)(172)))));
            this.tabMain.Appearance = appearance39;
            this.tabMain.BackColorInternal = System.Drawing.Color.Transparent;
            appearance40.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(108)))), ((int)(((byte)(172)))));
            appearance40.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(108)))), ((int)(((byte)(172)))));
            this.tabMain.ClientAreaAppearance = appearance40;
            this.tabMain.Controls.Add(this.ultraTabSharedControlsPage1);
            this.tabMain.Controls.Add(this.ultraTabPageControl1);
            this.tabMain.Location = new System.Drawing.Point(8, 11);
            this.tabMain.Name = "tabMain";
            this.tabMain.SharedControlsPage = this.ultraTabSharedControlsPage1;
            this.tabMain.Size = new System.Drawing.Size(1010, 75);
            this.tabMain.Style = Infragistics.Win.UltraWinTabControl.UltraTabControlStyle.PropertyPage2003;
            this.tabMain.TabIndex = 8;
            appearance41.BackColor = System.Drawing.Color.Transparent;
            ultraTab1.Appearance = appearance41;
            appearance42.BackColor = System.Drawing.Color.Transparent;
            appearance42.BackColor2 = System.Drawing.Color.Transparent;
            appearance42.ForeColor = System.Drawing.Color.Black;
            ultraTab1.ClientAreaAppearance = appearance42;
            appearance43.BackColor = System.Drawing.Color.Transparent;
            appearance43.BackColor2 = System.Drawing.Color.Transparent;
            ultraTab1.SelectedAppearance = appearance43;
            ultraTab1.TabPage = this.ultraTabPageControl1;
            ultraTab1.Text = "Criteria";
            this.tabMain.Tabs.AddRange(new Infragistics.Win.UltraWinTabControl.UltraTab[] {
            ultraTab1});
            this.tabMain.TabStop = false;
            this.tabMain.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.tabMain.ViewStyle = Infragistics.Win.UltraWinTabControl.ViewStyle.Office2003;
            // 
            // ultraTabSharedControlsPage1
            // 
            this.ultraTabSharedControlsPage1.Location = new System.Drawing.Point(-10000, -10000);
            this.ultraTabSharedControlsPage1.Name = "ultraTabSharedControlsPage1";
            this.ultraTabSharedControlsPage1.Size = new System.Drawing.Size(1006, 50);
            // 
            // btnEmailReport
            // 
            this.btnEmailReport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            appearance44.BackColor = System.Drawing.Color.White;
            appearance44.BackColor2 = System.Drawing.SystemColors.Control;
            appearance44.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance44.FontData.BoldAsString = "True";
            appearance44.ForeColor = System.Drawing.Color.Black;
            this.btnEmailReport.Appearance = appearance44;
            this.btnEmailReport.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Flat;
            appearance45.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(188)))), ((int)(((byte)(193)))), ((int)(((byte)(207)))));
            appearance45.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(188)))), ((int)(((byte)(193)))), ((int)(((byte)(207)))));
            this.btnEmailReport.HotTrackAppearance = appearance45;
            this.btnEmailReport.Location = new System.Drawing.Point(230, 502);
            this.btnEmailReport.Name = "btnEmailReport";
            this.btnEmailReport.Size = new System.Drawing.Size(117, 30);
            this.btnEmailReport.TabIndex = 9;
            this.btnEmailReport.Text = "Email &Report";
            this.btnEmailReport.UseHotTracking = Infragistics.Win.DefaultableBoolean.True;
            this.btnEmailReport.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnEmailReport.Click += new System.EventHandler(this.btnEmailReport_Click);
            // 
            // chkSelectAll
            // 
            this.chkSelectAll.AutoSize = true;
            this.chkSelectAll.Location = new System.Drawing.Point(22, 98);
            this.chkSelectAll.Name = "chkSelectAll";
            this.chkSelectAll.Size = new System.Drawing.Size(15, 14);
            this.chkSelectAll.TabIndex = 86;
            this.chkSelectAll.UseVisualStyleBackColor = true;
            this.chkSelectAll.CheckedChanged += new System.EventHandler(this.chkSelectAll_CheckedChanged);
            // 
            // btnOk
            // 
            this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            appearance46.BackColor = System.Drawing.Color.White;
            appearance46.BackColor2 = System.Drawing.SystemColors.Control;
            appearance46.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance46.FontData.BoldAsString = "True";
            appearance46.ForeColor = System.Drawing.Color.Black;
            this.btnOk.Appearance = appearance46;
            this.btnOk.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Flat;
            this.btnOk.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            appearance47.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(188)))), ((int)(((byte)(193)))), ((int)(((byte)(207)))));
            appearance47.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(188)))), ((int)(((byte)(193)))), ((int)(((byte)(207)))));
            this.btnOk.HotTrackAppearance = appearance47;
            this.btnOk.Location = new System.Drawing.Point(803, 502);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(102, 30);
            this.btnOk.TabIndex = 87;
            this.btnOk.Text = "&Ok";
            this.btnOk.UseHotTracking = Infragistics.Win.DefaultableBoolean.True;
            this.btnOk.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnOk.Visible = false;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // frmViewEODStation
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(7, 17);
            this.BackColor = System.Drawing.Color.White;
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(1034, 564);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.chkSelectAll);
            this.Controls.Add(this.btnEmailReport);
            this.Controls.Add(this.tabMain);
            this.Controls.Add(this.sbMain);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.btnEdit);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.grdSearch);
            this.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmViewEODStation";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Activated += new System.EventHandler(this.frmSearchMain_Activated);
            this.Load += new System.EventHandler(this.frmSearch_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmSearch_KeyDown);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.frmSearchMain_KeyUp);
            this.Resize += new System.EventHandler(this.frmViewEODStation_Resize);
            this.ultraTabPageControl1.ResumeLayout(false);
            this.ultraTabPageControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.clTo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.clFrom)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraDataSource1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdSearch)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sbMain)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tabMain)).EndInit();
            this.tabMain.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion
		private void btnSearch_Click(object sender, System.EventArgs e)
		{
			Search();
		}

		private void Search()
		{
			oDataSet = oBLSearch.SearchData(SearchTable,this.clFrom.Text.Trim() + " 00:00:00" ,this.clTo.Text.Trim() + " 23:59:59",ActiveOnly,-1, EODID);
            if (SearchTable == clsPOSDBConstants.StationCloseHeader_tbl)
            {
                for (int index = 0; index < oDataSet.Tables[0].Rows.Count; index++)
                {
                    oDataSet.Tables[0].Rows[index]["IsVerified"] = Configuration.convertNullToBoolean(oDataSet.Tables[0].Rows[index]["IsVerified"]);
                }
            }
            grdSearch.DataSource = oDataSet;

            #region PRIMEPOS-2494 26-Feb-2018 JY Added to allow multiselect records
            if (this.AllowMultiRowSelect == true)
            {

                if (this.grdSearch.DisplayLayout.Bands[0].Columns.Exists("CHECK") == false)
                {
                    this.grdSearch.DisplayLayout.Bands[0].Columns.Add("CHECK");
                    this.grdSearch.DisplayLayout.Bands[0].Columns["CHECK"].Header.Caption = "";

                    this.grdSearch.DisplayLayout.Bands[0].Columns["CHECK"].DataType = typeof(bool);
                    this.grdSearch.DisplayLayout.Bands[0].Columns["CHECK"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.CheckBox;
                }

                this.grdSearch.DisplayLayout.Bands[0].Columns["Check"].Header.VisiblePosition = 0;
                this.grdSearch.DisplayLayout.Bands[0].Columns["CHECK"].Width = 50;
            }
            #endregion

            this.resizeColumns();
			grdSearch.Focus();
			grdSearch.PerformAction(UltraGridAction.FirstRowInGrid);
			grdSearch.Refresh();
			sbMain.Panels[0].Text = "Record(s) Count = " + grdSearch.Rows.Count;
			//if (this.grdSearch.Rows.Count==0)
				//clsUIHelper.ShowErrorMsg("No matching row found.",POS_Core.Resources.Configuration.ApplicationName,MessageBoxButtons.OK,MessageBoxIcon.Information);
		}
		private void AddCustomer()
		{
			//if (grdSearch.Rows.Count <=0) return;

			try
			{
				frmCustomers oCustomer = new frmCustomers();
				oCustomer.Initialize();
				oCustomer.Owner = this;
				oCustomer.ShowDialog();
				if (!oCustomer.IsCanceled)
					Search();
			}
			catch(Exception exp)
			{
				clsUIHelper.ShowErrorMsg( exp.Message);
			}
		}

		private void AddDepartment()
		{
			//if (grdSearch.Rows.Count <=0) return;

			try
			{
				frmDepartment oDepartment = new frmDepartment();
				oDepartment.Initialize();
				oDepartment.ShowDialog(this);
				if (!oDepartment.IsCanceled)
					Search();
			}
			catch(Exception exp)
			{
				clsUIHelper.ShowErrorMsg( exp.Message);
			}
		}

		private void AddTaxCodes()
		{
			try
			{
				frmItemMonitorCategory oTaxCodes = new frmItemMonitorCategory();
				oTaxCodes.Initialize();
				oTaxCodes.ShowDialog(this);
				if (!oTaxCodes.IsCanceled)
					Search();
			}
			catch(Exception exp)
			{
				clsUIHelper.ShowErrorMsg( exp.Message);
			}
		}

		private void AddVendor()
		{
			//if (grdSearch.Rows.Count <=0) return;

			try
			{
				frmVendor oVendor = new frmVendor();
				oVendor.Initialize();
				oVendor.ShowDialog(this);
				if (!oVendor.IsCanceled)
					Search();
			}
			catch(Exception exp)
			{
				clsUIHelper.ShowErrorMsg( exp.Message);
			}
		}

		private void AddPurchaseOrder()
		{
			try
			{
				//frmPurchaseOrder oPO = new frmPurchaseOrder();
                frmCreateNewPurchaseOrder oPO = new frmCreateNewPurchaseOrder();
				oPO.Initialize();
				oPO.ShowDialog(this);
				if (!oPO.IsCanceled)
					Search();
			}
			catch(Exception exp)
			{
				clsUIHelper.ShowErrorMsg( exp.Message);
			}
		}

		private void AddItem()
		{
			//if (grdSearch.Rows.Count <=0) return;

			try
			{
				frmItems oItems = new frmItems();
				oItems.Initialize();
				oItems.ShowDialog(this);
				if (!oItems.IsCanceled)
					Search();
			}
			catch(Exception exp)
			{
				clsUIHelper.ShowErrorMsg( exp.Message);
			}
		}

		private void AddUser()
		{
			//if (grdSearch.Rows.Count <=0) return;

			try
			{
				UserManagement.frmUserInformation oUserInformation = new UserManagement.frmUserInformation();
				oUserInformation.SetNew();
				oUserInformation.ShowDialog(this);
				if (!oUserInformation.IsCanceled)
					Search();
			}
			catch(Exception exp)
			{
				clsUIHelper.ShowErrorMsg( exp.Message);
			}
		}

		private void btnAdd_Click(object sender, System.EventArgs e)
		{
			Add();
		}
		
		private void btnCancel_Click(object sender, System.EventArgs e)
		{
			IsCanceled = true;
			this.Close();
		}

		private void grdSearch_DoubleClick(object sender, System.EventArgs e)
		{
			try
			{
                if (btnEdit.Visible == false) return;   //PRIMEPOS-2494 26-Feb-2018 JY Added

                Point point = System.Windows.Forms.Cursor.Position;
				point = this.grdSearch.PointToClient(point);
				Infragistics.Win.UIElement oUI = this.grdSearch.DisplayLayout.UIElement.ElementFromPoint(point);
				if ( oUI == null )
					return;

				while ( oUI != null )
				{
					if ( oUI.GetType() == typeof( Infragistics.Win.UltraWinGrid.RowUIElement ) )
					{
						Infragistics.Win.UltraWinGrid.RowUIElement oRowUI = oUI as Infragistics.Win.UltraWinGrid.RowUIElement;
						Edit();

					}
					oUI = oUI.Parent;
				}
			}
			catch (Exception exp) { clsUIHelper.ShowErrorMsg(exp.Message);}
		}

		private void btnClear_Click(object sender, System.EventArgs e)
		{
			try
			{
				this.Clear();
			}
			catch(Exception exp)
			{
				clsUIHelper.ShowErrorMsg( exp.Message);
			}
		}

		private void Clear()
		{
			clFrom.Text = "";
			clTo.Text= "";

			if (oDataSet.Tables.Count !=0) oDataSet.Tables[0].Clear();

			grdSearch.Refresh();
		}

		public string SelectedRowID()
		{
			if (grdSearch.ActiveRow!=null)
				if (grdSearch.ActiveRow.Cells.Count>0)
					return grdSearch.ActiveRow.Cells[0].Text;
				else
					return "";
			else
				return "";
		}

		private void btnClear_Click_1(object sender, System.EventArgs e)
		{
			try
			{
				this.Clear();
			}
			catch(Exception exp)
			{
				clsUIHelper.ShowErrorMsg( exp.Message);
			}

		}

		private void frmSearch_Load(object sender, System.EventArgs e)
		{
			try
			{
                #region PRIMEPOS-2700 02-Jul-2019 JY Added
                if (EODID > 0)
                {
                    this.clFrom.Enabled = this.clTo.Enabled = false;
                }
                else
                {
                    this.clFrom.Enabled = this.clTo.Enabled = true;
                }
                #endregion

                this.clFrom.Value=DateTime.Today.Date.Subtract(new System.TimeSpan(30,0,0,0));
				this.clTo.Value=DateTime.Today;

				clsUIHelper.SetAppearance(this.grdSearch);
				clsUIHelper.SetReadonlyRow(this.grdSearch);
				//oDataSet = oBLSearch.SearchData(SearchTable,"----------------Invalid--------------","----------------Invalid----------------",ActiveOnly);
				//grdSearch.DataSource = oDataSet;
				//this.resizeColumns();
				//grdSearch.Refresh();
				
				this.clFrom.Enter += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
				this.clFrom.Leave += new System.EventHandler(clsUIHelper.AfterExitEditMode);

				this.clTo.Enter += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
				this.clTo.Leave += new System.EventHandler(clsUIHelper.AfterExitEditMode);
				
				if (DisplayRecordAtStartup) Search();
				setStatusBarText();
				if (this.isReadonly==true)
				{
					this.btnEdit.Visible=false;
					this.btnAdd.Visible=false;
                    this.btnEmailReport.Visible = false;    //Sprint-24 - PRIMEPOS-2363 27-Dec-2016 JY Added
                }
                if (SearchTable == clsPOSDBConstants.StationCloseHeader_tbl)
                {
                    btnAdd.Visible = false;
                    //btnEdit.Location = new Point(9, 493); //Sprint-24 - PRIMEPOS-2363 27-Dec-2016 JY Commented
                    btnEdit.Text = "View(F3)";

                    #region Sprint-24 - PRIMEPOS-2363 27-Dec-2016 JY Added
                    btnEdit.Top = btnClose.Top; 
                    btnEdit.Left = grdSearch.Left;  
                    btnEmailReport.Top = btnClose.Top;
                    btnEmailReport.Left = btnEdit.Left + btnEdit.Width + 10;
                    #endregion
                }
                if (SearchTable == clsPOSDBConstants.EndOfDay_tbl)
				{
					btnAdd.Visible = false;
                    //btnEdit.Location = new Point(9, 493); //Sprint-24 - PRIMEPOS-2363 27-Dec-2016 JY Commented
                    btnEdit.Text = "View(F3)";

                    #region Sprint-24 - PRIMEPOS-2363 27-Dec-2016 JY Added
                    btnEdit.Top = btnClose.Top;
                    btnEdit.Left = grdSearch.Left;
                    btnEmailReport.Top = btnClose.Top;
                    btnEmailReport.Left = btnEdit.Left + btnEdit.Width + 10;
                    #endregion
				}
				this.Text = this.FormCaption;
				this.lbl1.Text = this.LabelText1;
				this.lbl2.Text = this.LabelText2;

				if (this.grdSearch.Rows.Count==0)
				{
					this.clFrom.Focus();
				}
				else
				{
					grdSearch.Focus();
					grdSearch.PerformAction(UltraGridAction.FirstRowInGrid);
					grdSearch.Refresh();
				}

                #region PRIMEPOS-2494 26-Feb-2018 JY Added
                if (allowMultiRowSelect == false)
                {
                    this.grdSearch.DisplayLayout.Bands[0].Override.SelectTypeRow = SelectType.Single;
                }
                else
                {
                    this.grdSearch.DisplayLayout.Bands[0].Override.SelectTypeRow = SelectType.Extended;
                }
                this.chkSelectAll.Visible = this.allowMultiRowSelect;
                chkSelectAll.BackColor = Color.Transparent;
                this.grdSearch.DisplayLayout.Bands[0].Override.CellClickAction = CellClickAction.RowSelect;
                #endregion

                clsUIHelper.setColorSchecme(this);
				this.setTitle();

			}
			catch(Exception exp)
			{
				clsUIHelper.ShowErrorMsg( exp.Message);
			}
		}

		private void setStatusBarText()
		{
			//sbMain.Panels[1].Text = "Press F2 to Add, space or F3 to Edit and F4 to Search";
		}

		private void frmSearch_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			try
			{
				if(e.KeyData==System.Windows.Forms.Keys.Enter)
				{
					if (this.ActiveControl.Name != "grdSearch")
					{
						this.SelectNextControl(this.ActiveControl,true,true,true,true);
					}
					else
					{
						if (this.grdSearch.Rows.Count>0)
						{
							Edit();
						}
					}
				}
			}
			catch(Exception exp)
			{
				clsUIHelper.ShowErrorMsg( exp.Message);
			}
		}

		private void btnEdit_Click(object sender, System.EventArgs e)
		{
			if (grdSearch.Rows.Count <1)
				return;

			this.Edit();

		}
		private void EditCustomer()
		{
			if (grdSearch.Rows.Count <=0) return;

			try
			{
				frmCustomers oCustomer = new frmCustomers();
				oCustomer.Edit(grdSearch.ActiveRow.Cells[0].Text);
				oCustomer.ShowDialog(this);
				if (!oCustomer.IsCanceled)
					Search();
			}
			catch(Exception exp)
			{
				clsUIHelper.ShowErrorMsg( exp.Message);
			}
		}

		private void EditStationClose()
		{
			if (grdSearch.Rows.Count <=0) return;

			try
			{
				frmStationClose oStationClose = new frmStationClose();
				int id = Convert.ToInt32( grdSearch.ActiveRow.Cells[0].Text);
				oStationClose.Edit(id);
				oStationClose.StartPosition = FormStartPosition.CenterScreen;
				oStationClose.ShowDialog(this);
			}
			catch(Exception exp)
			{
				clsUIHelper.ShowErrorMsg( exp.Message);
			}
		}

		private void EndOfDayClose()
		{
			if (grdSearch.Rows.Count <=0) return;

			try
			{
				frmEndOfDay oEndOfDay= new frmEndOfDay();
				int id = Convert.ToInt32( grdSearch.ActiveRow.Cells[0].Text);
				oEndOfDay.Edit(id);
				oEndOfDay.StartPosition = FormStartPosition.CenterScreen;
				oEndOfDay.ShowDialog(this);
			}
			catch(Exception exp)
			{
				clsUIHelper.ShowErrorMsg( exp.Message);
			}
		}
		private void EditUser()
		{
			if (grdSearch.Rows.Count <=0) return;

			try
			{
				UserManagement.frmUserInformation oUserInformation = new UserManagement.frmUserInformation();
				oUserInformation.Edit(grdSearch.ActiveRow.Cells[0].Text);
				oUserInformation.ShowDialog(this);

				if (!oUserInformation.IsCanceled)
					Search();
			}
			catch(Exception exp)
			{
				clsUIHelper.ShowErrorMsg( exp.Message);
			}
		}

		private void EditVendor()
		{
			if (grdSearch.Rows.Count <=0) return;

			try
			{
				frmVendor oVendor = new frmVendor();
				oVendor.Edit(grdSearch.ActiveRow.Cells[0].Text);
				oVendor.ShowDialog(this);
				if (!oVendor.IsCanceled)
					Search();
			}
			catch(Exception exp)
			{
				clsUIHelper.ShowErrorMsg( exp.Message);
			}
		}


		private void EditPurchaseOrder()
		{
			if (grdSearch.Rows.Count <=0) return;

			try
			{
				//frmPurchaseOrder oPO = new frmPurchaseOrder();
                frmCreateNewPurchaseOrder oPO = new frmCreateNewPurchaseOrder();
				oPO.Edit(grdSearch.ActiveRow.Cells[0].Text);
				oPO.ShowDialog(this);
				if (!oPO.IsCanceled)
					Search();
			}
			catch(Exception exp)
			{
				clsUIHelper.ShowErrorMsg( exp.Message);
			}
		}

		private void EditDepartment()
		{
			if (grdSearch.Rows.Count <=0) return;

			try
			{
				frmDepartment oDepartment = new frmDepartment();
				oDepartment.Edit(grdSearch.ActiveRow.Cells[0].Text);
				oDepartment.ShowDialog(this);
				if (!oDepartment.IsCanceled)
					Search();
			}
			catch(Exception exp)
			{
				clsUIHelper.ShowErrorMsg( exp.Message);
			}
		}

		private void EditTaxCodes()
		{
			if (grdSearch.Rows.Count <=0) return;

			try
			{
				frmItemMonitorCategory oTaxCodes = new frmItemMonitorCategory();
				oTaxCodes.Edit(grdSearch.ActiveRow.Cells[0].Text);
				oTaxCodes.ShowDialog(this);
				if (!oTaxCodes.IsCanceled)
					Search();
			}
			catch(Exception exp)
			{
				clsUIHelper.ShowErrorMsg( exp.Message);
			}
		}

		private void EditItem()
		{
			if (grdSearch.Rows.Count <=0) return;

			try
			{
				frmItems oItems = new frmItems();
				oItems.Edit(grdSearch.ActiveRow.Cells[0].Text);
				oItems.ShowDialog(this);
				if (!oItems.IsCanceled)
					Search();
			}
			catch(Exception exp)
			{
				clsUIHelper.ShowErrorMsg(exp.Message);
			}
		}

		private void btnClose_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}

		private void grdSearch_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			//if (e.KeyData == System.Windows.Forms.Keys.Enter)
			//	Edit();
		}

		private void grdSearch_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
		{
		}
		
		private void Add()
		{
			try
			{
				if (frmMain.oPublicNumericPad !=null && (!frmMain.oPublicNumericPad.IsDisposed)) frmMain.oPublicNumericPad.RemoveParent();
				switch(SearchTable)
				{
					case clsPOSDBConstants.Customer_tbl:
						AddCustomer();
					break;
					case clsPOSDBConstants.Department_tbl:
						AddDepartment();
						break;
					case clsPOSDBConstants.TaxCodes_tbl:
						AddTaxCodes();
						break;
					case clsPOSDBConstants.Item_tbl:
						AddItem();
						break;
					case clsPOSDBConstants.Vendor_tbl:
						AddVendor();
						break;
					case clsPOSDBConstants.POHeader_tbl:
						AddPurchaseOrder();
						break;
					case clsPOSDBConstants.Users_tbl:
						AddUser();
						break;
				}
                if (frmMain.oPublicNumericPad != null && (!frmMain.oPublicNumericPad.IsDisposed)) frmMain.oPublicNumericPad.AttachParent(new IntPtr(0));
			}
			catch(Exception exp)
			{
                if (frmMain.oPublicNumericPad != null && (!frmMain.oPublicNumericPad.IsDisposed)) frmMain.oPublicNumericPad.AttachParent(new IntPtr(0));
				clsUIHelper.ShowErrorMsg(exp.Message);
			}
		}

		private void Edit()
		{
			try
			{

				if (grdSearch.ActiveRow==null)
					if (grdSearch.ActiveRow.Cells.Count==0)
						return;
				
				if (frmMain.oPublicNumericPad !=null && (!frmMain.oPublicNumericPad.IsDisposed)) frmMain.oPublicNumericPad.RemoveParent();

				switch(SearchTable)
				{
					case clsPOSDBConstants.StationCloseHeader_tbl:
						EditStationClose();
						break;
					case clsPOSDBConstants.EndOfDay_tbl:
						EndOfDayClose();
						break;
					case clsPOSDBConstants.Customer_tbl:
						EditCustomer();
						break;
					case clsPOSDBConstants.Department_tbl:
						EditDepartment();
						break;
					case clsPOSDBConstants.TaxCodes_tbl:
						EditTaxCodes();
						break;
					case clsPOSDBConstants.Item_tbl:
						EditItem();
						break;
					case clsPOSDBConstants.Vendor_tbl:
						EditVendor();
						break;
					case clsPOSDBConstants.POHeader_tbl:
						EditPurchaseOrder();
						break;
					case clsPOSDBConstants.Users_tbl:
						EditUser();
						break;

					}
					if (frmMain.oPublicNumericPad !=null && (!frmMain.oPublicNumericPad.IsDisposed)) frmMain.oPublicNumericPad.AttachParent(new IntPtr(0));
			}
			catch(Exception exp)
			{
                if (frmMain.oPublicNumericPad != null && (!frmMain.oPublicNumericPad.IsDisposed)) frmMain.oPublicNumericPad.AttachParent(new IntPtr(0));
				clsUIHelper.ShowErrorMsg(exp.Message);
			}

		}

		private void frmSearchMain_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			switch(e.KeyData)
			{
				case Keys.F2:
					Add();
				break;
				case Keys.F3:
					Edit();
				break;
				case Keys.F4:
					Search();
				break;
			}
		}

		private void grdSearch_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
		{
			for (int i=0; i<this.grdSearch.DisplayLayout.Bands[0].Columns.Count;i++)
			{
				if ( this.grdSearch.DisplayLayout.Bands[0].Columns[i].DataType==typeof(System.Decimal))
				{
					this.grdSearch.DisplayLayout.Bands[0].Columns[i].Format="#######0.00";
					this.grdSearch.DisplayLayout.Bands[0].Columns[i].CellAppearance.TextHAlign=Infragistics.Win.HAlign.Right;
				}
			}
		}

		private void frmSearchMain_Activated(object sender, System.EventArgs e)
		{
			clsUIHelper.CurrentForm=this;
		}

		private void grdSearch_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			this.CurrentX=e.X;
			this.CurrentY=e.Y;
		}

		private void TextBoxKeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			if (e.KeyData==Keys.Down)
			{
				if (this.grdSearch.Rows.Count>0)
				{
					this.grdSearch.Focus();
					this.grdSearch.ActiveRow=this.grdSearch.Rows[0];
				}
			}
		}

		private void resizeColumns()
		{
			foreach (UltraGridColumn oCol in grdSearch.DisplayLayout.Bands[0].Columns)
			{
				oCol.Width =oCol.CalculateAutoResizeWidth(PerformAutoSizeType.VisibleRows,true)+10;
				if ( oCol.DataType.Equals(typeof(System.Int32)) || oCol.DataType.Equals(typeof(System.Decimal)) || oCol.DataType.Equals(typeof(System.DateTime)))
				{
					oCol.CellAppearance.TextHAlign=Infragistics.Win.HAlign.Right ;
				}
				//oDataSet.Tables[0].Columns[oCol.Key].DataType.
            }
            #region Sprint-18 14-Nov-2014 JY Added - fix by Manoj 2-20-2015 Date is wrong
            if (grdSearch.DisplayLayout.Bands[0].Columns.Count > 2)
            {
                grdSearch.DisplayLayout.Bands[0].Columns[2].Format = "MM/dd/yyyy HH:mm:ss";//"dd/mm/yyyy hh:mm:ss"; 
                grdSearch.DisplayLayout.Bands[0].Columns[2].Width = 140;    
            }
            #endregion
        }

		private void setTitle()
		{
			string strCaption="";
			switch (SearchTable)
			{
				case clsPOSDBConstants.item_PriceInv_Lookup:
					strCaption="Search " + "Item Price Lookup";
					break;
				case clsPOSDBConstants.Customer_tbl:
					strCaption="Search " + "Customer";
					break;
				case clsPOSDBConstants.Department_tbl:
					strCaption="Search " + "Department";
					break;
				case clsPOSDBConstants.FunctionKeys_tbl:
					strCaption="Search " + "Function Keys";
					break;
				case clsPOSDBConstants.InvRecvHeader_tbl:
					strCaption="Search " + "Inventory Received";    //PRIMEPOS-2824 25-Mar-2020 JY modified
                    break;
				case clsPOSDBConstants.Item_tbl:
					strCaption="Search " + "Item File";
					break;
				case clsPOSDBConstants.PayOut_tbl:
					strCaption="Search " + "Payout";
					break;
				case clsPOSDBConstants.PayType_tbl:
					strCaption="Search " + "Payment Type";
					break;
				case clsPOSDBConstants.PhysicalInv_tbl:
					strCaption="Search " + "Physical Inventory";
					break;
				case clsPOSDBConstants.POHeader_tbl:
					strCaption="Search " + "Purchase Order";
					break;
				case clsPOSDBConstants.TaxCodes_tbl:
					strCaption="Search " + "Tax Table";
					break;
				case clsPOSDBConstants.TransHeader_tbl:
					strCaption="Search " + "POS Transaction";
					break;
				case clsPOSDBConstants.Users_tbl:
					strCaption="Search " + "Users Information";
					break;
				case clsPOSDBConstants.Vendor_tbl:
					strCaption="Search " + "Vendors";
					break;
				case clsPOSDBConstants.StationCloseHeader_tbl:
					strCaption="View Station Close";
					break;
				case clsPOSDBConstants.EndOfDay_tbl:
					strCaption="View End Of Day";
					break;
			}
			this.Text=strCaption;
		}

        private void frmViewEODStation_Resize(object sender, EventArgs e)
        {
            this.Left = clsPOSDBConstants.formLeft;
            this.Top = clsPOSDBConstants.formLeft;
        }

        #region Sprint-24 - PRIMEPOS-2363 27-Dec-2016 JY Added
        private void btnEmailReport_Click(object sender, EventArgs e)
        {
            try
            {
                if (grdSearch.Rows.Count < 1)
                    return;

                if (grdSearch.ActiveRow == null)
                    if (grdSearch.ActiveRow.Cells.Count == 0)
                        return;

                switch (SearchTable)
                {
                    case clsPOSDBConstants.StationCloseHeader_tbl:
                        EmailStationCloseReport();
                        break;
                    case clsPOSDBConstants.EndOfDay_tbl:
                        EmailEODReport();
                        break;

                    default:
                        break;
                }
            }
            catch(Exception Ex)
            {
                clsUIHelper.ShowErrorMsg(Ex.Message);
            }
        }

        private void EmailStationCloseReport()
        {
            if (grdSearch.Rows.Count <= 0) return;

            try
            {
                frmStationClose oStationClose = new frmStationClose();
                int StationCloseId = Convert.ToInt32(grdSearch.ActiveRow.Cells[0].Text);
                oStationClose.EmailReport(StationCloseId);
            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }

        private void EmailEODReport()
        {
            if (grdSearch.Rows.Count <= 0) return;

            try
            {
                frmEndOfDay ofrmEndOfDay = new frmEndOfDay();
                string sEODId = grdSearch.ActiveRow.Cells[0].Text;
                ofrmEndOfDay.EmailReport(sEODId);
            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }
        #endregion

        #region PRIMEPOS-2494 26-Feb-2018 JY Added
        private void grdSearch_MouseClick(object sender, MouseEventArgs e)
        {
            if (this.allowMultiRowSelect == true)
            {

                Point point = System.Windows.Forms.Cursor.Position;
                point = this.grdSearch.PointToClient(point);
                Infragistics.Win.UIElement oUI = this.grdSearch.DisplayLayout.UIElement.ElementFromPoint(point, Infragistics.Win.UIElementInputType.MouseClick);
                if (oUI == null)
                    return;

                while (oUI != null)
                {
                    if (oUI.GetType() == typeof(Infragistics.Win.UltraWinGrid.CellUIElement))
                    {
                        Infragistics.Win.UltraWinGrid.CellUIElement cellUIElement = (Infragistics.Win.UltraWinGrid.CellUIElement)oUI;
                        if (cellUIElement.Column.Key.ToUpper() == "CHECK")
                        {
                            CheckUncheckGridRow(cellUIElement.Cell);
                        }
                        break;
                    }
                    oUI = oUI.Parent;
                }
            }
        }

        private void CheckUncheckGridRow(UltraGridCell oCell)
        {
            if ((bool)oCell.Value == false)
            {
                oCell.Value = true;
            }
            else
            {
                oCell.Value = false;
            }
            oCell.Row.Update();
        }        

        private void chkSelectAll_CheckedChanged(object sender, EventArgs e)
        {
            grdSearch.BeginUpdate();
            foreach (UltraGridRow oRow in grdSearch.Rows)
            {
                oRow.Cells["check"].Value = chkSelectAll.Checked;
                oRow.Update();
            }
            grdSearch.EndUpdate();
        }        
        
        private void btnOk_Click(object sender, EventArgs e)
        {
            IsCanceled = false;
            this.Close();
        }
        #endregion
    }
}
