using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
//using POS_Core.DataAccess;
using POS_Core_UI.Reports.Reports;
using POS_Core.CommonData;
using POS_Core.DataAccess;
//using POS_Core_UI.Reports.ReportsUI;
using POS_Core.BusinessRules;
using System.Data;
using POS_Core.CommonData.Rows;
using System.IO;
using NLog; //PRIMEPOS-2384 29-Oct-2018 JY Added 
using POS_Core.Resources;
using POS_Core.LabelHandler;
using POS_Core_UI.Reports.ReportsUI;
using POS_Core.LabelHandler.RxLabel;
using System.Linq;

namespace POS_Core_UI
{
	/// <summary>
	/// Summary description for frmPOSChangeDue.
	/// </summary>
	public class frmPOSChangeDue : System.Windows.Forms.Form
	{
		private System.Windows.Forms.GroupBox groupBox1;
		private Infragistics.Win.Misc.UltraButton btnClose;
		private Infragistics.Win.Misc.UltraLabel ultraLabel3;
		private Infragistics.Win.Misc.UltraLabel ultraLabel1;
        private Infragistics.Win.Misc.UltraLabel lblChangeDue;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label lblTransactionNo;
        private Label lblTotTransAmt;
        private Label lblAmtTendered;
        private Label lblChangeD;
        private string tranSno = "";
        private string CustID = "";
        private string CustName = "";
        private string custEmail = "";
        DateTime TransDate = DateTime.Now;
        private Infragistics.Win.Misc.UltraButton btnEmail;
        private Infragistics.Win.UltraWinGrid.UltraGrid grdPaymentComplete;
        private Infragistics.Win.UltraWinDataSource.UltraDataSource ultraDataSource2;
        private Infragistics.Win.UltraWinDataSource.UltraDataSource ultraDataSource1;
        private IContainer components;
        private Infragistics.Win.Misc.UltraButton btnAddToStoreCredit;
        private Label lblStoreCreditAmount;
        private Label lblStoreCreditAmtTitle;
        private static ILogger logger = LogManager.GetCurrentClassLogger(); //PRIMEPOS-2384 29-Oct-2018 JY Added 

        #region StoreCredit PRIMEPOS-2747 - NileshJ - 20-Nov-2019 - NileshJ
        POS_Core.CommonData.Rows.TransHeaderRow oTransheaderRow = null;
        POS_Core.CommonData.Tables.POSTransPaymentTable oPOSTransPayment = null;
        POS_Core.CommonData.StoreCreditData oStoreCreditData = new StoreCreditData();
        StoreCredit oStoreCredit = new StoreCredit();
        StoreCreditRow oStoreCreditRow;
        POS_Core.CommonData.StoreCreditDetailsData oStoreCreditDetailsData = new StoreCreditDetailsData();
        StoreCreditDetailsRow oStoreCreditDetailsRow;
        StoreCreditDetails oStoreCreditDetails = new StoreCreditDetails();
        public decimal storeCreditAmount = 0;
        public bool IsStoreCredit = false;
        public string CustomerCode = string.Empty;
        #endregion
        //,Transid, decimal TotTransAmt, decimal AmtTendered
        public frmPOSChangeDue(POS_Core.CommonData.Rows.TransHeaderRow TransheaderRow, System.Decimal changeDue, POS_Core.CommonData.Tables.POSTransPaymentTable POSTransPayment, POS_Core.CommonData.StoreCreditData StoreCreditD, bool IsStoreCred = false, string CustCode = "") //PRIMEPOS-2384 29-Oct-2018 JY Added POSTransPayment // PRIMEPOS-2747 - NileshJ - 20-Nov-2019 - StoreCredit - NileshJ - added POS_Core.CommonData.StoreCreditData StoreCreditD, bool IsStoreCred = false , string CustCode = ""
        {
			InitializeComponent();
            this.lblChangeDue.Text=changeDue.ToString(Configuration.CInfo.CurrencySymbol+"######0.00");
			String strText="Change Due:";
			frmMain.PoleDisplay.ClearPoleDisplay();
			frmMain.PoleDisplay.WriteToPoleDisplay( strText+clsUIHelper.Spaces(Configuration.CPOSSet.PD_LINELEN-strText.Length-9)+" "+ changeDue.ToString(Configuration.CInfo.CurrencySymbol+"####0.00"));
            tranSno = TransheaderRow.TransID.ToString();
            TransDate = TransheaderRow.TransDate;
            CustID = TransheaderRow.CustomerID.ToString();
            this.lblTransactionNo.Text = "Trans#: " + TransheaderRow.TransID.ToString() + "    " + TransheaderRow.TransDate.ToString() + "     User Id:" + Configuration.UserName.Trim();
            this.lblTotTransAmt.Text = TransheaderRow.TotalPaid.ToString(Configuration.CInfo.CurrencySymbol+"######0.00");
            this.lblAmtTendered.Text = TransheaderRow.TenderedAmount.ToString(Configuration.CInfo.CurrencySymbol+"######0.00");
            this.lblChangeD.Text = changeDue.ToString(Configuration.CInfo.CurrencySymbol+"######0.00");

            #region PRIMEPOS-2384 29-Oct-2018 JY Added 
            try
            {
                if (Configuration.CInfo.ShowPaytypeDetails == true)
                {
                    this.Height = 416;
                    DataSet ds = new DataSet();
                    ds.Tables.Add("PaymentDetails");
                    ds.Tables[0].Columns.Add("Payment Type", Type.GetType("System.String"));
                    ds.Tables[0].Columns.Add("Amount", Type.GetType("System.String"));
                    ds.Tables[0].Columns.Add("Ref No / CC No", Type.GetType("System.String"));
                    ds.Tables[0].Columns.Add("Auth No", Type.GetType("System.String"));
                    if (POSTransPayment != null)
                    {
                        foreach (POSTransPaymentRow oRow in POSTransPayment)
                        {
                            DataRow drTemp = ds.Tables[0].NewRow();
                            drTemp[0] = (string.IsNullOrEmpty(oRow.TransTypeDesc) || Configuration.convertNullToString(oRow.TransTypeDesc).Trim() == "0") ? GetPayTypeDesc(Configuration.convertNullToString(oRow.TransTypeCode)) : Configuration.convertNullToString(oRow.TransTypeDesc);//PRIMEPOS-3081 //PRIMEPOS-3087
                            drTemp[1] = Configuration.convertNullToDecimal(oRow.Amount).ToString("########0.00");
                            drTemp[2] = Configuration.convertNullToString(oRow.RefNo);
                            drTemp[3] = Configuration.convertNullToString(oRow.AuthNo);
                            ds.Tables[0].Rows.Add(drTemp);
                        }
                    }
                    grdPaymentComplete.DataSource = ds;
                    grdPaymentComplete.DisplayLayout.Bands[0].Columns[0].Width = 100;
                    grdPaymentComplete.DisplayLayout.Bands[0].Columns[1].Width = 80;
                    grdPaymentComplete.DisplayLayout.Bands[0].Columns[2].Width = 170;
                    grdPaymentComplete.DisplayLayout.Bands[0].Columns[3].Width = 80;

                    grdPaymentComplete.DisplayLayout.Bands[0].Columns[1].CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right;
                    grdPaymentComplete.DisplayLayout.Bands[0].Columns[1].MinValue = -99999999999.99;
                    grdPaymentComplete.DisplayLayout.Bands[0].Columns[1].MaxValue = 99999999999.99;
                    grdPaymentComplete.DisplayLayout.Bands[0].Columns[1].Format = "##########0.00";
                    clsUIHelper.SetReadonlyRow(this.grdPaymentComplete);
                    this.grdPaymentComplete.DisplayLayout.TabNavigation = Infragistics.Win.UltraWinGrid.TabNavigation.NextControl;
                }
                else
                {
                    this.Height = 300;
                }
            }
            catch(Exception Ex)
            {
                logger.Fatal(Ex, clsPOSDBConstants.Log_Module_Transaction + " frmPOSChangeDue(...)");
            }
            #endregion

            #region StoreCredit PRIMEPOS-2747 - NileshJ - 20-Nov-2019 NileshJ
            if (Configuration.CPOSSet.EnableStoreCredit)
            {
                oTransheaderRow = TransheaderRow;
                oPOSTransPayment = POSTransPayment;
                storeCreditAmount = changeDue;
                oStoreCreditData = StoreCreditD;
                IsStoreCredit = IsStoreCred;
                CustomerCode = CustCode;
            }
            #endregion
        }

