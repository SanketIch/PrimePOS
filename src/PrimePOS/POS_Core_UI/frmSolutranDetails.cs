using Infragistics.Win.UltraWinGrid;
using NLog;
using System;
using System.Data;
using System.Windows.Forms;
using System.Drawing;

namespace POS_Core_UI
{
    public partial class frmSolutranDetails : System.Windows.Forms.Form
    {
        private ILogger logger = LogManager.GetCurrentClassLogger();
        public bool IsCanceled = true;
        public bool IsReturnTransaction = false;
        public DataTable dtS3Items = new DataTable();

        public frmSolutranDetails()
        {
            InitializeComponent();
        }

        #region FORM EVENT
        private void frmSolutranDetails_Load(object sender, EventArgs e)
        {
            if(IsReturnTransaction)
            {
                this.lblClose.Enabled = false;
                this.btnClose.Enabled = false;
            }
            Display();
        }

        private void frmSolutranDetails_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Alt)
            {
                ShortCutKeyAction(e.KeyCode);
            }
            //else if (e.KeyData == Keys.Escape)
            //{
            //    this.Close();
            //}
        }

        private void frmSolutranDetails_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyData)
            {
                case Keys.F2:
                    btnProcess_Click(null, null);
                    break;
            }
        }
        #endregion

        #region Methods
        private void Display()
        {
            try
            {
                grdSolutranDetails.DataSource = dtS3Items;
                grdSolutranDetails.DataBind();
                GridColumnDisplay();
            }
            catch (Exception Ex)
            {
                //clsUIHelper.ShowErrorMsg(Ex.Message);
                logger.Fatal(Ex, "Search()");
            }
        }

        private void GridColumnDisplay()
        {
            if (grdSolutranDetails != null && grdSolutranDetails.Rows.Count > 0)
            {
                #region Hide columns
                this.grdSolutranDetails.DisplayLayout.Bands[0].Columns["TransDetailId"].Hidden = true;
                this.grdSolutranDetails.DisplayLayout.Bands[0].Columns["TransId"].Hidden = true;
                this.grdSolutranDetails.DisplayLayout.Bands[0].Columns["Discount"].Hidden = true;
                this.grdSolutranDetails.DisplayLayout.Bands[0].Columns["TaxAmount"].Hidden = true;
                this.grdSolutranDetails.DisplayLayout.Bands[0].Columns["ExtendedPrice"].Hidden = true;
                this.grdSolutranDetails.DisplayLayout.Bands[0].Columns["Price"].Hidden = true;
                this.grdSolutranDetails.DisplayLayout.Bands[0].Columns["TaxID"].Hidden = true;
                this.grdSolutranDetails.DisplayLayout.Bands[0].Columns["TaxCode"].Hidden = true;
                this.grdSolutranDetails.DisplayLayout.Bands[0].Columns["ItemCost"].Hidden = true;
                this.grdSolutranDetails.DisplayLayout.Bands[0].Columns["UserID"].Hidden = true;
                this.grdSolutranDetails.DisplayLayout.Bands[0].Columns["IsPriceChanged"].Hidden = true;
                this.grdSolutranDetails.DisplayLayout.Bands[0].Columns["IsIIAS"].Hidden = true;
                this.grdSolutranDetails.DisplayLayout.Bands[0].Columns["IsCompanionItem"].Hidden = true;
                this.grdSolutranDetails.DisplayLayout.Bands[0].Columns["IsRxItem"].Hidden = true;
                this.grdSolutranDetails.DisplayLayout.Bands[0].Columns["IsEBT"].Hidden = true;
                this.grdSolutranDetails.DisplayLayout.Bands[0].Columns["ReturnTransDetailId"].Hidden = true;
                this.grdSolutranDetails.DisplayLayout.Bands[0].Columns["IsMonitored"].Hidden = true;
                this.grdSolutranDetails.DisplayLayout.Bands[0].Columns["Category"].Hidden = true;
                this.grdSolutranDetails.DisplayLayout.Bands[0].Columns["NonComboUnitPrice"].Hidden = true;
                this.grdSolutranDetails.DisplayLayout.Bands[0].Columns["ItemComboPricingID"].Hidden = true;
                this.grdSolutranDetails.DisplayLayout.Bands[0].Columns["OrignalPrice"].Hidden = true;
                this.grdSolutranDetails.DisplayLayout.Bands[0].Columns["IsComboItem"].Hidden = true;
                this.grdSolutranDetails.DisplayLayout.Bands[0].Columns["LoyaltyPoints"].Hidden = true;
                this.grdSolutranDetails.DisplayLayout.Bands[0].Columns["ItemDescriptionInOL"].Hidden = true;
                this.grdSolutranDetails.DisplayLayout.Bands[0].Columns["IsPriceChangedByOverride"].Hidden = true;
                this.grdSolutranDetails.DisplayLayout.Bands[0].Columns["CouponID"].Hidden = true;
                this.grdSolutranDetails.DisplayLayout.Bands[0].Columns["IsNonRefundable"].Hidden = true;
                this.grdSolutranDetails.DisplayLayout.Bands[0].Columns["InvoiceDiscount"].Hidden = true;
                this.grdSolutranDetails.DisplayLayout.Bands[0].Columns["S3TransID"].Hidden = true;
                #endregion

                //ItemID, ItemDescription, Qty, S3PurAmount, S3TaxAmount, S3DiscountAmount
                this.grdSolutranDetails.DisplayLayout.Bands[0].Columns["ItemID"].Header.VisiblePosition = 0;
                this.grdSolutranDetails.DisplayLayout.Bands[0].Columns["ItemID"].Header.Caption = "ItemID";
                this.grdSolutranDetails.DisplayLayout.Bands[0].Columns["ItemDescription"].Header.VisiblePosition = 1;
                this.grdSolutranDetails.DisplayLayout.Bands[0].Columns["ItemDescription"].Header.Caption = "Item Description";
                this.grdSolutranDetails.DisplayLayout.Bands[0].Columns["Qty"].Header.VisiblePosition = 2;
                this.grdSolutranDetails.DisplayLayout.Bands[0].Columns["Qty"].Header.Caption = "Qty";
                this.grdSolutranDetails.DisplayLayout.Bands[0].Columns["S3PurAmount"].Header.VisiblePosition = 3;
                this.grdSolutranDetails.DisplayLayout.Bands[0].Columns["S3PurAmount"].Header.Caption = "Amount";                
                this.grdSolutranDetails.DisplayLayout.Bands[0].Columns["S3TaxAmount"].Header.VisiblePosition = 4;
                this.grdSolutranDetails.DisplayLayout.Bands[0].Columns["S3TaxAmount"].Header.Caption = "Tax";
                this.grdSolutranDetails.DisplayLayout.Bands[0].Columns["S3DiscountAmount"].Header.VisiblePosition = 5;
                this.grdSolutranDetails.DisplayLayout.Bands[0].Columns["S3DiscountAmount"].Header.Caption = "Amount Covered";

                ResizeColumns();

                this.grdSolutranDetails.DisplayLayout.Bands[0].Columns["ItemID"].Width = 130;
                this.grdSolutranDetails.DisplayLayout.Bands[0].Columns["ItemDescription"].Width = 160;
                this.grdSolutranDetails.DisplayLayout.Bands[0].Columns["Qty"].Width = 60;
                this.grdSolutranDetails.DisplayLayout.Bands[0].Columns["S3PurAmount"].Width = 90;                
                this.grdSolutranDetails.DisplayLayout.Bands[0].Columns["S3TaxAmount"].Width = 60;
                this.grdSolutranDetails.DisplayLayout.Bands[0].Columns["S3DiscountAmount"].Width = 150;
            }
        }

        private void ResizeColumns()
        {
            try
            {
                foreach (UltraGridBand oBand in grdSolutranDetails.DisplayLayout.Bands)
                {
                    foreach (UltraGridColumn oCol in oBand.Columns)
                    {
                        //oCol.Width = oCol.CalculateAutoResizeWidth(PerformAutoSizeType.VisibleRows, true) + 28;
                        if (oCol.DataType.Equals(typeof(System.Int32)) || oCol.DataType.Equals(typeof(System.Decimal)) || oCol.DataType.Equals(typeof(System.DateTime)))
                        {
                            oCol.CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right;
                        }
                    }
                }
            }
            catch (Exception) { }
        }

        private void ShortCutKeyAction(Keys KeyCode)
        {
            switch (KeyCode)
            {
                case Keys.C:
                    btnCancel_Click(btnClose, new EventArgs());
                    break;
                default:
                    break;
            }
        }
        #endregion

        #region Control Event
        private void btnProcess_Click(object sender, EventArgs e)
        {
            IsCanceled = false;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {

            if (!IsReturnTransaction)
            {
                if (Resources.Message.Display("Are your sure, you want to Cancel?", "Cancel ", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    IsCanceled = true;
                    this.Close();
                }
            }
        }
        #endregion

        private void grdSolutranDetails_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            //Record Count
            SummarySettings summaryCnt = new SummarySettings();
            UltraGridColumn columnToSummarizeCnt = e.Layout.Bands[0].Columns["ItemID"];            
            try
            {
                summaryCnt = e.Layout.Bands[0].Summaries.Add("Record(s) Count = ", SummaryType.Count, columnToSummarizeCnt);
            }
            catch { }
            summaryCnt.DisplayFormat = "Record(s) Count = {0}";
            summaryCnt.Appearance.TextHAlign = Infragistics.Win.HAlign.Left;
            //summaryCnt.SummaryPosition = SummaryPosition.Left;
            //summaryCnt.SummaryDisplayArea = SummaryDisplayAreas.Default;
            e.Layout.Bands[0].Summaries[0].SummaryPositionColumn = columnToSummarizeCnt;

            //Qty
            SummarySettings summaryQty = new SummarySettings();
            UltraGridColumn columnToSummarizeQty = e.Layout.Bands[0].Columns["Qty"];
            try
            {
                summaryQty = e.Layout.Bands[0].Summaries.Add("Qty = ", SummaryType.Sum, columnToSummarizeQty);
            }
            catch { }
            summaryQty.DisplayFormat = "{0}";
            summaryQty.Appearance.TextHAlign = Infragistics.Win.HAlign.Right;
            //summaryQty.SummaryPosition = SummaryPosition.Left;
            //summaryQty.SummaryDisplayArea = SummaryDisplayAreas.Default;
            e.Layout.Bands[0].Summaries[1].SummaryPositionColumn = columnToSummarizeQty;

            //Amount
            SummarySettings summaryAmt = new SummarySettings();
            UltraGridColumn columnToSummarizeAmt = e.Layout.Bands[0].Columns["S3PurAmount"];
            try
            {
                summaryAmt = e.Layout.Bands[0].Summaries.Add("Amount = ", SummaryType.Sum, columnToSummarizeAmt);
            }
            catch { }
            summaryAmt.DisplayFormat = "{0}";
            summaryAmt.Appearance.TextHAlign = Infragistics.Win.HAlign.Right;
            //summaryAmt.SummaryPosition = SummaryPosition.Left;
            //summaryAmt.SummaryDisplayArea = SummaryDisplayAreas.Default;
            e.Layout.Bands[0].Summaries[2].SummaryPositionColumn = columnToSummarizeAmt;                       

            //Tax
            SummarySettings summaryTax = new SummarySettings();
            UltraGridColumn columnToSummarizeTax = e.Layout.Bands[0].Columns["S3TaxAmount"];
            try
            {
                summaryTax = e.Layout.Bands[0].Summaries.Add("Tax = ", SummaryType.Sum, columnToSummarizeTax);
            }
            catch { }
            summaryTax.DisplayFormat = "{0}";
            summaryTax.Appearance.TextHAlign = Infragistics.Win.HAlign.Right;
            //summaryTax.SummaryPosition = SummaryPosition.Left;
            //summaryTax.SummaryDisplayArea = SummaryDisplayAreas.Default;
            e.Layout.Bands[0].Summaries[3].SummaryPositionColumn = columnToSummarizeTax;

            //Discount
            SummarySettings summaryDisc = new SummarySettings();
            UltraGridColumn columnToSummarizeDisc = e.Layout.Bands[0].Columns["S3DiscountAmount"];
            try
            {
                summaryDisc = e.Layout.Bands[0].Summaries.Add("Discount = ", SummaryType.Sum, columnToSummarizeDisc);
            }
            catch { }
            summaryDisc.DisplayFormat = "{0}";
            summaryDisc.Appearance.TextHAlign = Infragistics.Win.HAlign.Right;
            //summaryDisc.SummaryPosition = SummaryPosition.Left;
            //summaryDisc.SummaryDisplayArea = SummaryDisplayAreas.Default;
            e.Layout.Bands[0].Summaries[4].SummaryPositionColumn = columnToSummarizeDisc;

            //e.Layout.Override.AllowRowSummaries = AllowRowSummaries.True;
            e.Layout.Bands[0].Override.SummaryFooterCaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            //e.Layout.Override.SummaryDisplayArea = SummaryDisplayAreas.BottomFixed;

            e.Layout.Override.SummaryFooterAppearance.BackColor = Color.Silver;
            e.Layout.Override.SummaryValueAppearance.BackColor = Color.Silver;
            e.Layout.Override.SummaryValueAppearance.ForeColor = Color.Maroon;
            e.Layout.Override.SummaryValueAppearance.FontData.Bold = Infragistics.Win.DefaultableBoolean.True;
            e.Layout.Override.SummaryFooterSpacingAfter = 5;
            e.Layout.Override.SummaryFooterSpacingBefore = 5;
        }
    }
}
