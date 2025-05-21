using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using POS_Core.BusinessRules;
using POS_Core.CommonData;
using System.Data;
//using POS_Core.DataAccess;
using POS_Core_UI.Reports.Reports;
using CrystalDecisions.CrystalReports;
using CrystalDecisions.CrystalReports.Engine;
using POS.Reports.ReportXSD;
//using POS_Core_UI.Reports.ReportsUI;
using Infragistics.Win.UltraWinGrid;
using System.Collections.Generic;
using POS_Core.DataAccess;
using POS_Core.Resources;
using POS_Core_UI.Reports.ReportsUI;
using POS_Core.LabelHandler;
using POS_Core.LabelHandler.RxLabel;
using POS_Core.ErrorLogging;
using NLog;

namespace POS_Core_UI
{
    /// <summary>
    /// Summary description for frmStationClose.
    /// </summary>
    public class frmStationClose : System.Windows.Forms.Form, ICommandLIneTaskControl
    {
        private CloseStationData oCloseStationData;

        private StationClose oStationClose = new StationClose();
        private Infragistics.Win.Misc.UltraLabel lbl;
        private Infragistics.Win.Misc.UltraLabel ultraLabel1;
        private Infragistics.Win.Misc.UltraLabel ultraLabel2;
        private Infragistics.Win.Misc.UltraLabel ultraLabel3;
        private Infragistics.Win.Misc.UltraLabel ultraLabel4;
        private Infragistics.Win.Misc.UltraLabel ultraLabel5;
        private Infragistics.Win.Misc.UltraLabel ultraLabel6;
        private Infragistics.Win.Misc.UltraLabel ultraLabel7;
        private Infragistics.Win.Misc.UltraLabel ultraLabel8;
        private Infragistics.Win.Misc.UltraLabel ultraLabel14;
        private Infragistics.Win.Misc.UltraLabel ultraLabel15;
        private Infragistics.Win.Misc.UltraLabel ultraLabel16;
        private Infragistics.Win.Misc.UltraLabel ultraLabel17;
        private Infragistics.Win.Misc.UltraLabel ultraLabel10;
        private Infragistics.Win.Misc.UltraButton cmdProcess;
        private Infragistics.Win.Misc.UltraButton cmdClose;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor txtStationId;
        private Infragistics.Win.UltraWinEditors.UltraNumericEditor numTotalSale;
        private Infragistics.Win.UltraWinEditors.UltraNumericEditor numTotalReturn;
        private Infragistics.Win.UltraWinEditors.UltraNumericEditor numTotalDiscount;
        private Infragistics.Win.UltraWinEditors.UltraNumericEditor numNetSale;
        private Infragistics.Win.UltraWinEditors.UltraNumericEditor numSalesTax;
        private Infragistics.Win.UltraWinEditors.UltraNumericEditor numTotalCash;
        private Infragistics.Win.UltraWinEditors.UltraNumericEditor numReceiveOnAccount;
        private Infragistics.Win.UltraWinEditors.UltraNumericEditor numGrandTotal;
        private Infragistics.Win.UltraWinEditors.UltraNumericEditor numTotal;
        private Infragistics.Win.UltraWinEditors.UltraNumericEditor numTotalCashU;
        private Infragistics.Win.UltraWinEditors.UltraNumericEditor numPayOut;
        private Infragistics.Win.UltraWinEditors.UltraNumericEditor numNetCash;
        private System.Windows.Forms.ColumnHeader colPayType;
        private System.Windows.Forms.ColumnHeader colAmount;
        private System.Windows.Forms.ListView lvPayType;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor txtStationCloseNo;
        private Infragistics.Win.Misc.UltraLabel lblTransactionType;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.GroupBox groupBox4;
        private Infragistics.Win.Misc.UltraLabel ultraLabel9;
        private Infragistics.Win.UltraWinEditors.UltraComboEditor cboDrawNo;
        private Infragistics.Win.Misc.UltraButton btnViewCurrentStatus;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor txtStationName;
        private Infragistics.Win.Misc.UltraLabel lblUserEnterCash;
        private Infragistics.Win.UltraWinEditors.UltraNumericEditor numUserEnterAmount;
        private Infragistics.Win.Misc.UltraLabel lblCashDifference;
        private Infragistics.Win.Misc.UltraButton btnVerify;
        private Infragistics.Win.Misc.UltraLabel lblVerifyBy;
        private Infragistics.Win.UltraWinEditors.UltraNumericEditor numCashDifference;
        private Infragistics.Win.Misc.UltraLabel lblVerifyDate;
        private Infragistics.Win.UltraWinEditors.UltraNumericEditor numVerifyAmount;
        private Infragistics.Win.Misc.UltraLabel lblVerifyAmt;
        public Infragistics.Win.UltraWinEditors.UltraTextEditor txtVerifyDate;
        public Infragistics.Win.UltraWinEditors.UltraTextEditor txtVerifyBy;
        private Infragistics.Win.UltraWinEditors.UltraNumericEditor numStartingBalance;
        private Infragistics.Win.Misc.UltraLabel lblStartingBalance;
        private Infragistics.Win.UltraWinEditors.UltraNumericEditor numVerifyCashDiff;
        private Infragistics.Win.Misc.UltraLabel lblVerifyCashDiff;
        private Infragistics.Win.UltraWinTabControl.UltraTabControl ultraTabControl1;
        private Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage ultraTabSharedControlsPage1;
        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl1;
        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl2;
        private Infragistics.Win.UltraWinGrid.UltraGrid grdDepartments;
        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl3;
        private UltraGrid grdTransDetail;
        private Infragistics.Win.Misc.UltraButton btnPrintTransactionDetails;
        private Infragistics.Win.Misc.UltraLabel lblCloseDate;
        public Infragistics.Win.UltraWinEditors.UltraTextEditor txtCloseDate;

        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;
        private Panel pnlTop;
        private Infragistics.Win.Misc.UltraLabel lblTransRange;
        private bool bEditMode = false;
        string strCompanyLogoPath = string.Empty;   //PRIMEPOS-2386 26-Feb-2021 JY Added
        private Infragistics.Win.Misc.UltraLabel lblTransFee;
        private Infragistics.Win.UltraWinEditors.UltraNumericEditor numTransFee;
        private ILogger logger = LogManager.GetCurrentClassLogger();

