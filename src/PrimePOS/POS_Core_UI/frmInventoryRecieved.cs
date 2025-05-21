using System;
using Infragistics.Win.UltraWinGrid;
using Infragistics.Win;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using POS_Core.CommonData;
using POS_Core.BusinessRules;
using POS_Core.ErrorLogging;
using POS_Core.CommonData.Rows;
using Infragistics.Win.UltraWinMaskedEdit;
using System.Data;
//using POS_Core.DataAccess;    //Sprint-21 - 2002 21-Jul-2015 JY Added
using System.Collections.Generic;   //Sprint-22 - PRIMEPOS-2251 02-Dec-2015 JY Added
using POS_Core_UI.Reports.ReportsUI;
using POS_Core.Resources;
using System.Timers;
//using POS_Core_UI.Reports.ReportsUI;    //PrimePOS-2395 01-Aug-2018 JY Added

namespace POS_Core_UI
{
    /// <summary>
    /// Summary description for InventoryRecieved.
    /// </summary>
    public class frmInventoryRecieved : System.Windows.Forms.Form
    {
        private InvRecvHeaderData oInvRecvHData;
        private InvRecvHeaderRow oInvRecvHRow;
        private InvRecieved oInvRecv = new InvRecieved();
        private InvRecvDetailData oInvRecvDData;

        private bool m_exceptionAccoured = false;

        private Infragistics.Win.Misc.UltraLabel ultraLabel21;
        private Infragistics.Win.Misc.UltraLabel ultraLabel19;
        private Infragistics.Win.Misc.UltraLabel ultraLabel12;
        private Infragistics.Win.Misc.UltraLabel lblTransactionType;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor txtReference;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor txtVendor;
        private Infragistics.Win.UltraWinGrid.UltraGrid grdDetail;
        private Infragistics.Win.Misc.UltraLabel lblVendorName;
        private Infragistics.Win.Misc.UltraButton btnSave;
        private Infragistics.Win.Misc.UltraButton btnClose;
        private Infragistics.Win.Misc.UltraButton btnNew;
        private Infragistics.Win.UltraWinSchedule.UltraCalendarCombo dtpRecvDate;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.ToolTip toolTip1;
        private Infragistics.Win.Misc.UltraLabel ultraLabel7;
        private Infragistics.Win.Misc.UltraLabel ultraLabel1;
        public Infragistics.Win.Misc.UltraButton btnAddItem;
        public Infragistics.Win.Misc.UltraButton btnDeleteItem;
        public Infragistics.Win.Misc.UltraButton btnFillFromPO;
        private Infragistics.Win.UltraWinEditors.UltraComboEditor cboTransType;
        private Infragistics.Win.Misc.UltraLabel ultraLabel2;
        private System.ComponentModel.IContainer components;
        private POHeaderData poHeadData = null;
        private PODetailData poDetailData = null;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor txtEditorNoOfItems;
        private Infragistics.Win.Misc.UltraLabel ultraLblNoOfItems;
        private String inventoryWay = String.Empty;

        public static bool isFromPurchaseOrder = false;
        public static bool isInventoryRecieved = false;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor txtGrandTotalCost;
        private Infragistics.Win.Misc.UltraLabel ultraLabel3;
        //Added by SRT(Abhishek) Date : 25 Aug 2009
        private bool isFillFromPO = false;
        //End of Added by SRT(Abhishek)
        Dictionary<int, int> dctInvTransType = new Dictionary<int, int>(); //Sprint-22 - PRIMEPOS-2251 02-Dec-2015 JY Added
        private Infragistics.Win.UltraWinEditors.UltraDateTimeEditor ultraDateTimeEditor1;
        private Infragistics.Win.Misc.UltraPanel pnlNew;
        private Infragistics.Win.Misc.UltraLabel lblNew;
        private Infragistics.Win.Misc.UltraPanel pnlClose;
        private Infragistics.Win.Misc.UltraLabel lblClose;
        private Infragistics.Win.Misc.UltraPanel pnlSave;
        private Infragistics.Win.Misc.UltraLabel lblSave;
        private Infragistics.Win.Misc.UltraPanel pnlAddItem;
        private Infragistics.Win.Misc.UltraLabel lblAddItem;
        private Infragistics.Win.Misc.UltraPanel pnlFillFromPO;
        private Infragistics.Win.Misc.UltraLabel lblFillFromPO;
        private Infragistics.Win.Misc.UltraPanel pnlDeleteItem;
        private Infragistics.Win.Misc.UltraLabel lblDeleteItem;
        private Infragistics.Win.Misc.UltraPanel pnlReport;
        private Infragistics.Win.Misc.UltraButton btnReport;
        private Infragistics.Win.Misc.UltraLabel lblReport;

        bool bException = false;    //Sprint-26 - PRIMEPOS-665 30-Jun-2017 JY Added
        public Infragistics.Win.Misc.UltraLabel lblMessage;
        private const string strSubDept = "SubDept";

        #region PRIMEPOS-2464 10-Mar-2020 JY Added
        int nDisplayItemCost = 0;
        System.Timers.Timer tmrBlinking;
        private long iBlinkCnt = 0;
        #endregion
        private Infragistics.Win.Misc.UltraPanel pnlPrintShelfStickers;
        private Infragistics.Win.Misc.UltraButton btnPrintShelfStickers;
        private Infragistics.Win.Misc.UltraLabel lblPrintShelfStickers;
        public static int nSetTransTypeId = 0;   //PRIMEPOS-2901 10-Nov-2020 JY Added

        public frmInventoryRecieved()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();

            //
            // TODO: Add any constructor code after InitializeComponent call
            ///
        }

