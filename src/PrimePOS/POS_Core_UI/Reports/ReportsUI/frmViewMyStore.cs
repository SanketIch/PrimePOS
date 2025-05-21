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
using POS_Core.DataAccess;
//using POS_Core_UI.Reports.ReportsUI;
using Infragistics.UltraChart.Resources.Appearance;
using POS_Core.Resources;

namespace POS_Core_UI
{
	/// <summary>
	/// Summary description for frmVendorSearch.
	/// </summary>
	public class frmViewMyStore : System.Windows.Forms.Form
	{

		//private Infragistics.Win.UltraWinChart.UltraChart oChart=null;


		private Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage ultraTabSharedControlsPage1;
		private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl1;
		private Infragistics.Win.UltraWinDataSource.UltraDataSource ultraDataSource1;
		private Infragistics.Win.Misc.UltraButton btnSearch;
		private Infragistics.Win.Misc.UltraButton btnClose;
		private Infragistics.Win.Misc.UltraLabel lbl2;
		private Infragistics.Win.Misc.UltraLabel lbl1;
		private Infragistics.Win.UltraWinTabControl.UltraTabControl tabMain;
		private Infragistics.Win.UltraWinSchedule.UltraCalendarCombo clTo1;
		private Infragistics.Win.UltraWinSchedule.UltraCalendarCombo clFrom1;
		private Infragistics.Win.UltraWinSchedule.UltraCalendarCombo clFrom;
		private Infragistics.Win.UltraWinSchedule.UltraCalendarCombo clTo;
		private Infragistics.Win.UltraWinChart.UltraChart chtSalesTrend;
		private Infragistics.Win.UltraWinChart.UltraChart chtSaleByDepartment;
		private Infragistics.Win.UltraWinChart.UltraChart chtSaleByEmp;
		private Infragistics.Win.UltraWinChart.UltraChart chtSaleByRegister;
		private Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage ultraTabSharedControlsPage2;
		private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl2;
		private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl3;
		private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl4;
		private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl5;
		private Infragistics.Win.UltraWinEditors.UltraNumericEditor numTopDept;
		private Infragistics.Win.UltraWinEditors.UltraNumericEditor numTopItem;
		private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl6;
		private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl7;
		private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl8;
		private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl9;
		private Infragistics.Win.Misc.UltraLabel ultraLabel1;
		private Infragistics.Win.Misc.UltraLabel ultraLabel2;
		private Infragistics.Win.UltraWinChart.UltraChart chtTopDept;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.GroupBox groupBox2;
		private Infragistics.Win.UltraWinChart.UltraChart chtTopItems;
		private Infragistics.Win.Misc.UltraLabel ultraLabel3;
		private Infragistics.Win.Misc.UltraLabel ultraLabel4;
		private Infragistics.Win.Misc.UltraLabel ultraLabel5;
		private Infragistics.Win.Misc.UltraLabel ultraLabel6;
		private Infragistics.Win.UltraWinEditors.UltraNumericEditor numAllSTransAmount;
		private Infragistics.Win.UltraWinEditors.UltraNumericEditor numAllSTransNo;
		private Infragistics.Win.UltraWinEditors.UltraNumericEditor numAllRTransNo;
		private Infragistics.Win.UltraWinEditors.UltraNumericEditor numAllRTransAmount;
		private System.Windows.Forms.GroupBox groupBox4;
		private Infragistics.Win.UltraWinEditors.UltraNumericEditor numTRTransNo;
		private Infragistics.Win.Misc.UltraLabel ultraLabel7;
		private Infragistics.Win.UltraWinEditors.UltraNumericEditor numTRTransAmount;
		private Infragistics.Win.Misc.UltraLabel ultraLabel8;
		private Infragistics.Win.UltraWinEditors.UltraNumericEditor numTSTransNo;
		private Infragistics.Win.Misc.UltraLabel ultraLabel9;
		private Infragistics.Win.UltraWinEditors.UltraNumericEditor numTSTransAmount;
		private Infragistics.Win.Misc.UltraLabel ultraLabel10;
		private System.Windows.Forms.GroupBox groupBox6;
		private Infragistics.Win.UltraWinGrid.UltraGrid grdPaymentAll;
		private Infragistics.Win.UltraWinGrid.UltraGrid grdPaymentToday;
		private Infragistics.Win.UltraWinEditors.UltraPictureBox picScroll;
		//private Infragistics.UltraChart.Resources.Editor.ChartTypeCtrl chtType;
		private Infragistics.Win.Misc.UltraPopupControlContainer popControl;
		private Infragistics.Win.UltraWinTabControl.UltraTabControl tbData;
		private System.Windows.Forms.GroupBox grpTransTypeAll;
		private System.Windows.Forms.GroupBox grpPayTypeAll;
        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl10;
        private Infragistics.Win.UltraWinTabControl.UltraTabControl SaleHistoryTab;
        private Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage ultraTabSharedControlsPage3;
        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl13;
        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl14;
        private Infragistics.Win.UltraWinDataSource.UltraDataSource ultraDataSource2;
        private Infragistics.Win.Misc.UltraButton btnByTransCount;
        private Infragistics.Win.Misc.UltraButton btnByAmount;
        private Infragistics.Win.UltraWinChart.UltraChart chartSaleHistory;
        private UltraGrid gdSalesHistory;
		private System.ComponentModel.IContainer components;

		public frmViewMyStore()
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
            Infragistics.UltraChart.Resources.Appearance.PaintElement paintElement1 = new Infragistics.UltraChart.Resources.Appearance.PaintElement();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmViewMyStore));
            Infragistics.UltraChart.Resources.Appearance.GradientEffect gradientEffect1 = new Infragistics.UltraChart.Resources.Appearance.GradientEffect();
            Infragistics.Win.UltraWinSchedule.CalendarCombo.DateButton dateButton1 = new Infragistics.Win.UltraWinSchedule.CalendarCombo.DateButton();
            Infragistics.Win.UltraWinSchedule.CalendarCombo.DateButton dateButton2 = new Infragistics.Win.UltraWinSchedule.CalendarCombo.DateButton();
            Infragistics.Win.Appearance appearance17 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance18 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance19 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance20 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance21 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance22 = new Infragistics.Win.Appearance();
            Infragistics.UltraChart.Resources.Appearance.PaintElement paintElement2 = new Infragistics.UltraChart.Resources.Appearance.PaintElement();
            Infragistics.UltraChart.Resources.Appearance.ThreeDEffect threeDEffect1 = new Infragistics.UltraChart.Resources.Appearance.ThreeDEffect();
            Infragistics.UltraChart.Resources.Appearance.GradientEffect gradientEffect2 = new Infragistics.UltraChart.Resources.Appearance.GradientEffect();
            Infragistics.UltraChart.Resources.Appearance.ShadowEffect shadowEffect1 = new Infragistics.UltraChart.Resources.Appearance.ShadowEffect();
            Infragistics.UltraChart.Resources.Appearance.PaintElement paintElement3 = new Infragistics.UltraChart.Resources.Appearance.PaintElement();
            Infragistics.UltraChart.Resources.Appearance.ColumnChartAppearance columnChartAppearance1 = new Infragistics.UltraChart.Resources.Appearance.ColumnChartAppearance();
            Infragistics.UltraChart.Resources.Appearance.ThreeDEffect threeDEffect2 = new Infragistics.UltraChart.Resources.Appearance.ThreeDEffect();
            Infragistics.UltraChart.Resources.Appearance.GradientEffect gradientEffect3 = new Infragistics.UltraChart.Resources.Appearance.GradientEffect();
            Infragistics.UltraChart.Resources.Appearance.PaintElement paintElement4 = new Infragistics.UltraChart.Resources.Appearance.PaintElement();
            Infragistics.UltraChart.Resources.Appearance.ThreeDEffect threeDEffect3 = new Infragistics.UltraChart.Resources.Appearance.ThreeDEffect();
            Infragistics.UltraChart.Resources.Appearance.GradientEffect gradientEffect4 = new Infragistics.UltraChart.Resources.Appearance.GradientEffect();
            Infragistics.UltraChart.Resources.Appearance.ShadowEffect shadowEffect2 = new Infragistics.UltraChart.Resources.Appearance.ShadowEffect();
            Infragistics.UltraChart.Resources.Appearance.PieChartAppearance pieChartAppearance1 = new Infragistics.UltraChart.Resources.Appearance.PieChartAppearance();
            Infragistics.UltraChart.Resources.Appearance.PaintElement paintElement5 = new Infragistics.UltraChart.Resources.Appearance.PaintElement();
            Infragistics.UltraChart.Resources.Appearance.ThreeDEffect threeDEffect4 = new Infragistics.UltraChart.Resources.Appearance.ThreeDEffect();
            Infragistics.UltraChart.Resources.Appearance.GradientEffect gradientEffect5 = new Infragistics.UltraChart.Resources.Appearance.GradientEffect();
            Infragistics.UltraChart.Resources.Appearance.PieChartAppearance pieChartAppearance2 = new Infragistics.UltraChart.Resources.Appearance.PieChartAppearance();
            Infragistics.UltraChart.Resources.Appearance.View3DAppearance view3DAppearance1 = new Infragistics.UltraChart.Resources.Appearance.View3DAppearance();
            Infragistics.Win.Appearance appearance23 = new Infragistics.Win.Appearance();
            Infragistics.UltraChart.Resources.Appearance.PaintElement paintElement6 = new Infragistics.UltraChart.Resources.Appearance.PaintElement();
            Infragistics.UltraChart.Resources.Appearance.ColumnChartAppearance columnChartAppearance2 = new Infragistics.UltraChart.Resources.Appearance.ColumnChartAppearance();
            Infragistics.UltraChart.Resources.Appearance.ThreeDEffect threeDEffect5 = new Infragistics.UltraChart.Resources.Appearance.ThreeDEffect();
            Infragistics.UltraChart.Resources.Appearance.GradientEffect gradientEffect6 = new Infragistics.UltraChart.Resources.Appearance.GradientEffect();
            Infragistics.UltraChart.Resources.Appearance.PaintElement paintElement7 = new Infragistics.UltraChart.Resources.Appearance.PaintElement();
            Infragistics.UltraChart.Resources.Appearance.ColumnChartAppearance columnChartAppearance3 = new Infragistics.UltraChart.Resources.Appearance.ColumnChartAppearance();
            Infragistics.UltraChart.Resources.Appearance.ThreeDEffect threeDEffect6 = new Infragistics.UltraChart.Resources.Appearance.ThreeDEffect();
            Infragistics.UltraChart.Resources.Appearance.GradientEffect gradientEffect7 = new Infragistics.UltraChart.Resources.Appearance.GradientEffect();
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
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab1 = new Infragistics.Win.UltraWinTabControl.UltraTab();
            Infragistics.Win.Appearance appearance41 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab2 = new Infragistics.Win.UltraWinTabControl.UltraTab();
            Infragistics.Win.UltraWinDataSource.UltraDataColumn ultraDataColumn1 = new Infragistics.Win.UltraWinDataSource.UltraDataColumn("Transdate");
            Infragistics.Win.UltraWinDataSource.UltraDataColumn ultraDataColumn2 = new Infragistics.Win.UltraWinDataSource.UltraDataColumn("totalpaid");
            Infragistics.Win.Appearance appearance42 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance43 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance44 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab11 = new Infragistics.Win.UltraWinTabControl.UltraTab();
            Infragistics.Win.Appearance appearance45 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance46 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance47 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance48 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance49 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance50 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab12 = new Infragistics.Win.UltraWinTabControl.UltraTab();
            Infragistics.Win.Appearance appearance51 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance52 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance53 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab3 = new Infragistics.Win.UltraWinTabControl.UltraTab();
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab4 = new Infragistics.Win.UltraWinTabControl.UltraTab();
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab5 = new Infragistics.Win.UltraWinTabControl.UltraTab();
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab6 = new Infragistics.Win.UltraWinTabControl.UltraTab();
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab7 = new Infragistics.Win.UltraWinTabControl.UltraTab();
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab8 = new Infragistics.Win.UltraWinTabControl.UltraTab();
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab9 = new Infragistics.Win.UltraWinTabControl.UltraTab();
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab10 = new Infragistics.Win.UltraWinTabControl.UltraTab();
            this.ultraTabPageControl13 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.gdSalesHistory = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.ultraTabPageControl14 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.btnByTransCount = new Infragistics.Win.Misc.UltraButton();
            this.btnByAmount = new Infragistics.Win.Misc.UltraButton();
            this.chartSaleHistory = new Infragistics.Win.UltraWinChart.UltraChart();
            this.ultraTabPageControl1 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.picScroll = new Infragistics.Win.UltraWinEditors.UltraPictureBox();
            this.clTo = new Infragistics.Win.UltraWinSchedule.UltraCalendarCombo();
            this.clFrom = new Infragistics.Win.UltraWinSchedule.UltraCalendarCombo();
            this.lbl2 = new Infragistics.Win.Misc.UltraLabel();
            this.lbl1 = new Infragistics.Win.Misc.UltraLabel();
            this.btnSearch = new Infragistics.Win.Misc.UltraButton();
            this.btnClose = new Infragistics.Win.Misc.UltraButton();
            this.ultraTabPageControl2 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.chtSalesTrend = new Infragistics.Win.UltraWinChart.UltraChart();
            this.ultraTabPageControl3 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.chtSaleByDepartment = new Infragistics.Win.UltraWinChart.UltraChart();
            this.ultraTabPageControl4 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.chtSaleByEmp = new Infragistics.Win.UltraWinChart.UltraChart();
            this.ultraTabPageControl5 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.chtSaleByRegister = new Infragistics.Win.UltraWinChart.UltraChart();
            this.ultraTabPageControl6 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.ultraLabel2 = new Infragistics.Win.Misc.UltraLabel();
            this.numTopDept = new Infragistics.Win.UltraWinEditors.UltraNumericEditor();
            this.chtTopDept = new Infragistics.Win.UltraWinChart.UltraChart();
            this.ultraTabPageControl7 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.chtTopItems = new Infragistics.Win.UltraWinChart.UltraChart();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.numTopItem = new Infragistics.Win.UltraWinEditors.UltraNumericEditor();
            this.ultraLabel1 = new Infragistics.Win.Misc.UltraLabel();
            this.ultraTabPageControl8 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.numTRTransNo = new Infragistics.Win.UltraWinEditors.UltraNumericEditor();
            this.ultraLabel7 = new Infragistics.Win.Misc.UltraLabel();
            this.numTRTransAmount = new Infragistics.Win.UltraWinEditors.UltraNumericEditor();
            this.ultraLabel8 = new Infragistics.Win.Misc.UltraLabel();
            this.numTSTransNo = new Infragistics.Win.UltraWinEditors.UltraNumericEditor();
            this.ultraLabel9 = new Infragistics.Win.Misc.UltraLabel();
            this.numTSTransAmount = new Infragistics.Win.UltraWinEditors.UltraNumericEditor();
            this.ultraLabel10 = new Infragistics.Win.Misc.UltraLabel();
            this.grpTransTypeAll = new System.Windows.Forms.GroupBox();
            this.numAllRTransNo = new Infragistics.Win.UltraWinEditors.UltraNumericEditor();
            this.ultraLabel5 = new Infragistics.Win.Misc.UltraLabel();
            this.numAllRTransAmount = new Infragistics.Win.UltraWinEditors.UltraNumericEditor();
            this.ultraLabel6 = new Infragistics.Win.Misc.UltraLabel();
            this.numAllSTransNo = new Infragistics.Win.UltraWinEditors.UltraNumericEditor();
            this.ultraLabel4 = new Infragistics.Win.Misc.UltraLabel();
            this.numAllSTransAmount = new Infragistics.Win.UltraWinEditors.UltraNumericEditor();
            this.ultraLabel3 = new Infragistics.Win.Misc.UltraLabel();
            this.ultraTabPageControl9 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.grdPaymentToday = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.grpPayTypeAll = new System.Windows.Forms.GroupBox();
            this.grdPaymentAll = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.ultraTabPageControl10 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.SaleHistoryTab = new Infragistics.Win.UltraWinTabControl.UltraTabControl();
            this.ultraTabSharedControlsPage3 = new Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage();
            this.clTo1 = new Infragistics.Win.UltraWinSchedule.UltraCalendarCombo();
            this.clFrom1 = new Infragistics.Win.UltraWinSchedule.UltraCalendarCombo();
            this.ultraDataSource1 = new Infragistics.Win.UltraWinDataSource.UltraDataSource(this.components);
            this.tabMain = new Infragistics.Win.UltraWinTabControl.UltraTabControl();
            this.ultraTabSharedControlsPage1 = new Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage();
            this.tbData = new Infragistics.Win.UltraWinTabControl.UltraTabControl();
            this.ultraTabSharedControlsPage2 = new Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage();
            this.popControl = new Infragistics.Win.Misc.UltraPopupControlContainer(this.components);
            this.ultraDataSource2 = new Infragistics.Win.UltraWinDataSource.UltraDataSource(this.components);
            this.ultraTabPageControl13.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gdSalesHistory)).BeginInit();
            this.ultraTabPageControl14.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chartSaleHistory)).BeginInit();
            this.ultraTabPageControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.clTo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.clFrom)).BeginInit();
            this.ultraTabPageControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chtSalesTrend)).BeginInit();
            this.ultraTabPageControl3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chtSaleByDepartment)).BeginInit();
            this.ultraTabPageControl4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chtSaleByEmp)).BeginInit();
            this.ultraTabPageControl5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chtSaleByRegister)).BeginInit();
            this.ultraTabPageControl6.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numTopDept)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chtTopDept)).BeginInit();
            this.ultraTabPageControl7.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chtTopItems)).BeginInit();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numTopItem)).BeginInit();
            this.ultraTabPageControl8.SuspendLayout();
            this.groupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numTRTransNo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numTRTransAmount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numTSTransNo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numTSTransAmount)).BeginInit();
            this.grpTransTypeAll.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numAllRTransNo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numAllRTransAmount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numAllSTransNo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numAllSTransAmount)).BeginInit();
            this.ultraTabPageControl9.SuspendLayout();
            this.groupBox6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdPaymentToday)).BeginInit();
            this.grpPayTypeAll.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdPaymentAll)).BeginInit();
            this.ultraTabPageControl10.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SaleHistoryTab)).BeginInit();
            this.SaleHistoryTab.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.clTo1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.clFrom1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraDataSource1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tabMain)).BeginInit();
            this.tabMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tbData)).BeginInit();
            this.tbData.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraDataSource2)).BeginInit();
            this.SuspendLayout();
            // 
            // ultraTabPageControl13
            // 
            this.ultraTabPageControl13.Controls.Add(this.gdSalesHistory);
            this.ultraTabPageControl13.Location = new System.Drawing.Point(-10000, -10000);
            this.ultraTabPageControl13.Name = "ultraTabPageControl13";
            this.ultraTabPageControl13.Size = new System.Drawing.Size(1030, 454);
            // 
            // gdSalesHistory
            // 
            appearance1.BackColor = System.Drawing.SystemColors.Window;
            appearance1.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.gdSalesHistory.DisplayLayout.Appearance = appearance1;
            this.gdSalesHistory.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ExtendLastColumn;
            this.gdSalesHistory.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.gdSalesHistory.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance2.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance2.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance2.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance2.BorderColor = System.Drawing.SystemColors.Window;
            this.gdSalesHistory.DisplayLayout.GroupByBox.Appearance = appearance2;
            appearance3.ForeColor = System.Drawing.SystemColors.GrayText;
            this.gdSalesHistory.DisplayLayout.GroupByBox.BandLabelAppearance = appearance3;
            this.gdSalesHistory.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance4.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance4.BackColor2 = System.Drawing.SystemColors.Control;
            appearance4.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance4.ForeColor = System.Drawing.SystemColors.GrayText;
            this.gdSalesHistory.DisplayLayout.GroupByBox.PromptAppearance = appearance4;
            this.gdSalesHistory.DisplayLayout.MaxColScrollRegions = 1;
            this.gdSalesHistory.DisplayLayout.MaxRowScrollRegions = 1;
            appearance5.BackColor = System.Drawing.SystemColors.Window;
            appearance5.ForeColor = System.Drawing.SystemColors.ControlText;
            this.gdSalesHistory.DisplayLayout.Override.ActiveCellAppearance = appearance5;
            appearance6.BackColor = System.Drawing.SystemColors.Highlight;
            appearance6.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.gdSalesHistory.DisplayLayout.Override.ActiveRowAppearance = appearance6;
            this.gdSalesHistory.DisplayLayout.Override.AllowRowSummaries = Infragistics.Win.UltraWinGrid.AllowRowSummaries.True;
            this.gdSalesHistory.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.gdSalesHistory.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance7.BackColor = System.Drawing.SystemColors.Window;
            this.gdSalesHistory.DisplayLayout.Override.CardAreaAppearance = appearance7;
            appearance8.BorderColor = System.Drawing.Color.Silver;
            appearance8.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.gdSalesHistory.DisplayLayout.Override.CellAppearance = appearance8;
            this.gdSalesHistory.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;
            this.gdSalesHistory.DisplayLayout.Override.CellPadding = 0;
            appearance9.BackColor = System.Drawing.SystemColors.Control;
            appearance9.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance9.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance9.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance9.BorderColor = System.Drawing.SystemColors.Window;
            this.gdSalesHistory.DisplayLayout.Override.GroupByRowAppearance = appearance9;
            this.gdSalesHistory.DisplayLayout.Override.GroupBySummaryDisplayStyle = Infragistics.Win.UltraWinGrid.GroupBySummaryDisplayStyle.SummaryCellsAlwaysBelowDescription;
            appearance10.TextHAlignAsString = "Left";
            this.gdSalesHistory.DisplayLayout.Override.HeaderAppearance = appearance10;
            this.gdSalesHistory.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.gdSalesHistory.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance11.BackColor = System.Drawing.SystemColors.Window;
            appearance11.BorderColor = System.Drawing.Color.Silver;
            this.gdSalesHistory.DisplayLayout.Override.RowAppearance = appearance11;
            this.gdSalesHistory.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            this.gdSalesHistory.DisplayLayout.Override.SummaryDisplayArea = Infragistics.Win.UltraWinGrid.SummaryDisplayAreas.Bottom;
            appearance12.BackColor = System.Drawing.SystemColors.ControlLight;
            this.gdSalesHistory.DisplayLayout.Override.TemplateAddRowAppearance = appearance12;
            this.gdSalesHistory.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.gdSalesHistory.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.gdSalesHistory.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gdSalesHistory.Location = new System.Drawing.Point(0, 0);
            this.gdSalesHistory.Name = "gdSalesHistory";
            this.gdSalesHistory.Size = new System.Drawing.Size(1030, 454);
            this.gdSalesHistory.TabIndex = 3;
            this.gdSalesHistory.Text = "Sales History For Last Five Years";
            this.gdSalesHistory.UpdateMode = Infragistics.Win.UltraWinGrid.UpdateMode.OnUpdate;
            this.gdSalesHistory.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.gdSalesHistory.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.gdSalesHistory_InitializeLayout);
            // 
            // ultraTabPageControl14
            // 
            this.ultraTabPageControl14.Controls.Add(this.btnByTransCount);
            this.ultraTabPageControl14.Controls.Add(this.btnByAmount);
            this.ultraTabPageControl14.Controls.Add(this.chartSaleHistory);
            this.ultraTabPageControl14.Location = new System.Drawing.Point(2, 23);
            this.ultraTabPageControl14.Name = "ultraTabPageControl14";
            this.ultraTabPageControl14.Size = new System.Drawing.Size(1030, 454);
            // 
            // btnByTransCount
            // 
            this.btnByTransCount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            appearance13.BackColor = System.Drawing.Color.White;
            appearance13.BackColor2 = System.Drawing.SystemColors.Control;
            appearance13.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance13.FontData.BoldAsString = "True";
            appearance13.ForeColor = System.Drawing.Color.Black;
            this.btnByTransCount.Appearance = appearance13;
            this.btnByTransCount.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Flat;
            appearance14.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(188)))), ((int)(((byte)(193)))), ((int)(((byte)(207)))));
            appearance14.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(188)))), ((int)(((byte)(193)))), ((int)(((byte)(207)))));
            this.btnByTransCount.HotTrackAppearance = appearance14;
            this.btnByTransCount.Location = new System.Drawing.Point(692, 6);
            this.btnByTransCount.Name = "btnByTransCount";
            this.btnByTransCount.Size = new System.Drawing.Size(157, 26);
            this.btnByTransCount.TabIndex = 16;
            this.btnByTransCount.Text = "By Trans &Count";
            this.btnByTransCount.UseHotTracking = Infragistics.Win.DefaultableBoolean.True;
            this.btnByTransCount.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnByTransCount.Click += new System.EventHandler(this.btnByTransCount_Click);
            // 
            // btnByAmount
            // 
            this.btnByAmount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            appearance15.BackColor = System.Drawing.Color.White;
            appearance15.BackColor2 = System.Drawing.SystemColors.Control;
            appearance15.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance15.FontData.BoldAsString = "True";
            appearance15.ForeColor = System.Drawing.Color.Black;
            this.btnByAmount.Appearance = appearance15;
            this.btnByAmount.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Flat;
            appearance16.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(188)))), ((int)(((byte)(193)))), ((int)(((byte)(207)))));
            appearance16.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(188)))), ((int)(((byte)(193)))), ((int)(((byte)(207)))));
            this.btnByAmount.HotTrackAppearance = appearance16;
            this.btnByAmount.Location = new System.Drawing.Point(860, 6);
            this.btnByAmount.Name = "btnByAmount";
            this.btnByAmount.Size = new System.Drawing.Size(157, 26);
            this.btnByAmount.TabIndex = 15;
            this.btnByAmount.Text = "By Trans &Amount";
            this.btnByAmount.UseHotTracking = Infragistics.Win.DefaultableBoolean.True;
            this.btnByAmount.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnByAmount.Click += new System.EventHandler(this.btnByAmount_Click);
            // 
