using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Infragistics.Win.UltraWinGrid;
using Infragistics.Win;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using POS_Core.CommonData;
using POS_Core.BusinessRules;
using POS_Core.ErrorLogging;
using POS_Core.CommonData.Rows;
using Infragistics.Win.UltraWinMaskedEdit;
using Infragistics.Win.CalcEngine;
using Infragistics.Win.UltraWinCalcManager;
//using POS_Core.DataAccess;
using System.Data;
using System.Reflection;
using POS_Core.UserManagement;
using System.Threading;
using PharmData;
using POS_Core.DataAccess;
//using POS_Core_UI.Reports.ReportsUI;
using NLog;
using POS_Core.Resources;

namespace POS_Core_UI
{
    public partial class frmItemsQuickAdd : Form
    {
    
        #region declarations
        string ItemId;
        int TaxID;

        private  frmItems FrmItems;
        private  ItemRow oItemRow;

        private DepartmentData oDepartmentData = new DepartmentData();
        private DepartmentRow oDepartmentRow;
        private Department oBRDepartment = new Department();

        private TaxCodesData oTaxcodeData = new TaxCodesData();
        private TaxCodesRow oTaxCodesRow;
        private TaxCodes oBRTaxCodes = new TaxCodes();

        private ItemData oItemData = new ItemData();
        private Item oBRItem = new Item();
        private static ILogger logger = LogManager.GetCurrentClassLogger();
        #endregion



        public frmItemsQuickAdd()
        {
            
            InitializeComponent();
            SetNew();
        }

        public frmItemsQuickAdd(string itmId) : this()
        {
            ItemId = itmId;
        }

		private void FillTaxInformation()
		{
			DataTable taxCodeDataTable = TaxCodeHelper.GetTaxCodeDataTable();

			cboTaxCodes.DataSource = taxCodeDataTable;
			cboTaxCodes.ValueMember = taxCodeDataTable.Columns[0].ColumnName;
			cboTaxCodes.DisplayMember = taxCodeDataTable.Columns[1].ColumnName;
		}

        private void ultraLabel11_Click(object sender, EventArgs e)
        {

        }

        private void frmItemsQuickAdd_Load(object sender, EventArgs e)
        {
            #region initiateEvents
			cboTaxCodes.Enabled = false;
            txtItemCode.Text = ItemId;

            this.txtDepartmentCode.AfterEnterEditMode += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
            this.txtDepartmentCode.AfterExitEditMode += new System.EventHandler(clsUIHelper.AfterExitEditMode);

            this.txtItemCode.AfterEnterEditMode += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
            this.txtItemCode.AfterExitEditMode += new System.EventHandler(clsUIHelper.AfterExitEditMode);

            this.txtDescription.AfterEnterEditMode += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
            this.txtDescription.AfterExitEditMode += new System.EventHandler(clsUIHelper.AfterExitEditMode);

			this.cboTaxCodes.AfterEnterEditMode += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
			this.cboTaxCodes.AfterExitEditMode += new System.EventHandler(clsUIHelper.AfterExitEditMode);

            this.numSellingPrice.AfterEnterEditMode += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
            this.numSellingPrice.AfterExitEditMode += new System.EventHandler(clsUIHelper.AfterExitEditMode);

            this.numLastCostPrice.AfterEnterEditMode += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
            this.numLastCostPrice.AfterExitEditMode += new System.EventHandler(clsUIHelper.AfterExitEditMode);

            this.txtLocation.AfterEnterEditMode += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
            this.txtLocation.AfterExitEditMode += new System.EventHandler(clsUIHelper.AfterExitEditMode);
            #endregion
			cboTaxCodes.Enabled = false;
            txtItemCode.Text=ItemId;

            #region Fetching Item Descriptions for showing Inteligence to users
            if (Configuration.CInfo.ShowTextPrediction == true)
            {
                this.txtItemDesc.Visible = true;
                this.txtDescription.Visible = false;
                Search oSearch = new Search();
                AutoCompleteStringCollection ItemDescCollection = new AutoCompleteStringCollection();

                ItemDescCollection = oSearch.GetAutoCompleteCollectionData(clsPOSDBConstants.Item_tbl, clsPOSDBConstants.Item_Fld_Description);

                this.txtItemDesc.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
                this.txtItemDesc.AutoCompleteSource = AutoCompleteSource.CustomSource;
                this.txtItemDesc.AutoCompleteCustomSource = ItemDescCollection;
            }
            #endregion

			clsUIHelper.setColorSchecme(this);

        }

