using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NLog;
using POS_Core.CommonData;
using POS_Core.BusinessRules;
using POS_Core.Resources;

namespace POS_Core_UI
{
    public partial class frmTaxBreakDown : Form
    {
        #region local variables
        private static ILogger logger = LogManager.GetCurrentClassLogger();
        TransDetailTaxData oTDTaxData = new TransDetailTaxData();
        TransDetailData oTransDetailData = new TransDetailData();   //PRIMEPOS-3034 02-Dec-2021 JY Added        
        private FormDataOnScreen DataOnScreen = FormDataOnScreen.TaxData;   //PRIMEPOS-2651 08-Apr-2022 JY Added
        DataTable dtRefrigeratedDrugs = new DataTable();    //PRIMEPOS-2651 08-Apr-2022 JY Added
        DataTable dtUnPickedRxs = new DataTable();  //PRIMEPOS-3093 24-May-2022 JY Added
        #endregion

        public frmTaxBreakDown()
        {
            InitializeComponent();
        }

        //public frmTaxBreakDown(TransDetailTaxData oTransDetailTaxData)
        //{
        //    oTDTaxData = oTransDetailTaxData;
        //    InitializeComponent();

        //}

        public frmTaxBreakDown(object oData, FormDataOnScreen dType)
        {
            DataOnScreen = dType;   //PRIMEPOS-2651 08-Apr-2022 JY Added

            if (oData.GetType() == typeof(TransDetailTaxData))
                oTDTaxData = (TransDetailTaxData)oData;
            else if (oData.GetType() == typeof(TransDetailData))
                oTransDetailData = (TransDetailData)oData;
            else if (oData.GetType() == typeof(DataTable))
            {
                if (DataOnScreen == FormDataOnScreen.RefrigeratedDrugsData)
                    dtRefrigeratedDrugs = (DataTable)oData;
                else if (DataOnScreen == FormDataOnScreen.UnPickedRxs)
                {
                    dtUnPickedRxs = (DataTable)oData;
                }
            }
            InitializeComponent();
        }

        private void frmTaxBreakDown_Load(object sender, EventArgs e)
        {
            clsUIHelper.setColorSchecme(this);
            if (DataOnScreen == FormDataOnScreen.TaxData)    //PRIMEPOS-2651 08-Apr-2022 JY Modified
            {
                GetTaxBreakDown();
                this.grdTaxBreakDown.Focus();
            }
            else if (DataOnScreen == FormDataOnScreen.LineItemDetailData)    //PRIMEPOS-2651 08-Apr-2022 JY Modified
            {
                this.lblTransactionType.Text = this.Text = "Line Item Details";
                this.Height = 350;
                this.Width = 900;
                ShowPOSTransactionDetail();
                this.grdTaxBreakDown.Focus();
            }
            else if (DataOnScreen == FormDataOnScreen.RefrigeratedDrugsData)    //PRIMEPOS-2651 08-Apr-2022 JY Added
            {
                this.Text = "Refrigerated Drugs";
                this.lblTransactionType.Text = "These items are refrigerated";
                this.Height = 450;
                ShowRefrigeratedDrugsData();
                btnClose.Focus();
            }
            else if (DataOnScreen == FormDataOnScreen.UnPickedRxs)  //PRIMEPOS-3093 24-May-2022 JY Added
            {
                this.Text = "Remaining Unpicked Rxs";
                this.lblTransactionType.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                this.lblTransactionType.Text = "Click \"Ok\" to include them or click \"Close\" to proceed.";
                this.Height = 350;
                this.Width = 700;
                ShowUnPickedRxsData();
                btnOk.Visible = true;
                btnOk.Focus();
            }
        }