//			'UltraChart' properties's serialization: Since 'ChartType' changes the way axes look,
//			'ChartType' must be persisted ahead of any Axes change made in design time.
//		
            this.chartSaleHistory.ChartType = Infragistics.UltraChart.Shared.Styles.ChartType.LineChart;
            // 
            // chartSaleHistory
            // 
            this.chartSaleHistory.Axis.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(248)))), ((int)(((byte)(220)))));
            paintElement1.ElementType = Infragistics.UltraChart.Shared.Styles.PaintElementType.None;
            paintElement1.Fill = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(248)))), ((int)(((byte)(220)))));
            this.chartSaleHistory.Axis.PE = paintElement1;
            this.chartSaleHistory.Axis.X.Extent = 50;
            this.chartSaleHistory.Axis.X.Labels.Font = new System.Drawing.Font("Verdana", 7F);
            this.chartSaleHistory.Axis.X.Labels.FontColor = System.Drawing.Color.DimGray;
            this.chartSaleHistory.Axis.X.Labels.HorizontalAlign = System.Drawing.StringAlignment.Near;
            this.chartSaleHistory.Axis.X.Labels.ItemFormatString = "<ITEM_LABEL>";
            this.chartSaleHistory.Axis.X.Labels.Layout.Behavior = Infragistics.UltraChart.Shared.Styles.AxisLabelLayoutBehaviors.Auto;
            this.chartSaleHistory.Axis.X.Labels.Orientation = Infragistics.UltraChart.Shared.Styles.TextOrientation.VerticalLeftFacing;
            this.chartSaleHistory.Axis.X.Labels.SeriesLabels.Font = new System.Drawing.Font("Verdana", 7F);
            this.chartSaleHistory.Axis.X.Labels.SeriesLabels.FontColor = System.Drawing.Color.DimGray;
            this.chartSaleHistory.Axis.X.Labels.SeriesLabels.FormatString = "";
            this.chartSaleHistory.Axis.X.Labels.SeriesLabels.HorizontalAlign = System.Drawing.StringAlignment.Near;
            this.chartSaleHistory.Axis.X.Labels.SeriesLabels.Layout.Behavior = Infragistics.UltraChart.Shared.Styles.AxisLabelLayoutBehaviors.Auto;
            this.chartSaleHistory.Axis.X.Labels.SeriesLabels.Orientation = Infragistics.UltraChart.Shared.Styles.TextOrientation.VerticalLeftFacing;
            this.chartSaleHistory.Axis.X.Labels.SeriesLabels.VerticalAlign = System.Drawing.StringAlignment.Center;
            this.chartSaleHistory.Axis.X.Labels.VerticalAlign = System.Drawing.StringAlignment.Center;
            this.chartSaleHistory.Axis.X.MajorGridLines.AlphaLevel = ((byte)(255));
            this.chartSaleHistory.Axis.X.MajorGridLines.Color = System.Drawing.Color.Gainsboro;
            this.chartSaleHistory.Axis.X.MajorGridLines.DrawStyle = Infragistics.UltraChart.Shared.Styles.LineDrawStyle.Dot;
            this.chartSaleHistory.Axis.X.MajorGridLines.Visible = true;
            this.chartSaleHistory.Axis.X.MinorGridLines.AlphaLevel = ((byte)(255));
            this.chartSaleHistory.Axis.X.MinorGridLines.Color = System.Drawing.Color.LightGray;
            this.chartSaleHistory.Axis.X.MinorGridLines.DrawStyle = Infragistics.UltraChart.Shared.Styles.LineDrawStyle.Dot;
            this.chartSaleHistory.Axis.X.MinorGridLines.Visible = false;
            this.chartSaleHistory.Axis.X.TickmarkStyle = Infragistics.UltraChart.Shared.Styles.AxisTickStyle.Smart;
            this.chartSaleHistory.Axis.X.Visible = true;
            this.chartSaleHistory.Axis.X2.Labels.Font = new System.Drawing.Font("Verdana", 7F);
            this.chartSaleHistory.Axis.X2.Labels.FontColor = System.Drawing.Color.Gray;
            this.chartSaleHistory.Axis.X2.Labels.HorizontalAlign = System.Drawing.StringAlignment.Far;
            this.chartSaleHistory.Axis.X2.Labels.ItemFormatString = "<ITEM_LABEL>";
            this.chartSaleHistory.Axis.X2.Labels.Layout.Behavior = Infragistics.UltraChart.Shared.Styles.AxisLabelLayoutBehaviors.Auto;
            this.chartSaleHistory.Axis.X2.Labels.Orientation = Infragistics.UltraChart.Shared.Styles.TextOrientation.VerticalLeftFacing;
            this.chartSaleHistory.Axis.X2.Labels.SeriesLabels.Font = new System.Drawing.Font("Verdana", 7F);
            this.chartSaleHistory.Axis.X2.Labels.SeriesLabels.FontColor = System.Drawing.Color.Gray;
            this.chartSaleHistory.Axis.X2.Labels.SeriesLabels.FormatString = "";
            this.chartSaleHistory.Axis.X2.Labels.SeriesLabels.HorizontalAlign = System.Drawing.StringAlignment.Far;
            this.chartSaleHistory.Axis.X2.Labels.SeriesLabels.Layout.Behavior = Infragistics.UltraChart.Shared.Styles.AxisLabelLayoutBehaviors.Auto;
            this.chartSaleHistory.Axis.X2.Labels.SeriesLabels.Orientation = Infragistics.UltraChart.Shared.Styles.TextOrientation.VerticalLeftFacing;
            this.chartSaleHistory.Axis.X2.Labels.SeriesLabels.VerticalAlign = System.Drawing.StringAlignment.Center;
            this.chartSaleHistory.Axis.X2.Labels.VerticalAlign = System.Drawing.StringAlignment.Center;
            this.chartSaleHistory.Axis.X2.Labels.Visible = false;
            this.chartSaleHistory.Axis.X2.LineThickness = 1;
            this.chartSaleHistory.Axis.X2.MajorGridLines.AlphaLevel = ((byte)(255));
            this.chartSaleHistory.Axis.X2.MajorGridLines.Color = System.Drawing.Color.Gainsboro;
            this.chartSaleHistory.Axis.X2.MajorGridLines.DrawStyle = Infragistics.UltraChart.Shared.Styles.LineDrawStyle.Dot;
            this.chartSaleHistory.Axis.X2.MajorGridLines.Visible = true;
            this.chartSaleHistory.Axis.X2.MinorGridLines.AlphaLevel = ((byte)(255));
            this.chartSaleHistory.Axis.X2.MinorGridLines.Color = System.Drawing.Color.LightGray;
            this.chartSaleHistory.Axis.X2.MinorGridLines.DrawStyle = Infragistics.UltraChart.Shared.Styles.LineDrawStyle.Dot;
            this.chartSaleHistory.Axis.X2.MinorGridLines.Visible = false;
            this.chartSaleHistory.Axis.X2.TickmarkStyle = Infragistics.UltraChart.Shared.Styles.AxisTickStyle.Smart;
            this.chartSaleHistory.Axis.X2.Visible = false;
            this.chartSaleHistory.Axis.Y.Extent = 50;
            this.chartSaleHistory.Axis.Y.Labels.Font = new System.Drawing.Font("Verdana", 7F);
            this.chartSaleHistory.Axis.Y.Labels.FontColor = System.Drawing.Color.DimGray;
            this.chartSaleHistory.Axis.Y.Labels.HorizontalAlign = System.Drawing.StringAlignment.Far;
            this.chartSaleHistory.Axis.Y.Labels.ItemFormatString = "<DATA_VALUE:00.##>";
            this.chartSaleHistory.Axis.Y.Labels.Layout.Behavior = Infragistics.UltraChart.Shared.Styles.AxisLabelLayoutBehaviors.Auto;
            this.chartSaleHistory.Axis.Y.Labels.Orientation = Infragistics.UltraChart.Shared.Styles.TextOrientation.Horizontal;
            this.chartSaleHistory.Axis.Y.Labels.SeriesLabels.Font = new System.Drawing.Font("Verdana", 7F);
            this.chartSaleHistory.Axis.Y.Labels.SeriesLabels.FontColor = System.Drawing.Color.DimGray;
            this.chartSaleHistory.Axis.Y.Labels.SeriesLabels.FormatString = "";
            this.chartSaleHistory.Axis.Y.Labels.SeriesLabels.HorizontalAlign = System.Drawing.StringAlignment.Far;
            this.chartSaleHistory.Axis.Y.Labels.SeriesLabels.Layout.Behavior = Infragistics.UltraChart.Shared.Styles.AxisLabelLayoutBehaviors.Auto;
            this.chartSaleHistory.Axis.Y.Labels.SeriesLabels.Orientation = Infragistics.UltraChart.Shared.Styles.TextOrientation.Horizontal;
            this.chartSaleHistory.Axis.Y.Labels.SeriesLabels.VerticalAlign = System.Drawing.StringAlignment.Center;
            this.chartSaleHistory.Axis.Y.Labels.VerticalAlign = System.Drawing.StringAlignment.Center;
            this.chartSaleHistory.Axis.Y.MajorGridLines.AlphaLevel = ((byte)(255));
            this.chartSaleHistory.Axis.Y.MajorGridLines.Color = System.Drawing.Color.Gainsboro;
            this.chartSaleHistory.Axis.Y.MajorGridLines.DrawStyle = Infragistics.UltraChart.Shared.Styles.LineDrawStyle.Dot;
            this.chartSaleHistory.Axis.Y.MajorGridLines.Visible = true;
            this.chartSaleHistory.Axis.Y.MinorGridLines.AlphaLevel = ((byte)(255));
            this.chartSaleHistory.Axis.Y.MinorGridLines.Color = System.Drawing.Color.LightGray;
            this.chartSaleHistory.Axis.Y.MinorGridLines.DrawStyle = Infragistics.UltraChart.Shared.Styles.LineDrawStyle.Dot;
            this.chartSaleHistory.Axis.Y.MinorGridLines.Visible = false;
            this.chartSaleHistory.Axis.Y.TickmarkInterval = 40D;
            this.chartSaleHistory.Axis.Y.TickmarkStyle = Infragistics.UltraChart.Shared.Styles.AxisTickStyle.Smart;
            this.chartSaleHistory.Axis.Y.Visible = true;
            this.chartSaleHistory.Axis.Y2.Labels.Font = new System.Drawing.Font("Verdana", 7F);
            this.chartSaleHistory.Axis.Y2.Labels.FontColor = System.Drawing.Color.Gray;
            this.chartSaleHistory.Axis.Y2.Labels.HorizontalAlign = System.Drawing.StringAlignment.Near;
            this.chartSaleHistory.Axis.Y2.Labels.ItemFormatString = "<DATA_VALUE:00.##>";
            this.chartSaleHistory.Axis.Y2.Labels.Layout.Behavior = Infragistics.UltraChart.Shared.Styles.AxisLabelLayoutBehaviors.Auto;
            this.chartSaleHistory.Axis.Y2.Labels.Orientation = Infragistics.UltraChart.Shared.Styles.TextOrientation.Horizontal;
            this.chartSaleHistory.Axis.Y2.Labels.SeriesLabels.Font = new System.Drawing.Font("Verdana", 7F);
            this.chartSaleHistory.Axis.Y2.Labels.SeriesLabels.FontColor = System.Drawing.Color.Gray;
            this.chartSaleHistory.Axis.Y2.Labels.SeriesLabels.FormatString = "";
            this.chartSaleHistory.Axis.Y2.Labels.SeriesLabels.HorizontalAlign = System.Drawing.StringAlignment.Near;
            this.chartSaleHistory.Axis.Y2.Labels.SeriesLabels.Layout.Behavior = Infragistics.UltraChart.Shared.Styles.AxisLabelLayoutBehaviors.Auto;
            this.chartSaleHistory.Axis.Y2.Labels.SeriesLabels.Orientation = Infragistics.UltraChart.Shared.Styles.TextOrientation.Horizontal;
            this.chartSaleHistory.Axis.Y2.Labels.SeriesLabels.VerticalAlign = System.Drawing.StringAlignment.Center;
            this.chartSaleHistory.Axis.Y2.Labels.VerticalAlign = System.Drawing.StringAlignment.Center;
            this.chartSaleHistory.Axis.Y2.Labels.Visible = false;
            this.chartSaleHistory.Axis.Y2.LineThickness = 1;
            this.chartSaleHistory.Axis.Y2.MajorGridLines.AlphaLevel = ((byte)(255));
            this.chartSaleHistory.Axis.Y2.MajorGridLines.Color = System.Drawing.Color.Gainsboro;
            this.chartSaleHistory.Axis.Y2.MajorGridLines.DrawStyle = Infragistics.UltraChart.Shared.Styles.LineDrawStyle.Dot;
            this.chartSaleHistory.Axis.Y2.MajorGridLines.Visible = true;
            this.chartSaleHistory.Axis.Y2.MinorGridLines.AlphaLevel = ((byte)(255));
            this.chartSaleHistory.Axis.Y2.MinorGridLines.Color = System.Drawing.Color.LightGray;
            this.chartSaleHistory.Axis.Y2.MinorGridLines.DrawStyle = Infragistics.UltraChart.Shared.Styles.LineDrawStyle.Dot;
            this.chartSaleHistory.Axis.Y2.MinorGridLines.Visible = false;
            this.chartSaleHistory.Axis.Y2.TickmarkInterval = 40D;
            this.chartSaleHistory.Axis.Y2.TickmarkStyle = Infragistics.UltraChart.Shared.Styles.AxisTickStyle.Smart;
            this.chartSaleHistory.Axis.Y2.Visible = false;
            this.chartSaleHistory.Axis.Z.Labels.Font = new System.Drawing.Font("Verdana", 7F);
            this.chartSaleHistory.Axis.Z.Labels.FontColor = System.Drawing.Color.DimGray;
            this.chartSaleHistory.Axis.Z.Labels.HorizontalAlign = System.Drawing.StringAlignment.Near;
            this.chartSaleHistory.Axis.Z.Labels.ItemFormatString = "";
            this.chartSaleHistory.Axis.Z.Labels.Layout.Behavior = Infragistics.UltraChart.Shared.Styles.AxisLabelLayoutBehaviors.Auto;
            this.chartSaleHistory.Axis.Z.Labels.Orientation = Infragistics.UltraChart.Shared.Styles.TextOrientation.Horizontal;
            this.chartSaleHistory.Axis.Z.Labels.SeriesLabels.Font = new System.Drawing.Font("Verdana", 7F);
            this.chartSaleHistory.Axis.Z.Labels.SeriesLabels.FontColor = System.Drawing.Color.DimGray;
            this.chartSaleHistory.Axis.Z.Labels.SeriesLabels.HorizontalAlign = System.Drawing.StringAlignment.Near;
            this.chartSaleHistory.Axis.Z.Labels.SeriesLabels.Layout.Behavior = Infragistics.UltraChart.Shared.Styles.AxisLabelLayoutBehaviors.Auto;
            this.chartSaleHistory.Axis.Z.Labels.SeriesLabels.Orientation = Infragistics.UltraChart.Shared.Styles.TextOrientation.Horizontal;
            this.chartSaleHistory.Axis.Z.Labels.SeriesLabels.VerticalAlign = System.Drawing.StringAlignment.Center;
            this.chartSaleHistory.Axis.Z.Labels.VerticalAlign = System.Drawing.StringAlignment.Center;
            this.chartSaleHistory.Axis.Z.Labels.Visible = false;
            this.chartSaleHistory.Axis.Z.LineThickness = 1;
            this.chartSaleHistory.Axis.Z.MajorGridLines.AlphaLevel = ((byte)(255));
            this.chartSaleHistory.Axis.Z.MajorGridLines.Color = System.Drawing.Color.Gainsboro;
            this.chartSaleHistory.Axis.Z.MajorGridLines.DrawStyle = Infragistics.UltraChart.Shared.Styles.LineDrawStyle.Dot;
            this.chartSaleHistory.Axis.Z.MajorGridLines.Visible = true;
            this.chartSaleHistory.Axis.Z.MinorGridLines.AlphaLevel = ((byte)(255));
            this.chartSaleHistory.Axis.Z.MinorGridLines.Color = System.Drawing.Color.LightGray;
            this.chartSaleHistory.Axis.Z.MinorGridLines.DrawStyle = Infragistics.UltraChart.Shared.Styles.LineDrawStyle.Dot;
            this.chartSaleHistory.Axis.Z.MinorGridLines.Visible = false;
            this.chartSaleHistory.Axis.Z.TickmarkStyle = Infragistics.UltraChart.Shared.Styles.AxisTickStyle.Smart;
            this.chartSaleHistory.Axis.Z.Visible = false;
            this.chartSaleHistory.Axis.Z2.Labels.Font = new System.Drawing.Font("Verdana", 7F);
            this.chartSaleHistory.Axis.Z2.Labels.FontColor = System.Drawing.Color.Gray;
            this.chartSaleHistory.Axis.Z2.Labels.HorizontalAlign = System.Drawing.StringAlignment.Near;
            this.chartSaleHistory.Axis.Z2.Labels.ItemFormatString = "";
            this.chartSaleHistory.Axis.Z2.Labels.Layout.Behavior = Infragistics.UltraChart.Shared.Styles.AxisLabelLayoutBehaviors.Auto;
            this.chartSaleHistory.Axis.Z2.Labels.Orientation = Infragistics.UltraChart.Shared.Styles.TextOrientation.Horizontal;
            this.chartSaleHistory.Axis.Z2.Labels.SeriesLabels.Font = new System.Drawing.Font("Verdana", 7F);
            this.chartSaleHistory.Axis.Z2.Labels.SeriesLabels.FontColor = System.Drawing.Color.Gray;
            this.chartSaleHistory.Axis.Z2.Labels.SeriesLabels.HorizontalAlign = System.Drawing.StringAlignment.Near;
            this.chartSaleHistory.Axis.Z2.Labels.SeriesLabels.Layout.Behavior = Infragistics.UltraChart.Shared.Styles.AxisLabelLayoutBehaviors.Auto;
            this.chartSaleHistory.Axis.Z2.Labels.SeriesLabels.Orientation = Infragistics.UltraChart.Shared.Styles.TextOrientation.Horizontal;
            this.chartSaleHistory.Axis.Z2.Labels.SeriesLabels.VerticalAlign = System.Drawing.StringAlignment.Center;
            this.chartSaleHistory.Axis.Z2.Labels.VerticalAlign = System.Drawing.StringAlignment.Center;
            this.chartSaleHistory.Axis.Z2.Labels.Visible = false;
            this.chartSaleHistory.Axis.Z2.LineThickness = 1;
            this.chartSaleHistory.Axis.Z2.MajorGridLines.AlphaLevel = ((byte)(255));
            this.chartSaleHistory.Axis.Z2.MajorGridLines.Color = System.Drawing.Color.Gainsboro;
            this.chartSaleHistory.Axis.Z2.MajorGridLines.DrawStyle = Infragistics.UltraChart.Shared.Styles.LineDrawStyle.Dot;
            this.chartSaleHistory.Axis.Z2.MajorGridLines.Visible = true;
            this.chartSaleHistory.Axis.Z2.MinorGridLines.AlphaLevel = ((byte)(255));
            this.chartSaleHistory.Axis.Z2.MinorGridLines.Color = System.Drawing.Color.LightGray;
            this.chartSaleHistory.Axis.Z2.MinorGridLines.DrawStyle = Infragistics.UltraChart.Shared.Styles.LineDrawStyle.Dot;
            this.chartSaleHistory.Axis.Z2.MinorGridLines.Visible = false;
            this.chartSaleHistory.Axis.Z2.TickmarkStyle = Infragistics.UltraChart.Shared.Styles.AxisTickStyle.Smart;
            this.chartSaleHistory.Axis.Z2.Visible = false;
            this.chartSaleHistory.BackColor = System.Drawing.Color.Transparent;
            this.chartSaleHistory.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("chartSaleHistory.BackgroundImage")));
            this.chartSaleHistory.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.chartSaleHistory.ColorModel.AlphaLevel = ((byte)(150));
            this.chartSaleHistory.ColorModel.ColorBegin = System.Drawing.Color.Pink;
            this.chartSaleHistory.ColorModel.ColorEnd = System.Drawing.Color.DarkRed;
            this.chartSaleHistory.ColorModel.ModelStyle = Infragistics.UltraChart.Shared.Styles.ColorModels.CustomLinear;
            this.chartSaleHistory.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chartSaleHistory.Effects.Effects.Add(gradientEffect1);
            this.chartSaleHistory.Legend.Visible = true;
            this.chartSaleHistory.Location = new System.Drawing.Point(0, 0);
            this.chartSaleHistory.Name = "chartSaleHistory";
            this.chartSaleHistory.Size = new System.Drawing.Size(1030, 454);
            this.chartSaleHistory.TabIndex = 14;
            this.chartSaleHistory.TitleBottom.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chartSaleHistory.TitleBottom.HorizontalAlign = System.Drawing.StringAlignment.Center;
            this.chartSaleHistory.TitleBottom.Text = "Months";
            this.chartSaleHistory.TitleBottom.VerticalAlign = System.Drawing.StringAlignment.Near;
            this.chartSaleHistory.TitleTop.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chartSaleHistory.TitleTop.HorizontalAlign = System.Drawing.StringAlignment.Center;
            this.chartSaleHistory.TitleTop.Text = "Sale History";
            this.chartSaleHistory.Tooltips.HighlightFillColor = System.Drawing.Color.DimGray;
            this.chartSaleHistory.Tooltips.HighlightOutlineColor = System.Drawing.Color.DarkGray;
            // 
            // ultraTabPageControl1
            // 
            this.ultraTabPageControl1.Controls.Add(this.picScroll);
            this.ultraTabPageControl1.Controls.Add(this.clTo);
            this.ultraTabPageControl1.Controls.Add(this.clFrom);
            this.ultraTabPageControl1.Controls.Add(this.lbl2);
            this.ultraTabPageControl1.Controls.Add(this.lbl1);
            this.ultraTabPageControl1.Controls.Add(this.btnSearch);
            this.ultraTabPageControl1.Controls.Add(this.btnClose);
            this.ultraTabPageControl1.Location = new System.Drawing.Point(2, 23);
            this.ultraTabPageControl1.Name = "ultraTabPageControl1";
            this.ultraTabPageControl1.Size = new System.Drawing.Size(1038, 53);
            // 
            // picScroll
            // 
            this.picScroll.BorderShadowColor = System.Drawing.Color.Empty;
            this.picScroll.Image = ((object)(resources.GetObject("picScroll.Image")));
            this.picScroll.Location = new System.Drawing.Point(419, 8);
            this.picScroll.Name = "picScroll";
            this.picScroll.Size = new System.Drawing.Size(71, 44);
            this.picScroll.TabIndex = 8;
            this.picScroll.Visible = false;
            // 
            // clTo
            // 
            this.clTo.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.clTo.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Office2003ToolbarButton;
            this.clTo.DateButtons.Add(dateButton1);
            this.clTo.Location = new System.Drawing.Point(266, 17);
            this.clTo.Name = "clTo";
            this.clTo.NonAutoSizeHeight = 22;
            this.clTo.Size = new System.Drawing.Size(123, 21);
            this.clTo.TabIndex = 2;
            this.clTo.Value = new System.DateTime(2006, 1, 9, 0, 0, 0, 0);
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
            this.clFrom.Value = new System.DateTime(2006, 1, 9, 0, 0, 0, 0);
            // 
            // lbl2
            // 
            appearance17.ForeColor = System.Drawing.Color.Black;
            this.lbl2.Appearance = appearance17;
            this.lbl2.AutoSize = true;
            this.lbl2.BackColorInternal = System.Drawing.Color.Transparent;
            this.lbl2.Location = new System.Drawing.Point(214, 18);
            this.lbl2.Name = "lbl2";
            this.lbl2.Size = new System.Drawing.Size(22, 18);
            this.lbl2.TabIndex = 7;
            this.lbl2.Text = "To ";
            this.lbl2.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // lbl1
            // 
            appearance18.ForeColor = System.Drawing.Color.Black;
            this.lbl1.Appearance = appearance18;
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
            appearance19.BackColor = System.Drawing.Color.White;
            appearance19.BackColor2 = System.Drawing.SystemColors.Control;
            appearance19.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance19.FontData.BoldAsString = "True";
            appearance19.ForeColor = System.Drawing.Color.Black;
            appearance19.Image = ((object)(resources.GetObject("appearance19.Image")));
            this.btnSearch.Appearance = appearance19;
            this.btnSearch.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Flat;
            appearance20.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(188)))), ((int)(((byte)(193)))), ((int)(((byte)(207)))));
            appearance20.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(188)))), ((int)(((byte)(193)))), ((int)(((byte)(207)))));
            this.btnSearch.HotTrackAppearance = appearance20;
            this.btnSearch.Location = new System.Drawing.Point(763, 11);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(121, 30);
            this.btnSearch.TabIndex = 3;
            this.btnSearch.Text = "&View";
            this.btnSearch.UseHotTracking = Infragistics.Win.DefaultableBoolean.True;
            this.btnSearch.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            appearance21.BackColor = System.Drawing.Color.White;
            appearance21.BackColor2 = System.Drawing.SystemColors.Control;
            appearance21.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance21.FontData.BoldAsString = "True";
            appearance21.ForeColor = System.Drawing.Color.Black;
            appearance21.Image = ((object)(resources.GetObject("appearance21.Image")));
            this.btnClose.Appearance = appearance21;
            this.btnClose.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Flat;
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            appearance22.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(188)))), ((int)(((byte)(193)))), ((int)(((byte)(207)))));
            appearance22.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(188)))), ((int)(((byte)(193)))), ((int)(((byte)(207)))));
            this.btnClose.HotTrackAppearance = appearance22;
            this.btnClose.Location = new System.Drawing.Point(898, 11);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(121, 30);
            this.btnClose.TabIndex = 5;
            this.btnClose.Text = "&Close";
            this.btnClose.UseHotTracking = Infragistics.Win.DefaultableBoolean.True;
            this.btnClose.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // ultraTabPageControl2
            // 
            this.ultraTabPageControl2.Controls.Add(this.chtSalesTrend);
            this.ultraTabPageControl2.Location = new System.Drawing.Point(-10000, -10000);
            this.ultraTabPageControl2.Name = "ultraTabPageControl2";
            this.ultraTabPageControl2.Size = new System.Drawing.Size(1034, 479);
            // 
