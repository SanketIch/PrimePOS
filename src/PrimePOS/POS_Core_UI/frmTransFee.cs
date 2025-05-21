using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using POS_Core.BusinessRules;
using POS_Core.CommonData;
using POS_Core.DataAccess;
using POS_Core.CommonData.Rows;
using NLog;
using POS_Core.Resources;
using POS_Core.ErrorLogging;

namespace POS_Core_UI
{
    public partial class frmTransFee : Form
    {
        #region Declaration
        public bool IsCanceled = false;
        private bool bEdit = false;
        private TransFeeData oTransFeeData;
        private TransFeeRow oTransFeeRow;
        //private CouponRow oTransCouponRow;
        private TransFee oTransFee = new TransFee();
        public string CouponCode = "";
        public decimal DiscountPercent = 0;
        public bool bCalledFromTrans = false;
        private static ILogger logger = LogManager.GetCurrentClassLogger();
        #endregion

        public frmTransFee()
        {
            InitializeComponent();
            PopulatePayTypes();
        }

        private void frmTransFee_Load(object sender, EventArgs e)
        {
            clsUIHelper.setColorSchecme(this);
            txtTransFeeDesc.Focus();
        }

        private void PopulatePayTypes()
        {
            DataTable dt = oTransFee.GetPayTypeData();
            if (dt != null && dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    this.cboPaymentType.Items.Add(dt.Rows[i]["PayTypeID"].ToString().Trim(), dt.Rows[i]["PayTypeDesc"].ToString().Trim());
                }
            }
        }

        public void SetNew()
        {
            try
            {
                bEdit = false;
                oTransFee = new TransFee();
                oTransFeeData = new TransFeeData();
                this.Text = "Add New Transaction Fee";
                this.lblTransactionType.Text = this.Text;
                Clear();
                oTransFeeRow = oTransFeeData.TransFee.AddRow(0, "", 0, false, 0, "", true);
            }
            catch (Exception Ex)
            {
                logger.Fatal(Ex, "SetNew()");
            }
        }

        private void Clear()
        {
            try
            {
                this.txtTransFeeDesc.Text = "";
                osChargeTransFeeFor.Value = 0;
                osTransFeeMode.Value = 0;
                this.numTransFeeValue.Value = 0;
                this.cboPaymentType.SelectedIndex = -1;
            }
            catch (Exception Ex)
            {
                logger.Fatal(Ex, "Clear()");
            }
        }

        public void Edit(Int32 TransFeeID)
        {
            try
            {
                logger.Trace("Edit(Int64 TransFeeID)) - " + clsPOSDBConstants.Log_Entering);
                oTransFeeData = oTransFee.GetTransFeeDataByTransFeeID(TransFeeID);
                oTransFeeRow = (TransFeeRow)oTransFeeData.TransFee.Rows[0];
                bEdit = true;
                if (oTransFeeRow != null)
                    Display();
                logger.Trace("Edit(Int64 TransFeeID) - " + clsPOSDBConstants.Log_Exiting);
            }
            catch (Exception exp)
            {
                logger.Fatal(exp, "Edit(Int64 TransFeeID)");
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }

        private void Display()
        {
            try
            {
                this.txtTransFeeDesc.Text = oTransFeeRow.TransFeeDesc.ToString();
                osChargeTransFeeFor.Value = oTransFeeRow.ChargeTransFeeFor;
                osTransFeeMode.Value = (oTransFeeRow.TransFeeMode == false ? 0 : 1);
                numTransFeeValue.Value = Configuration.convertNullToDecimal(oTransFeeRow.TransFeeValue);
                try
                {
                    if (Configuration.convertNullToString(oTransFeeRow.PayTypeID) == "")
                        this.cboPaymentType.SelectedIndex = -1;
                    else
                    {
                        this.cboPaymentType.Value = oTransFeeRow.PayTypeID;
                    }
                }
                catch
                {
                    this.cboPaymentType.SelectedIndex = -1;
                }
                chkIsActive.Checked = oTransFeeRow.IsActive;
            }
            catch (Exception Ex)
            {
                logger.Fatal(Ex, "Display()");
            }
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            logger.Trace("btnOk_Click(object sender, EventArgs e) - " + clsPOSDBConstants.Log_Entering);
            if (Save())
            {
                IsCanceled = false;
                this.Close();
            }
            else
            {
                IsCanceled = true;
            }
            logger.Trace("btnOk_Click(object sender, EventArgs e) - " + clsPOSDBConstants.Log_Exiting);
        }

        private bool Save()
        {
            try
            {
                if (!ValidateFields()) return false;

                oTransFeeRow.TransFeeDesc = txtTransFeeDesc.Text.Trim();
                oTransFeeRow.ChargeTransFeeFor = Configuration.convertNullToShort(osChargeTransFeeFor.CheckedItem.DataValue);
                oTransFeeRow.TransFeeMode = Configuration.convertNullToBoolean(osTransFeeMode.CheckedItem.DataValue);
                oTransFeeRow.TransFeeValue = Configuration.convertNullToDecimal(numTransFeeValue.Value);
                oTransFeeRow.PayTypeID = Configuration.convertNullToString(this.cboPaymentType.Value);
                oTransFeeRow.IsActive = chkIsActive.Checked;
                oTransFee.Persist(oTransFeeData);
                return true;
            }
            catch (POSExceptions exp)
            {
                clsUIHelper.ShowErrorMsg(exp.ErrMessage);
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
            this.Close();
        }

        private bool ValidateFields()
        {
            bool bReturn = true;
            string errorMessage = "";
            if (this.txtTransFeeDesc.Text.Trim() == string.Empty)
                errorMessage += "Transaction Fee Description should not be blank.";
            if (Configuration.convertNullToDecimal(this.numTransFeeValue.Value.ToString().Trim()) == 0)
                errorMessage += Environment.NewLine + "Transaction Fee Value % or amount should be greater than 0.";
            if (cboPaymentType.SelectedIndex == -1)
                errorMessage += Environment.NewLine + "Please select Payment Type.";

            if (errorMessage != "")
            {
                clsUIHelper.ShowErrorMsg(errorMessage);
                bReturn = false;
            }
            return bReturn;
        }
    }
}
