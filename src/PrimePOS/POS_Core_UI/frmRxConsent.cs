using Infragistics.Win.UltraWinGrid;
using NLog;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace POS_Core_UI
{
    public partial class frmRxConsent : System.Windows.Forms.Form
    {
        private ILogger logger = LogManager.GetCurrentClassLogger();
        public string selectedRxs;
        public DataTable selectedRxTable = new DataTable();

        public frmRxConsent()
        {
            InitializeComponent();
        }
        public frmRxConsent(string strTitle, string consentTitle, DataTable dtPendingRxSignature)
        {
            InitializeComponent();
            this.WindowState = FormWindowState.Normal;
            this.grdViewMain.Text = strTitle;
            this.grdViewMain.DataSource = dtPendingRxSignature;

            if (this.grdViewMain.DisplayLayout.Bands[0].Columns.Exists("CHECK") == false)
            {
                this.grdViewMain.DisplayLayout.Bands[0].Columns.Add("CHECK");
                this.grdViewMain.DisplayLayout.Bands[0].Columns["CHECK"].DataType = typeof(bool);
                this.grdViewMain.DisplayLayout.Bands[0].Columns["CHECK"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.CheckBox;
            }

            this.grdViewMain.DisplayLayout.Bands[0].Columns[0].CellActivation = Activation.AllowEdit;
            CheckAllDefault();
            GridColumnDisplay();
            this.grdViewMain.DisplayLayout.AutoFitStyle = AutoFitStyle.ResizeAllColumns;
            this.Focus();
            this.Activate();
        }

        private void GridColumnDisplay()
        {
            try
            {
                if (grdViewMain != null && grdViewMain.Rows.Count > 0)
                {
                    this.grdViewMain.DisplayLayout.Bands[0].Columns["CHECK"].Header.VisiblePosition = 0;
                    this.grdViewMain.DisplayLayout.Bands[0].Columns["CHECK"].Header.Caption = "Select Rxs";

                    this.grdViewMain.DisplayLayout.Bands[0].Columns["PatientName"].Header.VisiblePosition = 1;
                    this.grdViewMain.DisplayLayout.Bands[0].Columns["PatientName"].Header.Caption = "Patient Name";

                    this.grdViewMain.DisplayLayout.Bands[0].Columns["RxNo"].Header.VisiblePosition = 2;
                    this.grdViewMain.DisplayLayout.Bands[0].Columns["RxNo"].Header.Caption = "Rx#";

                    this.grdViewMain.DisplayLayout.Bands[0].Columns["DrugNDC"].Header.VisiblePosition = 3;
                    this.grdViewMain.DisplayLayout.Bands[0].Columns["DrugNDC"].Header.Caption = "Drug NDC";

                    this.grdViewMain.DisplayLayout.Bands[0].Columns["DrgName"].Header.VisiblePosition = 4;
                    this.grdViewMain.DisplayLayout.Bands[0].Columns["DrgName"].Header.Caption = "Drug Name";

                    this.grdViewMain.DisplayLayout.Bands[0].Columns["ConsentCaptureDate"].Header.VisiblePosition = 5;
                    this.grdViewMain.DisplayLayout.Bands[0].Columns["ConsentCaptureDate"].Header.Caption = "Consent Capture Date";
                    this.grdViewMain.DisplayLayout.Bands[0].Columns["ConsentCaptureDate"].Format = "MM/dd/yyyy hh:mm tt";

                    this.grdViewMain.DisplayLayout.Bands[0].Columns["ConsentEndDate"].Header.VisiblePosition = 6;
                    this.grdViewMain.DisplayLayout.Bands[0].Columns["ConsentEndDate"].Header.Caption = "Consent End Date";
                    this.grdViewMain.DisplayLayout.Bands[0].Columns["ConsentEndDate"].Format = "MM/dd/yyyy hh:mm tt";

                    this.grdViewMain.DisplayLayout.Bands[0].Columns["ConsentStatus"].Header.VisiblePosition = 7;
                    this.grdViewMain.DisplayLayout.Bands[0].Columns["ConsentStatus"].Header.Caption = "Consent Status";

                    this.grdViewMain.DisplayLayout.Bands[0].Columns["SignPending"].Header.VisiblePosition = 8;
                    this.grdViewMain.DisplayLayout.Bands[0].Columns["SignPending"].Header.Caption = "Sign.Pending";

                    this.grdViewMain.DisplayLayout.Bands[0].Columns["PatientConsentID"].Hidden = true;
                    this.grdViewMain.DisplayLayout.Bands[0].Columns["IsNewRx"].Hidden = true;
                    this.grdViewMain.DisplayLayout.Bands[0].Columns["PatientNo"].Hidden = true;
                    this.grdViewMain.DisplayLayout.Bands[0].Columns["SPECIFICPRODUCTID"].Hidden = true;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        #region FORM EVENT

        private void frmRxConsent_Load(object sender, EventArgs e)
        {
        }

        private void frmRxConsent_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Space)
                {
                    if (grdViewMain.ActiveRow != null)
                    {
                        if (Convert.ToBoolean(grdViewMain.ActiveRow.Cells["IsNewRx"].Value) == false)
                        {
                            grdViewMain.ActiveRow.Cells["CHECK"].Value = true;
                        }
                        else
                        {
                            CheckUncheckGridRow(this.grdViewMain.ActiveRow.Cells["CHECK"]);
                        }
                    }
                }

                switch (e.KeyData)
                {
                    case Keys.F2:
                        btnContinue_Click(null, null);
                        break;
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex, "frmRxConsent==>frmRxConsent_KeyUp(): An Error Occured");
            }
        }

        #endregion

        #region Control Event

        private void btnContinue_Click(object sender, EventArgs e)
        {
            selectedRxTable = CreateSelectedRxTbl();
            string selectedRxTemp = string.Empty;
            string consentedRx = "Consented for :- ";
            string notConsentedRx = "Not Consented for :- ";
            bool isAnyRxChecked = false;
            bool isAnyRxUnChecked = false;
            try
            {
                foreach (UltraGridRow oRow in grdViewMain.Rows)
                {
                    if (oRow.Cells["CHECK"].Value.ToString() == "True")
                    {
                        isAnyRxChecked = true;
                        consentedRx = consentedRx + Convert.ToString(oRow.Cells["DrgName"].Value).Trim() + " , ";
                    }
                    else if (oRow.Cells["CHECK"].Value.ToString() == "False")
                    {
                        notConsentedRx = notConsentedRx + Convert.ToString(oRow.Cells["DrgName"].Value).Trim() + " , ";
                        isAnyRxUnChecked = true;
                    }
                    DataRow row = selectedRxTable.NewRow();
                    row["RXNO"] = oRow.Cells["RxNo"].Value;
                    row["PatientNo"] = oRow.Cells["PatientNo"].Value;
                    row["DrugNDC"] = oRow.Cells["DrugNDC"].Value;
                    row["PatientConsentID"] = oRow.Cells["PatientConsentID"].Value;
                    row["IsNewRx"] = Convert.ToBoolean(oRow.Cells["IsNewRx"].Value);
                    row["IsChecked"] = oRow.Cells["CHECK"].Value;
                    row["SpecificProductId"] = oRow.Cells["SPECIFICPRODUCTID"].Value;
                    selectedRxTable.Rows.Add(row);
                }
                if (isAnyRxChecked && isAnyRxUnChecked)
                {
                    if (!string.IsNullOrWhiteSpace(consentedRx))
                    {
                        int sConsentIndex = consentedRx.LastIndexOf(",");
                        consentedRx = consentedRx.Substring(0, sConsentIndex);
                    }
                    if (!string.IsNullOrWhiteSpace(notConsentedRx))
                    {
                        int sNotConsentIndex = notConsentedRx.LastIndexOf(",");
                        notConsentedRx = notConsentedRx.Substring(0, sNotConsentIndex);
                    }
                    selectedRxs = consentedRx + notConsentedRx;
                }
                else if (isAnyRxChecked && !isAnyRxUnChecked)
                {
                    if (!string.IsNullOrWhiteSpace(consentedRx))
                    {
                        int sConsentIndex = consentedRx.LastIndexOf(",");
                        consentedRx = consentedRx.Substring(0, sConsentIndex);
                    }
                    selectedRxs = consentedRx;
                }
                else if (!isAnyRxChecked && isAnyRxUnChecked)
                {
                    if (!string.IsNullOrWhiteSpace(notConsentedRx))
                    {
                        int sNotConsentIndex = notConsentedRx.LastIndexOf(",");
                        notConsentedRx = notConsentedRx.Substring(0, sNotConsentIndex);
                    }
                    selectedRxs = notConsentedRx;
                }
                else
                {
                    selectedRxs = string.Empty;
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex, "frmRxConsent==>btnContinue_Click(): An Error Occured");
            }
            this.Close();
        }

        private void grdViewMain_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            try
            {
                UltraGridColumn columnToSummarize = e.Layout.Bands[0].Columns[0];
                SummarySettings summary = new SummarySettings();
                summary = e.Layout.Bands[0].Summaries.Add("Record(s) Count = ", SummaryType.Count, columnToSummarize);

                summary.DisplayFormat = "Record(s) Count = {0}";
                summary.Appearance.TextHAlign = Infragistics.Win.HAlign.Left;
                summary.SummaryPosition = SummaryPosition.Left;
                summary.SummaryDisplayArea = SummaryDisplayAreas.BottomFixed;
                e.Layout.Bands[0].Summaries[0].SummaryPositionColumn = columnToSummarize;
                e.Layout.Bands[0].Override.SummaryFooterCaptionVisible = Infragistics.Win.DefaultableBoolean.False;
                e.Layout.Override.SummaryDisplayArea = SummaryDisplayAreas.BottomFixed;
                e.Layout.Override.SummaryFooterAppearance.BackColor = Color.Silver;
                e.Layout.Override.SummaryValueAppearance.BackColor = Color.Silver;
                e.Layout.Override.SummaryValueAppearance.ForeColor = Color.Maroon;
                e.Layout.Override.SummaryValueAppearance.FontData.Bold = Infragistics.Win.DefaultableBoolean.True;
                e.Layout.Override.SummaryFooterSpacingAfter = 5;
                e.Layout.Override.SummaryFooterSpacingBefore = 5;
            }
            catch (Exception ex)
            {
                logger.Error(ex, "frmRxConsent==>grdViewMain_InitializeLayout(): An Error Occured");
            }
        }

        private void CheckAllDefault()
        {
            try
            {
                grdViewMain.BeginUpdate();
                foreach (UltraGridRow oRow in grdViewMain.Rows)
                {
                    oRow.Cells["CHECK"].Value = true;
                    oRow.Update();
                }
                grdViewMain.EndUpdate();
            }
            catch (Exception ex)
            {
                logger.Error(ex, "frmRxConsent==>CheckAllDefault(): An Error Occured");
            }
        }

        private void chkSelectAll_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                grdViewMain.BeginUpdate();
                foreach (UltraGridRow oRow in grdViewMain.Rows)
                {
                    oRow.Cells["CHECK"].Value = chkSelectAll.Checked;
                    oRow.Update();
                }
                grdViewMain.EndUpdate();
            }
            catch (Exception ex)
            {
                logger.Error(ex, "frmRxConsent==>chkSelectAll_CheckedChanged(): An Error Occured");
            }
        }
        private void CheckUncheckGridRow(UltraGridCell oCell)
        {
            try
            {
                oCell.Value = !(bool)oCell.Value;
                oCell.Row.Update();
            }
            catch (Exception ex)
            {
                logger.Error(ex, "frmRxConsent==>CheckUncheckGridRow(): An Error Occured");

            }
        }

        private void grdViewMain_MouseClick(object sender, MouseEventArgs e)
        {
            try
            {
                Point point = System.Windows.Forms.Cursor.Position;
                point = this.grdViewMain.PointToClient(point);
                Infragistics.Win.UIElement oUI = this.grdViewMain.DisplayLayout.UIElement.ElementFromPoint(point, Infragistics.Win.UIElementInputType.MouseClick);
                if (oUI == null)
                    return;

                while (oUI != null)
                {
                    if (oUI.GetType() == typeof(CellUIElement))
                    {
                        CellUIElement cellUIElement = (CellUIElement)oUI;
                        if (cellUIElement.Column.Key.ToUpper() == "CHECK")
                        {
                            if (Convert.ToBoolean(grdViewMain.ActiveRow.Cells["IsNewRx"].Value) == false)
                            {
                                grdViewMain.ActiveRow.Cells["CHECK"].Value = true;
                            }
                            else
                            {
                                CheckUncheckGridRow(cellUIElement.Cell);
                            }
                        }
                        break;
                    }
                    oUI = oUI.Parent;
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex, "frmRxConsent==>CheckUncheckGridRow(): An Error Occured");
            }
        }
        #endregion
        #region Table
        private DataTable CreateSelectedRxTbl()
        {
            try
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("RXNO", typeof(Int64));
                dt.Columns.Add("PatientNo", typeof(Int64));
                dt.Columns.Add("DrugNDC", typeof(string));
                dt.Columns.Add("PatientConsentID", typeof(Int64));
                dt.Columns.Add("IsNewRx", typeof(bool));
                dt.Columns.Add("IsChecked", typeof(bool));
                dt.Columns.Add("SpecificProductId", typeof(Int64));
                return dt;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
    }
}
