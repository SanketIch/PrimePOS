using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using POS_Core.CommonData.Rows;
using POS_Core.CommonData;
using POS_Core.BusinessRules;
using POS_Core.ErrorLogging;
using Infragistics.Win.UltraWinEditors;
//using POS.UI;
using POS_Core_UI.Reports.Reports;
using System.IO;
using System.Drawing.Imaging;
using System.Data;
using NLog;
using Resources;

namespace POS_Core_UI.Reports.ReportsUI
{
	/// <summary>
	/// Summary description for frmRptSalesByCustomer.
	/// </summary>
	public class frmRptSalesByItemMonitoring : System.Windows.Forms.Form
	{
		private System.Windows.Forms.GroupBox groupBox1;
		private Infragistics.Win.Misc.UltraLabel ultraLabel12;
        private Infragistics.Win.Misc.UltraLabel lblTransactionType;
		private Infragistics.Win.Misc.UltraLabel ultraLabel20;
		private Infragistics.Win.Misc.UltraLabel ultraLabel19;
		private Infragistics.Win.UltraWinSchedule.UltraCalendarCombo dtpSaleEndDate;
		private Infragistics.Win.UltraWinSchedule.UltraCalendarCombo dtpSaleStartDate;
		private System.Windows.Forms.GroupBox groupBox2;
		private Infragistics.Win.Misc.UltraButton btnPrint;
		private Infragistics.Win.Misc.UltraButton btnView;
		private Infragistics.Win.Misc.UltraButton btnClose;
        private Infragistics.Win.Misc.UltraLabel lblCustomerName;
        public UltraTextEditor txtCustomer;
        private RadioButton optTransReturn;
        private RadioButton optTransSales;
        private RadioButton optTransAll;
        private Infragistics.Win.Misc.UltraLabel ultraLabel1;
        private UltraComboEditor cboMonitoringCategory;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
        private static ILogger logger = LogManager.GetCurrentClassLogger();

