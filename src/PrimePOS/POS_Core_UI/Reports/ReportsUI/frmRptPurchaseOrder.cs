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
using POS_Core.BusinessRules;

namespace POS_Core_UI.Reports.ReportsUI
{
	/// <summary>
	/// Summary description for frmInventoryReports.
	/// </summary>
	public class frmRptPurchaseOrder : System.Windows.Forms.Form
	{
		private System.Windows.Forms.GroupBox gbInventoryReceived;
		private Infragistics.Win.UltraWinSchedule.UltraCalendarCombo dtpToDate;
		private Infragistics.Win.UltraWinSchedule.UltraCalendarCombo dtpFromDate;
		private Infragistics.Win.Misc.UltraLabel ultraLabel12;
		private Infragistics.Win.Misc.UltraLabel ultraLabel13;
		private Infragistics.Win.Misc.UltraLabel ultraLabel14;
		private System.Windows.Forms.GroupBox groupBox2;
		private Infragistics.Win.Misc.UltraButton ultraButton1;
		private Infragistics.Win.Misc.UltraButton btnView;
		private Infragistics.Win.Misc.UltraButton btnClose;
		private Infragistics.Win.Misc.UltraLabel lblTransactionType;
		private System.Windows.Forms.RadioButton optPBoth;
		private System.Windows.Forms.RadioButton optPNo;
        private System.Windows.Forms.RadioButton optPYes;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor txtVendor;
        private Infragistics.Win.UltraWinEditors.UltraComboEditor cboStatusList;
        private Infragistics.Win.Misc.UltraLabel cboPurchaseOrder;