        private void txtDepartmentCode_ValueChanged(object sender, EventArgs e)
        {
           // clsUIHelper.setColorSchecme(this);
        }
        

        private void txtItemCode_ValueChanged(object sender, EventArgs e)
        {
            
        }


        private void SetNew()
        {
	        FillTaxInformation();
            oBRItem = new Item();
            oItemData = new ItemData();

            //Clear();
            //Added By Shitaljit(QuicSolv) on 1 July 2011
            //Added last "false" value for isEBTItem field to set it false By default
            //Edited By Shitaljit(QuicSolv) on 10 Nov 2011 make by default item isDiscountable from false to true
            oItemRow = oItemData.Item.AddRow("", 0, "", "", "", "", "", "", 0, 0, 0, 0, false, 0, true, 0, DBNull.Value
                , DBNull.Value, 0, 0, "", 0, 0, 0, null
                , "", "", System.DateTime.MinValue, System.DateTime.MinValue, "", false, false, false, false, true, 0, false, 0, "", true, true, false, 0 
                , 0, 0, 0, 0 ); //Sprint-21 - 2173 06-Jul-2015 JY Added "True" parameter for IsActive   //PRIMEPOS-2592 01-Nov-2018 JY Added "false" for IsNonRefundable //  Added for Solutran : 0, 0, 0, 0  - PRIMEPOS-2663 - NileshJ - 05-July-2019

        }


