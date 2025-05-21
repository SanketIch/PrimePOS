using System;
using System.Data;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using POS_Core.CommonData;
using POS_Core_UI.Reports.Reports;
using CrystalDecisions.Shared;
using CrystalDecisions.ReportSource;
using CrystalDecisions.CrystalReports.Engine;
//using POS_Core.DataAccess;
//using POS.UI;

namespace POS_Core_UI.Reports.ReportsUI
{
	/// <summary>
	/// Summary description for frmInventoryReports.
	/// </summary>
	public class frmRptCustomerList : System.Windows.Forms.Form
	{
        private System.Windows.Forms.GroupBox gbItemFileListing;
		private Infragistics.Win.UltraWinEditors.UltraTextEditor txtState;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor txtCity;
		private Infragistics.Win.Misc.UltraLabel ultraLabel2;
		private Infragistics.Win.Misc.UltraLabel ultraLabel1;
		private System.Windows.Forms.GroupBox ultraGroupBox2;
		private Infragistics.Win.UltraWinEditors.UltraOptionSet optSortByName;
		private System.Windows.Forms.GroupBox groupBox2;
		private Infragistics.Win.Misc.UltraButton btnPrint;
		private Infragistics.Win.Misc.UltraButton btnView;
		private Infragistics.Win.Misc.UltraButton btnClose;
		private Infragistics.Win.Misc.UltraLabel lblTransactionType;
        private CheckBox chkLoyaltyCustomersOnly;
        private Infragistics.Win.UltraWinEditors.UltraOptionSet optGender;
        private Infragistics.Win.Misc.UltraLabel ultraLabel3;
        private Infragistics.Win.UltraWinEditors.UltraNumericEditor txtMonth;
        private Infragistics.Win.Misc.UltraLabel ultraLabel4;
        private Infragistics.Win.UltraWinEditors.UltraNumericEditor txtDay;
        private Infragistics.Win.Misc.UltraLabel ultraLabel6;
        private Infragistics.Win.Misc.UltraLabel ultraLabel5;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public frmRptCustomerList()
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
            Infragistics.Win.ValueListItem valueListItem1 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem2 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem3 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
            Infragistics.Win.ValueListItem valueListItem4 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem5 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmRptCustomerList));
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            this.gbItemFileListing = new System.Windows.Forms.GroupBox();
            this.txtDay = new Infragistics.Win.UltraWinEditors.UltraNumericEditor();
            this.ultraLabel6 = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabel5 = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabel4 = new Infragistics.Win.Misc.UltraLabel();
            this.txtMonth = new Infragistics.Win.UltraWinEditors.UltraNumericEditor();
            this.ultraLabel3 = new Infragistics.Win.Misc.UltraLabel();
            this.optGender = new Infragistics.Win.UltraWinEditors.UltraOptionSet();
            this.chkLoyaltyCustomersOnly = new System.Windows.Forms.CheckBox();
            this.txtState = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.txtCity = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.ultraLabel2 = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabel1 = new Infragistics.Win.Misc.UltraLabel();
            this.ultraGroupBox2 = new System.Windows.Forms.GroupBox();
            this.optSortByName = new Infragistics.Win.UltraWinEditors.UltraOptionSet();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnPrint = new Infragistics.Win.Misc.UltraButton();
            this.btnClose = new Infragistics.Win.Misc.UltraButton();
            this.btnView = new Infragistics.Win.Misc.UltraButton();
            this.lblTransactionType = new Infragistics.Win.Misc.UltraLabel();
            this.gbItemFileListing.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtDay)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtMonth)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.optGender)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtState)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCity)).BeginInit();
            this.ultraGroupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.optSortByName)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbItemFileListing
            // 
            this.gbItemFileListing.Controls.Add(this.txtDay);
            this.gbItemFileListing.Controls.Add(this.ultraLabel6);
            this.gbItemFileListing.Controls.Add(this.ultraLabel5);
            this.gbItemFileListing.Controls.Add(this.ultraLabel4);
            this.gbItemFileListing.Controls.Add(this.txtMonth);
            this.gbItemFileListing.Controls.Add(this.ultraLabel3);
            this.gbItemFileListing.Controls.Add(this.optGender);
            this.gbItemFileListing.Controls.Add(this.chkLoyaltyCustomersOnly);
            this.gbItemFileListing.Controls.Add(this.txtState);
            this.gbItemFileListing.Controls.Add(this.txtCity);
            this.gbItemFileListing.Controls.Add(this.ultraLabel2);
            this.gbItemFileListing.Controls.Add(this.ultraLabel1);
            this.gbItemFileListing.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbItemFileListing.Location = new System.Drawing.Point(15, 53);
            this.gbItemFileListing.Name = "gbItemFileListing";
            this.gbItemFileListing.Size = new System.Drawing.Size(424, 167);
            this.gbItemFileListing.TabIndex = 0;
            this.gbItemFileListing.TabStop = false;
            this.gbItemFileListing.Text = "Criteria";
            // 
            // txtDay
            // 
            this.txtDay.DisplayStyle = Infragistics.Win.EmbeddableElementDisplayStyle.OfficeXP;
            this.txtDay.Location = new System.Drawing.Point(204, 137);
            this.txtDay.MaskInput = "nn";
            this.txtDay.MaxValue = 31;
            this.txtDay.MinValue = 0;
            this.txtDay.Name = "txtDay";
            this.txtDay.Size = new System.Drawing.Size(30, 22);
            this.txtDay.TabIndex = 5;
            // 
            // ultraLabel6
            // 
            this.ultraLabel6.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultraLabel6.Location = new System.Drawing.Point(175, 141);
            this.ultraLabel6.Name = "ultraLabel6";
            this.ultraLabel6.Size = new System.Drawing.Size(42, 15);
            this.ultraLabel6.TabIndex = 13;
            this.ultraLabel6.Text = "Day";
            // 
            // ultraLabel5
            // 
            this.ultraLabel5.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultraLabel5.Location = new System.Drawing.Point(94, 141);
            this.ultraLabel5.Name = "ultraLabel5";
            this.ultraLabel5.Size = new System.Drawing.Size(42, 15);
            this.ultraLabel5.TabIndex = 12;
            this.ultraLabel5.Text = "Month";
            // 
            // ultraLabel4
            // 
            this.ultraLabel4.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultraLabel4.Location = new System.Drawing.Point(23, 141);
            this.ultraLabel4.Name = "ultraLabel4";
            this.ultraLabel4.Size = new System.Drawing.Size(67, 15);
            this.ultraLabel4.TabIndex = 10;
            this.ultraLabel4.Text = "DOB";
            // 
            // txtMonth
            // 
            this.txtMonth.DisplayStyle = Infragistics.Win.EmbeddableElementDisplayStyle.OfficeXP;
            this.txtMonth.Location = new System.Drawing.Point(136, 137);
            this.txtMonth.MaskInput = "nn";
            this.txtMonth.MaxValue = 12;
            this.txtMonth.MinValue = 0;
            this.txtMonth.Name = "txtMonth";
            this.txtMonth.Size = new System.Drawing.Size(30, 22);
            this.txtMonth.TabIndex = 4;
            // 
            // ultraLabel3
            // 
            this.ultraLabel3.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultraLabel3.Location = new System.Drawing.Point(23, 114);
            this.ultraLabel3.Name = "ultraLabel3";
            this.ultraLabel3.Size = new System.Drawing.Size(67, 15);
            this.ultraLabel3.TabIndex = 8;
            this.ultraLabel3.Text = "Gender";
            // 
            // optGender
            // 
            this.optGender.BorderStyle = Infragistics.Win.UIElementBorderStyle.None;
            this.optGender.CheckedIndex = 2;
            appearance1.FontData.BoldAsString = "False";
            this.optGender.ItemAppearance = appearance1;
            this.optGender.ItemOrigin = new System.Drawing.Point(0, 2);
            valueListItem1.DataValue = clsPOSDBConstants.BINARYIMAGE;
            valueListItem1.DisplayText = "Male";
            valueListItem2.DataValue = "F";
            valueListItem2.DisplayText = "Female";
            valueListItem3.DataValue = "B";
            valueListItem3.DisplayText = "Both";
            this.optGender.Items.AddRange(new Infragistics.Win.ValueListItem[] {
            valueListItem1,
            valueListItem2,
            valueListItem3});
            this.optGender.ItemSpacingHorizontal = 30;
            this.optGender.Location = new System.Drawing.Point(94, 111);
            this.optGender.Name = "optGender";
            this.optGender.Size = new System.Drawing.Size(264, 20);
            this.optGender.TabIndex = 3;
            this.optGender.Text = "Both";
            this.optGender.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            // 
            // chkLoyaltyCustomersOnly
            // 
            this.chkLoyaltyCustomersOnly.AutoSize = true;
            this.chkLoyaltyCustomersOnly.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chkLoyaltyCustomersOnly.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkLoyaltyCustomersOnly.Location = new System.Drawing.Point(94, 82);
            this.chkLoyaltyCustomersOnly.Name = "chkLoyaltyCustomersOnly";
            this.chkLoyaltyCustomersOnly.Size = new System.Drawing.Size(160, 17);
            this.chkLoyaltyCustomersOnly.TabIndex = 2;
            this.chkLoyaltyCustomersOnly.Text = "Loyalty Customers Only";
            this.chkLoyaltyCustomersOnly.UseVisualStyleBackColor = true;
            // 
            // txtState
            // 
            this.txtState.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.txtState.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtState.Location = new System.Drawing.Point(94, 52);
            this.txtState.MaxLength = 20;
            this.txtState.Name = "txtState";
            this.txtState.Size = new System.Drawing.Size(123, 20);
            this.txtState.TabIndex = 1;
            // 
            // txtCity
            // 
            this.txtCity.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.txtCity.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCity.Location = new System.Drawing.Point(94, 23);
            this.txtCity.MaxLength = 50;
            this.txtCity.Name = "txtCity";
            this.txtCity.Size = new System.Drawing.Size(123, 20);
            this.txtCity.TabIndex = 0;
            // 
            // ultraLabel2
            // 
            this.ultraLabel2.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultraLabel2.Location = new System.Drawing.Point(23, 55);
            this.ultraLabel2.Name = "ultraLabel2";
            this.ultraLabel2.Size = new System.Drawing.Size(91, 15);
            this.ultraLabel2.TabIndex = 2;
            this.ultraLabel2.Text = "State";
            // 
            // ultraLabel1
            // 
            this.ultraLabel1.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultraLabel1.Location = new System.Drawing.Point(23, 27);
            this.ultraLabel1.Name = "ultraLabel1";
            this.ultraLabel1.Size = new System.Drawing.Size(91, 15);
            this.ultraLabel1.TabIndex = 1;
            this.ultraLabel1.Text = "City";
            // 
            // ultraGroupBox2
            // 
            this.ultraGroupBox2.Controls.Add(this.optSortByName);
            this.ultraGroupBox2.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultraGroupBox2.Location = new System.Drawing.Point(15, 225);
            this.ultraGroupBox2.Name = "ultraGroupBox2";
            this.ultraGroupBox2.Size = new System.Drawing.Size(424, 50);
            this.ultraGroupBox2.TabIndex = 1;
            this.ultraGroupBox2.TabStop = false;
            this.ultraGroupBox2.Text = "Sort Item By";
            this.ultraGroupBox2.Enter += new System.EventHandler(this.ultraGroupBox2_Enter_1);
            // 
            // optSortByName
            // 
            this.optSortByName.BorderStyle = Infragistics.Win.UIElementBorderStyle.None;
            this.optSortByName.CheckedIndex = 0;
            appearance6.FontData.BoldAsString = "False";
            this.optSortByName.ItemAppearance = appearance6;
            this.optSortByName.ItemOrigin = new System.Drawing.Point(0, 2);
            valueListItem4.DataValue = 1;
            valueListItem4.DisplayText = "Name";
            valueListItem5.DataValue = 2;
            valueListItem5.DisplayText = "Code";
            this.optSortByName.Items.AddRange(new Infragistics.Win.ValueListItem[] {
            valueListItem4,
            valueListItem5});
            this.optSortByName.ItemSpacingHorizontal = 50;
            this.optSortByName.Location = new System.Drawing.Point(94, 18);
            this.optSortByName.Name = "optSortByName";
            this.optSortByName.Size = new System.Drawing.Size(182, 20);
            this.optSortByName.TabIndex = 6;
            this.optSortByName.Text = "Name";
            this.optSortByName.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnPrint);
            this.groupBox2.Controls.Add(this.btnClose);
            this.groupBox2.Controls.Add(this.btnView);
            this.groupBox2.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.groupBox2.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.Location = new System.Drawing.Point(15, 276);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(424, 57);
            this.groupBox2.TabIndex = 31;
            this.groupBox2.TabStop = false;
            // 
            // btnPrint
            // 
            appearance2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(62)))), ((int)(((byte)(176)))));
            appearance2.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance2.FontData.BoldAsString = "True";
            appearance2.ForeColor = System.Drawing.Color.White;
            appearance2.Image = ((object)(resources.GetObject("appearance2.Image")));
            this.btnPrint.Appearance = appearance2;
            this.btnPrint.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            this.btnPrint.Location = new System.Drawing.Point(142, 19);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(85, 26);
            this.btnPrint.TabIndex = 8;
            this.btnPrint.Text = "&Print";
            this.btnPrint.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click_1);
            // 
            // btnClose
            // 
            appearance3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(62)))), ((int)(((byte)(176)))));
            appearance3.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance3.FontData.BoldAsString = "True";
            appearance3.ForeColor = System.Drawing.Color.White;
            appearance3.Image = ((object)(resources.GetObject("appearance3.Image")));
            this.btnClose.Appearance = appearance3;
            this.btnClose.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(326, 20);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(85, 26);
            this.btnClose.TabIndex = 9;
            this.btnClose.Text = "&Close";
            this.btnClose.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnView
            // 
            appearance4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(62)))), ((int)(((byte)(176)))));
            appearance4.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance4.FontData.BoldAsString = "True";
            appearance4.ForeColor = System.Drawing.Color.White;
            appearance4.Image = ((object)(resources.GetObject("appearance4.Image")));
            this.btnView.Appearance = appearance4;
            this.btnView.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            this.btnView.Location = new System.Drawing.Point(234, 20);
            this.btnView.Name = "btnView";
            this.btnView.Size = new System.Drawing.Size(85, 26);
            this.btnView.TabIndex = 7;
            this.btnView.Text = "&View";
            this.btnView.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnView.Click += new System.EventHandler(this.btnView_Click);
            // 
            // lblTransactionType
            // 
            appearance5.ForeColor = System.Drawing.Color.White;
            appearance5.ForeColorDisabled = System.Drawing.Color.White;
            appearance5.TextHAlignAsString = "Center";
            this.lblTransactionType.Appearance = appearance5;
            this.lblTransactionType.BackColorInternal = System.Drawing.Color.Transparent;
            this.lblTransactionType.Font = new System.Drawing.Font("Arial", 18F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTransactionType.Location = new System.Drawing.Point(30, 17);
            this.lblTransactionType.Name = "lblTransactionType";
            this.lblTransactionType.Size = new System.Drawing.Size(385, 30);
            this.lblTransactionType.TabIndex = 32;
            this.lblTransactionType.Text = "Customer File List";
            // 
            // frmRptCustomerList
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(455, 351);
            this.Controls.Add(this.lblTransactionType);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.gbItemFileListing);
            this.Controls.Add(this.ultraGroupBox2);
            this.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmRptCustomerList";
            this.Text = "Customer File List";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmRptVendor_KeyDown);
            this.Load += new System.EventHandler(this.frmInventoryReports_Load);
            this.gbItemFileListing.ResumeLayout(false);
            this.gbItemFileListing.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtDay)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtMonth)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.optGender)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtState)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCity)).EndInit();
            this.ultraGroupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.optSortByName)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);

		}
		#endregion

		private void frmInventoryReports_Load(object sender, System.EventArgs e)
		{
			clsUIHelper.setColorSchecme(this);

			this.txtCity.AfterEnterEditMode += new System.EventHandler(clsUIHelper.AfterEnterEditMode);			
			this.txtCity.AfterExitEditMode += new System.EventHandler(clsUIHelper.AfterExitEditMode);			

			this.txtState.AfterEnterEditMode += new System.EventHandler(clsUIHelper.AfterEnterEditMode);			
			this.txtState.AfterExitEditMode += new System.EventHandler(clsUIHelper.AfterExitEditMode);			

		}


		private void btnPreview_Click(object sender, System.EventArgs e)
		{
			Preview(false);
		}


		private void Preview(bool PrintId)
		{
			try
			{
				rptCustomerList oRpt = new rptCustomerList();
				string sSQL = " SELECT " +
									" Acctnumber " +
									" 	, CustomerName + ',' + FirstName as CustomerName " +
									" 	, Address1 + ' ' + Address2 + ', ' + City + ', ' + State as Address  " +
                                    " 	, case PrimaryContact When 0 then Cellno When 1 then PhoneOff When 2  then PhoneHome End as PrimaryContact " +
									" 	, FaxNo  " +
									" 	, Case Gender When 0 Then 'Male' Else 'Female' End as Gender " +
                                    " 	, CustomerCode " +
                                    " 	, Convert(varchar,DateOfBirth,110) as DateOfBirth  " +
								" FROM " +
									" Customer Where AcctNumber<>-1 ";

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
			
			if (txtCity.Text.Trim().Replace("'","''") != "")
                sCriteria += " AND City LIKE '" + txtCity.Text.Trim().Replace("'", "''") + "%'";    //PRIMEPOS-2987 30-Jul-2021 JY Modified
            if (txtState.Text.Trim().Replace("'","''") != "")
                sCriteria += " AND State LIKE '" + txtState.Text.Trim().Replace("'", "''") + "%'";  //PRIMEPOS-2987 30-Jul-2021 JY Modified
            if (chkLoyaltyCustomersOnly.Checked == true)
                sCriteria += " and UseForCustomerLoyalty = 1";
            if (optGender.Value.ToString() == clsPOSDBConstants.BINARYIMAGE)
                sCriteria += " and Gender = 0";
            else if (optGender.Value.ToString() == "F")
                sCriteria +=  " and Gender = 1";

            if (txtMonth.Value.ToString().Length > 0 && txtDay.Value.ToString().Length > 0)
            {
                if (txtMonth.Value.ToString() != "0" && txtDay.Value.ToString() != "0")
                {
                    sCriteria += " and Convert(varchar,DateOfBirth,110) Like '%" + txtMonth.Value.ToString() + "-%" + txtDay.Value.ToString() + "-%'";
                }
            }
			sCriteria += " Order By " + ((optSortByName.CheckedIndex == 1) ? " AcctNumber " : " CustomerName, FirstName ");

			return sCriteria;
		}
		private void btnPrint_Click(object sender, System.EventArgs e)
		{
			Preview(true);
		}

		private void btnClose_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}

		private void frmRptVendor_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
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

		private void gbItemFileListing_Enter(object sender, System.EventArgs e)
		{
			txtCity.Focus();
		}

		private void ultraGroupBox2_Enter(object sender, System.EventArgs e)
		{
			optSortByName.Focus();
		}

		private void btnView_Click(object sender, System.EventArgs e)
		{
			Preview(false);
		}

		private void btnPrint_Click_1(object sender, System.EventArgs e)
		{
			Preview(true);
		}

        private void ultraGroupBox2_Enter_1(object sender, EventArgs e)
        {

        }

	}
}
