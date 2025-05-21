using Infragistics.Win.UltraWinGrid;
using System;
using System.Data;
using System.Windows.Forms;
using POS_Core.CommonData;
using System.Linq;
namespace POS_Core_UI
{
    public partial class frmAddCards : Form
    {
        //Add Shortcut key for Add Cards
        #region Variable declaration
        public bool IsCanceled = false;
        public string Cards = string.Empty;
        public string ProcessTransID;
        public string Amount;
        public string CardNumber;
        public DataTable dtCardDetails = null;
        #endregion
        public frmAddCards()
        {
            InitializeComponent();
        }

        #region Control Events
        private void btnAddCard_Click(object sender, EventArgs e)
        {
            #region PRIMEPOS-3488
            bool isInvalidCard = false; 
            string posCodeData = string.Empty;
            string cvvData = string.Empty;
            string cardNumber = txtCards.Text.Trim();

            if (string.IsNullOrEmpty(cardNumber) || cardNumber.Length < clsPOSDBConstants.s3Card_Len_Sixteen)
            {
                clsUIHelper.ShowErrorMsg("Card Number Is Invalid");
                isInvalidCard = true;
            } 
            else if (cardNumber.Length == clsPOSDBConstants.s3Card_Len_Seventeen || cardNumber.Length == clsPOSDBConstants.s3Card_Len_Sixteen)
            {
                if (cardNumber.All(char.IsDigit))
                {
                    posCodeData = clsPOSDBConstants.s3Card_Manual_Pos_Code_data;
                }
                else
                {
                    clsUIHelper.ShowErrorMsg("Card Number Is Invalid");
                    //clsUIHelper.ShowErrorMsg("Card Number must contain only numeric characters");
                    isInvalidCard = true;
                }
                posCodeData = clsPOSDBConstants.s3Card_Manual_Pos_Code_data;
            }
            else if (cardNumber.Length >= clsPOSDBConstants.s3Card_Len_SeventyEight)
            {
                string s3CardNumber = txtCards.Text.Substring(clsPOSDBConstants.s3Card_Len_FiftyThree).ToUpper();

                if (!string.IsNullOrWhiteSpace(s3CardNumber) && s3CardNumber.Contains("S3M")) //PRIMEPOS-3510
                {
                    cvvData = txtCards.Text.Substring(clsPOSDBConstants.s3Card_Len_SixtyEight, clsPOSDBConstants.s3Card_CVV_len_Three);
                    txtCards.Text = s3CardNumber.Substring(0, clsPOSDBConstants.s3Card_Len_thirteen);
                    posCodeData = clsPOSDBConstants.s3Card_s3m_Pos_Code_data;
                }
                else if (!string.IsNullOrWhiteSpace(s3CardNumber) && s3CardNumber.Contains("S3")) //PRIMEPOS-3510
                {
                    txtCards.Text = s3CardNumber.Substring(0, clsPOSDBConstants.s3Card_Len_thirteen);
                    posCodeData = clsPOSDBConstants.s3Card_s3_Pos_Code_data;
                }
                else
                {
                    clsUIHelper.ShowErrorMsg("Card Number Is Invalid");
                    isInvalidCard = true;
                }
            }
            else
            {
                clsUIHelper.ShowErrorMsg("Card Number Is Invalid");
                isInvalidCard = true;
            }
            #endregion

            bool entryFound = false;
            foreach (UltraGridRow row in grdCardDetails.Rows)
            {
                object val1 = row.Cells[0].Value;

                if (val1 != null && val1.ToString() == txtCards.Text)
                {
                    MessageBox.Show("Entry already exist");
                    entryFound = true;
                    break;
                }
            }

            if (!entryFound && !isInvalidCard) //PRIMPOS-3488
            {
                #region PRIEMPOS-3488 initialize memory table
                DataTable dtCardDetails = new DataTable();
                dtCardDetails.TableName = "Card Details";
                DataColumn colCardNumber = new DataColumn("CardNumber", typeof(string));
                DataColumn colCVV = new DataColumn("CVV", typeof(string)); //PRIMEPOS-3488
                DataColumn colPOSCodeData = new DataColumn("POSDataCode", typeof(string)); //PRIMEPOS-3488
                dtCardDetails.Columns.Add(colCardNumber);
                dtCardDetails.Columns.Add(colCVV); //PRIMEPOS-3488
                dtCardDetails.Columns.Add(colPOSCodeData); //PRIMEPOS-3488
                #endregion

                #region PRIMEPOS-3488 assign new card values
                DataRow cardRow = dtCardDetails.NewRow();
                cardRow["CardNumber"] = txtCards.Text;
                cardRow["POSDataCode"] = posCodeData; //PRIMEPOS-3488
                cardRow["CVV"] = cvvData; //PRIMEPOS-3488
                dtCardDetails.Rows.Add(cardRow);
                #endregion

                if (grdCardDetails.Rows.Count > 0)
                {
                    DataTable dtCardDetTemp = dtCardDetails.Clone();
                    for (int i = 0; i < grdCardDetails.Rows.Count; i++)
                    {
                        DataRow drCard = dtCardDetTemp.NewRow();
                        drCard["CardNumber"] = grdCardDetails.Rows[i].Cells["CardNumber"].Text;
                        drCard["CVV"] = grdCardDetails.Rows[i].Cells["CVV"].Text; //PRIMEPOS-3488
                        drCard["POSDataCode"] = grdCardDetails.Rows[i].Cells["POSDataCode"].Text; //PRIMEPOS-3488
                        dtCardDetTemp.Rows.Add(drCard);
                    }
                    dtCardDetTemp.Merge(dtCardDetails);
                    grdCardDetails.DataSource = dtCardDetTemp;
                }
                else
                {
                    grdCardDetails.DataSource = dtCardDetails;
                }

                txtCards.Text = "";
                txtCards.Focus();
            }
            else
            {
                txtCards.Text = "";
                txtCards.Focus();
            }
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            dtCardDetails = new DataTable();
            dtCardDetails.TableName = "Card Details";
            DataColumn colCardNumber = new DataColumn("CardNumber", typeof(string));
            DataColumn colPOSDataCode = new DataColumn("POSDataCode", typeof(string)); //PRIMEPOS-3488
            DataColumn colCVV = new DataColumn("CVV", typeof(string)); //PRIMEPOS-3488
            dtCardDetails.Columns.Add(colCardNumber);
            dtCardDetails.Columns.Add(colPOSDataCode); //PRIMEPOS-3488
            dtCardDetails.Columns.Add(colCVV); //PRIMEPOS-3488

            if (grdCardDetails.Rows.Count > 0) //PRIMEPOS-3275
            {
                for (int i = 0; i < grdCardDetails.Rows.Count; i++)
                {
                    DataRow dr = dtCardDetails.NewRow();
                    dr["CardNumber"] = grdCardDetails.Rows[i].Cells["CardNumber"].Text;
                    dr["POSDataCode"] = grdCardDetails.Rows[i].Cells["POSDataCode"].Text; //PRIMEPOS-3488
                    dr["CVV"] = grdCardDetails.Rows[i].Cells["CVV"].Text; //PRIMEPOS-3488
                    dtCardDetails.Rows.Add(dr);

                    Cards += grdCardDetails.Rows[i].Cells["CardNumber"].Text + "|";//"************" + grdCardDetails.Rows[i].Cells["CardNumber"].Text.Substring(grdCardDetails.Rows[i].Cells["CardNumber"].Text.Length - 4) + "|";// grdCardDetails.Rows[i].Cells["CardNumber"].Text + "|";
                }

                Cards = Cards.Remove(Cards.Length - 1);
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private void txtCards_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }
           
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();

            IsCanceled = true;
        }

