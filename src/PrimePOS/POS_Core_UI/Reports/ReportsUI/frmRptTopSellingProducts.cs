using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using CrystalDecisions.Shared;
using CrystalDecisions.ReportSource;
using CrystalDecisions.CrystalReports.Engine;
using POS_Core_UI.Reports.Reports;
using CrystalDecisions.CrystalReports.Engine;
using System.Data;
using POS_Core.BusinessRules;

namespace POS_Core_UI.Reports.ReportsUI
{
    /// <summary>
    /// Summary description for frmInventoryReports.
    /// </summary>
    public class frmRptTopSellingProducts : System.Windows.Forms.Form, ICommandLIneTaskControl
    {
        private System.Windows.Forms.GroupBox gbInventoryReceived;
        private Infragistics.Win.UltraWinSchedule.UltraCalendarCombo dtpToDate;
        private Infragistics.Win.UltraWinSchedule.UltraCalendarCombo dtpFromDate;
        private Infragistics.Win.Misc.UltraLabel ultraLabel13;
        private Infragistics.Win.Misc.UltraLabel ultraLabel14;
        private System.Windows.Forms.GroupBox ultraGroupBox2;
        private Infragistics.Win.UltraWinEditors.UltraOptionSet optByName;
        private System.Windows.Forms.GroupBox groupBox2;
        private Infragistics.Win.Misc.UltraButton btnPrint;
        private Infragistics.Win.Misc.UltraButton btnView;
        private Infragistics.Win.Misc.UltraButton btnClose;
        private Infragistics.Win.Misc.UltraLabel lblTransactionType;
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;

