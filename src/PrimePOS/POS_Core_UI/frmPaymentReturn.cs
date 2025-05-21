using Infragistics.Win.UltraWinGrid;
using System;
using System.Data;
using System.Windows.Forms;

namespace POS_Core_UI.UI
{
    public partial class frmPaymentReturn : Form
    {
        #region Variable declaration
        public bool IsCanceled = false;
        public string Cards = string.Empty;
        public string ProcessTransID;
        public string nbsUid; //PRIMEPOS-3375
        public string nbsPayType; //PRIMEPOS-3375
        public string Amount;
        public string CardNumber;
        public DataTable dtCardDetails = null;
        #region PRIMEPOS-2738 ADDED BY ARVIND 
        public string ReversedAmount;//PRIMEPOS-2738
        public string TransPayID;//PRIMEPOS-2738
        public string TransDate;//2943
        public string TransRefNumber;
        public bool isSolutran = false;
        #endregion
        public string TransTypeCode = string.Empty;//PRIMEPOS-3087
        #endregion
        public frmPaymentReturn()
        {
            InitializeComponent();
        }

        #region GridEvents

        private void grdPaymentSelect_DoubleClickCell(object sender, Infragistics.Win.UltraWinGrid.DoubleClickCellEventArgs e)
        {
            ProcessTransID = e.Cell.Row.Cells["ProcessorTransID"].Value.ToString();
            CardNumber = e.Cell.Row.Cells["RefNo"].Value.ToString();
            Amount = e.Cell.Row.Cells["Amount"].Value.ToString();
            TransDate = e.Cell.Row.Cells["TransDate"].Value.ToString();//2943
            //PRIMEPOS-2738 ADDED BY ARVIND
            if (!isSolutran)
            {
                ReversedAmount = e.Cell.Row.Cells["ReversedAmount"].Value.ToString();//PRIMEPOS-2738
                TransPayID = e.Cell.Row.Cells["TransPayID"].Value.ToString();//PRIMEPOS-2738
                TransRefNumber = e.Cell.Row.Cells["TransRefNum_Trn"].Value.ToString();//PRIMEPOS-2738
                nbsUid = Convert.ToString(e.Cell.Row.Cells["NBSTransUid"].Value); //PRIMEPOS-3375 //PRIMEPOS-3466
                nbsPayType = Convert.ToString(e.Cell.Row.Cells["NBSPaymentTpe"].Value); //PRIMEPOS-3375 //PRIMEPOS-3466
            }
            //

            this.Close();
        }

        private void grdPaymentSelect_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    if (this.grdPaymentSelect.ContainsFocus == true && this.grdPaymentSelect.ActiveRow != null && this.grdPaymentSelect.ActiveCell != null)
                    {
                        if (grdPaymentSelect.ActiveCell.Activation == Activation.Disabled)
                        {
                            e.Handled = true;
                            return;
                        }
                    }
                    ProcessTransID = this.grdPaymentSelect.ActiveRow.Cells["ProcessorTransID"].Text;
                    CardNumber = this.grdPaymentSelect.ActiveRow.Cells["RefNo"].Text;
                    Amount = this.grdPaymentSelect.ActiveRow.Cells["Amount"].Text;
                    TransDate = this.grdPaymentSelect.ActiveRow.Cells["TransDate"].Text;//2943
                    //PRIMEPOS-2738 ADDED BY ARVIND
                    if (!isSolutran)
                    {
                        TransTypeCode = this.grdPaymentSelect.ActiveRow.Cells["TransTypeCode"].Text;//PRIMEPOS-3087 //PRIMEPOS-3179 27-Jan-2023 JY just moved inside for solutran
                        ReversedAmount = this.grdPaymentSelect.ActiveRow.Cells["ReversedAmount"].Text;//PRIMEPOS-2738
                        TransPayID = this.grdPaymentSelect.ActiveRow.Cells["TransPayID"].Text;//PRIMEPOS-2738
                        TransRefNumber = this.grdPaymentSelect.ActiveRow.Cells["TransRefNum_Trn"].Text;//PRIMEPOS-2738
                        nbsUid = Convert.ToString(this.grdPaymentSelect.ActiveRow.Cells["NBSTransUid"]?.Text); //PRIMEPOS-3375 //PRIMEPOS-3466
                        nbsPayType = Convert.ToString(this.grdPaymentSelect.ActiveRow.Cells["NBSPaymentType"]?.Text); //PRIMEPOS-3375 //PRIMEPOS-3466
                    }

                    this.Close();
                }
                else if (e.KeyData == Keys.Escape)
                {
                    IsCanceled = true;//PRIMEPOS-2738 ADDED BY ARVIND
                    this.Close();
                }

            }

            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }
        #endregion

        #region Grid Methods
        public void grdPaymentTrans(DataSet ds)
        {
            grdPaymentSelect.DataSource = ds;
            resizeColumns();
            grdPaymentSelect.DisplayLayout.Bands[0].Columns["TransDate"].Format = "MM/dd/yyyy hh:mm";//2943
        }


        private void resizeColumns()
        {
            foreach (UltraGridColumn oCol in grdPaymentSelect.DisplayLayout.Bands[0].Columns)
            {
                oCol.Width = 179;
                oCol.CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right;
            }
        }
        #endregion

        #region Control Events
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();

            IsCanceled = true;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            KeyEventArgs d = new KeyEventArgs(Keys.Enter);
            grdPaymentSelect_KeyDown(this, d);

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        #endregion


    }
}