        #region PRIMEPOS-3081
        private string GetPayTypeDesc(string payTypeCode)
        {
            switch (payTypeCode.Trim().ToUpper())
            {
                case "1":
                    return "Cash";
                case "2":
                    return "Check";
                case "3":
                    return "American Express";
                case "4":
                case "0"://In Case of 0 then default //PRIMEPOS-3087
                    return "Visa";
                case "5":
                    return "Master Card";
                case "6":
                    return "Discover";
                case "7":
                    return "Debit Card";
                case "8":
                    return "ATH MOVIL";
                case "B":
                    return "Cash Back";
                case "C":
                    return "Coupon";
                case "E":
                    return "Elect. Benefit Transfer";
                case "F":
                    return "FONDO UNICA";
                case "G":
                    return "OTC";
                case "H":
                    return "House Charge";
                case "O":
                    return "PrimeRxPay";
                case "Q":
                    return "Monopoly Money";
                case "S":
                    return "S3 Card";
                case "X":
                    return "Store Credit";
                default:
                    return "";

            }
        }
        #endregion

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
            this.components = new System.ComponentModel.Container();
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance7 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand1 = new Infragistics.Win.UltraWinGrid.UltraGridBand("Band 0", -1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn52 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("PayTypeDesc");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn53 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Amount");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn54 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("RefNo");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn55 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("AuthNo");
            Infragistics.Win.Appearance appearance8 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance9 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance10 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance11 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance12 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance13 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance14 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance15 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance16 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance17 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance18 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance19 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance20 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance21 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance22 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance23 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance24 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance25 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance26 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinScrollBar.ScrollBarLook scrollBarLook1 = new Infragistics.Win.UltraWinScrollBar.ScrollBarLook();
            Infragistics.Win.Appearance appearance27 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinDataSource.UltraDataColumn ultraDataColumn1 = new Infragistics.Win.UltraWinDataSource.UltraDataColumn("PayTypeDesc");
            Infragistics.Win.UltraWinDataSource.UltraDataColumn ultraDataColumn2 = new Infragistics.Win.UltraWinDataSource.UltraDataColumn("Amount");
            Infragistics.Win.UltraWinDataSource.UltraDataColumn ultraDataColumn3 = new Infragistics.Win.UltraWinDataSource.UltraDataColumn("RefNo");
            Infragistics.Win.UltraWinDataSource.UltraDataColumn ultraDataColumn4 = new Infragistics.Win.UltraWinDataSource.UltraDataColumn("AuthNo");
            Infragistics.Win.UltraWinDataSource.UltraDataColumn ultraDataColumn5 = new Infragistics.Win.UltraWinDataSource.UltraDataColumn("TransPayID");
            Infragistics.Win.UltraWinDataSource.UltraDataColumn ultraDataColumn6 = new Infragistics.Win.UltraWinDataSource.UltraDataColumn("TransID");
            Infragistics.Win.UltraWinDataSource.UltraDataColumn ultraDataColumn7 = new Infragistics.Win.UltraWinDataSource.UltraDataColumn("TransTypeCode");
            Infragistics.Win.UltraWinDataSource.UltraDataColumn ultraDataColumn8 = new Infragistics.Win.UltraWinDataSource.UltraDataColumn("PayTypeDesc");
            Infragistics.Win.UltraWinDataSource.UltraDataColumn ultraDataColumn9 = new Infragistics.Win.UltraWinDataSource.UltraDataColumn("Amount");
            Infragistics.Win.UltraWinDataSource.UltraDataColumn ultraDataColumn10 = new Infragistics.Win.UltraWinDataSource.UltraDataColumn("RefNo");
            Infragistics.Win.UltraWinDataSource.UltraDataColumn ultraDataColumn11 = new Infragistics.Win.UltraWinDataSource.UltraDataColumn("AuthNo");
            Infragistics.Win.UltraWinDataSource.UltraDataColumn ultraDataColumn12 = new Infragistics.Win.UltraWinDataSource.UltraDataColumn("HC_Posted");
            Infragistics.Win.UltraWinDataSource.UltraDataColumn ultraDataColumn13 = new Infragistics.Win.UltraWinDataSource.UltraDataColumn("CCName");
            Infragistics.Win.UltraWinDataSource.UltraDataColumn ultraDataColumn14 = new Infragistics.Win.UltraWinDataSource.UltraDataColumn("CCTransNo");
            Infragistics.Win.UltraWinDataSource.UltraDataColumn ultraDataColumn15 = new Infragistics.Win.UltraWinDataSource.UltraDataColumn("TransDate");
            Infragistics.Win.UltraWinDataSource.UltraDataColumn ultraDataColumn16 = new Infragistics.Win.UltraWinDataSource.UltraDataColumn("CustomerSign");
            Infragistics.Win.UltraWinDataSource.UltraDataColumn ultraDataColumn17 = new Infragistics.Win.UltraWinDataSource.UltraDataColumn("IsIIASPayment");
            Infragistics.Win.UltraWinDataSource.UltraDataColumn ultraDataColumn18 = new Infragistics.Win.UltraWinDataSource.UltraDataColumn("ProcessorTransID");
            Infragistics.Win.UltraWinDataSource.UltraDataColumn ultraDataColumn19 = new Infragistics.Win.UltraWinDataSource.UltraDataColumn("PaymentProcessor");
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnEmail = new Infragistics.Win.Misc.UltraButton();
            this.lblChangeDue = new Infragistics.Win.Misc.UltraLabel();
            this.btnClose = new Infragistics.Win.Misc.UltraButton();
            this.ultraLabel3 = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabel1 = new Infragistics.Win.Misc.UltraLabel();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.lblTransactionNo = new System.Windows.Forms.Label();
            this.lblTotTransAmt = new System.Windows.Forms.Label();
            this.lblAmtTendered = new System.Windows.Forms.Label();
            this.lblChangeD = new System.Windows.Forms.Label();
            this.grdPaymentComplete = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.ultraDataSource2 = new Infragistics.Win.UltraWinDataSource.UltraDataSource(this.components);
            this.ultraDataSource1 = new Infragistics.Win.UltraWinDataSource.UltraDataSource(this.components);
            this.btnAddToStoreCredit = new Infragistics.Win.Misc.UltraButton();
            this.lblStoreCreditAmount = new System.Windows.Forms.Label();
            this.lblStoreCreditAmtTitle = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdPaymentComplete)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraDataSource2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraDataSource1)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnAddToStoreCredit);
            this.groupBox1.Controls.Add(this.btnEmail);
            this.groupBox1.Controls.Add(this.lblChangeDue);
            this.groupBox1.Controls.Add(this.btnClose);
            this.groupBox1.Controls.Add(this.ultraLabel3);
            this.groupBox1.Controls.Add(this.ultraLabel1);
            this.groupBox1.ForeColor = System.Drawing.Color.Blue;
            this.groupBox1.Location = new System.Drawing.Point(10, 4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(482, 114);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            // 
            // btnEmail
            // 
            appearance2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(62)))), ((int)(((byte)(176)))));
            appearance2.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance2.FontData.BoldAsString = "True";
            appearance2.ForeColor = System.Drawing.Color.White;
            this.btnEmail.Appearance = appearance2;
            this.btnEmail.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            this.btnEmail.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnEmail.Location = new System.Drawing.Point(370, 74);
            this.btnEmail.Name = "btnEmail";
            this.btnEmail.Size = new System.Drawing.Size(90, 32);
            this.btnEmail.TabIndex = 17;
            this.btnEmail.Text = "&Email Inv.";
            this.btnEmail.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnEmail.Visible = false;
            this.btnEmail.Click += new System.EventHandler(this.btnEmail_Click);
            // 
            // lblChangeDue
            // 
            appearance3.FontData.Name = "Arial";
            appearance3.FontData.SizeInPoints = 20F;
            appearance3.ForeColor = System.Drawing.Color.Maroon;
            appearance3.TextHAlignAsString = "Right";
            appearance3.TextVAlignAsString = "Middle";
            this.lblChangeDue.Appearance = appearance3;
            this.lblChangeDue.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblChangeDue.Location = new System.Drawing.Point(216, 30);
            this.lblChangeDue.Name = "lblChangeDue";
            this.lblChangeDue.Size = new System.Drawing.Size(148, 28);
            this.lblChangeDue.TabIndex = 6;
            this.lblChangeDue.Text = "0.00";
            // 
            // btnClose
            // 
            appearance4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(62)))), ((int)(((byte)(176)))));
            appearance4.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            this.btnClose.Appearance = appearance4;
            this.btnClose.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(244, 74);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(120, 32);
            this.btnClose.TabIndex = 1;
            this.btnClose.Text = "&Continue";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // ultraLabel3
            // 
            appearance5.ForeColor = System.Drawing.Color.Blue;
            appearance5.TextVAlignAsString = "Middle";
            this.ultraLabel3.Appearance = appearance5;
            this.ultraLabel3.Location = new System.Drawing.Point(62, 77);
            this.ultraLabel3.Name = "ultraLabel3";
            this.ultraLabel3.Size = new System.Drawing.Size(193, 26);
            this.ultraLabel3.TabIndex = 7;
            this.ultraLabel3.Text = "Press Enter to Contnue";
            // 
            // ultraLabel1
            // 
            appearance6.FontData.Name = "Arial";
            appearance6.ForeColor = System.Drawing.Color.Blue;
            appearance6.TextVAlignAsString = "Middle";
            this.ultraLabel1.Appearance = appearance6;
            this.ultraLabel1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultraLabel1.Location = new System.Drawing.Point(62, 30);
            this.ultraLabel1.Name = "ultraLabel1";
            this.ultraLabel1.Size = new System.Drawing.Size(143, 26);
            this.ultraLabel1.TabIndex = 5;
            this.ultraLabel1.Text = "Change Due :";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(12, 161);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(174, 15);
            this.label2.TabIndex = 2;
            this.label2.Tag = "NOCOLOR";
            this.label2.Text = "Total Transaction Amount:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(12, 185);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(124, 15);
            this.label3.TabIndex = 3;
            this.label3.Tag = "NOCOLOR";
            this.label3.Text = "Amount Tendered:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(12, 209);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(90, 15);
            this.label4.TabIndex = 4;
            this.label4.Tag = "NOCOLOR";
            this.label4.Text = "Change Due:";
            // 
            // lblTransactionNo
            // 
            this.lblTransactionNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblTransactionNo.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTransactionNo.ForeColor = System.Drawing.Color.Maroon;
            this.lblTransactionNo.Location = new System.Drawing.Point(10, 130);
            this.lblTransactionNo.Name = "lblTransactionNo";
            this.lblTransactionNo.Size = new System.Drawing.Size(473, 20);
            this.lblTransactionNo.TabIndex = 5;
            this.lblTransactionNo.Tag = "NOCOLOR";
            this.lblTransactionNo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblTotTransAmt
            // 
            this.lblTotTransAmt.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblTotTransAmt.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTotTransAmt.ForeColor = System.Drawing.Color.Black;
            this.lblTotTransAmt.Location = new System.Drawing.Point(262, 161);
            this.lblTotTransAmt.Name = "lblTotTransAmt";
            this.lblTotTransAmt.Size = new System.Drawing.Size(114, 20);
            this.lblTotTransAmt.TabIndex = 6;
            this.lblTotTransAmt.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblAmtTendered
            // 
            this.lblAmtTendered.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblAmtTendered.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAmtTendered.ForeColor = System.Drawing.Color.Black;
            this.lblAmtTendered.Location = new System.Drawing.Point(262, 185);
            this.lblAmtTendered.Name = "lblAmtTendered";
            this.lblAmtTendered.Size = new System.Drawing.Size(114, 20);
            this.lblAmtTendered.TabIndex = 7;
            this.lblAmtTendered.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblChangeD
            // 
            this.lblChangeD.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblChangeD.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblChangeD.ForeColor = System.Drawing.Color.Black;
            this.lblChangeD.Location = new System.Drawing.Point(262, 209);
            this.lblChangeD.Name = "lblChangeD";
            this.lblChangeD.Size = new System.Drawing.Size(114, 20);
            this.lblChangeD.TabIndex = 8;
            this.lblChangeD.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // grdPaymentComplete
            // 
            this.grdPaymentComplete.DataSource = this.ultraDataSource2;
            appearance7.BackColor = System.Drawing.Color.White;
            appearance7.BackColor2 = System.Drawing.Color.White;
            appearance7.BackColorDisabled = System.Drawing.Color.White;
            appearance7.BackColorDisabled2 = System.Drawing.Color.White;
            appearance7.BackGradientStyle = Infragistics.Win.GradientStyle.None;
            this.grdPaymentComplete.DisplayLayout.Appearance = appearance7;
            this.grdPaymentComplete.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ResizeAllColumns;
            ultraGridBand1.CardSettings.AutoFit = true;
            ultraGridColumn52.Header.Caption = "Payment Type";
            ultraGridColumn52.Header.VisiblePosition = 0;
            ultraGridColumn52.RowLayoutColumnInfo.OriginX = 4;
            ultraGridColumn52.RowLayoutColumnInfo.OriginY = 0;
            ultraGridColumn52.RowLayoutColumnInfo.PreferredCellSize = new System.Drawing.Size(135, 0);
            ultraGridColumn52.RowLayoutColumnInfo.SpanX = 2;
            ultraGridColumn52.RowLayoutColumnInfo.SpanY = 2;
            ultraGridColumn53.CellMultiLine = Infragistics.Win.DefaultableBoolean.False;
            ultraGridColumn53.DefaultCellValue = "";
            ultraGridColumn53.Header.VisiblePosition = 1;
            ultraGridColumn53.MaxLength = 10;
            ultraGridColumn53.RowLayoutColumnInfo.OriginX = 6;
            ultraGridColumn53.RowLayoutColumnInfo.OriginY = 0;
            ultraGridColumn53.RowLayoutColumnInfo.PreferredCellSize = new System.Drawing.Size(114, 0);
            ultraGridColumn53.RowLayoutColumnInfo.SpanX = 2;
            ultraGridColumn53.RowLayoutColumnInfo.SpanY = 2;
            ultraGridColumn54.ButtonDisplayStyle = Infragistics.Win.UltraWinGrid.ButtonDisplayStyle.OnRowActivate;
            ultraGridColumn54.Header.Caption = "Ref No / CC No";
            ultraGridColumn54.Header.VisiblePosition = 3;
            ultraGridColumn54.RowLayoutColumnInfo.OriginX = 8;
            ultraGridColumn54.RowLayoutColumnInfo.OriginY = 0;
            ultraGridColumn54.RowLayoutColumnInfo.PreferredCellSize = new System.Drawing.Size(205, 0);
            ultraGridColumn54.RowLayoutColumnInfo.SpanX = 2;
            ultraGridColumn54.RowLayoutColumnInfo.SpanY = 2;
            ultraGridColumn54.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.EditButton;
            ultraGridColumn55.Header.Caption = "Authorization No";
            ultraGridColumn55.Header.VisiblePosition = 2;
            ultraGridColumn55.RowLayoutColumnInfo.OriginX = 14;
            ultraGridColumn55.RowLayoutColumnInfo.OriginY = 0;
            ultraGridColumn55.RowLayoutColumnInfo.SpanX = 2;
            ultraGridColumn55.RowLayoutColumnInfo.SpanY = 2;
            ultraGridBand1.Columns.AddRange(new object[] {
            ultraGridColumn52,
            ultraGridColumn53,
            ultraGridColumn54,
            ultraGridColumn55});
            ultraGridBand1.Override.CellDisplayStyle = Infragistics.Win.UltraWinGrid.CellDisplayStyle.FormattedText;
            ultraGridBand1.RowLayoutStyle = Infragistics.Win.UltraWinGrid.RowLayoutStyle.ColumnLayout;
            this.grdPaymentComplete.DisplayLayout.BandsSerializer.Add(ultraGridBand1);
            this.grdPaymentComplete.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.grdPaymentComplete.DisplayLayout.InterBandSpacing = 10;
            this.grdPaymentComplete.DisplayLayout.MaxColScrollRegions = 1;
            this.grdPaymentComplete.DisplayLayout.MaxRowScrollRegions = 1;
            appearance8.BackColor = System.Drawing.Color.White;
            appearance8.BackColor2 = System.Drawing.Color.White;
            this.grdPaymentComplete.DisplayLayout.Override.ActiveCardCaptionAppearance = appearance8;
            appearance9.BackColor = System.Drawing.Color.White;
            appearance9.BackColor2 = System.Drawing.Color.White;
            appearance9.BorderColor = System.Drawing.Color.Gray;
            this.grdPaymentComplete.DisplayLayout.Override.ActiveRowAppearance = appearance9;
            appearance10.BackColor = System.Drawing.Color.White;
            appearance10.BackColor2 = System.Drawing.Color.White;
            appearance10.BorderColor = System.Drawing.Color.Gray;
            this.grdPaymentComplete.DisplayLayout.Override.AddRowAppearance = appearance10;
            this.grdPaymentComplete.DisplayLayout.Override.AllowAddNew = Infragistics.Win.UltraWinGrid.AllowAddNew.No;
            this.grdPaymentComplete.DisplayLayout.Override.AllowColMoving = Infragistics.Win.UltraWinGrid.AllowColMoving.NotAllowed;
            this.grdPaymentComplete.DisplayLayout.Override.AllowColSizing = Infragistics.Win.UltraWinGrid.AllowColSizing.None;
            this.grdPaymentComplete.DisplayLayout.Override.AllowColSwapping = Infragistics.Win.UltraWinGrid.AllowColSwapping.NotAllowed;
            this.grdPaymentComplete.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.False;
            this.grdPaymentComplete.DisplayLayout.Override.AllowGroupMoving = Infragistics.Win.UltraWinGrid.AllowGroupMoving.NotAllowed;
            this.grdPaymentComplete.DisplayLayout.Override.AllowGroupSwapping = Infragistics.Win.UltraWinGrid.AllowGroupSwapping.NotAllowed;
            this.grdPaymentComplete.DisplayLayout.Override.AllowRowFiltering = Infragistics.Win.DefaultableBoolean.False;
            this.grdPaymentComplete.DisplayLayout.Override.AllowRowLayoutCellSizing = Infragistics.Win.UltraWinGrid.RowLayoutSizing.None;
            this.grdPaymentComplete.DisplayLayout.Override.AllowRowLayoutLabelSizing = Infragistics.Win.UltraWinGrid.RowLayoutSizing.None;
            this.grdPaymentComplete.DisplayLayout.Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.True;
            appearance11.BackColor = System.Drawing.Color.Transparent;
            this.grdPaymentComplete.DisplayLayout.Override.CardAreaAppearance = appearance11;
            appearance12.BackColor = System.Drawing.Color.White;
            appearance12.BackColor2 = System.Drawing.Color.White;
            appearance12.BackColorDisabled = System.Drawing.Color.White;
            appearance12.BackColorDisabled2 = System.Drawing.Color.White;
            appearance12.BorderColor = System.Drawing.Color.Black;
            appearance12.BorderColor3DBase = System.Drawing.Color.Black;
            this.grdPaymentComplete.DisplayLayout.Override.CellAppearance = appearance12;
            appearance13.BackColor = System.Drawing.Color.White;
            appearance13.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(231)))), ((int)(((byte)(230)))));
            appearance13.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance13.BorderAlpha = Infragistics.Win.Alpha.Transparent;
            appearance13.BorderColor = System.Drawing.Color.Gray;
            appearance13.BorderColor3DBase = System.Drawing.Color.Gray;
            appearance13.ImageBackgroundAlpha = Infragistics.Win.Alpha.Transparent;
            appearance13.ImageBackgroundStyle = Infragistics.Win.ImageBackgroundStyle.Centered;
            this.grdPaymentComplete.DisplayLayout.Override.CellButtonAppearance = appearance13;
            appearance14.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            appearance14.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.grdPaymentComplete.DisplayLayout.Override.EditCellAppearance = appearance14;
            appearance15.BorderColor = System.Drawing.Color.Gray;
            this.grdPaymentComplete.DisplayLayout.Override.FilteredInRowAppearance = appearance15;
            appearance16.BorderColor = System.Drawing.Color.Gray;
            this.grdPaymentComplete.DisplayLayout.Override.FilteredOutRowAppearance = appearance16;
            appearance17.BackColor = System.Drawing.Color.White;
            appearance17.BackColor2 = System.Drawing.Color.White;
            appearance17.BackColorDisabled = System.Drawing.Color.White;
            appearance17.BackColorDisabled2 = System.Drawing.Color.White;
            this.grdPaymentComplete.DisplayLayout.Override.FixedCellAppearance = appearance17;
            appearance18.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            appearance18.BackColor2 = System.Drawing.Color.Beige;
            this.grdPaymentComplete.DisplayLayout.Override.FixedHeaderAppearance = appearance18;
            appearance19.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(221)))), ((int)(((byte)(223)))));
            appearance19.BorderColor = System.Drawing.Color.White;
            appearance19.ForeColor = System.Drawing.Color.Black;
            appearance19.TextHAlignAsString = "Center";
            appearance19.ThemedElementAlpha = Infragistics.Win.Alpha.Transparent;
            this.grdPaymentComplete.DisplayLayout.Override.HeaderAppearance = appearance19;
            appearance20.BorderColor = System.Drawing.Color.Gray;
            this.grdPaymentComplete.DisplayLayout.Override.RowAlternateAppearance = appearance20;
            appearance21.BackColor = System.Drawing.Color.White;
            appearance21.BackColor2 = System.Drawing.Color.White;
            appearance21.BackGradientStyle = Infragistics.Win.GradientStyle.None;
            appearance21.BackHatchStyle = Infragistics.Win.BackHatchStyle.None;
            appearance21.BorderColor = System.Drawing.Color.Gray;
            this.grdPaymentComplete.DisplayLayout.Override.RowAppearance = appearance21;
            appearance22.BorderColor = System.Drawing.Color.Gray;
            this.grdPaymentComplete.DisplayLayout.Override.RowPreviewAppearance = appearance22;
            appearance23.BackColor = System.Drawing.Color.White;
            appearance23.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(231)))), ((int)(((byte)(230)))));
            appearance23.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance23.BorderColor = System.Drawing.Color.Gray;
            this.grdPaymentComplete.DisplayLayout.Override.RowSelectorAppearance = appearance23;
            this.grdPaymentComplete.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            this.grdPaymentComplete.DisplayLayout.Override.RowSelectorWidth = 12;
            this.grdPaymentComplete.DisplayLayout.Override.RowSizing = Infragistics.Win.UltraWinGrid.RowSizing.Fixed;
            this.grdPaymentComplete.DisplayLayout.Override.RowSizingArea = Infragistics.Win.UltraWinGrid.RowSizingArea.RowSelectorsOnly;
            this.grdPaymentComplete.DisplayLayout.Override.RowSpacingBefore = 2;
            appearance24.BackColor = System.Drawing.Color.Navy;
            appearance24.BackColorAlpha = Infragistics.Win.Alpha.Opaque;
            this.grdPaymentComplete.DisplayLayout.Override.SelectedCellAppearance = appearance24;
            appearance25.BackColor = System.Drawing.Color.Yellow;
            appearance25.BackColorDisabled = System.Drawing.Color.Silver;
            appearance25.BackGradientStyle = Infragistics.Win.GradientStyle.None;
            appearance25.BackHatchStyle = Infragistics.Win.BackHatchStyle.None;
            appearance25.BorderColor = System.Drawing.Color.Gray;
            appearance25.ForeColor = System.Drawing.Color.Black;
            this.grdPaymentComplete.DisplayLayout.Override.SelectedRowAppearance = appearance25;
            this.grdPaymentComplete.DisplayLayout.Override.SelectTypeCell = Infragistics.Win.UltraWinGrid.SelectType.None;
            this.grdPaymentComplete.DisplayLayout.Override.SelectTypeCol = Infragistics.Win.UltraWinGrid.SelectType.None;
            this.grdPaymentComplete.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.None;
            appearance26.BorderColor = System.Drawing.Color.Gray;
            this.grdPaymentComplete.DisplayLayout.Override.TemplateAddRowAppearance = appearance26;
            this.grdPaymentComplete.DisplayLayout.RowConnectorColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            this.grdPaymentComplete.DisplayLayout.RowConnectorStyle = Infragistics.Win.UltraWinGrid.RowConnectorStyle.Solid;
            appearance27.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(190)))), ((int)(((byte)(191)))), ((int)(((byte)(193)))));
            appearance27.BackColor2 = System.Drawing.Color.White;
            appearance27.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance27.BorderAlpha = Infragistics.Win.Alpha.Transparent;
            appearance27.ThemedElementAlpha = Infragistics.Win.Alpha.Transparent;
            scrollBarLook1.ButtonAppearance = appearance27;
            this.grdPaymentComplete.DisplayLayout.ScrollBarLook = scrollBarLook1;
            this.grdPaymentComplete.DisplayLayout.Scrollbars = Infragistics.Win.UltraWinGrid.Scrollbars.Vertical;
            this.grdPaymentComplete.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grdPaymentComplete.Location = new System.Drawing.Point(10, 262);
            this.grdPaymentComplete.Name = "grdPaymentComplete";
            this.grdPaymentComplete.Size = new System.Drawing.Size(473, 108);
            this.grdPaymentComplete.TabIndex = 10;
            this.grdPaymentComplete.UpdateMode = Infragistics.Win.UltraWinGrid.UpdateMode.OnCellChangeOrLostFocus;
            this.grdPaymentComplete.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // ultraDataSource2
            // 
            this.ultraDataSource2.Band.Columns.AddRange(new object[] {
            ultraDataColumn1,
            ultraDataColumn2,
            ultraDataColumn3,
            ultraDataColumn4});
            // 
            // ultraDataSource1
            // 
            this.ultraDataSource1.Band.Columns.AddRange(new object[] {
            ultraDataColumn5,
            ultraDataColumn6,
            ultraDataColumn7,
            ultraDataColumn8,
            ultraDataColumn9,
            ultraDataColumn10,
            ultraDataColumn11,
            ultraDataColumn12,
            ultraDataColumn13,
            ultraDataColumn14,
            ultraDataColumn15,
            ultraDataColumn16,
            ultraDataColumn17,
            ultraDataColumn18,
            ultraDataColumn19});
            // 
            // btnAddToStoreCredit
            // 
            appearance1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(62)))), ((int)(((byte)(176)))));
            appearance1.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance1.FontData.BoldAsString = "True";
            appearance1.ForeColor = System.Drawing.Color.White;
            this.btnAddToStoreCredit.Appearance = appearance1;
            this.btnAddToStoreCredit.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            this.btnAddToStoreCredit.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAddToStoreCredit.Location = new System.Drawing.Point(370, 11);
            this.btnAddToStoreCredit.Name = "btnAddToStoreCredit";
            this.btnAddToStoreCredit.Size = new System.Drawing.Size(103, 47);
            this.btnAddToStoreCredit.TabIndex = 20;
            this.btnAddToStoreCredit.Text = "Add To &Store Credit";
            this.btnAddToStoreCredit.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnAddToStoreCredit.Visible = false;
            this.btnAddToStoreCredit.Click += new System.EventHandler(this.btnAddToStoreCredit_Click);
            // 
            // lblStoreCreditAmount
            // 
            this.lblStoreCreditAmount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblStoreCreditAmount.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStoreCreditAmount.ForeColor = System.Drawing.Color.Black;
            this.lblStoreCreditAmount.Location = new System.Drawing.Point(262, 233);
            this.lblStoreCreditAmount.Name = "lblStoreCreditAmount";
            this.lblStoreCreditAmount.Size = new System.Drawing.Size(114, 20);
            this.lblStoreCreditAmount.TabIndex = 18;
            this.lblStoreCreditAmount.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblStoreCreditAmount.Visible = false;
            // 
            // lblStoreCreditAmtTitle
            // 
            this.lblStoreCreditAmtTitle.AutoSize = true;
            this.lblStoreCreditAmtTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStoreCreditAmtTitle.Location = new System.Drawing.Point(12, 233);
            this.lblStoreCreditAmtTitle.Name = "lblStoreCreditAmtTitle";
            this.lblStoreCreditAmtTitle.Size = new System.Drawing.Size(139, 15);
            this.lblStoreCreditAmtTitle.TabIndex = 17;
            this.lblStoreCreditAmtTitle.Tag = "NOCOLOR";
            this.lblStoreCreditAmtTitle.Text = "Store Credit Amount:";
            this.lblStoreCreditAmtTitle.Visible = false;
            // 
            // frmPOSChangeDue
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(7, 17);
            this.BackColor = System.Drawing.Color.SkyBlue;
            this.ClientSize = new System.Drawing.Size(502, 377);
            this.Controls.Add(this.lblStoreCreditAmount);
            this.Controls.Add(this.lblStoreCreditAmtTitle);
            this.Controls.Add(this.grdPaymentComplete);
            this.Controls.Add(this.lblChangeD);
            this.Controls.Add(this.lblAmtTendered);
            this.Controls.Add(this.lblTotTransAmt);
            this.Controls.Add(this.lblTransactionNo);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.groupBox1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmPOSChangeDue";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Transaction Complete";
            this.Activated += new System.EventHandler(this.frmPOSChangeDue_Activated);
            this.Closing += new System.ComponentModel.CancelEventHandler(this.frmPOSChangeDue_Closing);
            this.Load += new System.EventHandler(this.frmPOSChangeDue_Load);
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdPaymentComplete)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraDataSource2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraDataSource1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion

		private void frmPOSChangeDue_Load(object sender, System.EventArgs e)
		{
            this.Activate(); //Added by Manoj 9/29/2011 This is use to give this form active focus.
			this.Left=100;		//( frmMain.getInstance().Width-this.Width)/2;
			this.Top=100;			//(frmMain.getInstance().Height-this.Height)/2;
			clsUIHelper.setColorSchecme(this);
            if (Configuration.CInfo.UseEmailInvoice && !Configuration.CInfo.OutGoingEmailPromptAutomatically)
            { this.btnEmail.Visible = true; }

            #region StoreCredit PRIMEPOS-2747 - NileshJ - 20-Nov-2019
            if (Configuration.CPOSSet.EnableStoreCredit)
            {
                if (lblChangeD.Text != string.Empty && lblChangeD.Text != "$0.00" && CustomerCode != "-1" && oTransheaderRow.TransType == 1)
                {
                    btnAddToStoreCredit.Visible = false;
                }
                else if (oPOSTransPayment.AsEnumerable().Where(x => x.Field<string>("TransTypeCode").Trim().ToUpper() == "X").Count() > 0 && oTransheaderRow.TransType == 1 && lblChangeD.Text != "$0.00")
                {
                    btnAddToStoreCredit.Visible = false;
                }
                else
                {
                    btnAddToStoreCredit.Visible = false;
                }
            }
            #endregion
        }

        private void frmPOSChangeDue_Activated(object sender, System.EventArgs e)
		{
			clsUIHelper.CurrentForm=this;
		}

		private void frmPOSChangeDue_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			//frmMain.PoleDisplay.WriteToPoleDisplay("");
		}

        private void btnClose_Click(object sender, EventArgs e)
        {
            try
            {
                if (Configuration.CInfo.UseEmailInvoice && Configuration.CInfo.OutGoingEmailPromptAutomatically)
                {
                    //if (custEmail.Trim() != "")
                    {
                        frmSendEmail email = new frmSendEmail(Configuration.convertNullToInt(tranSno.Trim()));                        
                        email.ShowDialog();
                    }
                    
                }
                #region PRIMEPOS-2938 29-Jan-2021 JY Commented - moved this logic inside the transaction
                //#region StoreCredit PRIMEPOS-2747 - NileshJ - 20-Nov-2019 - 11-Nov-2019
                //if (Configuration.CPOSSet.EnableStoreCredit)
                //{
                //    if (IsStoreCredit)
                //    {
                //        DataSet dsAlreadyStoreCreditCustomer = new DataSet();
                //        int StoreCreditID;
                //        var CreditAmount = oPOSTransPayment.AsEnumerable().Where(x => x.Field<string>("TransTypeCode").Trim().ToUpper() == "X").Select(p => p.Field<decimal>("Amount")).FirstOrDefault();
                //        if (oStoreCreditData != null)
                //        {
                //            //if (oTransheaderRow.TransType == 1)
                //            // {
                //            //if (CreditAmount > 0)
                //            //{
                //            CreditAmount = (-1) * CreditAmount;
                //            //}
                //            //}


                //            if (oStoreCreditData.Tables[0].Rows.Count > 0)
                //            {
                //                if (oTransheaderRow.CustomerID.ToString() != "")
                //                {
                //                    dsAlreadyStoreCreditCustomer = oStoreCredit.GetByCustomerID(Convert.ToInt32(oTransheaderRow.CustomerID));
                //                    if (dsAlreadyStoreCreditCustomer.Tables[0].Rows.Count > 0)
                //                    {
                //                        oStoreCredit.UpdateCreditAmount(oStoreCreditData);
                //                        StoreCreditID = Convert.ToInt32(dsAlreadyStoreCreditCustomer.Tables[0].Rows[0]["StoreCreditID"].ToString());
                //                    }
                //                    else
                //                    {
                //                        oStoreCreditRow = oStoreCreditData.StoreCredit.AddRow(0, Convert.ToInt32(oTransheaderRow.CustomerID.ToString()),
                //                            Convert.ToDecimal(oStoreCreditData.Tables[0].Rows[0]["CreditAmt"]), DateTime.Now, Configuration.UserName, 0);
                //                        oStoreCredit.Persist(oStoreCreditData);
                //                        oStoreCreditData = oStoreCredit.GetByCustomerID(Convert.ToInt32(oTransheaderRow.CustomerID));
                //                        StoreCreditID = Convert.ToInt32(oStoreCreditData.Tables[0].Rows[0]["StoreCreditID"].ToString());
                //                    }
                //                    if (oStoreCreditDetailsData != null)
                //                    {
                //                        oStoreCreditDetailsData.StoreCreditDetails.Rows.Clear();
                //                    }
                //                    oStoreCreditDetailsRow = oStoreCreditDetailsData.StoreCreditDetails.AddRow(0, StoreCreditID, Convert.ToInt32(oTransheaderRow.CustomerID.ToString()),
                //                        Convert.ToInt32(oTransheaderRow.TransID.ToString()), Convert.ToDecimal(CreditAmount), DateTime.Now, Configuration.UserName);
                //                    oStoreCreditDetails.Persist(oStoreCreditDetailsData);
                //                }
                //            }
                //        }
                //        else
                //        {

                //            DataRow[] myResultSet = oPOSTransPayment.Select("TransTypeCode = 'X'");

                //            if (myResultSet.Length > 0)
                //            {
                //                if (oTransheaderRow.TransType == 2)
                //                {
                //                    CreditAmount = (-1) * CreditAmount;
                //                }
                //                oStoreCreditData = new StoreCreditData();
                //                if (oStoreCreditData != null)
                //                {
                //                    oStoreCreditData.StoreCredit.Rows.Clear();
                //                }
                //                oStoreCreditRow = oStoreCreditData.StoreCredit.AddRow(0, Convert.ToInt32(oTransheaderRow.CustomerID.ToString()), Convert.ToDecimal(CreditAmount), DateTime.Now, Configuration.UserName, 0);
                //                oStoreCredit.Persist(oStoreCreditData);
                //                oStoreCreditData = oStoreCredit.GetByCustomerID(Convert.ToInt32(oTransheaderRow.CustomerID));
                //                StoreCreditID = Convert.ToInt32(oStoreCreditData.Tables[0].Rows[0]["StoreCreditID"].ToString());
                //                oStoreCreditDetailsRow = oStoreCreditDetailsData.StoreCreditDetails.AddRow(0, StoreCreditID, Convert.ToInt32(oTransheaderRow.CustomerID.ToString()), Convert.ToInt32(oTransheaderRow.TransID.ToString()), Convert.ToDecimal(CreditAmount), DateTime.Now, Configuration.UserName);
                //                oStoreCreditDetails.Persist(oStoreCreditDetailsData);
                //            }
                //        }
                //    }
                //}
                //#endregion
                #endregion
            }
            catch (Exception)
            {

                //throw;
            }
        }
        private String strWhere;
        private String strSubQuery;
        string strCQuery = "";
       
       string  strQuery="";
       rptPrintTransaction oRptPrintTrancsaction = null;       
       private void PopulateChargeAcct(RxLabel oRxlabel)
       {

           try
           {
               try
               {
                   clsReports.setCRTextObjectText("sCustomerName", "Customer Name : " + GetCustomerName(CustID), oRptPrintTrancsaction);
                   //oRptPrintTrancsaction.se("sCustomerName", "Customer Name : " + GetCustomerName(CustID));
               }
               catch (Exception)
               {
                   
                  // throw;
               }
               oRptPrintTrancsaction.SetParameterValue("sAccCode", Configuration.convertNullToString(oRxlabel.AccCode));
               oRptPrintTrancsaction.SetParameterValue("sAccName", Configuration.convertNullToString(oRxlabel.AccName));
               oRptPrintTrancsaction.SetParameterValue("AcctAmount", Configuration.convertNullToString(oRxlabel.AccAmount));

               oRptPrintTrancsaction.SetParameterValue("CurrBalance", Configuration.convertNullToString(oRxlabel.AccCurrBalance));

               if (oRxlabel.TransactionTypeCode == 3)
               {
                    oRptPrintTrancsaction.SetParameterValue("sChargeAccType", "Received On Account :");
                    string hcReference = Configuration.convertNullToString(oRxlabel.HCReference);
                    if (hcReference.Length > 25)
                    {
                        oRptPrintTrancsaction.SetParameterValue("HCReference", hcReference.Substring(0, 25) + "..");
                        if (hcReference.Length > 25)
                        {
                            oRptPrintTrancsaction.SetParameterValue("HCReference2", hcReference.Substring(25));
                        }
                        else
                        {
                            oRptPrintTrancsaction.SetParameterValue("HCReference2", "");
                        }
                    }
                    else
                    {
                        oRptPrintTrancsaction.SetParameterValue("HCReference", hcReference);
                        oRptPrintTrancsaction.SetParameterValue("HCReference2", "");
                    }
                }
               else
               {
                   oRptPrintTrancsaction.SetParameterValue("sChargeAccType", "Amount Charged :");
                    oRptPrintTrancsaction.SetParameterValue("HCReference", "");
                    oRptPrintTrancsaction.SetParameterValue("HCReference2", "");
                }

            }
           catch { }
       }
        string CurrentInvoiceNo;
        bool misROATrans = false;
        private void ClearSubReportPearameters(RxLabel oRxlabel)
        {

            oRptPrintTrancsaction.SetParameterValue(oRptPrintTrancsaction.Parameter_Pharmacyname_.ParameterFieldName, POS_Core.Resources.Configuration.CInfo.StoreName);
            oRptPrintTrancsaction.SetParameterValue(oRptPrintTrancsaction.Parameter_PhramecyAddress.ParameterFieldName, oRxlabel.CAddress);
            oRptPrintTrancsaction.SetParameterValue(oRptPrintTrancsaction.Parameter_txtPharmacyCityStateZip.ParameterFieldName, Configuration.convertNullToString(oRxlabel.CCity) + ", " + Configuration.convertNullToString(oRxlabel.CState) + " " + Configuration.convertNullToString(oRxlabel.CZip));  //PRIMEPOS-2560 16-Jul-2018 JY Added
            oRptPrintTrancsaction.SetParameterValue(oRptPrintTrancsaction.Parameter_TelephoneNo.ParameterFieldName, oRxlabel.CTelephone);

            oRptPrintTrancsaction.SetParameterValue(oRptPrintTrancsaction.Parameter_MasterCard.ParameterFieldName, "");
            //Added By SRT(Ritesh Parekh) Date : 25-Jul-2009
            //oRptPrintTrancsaction.SetParameterValue(oRptPrintTrancsaction.Parameter_rptViewTransPayment_PayTypeDesc.ParameterFieldName, "Card Type :");
            //End Of Added By SRT(Ritesh Parekh)
            oRptPrintTrancsaction.SetParameterValue("MasterCard", "");
            oRptPrintTrancsaction.SetParameterValue("Charge", "0");
            oRptPrintTrancsaction.SetParameterValue(oRptPrintTrancsaction.Parameter_Merchant.ParameterFieldName, "0");
            oRptPrintTrancsaction.SetParameterValue("Merchant", "0");
            oRptPrintTrancsaction.SetParameterValue("Authority", "0");
            oRptPrintTrancsaction.SetParameterValue(oRptPrintTrancsaction.Parameter_TotlaAmount.ParameterFieldName, "0");
            oRptPrintTrancsaction.ReportFooterSection2.SectionFormat.EnableSuppress = true;

        }
        private string GetCustomerName(string CstID)
        {
            Customer oCustomer = new Customer();
            CustomerData oCustdata = new CustomerData();
            CustomerRow oCustRow = null;
            try
            {
                oCustdata = oCustomer.GetCustomerByID(Configuration.convertNullToInt(CstID));
                if (oCustdata.Tables[0].Rows.Count > 0)
                {
                    oCustRow = (CustomerRow)oCustdata.Customer.Rows[0];
                    CustName = oCustRow.CustomerName + " " + oCustRow.FirstName;
                    custEmail = oCustRow.Email.ToLower();   //PRIMEPOS-2903 06-Oct-2020 JY Modified

                    return oCustRow.CustomerName + " " + oCustRow.FirstName;
                }
                return "";
            }
            catch (Exception)
            {
                return "";
                //throw;
            }
        }
            
       private void PrintPreview()
       {
           try
           {
               
               this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
               //rptViewTransaction oRptViewTrans= new rptViewTransaction();

               Search oSearch = new Search();
               strCQuery = strQuery + strWhere;
               DataSet ds = oSearch.SearchData(strCQuery);
               //oRptViewTrans.Database.Tables[0].SetDataSource(ds.Tables[0]);
               //Following Commented part UnCommented by Krishna on 17 May 2011
               if (ds != null)
               {
                   if (ds.Tables[0].Rows.Count > 0)
                   {
                       CurrentInvoiceNo = this.tranSno;
                       //Added By Shitaljit for ROA transaction.
                       if (POS_Core.Resources.Configuration.convertNullToString(ds.Tables[0].Rows[0]["TransType"].ToString().Trim()).Equals("ROA"))
                       {
                           misROATrans = true;
                       }
                   }
                   else
                   {
                       this.Cursor = System.Windows.Forms.Cursors.Default;
                       return;
                   }
               }
               //Comments removed till here by krishna

               DataSet subDs = oSearch.SearchData(strSubQuery + " and PT.TransID=" + CurrentInvoiceNo);
               #region Populate Values
               Customer oCustomer = new Customer();
               CustomerData oCustdata = new CustomerData();
               CustomerRow oCustRow = null;

               oCustdata = oCustomer.GetCustomerByID(Configuration.convertNullToInt(ds.Tables[0].Rows[0]["CustomerID"].ToString().Trim()));
               if (oCustdata.Tables[0].Rows.Count > 0)
               {
                   oCustRow = (CustomerRow)oCustdata.Customer.Rows[0];
                  
               }             


               
               int RowIndex = 0;
               DataSet oDataSet = new DataSet();
               oDataSet.Tables.Add("TransDetail");
               oDataSet.Tables[0].Columns.Add("Qty", Type.GetType("System.String"));
               oDataSet.Tables[0].Columns.Add("ItemDescription", Type.GetType("System.String"));
               oDataSet.Tables[0].Columns.Add("Price", Type.GetType("System.String"));
               oDataSet.Tables[0].Columns.Add("Discount", Type.GetType("System.String"));
               oDataSet.Tables[0].Columns.Add("TaxCode", Type.GetType("System.String"));
               oDataSet.Tables[0].Columns.Add("TaxAmount", Type.GetType("System.String"));
               oDataSet.Tables[0].Columns.Add("ExtendedPrice", Type.GetType("System.String"));

               foreach (DataRow oRow in ds.Tables[0].Rows)
               {
                   DataRow drTranDetRow = oDataSet.Tables[0].NewRow();
                   drTranDetRow[0] = Configuration.convertNullToString(oRow["Qty"]);
                   drTranDetRow[1] = Configuration.convertNullToString(oRow["Description"]);
                   drTranDetRow[2] = Configuration.convertNullToDecimal(oRow["Price"]).ToString("########0.00");
                   drTranDetRow[3] = Configuration.convertNullToDecimal(oRow["Discount"]).ToString("########0.00");
                   drTranDetRow[4] = Configuration.convertNullToString(oRow["TaxID"]);
                   drTranDetRow[5] = Configuration.convertNullToDecimal(oRow["TaxAmount"]).ToString("########0.00");
                   drTranDetRow[6] = Configuration.convertNullToDecimal(oRow["ExtendedPrice"]).ToString("########0.00");
                   oDataSet.Tables[0].Rows.Add(drTranDetRow);
                   RowIndex++;

               }
               //FillListView(subDs);
               
               //MaxInvoiceNo = oTHSvr.GetMaxTransId();

               
               #endregion

               #region Commented Code


              
               #endregion
               strWhere = "";
              

           }
           catch (Exception exp)
           {
               this.Cursor = System.Windows.Forms.Cursors.Default;
               clsUIHelper.ShowErrorMsg(exp.Message);
           }
       }
       private void PrintCC(RxLabel oRxlabel)
       {


           oRptPrintTrancsaction.SetParameterValue(oRptPrintTrancsaction.Parameter_Pharmacyname_.ParameterFieldName, POS_Core.Resources.Configuration.CInfo.StoreName);
           oRptPrintTrancsaction.SetParameterValue(oRptPrintTrancsaction.Parameter_PhramecyAddress.ParameterFieldName, oRxlabel.CAddress);
            oRptPrintTrancsaction.SetParameterValue(oRptPrintTrancsaction.Parameter_txtPharmacyCityStateZip.ParameterFieldName, Configuration.convertNullToString(oRxlabel.CCity) + ", " + Configuration.convertNullToString(oRxlabel.CState) + " " + Configuration.convertNullToString(oRxlabel.CZip));  //PRIMEPOS-2560 16-Jul-2018 JY Added
            oRptPrintTrancsaction.SetParameterValue(oRptPrintTrancsaction.Parameter_TelephoneNo.ParameterFieldName, oRxlabel.CTelephone);
           oRptPrintTrancsaction.SetParameterValue(oRptPrintTrancsaction.Parameter_MasterCard.ParameterFieldName, (oRxlabel.CCPaymentRow == null ? "" : oRxlabel.CCPaymentRow.RefNo.ToString()));
           //Added By SRT(Ritesh Parekh) Date : 25-Jul-2009
           //oRptPrintTrancsaction.SetParameterValue(oRptPrintTrancsaction.Parameter_rptViewTransPayment_PayTypeDesc.ParameterFieldName, (oRxlabel.CCPaymentRow == null ? "Card:" : oRxlabel.CCPaymentRow.TransTypeDesc.ToString()+":"));
           //End OF Added By SRT(Ritesh Parekh)
           oRptPrintTrancsaction.SetParameterValue(oRptPrintTrancsaction.Parameter_Charge.ParameterFieldName, Convert.ToString(oRxlabel.TransH(0).TenderedAmount - oRxlabel.TransH(0).TotalPaid));
           oRptPrintTrancsaction.SetParameterValue(oRptPrintTrancsaction.Parameter_Merchant.ParameterFieldName, (oRxlabel.CMerchantNo == null ? "" : oRxlabel.CMerchantNo));
           oRptPrintTrancsaction.SetParameterValue(oRptPrintTrancsaction.Parameter_Authority.ParameterFieldName, (oRxlabel.CCPaymentRow == null ? "" : oRxlabel.CCPaymentRow.AuthNo.ToString()));
           oRptPrintTrancsaction.SetParameterValue(oRptPrintTrancsaction.Parameter_TotlaAmount.ParameterFieldName, (oRxlabel.CCPaymentRow == null ? "" : oRxlabel.CCPaymentRow.Amount.ToString()));

           oRptPrintTrancsaction.ReportFooterSection2.SectionFormat.EnableSuppress = false;
       }
        //((CrystalDecisions.CrystalReports.Engine.TextObject)oRptViewTrans.ReportDefinition.ReportObjects["txtHeader"]).Text = Configuration.CInfo.StoreName;

        public void CreateTransReceiptForEmail(int TransID)
        {
            oRptPrintTrancsaction = new rptPrintTransaction(); 
            strQuery = " select PT.TransID,PT.TransDate,PT.CustomerID,PT.UserID,PT.StationID,PT.GrossTotal,PT.TotalDiscAmount, PT.TotalTaxAmount, PT.TotalPaid ," +
                           " Case TransType when 1 Then 'Sale' when 2 Then 'Return' when 3 then 'ROA' end as TransType, " +
                           " PTD.Qty,PTD.ItemID, PTD.ItemDescription as Description, PTD.Price,Tx.TaxCode as TaxID,PTD.TaxAmount,PTD.Discount,PTD.ExtendedPrice,ps.stationname, PT.TotalTransFeeAmt " + //PRIMEPOS-3119 11-Aug-2022 JY Added TotalTransFeeAmt
                           " from postransaction PT left join POSTransactionDetail PTD on PT.TransID=PTD.TransID " +
                           " left join Item I on I.ItemID=PTD.ItemID  " +
                           " left join taxcodes tx on (ptd.taxid=tx.taxid) " +
                           " left join util_POSSet ps on (ps.stationid=pt.stationid) where 1=1 ";   //and PT.StationID='" + StationID.Trim() + "'";

            //Modified The Sub Query By shitaljit To Dispaly Payments of singgle Paytype togethr in single Row.
            strSubQuery = "SELECT PAY.PAYTYPEDESC AS DESCRIPTION,cast(PT.TransAmt as varchar) as TransAmt, " +
                        " case CHARINDEX('|',isnull(refno,'')) when  0 then refno else '******'+right(rtrim(left(refno,CHARINDEX('|',refno)-1)) ,4) End as RefNo ,PT.TRANSID " +
                // " FROM POSTRANSPAYMENT PT,PAYTYPE PAY where PT.TransTypeCode=Pay.PayTypeID ";
                        " FROM PAYTYPE PAY, (Select RefNo,Transid, TransTypeCode, sum(POSTransPayment.Amount) " +
                        "as TransAmt from POSTRANSPAYMENT  GROUP BY TransTypeCode,Transid,RefNo)  as PT WHERE PT.TransTypeCode = Pay.PayTypeID";


            if (TransID.ToString().Trim() != "" && TransID.ToString().Trim() != "0")
                strWhere = " and PT.TransID=" + TransID.ToString().Trim();            
           
            PrintPreview();

            try
            {
                try
                {
                    if (TransID < 1)
                    {
                        return;
                    }

                    TransHeaderData oTransHData;
                    TransHeaderSvr oTransHSvr = new TransHeaderSvr();

                    TransDetailData oTransDData;
                    TransDetailSvr oTransDSvr = new TransDetailSvr();

                    POSTransPaymentData oTransPaymentData;
                    POSTransPaymentSvr oTransPaymentSvr = new POSTransPaymentSvr();

                    //added by atul 07-jan-2011
                    TransDetailRXData oTransRxData;
                    TransDetailRXSvr oTransRxSvr = new TransDetailRXSvr();
                    //End of added by atul 07-jan-2011

                    oTransHData = oTransHSvr.Populate(TransID);

                    oTransDData = oTransDSvr.PopulateData(TransID);

                    oTransPaymentData = oTransPaymentSvr.Populate(TransID);

                    TransDetailTaxSvr oTransDetailTaxSvr = new TransDetailTaxSvr(); //Sprint-26 - PRIMEPOS-2445 28-Aug-2017 JY Added
                    DataTable dtTransDetailTax = oTransDetailTaxSvr.GetTransDetailTax(TransID); //Sprint-26 - PRIMEPOS-2445 28-Aug-2017 JY Added

                    //added by atul 07-jan-2011
                    RxLabel oRxLabel;
                    if (oTransDData != null && oTransDData.Tables[0].Rows.Count > 0)
                    {
                        //if (oTransDData.TransDetail[0].ItemID.ToString().Trim().ToUpper() == "RX")    //Sprint-23 - PRIMEPOS-2319 24-Jun-2016 JY Commented as its checking only first item
                        oTransRxData = oTransRxSvr.PopulateData(TransID);   //Sprint-23 - PRIMEPOS-2319 24-Jun-2016 JY Added
                        if (oTransRxData != null && oTransRxData.Tables.Count > 0 && oTransRxData.Tables[0].Rows.Count > 0)  //Sprint-23 - PRIMEPOS-2319 24-Jun-2016 JY Added condtion to check whether rx item exists in transaction
                        {
                            //oTransRxData = oTransRxSvr.PopulateData(TransID); //Sprint-23 - PRIMEPOS-2319 24-Jun-2016 JY Commented
                            oRxLabel = new RxLabel(oTransHData, oTransDData, oTransPaymentData, oTransRxData, ReceiptType.Void, dtTransDetailTax);
                        } //end of added by atul 07-jan-2011
                        else
                        {
                            oRxLabel = new RxLabel(oTransHData, oTransDData, oTransPaymentData, ReceiptType.SalesTransactionReprint, dtTransDetailTax);
                        }
                    }
                    else
                    {
                        oRxLabel = new RxLabel(oTransHData, oTransDData, oTransPaymentData, ReceiptType.SalesTransactionReprint, dtTransDetailTax);
                    }

                    oRptPrintTrancsaction = new rptPrintTransaction();
                    Search oSearch = new Search();
                    DataSet ds = oSearch.SearchData(strCQuery);

                    ds.Tables[0].Columns.Add("CustomerSignature", typeof(System.Byte[]));
                    int index = 0;
                    DataTable oChargeAccTab = new DataTable();
                    oChargeAccTab.Columns.Add("TransID", System.Type.GetType("System.Int32"));
                    oChargeAccTab.Columns.Add("AcctSignature", typeof(System.Byte[]));

                    oTransPaymentData.Tables[0].Columns.Add("CustomerSignature", typeof(System.Byte[]));
                    oTransPaymentData.Tables[0].Columns.Add("UserID", typeof(System.String));
                    oTransPaymentData.Tables[0].Columns.Add("TransType", typeof(System.String));

                    //**************
                    Bitmap bit = null;
                    System.IO.MemoryStream oStream = null;
                    //*************

                    foreach (DataRow oRow in ds.Tables[0].Rows)
                    {
                        try
                        {
                            if (oTransPaymentData.Tables[0].Rows[0]["BinarySign"] != System.DBNull.Value) //Added by prashant(SRT) 28-sep-2010
                            {
                                //MemoryStream ms = new MemoryStream((byte[])oTransPaymentData.Tables[0].Rows[0]["BinarySign"]);
                                //bit = new Bitmap(ms);

                                #region PRIMEPOS-2900 15-Sep-2020 JY Added if part for Vantiv
                                try
                                {
                                    if (Configuration.convertNullToString(oTransPaymentData.Tables[0].Rows[0][clsPOSDBConstants.POSTransPayment_Fld_PaymentProcessor]).Trim().ToUpper() == "VANTIV")
                                    {
                                        Byte[] strBinSign = (byte[])oTransPaymentData.Tables[0].Rows[0]["BinarySign"];
                                        bit = POS_Core.Resources.DelegateHandler.clsCoreUIHelper.ConvertPoints(strBinSign);
                                    }
                                    else
                                    {
                                        MemoryStream ms = new MemoryStream((byte[])oTransPaymentData.Tables[0].Rows[0]["BinarySign"]);
                                        bit = new Bitmap(ms);
                                    }
                                }
                                catch { }
                                #endregion
                            }
                            else
                            {
                                bit = clsUIHelper.GetSignature(oTransPaymentData.Tables[0].Rows[0]["CustomerSign"].ToString(), POS_Core.Resources.Configuration.CInfo.SigType);
                            }
                            oStream = new System.IO.MemoryStream();
                            if (bit != null)
                            {
                                bit.Save(oStream, System.Drawing.Imaging.ImageFormat.Jpeg);
                                oRow["CustomerSignature"] = oStream.ToArray();
                            }
                            oRow["Description"] = oRxLabel.TransD(index).ItemDescription;
                            index++;
                        }
                        catch { }
                    }

                    foreach (DataRow oRow in oTransPaymentData.POSTransPayment.Rows)
                    {
                        oRow["TransType"] = ds.Tables[0].Rows[0]["TransType"].ToString();
                        oRow["UserID"] = ds.Tables[0].Rows[0]["UserID"].ToString();

                        switch (oRow[clsPOSDBConstants.POSTransPayment_Fld_TransTypeCode].ToString().Trim())
                        {
                            case "3":
                            case "4":
                            case "5":
                            case "6":
                            case "7":
                            case "H":
                                if (oRow["BinarySign"] != System.DBNull.Value) //Added by prashant(SRT) 28-sep-2010
                                {
                                    //MemoryStream ms = new MemoryStream((byte[])oTransPaymentData.Tables[0].Rows[0]["BinarySign"]);
                                    //bit = new Bitmap(ms);

                                    #region PRIMEPOS-2900 15-Sep-2020 JY Added if part for Vantiv
                                    try
                                    {
                                        if (Configuration.convertNullToString(oRow[clsPOSDBConstants.POSTransPayment_Fld_PaymentProcessor]).Trim().ToUpper() == "VANTIV")
                                        {
                                            Byte[] strBinSign = (byte[])oRow["BinarySign"];
                                            bit = POS_Core.Resources.DelegateHandler.clsCoreUIHelper.ConvertPoints(strBinSign);
                                        }
                                        else
                                        {
                                            MemoryStream ms = new MemoryStream((byte[])oRow["BinarySign"]);
                                            bit = new Bitmap(ms);
                                        }
                                    }
                                    catch { }
                                    #endregion
                                }
                                else
                                {
                                    bit = clsUIHelper.GetSignature(oRow[clsPOSDBConstants.POSTransPayment_Fld_CustomerSign].ToString(), POS_Core.Resources.Configuration.CInfo.SigType);
                                }

                                oStream = new System.IO.MemoryStream();
                                if (bit != null)
                                {
                                    bit.Save(oStream, System.Drawing.Imaging.ImageFormat.Jpeg);
                                    oRow["CustomerSignature"] = oStream.ToArray();
                                }
                                if (oRow[clsPOSDBConstants.POSTransPayment_Fld_TransTypeCode].ToString().Trim() == "H")
                                {
                                    DataRow oCRow = oChargeAccTab.NewRow();
                                    oCRow["TransID"] = oRow[clsPOSDBConstants.POSTransPayment_Fld_TransID].ToString();
                                    if (oStream != null)
                                        oCRow["AcctSignature"] = oStream.ToArray();
                                    oChargeAccTab.Rows.Add(oCRow);
                                }
                                break;
                        }
                    }

                    oRptPrintTrancsaction.ReportOptions.EnableSaveDataWithReport = false;
                    oRptPrintTrancsaction.Database.Tables[0].SetDataSource(ds.Tables[0]);
                    oRptPrintTrancsaction.Database.Tables[1].SetDataSource(oChargeAccTab);
                    oRptPrintTrancsaction.Subreports["rptPaymentDetail"].Database.Tables[0].SetDataSource(oTransPaymentData.Tables[0]);
                    oRptPrintTrancsaction.Subreports["rptViewTransPayment"].Database.Tables[0].SetDataSource(oTransPaymentData.Tables[0]);

                    if (oRxLabel.AccCode.Trim() == "")
                    {
                        oRptPrintTrancsaction.ReportFooterSection3.SectionFormat.EnableSuppress = true;
                    }
                    else
                    {
                        oRptPrintTrancsaction.ReportFooterSection3.SectionFormat.EnableSuppress = false;
                    }

                    PopulateChargeAcct(oRxLabel);
                    ClearSubReportPearameters(oRxLabel);
                
                    if (oRxLabel.MergeCCWithRecpt == true)
                    {
                        if (oRxLabel.CCPaymentRow == null || oRxLabel.CCPaymentRow.RefNo.ToString() == "")
                        {
                            ClearSubReportPearameters(oRxLabel);
                        }
                        else
                        {
                            {
                                PrintCC(oRxLabel); //add compies of pos credit card transcation report
                            }
                        }
                    }
                    else
                    {
                        ClearSubReportPearameters(oRxLabel);
                        //oRptPrintTrancsaction.PrintToPrinter(1, false, 1, 1);           
                    }
                    oRptPrintTrancsaction.SetParameterValue(oRptPrintTrancsaction.Parameter_TotalQuantity.ParameterFieldName, oRxLabel.TotalQty.ToString());
                    try
                    {
                        {
                            string emailSubject="Invoice :Transaction No:"+TransID+"Purchase Date: "+TransDate.ToString();
                            string Emailbody= Configuration.CInfo.OutGoingEmailBody.ToString();
                         
                           // clsReports.EmailReport(oRptPrintTrancsaction, CustName, custEmail, emailSubject, Emailbody, "INV" + TransID.ToString());
                            //oRptPrintTrancsaction.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, sFileName);
                        }
                    }
                    catch (Exception ex)
                    {
                        clsUIHelper.ShowErrorMsg(ex.Message);
                    }
                }
                catch (Exception exp)
                {
                    clsUIHelper.ShowErrorMsg(exp.Message);
                }
            }
            catch { }
        }

       private void btnEmail_Click(object sender, EventArgs e)
       {
           try
           {
               if (Configuration.CInfo.UseEmailInvoice && !Configuration.CInfo.OutGoingEmailPromptAutomatically)
               {
                   //if (custEmail.Trim() != "")
                   {
                       frmSendEmail email = new frmSendEmail(Configuration.convertNullToInt(tranSno.Trim()));
                       email.ShowDialog();
                       if (!frmSendEmail.IsCanceled)
                       {
                           this.Close();
                       }
                       
                   }

               }
           }
           catch (Exception)
           {

               //throw;
           }
       }
        #region StoreCredit NileshJ PRIMEPOS-2747
        private void btnAddToStoreCredit_Click(object sender, EventArgs e)
        {
            frmStoreCreditAdd ofrm = new frmStoreCreditAdd(oTransheaderRow.CustomerID.ToString(), oTransheaderRow.TransID.ToString(), oTransheaderRow.TransDate.ToString(), storeCreditAmount);
            ofrm.ShowDialog(this);
            if (!ofrm.IsSuccess)
            {
                return;
            }
            else
            {
                if (ofrm.CreditAmount <= storeCreditAmount)
                {
                    decimal changeDue = storeCreditAmount - ofrm.CreditAmount;
                    btnAddToStoreCredit.Visible = false;
                    lblStoreCreditAmount.Visible = true;
                    lblStoreCreditAmtTitle.Visible = true;
                    lblStoreCreditAmount.Text = storeCreditAmount.ToString();
                    this.lblChangeDue.Text = changeDue.ToString(Configuration.CInfo.CurrencySymbol + "######0.00");
                    this.lblChangeD.Text = changeDue.ToString(Configuration.CInfo.CurrencySymbol + "######0.00");
                    this.Height = 300;
                    btnClose.Focus();
                }
            }
        }
        #endregion
    }
}
