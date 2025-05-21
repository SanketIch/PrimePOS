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
//using POS_Core.DataAccess;

using NLog;
using POS_Core.Resources;

namespace POS_Core_UI
{
    public partial class frmPOSCoupon : Form
    {
        #region Declaration
        public bool IsCanceled = false;
        private bool IsEditing = false;
        private CouponData oCouponData;
        private CouponRow oCouponRow;
        private CouponRow oTransCouponRow;
        private Coupon oCoupon = new Coupon();
        public string CouponCode = "";
        public decimal DiscountPercent = 0;
        public bool bCalledFromTrans = false;   //Sprint-23 - PRIMEPOS-2280 05-May-2016 JY Added to know whether form called from transaction screen
        private static ILogger logger = LogManager.GetCurrentClassLogger();
        #endregion

        public frmPOSCoupon()
        {
           
            InitializeComponent();
            
        }

        public CouponRow TransCouponRow
        {
            get
            {
                return oCouponRow;
            }
        }
        private bool Save()
        {
            try
            {
                logger.Trace("Save() - " + clsPOSDBConstants.Log_Entering);

                string errorMessage = "";
                if (this.txtCouponCode.Text.Trim() == string.Empty)
                { 
                errorMessage+="Coupon Code can not be Null.";                
                }
                if (this.txtDesc.Text.Trim() == string.Empty) //Sprint-23 - PRIMEPOS-2279 17-Mar-2016 JY Added
                {
                    errorMessage += Environment.NewLine + "Coupon Description can not be Null.";
                }
                if (Configuration.convertNullToDecimal(this.txtDiscountPerc.Value.ToString().Trim()) <= 0)
                {
                    errorMessage += Environment.NewLine + "Discount % should be greater than 0."; 
                }
                if (errorMessage != "")
                {
                    clsUIHelper.ShowErrorMsg(errorMessage);
                    return false;
                }
                if (DateTime.TryParse(this.dtpSaleStartDate.Text, out sDate))
                {
                    oCouponRow.StartDate = sDate;
                }
                else
                {
                    oCouponRow.StartDate = DateTime.Now;
                }
                if (DateTime.TryParse(this.dtpSaleEndDate.Text, out eDate))
                {
                    oCouponRow.EndDate = eDate;
                }
                else
                {
                    oCouponRow.EndDate = DateTime.Now;
                }
                oCouponRow.DiscountPerc = Configuration.convertNullToDecimal(this.txtDiscountPerc.Value.ToString().Trim());
                oCouponRow.IsCouponInPercent = this.chkIsDiscInPercent.Checked;
                oCouponRow.CouponDesc = this.txtDesc.Text.Trim();   //Sprint-23 - PRIMEPOS-2279 08-Aug-2016 JY Added to update description

                if (!IsEditing)
                {
                    oCouponRow.CouponCode = this.txtCouponCode.Text.Trim();
                    oCouponRow.CouponDesc = this.txtDesc.Text.Trim();   //Sprint-23 - PRIMEPOS-2279 17-Mar-2016 JY Added to insert description
                    oCouponRow.UserID = Configuration.UserName.Trim();     
                    oCouponRow.CreatedDate = DateTime.Now;
                    CouponSvr oCouponSvr = new CouponSvr();
                    CouponData oTempCouponData = new CouponData();
                    oTempCouponData = oCouponSvr.PopulateList(" Where " + clsPOSDBConstants.Coupon_Fld_CouponCode + " = '" + oCouponRow.CouponCode + "'");
                    if (oTempCouponData.Tables[0].Rows.Count > 0)
                    {
                        clsUIHelper.ShowErrorMsg("Coupon Code :" + oCouponRow.CouponCode + " already exist ");
                        return false;
                    }
                    else
                    {
                        //oCouponData.Coupon.ImportRow(oCouponRow);                        
                        oCoupon.Persist(oCouponData);
                        logger.Trace("Save() - " + clsPOSDBConstants.Log_Exiting);
                        GetCouponByCode();  //PRIMEPOS-3048 11-Jan-2022 JY Added
                        return true;
                    }
                }
                else
                {                    
                    oCoupon.Persist(oCouponData);
                    logger.Trace("Save() - " + clsPOSDBConstants.Log_Exiting);
                    return true;
                }
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "Save()");
                clsUIHelper.ShowErrorMsg(ex.Message);
                return false;
            }
        }
        public void EnableDisableControls(bool bStatus) //Sprint-23 - PRIMEPOS-2280 05-May-2016 JY Added bStatus flag to enable/disable controls
        {
            this.txtDiscountPerc.Enabled = bStatus;
            this.dtpSaleEndDate.Enabled = bStatus;
            this.dtpSaleStartDate.Enabled = bStatus;
            this.chkIsDiscInPercent.Enabled = bStatus;
            this.txtDesc.Enabled = bStatus;   //Sprint-23 - PRIMEPOS-2279 19-Mar-2016 JY Added
        }
        private void Display()
        {
            try
            {
                this.txtCouponID.Text = oCouponRow.CouponID.ToString();     //PRIMEPOS-2034 05-Mar-2018 JY Added
                this.txtCouponCode.Text = oCouponRow.CouponCode;
                this.txtDesc.Text = oCouponRow.CouponDesc;  //Sprint-23 - PRIMEPOS-2279 17-Mar-2016 JY Added
                this.txtDiscountPerc.Value = Configuration.convertNullToDecimal(oCouponRow.DiscountPerc);
                this.dtpSaleEndDate.Value = oCouponRow.EndDate.ToShortDateString();
                this.dtpSaleStartDate.Value = oCouponRow.StartDate.ToShortDateString();
                this.chkIsDiscInPercent.Checked = oCouponRow.IsCouponInPercent;
            }
            catch (Exception Ex)
            {
                logger.Fatal(Ex, "Display()");
            }
        }
        
