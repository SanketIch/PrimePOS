using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using POS_Core.CommonData;
using POS_Core.CommonData.Rows;
using POS_Core.BusinessRules;
using POS_Core.ErrorLogging;
using Infragistics.Win.UltraWinGrid;
using System.Data;
//using POS_Core.DataAccess;
using NLog;
using POS_Core.Resources;

namespace POS_Core_UI
{
    /// <summary>
    /// Summary description for frmPayOut.
    /// </summary>
    public class frmItemMonitorCategoryDetail : System.Windows.Forms.Form
    {
        private bool m_FromError = false;
        private ItemMonitorCategoryDetail oItemMonitorCategoryDetail = new ItemMonitorCategoryDetail();
        private ItemMonitorCategoryDetailData oItemMonitorCategoryDetailData = new ItemMonitorCategoryDetailData();
        private ItemMonitorCategoryDetailData oTempItemMonitorCategoryDetailData = new ItemMonitorCategoryDetailData();
        ItemMonitorCategoryDetailRow oItemItemMonitorCategoryDetailRow = null;

        private string mItemId = "";
        private string mItemDescription = "";
        private int mItemMonCatID = 0;
        private int m_rowIndex = -1;
        private int m_ColIndex = -1;
        private bool isCellUpdateCalled;
        private static ILogger logger = LogManager.GetCurrentClassLogger();

        #region Windows Form Designer generated code
        private Infragistics.Win.UltraWinDataSource.UltraDataSource ultraDataSource1;
        private Infragistics.Win.UltraWinDataSource.UltraDataSource ultraDataSource2;
        private Infragistics.Win.Misc.UltraButton btnClose;
        private Infragistics.Win.Misc.UltraButton btnSave;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtItemCode;
        private System.Windows.Forms.TextBox txtCompanionItemCode;
        private Infragistics.Win.Misc.UltraButton btnSearch;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private Infragistics.Win.Misc.UltraLabel lblTransactionType;
        private GroupBox groupBox1;
        private UltraGrid grdDetail;
        private Label lblItemDetail;
        private GroupBox groupBox4;
        private Infragistics.Win.UltraWinDataSource.UltraDataSource ultraDataSource3;
        private IContainer components;
        #endregion

        //Added By shitaljit on 2 May 2012
        private DataSet SearchData(string sWhereClause)
        {
            string sSQL = "Select Detail."
                                    + clsPOSDBConstants.ItemMonitorCategory_Fld_ID
                                    + " , Detail." + clsPOSDBConstants.Item_Fld_ItemID
                                    + " , Master." + clsPOSDBConstants.ItemMonitorCategory_Fld_Description + " As Description "
                                    + " , Master." + clsPOSDBConstants.ItemMonitorCategory_Fld_ePSE + " As ePSE " //Sprint-23 - PRIMEPOS-2029 30-Mar-2016 JY Added
                //+ " , Master." + clsPOSDBConstants.ItemMonitorCategory_Fld_DailyLimit //Sprint-23 - PRIMEPOS-2029 30-Mar-2016 JY Commented 
                //+ " , Master." + clsPOSDBConstants.ItemMonitorCategory_Fld_ThirtyDaysLimit    //Sprint-23 - PRIMEPOS-2029 30-Mar-2016 JY Commented
                                    + " , Master." + clsPOSDBConstants.ItemMonitorCategory_Fld_LimitPeriodDays 
                                    + " , Master." + clsPOSDBConstants.ItemMonitorCategory_Fld_LimitPeriodQty 
                                    + " , Detail." + clsPOSDBConstants.ItemMonitorCategory_Fld_ItemMonCatID
                                    + " , Detail." + clsPOSDBConstants.fld_UserID
                                    + " , Detail." + clsPOSDBConstants.ItemMonitorCategoryDetail_Fld_UnitsPerPackage
                                + " FROM "
                                    + clsPOSDBConstants.ItemMonitorCategoryDetail_tbl + " As Detail"
                                    + " , " + clsPOSDBConstants.ItemMonitorCategory_tbl + " As Master "
                                + " WHERE "
                                    + " Master." + clsPOSDBConstants.ItemMonitorCategory_Fld_ID + " = Detail." + clsPOSDBConstants.ItemMonitorCategory_Fld_ItemMonCatID;

            string WhereClause = String.Concat(" and ", sWhereClause);

            sSQL = String.Concat(sSQL, WhereClause);
            Search oSearch = new Search();
            DataSet ds = null;
            ds = oSearch.SearchData(sSQL);
            return ds;

        }

        public frmItemMonitorCategoryDetail(string ItemId, string ItemDescription)
        {
            //
            // Required for Windows Form Designer support
            //
            mItemId = ItemId;
            mItemDescription = ItemDescription;
            InitializeComponent();
            this.lblItemDetail.Text = mItemId.Trim() + "( " + mItemDescription.Trim() + " )";
            SetNew();
            try
            {
                oItemMonitorCategoryDetailData = oItemMonitorCategoryDetail.PopulateList(" Detail." + clsPOSDBConstants.Item_Fld_ItemID + " = '" + mItemId + "'");
                oTempItemMonitorCategoryDetailData = oItemMonitorCategoryDetailData;
                grdDetail.DataSource = oItemMonitorCategoryDetailData;
                
                grdDetail.Refresh();
                ApplyGrigFormat();
            }
            catch (POSExceptions exp)
            {
                logger.Fatal(exp, "frmItemMonitorCategoryDetail(string ItemId, string ItemDescription)");
                clsUIHelper.ShowErrorMsg(exp.ErrMessage);
            }

            //
            // TODO: Add any constructor code after InitializeComponent call
            //
        }

