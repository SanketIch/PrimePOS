using Infragistics.Win.UltraWinGrid;
using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using POS_Core.BusinessRules;
using POS_Core.CommonData;
using System.Data;
using POS_Core_UI.Reports.Reports;
//using POS_Core.DataAccess;
using CrystalDecisions.Shared;
using CrystalDecisions.ReportSource;
using CrystalDecisions.CrystalReports.Engine;
using System.Collections.Generic;
//using POS_Core_UI.Reports.ReportsUI;
using POS_Core.DataAccess;
using POS_Core_UI.Reports.ReportsUI;
using POS_Core.Resources;
using POS_Core.LabelHandler;
using POS_Core.LabelHandler.RxLabel;
using NLog;

namespace POS_Core_UI
{
    /// <summary>
    /// Summary description for frmEndOFDay.
    /// </summary>
    public class frmEndOfDay : System.Windows.Forms.Form, ICommandLIneTaskControl
    {
        private EndOFDayData oEndOFDayData;
        private EndOFDay oEndOFDay = new EndOFDay();
        private Infragistics.Win.Misc.UltraLabel ultraLabel10;
        private Infragistics.Win.Misc.UltraButton cmdProcess;
        private Infragistics.Win.Misc.UltraButton cmdClose;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor txtEndOfDayNo;
        private Infragistics.Win.UltraWinTabControl.UltraTabControl ultraTabControl1;
        private Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage ultraTabSharedControlsPage1;
        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl1;
        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl2;
        private System.Windows.Forms.ColumnHeader colPayType;
        private System.Windows.Forms.ColumnHeader colAmount;
        private Infragistics.Win.UltraWinEditors.UltraNumericEditor numTotal;
        private Infragistics.Win.UltraWinEditors.UltraNumericEditor numGrandTotal;
        private Infragistics.Win.UltraWinEditors.UltraNumericEditor numReceiveOnAccount;
        private Infragistics.Win.UltraWinEditors.UltraNumericEditor numTotalCash;
        private Infragistics.Win.UltraWinEditors.UltraNumericEditor numTotalDiscount;
        private Infragistics.Win.UltraWinEditors.UltraNumericEditor numTotalReturn;
        private Infragistics.Win.UltraWinEditors.UltraNumericEditor numTotalSale;
        private System.Windows.Forms.ListView lvPayType;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private Infragistics.Win.Misc.UltraLabel ultraLabel14;
        private Infragistics.Win.Misc.UltraLabel ultraLabel8;
        private Infragistics.Win.Misc.UltraLabel ultraLabel7;
        private Infragistics.Win.Misc.UltraLabel ultraLabel6;
        private Infragistics.Win.Misc.UltraLabel ultraLabel5;
        private Infragistics.Win.Misc.UltraLabel ultraLabel4;
        private Infragistics.Win.Misc.UltraLabel ultraLabel3;
        private Infragistics.Win.Misc.UltraLabel ultraLabel2;
        private Infragistics.Win.Misc.UltraLabel ultraLabel1;
        private Infragistics.Win.UltraWinEditors.UltraNumericEditor numNetSale;
        private Infragistics.Win.UltraWinEditors.UltraNumericEditor numSalesTax;
        private Infragistics.Win.Misc.UltraLabel lblTransactionType;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.GroupBox groupBox4;
        private Infragistics.Win.UltraWinGrid.UltraGrid grdDepartments;
        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl3;
        private UltraGrid grdStations;
        private Infragistics.Win.Misc.UltraLabel ultraLabel17;
        private Infragistics.Win.Misc.UltraLabel ultraLabel16;
        private Infragistics.Win.Misc.UltraLabel ultraLabel15;
        private Infragistics.Win.UltraWinEditors.UltraNumericEditor numPayOut;
        private Infragistics.Win.UltraWinEditors.UltraNumericEditor numNetCash;
        private Infragistics.Win.UltraWinEditors.UltraNumericEditor numTotalCashU;
        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl4;
        private UltraGrid grdTransDetail;
        private Infragistics.Win.Misc.UltraButton btnPrintTransactionDetails;

        public Infragistics.Win.UltraWinEditors.UltraTextEditor txtEODDate;
        private Infragistics.Win.Misc.UltraLabel lblEODDate;

        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;
        private Panel pnlTop;
        private Infragistics.Win.Misc.UltraLabel lblTransRange;
        private bool bEditMode = false;
        private string strCompanyLogoPath = "";   //PRIMEPOS-2386 01-Mar-2021 JY Added
        private Infragistics.Win.Misc.UltraLabel lblTransFee;
        private Infragistics.Win.UltraWinEditors.UltraNumericEditor numTransFee;
        private ILogger logger = LogManager.GetCurrentClassLogger();

