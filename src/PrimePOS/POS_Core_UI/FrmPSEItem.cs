using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using POS_Core.CommonData.Rows;
using POS_Core.CommonData;
using POS_Core.BusinessRules;
using POS_Core.ErrorLogging;

namespace POS_Core_UI
{
    public partial class FrmPSEItem : Form
    {
        #region variable declaration
        public bool IsCanceled = true;
        private PSEItemData oPSEItemData = new PSEItemData();
        private PSEItemRow oPSEItemRow;
        private PSEItem oPSEItem = new PSEItem();
        private bool isInEditMode;
        #endregion

        public FrmPSEItem()
        {
            InitializeComponent();

            //try
            //{
            //    Initialize();
            //}
            //catch (Exception exp)
            //{
            //    clsUIHelper.ShowErrorMsg(exp.Message);
            //}
        }

        public void Initialize()
        {
            SetNew();
        }

        private void SetNew()
        {
            oPSEItem = new PSEItem();
            oPSEItemData = new PSEItemData();
            this.Text = "Add PSE Item Information";
            this.lblTransactionType.Text = "Add PSE Item Information";
            Clear();
            oPSEItemRow = oPSEItemData.PSEItem.AddRow(0, "", "","", 0, 0,true,"M");
        }

        private void Clear()
        {
            txtItemCode.Text = "";
            txtDescription.Text = "";
            txtItemNDC.Text = "";
            numGrams.Value = 0;
            numPillCount.Value = 0;
            chkIsActive.Checked = true;
        }

        private void FrmPSEItem_Load(object sender, EventArgs e)
        {
            this.txtItemCode.AfterEnterEditMode += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
            this.txtItemCode.AfterExitEditMode += new System.EventHandler(clsUIHelper.AfterExitEditMode);
            this.txtDescription.AfterEnterEditMode += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
            this.txtDescription.AfterExitEditMode += new System.EventHandler(clsUIHelper.AfterExitEditMode);
            this.txtItemNDC.AfterEnterEditMode += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
            this.txtItemNDC.AfterExitEditMode += new System.EventHandler(clsUIHelper.AfterExitEditMode);
            IsCanceled = true;

            clsUIHelper.setColorSchecme(this);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (Save())
            {
                IsCanceled = false;
                this.Close();
            }
        }

        private void txtBoxs_Validate(object sender, System.EventArgs e)
        {
            try
            {
                if (oPSEItemRow == null)
                    return;
                Infragistics.Win.UltraWinEditors.UltraTextEditor txtEditor = (Infragistics.Win.UltraWinEditors.UltraTextEditor)sender;
                switch (txtEditor.Name)
                {
                    case "txtItemCode":
                        oPSEItemRow.ProductId = txtItemCode.Text;
                        break;
                    case "txtDescription":
                        oPSEItemRow.ProductName = txtDescription.Text;
                        break;
                    case "txtItemNDC":
                        oPSEItemRow.ProductNDC = txtItemNDC.Text;
                        break;
                }
            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }

        private void txtNumericBoxs_Validate(object sender, System.EventArgs e)
        {
            try
            {
                if (oPSEItemRow == null)
                    return;
                Infragistics.Win.UltraWinEditors.UltraNumericEditor txtEditor = (Infragistics.Win.UltraWinEditors.UltraNumericEditor)sender;
                switch (txtEditor.Name)
                {
                    case "numGrams":
                        oPSEItemRow.ProductGrams = POS_Core.Resources.Configuration.convertNullToDecimal(this.numGrams.Value.ToString());
                        break;
                    case "numPillCount":
                        oPSEItemRow.ProductPillCnt = POS_Core.Resources.Configuration.convertNullToInt(this.numPillCount.Value.ToString());
                        break;
                }
            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }

        public void Edit(string ProductId)
        {
            try
            {
                isInEditMode = true;
                txtItemCode.Enabled = false;
                oPSEItemData = oPSEItem.Populate(ProductId);
                oPSEItemRow = oPSEItemData.PSEItem.GetRowByID(ProductId);
                this.Text = "Edit PSE Item Information";
                this.lblTransactionType.Text = "Edit PSE Item Information";
                if (oPSEItemRow != null)
                    Display();
            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }

        private void Display()
        {
            txtItemCode.Text = oPSEItemRow.ProductId;
            txtDescription.Text = oPSEItemRow.ProductName;
            txtItemNDC.Text = oPSEItemRow.ProductNDC;
            numGrams.Value = oPSEItemRow.ProductGrams;
            numPillCount.Value = oPSEItemRow.ProductPillCnt;
            chkIsActive.Checked = oPSEItemRow.IsActive;
        }

        private bool Save()
        {
            try
            {
                //ProductId, ProductName, ProductGrams are mandatory
                if (!isInEditMode)
                {
                    PSEItemData oPSEItemData = oPSEItem.Populate(txtItemCode.Text);

                    if (oPSEItemData != null && oPSEItemData.PSEItem.Rows.Count > 0)
                    {
                        ErrorHandler.throwCustomError(POSErrorENUM.PSE_Items_DuplicateProductId);
                        return false;
                    }
                }
                
                oPSEItemRow.ProductId = txtItemCode.Text;
                oPSEItem.Persist(oPSEItemData);
                return true;
            }
            catch (POSExceptions exp)
            {
                clsUIHelper.ShowErrorMsg(exp.ErrMessage);
                switch (exp.ErrNumber)
                {
                    case (long)POSErrorENUM.PSE_Items_PrimaryKeyVoilation:
                    case (long)POSErrorENUM.PSE_Items_ProductIdCanNotBeNULL:
                        txtItemCode.Focus();
                        break;
                    case (long)POSErrorENUM.PSE_Items_ProductNameCanNotBeNULL:
                        txtDescription.Focus();
                        break;
                    case (long)POSErrorENUM.PSE_Items_ProductGramsCanNotBeNULL:
                        numGrams.Focus();
                        break;
                    case (long)POSErrorENUM.PSE_Items_DuplicateProductId:
                        txtItemCode.Focus();
                        break;
                }
                return false;
            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
                return false;
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            IsCanceled = true;
            this.Close();
        }

        private void chkIsActive_CheckedChanged(object sender, EventArgs e)
        {
            oPSEItemRow.IsActive = chkIsActive.Checked;
        }
    }
}
