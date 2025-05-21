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
//using POS_Core.DataAccess;
using Infragistics.Win.UltraWinGrid;
using System.Data;
using POS_Core.Resources;
using System.Timers;
using POS_Core_UI.Reports.ReportsUI;

namespace POS_Core_UI
{
    /// <summary>
    /// Summary description for frmPhysicalInv.
    /// </summary>
    public class frmPhysicalInv : System.Windows.Forms.Form
    {
        #region variable declaration
        private PhysicalInvData oPhysicalInvData = new PhysicalInvData();
        private PhysicalInvRow oPhysicalInvRow;
        private PhysicalInv oPhysicalInv = new PhysicalInv();
        private System.Int32 CurrentID = 0;
        private int LastRow;
        private Infragistics.Win.UltraWinDataSource.UltraDataSource ultraDataSource2;
        private Infragistics.Win.Misc.UltraLabel lblTransactionType;
        private Infragistics.Win.UltraWinGrid.UltraGrid grdHistory;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox4;
        private Infragistics.Win.Misc.UltraButton btnProcess;
        private Infragistics.Win.Misc.UltraButton btnClose;
        private Infragistics.Win.UltraWinDataSource.UltraDataSource ultraDataSource1;
        private Infragistics.Win.Misc.UltraButton btnDeleteRow;
        private IContainer components;
        private Infragistics.Win.Misc.UltraPanel pnlProcess;
        private Infragistics.Win.Misc.UltraLabel lblProcess;
        private Infragistics.Win.Misc.UltraPanel pnlClose;
        private Infragistics.Win.Misc.UltraLabel lblClose;
        private Infragistics.Win.Misc.UltraPanel pnlDeleteRow;
        private Infragistics.Win.Misc.UltraLabel lblDeleteRow;
        private Infragistics.Win.UltraWinTabControl.UltraTabControl ultraTabPhyInv;
        private Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage ultraTabSharedControlsPage1;
        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl1;
        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl2;
        private Infragistics.Win.UltraWinGrid.UltraGrid grdPhysicalInventory;
        private Infragistics.Win.UltraWinEditors.UltraDateTimeEditor ultraDateTimeEditor1;

        //Added by SRT(Abhishek) Date : 09/10/2009
        private Infragistics.Win.Misc.UltraPanel pnlClear;
        private Infragistics.Win.Misc.UltraButton btnClear;
        private Infragistics.Win.Misc.UltraLabel lblClear;
        private Infragistics.Win.Misc.UltraPanel pnlAddItem;
        public Infragistics.Win.Misc.UltraButton btnAddItem;
        private Infragistics.Win.Misc.UltraLabel lblAddItem;
        private Infragistics.Win.Misc.UltraPanel pnlDelete;
        private Infragistics.Win.Misc.UltraButton btnDelete;
        private Infragistics.Win.Misc.UltraLabel lblDelete;
        private Infragistics.Win.Misc.UltraPanel pnlSave;
        private Infragistics.Win.Misc.UltraButton btnSave;
        private Infragistics.Win.Misc.UltraLabel lblSave;

        //End Of Added by SRT(Abhishek) Date : 09/10/2009
        private bool m_exceptionAccoured = false;   //PRIMEPOS-2395 13-Jun-2018 JY Added
        private Infragistics.Win.UltraWinEditors.UltraDateTimeEditor ultraDateTimeEditor2;
        bool bException = false;    //PRIMEPOS-2395 13-Jun-2018 JY Added
        public Infragistics.Win.Misc.UltraLabel lblMessage;
        private static int Cnt = 0;

        #region PRIMEPOS-2464 10-Mar-2020 JY Added
        int nDisplayItemCost = 0;
        System.Timers.Timer tmrBlinking;
        private Infragistics.Win.Misc.UltraPanel pnlPrintShelfStickers;
        private Infragistics.Win.Misc.UltraButton btnPrintShelfStickers;
        private Infragistics.Win.Misc.UltraLabel lblPrintShelfStickers;
        private long iBlinkCnt = 0;
        #endregion
        #endregion

        public frmPhysicalInv()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();