        //Following  constructor added by Krishna on 12April2011
        string ItemCode = "";
        bool RecInventoryFlag = false;
        bool VendoNullFlag = false;
        string VendorName = "";
        public frmInventoryRecieved(ItemRow objItemRow, string LastVendor)
        {
            InitializeComponent();
            RecInventoryFlag = true;
            Item oItem1 = new Item();
            ItemData oItemData1;
            ItemRow oItemRow1 = null;
            oItemData1 = oItem1.Populate(objItemRow.ItemID);
            oItemRow1 = oItemData1.Item[0];
            ItemCode = oItemRow1.ItemID;
            if (LastVendor == "")
                VendoNullFlag = true;
            VendorName = LastVendor;
        }
        //Till here added by Krishna

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
            Infragistics.Win.UltraWinEditors.EditorButton editorButton1 = new Infragistics.Win.UltraWinEditors.EditorButton();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmInventoryRecieved));
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinSchedule.CalendarCombo.DateButton dateButton1 = new Infragistics.Win.UltraWinSchedule.CalendarCombo.DateButton();
            Infragistics.Win.Appearance appearance7 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand1 = new Infragistics.Win.UltraWinGrid.UltraGridBand("Band 0", -1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn1 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("InvRecvDetailID");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn2 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("InvRecievedID");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn3 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("ItemID");
            Infragistics.Win.Appearance appearance8 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn4 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("VendorItemId");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn5 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Description");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn6 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("QtyOrdered");
            Infragistics.Win.Appearance appearance9 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn7 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Qty");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn8 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Cost");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn9 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("TotalCost");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn10 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("SalePrice");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn11 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Comments");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn12 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("VendorCode", 0);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn13 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("ExpDate", 1);
            Infragistics.Win.Appearance appearance10 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn14 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("QtyInStock", 2);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn15 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("LastInvUpdatedQty", 3);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn16 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("DeptName", 4);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn17 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("SubDept", 5);
            Infragistics.Win.Appearance appearance11 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn18 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("IsEBTItem", 6);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn19 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("DeptID", 7);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn20 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("SubDepartmentID", 8);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn21 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("DeptCode", 9);
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
            Infragistics.Win.Appearance appearance29 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance30 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance31 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinScrollBar.ScrollBarLook scrollBarLook1 = new Infragistics.Win.UltraWinScrollBar.ScrollBarLook();
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
            Infragistics.Win.Appearance appearance48 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance49 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance50 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance51 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance54 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance55 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance56 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance57 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance58 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance59 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance52 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance53 = new Infragistics.Win.Appearance();
            this.ultraDateTimeEditor1 = new Infragistics.Win.UltraWinEditors.UltraDateTimeEditor();
            this.ultraLabel21 = new Infragistics.Win.Misc.UltraLabel();
            this.txtReference = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.ultraLabel19 = new Infragistics.Win.Misc.UltraLabel();
            this.lblVendorName = new Infragistics.Win.Misc.UltraLabel();
            this.txtVendor = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.ultraLabel12 = new Infragistics.Win.Misc.UltraLabel();
            this.dtpRecvDate = new Infragistics.Win.UltraWinSchedule.UltraCalendarCombo();
            this.grdDetail = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.btnNew = new Infragistics.Win.Misc.UltraButton();
            this.btnClose = new Infragistics.Win.Misc.UltraButton();
            this.btnSave = new Infragistics.Win.Misc.UltraButton();
            this.lblTransactionType = new Infragistics.Win.Misc.UltraLabel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.ultraLabel2 = new Infragistics.Win.Misc.UltraLabel();
            this.cboTransType = new Infragistics.Win.UltraWinEditors.UltraComboEditor();
            this.ultraLabel1 = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabel7 = new Infragistics.Win.Misc.UltraLabel();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.pnlDeleteItem = new Infragistics.Win.Misc.UltraPanel();
            this.btnDeleteItem = new Infragistics.Win.Misc.UltraButton();
            this.lblDeleteItem = new Infragistics.Win.Misc.UltraLabel();
            this.pnlFillFromPO = new Infragistics.Win.Misc.UltraPanel();
            this.btnFillFromPO = new Infragistics.Win.Misc.UltraButton();
            this.lblFillFromPO = new Infragistics.Win.Misc.UltraLabel();
            this.pnlAddItem = new Infragistics.Win.Misc.UltraPanel();
            this.btnAddItem = new Infragistics.Win.Misc.UltraButton();
            this.lblAddItem = new Infragistics.Win.Misc.UltraLabel();
            this.txtGrandTotalCost = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.ultraLabel3 = new Infragistics.Win.Misc.UltraLabel();
            this.txtEditorNoOfItems = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.ultraLblNoOfItems = new Infragistics.Win.Misc.UltraLabel();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.pnlReport = new Infragistics.Win.Misc.UltraPanel();
            this.btnReport = new Infragistics.Win.Misc.UltraButton();
            this.lblReport = new Infragistics.Win.Misc.UltraLabel();
            this.pnlClose = new Infragistics.Win.Misc.UltraPanel();
            this.lblClose = new Infragistics.Win.Misc.UltraLabel();
            this.pnlSave = new Infragistics.Win.Misc.UltraPanel();
            this.lblSave = new Infragistics.Win.Misc.UltraLabel();
            this.pnlNew = new Infragistics.Win.Misc.UltraPanel();
            this.lblNew = new Infragistics.Win.Misc.UltraLabel();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.lblMessage = new Infragistics.Win.Misc.UltraLabel();
            this.pnlPrintShelfStickers = new Infragistics.Win.Misc.UltraPanel();
            this.btnPrintShelfStickers = new Infragistics.Win.Misc.UltraButton();
            this.lblPrintShelfStickers = new Infragistics.Win.Misc.UltraLabel();
            ((System.ComponentModel.ISupportInitialize)(this.ultraDateTimeEditor1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtReference)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtVendor)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtpRecvDate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdDetail)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cboTransType)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.pnlDeleteItem.ClientArea.SuspendLayout();
            this.pnlDeleteItem.SuspendLayout();
            this.pnlFillFromPO.ClientArea.SuspendLayout();
            this.pnlFillFromPO.SuspendLayout();
            this.pnlAddItem.ClientArea.SuspendLayout();
            this.pnlAddItem.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtGrandTotalCost)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtEditorNoOfItems)).BeginInit();
            this.groupBox3.SuspendLayout();
            this.pnlReport.ClientArea.SuspendLayout();
            this.pnlReport.SuspendLayout();
            this.pnlClose.ClientArea.SuspendLayout();
            this.pnlClose.SuspendLayout();
            this.pnlSave.ClientArea.SuspendLayout();
            this.pnlSave.SuspendLayout();
            this.pnlNew.ClientArea.SuspendLayout();
            this.pnlNew.SuspendLayout();
            this.pnlPrintShelfStickers.ClientArea.SuspendLayout();
            this.pnlPrintShelfStickers.SuspendLayout();
            this.SuspendLayout();
            // 
            // ultraDateTimeEditor1
            // 
            this.ultraDateTimeEditor1.AlwaysInEditMode = true;
            this.ultraDateTimeEditor1.AutoSize = false;
            this.ultraDateTimeEditor1.DateTime = new System.DateTime(2015, 3, 9, 0, 0, 0, 0);
            this.ultraDateTimeEditor1.Location = new System.Drawing.Point(791, 23);
            this.ultraDateTimeEditor1.MaskInput = "{LOC}mm/dd/yyyy";
            this.ultraDateTimeEditor1.MaxDate = new System.DateTime(2050, 12, 31, 0, 0, 0, 0);
            this.ultraDateTimeEditor1.Name = "ultraDateTimeEditor1";
            this.ultraDateTimeEditor1.Size = new System.Drawing.Size(130, 25);
            this.ultraDateTimeEditor1.TabIndex = 41;
            this.ultraDateTimeEditor1.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.ultraDateTimeEditor1.Value = new System.DateTime(2015, 3, 9, 0, 0, 0, 0);
            this.ultraDateTimeEditor1.Visible = false;
            // 
            // ultraLabel21
            // 
            appearance1.FontData.BoldAsString = "False";
            appearance1.ForeColor = System.Drawing.Color.Black;
            appearance1.ForeColorDisabled = System.Drawing.Color.Black;
            this.ultraLabel21.Appearance = appearance1;
            this.ultraLabel21.Location = new System.Drawing.Point(8, 57);
            this.ultraLabel21.Name = "ultraLabel21";
            this.ultraLabel21.Size = new System.Drawing.Size(80, 14);
            this.ultraLabel21.TabIndex = 4;
            this.ultraLabel21.Text = "Reference";
            this.ultraLabel21.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // txtReference
            // 
            this.txtReference.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.txtReference.Location = new System.Drawing.Point(104, 53);
            this.txtReference.Name = "txtReference";
            this.txtReference.Size = new System.Drawing.Size(344, 23);
            this.txtReference.TabIndex = 4;
            this.txtReference.Leave += new System.EventHandler(this.txtReference_Leave);
            // 
            // ultraLabel19
            // 
            appearance2.FontData.BoldAsString = "False";
            appearance2.ForeColor = System.Drawing.Color.Black;
            appearance2.ForeColorDisabled = System.Drawing.Color.Black;
            this.ultraLabel19.Appearance = appearance2;
            this.ultraLabel19.AutoSize = true;
            this.ultraLabel19.Location = new System.Drawing.Point(477, 25);
            this.ultraLabel19.Name = "ultraLabel19";
            this.ultraLabel19.Size = new System.Drawing.Size(104, 18);
            this.ultraLabel19.TabIndex = 6;
            this.ultraLabel19.Text = "Date Received";
            this.ultraLabel19.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // lblVendorName
            // 
            appearance3.ForeColor = System.Drawing.Color.White;
            this.lblVendorName.Appearance = appearance3;
            this.lblVendorName.BorderStyleOuter = Infragistics.Win.UIElementBorderStyle.Solid;
            this.lblVendorName.Location = new System.Drawing.Point(206, 23);
            this.lblVendorName.Name = "lblVendorName";
            this.lblVendorName.Size = new System.Drawing.Size(242, 23);
            this.lblVendorName.TabIndex = 3;
            // 
            // txtVendor
            // 
            this.txtVendor.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance4.BackColor = System.Drawing.Color.White;
            appearance4.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(231)))), ((int)(((byte)(230)))));
            appearance4.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance4.BorderAlpha = Infragistics.Win.Alpha.Transparent;
            appearance4.BorderColor3DBase = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            appearance4.Image = ((object)(resources.GetObject("appearance4.Image")));
            editorButton1.Appearance = appearance4;
            editorButton1.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Flat;
            editorButton1.Text = "";
            editorButton1.Width = 16;
            this.txtVendor.ButtonsRight.Add(editorButton1);
            this.txtVendor.Location = new System.Drawing.Point(104, 23);
            this.txtVendor.Name = "txtVendor";
            this.txtVendor.Size = new System.Drawing.Size(96, 23);
            this.txtVendor.TabIndex = 2;
            this.toolTip1.SetToolTip(this.txtVendor, "Press F4 To Search");
            this.txtVendor.ValueChanged += new System.EventHandler(this.txtVendor_ValueChanged);
            this.txtVendor.EditorButtonClick += new Infragistics.Win.UltraWinEditors.EditorButtonEventHandler(this.txtVendor_EditorButtonClick);
            this.txtVendor.Leave += new System.EventHandler(this.txtVendor_Leave);
            // 
            // ultraLabel12
            // 
            appearance5.FontData.BoldAsString = "False";
            appearance5.ForeColor = System.Drawing.Color.Black;
            appearance5.ForeColorDisabled = System.Drawing.Color.Black;
            this.ultraLabel12.Appearance = appearance5;
            this.ultraLabel12.Location = new System.Drawing.Point(8, 27);
            this.ultraLabel12.Name = "ultraLabel12";
            this.ultraLabel12.Size = new System.Drawing.Size(64, 14);
            this.ultraLabel12.TabIndex = 1;
            this.ultraLabel12.Text = "Vendor";
            this.ultraLabel12.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // dtpRecvDate
            // 
            this.dtpRecvDate.AllowNull = false;
            appearance6.FontData.BoldAsString = "False";
            this.dtpRecvDate.Appearance = appearance6;
            this.dtpRecvDate.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Office2003ToolbarButton;
            this.dtpRecvDate.DateButtons.Add(dateButton1);
            this.dtpRecvDate.Location = new System.Drawing.Point(587, 23);
            this.dtpRecvDate.Name = "dtpRecvDate";
            this.dtpRecvDate.NonAutoSizeHeight = 10;
            this.dtpRecvDate.Size = new System.Drawing.Size(192, 22);
            this.dtpRecvDate.TabIndex = 3;
            this.dtpRecvDate.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.dtpRecvDate.Value = new System.DateTime(2004, 5, 25, 0, 0, 0, 0);
            // 
            // grdDetail
            // 
            appearance7.BackColor = System.Drawing.Color.White;
            appearance7.BackColor2 = System.Drawing.Color.White;
            appearance7.BackColorDisabled = System.Drawing.Color.White;
            appearance7.BackColorDisabled2 = System.Drawing.Color.White;
            appearance7.BackGradientStyle = Infragistics.Win.GradientStyle.None;
            this.grdDetail.DisplayLayout.Appearance = appearance7;
            ultraGridColumn1.Header.VisiblePosition = 0;
            ultraGridColumn1.Hidden = true;
            ultraGridColumn1.RowLayoutColumnInfo.OriginX = 0;
            ultraGridColumn1.RowLayoutColumnInfo.OriginY = 0;
            ultraGridColumn1.RowLayoutColumnInfo.SpanX = 2;
            ultraGridColumn1.RowLayoutColumnInfo.SpanY = 2;
            ultraGridColumn1.Width = 122;
            ultraGridColumn2.Header.VisiblePosition = 1;
            ultraGridColumn2.Hidden = true;
            ultraGridColumn2.RowLayoutColumnInfo.OriginX = 2;
            ultraGridColumn2.RowLayoutColumnInfo.OriginY = 0;
            ultraGridColumn2.RowLayoutColumnInfo.SpanX = 2;
            ultraGridColumn2.RowLayoutColumnInfo.SpanY = 2;
            ultraGridColumn2.Width = 125;
            ultraGridColumn3.ButtonDisplayStyle = Infragistics.Win.UltraWinGrid.ButtonDisplayStyle.Always;
            appearance8.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(231)))), ((int)(((byte)(231)))));
            appearance8.BackColor2 = System.Drawing.Color.White;
            appearance8.BackGradientStyle = Infragistics.Win.GradientStyle.None;
            appearance8.Image = ((object)(resources.GetObject("appearance8.Image")));
            ultraGridColumn3.CellButtonAppearance = appearance8;
            ultraGridColumn3.Header.VisiblePosition = 2;
            ultraGridColumn3.RowLayoutColumnInfo.OriginX = 0;
            ultraGridColumn3.RowLayoutColumnInfo.OriginY = 0;
            ultraGridColumn3.RowLayoutColumnInfo.PreferredCellSize = new System.Drawing.Size(142, 0);
            ultraGridColumn3.RowLayoutColumnInfo.SpanX = 2;
            ultraGridColumn3.RowLayoutColumnInfo.SpanY = 2;
            ultraGridColumn3.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.EditButton;
            ultraGridColumn3.Width = 100;
            ultraGridColumn4.Header.Caption = "VenderItemId";
            ultraGridColumn4.Header.VisiblePosition = 3;
            ultraGridColumn4.RowLayoutColumnInfo.OriginX = 2;
            ultraGridColumn4.RowLayoutColumnInfo.OriginY = 0;
            ultraGridColumn4.RowLayoutColumnInfo.PreferredCellSize = new System.Drawing.Size(127, 0);
            ultraGridColumn4.RowLayoutColumnInfo.SpanX = 2;
            ultraGridColumn4.RowLayoutColumnInfo.SpanY = 2;
            ultraGridColumn4.Width = 100;
            ultraGridColumn5.Header.VisiblePosition = 4;
            ultraGridColumn5.RowLayoutColumnInfo.OriginX = 4;
            ultraGridColumn5.RowLayoutColumnInfo.OriginY = 0;
            ultraGridColumn5.RowLayoutColumnInfo.PreferredCellSize = new System.Drawing.Size(146, 0);
            ultraGridColumn5.RowLayoutColumnInfo.SpanX = 2;
            ultraGridColumn5.RowLayoutColumnInfo.SpanY = 2;
            ultraGridColumn5.Width = 120;
            appearance9.TextHAlignAsString = "Left";
            ultraGridColumn6.CellAppearance = appearance9;
            ultraGridColumn6.Header.Caption = "Ordered";
            ultraGridColumn6.Header.VisiblePosition = 9;
            ultraGridColumn6.RowLayoutColumnInfo.OriginX = 16;
            ultraGridColumn6.RowLayoutColumnInfo.OriginY = 0;
            ultraGridColumn6.RowLayoutColumnInfo.PreferredCellSize = new System.Drawing.Size(87, 0);
            ultraGridColumn6.RowLayoutColumnInfo.SpanX = 2;
            ultraGridColumn6.RowLayoutColumnInfo.SpanY = 2;
            ultraGridColumn7.Header.Caption = "Qty Ack.";
            ultraGridColumn7.Header.VisiblePosition = 10;
            ultraGridColumn7.RowLayoutColumnInfo.OriginX = 18;
            ultraGridColumn7.RowLayoutColumnInfo.OriginY = 0;
            ultraGridColumn7.RowLayoutColumnInfo.PreferredCellSize = new System.Drawing.Size(70, 0);
            ultraGridColumn7.RowLayoutColumnInfo.SpanX = 2;
            ultraGridColumn7.RowLayoutColumnInfo.SpanY = 2;
            ultraGridColumn8.Header.Caption = "Unit Cost";
            ultraGridColumn8.Header.VisiblePosition = 11;
            ultraGridColumn8.RowLayoutColumnInfo.OriginX = 20;
            ultraGridColumn8.RowLayoutColumnInfo.OriginY = 0;
            ultraGridColumn8.RowLayoutColumnInfo.PreferredCellSize = new System.Drawing.Size(80, 0);
            ultraGridColumn8.RowLayoutColumnInfo.SpanX = 2;
            ultraGridColumn8.RowLayoutColumnInfo.SpanY = 2;
            ultraGridColumn9.Header.Caption = "Total Cost";
            ultraGridColumn9.Header.VisiblePosition = 13;
            ultraGridColumn9.RowLayoutColumnInfo.OriginX = 22;
            ultraGridColumn9.RowLayoutColumnInfo.OriginY = 0;
            ultraGridColumn9.RowLayoutColumnInfo.PreferredCellSize = new System.Drawing.Size(90, 0);
            ultraGridColumn9.RowLayoutColumnInfo.SpanX = 2;
            ultraGridColumn9.RowLayoutColumnInfo.SpanY = 2;
            ultraGridColumn10.Header.VisiblePosition = 14;
            ultraGridColumn10.RowLayoutColumnInfo.OriginX = 24;
            ultraGridColumn10.RowLayoutColumnInfo.OriginY = 0;
            ultraGridColumn10.RowLayoutColumnInfo.PreferredCellSize = new System.Drawing.Size(80, 0);
            ultraGridColumn10.RowLayoutColumnInfo.SpanX = 2;
            ultraGridColumn10.RowLayoutColumnInfo.SpanY = 2;
            ultraGridColumn11.Header.VisiblePosition = 17;
            ultraGridColumn11.RowLayoutColumnInfo.OriginX = 30;
            ultraGridColumn11.RowLayoutColumnInfo.OriginY = 0;
            ultraGridColumn11.RowLayoutColumnInfo.SpanX = 2;
            ultraGridColumn11.RowLayoutColumnInfo.SpanY = 2;
            ultraGridColumn11.Width = 100;
            ultraGridColumn12.Header.VisiblePosition = 12;
            ultraGridColumn12.Hidden = true;
            ultraGridColumn13.ButtonDisplayStyle = Infragistics.Win.UltraWinGrid.ButtonDisplayStyle.Always;
            appearance10.TextHAlignAsString = "Left";
            ultraGridColumn13.CellAppearance = appearance10;
            ultraGridColumn13.EditorComponent = this.ultraDateTimeEditor1;
            ultraGridColumn13.Header.VisiblePosition = 15;
            ultraGridColumn13.RowLayoutColumnInfo.OriginX = 26;
            ultraGridColumn13.RowLayoutColumnInfo.OriginY = 0;
            ultraGridColumn13.RowLayoutColumnInfo.SpanX = 2;
            ultraGridColumn13.RowLayoutColumnInfo.SpanY = 2;
            ultraGridColumn14.Header.VisiblePosition = 7;
            ultraGridColumn14.RowLayoutColumnInfo.OriginX = 12;
            ultraGridColumn14.RowLayoutColumnInfo.OriginY = 0;
            ultraGridColumn14.RowLayoutColumnInfo.PreferredCellSize = new System.Drawing.Size(56, 0);
            ultraGridColumn14.RowLayoutColumnInfo.SpanX = 2;
            ultraGridColumn14.RowLayoutColumnInfo.SpanY = 2;
            ultraGridColumn15.Header.VisiblePosition = 8;
            ultraGridColumn15.RowLayoutColumnInfo.OriginX = 14;
            ultraGridColumn15.RowLayoutColumnInfo.OriginY = 0;
            ultraGridColumn15.RowLayoutColumnInfo.PreferredCellSize = new System.Drawing.Size(71, 0);
            ultraGridColumn15.RowLayoutColumnInfo.SpanX = 2;
            ultraGridColumn15.RowLayoutColumnInfo.SpanY = 2;
            ultraGridColumn16.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit;
            ultraGridColumn16.Header.VisiblePosition = 5;
            ultraGridColumn16.RowLayoutColumnInfo.OriginX = 8;
            ultraGridColumn16.RowLayoutColumnInfo.OriginY = 0;
            ultraGridColumn16.RowLayoutColumnInfo.SpanX = 2;
            ultraGridColumn16.RowLayoutColumnInfo.SpanY = 2;
            ultraGridColumn16.Width = 132;
            ultraGridColumn17.ButtonDisplayStyle = Infragistics.Win.UltraWinGrid.ButtonDisplayStyle.Always;
            appearance11.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(231)))), ((int)(((byte)(231)))));
            appearance11.BackColor2 = System.Drawing.Color.White;
            appearance11.BackGradientStyle = Infragistics.Win.GradientStyle.None;
            appearance11.Image = global::POS_Core_UI.Properties.Resources.search;
            ultraGridColumn17.CellButtonAppearance = appearance11;
            ultraGridColumn17.Header.VisiblePosition = 6;
            ultraGridColumn17.RowLayoutColumnInfo.OriginX = 10;
            ultraGridColumn17.RowLayoutColumnInfo.OriginY = 0;
            ultraGridColumn17.RowLayoutColumnInfo.SpanX = 2;
            ultraGridColumn17.RowLayoutColumnInfo.SpanY = 2;
            ultraGridColumn17.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.EditButton;
            ultraGridColumn17.Width = 120;
            ultraGridColumn18.Header.VisiblePosition = 16;
            ultraGridColumn18.RowLayoutColumnInfo.OriginX = 28;
            ultraGridColumn18.RowLayoutColumnInfo.OriginY = 0;
            ultraGridColumn18.RowLayoutColumnInfo.SpanX = 2;
            ultraGridColumn18.RowLayoutColumnInfo.SpanY = 2;
            ultraGridColumn18.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.CheckBox;
            ultraGridColumn19.Header.VisiblePosition = 18;
            ultraGridColumn19.Hidden = true;
            ultraGridColumn20.Header.VisiblePosition = 19;
            ultraGridColumn20.Hidden = true;
            ultraGridColumn21.ButtonDisplayStyle = Infragistics.Win.UltraWinGrid.ButtonDisplayStyle.Always;
            appearance12.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(231)))), ((int)(((byte)(231)))));
            appearance12.BackColor2 = System.Drawing.Color.White;
            appearance12.BackGradientStyle = Infragistics.Win.GradientStyle.None;
            appearance12.Image = global::POS_Core_UI.Properties.Resources.search;
            ultraGridColumn21.CellButtonAppearance = appearance12;
            ultraGridColumn21.Header.VisiblePosition = 20;
            ultraGridColumn21.RowLayoutColumnInfo.OriginX = 6;
            ultraGridColumn21.RowLayoutColumnInfo.OriginY = 0;
            ultraGridColumn21.RowLayoutColumnInfo.SpanX = 2;
            ultraGridColumn21.RowLayoutColumnInfo.SpanY = 2;
            ultraGridColumn21.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.EditButton;
            ultraGridColumn21.Width = 80;
            ultraGridBand1.Columns.AddRange(new object[] {
            ultraGridColumn1,
            ultraGridColumn2,
            ultraGridColumn3,
            ultraGridColumn4,
            ultraGridColumn5,
            ultraGridColumn6,
            ultraGridColumn7,
            ultraGridColumn8,
            ultraGridColumn9,
            ultraGridColumn10,
            ultraGridColumn11,
            ultraGridColumn12,
            ultraGridColumn13,
            ultraGridColumn14,
            ultraGridColumn15,
            ultraGridColumn16,
            ultraGridColumn17,
            ultraGridColumn18,
            ultraGridColumn19,
            ultraGridColumn20,
            ultraGridColumn21});
            ultraGridBand1.RowLayoutStyle = Infragistics.Win.UltraWinGrid.RowLayoutStyle.ColumnLayout;
            this.grdDetail.DisplayLayout.BandsSerializer.Add(ultraGridBand1);
            this.grdDetail.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.grdDetail.DisplayLayout.InterBandSpacing = 10;
            appearance13.BackColor = System.Drawing.Color.White;
            appearance13.BackColor2 = System.Drawing.Color.White;
            this.grdDetail.DisplayLayout.Override.ActiveCardCaptionAppearance = appearance13;
            appearance14.BackColor = System.Drawing.Color.White;
            appearance14.BackColor2 = System.Drawing.Color.White;
            appearance14.BorderColor = System.Drawing.Color.Gray;
            this.grdDetail.DisplayLayout.Override.ActiveRowAppearance = appearance14;
            appearance15.BackColor = System.Drawing.Color.White;
            appearance15.BackColor2 = System.Drawing.Color.White;
            appearance15.BorderColor = System.Drawing.Color.Gray;
            this.grdDetail.DisplayLayout.Override.AddRowAppearance = appearance15;
            this.grdDetail.DisplayLayout.Override.AllowAddNew = Infragistics.Win.UltraWinGrid.AllowAddNew.TemplateOnBottom;
            this.grdDetail.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.True;
            this.grdDetail.DisplayLayout.Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.True;
            appearance16.BackColor = System.Drawing.Color.Transparent;
            this.grdDetail.DisplayLayout.Override.CardAreaAppearance = appearance16;
            appearance17.BackColor = System.Drawing.Color.White;
            appearance17.BackColor2 = System.Drawing.Color.White;
            appearance17.BackColorDisabled = System.Drawing.Color.White;
            appearance17.BackColorDisabled2 = System.Drawing.Color.White;
            appearance17.BorderColor = System.Drawing.Color.Black;
            appearance17.BorderColor3DBase = System.Drawing.Color.Black;
            this.grdDetail.DisplayLayout.Override.CellAppearance = appearance17;
            appearance18.BackColor = System.Drawing.Color.White;
            appearance18.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(231)))), ((int)(((byte)(230)))));
            appearance18.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance18.BorderAlpha = Infragistics.Win.Alpha.Transparent;
            appearance18.BorderColor = System.Drawing.Color.Gray;
            appearance18.BorderColor3DBase = System.Drawing.Color.Gray;
            appearance18.ImageBackgroundAlpha = Infragistics.Win.Alpha.Transparent;
            appearance18.ImageBackgroundStyle = Infragistics.Win.ImageBackgroundStyle.Centered;
            this.grdDetail.DisplayLayout.Override.CellButtonAppearance = appearance18;
            appearance19.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            appearance19.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.grdDetail.DisplayLayout.Override.EditCellAppearance = appearance19;
            appearance20.BorderColor = System.Drawing.Color.Gray;
            this.grdDetail.DisplayLayout.Override.FilteredInRowAppearance = appearance20;
            appearance21.BorderColor = System.Drawing.Color.Gray;
            this.grdDetail.DisplayLayout.Override.FilteredOutRowAppearance = appearance21;
            appearance22.BackColor = System.Drawing.Color.White;
            appearance22.BackColor2 = System.Drawing.Color.White;
            appearance22.BackColorDisabled = System.Drawing.Color.White;
            appearance22.BackColorDisabled2 = System.Drawing.Color.White;
            this.grdDetail.DisplayLayout.Override.FixedCellAppearance = appearance22;
            appearance23.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            appearance23.BackColor2 = System.Drawing.Color.Beige;
            this.grdDetail.DisplayLayout.Override.FixedHeaderAppearance = appearance23;
            appearance24.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(62)))), ((int)(((byte)(176)))));
            appearance24.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance24.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance24.FontData.BoldAsString = "True";
            appearance24.FontData.SizeInPoints = 10F;
            appearance24.ForeColor = System.Drawing.Color.White;
            appearance24.TextHAlignAsString = "Left";
            appearance24.ThemedElementAlpha = Infragistics.Win.Alpha.Transparent;
            this.grdDetail.DisplayLayout.Override.HeaderAppearance = appearance24;
            appearance25.BorderColor = System.Drawing.Color.Gray;
            this.grdDetail.DisplayLayout.Override.RowAlternateAppearance = appearance25;
            appearance26.BackColor = System.Drawing.Color.White;
            appearance26.BackColor2 = System.Drawing.Color.White;
            appearance26.BackGradientStyle = Infragistics.Win.GradientStyle.None;
            appearance26.BackHatchStyle = Infragistics.Win.BackHatchStyle.None;
            appearance26.BorderColor = System.Drawing.Color.Gray;
            this.grdDetail.DisplayLayout.Override.RowAppearance = appearance26;
            appearance27.BorderColor = System.Drawing.Color.Gray;
            this.grdDetail.DisplayLayout.Override.RowPreviewAppearance = appearance27;
            appearance28.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(62)))), ((int)(((byte)(176)))));
            appearance28.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(62)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance28.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance28.BorderColor = System.Drawing.Color.Gray;
            appearance28.ForeColor = System.Drawing.Color.White;
            this.grdDetail.DisplayLayout.Override.RowSelectorAppearance = appearance28;
            this.grdDetail.DisplayLayout.Override.RowSelectorWidth = 12;
            this.grdDetail.DisplayLayout.Override.RowSpacingBefore = 2;
            appearance29.BackColor = System.Drawing.Color.Navy;
            appearance29.BackColorAlpha = Infragistics.Win.Alpha.Opaque;
            this.grdDetail.DisplayLayout.Override.SelectedCellAppearance = appearance29;
            appearance30.BackColor = System.Drawing.Color.Navy;
            appearance30.BackColorDisabled = System.Drawing.Color.Navy;
            appearance30.BackGradientStyle = Infragistics.Win.GradientStyle.None;
            appearance30.BackHatchStyle = Infragistics.Win.BackHatchStyle.None;
            appearance30.BorderColor = System.Drawing.Color.Gray;
            appearance30.ForeColor = System.Drawing.Color.White;
            this.grdDetail.DisplayLayout.Override.SelectedRowAppearance = appearance30;
            appearance31.BorderColor = System.Drawing.Color.Gray;
            this.grdDetail.DisplayLayout.Override.TemplateAddRowAppearance = appearance31;
            this.grdDetail.DisplayLayout.RowConnectorColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            this.grdDetail.DisplayLayout.RowConnectorStyle = Infragistics.Win.UltraWinGrid.RowConnectorStyle.Solid;
            appearance32.ImageBackgroundOrigin = Infragistics.Win.ImageBackgroundOrigin.Form;
            scrollBarLook1.TrackAppearance = appearance32;
            this.grdDetail.DisplayLayout.ScrollBarLook = scrollBarLook1;
            this.grdDetail.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.grdDetail.Location = new System.Drawing.Point(6, 23);
            this.grdDetail.Name = "grdDetail";
            this.grdDetail.Size = new System.Drawing.Size(1267, 266);
            this.grdDetail.TabIndex = 0;
            this.grdDetail.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.grdDetail.AfterCellUpdate += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.grdDetail_AfterCellUpdate);
            this.grdDetail.BeforeRowUpdate += new Infragistics.Win.UltraWinGrid.CancelableRowEventHandler(this.grdDetail_BeforeRowUpdate);
            this.grdDetail.ClickCellButton += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.grdDetail_ClickCellButton);
            this.grdDetail.BeforeCellDeactivate += new System.ComponentModel.CancelEventHandler(this.grdDetail_BeforeCellDeactivate);
            this.grdDetail.BeforeRowDeactivate += new System.ComponentModel.CancelEventHandler(this.ValidateRow);
            this.grdDetail.Enter += new System.EventHandler(this.grdDetail_Enter);
            // 
            // btnNew
            // 
            appearance33.BackColor = System.Drawing.Color.White;
            appearance33.ForeColor = System.Drawing.Color.Black;
            this.btnNew.Appearance = appearance33;
            this.btnNew.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Flat;
            this.btnNew.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnNew.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnNew.Location = new System.Drawing.Point(60, 0);
            this.btnNew.Name = "btnNew";
            this.btnNew.Size = new System.Drawing.Size(115, 30);
            this.btnNew.TabIndex = 11;
            this.btnNew.Text = "Clear";
            this.btnNew.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnNew.ContextMenuChanged += new System.EventHandler(this.btnNew_ContextMenuChanged);
            this.btnNew.Click += new System.EventHandler(this.btnNew_Click);
            this.btnNew.Enter += new System.EventHandler(this.Button_Enter);
            this.btnNew.Leave += new System.EventHandler(this.Button_Leave);
            // 
            // btnClose
            // 
            appearance34.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(62)))), ((int)(((byte)(76)))));
            appearance34.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(62)))), ((int)(((byte)(76)))));
            appearance34.ForeColor = System.Drawing.Color.Black;
            this.btnClose.Appearance = appearance34;
            this.btnClose.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Flat;
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnClose.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClose.Location = new System.Drawing.Point(60, 0);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(80, 30);
            this.btnClose.TabIndex = 12;
            this.btnClose.Text = "Close";
            this.btnClose.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnSave
            // 
            appearance35.BackColor = System.Drawing.Color.White;
            appearance35.ForeColor = System.Drawing.Color.Black;
            this.btnSave.Appearance = appearance35;
            this.btnSave.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Flat;
            this.btnSave.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnSave.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSave.Location = new System.Drawing.Point(60, 0);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(80, 30);
            this.btnSave.TabIndex = 9;
            this.btnSave.Text = "Save";
            this.btnSave.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            this.btnSave.Enter += new System.EventHandler(this.Button_Enter);
            this.btnSave.Leave += new System.EventHandler(this.Button_Leave);
            // 
            // lblTransactionType
            // 
            appearance36.BackColor = System.Drawing.Color.DeepSkyBlue;
            appearance36.BackColor2 = System.Drawing.Color.Azure;
            appearance36.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance36.ForeColor = System.Drawing.Color.Navy;
            appearance36.ForeColorDisabled = System.Drawing.Color.Navy;
            appearance36.TextHAlignAsString = "Center";
            this.lblTransactionType.Appearance = appearance36;
            this.lblTransactionType.BackColorInternal = System.Drawing.Color.Transparent;
            this.lblTransactionType.BorderStyleOuter = Infragistics.Win.UIElementBorderStyle.Solid;
            this.lblTransactionType.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblTransactionType.Font = new System.Drawing.Font("Verdana", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTransactionType.Location = new System.Drawing.Point(0, 0);
            this.lblTransactionType.Name = "lblTransactionType";
            this.lblTransactionType.Size = new System.Drawing.Size(1284, 30);
            this.lblTransactionType.TabIndex = 21;
            this.lblTransactionType.Tag = "Header";
            this.lblTransactionType.Text = "Inventory Received";
            this.lblTransactionType.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.ultraDateTimeEditor1);
            this.groupBox1.Controls.Add(this.ultraLabel2);
            this.groupBox1.Controls.Add(this.cboTransType);
            this.groupBox1.Controls.Add(this.ultraLabel1);
            this.groupBox1.Controls.Add(this.ultraLabel7);
            this.groupBox1.Controls.Add(this.txtReference);
            this.groupBox1.Controls.Add(this.lblVendorName);
            this.groupBox1.Controls.Add(this.txtVendor);
            this.groupBox1.Controls.Add(this.ultraLabel12);
            this.groupBox1.Controls.Add(this.dtpRecvDate);
            this.groupBox1.Controls.Add(this.ultraLabel21);
            this.groupBox1.Controls.Add(this.ultraLabel19);
            this.groupBox1.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.groupBox1.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.ForeColor = System.Drawing.Color.White;
            this.groupBox1.Location = new System.Drawing.Point(9, 35);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1283, 91);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Inventory Received Info";
            // 
            // ultraLabel2
            // 
            appearance37.FontData.BoldAsString = "False";
            appearance37.ForeColor = System.Drawing.Color.Black;
            appearance37.ForeColorDisabled = System.Drawing.Color.Black;
            this.ultraLabel2.Appearance = appearance37;
            this.ultraLabel2.AutoSize = true;
            this.ultraLabel2.Location = new System.Drawing.Point(477, 55);
            this.ultraLabel2.Name = "ultraLabel2";
            this.ultraLabel2.Size = new System.Drawing.Size(82, 18);
            this.ultraLabel2.TabIndex = 40;
            this.ultraLabel2.Text = "Trans Type";
            this.ultraLabel2.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // cboTransType
            // 
            appearance38.ForeColor = System.Drawing.Color.Black;
            this.cboTransType.Appearance = appearance38;
            this.cboTransType.AutoSize = false;
            this.cboTransType.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance39.BackColor = System.Drawing.Color.WhiteSmoke;
            appearance39.BackColor2 = System.Drawing.Color.Silver;
            appearance39.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            this.cboTransType.ButtonAppearance = appearance39;
            this.cboTransType.ButtonStyle = Infragistics.Win.UIElementButtonStyle.FlatBorderless;
            this.cboTransType.DropDownStyle = Infragistics.Win.DropDownStyle.DropDownList;
            this.cboTransType.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboTransType.Location = new System.Drawing.Point(587, 53);
            this.cboTransType.Name = "cboTransType";
            this.cboTransType.Size = new System.Drawing.Size(192, 23);
            this.cboTransType.TabIndex = 5;
            this.cboTransType.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.cboTransType.ValueChanged += new System.EventHandler(this.cboTransType_ValueChanged);
            this.cboTransType.Validating += new System.ComponentModel.CancelEventHandler(this.cboTransType_Validating);
            // 
            // ultraLabel1
            // 
            appearance40.ForeColor = System.Drawing.Color.Black;
            appearance40.TextHAlignAsString = "Center";
            appearance40.TextVAlignAsString = "Middle";
            this.ultraLabel1.Appearance = appearance40;
            this.ultraLabel1.AutoSize = true;
            this.ultraLabel1.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultraLabel1.Location = new System.Drawing.Point(90, 53);
            this.ultraLabel1.Name = "ultraLabel1";
            this.ultraLabel1.Size = new System.Drawing.Size(11, 15);
            this.ultraLabel1.TabIndex = 38;
            this.ultraLabel1.Text = "*";
            this.ultraLabel1.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // ultraLabel7
            // 
            appearance41.ForeColor = System.Drawing.Color.Black;
            appearance41.TextHAlignAsString = "Center";
            appearance41.TextVAlignAsString = "Middle";
            this.ultraLabel7.Appearance = appearance41;
            this.ultraLabel7.AutoSize = true;
            this.ultraLabel7.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultraLabel7.Location = new System.Drawing.Point(90, 23);
            this.ultraLabel7.Name = "ultraLabel7";
            this.ultraLabel7.Size = new System.Drawing.Size(11, 15);
            this.ultraLabel7.TabIndex = 37;
            this.ultraLabel7.Text = "*";
            this.ultraLabel7.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.pnlDeleteItem);
            this.groupBox2.Controls.Add(this.pnlFillFromPO);
            this.groupBox2.Controls.Add(this.pnlAddItem);
            this.groupBox2.Controls.Add(this.txtGrandTotalCost);
            this.groupBox2.Controls.Add(this.ultraLabel3);
            this.groupBox2.Controls.Add(this.txtEditorNoOfItems);
            this.groupBox2.Controls.Add(this.ultraLblNoOfItems);
            this.groupBox2.Controls.Add(this.grdDetail);
            this.groupBox2.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.groupBox2.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.ForeColor = System.Drawing.Color.White;
            this.groupBox2.Location = new System.Drawing.Point(9, 130);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(1283, 337);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Detail";
            // 
            // pnlDeleteItem
            // 
            // 
            // pnlDeleteItem.ClientArea
            // 
            this.pnlDeleteItem.ClientArea.Controls.Add(this.btnDeleteItem);
            this.pnlDeleteItem.ClientArea.Controls.Add(this.lblDeleteItem);
            this.pnlDeleteItem.Location = new System.Drawing.Point(587, 295);
            this.pnlDeleteItem.Name = "pnlDeleteItem";
            this.pnlDeleteItem.Size = new System.Drawing.Size(175, 30);
            this.pnlDeleteItem.TabIndex = 90;
            // 
            // btnDeleteItem
            // 
            appearance42.BackColor = System.Drawing.Color.White;
            appearance42.ForeColor = System.Drawing.Color.Black;
            this.btnDeleteItem.Appearance = appearance42;
            this.btnDeleteItem.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Flat;
            this.btnDeleteItem.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnDeleteItem.Location = new System.Drawing.Point(65, 0);
            this.btnDeleteItem.Name = "btnDeleteItem";
            this.btnDeleteItem.Size = new System.Drawing.Size(110, 30);
            this.btnDeleteItem.TabIndex = 15;
            this.btnDeleteItem.TabStop = false;
            this.btnDeleteItem.Text = "Delete Item ";
            this.btnDeleteItem.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnDeleteItem.Click += new System.EventHandler(this.btnDeleteItem_Click);
            this.btnDeleteItem.Enter += new System.EventHandler(this.Button_Enter);
            this.btnDeleteItem.Leave += new System.EventHandler(this.Button_Leave);
            // 
            // lblDeleteItem
            // 
            appearance43.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(138)))), ((int)(((byte)(31)))));
            appearance43.FontData.BoldAsString = "True";
            appearance43.ForeColor = System.Drawing.Color.White;
            appearance43.TextHAlignAsString = "Center";
            appearance43.TextVAlignAsString = "Middle";
            this.lblDeleteItem.Appearance = appearance43;
            this.lblDeleteItem.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblDeleteItem.Location = new System.Drawing.Point(0, 0);
            this.lblDeleteItem.Margin = new System.Windows.Forms.Padding(3, 3, 0, 3);
            this.lblDeleteItem.Name = "lblDeleteItem";
            this.lblDeleteItem.Size = new System.Drawing.Size(65, 30);
            this.lblDeleteItem.TabIndex = 0;
            this.lblDeleteItem.Tag = "NOCOLOR";
            this.lblDeleteItem.Text = "Alt + D";
            this.lblDeleteItem.Click += new System.EventHandler(this.btnDeleteItem_Click);
            // 
            // pnlFillFromPO
            // 
            // 
            // pnlFillFromPO.ClientArea
            // 
            this.pnlFillFromPO.ClientArea.Controls.Add(this.btnFillFromPO);
            this.pnlFillFromPO.ClientArea.Controls.Add(this.lblFillFromPO);
            this.pnlFillFromPO.Location = new System.Drawing.Point(777, 295);
            this.pnlFillFromPO.Name = "pnlFillFromPO";
            this.pnlFillFromPO.Size = new System.Drawing.Size(140, 30);
            this.pnlFillFromPO.TabIndex = 89;
            // 
            // btnFillFromPO
            // 
            appearance44.BackColor = System.Drawing.Color.White;
            appearance44.ForeColor = System.Drawing.Color.Black;
            this.btnFillFromPO.Appearance = appearance44;
            this.btnFillFromPO.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Flat;
            this.btnFillFromPO.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnFillFromPO.Location = new System.Drawing.Point(30, 0);
            this.btnFillFromPO.Name = "btnFillFromPO";
            this.btnFillFromPO.Size = new System.Drawing.Size(110, 30);
            this.btnFillFromPO.TabIndex = 16;
            this.btnFillFromPO.TabStop = false;
            this.btnFillFromPO.Text = "Fill from PO";
            this.btnFillFromPO.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnFillFromPO.Click += new System.EventHandler(this.btnFillFromPO_Click);
            this.btnFillFromPO.Enter += new System.EventHandler(this.Button_Enter);
            this.btnFillFromPO.Leave += new System.EventHandler(this.Button_Leave);
            // 
            // lblFillFromPO
            // 
            appearance45.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(138)))), ((int)(((byte)(31)))));
            appearance45.ForeColor = System.Drawing.Color.White;
            appearance45.TextHAlignAsString = "Center";
            appearance45.TextVAlignAsString = "Middle";
            this.lblFillFromPO.Appearance = appearance45;
            this.lblFillFromPO.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblFillFromPO.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Bold);
            this.lblFillFromPO.Location = new System.Drawing.Point(0, 0);
            this.lblFillFromPO.Margin = new System.Windows.Forms.Padding(3, 3, 0, 3);
            this.lblFillFromPO.Name = "lblFillFromPO";
            this.lblFillFromPO.Size = new System.Drawing.Size(30, 30);
            this.lblFillFromPO.TabIndex = 0;
            this.lblFillFromPO.Tag = "NOCOLOR";
            this.lblFillFromPO.Text = "F6";
            this.lblFillFromPO.Click += new System.EventHandler(this.btnFillFromPO_Click);
            // 
            // pnlAddItem
            // 
            // 
            // pnlAddItem.ClientArea
            // 
            this.pnlAddItem.ClientArea.Controls.Add(this.btnAddItem);
            this.pnlAddItem.ClientArea.Controls.Add(this.lblAddItem);
            this.pnlAddItem.Location = new System.Drawing.Point(932, 296);
            this.pnlAddItem.Name = "pnlAddItem";
            this.pnlAddItem.Size = new System.Drawing.Size(140, 30);
            this.pnlAddItem.TabIndex = 88;
            // 
            // btnAddItem
            // 
            appearance46.BackColor = System.Drawing.Color.White;
            appearance46.ForeColor = System.Drawing.Color.Black;
            this.btnAddItem.Appearance = appearance46;
            this.btnAddItem.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Flat;
            this.btnAddItem.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnAddItem.Location = new System.Drawing.Point(30, 0);
            this.btnAddItem.Name = "btnAddItem";
            this.btnAddItem.Size = new System.Drawing.Size(110, 30);
            this.btnAddItem.TabIndex = 17;
            this.btnAddItem.TabStop = false;
            this.btnAddItem.Text = "Add Items";
            this.btnAddItem.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnAddItem.Click += new System.EventHandler(this.btnAddItem_Click);
            this.btnAddItem.Enter += new System.EventHandler(this.Button_Enter);
            this.btnAddItem.Leave += new System.EventHandler(this.Button_Leave);
            // 
            // lblAddItem
            // 
            appearance47.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(138)))), ((int)(((byte)(31)))));
            appearance47.ForeColor = System.Drawing.Color.White;
            appearance47.TextHAlignAsString = "Center";
            appearance47.TextVAlignAsString = "Middle";
            this.lblAddItem.Appearance = appearance47;
            this.lblAddItem.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblAddItem.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Bold);
            this.lblAddItem.Location = new System.Drawing.Point(0, 0);
            this.lblAddItem.Margin = new System.Windows.Forms.Padding(3, 3, 0, 3);
            this.lblAddItem.Name = "lblAddItem";
            this.lblAddItem.Size = new System.Drawing.Size(30, 30);
            this.lblAddItem.TabIndex = 1;
            this.lblAddItem.Tag = "NOCOLOR";
            this.lblAddItem.Text = "F2";
            this.lblAddItem.Click += new System.EventHandler(this.btnAddItem_Click);
            // 
            // txtGrandTotalCost
            // 
            appearance48.ForeColor = System.Drawing.Color.Black;
            this.txtGrandTotalCost.Appearance = appearance48;
            this.txtGrandTotalCost.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtGrandTotalCost.Location = new System.Drawing.Point(303, 295);
            this.txtGrandTotalCost.Name = "txtGrandTotalCost";
            this.txtGrandTotalCost.ReadOnly = true;
            this.txtGrandTotalCost.Size = new System.Drawing.Size(120, 24);
            this.txtGrandTotalCost.TabIndex = 87;
            this.txtGrandTotalCost.TabStop = false;
            // 
            // ultraLabel3
            // 
            appearance49.ForeColor = System.Drawing.Color.Black;
            this.ultraLabel3.Appearance = appearance49;
            this.ultraLabel3.AutoSize = true;
            this.ultraLabel3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultraLabel3.Location = new System.Drawing.Point(188, 299);
            this.ultraLabel3.Name = "ultraLabel3";
            this.ultraLabel3.Size = new System.Drawing.Size(109, 17);
            this.ultraLabel3.TabIndex = 86;
            this.ultraLabel3.Text = "Grand Total Cost";
            // 
            // txtEditorNoOfItems
            // 
            appearance50.ForeColor = System.Drawing.Color.Black;
            this.txtEditorNoOfItems.Appearance = appearance50;
            this.txtEditorNoOfItems.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtEditorNoOfItems.Location = new System.Drawing.Point(91, 295);
            this.txtEditorNoOfItems.Name = "txtEditorNoOfItems";
            this.txtEditorNoOfItems.ReadOnly = true;
            this.txtEditorNoOfItems.Size = new System.Drawing.Size(91, 24);
            this.txtEditorNoOfItems.TabIndex = 85;
            this.txtEditorNoOfItems.TabStop = false;
            // 
            // ultraLblNoOfItems
            // 
            appearance51.ForeColor = System.Drawing.Color.Black;
            this.ultraLblNoOfItems.Appearance = appearance51;
            this.ultraLblNoOfItems.AutoSize = true;
            this.ultraLblNoOfItems.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultraLblNoOfItems.Location = new System.Drawing.Point(8, 299);
            this.ultraLblNoOfItems.Name = "ultraLblNoOfItems";
            this.ultraLblNoOfItems.Size = new System.Drawing.Size(82, 17);
            this.ultraLblNoOfItems.TabIndex = 84;
            this.ultraLblNoOfItems.Text = "No. Of Items";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.pnlPrintShelfStickers);
            this.groupBox3.Controls.Add(this.pnlReport);
            this.groupBox3.Controls.Add(this.pnlClose);
            this.groupBox3.Controls.Add(this.pnlSave);
            this.groupBox3.Controls.Add(this.pnlNew);
            this.groupBox3.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.groupBox3.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox3.Location = new System.Drawing.Point(9, 470);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(1283, 56);
            this.groupBox3.TabIndex = 2;
            this.groupBox3.TabStop = false;
            // 
            // pnlReport
            // 
            // 
            // pnlReport.ClientArea
            // 
            this.pnlReport.ClientArea.Controls.Add(this.btnReport);
            this.pnlReport.ClientArea.Controls.Add(this.lblReport);
            this.pnlReport.Location = new System.Drawing.Point(302, 15);
            this.pnlReport.Name = "pnlReport";
            this.pnlReport.Size = new System.Drawing.Size(270, 30);
            this.pnlReport.TabIndex = 1;
            // 
            // btnReport
            // 
            appearance54.BackColor = System.Drawing.Color.White;
            appearance54.ForeColor = System.Drawing.Color.Black;
            this.btnReport.Appearance = appearance54;
            this.btnReport.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Flat;
            this.btnReport.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnReport.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnReport.Location = new System.Drawing.Point(60, 0);
            this.btnReport.Name = "btnReport";
            this.btnReport.Size = new System.Drawing.Size(210, 30);
            this.btnReport.TabIndex = 10;
            this.btnReport.Text = "Print Inventory Received";
            this.btnReport.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnReport.Click += new System.EventHandler(this.btnReport_Click);
            this.btnReport.Enter += new System.EventHandler(this.Button_Enter);
            this.btnReport.Leave += new System.EventHandler(this.Button_Leave);
            // 
            // lblReport
            // 
            appearance55.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(138)))), ((int)(((byte)(31)))));
            appearance55.ForeColor = System.Drawing.Color.White;
            appearance55.TextHAlignAsString = "Center";
            appearance55.TextVAlignAsString = "Middle";
            this.lblReport.Appearance = appearance55;
            this.lblReport.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblReport.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Bold);
            this.lblReport.Location = new System.Drawing.Point(0, 0);
            this.lblReport.Margin = new System.Windows.Forms.Padding(3, 3, 0, 3);
            this.lblReport.Name = "lblReport";
            this.lblReport.Size = new System.Drawing.Size(60, 30);
            this.lblReport.TabIndex = 0;
            this.lblReport.Tag = "NOCOLOR";
            this.lblReport.Text = "Alt + P";
            this.lblReport.Click += new System.EventHandler(this.btnReport_Click);
            // 
            // pnlClose
            // 
            // 
            // pnlClose.ClientArea
            // 
            this.pnlClose.ClientArea.Controls.Add(this.btnClose);
            this.pnlClose.ClientArea.Controls.Add(this.lblClose);
            this.pnlClose.Location = new System.Drawing.Point(777, 15);
            this.pnlClose.Name = "pnlClose";
            this.pnlClose.Size = new System.Drawing.Size(140, 30);
            this.pnlClose.TabIndex = 3;
            // 
            // lblClose
            // 
            appearance56.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(138)))), ((int)(((byte)(31)))));
            appearance56.ForeColor = System.Drawing.Color.White;
            appearance56.TextHAlignAsString = "Center";
            appearance56.TextVAlignAsString = "Middle";
            this.lblClose.Appearance = appearance56;
            this.lblClose.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblClose.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Bold);
            this.lblClose.Location = new System.Drawing.Point(0, 0);
            this.lblClose.Margin = new System.Windows.Forms.Padding(3, 3, 0, 3);
            this.lblClose.Name = "lblClose";
            this.lblClose.Size = new System.Drawing.Size(60, 30);
            this.lblClose.TabIndex = 0;
            this.lblClose.Tag = "NOCOLOR";
            this.lblClose.Text = "Alt + C";
            this.lblClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // pnlSave
            // 
            // 
            // pnlSave.ClientArea
            // 
            this.pnlSave.ClientArea.Controls.Add(this.btnSave);
            this.pnlSave.ClientArea.Controls.Add(this.lblSave);
            this.pnlSave.Location = new System.Drawing.Point(932, 15);
            this.pnlSave.Name = "pnlSave";
            this.pnlSave.Size = new System.Drawing.Size(140, 30);
            this.pnlSave.TabIndex = 0;
            // 
            // lblSave
            // 
            appearance57.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(138)))), ((int)(((byte)(31)))));
            appearance57.ForeColor = System.Drawing.Color.White;
            appearance57.TextHAlignAsString = "Center";
            appearance57.TextVAlignAsString = "Middle";
            this.lblSave.Appearance = appearance57;
            this.lblSave.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblSave.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Bold);
            this.lblSave.Location = new System.Drawing.Point(0, 0);
            this.lblSave.Margin = new System.Windows.Forms.Padding(3, 3, 0, 3);
            this.lblSave.Name = "lblSave";
            this.lblSave.Size = new System.Drawing.Size(60, 30);
            this.lblSave.TabIndex = 0;
            this.lblSave.Tag = "NOCOLOR";
            this.lblSave.Text = "Alt + S";
            this.lblSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // pnlNew
            // 
            // 
            // pnlNew.ClientArea
            // 
            this.pnlNew.ClientArea.Controls.Add(this.btnNew);
            this.pnlNew.ClientArea.Controls.Add(this.lblNew);
            this.pnlNew.Location = new System.Drawing.Point(587, 15);
            this.pnlNew.Name = "pnlNew";
            this.pnlNew.Size = new System.Drawing.Size(175, 30);
            this.pnlNew.TabIndex = 2;
            // 
            // lblNew
            // 
            appearance58.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(138)))), ((int)(((byte)(31)))));
            appearance58.ForeColor = System.Drawing.Color.White;
            appearance58.TextHAlignAsString = "Center";
            appearance58.TextVAlignAsString = "Middle";
            this.lblNew.Appearance = appearance58;
            this.lblNew.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblNew.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Bold);
            this.lblNew.Location = new System.Drawing.Point(0, 0);
            this.lblNew.Margin = new System.Windows.Forms.Padding(3, 3, 0, 3);
            this.lblNew.Name = "lblNew";
            this.lblNew.Size = new System.Drawing.Size(60, 30);
            this.lblNew.TabIndex = 0;
            this.lblNew.Tag = "NOCOLOR";
            this.lblNew.Text = "Alt + L";
            this.lblNew.Click += new System.EventHandler(this.btnNew_Click);
            // 
            // lblMessage
            // 
            appearance59.ForeColor = System.Drawing.Color.Red;
            appearance59.TextHAlignAsString = "Center";
            this.lblMessage.Appearance = appearance59;
            this.lblMessage.BackColorInternal = System.Drawing.Color.Transparent;
            this.lblMessage.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblMessage.Font = new System.Drawing.Font("Times New Roman", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMessage.Location = new System.Drawing.Point(0, 531);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.Size = new System.Drawing.Size(1284, 20);
            this.lblMessage.TabIndex = 33;
            this.lblMessage.Tag = "NOCOLOR";
            this.lblMessage.Text = "We couldn\'t use this feature as \"Hide Item Cost\" user-level setting is turned off" +
    "";
            this.lblMessage.Visible = false;
            // 
            // pnlPrintShelfStickers
            // 
            // 
            // pnlPrintShelfStickers.ClientArea
            // 
            this.pnlPrintShelfStickers.ClientArea.Controls.Add(this.btnPrintShelfStickers);
            this.pnlPrintShelfStickers.ClientArea.Controls.Add(this.lblPrintShelfStickers);
            this.pnlPrintShelfStickers.Location = new System.Drawing.Point(58, 15);
            this.pnlPrintShelfStickers.Name = "pnlPrintShelfStickers";
            this.pnlPrintShelfStickers.Size = new System.Drawing.Size(230, 30);
            this.pnlPrintShelfStickers.TabIndex = 4;
            // 
            // btnPrintShelfStickers
            // 
            appearance52.BackColor = System.Drawing.Color.White;
            appearance52.ForeColor = System.Drawing.Color.Black;
            this.btnPrintShelfStickers.Appearance = appearance52;
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
            appearance53.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(138)))), ((int)(((byte)(31)))));
            appearance53.ForeColor = System.Drawing.Color.White;
            appearance53.TextHAlignAsString = "Center";
            appearance53.TextVAlignAsString = "Middle";
            this.lblPrintShelfStickers.Appearance = appearance53;
            this.lblPrintShelfStickers.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblPrintShelfStickers.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Bold);
            this.lblPrintShelfStickers.Location = new System.Drawing.Point(0, 0);
            this.lblPrintShelfStickers.Margin = new System.Windows.Forms.Padding(3, 3, 0, 3);
            this.lblPrintShelfStickers.Name = "lblPrintShelfStickers";
            this.lblPrintShelfStickers.Size = new System.Drawing.Size(60, 30);
            this.lblPrintShelfStickers.TabIndex = 0;
            this.lblPrintShelfStickers.Tag = "NOCOLOR";
            this.lblPrintShelfStickers.Text = "Alt + R";
            this.lblPrintShelfStickers.Click += new System.EventHandler(this.btnPrintShelfStickers_Click);
            // 
            // frmInventoryRecieved
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(1284, 551);
            this.Controls.Add(this.lblMessage);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.lblTransactionType);
            this.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.Name = "frmInventoryRecieved";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Tag = "";
            this.Text = "Inventory Received";
            this.Activated += new System.EventHandler(this.frmInventoryRecieved_Activated);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmInventoryRecieved_FormClosed);
            this.Load += new System.EventHandler(this.frmInventoryRecieved_Load);
            this.Shown += new System.EventHandler(this.frmInventoryRecieved_Shown);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmInventoryRecieved_KeyDown);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.frmInventoryRecieved_KeyUp);
            this.Resize += new System.EventHandler(this.frmInventoryRecieved_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.ultraDateTimeEditor1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtReference)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtVendor)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtpRecvDate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdDetail)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cboTransType)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.pnlDeleteItem.ClientArea.ResumeLayout(false);
            this.pnlDeleteItem.ResumeLayout(false);
            this.pnlFillFromPO.ClientArea.ResumeLayout(false);
            this.pnlFillFromPO.ResumeLayout(false);
            this.pnlAddItem.ClientArea.ResumeLayout(false);
            this.pnlAddItem.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.txtGrandTotalCost)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtEditorNoOfItems)).EndInit();
            this.groupBox3.ResumeLayout(false);
            this.pnlReport.ClientArea.ResumeLayout(false);
            this.pnlReport.ResumeLayout(false);
            this.pnlClose.ClientArea.ResumeLayout(false);
            this.pnlClose.ResumeLayout(false);
            this.pnlSave.ClientArea.ResumeLayout(false);
            this.pnlSave.ResumeLayout(false);
            this.pnlNew.ClientArea.ResumeLayout(false);
            this.pnlNew.ResumeLayout(false);
            this.pnlPrintShelfStickers.ClientArea.ResumeLayout(false);
            this.pnlPrintShelfStickers.ResumeLayout(false);
            this.ResumeLayout(false);

        }
        #endregion


        private void ApplyGrigFormat()
        {
            clsUIHelper.SetAppearance(this.grdDetail);

            this.grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.InvRecvDetail_Fld_InvRecievedID].Hidden = true;
            this.grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.InvRecvDetail_Fld_InvRecvDetailID].Hidden = true;
            this.grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.Vendor_Fld_VendorCode].Hidden = true;

            this.grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.InvRecvDetail_Fld_ItemID].MaxLength = 20;
            this.grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.InvRecvDetail_Fld_ItemID].Header.Caption = "Item#";

            this.grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.InvRecvDetail_Fld_Comments].MaxLength = 50;

            #region Sprint-21 - 2207 03-Aug-2015 JY Added
            this.grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.ItemVendor_Fld_VendorItemID].MaxLength = 100;
            this.grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.ItemVendor_Fld_VendorItemID].Header.Caption = "VendorItemID";
            this.grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.ItemVendor_Fld_VendorItemID].Width = 120;
            #endregion

            this.grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.Item_Fld_Description].MaxLength = 50;
            this.grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.Item_Fld_Description].Header.Caption = "Item Description";
            this.grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.Item_Fld_Description].CellActivation = Activation.Disabled;
            this.grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.Item_Fld_Description].Width = 200;

            this.grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.Department_Fld_DeptName].CellActivation = Activation.Disabled;

            this.grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.InvRecvDetail_Fld_QtyOrdered].CellActivation = Activation.Disabled;
            grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.InvRecvDetail_Fld_QtyOrdered].CellAppearance.TextHAlign = HAlign.Right;

            grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.InvRecvDetail_Fld_QTY].CellAppearance.TextHAlign = HAlign.Right;
            grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.InvRecvDetail_Fld_QTY].MaxValue = 999999;
            grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.InvRecvDetail_Fld_QTY].MinValue = 0;//Sprint-27 - PRIMEPOS-2474 10-Jan-2018 JY changed value from 1 to 0
            grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.InvRecvDetail_Fld_QTY].MaskInput = "999999";
            grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.InvRecvDetail_Fld_QTY].Format = "######";

            #region PRIMEPOS-2396 12-Jun-2018 JY Added
            grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.Item_Fld_QtyInStock].Header.Caption = "Stock";
            grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.Item_Fld_QtyInStock].CellAppearance.TextHAlign = HAlign.Right;
            this.grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.Item_Fld_QtyInStock].CellActivation = Activation.Disabled;

            grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.InvRecvDetail_Fld_LastInvUpdatedQty].CellAppearance.TextHAlign = HAlign.Right;
            this.grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.InvRecvDetail_Fld_LastInvUpdatedQty].CellActivation = Activation.Disabled;
            #endregion

            this.btnFillFromPO.Enabled = true;

            /*if (this.cboTransType.Value!=null)
			{
				InvTransType oType=new InvTransType();
				InvTransTypeData oTData= oType.PopulateList(" where id=" + cboTransType.Value.ToString());
				if (oTData.InvTransType.Rows.Count>0)
				{
					if (Resources.Configuration.convertNullToInt(oTData.InvTransType.Rows[0][clsPOSDBConstants.InvTransType_Fld_TransType].ToString())==1)
					{
						grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.InvRecvDetail_Fld_QTY].MaxValue = -1;
						grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.InvRecvDetail_Fld_QTY].MinValue = -999999;
						grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.InvRecvDetail_Fld_QTY].MaskInput="-nnnnnn";
						grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.InvRecvDetail_Fld_QTY].Format="######";
						this.btnFillFromPO.Enabled=false;
					}
				}
			}*/

            grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.InvRecvDetail_Fld_Cost].CellAppearance.TextHAlign = HAlign.Right;
            grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.InvRecvDetail_Fld_Cost].MaxValue = 9999.99;
            grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.InvRecvDetail_Fld_Cost].MaskInput = "9999.99";
            grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.InvRecvDetail_Fld_Cost].Format = "####.00";

            #region Sprint-21 - 2002 21-Jul-2015 JY Added
            grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.InvRecvDetail_Fld_TotalCost].CellAppearance.TextHAlign = HAlign.Right;
            grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.InvRecvDetail_Fld_TotalCost].MaxValue = 9999999999.99;
            grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.InvRecvDetail_Fld_TotalCost].MaskInput = "9999999999.99";
            grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.InvRecvDetail_Fld_TotalCost].Format = "##########.00";
            this.grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.InvRecvDetail_Fld_TotalCost].CellActivation = Activation.Disabled;
            #endregion

            grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.InvRecvDetail_Fld_SalePrice].CellAppearance.TextHAlign = HAlign.Right;
            grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.InvRecvDetail_Fld_SalePrice].MaxValue = 9999.99;
            grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.InvRecvDetail_Fld_SalePrice].Header.Caption = "Sale Price";
            grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.InvRecvDetail_Fld_SalePrice].MaskInput = "9999.99";
            grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.InvRecvDetail_Fld_SalePrice].Format = "####.00";

            grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.InvRecvDetail_Fld_ExpDate].Header.Caption = "Exp. Date";
            this.grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.InvRecvDetail_Fld_ExpDate].Width = 120;

            this.txtVendor.MaxLength = 20;
            this.txtReference.MaxLength = 20;

            #region PRIMEPOS-2822 17-Mar-2020 JY Added
            this.grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.InvRecvDetail_Fld_ItemID].TabIndex = 0;
            this.grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.ItemVendor_Fld_VendorItemID].TabIndex = 1;
            this.grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.Department_Fld_DeptCode].TabIndex = 2;
            this.grdDetail.DisplayLayout.Bands[0].Columns[strSubDept].TabIndex = 3;
            this.grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.InvRecvDetail_Fld_QTY].TabIndex = 4;
            this.grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.InvRecvDetail_Fld_Cost].TabIndex = 5;
            this.grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.InvRecvDetail_Fld_SalePrice].TabIndex = 6;
            this.grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.InvRecvDetail_Fld_ExpDate].TabIndex = 7;
            this.grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.InvRecvDetail_Fld_IsEBTItem].TabIndex = 8;
            this.grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.InvRecvDetail_Fld_Comments].TabIndex = 9;
            #endregion
        }

        private void populateTransType()
        {
            try
            {
                InvTransTypeData oInvTransTypeData = new InvTransTypeData();
                InvTransType oInvTransType = new InvTransType();
                oInvTransTypeData = oInvTransType.PopulateList("");

                if (oInvTransTypeData.InvTransType.Rows.Count == 0)
                {
                    InvTransTypeRow oNewRow = oInvTransTypeData.InvTransType.AddRow(0, "", 0, "");
                    oNewRow.TypeName = "Inventory Received";
                    oNewRow.TransType = 0;
                    oNewRow.UserID = POS_Core.Resources.Configuration.UserName;
                    oInvTransType.Persist(oInvTransTypeData);
                    oInvTransTypeData = oInvTransType.PopulateList("");
                }

                Infragistics.Win.ValueListItem oVl = new Infragistics.Win.ValueListItem();
                this.cboTransType.Items.Clear();
                dctInvTransType.Clear();    //Sprint-22 - PRIMEPOS-2251 02-Dec-2015 JY Added
                foreach (InvTransTypeRow oDRow in oInvTransTypeData.InvTransType.Rows)
                {
                    this.cboTransType.Items.Add(oDRow.ID.ToString(), oDRow.TypeName.ToString());
                    #region Sprint-22 - PRIMEPOS-2251 02-Dec-2015 JY Added
                    try
                    {
                        dctInvTransType.Add(oDRow.ID, oDRow.TransType);
                    }
                    catch { }
                    #endregion
                }

                //this.cboTransType.SelectedIndex = 0;  //PRIMEPOS-2901 10-Nov-2020 JY Commented

                #region PRIMEPOS-2901 10-Nov-2020 JY Added
                if (frmInventoryRecieved.nSetTransTypeId == Configuration.CInfo.DefaultInvReturnID)
                {
                    int Selected = 0;
                    for (int i = 0; i < this.cboTransType.Items.Count; i++)
                    {
                        if (this.cboTransType.Items[i].DataValue.ToString() == frmInventoryRecieved.nSetTransTypeId.ToString())
                        {
                            Selected = i;
                            break;
                        }
                    }
                    frmInventoryRecieved.nSetTransTypeId = 0;
                    this.cboTransType.SelectedIndex = Selected;
                }
                else
                {
                    this.cboTransType.SelectedIndex = 0;
                }
                #endregion
            }
            catch (Exception exp) { clsUIHelper.ShowErrorMsg(exp.Message); }
        }

        private void frmInventoryRecieved_Load(object sender, System.EventArgs e)
        {
            try
            {
                clsUIHelper.SetKeyActionMappings(this.grdDetail);
                this.grdDetail.KeyActionMappings.Add(new GridKeyActionMapping(Keys.Tab, UltraGridAction.NextCellByTab, 0, UltraGridState.InEdit, 0, 0));    //PRIMEPOS-2822 17-Mar-2020 JY Added
                this.grdDetail.DisplayLayout.TabNavigation = TabNavigation.NextCell;    //PRIMEPOS-2822 17-Mar-2020 JY Added
                this.ApplyGrigFormat();
                SetNew();

                this.txtVendor.AfterEnterEditMode += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
                this.txtVendor.AfterExitEditMode += new System.EventHandler(clsUIHelper.AfterExitEditMode);

                this.txtReference.AfterEnterEditMode += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
                this.txtReference.AfterExitEditMode += new System.EventHandler(clsUIHelper.AfterExitEditMode);

                this.grdDetail.AfterEnterEditMode += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
                this.grdDetail.AfterExitEditMode += new System.EventHandler(clsUIHelper.AfterExitEditMode);

                this.dtpRecvDate.Enter += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
                this.dtpRecvDate.Leave += new System.EventHandler(clsUIHelper.AfterExitEditMode);

                populateTransType();
                clsUIHelper.setColorSchecme(this);
                btnClose.Appearance.BackColor = Color.FromArgb(255, System.Convert.ToByte(POS_Core.Resources.Configuration.arrCloseBk[0].ToString()), System.Convert.ToByte(POS_Core.Resources.Configuration.arrCloseBk[1]), System.Convert.ToByte(POS_Core.Resources.Configuration.arrCloseBk[2]));
                btnClose.Appearance.BackColor2 = Color.FromArgb(255, System.Convert.ToByte(POS_Core.Resources.Configuration.arrCloseBk[0].ToString()), System.Convert.ToByte(POS_Core.Resources.Configuration.arrCloseBk[1]), System.Convert.ToByte(POS_Core.Resources.Configuration.arrCloseBk[2]));
                //Following added by krishna on 12 April2011
                txtVendor.Text = VendorName;
                //Till here Added by krishna;
                isInventoryRecieved = true; //Added by Ravindra

                #region PRIMEPOS-2464 10-Mar-2020 JY Added
                nDisplayItemCost = Configuration.convertBoolToInt(UserPriviliges.IsUserHasPriviliges(UserPriviliges.Modules.InventoryMgmt.ID, UserPriviliges.Screens.ItemFile.ID, UserPriviliges.Permissions.DisplayItemCost.ID));   //PRIMEPOS-2464 09-Mar-2020 JY Added
                if (nDisplayItemCost == 0)
                {
                    lblMessage.Visible = true;
                    tmrBlinking = new System.Timers.Timer();
                    tmrBlinking.Interval = 1000;//1 seconds
                    tmrBlinking.Elapsed += new ElapsedEventHandler(tmrBlinkingTimedEvent);
                    tmrBlinking.Enabled = true;
                    grdDetail.Enabled = pnlAddItem.Enabled = pnlFillFromPO.Enabled = false;
                }
                else
                {
                    this.Height -= 15;
                    grdDetail.Enabled = pnlAddItem.Enabled = pnlFillFromPO.Enabled = true;
                }
                #endregion
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
                if (!isFromPurchaseOrder)   //Sprint-27 - PRIMEPOS-2474 10-Jan-2018 JY Added condition to allow 0 ack qty in case of process acknowledgement
                {
                    #region Sprint-26 - PRIMEPOS-665 30-Jun-2017 JY Added
                    for (int i = 0; i < grdDetail.DisplayLayout.Rows.Count; i++)
                    {
                        if (Configuration.convertNullToInt(grdDetail.DisplayLayout.Rows[i].Cells[clsPOSDBConstants.InvRecvDetail_Fld_QTY].Value) == 0)
                        {
                            this.grdDetail.ActiveCell = grdDetail.DisplayLayout.Rows[i].Cells[clsPOSDBConstants.InvRecvDetail_Fld_QTY];
                            this.grdDetail.PerformAction(UltraGridAction.ActivateCell);
                            this.grdDetail.PerformAction(UltraGridAction.EnterEditMode);
                            return;
                        }
                    }
                    #endregion
                }
                //Modified By SRT(Ritesh Parekh) Date : 19-Aug-2009
                //Condition moved closer to persist method.
                PurchaseOrder oPO = new PurchaseOrder();
                oInvRecvHRow.RefNo = this.txtReference.Text;
                oInvRecvHRow.VendorID = Convert.ToInt32(this.txtVendor.Tag);
                //Added by Amit Date 18 Aug 2011
                oInvRecvHRow.VendorCode = this.txtVendor.Text;
                //End
                POS_Core.ErrorLogging.Logs.Logger("**Start Add Inventory");
                oInvRecvHRow.RecvDate = (System.DateTime)this.dtpRecvDate.Value;
                oInvRecvHRow.InvTransTypeID = Convert.ToInt32(cboTransType.Value.ToString());//Temporary Krishna=uncomment afterwards
                if (inventoryWay == clsPOSDBConstants.PurchaseOrder)
                {
                    oInvRecvHRow.POOrderNo = POS_Core.Resources.Configuration.convertNullToString(poHeadData.POHeader.Rows[0]["OrderNo"]);
                    POS_Core.ErrorLogging.Logs.Logger("**Inventory update from Purchase Order no:" + oInvRecvHRow.POOrderNo);
                }

                if (this.cboTransType.Value != null)
                {
                    InvTransType oType = new InvTransType();
                    InvTransTypeData oTData = oType.PopulateList(" where id=" + cboTransType.Value.ToString());
                    if (oTData.InvTransType.Rows.Count > 0)
                    {
                        if (POS_Core.Resources.Configuration.convertNullToInt(oTData.InvTransType.Rows[0][clsPOSDBConstants.InvTransType_Fld_TransType].ToString()) == 1)
                        {
                            foreach (InvRecvDetailRow oRow in oInvRecvDData.InvRecvDetail.Rows)
                            {
                                //oRow.QTY = oRow.QTY * -1;
                                if (oRow.QTY > 0) oRow.QTY = oRow.QTY * -1;   //PRIMEPOS-2620 17-Dec-2018 JY Added
                                oRow.ExpDate = null;    //Sprint-26 - PRIMEPOS-2387 14-Jul-2017 JY update ExpDate if TransType is "Add to Inventory" like "Inventory Received"
                            }
                        }
                    }
                }
                //if (inventoryWay == clsPOSDBConstants.PurchaseOrder)
                //{
                //    poHeadData.POHeader[0].Status = (int)PurchseOrdStatus.Processed;
                //    foreach (PODetailRow podRow in poDetailData.PODetail.Rows)
                //    {
                //        int processQty = 0;
                //        DataRow[] dr = (oInvRecvDData.InvRecvDetail.Select(@"" + clsPOSDBConstants.InvRecvDetail_Fld_ItemID + " = '" + podRow.ItemID.Trim() + "'"));//" ItemID = " + "'" + itemID + "'"
                //        if (int.TryParse(dr[0][clsPOSDBConstants.InvRecvDetail_Fld_QTY].ToString(), out processQty))
                //            podRow.ProcessedQTY = processQty;
                //    }
                //    oPO.Persist(poHeadData, poDetailData);
                //    isFromPurchaseOrder = true;//Added by Krishna on 30 December 2011 to set only cost price
                //}
                //Added By SRT(Ritesh Parekh) Date : 19-Aug-2009
                //Added to validate the data before moving towords the saving.
                oInvRecv.checkIsValidData(oInvRecvHData, oInvRecvDData);
                //End Of Added By SRT(Ritesh Parekh)tr
                try
                {
                    POS_Core.ErrorLogging.Logs.Logger("**Inventory Transaction Type:" + cboTransType.Text);
                }
                catch { }
                if (Resources.Message.Display("This will update your inventory for all the listed Items.\n Are you sure ?\n", "Inventory Received", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {


                    oInvRecv.Persist(oInvRecvHData, oInvRecvDData, this.btnFillFromPO.Tag.ToString(), POS_Core_UI.frmInventoryRecieved.isFromPurchaseOrder);
                    #region Sprint-21 - 2207 13-Aug-2015 JY Added to save ItemVendorId in ItemVendor Table
                    ItemVendor oItemVendor = new ItemVendor();
                    ItemVendorData oItemVendorData = new ItemVendorData();

                    for (int i = 0; i < grdDetail.Rows.Count; i++)
                    {
                        if ((grdDetail.Rows[i].Cells[clsPOSDBConstants.Vendor_Fld_VendorCode].Value.ToString().Trim() != "") && (grdDetail.Rows[i].Cells[clsPOSDBConstants.ItemVendor_Fld_VendorItemID].Value.ToString().Trim() != ""))
                        {
                            oItemVendorData = oItemVendor.PopulateList(" WHERE ItemVendor." + clsPOSDBConstants.ItemVendor_Fld_ItemID + " = '" + grdDetail.Rows[i].Cells[clsPOSDBConstants.ItemVendor_Fld_ItemID].Value.ToString().Trim() + "' AND Vendor." + clsPOSDBConstants.Vendor_Fld_VendorCode + " = '" + txtVendor.Text.Trim() + "'");

                            if (oItemVendorData.Tables.Count > 0 && oItemVendorData.Tables[0].Rows.Count > 0)
                            {
                                if (oItemVendorData.Tables[0].Rows[0][clsPOSDBConstants.ItemVendor_Fld_VendorItemID].ToString().Trim().ToUpper() != grdDetail.Rows[i].Cells[clsPOSDBConstants.ItemVendor_Fld_VendorItemID].Value.ToString().Trim().ToUpper())
                                    oItemVendor.UpdateItemVendor((Int32)oItemVendorData.Tables[0].Rows[0][clsPOSDBConstants.ItemVendor_Fld_ItemDetailID], grdDetail.Rows[i].Cells[clsPOSDBConstants.ItemVendor_Fld_VendorItemID].Value.ToString().Trim());
                            }
                            else
                            {
                                ItemVendorData oItemVendorData1 = new ItemVendorData();
                                ItemVendorRow itemVendRow = oItemVendorData1.ItemVendor.NewItemVendorRow();
                                itemVendRow.ItemDetailID = 0;
                                itemVendRow.ItemID = grdDetail.Rows[i].Cells[clsPOSDBConstants.InvRecvDetail_Fld_ItemID].Value.ToString().Trim();
                                itemVendRow.VendorItemID = grdDetail.Rows[i].Cells[clsPOSDBConstants.ItemVendor_Fld_VendorItemID].Value.ToString().Trim();
                                itemVendRow.VendorID = Convert.ToInt32(this.txtVendor.Tag);
                                itemVendRow.VenorCostPrice = Configuration.convertNullToDecimal(grdDetail.Rows[i].Cells[clsPOSDBConstants.InvRecvDetail_Fld_Cost].Value);
                                itemVendRow.VendorCode = grdDetail.Rows[i].Cells[clsPOSDBConstants.Vendor_Fld_VendorCode].Value.ToString().Trim();
                                //itemVendRow.VendorName
                                //itemVendRow.LastOrderDate
                                itemVendRow.AverageWholeSalePrice = 0M;
                                itemVendRow.CatalogPrice = 0M;
                                itemVendRow.ContractPrice = 0M;
                                itemVendRow.DealerAdjustedPrice = 0M;
                                itemVendRow.FedrelUpperLimitPrice = 0M;
                                itemVendRow.ManufacturerSuggPrice = 0M;
                                itemVendRow.NetItemPrice = 0M;
                                itemVendRow.ProducersPrice = 0M;
                                itemVendRow.RetailPrice = 0M;
                                itemVendRow.InVoiceBillingPrice = 0M;
                                itemVendRow.BaseCharge = 0M;
                                itemVendRow.UnitPriceBegQuantity = 0M;
                                itemVendRow.Resale = 0M;
                                //itemVendRow.UnitCostPrice
                                //itemVendRow.VendorSalePrice
                                //itemVendRow.HammacherDeptClass
                                //itemVendRow.SaleStartDate
                                //itemVendRow.SaleEndDate*/

                                oItemVendorData1.ItemVendor.AddRow(itemVendRow, false);
                                oItemVendorData1.Tables[0].Rows[0].AcceptChanges();
                                oItemVendorData1.Tables[0].Rows[0].SetAdded();
                                oItemVendor.Persist(oItemVendorData1);
                            }
                        }
                    }
                    #endregion

                    //From Here Added by Ravindra (QuicSolv) 5 April 2013 
                    if (inventoryWay == clsPOSDBConstants.PurchaseOrder)
                    {
                        poHeadData.POHeader[0].Status = (int)PurchseOrdStatus.Processed;
                        foreach (PODetailRow podRow in poDetailData.PODetail.Rows)
                        {
                            int processQty = 0;
                            DataRow[] dr = (oInvRecvDData.InvRecvDetail.Select(@"" + clsPOSDBConstants.InvRecvDetail_Fld_ItemID + " = '" + podRow.ItemID.Trim() + "'"));//" ItemID = " + "'" + itemID + "'"

                            #region 17-Jun-2015 JY Added  - If AckStatus = 'IS' (itemAcceptedSubstitutionMade) and ChangedProductId is not NULL then the application throws exception as it compares the changed itemid with original itemid. below code added to to handle it
                            if (dr.Length == 0 && podRow.ChangedProductID.Trim() != "")
                            {
                                dr = (oInvRecvDData.InvRecvDetail.Select(@"" + clsPOSDBConstants.InvRecvDetail_Fld_ItemID + " = '" + podRow.ChangedProductID.Trim() + "'"));
                            }
                            if (dr.Length == 0)
                            {
                                clsUIHelper.ShowErrorMsg(podRow.ItemID.Trim() + " : ItemId is invalid or Not found");
                                return;
                            }
                            #endregion

                            if (int.TryParse(dr[0][clsPOSDBConstants.InvRecvDetail_Fld_QTY].ToString(), out processQty))
                                podRow.ProcessedQTY = processQty;
                            try
                            {
                                POS_Core.ErrorLogging.Logs.Logger("**Inventory Update Item code:" + podRow.ItemID.Trim() + " ,Order Qty=" + podRow.QTY + " ,Process Qty=" + processQty);
                            }
                            catch { }
                        }
                        oPO.Persist(poHeadData, poDetailData);
                        isFromPurchaseOrder = true;
                        //Added by Krishna on 30 December 2011 to set only cost price
                    }
                    //till Here added by Ravindra

                    SetNew();
                    this.Close();
                    //frmInventoryRecieved oIFrm= (frmInventoryRecieved)frmMain.ShowForm(POSMenuItems.InventoryRecieved);
                    if (inventoryWay == clsPOSDBConstants.PurchaseOrder)
                    {
                        frmSearchPOAck poAck = (frmSearchPOAck)frmMain.ShowForm(POSMenuItems.POAcknowledgement);
                    }

                    isFromPurchaseOrder = false;//Added by Krishna on 30 December 2011 to set only cost price

                    DialogResult = DialogResult.OK;//Added By Shitaljit(QuicSolv) on 21 Sept 2011 
                }
                //end Of Updated By SRT(Ritesh Parekh)
            }
            catch (POSExceptions exp)
            {
                isFromPurchaseOrder = false;//Added by Krishna on 30 December 2011 to set only cost price
                clsUIHelper.ShowErrorMsg(exp.ErrMessage);
                switch (exp.ErrNumber)
                {
                    case (long)POSErrorENUM.InvRecvHeader_VendorIDNotNull:
                        this.txtVendor.Focus();
                        break;
                    case (long)POSErrorENUM.InvRecvHeader_RefNoNotNull:
                        this.txtReference.Focus();
                        break;
                    case (long)POSErrorENUM.InvRecvHeader_RecvDateNotNull:
                        this.dtpRecvDate.Focus();
                        break;
                    case (long)POSErrorENUM.InvRecvHeader_RecvDetailMissing:
                        this.grdDetail.Focus();
                        break;
                }
            }

            catch (Exception exp)
            {
                isFromPurchaseOrder = false;//Added by Krishna on 30 December 2011 to set only cost price
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }

        private void Clear()
        {
            this.txtVendor.Text = String.Empty;
            if (!txtVendor.Enabled) txtVendor.Enabled = true;   //Sprint-21 - 2207 05-Aug-2015 JY Added
            this.txtReference.Text = String.Empty;
            this.dtpRecvDate.Value = System.DateTime.Now;
            this.lblVendorName.Text = String.Empty;
        }

        private void SetNew()
        {
            oInvRecvHData = new InvRecvHeaderData();
            this.btnFillFromPO.Tag = "";
            Clear();
            oInvRecvHRow = oInvRecvHData.InvRecievedHeader.AddRow(0, "", System.DateTime.MinValue, 0, 0, "");
            oInvRecvDData = new InvRecvDetailData();
            this.grdDetail.DataSource = oInvRecvDData;
            this.grdDetail.Refresh();
            ApplyGrigFormat();
            this.txtVendor.Focus();
            #region Sprint-21 - 2002 22-Jul-2015 JY Added
            txtGrandTotalCost.Text = "0";
            txtEditorNoOfItems.Text = "0";
            #endregion
        }

        private void txtVendor_EditorButtonClick(object sender, Infragistics.Win.UltraWinEditors.EditorButtonEventArgs e)
        {
            SearchVendor();
        }

        private void SearchVendor()
        {
            try
            {
                //frmSearch oSearch = new frmSearch(clsPOSDBConstants.Vendor_tbl);
                frmSearchMain oSearch = new frmSearchMain(clsPOSDBConstants.Vendor_tbl, this.txtVendor.Text, "", true);    //20-Dec-2017 JY Added new reference
                //Added by Krishna on 4 May 2011
                //oSearch.txtCode.Text = this.txtVendor.Text;

                //oSearch.btnAdd.Visible = true;
                //oSearch.btnEdit.Visible = true;

                oSearch.SearchInConstructor = true;
                //Till here by Krishna
                oSearch.ActiveOnly = 1;
                oSearch.ShowDialog(this);
                if (!oSearch.IsCanceled)
                {
                    string strCode = oSearch.SelectedRowID();
                    if (strCode == "")
                    {
                        return;
                    }
                    FKEdit(strCode, clsPOSDBConstants.Vendor_tbl);
                }
                else
                    this.txtVendor.Focus();
            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }
        private void FKEdit(string code, string senderName, Boolean bSkipIncompleteItems = false, Boolean bMultiple = false)    //Sprint-27 - PRIMEPOS-2026 13-Oct-2017 JY Added optional parameter
        {
            if (senderName == clsPOSDBConstants.Vendor_tbl)
            {
                #region Vendor
                try
                {
                    POS_Core.BusinessRules.Vendor oVendor = new POS_Core.BusinessRules.Vendor();
                    VendorData oVendorData;
                    VendorRow oVendorRow = null;
                    oVendorData = oVendor.Populate(code);
                    if (oVendorData.Vendor.Count > 0)   //Sprint-21 - 2207 14-Aug-2015 JY Added to avoid "no row found at index 0 error"
                        oVendorRow = oVendorData.Vendor[0];
                    if (oVendorRow != null)
                    {
                        this.txtVendor.Text = oVendorRow.Vendorcode;
                        this.lblVendorName.Text = oVendorRow.Vendorname;
                        this.txtVendor.Tag = oVendorRow.VendorId;
                        #region Sprint-21 - 2207 11-Aug-2015 JY Added  
                        if (grdDetail.Rows.Count > 0 && this.txtVendor.Text.Trim().ToUpper() != grdDetail.Rows[0].Cells[clsPOSDBConstants.ItemVendor_Fld_VendorCode].Value.ToString().Trim().ToUpper())
                        {
                            RefreshVendorItemId();
                        }
                        #endregion
                    }
                    else
                    {
                        this.txtVendor.Text = String.Empty;
                        this.txtVendor.Tag = "0";
                        this.lblVendorName.Text = String.Empty;
                    }
                }
                catch (System.IndexOutOfRangeException)
                {
                    this.txtVendor.Text = String.Empty;
                    this.lblVendorName.Text = String.Empty;
                    this.txtVendor.Tag = "0";
                    SearchVendor();
                }
                catch (Exception exp)
                {
                    clsUIHelper.ShowErrorMsg(exp.Message);
                    this.txtVendor.Text = String.Empty;
                    this.lblVendorName.Text = String.Empty;
                    this.txtVendor.Tag = "0";
                    SearchVendor();
                }
                #endregion
            }
            else if (senderName == clsPOSDBConstants.POHeaderOnHold_tbl)
            {
                ProcessOnHoldPO(code);
            }
            else if (senderName == clsPOSDBConstants.POHeader_tbl)
            {
                #region PurchaseOrder
                try
                {
                    bool isEPO = false;
                    PurchaseOrder oPO = new PurchaseOrder();
                    PODetailData oPODData;
                    POHeaderData oPOHeaderData;

                    oPOHeaderData = oPO.PopulateHeader(Convert.ToInt32(code));
                    oPODData = oPO.PopulateDetail(Convert.ToInt32(code), bSkipIncompleteItems); //Sprint-27 - PRIMEPOS-2026 13-Oct-2017 JY Added optional parameter

                    System.String VendorID = oPOHeaderData.POHeader[0].VendorCode;
                    if (VendorID != "")
                    {
                        this.txtVendor.Text = oPOHeaderData.POHeader[0].VendorCode.Trim();
                        this.txtVendor.Tag = oPOHeaderData.POHeader[0].VendorID.ToString().Trim();
                        this.lblVendorName.Text = oPOHeaderData.POHeader[0].VendorName.Trim();

                        POS_Core.BusinessRules.Vendor vendor = new POS_Core.BusinessRules.Vendor();
                        VendorData vendData = new VendorData();
                        vendData = vendor.Populate(oPOHeaderData.POHeader[0].VendorID);
                        if (vendData.Vendor.Rows.Count > 0)
                            isEPO = vendData.Vendor[0].USEVICForEPO;
                    }

                    this.txtReference.Text = oPOHeaderData.POHeader[0].OrderID.ToString().Trim();
                    InvRecvDetailRow oRow = null;
                    oInvRecvDData = new InvRecvDetailData();

                    //try
                    //{
                    foreach (PODetailRow oPODRow in oPODData.PODetail.Rows)
                    {
                        oRow = oInvRecvDData.InvRecvDetail.AddRow(0, 0, oPODRow.QTY, oPODRow.Cost, oPODRow.Cost, oPODRow.ItemID, "", oPODRow.QTY);
                        oRow.ItemDescription = oPODRow.ItemDescription.ToString();
                        oRow.ItemID = oPODRow.ItemID.ToString();

                        #region PRIMEPOS-2396 12-Jun-2018 JY Added
                        oRow.QtyInStock = oPODRow.QtyInStock;
                        PhysicalInv oPhysicalInv = new PhysicalInv();
                        oRow.LastInvUpdatedQty = oPhysicalInv.GetLastInvUpdatedQty(oRow.ItemID);
                        #endregion

                        //Commented By SRT(Abhishek) Date : 01/15/2009
                        // As it is not used by vendor now
                        //if (oPOHeaderData.POHeader[0].isFTPUsed==2)
                        //{

                        if (oPOHeaderData.POHeader[0].AckStatus == POAckType.RejectedNoDetail)
                        {
                            oRow.QTY = 0;
                        }
                        else if (oPOHeaderData.POHeader[0].AckStatus == POAckType.AcknowledgeWithDetailNoChange)
                        {
                            oRow.QTY = Convert.ToInt32(oPODRow.QTY.ToString());
                        }
                        else
                        {
                            if (oPODRow.AckStatus == ItemAckStatus.itemRejected)
                            {
                                oRow.QTY = 0;
                            }
                            else if (oPODRow.AckStatus == ItemAckStatus.itemAcceptedQuantityChanged)
                            {
                                oRow.QTY = Convert.ToInt32(oPODRow.AckQTY.ToString());
                            }
                            else if (oPODRow.AckStatus == ItemAckStatus.itemAcceptedSubstitutionMade)
                            {
                                oRow.QTY = Convert.ToInt32(oPODRow.AckQTY.ToString());
                                if (oPODRow.ChangedProductID.Trim() != "")  //02-Sep-2015 JY Added condition to handle showing blank itemid on inventory received screen
                                {
                                    Item item = new Item();
                                    ItemData itemData = new ItemData();
                                    itemData = item.Populate(oPODRow.ChangedProductID);
                                    if (itemData.Tables[0].Rows.Count > 0)
                                    {
                                        oRow.ItemID = oPODRow.ChangedProductID;
                                        oRow.ItemDescription = itemData.Item[0].Description;
                                    }
                                }
                            }
                            else
                            {
                                oRow.QTY = Convert.ToInt32(oPODRow.QTY.ToString());
                            }
                        }
                        oRow.Comments = "";
                        oRow.Cost = Convert.ToDecimal(oPODRow.Cost.ToString());
                        oRow.SalePrice = Configuration.convertNullToDecimal(oPODRow.Price);    //PRIMEPOS-3124 19-Sep-2022 JY replaced decimal.Zero by selling price//Convert.ToDecimal(oPODRow.Cost);//Orignal
                        oRow.QtyOrdered = oPODRow.QTY;

                        #region Sprint-21 - 2002 21-Jul-2015 JY Added
                        if (Configuration.convertNullToDecimal(oRow.QTY) != 0 && Configuration.convertNullToDecimal(oRow.Cost) != 0)
                            oRow.TotalCost = oRow.QTY * oRow.Cost;
                        else
                            oRow.TotalCost = 0;
                        #endregion

                        oRow.VendorItemID = Configuration.convertNullToString(oPODRow.VendorItemCode); //Sprint-21 - 2207 04-Aug-2015 JY Added
                        oRow.VendorCode = txtVendor.Text.Trim();    //Sprint-21 - 2207 04-Aug-2015 JY Added

                        #region PRIMEPOS-2725 29-Aug-2019 JY Added logic to load other columns in grid
                        Item oItem = new Item();
                        DataTable dtItem = oItem.GetItemDetailsWithVendor(oRow.ItemID, this.txtVendor.Text);  //PRIMEPOS-2395 22-Jun-2018 JY Added
                        if (dtItem != null && dtItem.Rows.Count > 0)
                        {
                            oRow.DeptID = Configuration.convertNullToInt(dtItem.Rows[0][clsPOSDBConstants.InvRecvDetail_Fld_DeptID]);
                            oRow.DeptCode = Configuration.convertNullToString(dtItem.Rows[0][clsPOSDBConstants.Department_Fld_DeptCode]);
                            oRow.SubDepartmentID = Configuration.convertNullToInt(dtItem.Rows[0][clsPOSDBConstants.InvRecvDetail_Fld_SubDepartmentID]);
                            oRow.DeptName = Configuration.convertNullToString(dtItem.Rows[0][clsPOSDBConstants.Department_Fld_DeptName]);
                            oRow.SubDept = Configuration.convertNullToString(dtItem.Rows[0][strSubDept]);
                            oRow.IsEBTItem = Configuration.convertNullToBoolean(dtItem.Rows[0][clsPOSDBConstants.InvRecvDetail_Fld_IsEBTItem]);
                            try
                            {
                                oRow.ExpDate = dtItem.Rows[0]["ExpDate"];
                            }
                            catch
                            {
                                //Added try catch to avoid exception in case of NULL/invalid date  
                            }
                        }
                        #endregion
                    }
                    //}
                    //catch (Exception Ex)
                    //{ }
                    GetPOData(oPOHeaderData, oPODData);

                    //Added by SRT(Abhishek) Date : 22/09/2009
                    PurchseOrdStatus poStatus = (PurchseOrdStatus)oPOHeaderData.POHeader[0].Status;
                    //End Of Added by SRT(Abhishek) Date : 22/09/2009


                    if (isEPO && poStatus != PurchseOrdStatus.AcknowledgeManually)
                    {
                        //Following iline is commented by shitaljit on 6 dec 2012
                        //As requested by Fahad that users should be allowed to change Qty.
                        //this.grdDetail.DisplayLayout.Bands[0].Columns["Qty"].CellActivation = Activation.Disabled;
                        this.grdDetail.DisplayLayout.Bands[0].Columns["Qty"].CellActivation = Activation.AllowEdit;
                        this.grdDetail.DisplayLayout.Bands[0].Columns["Cost"].CellActivation = Activation.Disabled;
                        this.grdDetail.DisplayLayout.Bands[0].Columns["SalePrice"].CellActivation = Activation.Disabled;
                        this.btnNew.Enabled = false;
                    }
                    //Added by SRT(Abhishek) Date : 25 Aug 2009
                    //Added to check if it is called from FillFromPO buton 
                    if (isFillFromPO)
                    {
                        this.grdDetail.DisplayLayout.Bands[0].Columns["Qty"].CellActivation = Activation.AllowEdit;
                        this.grdDetail.DisplayLayout.Bands[0].Columns["Cost"].CellActivation = Activation.AllowEdit;
                        this.grdDetail.DisplayLayout.Bands[0].Columns["SalePrice"].CellActivation = Activation.AllowEdit;
                        this.btnNew.Enabled = true;
                        isFillFromPO = false;
                    }
                    //End of Added by SRT(Abhishek) Date : 25 Aug 2009
                    txtVendor.Enabled = false;
                    this.grdDetail.DataSource = oInvRecvDData;
                    this.grdDetail.Refresh();
                    #region Sprint-21 - 2002 22-Jul-2015 JY Added
                    if (grdDetail.Rows.Count > 0)
                        CalcGrandTotalCost();
                    else
                    {
                        txtGrandTotalCost.Text = "0";
                        txtEditorNoOfItems.Text = "0";
                    }
                    #endregion
                }
                catch (System.IndexOutOfRangeException)
                {
                    this.txtVendor.Text = String.Empty;
                    this.lblVendorName.Text = String.Empty;
                    this.txtVendor.Tag = "0";
                    SearchVendor();
                }
                catch (Exception exp)
                {
                    clsUIHelper.ShowErrorMsg(exp.Message);
                    this.txtVendor.Text = String.Empty;
                    this.lblVendorName.Text = String.Empty;
                    this.txtVendor.Tag = "0";
                    SearchVendor();
                }
                #endregion
            }
            else if (senderName == clsPOSDBConstants.Item_tbl)
            {
                #region Items
                try
                {
                    if (bMultiple == false) //specific item
                    {
                        Item oItem = new Item();
                        //ItemData oItemData;   
                        //ItemRow oItemRow = null;
                        //oItemData = oItem.Populate(code);
                        DataTable dtItem = oItem.GetItemDetailsWithVendor(code, this.txtVendor.Text);  //PRIMEPOS-2395 22-Jun-2018 JY Added

                        //Added By shitaljit(QuicSolv)on 5 Dec 2011
                        //if (oItemData.Tables[0].Rows.Count == 0)
                        if (dtItem != null && dtItem.Rows.Count == 0)   //PRIMEPOS-2395 22-Jun-2018 JY Added
                        {
                            if (POS_Core.Resources.UserPriviliges.IsUserHasPriviliges(POS_Core.Resources.UserPriviliges.Modules.InventoryMgmt.ID, POS_Core.Resources.UserPriviliges.Screens.ItemFile.ID, -998))
                            {
                                frmItems ofrmItem = new frmItems(code, this.txtVendor.Text);
                                ofrmItem.numQtyInStock.ReadOnly = true;
                                ofrmItem.numQtyInStock.Enabled = false;
                                ofrmItem.ShowDialog();
                                //oItemData = oItem.Populate(code); 
                                dtItem = oItem.GetItemDetailsWithVendor(code, this.txtVendor.Text);  //PRIMEPOS-2395 22-Jun-2018 JY Added
                            }
                        }
                        ////END of added by shitaljit
                        //oItemRow = oItemData.Item[0];
                        //if (oItemRow != null)
                        if (dtItem != null && dtItem.Rows.Count > 0)    //PRIMEPOS-2395 22-Jun-2018 JY Added
                        {
                            if (grdDetail.ActiveRow == null)
                                this.grdDetail.Rows.Band.AddNew();
                            this.grdDetail.ActiveCell.Value = Configuration.convertNullToString(dtItem.Rows[0]["ItemId"]);
                            this.grdDetail.ActiveRow.Cells[clsPOSDBConstants.Item_Fld_Description].Value = Configuration.convertNullToString(dtItem.Rows[0]["Description"]);

                            #region PRIMEPOS-2396 12-Jun-2018 JY Added
                            this.grdDetail.ActiveRow.Cells[clsPOSDBConstants.Item_Fld_QtyInStock].Value = Configuration.convertNullToInt(dtItem.Rows[0]["QtyInStock"]);
                            this.grdDetail.ActiveRow.Cells[clsPOSDBConstants.InvRecvDetail_Fld_LastInvUpdatedQty].Value = Configuration.convertNullToInt(dtItem.Rows[0]["LastInvUpdatedQty"]);
                            #endregion

                            this.grdDetail.ActiveRow.Cells[clsPOSDBConstants.InvRecvDetail_Fld_Cost].Value = Configuration.convertNullToDecimal(dtItem.Rows[0]["LastCostPrice"]);
                            this.grdDetail.ActiveRow.Cells[clsPOSDBConstants.InvRecvDetail_Fld_SalePrice].Value = Configuration.convertNullToDecimal(dtItem.Rows[0]["SellingPrice"]);
                            try
                            {
                                this.grdDetail.ActiveRow.Cells["ExpDate"].Value = dtItem.Rows[0]["ExpDate"]; //Sprint-26 - PRIMEPOS-2387 14-Jul-2017 JY Added
                            }
                            catch
                            {
                                //Added try catch to avoid exception in case of NULL/invalid date  
                            }

                            #region Sprint-21 - 2207 04-Aug-2015 JY Added
                            this.grdDetail.ActiveRow.Cells[clsPOSDBConstants.ItemVendor_Fld_VendorItemID].Value = Configuration.convertNullToString(dtItem.Rows[0]["VendorItemID"]);
                            this.grdDetail.ActiveRow.Cells[clsPOSDBConstants.Vendor_Fld_VendorCode].Value = Configuration.convertNullToString(txtVendor.Text.Trim());
                            #endregion

                            #region PRIMEPOS-2419 09-May-2019 JY Added
                            this.grdDetail.ActiveRow.Cells[clsPOSDBConstants.InvRecvDetail_Fld_DeptID].Value = Configuration.convertNullToInt(dtItem.Rows[0][clsPOSDBConstants.InvRecvDetail_Fld_DeptID]);
                            this.grdDetail.ActiveRow.Cells[clsPOSDBConstants.Department_Fld_DeptCode].Value = Configuration.convertNullToString(dtItem.Rows[0][clsPOSDBConstants.Department_Fld_DeptCode]);
                            this.grdDetail.ActiveRow.Cells[clsPOSDBConstants.Department_Fld_DeptName].Value = Configuration.convertNullToString(dtItem.Rows[0][clsPOSDBConstants.Department_Fld_DeptName]);
                            this.grdDetail.ActiveRow.Cells[clsPOSDBConstants.InvRecvDetail_Fld_SubDepartmentID].Value = Configuration.convertNullToInt(dtItem.Rows[0][clsPOSDBConstants.InvRecvDetail_Fld_SubDepartmentID]);
                            this.grdDetail.ActiveRow.Cells[strSubDept].Value = Configuration.convertNullToString(dtItem.Rows[0][strSubDept]);
                            this.grdDetail.ActiveRow.Cells[clsPOSDBConstants.InvRecvDetail_Fld_IsEBTItem].Value = Configuration.convertNullToBoolean(dtItem.Rows[0][clsPOSDBConstants.InvRecvDetail_Fld_IsEBTItem]);
                            #endregion
                        }
                    }
                    else //PRIMEPOS-2395 22-Jun-2018 JY Added
                    {
                        try
                        {
                            Item oItem = new Item();
                            DataTable dtItem = oItem.GetItemDetailsWithVendor(code, this.txtVendor.Text);  //PRIMEPOS-2395 22-Jun-2018 JY Added
                            //if (dtItem != null && dtItem.Rows.Count == 0)
                            //{
                            //    if (POS.Resources.UserPriviliges.IsUserHasPriviliges(POS.Resources.UserPriviliges.Modules.InventoryMgmt.ID, POS.Resources.UserPriviliges.Screens.ItemFile.ID, -998))
                            //    {
                            //        frmItems ofrmItem = new frmItems(code, this.txtVendor.Text);
                            //        ofrmItem.numQtyInStock.ReadOnly = true;
                            //        ofrmItem.numQtyInStock.Enabled = false;
                            //        ofrmItem.ShowDialog();
                            //        //oItemData = oItem.Populate(code);
                            //        dtItem = oItem.GetItemDetailsWithVendor(code, this.txtVendor.Text);  //PRIMEPOS-2395 22-Jun-2018 JY Added
                            //    }
                            //}
                            if (dtItem != null && dtItem.Rows.Count > 0)    //PRIMEPOS-2395 22-Jun-2018 JY Added
                            {
                                InvRecvDetailRow oInvRecvDetailRow = oInvRecvDData.InvRecvDetail.AddRow(0, 0, 1, Configuration.convertNullToDecimal(dtItem.Rows[0]["SellingPrice"]), Configuration.convertNullToDecimal(dtItem.Rows[0]["LastCostPrice"]), Configuration.convertNullToString(dtItem.Rows[0]["ItemId"]), "", 0);
                                //assign values to the last row of grid
                                grdDetail.Rows[grdDetail.Rows.Count - 1].Activate();
                                this.grdDetail.ActiveRow.Cells[clsPOSDBConstants.Item_Fld_ItemID].Value = Configuration.convertNullToString(dtItem.Rows[0]["ItemId"]);
                                this.grdDetail.ActiveRow.Cells[clsPOSDBConstants.Item_Fld_Description].Value = Configuration.convertNullToString(dtItem.Rows[0]["Description"]);
                                this.grdDetail.ActiveRow.Cells[clsPOSDBConstants.InvRecvDetail_Fld_QTY].Value = 1;
                                #region PRIMEPOS-2396 12-Jun-2018 JY Added
                                this.grdDetail.ActiveRow.Cells[clsPOSDBConstants.Item_Fld_QtyInStock].Value = Configuration.convertNullToInt(dtItem.Rows[0]["QtyInStock"]);
                                this.grdDetail.ActiveRow.Cells[clsPOSDBConstants.InvRecvDetail_Fld_LastInvUpdatedQty].Value = Configuration.convertNullToInt(dtItem.Rows[0]["LastInvUpdatedQty"]);
                                #endregion

                                this.grdDetail.ActiveRow.Cells[clsPOSDBConstants.InvRecvDetail_Fld_Cost].Value = Configuration.convertNullToDecimal(dtItem.Rows[0]["LastCostPrice"]);
                                this.grdDetail.ActiveRow.Cells[clsPOSDBConstants.InvRecvDetail_Fld_SalePrice].Value = Configuration.convertNullToDecimal(dtItem.Rows[0]["SellingPrice"]);
                                try
                                {
                                    this.grdDetail.ActiveRow.Cells["ExpDate"].Value = dtItem.Rows[0]["ExpDate"]; //Sprint-26 - PRIMEPOS-2387 14-Jul-2017 JY Added
                                }
                                catch
                                {
                                    //Added try catch to avoid exception in case of NULL/invalid date  
                                }
                                this.grdDetail.ActiveRow.Cells[clsPOSDBConstants.ItemVendor_Fld_VendorItemID].Value = Configuration.convertNullToString(dtItem.Rows[0]["VendorItemID"]);
                                this.grdDetail.ActiveRow.Cells[clsPOSDBConstants.Vendor_Fld_VendorCode].Value = Configuration.convertNullToString(txtVendor.Text.Trim());

                                #region PRIMEPOS-2419 09-May-2019 JY Added
                                this.grdDetail.ActiveRow.Cells[clsPOSDBConstants.InvRecvDetail_Fld_DeptID].Value = Configuration.convertNullToInt(dtItem.Rows[0][clsPOSDBConstants.InvRecvDetail_Fld_DeptID]);
                                this.grdDetail.ActiveRow.Cells[clsPOSDBConstants.Department_Fld_DeptCode].Value = Configuration.convertNullToString(dtItem.Rows[0][clsPOSDBConstants.Department_Fld_DeptCode]);
                                this.grdDetail.ActiveRow.Cells[clsPOSDBConstants.Department_Fld_DeptName].Value = Configuration.convertNullToString(dtItem.Rows[0][clsPOSDBConstants.Department_Fld_DeptName]);
                                this.grdDetail.ActiveRow.Cells[clsPOSDBConstants.InvRecvDetail_Fld_SubDepartmentID].Value = Configuration.convertNullToInt(dtItem.Rows[0][clsPOSDBConstants.InvRecvDetail_Fld_SubDepartmentID]);
                                this.grdDetail.ActiveRow.Cells[strSubDept].Value = Configuration.convertNullToString(dtItem.Rows[0][strSubDept]);
                                this.grdDetail.ActiveRow.Cells[clsPOSDBConstants.InvRecvDetail_Fld_IsEBTItem].Value = Configuration.convertNullToBoolean(dtItem.Rows[0][clsPOSDBConstants.InvRecvDetail_Fld_IsEBTItem]);
                                #endregion
                            }
                        }
                        catch (System.IndexOutOfRangeException)
                        {
                            this.grdDetail.Focus();
                        }
                        catch (Exception exp)
                        {
                            clsUIHelper.ShowErrorMsg(exp.Message);
                            this.grdDetail.Focus();
                        }
                    }
                }
                catch (System.IndexOutOfRangeException)
                {
                    this.grdDetail.ActiveCell.Value = String.Empty;
                    this.grdDetail.ActiveRow.Cells["Description"].Value = String.Empty;
                }
                catch (Exception exp)
                {
                    clsUIHelper.ShowErrorMsg(exp.Message);
                    this.grdDetail.ActiveCell.Value = String.Empty;
                    this.grdDetail.ActiveRow.Cells["Description"].Value = String.Empty;
                }
                #endregion
            }
            else if (senderName == clsPOSDBConstants.Department_tbl)
            {
                #region Department
                if (bMultiple == false) //specific item
                {
                    Department oDepartment = new Department();
                    DepartmentData oDepartmentData = oDepartment.Populate(code);
                    if (oDepartmentData != null && oDepartmentData.Tables.Count > 0 && oDepartmentData.Tables[0].Rows.Count > 0)
                    {
                        DepartmentRow oDepartmentRow = (DepartmentRow)oDepartmentData.Tables[0].Rows[0];
                        int nPreviousDeptId = Configuration.convertNullToInt(this.grdDetail.ActiveRow.Cells[clsPOSDBConstants.Department_Fld_DeptID].Value);
                        this.grdDetail.ActiveRow.Cells[clsPOSDBConstants.Department_Fld_DeptCode].Value = code.Trim();
                        this.grdDetail.ActiveRow.Cells[clsPOSDBConstants.Department_Fld_DeptID].Value = oDepartmentRow.DeptID.ToString();
                        this.grdDetail.ActiveRow.Cells[clsPOSDBConstants.Department_Fld_DeptName].Value = oDepartmentRow.DeptName;

                        if (nPreviousDeptId != oDepartmentRow.DeptID)
                        {
                            this.grdDetail.ActiveRow.Cells[clsPOSDBConstants.SubDepartment_Fld_SubDepartmentID].Value = 0;
                            this.grdDetail.ActiveRow.Cells[strSubDept].Value = "";
                        }
                    }
                    else
                    {
                        this.grdDetail.ActiveRow.Cells[clsPOSDBConstants.Department_Fld_DeptCode].Value = "";
                        this.grdDetail.ActiveRow.Cells[clsPOSDBConstants.Department_Fld_DeptID].Value = 0;
                        this.grdDetail.ActiveRow.Cells[clsPOSDBConstants.Department_Fld_DeptName].Value = "";
                        this.grdDetail.ActiveRow.Cells[clsPOSDBConstants.SubDepartment_Fld_SubDepartmentID].Value = 0;
                        this.grdDetail.ActiveRow.Cells[strSubDept].Value = "";
                    }
                }
                else
                {

                }
                #endregion
            }
            else if (senderName == clsPOSDBConstants.SubDepartment_tbl)
            {
                if (bMultiple == false) //specific item
                {
                    SubDepartment oSubDepartment = new SubDepartment();
                    SubDepartmentData oSubDepartmentData = oSubDepartment.PopulateList("AND DepartmentID = " + this.grdDetail.ActiveRow.Cells[clsPOSDBConstants.Department_Fld_DeptID].Value + " And Description = '" + code.Replace("'", "''") + "'");
                    if (oSubDepartmentData != null && oSubDepartmentData.Tables.Count > 0 && oSubDepartmentData.Tables[0].Rows.Count > 0)
                    {
                        SubDepartmentRow oSubDepartmentRow = (SubDepartmentRow)oSubDepartmentData.Tables[0].Rows[0];
                        this.grdDetail.ActiveRow.Cells[clsPOSDBConstants.SubDepartment_Fld_SubDepartmentID].Value = oSubDepartmentRow.SubDepartmentID;
                        this.grdDetail.ActiveRow.Cells[strSubDept].Value = oSubDepartmentRow.Description;
                    }
                    else
                    {
                        this.grdDetail.ActiveRow.Cells[clsPOSDBConstants.SubDepartment_Fld_SubDepartmentID].Value = 0;
                        this.grdDetail.ActiveRow.Cells[strSubDept].Value = "";
                    }
                }
                else
                {

                }
            }
            this.txtEditorNoOfItems.Text = this.grdDetail.Rows.Count.ToString();
        }

        /// <summary>
        /// Author:Shitaljit
        /// Added to process on hold POss
        /// </summary>
        /// <param name="code"></param>
        private void ProcessOnHoldPO(string code)
        {
            #region PurchaseOrder
            try
            {
                bool isEPO = false;

                PurchaseOrder oPO = new PurchaseOrder();
                PODetailData oPODData;
                POHeaderData oPOHeaderData;

                oPOHeaderData = oPO.PopulateHeader(Convert.ToInt32(code));
                oPODData = oPO.PopulateDetail(Convert.ToInt32(code));

                System.String VendorID = oPOHeaderData.POHeader[0].VendorCode;
                if (VendorID != "")
                {
                    this.txtVendor.Text = oPOHeaderData.POHeader[0].VendorCode.Trim();
                    this.txtVendor.Tag = oPOHeaderData.POHeader[0].VendorID.ToString().Trim();
                    this.lblVendorName.Text = oPOHeaderData.POHeader[0].VendorName.Trim();

                    POS_Core.BusinessRules.Vendor vendor = new POS_Core.BusinessRules.Vendor();
                    VendorData vendData = new VendorData();
                    vendData = vendor.Populate(oPOHeaderData.POHeader[0].VendorID);
                    if (vendData.Vendor.Rows.Count > 0)
                        isEPO = vendData.Vendor[0].USEVICForEPO;
                }

                this.txtReference.Text = oPOHeaderData.POHeader[0].OrderID.ToString().Trim();
                InvRecvDetailRow oRow = null;
                oInvRecvDData = new InvRecvDetailData();

                foreach (PODetailRow oPODRow in oPODData.PODetail.Rows)
                {
                    oRow = oInvRecvDData.InvRecvDetail.AddRow(0, 0, oPODRow.QTY, oPODRow.Cost, oPODRow.Cost, "", "", oPODRow.QTY);
                    oRow.ItemDescription = oPODRow.ItemDescription.ToString();
                    oRow.Comments = "";
                    oRow.Cost = Convert.ToDecimal(oPODRow.Cost.ToString());
                    oRow.SalePrice = decimal.Zero;
                    oRow.QtyOrdered = oPODRow.QTY;
                }

                GetPOData(oPOHeaderData, oPODData);

                if (isFillFromPO)
                {
                    this.grdDetail.DisplayLayout.Bands[0].Columns["Qty"].CellActivation = Activation.AllowEdit;
                    this.grdDetail.DisplayLayout.Bands[0].Columns["Cost"].CellActivation = Activation.AllowEdit;
                    this.grdDetail.DisplayLayout.Bands[0].Columns["SalePrice"].CellActivation = Activation.AllowEdit;
                    this.btnNew.Enabled = true;
                    isFillFromPO = false;
                }
                txtVendor.Enabled = false;
                btnDeleteItem.Enabled = true;
                this.grdDetail.DataSource = oInvRecvDData;
                this.grdDetail.Refresh();
            }
            catch (System.IndexOutOfRangeException)
            {
                this.txtVendor.Text = String.Empty;
                this.lblVendorName.Text = String.Empty;
                this.txtVendor.Tag = "0";
                SearchVendor();
            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
                this.txtVendor.Text = String.Empty;
                this.lblVendorName.Text = String.Empty;
                this.txtVendor.Tag = "0";
                SearchVendor();
            }
            #endregion
        }
        private void GetPOData(POHeaderData oPOHeaderData, PODetailData oPODData)
        {
            poHeadData = new POHeaderData();
            poDetailData = new PODetailData();

            poHeadData.Merge(oPOHeaderData, true);
            poDetailData.Merge(oPODData, true);
        }

        private void txtVendor_Leave(object sender, System.EventArgs e)
        {
            try
            {
                string txtValue = this.txtVendor.Text;
                if (txtValue.Trim() == "")
                    return;
                FKEdit(txtValue, clsPOSDBConstants.Vendor_tbl);

                //Added by shitalit(QuicSolv) on 11 Oct 2011
                POS_Core.BusinessRules.Vendor oVendor = new POS_Core.BusinessRules.Vendor();
                VendorData oVendorData;
                VendorRow oVendorRow = null;
                oVendorData = oVendor.Populate(txtValue);
                if (oVendorData.Vendor.Count > 0)   //Sprint-21 - 2207 14-Aug-2015 JY Added to avoid "no row found at index 0 error"
                    oVendorRow = oVendorData.Vendor[0];
                else
                    return;
                Notes oNotes = new Notes();
                NotesData oNotesData = new NotesData();
                string whereClause = " WHERE " + clsPOSDBConstants.Notes_Fld_EntityId + "= '" + oVendorRow.VendorId.ToString() + "'  AND  " + clsPOSDBConstants.Notes_Fld_EntityType + "= '" + clsEntityType.VendorNote + "' AND " + clsPOSDBConstants.Notes_Fld_POPUPMSG + "= '" + true + "'";
                oNotesData = oNotes.PopulateList(whereClause);
                if (oNotesData.Notes.Rows.Count > 0)
                {
                    frmCustomerNotesView ofrmCustomerNotesView = new frmCustomerNotesView(oVendorRow.VendorId.ToString(), clsEntityType.VendorNote);
                    ofrmCustomerNotesView.ShowDialog();
                }
                //END of Added by shitalit(QuicSolv) on 11 Oct 2011
                if (txtVendor.Text != "" && RecInventoryFlag == true)
                {
                    txtReference.Focus();
                }
            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }

        private void btnClose_Click(object sender, System.EventArgs e)
        {
            this.Close();
        }

        private void grdDetail_Enter(object sender, System.EventArgs e)
        {
            try
            {
                if (this.grdDetail.Rows.Count > 0)
                {
                    #region Sprint-21 - 2207 05-Aug-2015 JY Added
                    if (txtVendor.Text.Trim() == "")
                    {
                        Resources.Message.Display("Please select vendor", "Prime POS", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        if (!txtVendor.Enabled) txtVendor.Enabled = true;
                        txtVendor.Focus();
                        return;
                    }
                    #endregion

                    if (!this.grdDetail.Rows[0].Cells["Qty"].Activated)
                        this.grdDetail.Rows[0].Cells[clsPOSDBConstants.Item_Fld_ItemID].Activate();
                    this.grdDetail.PerformAction(UltraGridAction.EnterEditMode);
                }
                else
                {
                    this.grdDetail.Rows.Band.AddNew();
                }
            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }

        private void grdDetail_ClickCellButton(object sender, Infragistics.Win.UltraWinGrid.CellEventArgs e)
        {
            try
            {
                if (m_exceptionAccoured)
                {
                    m_exceptionAccoured = false;
                    return;
                }

                #region Sprint-21 - 2207 05-Aug-2015 JY Added
                if (e.Cell.Column.Key == clsPOSDBConstants.InvRecvDetail_Fld_ItemID || e.Cell.Column.Key == clsPOSDBConstants.Department_Fld_DeptName || e.Cell.Column.Key == strSubDept)
                {
                    if (txtVendor.Text.Trim() == "")
                    {
                        Resources.Message.Display("Please select vendor", "Prime POS", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        if (!txtVendor.Enabled) txtVendor.Enabled = true;
                        txtVendor.Focus();
                        return;
                    }
                }
                #endregion

                if (e.Cell.Column.Key == clsPOSDBConstants.InvRecvDetail_Fld_ItemID)
                {
                    SearchItem();
                }
                #region PRIMEPOS-2419 24-May-2019 JY Added
                else if (e.Cell.Column.Key == clsPOSDBConstants.Department_Fld_DeptCode)
                {
                    SearchDept();
                }
                else if (e.Cell.Column.Key == strSubDept)
                {
                    if (Configuration.convertNullToString(this.grdDetail.ActiveRow.Cells[clsPOSDBConstants.Department_Fld_DeptCode].Value) == "")
                    {
                        this.grdDetail.ActiveRow.Cells[strSubDept].Value = "";
                        Resources.Message.Display("Please select department", "Prime POS", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        //UltraGridCell aCell = this.grdDetail.ActiveRow.Cells[clsPOSDBConstants.Department_Fld_DeptCode];
                        //this.grdDetail.ActiveCell = aCell;
                        //this.grdDetail.Focus();
                        //this.grdDetail.ActiveCell.Selected = true;
                        //this.grdDetail.ActiveCell.Activate();
                        //this.grdDetail.PerformAction(UltraGridAction.EnterEditMode, false, false);
                        return;
                    }
                    else
                    {
                        SearchSubDept();
                    }
                }
                #endregion
            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }

        private void SearchItem()
        {
            try
            {
                //frmSearch oSearch = new frmSearch(clsPOSDBConstants.Item_tbl);
                frmSearchMain oSearch = new frmSearchMain(true);    //20-Dec-2017 JY Added new reference
                oSearch.SearchTable = clsPOSDBConstants.Item_tbl;  //20-Dec-2017 JY Added 
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

        private void grdDetail_BeforeCellDeactivate(object sender, System.ComponentModel.CancelEventArgs e)
        {
            UltraGridCell oCurrentCell;
            oCurrentCell = this.grdDetail.ActiveCell;
            try
            {
                if (oCurrentCell == null) return;   //Sprint-21 - 2002 22-Jul-2015 JY Added to resolve object reference issue
                //if (grdDetail.Rows.Count > 0) return; //Sprint-21 - 2002 22-Jul-2015 JY Added to resolve object reference issue

                if (oCurrentCell.DataChanged == false)
                {
                    txtEditorNoOfItems.Text = grdDetail.Rows.Count.ToString();    //Sprint-21 - 2002 22-Jul-2015 JY Added to correct item count
                    return;
                }
            }
            catch (Exception ex)
            {
            }
            try
            {
                if (oCurrentCell.Column.Key == clsPOSDBConstants.InvRecvDetail_Fld_ItemID && oCurrentCell.Value.ToString() != "")
                {
                    FKEdit(oCurrentCell.Value.ToString(), clsPOSDBConstants.Item_tbl);
                    if (oCurrentCell.Value.ToString() == "")
                    {
                        e.Cancel = true;
                        this.grdDetail.PerformAction(UltraGridAction.EnterEditMode);
                    }
                }
                else if (oCurrentCell.Column.Key == clsPOSDBConstants.InvRecvDetail_Fld_QTY)
                {
                    oInvRecv.Validate_Qty(oCurrentCell.Text.ToString(), isFromPurchaseOrder);
                }
                else if (oCurrentCell.Column.Key == clsPOSDBConstants.InvRecvDetail_Fld_Cost)
                {
                    oInvRecv.Validate_Cost(oCurrentCell.Text.ToString());
                }
                else if (oCurrentCell.Column.Key == clsPOSDBConstants.InvRecvDetail_Fld_SalePrice)
                {
                    oInvRecv.Validate_SalePrice(oCurrentCell.Text.ToString());
                }
                #region PRIMEPOS-2419 28-May-2019 JY Added
                else if (oCurrentCell.Column.Key == clsPOSDBConstants.Department_Fld_DeptCode && oCurrentCell.Value.ToString() != "")
                {
                    FKEdit(oCurrentCell.Value.ToString(), clsPOSDBConstants.Department_tbl);
                    if (oCurrentCell.Value.ToString() == "")
                    {
                        e.Cancel = true;
                        this.grdDetail.PerformAction(UltraGridAction.EnterEditMode);
                    }
                }
                else if (oCurrentCell.Column.Key == strSubDept && oCurrentCell.Value.ToString() != "")
                {
                    if (Configuration.convertNullToString(this.grdDetail.ActiveRow.Cells[clsPOSDBConstants.Department_Fld_DeptCode].Value) == "")
                    {
                        Resources.Message.Display("Please select department", "Prime POS", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        oCurrentCell.Value = "";
                        e.Cancel = true;
                        return;
                    }
                    else
                    {
                        if (oCurrentCell.Value.ToString().Trim() != "")
                        {
                            FKEdit(oCurrentCell.Value.ToString().Trim(), clsPOSDBConstants.SubDepartment_tbl);
                        }
                        else
                        {
                            oCurrentCell.Value = "";
                            e.Cancel = true;
                            this.grdDetail.PerformAction(UltraGridAction.EnterEditMode);
                        }
                    }
                }
                #endregion
            }
            catch (Exception exp)
            {
                m_exceptionAccoured = true;
                clsUIHelper.ShowErrorMsg(exp.Message);
                e.Cancel = true;
                this.grdDetail.PerformAction(UltraGridAction.EnterEditMode);
            }
        }


        private void ValidateRow(object sender, System.ComponentModel.CancelEventArgs e)
        {
            UltraGridRow oCurrentRow;
            UltraGridCell oCurrentCell;
            oCurrentRow = this.grdDetail.ActiveRow;
            oCurrentCell = null;
            bool blnCellChanged;
            blnCellChanged = false;
            bException = false; //Sprint-26 - PRIMEPOS-665 30-Jun-2017 JY Added

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
                oCurrentCell = oCurrentRow.Cells[clsPOSDBConstants.InvRecvDetail_Fld_ItemID];
                oInvRecv.Validate_ItemID(oCurrentCell.Text.ToString());

                oCurrentCell = oCurrentRow.Cells[clsPOSDBConstants.InvRecvDetail_Fld_QTY];
                oInvRecv.Validate_Qty(oCurrentCell.Text.ToString(), isFromPurchaseOrder);

                oCurrentCell = oCurrentRow.Cells[clsPOSDBConstants.InvRecvDetail_Fld_Cost];
                oInvRecv.Validate_Cost(oCurrentCell.Text.ToString());

                oCurrentCell = oCurrentRow.Cells[clsPOSDBConstants.InvRecvDetail_Fld_SalePrice];
                oInvRecv.Validate_SalePrice(oCurrentCell.Text.ToString());
            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
                if (oCurrentCell != null)
                {
                    bException = true;  //Sprint-26 - PRIMEPOS-665 30-Jun-2017 JY Added
                    m_exceptionAccoured = true;
                    e.Cancel = true;
                    this.grdDetail.ActiveCell = oCurrentCell;
                    this.grdDetail.PerformAction(UltraGridAction.ActivateCell);
                    this.grdDetail.PerformAction(UltraGridAction.EnterEditMode);
                }
            }
        }

        private void btnNew_Click(object sender, System.EventArgs e)
        {
            SetNew();
        }

        private void btnNew_ContextMenuChanged(object sender, System.EventArgs e)
        {

        }
        private void frmInventoryRecieved_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            try
            {
                #region PRIMEPOS-2822 17-Mar-2020 JY Added
                if (e.KeyData == System.Windows.Forms.Keys.Tab)
                {
                    if (this.grdDetail.ContainsFocus == true && this.grdDetail.ActiveCell.Text.Trim() == "" && this.grdDetail.ActiveCell.Column.Key == clsPOSDBConstants.InvRecvDetail_Fld_ItemID && this.grdDetail.ActiveCell.Row.IsAddRow == true)
                    {
                        //SearchItem();
                        if (pnlSave.Enabled == true && btnSave.Enabled == true)
                            btnSave.Focus();
                    }
                }
                #endregion

                if (e.KeyData == System.Windows.Forms.Keys.Enter)
                {
                    if (this.grdDetail.ContainsFocus == true && this.grdDetail.ActiveCell.Text.Trim() == "" && this.grdDetail.ActiveCell.Column.Key == clsPOSDBConstants.InvRecvDetail_Fld_ItemID && this.grdDetail.ActiveCell.Row.IsAddRow == true)
                    {
                        //this.SelectNextControl(this.grdDetail,true,true,true,true);   //PRIMEPOS-2822 17-Mar-2020 JY Commented
                        //SearchItem();   //PRIMEPOS-2822 17-Mar-2020 JY Added
                        if (pnlSave.Enabled == true && btnSave.Enabled == true)
                            btnSave.Focus();
                    }
                    else if (this.grdDetail.ContainsFocus == false)
                    {
                        this.SelectNextControl(this.ActiveControl, true, true, true, true);
                    }
                }
                #region PRIMEPOS-2396 12-Jun-2018 JY Added
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
                    btnDeleteItem_Click(btnDeleteItem, new EventArgs());
                    break;
                case Keys.L:
                    SetNew();
                    break;
                case Keys.P:
                    btnReport_Click(btnReport, new EventArgs());
                    break;
                case Keys.R:    //PRIMEPOS-2243 27-Apr-2021 JY Added
                    btnPrintShelfStickers_Click(btnPrintShelfStickers, new EventArgs());
                    break;
                case Keys.S:
                    btnSave_Click(btnSave, new EventArgs());
                    break;
                default:
                    break;
            }
        }
        #endregion

        private void frmInventoryRecieved_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            try
            {
                if (e.KeyData == System.Windows.Forms.Keys.F4)
                {
                    if (this.txtVendor.ContainsFocus == true)
                        this.SearchVendor();
                    else if (this.grdDetail.ContainsFocus == true)
                    {
                        if (this.grdDetail.ActiveCell != null)
                        {
                            if (this.grdDetail.ActiveCell.Column.Key == clsPOSDBConstants.PODetail_Fld_ItemID)
                                this.SearchItem();
                            #region PRIMEPOS-2419 30-May-2019 JY Added
                            else if (this.grdDetail.ActiveCell.Column.Key == clsPOSDBConstants.Department_Fld_DeptCode)
                                SearchDept();
                            else if (this.grdDetail.ActiveCell.Column.Key == strSubDept)
                            {
                                if (Configuration.convertNullToString(this.grdDetail.ActiveRow.Cells[clsPOSDBConstants.Department_Fld_DeptCode].Value) == "")
                                {
                                    Resources.Message.Display("Please select department", "Prime POS", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    this.grdDetail.ActiveRow.Cells[strSubDept].Value = "";
                                }
                                else
                                {
                                    SearchSubDept();
                                }

                            }
                            #endregion
                        }
                    }
                }
                else if (e.KeyData == System.Windows.Forms.Keys.F2)
                {
                    //AddRow();
                    if (nDisplayItemCost == 0)
                        btnAddItem_Click(sender, e);
                }
                else if (e.KeyData == System.Windows.Forms.Keys.F6)
                {
                    if (nDisplayItemCost == 0)
                        SearchPO();
                }
            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
            finally { e.Handled = true; }
        }

        private void AddRow()
        {
            try
            {
                this.grdDetail.Focus();
                this.grdDetail.PerformAction(UltraGridAction.LastCellInGrid);
                if (bException == false)  //Sprint-26 - PRIMEPOS-665 30-Jun-2017 JY Added)
                {
                    this.grdDetail.PerformAction(UltraGridAction.FirstCellInRow);
                    this.grdDetail.PerformAction(UltraGridAction.EnterEditMode);
                }
                bException = false; //Sprint-26 - PRIMEPOS-665 30-Jun-2017 JY Added)
            }
            catch (Exception) { }
        }

        private void frmInventoryRecieved_Activated(object sender, System.EventArgs e)
        {
            clsUIHelper.CurrentForm = this;
            this.Left = clsPOSDBConstants.formLeft;
            this.Top = clsPOSDBConstants.formLeft;
        }

        private void grdDetail_BeforeRowUpdate(object sender, Infragistics.Win.UltraWinGrid.CancelableRowEventArgs e)
        {
            #region Sprint-26 - PRIMEPOS-664 03-Jul-2017 JY Commented       
            //UltraGridRow oCurrentRow;
            //UltraGridCell oCurrentCell;
            //oCurrentRow = e.Row;
            //oCurrentCell = null;

            //try
            //{
            //    oCurrentCell = oCurrentRow.Cells[clsPOSDBConstants.InvRecvDetail_Fld_ItemID];
            //    oInvRecv.Validate_ItemID(oCurrentCell.Text.ToString());
            //   POS_Core.ErrorLogging.Logs.Logger("**Frm Inventory Recieved " + clsPOSDBConstants.InvRecvDetail_Fld_ItemID + ":" + oCurrentCell.Text.ToString());
            //    oCurrentCell = oCurrentRow.Cells[clsPOSDBConstants.InvRecvDetail_Fld_QTY];
            //    oInvRecv.Validate_Qty(oCurrentCell.Text.ToString());
            //   POS_Core.ErrorLogging.Logs.Logger("**Frm Inventory Recieved  " + clsPOSDBConstants.InvRecvDetail_Fld_QTY + ":" + oCurrentCell.Text.ToString());
            //    oCurrentCell = oCurrentRow.Cells[clsPOSDBConstants.InvRecvDetail_Fld_Cost];
            //    oInvRecv.Validate_Cost(oCurrentCell.Text.ToString());

            //    oCurrentCell = oCurrentRow.Cells[clsPOSDBConstants.InvRecvDetail_Fld_SalePrice];
            //    oInvRecv.Validate_SalePrice(oCurrentCell.Text.ToString());


            //}
            //catch (Exception exp)
            //{
            //    clsUIHelper.ShowErrorMsg(exp.Message);
            //    if (oCurrentCell != null)
            //    {
            //        this.grdDetail.ActiveCell = oCurrentCell;
            //        this.grdDetail.PerformAction(UltraGridAction.ActivateCell);
            //        this.grdDetail.PerformAction(UltraGridAction.EnterEditMode);
            //    }
            //    //e.Cancel=true;
            //}
            #endregion
        }

        private void btnAddItem_Click(object sender, System.EventArgs e)
        {
            POS_Core.ErrorLogging.Logs.Logger("**Frm Inventory Received Add multiple items");
            if (txtVendor.Text.Trim() == "")
            {
                Resources.Message.Display("Please select vendor", "Prime POS", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                if (!txtVendor.Enabled) txtVendor.Enabled = true;
                txtVendor.Focus();
                return;
            }
            AddMultipleItems(); //PRIMEPOS-2395 22-Jun-2018 JY Added
        }

        #region PRIMEPOS-2395 22-Jun-2018 JY Added
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
                            FKEdit(strItemId, clsPOSDBConstants.Item_tbl, true, true);  //PRIMEPOS-2395 22-Jun-2018 JY Added bMultiple parameter
                        }
                    }
                    this.grdDetail.Focus();
                    this.grdDetail.Refresh();
                }
            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }
        #endregion

        private void SearchPO()
        {
            try
            {
                POS_Core.ErrorLogging.Logs.Logger("**Frm Inventory Received Search PO");
                //frmSearch oSearch = new frmSearch(clsPOSDBConstants.POHeader_CompNotRecvd);
                frmSearchMain oSearch = new frmSearchMain(true);    //20-Dec-2017 JY Added new reference
                oSearch.SearchTable = clsPOSDBConstants.POHeader_CompNotRecvd;    //20-Dec-2017 JY Added 
                oSearch.ShowDialog(this);
                if (!oSearch.IsCanceled)
                {
                    string strCode = oSearch.SelectedRowID();
                    //Added by SRT(Abhishek) Date : 25 Aug 2009
                    isFillFromPO = true;
                    //End of Added by SRT(Abhishek)
                    FillFromPO(strCode);
                }
            }
            catch (Exception exp)
            {
                this.Cursor = Cursors.Default;
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }

        public void FillFromPO(string strCode, Boolean bSkipIncompleteItems = false)    //Sprint-27 - PRIMEPOS-2026 13-Oct-2017 JY Added optional parameter
        {
            if (strCode == "")
            {
                return;
            }
            POS_Core.ErrorLogging.Logs.Logger("**Frm Inventory Received FillFromPO ");
            System.Windows.Forms.Application.DoEvents();
            this.Cursor = Cursors.WaitCursor;
            FKEdit(strCode, clsPOSDBConstants.POHeader_tbl, bSkipIncompleteItems);
            try
            {
                POS_Core.ErrorLogging.Logs.Logger("**Frm Inventory Received FillFromPO  OrderID:" + this.txtReference.Text);
            }
            catch { }
            this.Cursor = Cursors.Default;
            this.btnFillFromPO.Tag = strCode;
        }

        private void btnFillFromPO_Click(object sender, System.EventArgs e)
        {
            SearchPO();
        }

        private void btnDeleteItem_Click(object sender, System.EventArgs e)
        {
            if (this.grdDetail.ActiveRow != null)
            {
                if (Resources.Message.Display("Are you sure, you want to delete the item?", "Delete Item", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                {
                    this.grdDetail.ActiveRow.Delete(false);
                    #region Sprint-21 - 2002 22-Jul-2015 JY Added
                    if (grdDetail.Rows.Count > 0)
                    {
                        txtEditorNoOfItems.Text = grdDetail.Rows.Count.ToString();
                        CalcGrandTotalCost();
                    }
                    else
                    {
                        txtGrandTotalCost.Text = "0";
                        txtEditorNoOfItems.Text = "0";
                    }
                    #endregion
                }
            }
        }

        private void cboTransType_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            this.cboTransType.Text = this.cboTransType.SelectedItem.DisplayText;
        }

        private void cboTransType_ValueChanged(object sender, System.EventArgs e)
        {
            //oInvRecvDData=new InvRecvDetailData();    //Sprint-22 - PRIMEPOS-2251 02-Dec-2015 JY commented because it blanks the grid if we change the TransType
            #region Sprint-22 - PRIMEPOS-2251 02-Dec-2015 JY Added
            if (grdDetail.Rows.Count > 0)
            {
                int nTransEffect = 0;
                dctInvTransType.TryGetValue(Configuration.convertNullToInt(cboTransType.Value), out nTransEffect);
                if (nTransEffect == 1)
                {
                    if (Resources.Message.Display(" You are deducting rather than adding to inventory. \n Do you wish to continue? \n", "Inventory Received", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
                    {
                        cboTransType.SelectedIndex = 0;
                        return;
                    }
                }
            }
            #endregion

            this.grdDetail.DataSource = oInvRecvDData;
            this.grdDetail.Refresh();
            ApplyGrigFormat();
        }
        public String InventoryWay
        {
            set
            {
                this.inventoryWay = value;
            }
        }

        private void frmInventoryRecieved_Resize(object sender, EventArgs e)
        {
            this.Left = clsPOSDBConstants.formLeft;
            this.Top = clsPOSDBConstants.formLeft;
        }

        private void txtReference_Leave(object sender, EventArgs e)
        {
            //By Krishna 0n 13 April 2011
            if (txtVendor.Text != "" && RecInventoryFlag == true)
            {
                grdDetail.Focus();
                FKEdit(ItemCode, "Item");
                RecInventoryFlag = false;
                if (grdDetail.Rows.Count > 0)
                {
                    bool res = grdDetail.Rows[0].Cells["Qty"].CanEnterEditMode;
                    grdDetail.Rows[0].Cells["Qty"].Activate();
                    grdDetail.PerformAction(UltraGridAction.EnterEditMode);
                    //VendoNullFlag = false;
                }
            }
            //Till here by krishna
        }

        private void frmInventoryRecieved_FormClosed(object sender, FormClosedEventArgs e)
        {
            isInventoryRecieved = false;
            frmInventoryRecieved.isFromPurchaseOrder = false;
        }

        private void txtVendor_ValueChanged(object sender, EventArgs e)
        {

        }

        //Sprint-21 - 2002 21-Jul-2015 JY Added
        private void grdDetail_AfterCellUpdate(object sender, CellEventArgs e)
        {
            try
            {
                if (e.Cell.Column.Key == "Qty" || e.Cell.Column.Key == "Cost")
                {
                    int nActiveRowIndex = e.Cell.Row.Index;
                    if (Configuration.convertNullToDecimal(grdDetail.Rows[nActiveRowIndex].Cells["Qty"].Value.ToString()) != 0 && Configuration.convertNullToDecimal(grdDetail.Rows[nActiveRowIndex].Cells["Cost"].Value.ToString()) != 0)
                        grdDetail.Rows[nActiveRowIndex].Cells["TotalCost"].Value = Configuration.convertNullToDecimal(grdDetail.Rows[nActiveRowIndex].Cells["Qty"].Value.ToString()) * Configuration.convertNullToDecimal(grdDetail.Rows[nActiveRowIndex].Cells["Cost"].Value.ToString());
                    else
                        grdDetail.Rows[nActiveRowIndex].Cells["TotalCost"].Value = 0;
                    CalcGrandTotalCost();
                }
            }
            catch (Exception exp)
            {
                this.Cursor = Cursors.Default;
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }

        //Sprint-21 - 2002 21-Jul-2015 JY Added to calculate Grand Total Cost
        private void CalcGrandTotalCost()
        {
            decimal d = 0;
            for (int i = 0; i < grdDetail.Rows.Count; i++)
            {
                d += Configuration.convertNullToDecimal(grdDetail.Rows[i].Cells["TotalCost"].Value);
            }
            txtGrandTotalCost.Text = d.ToString();
        }

        //Sprint-21 - 2207 05-Aug-2015 JY Added below function - need to refresh the VendorItemId on change on vendor because previosuly it was populated by old vendor.
        private void RefreshVendorItemId()
        {
            try
            {
                if (grdDetail.Rows.Count > 0 && txtVendor.Text.Trim() != Configuration.convertNullToString(grdDetail.Rows[0].Cells["VendorCode"].Value))
                {
                    string strItems = string.Empty;
                    for (int i = 0; i < grdDetail.Rows.Count; i++)
                    {
                        grdDetail.Rows[i].Cells[clsPOSDBConstants.Vendor_Fld_VendorCode].Value = txtVendor.Text.Trim();
                        if (strItems == string.Empty)
                            strItems = "'" + Configuration.convertNullToString(grdDetail.Rows[i].Cells[clsPOSDBConstants.ItemVendor_Fld_ItemID].Value).Replace("'", "''") + "'";
                        else
                            strItems += ",'" + Configuration.convertNullToString(grdDetail.Rows[i].Cells[clsPOSDBConstants.ItemVendor_Fld_ItemID].Value).Replace("'", "''") + "'";
                    }

                    if (strItems.Length > 0)
                    {
                        ItemVendorData oItemVendorData = new ItemVendorData();
                        ItemVendor oItemVendor = new ItemVendor();
                        oItemVendorData = oItemVendor.PopulateList(" WHERE ItemVendor." + clsPOSDBConstants.ItemVendor_Fld_ItemID + " IN (" + strItems + ") AND Vendor." + clsPOSDBConstants.Vendor_Fld_VendorCode + " = '" + txtVendor.Text.Trim() + "'");

                        if (oItemVendorData.Tables.Count > 0 && oItemVendorData.Tables[0].Rows.Count > 0)
                        {
                            for (int i = 0; i < grdDetail.Rows.Count; i++)
                            {
                                for (int j = 0; j < oItemVendorData.Tables[0].Rows.Count; j++)
                                {
                                    if (Configuration.convertNullToString(grdDetail.Rows[i].Cells[clsPOSDBConstants.ItemVendor_Fld_ItemID].Value).Trim().ToUpper() == Configuration.convertNullToString(oItemVendorData.Tables[0].Rows[j][clsPOSDBConstants.ItemVendor_Fld_ItemID]).Trim().ToUpper())
                                    {
                                        grdDetail.Rows[i].Cells[clsPOSDBConstants.ItemVendor_Fld_VendorItemID].Value = Configuration.convertNullToString(oItemVendorData.Tables[0].Rows[j][clsPOSDBConstants.ItemVendor_Fld_VendorItemID]);
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch
            { }
        }

        #region PrimePOS-2395 01-Aug-2018 JY Added
        private void btnReport_Click(object sender, EventArgs e)
        {
            try
            {
                frmRptInventoryReceived ofrmRptInventoryReceived = new frmRptInventoryReceived(InventoryReceivedEnum.InventoryReceive);
                ofrmRptInventoryReceived.ShowDialog(frmMain.getInstance());

                //frmMain.RptInventoryReceived.Show();
            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }
        #endregion

        private void frmInventoryRecieved_Shown(object sender, EventArgs e)
        {
            //this.WindowState = FormWindowState.Maximized;
        }

        #region PRIMEPOS-2419 24-May-2019 JY Added
        private void SearchDept()
        {
            try
            {
                frmSearchMain oSearch = new frmSearchMain(true);
                oSearch.SearchTable = clsPOSDBConstants.Department_tbl;
                oSearch.SearchInConstructor = true;
                oSearch.ShowDialog(this);
                if (!oSearch.IsCanceled)
                {
                    string strCode = oSearch.SelectedRowID();
                    if (strCode == "")
                        return;

                    FKEdit(strCode, clsPOSDBConstants.Department_tbl);
                }
            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }

        private void SearchSubDept()
        {
            try
            {
                frmSearchMain oSearch = new frmSearchMain(true);
                oSearch.SearchTable = clsPOSDBConstants.SubDepartment_tbl;
                oSearch.SearchInConstructor = true;
                oSearch.AdditionalParameter = Configuration.convertNullToInt(this.grdDetail.ActiveRow.Cells[clsPOSDBConstants.Department_Fld_DeptID].Value);
                oSearch.ShowDialog(this);
                if (!oSearch.IsCanceled)
                {
                    string strCode = oSearch.SelectedRowID();
                    if (strCode == "")
                        return;

                    //FKEdit(strCode, clsPOSDBConstants.SubDepartment_tbl);
                    SubDepartment oSubDepartment = new SubDepartment();
                    SubDepartmentData oSubDepartmentData = oSubDepartment.PopulateList("AND DepartmentID = " + this.grdDetail.ActiveRow.Cells[clsPOSDBConstants.Department_Fld_DeptID].Value + " And SubDepartmentID = " + strCode);
                    if (oSubDepartmentData != null && oSubDepartmentData.Tables.Count > 0 && oSubDepartmentData.Tables[0].Rows.Count > 0)
                    {
                        SubDepartmentRow oSubDepartmentRow = (SubDepartmentRow)oSubDepartmentData.Tables[0].Rows[0];
                        this.grdDetail.ActiveRow.Cells[clsPOSDBConstants.SubDepartment_Fld_SubDepartmentID].Value = Configuration.convertNullToInt(strCode);
                        this.grdDetail.ActiveRow.Cells[strSubDept].Value = oSubDepartmentRow.Description;
                    }
                    else
                    {
                        this.grdDetail.ActiveRow.Cells[clsPOSDBConstants.SubDepartment_Fld_SubDepartmentID].Value = 0;
                        this.grdDetail.ActiveRow.Cells[strSubDept].Value = "";
                    }
                }
            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }
        #endregion

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

        #region PRIMEPOS-2822 17-Mar-2020 JY Added
        private void Button_Enter(object sender, EventArgs e)
        {
            Infragistics.Win.Misc.UltraButton oButton = (Infragistics.Win.Misc.UltraButton)sender;
            string EditorName = oButton.Name;

            switch (EditorName)
            {
                case "btnAddItem":
                    btnAddItem.Appearance.BackColor = Color.LightGray;
                    break;
                case "btnDeleteItem":
                    btnDeleteItem.Appearance.BackColor = Color.LightGray;
                    break;
                case "btnFillFromPO":
                    btnFillFromPO.Appearance.BackColor = Color.LightGray;
                    break;
                case "btnNew":
                    btnNew.Appearance.BackColor = Color.LightGray;
                    break;
                case "btnReport":
                    btnReport.Appearance.BackColor = Color.LightGray;
                    break;
                case "btnSave":
                    btnSave.Appearance.BackColor = Color.LightGray;
                    break;
                default:
                    break;
            }
        }

        private void Button_Leave(object sender, EventArgs e)
        {
            Infragistics.Win.Misc.UltraButton oButton = (Infragistics.Win.Misc.UltraButton)sender;
            string EditorName = oButton.Name;

            switch (EditorName)
            {
                case "btnAddItem":
                    btnAddItem.Appearance.BackColor = Color.White;
                    break;
                case "btnDeleteItem":
                    btnDeleteItem.Appearance.BackColor = Color.White;
                    break;
                case "btnFillFromPO":
                    btnFillFromPO.Appearance.BackColor = Color.White;
                    break;
                case "btnNew":
                    btnNew.Appearance.BackColor = Color.White;
                    break;
                case "btnReport":
                    btnReport.Appearance.BackColor = Color.White;
                    break;
                case "btnSave":
                    btnSave.Appearance.BackColor = Color.White;
                    break;
                default:
                    break;
            }
        }
        #endregion

        #region PRIMEPOS-2243 27-Apr-2021 JY Added
        private void btnPrintShelfStickers_Click(object sender, EventArgs e)
        {
            frmRptItemPriceLogLable ofrmRptItemPriceLogLable = new frmRptItemPriceLogLable();
            if (grdDetail.Rows.Count <= 0)
                return;
            try
            {
                string strItemID = string.Empty;
                for (int i = 0; i < grdDetail.Rows.Count; i++)
                {
                    if (strItemID == string.Empty)
                        strItemID = "'" + Configuration.convertNullToString(grdDetail.Rows[i].Cells[clsPOSDBConstants.InvRecvDetail_Fld_ItemID].Text).Replace("'", "''") + "'";
                    else
                        strItemID += ",'" + Configuration.convertNullToString(grdDetail.Rows[i].Cells[clsPOSDBConstants.InvRecvDetail_Fld_ItemID].Text).Replace("'", "''") + "'";
                }
                ofrmRptItemPriceLogLable.ItemID = strItemID;
                ofrmRptItemPriceLogLable.VendorID = Configuration.convertNullToInt(this.txtVendor.Tag);
                ofrmRptItemPriceLogLable.Show();
            }
            catch (Exception Ex)
            {
            }
        }
        #endregion
    }
}