//			'UltraChart' properties's serialization: Since 'ChartType' changes the way axes look,
//			'ChartType' must be persisted ahead of any Axes change made in design time.
//		
            this.chtSalesTrend.ChartType = Infragistics.UltraChart.Shared.Styles.ChartType.SplineChart;
            // 
            // chtSalesTrend
            // 
            this.chtSalesTrend.Axis.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(248)))), ((int)(((byte)(220)))));
            paintElement2.ElementType = Infragistics.UltraChart.Shared.Styles.PaintElementType.None;
            paintElement2.Fill = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(248)))), ((int)(((byte)(220)))));
            this.chtSalesTrend.Axis.PE = paintElement2;
            this.chtSalesTrend.Axis.X.Labels.HorizontalAlign = System.Drawing.StringAlignment.Near;
            this.chtSalesTrend.Axis.X.Labels.ItemFormatString = "<ITEM_LABEL>";
            this.chtSalesTrend.Axis.X.Labels.Orientation = Infragistics.UltraChart.Shared.Styles.TextOrientation.VerticalLeftFacing;
            this.chtSalesTrend.Axis.X.Labels.SeriesLabels.FormatString = "";
            this.chtSalesTrend.Axis.X.Labels.SeriesLabels.HorizontalAlign = System.Drawing.StringAlignment.Near;
            this.chtSalesTrend.Axis.X.Labels.SeriesLabels.Orientation = Infragistics.UltraChart.Shared.Styles.TextOrientation.VerticalLeftFacing;
            this.chtSalesTrend.Axis.X.Labels.SeriesLabels.VerticalAlign = System.Drawing.StringAlignment.Center;
            this.chtSalesTrend.Axis.X.Labels.VerticalAlign = System.Drawing.StringAlignment.Center;
            this.chtSalesTrend.Axis.X.MajorGridLines.AlphaLevel = ((byte)(255));
            this.chtSalesTrend.Axis.X.MajorGridLines.Color = System.Drawing.Color.Gainsboro;
            this.chtSalesTrend.Axis.X.MajorGridLines.DrawStyle = Infragistics.UltraChart.Shared.Styles.LineDrawStyle.Dot;
            this.chtSalesTrend.Axis.X.MajorGridLines.Visible = true;
            this.chtSalesTrend.Axis.X.MinorGridLines.AlphaLevel = ((byte)(255));
            this.chtSalesTrend.Axis.X.MinorGridLines.Color = System.Drawing.Color.LightGray;
            this.chtSalesTrend.Axis.X.MinorGridLines.DrawStyle = Infragistics.UltraChart.Shared.Styles.LineDrawStyle.Dot;
            this.chtSalesTrend.Axis.X.MinorGridLines.Visible = false;
            this.chtSalesTrend.Axis.X.ScrollScale.Visible = true;
            this.chtSalesTrend.Axis.X.Visible = true;
            this.chtSalesTrend.Axis.X2.Labels.HorizontalAlign = System.Drawing.StringAlignment.Far;
            this.chtSalesTrend.Axis.X2.Labels.ItemFormatString = "<ITEM_LABEL>";
            this.chtSalesTrend.Axis.X2.Labels.Orientation = Infragistics.UltraChart.Shared.Styles.TextOrientation.VerticalLeftFacing;
            this.chtSalesTrend.Axis.X2.Labels.SeriesLabels.FormatString = "";
            this.chtSalesTrend.Axis.X2.Labels.SeriesLabels.HorizontalAlign = System.Drawing.StringAlignment.Far;
            this.chtSalesTrend.Axis.X2.Labels.SeriesLabels.Orientation = Infragistics.UltraChart.Shared.Styles.TextOrientation.VerticalLeftFacing;
            this.chtSalesTrend.Axis.X2.Labels.SeriesLabels.VerticalAlign = System.Drawing.StringAlignment.Center;
            this.chtSalesTrend.Axis.X2.Labels.VerticalAlign = System.Drawing.StringAlignment.Center;
            this.chtSalesTrend.Axis.X2.Labels.Visible = false;
            this.chtSalesTrend.Axis.X2.MajorGridLines.AlphaLevel = ((byte)(255));
            this.chtSalesTrend.Axis.X2.MajorGridLines.Color = System.Drawing.Color.Gainsboro;
            this.chtSalesTrend.Axis.X2.MajorGridLines.DrawStyle = Infragistics.UltraChart.Shared.Styles.LineDrawStyle.Dot;
            this.chtSalesTrend.Axis.X2.MajorGridLines.Visible = true;
            this.chtSalesTrend.Axis.X2.MinorGridLines.AlphaLevel = ((byte)(255));
            this.chtSalesTrend.Axis.X2.MinorGridLines.Color = System.Drawing.Color.LightGray;
            this.chtSalesTrend.Axis.X2.MinorGridLines.DrawStyle = Infragistics.UltraChart.Shared.Styles.LineDrawStyle.Dot;
            this.chtSalesTrend.Axis.X2.MinorGridLines.Visible = false;
            this.chtSalesTrend.Axis.X2.Visible = false;
            this.chtSalesTrend.Axis.Y.Labels.HorizontalAlign = System.Drawing.StringAlignment.Far;
            this.chtSalesTrend.Axis.Y.Labels.ItemFormatString = "<DATA_VALUE:"+Configuration.CInfo.CurrencySymbol.ToString()+" 00.00>";
            this.chtSalesTrend.Axis.Y.Labels.Orientation = Infragistics.UltraChart.Shared.Styles.TextOrientation.Horizontal;
            this.chtSalesTrend.Axis.Y.Labels.SeriesLabels.FormatString = "";
            this.chtSalesTrend.Axis.Y.Labels.SeriesLabels.HorizontalAlign = System.Drawing.StringAlignment.Far;
            this.chtSalesTrend.Axis.Y.Labels.SeriesLabels.Orientation = Infragistics.UltraChart.Shared.Styles.TextOrientation.Horizontal;
            this.chtSalesTrend.Axis.Y.Labels.SeriesLabels.VerticalAlign = System.Drawing.StringAlignment.Center;
            this.chtSalesTrend.Axis.Y.Labels.VerticalAlign = System.Drawing.StringAlignment.Center;
            this.chtSalesTrend.Axis.Y.MajorGridLines.AlphaLevel = ((byte)(255));
            this.chtSalesTrend.Axis.Y.MajorGridLines.Color = System.Drawing.Color.Gainsboro;
            this.chtSalesTrend.Axis.Y.MajorGridLines.DrawStyle = Infragistics.UltraChart.Shared.Styles.LineDrawStyle.Dot;
            this.chtSalesTrend.Axis.Y.MajorGridLines.Visible = true;
            this.chtSalesTrend.Axis.Y.MinorGridLines.AlphaLevel = ((byte)(255));
            this.chtSalesTrend.Axis.Y.MinorGridLines.Color = System.Drawing.Color.LightGray;
            this.chtSalesTrend.Axis.Y.MinorGridLines.DrawStyle = Infragistics.UltraChart.Shared.Styles.LineDrawStyle.Dot;
            this.chtSalesTrend.Axis.Y.MinorGridLines.Visible = false;
            this.chtSalesTrend.Axis.Y.Visible = true;
            this.chtSalesTrend.Axis.Y2.Labels.HorizontalAlign = System.Drawing.StringAlignment.Near;
            this.chtSalesTrend.Axis.Y2.Labels.ItemFormatString = "<DATA_VALUE:00.00>";
            this.chtSalesTrend.Axis.Y2.Labels.Orientation = Infragistics.UltraChart.Shared.Styles.TextOrientation.Horizontal;
            this.chtSalesTrend.Axis.Y2.Labels.SeriesLabels.FormatString = "";
            this.chtSalesTrend.Axis.Y2.Labels.SeriesLabels.HorizontalAlign = System.Drawing.StringAlignment.Near;
            this.chtSalesTrend.Axis.Y2.Labels.SeriesLabels.Orientation = Infragistics.UltraChart.Shared.Styles.TextOrientation.Horizontal;
            this.chtSalesTrend.Axis.Y2.Labels.SeriesLabels.VerticalAlign = System.Drawing.StringAlignment.Center;
            this.chtSalesTrend.Axis.Y2.Labels.VerticalAlign = System.Drawing.StringAlignment.Center;
            this.chtSalesTrend.Axis.Y2.Labels.Visible = false;
            this.chtSalesTrend.Axis.Y2.MajorGridLines.AlphaLevel = ((byte)(255));
            this.chtSalesTrend.Axis.Y2.MajorGridLines.Color = System.Drawing.Color.Gainsboro;
            this.chtSalesTrend.Axis.Y2.MajorGridLines.DrawStyle = Infragistics.UltraChart.Shared.Styles.LineDrawStyle.Dot;
            this.chtSalesTrend.Axis.Y2.MajorGridLines.Visible = true;
            this.chtSalesTrend.Axis.Y2.MinorGridLines.AlphaLevel = ((byte)(255));
            this.chtSalesTrend.Axis.Y2.MinorGridLines.Color = System.Drawing.Color.LightGray;
            this.chtSalesTrend.Axis.Y2.MinorGridLines.DrawStyle = Infragistics.UltraChart.Shared.Styles.LineDrawStyle.Dot;
            this.chtSalesTrend.Axis.Y2.MinorGridLines.Visible = false;
            this.chtSalesTrend.Axis.Y2.Visible = false;
            this.chtSalesTrend.Axis.Z.Labels.HorizontalAlign = System.Drawing.StringAlignment.Near;
            this.chtSalesTrend.Axis.Z.Labels.ItemFormatString = "";
            this.chtSalesTrend.Axis.Z.Labels.Orientation = Infragistics.UltraChart.Shared.Styles.TextOrientation.Horizontal;
            this.chtSalesTrend.Axis.Z.Labels.SeriesLabels.HorizontalAlign = System.Drawing.StringAlignment.Near;
            this.chtSalesTrend.Axis.Z.Labels.SeriesLabels.Orientation = Infragistics.UltraChart.Shared.Styles.TextOrientation.Horizontal;
            this.chtSalesTrend.Axis.Z.Labels.SeriesLabels.VerticalAlign = System.Drawing.StringAlignment.Center;
            this.chtSalesTrend.Axis.Z.Labels.VerticalAlign = System.Drawing.StringAlignment.Center;
            this.chtSalesTrend.Axis.Z.MajorGridLines.AlphaLevel = ((byte)(255));
            this.chtSalesTrend.Axis.Z.MajorGridLines.Color = System.Drawing.Color.Gainsboro;
            this.chtSalesTrend.Axis.Z.MajorGridLines.DrawStyle = Infragistics.UltraChart.Shared.Styles.LineDrawStyle.Dot;
            this.chtSalesTrend.Axis.Z.MajorGridLines.Visible = true;
            this.chtSalesTrend.Axis.Z.MinorGridLines.AlphaLevel = ((byte)(255));
            this.chtSalesTrend.Axis.Z.MinorGridLines.Color = System.Drawing.Color.LightGray;
            this.chtSalesTrend.Axis.Z.MinorGridLines.DrawStyle = Infragistics.UltraChart.Shared.Styles.LineDrawStyle.Dot;
            this.chtSalesTrend.Axis.Z.MinorGridLines.Visible = false;
            this.chtSalesTrend.Axis.Z.Visible = false;
            this.chtSalesTrend.Axis.Z2.Labels.HorizontalAlign = System.Drawing.StringAlignment.Near;
            this.chtSalesTrend.Axis.Z2.Labels.ItemFormatString = "";
            this.chtSalesTrend.Axis.Z2.Labels.Orientation = Infragistics.UltraChart.Shared.Styles.TextOrientation.Horizontal;
            this.chtSalesTrend.Axis.Z2.Labels.SeriesLabels.HorizontalAlign = System.Drawing.StringAlignment.Near;
            this.chtSalesTrend.Axis.Z2.Labels.SeriesLabels.Orientation = Infragistics.UltraChart.Shared.Styles.TextOrientation.Horizontal;
            this.chtSalesTrend.Axis.Z2.Labels.SeriesLabels.VerticalAlign = System.Drawing.StringAlignment.Center;
            this.chtSalesTrend.Axis.Z2.Labels.VerticalAlign = System.Drawing.StringAlignment.Center;
            this.chtSalesTrend.Axis.Z2.Labels.Visible = false;
            this.chtSalesTrend.Axis.Z2.MajorGridLines.AlphaLevel = ((byte)(255));
            this.chtSalesTrend.Axis.Z2.MajorGridLines.Color = System.Drawing.Color.Gainsboro;
            this.chtSalesTrend.Axis.Z2.MajorGridLines.DrawStyle = Infragistics.UltraChart.Shared.Styles.LineDrawStyle.Dot;
            this.chtSalesTrend.Axis.Z2.MajorGridLines.Visible = true;
            this.chtSalesTrend.Axis.Z2.MinorGridLines.AlphaLevel = ((byte)(255));
            this.chtSalesTrend.Axis.Z2.MinorGridLines.Color = System.Drawing.Color.LightGray;
            this.chtSalesTrend.Axis.Z2.MinorGridLines.DrawStyle = Infragistics.UltraChart.Shared.Styles.LineDrawStyle.Dot;
            this.chtSalesTrend.Axis.Z2.MinorGridLines.Visible = false;
            this.chtSalesTrend.Axis.Z2.Visible = false;
            this.chtSalesTrend.BackColor = System.Drawing.Color.Transparent;
            this.chtSalesTrend.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("chtSalesTrend.BackgroundImage")));
            this.chtSalesTrend.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.chtSalesTrend.ColorModel.AlphaLevel = ((byte)(150));
            this.chtSalesTrend.Data.SwapRowsAndColumns = true;
            this.chtSalesTrend.Data.ZeroAligned = true;
            this.chtSalesTrend.Dock = System.Windows.Forms.DockStyle.Fill;
            gradientEffect2.Style = Infragistics.UltraChart.Shared.Styles.GradientStyle.Elliptical;
            shadowEffect1.Angle = 45D;
            this.chtSalesTrend.Effects.Effects.Add(threeDEffect1);
            this.chtSalesTrend.Effects.Effects.Add(gradientEffect2);
            this.chtSalesTrend.Effects.Effects.Add(shadowEffect1);
            this.chtSalesTrend.ForeColor = System.Drawing.SystemColors.ControlText;
            this.chtSalesTrend.Location = new System.Drawing.Point(0, 0);
            this.chtSalesTrend.Name = "chtSalesTrend";
            this.chtSalesTrend.Size = new System.Drawing.Size(1034, 479);
            this.chtSalesTrend.TabIndex = 9;
            this.chtSalesTrend.TitleBottom.Visible = false;
            this.chtSalesTrend.TitleLeft.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold);
            this.chtSalesTrend.TitleLeft.HorizontalAlign = System.Drawing.StringAlignment.Center;
            this.chtSalesTrend.TitleLeft.Text = "AMOUNT";
            this.chtSalesTrend.TitleLeft.Visible = true;
            this.chtSalesTrend.TitleTop.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold);
            this.chtSalesTrend.TitleTop.HorizontalAlign = System.Drawing.StringAlignment.Center;
            this.chtSalesTrend.TitleTop.Text = "SALES TREND";
            this.chtSalesTrend.Tooltips.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F);
            this.chtSalesTrend.MouseUp += new System.Windows.Forms.MouseEventHandler(this.chart_MouseUp);
            // 
            // ultraTabPageControl3
            // 
            this.ultraTabPageControl3.Controls.Add(this.chtSaleByDepartment);
            this.ultraTabPageControl3.Location = new System.Drawing.Point(-10000, -10000);
            this.ultraTabPageControl3.Name = "ultraTabPageControl3";
            this.ultraTabPageControl3.Size = new System.Drawing.Size(1034, 479);
            // 
            // chtSaleByDepartment
            // 
            this.chtSaleByDepartment.Axis.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(248)))), ((int)(((byte)(220)))));
            paintElement3.ElementType = Infragistics.UltraChart.Shared.Styles.PaintElementType.None;
            paintElement3.Fill = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(248)))), ((int)(((byte)(220)))));
            this.chtSaleByDepartment.Axis.PE = paintElement3;
            this.chtSaleByDepartment.Axis.X.Labels.HorizontalAlign = System.Drawing.StringAlignment.Near;
            this.chtSaleByDepartment.Axis.X.Labels.ItemFormatString = "<ITEM_LABEL>";
            this.chtSaleByDepartment.Axis.X.Labels.Orientation = Infragistics.UltraChart.Shared.Styles.TextOrientation.Custom;
            this.chtSaleByDepartment.Axis.X.Labels.OrientationAngle = 315;
            this.chtSaleByDepartment.Axis.X.Labels.SeriesLabels.HorizontalAlign = System.Drawing.StringAlignment.Near;
            this.chtSaleByDepartment.Axis.X.Labels.SeriesLabels.Orientation = Infragistics.UltraChart.Shared.Styles.TextOrientation.Horizontal;
            this.chtSaleByDepartment.Axis.X.Labels.SeriesLabels.VerticalAlign = System.Drawing.StringAlignment.Center;
            this.chtSaleByDepartment.Axis.X.Labels.VerticalAlign = System.Drawing.StringAlignment.Center;
            this.chtSaleByDepartment.Axis.X.MajorGridLines.AlphaLevel = ((byte)(255));
            this.chtSaleByDepartment.Axis.X.MajorGridLines.Color = System.Drawing.Color.Gainsboro;
            this.chtSaleByDepartment.Axis.X.MajorGridLines.DrawStyle = Infragistics.UltraChart.Shared.Styles.LineDrawStyle.Dot;
            this.chtSaleByDepartment.Axis.X.MajorGridLines.Visible = true;
            this.chtSaleByDepartment.Axis.X.MinorGridLines.AlphaLevel = ((byte)(255));
            this.chtSaleByDepartment.Axis.X.MinorGridLines.Color = System.Drawing.Color.LightGray;
            this.chtSaleByDepartment.Axis.X.MinorGridLines.DrawStyle = Infragistics.UltraChart.Shared.Styles.LineDrawStyle.Dot;
            this.chtSaleByDepartment.Axis.X.MinorGridLines.Visible = false;
            this.chtSaleByDepartment.Axis.X.Visible = true;
            this.chtSaleByDepartment.Axis.X2.Labels.HorizontalAlign = System.Drawing.StringAlignment.Near;
            this.chtSaleByDepartment.Axis.X2.Labels.ItemFormatString = "<ITEM_LABEL>";
            this.chtSaleByDepartment.Axis.X2.Labels.Orientation = Infragistics.UltraChart.Shared.Styles.TextOrientation.VerticalLeftFacing;
            this.chtSaleByDepartment.Axis.X2.Labels.SeriesLabels.HorizontalAlign = System.Drawing.StringAlignment.Near;
            this.chtSaleByDepartment.Axis.X2.Labels.SeriesLabels.Orientation = Infragistics.UltraChart.Shared.Styles.TextOrientation.Horizontal;
            this.chtSaleByDepartment.Axis.X2.Labels.SeriesLabels.VerticalAlign = System.Drawing.StringAlignment.Center;
            this.chtSaleByDepartment.Axis.X2.Labels.VerticalAlign = System.Drawing.StringAlignment.Center;
            this.chtSaleByDepartment.Axis.X2.MajorGridLines.AlphaLevel = ((byte)(255));
            this.chtSaleByDepartment.Axis.X2.MajorGridLines.Color = System.Drawing.Color.Gainsboro;
            this.chtSaleByDepartment.Axis.X2.MajorGridLines.DrawStyle = Infragistics.UltraChart.Shared.Styles.LineDrawStyle.Dot;
            this.chtSaleByDepartment.Axis.X2.MajorGridLines.Visible = true;
            this.chtSaleByDepartment.Axis.X2.MinorGridLines.AlphaLevel = ((byte)(255));
            this.chtSaleByDepartment.Axis.X2.MinorGridLines.Color = System.Drawing.Color.LightGray;
            this.chtSaleByDepartment.Axis.X2.MinorGridLines.DrawStyle = Infragistics.UltraChart.Shared.Styles.LineDrawStyle.Dot;
            this.chtSaleByDepartment.Axis.X2.MinorGridLines.Visible = false;
            this.chtSaleByDepartment.Axis.X2.Visible = false;
            this.chtSaleByDepartment.Axis.Y.Labels.HorizontalAlign = System.Drawing.StringAlignment.Far;
			this.chtSaleByDepartment.Axis.Y.Labels.ItemFormatString = "<DATA_VALUE:" + Configuration.CInfo.CurrencySymbol.ToString() + " 00.00>";
            this.chtSaleByDepartment.Axis.Y.Labels.Orientation = Infragistics.UltraChart.Shared.Styles.TextOrientation.Horizontal;
            this.chtSaleByDepartment.Axis.Y.Labels.SeriesLabels.FormatString = "";
            this.chtSaleByDepartment.Axis.Y.Labels.SeriesLabels.HorizontalAlign = System.Drawing.StringAlignment.Far;
            this.chtSaleByDepartment.Axis.Y.Labels.SeriesLabels.Orientation = Infragistics.UltraChart.Shared.Styles.TextOrientation.VerticalLeftFacing;
            this.chtSaleByDepartment.Axis.Y.Labels.SeriesLabels.VerticalAlign = System.Drawing.StringAlignment.Center;
            this.chtSaleByDepartment.Axis.Y.Labels.VerticalAlign = System.Drawing.StringAlignment.Center;
            this.chtSaleByDepartment.Axis.Y.MajorGridLines.AlphaLevel = ((byte)(255));
            this.chtSaleByDepartment.Axis.Y.MajorGridLines.Color = System.Drawing.Color.Gainsboro;
            this.chtSaleByDepartment.Axis.Y.MajorGridLines.DrawStyle = Infragistics.UltraChart.Shared.Styles.LineDrawStyle.Dot;
            this.chtSaleByDepartment.Axis.Y.MajorGridLines.Visible = true;
            this.chtSaleByDepartment.Axis.Y.MinorGridLines.AlphaLevel = ((byte)(255));
            this.chtSaleByDepartment.Axis.Y.MinorGridLines.Color = System.Drawing.Color.LightGray;
            this.chtSaleByDepartment.Axis.Y.MinorGridLines.DrawStyle = Infragistics.UltraChart.Shared.Styles.LineDrawStyle.Dot;
            this.chtSaleByDepartment.Axis.Y.MinorGridLines.Visible = false;
            this.chtSaleByDepartment.Axis.Y.Visible = true;
            this.chtSaleByDepartment.Axis.Y2.Labels.HorizontalAlign = System.Drawing.StringAlignment.Near;
            this.chtSaleByDepartment.Axis.Y2.Labels.ItemFormatString = "<DATA_VALUE:00.00>";
            this.chtSaleByDepartment.Axis.Y2.Labels.Orientation = Infragistics.UltraChart.Shared.Styles.TextOrientation.Horizontal;
            this.chtSaleByDepartment.Axis.Y2.Labels.SeriesLabels.FormatString = "";
            this.chtSaleByDepartment.Axis.Y2.Labels.SeriesLabels.HorizontalAlign = System.Drawing.StringAlignment.Near;
            this.chtSaleByDepartment.Axis.Y2.Labels.SeriesLabels.Orientation = Infragistics.UltraChart.Shared.Styles.TextOrientation.VerticalLeftFacing;
            this.chtSaleByDepartment.Axis.Y2.Labels.SeriesLabels.VerticalAlign = System.Drawing.StringAlignment.Center;
            this.chtSaleByDepartment.Axis.Y2.Labels.VerticalAlign = System.Drawing.StringAlignment.Center;
            this.chtSaleByDepartment.Axis.Y2.MajorGridLines.AlphaLevel = ((byte)(255));
            this.chtSaleByDepartment.Axis.Y2.MajorGridLines.Color = System.Drawing.Color.Gainsboro;
            this.chtSaleByDepartment.Axis.Y2.MajorGridLines.DrawStyle = Infragistics.UltraChart.Shared.Styles.LineDrawStyle.Dot;
            this.chtSaleByDepartment.Axis.Y2.MajorGridLines.Visible = true;
            this.chtSaleByDepartment.Axis.Y2.MinorGridLines.AlphaLevel = ((byte)(255));
            this.chtSaleByDepartment.Axis.Y2.MinorGridLines.Color = System.Drawing.Color.LightGray;
            this.chtSaleByDepartment.Axis.Y2.MinorGridLines.DrawStyle = Infragistics.UltraChart.Shared.Styles.LineDrawStyle.Dot;
            this.chtSaleByDepartment.Axis.Y2.MinorGridLines.Visible = false;
            this.chtSaleByDepartment.Axis.Y2.Visible = false;
            this.chtSaleByDepartment.Axis.Z.Labels.HorizontalAlign = System.Drawing.StringAlignment.Near;
            this.chtSaleByDepartment.Axis.Z.Labels.ItemFormatString = "<DATA_VALUE:00.00>";
            this.chtSaleByDepartment.Axis.Z.Labels.Orientation = Infragistics.UltraChart.Shared.Styles.TextOrientation.Horizontal;
            this.chtSaleByDepartment.Axis.Z.Labels.SeriesLabels.HorizontalAlign = System.Drawing.StringAlignment.Near;
            this.chtSaleByDepartment.Axis.Z.Labels.SeriesLabels.Orientation = Infragistics.UltraChart.Shared.Styles.TextOrientation.Horizontal;
            this.chtSaleByDepartment.Axis.Z.Labels.SeriesLabels.VerticalAlign = System.Drawing.StringAlignment.Center;
            this.chtSaleByDepartment.Axis.Z.Labels.VerticalAlign = System.Drawing.StringAlignment.Center;
            this.chtSaleByDepartment.Axis.Z.MajorGridLines.AlphaLevel = ((byte)(255));
            this.chtSaleByDepartment.Axis.Z.MajorGridLines.Color = System.Drawing.Color.Gainsboro;
            this.chtSaleByDepartment.Axis.Z.MajorGridLines.DrawStyle = Infragistics.UltraChart.Shared.Styles.LineDrawStyle.Dot;
            this.chtSaleByDepartment.Axis.Z.MajorGridLines.Visible = true;
            this.chtSaleByDepartment.Axis.Z.MinorGridLines.AlphaLevel = ((byte)(255));
            this.chtSaleByDepartment.Axis.Z.MinorGridLines.Color = System.Drawing.Color.LightGray;
            this.chtSaleByDepartment.Axis.Z.MinorGridLines.DrawStyle = Infragistics.UltraChart.Shared.Styles.LineDrawStyle.Dot;
            this.chtSaleByDepartment.Axis.Z.MinorGridLines.Visible = false;
            this.chtSaleByDepartment.Axis.Z.Visible = false;
            this.chtSaleByDepartment.Axis.Z2.Labels.HorizontalAlign = System.Drawing.StringAlignment.Near;
            this.chtSaleByDepartment.Axis.Z2.Labels.ItemFormatString = "";
            this.chtSaleByDepartment.Axis.Z2.Labels.Orientation = Infragistics.UltraChart.Shared.Styles.TextOrientation.Horizontal;
            this.chtSaleByDepartment.Axis.Z2.Labels.SeriesLabels.HorizontalAlign = System.Drawing.StringAlignment.Near;
            this.chtSaleByDepartment.Axis.Z2.Labels.SeriesLabels.Orientation = Infragistics.UltraChart.Shared.Styles.TextOrientation.Horizontal;
            this.chtSaleByDepartment.Axis.Z2.Labels.SeriesLabels.VerticalAlign = System.Drawing.StringAlignment.Center;
            this.chtSaleByDepartment.Axis.Z2.Labels.VerticalAlign = System.Drawing.StringAlignment.Center;
            this.chtSaleByDepartment.Axis.Z2.MajorGridLines.AlphaLevel = ((byte)(255));
            this.chtSaleByDepartment.Axis.Z2.MajorGridLines.Color = System.Drawing.Color.Gainsboro;
            this.chtSaleByDepartment.Axis.Z2.MajorGridLines.DrawStyle = Infragistics.UltraChart.Shared.Styles.LineDrawStyle.Dot;
            this.chtSaleByDepartment.Axis.Z2.MajorGridLines.Visible = true;
            this.chtSaleByDepartment.Axis.Z2.MinorGridLines.AlphaLevel = ((byte)(255));
            this.chtSaleByDepartment.Axis.Z2.MinorGridLines.Color = System.Drawing.Color.LightGray;
            this.chtSaleByDepartment.Axis.Z2.MinorGridLines.DrawStyle = Infragistics.UltraChart.Shared.Styles.LineDrawStyle.Dot;
            this.chtSaleByDepartment.Axis.Z2.MinorGridLines.Visible = false;
            this.chtSaleByDepartment.Axis.Z2.Visible = false;
            this.chtSaleByDepartment.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("chtSaleByDepartment.BackgroundImage")));
            this.chtSaleByDepartment.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.chtSaleByDepartment.ColorModel.AlphaLevel = ((byte)(150));
            columnChartAppearance1.ColumnSpacing = 1;
            this.chtSaleByDepartment.ColumnChart = columnChartAppearance1;
            this.chtSaleByDepartment.Data.SwapRowsAndColumns = true;
            this.chtSaleByDepartment.Data.ZeroAligned = true;
            this.chtSaleByDepartment.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chtSaleByDepartment.Effects.Effects.Add(threeDEffect2);
            this.chtSaleByDepartment.Effects.Effects.Add(gradientEffect3);
            this.chtSaleByDepartment.ForeColor = System.Drawing.SystemColors.ControlText;
            this.chtSaleByDepartment.Location = new System.Drawing.Point(0, 0);
            this.chtSaleByDepartment.Name = "chtSaleByDepartment";
            this.chtSaleByDepartment.Size = new System.Drawing.Size(1034, 479);
            this.chtSaleByDepartment.TabIndex = 10;
            this.chtSaleByDepartment.TitleBottom.Visible = false;
            this.chtSaleByDepartment.TitleLeft.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold);
            this.chtSaleByDepartment.TitleLeft.HorizontalAlign = System.Drawing.StringAlignment.Center;
            this.chtSaleByDepartment.TitleLeft.Text = "AMOUNT";
            this.chtSaleByDepartment.TitleLeft.Visible = true;
            this.chtSaleByDepartment.TitleTop.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold);
            this.chtSaleByDepartment.TitleTop.HorizontalAlign = System.Drawing.StringAlignment.Center;
            this.chtSaleByDepartment.TitleTop.Text = "SALE BY DEPARTMENT";
            this.chtSaleByDepartment.Tooltips.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F);
            // 
            // ultraTabPageControl4
            // 
            this.ultraTabPageControl4.Controls.Add(this.chtSaleByEmp);
            this.ultraTabPageControl4.Location = new System.Drawing.Point(-10000, -10000);
            this.ultraTabPageControl4.Name = "ultraTabPageControl4";
            this.ultraTabPageControl4.Size = new System.Drawing.Size(1034, 479);
            // 
