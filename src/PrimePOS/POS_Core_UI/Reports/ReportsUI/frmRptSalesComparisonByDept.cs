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
using System.Data;
namespace POS_Core_UI.Reports.ReportsUI
{
	/// <summary>
	/// Summary description for frmRptSalesComparisonByDept.
	/// </summary>
    public class frmRptSalesComparisonByDept : System.Windows.Forms.Form
    {
        private System.Windows.Forms.GroupBox groupBox1;
        private Infragistics.Win.Misc.UltraLabel lblTransactionType;
        private Infragistics.Win.Misc.UltraLabel ultraLabel20;
        private Infragistics.Win.Misc.UltraLabel ultraLabel19;
        private Infragistics.Win.UltraWinSchedule.UltraCalendarCombo dtpSaleEndDate;
        private Infragistics.Win.UltraWinSchedule.UltraCalendarCombo dtpSaleStartDate;
        private System.Windows.Forms.GroupBox groupBox2;
        private Infragistics.Win.Misc.UltraButton btnPrint;
        private Infragistics.Win.Misc.UltraButton btnView;
        private Infragistics.Win.Misc.UltraButton btnClose;
        private Infragistics.Win.Misc.UltraLabel ultraLabel1;
        private UltraComboEditor cboReportColumnBy;
        private UltraTextEditor txtDepartment;
        private Infragistics.Win.Misc.UltraLabel ultraLabel12;
        private Infragistics.Win.Misc.UltraLabel ultraLabel2;
        private RadioButton optReportByItem;
        private RadioButton optReportByDepartment;
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;