        private void txtCards_KeyDown(object sender, KeyEventArgs e)
        {

            if (this.txtCards.Text.Trim() == "" && e.KeyData == Keys.Enter && grdCardDetails.Rows.Count > 0)
            {
                    btnOk_Click(this, new EventArgs());
            }
            if (e.KeyCode == Keys.Enter && this.txtCards.Text.Trim() != "")
            {
                btnAddCard_Click(this, new EventArgs());
                this.txtCards.Focus();

            }
            
        }

        //private void txtCvv_KeyDown(object sender, KeyEventArgs e)
        //{
        //    if (e.KeyCode == Keys.Enter)
        //    {
        //        btnAddCard_Click(this, new EventArgs());
        //        this.txtCards.Focus();
        //    }
        //    //this.txtCards.Focus();
        //}

        //private void txtCvv_KeyPress(object sender, KeyPressEventArgs e)
        //{
        //    if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
        //    {
        //        e.Handled = true;
        //    }
        //}

        #endregion

        #region GridEvents
        private void grdCardDetails_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
        {
            UltraGridLayout layout = e.Layout;
            UltraGridBand band = layout.Bands[0];

            if (false == band.Columns.Exists("Delete"))
            {
                UltraGridColumn deleteButtonColumn = band.Columns.Add("Delete");
                deleteButtonColumn.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.Button;
            }

            if (band.Columns.Exists("CVV")) //PRIMEPOS-3488
            {
                band.Columns["CVV"].Hidden = true;
            }

            if (band.Columns.Exists("POSDataCode")) //PRIMEPOS-3488
            {
                band.Columns["POSDataCode"].Hidden = true;
            }
        }

        private void grdCardDetails_ClickCellButton(object sender, Infragistics.Win.UltraWinGrid.CellEventArgs e)
        {
            if (e.Cell.Column.Key == "Delete")
            {
                e.Cell.Row.Delete();
            }
        }

        private void grdCardDetails_InitializeRow(object sender, Infragistics.Win.UltraWinGrid.InitializeRowEventArgs e)
        {
            if (e.ReInitialize == false)
            {
                e.Row.Cells["Delete"].Value = "Delete";
            }
        }

        #endregion

        #region Form Events

        private void frmAddSolutranCards_Load(object sender, EventArgs e)
        {
            this.ActiveControl = txtCards;

        }

        private void frmAddSolutranCards_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnAddCard_Click(this, new EventArgs());
            }
            this.txtCards.Focus();
        }

        #endregion


    }
}