//			'UltraChart' properties's serialization: Since 'ChartType' changes the way axes look,
//			'ChartType' must be persisted ahead of any Axes change made in design time.
//		
            this.chtSaleByEmp.ChartType = Infragistics.UltraChart.Shared.Styles.ChartType.PieChart;
            // 
            // chtSaleByEmp
            // 
            this.chtSaleByEmp.Axis.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(248)))), ((int)(((byte)(220)))));
            paintElement4.ElementType = Infragistics.UltraChart.Shared.Styles.PaintElementType.None;
            paintElement4.Fill = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(248)))), ((int)(((byte)(220)))));
            this.chtSaleByEmp.Axis.PE = paintElement4;
            this.chtSaleByEmp.Axis.X.Labels.HorizontalAlign = System.Drawing.StringAlignment.Near;
            this.chtSaleByEmp.Axis.X.Labels.ItemFormatString = "<ITEM_LABEL>";
            this.chtSaleByEmp.Axis.X.Labels.Orientation = Infragistics.UltraChart.Shared.Styles.TextOrientation.Horizontal;
            this.chtSaleByEmp.Axis.X.Labels.SeriesLabels.FormatString = "";
            this.chtSaleByEmp.Axis.X.Labels.SeriesLabels.HorizontalAlign = System.Drawing.StringAlignment.Near;
            this.chtSaleByEmp.Axis.X.Labels.SeriesLabels.Orientation = Infragistics.UltraChart.Shared.Styles.TextOrientation.Horizontal;
            this.chtSaleByEmp.Axis.X.Labels.SeriesLabels.VerticalAlign = System.Drawing.StringAlignment.Center;
            this.chtSaleByEmp.Axis.X.Labels.VerticalAlign = System.Drawing.StringAlignment.Center;
            this.chtSaleByEmp.Axis.X.MajorGridLines.AlphaLevel = ((byte)(255));
            this.chtSaleByEmp.Axis.X.MajorGridLines.Color = System.Drawing.Color.Gainsboro;
            this.chtSaleByEmp.Axis.X.MajorGridLines.DrawStyle = Infragistics.UltraChart.Shared.Styles.LineDrawStyle.Dot;
            this.chtSaleByEmp.Axis.X.MajorGridLines.Visible = true;
            this.chtSaleByEmp.Axis.X.MinorGridLines.AlphaLevel = ((byte)(255));
            this.chtSaleByEmp.Axis.X.MinorGridLines.Color = System.Drawing.Color.LightGray;
            this.chtSaleByEmp.Axis.X.MinorGridLines.DrawStyle = Infragistics.UltraChart.Shared.Styles.LineDrawStyle.Dot;
            this.chtSaleByEmp.Axis.X.MinorGridLines.Visible = false;
            this.chtSaleByEmp.Axis.X.Visible = true;
            this.chtSaleByEmp.Axis.X2.Labels.HorizontalAlign = System.Drawing.StringAlignment.Near;
            this.chtSaleByEmp.Axis.X2.Labels.ItemFormatString = "";
            this.chtSaleByEmp.Axis.X2.Labels.Orientation = Infragistics.UltraChart.Shared.Styles.TextOrientation.Horizontal;
            this.chtSaleByEmp.Axis.X2.Labels.SeriesLabels.FormatString = "";
            this.chtSaleByEmp.Axis.X2.Labels.SeriesLabels.HorizontalAlign = System.Drawing.StringAlignment.Near;
            this.chtSaleByEmp.Axis.X2.Labels.SeriesLabels.Orientation = Infragistics.UltraChart.Shared.Styles.TextOrientation.Horizontal;
            this.chtSaleByEmp.Axis.X2.Labels.SeriesLabels.VerticalAlign = System.Drawing.StringAlignment.Center;
            this.chtSaleByEmp.Axis.X2.Labels.VerticalAlign = System.Drawing.StringAlignment.Center;
            this.chtSaleByEmp.Axis.X2.Labels.Visible = false;
            this.chtSaleByEmp.Axis.X2.MajorGridLines.AlphaLevel = ((byte)(255));
            this.chtSaleByEmp.Axis.X2.MajorGridLines.Color = System.Drawing.Color.Gainsboro;
            this.chtSaleByEmp.Axis.X2.MajorGridLines.DrawStyle = Infragistics.UltraChart.Shared.Styles.LineDrawStyle.Dot;
            this.chtSaleByEmp.Axis.X2.MajorGridLines.Visible = true;
            this.chtSaleByEmp.Axis.X2.MinorGridLines.AlphaLevel = ((byte)(255));
            this.chtSaleByEmp.Axis.X2.MinorGridLines.Color = System.Drawing.Color.LightGray;
            this.chtSaleByEmp.Axis.X2.MinorGridLines.DrawStyle = Infragistics.UltraChart.Shared.Styles.LineDrawStyle.Dot;
            this.chtSaleByEmp.Axis.X2.MinorGridLines.Visible = false;
            this.chtSaleByEmp.Axis.X2.Visible = false;
            this.chtSaleByEmp.Axis.Y.Labels.HorizontalAlign = System.Drawing.StringAlignment.Near;
            this.chtSaleByEmp.Axis.Y.Labels.ItemFormatString = "<DATA_VALUE:00.00>";
            this.chtSaleByEmp.Axis.Y.Labels.Orientation = Infragistics.UltraChart.Shared.Styles.TextOrientation.Horizontal;
            this.chtSaleByEmp.Axis.Y.Labels.SeriesLabels.FormatString = "";
            this.chtSaleByEmp.Axis.Y.Labels.SeriesLabels.HorizontalAlign = System.Drawing.StringAlignment.Near;
            this.chtSaleByEmp.Axis.Y.Labels.SeriesLabels.Orientation = Infragistics.UltraChart.Shared.Styles.TextOrientation.Horizontal;
            this.chtSaleByEmp.Axis.Y.Labels.SeriesLabels.VerticalAlign = System.Drawing.StringAlignment.Center;
            this.chtSaleByEmp.Axis.Y.Labels.VerticalAlign = System.Drawing.StringAlignment.Center;
            this.chtSaleByEmp.Axis.Y.MajorGridLines.AlphaLevel = ((byte)(255));
            this.chtSaleByEmp.Axis.Y.MajorGridLines.Color = System.Drawing.Color.Gainsboro;
            this.chtSaleByEmp.Axis.Y.MajorGridLines.DrawStyle = Infragistics.UltraChart.Shared.Styles.LineDrawStyle.Dot;
            this.chtSaleByEmp.Axis.Y.MajorGridLines.Visible = true;
            this.chtSaleByEmp.Axis.Y.MinorGridLines.AlphaLevel = ((byte)(255));
            this.chtSaleByEmp.Axis.Y.MinorGridLines.Color = System.Drawing.Color.LightGray;
            this.chtSaleByEmp.Axis.Y.MinorGridLines.DrawStyle = Infragistics.UltraChart.Shared.Styles.LineDrawStyle.Dot;
            this.chtSaleByEmp.Axis.Y.MinorGridLines.Visible = false;
            this.chtSaleByEmp.Axis.Y.Visible = true;
            this.chtSaleByEmp.Axis.Y2.Labels.HorizontalAlign = System.Drawing.StringAlignment.Near;
            this.chtSaleByEmp.Axis.Y2.Labels.ItemFormatString = "";
            this.chtSaleByEmp.Axis.Y2.Labels.Orientation = Infragistics.UltraChart.Shared.Styles.TextOrientation.Horizontal;
            this.chtSaleByEmp.Axis.Y2.Labels.SeriesLabels.FormatString = "";
            this.chtSaleByEmp.Axis.Y2.Labels.SeriesLabels.HorizontalAlign = System.Drawing.StringAlignment.Near;
            this.chtSaleByEmp.Axis.Y2.Labels.SeriesLabels.Orientation = Infragistics.UltraChart.Shared.Styles.TextOrientation.Horizontal;
            this.chtSaleByEmp.Axis.Y2.Labels.SeriesLabels.VerticalAlign = System.Drawing.StringAlignment.Center;
            this.chtSaleByEmp.Axis.Y2.Labels.VerticalAlign = System.Drawing.StringAlignment.Center;
            this.chtSaleByEmp.Axis.Y2.Labels.Visible = false;
            this.chtSaleByEmp.Axis.Y2.MajorGridLines.AlphaLevel = ((byte)(255));
            this.chtSaleByEmp.Axis.Y2.MajorGridLines.Color = System.Drawing.Color.Gainsboro;
            this.chtSaleByEmp.Axis.Y2.MajorGridLines.DrawStyle = Infragistics.UltraChart.Shared.Styles.LineDrawStyle.Dot;
            this.chtSaleByEmp.Axis.Y2.MajorGridLines.Visible = true;
            this.chtSaleByEmp.Axis.Y2.MinorGridLines.AlphaLevel = ((byte)(255));
            this.chtSaleByEmp.Axis.Y2.MinorGridLines.Color = System.Drawing.Color.LightGray;
            this.chtSaleByEmp.Axis.Y2.MinorGridLines.DrawStyle = Infragistics.UltraChart.Shared.Styles.LineDrawStyle.Dot;
            this.chtSaleByEmp.Axis.Y2.MinorGridLines.Visible = false;
            this.chtSaleByEmp.Axis.Y2.Visible = false;
            this.chtSaleByEmp.Axis.Z.Labels.HorizontalAlign = System.Drawing.StringAlignment.Near;
            this.chtSaleByEmp.Axis.Z.Labels.ItemFormatString = "";
            this.chtSaleByEmp.Axis.Z.Labels.Orientation = Infragistics.UltraChart.Shared.Styles.TextOrientation.Horizontal;
            this.chtSaleByEmp.Axis.Z.Labels.SeriesLabels.HorizontalAlign = System.Drawing.StringAlignment.Near;
            this.chtSaleByEmp.Axis.Z.Labels.SeriesLabels.Orientation = Infragistics.UltraChart.Shared.Styles.TextOrientation.Horizontal;
            this.chtSaleByEmp.Axis.Z.Labels.SeriesLabels.VerticalAlign = System.Drawing.StringAlignment.Center;
            this.chtSaleByEmp.Axis.Z.Labels.VerticalAlign = System.Drawing.StringAlignment.Center;
            this.chtSaleByEmp.Axis.Z.MajorGridLines.AlphaLevel = ((byte)(255));
            this.chtSaleByEmp.Axis.Z.MajorGridLines.Color = System.Drawing.Color.Gainsboro;
            this.chtSaleByEmp.Axis.Z.MajorGridLines.DrawStyle = Infragistics.UltraChart.Shared.Styles.LineDrawStyle.Dot;
            this.chtSaleByEmp.Axis.Z.MajorGridLines.Visible = true;
            this.chtSaleByEmp.Axis.Z.MinorGridLines.AlphaLevel = ((byte)(255));
            this.chtSaleByEmp.Axis.Z.MinorGridLines.Color = System.Drawing.Color.LightGray;
            this.chtSaleByEmp.Axis.Z.MinorGridLines.DrawStyle = Infragistics.UltraChart.Shared.Styles.LineDrawStyle.Dot;
            this.chtSaleByEmp.Axis.Z.MinorGridLines.Visible = false;
            this.chtSaleByEmp.Axis.Z.Visible = false;
            this.chtSaleByEmp.Axis.Z2.Labels.HorizontalAlign = System.Drawing.StringAlignment.Near;
            this.chtSaleByEmp.Axis.Z2.Labels.ItemFormatString = "";
            this.chtSaleByEmp.Axis.Z2.Labels.Orientation = Infragistics.UltraChart.Shared.Styles.TextOrientation.Horizontal;
            this.chtSaleByEmp.Axis.Z2.Labels.SeriesLabels.HorizontalAlign = System.Drawing.StringAlignment.Near;
            this.chtSaleByEmp.Axis.Z2.Labels.SeriesLabels.Orientation = Infragistics.UltraChart.Shared.Styles.TextOrientation.Horizontal;
            this.chtSaleByEmp.Axis.Z2.Labels.SeriesLabels.VerticalAlign = System.Drawing.StringAlignment.Center;
            this.chtSaleByEmp.Axis.Z2.Labels.VerticalAlign = System.Drawing.StringAlignment.Center;
            this.chtSaleByEmp.Axis.Z2.Labels.Visible = false;
            this.chtSaleByEmp.Axis.Z2.MajorGridLines.AlphaLevel = ((byte)(255));
            this.chtSaleByEmp.Axis.Z2.MajorGridLines.Color = System.Drawing.Color.Gainsboro;
            this.chtSaleByEmp.Axis.Z2.MajorGridLines.DrawStyle = Infragistics.UltraChart.Shared.Styles.LineDrawStyle.Dot;
            this.chtSaleByEmp.Axis.Z2.MajorGridLines.Visible = true;
            this.chtSaleByEmp.Axis.Z2.MinorGridLines.AlphaLevel = ((byte)(255));
            this.chtSaleByEmp.Axis.Z2.MinorGridLines.Color = System.Drawing.Color.LightGray;
            this.chtSaleByEmp.Axis.Z2.MinorGridLines.DrawStyle = Infragistics.UltraChart.Shared.Styles.LineDrawStyle.Dot;
            this.chtSaleByEmp.Axis.Z2.MinorGridLines.Visible = false;
            this.chtSaleByEmp.Axis.Z2.Visible = false;
            this.chtSaleByEmp.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("chtSaleByEmp.BackgroundImage")));
            this.chtSaleByEmp.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.chtSaleByEmp.ColorModel.AlphaLevel = ((byte)(150));
            this.chtSaleByEmp.Data.ZeroAligned = true;
            this.chtSaleByEmp.Dock = System.Windows.Forms.DockStyle.Fill;
            gradientEffect4.Style = Infragistics.UltraChart.Shared.Styles.GradientStyle.Elliptical;
            shadowEffect2.Angle = 45D;
            this.chtSaleByEmp.Effects.Effects.Add(threeDEffect3);
            this.chtSaleByEmp.Effects.Effects.Add(gradientEffect4);
            this.chtSaleByEmp.Effects.Effects.Add(shadowEffect2);
            this.chtSaleByEmp.ForeColor = System.Drawing.SystemColors.ControlText;
            this.chtSaleByEmp.Legend.Visible = true;
            this.chtSaleByEmp.Location = new System.Drawing.Point(0, 0);
            this.chtSaleByEmp.Name = "chtSaleByEmp";
            pieChartAppearance1.ColumnIndex = 0;
            pieChartAppearance1.RadiusFactor = 80;
            this.chtSaleByEmp.PieChart = pieChartAppearance1;
            this.chtSaleByEmp.Size = new System.Drawing.Size(1034, 479);
            this.chtSaleByEmp.TabIndex = 11;
            this.chtSaleByEmp.TitleBottom.Visible = false;
            this.chtSaleByEmp.TitleTop.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold);
            this.chtSaleByEmp.TitleTop.HorizontalAlign = System.Drawing.StringAlignment.Center;
            this.chtSaleByEmp.TitleTop.Text = "SALE BY EMPLOYEE";
            this.chtSaleByEmp.Tooltips.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F);
            // 
            // ultraTabPageControl5
            // 
            this.ultraTabPageControl5.Controls.Add(this.chtSaleByRegister);
            this.ultraTabPageControl5.Location = new System.Drawing.Point(-10000, -10000);
            this.ultraTabPageControl5.Name = "ultraTabPageControl5";
            this.ultraTabPageControl5.Size = new System.Drawing.Size(1034, 479);
            // 
