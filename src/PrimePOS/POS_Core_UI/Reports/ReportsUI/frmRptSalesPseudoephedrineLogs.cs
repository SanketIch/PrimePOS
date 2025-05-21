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
using Infragistics.Documents.Reports.Report.Preferences.PDF;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace POS_Core_UI.Reports.ReportsUI
{
    /// <summary>
    /// Summary description for frmRptSalesPseudoephedrineLogs.
    /// </summary>
    public class frmRptSalesPseudoephedrineLogs : System.Windows.Forms.Form
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
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
        private static ILogger logger = LogManager.GetCurrentClassLogger();

        public frmRptSalesPseudoephedrineLogs()
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
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinSchedule.CalendarCombo.DateButton dateButton1 = new Infragistics.Win.UltraWinSchedule.CalendarCombo.DateButton();
            Infragistics.Win.UltraWinSchedule.CalendarCombo.DateButton dateButton2 = new Infragistics.Win.UltraWinSchedule.CalendarCombo.DateButton();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.ultraLabel20 = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabel19 = new Infragistics.Win.Misc.UltraLabel();
            this.dtpSaleEndDate = new Infragistics.Win.UltraWinSchedule.UltraCalendarCombo();
            this.dtpSaleStartDate = new Infragistics.Win.UltraWinSchedule.UltraCalendarCombo();
            this.lblTransactionType = new Infragistics.Win.Misc.UltraLabel();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnPrint = new Infragistics.Win.Misc.UltraButton();
            this.btnClose = new Infragistics.Win.Misc.UltraButton();
            this.btnView = new Infragistics.Win.Misc.UltraButton();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dtpSaleEndDate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtpSaleStartDate)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.ultraLabel20);
            this.groupBox1.Controls.Add(this.ultraLabel19);
            this.groupBox1.Controls.Add(this.dtpSaleEndDate);
            this.groupBox1.Controls.Add(this.dtpSaleStartDate);
            this.groupBox1.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.groupBox1.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(16, 84);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(463, 111);
            this.groupBox1.TabIndex = 27;
            this.groupBox1.TabStop = false;
            // 
            // ultraLabel20
            // 
            appearance1.ForeColor = System.Drawing.Color.White;
            this.ultraLabel20.Appearance = appearance1;
            this.ultraLabel20.Location = new System.Drawing.Point(12, 38);
            this.ultraLabel20.Name = "ultraLabel20";
            this.ultraLabel20.Size = new System.Drawing.Size(116, 14);
            this.ultraLabel20.TabIndex = 20;
            this.ultraLabel20.Text = "Start Date";
            // 
            // ultraLabel19
            // 
            appearance2.ForeColor = System.Drawing.Color.White;
            this.ultraLabel19.Appearance = appearance2;
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
            // lblTransactionType
            // 
            appearance3.ForeColor = System.Drawing.Color.White;
            appearance3.ForeColorDisabled = System.Drawing.Color.White;
            appearance3.TextHAlignAsString = "Center";
            this.lblTransactionType.Appearance = appearance3;
            this.lblTransactionType.BackColorInternal = System.Drawing.Color.Transparent;
            this.lblTransactionType.Font = new System.Drawing.Font("Arial", 18F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTransactionType.Location = new System.Drawing.Point(16, 16);
            this.lblTransactionType.Name = "lblTransactionType";
            this.lblTransactionType.Size = new System.Drawing.Size(424, 58);
            this.lblTransactionType.TabIndex = 26;
            this.lblTransactionType.Text = "Pseudoephedrine Sales Logs";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnPrint);
            this.groupBox2.Controls.Add(this.btnClose);
            this.groupBox2.Controls.Add(this.btnView);
            this.groupBox2.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.groupBox2.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.Location = new System.Drawing.Point(16, 201);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(462, 57);
            this.groupBox2.TabIndex = 31;
            this.groupBox2.TabStop = false;
            // 
            // btnPrint
            // 
            appearance4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(62)))), ((int)(((byte)(176)))));
            appearance4.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance4.FontData.BoldAsString = "True";
            appearance4.ForeColor = System.Drawing.Color.White;
            this.btnPrint.Appearance = appearance4;
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
            appearance5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(62)))), ((int)(((byte)(176)))));
            appearance5.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance5.FontData.BoldAsString = "True";
            appearance5.ForeColor = System.Drawing.Color.White;
            this.btnClose.Appearance = appearance5;
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
            appearance6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(62)))), ((int)(((byte)(176)))));
            appearance6.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance6.FontData.BoldAsString = "True";
            appearance6.ForeColor = System.Drawing.Color.White;
            this.btnView.Appearance = appearance6;
            this.btnView.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            this.btnView.Location = new System.Drawing.Point(277, 20);
            this.btnView.Name = "btnView";
            this.btnView.Size = new System.Drawing.Size(85, 26);
            this.btnView.TabIndex = 5;
            this.btnView.Text = "&View";
            this.btnView.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnView.Click += new System.EventHandler(this.btnView_Click);
            // 
            // frmRptSalesPseudoephedrineLogs
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(108)))), ((int)(((byte)(172)))));
            this.ClientSize = new System.Drawing.Size(491, 283);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.lblTransactionType);
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.Name = "frmRptSalesPseudoephedrineLogs";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Pseudoephedrine Sales Logs";
            this.Load += new System.EventHandler(this.frmRptSalesPseudoephedrineLogs_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmRptSalesPseudoephedrineLogs_KeyDown);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dtpSaleEndDate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtpSaleStartDate)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);

		}
		#endregion

		private void frmRptSalesPseudoephedrineLogs_Load(object sender, System.EventArgs e)
		{
			this.dtpSaleStartDate.Enter += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
			this.dtpSaleStartDate.Leave += new System.EventHandler(clsUIHelper.AfterExitEditMode);

			this.dtpSaleEndDate.Enter += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
			this.dtpSaleEndDate.Leave += new System.EventHandler(clsUIHelper.AfterExitEditMode);

			this.Left=(frmMain.getInstance().Width-frmMain.getInstance().ultraExplorerBar1.Width-this.Width)/2;
			this.Top=(frmMain.getInstance().Height-this.Height)/2;
			
			clsUIHelper.setColorSchecme(this);
			this.dtpSaleEndDate.Value=DateTime.Now;
			this.dtpSaleStartDate.Value=DateTime.Now;

		}

		private void btnView_Click(object sender, System.EventArgs e)
		{
            PreviewReport(false);
        }

        private void btnClose_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}

		private void frmRptSalesPseudoephedrineLogs_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
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
            try
            {
                if (Convert.ToDateTime(this.dtpSaleEndDate.Value.ToString()).Date < Convert.ToDateTime(this.dtpSaleStartDate.Value.ToString()).Date)
                {
                    throw (new Exception("End date cannot be less than Start date."));
                }

                this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
                rptSalesPseudoephedrineLogs oRpt = new rptSalesPseudoephedrineLogs();

                String strQuery = string.Empty, strTotals = string.Empty, strWhereClause = string.Empty;
                DataSet oDataSet = new DataSet();
                strQuery = "Select DISTINCT IsNull(CustomerName,'') + ', '+ IsNull(FirstName,'') as [Customer], " +
                    " IsNull(Address1,'') +' '+ IsNull(Address2,'') +' '+ IsNull(City,'') +' '+ IsNull(State,'') As Address, " +
                    " PTH.TransID , TransDate as [Trans Date]  ,  " +
                    " Case PTH.TransType when 1 Then 'Sale' when 2 Then 'Return' end as TransType , " +
                    " ISNULL(PTS.POSTransId,0) as  POSTransId " +
                    ",case CustomerIDType when 'DL' then 'Driver License(US or Canada)' when 'UP' then 'US Passport' when 'RC' then 'Alien Registration or Permanent Resident Card'" +
                    " when 'FP' then 'Unexpired Foreign Passport with temporary I-551 Stamp' when 'EA' then 'Unexpired Employment Authorization Document'" +
                    " when 'SI' then 'School ID with Picture' when 'VC' then 'Voter Registration Card' when 'MC' then 'US Military Card' " +
                    " when 'TD' then 'Native American Tribal Documents' " +
                    " when 'STATE_ID' then 'Other state-issued ID' when 'ALIEN' then 'Alien Registration Card' END as CustomerIDType, " +

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
                    ", PTD.TransDetailID, PTH.CustomerID" +
                    " , PSE.ProductPillCnt, PSE.ProductGrams, PSE.ProductId " + //PRIMEPOS-3360
                    " FROM POSTransaction as PTH " +
                    " LEFT JOIN Users U ON U.UserID = PTH.UserID " +
                    " INNER JOIN Customer as Cus ON PTH.CustomerId = Cus.CustomerID " +
                    " INNER JOIN util_POSSet ps ON ps.stationid=PTH.stationid " +
                    " INNER JOIN POSTransactionDetail PTD ON PTH.TransID=PTD.TransID " +
                    " LEFT OUTER JOIN POSTransSignLog PTS ON PTS.POSTransId = PTD.TransID " +
                    " INNER JOIN Item I ON I.ItemID=PTD.ItemID " +
                    " INNER JOIN ItemMonitorTransDetail IMT ON IMT.TransDetailID = PTD.TransDetailID " +
                    " LEFT JOIN PSE_Items PSE ON PSE.ProductId = PTD.ItemID " + //PRIMEPOS-3360 //PRIMEPOS-3472
                    " Where convert(datetime,TransDate,109) between convert(datetime, cast('" + this.dtpSaleStartDate.Text + " 00:00:00 ' as datetime) ,113) and convert(datetime, cast('" + this.dtpSaleEndDate.Text + " 23:59:59' as datetime) ,113) ";

                clsReports.setCRTextObjectText("txtFromDate", "From :" + this.dtpSaleStartDate.Text, oRpt);
                clsReports.setCRTextObjectText("txtToDate", "To :" + dtpSaleEndDate.Text, oRpt);

                DataSet ds = clsReports.GetReportSource(strQuery);

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

                clsReports.DStoExport = ds; 
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

    }
}