        //Sprint-22 11-Dec-2015 JY Added return type
        private bool GetCouponByCode()
        {
            bool bStatus = false; 
            try
            {
                if (string.IsNullOrEmpty(txtCouponCode.Text) == false)
                {
                    oCouponData = oCoupon.Populate(this.txtCouponCode.Text.Trim());
                    //CouponRow oCPRow = oCouponData.Coupon.GetRow(iID);
                    if (!Configuration.isNullOrEmptyDataSet(oCouponData))
                    {
                        bStatus = true; 
                        oCouponRow = (CouponRow)oCouponData.Coupon.Rows[0];
                        Display();
                        if (this.btnSave.Text.Equals("&OK") == true)
                        {
                            DateTime.TryParse(this.dtpSaleEndDate.Text, out eDate);
                            DateTime currentdate = Convert.ToDateTime(DateTime.Now.ToShortDateString());
                            if (eDate < currentdate)
                            {
                                IsCanceled = true;
                                clsUIHelper.ShowErrorMsg("Coupon is expired.");
                                this.txtCouponCode.Focus();
                            }
                            //btnSave.Enabled = true;   //Sprint-22 11-Dec-2015 JY Commented
                        }
                    }
                    else
                    {
                        clsUIHelper.ShowErrorMsg("Invalid coupon code.");
                        this.txtCouponCode.Focus();
                    }
                }
            }
            catch (Exception Ex)
            {
                logger.Fatal(Ex, "GetCouponByCode()");
                clsUIHelper.ShowErrorMsg(Ex.Message);
            }
            return bStatus; 
        }