        private void frmTaxBreakDown_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Escape)
            {
                btnClose_Click(btnClose, new System.EventArgs());
                e.Handled = true;
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void GetTaxBreakDown()
        {
            try
            {
                POSTransaction oPOSTransaction = new POSTransaction();

                var varTaxBreakDown = from TaxDetails in oTDTaxData.TransDetailTax.AsEnumerable()
                                      group TaxDetails by new
                                      {
                                          TaxID = TaxDetails.Field<int>("TaxID"),
                                      } into userg
                                      select new
                                      {
                                          TaxID = userg.Key.TaxID,
                                          TaxAmount = userg.Sum(x => x.Field<decimal>("TaxAmount"))
                                      };

                DataTable dt = new DataTable();
                dt.Columns.Add("TaxID", typeof(System.Int32));
                dt.Columns.Add("Description", typeof(System.String));
                dt.Columns.Add("TaxAmount", typeof(System.Decimal));
                DataRow dr;

                foreach (var Summary in varTaxBreakDown)
                {
                    dr = dt.NewRow();
                    dr["TaxID"] = Summary.TaxID;
                    dr["Description"] = oPOSTransaction.GetTaxDescription(Summary.TaxID);
                    dr["TaxAmount"] = Summary.TaxAmount;
                    dt.Rows.Add(dr);
                }
                grdTaxBreakDown.DataSource = dt;

                grdTaxBreakDown.DisplayLayout.Bands[0].Columns[0].CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit;
                grdTaxBreakDown.DisplayLayout.Bands[0].Columns[1].CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit;
                grdTaxBreakDown.DisplayLayout.Bands[0].Columns[2].CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit;

                grdTaxBreakDown.DisplayLayout.Bands[0].Columns[2].CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right;
                grdTaxBreakDown.DisplayLayout.Bands[0].Columns[2].MinValue = -99999999999.99;
                grdTaxBreakDown.DisplayLayout.Bands[0].Columns[2].MaxValue = 99999999999.99;
                grdTaxBreakDown.DisplayLayout.Bands[0].Columns[2].Format = "##########0.00";
                clsUIHelper.SetReadonlyRow(this.grdTaxBreakDown);
                this.grdTaxBreakDown.DisplayLayout.TabNavigation = Infragistics.Win.UltraWinGrid.TabNavigation.NextControl;
                this.grdTaxBreakDown.PerformAction(Infragistics.Win.UltraWinGrid.UltraGridAction.BelowRow);
            }
            catch (Exception Ex)
            {
                logger.Fatal(Ex, "GetTaxBreakDown()");
            }
        }

        private void ShowPOSTransactionDetail()
        {
            try
            {
                grdTaxBreakDown.DataSource = oTransDetailData;

                for (int i = 0; i < this.grdTaxBreakDown.DisplayLayout.Bands[0].Columns.Count; i++)
                {
                    this.grdTaxBreakDown.DisplayLayout.Bands[0].Columns[i].Hidden = true;
                }

                this.grdTaxBreakDown.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.TransDetail_Fld_ItemID].Hidden = false;
                this.grdTaxBreakDown.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.TransDetail_Fld_ItemDescription].Hidden = false;
                this.grdTaxBreakDown.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.TransDetail_Fld_QTY].Hidden = false;
                this.grdTaxBreakDown.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.TransDetail_Fld_Price].Hidden = false;
                this.grdTaxBreakDown.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.TransDetail_Fld_Discount].Hidden = false;
                this.grdTaxBreakDown.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.TransDetail_Fld_TaxAmount].Hidden = false;
                this.grdTaxBreakDown.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.TransDetail_Fld_Category].Hidden = false;
                this.grdTaxBreakDown.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.TransDetail_Fld_OrignalPrice].Hidden = false;

                this.grdTaxBreakDown.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.TransDetail_Fld_Discount].Format = "##0.00";
                this.grdTaxBreakDown.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.TransDetail_Fld_Price].Format = "########0.00";
                this.grdTaxBreakDown.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.TransDetail_Fld_TaxAmount].Format = "########0.00";
                this.grdTaxBreakDown.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.TransDetail_Fld_OrignalPrice].Format = "########0.00";

                this.grdTaxBreakDown.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.TransDetail_Fld_QTY].CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right;
                this.grdTaxBreakDown.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.TransDetail_Fld_Price].CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right;
                this.grdTaxBreakDown.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.TransDetail_Fld_Discount].CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right;
                this.grdTaxBreakDown.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.TransDetail_Fld_TaxAmount].CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right;
                this.grdTaxBreakDown.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.TransDetail_Fld_OrignalPrice].CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right;

                #region Net Price and Total Price
                for (int i = 0; i < this.grdTaxBreakDown.Rows.Count; i++)
                {
                    this.grdTaxBreakDown.Rows[i].Cells[clsPOSDBConstants.TransDetail_Fld_InvoiceDiscount].Value = (Configuration.convertNullToInt(this.grdTaxBreakDown.Rows[i].Cells[clsPOSDBConstants.TransDetail_Fld_QTY].Value) * Configuration.convertNullToDecimal(this.grdTaxBreakDown.Rows[i].Cells[clsPOSDBConstants.TransDetail_Fld_Price].Value)) - Configuration.convertNullToDecimal(this.grdTaxBreakDown.Rows[i].Cells[clsPOSDBConstants.TransDetail_Fld_Discount].Value);
                    this.grdTaxBreakDown.Rows[i].Cells[clsPOSDBConstants.TransDetail_Fld_OldDiscountAmt].Value = Configuration.convertNullToDecimal(this.grdTaxBreakDown.Rows[i].Cells[clsPOSDBConstants.TransDetail_Fld_InvoiceDiscount].Value) + Configuration.convertNullToDecimal(this.grdTaxBreakDown.Rows[i].Cells[clsPOSDBConstants.TransDetail_Fld_TaxAmount].Value);
                }

                this.grdTaxBreakDown.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.TransDetail_Fld_InvoiceDiscount].Hidden = false;
                this.grdTaxBreakDown.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.TransDetail_Fld_InvoiceDiscount].Header.Caption = "Net Price";
                this.grdTaxBreakDown.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.TransDetail_Fld_InvoiceDiscount].Format = "########0.00";
                this.grdTaxBreakDown.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.TransDetail_Fld_InvoiceDiscount].CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right;

                this.grdTaxBreakDown.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.TransDetail_Fld_OldDiscountAmt].Hidden = false;
                this.grdTaxBreakDown.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.TransDetail_Fld_OldDiscountAmt].Header.Caption = "Total Price";
                this.grdTaxBreakDown.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.TransDetail_Fld_OldDiscountAmt].Format = "########0.00";
                this.grdTaxBreakDown.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.TransDetail_Fld_OldDiscountAmt].CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right;
                #endregion

                this.grdTaxBreakDown.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.TransDetail_Fld_ItemID].Header.VisiblePosition = 0;
                this.grdTaxBreakDown.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.TransDetail_Fld_ItemDescription].Header.VisiblePosition = 1;
                this.grdTaxBreakDown.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.TransDetail_Fld_QTY].Header.VisiblePosition = 2;
                this.grdTaxBreakDown.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.TransDetail_Fld_Price].Header.VisiblePosition = 3;
                this.grdTaxBreakDown.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.TransDetail_Fld_OrignalPrice].Header.VisiblePosition = 4;
                this.grdTaxBreakDown.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.TransDetail_Fld_Discount].Header.VisiblePosition = 5;
                this.grdTaxBreakDown.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.TransDetail_Fld_InvoiceDiscount].Header.VisiblePosition = 6;  //Net Price
                this.grdTaxBreakDown.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.TransDetail_Fld_TaxAmount].Header.VisiblePosition = 7;
                this.grdTaxBreakDown.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.TransDetail_Fld_OldDiscountAmt].Header.VisiblePosition = 8;   //Total Price
                this.grdTaxBreakDown.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.TransDetail_Fld_Category].Header.VisiblePosition = 9;

                this.grdTaxBreakDown.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.TransDetail_Fld_ItemDescription].Width = 120;
                this.grdTaxBreakDown.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.TransDetail_Fld_QTY].Width = 40;
                this.grdTaxBreakDown.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.TransDetail_Fld_Price].Width = 55;
                this.grdTaxBreakDown.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.TransDetail_Fld_Discount].Width = 65;
                this.grdTaxBreakDown.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.TransDetail_Fld_Category].Width = 80;
                this.grdTaxBreakDown.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.TransDetail_Fld_TaxAmount].Width = 80;

                clsUIHelper.SetReadonlyRow(this.grdTaxBreakDown);
                if (this.grdTaxBreakDown.Rows.Count > 0)
                {
                    this.grdTaxBreakDown.ActiveRow = this.grdTaxBreakDown.Rows[0];
                    this.grdTaxBreakDown.ActiveRow.Selected = true;
                }
                this.grdTaxBreakDown.DisplayLayout.TabNavigation = Infragistics.Win.UltraWinGrid.TabNavigation.NextControl;
            }
            catch (Exception Ex)
            {
                logger.Fatal(Ex, "ShowPOSTransactionDetail()");
            }
        }

        #region PRIMEPOS-2651 08-Apr-2022 JY Added
        private void ShowRefrigeratedDrugsData()
        {
            try
            {
                grdTaxBreakDown.DataSource = dtRefrigeratedDrugs;
                this.grdTaxBreakDown.DisplayLayout.Bands[0].Columns[0].Width = 500;
                clsUIHelper.SetReadonlyRow(this.grdTaxBreakDown);
                this.grdTaxBreakDown.DisplayLayout.TabNavigation = Infragistics.Win.UltraWinGrid.TabNavigation.NextControl;
            }
            catch (Exception Ex)
            {
                logger.Fatal(Ex, "ShowRefrigeratedDrugsData()");
            }
        }
        #endregion

        #region PRIMEPOS-3093 24-May-2022 JY Added
        private void ShowUnPickedRxsData()
        {
            try
            {
                grdTaxBreakDown.DataSource = dtUnPickedRxs;
                this.grdTaxBreakDown.DisplayLayout.Bands[0].Columns[0].Width = 200;
                this.grdTaxBreakDown.DisplayLayout.Bands[0].Columns[1].Width = 80;
                this.grdTaxBreakDown.DisplayLayout.Bands[0].Columns[2].Width = 60;
                this.grdTaxBreakDown.DisplayLayout.Bands[0].Columns[3].Width = 80;
                this.grdTaxBreakDown.DisplayLayout.Bands[0].Columns[4].Width = 150;
                this.grdTaxBreakDown.DisplayLayout.Bands[0].Columns[5].Width = 80;

                clsUIHelper.SetReadonlyRow(this.grdTaxBreakDown);
                this.grdTaxBreakDown.DisplayLayout.TabNavigation = Infragistics.Win.UltraWinGrid.TabNavigation.NextControl;
            }
            catch (Exception Ex)
            {
                logger.Fatal(Ex, "ShowUnPickedRxsData()");
            }
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        #endregion

        #region PRIMEPOS-3093 24-May-2022 JY Added
        private void grdTaxBreakDown_InitializeRow(object sender, Infragistics.Win.UltraWinGrid.InitializeRowEventArgs e)
        {
            try
            {
                if (DataOnScreen == FormDataOnScreen.UnPickedRxs)
                {
                    if (Configuration.convertNullToString(e.Row.Cells["Status"].Value).Trim().ToUpper() == "U")
                    {
                        e.Row.Appearance.ForeColor = Color.Red;
                        e.Row.Appearance.ForeColorDisabled = Color.Red;
                    }
                }
            }
            catch (Exception Ex)
            {
            }
        }
        #endregion
    }

    public enum FormDataOnScreen
    {
        TaxData,
        LineItemDetailData,
        RefrigeratedDrugsData,
        UnPickedRxs //PRIMEPOS-3093 24-May-2022 JY Added
    }
}