//			'UltraChart' properties's serialization: Since 'ChartType' changes the way axes look,
//			'ChartType' must be persisted ahead of any Axes change made in design time.
//		
            this.chtSaleByRegister.ChartType = Infragistics.UltraChart.Shared.Styles.ChartType.PieChart;
            // 
            // chtSaleByRegister
            // 
            this.chtSaleByRegister.Axis.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(248)))), ((int)(((byte)(220)))));
            paintElement5.ElementType = Infragistics.UltraChart.Shared.Styles.PaintElementType.None;
            paintElement5.Fill = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(248)))), ((int)(((byte)(220)))));
            this.chtSaleByRegister.Axis.PE = paintElement5;
            this.chtSaleByRegister.Axis.X.Labels.HorizontalAlign = System.Drawing.StringAlignment.Near;
            this.chtSaleByRegister.Axis.X.Labels.ItemFormatString = "<ITEM_LABEL>";
            this.chtSaleByRegister.Axis.X.Labels.Orientation = Infragistics.UltraChart.Shared.Styles.TextOrientation.Horizontal;
            this.chtSaleByRegister.Axis.X.Labels.SeriesLabels.FormatString = "";
            this.chtSaleByRegister.Axis.X.Labels.SeriesLabels.HorizontalAlign = System.Drawing.StringAlignment.Near;
            this.chtSaleByRegister.Axis.X.Labels.SeriesLabels.Orientation = Infragistics.UltraChart.Shared.Styles.TextOrientation.Horizontal;
            this.chtSaleByRegister.Axis.X.Labels.SeriesLabels.VerticalAlign = System.Drawing.StringAlignment.Center;
            this.chtSaleByRegister.Axis.X.Labels.VerticalAlign = System.Drawing.StringAlignment.Center;
            this.chtSaleByRegister.Axis.X.MajorGridLines.AlphaLevel = ((byte)(255));
            this.chtSaleByRegister.Axis.X.MajorGridLines.Color = System.Drawing.Color.Gainsboro;
            this.chtSaleByRegister.Axis.X.MajorGridLines.DrawStyle = Infragistics.UltraChart.Shared.Styles.LineDrawStyle.Dot;
            this.chtSaleByRegister.Axis.X.MajorGridLines.Visible = true;
            this.chtSaleByRegister.Axis.X.MinorGridLines.AlphaLevel = ((byte)(255));
            this.chtSaleByRegister.Axis.X.MinorGridLines.Color = System.Drawing.Color.LightGray;
            this.chtSaleByRegister.Axis.X.MinorGridLines.DrawStyle = Infragistics.UltraChart.Shared.Styles.LineDrawStyle.Dot;
            this.chtSaleByRegister.Axis.X.MinorGridLines.Visible = false;
            this.chtSaleByRegister.Axis.X.Visible = true;
            this.chtSaleByRegister.Axis.X2.Labels.HorizontalAlign = System.Drawing.StringAlignment.Near;
            this.chtSaleByRegister.Axis.X2.Labels.ItemFormatString = "";
            this.chtSaleByRegister.Axis.X2.Labels.Orientation = Infragistics.UltraChart.Shared.Styles.TextOrientation.Horizontal;
            this.chtSaleByRegister.Axis.X2.Labels.SeriesLabels.FormatString = "";
            this.chtSaleByRegister.Axis.X2.Labels.SeriesLabels.HorizontalAlign = System.Drawing.StringAlignment.Near;
            this.chtSaleByRegister.Axis.X2.Labels.SeriesLabels.Orientation = Infragistics.UltraChart.Shared.Styles.TextOrientation.Horizontal;
            this.chtSaleByRegister.Axis.X2.Labels.SeriesLabels.VerticalAlign = System.Drawing.StringAlignment.Center;
            this.chtSaleByRegister.Axis.X2.Labels.VerticalAlign = System.Drawing.StringAlignment.Center;
            this.chtSaleByRegister.Axis.X2.Labels.Visible = false;
            this.chtSaleByRegister.Axis.X2.MajorGridLines.AlphaLevel = ((byte)(255));
            this.chtSaleByRegister.Axis.X2.MajorGridLines.Color = System.Drawing.Color.Gainsboro;
            this.chtSaleByRegister.Axis.X2.MajorGridLines.DrawStyle = Infragistics.UltraChart.Shared.Styles.LineDrawStyle.Dot;
            this.chtSaleByRegister.Axis.X2.MajorGridLines.Visible = true;
            this.chtSaleByRegister.Axis.X2.MinorGridLines.AlphaLevel = ((byte)(255));
            this.chtSaleByRegister.Axis.X2.MinorGridLines.Color = System.Drawing.Color.LightGray;
            this.chtSaleByRegister.Axis.X2.MinorGridLines.DrawStyle = Infragistics.UltraChart.Shared.Styles.LineDrawStyle.Dot;
            this.chtSaleByRegister.Axis.X2.MinorGridLines.Visible = false;
            this.chtSaleByRegister.Axis.X2.Visible = false;
            this.chtSaleByRegister.Axis.Y.Labels.HorizontalAlign = System.Drawing.StringAlignment.Near;
            this.chtSaleByRegister.Axis.Y.Labels.ItemFormatString = "<DATA_VALUE:00.00>";
            this.chtSaleByRegister.Axis.Y.Labels.Orientation = Infragistics.UltraChart.Shared.Styles.TextOrientation.Horizontal;
            this.chtSaleByRegister.Axis.Y.Labels.SeriesLabels.FormatString = "";
            this.chtSaleByRegister.Axis.Y.Labels.SeriesLabels.HorizontalAlign = System.Drawing.StringAlignment.Near;
            this.chtSaleByRegister.Axis.Y.Labels.SeriesLabels.Orientation = Infragistics.UltraChart.Shared.Styles.TextOrientation.Horizontal;
            this.chtSaleByRegister.Axis.Y.Labels.SeriesLabels.VerticalAlign = System.Drawing.StringAlignment.Center;
            this.chtSaleByRegister.Axis.Y.Labels.VerticalAlign = System.Drawing.StringAlignment.Center;
            this.chtSaleByRegister.Axis.Y.MajorGridLines.AlphaLevel = ((byte)(255));
            this.chtSaleByRegister.Axis.Y.MajorGridLines.Color = System.Drawing.Color.Gainsboro;
            this.chtSaleByRegister.Axis.Y.MajorGridLines.DrawStyle = Infragistics.UltraChart.Shared.Styles.LineDrawStyle.Dot;
            this.chtSaleByRegister.Axis.Y.MajorGridLines.Visible = true;
            this.chtSaleByRegister.Axis.Y.MinorGridLines.AlphaLevel = ((byte)(255));
            this.chtSaleByRegister.Axis.Y.MinorGridLines.Color = System.Drawing.Color.LightGray;
            this.chtSaleByRegister.Axis.Y.MinorGridLines.DrawStyle = Infragistics.UltraChart.Shared.Styles.LineDrawStyle.Dot;
            this.chtSaleByRegister.Axis.Y.MinorGridLines.Visible = false;
            this.chtSaleByRegister.Axis.Y.Visible = true;
            this.chtSaleByRegister.Axis.Y2.Labels.HorizontalAlign = System.Drawing.StringAlignment.Near;
            this.chtSaleByRegister.Axis.Y2.Labels.ItemFormatString = "";
            this.chtSaleByRegister.Axis.Y2.Labels.Orientation = Infragistics.UltraChart.Shared.Styles.TextOrientation.Horizontal;
            this.chtSaleByRegister.Axis.Y2.Labels.SeriesLabels.FormatString = "";
            this.chtSaleByRegister.Axis.Y2.Labels.SeriesLabels.HorizontalAlign = System.Drawing.StringAlignment.Near;
            this.chtSaleByRegister.Axis.Y2.Labels.SeriesLabels.Orientation = Infragistics.UltraChart.Shared.Styles.TextOrientation.Horizontal;
            this.chtSaleByRegister.Axis.Y2.Labels.SeriesLabels.VerticalAlign = System.Drawing.StringAlignment.Center;
            this.chtSaleByRegister.Axis.Y2.Labels.VerticalAlign = System.Drawing.StringAlignment.Center;
            this.chtSaleByRegister.Axis.Y2.Labels.Visible = false;
            this.chtSaleByRegister.Axis.Y2.MajorGridLines.AlphaLevel = ((byte)(255));
            this.chtSaleByRegister.Axis.Y2.MajorGridLines.Color = System.Drawing.Color.Gainsboro;
            this.chtSaleByRegister.Axis.Y2.MajorGridLines.DrawStyle = Infragistics.UltraChart.Shared.Styles.LineDrawStyle.Dot;
            this.chtSaleByRegister.Axis.Y2.MajorGridLines.Visible = true;
            this.chtSaleByRegister.Axis.Y2.MinorGridLines.AlphaLevel = ((byte)(255));
            this.chtSaleByRegister.Axis.Y2.MinorGridLines.Color = System.Drawing.Color.LightGray;
            this.chtSaleByRegister.Axis.Y2.MinorGridLines.DrawStyle = Infragistics.UltraChart.Shared.Styles.LineDrawStyle.Dot;
            this.chtSaleByRegister.Axis.Y2.MinorGridLines.Visible = false;
            this.chtSaleByRegister.Axis.Y2.Visible = false;
            this.chtSaleByRegister.Axis.Z.Labels.HorizontalAlign = System.Drawing.StringAlignment.Near;
            this.chtSaleByRegister.Axis.Z.Labels.ItemFormatString = "<DATA_VALUE:00.00>";
            this.chtSaleByRegister.Axis.Z.Labels.Orientation = Infragistics.UltraChart.Shared.Styles.TextOrientation.Horizontal;
            this.chtSaleByRegister.Axis.Z.Labels.SeriesLabels.HorizontalAlign = System.Drawing.StringAlignment.Near;
            this.chtSaleByRegister.Axis.Z.Labels.SeriesLabels.Orientation = Infragistics.UltraChart.Shared.Styles.TextOrientation.Horizontal;
            this.chtSaleByRegister.Axis.Z.Labels.SeriesLabels.VerticalAlign = System.Drawing.StringAlignment.Center;
            this.chtSaleByRegister.Axis.Z.Labels.VerticalAlign = System.Drawing.StringAlignment.Center;
            this.chtSaleByRegister.Axis.Z.MajorGridLines.AlphaLevel = ((byte)(255));
            this.chtSaleByRegister.Axis.Z.MajorGridLines.Color = System.Drawing.Color.Gainsboro;
            this.chtSaleByRegister.Axis.Z.MajorGridLines.DrawStyle = Infragistics.UltraChart.Shared.Styles.LineDrawStyle.Dot;
            this.chtSaleByRegister.Axis.Z.MajorGridLines.Visible = true;
            this.chtSaleByRegister.Axis.Z.MinorGridLines.AlphaLevel = ((byte)(255));
            this.chtSaleByRegister.Axis.Z.MinorGridLines.Color = System.Drawing.Color.LightGray;
            this.chtSaleByRegister.Axis.Z.MinorGridLines.DrawStyle = Infragistics.UltraChart.Shared.Styles.LineDrawStyle.Dot;
            this.chtSaleByRegister.Axis.Z.MinorGridLines.Visible = false;
            this.chtSaleByRegister.Axis.Z.Visible = false;
            this.chtSaleByRegister.Axis.Z2.Labels.HorizontalAlign = System.Drawing.StringAlignment.Near;
            this.chtSaleByRegister.Axis.Z2.Labels.ItemFormatString = "";
            this.chtSaleByRegister.Axis.Z2.Labels.Orientation = Infragistics.UltraChart.Shared.Styles.TextOrientation.Horizontal;
            this.chtSaleByRegister.Axis.Z2.Labels.SeriesLabels.HorizontalAlign = System.Drawing.StringAlignment.Near;
            this.chtSaleByRegister.Axis.Z2.Labels.SeriesLabels.Orientation = Infragistics.UltraChart.Shared.Styles.TextOrientation.Horizontal;
            this.chtSaleByRegister.Axis.Z2.Labels.SeriesLabels.VerticalAlign = System.Drawing.StringAlignment.Center;
            this.chtSaleByRegister.Axis.Z2.Labels.VerticalAlign = System.Drawing.StringAlignment.Center;
            this.chtSaleByRegister.Axis.Z2.Labels.Visible = false;
            this.chtSaleByRegister.Axis.Z2.MajorGridLines.AlphaLevel = ((byte)(255));
            this.chtSaleByRegister.Axis.Z2.MajorGridLines.Color = System.Drawing.Color.Gainsboro;
            this.chtSaleByRegister.Axis.Z2.MajorGridLines.DrawStyle = Infragistics.UltraChart.Shared.Styles.LineDrawStyle.Dot;
            this.chtSaleByRegister.Axis.Z2.MajorGridLines.Visible = true;
            this.chtSaleByRegister.Axis.Z2.MinorGridLines.AlphaLevel = ((byte)(255));
            this.chtSaleByRegister.Axis.Z2.MinorGridLines.Color = System.Drawing.Color.LightGray;
            this.chtSaleByRegister.Axis.Z2.MinorGridLines.DrawStyle = Infragistics.UltraChart.Shared.Styles.LineDrawStyle.Dot;
            this.chtSaleByRegister.Axis.Z2.MinorGridLines.Visible = false;
            this.chtSaleByRegister.Axis.Z2.Visible = false;
            this.chtSaleByRegister.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("chtSaleByRegister.BackgroundImage")));
            this.chtSaleByRegister.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.chtSaleByRegister.Border.Color = System.Drawing.Color.Transparent;
            this.chtSaleByRegister.ColorModel.AlphaLevel = ((byte)(150));
            this.chtSaleByRegister.Data.ZeroAligned = true;
            this.chtSaleByRegister.Dock = System.Windows.Forms.DockStyle.Fill;
            gradientEffect5.Coloring = Infragistics.UltraChart.Shared.Styles.GradientColoringStyle.Lighten;
            gradientEffect5.Style = Infragistics.UltraChart.Shared.Styles.GradientStyle.Horizontal;
            this.chtSaleByRegister.Effects.Effects.Add(threeDEffect4);
            this.chtSaleByRegister.Effects.Effects.Add(gradientEffect5);
            this.chtSaleByRegister.ForeColor = System.Drawing.SystemColors.ControlText;
            this.chtSaleByRegister.Legend.Visible = true;
            this.chtSaleByRegister.Location = new System.Drawing.Point(0, 0);
            this.chtSaleByRegister.Name = "chtSaleByRegister";
            pieChartAppearance2.ColumnIndex = 0;
            pieChartAppearance2.RadiusFactor = 80;
            this.chtSaleByRegister.PieChart = pieChartAppearance2;
            this.chtSaleByRegister.Size = new System.Drawing.Size(1034, 479);
            this.chtSaleByRegister.TabIndex = 12;
            this.chtSaleByRegister.TitleBottom.Visible = false;
            this.chtSaleByRegister.TitleLeft.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold);
            this.chtSaleByRegister.TitleLeft.HorizontalAlign = System.Drawing.StringAlignment.Center;
            this.chtSaleByRegister.TitleLeft.Text = "AMOUNT";
            this.chtSaleByRegister.TitleTop.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold);
            this.chtSaleByRegister.TitleTop.HorizontalAlign = System.Drawing.StringAlignment.Center;
            this.chtSaleByRegister.TitleTop.Text = "SALE BY REGISTER";
            this.chtSaleByRegister.Tooltips.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F);
            view3DAppearance1.XRotation = -124F;
            view3DAppearance1.YRotation = -180F;
            this.chtSaleByRegister.Transform3D = view3DAppearance1;
            // 
            // ultraTabPageControl6
            // 
            this.ultraTabPageControl6.Controls.Add(this.groupBox1);
            this.ultraTabPageControl6.Controls.Add(this.chtTopDept);
            this.ultraTabPageControl6.Location = new System.Drawing.Point(-10000, -10000);
            this.ultraTabPageControl6.Name = "ultraTabPageControl6";
            this.ultraTabPageControl6.Size = new System.Drawing.Size(1034, 479);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.ultraLabel2);
            this.groupBox1.Controls.Add(this.numTopDept);
            this.groupBox1.Location = new System.Drawing.Point(8, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(774, 55);
            this.groupBox1.TabIndex = 18;
            this.groupBox1.TabStop = false;
            // 
            // ultraLabel2
            // 
            appearance23.ForeColor = System.Drawing.Color.Black;
            this.ultraLabel2.Appearance = appearance23;
            this.ultraLabel2.AutoSize = true;
            this.ultraLabel2.BackColorInternal = System.Drawing.Color.Transparent;
            this.ultraLabel2.Location = new System.Drawing.Point(16, 24);
            this.ultraLabel2.Name = "ultraLabel2";
            this.ultraLabel2.Size = new System.Drawing.Size(159, 18);
            this.ultraLabel2.TabIndex = 16;
            this.ultraLabel2.Text = "Top # of Departments";
            this.ultraLabel2.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // numTopDept
            // 
            this.numTopDept.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            this.numTopDept.DisplayStyle = Infragistics.Win.EmbeddableElementDisplayStyle.Office2003;
            this.numTopDept.Location = new System.Drawing.Point(176, 21);
            this.numTopDept.MaxValue = 99;
            this.numTopDept.MinValue = 5;
            this.numTopDept.Name = "numTopDept";
            this.numTopDept.NullText = "5";
            this.numTopDept.Size = new System.Drawing.Size(123, 25);
            this.numTopDept.TabIndex = 8;
            // 
            // chtTopDept
            // 
            this.chtTopDept.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.chtTopDept.Axis.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(248)))), ((int)(((byte)(220)))));
            paintElement6.ElementType = Infragistics.UltraChart.Shared.Styles.PaintElementType.None;
            paintElement6.Fill = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(248)))), ((int)(((byte)(220)))));
            this.chtTopDept.Axis.PE = paintElement6;
            this.chtTopDept.Axis.X.Labels.HorizontalAlign = System.Drawing.StringAlignment.Near;
            this.chtTopDept.Axis.X.Labels.ItemFormatString = "<ITEM_LABEL>";
            this.chtTopDept.Axis.X.Labels.Orientation = Infragistics.UltraChart.Shared.Styles.TextOrientation.VerticalLeftFacing;
            this.chtTopDept.Axis.X.Labels.SeriesLabels.HorizontalAlign = System.Drawing.StringAlignment.Near;
            this.chtTopDept.Axis.X.Labels.SeriesLabels.Orientation = Infragistics.UltraChart.Shared.Styles.TextOrientation.Horizontal;
            this.chtTopDept.Axis.X.Labels.SeriesLabels.VerticalAlign = System.Drawing.StringAlignment.Center;
            this.chtTopDept.Axis.X.Labels.VerticalAlign = System.Drawing.StringAlignment.Center;
            this.chtTopDept.Axis.X.MajorGridLines.AlphaLevel = ((byte)(255));
            this.chtTopDept.Axis.X.MajorGridLines.Color = System.Drawing.Color.Gainsboro;
            this.chtTopDept.Axis.X.MajorGridLines.DrawStyle = Infragistics.UltraChart.Shared.Styles.LineDrawStyle.Dot;
            this.chtTopDept.Axis.X.MajorGridLines.Visible = true;
            this.chtTopDept.Axis.X.MinorGridLines.AlphaLevel = ((byte)(255));
            this.chtTopDept.Axis.X.MinorGridLines.Color = System.Drawing.Color.LightGray;
            this.chtTopDept.Axis.X.MinorGridLines.DrawStyle = Infragistics.UltraChart.Shared.Styles.LineDrawStyle.Dot;
            this.chtTopDept.Axis.X.MinorGridLines.Visible = false;
            this.chtTopDept.Axis.X.Visible = true;
            this.chtTopDept.Axis.X2.Labels.HorizontalAlign = System.Drawing.StringAlignment.Near;
            this.chtTopDept.Axis.X2.Labels.ItemFormatString = "<ITEM_LABEL>";
            this.chtTopDept.Axis.X2.Labels.Orientation = Infragistics.UltraChart.Shared.Styles.TextOrientation.VerticalLeftFacing;
            this.chtTopDept.Axis.X2.Labels.SeriesLabels.HorizontalAlign = System.Drawing.StringAlignment.Near;
            this.chtTopDept.Axis.X2.Labels.SeriesLabels.Orientation = Infragistics.UltraChart.Shared.Styles.TextOrientation.Horizontal;
            this.chtTopDept.Axis.X2.Labels.SeriesLabels.VerticalAlign = System.Drawing.StringAlignment.Center;
            this.chtTopDept.Axis.X2.Labels.VerticalAlign = System.Drawing.StringAlignment.Center;
            this.chtTopDept.Axis.X2.MajorGridLines.AlphaLevel = ((byte)(255));
            this.chtTopDept.Axis.X2.MajorGridLines.Color = System.Drawing.Color.Gainsboro;
            this.chtTopDept.Axis.X2.MajorGridLines.DrawStyle = Infragistics.UltraChart.Shared.Styles.LineDrawStyle.Dot;
            this.chtTopDept.Axis.X2.MajorGridLines.Visible = true;
            this.chtTopDept.Axis.X2.MinorGridLines.AlphaLevel = ((byte)(255));
            this.chtTopDept.Axis.X2.MinorGridLines.Color = System.Drawing.Color.LightGray;
            this.chtTopDept.Axis.X2.MinorGridLines.DrawStyle = Infragistics.UltraChart.Shared.Styles.LineDrawStyle.Dot;
            this.chtTopDept.Axis.X2.MinorGridLines.Visible = false;
            this.chtTopDept.Axis.X2.Visible = false;
            this.chtTopDept.Axis.Y.Labels.HorizontalAlign = System.Drawing.StringAlignment.Far;
			this.chtTopDept.Axis.Y.Labels.ItemFormatString = "<DATA_VALUE:" + Configuration.CInfo.CurrencySymbol.ToString() + " 00.00>";
            this.chtTopDept.Axis.Y.Labels.Orientation = Infragistics.UltraChart.Shared.Styles.TextOrientation.Horizontal;
            this.chtTopDept.Axis.Y.Labels.SeriesLabels.FormatString = "";
            this.chtTopDept.Axis.Y.Labels.SeriesLabels.HorizontalAlign = System.Drawing.StringAlignment.Far;
            this.chtTopDept.Axis.Y.Labels.SeriesLabels.Orientation = Infragistics.UltraChart.Shared.Styles.TextOrientation.VerticalLeftFacing;
            this.chtTopDept.Axis.Y.Labels.SeriesLabels.VerticalAlign = System.Drawing.StringAlignment.Center;
            this.chtTopDept.Axis.Y.Labels.VerticalAlign = System.Drawing.StringAlignment.Center;
            this.chtTopDept.Axis.Y.MajorGridLines.AlphaLevel = ((byte)(255));
            this.chtTopDept.Axis.Y.MajorGridLines.Color = System.Drawing.Color.Gainsboro;
            this.chtTopDept.Axis.Y.MajorGridLines.DrawStyle = Infragistics.UltraChart.Shared.Styles.LineDrawStyle.Dot;
            this.chtTopDept.Axis.Y.MajorGridLines.Visible = true;
            this.chtTopDept.Axis.Y.MinorGridLines.AlphaLevel = ((byte)(255));
            this.chtTopDept.Axis.Y.MinorGridLines.Color = System.Drawing.Color.LightGray;
            this.chtTopDept.Axis.Y.MinorGridLines.DrawStyle = Infragistics.UltraChart.Shared.Styles.LineDrawStyle.Dot;
            this.chtTopDept.Axis.Y.MinorGridLines.Visible = false;
            this.chtTopDept.Axis.Y.Visible = true;
            this.chtTopDept.Axis.Y2.Labels.HorizontalAlign = System.Drawing.StringAlignment.Near;
            this.chtTopDept.Axis.Y2.Labels.ItemFormatString = "<DATA_VALUE:00.00>";
            this.chtTopDept.Axis.Y2.Labels.Orientation = Infragistics.UltraChart.Shared.Styles.TextOrientation.Horizontal;
            this.chtTopDept.Axis.Y2.Labels.SeriesLabels.FormatString = "";
            this.chtTopDept.Axis.Y2.Labels.SeriesLabels.HorizontalAlign = System.Drawing.StringAlignment.Near;
            this.chtTopDept.Axis.Y2.Labels.SeriesLabels.Orientation = Infragistics.UltraChart.Shared.Styles.TextOrientation.VerticalLeftFacing;
            this.chtTopDept.Axis.Y2.Labels.SeriesLabels.VerticalAlign = System.Drawing.StringAlignment.Center;
            this.chtTopDept.Axis.Y2.Labels.VerticalAlign = System.Drawing.StringAlignment.Center;
            this.chtTopDept.Axis.Y2.MajorGridLines.AlphaLevel = ((byte)(255));
            this.chtTopDept.Axis.Y2.MajorGridLines.Color = System.Drawing.Color.Gainsboro;
            this.chtTopDept.Axis.Y2.MajorGridLines.DrawStyle = Infragistics.UltraChart.Shared.Styles.LineDrawStyle.Dot;
            this.chtTopDept.Axis.Y2.MajorGridLines.Visible = true;
            this.chtTopDept.Axis.Y2.MinorGridLines.AlphaLevel = ((byte)(255));
            this.chtTopDept.Axis.Y2.MinorGridLines.Color = System.Drawing.Color.LightGray;
            this.chtTopDept.Axis.Y2.MinorGridLines.DrawStyle = Infragistics.UltraChart.Shared.Styles.LineDrawStyle.Dot;
            this.chtTopDept.Axis.Y2.MinorGridLines.Visible = false;
            this.chtTopDept.Axis.Y2.Visible = false;
            this.chtTopDept.Axis.Z.Labels.HorizontalAlign = System.Drawing.StringAlignment.Near;
            this.chtTopDept.Axis.Z.Labels.ItemFormatString = "<DATA_VALUE:00.00>";
            this.chtTopDept.Axis.Z.Labels.Orientation = Infragistics.UltraChart.Shared.Styles.TextOrientation.Horizontal;
            this.chtTopDept.Axis.Z.Labels.SeriesLabels.HorizontalAlign = System.Drawing.StringAlignment.Near;
            this.chtTopDept.Axis.Z.Labels.SeriesLabels.Orientation = Infragistics.UltraChart.Shared.Styles.TextOrientation.Horizontal;
            this.chtTopDept.Axis.Z.Labels.SeriesLabels.VerticalAlign = System.Drawing.StringAlignment.Center;
            this.chtTopDept.Axis.Z.Labels.VerticalAlign = System.Drawing.StringAlignment.Center;
            this.chtTopDept.Axis.Z.MajorGridLines.AlphaLevel = ((byte)(255));
            this.chtTopDept.Axis.Z.MajorGridLines.Color = System.Drawing.Color.Gainsboro;
            this.chtTopDept.Axis.Z.MajorGridLines.DrawStyle = Infragistics.UltraChart.Shared.Styles.LineDrawStyle.Dot;
            this.chtTopDept.Axis.Z.MajorGridLines.Visible = true;
            this.chtTopDept.Axis.Z.MinorGridLines.AlphaLevel = ((byte)(255));
            this.chtTopDept.Axis.Z.MinorGridLines.Color = System.Drawing.Color.LightGray;
            this.chtTopDept.Axis.Z.MinorGridLines.DrawStyle = Infragistics.UltraChart.Shared.Styles.LineDrawStyle.Dot;
            this.chtTopDept.Axis.Z.MinorGridLines.Visible = false;
            this.chtTopDept.Axis.Z.Visible = false;
            this.chtTopDept.Axis.Z2.Labels.HorizontalAlign = System.Drawing.StringAlignment.Near;
            this.chtTopDept.Axis.Z2.Labels.ItemFormatString = "";
            this.chtTopDept.Axis.Z2.Labels.Orientation = Infragistics.UltraChart.Shared.Styles.TextOrientation.Horizontal;
            this.chtTopDept.Axis.Z2.Labels.SeriesLabels.HorizontalAlign = System.Drawing.StringAlignment.Near;
            this.chtTopDept.Axis.Z2.Labels.SeriesLabels.Orientation = Infragistics.UltraChart.Shared.Styles.TextOrientation.Horizontal;
            this.chtTopDept.Axis.Z2.Labels.SeriesLabels.VerticalAlign = System.Drawing.StringAlignment.Center;
            this.chtTopDept.Axis.Z2.Labels.VerticalAlign = System.Drawing.StringAlignment.Center;
            this.chtTopDept.Axis.Z2.MajorGridLines.AlphaLevel = ((byte)(255));
            this.chtTopDept.Axis.Z2.MajorGridLines.Color = System.Drawing.Color.Gainsboro;
            this.chtTopDept.Axis.Z2.MajorGridLines.DrawStyle = Infragistics.UltraChart.Shared.Styles.LineDrawStyle.Dot;
            this.chtTopDept.Axis.Z2.MajorGridLines.Visible = true;
            this.chtTopDept.Axis.Z2.MinorGridLines.AlphaLevel = ((byte)(255));
            this.chtTopDept.Axis.Z2.MinorGridLines.Color = System.Drawing.Color.LightGray;
            this.chtTopDept.Axis.Z2.MinorGridLines.DrawStyle = Infragistics.UltraChart.Shared.Styles.LineDrawStyle.Dot;
            this.chtTopDept.Axis.Z2.MinorGridLines.Visible = false;
            this.chtTopDept.Axis.Z2.Visible = false;
            this.chtTopDept.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("chtTopDept.BackgroundImage")));
            this.chtTopDept.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.chtTopDept.Border.Color = System.Drawing.Color.Transparent;
            this.chtTopDept.ColorModel.AlphaLevel = ((byte)(150));
            columnChartAppearance2.ColumnSpacing = 1;
            this.chtTopDept.ColumnChart = columnChartAppearance2;
            this.chtTopDept.Data.SwapRowsAndColumns = true;
            this.chtTopDept.Data.ZeroAligned = true;
            this.chtTopDept.Effects.Effects.Add(threeDEffect5);
            this.chtTopDept.Effects.Effects.Add(gradientEffect6);
            this.chtTopDept.ForeColor = System.Drawing.SystemColors.ControlText;
            this.chtTopDept.Location = new System.Drawing.Point(8, 64);
            this.chtTopDept.Name = "chtTopDept";
            this.chtTopDept.Size = new System.Drawing.Size(774, 388);
            this.chtTopDept.TabIndex = 17;
            this.chtTopDept.TitleBottom.Visible = false;
            this.chtTopDept.TitleLeft.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold);
            this.chtTopDept.TitleLeft.HorizontalAlign = System.Drawing.StringAlignment.Center;
            this.chtTopDept.TitleLeft.Text = "AMOUNT";
            this.chtTopDept.TitleLeft.Visible = true;
            this.chtTopDept.TitleTop.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold);
            this.chtTopDept.TitleTop.FontColor = System.Drawing.Color.Blue;
            this.chtTopDept.TitleTop.HorizontalAlign = System.Drawing.StringAlignment.Center;
            this.chtTopDept.TitleTop.Text = "TOP PERFORMING DEPARTMENTS";
            this.chtTopDept.Tooltips.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F);
            // 
            // ultraTabPageControl7
            // 
            this.ultraTabPageControl7.Controls.Add(this.chtTopItems);
            this.ultraTabPageControl7.Controls.Add(this.groupBox2);
            this.ultraTabPageControl7.Location = new System.Drawing.Point(-10000, -10000);
            this.ultraTabPageControl7.Name = "ultraTabPageControl7";
            this.ultraTabPageControl7.Size = new System.Drawing.Size(1034, 479);
            // 
            // chtTopItems
            // 
            this.chtTopItems.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.chtTopItems.Axis.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(248)))), ((int)(((byte)(220)))));
            paintElement7.ElementType = Infragistics.UltraChart.Shared.Styles.PaintElementType.None;
            paintElement7.Fill = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(248)))), ((int)(((byte)(220)))));
            this.chtTopItems.Axis.PE = paintElement7;
            this.chtTopItems.Axis.X.Labels.Font = new System.Drawing.Font("Arial", 6.5F);
            this.chtTopItems.Axis.X.Labels.HorizontalAlign = System.Drawing.StringAlignment.Near;
            this.chtTopItems.Axis.X.Labels.ItemFormatString = "<ITEM_LABEL>";
            this.chtTopItems.Axis.X.Labels.Orientation = Infragistics.UltraChart.Shared.Styles.TextOrientation.VerticalLeftFacing;
            this.chtTopItems.Axis.X.Labels.SeriesLabels.HorizontalAlign = System.Drawing.StringAlignment.Near;
            this.chtTopItems.Axis.X.Labels.SeriesLabels.Orientation = Infragistics.UltraChart.Shared.Styles.TextOrientation.Horizontal;
            this.chtTopItems.Axis.X.Labels.SeriesLabels.VerticalAlign = System.Drawing.StringAlignment.Center;
            this.chtTopItems.Axis.X.Labels.VerticalAlign = System.Drawing.StringAlignment.Center;
            this.chtTopItems.Axis.X.MajorGridLines.AlphaLevel = ((byte)(255));
            this.chtTopItems.Axis.X.MajorGridLines.Color = System.Drawing.Color.Gainsboro;
            this.chtTopItems.Axis.X.MajorGridLines.DrawStyle = Infragistics.UltraChart.Shared.Styles.LineDrawStyle.Dot;
            this.chtTopItems.Axis.X.MajorGridLines.Visible = true;
            this.chtTopItems.Axis.X.MinorGridLines.AlphaLevel = ((byte)(255));
            this.chtTopItems.Axis.X.MinorGridLines.Color = System.Drawing.Color.LightGray;
            this.chtTopItems.Axis.X.MinorGridLines.DrawStyle = Infragistics.UltraChart.Shared.Styles.LineDrawStyle.Dot;
            this.chtTopItems.Axis.X.MinorGridLines.Visible = false;
            this.chtTopItems.Axis.X.Visible = true;
            this.chtTopItems.Axis.X2.Labels.HorizontalAlign = System.Drawing.StringAlignment.Near;
            this.chtTopItems.Axis.X2.Labels.ItemFormatString = "<ITEM_LABEL>";
            this.chtTopItems.Axis.X2.Labels.Orientation = Infragistics.UltraChart.Shared.Styles.TextOrientation.VerticalLeftFacing;
            this.chtTopItems.Axis.X2.Labels.SeriesLabels.HorizontalAlign = System.Drawing.StringAlignment.Near;
            this.chtTopItems.Axis.X2.Labels.SeriesLabels.Orientation = Infragistics.UltraChart.Shared.Styles.TextOrientation.Horizontal;
            this.chtTopItems.Axis.X2.Labels.SeriesLabels.VerticalAlign = System.Drawing.StringAlignment.Center;
            this.chtTopItems.Axis.X2.Labels.VerticalAlign = System.Drawing.StringAlignment.Center;
            this.chtTopItems.Axis.X2.MajorGridLines.AlphaLevel = ((byte)(255));
            this.chtTopItems.Axis.X2.MajorGridLines.Color = System.Drawing.Color.Gainsboro;
            this.chtTopItems.Axis.X2.MajorGridLines.DrawStyle = Infragistics.UltraChart.Shared.Styles.LineDrawStyle.Dot;
            this.chtTopItems.Axis.X2.MajorGridLines.Visible = true;
            this.chtTopItems.Axis.X2.MinorGridLines.AlphaLevel = ((byte)(255));
            this.chtTopItems.Axis.X2.MinorGridLines.Color = System.Drawing.Color.LightGray;
            this.chtTopItems.Axis.X2.MinorGridLines.DrawStyle = Infragistics.UltraChart.Shared.Styles.LineDrawStyle.Dot;
            this.chtTopItems.Axis.X2.MinorGridLines.Visible = false;
            this.chtTopItems.Axis.X2.Visible = false;
            this.chtTopItems.Axis.Y.Labels.HorizontalAlign = System.Drawing.StringAlignment.Far;
			this.chtTopItems.Axis.Y.Labels.ItemFormatString = "<DATA_VALUE:" + Configuration.CInfo.CurrencySymbol.ToString() + " 00.00>";
            this.chtTopItems.Axis.Y.Labels.Orientation = Infragistics.UltraChart.Shared.Styles.TextOrientation.Horizontal;
            this.chtTopItems.Axis.Y.Labels.SeriesLabels.FormatString = "";
            this.chtTopItems.Axis.Y.Labels.SeriesLabels.HorizontalAlign = System.Drawing.StringAlignment.Far;
            this.chtTopItems.Axis.Y.Labels.SeriesLabels.Orientation = Infragistics.UltraChart.Shared.Styles.TextOrientation.VerticalLeftFacing;
            this.chtTopItems.Axis.Y.Labels.SeriesLabels.VerticalAlign = System.Drawing.StringAlignment.Center;
            this.chtTopItems.Axis.Y.Labels.VerticalAlign = System.Drawing.StringAlignment.Center;
            this.chtTopItems.Axis.Y.MajorGridLines.AlphaLevel = ((byte)(255));
            this.chtTopItems.Axis.Y.MajorGridLines.Color = System.Drawing.Color.Gainsboro;
            this.chtTopItems.Axis.Y.MajorGridLines.DrawStyle = Infragistics.UltraChart.Shared.Styles.LineDrawStyle.Dot;
            this.chtTopItems.Axis.Y.MajorGridLines.Visible = true;
            this.chtTopItems.Axis.Y.MinorGridLines.AlphaLevel = ((byte)(255));
            this.chtTopItems.Axis.Y.MinorGridLines.Color = System.Drawing.Color.LightGray;
            this.chtTopItems.Axis.Y.MinorGridLines.DrawStyle = Infragistics.UltraChart.Shared.Styles.LineDrawStyle.Dot;
            this.chtTopItems.Axis.Y.MinorGridLines.Visible = false;
            this.chtTopItems.Axis.Y.Visible = true;
            this.chtTopItems.Axis.Y2.Labels.HorizontalAlign = System.Drawing.StringAlignment.Near;
            this.chtTopItems.Axis.Y2.Labels.ItemFormatString = "<DATA_VALUE:00.00>";
            this.chtTopItems.Axis.Y2.Labels.Orientation = Infragistics.UltraChart.Shared.Styles.TextOrientation.Horizontal;
            this.chtTopItems.Axis.Y2.Labels.SeriesLabels.FormatString = "";
            this.chtTopItems.Axis.Y2.Labels.SeriesLabels.HorizontalAlign = System.Drawing.StringAlignment.Near;
            this.chtTopItems.Axis.Y2.Labels.SeriesLabels.Orientation = Infragistics.UltraChart.Shared.Styles.TextOrientation.VerticalLeftFacing;
            this.chtTopItems.Axis.Y2.Labels.SeriesLabels.VerticalAlign = System.Drawing.StringAlignment.Center;
            this.chtTopItems.Axis.Y2.Labels.VerticalAlign = System.Drawing.StringAlignment.Center;
            this.chtTopItems.Axis.Y2.MajorGridLines.AlphaLevel = ((byte)(255));
            this.chtTopItems.Axis.Y2.MajorGridLines.Color = System.Drawing.Color.Gainsboro;
            this.chtTopItems.Axis.Y2.MajorGridLines.DrawStyle = Infragistics.UltraChart.Shared.Styles.LineDrawStyle.Dot;
            this.chtTopItems.Axis.Y2.MajorGridLines.Visible = true;
            this.chtTopItems.Axis.Y2.MinorGridLines.AlphaLevel = ((byte)(255));
            this.chtTopItems.Axis.Y2.MinorGridLines.Color = System.Drawing.Color.LightGray;
            this.chtTopItems.Axis.Y2.MinorGridLines.DrawStyle = Infragistics.UltraChart.Shared.Styles.LineDrawStyle.Dot;
            this.chtTopItems.Axis.Y2.MinorGridLines.Visible = false;
            this.chtTopItems.Axis.Y2.Visible = false;
            this.chtTopItems.Axis.Z.Labels.HorizontalAlign = System.Drawing.StringAlignment.Near;
            this.chtTopItems.Axis.Z.Labels.ItemFormatString = "<DATA_VALUE:00.00>";
            this.chtTopItems.Axis.Z.Labels.Orientation = Infragistics.UltraChart.Shared.Styles.TextOrientation.Horizontal;
            this.chtTopItems.Axis.Z.Labels.SeriesLabels.HorizontalAlign = System.Drawing.StringAlignment.Near;
            this.chtTopItems.Axis.Z.Labels.SeriesLabels.Orientation = Infragistics.UltraChart.Shared.Styles.TextOrientation.Horizontal;
            this.chtTopItems.Axis.Z.Labels.SeriesLabels.VerticalAlign = System.Drawing.StringAlignment.Center;
            this.chtTopItems.Axis.Z.Labels.VerticalAlign = System.Drawing.StringAlignment.Center;
            this.chtTopItems.Axis.Z.MajorGridLines.AlphaLevel = ((byte)(255));
            this.chtTopItems.Axis.Z.MajorGridLines.Color = System.Drawing.Color.Gainsboro;
            this.chtTopItems.Axis.Z.MajorGridLines.DrawStyle = Infragistics.UltraChart.Shared.Styles.LineDrawStyle.Dot;
            this.chtTopItems.Axis.Z.MajorGridLines.Visible = true;
            this.chtTopItems.Axis.Z.MinorGridLines.AlphaLevel = ((byte)(255));
            this.chtTopItems.Axis.Z.MinorGridLines.Color = System.Drawing.Color.LightGray;
            this.chtTopItems.Axis.Z.MinorGridLines.DrawStyle = Infragistics.UltraChart.Shared.Styles.LineDrawStyle.Dot;
            this.chtTopItems.Axis.Z.MinorGridLines.Visible = false;
            this.chtTopItems.Axis.Z.Visible = false;
            this.chtTopItems.Axis.Z2.Labels.HorizontalAlign = System.Drawing.StringAlignment.Near;
            this.chtTopItems.Axis.Z2.Labels.ItemFormatString = "";
            this.chtTopItems.Axis.Z2.Labels.Orientation = Infragistics.UltraChart.Shared.Styles.TextOrientation.Horizontal;
            this.chtTopItems.Axis.Z2.Labels.SeriesLabels.HorizontalAlign = System.Drawing.StringAlignment.Near;
            this.chtTopItems.Axis.Z2.Labels.SeriesLabels.Orientation = Infragistics.UltraChart.Shared.Styles.TextOrientation.Horizontal;
            this.chtTopItems.Axis.Z2.Labels.SeriesLabels.VerticalAlign = System.Drawing.StringAlignment.Center;
            this.chtTopItems.Axis.Z2.Labels.VerticalAlign = System.Drawing.StringAlignment.Center;
            this.chtTopItems.Axis.Z2.MajorGridLines.AlphaLevel = ((byte)(255));
            this.chtTopItems.Axis.Z2.MajorGridLines.Color = System.Drawing.Color.Gainsboro;
            this.chtTopItems.Axis.Z2.MajorGridLines.DrawStyle = Infragistics.UltraChart.Shared.Styles.LineDrawStyle.Dot;
            this.chtTopItems.Axis.Z2.MajorGridLines.Visible = true;
            this.chtTopItems.Axis.Z2.MinorGridLines.AlphaLevel = ((byte)(255));
            this.chtTopItems.Axis.Z2.MinorGridLines.Color = System.Drawing.Color.LightGray;
            this.chtTopItems.Axis.Z2.MinorGridLines.DrawStyle = Infragistics.UltraChart.Shared.Styles.LineDrawStyle.Dot;
            this.chtTopItems.Axis.Z2.MinorGridLines.Visible = false;
            this.chtTopItems.Axis.Z2.Visible = false;
            this.chtTopItems.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("chtTopItems.BackgroundImage")));
            this.chtTopItems.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.chtTopItems.ColorModel.AlphaLevel = ((byte)(150));
            columnChartAppearance3.ColumnSpacing = 1;
            this.chtTopItems.ColumnChart = columnChartAppearance3;
            this.chtTopItems.Data.SwapRowsAndColumns = true;
            this.chtTopItems.Data.ZeroAligned = true;
            this.chtTopItems.Effects.Effects.Add(threeDEffect6);
            this.chtTopItems.Effects.Effects.Add(gradientEffect7);
            this.chtTopItems.ForeColor = System.Drawing.SystemColors.ControlText;
            this.chtTopItems.Location = new System.Drawing.Point(8, 64);
            this.chtTopItems.Name = "chtTopItems";
            this.chtTopItems.Size = new System.Drawing.Size(774, 388);
            this.chtTopItems.TabIndex = 20;
            this.chtTopItems.TitleBottom.Visible = false;
            this.chtTopItems.TitleLeft.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold);
            this.chtTopItems.TitleLeft.HorizontalAlign = System.Drawing.StringAlignment.Center;
            this.chtTopItems.TitleLeft.Text = "AMOUNT";
            this.chtTopItems.TitleLeft.Visible = true;
            this.chtTopItems.TitleTop.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold);
            this.chtTopItems.TitleTop.HorizontalAlign = System.Drawing.StringAlignment.Center;
            this.chtTopItems.TitleTop.Text = "TOP PERFORMING ITEMS";
            this.chtTopItems.Tooltips.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F);
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.numTopItem);
            this.groupBox2.Controls.Add(this.ultraLabel1);
            this.groupBox2.Location = new System.Drawing.Point(7, 0);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(774, 55);
            this.groupBox2.TabIndex = 19;
            this.groupBox2.TabStop = false;
            // 
            // numTopItem
            // 
            this.numTopItem.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            this.numTopItem.DisplayStyle = Infragistics.Win.EmbeddableElementDisplayStyle.Office2003;
            this.numTopItem.Location = new System.Drawing.Point(131, 20);
            this.numTopItem.MaxValue = 999;
            this.numTopItem.MinValue = 5;
            this.numTopItem.Name = "numTopItem";
            this.numTopItem.NullText = "5";
            this.numTopItem.Size = new System.Drawing.Size(123, 25);
            this.numTopItem.TabIndex = 14;
            // 
            // ultraLabel1
            // 
            appearance24.ForeColor = System.Drawing.Color.Black;
            this.ultraLabel1.Appearance = appearance24;
            this.ultraLabel1.AutoSize = true;
            this.ultraLabel1.BackColorInternal = System.Drawing.Color.Transparent;
            this.ultraLabel1.Location = new System.Drawing.Point(11, 23);
            this.ultraLabel1.Name = "ultraLabel1";
            this.ultraLabel1.Size = new System.Drawing.Size(109, 18);
            this.ultraLabel1.TabIndex = 15;
            this.ultraLabel1.Text = "Top # of Items";
            this.ultraLabel1.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // ultraTabPageControl8
            // 
            this.ultraTabPageControl8.Controls.Add(this.groupBox4);
            this.ultraTabPageControl8.Controls.Add(this.grpTransTypeAll);
            this.ultraTabPageControl8.Location = new System.Drawing.Point(-10000, -10000);
            this.ultraTabPageControl8.Name = "ultraTabPageControl8";
            this.ultraTabPageControl8.Size = new System.Drawing.Size(1034, 479);
            // 
            // groupBox4
            // 
            this.groupBox4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox4.Controls.Add(this.numTRTransNo);
            this.groupBox4.Controls.Add(this.ultraLabel7);
            this.groupBox4.Controls.Add(this.numTRTransAmount);
            this.groupBox4.Controls.Add(this.ultraLabel8);
            this.groupBox4.Controls.Add(this.numTSTransNo);
            this.groupBox4.Controls.Add(this.ultraLabel9);
            this.groupBox4.Controls.Add(this.numTSTransAmount);
            this.groupBox4.Controls.Add(this.ultraLabel10);
            this.groupBox4.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox4.Location = new System.Drawing.Point(13, 168);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(1017, 136);
            this.groupBox4.TabIndex = 21;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "For Today";
            // 
            // numTRTransNo
            // 
            appearance25.BackColor = System.Drawing.Color.White;
            appearance25.BackColorDisabled = System.Drawing.Color.White;
            appearance25.ForeColor = System.Drawing.Color.Black;
            appearance25.ForeColorDisabled = System.Drawing.Color.Black;
            this.numTRTransNo.Appearance = appearance25;
            this.numTRTransNo.AutoSize = false;
            this.numTRTransNo.BackColor = System.Drawing.Color.White;
            this.numTRTransNo.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            this.numTRTransNo.DisplayStyle = Infragistics.Win.EmbeddableElementDisplayStyle.Office2003;
            this.numTRTransNo.Enabled = false;
            this.numTRTransNo.Location = new System.Drawing.Point(184, 89);
            this.numTRTransNo.MaskClipMode = Infragistics.Win.UltraWinMaskedEdit.MaskMode.Raw;
            this.numTRTransNo.MaskDisplayMode = Infragistics.Win.UltraWinMaskedEdit.MaskMode.Raw;
            this.numTRTransNo.Name = "numTRTransNo";
            this.numTRTransNo.NullText = "0";
            this.numTRTransNo.PromptChar = ' ';
            this.numTRTransNo.ReadOnly = true;
            this.numTRTransNo.Size = new System.Drawing.Size(123, 25);
            this.numTRTransNo.TabIndex = 20;
            this.numTRTransNo.TabNavigation = Infragistics.Win.UltraWinMaskedEdit.MaskedEditTabNavigation.NextControl;
            this.numTRTransNo.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            // 
            // ultraLabel7
            // 
            appearance26.ForeColor = System.Drawing.Color.Black;
            this.ultraLabel7.Appearance = appearance26;
            this.ultraLabel7.AutoSize = true;
            this.ultraLabel7.BackColorInternal = System.Drawing.Color.Transparent;
            this.ultraLabel7.Location = new System.Drawing.Point(56, 92);
            this.ultraLabel7.Name = "ultraLabel7";
            this.ultraLabel7.Size = new System.Drawing.Size(143, 18);
            this.ultraLabel7.TabIndex = 21;
            this.ultraLabel7.Text = "# of Return Trans.";
            this.ultraLabel7.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // numTRTransAmount
            // 
            appearance27.BackColor = System.Drawing.Color.White;
            appearance27.BackColorDisabled = System.Drawing.Color.White;
            appearance27.ForeColor = System.Drawing.Color.Black;
            appearance27.ForeColorDisabled = System.Drawing.Color.Black;
            this.numTRTransAmount.Appearance = appearance27;
            this.numTRTransAmount.AutoSize = false;
            this.numTRTransAmount.BackColor = System.Drawing.Color.White;
            this.numTRTransAmount.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            this.numTRTransAmount.DisplayStyle = Infragistics.Win.EmbeddableElementDisplayStyle.Office2003;
            this.numTRTransAmount.Enabled = false;
            this.numTRTransAmount.FormatString = Configuration.CInfo.CurrencySymbol.ToString()+"########0.00";
            this.numTRTransAmount.Location = new System.Drawing.Point(479, 89);
            this.numTRTransAmount.Name = "numTRTransAmount";
            this.numTRTransAmount.NullText = "0.00";
            this.numTRTransAmount.NumericType = Infragistics.Win.UltraWinEditors.NumericType.Double;
            this.numTRTransAmount.PromptChar = ' ';
            this.numTRTransAmount.ReadOnly = true;
            this.numTRTransAmount.Size = new System.Drawing.Size(123, 25);
            this.numTRTransAmount.TabIndex = 18;
            this.numTRTransAmount.TabNavigation = Infragistics.Win.UltraWinMaskedEdit.MaskedEditTabNavigation.NextControl;
            this.numTRTransAmount.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            // 
            // ultraLabel8
            // 
            appearance28.ForeColor = System.Drawing.Color.Black;
            this.ultraLabel8.Appearance = appearance28;
            this.ultraLabel8.AutoSize = true;
            this.ultraLabel8.BackColorInternal = System.Drawing.Color.Transparent;
            this.ultraLabel8.Location = new System.Drawing.Point(336, 92);
            this.ultraLabel8.Name = "ultraLabel8";
            this.ultraLabel8.Size = new System.Drawing.Size(162, 18);
            this.ultraLabel8.TabIndex = 19;
            this.ultraLabel8.Text = "Total Return Amount";
            this.ultraLabel8.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // numTSTransNo
            // 
            appearance29.BackColor = System.Drawing.Color.White;
            appearance29.BackColorDisabled = System.Drawing.Color.White;
            appearance29.ForeColor = System.Drawing.Color.Black;
            appearance29.ForeColorDisabled = System.Drawing.Color.Black;
            this.numTSTransNo.Appearance = appearance29;
            this.numTSTransNo.AutoSize = false;
            this.numTSTransNo.BackColor = System.Drawing.Color.White;
            this.numTSTransNo.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            this.numTSTransNo.DisplayStyle = Infragistics.Win.EmbeddableElementDisplayStyle.Office2003;
            this.numTSTransNo.Enabled = false;
            this.numTSTransNo.Location = new System.Drawing.Point(184, 56);
            this.numTSTransNo.MaskClipMode = Infragistics.Win.UltraWinMaskedEdit.MaskMode.Raw;
            this.numTSTransNo.MaskDisplayMode = Infragistics.Win.UltraWinMaskedEdit.MaskMode.Raw;
            this.numTSTransNo.Name = "numTSTransNo";
            this.numTSTransNo.NullText = "0";
            this.numTSTransNo.PromptChar = ' ';
            this.numTSTransNo.ReadOnly = true;
            this.numTSTransNo.Size = new System.Drawing.Size(123, 25);
            this.numTSTransNo.TabIndex = 16;
            this.numTSTransNo.TabNavigation = Infragistics.Win.UltraWinMaskedEdit.MaskedEditTabNavigation.NextControl;
            this.numTSTransNo.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            // 
            // ultraLabel9
            // 
            appearance30.ForeColor = System.Drawing.Color.Black;
            this.ultraLabel9.Appearance = appearance30;
            this.ultraLabel9.AutoSize = true;
            this.ultraLabel9.BackColorInternal = System.Drawing.Color.Transparent;
            this.ultraLabel9.Location = new System.Drawing.Point(56, 59);
            this.ultraLabel9.Name = "ultraLabel9";
            this.ultraLabel9.Size = new System.Drawing.Size(124, 18);
            this.ultraLabel9.TabIndex = 17;
            this.ultraLabel9.Text = "# of Sale Trans.";
            this.ultraLabel9.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // numTSTransAmount
            // 
            appearance31.BackColor = System.Drawing.Color.White;
            appearance31.BackColorDisabled = System.Drawing.Color.White;
            appearance31.ForeColor = System.Drawing.Color.Black;
            appearance31.ForeColorDisabled = System.Drawing.Color.Black;
            this.numTSTransAmount.Appearance = appearance31;
            this.numTSTransAmount.AutoSize = false;
            this.numTSTransAmount.BackColor = System.Drawing.Color.White;
            this.numTSTransAmount.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            this.numTSTransAmount.DisplayStyle = Infragistics.Win.EmbeddableElementDisplayStyle.Office2003;
            this.numTSTransAmount.Enabled = false;
            this.numTSTransAmount.FormatString = Configuration.CInfo.CurrencySymbol.ToString()+"########0.00";
            this.numTSTransAmount.Location = new System.Drawing.Point(479, 56);
            this.numTSTransAmount.Name = "numTSTransAmount";
            this.numTSTransAmount.NullText = "0.00";
            this.numTSTransAmount.NumericType = Infragistics.Win.UltraWinEditors.NumericType.Double;
            this.numTSTransAmount.PromptChar = ' ';
            this.numTSTransAmount.ReadOnly = true;
            this.numTSTransAmount.Size = new System.Drawing.Size(123, 25);
            this.numTSTransAmount.TabIndex = 14;
            this.numTSTransAmount.TabNavigation = Infragistics.Win.UltraWinMaskedEdit.MaskedEditTabNavigation.NextControl;
            this.numTSTransAmount.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            // 
            // ultraLabel10
            // 
            appearance32.ForeColor = System.Drawing.Color.Black;
            this.ultraLabel10.Appearance = appearance32;
            this.ultraLabel10.AutoSize = true;
            this.ultraLabel10.BackColorInternal = System.Drawing.Color.Transparent;
            this.ultraLabel10.Location = new System.Drawing.Point(336, 59);
            this.ultraLabel10.Name = "ultraLabel10";
            this.ultraLabel10.Size = new System.Drawing.Size(143, 18);
            this.ultraLabel10.TabIndex = 15;
            this.ultraLabel10.Text = "Total Sale Amount";
            this.ultraLabel10.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // grpTransTypeAll
            // 
            this.grpTransTypeAll.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpTransTypeAll.Controls.Add(this.numAllRTransNo);
            this.grpTransTypeAll.Controls.Add(this.ultraLabel5);
            this.grpTransTypeAll.Controls.Add(this.numAllRTransAmount);
            this.grpTransTypeAll.Controls.Add(this.ultraLabel6);
            this.grpTransTypeAll.Controls.Add(this.numAllSTransNo);
            this.grpTransTypeAll.Controls.Add(this.ultraLabel4);
            this.grpTransTypeAll.Controls.Add(this.numAllSTransAmount);
            this.grpTransTypeAll.Controls.Add(this.ultraLabel3);
            this.grpTransTypeAll.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpTransTypeAll.Location = new System.Drawing.Point(13, 17);
            this.grpTransTypeAll.Name = "grpTransTypeAll";
            this.grpTransTypeAll.Size = new System.Drawing.Size(1017, 136);
            this.grpTransTypeAll.TabIndex = 20;
            this.grpTransTypeAll.TabStop = false;
            this.grpTransTypeAll.Text = "For Date";
            // 
            // numAllRTransNo
            // 
            appearance33.BackColor = System.Drawing.Color.White;
            appearance33.BackColorDisabled = System.Drawing.Color.White;
            appearance33.ForeColor = System.Drawing.Color.Black;
            appearance33.ForeColorDisabled = System.Drawing.Color.Black;
            this.numAllRTransNo.Appearance = appearance33;
            this.numAllRTransNo.AutoSize = false;
            this.numAllRTransNo.BackColor = System.Drawing.Color.White;
            this.numAllRTransNo.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            this.numAllRTransNo.DisplayStyle = Infragistics.Win.EmbeddableElementDisplayStyle.Office2003;
            this.numAllRTransNo.Enabled = false;
            this.numAllRTransNo.Location = new System.Drawing.Point(184, 89);
            this.numAllRTransNo.MaskClipMode = Infragistics.Win.UltraWinMaskedEdit.MaskMode.Raw;
            this.numAllRTransNo.MaskDisplayMode = Infragistics.Win.UltraWinMaskedEdit.MaskMode.Raw;
            this.numAllRTransNo.Name = "numAllRTransNo";
            this.numAllRTransNo.NullText = "0";
            this.numAllRTransNo.PromptChar = ' ';
            this.numAllRTransNo.ReadOnly = true;
            this.numAllRTransNo.Size = new System.Drawing.Size(123, 25);
            this.numAllRTransNo.TabIndex = 20;
            this.numAllRTransNo.TabNavigation = Infragistics.Win.UltraWinMaskedEdit.MaskedEditTabNavigation.NextControl;
            this.numAllRTransNo.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            // 
            // ultraLabel5
            // 
            appearance34.ForeColor = System.Drawing.Color.Black;
            this.ultraLabel5.Appearance = appearance34;
            this.ultraLabel5.AutoSize = true;
            this.ultraLabel5.BackColorInternal = System.Drawing.Color.Transparent;
            this.ultraLabel5.Location = new System.Drawing.Point(56, 92);
            this.ultraLabel5.Name = "ultraLabel5";
            this.ultraLabel5.Size = new System.Drawing.Size(143, 18);
            this.ultraLabel5.TabIndex = 21;
            this.ultraLabel5.Text = "# of Return Trans.";
            this.ultraLabel5.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // numAllRTransAmount
            // 
            appearance35.BackColor = System.Drawing.Color.White;
            appearance35.BackColorDisabled = System.Drawing.Color.White;
            appearance35.ForeColor = System.Drawing.Color.Black;
            appearance35.ForeColorDisabled = System.Drawing.Color.Black;
            this.numAllRTransAmount.Appearance = appearance35;
            this.numAllRTransAmount.AutoSize = false;
            this.numAllRTransAmount.BackColor = System.Drawing.Color.White;
            this.numAllRTransAmount.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            this.numAllRTransAmount.DisplayStyle = Infragistics.Win.EmbeddableElementDisplayStyle.Office2003;
            this.numAllRTransAmount.Enabled = false;
            this.numAllRTransAmount.FormatString = Configuration.CInfo.CurrencySymbol.ToString()+"########0.00";
            this.numAllRTransAmount.Location = new System.Drawing.Point(479, 89);
            this.numAllRTransAmount.MaskDisplayMode = Infragistics.Win.UltraWinMaskedEdit.MaskMode.IncludeLiterals;
            this.numAllRTransAmount.Name = "numAllRTransAmount";
            this.numAllRTransAmount.NullText = "0.00";
            this.numAllRTransAmount.NumericType = Infragistics.Win.UltraWinEditors.NumericType.Double;
            this.numAllRTransAmount.PromptChar = ' ';
            this.numAllRTransAmount.ReadOnly = true;
            this.numAllRTransAmount.Size = new System.Drawing.Size(123, 25);
            this.numAllRTransAmount.TabIndex = 18;
            this.numAllRTransAmount.TabNavigation = Infragistics.Win.UltraWinMaskedEdit.MaskedEditTabNavigation.NextControl;
            this.numAllRTransAmount.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            // 
            // ultraLabel6
            // 
            appearance36.ForeColor = System.Drawing.Color.Black;
            this.ultraLabel6.Appearance = appearance36;
            this.ultraLabel6.AutoSize = true;
            this.ultraLabel6.BackColorInternal = System.Drawing.Color.Transparent;
            this.ultraLabel6.Location = new System.Drawing.Point(336, 92);
            this.ultraLabel6.Name = "ultraLabel6";
            this.ultraLabel6.Size = new System.Drawing.Size(162, 18);
            this.ultraLabel6.TabIndex = 19;
            this.ultraLabel6.Text = "Total Return Amount";
            this.ultraLabel6.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // numAllSTransNo
            // 
            appearance37.BackColor = System.Drawing.Color.White;
            appearance37.BackColorDisabled = System.Drawing.Color.White;
            appearance37.ForeColor = System.Drawing.Color.Black;
            appearance37.ForeColorDisabled = System.Drawing.Color.Black;
            this.numAllSTransNo.Appearance = appearance37;
            this.numAllSTransNo.AutoSize = false;
            this.numAllSTransNo.BackColor = System.Drawing.Color.White;
            this.numAllSTransNo.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            this.numAllSTransNo.DisplayStyle = Infragistics.Win.EmbeddableElementDisplayStyle.Office2003;
            this.numAllSTransNo.Enabled = false;
            this.numAllSTransNo.Location = new System.Drawing.Point(184, 56);
            this.numAllSTransNo.MaskClipMode = Infragistics.Win.UltraWinMaskedEdit.MaskMode.Raw;
            this.numAllSTransNo.MaskDisplayMode = Infragistics.Win.UltraWinMaskedEdit.MaskMode.Raw;
            this.numAllSTransNo.Name = "numAllSTransNo";
            this.numAllSTransNo.NullText = "0";
            this.numAllSTransNo.PromptChar = ' ';
            this.numAllSTransNo.ReadOnly = true;
            this.numAllSTransNo.Size = new System.Drawing.Size(123, 25);
            this.numAllSTransNo.TabIndex = 16;
            this.numAllSTransNo.TabNavigation = Infragistics.Win.UltraWinMaskedEdit.MaskedEditTabNavigation.NextControl;
            this.numAllSTransNo.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            // 
            // ultraLabel4
            // 
            appearance38.ForeColor = System.Drawing.Color.Black;
            this.ultraLabel4.Appearance = appearance38;
            this.ultraLabel4.AutoSize = true;
            this.ultraLabel4.BackColorInternal = System.Drawing.Color.Transparent;
            this.ultraLabel4.Location = new System.Drawing.Point(56, 59);
            this.ultraLabel4.Name = "ultraLabel4";
            this.ultraLabel4.Size = new System.Drawing.Size(124, 18);
            this.ultraLabel4.TabIndex = 17;
            this.ultraLabel4.Text = "# of Sale Trans.";
            this.ultraLabel4.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // numAllSTransAmount
            // 
            appearance39.BackColor = System.Drawing.Color.White;
            appearance39.BackColorDisabled = System.Drawing.Color.White;
            appearance39.ForeColor = System.Drawing.Color.Black;
            appearance39.ForeColorDisabled = System.Drawing.Color.Black;
            this.numAllSTransAmount.Appearance = appearance39;
            this.numAllSTransAmount.AutoSize = false;
            this.numAllSTransAmount.BackColor = System.Drawing.Color.White;
            this.numAllSTransAmount.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            this.numAllSTransAmount.DisplayStyle = Infragistics.Win.EmbeddableElementDisplayStyle.Office2003;
            this.numAllSTransAmount.Enabled = false;
            this.numAllSTransAmount.FormatString = Configuration.CInfo.CurrencySymbol.ToString()+"########0.00";
            this.numAllSTransAmount.Location = new System.Drawing.Point(479, 56);
            this.numAllSTransAmount.Name = "numAllSTransAmount";
            this.numAllSTransAmount.NullText = "0.00";
            this.numAllSTransAmount.NumericType = Infragistics.Win.UltraWinEditors.NumericType.Double;
            this.numAllSTransAmount.PromptChar = ' ';
            this.numAllSTransAmount.ReadOnly = true;
            this.numAllSTransAmount.Size = new System.Drawing.Size(123, 25);
            this.numAllSTransAmount.TabIndex = 14;
            this.numAllSTransAmount.TabNavigation = Infragistics.Win.UltraWinMaskedEdit.MaskedEditTabNavigation.NextControl;
            this.numAllSTransAmount.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            // 
            // ultraLabel3
            // 
            appearance40.ForeColor = System.Drawing.Color.Black;
            this.ultraLabel3.Appearance = appearance40;
            this.ultraLabel3.AutoSize = true;
            this.ultraLabel3.BackColorInternal = System.Drawing.Color.Transparent;
            this.ultraLabel3.Location = new System.Drawing.Point(336, 59);
            this.ultraLabel3.Name = "ultraLabel3";
            this.ultraLabel3.Size = new System.Drawing.Size(143, 18);
            this.ultraLabel3.TabIndex = 15;
            this.ultraLabel3.Text = "Total Sale Amount";
            this.ultraLabel3.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // ultraTabPageControl9
            // 
            this.ultraTabPageControl9.Controls.Add(this.groupBox6);
            this.ultraTabPageControl9.Controls.Add(this.grpPayTypeAll);
            this.ultraTabPageControl9.Location = new System.Drawing.Point(-10000, -10000);
            this.ultraTabPageControl9.Name = "ultraTabPageControl9";
            this.ultraTabPageControl9.Size = new System.Drawing.Size(1034, 479);
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.grdPaymentToday);
            this.groupBox6.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox6.Location = new System.Drawing.Point(400, 19);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(365, 380);
            this.groupBox6.TabIndex = 1;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "For Today";
            // 
            // grdPaymentToday
            // 
            this.grdPaymentToday.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.None;
            this.grdPaymentToday.DisplayLayout.GroupByBox.Hidden = true;
            this.grdPaymentToday.DisplayLayout.MaxColScrollRegions = 1;
            this.grdPaymentToday.DisplayLayout.MaxRowScrollRegions = 1;
            this.grdPaymentToday.DisplayLayout.Override.AllowAddNew = Infragistics.Win.UltraWinGrid.AllowAddNew.No;
            this.grdPaymentToday.DisplayLayout.Override.AllowColMoving = Infragistics.Win.UltraWinGrid.AllowColMoving.NotAllowed;
            this.grdPaymentToday.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.False;
            this.grdPaymentToday.DisplayLayout.Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.False;
            this.grdPaymentToday.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            this.grdPaymentToday.DisplayLayout.Override.SelectTypeCell = Infragistics.Win.UltraWinGrid.SelectType.None;
            this.grdPaymentToday.DisplayLayout.Override.SelectTypeCol = Infragistics.Win.UltraWinGrid.SelectType.Single;
            this.grdPaymentToday.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.Single;
            this.grdPaymentToday.Location = new System.Drawing.Point(15, 24);
            this.grdPaymentToday.Name = "grdPaymentToday";
            this.grdPaymentToday.Size = new System.Drawing.Size(334, 341);
            this.grdPaymentToday.TabIndex = 1;
            this.grdPaymentToday.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            // 
            // grpPayTypeAll
            // 
            this.grpPayTypeAll.Controls.Add(this.grdPaymentAll);
            this.grpPayTypeAll.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpPayTypeAll.Location = new System.Drawing.Point(16, 19);
            this.grpPayTypeAll.Name = "grpPayTypeAll";
            this.grpPayTypeAll.Size = new System.Drawing.Size(365, 380);
            this.grpPayTypeAll.TabIndex = 0;
            this.grpPayTypeAll.TabStop = false;
            this.grpPayTypeAll.Text = "For Date";
            // 
            // grdPaymentAll
            // 
            this.grdPaymentAll.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.None;
            this.grdPaymentAll.DisplayLayout.GroupByBox.Hidden = true;
            this.grdPaymentAll.DisplayLayout.MaxColScrollRegions = 1;
            this.grdPaymentAll.DisplayLayout.MaxRowScrollRegions = 1;
            this.grdPaymentAll.DisplayLayout.Override.AllowAddNew = Infragistics.Win.UltraWinGrid.AllowAddNew.No;
            this.grdPaymentAll.DisplayLayout.Override.AllowColMoving = Infragistics.Win.UltraWinGrid.AllowColMoving.NotAllowed;
            this.grdPaymentAll.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.False;
            this.grdPaymentAll.DisplayLayout.Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.False;
            this.grdPaymentAll.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            this.grdPaymentAll.DisplayLayout.Override.SelectTypeCell = Infragistics.Win.UltraWinGrid.SelectType.None;
            this.grdPaymentAll.DisplayLayout.Override.SelectTypeCol = Infragistics.Win.UltraWinGrid.SelectType.Single;
            this.grdPaymentAll.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.Single;
            this.grdPaymentAll.Location = new System.Drawing.Point(15, 24);
            this.grdPaymentAll.Name = "grdPaymentAll";
            this.grdPaymentAll.Size = new System.Drawing.Size(334, 341);
            this.grdPaymentAll.TabIndex = 0;
            this.grdPaymentAll.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            // 
            // ultraTabPageControl10
            // 
            this.ultraTabPageControl10.Controls.Add(this.SaleHistoryTab);
            this.ultraTabPageControl10.Location = new System.Drawing.Point(2, 45);
            this.ultraTabPageControl10.Name = "ultraTabPageControl10";
            this.ultraTabPageControl10.Size = new System.Drawing.Size(1034, 479);
            // 
            // SaleHistoryTab
            // 
            this.SaleHistoryTab.Controls.Add(this.ultraTabSharedControlsPage3);
            this.SaleHistoryTab.Controls.Add(this.ultraTabPageControl13);
            this.SaleHistoryTab.Controls.Add(this.ultraTabPageControl14);
            this.SaleHistoryTab.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SaleHistoryTab.Location = new System.Drawing.Point(0, 0);
            this.SaleHistoryTab.Name = "SaleHistoryTab";
            this.SaleHistoryTab.SharedControlsPage = this.ultraTabSharedControlsPage3;
            this.SaleHistoryTab.ShowButtonSeparators = true;
            this.SaleHistoryTab.Size = new System.Drawing.Size(1034, 479);
            this.SaleHistoryTab.TabIndex = 15;
            this.SaleHistoryTab.TabLayoutStyle = Infragistics.Win.UltraWinTabs.TabLayoutStyle.MultiRowSizeToFit;
            appearance41.BackColorAlpha = Infragistics.Win.Alpha.UseAlphaLevel;
            ultraTab1.ClientAreaAppearance = appearance41;
            ultraTab1.Key = "01";
            ultraTab1.TabPage = this.ultraTabPageControl13;
            ultraTab1.Text = "TABULAR VIEW";
            ultraTab2.Key = "02";
            ultraTab2.TabPage = this.ultraTabPageControl14;
            ultraTab2.Text = "GRAPHICAL VIEW";
            this.SaleHistoryTab.Tabs.AddRange(new Infragistics.Win.UltraWinTabControl.UltraTab[] {
            ultraTab1,
            ultraTab2});
            this.SaleHistoryTab.TabStop = false;
            this.SaleHistoryTab.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.SaleHistoryTab.ViewStyle = Infragistics.Win.UltraWinTabControl.ViewStyle.VisualStudio2005;
            // 
            // ultraTabSharedControlsPage3
            // 
            this.ultraTabSharedControlsPage3.Location = new System.Drawing.Point(-10000, -10000);
            this.ultraTabSharedControlsPage3.Name = "ultraTabSharedControlsPage3";
            this.ultraTabSharedControlsPage3.Size = new System.Drawing.Size(1030, 454);
            // 
            // clTo1
            // 
            this.clTo1.BackColor = System.Drawing.SystemColors.Window;
            this.clTo1.Location = new System.Drawing.Point(0, 0);
            this.clTo1.Name = "clTo1";
            this.clTo1.NonAutoSizeHeight = 21;
            this.clTo1.Size = new System.Drawing.Size(121, 21);
            this.clTo1.TabIndex = 0;
            this.clTo1.Value = new System.DateTime(2006, 1, 9, 0, 0, 0, 0);
            // 
            // clFrom1
            // 
            this.clFrom1.BackColor = System.Drawing.SystemColors.Window;
            this.clFrom1.Location = new System.Drawing.Point(1, 1);
            this.clFrom1.Name = "clFrom1";
            this.clFrom1.NonAutoSizeHeight = 21;
            this.clFrom1.Size = new System.Drawing.Size(121, 21);
            this.clFrom1.TabIndex = 0;
            this.clFrom1.Value = new System.DateTime(2006, 1, 9, 0, 0, 0, 0);
            // 
            // ultraDataSource1
            // 
            ultraDataColumn1.DataType = typeof(System.DateTime);
            ultraDataColumn2.DataType = typeof(decimal);
            this.ultraDataSource1.Band.Columns.AddRange(new object[] {
            ultraDataColumn1,
            ultraDataColumn2});
            this.ultraDataSource1.Rows.AddRange(new object[] {
            new Infragistics.Win.UltraWinDataSource.UltraDataRow(new object[] {
                        ((object)("Transdate")),
                        ((object)(new System.DateTime(2006, 1, 2, 0, 0, 0, 0))),
                        ((object)("totalpaid")),
                        ((object)(new decimal(new int[] {
                                    23,
                                    0,
                                    0,
                                    0})))}),
            new Infragistics.Win.UltraWinDataSource.UltraDataRow(new object[] {
                        ((object)("Transdate")),
                        ((object)(new System.DateTime(2006, 1, 5, 0, 0, 0, 0))),
                        ((object)("totalpaid")),
                        ((object)(new decimal(new int[] {
                                    42,
                                    0,
                                    0,
                                    0})))})});
            // 
            // tabMain
            // 
            appearance42.FontData.BoldAsString = "True";
            this.tabMain.ActiveTabAppearance = appearance42;
            this.tabMain.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            appearance43.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(108)))), ((int)(((byte)(172)))));
            this.tabMain.Appearance = appearance43;
            this.tabMain.BackColorInternal = System.Drawing.Color.Transparent;
            appearance44.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(108)))), ((int)(((byte)(172)))));
            appearance44.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(108)))), ((int)(((byte)(172)))));
            this.tabMain.ClientAreaAppearance = appearance44;
            this.tabMain.Controls.Add(this.ultraTabSharedControlsPage1);
            this.tabMain.Controls.Add(this.ultraTabPageControl1);
            this.tabMain.Location = new System.Drawing.Point(14, 10);
            this.tabMain.Name = "tabMain";
            this.tabMain.SharedControlsPage = this.ultraTabSharedControlsPage1;
            this.tabMain.Size = new System.Drawing.Size(1042, 78);
            this.tabMain.Style = Infragistics.Win.UltraWinTabControl.UltraTabControlStyle.PropertyPage2003;
            this.tabMain.TabIndex = 0;
            appearance45.BackColor = System.Drawing.Color.Transparent;
            ultraTab11.Appearance = appearance45;
            appearance46.BackColor = System.Drawing.Color.Transparent;
            appearance46.BackColor2 = System.Drawing.Color.Transparent;
            appearance46.ForeColor = System.Drawing.Color.Black;
            ultraTab11.ClientAreaAppearance = appearance46;
            appearance47.BackColor = System.Drawing.Color.Transparent;
            appearance47.BackColor2 = System.Drawing.Color.Transparent;
            ultraTab11.SelectedAppearance = appearance47;
            ultraTab11.TabPage = this.ultraTabPageControl1;
            ultraTab11.Text = "Criteria";
            this.tabMain.Tabs.AddRange(new Infragistics.Win.UltraWinTabControl.UltraTab[] {
            ultraTab11});
            this.tabMain.TabStop = false;
            this.tabMain.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.tabMain.ViewStyle = Infragistics.Win.UltraWinTabControl.ViewStyle.Office2003;
            // 
            // ultraTabSharedControlsPage1
            // 
            this.ultraTabSharedControlsPage1.Location = new System.Drawing.Point(-10000, -10000);
            this.ultraTabSharedControlsPage1.Name = "ultraTabSharedControlsPage1";
            this.ultraTabSharedControlsPage1.Size = new System.Drawing.Size(1038, 53);
            // 
            // tbData
            // 
            appearance48.FontData.BoldAsString = "True";
            this.tbData.ActiveTabAppearance = appearance48;
            this.tbData.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            appearance49.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(108)))), ((int)(((byte)(172)))));
            this.tbData.Appearance = appearance49;
            this.tbData.BackColorInternal = System.Drawing.Color.Transparent;
            appearance50.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(108)))), ((int)(((byte)(172)))));
            appearance50.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(108)))), ((int)(((byte)(172)))));
            this.tbData.ClientAreaAppearance = appearance50;
            this.tbData.Controls.Add(this.ultraTabSharedControlsPage2);
            this.tbData.Controls.Add(this.ultraTabPageControl2);
            this.tbData.Controls.Add(this.ultraTabPageControl3);
            this.tbData.Controls.Add(this.ultraTabPageControl4);
            this.tbData.Controls.Add(this.ultraTabPageControl5);
            this.tbData.Controls.Add(this.ultraTabPageControl6);
            this.tbData.Controls.Add(this.ultraTabPageControl7);
            this.tbData.Controls.Add(this.ultraTabPageControl8);
            this.tbData.Controls.Add(this.ultraTabPageControl9);
            this.tbData.Controls.Add(this.ultraTabPageControl10);
            this.tbData.Location = new System.Drawing.Point(14, 100);
            this.tbData.Name = "tbData";
            this.tbData.SharedControlsPage = this.ultraTabSharedControlsPage2;
            this.tbData.ShowButtonSeparators = true;
            this.tbData.Size = new System.Drawing.Size(1038, 526);
            this.tbData.TabIndex = 13;
            this.tbData.TabLayoutStyle = Infragistics.Win.UltraWinTabs.TabLayoutStyle.MultiRowSizeToFit;
            appearance51.BackColor = System.Drawing.Color.Transparent;
            ultraTab12.Appearance = appearance51;
            appearance52.BackColor = System.Drawing.Color.Transparent;
            appearance52.BackColor2 = System.Drawing.Color.Transparent;
            appearance52.ForeColor = System.Drawing.Color.Black;
            ultraTab12.ClientAreaAppearance = appearance52;
            ultraTab12.Key = "01";
            appearance53.BackColor = System.Drawing.Color.Transparent;
            appearance53.BackColor2 = System.Drawing.Color.Transparent;
            ultraTab12.SelectedAppearance = appearance53;
            ultraTab12.TabPage = this.ultraTabPageControl2;
            ultraTab12.Text = "SALES TREND";
            ultraTab3.Key = "02";
            ultraTab3.TabPage = this.ultraTabPageControl3;
            ultraTab3.Text = "SALES BY DEPRTMENT";
            ultraTab4.Key = "03";
            ultraTab4.TabPage = this.ultraTabPageControl4;
            ultraTab4.Text = "SALES BY EMPLOYEE";
            ultraTab5.Key = "04";
            ultraTab5.TabPage = this.ultraTabPageControl5;
            ultraTab5.Text = "SALES BY REGISTER";
            ultraTab6.Key = "05";
            ultraTab6.TabPage = this.ultraTabPageControl6;
            ultraTab6.Text = "TOP DEPARTMENTS";
            ultraTab7.Key = "06";
            ultraTab7.TabPage = this.ultraTabPageControl7;
            ultraTab7.Text = "TOP ITEMS";
            ultraTab8.Key = "07";
            ultraTab8.TabPage = this.ultraTabPageControl8;
            ultraTab8.Text = "TOTAL BY TRANS. TYPE";
            ultraTab9.Key = "08";
            ultraTab9.TabPage = this.ultraTabPageControl9;
            ultraTab9.Text = "TOTAL BY PAYMENT TYPE";
            ultraTab10.Key = "09";
            ultraTab10.TabPage = this.ultraTabPageControl10;
            ultraTab10.Text = "SALE HISTORY";
            this.tbData.Tabs.AddRange(new Infragistics.Win.UltraWinTabControl.UltraTab[] {
            ultraTab12,
            ultraTab3,
            ultraTab4,
            ultraTab5,
            ultraTab6,
            ultraTab7,
            ultraTab8,
            ultraTab9,
            ultraTab10});
            this.tbData.TabStop = false;
            this.tbData.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.tbData.ViewStyle = Infragistics.Win.UltraWinTabControl.ViewStyle.VisualStudio2005;
            this.tbData.SelectedTabChanged += new Infragistics.Win.UltraWinTabControl.SelectedTabChangedEventHandler(this.ultraTabControl1_SelectedTabChanged);
            // 
            // ultraTabSharedControlsPage2
            // 
            this.ultraTabSharedControlsPage2.Location = new System.Drawing.Point(-10000, -10000);
            this.ultraTabSharedControlsPage2.Name = "ultraTabSharedControlsPage2";
            this.ultraTabSharedControlsPage2.Size = new System.Drawing.Size(1034, 479);
            // 
            // frmViewMyStore
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(7, 17);
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1065, 640);
            this.Controls.Add(this.tbData);
            this.Controls.Add(this.tabMain);
            this.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmViewMyStore";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "My Store View";
            this.Activated += new System.EventHandler(this.frmSearchMain_Activated);
            this.Load += new System.EventHandler(this.frmSearch_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmSearch_KeyDown);
            this.ultraTabPageControl13.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gdSalesHistory)).EndInit();
            this.ultraTabPageControl14.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.chartSaleHistory)).EndInit();
            this.ultraTabPageControl1.ResumeLayout(false);
            this.ultraTabPageControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.clTo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.clFrom)).EndInit();
            this.ultraTabPageControl2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.chtSalesTrend)).EndInit();
            this.ultraTabPageControl3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.chtSaleByDepartment)).EndInit();
            this.ultraTabPageControl4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.chtSaleByEmp)).EndInit();
            this.ultraTabPageControl5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.chtSaleByRegister)).EndInit();
            this.ultraTabPageControl6.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numTopDept)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chtTopDept)).EndInit();
            this.ultraTabPageControl7.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.chtTopItems)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numTopItem)).EndInit();
            this.ultraTabPageControl8.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numTRTransNo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numTRTransAmount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numTSTransNo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numTSTransAmount)).EndInit();
            this.grpTransTypeAll.ResumeLayout(false);
            this.grpTransTypeAll.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numAllRTransNo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numAllRTransAmount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numAllSTransNo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numAllSTransAmount)).EndInit();
            this.ultraTabPageControl9.ResumeLayout(false);
            this.groupBox6.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdPaymentToday)).EndInit();
            this.grpPayTypeAll.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdPaymentAll)).EndInit();
            this.ultraTabPageControl10.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.SaleHistoryTab)).EndInit();
            this.SaleHistoryTab.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.clTo1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.clFrom1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraDataSource1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tabMain)).EndInit();
            this.tabMain.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.tbData)).EndInit();
            this.tbData.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ultraDataSource2)).EndInit();
            this.ResumeLayout(false);

		}
		#endregion
		private void btnSearch_Click(object sender, System.EventArgs e)
		{
			try
			{
				PopulateViews();
			}
			catch (Exception exp) {clsUIHelper.ShowErrorMsg(exp.Message);}
		}

        private void PopulateViews()
        {
            ViewSalesTrend();
            ViewSaleByDept();
            ViewTopDept();
            ViewSaleByEmp();
            ViewSaleByRegister();
            ViewTopItem();
            ViewTotalByTransType();
            ViewTotalByPaymentType();
            ViewSalesHistory();
        }

		private void ViewSaleByDept()
		{
			DataSet oDataSet=null;
			SearchSvr oSearchSvr= new SearchSvr();
			
			string sSQL = " select d.deptname Department,sum(PTD.ExtendedPrice-PTD.Discount+PTD.TaxAmount) as TotalPaid " +
				" from postransaction pt,postransactiondetail ptd ,item i left join department d on (i.departmentid=d.deptid) " +
				" where pt.transid=ptd.transid and i.itemid=ptd.itemid " +
				" and convert(datetime,transdate,109) between convert(datetime, cast('" + this.clFrom.Text + " 00:00:00 ' as datetime) ,113) and convert(datetime, cast('" + this.clTo.Text  + " 23:59:59 ' as datetime) ,113) " +
				" group by d.deptname  " ;

			//sSQL+= " order by " + clsPOSDBConstants.InvRecvHeader_Fld_InvRecvID + " desc " ;

			oDataSet=oSearchSvr.Search(sSQL);
			
			this.chtSaleByDepartment.Data.DataSource=oDataSet;
			//this.chtSaleByDepartment.BackColor=this.BackColor;
		}
		
		private void ViewTopDept()
		{
			DataSet oDataSet=null;
			SearchSvr oSearchSvr= new SearchSvr();

            //Sprint-19 - 2006 04-Mar-2015 JY Commented
            //string sSQL = " select top " + this.numTopDept.Value.ToString() + " d.deptname Department,sum(totalpaid) as TotalPaid " +
            //    " from postransaction pt,postransactiondetail ptd ,item i left join department d on (i.departmentid=d.deptid) " +
            //    " where pt.transid=ptd.transid and i.itemid=ptd.itemid " +
            //    " and convert(datetime,transdate,109) between convert(datetime, cast('" + this.clFrom.Text + " 00:00:00 ' as datetime) ,113) and convert(datetime, cast('" + this.clTo.Text  + " 23:59:59 ' as datetime) ,113) " +
            //    " group by d.deptname  order by totalpaid desc " ;

            //Sprint-19 - 2006 04-Mar-2015 JY Corrected the above SQL as TotalPaid calculationg from PosTransaction which is incorrect if we have join with PosTransactionDetail
            string sSQL = " SELECT TOP " + this.numTopDept.Value.ToString() + " d.deptname Department, SUM(ptd.ExtendedPrice-ptd.Discount+ptd.TaxAmount) AS TotalPaid " +
                " FROM postransaction pt INNER JOIN postransactiondetail ptd ON pt.transid=ptd.transid INNER JOIN item i ON i.itemid=ptd.itemid LEFT JOIN department d ON i.departmentid=d.deptid " +
                " WHERE convert(datetime,transdate,109) between convert(datetime, cast('" + this.clFrom.Text + " 00:00:00 ' as datetime) ,113) and convert(datetime, cast('" + this.clTo.Text + " 23:59:59 ' as datetime) ,113) " +
                " group by d.deptname order by totalpaid desc ";

            //SUM(ptd.ExtendedPrice-ptd.Discount+ptd.TaxAmount) AS Amount

			//sSQL+= " order by " + clsPOSDBConstants.InvRecvHeader_Fld_InvRecvID + " desc " ;

			oDataSet=oSearchSvr.Search(sSQL);
			
			this.chtTopDept.Data.DataSource=oDataSet;
			//this.chtSaleByDepartment.BackColor=this.BackColor;
		}
		
		private void ViewTopItem()
		{
			DataSet oDataSet=null;
			SearchSvr oSearchSvr= new SearchSvr();

            //Sprint-19 - 2006 04-Mar-2015 JY Commented as it returns wrong output
            //string sSQL = " select top " + this.numTopItem.Value.ToString() + " i.description as Item,sum(totalpaid) as TotalPaid " +
            //    " from postransaction pt,postransactiondetail ptd ,item i " +
            //    " where pt.transid=ptd.transid and i.itemid=ptd.itemid " +
            //    " and convert(datetime,transdate,109) between convert(datetime, cast('" + this.clFrom.Text + " 00:00:00 ' as datetime) ,113) and convert(datetime, cast('" + this.clTo.Text  + " 23:59:59 ' as datetime) ,113) " +
            //    " group by i.description  order by totalpaid desc " ;

            //Sprint-19 - 2006 04-Mar-2015 JY Corrected query
            string sSQL = " SELECT TOP " + this.numTopItem.Value.ToString() + " i.description AS Item, SUM(ptd.ExtendedPrice-ptd.Discount+ptd.TaxAmount) AS TotalPaid FROM postransaction pt " +
                " INNER JOIN postransactiondetail ptd ON pt.transid=ptd.transid " +
                " INNER JOIN item i ON i.itemid=ptd.itemid " +  
                " WHERE convert(datetime,transdate,109) between convert(datetime, cast('" + this.clFrom.Text + " 00:00:00 ' as datetime) ,113) and convert(datetime, cast('" + this.clTo.Text  + " 23:59:59 ' as datetime) ,113) " +
                " group by i.description  order by totalpaid desc"; 

			//sSQL+= " order by " + clsPOSDBConstants.InvRecvHeader_Fld_InvRecvID + " desc " ;

			oDataSet=oSearchSvr.Search(sSQL);
			
			this.chtTopItems.Data.DataSource=oDataSet;
			//this.chtSaleByDepartment.BackColor=this.BackColor;
		}
		
		private void ViewSalesTrend()
		{
			DataSet oDataSet=null;
			SearchSvr oSearchSvr= new SearchSvr();

            //Sprint-19 - 2006 04-Mar-2015 JY Commented
            //string sSQL = " select convert(varchar,transdate,110) as transdate,sum(totalpaid) as TotalPaid from postransaction " +
            //     " where convert(datetime,transdate,109) between convert(datetime, cast('" + this.clFrom.Text + " 00:00:00 ' as datetime) ,113) and convert(datetime, cast('" + this.clTo.Text  + " 23:59:59 ' as datetime) ,113) " +
            //    " group by convert(varchar,transdate,110)  order by transdate asc " ;

            //Sprint-19 - 2006 04-Mar-2015 JY Added
            string sSQL = " select convert(varchar,pt.transdate,110) as transdate, SUM(ptd.ExtendedPrice-ptd.Discount+ptd.TaxAmount) AS TotalPaid from postransaction pt " +
                    " INNER JOIN postransactiondetail ptd ON pt.transid=ptd.transid " +
                    " where convert(datetime,transdate,109) between convert(datetime, cast('" + this.clFrom.Text + " 00:00:00 ' as datetime) ,113) and convert(datetime, cast('" + this.clTo.Text + " 23:59:59 ' as datetime) ,113) " +
                    " group by convert(varchar,transdate,110)  order by transdate asc ";

            //sSQL+= " order by " + clsPOSDBConstants.InvRecvHeader_Fld_InvRecvID + " desc " ;

			oDataSet=oSearchSvr.Search(sSQL);
			
			this.chtSalesTrend.Data.DataSource=oDataSet;
			//this.chtSalesTrend.BackColor=this.BackColor;
		}
		
		private void ViewTotalByTransType()
		{
			DataSet oDataSet=null;
			SearchSvr oSearchSvr= new SearchSvr();
			
			string sSQL = " select count(case transtype when 1 then 1 else null end)as NoofSTrans,count(case transtype when 2 then 1 else null end)as NoofRTrans,sum(case transtype when 1 then totalpaid else 0 end) as TSAmt,sum(case transtype when 2 then totalpaid else 0 end) as TRAmt " +
				" from postransaction where convert(datetime,transdate,109) between convert(datetime, cast('" + this.clFrom.Text + " 00:00:00 ' as datetime) ,113) and convert(datetime, cast('" + this.clTo.Text  + " 23:59:59 ' as datetime) ,113) " +
				" and (transtype=1 or transtype=2)";
			oDataSet=oSearchSvr.Search(sSQL);
			if (oDataSet.Tables[0].Rows.Count>0)
			{
				this.numAllSTransNo.Value=Configuration.convertNullToInt(oDataSet.Tables[0].Rows[0]["noofStrans"].ToString());
				this.numAllSTransAmount.Value=Configuration.convertNullToDecimal(oDataSet.Tables[0].Rows[0]["TSAmt"].ToString());

				this.numAllRTransNo.Value=Configuration.convertNullToInt(oDataSet.Tables[0].Rows[0]["noofRtrans"].ToString());
				this.numAllRTransAmount.Value=Configuration.convertNullToDecimal(oDataSet.Tables[0].Rows[0]["TRAmt"].ToString());
			}
			
			sSQL = " select count(case transtype when 1 then 1 else null end)as NoofSTrans,count(case transtype when 2 then 1 else null end)as NoofRTrans,sum(case transtype when 1 then totalpaid else 0 end) as TSAmt,sum(case transtype when 2 then totalpaid else 0 end) as TRAmt " +
				" from postransaction where convert(datetime,transdate,109) between convert(datetime, cast('" + DateTime.Today.ToShortDateString() + " 00:00:00 ' as datetime) ,113) and convert(datetime, cast('" + DateTime.Today.ToShortDateString()  + " 23:59:59 ' as datetime) ,113) " +
				" and (transtype=1 or transtype=2)";
			oDataSet=oSearchSvr.Search(sSQL);
			if (oDataSet.Tables[0].Rows.Count>0)
			{
				this.numTSTransNo.Value=Configuration.convertNullToInt(oDataSet.Tables[0].Rows[0]["noofStrans"].ToString());
				this.numTSTransAmount.Value=Configuration.convertNullToDecimal(oDataSet.Tables[0].Rows[0]["TSAmt"].ToString());

				this.numTRTransNo.Value=Configuration.convertNullToInt(oDataSet.Tables[0].Rows[0]["noofRtrans"].ToString());
				this.numTRTransAmount.Value=Configuration.convertNullToDecimal(oDataSet.Tables[0].Rows[0]["TRAmt"].ToString());
			}

			this.grpTransTypeAll.Text=" For " + this.clFrom.Text + " To " + this.clTo.Text;
			//this.chtSalesTrend.BackColor=this.BackColor;
		}
		
		private void ViewSaleByEmp()
		{
			DataSet oDataSet=null;
			SearchSvr oSearchSvr= new SearchSvr();

            //Sprint-19 - 2006 04-Mar-2015 JY Commented
            //string sSQL = " select userid as Employee,sum(totalpaid) as TotalPaid from postransaction " +
            //    " where convert(datetime,transdate,109) between convert(datetime, cast('" + this.clFrom.Text + " 00:00:00 ' as datetime) ,113) and convert(datetime, cast('" + this.clTo.Text  + " 23:59:59 ' as datetime) ,113) " +
            //    " group by userid  " ;

            //Sprint-19 - 2006 04-Mar-2015 JY Calculate Total paid from Detail
            string sSQL = " select pt.userid as Employee, SUM(ptd.ExtendedPrice-ptd.Discount+ptd.TaxAmount) AS TotalPaid from postransaction pt" +
                " INNER JOIN postransactiondetail ptd ON pt.transid=ptd.transid " +
                " where convert(datetime,pt.transdate,109) between convert(datetime, cast('" + this.clFrom.Text + " 00:00:00 ' as datetime) ,113) and convert(datetime, cast('" + this.clTo.Text + " 23:59:59 ' as datetime) ,113) " +
                " group by pt.userid  ";

			//sSQL+= " order by " + clsPOSDBConstants.InvRecvHeader_Fld_InvRecvID + " desc " ;

			oDataSet=oSearchSvr.Search(sSQL);
			
			this.chtSaleByEmp.PieChart.ColumnIndex=-1;
			this.chtSaleByEmp.Data.DataSource=oDataSet.Tables[0];
			//this.chtSalesTrend.BackColor=this.BackColor;
		}
		
		private void ViewSaleByRegister()
		{
			DataSet oDataSet=null;
			SearchSvr oSearchSvr= new SearchSvr();

            //Sprint-19 - 2006 04-Mar-2015 JY Commented
            //string sSQL = " select stationid as Register,sum(totalpaid) as TotalPaid from postransaction " +
            //    " where convert(datetime,transdate,109) between convert(datetime, cast('" + this.clFrom.Text + " 00:00:00 ' as datetime) ,113) and convert(datetime, cast('" + this.clTo.Text  + " 23:59:59 ' as datetime) ,113) " +
            //    " group by stationid  " ;

            //Sprint-19 - 2006 04-Mar-2015 JY Calculate Total paid from Detail
            string sSQL = " SELECT pt.stationid AS Register, SUM(ptd.ExtendedPrice-ptd.Discount+ptd.TaxAmount) AS TotalPaid FROM postransaction pt" +
                " INNER JOIN postransactiondetail ptd ON pt.transid=ptd.transid " +
                " WHERE convert(datetime,pt.transdate,109) between convert(datetime, cast('" + this.clFrom.Text + " 00:00:00 ' as datetime) ,113) and convert(datetime, cast('" + this.clTo.Text + " 23:59:59 ' as datetime) ,113) " +
                " group by pt.stationid  ";

			//sSQL+= " order by " + clsPOSDBConstants.InvRecvHeader_Fld_InvRecvID + " desc " ;

			oDataSet=oSearchSvr.Search(sSQL);
			
			this.chtSaleByRegister.PieChart.ColumnIndex=-1;
			this.chtSaleByRegister.Data.DataSource=oDataSet;
			//this.chtSalesTrend.BackColor=this.BackColor;
		}
		
		private void ViewTotalByPaymentType()
		{
			DataSet oDataSet=null;
			SearchSvr oSearchSvr= new SearchSvr();
			
			string sSQL = " select paytypedesc as [Payment Type], sum(Amount) as [Amount] from postranspayment pt ,paytype ptype where pt.transtypecode=ptype.paytypeid " +
				" and convert(datetime,transdate,109) between convert(datetime, cast('" + this.clFrom.Text + " 00:00:00 ' as datetime) ,113) and convert(datetime, cast('" + this.clTo.Text  + " 23:59:59 ' as datetime) ,113) " +
				" group by paytypedesc ";
			oDataSet=oSearchSvr.Search(sSQL);
			this.grdPaymentAll.DataSource=oDataSet.Tables[0];
			resizeColumns(this.grdPaymentAll);
			
			sSQL = " select paytypedesc as [Payment Type], sum(Amount) as [Amount] from postranspayment pt ,paytype ptype where pt.transtypecode=ptype.paytypeid " +
				" and convert(datetime,transdate,109) between convert(datetime, cast('" + DateTime.Today.ToShortDateString() + " 00:00:00 ' as datetime) ,113) and convert(datetime, cast('" + DateTime.Today.ToShortDateString()  + " 23:59:59 ' as datetime) ,113) " +
				" group by paytypedesc ";

			oDataSet=oSearchSvr.Search(sSQL);
			this.grdPaymentToday.DataSource=oDataSet.Tables[0];
			resizeColumns(this.grdPaymentToday);
			this.grpPayTypeAll.Text=" For " + this.clFrom.Text + " To " + this.clTo.Text;
		}
		
		private void initChart(Infragistics.Win.UltraWinChart.UltraChart oChart)
		{
			oChart.Axis.X.ScrollScale.Visible=true;
			oChart.Axis.Y.ScrollScale.Visible=true;

			//picScroll.Image=this.imageList1.Images[8];
			oChart.ScrollBarImage = (System.Drawing.Bitmap)picScroll.Image;
			//oChart.ScrollBarImage = (System.Drawing.Bitmap)this.imageList1.Images[Convert.ToInt32((new System.Random()).Next(0,9))];
				
			((Infragistics.UltraChart.Resources.IChartComponent)oChart).Invalidate(Infragistics.UltraChart.Shared.Styles.CacheLevel.LayerLevelCache);

			oChart.TitleTop.FontColor=Color.Blue;
			oChart.TitleLeft.FontColor=Color.Blue;
			oChart.TitleBottom.FontColor=Color.Blue;
			oChart.TitleRight.FontColor=Color.Blue;

			oChart.Axis.X.Labels.FontColor=Color.Blue;
			oChart.Axis.X2.Labels.FontColor=Color.Blue;
			oChart.Axis.Y.Labels.FontColor=Color.Blue;
			oChart.Axis.Y2.Labels.FontColor=Color.Blue;

			oChart.Legend.BackgroundColor=Color.Transparent;
			oChart.Legend.BorderColor=Color.Transparent;
			oChart.Legend.FontColor=Color.Blue;

			oChart.Axis.X.Labels.Orientation=Infragistics.UltraChart.Shared.Styles.TextOrientation.Custom;
			oChart.Axis.X.Labels.OrientationAngle=-155;
			
			oChart.Axis.X.Labels.Flip=true;
			oChart.Axis.X.Extent=110;
		}

		private void btnCancel_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}

		private void frmSearch_Load(object sender, System.EventArgs e)
		{
			try
			{
                this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
				this.clFrom.Value=DateTime.Today.Date.Subtract(new System.TimeSpan(30,0,0,0));
				this.clTo.Value=DateTime.Today;
				clsUIHelper.setColorSchecme(this);
				this.numTopDept.Value=5;
				this.numTopItem.Value=5;
				PopulateViews();

				this.grdPaymentAll.DisplayLayout.Appearance.BackColor=this.BackColor;
				this.grdPaymentToday.DisplayLayout.Appearance.BackColor=this.BackColor;

				initChart( this.chtSaleByDepartment);
				initChart( this.chtSaleByEmp);
				initChart( this.chtSaleByRegister);
				initChart( this.chtSalesTrend);
				initChart( this.chtTopDept);
				initChart( this.chtTopItems);
                initChart(this.chartSaleHistory);

				this.chtSaleByEmp.MouseUp += new System.Windows.Forms.MouseEventHandler(this.chart_MouseUp);
				this.chtSaleByDepartment.MouseUp += new System.Windows.Forms.MouseEventHandler(this.chart_MouseUp);
				this.chtSaleByRegister.MouseUp += new System.Windows.Forms.MouseEventHandler(this.chart_MouseUp);
				this.chtSalesTrend.MouseUp += new System.Windows.Forms.MouseEventHandler(this.chart_MouseUp);
				this.chtTopDept.MouseUp += new System.Windows.Forms.MouseEventHandler(this.chart_MouseUp);
				this.chtTopItems.MouseUp += new System.Windows.Forms.MouseEventHandler(this.chart_MouseUp);
                this.chartSaleHistory.MouseUp += new System.Windows.Forms.MouseEventHandler(this.chart_MouseUp);
                this.Cursor = System.Windows.Forms.Cursors.Default;
			}
			catch(Exception exp)
			{
                this.Cursor = System.Windows.Forms.Cursors.Default;
				clsUIHelper.ShowErrorMsg( exp.Message);
			}
		}
		private void resizeColumns(UltraGrid grdSearch)
		{
			try
			{
				foreach(UltraGridBand oBand in grdSearch.DisplayLayout.Bands)
				{
					foreach (UltraGridColumn oCol in oBand.Columns)
					{
						oCol.Width =oCol.CalculateAutoResizeWidth(PerformAutoSizeType.VisibleRows,true)+10;
						if ( oCol.DataType.Equals(typeof(System.Int32)) || oCol.DataType.Equals(typeof(System.Decimal)) || oCol.DataType.Equals(typeof(System.DateTime)))
						{
							oCol.CellAppearance.TextHAlign=Infragistics.Win.HAlign.Right ;
						}
					}
				}
			}
			catch (Exception ){}
		}
		private void frmSearch_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			try
			{
				if(e.KeyData==System.Windows.Forms.Keys.Enter && this.ActiveControl.Name != "grdSearch")
				{
					this.SelectNextControl(this.ActiveControl,true,true,true,true);
				}
			}
			catch(Exception exp)
			{
				clsUIHelper.ShowErrorMsg( exp.Message);
			}
		}

		private void btnClose_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}

		private void frmSearchMain_Activated(object sender, System.EventArgs e)
		{
			clsUIHelper.CurrentForm=this;
		}

        //Added By shitaljit on 22 May 2013 for disabling From and To date in case user is lookinf fo r sale history JIRA-933
		private void ultraTabControl1_SelectedTabChanged(object sender, Infragistics.Win.UltraWinTabControl.SelectedTabChangedEventArgs e)
		{
		     Infragistics.Win.UltraWinTabControl.UltraTabControl ObjTabControl = (Infragistics.Win.UltraWinTabControl.UltraTabControl) sender;
             try
             {
                 if (ObjTabControl.ActiveTab.Key == "09")
                 {
                     this.clFrom.Enabled = this.clTo.Enabled = false;
                 }
                 else
                 {
                     this.clFrom.Enabled = this.clTo.Enabled = true;
                 }
             }
             catch { }
		}

		private void chart_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			return;