        public frmEndOfDay()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();
            //
            // TODO: Add any constructor code after InitializeComponent call
            //
            this.customControl = new usrDateRangeParams();  //PRIMEPOS-3039 16-Dec-2021 JY Added
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
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand1 = new Infragistics.Win.UltraWinGrid.UltraGridBand("Band 0", -1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn5 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("ColumnName");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn6 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Type", -1, null, 0, Infragistics.Win.UltraWinGrid.SortIndicator.Ascending, false);
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
            Infragistics.Win.Appearance appearance52 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance53 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance54 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance55 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinScrollBar.ScrollBarLook scrollBarLook1 = new Infragistics.Win.UltraWinScrollBar.ScrollBarLook();
            Infragistics.Win.Appearance appearance56 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance57 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand2 = new Infragistics.Win.UltraWinGrid.UltraGridBand("Band 0", -1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn7 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("ColumnName");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn4 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Type");
            Infragistics.Win.Appearance appearance58 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance59 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance60 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance61 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance62 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance63 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance64 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance65 = new Infragistics.Win.Appearance();
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
            Infragistics.Win.UltraWinScrollBar.ScrollBarLook scrollBarLook2 = new Infragistics.Win.UltraWinScrollBar.ScrollBarLook();
            Infragistics.Win.Appearance appearance77 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance78 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand3 = new Infragistics.Win.UltraWinGrid.UltraGridBand("Band 0", -1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn1 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Column Name");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn2 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Type");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn3 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Criteria Value");
            Infragistics.Win.Appearance appearance79 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance80 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance81 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance82 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance83 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance84 = new Infragistics.Win.Appearance();
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
            Infragistics.Win.UltraWinScrollBar.ScrollBarLook scrollBarLook3 = new Infragistics.Win.UltraWinScrollBar.ScrollBarLook();
            Infragistics.Win.Appearance appearance98 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance99 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance100 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance101 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance102 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance103 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance104 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance105 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance106 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance107 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab1 = new Infragistics.Win.UltraWinTabControl.UltraTab();
            Infragistics.Win.Appearance appearance108 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab2 = new Infragistics.Win.UltraWinTabControl.UltraTab();
            Infragistics.Win.Appearance appearance109 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab3 = new Infragistics.Win.UltraWinTabControl.UltraTab();
            Infragistics.Win.Appearance appearance110 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab4 = new Infragistics.Win.UltraWinTabControl.UltraTab();
            Infragistics.Win.Appearance appearance111 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance112 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance113 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance114 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance115 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance9 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance10 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance11 = new Infragistics.Win.Appearance();
            this.ultraTabPageControl1 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.ultraLabel17 = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabel16 = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabel15 = new Infragistics.Win.Misc.UltraLabel();
            this.numPayOut = new Infragistics.Win.UltraWinEditors.UltraNumericEditor();
            this.numNetCash = new Infragistics.Win.UltraWinEditors.UltraNumericEditor();
            this.numTotalCashU = new Infragistics.Win.UltraWinEditors.UltraNumericEditor();
            this.ultraLabel14 = new Infragistics.Win.Misc.UltraLabel();
            this.numTotal = new Infragistics.Win.UltraWinEditors.UltraNumericEditor();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.ultraLabel8 = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabel7 = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabel6 = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabel5 = new Infragistics.Win.Misc.UltraLabel();
            this.numGrandTotal = new Infragistics.Win.UltraWinEditors.UltraNumericEditor();
            this.numReceiveOnAccount = new Infragistics.Win.UltraWinEditors.UltraNumericEditor();
            this.numTotalCash = new Infragistics.Win.UltraWinEditors.UltraNumericEditor();
            this.numTotalDiscount = new Infragistics.Win.UltraWinEditors.UltraNumericEditor();
            this.numTotalReturn = new Infragistics.Win.UltraWinEditors.UltraNumericEditor();
            this.ultraLabel4 = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabel3 = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabel2 = new Infragistics.Win.Misc.UltraLabel();
            this.numTotalSale = new Infragistics.Win.UltraWinEditors.UltraNumericEditor();
            this.ultraLabel1 = new Infragistics.Win.Misc.UltraLabel();
            this.numNetSale = new Infragistics.Win.UltraWinEditors.UltraNumericEditor();
            this.numSalesTax = new Infragistics.Win.UltraWinEditors.UltraNumericEditor();
            this.lvPayType = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ultraTabPageControl2 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.grdDepartments = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.ultraTabPageControl3 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.grdStations = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.ultraTabPageControl4 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.grdTransDetail = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.pnlTop = new System.Windows.Forms.Panel();
            this.lblTransRange = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabel10 = new Infragistics.Win.Misc.UltraLabel();
            this.cmdProcess = new Infragistics.Win.Misc.UltraButton();
            this.cmdClose = new Infragistics.Win.Misc.UltraButton();
            this.txtEndOfDayNo = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.ultraTabControl1 = new Infragistics.Win.UltraWinTabControl.UltraTabControl();
            this.ultraTabSharedControlsPage1 = new Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage();
            this.colPayType = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colAmount = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.lblTransactionType = new Infragistics.Win.Misc.UltraLabel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtEODDate = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.lblEODDate = new Infragistics.Win.Misc.UltraLabel();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.btnPrintTransactionDetails = new Infragistics.Win.Misc.UltraButton();
            this.lblTransFee = new Infragistics.Win.Misc.UltraLabel();
            this.numTransFee = new Infragistics.Win.UltraWinEditors.UltraNumericEditor();
            this.ultraTabPageControl1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numPayOut)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numNetCash)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numTotalCashU)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numTotal)).BeginInit();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numGrandTotal)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numReceiveOnAccount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numTotalCash)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numTotalDiscount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numTotalReturn)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numTotalSale)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numNetSale)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numSalesTax)).BeginInit();
            this.ultraTabPageControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdDepartments)).BeginInit();
            this.ultraTabPageControl3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdStations)).BeginInit();
            this.ultraTabPageControl4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdTransDetail)).BeginInit();
            this.pnlTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtEndOfDayNo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraTabControl1)).BeginInit();
            this.ultraTabControl1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtEODDate)).BeginInit();
            this.groupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numTransFee)).BeginInit();
            this.SuspendLayout();
            // 
            // ultraTabPageControl1
            // 
            this.ultraTabPageControl1.Controls.Add(this.groupBox3);
            this.ultraTabPageControl1.Controls.Add(this.groupBox2);
            this.ultraTabPageControl1.Controls.Add(this.lvPayType);
            this.ultraTabPageControl1.Location = new System.Drawing.Point(2, 23);
            this.ultraTabPageControl1.Name = "ultraTabPageControl1";
            this.ultraTabPageControl1.Size = new System.Drawing.Size(754, 383);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.ultraLabel17);
            this.groupBox3.Controls.Add(this.ultraLabel16);
            this.groupBox3.Controls.Add(this.ultraLabel15);
            this.groupBox3.Controls.Add(this.numPayOut);
            this.groupBox3.Controls.Add(this.numNetCash);
            this.groupBox3.Controls.Add(this.numTotalCashU);
            this.groupBox3.Controls.Add(this.ultraLabel14);
            this.groupBox3.Controls.Add(this.numTotal);
            this.groupBox3.Location = new System.Drawing.Point(10, 255);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(737, 124);
            this.groupBox3.TabIndex = 60;
            this.groupBox3.TabStop = false;
            // 
            // ultraLabel17
            // 
            appearance1.ForeColor = System.Drawing.Color.White;
            appearance1.TextVAlignAsString = "Middle";
            this.ultraLabel17.Appearance = appearance1;
            this.ultraLabel17.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultraLabel17.Location = new System.Drawing.Point(506, 96);
            this.ultraLabel17.Name = "ultraLabel17";
            this.ultraLabel17.Size = new System.Drawing.Size(102, 18);
            this.ultraLabel17.TabIndex = 61;
            this.ultraLabel17.Text = "Net Cash";
            // 
            // ultraLabel16
            // 
            appearance2.ForeColor = System.Drawing.Color.White;
            appearance2.TextVAlignAsString = "Middle";
            this.ultraLabel16.Appearance = appearance2;
            this.ultraLabel16.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultraLabel16.Location = new System.Drawing.Point(506, 71);
            this.ultraLabel16.Name = "ultraLabel16";
            this.ultraLabel16.Size = new System.Drawing.Size(102, 18);
            this.ultraLabel16.TabIndex = 60;
            this.ultraLabel16.Text = "Payout";
            // 
            // ultraLabel15
            // 
            appearance3.ForeColor = System.Drawing.Color.White;
            appearance3.TextVAlignAsString = "Middle";
            this.ultraLabel15.Appearance = appearance3;
            this.ultraLabel15.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultraLabel15.Location = new System.Drawing.Point(506, 46);
            this.ultraLabel15.Name = "ultraLabel15";
            this.ultraLabel15.Size = new System.Drawing.Size(102, 18);
            this.ultraLabel15.TabIndex = 59;
            this.ultraLabel15.Text = "Total Cash";
            // 
            // numPayOut
            // 
            appearance4.BackColor = System.Drawing.Color.White;
            appearance4.ForeColor = System.Drawing.Color.Black;
            this.numPayOut.Appearance = appearance4;
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
            this.numPayOut.TabIndex = 63;
            this.numPayOut.TabStop = false;
            // 
            // numNetCash
            // 
            appearance5.BackColor = System.Drawing.Color.RoyalBlue;
            appearance5.FontData.SizeInPoints = 11F;
            appearance5.ForeColor = System.Drawing.Color.White;
            this.numNetCash.Appearance = appearance5;
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
            this.numNetCash.TabIndex = 64;
            this.numNetCash.TabStop = false;
            // 
            // numTotalCashU
            // 
            appearance6.BackColor = System.Drawing.Color.LightYellow;
            appearance6.BorderColor = System.Drawing.Color.Navy;
            appearance6.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.numTotalCashU.Appearance = appearance6;
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
            this.numTotalCashU.TabIndex = 62;
            this.numTotalCashU.TabStop = false;
            // 
            // ultraLabel14
            // 
            appearance7.ForeColor = System.Drawing.Color.White;
            appearance7.TextVAlignAsString = "Middle";
            this.ultraLabel14.Appearance = appearance7;
            this.ultraLabel14.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
            this.ultraLabel14.Location = new System.Drawing.Point(506, 21);
            this.ultraLabel14.Name = "ultraLabel14";
            this.ultraLabel14.Size = new System.Drawing.Size(102, 18);
            this.ultraLabel14.TabIndex = 45;
            this.ultraLabel14.Text = "Total Payment";
            this.ultraLabel14.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // numTotal
            // 
            appearance8.BackColor = System.Drawing.Color.RoyalBlue;
            appearance8.FontData.SizeInPoints = 11F;
            appearance8.ForeColor = System.Drawing.Color.White;
            this.numTotal.Appearance = appearance8;
            this.numTotal.AutoSize = false;
            this.numTotal.BackColor = System.Drawing.Color.RoyalBlue;
            this.numTotal.BorderStyle = Infragistics.Win.UIElementBorderStyle.None;
            this.numTotal.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
            this.numTotal.Location = new System.Drawing.Point(614, 20);
            this.numTotal.MaskInput = "{LOC}-nnnnnnnnnn.nn";
            this.numTotal.Name = "numTotal";
            this.numTotal.NumericType = Infragistics.Win.UltraWinEditors.NumericType.Double;
            this.numTotal.PromptChar = ' ';
            this.numTotal.ReadOnly = true;
            this.numTotal.Size = new System.Drawing.Size(114, 20);
            this.numTotal.TabIndex = 58;
            this.numTotal.TabStop = false;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.lblTransFee);
            this.groupBox2.Controls.Add(this.numTransFee);
            this.groupBox2.Controls.Add(this.ultraLabel8);
            this.groupBox2.Controls.Add(this.ultraLabel7);
            this.groupBox2.Controls.Add(this.ultraLabel6);
            this.groupBox2.Controls.Add(this.ultraLabel5);
            this.groupBox2.Controls.Add(this.numGrandTotal);
            this.groupBox2.Controls.Add(this.numReceiveOnAccount);
            this.groupBox2.Controls.Add(this.numTotalCash);
            this.groupBox2.Controls.Add(this.numTotalDiscount);
            this.groupBox2.Controls.Add(this.numTotalReturn);
            this.groupBox2.Controls.Add(this.ultraLabel4);
            this.groupBox2.Controls.Add(this.ultraLabel3);
            this.groupBox2.Controls.Add(this.ultraLabel2);
            this.groupBox2.Controls.Add(this.numTotalSale);
            this.groupBox2.Controls.Add(this.ultraLabel1);
            this.groupBox2.Controls.Add(this.numNetSale);
            this.groupBox2.Controls.Add(this.numSalesTax);
            this.groupBox2.Location = new System.Drawing.Point(10, 5);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(284, 248);
            this.groupBox2.TabIndex = 59;
            this.groupBox2.TabStop = false;
            // 
            // ultraLabel8
            // 
            appearance12.FontData.BoldAsString = "True";
            appearance12.FontData.Name = "Arial";
            appearance12.ForeColor = System.Drawing.Color.White;
            appearance12.TextVAlignAsString = "Middle";
            this.ultraLabel8.Appearance = appearance12;
            this.ultraLabel8.BackColorInternal = System.Drawing.Color.Transparent;
            this.ultraLabel8.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
            this.ultraLabel8.Location = new System.Drawing.Point(10, 221);
            this.ultraLabel8.Name = "ultraLabel8";
            this.ultraLabel8.Size = new System.Drawing.Size(140, 18);
            this.ultraLabel8.TabIndex = 44;
            this.ultraLabel8.Text = "Grand Total";
            this.ultraLabel8.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // ultraLabel7
            // 
            appearance13.FontData.BoldAsString = "True";
            appearance13.FontData.Name = "Arial";
            appearance13.ForeColor = System.Drawing.Color.White;
            appearance13.TextVAlignAsString = "Middle";
            this.ultraLabel7.Appearance = appearance13;
            this.ultraLabel7.BackColorInternal = System.Drawing.Color.Transparent;
            this.ultraLabel7.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
            this.ultraLabel7.Location = new System.Drawing.Point(10, 171);
            this.ultraLabel7.Name = "ultraLabel7";
            this.ultraLabel7.Size = new System.Drawing.Size(140, 18);
            this.ultraLabel7.TabIndex = 43;
            this.ultraLabel7.Text = "Receive On Account";
            this.ultraLabel7.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // ultraLabel6
            // 
            appearance14.FontData.BoldAsString = "True";
            appearance14.FontData.Name = "Arial";
            appearance14.ForeColor = System.Drawing.Color.White;
            appearance14.TextVAlignAsString = "Middle";
            this.ultraLabel6.Appearance = appearance14;
            this.ultraLabel6.BackColorInternal = System.Drawing.Color.Transparent;
            this.ultraLabel6.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
            this.ultraLabel6.Location = new System.Drawing.Point(10, 146);
            this.ultraLabel6.Name = "ultraLabel6";
            this.ultraLabel6.Size = new System.Drawing.Size(140, 18);
            this.ultraLabel6.TabIndex = 42;
            this.ultraLabel6.Text = "Total";
            this.ultraLabel6.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // ultraLabel5
            // 
            appearance15.FontData.BoldAsString = "True";
            appearance15.FontData.Name = "Arial";
            appearance15.ForeColor = System.Drawing.Color.White;
            appearance15.TextVAlignAsString = "Middle";
            this.ultraLabel5.Appearance = appearance15;
            this.ultraLabel5.BackColorInternal = System.Drawing.Color.Transparent;
            this.ultraLabel5.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
            this.ultraLabel5.Location = new System.Drawing.Point(10, 121);
            this.ultraLabel5.Name = "ultraLabel5";
            this.ultraLabel5.Size = new System.Drawing.Size(140, 18);
            this.ultraLabel5.TabIndex = 41;
            this.ultraLabel5.Text = "Sales Tax";
            this.ultraLabel5.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // numGrandTotal
            // 
            appearance16.BackColor = System.Drawing.Color.RoyalBlue;
            appearance16.FontData.SizeInPoints = 11F;
            appearance16.ForeColor = System.Drawing.Color.White;
            this.numGrandTotal.Appearance = appearance16;
            this.numGrandTotal.AutoSize = false;
            this.numGrandTotal.BackColor = System.Drawing.Color.RoyalBlue;
            this.numGrandTotal.BorderStyle = Infragistics.Win.UIElementBorderStyle.None;
            appearance17.BackColor = System.Drawing.Color.Transparent;
            appearance17.BackColor2 = System.Drawing.Color.Transparent;
            this.numGrandTotal.ButtonAppearance = appearance17;
            this.numGrandTotal.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
            this.numGrandTotal.Location = new System.Drawing.Point(160, 220);
            this.numGrandTotal.MaskInput = "{LOC}-nnnnnnnnnn.nn";
            this.numGrandTotal.Name = "numGrandTotal";
            this.numGrandTotal.NumericType = Infragistics.Win.UltraWinEditors.NumericType.Double;
            this.numGrandTotal.PromptChar = ' ';
            this.numGrandTotal.ReadOnly = true;
            this.numGrandTotal.Size = new System.Drawing.Size(114, 20);
            this.numGrandTotal.TabIndex = 57;
            this.numGrandTotal.TabStop = false;
            // 
            // numReceiveOnAccount
            // 
            appearance18.BackColor = System.Drawing.Color.White;
            appearance18.BackColorDisabled = System.Drawing.Color.Transparent;
            appearance18.FontData.BoldAsString = "True";
            appearance18.FontData.Name = "Arial";
            appearance18.ForeColor = System.Drawing.Color.Black;
            this.numReceiveOnAccount.Appearance = appearance18;
            this.numReceiveOnAccount.BackColor = System.Drawing.Color.White;
            this.numReceiveOnAccount.BorderStyle = Infragistics.Win.UIElementBorderStyle.None;
            appearance19.BackColor = System.Drawing.Color.Transparent;
            appearance19.BackColor2 = System.Drawing.Color.Transparent;
            this.numReceiveOnAccount.ButtonAppearance = appearance19;
            this.numReceiveOnAccount.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
            this.numReceiveOnAccount.Location = new System.Drawing.Point(160, 170);
            this.numReceiveOnAccount.MaskInput = "{LOC}-nnnnnnnnnn.nn";
            this.numReceiveOnAccount.Name = "numReceiveOnAccount";
            this.numReceiveOnAccount.NumericType = Infragistics.Win.UltraWinEditors.NumericType.Double;
            this.numReceiveOnAccount.PromptChar = ' ';
            this.numReceiveOnAccount.ReadOnly = true;
            this.numReceiveOnAccount.Size = new System.Drawing.Size(114, 20);
            this.numReceiveOnAccount.TabIndex = 56;
            this.numReceiveOnAccount.TabStop = false;
            // 
            // numTotalCash
            // 
            appearance20.BackColor = System.Drawing.Color.RoyalBlue;
            appearance20.FontData.SizeInPoints = 11F;
            appearance20.ForeColor = System.Drawing.Color.White;
            this.numTotalCash.Appearance = appearance20;
            this.numTotalCash.AutoSize = false;
            this.numTotalCash.BackColor = System.Drawing.Color.RoyalBlue;
            this.numTotalCash.BorderStyle = Infragistics.Win.UIElementBorderStyle.None;
            appearance21.BackColor = System.Drawing.Color.Transparent;
            appearance21.BackColor2 = System.Drawing.Color.Transparent;
            this.numTotalCash.ButtonAppearance = appearance21;
            this.numTotalCash.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
            this.numTotalCash.Location = new System.Drawing.Point(160, 145);
            this.numTotalCash.MaskInput = "{LOC}-nnnnnnnnnn.nn";
            this.numTotalCash.Name = "numTotalCash";
            this.numTotalCash.NumericType = Infragistics.Win.UltraWinEditors.NumericType.Double;
            this.numTotalCash.PromptChar = ' ';
            this.numTotalCash.ReadOnly = true;
            this.numTotalCash.Size = new System.Drawing.Size(114, 20);
            this.numTotalCash.TabIndex = 55;
            this.numTotalCash.TabStop = false;
            // 
            // numTotalDiscount
            // 
            appearance22.BackColor = System.Drawing.Color.LightYellow;
            appearance22.FontData.BoldAsString = "True";
            appearance22.FontData.Name = "Arial";
            appearance22.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.numTotalDiscount.Appearance = appearance22;
            this.numTotalDiscount.BackColor = System.Drawing.Color.LightYellow;
            this.numTotalDiscount.BorderStyle = Infragistics.Win.UIElementBorderStyle.None;
            appearance23.BackColor = System.Drawing.Color.Transparent;
            appearance23.BackColor2 = System.Drawing.Color.Transparent;
            this.numTotalDiscount.ButtonAppearance = appearance23;
            this.numTotalDiscount.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
            this.numTotalDiscount.FormatString = "";
            this.numTotalDiscount.Location = new System.Drawing.Point(160, 70);
            this.numTotalDiscount.MaskInput = "{LOC}-nnnnnnnnnn.nn";
            this.numTotalDiscount.Name = "numTotalDiscount";
            this.numTotalDiscount.NumericType = Infragistics.Win.UltraWinEditors.NumericType.Double;
            this.numTotalDiscount.PromptChar = ' ';
            this.numTotalDiscount.ReadOnly = true;
            this.numTotalDiscount.Size = new System.Drawing.Size(114, 20);
            this.numTotalDiscount.TabIndex = 52;
            this.numTotalDiscount.TabStop = false;
            // 
            // numTotalReturn
            // 
            appearance24.BackColor = System.Drawing.Color.LightYellow;
            appearance24.FontData.BoldAsString = "True";
            appearance24.FontData.Name = "Arial";
            appearance24.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.numTotalReturn.Appearance = appearance24;
            this.numTotalReturn.BackColor = System.Drawing.Color.LightYellow;
            this.numTotalReturn.BorderStyle = Infragistics.Win.UIElementBorderStyle.None;
            appearance25.BackColor = System.Drawing.Color.Transparent;
            appearance25.BackColor2 = System.Drawing.Color.Transparent;
            this.numTotalReturn.ButtonAppearance = appearance25;
            this.numTotalReturn.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
            this.numTotalReturn.FormatString = "";
            this.numTotalReturn.Location = new System.Drawing.Point(160, 45);
            this.numTotalReturn.MaskInput = "{LOC}-nnnnnnnnnn.nn";
            this.numTotalReturn.Name = "numTotalReturn";
            this.numTotalReturn.NumericType = Infragistics.Win.UltraWinEditors.NumericType.Double;
            this.numTotalReturn.PromptChar = ' ';
            this.numTotalReturn.ReadOnly = true;
            this.numTotalReturn.Size = new System.Drawing.Size(114, 20);
            this.numTotalReturn.TabIndex = 51;
            this.numTotalReturn.TabStop = false;
            // 
            // ultraLabel4
            // 
            appearance26.FontData.BoldAsString = "True";
            appearance26.FontData.Name = "Arial";
            appearance26.ForeColor = System.Drawing.Color.White;
            appearance26.TextVAlignAsString = "Middle";
            this.ultraLabel4.Appearance = appearance26;
            this.ultraLabel4.BackColorInternal = System.Drawing.Color.Transparent;
            this.ultraLabel4.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
            this.ultraLabel4.Location = new System.Drawing.Point(10, 96);
            this.ultraLabel4.Name = "ultraLabel4";
            this.ultraLabel4.Size = new System.Drawing.Size(140, 18);
            this.ultraLabel4.TabIndex = 40;
            this.ultraLabel4.Text = "Net Sale";
            this.ultraLabel4.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // ultraLabel3
            // 
            appearance27.FontData.BoldAsString = "True";
            appearance27.FontData.Name = "Arial";
            appearance27.ForeColor = System.Drawing.Color.White;
            appearance27.TextVAlignAsString = "Middle";
            this.ultraLabel3.Appearance = appearance27;
            this.ultraLabel3.BackColorInternal = System.Drawing.Color.Transparent;
            this.ultraLabel3.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
            this.ultraLabel3.Location = new System.Drawing.Point(10, 71);
            this.ultraLabel3.Name = "ultraLabel3";
            this.ultraLabel3.Size = new System.Drawing.Size(140, 18);
            this.ultraLabel3.TabIndex = 39;
            this.ultraLabel3.Text = "Total Discount";
            this.ultraLabel3.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // ultraLabel2
            // 
            appearance28.FontData.BoldAsString = "True";
            appearance28.FontData.Name = "Arial";
            appearance28.ForeColor = System.Drawing.Color.White;
            appearance28.TextVAlignAsString = "Middle";
            this.ultraLabel2.Appearance = appearance28;
            this.ultraLabel2.BackColorInternal = System.Drawing.Color.Transparent;
            this.ultraLabel2.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
            this.ultraLabel2.Location = new System.Drawing.Point(10, 46);
            this.ultraLabel2.Name = "ultraLabel2";
            this.ultraLabel2.Size = new System.Drawing.Size(140, 18);
            this.ultraLabel2.TabIndex = 38;
            this.ultraLabel2.Text = "Total Return ";
            this.ultraLabel2.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // numTotalSale
            // 
            appearance29.BackColor = System.Drawing.Color.LightYellow;
            appearance29.FontData.BoldAsString = "True";
            appearance29.FontData.Name = "Arial";
            appearance29.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.numTotalSale.Appearance = appearance29;
            this.numTotalSale.BackColor = System.Drawing.Color.LightYellow;
            this.numTotalSale.BorderStyle = Infragistics.Win.UIElementBorderStyle.None;
            appearance30.BackColor = System.Drawing.Color.LightGoldenrodYellow;
            appearance30.ForeColor = System.Drawing.Color.Blue;
            this.numTotalSale.ButtonAppearance = appearance30;
            this.numTotalSale.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
            this.numTotalSale.FormatString = "";
            this.numTotalSale.Location = new System.Drawing.Point(160, 20);
            this.numTotalSale.MaskInput = "{LOC}-nnnnnnnnnn.nn";
            this.numTotalSale.Name = "numTotalSale";
            this.numTotalSale.NumericType = Infragistics.Win.UltraWinEditors.NumericType.Double;
            this.numTotalSale.PromptChar = ' ';
            this.numTotalSale.ReadOnly = true;
            this.numTotalSale.Size = new System.Drawing.Size(114, 20);
            this.numTotalSale.TabIndex = 50;
            this.numTotalSale.TabStop = false;
            // 
            // ultraLabel1
            // 
            appearance31.FontData.BoldAsString = "True";
            appearance31.FontData.Name = "Arial";
            appearance31.ForeColor = System.Drawing.Color.White;
            appearance31.TextVAlignAsString = "Middle";
            this.ultraLabel1.Appearance = appearance31;
            this.ultraLabel1.BackColorInternal = System.Drawing.Color.Transparent;
            this.ultraLabel1.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
            this.ultraLabel1.Location = new System.Drawing.Point(10, 21);
            this.ultraLabel1.Name = "ultraLabel1";
            this.ultraLabel1.Size = new System.Drawing.Size(140, 18);
            this.ultraLabel1.TabIndex = 37;
            this.ultraLabel1.Text = "Total Sale";
            this.ultraLabel1.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // numNetSale
            // 
            appearance32.BackColor = System.Drawing.Color.RoyalBlue;
            appearance32.FontData.SizeInPoints = 11F;
            appearance32.ForeColor = System.Drawing.Color.White;
            this.numNetSale.Appearance = appearance32;
            this.numNetSale.AutoSize = false;
            this.numNetSale.BackColor = System.Drawing.Color.RoyalBlue;
            this.numNetSale.BorderStyle = Infragistics.Win.UIElementBorderStyle.None;
            appearance33.BackColor = System.Drawing.Color.Transparent;
            appearance33.BackColor2 = System.Drawing.Color.Transparent;
            this.numNetSale.ButtonAppearance = appearance33;
            this.numNetSale.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
            this.numNetSale.Location = new System.Drawing.Point(160, 95);
            this.numNetSale.MaskInput = "{LOC}-nnnnnnnnnn.nn";
            this.numNetSale.Name = "numNetSale";
            this.numNetSale.NumericType = Infragistics.Win.UltraWinEditors.NumericType.Double;
            this.numNetSale.PromptChar = ' ';
            this.numNetSale.ReadOnly = true;
            this.numNetSale.Size = new System.Drawing.Size(114, 20);
            this.numNetSale.TabIndex = 53;
            this.numNetSale.TabStop = false;
            // 
            // numSalesTax
            // 
            appearance34.BackColor = System.Drawing.Color.White;
            appearance34.FontData.BoldAsString = "True";
            appearance34.FontData.Name = "Arial";
            appearance34.ForeColor = System.Drawing.Color.Black;
            this.numSalesTax.Appearance = appearance34;
            this.numSalesTax.BackColor = System.Drawing.Color.White;
            this.numSalesTax.BorderStyle = Infragistics.Win.UIElementBorderStyle.None;
            appearance35.BackColor = System.Drawing.Color.Transparent;
            appearance35.BackColor2 = System.Drawing.Color.Transparent;
            this.numSalesTax.ButtonAppearance = appearance35;
            this.numSalesTax.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
            this.numSalesTax.Location = new System.Drawing.Point(160, 120);
            this.numSalesTax.MaskInput = "{LOC}-nnnnnnnnnn.nn";
            this.numSalesTax.Name = "numSalesTax";
            this.numSalesTax.NumericType = Infragistics.Win.UltraWinEditors.NumericType.Double;
            this.numSalesTax.PromptChar = ' ';
            this.numSalesTax.ReadOnly = true;
            this.numSalesTax.Size = new System.Drawing.Size(114, 20);
            this.numSalesTax.TabIndex = 54;
            this.numSalesTax.TabStop = false;
            // 
            // lvPayType
            // 
            this.lvPayType.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lvPayType.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2});
            this.lvPayType.FullRowSelect = true;
            this.lvPayType.GridLines = true;
            this.lvPayType.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.lvPayType.HideSelection = false;
            this.lvPayType.Location = new System.Drawing.Point(300, 12);
            this.lvPayType.MultiSelect = false;
            this.lvPayType.Name = "lvPayType";
            this.lvPayType.Size = new System.Drawing.Size(447, 242);
            this.lvPayType.TabIndex = 46;
            this.lvPayType.TabStop = false;
            this.lvPayType.UseCompatibleStateImageBehavior = false;
            this.lvPayType.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Pay Type";
            this.columnHeader1.Width = 131;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Amount";
            this.columnHeader2.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.columnHeader2.Width = 103;
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
            appearance36.BackColor = System.Drawing.Color.White;
            appearance36.BackColor2 = System.Drawing.Color.White;
            appearance36.BackColorDisabled = System.Drawing.Color.White;
            appearance36.BackColorDisabled2 = System.Drawing.Color.White;
            appearance36.BackGradientStyle = Infragistics.Win.GradientStyle.None;
            this.grdDepartments.DisplayLayout.Appearance = appearance36;
            ultraGridColumn5.Header.VisiblePosition = 0;
            ultraGridColumn6.Header.VisiblePosition = 1;
            ultraGridBand1.Columns.AddRange(new object[] {
            ultraGridColumn5,
            ultraGridColumn6});
            this.grdDepartments.DisplayLayout.BandsSerializer.Add(ultraGridBand1);
            this.grdDepartments.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.grdDepartments.DisplayLayout.InterBandSpacing = 10;
            appearance37.BackColor = System.Drawing.Color.White;
            appearance37.BackColor2 = System.Drawing.Color.White;
            this.grdDepartments.DisplayLayout.Override.ActiveCardCaptionAppearance = appearance37;
            appearance38.BackColor = System.Drawing.Color.White;
            appearance38.BackColor2 = System.Drawing.Color.White;
            appearance38.BorderColor = System.Drawing.Color.Gray;
            this.grdDepartments.DisplayLayout.Override.ActiveRowAppearance = appearance38;
            appearance39.BackColor = System.Drawing.Color.White;
            appearance39.BackColor2 = System.Drawing.Color.White;
            appearance39.BorderColor = System.Drawing.Color.Gray;
            this.grdDepartments.DisplayLayout.Override.AddRowAppearance = appearance39;
            this.grdDepartments.DisplayLayout.Override.AllowAddNew = Infragistics.Win.UltraWinGrid.AllowAddNew.No;
            this.grdDepartments.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.False;
            this.grdDepartments.DisplayLayout.Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.False;
            appearance40.BackColor = System.Drawing.Color.Transparent;
            this.grdDepartments.DisplayLayout.Override.CardAreaAppearance = appearance40;
            appearance41.BackColor = System.Drawing.Color.White;
            appearance41.BackColor2 = System.Drawing.Color.White;
            appearance41.BackColorDisabled = System.Drawing.Color.White;
            appearance41.BackColorDisabled2 = System.Drawing.Color.White;
            appearance41.BorderColor = System.Drawing.Color.Black;
            appearance41.BorderColor3DBase = System.Drawing.Color.Black;
            this.grdDepartments.DisplayLayout.Override.CellAppearance = appearance41;
            appearance42.BackColor = System.Drawing.Color.White;
            appearance42.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(231)))), ((int)(((byte)(230)))));
            appearance42.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance42.BorderAlpha = Infragistics.Win.Alpha.Transparent;
            appearance42.BorderColor = System.Drawing.Color.Gray;
            appearance42.BorderColor3DBase = System.Drawing.Color.Gray;
            appearance42.ImageBackgroundAlpha = Infragistics.Win.Alpha.Transparent;
            appearance42.ImageBackgroundStyle = Infragistics.Win.ImageBackgroundStyle.Centered;
            this.grdDepartments.DisplayLayout.Override.CellButtonAppearance = appearance42;
            appearance43.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            appearance43.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.grdDepartments.DisplayLayout.Override.EditCellAppearance = appearance43;
            appearance44.BorderColor = System.Drawing.Color.Gray;
            this.grdDepartments.DisplayLayout.Override.FilteredInRowAppearance = appearance44;
            appearance45.BorderColor = System.Drawing.Color.Gray;
            this.grdDepartments.DisplayLayout.Override.FilteredOutRowAppearance = appearance45;
            appearance46.BackColor = System.Drawing.Color.White;
            appearance46.BackColor2 = System.Drawing.Color.White;
            appearance46.BackColorDisabled = System.Drawing.Color.White;
            appearance46.BackColorDisabled2 = System.Drawing.Color.White;
            this.grdDepartments.DisplayLayout.Override.FixedCellAppearance = appearance46;
            appearance47.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            appearance47.BackColor2 = System.Drawing.Color.Beige;
            this.grdDepartments.DisplayLayout.Override.FixedHeaderAppearance = appearance47;
            appearance48.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(62)))), ((int)(((byte)(176)))));
            appearance48.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance48.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance48.FontData.BoldAsString = "True";
            appearance48.FontData.SizeInPoints = 10F;
            appearance48.ForeColor = System.Drawing.Color.White;
            appearance48.TextHAlignAsString = "Left";
            appearance48.ThemedElementAlpha = Infragistics.Win.Alpha.Transparent;
            this.grdDepartments.DisplayLayout.Override.HeaderAppearance = appearance48;
            this.grdDepartments.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortSingle;
            appearance49.BorderColor = System.Drawing.Color.Gray;
            this.grdDepartments.DisplayLayout.Override.RowAlternateAppearance = appearance49;
            appearance50.BackColor = System.Drawing.Color.White;
            appearance50.BackColor2 = System.Drawing.Color.White;
            appearance50.BackGradientStyle = Infragistics.Win.GradientStyle.None;
            appearance50.BackHatchStyle = Infragistics.Win.BackHatchStyle.None;
            appearance50.BorderColor = System.Drawing.Color.Gray;
            this.grdDepartments.DisplayLayout.Override.RowAppearance = appearance50;
            appearance51.BorderColor = System.Drawing.Color.Gray;
            this.grdDepartments.DisplayLayout.Override.RowPreviewAppearance = appearance51;
            appearance52.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(62)))), ((int)(((byte)(176)))));
            appearance52.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance52.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance52.BorderColor = System.Drawing.Color.Gray;
            this.grdDepartments.DisplayLayout.Override.RowSelectorAppearance = appearance52;
            this.grdDepartments.DisplayLayout.Override.RowSelectorWidth = 12;
            this.grdDepartments.DisplayLayout.Override.RowSpacingBefore = 2;
            appearance53.BackColor = System.Drawing.Color.Navy;
            appearance53.BackColorAlpha = Infragistics.Win.Alpha.Opaque;
            this.grdDepartments.DisplayLayout.Override.SelectedCellAppearance = appearance53;
            appearance54.BackColor = System.Drawing.Color.Navy;
            appearance54.BackColorDisabled = System.Drawing.Color.Navy;
            appearance54.BackGradientStyle = Infragistics.Win.GradientStyle.None;
            appearance54.BackHatchStyle = Infragistics.Win.BackHatchStyle.None;
            appearance54.BorderColor = System.Drawing.Color.Gray;
            appearance54.ForeColor = System.Drawing.Color.White;
            this.grdDepartments.DisplayLayout.Override.SelectedRowAppearance = appearance54;
            this.grdDepartments.DisplayLayout.Override.SelectTypeCell = Infragistics.Win.UltraWinGrid.SelectType.None;
            this.grdDepartments.DisplayLayout.Override.SelectTypeCol = Infragistics.Win.UltraWinGrid.SelectType.None;
            this.grdDepartments.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.Single;
            appearance55.BorderColor = System.Drawing.Color.Gray;
            this.grdDepartments.DisplayLayout.Override.TemplateAddRowAppearance = appearance55;
            this.grdDepartments.DisplayLayout.RowConnectorColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            this.grdDepartments.DisplayLayout.RowConnectorStyle = Infragistics.Win.UltraWinGrid.RowConnectorStyle.Solid;
            appearance56.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(231)))), ((int)(((byte)(231)))));
            appearance56.BackColor2 = System.Drawing.Color.White;
            appearance56.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance56.BorderAlpha = Infragistics.Win.Alpha.Transparent;
            appearance56.ThemedElementAlpha = Infragistics.Win.Alpha.Transparent;
            scrollBarLook1.ButtonAppearance = appearance56;
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
            this.ultraTabPageControl3.Controls.Add(this.grdStations);
            this.ultraTabPageControl3.Location = new System.Drawing.Point(-10000, -10000);
            this.ultraTabPageControl3.Name = "ultraTabPageControl3";
            this.ultraTabPageControl3.Size = new System.Drawing.Size(754, 383);
            // 
            // grdStations
            // 
            appearance57.BackColor = System.Drawing.Color.White;
            appearance57.BackColor2 = System.Drawing.Color.White;
            appearance57.BackColorDisabled = System.Drawing.Color.White;
            appearance57.BackColorDisabled2 = System.Drawing.Color.White;
            appearance57.BackGradientStyle = Infragistics.Win.GradientStyle.None;
            this.grdStations.DisplayLayout.Appearance = appearance57;
            ultraGridColumn7.Header.VisiblePosition = 0;
            ultraGridColumn4.Header.VisiblePosition = 1;
            ultraGridBand2.Columns.AddRange(new object[] {
            ultraGridColumn7,
            ultraGridColumn4});
            this.grdStations.DisplayLayout.BandsSerializer.Add(ultraGridBand2);
            this.grdStations.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.grdStations.DisplayLayout.InterBandSpacing = 10;
            appearance58.BackColor = System.Drawing.Color.White;
            appearance58.BackColor2 = System.Drawing.Color.White;
            this.grdStations.DisplayLayout.Override.ActiveCardCaptionAppearance = appearance58;
            appearance59.BackColor = System.Drawing.Color.White;
            appearance59.BackColor2 = System.Drawing.Color.White;
            appearance59.BorderColor = System.Drawing.Color.Gray;
            this.grdStations.DisplayLayout.Override.ActiveRowAppearance = appearance59;
            appearance60.BackColor = System.Drawing.Color.White;
            appearance60.BackColor2 = System.Drawing.Color.White;
            appearance60.BorderColor = System.Drawing.Color.Gray;
            this.grdStations.DisplayLayout.Override.AddRowAppearance = appearance60;
            this.grdStations.DisplayLayout.Override.AllowAddNew = Infragistics.Win.UltraWinGrid.AllowAddNew.No;
            this.grdStations.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.False;
            this.grdStations.DisplayLayout.Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.False;
            appearance61.BackColor = System.Drawing.Color.Transparent;
            this.grdStations.DisplayLayout.Override.CardAreaAppearance = appearance61;
            appearance62.BackColor = System.Drawing.Color.White;
            appearance62.BackColor2 = System.Drawing.Color.White;
            appearance62.BackColorDisabled = System.Drawing.Color.White;
            appearance62.BackColorDisabled2 = System.Drawing.Color.White;
            appearance62.BorderColor = System.Drawing.Color.Black;
            appearance62.BorderColor3DBase = System.Drawing.Color.Black;
            this.grdStations.DisplayLayout.Override.CellAppearance = appearance62;
            appearance63.BackColor = System.Drawing.Color.White;
            appearance63.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(231)))), ((int)(((byte)(230)))));
            appearance63.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance63.BorderAlpha = Infragistics.Win.Alpha.Transparent;
            appearance63.BorderColor = System.Drawing.Color.Gray;
            appearance63.BorderColor3DBase = System.Drawing.Color.Gray;
            appearance63.ImageBackgroundAlpha = Infragistics.Win.Alpha.Transparent;
            appearance63.ImageBackgroundStyle = Infragistics.Win.ImageBackgroundStyle.Centered;
            this.grdStations.DisplayLayout.Override.CellButtonAppearance = appearance63;
            appearance64.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            appearance64.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.grdStations.DisplayLayout.Override.EditCellAppearance = appearance64;
            appearance65.BorderColor = System.Drawing.Color.Gray;
            this.grdStations.DisplayLayout.Override.FilteredInRowAppearance = appearance65;
            appearance66.BorderColor = System.Drawing.Color.Gray;
            this.grdStations.DisplayLayout.Override.FilteredOutRowAppearance = appearance66;
            appearance67.BackColor = System.Drawing.Color.White;
            appearance67.BackColor2 = System.Drawing.Color.White;
            appearance67.BackColorDisabled = System.Drawing.Color.White;
            appearance67.BackColorDisabled2 = System.Drawing.Color.White;
            this.grdStations.DisplayLayout.Override.FixedCellAppearance = appearance67;
            appearance68.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            appearance68.BackColor2 = System.Drawing.Color.Beige;
            this.grdStations.DisplayLayout.Override.FixedHeaderAppearance = appearance68;
            appearance69.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(62)))), ((int)(((byte)(176)))));
            appearance69.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance69.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance69.FontData.BoldAsString = "True";
            appearance69.FontData.SizeInPoints = 10F;
            appearance69.ForeColor = System.Drawing.Color.White;
            appearance69.TextHAlignAsString = "Left";
            appearance69.ThemedElementAlpha = Infragistics.Win.Alpha.Transparent;
            this.grdStations.DisplayLayout.Override.HeaderAppearance = appearance69;
            this.grdStations.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortSingle;
            appearance70.BorderColor = System.Drawing.Color.Gray;
            this.grdStations.DisplayLayout.Override.RowAlternateAppearance = appearance70;
            appearance71.BackColor = System.Drawing.Color.White;
            appearance71.BackColor2 = System.Drawing.Color.White;
            appearance71.BackGradientStyle = Infragistics.Win.GradientStyle.None;
            appearance71.BackHatchStyle = Infragistics.Win.BackHatchStyle.None;
            appearance71.BorderColor = System.Drawing.Color.Gray;
            this.grdStations.DisplayLayout.Override.RowAppearance = appearance71;
            appearance72.BorderColor = System.Drawing.Color.Gray;
            this.grdStations.DisplayLayout.Override.RowPreviewAppearance = appearance72;
            appearance73.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(62)))), ((int)(((byte)(176)))));
            appearance73.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance73.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance73.BorderColor = System.Drawing.Color.Gray;
            this.grdStations.DisplayLayout.Override.RowSelectorAppearance = appearance73;
            this.grdStations.DisplayLayout.Override.RowSelectorWidth = 12;
            this.grdStations.DisplayLayout.Override.RowSpacingBefore = 2;
            appearance74.BackColor = System.Drawing.Color.Navy;
            appearance74.BackColorAlpha = Infragistics.Win.Alpha.Opaque;
            this.grdStations.DisplayLayout.Override.SelectedCellAppearance = appearance74;
            appearance75.BackColor = System.Drawing.Color.Navy;
            appearance75.BackColorDisabled = System.Drawing.Color.Navy;
            appearance75.BackGradientStyle = Infragistics.Win.GradientStyle.None;
            appearance75.BackHatchStyle = Infragistics.Win.BackHatchStyle.None;
            appearance75.BorderColor = System.Drawing.Color.Gray;
            appearance75.ForeColor = System.Drawing.Color.White;
            this.grdStations.DisplayLayout.Override.SelectedRowAppearance = appearance75;
            this.grdStations.DisplayLayout.Override.SelectTypeCell = Infragistics.Win.UltraWinGrid.SelectType.None;
            this.grdStations.DisplayLayout.Override.SelectTypeCol = Infragistics.Win.UltraWinGrid.SelectType.None;
            this.grdStations.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.Single;
            appearance76.BorderColor = System.Drawing.Color.Gray;
            this.grdStations.DisplayLayout.Override.TemplateAddRowAppearance = appearance76;
            this.grdStations.DisplayLayout.RowConnectorColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            this.grdStations.DisplayLayout.RowConnectorStyle = Infragistics.Win.UltraWinGrid.RowConnectorStyle.Solid;
            appearance77.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(231)))), ((int)(((byte)(231)))));
            appearance77.BackColor2 = System.Drawing.Color.White;
            appearance77.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance77.BorderAlpha = Infragistics.Win.Alpha.Transparent;
            appearance77.ThemedElementAlpha = Infragistics.Win.Alpha.Transparent;
            scrollBarLook2.ButtonAppearance = appearance77;
            this.grdStations.DisplayLayout.ScrollBarLook = scrollBarLook2;
            this.grdStations.DisplayLayout.TabNavigation = Infragistics.Win.UltraWinGrid.TabNavigation.NextControl;
            this.grdStations.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdStations.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.grdStations.Location = new System.Drawing.Point(0, 0);
            this.grdStations.Name = "grdStations";
            this.grdStations.Size = new System.Drawing.Size(754, 383);
            this.grdStations.TabIndex = 72;
            this.grdStations.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // ultraTabPageControl4
            // 
            this.ultraTabPageControl4.Controls.Add(this.grdTransDetail);
            this.ultraTabPageControl4.Controls.Add(this.pnlTop);
            this.ultraTabPageControl4.Location = new System.Drawing.Point(-10000, -10000);
            this.ultraTabPageControl4.Name = "ultraTabPageControl4";
            this.ultraTabPageControl4.Size = new System.Drawing.Size(754, 383);
            // 
            // grdTransDetail
            // 
            appearance78.BackColor = System.Drawing.Color.White;
            appearance78.BackColor2 = System.Drawing.Color.White;
            appearance78.BackColorDisabled = System.Drawing.Color.White;
            appearance78.BackColorDisabled2 = System.Drawing.Color.White;
            appearance78.BackGradientStyle = Infragistics.Win.GradientStyle.None;
            this.grdTransDetail.DisplayLayout.Appearance = appearance78;
            ultraGridColumn1.Header.VisiblePosition = 0;
            ultraGridColumn2.Header.VisiblePosition = 1;
            ultraGridColumn3.Header.VisiblePosition = 2;
            ultraGridBand3.Columns.AddRange(new object[] {
            ultraGridColumn1,
            ultraGridColumn2,
            ultraGridColumn3});
            ultraGridBand3.HeaderVisible = true;
            this.grdTransDetail.DisplayLayout.BandsSerializer.Add(ultraGridBand3);
            this.grdTransDetail.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.grdTransDetail.DisplayLayout.InterBandSpacing = 10;
            this.grdTransDetail.DisplayLayout.MaxColScrollRegions = 1;
            this.grdTransDetail.DisplayLayout.MaxRowScrollRegions = 1;
            appearance79.BackColor = System.Drawing.Color.White;
            appearance79.BackColor2 = System.Drawing.Color.White;
            this.grdTransDetail.DisplayLayout.Override.ActiveCardCaptionAppearance = appearance79;
            appearance80.BackColor = System.Drawing.Color.White;
            appearance80.BackColor2 = System.Drawing.Color.White;
            appearance80.BorderColor = System.Drawing.Color.Gray;
            this.grdTransDetail.DisplayLayout.Override.ActiveRowAppearance = appearance80;
            appearance81.BackColor = System.Drawing.Color.White;
            appearance81.BackColor2 = System.Drawing.Color.White;
            appearance81.BorderColor = System.Drawing.Color.Gray;
            this.grdTransDetail.DisplayLayout.Override.AddRowAppearance = appearance81;
            this.grdTransDetail.DisplayLayout.Override.AllowAddNew = Infragistics.Win.UltraWinGrid.AllowAddNew.No;
            this.grdTransDetail.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.False;
            this.grdTransDetail.DisplayLayout.Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.False;
            appearance82.BackColor = System.Drawing.Color.Transparent;
            this.grdTransDetail.DisplayLayout.Override.CardAreaAppearance = appearance82;
            appearance83.BackColor = System.Drawing.Color.White;
            appearance83.BackColor2 = System.Drawing.Color.White;
            appearance83.BackColorDisabled = System.Drawing.Color.White;
            appearance83.BackColorDisabled2 = System.Drawing.Color.White;
            appearance83.BorderColor = System.Drawing.Color.Black;
            appearance83.BorderColor3DBase = System.Drawing.Color.Black;
            this.grdTransDetail.DisplayLayout.Override.CellAppearance = appearance83;
            appearance84.BackColor = System.Drawing.Color.White;
            appearance84.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(231)))), ((int)(((byte)(230)))));
            appearance84.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance84.BorderAlpha = Infragistics.Win.Alpha.Transparent;
            appearance84.BorderColor = System.Drawing.Color.Gray;
            appearance84.BorderColor3DBase = System.Drawing.Color.Gray;
            appearance84.ImageBackgroundAlpha = Infragistics.Win.Alpha.Transparent;
            appearance84.ImageBackgroundStyle = Infragistics.Win.ImageBackgroundStyle.Centered;
            this.grdTransDetail.DisplayLayout.Override.CellButtonAppearance = appearance84;
            appearance85.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            appearance85.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.grdTransDetail.DisplayLayout.Override.EditCellAppearance = appearance85;
            appearance86.BorderColor = System.Drawing.Color.Gray;
            this.grdTransDetail.DisplayLayout.Override.FilteredInRowAppearance = appearance86;
            appearance87.BorderColor = System.Drawing.Color.Gray;
            this.grdTransDetail.DisplayLayout.Override.FilteredOutRowAppearance = appearance87;
            appearance88.BackColor = System.Drawing.Color.White;
            appearance88.BackColor2 = System.Drawing.Color.White;
            appearance88.BackColorDisabled = System.Drawing.Color.White;
            appearance88.BackColorDisabled2 = System.Drawing.Color.White;
            this.grdTransDetail.DisplayLayout.Override.FixedCellAppearance = appearance88;
            appearance89.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            appearance89.BackColor2 = System.Drawing.Color.Beige;
            this.grdTransDetail.DisplayLayout.Override.FixedHeaderAppearance = appearance89;
            appearance90.BackColor = System.Drawing.Color.White;
            appearance90.BackColor2 = System.Drawing.SystemColors.Control;
            appearance90.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance90.FontData.BoldAsString = "True";
            appearance90.ForeColor = System.Drawing.Color.Black;
            appearance90.TextHAlignAsString = "Left";
            appearance90.ThemedElementAlpha = Infragistics.Win.Alpha.Transparent;
            this.grdTransDetail.DisplayLayout.Override.HeaderAppearance = appearance90;
            appearance91.BorderColor = System.Drawing.Color.Gray;
            this.grdTransDetail.DisplayLayout.Override.RowAlternateAppearance = appearance91;
            appearance92.BackColor = System.Drawing.Color.White;
            appearance92.BackColor2 = System.Drawing.Color.White;
            appearance92.BackGradientStyle = Infragistics.Win.GradientStyle.None;
            appearance92.BackHatchStyle = Infragistics.Win.BackHatchStyle.None;
            appearance92.BorderColor = System.Drawing.Color.Gray;
            this.grdTransDetail.DisplayLayout.Override.RowAppearance = appearance92;
            appearance93.BorderColor = System.Drawing.Color.Gray;
            this.grdTransDetail.DisplayLayout.Override.RowPreviewAppearance = appearance93;
            appearance94.BackColor = System.Drawing.Color.White;
            appearance94.BackColor2 = System.Drawing.SystemColors.Control;
            appearance94.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance94.BorderColor = System.Drawing.Color.Gray;
            this.grdTransDetail.DisplayLayout.Override.RowSelectorAppearance = appearance94;
            this.grdTransDetail.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            this.grdTransDetail.DisplayLayout.Override.RowSelectorWidth = 12;
            this.grdTransDetail.DisplayLayout.Override.RowSpacingBefore = 2;
            appearance95.BackColor = System.Drawing.Color.Navy;
            appearance95.BackColorAlpha = Infragistics.Win.Alpha.Opaque;
            this.grdTransDetail.DisplayLayout.Override.SelectedCellAppearance = appearance95;
            appearance96.BackColor = System.Drawing.Color.Navy;
            appearance96.BackColorDisabled = System.Drawing.Color.Navy;
            appearance96.BackGradientStyle = Infragistics.Win.GradientStyle.None;
            appearance96.BackHatchStyle = Infragistics.Win.BackHatchStyle.None;
            appearance96.BorderColor = System.Drawing.Color.Gray;
            appearance96.ForeColor = System.Drawing.Color.Black;
            this.grdTransDetail.DisplayLayout.Override.SelectedRowAppearance = appearance96;
            this.grdTransDetail.DisplayLayout.Override.SelectTypeCell = Infragistics.Win.UltraWinGrid.SelectType.None;
            this.grdTransDetail.DisplayLayout.Override.SelectTypeCol = Infragistics.Win.UltraWinGrid.SelectType.None;
            this.grdTransDetail.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.Single;
            appearance97.BorderColor = System.Drawing.Color.Gray;
            this.grdTransDetail.DisplayLayout.Override.TemplateAddRowAppearance = appearance97;
            this.grdTransDetail.DisplayLayout.RowConnectorColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            this.grdTransDetail.DisplayLayout.RowConnectorStyle = Infragistics.Win.UltraWinGrid.RowConnectorStyle.Solid;
            appearance98.BackColor = System.Drawing.Color.White;
            appearance98.BackColor2 = System.Drawing.SystemColors.Control;
            appearance98.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            scrollBarLook3.Appearance = appearance98;
            appearance99.BackColor = System.Drawing.Color.White;
            appearance99.BackColor2 = System.Drawing.SystemColors.Control;
            appearance99.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance99.ThemedElementAlpha = Infragistics.Win.Alpha.Transparent;
            scrollBarLook3.ButtonAppearance = appearance99;
            appearance100.BackColor = System.Drawing.Color.White;
            appearance100.BackColor2 = System.Drawing.SystemColors.Control;
            scrollBarLook3.ThumbAppearance = appearance100;
            appearance101.BackColor = System.Drawing.Color.White;
            appearance101.BackColor2 = System.Drawing.Color.White;
            scrollBarLook3.TrackAppearance = appearance101;
            this.grdTransDetail.DisplayLayout.ScrollBarLook = scrollBarLook3;
            this.grdTransDetail.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdTransDetail.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.grdTransDetail.Location = new System.Drawing.Point(0, 25);
            this.grdTransDetail.Name = "grdTransDetail";
            this.grdTransDetail.Size = new System.Drawing.Size(754, 358);
            this.grdTransDetail.TabIndex = 2;
            this.grdTransDetail.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // pnlTop
            // 
            this.pnlTop.Controls.Add(this.lblTransRange);
            this.pnlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTop.Location = new System.Drawing.Point(0, 0);
            this.pnlTop.Name = "pnlTop";
            this.pnlTop.Size = new System.Drawing.Size(754, 25);
            this.pnlTop.TabIndex = 3;
            // 
            // lblTransRange
            // 
            appearance102.FontData.BoldAsString = "True";
            appearance102.ForeColor = System.Drawing.Color.Red;
            this.lblTransRange.Appearance = appearance102;
            this.lblTransRange.Location = new System.Drawing.Point(5, 2);
            this.lblTransRange.Name = "lblTransRange";
            this.lblTransRange.Size = new System.Drawing.Size(745, 20);
            this.lblTransRange.TabIndex = 0;
            this.lblTransRange.Tag = "NOCOLOR";
            // 
            // ultraLabel10
            // 
            appearance103.FontData.BoldAsString = "True";
            appearance103.FontData.Name = "Arial";
            appearance103.ForeColor = System.Drawing.Color.White;
            appearance103.TextVAlignAsString = "Middle";
            this.ultraLabel10.Appearance = appearance103;
            this.ultraLabel10.AutoSize = true;
            this.ultraLabel10.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
            this.ultraLabel10.Location = new System.Drawing.Point(10, 18);
            this.ultraLabel10.Name = "ultraLabel10";
            this.ultraLabel10.Size = new System.Drawing.Size(79, 17);
            this.ultraLabel10.TabIndex = 20;
            this.ultraLabel10.Text = "End Of Day";
            this.ultraLabel10.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // cmdProcess
            // 
            appearance104.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(62)))), ((int)(((byte)(176)))));
            appearance104.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance104.FontData.BoldAsString = "True";
            appearance104.ForeColor = System.Drawing.Color.White;
            this.cmdProcess.Appearance = appearance104;
            this.cmdProcess.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            this.cmdProcess.Location = new System.Drawing.Point(545, 16);
            this.cmdProcess.Name = "cmdProcess";
            this.cmdProcess.Size = new System.Drawing.Size(100, 28);
            this.cmdProcess.TabIndex = 1;
            this.cmdProcess.Text = "&Process";
            this.cmdProcess.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.cmdProcess.Click += new System.EventHandler(this.cmdProcess_Click);
            // 
            // cmdClose
            // 
            appearance105.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(62)))), ((int)(((byte)(176)))));
            appearance105.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance105.FontData.BoldAsString = "True";
            appearance105.ForeColor = System.Drawing.Color.White;
            this.cmdClose.Appearance = appearance105;
            this.cmdClose.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            this.cmdClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cmdClose.Location = new System.Drawing.Point(650, 16);
            this.cmdClose.Name = "cmdClose";
            this.cmdClose.Size = new System.Drawing.Size(100, 28);
            this.cmdClose.TabIndex = 2;
            this.cmdClose.Text = "&Close";
            this.cmdClose.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.cmdClose.Click += new System.EventHandler(this.cmdClose_Click);
            // 
            // txtEndOfDayNo
            // 
            appearance106.BackColor = System.Drawing.Color.White;
            appearance106.FontData.BoldAsString = "True";
            appearance106.FontData.Name = "Arial";
            appearance106.ForeColor = System.Drawing.Color.Red;
            appearance106.TextVAlignAsString = "Middle";
            this.txtEndOfDayNo.Appearance = appearance106;
            this.txtEndOfDayNo.BackColor = System.Drawing.Color.White;
            this.txtEndOfDayNo.BorderStyle = Infragistics.Win.UIElementBorderStyle.None;
            this.txtEndOfDayNo.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
            this.txtEndOfDayNo.Location = new System.Drawing.Point(96, 16);
            this.txtEndOfDayNo.Name = "txtEndOfDayNo";
            this.txtEndOfDayNo.Size = new System.Drawing.Size(133, 20);
            this.txtEndOfDayNo.TabIndex = 27;
            this.txtEndOfDayNo.TabStop = false;
            // 
            // ultraTabControl1
            // 
            appearance107.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(108)))), ((int)(((byte)(172)))));
            appearance107.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(108)))), ((int)(((byte)(172)))));
            appearance107.BackGradientStyle = Infragistics.Win.GradientStyle.None;
            appearance107.BorderColor = System.Drawing.Color.Navy;
            appearance107.BorderColor3DBase = System.Drawing.Color.Navy;
            this.ultraTabControl1.ClientAreaAppearance = appearance107;
            this.ultraTabControl1.Controls.Add(this.ultraTabSharedControlsPage1);
            this.ultraTabControl1.Controls.Add(this.ultraTabPageControl1);
            this.ultraTabControl1.Controls.Add(this.ultraTabPageControl2);
            this.ultraTabControl1.Controls.Add(this.ultraTabPageControl3);
            this.ultraTabControl1.Controls.Add(this.ultraTabPageControl4);
            this.ultraTabControl1.Location = new System.Drawing.Point(10, 80);
            this.ultraTabControl1.MinTabWidth = 80;
            this.ultraTabControl1.Name = "ultraTabControl1";
            this.ultraTabControl1.SharedControlsPage = this.ultraTabSharedControlsPage1;
            this.ultraTabControl1.Size = new System.Drawing.Size(758, 408);
            this.ultraTabControl1.Style = Infragistics.Win.UltraWinTabControl.UltraTabControlStyle.PropertyPage2003;
            this.ultraTabControl1.TabIndex = 41;
            appearance108.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(108)))), ((int)(((byte)(172)))));
            appearance108.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance108.FontData.BoldAsString = "True";
            appearance108.ForeColor = System.Drawing.Color.White;
            ultraTab1.Appearance = appearance108;
            ultraTab1.TabPage = this.ultraTabPageControl1;
            ultraTab1.Text = "End Of Day";
            appearance109.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(108)))), ((int)(((byte)(172)))));
            appearance109.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance109.FontData.BoldAsString = "True";
            appearance109.ForeColor = System.Drawing.Color.White;
            ultraTab2.Appearance = appearance109;
            ultraTab2.TabPage = this.ultraTabPageControl2;
            ultraTab2.Text = "Department Summary";
            appearance110.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(108)))), ((int)(((byte)(172)))));
            appearance110.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance110.FontData.BoldAsString = "True";
            appearance110.ForeColor = System.Drawing.Color.White;
            ultraTab3.Appearance = appearance110;
            ultraTab3.TabPage = this.ultraTabPageControl3;
            ultraTab3.Text = "Station Summary";
            appearance111.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(108)))), ((int)(((byte)(172)))));
            appearance111.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance111.FontData.BoldAsString = "True";
            appearance111.ForeColor = System.Drawing.Color.White;
            ultraTab4.Appearance = appearance111;
            ultraTab4.TabPage = this.ultraTabPageControl4;
            ultraTab4.Text = "Transaction Details";
            this.ultraTabControl1.Tabs.AddRange(new Infragistics.Win.UltraWinTabControl.UltraTab[] {
            ultraTab1,
            ultraTab2,
            ultraTab3,
            ultraTab4});
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
            // lblTransactionType
            // 
            appearance112.ForeColor = System.Drawing.Color.White;
            appearance112.ForeColorDisabled = System.Drawing.Color.Navy;
            appearance112.ImageVAlign = Infragistics.Win.VAlign.Bottom;
            appearance112.TextHAlignAsString = "Center";
            this.lblTransactionType.Appearance = appearance112;
            this.lblTransactionType.BackColorInternal = System.Drawing.Color.Transparent;
            this.lblTransactionType.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTransactionType.Location = new System.Drawing.Point(10, 0);
            this.lblTransactionType.Name = "lblTransactionType";
            this.lblTransactionType.Size = new System.Drawing.Size(758, 32);
            this.lblTransactionType.TabIndex = 42;
            this.lblTransactionType.Tag = "Header";
            this.lblTransactionType.Text = "End Of Day";
            this.lblTransactionType.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtEODDate);
            this.groupBox1.Controls.Add(this.lblEODDate);
            this.groupBox1.Controls.Add(this.ultraLabel10);
            this.groupBox1.Controls.Add(this.txtEndOfDayNo);
            this.groupBox1.Location = new System.Drawing.Point(10, 33);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(758, 44);
            this.groupBox1.TabIndex = 43;
            this.groupBox1.TabStop = false;
            // 
            // txtEODDate
            // 
            appearance113.FontData.BoldAsString = "True";
            appearance113.FontData.ItalicAsString = "False";
            appearance113.FontData.StrikeoutAsString = "False";
            appearance113.FontData.UnderlineAsString = "False";
            appearance113.ForeColor = System.Drawing.Color.Black;
            appearance113.ForeColorDisabled = System.Drawing.Color.Black;
            this.txtEODDate.Appearance = appearance113;
            this.txtEODDate.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.txtEODDate.Location = new System.Drawing.Point(547, 16);
            this.txtEODDate.MaxLength = 20;
            this.txtEODDate.Name = "txtEODDate";
            this.txtEODDate.ReadOnly = true;
            this.txtEODDate.Size = new System.Drawing.Size(200, 23);
            this.txtEODDate.TabIndex = 34;
            this.txtEODDate.TabStop = false;
            // 
            // lblEODDate
            // 
            appearance114.ForeColor = System.Drawing.Color.White;
            appearance114.TextVAlignAsString = "Middle";
            this.lblEODDate.Appearance = appearance114;
            this.lblEODDate.AutoSize = true;
            this.lblEODDate.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblEODDate.Location = new System.Drawing.Point(468, 18);
            this.lblEODDate.Name = "lblEODDate";
            this.lblEODDate.Size = new System.Drawing.Size(68, 17);
            this.lblEODDate.TabIndex = 33;
            this.lblEODDate.Text = "EOD Date";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.btnPrintTransactionDetails);
            this.groupBox4.Controls.Add(this.cmdProcess);
            this.groupBox4.Controls.Add(this.cmdClose);
            this.groupBox4.Location = new System.Drawing.Point(10, 490);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(758, 51);
            this.groupBox4.TabIndex = 61;
            this.groupBox4.TabStop = false;
            // 
            // btnPrintTransactionDetails
            // 
            appearance115.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(62)))), ((int)(((byte)(176)))));
            appearance115.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance115.FontData.BoldAsString = "True";
            appearance115.ForeColor = System.Drawing.Color.White;
            this.btnPrintTransactionDetails.Appearance = appearance115;
            this.btnPrintTransactionDetails.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            this.btnPrintTransactionDetails.Location = new System.Drawing.Point(315, 16);
            this.btnPrintTransactionDetails.Name = "btnPrintTransactionDetails";
            this.btnPrintTransactionDetails.Size = new System.Drawing.Size(220, 28);
            this.btnPrintTransactionDetails.TabIndex = 3;
            this.btnPrintTransactionDetails.Text = "&Print Transaction Details";
            this.btnPrintTransactionDetails.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnPrintTransactionDetails.Visible = false;
            this.btnPrintTransactionDetails.Click += new System.EventHandler(this.btnPrintTransactionDetails_Click);
            // 
            // lblTransFee
            // 
            appearance9.FontData.BoldAsString = "True";
            appearance9.FontData.Name = "Arial";
            appearance9.ForeColor = System.Drawing.Color.White;
            appearance9.TextVAlignAsString = "Middle";
            this.lblTransFee.Appearance = appearance9;
            this.lblTransFee.BackColorInternal = System.Drawing.Color.Transparent;
            this.lblTransFee.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
            this.lblTransFee.Location = new System.Drawing.Point(10, 196);
            this.lblTransFee.Name = "lblTransFee";
            this.lblTransFee.Size = new System.Drawing.Size(140, 18);
            this.lblTransFee.TabIndex = 58;
            this.lblTransFee.Text = "Transaction Fee";
            this.lblTransFee.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // numTransFee
            // 
            appearance10.BackColor = System.Drawing.Color.White;
            appearance10.BackColorDisabled = System.Drawing.Color.Transparent;
            appearance10.FontData.BoldAsString = "True";
            appearance10.FontData.Name = "Arial";
            appearance10.ForeColor = System.Drawing.Color.Black;
            this.numTransFee.Appearance = appearance10;
            this.numTransFee.BackColor = System.Drawing.Color.White;
            this.numTransFee.BorderStyle = Infragistics.Win.UIElementBorderStyle.None;
            appearance11.BackColor = System.Drawing.Color.Transparent;
            appearance11.BackColor2 = System.Drawing.Color.Transparent;
            this.numTransFee.ButtonAppearance = appearance11;
            this.numTransFee.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
            this.numTransFee.Location = new System.Drawing.Point(160, 195);
            this.numTransFee.MaskInput = "{LOC}-nnnnnnnnnn.nn";
            this.numTransFee.Name = "numTransFee";
            this.numTransFee.NumericType = Infragistics.Win.UltraWinEditors.NumericType.Double;
            this.numTransFee.PromptChar = ' ';
            this.numTransFee.ReadOnly = true;
            this.numTransFee.Size = new System.Drawing.Size(114, 20);
            this.numTransFee.TabIndex = 59;
            this.numTransFee.TabStop = false;
            // 
            // frmEndOfDay
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(7, 17);
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(108)))), ((int)(((byte)(172)))));
            this.CancelButton = this.cmdClose;
            this.ClientSize = new System.Drawing.Size(774, 546);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.lblTransactionType);
            this.Controls.Add(this.ultraTabControl1);
            this.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "frmEndOfDay";
            this.Text = "End Of Day";
            this.Activated += new System.EventHandler(this.frmEndOfDay_Activated);
            this.Load += new System.EventHandler(this.frmEndOfDay_Load);
            this.Resize += new System.EventHandler(this.frmEndOfDay_Resize);
            this.ultraTabPageControl1.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numPayOut)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numNetCash)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numTotalCashU)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numTotal)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numGrandTotal)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numReceiveOnAccount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numTotalCash)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numTotalDiscount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numTotalReturn)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numTotalSale)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numNetSale)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numSalesTax)).EndInit();
            this.ultraTabPageControl2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdDepartments)).EndInit();
            this.ultraTabPageControl3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdStations)).EndInit();
            this.ultraTabPageControl4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdTransDetail)).EndInit();
            this.pnlTop.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.txtEndOfDayNo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraTabControl1)).EndInit();
            this.ultraTabControl1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtEODDate)).EndInit();
            this.groupBox4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.numTransFee)).EndInit();
            this.ResumeLayout(false);

        }
        #endregion

        private void cmdClose_Click(object sender, System.EventArgs e)
        {
            this.Close();
        }

        private void cmdProcess_Click(object sender, System.EventArgs e)
        {
            string[] sOpenStations = null;
            try
            {
                if (cmdProcess.Text != "&Print EOD Report")
                {
                    if (bCalledFromScheduler || Resources.Message.Display("Process End of Day. Are you sure?", "End of Day", MessageBoxButtons.YesNo) == DialogResult.Yes)  //PRIMEPOS-3039 16-Dec-2021 JY Modified
                    {
                        #region Added By Shitaljit for EOD And Station Close at one Shot
                        sOpenStations = oEndOFDay.CheckIfAllStationClosed();
                        if (string.IsNullOrEmpty(Configuration.convertNullToString(sOpenStations)) == false)
                        {
                            if (sOpenStations.Length == 1 && Configuration.StationID.Equals(sOpenStations.GetValue(0)) == true &&
                                Configuration.CInfo.UseCashManagement == false)
                            {
                                #region Close Current Station
                                if (!bCalledFromScheduler && Resources.Message.Display("Current station# " + Configuration.StationID + " is open, Click Yes to close the current station or No to exit?", "End of Day", MessageBoxButtons.YesNo) == DialogResult.Yes)   //PRIMEPOS-3039 16-Dec-2021 JY Modified
                                {
                                    frmStationClose ofrmStClose = new frmStationClose();
                                    ofrmStClose.CloseStation("ALL", true);
                                }
                                else
                                {
                                    strMessage = "Current station# " + Configuration.StationID + " is open, so we cant proceed with the automation for EOD";
                                    this.Close();
                                    return;
                                }
                                #endregion
                            }
                            else
                            {
                                string stations = string.Empty;
                                foreach (string sStation in sOpenStations)
                                {
                                    if (stations == string.Empty)
                                        stations = "'" + sStation + "'";
                                    else
                                        stations += ", '" + sStation + "'";
                                }
                                if (string.IsNullOrEmpty(stations) == false)
                                {
                                    if (bCalledFromScheduler)   //PRIMEPOS-3039 16-Dec-2021 JY Modified
                                    {
                                        //strMessage = "Please Close Stations " + stations + " First";
                                        strMessage = "Please close the following stations before the End of Day can be completed" + Environment.NewLine + "Station(s): " + stations;
                                        this.Close();
                                        return;
                                    }
                                    else
                                    {
                                        //throw new POS_Core.ErrorLogging.POSExceptions("Please Close Stations " + stations + " First", 0);
                                        throw new POS_Core.ErrorLogging.POSExceptions("Please close the following stations before the End of Day can be completed" + Environment.NewLine + "Station(s): " + stations, 0);
                                    }
                                }
                            }
                        }
                        #endregion

                        oEndOFDayData = oEndOFDay.ProcessEndOFDay();
                        Display();
                        RxLabel oLabel = new RxLabel(null, null, null, ReceiptType.Void, null);
                        oLabel.OpenDrawer(bCalledFromScheduler);    //PRIMEPOS-3039 16-Dec-2021 JY Modified
                        FillListView(oEndOFDayData);
                        FillDepartmentListView(oEndOFDayData);
                        //Added By Amit Date 13 May 2011
                        FillStaionListView();
                        PopulateTransDetail(oEndOFDayData.EODID);  //PRIMEPOS-2494 28-Feb-2018 JY Added
                        if (!bCalledFromScheduler)  //PRIMEPOS-3039 16-Dec-2021 JY Added condition
                            Print(false);
                        this.cmdProcess.Text = "&Print EOD Report";
                    }
                }
                else
                {
                    Print(true);
                }
            }
            catch (Exception exp)
            {
                if (bCalledFromScheduler)   //PRIMEPOS-3039 16-Dec-2021 JY Added
                    strMessage = exp.Message;
                else
                    clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }

        private void Display()
        {
            numGrandTotal.Value = oEndOFDayData.GrandTotal;
            //			numNetCash.Value = oEndOFDayData.NetCash;
            numNetSale.Value = oEndOFDayData.NetSale;
            //			numPayOut.Value = oEndOFDayData.Payout;
            numReceiveOnAccount.Value = oEndOFDayData.ReceiveOnAccount;
            numSalesTax.Value = oEndOFDayData.SalesTax;
            numTotal.Value = oEndOFDayData.Total;
            numTotalCash.Value = oEndOFDayData.TotalCash;
            //			numTotalCashU.Value = oEndOFDayData.TotalCashPT;
            //Start b: Edited By Amit Date 13 May 2011 
            //Following if-els is added by shitaljit(QuicSolv) on 10 jan 2012
            if (oEndOFDayData.TotalDiscount > 0)
            {
                numTotalDiscount.Value = oEndOFDayData.TotalDiscount * -1;
            }
            else
            {
                numTotalDiscount.Value = oEndOFDayData.TotalDiscount;
            }
            //numTotalDiscount.Value ="-" +oEndOFDayData.TotalDiscount.ToString();//Commented by shitaljit(QuicSolv) on 10 Jan 2012
            //End
            numTotalReturn.Value = oEndOFDayData.TotalReturn.ToString();
            numTotalSale.Value = oEndOFDayData.TotalSale;
            //Start : Added by Amit Date 04/19/2011
            if (Configuration.CInfo.PrintEODNo == true)
                txtEndOfDayNo.Visible = true;
            else
                txtEndOfDayNo.Visible = false;

            txtEndOfDayNo.Value = oEndOFDayData.EODID;
            //End 

            #region Sprint-24 - PRIMEPOS-2326 20-Oct-2016 JY Added
            numPayOut.Value = oEndOFDayData.Payout;
            numTotalCashU.Value = oEndOFDayData.TotalCashPT;
            decimal CashbackAmount = 0;
            for (int i = 0; i < oEndOFDayData.Details.Count; i++)
            {
                if (oEndOFDayData.Details[i].PayTypeName == "Cash Back")
                {
                    CashbackAmount = oEndOFDayData.Details[i].Amount;
                }
            }
            numNetCash.Value = oEndOFDayData.NetCash + CashbackAmount;
            #endregion
            this.txtEODDate.Text = Configuration.convertNullToString(oEndOFDayData.CloseDate);    //PRIMEPOS-2480 26-Jun-2020 JY Added
            this.numTransFee.Value = oEndOFDayData.TransFee;    //PRIMEPOS-3118 03-Aug-2022 JY Added
        }
        private void FillListView(EndOFDayData oEndOFDayData)
        {
            ListViewItem oItem;
            lvPayType.Items.Clear();
            string[] arr = { "", "" };

            for (int i = 0; i < oEndOFDayData.Details.Count; i++)
            {
                arr[0] = oEndOFDayData.Details[i].PayTypeName;
                arr[1] = oEndOFDayData.Details[i].Amount.ToString("######0.00");
                oItem = new ListViewItem(arr);
                lvPayType.Items.Add(oItem);

                if (oEndOFDayData.Details[i].PayTypeName == "Cash Back")
                {
                    this.numTotalCash.Value = oEndOFDayData.TotalCash + oEndOFDayData.Details[i].Amount;
                }
            }
        }

        private void FillDepartmentListView(EndOFDayData oEndOFDayData)
        {
            /*ListViewItem oItem;
			lvDepartments.Items.Clear();
			string []arr={"","","","",""};

			for( int i =0 ; i < oEndOFDayData.Departments.Count;i++)
			{
				arr[0] = oEndOFDayData.Departments[i].DepartmentId;
				arr[1] = oEndOFDayData.Departments[i].DepartmentName;
				arr[2] = oEndOFDayData.Departments[i].Sales.ToString("######0.00");
				arr[3] = oEndOFDayData.Departments[i].Tax.ToString("######0.00");
				arr[4] = oEndOFDayData.Departments[i].Discount.ToString("######0.00");

				oItem = new ListViewItem( arr);
				lvDepartments.Items.Add(oItem);
			} */
            this.grdDepartments.DataSource = oEndOFDay.GetDepartmentDS(Configuration.convertNullToInt(oEndOFDayData.EODID));

            this.grdDepartments.DisplayLayout.Bands[0].Columns["deptcode"].Header.Caption = "Dept Code";
            this.grdDepartments.DisplayLayout.Bands[0].Columns["deptname"].Header.Caption = "Dept Name";
            this.grdDepartments.DisplayLayout.Bands[0].Columns["totalsale"].Header.Caption = "Total Sale";
            this.grdDepartments.DisplayLayout.Bands[0].Columns["totalreturn"].Header.Caption = "Total Return";
            this.grdDepartments.DisplayLayout.Bands[0].Columns["totaldiscount"].Header.Caption = "Total Disc.";
            this.grdDepartments.DisplayLayout.Bands[0].Columns["totaltax"].Header.Caption = "Total Tax";
            this.resizeColumns(grdDepartments);

        }

        private void FillStaionListView()
        {
            this.grdStations.DataSource = oEndOFDay.GetStationDS(Configuration.convertNullToInt(oEndOFDayData.EODID)); ;
            this.grdStations.DisplayLayout.Bands[0].Columns["StationId"].Header.Caption = "Station ID";
            this.grdStations.DisplayLayout.Bands[0].Columns["StationName"].Header.Caption = "Station Name";
            this.grdStations.DisplayLayout.Bands[0].Columns["TotalSale"].Header.Caption = "Total Sale";
            this.grdStations.DisplayLayout.Bands[0].Columns["TotalReturn"].Header.Caption = "Total Return";
            this.grdStations.DisplayLayout.Bands[0].Columns["TotalDiscount"].Header.Caption = "Total Disc.";
            this.grdStations.DisplayLayout.Bands[0].Columns["NetSale"].Header.Caption = "Net Sale";
            this.grdStations.DisplayLayout.Bands[0].Columns["TotalTax"].Header.Caption = "Total Tax";
            this.resizeColumns(grdStations);
        }

        private void Print(bool PrintDirect)
        {
            try
            {
                this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
                //PrintDirect=false;//Commented By Shitaljit(QuicSolv) on 12 Dec 2011

                Int32 id = Convert.ToInt32(this.txtEndOfDayNo.Text);

                if (Configuration.CPOSSet.UseRcptForEOD == true)
                {
                    Reports.ReportsUI.RcptEOD oRcptEOD = new POS_Core_UI.Reports.ReportsUI.RcptEOD(id);
                    oRcptEOD.Print();
                    Reports.ReportsUI.RcptEODByStation oRcptEODBySt = new POS_Core_UI.Reports.ReportsUI.RcptEODByStation(id);
                    oRcptEODBySt.Print();

                    //Sprint-24 - PRIMEPOS-2363 28-Dec-2016 JY Added
                    if (Configuration.CInfo.AutoEmailEODReport == true)
                    {
                        if (PrintDirect == false) EmailReport(id.ToString());
                    }
                }
                else
                {
                    //rptEOD oRptEndOfDay = new rptEOD();
                    //DataSet ds = oEndOFDay.GetReportSource(id);
                    //DataSet subDs = oEndOFDay.GetSubReportSource(id);
                    //oRptEndOfDay.Database.Tables[0].SetDataSource(ds.Tables[0]);
                    //oRptEndOfDay.OpenSubreport("rptEODPmt").Database.Tables[0].SetDataSource(subDs.Tables[0]);
                    //((CrystalDecisions.CrystalReports.Engine.TextObject) oRptEndOfDay.ReportDefinition.ReportObjects["txtHeader"]).Text =Configuration.CInfo.StoreName;
                    //POS_Core_UI.Reports.ReportsUI.clsReports.PrintReport(oRptEndOfDay);

                    rptEODDepartment oEODDepartment = new rptEODDepartment();
                    rptEODByStation oEODByStation = new rptEODByStation();
                    rptEODTotal oEODTotal = new rptEODTotal();

                    Search oSearch = new Search();
                    string sql;
                    //PRIMEPOS-2386 01-Mar-2021 JY modified
                    sql = " SELECT HDR.CLOSEDATE ,T.EODID,dept.deptname, " +
                        " Sum(CASE T.TRANSTYPE WHEN 1 THEN ExtendedPrice ELSE 0 END) TotalSale " +
                        " , Sum(CASE T.TRANSTYPE WHEN 2 THEN ExtendedPrice ELSE 0 END) TotalReturn " +
                        " , Sum(td.Discount) TotalDiscount, Sum(TaxAmount) TotalTax, HDR.UserID AS Users " +
                        " FROM POSTransactionDetail TD join item i on (i.itemid=td.itemid) " +
                        " left join department dept on ( dept.deptid=i.departmentid) " +
                        " , POSTransaction T ,ENDOFDAYHEADER HDR WHERE TD.TransID = T.TransID AND T.EODID=HDR.EODID AND T.EODID =" + id +
                        " GROUP BY dept.deptname,T.EODID,HDR.CLOSEDATE, HDR.UserID" +
                        " order by dept.deptname ";

                    DataSet dsDept = oSearch.SearchData(sql);
                    oEODDepartment.Database.Tables[0].SetDataSource(dsDept.Tables[0]);
                    oEODDepartment.SetParameterValue("CompanyLogoPath", strCompanyLogoPath);   //PRIMEPOS-2386 01-Mar-2021 JY Added
                    ((CrystalDecisions.CrystalReports.Engine.TextObject)oEODDepartment.ReportDefinition.ReportObjects["txtHeader"]).Text = Configuration.CInfo.StoreName;
                    try
                    {
                        ((CrystalDecisions.CrystalReports.Engine.TextObject)oEODDepartment.ReportDefinition.ReportObjects["txtCloseDate"]).Text = "Close Date: " + Convert.ToDateTime(dsDept.Tables[0].Rows[0]["CloseDate"]).ToString("MM/dd/yyyy HH:mm:ss tt");
                    }
                    catch
                    {
                        ((CrystalDecisions.CrystalReports.Engine.TextObject)oEODDepartment.ReportDefinition.ReportObjects["txtCloseDate"]).Text = " ";
                    }
                    //Start : Addded By Amit Date 19 April 2011
                    if (Configuration.CInfo.PrintEODNo == false)
                        ((CrystalDecisions.CrystalReports.Engine.TextObject)oEODDepartment.ReportDefinition.ReportObjects["txtId"]).Text = " ";
                    else
                        ((CrystalDecisions.CrystalReports.Engine.TextObject)oEODDepartment.ReportDefinition.ReportObjects["txtId"]).Text = "EOD #: " + id.ToString();
                    //End
                    //PRIMEPOS-2983 02-Jul-2021 JY Modified
                    sql = " select eodh.closedate,case transtype when 'U-1' then 1 " +
                        " when 'U-2' then 1 when 'U-3' then 1 when 'U-4' then 1 when 'U-5' then 1 when 'U-6' then 1 " +
                        " when 'U-7' then 1 when 'U-C' then 1 when 'U-H' then 1 when 'U-B' then 1 when 'U-E' then 1 when 'PO' then 3 else 2 end as GroupType, " +
                        " sch.*,scd.*,upos.stationname , ISNULL((select Pt.PayTypeDesc  from Paytype PT where  SUBSTRING(scd.TransType, 3, len(scd.TransType)) = pt.PayTypeID),TransType) as PayTypeDesc" +
                        " from stationcloseheader sch,stationclosedetail scd ," +
                        " endofdayheader eodh,util_POSSet upos  " +
                        " where sch.stationid=upos.stationid and sch.stationcloseid=scd.stationcloseid and " +
                        " eodh.eodid=sch.eodid and eodh.eodid=" + id;

                    DataSet dsEODSt = oSearch.SearchData(sql);
                    DataTable dtEODSt = setGroupType(dsEODSt.Tables[0]);
                    oEODByStation.Database.Tables[0].SetDataSource(dtEODSt);
                    oEODByStation.SetParameterValue("CompanyLogoPath", strCompanyLogoPath);   //PRIMEPOS-2386 01-Mar-2021 JY Added
                    //oEODByStation.Database.Tables[0].SetDataSource(dsEODSt.Tables[0]);
                    ((CrystalDecisions.CrystalReports.Engine.TextObject)oEODByStation.ReportDefinition.ReportObjects["txtHeader"]).Text = Configuration.CInfo.StoreName;
                    ((CrystalDecisions.CrystalReports.Engine.TextObject)oEODByStation.ReportDefinition.ReportObjects["txtPharmacyAddress"]).Text = Configuration.CInfo.Address;
                    //Following line is Added By Shitaljit(QuicSolv) on 21 July 2011
                    ((CrystalDecisions.CrystalReports.Engine.TextObject)oEODByStation.ReportDefinition.ReportObjects["txtPharmacyCityStateZip"]).Text = Configuration.CInfo.City + ", " + Configuration.CInfo.State + " " + Configuration.CInfo.Zip;
                    ((CrystalDecisions.CrystalReports.Engine.TextObject)oEODByStation.ReportDefinition.ReportObjects["txtTelephoneNo"]).Text = Configuration.CInfo.Telephone;

                    //Start : Addded By Amit Date 20 April 2011
                    if (Configuration.CInfo.PrintEODNo == false)
                    {
                        ((CrystalDecisions.CrystalReports.Engine.TextObject)oEODByStation.ReportDefinition.ReportObjects["txtEODID"]).Text = " ";
                        ((CrystalDecisions.CrystalReports.Engine.TextObject)oEODByStation.ReportDefinition.ReportObjects["txtEOD"]).Text = " ";
                    }
                    else
                    {
                        ((CrystalDecisions.CrystalReports.Engine.TextObject)oEODByStation.ReportDefinition.ReportObjects["txtEODID"]).Text = id.ToString();
                    }
                    //End				

                    oEODTotal.Database.Tables[0].SetDataSource(dsEODSt.Tables[0]);
                    oEODTotal.SetParameterValue("CompanyLogoPath", strCompanyLogoPath);   //PRIMEPOS-2386 01-Mar-2021 JY Added
                    ((CrystalDecisions.CrystalReports.Engine.TextObject)oEODTotal.ReportDefinition.ReportObjects["txtHeader"]).Text = Configuration.CInfo.StoreName;
                    ((CrystalDecisions.CrystalReports.Engine.TextObject)oEODTotal.ReportDefinition.ReportObjects["txtPharmacyAddress"]).Text = Configuration.CInfo.Address;
                    //Following line is Added By Shitaljit(QuicSolv) on 21 July 2011
                    ((CrystalDecisions.CrystalReports.Engine.TextObject)oEODTotal.ReportDefinition.ReportObjects["txtPharmacyCityStateZip"]).Text = Configuration.CInfo.City + ", " + Configuration.CInfo.State + " " + Configuration.CInfo.Zip;
                    ((CrystalDecisions.CrystalReports.Engine.TextObject)oEODTotal.ReportDefinition.ReportObjects["txtTelephoneNo"]).Text = Configuration.CInfo.Telephone;

                    ((CrystalDecisions.CrystalReports.Engine.TextObject)oEODDepartment.ReportDefinition.ReportObjects["txtHeader"]).Text = Configuration.CInfo.StoreName;
                    ((CrystalDecisions.CrystalReports.Engine.TextObject)oEODDepartment.ReportDefinition.ReportObjects["txtPharmacyAddress"]).Text = Configuration.CInfo.Address;
                    //Following line is Added By Shitaljit(QuicSolv) on 21 July 2011
                    ((CrystalDecisions.CrystalReports.Engine.TextObject)oEODDepartment.ReportDefinition.ReportObjects["txtPharmacyCityStateZip"]).Text = Configuration.CInfo.City + ", " + Configuration.CInfo.State + " " + Configuration.CInfo.Zip;
                    ((CrystalDecisions.CrystalReports.Engine.TextObject)oEODDepartment.ReportDefinition.ReportObjects["txtTelephoneNo"]).Text = Configuration.CInfo.Telephone;

                    sql = "select IsNull(Sum(Amount),0) from POSTransaction pt inner join POSTransPayment PTP "
                        + " On (PT.TransID=PTP.TransID) Where IsIIASPayment=1 and pt.EODID=" + id;

                    string strIIASTotal = oSearch.SearchScalar(sql);
                    strIIASTotal = Configuration.convertNullToDecimal(strIIASTotal).ToString("######0.00");
                    ((CrystalDecisions.CrystalReports.Engine.TextObject)oEODTotal.ReportDefinition.ReportObjects["txtIIASAmount"]).Text = strIIASTotal;

                    //Added By Shitaljit for JIRA Ticket 286 to add RX , Taxable , Non-Taxable Item sales break down in EOD Total Report.
                    string strRxTotal = "0";
                    string strRXItemCount = "0";
                    string strTaxableItemTotal = "0";
                    string strTaxableItemCount = "0";
                    string strNonTaxableTotal = "0";
                    string strNonTaxableCount = "0";
                    string strTotalWithoutTax = "0";
                    string strTotalWithTax = "0";

                    DataSet dsRXitem = oEndOFDay.GetRxSalesDetails(id);
                    if (dsRXitem != null)
                    {
                        strRXItemCount = Configuration.convertNullToDecimal(dsRXitem.Tables[0].Rows[0][0].ToString()).ToString();
                        strRxTotal = Configuration.convertNullToDecimal(dsRXitem.Tables[0].Rows[0][1].ToString()).ToString("######0.00");
                    }

                    DataSet DsNonTaxableItem = oEndOFDay.GetNonTaxableItemsSalesDetails(id);
                    if (DsNonTaxableItem != null)
                    {
                        strNonTaxableCount = Configuration.convertNullToDecimal(DsNonTaxableItem.Tables[0].Rows[0][0].ToString()).ToString();
                        strNonTaxableTotal = Configuration.convertNullToDecimal(DsNonTaxableItem.Tables[0].Rows[0][1].ToString()).ToString("######0.00");
                    }

                    DataSet DsTaxableItem = oEndOFDay.GetTaxableItemSalesDetails(id);
                    if (DsTaxableItem != null)
                    {
                        strTaxableItemCount = Configuration.convertNullToDecimal(DsTaxableItem.Tables[0].Rows[0][0].ToString()).ToString();
                        strTaxableItemTotal = Configuration.convertNullToDecimal(DsTaxableItem.Tables[0].Rows[0][1].ToString()).ToString("######0.00");
                    }

                    strTotalWithoutTax = (Configuration.convertNullToDecimal(strRxTotal) + Configuration.convertNullToDecimal(strNonTaxableTotal) + Configuration.convertNullToDecimal(strTaxableItemTotal)).ToString("######0.00");
                    strTotalWithTax = (Configuration.convertNullToDecimal(strTotalWithoutTax) + Configuration.convertNullToDecimal(oEndOFDayData.SalesTax)).ToString("######0.00");

                    ((CrystalDecisions.CrystalReports.Engine.TextObject)oEODTotal.ReportDefinition.ReportObjects["RxSalesCount"]).Text = strRXItemCount;
                    ((CrystalDecisions.CrystalReports.Engine.TextObject)oEODTotal.ReportDefinition.ReportObjects["TotRxSalesTotal"]).Text = strRxTotal;
                    ((CrystalDecisions.CrystalReports.Engine.TextObject)oEODTotal.ReportDefinition.ReportObjects["NonTaxableSalesCount"]).Text = strNonTaxableCount;
                    ((CrystalDecisions.CrystalReports.Engine.TextObject)oEODTotal.ReportDefinition.ReportObjects["NonTaxableSalesTotal"]).Text = strNonTaxableTotal;
                    ((CrystalDecisions.CrystalReports.Engine.TextObject)oEODTotal.ReportDefinition.ReportObjects["TaxableSalesCount"]).Text = strTaxableItemCount;
                    ((CrystalDecisions.CrystalReports.Engine.TextObject)oEODTotal.ReportDefinition.ReportObjects["TaxableSalesTotal"]).Text = strTaxableItemTotal;
                    ((CrystalDecisions.CrystalReports.Engine.TextObject)oEODTotal.ReportDefinition.ReportObjects["TotalSalesWithoutTax"]).Text = strTotalWithoutTax;
                    ((CrystalDecisions.CrystalReports.Engine.TextObject)oEODTotal.ReportDefinition.ReportObjects["TotalSalesWithTax"]).Text = strTotalWithTax;
                    ((CrystalDecisions.CrystalReports.Engine.TextObject)oEODTotal.ReportDefinition.ReportObjects["TotalSaleTax"]).Text = oEndOFDayData.SalesTax.ToString("######0.00");

                    //END of added by shitaljit.

                    //Start : Addded By Amit Date 20 April 2011
                    if (Configuration.CInfo.PrintEODNo == false)
                    {
                        ((CrystalDecisions.CrystalReports.Engine.TextObject)oEODTotal.ReportDefinition.ReportObjects["txtEODID"]).Text = " ";
                        ((CrystalDecisions.CrystalReports.Engine.TextObject)oEODTotal.ReportDefinition.ReportObjects["txtEOD"]).Text = " ";
                    }
                    else
                    {
                        ((CrystalDecisions.CrystalReports.Engine.TextObject)oEODTotal.ReportDefinition.ReportObjects["txtEODID"]).Text = id.ToString();
                    }
                    //End

                    #region PRIMEPOS-2495 14-Mar-2018 JY Added logic to print transaction details
                    rptTransactionDetail1 orptTransactionDetail1 = new rptTransactionDetail1();
                    DataSet ds = GetTransactionDetails(id.ToString());
                    orptTransactionDetail1.SetDataSource(ds.Tables[0]);
                    orptTransactionDetail1.SetParameterValue("CompanyLogoPath", strCompanyLogoPath);   //PRIMEPOS-2386 01-Mar-2021 JY Added

                    #region PRIMEPOS-2480 26-Jun-2020 JY Added
                    try
                    {
                        ((CrystalDecisions.CrystalReports.Engine.TextObject)orptTransactionDetail1.ReportDefinition.ReportObjects["txtCloseDate"]).Text = "Close Date: " + Convert.ToDateTime(ds.Tables[0].Rows[0]["CloseDate"]).ToString("MM/dd/yyyy HH:mm:ss tt");
                    }
                    catch
                    {
                        ((CrystalDecisions.CrystalReports.Engine.TextObject)orptTransactionDetail1.ReportDefinition.ReportObjects["txtCloseDate"]).Text = " ";
                    }
                    if (Configuration.CInfo.PrintEODNo == false)
                        ((CrystalDecisions.CrystalReports.Engine.TextObject)orptTransactionDetail1.ReportDefinition.ReportObjects["txtId"]).Text = " ";
                    else
                        ((CrystalDecisions.CrystalReports.Engine.TextObject)orptTransactionDetail1.ReportDefinition.ReportObjects["txtId"]).Text = "EOD #: " + id.ToString();
                    #endregion

                    ((CrystalDecisions.CrystalReports.Engine.TextObject)orptTransactionDetail1.ReportDefinition.ReportObjects["txtHeader"]).Text = Configuration.CInfo.StoreName;
                    ((CrystalDecisions.CrystalReports.Engine.TextObject)orptTransactionDetail1.ReportDefinition.ReportObjects["txtPharmacyAddress"]).Text = Configuration.CInfo.Address;
                    ((CrystalDecisions.CrystalReports.Engine.TextObject)orptTransactionDetail1.ReportDefinition.ReportObjects["txtPharmacyCityStateZip"]).Text = Configuration.CInfo.City + ", " + Configuration.CInfo.State + " " + Configuration.CInfo.Zip;
                    ((CrystalDecisions.CrystalReports.Engine.TextObject)orptTransactionDetail1.ReportDefinition.ReportObjects["txtTelephoneNo"]).Text = Configuration.CInfo.Telephone;
                    #endregion

                    this.PrintReport(oEODDepartment, oEODByStation, oEODTotal, orptTransactionDetail1, PrintDirect, id.ToString());

                    //POS_Core_UI.Reports.ReportsUI.clsReports.PrintReport(oEODDepartment);
                    //POS_Core_UI.Reports.ReportsUI.clsReports.PrintReport(oEODByStation);
                    //POS_Core_UI.Reports.ReportsUI.clsReports.PrintReport(oEODTotal);
                }

                this.Cursor = System.Windows.Forms.Cursors.Default;

            }
            catch (Exception exp)
            {
                this.Cursor = System.Windows.Forms.Cursors.Default;
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }

        private void frmEndOfDay_Load(object sender, System.EventArgs e)
        {
            clsUIHelper.setColorSchecme(this);
            clsUIHelper.SetAppearance(this.grdDepartments);
            clsUIHelper.SetReadonlyRow(this.grdDepartments);
            //Start Added By Amit Date 16 May 2011
            clsUIHelper.SetAppearance(this.grdStations);
            clsUIHelper.SetReadonlyRow(this.grdStations);
            //End
            if (!bEditMode) this.txtEODDate.Text = Configuration.convertNullToString(DateTime.Now);    //PRIMEPOS-2480 26-Jun-2020 JY Added
            strCompanyLogoPath = Configuration.GetCompanyLogoPath(this);   //PRIMEPOS-2386 01-Mar-2021 JY Added
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
                //oDataSet.Tables[0].Columns[oCol.Key].DataType.
            }
        }

        private void frmEndOfDay_Activated(object sender, System.EventArgs e)
        {
            clsUIHelper.CurrentForm = this;
        }
        public DataTable setGroupType(DataTable dt)
        {
            int RowIndex = 0;
            try
            {
                foreach (DataRow oRow in dt.Rows)
                {
                    if (Configuration.convertNullToString(oRow["TransType"]).Contains("U") == true)
                    {
                        dt.Rows[RowIndex]["GroupType"] = "1";
                    }
                    RowIndex++;
                }

            }
            catch (Exception Ex)
            {

                throw (Ex);
            }
            return dt;
        }

        private void PrintReport(ReportClass pReport1, ReportClass pReport2, ReportClass pReport3, ReportClass pReport4, bool PrintDirect, string EODId)
        {
            if (PrintDirect == true)
            {
                System.Windows.Forms.PrintDialog pd = new PrintDialog();
                pd.PrinterSettings = new System.Drawing.Printing.PrinterSettings();
                pd.AllowSomePages = true;
                pd.AllowSelection = false;
                pd.ShowNetwork = true;

                #region PRIMEPOS-2996 23-Sep-2021 JY Added
                string strReportPrinter = string.Empty;
                try
                {
                    if (Configuration.CPOSSet.ReportPrinter.Trim() != "")
                    {
                        Configuration.SetReportPrinter(ref strReportPrinter);
                        if (strReportPrinter != "")
                            pd.PrinterSettings.PrinterName = strReportPrinter;
                    }
                    if (Configuration.CPOSSet.ReportPrinterPaperSource.Trim() != "")
                    {
                        System.Drawing.Printing.PrinterSettings oPrinterSettings = new System.Drawing.Printing.PrinterSettings();
                        foreach (System.Drawing.Printing.PaperSource ps in pd.PrinterSettings.PaperSources)
                        {
                            if (ps.SourceName.Trim().ToUpper() == Configuration.CPOSSet.ReportPrinterPaperSource.Trim().ToUpper())
                            {
                                pReport1.PrintOptions.CustomPaperSource = ps;
                                pReport2.PrintOptions.CustomPaperSource = ps;
                                pReport3.PrintOptions.CustomPaperSource = ps;
                                break;
                            }
                        }
                    }
                }
                catch { }
                #endregion

                if (pd.ShowDialog() != DialogResult.Cancel)
                {
                    if (pd.PrinterSettings.IsValid == true)
                    {
                        try
                        {
                            pReport1.PrintOptions.PrinterName = pd.PrinterSettings.PrinterName;  //PrinterSel.cbPrinter.Text;
                            pReport2.PrintOptions.PrinterName = pd.PrinterSettings.PrinterName;  //PrinterSel.cbPrinter.Text;
                            pReport3.PrintOptions.PrinterName = pd.PrinterSettings.PrinterName;	 //PrinterSel.cbPrinter.Text;
                            //pReport4.PrintOptions.PrinterName = pd.PrinterSettings.PrinterName;	 //PrinterSel.cbPrinter.Text;
                            pReport1.SetParameterValue("Currency", Configuration.CInfo.CurrencySymbol.ToString());
                            pReport2.SetParameterValue("Currency", Configuration.CInfo.CurrencySymbol.ToString());
                            pReport3.SetParameterValue("Currency", Configuration.CInfo.CurrencySymbol.ToString());
                            //pReport4.SetParameterValue("Currency", Configuration.CInfo.CurrencySymbol.ToString());
                            pReport1.PrintToPrinter(pd.PrinterSettings.Copies, pd.PrinterSettings.Collate, pd.PrinterSettings.FromPage, pd.PrinterSettings.ToPage);
                            pReport2.PrintToPrinter(pd.PrinterSettings.Copies, pd.PrinterSettings.Collate, pd.PrinterSettings.FromPage, pd.PrinterSettings.ToPage);
                            pReport3.PrintToPrinter(pd.PrinterSettings.Copies, pd.PrinterSettings.Collate, pd.PrinterSettings.FromPage, pd.PrinterSettings.ToPage);
                            //pReport4.PrintToPrinter(pd.PrinterSettings.Copies, pd.PrinterSettings.Collate, pd.PrinterSettings.FromPage, pd.PrinterSettings.ToPage);
                        }
                        catch (PrintException)
                        { }
                    }
                    else
                    {
                        throw (new Exception("Invalid printer name " + pd.PrinterSettings.PrinterName));
                    }
                }
            }
            else
            {
                Reports.ReportsUI.clsReports.ShowReport(pReport1);
                Reports.ReportsUI.clsReports.ShowReport(pReport2);
                Reports.ReportsUI.clsReports.ShowReport(pReport3);
                //Reports.ReportsUI.clsReports.ShowReport(pReport4);    //PRIMEPOS-2495 20-Mar-2018 JY we added different button to show this report

                #region Sprint-24 - PRIMEPOS-2363 28-Dec-2016 JY Added
                if (Configuration.CInfo.AutoEmailEODReport == true)
                {
                    List<ReportClass> lstReportClass = new List<ReportClass>();
                    lstReportClass.Add(pReport1);
                    lstReportClass.Add(pReport2);
                    lstReportClass.Add(pReport3);
                    lstReportClass.Add(pReport4);
                    new System.Threading.Thread(delegate ()
                    {
                        clsReports.EmailReport(lstReportClass, Configuration.CInfo.OwnersEmailId, "End Of Day Report - EOD #:" + EODId, "End Of Day Report", "File");
                    }).Start();
                }
                #endregion
            }
        }

        public void Edit(int EODID)
        {
            try
            {
                bEditMode = true;
                cmdProcess.Text = "&Print EOD Report";
                btnPrintTransactionDetails.Visible = true;
                cmdProcess.Width = btnPrintTransactionDetails.Width;
                cmdProcess.Left = cmdClose.Left - cmdProcess.Width - 5;
                btnPrintTransactionDetails.Left = cmdProcess.Left - btnPrintTransactionDetails.Width - 5;
                oEndOFDayData = oEndOFDay.FillData(EODID);
                Display();
                FillListView(oEndOFDayData);
                FillDepartmentListView(oEndOFDayData);
                //Added By Amit Date 13 May 2011
                FillStaionListView();
                PopulateTransDetail(oEndOFDayData.EODID);  //PRIMEPOS-2494 28-Feb-2018 JY Added
            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }

        private void frmEndOfDay_Resize(object sender, EventArgs e)
        {
            this.Left = clsPOSDBConstants.formLeft;
            this.Top = clsPOSDBConstants.formLeft;
        }

        public void EmailReport(string sEODId, string sSubject = "", string sBody = "", string sEmailAddress = "", bool bCalledFromScheduler = false)   //PRIMEPOS-3039 16-Dec-2021 JY Added
        {
            try
            {
                strCompanyLogoPath = Configuration.GetCompanyLogoPath(this);   //PRIMEPOS-2386 01-Mar-2021 JY Added

                rptEODDepartment oEODDepartment = new rptEODDepartment();
                rptEODByStation oEODByStation = new rptEODByStation();
                rptEODTotal oEODTotal = new rptEODTotal();
                Search oSearch = new Search();

                oEndOFDayData = oEndOFDay.FillData(Convert.ToInt32(sEODId));
                //PRIMEPOS-2386 01-Mar-2021 JY modified
                string sql = " SELECT HDR.CLOSEDATE ,T.EODID,dept.deptname, " +
                            " Sum(CASE T.TRANSTYPE WHEN 1 THEN ExtendedPrice ELSE 0 END) TotalSale " +
                            " , Sum(CASE T.TRANSTYPE WHEN 2 THEN ExtendedPrice ELSE 0 END) TotalReturn " +
                            " , Sum(td.Discount) TotalDiscount, Sum(TaxAmount) TotalTax, HDR.UserID AS Users " +
                            " FROM POSTransactionDetail TD join item i on (i.itemid=td.itemid) " +
                            " left join department dept on ( dept.deptid=i.departmentid) " +
                            " , POSTransaction T ,ENDOFDAYHEADER HDR WHERE TD.TransID = T.TransID AND T.EODID=HDR.EODID AND T.EODID =" + sEODId +
                            " GROUP BY dept.deptname,T.EODID,HDR.CLOSEDATE, HDR.UserID"  +
                            " order by dept.deptname ";

                DataSet dsDept = oSearch.SearchData(sql);
                oEODDepartment.Database.Tables[0].SetDataSource(dsDept.Tables[0]);
                oEODDepartment.SetParameterValue("CompanyLogoPath", strCompanyLogoPath);   //PRIMEPOS-2386 01-Mar-2021 JY Added
                ((CrystalDecisions.CrystalReports.Engine.TextObject)oEODDepartment.ReportDefinition.ReportObjects["txtHeader"]).Text = Configuration.CInfo.StoreName;
                try
                {
                    ((CrystalDecisions.CrystalReports.Engine.TextObject)oEODDepartment.ReportDefinition.ReportObjects["txtCloseDate"]).Text = "Close Date: " + Convert.ToDateTime(dsDept.Tables[0].Rows[0]["CloseDate"]).ToString("MM/dd/yyyy HH:mm:ss tt");
                }
                catch
                {
                    ((CrystalDecisions.CrystalReports.Engine.TextObject)oEODDepartment.ReportDefinition.ReportObjects["txtCloseDate"]).Text = " ";
                }
                if (Configuration.CInfo.PrintEODNo == false)
                    ((CrystalDecisions.CrystalReports.Engine.TextObject)oEODDepartment.ReportDefinition.ReportObjects["txtId"]).Text = " ";
                else
                    ((CrystalDecisions.CrystalReports.Engine.TextObject)oEODDepartment.ReportDefinition.ReportObjects["txtId"]).Text = "EOD #: " + sEODId.ToString();

                //PRIMEPOS-2983 02-Jul-2021 JY Modified
                sql = " select eodh.closedate,case transtype when 'U-1' then 1 " +
                    " when 'U-2' then 1 when 'U-3' then 1 when 'U-4' then 1 when 'U-5' then 1 when 'U-6' then 1 " +
                    " when 'U-7' then 1 when 'U-C' then 1 when 'U-H' then 1 when 'U-B' then 1 when 'U-E' then 1 when 'PO' then 3 else 2 end as GroupType, " +
                    " sch.*,scd.*,upos.stationname , ISNULL((select Pt.PayTypeDesc  from Paytype PT where  SUBSTRING(scd.TransType, 3, len(scd.TransType)) = pt.PayTypeID),TransType) as PayTypeDesc" +
                    " from stationcloseheader sch,stationclosedetail scd ," +
                    " endofdayheader eodh,util_POSSet upos  " +
                    " where sch.stationid=upos.stationid and sch.stationcloseid=scd.stationcloseid and " +
                    " eodh.eodid=sch.eodid and eodh.eodid=" + sEODId;

                DataSet dsEODSt = oSearch.SearchData(sql);
                DataTable dtEODSt = setGroupType(dsEODSt.Tables[0]);
                oEODByStation.Database.Tables[0].SetDataSource(dtEODSt);
                oEODByStation.SetParameterValue("CompanyLogoPath", strCompanyLogoPath);   //PRIMEPOS-2386 01-Mar-2021 JY Added
                ((CrystalDecisions.CrystalReports.Engine.TextObject)oEODByStation.ReportDefinition.ReportObjects["txtHeader"]).Text = Configuration.CInfo.StoreName;
                ((CrystalDecisions.CrystalReports.Engine.TextObject)oEODByStation.ReportDefinition.ReportObjects["txtPharmacyAddress"]).Text = Configuration.CInfo.Address;
                ((CrystalDecisions.CrystalReports.Engine.TextObject)oEODByStation.ReportDefinition.ReportObjects["txtPharmacyCityStateZip"]).Text = Configuration.CInfo.City + ", " + Configuration.CInfo.State + " " + Configuration.CInfo.Zip;
                ((CrystalDecisions.CrystalReports.Engine.TextObject)oEODByStation.ReportDefinition.ReportObjects["txtTelephoneNo"]).Text = Configuration.CInfo.Telephone;

                if (Configuration.CInfo.PrintEODNo == false)
                {
                    ((CrystalDecisions.CrystalReports.Engine.TextObject)oEODByStation.ReportDefinition.ReportObjects["txtEODID"]).Text = " ";
                    ((CrystalDecisions.CrystalReports.Engine.TextObject)oEODByStation.ReportDefinition.ReportObjects["txtEOD"]).Text = " ";
                }
                else
                {
                    ((CrystalDecisions.CrystalReports.Engine.TextObject)oEODByStation.ReportDefinition.ReportObjects["txtEODID"]).Text = sEODId;
                }

                oEODTotal.Database.Tables[0].SetDataSource(dsEODSt.Tables[0]);
                oEODTotal.SetParameterValue("CompanyLogoPath", strCompanyLogoPath);   //PRIMEPOS-2386 01-Mar-2021 JY Added
                ((CrystalDecisions.CrystalReports.Engine.TextObject)oEODTotal.ReportDefinition.ReportObjects["txtHeader"]).Text = Configuration.CInfo.StoreName;
                ((CrystalDecisions.CrystalReports.Engine.TextObject)oEODTotal.ReportDefinition.ReportObjects["txtPharmacyAddress"]).Text = Configuration.CInfo.Address;
                ((CrystalDecisions.CrystalReports.Engine.TextObject)oEODTotal.ReportDefinition.ReportObjects["txtPharmacyCityStateZip"]).Text = Configuration.CInfo.City + ", " + Configuration.CInfo.State + " " + Configuration.CInfo.Zip;
                ((CrystalDecisions.CrystalReports.Engine.TextObject)oEODTotal.ReportDefinition.ReportObjects["txtTelephoneNo"]).Text = Configuration.CInfo.Telephone;

                ((CrystalDecisions.CrystalReports.Engine.TextObject)oEODDepartment.ReportDefinition.ReportObjects["txtHeader"]).Text = Configuration.CInfo.StoreName;
                ((CrystalDecisions.CrystalReports.Engine.TextObject)oEODDepartment.ReportDefinition.ReportObjects["txtPharmacyAddress"]).Text = Configuration.CInfo.Address;
                ((CrystalDecisions.CrystalReports.Engine.TextObject)oEODDepartment.ReportDefinition.ReportObjects["txtPharmacyCityStateZip"]).Text = Configuration.CInfo.City + ", " + Configuration.CInfo.State + " " + Configuration.CInfo.Zip;
                ((CrystalDecisions.CrystalReports.Engine.TextObject)oEODDepartment.ReportDefinition.ReportObjects["txtTelephoneNo"]).Text = Configuration.CInfo.Telephone;

                sql = "select IsNull(Sum(Amount),0) from POSTransaction pt inner join POSTransPayment PTP "
                    + " On (PT.TransID=PTP.TransID) Where IsIIASPayment=1 and pt.EODID=" + sEODId;

                string strIIASTotal = oSearch.SearchScalar(sql);
                strIIASTotal = Configuration.convertNullToDecimal(strIIASTotal).ToString("######0.00");
                ((CrystalDecisions.CrystalReports.Engine.TextObject)oEODTotal.ReportDefinition.ReportObjects["txtIIASAmount"]).Text = strIIASTotal;

                string strRxTotal = "0";
                string strRXItemCount = "0";
                string strTaxableItemTotal = "0";
                string strTaxableItemCount = "0";
                string strNonTaxableTotal = "0";
                string strNonTaxableCount = "0";
                string strTotalWithoutTax = "0";
                string strTotalWithTax = "0";

                DataSet dsRXitem = oEndOFDay.GetRxSalesDetails(Convert.ToInt32(sEODId));
                if (dsRXitem != null)
                {
                    strRXItemCount = Configuration.convertNullToDecimal(dsRXitem.Tables[0].Rows[0][0].ToString()).ToString();
                    strRxTotal = Configuration.convertNullToDecimal(dsRXitem.Tables[0].Rows[0][1].ToString()).ToString("######0.00");
                }

                DataSet DsNonTaxableItem = oEndOFDay.GetNonTaxableItemsSalesDetails(Convert.ToInt32(sEODId));
                if (DsNonTaxableItem != null)
                {
                    strNonTaxableCount = Configuration.convertNullToDecimal(DsNonTaxableItem.Tables[0].Rows[0][0].ToString()).ToString();
                    strNonTaxableTotal = Configuration.convertNullToDecimal(DsNonTaxableItem.Tables[0].Rows[0][1].ToString()).ToString("######0.00");
                }

                DataSet DsTaxableItem = oEndOFDay.GetTaxableItemSalesDetails(Convert.ToInt32(sEODId));
                if (DsTaxableItem != null)
                {
                    strTaxableItemCount = Configuration.convertNullToDecimal(DsTaxableItem.Tables[0].Rows[0][0].ToString()).ToString();
                    strTaxableItemTotal = Configuration.convertNullToDecimal(DsTaxableItem.Tables[0].Rows[0][1].ToString()).ToString("######0.00");
                }

                strTotalWithoutTax = (Configuration.convertNullToDecimal(strRxTotal) + Configuration.convertNullToDecimal(strNonTaxableTotal) + Configuration.convertNullToDecimal(strTaxableItemTotal)).ToString("######0.00");
                strTotalWithTax = (Configuration.convertNullToDecimal(strTotalWithoutTax) + Configuration.convertNullToDecimal(oEndOFDayData.SalesTax)).ToString("######0.00");

                ((CrystalDecisions.CrystalReports.Engine.TextObject)oEODTotal.ReportDefinition.ReportObjects["RxSalesCount"]).Text = strRXItemCount;
                ((CrystalDecisions.CrystalReports.Engine.TextObject)oEODTotal.ReportDefinition.ReportObjects["TotRxSalesTotal"]).Text = strRxTotal;
                ((CrystalDecisions.CrystalReports.Engine.TextObject)oEODTotal.ReportDefinition.ReportObjects["NonTaxableSalesCount"]).Text = strNonTaxableCount;
                ((CrystalDecisions.CrystalReports.Engine.TextObject)oEODTotal.ReportDefinition.ReportObjects["NonTaxableSalesTotal"]).Text = strNonTaxableTotal;
                ((CrystalDecisions.CrystalReports.Engine.TextObject)oEODTotal.ReportDefinition.ReportObjects["TaxableSalesCount"]).Text = strTaxableItemCount;
                ((CrystalDecisions.CrystalReports.Engine.TextObject)oEODTotal.ReportDefinition.ReportObjects["TaxableSalesTotal"]).Text = strTaxableItemTotal;
                ((CrystalDecisions.CrystalReports.Engine.TextObject)oEODTotal.ReportDefinition.ReportObjects["TotalSalesWithoutTax"]).Text = strTotalWithoutTax;
                ((CrystalDecisions.CrystalReports.Engine.TextObject)oEODTotal.ReportDefinition.ReportObjects["TotalSalesWithTax"]).Text = strTotalWithTax;
                ((CrystalDecisions.CrystalReports.Engine.TextObject)oEODTotal.ReportDefinition.ReportObjects["TotalSaleTax"]).Text = oEndOFDayData.SalesTax.ToString("######0.00");

                if (Configuration.CInfo.PrintEODNo == false)
                {
                    ((CrystalDecisions.CrystalReports.Engine.TextObject)oEODTotal.ReportDefinition.ReportObjects["txtEODID"]).Text = " ";
                    ((CrystalDecisions.CrystalReports.Engine.TextObject)oEODTotal.ReportDefinition.ReportObjects["txtEOD"]).Text = " ";
                }
                else
                {
                    ((CrystalDecisions.CrystalReports.Engine.TextObject)oEODTotal.ReportDefinition.ReportObjects["txtEODID"]).Text = sEODId;
                }

                #region PRIMEPOS-2495 14-Mar-2018 JY Added logic to print transaction details
                rptTransactionDetail1 orptTransactionDetail1 = new rptTransactionDetail1();
                DataSet ds = GetTransactionDetails(sEODId);
                orptTransactionDetail1.SetDataSource(ds.Tables[0]);
                orptTransactionDetail1.SetParameterValue("CompanyLogoPath", strCompanyLogoPath);   //PRIMEPOS-2386 01-Mar-2021 JY Added

                #region PRIMEPOS-2480 26-Jun-2020 JY Added
                try
                {
                    ((CrystalDecisions.CrystalReports.Engine.TextObject)orptTransactionDetail1.ReportDefinition.ReportObjects["txtCloseDate"]).Text = "Close Date: " + Convert.ToDateTime(ds.Tables[0].Rows[0]["CloseDate"]).ToString("MM/dd/yyyy HH:mm:ss tt");
                }
                catch
                {
                    ((CrystalDecisions.CrystalReports.Engine.TextObject)orptTransactionDetail1.ReportDefinition.ReportObjects["txtCloseDate"]).Text = " ";
                }
                if (Configuration.CInfo.PrintEODNo == false)
                    ((CrystalDecisions.CrystalReports.Engine.TextObject)orptTransactionDetail1.ReportDefinition.ReportObjects["txtId"]).Text = " ";
                else
                    ((CrystalDecisions.CrystalReports.Engine.TextObject)orptTransactionDetail1.ReportDefinition.ReportObjects["txtId"]).Text = "EOD #: " + sEODId;
                #endregion

                ((CrystalDecisions.CrystalReports.Engine.TextObject)orptTransactionDetail1.ReportDefinition.ReportObjects["txtHeader"]).Text = Configuration.CInfo.StoreName;
                ((CrystalDecisions.CrystalReports.Engine.TextObject)orptTransactionDetail1.ReportDefinition.ReportObjects["txtPharmacyAddress"]).Text = Configuration.CInfo.Address;
                ((CrystalDecisions.CrystalReports.Engine.TextObject)orptTransactionDetail1.ReportDefinition.ReportObjects["txtPharmacyCityStateZip"]).Text = Configuration.CInfo.City + ", " + Configuration.CInfo.State + " " + Configuration.CInfo.Zip;
                ((CrystalDecisions.CrystalReports.Engine.TextObject)orptTransactionDetail1.ReportDefinition.ReportObjects["txtTelephoneNo"]).Text = Configuration.CInfo.Telephone;

                orptTransactionDetail1.SetParameterValue("Currency", Configuration.CInfo.CurrencySymbol.ToString());
                #endregion

                oEODDepartment.SetParameterValue("Currency", Configuration.CInfo.CurrencySymbol.ToString());
                oEODByStation.SetParameterValue("Currency", Configuration.CInfo.CurrencySymbol.ToString());
                oEODTotal.SetParameterValue("Currency", Configuration.CInfo.CurrencySymbol.ToString());

                List<ReportClass> lstReportClass = new List<ReportClass>();
                lstReportClass.Add(oEODDepartment);
                lstReportClass.Add(oEODByStation);
                lstReportClass.Add(oEODTotal);
                lstReportClass.Add(orptTransactionDetail1);
                if (bCalledFromScheduler)   //PRIMEPOS-3039 16-Dec-2021 JY Added
                {
                    clsReports.EmailReport(lstReportClass, sEmailAddress, sSubject + " - EOD#: " + sEODId, sBody, "File", false, true);
                }
                else
                {
                    new System.Threading.Thread(delegate ()
                    {
                        clsReports.EmailReport(lstReportClass, Configuration.CInfo.OwnersEmailId, "End Of Day Report - EOD #:" + sEODId, "End Of Day Report", "File");
                    }).Start();
                }
            }
            catch (Exception exp)
            {
                this.Cursor = System.Windows.Forms.Cursors.Default;
                if (!bCalledFromScheduler)
                    clsUIHelper.ShowErrorMsg(exp.Message);
                else
                    logger.Fatal(exp, "EmailReport(...)");
            }
        }

        #region PRIMEPOS-2494 28-Feb-2018 JY Added
        private void PopulateTransDetail(string EODID)
        {
            try
            {
                SearchSvr oSearchSvr = new SearchSvr();
                DataSet oDataSet = new DataSet();

                string strTransSQL = "SELECT DISTINCT PT.StClosedID, PT.TransID, PT.UserID, PT.TransDate, Case PT.TransType when 1 Then 'Sale' when 2 Then 'Return'  WHEN 3 THEN 'ROA' end as TransType,  PT.StationID, ps.StationName " +
                            " FROM postransaction PT " +
                            " INNER JOIN util_POSSet ps ON ps.stationid = PT.stationid " +
                            " WHERE PT.EODID = " + EODID + " Order By PT.StClosedID, PT.TransID";

                string strTransDetailsSQL = "select PT.TransID, I.ItemID, PTD.Qty, PTD.Discount, PTD.ItemDescription as Description, PTD.Price, PTD.TaxAmount, PTD.ExtendedPrice " +
                            " from postransaction PT " +
                            " INNER JOIN POSTransactionDetail PTD ON PT.TransID=PTD.TransID " +
                            " INNER JOIN Item I ON I.ItemID=PTD.ItemID " +
                            " WHERE PT.EODID = " + EODID + " Order By PTD.ItemDescription";

                string strPaymentSQL = "select PT.TransID, PTY.PayTypeDesc as [Payment Type], PTP.Amount, case CHARINDEX('|',isnull(PTP.refno,'')) " +
                            " when 0 then PTP.refno else '******'+right(rtrim(left(PTP.refno,CHARINDEX('|',PTP.refno)-1)) ,4) End as [Ref No.], PTP.CustomerSign, PTP.BinarySign, PTP.SigType " +
                            " from POSTransaction PT " +
                            " INNER JOIN postranspayment PTP ON PT.TransID = PTP.TransID " +
                            " INNER JOIN PayType PTY ON PTP.TransTypeCode = PTY.PayTypeID " +
                            " INNER JOIN Customer Cus ON PT.CustomerID = Cus.CustomerID " +
                            " WHERE PT.EODID = " + EODID + " Order By PTY.PayTypeDesc";

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
                string strSQL = "SELECT ISNULL(MIN(PT.TransID),0) AS MinTransID, ISNULL(MAX(PT.TransID),0) AS MaxTransID FROM postransaction PT WHERE PT.EODID = " + EODID;
                DataSet ds = oSearchSvr.Search(strSQL);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    lblTransRange.Text = "Transaction# from " + ds.Tables[0].Rows[0]["MinTransID"].ToString() + " to " + ds.Tables[0].Rows[0]["MaxTransID"].ToString();
                }
                #endregion
            }
            catch (Exception exp)
            {
                if (!bCalledFromScheduler)  //PRIMEPOS-3039 16-Dec-2021 JY Modified
                    clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }
        #endregion

        #region PRIMEPOS-2495 20-Mar-2018 JY Added
        private void btnPrintTransactionDetails_Click(object sender, EventArgs e)
        {
            rptTransactionDetail1 orptTransactionDetail1 = new rptTransactionDetail1();
            Int32 id = Convert.ToInt32(this.txtEndOfDayNo.Text);
            DataSet ds = GetTransactionDetails(id.ToString());
            orptTransactionDetail1.SetDataSource(ds.Tables[0]);
            orptTransactionDetail1.SetParameterValue("CompanyLogoPath", strCompanyLogoPath);   //PRIMEPOS-2386 01-Mar-2021 JY Added

            #region PRIMEPOS-2480 26-Jun-2020 JY Added
            try
            {
                ((CrystalDecisions.CrystalReports.Engine.TextObject)orptTransactionDetail1.ReportDefinition.ReportObjects["txtCloseDate"]).Text = "Close Date: " + Convert.ToDateTime(ds.Tables[0].Rows[0]["CloseDate"]).ToString("MM/dd/yyyy HH:mm:ss tt");
            }
            catch
            {
                ((CrystalDecisions.CrystalReports.Engine.TextObject)orptTransactionDetail1.ReportDefinition.ReportObjects["txtCloseDate"]).Text = " ";
            }
            if (Configuration.CInfo.PrintEODNo == false)
                ((CrystalDecisions.CrystalReports.Engine.TextObject)orptTransactionDetail1.ReportDefinition.ReportObjects["txtId"]).Text = " ";
            else
                ((CrystalDecisions.CrystalReports.Engine.TextObject)orptTransactionDetail1.ReportDefinition.ReportObjects["txtId"]).Text = "EOD #: " + id.ToString();
            #endregion

            ((CrystalDecisions.CrystalReports.Engine.TextObject)orptTransactionDetail1.ReportDefinition.ReportObjects["txtHeader"]).Text = Configuration.CInfo.StoreName;
            ((CrystalDecisions.CrystalReports.Engine.TextObject)orptTransactionDetail1.ReportDefinition.ReportObjects["txtPharmacyAddress"]).Text = Configuration.CInfo.Address;
            ((CrystalDecisions.CrystalReports.Engine.TextObject)orptTransactionDetail1.ReportDefinition.ReportObjects["txtPharmacyCityStateZip"]).Text = Configuration.CInfo.City + ", " + Configuration.CInfo.State + " " + Configuration.CInfo.Zip;
            ((CrystalDecisions.CrystalReports.Engine.TextObject)orptTransactionDetail1.ReportDefinition.ReportObjects["txtTelephoneNo"]).Text = Configuration.CInfo.Telephone;

            System.Windows.Forms.PrintDialog pd = new PrintDialog();
            pd.PrinterSettings = new System.Drawing.Printing.PrinterSettings();
            pd.AllowSomePages = true;
            pd.AllowSelection = false;
            pd.ShowNetwork = true;

            #region PRIMEPOS-2996 23-Sep-2021 JY Added
            string strReportPrinter = string.Empty;
            try
            {
                if (Configuration.CPOSSet.ReportPrinter.Trim() != "")
                {
                    Configuration.SetReportPrinter(ref strReportPrinter);
                    if (strReportPrinter != "")
                        pd.PrinterSettings.PrinterName = strReportPrinter;
                }
                if (Configuration.CPOSSet.ReportPrinterPaperSource.Trim() != "")
                {
                    System.Drawing.Printing.PrinterSettings oPrinterSettings = new System.Drawing.Printing.PrinterSettings();
                    foreach (System.Drawing.Printing.PaperSource ps in pd.PrinterSettings.PaperSources)
                    {
                        if (ps.SourceName.Trim().ToUpper() == Configuration.CPOSSet.ReportPrinterPaperSource.Trim().ToUpper())
                        {
                            orptTransactionDetail1.PrintOptions.CustomPaperSource = ps;
                            break;
                        }
                    }
                }
            }
            catch { }
            #endregion

            if (pd.ShowDialog() != DialogResult.Cancel)
            {
                if (pd.PrinterSettings.IsValid == true)
                {
                    try
                    {
                        orptTransactionDetail1.PrintOptions.PrinterName = pd.PrinterSettings.PrinterName;  //PrinterSel.cbPrinter.Text;
                        orptTransactionDetail1.SetParameterValue("Currency", Configuration.CInfo.CurrencySymbol.ToString());
                        orptTransactionDetail1.PrintToPrinter(pd.PrinterSettings.Copies, pd.PrinterSettings.Collate, pd.PrinterSettings.FromPage, pd.PrinterSettings.ToPage);
                    }
                    catch (PrintException)
                    {
                    }
                }
                else
                {
                    throw (new Exception("Invalid printer name " + pd.PrinterSettings.PrinterName));
                }
            }
        }

        private DataSet GetTransactionDetails(string sEODId)
        {
            //PRIMEPOS-2386 01-Mar-2021 JY modified
            string strSQL = "SELECT a.TransID, a.TransDate, Case a.TransType when 1 Then 'Sale' when 2 Then 'Return' WHEN 3 THEN 'ROA' end as TransType, a.UserID, a.StationID, a.TotalPaid, a.StClosedID, a.EODID, b.CloseDate, b.UserID As Users"
                            + " FROM POSTransaction a INNER JOIN EndOfDayHeader b ON a.EODID = b.EODID"
                            + " WHERE a.EODID = " + sEODId + " ORDER BY a.TransID";
            DataSet ds = clsReports.GetReportSource(strSQL);
            return ds;
        }
        #endregion

        #region PRIMEPOS-3039 16-Dec-2021 JY Added
        public usrDateRangeParams customControl;
        private bool bCalledFromScheduler = false;
        private string strMessage = string.Empty;
        //public bool bSendPrint = true;

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
                cmdProcess_Click(null, null);
                if (oEndOFDayData != null && oEndOFDayData.EODID != null)
                    filePath = oEndOFDayData.EODID;
                sNoOfRecordAffect = strMessage;
            }
            catch(Exception Ex)
            { }
            finally
            {
                bCalledFromScheduler = false;
                strMessage = string.Empty;
            }
            return true;
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
