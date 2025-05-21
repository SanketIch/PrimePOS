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
	public class frmRptVendor : System.Windows.Forms.Form
	{
		private System.Windows.Forms.GroupBox gbItemFileListing;
		private Infragistics.Win.UltraWinEditors.UltraTextEditor txtZip;
		private Infragistics.Win.UltraWinEditors.UltraTextEditor txtState;
		private Infragistics.Win.UltraWinEditors.UltraTextEditor txtCity;
		private Infragistics.Win.Misc.UltraLabel ultraLabel3;
		private Infragistics.Win.Misc.UltraLabel ultraLabel2;
		private Infragistics.Win.Misc.UltraLabel ultraLabel1;
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

		public frmRptVendor()
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
			Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(frmRptVendor));
			Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
			Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
			Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
			this.gbItemFileListing = new System.Windows.Forms.GroupBox();
			this.txtZip = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
			this.txtState = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
			this.txtCity = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
			this.ultraLabel3 = new Infragistics.Win.Misc.UltraLabel();
			this.ultraLabel2 = new Infragistics.Win.Misc.UltraLabel();
			this.ultraLabel1 = new Infragistics.Win.Misc.UltraLabel();
			this.ultraGroupBox2 = new System.Windows.Forms.GroupBox();
			this.optByName = new Infragistics.Win.UltraWinEditors.UltraOptionSet();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.btnPrint = new Infragistics.Win.Misc.UltraButton();
			this.btnClose = new Infragistics.Win.Misc.UltraButton();
			this.btnView = new Infragistics.Win.Misc.UltraButton();
			this.lblTransactionType = new Infragistics.Win.Misc.UltraLabel();
			this.gbItemFileListing.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.txtZip)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.txtState)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.txtCity)).BeginInit();
			this.ultraGroupBox2.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.optByName)).BeginInit();
			this.groupBox2.SuspendLayout();
			this.SuspendLayout();
			// 
			// gbItemFileListing
			// 
			this.gbItemFileListing.Controls.Add(this.txtZip);
			this.gbItemFileListing.Controls.Add(this.txtState);
			this.gbItemFileListing.Controls.Add(this.txtCity);
			this.gbItemFileListing.Controls.Add(this.ultraLabel3);
			this.gbItemFileListing.Controls.Add(this.ultraLabel2);
			this.gbItemFileListing.Controls.Add(this.ultraLabel1);
			this.gbItemFileListing.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.gbItemFileListing.Location = new System.Drawing.Point(15, 120);
			this.gbItemFileListing.Name = "gbItemFileListing";
			this.gbItemFileListing.Size = new System.Drawing.Size(424, 118);
			this.gbItemFileListing.TabIndex = 3;
			this.gbItemFileListing.TabStop = false;
			this.gbItemFileListing.Text = "Vendor";
			// 
			// txtZip
			// 
			this.txtZip.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
			this.txtZip.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.txtZip.Location = new System.Drawing.Point(94, 80);
			this.txtZip.MaxLength = 20;
			this.txtZip.Name = "txtZip";
			this.txtZip.Size = new System.Drawing.Size(123, 20);
			this.txtZip.TabIndex = 6;
			// 
			// txtState
			// 
			this.txtState.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
			this.txtState.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.txtState.Location = new System.Drawing.Point(94, 52);
			this.txtState.MaxLength = 20;
			this.txtState.Name = "txtState";
			this.txtState.Size = new System.Drawing.Size(123, 20);
			this.txtState.TabIndex = 5;
			// 
			// txtCity
			// 
			this.txtCity.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
			this.txtCity.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.txtCity.Location = new System.Drawing.Point(94, 23);
			this.txtCity.MaxLength = 50;
			this.txtCity.Name = "txtCity";
			this.txtCity.Size = new System.Drawing.Size(123, 20);
			this.txtCity.TabIndex = 4;
			// 
			// ultraLabel3
			// 
			this.ultraLabel3.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.ultraLabel3.Location = new System.Drawing.Point(24, 83);
			this.ultraLabel3.Name = "ultraLabel3";
			this.ultraLabel3.Size = new System.Drawing.Size(91, 15);
			this.ultraLabel3.TabIndex = 3;
			this.ultraLabel3.Text = "Zip";
			// 
			// ultraLabel2
			// 
			this.ultraLabel2.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.ultraLabel2.Location = new System.Drawing.Point(24, 55);
			this.ultraLabel2.Name = "ultraLabel2";
			this.ultraLabel2.Size = new System.Drawing.Size(91, 15);
			this.ultraLabel2.TabIndex = 2;
			this.ultraLabel2.Text = "State";
			// 
			// ultraLabel1
			// 
			this.ultraLabel1.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.ultraLabel1.Location = new System.Drawing.Point(24, 27);
			this.ultraLabel1.Name = "ultraLabel1";
			this.ultraLabel1.Size = new System.Drawing.Size(91, 15);
			this.ultraLabel1.TabIndex = 1;
			this.ultraLabel1.Text = "City";
			// 
			// ultraGroupBox2
			// 
			this.ultraGroupBox2.Controls.Add(this.optByName);
			this.ultraGroupBox2.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.ultraGroupBox2.Location = new System.Drawing.Point(15, 60);
			this.ultraGroupBox2.Name = "ultraGroupBox2";
			this.ultraGroupBox2.Size = new System.Drawing.Size(424, 50);
			this.ultraGroupBox2.TabIndex = 1;
			this.ultraGroupBox2.TabStop = false;
			this.ultraGroupBox2.Text = "Sort Item By";
			// 
			// optByName
			// 
			this.optByName.BorderStyle = Infragistics.Win.UIElementBorderStyle.None;
			this.optByName.CheckedIndex = 0;
			this.optByName.FlatMode = true;
			appearance1.FontData.BoldAsString = "False";
			this.optByName.ItemAppearance = appearance1;
			this.optByName.ItemOrigin = new System.Drawing.Point(0, 2);
			valueListItem1.DataValue = 1;
			valueListItem1.DisplayText = "Name";
			valueListItem2.DataValue = 2;
			valueListItem2.DisplayText = "Code";
			this.optByName.Items.Add(valueListItem1);
			this.optByName.Items.Add(valueListItem2);
			this.optByName.ItemSpacingHorizontal = 50;
			this.optByName.Location = new System.Drawing.Point(94, 18);
			this.optByName.Name = "optByName";
			this.optByName.Size = new System.Drawing.Size(182, 20);
			this.optByName.TabIndex = 2;
			this.optByName.Text = "Name";
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this.btnPrint);
			this.groupBox2.Controls.Add(this.btnClose);
			this.groupBox2.Controls.Add(this.btnView);
			this.groupBox2.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.groupBox2.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.groupBox2.Location = new System.Drawing.Point(15, 244);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(424, 57);
			this.groupBox2.TabIndex = 31;
			this.groupBox2.TabStop = false;
			// 
			// btnPrint
			// 
			appearance2.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(52)), ((System.Byte)(62)), ((System.Byte)(176)));
			appearance2.BackColor2 = System.Drawing.Color.FromArgb(((System.Byte)(65)), ((System.Byte)(129)), ((System.Byte)(247)));
			appearance2.FontData.BoldAsString = "True";
			appearance2.ForeColor = System.Drawing.Color.White;
			appearance2.Image = ((object)(resources.GetObject("appearance2.Image")));
			this.btnPrint.Appearance = appearance2;
			this.btnPrint.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
			this.btnPrint.Location = new System.Drawing.Point(142, 19);
			this.btnPrint.Name = "btnPrint";
			this.btnPrint.Size = new System.Drawing.Size(85, 26);
			this.btnPrint.SupportThemes = false;
			this.btnPrint.TabIndex = 6;
			this.btnPrint.Text = "&Print";
			this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click_1);
			// 
			// btnClose
			// 
			appearance3.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(52)), ((System.Byte)(62)), ((System.Byte)(176)));
			appearance3.BackColor2 = System.Drawing.Color.FromArgb(((System.Byte)(65)), ((System.Byte)(129)), ((System.Byte)(247)));
			appearance3.FontData.BoldAsString = "True";
			appearance3.ForeColor = System.Drawing.Color.White;
			appearance3.Image = ((object)(resources.GetObject("appearance3.Image")));
			this.btnClose.Appearance = appearance3;
			this.btnClose.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
			this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnClose.Location = new System.Drawing.Point(326, 20);
			this.btnClose.Name = "btnClose";
			this.btnClose.Size = new System.Drawing.Size(85, 26);
			this.btnClose.SupportThemes = false;
			this.btnClose.TabIndex = 7;
			this.btnClose.Text = "&Close";
			this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
			// 
			// btnView
			// 
			appearance4.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(52)), ((System.Byte)(62)), ((System.Byte)(176)));
			appearance4.BackColor2 = System.Drawing.Color.FromArgb(((System.Byte)(65)), ((System.Byte)(129)), ((System.Byte)(247)));
			appearance4.FontData.BoldAsString = "True";
			appearance4.ForeColor = System.Drawing.Color.White;
			appearance4.Image = ((object)(resources.GetObject("appearance4.Image")));
			this.btnView.Appearance = appearance4;
			this.btnView.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
			this.btnView.Location = new System.Drawing.Point(234, 20);
			this.btnView.Name = "btnView";
			this.btnView.Size = new System.Drawing.Size(85, 26);
			this.btnView.SupportThemes = false;
			this.btnView.TabIndex = 5;
			this.btnView.Text = "&View";
			this.btnView.Click += new System.EventHandler(this.btnView_Click);
			// 
			// lblTransactionType
			// 
			appearance5.ForeColor = System.Drawing.Color.White;
			appearance5.ForeColorDisabled = System.Drawing.Color.White;
			appearance5.TextHAlign = Infragistics.Win.HAlign.Center;
			this.lblTransactionType.Appearance = appearance5;
			this.lblTransactionType.BackColor = System.Drawing.Color.Transparent;
			this.lblTransactionType.Font = new System.Drawing.Font("Arial", 18F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.lblTransactionType.Location = new System.Drawing.Point(30, 17);
			this.lblTransactionType.Name = "lblTransactionType";
			this.lblTransactionType.Size = new System.Drawing.Size(385, 30);
			this.lblTransactionType.TabIndex = 32;
			this.lblTransactionType.Text = "Vendor File List";
			// 
			// frmRptVendor
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
			this.CancelButton = this.btnClose;
			this.ClientSize = new System.Drawing.Size(455, 312);
			this.Controls.Add(this.lblTransactionType);
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.gbItemFileListing);
			this.Controls.Add(this.ultraGroupBox2);
			this.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.KeyPreview = true;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "frmRptVendor";
			this.Text = "Vendor File List";
			this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmRptVendor_KeyDown);
			this.Load += new System.EventHandler(this.frmInventoryReports_Load);
			this.gbItemFileListing.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.txtZip)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.txtState)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.txtCity)).EndInit();
			this.ultraGroupBox2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.optByName)).EndInit();
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

			this.txtZip.AfterEnterEditMode += new System.EventHandler(clsUIHelper.AfterEnterEditMode);			
			this.txtZip.AfterExitEditMode += new System.EventHandler(clsUIHelper.AfterExitEditMode);			
		}


		private void btnPreview_Click(object sender, System.EventArgs e)
		{
			Preview(false);
		}


		private void Preview(bool PrintId)
		{
			try
			{
				 rptIrVendor oRpt = new rptIrVendor();
				string sSQL = " SELECT " +
									" VendorCode " +
									" , VendorName " +
									" , IsNull(Address1,'') + ' ' + IsNull(Address2,'') + ', ' + IsNull(City,'') + ', ' + IsNull(State,'') Address " +
									" , PhoneOff " +
									" , FaxNo  " +
								" FROM " +
									" Vendor ";

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
			
			if (txtCity.Text.Trim().Replace("'","''")!="")
				sCriteria = sCriteria + ((sCriteria=="")? " WHERE " : " AND ") + " City = '" + txtCity.Text + "'";
			if (txtState.Text.Trim().Replace("'","''")!="")
				sCriteria = sCriteria + ((sCriteria=="")? " WHERE " : " AND ") + " State = '" + txtState.Text + "'";
			if (txtZip.Text.Trim().Replace("'","''")!="")
				sCriteria = sCriteria + ((sCriteria=="")? " WHERE " : " AND ") + " Zip = '" + txtZip.Text + "'";
			
			sCriteria = sCriteria + " Order By " + ((optByName.CheckedIndex==1)? " VendorCode " : " VendorName ");

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
			optByName.Focus();
		}

		private void btnView_Click(object sender, System.EventArgs e)
		{
			this.optByName.Focus();
			Preview(false);
		}

		private void btnPrint_Click_1(object sender, System.EventArgs e)
		{
			this.optByName.Focus();
			Preview(true);
		}

	}
}