        public frmStationClose()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();
            this.txtStationId.Text = Configuration.StationID;
            this.txtStationName.Text = Configuration.StationName;
            this.customControl = new usrDateRangeParams();  //PRIMEPOS-3042 22-Dec-2021 JY Added
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
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
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
            Infragistics.Win.Appearance appearance25 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance26 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance27 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance28 = new Infragistics.Win.Appearance();
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
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand1 = new Infragistics.Win.UltraWinGrid.UltraGridBand("Band 0", -1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn4 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("ColumnName");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn5 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Type");
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
            Infragistics.Win.Appearance appearance57 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance58 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance59 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance60 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance61 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance62 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance63 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinScrollBar.ScrollBarLook scrollBarLook1 = new Infragistics.Win.UltraWinScrollBar.ScrollBarLook();
            Infragistics.Win.Appearance appearance64 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance65 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand2 = new Infragistics.Win.UltraWinGrid.UltraGridBand("Band 0", -1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn1 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Column Name");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn2 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Type");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn3 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Criteria Value");
            Infragistics.Win.Appearance appearance66 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance67 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance68 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance69 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance70 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance71 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance72 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance73 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance74 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance75 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance76 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance77 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance78 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance79 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance80 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance81 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance82 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance83 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance84 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinScrollBar.ScrollBarLook scrollBarLook2 = new Infragistics.Win.UltraWinScrollBar.ScrollBarLook();
            Infragistics.Win.Appearance appearance85 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance86 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance87 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance88 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance89 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance90 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance91 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance92 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance93 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance94 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance95 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance96 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance97 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance98 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance99 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance100 = new Infragistics.Win.Appearance();
            Infragistics.Win.ValueListItem valueListItem1 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem2 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem3 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.Appearance appearance101 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance102 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance103 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance104 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance105 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab1 = new Infragistics.Win.UltraWinTabControl.UltraTab();
            Infragistics.Win.Appearance appearance106 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab2 = new Infragistics.Win.UltraWinTabControl.UltraTab();
            Infragistics.Win.Appearance appearance107 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab3 = new Infragistics.Win.UltraWinTabControl.UltraTab();
            Infragistics.Win.Appearance appearance108 = new Infragistics.Win.Appearance();
            this.ultraTabPageControl1 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.lblTransFee = new Infragistics.Win.Misc.UltraLabel();
            this.numTransFee = new Infragistics.Win.UltraWinEditors.UltraNumericEditor();
            this.ultraLabel8 = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabel7 = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabel6 = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabel5 = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabel4 = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabel3 = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabel2 = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabel1 = new Infragistics.Win.Misc.UltraLabel();
            this.numGrandTotal = new Infragistics.Win.UltraWinEditors.UltraNumericEditor();
            this.numReceiveOnAccount = new Infragistics.Win.UltraWinEditors.UltraNumericEditor();
            this.numTotalCash = new Infragistics.Win.UltraWinEditors.UltraNumericEditor();
            this.numSalesTax = new Infragistics.Win.UltraWinEditors.UltraNumericEditor();
            this.numNetSale = new Infragistics.Win.UltraWinEditors.UltraNumericEditor();
            this.numTotalDiscount = new Infragistics.Win.UltraWinEditors.UltraNumericEditor();
            this.numTotalReturn = new Infragistics.Win.UltraWinEditors.UltraNumericEditor();
            this.numTotalSale = new Infragistics.Win.UltraWinEditors.UltraNumericEditor();
            this.lvPayType = new System.Windows.Forms.ListView();
            this.colPayType = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colAmount = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.numVerifyCashDiff = new Infragistics.Win.UltraWinEditors.UltraNumericEditor();
            this.lblVerifyCashDiff = new Infragistics.Win.Misc.UltraLabel();
            this.numStartingBalance = new Infragistics.Win.UltraWinEditors.UltraNumericEditor();
            this.lblStartingBalance = new Infragistics.Win.Misc.UltraLabel();
            this.txtVerifyBy = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.txtVerifyDate = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.lblVerifyDate = new Infragistics.Win.Misc.UltraLabel();
            this.numVerifyAmount = new Infragistics.Win.UltraWinEditors.UltraNumericEditor();
            this.lblVerifyAmt = new Infragistics.Win.Misc.UltraLabel();
            this.lblVerifyBy = new Infragistics.Win.Misc.UltraLabel();
            this.numCashDifference = new Infragistics.Win.UltraWinEditors.UltraNumericEditor();
            this.lblCashDifference = new Infragistics.Win.Misc.UltraLabel();
            this.lblUserEnterCash = new Infragistics.Win.Misc.UltraLabel();
            this.numUserEnterAmount = new Infragistics.Win.UltraWinEditors.UltraNumericEditor();
            this.ultraLabel17 = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabel16 = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabel15 = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabel14 = new Infragistics.Win.Misc.UltraLabel();
            this.numPayOut = new Infragistics.Win.UltraWinEditors.UltraNumericEditor();
            this.numNetCash = new Infragistics.Win.UltraWinEditors.UltraNumericEditor();
            this.numTotal = new Infragistics.Win.UltraWinEditors.UltraNumericEditor();
            this.numTotalCashU = new Infragistics.Win.UltraWinEditors.UltraNumericEditor();
            this.ultraTabPageControl2 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.grdDepartments = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.ultraTabPageControl3 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.grdTransDetail = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.pnlTop = new System.Windows.Forms.Panel();
            this.lblTransRange = new Infragistics.Win.Misc.UltraLabel();
            this.lbl = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabel10 = new Infragistics.Win.Misc.UltraLabel();
            this.cmdProcess = new Infragistics.Win.Misc.UltraButton();
            this.cmdClose = new Infragistics.Win.Misc.UltraButton();
            this.txtStationId = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.txtStationCloseNo = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.lblTransactionType = new Infragistics.Win.Misc.UltraLabel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtCloseDate = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.lblCloseDate = new Infragistics.Win.Misc.UltraLabel();
            this.txtStationName = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.cboDrawNo = new Infragistics.Win.UltraWinEditors.UltraComboEditor();
            this.ultraLabel9 = new Infragistics.Win.Misc.UltraLabel();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.btnPrintTransactionDetails = new Infragistics.Win.Misc.UltraButton();
            this.btnVerify = new Infragistics.Win.Misc.UltraButton();
            this.btnViewCurrentStatus = new Infragistics.Win.Misc.UltraButton();
            this.ultraTabControl1 = new Infragistics.Win.UltraWinTabControl.UltraTabControl();
            this.ultraTabSharedControlsPage1 = new Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage();
            this.ultraTabPageControl1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numTransFee)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numGrandTotal)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numReceiveOnAccount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numTotalCash)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numSalesTax)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numNetSale)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numTotalDiscount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numTotalReturn)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numTotalSale)).BeginInit();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numVerifyCashDiff)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numStartingBalance)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtVerifyBy)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtVerifyDate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numVerifyAmount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numCashDifference)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numUserEnterAmount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numPayOut)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numNetCash)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numTotal)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numTotalCashU)).BeginInit();
            this.ultraTabPageControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdDepartments)).BeginInit();
            this.ultraTabPageControl3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdTransDetail)).BeginInit();
            this.pnlTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtStationId)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtStationCloseNo)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtCloseDate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtStationName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboDrawNo)).BeginInit();
            this.groupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraTabControl1)).BeginInit();
            this.ultraTabControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // ultraTabPageControl1
            // 
            this.ultraTabPageControl1.Controls.Add(this.groupBox2);
            this.ultraTabPageControl1.Controls.Add(this.lvPayType);
            this.ultraTabPageControl1.Controls.Add(this.groupBox3);
            this.ultraTabPageControl1.Location = new System.Drawing.Point(2, 23);
            this.ultraTabPageControl1.Name = "ultraTabPageControl1";
            this.ultraTabPageControl1.Size = new System.Drawing.Size(754, 383);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.lblTransFee);
            this.groupBox2.Controls.Add(this.numTransFee);
            this.groupBox2.Controls.Add(this.ultraLabel8);
            this.groupBox2.Controls.Add(this.ultraLabel7);
            this.groupBox2.Controls.Add(this.ultraLabel6);
            this.groupBox2.Controls.Add(this.ultraLabel5);
            this.groupBox2.Controls.Add(this.ultraLabel4);
            this.groupBox2.Controls.Add(this.ultraLabel3);
            this.groupBox2.Controls.Add(this.ultraLabel2);
            this.groupBox2.Controls.Add(this.ultraLabel1);
            this.groupBox2.Controls.Add(this.numGrandTotal);
            this.groupBox2.Controls.Add(this.numReceiveOnAccount);
            this.groupBox2.Controls.Add(this.numTotalCash);
            this.groupBox2.Controls.Add(this.numSalesTax);
            this.groupBox2.Controls.Add(this.numNetSale);
            this.groupBox2.Controls.Add(this.numTotalDiscount);
            this.groupBox2.Controls.Add(this.numTotalReturn);
            this.groupBox2.Controls.Add(this.numTotalSale);
            this.groupBox2.Location = new System.Drawing.Point(10, 5);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(284, 248);
            this.groupBox2.TabIndex = 42;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Closing Details";
            // 
            // lblTransFee
            // 
            appearance1.ForeColor = System.Drawing.Color.White;
            appearance1.TextVAlignAsString = "Middle";
            this.lblTransFee.Appearance = appearance1;
            this.lblTransFee.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTransFee.Location = new System.Drawing.Point(10, 196);
            this.lblTransFee.Name = "lblTransFee";
            this.lblTransFee.Size = new System.Drawing.Size(138, 18);
            this.lblTransFee.TabIndex = 36;
            this.lblTransFee.Text = "Transaction Fee";
            // 
            // numTransFee
            // 
            appearance2.BackColor = System.Drawing.Color.White;
            appearance2.FontData.BoldAsString = "True";
            appearance2.FontData.Name = "Arial";
            appearance2.ForeColor = System.Drawing.Color.Black;
            this.numTransFee.Appearance = appearance2;
            this.numTransFee.BackColor = System.Drawing.Color.White;
            this.numTransFee.BorderStyle = Infragistics.Win.UIElementBorderStyle.None;
            this.numTransFee.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(10)));
            this.numTransFee.Location = new System.Drawing.Point(160, 195);
            this.numTransFee.MaskInput = "{LOC}-nnnnnnnnnn.nn";
            this.numTransFee.Name = "numTransFee";
            this.numTransFee.NumericType = Infragistics.Win.UltraWinEditors.NumericType.Double;
            this.numTransFee.PromptChar = ' ';
            this.numTransFee.ReadOnly = true;
            this.numTransFee.Size = new System.Drawing.Size(114, 20);
            this.numTransFee.TabIndex = 37;
            this.numTransFee.TabStop = false;
            // 
            // ultraLabel8
            // 
            appearance3.ForeColor = System.Drawing.Color.White;
            appearance3.TextVAlignAsString = "Middle";
            this.ultraLabel8.Appearance = appearance3;
            this.ultraLabel8.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultraLabel8.Location = new System.Drawing.Point(10, 221);
            this.ultraLabel8.Name = "ultraLabel8";
            this.ultraLabel8.Size = new System.Drawing.Size(138, 18);
            this.ultraLabel8.TabIndex = 8;
            this.ultraLabel8.Text = "Grand Total";
            // 
            // ultraLabel7
            // 
            appearance4.ForeColor = System.Drawing.Color.White;
            appearance4.TextVAlignAsString = "Middle";
            this.ultraLabel7.Appearance = appearance4;
            this.ultraLabel7.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultraLabel7.Location = new System.Drawing.Point(10, 171);
            this.ultraLabel7.Name = "ultraLabel7";
            this.ultraLabel7.Size = new System.Drawing.Size(138, 18);
            this.ultraLabel7.TabIndex = 7;
            this.ultraLabel7.Text = "Receive On Account";
            // 
            // ultraLabel6
            // 
            appearance5.ForeColor = System.Drawing.Color.White;
            appearance5.TextVAlignAsString = "Middle";
            this.ultraLabel6.Appearance = appearance5;
            this.ultraLabel6.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultraLabel6.Location = new System.Drawing.Point(10, 146);
            this.ultraLabel6.Name = "ultraLabel6";
            this.ultraLabel6.Size = new System.Drawing.Size(138, 18);
            this.ultraLabel6.TabIndex = 6;
            this.ultraLabel6.Text = "Total";
            // 
            // ultraLabel5
            // 
            appearance6.ForeColor = System.Drawing.Color.White;
            appearance6.TextVAlignAsString = "Middle";
            this.ultraLabel5.Appearance = appearance6;
            this.ultraLabel5.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultraLabel5.Location = new System.Drawing.Point(10, 121);
            this.ultraLabel5.Name = "ultraLabel5";
            this.ultraLabel5.Size = new System.Drawing.Size(138, 18);
            this.ultraLabel5.TabIndex = 5;
            this.ultraLabel5.Text = "Sales Tax";
            // 
            // ultraLabel4
            // 
            appearance7.ForeColor = System.Drawing.Color.White;
            appearance7.TextVAlignAsString = "Middle";
            this.ultraLabel4.Appearance = appearance7;
            this.ultraLabel4.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultraLabel4.Location = new System.Drawing.Point(10, 96);
            this.ultraLabel4.Name = "ultraLabel4";
            this.ultraLabel4.Size = new System.Drawing.Size(138, 18);
            this.ultraLabel4.TabIndex = 4;
            this.ultraLabel4.Text = "Net Sale";
            // 
            // ultraLabel3
            // 
            appearance8.ForeColor = System.Drawing.Color.White;
            appearance8.TextVAlignAsString = "Middle";
            this.ultraLabel3.Appearance = appearance8;
            this.ultraLabel3.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultraLabel3.Location = new System.Drawing.Point(10, 71);
            this.ultraLabel3.Name = "ultraLabel3";
            this.ultraLabel3.Size = new System.Drawing.Size(138, 18);
            this.ultraLabel3.TabIndex = 3;
            this.ultraLabel3.Text = "Total Discount";
            // 
            // ultraLabel2
            // 
            appearance9.ForeColor = System.Drawing.Color.White;
            appearance9.TextVAlignAsString = "Middle";
            this.ultraLabel2.Appearance = appearance9;
            this.ultraLabel2.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultraLabel2.Location = new System.Drawing.Point(10, 46);
            this.ultraLabel2.Name = "ultraLabel2";
            this.ultraLabel2.Size = new System.Drawing.Size(138, 18);
            this.ultraLabel2.TabIndex = 2;
            this.ultraLabel2.Text = "Total Return";
            // 
            // ultraLabel1
            // 
            appearance10.ForeColor = System.Drawing.Color.White;
            appearance10.TextVAlignAsString = "Middle";
            this.ultraLabel1.Appearance = appearance10;
            this.ultraLabel1.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultraLabel1.Location = new System.Drawing.Point(10, 21);
            this.ultraLabel1.Name = "ultraLabel1";
            this.ultraLabel1.Size = new System.Drawing.Size(138, 18);
            this.ultraLabel1.TabIndex = 1;
            this.ultraLabel1.Text = "Total Sale";
            // 
            // numGrandTotal
            // 
            appearance11.BackColor = System.Drawing.Color.RoyalBlue;
            appearance11.FontData.SizeInPoints = 11F;
            appearance11.ForeColor = System.Drawing.Color.White;
            this.numGrandTotal.Appearance = appearance11;
            this.numGrandTotal.AutoSize = false;
            this.numGrandTotal.BackColor = System.Drawing.Color.RoyalBlue;
            this.numGrandTotal.BorderStyle = Infragistics.Win.UIElementBorderStyle.None;
            this.numGrandTotal.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(10)));
            this.numGrandTotal.Location = new System.Drawing.Point(160, 220);
            this.numGrandTotal.MaskInput = "{LOC}-nnnnnnnnnn.nn";
            this.numGrandTotal.Name = "numGrandTotal";
            this.numGrandTotal.NumericType = Infragistics.Win.UltraWinEditors.NumericType.Double;
            this.numGrandTotal.PromptChar = ' ';
            this.numGrandTotal.ReadOnly = true;
            this.numGrandTotal.Size = new System.Drawing.Size(114, 20);
            this.numGrandTotal.TabIndex = 35;
            this.numGrandTotal.TabStop = false;
            // 
            // numReceiveOnAccount
            // 
            appearance12.BackColor = System.Drawing.Color.White;
            appearance12.FontData.BoldAsString = "True";
            appearance12.FontData.Name = "Arial";
            appearance12.ForeColor = System.Drawing.Color.Black;
            this.numReceiveOnAccount.Appearance = appearance12;
            this.numReceiveOnAccount.BackColor = System.Drawing.Color.White;
            this.numReceiveOnAccount.BorderStyle = Infragistics.Win.UIElementBorderStyle.None;
            this.numReceiveOnAccount.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(10)));
            this.numReceiveOnAccount.Location = new System.Drawing.Point(160, 170);
            this.numReceiveOnAccount.MaskInput = "{LOC}-nnnnnnnnnn.nn";
            this.numReceiveOnAccount.Name = "numReceiveOnAccount";
            this.numReceiveOnAccount.NumericType = Infragistics.Win.UltraWinEditors.NumericType.Double;
            this.numReceiveOnAccount.PromptChar = ' ';
            this.numReceiveOnAccount.ReadOnly = true;
            this.numReceiveOnAccount.Size = new System.Drawing.Size(114, 20);
            this.numReceiveOnAccount.TabIndex = 34;
            this.numReceiveOnAccount.TabStop = false;
            // 
            // numTotalCash
            // 
            appearance13.BackColor = System.Drawing.Color.RoyalBlue;
            appearance13.FontData.SizeInPoints = 11F;
            appearance13.ForeColor = System.Drawing.Color.White;
            this.numTotalCash.Appearance = appearance13;
            this.numTotalCash.AutoSize = false;
            this.numTotalCash.BackColor = System.Drawing.Color.RoyalBlue;
            this.numTotalCash.BorderStyle = Infragistics.Win.UIElementBorderStyle.None;
            this.numTotalCash.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(10)));
            this.numTotalCash.Location = new System.Drawing.Point(160, 145);
            this.numTotalCash.MaskInput = "{LOC}-nnnnnnnnnn.nn";
            this.numTotalCash.Name = "numTotalCash";
            this.numTotalCash.NumericType = Infragistics.Win.UltraWinEditors.NumericType.Double;
            this.numTotalCash.PromptChar = ' ';
            this.numTotalCash.ReadOnly = true;
            this.numTotalCash.Size = new System.Drawing.Size(114, 20);
            this.numTotalCash.TabIndex = 33;
            this.numTotalCash.TabStop = false;
            // 
            // numSalesTax
            // 
            appearance14.BackColor = System.Drawing.Color.White;
            appearance14.FontData.BoldAsString = "True";
            appearance14.FontData.Name = "Arial";
            appearance14.ForeColor = System.Drawing.Color.Black;
            this.numSalesTax.Appearance = appearance14;
            this.numSalesTax.BackColor = System.Drawing.Color.White;
            this.numSalesTax.BorderStyle = Infragistics.Win.UIElementBorderStyle.None;
            this.numSalesTax.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(10)));
            this.numSalesTax.Location = new System.Drawing.Point(160, 120);
            this.numSalesTax.MaskInput = "{LOC}-nnnnnnnnnn.nn";
            this.numSalesTax.Name = "numSalesTax";
            this.numSalesTax.NumericType = Infragistics.Win.UltraWinEditors.NumericType.Double;
            this.numSalesTax.PromptChar = ' ';
            this.numSalesTax.ReadOnly = true;
            this.numSalesTax.Size = new System.Drawing.Size(114, 20);
            this.numSalesTax.TabIndex = 32;
            this.numSalesTax.TabStop = false;
            // 
            // numNetSale
            // 
            appearance15.BackColor = System.Drawing.Color.RoyalBlue;
            appearance15.FontData.SizeInPoints = 11F;
            appearance15.ForeColor = System.Drawing.Color.White;
            this.numNetSale.Appearance = appearance15;
            this.numNetSale.AutoSize = false;
            this.numNetSale.BackColor = System.Drawing.Color.RoyalBlue;
            this.numNetSale.BorderStyle = Infragistics.Win.UIElementBorderStyle.None;
            this.numNetSale.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(10)));
            this.numNetSale.Location = new System.Drawing.Point(160, 95);
            this.numNetSale.MaskInput = "{LOC}-nnnnnnnnnn.nn";
            this.numNetSale.Name = "numNetSale";
            this.numNetSale.NumericType = Infragistics.Win.UltraWinEditors.NumericType.Double;
            this.numNetSale.PromptChar = ' ';
            this.numNetSale.ReadOnly = true;
            this.numNetSale.Size = new System.Drawing.Size(114, 20);
            this.numNetSale.TabIndex = 31;
            this.numNetSale.TabStop = false;
            // 
            // numTotalDiscount
            // 
            appearance16.BackColor = System.Drawing.Color.LightYellow;
            appearance16.FontData.BoldAsString = "True";
            appearance16.FontData.Name = "Arial";
            appearance16.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.numTotalDiscount.Appearance = appearance16;
            this.numTotalDiscount.BackColor = System.Drawing.Color.LightYellow;
            this.numTotalDiscount.BorderStyle = Infragistics.Win.UIElementBorderStyle.None;
            appearance17.BackColor = System.Drawing.Color.LightGoldenrodYellow;
            appearance17.ForeColor = System.Drawing.Color.Blue;
            this.numTotalDiscount.ButtonAppearance = appearance17;
            this.numTotalDiscount.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(10)));
            this.numTotalDiscount.Location = new System.Drawing.Point(160, 70);
            this.numTotalDiscount.MaskInput = "{LOC}-nnnnnnnnnn.nn";
            this.numTotalDiscount.Name = "numTotalDiscount";
            this.numTotalDiscount.NullText = "0";
            this.numTotalDiscount.NumericType = Infragistics.Win.UltraWinEditors.NumericType.Double;
            this.numTotalDiscount.PromptChar = ' ';
            this.numTotalDiscount.ReadOnly = true;
            this.numTotalDiscount.Size = new System.Drawing.Size(114, 20);
            this.numTotalDiscount.TabIndex = 30;
            this.numTotalDiscount.TabStop = false;
            // 
            // numTotalReturn
            // 
            appearance18.BackColor = System.Drawing.Color.LightYellow;
            appearance18.FontData.BoldAsString = "True";
            appearance18.FontData.Name = "Arial";
            appearance18.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.numTotalReturn.Appearance = appearance18;
            this.numTotalReturn.BackColor = System.Drawing.Color.LightYellow;
            this.numTotalReturn.BorderStyle = Infragistics.Win.UIElementBorderStyle.None;
            appearance19.BackColor = System.Drawing.Color.LightGoldenrodYellow;
            appearance19.ForeColor = System.Drawing.Color.Blue;
            this.numTotalReturn.ButtonAppearance = appearance19;
            this.numTotalReturn.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(10)));
            this.numTotalReturn.Location = new System.Drawing.Point(160, 45);
            this.numTotalReturn.MaskInput = "{LOC}-nnnnnnnnnn.nn";
            this.numTotalReturn.Name = "numTotalReturn";
            this.numTotalReturn.NumericType = Infragistics.Win.UltraWinEditors.NumericType.Double;
            this.numTotalReturn.PromptChar = ' ';
            this.numTotalReturn.ReadOnly = true;
            this.numTotalReturn.Size = new System.Drawing.Size(114, 20);
            this.numTotalReturn.TabIndex = 29;
            this.numTotalReturn.TabStop = false;
            // 
            // numTotalSale
            // 
            appearance20.BackColor = System.Drawing.Color.LightYellow;
            appearance20.FontData.BoldAsString = "True";
            appearance20.FontData.Name = "Arial";
            appearance20.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.numTotalSale.Appearance = appearance20;
            this.numTotalSale.BackColor = System.Drawing.Color.LightYellow;
            this.numTotalSale.BorderStyle = Infragistics.Win.UIElementBorderStyle.None;
            appearance21.BackColor = System.Drawing.Color.LightGoldenrodYellow;
            appearance21.ForeColor = System.Drawing.Color.Blue;
            this.numTotalSale.ButtonAppearance = appearance21;
            this.numTotalSale.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(10)));
            this.numTotalSale.Location = new System.Drawing.Point(160, 20);
            this.numTotalSale.MaskInput = "{LOC}-nnnnnnnnnn.nn";
            this.numTotalSale.Name = "numTotalSale";
            this.numTotalSale.NumericType = Infragistics.Win.UltraWinEditors.NumericType.Double;
            this.numTotalSale.PromptChar = ' ';
            this.numTotalSale.ReadOnly = true;
            this.numTotalSale.Size = new System.Drawing.Size(114, 20);
            this.numTotalSale.TabIndex = 28;
            this.numTotalSale.TabStop = false;
            // 
            // lvPayType
            // 
            this.lvPayType.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lvPayType.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colPayType,
            this.colAmount});
            this.lvPayType.FullRowSelect = true;
            this.lvPayType.GridLines = true;
            this.lvPayType.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.lvPayType.HideSelection = false;
            this.lvPayType.Location = new System.Drawing.Point(300, 12);
            this.lvPayType.MultiSelect = false;
            this.lvPayType.Name = "lvPayType";
            this.lvPayType.Size = new System.Drawing.Size(447, 242);
            this.lvPayType.TabIndex = 18;
            this.lvPayType.UseCompatibleStateImageBehavior = false;
            this.lvPayType.View = System.Windows.Forms.View.Details;
            // 
            // colPayType
            // 
            this.colPayType.Text = "Pay Type";
            this.colPayType.Width = 131;
            // 
            // colAmount
            // 
            this.colAmount.Text = "Amount";
            this.colAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.colAmount.Width = 103;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.numVerifyCashDiff);
            this.groupBox3.Controls.Add(this.lblVerifyCashDiff);
            this.groupBox3.Controls.Add(this.numStartingBalance);
            this.groupBox3.Controls.Add(this.lblStartingBalance);
            this.groupBox3.Controls.Add(this.txtVerifyBy);
            this.groupBox3.Controls.Add(this.txtVerifyDate);
            this.groupBox3.Controls.Add(this.lblVerifyDate);
            this.groupBox3.Controls.Add(this.numVerifyAmount);
            this.groupBox3.Controls.Add(this.lblVerifyAmt);
            this.groupBox3.Controls.Add(this.lblVerifyBy);
            this.groupBox3.Controls.Add(this.numCashDifference);
            this.groupBox3.Controls.Add(this.lblCashDifference);
            this.groupBox3.Controls.Add(this.lblUserEnterCash);
            this.groupBox3.Controls.Add(this.numUserEnterAmount);
            this.groupBox3.Controls.Add(this.ultraLabel17);
            this.groupBox3.Controls.Add(this.ultraLabel16);
            this.groupBox3.Controls.Add(this.ultraLabel15);
            this.groupBox3.Controls.Add(this.ultraLabel14);
            this.groupBox3.Controls.Add(this.numPayOut);
            this.groupBox3.Controls.Add(this.numNetCash);
            this.groupBox3.Controls.Add(this.numTotal);
            this.groupBox3.Controls.Add(this.numTotalCashU);
            this.groupBox3.Location = new System.Drawing.Point(10, 255);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(737, 124);
            this.groupBox3.TabIndex = 43;
            this.groupBox3.TabStop = false;
            // 
            // numVerifyCashDiff
            // 
            appearance22.BackColor = System.Drawing.Color.White;
            appearance22.ForeColor = System.Drawing.Color.Black;
            this.numVerifyCashDiff.Appearance = appearance22;
            this.numVerifyCashDiff.BackColor = System.Drawing.Color.White;
            this.numVerifyCashDiff.BorderStyle = Infragistics.Win.UIElementBorderStyle.None;
            this.numVerifyCashDiff.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(10)));
            this.numVerifyCashDiff.Location = new System.Drawing.Point(125, 95);
            this.numVerifyCashDiff.MaskInput = "{LOC}-nnnnnnnnnn.nn";
            this.numVerifyCashDiff.Name = "numVerifyCashDiff";
            this.numVerifyCashDiff.NumericType = Infragistics.Win.UltraWinEditors.NumericType.Double;
            this.numVerifyCashDiff.PromptChar = ' ';
            this.numVerifyCashDiff.ReadOnly = true;
            this.numVerifyCashDiff.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.numVerifyCashDiff.Size = new System.Drawing.Size(143, 20);
            this.numVerifyCashDiff.TabIndex = 54;
            this.numVerifyCashDiff.TabStop = false;
            this.numVerifyCashDiff.Visible = false;
            // 
            // lblVerifyCashDiff
            // 
            appearance23.ForeColor = System.Drawing.Color.White;
            appearance23.TextVAlignAsString = "Middle";
            this.lblVerifyCashDiff.Appearance = appearance23;
            this.lblVerifyCashDiff.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblVerifyCashDiff.Location = new System.Drawing.Point(10, 96);
            this.lblVerifyCashDiff.Name = "lblVerifyCashDiff";
            this.lblVerifyCashDiff.Size = new System.Drawing.Size(109, 18);
            this.lblVerifyCashDiff.TabIndex = 53;
            this.lblVerifyCashDiff.Text = "Cash Difference ";
            this.lblVerifyCashDiff.Visible = false;
            // 
            // numStartingBalance
            // 
            appearance24.BackColor = System.Drawing.Color.White;
            appearance24.ForeColor = System.Drawing.Color.Black;
            this.numStartingBalance.Appearance = appearance24;
            this.numStartingBalance.BackColor = System.Drawing.Color.White;
            this.numStartingBalance.BorderStyle = Infragistics.Win.UIElementBorderStyle.None;
            this.numStartingBalance.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(10)));
            this.numStartingBalance.Location = new System.Drawing.Point(397, 20);
            this.numStartingBalance.MaskInput = "{LOC}-nnnnnnnnnn.nn";
            this.numStartingBalance.Name = "numStartingBalance";
            this.numStartingBalance.NumericType = Infragistics.Win.UltraWinEditors.NumericType.Double;
            this.numStartingBalance.PromptChar = ' ';
            this.numStartingBalance.ReadOnly = true;
            this.numStartingBalance.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.numStartingBalance.Size = new System.Drawing.Size(104, 20);
            this.numStartingBalance.TabIndex = 52;
            this.numStartingBalance.TabStop = false;
            this.numStartingBalance.Visible = false;
            // 
            // lblStartingBalance
            // 
            appearance25.ForeColor = System.Drawing.Color.White;
            appearance25.TextVAlignAsString = "Middle";
            this.lblStartingBalance.Appearance = appearance25;
            this.lblStartingBalance.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStartingBalance.Location = new System.Drawing.Point(273, 21);
            this.lblStartingBalance.Name = "lblStartingBalance";
            this.lblStartingBalance.Size = new System.Drawing.Size(117, 18);
            this.lblStartingBalance.TabIndex = 51;
            this.lblStartingBalance.Text = "Starting Balance";
            this.lblStartingBalance.Visible = false;
            // 
            // txtVerifyBy
            // 
            appearance26.BorderColor = System.Drawing.Color.Lime;
            appearance26.BorderColor3DBase = System.Drawing.Color.Lime;
            this.txtVerifyBy.Appearance = appearance26;
            this.txtVerifyBy.AutoSize = false;
            this.txtVerifyBy.BorderStyle = Infragistics.Win.UIElementBorderStyle.None;
            this.txtVerifyBy.Location = new System.Drawing.Point(125, 20);
            this.txtVerifyBy.Name = "txtVerifyBy";
            this.txtVerifyBy.Size = new System.Drawing.Size(143, 20);
            this.txtVerifyBy.TabIndex = 50;
            this.txtVerifyBy.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.txtVerifyBy.Visible = false;
            // 
            // txtVerifyDate
            // 
            appearance27.BorderColor = System.Drawing.Color.Lime;
            appearance27.BorderColor3DBase = System.Drawing.Color.Lime;
            this.txtVerifyDate.Appearance = appearance27;
            this.txtVerifyDate.AutoSize = false;
            this.txtVerifyDate.BorderStyle = Infragistics.Win.UIElementBorderStyle.None;
            this.txtVerifyDate.Location = new System.Drawing.Point(125, 45);
            this.txtVerifyDate.Name = "txtVerifyDate";
            this.txtVerifyDate.ReadOnly = true;
            this.txtVerifyDate.Size = new System.Drawing.Size(143, 20);
            this.txtVerifyDate.TabIndex = 49;
            this.txtVerifyDate.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.txtVerifyDate.Visible = false;
            // 
            // lblVerifyDate
            // 
            appearance28.ForeColor = System.Drawing.Color.White;
            appearance28.TextVAlignAsString = "Middle";
            this.lblVerifyDate.Appearance = appearance28;
            this.lblVerifyDate.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblVerifyDate.Location = new System.Drawing.Point(10, 46);
            this.lblVerifyDate.Name = "lblVerifyDate";
            this.lblVerifyDate.Size = new System.Drawing.Size(114, 18);
            this.lblVerifyDate.TabIndex = 48;
            this.lblVerifyDate.Text = "Verified Date ";
            this.lblVerifyDate.Visible = false;
            // 
            // numVerifyAmount
            // 
            appearance29.BackColor = System.Drawing.Color.White;
            appearance29.ForeColor = System.Drawing.Color.Black;
            this.numVerifyAmount.Appearance = appearance29;
            this.numVerifyAmount.BackColor = System.Drawing.Color.White;
            this.numVerifyAmount.BorderStyle = Infragistics.Win.UIElementBorderStyle.None;
            this.numVerifyAmount.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(10)));
            this.numVerifyAmount.Location = new System.Drawing.Point(125, 70);
            this.numVerifyAmount.MaskInput = "{LOC}-nnnnnnnnnn.nn";
            this.numVerifyAmount.Name = "numVerifyAmount";
            this.numVerifyAmount.NumericType = Infragistics.Win.UltraWinEditors.NumericType.Double;
            this.numVerifyAmount.PromptChar = ' ';
            this.numVerifyAmount.ReadOnly = true;
            this.numVerifyAmount.Size = new System.Drawing.Size(143, 20);
            this.numVerifyAmount.TabIndex = 47;
            this.numVerifyAmount.TabStop = false;
            this.numVerifyAmount.Visible = false;
            // 
            // lblVerifyAmt
            // 
            appearance30.ForeColor = System.Drawing.Color.White;
            appearance30.TextVAlignAsString = "Middle";
            this.lblVerifyAmt.Appearance = appearance30;
            this.lblVerifyAmt.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblVerifyAmt.Location = new System.Drawing.Point(10, 71);
            this.lblVerifyAmt.Name = "lblVerifyAmt";
            this.lblVerifyAmt.Size = new System.Drawing.Size(114, 18);
            this.lblVerifyAmt.TabIndex = 46;
            this.lblVerifyAmt.Text = "Verified Amount";
            this.lblVerifyAmt.Visible = false;
            // 
            // lblVerifyBy
            // 
            appearance31.ForeColor = System.Drawing.Color.White;
            appearance31.TextVAlignAsString = "Middle";
            this.lblVerifyBy.Appearance = appearance31;
            this.lblVerifyBy.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblVerifyBy.Location = new System.Drawing.Point(10, 21);
            this.lblVerifyBy.Name = "lblVerifyBy";
            this.lblVerifyBy.Size = new System.Drawing.Size(114, 18);
            this.lblVerifyBy.TabIndex = 44;
            this.lblVerifyBy.Text = "Verified By ";
            this.lblVerifyBy.Visible = false;
            // 
            // numCashDifference
            // 
            appearance32.BackColor = System.Drawing.Color.White;
            appearance32.ForeColor = System.Drawing.Color.Black;
            this.numCashDifference.Appearance = appearance32;
            this.numCashDifference.BackColor = System.Drawing.Color.White;
            this.numCashDifference.BorderStyle = Infragistics.Win.UIElementBorderStyle.None;
            this.numCashDifference.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(10)));
            this.numCashDifference.Location = new System.Drawing.Point(397, 70);
            this.numCashDifference.MaskInput = "{LOC}-nnnnnnnnnn.nn";
            this.numCashDifference.Name = "numCashDifference";
            this.numCashDifference.NumericType = Infragistics.Win.UltraWinEditors.NumericType.Double;
            this.numCashDifference.PromptChar = ' ';
            this.numCashDifference.ReadOnly = true;
            this.numCashDifference.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.numCashDifference.Size = new System.Drawing.Size(104, 20);
            this.numCashDifference.TabIndex = 43;
            this.numCashDifference.TabStop = false;
            this.numCashDifference.Visible = false;
            // 
            // lblCashDifference
            // 
            appearance33.ForeColor = System.Drawing.Color.White;
            appearance33.TextVAlignAsString = "Middle";
            this.lblCashDifference.Appearance = appearance33;
            this.lblCashDifference.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCashDifference.Location = new System.Drawing.Point(273, 71);
            this.lblCashDifference.Name = "lblCashDifference";
            this.lblCashDifference.Size = new System.Drawing.Size(117, 18);
            this.lblCashDifference.TabIndex = 42;
            this.lblCashDifference.Text = "Cash Difference ";
            this.lblCashDifference.Visible = false;
            // 
            // lblUserEnterCash
            // 
            appearance34.ForeColor = System.Drawing.Color.White;
            appearance34.TextVAlignAsString = "Middle";
            this.lblUserEnterCash.Appearance = appearance34;
            this.lblUserEnterCash.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblUserEnterCash.Location = new System.Drawing.Point(273, 46);
            this.lblUserEnterCash.Name = "lblUserEnterCash";
            this.lblUserEnterCash.Size = new System.Drawing.Size(117, 18);
            this.lblUserEnterCash.TabIndex = 40;
            this.lblUserEnterCash.Text = "Entered Cash:";
            this.lblUserEnterCash.Visible = false;
            // 
            // numUserEnterAmount
            // 
            appearance35.BackColor = System.Drawing.Color.RoyalBlue;
            appearance35.FontData.SizeInPoints = 11F;
            appearance35.ForeColor = System.Drawing.Color.White;
            this.numUserEnterAmount.Appearance = appearance35;
            this.numUserEnterAmount.AutoSize = false;
            this.numUserEnterAmount.BackColor = System.Drawing.Color.RoyalBlue;
            this.numUserEnterAmount.BorderStyle = Infragistics.Win.UIElementBorderStyle.None;
            this.numUserEnterAmount.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(10)));
            this.numUserEnterAmount.Location = new System.Drawing.Point(397, 45);
            this.numUserEnterAmount.MaskInput = "{LOC}-nnnnnnnnnn.nn";
            this.numUserEnterAmount.Name = "numUserEnterAmount";
            this.numUserEnterAmount.NumericType = Infragistics.Win.UltraWinEditors.NumericType.Double;
            this.numUserEnterAmount.PromptChar = ' ';
            this.numUserEnterAmount.ReadOnly = true;
            this.numUserEnterAmount.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.numUserEnterAmount.Size = new System.Drawing.Size(105, 20);
            this.numUserEnterAmount.SpinButtonAlignment = Infragistics.Win.ButtonAlignment.Left;
            this.numUserEnterAmount.TabIndex = 41;
            this.numUserEnterAmount.TabStop = false;
            this.numUserEnterAmount.Visible = false;
            // 
            // ultraLabel17
            // 
            appearance36.ForeColor = System.Drawing.Color.White;
            appearance36.TextVAlignAsString = "Middle";
            this.ultraLabel17.Appearance = appearance36;
            this.ultraLabel17.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultraLabel17.Location = new System.Drawing.Point(506, 96);
            this.ultraLabel17.Name = "ultraLabel17";
            this.ultraLabel17.Size = new System.Drawing.Size(102, 18);
            this.ultraLabel17.TabIndex = 17;
            this.ultraLabel17.Text = "Net Cash";
            // 
            // ultraLabel16
            // 
            appearance37.ForeColor = System.Drawing.Color.White;
            appearance37.TextVAlignAsString = "Middle";
            this.ultraLabel16.Appearance = appearance37;
            this.ultraLabel16.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultraLabel16.Location = new System.Drawing.Point(506, 71);
            this.ultraLabel16.Name = "ultraLabel16";
            this.ultraLabel16.Size = new System.Drawing.Size(102, 18);
            this.ultraLabel16.TabIndex = 16;
            this.ultraLabel16.Text = "Payout";
            // 
            // ultraLabel15
            // 
            appearance38.ForeColor = System.Drawing.Color.White;
            appearance38.TextVAlignAsString = "Middle";
            this.ultraLabel15.Appearance = appearance38;
            this.ultraLabel15.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultraLabel15.Location = new System.Drawing.Point(506, 46);
            this.ultraLabel15.Name = "ultraLabel15";
            this.ultraLabel15.Size = new System.Drawing.Size(102, 18);
            this.ultraLabel15.TabIndex = 15;
            this.ultraLabel15.Text = "Total Cash";
            // 
            // ultraLabel14
            // 
            appearance39.ForeColor = System.Drawing.Color.White;
            appearance39.TextVAlignAsString = "Middle";
            this.ultraLabel14.Appearance = appearance39;
            this.ultraLabel14.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultraLabel14.Location = new System.Drawing.Point(506, 21);
            this.ultraLabel14.Name = "ultraLabel14";
            this.ultraLabel14.Size = new System.Drawing.Size(102, 18);
            this.ultraLabel14.TabIndex = 14;
            this.ultraLabel14.Text = "Total Payment";
            // 
            // numPayOut
            // 
            appearance40.BackColor = System.Drawing.Color.White;
            appearance40.ForeColor = System.Drawing.Color.Black;
            this.numPayOut.Appearance = appearance40;
            this.numPayOut.BackColor = System.Drawing.Color.White;
            this.numPayOut.BorderStyle = Infragistics.Win.UIElementBorderStyle.None;
            this.numPayOut.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(10)));
            this.numPayOut.Location = new System.Drawing.Point(614, 70);
            this.numPayOut.MaskInput = "{LOC}-nnnnnnnnnn.nn";
            this.numPayOut.Name = "numPayOut";
            this.numPayOut.NumericType = Infragistics.Win.UltraWinEditors.NumericType.Double;
            this.numPayOut.PromptChar = ' ';
            this.numPayOut.ReadOnly = true;
            this.numPayOut.Size = new System.Drawing.Size(114, 20);
            this.numPayOut.TabIndex = 38;
            this.numPayOut.TabStop = false;
            // 
            // numNetCash
            // 
            appearance41.BackColor = System.Drawing.Color.RoyalBlue;
            appearance41.FontData.SizeInPoints = 11F;
            appearance41.ForeColor = System.Drawing.Color.White;
            this.numNetCash.Appearance = appearance41;
            this.numNetCash.AutoSize = false;
            this.numNetCash.BackColor = System.Drawing.Color.RoyalBlue;
            this.numNetCash.BorderStyle = Infragistics.Win.UIElementBorderStyle.None;
            this.numNetCash.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(10)));
            this.numNetCash.Location = new System.Drawing.Point(614, 95);
            this.numNetCash.MaskInput = "{LOC}-nnnnnnnnnn.nn";
            this.numNetCash.Name = "numNetCash";
            this.numNetCash.NumericType = Infragistics.Win.UltraWinEditors.NumericType.Double;
            this.numNetCash.PromptChar = ' ';
            this.numNetCash.ReadOnly = true;
            this.numNetCash.Size = new System.Drawing.Size(114, 20);
            this.numNetCash.TabIndex = 39;
            this.numNetCash.TabStop = false;
            // 
            // numTotal
            // 
            appearance42.BackColor = System.Drawing.Color.RoyalBlue;
            appearance42.BorderColor = System.Drawing.Color.Navy;
            appearance42.FontData.SizeInPoints = 11F;
            appearance42.ForeColor = System.Drawing.Color.White;
            this.numTotal.Appearance = appearance42;
            this.numTotal.BackColor = System.Drawing.Color.RoyalBlue;
            this.numTotal.BorderStyle = Infragistics.Win.UIElementBorderStyle.None;
            this.numTotal.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(10)));
            this.numTotal.Location = new System.Drawing.Point(614, 20);
            this.numTotal.MaskInput = "{LOC}-nnnnnnnnnn.nn";
            this.numTotal.Name = "numTotal";
            this.numTotal.NumericType = Infragistics.Win.UltraWinEditors.NumericType.Double;
            this.numTotal.PromptChar = ' ';
            this.numTotal.ReadOnly = true;
            this.numTotal.Size = new System.Drawing.Size(114, 22);
            this.numTotal.TabIndex = 36;
            this.numTotal.TabStop = false;
            // 
            // numTotalCashU
            // 
            appearance43.BackColor = System.Drawing.Color.LightYellow;
            appearance43.BorderColor = System.Drawing.Color.Navy;
            appearance43.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.numTotalCashU.Appearance = appearance43;
            this.numTotalCashU.BackColor = System.Drawing.Color.LightYellow;
            this.numTotalCashU.BorderStyle = Infragistics.Win.UIElementBorderStyle.None;
            this.numTotalCashU.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(10)));
            this.numTotalCashU.Location = new System.Drawing.Point(614, 45);
            this.numTotalCashU.MaskInput = "{LOC}-nnnnnnnnnn.nn";
            this.numTotalCashU.Name = "numTotalCashU";
            this.numTotalCashU.NumericType = Infragistics.Win.UltraWinEditors.NumericType.Double;
            this.numTotalCashU.PromptChar = ' ';
            this.numTotalCashU.ReadOnly = true;
            this.numTotalCashU.Size = new System.Drawing.Size(114, 20);
            this.numTotalCashU.TabIndex = 37;
            this.numTotalCashU.TabStop = false;
            // 
            // ultraTabPageControl2
            // 
            this.ultraTabPageControl2.Controls.Add(this.grdDepartments);
            this.ultraTabPageControl2.Location = new System.Drawing.Point(-10000, -10000);
            this.ultraTabPageControl2.Name = "ultraTabPageControl2";
            this.ultraTabPageControl2.Size = new System.Drawing.Size(754, 383);
            // 
            // grdDepartments
            // 
            appearance44.BackColor = System.Drawing.Color.White;
            appearance44.BackColor2 = System.Drawing.Color.White;
            appearance44.BackColorDisabled = System.Drawing.Color.White;
            appearance44.BackColorDisabled2 = System.Drawing.Color.White;
            appearance44.BackGradientStyle = Infragistics.Win.GradientStyle.None;
            this.grdDepartments.DisplayLayout.Appearance = appearance44;
            ultraGridColumn4.Header.VisiblePosition = 0;
            ultraGridColumn5.Header.VisiblePosition = 1;
            ultraGridBand1.Columns.AddRange(new object[] {
            ultraGridColumn4,
            ultraGridColumn5});
            this.grdDepartments.DisplayLayout.BandsSerializer.Add(ultraGridBand1);
            this.grdDepartments.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.grdDepartments.DisplayLayout.InterBandSpacing = 10;
            appearance45.BackColor = System.Drawing.Color.White;
            appearance45.BackColor2 = System.Drawing.Color.White;
            this.grdDepartments.DisplayLayout.Override.ActiveCardCaptionAppearance = appearance45;
            appearance46.BackColor = System.Drawing.Color.White;
            appearance46.BackColor2 = System.Drawing.Color.White;
            appearance46.BorderColor = System.Drawing.Color.Gray;
            this.grdDepartments.DisplayLayout.Override.ActiveRowAppearance = appearance46;
            appearance47.BackColor = System.Drawing.Color.White;
            appearance47.BackColor2 = System.Drawing.Color.White;
            appearance47.BorderColor = System.Drawing.Color.Gray;
            this.grdDepartments.DisplayLayout.Override.AddRowAppearance = appearance47;
            this.grdDepartments.DisplayLayout.Override.AllowAddNew = Infragistics.Win.UltraWinGrid.AllowAddNew.No;
            this.grdDepartments.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.False;
            this.grdDepartments.DisplayLayout.Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.False;
            appearance48.BackColor = System.Drawing.Color.Transparent;
            this.grdDepartments.DisplayLayout.Override.CardAreaAppearance = appearance48;
            appearance49.BackColor = System.Drawing.Color.White;
            appearance49.BackColor2 = System.Drawing.Color.White;
            appearance49.BackColorDisabled = System.Drawing.Color.White;
            appearance49.BackColorDisabled2 = System.Drawing.Color.White;
            appearance49.BorderColor = System.Drawing.Color.Black;
            appearance49.BorderColor3DBase = System.Drawing.Color.Black;
            this.grdDepartments.DisplayLayout.Override.CellAppearance = appearance49;
            appearance50.BackColor = System.Drawing.Color.White;
            appearance50.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(231)))), ((int)(((byte)(230)))));
            appearance50.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance50.BorderAlpha = Infragistics.Win.Alpha.Transparent;
            appearance50.BorderColor = System.Drawing.Color.Gray;
            appearance50.BorderColor3DBase = System.Drawing.Color.Gray;
            appearance50.ImageBackgroundAlpha = Infragistics.Win.Alpha.Transparent;
            appearance50.ImageBackgroundStyle = Infragistics.Win.ImageBackgroundStyle.Centered;
            this.grdDepartments.DisplayLayout.Override.CellButtonAppearance = appearance50;
            appearance51.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            appearance51.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.grdDepartments.DisplayLayout.Override.EditCellAppearance = appearance51;
            appearance52.BorderColor = System.Drawing.Color.Gray;
            this.grdDepartments.DisplayLayout.Override.FilteredInRowAppearance = appearance52;
            appearance53.BorderColor = System.Drawing.Color.Gray;
            this.grdDepartments.DisplayLayout.Override.FilteredOutRowAppearance = appearance53;
            appearance54.BackColor = System.Drawing.Color.White;
            appearance54.BackColor2 = System.Drawing.Color.White;
            appearance54.BackColorDisabled = System.Drawing.Color.White;
            appearance54.BackColorDisabled2 = System.Drawing.Color.White;
            this.grdDepartments.DisplayLayout.Override.FixedCellAppearance = appearance54;
            appearance55.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            appearance55.BackColor2 = System.Drawing.Color.Beige;
            this.grdDepartments.DisplayLayout.Override.FixedHeaderAppearance = appearance55;
            appearance56.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(62)))), ((int)(((byte)(176)))));
            appearance56.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance56.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance56.FontData.BoldAsString = "True";
            appearance56.FontData.SizeInPoints = 10F;
            appearance56.ForeColor = System.Drawing.Color.White;
            appearance56.TextHAlignAsString = "Left";
            appearance56.ThemedElementAlpha = Infragistics.Win.Alpha.Transparent;
            this.grdDepartments.DisplayLayout.Override.HeaderAppearance = appearance56;
            this.grdDepartments.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortSingle;
            appearance57.BorderColor = System.Drawing.Color.Gray;
            this.grdDepartments.DisplayLayout.Override.RowAlternateAppearance = appearance57;
            appearance58.BackColor = System.Drawing.Color.White;
            appearance58.BackColor2 = System.Drawing.Color.White;
            appearance58.BackGradientStyle = Infragistics.Win.GradientStyle.None;
            appearance58.BackHatchStyle = Infragistics.Win.BackHatchStyle.None;
            appearance58.BorderColor = System.Drawing.Color.Gray;
            this.grdDepartments.DisplayLayout.Override.RowAppearance = appearance58;
            appearance59.BorderColor = System.Drawing.Color.Gray;
            this.grdDepartments.DisplayLayout.Override.RowPreviewAppearance = appearance59;
            appearance60.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(62)))), ((int)(((byte)(176)))));
            appearance60.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance60.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance60.BorderColor = System.Drawing.Color.Gray;
            this.grdDepartments.DisplayLayout.Override.RowSelectorAppearance = appearance60;
            this.grdDepartments.DisplayLayout.Override.RowSelectorWidth = 12;
            this.grdDepartments.DisplayLayout.Override.RowSpacingBefore = 2;
            appearance61.BackColor = System.Drawing.Color.Navy;
            appearance61.BackColorAlpha = Infragistics.Win.Alpha.Opaque;
            this.grdDepartments.DisplayLayout.Override.SelectedCellAppearance = appearance61;
            appearance62.BackColor = System.Drawing.Color.Navy;
            appearance62.BackColorDisabled = System.Drawing.Color.Navy;
            appearance62.BackGradientStyle = Infragistics.Win.GradientStyle.None;
            appearance62.BackHatchStyle = Infragistics.Win.BackHatchStyle.None;
            appearance62.BorderColor = System.Drawing.Color.Gray;
            appearance62.ForeColor = System.Drawing.Color.White;
            this.grdDepartments.DisplayLayout.Override.SelectedRowAppearance = appearance62;
            this.grdDepartments.DisplayLayout.Override.SelectTypeCell = Infragistics.Win.UltraWinGrid.SelectType.None;
            this.grdDepartments.DisplayLayout.Override.SelectTypeCol = Infragistics.Win.UltraWinGrid.SelectType.None;
            this.grdDepartments.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.Single;
            appearance63.BorderColor = System.Drawing.Color.Gray;
            this.grdDepartments.DisplayLayout.Override.TemplateAddRowAppearance = appearance63;
            this.grdDepartments.DisplayLayout.RowConnectorColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            this.grdDepartments.DisplayLayout.RowConnectorStyle = Infragistics.Win.UltraWinGrid.RowConnectorStyle.Solid;
            appearance64.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(231)))), ((int)(((byte)(231)))));
            appearance64.BackColor2 = System.Drawing.Color.White;
            appearance64.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance64.BorderAlpha = Infragistics.Win.Alpha.Transparent;
            appearance64.ThemedElementAlpha = Infragistics.Win.Alpha.Transparent;
            scrollBarLook1.ButtonAppearance = appearance64;
            this.grdDepartments.DisplayLayout.ScrollBarLook = scrollBarLook1;
            this.grdDepartments.DisplayLayout.TabNavigation = Infragistics.Win.UltraWinGrid.TabNavigation.NextControl;
            this.grdDepartments.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdDepartments.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.grdDepartments.Location = new System.Drawing.Point(0, 0);
            this.grdDepartments.Name = "grdDepartments";
            this.grdDepartments.Size = new System.Drawing.Size(754, 383);
            this.grdDepartments.TabIndex = 71;
            this.grdDepartments.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // ultraTabPageControl3
            // 
            this.ultraTabPageControl3.Controls.Add(this.grdTransDetail);
            this.ultraTabPageControl3.Controls.Add(this.pnlTop);
            this.ultraTabPageControl3.Location = new System.Drawing.Point(-10000, -10000);
            this.ultraTabPageControl3.Name = "ultraTabPageControl3";
            this.ultraTabPageControl3.Size = new System.Drawing.Size(754, 383);
            // 
            // grdTransDetail
            // 
            appearance65.BackColor = System.Drawing.Color.White;
            appearance65.BackColor2 = System.Drawing.Color.White;
            appearance65.BackColorDisabled = System.Drawing.Color.White;
            appearance65.BackColorDisabled2 = System.Drawing.Color.White;
            appearance65.BackGradientStyle = Infragistics.Win.GradientStyle.None;
            this.grdTransDetail.DisplayLayout.Appearance = appearance65;
            ultraGridColumn1.Header.VisiblePosition = 0;
            ultraGridColumn2.Header.VisiblePosition = 1;
            ultraGridColumn3.Header.VisiblePosition = 2;
            ultraGridBand2.Columns.AddRange(new object[] {
            ultraGridColumn1,
            ultraGridColumn2,
            ultraGridColumn3});
            ultraGridBand2.HeaderVisible = true;
            this.grdTransDetail.DisplayLayout.BandsSerializer.Add(ultraGridBand2);
            this.grdTransDetail.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.grdTransDetail.DisplayLayout.InterBandSpacing = 10;
            this.grdTransDetail.DisplayLayout.MaxColScrollRegions = 1;
            this.grdTransDetail.DisplayLayout.MaxRowScrollRegions = 1;
            appearance66.BackColor = System.Drawing.Color.White;
            appearance66.BackColor2 = System.Drawing.Color.White;
            this.grdTransDetail.DisplayLayout.Override.ActiveCardCaptionAppearance = appearance66;
            appearance67.BackColor = System.Drawing.Color.White;
            appearance67.BackColor2 = System.Drawing.Color.White;
            appearance67.BorderColor = System.Drawing.Color.Gray;
            this.grdTransDetail.DisplayLayout.Override.ActiveRowAppearance = appearance67;
            appearance68.BackColor = System.Drawing.Color.White;
            appearance68.BackColor2 = System.Drawing.Color.White;
            appearance68.BorderColor = System.Drawing.Color.Gray;
            this.grdTransDetail.DisplayLayout.Override.AddRowAppearance = appearance68;
            this.grdTransDetail.DisplayLayout.Override.AllowAddNew = Infragistics.Win.UltraWinGrid.AllowAddNew.No;
            this.grdTransDetail.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.False;
            this.grdTransDetail.DisplayLayout.Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.False;
            appearance69.BackColor = System.Drawing.Color.Transparent;
            this.grdTransDetail.DisplayLayout.Override.CardAreaAppearance = appearance69;
            appearance70.BackColor = System.Drawing.Color.White;
            appearance70.BackColor2 = System.Drawing.Color.White;
            appearance70.BackColorDisabled = System.Drawing.Color.White;
            appearance70.BackColorDisabled2 = System.Drawing.Color.White;
            appearance70.BorderColor = System.Drawing.Color.Black;
            appearance70.BorderColor3DBase = System.Drawing.Color.Black;
            this.grdTransDetail.DisplayLayout.Override.CellAppearance = appearance70;
            appearance71.BackColor = System.Drawing.Color.White;
            appearance71.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(231)))), ((int)(((byte)(230)))));
            appearance71.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance71.BorderAlpha = Infragistics.Win.Alpha.Transparent;
            appearance71.BorderColor = System.Drawing.Color.Gray;
            appearance71.BorderColor3DBase = System.Drawing.Color.Gray;
            appearance71.ImageBackgroundAlpha = Infragistics.Win.Alpha.Transparent;
            appearance71.ImageBackgroundStyle = Infragistics.Win.ImageBackgroundStyle.Centered;
            this.grdTransDetail.DisplayLayout.Override.CellButtonAppearance = appearance71;
            appearance72.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            appearance72.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.grdTransDetail.DisplayLayout.Override.EditCellAppearance = appearance72;
            appearance73.BorderColor = System.Drawing.Color.Gray;
            this.grdTransDetail.DisplayLayout.Override.FilteredInRowAppearance = appearance73;
            appearance74.BorderColor = System.Drawing.Color.Gray;
            this.grdTransDetail.DisplayLayout.Override.FilteredOutRowAppearance = appearance74;
            appearance75.BackColor = System.Drawing.Color.White;
            appearance75.BackColor2 = System.Drawing.Color.White;
            appearance75.BackColorDisabled = System.Drawing.Color.White;
            appearance75.BackColorDisabled2 = System.Drawing.Color.White;
            this.grdTransDetail.DisplayLayout.Override.FixedCellAppearance = appearance75;
            appearance76.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            appearance76.BackColor2 = System.Drawing.Color.Beige;
            this.grdTransDetail.DisplayLayout.Override.FixedHeaderAppearance = appearance76;
            appearance77.BackColor = System.Drawing.Color.White;
            appearance77.BackColor2 = System.Drawing.SystemColors.Control;
            appearance77.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance77.FontData.BoldAsString = "True";
            appearance77.ForeColor = System.Drawing.Color.Black;
            appearance77.TextHAlignAsString = "Left";
            appearance77.ThemedElementAlpha = Infragistics.Win.Alpha.Transparent;
            this.grdTransDetail.DisplayLayout.Override.HeaderAppearance = appearance77;
            appearance78.BorderColor = System.Drawing.Color.Gray;
            this.grdTransDetail.DisplayLayout.Override.RowAlternateAppearance = appearance78;
            appearance79.BackColor = System.Drawing.Color.White;
            appearance79.BackColor2 = System.Drawing.Color.White;
            appearance79.BackGradientStyle = Infragistics.Win.GradientStyle.None;
            appearance79.BackHatchStyle = Infragistics.Win.BackHatchStyle.None;
            appearance79.BorderColor = System.Drawing.Color.Gray;
            this.grdTransDetail.DisplayLayout.Override.RowAppearance = appearance79;
            appearance80.BorderColor = System.Drawing.Color.Gray;
            this.grdTransDetail.DisplayLayout.Override.RowPreviewAppearance = appearance80;
            appearance81.BackColor = System.Drawing.Color.White;
            appearance81.BackColor2 = System.Drawing.SystemColors.Control;
            appearance81.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance81.BorderColor = System.Drawing.Color.Gray;
            this.grdTransDetail.DisplayLayout.Override.RowSelectorAppearance = appearance81;
            this.grdTransDetail.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            this.grdTransDetail.DisplayLayout.Override.RowSelectorWidth = 12;
            this.grdTransDetail.DisplayLayout.Override.RowSpacingBefore = 2;
            appearance82.BackColor = System.Drawing.Color.Navy;
            appearance82.BackColorAlpha = Infragistics.Win.Alpha.Opaque;
            this.grdTransDetail.DisplayLayout.Override.SelectedCellAppearance = appearance82;
            appearance83.BackColor = System.Drawing.Color.Navy;
            appearance83.BackColorDisabled = System.Drawing.Color.Navy;
            appearance83.BackGradientStyle = Infragistics.Win.GradientStyle.None;
            appearance83.BackHatchStyle = Infragistics.Win.BackHatchStyle.None;
            appearance83.BorderColor = System.Drawing.Color.Gray;
            appearance83.ForeColor = System.Drawing.Color.Black;
            this.grdTransDetail.DisplayLayout.Override.SelectedRowAppearance = appearance83;
            this.grdTransDetail.DisplayLayout.Override.SelectTypeCell = Infragistics.Win.UltraWinGrid.SelectType.None;
            this.grdTransDetail.DisplayLayout.Override.SelectTypeCol = Infragistics.Win.UltraWinGrid.SelectType.None;
            this.grdTransDetail.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.Single;
            appearance84.BorderColor = System.Drawing.Color.Gray;
            this.grdTransDetail.DisplayLayout.Override.TemplateAddRowAppearance = appearance84;
            this.grdTransDetail.DisplayLayout.RowConnectorColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            this.grdTransDetail.DisplayLayout.RowConnectorStyle = Infragistics.Win.UltraWinGrid.RowConnectorStyle.Solid;
            appearance85.BackColor = System.Drawing.Color.White;
            appearance85.BackColor2 = System.Drawing.SystemColors.Control;
            appearance85.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            scrollBarLook2.Appearance = appearance85;
            appearance86.BackColor = System.Drawing.Color.White;
            appearance86.BackColor2 = System.Drawing.SystemColors.Control;
            appearance86.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance86.ThemedElementAlpha = Infragistics.Win.Alpha.Transparent;
            scrollBarLook2.ButtonAppearance = appearance86;
            appearance87.BackColor = System.Drawing.Color.White;
            appearance87.BackColor2 = System.Drawing.SystemColors.Control;
            scrollBarLook2.ThumbAppearance = appearance87;
            appearance88.BackColor = System.Drawing.Color.White;
            appearance88.BackColor2 = System.Drawing.Color.White;
            scrollBarLook2.TrackAppearance = appearance88;
            this.grdTransDetail.DisplayLayout.ScrollBarLook = scrollBarLook2;
            this.grdTransDetail.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdTransDetail.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.grdTransDetail.Location = new System.Drawing.Point(0, 25);
            this.grdTransDetail.Name = "grdTransDetail";
            this.grdTransDetail.Size = new System.Drawing.Size(754, 358);
            this.grdTransDetail.TabIndex = 1;
            this.grdTransDetail.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // pnlTop
            // 
            this.pnlTop.Controls.Add(this.lblTransRange);
            this.pnlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTop.Location = new System.Drawing.Point(0, 0);
            this.pnlTop.Name = "pnlTop";
            this.pnlTop.Size = new System.Drawing.Size(754, 25);
            this.pnlTop.TabIndex = 4;
            // 
            // lblTransRange
            // 
            appearance89.FontData.BoldAsString = "True";
            appearance89.ForeColor = System.Drawing.Color.Red;
            this.lblTransRange.Appearance = appearance89;
            this.lblTransRange.Location = new System.Drawing.Point(5, 2);
            this.lblTransRange.Name = "lblTransRange";
            this.lblTransRange.Size = new System.Drawing.Size(745, 20);
            this.lblTransRange.TabIndex = 0;
            this.lblTransRange.Tag = "NOCOLOR";
            // 
            // lbl
            // 
            appearance90.ForeColor = System.Drawing.Color.White;
            appearance90.TextVAlignAsString = "Middle";
            this.lbl.Appearance = appearance90;
            this.lbl.AutoSize = true;
            this.lbl.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl.Location = new System.Drawing.Point(5, 18);
            this.lbl.Name = "lbl";
            this.lbl.Size = new System.Drawing.Size(69, 17);
            this.lbl.TabIndex = 0;
            this.lbl.Text = "Station ID";
            // 
            // ultraLabel10
            // 
            appearance91.ForeColor = System.Drawing.Color.White;
            appearance91.TextVAlignAsString = "Middle";
            this.ultraLabel10.Appearance = appearance91;
            this.ultraLabel10.AutoSize = true;
            this.ultraLabel10.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultraLabel10.Location = new System.Drawing.Point(175, 18);
            this.ultraLabel10.Name = "ultraLabel10";
            this.ultraLabel10.Size = new System.Drawing.Size(115, 17);
            this.ultraLabel10.TabIndex = 20;
            this.ultraLabel10.Text = "Station Close No";
            // 
            // cmdProcess
            // 
            this.cmdProcess.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            appearance92.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(62)))), ((int)(((byte)(176)))));
            appearance92.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance92.ForeColor = System.Drawing.Color.White;
            this.cmdProcess.Appearance = appearance92;
            this.cmdProcess.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            this.cmdProcess.Location = new System.Drawing.Point(545, 16);
            this.cmdProcess.Name = "cmdProcess";
            this.cmdProcess.Size = new System.Drawing.Size(100, 28);
            this.cmdProcess.TabIndex = 1;
            this.cmdProcess.Text = "&Process";
            this.cmdProcess.Click += new System.EventHandler(this.cmdProcess_Click);
            // 
            // cmdClose
            // 
            this.cmdClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            appearance93.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(62)))), ((int)(((byte)(176)))));
            appearance93.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance93.ForeColor = System.Drawing.Color.White;
            this.cmdClose.Appearance = appearance93;
            this.cmdClose.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            this.cmdClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cmdClose.Location = new System.Drawing.Point(650, 16);
            this.cmdClose.Name = "cmdClose";
            this.cmdClose.Size = new System.Drawing.Size(100, 28);
            this.cmdClose.TabIndex = 2;
            this.cmdClose.Text = "&Close";
            this.cmdClose.Click += new System.EventHandler(this.cmdClose_Click);
            // 
            // txtStationId
            // 
            appearance94.BackColor = System.Drawing.Color.White;
            appearance94.ForeColor = System.Drawing.Color.Red;
            appearance94.TextVAlignAsString = "Middle";
            this.txtStationId.Appearance = appearance94;
            this.txtStationId.BackColor = System.Drawing.Color.White;
            this.txtStationId.BorderStyle = Infragistics.Win.UIElementBorderStyle.None;
            this.txtStationId.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(10)));
            this.txtStationId.Location = new System.Drawing.Point(51, 33);
            this.txtStationId.Name = "txtStationId";
            this.txtStationId.ReadOnly = true;
            this.txtStationId.Size = new System.Drawing.Size(114, 20);
            this.txtStationId.TabIndex = 26;
            this.txtStationId.TabStop = false;
            this.txtStationId.Text = "01";
            this.txtStationId.Visible = false;
            // 
            // txtStationCloseNo
            // 
            appearance95.BackColor = System.Drawing.Color.White;
            appearance95.ForeColor = System.Drawing.Color.Red;
            appearance95.TextVAlignAsString = "Middle";
            this.txtStationCloseNo.Appearance = appearance95;
            this.txtStationCloseNo.BackColor = System.Drawing.Color.White;
            this.txtStationCloseNo.BorderStyle = Infragistics.Win.UIElementBorderStyle.None;
            this.txtStationCloseNo.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(10)));
            this.txtStationCloseNo.Location = new System.Drawing.Point(295, 15);
            this.txtStationCloseNo.Name = "txtStationCloseNo";
            this.txtStationCloseNo.ReadOnly = true;
            this.txtStationCloseNo.Size = new System.Drawing.Size(70, 20);
            this.txtStationCloseNo.TabIndex = 27;
            this.txtStationCloseNo.TabStop = false;
            // 
            // lblTransactionType
            // 
            appearance96.ForeColor = System.Drawing.Color.White;
            appearance96.ForeColorDisabled = System.Drawing.Color.Navy;
            appearance96.ImageVAlign = Infragistics.Win.VAlign.Bottom;
            appearance96.TextHAlignAsString = "Center";
            this.lblTransactionType.Appearance = appearance96;
            this.lblTransactionType.BackColorInternal = System.Drawing.Color.Transparent;
            this.lblTransactionType.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Bold);
            this.lblTransactionType.Location = new System.Drawing.Point(10, 0);
            this.lblTransactionType.Name = "lblTransactionType";
            this.lblTransactionType.Size = new System.Drawing.Size(758, 32);
            this.lblTransactionType.TabIndex = 40;
            this.lblTransactionType.Tag = "Header";
            this.lblTransactionType.Text = "Close Station";
            this.lblTransactionType.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtCloseDate);
            this.groupBox1.Controls.Add(this.lblCloseDate);
            this.groupBox1.Controls.Add(this.txtStationName);
            this.groupBox1.Controls.Add(this.cboDrawNo);
            this.groupBox1.Controls.Add(this.ultraLabel9);
            this.groupBox1.Controls.Add(this.ultraLabel10);
            this.groupBox1.Controls.Add(this.txtStationCloseNo);
            this.groupBox1.Controls.Add(this.txtStationId);
            this.groupBox1.Controls.Add(this.lbl);
            this.groupBox1.Location = new System.Drawing.Point(10, 33);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(758, 45);
            this.groupBox1.TabIndex = 41;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Station Info";
            // 
            // txtCloseDate
            // 
            appearance97.FontData.BoldAsString = "True";
            appearance97.FontData.ItalicAsString = "False";
            appearance97.FontData.StrikeoutAsString = "False";
            appearance97.FontData.UnderlineAsString = "False";
            appearance97.ForeColor = System.Drawing.Color.Black;
            appearance97.ForeColorDisabled = System.Drawing.Color.Black;
            this.txtCloseDate.Appearance = appearance97;
            this.txtCloseDate.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.txtCloseDate.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCloseDate.Location = new System.Drawing.Point(587, 15);
            this.txtCloseDate.MaxLength = 20;
            this.txtCloseDate.Name = "txtCloseDate";
            this.txtCloseDate.ReadOnly = true;
            this.txtCloseDate.Size = new System.Drawing.Size(160, 22);
            this.txtCloseDate.TabIndex = 32;
            this.txtCloseDate.TabStop = false;
            // 
            // lblCloseDate
            // 
            appearance98.ForeColor = System.Drawing.Color.White;
            appearance98.TextVAlignAsString = "Middle";
            this.lblCloseDate.Appearance = appearance98;
            this.lblCloseDate.AutoSize = true;
            this.lblCloseDate.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCloseDate.Location = new System.Drawing.Point(504, 18);
            this.lblCloseDate.Name = "lblCloseDate";
            this.lblCloseDate.Size = new System.Drawing.Size(76, 17);
            this.lblCloseDate.TabIndex = 31;
            this.lblCloseDate.Text = "Close Date";
            // 
            // txtStationName
            // 
            appearance99.BackColor = System.Drawing.Color.White;
            appearance99.ForeColor = System.Drawing.Color.Red;
            appearance99.TextVAlignAsString = "Middle";
            this.txtStationName.Appearance = appearance99;
            this.txtStationName.BackColor = System.Drawing.Color.White;
            this.txtStationName.BorderStyle = Infragistics.Win.UIElementBorderStyle.None;
            this.txtStationName.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(10)));
            this.txtStationName.Location = new System.Drawing.Point(81, 15);
            this.txtStationName.Name = "txtStationName";
            this.txtStationName.ReadOnly = true;
            this.txtStationName.Size = new System.Drawing.Size(85, 20);
            this.txtStationName.TabIndex = 30;
            this.txtStationName.TabStop = false;
            // 
            // cboDrawNo
            // 
            appearance100.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(108)))), ((int)(((byte)(172)))));
            this.cboDrawNo.Appearance = appearance100;
            this.cboDrawNo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(108)))), ((int)(((byte)(172)))));
            this.cboDrawNo.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.cboDrawNo.DisplayStyle = Infragistics.Win.EmbeddableElementDisplayStyle.Office2003;
            this.cboDrawNo.DropDownStyle = Infragistics.Win.DropDownStyle.DropDownList;
            this.cboDrawNo.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            valueListItem1.DataValue = "ALL";
            valueListItem1.DisplayText = "ALL";
            valueListItem2.DataValue = "1";
            valueListItem2.DisplayText = "1";
            valueListItem3.DataValue = "2";
            valueListItem3.DisplayText = "2";
            this.cboDrawNo.Items.AddRange(new Infragistics.Win.ValueListItem[] {
            valueListItem1,
            valueListItem2,
            valueListItem3});
            this.cboDrawNo.LimitToList = true;
            this.cboDrawNo.Location = new System.Drawing.Point(446, 15);
            this.cboDrawNo.Name = "cboDrawNo";
            this.cboDrawNo.Size = new System.Drawing.Size(50, 23);
            this.cboDrawNo.TabIndex = 29;
            this.cboDrawNo.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            // 
            // ultraLabel9
            // 
            appearance101.ForeColor = System.Drawing.Color.White;
            appearance101.TextVAlignAsString = "Middle";
            this.ultraLabel9.Appearance = appearance101;
            this.ultraLabel9.AutoSize = true;
            this.ultraLabel9.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultraLabel9.Location = new System.Drawing.Point(377, 18);
            this.ultraLabel9.Name = "ultraLabel9";
            this.ultraLabel9.Size = new System.Drawing.Size(63, 17);
            this.ultraLabel9.TabIndex = 28;
            this.ultraLabel9.Text = "Drawer #";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.btnPrintTransactionDetails);
            this.groupBox4.Controls.Add(this.btnVerify);
            this.groupBox4.Controls.Add(this.btnViewCurrentStatus);
            this.groupBox4.Controls.Add(this.cmdClose);
            this.groupBox4.Controls.Add(this.cmdProcess);
            this.groupBox4.Location = new System.Drawing.Point(10, 490);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(758, 51);
            this.groupBox4.TabIndex = 44;
            this.groupBox4.TabStop = false;
            // 
            // btnPrintTransactionDetails
            // 
            this.btnPrintTransactionDetails.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            appearance102.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(62)))), ((int)(((byte)(176)))));
            appearance102.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance102.ForeColor = System.Drawing.Color.White;
            this.btnPrintTransactionDetails.Appearance = appearance102;
            this.btnPrintTransactionDetails.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            this.btnPrintTransactionDetails.Location = new System.Drawing.Point(226, 16);
            this.btnPrintTransactionDetails.Name = "btnPrintTransactionDetails";
            this.btnPrintTransactionDetails.Size = new System.Drawing.Size(190, 28);
            this.btnPrintTransactionDetails.TabIndex = 5;
            this.btnPrintTransactionDetails.Text = "&Print Transaction Details";
            this.btnPrintTransactionDetails.Visible = false;
            this.btnPrintTransactionDetails.Click += new System.EventHandler(this.btnPrintTransactionDetails_Click);
            // 
            // btnVerify
            // 
            this.btnVerify.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            appearance103.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(62)))), ((int)(((byte)(176)))));
            appearance103.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance103.ForeColor = System.Drawing.Color.White;
            this.btnVerify.Appearance = appearance103;
            this.btnVerify.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            this.btnVerify.Location = new System.Drawing.Point(440, 16);
            this.btnVerify.Name = "btnVerify";
            this.btnVerify.Size = new System.Drawing.Size(100, 28);
            this.btnVerify.TabIndex = 4;
            this.btnVerify.Text = "&Verify";
            this.btnVerify.Visible = false;
            this.btnVerify.Click += new System.EventHandler(this.btnVerify_Click);
            // 
            // btnViewCurrentStatus
            // 
            this.btnViewCurrentStatus.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            appearance104.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(62)))), ((int)(((byte)(176)))));
            appearance104.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance104.ForeColor = System.Drawing.Color.White;
            this.btnViewCurrentStatus.Appearance = appearance104;
            this.btnViewCurrentStatus.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            this.btnViewCurrentStatus.Location = new System.Drawing.Point(6, 16);
            this.btnViewCurrentStatus.Name = "btnViewCurrentStatus";
            this.btnViewCurrentStatus.Size = new System.Drawing.Size(180, 28);
            this.btnViewCurrentStatus.TabIndex = 3;
            this.btnViewCurrentStatus.Text = "&View Current Status";
            this.btnViewCurrentStatus.Click += new System.EventHandler(this.btnViewCurrentStatus_Click);
            // 
            // ultraTabControl1
            // 
            appearance105.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(108)))), ((int)(((byte)(172)))));
            appearance105.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(108)))), ((int)(((byte)(172)))));
            appearance105.BackGradientStyle = Infragistics.Win.GradientStyle.None;
            appearance105.BorderColor = System.Drawing.Color.Navy;
            appearance105.BorderColor3DBase = System.Drawing.Color.Navy;
            this.ultraTabControl1.ClientAreaAppearance = appearance105;
            this.ultraTabControl1.Controls.Add(this.ultraTabSharedControlsPage1);
            this.ultraTabControl1.Controls.Add(this.ultraTabPageControl1);
            this.ultraTabControl1.Controls.Add(this.ultraTabPageControl2);
            this.ultraTabControl1.Controls.Add(this.ultraTabPageControl3);
            this.ultraTabControl1.Location = new System.Drawing.Point(10, 80);
            this.ultraTabControl1.MinTabWidth = 80;
            this.ultraTabControl1.Name = "ultraTabControl1";
            this.ultraTabControl1.SharedControlsPage = this.ultraTabSharedControlsPage1;
            this.ultraTabControl1.Size = new System.Drawing.Size(758, 408);
            this.ultraTabControl1.Style = Infragistics.Win.UltraWinTabControl.UltraTabControlStyle.PropertyPage2003;
            this.ultraTabControl1.TabIndex = 45;
            appearance106.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(108)))), ((int)(((byte)(172)))));
            appearance106.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance106.FontData.BoldAsString = "True";
            appearance106.ForeColor = System.Drawing.Color.White;
            ultraTab1.Appearance = appearance106;
            ultraTab1.TabPage = this.ultraTabPageControl1;
            ultraTab1.Text = "Close Station";
            appearance107.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(108)))), ((int)(((byte)(172)))));
            appearance107.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance107.FontData.BoldAsString = "True";
            appearance107.ForeColor = System.Drawing.Color.White;
            ultraTab2.Appearance = appearance107;
            ultraTab2.TabPage = this.ultraTabPageControl2;
            ultraTab2.Text = "Department Summary";
            appearance108.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(108)))), ((int)(((byte)(172)))));
            appearance108.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance108.FontData.BoldAsString = "True";
            appearance108.ForeColor = System.Drawing.Color.White;
            ultraTab3.Appearance = appearance108;
            ultraTab3.TabPage = this.ultraTabPageControl3;
            ultraTab3.Text = "Transaction Details";
            this.ultraTabControl1.Tabs.AddRange(new Infragistics.Win.UltraWinTabControl.UltraTab[] {
            ultraTab1,
            ultraTab2,
            ultraTab3});
            this.ultraTabControl1.TabsPerRow = 2;
            this.ultraTabControl1.TabStop = false;
            this.ultraTabControl1.ViewStyle = Infragistics.Win.UltraWinTabControl.ViewStyle.VisualStudio2005;
            // 
            // ultraTabSharedControlsPage1
            // 
            this.ultraTabSharedControlsPage1.Location = new System.Drawing.Point(-10000, -10000);
            this.ultraTabSharedControlsPage1.Name = "ultraTabSharedControlsPage1";
            this.ultraTabSharedControlsPage1.Size = new System.Drawing.Size(754, 383);
            // 
            // frmStationClose
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(7, 17);
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(108)))), ((int)(((byte)(172)))));
            this.CancelButton = this.cmdClose;
            this.ClientSize = new System.Drawing.Size(774, 546);
            this.Controls.Add(this.ultraTabControl1);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.lblTransactionType);
            this.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "frmStationClose";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Close Station";
            this.Activated += new System.EventHandler(this.frmStationClose_Activated);
            this.Load += new System.EventHandler(this.frmStationClose_Load);
            this.Resize += new System.EventHandler(this.frmStationClose_Resize);
            this.ultraTabPageControl1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numTransFee)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numGrandTotal)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numReceiveOnAccount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numTotalCash)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numSalesTax)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numNetSale)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numTotalDiscount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numTotalReturn)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numTotalSale)).EndInit();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numVerifyCashDiff)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numStartingBalance)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtVerifyBy)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtVerifyDate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numVerifyAmount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numCashDifference)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numUserEnterAmount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numPayOut)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numNetCash)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numTotal)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numTotalCashU)).EndInit();
            this.ultraTabPageControl2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdDepartments)).EndInit();
            this.ultraTabPageControl3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdTransDetail)).EndInit();
            this.pnlTop.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.txtStationId)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtStationCloseNo)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtCloseDate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtStationName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboDrawNo)).EndInit();
            this.groupBox4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ultraTabControl1)).EndInit();
            this.ultraTabControl1.ResumeLayout(false);
            this.ResumeLayout(false);

        }
        #endregion

        private void cmdClose_Click(object sender, System.EventArgs e)
        {
            this.Close();
        }
        /// <summary>
        /// Aother: Shitaljit
        /// To do station close related operations.
        /// </summary>
        public void CloseStation(string sDrawerNo, bool isCalledFromEOD)
        {

            oCloseStationData = oStationClose.Close(sDrawerNo);
            Display();
            if (isCalledFromEOD == false)
            {
                RxLabel oLabel = new RxLabel(null, null, null, ReceiptType.Void, null);
                if (sDrawerNo == "1")
                {
                    oLabel.OpenDrawer(1);
                }
                else if (sDrawerNo == "2")
                {
                    oLabel.OpenDrawer(2);
                }
                else
                {
                    oLabel.OpenDrawer(1);
                    oLabel.OpenDrawer(2);
                }

                FillListView(oCloseStationData);
                FillDepartmentListView(oCloseStationData);  //Sprint-24 - PRIMEPOS-2326 20-Oct-2016 JY Added
                PopulateTransDetail(oCloseStationData.StationCloseNo);  //PRIMEPOS-2494 27-Feb-2018 JY Added
                this.cmdProcess.Text = "&Print CS Report";
                btnPrintTransactionDetails.Visible = true;
                cmdProcess.Width = btnPrintTransactionDetails.Width;
                cmdProcess.Left = cmdClose.Left - cmdProcess.Width - 5;
                btnPrintTransactionDetails.Left = cmdProcess.Left - btnPrintTransactionDetails.Width - 5;
                btnVerify.Left = btnPrintTransactionDetails.Left - btnVerify.Width - 5;
            }
            Print(false);
        }
        private void cmdProcess_Click(object sender, System.EventArgs e)
        {
            try
            {
                if (cmdProcess.Text != "&Print CS Report")
                {
                    if (this.cboDrawNo.SelectedIndex == -1)
                    {
                        clsUIHelper.ShowErrorMsg("Please select Draw#.");
                        return;
                    }
                    else if (Resources.Message.Display("Close this station.Are you sure?", "Close Station", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        //Following If is added By Shitaljit on 15 June 2011
                        if (!Configuration.CInfo.UseCashManagement)
                        {
                            CloseStation(this.cboDrawNo.Value.ToString(), false);
                            #region commented Code
                            //oCloseStationData = oStationClose.Close(this.cboDrawNo.Value.ToString());
                            //Display();
                            //Reports.ReportsUI.RxLabel oLabel = new POS_Core_UI.Reports.ReportsUI.RxLabel(null, null, null);
                            //if (this.cboDrawNo.Value.ToString() == "1")
                            //{
                            //    oLabel.OpenDrawer(1);
                            //}
                            //else if (this.cboDrawNo.Value.ToString() == "2")
                            //{
                            //    oLabel.OpenDrawer(2);
                            //}
                            //else
                            //{
                            //    oLabel.OpenDrawer(1);
                            //    oLabel.OpenDrawer(2);
                            //}

                            //FillListView(oCloseStationData);
                            //Print(false);
                            //this.cmdProcess.Text = "&Print";
                            #endregion
                        }
                        //Addded By Shitaljit(QuicSolv) on 10 June 2011
                        else
                        {
                            if (Configuration.CInfo.UseCashManagement)
                            {
                                frmStationCloseCash ofrmStcloseCash = new frmStationCloseCash(this.cboDrawNo.Value.ToString());
                                ofrmStcloseCash.StartPosition = FormStartPosition.CenterScreen;
                                //Print(false);//Added by Ravindra for PRIMEPOS-574 Add for Testing 
                                string sDrawerNo = cboDrawNo.Text.Trim();
                                RxLabel oLabel = new RxLabel(null, null, null, ReceiptType.Void, null);
                                if (sDrawerNo == "1")
                                {
                                    oLabel.OpenDrawer(1);
                                }
                                else if (sDrawerNo == "2")
                                {
                                    oLabel.OpenDrawer(2);
                                }
                                else
                                {
                                    oLabel.OpenDrawer(1);
                                    oLabel.OpenDrawer(2);
                                }

                                //
                                ofrmStcloseCash.ShowDialog();
                                if (!frmStationCloseCash.FlagCloseStationForm)
                                {
                                    this.Close();
                                }
                            }
                        }
                        //Till Here Added By Shitaljit.
                    }
                }
                else
                {
                    Print(true);
                }
            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }

        private void Display()
        {
            decimal CashbackAmount = 0;
            for (int i = 0; i < oCloseStationData.Details.Count; i++)
            {
                if (oCloseStationData.Details[i].PayTypeName == "Cash Back")
                {
                    CashbackAmount = oCloseStationData.Details[i].Amount;
                }
            }
            numGrandTotal.Value = oCloseStationData.GrandTotal;
            numNetCash.Value = oCloseStationData.NetCash + CashbackAmount;
            numNetSale.Value = oCloseStationData.NetSale;
            numPayOut.Value = oCloseStationData.Payout;
            numReceiveOnAccount.Value = oCloseStationData.ReceiveOnAccount;
            numSalesTax.Value = oCloseStationData.SalesTax;
            numTotal.Value = oCloseStationData.Total;
            numTotalCash.Value = oCloseStationData.TotalCash;
            numTotalCashU.Value = oCloseStationData.TotalCashPT;
            //Following if-els is added by shitaljit(QuicSolv) on 10 jan 2012
            if (oCloseStationData.TotalDiscount > 0)
            {
                numTotalDiscount.Value = oCloseStationData.TotalDiscount * -1;
            }
            else
            {
                numTotalDiscount.Value = oCloseStationData.TotalDiscount;
            }
            numTotalReturn.Value = oCloseStationData.TotalReturn;
            numTotalSale.Value = oCloseStationData.TotalSale;
            //Start : Added By Amit Date 20 April 2011
            if (Configuration.CInfo.PrintStCloseNo == true)
                txtStationCloseNo.Visible = true;
            else
                txtStationCloseNo.Visible = false;
            //End
            txtStationCloseNo.Text = oCloseStationData.StationCloseNo;
            this.txtStationId.Text = oCloseStationData.StationID;
            this.txtStationName.Text = Configuration.GetStationName(oCloseStationData.StationID);
            numStartingBalance.Value = oCloseStationData.DefCDStartBalance; //Sprint-19 - 2165 19-Mar-2015 JY Added
            //this.txtCloseDate.Text = Configuration.convertNullToString(oCloseStationData.CloseDate);    //PRIMEPOS-2480 26-Jun-2020 JY Added
            this.txtCloseDate.Text = Convert.ToDateTime(oCloseStationData.CloseDate).ToString("MM/dd/yyyy HH:mm tt");   //PRIMEPOS-2480 26-Jun-2020 JY Added
            this.numTransFee.Value = oCloseStationData.TransFee;    //PRIMEPOS-3118 03-Aug-2022 JY Added
        }

        private void FillListView(CloseStationData oCloseStationData)
        {
            ListViewItem oItem;
            lvPayType.Items.Clear();
            string[] arr = { "", "" };

            for (int i = 0; i < oCloseStationData.Details.Count; i++)
            {
                arr[0] = oCloseStationData.Details[i].PayTypeName;
                arr[1] = oCloseStationData.Details[i].Amount.ToString("######0.00");
                oItem = new ListViewItem(arr);
                lvPayType.Items.Add(oItem);
            }
        }

        private void Print(bool isPrint)
        {
            try
            {
                this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
                Int32 id = Configuration.convertNullToInt(txtStationCloseNo.Text);
                Search oSearch = new Search();

                if (Configuration.CPOSSet.UseRcptForCloseStation == true)
                {
                    POS_Core_UI.Reports.ReportsUI.RcptStationClose oSTrpt = new POS_Core_UI.Reports.ReportsUI.RcptStationClose(id);
                    oSTrpt.Print();

                    //Sprint-24 - PRIMEPOS-2363 27-Dec-2016 JY Added
                    if (Configuration.CInfo.AutoEmailStationCloseReport == true)
                    {
                        if (isPrint == false) EmailReport(id);
                    }
                }
                else
                {
                    rptCloseStation oRptCloseStation = new rptCloseStation();
                    rptCloseStationSummary oRptCloseStationSummary = new rptCloseStationSummary();

                    //PRIMEPOS-2386 26-Feb-2021 JY modified
                    #region PRIMEPOS-2495 20-Mar-2018 JY Added
                    rptEODDepartment oEODDepartment = new rptEODDepartment();
                    string sql;
                    sql = " SELECT SCH.CloseDate, T.StClosedID, isnull(d.deptname, ' ') as deptname " +
                        " ,  Sum(CASE T.TRANSTYPE WHEN 1 THEN ExtendedPrice ELSE 0 END) TotalSale " +
                        " , Sum(CASE T.TRANSTYPE WHEN 2 THEN ExtendedPrice ELSE 0 END) TotalReturn " +
                        " ,cast(-Sum(isnull(TD.Discount, 0)) as float) TotalDiscount, Sum(isnull(TD.TaxAmount, 0)) TotalTax, sch.UserID AS Users FROM POSTransaction T " +
                        " INNER JOIN POSTransactionDetail TD ON TD.TransID = T.TransID " +
                        " INNER JOIN Item i ON TD.ItemID = i.ItemID " +
                        " INNER JOIN StationCloseHeader SCH ON SCH.StationCloseID = T.StClosedID " +
                        " LEFT JOIN department d on i.departmentid = d.deptid " +
                        " WHERE T.StClosedID = " + id +
                        " GROUP BY SCH.CloseDate, T.StClosedID, d.deptname, sch.UserID " +
                        " order by d.deptname";

                    DataSet dsDept = oSearch.SearchData(sql);
                    oEODDepartment.Database.Tables[0].SetDataSource(dsDept.Tables[0]);
                    oEODDepartment.SetParameterValue("CompanyLogoPath", strCompanyLogoPath);   //PRIMEPOS-2386 26-Feb-2021 JY Added

                    ((CrystalDecisions.CrystalReports.Engine.TextObject)oEODDepartment.ReportDefinition.ReportObjects["txtHeader"]).Text = Configuration.CInfo.StoreName;
                    try
                    {
                        ((CrystalDecisions.CrystalReports.Engine.TextObject)oEODDepartment.ReportDefinition.ReportObjects["txtCloseDate"]).Text = "Close Date: " + Convert.ToDateTime(dsDept.Tables[0].Rows[0]["CloseDate"]).ToString("MM/dd/yyyy HH:mm tt");   //PRIMEPOS-3114 10-Aug-2022 JY modified
                    }
                    catch
                    {
                        ((CrystalDecisions.CrystalReports.Engine.TextObject)oEODDepartment.ReportDefinition.ReportObjects["txtCloseDate"]).Text = " ";
                    }
                    if (Configuration.CInfo.PrintStCloseNo == false)
                        ((CrystalDecisions.CrystalReports.Engine.TextObject)oEODDepartment.ReportDefinition.ReportObjects["txtId"]).Text = " ";
                    else
                        ((CrystalDecisions.CrystalReports.Engine.TextObject)oEODDepartment.ReportDefinition.ReportObjects["txtId"]).Text = "Station Close #: " + id.ToString();

                    ((CrystalDecisions.CrystalReports.Engine.TextObject)oEODDepartment.ReportDefinition.ReportObjects["txtHeader"]).Text = Configuration.CInfo.StoreName;
                    ((CrystalDecisions.CrystalReports.Engine.TextObject)oEODDepartment.ReportDefinition.ReportObjects["txtPharmacyAddress"]).Text = Configuration.CInfo.Address;
                    ((CrystalDecisions.CrystalReports.Engine.TextObject)oEODDepartment.ReportDefinition.ReportObjects["txtPharmacyCityStateZip"]).Text = Configuration.CInfo.City + ", " + Configuration.CInfo.State + " " + Configuration.CInfo.Zip;
                    ((CrystalDecisions.CrystalReports.Engine.TextObject)oEODDepartment.ReportDefinition.ReportObjects["txtTelephoneNo"]).Text = Configuration.CInfo.Telephone;
                    #endregion

                    #region PRIMEPOS-2495 14-Mar-2018 JY Added logic to print transaction details
                    rptTransactionDetail1 orptTransactionDetail1 = new rptTransactionDetail1();
                    DataSet dsTransactionDetail = GetTransactionDetails(id);
                    orptTransactionDetail1.SetDataSource(dsTransactionDetail.Tables[0]);
                    orptTransactionDetail1.SetParameterValue("CompanyLogoPath", strCompanyLogoPath);   //PRIMEPOS-2386 26-Feb-2021 JY Added

                    #region PRIMEPOS-2480 26-Jun-2020 JY Added
                    try
                    {
                        ((CrystalDecisions.CrystalReports.Engine.TextObject)orptTransactionDetail1.ReportDefinition.ReportObjects["txtCloseDate"]).Text = "Close Date: " + Convert.ToDateTime(dsTransactionDetail.Tables[0].Rows[0]["CloseDate"]).ToString("MM/dd/yyyy HH:mm tt");  //PRIMEPOS-3114 10-Aug-2022 JY modified
                    }
                    catch
                    {
                        ((CrystalDecisions.CrystalReports.Engine.TextObject)orptTransactionDetail1.ReportDefinition.ReportObjects["txtCloseDate"]).Text = " ";
                    }
                    if (Configuration.CInfo.PrintStCloseNo == false)
                        ((CrystalDecisions.CrystalReports.Engine.TextObject)orptTransactionDetail1.ReportDefinition.ReportObjects["txtId"]).Text = " ";
                    else
                        ((CrystalDecisions.CrystalReports.Engine.TextObject)orptTransactionDetail1.ReportDefinition.ReportObjects["txtId"]).Text = "Station Close #: " + id.ToString();
                    #endregion

                    ((CrystalDecisions.CrystalReports.Engine.TextObject)orptTransactionDetail1.ReportDefinition.ReportObjects["txtHeader"]).Text = Configuration.CInfo.StoreName;
                    ((CrystalDecisions.CrystalReports.Engine.TextObject)orptTransactionDetail1.ReportDefinition.ReportObjects["txtPharmacyAddress"]).Text = Configuration.CInfo.Address;
                    ((CrystalDecisions.CrystalReports.Engine.TextObject)orptTransactionDetail1.ReportDefinition.ReportObjects["txtPharmacyCityStateZip"]).Text = Configuration.CInfo.City + ", " + Configuration.CInfo.State + " " + Configuration.CInfo.Zip;
                    ((CrystalDecisions.CrystalReports.Engine.TextObject)orptTransactionDetail1.ReportDefinition.ReportObjects["txtTelephoneNo"]).Text = Configuration.CInfo.Telephone;
                    #endregion

                    //PRIMEPOS-XXXX 25-May-2017 JY Commented 
                    //string sql = " select(select sum(Totalvalue) from stationclosecash where stationcloseid="+id+") as TotalCash, case transtype when 'U-1' then 1 when 'U-2' then 1 when 'U-3' then 1 when 'U-4' then 1 " +
                    //    " when 'U-5' then 1 when 'U-6' then 1 when 'U-7' then 1 when 'U-C' then 1 when 'U-H' then 1 when 'U-B' then 1 when 'U-E' then 1 when 'PO' then -1 else 2 end as GroupType, " +
                    //    " sch.*,scd.*,ps.*, ISNULL((select Pt.PayTypeDesc  from Paytype PT where  SUBSTRING(scd.TransType, 3, 1) = pt.PayTypeID),TransType) as PayTypeDesc"+
                    //    " FROM stationcloseheader sch,stationclosedetail scd, util_POSSet ps " +
                    //    " where sch.stationid=ps.stationid and sch.stationcloseid=scd.stationcloseid and sch.stationcloseid=" + id;

                    //PRIMEPOS-XXXX 25-May-2017 JY Added below query to work in case of multiple drawer records against one station //PRIMEPOS-2983 02-Jul-2021 JY Modified
                    sql = "select (select ISNULL(sum(Totalvalue),0) from stationclosecash where stationcloseid=" + id + ") as TotalCash," +
                        " case transtype when 'U-1' then 1 when 'U-2' then 1 when 'U-3' then 1 when 'U-4' then 1  when 'U-5' then 1 when 'U-6' then 1 when 'U-7' then 1 when 'U-C' then 1 when 'U-H' then 1 when 'U-B' then 1 when 'U-E' then 1 when 'PO' then - 1 else 2 end as GroupType," +
                        " ISNULL((select Pt.PayTypeDesc  from Paytype PT where SUBSTRING(scd.TransType, 3, len(scd.TransType)) = pt.PayTypeID),TransType) as PayTypeDesc," +
                        " scd.TransType, SUM(ISNULL(scd.TransCount, 0)) AS TransCount, SUM(ISNULL(scd.TransAmount, 0)) AS TransAmount, ps.STATIONNAME, sch.UserID, sch.CloseDate, sch.StationCloseID" +
                        " FROM stationcloseheader sch" +
                        " INNER JOIN stationclosedetail scd ON sch.stationcloseid = scd.stationcloseid" +
                        " INNER JOIN util_POSSet ps ON sch.stationid = ps.stationid" +
                        " WHERE sch.stationcloseid =" + id +
                        " GROUP BY scd.TransType, ps.STATIONNAME, sch.UserID, sch.CloseDate, sch.StationCloseID";

                    frmEndOfDay ofrmEOD = new frmEndOfDay();
                    //Search oSearch=new Search();
                    DataSet ds = oSearch.SearchData(sql);
                    //DataSet subDS= oSearch.SearchData(subSQL);
                    DataTable dtStClose = ofrmEOD.setGroupType(ds.Tables[0]);
                    ((CrystalDecisions.CrystalReports.Engine.TextObject)oRptCloseStation.ReportDefinition.ReportObjects["txtHeader"]).Text = Configuration.CInfo.StoreName;
                    ((CrystalDecisions.CrystalReports.Engine.TextObject)oRptCloseStation.ReportDefinition.ReportObjects["txtPharmacyAddress"]).Text = Configuration.CInfo.Address;
                    ((CrystalDecisions.CrystalReports.Engine.TextObject)oRptCloseStation.ReportDefinition.ReportObjects["txtPharmacyCityStateZip"]).Text = Configuration.CInfo.City + ", " + Configuration.CInfo.State + " " + Configuration.CInfo.Zip;
                    ((CrystalDecisions.CrystalReports.Engine.TextObject)oRptCloseStation.ReportDefinition.ReportObjects["txtTelephoneNo"]).Text = Configuration.CInfo.Telephone;

                    //oRptCloseStation.OpenSubreport("rptStationClosePmt").Database.Tables[0].SetDataSource(subDS.Tables[0]);
                    //POS_Core_UI.Reports.ReportsUI.clsReports.PrintReport(oRptCloseStation);
                    //String subreportName; 
                    //SubreportObject subreportObject;
                    //ReportDocument CSPayment; 
                    //subreportObject = (SubreportObject)oRptCloseStation.ReportDefinition.ReportObjects["rptStationClosePmt"];
                    //subreportName = subreportObject.SubreportName;

                    //CSPayment= oRptCloseStation.OpenSubreport(subreportName);

                    //CSPayment.Database.Tables[0].SetDataSource(subDS.Tables[0] );


                    //Added By Shitaljit(QuicSolv) on 5 July 2011
                    DataSet dsStnCloseCash = new DataSet();
                    dsCloseStationCurrDenom odsCloseStationCurrDenom = new dsCloseStationCurrDenom();
                    dsStnCloseCash = oStationClose.GetStationCloseCashDetail(id);
                    if (dsStnCloseCash.Tables[0].Rows.Count > 0)
                    {
                        rptCloseStationCurrDenom oRptCloseStationCurrDenom = new rptCloseStationCurrDenom();
                        string sqlCurrDenom = string.Empty;
                        //Modified by shitaljit on 3dec2013 to print verified total once manager has verified the station close 
                        if (this.txtVerifyBy.Visible == false)
                        {
                            sqlCurrDenom = @"SELECT cd.CurrencyDescription , cd.IsCoin, scc.Count,scc.TotalValue,
                                            sch.UserID, sch.CloseDate
                                            FROM stationclosecash scc , currencydenominations cd, StationCloseHeader sch
                                            WHERE cd.currencydenomid = scc.currencydenomid and  sch.stationcloseid = scc.stationcloseid
		                                    and scc.stationcloseid =" + id;
                        }
                        else
                        {
                            sqlCurrDenom = @"SELECT cd.CurrencyDescription , cd.IsCoin, scc.VerifiedCount as Count,scc.VerifiedTotalValue as TotalValue,
                                            sch.UserID, sch.CloseDate
                                            FROM stationclosecash scc , currencydenominations cd, StationCloseHeader sch
                                            WHERE cd.currencydenomid = scc.currencydenomid and  sch.stationcloseid = scc.stationcloseid
		                                    and scc.stationcloseid =" + id;
                        }
                        DataSet dsCurrDenom = new DataSet();
                        dsCurrDenom = oSearch.SearchData(sqlCurrDenom);
                        for (int index = 0; index < dsCurrDenom.Tables[0].Rows.Count; index++)
                        {
                            odsCloseStationCurrDenom.Tables[0].ImportRow(dsCurrDenom.Tables[0].Rows[index]);
                        }
                        ((CrystalDecisions.CrystalReports.Engine.TextObject)oRptCloseStationCurrDenom.ReportDefinition.ReportObjects["txtHeader"]).Text = Configuration.CInfo.StoreName;
                        ((CrystalDecisions.CrystalReports.Engine.TextObject)oRptCloseStationCurrDenom.ReportDefinition.ReportObjects["txtPharmacyAddress"]).Text = Configuration.CInfo.Address;
                        ((CrystalDecisions.CrystalReports.Engine.TextObject)oRptCloseStationCurrDenom.ReportDefinition.ReportObjects["txtPharmacyCityStateZip"]).Text = Configuration.CInfo.City + ", " + Configuration.CInfo.State + " " + Configuration.CInfo.Zip;
                        ((CrystalDecisions.CrystalReports.Engine.TextObject)oRptCloseStationCurrDenom.ReportDefinition.ReportObjects["txtTelephoneNo"]).Text = Configuration.CInfo.Telephone;

                        ((CrystalDecisions.CrystalReports.Engine.TextObject)oRptCloseStationSummary.ReportDefinition.ReportObjects["txtHeader"]).Text = Configuration.CInfo.StoreName;
                        ((CrystalDecisions.CrystalReports.Engine.TextObject)oRptCloseStationSummary.ReportDefinition.ReportObjects["txtPharmacyAddress"]).Text = Configuration.CInfo.Address;
                        ((CrystalDecisions.CrystalReports.Engine.TextObject)oRptCloseStationSummary.ReportDefinition.ReportObjects["txtPharmacyCityStateZip"]).Text = Configuration.CInfo.City + ", " + Configuration.CInfo.State + " " + Configuration.CInfo.Zip;
                        ((CrystalDecisions.CrystalReports.Engine.TextObject)oRptCloseStationSummary.ReportDefinition.ReportObjects["txtTelephoneNo"]).Text = Configuration.CInfo.Telephone;

                        oRptCloseStationCurrDenom.Database.Tables[0].SetDataSource(odsCloseStationCurrDenom.Tables[0]);
                        //oRptCloseStationSummary.Database.Tables[0].SetDataSource(ds.Tables[0]);
                        oRptCloseStationSummary.Database.Tables[0].SetDataSource(dtStClose);

                        oRptCloseStationCurrDenom.SetParameterValue("CompanyLogoPath", strCompanyLogoPath);   //PRIMEPOS-2386 26-Feb-2021 JY Added
                        oRptCloseStationSummary.SetParameterValue("CompanyLogoPath", strCompanyLogoPath);   //PRIMEPOS-2386 26-Feb-2021 JY Added

                        if (Configuration.CInfo.PrintStCloseNo)
                        {
                            clsReports.setCRTextObjectText("txtCloseID", id.ToString(), oRptCloseStationCurrDenom);
                            clsReports.setCRTextObjectText("txtCloseID", id.ToString(), oRptCloseStationSummary);
                        }
                        clsReports.setCRTextObjectText("txtStationName", txtStationName.Text, oRptCloseStationCurrDenom);

                        if (isPrint == false)
                        {
                            POS_Core_UI.Reports.ReportsUI.clsReports.ShowReport(oRptCloseStationSummary);
                            POS_Core_UI.Reports.ReportsUI.clsReports.ShowReport(oRptCloseStationCurrDenom);
                            POS_Core_UI.Reports.ReportsUI.clsReports.ShowReport(oEODDepartment);
                            //POS_Core_UI.Reports.ReportsUI.clsReports.ShowReport(orptTransactionDetail1);    //PRIMEPOS-2495 14-Mar-2018 JY Added

                            #region Sprint-24 - PRIMEPOS-2363 20-Dec-2016 JY Added logic auto send station close report
                            if (Configuration.CInfo.AutoEmailStationCloseReport == true)
                            {
                                oRptCloseStationSummary.SetParameterValue("Currency", Configuration.CInfo.CurrencySymbol.ToString());   //PRIMEPOS-2495 15-Mar-2018 JY Added
                                oRptCloseStationCurrDenom.SetParameterValue("Currency", Configuration.CInfo.CurrencySymbol.ToString()); //PRIMEPOS-2495 15-Mar-2018 JY Added
                                oEODDepartment.SetParameterValue("Currency", Configuration.CInfo.CurrencySymbol.ToString());    //PRIMEPOS-2495 15-Mar-2018 JY Added
                                orptTransactionDetail1.SetParameterValue("Currency", Configuration.CInfo.CurrencySymbol.ToString());    //PRIMEPOS-2495 15-Mar-2018 JY Added

                                List<ReportClass> lstReportClass = new List<ReportClass>();
                                lstReportClass.Add(oRptCloseStationSummary);
                                lstReportClass.Add(oRptCloseStationCurrDenom);
                                lstReportClass.Add(oEODDepartment);
                                lstReportClass.Add(orptTransactionDetail1); //PRIMEPOS-2495 14-Mar-2018 JY Added
                                new System.Threading.Thread(delegate ()
                                {
                                    clsReports.EmailReport(lstReportClass, Configuration.CInfo.OwnersEmailId, "Close Station Report - Close #:" + id.ToString(), "Close Station Report", "File");
                                }).Start();
                            }
                            #endregion
                        }
                        else
                        {
                            POS_Core_UI.Reports.ReportsUI.clsReports.PrintReport(oRptCloseStationSummary);
                            POS_Core_UI.Reports.ReportsUI.clsReports.PrintReport(oRptCloseStationCurrDenom);
                            POS_Core_UI.Reports.ReportsUI.clsReports.PrintReport(oEODDepartment);
                            //POS_Core_UI.Reports.ReportsUI.clsReports.PrintReport(orptTransactionDetail1);
                        }
                        //End of Added By Shitaljit.
                    }
                    // Following else is added by shitaljit(QuicSolv) On 6 July 2011
                    //To print the closeStation Report in normal format withouth the currency denominations.
                    else
                    {
                        #region PRIMEPOS-XXXX 25-May-2017 JY Added code to keep existing functionality working as it is in case of CASH Management settings off //PRIMEPOS-2983 02-Jul-2021 JY Modified
                        sql = "select (select ISNULL(sum(Totalvalue),0) from stationclosecash where stationcloseid=" + id + ") as TotalCash," +
                        " case transtype when 'U-1' then 1 when 'U-2' then 1 when 'U-3' then 1 when 'U-4' then 1  when 'U-5' then 1 when 'U-6' then 1 when 'U-7' then 1 when 'U-C' then 1 when 'U-H' then 1 when 'U-B' then 1 when 'U-E' then 1 when 'PO' then - 1 else 2 end as GroupType," +
                        " ISNULL((select Pt.PayTypeDesc  from Paytype PT where SUBSTRING(scd.TransType, 3, len(scd.TransType)) = pt.PayTypeID),TransType) as PayTypeDesc," +
                        " scd.TransType, scd.TransCount, scd.TransAmount, ps.STATIONNAME, sch.UserID, sch.CloseDate, sch.StationCloseID, scd.DrawNo" +
                        " FROM stationcloseheader sch" +
                        " INNER JOIN stationclosedetail scd ON sch.stationcloseid = scd.stationcloseid" +
                        " INNER JOIN util_POSSet ps ON sch.stationid = ps.stationid" +
                        " WHERE sch.stationcloseid =" + id;

                        ds = oSearch.SearchData(sql);
                        dtStClose = ofrmEOD.setGroupType(ds.Tables[0]);

                        if (Configuration.CInfo.PrintStCloseNo)
                        {
                            clsReports.setCRTextObjectText("txtCloseID", id.ToString(), oRptCloseStation);  //Added code to display CloseId on report  
                        }
                        ((CrystalDecisions.CrystalReports.Engine.TextObject)oRptCloseStation.ReportDefinition.ReportObjects["txtHeader"]).Text = Configuration.CInfo.StoreName;
                        ((CrystalDecisions.CrystalReports.Engine.TextObject)oRptCloseStation.ReportDefinition.ReportObjects["txtPharmacyAddress"]).Text = Configuration.CInfo.Address;
                        ((CrystalDecisions.CrystalReports.Engine.TextObject)oRptCloseStation.ReportDefinition.ReportObjects["txtPharmacyCityStateZip"]).Text = Configuration.CInfo.City + ", " + Configuration.CInfo.State + " " + Configuration.CInfo.Zip;
                        ((CrystalDecisions.CrystalReports.Engine.TextObject)oRptCloseStation.ReportDefinition.ReportObjects["txtTelephoneNo"]).Text = Configuration.CInfo.Telephone;
                        #endregion

                        //oRptCloseStation.Database.Tables[0].SetDataSource(ds.Tables[0]);
                        oRptCloseStation.Database.Tables[0].SetDataSource(dtStClose);
                        oRptCloseStation.SetParameterValue("CompanyLogoPath", strCompanyLogoPath);   //PRIMEPOS-2386 26-Feb-2021 JY Added

                        if (isPrint == false)
                        {
                            POS_Core_UI.Reports.ReportsUI.clsReports.ShowReport(oRptCloseStation);
                            POS_Core_UI.Reports.ReportsUI.clsReports.ShowReport(oEODDepartment);
                            //POS_Core_UI.Reports.ReportsUI.clsReports.ShowReport(orptTransactionDetail1);        //PRIMEPOS-2495 14-Mar-2018 JY Added 
                            #region Sprint-24 - PRIMEPOS-2363 20-Dec-2016 JY Added
                            if (Configuration.CInfo.AutoEmailStationCloseReport == true)
                            {
                                oRptCloseStation.SetParameterValue("Currency", Configuration.CInfo.CurrencySymbol.ToString());
                                oEODDepartment.SetParameterValue("Currency", Configuration.CInfo.CurrencySymbol.ToString());
                                orptTransactionDetail1.SetParameterValue("Currency", Configuration.CInfo.CurrencySymbol.ToString());    //PRIMEPOS-2495 14-Mar-2018 JY Added

                                List<ReportClass> lstReportClass = new List<ReportClass>();
                                lstReportClass.Add(oRptCloseStation);
                                lstReportClass.Add(oEODDepartment); //PRIMEPOS-3086 20-Apr-2022 JY Added
                                lstReportClass.Add(orptTransactionDetail1); //PRIMEPOS-2495 14-Mar-2018 JY Added
                                new System.Threading.Thread(delegate ()
                                {
                                    clsReports.EmailReport(lstReportClass, Configuration.CInfo.OwnersEmailId, "Close Station Report - Close #:" + id.ToString(), "Close Station Report", "File");
                                }).Start();
                            }
                            #endregion
                        }
                        else
                        {
                            POS_Core_UI.Reports.ReportsUI.clsReports.PrintReport(oRptCloseStation);
                            POS_Core_UI.Reports.ReportsUI.clsReports.PrintReport(oEODDepartment);
                            //POS_Core_UI.Reports.ReportsUI.clsReports.PrintReport(orptTransactionDetail1);                            
                        }
                    }
                }
                this.Cursor = System.Windows.Forms.Cursors.Default;
            }
            catch (Exception exp)
            {
                this.Cursor = System.Windows.Forms.Cursors.Default;
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }

        public void Edit(int StationCloseId)
        {
            bEditMode = true;
            decimal DifferenceAmount = 0;
            decimal VerifyDiffAmt = 0;  //Sprint-19 - 2165 19-Mar-2015 JY Added 
            decimal userEnterAmount = 0;
            decimal verifiedAmount = 0;
            bool isVerifiedFlag = false;
            try
            {
                cmdProcess.Text = "&Print CS Report";
                btnPrintTransactionDetails.Visible = true;
                cmdProcess.Width = btnPrintTransactionDetails.Width;
                cmdProcess.Left = cmdClose.Left - cmdProcess.Width - 5;
                btnPrintTransactionDetails.Left = cmdProcess.Left - btnPrintTransactionDetails.Width - 5;
                btnVerify.Left = btnPrintTransactionDetails.Left - btnVerify.Width - 5;
                oCloseStationData = oStationClose.FillData(StationCloseId);
                Display();
                userEnterAmount = oStationClose.GetUserEnterStationCloseCash(StationCloseId);   //PRIMEPOS-XXXX 24-May-2017 JY Added
                //Added By Shitaljit(QuicSolv) on 15 June 2011
                if (UserPriviliges.IsUserHasPriviliges(UserPriviliges.Modules.AdminFunc.ID, UserPriviliges.Screens.StationClose.ID, UserPriviliges.Permissions.UnClStView.ID))
                {
                    //userEnterAmount = oStationClose.GetUserEnterStationCloseCash(StationCloseId); //PRIMEPOS-XXXX 24-May-2017 JY Commented
                    //DataSet dsStationCloseData = new DataSet();
                    //dsStationCloseData = oStationClose.GetStationCloseCashDetail(StationCloseId);
                    if (oCloseStationData.VerifiedBy != "")
                    {
                        isVerifiedFlag = true;
                        verifiedAmount = oCloseStationData.VerifiedAmount;
                        if (isVerifiedFlag)
                        {
                            btnVerify.Visible = false;
                            lblVerifyBy.Visible = true;
                            txtVerifyBy.Visible = true;
                            txtVerifyBy.Text = oCloseStationData.VerifiedBy;
                            lblVerifyAmt.Visible = true;
                            numVerifyAmount.Visible = true;
                            numVerifyAmount.Value = verifiedAmount;
                            lblVerifyDate.Visible = true;
                            txtVerifyDate.Visible = true;
                            txtVerifyDate.Text = oCloseStationData.VerifiedDate;
                            #region Sprint-19 - 2165 19-Mar-2015 JY Added
                            if (UserPriviliges.IsUserHasPriviliges(UserPriviliges.Modules.AdminFunc.ID, UserPriviliges.Screens.StationClose.ID, UserPriviliges.Permissions.ViewCashDifference.ID))
                            {
                                lblVerifyCashDiff.Visible = true;
                                numVerifyCashDiff.Visible = true;
                            }
                            #endregion
                        }
                    }
                }

                if (userEnterAmount >= 0)
                {
                    DifferenceAmount = Math.Abs(userEnterAmount - Convert.ToDecimal(numNetCash.Value) - oCloseStationData.DefCDStartBalance);   //Sprint-19 - 2165 19-Mar-2015 JY Need to substract oCloseStationData.DefCDStartBalance because for cash mgt starting balance was not considered previously

                    if (!isVerifiedFlag)
                        btnVerify.Visible = true;
                    else
                        btnVerify.Visible = false;

                    lblUserEnterCash.Visible = true;
                    numUserEnterAmount.Visible = true;

                    if (UserPriviliges.IsUserHasPriviliges(UserPriviliges.Modules.AdminFunc.ID, UserPriviliges.Screens.StationClose.ID, UserPriviliges.Permissions.ViewCashDifference.ID))    //Sprint-19 - 2165 19-Mar-2015 JY Added 
                    {
                        lblCashDifference.Visible = true;
                        numCashDifference.Visible = true;
                        lblStartingBalance.Visible = true;  //Sprint-19 - 2165 19-Mar-2015 JY Added 
                        numStartingBalance.Visible = true;  //Sprint-19 - 2165 19-Mar-2015 JY Added 
                    }

                    if (isVerifiedFlag)
                    {
                        if (verifiedAmount != userEnterAmount)
                        {
                            numUserEnterAmount.Appearance.BackColor = System.Drawing.Color.Red;
                            numVerifyAmount.Appearance.BackColor = System.Drawing.Color.Red;
                        }
                        else
                        {
                            numUserEnterAmount.Appearance.BackColor = System.Drawing.Color.Green;
                            numVerifyAmount.Appearance.BackColor = System.Drawing.Color.Green;
                        }
                    }

                    numUserEnterAmount.Value = userEnterAmount;
                    if (Convert.ToDecimal(numUserEnterAmount.Value) == (Convert.ToDecimal(numNetCash.Value) + oCloseStationData.DefCDStartBalance)) //Sprint-19 - 2165 19-Mar-2015 JY Added DefCDStartBalance
                    {
                        lblCashDifference.Text = "Match";
                        numCashDifference.Value = DifferenceAmount;
                    }
                    else if (Convert.ToDecimal(numUserEnterAmount.Value) > (Convert.ToDecimal(numNetCash.Value) + oCloseStationData.DefCDStartBalance))  //Sprint-19 - 2165 19-Mar-2015 JY Added DefCDStartBalance
                    {
                        lblCashDifference.Text = "Over By";
                        numCashDifference.Value = DifferenceAmount;
                        if (isVerifiedFlag && verifiedAmount != userEnterAmount)
                        {
                            numUserEnterAmount.Appearance.BackColor = System.Drawing.Color.Red;
                            numVerifyAmount.Appearance.BackColor = System.Drawing.Color.Red;
                        }
                    }
                    else if (Convert.ToDecimal(numUserEnterAmount.Value) < (Convert.ToDecimal(numNetCash.Value) + oCloseStationData.DefCDStartBalance))  //Sprint-19 - 2165 19-Mar-2015 JY Added DefCDStartBalance
                    {
                        lblCashDifference.Text = "Under By";
                        numCashDifference.Value = DifferenceAmount;
                    }
                }

                #region Sprint-19 - 2165 19-Mar-2015 JY Added for Verified amount cash difference
                if (verifiedAmount >= 0)
                {
                    VerifyDiffAmt = Math.Abs(verifiedAmount - Convert.ToDecimal(numNetCash.Value) - oCloseStationData.DefCDStartBalance);   //Sprint-19 - 2165 19-Mar-2015 JY Added for Verified amount cash difference

                    if (Convert.ToDecimal(numVerifyAmount.Value) == (Convert.ToDecimal(numNetCash.Value) + oCloseStationData.DefCDStartBalance))
                    {
                        lblVerifyCashDiff.Text = "Match";
                        numVerifyCashDiff.Value = VerifyDiffAmt;

                    }
                    else if (Convert.ToDecimal(numVerifyAmount.Value) > (Convert.ToDecimal(numNetCash.Value) + oCloseStationData.DefCDStartBalance))
                    {
                        lblVerifyCashDiff.Text = "Over By";
                        numVerifyCashDiff.Value = VerifyDiffAmt;
                    }
                    else if (Convert.ToDecimal(numVerifyAmount.Value) < (Convert.ToDecimal(numNetCash.Value) + oCloseStationData.DefCDStartBalance))  //Sprint-19 - 2165 19-Mar-2015 JY Added DefCDStartBalance
                    {
                        lblVerifyCashDiff.Text = "Under By";
                        numVerifyCashDiff.Value = VerifyDiffAmt;
                    }
                }
                #endregion

                //End of added by shitaljit.
                FillListView(oCloseStationData);
                FillDepartmentListView(oCloseStationData);  //Sprint-24 - PRIMEPOS-2326 20-Oct-2016 JY Added
                PopulateTransDetail(oCloseStationData.StationCloseNo);  //PRIMEPOS-2494 27-Feb-2018 JY Added
            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }

        private void frmStationClose_Load(object sender, System.EventArgs e)
        {
            clsUIHelper.setColorSchecme(this);
            this.cboDrawNo.SelectedIndex = 0;
            if (!bEditMode)
            {
                //this.txtCloseDate.Text = Configuration.convertNullToString(DateTime.Now);    //PRIMEPOS-2480 26-Jun-2020 JY Added
                this.txtCloseDate.Text = Convert.ToDateTime(DateTime.Now).ToString("MM/dd/yyyy HH:mm tt");  //PRIMEPOS-2480 26-Jun-2020 JY Added
            }
            // (cmdProcess.Text != "&Print") Added By shitaljit to hide button when user is viewign old station close
            if (UserPriviliges.IsUserHasPriviliges(UserPriviliges.Modules.AdminFunc.ID, UserPriviliges.Screens.StationClose.ID, UserPriviliges.Permissions.UnClStView.ID) && (cmdProcess.Text != "&Print CS Report"))
            //if (Configuration.UserRole==UserManagement.UserRoleENUM.Supervisor)
            {
                this.btnViewCurrentStatus.Visible = true;
                //this.btnVerify.Visible = true;//Added By Shitaljit (QuicSolv) on 15 June 2011
            }
            else
            {
                this.btnViewCurrentStatus.Visible = false;
            }
            if (UserPriviliges.IsUserHasPriviliges(UserPriviliges.Modules.AdminFunc.ID, UserPriviliges.Screens.StationClose.ID, UserPriviliges.Permissions.ProcessStClose.ID))
            //if (Configuration.UserRole==UserManagement.UserRoleENUM.Supervisor)
            {

                this.cmdProcess.Visible = true;
            }
            else
            {
                this.cmdProcess.Visible = false;
            }
            strCompanyLogoPath = Configuration.GetCompanyLogoPath(this);   //PRIMEPOS-2386 26-Feb-2021 JY Added
        }

        private void frmStationClose_Activated(object sender, System.EventArgs e)
        {
            clsUIHelper.CurrentForm = this;
        }


        //This button provides view of a register without close station
        private void btnViewCurrentStatus_Click(object sender, System.EventArgs e)
        {

            try
            {
                if (this.cboDrawNo.SelectedIndex == -1)
                {
                    clsUIHelper.ShowErrorMsg("Please select Draw#.");
                    return;
                }

                this.Cursor = System.Windows.Forms.Cursors.WaitCursor;

                rptCloseStation oRptCloseStation = new rptCloseStation();

                Search oSearch = new Search();
                DataSet ds = oSearch.SearchData(BuildCurrentViewQuery());
                frmEndOfDay ofrmEOD = new frmEndOfDay();
                DataTable dtCurrStatus = ofrmEOD.setGroupType(ds.Tables[0]);
                ((CrystalDecisions.CrystalReports.Engine.TextObject)oRptCloseStation.ReportDefinition.ReportObjects["txtHeader"]).Text = Configuration.CInfo.StoreName;
                ((CrystalDecisions.CrystalReports.Engine.TextObject)oRptCloseStation.ReportDefinition.ReportObjects["txtTitle"]).Text = "Station " + Configuration.StationName + " Current Status";

                ((CrystalDecisions.CrystalReports.Engine.TextObject)oRptCloseStation.ReportDefinition.ReportObjects["txtPharmacyAddress"]).Text = Configuration.CInfo.Address;
                //Following line is added By Shitaljit(QuicSolv) on 22 July 2011
                ((CrystalDecisions.CrystalReports.Engine.TextObject)oRptCloseStation.ReportDefinition.ReportObjects["txtPharmacyCityStateZip"]).Text = Configuration.CInfo.City + ", " + Configuration.CInfo.State + " " + Configuration.CInfo.Zip;
                //Start: Modified By Amit Date 12 May 2011
                //((CrystalDecisions.CrystalReports.Engine.TextObject)oRptCloseStation.ReportDefinition.ReportObjects["TelephoneNo"]).Text = Configuration.CInfo.Telephone;
                ((CrystalDecisions.CrystalReports.Engine.TextObject)oRptCloseStation.ReportDefinition.ReportObjects["txtTelephoneNo"]).Text = "(" + Configuration.CInfo.Telephone.Substring(0, 3) + ")" + Configuration.CInfo.Telephone.Substring(3, 3) + "-" + Configuration.CInfo.Telephone.Substring(6, 4);
                //End
                //Start : Added By Amit Date 20 April 2011
                if (Configuration.CInfo.PrintStCloseNo == false)
                {
                    ((CrystalDecisions.CrystalReports.Engine.TextObject)oRptCloseStation.ReportDefinition.ReportObjects["txtClose"]).Text = "";
                    ((CrystalDecisions.CrystalReports.Engine.TextObject)oRptCloseStation.ReportDefinition.ReportObjects["txtCloseID"]).Text = "";
                }
                else
                {
                    //((CrystalDecisions.CrystalReports.Engine.TextObject)oRptCloseStation.ReportDefinition.ReportObjects["txtCloseID"]).Text = oCloseStationData.StationCloseNo.ToString();
                }
                //End
                //oRptCloseStation.Database.Tables[0].SetDataSource(ds.Tables[0]  );
                oRptCloseStation.Database.Tables[0].SetDataSource(dtCurrStatus);
                oRptCloseStation.SetParameterValue("CompanyLogoPath", strCompanyLogoPath);   //PRIMEPOS-2386 26-Feb-2021 JY Added
                POS_Core_UI.Reports.ReportsUI.clsReports.ShowReport(oRptCloseStation);
                this.Cursor = System.Windows.Forms.Cursors.Default;

            }
            catch (Exception exp)
            {
                this.Cursor = System.Windows.Forms.Cursors.Default;
                clsUIHelper.ShowErrorMsg(exp.Message);
            }

        }

        //this function builds the query base on station and drawer selected 
        //to print station current status without closing
        private String BuildCurrentViewQuery()
        {

            String strSql = "";
            String sDrawNo = "";
            String sDrawNo1 = "";
            if (this.cboDrawNo.Text == "ALL" || this.cboDrawNo.Text == "")
            {
                sDrawNo = "";
                sDrawNo1 = "";
            }
            else
            {
                sDrawNo = " and drawerno='" + this.cboDrawNo.Text + "'";
                sDrawNo1 = " and drawno='" + this.cboDrawNo.Text + "'";
            }

            //query required to report
            //PRIMEPOS-2983 02-Jul-2021 JY Modified
            strSql = " select case transtype when 'U-1' then 1 when 'U-2' then 1 when 'U-3' then 1 when 'U-4' then 1  " +
            " when 'U-5' then 1 when 'U-6' then 1 when 'U-7' then 1 when 'U-C' then 1 when 'U-H' then 1 when 'U-B' then 1 when 'PO' then -1 else 2 end as GroupType,  " +
            " sch.*,scd.*,sch.stationid as stationname, ISNULL((select Pt.PayTypeDesc  from Paytype PT where  SUBSTRING(scd.TransType, 3, len(scd.TransType)) = pt.PayTypeID),TransType) as PayTypeDesc from " +
            //query to get header data
            " (select * from (select -1 as stationcloseid, '" + Configuration.StationName + "' as stationid,'" + Configuration.UserName + "' as userid,null as closedate,-1 as eodid) as header) as sch, " +
            //query to get detail data
            " (select * from ( " +
            " (Select -1 as stationcloseid, DrawNo as DrawNo , 'PO' TransType , Count(*) TransCount , Sum(Amount) as TransAmount From Payout WHERE StationId = " + Configuration.StationID + sDrawNo1 + " AND  isnull(IsStationClosed,0) = 0  group by DrawNo) " +
            " UNION ALL " +
            " (SELECT -1 as stationcloseid, DrawerNo as DrawNo , 'S' TransType, Count(*) TransCount, Sum(GrossTotal) as TransAmount FROM POSTransaction WHERE StationId =  " + Configuration.StationID + sDrawNo + "   AND TransType = 1 AND  isnull(IsStationClosed,0) = 0 Group By DrawerNo)  " +
            " UNION ALL  " +
            " (SELECT -1 as stationcloseid, DrawerNo as DrawNo , 'A' TransType, Count(*) TransCount, Sum(totalpaid) as TransAmount FROM POSTransaction WHERE StationId =  " + Configuration.StationID + sDrawNo + "   AND TransType = 3 AND  isnull(IsStationClosed,0) = 0   Group By DrawerNo)  " +
            " UNION ALL " +
            " (SELECT -1 as stationcloseid, DrawerNo as DrawNo , 'SR' TransType, Count(*) TransCount, Sum(GrossTotal) as TransAmount FROM POSTransaction WHERE StationId =  " + Configuration.StationID + sDrawNo + "  AND TransType = 2 AND  isnull(IsStationClosed,0) = 0    Group By DrawerNo) " +
            " UNION ALL  " +
            " (SELECT -1 as stationcloseid, DrawerNo as DrawNo , 'DT' TransType, Count(*) TransCount, Sum(TotalDiscAmount) as TransAmount FROM POSTransaction  WHERE isnull(totaldiscamount,0)<>0 and StationId =  " + Configuration.StationID + sDrawNo + "  AND  isnull(IsStationClosed,0) = 0     Group By DrawerNo)  " +
            " UNION ALL  " +
            " (SELECT -1 as stationcloseid, DrawerNo as DrawNo , 'TX' TransType, Count(*) TransCount, Sum(TotalTaxAmount) as TransAmount FROM POSTransaction WHERE isnull(totaltaxamount,0)<>0 and StationId =  " + Configuration.StationID + sDrawNo + "  AND  isnull(IsStationClosed,0) = 0   Group By DrawerNo) " +
            " UNION ALL " +
            " (SELECT -1 as stationcloseid, DrawerNo as DrawNo , 'U-'+PT.PayTypeId  as TransType, Count(*) as TransCount , Sum(TP.Amount) as TransAmount FROM POSTransPayment As TP , PayType PT , POSTransaction Trn WHERE TP.TransTypeCode = PT.PayTypeId AND TP.TransID = Trn.TransID AND Trn.StationId =  " + Configuration.StationID + sDrawNo + "  AND  isnull(IsStationClosed,0) = 0   GROUP BY PT.PayTypeId,Trn.DrawerNo) " +
            " ) as details) as scd ";

            return strSql;
        }

        private void frmStationClose_Resize(object sender, EventArgs e)
        {
            this.Left = clsPOSDBConstants.formLeft;
            this.Top = clsPOSDBConstants.formLeft;

        }

        private void btnVerify_Click(object sender, EventArgs e)
        {
            decimal UserEnterAmnt, SytemCalculatedAmnt;
            UserEnterAmnt = Convert.ToDecimal(numUserEnterAmount.Value);
            SytemCalculatedAmnt = Convert.ToDecimal(numNetCash.Value);
            int StationCloseID = Convert.ToInt32(txtStationCloseNo.Text);
            frmStationCloseCash ofrmStcloseCash = new frmStationCloseCash(StationCloseID, UserEnterAmnt, SytemCalculatedAmnt);
            ofrmStcloseCash.StartPosition = FormStartPosition.CenterScreen;
            ofrmStcloseCash.ShowDialog(this);
            Edit(StationCloseID);
        }

        #region Sprint-24 - PRIMEPOS-2326 20-Oct-2016 JY Added
        private void FillDepartmentListView(CloseStationData oCloseStationData)
        {
            this.grdDepartments.DataSource = oStationClose.GetDepartmentDS(Configuration.convertNullToInt(oCloseStationData.StationCloseNo));

            this.grdDepartments.DisplayLayout.Bands[0].Columns["deptcode"].Header.Caption = "Dept Code";
            this.grdDepartments.DisplayLayout.Bands[0].Columns["deptname"].Header.Caption = "Dept Name";
            this.grdDepartments.DisplayLayout.Bands[0].Columns["totalsale"].Header.Caption = "Total Sale";
            this.grdDepartments.DisplayLayout.Bands[0].Columns["totalreturn"].Header.Caption = "Total Return";
            this.grdDepartments.DisplayLayout.Bands[0].Columns["totaldiscount"].Header.Caption = "Total Disc.";
            this.grdDepartments.DisplayLayout.Bands[0].Columns["totaltax"].Header.Caption = "Total Tax";
            this.resizeColumns(grdDepartments);
        }

        private void resizeColumns(UltraGrid grd)
        {
            foreach (UltraGridColumn oCol in grd.DisplayLayout.Bands[0].Columns)
            {
                oCol.Width = oCol.CalculateAutoResizeWidth(PerformAutoSizeType.VisibleRows, true) + 10;
                if (oCol.DataType.Equals(typeof(System.Int32)) || oCol.DataType.Equals(typeof(System.Decimal)) || oCol.DataType.Equals(typeof(System.DateTime)))
                {
                    oCol.CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right;
                }
            }
        }
        #endregion

        //public void EmailReport(int StationCloseId)   //PRIMEPOS-3042 22-Dec-2021 JY Commented
        public List<ReportClass> EmailReport(int StationCloseId, bool bCalledFromScheduler = false) //PRIMEPOS-3042 22-Dec-2021 JY Added
        {
            List<ReportClass> lstReportClass = new List<ReportClass>();
            try
            {
                strCompanyLogoPath = Configuration.GetCompanyLogoPath(this);   //PRIMEPOS-2386 26-Feb-2021 JY Added
                Search oSearch = new Search();

                rptCloseStation oRptCloseStation = new rptCloseStation();
                rptCloseStationSummary oRptCloseStationSummary = new rptCloseStationSummary();
                //PRIMEPOS-XXXX 25-May-2017 JY Commented
                //string sql = " select(select sum(Totalvalue) from stationclosecash where stationcloseid=" + StationCloseId + ") as TotalCash, case transtype when 'U-1' then 1 when 'U-2' then 1 when 'U-3' then 1 when 'U-4' then 1 " +
                //            " when 'U-5' then 1 when 'U-6' then 1 when 'U-7' then 1 when 'U-C' then 1 when 'U-H' then 1 when 'U-B' then 1 when 'U-E' then 1 when 'PO' then -1 else 2 end as GroupType, " +
                //            " sch.*,scd.*,ps.*, ISNULL((select Pt.PayTypeDesc  from Paytype PT where  SUBSTRING(scd.TransType, 3, 1) = pt.PayTypeID),TransType) as PayTypeDesc" +
                //            " FROM stationcloseheader sch,stationclosedetail scd, util_POSSet ps " +
                //            " where sch.stationid=ps.stationid and sch.stationcloseid=scd.stationcloseid and sch.stationcloseid=" + StationCloseId;

                //PRIMEPOS-XXXX 25-May-2017 JY Added below query to work in case of multiple drawer records against one station //PRIMEPOS-2983 02-Jul-2021 JY Modified
                string sql = "select (select ISNULL(sum(Totalvalue),0) from stationclosecash where stationcloseid=" + StationCloseId + ") as TotalCash," +
                    " case transtype when 'U-1' then 1 when 'U-2' then 1 when 'U-3' then 1 when 'U-4' then 1  when 'U-5' then 1 when 'U-6' then 1 when 'U-7' then 1 when 'U-C' then 1 when 'U-H' then 1 when 'U-B' then 1 when 'U-E' then 1 when 'PO' then - 1 else 2 end as GroupType," +
                    " ISNULL((select Pt.PayTypeDesc  from Paytype PT where SUBSTRING(scd.TransType, 3, len(scd.TransType)) = pt.PayTypeID),TransType) as PayTypeDesc," +
                    " scd.TransType, SUM(ISNULL(scd.TransCount, 0)) AS TransCount, SUM(ISNULL(scd.TransAmount, 0)) AS TransAmount, ps.STATIONNAME, sch.UserID, sch.CloseDate, sch.StationCloseID" +
                    " FROM stationcloseheader sch" +
                    " INNER JOIN stationclosedetail scd ON sch.stationcloseid = scd.stationcloseid" +
                    " INNER JOIN util_POSSet ps ON sch.stationid = ps.stationid" +
                    " WHERE sch.stationcloseid =" + StationCloseId +
                    " GROUP BY scd.TransType, ps.STATIONNAME, sch.UserID, sch.CloseDate, sch.StationCloseID";

                frmEndOfDay ofrmEOD = new frmEndOfDay();
                //Search oSearch=new Search();
                DataSet ds = oSearch.SearchData(sql);
                DataTable dtStClose = ofrmEOD.setGroupType(ds.Tables[0]);

                //PRIMEPOS-2386 26-Feb-2021 JY modified
                #region PRIMEPOS-2495 20-Mar-2018 JY Added
                rptEODDepartment oEODDepartment = new rptEODDepartment();
                sql = " SELECT SCH.CloseDate, T.StClosedID, isnull(d.deptname, ' ') as deptname " +
                    " ,  Sum(CASE T.TRANSTYPE WHEN 1 THEN ExtendedPrice ELSE 0 END) TotalSale " +
                    " , Sum(CASE T.TRANSTYPE WHEN 2 THEN ExtendedPrice ELSE 0 END) TotalReturn " +
                    " ,cast(-Sum(isnull(TD.Discount, 0)) as float) TotalDiscount, Sum(isnull(TD.TaxAmount, 0)) TotalTax, sch.UserID AS Users FROM POSTransaction T " +
                    " INNER JOIN POSTransactionDetail TD ON TD.TransID = T.TransID " +
                    " INNER JOIN Item i ON TD.ItemID = i.ItemID " +
                    " INNER JOIN StationCloseHeader SCH ON SCH.StationCloseID = T.StClosedID " +
                    " LEFT JOIN department d on i.departmentid = d.deptid " +
                    " WHERE T.StClosedID = " + StationCloseId +
                    " GROUP BY SCH.CloseDate, T.StClosedID, d.deptname, sch.UserID " +
                    " order by d.deptname";

                DataSet dsDept = oSearch.SearchData(sql);
                oEODDepartment.Database.Tables[0].SetDataSource(dsDept.Tables[0]);
                oEODDepartment.SetParameterValue("CompanyLogoPath", strCompanyLogoPath);   //PRIMEPOS-2386 26-Feb-2021 JY Added
                ((CrystalDecisions.CrystalReports.Engine.TextObject)oEODDepartment.ReportDefinition.ReportObjects["txtHeader"]).Text = Configuration.CInfo.StoreName;
                try
                {
                    ((CrystalDecisions.CrystalReports.Engine.TextObject)oEODDepartment.ReportDefinition.ReportObjects["txtCloseDate"]).Text = "Close Date: " + Convert.ToDateTime(dsDept.Tables[0].Rows[0]["CloseDate"]).ToString("MM/dd/yyyy HH:mm tt");   //PRIMEPOS-3114 10-Aug-2022 JY modified
                }
                catch
                {
                    ((CrystalDecisions.CrystalReports.Engine.TextObject)oEODDepartment.ReportDefinition.ReportObjects["txtCloseDate"]).Text = " ";
                }
                if (Configuration.CInfo.PrintStCloseNo == false)
                    ((CrystalDecisions.CrystalReports.Engine.TextObject)oEODDepartment.ReportDefinition.ReportObjects["txtId"]).Text = " ";
                else
                    ((CrystalDecisions.CrystalReports.Engine.TextObject)oEODDepartment.ReportDefinition.ReportObjects["txtId"]).Text = "Station Close #: " + StationCloseId.ToString();

                ((CrystalDecisions.CrystalReports.Engine.TextObject)oEODDepartment.ReportDefinition.ReportObjects["txtHeader"]).Text = Configuration.CInfo.StoreName;
                ((CrystalDecisions.CrystalReports.Engine.TextObject)oEODDepartment.ReportDefinition.ReportObjects["txtPharmacyAddress"]).Text = Configuration.CInfo.Address;
                ((CrystalDecisions.CrystalReports.Engine.TextObject)oEODDepartment.ReportDefinition.ReportObjects["txtPharmacyCityStateZip"]).Text = Configuration.CInfo.City + ", " + Configuration.CInfo.State + " " + Configuration.CInfo.Zip;
                ((CrystalDecisions.CrystalReports.Engine.TextObject)oEODDepartment.ReportDefinition.ReportObjects["txtTelephoneNo"]).Text = Configuration.CInfo.Telephone;
                #endregion

                #region PRIMEPOS-2495 14-Mar-2018 JY Added logic to print transaction details
                rptTransactionDetail1 orptTransactionDetail1 = new rptTransactionDetail1();
                DataSet dsTransactionDetail = GetTransactionDetails(StationCloseId);
                orptTransactionDetail1.SetDataSource(dsTransactionDetail.Tables[0]);
                orptTransactionDetail1.SetParameterValue("CompanyLogoPath", strCompanyLogoPath);   //PRIMEPOS-2386 26-Feb-2021 JY Added

                #region PRIMEPOS-2480 26-Jun-2020 JY Added
                try
                {
                    ((CrystalDecisions.CrystalReports.Engine.TextObject)orptTransactionDetail1.ReportDefinition.ReportObjects["txtCloseDate"]).Text = "Close Date: " + Convert.ToDateTime(dsTransactionDetail.Tables[0].Rows[0]["CloseDate"]).ToString("MM/dd/yyyy HH:mm tt");  //PRIMEPOS-3114 10-Aug-2022 JY modified
                }
                catch
                {
                    ((CrystalDecisions.CrystalReports.Engine.TextObject)orptTransactionDetail1.ReportDefinition.ReportObjects["txtCloseDate"]).Text = " ";
                }
                if (Configuration.CInfo.PrintStCloseNo == false)
                    ((CrystalDecisions.CrystalReports.Engine.TextObject)orptTransactionDetail1.ReportDefinition.ReportObjects["txtId"]).Text = " ";
                else
                    ((CrystalDecisions.CrystalReports.Engine.TextObject)orptTransactionDetail1.ReportDefinition.ReportObjects["txtId"]).Text = "Station Close #: " + StationCloseId.ToString();
                #endregion

                ((CrystalDecisions.CrystalReports.Engine.TextObject)orptTransactionDetail1.ReportDefinition.ReportObjects["txtHeader"]).Text = Configuration.CInfo.StoreName;
                ((CrystalDecisions.CrystalReports.Engine.TextObject)orptTransactionDetail1.ReportDefinition.ReportObjects["txtPharmacyAddress"]).Text = Configuration.CInfo.Address;
                ((CrystalDecisions.CrystalReports.Engine.TextObject)orptTransactionDetail1.ReportDefinition.ReportObjects["txtPharmacyCityStateZip"]).Text = Configuration.CInfo.City + ", " + Configuration.CInfo.State + " " + Configuration.CInfo.Zip;
                ((CrystalDecisions.CrystalReports.Engine.TextObject)orptTransactionDetail1.ReportDefinition.ReportObjects["txtTelephoneNo"]).Text = Configuration.CInfo.Telephone;
                #endregion

                DataSet dsStnCloseCash = new DataSet();
                dsCloseStationCurrDenom odsCloseStationCurrDenom = new dsCloseStationCurrDenom();
                dsStnCloseCash = oStationClose.GetStationCloseCashDetail(StationCloseId);
                if (dsStnCloseCash.Tables[0].Rows.Count > 0)
                {
                    rptCloseStationCurrDenom oRptCloseStationCurrDenom = new rptCloseStationCurrDenom();
                    string sqlCurrDenom = string.Empty;
                    if (this.txtVerifyBy.Visible == false)
                    {
                        sqlCurrDenom = @"SELECT cd.CurrencyDescription , cd.IsCoin, scc.Count,scc.TotalValue,
                                        sch.UserID, sch.CloseDate
                                        FROM stationclosecash scc , currencydenominations cd, StationCloseHeader sch
                                        WHERE cd.currencydenomid = scc.currencydenomid and  sch.stationcloseid = scc.stationcloseid
		                                and scc.stationcloseid =" + StationCloseId;
                    }
                    else
                    {
                        sqlCurrDenom = @"SELECT cd.CurrencyDescription , cd.IsCoin, scc.VerifiedCount as Count,scc.VerifiedTotalValue as TotalValue,
                                        sch.UserID, sch.CloseDate
                                        FROM stationclosecash scc , currencydenominations cd, StationCloseHeader sch
                                        WHERE cd.currencydenomid = scc.currencydenomid and  sch.stationcloseid = scc.stationcloseid
		                                and scc.stationcloseid =" + StationCloseId;
                    }
                    DataSet dsCurrDenom = new DataSet();
                    dsCurrDenom = oSearch.SearchData(sqlCurrDenom);
                    for (int index = 0; index < dsCurrDenom.Tables[0].Rows.Count; index++)
                    {
                        odsCloseStationCurrDenom.Tables[0].ImportRow(dsCurrDenom.Tables[0].Rows[index]);
                    }
                    ((CrystalDecisions.CrystalReports.Engine.TextObject)oRptCloseStationCurrDenom.ReportDefinition.ReportObjects["txtHeader"]).Text = Configuration.CInfo.StoreName;
                    ((CrystalDecisions.CrystalReports.Engine.TextObject)oRptCloseStationCurrDenom.ReportDefinition.ReportObjects["txtPharmacyAddress"]).Text = Configuration.CInfo.Address;
                    ((CrystalDecisions.CrystalReports.Engine.TextObject)oRptCloseStationCurrDenom.ReportDefinition.ReportObjects["txtPharmacyCityStateZip"]).Text = Configuration.CInfo.City + ", " + Configuration.CInfo.State + " " + Configuration.CInfo.Zip;
                    ((CrystalDecisions.CrystalReports.Engine.TextObject)oRptCloseStationCurrDenom.ReportDefinition.ReportObjects["txtTelephoneNo"]).Text = Configuration.CInfo.Telephone;

                    ((CrystalDecisions.CrystalReports.Engine.TextObject)oRptCloseStationSummary.ReportDefinition.ReportObjects["txtHeader"]).Text = Configuration.CInfo.StoreName;
                    ((CrystalDecisions.CrystalReports.Engine.TextObject)oRptCloseStationSummary.ReportDefinition.ReportObjects["txtPharmacyAddress"]).Text = Configuration.CInfo.Address;
                    ((CrystalDecisions.CrystalReports.Engine.TextObject)oRptCloseStationSummary.ReportDefinition.ReportObjects["txtPharmacyCityStateZip"]).Text = Configuration.CInfo.City + ", " + Configuration.CInfo.State + " " + Configuration.CInfo.Zip;
                    ((CrystalDecisions.CrystalReports.Engine.TextObject)oRptCloseStationSummary.ReportDefinition.ReportObjects["txtTelephoneNo"]).Text = Configuration.CInfo.Telephone;

                    oRptCloseStationCurrDenom.Database.Tables[0].SetDataSource(odsCloseStationCurrDenom.Tables[0]);
                    oRptCloseStationSummary.Database.Tables[0].SetDataSource(dtStClose);
                    oRptCloseStationCurrDenom.SetParameterValue("CompanyLogoPath", strCompanyLogoPath);   //PRIMEPOS-2386 26-Feb-2021 JY Added
                    oRptCloseStationSummary.SetParameterValue("CompanyLogoPath", strCompanyLogoPath);   //PRIMEPOS-2386 26-Feb-2021 JY Added

                    if (Configuration.CInfo.PrintStCloseNo)
                    {
                        clsReports.setCRTextObjectText("txtCloseID", StationCloseId.ToString(), oRptCloseStationCurrDenom);
                        clsReports.setCRTextObjectText("txtCloseID", StationCloseId.ToString(), oRptCloseStationSummary);
                    }
                    clsReports.setCRTextObjectText("txtStationName", txtStationName.Text, oRptCloseStationCurrDenom);

                    oRptCloseStationSummary.SetParameterValue("Currency", Configuration.CInfo.CurrencySymbol.ToString());   //PRIMEPOS-2495 15-Mar-2018 JY Added
                    oRptCloseStationCurrDenom.SetParameterValue("Currency", Configuration.CInfo.CurrencySymbol.ToString()); //PRIMEPOS-2495 15-Mar-2018 JY Added
                    orptTransactionDetail1.SetParameterValue("Currency", Configuration.CInfo.CurrencySymbol.ToString());    //PRIMEPOS-2495 15-Mar-2018 JY Added

                    //List<ReportClass> lstReportClass = new List<ReportClass>();
                    lstReportClass.Add(oRptCloseStationSummary);
                    lstReportClass.Add(oRptCloseStationCurrDenom);
                    lstReportClass.Add(oEODDepartment);
                    lstReportClass.Add(orptTransactionDetail1); //PRIMEPOS-2495 14-Mar-2018 JY Added
                    if (!bCalledFromScheduler)  //PRIMEPOS-3042 22-Dec-2021 JY Added condition
                    {
                        new System.Threading.Thread(delegate ()
                        {
                            clsReports.EmailReport(lstReportClass, Configuration.CInfo.OwnersEmailId, "Close Station Report - Close #:" + StationCloseId.ToString(), "Close Station Report", "File");
                        }).Start();
                    }
                }
                //To print the closeStation Report in normal format withouth the currency denominations.
                else
                {
                    #region PRIMEPOS-2495 16-Mar-2018 JY Added
                    //PRIMEPOS-2983 02-Jul-2021 JY Modified
                    sql = "select (select ISNULL(sum(Totalvalue),0) from stationclosecash where stationcloseid=" + StationCloseId + ") as TotalCash," +
                    " case transtype when 'U-1' then 1 when 'U-2' then 1 when 'U-3' then 1 when 'U-4' then 1  when 'U-5' then 1 when 'U-6' then 1 when 'U-7' then 1 when 'U-C' then 1 when 'U-H' then 1 when 'U-B' then 1 when 'U-E' then 1 when 'PO' then - 1 else 2 end as GroupType," +
                    " ISNULL((select Pt.PayTypeDesc  from Paytype PT where SUBSTRING(scd.TransType, 3, len(scd.TransType)) = pt.PayTypeID),TransType) as PayTypeDesc," +
                    " scd.TransType, scd.TransCount, scd.TransAmount, ps.STATIONNAME, sch.UserID, sch.CloseDate, sch.StationCloseID, scd.DrawNo" +
                    " FROM stationcloseheader sch" +
                    " INNER JOIN stationclosedetail scd ON sch.stationcloseid = scd.stationcloseid" +
                    " INNER JOIN util_POSSet ps ON sch.stationid = ps.stationid" +
                    " WHERE sch.stationcloseid =" + StationCloseId;

                    ds = oSearch.SearchData(sql);
                    dtStClose = ofrmEOD.setGroupType(ds.Tables[0]);
                    #endregion

                    oRptCloseStation.Database.Tables[0].SetDataSource(dtStClose);
                    oRptCloseStation.SetParameterValue("CompanyLogoPath", strCompanyLogoPath);   //PRIMEPOS-2386 26-Feb-2021 JY Added
                    ((CrystalDecisions.CrystalReports.Engine.TextObject)oRptCloseStation.ReportDefinition.ReportObjects["txtHeader"]).Text = Configuration.CInfo.StoreName;
                    ((CrystalDecisions.CrystalReports.Engine.TextObject)oRptCloseStation.ReportDefinition.ReportObjects["txtPharmacyAddress"]).Text = Configuration.CInfo.Address;
                    ((CrystalDecisions.CrystalReports.Engine.TextObject)oRptCloseStation.ReportDefinition.ReportObjects["txtPharmacyCityStateZip"]).Text = Configuration.CInfo.City + ", " + Configuration.CInfo.State + " " + Configuration.CInfo.Zip;
                    ((CrystalDecisions.CrystalReports.Engine.TextObject)oRptCloseStation.ReportDefinition.ReportObjects["txtTelephoneNo"]).Text = Configuration.CInfo.Telephone;

                    if (Configuration.CInfo.PrintStCloseNo)
                        clsReports.setCRTextObjectText("txtCloseID", StationCloseId.ToString(), oRptCloseStation);  //PRIMEPOS-2495 15-Mar-2018 JY Added to resolve email issue

                    oRptCloseStation.SetParameterValue("Currency", Configuration.CInfo.CurrencySymbol.ToString());
                    orptTransactionDetail1.SetParameterValue("Currency", Configuration.CInfo.CurrencySymbol.ToString());    //PRIMEPOS-2495 14-Mar-2018 JY Added
                    //List<ReportClass> lstReportClass = new List<ReportClass>();
                    lstReportClass.Add(oRptCloseStation);
                    lstReportClass.Add(oEODDepartment);
                    lstReportClass.Add(orptTransactionDetail1); //PRIMEPOS-2495 14-Mar-2018 JY Added
                    if (!bCalledFromScheduler)  //PRIMEPOS-3042 22-Dec-2021 JY Added condition
                    {
                        new System.Threading.Thread(delegate ()
                        {
                            clsReports.EmailReport(lstReportClass, Configuration.CInfo.OwnersEmailId, "Close Station Report - Close #:" + StationCloseId.ToString(), "Close Station Report", "File");
                        }).Start();
                    }
                }
            }
            catch (Exception exp)
            {
                if (!bCalledFromScheduler)
                    clsUIHelper.ShowErrorMsg(exp.Message);
                else
                    logger.Fatal(exp, "EmailReport(...)");
            }
            return lstReportClass;  //PRIMEPOS-3042 22-Dec-2021 JY Added
        }

        #region PRIMEPOS-2494 27-Feb-2018 JY Added
        private void PopulateTransDetail(string StationCloseNo)
        {
            try
            {
                SearchSvr oSearchSvr = new SearchSvr();
                DataSet oDataSet = new DataSet();

                string strTransSQL = "SELECT DISTINCT PT.TransID, PT.UserID, PT.TransDate, Case PT.TransType when 1 Then 'Sale' when 2 Then 'Return' WHEN 3 THEN 'ROA' end as TransType,  PT.StationID, ps.StationName " +
                            " FROM postransaction PT " +
                            " INNER JOIN util_POSSet ps ON ps.stationid = PT.stationid " +
                            " WHERE PT.StClosedID = " + StationCloseNo + " Order By PT.TransID";

                string strTransDetailsSQL = "select PT.TransID, I.ItemID, PTD.Qty, PTD.Discount, PTD.ItemDescription as Description, PTD.Price, PTD.TaxAmount, PTD.ExtendedPrice " +
                            " from postransaction PT " +
                            " INNER JOIN POSTransactionDetail PTD ON PT.TransID=PTD.TransID " +
                            " INNER JOIN Item I ON I.ItemID=PTD.ItemID " +
                            " WHERE PT.StClosedID = " + StationCloseNo + " Order By PTD.ItemDescription";

                string strPaymentSQL = "select PT.TransID, PTY.PayTypeDesc as [Payment Type], PTP.Amount, case CHARINDEX('|',isnull(PTP.refno,'')) " +
                            " when 0 then PTP.refno else '******'+right(rtrim(left(PTP.refno,CHARINDEX('|',PTP.refno)-1)) ,4) End as [Ref No.], PTP.CustomerSign, PTP.BinarySign, PTP.SigType " +
                            " from POSTransaction PT " +
                            " INNER JOIN postranspayment PTP ON PT.TransID = PTP.TransID " +
                            " INNER JOIN PayType PTY ON PTP.TransTypeCode = PTY.PayTypeID " +
                            " INNER JOIN Customer Cus ON PT.CustomerID = Cus.CustomerID " +
                            " WHERE PT.StClosedID = " + StationCloseNo + " Order By PTY.PayTypeDesc";

                oDataSet.Tables.Add(oSearchSvr.Search(strTransSQL).Tables[0].Copy());
                oDataSet.Tables[0].TableName = "Master";

                oDataSet.Tables.Add(oSearchSvr.Search(strTransDetailsSQL).Tables[0].Copy());
                oDataSet.Tables[1].TableName = "Detail";

                oDataSet.Tables.Add(oSearchSvr.Search(strPaymentSQL).Tables[0].Copy());
                oDataSet.Tables[2].TableName = "Payment";

                oDataSet.Relations.Add("MasterDetail", oDataSet.Tables[0].Columns["TransID"], oDataSet.Tables[1].Columns["TransID"]);
                oDataSet.Relations.Add("MasterPayment", oDataSet.Tables[0].Columns["TransID"], oDataSet.Tables[2].Columns["TransID"]);

                grdTransDetail.DataSource = oDataSet;

                grdTransDetail.DisplayLayout.Bands[0].HeaderVisible = true;
                grdTransDetail.DisplayLayout.Bands[0].Header.Appearance.FontData.SizeInPoints = 10;
                grdTransDetail.DisplayLayout.Bands[0].Header.Appearance.TextHAlign = Infragistics.Win.HAlign.Center;
                grdTransDetail.DisplayLayout.Bands[0].Header.Caption = "Transactions";
                grdTransDetail.DisplayLayout.Bands[0].Columns["StationID"].Hidden = true;
                grdTransDetail.DisplayLayout.Bands[0].Columns["UserID"].Header.Caption = "User";
                grdTransDetail.DisplayLayout.Bands[0].Columns["TransID"].SortIndicator = SortIndicator.Ascending;   //PRIMEPOS-2034 13-Mar-2018 JY Added to sort records

                grdTransDetail.DisplayLayout.Bands[1].HeaderVisible = true;
                grdTransDetail.DisplayLayout.Bands[1].Header.Caption = "Transaction Detail";
                grdTransDetail.DisplayLayout.Bands[1].Header.Appearance.FontData.SizeInPoints = 10;
                grdTransDetail.DisplayLayout.Bands[1].Header.Appearance.TextHAlign = Infragistics.Win.HAlign.Center;
                grdTransDetail.DisplayLayout.Bands[1].Columns["TransID"].Hidden = true;
                grdTransDetail.DisplayLayout.Bands[1].Expandable = true;

                grdTransDetail.DisplayLayout.Bands[2].HeaderVisible = true;
                grdTransDetail.DisplayLayout.Bands[2].Header.Caption = "Payment Information";
                grdTransDetail.DisplayLayout.Bands[2].Header.Appearance.FontData.SizeInPoints = 10;
                grdTransDetail.DisplayLayout.Bands[2].Header.Appearance.TextHAlign = Infragistics.Win.HAlign.Center;
                grdTransDetail.DisplayLayout.Bands[2].Columns["TransID"].Hidden = true;
                grdTransDetail.DisplayLayout.Bands[2].Columns["CustomerSign"].Hidden = true;
                grdTransDetail.DisplayLayout.Bands[2].Columns["BinarySign"].Hidden = true;
                grdTransDetail.DisplayLayout.Bands[2].Columns["SigType"].Hidden = true;


                if (grdTransDetail.DisplayLayout.Bands[2].Columns.Exists("ViewSign") == false)
                {
                    Infragistics.Win.UltraWinGrid.UltraGridColumn oCol = grdTransDetail.DisplayLayout.Bands[2].Columns.Add("ViewSign");
                    oCol.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.Button;
                    oCol.Header.Caption = "View Sign";
                }

                this.resizeColumns(grdTransDetail);
                grdTransDetail.Focus();
                grdTransDetail.PerformAction(UltraGridAction.FirstRowInGrid);
                grdTransDetail.Refresh();

                #region PRIMEPOS-2480 06-Jul-2020 JY Added
                string strSQL = "SELECT ISNULL(MIN(PT.TransID),0) AS MinTransID, ISNULL(MAX(PT.TransID),0) AS MaxTransID FROM postransaction PT WHERE PT.StClosedID = " + StationCloseNo;
                DataSet ds = oSearchSvr.Search(strSQL);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    lblTransRange.Text = "Transaction# from " + ds.Tables[0].Rows[0]["MinTransID"].ToString() + " to " + ds.Tables[0].Rows[0]["MaxTransID"].ToString();
                }
                #endregion
            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }
        #endregion

        #region PRIMEPOS-2495 20-Mar-2018 JY Added
        private void btnPrintTransactionDetails_Click(object sender, EventArgs e)
        {
            rptTransactionDetail1 orptTransactionDetail1 = new rptTransactionDetail1();
            Int32 id = Configuration.convertNullToInt(txtStationCloseNo.Text);
            DataSet dsTransactionDetail = GetTransactionDetails(id);
            orptTransactionDetail1.SetDataSource(dsTransactionDetail.Tables[0]);
            orptTransactionDetail1.SetParameterValue("CompanyLogoPath", strCompanyLogoPath);   //PRIMEPOS-2386 26-Feb-2021 JY Added

            #region PRIMEPOS-2480 26-Jun-2020 JY Added
            try
            {
                ((CrystalDecisions.CrystalReports.Engine.TextObject)orptTransactionDetail1.ReportDefinition.ReportObjects["txtCloseDate"]).Text = "Close Date: " + Convert.ToDateTime(dsTransactionDetail.Tables[0].Rows[0]["CloseDate"]).ToString("MM/dd/yyyy HH:mm tt");  //PRIMEPOS-3114 10-Aug-2022 JY modified
            }
            catch
            {
                ((CrystalDecisions.CrystalReports.Engine.TextObject)orptTransactionDetail1.ReportDefinition.ReportObjects["txtCloseDate"]).Text = " ";
            }
            if (Configuration.CInfo.PrintStCloseNo == false)
                ((CrystalDecisions.CrystalReports.Engine.TextObject)orptTransactionDetail1.ReportDefinition.ReportObjects["txtId"]).Text = " ";
            else
                ((CrystalDecisions.CrystalReports.Engine.TextObject)orptTransactionDetail1.ReportDefinition.ReportObjects["txtId"]).Text = "Station Close #: " + id.ToString();
            #endregion

            ((CrystalDecisions.CrystalReports.Engine.TextObject)orptTransactionDetail1.ReportDefinition.ReportObjects["txtHeader"]).Text = Configuration.CInfo.StoreName;
            ((CrystalDecisions.CrystalReports.Engine.TextObject)orptTransactionDetail1.ReportDefinition.ReportObjects["txtPharmacyAddress"]).Text = Configuration.CInfo.Address;
            ((CrystalDecisions.CrystalReports.Engine.TextObject)orptTransactionDetail1.ReportDefinition.ReportObjects["txtPharmacyCityStateZip"]).Text = Configuration.CInfo.City + ", " + Configuration.CInfo.State + " " + Configuration.CInfo.Zip;
            ((CrystalDecisions.CrystalReports.Engine.TextObject)orptTransactionDetail1.ReportDefinition.ReportObjects["txtTelephoneNo"]).Text = Configuration.CInfo.Telephone;

            POS_Core_UI.Reports.ReportsUI.clsReports.PrintReport(orptTransactionDetail1);
        }

        private DataSet GetTransactionDetails(int StationCloseId)
        {
            //PRIMEPOS-2386 26-Feb-2021 JY modified
            string strSQL = "SELECT a.TransID, a.TransDate, Case a.TransType when 1 Then 'Sale' when 2 Then 'Return' WHEN 3 THEN 'ROA' end as TransType, a.UserID, a.StationID, a.TotalPaid, a.StClosedID, a.EODID, b.CloseDate, b.UserID As Users"
                            + " FROM POSTransaction a INNER JOIN StationCloseHeader b ON a.StClosedID = b.StationCloseID"
                            + " WHERE a.StClosedID = " + StationCloseId + " ORDER BY a.TransID";
            DataSet ds = clsReports.GetReportSource(strSQL);
            return ds;
        }
        #endregion

        #region PRIMEPOS-3042 22-Dec-2021 JY Added
        public usrDateRangeParams customControl;
        private bool bCalledFromScheduler = false;
        private string strMessage = string.Empty;

        public bool CheckTags()
        {
            return true;
        }

        public bool SaveTaskParameters(DataTable dt, int ScheduledTasksID)
        {
            try
            {
                ScheduledTasks oScheduledTasks = new ScheduledTasks();
                oScheduledTasks.SaveTaskParameters(dt, ScheduledTasksID);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool SetControlParameters(int ScheduledTasksID)
        {
            ScheduledTasks oScheduledTasks = new ScheduledTasks();
            DataTable dt = oScheduledTasks.GetScheduledTasksControlsList(ScheduledTasksID);
            customControl.setControlsValues(ref dt);
            return true;
        }

        public bool RunTask(int TaskId, ref string filePath, bool bsendToPrint, ref string sNoOfRecordAffect)
        {
            try
            {
                SetControlParameters(TaskId);
                bCalledFromScheduler = true;
                bsendToPrint = false;
                bool bStatus = CloseAllStaitons(ref strMessage, ref filePath);
                sNoOfRecordAffect = strMessage;
            }
            catch (Exception Ex)
            { }
            finally
            {
                bCalledFromScheduler = false;
                strMessage = string.Empty;
            }
            return true;
        }

        public bool CloseAllStaitons(ref string strErrorMessage, ref string filePath)
        {
            bool bStatus = true;            
            EndOFDay oEndOFDay = new EndOFDay();
            try
            {
                string[] sOpenStations = oEndOFDay.CheckIfAllStationClosed();
                if (sOpenStations.Length > 0)
                {
                    for (int i = 0; i < sOpenStations.Length; i++)
                    {
                        string strErrMsg = string.Empty;
                        int StationCloseID = CloseStation(sOpenStations[i], ref strErrMsg);
                        if (strErrorMessage == "")
                            strErrorMessage = strErrMsg;
                        else
                            strErrorMessage += ", " + strErrMsg;
                        if (filePath == "")
                            filePath = StationCloseID.ToString();
                        else
                            filePath += "," + StationCloseID.ToString();
                    }
                }
                else
                {
                    strMessage = ErrorHandler.getCustomMessageFromDb((long)POSErrorENUM.StationClose_NoTransactionFoundForCloseStation);
                }
            }
            catch (Exception ex)
            {
            }
            return bStatus;
        }

        private int CloseStation(string strStationID, ref string strErrMsg)
        {
            int StationCloseID = oStationClose.CloseStation(strStationID, ref strErrMsg);
            return StationCloseID;
        }


        public void GetTaskParameters(ref DataTable dt, int TaskId)
        {
            customControl.getControlsValues(ref dt);
        }

        public Control GetParameterControl()
        {
            customControl.setDateTimeControl();
            customControl.Dock = DockStyle.Fill;
            return customControl;
        }

        public bool checkValidation()
        {
            return true;
        }
        #endregion
    }
}