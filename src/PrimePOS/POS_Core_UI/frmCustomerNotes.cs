using System;
using System.Drawing;
using System.Data;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using POS_Core.CommonData.Rows;
using POS_Core.CommonData;
using POS_Core.BusinessRules;
using POS_Core.ErrorLogging;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
//using POS_Core.DataAccess;
using POS_Core.DataAccess;
using POS_Core.Resources;

namespace POS_Core_UI
{
    /// <summary>
    /// Summary description for frmCustomerNotes.
    /// </summary>
    public class frmCustomerNotes : System.Windows.Forms.Form
    {
        private CustomerNotesData oCustomerNotesData = new CustomerNotesData();
        private CustomerNotesRow oCustomerNotesRow = null;
        private CustomerNotes oCustomerNotes = new CustomerNotes();

        private System.Int32 CurrentID = 0;
        private CustomerRow oCustomerRow = null;
        private int LastRow;

        //Added By Shitaljit(QuicSolv) 0n 5 oct 2011
        private Notes oNotes = null;
        private NotesData oNotesData = null;
        private NotesRow oNotesRow = null;
        private string _Table = "";
        private string strEntityID = "";
        private clsEntityType oEntityType = null;
        private string strEntityType = "";
        bool isManageNotes = false;
        Customer oCust = new Customer();
        CustomerData oCustData = null;
        bool isValid = false;
        //END OF Added By Shitaljit(QuicSolv) 0n 5 oct 2011

        private Infragistics.Win.Misc.UltraLabel ultraLabel14;
        private Infragistics.Win.UltraWinDataSource.UltraDataSource ultraDataSource2;
        private Infragistics.Win.Misc.UltraLabel lblTransactionType;
        private Infragistics.Win.Misc.UltraButton btnSave;
        private Infragistics.Win.UltraWinGrid.UltraGrid grdHistory;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.GroupBox groupBox4;
        private Infragistics.Win.Misc.UltraButton btnClose;
        private Infragistics.Win.UltraWinDataSource.UltraDataSource ultraDataSource1;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor txtComments;
        private CheckBox chkIsActive;
        private Infragistics.Win.Misc.UltraLabel ultraLabel1;
        private Infragistics.Win.Misc.UltraButton btnClear;
        private ImageList imageList1;
        private GroupBox grpBoxEntityDetails;
        private Infragistics.Win.Misc.UltraLabel lblEntityDetails;
        private GroupBox grpBoxSearchFields;
        public Infragistics.Win.UltraWinEditors.UltraTextEditor txtName;
        public Infragistics.Win.UltraWinEditors.UltraTextEditor txtCode;
        private Infragistics.Win.Misc.UltraLabel ultraLabel25;
        private Infragistics.Win.Misc.UltraLabel ultraLabel2;
        private Infragistics.Win.Misc.UltraButton btnSearch;
        private Infragistics.Win.Misc.UltraLabel lblviewNotes;
        public Infragistics.Win.UltraWinEditors.UltraComboEditor combNote;
        private Infragistics.Win.Misc.UltraLabel lblSelectedEntityInfo;
        public Infragistics.Win.Misc.UltraButton btnDelete;
        public Infragistics.Win.Misc.UltraButton btnEdit;
        private GroupBox groupBox2;
        public Infragistics.Win.Misc.UltraButton btnNewNote;
        private IContainer components;

        public frmCustomerNotes(CustomerRow selectedCustomer)
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();
            oCustomerRow = selectedCustomer;
            _Table = clsPOSDBConstants.Customer_tbl;//Added By Shitaljit(QuicSolv) 0n 5 oct 2011
            strEntityID = Configuration.convertNullToString(selectedCustomer.CustomerId);   //Sprint-21 - 2234 07-Sep-2015 JY Added
            //
            // TODO: Add any constructor code after InitializeComponent call
            //
        }