//			if (e.Button==MouseButtons.Right)
//			{
//				chtType.Show();
//				chtType.Left=e.X;
//				chtType.Top=e.Y;
//				oChart=(Infragistics.Win.UltraWinChart.UltraChart)sender;
//			}
		}

		private void chtType_Click(object sender, System.EventArgs e)
		{
			return;
//			try
//			{
//				if (oChart==null) return;
//				oChart.ChartType=(Infragistics.UltraChart.Shared.Styles.ChartType)this.chtType.GetValue();
//				//oChart.Refresh();
//				if (oChart.ChartType==Infragistics.UltraChart.Shared.Styles.ChartType.PieChart ||
//					oChart.ChartType==Infragistics.UltraChart.Shared.Styles.ChartType.PieChart3D ||
//					oChart.ChartType==Infragistics.UltraChart.Shared.Styles.ChartType.RadarChart ||
//					oChart.ChartType==Infragistics.UltraChart.Shared.Styles.ChartType.ScatterChart ||
//					oChart.ChartType==Infragistics.UltraChart.Shared.Styles.ChartType.ScatterLineChart ||
//					oChart.ChartType==Infragistics.UltraChart.Shared.Styles.ChartType.HeatMapChart ||
//					oChart.ChartType==Infragistics.UltraChart.Shared.Styles.ChartType.CandleChart ||
//					oChart.ChartType==Infragistics.UltraChart.Shared.Styles.ChartType.StepLineChart ||
//					oChart.ChartType==Infragistics.UltraChart.Shared.Styles.ChartType.StepAreaChart ||
//					oChart.ChartType==Infragistics.UltraChart.Shared.Styles.ChartType.GanttChart ||
//					oChart.ChartType==Infragistics.UltraChart.Shared.Styles.ChartType.PolarChart ||
//					oChart.ChartType==Infragistics.UltraChart.Shared.Styles.ChartType.DoughnutChart ||
//					oChart.ChartType==Infragistics.UltraChart.Shared.Styles.ChartType.BubbleChart
//					)
//				{
//					oChart.Data.SwapRowsAndColumns=false;
//					oChart.PieChart.ColumnIndex=-1;
//					oChart.Legend.Visible=true;
//				}
//				else
//				{
//					oChart.Data.SwapRowsAndColumns=true;
//					oChart.Legend.Visible=false;
//				}
//
//				oChart.Axis.X.Labels.Orientation=Infragistics.UltraChart.Shared.Styles.TextOrientation.Custom;
//				oChart.Axis.X.Labels.OrientationAngle=-145;
//				oChart.Axis.X.Labels.Flip=true;
//				oChart.Axis.X.Extent=110;
//
//				this.chtType.Hide();
//				oChart=null;
//			}
//			catch (Exception exp)
//			{
//				clsUIHelper.ShowErrorMsg(exp.Message);
//			}
        }

        #region Sale History 
        private void ViewSalesHistory()
        {
            Search oSearch = new Search();
            string SSQL = string.Empty;
            SSQL = @"DECLARE @CurrDt datetime
                DECLARE @StartDt DateTime
                Set @CurrDt = EOMONTH(getdate())
                SET @StartDt = DateAdd(day,1,EOMONTH((DATEADD(DAY,-1825,@CurrDt )),-1))
                Select Year_Months.Month_Name, Year_Months.[Year],CAST(Sum(NetAmount) AS VARCHAR(20)) + ' (' +  
                CAST( Count(TransID) AS VARCHAR(20)) + ')'  AS Total ,Year_Months.Month_Num
                From ( SELECT DATENAME(month, PT.TransDate) AS 'Month_Name'
                ,DATEPart(MM, PT.TransDate) AS 'Month_Num' ,DATENAME(Year, PT.TransDate) AS 'Year',
                TransID,case TransType when 3 then PT.TotalPaid else  PT.GrossTotal+PT.TotalTaxAmount-PT.TotalDiscAmount end as NetAmount,    
                Case TransType when 1 Then 'Sale' when 2 Then 'Return' when 3 then 'ROA' end as TransType 
                From postransaction PT where transdate>@StartDt) as TempTable
                Right Outer Join ( SELECT * FROM (
                Select 'January' as Month_Name, 1 as Month_Num
                Union Select 'February' as Month_Name, 2 as Month_Num
                Union Select 'March' as Month_Name, 3 as Month_Num
                Union Select 'April' as Month_Name, 4 as Month_Num
                Union Select 'May' as Month_Name, 5 as Month_Num
                Union Select 'June' as Month_Name, 6 as Month_Num
                Union Select 'July' as Month_Name, 7 as Month_Num
                Union Select 'August' as Month_Name, 8 as Month_Num
                Union Select 'September' as Month_Name, 9 as Month_Num
                Union Select 'October' as Month_Name, 10 as Month_Num
                Union Select 'November' as Month_Name, 11  as Month_Num
                Union Select 'December' as Month_Name, 12  as Month_Num
                ) AS MonthName,
                (Select Year(TransDate) as [Year] from postransaction PT  where transdate>@StartDt Group by
                 Year(TransDate) ) as YearName) as Year_Months On Year_Months.Month_Num=TempTable.Month_Num And Year_Months.[Year]=TempTable.[Year]
                Where Cast(Year_Months.[Year] as Varchar)+ Right('00'+Cast(Year_Months.Month_Num as Varchar),2) <=Cast(Year(@CurrDt) as Varchar)+ Right('00'+Cast(Month(@CurrDt) as Varchar),2) 
                And Cast(Year_Months.[Year] as Varchar)+ Right('00'+Cast(Year_Months.Month_Num as Varchar),2) >= Cast(Year(@StartDt) as Varchar)+ Right('00'+Cast(Month(@StartDt) as Varchar),2) 
                Group by Year_Months.[Year] ,TempTable.Month_Name, Year_Months.Month_Num,Year_Months.Month_Name
                Order by Year_Months.[Year], Year_Months.Month_Num Asc";

            DataTable dt = oSearch.SearchData(SSQL).Tables[0];
            DataTable dtSaleHistory = new DataTable("SaleHistoryData");

            dtSaleHistory.Columns.Add("Year", typeof(string));
            dtSaleHistory.Columns.Add("Jan", typeof(string));
            dtSaleHistory.Columns.Add("Feb", typeof(string));
            dtSaleHistory.Columns.Add("Mar", typeof(string));
            dtSaleHistory.Columns.Add("Apr", typeof(string));
            dtSaleHistory.Columns.Add("May", typeof(string));
            dtSaleHistory.Columns.Add("Jun", typeof(string));
            dtSaleHistory.Columns.Add("Jul", typeof(string));
            dtSaleHistory.Columns.Add("Aug", typeof(string));
            dtSaleHistory.Columns.Add("Sep", typeof(string));
            dtSaleHistory.Columns.Add("Oct", typeof(string));
            dtSaleHistory.Columns.Add("Nov", typeof(string));
            dtSaleHistory.Columns.Add("Dec", typeof(string));
            dtSaleHistory.Columns.Add("Total", typeof(string));
            string[] fields = new String[14];
            DateTime currDate = DateTime.Now;
            int YearAgo = -4;
            string lastYear = DateTime.Today.AddYears(YearAgo).Year.ToString();
            int nCurrMonth = DateTime.Today.Month;
            int RowIndex = 0;
            Decimal TotAmount = 0;
            Decimal TotTransCount = 0;
            foreach (DataRow oRow in dt.Rows)
            {
                //Sprint-19 - 2006 15-May-2015 JY Added because logic is not working if we have the data more than past 4 years  
                if (Configuration.convertNullToInt(oRow["Year"]) < Configuration.convertNullToInt(lastYear))
                {
                    RowIndex++;
                    continue;  
                }

                if (Configuration.convertNullToString(oRow["Year"]).Equals(lastYear))
                {
                    fields[0] = lastYear;
                    if (Configuration.convertNullToString(oRow["Year"]).Equals(lastYear) && Configuration.convertNullToString(oRow["Month_Name"]).Equals("January"))
                    {
                        fields[1] = string.IsNullOrEmpty(Configuration.convertNullToString(oRow["Total"])) ? "0.00(0)" : Configuration.convertNullToString(oRow["Total"]);
                        TotAmount += Configuration.convertNullToDecimal(fields[1].ToString().Split('(')[0].Trim());
                        TotTransCount += Configuration.convertNullToDecimal(fields[1].ToString().Split(')')[0].Split('(')[1].Trim());
                        nCurrMonth = 1;
                    }
                    else if (Configuration.convertNullToString(oRow["Year"]).Equals(lastYear) && Configuration.convertNullToString(oRow["Month_Name"]).Equals("February"))
                    {
                        fields[2] = string.IsNullOrEmpty(Configuration.convertNullToString(oRow["Total"])) ? "0.00(0)" : Configuration.convertNullToString(oRow["Total"]);
                        TotAmount += Configuration.convertNullToDecimal(fields[2].ToString().Split('(')[0].Trim());
                        TotTransCount += Configuration.convertNullToDecimal(fields[2].ToString().Split(')')[0].Split('(')[1].Trim());
                        nCurrMonth = 2;
                    }
                    else if (Configuration.convertNullToString(oRow["Year"]).Equals(lastYear) && Configuration.convertNullToString(oRow["Month_Name"]).Equals("March"))
                    {
                        fields[3] = string.IsNullOrEmpty(Configuration.convertNullToString(oRow["Total"])) ? "0.00(0)" : Configuration.convertNullToString(oRow["Total"]);
                        TotAmount += Configuration.convertNullToDecimal(fields[3].ToString().Split('(')[0].Trim());
                        TotTransCount += Configuration.convertNullToDecimal(fields[3].ToString().Split(')')[0].Split('(')[1].Trim());
                        nCurrMonth = 3;
                    }
                    else if (Configuration.convertNullToString(oRow["Year"]).Equals(lastYear) && Configuration.convertNullToString(oRow["Month_Name"]).Equals("April"))
                    {
                        fields[4] = string.IsNullOrEmpty(Configuration.convertNullToString(oRow["Total"])) ? "0.00(0)" : Configuration.convertNullToString(oRow["Total"]);
                        TotAmount += Configuration.convertNullToDecimal(fields[4].ToString().Split('(')[0].Trim());
                        TotTransCount += Configuration.convertNullToDecimal(fields[4].ToString().Split(')')[0].Split('(')[1].Trim());
                        nCurrMonth = 4;
                    }
                    else if (Configuration.convertNullToString(oRow["Year"]).Equals(lastYear) && Configuration.convertNullToString(oRow["Month_Name"]).Equals("May"))
                    {
                        fields[5] = string.IsNullOrEmpty(Configuration.convertNullToString(oRow["Total"])) ? "0.00(0)" : Configuration.convertNullToString(oRow["Total"]);
                        TotAmount += Configuration.convertNullToDecimal(fields[5].ToString().Split('(')[0].Trim());
                        TotTransCount += Configuration.convertNullToDecimal(fields[5].ToString().Split(')')[0].Split('(')[1].Trim());
                        nCurrMonth = 5;
                    }
                    else if (Configuration.convertNullToString(oRow["Year"]).Equals(lastYear) && Configuration.convertNullToString(oRow["Month_Name"]).Equals("June"))
                    {
                        fields[6] = string.IsNullOrEmpty(Configuration.convertNullToString(oRow["Total"])) ? "0.00(0)" : Configuration.convertNullToString(oRow["Total"]);
                        TotAmount += Configuration.convertNullToDecimal(fields[6].ToString().Split('(')[0].Trim());
                        TotTransCount += Configuration.convertNullToDecimal(fields[6].ToString().Split(')')[0].Split('(')[1].Trim());
                        nCurrMonth = 6;
                    }
                    else if (Configuration.convertNullToString(oRow["Year"]).Equals(lastYear) && Configuration.convertNullToString(oRow["Month_Name"]).Equals("July"))
                    {
                        fields[7] = string.IsNullOrEmpty(Configuration.convertNullToString(oRow["Total"])) ? "0.00(0)" : Configuration.convertNullToString(oRow["Total"]);
                        TotAmount += Configuration.convertNullToDecimal(fields[7].ToString().Split('(')[0].Trim());
                        TotTransCount += Configuration.convertNullToDecimal(fields[7].ToString().Split(')')[0].Split('(')[1].Trim());
                        nCurrMonth = 7;
                    }
                    else if (Configuration.convertNullToString(oRow["Year"]).Equals(lastYear) && Configuration.convertNullToString(oRow["Month_Name"]).Equals("August"))
                    {
                        fields[8] = string.IsNullOrEmpty(Configuration.convertNullToString(oRow["Total"])) ? "0.00(0)" : Configuration.convertNullToString(oRow["Total"]);
                        TotAmount += Configuration.convertNullToDecimal(fields[8].ToString().Split('(')[0].Trim());
                        TotTransCount += Configuration.convertNullToDecimal(fields[8].ToString().Split(')')[0].Split('(')[1].Trim());
                        nCurrMonth = 8;
                    }
                    else if (Configuration.convertNullToString(oRow["Year"]).Equals(lastYear) && Configuration.convertNullToString(oRow["Month_Name"]).Equals("September"))
                    {
                        fields[9] = string.IsNullOrEmpty(Configuration.convertNullToString(oRow["Total"])) ? "0.00(0)" : Configuration.convertNullToString(oRow["Total"]);
                        TotAmount += Configuration.convertNullToDecimal(fields[9].ToString().Split('(')[0].Trim());
                        TotTransCount += Configuration.convertNullToDecimal(fields[9].ToString().Split(')')[0].Split('(')[1].Trim());
                        nCurrMonth = 9;
                    }
                    else if (Configuration.convertNullToString(oRow["Year"]).Equals(lastYear) && Configuration.convertNullToString(oRow["Month_Name"]).Equals("October"))
                    {
                        fields[10] = string.IsNullOrEmpty(Configuration.convertNullToString(oRow["Total"])) ? "0.00(0)" : Configuration.convertNullToString(oRow["Total"]);
                        TotAmount += Configuration.convertNullToDecimal(fields[10].ToString().Split('(')[0].Trim());
                        TotTransCount += Configuration.convertNullToDecimal(fields[10].ToString().Split(')')[0].Split('(')[1].Trim());
                        nCurrMonth = 10;
                    }
                    else if (Configuration.convertNullToString(oRow["Year"]).Equals(lastYear) && Configuration.convertNullToString(oRow["Month_Name"]).Equals("November"))
                    {
                        fields[11] = string.IsNullOrEmpty(Configuration.convertNullToString(oRow["Total"])) ? "0.00(0)" : Configuration.convertNullToString(oRow["Total"]);
                        TotAmount += Configuration.convertNullToDecimal(fields[11].ToString().Split('(')[0].Trim());
                        TotTransCount += Configuration.convertNullToDecimal(fields[11].ToString().Split(')')[0].Split('(')[1].Trim());
                        nCurrMonth = 11;
                    }
                    else if (Configuration.convertNullToString(oRow["Year"]).Equals(lastYear) && Configuration.convertNullToString(oRow["Month_Name"]).Equals("December"))
                    {
                        fields[12] = string.IsNullOrEmpty(Configuration.convertNullToString(oRow["Total"])) ? "0.00(0)" : Configuration.convertNullToString(oRow["Total"]);
                        TotAmount += Configuration.convertNullToDecimal(fields[12].ToString().Split('(')[0].Trim());
                        TotTransCount += Configuration.convertNullToDecimal(fields[12].ToString().Split(')')[0].Split('(')[1].Trim());
                        nCurrMonth = 12;
                    }
                    if (Configuration.convertNullToString(oRow["Month_Num"]).Equals("12") == true)
                    {
                        fields[13] = TotAmount.ToString() + "(" + TotTransCount.ToString() + ")";
                        dtSaleHistory.Rows.Add(fields);
                        fields = new String[14];
                        YearAgo++;
                        TotTransCount = 0;
                        TotAmount = 0;
                        lastYear = DateTime.Today.AddYears(YearAgo).Year.ToString();
                    }
                }
                else
                {
                    fields[0] = lastYear;
                    for (int Index = 1; Index <= 13; Index++)
                    {
                        fields[Index] = "0.00(0)";
                    }
                    dtSaleHistory.Rows.Add(fields);
                    fields = new String[14];
                    YearAgo++;
                    TotTransCount = 0;
                    TotAmount = 0;
                    lastYear = DateTime.Today.AddYears(YearAgo).Year.ToString();
                    if (Configuration.convertNullToString(oRow["Year"]).Equals(lastYear) && Configuration.convertNullToString(oRow["Month_Name"]).Equals("January"))
                    {
                        fields[1] = string.IsNullOrEmpty(Configuration.convertNullToString(oRow["Total"])) ? "0.00(0)" : Configuration.convertNullToString(oRow["Total"]);
                        TotAmount += Configuration.convertNullToDecimal(fields[1].ToString().Split('(')[0].Trim());
                        TotTransCount += Configuration.convertNullToDecimal(fields[1].ToString().Split(')')[0].Split('(')[1].Trim());
                    }
                }
                RowIndex++;
                if (RowIndex == dt.Rows.Count && nCurrMonth != 12)
                {
                    for (int Index = nCurrMonth+1; Index <= 12; Index++)
                    {
                        fields[Index] = "0.00(0)";
                    }
                    fields[13] = TotAmount.ToString() + "(" + TotTransCount.ToString() + ")";
                    dtSaleHistory.Rows.Add(fields);
                }
            }
            gdSalesHistory.DataSource = dtSaleHistory;
            showSaleHistoryByGraphical("C");

        }
        private void showSaleHistoryByGraphical(string sType)
        {
            DataTable dt = (DataTable)gdSalesHistory.DataSource;
            DataTable dtGraph = new DataTable("Search Data");

            dtGraph.Columns.Add("Year", typeof(string));
            dtGraph.Columns.Add("Jan", typeof(Double));
            dtGraph.Columns.Add("Feb", typeof(Double));
            dtGraph.Columns.Add("Mar", typeof(Double));
            dtGraph.Columns.Add("Apr", typeof(Double));
            dtGraph.Columns.Add("May", typeof(Double));
            dtGraph.Columns.Add("Jun", typeof(Double));
            dtGraph.Columns.Add("Jul", typeof(Double));
            dtGraph.Columns.Add("Aug", typeof(Double));
            dtGraph.Columns.Add("Sep", typeof(Double));
            dtGraph.Columns.Add("Oct", typeof(Double));
            dtGraph.Columns.Add("Nov", typeof(Double));
            dtGraph.Columns.Add("Dec", typeof(Double));
            DataRow drg = dtGraph.NewRow(); ;
           
            if (sType == "A")
            {
                foreach (DataRow dr in dt.Rows)
                {
                    try
                    {
                        drg = dtGraph.NewRow();
                        drg[0] = dr.ItemArray[0];
                        drg[1] = dr.ItemArray[1].ToString().Split('(')[0];
                        drg[2] = dr.ItemArray[2].ToString().Split('(')[0];
                        drg[3] = dr.ItemArray[3].ToString().Split('(')[0];
                        drg[4] = dr.ItemArray[4].ToString().Split('(')[0];
                        drg[5] = dr.ItemArray[5].ToString().Split('(')[0];
                        drg[6] = dr.ItemArray[6].ToString().Split('(')[0];
                        drg[7] = dr.ItemArray[7].ToString().Split('(')[0];
                        drg[8] = dr.ItemArray[8].ToString().Split('(')[0];
                        drg[9] = dr.ItemArray[9].ToString().Split('(')[0];
                        drg[10] = dr.ItemArray[10].ToString().Split('(')[0];
                        drg[11] = dr.ItemArray[11].ToString().Split('(')[0];
                        drg[12] = dr.ItemArray[12].ToString().Split('(')[0];
                        dtGraph.Rows.Add(drg);
                    }
                    catch
                    {
                        continue;
                    }
                }

                chartSaleHistory.TitleTop.Text = "Sale History (By Trans Amount)";
            }
            else if (sType == "C")
            {
                foreach (DataRow dr in dt.Rows)
                {
                    try
                    {
                        drg = dtGraph.NewRow();
                        drg[0] = dr.ItemArray[0];
                        drg[1] = dr.ItemArray[1].ToString().Split(')')[0].Split('(')[1];
                        drg[2] = dr.ItemArray[2].ToString().Split(')')[0].Split('(')[1];
                        drg[3] = dr.ItemArray[3].ToString().Split(')')[0].Split('(')[1];
                        drg[4] = dr.ItemArray[4].ToString().Split(')')[0].Split('(')[1];
                        drg[5] = dr.ItemArray[5].ToString().Split(')')[0].Split('(')[1];
                        drg[6] = dr.ItemArray[6].ToString().Split(')')[0].Split('(')[1];
                        drg[7] = dr.ItemArray[7].ToString().Split(')')[0].Split('(')[1];
                        drg[8] = dr.ItemArray[8].ToString().Split(')')[0].Split('(')[1];
                        drg[9] = dr.ItemArray[9].ToString().Split(')')[0].Split('(')[1];
                        drg[10] = dr.ItemArray[10].ToString().Split(')')[0].Split('(')[1];
                        drg[11] = dr.ItemArray[11].ToString().Split(')')[0].Split('(')[1];
                        drg[12] = dr.ItemArray[12].ToString().Split(')')[0].Split('(')[1];
                        dtGraph.Rows.Add(drg);
                        
                    }
                    catch
                    {
                        continue;
                    }
                }
              
                chartSaleHistory.TitleTop.Text = "Sale History (By Trans Count)";
            }
           
            chartSaleHistory.Dock = DockStyle.Fill;
            chartSaleHistory.Series.Clear();
            chartSaleHistory.Series.Add(GetNumericSeriesUnBound("", drg));
            chartSaleHistory.Data.DataSource = dtGraph;
            chartSaleHistory.Data.DataBind();

        }
        private  NumericSeries GetNumericSeriesUnBound(string lbl, DataRow dr)
        {
            NumericSeries series = new NumericSeries();
            try
            {

                series.Label = "Yaer " + lbl;
                // this code populates the series using unbound data
                series.Points.Add(new NumericDataPoint(Configuration.convertNullToDouble(dr["Jan"]), "Jan", false));
                series.Points.Add(new NumericDataPoint(Configuration.convertNullToDouble(dr["Feb"]), "Feb", false));
                series.Points.Add(new NumericDataPoint(Configuration.convertNullToDouble(dr["Mar"]), "Mar", false));
                series.Points.Add(new NumericDataPoint(Configuration.convertNullToDouble(dr["Apr"]), "Apr", false));
                series.Points.Add(new NumericDataPoint(Configuration.convertNullToDouble(dr["May"]), "May", false));
                series.Points.Add(new NumericDataPoint(Configuration.convertNullToDouble(dr["Jun"]), "Jun", false));
                series.Points.Add(new NumericDataPoint(Configuration.convertNullToDouble(dr["Jul"]), "Jul", false));
                series.Points.Add(new NumericDataPoint(Configuration.convertNullToDouble(dr["Aug"]), "Aug", false));
                series.Points.Add(new NumericDataPoint(Configuration.convertNullToDouble(dr["Sep"]), "Sep", false));
                series.Points.Add(new NumericDataPoint(Configuration.convertNullToDouble(dr["Oct"]), "Oct", false));
                series.Points.Add(new NumericDataPoint(Configuration.convertNullToDouble(dr["Nov"]), "Nov", false));
                series.Points.Add(new NumericDataPoint(Configuration.convertNullToDouble(dr["Dec"]), "Dec", false));
            }
            catch (Exception EX)
            {

            }
            return series;
        }
        private  NumericSeries GetNumericSeriesBound(DataTable dt, string strValueColumn)
        {
            NumericSeries series = new NumericSeries();
            try
            {
                series.Label = "Series A";
                // this code populates the series from an external data source
                DataTable table = dt;
                series.Data.DataSource = table;
                series.Data.LabelColumn = strValueColumn;
                series.Data.ValueColumn = "Year";

            }
            catch (Exception Ex)
            {

                throw;
            }
            return series;
        }
        private void gdSalesHistory_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            // Change Backcolor of first column "Year" to light gray.
            e.Layout.Bands[0].Columns[0].CellAppearance.BackColor = Color.LightGray;
            // Align text of first column (Year) to Center. All then years will be displayed in center of column.
            e.Layout.Bands[0].Columns[0].CellAppearance.TextHAlign = Infragistics.Win.HAlign.Center;
            // Align header text of all the columns to center.
            e.Layout.Bands[0].Override.HeaderAppearance.TextHAlign = Infragistics.Win.HAlign.Center;
            // Now set width of each column
            e.Layout.Bands[0].Columns[0].Width = 80;
            e.Layout.Bands[0].Columns[1].Width = 150;
            e.Layout.Bands[0].Columns[2].Width = 150;
            e.Layout.Bands[0].Columns[3].Width = 150;
            e.Layout.Bands[0].Columns[4].Width = 150;
            e.Layout.Bands[0].Columns[5].Width = 150;
            e.Layout.Bands[0].Columns[6].Width = 150;
            e.Layout.Bands[0].Columns[7].Width = 150;
            e.Layout.Bands[0].Columns[8].Width = 150;
            e.Layout.Bands[0].Columns[9].Width = 150;
            e.Layout.Bands[0].Columns[10].Width = 150;
            e.Layout.Bands[0].Columns[11].Width = 150;
            e.Layout.Bands[0].Columns[12].Width = 150;
            e.Layout.Bands[0].Columns["Total"].Width = 200;
        }
        #endregion

        private void btnByTransCount_Click(object sender, EventArgs e)
        {
            showSaleHistoryByGraphical("C");
        }

        private void btnByAmount_Click(object sender, EventArgs e)
        {
            showSaleHistoryByGraphical("A");
        }
    }
}
