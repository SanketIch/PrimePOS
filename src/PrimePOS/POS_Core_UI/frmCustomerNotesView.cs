using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using POS_Core.CommonData.Rows;
using POS_Core.CommonData;
using POS_Core.BusinessRules;
using POS_Core.ErrorLogging;
using Infragistics.Win;
using System.Data;
using Infragistics.Win.UltraWinGrid;
using POS_Core.Resources;
//using POS_Core.DataAccess;

namespace POS_Core_UI
{
	/// <summary>
	/// Summary description for frmCustomerNotesView.
	/// </summary>
	public class frmCustomerNotesView : System.Windows.Forms.Form
	{
		private Infragistics.Win.UltraWinDataSource.UltraDataSource ultraDataSource2;
		private Infragistics.Win.Misc.UltraLabel lblTransactionType;
		private Infragistics.Win.Misc.UltraButton btnClose;
		private Infragistics.Win.UltraWinGrid.UltraGrid grdHistory;
		private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private IContainer components;

        private Int32 iCustomerID;

        //Added By Shitaljit(QuicSolv) 0n 8 oct 2011 
        CustomerNotes oCNotes = new CustomerNotes();
        CustomerNotesData oData = null;
        Notes oNotes = new Notes();
        NotesData oNotesData = new NotesData();
        NotesRow oNotesRow = null;
        private string strEntityID = "";
        private string strEntityType = "";
        private Infragistics.Win.Misc.UltraButton btnSave;
        private Infragistics.Win.UltraWinDataSource.UltraDataSource ultraDataSource1;
        private bool isCoustomerNote = false;
        private string sNotesToDispaly = string.Empty;
        public frmCustomerNotesView(string EntityId, string EntityType)
        {
            InitializeComponent();
            strEntityID = EntityId;
            strEntityType = EntityType;
            if (EntityType == clsEntityType.ItemNote)
            {
                lblTransactionType.Text = "Item Notes";
                this.Text = "Item Notes";
            }
            if (EntityType == clsEntityType.DepartmentNote)
            {
                lblTransactionType.Text = "Department Notes";
                this.Text = "Department Notes";
            }
            if (EntityType == clsEntityType.VendorNote)
            {
                lblTransactionType.Text = "Vendor Notes";
                this.Text = "Vendor Notes ";
            }
            if (EntityType == clsEntityType.SystemNote)
            {
                lblTransactionType.Text = "System Notes";
                this.Text = "System Notes";
            }
            if (EntityType == clsEntityType.UserNote)
            {
                lblTransactionType.Text = "User Notes";
                this.Text = "User Notes";
            }
            if (EntityType == clsEntityType.PatNote)
            {
                lblTransactionType.Text = "Patient Notes";
                this.Text = "Patient Notes";
            }
            if (EntityType == clsEntityType.RXNote)
            {
                lblTransactionType.Text = "RX Notes";
                this.Text = "RX Notes";
            }
        }
        public void Initialize( DataTable dtNotes)
        {
            int RowIndex = 0;
            string strEntityId = string.Empty;
            foreach (DataRow oRow in dtNotes.Rows)
            {
                sNotesToDispaly = Configuration.convertNullToString(oRow["Note"]);
                strEntityId = Configuration.convertNullToString(oRow["EntityId"]);
                if (strEntityId.Length > 20)
                    strEntityId = strEntityId.Substring(0, 19);
                //oNotesData.Notes.AddRow(RowIndex, "", strEntityType, sNotesToDispaly, System.DateTime.Now, "", System.DateTime.Now, "", true);    //PRIMEPOS-2633 29-Jan-2019 JY Commented
                oNotesData.Notes.AddRow(RowIndex, strEntityId, strEntityType, sNotesToDispaly, System.DateTime.Now, Configuration.UserName, System.DateTime.Now, Configuration.UserName, true); //PRIMEPOS-2633 29-Jan-2019 JY Added
                RowIndex++;
            }
           
        }
        //END of Added By Shitaljit(QuicSolv) 0n 8 oct 2011 