        public void Edit(Int64 iID)
        {
            try
            {
                logger.Trace("Edit(Int32 iID) - " + clsPOSDBConstants.Log_Entering);
                oCouponData = oCoupon.Populate(iID);
                oCouponRow =(CouponRow)oCouponData.Coupon.Rows[0];
                IsEditing = true;
                if (oCouponRow != null)
                    Display();
                logger.Trace("Edit(Int32 iID) - " + clsPOSDBConstants.Log_Exiting);
            }
            catch (Exception exp)
            {
                logger.Fatal(exp, "Edit(Int32 iID)");
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }

        public void SetNew()
        {
            try
            {
                IsEditing = false;
                oCoupon = new Coupon();
                oCouponData = new CouponData();
                this.Text = "Add New Coupon";
                this.lblTransactionType.Text = this.Text;
                Clear();
                oCouponRow = oCouponData.Coupon.AddRow(0, "", "", DateTime.Now, false, "");   //Sprint-23 - PRIMEPOS-2279 17-Mar-2016 JY Added
            }
            catch(Exception Ex)
            {
                logger.Fatal(Ex, "SetNew()");
            }
        }

        private void Clear()
        {
            try
            {
                this.txtCouponCode.Text = "";
                this.txtDesc.Text = ""; //Sprint-23 - PRIMEPOS-2279 17-Mar-2016 JY Added
                this.txtDiscountPerc.Value = 0;
                this.dtpSaleStartDate.Text = DateTime.Now.ToShortDateString();
                this.dtpSaleEndDate.Text = DateTime.Now.ToShortDateString();
            }
            catch (Exception Ex) 
            {
                logger.Fatal(Ex, "Clear()");
            }
        }

        private void btnNew_Click(object sender, System.EventArgs e)
        {
            try
            {
                this.txtCouponCode.Text = "";
                this.txtDesc.Text = ""; //Sprint-23 - PRIMEPOS-2279 17-Mar-2016 JY Added
                SetNew();
            }
            catch (Exception exp)
            {
                logger.Fatal(exp, "btnNew_Click(object sender, System.EventArgs e)");
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }
        DateTime sDate, eDate;
        private bool CheckDate()
        {
            try
            {
                
                DateTime.TryParse(this.dtpSaleStartDate.Text, out sDate);

                DateTime.TryParse(this.dtpSaleEndDate.Text, out eDate);
                if (sDate > eDate)
                    return false;
                else
                {
                    return true;
                }
            }
            catch { return false; }
        
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            logger.Trace("btnSave_Click(object sender, EventArgs e) - " + clsPOSDBConstants.Log_Entering);
            if (!this.btnSave.Text.ToUpper().Contains("OK".ToUpper()))
            {
                if (CheckDate() && Save())
                {
                    IsCanceled = false;
                    this.DialogResult = DialogResult.OK;    //Sprint-23 - PRIMEPOS-2280 06-May-2016 JY Added 
                    DiscountPercent = Configuration.convertNullToDecimal(this.txtDiscountPerc.Value.ToString());    //Sprint-23 - PRIMEPOS-2280 06-May-2016 JY Added 
                    this.Close();
                }
                else if (!CheckDate())
                {
                    IsCanceled = true;
                    clsUIHelper.ShowErrorMsg("Invalid date range");
                }
            }
            else
            {
                #region Sprint-22 11-Dec-2015 JY Added
                if (this.txtCouponCode.Text.Trim() == string.Empty)
                {
                    clsUIHelper.ShowErrorMsg("Coupon Code can not be Null");
                    if (this.txtCouponCode.Enabled) this.txtCouponCode.Focus();
                    return;
                }
                else
                {
                    if (!GetCouponByCode()) return;
                }
                #endregion

                DateTime currentdate = Convert.ToDateTime(DateTime.Now.ToShortDateString());
                if (CheckDate() && eDate >= currentdate)
                {
                    IsCanceled = false;
                    this.DialogResult = DialogResult.OK;
                    CouponCode = this.txtCouponCode.Text.Trim();
                    DiscountPercent = Configuration.convertNullToDecimal(this.txtDiscountPerc.Value.ToString());
                    oTransCouponRow = oCouponRow;
                    IsCanceled = false;
                    this.Close();
                }
                else if (eDate < currentdate)
                {
                    IsCanceled = true;
                    clsUIHelper.ShowErrorMsg("This coupon is Expired.");
                }
            }
            logger.Trace("btnSave_Click(object sender, EventArgs e) - " + clsPOSDBConstants.Log_Exiting);
        }

        private void frmPOSCoupon_Load(object sender, EventArgs e)
        {
            this.txtCouponCode.AfterEnterEditMode += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
            this.txtDesc.AfterEnterEditMode += new System.EventHandler(clsUIHelper.AfterEnterEditMode); //Sprint-23 - PRIMEPOS-2279 18-Mar-2016 JY Added
            this.txtDiscountPerc.AfterExitEditMode += new System.EventHandler(clsUIHelper.AfterExitEditMode);
            this.lblTransactionType.Text = "Coupon";
            if (!IsEditing)
            {
                SetNew();
                //this.btnSave.Enabled = false; //Sprint-22 11-Dec-2015 JY Commented
            }
            clsUIHelper.setColorSchecme(this);
            this.txtCouponCode.Focus();

            //Sprint-23 - PRIMEPOS-2280 05-May-2016 JY Added 
            if ((bCalledFromTrans == true) && (UserPriviliges.IsUserHasPriviliges(UserPriviliges.Modules.POSTransaction.ID, UserPriviliges.Screens.POSTransaction.ID, UserPriviliges.Permissions.CreateNewCoupon.ID) == true))

                btnAddNewCoupon.Visible = true;   
        }

        private void frmPOSCoupon_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.KeyCode == Keys.Escape)
            {
                IsCanceled = true;
                this.Close();
            }
        }

        private void txtCouponCode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter )
            {
                if (IsEditing ||!this.dtpSaleEndDate.Enabled)
                {
                    GetCouponByCode();
                }
                //Sprint-22 11-Dec-2015 JY Commented below code
                //if(btnSave.Text.ToUpper() == "&SAVE")
                //{
                //    btnSave.Enabled = true;
                //}
                this.SelectNextControl(this.ActiveControl, true, true, true, true);
                e.Handled = true;
            }
        }

        private void txtDiscountPerc_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                this.SelectNextControl(this.ActiveControl, true, true, true, true);
                e.Handled = true;
            }
        }

        //Sprint-23 - PRIMEPOS-2280 05-May-2016 JY Added to "Add new coupon from transaction screen"
        private void btnAddNewCoupon_Click(object sender, EventArgs e)
        {
            logger.Trace("btnAddNewCoupon_Click(object sender, EventArgs e) - " + clsPOSDBConstants.Log_Entering);
            EnableDisableControls(true);
            SetNew();
            btnSave.Text = "&Save";
            this.Text = lblTransactionType.Text = "Add New Coupon";
            logger.Trace("btnAddNewCoupon_Click(object sender, EventArgs e) - " + clsPOSDBConstants.Log_Exiting);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
