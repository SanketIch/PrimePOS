using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using CrystalDecisions.Shared;
using CrystalDecisions.ReportSource;
using CrystalDecisions.CrystalReports.Engine;
using POS_Core_UI.Reports.Reports;
//using POS.UI;
using POS_Core.CommonData;

namespace POS_Core_UI.Reports.ReportsUI
{
	/// <summary>
	/// Summary description for frmInventoryReports.
	/// </summary>
	public class frmRptSaleAnalysisByProduct : System.Windows.Forms.Form
	{
		private System.Windows.Forms.GroupBox gbInventoryReceived;
		private Infragistics.Win.UltraWinSchedule.UltraCalendarCombo dtpToDate;
		private Infragistics.Win.UltraWinSchedule.UltraCalendarCombo dtpFromDate;
		private Infragistics.Win.Misc.UltraLabel ultraLabel13;
		private Infragistics.Win.Misc.UltraLabel ultraLabel14;
		private System.Windows.Forms.GroupBox ultraGroupBox2;
		private Infragistics.Win.UltraWinEditors.UltraOptionSet optByName;
		private Infragistics.Win.UltraWinEditors.UltraTextEditor txtUserProduct;
		private Infragistics.Win.Misc.UltraLabel lblUser;
		private System.Windows.Forms.GroupBox groupBox2;
		private Infragistics.Win.Misc.UltraButton btnPrint;
		private Infragistics.Win.Misc.UltraButton btnView;
		private Infragistics.Win.Misc.UltraButton btnClose;
		private Infragistics.Win.Misc.UltraLabel lblTransactionType;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public frmRptSaleAnalysisByProduct()
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
			Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
			Infragistics.Win.UltraWinSchedule.CalendarCombo.DateButton dateButton1 = new Infragistics.Win.UltraWinSchedule.CalendarCombo.DateButton();
			Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
			Infragistics.Win.UltraWinSchedule.CalendarCombo.DateButton dateButton2 = new Infragistics.Win.UltraWinSchedule.CalendarCombo.DateButton();
			Infragistics.Win.UltraWinEditors.EditorButton editorButton1 = new Infragistics.Win.UltraWinEditors.EditorButton();
			Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(frmRptSaleAnalysisByProduct));
			Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
			Infragistics.Win.ValueListItem valueListItem1 = new Infragistics.Win.ValueListItem();
			Infragistics.Win.ValueListItem valueListItem2 = new Infragistics.Win.ValueListItem();
			Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
			Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
			Infragistics.Win.Appearance appearance7 = new Infragistics.Win.Appearance();
			Infragistics.Win.Appearance appearance8 = new Infragistics.Win.Appearance();
			this.gbInventoryReceived = new System.Windows.Forms.GroupBox();
			this.dtpToDate = new Infragistics.Win.UltraWinSchedule.UltraCalendarCombo();
			this.dtpFromDate = new Infragistics.Win.UltraWinSchedule.UltraCalendarCombo();
			this.ultraLabel13 = new Infragistics.Win.Misc.UltraLabel();
			this.ultraLabel14 = new Infragistics.Win.Misc.UltraLabel();
			this.ultraGroupBox2 = new System.Windows.Forms.GroupBox();
			this.lblUser = new Infragistics.Win.Misc.UltraLabel();
			this.txtUserProduct = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
			this.optByName = new Infragistics.Win.UltraWinEditors.UltraOptionSet();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.btnPrint = new Infragistics.Win.Misc.UltraButton();
			this.btnClose = new Infragistics.Win.Misc.UltraButton();
			this.btnView = new Infragistics.Win.Misc.UltraButton();
			this.lblTransactionType = new Infragistics.Win.Misc.UltraLabel();
			this.gbInventoryReceived.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.dtpToDate)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.dtpFromDate)).BeginInit();
			this.ultraGroupBox2.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.txtUserProduct)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.optByName)).BeginInit();
			this.groupBox2.SuspendLayout();
			this.SuspendLayout();
			// 
			// gbInventoryReceived
			// 
			this.gbInventoryReceived.Controls.Add(this.dtpToDate);
			this.gbInventoryReceived.Controls.Add(this.dtpFromDate);
			this.gbInventoryReceived.Controls.Add(this.ultraLabel13);
			this.gbInventoryReceived.Controls.Add(this.ultraLabel14);
			this.gbInventoryReceived.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.gbInventoryReceived.Location = new System.Drawing.Point(14, 134);
			this.gbInventoryReceived.Name = "gbInventoryReceived";
			this.gbInventoryReceived.Size = new System.Drawing.Size(424, 86);
			this.gbInventoryReceived.TabIndex = 0;
			this.gbInventoryReceived.TabStop = false;
			this.gbInventoryReceived.Text = "Sale Analysis";
			// 
			// dtpToDate
			// 
			this.dtpToDate.AllowNull = false;
			appearance1.FontData.BoldAsString = "False";
			appearance1.FontData.ItalicAsString = "False";
			appearance1.FontData.StrikeoutAsString = "False";
			appearance1.FontData.UnderlineAsString = "False";
			appearance1.ForeColor = System.Drawing.Color.Black;
			appearance1.ForeColorDisabled = System.Drawing.Color.Black;
			this.dtpToDate.Appearance = appearance1;
			this.dtpToDate.BackColor = System.Drawing.SystemColors.Window;
			this.dtpToDate.ButtonStyle = Infragistics.Win.UIElementButtonStyle.PopupSoft;
			dateButton1.Caption = "Today";
			this.dtpToDate.DateButtons.Add(dateButton1);
			this.dtpToDate.FlatMode = true;
			this.dtpToDate.Location = new System.Drawing.Point(284, 52);
			this.dtpToDate.Name = "dtpToDate";
			this.dtpToDate.NonAutoSizeHeight = 10;
			this.dtpToDate.Size = new System.Drawing.Size(123, 21);
			this.dtpToDate.TabIndex = 2;
			this.dtpToDate.Value = new System.DateTime(2004, 5, 25, 0, 0, 0, 0);
			// 
			// dtpFromDate
			// 
			this.dtpFromDate.AllowNull = false;
			appearance2.FontData.BoldAsString = "False";
			appearance2.FontData.ItalicAsString = "False";
			appearance2.FontData.StrikeoutAsString = "False";
			appearance2.FontData.UnderlineAsString = "False";
			appearance2.ForeColor = System.Drawing.Color.Black;
			appearance2.ForeColorDisabled = System.Drawing.Color.Black;
			this.dtpFromDate.Appearance = appearance2;
			this.dtpFromDate.BackColor = System.Drawing.SystemColors.Window;
			this.dtpFromDate.ButtonStyle = Infragistics.Win.UIElementButtonStyle.PopupSoft;
			dateButton2.Caption = "Today";
			this.dtpFromDate.DateButtons.Add(dateButton2);
			this.dtpFromDate.FlatMode = true;
			this.dtpFromDate.Location = new System.Drawing.Point(284, 24);
			this.dtpFromDate.Name = "dtpFromDate";
			this.dtpFromDate.NonAutoSizeHeight = 10;
			this.dtpFromDate.Size = new System.Drawing.Size(123, 21);
			this.dtpFromDate.TabIndex = 1;
			this.dtpFromDate.Value = new System.DateTime(2004, 5, 25, 0, 0, 0, 0);
			// 
			// ultraLabel13
			// 
			this.ultraLabel13.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.ultraLabel13.Location = new System.Drawing.Point(180, 55);
			this.ultraLabel13.Name = "ultraLabel13";
			this.ultraLabel13.Size = new System.Drawing.Size(72, 15);
			this.ultraLabel13.TabIndex = 2;
			this.ultraLabel13.Text = "To Date";
			// 
			// ultraLabel14
			// 
			this.ultraLabel14.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.ultraLabel14.Location = new System.Drawing.Point(180, 27);
			this.ultraLabel14.Name = "ultraLabel14";
			this.ultraLabel14.Size = new System.Drawing.Size(72, 15);
			this.ultraLabel14.TabIndex = 1;
			this.ultraLabel14.Text = "From Date";
			// 
			// ultraGroupBox2
			// 
			this.ultraGroupBox2.Controls.Add(this.lblUser);
			this.ultraGroupBox2.Controls.Add(this.txtUserProduct);
			this.ultraGroupBox2.Controls.Add(this.optByName);
			this.ultraGroupBox2.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.ultraGroupBox2.Location = new System.Drawing.Point(14, 52);
			this.ultraGroupBox2.Name = "ultraGroupBox2";
			this.ultraGroupBox2.Size = new System.Drawing.Size(424, 71);
			this.ultraGroupBox2.TabIndex = 8;
			this.ultraGroupBox2.TabStop = false;
			this.ultraGroupBox2.Text = "Sale Analysis";
			// 
			// lblUser
			// 
			this.lblUser.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.lblUser.Location = new System.Drawing.Point(180, 29);
			this.lblUser.Name = "lblUser";
			this.lblUser.Size = new System.Drawing.Size(92, 20);
			this.lblUser.TabIndex = 5;
			this.lblUser.Text = "User Id";
			// 
			// txtUserProduct
			// 
			this.txtUserProduct.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
			appearance3.Image = ((object)(resources.GetObject("appearance3.Image")));
			editorButton1.Appearance = appearance3;
			editorButton1.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Office2003ToolbarButton;
			editorButton1.Text = "";
			this.txtUserProduct.ButtonsRight.Add(editorButton1);
			this.txtUserProduct.Location = new System.Drawing.Point(284, 28);
			this.txtUserProduct.Name = "txtUserProduct";
			this.txtUserProduct.Size = new System.Drawing.Size(123, 20);
			this.txtUserProduct.TabIndex = 4;
			this.txtUserProduct.EditorButtonClick += new Infragistics.Win.UltraWinEditors.EditorButtonEventHandler(this.txtUserProduct_EditorButtonClick);
			// 
			// optByName
			// 
			this.optByName.BorderStyle = Infragistics.Win.UIElementBorderStyle.None;
			this.optByName.CheckedIndex = 0;
			this.optByName.FlatMode = true;
			appearance4.FontData.BoldAsString = "False";
			this.optByName.ItemAppearance = appearance4;
			this.optByName.ItemOrigin = new System.Drawing.Point(0, 2);
			valueListItem1.DataValue = 1;
			valueListItem1.DisplayText = "By User";
			valueListItem2.DataValue = 2;
			valueListItem2.DisplayText = "By Product";
			this.optByName.Items.Add(valueListItem1);
			this.optByName.Items.Add(valueListItem2);
			this.optByName.ItemSpacingHorizontal = 5;
			this.optByName.Location = new System.Drawing.Point(10, 29);
			this.optByName.Name = "optByName";
			this.optByName.Size = new System.Drawing.Size(153, 20);
			this.optByName.TabIndex = 2;
			this.optByName.Text = "By User";
			this.optByName.ValueChanged += new System.EventHandler(this.optByName_ValueChanged);
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this.btnPrint);
			this.groupBox2.Controls.Add(this.btnClose);
			this.groupBox2.Controls.Add(this.btnView);
			this.groupBox2.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.groupBox2.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.groupBox2.Location = new System.Drawing.Point(14, 228);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(424, 57);
			this.groupBox2.TabIndex = 30;
			this.groupBox2.TabStop = false;
			// 
			// btnPrint
			// 
			appearance5.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(52)), ((System.Byte)(62)), ((System.Byte)(176)));
			appearance5.BackColor2 = System.Drawing.Color.FromArgb(((System.Byte)(65)), ((System.Byte)(129)), ((System.Byte)(247)));
			appearance5.FontData.BoldAsString = "True";
			appearance5.ForeColor = System.Drawing.Color.White;
			appearance5.Image = ((object)(resources.GetObject("appearance5.Image")));
			this.btnPrint.Appearance = appearance5;
			this.btnPrint.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
			this.btnPrint.Location = new System.Drawing.Point(138, 19);
			this.btnPrint.Name = "btnPrint";
			this.btnPrint.Size = new System.Drawing.Size(85, 26);
			this.btnPrint.SupportThemes = false;
			this.btnPrint.TabIndex = 7;
			this.btnPrint.Text = "&Print";
			this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click_1);
			// 
			// btnClose
			// 
			appearance6.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(52)), ((System.Byte)(62)), ((System.Byte)(176)));
			appearance6.BackColor2 = System.Drawing.Color.FromArgb(((System.Byte)(65)), ((System.Byte)(129)), ((System.Byte)(247)));
			appearance6.FontData.BoldAsString = "True";
			appearance6.ForeColor = System.Drawing.Color.White;
			appearance6.Image = ((object)(resources.GetObject("appearance6.Image")));
			this.btnClose.Appearance = appearance6;
			this.btnClose.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
			this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnClose.Location = new System.Drawing.Point(322, 20);
			this.btnClose.Name = "btnClose";
			this.btnClose.Size = new System.Drawing.Size(85, 26);
			this.btnClose.SupportThemes = false;
			this.btnClose.TabIndex = 6;
			this.btnClose.Text = "&Close";
			this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
			// 
			// btnView
			// 
			appearance7.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(52)), ((System.Byte)(62)), ((System.Byte)(176)));
			appearance7.BackColor2 = System.Drawing.Color.FromArgb(((System.Byte)(65)), ((System.Byte)(129)), ((System.Byte)(247)));
			appearance7.FontData.BoldAsString = "True";
			appearance7.ForeColor = System.Drawing.Color.White;
			appearance7.Image = ((object)(resources.GetObject("appearance7.Image")));
			this.btnView.Appearance = appearance7;
			this.btnView.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
			this.btnView.Location = new System.Drawing.Point(230, 20);
			this.btnView.Name = "btnView";
			this.btnView.Size = new System.Drawing.Size(85, 26);
			this.btnView.SupportThemes = false;
			this.btnView.TabIndex = 5;
			this.btnView.Text = "&View";
			this.btnView.Click += new System.EventHandler(this.btnView_Click);
			// 
			// lblTransactionType
			// 
			appearance8.ForeColor = System.Drawing.Color.White;
			appearance8.ForeColorDisabled = System.Drawing.Color.White;
			appearance8.TextHAlign = Infragistics.Win.HAlign.Center;
			this.lblTransactionType.Appearance = appearance8;
			this.lblTransactionType.BackColor = System.Drawing.Color.Transparent;
			this.lblTransactionType.Font = new System.Drawing.Font("Arial", 18F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.lblTransactionType.Location = new System.Drawing.Point(28, 14);
			this.lblTransactionType.Name = "lblTransactionType";
			this.lblTransactionType.Size = new System.Drawing.Size(385, 30);
			this.lblTransactionType.TabIndex = 31;
			this.lblTransactionType.Text = "Sales Analysis";
			// 
			// frmRptSaleAnalysisByProduct
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
			this.CancelButton = this.btnClose;
			this.ClientSize = new System.Drawing.Size(455, 300);
			this.Controls.Add(this.lblTransactionType);
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.ultraGroupBox2);
			this.Controls.Add(this.gbInventoryReceived);
			this.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.KeyPreview = true;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "frmRptSaleAnalysisByProduct";
			this.Text = "Sale Analysis";
			this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmRptInventoryReceived_KeyDown);
			this.Load += new System.EventHandler(this.frmInventoryReports_Load);
			this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.frmRptSaleAnalysisByProduct_KeyUp);
			this.gbInventoryReceived.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.dtpToDate)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.dtpFromDate)).EndInit();
			this.ultraGroupBox2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.txtUserProduct)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.optByName)).EndInit();
			this.groupBox2.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		private void frmInventoryReports_Load(object sender, System.EventArgs e)
		{
			clsUIHelper.setColorSchecme(this);
			dtpFromDate.Value = DateTime.Now;
			dtpToDate.Value = DateTime.Now;
		}

		private void btnPreview_Click(object sender, System.EventArgs e)
		{
			Preview(false);
		}

		private void btnPrint_Click(object sender, System.EventArgs e)
		{
			Preview(true);
		}

		private void Preview(bool PrintId)
		{
			try
			{
				rptSaleAnalysis oRpt= new rptSaleAnalysis();
//				rptTopSellingProductsPrice oRpt2= new rptTopSellingProductsPrice();

				string sSQL = "";

				clsReports.setCRTextObjectText("txtFromDate",dtpFromDate.Text,oRpt);
				clsReports.setCRTextObjectText("txtToDate",dtpToDate.Text,oRpt);

				if (optByName.CheckedIndex == 0)
				sSQL = "SELECT " +
							" cast(month(transdate) as varchar)+'-'+cast(year(transdate) as varchar) as TransactionDate " +
							" , SUM(ExtendedPrice + dtl.TaxAmount - dtl.Discount) Price " +
						" FROM " +
							" POSTransactionDetail As dtl " +
							" , POSTransaction as Trans " +
						" WHERE " +
							" Trans.TransID = dtl.TransID";
				else
					sSQL = " SELECT  " +
								" cast(month(transdate) as varchar)+'-'+cast(year(transdate) as varchar) as TransactionDate  " +
								" , SUM(ExtendedPrice + dtl.TaxAmount - dtl.Discount) Price " +
							" FROM " +
								" POSTransactionDetail As dtl " +
								" , POSTransaction as Trans " +
								" , Item as Itm " +
							" WHERE " +
								" Trans.TransID = dtl.TransID " +
								" AND Itm.ItemId = dtl.ItemId " ;
				

				sSQL = sSQL + buildCriteria();

//				if (optByName.CheckedIndex == 0)
				clsReports.Preview(PrintId,sSQL,oRpt);
//				else
//					clsReports.Preview(PrintId,sSQL,oRpt2);
			}
			catch(Exception exp)
			{
				clsUIHelper.ShowErrorMsg(exp.Message);
			}
		}

		private string buildCriteria()
		{
			string sCriteria = "";
			
			if (dtpFromDate.Value.ToString()!="")
				sCriteria = sCriteria + " AND Convert(smalldatetime,Convert(Varchar,TransDate,107)) >= '" + dtpFromDate.Text + "'";
			if (dtpToDate.Value.ToString()!="")
				sCriteria = sCriteria + " AND Convert(smalldatetime,Convert(Varchar,TransDate,107)) <= '" + dtpToDate.Text + "'";
			if (optByName.CheckedIndex ==1)
			{
				if (txtUserProduct.Text!="")
					sCriteria = sCriteria + " AND Itm.ProductCode = '" + txtUserProduct.Text + "'";
			}
			else
			{
				if (txtUserProduct.Text.ToString()!="")
					sCriteria = sCriteria + " AND Trans.UserId = '" + txtUserProduct.Text + "'";
			}
			
			sCriteria = sCriteria + " GROUP BY  " +
									" cast(month(transdate) as varchar)+'-'+cast(year(transdate) as varchar) ";

			return sCriteria;
		}

		private void btnClose_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}

		private void dtpToDate_BeforeDropDown(object sender, System.ComponentModel.CancelEventArgs e)
		{
		
		}

		private void frmRptInventoryReceived_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
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

		private void gbInventoryReceived_Enter(object sender, System.EventArgs e)
		{
			dtpFromDate.Focus();
		}

		private void optByName_ValueChanged(object sender, System.EventArgs e)
		{
			lblUser.Text = (optByName.CheckedIndex==0? "User Id":"SKU Code");
		}

		private void btnView_Click(object sender, System.EventArgs e)
		{
			Preview(false);
		}

		private void btnPrint_Click_1(object sender, System.EventArgs e)
		{
			Preview(true);
		}

		private void txtUserProduct_EditorButtonClick(object sender, Infragistics.Win.UltraWinEditors.EditorButtonEventArgs e)
		{
			if (optByName.CheckedIndex == 0 ) 
			{
                //frmSearch oSearch = new frmSearch(clsPOSDBConstants.Users_tbl);
                frmSearchMain oSearch = new frmSearchMain(true);    //20-Dec-2017 JY Added new reference
                oSearch.SearchTable = clsPOSDBConstants.Users_tbl;  //20-Dec-2017 JY Added 
                oSearch.ShowDialog();
				if (oSearch.IsCanceled) return;
				txtUserProduct.Text = oSearch.SelectedRowID();
			}
			else if (optByName.CheckedIndex == 1 ) 
			{
                //frmSearch oSearch = new frmSearch(clsPOSDBConstants.Item_tbl);
                frmSearchMain oSearch = new frmSearchMain(true);    //20-Dec-2017 JY Added new reference
                oSearch.SearchTable = clsPOSDBConstants.Item_tbl;  //20-Dec-2017 JY Added 
                oSearch.ShowDialog();
				if (oSearch.IsCanceled) return;
				txtUserProduct.Text = oSearch.SelectedRowID();
			}

		}

		private void frmRptSaleAnalysisByProduct_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			try
			{
				if (e.KeyData==System.Windows.Forms.Keys.F4)
				{
					if ( txtUserProduct.ContainsFocus)
						txtUserProduct_EditorButtonClick(null,null);
				}
			}
			catch(Exception exp)
			{
				clsUIHelper.ShowErrorMsg(exp.Message);
			}
		}

	}
}
