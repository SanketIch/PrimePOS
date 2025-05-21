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
	/// Summary description for frmViewTransaction.
	/// </summary>
	public class frmRptProductivityReport : System.Windows.Forms.Form
	{
		private System.Windows.Forms.GroupBox groupBox1;
		private Infragistics.Win.Misc.UltraLabel lblTransactionType;
		private Infragistics.Win.Misc.UltraLabel ultraLabel21;
		private Infragistics.Win.UltraWinEditors.UltraTextEditor txtStationID;
		private Infragistics.Win.Misc.UltraLabel ultraLabel20;
		private Infragistics.Win.Misc.UltraLabel ultraLabel19;
		private Infragistics.Win.UltraWinEditors.UltraComboEditor cboTransType;
		private Infragistics.Win.Misc.UltraLabel ultraLabel2;
		private System.Windows.Forms.GroupBox groupBox2;
		private Infragistics.Win.Misc.UltraButton btnPrint;
		private Infragistics.Win.Misc.UltraButton btnView;
		private Infragistics.Win.Misc.UltraButton btnClose;
		private Infragistics.Win.UltraWinMaskedEdit.UltraMaskedEdit mskSaleStartDate;
		private Infragistics.Win.UltraWinMaskedEdit.UltraMaskedEdit mskSaleEndDate;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public frmRptProductivityReport()
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
            Infragistics.Win.Appearance appearance43 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance35 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance27 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance44 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance45 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance46 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance47 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance48 = new Infragistics.Win.Appearance();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmRptProductivityReport));
            Infragistics.Win.Appearance appearance49 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance50 = new Infragistics.Win.Appearance();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.mskSaleEndDate = new Infragistics.Win.UltraWinMaskedEdit.UltraMaskedEdit();
            this.mskSaleStartDate = new Infragistics.Win.UltraWinMaskedEdit.UltraMaskedEdit();
            this.ultraLabel2 = new Infragistics.Win.Misc.UltraLabel();
            this.cboTransType = new Infragistics.Win.UltraWinEditors.UltraComboEditor();
            this.ultraLabel20 = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabel19 = new Infragistics.Win.Misc.UltraLabel();
            this.txtStationID = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.ultraLabel21 = new Infragistics.Win.Misc.UltraLabel();
            this.lblTransactionType = new Infragistics.Win.Misc.UltraLabel();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnPrint = new Infragistics.Win.Misc.UltraButton();
            this.btnClose = new Infragistics.Win.Misc.UltraButton();
            this.btnView = new Infragistics.Win.Misc.UltraButton();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cboTransType)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtStationID)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.mskSaleEndDate);
            this.groupBox1.Controls.Add(this.mskSaleStartDate);
            this.groupBox1.Controls.Add(this.ultraLabel2);
            this.groupBox1.Controls.Add(this.cboTransType);
            this.groupBox1.Controls.Add(this.ultraLabel20);
            this.groupBox1.Controls.Add(this.ultraLabel19);
            this.groupBox1.Controls.Add(this.txtStationID);
            this.groupBox1.Controls.Add(this.ultraLabel21);
            this.groupBox1.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.groupBox1.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(12, 60);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(415, 189);
            this.groupBox1.TabIndex = 27;
            this.groupBox1.TabStop = false;
            // 
            // mskSaleEndDate
            // 
            appearance43.ForeColor = System.Drawing.Color.Black;
            this.mskSaleEndDate.Appearance = appearance43;
            this.mskSaleEndDate.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.mskSaleEndDate.DisplayStyle = Infragistics.Win.EmbeddableElementDisplayStyle.OfficeXP;
            this.mskSaleEndDate.EditAs = Infragistics.Win.UltraWinMaskedEdit.EditAsType.Date;
            this.mskSaleEndDate.Location = new System.Drawing.Point(230, 62);
            this.mskSaleEndDate.MaxValue = new System.DateTime(9999, 1, 1, 0, 0, 0, 0);
            this.mskSaleEndDate.MinValue = new System.DateTime(1901, 1, 1, 0, 0, 0, 0);
            this.mskSaleEndDate.Name = "mskSaleEndDate";
            this.mskSaleEndDate.NonAutoSizeHeight = 24;
            this.mskSaleEndDate.Size = new System.Drawing.Size(172, 24);
            this.mskSaleEndDate.SpinButtonDisplayStyle = Infragistics.Win.SpinButtonDisplayStyle.OnRight;
            this.mskSaleEndDate.TabIndex = 1;
            this.mskSaleEndDate.Text = "// : ";
            // 
            // mskSaleStartDate
            // 
            appearance35.ForeColor = System.Drawing.Color.Black;
            this.mskSaleStartDate.Appearance = appearance35;
            this.mskSaleStartDate.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.mskSaleStartDate.DisplayStyle = Infragistics.Win.EmbeddableElementDisplayStyle.OfficeXP;
            this.mskSaleStartDate.EditAs = Infragistics.Win.UltraWinMaskedEdit.EditAsType.Date;
            this.mskSaleStartDate.Location = new System.Drawing.Point(230, 33);
            this.mskSaleStartDate.MaxValue = new System.DateTime(9999, 1, 1, 0, 0, 0, 0);
            this.mskSaleStartDate.MinValue = new System.DateTime(1901, 1, 1, 0, 0, 0, 0);
            this.mskSaleStartDate.Name = "mskSaleStartDate";
            this.mskSaleStartDate.NonAutoSizeHeight = 24;
            this.mskSaleStartDate.Size = new System.Drawing.Size(172, 24);
            this.mskSaleStartDate.SpinButtonDisplayStyle = Infragistics.Win.SpinButtonDisplayStyle.OnRight;
            this.mskSaleStartDate.TabIndex = 0;
            this.mskSaleStartDate.Text = "//";
            // 
            // ultraLabel2
            // 
            appearance27.ForeColor = System.Drawing.Color.White;
            this.ultraLabel2.Appearance = appearance27;
            this.ultraLabel2.AutoSize = true;
            this.ultraLabel2.Location = new System.Drawing.Point(10, 126);
            this.ultraLabel2.Name = "ultraLabel2";
            this.ultraLabel2.Size = new System.Drawing.Size(123, 18);
            this.ultraLabel2.TabIndex = 23;
            this.ultraLabel2.Text = "Transaction Type";
            // 
            // cboTransType
            // 
            this.cboTransType.AutoSize = false;
            this.cboTransType.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.cboTransType.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Office2003ToolbarButton;
            this.cboTransType.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboTransType.LimitToList = true;
            this.cboTransType.Location = new System.Drawing.Point(230, 124);
            this.cboTransType.Name = "cboTransType";
            this.cboTransType.Size = new System.Drawing.Size(172, 23);
            this.cboTransType.TabIndex = 4;
            this.cboTransType.KeyDown += new System.Windows.Forms.KeyEventHandler(this.cboTransType_KeyDown);
            this.cboTransType.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.cboTransType_KeyPress);
            // 
            // ultraLabel20
            // 
            appearance44.ForeColor = System.Drawing.Color.White;
            this.ultraLabel20.Appearance = appearance44;
            this.ultraLabel20.Location = new System.Drawing.Point(10, 38);
            this.ultraLabel20.Name = "ultraLabel20";
            this.ultraLabel20.Size = new System.Drawing.Size(116, 14);
            this.ultraLabel20.TabIndex = 20;
            this.ultraLabel20.Text = "Start Date";
            // 
            // ultraLabel19
            // 
            appearance45.ForeColor = System.Drawing.Color.White;
            this.ultraLabel19.Appearance = appearance45;
            this.ultraLabel19.Location = new System.Drawing.Point(10, 67);
            this.ultraLabel19.Name = "ultraLabel19";
            this.ultraLabel19.Size = new System.Drawing.Size(106, 14);
            this.ultraLabel19.TabIndex = 22;
            this.ultraLabel19.Text = "End Date";
            // 
            // txtStationID
            // 
            this.txtStationID.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.txtStationID.Location = new System.Drawing.Point(230, 94);
            this.txtStationID.MaxLength = 10;
            this.txtStationID.Name = "txtStationID";
            this.txtStationID.Size = new System.Drawing.Size(172, 23);
            this.txtStationID.TabIndex = 3;
            this.txtStationID.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // ultraLabel21
            // 
            appearance46.ForeColor = System.Drawing.Color.White;
            this.ultraLabel21.Appearance = appearance46;
            this.ultraLabel21.AutoSize = true;
            this.ultraLabel21.Location = new System.Drawing.Point(10, 96);
            this.ultraLabel21.Name = "ultraLabel21";
            this.ultraLabel21.Size = new System.Drawing.Size(228, 18);
            this.ultraLabel21.TabIndex = 17;
            this.ultraLabel21.Text = "Station # <Blank = All Station>";
            // 
            // lblTransactionType
            // 
            appearance47.ForeColor = System.Drawing.Color.White;
            appearance47.ForeColorDisabled = System.Drawing.Color.White;
            appearance47.TextHAlignAsString = "Center";
            this.lblTransactionType.Appearance = appearance47;
            this.lblTransactionType.BackColorInternal = System.Drawing.Color.Transparent;
            this.lblTransactionType.Font = new System.Drawing.Font("Arial", 18F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTransactionType.Location = new System.Drawing.Point(11, 16);
            this.lblTransactionType.Name = "lblTransactionType";
            this.lblTransactionType.Size = new System.Drawing.Size(435, 30);
            this.lblTransactionType.TabIndex = 26;
            this.lblTransactionType.Text = "Productivity Report";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnPrint);
            this.groupBox2.Controls.Add(this.btnClose);
            this.groupBox2.Controls.Add(this.btnView);
            this.groupBox2.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.groupBox2.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.Location = new System.Drawing.Point(12, 250);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(415, 57);
            this.groupBox2.TabIndex = 31;
            this.groupBox2.TabStop = false;
            // 
            // btnPrint
            // 
            appearance48.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(62)))), ((int)(((byte)(176)))));
            appearance48.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance48.FontData.BoldAsString = "True";
            appearance48.ForeColor = System.Drawing.Color.White;
            appearance48.Image = ((object)(resources.GetObject("appearance48.Image")));
            this.btnPrint.Appearance = appearance48;
            this.btnPrint.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            this.btnPrint.Location = new System.Drawing.Point(134, 19);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(85, 26);
            this.btnPrint.TabIndex = 7;
            this.btnPrint.Text = "&Print";
            this.btnPrint.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            // 
            // btnClose
            // 
            appearance49.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(62)))), ((int)(((byte)(176)))));
            appearance49.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance49.FontData.BoldAsString = "True";
            appearance49.ForeColor = System.Drawing.Color.White;
            appearance49.Image = ((object)(resources.GetObject("appearance49.Image")));
            this.btnClose.Appearance = appearance49;
            this.btnClose.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(317, 20);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(85, 26);
            this.btnClose.TabIndex = 8;
            this.btnClose.Text = "&Close";
            this.btnClose.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnView
            // 
            appearance50.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(62)))), ((int)(((byte)(176)))));
            appearance50.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance50.FontData.BoldAsString = "True";
            appearance50.ForeColor = System.Drawing.Color.White;
            appearance50.Image = ((object)(resources.GetObject("appearance50.Image")));
            this.btnView.Appearance = appearance50;
            this.btnView.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            this.btnView.Location = new System.Drawing.Point(226, 20);
            this.btnView.Name = "btnView";
            this.btnView.Size = new System.Drawing.Size(85, 26);
            this.btnView.TabIndex = 6;
            this.btnView.Text = "&View";
            this.btnView.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnView.Click += new System.EventHandler(this.btnView_Click_1);
            // 
            // frmRptProductivityReport
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(108)))), ((int)(((byte)(172)))));
            this.ClientSize = new System.Drawing.Size(443, 326);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.lblTransactionType);
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.Name = "frmRptProductivityReport";
            this.Text = "Productivity Report";
            this.Load += new System.EventHandler(this.frmViewTransaction_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmViewTransaction_KeyDown);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cboTransType)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtStationID)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);

		}
		#endregion

		private void frmViewTransaction_Load(object sender, System.EventArgs e)
		{
			this.txtStationID.AfterEnterEditMode += new System.EventHandler(clsUIHelper.AfterEnterEditMode);			
			this.txtStationID.AfterExitEditMode += new System.EventHandler(clsUIHelper.AfterExitEditMode);			

			this.mskSaleStartDate.Enter += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
			this.mskSaleStartDate.Leave += new System.EventHandler(clsUIHelper.AfterExitEditMode);

			this.mskSaleEndDate.Enter += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
			this.mskSaleEndDate.Leave += new System.EventHandler(clsUIHelper.AfterExitEditMode);

			//this.txtUserID.AfterEnterEditMode += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
			//this.txtUserID.AfterExitEditMode += new System.EventHandler(clsUIHelper.AfterExitEditMode);

			this.Left=(frmMain.getInstance().Width-frmMain.getInstance().ultraExplorerBar1.Width-this.Width)/2;
			this.Top=(frmMain.getInstance().Height-this.Height)/2;
			populateTransType();
			clsUIHelper.setColorSchecme(this);

			this.mskSaleStartDate.Value=DateTime.Now;
			this.mskSaleEndDate.Value=DateTime.Now;
			//this.dtpSaleStartDate.Value=DateTime.Now;
			//this.dtpSaleEndDate.Value=DateTime.Now;
		}

		private void btnView_Click(object sender, System.EventArgs e)
		{
			PreviewReport(false);
		}

		private void btnClose_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}

		private void frmViewTransaction_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
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

		private void populateTransType()
		{
			try
			{
				
				this.cboTransType.Items.Add("All","All");
				this.cboTransType.Items.Add("Sales","Sales");
				this.cboTransType.Items.Add("Returns","Returns");
				this.cboTransType.SelectedIndex=0;
			}
			catch(Exception ) {}
		}

		private void PreviewReport(bool blnPrint)
		{
			try
			{
				this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
				rptHourlyTransSummary oRpt = new rptHourlyTransSummary();

				String strQuery;
				
				strQuery=" select count(TransID) as Trans, cast(DATENAME(hour, PT.transdate) as varchar) + ':00 To ' + cast(DATENAME(hour, PT.transdate)+1 as varchar) + ':00' as TransDate,PT.StationID,ps.stationname " +
						" ,sum(PT.GROSSTOTAL) as GrossTotal,cast('"+ this.mskSaleStartDate.Text +"' as datetime) as StartDate, cast('" + this.mskSaleEndDate.Text + "' as datetime) as EndDate " +
						" from postransaction PT, util_POSSet ps " +
						" where pt.stationid=ps.stationid and convert(datetime,PT.TransDate,109) between convert(datetime, cast('" + this.mskSaleStartDate.Text  + " 00:00:00 ' as datetime) ,113) and convert(datetime, cast('" + this.mskSaleEndDate.Text  + " 23:59:59 ' as datetime) ,113) ";
						
				if (this.cboTransType.SelectedIndex==1 )
					strQuery+=" and PT.TransType=1";
				else if (this.cboTransType.SelectedIndex==2 )
					strQuery+=" and PT.TransType=2"; 

				if (this.txtStationID.Text.Trim()!="")
					strQuery+=" and Ps.stationname='" + this.txtStationID.Text.Trim() + "' ";

				strQuery+= " group by cast(DATENAME(hour, PT.transdate) as varchar) + ':00 To ' + cast(DATENAME(hour, PT.transdate)+1 as varchar) + ':00' ,pt.stationid,ps.stationname " +
							" order by transdate";
				clsReports.Preview(blnPrint,strQuery,oRpt);
				this.Cursor = System.Windows.Forms.Cursors.Default;

			}
			catch(Exception exp)
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

        private void cboTransType_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Tab || e.KeyCode == Keys.Up || e.KeyCode == Keys.Down)
                e.Handled = false;
            else
                e.Handled = true;
        }

        private void cboTransType_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }
    }
}