        private void SetNew()
        {
            oItemItemMonitorCategoryDetailRow = oItemMonitorCategoryDetailData.ItemMonitorCategoryDetail.AddRow(0, "", "", 0, "", 0);
        }
        private void ApplyGrigFormat()
        {

            grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.ItemMonitorCategory_Fld_ItemMonCatID].MaxLength = 20;
            grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.ItemMonitorCategory_Fld_ItemMonCatID].Width = 150;
            grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.ItemMonitorCategory_Fld_ItemMonCatID].CellClickAction = CellClickAction.Edit;
            grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.ItemMonitorCategory_Fld_ItemMonCatID].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.EditButton;
            grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.ItemMonitorCategory_Fld_ItemMonCatID].ButtonDisplayStyle = Infragistics.Win.UltraWinGrid.ButtonDisplayStyle.OnCellActivate;
            grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.ItemMonitorCategory_Fld_Description].Width = 300;
            grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.Item_Fld_ItemID].Hidden = true;
            grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.ItemMonitorCategory_Fld_ID].Hidden = true;
            grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.fld_UserID].Hidden = true;

            grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.ItemMonitorCategory_Fld_Description].CellActivation = Infragistics.Win.UltraWinGrid.Activation.Disabled;
            grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.ItemMonitorCategory_Fld_Description].Header.SetVisiblePosition(1, false);

            grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.ItemMonitorCategory_Fld_ePSE].CellActivation = Infragistics.Win.UltraWinGrid.Activation.Disabled;    //Sprint-23 - PRIMEPOS-2029 30-Mar-2016 JY Added
            grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.ItemMonitorCategory_Fld_ePSE].Header.SetVisiblePosition(2, false);   //Sprint-23 - PRIMEPOS-2029 30-Mar-2016 JY Added
            grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.ItemMonitorCategory_Fld_Description].Width = 100;

            grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.ItemMonitorCategory_Fld_ItemMonCatID].Header.SetVisiblePosition(0, false);
            grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.ItemMonitorCategory_Fld_ItemMonCatID].Header.Caption = "Cat. ID";
            
            //Added By shitaljit on 2 May 2012
            grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.ItemMonitorCategoryDetail_Fld_UnitsPerPackage].Header.Caption = "Units";
            grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.ItemMonitorCategoryDetail_Fld_UnitsPerPackage].CellClickAction = CellClickAction.EditAndSelectText;
            grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.ItemMonitorCategoryDetail_Fld_UnitsPerPackage].CellActivation = Activation.AllowEdit;
            grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.ItemMonitorCategoryDetail_Fld_UnitsPerPackage].Format = "#######0.00";
            grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.ItemMonitorCategoryDetail_Fld_UnitsPerPackage].CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right;
            this.grdDetail.DisplayLayout.Bands[0].Columns["Delete"].Header.SetVisiblePosition(9, false);

            //grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.ItemMonitorCategory_Fld_DailyLimit].Header.Caption = "Daily";
            //grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.ItemMonitorCategory_Fld_DailyLimit].CellClickAction = CellClickAction.RowSelect;
            //grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.ItemMonitorCategory_Fld_DailyLimit].TabStop = false;

            //grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.ItemMonitorCategory_Fld_ThirtyDaysLimit].Header.Caption = "30Days";
            //grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.ItemMonitorCategory_Fld_ThirtyDaysLimit].CellClickAction = CellClickAction.RowSelect;
            //grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.ItemMonitorCategory_Fld_ThirtyDaysLimit].TabStop = false;

            grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.ItemMonitorCategory_Fld_LimitPeriodDays].Header.Caption = "Limit Period";
            grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.ItemMonitorCategory_Fld_LimitPeriodDays].Width = 150;
            grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.ItemMonitorCategory_Fld_LimitPeriodDays].CellClickAction = CellClickAction.RowSelect;
            grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.ItemMonitorCategory_Fld_LimitPeriodDays].TabStop = false;

            grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.ItemMonitorCategory_Fld_LimitPeriodQty].Header.Caption = "Limit Qty";
            grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.ItemMonitorCategory_Fld_LimitPeriodQty].CellClickAction = CellClickAction.RowSelect;
            grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.ItemMonitorCategory_Fld_LimitPeriodQty].TabStop = false;
            grdDetail.DisplayLayout.AutoFitStyle = AutoFitStyle.ResizeAllColumns;
            //till here added by SHITALJIT 
            clsUIHelper.SetAppearance(this.grdDetail);
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
            Infragistics.Win.UltraWinDataSource.UltraDataColumn ultraDataColumn1 = new Infragistics.Win.UltraWinDataSource.UltraDataColumn("Item Code");
            Infragistics.Win.UltraWinDataSource.UltraDataColumn ultraDataColumn2 = new Infragistics.Win.UltraWinDataSource.UltraDataColumn("Description");
            Infragistics.Win.UltraWinDataSource.UltraDataColumn ultraDataColumn3 = new Infragistics.Win.UltraWinDataSource.UltraDataColumn("Amount");
            Infragistics.Win.UltraWinDataSource.UltraDataColumn ultraDataColumn4 = new Infragistics.Win.UltraWinDataSource.UltraDataColumn("Percentage");
            Infragistics.Win.UltraWinDataSource.UltraDataColumn ultraDataColumn5 = new Infragistics.Win.UltraWinDataSource.UltraDataColumn("Qty");
            Infragistics.Win.UltraWinDataSource.UltraDataColumn ultraDataColumn6 = new Infragistics.Win.UltraWinDataSource.UltraDataColumn("Cost");
            Infragistics.Win.UltraWinDataSource.UltraDataColumn ultraDataColumn7 = new Infragistics.Win.UltraWinDataSource.UltraDataColumn("Price");
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmItemMonitorCategoryDetail));
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand1 = new Infragistics.Win.UltraWinGrid.UltraGridBand("Band 0", -1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn1 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Item Code");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn2 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Description");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn3 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Delete", 0);
            Infragistics.Win.Appearance appearance7 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn4 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("UnitsPerPackage", 1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn7 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("LimitPeriodDays", 2);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn8 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("LimitPeriodQty", 3);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn9 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("ePSE", 4);
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
            Infragistics.Win.Appearance appearance25 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance26 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance27 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance28 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinScrollBar.ScrollBarLook scrollBarLook1 = new Infragistics.Win.UltraWinScrollBar.ScrollBarLook();
            Infragistics.Win.Appearance appearance29 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinDataSource.UltraDataColumn ultraDataColumn8 = new Infragistics.Win.UltraWinDataSource.UltraDataColumn("Item Code");
            Infragistics.Win.UltraWinDataSource.UltraDataColumn ultraDataColumn9 = new Infragistics.Win.UltraWinDataSource.UltraDataColumn("Description");
            this.ultraDataSource2 = new Infragistics.Win.UltraWinDataSource.UltraDataSource(this.components);
            this.ultraDataSource1 = new Infragistics.Win.UltraWinDataSource.UltraDataSource(this.components);
            this.btnClose = new Infragistics.Win.Misc.UltraButton();
            this.btnSave = new Infragistics.Win.Misc.UltraButton();
            this.btnSearch = new Infragistics.Win.Misc.UltraButton();
            this.label2 = new System.Windows.Forms.Label();
            this.txtItemCode = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtCompanionItemCode = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.lblTransactionType = new Infragistics.Win.Misc.UltraLabel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.grdDetail = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.ultraDataSource3 = new Infragistics.Win.UltraWinDataSource.UltraDataSource(this.components);
            this.lblItemDetail = new System.Windows.Forms.Label();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            ((System.ComponentModel.ISupportInitialize)(this.ultraDataSource2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraDataSource1)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdDetail)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraDataSource3)).BeginInit();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // ultraDataSource2
            // 
            this.ultraDataSource2.Band.Columns.AddRange(new object[] {
            ultraDataColumn1,
            ultraDataColumn2,
            ultraDataColumn3,
            ultraDataColumn4});
            // 
            // ultraDataSource1
            // 
            this.ultraDataSource1.Band.Columns.AddRange(new object[] {
            ultraDataColumn5,
            ultraDataColumn6,
            ultraDataColumn7});
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            appearance1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(62)))), ((int)(((byte)(176)))));
            appearance1.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance1.FontData.BoldAsString = "True";
            appearance1.ForeColor = System.Drawing.Color.White;
            this.btnClose.Appearance = appearance1;
            this.btnClose.ButtonStyle = Infragistics.Win.UIElementButtonStyle.OfficeXPToolbarButton;
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(703, 15);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(90, 26);
            this.btnClose.TabIndex = 3;
            this.btnClose.Text = "&Cancel";
            this.btnClose.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnClose.Click += new System.EventHandler(this.ultraButton2_Click);
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            appearance2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(62)))), ((int)(((byte)(176)))));
            appearance2.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance2.FontData.BoldAsString = "True";
            appearance2.ForeColor = System.Drawing.Color.White;
            this.btnSave.Appearance = appearance2;
            this.btnSave.ButtonStyle = Infragistics.Win.UIElementButtonStyle.OfficeXPToolbarButton;
            this.btnSave.Location = new System.Drawing.Point(605, 15);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(90, 26);
            this.btnSave.TabIndex = 2;
            this.btnSave.Text = "&Ok";
            this.btnSave.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnSearch
            // 
            appearance3.ForeColor = System.Drawing.Color.Black;
            appearance3.Image = ((object)(resources.GetObject("appearance3.Image")));
            this.btnSearch.Appearance = appearance3;
            this.btnSearch.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            this.btnSearch.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSearch.Location = new System.Drawing.Point(440, 11);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(73, 26);
            this.btnSearch.TabIndex = 5;
            this.btnSearch.Text = "S&earch";
            this.btnSearch.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.Black;
            this.label2.Location = new System.Drawing.Point(237, 17);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(71, 16);
            this.label2.TabIndex = 3;
            this.label2.Text = "Item Code";
            // 
            // txtItemCode
            // 
            this.txtItemCode.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtItemCode.Location = new System.Drawing.Point(313, 15);
            this.txtItemCode.Name = "txtItemCode";
            this.txtItemCode.Size = new System.Drawing.Size(110, 21);
            this.txtItemCode.TabIndex = 4;
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Location = new System.Drawing.Point(6, 30);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(101, 16);
            this.label1.TabIndex = 1;
            this.label1.Text = "Cmpn Itm Code";
            // 
            // txtCompanionItemCode
            // 
            this.txtCompanionItemCode.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtCompanionItemCode.Location = new System.Drawing.Point(121, 15);
            this.txtCompanionItemCode.Name = "txtCompanionItemCode";
            this.txtCompanionItemCode.Size = new System.Drawing.Size(110, 21);
            this.txtCompanionItemCode.TabIndex = 3;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.txtCompanionItemCode);
            this.groupBox2.Controls.Add(this.btnSearch);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.txtItemCode);
            this.groupBox2.Font = new System.Drawing.Font("Verdana", 8.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.ForeColor = System.Drawing.Color.Blue;
            this.groupBox2.Location = new System.Drawing.Point(38, 139);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(50, 49);
            this.groupBox2.TabIndex = 29;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Sea&rch";
            this.groupBox2.Visible = false;
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox3.Controls.Add(this.btnClose);
            this.groupBox3.Controls.Add(this.btnSave);
            this.groupBox3.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.groupBox3.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox3.Location = new System.Drawing.Point(10, 343);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(808, 54);
            this.groupBox3.TabIndex = 27;
            this.groupBox3.TabStop = false;
            // 
            // lblTransactionType
            // 
            appearance4.BackGradientStyle = Infragistics.Win.GradientStyle.None;
            appearance4.BorderAlpha = Infragistics.Win.Alpha.Transparent;
            appearance4.ForeColor = System.Drawing.Color.White;
            appearance4.ForeColorDisabled = System.Drawing.Color.White;
            appearance4.TextHAlignAsString = "Center";
            this.lblTransactionType.Appearance = appearance4;
            this.lblTransactionType.BackColorInternal = System.Drawing.Color.Transparent;
            this.lblTransactionType.BorderStyleOuter = Infragistics.Win.UIElementBorderStyle.Solid;
            this.lblTransactionType.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblTransactionType.Font = new System.Drawing.Font("Microsoft Sans Serif", 24.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTransactionType.Location = new System.Drawing.Point(0, 0);
            this.lblTransactionType.Name = "lblTransactionType";
            this.lblTransactionType.Size = new System.Drawing.Size(828, 49);
            this.lblTransactionType.TabIndex = 31;
            this.lblTransactionType.Tag = "Header";
            this.lblTransactionType.Text = "Item Monitor Category Detail";
            this.lblTransactionType.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.grdDetail);
            this.groupBox1.Controls.Add(this.groupBox2);
            this.groupBox1.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.groupBox1.Font = new System.Drawing.Font("Verdana", 9F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.ForeColor = System.Drawing.Color.White;
            this.groupBox1.Location = new System.Drawing.Point(10, 110);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(808, 231);
            this.groupBox1.TabIndex = 27;
            this.groupBox1.TabStop = false;
            // 
            // grdDetail
            // 
            this.grdDetail.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grdDetail.DataSource = this.ultraDataSource3;
            appearance5.BackColor = System.Drawing.Color.White;
            appearance5.BackColor2 = System.Drawing.Color.White;
            appearance5.BackGradientStyle = Infragistics.Win.GradientStyle.None;
            this.grdDetail.DisplayLayout.AddNewBox.Appearance = appearance5;
            appearance6.BackColor = System.Drawing.Color.White;
            appearance6.BackColor2 = System.Drawing.Color.White;
            appearance6.BackColorDisabled = System.Drawing.Color.White;
            appearance6.BackColorDisabled2 = System.Drawing.Color.White;
            appearance6.BackGradientStyle = Infragistics.Win.GradientStyle.None;
            this.grdDetail.DisplayLayout.Appearance = appearance6;
            ultraGridColumn1.ButtonDisplayStyle = Infragistics.Win.UltraWinGrid.ButtonDisplayStyle.Always;
            ultraGridColumn1.Header.VisiblePosition = 0;
            ultraGridColumn1.MaskInput = "{LOC}nnnnnnn.nn";
            ultraGridColumn1.ShowInkButton = Infragistics.Win.ShowInkButton.Always;
            ultraGridColumn1.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.EditButton;
            ultraGridColumn1.Width = 104;
            ultraGridColumn2.Header.VisiblePosition = 1;
            ultraGridColumn2.Width = 165;
            ultraGridColumn3.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.None;
            ultraGridColumn3.ButtonDisplayStyle = Infragistics.Win.UltraWinGrid.ButtonDisplayStyle.Always;
            ultraGridColumn3.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit;
            appearance7.Image = ((object)(resources.GetObject("appearance7.Image")));
            ultraGridColumn3.CellButtonAppearance = appearance7;
            ultraGridColumn3.DefaultCellValue = "Delete";
            ultraGridColumn3.Header.Caption = "";
            ultraGridColumn3.Header.VisiblePosition = 4;
            ultraGridColumn3.NullText = "Delete";
            ultraGridColumn3.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.Button;
            ultraGridColumn3.TabStop = false;
            ultraGridColumn4.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.Edit;
            ultraGridColumn4.DataType = typeof(decimal);
            ultraGridColumn4.ExcludeFromColumnChooser = Infragistics.Win.UltraWinGrid.ExcludeFromColumnChooser.True;
            ultraGridColumn4.Header.VisiblePosition = 3;
            ultraGridColumn7.Header.VisiblePosition = 5;
            ultraGridColumn8.Header.VisiblePosition = 6;
            ultraGridColumn9.Header.VisiblePosition = 2;
            ultraGridBand1.Columns.AddRange(new object[] {
            ultraGridColumn1,
            ultraGridColumn2,
            ultraGridColumn3,
            ultraGridColumn4,
            ultraGridColumn7,
            ultraGridColumn8,
            ultraGridColumn9});
            this.grdDetail.DisplayLayout.BandsSerializer.Add(ultraGridBand1);
            this.grdDetail.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.grdDetail.DisplayLayout.InterBandSpacing = 10;
            this.grdDetail.DisplayLayout.MaxColScrollRegions = 1;
            this.grdDetail.DisplayLayout.MaxRowScrollRegions = 1;
            appearance8.BackColor = System.Drawing.Color.White;
            appearance8.BackColor2 = System.Drawing.Color.White;
            this.grdDetail.DisplayLayout.Override.ActiveCardCaptionAppearance = appearance8;
            appearance9.BackColor = System.Drawing.Color.White;
            appearance9.BackColor2 = System.Drawing.Color.White;
            this.grdDetail.DisplayLayout.Override.ActiveCellAppearance = appearance9;
            appearance10.BackColor = System.Drawing.Color.White;
            appearance10.BackColor2 = System.Drawing.Color.White;
            appearance10.BorderColor = System.Drawing.Color.Gray;
            this.grdDetail.DisplayLayout.Override.ActiveRowAppearance = appearance10;
            appearance11.BackColor = System.Drawing.Color.White;
            appearance11.BackColor2 = System.Drawing.Color.White;
            appearance11.BackColorAlpha = Infragistics.Win.Alpha.Opaque;
            appearance11.BackGradientStyle = Infragistics.Win.GradientStyle.None;
            appearance11.BorderColor = System.Drawing.Color.Gray;
            this.grdDetail.DisplayLayout.Override.AddRowAppearance = appearance11;
            this.grdDetail.DisplayLayout.Override.AllowAddNew = Infragistics.Win.UltraWinGrid.AllowAddNew.TemplateOnBottom;
            this.grdDetail.DisplayLayout.Override.AllowColMoving = Infragistics.Win.UltraWinGrid.AllowColMoving.WithinBand;
            this.grdDetail.DisplayLayout.Override.AllowColSwapping = Infragistics.Win.UltraWinGrid.AllowColSwapping.NotAllowed;
            this.grdDetail.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.True;
            this.grdDetail.DisplayLayout.Override.AllowRowFiltering = Infragistics.Win.DefaultableBoolean.False;
            this.grdDetail.DisplayLayout.Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.True;
            appearance12.BackColor = System.Drawing.Color.Transparent;
            this.grdDetail.DisplayLayout.Override.CardAreaAppearance = appearance12;
            appearance13.BackColor = System.Drawing.Color.White;
            appearance13.BackColor2 = System.Drawing.Color.White;
            appearance13.BackColorDisabled = System.Drawing.Color.White;
            appearance13.BackColorDisabled2 = System.Drawing.Color.White;
            appearance13.BorderColor = System.Drawing.Color.Black;
            appearance13.BorderColor3DBase = System.Drawing.Color.Black;
            this.grdDetail.DisplayLayout.Override.CellAppearance = appearance13;
            appearance14.BackColor = System.Drawing.Color.White;
            appearance14.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(231)))), ((int)(((byte)(230)))));
            appearance14.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance14.BorderAlpha = Infragistics.Win.Alpha.Transparent;
            appearance14.BorderColor = System.Drawing.Color.Gray;
            appearance14.BorderColor3DBase = System.Drawing.Color.Gray;
            appearance14.Image = ((object)(resources.GetObject("appearance14.Image")));
            appearance14.ImageBackgroundAlpha = Infragistics.Win.Alpha.Transparent;
            appearance14.ImageBackgroundStyle = Infragistics.Win.ImageBackgroundStyle.Centered;
            this.grdDetail.DisplayLayout.Override.CellButtonAppearance = appearance14;
            appearance15.BackColor = System.Drawing.Color.White;
            this.grdDetail.DisplayLayout.Override.DataErrorRowAppearance = appearance15;
            appearance16.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            appearance16.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.grdDetail.DisplayLayout.Override.EditCellAppearance = appearance16;
            appearance17.BorderColor = System.Drawing.Color.Gray;
            this.grdDetail.DisplayLayout.Override.FilteredInRowAppearance = appearance17;
            appearance18.BorderColor = System.Drawing.Color.Gray;
            this.grdDetail.DisplayLayout.Override.FilteredOutRowAppearance = appearance18;
            appearance19.BackColor = System.Drawing.Color.White;
            appearance19.BackColor2 = System.Drawing.Color.White;
            appearance19.BackColorDisabled = System.Drawing.Color.White;
            appearance19.BackColorDisabled2 = System.Drawing.Color.White;
            this.grdDetail.DisplayLayout.Override.FixedCellAppearance = appearance19;
            appearance20.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            appearance20.BackColor2 = System.Drawing.Color.Beige;
            this.grdDetail.DisplayLayout.Override.FixedHeaderAppearance = appearance20;
            appearance21.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(62)))), ((int)(((byte)(176)))));
            appearance21.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance21.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance21.FontData.BoldAsString = "True";
            appearance21.FontData.SizeInPoints = 9F;
            appearance21.ForeColor = System.Drawing.Color.White;
            appearance21.TextHAlignAsString = "Left";
            appearance21.ThemedElementAlpha = Infragistics.Win.Alpha.Transparent;
            this.grdDetail.DisplayLayout.Override.HeaderAppearance = appearance21;
            this.grdDetail.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortSingle;
            this.grdDetail.DisplayLayout.Override.MergedCellStyle = Infragistics.Win.UltraWinGrid.MergedCellStyle.Never;
            appearance22.BorderColor = System.Drawing.Color.Gray;
            this.grdDetail.DisplayLayout.Override.RowAlternateAppearance = appearance22;
            appearance23.BackColor = System.Drawing.Color.White;
            appearance23.BackColor2 = System.Drawing.Color.White;
            appearance23.BackGradientStyle = Infragistics.Win.GradientStyle.None;
            appearance23.BackHatchStyle = Infragistics.Win.BackHatchStyle.None;
            appearance23.BorderColor = System.Drawing.Color.Gray;
            this.grdDetail.DisplayLayout.Override.RowAppearance = appearance23;
            appearance24.BackColor = System.Drawing.Color.White;
            appearance24.BackColor2 = System.Drawing.Color.White;
            appearance24.BorderColor = System.Drawing.Color.Gray;
            this.grdDetail.DisplayLayout.Override.RowPreviewAppearance = appearance24;
            appearance25.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(62)))), ((int)(((byte)(176)))));
            appearance25.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance25.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance25.BorderColor = System.Drawing.Color.Gray;
            appearance25.ForeColor = System.Drawing.Color.White;
            this.grdDetail.DisplayLayout.Override.RowSelectorAppearance = appearance25;
            this.grdDetail.DisplayLayout.Override.RowSelectorWidth = 12;
            this.grdDetail.DisplayLayout.Override.RowSizingArea = Infragistics.Win.UltraWinGrid.RowSizingArea.EntireRow;
            this.grdDetail.DisplayLayout.Override.RowSpacingBefore = 2;
            appearance26.BackColor = System.Drawing.Color.Navy;
            appearance26.BackColorAlpha = Infragistics.Win.Alpha.Opaque;
            this.grdDetail.DisplayLayout.Override.SelectedCellAppearance = appearance26;
            appearance27.BackColor = System.Drawing.Color.Navy;
            appearance27.BackColorDisabled = System.Drawing.Color.Navy;
            appearance27.BackGradientStyle = Infragistics.Win.GradientStyle.None;
            appearance27.BackHatchStyle = Infragistics.Win.BackHatchStyle.None;
            appearance27.BorderColor = System.Drawing.Color.Gray;
            appearance27.ForeColor = System.Drawing.Color.White;
            this.grdDetail.DisplayLayout.Override.SelectedRowAppearance = appearance27;
            this.grdDetail.DisplayLayout.Override.SelectTypeCell = Infragistics.Win.UltraWinGrid.SelectType.Single;
            this.grdDetail.DisplayLayout.Override.SelectTypeCol = Infragistics.Win.UltraWinGrid.SelectType.Single;
            this.grdDetail.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.Single;
            appearance28.BackColor = System.Drawing.Color.White;
            appearance28.BackColor2 = System.Drawing.Color.White;
            appearance28.BackColorAlpha = Infragistics.Win.Alpha.Opaque;
            appearance28.BackColorDisabled = System.Drawing.Color.White;
            appearance28.BackColorDisabled2 = System.Drawing.Color.White;
            appearance28.BorderColor = System.Drawing.Color.Gray;
            this.grdDetail.DisplayLayout.Override.TemplateAddRowAppearance = appearance28;
            this.grdDetail.DisplayLayout.RowConnectorColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            this.grdDetail.DisplayLayout.RowConnectorStyle = Infragistics.Win.UltraWinGrid.RowConnectorStyle.Solid;
            appearance29.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(231)))), ((int)(((byte)(231)))));
            appearance29.BackColor2 = System.Drawing.Color.White;
            appearance29.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance29.BorderAlpha = Infragistics.Win.Alpha.Transparent;
            appearance29.ThemedElementAlpha = Infragistics.Win.Alpha.Transparent;
            scrollBarLook1.ButtonAppearance = appearance29;
            this.grdDetail.DisplayLayout.ScrollBarLook = scrollBarLook1;
            this.grdDetail.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.grdDetail.Location = new System.Drawing.Point(14, 18);
            this.grdDetail.Name = "grdDetail";
            this.grdDetail.Size = new System.Drawing.Size(785, 201);
            this.grdDetail.TabIndex = 1;
            this.grdDetail.UpdateMode = Infragistics.Win.UltraWinGrid.UpdateMode.OnCellChangeOrLostFocus;
            this.grdDetail.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.grdDetail.AfterCellActivate += new System.EventHandler(this.grdDetail_AfterCellActivate);
            this.grdDetail.AfterCellUpdate += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.grdDetail_AfterCellUpdate);
            this.grdDetail.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.grdDetail_InitializeLayout);
            this.grdDetail.AfterRowsDeleted += new System.EventHandler(this.grdDetail_AfterRowsDeleted);
            this.grdDetail.AfterRowInsert += new Infragistics.Win.UltraWinGrid.RowEventHandler(this.grdDetail_AfterRowInsert);
            this.grdDetail.CellChange += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.grdDetail_CellChange);
            this.grdDetail.ClickCellButton += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.grdDetail_ClickCellButton);
            this.grdDetail.Error += new Infragistics.Win.UltraWinGrid.ErrorEventHandler(this.grdDetail_Error);
            this.grdDetail.CellDataError += new Infragistics.Win.UltraWinGrid.CellDataErrorEventHandler(this.grdDetail_CellDataError);
            this.grdDetail.Click += new System.EventHandler(this.grdDetail_Click);
            this.grdDetail.Enter += new System.EventHandler(this.grdDetail_Enter);
            this.grdDetail.Validated += new System.EventHandler(this.grdDetail_Validated);
            // 
            // ultraDataSource3
            // 
            this.ultraDataSource3.Band.Columns.AddRange(new object[] {
            ultraDataColumn8,
            ultraDataColumn9});
            // 
            // lblItemDetail
            // 
            this.lblItemDetail.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblItemDetail.ForeColor = System.Drawing.Color.Black;
            this.lblItemDetail.Location = new System.Drawing.Point(12, 22);
            this.lblItemDetail.Name = "lblItemDetail";
            this.lblItemDetail.Size = new System.Drawing.Size(595, 19);
            this.lblItemDetail.TabIndex = 6;
            this.lblItemDetail.Text = "Item Code";
            // 
            // groupBox4
            // 
            this.groupBox4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox4.Controls.Add(this.lblItemDetail);
            this.groupBox4.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.groupBox4.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox4.Location = new System.Drawing.Point(8, 53);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(808, 53);
            this.groupBox4.TabIndex = 28;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Item Details";
            // 
            // frmItemMonitorCategoryDetail
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(108)))), ((int)(((byte)(172)))));
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(828, 409);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.lblTransactionType);
            this.Controls.Add(this.groupBox3);
            this.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.Name = "frmItemMonitorCategoryDetail";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Item Monitor Category Detail";
            this.Activated += new System.EventHandler(this.frmItemMonitorCategoryDetail_Activated);
            this.Load += new System.EventHandler(this.frmItemMonitorCategoryDetail_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmItemMonitorCategoryDetail_KeyDown);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.frmItemMonitorCategoryDetail_KeyUp);
            ((System.ComponentModel.ISupportInitialize)(this.ultraDataSource2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraDataSource1)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdDetail)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraDataSource3)).EndInit();
            this.groupBox4.ResumeLayout(false);
            this.ResumeLayout(false);

        }
        #endregion


        private void ultraButton2_Click(object sender, System.EventArgs e)
        {
            this.Close();
        }

        //Added By shitaljit on 2 May 2012
        private void setDatachanges()
        {
            foreach (UltraGridRow oRow in grdDetail.Rows)
            {
                if (Convert.ToString(oRow.Cells[clsPOSDBConstants.ItemMonitorCategory_Fld_ID].Value) != "")
                {
                    oItemItemMonitorCategoryDetailRow.UnitsPerPackage = Convert.ToDecimal(oRow.Cells[clsPOSDBConstants.ItemMonitorCategoryDetail_Fld_UnitsPerPackage].Value.ToString());
                    oItemMonitorCategoryDetailData.AcceptChanges();
                }
            }
           
        }
        private bool Save()
        {
            logger.Trace("Save() - " + clsPOSDBConstants.Log_Entering);
            try
            {
                if (Validate() == false) return false;
                oItemMonitorCategoryDetail.Persist(oItemMonitorCategoryDetailData);
                return true;
            }
            catch (Exception exp)
            {
                logger.Fatal(exp, "Save()");
                clsUIHelper.ShowErrorMsg(exp.Message);
                return false;
            }
            finally
            {
                logger.Trace("Save() - " + clsPOSDBConstants.Log_Exiting);
            }
        }

        private bool Validate()
        {
            logger.Trace("Validate() - " + clsPOSDBConstants.Log_Entering);
            bool status = true;
            try
            {
                if (Configuration.CInfo.useNplex == true)
                {
                    if (oItemMonitorCategoryDetailData.Tables.Count > 0 && oItemMonitorCategoryDetailData.ItemMonitorCategoryDetail.Rows.Count > 0)
                    {
                        int rowCnt = 0;
                        foreach (ItemMonitorCategoryDetailRow oRow in oItemMonitorCategoryDetailData.ItemMonitorCategoryDetail.Rows)
                        {
                            if (Configuration.CInfo.useNplex == true)
                            {
                                if (oRow.ePSE == true && oRow.UnitsPerPackage <= 0.0M)
                                {
                                    Resources.Message.Display("Units should be greater than zero as nplex and ePSE settings are enabled.", "Item Monitor Category Detail", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    try
                                    {
                                        grdDetail.Rows[rowCnt].Cells[clsPOSDBConstants.ItemMonitorCategoryDetail_Fld_UnitsPerPackage].Activate();
                                        grdDetail.PerformAction(UltraGridAction.EnterEditMode, false, false);
                                    }
                                    catch { }
                                    status = false;
                                    break;
                                }
                            }
                            rowCnt++;
                        }
                    }
                }
                else
                {
                    status = true;
                }
            }
            catch (Exception exp)
            {
                logger.Fatal(exp, "Validate()");
                status = false;
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
            logger.Trace("Validate() - " + clsPOSDBConstants.Log_Exiting);
            return status;
        }

        private void Search()
        {
            oItemMonitorCategoryDetailData = oItemMonitorCategoryDetail.PopulateList(buildCriteria());
            grdDetail.DataSource = oItemMonitorCategoryDetailData;
            grdDetail.Refresh();
        }

        private string buildCriteria()
        {
            string sCriteria = "";
            if (txtCompanionItemCode.Text != "")
            {
                sCriteria = sCriteria + ((sCriteria == "") ? " WHERE " : " AND ");
                sCriteria = sCriteria + clsPOSDBConstants.ItemMonitorCategory_Fld_ItemMonCatID + " LIKE '" + txtCompanionItemCode.Text + "%'";
            }
            if (txtItemCode.Text != "")
            {
                sCriteria = sCriteria + ((sCriteria == "") ? " WHERE " : " AND ");
                sCriteria = sCriteria + " Companion." + clsPOSDBConstants.ItemMonitorCategory_Fld_ItemMonCatID + " LIKE '" + txtItemCode.Text + "%'";
            }

            return sCriteria;
        }

        private void btnSearch_Click(object sender, System.EventArgs e)
        {
            Search();
        }

        private void btnSave_Click(object sender, System.EventArgs e)
        {
            if (Save())
                this.Close();
        }

        private void grdDetail_AfterCellUpdate(object sender, Infragistics.Win.UltraWinGrid.CellEventArgs e)
        {
            ItemMonitorCategory oItemMonitorCategory = new ItemMonitorCategory();
            ItemMonitorCategoryData oItemMonitorCategoryData;
            ItemMonitorCategoryRow oItemMonitorCategoryRow;

            if (isCellUpdateCalled)
            {
                isCellUpdateCalled = false;
                return;
            }

            if (m_rowIndex == -1)
            {
                isCellUpdateCalled = false;
                return;
            }

            grdDetail.BeginUpdate();

            isCellUpdateCalled = false;
            if (e.Cell.Column.Index == grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.ItemMonitorCategory_Fld_ItemMonCatID].Index)
            {
                try
                {
                    oItemMonitorCategoryData = oItemMonitorCategory.Populate(mItemMonCatID);
                    oItemMonitorCategoryRow = (ItemMonitorCategoryRow)oItemMonitorCategoryData.ItemMonitorCategory.Rows[0];
                    grdDetail.Rows[m_rowIndex].Cells[clsPOSDBConstants.ItemMonitorCategory_Fld_Description].Value = oItemMonitorCategoryRow.Description;
                    int tempid = Configuration.convertNullToInt(grdDetail.Rows[m_rowIndex].Cells[clsPOSDBConstants.ItemMonitorCategory_Fld_ID].Value);
                    if (tempid == 0)
                    {
                        grdDetail.Rows[m_rowIndex].Cells[clsPOSDBConstants.ItemMonitorCategory_Fld_ID].Value = oItemMonitorCategoryDetailData.ItemMonitorCategoryDetail.GetNextID();
                        tempid = Configuration.convertNullToInt(grdDetail.Rows[m_rowIndex].Cells[clsPOSDBConstants.ItemMonitorCategory_Fld_ID].Value);
                    }
                    grdDetail.Rows[m_rowIndex].Cells[clsPOSDBConstants.Item_Fld_ItemID].Value = mItemId;
                    //Added by shitaljit on 2 May 2012
                    //grdDetail.Rows[grdDetail.ActiveRow.Index].Cells[clsPOSDBConstants.ItemMonitorCategory_Fld_DailyLimit].Value = oItemMonitorCategoryRow.DailyLimit;
                    //grdDetail.Rows[grdDetail.ActiveRow.Index].Cells[clsPOSDBConstants.ItemMonitorCategory_Fld_ThirtyDaysLimit].Value = oItemMonitorCategoryRow.ThirtyDaysLimit;
                    grdDetail.Rows[grdDetail.ActiveRow.Index].Cells[clsPOSDBConstants.ItemMonitorCategory_Fld_LimitPeriodDays].Value = oItemMonitorCategoryRow.LimitPeriodDays;
                    grdDetail.Rows[grdDetail.ActiveRow.Index].Cells[clsPOSDBConstants.ItemMonitorCategory_Fld_LimitPeriodQty].Value = oItemMonitorCategoryRow.LimitPeriodQty;
                    grdDetail.Rows[grdDetail.ActiveRow.Index].Cells[clsPOSDBConstants.ItemMonitorCategoryDetail_Fld_UnitsPerPackage].Value = "0.00";
                }
                catch (Exception exp)
                {
                    logger.Fatal(exp, "grdDetail_AfterCellUpdate(object sender, Infragistics.Win.UltraWinGrid.CellEventArgs e)");
                    isCellUpdateCalled = true;

                    grdDetail.AfterCellUpdate -= new CellEventHandler(grdDetail_AfterCellUpdate);
                    grdDetail.Rows[m_rowIndex].Cells[clsPOSDBConstants.ItemMonitorCategory_Fld_Description].Value = "";
                    grdDetail.Rows[m_rowIndex].Cells[clsPOSDBConstants.ItemMonitorCategory_Fld_ID].Value = oItemMonitorCategoryDetailData.ItemMonitorCategoryDetail.GetNextID();
                    grdDetail.Rows[m_rowIndex].Cells[clsPOSDBConstants.Item_Fld_ItemID].Value = "";

                    isCellUpdateCalled = true;
                    grdDetail.AfterCellUpdate += new CellEventHandler(grdDetail_AfterCellUpdate);
                    grdDetail.Focus();
                }
            }

            grdDetail.EndUpdate();
        }

        private void grdDetail_AfterRowInsert(object sender, Infragistics.Win.UltraWinGrid.RowEventArgs e)
        {
            try
            {
                if (grdDetail.Rows.Count != 0)
                {
                    m_rowIndex = grdDetail.Rows.Count - 1;
                }
                else
                {
                    m_rowIndex = -1;

                    return;
                }

                if (oItemMonitorCategoryDetailData.Tables[0].Rows.Count > m_rowIndex)
                {
                    if (oItemMonitorCategoryDetailData.Tables[0].Rows[m_rowIndex].RowState != DataRowState.Deleted)
                    {
                        ItemMonitorCategoryDetailRow oRow = (ItemMonitorCategoryDetailRow)oItemMonitorCategoryDetailData.Tables[0].Rows[m_rowIndex];
                        oRow.ItemID = mItemId;
                    }
                    grdDetail.Update();
                }
            }
            catch (Exception Exp)
            {
                logger.Fatal(Exp, "grdDetail_AfterRowInsert(object sender, Infragistics.Win.UltraWinGrid.RowEventArgs e)");
                clsUIHelper.ShowErrorMsg(Exp.Message);
            }
        }

        private void grdDetail_BeforeRowInsert(object sender, Infragistics.Win.UltraWinGrid.BeforeRowInsertEventArgs e)
        {

        }

        private void grdDetail_CellChange(object sender, Infragistics.Win.UltraWinGrid.CellEventArgs e)
        {
            try
            {
                if (grdDetail.ActiveRow == null)
                {
                    if (grdDetail.Rows.Count != 0)
                        m_rowIndex = grdDetail.Rows.Count;
                    else
                        m_rowIndex = -1;

                    return;
                }
                if (grdDetail.ActiveCell.Column.Index == grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.ItemMonitorCategory_Fld_ItemMonCatID].Index)
                    mItemMonCatID = POS_Core.Resources.Configuration.convertNullToInt(grdDetail.ActiveCell.Text);

                m_rowIndex = grdDetail.ActiveRow.Index;
                m_ColIndex = grdDetail.ActiveCell.Column.Index;
            }
            catch (POSExceptions exp)
            {
                logger.Fatal(exp, "grdDetail_CellChange(object sender, Infragistics.Win.UltraWinGrid.CellEventArgs e)");
                clsUIHelper.ShowErrorMsg(exp.ErrMessage);
            }
        }

        private void grdDetail_ClickCellButton(object sender, Infragistics.Win.UltraWinGrid.CellEventArgs e)
        {
            try
            {
                if (e.Cell.Row.IsAddRow == false)
                {

                    if (e.Cell.Column.Key == "Delete")
                    {
                        e.Cell.Row.Delete(false);
                    }
                    //Added By shitaljit on 30 April 2012
                    else if (e.Cell.Column.Key == "ItemMonCatID")
                    {
                        SearchItem(e.Cell.Row.IsAddRow); //PRIMEPOS-3166N
                    }
                    else if (e.Cell.Column.Key != "UnitsPerPackage")
                    {
                        e.Cell.Row.Selected = true;
                    }

                }
                else
                {
                    if (grdDetail.ActiveCell.Column.Index == grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.ItemMonitorCategory_Fld_ItemMonCatID].Index)
                    {
                        SearchItem(e.Cell.Row.IsAddRow); //PRIMEPOS-3166N
                    }
                }
            }
            catch (POSExceptions exp)
            {
                logger.Fatal(exp, "grdDetail_ClickCellButton(object sender, Infragistics.Win.UltraWinGrid.CellEventArgs e)");
                clsUIHelper.ShowErrorMsg(exp.ErrMessage);
            }
        }

        private void SearchItem(bool isNewCategory) //PRIMEPOS-3166
        {
            ItemMonitorCategory oItemMonitorCategory = new ItemMonitorCategory();
            ItemMonitorCategoryData oItemMonitorCategoryData;
            ItemMonitorCategoryRow oItemMonitorCategoryRow;

            try
            {
                //frmSearch oSearch = new frmSearch(clsPOSDBConstants.ItemMonitorCategory_tbl);
                frmSearchMain oSearch = new frmSearchMain(true);    //20-Dec-2017 JY Added new reference
                oSearch.SearchTable = clsPOSDBConstants.ItemMonitorCategory_tbl;    //20-Dec-2017 JY Added
                oSearch.SearchInConstructor = true;
                oSearch.ShowDialog(this);
                if (oSearch.IsCanceled) return;

                oItemMonitorCategoryData = oItemMonitorCategory.Populate(POS_Core.Resources.Configuration.convertNullToInt(oSearch.SelectedRowID()));
                oItemMonitorCategoryRow = (ItemMonitorCategoryRow)oItemMonitorCategoryData.ItemMonitorCategory.Rows[0];
                mItemMonCatID = oItemMonitorCategoryRow.ID;
                grdDetail.Rows[grdDetail.ActiveRow.Index].Cells[clsPOSDBConstants.ItemMonitorCategory_Fld_ItemMonCatID].Value = oItemMonitorCategoryRow.ID;
                grdDetail.Rows[grdDetail.ActiveRow.Index].Cells[clsPOSDBConstants.ItemMonitorCategory_Fld_Description].Value = oItemMonitorCategoryRow.Description;
                if (isNewCategory)  //PRIMEPOS-3166
                {
                    grdDetail.Rows[grdDetail.ActiveRow.Index].Cells[clsPOSDBConstants.ItemMonitorCategory_Fld_ID].Value = oItemMonitorCategoryDetailData.ItemMonitorCategoryDetail.GetNextID();
                }
                grdDetail.Rows[grdDetail.ActiveRow.Index].Cells[clsPOSDBConstants.Item_Fld_ItemID].Value = mItemId;
                //Added by shitaljit on 2 May 2012
                //grdDetail.Rows[grdDetail.ActiveRow.Index].Cells[clsPOSDBConstants.ItemMonitorCategory_Fld_DailyLimit].Value = oItemMonitorCategoryRow.DailyLimit;
                //grdDetail.Rows[grdDetail.ActiveRow.Index].Cells[clsPOSDBConstants.ItemMonitorCategory_Fld_ThirtyDaysLimit].Value = oItemMonitorCategoryRow.ThirtyDaysLimit;
                grdDetail.Rows[grdDetail.ActiveRow.Index].Cells[clsPOSDBConstants.ItemMonitorCategory_Fld_LimitPeriodDays].Value = oItemMonitorCategoryRow.LimitPeriodDays;
                grdDetail.Rows[grdDetail.ActiveRow.Index].Cells[clsPOSDBConstants.ItemMonitorCategory_Fld_LimitPeriodQty].Value = oItemMonitorCategoryRow.LimitPeriodQty;
                grdDetail.Rows[grdDetail.ActiveRow.Index].Cells[clsPOSDBConstants.ItemMonitorCategory_Fld_ePSE].Value = oItemMonitorCategoryRow.ePSE;   //Sprint-23 - PRIMEPOS-2029 31-Mar-2016 JY Added
                //grdDetail.Rows[grdDetail.ActiveRow.Index].Cells[clsPOSDBConstants.ItemMonitorCategory_Fld_canOverrideMonitorItem].Value = oItemMonitorCategoryRow.canOverrideMonitorItem; //PRIMEPOS-3166
                isCellUpdateCalled = true;
            }
            catch (Exception exp)
            {
                logger.Fatal(exp, "SearchItem()");
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }

        private void frmItemMonitorCategoryDetail_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {

            try
            {
                if (e.KeyData == Keys.Enter)
                {
                    if (this.grdDetail.ContainsFocus == true && this.grdDetail.ActiveCell.Text.Trim() == "" && this.grdDetail.ActiveCell.Column.Key == clsPOSDBConstants.ItemMonitorCategory_Fld_ItemMonCatID && this.grdDetail.ActiveCell.Row.IsAddRow == true)
                    {
                        this.SelectNextControl(this.grdDetail, true, true, true, true);
                        e.Handled = true;
                    }
                    else if (this.grdDetail.ContainsFocus == false)
                    {
                        this.SelectNextControl(this.ActiveControl, true, true, true, true);
                    }
                }

            }
            catch (Exception exp)
            {
                logger.Fatal(exp, "frmItemMonitorCategoryDetail_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)");
                clsUIHelper.ShowErrorMsg(exp.Message);
            }

        }

        private void frmItemMonitorCategoryDetail_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            try
            {
                if (this.grdDetail.ContainsFocus == true)
                {
                    if (this.grdDetail.ActiveCell != null)
                    {
                        if (this.grdDetail.ActiveCell.Column.Key == clsPOSDBConstants.ItemMonitorCategory_Fld_ItemMonCatID)
                            if (e.KeyData == Keys.F4)
                            {
                                if (this.grdDetail.ActiveCell.Row.IsAddRow == true)
                                {
                                    this.SearchItem(true); //PRIMEPOS-3166N
                                }
                                else
                                {
                                    this.SearchItem(false); //PRIMEPOS-3166N
                                }
                                e.Handled = true;
                            }
                    }
                }
            }
            catch (POSExceptions exp)
            {
                logger.Fatal(exp, "frmItemMonitorCategoryDetail_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)");
                clsUIHelper.ShowErrorMsg(exp.ErrMessage);
            }
        }

        private void frmItemMonitorCategoryDetail_Load(object sender, System.EventArgs e)
        {
            this.grdDetail.AfterEnterEditMode += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
            this.grdDetail.AfterExitEditMode += new System.EventHandler(clsUIHelper.AfterExitEditMode);

            clsUIHelper.SetAppearance(this.grdDetail);
            clsUIHelper.SetKeyActionMappings(this.grdDetail);
            clsUIHelper.setColorSchecme(this);
        }

        private void grdDetail_Enter(object sender, System.EventArgs e)
        {

            if (this.grdDetail.Rows.Count > 0)
            {
                if (!m_FromError)
                {
                    this.grdDetail.ActiveCell = this.grdDetail.Rows[0].Cells[clsPOSDBConstants.ItemMonitorCategory_Fld_ItemMonCatID];
                }
                this.grdDetail.PerformAction(UltraGridAction.EnterEditMode);
            }
            else
            {
                this.grdDetail.Rows.Band.AddNew();
            }
            m_FromError = false;
        }

        private void grdDetail_Error(object sender, Infragistics.Win.UltraWinGrid.ErrorEventArgs e)
        {
            m_FromError = true;
            CommonUI.checkGridError((Infragistics.Win.UltraWinGrid.UltraGrid)sender, e, clsPOSDBConstants.ItemMonitorCategory_Fld_ItemMonCatID);
        }

        private string getColumnName(String ErrorText)
        {
            int StartIndex = ErrorText.IndexOf("Column '");
            int EndIndex = ErrorText.IndexOf("'", StartIndex + 8);

            string columnName = ErrorText.Substring(StartIndex + 8, EndIndex - StartIndex - 8);
            return columnName;
        }

        private void grdDetail_CellDataError(object sender, Infragistics.Win.UltraWinGrid.CellDataErrorEventArgs e)
        {
            e.RaiseErrorEvent = true;
        }

        private void grdDetail_AfterCellActivate(object sender, System.EventArgs e)
        {
            //this.grdDetail.PerformAction(UltraGridAction.EnterEditMode);
        }

        private void grdDetail_Validated(object sender, System.EventArgs e)
        {
            grdDetail.PerformAction(UltraGridAction.LastCellInGrid);
        }

        private void frmItemMonitorCategoryDetail_Activated(object sender, System.EventArgs e)
        {
            clsUIHelper.CurrentForm = this;
        }

        private void grdDetail_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            DataSet ds = SearchData(" Detail." + clsPOSDBConstants.Item_Fld_ItemID + " = '" + mItemId + "'"); //Added By shitaljit on 2 May 2012
            m_rowIndex = 0;
            foreach (DataRow oRow in ds.Tables[0].Rows)
            {
                //grdDetail.Rows[m_rowIndex].Cells[clsPOSDBConstants.ItemMonitorCategory_Fld_DailyLimit].Value = oRow.ItemArray[3]; //Sprint-23 - PRIMEPOS-2029 30-Mar-2016 JY Commented
                //grdDetail.Rows[m_rowIndex].Cells[clsPOSDBConstants.ItemMonitorCategory_Fld_ThirtyDaysLimit].Value = oRow.ItemArray[4];    //Sprint-23 - PRIMEPOS-2029 30-Mar-2016 JY Commented
                grdDetail.Rows[m_rowIndex].Cells[clsPOSDBConstants.ItemMonitorCategory_Fld_LimitPeriodDays].Value = oRow.ItemArray[4];
                grdDetail.Rows[m_rowIndex].Cells[clsPOSDBConstants.ItemMonitorCategory_Fld_LimitPeriodQty].Value = oRow.ItemArray[5];
                m_rowIndex++;
            }
            m_rowIndex = -1;
        }

        private void grdDetail_AfterRowsDeleted(object sender, EventArgs e)
        {
            if (this.grdDetail.Rows.Count > 0)
            {
                this.grdDetail.ActiveRow = this.grdDetail.Rows[0];
                this.grdDetail.ActiveCell = this.grdDetail.ActiveRow.Cells[clsPOSDBConstants.ItemMonitorCategory_Fld_ItemMonCatID];
                this.grdDetail.PerformAction(UltraGridAction.EnterEditMode);
            }         
            else
            {
                this.grdDetail.DisplayLayout.Bands[0].AddNew();
            }
        }

        private void grdDetail_Click(object sender, EventArgs e)
        {
            if (grdDetail.Rows.Count == 0)
            {
                return;
            }
            else if (this.grdDetail.ActiveCell != this.grdDetail.ActiveRow.Cells[clsPOSDBConstants.ItemMonitorCategory_Fld_ItemMonCatID] || 
                this.grdDetail.ActiveCell != this.grdDetail.ActiveRow.Cells[clsPOSDBConstants.ItemMonitorCategoryDetail_Fld_UnitsPerPackage])
            {
                this.grdDetail.DisplayLayout.Override.CellClickAction = CellClickAction.RowSelect;
            }
        }
    }
}
