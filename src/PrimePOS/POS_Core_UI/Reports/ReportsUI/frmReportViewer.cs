using System;
using System.Data;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Forms;
//using POS_Core.DataAccess;
//using POS.UI;
using Microsoft.Office.Interop.Excel;
using System.IO;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System.Text;
using POS_Core.Resources;

namespace POS_Core_UI.Reports.ReportsUI
{
	/// <summary>
	/// Summary description for frmReportViewer.
	/// </summary>
	public class frmReportViewer : System.Windows.Forms.Form
	{
		public CrystalDecisions.Windows.Forms.CrystalReportViewer rvReportViewer;
		public System.Windows.Forms.PrintDialog printDialog1;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnExportToExcel;
        private System.Windows.Forms.SaveFileDialog SaveExportFile;
        private SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.Button btnExportToPDF;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

        public frmReportViewer()
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
            this.rvReportViewer = new CrystalDecisions.Windows.Forms.CrystalReportViewer();
            this.printDialog1 = new System.Windows.Forms.PrintDialog();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnExportToExcel = new System.Windows.Forms.Button();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.btnExportToPDF = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // rvReportViewer
            // 
            this.rvReportViewer.ActiveViewIndex = -1;
            this.rvReportViewer.AutoScroll = true;
            this.rvReportViewer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.rvReportViewer.Cursor = System.Windows.Forms.Cursors.Default;
            this.rvReportViewer.DisplayBackgroundEdge = false;
            this.rvReportViewer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rvReportViewer.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rvReportViewer.Location = new System.Drawing.Point(0, 0);
            this.rvReportViewer.Name = "rvReportViewer";
            this.rvReportViewer.SelectionFormula = "";
            this.rvReportViewer.ShowCloseButton = false;
            this.rvReportViewer.ShowGroupTreeButton = false;
            this.rvReportViewer.ShowTextSearchButton = false;
            this.rvReportViewer.Size = new System.Drawing.Size(743, 273);
            this.rvReportViewer.TabIndex = 0;
            this.rvReportViewer.ToolPanelView = CrystalDecisions.Windows.Forms.ToolPanelViewType.None;
            this.rvReportViewer.ToolPanelWidth = 0;
            this.rvReportViewer.ViewTimeSelectionFormula = "";
            // 
            // btnClose
            // 
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(422, 4);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(91, 21);
            this.btnClose.TabIndex = 1;
            this.btnClose.Text = "Close(Esc)";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            this.btnClose.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmReportViewer_KeyDown);
            // 
            // btnExportToExcel
            // 
            this.btnExportToExcel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnExportToExcel.Location = new System.Drawing.Point(520, 4);
            this.btnExportToExcel.Name = "btnExportToExcel";
            this.btnExportToExcel.Size = new System.Drawing.Size(96, 21);
            this.btnExportToExcel.TabIndex = 2;
            this.btnExportToExcel.Text = "Export To Excel";
            this.btnExportToExcel.Click += new System.EventHandler(this.btnExportToExcel_Click);
            // 
            // btnExportToPDF
            // 
            this.btnExportToPDF.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnExportToPDF.Location = new System.Drawing.Point(622, 4);
            this.btnExportToPDF.Name = "btnExportToPDF";
            this.btnExportToPDF.Size = new System.Drawing.Size(96, 21);
            this.btnExportToPDF.TabIndex = 3;
            this.btnExportToPDF.Text = "Export To PDF";
            this.btnExportToPDF.Click += new System.EventHandler(this.btnExportToPDF_Click);
            // 
            // frmReportViewer
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.AutoScroll = true;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(108)))), ((int)(((byte)(172)))));
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(743, 273);
            this.Controls.Add(this.btnExportToPDF);
            this.Controls.Add(this.btnExportToExcel);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.rvReportViewer);
            this.KeyPreview = true;
            this.Name = "frmReportViewer";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmReportViewer_FormClosed);
            this.Load += new System.EventHandler(this.frmReportViewer_Load);
            this.Shown += new System.EventHandler(this.frmReportViewer_Shown);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmReportViewer_KeyDown);
            this.ResumeLayout(false);

		}
		#endregion

		private void frmReportViewer_Load(object sender, System.EventArgs e)
		{
            SetCurrencyParameterValueOnReport();

		    rvReportViewer.AutoScroll = true;
            

			clsUIHelper.setColorSchecme(this);
		}

        /// <summary>
        /// Added by Shrikant Mali on 1-24-2014
        /// Following code sets the Currency Parameter Value.
        /// </summary>
	    private void SetCurrencyParameterValueOnReport()
	    {
            try
            {
                var curSymbol = Configuration.CInfo.CurrencySymbol.ToString(CultureInfo.InvariantCulture);
                var reportSource = (ReportClass)rvReportViewer.ReportSource;
                reportSource.SetParameterValue("Currency", curSymbol);
            }
            catch (Exception Ex)
            {
                POS_Core.ErrorLogging.Logs.Logger(Ex.StackTrace);
            }
	    }

	    private void frmReportViewer_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			if (e.KeyData==Keys.Escape)
					this.Close();
		}

		private void btnClose_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}

        private void frmReportViewer_Shown(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
            btnClose.Focus();
        }

        private void frmReportViewer_FormClosed(object sender, FormClosedEventArgs e)
        {
            //Added By SRT(Ritesh Parekh) Date : 29-08-2009
            frmMain.getInstance().ShowInTaskbar = true;
            frmMain.getInstance().BringToFront();
            //End Of Added By SRT(Ritesh Parekh)

            //Following func added by Krishna on 9 June 2011
            clsReports.dsToExportList.Clear();
            frmCreateNewPurchaseOrder ofrmPOOrder = new frmCreateNewPurchaseOrder();
            ofrmPOOrder = frmCreateNewPurchaseOrder.getInstance();
            if (ofrmPOOrder != null)
            {
                ofrmPOOrder.BringToFront();
            }
        }

        //Following func added by Krishna on 9 June 2011
        ArrayList TempList = new ArrayList();
        private void btnExportToExcel_Click(object sender, EventArgs e)
        {
            try
            {
                if (clsReports.DStoExport == null)
                    return;
                System.Data.DataSet DStoExport = clsReports.DStoExport;

                ReportDocument cryRpt = new ReportDocument();
                cryRpt = (ReportDocument)rvReportViewer.ReportSource;
                Stream strm= cryRpt.ExportToStream(ExportFormatType.Excel);
                byte[] Arr = new byte[strm.Length];
                strm.Read(Arr,0,(int)strm.Length-1);
                Microsoft.Office.Interop.Excel.Application excel = new Microsoft.Office.Interop.Excel.Application();
                excel.Application.Workbooks.Add(true);
                int ColumnIndex = 0;
                foreach (System.Data.DataColumn col in DStoExport.Tables[0].Columns)
                {
                    ColumnIndex++;
                    excel.Cells[1, ColumnIndex] = col.ColumnName;
                    excel.Application.Cells.HorizontalAlignment = HorizontalAlignment.Center;
                    excel.Application.Cells.ColumnWidth = 17;
                }

                int rowIndex = 0;
                excel.Application.Cells.Font.Bold = false;
                foreach (System.Data.DataRow row in DStoExport.Tables[0].Rows)
                {
                    rowIndex++;
                    ColumnIndex = 0;

                    foreach (System.Data.DataColumn col in DStoExport.Tables[0].Columns)
                    {
                        ColumnIndex++;
                        excel.Cells[rowIndex + 1, ColumnIndex] = row[col.ColumnName].ToString();
                    }
                }
                excel.Visible = true;
                Worksheet worksheet = (Worksheet)excel.ActiveSheet;
                worksheet.Activate();
            }
            catch (Exception ex)
            {
                clsUIHelper.ShowErrorMsg(ex.Message);
            }
        }

        //Added By Shitaljit to export reports to PDF
        private void btnExportToPDF_Click(object sender, EventArgs e)
        {
            try
            {
                if (clsReports.DStoExport == null)
                    return;
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "pdf files (*.pdf)|*.pdf";
                saveFileDialog.RestoreDirectory = true;
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    System.Data.DataSet DStoExport = clsReports.DStoExport;
                  
                    ReportDocument cryRpt = new ReportDocument();
                    cryRpt = (ReportDocument)rvReportViewer.ReportSource;
                    string sFileName = string.Empty;
                    string Path = System.Windows.Forms.Application.ExecutablePath;
                    System.IO.FileInfo FI = new System.IO.FileInfo(Path);
                    sFileName = saveFileDialog.FileName+ ".pdf";
                    cryRpt.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, sFileName);
                }
            }
            catch (Exception ex)
            {
                clsUIHelper.ShowErrorMsg(ex.Message);
            }
        }
        //Till here added by Krishna on 9 June 2011
	}
}