        private string poStatus = string.Empty;       


		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public frmRptPurchaseOrder()
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
            Infragistics.Win.ValueListItem valueListItem1 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem2 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem3 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem4 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem5 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem6 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem7 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem8 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem9 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem10 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem11 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.UltraWinEditors.EditorButton editorButton1 = new Infragistics.Win.UltraWinEditors.EditorButton();
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmRptPurchaseOrder));
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinSchedule.CalendarCombo.DateButton dateButton1 = new Infragistics.Win.UltraWinSchedule.CalendarCombo.DateButton();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinSchedule.CalendarCombo.DateButton dateButton2 = new Infragistics.Win.UltraWinSchedule.CalendarCombo.DateButton();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance7 = new Infragistics.Win.Appearance();
            this.gbInventoryReceived = new System.Windows.Forms.GroupBox();
            this.cboPurchaseOrder = new Infragistics.Win.Misc.UltraLabel();
            this.cboStatusList = new Infragistics.Win.UltraWinEditors.UltraComboEditor();
            this.txtVendor = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.dtpToDate = new Infragistics.Win.UltraWinSchedule.UltraCalendarCombo();
            this.dtpFromDate = new Infragistics.Win.UltraWinSchedule.UltraCalendarCombo();
            this.ultraLabel12 = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabel13 = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabel14 = new Infragistics.Win.Misc.UltraLabel();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.ultraButton1 = new Infragistics.Win.Misc.UltraButton();
            this.btnClose = new Infragistics.Win.Misc.UltraButton();
            this.btnView = new Infragistics.Win.Misc.UltraButton();
            this.lblTransactionType = new Infragistics.Win.Misc.UltraLabel();
            this.optPBoth = new System.Windows.Forms.RadioButton();
            this.optPNo = new System.Windows.Forms.RadioButton();
            this.optPYes = new System.Windows.Forms.RadioButton();
            this.gbInventoryReceived.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cboStatusList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtVendor)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtpToDate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtpFromDate)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbInventoryReceived
            // 
            this.gbInventoryReceived.Controls.Add(this.cboPurchaseOrder);
            this.gbInventoryReceived.Controls.Add(this.cboStatusList);
            this.gbInventoryReceived.Controls.Add(this.txtVendor);
            this.gbInventoryReceived.Controls.Add(this.dtpToDate);
            this.gbInventoryReceived.Controls.Add(this.dtpFromDate);
            this.gbInventoryReceived.Controls.Add(this.ultraLabel12);
            this.gbInventoryReceived.Controls.Add(this.ultraLabel13);
            this.gbInventoryReceived.Controls.Add(this.ultraLabel14);
            this.gbInventoryReceived.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbInventoryReceived.Location = new System.Drawing.Point(15, 46);
            this.gbInventoryReceived.Name = "gbInventoryReceived";
            this.gbInventoryReceived.Size = new System.Drawing.Size(424, 169);
            this.gbInventoryReceived.TabIndex = 0;
            this.gbInventoryReceived.TabStop = false;
            this.gbInventoryReceived.Enter += new System.EventHandler(this.gbInventoryReceived_Enter_1);
            // 
            // cboPurchaseOrder
            // 
            this.cboPurchaseOrder.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboPurchaseOrder.Location = new System.Drawing.Point(78, 137);
            this.cboPurchaseOrder.Name = "cboPurchaseOrder";
            this.cboPurchaseOrder.Size = new System.Drawing.Size(103, 23);
            this.cboPurchaseOrder.TabIndex = 73;
            this.cboPurchaseOrder.Text = "Purchase Order";
            // 
            // cboStatusList
            // 
            this.cboStatusList.DropDownStyle = Infragistics.Win.DropDownStyle.DropDownList;
            this.cboStatusList.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            valueListItem1.DataValue = "8";
            valueListItem1.DisplayText = "ALL";
            valueListItem2.DataValue = "4";
            valueListItem2.DisplayText = "Acknowledge";
            valueListItem3.DataValue = "5";
            valueListItem3.DisplayText = "AcknowledgeManually";
            valueListItem4.DataValue = "3";
            valueListItem4.DisplayText = "Canceled";
            valueListItem5.DataValue = "15";
            valueListItem5.DisplayText = "DirectAcknowledge";
            valueListItem6.DataValue = "9";
            valueListItem6.DisplayText = "Expired";
            valueListItem7.DataValue = "10";
            valueListItem7.DisplayText = "Incomplete";
            valueListItem8.DataValue = "7";
            valueListItem8.DisplayText = "MaxAttempt";
            valueListItem9.DataValue = "0";
            valueListItem9.DisplayText = "Pending";
            valueListItem10.DataValue = "1";
            valueListItem10.DisplayText = "Processed";
            valueListItem11.DataValue = "2";
            valueListItem11.DisplayText = "Queued";
            this.cboStatusList.Items.AddRange(new Infragistics.Win.ValueListItem[] {
            valueListItem1,
            valueListItem2,
            valueListItem3,
            valueListItem4,
            valueListItem5,
            valueListItem6,
            valueListItem7,
            valueListItem8,
            valueListItem9,
            valueListItem10,
            valueListItem11});
            this.cboStatusList.Location = new System.Drawing.Point(215, 134);
            this.cboStatusList.Name = "cboStatusList";
            this.cboStatusList.Size = new System.Drawing.Size(131, 22);
            this.cboStatusList.TabIndex = 72;
            this.cboStatusList.SelectionChangeCommitted += new System.EventHandler(this.cboStatusList_SelectionChangeCommitted);
            // 
            // txtVendor
            // 
            this.txtVendor.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance1.Image = ((object)(resources.GetObject("appearance1.Image")));
            editorButton1.Appearance = appearance1;
            editorButton1.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Office2003ToolbarButton;
            editorButton1.Text = "";
            this.txtVendor.ButtonsRight.Add(editorButton1);
            this.txtVendor.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtVendor.Location = new System.Drawing.Point(215, 98);
            this.txtVendor.MaxLength = 20;
            this.txtVendor.Name = "txtVendor";
            this.txtVendor.Size = new System.Drawing.Size(131, 20);
            this.txtVendor.TabIndex = 5;
            this.txtVendor.EditorButtonClick += new Infragistics.Win.UltraWinEditors.EditorButtonEventHandler(this.txtVendor_EditorButtonClick);
            // 
            // dtpToDate
            // 
            this.dtpToDate.AllowNull = false;
            appearance2.FontData.BoldAsString = "False";
            appearance2.FontData.ItalicAsString = "False";
            appearance2.FontData.StrikeoutAsString = "False";
            appearance2.FontData.UnderlineAsString = "False";
            appearance2.ForeColor = System.Drawing.Color.Black;
            appearance2.ForeColorDisabled = System.Drawing.Color.Black;
            this.dtpToDate.Appearance = appearance2;
            this.dtpToDate.BackColor = System.Drawing.SystemColors.Window;
            this.dtpToDate.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Office2003ToolbarButton;
            this.dtpToDate.DateButtons.Add(dateButton1);
            this.dtpToDate.Location = new System.Drawing.Point(215, 61);
            this.dtpToDate.Name = "dtpToDate";
            this.dtpToDate.NonAutoSizeHeight = 10;
            this.dtpToDate.Size = new System.Drawing.Size(131, 21);
            this.dtpToDate.TabIndex = 4;
            this.dtpToDate.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.dtpToDate.Value = new System.DateTime(2011, 11, 9, 0, 0, 0, 0);
            this.dtpToDate.ValueChanged += new System.EventHandler(this.dtpToDate_ValueChanged);
            // 
            // dtpFromDate
            // 
            this.dtpFromDate.AllowNull = false;
            appearance3.FontData.BoldAsString = "False";
            appearance3.FontData.ItalicAsString = "False";
            appearance3.FontData.StrikeoutAsString = "False";
            appearance3.FontData.UnderlineAsString = "False";
            appearance3.ForeColor = System.Drawing.Color.Black;
            appearance3.ForeColorDisabled = System.Drawing.Color.Black;
            this.dtpFromDate.Appearance = appearance3;
            this.dtpFromDate.BackColor = System.Drawing.SystemColors.Window;
            this.dtpFromDate.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Office2003ToolbarButton;
            this.dtpFromDate.DateButtons.Add(dateButton2);
            this.dtpFromDate.Location = new System.Drawing.Point(215, 24);
            this.dtpFromDate.Name = "dtpFromDate";
            this.dtpFromDate.NonAutoSizeHeight = 10;
            this.dtpFromDate.Size = new System.Drawing.Size(131, 21);
            this.dtpFromDate.TabIndex = 3;
            this.dtpFromDate.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.dtpFromDate.Value = new System.DateTime(2011, 11, 9, 0, 0, 0, 0);
            this.dtpFromDate.ValueChanged += new System.EventHandler(this.dtpFromDate_ValueChanged);
            // 
            // ultraLabel12
            // 
            this.ultraLabel12.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultraLabel12.Location = new System.Drawing.Point(78, 101);
            this.ultraLabel12.Name = "ultraLabel12";
            this.ultraLabel12.Size = new System.Drawing.Size(91, 15);
            this.ultraLabel12.TabIndex = 3;
            this.ultraLabel12.Text = "Vendor Code";
            // 
            // ultraLabel13
            // 
            this.ultraLabel13.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultraLabel13.Location = new System.Drawing.Point(78, 62);
            this.ultraLabel13.Name = "ultraLabel13";
            this.ultraLabel13.Size = new System.Drawing.Size(52, 15);
            this.ultraLabel13.TabIndex = 2;
            this.ultraLabel13.Text = "To Date";
            // 
            // ultraLabel14
            // 
            this.ultraLabel14.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultraLabel14.Location = new System.Drawing.Point(78, 27);
            this.ultraLabel14.Name = "ultraLabel14";
            this.ultraLabel14.Size = new System.Drawing.Size(103, 15);
            this.ultraLabel14.TabIndex = 1;
            this.ultraLabel14.Text = "Order From Date";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.ultraButton1);
            this.groupBox2.Controls.Add(this.btnClose);
            this.groupBox2.Controls.Add(this.btnView);
            this.groupBox2.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.groupBox2.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.Location = new System.Drawing.Point(15, 216);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(424, 57);
            this.groupBox2.TabIndex = 30;
            this.groupBox2.TabStop = false;
            // 
            // ultraButton1
            // 
            appearance4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(62)))), ((int)(((byte)(176)))));
            appearance4.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance4.FontData.BoldAsString = "True";
            appearance4.ForeColor = System.Drawing.Color.White;
            appearance4.Image = ((object)(resources.GetObject("appearance4.Image")));
            this.ultraButton1.Appearance = appearance4;
            this.ultraButton1.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            this.ultraButton1.Location = new System.Drawing.Point(121, 19);
            this.ultraButton1.Name = "ultraButton1";
            this.ultraButton1.Size = new System.Drawing.Size(85, 26);
            this.ultraButton1.TabIndex = 11;
            this.ultraButton1.Text = "&Print";
            this.ultraButton1.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.ultraButton1.Click += new System.EventHandler(this.ultraButton1_Click);
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
            this.btnClose.Location = new System.Drawing.Point(305, 20);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(85, 26);
            this.btnClose.TabIndex = 12;
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
            this.btnView.Location = new System.Drawing.Point(213, 20);
            this.btnView.Name = "btnView";
            this.btnView.Size = new System.Drawing.Size(85, 26);
            this.btnView.TabIndex = 10;
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
            this.lblTransactionType.Location = new System.Drawing.Point(18, 8);
            this.lblTransactionType.Name = "lblTransactionType";
            this.lblTransactionType.Size = new System.Drawing.Size(417, 30);
            this.lblTransactionType.TabIndex = 31;
            this.lblTransactionType.Text = "Purchase Order";
            // 
            // optPBoth
            // 
            this.optPBoth.Location = new System.Drawing.Point(0, 0);
            this.optPBoth.Name = "optPBoth";
            this.optPBoth.Size = new System.Drawing.Size(104, 24);
            this.optPBoth.TabIndex = 0;
            // 
            // optPNo
            // 
            this.optPNo.Location = new System.Drawing.Point(0, 0);
            this.optPNo.Name = "optPNo";
            this.optPNo.Size = new System.Drawing.Size(104, 24);
            this.optPNo.TabIndex = 0;
            // 
            // optPYes
            // 
            this.optPYes.Location = new System.Drawing.Point(0, 0);
            this.optPYes.Name = "optPYes";
            this.optPYes.Size = new System.Drawing.Size(104, 24);
            this.optPYes.TabIndex = 0;
            // 
            // frmRptPurchaseOrder
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(455, 294);
            this.Controls.Add(this.lblTransactionType);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.gbInventoryReceived);
            this.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmRptPurchaseOrder";
            this.Text = "Purchase Order";
            this.Load += new System.EventHandler(this.frmRptPurchaseOrder_Load);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.frmRptPurchaseOrder_KeyUp);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmRptPurchaseOrder_KeyDown);
            this.gbInventoryReceived.ResumeLayout(false);
            this.gbInventoryReceived.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cboStatusList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtVendor)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtpToDate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtpFromDate)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);

		}
		#endregion

		private void frmRptPurchaseOrder_Load(object sender, System.EventArgs e)
		{
			clsUIHelper.setColorSchecme(this);
            //Added By Shitaljit on 16 August 2011
            DateTime dtCurrTime = new DateTime();
            dtCurrTime = DateTime.Now;
            dtpToDate.Value = dtCurrTime;
            dtpFromDate.Value = dtCurrTime;
            
            //Commented By Shitaljit on 16 August 2011
            //dtpToDate.Value = DateTime.Now;
            //dtpFromDate.Value = DateTime.Now;
           //End of Coding By shitaljit on 16 Aug 2011
		
			this.txtVendor.AfterEnterEditMode += new System.EventHandler(clsUIHelper.AfterEnterEditMode);			
			this.txtVendor.AfterExitEditMode += new System.EventHandler(clsUIHelper.AfterExitEditMode);			

			this.dtpFromDate.Enter += new System.EventHandler(clsUIHelper.AfterEnterEditMode);			
			this.dtpFromDate.Leave += new System.EventHandler(clsUIHelper.AfterExitEditMode);			
			
			this.dtpToDate.Enter += new System.EventHandler(clsUIHelper.AfterEnterEditMode);			
			this.dtpToDate.Leave += new System.EventHandler(clsUIHelper.AfterExitEditMode);
         
            this.cboStatusList.SelectedIndex = 0;  

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
                Reports.rptPurchaseOrderHistory oRpt =new rptPurchaseOrderHistory();
				string sSQL = " SELECT " +
									"  po.orderno,v.vendorname,po.userid,po.orderdate,po.exptdeliverydate,  " +
                                    " case po.status when 0 then 'Incomplete' when 1 then 'Pending' when 2 then 'Queued' when 3 then 'Submitted' when 4 then 'Canceled' when 5 then 'Acknowledge' when 6 then 'AcknowledgeManually' when 7 then 'MaxAttempt' when 8 then 'Processed' when 9 then 'Expired' when 10 then 'PartiallyAck' when 11 then 'PartiallyAck-Reorder' when 12 then 'Error' When 13 then 'Overdue' when 15 then 'DirectAcknowledge' when 16 then 'DeliveryReceived' when 17 then 'DirectDelivery'  end as Status , " + //Change by SRT (Sachin) Date : 19 Feb 2010
									" pod.itemid,i.description, pod.qty, pod.cost, pod.comments " +
									" from purchaseorder po, purchaseorderdetail pod,item i, vendor v " +
									" where po.orderid=pod.orderid and pod.itemid=i.itemid and v.vendorid=po.vendorid ";
                sSQL = sSQL + buildCriteria();
				clsReports.Preview(PrintId,sSQL,oRpt);
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
				sCriteria = sCriteria + " and  convert(datetime,Po.OrderDate,109) between convert(datetime, cast('" + this.dtpFromDate.Text  + " 00:00:00' as datetime) ,113) and convert(datetime, cast('" + this.dtpToDate.Text  + " 23:59:59' as datetime) ,113) ";
			if (txtVendor.Text.Trim().Replace("'","''")!="")
				sCriteria = sCriteria + " AND v.VendorCode = '" + txtVendor.Text + "' ";

            if (this.GetPOStatus != "")
                sCriteria = sCriteria + " and po.status= " + SetStatus(this.GetPOStatus);
           
            return sCriteria;
		}

        private string GetPOStatus
        {
            get { return poStatus; }
            set { poStatus = value;}        
        }

		private void btnClose_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}

		private void dtpToDate_BeforeDropDown(object sender, System.ComponentModel.CancelEventArgs e)
		{
            
		}

        private int SetStatus(string displaytext)
        {
            int status = (int)PurchseOrdStatus.All;           

            switch (displaytext)
            {
                case clsPOSDBConstants.Incomplete:
                    status = (int)PurchseOrdStatus.Incomplete;
                    break;
                case clsPOSDBConstants.Pending:
                    status = (int)PurchseOrdStatus.Pending;
                    break;
                case clsPOSDBConstants.Queued:
                    status = (int)PurchseOrdStatus.Queued;
                    break;
                case clsPOSDBConstants.Submitted:
                    status = (int)PurchseOrdStatus.Submitted;
                    break;
                case clsPOSDBConstants.Canceled:
                    status = (int)PurchseOrdStatus.Canceled;
                    break;
                case clsPOSDBConstants.Acknowledge:
                    status = (int)PurchseOrdStatus.Acknowledge;
                    break;
                case clsPOSDBConstants.AcknowledgeManually:
                    status = (int)PurchseOrdStatus.AcknowledgeManually;
                    break;
                case clsPOSDBConstants.MaxAttempt:
                    status = (int)PurchseOrdStatus.MaxAttempt;
                    break;
                case clsPOSDBConstants.Processed:
                    status = (int)PurchseOrdStatus.Processed;
                    break;
                case clsPOSDBConstants.Expired:
                    status = (int)PurchseOrdStatus.Expired;
                    break;
                case clsPOSDBConstants.All:
                    status = (int)PurchseOrdStatus.All;
                    break;
                case clsPOSDBConstants.Error:
                    status = (int)PurchseOrdStatus.Error;
                    break;
                case clsPOSDBConstants.PartiallyAckReorder:
                    status = (int)PurchseOrdStatus.PartiallyAckReorder;
                    break;
                case clsPOSDBConstants.Overdue:
                    status = (int)PurchseOrdStatus.Overdue;
                    break;
                    //Add by SRT(Sachin) Date : 18 Feb 2010
                case clsPOSDBConstants.DirectAcknowledge:
                    status = (int)PurchseOrdStatus.DirectAcknowledge;
                    break;
                    //End of Add by SRT(Sachin) Date : 18 Feb 2010
            }
            return status;            
        }

		private void frmRptPurchaseOrder_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
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

		private void gbInventoryReceived_Enter_1(object sender, System.EventArgs e)
		{
		
		}
        private bool validateFields(out string fieldName)
        {
            bool isValid = true;
            string field = string.Empty;
            try
            {
                if ((DateTime)dtpFromDate.Value > (DateTime)dtpToDate.Value)
                {
                    isValid = false;
                    fieldName = "DATE";
                    return isValid;
                }
            }
            catch (Exception ex)
            {
                clsUIHelper.ShowErrorMsg(ex.Message);
            }
            fieldName = field;
            return isValid;
        }
		private void btnView_Click(object sender, System.EventArgs e)
		{
             string fieldName = string.Empty;
             try
             {
                 if (!validateFields(out fieldName))
                 {
                     if (fieldName == "DATE")
                     {
                         clsUIHelper.ShowErrorMsg("From Date can not be greater than To Date.");
                     }

                     return;
                 }
                 this.dtpFromDate.Focus();
                 Preview(false);
             }
             catch (Exception ex)
             {
                 clsUIHelper.ShowErrorMsg(ex.Message);
             }
		}

		private void ultraButton1_Click(object sender, System.EventArgs e)
		{
			this.dtpFromDate.Focus();
			Preview(true);
		}

		private void txtVendor_EditorButtonClick(object sender, Infragistics.Win.UltraWinEditors.EditorButtonEventArgs e)
		{
            //frmSearch oSearch = new frmSearch(clsPOSDBConstants.Vendor_tbl);
            frmSearchMain oSearch = new frmSearchMain(true);    //20-Dec-2017 JY Added new reference
            oSearch.SearchTable = clsPOSDBConstants.Vendor_tbl; //20-Dec-2017 JY Added 
            oSearch.ShowDialog();
			if (oSearch.IsCanceled) return;
			txtVendor.Text = oSearch.SelectedRowID();
		}

		private void frmRptPurchaseOrder_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			try
			{
				if (e.KeyData==System.Windows.Forms.Keys.F4)
				{
					if ( txtVendor.ContainsFocus)
						txtVendor_EditorButtonClick(null,null);
				}
			}
			catch(Exception exp)
			{
				clsUIHelper.ShowErrorMsg(exp.Message);
			}
		}

        private void cboStatusList_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if(cboStatusList.SelectedItem.DisplayText == "ALL")
            {
                this.GetPOStatus = "";
            }
            else
            {
                this.GetPOStatus = cboStatusList.SelectedItem.DisplayText;
            }
        }

        private void dtpFromDate_ValueChanged(object sender, EventArgs e)
        {
            string fieldName = string.Empty;
            try
            {
                if (!validateFields(out fieldName))
                {
                    if (fieldName == "DATE")
                    {
                        clsUIHelper.ShowErrorMsg("From Date can not be greater than To Date.");
                    }

                    return;
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void dtpToDate_ValueChanged(object sender, EventArgs e)
        {
            string fieldName = string.Empty;
            try
            {
                if (!validateFields(out fieldName))
                {
                    if (fieldName == "DATE")
                    {
                        clsUIHelper.ShowErrorMsg("From Date can not be greater than To Date.");
                    }

                    return;
                }
            }
            catch (Exception ex)
            {
            }
        }
	}
}