        public frmRptSalesComparisonByDept()
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
            Infragistics.Win.Appearance appearance20 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinEditors.EditorButton editorButton1 = new Infragistics.Win.UltraWinEditors.EditorButton();
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmRptSalesComparisonByDept));
            Infragistics.Win.Appearance appearance21 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance22 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance23 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinSchedule.CalendarCombo.DateButton dateButton5 = new Infragistics.Win.UltraWinSchedule.CalendarCombo.DateButton();
            Infragistics.Win.UltraWinSchedule.CalendarCombo.DateButton dateButton6 = new Infragistics.Win.UltraWinSchedule.CalendarCombo.DateButton();
            Infragistics.Win.Appearance appearance24 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance25 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance26 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance27 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance28 = new Infragistics.Win.Appearance();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.ultraLabel2 = new Infragistics.Win.Misc.UltraLabel();
            this.optReportByItem = new System.Windows.Forms.RadioButton();
            this.optReportByDepartment = new System.Windows.Forms.RadioButton();
            this.txtDepartment = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.ultraLabel12 = new Infragistics.Win.Misc.UltraLabel();
            this.cboReportColumnBy = new Infragistics.Win.UltraWinEditors.UltraComboEditor();
            this.ultraLabel20 = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabel19 = new Infragistics.Win.Misc.UltraLabel();
            this.dtpSaleEndDate = new Infragistics.Win.UltraWinSchedule.UltraCalendarCombo();
            this.dtpSaleStartDate = new Infragistics.Win.UltraWinSchedule.UltraCalendarCombo();
            this.ultraLabel1 = new Infragistics.Win.Misc.UltraLabel();
            this.lblTransactionType = new Infragistics.Win.Misc.UltraLabel();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnPrint = new Infragistics.Win.Misc.UltraButton();
            this.btnClose = new Infragistics.Win.Misc.UltraButton();
            this.btnView = new Infragistics.Win.Misc.UltraButton();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtDepartment)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboReportColumnBy)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtpSaleEndDate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtpSaleStartDate)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.ultraLabel2);
            this.groupBox1.Controls.Add(this.optReportByItem);
            this.groupBox1.Controls.Add(this.optReportByDepartment);
            this.groupBox1.Controls.Add(this.txtDepartment);
            this.groupBox1.Controls.Add(this.ultraLabel12);
            this.groupBox1.Controls.Add(this.cboReportColumnBy);
            this.groupBox1.Controls.Add(this.ultraLabel20);
            this.groupBox1.Controls.Add(this.ultraLabel19);
            this.groupBox1.Controls.Add(this.dtpSaleEndDate);
            this.groupBox1.Controls.Add(this.dtpSaleStartDate);
            this.groupBox1.Controls.Add(this.ultraLabel1);
            this.groupBox1.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.groupBox1.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(16, 61);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(463, 210);
            this.groupBox1.TabIndex = 27;
            this.groupBox1.TabStop = false;
            // 
            // ultraLabel2
            // 
            appearance20.ForeColor = System.Drawing.Color.White;
            this.ultraLabel2.Appearance = appearance20;
            this.ultraLabel2.AutoSize = true;
            this.ultraLabel2.Location = new System.Drawing.Point(12, 153);
            this.ultraLabel2.Name = "ultraLabel2";
            this.ultraLabel2.Size = new System.Drawing.Size(79, 18);
            this.ultraLabel2.TabIndex = 38;
            this.ultraLabel2.Text = "Report By:";
            // 
            // optReportByItem
            // 
            this.optReportByItem.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.optReportByItem.ForeColor = System.Drawing.Color.White;
            this.optReportByItem.Location = new System.Drawing.Point(223, 150);
            this.optReportByItem.Name = "optReportByItem";
            this.optReportByItem.Size = new System.Drawing.Size(141, 26);
            this.optReportByItem.TabIndex = 37;
            this.optReportByItem.Text = "Item";
            // 
            // optReportByDepartment
            // 
            this.optReportByDepartment.Checked = true;
            this.optReportByDepartment.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.optReportByDepartment.ForeColor = System.Drawing.Color.White;
            this.optReportByDepartment.Location = new System.Drawing.Point(96, 150);
            this.optReportByDepartment.Name = "optReportByDepartment";
            this.optReportByDepartment.Size = new System.Drawing.Size(143, 26);
            this.optReportByDepartment.TabIndex = 36;
            this.optReportByDepartment.TabStop = true;
            this.optReportByDepartment.Text = "Department";
            // 
            // txtDepartment
            // 
            this.txtDepartment.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance2.BackColor = System.Drawing.Color.White;
            appearance2.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(231)))), ((int)(((byte)(230)))));
            appearance2.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance2.BorderAlpha = Infragistics.Win.Alpha.Transparent;
            appearance2.Image = ((object)(resources.GetObject("appearance2.Image")));
            appearance2.ImageAlpha = Infragistics.Win.Alpha.Opaque;
            appearance2.ImageBackgroundAlpha = Infragistics.Win.Alpha.Opaque;
            appearance2.ImageBackgroundStyle = Infragistics.Win.ImageBackgroundStyle.Centered;
            editorButton1.Appearance = appearance2;
            editorButton1.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            editorButton1.Text = "";
            editorButton1.Width = 20;
            this.txtDepartment.ButtonsRight.Add(editorButton1);
            this.txtDepartment.Location = new System.Drawing.Point(256, 121);
            this.txtDepartment.Name = "txtDepartment";
            this.txtDepartment.ReadOnly = true;
            this.txtDepartment.Size = new System.Drawing.Size(152, 23);
            this.txtDepartment.TabIndex = 34;
            this.txtDepartment.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.txtDepartment.EditorButtonClick += new Infragistics.Win.UltraWinEditors.EditorButtonEventHandler(this.txtDepartment_EditorButtonClick);
            // 
            // ultraLabel12
            // 
            appearance21.ForeColor = System.Drawing.Color.White;
            this.ultraLabel12.Appearance = appearance21;
            this.ultraLabel12.AutoSize = true;
            this.ultraLabel12.Location = new System.Drawing.Point(12, 123);
            this.ultraLabel12.Name = "ultraLabel12";
            this.ultraLabel12.Size = new System.Drawing.Size(238, 18);
            this.ultraLabel12.TabIndex = 35;
            this.ultraLabel12.Text = "Department <Blank = Any Dept>";
            // 
            // cboReportColumnBy
            // 
            this.cboReportColumnBy.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.cboReportColumnBy.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboReportColumnBy.LimitToList = true;
            this.cboReportColumnBy.Location = new System.Drawing.Point(256, 92);
            this.cboReportColumnBy.Name = "cboReportColumnBy";
            this.cboReportColumnBy.Size = new System.Drawing.Size(152, 23);
            this.cboReportColumnBy.TabIndex = 32;
            this.cboReportColumnBy.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.cboReportColumnBy.KeyDown += new System.Windows.Forms.KeyEventHandler(this.cboReportColumnBy_KeyDown);
            this.cboReportColumnBy.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.cboReportColumnBy_KeyPress);
            // 
            // ultraLabel20
            // 
            appearance22.ForeColor = System.Drawing.Color.White;
            this.ultraLabel20.Appearance = appearance22;
            this.ultraLabel20.Location = new System.Drawing.Point(12, 38);
            this.ultraLabel20.Name = "ultraLabel20";
            this.ultraLabel20.Size = new System.Drawing.Size(116, 14);
            this.ultraLabel20.TabIndex = 20;
            this.ultraLabel20.Text = "Start Date";
            // 
            // ultraLabel19
            // 
            appearance23.ForeColor = System.Drawing.Color.White;
            this.ultraLabel19.Appearance = appearance23;
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
            this.dtpSaleEndDate.DateButtons.Add(dateButton5);
            this.dtpSaleEndDate.Location = new System.Drawing.Point(256, 63);
            this.dtpSaleEndDate.Name = "dtpSaleEndDate";
            this.dtpSaleEndDate.NonAutoSizeHeight = 10;
            this.dtpSaleEndDate.Size = new System.Drawing.Size(152, 22);
            this.dtpSaleEndDate.TabIndex = 1;
            this.dtpSaleEndDate.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.dtpSaleEndDate.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.dtpSaleEndDate.Value = new System.DateTime(2004, 5, 25, 0, 0, 0, 0);
            // 
            // dtpSaleStartDate
            // 
            this.dtpSaleStartDate.AllowNull = false;
            this.dtpSaleStartDate.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Office2003ToolbarButton;
            this.dtpSaleStartDate.DateButtons.Add(dateButton6);
            this.dtpSaleStartDate.Location = new System.Drawing.Point(256, 34);
            this.dtpSaleStartDate.Name = "dtpSaleStartDate";
            this.dtpSaleStartDate.NonAutoSizeHeight = 10;
            this.dtpSaleStartDate.Size = new System.Drawing.Size(152, 22);
            this.dtpSaleStartDate.TabIndex = 0;
            this.dtpSaleStartDate.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.dtpSaleStartDate.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.dtpSaleStartDate.Value = new System.DateTime(2004, 5, 25, 0, 0, 0, 0);
            // 
            // ultraLabel1
            // 
            appearance24.ForeColor = System.Drawing.Color.White;
            this.ultraLabel1.Appearance = appearance24;
            this.ultraLabel1.AutoSize = true;
            this.ultraLabel1.Location = new System.Drawing.Point(12, 94);
            this.ultraLabel1.Name = "ultraLabel1";
            this.ultraLabel1.Size = new System.Drawing.Size(108, 18);
            this.ultraLabel1.TabIndex = 33;
            this.ultraLabel1.Text = "Report Column";
            // 
            // lblTransactionType
            // 
            appearance25.ForeColor = System.Drawing.Color.White;
            appearance25.ForeColorDisabled = System.Drawing.Color.White;
            appearance25.TextHAlignAsString = "Center";
            this.lblTransactionType.Appearance = appearance25;
            this.lblTransactionType.BackColorInternal = System.Drawing.Color.Transparent;
            this.lblTransactionType.Font = new System.Drawing.Font("Arial", 18F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTransactionType.Location = new System.Drawing.Point(16, 16);
            this.lblTransactionType.Name = "lblTransactionType";
            this.lblTransactionType.Size = new System.Drawing.Size(424, 58);
            this.lblTransactionType.TabIndex = 26;
            this.lblTransactionType.Text = "Sales Comparison by Dept.";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnPrint);
            this.groupBox2.Controls.Add(this.btnClose);
            this.groupBox2.Controls.Add(this.btnView);
            this.groupBox2.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.groupBox2.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.Location = new System.Drawing.Point(17, 277);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(462, 57);
            this.groupBox2.TabIndex = 31;
            this.groupBox2.TabStop = false;
            // 
            // btnPrint
            // 
            appearance26.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(62)))), ((int)(((byte)(176)))));
            appearance26.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance26.FontData.BoldAsString = "True";
            appearance26.ForeColor = System.Drawing.Color.White;
            appearance26.Image = ((object)(resources.GetObject("appearance26.Image")));
            this.btnPrint.Appearance = appearance26;
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
            appearance27.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(62)))), ((int)(((byte)(176)))));
            appearance27.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance27.FontData.BoldAsString = "True";
            appearance27.ForeColor = System.Drawing.Color.White;
            appearance27.Image = ((object)(resources.GetObject("appearance27.Image")));
            this.btnClose.Appearance = appearance27;
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
            appearance28.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(62)))), ((int)(((byte)(176)))));
            appearance28.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance28.FontData.BoldAsString = "True";
            appearance28.ForeColor = System.Drawing.Color.White;
            appearance28.Image = ((object)(resources.GetObject("appearance28.Image")));
            this.btnView.Appearance = appearance28;
            this.btnView.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            this.btnView.Location = new System.Drawing.Point(277, 20);
            this.btnView.Name = "btnView";
            this.btnView.Size = new System.Drawing.Size(85, 26);
            this.btnView.TabIndex = 5;
            this.btnView.Text = "&View";
            this.btnView.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnView.Click += new System.EventHandler(this.btnView_Click_1);
            // 
            // frmRptSalesComparisonByDept
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(108)))), ((int)(((byte)(172)))));
            this.ClientSize = new System.Drawing.Size(491, 350);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.lblTransactionType);
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.Name = "frmRptSalesComparisonByDept";
            this.Text = "Sales Comparison by Dept.";
            this.Load += new System.EventHandler(this.frmRptSalesComparisonByDept_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmRptSalesComparisonByDept_KeyDown);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtDepartment)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboReportColumnBy)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtpSaleEndDate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtpSaleStartDate)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);

        }
        #endregion

        private void frmRptSalesComparisonByDept_Load(object sender, System.EventArgs e)
        {
            this.dtpSaleStartDate.Enter += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
            this.dtpSaleStartDate.Leave += new System.EventHandler(clsUIHelper.AfterExitEditMode);

            this.dtpSaleEndDate.Enter += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
            this.dtpSaleEndDate.Leave += new System.EventHandler(clsUIHelper.AfterExitEditMode);

            this.Left = (frmMain.getInstance().Width - frmMain.getInstance().ultraExplorerBar1.Width - this.Width) / 2;
            this.Top = (frmMain.getInstance().Height - this.Height) / 2;

            clsUIHelper.setColorSchecme(this);
            this.dtpSaleEndDate.Value = DateTime.Now;
            this.dtpSaleStartDate.Value = DateTime.Now;
            PopulateColumnTypes();

        }

        private void PopulateColumnTypes()
        {
            try
            {
                this.cboReportColumnBy.Items.Add("Y", "Yearly");
                this.cboReportColumnBy.Items.Add("Q", "Quarterly");
                this.cboReportColumnBy.Items.Add("M", "Monthly");
                this.cboReportColumnBy.Items.Add("W", "Weekly");
                this.cboReportColumnBy.SelectedIndex = 2;
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

        private void frmRptSalesComparisonByDept_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            try
            {
                if (e.KeyData == System.Windows.Forms.Keys.Enter)
                {
                    this.SelectNextControl(this.ActiveControl, true, true, true, true);
                }
                else if (e.KeyData == Keys.Escape)
                {
                    this.Close();
                }
                else if (e.KeyData == System.Windows.Forms.Keys.F4)
                {
                    if (this.txtDepartment.ContainsFocus == true)
                        this.SearchDept();
                }
            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }

        private void PreviewReport(bool blnPrint)
        {
            try
            {

                if (Convert.ToDateTime(this.dtpSaleEndDate.Value.ToString()).Date < Convert.ToDateTime(this.dtpSaleStartDate.Value.ToString()).Date)
                {
                    throw (new Exception("End date cannot be less than Start date."));
                }

                this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
                CrystalDecisions.CrystalReports.Engine.ReportClass oRpt;
                
                if (optReportByDepartment.Checked == true)
                {
                    oRpt = new rptSalesComparisonByDept();
                }
                else
                {
                    oRpt = new rptSalesComparisonByItem();
                }

                String strQuery="";
                //string strSubQuery;

                DataSet oDataSet = new DataSet();

                string itemDescription = " ,Rtrim(item.itemid) +'-'+rtrim(item.description) as itemdescription,ptd.Qty ";
                if (this.cboReportColumnBy.Value.ToString() == "Y")
                {
                    strQuery = "DECLARE @CurrDt datetime " +
                            " Set @CurrDt= cast(Cast(Month(GetDate()) as varchar) + '-1-'+ Cast(Year(GetDate()) as varchar) as datetime) " +
                            " Select PT.Transdate, cast(year(transdate) as varchar) as DataColumn, " +
                            " dept.deptname, subdept.description as subdeptname, ptd.extendedprice-ptd.discount+ptd.TaxAmount as NetAmount, " +
                            " cast(Year(transdate) as varchar) as DataColumnNum " + itemDescription +
                            " From postransactiondetail ptd  inner join postransaction pt on (pt.transid=ptd.transid) " +
                            " inner join item on (ptd.itemid=item.itemid) inner join department dept on (item.departmentid=dept.deptid) " +
                            " left join subdepartment subdept on (item.subdepartmentid=subdept.subdepartmentid) " +
                            " Where convert(datetime,TransDate,109) between convert(datetime, cast('" + this.dtpSaleStartDate.Text + " 00:00:00 ' as datetime) ,113) and convert(datetime, cast('" + this.dtpSaleEndDate.Text + " 23:59:59' as datetime) ,113) ";
                }
                else if (this.cboReportColumnBy.Value.ToString() == "Q")
                {
                    strQuery = "DECLARE @CurrDt datetime " +
                            " Set @CurrDt= cast(Cast(Month(GetDate()) as varchar) + '-1-'+ Cast(Year(GetDate()) as varchar) as datetime) " +
                            " Select PT.Transdate, ' Q-' + Cast(DatePart(q,transdate) as varchar) +' '+ cast(year(transdate) as varchar) as DataColumn, " +
                            " dept.deptname, subdept.description as subdeptname, ptd.extendedprice-ptd.discount+ptd.TaxAmount as NetAmount, " +
                            " cast(year(transdate) as varchar)+cast(DatePart(q,transdate) as varchar) as DataColumnNum " + itemDescription +
                            " From postransactiondetail ptd  inner join postransaction pt on (pt.transid=ptd.transid) " +
                            " inner join item on (ptd.itemid=item.itemid) inner join department dept on (item.departmentid=dept.deptid) " +
                            " left join subdepartment subdept on (item.subdepartmentid=subdept.subdepartmentid) " +
                            " Where convert(datetime,TransDate,109) between convert(datetime, cast('" + this.dtpSaleStartDate.Text + " 00:00:00 ' as datetime) ,113) and convert(datetime, cast('" + this.dtpSaleEndDate.Text + " 23:59:59' as datetime) ,113) ";
                }
                else if (this.cboReportColumnBy.Value.ToString() == "M")
                {
                    strQuery = "DECLARE @CurrDt datetime " +
                            " Set @CurrDt= cast(Cast(Month(GetDate()) as varchar) + '-1-'+ Cast(Year(GetDate()) as varchar) as datetime) " +
                            " Select PT.Transdate, Left(DateNAME(m,transdate),3) +' '+ cast(year(transdate) as varchar) as DataColumn, " +
                            " dept.deptname, subdept.description as subdeptname, ptd.extendedprice-ptd.discount+ptd.TaxAmount as NetAmount, " +
                            " cast(Year(transdate) as varchar)+right('0'+Cast(month(transdate) as varchar),2) as DataColumnNum " + itemDescription +
                            " From postransactiondetail ptd  inner join postransaction pt on (pt.transid=ptd.transid) " +
                            " inner join item on (ptd.itemid=item.itemid) inner join department dept on (item.departmentid=dept.deptid) " +
                            " left join subdepartment subdept on (item.subdepartmentid=subdept.subdepartmentid) " +
                            " Where convert(datetime,TransDate,109) between convert(datetime, cast('" + this.dtpSaleStartDate.Text + " 00:00:00 ' as datetime) ,113) and convert(datetime, cast('" + this.dtpSaleEndDate.Text + " 23:59:59' as datetime) ,113) ";
                }
                else if (this.cboReportColumnBy.Value.ToString() == "W")
                {
                    strQuery = "DECLARE @CurrDt datetime " +
                            " Set @CurrDt= cast(Cast(Month(GetDate()) as varchar) + '-1-'+ Cast(Year(GetDate()) as varchar) as datetime) " +
                            " Select PT.Transdate, ' W-' + Cast(DatePart(wk,transdate) as varchar) +' '+ cast(year(transdate) as varchar) as DataColumn, " +
                            " dept.deptname, subdept.description as subdeptname, ptd.extendedprice-ptd.discount+ptd.TaxAmount as NetAmount, " +
                            " cast(year(transdate) as varchar)+right('0'+cast(DatePart(wk,transdate) as varchar),2) as DataColumnNum " + itemDescription +
                            " From postransactiondetail ptd  inner join postransaction pt on (pt.transid=ptd.transid) " +
                            " inner join item on (ptd.itemid=item.itemid) inner join department dept on (item.departmentid=dept.deptid) " +
                            " left join subdepartment subdept on (item.subdepartmentid=subdept.subdepartmentid) " +
                            " Where convert(datetime,TransDate,109) between convert(datetime, cast('" + this.dtpSaleStartDate.Text + " 00:00:00 ' as datetime) ,113) and convert(datetime, cast('" + this.dtpSaleEndDate.Text + " 23:59:59' as datetime) ,113) ";
                }

                if (this.txtDepartment.Text.Trim() != "" && txtDepartment.Tag != null && txtDepartment.Tag.ToString().Trim().Length > 0)
                {
                    strQuery += " and dept.DeptCode in (" + this.txtDepartment.Tag.ToString().Trim() + ")";
                }

                strQuery += " order by PT.transdate  ";

                DataSet ds = clsReports.GetReportSource(strQuery);
                oRpt.Database.Tables[0].SetDataSource(ds.Tables[0]);
                clsReports.DStoExport = ds; //PRIMEPOS-2471 16-Feb-2021 JY Added
                clsReports.Preview(blnPrint, oRpt);
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

        private void btnView_Click_1(object sender, System.EventArgs e)
        {
            PreviewReport(false);
        }

        private void txtDepartment_EditorButtonClick(object sender, EditorButtonEventArgs e)
        {
            SearchDept();
        }

        private void SearchDept()
        {
            try
            {
                //frmSearch oSearch = new frmSearch(clsPOSDBConstants.Department_tbl);
                frmSearchMain oSearch = new frmSearchMain(true);    //20-Dec-2017 JY Added new reference
                oSearch.SearchTable = clsPOSDBConstants.Department_tbl; //20-Dec-2017 JY Added 
                oSearch.AllowMultiRowSelect = true;
                oSearch.SearchInConstructor = true;
                oSearch.ShowDialog(this);
                if (!oSearch.IsCanceled)
                {
                    string strDeptCode = "";
                    string strDeptName = "";
                    foreach (Infragistics.Win.UltraWinGrid.UltraGridRow oRow in oSearch.grdSearch.Rows)
                    {
                        if ((bool)oRow.Cells["check"].Value == true)
                        {
                            strDeptCode += ",'" + oRow.Cells["Code"].Text + "'";
                            strDeptName += "," + oRow.Cells["Name"].Text;
                        }
                    }

                    if (strDeptCode.StartsWith(","))
                    {
                        strDeptCode = strDeptCode.Substring(1);
                        strDeptName = strDeptName.Substring(1);
                    }
                    txtDepartment.Text = strDeptName;
                    txtDepartment.Tag = strDeptCode;
                }
                else
                {
                    txtDepartment.Text = string.Empty;
                    txtDepartment.Tag = string.Empty;
                }
            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }
        private string GetDeptName(string code, string senderName)
        {
            string returnValue = "";
            if (senderName == clsPOSDBConstants.Department_tbl)
            {
                #region Department
                try
                {
                    POS_Core.BusinessRules.Department oDept = new Department();
                    DepartmentData oDeptData;
                    DepartmentRow oDeptRow = null;
                    oDeptData = oDept.Populate(code);
                    oDeptRow = oDeptData.Department[0];
                    if (oDeptRow != null)
                    {
                        returnValue = oDeptRow.DeptCode.ToString();
                    }
                }
                catch (System.IndexOutOfRangeException)
                {
                    this.txtDepartment.Text = String.Empty;
                    SearchDept();
                }
                catch (Exception exp)
                {
                    clsUIHelper.ShowErrorMsg(exp.Message);
                    exp = null;
                    this.txtDepartment.Text = String.Empty;
                    SearchDept();
                }
                #endregion
            }
            return returnValue;
        }

        private void cboReportColumnBy_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Tab || e.KeyCode == Keys.Up || e.KeyCode == Keys.Down)
                e.Handled = false;
            else
                e.Handled = true;
        }

        private void cboReportColumnBy_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }
    }
}