        //Added By Shitaljit(QuicSolv) 0n 5 oct 2011
        public frmCustomerNotes(string EntityID, string tableName, string EntityType)
        {
            InitializeComponent();
            lblEntityDetails.Text = "";
            strEntityID = EntityID;
            strEntityType = EntityType;
            _Table = tableName;
        }
        public frmCustomerNotes()
        {
            InitializeComponent();
            isManageNotes = true;
        }
        //End of Added By Shitaljit(QuicSolv) 0n 5 oct 2011

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
            Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance11 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand1 = new Infragistics.Win.UltraWinGrid.UltraGridBand("Band 0", -1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn1 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("ID");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn2 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("CustomerID");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn3 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Notes");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn4 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("UserID");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn5 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("LastUpdatedOn");
            Infragistics.Win.Appearance appearance27 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance33 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance40 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance8 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance9 = new Infragistics.Win.Appearance();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmCustomerNotes));
            Infragistics.Win.Appearance appearance10 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance36 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance12 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance13 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance41 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance42 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance16 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance17 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance18 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance19 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance20 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance21 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance43 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinScrollBar.ScrollBarLook scrollBarLook1 = new Infragistics.Win.UltraWinScrollBar.ScrollBarLook();
            Infragistics.Win.Appearance appearance44 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance24 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance25 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance26 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinDataSource.UltraDataColumn ultraDataColumn1 = new Infragistics.Win.UltraWinDataSource.UltraDataColumn("ID");
            Infragistics.Win.UltraWinDataSource.UltraDataColumn ultraDataColumn2 = new Infragistics.Win.UltraWinDataSource.UltraDataColumn("CustomerID");
            Infragistics.Win.UltraWinDataSource.UltraDataColumn ultraDataColumn3 = new Infragistics.Win.UltraWinDataSource.UltraDataColumn("Notes");
            Infragistics.Win.UltraWinDataSource.UltraDataColumn ultraDataColumn4 = new Infragistics.Win.UltraWinDataSource.UltraDataColumn("UserID");
            Infragistics.Win.UltraWinDataSource.UltraDataColumn ultraDataColumn5 = new Infragistics.Win.UltraWinDataSource.UltraDataColumn("LastUpdatedOn");
            Infragistics.Win.UltraWinDataSource.UltraDataColumn ultraDataColumn6 = new Infragistics.Win.UltraWinDataSource.UltraDataColumn("Amount");
            Infragistics.Win.UltraWinDataSource.UltraDataColumn ultraDataColumn7 = new Infragistics.Win.UltraWinDataSource.UltraDataColumn("Reference");
            Infragistics.Win.Appearance appearance7 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance50 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance51 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance52 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance32 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance29 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance30 = new Infragistics.Win.Appearance();
            Infragistics.Win.ValueListItem valueListItem1 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem2 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem3 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem4 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem5 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem6 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem7 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance34 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance28 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance45 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance46 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance14 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance15 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance47 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance48 = new Infragistics.Win.Appearance();
            this.ultraLabel14 = new Infragistics.Win.Misc.UltraLabel();
            this.grdHistory = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.ultraDataSource1 = new Infragistics.Win.UltraWinDataSource.UltraDataSource(this.components);
            this.ultraDataSource2 = new Infragistics.Win.UltraWinDataSource.UltraDataSource(this.components);
            this.btnSave = new Infragistics.Win.Misc.UltraButton();
            this.lblTransactionType = new Infragistics.Win.Misc.UltraLabel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.chkIsActive = new System.Windows.Forms.CheckBox();
            this.txtComments = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.ultraLabel1 = new Infragistics.Win.Misc.UltraLabel();
            this.btnClear = new Infragistics.Win.Misc.UltraButton();
            this.btnSearch = new Infragistics.Win.Misc.UltraButton();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.btnClose = new Infragistics.Win.Misc.UltraButton();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.grpBoxEntityDetails = new System.Windows.Forms.GroupBox();
            this.grpBoxSearchFields = new System.Windows.Forms.GroupBox();
            this.lblviewNotes = new Infragistics.Win.Misc.UltraLabel();
            this.combNote = new Infragistics.Win.UltraWinEditors.UltraComboEditor();
            this.txtName = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.txtCode = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.ultraLabel25 = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabel2 = new Infragistics.Win.Misc.UltraLabel();
            this.lblEntityDetails = new Infragistics.Win.Misc.UltraLabel();
            this.lblSelectedEntityInfo = new Infragistics.Win.Misc.UltraLabel();
            this.btnDelete = new Infragistics.Win.Misc.UltraButton();
            this.btnEdit = new Infragistics.Win.Misc.UltraButton();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnNewNote = new Infragistics.Win.Misc.UltraButton();
            ((System.ComponentModel.ISupportInitialize)(this.grdHistory)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraDataSource1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraDataSource2)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtComments)).BeginInit();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.grpBoxEntityDetails.SuspendLayout();
            this.grpBoxSearchFields.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.combNote)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCode)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // ultraLabel14
            // 
            appearance6.ForeColor = System.Drawing.Color.White;
            this.ultraLabel14.Appearance = appearance6;
            this.ultraLabel14.Location = new System.Drawing.Point(14, 59);
            this.ultraLabel14.Name = "ultraLabel14";
            this.ultraLabel14.Size = new System.Drawing.Size(44, 18);
            this.ultraLabel14.TabIndex = 1;
            this.ultraLabel14.Text = "Note:";
            this.ultraLabel14.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // grdHistory
            // 
            this.grdHistory.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.grdHistory.DataSource = this.ultraDataSource1;
            appearance11.BackColor = System.Drawing.Color.White;
            appearance11.BackColor2 = System.Drawing.Color.White;
            appearance11.BackColorDisabled = System.Drawing.Color.White;
            appearance11.BackColorDisabled2 = System.Drawing.Color.White;
            appearance11.BackGradientStyle = Infragistics.Win.GradientStyle.None;
            this.grdHistory.DisplayLayout.Appearance = appearance11;
            ultraGridColumn1.Header.VisiblePosition = 0;
            ultraGridColumn2.Header.VisiblePosition = 1;
            ultraGridColumn3.Header.VisiblePosition = 2;
            ultraGridColumn4.Header.VisiblePosition = 3;
            ultraGridColumn5.Header.VisiblePosition = 4;
            ultraGridBand1.Columns.AddRange(new object[] {
            ultraGridColumn1,
            ultraGridColumn2,
            ultraGridColumn3,
            ultraGridColumn4,
            ultraGridColumn5});
            ultraGridBand1.UseRowLayout = true;
            this.grdHistory.DisplayLayout.BandsSerializer.Add(ultraGridBand1);
            this.grdHistory.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.grdHistory.DisplayLayout.InterBandSpacing = 10;
            this.grdHistory.DisplayLayout.MaxColScrollRegions = 1;
            this.grdHistory.DisplayLayout.MaxRowScrollRegions = 1;
            appearance27.BackColor = System.Drawing.Color.White;
            appearance27.BackColor2 = System.Drawing.Color.White;
            this.grdHistory.DisplayLayout.Override.ActiveCardCaptionAppearance = appearance27;
            appearance5.BackColor = System.Drawing.Color.White;
            appearance5.BackColor2 = System.Drawing.Color.White;
            appearance5.BorderColor = System.Drawing.Color.Gray;
            this.grdHistory.DisplayLayout.Override.ActiveRowAppearance = appearance5;
            appearance33.BackColor = System.Drawing.Color.White;
            appearance33.BackColor2 = System.Drawing.Color.White;
            appearance33.BorderColor = System.Drawing.Color.Gray;
            this.grdHistory.DisplayLayout.Override.AddRowAppearance = appearance33;
            this.grdHistory.DisplayLayout.Override.AllowAddNew = Infragistics.Win.UltraWinGrid.AllowAddNew.No;
            this.grdHistory.DisplayLayout.Override.AllowColMoving = Infragistics.Win.UltraWinGrid.AllowColMoving.WithinBand;
            this.grdHistory.DisplayLayout.Override.AllowColSwapping = Infragistics.Win.UltraWinGrid.AllowColSwapping.NotAllowed;
            this.grdHistory.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.True;
            this.grdHistory.DisplayLayout.Override.AllowRowFiltering = Infragistics.Win.DefaultableBoolean.False;
            this.grdHistory.DisplayLayout.Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.True;
            appearance40.BackColor = System.Drawing.Color.Transparent;
            this.grdHistory.DisplayLayout.Override.CardAreaAppearance = appearance40;
            appearance8.BackColor = System.Drawing.Color.White;
            appearance8.BackColor2 = System.Drawing.Color.White;
            appearance8.BackColorDisabled = System.Drawing.Color.White;
            appearance8.BackColorDisabled2 = System.Drawing.Color.White;
            appearance8.BorderColor = System.Drawing.Color.Black;
            appearance8.BorderColor3DBase = System.Drawing.Color.Black;
            this.grdHistory.DisplayLayout.Override.CellAppearance = appearance8;
            appearance9.BackColor = System.Drawing.Color.White;
            appearance9.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(231)))), ((int)(((byte)(230)))));
            appearance9.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance9.BorderAlpha = Infragistics.Win.Alpha.Transparent;
            appearance9.BorderColor = System.Drawing.Color.Gray;
            appearance9.BorderColor3DBase = System.Drawing.Color.Gray;
            appearance9.Image = ((object)(resources.GetObject("appearance9.Image")));
            appearance9.ImageBackgroundAlpha = Infragistics.Win.Alpha.Transparent;
            appearance9.ImageBackgroundStyle = Infragistics.Win.ImageBackgroundStyle.Centered;
            this.grdHistory.DisplayLayout.Override.CellButtonAppearance = appearance9;
            appearance10.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            appearance10.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.grdHistory.DisplayLayout.Override.EditCellAppearance = appearance10;
            appearance36.BorderColor = System.Drawing.Color.Gray;
            this.grdHistory.DisplayLayout.Override.FilteredInRowAppearance = appearance36;
            appearance12.BorderColor = System.Drawing.Color.Gray;
            this.grdHistory.DisplayLayout.Override.FilteredOutRowAppearance = appearance12;
            appearance13.BackColor = System.Drawing.Color.White;
            appearance13.BackColor2 = System.Drawing.Color.White;
            appearance13.BackColorDisabled = System.Drawing.Color.White;
            appearance13.BackColorDisabled2 = System.Drawing.Color.White;
            this.grdHistory.DisplayLayout.Override.FixedCellAppearance = appearance13;
            appearance41.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            appearance41.BackColor2 = System.Drawing.Color.Beige;
            this.grdHistory.DisplayLayout.Override.FixedHeaderAppearance = appearance41;
            appearance42.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(62)))), ((int)(((byte)(176)))));
            appearance42.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance42.FontData.BoldAsString = "True";
            appearance42.FontData.SizeInPoints = 10F;
            appearance42.ForeColor = System.Drawing.Color.White;
            this.grdHistory.DisplayLayout.Override.HeaderAppearance = appearance42;
            this.grdHistory.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortSingle;
            this.grdHistory.DisplayLayout.Override.MergedCellStyle = Infragistics.Win.UltraWinGrid.MergedCellStyle.Never;
            appearance16.BorderColor = System.Drawing.Color.Gray;
            this.grdHistory.DisplayLayout.Override.RowAlternateAppearance = appearance16;
            appearance17.BackColor = System.Drawing.Color.White;
            appearance17.BackColor2 = System.Drawing.Color.White;
            appearance17.BorderColor = System.Drawing.Color.Gray;
            this.grdHistory.DisplayLayout.Override.RowAppearance = appearance17;
            appearance18.BorderColor = System.Drawing.Color.Gray;
            this.grdHistory.DisplayLayout.Override.RowPreviewAppearance = appearance18;
            appearance19.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(62)))), ((int)(((byte)(176)))));
            appearance19.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance19.BorderColor = System.Drawing.Color.Gray;
            this.grdHistory.DisplayLayout.Override.RowSelectorAppearance = appearance19;
            this.grdHistory.DisplayLayout.Override.RowSelectorWidth = 12;
            this.grdHistory.DisplayLayout.Override.RowSizingArea = Infragistics.Win.UltraWinGrid.RowSizingArea.EntireRow;
            this.grdHistory.DisplayLayout.Override.RowSpacingBefore = 2;
            appearance20.BackColor = System.Drawing.Color.Navy;
            this.grdHistory.DisplayLayout.Override.SelectedCellAppearance = appearance20;
            appearance21.BackColor = System.Drawing.Color.Navy;
            appearance21.BackColorDisabled = System.Drawing.Color.Navy;
            appearance21.BorderColor = System.Drawing.Color.Gray;
            appearance21.ForeColor = System.Drawing.Color.White;
            this.grdHistory.DisplayLayout.Override.SelectedRowAppearance = appearance21;
            this.grdHistory.DisplayLayout.Override.SelectTypeCell = Infragistics.Win.UltraWinGrid.SelectType.None;
            this.grdHistory.DisplayLayout.Override.SelectTypeCol = Infragistics.Win.UltraWinGrid.SelectType.None;
            this.grdHistory.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.None;
            appearance43.BorderColor = System.Drawing.Color.Gray;
            this.grdHistory.DisplayLayout.Override.TemplateAddRowAppearance = appearance43;
            this.grdHistory.DisplayLayout.RowConnectorColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            this.grdHistory.DisplayLayout.RowConnectorStyle = Infragistics.Win.UltraWinGrid.RowConnectorStyle.Solid;
            appearance44.BackColor = System.Drawing.Color.White;
            appearance44.BackColor2 = System.Drawing.Color.Gainsboro;
            appearance44.BorderColor = System.Drawing.Color.WhiteSmoke;
            appearance44.BorderColor3DBase = System.Drawing.Color.WhiteSmoke;
            scrollBarLook1.Appearance = appearance44;
            appearance24.BackColor = System.Drawing.Color.LightGray;
            appearance24.BackColor2 = System.Drawing.Color.WhiteSmoke;
            scrollBarLook1.ButtonAppearance = appearance24;
            appearance25.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            scrollBarLook1.ThumbAppearance = appearance25;
            appearance26.BackColor = System.Drawing.Color.Gainsboro;
            appearance26.BackColor2 = System.Drawing.Color.WhiteSmoke;
            appearance26.BorderColor = System.Drawing.Color.White;
            appearance26.BorderColor3DBase = System.Drawing.Color.Gainsboro;
            scrollBarLook1.TrackAppearance = appearance26;
            this.grdHistory.DisplayLayout.ScrollBarLook = scrollBarLook1;
            this.grdHistory.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.grdHistory.Location = new System.Drawing.Point(12, 16);
            this.grdHistory.Name = "grdHistory";
            this.grdHistory.Size = new System.Drawing.Size(430, 315);
            this.grdHistory.TabIndex = 10;
            this.grdHistory.UpdateMode = Infragistics.Win.UltraWinGrid.UpdateMode.OnCellChangeOrLostFocus;
            this.grdHistory.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.grdHistory.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.grdHistory_InitializeLayout);
            this.grdHistory.AfterSelectChange += new Infragistics.Win.UltraWinGrid.AfterSelectChangeEventHandler(this.grdHistory_AfterSelectChange);
            this.grdHistory.InitializeRow += new Infragistics.Win.UltraWinGrid.InitializeRowEventHandler(this.grdHistory_InitializeRow);
            // 
            // ultraDataSource1
            // 
            this.ultraDataSource1.Band.Columns.AddRange(new object[] {
            ultraDataColumn1,
            ultraDataColumn2,
            ultraDataColumn3,
            ultraDataColumn4,
            ultraDataColumn5});
            // 
            // ultraDataSource2
            // 
            this.ultraDataSource2.Band.Columns.AddRange(new object[] {
            ultraDataColumn6,
            ultraDataColumn7});
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            appearance7.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(62)))), ((int)(((byte)(176)))));
            appearance7.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance7.FontData.BoldAsString = "True";
            appearance7.ForeColor = System.Drawing.Color.White;
            appearance7.Image = ((object)(resources.GetObject("appearance7.Image")));
            this.btnSave.Appearance = appearance7;
            this.btnSave.Location = new System.Drawing.Point(807, 13);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(95, 26);
            this.btnSave.TabIndex = 7;
            this.btnSave.Text = "&Save";
            this.btnSave.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // lblTransactionType
            // 
            this.lblTransactionType.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            appearance1.ForeColor = System.Drawing.Color.White;
            appearance1.ForeColorDisabled = System.Drawing.Color.Navy;
            appearance1.ImageHAlign = Infragistics.Win.HAlign.Center;
            appearance1.ImageVAlign = Infragistics.Win.VAlign.Middle;
            appearance1.TextHAlignAsString = "Center";
            appearance1.TextVAlignAsString = "Middle";
            this.lblTransactionType.Appearance = appearance1;
            this.lblTransactionType.BackColorInternal = System.Drawing.Color.Transparent;
            this.lblTransactionType.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTransactionType.Location = new System.Drawing.Point(16, 8);
            this.lblTransactionType.Name = "lblTransactionType";
            this.lblTransactionType.Size = new System.Drawing.Size(1050, 26);
            this.lblTransactionType.TabIndex = 26;
            this.lblTransactionType.Tag = "Header";
            this.lblTransactionType.Text = "Notes";
            this.lblTransactionType.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.chkIsActive);
            this.groupBox1.Controls.Add(this.txtComments);
            this.groupBox1.Controls.Add(this.ultraLabel14);
            this.groupBox1.Controls.Add(this.ultraLabel1);
            this.groupBox1.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.groupBox1.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(491, 195);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(574, 356);
            this.groupBox1.TabIndex = 27;
            this.groupBox1.TabStop = false;
            // 
            // chkIsActive
            // 
            this.chkIsActive.BackColor = System.Drawing.Color.Transparent;
            this.chkIsActive.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkIsActive.Checked = true;
            this.chkIsActive.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkIsActive.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chkIsActive.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkIsActive.ForeColor = System.Drawing.Color.White;
            this.chkIsActive.Location = new System.Drawing.Point(160, 21);
            this.chkIsActive.Name = "chkIsActive";
            this.chkIsActive.Size = new System.Drawing.Size(12, 23);
            this.chkIsActive.TabIndex = 6;
            this.chkIsActive.UseVisualStyleBackColor = false;
            // 
            // txtComments
            // 
            this.txtComments.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            appearance50.FontData.BoldAsString = "False";
            appearance50.FontData.ItalicAsString = "False";
            appearance50.FontData.StrikeoutAsString = "False";
            appearance50.FontData.UnderlineAsString = "False";
            appearance50.ForeColor = System.Drawing.Color.Black;
            appearance50.ForeColorDisabled = System.Drawing.Color.Black;
            this.txtComments.Appearance = appearance50;
            this.txtComments.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.txtComments.Location = new System.Drawing.Point(64, 58);
            this.txtComments.MaxLength = 800;
            this.txtComments.Multiline = true;
            this.txtComments.Name = "txtComments";
            this.txtComments.Scrollbars = System.Windows.Forms.ScrollBars.Both;
            this.txtComments.Size = new System.Drawing.Size(485, 278);
            this.txtComments.TabIndex = 5;
            this.txtComments.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // ultraLabel1
            // 
            appearance51.ForeColor = System.Drawing.Color.White;
            this.ultraLabel1.Appearance = appearance51;
            this.ultraLabel1.Location = new System.Drawing.Point(14, 23);
            this.ultraLabel1.Name = "ultraLabel1";
            this.ultraLabel1.Size = new System.Drawing.Size(130, 18);
            this.ultraLabel1.TabIndex = 40;
            this.ultraLabel1.Text = "Show as PopUp ?";
            this.ultraLabel1.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // btnClear
            // 
            this.btnClear.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            appearance2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(62)))), ((int)(((byte)(176)))));
            appearance2.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance2.FontData.BoldAsString = "True";
            appearance2.ForeColor = System.Drawing.Color.White;
            appearance2.Image = ((object)(resources.GetObject("appearance2.Image")));
            this.btnClear.Appearance = appearance2;
            this.btnClear.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            this.btnClear.Location = new System.Drawing.Point(477, 14);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(120, 26);
            this.btnClear.TabIndex = 8;
            this.btnClear.Text = "&Clear";
            this.btnClear.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // btnSearch
            // 
            appearance4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance4.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(108)))), ((int)(((byte)(172)))));
            appearance4.FontData.BoldAsString = "True";
            appearance4.ForeColor = System.Drawing.Color.White;
            appearance4.Image = ((object)(resources.GetObject("appearance4.Image")));
            this.btnSearch.Appearance = appearance4;
            this.btnSearch.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            this.btnSearch.Location = new System.Drawing.Point(852, 22);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(142, 28);
            this.btnSearch.TabIndex = 4;
            this.btnSearch.Text = "&Search (F4)";
            this.btnSearch.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox3.Controls.Add(this.grdHistory);
            this.groupBox3.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.groupBox3.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox3.ForeColor = System.Drawing.Color.White;
            this.groupBox3.Location = new System.Drawing.Point(16, 200);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(450, 351);
            this.groupBox3.TabIndex = 9;
            this.groupBox3.TabStop = false;
            // 
            // groupBox4
            // 
            this.groupBox4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox4.Controls.Add(this.btnClose);
            this.groupBox4.Controls.Add(this.btnSave);
            this.groupBox4.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.groupBox4.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox4.Location = new System.Drawing.Point(16, 564);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(1050, 46);
            this.groupBox4.TabIndex = 30;
            this.groupBox4.TabStop = false;
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            appearance52.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(62)))), ((int)(((byte)(176)))));
            appearance52.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance52.FontData.BoldAsString = "True";
            appearance52.ForeColor = System.Drawing.Color.White;
            appearance52.Image = ((object)(resources.GetObject("appearance52.Image")));
            this.btnClose.Appearance = appearance52;
            this.btnClose.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(924, 13);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(100, 26);
            this.btnClose.TabIndex = 11;
            this.btnClose.Text = "&Close";
            this.btnClose.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "");
            this.imageList1.Images.SetKeyName(1, "change password1.jpg");
            this.imageList1.Images.SetKeyName(2, "delete.jpg");
            // 
            // grpBoxEntityDetails
            // 
            this.grpBoxEntityDetails.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.grpBoxEntityDetails.Controls.Add(this.grpBoxSearchFields);
            this.grpBoxEntityDetails.Controls.Add(this.lblEntityDetails);
            this.grpBoxEntityDetails.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.grpBoxEntityDetails.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpBoxEntityDetails.Location = new System.Drawing.Point(15, 42);
            this.grpBoxEntityDetails.Name = "grpBoxEntityDetails";
            this.grpBoxEntityDetails.Size = new System.Drawing.Size(1050, 61);
            this.grpBoxEntityDetails.TabIndex = 42;
            this.grpBoxEntityDetails.TabStop = false;
            // 
            // grpBoxSearchFields
            // 
            this.grpBoxSearchFields.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.grpBoxSearchFields.Controls.Add(this.btnSearch);
            this.grpBoxSearchFields.Controls.Add(this.lblviewNotes);
            this.grpBoxSearchFields.Controls.Add(this.combNote);
            this.grpBoxSearchFields.Controls.Add(this.txtName);
            this.grpBoxSearchFields.Controls.Add(this.txtCode);
            this.grpBoxSearchFields.Controls.Add(this.ultraLabel25);
            this.grpBoxSearchFields.Controls.Add(this.ultraLabel2);
            this.grpBoxSearchFields.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.grpBoxSearchFields.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpBoxSearchFields.Location = new System.Drawing.Point(0, -1);
            this.grpBoxSearchFields.Name = "grpBoxSearchFields";
            this.grpBoxSearchFields.Size = new System.Drawing.Size(1050, 61);
            this.grpBoxSearchFields.TabIndex = 1;
            this.grpBoxSearchFields.TabStop = false;
            this.grpBoxSearchFields.Text = "Search Criteria";
            // 
            // lblviewNotes
            // 
            appearance32.ForeColor = System.Drawing.Color.White;
            this.lblviewNotes.Appearance = appearance32;
            this.lblviewNotes.Location = new System.Drawing.Point(13, 30);
            this.lblviewNotes.Name = "lblviewNotes";
            this.lblviewNotes.Size = new System.Drawing.Size(125, 18);
            this.lblviewNotes.TabIndex = 49;
            this.lblviewNotes.Text = "View Notes For";
            this.lblviewNotes.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.lblviewNotes.Visible = false;
            // 
            // combNote
            // 
            appearance29.BorderAlpha = Infragistics.Win.Alpha.Opaque;
            appearance29.BorderColor3DBase = System.Drawing.Color.Black;
            appearance29.FontData.BoldAsString = "False";
            appearance29.FontData.ItalicAsString = "False";
            appearance29.FontData.StrikeoutAsString = "False";
            appearance29.FontData.UnderlineAsString = "False";
            appearance29.ForeColor = System.Drawing.Color.Black;
            appearance29.ForeColorDisabled = System.Drawing.Color.Black;
            this.combNote.Appearance = appearance29;
            this.combNote.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance30.BackColor = System.Drawing.Color.White;
            appearance30.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(231)))), ((int)(((byte)(230)))));
            appearance30.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance30.BorderAlpha = Infragistics.Win.Alpha.Transparent;
            this.combNote.ButtonAppearance = appearance30;
            valueListItem1.DataValue = "A";
            valueListItem1.DisplayText = "All";
            valueListItem2.DataValue = "C";
            valueListItem2.DisplayText = "Customer";
            valueListItem3.DataValue = "D";
            valueListItem3.DisplayText = "Department";
            valueListItem4.DataValue = "I";
            valueListItem4.DisplayText = "Item";
            valueListItem5.DataValue = "S";
            valueListItem5.DisplayText = "System";
            valueListItem6.DataValue = "U";
            valueListItem6.DisplayText = "User";
            valueListItem7.DataValue = "V";
            valueListItem7.DisplayText = "Vendor";
            this.combNote.Items.AddRange(new Infragistics.Win.ValueListItem[] {
            valueListItem1,
            valueListItem2,
            valueListItem3,
            valueListItem4,
            valueListItem5,
            valueListItem6,
            valueListItem7});
            this.combNote.Location = new System.Drawing.Point(144, 26);
            this.combNote.MaxLength = 20;
            this.combNote.Name = "combNote";
            this.combNote.Size = new System.Drawing.Size(161, 23);
            this.combNote.TabIndex = 1;
            this.combNote.Visible = false;
            this.combNote.ValueChanged += new System.EventHandler(this.combNote_ValueChanged);
            // 
            // txtName
            // 
            this.txtName.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.txtName.Location = new System.Drawing.Point(655, 24);
            this.txtName.MaxLength = 50;
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(170, 23);
            this.txtName.TabIndex = 3;
            this.txtName.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.txtName.ValueChanged += new System.EventHandler(this.txtCode_ValueChanged);
            this.txtName.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBox_KeyDown);
            // 
            // txtCode
            // 
            this.txtCode.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.txtCode.Location = new System.Drawing.Point(420, 25);
            this.txtCode.MaxLength = 50;
            this.txtCode.Name = "txtCode";
            this.txtCode.Size = new System.Drawing.Size(159, 23);
            this.txtCode.TabIndex = 2;
            this.txtCode.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.txtCode.ValueChanged += new System.EventHandler(this.txtCode_ValueChanged);
            this.txtCode.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBox_KeyDown);
            // 
            // ultraLabel25
            // 
            appearance3.ForeColor = System.Drawing.Color.White;
            this.ultraLabel25.Appearance = appearance3;
            this.ultraLabel25.BackColorInternal = System.Drawing.Color.Transparent;
            this.ultraLabel25.Location = new System.Drawing.Point(365, 28);
            this.ultraLabel25.Name = "ultraLabel25";
            this.ultraLabel25.Size = new System.Drawing.Size(42, 19);
            this.ultraLabel25.TabIndex = 44;
            this.ultraLabel25.Text = "&Code";
            // 
            // ultraLabel2
            // 
            appearance34.ForeColor = System.Drawing.Color.White;
            this.ultraLabel2.Appearance = appearance34;
            this.ultraLabel2.BackColorInternal = System.Drawing.Color.Transparent;
            this.ultraLabel2.Location = new System.Drawing.Point(588, 26);
            this.ultraLabel2.Name = "ultraLabel2";
            this.ultraLabel2.Size = new System.Drawing.Size(56, 19);
            this.ultraLabel2.TabIndex = 47;
            this.ultraLabel2.Text = "&Name";
            this.ultraLabel2.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // lblEntityDetails
            // 
            this.lblEntityDetails.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            appearance28.ForeColor = System.Drawing.Color.White;
            appearance28.ForeColorDisabled = System.Drawing.Color.Navy;
            appearance28.ImageHAlign = Infragistics.Win.HAlign.Center;
            appearance28.ImageVAlign = Infragistics.Win.VAlign.Middle;
            appearance28.TextHAlignAsString = "Left";
            appearance28.TextVAlignAsString = "Middle";
            this.lblEntityDetails.Appearance = appearance28;
            this.lblEntityDetails.BackColorInternal = System.Drawing.Color.Transparent;
            this.lblEntityDetails.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblEntityDetails.Location = new System.Drawing.Point(5, 16);
            this.lblEntityDetails.Name = "lblEntityDetails";
            this.lblEntityDetails.Size = new System.Drawing.Size(1038, 32);
            this.lblEntityDetails.TabIndex = 27;
            this.lblEntityDetails.Tag = "Header";
            this.lblEntityDetails.Text = "Entity Information";
            this.lblEntityDetails.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // lblSelectedEntityInfo
            // 
            this.lblSelectedEntityInfo.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSelectedEntityInfo.Location = new System.Drawing.Point(15, 108);
            this.lblSelectedEntityInfo.Name = "lblSelectedEntityInfo";
            this.lblSelectedEntityInfo.Size = new System.Drawing.Size(1049, 35);
            this.lblSelectedEntityInfo.TabIndex = 50;
            this.lblSelectedEntityInfo.Text = "Selected Entity Info.";
            this.lblSelectedEntityInfo.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.lblSelectedEntityInfo.Visible = false;
            // 
            // btnDelete
            // 
            this.btnDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            appearance45.BackColor = System.Drawing.Color.White;
            appearance45.BackColor2 = System.Drawing.SystemColors.Control;
            appearance45.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance45.FontData.BoldAsString = "True";
            appearance45.ForeColor = System.Drawing.Color.Black;
            this.btnDelete.Appearance = appearance45;
            this.btnDelete.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Flat;
            appearance46.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(188)))), ((int)(((byte)(193)))), ((int)(((byte)(207)))));
            appearance46.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(188)))), ((int)(((byte)(193)))), ((int)(((byte)(207)))));
            this.btnDelete.HotTrackAppearance = appearance46;
            this.btnDelete.Location = new System.Drawing.Point(907, 15);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(120, 26);
            this.btnDelete.TabIndex = 92;
            this.btnDelete.Text = "&Delete(F11)";
            this.btnDelete.UseHotTracking = Infragistics.Win.DefaultableBoolean.True;
            this.btnDelete.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnEdit
            // 
            this.btnEdit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            appearance14.BackColor = System.Drawing.Color.White;
            appearance14.BackColor2 = System.Drawing.SystemColors.Control;
            appearance14.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance14.FontData.BoldAsString = "True";
            appearance14.ForeColor = System.Drawing.Color.Black;
            this.btnEdit.Appearance = appearance14;
            this.btnEdit.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Flat;
            appearance15.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(188)))), ((int)(((byte)(193)))), ((int)(((byte)(207)))));
            appearance15.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(188)))), ((int)(((byte)(193)))), ((int)(((byte)(207)))));
            this.btnEdit.HotTrackAppearance = appearance15;
            this.btnEdit.Location = new System.Drawing.Point(777, 14);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(123, 26);
            this.btnEdit.TabIndex = 91;
            this.btnEdit.Text = "&Edit Note (F3)";
            this.btnEdit.UseHotTracking = Infragistics.Win.DefaultableBoolean.True;
            this.btnEdit.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.btnNewNote);
            this.groupBox2.Controls.Add(this.btnClear);
            this.groupBox2.Controls.Add(this.btnEdit);
            this.groupBox2.Controls.Add(this.btnDelete);
            this.groupBox2.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.groupBox2.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.Location = new System.Drawing.Point(14, 145);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(1050, 46);
            this.groupBox2.TabIndex = 31;
            this.groupBox2.TabStop = false;
            // 
            // btnNewNote
            // 
            this.btnNewNote.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            appearance47.BackColor = System.Drawing.Color.White;
            appearance47.BackColor2 = System.Drawing.SystemColors.Control;
            appearance47.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance47.FontData.BoldAsString = "True";
            appearance47.ForeColor = System.Drawing.Color.Black;
            this.btnNewNote.Appearance = appearance47;
            this.btnNewNote.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Flat;
            appearance48.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(188)))), ((int)(((byte)(193)))), ((int)(((byte)(207)))));
            appearance48.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(188)))), ((int)(((byte)(193)))), ((int)(((byte)(207)))));
            this.btnNewNote.HotTrackAppearance = appearance48;
            this.btnNewNote.Location = new System.Drawing.Point(612, 14);
            this.btnNewNote.Name = "btnNewNote";
            this.btnNewNote.Size = new System.Drawing.Size(159, 26);
            this.btnNewNote.TabIndex = 93;
            this.btnNewNote.Text = "&Add New Note(F1)";
            this.btnNewNote.UseHotTracking = Infragistics.Win.DefaultableBoolean.True;
            this.btnNewNote.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnNewNote.Click += new System.EventHandler(this.btnNewNote_Click);
            // 
            // frmCustomerNotes
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(108)))), ((int)(((byte)(172)))));
            this.ClientSize = new System.Drawing.Size(1084, 612);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.lblSelectedEntityInfo);
            this.Controls.Add(this.grpBoxEntityDetails);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.lblTransactionType);
            this.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.Name = "frmCustomerNotes";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Notes";
            this.Load += new System.EventHandler(this.frmCustomerNotes_Load);
            this.Activated += new System.EventHandler(this.frmCustomerNotes_Activated);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmCustomerNotes_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.grdHistory)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraDataSource1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraDataSource2)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtComments)).EndInit();
            this.groupBox3.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.grpBoxEntityDetails.ResumeLayout(false);
            this.grpBoxSearchFields.ResumeLayout(false);
            this.grpBoxSearchFields.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.combNote)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCode)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);

        }
        #endregion

        private void frmCustomerNotes_Load(object sender, System.EventArgs e)
        {
            try
            {
                Clear();
                Display();
                SetNew();
                clsUIHelper.SetReadonlyRow(this.grdHistory);
                clsUIHelper.SetAppearance(this.grdHistory);
                this.txtComments.AfterEnterEditMode += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
                this.txtComments.AfterExitEditMode += new System.EventHandler(clsUIHelper.AfterExitEditMode);

                //Added By Shitaljit(QuicSolv) 0n 5 oct 2011 
                this.txtName.AfterEnterEditMode += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
                this.txtName.AfterExitEditMode += new System.EventHandler(clsUIHelper.AfterExitEditMode);

                this.txtCode.AfterEnterEditMode += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
                this.txtCode.AfterExitEditMode += new System.EventHandler(clsUIHelper.AfterExitEditMode);

                this.combNote.AfterEnterEditMode += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
                this.combNote.AfterExitEditMode += new System.EventHandler(clsUIHelper.AfterExitEditMode);

                clsUIHelper.setColorSchecme(this);
                this.txtComments.Enabled = false;
                //End of Added By Shitaljit(QuicSolv) 0n 5 oct 2011 
                btnClear.Visible = isManageNotes;   //Sprint-21 - 2234 07-Sep-2015 JY Added
                this.chkIsActive.Enabled = false;    //Sprint-23 - PRIMEPOS-413 22-Jun-2016 JY Added
                grdHistory.Selected.Rows.Clear();   //Sprint-23 - PRIMEPOS-413 22-Jun-2016 JY Added
            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }

        private void Display()
        {
            this.grdHistory.DataSource = null;

            //Following if-else is Added By Shitaljit(QuicSolv) 0n 5 oct 2011 
            if (isManageNotes == true)
            {
                SetNew();
                combNote.Visible = true;
                lblviewNotes.Visible = true;
                oNotesData = oNotes.PopulateList("");
                grdHistory.DataSource = oNotesData;

            }
            if (_Table == clsPOSDBConstants.Customer_tbl && isManageNotes == false)
            {
                grpBoxSearchFields.Visible = false;
                CustomerNotesData oGCustomerNotesData;
                oGCustomerNotesData = oCustomerNotes.Populate(oCustomerRow.CustomerId, false);
                this.grdHistory.DataSource = oGCustomerNotesData;
                grpBoxEntityDetails.Text = "Customer Details";
                this.txtCode.Text = oCustomerRow.CustomerId.ToString().Trim();
                lblEntityDetails.Text = "ID: " + oCustomerRow.CustomerId.ToString().Trim() + "               Name: " + oCustomerRow.CustomerFullName.Trim() + "          Address:" + oCustomerRow.Address1.Trim() + "  " + oCustomerRow.Address2.Trim() + "  " + oCustomerRow.City.Trim() + ", " + oCustomerRow.State.Trim() + "  " + oCustomerRow.Zip.Trim();
            }
            else if (isManageNotes == false)
            {
                SetNew();
                grpBoxSearchFields.Visible = false;
                if (strEntityType == clsEntityType.DepartmentNote)
                {
                    Department oDept = new Department();
                    DepartmentData oDeptData = new DepartmentData();
                    DepartmentRow oDeptRow = null;
                    oDeptData = oDept.Populate(Configuration.convertNullToInt(strEntityID));
                    oDeptRow = (DepartmentRow)oDeptData.Department[0];
                    grpBoxEntityDetails.Text = "Department Details";
                    lblEntityDetails.Text = "Department Code:    " + oDeptRow.DeptCode.Trim() + "                    Department Name:    " + oDeptRow.DeptName.Trim();
                }
                if (strEntityType == clsEntityType.VendorNote)
                {
                    POS_Core.BusinessRules.Vendor oVendor = new POS_Core.BusinessRules.Vendor();
                    VendorData oVendorData = new VendorData();
                    VendorRow oVendorRow = null;
                    oVendorData = oVendor.Populate(Configuration.convertNullToInt(strEntityID));
                    oVendorRow = (VendorRow)oVendorData.Vendor[0];
                    grpBoxEntityDetails.Text = "Vendor Details";
                    lblEntityDetails.Text = "Vendor Code:  " + oVendorRow.Vendorcode.Trim() + "          Vendor Name:    " + oVendorRow.Vendorname.Trim() + "                 Address: " + oVendorRow.Address1.Trim() + " " + oVendorRow.Address2.Trim() + " " + oVendorRow.City.Trim() + ", " + oVendorRow.State.Trim() + " " + oVendorRow.Zip.Trim();
                }
                if (strEntityType == clsEntityType.UserNote)
                {
                    User oUser = new User();
                    UserData oUserData = new UserData();
                    UserRow oUserRow = null;
                    oUserData = oUser.GetUserByUserID(strEntityID);
                    oUserRow = (UserRow)oUserData.User[0];
                    grpBoxEntityDetails.Text = "User Details";
                    lblEntityDetails.Text = "Users ID:    " + oUserRow.UserID + "                    Users Name:    " + oUserRow.LastName.Trim() + "," + oUserRow.FirstName.Trim();
                }
                if (strEntityType == clsEntityType.ItemNote)
                {
                    Item oItem = new Item();
                    ItemData oItemData = new ItemData();
                    ItemRow oItemRow = null;
                    oItemData = oItem.Populate(strEntityID);
                    oItemRow = (ItemRow)oItemData.Item[0];
                    grpBoxEntityDetails.Text = "Item Details";
                    lblEntityDetails.Text = "Items Code:    " + oItemRow.ItemID + "                        Items Description:    " + oItemRow.Description;
                }
                if (strEntityType == clsEntityType.SystemNote)
                {
                    grpBoxEntityDetails.Text = "System Details";
                    lblEntityDetails.Text = "Station: " + Configuration.StationName.Trim() + "               Pharmacy: " + Configuration.CInfo.StoreName.Trim() + "                     " + Application.ProductName.Trim() + "  Version:   " + Application.ProductVersion.Trim();
                }
                string whereClause = " WHERE " + clsPOSDBConstants.Notes_Fld_EntityId + "= '" + strEntityID + "'  AND  " + clsPOSDBConstants.Notes_Fld_EntityType + "= '" + strEntityType + "'";
                oNotesData = oNotes.PopulateList(whereClause);
                this.grdHistory.DataSource = oNotesData;

            }
            #region Commented by shitaljit on 2 nov 2011
            //Infragistics.Win.UltraWinGrid.UltraGridColumn oCol = null;

            //if (this.grdHistory.DisplayLayout.Bands[0].Columns.Exists("btnEdit") == false)
            //{
            //    oCol = this.grdHistory.DisplayLayout.Bands[0].Columns.Add("btnEdit");
            //    oCol.Header.Caption = "";
            //    oCol.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.Button;
            //    oCol.CellAppearance.Image = this.imageList1.Images[1];
            //    oCol.CellButtonAppearance.Image = this.imageList1.Images[1];
            //    oCol.Width = 20;
            //    oCol.AutoEdit = false;
            //    oCol.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit;
            //}

            //if (this.grdHistory.DisplayLayout.Bands[0].Columns.Exists("btnDelete") == false)
            //{
            //    oCol = this.grdHistory.DisplayLayout.Bands[0].Columns.Add("btnDelete");
            //    oCol.Header.Caption = "";
            //    oCol.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.Button;
            //    oCol.CellAppearance.Image = this.imageList1.Images[2];
            //    oCol.CellButtonAppearance.Image = this.imageList1.Images[2];
            //    oCol.Width = 20;
            //    oCol.AutoEdit = false;
            //    oCol.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit;
            //}
            #endregion

            this.grdHistory.Refresh();
            ApplyGrigFormat();
        }

        private void ApplyGrigFormat()
        {
            //Following if-else is Added By Shitaljit(QuicSolv) 0n 5 oct 2011 
            if (_Table == clsPOSDBConstants.Customer_tbl)
            {
                this.grdHistory.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.CustomerNotes_Fld_ID].Hidden = true;

                this.grdHistory.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.CustomerNotes_Fld_CustomerID].Header.Caption = "Cust. ID";
                this.grdHistory.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.CustomerNotes_Fld_CustomerID].Hidden = true;
                this.grdHistory.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.CustomerNotes_Fld_CustomerID].Header.SetVisiblePosition(1, false);
                this.grdHistory.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.CustomerNotes_Fld_CustomerID].Width = 100;
                this.grdHistory.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.CustomerNotes_Fld_Notes].Header.Caption = "Notes";
                this.grdHistory.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.CustomerNotes_Fld_UserID].Header.Caption = "UserID";
                
                this.grdHistory.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.CustomerNotes_Fld_LastUpdatedOn].Header.Caption = "Last Updated On";
                this.grdHistory.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.CustomerNotes_Fld_IsActive].Header.Caption = "IsActive";
                this.grdHistory.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.CustomerNotes_Fld_UserID].Hidden = true;
                this.grdHistory.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.CustomerNotes_Fld_IsActive].Hidden = true;
                this.grdHistory.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.CustomerNotes_Fld_LastUpdatedOn].Header.Caption = "Date";
                this.grdHistory.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.CustomerNotes_Fld_LastUpdatedOn].Width = 150;
                if (isManageNotes == true)
                {
                    this.grdHistory.DisplayLayout.Bands[0].Columns["Name"].Width = 150;
                }
                // this.grdHistory.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.CustomerNotes_Fld_IsActive].Header.SetVisiblePosition(3, false);
                this.grdHistory.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.CustomerNotes_Fld_LastUpdatedOn].CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit;
                this.grdHistory.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.CustomerNotes_Fld_Notes].Width = 250;
                //this.grdHistory.DisplayLayout.Bands[0].Columns["btnEdit"].Swap(this.grdHistory.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.CustomerNotes_Fld_UserID]);
                //this.grdHistory.DisplayLayout.Bands[0].Columns["btnDelete"].Swap(this.grdHistory.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.CustomerNotes_Fld_LastUpdatedOn]);
                this.grdHistory.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.CustomerNotes_Fld_Notes].Header.SetVisiblePosition(3, false);
                //this.grdHistory.DisplayLayout.Bands[0].Columns["btnEdit"].Header.SetVisiblePosition(4, false);
                //this.grdHistory.DisplayLayout.Bands[0].Columns["btnDelete"].Header.SetVisiblePosition(5, false);
                this.grdHistory.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.CustomerNotes_Fld_UserID].Header.SetVisiblePosition(6, false);
                this.grdHistory.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.CustomerNotes_Fld_LastUpdatedOn].Header.SetVisiblePosition(7, false);
            }

            else
            {
                this.grdHistory.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.Notes_Fld_EntityId].Hidden = true;
                this.grdHistory.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.Notes_Fld_NoteId].Hidden = true;
                this.grdHistory.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.Notes_Fld_CreatedBy].Hidden = true;
                this.grdHistory.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.Notes_Fld_UpdatedBy].Hidden = true;
                this.grdHistory.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.Notes_Fld_UpdatedDate].Hidden = true;
                this.grdHistory.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.Notes_Fld_POPUPMSG].Hidden = true;
                this.grdHistory.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.Notes_Fld_EntityType].Hidden = true;
                if (strEntityType != "A" && strEntityType != clsEntityType.SystemNote && isManageNotes == true)
                {
                    this.grdHistory.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.Notes_Fld_EntityId].Hidden = true;
                    this.grdHistory.DisplayLayout.Bands[0].Columns["Code"].Width = 120;
                    this.grdHistory.DisplayLayout.Bands[0].Columns["Name"].Width = 200;
                }
                else if (strEntityType == "A")
                {
                    this.grdHistory.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.Notes_Fld_EntityId].Hidden = false;
                    this.grdHistory.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.Notes_Fld_EntityId].Header.SetVisiblePosition(1, false);
                    this.grdHistory.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.Notes_Fld_EntityType].Header.SetVisiblePosition(2, false);
                }
                this.grdHistory.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.Notes_Fld_Note].Header.Caption = "Notes";
                this.grdHistory.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.Notes_Fld_CreatedDate].Header.Caption = "Date";
                this.grdHistory.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.Notes_Fld_CreatedDate].Width = 100;
                this.grdHistory.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.Notes_Fld_CreatedBy].Header.Caption = "UserID";

                this.grdHistory.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.Notes_Fld_UpdatedDate].Hidden = true;
                this.grdHistory.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.Notes_Fld_UpdatedBy].Hidden = true;
                this.grdHistory.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.Notes_Fld_POPUPMSG].Header.Caption = "PopUp";
                this.grdHistory.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.Notes_Fld_POPUPMSG].Width = 60;

                this.grdHistory.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.Notes_Fld_Note].Width = 270;
                this.grdHistory.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.Notes_Fld_EntityId].Header.Caption = "Ent. ID";
                this.grdHistory.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.Notes_Fld_EntityType].Header.Caption = "Type";
                this.grdHistory.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.Notes_Fld_Note].Header.SetVisiblePosition(3, false);
                this.grdHistory.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.Notes_Fld_POPUPMSG].Width = 60;
                this.grdHistory.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.Notes_Fld_CreatedDate].Width = 100;
                this.grdHistory.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.Notes_Fld_EntityId].Width = 120;
                this.grdHistory.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.Notes_Fld_EntityType].Width = 50;
                this.grdHistory.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.Notes_Fld_CreatedBy].Width = 60;
                this.grdHistory.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.Notes_Fld_Note].CellMultiLine = DefaultableBoolean.True;
                this.grdHistory.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.Notes_Fld_Note].VertScrollBar = true;
            }

            clsUIHelper.SetAppearance(this.grdHistory);
            this.grdHistory.DisplayLayout.Override.RowSelectors = DefaultableBoolean.False;
            this.grdHistory.DisplayLayout.AutoFitStyle = AutoFitStyle.ResizeAllColumns;
            this.grdHistory.DisplayLayout.Bands[0].Override.AllowUpdate = DefaultableBoolean.False;
            this.grdHistory.DisplayLayout.Override.CellMultiLine = DefaultableBoolean.True;
            this.grdHistory.DisplayLayout.Override.RowSizing = RowSizing.AutoFree;
            this.grdHistory.DisplayLayout.Override.RowSizingAutoMaxLines = 5;
        }

        private void btnClose_Click(object sender, System.EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, System.EventArgs e)
        {
            //Following if-else is Added By Shitaljit(QuicSolv) 0n 5 oct 2011 
            try
            {
                if (this.txtComments.Text == "")
                {
                    clsUIHelper.ShowErrorMsg("Notes Cannot be blank");
                    this.txtComments.Focus();
                    return;
                }
                if (_Table == clsPOSDBConstants.Customer_tbl)
                {
                    if (oCustomerRow != null)
                    {
                        oCustomerNotesRow.CustomerID = oCustomerRow.CustomerId;
                        oCustomerNotesRow.Notes = this.txtComments.Text;
                        oCustomerNotesRow.LastUpdatedOn = System.DateTime.Now;
                        oCustomerNotesRow.IsActive = chkIsActive.Checked;
                        oCustomerNotes.Persist(oCustomerNotesData);
                        isSave = true;
                        this.txtName.Text = oCustomerRow.CustomerName;
                        this.txtComments.Text = "";
                        SetNew();
                        Search(false);
                    }
                    else
                    {
                        clsUIHelper.ShowErrorMsg("Please select a Customer");
                        this.txtCode.Focus();
                        return;
                    }
                }
                else
                {
                    if (Save())
                    {
                        SetNew();
                        Search(false);
                    }
                }

            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }
        //Added By Shitaljit(QuicSolv) 0n 5 oct 2011
        bool isSave = false;
        private bool Save()
        {
            bool retVal = false;
            try
            {
                if (strEntityType == "A" || strEntityType == "")
                {
                    clsUIHelper.ShowErrorMsg("Select an entity type.");
                    return retVal;
                }
                if (strEntityID == "")
                {
                    clsUIHelper.ShowErrorMsg("Please Select a " + this.combNote.Text);
                    this.txtCode.Focus();
                    return retVal;
                }
                if (strEntityType == clsEntityType.SystemNote)
                {
                    oNotesRow.EntityId = "SYSTEM";
                }
                
                else
                {
                    oNotesRow.EntityId = strEntityID;
                }

                oNotesRow.EntityType = strEntityType;
                oNotesRow.Note = this.txtComments.Text;
                oNotesRow.CreatedDate = System.DateTime.Now;
                oNotesRow.CreatedBy = Configuration.UserName;
                oNotesRow.UpdatedDate = DBNull.Value;
                oNotesRow.UpdatedBy = "";
                oNotesRow.POPUPMSG = chkIsActive.Checked;
                oNotes.Persist(oNotesData);
                retVal = true;
                isSave = true;
                this.chkIsActive.Enabled = false;    //Sprint-23 - PRIMEPOS-413 22-Jun-2016 JY Added
                this.txtComments.Enabled = false;
            }
            catch (Exception ex)
            {
                clsUIHelper.ShowErrorMsg(ex.Message);
            }
            return retVal;
        }
        //End of Added By Shitaljit(QuicSolv) 0n 5 oct 2011
        private void Clear()
        {
            this.txtComments.Text = String.Empty;
            this.chkIsActive.Checked = false;
            if (isSave == false)
            {
                this.combNote.SelectedIndex = 0;
            }
            else
            {
                if (isManageNotes == true)  //Sprint-21 - 2234 08-Sep-2015 JY Added conditon (isManageNotes == true) - to load specific notes w.r.t. selected entity
                {
                    // Added By Shitaljit(QuicSolv) 0n 10 oct 2011
                    this.txtCode.Text = String.Empty;
                    this.txtName.Text = String.Empty;
                    this.strEntityID = "";
                }
            }
        }

        private void SetNew()
        {
            oCustomerNotes = new CustomerNotes();
            oCustomerNotesData = new CustomerNotesData();

            //Added By Shitaljit(QuicSolv) 0n 5 oct 2011
            oNotes = new Notes();
            oNotesData = new NotesData();
            oNotesRow = oNotesData.Notes.AddRow(0, "", "", "", System.DateTime.MinValue, "", System.DateTime.MinValue, "", false);
            oEntityType = new clsEntityType();
            //END of Added By Shitaljit(QuicSolv) 0n 5 oct 2011
            Clear();
            oCustomerNotesRow = oCustomerNotesData.CustomerNotes.AddRow(0, 0, "");

            //Added By Shitaljit(QuicSolv) 0n 30 May 2012
            if (strEntityID == "")
            {
                this.btnNewNote.Enabled = false;
            }
            this.btnEdit.Enabled = false;
            this.btnDelete.Enabled = false;
        }

        private void frmCustomerNotes_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            try
            {
                if (e.KeyData == System.Windows.Forms.Keys.Enter && isManageNotes == false)
                {
                    this.SelectNextControl(this.ActiveControl, true, true, true, true);
                }
                //Added By Shitaljit(QuicSolv) on 2 nov 2011
                else if (e.KeyData == Keys.F1 && this.btnNewNote.Enabled == true)
                {
                    this.btnNewNote_Click(null, null);
                }
                else if (e.KeyData == Keys.F3 && this.btnEdit.Enabled == true)
                {
                    this.btnEdit_Click(null,null);
                }
                else  if (e.KeyData == Keys.F11 && this.btnDelete.Enabled == true)
                {
                    this.btnDelete_Click(null,null);
                }
                else if (e.KeyData == Keys.F4 )
                {
                    Search(true);
                }
                //End of added by shitaljit.
                else if (e.KeyData == System.Windows.Forms.Keys.Escape)
                {
                    this.Close();
                }

            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }

        private void frmCustomerNotes_Activated(object sender, System.EventArgs e)
        {
            clsUIHelper.CurrentForm = this;
            if (isManageNotes == true)
            {
                this.combNote.Focus();
                this.chkIsActive.Enabled = false;    //Sprint-23 - PRIMEPOS-413 22-Jun-2016 JY Added
                this.txtComments.Enabled = false;
            }
            
        }

        private void grdHistory_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
        {
            //this.grdHistory.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.CustomerNotes_Fld_Notes].EditorControl = this.txtComments;
            //this.grdHistory.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.CustomerNotes_Fld_Notes].CellActivation = Infragistics.Win.UltraWinGrid.Activation.ActivateOnly;
            //this.grdHistory.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.CustomerNotes_Fld_Notes].CellMultiLine = DefaultableBoolean.True;
            //this.grdHistory.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.CustomerNotes_Fld_Notes].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.FormattedTextEditor;
            //this.grdHistory.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.CustomerNotes_Fld_Notes].CellDisplayStyle = Infragistics.Win.UltraWinGrid.CellDisplayStyle.FullEditorDisplay;
            //this.grdHistory.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.CustomerNotes_Fld_Notes].VertScrollBar = true;
            //this.grdHistory.DisplayLayout.Override.CellMultiLine = DefaultableBoolean.True;

        }

        private void grdHistory_InitializeRow(object sender, Infragistics.Win.UltraWinGrid.InitializeRowEventArgs e)
        {
            if (_Table == clsPOSDBConstants.Customer_tbl)
            {
                if (e.Row.Cells[clsPOSDBConstants.CustomerNotes_Fld_Notes].Text.Length > 0)
                {
                    e.Row.Height = (e.Row.Cells[clsPOSDBConstants.CustomerNotes_Fld_Notes].Text.Length / 40) * 20;
                    if (e.Row.Height < 20)
                    {
                        e.Row.Height = 20;
                    }
                }
            }
            else
            {
                if (e.Row.Cells[clsPOSDBConstants.Notes_Fld_Note].Text.Length > 0)
                {
                    e.Row.Height = (e.Row.Cells[clsPOSDBConstants.Notes_Fld_Note].Text.Length / 40) * 20;
                    if (e.Row.Height < 20)
                    {
                        e.Row.Height = 20;
                    }
                }
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            this.txtCode.Text = "";
            this.txtName.Text = "";
            oCustomerRow = null;
            if (isManageNotes == true)
            {
                this.chkIsActive.Enabled = false;    //Sprint-23 - PRIMEPOS-413 22-Jun-2016 JY Added
                this.txtComments.Enabled = false;
            }
            if (oNotesData != null)
            {
                if (oNotesData.Notes.Rows.Count > 0)
                    oNotesData.Tables[0].Clear();
            }
            this.grdHistory.DataSource = oNotesData;
            this.combNote.SelectedIndex = 0;
            isSave = false;
            SetNew();
        }

        //Sprint-23 - PRIMEPOS-413 22-Jun-2016 JY Commented as not in use
        //private void PopulateData()
        //{
        //    //Added By Shitaljit(QuicSolv) 0n 5 oct 2011 
        //    this.txtCode.Text = oCustomerNotesRow.CustomerID.ToString();
        //    this.txtName.Text = oCustomerRow.CustomerName;
        //    //this.txtComments.Enabled = true;
        //    //End of added by shitaljit(QuicSolv)
        //    this.txtComments.Text = oCustomerNotesRow.Notes;
        //    this.chkIsActive.Checked = oCustomerNotesRow.IsActive;
        //    this.txtComments.Enabled = true;
        //    this.txtComments.Focus();
        //}

        #region Commented by shitaljit on 2 nov 2011
        //private void grdHistory_ClickCellButton(object sender, CellEventArgs e)
        //{
        //    //Following if-else is Added By Shitaljit(QuicSolv) 0n 5 oct 2011 
        //    if (e.Cell.Column.Key == "btnEdit")
        //    {
        //        this.btnSave.Enabled = true;
        //        this.txtComments.Enabled = true;
        //        if (_Table == clsPOSDBConstants.Customer_tbl)
        //        {
        //            oCustomerNotesData = oCustomerNotes.PopulateByID(Resources.Configuration.convertNullToInt(e.Cell.Row.Cells[clsPOSDBConstants.CustomerNotes_Fld_ID].Text));

        //            if (oCustomerNotesData.CustomerNotes.Rows.Count > 0)
        //            {
        //                oCustomerNotesRow = oCustomerNotesData.CustomerNotes[0];
        //                oCustData = oCust.Populate(oCustomerNotesRow.CustomerID.ToString());
        //                oCustomerRow = oCustData.Customer[0];
        //                PopulateData();
        //                this.lblSelectedEntityInfo.Visible = true;
        //                this.lblSelectedEntityInfo.Text = _Table + " Information         " + "Code: " + oCustomerRow.CustomerId + "                  " + "Name:  " + oCustomerRow.CustomerFullName;
        //            }
        //        }
        //        else
        //        {
        //            int NoteID = Configuration.convertNullToInt(e.Cell.Row.Cells[clsPOSDBConstants.Notes_Fld_NoteId].Text);
        //            strEntityID = Configuration.convertNullToString(e.Cell.Row.Cells[clsPOSDBConstants.Notes_Fld_EntityId].Text);
        //            strEntityType = Configuration.convertNullToString(e.Cell.Row.Cells[clsPOSDBConstants.Notes_Fld_EntityType].Text);
        //            oNotesData = oNotes.Populate(NoteID);
        //            if (oNotesData.Notes.Rows.Count > 0)
        //            {
        //                oNotesRow = oNotesData.Notes.GetRow(NoteID);
        //                this.txtComments.Text = oNotesRow.Note;
        //                this.chkIsActive.Checked = oNotesRow.POPUPMSG;
        //                this.txtComments.Focus();
        //            }
        //        }
        //    }
        //    else if (e.Cell.Column.Key == "btnDelete")
        //    {
        //        try
        //        {
        //            if (Resources.Message.Display("This action will delete selected record. Are your sure?", "Payout", MessageBoxButtons.YesNo) == DialogResult.Yes)
        //            {
        //                //Following if-else is Added By Shitaljit(QuicSolv) 0n 5 oct 2011 
        //                if (_Table == clsPOSDBConstants.Customer_tbl)
        //                {
        //                    oCustomerNotes.DeleteRow(Resources.Configuration.convertNullToInt(e.Cell.Row.Cells[clsPOSDBConstants.CustomerNotes_Fld_ID].Text));
        //                    Search();
        //                }
        //                else
        //                {
        //                    NotesSvr oNotesSvr = new NotesSvr();
        //                    int NoteID = Configuration.convertNullToInt(e.Cell.Row.Cells[clsPOSDBConstants.Notes_Fld_NoteId].Text);
        //                    oNotesSvr.DeleteRow(NoteID);
        //                    Search();
        //                }

        //            }
        //        }
        //        catch (Exception exp)
        //        {
        //            clsUIHelper.ShowErrorMsg(exp.Message);
        //        }
        //    }
        //}
        #endregion

        
        private void btnSearch_Click(object sender, EventArgs e)
        {
            this.txtComments.Text = "";
            this.chkIsActive.Checked = false;
            Search(true);
        }

        private void SearchEntity()
        {
            try
            {
                //frmSearch oSearch = new frmSearch(_Table, this.txtCode.Text, this.txtName.Text);
                frmSearchMain oSearch = new frmSearchMain(_Table, this.txtCode.Text, this.txtName.Text, true);  //20-Dec-2017 JY Added new reference
                if (_Table != clsPOSDBConstants.Item_tbl)
                    oSearch.SearchInConstructor = true;
                oSearch.ShowDialog(this);
                if (!oSearch.IsCanceled)
                {
                    oNotesRow.EntityId = strEntityID = oSearch.SelectedRowID();
                    this.txtCode.Text = oSearch.SelectedRowID();
                    if (_Table == clsPOSDBConstants.Customer_tbl)
                    {
                        oCustomerNotesRow.CustomerID = Configuration.convertNullToInt(oSearch.SelectedRowID());
                        oCustData = oCust.GetCustomerByID(oCustomerNotesRow.CustomerID);
                        oCustomerRow = oCustData.Customer[0];
                        this.txtName.Text = oCustomerRow.CustomerName;
                        lblSelectedEntityInfo.Text = "ID: " + oCustomerRow.CustomerId.ToString().Trim() + "               Name: " + oCustomerRow.CustomerFullName.Trim() + "          Address:" + oCustomerRow.Address1.Trim() + "  " + oCustomerRow.Address2.Trim() + "  " + oCustomerRow.City.Trim() + ", " + oCustomerRow.State.Trim() + "  " + oCustomerRow.Zip.Trim();
                    }
                    else
                    {
                        setEnttiyInfo(true);
                    }
                    this.lblSelectedEntityInfo.Visible = true;
                }

            }
            catch (Exception Ex)
            {
                clsUIHelper.ShowErrorMsg(Ex.Message);
            }
        }

        private void SearchNotes()
        {
            string whereClause = "";
            Search oSearch = new Search();
            Notes oNotes = new Notes();
            try
            {
                if (_Table == clsPOSDBConstants.Customer_tbl)
                {
                    DataSet ds = new DataSet();
                    string sSQL = "Select "
                                    + clsPOSDBConstants.CustomerNotes_Fld_ID
                                    + " , " + clsPOSDBConstants.Customer_Fld_CustomerName + "+', '+ IsNull(" + clsPOSDBConstants.Customer_Fld_FirstName + ",'') as Name "
                                    + " , " + clsPOSDBConstants.CustomerNotes_Fld_Notes
                                    + " , " + clsPOSDBConstants.CustomerNotes_tbl + "." + clsPOSDBConstants.CustomerNotes_Fld_CustomerID
                                    + " , " + clsPOSDBConstants.CustomerNotes_Fld_LastUpdatedOn
                                    + " , " + clsPOSDBConstants.CustomerNotes_tbl + "." + clsPOSDBConstants.CustomerNotes_Fld_UserID
                                    + " , " + clsPOSDBConstants.CustomerNotes_tbl + "." + clsPOSDBConstants.CustomerNotes_Fld_IsActive
                                + " FROM "
                                    + clsPOSDBConstants.CustomerNotes_tbl + " , " + clsPOSDBConstants.Customer_tbl + "  WHERE  Customer.CustomerID = CustomerNotes.CustomerID";

                    if (txtCode.Text != "")
                        whereClause += "  AND  " + clsPOSDBConstants.CustomerNotes_tbl + "." + clsPOSDBConstants.CustomerNotes_Fld_CustomerID + " LIKE( '" + txtCode.Text + "%')";

                    if (whereClause != "")
                        sSQL += whereClause;
                    ds = oSearch.SearchData(sSQL);
                    grdHistory.DataSource = ds.Tables[0];

                }
                else
                {
                    if (isManageNotes == true && this.combNote.Text == "All")    //Sprint-21 - 2234 08-Sep-2015 JY Added conditon (isManageNotes == true) - to load specific notes w.r.t. selected entity
                    {
                        DataSet ds = new DataSet();
                        ds = oNotes.PopulateList(_Table, whereClause);
                        this.grdHistory.DataSource = ds.Tables[0];
                    
                    }
                    else if (strEntityType == "A")
                    {
                        if (txtCode.Text != "" && txtName.Text == "")
                            whereClause += "  WHERE  " + clsPOSDBConstants.Notes_Fld_EntityId + " LIKE( '" + txtCode.Text + "%')";
                        else if (txtName.Text != "" && txtCode.Text == "")
                            whereClause += "  WHERE  " + clsPOSDBConstants.Notes_Fld_Note + " LIKE( '" + txtName.Text + "%')";
                        if (txtName.Text != "" && txtCode.Text != "")
                            whereClause += "  WHERE  " + clsPOSDBConstants.Notes_Fld_EntityId + " LIKE( '" + txtCode.Text + "%')" + " AND  " + clsPOSDBConstants.Notes_Fld_Note + " LIKE( '" + txtName.Text + "%')";
                        oNotesData = oNotes.PopulateList(whereClause);
                        this.grdHistory.DataSource = oNotesData;
                    }
                    else if (strEntityType == clsEntityType.SystemNote)
                    {
                        whereClause += " WHERE " + clsPOSDBConstants.Notes_Fld_EntityType + "  = '" + strEntityType + "'";
                        if (txtCode.Text != "" && txtName.Text == "")
                            whereClause += "  AND  " + clsPOSDBConstants.Notes_Fld_EntityId + " LIKE( '" + txtCode.Text + "%')";
                        else if (txtName.Text != "" && txtCode.Text == "")
                            whereClause += "  AND  " + clsPOSDBConstants.Notes_Fld_Note + " LIKE( '" + txtName.Text + "%')";
                        if (txtName.Text != "" && txtCode.Text != "")
                            whereClause += "  AND  " + clsPOSDBConstants.Notes_Fld_EntityId + " LIKE( '" + txtCode.Text + "%')" + " AND  " + clsPOSDBConstants.Notes_Fld_Note + " LIKE( '" + txtName.Text + "%')";
                        oNotesData = oNotes.PopulateList(whereClause);
                        this.grdHistory.DataSource = oNotesData;
                    }
                    else
                    {
                        whereClause += " AND " + clsPOSDBConstants.Notes_Fld_EntityType + "  = '" + strEntityType + "'";
                        if (strEntityID != "")
                            whereClause += "  AND  " + clsPOSDBConstants.Notes_Fld_EntityId + " LIKE( '" + strEntityID + "')";
                        if (this.txtName.Text != "")
                        {
                            whereClause += " AND " + clsPOSDBConstants.Notes_Fld_Note + " LIKE( '" + this.txtName.Text.Trim() + "%')";
                        }
                        DataSet ds = new DataSet();
                        ds = oNotes.PopulateList(_Table, whereClause);
                        this.grdHistory.DataSource = ds.Tables[0];

                    }
                }
                if (_Table != "" || strEntityType == "A")
                {
                    ApplyGrigFormat();
                }
            }
            catch (Exception Ex)
            {
                clsUIHelper.ShowErrorMsg(Ex.Message);
            }
            
        }
        private void Search( bool searchEntity)
        {
            Notes oNotes = new Notes();
            DataSet dsEntity = new DataSet();
            SearchSvr oSearchSvr = new SearchSvr();
            
            try
            {
                if (isManageNotes == true && strEntityType != "A" && searchEntity == true)
                {
                    strEntityType = combNote.SelectedItem.DataValue.ToString();

                    if ((this.txtCode.Text != "" || this.txtName.Text != "") && strEntityType != clsEntityType.SystemNote)
                    {
                        isValid = false;
                        int index = 0;
                        string[] strEntityName = null;
                        dsEntity = oSearchSvr.Search(_Table, txtCode.Text, txtName.Text, 0, -1);

                        if (dsEntity != null)
                        {
                            if (dsEntity.Tables[0].Rows.Count > 0)
                            {
                                strEntityName = dsEntity.Tables[0].Rows[index][1].ToString().Split(',');
                                if (this.txtCode.Text != "")
                                {
                                    if (this.txtCode.Text.Trim().ToUpper() == dsEntity.Tables[0].Rows[0][0].ToString().Trim().ToUpper())
                                    {
                                        strEntityID = dsEntity.Tables[0].Rows[0][0].ToString();
                                        this.txtName.Text = strEntityName[0].Trim().ToUpper();
                                        isValid = true;
                                    }
                                }
                                else 
                                {
                                    SearchNotes();
                                    SearchEntity();
                                    return;
                                }
                            }
                            if (isValid == true)
                            {
                                this.txtCode.Text = strEntityID;
                                if (_Table == clsPOSDBConstants.Customer_tbl)
                                {
                                    oCustData = oCust.Populate(strEntityID);
                                    oCustomerRow = oCustData.Customer[0];
                                    oCustomerNotesRow.CustomerID = oCustomerRow.CustomerId;
                                    strEntityID = Configuration.convertNullToString(oCustomerRow.CustomerId);
                                    lblSelectedEntityInfo.Visible = true;
                                    lblSelectedEntityInfo.Text = "ID: " + oCustomerRow.CustomerId.ToString().Trim() + "               Name: " + oCustomerRow.CustomerFullName.Trim() + "          Address:" + oCustomerRow.Address1.Trim() + "  " + oCustomerRow.Address2.Trim() + "  " + oCustomerRow.City.Trim() + ", " + oCustomerRow.State.Trim() + "  " + oCustomerRow.Zip.Trim();
                                }
                                else
                                {
                                    setEnttiyInfo(true);
                                }
                                if (isValid == true && this.txtComments.Enabled == false)
                                {
                                    this.chkIsActive.Enabled = true;    //Sprint-23 - PRIMEPOS-413 22-Jun-2016 JY Added
                                    this.txtComments.Enabled = true;
                                }
                            }
                            else if (isValid == false)
                            {
                                if (Resources.Message.Display("The entered  " + _Table + " does not exist \n Do you want to search?.", "Select " + _Table, MessageBoxButtons.YesNo) == DialogResult.Yes)
                                {
                                    SearchEntity();
                                }
                                else
                                {
                                    this.txtCode.Focus();
                                }
                            }
                        }
                    }
                }
                SearchNotes();
            }

            catch (Exception ex)
            {
                clsUIHelper.ShowErrorMsg(ex.Message);
            }

        }

        private void combNote_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (isManageNotes == true)
                {
                    this.txtName.Text = "";
                    this.txtCode.Text = "";
                    strEntityID = "";
                    this.lblSelectedEntityInfo.Visible = false;
                    this.chkIsActive.Enabled = false;    //Sprint-23 - PRIMEPOS-413 22-Jun-2016 JY Added
                    this.txtComments.Enabled = false;
                    strEntityType = combNote.SelectedItem.DataValue.ToString();
                    if (strEntityType == "C")
                    {
                        _Table = clsPOSDBConstants.Customer_tbl;
                        strEntityType = "C";
                    }
                    if (strEntityType == clsEntityType.ItemNote)
                    {
                        _Table = clsPOSDBConstants.Item_tbl;
                        strEntityType = clsEntityType.ItemNote;
                    }
                    if (strEntityType == clsEntityType.DepartmentNote)
                    {
                        _Table = clsPOSDBConstants.Department_tbl;
                        strEntityType = clsEntityType.DepartmentNote;
                    }
                    if (strEntityType == clsEntityType.VendorNote)
                    {
                        _Table = clsPOSDBConstants.Vendor_tbl;
                        strEntityType = clsEntityType.VendorNote;
                    }

                    if (strEntityType == clsEntityType.UserNote)
                    {
                        _Table = clsPOSDBConstants.Users_tbl;
                        strEntityType = clsEntityType.UserNote;
                    }
                    if (strEntityType == clsEntityType.SystemNote)
                    {
                        _Table = "";
                        strEntityID = "SYSTEM";
                    }
                    if (strEntityType == "A")
                    {
                        btnSave.Enabled = false;
                        _Table = "";
                    }
                    else
                    {
                        btnSave.Enabled = true;
                        this.btnNewNote.Enabled = true;
                    }
                    Search(false);
                }
            }
            catch (Exception ex)
            {
                clsUIHelper.ShowErrorMsg(ex.Message);
            }
        }

        private void txtCode_ValueChanged(object sender, EventArgs e)
        {
            if (isManageNotes == true && isValid ==false)
            {
                this.lblSelectedEntityInfo.Visible = false;
                this.chkIsActive.Enabled = false;    //Sprint-23 - PRIMEPOS-413 22-Jun-2016 JY Added
                this.txtComments.Enabled = false;
            }
        }

        //Added By Shitaljit(QuicSolv) 0n 2 Nov 2011
        private void btnEdit_Click(object sender, EventArgs e)
        { 
            try
            {
                if (grdHistory.Rows.Count <= 0)
                    return;

                this.btnSave.Enabled = true;
                this.chkIsActive.Enabled = true;    //Sprint-23 - PRIMEPOS-413 22-Jun-2016 JY Added
                this.txtComments.Enabled = true;
                this.txtComments.Focus();

                #region Sprint-23 - PRIMEPOS-413 22-Jun-2016 JY Commented
                //this.lblSelectedEntityInfo.Text = "";
                //if (_Table == clsPOSDBConstants.Customer_tbl)
                //{
                //    oCustomerNotesData = oCustomerNotes.PopulateByID(Resources.Configuration.convertNullToInt(this.grdHistory.ActiveRow.Cells[clsPOSDBConstants.CustomerNotes_Fld_ID].Text));

                //    if (oCustomerNotesData.CustomerNotes.Rows.Count > 0)
                //    {
                //        oCustomerNotesRow = oCustomerNotesData.CustomerNotes[0];
                //        oCustData = oCust.Populate(oCustomerNotesRow.CustomerID.ToString());
                //        oCustomerRow = oCustData.Customer[0];
                //        PopulateData();
                //        if (isManageNotes == true)
                //        {
                //            this.lblSelectedEntityInfo.Visible = true;
                //            this.lblSelectedEntityInfo.Text = _Table + " Information         " + "Code: " + oCustomerRow.CustomerId + "                  " + "Name:  " + oCustomerRow.CustomerFullName;
                //        }
                //    }
                //}
                //else
                //{
                //    int NoteID = Configuration.convertNullToInt(this.grdHistory.ActiveRow.Cells[clsPOSDBConstants.Notes_Fld_NoteId].Text);
                //    strEntityID = Configuration.convertNullToString(this.grdHistory.ActiveRow.Cells[clsPOSDBConstants.Notes_Fld_EntityId].Text);
                //    strEntityType = Configuration.convertNullToString(this.grdHistory.ActiveRow.Cells[clsPOSDBConstants.Notes_Fld_EntityType].Text);
                //    oNotesData = oNotes.Populate(NoteID);
                //    if (oNotesData.Notes.Rows.Count > 0)
                //    {
                //        oNotesRow = oNotesData.Notes.GetRowByID(NoteID);
                //        this.txtComments.Text = oNotesRow.Note;
                //        this.chkIsActive.Checked = oNotesRow.POPUPMSG;
                //        setEnttiyInfo(false);
                //        this.lblSelectedEntityInfo.Visible = true;
                //        this.txtComments.Enabled = true;
                //        this.txtComments.Focus();
                //    }
                //}
                #endregion
            }
            catch (Exception ex)
            {
                clsUIHelper.ShowErrorMsg(ex.Message);
            }
        }


        private void setEnttiyInfo(bool isSearchEntity)
        {
            if (isManageNotes == true)
            {
                this.lblSelectedEntityInfo.Visible = true;
                lblSelectedEntityInfo.Text = "";
                if (strEntityType == clsEntityType.VendorNote)
                {
                    POS_Core.BusinessRules.Vendor oVendor = new POS_Core.BusinessRules.Vendor();
                    VendorData oVendorData = new VendorData();
                    VendorRow oVendorRow = null;
                    if (isSearchEntity == true)
                    {
                        oVendorData = oVendor.Populate(strEntityID);
                    }
                    else
                    {
                        oVendorData = oVendor.Populate(Configuration.convertNullToInt(strEntityID));
                    }
                    if (oVendorData.Tables[0].Rows.Count > 0)
                    {
                        oVendorRow = (VendorRow)oVendorData.Vendor[0];
                        strEntityID = oVendorRow.VendorId.ToString();
                        if (isSearchEntity == true)
                        {
                            this.txtCode.Text = strEntityID;
                            this.txtName.Text = oVendorRow.Vendorname.Trim();
                        }
                        lblSelectedEntityInfo.Text = "Vendor Code:  " + oVendorRow.Vendorcode.Trim() + "          Vendor Name:    " + oVendorRow.Vendorname.Trim() + "                 Address: " + oVendorRow.Address1.Trim() + " " + oVendorRow.Address2.Trim() + " " + oVendorRow.City.Trim() + ", " + oVendorRow.State.Trim() + " " + oVendorRow.Zip.Trim();
                    }
                }
                else if (strEntityType == clsEntityType.DepartmentNote)
                {
                    Department oDept = new Department();
                    DepartmentData oDeptData = new DepartmentData();
                    DepartmentRow oDeptRow = null;
                    if (isSearchEntity == true)
                    {
                        oDeptData = oDept.Populate(strEntityID);
                    }
                    else
                    {
                        oDeptData = oDept.Populate(Configuration.convertNullToInt(strEntityID));
                    }
                    if (oDeptData.Tables[0].Rows.Count > 0)
                    {
                        oDeptRow = (DepartmentRow)oDeptData.Department[0];
                        strEntityID = oDeptRow.DeptID.ToString();
                        if (isSearchEntity == true)
                        {
                            this.txtCode.Text = strEntityID;
                            this.txtName.Text = oDeptRow.DeptName.Trim();
                        }
                        lblSelectedEntityInfo.Text = "Department Code:    " + oDeptRow.DeptCode.Trim() + "                    Department Name:    " + oDeptRow.DeptName.Trim();
                    }
                }
                else if (strEntityType == clsEntityType.UserNote)
                {
                    User oUser = new User();
                    UserData oUserData = new UserData();
                    UserRow oUserRow = null;
                    oUserData = oUser.GetUserByUserID(strEntityID);
                    if (oUserData.Tables[0].Rows.Count > 0)
                    {
                        oUserRow = (UserRow)oUserData.User[0];
                        strEntityID = oUserRow.UserID;
                        if (isSearchEntity == true)
                        {
                            this.txtName.Text = oUserRow.LastName.Trim();
                            this.txtCode.Text = strEntityID;
                        }
                        lblSelectedEntityInfo.Text = "Users ID:    " + oUserRow.UserID + "                    Users Name:    " + oUserRow.LastName.Trim() + "," + oUserRow.FirstName.Trim();
                    }
                }
                else if (strEntityType == clsEntityType.ItemNote)
                {
                    Item oItem = new Item();
                    ItemData oItemData = new ItemData();
                    ItemRow oItemRow = null;
                    oItemData = oItem.Populate(strEntityID);
                    if (oItemData.Tables[0].Rows.Count > 0)
                    {
                        oItemRow = (ItemRow)oItemData.Item[0];
                        strEntityID = oItemRow.ItemID;
                        if (isSearchEntity == true)
                        {
                            this.txtCode.Text = strEntityID;
                            this.txtName.Text = oItemRow.Description;
                        }
                        lblSelectedEntityInfo.Text = "Items Code:    " + oItemRow.ItemID + "                        Items Description:    " + oItemRow.Description;
                    }
                }
                else if (strEntityType == clsEntityType.SystemNote)
                {
                    lblSelectedEntityInfo.Text = "Station: " + Configuration.StationName.Trim() + "               Pharmacy: " + Configuration.CInfo.StoreName.Trim() + "                     " + Application.ProductName.Trim() + "  Version:   " + Application.ProductVersion.Trim();
                }
            }
        }
        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (grdHistory.Rows.Count <= 0)
                    return;
                if (Resources.Message.Display("This action will delete selected record. Are your sure?", "Payout", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    if (_Table == clsPOSDBConstants.Customer_tbl)
                    {
                        oCustomerNotes.DeleteRow(Configuration.convertNullToInt(this.grdHistory.ActiveRow.Cells[clsPOSDBConstants.CustomerNotes_Fld_ID].Text));
                    }
                    else
                    {
                        NotesSvr oNotesSvr = new NotesSvr();
                        int NoteID = Configuration.convertNullToInt(this.grdHistory.ActiveRow.Cells[clsPOSDBConstants.Notes_Fld_NoteId].Text);
                        oNotesSvr.DeleteRow(NoteID);
                    }
                    Clear();
                    Search(false);
                }
            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }

        private void btnNewNote_Click(object sender, EventArgs e)
        {
            SetNew();   //Sprint-23 - PRIMEPOS-413 22-Jun-2016 JY Added
            grdHistory.Selected.Rows.Clear();   //Sprint-23 - PRIMEPOS-413 22-Jun-2016 JY Added
            this.btnSave.Enabled = true;    //Sprint-23 - PRIMEPOS-413 22-Jun-2016 JY Added
            this.chkIsActive.Enabled = true;    //Sprint-23 - PRIMEPOS-413 22-Jun-2016 JY Added
            this.txtComments.Enabled = true;
            this.chkIsActive.Checked = false;
            oNotesData = new NotesData();
            oNotesRow = oNotesData.Notes.AddRow(0, "", "", "", System.DateTime.MinValue, "", System.DateTime.MinValue, "", false);
            this.txtComments.Text = "";
            this.txtComments.Focus();
        }

        private void grdHistory_AfterSelectChange(object sender, AfterSelectChangeEventArgs e)
        {
            if (grdHistory.Selected.Rows.Count > 0)
            {
                this.txtCode.Text = String.Empty;
                this.txtName.Text = String.Empty;
                this.btnNewNote.Enabled = true;
                this.btnEdit.Enabled = true;
                this.btnDelete.Enabled = true;
                //btnEdit_Click(null, null);    //Sprint-23 - PRIMEPOS-413 22-Jun-2016 JY Commented
                #region Sprint-23 - PRIMEPOS-413 22-Jun-2016 JY Added
                PopulateNotes();    
                this.btnSave.Enabled = false;
                this.chkIsActive.Enabled = false;    //Sprint-23 - PRIMEPOS-413 22-Jun-2016 JY Added
                this.txtComments.Enabled = false;
                #endregion
            }
        }

        private void textBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter && isManageNotes == true)
            {
                if (this.txtCode.Text != "" || this.txtName.Text != "")
                {
                    Search(true);
                }
            }
        }

        //Sprint-23 - PRIMEPOS-413 22-Jun-2016 JY Added to populate notes
        private void PopulateNotes()
        {
            try
            {
                if (grdHistory.Rows.Count <= 0)
                    return;
                
                if (_Table == clsPOSDBConstants.Customer_tbl)
                {
                    oCustomerNotesData = oCustomerNotes.PopulateByID(Configuration.convertNullToInt(this.grdHistory.ActiveRow.Cells[clsPOSDBConstants.CustomerNotes_Fld_ID].Text));

                    if (oCustomerNotesData.CustomerNotes.Rows.Count > 0)
                    {
                        oCustomerNotesRow = oCustomerNotesData.CustomerNotes[0];
                        oCustData = oCust.Populate(oCustomerNotesRow.CustomerID.ToString());
                        oCustomerRow = oCustData.Customer[0];

                        this.txtCode.Text = oCustomerNotesRow.CustomerID.ToString();
                        this.txtName.Text = oCustomerRow.CustomerName;
                        this.txtComments.Text = oCustomerNotesRow.Notes;
                        this.chkIsActive.Checked = oCustomerNotesRow.IsActive;

                        if (isManageNotes == true)
                        {
                            this.lblSelectedEntityInfo.Visible = true;
                            this.lblSelectedEntityInfo.Text = _Table + " Information         " + "Code: " + oCustomerRow.CustomerId + "                  " + "Name:  " + oCustomerRow.CustomerFullName;
                        }
                    }
                }
                else
                {
                    int NoteID = Configuration.convertNullToInt(this.grdHistory.ActiveRow.Cells[clsPOSDBConstants.Notes_Fld_NoteId].Text);
                    strEntityID = Configuration.convertNullToString(this.grdHistory.ActiveRow.Cells[clsPOSDBConstants.Notes_Fld_EntityId].Text);
                    strEntityType = Configuration.convertNullToString(this.grdHistory.ActiveRow.Cells[clsPOSDBConstants.Notes_Fld_EntityType].Text);
                    oNotesData = oNotes.Populate(NoteID);
                    if (oNotesData.Notes.Rows.Count > 0)
                    {
                        oNotesRow = oNotesData.Notes.GetRowByID(NoteID);
                        this.txtComments.Text = oNotesRow.Note;
                        this.chkIsActive.Checked = oNotesRow.POPUPMSG;
                        setEnttiyInfo(false);
                        this.lblSelectedEntityInfo.Visible = true;
                    }
                }
                this.btnSave.Enabled = false;
                this.chkIsActive.Enabled = false;    //Sprint-23 - PRIMEPOS-413 22-Jun-2016 JY Added
                this.txtComments.Enabled = false;
                this.lblSelectedEntityInfo.Text = "";
            }
            catch (Exception ex)
            {
                clsUIHelper.ShowErrorMsg(ex.Message);
            }

        }
    }
    //Added By Shitaljit(QuicSolv) 0n 5 oct 2011
    public class clsEntityType
    {
        public const string ItemNote = "I";
        public const string VendorNote = "V";
        public const string DepartmentNote = "D";
        public const string UserNote = "U";
        public const string SystemNote = "S";
        public const string RXNote = "R";
        public const string PatNote = "P";
    }
    //Added By Shitaljit(QuicSolv) 0n 5 oct 2011
}