        public frmRptSalesByItemMonitoring()
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
            Infragistics.Win.UltraWinEditors.EditorButton editorButton1 = new Infragistics.Win.UltraWinEditors.EditorButton();
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinSchedule.CalendarCombo.DateButton dateButton1 = new Infragistics.Win.UltraWinSchedule.CalendarCombo.DateButton();
            Infragistics.Win.UltraWinSchedule.CalendarCombo.DateButton dateButton2 = new Infragistics.Win.UltraWinSchedule.CalendarCombo.DateButton();
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance7 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance8 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance9 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance10 = new Infragistics.Win.Appearance();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cboMonitoringCategory = new Infragistics.Win.UltraWinEditors.UltraComboEditor();
            this.optTransReturn = new System.Windows.Forms.RadioButton();
            this.optTransSales = new System.Windows.Forms.RadioButton();
            this.optTransAll = new System.Windows.Forms.RadioButton();
            this.lblCustomerName = new Infragistics.Win.Misc.UltraLabel();
            this.txtCustomer = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.ultraLabel20 = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabel19 = new Infragistics.Win.Misc.UltraLabel();
            this.dtpSaleEndDate = new Infragistics.Win.UltraWinSchedule.UltraCalendarCombo();
            this.dtpSaleStartDate = new Infragistics.Win.UltraWinSchedule.UltraCalendarCombo();
            this.ultraLabel12 = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabel1 = new Infragistics.Win.Misc.UltraLabel();
            this.lblTransactionType = new Infragistics.Win.Misc.UltraLabel();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnPrint = new Infragistics.Win.Misc.UltraButton();
            this.btnClose = new Infragistics.Win.Misc.UltraButton();
            this.btnView = new Infragistics.Win.Misc.UltraButton();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cboMonitoringCategory)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCustomer)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtpSaleEndDate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtpSaleStartDate)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cboMonitoringCategory);
            this.groupBox1.Controls.Add(this.optTransReturn);
            this.groupBox1.Controls.Add(this.optTransSales);
            this.groupBox1.Controls.Add(this.optTransAll);
            this.groupBox1.Controls.Add(this.lblCustomerName);
            this.groupBox1.Controls.Add(this.txtCustomer);
            this.groupBox1.Controls.Add(this.ultraLabel20);
            this.groupBox1.Controls.Add(this.ultraLabel19);
            this.groupBox1.Controls.Add(this.dtpSaleEndDate);
            this.groupBox1.Controls.Add(this.dtpSaleStartDate);
            this.groupBox1.Controls.Add(this.ultraLabel12);
            this.groupBox1.Controls.Add(this.ultraLabel1);
            this.groupBox1.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.groupBox1.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(16, 84);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(463, 210);
            this.groupBox1.TabIndex = 27;
            this.groupBox1.TabStop = false;
            // 
            // cboMonitoringCategory
            // 
            this.cboMonitoringCategory.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.cboMonitoringCategory.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboMonitoringCategory.LimitToList = true;
            this.cboMonitoringCategory.Location = new System.Drawing.Point(156, 124);
            this.cboMonitoringCategory.Name = "cboMonitoringCategory";
            this.cboMonitoringCategory.Size = new System.Drawing.Size(268, 23);
            this.cboMonitoringCategory.TabIndex = 32;
            this.cboMonitoringCategory.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.cboMonitoringCategory.KeyDown += new System.Windows.Forms.KeyEventHandler(this.cboMonitoringCategory_KeyDown);
            this.cboMonitoringCategory.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.cboMonitoringCategory_KeyPress);
            // 
            // optTransReturn
            // 
            this.optTransReturn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.optTransReturn.ForeColor = System.Drawing.Color.White;
            this.optTransReturn.Location = new System.Drawing.Point(310, 156);
            this.optTransReturn.Name = "optTransReturn";
            this.optTransReturn.Size = new System.Drawing.Size(122, 26);
            this.optTransReturn.TabIndex = 31;
            this.optTransReturn.Text = "Returns Only";
            // 
            // optTransSales
            // 
            this.optTransSales.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.optTransSales.ForeColor = System.Drawing.Color.White;
            this.optTransSales.Location = new System.Drawing.Point(204, 156);
            this.optTransSales.Name = "optTransSales";
            this.optTransSales.Size = new System.Drawing.Size(122, 26);
            this.optTransSales.TabIndex = 30;
            this.optTransSales.Text = "Sales Only";
            // 
            // optTransAll
            // 
            this.optTransAll.Checked = true;
            this.optTransAll.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.optTransAll.ForeColor = System.Drawing.Color.White;
            this.optTransAll.Location = new System.Drawing.Point(156, 156);
            this.optTransAll.Name = "optTransAll";
            this.optTransAll.Size = new System.Drawing.Size(122, 26);
            this.optTransAll.TabIndex = 29;
            this.optTransAll.TabStop = true;
            this.optTransAll.Text = "All";
            // 
            // lblCustomerName
            // 
            appearance1.ForeColor = System.Drawing.Color.Black;
            this.lblCustomerName.Appearance = appearance1;
            this.lblCustomerName.BackColorInternal = System.Drawing.Color.Transparent;
            this.lblCustomerName.Location = new System.Drawing.Point(211, 94);
            this.lblCustomerName.Name = "lblCustomerName";
            this.lblCustomerName.Size = new System.Drawing.Size(227, 18);
            this.lblCustomerName.TabIndex = 28;
            this.lblCustomerName.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // txtCustomer
            // 
            this.txtCustomer.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance2.BackColor = System.Drawing.Color.White;
            appearance2.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(231)))), ((int)(((byte)(230)))));
            appearance2.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance2.BorderAlpha = Infragistics.Win.Alpha.Transparent;
            appearance2.ImageAlpha = Infragistics.Win.Alpha.Opaque;
            appearance2.ImageBackgroundAlpha = Infragistics.Win.Alpha.Opaque;
            appearance2.ImageBackgroundStyle = Infragistics.Win.ImageBackgroundStyle.Centered;
            editorButton1.Appearance = appearance2;
            editorButton1.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            editorButton1.Text = "";
            editorButton1.Width = 20;
            this.txtCustomer.ButtonsRight.Add(editorButton1);
            this.txtCustomer.Location = new System.Drawing.Point(156, 92);
            this.txtCustomer.Name = "txtCustomer";
            this.txtCustomer.Size = new System.Drawing.Size(52, 23);
            this.txtCustomer.TabIndex = 27;
            this.txtCustomer.TabStop = false;
            this.txtCustomer.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.txtCustomer.EditorButtonClick += new Infragistics.Win.UltraWinEditors.EditorButtonEventHandler(this.txtCustomer_EditorButtonClick);
            this.txtCustomer.Leave += new System.EventHandler(this.txtCustomer_Leave);
            // 
            // ultraLabel20
            // 
            appearance3.ForeColor = System.Drawing.Color.White;
            this.ultraLabel20.Appearance = appearance3;
            this.ultraLabel20.Location = new System.Drawing.Point(12, 38);
            this.ultraLabel20.Name = "ultraLabel20";
            this.ultraLabel20.Size = new System.Drawing.Size(116, 14);
            this.ultraLabel20.TabIndex = 20;
            this.ultraLabel20.Text = "Start Date";
            // 
            // ultraLabel19
            // 
            appearance4.ForeColor = System.Drawing.Color.White;
            this.ultraLabel19.Appearance = appearance4;
            this.ultraLabel19.Location = new System.Drawing.Point(12, 67);
            this.ultraLabel19.Name = "ultraLabel19";
            this.ultraLabel19.Size = new System.Drawing.Size(106, 14);
            this.ultraLabel19.TabIndex = 22;
            this.ultraLabel19.Text = "End Date";
            // 
            // dtpSaleEndDate
            // 
            this.dtpSaleEndDate.AllowNull = false;
            this.dtpSaleEndDate.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Office2003ToolbarButton;
            this.dtpSaleEndDate.DateButtons.Add(dateButton1);
            this.dtpSaleEndDate.Location = new System.Drawing.Point(156, 63);
            this.dtpSaleEndDate.Name = "dtpSaleEndDate";
            this.dtpSaleEndDate.NonAutoSizeHeight = 10;
            this.dtpSaleEndDate.Size = new System.Drawing.Size(123, 22);
            this.dtpSaleEndDate.TabIndex = 1;
            this.dtpSaleEndDate.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.dtpSaleEndDate.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.dtpSaleEndDate.Value = new System.DateTime(2004, 5, 25, 0, 0, 0, 0);
            // 
            // dtpSaleStartDate
            // 
            this.dtpSaleStartDate.AllowNull = false;
            this.dtpSaleStartDate.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Office2003ToolbarButton;
            this.dtpSaleStartDate.DateButtons.Add(dateButton2);
            this.dtpSaleStartDate.Location = new System.Drawing.Point(156, 34);
            this.dtpSaleStartDate.Name = "dtpSaleStartDate";
            this.dtpSaleStartDate.NonAutoSizeHeight = 10;
            this.dtpSaleStartDate.Size = new System.Drawing.Size(123, 22);
            this.dtpSaleStartDate.TabIndex = 0;
            this.dtpSaleStartDate.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.dtpSaleStartDate.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.dtpSaleStartDate.Value = new System.DateTime(2004, 5, 25, 0, 0, 0, 0);
            // 
            // ultraLabel12
            // 
            appearance5.ForeColor = System.Drawing.Color.White;
            this.ultraLabel12.Appearance = appearance5;
            this.ultraLabel12.AutoSize = true;
            this.ultraLabel12.Location = new System.Drawing.Point(12, 94);
            this.ultraLabel12.Name = "ultraLabel12";
            this.ultraLabel12.Size = new System.Drawing.Size(111, 18);
            this.ultraLabel12.TabIndex = 6;
            this.ultraLabel12.Text = "Customer Code";
            // 
            // ultraLabel1
            // 
            appearance6.ForeColor = System.Drawing.Color.White;
            this.ultraLabel1.Appearance = appearance6;
            this.ultraLabel1.AutoSize = true;
            this.ultraLabel1.Location = new System.Drawing.Point(12, 126);
            this.ultraLabel1.Name = "ultraLabel1";
            this.ultraLabel1.Size = new System.Drawing.Size(145, 18);
            this.ultraLabel1.TabIndex = 33;
            this.ultraLabel1.Text = "Monitoring Category";
            // 
            // lblTransactionType
            // 
            appearance7.ForeColor = System.Drawing.Color.White;
            appearance7.ForeColorDisabled = System.Drawing.Color.White;
            appearance7.TextHAlignAsString = "Center";
            this.lblTransactionType.Appearance = appearance7;
            this.lblTransactionType.BackColorInternal = System.Drawing.Color.Transparent;
            this.lblTransactionType.Font = new System.Drawing.Font("Arial", 18F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTransactionType.Location = new System.Drawing.Point(16, 16);
            this.lblTransactionType.Name = "lblTransactionType";
            this.lblTransactionType.Size = new System.Drawing.Size(424, 58);
            this.lblTransactionType.TabIndex = 26;
            this.lblTransactionType.Text = "Sales By Item Monitoring Category";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnPrint);
            this.groupBox2.Controls.Add(this.btnClose);
            this.groupBox2.Controls.Add(this.btnView);
            this.groupBox2.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.groupBox2.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.Location = new System.Drawing.Point(17, 300);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(462, 57);
            this.groupBox2.TabIndex = 31;
            this.groupBox2.TabStop = false;
            // 
            // btnPrint
            // 
            appearance8.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(62)))), ((int)(((byte)(176)))));
            appearance8.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance8.FontData.BoldAsString = "True";
            appearance8.ForeColor = System.Drawing.Color.White;
            this.btnPrint.Appearance = appearance8;
            this.btnPrint.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            this.btnPrint.Location = new System.Drawing.Point(185, 20);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(85, 26);
            this.btnPrint.TabIndex = 6;
            this.btnPrint.Text = "&Print";
            this.btnPrint.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            // 
            // btnClose
            // 
            appearance9.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(62)))), ((int)(((byte)(176)))));
            appearance9.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance9.FontData.BoldAsString = "True";
            appearance9.ForeColor = System.Drawing.Color.White;
            this.btnClose.Appearance = appearance9;
            this.btnClose.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(369, 20);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(85, 26);
            this.btnClose.TabIndex = 7;
            this.btnClose.Text = "&Close";
            this.btnClose.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnView
            // 
            appearance10.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(62)))), ((int)(((byte)(176)))));
            appearance10.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance10.FontData.BoldAsString = "True";
            appearance10.ForeColor = System.Drawing.Color.White;
            this.btnView.Appearance = appearance10;
            this.btnView.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            this.btnView.Location = new System.Drawing.Point(277, 20);
            this.btnView.Name = "btnView";
            this.btnView.Size = new System.Drawing.Size(85, 26);
            this.btnView.TabIndex = 5;
            this.btnView.Text = "&View";
            this.btnView.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnView.Click += new System.EventHandler(this.btnView_Click);
            // 
            // frmRptSalesByItemMonitoring
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(108)))), ((int)(((byte)(172)))));
            this.ClientSize = new System.Drawing.Size(491, 369);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.lblTransactionType);
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.Name = "frmRptSalesByItemMonitoring";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Sales By Item Monitoring Category";
            this.Load += new System.EventHandler(this.frmRptSalesByCustomer_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmRptSalesByCustomer_KeyDown);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cboMonitoringCategory)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCustomer)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtpSaleEndDate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtpSaleStartDate)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);

		}
		#endregion

		private void frmRptSalesByCustomer_Load(object sender, System.EventArgs e)
		{
			this.dtpSaleStartDate.Enter += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
			this.dtpSaleStartDate.Leave += new System.EventHandler(clsUIHelper.AfterExitEditMode);

			this.dtpSaleEndDate.Enter += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
			this.dtpSaleEndDate.Leave += new System.EventHandler(clsUIHelper.AfterExitEditMode);

			this.txtCustomer.AfterEnterEditMode += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
			this.txtCustomer.AfterExitEditMode += new System.EventHandler(clsUIHelper.AfterExitEditMode);

			this.Left=(frmMain.getInstance().Width-frmMain.getInstance().ultraExplorerBar1.Width-this.Width)/2;
			this.Top=(frmMain.getInstance().Height-this.Height)/2;
			
			clsUIHelper.setColorSchecme(this);
			this.dtpSaleEndDate.Value=DateTime.Now;
			this.dtpSaleStartDate.Value=DateTime.Now;
            populateMonitoringCategory();

		}

        private void populateMonitoringCategory()
        {
            try
            {
                ItemMonitorCategory oItemMonCat=new ItemMonitorCategory();
                ItemMonitorCategoryData oData= oItemMonCat.PopulateList();
                
                this.cboMonitoringCategory.Items.Add("All", "All");
                foreach(ItemMonitorCategoryRow oRow in oData.ItemMonitorCategory.Rows)
                {   
                    this.cboMonitoringCategory.Items.Add(oRow.ID.ToString(), oRow.Description);
                }
                this.cboMonitoringCategory.SelectedIndex = 0;
            }
            catch (Exception) { }
        }

		private void btnView_Click(object sender, System.EventArgs e)
		{
            PreviewReport(false);
        }

        private void btnClose_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}

		private void frmRptSalesByCustomer_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			try
			{
				if(e.KeyData==System.Windows.Forms.Keys.Enter)
				{
					this.SelectNextControl(this.ActiveControl,true,true,true,true);
				}
				else if (e.KeyData==Keys.Escape)
					this.Close();
			}
			catch(Exception exp)
			{
				clsUIHelper.ShowErrorMsg( exp.Message);
			}
		}

        private void PreviewReport(bool blnPrint)
        {
            #region PRIMEPOS-3082 05-Apr-2022 JY Added
            try
            {
                string strSQL = "SELECT COUNT(ID) FROM (SELECT ROW_NUMBER() OVER(PARTITION BY TransDetailID, ItemMoncatID ORDER BY TransDetailID, ISNULL(pseTrxId, 0) DESC, SentToNplex) AS rNum, ID FROM ItemMonitorTransDetail) x WHERE rnum = 2";
                DataTable dt = DataHelper.ExecuteDataTable(strSQL);
                if (dt != null && dt.Rows.Count > 0)
                {
                    if (POS_Core.Resources.Configuration.convertNullToInt(dt.Rows[0][0]) > 0)
                    {
                        strSQL = "WITH CTE(rNum, ID)" +
                                    " AS" +
                                    " (SELECT ROW_NUMBER() OVER(PARTITION BY TransDetailID, ItemMoncatID ORDER BY TransDetailID, ISNULL(pseTrxId, 0) DESC, SentToNplex) AS rnum, ID FROM ItemMonitorTransDetail)" +
                                    " DELETE FROM CTE WHERE rNum = 2";
                        DataHelper.ExecuteNonQuery(strSQL);
                    }
                }
            }
            catch(Exception Ex)
            {
                logger.Fatal(Ex, "PreviewReport(bool blnPrint) - Delete duplicates");
            }
            #endregion
            try
            {
                if (Convert.ToDateTime(this.dtpSaleEndDate.Value.ToString()).Date < Convert.ToDateTime(this.dtpSaleStartDate.Value.ToString()).Date)
                {
                    throw (new Exception("End date cannot be less than Start date."));
                }

                this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
                rptSaleByItemMonitoring oRpt = new rptSaleByItemMonitoring();

                String strQuery = string.Empty, strTotals = string.Empty, strWhereClause = string.Empty;
                //string WhereClause = string.Empty;    Sprint-23 - PRIMEPOS-2029 28-Apr-2016 JY Commented
                //string strSubQuery;
                //Modified by shitaljit to add signature and ID detail for Monitoring cat ID details.
                DataSet oDataSet = new DataSet();
                //strQuery = "Select IsNull(CustomerName,'') + ', '+ IsNull(FirstName,'') as [Customer], " +
                //    " IsNull(Address1,'') +' '+ IsNull(Address2,'') +' '+ IsNull(City,'') +' '+ IsNull(State,'') As Address, " +
                //    " PTH.TransID , TransDate as [Trans Date]  ,  " +
                //    " case TransType when 1 then 'Sale' when 2 then 'Return' when 3 then 'Receive on Account' end  as [Trans Type]   , ISNULL(PTS.POSTransId,0) as  POSTransId " +
                //    ",case CustomerIDType when 'DL' then 'Driver License(US or Canada)' when 'UP' then 'US Passport' when 'RC' then 'Alien Registration or Permanent Resident Card'" +
                //    " when 'FP' then 'Unexpired Foreign Passport with temporary I-551 Stamp' when 'EA' then 'Unexpired Employment Authorization Document'" +
                //    " when 'SI' then 'School ID with Picture' when 'VC' then 'Voter Registration Card' when 'MC' then 'US Military Card' " +
                //    " when 'TD' then 'Native American Tribal Documents' END as CustomerIDType, ISNULL(PTS.CustomerIDDetail,'') as CustomerIDDetail " +
                //    " , ps.stationname as [Station ID],PTD.ITEMID, Case I.ItemID When 'RX' Then PTD.ItemDescription Else I.Description End As ItemName " +
                //    " , PTD.Discount, PTD.Price, PTD.QTY, PTD.TaxAmount,PTD.ExtendedPrice, PTH.UserID as [User ID] " +
                //    " FROM POSTransaction as PTH LEFT OUTER JOIN  POSTransSignLog PTS  ON PTH.TransID= PTS.POSTransId , Customer as Cus, util_POSSet ps  , POSTransactionDetail PTD, Item I " +
                //    " Where ps.stationid=PTH.stationid and PTH.CustomerId = Cus.CustomerID And PTH.TransID=PTD.TransID And I.ItemID=PTD.ItemID " +
                //    " and convert(datetime,TransDate,109) between convert(datetime, cast('" + this.dtpSaleStartDate.Text + " 00:00:00 ' as datetime) ,113) and convert(datetime, cast('" + this.dtpSaleEndDate.Text + " 23:59:59' as datetime) ,113) ";

                //Sprint-23 - PRIMEPOS-2029 21-Apr-2016 JY corrected the sql as it should fetch the data w.r.t. transaction entry
                //PRIMEPOS-2629 16-Jan-2019 JY Added logic to split customer details in ID and DOB
                strQuery = "Select DISTINCT IsNull(CustomerName,'') + ', '+ IsNull(FirstName,'') as [Customer], " +
                    " IsNull(Address1,'') +' '+ IsNull(Address2,'') +' '+ IsNull(City,'') +' '+ IsNull(State,'') As Address, " +
                    " PTH.TransID , TransDate as [Trans Date]  ,  " +
                    " case TransType when 1 then 'Sale' when 2 then 'Return' when 3 then 'Receive on Account' end  as [Trans Type]   , ISNULL(PTS.POSTransId,0) as  POSTransId " +
                    ",case CustomerIDType when 'DL' then 'Driver License(US or Canada)' when 'UP' then 'US Passport' when 'RC' then 'Alien Registration or Permanent Resident Card'" +
                    " when 'FP' then 'Unexpired Foreign Passport with temporary I-551 Stamp' when 'EA' then 'Unexpired Employment Authorization Document'" +
                    " when 'SI' then 'School ID with Picture' when 'VC' then 'Voter Registration Card' when 'MC' then 'US Military Card' " +
                    " when 'TD' then 'Native American Tribal Documents' " +
                    " when 'STATE_ID' then 'Other state-issued ID' when 'ALIEN' then 'Alien Registration Card' END as CustomerIDType, " +
                    //" ISNULL(PTS.CustomerIDDetail,'') as CustomerIDDetail " +

                    " CASE WHEN ISNULL(PTS.CustomerIDDetail, '') <> '' THEN" +
                        " CASE WHEN CHARINDEX('^', PTS.CustomerIDDetail) <> 0 THEN" +
                            " CASE WHEN LEN(LTRIM(RTRIM(PTS.CustomerIDDetail))) > CHARINDEX('^', LTRIM(RTRIM(PTS.CustomerIDDetail))) THEN" +
                                " CASE WHEN LEN(PTS.CustomerIDDetail) - CHARINDEX('^', PTS.CustomerIDDetail) = 8 THEN" +
                                    " SubString(LTRIM(RTRIM(PTS.CustomerIDDetail)), 1, CHARINDEX('^', LTRIM(RTRIM(PTS.CustomerIDDetail))) - 1) +" +
                                    " ' - ' + SubString(PTS.CustomerIDDetail, CHARINDEX('^', PTS.CustomerIDDetail) + 1, LEN(PTS.CustomerIDDetail) - CHARINDEX('^', PTS.CustomerIDDetail) - 6) + '/' +" +
                                    " SubString(PTS.CustomerIDDetail, CHARINDEX('^', PTS.CustomerIDDetail) + 3, LEN(PTS.CustomerIDDetail) - CHARINDEX('^', PTS.CustomerIDDetail) - 6) + '/' +" +
                                    " SubString(PTS.CustomerIDDetail, CHARINDEX('^', PTS.CustomerIDDetail) + 5, LEN(PTS.CustomerIDDetail) - CHARINDEX('^', PTS.CustomerIDDetail))" +
                                " ELSE SubString(LTRIM(RTRIM(PTS.CustomerIDDetail)), 1, CHARINDEX('^', LTRIM(RTRIM(PTS.CustomerIDDetail))) - 1) +" +
                                    " ' - ' + SubString(PTS.CustomerIDDetail, CHARINDEX('^', PTS.CustomerIDDetail) + 1, LEN(PTS.CustomerIDDetail) - CHARINDEX('^', PTS.CustomerIDDetail)) END" +
                            " ELSE SubString(LTRIM(RTRIM(PTS.CustomerIDDetail)), 1, CHARINDEX('^', LTRIM(RTRIM(PTS.CustomerIDDetail))) - 1) END" +
                        " ELSE LTRIM(RTRIM(PTS.CustomerIDDetail)) END" +
                    " ELSE  '' END AS CustomerIDDetail" +

                    ", ps.stationname as [Station ID],PTD.ITEMID, Case I.ItemID When 'RX' Then PTD.ItemDescription Else I.Description End As ItemName " +
                    ", PTD.Discount, PTD.Price, PTD.QTY, PTD.TaxAmount,PTD.ExtendedPrice " +
                    ", CASE WHEN LTRIM(RTRIM(ISNULL(U.LNAME,''))) = '' OR LTRIM(RTRIM(ISNULL(U.FNAME,''))) = '' THEN '(' + U.UserID + ')' ELSE SUBSTRING(LTRIM(RTRIM(U.FNAME)), 1, 1) + SUBSTRING(LTRIM(RTRIM(U.LNAME)), 1, 1) + '(' + U.UserID + ')' END AS [User ID] " +
                    ", PTD.TransDetailID, IMC.Description AS MonitorCategory" +
                    //", IsNull(PTD.Qty,0) * (CASE WHEN IMT.UOM = 0 THEN 0 WHEN IMT.UOM = 1 THEN ISNULL(IMT.UnitsPerPackage,0)/1000 ELSE ISNULL(IMT.UnitsPerPackage,0) END) AS PSEQty "  +  //PRIMEPOS-3082 01-Apr-2022 JY Commented
                    ", CASE WHEN(IsNull(PTD.Qty, 0) * (CASE WHEN IMT.UOM = 0 THEN 0 ELSE ISNULL(IMT.UnitsPerPackage, 0) END)) <> 0 THEN (CAST(IsNull(PTD.Qty, 0) * (CASE WHEN IMT.UOM = 0 THEN 0 ELSE ISNULL(IMT.UnitsPerPackage, 0) END) AS VARCHAR(MAX)) + ' ' + CASE WHEN IMT.UOM = 1 THEN 'MG' WHEN IMT.UOM = 2 THEN 'G' WHEN IMT.UOM = 3 THEN 'ML' END) ELSE '0' END AS PSEQty" +    //PRIMEPOS-3082 01-Apr-2022 JY Added
                    ", IMT.SentToNplex, IMT.PostSaleInd, PTH.CustomerID " +
                    " , Z.TotalExtPrice, Z.TotalDiscount, Z.TotalTax, Z.TotalAmount " +
                    " FROM POSTransaction as PTH " +
                    " LEFT JOIN Users U ON U.UserID = PTH.UserID " +
                    " INNER JOIN Customer as Cus ON PTH.CustomerId = Cus.CustomerID " +
                    " INNER JOIN util_POSSet ps ON ps.stationid=PTH.stationid " +
                    " INNER JOIN POSTransactionDetail PTD ON PTH.TransID=PTD.TransID " +
                    " LEFT OUTER JOIN POSTransSignLog PTS ON PTS.POSTransId = PTD.TransID " +  //Sprint-25 24-Feb-2017 JY Added "PTS.POSTransId = PTD.TransID" and removed "PTS.TransDetailID = PTD.TransDetailID"
                    " INNER JOIN Item I ON I.ItemID=PTD.ItemID " +
                    " INNER JOIN ItemMonitorTransDetail IMT ON IMT.TransDetailID = PTD.TransDetailID " +
                    " INNER JOIN(SELECT ID, Description, UserID, UOM, DailyLimit, ThirtyDaysLimit, MaxExempt, VerificationBy, LimitPeriodDays, LimitPeriodQty, AgeLimit, IsAgeLimit, ePSE FROM ItemMonitorCategory " +
                                " UNION SELECT 0 as ID, 'NPLEx' AS Description, 'POS' AS UserID, 2 AS UOM, 0 AS DailyLimit, 0 AS ThirtyDaysLimit, 0 AS MaxExempt, 'B' AS VerificationBy, 0 AS LimitPeriodDays, 0 AS LimitPeriodQty, 0 AS AgeLimit, 0 AS IsAgeLimit, 1 AS ePSE " +
                                " ) IMC ON IMC.ID = IMT.ItemMonCatID ";
                //" INNER JOIN ItemMonitorCategory IMC ON IMC.ID = IMT.ItemMonCatID ";

                strTotals = "Select PTH.CustomerID, SUM(PTD.ExtendedPrice) AS TotalExtPrice, SUM(PTD.Discount) AS TotalDiscount, SUM(PTD.TaxAmount) AS TotalTax, (SUM(PTD.ExtendedPrice) - SUM(PTD.Discount) + SUM(PTD.TaxAmount)) AS TotalAmount " +
                            " FROM POSTransaction as PTH  " +
                            " INNER JOIN Customer Cus ON PTH.CustomerId = Cus.CustomerID " +
                            " INNER JOIN util_POSSet ps ON ps.stationid=PTH.stationid  " +
                            " INNER JOIN POSTransactionDetail PTD ON PTH.TransID=PTD.TransID " +
                            " LEFT OUTER JOIN POSTransSignLog PTS ON PTS.POSTransId = PTD.TransID ";  //Sprint-25 24-Feb-2017 JY Added "PTS.POSTransId = PTD.TransID" and removed "PTS.TransDetailID = PTD.TransDetailID"

                strWhereClause = " Where convert(datetime,TransDate,109) between convert(datetime, cast('" + this.dtpSaleStartDate.Text + " 00:00:00 ' as datetime) ,113) and convert(datetime, cast('" + this.dtpSaleEndDate.Text + " 23:59:59' as datetime) ,113) ";
                if (this.txtCustomer.Tag != null && this.txtCustomer.Text != "")
                {
                    strWhereClause += " and PTH.CustomerID=" + POS_Core.Resources.Configuration.convertNullToInt(this.txtCustomer.Tag.ToString().Trim()).ToString() + " ";
                    //WhereClause += " and PTH.CustomerID=" + Resources.Configuration.convertNullToInt(this.txtCustomer.Tag.ToString().Trim()).ToString() + " ";    Sprint-23 - PRIMEPOS-2029 28-Apr-2016 JY Commented
                }

                if (optTransSales.Checked == true)
                {
                    strWhereClause += " and PTH.TransType=1 ";
                    //WhereClause += " and PTH.TransType=1 ";   Sprint-23 - PRIMEPOS-2029 28-Apr-2016 JY Commented
                }
                else if (optTransReturn.Checked == true)
                {
                    strWhereClause += " and PTH.TransType=2 ";
                    //WhereClause += " and PTH.TransType=2 ";   Sprint-23 - PRIMEPOS-2029 28-Apr-2016 JY Commented
                }

                if (cboMonitoringCategory.SelectedIndex > 0)
                {
                    //strQuery += " and PTD.ItemID In (Select ItemID From ItemMonitorCategoryDetail Where ItemMonCatID=" + cboMonitoringCategory.SelectedItem.DataValue.ToString() + ") ";
                    strWhereClause += " and PTD.TransDetailID IN (Select TransDetailID From ItemMonitorTransDetail WHERE ItemMonCatID = " + cboMonitoringCategory.SelectedItem.DataValue.ToString() + ")";  
                    //WhereClause += " and PTD.TransDetailID  In (Select TransDetailID From ItemMonitorTransDetail WHERE ItemMonCatID = " + cboMonitoringCategory.SelectedItem.DataValue.ToString() + ")";  Sprint-23 - PRIMEPOS-2029 28-Apr-2016 JY Commented
                }
                else
                {
                    //strQuery += " and PTD.ItemID In (Select ItemID From ItemMonitorCategoryDetail) ";
                }

                strTotals += strWhereClause + " AND PTD.TransDetailID  IN (SELECT TransDetailID FROM ItemMonitorTransDetail) GROUP BY PTH.CustomerID";
                strQuery += " LEFT JOIN  (" + strTotals + " ) Z ON Z.CustomerID = Cus.CustomerID" + strWhereClause + " order by PTH.CustomerID, PTD.ItemID, PTH.transid desc ";

                clsReports.setCRTextObjectText("txtFromDate", "From :" + this.dtpSaleStartDate.Text, oRpt);
                clsReports.setCRTextObjectText("txtToDate", "To :" + dtpSaleEndDate.Text, oRpt);

                DataSet ds = clsReports.GetReportSource(strQuery);

                //ds.Tables[0].Columns.Add("MonitoringCats", typeof(string));   Sprint-23 - PRIMEPOS-2029 28-Apr-2016 JY Commented
                string sItems = "";
                string sPOSTrans = "";
                foreach (DataRow oRow in ds.Tables[0].Rows)
                {
                    if (sItems.IndexOf("'" + oRow["ItemID"].ToString() + "'") < 0)
                    {
                        sItems += "'" + oRow["ItemID"].ToString() + "',";
                    }

                    if (sItems.IndexOf("'" + oRow["TransID"].ToString() + "'") < 0)
                    {
                        sPOSTrans += "'" + oRow["TransID"].ToString() + "',";
                    }
                }

                #region Sprint-23 - PRIMEPOS-2029 28-Apr-2016 JY Commented
                //if (sItems.Length > 0)
                //{
                //    sItems = sItems.Substring(0, sItems.Length - 1);
                //    //strQuery = "Select IMC.ItemID, IM.Description from ItemMonitorCategoryDetail IMC, ItemMonitorCategory IM " +
                //    //   " Where IMC.ItemMonCatID=IM.ID And ItemID In (" + sItems + ")";

                //    ////Sprint-23 - PRIMEPOS-2029 21-Apr-2016 JY corrected the sql as it should fetch the data w.r.t. transaction entry
                //    strQuery = "select distinct PTD.TransDetailID, b.Description from ItemMonitorTransDetail a " +
                //                " inner join ItemMonitorCategory b on a.ItemMonCatId = b.ID " +
                //                " inner join POSTransactionDetail PTD on PTD.TransDetailID = a.TransDetailId " +
                //                " inner join POSTransaction PTH on PTH.TransID =PTD.TransID " +
                //                " Where convert(datetime,TransDate,109) between convert(datetime, cast('" + this.dtpSaleStartDate.Text + " 00:00:00 ' as datetime) ,113) and convert(datetime, cast('" + this.dtpSaleEndDate.Text + " 23:59:59' as datetime) ,113) " +
                //                WhereClause;
                    
                //    DataSet dsCat = clsReports.GetReportSource(strQuery);

                //    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                //    {
                //        sItems = "";
                //        DataRow[] oCatRows = dsCat.Tables[0].Select("TransDetailID='" + ds.Tables[0].Rows[i]["TransDetailID"].ToString() + "'");

                //        foreach (DataRow oCatRow in oCatRows)
                //        {
                //            sItems += oCatRow["Description"].ToString() + ",";
                //        }
                //        if (sItems.Length > 0)
                //        {
                //            ds.Tables[0].Rows[i]["MonitoringCats"] = sItems.Substring(0, sItems.Length - 1);
                //        }
                //    }
                //}
                #endregion

                ds.Tables[0].Columns.Add("SignDataBinary", System.Type.GetType("System.Byte[]"));
                if (sPOSTrans.Length > 0)
                {
                    sPOSTrans = sPOSTrans.Substring(0, sPOSTrans.Length - 1);
                    POSTransSignLog oPOSSignLog = new POSTransSignLog();
                    POSTransSignLogData oPOSSignLogData = new POSTransSignLogData();
                    string whereclause = clsPOSDBConstants.POSTransSignLog_Fld_POSTransId + " IN (" + sPOSTrans + ")";
                    oPOSSignLogData = oPOSSignLog.PopulateList(whereclause);
                    Bitmap bit = null;
                    System.IO.MemoryStream oStream = null;
                    foreach (POSTransSignLogRow oRow in oPOSSignLogData.POSTransSignLog.Rows)
                    {
                        if (oRow.SignDataBinary != null) 
                        {
                            MemoryStream ms = new MemoryStream((byte[])oRow.SignDataBinary);
                            bit = new Bitmap(ms);
                        }
                        else
                        {
                            bit = clsUIHelper.GetSignature(oRow.SignDataText.ToString(), POS_Core.Resources.Configuration.CInfo.SigType);
                        }
                        oStream = new System.IO.MemoryStream();
                        if (bit != null)
                        {
                            int RowIndex = 0;
                            foreach (DataRow dr in ds.Tables[0].Rows)
                            {
                                if (oRow.POSTransId.ToString() == dr["TransID"].ToString())
                                {
                                    String ErrorMsg = "";
                                    Image oBmpSig = clsUIHelper.GetSignature(string.Empty, oRow.SignDataBinary, oRow.SignContext, out ErrorMsg);

                                    System.IO.MemoryStream SigoStream = new System.IO.MemoryStream();
                                    if (ErrorMsg != "")
                                    {
                                        Graphics oGrp = Graphics.FromImage(oBmpSig);
                                        oGrp.Clear(Color.White);
                                        oGrp.DrawRectangle(new Pen(Color.White), 0, 0, 300, 300);
                                        oBmpSig.Save(SigoStream, System.Drawing.Imaging.ImageFormat.Jpeg);

                                        dr["SignDataBinary"] = SigoStream.ToArray();
                                    }
                                    else
                                    {
                                        oBmpSig.Save(SigoStream, System.Drawing.Imaging.ImageFormat.Jpeg);
                                        dr["SignDataBinary"] = SigoStream.ToArray();
                                    }
                                }
                                RowIndex++;
                            }
                        }
                    }
                }
                oRpt.Database.Tables[0].SetDataSource(ds.Tables[0]);

                #region Sprint-23 - PRIMEPOS-2029 29-Apr-2016 JY Added for report footer totals
                string strFooterTotals = "Select SUM(PTD.ExtendedPrice) AS TotalExtPrice, SUM(PTD.Discount) AS TotalDiscount, SUM(PTD.TaxAmount) AS TotalTax, (SUM(PTD.ExtendedPrice) - SUM(PTD.Discount) + SUM(PTD.TaxAmount)) AS TotalAmount " +
                            " FROM POSTransaction as PTH  " +
                            " INNER JOIN Customer Cus ON PTH.CustomerId = Cus.CustomerID " +
                            " INNER JOIN util_POSSet ps ON ps.stationid=PTH.stationid  " +
                            " INNER JOIN POSTransactionDetail PTD ON PTH.TransID = PTD.TransID " +
                            " LEFT OUTER JOIN POSTransSignLog PTS ON PTS.POSTransId = PTD.TransID " +  //Sprint-25 24-Feb-2017 JY Added "PTS.POSTransId = PTD.TransID" and removed "PTS.TransDetailID = PTD.TransDetailID"
                            strWhereClause + " AND PTD.TransDetailID  IN (SELECT TransDetailID FROM ItemMonitorTransDetail)";

                DataSet dsFooterTotals = clsReports.GetReportSource(strFooterTotals);
                oRpt.Database.Tables[1].SetDataSource(dsFooterTotals.Tables[0]);
                #endregion
                clsReports.DStoExport = ds; //PRIMEPOS-2471 16-Feb-2021 JY Added
                clsReports.Preview(blnPrint, oRpt);
                //clsReports.Preview(blnPrint, strQuery, strFooterTotals, oRpt, 1);
                this.Cursor = System.Windows.Forms.Cursors.Default;
            }
            catch (Exception exp)
            {
                this.Cursor = System.Windows.Forms.Cursors.Default;
                clsUIHelper.ShowErrorMsg(exp.Message);
            }

        }

		private void btnPrint_Click(object sender, System.EventArgs e)
		{
			PreviewReport(true);
		}	

        private void txtCustomer_EditorButtonClick(object sender, Infragistics.Win.UltraWinEditors.EditorButtonEventArgs e)
        {
            try
            {
                SearchCustomer();
            }
            catch (Exception) { }
        }

        private void SearchCustomer()
        {
            try
            {
                //frmCustomerSearch oSearch = new frmCustomerSearch(txtCustomer.Text);
                frmSearchMain oSearch = new frmSearchMain(txtCustomer.Text, true, clsPOSDBConstants.Customer_tbl);    //18-Dec-2017 JY Added new reference
                oSearch.ActiveOnly = 1;
                oSearch.ShowDialog(this);
                if (!oSearch.IsCanceled)
                {
                    string strCode = oSearch.SelectedRowID();
                    if (strCode == "")
                    {
                        ClearCustomer();
                        return;
                    }

                    FKEdit(strCode, clsPOSDBConstants.Customer_tbl);
                    this.txtCustomer.Focus();
                }
                else
                {
                    ClearCustomer();
                }
            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
                ClearCustomer();
            }
        }

        private void ClearCustomer()
        {
            //FKEdit("-1", clsPOSDBConstants.Customer_tbl);
            this.txtCustomer.Text = String.Empty;
            this.lblCustomerName.Text = String.Empty;
            this.txtCustomer.Tag = String.Empty;
            this.lblCustomerName.Text = String.Empty;
        }

        private void FKEdit(string code, string senderName)
        {
            if (senderName == clsPOSDBConstants.Customer_tbl)
            {
                #region Customer
                try
                {
                    Customer oCustomer = new Customer();
                    CustomerData oCustomerData;
                    CustomerRow oCustomerRow = null;
                    oCustomerData = oCustomer.Populate(code);
                    oCustomerRow = oCustomerData.Customer[0];
                    if (oCustomerRow != null)
                    {
                        this.txtCustomer.Text = oCustomerRow.AccountNumber.ToString();
                        this.txtCustomer.Tag = oCustomerRow.CustomerId.ToString();
                        this.lblCustomerName.Text = oCustomerRow.CustomerFullName;
                        //Added By Dharmendra(SRT) which will be required when processing
                    }
                }
                catch (System.IndexOutOfRangeException)
                {
                    SearchCustomer();
                }
                catch (Exception exp)
                {
                    clsUIHelper.ShowErrorMsg(exp.Message);
                    SearchCustomer();
                }
                #endregion
            }
        }

        private void txtCustomer_Leave(object sender, EventArgs e)
        {
            try
            {
                string txtValue = this.txtCustomer.Text;
                if (txtValue.Trim() != "")
                {
                    FKEdit(txtValue, clsPOSDBConstants.Customer_tbl);
                }
                else
                {
                    this.txtCustomer.Tag = "";
                    this.lblCustomerName.Text = "";
                }
            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }

        private void cboMonitoringCategory_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Tab || e.KeyCode == Keys.Up || e.KeyCode == Keys.Down)
                e.Handled = false;
            else
                e.Handled = true;
        }

        private void cboMonitoringCategory_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }
    }
}