        public frmRptTopSellingProducts()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();
            //
            // TODO: Add any constructor code after InitializeComponent call
            //
            this.customControl = new usrDateRangeParams();  //PRIMEPOS-2485 02-Apr-2021 JY Added
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
            Infragistics.Win.UltraWinSchedule.CalendarCombo.DateButton dateButton1 = new Infragistics.Win.UltraWinSchedule.CalendarCombo.DateButton();
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinSchedule.CalendarCombo.DateButton dateButton2 = new Infragistics.Win.UltraWinSchedule.CalendarCombo.DateButton();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.ValueListItem valueListItem1 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem2 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmRptTopSellingProducts));
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance7 = new Infragistics.Win.Appearance();
            this.gbInventoryReceived = new System.Windows.Forms.GroupBox();
            this.dtpToDate = new Infragistics.Win.UltraWinSchedule.UltraCalendarCombo();
            this.dtpFromDate = new Infragistics.Win.UltraWinSchedule.UltraCalendarCombo();
            this.ultraLabel13 = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabel14 = new Infragistics.Win.Misc.UltraLabel();
            this.ultraGroupBox2 = new System.Windows.Forms.GroupBox();
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
            this.gbInventoryReceived.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbInventoryReceived.Location = new System.Drawing.Point(15, 112);
            this.gbInventoryReceived.Name = "gbInventoryReceived";
            this.gbInventoryReceived.Size = new System.Drawing.Size(424, 86);
            this.gbInventoryReceived.TabIndex = 1;
            this.gbInventoryReceived.TabStop = false;
            this.gbInventoryReceived.Text = "Top Selling Products";
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
            this.dtpToDate.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Office2003ToolbarButton;
            this.dtpToDate.DateButtons.Add(dateButton1);
            this.dtpToDate.Location = new System.Drawing.Point(200, 52);
            this.dtpToDate.Name = "dtpToDate";
            this.dtpToDate.NonAutoSizeHeight = 10;
            this.dtpToDate.Size = new System.Drawing.Size(123, 21);
            this.dtpToDate.TabIndex = 5;
            this.dtpToDate.Tag = "To Date";
            this.dtpToDate.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
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
            this.dtpFromDate.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Office2003ToolbarButton;
            this.dtpFromDate.DateButtons.Add(dateButton2);
            this.dtpFromDate.Location = new System.Drawing.Point(200, 24);
            this.dtpFromDate.Name = "dtpFromDate";
            this.dtpFromDate.NonAutoSizeHeight = 10;
            this.dtpFromDate.Size = new System.Drawing.Size(123, 21);
            this.dtpFromDate.TabIndex = 4;
            this.dtpFromDate.Tag = "From Date";
            this.dtpFromDate.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.dtpFromDate.Value = new System.DateTime(2004, 5, 25, 0, 0, 0, 0);
            // 
            // ultraLabel13
            // 
            this.ultraLabel13.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultraLabel13.Location = new System.Drawing.Point(104, 55);
            this.ultraLabel13.Name = "ultraLabel13";
            this.ultraLabel13.Size = new System.Drawing.Size(91, 15);
            this.ultraLabel13.TabIndex = 2;
            this.ultraLabel13.Text = "To Date";
            // 
            // ultraLabel14
            // 
            this.ultraLabel14.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultraLabel14.Location = new System.Drawing.Point(104, 27);
            this.ultraLabel14.Name = "ultraLabel14";
            this.ultraLabel14.Size = new System.Drawing.Size(91, 15);
            this.ultraLabel14.TabIndex = 1;
            this.ultraLabel14.Text = "From Date";
            // 
            // ultraGroupBox2
            // 
            this.ultraGroupBox2.Controls.Add(this.optByName);
            this.ultraGroupBox2.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultraGroupBox2.Location = new System.Drawing.Point(15, 54);
            this.ultraGroupBox2.Name = "ultraGroupBox2";
            this.ultraGroupBox2.Size = new System.Drawing.Size(424, 50);
            this.ultraGroupBox2.TabIndex = 0;
            this.ultraGroupBox2.TabStop = false;
            this.ultraGroupBox2.Text = "Selling Products By";
            // 
            // optByName
            // 
            this.optByName.BorderStyle = Infragistics.Win.UIElementBorderStyle.None;
            this.optByName.CheckedIndex = 0;
            appearance3.FontData.BoldAsString = "False";
            this.optByName.ItemAppearance = appearance3;
            this.optByName.ItemOrigin = new System.Drawing.Point(0, 2);
            valueListItem1.DataValue = 1;
            valueListItem1.DisplayText = "Quantity";
            valueListItem2.DataValue = 2;
            valueListItem2.DisplayText = "Amount";
            this.optByName.Items.AddRange(new Infragistics.Win.ValueListItem[] {
            valueListItem1,
            valueListItem2});
            this.optByName.ItemSpacingHorizontal = 50;
            this.optByName.Location = new System.Drawing.Point(104, 20);
            this.optByName.Name = "optByName";
            this.optByName.Size = new System.Drawing.Size(238, 20);
            this.optByName.TabIndex = 3;
            this.optByName.Text = "Quantity";
            this.optByName.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnPrint);
            this.groupBox2.Controls.Add(this.btnClose);
            this.groupBox2.Controls.Add(this.btnView);
            this.groupBox2.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.groupBox2.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.Location = new System.Drawing.Point(15, 202);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(424, 57);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            // 
            // btnPrint
            // 
            appearance4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(62)))), ((int)(((byte)(176)))));
            appearance4.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance4.FontData.BoldAsString = "True";
            appearance4.ForeColor = System.Drawing.Color.White;
            appearance4.Image = ((object)(resources.GetObject("appearance4.Image")));
            this.btnPrint.Appearance = appearance4;
            this.btnPrint.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            this.btnPrint.Location = new System.Drawing.Point(117, 19);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(85, 26);
            this.btnPrint.TabIndex = 7;
            this.btnPrint.Text = "&Print";
            this.btnPrint.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click_1);
            // 
            // btnClose
            // 
            appearance5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(62)))), ((int)(((byte)(176)))));
            appearance5.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance5.FontData.BoldAsString = "True";
            appearance5.ForeColor = System.Drawing.Color.White;
            appearance5.Image = ((object)(resources.GetObject("appearance5.Image")));
            this.btnClose.Appearance = appearance5;
            this.btnClose.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(301, 20);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(85, 26);
            this.btnClose.TabIndex = 8;
            this.btnClose.Text = "&Close";
            this.btnClose.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnView
            // 
            appearance6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(62)))), ((int)(((byte)(176)))));
            appearance6.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance6.FontData.BoldAsString = "True";
            appearance6.ForeColor = System.Drawing.Color.White;
            appearance6.Image = ((object)(resources.GetObject("appearance6.Image")));
            this.btnView.Appearance = appearance6;
            this.btnView.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            this.btnView.Location = new System.Drawing.Point(209, 20);
            this.btnView.Name = "btnView";
            this.btnView.Size = new System.Drawing.Size(85, 26);
            this.btnView.TabIndex = 6;
            this.btnView.Text = "&View";
            this.btnView.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnView.Click += new System.EventHandler(this.btnView_Click);
            // 
            // lblTransactionType
            // 
            appearance7.ForeColor = System.Drawing.Color.White;
            appearance7.ForeColorDisabled = System.Drawing.Color.White;
            appearance7.TextHAlignAsString = "Center";
            this.lblTransactionType.Appearance = appearance7;
            this.lblTransactionType.BackColorInternal = System.Drawing.Color.Transparent;
            this.lblTransactionType.Font = new System.Drawing.Font("Arial", 18F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTransactionType.Location = new System.Drawing.Point(35, 14);
            this.lblTransactionType.Name = "lblTransactionType";
            this.lblTransactionType.Size = new System.Drawing.Size(385, 30);
            this.lblTransactionType.TabIndex = 32;
            this.lblTransactionType.Text = "Top Selling Products";
            // 
            // frmRptTopSellingProducts
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(455, 275);
            this.Controls.Add(this.lblTransactionType);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.ultraGroupBox2);
            this.Controls.Add(this.gbInventoryReceived);
            this.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmRptTopSellingProducts";
            this.Text = "Top Selling Products";
            this.Load += new System.EventHandler(this.frmInventoryReports_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmRptInventoryReceived_KeyDown);
            this.gbInventoryReceived.ResumeLayout(false);
            this.gbInventoryReceived.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dtpToDate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtpFromDate)).EndInit();
            this.ultraGroupBox2.ResumeLayout(false);
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

        private void Preview(bool PrintId, bool bCalledFromScheduler = false)   //PRIMEPOS-2485 02-Apr-2021 JY Added bCalledFromScheduler
        {
            try
            {
                rptTopSellingProduct oRpt = new rptTopSellingProduct();
                //rptTopSellingProduct oRpt2= new rptTopSellingProduct();

                clsReports.setCRTextObjectText("txtFromDate", dtpFromDate.Text, oRpt);
                clsReports.setCRTextObjectText("txtToDate", dtpToDate.Text, oRpt);

                //clsReports.setCRTextObjectText("txtFromDate",dtpFromDate.Text,oRpt2);
                //clsReports.setCRTextObjectText("txtToDate",dtpToDate.Text,oRpt2);

                //	CrystalDecisions.CrystalReports.Engine.TextObject txt = (CrystalDecisions.CrystalReports.Engine.TextObject)oRpt.ReportDefinition.ReportObjects["txtFromDate"];
                //				txt.Text = dtpFromDate.Text;

                string sSQL = "";

                //Modifeid  By Shitaljit on 20 Aug 2012
                //To add extra fields Item.QtyInStock, Item.ReOrderLevel,Item.QtyOnOrder into the report.
                if (optByName.CheckedIndex == 0)
                {
                    clsReports.setCRTextObjectText("txtAmountDiscount", "Quantity", oRpt);
                    sSQL = " SELECT Item.ItemId," +
                                    " dtl.ItemDescription as Description " +
                                    " ,Item.QtyInStock, Item.ReOrderLevel  " +
                                    ", Item.LastCostPrice as Cost,Item.MinOrdQty " +
                                    " , SUM(Qty) Quantity ,  '1' as IsQty  " +
                                " FROM  " +
                                    " POSTransactionDetail As dtl " +
                                    " , POSTransaction as Trans " +
                                    " , Item " +
                                " WHERE " +
                                    " Trans.TransID = dtl.TransID " +
                                    " AND dtl.ItemId = Item.ItemId ";//AND Qty >0
                }
                else
                {
                    clsReports.setCRTextObjectText("txtAmountDiscount", "Amount", oRpt);
                    sSQL = " SELECT Item.ItemId," +
                                " dtl.ItemDescription as Description " +
                                " ,Item.QtyInStock, Item.ReOrderLevel  " +
                                ", Item.LastCostPrice as Cost,Item.MinOrdQty" +
                                " , SUM(ExtendedPrice + dtl.TaxAmount - dtl.Discount) Quantity , '0' as IsQty " +
                            " FROM  " +
                                " POSTransactionDetail As dtl " +
                                " , POSTransaction as Trans " +
                                " , Item " +
                            " WHERE " +
                                " Trans.TransID = dtl.TransID " +
                                " AND dtl.ItemId = Item.ItemId ";//and (ExtendedPrice + dtl.TaxAmount - dtl.Discount) >0 ";
                }


                sSQL = sSQL + buildCriteria();
                sSQL = sSQL + " ORDER BY Quantity desc ";
                if (optByName.CheckedIndex == 0) clsReports.Preview(PrintId, sSQL, oRpt, bCalledFromScheduler); //PRIMEPOS-2485 02-Apr-2021 JY Added bCalledFromScheduler
                oReport = oRpt; //PRIMEPOS-2485 02-Apr-2021 JY Added
            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }

        private string buildCriteria()
        {
            string sCriteria = "";

            if (dtpFromDate.Text != "")
                sCriteria = sCriteria + " AND Convert(smalldatetime,Convert(Varchar,TransDate,107)) >= '" + dtpFromDate.Text + "'";
            if (dtpToDate.Text != "")
                sCriteria = sCriteria + " AND Convert(smalldatetime,Convert(Varchar,TransDate,107)) <= '" + dtpToDate.Text + "'";

            sCriteria = sCriteria + " GROUP BY  " +
                                    "Item.ItemId, dtl.ItemDescription,Item.QtyInStock, Item.ReOrderLevel,Item.LastCostPrice,Item.MinOrdQty ";//Added By Shitaljit on 20 Aug 2012

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
                if (e.KeyData == System.Windows.Forms.Keys.Enter)
                {
                    this.SelectNextControl(this.ActiveControl, true, true, true, true);
                }
            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }

        private void gbInventoryReceived_Enter(object sender, System.EventArgs e)
        {
            dtpFromDate.Focus();
        }

        private void btnPrint_Click_1(object sender, System.EventArgs e)
        {
            Preview(true);
        }

        private void btnView_Click(object sender, System.EventArgs e)
        {
            Preview(false);
        }

        #region PRIMEPOS-2485 02-Apr-2021 JY Added
        public bool bSendPrint = true;
        private ReportClass oReport = new ReportClass();
        public usrDateRangeParams customControl;
        private const string ReportName = "TopSellingProducts";

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
            setControlsValues(ref dt);  //PRIMEPOS-3066 21-Mar-2022 JY Added
            return true;
        }

        #region PRIMEPOS-3066 21-Mar-2022 JY Added
        public void setControlsValues(ref DataTable dt)
        {
            double Num;

            foreach (DataRow odr in dt.Select("ControlsName = '" + this.dtpFromDate.Tag + " ' "))
            {
                if (double.TryParse(odr["ControlsValue"].ToString().Trim(), out Num))
                {
                    dtpFromDate.Value = DateTime.Now.AddDays(Convert.ToDouble(odr["ControlsValue"].ToString().Trim()) * -1);
                }
                else
                {
                    dtpFromDate.Value = odr["ControlsValue"].ToString().Trim();
                }
            }

            foreach (DataRow odr in dt.Select("ControlsName = '" + this.dtpToDate.Tag + "' "))
            {
                if (double.TryParse(odr["ControlsValue"].ToString().Trim(), out Num))
                {
                    dtpToDate.Value = DateTime.Now.AddDays(Convert.ToDouble(odr["ControlsValue"].ToString().Trim()) * -1);
                }
                else
                {
                    dtpToDate.Value = odr["ControlsValue"].ToString().Trim();
                }
            }
        }
        #endregion

        public bool RunTask(int TaskId, ref string filePath, bool bsendToPrint, ref string sNoOfRecordAffect)
        {
            SetControlParameters(TaskId);
            bSendPrint = bsendToPrint;
            //dtpFromDate.Value = DateTime.Now.AddDays(Left - 60);
            //dtpToDate.Value = DateTime.Now;
            Preview(bSendPrint, true);
            filePath = Application.StartupPath + @"\" + ReportName + (DateTime.Now).ToString().Replace("/", "").Replace(":", "") + ".pdf";
            this.oReport.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, filePath);
            return true;
        }

        public void GetTaskParameters(ref DataTable dt, int ScheduledTasksID)
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