        private bool Save()
        {
        bool retVal = false;
            try
            {
                logger.Trace("Save() - " + clsPOSDBConstants.Log_Entering);
                bool setDeptFlag = false;
                if (txtDepartmentCode.Text == "")
                {
                    if (Resources.Message.DisplayDefaultNo("DepartmentID For Item # " + oItemRow.ItemID + " Is Not Set Do You Want To Set It?", "Department Not Set", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        txtDepartmentCode.Focus();
                        setDeptFlag = true;
                        return retVal;
                    }
                    else
                    {
                        oItemRow.DepartmentID = Configuration.CInfo.DefaultDeptId;
                    }
                }
                
                ItemPriceValidation oItemPriceValidation = new ItemPriceValidation();
                if (oItemPriceValidation.ValidateItem(oItemData.Item[0], oItemData.Item[0].SellingPrice) == false)
                {
                    throw (new Exception("Current values in item conflicts with validation settings."));
                }
                if (oItemData.Item[0].isOTCItem == true)
                {
                    ItemMonitorCategoryDetail oIMCDetail = new ItemMonitorCategoryDetail();
                    ItemMonitorCategoryDetailData oIMCData = oIMCDetail.Populate(oItemData.Item[0].ItemID);
                    if (oIMCData.Tables[0].Rows.Count == 0)
                    {
                        throw (new Exception("Please select atleast one Item Monitoring Category."));
                    }
                }
                    
                Configuration.UpdatedBy = "M";
                oBRItem.Persist(oItemData);
                    //EnabelButtons();
                retVal = true;
	            PersistSelectedTaxCodes();
                logger.Trace("Save() - " + clsPOSDBConstants.Log_Exiting);
            }
            catch (POSExceptions exp)
            {
                logger.Fatal(exp, "Save()");
                clsUIHelper.ShowErrorMsg(exp.ErrMessage);
                switch (exp.ErrNumber)
                {
                    case (long)POSErrorENUM.Item_DuplicateCode:
                        txtItemCode.Focus();
                        break;
                    case (long)POSErrorENUM.Item_CodeCanNotBeNULL:
                        txtItemCode.Focus();
                        break;
                    case (long)POSErrorENUM.Item_DepartmentCanNotBeNull:
                        txtDepartmentCode.Focus();
                        break;
                    case (long)POSErrorENUM.Item_DescriptionCanNotBeNULL:
                        txtDescription.Focus();
                        break;
                    case (long)POSErrorENUM.Item_TaxCodeCanNotBeNull:
						cboTaxCodes.Focus();
                        break;
                }
            }
            catch (Exception exp)
            {
                logger.Fatal(exp, "Save()");
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
            return retVal;
        }


       
        private void Search(string SenderName)
        {
            string CtrlName ="";
            string Descp="";
            SearchSvr.NameFlag = true;
            //frmSearch FrmSearch=new frmSearch("");
            frmSearchMain FrmSearch = new frmSearchMain(true);  //20-Dec-2017 JY Added new reference

            switch (SenderName)
            { 
                case "txtDepartmentCode":
                    SearchSvr.StrName = txtDepartmentCode.Text;
                    //FrmSearch = new frmSearch(clsPOSDBConstants.Department_tbl);
                    //FrmSearch.txtCode.Text = txtDepartmentCode.Text;
                    FrmSearch = new frmSearchMain(clsPOSDBConstants.Department_tbl, txtDepartmentCode.Text, "", true);    //20-Dec-2017 JY Added new reference
                    FrmSearch.SearchInConstructor = true;
                    CtrlName = "txtDepartmentCode";
                    break;
				// Commented by Shrikant Mali, as because we have added a drop down, we no longer need to search for the tax codes.
				//case "txtTaxCode":
				//	SearchSvr.StrName = txtTaxCode.Text;
				//	FrmSearch = new frmSearch(clsPOSDBConstants.TaxCodes_tbl);
				//	FrmSearch.searchInConstructor = true;
				//	FrmSearch.txtCode.Text = txtTaxCode.Text;
				//	CtrlName = "txtTaxCode";
				//	break;
            }
            FrmSearch.ShowDialog(this);
            if (!FrmSearch.IsCanceled)
            {
                string strCode = FrmSearch.SelectedRowID();
                string strName = FrmSearch.SelectedRowCode;
                switch (CtrlName)
                { 
                    case "txtDepartmentCode" :txtDepartmentCode.Text=strCode;
                        txtDepartmentDescription.Text = strName;
                        break;
					//case "txtTaxCode": txtTaxCode.Text = strCode;
					//	break;
                }

                //if (SenderName == "txtItemCode")
                //    Edit(strCode);
                //else
                //    FKEdit(strCode, SenderName);
            }
            SearchSvr.NameFlag =false;
            SearchSvr.StrName = "";
        }


        private void FillRow()
        {
            try
            {
                oItemRow.ItemID = txtItemCode.Text;
                oItemRow.Description = txtDescription.Text;
                //if (txtDepartmentCode.Text.Trim().Length > 0)
                //{
                //    oItemRow.DepartmentID = Configuration.convertNullToInt(txtDepartmentCode.Text);
                //}
                
                oItemRow.SellingPrice = decimal.Parse(numSellingPrice.Value.ToString());
                oItemRow.LastCostPrice = decimal.Parse(numLastCostPrice.Value.ToString());
                oItemRow.Location = txtLocation.Text;
                if (chkIsTaxable.Checked)
                {
                    oItemRow.isTaxable = true;
                }
                else
                {
                    oItemRow.isTaxable = false;
                    oItemRow.TaxID = 0;
                }
                oItemRow.IsNonRefundable = chkNonRefundable.Checked;    //PRIMEPOS-2592 06-Nov-2018 JY Added 
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "FillRow()");
                clsUIHelper.ShowErrorMsg(ex.Message);
                oItemRow = null;
            }
        }
        private bool CheckValidity()
        {
            string CtrlName;
            CtrlName= Validation("");
            switch (CtrlName)
            { 
                case "txtItemCode":
                    clsUIHelper.ShowErrorMsg("Please Enter ItemID.");
                    txtItemCode.Focus();
                    return false;
                   // break;
                case "txtDescription":
                    clsUIHelper.ShowErrorMsg("Please Enter Description.");
                    txtDescription.Focus();
                    return false;
                    //break;
                case "txtDepartmentCode":
                    clsUIHelper.ShowErrorMsg("Please Enter Department Code.");
                    txtDepartmentCode.Focus();
                    return false;
                    //break;
                case "numSellingPrice":
                    //clsUIHelper.ShowErrorMsg("Please Enter Selling Price.");
                    //    numSellingPrice.Focus();
                    //    return false;
                    #region Sprint-21 - 2204 29-Jun-2015 JY Added to validate selling price if respective settings is off
                    if (Configuration.CInfo.AllowZeroSellingPrice == false) 
                    {
                        clsUIHelper.ShowErrorMsg("Please Enter Selling Price.");
                        numSellingPrice.Focus();
                        return false;
                    }
                    return true;
                    #endregion
                //break;
                case "txtTaxCode":
                    clsUIHelper.ShowErrorMsg("Please Enter Tax Code.");
					cboTaxCodes.Focus();
                    return false;
                //break;
                #region Sprint-19 - 2146 29-Dec-2014 JY Added multiple tax selection validation
                case "txtTaxCode1":
                    clsUIHelper.ShowErrorMsg("You can not select multiple Tax Codes as the respective settings is off.");
                    cboTaxCodes.Focus();
                    return false;
                #endregion
                case "True":
                    return true;
                    ///break;
            }
            return true;
        }

        private string Validation(string CtrlNotValid)
        {
            if (txtItemCode.Text == "")
                return CtrlNotValid = "txtItemCode";
            else if (txtDescription.Text == "")
                return CtrlNotValid = "txtDescription";
            else if (txtDepartmentCode.Text == "")
                return CtrlNotValid = "txtDepartmentCode";
            else if (double.Parse(numSellingPrice.Value.ToString()) ==0.0)
                return CtrlNotValid = "numSellingPrice";
            #region Sprint-19 - 2146 29-Dec-2014 JY Added multiple tax selection validation
            else if (chkIsTaxable.Checked && cboTaxCodes.CheckedItems.Count == 0)
                return CtrlNotValid = "txtTaxCode";
            else if (chkIsTaxable.Checked && !Configuration.CPOSSet.SelectMultipleTaxes && cboTaxCodes.CheckedItems.Count > 1)
                return CtrlNotValid = "txtTaxCode1";
            #endregion
            else return CtrlNotValid = "True";                
        }


        private void btnOk_Click(object sender, EventArgs e)
        {
            try
            {
                logger.Trace("btnOk_Click(object sender, EventArgs e) - " + clsPOSDBConstants.Log_Entering);
                if (Configuration.CInfo.ShowTextPrediction == true)
                {
                    this.txtDescription.Text = this.txtItemDesc.Text.Trim();//Added By shitaljit on 10 aug 2012
                }
                if (CheckValidity())
                {
                    FillRow();
                    if (Save())
                    {
                        clsUIHelper.ShowOKMsg("Item is Succesfully Added in the System");
                        //frmPOSTransaction FrmPOSTrans = new frmPOSTransaction();
                        this.Close();
                    }
                    else
                        clsUIHelper.ShowOKMsg("Item was not Added in the System");
                }
                logger.Trace("btnOk_Click(object sender, EventArgs e) - " + clsPOSDBConstants.Log_Exiting);
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "btnOk_Click(object sender, EventArgs e)");
                clsUIHelper.ShowErrorMsg(ex.Message);
            }
        }