            //
            // TODO: Add any constructor code after InitializeComponent call
            //
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
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance7 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance8 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance9 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand1 = new Infragistics.Win.UltraWinGrid.UltraGridBand("Band 0", -1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn12 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("ID");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn52 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("ItemCode");
            Infragistics.Win.Appearance appearance10 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn53 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Description");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn54 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("OldQty");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn5 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("NewQty");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn13 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("isProcessed");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn14 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("UserID");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn15 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("TransDate");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn28 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("PUserID");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn29 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("PTransDate");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn4 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("LastInvUpdatedQty", 0);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn6 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("OldExpDate", 1);
            Infragistics.Win.Appearance appearance11 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn7 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("NewExpDate", 2);
            Infragistics.Win.Appearance appearance12 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn8 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("OldSellingPrice", 3);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn9 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("NewSellingPrice", 4);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn10 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("OldCostPrice", 5);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn11 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("NewCostPrice", 6);
            Infragistics.Win.Appearance appearance13 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance14 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance15 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance16 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance17 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance18 = new Infragistics.Win.Appearance();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmPhysicalInv));
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
            Infragistics.Win.Appearance appearance29 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance30 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance31 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinScrollBar.ScrollBarLook scrollBarLook1 = new Infragistics.Win.UltraWinScrollBar.ScrollBarLook();
            Infragistics.Win.Appearance appearance32 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinDataSource.UltraDataColumn ultraDataColumn1 = new Infragistics.Win.UltraWinDataSource.UltraDataColumn("ID");
            Infragistics.Win.UltraWinDataSource.UltraDataColumn ultraDataColumn2 = new Infragistics.Win.UltraWinDataSource.UltraDataColumn("ItemCode");
            Infragistics.Win.UltraWinDataSource.UltraDataColumn ultraDataColumn3 = new Infragistics.Win.UltraWinDataSource.UltraDataColumn("Description");
            Infragistics.Win.UltraWinDataSource.UltraDataColumn ultraDataColumn4 = new Infragistics.Win.UltraWinDataSource.UltraDataColumn("OldQty");
            Infragistics.Win.UltraWinDataSource.UltraDataColumn ultraDataColumn5 = new Infragistics.Win.UltraWinDataSource.UltraDataColumn("NewQty");
            Infragistics.Win.UltraWinDataSource.UltraDataColumn ultraDataColumn6 = new Infragistics.Win.UltraWinDataSource.UltraDataColumn("isProcessed");
            Infragistics.Win.UltraWinDataSource.UltraDataColumn ultraDataColumn7 = new Infragistics.Win.UltraWinDataSource.UltraDataColumn("UserID");
            Infragistics.Win.UltraWinDataSource.UltraDataColumn ultraDataColumn8 = new Infragistics.Win.UltraWinDataSource.UltraDataColumn("TransDate");
            Infragistics.Win.UltraWinDataSource.UltraDataColumn ultraDataColumn9 = new Infragistics.Win.UltraWinDataSource.UltraDataColumn("PUserID");
            Infragistics.Win.UltraWinDataSource.UltraDataColumn ultraDataColumn10 = new Infragistics.Win.UltraWinDataSource.UltraDataColumn("PTransDate");
            Infragistics.Win.Appearance appearance33 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance34 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance35 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance36 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance37 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand2 = new Infragistics.Win.UltraWinGrid.UltraGridBand("Band 0", -1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn32 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("ID");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn33 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("ItemCode");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn34 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Description");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn35 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("OldQty");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn36 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("NewQty");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn37 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("isProcessed");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn38 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("UserID");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn39 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("TransDate");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn40 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("PUserID");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn41 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("PTransDate");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn26 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("OldExpDate", 0);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn27 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("NewExpDate", 1);
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
            Infragistics.Win.Appearance appearance48 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance49 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance50 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance51 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance52 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance53 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance54 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance55 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance56 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinScrollBar.ScrollBarLook scrollBarLook2 = new Infragistics.Win.UltraWinScrollBar.ScrollBarLook();
            Infragistics.Win.Appearance appearance57 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance58 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance59 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance60 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance61 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance62 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance63 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinDataSource.UltraDataColumn ultraDataColumn11 = new Infragistics.Win.UltraWinDataSource.UltraDataColumn("Amount");
            Infragistics.Win.UltraWinDataSource.UltraDataColumn ultraDataColumn12 = new Infragistics.Win.UltraWinDataSource.UltraDataColumn("Reference");
            Infragistics.Win.Appearance appearance64 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance65 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab1 = new Infragistics.Win.UltraWinTabControl.UltraTab();
            Infragistics.Win.Appearance appearance66 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab2 = new Infragistics.Win.UltraWinTabControl.UltraTab();
            Infragistics.Win.Appearance appearance67 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance68 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance69 = new Infragistics.Win.Appearance();
            this.ultraDateTimeEditor2 = new Infragistics.Win.UltraWinEditors.UltraDateTimeEditor();
            this.ultraDateTimeEditor1 = new Infragistics.Win.UltraWinEditors.UltraDateTimeEditor();
            this.ultraTabPageControl1 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.pnlClear = new Infragistics.Win.Misc.UltraPanel();
            this.btnClear = new Infragistics.Win.Misc.UltraButton();
            this.lblClear = new Infragistics.Win.Misc.UltraLabel();
            this.pnlAddItem = new Infragistics.Win.Misc.UltraPanel();
            this.btnAddItem = new Infragistics.Win.Misc.UltraButton();
            this.lblAddItem = new Infragistics.Win.Misc.UltraLabel();
            this.pnlDelete = new Infragistics.Win.Misc.UltraPanel();
            this.btnDelete = new Infragistics.Win.Misc.UltraButton();
            this.lblDelete = new Infragistics.Win.Misc.UltraLabel();
            this.pnlSave = new Infragistics.Win.Misc.UltraPanel();
            this.btnSave = new Infragistics.Win.Misc.UltraButton();
            this.lblSave = new Infragistics.Win.Misc.UltraLabel();
            this.grdPhysicalInventory = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.ultraDataSource1 = new Infragistics.Win.UltraWinDataSource.UltraDataSource(this.components);
            this.ultraTabPageControl2 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.pnlDeleteRow = new Infragistics.Win.Misc.UltraPanel();
            this.btnDeleteRow = new Infragistics.Win.Misc.UltraButton();
            this.lblDeleteRow = new Infragistics.Win.Misc.UltraLabel();
            this.pnlProcess = new Infragistics.Win.Misc.UltraPanel();
            this.btnProcess = new Infragistics.Win.Misc.UltraButton();
            this.lblProcess = new Infragistics.Win.Misc.UltraLabel();
            this.grdHistory = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.pnlClose = new Infragistics.Win.Misc.UltraPanel();
            this.btnClose = new Infragistics.Win.Misc.UltraButton();
            this.lblClose = new Infragistics.Win.Misc.UltraLabel();
            this.ultraDataSource2 = new Infragistics.Win.UltraWinDataSource.UltraDataSource(this.components);
            this.lblTransactionType = new Infragistics.Win.Misc.UltraLabel();
            this.ultraTabPhyInv = new Infragistics.Win.UltraWinTabControl.UltraTabControl();
            this.ultraTabSharedControlsPage1 = new Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage();
            this.lblMessage = new Infragistics.Win.Misc.UltraLabel();
            this.pnlPrintShelfStickers = new Infragistics.Win.Misc.UltraPanel();
            this.btnPrintShelfStickers = new Infragistics.Win.Misc.UltraButton();
            this.lblPrintShelfStickers = new Infragistics.Win.Misc.UltraLabel();
            ((System.ComponentModel.ISupportInitialize)(this.ultraDateTimeEditor2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraDateTimeEditor1)).BeginInit();
            this.ultraTabPageControl1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.pnlClear.ClientArea.SuspendLayout();
            this.pnlClear.SuspendLayout();
            this.pnlAddItem.ClientArea.SuspendLayout();
            this.pnlAddItem.SuspendLayout();
            this.pnlDelete.ClientArea.SuspendLayout();
            this.pnlDelete.SuspendLayout();
            this.pnlSave.ClientArea.SuspendLayout();
            this.pnlSave.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdPhysicalInventory)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraDataSource1)).BeginInit();
            this.ultraTabPageControl2.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.pnlDeleteRow.ClientArea.SuspendLayout();
            this.pnlDeleteRow.SuspendLayout();
            this.pnlProcess.ClientArea.SuspendLayout();
            this.pnlProcess.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdHistory)).BeginInit();
            this.pnlClose.ClientArea.SuspendLayout();
            this.pnlClose.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraDataSource2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraTabPhyInv)).BeginInit();
            this.ultraTabPhyInv.SuspendLayout();
            this.pnlPrintShelfStickers.ClientArea.SuspendLayout();
            this.pnlPrintShelfStickers.SuspendLayout();
            this.SuspendLayout();
            // 
            // ultraDateTimeEditor2
            // 
            this.ultraDateTimeEditor2.AlwaysInEditMode = true;
            this.ultraDateTimeEditor2.AutoSize = false;
            this.ultraDateTimeEditor2.DateTime = new System.DateTime(2015, 3, 9, 0, 0, 0, 0);
            this.ultraDateTimeEditor2.Location = new System.Drawing.Point(164, 160);
            this.ultraDateTimeEditor2.MaskInput = "{LOC}mm/dd/yyyy";
            this.ultraDateTimeEditor2.MaxDate = new System.DateTime(2050, 12, 31, 0, 0, 0, 0);
            this.ultraDateTimeEditor2.Name = "ultraDateTimeEditor2";
            this.ultraDateTimeEditor2.Size = new System.Drawing.Size(130, 25);
            this.ultraDateTimeEditor2.TabIndex = 43;
            this.ultraDateTimeEditor2.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.ultraDateTimeEditor2.Value = new System.DateTime(2015, 3, 9, 0, 0, 0, 0);
            this.ultraDateTimeEditor2.Visible = false;
            // 
            // ultraDateTimeEditor1
            // 
            this.ultraDateTimeEditor1.AlwaysInEditMode = true;
            this.ultraDateTimeEditor1.AutoSize = false;
            this.ultraDateTimeEditor1.DateTime = new System.DateTime(2015, 3, 9, 0, 0, 0, 0);
            this.ultraDateTimeEditor1.Location = new System.Drawing.Point(28, 160);
            this.ultraDateTimeEditor1.MaskInput = "{LOC}mm/dd/yyyy";
            this.ultraDateTimeEditor1.MaxDate = new System.DateTime(2050, 12, 31, 0, 0, 0, 0);
            this.ultraDateTimeEditor1.Name = "ultraDateTimeEditor1";
            this.ultraDateTimeEditor1.Size = new System.Drawing.Size(130, 25);
            this.ultraDateTimeEditor1.TabIndex = 42;
            this.ultraDateTimeEditor1.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.ultraDateTimeEditor1.Value = new System.DateTime(2015, 3, 9, 0, 0, 0, 0);
            this.ultraDateTimeEditor1.Visible = false;
            // 
            // ultraTabPageControl1
            // 
            this.ultraTabPageControl1.Controls.Add(this.ultraDateTimeEditor2);
            this.ultraTabPageControl1.Controls.Add(this.ultraDateTimeEditor1);
            this.ultraTabPageControl1.Controls.Add(this.groupBox2);
            this.ultraTabPageControl1.Controls.Add(this.grdPhysicalInventory);
            this.ultraTabPageControl1.Location = new System.Drawing.Point(-10000, -10000);
            this.ultraTabPageControl1.Name = "ultraTabPageControl1";
            this.ultraTabPageControl1.Size = new System.Drawing.Size(1041, 439);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.pnlClear);
            this.groupBox2.Controls.Add(this.pnlAddItem);
            this.groupBox2.Controls.Add(this.pnlDelete);
            this.groupBox2.Controls.Add(this.pnlSave);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.groupBox2.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.groupBox2.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.Location = new System.Drawing.Point(0, 387);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(1041, 52);
            this.groupBox2.TabIndex = 28;
            this.groupBox2.TabStop = false;
            // 
            // pnlClear
            // 
            // 
            // pnlClear.ClientArea
            // 
            this.pnlClear.ClientArea.Controls.Add(this.btnClear);
            this.pnlClear.ClientArea.Controls.Add(this.lblClear);
            this.pnlClear.Location = new System.Drawing.Point(415, 17);
            this.pnlClear.Name = "pnlClear";
            this.pnlClear.Size = new System.Drawing.Size(135, 30);
            this.pnlClear.TabIndex = 4;
            // 
            // btnClear
            // 
            appearance1.BackColor = System.Drawing.Color.White;
            appearance1.FontData.BoldAsString = "True";
            appearance1.ForeColor = System.Drawing.Color.Black;
            this.btnClear.Appearance = appearance1;
            this.btnClear.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Flat;
            this.btnClear.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnClear.Location = new System.Drawing.Point(65, 0);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(70, 30);
            this.btnClear.TabIndex = 5;
            this.btnClear.Text = "Clear";
            this.btnClear.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // lblClear
            // 
            appearance2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(138)))), ((int)(((byte)(31)))));
            appearance2.FontData.BoldAsString = "True";
            appearance2.ForeColor = System.Drawing.Color.White;
            appearance2.TextHAlignAsString = "Center";
            appearance2.TextVAlignAsString = "Middle";
            this.lblClear.Appearance = appearance2;
            this.lblClear.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblClear.Location = new System.Drawing.Point(0, 0);
            this.lblClear.Margin = new System.Windows.Forms.Padding(3, 3, 0, 3);
            this.lblClear.Name = "lblClear";
            this.lblClear.Size = new System.Drawing.Size(65, 30);
            this.lblClear.TabIndex = 0;
            this.lblClear.Tag = "NOCOLOR";
            this.lblClear.Text = "Alt + L";
            this.lblClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // pnlAddItem
            // 
            // 
            // pnlAddItem.ClientArea
            // 
            this.pnlAddItem.ClientArea.Controls.Add(this.btnAddItem);
            this.pnlAddItem.ClientArea.Controls.Add(this.lblAddItem);
            this.pnlAddItem.Location = new System.Drawing.Point(274, 17);
            this.pnlAddItem.Name = "pnlAddItem";
            this.pnlAddItem.Size = new System.Drawing.Size(130, 30);
            this.pnlAddItem.TabIndex = 6;
            // 
            // btnAddItem
            // 
            appearance3.BackColor = System.Drawing.Color.White;
            appearance3.FontData.BoldAsString = "True";
            appearance3.ForeColor = System.Drawing.Color.Black;
            this.btnAddItem.Appearance = appearance3;
            this.btnAddItem.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Flat;
            this.btnAddItem.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnAddItem.Location = new System.Drawing.Point(30, 0);
            this.btnAddItem.Name = "btnAddItem";
            this.btnAddItem.Size = new System.Drawing.Size(100, 30);
            this.btnAddItem.TabIndex = 7;
            this.btnAddItem.TabStop = false;
            this.btnAddItem.Text = "Add Items";
            this.btnAddItem.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnAddItem.Click += new System.EventHandler(this.btnAddItem_Click);
            // 
            // lblAddItem
            // 
            appearance4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(138)))), ((int)(((byte)(31)))));
            appearance4.ForeColor = System.Drawing.Color.White;
            appearance4.TextHAlignAsString = "Center";
            appearance4.TextVAlignAsString = "Middle";
            this.lblAddItem.Appearance = appearance4;
            this.lblAddItem.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblAddItem.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Bold);
            this.lblAddItem.Location = new System.Drawing.Point(0, 0);
            this.lblAddItem.Margin = new System.Windows.Forms.Padding(3, 3, 0, 3);
            this.lblAddItem.Name = "lblAddItem";
            this.lblAddItem.Size = new System.Drawing.Size(30, 30);
            this.lblAddItem.TabIndex = 0;
            this.lblAddItem.Tag = "NOCOLOR";
            this.lblAddItem.Text = "F2";
            this.lblAddItem.Click += new System.EventHandler(this.btnAddItem_Click);
            // 
            // pnlDelete
            // 
            // 
            // pnlDelete.ClientArea
            // 
            this.pnlDelete.ClientArea.Controls.Add(this.btnDelete);
            this.pnlDelete.ClientArea.Controls.Add(this.lblDelete);
            this.pnlDelete.Location = new System.Drawing.Point(561, 17);
            this.pnlDelete.Name = "pnlDelete";
            this.pnlDelete.Size = new System.Drawing.Size(175, 30);
            this.pnlDelete.TabIndex = 8;
            // 
            // btnDelete
            // 
            appearance5.BackColor = System.Drawing.Color.White;
            appearance5.FontData.BoldAsString = "True";
            appearance5.ForeColor = System.Drawing.Color.Black;
            this.btnDelete.Appearance = appearance5;
            this.btnDelete.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Flat;
            this.btnDelete.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnDelete.Location = new System.Drawing.Point(65, 0);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(110, 30);
            this.btnDelete.TabIndex = 9;
            this.btnDelete.Text = "Delete Row";
            this.btnDelete.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // lblDelete
            // 
            appearance6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(138)))), ((int)(((byte)(31)))));
            appearance6.FontData.BoldAsString = "True";
            appearance6.ForeColor = System.Drawing.Color.White;
            appearance6.TextHAlignAsString = "Center";
            appearance6.TextVAlignAsString = "Middle";
            this.lblDelete.Appearance = appearance6;
            this.lblDelete.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblDelete.Location = new System.Drawing.Point(0, 0);
            this.lblDelete.Margin = new System.Windows.Forms.Padding(3, 3, 0, 3);
            this.lblDelete.Name = "lblDelete";
            this.lblDelete.Size = new System.Drawing.Size(65, 30);
            this.lblDelete.TabIndex = 0;
            this.lblDelete.Tag = "NOCOLOR";
            this.lblDelete.Text = "Alt + R";
            this.lblDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // pnlSave
            // 
            // 
            // pnlSave.ClientArea
            // 
            this.pnlSave.ClientArea.Controls.Add(this.btnSave);
            this.pnlSave.ClientArea.Controls.Add(this.lblSave);
            this.pnlSave.Location = new System.Drawing.Point(747, 17);
            this.pnlSave.Name = "pnlSave";
            this.pnlSave.Size = new System.Drawing.Size(135, 30);
            this.pnlSave.TabIndex = 2;
            // 
            // btnSave
            // 
            appearance7.BackColor = System.Drawing.Color.White;
            appearance7.FontData.BoldAsString = "True";
            appearance7.ForeColor = System.Drawing.Color.Black;
            this.btnSave.Appearance = appearance7;
            this.btnSave.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Flat;
            this.btnSave.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnSave.Location = new System.Drawing.Point(65, 0);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(70, 30);
            this.btnSave.TabIndex = 3;
            this.btnSave.Text = "Save";
            this.btnSave.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // lblSave
            // 
            appearance8.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(138)))), ((int)(((byte)(31)))));
            appearance8.FontData.BoldAsString = "True";
            appearance8.ForeColor = System.Drawing.Color.White;
            appearance8.TextHAlignAsString = "Center";
            appearance8.TextVAlignAsString = "Middle";
            this.lblSave.Appearance = appearance8;
            this.lblSave.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblSave.Location = new System.Drawing.Point(0, 0);
            this.lblSave.Margin = new System.Windows.Forms.Padding(3, 3, 0, 3);
            this.lblSave.Name = "lblSave";
            this.lblSave.Size = new System.Drawing.Size(65, 30);
            this.lblSave.TabIndex = 0;
            this.lblSave.Tag = "NOCOLOR";
            this.lblSave.Text = "Alt + S";
            this.lblSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // grdPhysicalInventory
            // 
            this.grdPhysicalInventory.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grdPhysicalInventory.DataSource = this.ultraDataSource1;
            appearance9.BackColor = System.Drawing.Color.White;
            appearance9.BackColor2 = System.Drawing.Color.White;
            appearance9.BackColorDisabled = System.Drawing.Color.White;
            appearance9.BackColorDisabled2 = System.Drawing.Color.White;
            appearance9.BackGradientStyle = Infragistics.Win.GradientStyle.None;
            this.grdPhysicalInventory.DisplayLayout.Appearance = appearance9;
            ultraGridColumn12.Header.VisiblePosition = 16;
            ultraGridColumn12.Hidden = true;
            ultraGridColumn52.ButtonDisplayStyle = Infragistics.Win.UltraWinGrid.ButtonDisplayStyle.Always;
            appearance10.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(231)))), ((int)(((byte)(231)))));
            appearance10.BackColor2 = System.Drawing.Color.White;
            appearance10.BackGradientStyle = Infragistics.Win.GradientStyle.None;
            appearance10.Image = global::POS_Core_UI.Properties.Resources.search;
            ultraGridColumn52.CellButtonAppearance = appearance10;
            ultraGridColumn52.Header.VisiblePosition = 0;
            ultraGridColumn52.RowLayoutColumnInfo.OriginX = 0;
            ultraGridColumn52.RowLayoutColumnInfo.OriginY = 0;
            ultraGridColumn52.RowLayoutColumnInfo.PreferredCellSize = new System.Drawing.Size(142, 0);
            ultraGridColumn52.RowLayoutColumnInfo.SpanX = 2;
            ultraGridColumn52.RowLayoutColumnInfo.SpanY = 2;
            ultraGridColumn52.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.EditButton;
            ultraGridColumn52.Width = 100;
            ultraGridColumn53.Header.VisiblePosition = 1;
            ultraGridColumn53.RowLayoutColumnInfo.OriginX = 2;
            ultraGridColumn53.RowLayoutColumnInfo.OriginY = 0;
            ultraGridColumn53.RowLayoutColumnInfo.PreferredCellSize = new System.Drawing.Size(156, 0);
            ultraGridColumn53.RowLayoutColumnInfo.SpanX = 2;
            ultraGridColumn53.RowLayoutColumnInfo.SpanY = 2;
            ultraGridColumn54.Header.VisiblePosition = 2;
            ultraGridColumn54.RowLayoutColumnInfo.OriginX = 6;
            ultraGridColumn54.RowLayoutColumnInfo.OriginY = 0;
            ultraGridColumn54.RowLayoutColumnInfo.PreferredCellSize = new System.Drawing.Size(51, 0);
            ultraGridColumn54.RowLayoutColumnInfo.SpanX = 2;
            ultraGridColumn54.RowLayoutColumnInfo.SpanY = 2;
            ultraGridColumn5.Header.VisiblePosition = 4;
            ultraGridColumn5.RowLayoutColumnInfo.OriginX = 8;
            ultraGridColumn5.RowLayoutColumnInfo.OriginY = 0;
            ultraGridColumn5.RowLayoutColumnInfo.PreferredCellSize = new System.Drawing.Size(41, 0);
            ultraGridColumn5.RowLayoutColumnInfo.SpanX = 2;
            ultraGridColumn5.RowLayoutColumnInfo.SpanY = 2;
            ultraGridColumn13.Header.VisiblePosition = 5;
            ultraGridColumn13.Hidden = true;
            ultraGridColumn13.RowLayoutColumnInfo.OriginX = 30;
            ultraGridColumn13.RowLayoutColumnInfo.OriginY = 0;
            ultraGridColumn13.RowLayoutColumnInfo.SpanX = 2;
            ultraGridColumn13.RowLayoutColumnInfo.SpanY = 2;
            ultraGridColumn14.Header.VisiblePosition = 7;
            ultraGridColumn14.Hidden = true;
            ultraGridColumn14.RowLayoutColumnInfo.OriginX = 26;
            ultraGridColumn14.RowLayoutColumnInfo.OriginY = 0;
            ultraGridColumn14.RowLayoutColumnInfo.SpanX = 2;
            ultraGridColumn14.RowLayoutColumnInfo.SpanY = 2;
            ultraGridColumn15.Header.VisiblePosition = 9;
            ultraGridColumn15.Hidden = true;
            ultraGridColumn15.RowLayoutColumnInfo.OriginX = 28;
            ultraGridColumn15.RowLayoutColumnInfo.OriginY = 0;
            ultraGridColumn15.RowLayoutColumnInfo.SpanX = 2;
            ultraGridColumn15.RowLayoutColumnInfo.SpanY = 2;
            ultraGridColumn28.Header.VisiblePosition = 11;
            ultraGridColumn28.Hidden = true;
            ultraGridColumn28.RowLayoutColumnInfo.OriginX = 22;
            ultraGridColumn28.RowLayoutColumnInfo.OriginY = 0;
            ultraGridColumn28.RowLayoutColumnInfo.SpanX = 2;
            ultraGridColumn28.RowLayoutColumnInfo.SpanY = 2;
            ultraGridColumn29.Header.VisiblePosition = 13;
            ultraGridColumn29.Hidden = true;
            ultraGridColumn29.RowLayoutColumnInfo.OriginX = 24;
            ultraGridColumn29.RowLayoutColumnInfo.OriginY = 0;
            ultraGridColumn29.RowLayoutColumnInfo.SpanX = 2;
            ultraGridColumn29.RowLayoutColumnInfo.SpanY = 2;
            ultraGridColumn4.Header.VisiblePosition = 3;
            ultraGridColumn4.RowLayoutColumnInfo.OriginX = 4;
            ultraGridColumn4.RowLayoutColumnInfo.OriginY = 0;
            ultraGridColumn4.RowLayoutColumnInfo.PreferredCellSize = new System.Drawing.Size(65, 0);
            ultraGridColumn4.RowLayoutColumnInfo.SpanX = 2;
            ultraGridColumn4.RowLayoutColumnInfo.SpanY = 2;
            ultraGridColumn6.ButtonDisplayStyle = Infragistics.Win.UltraWinGrid.ButtonDisplayStyle.Always;
            appearance11.TextHAlignAsString = "Left";
            ultraGridColumn6.CellButtonAppearance = appearance11;
            ultraGridColumn6.EditorComponent = this.ultraDateTimeEditor2;
            ultraGridColumn6.Header.VisiblePosition = 6;
            ultraGridColumn6.RowLayoutColumnInfo.OriginX = 18;
            ultraGridColumn6.RowLayoutColumnInfo.OriginY = 0;
            ultraGridColumn6.RowLayoutColumnInfo.PreferredCellSize = new System.Drawing.Size(97, 0);
            ultraGridColumn6.RowLayoutColumnInfo.SpanX = 2;
            ultraGridColumn6.RowLayoutColumnInfo.SpanY = 2;
            ultraGridColumn7.ButtonDisplayStyle = Infragistics.Win.UltraWinGrid.ButtonDisplayStyle.Always;
            appearance12.TextHAlignAsString = "Left";
            ultraGridColumn7.CellButtonAppearance = appearance12;
            ultraGridColumn7.EditorComponent = this.ultraDateTimeEditor1;
            ultraGridColumn7.Header.VisiblePosition = 8;
            ultraGridColumn7.RowLayoutColumnInfo.OriginX = 20;
            ultraGridColumn7.RowLayoutColumnInfo.OriginY = 0;
            ultraGridColumn7.RowLayoutColumnInfo.PreferredCellSize = new System.Drawing.Size(89, 0);
            ultraGridColumn7.RowLayoutColumnInfo.SpanX = 2;
            ultraGridColumn7.RowLayoutColumnInfo.SpanY = 2;
            ultraGridColumn8.Header.VisiblePosition = 10;
            ultraGridColumn8.RowLayoutColumnInfo.OriginX = 10;
            ultraGridColumn8.RowLayoutColumnInfo.OriginY = 0;
            ultraGridColumn8.RowLayoutColumnInfo.PreferredCellSize = new System.Drawing.Size(77, 0);
            ultraGridColumn8.RowLayoutColumnInfo.SpanX = 2;
            ultraGridColumn8.RowLayoutColumnInfo.SpanY = 2;
            ultraGridColumn9.Header.VisiblePosition = 12;
            ultraGridColumn9.RowLayoutColumnInfo.OriginX = 12;
            ultraGridColumn9.RowLayoutColumnInfo.OriginY = 0;
            ultraGridColumn9.RowLayoutColumnInfo.PreferredCellSize = new System.Drawing.Size(96, 0);
            ultraGridColumn9.RowLayoutColumnInfo.SpanX = 2;
            ultraGridColumn9.RowLayoutColumnInfo.SpanY = 2;
            ultraGridColumn10.Header.VisiblePosition = 14;
            ultraGridColumn10.RowLayoutColumnInfo.OriginX = 14;
            ultraGridColumn10.RowLayoutColumnInfo.OriginY = 0;
            ultraGridColumn10.RowLayoutColumnInfo.PreferredCellSize = new System.Drawing.Size(95, 0);
            ultraGridColumn10.RowLayoutColumnInfo.SpanX = 2;
            ultraGridColumn10.RowLayoutColumnInfo.SpanY = 2;
            ultraGridColumn11.Header.VisiblePosition = 15;
            ultraGridColumn11.RowLayoutColumnInfo.OriginX = 16;
            ultraGridColumn11.RowLayoutColumnInfo.OriginY = 0;
            ultraGridColumn11.RowLayoutColumnInfo.PreferredCellSize = new System.Drawing.Size(85, 0);
            ultraGridColumn11.RowLayoutColumnInfo.SpanX = 2;
            ultraGridColumn11.RowLayoutColumnInfo.SpanY = 2;
            ultraGridBand1.Columns.AddRange(new object[] {
            ultraGridColumn12,
            ultraGridColumn52,
            ultraGridColumn53,
            ultraGridColumn54,
            ultraGridColumn5,
            ultraGridColumn13,
            ultraGridColumn14,
            ultraGridColumn15,
            ultraGridColumn28,
            ultraGridColumn29,
            ultraGridColumn4,
            ultraGridColumn6,
            ultraGridColumn7,
            ultraGridColumn8,
            ultraGridColumn9,
            ultraGridColumn10,
            ultraGridColumn11});
            ultraGridBand1.RowLayoutStyle = Infragistics.Win.UltraWinGrid.RowLayoutStyle.ColumnLayout;
            this.grdPhysicalInventory.DisplayLayout.BandsSerializer.Add(ultraGridBand1);
            this.grdPhysicalInventory.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.grdPhysicalInventory.DisplayLayout.InterBandSpacing = 10;
            appearance13.BackColor = System.Drawing.Color.White;
            appearance13.BackColor2 = System.Drawing.Color.White;
            this.grdPhysicalInventory.DisplayLayout.Override.ActiveCardCaptionAppearance = appearance13;
            appearance14.BackColor = System.Drawing.Color.White;
            appearance14.BackColor2 = System.Drawing.Color.White;
            appearance14.BorderColor = System.Drawing.Color.Gray;
            this.grdPhysicalInventory.DisplayLayout.Override.ActiveRowAppearance = appearance14;
            appearance15.BackColor = System.Drawing.Color.White;
            appearance15.BackColor2 = System.Drawing.Color.White;
            appearance15.BorderColor = System.Drawing.Color.Gray;
            this.grdPhysicalInventory.DisplayLayout.Override.AddRowAppearance = appearance15;
            this.grdPhysicalInventory.DisplayLayout.Override.AllowAddNew = Infragistics.Win.UltraWinGrid.AllowAddNew.TemplateOnBottom;
            this.grdPhysicalInventory.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.True;
            this.grdPhysicalInventory.DisplayLayout.Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.True;
            appearance16.BackColor = System.Drawing.Color.Transparent;
            this.grdPhysicalInventory.DisplayLayout.Override.CardAreaAppearance = appearance16;
            appearance17.BackColor = System.Drawing.Color.White;
            appearance17.BackColor2 = System.Drawing.Color.White;
            appearance17.BackColorDisabled = System.Drawing.Color.White;
            appearance17.BackColorDisabled2 = System.Drawing.Color.White;
            appearance17.BorderColor = System.Drawing.Color.Black;
            appearance17.BorderColor3DBase = System.Drawing.Color.Black;
            this.grdPhysicalInventory.DisplayLayout.Override.CellAppearance = appearance17;
            appearance18.BackColor = System.Drawing.Color.White;
            appearance18.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(231)))), ((int)(((byte)(230)))));
            appearance18.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance18.BorderAlpha = Infragistics.Win.Alpha.Transparent;
            appearance18.BorderColor = System.Drawing.Color.Gray;
            appearance18.BorderColor3DBase = System.Drawing.Color.Gray;
            appearance18.Image = ((object)(resources.GetObject("appearance18.Image")));
            appearance18.ImageBackgroundAlpha = Infragistics.Win.Alpha.Transparent;
            appearance18.ImageBackgroundStyle = Infragistics.Win.ImageBackgroundStyle.Centered;
            this.grdPhysicalInventory.DisplayLayout.Override.CellButtonAppearance = appearance18;
            appearance19.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            appearance19.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.grdPhysicalInventory.DisplayLayout.Override.EditCellAppearance = appearance19;
            appearance20.BorderColor = System.Drawing.Color.Gray;
            this.grdPhysicalInventory.DisplayLayout.Override.FilteredInRowAppearance = appearance20;
            appearance21.BorderColor = System.Drawing.Color.Gray;
            this.grdPhysicalInventory.DisplayLayout.Override.FilteredOutRowAppearance = appearance21;
            appearance22.BackColor = System.Drawing.Color.White;
            appearance22.BackColor2 = System.Drawing.Color.White;
            appearance22.BackColorDisabled = System.Drawing.Color.White;
            appearance22.BackColorDisabled2 = System.Drawing.Color.White;
            this.grdPhysicalInventory.DisplayLayout.Override.FixedCellAppearance = appearance22;
            appearance23.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            appearance23.BackColor2 = System.Drawing.Color.Beige;
            this.grdPhysicalInventory.DisplayLayout.Override.FixedHeaderAppearance = appearance23;
            appearance24.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(62)))), ((int)(((byte)(176)))));
            appearance24.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance24.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance24.FontData.BoldAsString = "True";
            appearance24.FontData.SizeInPoints = 10F;
            appearance24.ForeColor = System.Drawing.Color.White;
            appearance24.TextHAlignAsString = "Left";
            appearance24.ThemedElementAlpha = Infragistics.Win.Alpha.Transparent;
            this.grdPhysicalInventory.DisplayLayout.Override.HeaderAppearance = appearance24;
            appearance25.BorderColor = System.Drawing.Color.Gray;
            this.grdPhysicalInventory.DisplayLayout.Override.RowAlternateAppearance = appearance25;
            appearance26.BackColor = System.Drawing.Color.White;
            appearance26.BackColor2 = System.Drawing.Color.White;
            appearance26.BackGradientStyle = Infragistics.Win.GradientStyle.None;
            appearance26.BackHatchStyle = Infragistics.Win.BackHatchStyle.None;
            appearance26.BorderColor = System.Drawing.Color.Gray;
            this.grdPhysicalInventory.DisplayLayout.Override.RowAppearance = appearance26;
            appearance27.BorderColor = System.Drawing.Color.Gray;
            this.grdPhysicalInventory.DisplayLayout.Override.RowPreviewAppearance = appearance27;
            appearance28.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(62)))), ((int)(((byte)(176)))));
            appearance28.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(62)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance28.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance28.BorderColor = System.Drawing.Color.Gray;
            appearance28.ForeColor = System.Drawing.Color.White;
            this.grdPhysicalInventory.DisplayLayout.Override.RowSelectorAppearance = appearance28;
            this.grdPhysicalInventory.DisplayLayout.Override.RowSelectorWidth = 12;
            this.grdPhysicalInventory.DisplayLayout.Override.RowSpacingBefore = 2;
            appearance29.BackColor = System.Drawing.Color.Navy;
            appearance29.BackColorAlpha = Infragistics.Win.Alpha.Opaque;
            this.grdPhysicalInventory.DisplayLayout.Override.SelectedCellAppearance = appearance29;
            appearance30.BackColor = System.Drawing.Color.Navy;
            appearance30.BackColorDisabled = System.Drawing.Color.Navy;
            appearance30.BackGradientStyle = Infragistics.Win.GradientStyle.None;
            appearance30.BackHatchStyle = Infragistics.Win.BackHatchStyle.None;
            appearance30.BorderColor = System.Drawing.Color.Gray;
            appearance30.ForeColor = System.Drawing.Color.White;
            this.grdPhysicalInventory.DisplayLayout.Override.SelectedRowAppearance = appearance30;
            appearance31.BorderColor = System.Drawing.Color.Gray;
            this.grdPhysicalInventory.DisplayLayout.Override.TemplateAddRowAppearance = appearance31;
            this.grdPhysicalInventory.DisplayLayout.RowConnectorColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            this.grdPhysicalInventory.DisplayLayout.RowConnectorStyle = Infragistics.Win.UltraWinGrid.RowConnectorStyle.Solid;
            appearance32.ImageBackgroundOrigin = Infragistics.Win.ImageBackgroundOrigin.Form;
            scrollBarLook1.TrackAppearance = appearance32;
            this.grdPhysicalInventory.DisplayLayout.ScrollBarLook = scrollBarLook1;
            this.grdPhysicalInventory.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.grdPhysicalInventory.Location = new System.Drawing.Point(10, 10);
            this.grdPhysicalInventory.Name = "grdPhysicalInventory";
            this.grdPhysicalInventory.Size = new System.Drawing.Size(1022, 372);
            this.grdPhysicalInventory.TabIndex = 1;
            this.grdPhysicalInventory.UpdateMode = Infragistics.Win.UltraWinGrid.UpdateMode.OnCellChangeOrLostFocus;
            this.grdPhysicalInventory.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.grdPhysicalInventory.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.grdPhysicalInventory_InitializeLayout);
            this.grdPhysicalInventory.ClickCellButton += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.grdPhysicalInventory_ClickCellButton);
            this.grdPhysicalInventory.BeforeCellDeactivate += new System.ComponentModel.CancelEventHandler(this.grdPhysicalInventory_BeforeCellDeactivate);
            this.grdPhysicalInventory.BeforeRowDeactivate += new System.ComponentModel.CancelEventHandler(this.grdPhysicalInventory_BeforeRowDeactivate);
            this.grdPhysicalInventory.Enter += new System.EventHandler(this.grdPhysicalInventory_Enter);
            this.grdPhysicalInventory.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmPhysicalInv_KeyDown);
            this.grdPhysicalInventory.KeyUp += new System.Windows.Forms.KeyEventHandler(this.frmPhysicalInv_KeyUp);
            // 
            // ultraDataSource1
            // 
            ultraDataColumn8.ReadOnly = Infragistics.Win.DefaultableBoolean.True;
            this.ultraDataSource1.Band.Columns.AddRange(new object[] {
            ultraDataColumn1,
            ultraDataColumn2,
            ultraDataColumn3,
            ultraDataColumn4,
            ultraDataColumn5,
            ultraDataColumn6,
            ultraDataColumn7,
            ultraDataColumn8,
            ultraDataColumn9,
            ultraDataColumn10});
            // 
            // ultraTabPageControl2
            // 
            this.ultraTabPageControl2.Controls.Add(this.groupBox4);
            this.ultraTabPageControl2.Controls.Add(this.grdHistory);
            this.ultraTabPageControl2.Location = new System.Drawing.Point(2, 22);
            this.ultraTabPageControl2.Name = "ultraTabPageControl2";
            this.ultraTabPageControl2.Size = new System.Drawing.Size(1041, 439);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.pnlDeleteRow);
            this.groupBox4.Controls.Add(this.pnlProcess);
            this.groupBox4.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.groupBox4.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.groupBox4.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox4.Location = new System.Drawing.Point(0, 387);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(1041, 52);
            this.groupBox4.TabIndex = 30;
            this.groupBox4.TabStop = false;
            // 
            // pnlDeleteRow
            // 
            // 
            // pnlDeleteRow.ClientArea
            // 
            this.pnlDeleteRow.ClientArea.Controls.Add(this.btnDeleteRow);
            this.pnlDeleteRow.ClientArea.Controls.Add(this.lblDeleteRow);
            this.pnlDeleteRow.Location = new System.Drawing.Point(555, 17);
            this.pnlDeleteRow.Name = "pnlDeleteRow";
            this.pnlDeleteRow.Size = new System.Drawing.Size(175, 30);
            this.pnlDeleteRow.TabIndex = 15;
            // 
            // btnDeleteRow
            // 
            appearance33.BackColor = System.Drawing.Color.White;
            appearance33.FontData.BoldAsString = "True";
            appearance33.ForeColor = System.Drawing.Color.Black;
            this.btnDeleteRow.Appearance = appearance33;
            this.btnDeleteRow.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Flat;
            this.btnDeleteRow.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnDeleteRow.Location = new System.Drawing.Point(65, 0);
            this.btnDeleteRow.Name = "btnDeleteRow";
            this.btnDeleteRow.Size = new System.Drawing.Size(110, 30);
            this.btnDeleteRow.TabIndex = 16;
            this.btnDeleteRow.Text = "Delete Row";
            this.btnDeleteRow.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnDeleteRow.Click += new System.EventHandler(this.btnDeleteRow_Click);
            // 
            // lblDeleteRow
            // 
            appearance34.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(138)))), ((int)(((byte)(31)))));
            appearance34.FontData.BoldAsString = "True";
            appearance34.ForeColor = System.Drawing.Color.White;
            appearance34.TextHAlignAsString = "Center";
            appearance34.TextVAlignAsString = "Middle";
            this.lblDeleteRow.Appearance = appearance34;
            this.lblDeleteRow.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblDeleteRow.Location = new System.Drawing.Point(0, 0);
            this.lblDeleteRow.Margin = new System.Windows.Forms.Padding(3, 3, 0, 3);
            this.lblDeleteRow.Name = "lblDeleteRow";
            this.lblDeleteRow.Size = new System.Drawing.Size(65, 30);
            this.lblDeleteRow.TabIndex = 0;
            this.lblDeleteRow.Tag = "NOCOLOR";
            this.lblDeleteRow.Text = "Alt + D";
            this.lblDeleteRow.Click += new System.EventHandler(this.btnDeleteRow_Click);
            // 
            // pnlProcess
            // 
            // 
            // pnlProcess.ClientArea
            // 
            this.pnlProcess.ClientArea.Controls.Add(this.btnProcess);
            this.pnlProcess.ClientArea.Controls.Add(this.lblProcess);
            this.pnlProcess.Location = new System.Drawing.Point(741, 17);
            this.pnlProcess.Name = "pnlProcess";
            this.pnlProcess.Size = new System.Drawing.Size(145, 30);
            this.pnlProcess.TabIndex = 13;
            // 
            // btnProcess
            // 
            appearance35.BackColor = System.Drawing.Color.White;
            appearance35.FontData.BoldAsString = "True";
            appearance35.ForeColor = System.Drawing.Color.Black;
            this.btnProcess.Appearance = appearance35;
            this.btnProcess.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Flat;
            this.btnProcess.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnProcess.Location = new System.Drawing.Point(65, 0);
            this.btnProcess.Name = "btnProcess";
            this.btnProcess.Size = new System.Drawing.Size(80, 30);
            this.btnProcess.TabIndex = 14;
            this.btnProcess.Text = "Process";
            this.btnProcess.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnProcess.Click += new System.EventHandler(this.btnProcess_Click);
            // 
            // lblProcess
            // 
            appearance36.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(138)))), ((int)(((byte)(31)))));
            appearance36.FontData.BoldAsString = "True";
            appearance36.ForeColor = System.Drawing.Color.White;
            appearance36.TextHAlignAsString = "Center";
            appearance36.TextVAlignAsString = "Middle";
            this.lblProcess.Appearance = appearance36;
            this.lblProcess.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblProcess.Location = new System.Drawing.Point(0, 0);
            this.lblProcess.Margin = new System.Windows.Forms.Padding(3, 3, 0, 3);
            this.lblProcess.Name = "lblProcess";
            this.lblProcess.Size = new System.Drawing.Size(65, 30);
            this.lblProcess.TabIndex = 0;
            this.lblProcess.Tag = "NOCOLOR";
            this.lblProcess.Text = "Alt + P";
            this.lblProcess.Click += new System.EventHandler(this.btnProcess_Click);
            // 
            // grdHistory
            // 
            this.grdHistory.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grdHistory.DataSource = this.ultraDataSource1;
            appearance37.BackColor = System.Drawing.Color.White;
            appearance37.BackColor2 = System.Drawing.Color.White;
            appearance37.BackColorDisabled = System.Drawing.Color.White;
            appearance37.BackColorDisabled2 = System.Drawing.Color.White;
            appearance37.BackGradientStyle = Infragistics.Win.GradientStyle.None;
            this.grdHistory.DisplayLayout.Appearance = appearance37;
            ultraGridColumn32.Header.VisiblePosition = 0;
            ultraGridColumn33.Header.VisiblePosition = 1;
            ultraGridColumn34.Header.VisiblePosition = 2;
            ultraGridColumn35.Header.VisiblePosition = 4;
            ultraGridColumn36.Header.VisiblePosition = 6;
            ultraGridColumn37.Header.VisiblePosition = 7;
            ultraGridColumn38.Header.VisiblePosition = 8;
            ultraGridColumn39.Header.VisiblePosition = 9;
            ultraGridColumn40.Header.VisiblePosition = 10;
            ultraGridColumn41.Header.VisiblePosition = 11;
            ultraGridColumn26.Header.VisiblePosition = 3;
            ultraGridColumn26.RowLayoutColumnInfo.OriginX = 22;
            ultraGridColumn26.RowLayoutColumnInfo.OriginY = 0;
            ultraGridColumn26.RowLayoutColumnInfo.SpanX = 2;
            ultraGridColumn26.RowLayoutColumnInfo.SpanY = 2;
            ultraGridColumn27.Header.VisiblePosition = 5;
            ultraGridColumn27.RowLayoutColumnInfo.OriginX = 24;
            ultraGridColumn27.RowLayoutColumnInfo.OriginY = 0;
            ultraGridColumn27.RowLayoutColumnInfo.SpanX = 2;
            ultraGridColumn27.RowLayoutColumnInfo.SpanY = 2;
            ultraGridBand2.Columns.AddRange(new object[] {
            ultraGridColumn32,
            ultraGridColumn33,
            ultraGridColumn34,
            ultraGridColumn35,
            ultraGridColumn36,
            ultraGridColumn37,
            ultraGridColumn38,
            ultraGridColumn39,
            ultraGridColumn40,
            ultraGridColumn41,
            ultraGridColumn26,
            ultraGridColumn27});
            ultraGridBand2.RowLayoutStyle = Infragistics.Win.UltraWinGrid.RowLayoutStyle.ColumnLayout;
            ultraGridBand2.SummaryFooterCaption = "";
            this.grdHistory.DisplayLayout.BandsSerializer.Add(ultraGridBand2);
            this.grdHistory.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.grdHistory.DisplayLayout.InterBandSpacing = 10;
            this.grdHistory.DisplayLayout.MaxColScrollRegions = 1;
            this.grdHistory.DisplayLayout.MaxRowScrollRegions = 1;
            appearance38.BackColor = System.Drawing.Color.White;
            appearance38.BackColor2 = System.Drawing.Color.White;
            this.grdHistory.DisplayLayout.Override.ActiveCardCaptionAppearance = appearance38;
            appearance39.BackColor = System.Drawing.Color.White;
            appearance39.BackColor2 = System.Drawing.Color.White;
            appearance39.BorderColor = System.Drawing.Color.Gray;
            this.grdHistory.DisplayLayout.Override.ActiveRowAppearance = appearance39;
            appearance40.BackColor = System.Drawing.Color.White;
            appearance40.BackColor2 = System.Drawing.Color.White;
            appearance40.BorderColor = System.Drawing.Color.Gray;
            this.grdHistory.DisplayLayout.Override.AddRowAppearance = appearance40;
            this.grdHistory.DisplayLayout.Override.AllowAddNew = Infragistics.Win.UltraWinGrid.AllowAddNew.No;
            this.grdHistory.DisplayLayout.Override.AllowColMoving = Infragistics.Win.UltraWinGrid.AllowColMoving.WithinBand;
            this.grdHistory.DisplayLayout.Override.AllowColSwapping = Infragistics.Win.UltraWinGrid.AllowColSwapping.NotAllowed;
            this.grdHistory.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.True;
            this.grdHistory.DisplayLayout.Override.AllowRowFiltering = Infragistics.Win.DefaultableBoolean.False;
            this.grdHistory.DisplayLayout.Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.False;
            appearance41.BackColor = System.Drawing.Color.Transparent;
            this.grdHistory.DisplayLayout.Override.CardAreaAppearance = appearance41;
            appearance42.BackColor = System.Drawing.Color.White;
            appearance42.BackColor2 = System.Drawing.Color.White;
            appearance42.BackColorDisabled = System.Drawing.Color.White;
            appearance42.BackColorDisabled2 = System.Drawing.Color.White;
            appearance42.BorderColor = System.Drawing.Color.Black;
            appearance42.BorderColor3DBase = System.Drawing.Color.Black;
            this.grdHistory.DisplayLayout.Override.CellAppearance = appearance42;
            appearance43.BackColor = System.Drawing.Color.White;
            appearance43.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(231)))), ((int)(((byte)(230)))));
            appearance43.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance43.BorderAlpha = Infragistics.Win.Alpha.Transparent;
            appearance43.BorderColor = System.Drawing.Color.Gray;
            appearance43.BorderColor3DBase = System.Drawing.Color.Gray;
            appearance43.Image = ((object)(resources.GetObject("appearance43.Image")));
            appearance43.ImageBackgroundAlpha = Infragistics.Win.Alpha.Transparent;
            appearance43.ImageBackgroundStyle = Infragistics.Win.ImageBackgroundStyle.Centered;
            this.grdHistory.DisplayLayout.Override.CellButtonAppearance = appearance43;
            appearance44.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            appearance44.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.grdHistory.DisplayLayout.Override.EditCellAppearance = appearance44;
            appearance45.BorderColor = System.Drawing.Color.Gray;
            this.grdHistory.DisplayLayout.Override.FilteredInRowAppearance = appearance45;
            appearance46.BorderColor = System.Drawing.Color.Gray;
            this.grdHistory.DisplayLayout.Override.FilteredOutRowAppearance = appearance46;
            appearance47.BackColor = System.Drawing.Color.White;
            appearance47.BackColor2 = System.Drawing.Color.White;
            appearance47.BackColorDisabled = System.Drawing.Color.White;
            appearance47.BackColorDisabled2 = System.Drawing.Color.White;
            this.grdHistory.DisplayLayout.Override.FixedCellAppearance = appearance47;
            appearance48.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            appearance48.BackColor2 = System.Drawing.Color.Beige;
            this.grdHistory.DisplayLayout.Override.FixedHeaderAppearance = appearance48;
            appearance49.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(62)))), ((int)(((byte)(176)))));
            appearance49.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance49.FontData.BoldAsString = "True";
            appearance49.FontData.SizeInPoints = 10F;
            appearance49.ForeColor = System.Drawing.Color.White;
            this.grdHistory.DisplayLayout.Override.HeaderAppearance = appearance49;
            this.grdHistory.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortSingle;
            this.grdHistory.DisplayLayout.Override.MergedCellStyle = Infragistics.Win.UltraWinGrid.MergedCellStyle.Never;
            appearance50.BorderColor = System.Drawing.Color.Gray;
            this.grdHistory.DisplayLayout.Override.RowAlternateAppearance = appearance50;
            appearance51.BackColor = System.Drawing.Color.White;
            appearance51.BackColor2 = System.Drawing.Color.White;
            appearance51.BorderColor = System.Drawing.Color.Gray;
            this.grdHistory.DisplayLayout.Override.RowAppearance = appearance51;
            appearance52.BorderColor = System.Drawing.Color.Gray;
            this.grdHistory.DisplayLayout.Override.RowPreviewAppearance = appearance52;
            appearance53.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(62)))), ((int)(((byte)(176)))));
            appearance53.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance53.BorderColor = System.Drawing.Color.Gray;
            this.grdHistory.DisplayLayout.Override.RowSelectorAppearance = appearance53;
            this.grdHistory.DisplayLayout.Override.RowSelectorWidth = 12;
            this.grdHistory.DisplayLayout.Override.RowSizingArea = Infragistics.Win.UltraWinGrid.RowSizingArea.EntireRow;
            this.grdHistory.DisplayLayout.Override.RowSpacingBefore = 2;
            appearance54.BackColor = System.Drawing.Color.Navy;
            this.grdHistory.DisplayLayout.Override.SelectedCellAppearance = appearance54;
            appearance55.BackColor = System.Drawing.Color.Navy;
            appearance55.BackColorDisabled = System.Drawing.Color.Navy;
            appearance55.BorderColor = System.Drawing.Color.Gray;
            appearance55.ForeColor = System.Drawing.Color.White;
            this.grdHistory.DisplayLayout.Override.SelectedRowAppearance = appearance55;
            this.grdHistory.DisplayLayout.Override.SelectTypeCell = Infragistics.Win.UltraWinGrid.SelectType.None;
            this.grdHistory.DisplayLayout.Override.SelectTypeCol = Infragistics.Win.UltraWinGrid.SelectType.None;
            this.grdHistory.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.None;
            appearance56.BorderColor = System.Drawing.Color.Gray;
            this.grdHistory.DisplayLayout.Override.TemplateAddRowAppearance = appearance56;
            this.grdHistory.DisplayLayout.RowConnectorColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            this.grdHistory.DisplayLayout.RowConnectorStyle = Infragistics.Win.UltraWinGrid.RowConnectorStyle.Solid;
            appearance57.BackColor = System.Drawing.Color.White;
            appearance57.BackColor2 = System.Drawing.Color.Gainsboro;
            appearance57.BorderColor = System.Drawing.Color.WhiteSmoke;
            appearance57.BorderColor3DBase = System.Drawing.Color.WhiteSmoke;
            scrollBarLook2.Appearance = appearance57;
            appearance58.BackColor = System.Drawing.Color.LightGray;
            appearance58.BackColor2 = System.Drawing.Color.WhiteSmoke;
            scrollBarLook2.ButtonAppearance = appearance58;
            appearance59.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            scrollBarLook2.ThumbAppearance = appearance59;
            appearance60.BackColor = System.Drawing.Color.Gainsboro;
            appearance60.BackColor2 = System.Drawing.Color.WhiteSmoke;
            appearance60.BorderColor = System.Drawing.Color.White;
            appearance60.BorderColor3DBase = System.Drawing.Color.Gainsboro;
            scrollBarLook2.TrackAppearance = appearance60;
            this.grdHistory.DisplayLayout.ScrollBarLook = scrollBarLook2;
            this.grdHistory.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.grdHistory.Location = new System.Drawing.Point(10, 10);
            this.grdHistory.Name = "grdHistory";
            this.grdHistory.Size = new System.Drawing.Size(1022, 372);
            this.grdHistory.TabIndex = 12;
            this.grdHistory.UpdateMode = Infragistics.Win.UltraWinGrid.UpdateMode.OnCellChangeOrLostFocus;
            this.grdHistory.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.grdHistory.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.grdHistory_InitializeLayout);
            this.grdHistory.AfterRowsDeleted += new System.EventHandler(this.grdHistory_AfterRowsDeleted);
            this.grdHistory.BeforeRowsDeleted += new Infragistics.Win.UltraWinGrid.BeforeRowsDeletedEventHandler(this.grdHistory_BeforeRowsDeleted);
            // 
            // pnlClose
            // 
            // 
            // pnlClose.ClientArea
            // 
            this.pnlClose.ClientArea.Controls.Add(this.btnClose);
            this.pnlClose.ClientArea.Controls.Add(this.lblClose);
            this.pnlClose.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pnlClose.Location = new System.Drawing.Point(910, 470);
            this.pnlClose.Name = "pnlClose";
            this.pnlClose.Size = new System.Drawing.Size(135, 30);
            this.pnlClose.TabIndex = 10;
            // 
            // btnClose
            // 
            appearance61.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(62)))), ((int)(((byte)(76)))));
            appearance61.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(62)))), ((int)(((byte)(76)))));
            appearance61.FontData.BoldAsString = "True";
            appearance61.ForeColor = System.Drawing.Color.Black;
            this.btnClose.Appearance = appearance61;
            this.btnClose.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Flat;
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Dock = System.Windows.Forms.DockStyle.Fill;
            appearance62.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(188)))), ((int)(((byte)(193)))), ((int)(((byte)(207)))));
            appearance62.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(188)))), ((int)(((byte)(193)))), ((int)(((byte)(207)))));
            this.btnClose.HotTrackAppearance = appearance62;
            this.btnClose.Location = new System.Drawing.Point(65, 0);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(70, 30);
            this.btnClose.TabIndex = 11;
            this.btnClose.Tag = "";
            this.btnClose.Text = "Close";
            this.btnClose.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // lblClose
            // 
            appearance63.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(138)))), ((int)(((byte)(31)))));
            appearance63.FontData.BoldAsString = "True";
            appearance63.ForeColor = System.Drawing.Color.White;
            appearance63.TextHAlignAsString = "Center";
            appearance63.TextVAlignAsString = "Middle";
            this.lblClose.Appearance = appearance63;
            this.lblClose.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblClose.Location = new System.Drawing.Point(0, 0);
            this.lblClose.Margin = new System.Windows.Forms.Padding(3, 3, 0, 3);
            this.lblClose.Name = "lblClose";
            this.lblClose.Size = new System.Drawing.Size(65, 30);
            this.lblClose.TabIndex = 0;
            this.lblClose.Tag = "NOCOLOR";
            this.lblClose.Text = "Alt + C";
            this.lblClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // ultraDataSource2
            // 
            this.ultraDataSource2.Band.Columns.AddRange(new object[] {
            ultraDataColumn11,
            ultraDataColumn12});
            // 
            // lblTransactionType
            // 
            appearance64.ForeColor = System.Drawing.Color.Black;
            appearance64.ForeColorDisabled = System.Drawing.Color.Navy;
            appearance64.ImageHAlign = Infragistics.Win.HAlign.Center;
            appearance64.ImageVAlign = Infragistics.Win.VAlign.Middle;
            appearance64.TextHAlignAsString = "Center";
            appearance64.TextVAlignAsString = "Middle";
            this.lblTransactionType.Appearance = appearance64;
            this.lblTransactionType.BackColorInternal = System.Drawing.Color.Transparent;
            this.lblTransactionType.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblTransactionType.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTransactionType.Location = new System.Drawing.Point(0, 0);
            this.lblTransactionType.Name = "lblTransactionType";
            this.lblTransactionType.Size = new System.Drawing.Size(1069, 38);
            this.lblTransactionType.TabIndex = 26;
            this.lblTransactionType.Tag = "Header";
            this.lblTransactionType.Text = "Physical Inventory";
            this.lblTransactionType.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // ultraTabPhyInv
            // 
            this.ultraTabPhyInv.Controls.Add(this.ultraTabSharedControlsPage1);
            this.ultraTabPhyInv.Controls.Add(this.ultraTabPageControl1);
            this.ultraTabPhyInv.Controls.Add(this.ultraTabPageControl2);
            this.ultraTabPhyInv.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultraTabPhyInv.Location = new System.Drawing.Point(12, 44);
            this.ultraTabPhyInv.Name = "ultraTabPhyInv";
            appearance65.BackColor = System.Drawing.Color.Blue;
            appearance65.ForeColor = System.Drawing.Color.White;
            this.ultraTabPhyInv.SelectedTabAppearance = appearance65;
            this.ultraTabPhyInv.SharedControlsPage = this.ultraTabSharedControlsPage1;
            this.ultraTabPhyInv.Size = new System.Drawing.Size(1045, 463);
            this.ultraTabPhyInv.Style = Infragistics.Win.UltraWinTabControl.UltraTabControlStyle.PropertyPage2003;
            this.ultraTabPhyInv.TabIndex = 0;
            this.ultraTabPhyInv.TabLayoutStyle = Infragistics.Win.UltraWinTabs.TabLayoutStyle.SingleRowSizeToFit;
            appearance66.FontData.BoldAsString = "True";
            ultraTab1.Appearance = appearance66;
            ultraTab1.Key = "PhysicalInventory";
            ultraTab1.TabPage = this.ultraTabPageControl1;
            ultraTab1.Text = "Add Physical Inventory";
            ultraTab2.Key = "Process";
            ultraTab2.TabPage = this.ultraTabPageControl2;
            ultraTab2.Text = "Process";
            this.ultraTabPhyInv.Tabs.AddRange(new Infragistics.Win.UltraWinTabControl.UltraTab[] {
            ultraTab1,
            ultraTab2});
            this.ultraTabPhyInv.ViewStyle = Infragistics.Win.UltraWinTabControl.ViewStyle.Office2003;
            this.ultraTabPhyInv.SelectedTabChanged += new Infragistics.Win.UltraWinTabControl.SelectedTabChangedEventHandler(this.ultraTabPhyInv_SelectedTabChanged);
            // 
            // ultraTabSharedControlsPage1
            // 
            this.ultraTabSharedControlsPage1.Location = new System.Drawing.Point(-10000, -10000);
            this.ultraTabSharedControlsPage1.Name = "ultraTabSharedControlsPage1";
            this.ultraTabSharedControlsPage1.Size = new System.Drawing.Size(1041, 439);
            // 
            // lblMessage
            // 
            appearance67.ForeColor = System.Drawing.Color.Red;
            appearance67.TextHAlignAsString = "Center";
            this.lblMessage.Appearance = appearance67;
            this.lblMessage.BackColorInternal = System.Drawing.Color.Transparent;
            this.lblMessage.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblMessage.Font = new System.Drawing.Font("Times New Roman", 11F, System.Drawing.FontStyle.Bold);
            this.lblMessage.Location = new System.Drawing.Point(0, 516);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.Size = new System.Drawing.Size(1069, 20);
            this.lblMessage.TabIndex = 33;
            this.lblMessage.Tag = "NOCOLOR";
            this.lblMessage.Text = "cost price is hidden due to the user does not have enough permissions";
            this.lblMessage.Visible = false;
            // 
            // pnlPrintShelfStickers
            // 
            // 
            // pnlPrintShelfStickers.ClientArea
            // 
            this.pnlPrintShelfStickers.ClientArea.Controls.Add(this.btnPrintShelfStickers);
            this.pnlPrintShelfStickers.ClientArea.Controls.Add(this.lblPrintShelfStickers);
            this.pnlPrintShelfStickers.Location = new System.Drawing.Point(50, 470);
            this.pnlPrintShelfStickers.Name = "pnlPrintShelfStickers";
            this.pnlPrintShelfStickers.Size = new System.Drawing.Size(230, 30);
            this.pnlPrintShelfStickers.TabIndex = 9;
            // 
            // btnPrintShelfStickers
            // 
            appearance68.BackColor = System.Drawing.Color.White;
            appearance68.ForeColor = System.Drawing.Color.Black;
            this.btnPrintShelfStickers.Appearance = appearance68;
            this.btnPrintShelfStickers.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Flat;
            this.btnPrintShelfStickers.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnPrintShelfStickers.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPrintShelfStickers.Location = new System.Drawing.Point(60, 0);
            this.btnPrintShelfStickers.Name = "btnPrintShelfStickers";
            this.btnPrintShelfStickers.Size = new System.Drawing.Size(170, 30);
            this.btnPrintShelfStickers.TabIndex = 10;
            this.btnPrintShelfStickers.Text = "Print Shelf Stickers";
            this.btnPrintShelfStickers.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnPrintShelfStickers.Click += new System.EventHandler(this.btnPrintShelfStickers_Click);
            // 
            // lblPrintShelfStickers
            // 
            appearance69.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(138)))), ((int)(((byte)(31)))));
            appearance69.ForeColor = System.Drawing.Color.White;
            appearance69.TextHAlignAsString = "Center";
            appearance69.TextVAlignAsString = "Middle";
            this.lblPrintShelfStickers.Appearance = appearance69;
            this.lblPrintShelfStickers.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblPrintShelfStickers.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Bold);
            this.lblPrintShelfStickers.Location = new System.Drawing.Point(0, 0);
            this.lblPrintShelfStickers.Margin = new System.Windows.Forms.Padding(3, 3, 0, 3);
            this.lblPrintShelfStickers.Name = "lblPrintShelfStickers";
            this.lblPrintShelfStickers.Size = new System.Drawing.Size(60, 30);
            this.lblPrintShelfStickers.TabIndex = 0;
            this.lblPrintShelfStickers.Tag = "NOCOLOR";
            this.lblPrintShelfStickers.Text = "Alt + Q";
            this.lblPrintShelfStickers.Click += new System.EventHandler(this.btnPrintShelfStickers_Click);
            // 
            // frmPhysicalInv
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1069, 536);
            this.Controls.Add(this.pnlPrintShelfStickers);
            this.Controls.Add(this.lblMessage);
            this.Controls.Add(this.pnlClose);
            this.Controls.Add(this.ultraTabPhyInv);
            this.Controls.Add(this.lblTransactionType);
            this.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.Name = "frmPhysicalInv";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Physical Inventory";
            this.Activated += new System.EventHandler(this.frmPhysicalInv_Activated);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmPhysicalInv_FormClosing);
            this.Load += new System.EventHandler(this.frmPhysicalInv_Load);
            this.Shown += new System.EventHandler(this.frmPhysicalInv_Shown);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmPhysicalInv_KeyDown);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.frmPhysicalInv_KeyUp);
            ((System.ComponentModel.ISupportInitialize)(this.ultraDateTimeEditor2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraDateTimeEditor1)).EndInit();
            this.ultraTabPageControl1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.pnlClear.ClientArea.ResumeLayout(false);
            this.pnlClear.ResumeLayout(false);
            this.pnlAddItem.ClientArea.ResumeLayout(false);
            this.pnlAddItem.ResumeLayout(false);
            this.pnlDelete.ClientArea.ResumeLayout(false);
            this.pnlDelete.ResumeLayout(false);
            this.pnlSave.ClientArea.ResumeLayout(false);
            this.pnlSave.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdPhysicalInventory)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraDataSource1)).EndInit();
            this.ultraTabPageControl2.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.pnlDeleteRow.ClientArea.ResumeLayout(false);
            this.pnlDeleteRow.ResumeLayout(false);
            this.pnlProcess.ClientArea.ResumeLayout(false);
            this.pnlProcess.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdHistory)).EndInit();
            this.pnlClose.ClientArea.ResumeLayout(false);
            this.pnlClose.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ultraDataSource2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraTabPhyInv)).EndInit();
            this.ultraTabPhyInv.ResumeLayout(false);
            this.pnlPrintShelfStickers.ClientArea.ResumeLayout(false);
            this.pnlPrintShelfStickers.ResumeLayout(false);
            this.ResumeLayout(false);

        }
        #endregion

        #region form events
        private void frmPhysicalInv_Load(object sender, System.EventArgs e)
        {
            try
            {
                #region PRIMEPOS-2464 10-Mar-2020 JY Added
                nDisplayItemCost = Configuration.convertBoolToInt(UserPriviliges.IsUserHasPriviliges(UserPriviliges.Modules.InventoryMgmt.ID, UserPriviliges.Screens.ItemFile.ID, UserPriviliges.Permissions.DisplayItemCost.ID));   //PRIMEPOS-2464 09-Mar-2020 JY Added
                if (nDisplayItemCost == 0)
                {
                    lblMessage.Visible = true;
                    tmrBlinking = new System.Timers.Timer();
                    tmrBlinking.Interval = 1000;//1 seconds
                    tmrBlinking.Elapsed += new ElapsedEventHandler(tmrBlinkingTimedEvent);
                    tmrBlinking.Enabled = true;
                    pnlAddItem.Enabled = false;
                }
                else
                {
                    this.Height -= 15;
                }
                #endregion

                clsUIHelper.SetKeyActionMappings(this.grdPhysicalInventory);
                //Clear();
                LoadHistory();
                SetNew();
                ApplyPhysicalInventoryGridFormat();

                clsUIHelper.SetReadonlyRow(this.grdHistory);
                clsUIHelper.SetAppearance(this.grdHistory);

                this.grdPhysicalInventory.AfterEnterEditMode += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
                this.grdPhysicalInventory.AfterExitEditMode += new System.EventHandler(clsUIHelper.AfterExitEditMode);

                clsUIHelper.setColorSchecme(this);
                btnClose.Appearance.BackColor = System.Drawing.Color.Green; //PRIMEPOS-2984 21-Jul-2021 JY Added
                btnClose.Appearance.BackColor2 = System.Drawing.Color.Green;    //PRIMEPOS-2984 21-Jul-2021 JY Added
            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }

        private void frmPhysicalInv_Shown(object sender, EventArgs e)
        {
            this.grdPhysicalInventory.PerformAction(UltraGridAction.EnterEditMode);
        }

        private void frmPhysicalInv_Activated(object sender, System.EventArgs e)
        {
            clsUIHelper.CurrentForm = this;
        }

        private void frmPhysicalInv_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            try
            {
                if (e.KeyData == System.Windows.Forms.Keys.F2)  //PRIMEPOS-2395 21-Jun-2018 JY Added
                {
                    if (nDisplayItemCost == 0)
                        btnAddItem_Click(btnAddItem, e);
                }
                else if (e.KeyData == System.Windows.Forms.Keys.F4)
                {
                    if (this.grdPhysicalInventory.ContainsFocus == true)
                    {
                        if (this.grdPhysicalInventory.ActiveCell != null)
                        {
                            if (this.grdPhysicalInventory.ActiveCell.Column.Key == clsPOSDBConstants.PhysicalInv_Fld_ItemCode)
                                this.SearchItem(Configuration.convertNullToString(this.grdPhysicalInventory.ActiveRow.Cells[clsPOSDBConstants.PhysicalInv_Fld_ItemCode].Value));
                        }
                    }
                }
            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }

        private void frmPhysicalInv_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            try
            {
                if (e.KeyData == System.Windows.Forms.Keys.Enter)
                {
                    if (this.grdPhysicalInventory.ContainsFocus == true && this.grdPhysicalInventory.ActiveCell.Text.Trim() == "" && this.grdPhysicalInventory.ActiveCell.Column.Key == clsPOSDBConstants.PhysicalInv_Fld_ItemCode && this.grdPhysicalInventory.ActiveCell.Row.IsAddRow == true)
                    {
                        this.SelectNextControl(this.grdPhysicalInventory, true, true, true, true);
                    }
                    else if (this.grdPhysicalInventory.ContainsFocus == false)
                    {
                        this.SelectNextControl(this.ActiveControl, true, true, true, true);
                    }
                }
                #region PRIMEPOS-2396 11-Jun-2018 JY Added
                else
                {
                    if (e.Alt)
                        ShortCutKeyAction(e.KeyCode);
                }
                #endregion
            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }
        #endregion

        #region PRIMEPOS-2396 11-Jun-2018 JY Added
        private void ShortCutKeyAction(Keys KeyCode)
        {
            switch (KeyCode)
            {
                case Keys.C:
                    if (pnlClose.Enabled == true)
                        btnClose_Click(btnClose, new EventArgs());
                    break;
                case Keys.D:
                    btnDeleteRow_Click(btnDeleteRow, new EventArgs());
                    break;
                case Keys.L:
                    if (pnlClear.Enabled == true)
                        btnClear_Click(btnClear, new EventArgs());
                    break;
                case Keys.P:
                    btnProcess_Click(btnProcess, new EventArgs());
                    break;
                case Keys.Q:    //PRIMEPOS-2243 27-Apr-2021 JY Added
                    btnPrintShelfStickers_Click(btnPrintShelfStickers, new EventArgs());
                    break;
                case Keys.R:
                    btnDelete_Click(btnDelete, new EventArgs());
                    break;
                case Keys.S:
                    btnSave_Click(btnSave, new EventArgs());
                    break;
                default:
                    break;
            }
        }
        #endregion           

        #region Process tab
        private void LoadHistory()
        {
            PhysicalInvData oPhysicalInvData;
            oPhysicalInvData = oPhysicalInv.Populate(0);
            this.grdHistory.DataSource = null;  //Sprint-21 - 2206 10-Mar-2016 JY Added
            this.grdHistory.DataSource = oPhysicalInvData;
            this.grdHistory.Refresh();
            ApplyHistoryGridFormat();
        }

        private void ApplyHistoryGridFormat()
        {
            this.grdHistory.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.PhysicalInv_Fld_ID].Hidden = true;
            this.grdHistory.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.PhysicalInv_Fld_isProcessed].Hidden = true;
            this.grdHistory.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.PhysicalInv_Fld_PTransDate].Hidden = true;
            this.grdHistory.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.PhysicalInv_Fld_PUserID].Hidden = true;
            this.grdHistory.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.PhysicalInv_Fld_LastInvUpdatedQty].Hidden = true;
            //ugTax.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.fld_UserID].Hidden = true;
            //ugTax.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.fld_UserID].Hidden = true;

            #region PRIMEPOS-2464 10-Mar-2020 JY Added
            if (nDisplayItemCost == 0)
            {
                this.grdHistory.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.PhysicalInv_Fld_OldCostPrice].Hidden = true;
                this.grdHistory.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.PhysicalInv_Fld_NewCostPrice].Hidden = true;
            }
            #endregion

            this.grdHistory.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.PhysicalInv_Fld_TransDate].CellActivation = Activation.NoEdit;

            clsUIHelper.SetAppearance(this.grdHistory);
            this.grdHistory.DisplayLayout.Override.RowSelectors = DefaultableBoolean.False;
            //this.grdHistory.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.PhysicalInv_Fld_Amount].CellAppearance.TextHAlign= Infragistics.Win.HAlign.Right;
        }

        private void btnProcess_Click(object sender, System.EventArgs e)
        {
            if (this.grdHistory.Rows.Count > 0)
            {
                try
                {
                    if (Resources.Message.Display("This will update item's inventory.\n Are you sure?", "Confirmation", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        this.oPhysicalInv.ProcessData();
                        this.LoadHistory();
                        Resources.Message.Display("Process completed successfully.", "Physical Inventory", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                catch (Exception exp)
                { clsUIHelper.ShowErrorMsg(exp.Message); }
            }
        }

        private void btnDeleteRow_Click(object sender, System.EventArgs e)
        {
            if (this.grdHistory.Rows.Count > 0)
            {
                this.grdHistory.ActiveRow.Delete();
            }
        }

        private void grdHistory_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            #region dynamic record count 
            SummarySettings summary = new SummarySettings();
            UltraGridColumn columnToSummarize = e.Layout.Bands[0].Columns[0];
            columnToSummarize = e.Layout.Bands[0].Columns[1];

            try
            {
                summary = e.Layout.Bands[0].Summaries.Add("Record(s) Count = ", SummaryType.Count, columnToSummarize);
            }
            catch { }
            summary.DisplayFormat = "Record(s) Count = {0}";
            summary.Appearance.TextHAlign = Infragistics.Win.HAlign.Left;
            summary.SummaryPosition = SummaryPosition.Left;
            summary.SummaryDisplayArea = SummaryDisplayAreas.BottomFixed;
            e.Layout.Bands[0].Summaries[0].SummaryPositionColumn = columnToSummarize;
            e.Layout.Bands[0].Override.SummaryFooterCaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            e.Layout.Override.SummaryDisplayArea = SummaryDisplayAreas.BottomFixed;

            e.Layout.Override.SummaryFooterAppearance.BackColor = Color.Silver;
            e.Layout.Override.SummaryValueAppearance.BackColor = Color.Silver;
            e.Layout.Override.SummaryValueAppearance.ForeColor = Color.Maroon;
            e.Layout.Override.SummaryValueAppearance.FontData.Bold = Infragistics.Win.DefaultableBoolean.True;

            e.Layout.Override.SummaryFooterSpacingAfter = 5;
            e.Layout.Override.SummaryFooterSpacingBefore = 5;
            #endregion
        }

        private void grdHistory_AfterRowsDeleted(object sender, System.EventArgs e)
        {
            try
            {
                if (CurrentID > 0)
                    this.oPhysicalInv.DeleteRow(CurrentID);
            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
            LoadHistory();
            if (this.grdHistory.Rows.Count > 0)
            {
                if (LastRow > this.grdHistory.Rows.Count - 1)
                {
                    this.grdHistory.ActiveRow = this.grdHistory.Rows[this.grdHistory.Rows.Count - 1];
                    this.grdHistory.ActiveRow.Selected = true;
                }
                else
                {
                    this.grdHistory.ActiveRow = this.grdHistory.Rows[LastRow];
                    this.grdHistory.ActiveRow.Selected = true;
                }
                this.grdHistory.Select();
            }
        }

        private void grdHistory_BeforeRowsDeleted(object sender, Infragistics.Win.UltraWinGrid.BeforeRowsDeletedEventArgs e)
        {
            if (this.grdHistory.ActiveRow != null)
            {
                CurrentID = Convert.ToInt32(this.grdHistory.ActiveRow.Cells[clsPOSDBConstants.PhysicalInv_Fld_ID].Value.ToString());
                LastRow = this.grdHistory.ActiveRow.Index;
            }
            else
            {
                CurrentID = 0;
                LastRow = 0;
            }
        }
        #endregion              

        #region Add physical inventory tab        
        private void SetNew()
        {
            Cnt = 0;
            oPhysicalInv = new PhysicalInv();
            oPhysicalInvData = new PhysicalInvData();
            this.grdPhysicalInventory.DataSource = oPhysicalInvData;    //PRIMEPOS-2395 13-Jun-2018 JY Added

            if (grdPhysicalInventory.Rows.Count == 0)
                grdPhysicalInventory.DisplayLayout.Bands[0].AddNew();

            this.grdPhysicalInventory.Focus();
            this.grdPhysicalInventory.Refresh();    //PRIMEPOS-2395 13-Jun-2018 JY Added
        }

        private void grdPhysicalInventory_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            #region dynamic record count 
            SummarySettings summary = new SummarySettings();
            UltraGridColumn columnToSummarize = e.Layout.Bands[0].Columns[0];
            columnToSummarize = e.Layout.Bands[0].Columns[1];

            try
            {
                summary = e.Layout.Bands[0].Summaries.Add("Record(s) Count = ", SummaryType.Count, columnToSummarize);
            }
            catch { }
            summary.DisplayFormat = "Record(s) Count = {0}";
            summary.Appearance.TextHAlign = Infragistics.Win.HAlign.Left;
            summary.SummaryPosition = SummaryPosition.Left;
            summary.SummaryDisplayArea = SummaryDisplayAreas.BottomFixed;
            e.Layout.Bands[0].Summaries[0].SummaryPositionColumn = columnToSummarize;
            e.Layout.Bands[0].Override.SummaryFooterCaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            e.Layout.Override.SummaryDisplayArea = SummaryDisplayAreas.BottomFixed;

            e.Layout.Override.SummaryFooterAppearance.BackColor = Color.Silver;
            e.Layout.Override.SummaryValueAppearance.BackColor = Color.Silver;
            e.Layout.Override.SummaryValueAppearance.ForeColor = Color.Maroon;
            e.Layout.Override.SummaryValueAppearance.FontData.Bold = Infragistics.Win.DefaultableBoolean.True;

            e.Layout.Override.SummaryFooterSpacingAfter = 5;
            e.Layout.Override.SummaryFooterSpacingBefore = 5;
            #endregion
        }

        private void grdPhysicalInventory_ClickCellButton(object sender, CellEventArgs e)
        {
            try
            {
                if (m_exceptionAccoured)
                {
                    m_exceptionAccoured = false;
                    return;
                }

                if (e.Cell.Column.Key == clsPOSDBConstants.PhysicalInv_Fld_ItemCode)
                {
                    SearchItem(Configuration.convertNullToString(this.grdPhysicalInventory.ActiveRow.Cells[clsPOSDBConstants.PhysicalInv_Fld_ItemCode].Value));
                }
            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }

        private void grdPhysicalInventory_Enter(object sender, EventArgs e)
        {
            try
            {
                if (this.grdPhysicalInventory.Rows.Count > 0)
                {
                    if (!this.grdPhysicalInventory.Rows[0].Cells[clsPOSDBConstants.PhysicalInv_Fld_NewQty].Activated)
                        this.grdPhysicalInventory.Rows[0].Cells[clsPOSDBConstants.PhysicalInv_Fld_ItemCode].Activate();

                    this.grdPhysicalInventory.PerformAction(UltraGridAction.EnterEditMode);
                }
                else
                {
                    this.grdPhysicalInventory.Rows.Band.AddNew();
                }
            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }

        private void grdPhysicalInventory_BeforeCellDeactivate(object sender, CancelEventArgs e)
        {
            UltraGridCell oCurrentCell;
            oCurrentCell = this.grdPhysicalInventory.ActiveCell;
            try
            {
                if (oCurrentCell.Column.Key == clsPOSDBConstants.PhysicalInv_Fld_ItemCode && oCurrentCell.Value.ToString() != "")
                {
                    FKEdit(oCurrentCell.Value.ToString(), clsPOSDBConstants.Item_tbl);
                    if (oCurrentCell.Value.ToString() == "")
                    {
                        e.Cancel = true;
                        this.grdPhysicalInventory.PerformAction(UltraGridAction.EnterEditMode);
                    }
                }
            }
            catch (Exception exp)
            {
                m_exceptionAccoured = true;
                clsUIHelper.ShowErrorMsg(exp.Message);
                e.Cancel = true;
                this.grdPhysicalInventory.PerformAction(UltraGridAction.EnterEditMode);
            }
        }

        private void grdPhysicalInventory_BeforeRowDeactivate(object sender, CancelEventArgs e)
        {
            UltraGridRow oCurrentRow;
            UltraGridCell oCurrentCell;
            oCurrentRow = this.grdPhysicalInventory.ActiveRow;
            oCurrentCell = null;
            bool blnCellChanged;
            blnCellChanged = false;
            bException = false;

            foreach (UltraGridCell oCell in oCurrentRow.Cells)
            {
                if (oCell.DataChanged == true || oCell.Text.Trim() != "")
                {
                    blnCellChanged = true;
                    break;
                }
            }

            if (blnCellChanged == false)
            {
                return;
            }
            try
            {
                oCurrentCell = oCurrentRow.Cells[clsPOSDBConstants.PhysicalInv_Fld_ItemCode];
                oPhysicalInv.Validate_ItemCode(oCurrentCell.Text.ToString());

                //oCurrentCell = oCurrentRow.Cells[clsPOSDBConstants.PhysicalInv_Fld_NewQty];   //PRIMEPOS-3040 21-Dec-2021 JY Commented
                //oPhysicalInv.Validate_Qty(oCurrentCell.Text.ToString());  //PRIMEPOS-3040 21-Dec-2021 JY Commented

                oCurrentCell = oCurrentRow.Cells[clsPOSDBConstants.PhysicalInv_Fld_NewSellingPrice];
                oPhysicalInv.Validate_Cost(oCurrentCell.Text.ToString());

                oCurrentCell = oCurrentRow.Cells[clsPOSDBConstants.PhysicalInv_Fld_NewCostPrice];
                oPhysicalInv.Validate_Cost(oCurrentCell.Text.ToString());
            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
                if (oCurrentCell != null)
                {
                    bException = true;
                    m_exceptionAccoured = true;
                    e.Cancel = true;
                    this.grdPhysicalInventory.ActiveCell = oCurrentCell;
                    this.grdPhysicalInventory.PerformAction(UltraGridAction.ActivateCell);
                    this.grdPhysicalInventory.PerformAction(UltraGridAction.EnterEditMode);
                }
            }
        }

        #region PRIMEPOS-2395 13-Jun-2018 JY Added
        private void ApplyPhysicalInventoryGridFormat()
        {
            clsUIHelper.SetAppearance(this.grdPhysicalInventory);
            //this.grdPhysicalInventory.DisplayLayout.Bands[0].Override.AllowColSizing = AllowColSizing.Free;
            //this.grdPhysicalInventory.DisplayLayout.Override.ColumnSizingArea = ColumnSizingArea.EntireColumn;

            this.grdPhysicalInventory.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.PhysicalInv_Fld_ItemCode].Header.Caption = "Item#";
            this.grdPhysicalInventory.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.Item_Fld_Description].Header.Caption = "Description";
            this.grdPhysicalInventory.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.InvRecvDetail_Fld_LastInvUpdatedQty].Header.Caption = "Last\nInv\nUpdated";
            this.grdPhysicalInventory.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.PhysicalInv_Fld_OldQty].Header.Caption = "Qty in\nHand";
            this.grdPhysicalInventory.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.PhysicalInv_Fld_NewQty].Header.Caption = "New\nQty";
            this.grdPhysicalInventory.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.PhysicalInv_Fld_OldSellingPrice].Header.Caption = "Old\nSelling\nPrice";
            this.grdPhysicalInventory.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.PhysicalInv_Fld_NewSellingPrice].Header.Caption = "New\nSelling\nPrice";
            this.grdPhysicalInventory.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.PhysicalInv_Fld_OldCostPrice].Header.Caption = "Old\nCost\nPrice";
            this.grdPhysicalInventory.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.PhysicalInv_Fld_NewCostPrice].Header.Caption = "New\nCost\nPrice";
            this.grdPhysicalInventory.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.PhysicalInv_Fld_OldExpDate].Header.Caption = "Old\nExp.Date";
            this.grdPhysicalInventory.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.PhysicalInv_Fld_NewExpDate].Header.Caption = "New\nExp.Date";

            this.grdPhysicalInventory.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.PhysicalInv_Fld_ItemCode].Width = 90;
            this.grdPhysicalInventory.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.Item_Fld_Description].Width = 120;
            this.grdPhysicalInventory.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.InvRecvDetail_Fld_LastInvUpdatedQty].Width = 100;
            this.grdPhysicalInventory.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.PhysicalInv_Fld_OldQty].Width = 100;
            this.grdPhysicalInventory.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.PhysicalInv_Fld_NewQty].Width = 100;
            this.grdPhysicalInventory.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.PhysicalInv_Fld_OldSellingPrice].Width = 100;
            this.grdPhysicalInventory.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.PhysicalInv_Fld_NewSellingPrice].Width = 100;
            this.grdPhysicalInventory.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.PhysicalInv_Fld_OldCostPrice].Width = 100;
            this.grdPhysicalInventory.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.PhysicalInv_Fld_NewCostPrice].Width = 100;
            this.grdPhysicalInventory.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.PhysicalInv_Fld_OldExpDate].Width = 120;
            this.grdPhysicalInventory.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.PhysicalInv_Fld_NewExpDate].Width = 120;

            this.grdPhysicalInventory.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.Item_Fld_Description].CellActivation = Activation.Disabled;
            this.grdPhysicalInventory.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.InvRecvDetail_Fld_LastInvUpdatedQty].CellActivation = Activation.Disabled;
            this.grdPhysicalInventory.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.PhysicalInv_Fld_OldQty].CellActivation = Activation.Disabled;
            this.grdPhysicalInventory.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.PhysicalInv_Fld_OldSellingPrice].CellActivation = Activation.Disabled;
            this.grdPhysicalInventory.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.PhysicalInv_Fld_OldCostPrice].CellActivation = Activation.Disabled;
            this.grdPhysicalInventory.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.PhysicalInv_Fld_OldExpDate].CellActivation = Activation.Disabled;

            #region PRIMEPOS-2464 10-Mar-2020 JY Added
            if (nDisplayItemCost == 0)
            {
                this.grdPhysicalInventory.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.PhysicalInv_Fld_OldCostPrice].Hidden = true;
                this.grdPhysicalInventory.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.PhysicalInv_Fld_NewCostPrice].Hidden = true;
            }
            #endregion

            //Added By Shitaljit(QuicSolv) on 18 Jan 2012
            //Logic To check whether login user have right to changes selling price and cost price.
            if (UserPriviliges.IsUserHasPriviliges(UserPriviliges.Modules.InventoryMgmt.ID, UserPriviliges.Screens.PhysicalInventory.ID, UserPriviliges.Permissions.InvPhysicalInvChangeSellingPrice.ID) == false)
            {
                grdPhysicalInventory.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.PhysicalInv_Fld_NewSellingPrice].CellActivation = Activation.Disabled;
            }
            if (UserPriviliges.IsUserHasPriviliges(UserPriviliges.Modules.InventoryMgmt.ID, UserPriviliges.Screens.PhysicalInventory.ID, UserPriviliges.Permissions.InvPhysicalInvChangeCostPrice.ID) == false)
            {
                grdPhysicalInventory.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.PhysicalInv_Fld_NewCostPrice].CellActivation = Activation.Disabled;
            }
            //End of Added by shitaljit(QuicSolv) on 18 Jan 2012
            //Sprint-21 - 2206 11-Mar-2016 JY Added
            if (UserPriviliges.IsUserHasPriviliges(UserPriviliges.Modules.InventoryMgmt.ID, UserPriviliges.Screens.PhysicalInventory.ID, UserPriviliges.Permissions.InvPhysicalInvChangeExpDate.ID) == false)
            {
                grdPhysicalInventory.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.PhysicalInv_Fld_NewExpDate].CellActivation = Activation.Disabled;
            }

            grdPhysicalInventory.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.PhysicalInv_Fld_OldQty].CellAppearance.TextHAlign = HAlign.Right;
            grdPhysicalInventory.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.PhysicalInv_Fld_OldQty].MaxValue = 99999;
            grdPhysicalInventory.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.PhysicalInv_Fld_OldQty].MinValue = 0;
            grdPhysicalInventory.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.PhysicalInv_Fld_OldQty].MaskInput = "99999";
            grdPhysicalInventory.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.PhysicalInv_Fld_OldQty].Format = "#####";

            grdPhysicalInventory.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.PhysicalInv_Fld_NewQty].CellAppearance.TextHAlign = HAlign.Right;
            grdPhysicalInventory.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.PhysicalInv_Fld_NewQty].MaxValue = 99999;
            grdPhysicalInventory.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.PhysicalInv_Fld_NewQty].MinValue = 0;
            grdPhysicalInventory.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.PhysicalInv_Fld_NewQty].MaskInput = "99999";
            grdPhysicalInventory.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.PhysicalInv_Fld_NewQty].Format = "####0";

            grdPhysicalInventory.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.PhysicalInv_Fld_OldSellingPrice].CellAppearance.TextHAlign = HAlign.Right;
            grdPhysicalInventory.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.PhysicalInv_Fld_OldSellingPrice].MaxValue = 99999.99;
            grdPhysicalInventory.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.PhysicalInv_Fld_OldSellingPrice].MaskInput = "99999.99";
            grdPhysicalInventory.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.PhysicalInv_Fld_OldSellingPrice].Format = "#####.00";

            grdPhysicalInventory.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.PhysicalInv_Fld_NewSellingPrice].CellAppearance.TextHAlign = HAlign.Right;
            grdPhysicalInventory.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.PhysicalInv_Fld_NewSellingPrice].MaxValue = 99999.99;
            grdPhysicalInventory.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.PhysicalInv_Fld_NewSellingPrice].MaskInput = "99999.99";
            grdPhysicalInventory.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.PhysicalInv_Fld_NewSellingPrice].Format = "#####.00";

            grdPhysicalInventory.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.PhysicalInv_Fld_OldCostPrice].CellAppearance.TextHAlign = HAlign.Right;
            grdPhysicalInventory.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.PhysicalInv_Fld_OldCostPrice].MaxValue = 99999.99;
            grdPhysicalInventory.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.PhysicalInv_Fld_OldCostPrice].MaskInput = "99999.99";
            grdPhysicalInventory.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.PhysicalInv_Fld_OldCostPrice].Format = "#####.00";

            grdPhysicalInventory.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.PhysicalInv_Fld_NewCostPrice].CellAppearance.TextHAlign = HAlign.Right;
            grdPhysicalInventory.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.PhysicalInv_Fld_NewCostPrice].MaxValue = 99999.99;
            grdPhysicalInventory.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.PhysicalInv_Fld_NewCostPrice].MaskInput = "99999.99";
            grdPhysicalInventory.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.PhysicalInv_Fld_NewCostPrice].Format = "#####.00";

            grdPhysicalInventory.DisplayLayout.Override.RowSelectors = DefaultableBoolean.False;
        }
        #endregion

        private void FKEdit(string code, string senderName, Boolean bMultiple = false)
        {
            if (bMultiple == false && senderName == clsPOSDBConstants.Item_tbl)
            {
                try
                {
                    Item oItem = new Item();
                    //ItemData oItemData = oItem.Populate(code);
                    //ItemRow oItemRow = oItemData.Item[0];
                    DataTable dtItem = oItem.GetItemDetails(code);  //PRIMEPOS-2395 22-Jun-2018 JY Added
                    if (dtItem != null && dtItem.Rows.Count > 0)    //PRIMEPOS-2395 22-Jun-2018 JY Added
                    {
                        if (grdPhysicalInventory.ActiveRow == null)
                            this.grdPhysicalInventory.Rows.Band.AddNew();

                        this.grdPhysicalInventory.ActiveCell.Value = Configuration.convertNullToString(dtItem.Rows[0]["ItemId"]);

                        this.grdPhysicalInventory.ActiveRow.Cells[clsPOSDBConstants.PhysicalInv_Fld_ID].Value = Cnt++;
                        this.grdPhysicalInventory.ActiveRow.Cells[clsPOSDBConstants.Item_Fld_Description].Value = Configuration.convertNullToString(dtItem.Rows[0]["Description"]);
                        this.grdPhysicalInventory.ActiveRow.Cells[clsPOSDBConstants.InvRecvDetail_Fld_LastInvUpdatedQty].Value = Configuration.convertNullToInt(dtItem.Rows[0]["LastInvUpdatedQty"]);   //PRIMEPOS-2396 11-Jun-2018 JY Added   
                        this.grdPhysicalInventory.ActiveRow.Cells[clsPOSDBConstants.PhysicalInv_Fld_OldQty].Value = Configuration.convertNullToInt(dtItem.Rows[0]["QtyInStock"]);    //PRIMEPOS-2396 11-Jun-2018 JY Commented
                        if (this.grdPhysicalInventory.ActiveRow.Cells[clsPOSDBConstants.PhysicalInv_Fld_NewQty].Value == System.DBNull.Value)
                            this.grdPhysicalInventory.ActiveRow.Cells[clsPOSDBConstants.PhysicalInv_Fld_NewQty].Value = 1;
                        this.grdPhysicalInventory.ActiveRow.Cells[clsPOSDBConstants.PhysicalInv_Fld_OldSellingPrice].Value = Configuration.convertNullToDecimal(dtItem.Rows[0]["SellingPrice"]);
                        this.grdPhysicalInventory.ActiveRow.Cells[clsPOSDBConstants.PhysicalInv_Fld_NewSellingPrice].Value = Configuration.convertNullToDecimal(dtItem.Rows[0]["SellingPrice"]);
                        this.grdPhysicalInventory.ActiveRow.Cells[clsPOSDBConstants.PhysicalInv_Fld_OldCostPrice].Value = Configuration.convertNullToDecimal(dtItem.Rows[0]["LastCostPrice"]);
                        this.grdPhysicalInventory.ActiveRow.Cells[clsPOSDBConstants.PhysicalInv_Fld_NewCostPrice].Value = Configuration.convertNullToDecimal(dtItem.Rows[0]["LastCostPrice"]);
                        this.grdPhysicalInventory.ActiveRow.Cells[clsPOSDBConstants.PhysicalInv_Fld_UserID].Value = Configuration.UserName;
                        this.grdPhysicalInventory.ActiveRow.Cells[clsPOSDBConstants.PhysicalInv_Fld_TransDate].Value = System.DateTime.Now;

                        try
                        {
                            this.grdPhysicalInventory.ActiveRow.Cells[clsPOSDBConstants.PhysicalInv_Fld_OldExpDate].Value = dtItem.Rows[0]["ExpDate"]; //Sprint-21 - 2206 11-Mar-2016 JY Added
                            this.grdPhysicalInventory.ActiveRow.Cells[clsPOSDBConstants.PhysicalInv_Fld_NewExpDate].Value = dtItem.Rows[0]["ExpDate"]; //Sprint-21 - 2206 11-Mar-2016 JY Added
                        }
                        catch
                        {
                            //Added try catch to avoid exception in case of NULL/invalid date
                        }
                    }
                }
                catch (System.IndexOutOfRangeException)
                {
                    this.grdPhysicalInventory.Focus();
                    SearchItem(Configuration.convertNullToString(this.grdPhysicalInventory.ActiveRow.Cells[clsPOSDBConstants.PhysicalInv_Fld_ItemCode].Value));
                }
                catch (Exception exp)
                {
                    clsUIHelper.ShowErrorMsg(exp.Message);
                    this.grdPhysicalInventory.Focus();
                    SearchItem(Configuration.convertNullToString(this.grdPhysicalInventory.ActiveRow.Cells[clsPOSDBConstants.PhysicalInv_Fld_ItemCode].Value));
                }
            }
            else if (bMultiple == true && senderName == clsPOSDBConstants.Item_tbl)
            {
                try
                {
                    Item oItem = new Item();
                    //ItemData oItemData = oItem.Populate(code);
                    //ItemRow oItemRow = oItemData.Item[0];
                    DataTable dtItem = oItem.GetItemDetails(code);  //PRIMEPOS-2395 22-Jun-2018 JY Added
                    if (dtItem != null && dtItem.Rows.Count > 0)    //PRIMEPOS-2395 22-Jun-2018 JY Added
                    {
                        oPhysicalInvRow = oPhysicalInvData.PhysicalInv.AddRow(Cnt++, Configuration.convertNullToString(dtItem.Rows[0]["ItemId"]), Configuration.convertNullToInt(dtItem.Rows[0]["QtyInStock"]), 1, Configuration.convertNullToDecimal(dtItem.Rows[0]["SellingPrice"]), Configuration.convertNullToDecimal(dtItem.Rows[0]["SellingPrice"]), Configuration.convertNullToDecimal(dtItem.Rows[0]["LastCostPrice"]), Configuration.convertNullToDecimal(dtItem.Rows[0]["LastCostPrice"]));
                        grdPhysicalInventory.Rows[grdPhysicalInventory.Rows.Count - 1].Activate();

                        this.grdPhysicalInventory.ActiveRow.Cells[clsPOSDBConstants.Item_Fld_Description].Value = Configuration.convertNullToString(dtItem.Rows[0]["Description"]);
                        this.grdPhysicalInventory.ActiveRow.Cells[clsPOSDBConstants.InvRecvDetail_Fld_LastInvUpdatedQty].Value = Configuration.convertNullToInt(dtItem.Rows[0]["LastInvUpdatedQty"]);   //PRIMEPOS-2396 11-Jun-2018 JY Added   
                        oPhysicalInvRow.UserID = Configuration.UserName;
                        oPhysicalInvRow.TransDate = System.DateTime.Now;
                        oPhysicalInvRow.isProcessed = false;    //PRIMEPOS-3121 12-Sep-2022 JY Added
                        try
                        {
                            this.grdPhysicalInventory.ActiveRow.Cells[clsPOSDBConstants.PhysicalInv_Fld_OldExpDate].Value = dtItem.Rows[0]["ExpDate"]; //Sprint-21 - 2206 11-Mar-2016 JY Added
                            this.grdPhysicalInventory.ActiveRow.Cells[clsPOSDBConstants.PhysicalInv_Fld_NewExpDate].Value = dtItem.Rows[0]["ExpDate"]; //Sprint-21 - 2206 11-Mar-2016 JY Added
                        }
                        catch
                        {
                            //Added try catch to avoid exception in case of NULL/invalid date
                        }
                    }
                }
                catch (System.IndexOutOfRangeException)
                {
                    this.grdPhysicalInventory.Focus();
                    SearchItem(Configuration.convertNullToString(this.grdPhysicalInventory.ActiveRow.Cells[clsPOSDBConstants.PhysicalInv_Fld_ItemCode].Value));
                }
                catch (Exception exp)
                {
                    clsUIHelper.ShowErrorMsg(exp.Message);
                    this.grdPhysicalInventory.Focus();
                    SearchItem(Configuration.convertNullToString(this.grdPhysicalInventory.ActiveRow.Cells[clsPOSDBConstants.PhysicalInv_Fld_ItemCode].Value));
                }
            }
        }

        private void SearchItem(string ItemCode)
        {
            try
            {
                frmSearchMain oSearch = new frmSearchMain(true);    //20-Dec-2017 JY Added new reference
                oSearch.SearchTable = clsPOSDBConstants.Item_tbl;  //20-Dec-2017 JY Added 
                oSearch.DefaultCode = ItemCode;
                oSearch.ShowDialog(this);
                if (!oSearch.IsCanceled)
                {
                    string strCode = oSearch.SelectedRowID();
                    if (strCode == "")
                        return;
                    FKEdit(strCode, clsPOSDBConstants.Item_tbl);
                }
            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }

        private void btnSave_Click(object sender, System.EventArgs e)
        {
            try
            {
                POS_Core.ErrorLogging.Logs.Logger(" **Start Physical Inventory");
                oPhysicalInv.Persist(oPhysicalInvData);
                POS_Core.ErrorLogging.Logs.Logger(" **End Physical Inventory");
                SetNew();
                LoadHistory();
                grdHistory.Focus();
            }
            catch (POSExceptions exp)
            {
                clsUIHelper.ShowErrorMsg(exp.ErrMessage);
                switch (exp.ErrNumber)
                {
                    case (long)POSErrorENUM.Item_CodeCanNotBeNULL:
                        this.grdPhysicalInventory.Focus();
                        break;
                }
            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (this.grdPhysicalInventory.Rows.Count > 0)
            {
                this.grdPhysicalInventory.ActiveRow.Delete();
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            SetNew();
        }

        private void btnAddItem_Click(object sender, EventArgs e)
        {
            AddMultipleItems();
        }

        private void AddMultipleItems()
        {
            try
            {
                frmSearchMain oSearch = new frmSearchMain(true);
                oSearch.SearchTable = clsPOSDBConstants.Item_tbl;
                oSearch.AllowMultiRowSelect = true;
                oSearch.SearchInConstructor = false;
                oSearch.ShowDialog(this);
                if (!oSearch.IsCanceled)
                {
                    string strItemId = string.Empty;
                    foreach (UltraGridRow oRow in oSearch.grdSearch.Rows)
                    {
                        if ((bool)oRow.Cells["check"].Value == true)
                        {
                            strItemId = oRow.Cells["Item Code"].Text.Trim().Replace("'", "''");
                            FKEdit(strItemId, clsPOSDBConstants.Item_tbl, true);
                        }
                    }
                }
            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }

        #endregion

        private void ultraTabPhyInv_SelectedTabChanged(object sender, Infragistics.Win.UltraWinTabControl.SelectedTabChangedEventArgs e)
        {
            if (ultraTabPhyInv.ActiveTab.Index == 0)
            {
                #region PRIMEPOS-2984 21-Jul-2021 JY Added
                btnClose.Text = "Next";
                btnClose.Appearance.BackColor = System.Drawing.Color.Green;
                btnClose.Appearance.BackColor2= System.Drawing.Color.Green;
                #endregion
                this.grdPhysicalInventory.Focus();
            }
            else
            {
                #region PRIMEPOS-2984 21-Jul-2021 JY Added
                btnClose.Text = "Close";    
                btnClose.Appearance.BackColor = Color.FromArgb(255, System.Convert.ToByte(POS_Core.Resources.Configuration.arrCloseBk[0].ToString()), System.Convert.ToByte(POS_Core.Resources.Configuration.arrCloseBk[1]), System.Convert.ToByte(POS_Core.Resources.Configuration.arrCloseBk[2]));
                btnClose.Appearance.BackColor2 = Color.FromArgb(255, System.Convert.ToByte(POS_Core.Resources.Configuration.arrCloseBk[0].ToString()), System.Convert.ToByte(POS_Core.Resources.Configuration.arrCloseBk[1]), System.Convert.ToByte(POS_Core.Resources.Configuration.arrCloseBk[2]));
                #endregion
                this.grdHistory.Focus();
            }
        }

        private void btnClose_Click(object sender, System.EventArgs e)
        {
            Infragistics.Win.UltraWinTabControl.UltraTabsCollection tabs = this.ultraTabPhyInv.Tabs;
            if (ultraTabPhyInv.ActiveTab.Index == 0)
            {
                this.ultraTabPhyInv.SelectedTab = tabs[1];
            }
            else
            {
                this.Close();                
            }
        }

        #region PRIMEPOS-2464 10-Mar-2020 JY Added
        public void tmrBlinkingTimedEvent(object source, ElapsedEventArgs e)
        {
            try
            {
                iBlinkCnt++;
                if (iBlinkCnt % 4 == 0)
                    lblMessage.Appearance.ForeColor = Color.Transparent;
                else
                    lblMessage.Appearance.ForeColor = Color.Red;
            }
            catch (Exception Ex)
            {
            }
        }
        #endregion

        #region code not in use
        #region Sprint-21 - 2206 11-Mar-2016 JY Added
        private void dtpExpiryDate_Validated(object sender, EventArgs e)
        {
            //try
            //{
            //    if (oPhysicalInvRow == null) return;

            //    if (dtpExpiryDate.Value.ToString() == "")
            //    {
            //        oPhysicalInvRow.NewExpDate = null;
            //        return;
            //    }
            //    oPhysicalInvRow.NewExpDate = Convert.ToDateTime(dtpExpiryDate.Value.ToString());
            //}
            //catch (Exception exp)
            //{
            //    clsUIHelper.ShowErrorMsg(exp.Message);
            //}
        }
        #endregion

        #endregion

        #region PRIMEPOS-2243 27-Apr-2021 JY Added
        private void btnPrintShelfStickers_Click(object sender, EventArgs e)
        {
            frmRptItemPriceLogLable ofrmRptItemPriceLogLable = new frmRptItemPriceLogLable();
            if (ultraTabPhyInv.ActiveTab.Index == 0)
            {
                if (grdPhysicalInventory.Rows.Count <= 0)
                    return;
                try
                {
                    string strItemID = string.Empty;
                    for (int i = 0; i < grdPhysicalInventory.Rows.Count; i++)
                    {
                        if (strItemID == string.Empty)
                            strItemID = "'" + Configuration.convertNullToString(grdPhysicalInventory.Rows[i].Cells[clsPOSDBConstants.PhysicalInv_Fld_ItemCode].Text).Replace("'", "''") + "'";
                        else
                            strItemID += ",'" + Configuration.convertNullToString(grdPhysicalInventory.Rows[i].Cells[clsPOSDBConstants.PhysicalInv_Fld_ItemCode].Text).Replace("'", "''") + "'";
                    }
                    ofrmRptItemPriceLogLable.ItemID = strItemID;
                    ofrmRptItemPriceLogLable.Show();
                }
                catch (Exception Ex)
                {
                }
            }
            else
            {
                if (grdHistory.Rows.Count <= 0)
                    return;
                try
                {
                    string strItemID = string.Empty;
                    for (int i = 0; i < grdHistory.Rows.Count; i++)
                    {
                        if (strItemID == string.Empty)
                            strItemID = "'" + Configuration.convertNullToString(grdHistory.Rows[i].Cells[clsPOSDBConstants.PhysicalInv_Fld_ItemCode].Text).Replace("'", "''") + "'";
                        else
                            strItemID += ",'" + Configuration.convertNullToString(grdHistory.Rows[i].Cells[clsPOSDBConstants.PhysicalInv_Fld_ItemCode].Text).Replace("'", "''") + "'";
                    }
                    ofrmRptItemPriceLogLable.ItemID = strItemID;
                    ofrmRptItemPriceLogLable.Show();
                }
                catch (Exception Ex)
                {
                }
            }
        }
        #endregion

        #region PRIMEPOS-2984 21-Jul-2021 JY Added
        bool bExecute = true;
        private void frmPhysicalInv_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (bExecute == true && e.CloseReason == CloseReason.UserClosing)
            {
                if (this.grdHistory.Rows.Count > 0)
                {
                    Infragistics.Win.UltraWinTabControl.UltraTabsCollection tabs = this.ultraTabPhyInv.Tabs;
                    if (Resources.Message.Display("Some inventory has not been processed. \n Are you sure you want to close?", "Confirmation", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        bExecute = false;
                        this.Close();
                    }
                    else
                    {
                        this.ultraTabPhyInv.SelectedTab = tabs[1];
                        e.Cancel = true;
                    }
                }
                else
                {
                    bExecute = false;
                    this.Close();
                }
            }
        }
        #endregion
    }
}