		public frmCustomerNotesView(Int32 iID)
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
            iCustomerID = iID;
            isCoustomerNote = true; //Added By Shitaljit(QuicSolv) 0n 8 oct 2011 
			//
			// TODO: Add any constructor code after InitializeComponent call
			//
		}

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
            Infragistics.Win.Appearance appearance28 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand1 = new Infragistics.Win.UltraWinGrid.UltraGridBand("Band 0", -1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn1 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("ID");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn2 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Don\'t Show", 0);
            Infragistics.Win.Appearance appearance29 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance30 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance31 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance32 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance33 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance34 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance35 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance36 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance37 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance38 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance39 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance40 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance41 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance42 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance43 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance44 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance45 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance46 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance47 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinScrollBar.ScrollBarLook scrollBarLook1 = new Infragistics.Win.UltraWinScrollBar.ScrollBarLook();
            Infragistics.Win.Appearance appearance48 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance49 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance50 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance51 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinDataSource.UltraDataColumn ultraDataColumn4 = new Infragistics.Win.UltraWinDataSource.UltraDataColumn("ID");
            Infragistics.Win.UltraWinDataSource.UltraDataColumn ultraDataColumn5 = new Infragistics.Win.UltraWinDataSource.UltraDataColumn("Amount");
            Infragistics.Win.UltraWinDataSource.UltraDataColumn ultraDataColumn6 = new Infragistics.Win.UltraWinDataSource.UltraDataColumn("Reference");
            Infragistics.Win.Appearance appearance25 = new Infragistics.Win.Appearance();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmCustomerNotesView));
            Infragistics.Win.Appearance appearance26 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            this.grdHistory = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.ultraDataSource1 = new Infragistics.Win.UltraWinDataSource.UltraDataSource(this.components);
            this.ultraDataSource2 = new Infragistics.Win.UltraWinDataSource.UltraDataSource(this.components);
            this.btnClose = new Infragistics.Win.Misc.UltraButton();
            this.lblTransactionType = new Infragistics.Win.Misc.UltraLabel();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnSave = new Infragistics.Win.Misc.UltraButton();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            ((System.ComponentModel.ISupportInitialize)(this.grdHistory)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraDataSource1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraDataSource2)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // grdHistory
            // 
            this.grdHistory.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.grdHistory.DataSource = this.ultraDataSource1;
            appearance28.BackColor = System.Drawing.Color.White;
            appearance28.BackColor2 = System.Drawing.Color.White;
            appearance28.BackColorDisabled = System.Drawing.Color.White;
            appearance28.BackColorDisabled2 = System.Drawing.Color.White;
            appearance28.BackGradientStyle = Infragistics.Win.GradientStyle.None;
            this.grdHistory.DisplayLayout.Appearance = appearance28;
            ultraGridColumn1.Header.VisiblePosition = 0;
            ultraGridColumn1.Hidden = true;
            ultraGridColumn1.RowLayoutColumnInfo.PreferredCellSize = new System.Drawing.Size(34, 0);
            ultraGridColumn2.CellMultiLine = Infragistics.Win.DefaultableBoolean.True;
            ultraGridColumn2.DataType = typeof(bool);
            ultraGridColumn2.DefaultCellValue = false;
            ultraGridColumn2.Header.VisiblePosition = 1;
            ultraGridColumn2.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.CheckBox;
            ultraGridBand1.Columns.AddRange(new object[] {
            ultraGridColumn1,
            ultraGridColumn2});
            ultraGridBand1.UseRowLayout = true;
            this.grdHistory.DisplayLayout.BandsSerializer.Add(ultraGridBand1);
            this.grdHistory.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.grdHistory.DisplayLayout.InterBandSpacing = 10;
            this.grdHistory.DisplayLayout.MaxColScrollRegions = 1;
            this.grdHistory.DisplayLayout.MaxRowScrollRegions = 1;
            appearance29.BackColor = System.Drawing.Color.White;
            appearance29.BackColor2 = System.Drawing.Color.White;
            this.grdHistory.DisplayLayout.Override.ActiveCardCaptionAppearance = appearance29;
            appearance30.BackColor = System.Drawing.Color.White;
            appearance30.BackColor2 = System.Drawing.Color.White;
            appearance30.BorderColor = System.Drawing.Color.Gray;
            this.grdHistory.DisplayLayout.Override.ActiveRowAppearance = appearance30;
            appearance31.BackColor = System.Drawing.Color.White;
            appearance31.BackColor2 = System.Drawing.Color.White;
            appearance31.BorderColor = System.Drawing.Color.Gray;
            this.grdHistory.DisplayLayout.Override.AddRowAppearance = appearance31;
            this.grdHistory.DisplayLayout.Override.AllowAddNew = Infragistics.Win.UltraWinGrid.AllowAddNew.No;
            this.grdHistory.DisplayLayout.Override.AllowColMoving = Infragistics.Win.UltraWinGrid.AllowColMoving.WithinBand;
            this.grdHistory.DisplayLayout.Override.AllowColSwapping = Infragistics.Win.UltraWinGrid.AllowColSwapping.NotAllowed;
            this.grdHistory.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.False;
            this.grdHistory.DisplayLayout.Override.AllowRowFiltering = Infragistics.Win.DefaultableBoolean.False;
            this.grdHistory.DisplayLayout.Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.True;
            appearance32.BackColor = System.Drawing.Color.Transparent;
            this.grdHistory.DisplayLayout.Override.CardAreaAppearance = appearance32;
            appearance33.BackColor = System.Drawing.Color.White;
            appearance33.BackColor2 = System.Drawing.Color.White;
            appearance33.BackColorDisabled = System.Drawing.Color.White;
            appearance33.BackColorDisabled2 = System.Drawing.Color.White;
            appearance33.BorderColor = System.Drawing.Color.Black;
            appearance33.BorderColor3DBase = System.Drawing.Color.Black;
            this.grdHistory.DisplayLayout.Override.CellAppearance = appearance33;
            appearance34.BackColor = System.Drawing.Color.White;
            appearance34.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(231)))), ((int)(((byte)(230)))));
            appearance34.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance34.BorderAlpha = Infragistics.Win.Alpha.Transparent;
            appearance34.BorderColor = System.Drawing.Color.Gray;
            appearance34.BorderColor3DBase = System.Drawing.Color.Gray;
            appearance34.ImageBackgroundAlpha = Infragistics.Win.Alpha.Transparent;
            appearance34.ImageBackgroundStyle = Infragistics.Win.ImageBackgroundStyle.Centered;
            this.grdHistory.DisplayLayout.Override.CellButtonAppearance = appearance34;
            appearance35.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            appearance35.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.grdHistory.DisplayLayout.Override.EditCellAppearance = appearance35;
            appearance36.BorderColor = System.Drawing.Color.Gray;
            this.grdHistory.DisplayLayout.Override.FilteredInRowAppearance = appearance36;
            appearance37.BorderColor = System.Drawing.Color.Gray;
            this.grdHistory.DisplayLayout.Override.FilteredOutRowAppearance = appearance37;
            appearance38.BackColor = System.Drawing.Color.White;
            appearance38.BackColor2 = System.Drawing.Color.White;
            appearance38.BackColorDisabled = System.Drawing.Color.White;
            appearance38.BackColorDisabled2 = System.Drawing.Color.White;
            this.grdHistory.DisplayLayout.Override.FixedCellAppearance = appearance38;
            appearance39.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            appearance39.BackColor2 = System.Drawing.Color.Beige;
            this.grdHistory.DisplayLayout.Override.FixedHeaderAppearance = appearance39;
            appearance40.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(62)))), ((int)(((byte)(176)))));
            appearance40.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance40.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance40.FontData.BoldAsString = "True";
            appearance40.FontData.SizeInPoints = 10F;
            appearance40.ForeColor = System.Drawing.Color.White;
            appearance40.TextHAlignAsString = "Left";
            appearance40.ThemedElementAlpha = Infragistics.Win.Alpha.Transparent;
            this.grdHistory.DisplayLayout.Override.HeaderAppearance = appearance40;
            this.grdHistory.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortSingle;
            this.grdHistory.DisplayLayout.Override.MergedCellStyle = Infragistics.Win.UltraWinGrid.MergedCellStyle.Never;
            appearance41.BorderColor = System.Drawing.Color.Gray;
            this.grdHistory.DisplayLayout.Override.RowAlternateAppearance = appearance41;
            appearance42.BackColor = System.Drawing.Color.White;
            appearance42.BackColor2 = System.Drawing.Color.White;
            appearance42.BackGradientStyle = Infragistics.Win.GradientStyle.None;
            appearance42.BackHatchStyle = Infragistics.Win.BackHatchStyle.None;
            appearance42.BorderColor = System.Drawing.Color.Gray;
            this.grdHistory.DisplayLayout.Override.RowAppearance = appearance42;
            appearance43.BorderColor = System.Drawing.Color.Gray;
            this.grdHistory.DisplayLayout.Override.RowPreviewAppearance = appearance43;
            appearance44.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(62)))), ((int)(((byte)(176)))));
            appearance44.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance44.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance44.BorderColor = System.Drawing.Color.Gray;
            this.grdHistory.DisplayLayout.Override.RowSelectorAppearance = appearance44;
            this.grdHistory.DisplayLayout.Override.RowSelectorWidth = 12;
            this.grdHistory.DisplayLayout.Override.RowSizingArea = Infragistics.Win.UltraWinGrid.RowSizingArea.EntireRow;
            this.grdHistory.DisplayLayout.Override.RowSpacingBefore = 2;
            appearance45.BackColor = System.Drawing.Color.Navy;
            appearance45.BackColorAlpha = Infragistics.Win.Alpha.Opaque;
            this.grdHistory.DisplayLayout.Override.SelectedCellAppearance = appearance45;
            appearance46.BackColor = System.Drawing.Color.Navy;
            appearance46.BackColorDisabled = System.Drawing.Color.Navy;
            appearance46.BackGradientStyle = Infragistics.Win.GradientStyle.None;
            appearance46.BackHatchStyle = Infragistics.Win.BackHatchStyle.None;
            appearance46.BorderColor = System.Drawing.Color.Gray;
            appearance46.ForeColor = System.Drawing.Color.White;
            this.grdHistory.DisplayLayout.Override.SelectedRowAppearance = appearance46;
            this.grdHistory.DisplayLayout.Override.SelectTypeCell = Infragistics.Win.UltraWinGrid.SelectType.None;
            this.grdHistory.DisplayLayout.Override.SelectTypeCol = Infragistics.Win.UltraWinGrid.SelectType.None;
            this.grdHistory.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.None;
            appearance47.BorderColor = System.Drawing.Color.Gray;
            this.grdHistory.DisplayLayout.Override.TemplateAddRowAppearance = appearance47;
            this.grdHistory.DisplayLayout.RowConnectorColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            this.grdHistory.DisplayLayout.RowConnectorStyle = Infragistics.Win.UltraWinGrid.RowConnectorStyle.Solid;
            appearance48.BackColor = System.Drawing.Color.White;
            appearance48.BackColor2 = System.Drawing.Color.Gainsboro;
            appearance48.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance48.BackHatchStyle = Infragistics.Win.BackHatchStyle.Horizontal;
            appearance48.BorderColor = System.Drawing.Color.WhiteSmoke;
            appearance48.BorderColor3DBase = System.Drawing.Color.WhiteSmoke;
            appearance48.ImageBackgroundStyle = Infragistics.Win.ImageBackgroundStyle.Stretched;
            scrollBarLook1.Appearance = appearance48;
            appearance49.BackColor = System.Drawing.Color.LightGray;
            appearance49.BackColor2 = System.Drawing.Color.WhiteSmoke;
            appearance49.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            scrollBarLook1.ButtonAppearance = appearance49;
            appearance50.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            scrollBarLook1.ThumbAppearance = appearance50;
            appearance51.BackColor = System.Drawing.Color.Gainsboro;
            appearance51.BackColor2 = System.Drawing.Color.WhiteSmoke;
            appearance51.BackGradientStyle = Infragistics.Win.GradientStyle.None;
            appearance51.BackHatchStyle = Infragistics.Win.BackHatchStyle.None;
            appearance51.BorderAlpha = Infragistics.Win.Alpha.UseAlphaLevel;
            appearance51.BorderColor = System.Drawing.Color.White;
            appearance51.BorderColor3DBase = System.Drawing.Color.Gainsboro;
            appearance51.ImageBackgroundOrigin = Infragistics.Win.ImageBackgroundOrigin.Form;
            scrollBarLook1.TrackAppearance = appearance51;
            this.grdHistory.DisplayLayout.ScrollBarLook = scrollBarLook1;
            this.grdHistory.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.grdHistory.Location = new System.Drawing.Point(14, 24);
            this.grdHistory.Name = "grdHistory";
            this.grdHistory.Size = new System.Drawing.Size(664, 312);
            this.grdHistory.TabIndex = 0;
            this.grdHistory.UpdateMode = Infragistics.Win.UltraWinGrid.UpdateMode.OnCellChangeOrLostFocus;
            this.grdHistory.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.grdHistory.Leave += new System.EventHandler(this.grdHistory_Leave);
            this.grdHistory.KeyDown += new System.Windows.Forms.KeyEventHandler(this.grdHistory_KeyDown);
            // 
            // ultraDataSource1
            // 
            this.ultraDataSource1.Band.Columns.AddRange(new object[] {
            ultraDataColumn4});
            // 
            // ultraDataSource2
            // 
            this.ultraDataSource2.Band.Columns.AddRange(new object[] {
            ultraDataColumn5,
            ultraDataColumn6});
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            appearance25.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(62)))), ((int)(((byte)(176)))));
            appearance25.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance25.FontData.BoldAsString = "True";
            appearance25.ForeColor = System.Drawing.Color.White;
            appearance25.Image = ((object)(resources.GetObject("appearance25.Image")));
            this.btnClose.Appearance = appearance25;
            this.btnClose.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(581, 14);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(100, 26);
            this.btnClose.TabIndex = 3;
            this.btnClose.Text = "&Close";
            this.btnClose.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // lblTransactionType
            // 
            this.lblTransactionType.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            appearance26.ForeColor = System.Drawing.Color.White;
            appearance26.ForeColorDisabled = System.Drawing.Color.Navy;
            appearance26.TextHAlignAsString = "Center";
            this.lblTransactionType.Appearance = appearance26;
            this.lblTransactionType.BackColorInternal = System.Drawing.Color.Transparent;
            this.lblTransactionType.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTransactionType.Location = new System.Drawing.Point(22, 15);
            this.lblTransactionType.Name = "lblTransactionType";
            this.lblTransactionType.Size = new System.Drawing.Size(690, 38);
            this.lblTransactionType.TabIndex = 26;
            this.lblTransactionType.Tag = "Header";
            this.lblTransactionType.Text = "Customer Notes";
            this.lblTransactionType.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.btnSave);
            this.groupBox2.Controls.Add(this.btnClose);
            this.groupBox2.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.groupBox2.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.Location = new System.Drawing.Point(22, 410);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(690, 50);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            // 
            // btnSave
            // 
            appearance4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(62)))), ((int)(((byte)(176)))));
            appearance4.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance4.FontData.BoldAsString = "True";
            appearance4.ForeColor = System.Drawing.Color.White;
            appearance4.Image = ((object)(resources.GetObject("appearance4.Image")));
            this.btnSave.Appearance = appearance4;
            this.btnSave.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Flat;
            this.btnSave.Location = new System.Drawing.Point(468, 14);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(103, 26);
            this.btnSave.TabIndex = 2;
            this.btnSave.Text = "&Ok";
            this.btnSave.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.btnSave.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox3.Controls.Add(this.grdHistory);
            this.groupBox3.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.groupBox3.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox3.ForeColor = System.Drawing.Color.White;
            this.groupBox3.Location = new System.Drawing.Point(22, 58);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(690, 348);
            this.groupBox3.TabIndex = 0;
            this.groupBox3.TabStop = false;
            // 
            // frmCustomerNotesView
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(108)))), ((int)(((byte)(172)))));
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(724, 479);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.lblTransactionType);
            this.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmCustomerNotesView";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Customer Notes";
            this.Load += new System.EventHandler(this.frmCustomerNotesView_Load);
            this.Activated += new System.EventHandler(this.frmCustomerNotesView_Activated);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmCustomerNotesView_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.grdHistory)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraDataSource1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraDataSource2)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.ResumeLayout(false);

		}
		#endregion

		private void frmCustomerNotesView_Load(object sender, System.EventArgs e)
		{
			try
			{
				Display();
				clsUIHelper.SetReadonlyRow(this.grdHistory);
				clsUIHelper.SetAppearance(this.grdHistory);
				clsUIHelper.setColorSchecme(this);
                this.grdHistory.Focus();
			}
			catch(Exception exp)
			{
				clsUIHelper.ShowErrorMsg( exp.Message);
			}
		}

        private void Display() 
		{
            //Following if-else is Added By Shitaljit(QuicSolv) 0n 10 oct 2011 
            if (isCoustomerNote == true)
            {
                oData = oCNotes.Populate(iCustomerID, true);
                this.grdHistory.DataSource = oData;
                this.grdHistory.Refresh();
                ApplyGrigFormat();
                return;
                //resizeColumns();
            }
            if (strEntityType == clsEntityType.RXNote || strEntityType == clsEntityType.PatNote)
            {
                this.grdHistory.DataSource = oNotesData;
                this.grdHistory.Refresh();
                ApplyGrigFormat();
            }
            else
            {
                string whereClause = " WHERE " + clsPOSDBConstants.Notes_Fld_EntityId + "= '" + strEntityID + "'  AND  " + clsPOSDBConstants.Notes_Fld_EntityType + "= '" + strEntityType + "' AND " + clsPOSDBConstants.Notes_Fld_POPUPMSG + "= '" + true + "'";
                oNotesData = oNotes.PopulateList(whereClause);
                this.grdHistory.DataSource = oNotesData;
                this.grdHistory.Refresh();
                ApplyGrigFormat();
            }
		}

        private void resizeColumns()
        {
            try
            {
                foreach (UltraGridBand oBand in grdHistory.DisplayLayout.Bands)
                {
                    foreach (UltraGridColumn oCol in oBand.Columns)
                    {
                        oCol.Width = oCol.CalculateAutoResizeWidth(PerformAutoSizeType.VisibleRows, true) + 10;
                        if (oCol.DataType.Equals(typeof(System.Int32)) || oCol.DataType.Equals(typeof(System.Decimal)) || oCol.DataType.Equals(typeof(System.DateTime)))
                        {
                            oCol.CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right;
                        }
                    }
                }
            }
            catch (Exception) { }
        }

		private void ApplyGrigFormat()
		{
            //Following if-else is Added By Shitaljit(QuicSolv) 0n 10 oct 2011 
            if (isCoustomerNote == true)
            {
                this.grdHistory.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.CustomerNotes_Fld_CustomerID].Hidden = true;
                this.grdHistory.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.CustomerNotes_Fld_Notes].Header.Caption = "Notes";
                this.grdHistory.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.CustomerNotes_Fld_UserID].Header.Caption = "User ID";
                this.grdHistory.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.CustomerNotes_Fld_LastUpdatedOn].Header.Caption = "Last Updated On";
                this.grdHistory.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.CustomerNotes_Fld_IsActive].Hidden = true;
                this.grdHistory.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.CustomerNotes_Fld_LastUpdatedOn].Hidden = true;
                this.grdHistory.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.CustomerNotes_Fld_UserID].Hidden = true;
                this.grdHistory.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.CustomerNotes_Fld_LastUpdatedOn].CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit;
                this.grdHistory.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.CustomerNotes_Fld_Notes].Width = 400;
                this.grdHistory.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.CustomerNotes_Fld_Notes].CellMultiLine = DefaultableBoolean.True;
                this.grdHistory.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.CustomerNotes_Fld_Notes].VertScrollBar = true;
            }
            else
            {
                this.grdHistory.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.Notes_Fld_NoteId].Hidden = true;
                this.grdHistory.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.Notes_Fld_EntityId].Hidden = true;
                this.grdHistory.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.Notes_Fld_EntityType].Hidden = true;
                this.grdHistory.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.Notes_Fld_Note].Header.Caption = "Notes";
                this.grdHistory.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.Notes_Fld_CreatedDate].Hidden = true;
                this.grdHistory.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.Notes_Fld_CreatedBy].Hidden = true;
                this.grdHistory.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.Notes_Fld_UpdatedDate].Header.Caption = "Last Updated On";
                this.grdHistory.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.Notes_Fld_UpdatedDate].Hidden = true;
                this.grdHistory.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.Notes_Fld_UpdatedBy].Hidden = true;
                this.grdHistory.DisplayLayout.Bands[0].Columns["Don't Show"].Swap(this.grdHistory.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.Notes_Fld_POPUPMSG]);
                this.grdHistory.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.Notes_Fld_POPUPMSG].Hidden = true;
                this.grdHistory.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.Notes_Fld_Note].Width = 400;
                this.grdHistory.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.Notes_Fld_Note].CellMultiLine = DefaultableBoolean.True;
                this.grdHistory.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.Notes_Fld_Note].VertScrollBar = true;
                grdHistory.DisplayLayout.AutoFitStyle = AutoFitStyle.ResizeAllColumns;
            }

                clsUIHelper.SetAppearance(this.grdHistory);
                this.grdHistory.DisplayLayout.Override.RowSelectors = DefaultableBoolean.False;
                grdHistory.DisplayLayout.AutoFitStyle = AutoFitStyle.ResizeAllColumns;
                this.grdHistory.DisplayLayout.Override.CellMultiLine = DefaultableBoolean.True;
                this.grdHistory.DisplayLayout.Override.RowSizing = RowSizing.AutoFree;
                this.grdHistory.DisplayLayout.Override.RowSizingAutoMaxLines = 5;
                this.grdHistory.DisplayLayout.Bands[0].Columns["Don't Show"].Header.VisiblePosition = 7;
                if (UserPriviliges.IsUserHasPriviliges(UserPriviliges.Modules.Notes.ID, UserPriviliges.Screens.ManageNotes.ID))
                {
                    this.grdHistory.DisplayLayout.Bands[0].Columns["Don't Show"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.CheckBox;
                    this.grdHistory.DisplayLayout.Bands[0].Columns["Don't Show"].CellClickAction = CellClickAction.Edit;
                    this.grdHistory.DisplayLayout.Bands[0].Columns["Don't Show"].CellActivation = Infragistics.Win.UltraWinGrid.Activation.AllowEdit;
                    this.grdHistory.DisplayLayout.Bands[0].Columns["Don't Show"].AutoEdit = true;
                }
                if (strEntityType == clsEntityType.RXNote || strEntityType == clsEntityType.PatNote)
                {
                    this.grdHistory.DisplayLayout.Bands[0].Columns["Don't Show"].Hidden = true;
                }
               //this.grdHistory.DisplayLayout.Appearance.FontData.Bold = DefaultableBoolean.True;
                this.grdHistory.DisplayLayout.Appearance.FontData.SizeInPoints = 10;
		}

		private void btnClose_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}

		private void frmCustomerNotesView_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			try
			{
				if(e.KeyData==System.Windows.Forms.Keys.Enter)
				{
					this.SelectNextControl(this.ActiveControl,true,true,true,true);
				}
				
			}
			catch(Exception exp)
			{
				clsUIHelper.ShowErrorMsg(exp.Message);
			}
		}

		private void frmCustomerNotesView_Activated(object sender, System.EventArgs e)
		{
			clsUIHelper.CurrentForm=this;
		}

        private bool Save()
        {
            bool RetVal = false;
            int RowIndex =0;
            grdHistory.Refresh();
            grdHistory.Update();
            try
            {
                if (isCoustomerNote == true)
                {
                    foreach (UltraGridRow oRow in grdHistory.Rows)
                    {
                        if(Configuration.convertNullToBoolean(oRow.Cells["Don't Show"].Value)== true)
                            oData.Tables[0].Rows[RowIndex][clsPOSDBConstants.CustomerNotes_Fld_IsActive]= false;
                        RowIndex++;
                    }
                    oCNotes.Persist(oData);
                }
                else
                {
                    foreach (UltraGridRow oRow in grdHistory.Rows)
                    {
                        if (Configuration.convertNullToBoolean(oRow.Cells["Don't Show"].Value) == true)
                            oNotesData.Tables[0].Rows[RowIndex][clsPOSDBConstants.Notes_Fld_POPUPMSG] = false;
                        RowIndex++;
                    }
                    oNotes.Persist(oNotesData);
                }
                RetVal = true;
            }

            catch (Exception ex)
            {
                clsUIHelper.ShowErrorMsg(ex.Message);
            }
            return RetVal;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (Save())
                this.Close();
        }

        private void grdHistory_Leave(object sender, EventArgs e)
        {
            grdHistory.UpdateData();
        }

        private void grdHistory_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter || e.KeyData == Keys.Tab)
            {
                grdHistory.SelectNextControl(this.ActiveControl, true, true, true, true);
            }
        }
	}
}