        private void txtDepartmentCode_MouseHover(object sender, EventArgs e)
        {
        }

        private void frmItemsQuickAdd_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.O)
                btnOk_Click(null, null);
            else if (e.KeyData == Keys.C)
                btnClose_Click(null, null);

        }

        private void frmItemsQuickAdd_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                if(e.KeyData==Keys.F2)
                    btnOk_Click(null, null);
                else if(e.KeyData==Keys.F5)
                    btnClose_Click(null, null);


                if (e.KeyData == System.Windows.Forms.Keys.F4)
                {
                    if (txtDepartmentCode.ContainsFocus)
                        this.Search(txtDepartmentCode.Name);
					//else if (cboTaxCodes.ContainsFocus)
					//	this.Search(cboTaxCodes.Name);
                }

            }
            catch (Exception exp)
            {
                logger.Fatal(exp, "frmItemsQuickAdd_KeyUp(object sender, KeyEventArgs e)");
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }

        private void txtDepartmentCode_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyData == System.Windows.Forms.Keys.F4)
                {
                    if (txtDepartmentCode.ContainsFocus)
                        this.Search(txtDepartmentCode.Name);
					//else if (txtTaxCode.ContainsFocus)
					//	this.Search(txtTaxCode.Name);
                }

            }
            catch (Exception exp)
            {
                logger.Fatal(exp, "txtDepartmentCode_KeyUp(object sender, KeyEventArgs e)");
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtTaxCode_KeyUp(object sender, KeyEventArgs e)
        {

            try
            {
                if (e.KeyData == System.Windows.Forms.Keys.F4)
                {
					//if (txtTaxCode.ContainsFocus)
					//	this.Search(txtTaxCode.Name);
                }

            }
            catch (Exception exp)
            {
                logger.Fatal(exp, "txtTaxCode_KeyUp(object sender, KeyEventArgs e)");
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }

        private void chkIsTaxable_CheckedChanged(object sender, EventArgs e)
        {
            bool IsChecked = chkIsTaxable.Checked;
            cboTaxCodes.Enabled = chkIsTaxable.Checked;
            if (!IsChecked)
            {
				//cboTaxCodes.Text = "";
				//txtTaxDescription.Text = "";
                if (oItemRow != null)
                {
                    oItemRow.TaxID = 0;
                    oItemRow.isTaxable = false;
                }
            }
        }

        private void txtDepartmentCode_EditorButtonClick(object sender, Infragistics.Win.UltraWinEditors.EditorButtonEventArgs e)
        {
           // if (txtDepartmentCode.ContainsFocus)
                this.Search(txtDepartmentCode.Name);
        }

        private void txtTaxCode_EditorButtonClick(object sender, Infragistics.Win.UltraWinEditors.EditorButtonEventArgs e)
        {
             //if (txtTaxCode.ContainsFocus)
				//this.Search(txtTaxCode.Name);
           
        }

        private void txtboxes_Fillinfo(object Sender, EventArgs e)
        {
            try
            {
                if (txtDepartmentCode.Text != "" && ((Infragistics.Win.UltraWinEditors.UltraTextEditor)Sender).Name == "txtDepartmentCode")
                {
                    oDepartmentData = oBRDepartment.Populate(txtDepartmentCode.Text);
                    if (oDepartmentData.Department.Count != 0)
                    {
                        oDepartmentRow = oDepartmentData.Department[0];
                    }
                    else
                    {
                        //clsUIHelper.ShowErrorMsg("Improper Value Selected");
                        txtDepartmentCode.Focus();
                    }

                    if (oDepartmentData.Department.Rows.Count > 0)
                    {
                        oItemRow.DepartmentID = oDepartmentRow.DeptID;
                        txtDepartmentDescription.Text = oDepartmentRow.DeptName;
                    }
                    else
                    {
                        if((Resources.Message.Display("This Deparment does not Exist. Do you want to Add this Deparment?","Add Department",MessageBoxButtons.YesNo,MessageBoxIcon.Warning)==DialogResult.Yes))
                        {
                            frmDepartment oFrmDept = new frmDepartment(txtDepartmentCode.Text);
                            oFrmDept.ShowDialog();
                        }

                    }
                }

				if (chkIsTaxable.Checked && cboTaxCodes.CheckedItems.Count != 0 && ((Infragistics.Win.UltraWinEditors.UltraTextEditor)Sender).Name == "cboTaxCodes")
                {
					oTaxcodeData = oBRTaxCodes.Populate(cboTaxCodes.Text);
                    if (oTaxcodeData.TaxCodes.Count != 0)
                    {
                        oTaxCodesRow = oTaxcodeData.TaxCodes[0];
                    }
                    else
                    {
                        //clsUIHelper.ShowErrorMsg("Improper Value Selected");
                        cboTaxCodes.Focus();
                    }
                    if (oTaxcodeData.TaxCodes.Rows.Count > 0)
                    {
						//oItemRow.TaxID = oTaxCodesRow.TaxID;
						//txtTaxDescription.Text = oTaxCodesRow.Description;

                    }
                    else
                        clsUIHelper.ShowWarningMsg("No Tax Code Selected.");
                }
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "txtboxes_Fillinfo(object Sender, EventArgs e)");
            }

        }

		private void PersistSelectedTaxCodes()
		{
			List<int> selectedTaxCodes = cboTaxCodes.CheckedItems.Select(checkedItem => int.Parse(checkedItem.DataValue.ToString())).ToList();

			TaxCodeHelper.PersistItemTaxCodes(selectedTaxCodes, oItemData.Item[0].ItemID.ToString(CultureInfo.InvariantCulture));
		}

        private void txt_EnterClick(object sender, KeyEventArgs e)
        {
            switch (e.KeyData)
            {
                case Keys.Enter:
                    if (txtItemCode.ContainsFocus)
                        this.SelectNextControl(this.ActiveControl, true, true, true, true);
                    else if (txtItemDesc.ContainsFocus)
                        this.SelectNextControl(this.ActiveControl, true, true, true, true);
                    else if (txtDescription.ContainsFocus)
                        this.SelectNextControl(this.ActiveControl, true, true, true, true);
                    else if (txtDepartmentCode.ContainsFocus)
                        this.SelectNextControl(this.ActiveControl, true, true, true, true);
                    else if (numSellingPrice.ContainsFocus)
                        this.SelectNextControl(this.ActiveControl, true, true, true, true);
                    else if (numLastCostPrice.ContainsFocus)
                        this.SelectNextControl(this.ActiveControl, true, true, true, true);
                    else if (cboTaxCodes.ContainsFocus)
                        this.SelectNextControl(this.ActiveControl, true, true, true, true);
                    else if (chkIsTaxable.ContainsFocus)
                        this.SelectNextControl(this.ActiveControl, true, true, true, true);
                    else if (chkNonRefundable.ContainsFocus)    //PRIMEPOS-2592 06-Nov-2018 JY Added 
                        this.SelectNextControl(this.ActiveControl, true, true, true, true);
                    else btnOk.Focus();
                    break;

                case Keys.F2:
                    btnOk_Click(null, null);
                    break;
                case Keys.F5:
                    this.Close();
                    break;
                //case Keys.F4:
                //    if (txtDepartmentCode.ContainsFocus)
                //        this.Search(txtDepartmentCode.Name);
                //    else if (txtTaxCode.ContainsFocus)
                //        this.Search(txtTaxCode.Name);
                //    else if (txtDepartmentDescription.ContainsFocus)
                //        this.Search(txtDescription.Name);
                //    break;
                case Keys.Escape:
                    this.Close();
                    break;
            }
        }

        private void txtItemDesc_Enter(object sender, EventArgs e)
        {
            clsUIHelper.AfterEnterEditMode(sender, e);
        }

        private void txtItemDesc_Leave(object sender, EventArgs e)
        {
            clsUIHelper.AfterExitEditMode(sender, e);
        }

		private void cboTaxCodes_TextChanged(object sender, EventArgs e)
		{
			cboTaxCodes.Text = TaxCodeHelper.GetTrimmedTaxCodes(cboTaxCodes.CheckedItems);
		}

		private void cboTaxCodes_AfterCloseUp(object sender, EventArgs e)
		{
			cboTaxCodes.Text = TaxCodeHelper.GetTrimmedTaxCodes(cboTaxCodes.CheckedItems);

            #region Sprint-19 - 2146 29-Dec-2014 JY Added multiple tax selection validation
            if (chkIsTaxable.Checked)
            {
                if (cboTaxCodes.CheckedItems.Count == 0)
                {
                    clsUIHelper.ShowErrorMsg("\nPlease select Tax Code(s).");
                }
                else if (!Configuration.CPOSSet.SelectMultipleTaxes && cboTaxCodes.CheckedItems.Count > 1)
                {
                    clsUIHelper.ShowErrorMsg("\nYou can not select multiple Tax Codes as the respective settings is off.");
                }
            }
            #endregion
		}

        #region PRIMEPOS-2592 06-Nov-2018 JY Added 
        private void chkNonRefundable_CheckedChanged(object sender, EventArgs e)
        {
            oItemRow.IsNonRefundable = chkNonRefundable.Checked;
        }
        #endregion
    }
}